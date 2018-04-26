using CodeBase;
using Microsoft.Win32;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.Mobile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Globalization;
using System.Data;
using System.Linq;
using System.IO;
using WebServiceSerializer;
using OpenDentBusiness.WebServiceMainHQ;
using OpenDentBusiness.WebTypes.WebSched.TimeSlot;

namespace OpenDental {
	public partial class FormEServicesSetup {
		///<summary>A list of all clinics.  This list could include clinics that the user should not have access to so be careful using it. For the sake of modular code, there are seperate lists for the Integrated Texting (sms) and Automated eConfirmation (eC) tabs.</summary>
		private List<Clinic> _listWebSchedClinics;
		private List<RecallType> _listRecallTypes;
		///<summary>A list of all operatories that have IsWebSched set to true.</summary>
		private List<Operatory> _listWebSchedRecallOps;		
		///<summary>A deep copy of Providers.ListShortDeep.  Use the cache instead of this list if you need an up to date list of providers.</summary>
		private List<Provider> _listWebSchedProviders;
		///<summary>Provider number used to filter the Time Slots grid.  0 is treated as 'All'</summary>
		private long _webSchedProvNum=0;
		///<summary>Clinic number used to filter the Time Slots grid.  0 is treated as 'Unassigned'</summary>
		private long _webSchedClinicNum=0;				
		private ODThread _threadFillGridWebSchedTimeSlots=null;
		///<summary>Set to true whenever the Web Sched recall thread is already running while another thing wants it to refresh yet again.
		///E.g. The window loads which initially starts a fill thread and then the user quickly starts changing filters.</summary>
		private bool _isWebSchedTimeSlotsOutdated=false;

		private bool IsTabValidWebSchedRecall() {
			if(textWebSchedPerBatch.errorProvider1.GetError(textWebSchedPerBatch)!="") {
				return false;
			}
			return true;
		}

