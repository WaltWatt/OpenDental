using System;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormLoginFailed:ODForm {
		private string _errorMsg;

		///<summary></summary>
		public FormLoginFailed(string errorMessage) {
			InitializeComponent();
			_errorMsg=errorMessage;
		}

		private void FormLoginFailed_Load(object sender,EventArgs e) {
			labelErrMsg.Text=_errorMsg;
			textUser.Text=Security.CurUser.UserName;//CurUser verified to not be null in FormOpenDental before loading this form
			textPassword.Focus();
		}

		private void butLogin_Click(object sender,EventArgs e) {
			Userod userEntered;
			string password;
			try {
				bool useEcwAlgorithm=Programs.UsingEcwTightOrFullMode();
				//ecw requires hash, but non-ecw requires actual password
				password=textPassword.Text;
				if(useEcwAlgorithm) {
					//Userods.HashPassword explicitly goes over to middle tier in order to use it's MD5 algorithm.
					//It doesn't matter what Security.CurUser is when it is null because we are technically trying to set it for the first time.
					//It cannot be null before invoking HashPassword because middle needs it to NOT be null when creating the credentials for DtoGetString.
					if(Security.CurUser==null) {
						Security.CurUser=new Userod();
					}
					password=Userods.HashPassword(password,true);
				}
				string username=textUser.Text;
				#if DEBUG
				if(username=="") {
					username="Admin";
					password="pass";
				}
				#endif
				userEntered=Userods.CheckUserAndPassword(username,password,useEcwAlgorithm);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			//successful login.
			Security.CurUser=userEntered;
			Security.PasswordTyped=password;
			Security.IsUserLoggedIn=true;
			RemotingClient.HasLoginFailed=false;
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb 
				&& string.IsNullOrEmpty(userEntered.Password) 
				&& string.IsNullOrEmpty(textPassword.Text)) 
			{
				MsgBox.Show(this,"When using the web service, not allowed to log in with no password.  A password should be added for this user.");
				FormOpenDental FormOD=Application.OpenForms.OfType<FormOpenDental>().ToList()[0];//There always should be exactly 1.
				if(!FormOD.ChangePassword(true)) {//Failed password update.
					return;
				}
			}
			if(PrefC.GetBool(PrefName.PasswordsMustBeStrong)
				&& PrefC.GetBool(PrefName.PasswordsWeakChangeToStrong)
				&& Userods.IsPasswordStrong(textPassword.Text)!="") //Password is not strong
			{
				MsgBox.Show(this,"You must change your password to a strong password due to the current Security settings.");
				FormOpenDental FormOD=Application.OpenForms.OfType<FormOpenDental>().ToList()[0];//There always should be exactly 1.
				if(!FormOD.ChangePassword(true)) {//Failed password update.
					return;
				}
			}
			SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff,0,"User: "+Security.CurUser.UserNum+" has logged on.");
			DialogResult=DialogResult.OK;
		}

		private void butExit_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}