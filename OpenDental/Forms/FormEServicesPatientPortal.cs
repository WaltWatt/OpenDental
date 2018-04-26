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
		private List<ApptReminderRule> _listPatPortalInviteRules;
		///<summary>The currently selected clinic for Patient Portal Invites.</summary>
		private Clinic _clinicCurPPInvite;

		private bool _useDefaultsPPInvite {
			get {
				if(_clinicCurPPInvite==null) {
					return true;
				}
				ClinicPref clinicPref=ClinicPrefs.GetPref(PrefName.PatientPortalInviteUseDefaults,_clinicCurPPInvite.ClinicNum);
				return clinicPref==null || clinicPref.ValueString=="1";
			}
			set {
				if(_clinicCurPPInvite==null) {
					return;
				}
				if(ClinicPrefs.Upsert(PrefName.PatientPortalInviteUseDefaults,_clinicCurPPInvite.ClinicNum,value ? "1": "0")) {
					ClinicPrefs.RefreshCache();
					_doSetInvalidClinicPrefs=true;
				}
			}
		}

		private bool _isClinicEnabledPPInvite {
			get {
				if(_clinicCurPPInvite==null) {
					return true;
				}
				ClinicPref clinicPref=ClinicPrefs.GetPref(PrefName.PatientPortalInviteEnabled,_clinicCurPPInvite.ClinicNum);
				return clinicPref!=null && clinicPref.ValueString=="1";
			}
			set {
				if(_clinicCurPPInvite==null) {
					return;
				}
				if(ClinicPrefs.Upsert(PrefName.PatientPortalInviteEnabled,_clinicCurPPInvite.ClinicNum,value ? "1" : "0")) {
					ClinicPrefs.RefreshCache();
					_doSetInvalidClinicPrefs=true;
				}
			}
		}

		private bool IsTabValidPatientPortal() {
#if !DEBUG
			if(!textPatientFacingUrlPortal.Text.ToUpper().StartsWith("HTTPS")) {
				MsgBox.Show(this,"Patient Facing URL must start with HTTPS.");
				return false;
			}
#endif
			if(textBoxNotificationSubject.Text=="") {
				MsgBox.Show(this,"Notification Subject is empty");
				textBoxNotificationSubject.Focus();
				return false;
			}
			if(textBoxNotificationBody.Text=="") {
				MsgBox.Show(this,"Notification Body is empty");
				textBoxNotificationBody.Focus();
				return false;
			}
			if(!textBoxNotificationBody.Text.Contains("[URL]")) { //prompt user that they omitted the URL field but don't prevent them from continuing
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"[URL] not included in notification body. Continue without setting the [URL] field?")) {
					textBoxNotificationBody.Focus();
					return false;
				}
			}
			return true;
		}

		private void FillTabPatientPortal() {
			//Office may have set a customer URL
			textPatientFacingUrlPortal.Text=PrefC.GetString(PrefName.PatientPortalURL);
			//HQ provides this URL for this customer.
			WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService urlsFromHQ=WebServiceMainHQProxy.GetSignups<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService>(_signupOut,eServiceCode.PatientPortal).FirstOrDefault()??new WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService() { HostedUrl="", HostedUrlPayment="" };
			textHostedUrlPortal.Text=urlsFromHQ.HostedUrl;
			textHostedUrlPortalPayment.Text=urlsFromHQ.HostedUrlPayment;
			if(textPatientFacingUrlPortal.Text=="") { //Customer has not set their own URL so use the URL provided by OD.
				textPatientFacingUrlPortal.Text=urlsFromHQ.HostedUrl;
			}
			textBoxNotificationSubject.Text=PrefC.GetString(PrefName.PatientPortalNotifySubject);
			textBoxNotificationBody.Text=PrefC.GetString(PrefName.PatientPortalNotifyBody);
			_listPatPortalInviteRules=ApptReminderRules.GetForTypes(ApptReminderType.PatientPortalInvite);
			if(PrefC.HasClinicsEnabled) {
				_clinicCurPPInvite=comboClinicsPPInvites.SelectedTag<Clinic>();
			}
			else {
				_clinicCurPPInvite=Clinics.GetPracticeAsClinicZero();
				labelClinicPPInvites.Visible=false;
				checkUseDefaultsPPInvites.Visible=false;
				checkIsPPInvitesEnabled.Visible=false;
			}
			FillPatPortalInvites();
		}

		private void FillPatPortalInvites() {			
			gridPatPortalInviteRules.BeginUpdate();
			gridPatPortalInviteRules.Columns.Clear();
			gridPatPortalInviteRules.Columns.Add(new ODGridColumn(Lan.g(this,"Send Time"),100));
			gridPatPortalInviteRules.Columns.Add(new ODGridColumn(Lan.g(this,"Templates"),200));
			gridPatPortalInviteRules.Rows.Clear();
			long clinicNum;
			if(_clinicCurPPInvite==null || _useDefaultsPPInvite) {
				clinicNum=0;//Use defaults
			}
			else {
				clinicNum=_clinicCurPPInvite.ClinicNum;
			}
			IEnumerable<ApptReminderRule> apptReminderRules=_listPatPortalInviteRules
				.Where(x => x.ClinicNum==clinicNum)
				.OrderBy(x => !x.IsEnabled)//Show enabled before disabled
				.ThenBy(x => x.TSPrior < TimeSpan.Zero)//Show Send Times that are before the appointment first, then show ones after the appointment
				.ThenBy(x => Math.Abs(x.TSPrior.TotalSeconds));//Show times from lowest to highest
			foreach(ApptReminderRule apptRule in apptReminderRules) {
				ODGridRow row=new ODGridRow();
				string sendTime;
				if(apptRule.TSPrior==TimeSpan.Zero) {
					sendTime=Lan.g(this,"Disabled");
				}
				else {
					sendTime=apptRule.TSPrior.ToStringDH();
				}
				if(apptRule.TSPrior > TimeSpan.Zero) {
					sendTime+="\r\n"+Lan.g(this,"before appt");
				}
				else
				if(apptRule.TSPrior < TimeSpan.Zero) {
					sendTime+="\r\n"+Lan.g(this,"after appt");
				}
				if(_clinicCurPPInvite.ClinicNum > 0 && _useDefaultsPPInvite) {
					sendTime+="\r\n"+Lan.g(this,"(Defaults)");
				}
				row.Cells.Add(sendTime);
				row.Cells.Add(Lan.g(this,"Email Subject Template")+":\r\n"+apptRule.TemplateEmailSubject+"\r\n"
					+Lan.g(this,"Email Template")+":\r\n"+apptRule.TemplateEmail);
				row.Tag=apptRule;
				if(gridPatPortalInviteRules.Rows.Count%2==1) {
					row.ColorBackG=Color.FromArgb(240,240,240);//light gray every other row.
				}
				gridPatPortalInviteRules.Rows.Add(row);
			}
			gridPatPortalInviteRules.EndUpdate();
			checkUseDefaultsPPInvites.Checked=_useDefaultsPPInvite;
			checkIsPPInvitesEnabled.Checked=_isClinicEnabledPPInvite;
			if(_clinicCurPPInvite.ClinicNum > 0) {
				checkUseDefaultsPPInvites.Visible=true;
				checkIsPPInvitesEnabled.Visible=true;
			}
			else {
				checkUseDefaultsPPInvites.Visible=false;
				checkIsPPInvitesEnabled.Visible=false;
			}
			FillPPInviteActivationButton();
		}

		private void butActivateInvites_Click(object sender,EventArgs e) {
			if(!WebServiceMainHQProxy.IsEServiceActive(_signupOut,eServiceCode.PatientPortal)) { //Not yet activated with HQ.
				MsgBox.Show(this,"You must first signup for Patient Portal via the Signup tab before activating Patient Portal Invites.");
				return;
			}
			bool isPatPortalInvitesEnabled=PrefC.GetBool(PrefName.PatientPortalInviteEnabled);
			isPatPortalInvitesEnabled=!isPatPortalInvitesEnabled;
			Prefs.UpdateBool(PrefName.PatientPortalInviteEnabled,isPatPortalInvitesEnabled);
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Patient Portal Invites "+(isPatPortalInvitesEnabled ? "activated" : "deactivated")+".");
			Prefs.RefreshCache();
			Signalods.SetInvalid(InvalidType.Prefs);
			FillPPInviteActivationButton();
			if(isPatPortalInvitesEnabled && _listPatPortalInviteRules.Count==0) {
				_listPatPortalInviteRules.Add(ApptReminderRules.CreateDefaultReminderRule(ApptReminderType.PatientPortalInvite,0,isBeforeAppointment: false));
				_listPatPortalInviteRules.Add(ApptReminderRules.CreateDefaultReminderRule(ApptReminderType.PatientPortalInvite,0,isBeforeAppointment: true));
				FillPatPortalInvites();
			}
		}

		private void FillPPInviteActivationButton() {
			if(PrefC.GetBool(PrefName.PatientPortalInviteEnabled)) {
				textStatusInvites.Text=Lan.g(this,"Invites")+" : "+Lan.g(this,"Active");
				textStatusInvites.BackColor=Color.FromArgb(236,255,236);//light green
				textStatusInvites.ForeColor=Color.Black;//instead of disabled grey
				butActivateInvites.Text=Lan.g(this,"Deactivate Invites");
			}
			else {
				textStatusInvites.Text=Lan.g(this,"Invites")+" : "+Lan.g(this,"Inactive");
				textStatusInvites.BackColor=Color.FromArgb(254,235,233);//light red;
				textStatusInvites.ForeColor=Color.Black;//instead of disabled grey
				butActivateInvites.Text=Lan.g(this,"Activate Invites");
			}
		}

		private void gridPatPortalInviteRules_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!Security.IsAuthorized(Permissions.EServicesSetup)) {
				return;
			}
			if(e.Row<0 || !(gridPatPortalInviteRules.Rows[e.Row].Tag is ApptReminderRule)) {
				return;//we did not click on a valid row.
			}
			if(_clinicCurPPInvite!=null && _clinicCurPPInvite.ClinicNum > 0 && _useDefaultsPPInvite && !SwitchFromDefaultsPPInvites()) {
				return;
			}
			ApptReminderRule arr=(ApptReminderRule)gridPatPortalInviteRules.Rows[e.Row].Tag;
			int idx=_listPatPortalInviteRules.IndexOf(arr);
			FormApptReminderRuleEdit FormARRE=new FormApptReminderRuleEdit(arr);
			FormARRE.ShowDialog();
			if(FormARRE.DialogResult!=DialogResult.OK) {
				return;
			}
			if(FormARRE.ApptReminderRuleCur==null) {//Delete
				_listPatPortalInviteRules.RemoveAt(idx);
			}
			else if(FormARRE.ApptReminderRuleCur.IsNew) {//Insert
				_listPatPortalInviteRules.Add(FormARRE.ApptReminderRuleCur);//should never happen from the double click event
			}
			else {//Update
				_listPatPortalInviteRules[idx]=FormARRE.ApptReminderRuleCur;
			}
			FillPatPortalInvites();
		}

		private void comboClinicsPPInvites_SelectionChangeCommitted(object sender,EventArgs e) {
			if(_listPatPortalInviteRules.Count==0) {
				return;
			}
			if(_clinicCurPPInvite!=null) {
				if(_clinicCurPPInvite.ClinicNum > 0 && checkIsPPInvitesEnabled.Checked!=_isClinicEnabledPPInvite) {
					_isClinicEnabledPPInvite=checkIsPPInvitesEnabled.Checked;
				}
				ApptReminderRules.SyncByClinicAndTypes(_listPatPortalInviteRules.FindAll(x => x.ClinicNum==_clinicCurPPInvite.ClinicNum),
					_clinicCurPPInvite.ClinicNum,ApptReminderType.PatientPortalInvite);
			}
			_clinicCurPPInvite=comboClinicsPPInvites.SelectedTag<Clinic>();
			FillPatPortalInvites();
		}

		private void checkUseDefaultsPPInvites_CheckedChanged(object sender,EventArgs e) {
			//TURNING DEFAULTS OFF
			if(!checkUseDefaultsPPInvites.Checked && _useDefaultsPPInvite && _clinicCurPPInvite.ClinicNum > 0) {//Default switched off
				SwitchFromDefaultsPPInvites();
				return;
			}
			//TURNING DEFAULTS ON
			else if(checkUseDefaultsPPInvites.Checked && !_useDefaultsPPInvite && _clinicCurPPInvite.ClinicNum > 0) {//Default switched on
				SwitchToDefaultsPPInvites();
				return;
			}
			//Silently do nothing because we just "changed" the checkbox to the state of the current clinic. 
		}

		private void butAddPPInviteRule_Click(object sender,EventArgs e) {
			if(_clinicCurPPInvite.ClinicNum > 0 && _useDefaultsPPInvite && !SwitchFromDefaultsPPInvites()) {
				return;
			}
			ApptReminderRule arr=ApptReminderRules.CreateDefaultReminderRule(ApptReminderType.PatientPortalInvite,_clinicCurPPInvite.ClinicNum);
			FormApptReminderRuleEdit FormARRE=new FormApptReminderRuleEdit(arr);
			FormARRE.ShowDialog();
			if(FormARRE.DialogResult!=DialogResult.OK) {
				return;
			}
			if(FormARRE.ApptReminderRuleCur==null || FormARRE.ApptReminderRuleCur.IsNew) {//Delete or Update
				//Nothing to delete or update, this was a new rule.
			}
			else {//Insert
				_listPatPortalInviteRules.Add(FormARRE.ApptReminderRuleCur);
			}
			FillPatPortalInvites();
		}

		///<summary>Switches the currently selected clinic over to using defaults. Also prompts user before continuing. 
		///Returns false if user cancelled or if there is no need to have switched to defaults.</summary>
		private bool SwitchFromDefaultsPPInvites() {
			if(_clinicCurPPInvite==null || _clinicCurPPInvite.ClinicNum==0) {
				return false;//somehow editing default clinic anyways, no need to switch.
			}
			_useDefaultsPPInvite=false;
			List<ApptReminderRule> listRulesThisClinic=_listPatPortalInviteRules.Where(x => x.ClinicNum==0)
				.Select(x => x.Copy()).ToList();
			listRulesThisClinic.ForEach(x => x.ClinicNum=_clinicCurPPInvite.ClinicNum);
			_listPatPortalInviteRules.AddRange(listRulesThisClinic);
			FillPatPortalInvites();
			return true;
		}

		///<summary>Switches the currently selected clinic over to using defaults. Also prompts user before continuing. 
		///Returns false if user cancelled or if there is no need to have switched to defaults.</summary>
		private bool SwitchToDefaultsPPInvites() {
			if(_clinicCurPPInvite==null || _clinicCurPPInvite.ClinicNum==0) {
				return false;//somehow editing default clinic anyways, no need to switch.
			}
			if(_listPatPortalInviteRules.Count(x => x.ClinicNum==_clinicCurPPInvite.ClinicNum) > 0 
				&& !MsgBox.Show(this,true,"Delete custom rules for this clinic and switch to using defaults? This cannot be undone.")) 
			{
				checkUseDefaultsPPInvites.Checked=false;//undo checking of box.
				return false;
			}
			_useDefaultsPPInvite=true;
			_listPatPortalInviteRules.RemoveAll(x => x.ClinicNum==_clinicCurPPInvite.ClinicNum);
			FillPatPortalInvites();
			return true;
		}

		private void SaveTabPatientPortal() {
			Prefs.UpdateString(PrefName.PatientPortalURL,textPatientFacingUrlPortal.Text);
			Prefs.UpdateString(PrefName.PatientPortalNotifySubject,textBoxNotificationSubject.Text);
			Prefs.UpdateString(PrefName.PatientPortalNotifyBody,textBoxNotificationBody.Text);
			ApptReminderRules.SyncByClinicAndTypes(_listPatPortalInviteRules.FindAll(x => x.ClinicNum==_clinicCurPPInvite.ClinicNum),
				_clinicCurPPInvite.ClinicNum,ApptReminderType.PatientPortalInvite);
			if(_clinicCurPPInvite.ClinicNum!=0) {
				_isClinicEnabledPPInvite=checkIsPPInvitesEnabled.Checked;
			}
		}

		private void AuthorizeTabPatientPortal(bool allowEdit) {
			groupBoxNotification.Enabled=allowEdit;
			textPatientFacingUrlPortal.Enabled=allowEdit;
		}
	}
}