		private void SaveTabWebSchedRecall() {
			WebSchedAutomaticSend sendType = WebSchedAutomaticSend.SendToEmailOnlyPreferred;
			if(radioDoNotSend.Checked) {
				sendType=WebSchedAutomaticSend.DoNotSend;
			}
			else if(radioSendToEmail.Checked) {
				sendType=WebSchedAutomaticSend.SendToEmail;
			}
			else if(radioSendToEmailNoPreferred.Checked) {
				sendType=WebSchedAutomaticSend.SendToEmailNoPreferred;
			}
			WebSchedAutomaticSend beforeEnum=(WebSchedAutomaticSend)PrefC.GetInt(PrefName.WebSchedAutomaticSendSetting);
			if(Prefs.UpdateInt(PrefName.WebSchedAutomaticSendSetting,(int)sendType)) {
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"WebSched automated email preference changed from "+beforeEnum.ToString()+" to "+sendType.ToString()+".");
			}
			WebSchedAutomaticSend sendTypeText=WebSchedAutomaticSend.SendToText;
			if(radioDoNotSendText.Checked) {
				sendTypeText=WebSchedAutomaticSend.DoNotSend;
			}
			beforeEnum=(WebSchedAutomaticSend)PrefC.GetInt(PrefName.WebSchedAutomaticSendTextSetting);
			if(Prefs.UpdateInt(PrefName.WebSchedAutomaticSendTextSetting,(int)sendTypeText)) {
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"WebSched automated text preference changed from "+beforeEnum.ToString()+" to "+sendTypeText.ToString()+".");
			}
			int beforeInt=PrefC.GetInt(PrefName.WebSchedTextsPerBatch);
			int afterInt=PIn.Int(textWebSchedPerBatch.Text);
			if(Prefs.UpdateInt(PrefName.WebSchedTextsPerBatch,afterInt)) {
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"WebSched batch size preference changed from "+beforeInt.ToString()+" to "+afterInt.ToString()+".");
			}
			Prefs.UpdateBool(PrefName.WebSchedRecallAllowProvSelection,checkRecallAllowProvSelection.Checked);
			if(comboWSRConfirmStatus.SelectedIndex!=-1) {
				Def newConfirmStatus=Defs.GetDefByExactName(DefCat.ApptConfirmed,comboWSRConfirmStatus.SelectedItem.ToString());
				Prefs.UpdateLong(PrefName.WebSchedRecallConfirmStatus,newConfirmStatus.DefNum);
			}
		}

		private void FillTabWebSchedRecall() {
			switch(PrefC.GetInt(PrefName.WebSchedAutomaticSendSetting)) {
				case (int)WebSchedAutomaticSend.DoNotSend:
					radioDoNotSend.Checked=true;
					break;
				case (int)WebSchedAutomaticSend.SendToEmail:
					radioSendToEmail.Checked=true;
					break;
				case (int)WebSchedAutomaticSend.SendToEmailNoPreferred:
					radioSendToEmailNoPreferred.Checked=true;
					break;
				case (int)WebSchedAutomaticSend.SendToEmailOnlyPreferred:
					radioSendToEmailOnlyPreferred.Checked=true;
					break;
			}
			switch(PrefC.GetInt(PrefName.WebSchedAutomaticSendTextSetting)) {
				case (int)WebSchedAutomaticSend.DoNotSend:
					radioDoNotSendText.Checked=true;
					break;
				case (int)WebSchedAutomaticSend.SendToText:
					radioSendText.Checked=true;
					break;
			}
			textWebSchedPerBatch.Text=PrefC.GetString(PrefName.WebSchedTextsPerBatch);
			textWebSchedDateStart.Text=DateTime.Today.ToShortDateString();
			comboWebSchedClinic.Items.Clear();
			comboWebSchedClinic.Items.Add(Lan.g(this,"Unassigned"));
			_listWebSchedClinics=Clinics.GetDeepCopy();
			for(int i = 0;i<_listWebSchedClinics.Count;i++) {
				comboWebSchedClinic.Items.Add(_listWebSchedClinics[i].Abbr);
			}
			comboWebSchedClinic.SelectedIndex=0;
			_listWebSchedProviders=Providers.GetDeepCopy(true);
			comboWebSchedProviders.Items.Clear();
			comboWebSchedProviders.Items.Add(Lan.g(this,"All"));
			for(int i = 0;i<_listWebSchedProviders.Count;i++) {
				comboWebSchedProviders.Items.Add(_listWebSchedProviders[i].GetLongDesc());
			}
			comboWebSchedProviders.SelectedIndex=0;
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				labelWebSchedClinic.Visible=false;
				comboWebSchedClinic.Visible=false;
				butWebSchedPickClinic.Visible=false;
			}
			FillListBoxWebSchedBlockoutTypes(PrefC.GetString(PrefName.WebSchedRecallIgnoreBlockoutTypes).Split(new char[] { ',' }),listboxWebSchedRecallIgnoreBlockoutTypes);
			FillGridWebSchedRecallTypes();
			FillGridWebSchedOperatories();
			FillGridWebSchedTimeSlotsThreaded();
			listBoxWebSchedProviderPref.SelectedIndex=PrefC.GetInt(PrefName.WebSchedProviderRule);
			checkRecallAllowProvSelection.Checked=PrefC.GetBool(PrefName.WebSchedRecallAllowProvSelection);
			long defaultStatus=PrefC.GetLong(PrefName.WebSchedRecallConfirmStatus);
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.ApptConfirmed,true);
			for(int i=0;i<listDefs.Count;i++) {
				int idx=comboWSRConfirmStatus.Items.Add(listDefs[i].ItemName);
				if(listDefs[i].DefNum==defaultStatus) {
					comboWSRConfirmStatus.SelectedIndex=idx;
				}
			}
			comboWSRConfirmStatus.IndexSelectOrSetText(listDefs.ToList().FindIndex(x => x.DefNum==defaultStatus),
			   () => { return defaultStatus==0 ? "" : Defs.GetName(DefCat.ApptConfirmed,defaultStatus)+" ("+Lan.g(this,"hidden")+")"; });
		}

		private void AuthorizeTabWebSchedRecall(bool allowEdit) {
			butRecallSchedSetup.Enabled=allowEdit;
			butWebSchedRecallBlockouts.Enabled=allowEdit;
			listBoxWebSchedProviderPref.Enabled=allowEdit;
			groupBoxWebSchedAutomation.Enabled=allowEdit;
			groupWebSchedPreview.Enabled=allowEdit;
			groupWebSchedText.Enabled=allowEdit;
		}

		private void FillListBoxWebSchedBlockoutTypes(string[] arrayBlockoutTypeDefNums,ListBox listBoxToFill) {
			List<long> listBlockoutTypes=new List<long>();
			foreach(string strDefNum in arrayBlockoutTypeDefNums) {
				listBlockoutTypes.Add(PIn.Long(strDefNum));
			}
			List<Def> listBlockoutTypeDefs=Defs.GetDefs(DefCat.BlockoutTypes,listBlockoutTypes);
			listBoxToFill.Items.Clear();
			foreach(Def defCur in listBlockoutTypeDefs) {
				listBoxToFill.Items.Add(defCur.ItemName);
			}
		}

		private void linkLabelAboutWebSched_Click(object sender,EventArgs e) {
			MessageBox.Show("Use the Web Sched button in the Recall List window to send "
				+"emails with a link that will allow patients to quickly schedule their recall appointments. "
				+"You may also publish a link to the New Patient Appt URL on your web site to allow new patients to schedule "
				+"their first appointment.  All appointments scheduled using Web Sched will instantly show "
				+"up on the schedule.",Lan.g(this,"Web Sched Information"),MessageBoxButtons.OK);
		}

		private void butWebSchedRecallBlockouts_Click(object sender,EventArgs e) {
			string[] arrayDefNums=PrefC.GetString(PrefName.WebSchedRecallIgnoreBlockoutTypes).Split(new char[] {','}); //comma-delimited list.
			List<long> listBlockoutTypes=new List<long>();
			foreach(string strDefNum in arrayDefNums) {
				listBlockoutTypes.Add(PIn.Long(strDefNum));
			}
			List<Def> listBlockoutTypeDefs=Defs.GetDefs(DefCat.BlockoutTypes,listBlockoutTypes);
			FormDefinitionPicker FormDP=new FormDefinitionPicker(DefCat.BlockoutTypes,listBlockoutTypeDefs);
			FormDP.HasShowHiddenOption=true;
			FormDP.IsMultiSelectionMode=true;
			FormDP.ShowDialog();
			if(FormDP.DialogResult==DialogResult.OK) {
				listboxWebSchedRecallIgnoreBlockoutTypes.Items.Clear();
				foreach(Def defCur in FormDP.ListSelectedDefs) {
					listboxWebSchedRecallIgnoreBlockoutTypes.Items.Add(defCur.ItemName);
				}
				string strListWebSChedRecallIgnoreBlockoutTypes=String.Join(",",FormDP.ListSelectedDefs.Select(x => x.DefNum));
				Prefs.UpdateString(PrefName.WebSchedRecallIgnoreBlockoutTypes,strListWebSChedRecallIgnoreBlockoutTypes);
			}
		}

		///<summary>Shows the Operatories window and allows the user to edit them.  Does not show the window if user does not have Setup permission.
		///Refreshes all corresponding grids within the Web Sched tab that display Operatory information.  Feel free to add to this method.</summary>
		private void ShowOperatoryEditAndRefreshGrids() {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormOperatories FormO=new FormOperatories();
			FormO.ShowDialog();
			if(FormO.ListConflictingAppts.Count>0) {
				FormApptConflicts FormAC=new FormApptConflicts(FormO.ListConflictingAppts);
				FormAC.Show();
				FormAC.BringToFront();
			}
			FillGridWebSchedOperatories();
			FillGridWebSchedTimeSlotsThreaded();
			FillGridWebSchedNewPatApptOps();
			FillGridWebSchedNewPatApptTimeSlotsThreaded();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Operatories accessed via EServices Setup window.");
		}

		#region Web Sched - Recalls
		///<summary>Also refreshed the combo box of available recall types.</summary>
		private void FillGridWebSchedRecallTypes() {
			//Keep track of the previously selected recall type.
			long selectedRecallTypeNum=0;
			if(comboWebSchedRecallTypes.SelectedIndex!=-1) {
				selectedRecallTypeNum=_listRecallTypes[comboWebSchedRecallTypes.SelectedIndex].RecallTypeNum;
			}
			//Fill the combo boxes for the time slots preview.
			comboWebSchedRecallTypes.Items.Clear();
			_listRecallTypes=RecallTypes.GetDeepCopy();
			for(int i = 0;i<_listRecallTypes.Count;i++) {
				comboWebSchedRecallTypes.Items.Add(_listRecallTypes[i].Description);
				if(_listRecallTypes[i].RecallTypeNum==selectedRecallTypeNum) {
					comboWebSchedRecallTypes.SelectedIndex=i;
				}
			}
			if(selectedRecallTypeNum==0&&comboWebSchedRecallTypes.Items.Count>0) {
				comboWebSchedRecallTypes.SelectedIndex=0;//Arbitrarily select the first recall type.
			}
			gridWebSchedRecallTypes.BeginUpdate();
			gridWebSchedRecallTypes.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableRecallTypes","Description"),130);
			gridWebSchedRecallTypes.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRecallTypes","Time Pattern"),100);
			gridWebSchedRecallTypes.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRecallTypes","Time Length"),0);
			col.TextAlign=HorizontalAlignment.Center;
			gridWebSchedRecallTypes.Columns.Add(col);
			gridWebSchedRecallTypes.Rows.Clear();
			ODGridRow row;
			for(int i = 0;i<_listRecallTypes.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listRecallTypes[i].Description);
				row.Cells.Add(_listRecallTypes[i].TimePattern);
				int timeLength=RecallTypes.ConvertTimePattern(_listRecallTypes[i].TimePattern).Length * 5;
				if(timeLength==0) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(timeLength.ToString()+" "+Lan.g("TableRecallTypes","mins"));
				}
				gridWebSchedRecallTypes.Rows.Add(row);
			}
			gridWebSchedRecallTypes.EndUpdate();
		}

		private void FillGridWebSchedOperatories() {
			_listWebSchedRecallOps=Operatories.GetOpsForWebSched();
			int opNameWidth=170;
			int clinicWidth=80;
			if(!PrefC.HasClinicsEnabled) {
				opNameWidth+=clinicWidth;
			}
			gridWebSchedOperatories.BeginUpdate();
			gridWebSchedOperatories.Columns.Clear();
			gridWebSchedOperatories.Columns.Add(new ODGridColumn(Lan.g("TableOperatories","Op Name"),opNameWidth));
			gridWebSchedOperatories.Columns.Add(new ODGridColumn(Lan.g("TableOperatories","Abbrev"),70));
			if(PrefC.HasClinicsEnabled) {
				gridWebSchedOperatories.Columns.Add(new ODGridColumn(Lan.g("TableOperatories","Clinic"),clinicWidth));
			}
			gridWebSchedOperatories.Columns.Add(new ODGridColumn(Lan.g("TableOperatories","Provider"),90));
			gridWebSchedOperatories.Columns.Add(new ODGridColumn(Lan.g("TableOperatories","Hygienist"),90));
			gridWebSchedOperatories.Rows.Clear();
			ODGridRow row;
			for(int i = 0;i<_listWebSchedRecallOps.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listWebSchedRecallOps[i].OpName);
				row.Cells.Add(_listWebSchedRecallOps[i].Abbrev);
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(Clinics.GetAbbr(_listWebSchedRecallOps[i].ClinicNum));
				}
				row.Cells.Add(Providers.GetAbbr(_listWebSchedRecallOps[i].ProvDentist));
				row.Cells.Add(Providers.GetAbbr(_listWebSchedRecallOps[i].ProvHygienist));
				gridWebSchedOperatories.Rows.Add(row);
			}
			gridWebSchedOperatories.EndUpdate();
		}

		private void FillGridWebSchedTimeSlotsThreaded() {
			if(this.InvokeRequired) {
				this.BeginInvoke((Action)delegate () {
					FillGridWebSchedTimeSlotsThreaded();
				});
				return;
			}
			//Validate time slot settings.
			if(textWebSchedDateStart.errorProvider1.GetError(textWebSchedDateStart)!="") {
				//Don't bother warning the user.  It will just be annoying.  The red indicator should be sufficient.
				return;
			}
			if(comboWebSchedRecallTypes.SelectedIndex<0
				||comboWebSchedClinic.SelectedIndex<0
				||comboWebSchedProviders.SelectedIndex<0) {
				return;
			}
			//Protect against re-entry
			if(_threadFillGridWebSchedTimeSlots!=null) {
				//A thread is already refreshing the time slots grid so we simply need to queue up another refresh once the one thread has finished.
				_isWebSchedTimeSlotsOutdated=true;
				return;
			}
			_isWebSchedTimeSlotsOutdated=false;
			DateTime dateStart=PIn.Date(textWebSchedDateStart.Text);
			RecallType recallType=_listRecallTypes[comboWebSchedRecallTypes.SelectedIndex];
			Clinic clinic=_listWebSchedClinics.Find(x => x.ClinicNum==_webSchedClinicNum);//null clinic is treated as unassigned.
			List<Provider> listProviders=new List<Provider>(_listWebSchedProviders);//Use all providers by default.
			Provider provider=_listWebSchedProviders.Find(x => x.ProvNum==_webSchedProvNum);
			if(provider!=null) {
				//Only use the provider that the user picked from the provider picker.
				listProviders=new List<Provider>() { provider };
			}
			WebSchedTimeSlotArgs webSchedTimeSlotArgs=new WebSchedTimeSlotArgs() {
				RecallTypeCur=recallType,
				ClinicCur=clinic,
				DateStart=dateStart,
				DateEnd=dateStart.AddDays(30),
				ListProviders=listProviders
			};
			_threadFillGridWebSchedTimeSlots=new ODThread(GetWebSchedTimeSlotsWorker,webSchedTimeSlotArgs);
			_threadFillGridWebSchedTimeSlots.Name="ThreadWebSchedRecallTimeSlots";
			_threadFillGridWebSchedTimeSlots.AddExitHandler(GetWebSchedTimeSlotsThreadExitHandler);
			_threadFillGridWebSchedTimeSlots.AddExceptionHandler(GetWebSchedTimeSlotsExceptionHandler);
			_threadFillGridWebSchedTimeSlots.Start(true);
		}

		private void GetWebSchedTimeSlotsWorker(ODThread o) {
			WebSchedTimeSlotArgs w=(WebSchedTimeSlotArgs)o.Parameters[0];
			List<TimeSlot> listTimeSlots=new List<TimeSlot>();
			try {
				//Get the next 30 days of open time schedules with the current settings
				listTimeSlots=TimeSlots.GetAvailableWebSchedTimeSlots(w.RecallTypeCur,w.ListProviders,w.ClinicCur,w.DateStart,w.DateEnd);
			}
			catch(Exception) {
				//The user might not have Web Sched ops set up correctly.  Don't warn them here because it is just annoying.  They'll figure it out.
			}
			o.Tag=listTimeSlots;
		}

		private void GetWebSchedTimeSlotsThreadExitHandler(ODThread o) {
			ODException.SwallowAnyException(() => {
				FillGridWebSchedTimeSlots((List<TimeSlot>)o.Tag);
			});
			_threadFillGridWebSchedTimeSlots=null;
			//If something else wanted to refresh the grid while we were busy filling it then we need to refresh again.  A filter could have changed.
			if(_isWebSchedTimeSlotsOutdated) {
				FillGridWebSchedTimeSlotsThreaded();
			}
		}

		private void GetWebSchedTimeSlotsExceptionHandler(Exception e) {
			_threadFillGridWebSchedTimeSlots=null;
		}

		private void FillGridWebSchedTimeSlots(List<TimeSlot> listTimeSlots) {
			if(this.InvokeRequired) {
				this.Invoke((Action)delegate () { FillGridWebSchedTimeSlots(listTimeSlots); });
				return;
			}
			gridWebSchedTimeSlots.BeginUpdate();
			gridWebSchedTimeSlots.Columns.Clear();
			ODGridColumn col=new ODGridColumn("",0);
			col.TextAlign=HorizontalAlignment.Center;
			gridWebSchedTimeSlots.Columns.Add(col);
			gridWebSchedTimeSlots.Rows.Clear();
			ODGridRow row;
			DateTime dateTimeSlotLast=DateTime.MinValue;
			foreach(TimeSlot timeSlot in listTimeSlots) {
				//Make a new row for every unique day.
				if(dateTimeSlotLast.Date!=timeSlot.DateTimeStart.Date) {
					dateTimeSlotLast=timeSlot.DateTimeStart;
					row=new ODGridRow();
					row.ColorBackG=Color.LightBlue;
					row.Cells.Add(timeSlot.DateTimeStart.ToShortDateString());
					gridWebSchedTimeSlots.Rows.Add(row);
				}
				row=new ODGridRow();
				row.Cells.Add(timeSlot.DateTimeStart.ToShortTimeString()+" - "+timeSlot.DateTimeStop.ToShortTimeString());
				gridWebSchedTimeSlots.Rows.Add(row);
			}
			gridWebSchedTimeSlots.EndUpdate();
		}

		private void gridWebSchedRecallTypes_DoubleClick(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormRecallTypes FormRT=new FormRecallTypes();
			FormRT.ShowDialog();
			FillGridWebSchedRecallTypes();
			FillGridWebSchedTimeSlotsThreaded();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Recall Types accessed via EServices Setup window.");
		}

		private void gridWebSchedOperatories_DoubleClick(object sender,EventArgs e) {
			ShowOperatoryEditAndRefreshGrids();
		}

		private void listBoxWebSchedProviderPref_SelectedIndexChanged(object sender,EventArgs e) {
			Prefs.UpdateInt(PrefName.WebSchedProviderRule,listBoxWebSchedProviderPref.SelectedIndex);
		}

		private void comboWebSchedRecallTypes_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGridWebSchedTimeSlotsThreaded();
		}

		private void comboWebSchedProviders_SelectionChangeCommitted(object sender,EventArgs e) {
			_webSchedProvNum=0;
			if(comboWebSchedProviders.SelectedIndex>0) {//Greater than 0 due to "All"
				_webSchedProvNum=_listWebSchedProviders[comboWebSchedProviders.SelectedIndex-1].ProvNum;//-1 for 'All'
			}
			FillGridWebSchedTimeSlotsThreaded();
		}

		private void comboWebSchedClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			_webSchedClinicNum=0;
			if(comboWebSchedClinic.SelectedIndex>0) {//Greater than 0 due to "Unassigned"
				_webSchedClinicNum=_listWebSchedClinics[comboWebSchedClinic.SelectedIndex-1].ClinicNum;//-1 for 'Unassigned'
			}
			FillGridWebSchedTimeSlotsThreaded();
		}

		private void textWebSchedDateStart_TextChanged(object sender,EventArgs e) {
			//Only refresh the grid if the user has typed in a valid date.
			if(textWebSchedDateStart.errorProvider1.GetError(textWebSchedDateStart)=="") {
				FillGridWebSchedTimeSlotsThreaded();
			}
		}

		private void WebSchedRecallAutoSendRadioButtons_CheckedChanged(object sender,EventArgs e) {
			//Only do validation when the Web Sched tab is selected and either Do Not Send is NOT checked.
			if(tabControl.SelectedTab!=tabWebSched || (radioDoNotSend.Checked && radioDoNotSendText.Checked)) {
				return;
			}
			//Validate the following recall setup preferences.  See task #880961 or #879613 for more details.
			//1. The Days Past field is not blank
			//2. The Initial Reminder field is greater than 0
			//3. The Second(or more) Reminder field is greater than 0
			//4. Integrated texting is enabled if Send Text is checked
			List<string> listSetupErrors=new List<string>();
			bool isEmailSendInvalid=false;
			bool isTextSendInvalid=false;
			if(PrefC.GetLong(PrefName.RecallDaysPast)==-1) {//Days Past field
				listSetupErrors.Add("- "+Lan.g(this,"Days Past (e.g. 1095, blank, etc) field cannot be blank."));
				isEmailSendInvalid=true;
				isTextSendInvalid=true;
			}
			if(PrefC.GetLong(PrefName.RecallShowIfDaysFirstReminder)<1) {//Initial Reminder field
				listSetupErrors.Add("- "+Lan.g(this,"Initial Reminder field has to be greater than 0."));
				isEmailSendInvalid=true;
				isTextSendInvalid=true;
			}
			if(PrefC.GetLong(PrefName.RecallShowIfDaysSecondReminder)<1) {//Second(or more) Reminder field
				listSetupErrors.Add("- "+Lan.g(this,"Second (or more) Reminder field has to be greater than 0."));
				isEmailSendInvalid=true;
				isTextSendInvalid=true;
			}
			if(radioSendText.Checked && !SmsPhones.IsIntegratedTextingEnabled()) {
				listSetupErrors.Add("- "+Lan.g(this,"Integrated texting must be enabled."));
				isTextSendInvalid=true;
			}
			//Checking the "Do Not Send" radio button will automatically uncheck all the other radio buttons in the group box.
			if(isEmailSendInvalid) {
				radioDoNotSend.Checked=true;
			}
			if(isTextSendInvalid) {
				radioDoNotSendText.Checked=true;
			}
			if(listSetupErrors.Count>0) {
				MessageBox.Show(Lan.g(this,"Recall Setup settings are not correctly set in order to Send Messages Automatically to patients:")
						+"\r\n"+string.Join("\r\n",listSetupErrors)
					,Lan.g(this,"Web Sched - Recall Setup Error"));
			}
		}

		private void butWebSchedSetup_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormRecallSetup FormRS=new FormRecallSetup();
			FormRS.ShowDialog();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Recall Setup accessed via EServices Setup window.");
		}

		private void butWebSchedToday_Click(object sender,EventArgs e) {
			textWebSchedDateStart.Text=DateTime.Today.ToShortDateString();
			//Don't need to call FillTimeSlots because textChanged event already calls it.
		}

		private void butWebSchedPickProv_Click(object sender,EventArgs e) {
			FormProviderPick FormPP=new FormProviderPick();
			if(comboWebSchedProviders.SelectedIndex>0) {
				FormPP.SelectedProvNum=_webSchedProvNum;
			}
			FormPP.ShowDialog();
			if(FormPP.DialogResult!=DialogResult.OK) {
				return;
			}
			comboWebSchedProviders.SelectedIndex=_listWebSchedProviders.FindIndex(x => x.ProvNum==FormPP.SelectedProvNum)+1;//+1 for 'All'
			_webSchedProvNum=FormPP.SelectedProvNum;
			FillGridWebSchedTimeSlotsThreaded();
		}

		private void butWebSchedPickClinic_Click(object sender,EventArgs e) {
			FormClinics FormC=new FormClinics();
			FormC.IsSelectionMode=true;
			FormC.ShowDialog();
			if(FormC.DialogResult!=DialogResult.OK) {
				return;
			}
			comboWebSchedClinic.SelectedIndex=_listWebSchedClinics.FindIndex(x => x.ClinicNum==FormC.SelectedClinicNum)+1;//+1 for 'Unassigned'
			_webSchedClinicNum=FormC.SelectedClinicNum;
			FillGridWebSchedTimeSlotsThreaded();
		}
		#endregion
		
		///<summary>This is a helper struct which is set to the Tag of the Web Sched threads so that they don't have to access UI elements.</summary>
		private struct WebSchedTimeSlotArgs {
			///<summary>Only used for Web Sched Recall.</summary>
			public RecallType RecallTypeCur;
			///<summary>Only used for Web Sched Recall.</summary>
			public List<Provider> ListProviders;
			///<summary>Only trust ClinicNum from this object.</summary>
			public Clinic ClinicCur;
			///<summary></summary>
			public DateTime DateStart;
			///<summary></summary>
			public DateTime DateEnd;
		}
	}
}
