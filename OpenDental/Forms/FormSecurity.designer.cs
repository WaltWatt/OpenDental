namespace OpenDental{
	partial class FormSecurity {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSecurity));
			this.userControlSecurityTabs = new OpenDental.UserControlSecurityUserGroup();
			this.butOK = new OpenDental.UI.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.globalSecuritySettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// userControlSecurityTabs
			// 
			this.userControlSecurityTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.userControlSecurityTabs.IsForCEMT = false;
			this.userControlSecurityTabs.Location = new System.Drawing.Point(3, 26);
			this.userControlSecurityTabs.MinimumSize = new System.Drawing.Size(914, 217);
			this.userControlSecurityTabs.Name = "userControlSecurityTabs";
			this.userControlSecurityTabs.SelectedUser = null;
			this.userControlSecurityTabs.SelectedUserGroup = null;
			this.userControlSecurityTabs.Size = new System.Drawing.Size(969, 667);
			this.userControlSecurityTabs.TabIndex = 252;
			this.userControlSecurityTabs.AddUserClick += new OpenDental.UserControlSecurityUserGroup.SecurityTabsEventHandler(this.userControlSecurityTabs_AddUserClick);
			this.userControlSecurityTabs.EditUserClick += new OpenDental.UserControlSecurityUserGroup.SecurityTabsEventHandler(this.userControlSecurityTabs_EditUserClick);
			this.userControlSecurityTabs.AddUserGroupClick += new OpenDental.UserControlSecurityUserGroup.SecurityTabsEventHandler(this.userControlSecurityTabs_AddUserGroupClick);
			this.userControlSecurityTabs.EditUserGroupClick += new OpenDental.UserControlSecurityUserGroup.SecurityTabsEventHandler(this.userControlSecurityTabs_EditUserGroupClick);
			this.userControlSecurityTabs.ReportPermissionChecked += new OpenDental.UserControlSecurityUserGroup.SecurityTreeEventHandler(this.userControlSecurityTabs_ReportPermissionChecked);
			this.userControlSecurityTabs.GroupPermissionChecked += new OpenDental.UserControlSecurityUserGroup.SecurityTreeEventHandler(this.userControlSecurityTabs_GroupPermissionChecked);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(891, 699);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "Close";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.globalSecuritySettingsToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(984, 24);
			this.menuStrip1.TabIndex = 251;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// globalSecuritySettingsToolStripMenuItem
			// 
			this.globalSecuritySettingsToolStripMenuItem.Name = "globalSecuritySettingsToolStripMenuItem";
			this.globalSecuritySettingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.globalSecuritySettingsToolStripMenuItem.Text = "Settings";
			this.globalSecuritySettingsToolStripMenuItem.Click += new System.EventHandler(this.globalSecuritySettingsToolStripMenuItem_Click);
			// 
			// FormSecurity
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(984, 735);
			this.Controls.Add(this.userControlSecurityTabs);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.MinimumSize = new System.Drawing.Size(940, 365);
			this.Name = "FormSecurity";
			this.Text = "Security";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSecurityEdit_FormClosing);
			this.Load += new System.EventHandler(this.FormSecurityEdit_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem globalSecuritySettingsToolStripMenuItem;
		private UserControlSecurityUserGroup userControlSecurityTabs;
	}
}