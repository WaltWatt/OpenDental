using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using System.Diagnostics;

namespace OpenDental {
	public partial class FormPaySimpleSetup:ODForm {

		private Program _progCur;
		///<summary>Local cache of all of the ClinicNums the current user has permission to access at the time the form loads.  Filled at the same time
		///as comboClinic and is used to set programproperty.ClinicNum when saving.</summary>
		private List<long> _listUserClinicNums;
		///<summary>List of PaySimple program properties for all clinics.
		///Includes properties with ClinicNum=0, the headquarters props or props not assigned to a clinic.</summary>
		private List<ProgramProperty> _listProgProps;

		///<summary>Used to revert the slected index in the clinic drop down box if the user tries to change clinics
		///and the payment type has not been set.</summary>
		private int _indexClinicRevert;
		private List<Def> _listPaymentTypeDefs;

		public FormPaySimpleSetup() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormPaySimpleSetup_Load(object sender,EventArgs e) {
			_progCur=Programs.GetCur(ProgramName.PaySimple);
			if(_progCur==null) {
				MsgBox.Show(this,"The PaySimple entry is missing from the database.");//should never happen
				return;
			}
			checkEnabled.Checked=_progCur.Enabled;
			if(!PrefC.HasClinicsEnabled) {//clinics are not enabled, use ClinicNum 0 to indicate 'Headquarters' or practice level program properties
				checkEnabled.Text=Lan.g(this,"Enabled");
				groupPaySettings.Text=Lan.g(this,"Payment Settings");
				comboClinic.Visible=false;
				labelClinic.Visible=false;
				labelClinicEnable.Visible=false;
				_listUserClinicNums=new List<long>() { 0 };//if clinics are disabled, programproperty.ClinicNum will be set to 0
			}
			else {//Using clinics
				groupPaySettings.Text=Lan.g(this,"Clinic Payment Settings");
				_listUserClinicNums=new List<long>();
				comboClinic.Items.Clear();
				//if PaySimple is enabled and the user is restricted to a clinic, don't allow the user to disable for all clinics
				if(Security.CurUser.ClinicIsRestricted) {
					if(checkEnabled.Checked) {
						checkEnabled.Enabled=false;
					}
				}
				else {
					comboClinic.Items.Add(Lan.g(this,"Headquarters"));
					//this way both lists have the same number of items in it and if 'Headquarters' is selected the programproperty.ClinicNum will be set to 0
					_listUserClinicNums.Add(0);
					comboClinic.SelectedIndex=0;
				}
				List<Clinic> listClinics=Clinics.GetForUserod(Security.CurUser);
				for(int i=0;i<listClinics.Count;i++) {
					comboClinic.Items.Add(listClinics[i].Abbr);
					_listUserClinicNums.Add(listClinics[i].ClinicNum);
					if(Clinics.ClinicNum==listClinics[i].ClinicNum) {
						comboClinic.SelectedIndex=i;
						if(!Security.CurUser.ClinicIsRestricted) {
							comboClinic.SelectedIndex++;//increment the SelectedIndex to account for 'Headquarters' in the list at position 0 if the user is not restricted.
						}
					}
				}
				_indexClinicRevert=comboClinic.SelectedIndex;
			}
			_listProgProps=ProgramProperties.GetForProgram(_progCur.ProgramNum);
			if(PrefC.HasClinicsEnabled) {
				foreach(Clinic clinicCur in Clinics.GetForUserod(Security.CurUser)) {
					AddNeededProgramProperties(clinicCur.ClinicNum);
				}
			}
			FillFields();
		}

		private void AddNeededProgramProperties(long clinicNum) {
			if(!_listProgProps.Any(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiUserName)) {
				_listProgProps.Add(new ProgramProperty() {
					ClinicNum=clinicNum,
					PropertyDesc=PaySimple.PropertyDescs.PaySimpleApiUserName,
					ProgramNum=_progCur.ProgramNum,
					PropertyValue=_listProgProps.FirstOrDefault(x => x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiUserName && x.ClinicNum==0).PropertyValue,
				});
			}
			if(!_listProgProps.Any(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiKey)) {
				_listProgProps.Add(new ProgramProperty() {
					ClinicNum=clinicNum,
					PropertyDesc=PaySimple.PropertyDescs.PaySimpleApiKey,
					ProgramNum=_progCur.ProgramNum,
					PropertyValue=_listProgProps.FirstOrDefault(x => x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiKey && x.ClinicNum==0).PropertyValue,
				});
			}
			if(!_listProgProps.Any(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimplePayType)) {
				_listProgProps.Add(new ProgramProperty() {
					ClinicNum=clinicNum,
					PropertyDesc=PaySimple.PropertyDescs.PaySimplePayType,
					ProgramNum=_progCur.ProgramNum,
					PropertyValue=_listProgProps.FirstOrDefault(x => x.PropertyDesc==PaySimple.PropertyDescs.PaySimplePayType && x.ClinicNum==0).PropertyValue,
				});
			}
		}

