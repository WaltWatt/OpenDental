using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	///<summary>In-memory form. Changes are not saved to the DB from this form.</summary>
	public partial class FormApptReminderRuleEdit:ODForm {
		public ApptReminderRule ApptReminderRuleCur;
		private List<CommType> _sendOrder;
		private List<ApptReminderRule> _listRulesClinic;

		///<summary>True if any preferences were updated.</summary>
		public bool IsPrefsChanged {
			get;
			private set;
		}

		public FormApptReminderRuleEdit(ApptReminderRule apptReminderCur,List<ApptReminderRule> listRulesClinic=null) {
			InitializeComponent();
			Lan.F(this);
			ApptReminderRuleCur=apptReminderCur;
			_listRulesClinic=listRulesClinic??new List<ApptReminderRule>();
		}

		private void FormApptReminderRuleEdit_Load(object sender,EventArgs e) {
			switch(ApptReminderRuleCur.TypeCur) {
				case ApptReminderType.Reminder:
					Text=Lan.g(this,"Edit eReminder Rule");
					break;
				case ApptReminderType.ConfirmationFutureDay:
					Text=Lan.g(this,"Edit eConfirmation Rule");
					break;
				case ApptReminderType.PatientPortalInvite:
					Text=Lan.g(this,"Edit Patient Portal Invite Rule");
					break;
				default:
					Text=Lan.g(this,"Edit Rule");
					break;
			}
			checkEnabled.Checked=ApptReminderRuleCur.IsEnabled;
			labelRuleType.Text=ApptReminderRuleCur.TypeCur.GetDescription();
			labelTags.Text=Lan.g(this,"Use the following replacement tags to customize messages : ")+string.Join(", ",ApptReminderRules.GetAvailableTags(ApptReminderRuleCur.TypeCur));
			//replacementTags.ForEach(x => listBoxTags.Items.Add(x));
			textTemplateSms.Text=ApptReminderRuleCur.TemplateSMS;
			textTemplateEmail.Text=ApptReminderRuleCur.TemplateEmail;
			if(ApptReminderRuleCur.TypeCur==ApptReminderType.PatientPortalInvite) {
				textTemplateSms.Enabled=false;
				checkSendAll.Visible=false;
			}
			_sendOrder=ApptReminderRuleCur.SendOrder.Split(',').Select(x => (CommType)PIn.Int(x)).ToList();
			FillGridPriority();
			FillTimeSpan();
			textTemplateSubject.Text=ApptReminderRuleCur.TemplateEmailSubject;
			checkSendAll.Checked=ApptReminderRuleCur.IsSendAll;
			textHours.errorProvider1.SetIconAlignment(textHours,ErrorIconAlignment.MiddleLeft);
			textDays.errorProvider1.SetIconAlignment(textDays,ErrorIconAlignment.MiddleLeft);
		}

		private void FillTimeSpan() {
			textHours.Text=Math.Abs(ApptReminderRuleCur.TSPrior.Hours).ToString();//Hours, not total hours.
			textDays.Text=Math.Abs(ApptReminderRuleCur.TSPrior.Days).ToString();//Days, not total Days.
			if(ApptReminderRuleCur.TSPrior>=TimeSpan.Zero) {
				radioBeforeAppt.Checked=true;
			}
			else {
				radioAfterAppt.Checked=true;
			}
			if(ApptReminderRuleCur.TypeCur!=ApptReminderType.PatientPortalInvite) {
				radioAfterAppt.Enabled=false;
			}
			if(ApptReminderRuleCur.DoNotSendWithin.Days > 0) {
				textDaysWithin.Text=ApptReminderRuleCur.DoNotSendWithin.Days.ToString();
			}
			if(ApptReminderRuleCur.DoNotSendWithin.Hours > 0) {
				textHoursWithin.Text=ApptReminderRuleCur.DoNotSendWithin.Hours.ToString();
			}
			UpdateDoNotSendWithinLabel();
		}

		private void FillGridPriority() {
			gridPriorities.BeginUpdate();
			gridPriorities.Columns.Clear();
			gridPriorities.Columns.Add(new ODGridColumn("",0));
			gridPriorities.Rows.Clear();
			for(int i = 0;i<_sendOrder.Count;i++) {
				CommType typeCur = _sendOrder[i];
				if(typeCur==CommType.Preferred) {
					if(checkSendAll.Checked) {
						//"Preferred" is irrelevant when SendAll is checked.
						continue;
					}
					gridPriorities.AddRow(Lan.g(this,"Preferred Confirm Method"));
					continue;
				}
				if(typeCur==CommType.Text && !SmsPhones.IsIntegratedTextingEnabled()) {
					gridPriorities.AddRow(Lan.g(this,typeCur.ToString())+" ("+Lan.g(this,"Not Configured")+")");
					gridPriorities.Rows[gridPriorities.Rows.Count-1].ColorBackG=Color.LightGray;
				}
				else {
					gridPriorities.AddRow(Lan.g(this,typeCur.ToString()));
				}
			}
			gridPriorities.EndUpdate();
		}

		private void radioBeforeAfterAppt_CheckedChanged(object sender,EventArgs e) {
			labelDoNotSendWithin.Enabled=radioBeforeAppt.Checked;
			labelDaysWithin.Enabled=radioBeforeAppt.Checked;
			labelHoursWithin.Enabled=radioBeforeAppt.Checked;
			textDaysWithin.Enabled=radioBeforeAppt.Checked;
			textHoursWithin.Enabled=radioBeforeAppt.Checked;
		}

		private void textDoNotSendWithin_TextChanged(object sender,EventArgs e) {
			UpdateDoNotSendWithinLabel();
		}

		private void UpdateDoNotSendWithinLabel() {
			string daysHoursTxt="";
			int daysWithin=PIn.Int(textDaysWithin.Text,false);
			int hoursWithin=PIn.Int(textHoursWithin.Text,false);
			if(!textDaysWithin.IsValid || !textHoursWithin.IsValid
				|| (daysWithin==0 && hoursWithin==0)) 
			{
				daysHoursTxt="_____________";
			}
			else {
				if(daysWithin==1) {
					daysHoursTxt+=daysWithin+" "+Lans.g(this,"day");
				}
				else if(daysWithin > 1) {
					daysHoursTxt+=daysWithin+" "+Lans.g(this,"days");
				}
				if(daysWithin > 0 && hoursWithin > 0) {
					daysHoursTxt+=" ";
				}
				if(hoursWithin==1) {
					daysHoursTxt+=hoursWithin+" "+Lans.g(this,"hour");
				}
				else if(hoursWithin > 1) {
					daysHoursTxt+=hoursWithin+" "+Lans.g(this,"hours");
				}
			}
			labelDoNotSendWithin.Text=Lans.g(this,"Do not send within")+" "+daysHoursTxt+" "+Lans.g(this,"of appointment");
		}

		private void butUp_Click(object sender,EventArgs e) {
			int idx = gridPriorities.GetSelectedIndex();
			if(idx<1) {
				//-1 if nothing selected. 0 if top item selected.
				return;
			}
			_sendOrder.Reverse(idx-1,2);
			FillGridPriority();
			gridPriorities.SetSelected(idx-1,true);
		}

		private void butDown_Click(object sender,EventArgs e) {
			int idx = gridPriorities.GetSelectedIndex();
			if(idx==-1 || idx==_sendOrder.Count-1) {
				//-1 nothing selected. Count-1 if last item selected.
				return;
			}
			_sendOrder.Reverse(idx,2);
			FillGridPriority();
			gridPriorities.SetSelected(idx+1,true);
		}

		private void butAdvanced_Click(object sender,EventArgs e) {
			FormApptReminderRuleAggEdit formAddEdit=new FormApptReminderRuleAggEdit(ApptReminderRuleCur);
			formAddEdit.ShowDialog();
			if(formAddEdit.DialogResult==DialogResult.Cancel) {
				//since we don't make a deep copy of the ApptReminderRuleCur in the advanced tap, this is the simple (lazy) way to undo any changes we've made
				DialogResult=DialogResult.Cancel; 
			}
		}

		///<summary>Removes 'Do not send eConfirmations' from the confirmed status for 'eConfirm Sent' if multiple eConfirmations are set up.</summary>
		private void CheckMultipleEConfirms() {
			int countEConfirm=_listRulesClinic?.Count(x => x.TypeCur==ApptReminderType.ConfirmationFutureDay)??0;
			string confStatusEConfirmSent=Defs.GetDef(DefCat.ApptConfirmed,PrefC.GetLong(PrefName.ApptEConfirmStatusSent)).ItemName;
			List<string> listExclude=PrefC.GetString(PrefName.ApptConfirmExcludeESend)
				.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries).ToList();
			if(ApptReminderRuleCur.TypeCur==ApptReminderType.ConfirmationFutureDay
				//And there is more than 1 eConfirmation rule.
				&& (countEConfirm > 1 || (countEConfirm==1 && ApptReminderRuleCur.ApptReminderRuleNum==0))
				//And the confirmed status for 'eConfirm Sent' is marked 'Do not send eConfirmations'
				&& listExclude.Contains(PrefC.GetString(PrefName.ApptEConfirmStatusSent))
				//Ask them to fix their exclude send statuses
				&& MessageBox.Show(Lans.g(this,"Appointments will not receive multiple eConfirmations if the '")+confStatusEConfirmSent+"' "+
						Lans.g(this,"status is set as 'Don't Send'. Would you like to remove 'Don't Send' from that status?"),
					"",MessageBoxButtons.YesNo)==DialogResult.Yes) 
			{
				listExclude.RemoveAll(x => x==PrefC.GetString(PrefName.ApptEConfirmStatusSent));
				IsPrefsChanged|=Prefs.UpdateString(PrefName.ApptConfirmExcludeESend,string.Join(",",listExclude));
			}
		}

		private void butOk_Click(object sender,EventArgs e) {
			if(!textHours.IsValid	|| !textDays.IsValid || !textHoursWithin.IsValid || !textDaysWithin.IsValid) {
				MsgBox.Show(this,"Fix data entry errors first.");
				return;
			}
			if(!ValidateRule()) {
				return;
			}
			TimeSpan tsPrior;
			if(checkEnabled.Checked) {
				if(radioBeforeAppt.Checked) {
					tsPrior=new TimeSpan(PIn.Int(textDays.Text,false),PIn.Int(textHours.Text,false),0,0);
				}
				else {
					tsPrior=new TimeSpan(-PIn.Int(textDays.Text,false),-PIn.Int(textHours.Text,false),0,0);
				}
				if(_listRulesClinic.Any(x => x.TypeCur!=ApptReminderRuleCur.TypeCur && x.TSPrior==tsPrior)
					&& !MsgBox.Show(this,MsgBoxButtons.OKCancel,"There are multiple rules for sending at this send time. Are you sure you want to send multiple "
					+"messages at the same time?")) 
				{
					return;
				}
			}
			else {
				tsPrior=TimeSpan.Zero;
			}
			CheckMultipleEConfirms();
			ApptReminderRuleCur.TemplateSMS=textTemplateSms.Text.Replace("[ConfirmURL].","[ConfirmURL] .");//Clicking a link with a period will not get recognized. 
			ApptReminderRuleCur.TemplateEmailSubject=textTemplateSubject.Text;
			ApptReminderRuleCur.TemplateEmail=textTemplateEmail.Text;
			ApptReminderRuleCur.SendOrder=string.Join(",",_sendOrder.Select(x => ((int)x).ToString()).ToArray());
			ApptReminderRuleCur.IsSendAll=checkSendAll.Checked;
			ApptReminderRuleCur.TSPrior=tsPrior;
			if(radioBeforeAppt.Checked) {
				ApptReminderRuleCur.DoNotSendWithin=new TimeSpan(PIn.Int(textDaysWithin.Text,false),PIn.Int(textHoursWithin.Text,false),0,0);
			}
			DialogResult=DialogResult.OK;
		}

		private bool ValidateRule() {
			if(!checkEnabled.Checked) {
				return true;
			}
			List<string> errors = new List<string>();
			if(ApptReminderRuleCur.TypeCur!=ApptReminderType.PatientPortalInvite) {
				if(string.IsNullOrWhiteSpace(textTemplateSms.Text)) {
					errors.Add(Lan.g(this,"Text message cannot be blank."));
				}
			}
			if(string.IsNullOrWhiteSpace(textTemplateSubject.Text)) {
				errors.Add(Lan.g(this,"Email subject cannot be blank."));
			}
			if(string.IsNullOrWhiteSpace(textTemplateEmail.Text)) {
				errors.Add(Lan.g(this,"Email message cannot be blank."));
			}
			if(PIn.Int(textDays.Text,false)>366) {
				errors.Add(Lan.g(this,"Lead time must 365 days or less."));
			}
			if(checkEnabled.Checked && PIn.Int(textHours.Text,false)==0 && PIn.Int(textDays.Text,false)==0) {
				errors.Add(Lan.g(this,"Lead time must be greater than 0 hours."));
			}
			if(ApptReminderRuleCur.TypeCur==ApptReminderType.ConfirmationFutureDay) {
				if(PIn.Int(textDays.Text,false)==0) {
					errors.Add(Lan.g(this,"Lead time must 1 day or more for confirmations."));
				}
				if(!textTemplateSms.Text.ToLower().Contains("[confirmcode]")) {
					errors.Add(Lan.g(this,"Confirmation texts must contain the \"[ConfirmCode]\" tag."));
				}
				if(textTemplateEmail.Text.ToLower().Contains("[confirmcode]")) {
					errors.Add(Lan.g(this,"Confirmation emails should not contain the \"[ConfirmCode]\" tag."));
				}
				if(!textTemplateEmail.Text.ToLower().Contains("[confirmurl]")) {
					errors.Add(Lan.g(this,"Confirmation emails must contain the \"[ConfirmURL]\" tag."));
				}
			}
			if(radioBeforeAppt.Checked) {
				TimeSpan tsPrior=new TimeSpan(PIn.Int(textDays.Text,false),PIn.Int(textHours.Text,false),0,0);
				TimeSpan doNotSendWithin=new TimeSpan(PIn.Int(textDaysWithin.Text,false),PIn.Int(textHoursWithin.Text,false),0,0);
				if(doNotSendWithin >= tsPrior) {
					errors.Add(Lan.g(this,"'Send Time' must be greater than 'Do Not Send Within' time."));
				}
			}
			if(errors.Count>0) {
				MessageBox.Show(Lan.g(this,"You must fix the following errors before continuing.")+"\r\n\r\n-"+string.Join("\r\n-",errors));
				return false;
			}
			return true;
		}

		private void checkSendAll_CheckedChanged(object sender,EventArgs e) {
			butUp.Enabled=!checkSendAll.Checked;
			butDown.Enabled=!checkSendAll.Checked;
			gridPriorities.Enabled=!checkSendAll.Checked;
			gridPriorities.SetSelected(false);
			FillGridPriority();
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			ApptReminderRuleCur=null;
			DialogResult=DialogResult.OK;
		}

	}
}
