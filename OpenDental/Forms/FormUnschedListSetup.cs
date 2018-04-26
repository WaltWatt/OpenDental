using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormUnschedListSetup:ODForm {

		public FormUnschedListSetup() {
			InitializeComponent();
			Lan.F(this);
		}
		
		private void FormUnschedListSetup_Load(object sender,EventArgs e) {
			textDaysPast.Text=PrefC.GetLong(PrefName.UnschedDaysPast).ToString();
			textDaysFuture.Text=PrefC.GetLong(PrefName.UnschedDaysFuture).ToString();
		}

		private void butOK_Click(object sender,EventArgs e) {
			bool isPrefsInvalid=false;
			int uschedDaysPastValue=PIn.Int(textDaysPast.Text,false);
			isPrefsInvalid=Prefs.UpdateInt(PrefName.UnschedDaysPast,uschedDaysPastValue);
			int uschedDaysFutureValue=PIn.Int(textDaysFuture.Text,false);
			isPrefsInvalid=(isPrefsInvalid || Prefs.UpdateLong(PrefName.UnschedDaysFuture,uschedDaysFutureValue));
			if(isPrefsInvalid) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}