		private void FillFields() {
			long clinicNum=0;
			if(PrefC.HasClinicsEnabled) {
				clinicNum=_listUserClinicNums[comboClinic.SelectedIndex];
			}
			textUsername.Text=ProgramProperties.GetPropValFromList(_listProgProps,PaySimple.PropertyDescs.PaySimpleApiUserName,clinicNum);
			textKey.Text=ProgramProperties.GetPropValFromList(_listProgProps,PaySimple.PropertyDescs.PaySimpleApiKey,clinicNum);
			string payTypeDefNum=ProgramProperties.GetPropValFromList(_listProgProps,PaySimple.PropertyDescs.PaySimplePayType,clinicNum);
			comboPaymentType.Items.Clear();
			_listPaymentTypeDefs=Defs.GetDefsForCategory(DefCat.PaymentTypes,true);
			for(int i=0;i<_listPaymentTypeDefs.Count;i++) {
				comboPaymentType.Items.Add(_listPaymentTypeDefs[i].ItemName);
				if(_listPaymentTypeDefs[i].DefNum.ToString()==payTypeDefNum) {
					comboPaymentType.SelectedIndex=i;
				}
			}
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboClinic.SelectedIndex==_indexClinicRevert) {//didn't change the selected clinic
				return;
			}
			//if PaySimple is enabled and the username and key are set for the current clinic,
			//make the user select a payment type before switching clinics
			if(checkEnabled.Checked && !IsClinicCurSetupDone()) {
				comboClinic.SelectedIndex=_indexClinicRevert;
				MsgBox.Show(this,"Please select a username, key, and/or payment type first.");
				return;
			}
			SynchWithHQ();//if the user just modified the HQ credentials, change any credentials that were the same as HQ to keep them synched
			//set the values in the list for the clinic we are switching from, at _clinicIndexRevert
			_listProgProps.FindAll(x => x.ClinicNum==_listUserClinicNums[_indexClinicRevert] && x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiUserName)
				.ForEach(x => x.PropertyValue=textUsername.Text);//always 1 item; null safe
			_listProgProps.FindAll(x => x.ClinicNum==_listUserClinicNums[_indexClinicRevert] && x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiKey)
				.ForEach(x => x.PropertyValue=textKey.Text);//always 1 item; null safe
			_listProgProps.FindAll(x => x.ClinicNum==_listUserClinicNums[_indexClinicRevert] 
					&& x.PropertyDesc==PaySimple.PropertyDescs.PaySimplePayType 
					&& comboPaymentType.SelectedIndex>-1)
				.ForEach(x => x.PropertyValue=_listPaymentTypeDefs[comboPaymentType.SelectedIndex].DefNum.ToString());//always 1 item; null safe
			_indexClinicRevert=comboClinic.SelectedIndex;//now that we've updated the values for the clinic we're switching from, update _indexClinicRevert
			FillFields();
		}

		private bool IsClinicCurSetupDone() {
			//If nothing is set, they are OK to change clinics and save.
			if(string.IsNullOrWhiteSpace(textUsername.Text) && string.IsNullOrWhiteSpace(textKey.Text)){
				return true;
			}
			//If everything is set, they are OK to change clinics and save.
			if(!string.IsNullOrWhiteSpace(textUsername.Text) && !string.IsNullOrWhiteSpace(textKey.Text) && comboPaymentType.SelectedIndex>=0) {
				return true;
			}
			return false;
		}

