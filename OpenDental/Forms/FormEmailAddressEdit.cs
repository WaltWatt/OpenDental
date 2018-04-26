using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using System.Collections.Generic;

namespace OpenDental{
///<summary></summary>
	public class FormEmailAddressEdit:ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textSMTPserver;
		private System.Windows.Forms.TextBox textSender;
		private TextBox textUsername;
		private Label label3;
		private TextBox textPassword;
		private Label label4;
		private TextBox textPort;
		private Label label5;
		private Label label6;
		private CheckBox checkSSL;
		private Label label7;
		private System.ComponentModel.Container components = null;
		private UI.Button butDelete;
		public EmailAddress EmailAddressCur;
		private GroupBox groupOutgoing;
		private GroupBox groupIncoming;
		private TextBox textSMTPserverIncoming;
		private Label label8;
		private Label label10;
		private TextBox textPortIncoming;
		private Label label11;
		private Label label9;
		private Label label12;
		private UI.Button butRegisterCertificate;
		private Label label13;
		private Label label14;
		public bool IsNew;

		///<summary></summary>
		public FormEmailAddressEdit() {
			InitializeComponent();
			Lan.F(this);
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmailAddressEdit));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textSMTPserver = new System.Windows.Forms.TextBox();
			this.textSender = new System.Windows.Forms.TextBox();
			this.textUsername = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textPassword = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textPort = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.checkSSL = new System.Windows.Forms.CheckBox();
			this.label7 = new System.Windows.Forms.Label();
			this.butDelete = new OpenDental.UI.Button();
			this.groupOutgoing = new System.Windows.Forms.GroupBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.groupIncoming = new System.Windows.Forms.GroupBox();
			this.label14 = new System.Windows.Forms.Label();
			this.textSMTPserverIncoming = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.textPortIncoming = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.butRegisterCertificate = new OpenDental.UI.Button();
			this.groupOutgoing.SuspendLayout();
			this.groupIncoming.SuspendLayout();
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
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(509, 384);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 7;
			this.butCancel.Text = "&Cancel";
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
			this.butOK.Location = new System.Drawing.Point(428, 384);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 6;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(173, 20);
			this.label1.TabIndex = 2;
			this.label1.Text = "Outgoing SMTP Server";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 142);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(177, 20);
			this.label2.TabIndex = 3;
			this.label2.Text = "E-mail address of sender";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textSMTPserver
			// 
			this.textSMTPserver.Location = new System.Drawing.Point(185, 19);
			this.textSMTPserver.Name = "textSMTPserver";
			this.textSMTPserver.Size = new System.Drawing.Size(218, 20);
			this.textSMTPserver.TabIndex = 1;
			// 
			// textSender
			// 
			this.textSender.Location = new System.Drawing.Point(185, 142);
			this.textSender.Name = "textSender";
			this.textSender.Size = new System.Drawing.Size(218, 20);
			this.textSender.TabIndex = 3;
			// 
			// textUsername
			// 
			this.textUsername.Location = new System.Drawing.Point(197, 6);
			this.textUsername.Name = "textUsername";
			this.textUsername.Size = new System.Drawing.Size(218, 20);
			this.textUsername.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(97, 6);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(98, 20);
			this.label3.TabIndex = 0;
			this.label3.Text = "Username";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPassword
			// 
			this.textPassword.Location = new System.Drawing.Point(197, 26);
			this.textPassword.Name = "textPassword";
			this.textPassword.PasswordChar = '*';
			this.textPassword.Size = new System.Drawing.Size(218, 20);
			this.textPassword.TabIndex = 2;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(18, 26);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(177, 20);
			this.label4.TabIndex = 0;
			this.label4.Text = "Password";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPort
			// 
			this.textPort.Location = new System.Drawing.Point(185, 114);
			this.textPort.Name = "textPort";
			this.textPort.Size = new System.Drawing.Size(56, 20);
			this.textPort.TabIndex = 2;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(9, 114);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(174, 20);
			this.label5.TabIndex = 22;
			this.label5.Text = "Outgoing Port";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(247, 114);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(251, 20);
			this.label6.TabIndex = 0;
			this.label6.Text = "Usually 587.  Sometimes 25 or 465.";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkSSL
			// 
			this.checkSSL.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSSL.Location = new System.Drawing.Point(21, 47);
			this.checkSSL.Name = "checkSSL";
			this.checkSSL.Size = new System.Drawing.Size(190, 17);
			this.checkSSL.TabIndex = 3;
			this.checkSSL.Text = "Use SSL";
			this.checkSSL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSSL.UseVisualStyleBackColor = true;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(217, 48);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(251, 20);
			this.label7.TabIndex = 0;
			this.label7.Text = "For Gmail and some others";
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(12, 384);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(75, 24);
			this.butDelete.TabIndex = 8;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// groupOutgoing
			// 
			this.groupOutgoing.Controls.Add(this.label13);
			this.groupOutgoing.Controls.Add(this.label9);
			this.groupOutgoing.Controls.Add(this.textSMTPserver);
			this.groupOutgoing.Controls.Add(this.label1);
			this.groupOutgoing.Controls.Add(this.label2);
			this.groupOutgoing.Controls.Add(this.label6);
			this.groupOutgoing.Controls.Add(this.textSender);
			this.groupOutgoing.Controls.Add(this.textPort);
			this.groupOutgoing.Controls.Add(this.label5);
			this.groupOutgoing.Location = new System.Drawing.Point(12, 73);
			this.groupOutgoing.Name = "groupOutgoing";
			this.groupOutgoing.Size = new System.Drawing.Size(572, 180);
			this.groupOutgoing.TabIndex = 4;
			this.groupOutgoing.TabStop = false;
			this.groupOutgoing.Text = "Outgoing Email Settings";
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(187, 42);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(198, 69);
			this.label13.TabIndex = 0;
			this.label13.Text = "smtp.comcast.net\r\nmailhost.mycompany.com \r\nmail.mycompany.com\r\nsmtp.gmail.com\r\nor" +
    " similar...";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(404, 142);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(159, 20);
			this.label9.TabIndex = 0;
			this.label9.Text = "(not used in encrypted email)";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupIncoming
			// 
			this.groupIncoming.Controls.Add(this.label14);
			this.groupIncoming.Controls.Add(this.textSMTPserverIncoming);
			this.groupIncoming.Controls.Add(this.label8);
			this.groupIncoming.Controls.Add(this.label10);
			this.groupIncoming.Controls.Add(this.textPortIncoming);
			this.groupIncoming.Controls.Add(this.label11);
			this.groupIncoming.Location = new System.Drawing.Point(12, 259);
			this.groupIncoming.Name = "groupIncoming";
			this.groupIncoming.Size = new System.Drawing.Size(572, 116);
			this.groupIncoming.TabIndex = 5;
			this.groupIncoming.TabStop = false;
			this.groupIncoming.Text = "Incoming Email Settings";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(187, 42);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(198, 43);
			this.label14.TabIndex = 0;
			this.label14.Text = "pop.secureserver.net\r\npop.gmail.com\r\nor similar...";
			// 
			// textSMTPserverIncoming
			// 
			this.textSMTPserverIncoming.Location = new System.Drawing.Point(185, 19);
			this.textSMTPserverIncoming.Name = "textSMTPserverIncoming";
			this.textSMTPserverIncoming.Size = new System.Drawing.Size(218, 20);
			this.textSMTPserverIncoming.TabIndex = 1;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(12, 19);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(173, 20);
			this.label8.TabIndex = 0;
			this.label8.Text = "Incoming POP3 Server";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(247, 88);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(251, 20);
			this.label10.TabIndex = 0;
			this.label10.Text = "Usually 110.  Sometimes 995.";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textPortIncoming
			// 
			this.textPortIncoming.Location = new System.Drawing.Point(185, 88);
			this.textPortIncoming.Name = "textPortIncoming";
			this.textPortIncoming.Size = new System.Drawing.Size(56, 20);
			this.textPortIncoming.TabIndex = 2;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(9, 88);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(174, 20);
			this.label11.TabIndex = 0;
			this.label11.Text = "Incoming Port";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(416, 6);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(178, 20);
			this.label12.TabIndex = 0;
			this.label12.Text = "(full email address)";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butRegisterCertificate
			// 
			this.butRegisterCertificate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRegisterCertificate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butRegisterCertificate.Autosize = true;
			this.butRegisterCertificate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRegisterCertificate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRegisterCertificate.CornerRadius = 4F;
			this.butRegisterCertificate.Location = new System.Drawing.Point(197, 384);
			this.butRegisterCertificate.Name = "butRegisterCertificate";
			this.butRegisterCertificate.Size = new System.Drawing.Size(122, 24);
			this.butRegisterCertificate.TabIndex = 9;
			this.butRegisterCertificate.Text = "Certificate";
			this.butRegisterCertificate.Click += new System.EventHandler(this.butRegisterCertificate_Click);
			// 
			// FormEmailAddressEdit
			// 
			this.AcceptButton = this.butOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(600, 424);
			this.Controls.Add(this.butRegisterCertificate);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.groupIncoming);
			this.Controls.Add(this.groupOutgoing);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.checkSSL);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.textUsername);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textPassword);
			this.Controls.Add(this.label3);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(616, 463);
			this.MinimizeBox = false;
			this.Name = "FormEmailAddressEdit";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Email Address";
			this.Load += new System.EventHandler(this.FormEmailAddress_Load);
			this.groupOutgoing.ResumeLayout(false);
			this.groupOutgoing.PerformLayout();
			this.groupIncoming.ResumeLayout(false);
			this.groupIncoming.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormEmailAddress_Load(object sender, System.EventArgs e) {
			if(EmailAddressCur!=null) {
				textSMTPserver.Text=EmailAddressCur.SMTPserver;
				textUsername.Text=EmailAddressCur.EmailUsername;
				if(!String.IsNullOrEmpty(EmailAddressCur.EmailPassword)) { //can happen if creating a new user email.
					textPassword.Text=MiscUtils.Decrypt(EmailAddressCur.EmailPassword);
				}
				textPort.Text=EmailAddressCur.ServerPort.ToString();
				checkSSL.Checked=EmailAddressCur.UseSSL;
				textSender.Text=EmailAddressCur.SenderAddress;
				textSMTPserverIncoming.Text=EmailAddressCur.Pop3ServerIncoming;
				textPortIncoming.Text=EmailAddressCur.ServerPortIncoming.ToString();
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(EmailAddressCur.EmailAddressNum==PrefC.GetLong(PrefName.EmailDefaultAddressNum)) {
				MsgBox.Show(this,"Cannot delete the default email address.");
				return;
			}
			if(EmailAddressCur.EmailAddressNum==PrefC.GetLong(PrefName.EmailNotifyAddressNum)) {
				MsgBox.Show(this,"Cannot delete the notify email address.");
				return;
			}
			Clinic clinic=Clinics.GetFirstOrDefault(x => x.EmailAddressNum==EmailAddressCur.EmailAddressNum);
			if(clinic!=null) {
				MessageBox.Show(Lan.g(this,"Cannot delete the email address because it is used by clinic")+" "+clinic.Description);
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete this email address?")) {
				return;
			}
			EmailAddresses.Delete(EmailAddressCur.EmailAddressNum);
			DialogResult=DialogResult.OK;//OK triggers a refresh for the grid.
		}

		private void butRegisterCertificate_Click(object sender,EventArgs e) {
			FormEmailCertRegister form=new FormEmailCertRegister(textUsername.Text);
			form.ShowDialog();
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			try {
				PIn.Int(textPort.Text);
			}
			catch {
				MsgBox.Show(this,"Invalid outgoing port number.");
				return;
			}
			try {
				PIn.Int(textPortIncoming.Text);
			}
			catch {
				MsgBox.Show(this,"Invalid incoming port number.");
				return;
			}
			if(string.IsNullOrWhiteSpace(textUsername.Text) 
				|| !textUsername.Text.Contains("@"))//super basic validation
			{
				MsgBox.Show(this,"Please enter a valid email address.");
				return;
			}
			if(EmailAddresses.AddressExists(textUsername.Text,EmailAddressCur.EmailAddressNum)) {
				MsgBox.Show(this,"This email address already exists.");
				return;
			}
			EmailAddressCur.SMTPserver=PIn.String(textSMTPserver.Text);
			EmailAddressCur.EmailUsername=PIn.String(textUsername.Text);
			EmailAddressCur.EmailPassword=PIn.String(MiscUtils.Encrypt(textPassword.Text));
			EmailAddressCur.ServerPort=PIn.Int(textPort.Text);
			EmailAddressCur.UseSSL=checkSSL.Checked;
			EmailAddressCur.SenderAddress=PIn.String(textSender.Text);
			EmailAddressCur.Pop3ServerIncoming=PIn.String(textSMTPserverIncoming.Text);
			EmailAddressCur.ServerPortIncoming=PIn.Int(textPortIncoming.Text);
			if(IsNew) {
				EmailAddresses.Insert(EmailAddressCur);
			}
			else {
				EmailAddresses.Update(EmailAddressCur);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}
