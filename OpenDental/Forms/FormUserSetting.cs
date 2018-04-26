using OpenDentBusiness;
using System;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental {
	/// <summary>
	/// This form (per Nathan) should be used for any future features that could be categorized as a user setting. The intent of this class was to
	/// create a place for specific user settings.
	/// </summary>
	public partial class FormUserSetting:ODForm {
		private UserOdPref _suppressLogOffMessage;
		public FormUserSetting() {
			InitializeComponent();
			Lan.F(this);
		}
		private void FormUserSetting_Load(object sender,EventArgs e) {
			_suppressLogOffMessage=UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum,UserOdFkeyType.SuppressLogOffMessage).FirstOrDefault();
			if(_suppressLogOffMessage!=null) {//Does exist in the database
				checkSuppressMessage.Checked=true;
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(checkSuppressMessage.Checked && _suppressLogOffMessage==null) {
				//insert
				UserOdPrefs.Insert(new UserOdPref() {
					UserNum=Security.CurUser.UserNum,
					FkeyType=UserOdFkeyType.SuppressLogOffMessage
				});
			}
			else if(!checkSuppressMessage.Checked && _suppressLogOffMessage!=null) {
				//delete
				UserOdPrefs.Delete(_suppressLogOffMessage.UserOdPrefNum);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}