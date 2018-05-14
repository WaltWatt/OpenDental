using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormLogOn : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.ListBox listUser;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textPassword;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private TextBox textUser;
		private CheckBox checkShowCEMTUsers;
		private List<Userod> _listUsers;
		///<summary>Used when temporarily switching users. Currently only used when overriding signed notes.
		///The user will not be logged on (Security.CurUser is untouched) but CurUserSimpleSwitch will be set to the desired user.</summary>
		public bool IsSimpleSwitch=false;
		public Userod CurUserSimpleSwitch;
		///<summary>If set AND available, this will be the user selected when the form opens.</summary>
		public long UserNumPrompt;

		///<summary></summary>
		public FormLogOn()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Plugins.HookAddCode(this,"FormLogOn.InitializeComponent_end");
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogOn));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.listUser = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textPassword = new System.Windows.Forms.TextBox();
			this.textUser = new System.Windows.Forms.TextBox();
			this.checkShowCEMTUsers = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(361, 321);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "Exit";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(361, 280);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// listUser
			// 
			this.listUser.Location = new System.Drawing.Point(31, 31);
			this.listUser.Name = "listUser";
			this.listUser.Size = new System.Drawing.Size(141, 316);
			this.listUser.TabIndex = 2;
			this.listUser.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listUser_MouseUp);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(30, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(122, 18);
			this.label1.TabIndex = 6;
			this.label1.Text = "User";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(188, 10);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(122, 18);
			this.label2.TabIndex = 7;
			this.label2.Text = "Password";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textPassword
			// 
			this.textPassword.Location = new System.Drawing.Point(189, 31);
			this.textPassword.Name = "textPassword";
			this.textPassword.PasswordChar = '*';
			this.textPassword.Size = new System.Drawing.Size(215, 20);
			this.textPassword.TabIndex = 0;
			// 
			// textUser
			// 
			this.textUser.Location = new System.Drawing.Point(31, 31);
			this.textUser.Name = "textUser";
			this.textUser.Size = new System.Drawing.Size(141, 20);
			this.textUser.TabIndex = 8;
			this.textUser.Visible = false;
			// 
			// checkShowCEMTUsers
			// 
			this.checkShowCEMTUsers.Location = new System.Drawing.Point(191, 68);
			this.checkShowCEMTUsers.Name = "checkShowCEMTUsers";
			this.checkShowCEMTUsers.Size = new System.Drawing.Size(213, 24);
			this.checkShowCEMTUsers.TabIndex = 9;
			this.checkShowCEMTUsers.Text = "Show CEMT users";
			this.checkShowCEMTUsers.UseVisualStyleBackColor = true;
			this.checkShowCEMTUsers.Visible = false;
			this.checkShowCEMTUsers.CheckedChanged += new System.EventHandler(this.checkShowCEMTUsers_CheckedChanged);
			// 
			// FormLogOn
			// 
			this.AcceptButton = this.butOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(464, 378);
			this.Controls.Add(this.checkShowCEMTUsers);
			this.Controls.Add(this.textUser);
			this.Controls.Add(this.textPassword);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.listUser);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "FormLogOn";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Log On";
			this.Load += new System.EventHandler(this.FormLogOn_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormLogOn_Load(object sender, System.EventArgs e) {
			TextBox textSelectOnLoad=textPassword;
			if(PrefC.GetBool(PrefName.UserNameManualEntry)) {
				listUser.Visible=false;
				textUser.Visible=true;
				textSelectOnLoad=textUser;//Focus should start with user name text box.
			}
			else {//Show a list of users.
				//Only show the show CEMT user check box if not manually typing user names and there are CEMT users present in the db.
				if(Userods.GetUsersForCEMT().Count>0) {
					checkShowCEMTUsers.Visible=true;
				}
			}
			FillListBox();
			this.Focus();//Attempted fix, customers had issue with UI not defaulting focus to this form on startup.
			textSelectOnLoad.Select();//Give focus to appropriate text box.
		}

		private void listUser_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
			textPassword.Focus();
		}

		private void butResetPassword_Click(object sender, System.EventArgs e) {
			//This feature was never used, so was removed.
			//FormPasswordReset FormPR=new FormPasswordReset();
			//FormPR.ShowDialog();
		}

		private void FillListBox(){
			Userods.RefreshCache();
			UserGroups.RefreshCache();
			GroupPermissions.RefreshCache();
			listUser.BeginUpdate();
			listUser.Items.Clear();
			if(PrefC.GetBool(PrefName.UserNameManualEntry)) {
				//Because _listUsers is used to verify the user name typed in, we need to include both non-hidden and CEMT users for offices that type in their credentials instead of picking.
				_listUsers=Userods.GetUsers(true);
			}
			else if(checkShowCEMTUsers.Checked) {//Only show list of CEMT users.
				_listUsers=Userods.GetUsersForCEMT().Where(x => !x.IsHidden).ToList();
			}
			else {//This will be the most common way to fill the user list.  Only includes non-hidden, non-CEMT users.
				_listUsers=Userods.GetUsers();
			}
			_listUsers.ForEach(x => listUser.Items.Add(x));
			if(UserNumPrompt>0) {
				listUser.SelectedIndex=_listUsers.FindIndex(x => x.UserNum==UserNumPrompt);//can be -1 if not found
			}
			else if(Security.CurUser!=null) {
				listUser.SelectedIndex=_listUsers.FindIndex(x => x.UserNum==Security.CurUser.UserNum);//can be -1 if not found
			}
			if(listUser.SelectedIndex==-1 && listUser.Items.Count>0){//It is possible there are no users in the list if all users are CEMT users.
				listUser.SelectedIndex=0;
			}
			listUser.EndUpdate();
		}

		private void checkShowCEMTUsers_CheckedChanged(object sender,EventArgs e) {
			FillListBox();
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			bool usingEcw=Programs.UsingEcwTightOrFullMode();
			Userod selectedUser=null;
			if(PrefC.GetBool(PrefName.UserNameManualEntry)) {
				for(int i=0;i<listUser.Items.Count;i++) {
					//Check the user name typed in using ToLower and Trim because Open Dental is case insensitive and does not allow white-space in regards to user names.
					if(textUser.Text.Trim().ToLower()==listUser.Items[i].ToString().Trim().ToLower()) {
						selectedUser=(Userod)listUser.Items[i];//Found the typed username
						break;
					}
				}
				if(selectedUser==null) {
					MsgBox.Show(this,"Login failed");
					return;
				}
			}
			else {
				selectedUser=(Userod)listUser.SelectedItem;
			}
			string password=textPassword.Text;
			if(usingEcw) {//ecw requires hash, but non-ecw requires actual password
				password=Userods.HashPassword(password,true);
			}
			if(selectedUser.UserName=="Stay Open" && IsSimpleSwitch && PrefC.IsODHQ) {
				// No need to check password when changing task users at HQ to user "Stay Open".
			}
			else {
				try {
					Userods.CheckUserAndPassword(selectedUser.UserName,password,usingEcw);
				}
				catch(Exception ex) {
					MessageBox.Show(ex.Message);
					return;
				}
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb && selectedUser.Password=="" && textPassword.Text=="") {
				MsgBox.Show(this,"When using the web service, not allowed to log in with no password.  A password should be added for this user.");
				return;
			}
			//successful login.
			if(!IsSimpleSwitch) {
				Security.CurUser=selectedUser.Copy();
				Security.IsUserLoggedIn=true;
				//Jason approved always storing the cleartext password that the user typed in 
				//since this is necessary for Reporting Servers over middle tier and was already happening when a user logged in over middle tier.
				Security.PasswordTyped=password;
				if(PrefC.GetBool(PrefName.PasswordsMustBeStrong) && PrefC.GetBool(PrefName.PasswordsWeakChangeToStrong)){
					if(Userods.IsPasswordStrong(textPassword.Text)!="") {//Password is not strong
						MsgBox.Show(this,"You must change your password to a strong password due to the current Security settings.");
						FormOpenDental FormOD=Application.OpenForms.OfType<FormOpenDental>().ToList()[0];//There always should be exactly 1.
						if(!FormOD.ChangePassword(true)) {//Failed password update.
							return;
						}
					}
				}
			}
			else {
				CurUserSimpleSwitch=selectedUser.Copy();
			}
			if(!IsSimpleSwitch) {
				SecurityLogs.MakeLogEntry(Permissions.UserLogOnOff,0,"User: "+Security.CurUser.UserName+" has logged on.");
			}
			Plugins.HookAddCode(this,"FormLogOn.butOK_Click_end");
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
		
		


	}

	


}





















