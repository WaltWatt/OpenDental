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
		//==================== eConfirm & eRemind Variables ====================
		private List<Def> _listDefsApptStatus;
		private List<Clinic> _ecListClinics;
		private Clinic _ecClinicCur;
		///<summary>When using clinics, this is the index of the clinic rules to use.</summary>
		///<summary>not acutal idx, actually just ClinicNum.</summary>
		private long _clinicRuleClinicNum;
		///<summary>Key = ClinicNum, 0=Practice/Defaults. Value = Rules defined for that clinic. If a clinic uses defaults, its respective list of rules will be empty.</summary>
		private Dictionary<long,List<ApptReminderRule>> _dictClinicRules;
		
		private bool IsTabValidECR() {
			if(new[] { comboStatusEAccepted.SelectedIndex,comboStatusESent.SelectedIndex,comboStatusEDeclined.SelectedIndex,comboStatusEFailed.SelectedIndex }.Where(x => x!=-1).GroupBy(x => x).Any(x => x.Count()>1)) {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"All eConfirmation appointment statuses should be different. Continue anyway?")) {
					return false;
				}
			}
			return true;
		}

		private void FillTabECR() {
			FillECRActivationButtons();
			checkEnableNoClinic.Checked=PrefC.GetBool(PrefName.ApptConfirmEnableForClinicZero);
			if(PrefC.HasClinicsEnabled) {//CLINICS
				checkUseDefaultsEC.Visible=true;
				checkUseDefaultsEC.Enabled=false;//when loading form we will be viewing defaults.
				checkIsConfirmEnabled.Visible=true;
				groupAutomationStatuses.Text=Lan.g(this,"eConfirmation Settings")+" - "+Lan.g(this,"Affects all Clinics");
			}
			else {//NO CLINICS
				checkUseDefaultsEC.Visible=false;
				checkUseDefaultsEC.Enabled=false;
				checkUseDefaultsEC.Checked=false;
				checkIsConfirmEnabled.Visible=false;
				checkEnableNoClinic.Visible=false;
				groupAutomationStatuses.Text=Lan.g(this,"eConfirmation Settings");
			}
			setListClinicsAndDictRulesHelper();
			comboClinicEConfirm.SelectedIndex=0;
			_listDefsApptStatus=Defs.GetDefsForCategory(DefCat.ApptConfirmed,true);
			comboStatusESent.Items.Clear();
			comboStatusEAccepted.Items.Clear();
			comboStatusEDeclined.Items.Clear();
			comboStatusEFailed.Items.Clear();
			_listDefsApptStatus.ForEach(x => comboStatusESent.Items.Add(x.ItemName));
			_listDefsApptStatus.ForEach(x => comboStatusEAccepted.Items.Add(x.ItemName));
			_listDefsApptStatus.ForEach(x => comboStatusEDeclined.Items.Add(x.ItemName));
			_listDefsApptStatus.ForEach(x => comboStatusEFailed.Items.Add(x.ItemName));
			long prefApptEConfirmStatusSent=PrefC.GetLong(PrefName.ApptEConfirmStatusSent);
			long prefApptEConfirmStatusAccepted=PrefC.GetLong(PrefName.ApptEConfirmStatusAccepted);
			long prefApptEConfirmStatusDeclined=PrefC.GetLong(PrefName.ApptEConfirmStatusDeclined);
			long prefApptEConfirmStatusSendFailed=PrefC.GetLong(PrefName.ApptEConfirmStatusSendFailed);
			//SENT
			if(prefApptEConfirmStatusSent>0) {
				//Selects combo box option if it exists, if it doesn't it sets the text of the combo box to the hidden one.
				comboStatusESent.IndexSelectOrSetText(_listDefsApptStatus.FindIndex(x => x.DefNum==prefApptEConfirmStatusSent),() => {
					return Defs.GetName(DefCat.ApptConfirmed,prefApptEConfirmStatusSent)+" (hidden)";
				});
			}
			else {
				comboStatusESent.SelectedIndex=0;
			}
			//CONFIRMED
			if(prefApptEConfirmStatusAccepted>0) {
				//Selects combo box option if it exists, if it doesn't it sets the text of the combo box to the hidden one.
				comboStatusEAccepted.IndexSelectOrSetText(_listDefsApptStatus.FindIndex(x => x.DefNum==prefApptEConfirmStatusAccepted),() => {
					return Defs.GetName(DefCat.ApptConfirmed,prefApptEConfirmStatusAccepted)+" (hidden)";
				});
			}
			else {
				comboStatusEAccepted.SelectedIndex=0;
			}
			//NOT CONFIRMED
			if(prefApptEConfirmStatusDeclined>0) {
				//Selects combo box option if it exists, if it doesn't it sets the text of the combo box to the hidden one.
				comboStatusEDeclined.IndexSelectOrSetText(_listDefsApptStatus.FindIndex(x => x.DefNum==prefApptEConfirmStatusDeclined),() => {
					return Defs.GetName(DefCat.ApptConfirmed,prefApptEConfirmStatusDeclined)+" (hidden)";
				});
			}
			else {
				comboStatusEDeclined.SelectedIndex=0;
			}
			//Failed
			if(prefApptEConfirmStatusSendFailed>0) {
				//Selects combo box option if it exists, if it doesn't it sets the text of the combo box to the hidden one.
				comboStatusEFailed.IndexSelectOrSetText(_listDefsApptStatus.FindIndex(x => x.DefNum==prefApptEConfirmStatusSendFailed),() => {
					return Defs.GetName(DefCat.ApptConfirmed,prefApptEConfirmStatusSendFailed)+" (hidden)";
				});
			}
			else {
				comboStatusEFailed.SelectedIndex=0;
			}
			if(PrefC.GetBool(PrefName.ApptEConfirm2ClickConfirmation)) {
				radio2ClickConfirm.Checked=true;
			}
			else {
				radio1ClickConfirm.Checked=true;
			}
			FillConfStatusesGrid();
			FillRemindConfirmData();
		}

		private void SaveTabECR() {
			if(comboStatusESent.SelectedIndex!=-1) {
				Prefs.UpdateLong(PrefName.ApptEConfirmStatusSent,_listDefsApptStatus[comboStatusESent.SelectedIndex].DefNum);
			}
			if(comboStatusEAccepted.SelectedIndex!=-1) {
				Prefs.UpdateLong(PrefName.ApptEConfirmStatusAccepted,_listDefsApptStatus[comboStatusEAccepted.SelectedIndex].DefNum);
			}
			if(comboStatusEDeclined.SelectedIndex!=-1) {
				Prefs.UpdateLong(PrefName.ApptEConfirmStatusDeclined,_listDefsApptStatus[comboStatusEDeclined.SelectedIndex].DefNum);
			}
			if(comboStatusEFailed.SelectedIndex!=-1) {
				Prefs.UpdateLong(PrefName.ApptEConfirmStatusSendFailed,_listDefsApptStatus[comboStatusEFailed.SelectedIndex].DefNum);
			}
			Prefs.UpdateBool(PrefName.ApptConfirmEnableForClinicZero,checkEnableNoClinic.Checked);
			Prefs.UpdateBool(PrefName.ApptEConfirm2ClickConfirmation,radio2ClickConfirm.Checked);
			ApptReminderRules.SyncByClinicAndTypes(_dictClinicRules[_ecClinicCur.ClinicNum],_ecClinicCur.ClinicNum,
				ApptReminderType.Reminder,ApptReminderType.ConfirmationFutureDay);
			if(_ecClinicCur!=null&&_ecClinicCur.ClinicNum!=0) {
				_ecClinicCur.IsConfirmEnabled=checkIsConfirmEnabled.Checked;
				Clinics.Update(_ecClinicCur);
			}
		}

		private void AuthorizeTabECR(bool allowEdit) {
			groupAutomationStatuses.Enabled=allowEdit;
			butActivateReminder.Enabled=allowEdit;
			butActivateConfirm.Enabled=allowEdit;
			checkIsConfirmEnabled.Enabled=allowEdit;
			checkUseDefaultsEC.Enabled=allowEdit;
		}

		///<summary>Fills in memory Rules dictionary and clinics list based. This is very different from AppointmentReminderRules.GetRuleAndClinics.</summary>
		private void setListClinicsAndDictRulesHelper() {
			if(PrefC.HasClinicsEnabled) {//CLINICS
				_ecListClinics=new List<Clinic>() { new Clinic() { Description="Defaults",Abbr="Defaults" } };
				_ecListClinics.AddRange(Clinics.GetForUserod(Security.CurUser));
			}
			else {//NO CLINICS
				_ecListClinics=new List<Clinic>() { new Clinic() { Description="Practice",Abbr="Practice" } };
			}
			List<ApptReminderRule> listRulesTemp = ApptReminderRules.GetForTypes(ApptReminderType.Reminder,ApptReminderType.ConfirmationFutureDay);
			_dictClinicRules=_ecListClinics.Select(x => x.ClinicNum).ToDictionary(x => x,x => listRulesTemp.FindAll(y => y.ClinicNum==x));
			int idx = comboClinicEConfirm.SelectedIndex>0 ? comboClinicEConfirm.SelectedIndex : 0;
			comboClinicEConfirm.BeginUpdate();
			comboClinicEConfirm.Items.Clear();
			_ecListClinics.ForEach(x => comboClinicEConfirm.Items.Add(x.Abbr));//combo clinics may not be visible.
			if(idx>-1&&idx<comboClinicEConfirm.Items.Count) {
				comboClinicEConfirm.SelectedIndex=idx;
			}
			comboClinicEConfirm.EndUpdate();
		}

		private void FillConfStatusesGrid() {
			List<long> listDontSendConf=PrefC.GetString(PrefName.ApptConfirmExcludeESend).Split(',').Select(x => PIn.Long(x)).ToList();
			List<long> listDontChange=PrefC.GetString(PrefName.ApptConfirmExcludeEConfirm).Split(',').Select(x => PIn.Long(x)).ToList();
			List<long> listDontSendRem=PrefC.GetString(PrefName.ApptConfirmExcludeERemind).Split(',').Select(x => PIn.Long(x)).ToList();
			gridConfStatuses.BeginUpdate();
			gridConfStatuses.Columns.Clear();
			gridConfStatuses.Columns.Add(new ODGridColumn(Lan.g(this,"Status"),100));
			gridConfStatuses.Columns.Add(new ODGridColumn(Lan.g(this,"Don't Send"),70,HorizontalAlignment.Center));
			gridConfStatuses.Columns.Add(new ODGridColumn(Lan.g(this,"Don't Change"),70,HorizontalAlignment.Center));
			gridConfStatuses.Rows.Clear();
			foreach(Def defConfStatus in _listDefsApptStatus) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(defConfStatus.ItemName);
				row.Cells.Add(listDontSendConf.Contains(defConfStatus.DefNum) ? "X" : "");
				row.Cells.Add(listDontChange.Contains(defConfStatus.DefNum) ? "X" : "");
				row.Tag=defConfStatus;
				gridConfStatuses.Rows.Add(row);
			}
			gridConfStatuses.EndUpdate();
		}

		private void FillRemindConfirmData() {
			#region Fill Reminders grid.
			gridRemindersMain.BeginUpdate();
			gridRemindersMain.Columns.Clear();
			gridRemindersMain.Columns.Add(new ODGridColumn(Lan.g(this,"Type"),150) { TextAlign=HorizontalAlignment.Center });
			gridRemindersMain.Columns.Add(new ODGridColumn(Lan.g(this,"Lead Time"),250));
			//gridRemindersMain.Columns.Add(new ODGridColumn("Send\r\nAll",50) { TextAlign=HorizontalAlignment.Center });
			gridRemindersMain.Columns.Add(new ODGridColumn(Lan.g(this,"Send Order"),100));
			gridRemindersMain.NoteSpanStart=1;
			gridRemindersMain.NoteSpanStop=2;
			gridRemindersMain.Rows.Clear();
			ODGridRow row;
			if(_ecClinicCur==null||_ecClinicCur.IsConfirmDefault) {//Use defaults
				_clinicRuleClinicNum=0;
			}
			else {
				_clinicRuleClinicNum=_ecClinicCur.ClinicNum;
			}
			IEnumerable<ApptReminderRule> apptReminderRules=_dictClinicRules[_clinicRuleClinicNum]
				.OrderBy(x => x.TypeCur)//Reminders first, then Confirmation
				.ThenBy(x => !x.IsEnabled)//Show enabled before disabled
				.ThenBy(x => x.TSPrior);
			foreach(ApptReminderRule apptRule in apptReminderRules) {
				string sendOrderText = string.Join(", ",apptRule.SendOrder.Split(',').Select(x => Enum.Parse(typeof(CommType),x).ToString()));
				row=new ODGridRow();
				row.Cells.Add(Lan.g(this,apptRule.TypeCur.GetDescription())
					+(_ecClinicCur.IsConfirmDefault ? "\r\n("+Lan.g(this,"Defaults")+")" : ""));
				if(apptRule.TSPrior<=TimeSpan.Zero) {
					row.Cells.Add(Lan.g(this,"Disabled"));
				}
				else {
					row.Cells.Add(apptRule.TSPrior.ToStringDH());
				}
				row.Cells.Add(apptRule.IsSendAll ? Lan.g(this,"All") : sendOrderText);
				row.Note=Lan.g(this,"SMS Template")+":\r\n"+apptRule.TemplateSMS+"\r\n\r\n"+Lan.g(this,"Email Subject Template")+":\r\n"+apptRule.TemplateEmailSubject+"\r\n"+Lan.g(this,"Email Template")+":\r\n"+apptRule.TemplateEmail;
				row.Tag=apptRule;
				if(gridRemindersMain.Rows.Count%2==1) {
					row.ColorBackG=Color.FromArgb(240,240,240);//light gray every other row.
				}
				gridRemindersMain.Rows.Add(row);
			}
			gridRemindersMain.EndUpdate();
			#endregion
			#region Set add buttons
			bool allowEdit=Security.IsAuthorized(Permissions.EServicesSetup,true);
			if(comboClinicEConfirm.SelectedIndex>0) {//REAL CLINIC
				checkUseDefaultsEC.Visible=true;
				checkUseDefaultsEC.Enabled=allowEdit;
				checkIsConfirmEnabled.Enabled=allowEdit;//because we either cannot see it, or we are editing defaults.
				checkIsConfirmEnabled.Visible=true;
			}
			else {//CLINIC DEFAULTS/PRACTICE
				checkUseDefaultsEC.Visible=false;
				checkUseDefaultsEC.Enabled=false;
				checkIsConfirmEnabled.Enabled=false;//because we either cannot see it, or we are editing defaults.
				checkIsConfirmEnabled.Visible=false;
			}
			checkUseDefaultsEC.Checked=(_ecClinicCur!=null&&_ecClinicCur.ClinicNum>0&&_ecClinicCur.IsConfirmDefault);
			butAddReminder.Enabled=allowEdit;
			butAddConfirmation.Enabled=allowEdit;
			#endregion
		}

		private void FillECRActivationButtons() {
			//Reminder Activation Status
			if(PrefC.GetBool(PrefName.ApptRemindAutoEnabled)) {
				textStatusReminders.Text=Lan.g(this,"eReminders")+" : "+Lan.g(this,"Active");
				textStatusReminders.BackColor=Color.FromArgb(236,255,236);//light green
				textStatusReminders.ForeColor=Color.Black;//instead of disabled grey
				butActivateReminder.Text=Lan.g(this,"Deactivate eReminders");
			}
			else {
				textStatusReminders.Text=Lan.g(this,"eReminders")+" : "+Lan.g(this,"Inactive");
				textStatusReminders.BackColor=Color.FromArgb(254,235,233);//light red;
				textStatusReminders.ForeColor=Color.Black;//instead of disabled grey
				butActivateReminder.Text=Lan.g(this,"Activate eReminders");
			}
			//Confirmation Activation Status
			if(PrefC.GetBool(PrefName.ApptConfirmAutoEnabled)) {
				textStatusConfirmations.Text=Lan.g(this,"eConfirmations")+" : "+Lan.g(this,"Active");
				textStatusConfirmations.BackColor=Color.FromArgb(236,255,236);//light green
				textStatusConfirmations.ForeColor=Color.Black;//instead of disabled grey
				butActivateConfirm.Text=Lan.g(this,"Deactivate eConfirmations");
			}
			else {
				textStatusConfirmations.Text=Lan.g(this,"eConfirmations")+" : "+Lan.g(this,"Inactive");
				textStatusConfirmations.BackColor=Color.FromArgb(254,235,233);//light red;
				textStatusConfirmations.ForeColor=Color.Black;//instead of disabled grey
				butActivateConfirm.Text=Lan.g(this,"Activate eConfirmations");
			}
		}

		private void gridConfStatuses_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Def defConfirmation=(Def)gridConfStatuses.Rows[e.Row].Tag;
			FormDefEdit FormDE=new FormDefEdit(defConfirmation,_listDefsApptStatus,new DefCatOptions(DefCat.ApptConfirmed,enableColor:true,enableValue:true));
			FormDE.ShowDialog();
			if(FormDE.DialogResult==DialogResult.OK) {
				Defs.RefreshCache();
				_listDefsApptStatus=Defs.GetDefsForCategory(DefCat.ApptConfirmed,true);
				FillConfStatusesGrid();
			}
		}

		private void gridRemindersMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!Security.IsAuthorized(Permissions.EServicesSetup)) {
				return;
			}
			if(e.Row<0 || !(gridRemindersMain.Rows[e.Row].Tag is ApptReminderRule)) {
				return;//we did not click on a valid row.
			}
			if(_ecClinicCur!=null && _ecClinicCur.ClinicNum>0 && _ecClinicCur.IsConfirmDefault && !SwitchFromDefaults()) {
				return;
			}
			ApptReminderRule arr = (ApptReminderRule)gridRemindersMain.Rows[e.Row].Tag;
			int idx=_dictClinicRules[_clinicRuleClinicNum].IndexOf(arr);
			FormApptReminderRuleEdit FormARRE = new FormApptReminderRuleEdit(arr,_dictClinicRules[_clinicRuleClinicNum]);
			FormARRE.ShowDialog();
			if(FormARRE.DialogResult!=DialogResult.OK) {
				return;
			}
			if(FormARRE.ApptReminderRuleCur==null) {//Delete
				_dictClinicRules[_clinicRuleClinicNum].RemoveAt(idx);
			}
			else if(FormARRE.ApptReminderRuleCur.IsNew) {//Insert
				_dictClinicRules[_clinicRuleClinicNum].Add(FormARRE.ApptReminderRuleCur);//should never happen from the double click event
			}
			else {//Update
				_dictClinicRules[_clinicRuleClinicNum][idx]=FormARRE.ApptReminderRuleCur;
			}
			if(FormARRE.IsPrefsChanged) {
				FillConfStatusesGrid();
			}
			FillRemindConfirmData();
		}
		
		private void butAddReminder_Click(object sender,EventArgs e) {
			if(_ecClinicCur!=null && _ecClinicCur.ClinicNum>0 && _ecClinicCur.IsConfirmDefault) {
				if(!SwitchFromDefaults()) {
					return;
				}
			}
			ApptReminderRule arr = ApptReminderRules.CreateDefaultReminderRule(ApptReminderType.Reminder,_ecClinicCur.ClinicNum);
			FormApptReminderRuleEdit FormARRE = new FormApptReminderRuleEdit(arr,_dictClinicRules[_clinicRuleClinicNum]);
			FormARRE.ShowDialog();
			if(FormARRE.DialogResult!=DialogResult.OK) {
				return;
			}
			if(FormARRE.ApptReminderRuleCur==null||FormARRE.ApptReminderRuleCur.IsNew) {//Delete or Update
				//Nothing to update, this was a new rule.
			}
			else {//Insert
				_dictClinicRules[_clinicRuleClinicNum].Add(FormARRE.ApptReminderRuleCur);
			}
			if(FormARRE.IsPrefsChanged) {
				FillConfStatusesGrid();
			}
			FillRemindConfirmData();
		}
		
		private void butAddConfirmation_Click(object sender,EventArgs e) {
			if(_ecClinicCur!=null && _ecClinicCur.ClinicNum>0 && _ecClinicCur.IsConfirmDefault) {
				if(!SwitchFromDefaults()) {
					return;
				}
			}
			ApptReminderRule arr = ApptReminderRules.CreateDefaultReminderRule(ApptReminderType.ConfirmationFutureDay,_ecClinicCur.ClinicNum);
			FormApptReminderRuleEdit FormARRE = new FormApptReminderRuleEdit(arr,_dictClinicRules[_clinicRuleClinicNum]);
			FormARRE.ShowDialog();
			if(FormARRE.DialogResult!=DialogResult.OK) {
				return;
			}
			if(FormARRE.ApptReminderRuleCur==null||FormARRE.ApptReminderRuleCur.IsNew) {
				//Delete or Update
				//Nothing to delete or update, this was a new rule.
			}
			else {//Insert
				_dictClinicRules[_clinicRuleClinicNum].Add(FormARRE.ApptReminderRuleCur);
			}
			if(FormARRE.IsPrefsChanged) {
				FillConfStatusesGrid();
			}
			FillRemindConfirmData();
		}

		private void comboClinicEConfirm_SelectedIndexChanged(object sender,EventArgs e) {
			if(_ecListClinics.Count==0||_dictClinicRules.Count==0) {
				return;//form load;
			}
			if(_ecClinicCur!=null&&_ecClinicCur.ClinicNum>0) {//do not update this clinic-pref if we are editing defaults.
				_ecClinicCur.IsConfirmEnabled=checkIsConfirmEnabled.Checked;
				Clinics.Update(_ecClinicCur);
				Signalods.SetInvalid(InvalidType.Providers);
				//no need to save changes here because all Appointment reminder rules are saved to the DB from the edit window.
			}
			if(_ecClinicCur!=null) {
				ApptReminderRules.SyncByClinicAndTypes(_dictClinicRules[_ecClinicCur.ClinicNum],_ecClinicCur.ClinicNum,
					ApptReminderType.Reminder,ApptReminderType.ConfirmationFutureDay);
			}
			if(comboClinicEConfirm.SelectedIndex>-1&&comboClinicEConfirm.SelectedIndex<_ecListClinics.Count) {
				_ecClinicCur=_ecListClinics[comboClinicEConfirm.SelectedIndex];
			}
			checkUseDefaultsEC.Checked=_ecClinicCur!=null&&_ecClinicCur.IsConfirmDefault;
			checkIsConfirmEnabled.Checked=_ecClinicCur!=null&&_ecClinicCur.IsConfirmEnabled;
			FillRemindConfirmData();
		}

		///<summary>Switches the currently selected clinic over to using defaults. Also prompts user before continuing. 
		///Returns false if user cancelled or if there is no need to have switched to defaults.</summary>
		private bool SwitchFromDefaults() {
			if(_ecClinicCur==null||_ecClinicCur.ClinicNum==0) {
				return false;//somehow editing default clinic anyways, no need to switch.
			}
			//if(!MsgBox.Show(this,true,"Would you like to make a copy of the defaults for this clinic and continue editing the copy?")) {
			//	return false;
			//}
			_dictClinicRules[_ecClinicCur.ClinicNum]=_dictClinicRules[0].Select(x => x.Copy()).ToList();
			_dictClinicRules[_ecClinicCur.ClinicNum].ForEach(x => x.ClinicNum=_ecClinicCur.ClinicNum);
			_ecClinicCur.IsConfirmDefault=false;
			_ecListClinics[_ecListClinics.FindIndex(x => x.ClinicNum==_ecClinicCur.ClinicNum)].IsConfirmDefault=false;
			//Clinics.Update(_clinicCur);
			//Signalods.SetInvalid(InvalidType.Providers);//for clinics
			FillRemindConfirmData();
			return true;
		}

		///<summary>Switches the currently selected clinic over to using defaults. Also prompts user before continuing. 
		///Returns false if user cancelled or if there is no need to have switched to defaults.</summary>
		private bool SwitchToDefaults() {
			if(_ecClinicCur==null||_ecClinicCur.ClinicNum==0) {
				return false;//somehow editing default clinic anyways, no need to switch.
			}
			if(_dictClinicRules[_ecClinicCur.ClinicNum].Count>0&&!MsgBox.Show(this,true,"Delete custom rules for this clinic and switch to using defaults? This cannot be undone.")) {
				checkUseDefaultsEC.Checked=false;//undo checking of box.
				return false;
			}
			_ecClinicCur.IsConfirmDefault=true;
			_dictClinicRules[_ecClinicCur.ClinicNum]=new List<ApptReminderRule>();
			FillRemindConfirmData();
			return true;
		}

		private void checkIsConfirmEnabled_CheckedChanged(object sender,EventArgs e) {
			FillRemindConfirmData();
		}

		private void checkUseDefaultsEC_CheckedChanged(object sender,EventArgs e) {
			//TURNING DEFAULTS OFF
			if(!checkUseDefaultsEC.Checked&&_ecClinicCur.IsConfirmDefault&&_ecClinicCur.ClinicNum>0) {//Default switched off
				_ecClinicCur.IsConfirmDefault=false;
				_ecListClinics[comboClinicEConfirm.SelectedIndex].IsConfirmDefault=false;
				FillRemindConfirmData();
				return;
			}
			//TURNING DEFAULTS ON
			else if(checkUseDefaultsEC.Checked&&!_ecClinicCur.IsConfirmDefault&&_ecClinicCur.ClinicNum>0) {//Default switched on
				SwitchToDefaults();
				return;
			}
			//Silently do nothing because we just "changed" the checkbox to the state of the current clinic. 
			//I.e. When switching from clinic 1 to clinic 2, if 1 uses defaults and 2 does not, then this allows the new clinic to be loaded without updating the DB.
		}

		private void butActivateConfirm_Click(object sender,EventArgs e) {
			if(!WebServiceMainHQProxy.IsEServiceActive(_signupOut,eServiceCode.ConfirmationRequest)) { //Not yet activated with HQ.
				MsgBox.Show(this,"You must first signup for eConfirmations via the Signup tab before activating eConfirmations.");
				return;
			}
			bool isApptConfirmAutoEnabled = PrefC.GetBool(PrefName.ApptConfirmAutoEnabled);
			isApptConfirmAutoEnabled=!isApptConfirmAutoEnabled;
			Prefs.UpdateBool(PrefName.ApptConfirmAutoEnabled,isApptConfirmAutoEnabled);
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Automated appointment eConfirmations "+(isApptConfirmAutoEnabled ? "activated" : "deactivated")+".");
			Prefs.RefreshCache();
			Signalods.SetInvalid(InvalidType.Prefs);
			FillECRActivationButtons();
			//Add a default confirmation rule if none exists.
			if(isApptConfirmAutoEnabled && _dictClinicRules[0].Count(x => x.TypeCur==ApptReminderType.ConfirmationFutureDay)==0) {
				ApptReminderRule arr=ApptReminderRules.CreateDefaultReminderRule(ApptReminderType.ConfirmationFutureDay,0);//defaults to 7 days before appt
				_dictClinicRules[0].Add(arr);
				FillRemindConfirmData();
			}
		}

		private void butActivateReminder_Click(object sender,EventArgs e) {
			bool isApptRemindAutoEnabled = PrefC.GetBool(PrefName.ApptRemindAutoEnabled);
			isApptRemindAutoEnabled=!isApptRemindAutoEnabled;
			Prefs.UpdateBool(PrefName.ApptRemindAutoEnabled,isApptRemindAutoEnabled);
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Automated appointment eReminders "+(isApptRemindAutoEnabled ? "activated" : "deactivated")+".");
			Prefs.RefreshCache();
			Signalods.SetInvalid(InvalidType.Prefs);
			FillECRActivationButtons();
			//Add two default reminder rules if none exists.
			if(isApptRemindAutoEnabled && _dictClinicRules[0].Count(x => x.TypeCur==ApptReminderType.Reminder)==0) {
				ApptReminderRule arr=ApptReminderRules.CreateDefaultReminderRule(ApptReminderType.Reminder,0);//defaults to 3 hours before appt
				_dictClinicRules[0].Add(arr);
				ApptReminderRule arr2=ApptReminderRules.CreateDefaultReminderRule(ApptReminderType.Reminder,0);
				arr2.TSPrior=TimeSpan.FromDays(2);
				_dictClinicRules[0].Add(arr2);
				FillRemindConfirmData();
			}
		}
		
	}
}
