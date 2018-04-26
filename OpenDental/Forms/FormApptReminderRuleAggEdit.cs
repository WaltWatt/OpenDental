using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormApptReminderRuleAggEdit:ODForm {
		public ApptReminderRule ApptReminderRuleCur;

		public FormApptReminderRuleAggEdit(ApptReminderRule apptReminderCur) {
			InitializeComponent();
			Lan.F(this);
			//This needs to remain a shallow copy because FormEServicesECR is expecting shallow copy changes only. Making a new instance would break that.
			ApptReminderRuleCur=apptReminderCur;
		}

		private void FormApptReminderRuleEdit_Load(object sender,EventArgs e) {
			textSMSAggShared.Text=PIn.String(ApptReminderRuleCur.TemplateSMSAggShared);
			textSMSAggPerAppt.Text=PIn.String(ApptReminderRuleCur.TemplateSMSAggPerAppt);
			textEmailSubjAggShared.Text=PIn.String(ApptReminderRuleCur.TemplateEmailSubjAggShared);
			textEmailAggShared.Text=PIn.String(ApptReminderRuleCur.TemplateEmailAggShared);
			textEmailAggPerAppt.Text=PIn.String(ApptReminderRuleCur.TemplateEmailAggPerAppt);
			labelTags.Text=Lan.g(this,"Use the following replacement tags to customize messages: ")
				+string.Join(", ",ApptReminderRules.GetAvailableAggTags(ApptReminderRuleCur.TypeCur));
			if(ApptReminderRuleCur.TypeCur==ApptReminderType.PatientPortalInvite) {
				textSMSAggShared.Enabled=false;	
				textSMSAggPerAppt.Enabled=false;
				labelEmailAggPerAppt.Text+="  "+Lans.g(this,"Replaces the [Credentials] tag.");
			}
		}

		private bool ValidateRule() {
			List<string> errors = new List<string>();
			if(ApptReminderRuleCur.TypeCur!=ApptReminderType.PatientPortalInvite) {
				if(string.IsNullOrWhiteSpace(textSMSAggShared.Text)) {
					errors.Add(Lan.g(this,"Text message cannot be blank."));
				}
				if(!textSMSAggShared.Text.ToLower().Contains("[appts]")) {
					errors.Add(Lan.g(this,"Text message must contain the \"[Appts]\" tag."));
				}
				if(!textEmailAggShared.Text.ToLower().Contains("[appts]")) {
					errors.Add(Lan.g(this,"Email message must contain the \"[Appts]\" tag."));
				}
			}
			if(string.IsNullOrWhiteSpace(textEmailSubjAggShared.Text)) {
				errors.Add(Lan.g(this,"Email subject cannot be blank."));
			}
			if(string.IsNullOrWhiteSpace(textEmailAggShared.Text)) {
				errors.Add(Lan.g(this,"Email message cannot be blank."));
			}	
			if(ApptReminderRuleCur.TypeCur==ApptReminderType.ConfirmationFutureDay) {
				if(textEmailAggShared.Text.ToLower().Contains("[confirmcode]")) {
					errors.Add(Lan.g(this,"Confirmation emails should not contain the \"[ConfirmCode]\" tag."));
				}
				if(!textEmailAggShared.Text.ToLower().Contains("[confirmurl]")) {
					errors.Add(Lan.g(this,"Confirmation emails must contain the \"[ConfirmURL]\" tag."));
				}
			}
			if(errors.Count>0) {
				MessageBox.Show(Lan.g(this,"You must fix the following errors before continuing.")+"\r\n\r\n-"+string.Join("\r\n-",errors));
			}
			return errors.Count==0;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!ValidateRule()) {
				return;
			}
			ApptReminderRuleCur.TemplateSMSAggShared=textSMSAggShared.Text.Replace("[ConfirmURL].","[ConfirmURL] .");//Clicking a link with a period will not get recognized. 
			ApptReminderRuleCur.TemplateSMSAggPerAppt=textSMSAggPerAppt.Text;
			ApptReminderRuleCur.TemplateEmailSubjAggShared=textEmailSubjAggShared.Text;
			ApptReminderRuleCur.TemplateEmailAggShared=textEmailAggShared.Text;
			ApptReminderRuleCur.TemplateEmailAggPerAppt=textEmailAggPerAppt.Text;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}