		///<summary>For each clinic, if the Username and Key are the same as the HQ (ClinicNum=0) Username and Key, update the clinic with the
		///values in the text boxes.  Only modifies other clinics if _indexClinicRevert=0, meaning user just modified the HQ clinic credentials.</summary>
		private void SynchWithHQ() {
			if(!PrefC.HasClinicsEnabled || _listUserClinicNums[_indexClinicRevert]>0) {//using clinics, and modifying the HQ clinic. otherwise return.
				return;
			}
			string hqUsername=ProgramProperties.GetPropValFromList(_listProgProps,PaySimple.PropertyDescs.PaySimpleApiUserName,0);//HQ Username before updating to value in textbox
			string hqKey=ProgramProperties.GetPropValFromList(_listProgProps,PaySimple.PropertyDescs.PaySimpleApiKey,0);//HQ Key before updating to value in textbox
			string hqPayType=ProgramProperties.GetPropValFromList(_listProgProps,PaySimple.PropertyDescs.PaySimplePayType,0);//HQ PaymentType before updating to combo box selection
			string payTypeCur="";
			if(comboPaymentType.SelectedIndex>-1) {
				payTypeCur=_listPaymentTypeDefs[comboPaymentType.SelectedIndex].DefNum.ToString();
			}
			//for each distinct ClinicNum in the prog property list for PaySimple except HQ
			foreach(long clinicNum in _listProgProps.Select(x => x.ClinicNum).Where(x => x>0).Distinct()) {
				//if this clinic has a different username or key, skip it
				if(!_listProgProps.Exists(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiUserName && x.PropertyValue==hqUsername)
					|| !_listProgProps.Exists(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiKey && x.PropertyValue==hqKey)) {
					continue;
				}
				//this clinic had a matching username and key, so update the username and key to keep it synched with HQ
				_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiUserName)
					.ForEach(x => x.PropertyValue=textUsername.Text);//always 1 item; null safe
				_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiKey)
					.ForEach(x => x.PropertyValue=textKey.Text);//always 1 item; null safe
				if(string.IsNullOrEmpty(payTypeCur)) {
					continue;
				}
				//update clinic payment type if it originally matched HQ's payment type and the selected payment type is valid
				_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimplePayType && x.PropertyValue==hqPayType)
					.ForEach(x => x.PropertyValue=payTypeCur);//always 1 item; null safe
			}
		}

		private void linkLabel1_LinkClicked(object sender,LinkLabelLinkClickedEventArgs e) {
			Process.Start("http://www.paysimple.com");
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			#region Validation
			//if clinics are not enabled and the PaySimple program link is enabled, make sure there is a username and key set
			//if clinics are enabled, the program link can be enabled with blank username and/or key fields for some clinics
			//clinics with blank username and/or key will essentially not have PaySimple enabled
			if(checkEnabled.Checked && !IsClinicCurSetupDone()) {//Also works for offices not using clinics
				MsgBox.Show(this,"Please enter a username, key, and/or payment type first.");
				return;
			}
			SynchWithHQ();//if the user changes the HQ credentials, any clinic that had the same credentials will be kept in synch with HQ
			long clinicNum=0;
			if(PrefC.HasClinicsEnabled) {
				clinicNum=_listUserClinicNums[comboClinic.SelectedIndex];
			}
			string payTypeSelected="";
			if(comboPaymentType.SelectedIndex>-1) {
				payTypeSelected=_listPaymentTypeDefs[comboPaymentType.SelectedIndex].DefNum.ToString();
			}
			//set the values in the list for this clinic
			_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiUserName)
				.ForEach(x => x.PropertyValue=textUsername.Text);//always 1 item; null safe
			_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimpleApiKey)
				.ForEach(x => x.PropertyValue=textKey.Text);//always 1 item; null safe
			_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc==PaySimple.PropertyDescs.PaySimplePayType)
				.ForEach(x => x.PropertyValue=payTypeSelected);//always 1 item; null safe
			string payTypeCur;
			//make sure any other clinics with PaySimple enabled also have a payment type selected
			for(int i=0;i<_listUserClinicNums.Count;i++) {
				if(!checkEnabled.Checked) {//if program link is not enabled, do not bother checking the payment type selected
					break;
				}
				payTypeCur=ProgramProperties.GetPropValFromList(_listProgProps,PaySimple.PropertyDescs.PaySimplePayType,_listUserClinicNums[i]);
				//if the program is enabled and the username and key fields are not blank,
				//PaySimple is enabled for this clinic so make sure the payment type is also set
				if(ProgramProperties.GetPropValFromList(_listProgProps,PaySimple.PropertyDescs.PaySimpleApiUserName,_listUserClinicNums[i])!="" //if username set
					&& ProgramProperties.GetPropValFromList(_listProgProps,PaySimple.PropertyDescs.PaySimpleApiKey,_listUserClinicNums[i])!=""//and key set
					&& !_listPaymentTypeDefs.Any(x => x.DefNum.ToString()==payTypeCur)) //and paytype is not a valid DefNum
				{
					MsgBox.Show(this,"Please select the payment type for all clinics with PaySimple username and key set.");
					return;
				}
			}
			#endregion Validation
			#region Save
			if(_progCur.Enabled!=checkEnabled.Checked) {//only update the program if the IsEnabled flag has changed
				_progCur.Enabled=checkEnabled.Checked;
				Programs.Update(_progCur);
			}
			ProgramProperties.Sync(_listProgProps,_progCur.ProgramNum);
			#endregion Save
			DataValid.SetInvalid(InvalidType.Programs);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}