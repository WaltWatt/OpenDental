using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormFHIRAPIKeyEdit:ODForm {
		public APIKey APIKeyCur;

		public FormFHIRAPIKeyEdit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormFHIRAPIKeyEdit_Load(object sender,EventArgs e) {
			textKey.Text=APIKeyCur.Key;
			textStatus.Text=Lan.g("enumFHIRAPIKeyStatus",APIKeyCur.KeyStatus.ToString());
			textName.Text=APIKeyCur.DeveloperName;
			textEmail.Text=APIKeyCur.DeveloperEmail;
			textPhone.Text=APIKeyCur.DeveloperPhone;
			if(APIKeyCur.DateDisabled.Year>1880) {
				textDateDisabled.Text=APIKeyCur.DateDisabled.ToShortDateString();
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textName.Text.Trim()=="") {
				MsgBox.Show(this,"Developer name required.");
				return;
			}
			if(textEmail.Text.Trim()=="" && textPhone.Text.Trim()=="") {
				MsgBox.Show(this,"At least one form of contact must be entered.");
				return;
			}
			APIKeyCur.DeveloperName=textName.Text;
			APIKeyCur.DeveloperEmail=textEmail.Text;
			APIKeyCur.DeveloperPhone=textPhone.Text;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}