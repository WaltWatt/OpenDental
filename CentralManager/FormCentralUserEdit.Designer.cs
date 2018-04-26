namespace CentralManager {
	partial class FormCentralUserEdit {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.checkIsHidden = new System.Windows.Forms.CheckBox();
			this.butPassword = new OpenDental.UI.Button();
			this.textUserName = new System.Windows.Forms.TextBox();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.listUserGroup = new System.Windows.Forms.ListBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabUser = new System.Windows.Forms.TabPage();
			this.tabAlertSubs = new System.Windows.Forms.TabPage();
			this.listAlertSubMulti = new System.Windows.Forms.ListBox();
			this.label7 = new System.Windows.Forms.Label();
			this.butUnlock = new OpenDental.UI.Button();
			this.tabControl.SuspendLayout();
			this.tabUser.SuspendLayout();
			this.tabAlertSubs.SuspendLayout();
			this.SuspendLayout();
			// 
			// checkIsHidden
			// 
			this.checkIsHidden.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsHidden.Location = new System.Drawing.Point(12, 5);
			this.checkIsHidden.Name = "checkIsHidden";
			this.checkIsHidden.Size = new System.Drawing.Size(103, 16);
			this.checkIsHidden.TabIndex = 33;
			this.checkIsHidden.Text = "Is Hidden";
			this.checkIsHidden.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsHidden.UseVisualStyleBackColor = true;
			// 
			// butPassword
			// 
			this.butPassword.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPassword.Autosize = true;
			this.butPassword.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPassword.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPassword.CornerRadius = 4F;
			this.butPassword.Location = new System.Drawing.Point(12, 309);
			this.butPassword.Name = "butPassword";
			this.butPassword.Size = new System.Drawing.Size(103, 26);
			this.butPassword.TabIndex = 25;
			this.butPassword.Text = "Change Password";
			this.butPassword.Click += new System.EventHandler(this.butPassword_Click);
			// 
			// textUserName
			// 
			this.textUserName.Location = new System.Drawing.Point(98, 7);
			this.textUserName.Name = "textUserName";
			this.textUserName.Size = new System.Drawing.Size(198, 20);
			this.textUserName.TabIndex = 0;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(262, 309);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 20;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(343, 309);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 19;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// listUserGroup
			// 
			this.listUserGroup.Location = new System.Drawing.Point(99, 33);
			this.listUserGroup.Name = "listUserGroup";
			this.listUserGroup.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listUserGroup.Size = new System.Drawing.Size(197, 212);
			this.listUserGroup.TabIndex = 24;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 59);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 60);
			this.label3.TabIndex = 23;
			this.label3.Text = "User Group";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 33);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 20);
			this.label1.TabIndex = 21;
			this.label1.Text = "Name";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabUser);
			this.tabControl.Controls.Add(this.tabAlertSubs);
			this.tabControl.Location = new System.Drawing.Point(15, 27);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(403, 276);
			this.tabControl.TabIndex = 34;
			// 
			// tabUser
			// 
			this.tabUser.BackColor = System.Drawing.SystemColors.Control;
			this.tabUser.Controls.Add(this.listUserGroup);
			this.tabUser.Controls.Add(this.textUserName);
			this.tabUser.Location = new System.Drawing.Point(4, 22);
			this.tabUser.Name = "tabUser";
			this.tabUser.Padding = new System.Windows.Forms.Padding(3);
			this.tabUser.Size = new System.Drawing.Size(395, 250);
			this.tabUser.TabIndex = 0;
			this.tabUser.Text = "User";
			// 
			// tabAlertSubs
			// 
			this.tabAlertSubs.Controls.Add(this.listAlertSubMulti);
			this.tabAlertSubs.Controls.Add(this.label7);
			this.tabAlertSubs.Location = new System.Drawing.Point(4, 22);
			this.tabAlertSubs.Name = "tabAlertSubs";
			this.tabAlertSubs.Padding = new System.Windows.Forms.Padding(3);
			this.tabAlertSubs.Size = new System.Drawing.Size(395, 250);
			this.tabAlertSubs.TabIndex = 1;
			this.tabAlertSubs.Text = "Alert Subs";
			// 
			// listAlertSubMulti
			// 
			this.listAlertSubMulti.Location = new System.Drawing.Point(72, 24);
			this.listAlertSubMulti.Name = "listAlertSubMulti";
			this.listAlertSubMulti.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listAlertSubMulti.Size = new System.Drawing.Size(250, 225);
			this.listAlertSubMulti.TabIndex = 170;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(72, 1);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(167, 20);
			this.label7.TabIndex = 169;
			this.label7.Text = "User Alert Subscriptions";
			this.label7.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butUnlock
			// 
			this.butUnlock.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUnlock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butUnlock.Autosize = true;
			this.butUnlock.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUnlock.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUnlock.CornerRadius = 4F;
			this.butUnlock.Location = new System.Drawing.Point(136, 309);
			this.butUnlock.Name = "butUnlock";
			this.butUnlock.Size = new System.Drawing.Size(103, 26);
			this.butUnlock.TabIndex = 169;
			this.butUnlock.Text = "Unlock Account";
			this.butUnlock.Click += new System.EventHandler(this.butUnlock_Click);
			// 
			// FormCentralUserEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(430, 347);
			this.Controls.Add(this.butUnlock);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.checkIsHidden);
			this.Controls.Add(this.butPassword);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(346, 355);
			this.Name = "FormCentralUserEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "User Edit";
			this.Load += new System.EventHandler(this.FormCentralUserEdit_Load);
			this.tabControl.ResumeLayout(false);
			this.tabUser.ResumeLayout(false);
			this.tabUser.PerformLayout();
			this.tabAlertSubs.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.CheckBox checkIsHidden;
		private OpenDental.UI.Button butPassword;
		private System.Windows.Forms.TextBox textUserName;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.ListBox listUserGroup;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabUser;
		private System.Windows.Forms.TabPage tabAlertSubs;
		private System.Windows.Forms.ListBox listAlertSubMulti;
		private System.Windows.Forms.Label label7;
		private OpenDental.UI.Button butUnlock;
	}
}