namespace OpenDental {
	partial class UserControlSecurityUserGroup {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.tabControlMain = new System.Windows.Forms.TabControl();
			this.tabPageUsers = new System.Windows.Forms.TabPage();
			this.butEditUser = new OpenDental.UI.Button();
			this.butAddUser = new OpenDental.UI.Button();
			this.securityTreeUser = new OpenDental.UserControlSecurityTree();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkShowHidden = new System.Windows.Forms.CheckBox();
			this.labelFilterType = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.comboShowOnly = new System.Windows.Forms.ComboBox();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.textPowerSearch = new System.Windows.Forms.TextBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.comboGroups = new System.Windows.Forms.ComboBox();
			this.labelPermission = new System.Windows.Forms.Label();
			this.comboSchoolClass = new System.Windows.Forms.ComboBox();
			this.labelSchoolClass = new System.Windows.Forms.Label();
			this.listUserTabUsers = new System.Windows.Forms.ListBox();
			this.listUserTabUserGroups = new System.Windows.Forms.ListBox();
			this.labelUserTabUsers = new System.Windows.Forms.Label();
			this.labelPerm = new System.Windows.Forms.Label();
			this.labelUserTabUserGroups = new System.Windows.Forms.Label();
			this.tabPageUserGroups = new System.Windows.Forms.TabPage();
			this.butEditGroup = new OpenDental.UI.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.securityTreeUserGroup = new OpenDental.UserControlSecurityTree();
			this.listAssociatedUsers = new System.Windows.Forms.ListBox();
			this.label4 = new System.Windows.Forms.Label();
			this.butSetAll = new OpenDental.UI.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.butAddGroup = new OpenDental.UI.Button();
			this.listUserGroupTabUserGroups = new System.Windows.Forms.ListBox();
			this.tabControlMain.SuspendLayout();
			this.tabPageUsers.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabPageUserGroups.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControlMain
			// 
			this.tabControlMain.Controls.Add(this.tabPageUsers);
			this.tabControlMain.Controls.Add(this.tabPageUserGroups);
			this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlMain.Location = new System.Drawing.Point(0, 0);
			this.tabControlMain.Name = "tabControlMain";
			this.tabControlMain.SelectedIndex = 0;
			this.tabControlMain.Size = new System.Drawing.Size(860, 667);
			this.tabControlMain.TabIndex = 251;
			this.tabControlMain.SelectedIndexChanged += new System.EventHandler(this.tabControlMain_SelectedIndexChanged);
			// 
			// tabPageUsers
			// 
			this.tabPageUsers.Controls.Add(this.butEditUser);
			this.tabPageUsers.Controls.Add(this.butAddUser);
			this.tabPageUsers.Controls.Add(this.securityTreeUser);
			this.tabPageUsers.Controls.Add(this.groupBox2);
			this.tabPageUsers.Controls.Add(this.listUserTabUsers);
			this.tabPageUsers.Controls.Add(this.listUserTabUserGroups);
			this.tabPageUsers.Controls.Add(this.labelUserTabUsers);
			this.tabPageUsers.Controls.Add(this.labelPerm);
			this.tabPageUsers.Controls.Add(this.labelUserTabUserGroups);
			this.tabPageUsers.Location = new System.Drawing.Point(4, 22);
			this.tabPageUsers.Name = "tabPageUsers";
			this.tabPageUsers.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageUsers.Size = new System.Drawing.Size(852, 641);
			this.tabPageUsers.TabIndex = 1;
			this.tabPageUsers.Text = "Users";
			this.tabPageUsers.UseVisualStyleBackColor = true;
			// 
			// butEditUser
			// 
			this.butEditUser.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEditUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butEditUser.Autosize = true;
			this.butEditUser.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEditUser.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEditUser.CornerRadius = 4F;
			this.butEditUser.Image = global::OpenDental.Properties.Resources.editPencil;
			this.butEditUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEditUser.Location = new System.Drawing.Point(114, 612);
			this.butEditUser.Name = "butEditUser";
			this.butEditUser.Size = new System.Drawing.Size(87, 24);
			this.butEditUser.TabIndex = 249;
			this.butEditUser.Text = "Edit User";
			this.butEditUser.Click += new System.EventHandler(this.butEditUser_Click);
			// 
			// butAddUser
			// 
			this.butAddUser.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAddUser.Autosize = true;
			this.butAddUser.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddUser.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddUser.CornerRadius = 4F;
			this.butAddUser.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddUser.Location = new System.Drawing.Point(7, 612);
			this.butAddUser.Name = "butAddUser";
			this.butAddUser.Size = new System.Drawing.Size(87, 24);
			this.butAddUser.TabIndex = 14;
			this.butAddUser.Text = "Add User";
			this.butAddUser.Click += new System.EventHandler(this.butAddUser_Click);
			// 
			// securityTreeUser
			// 
			this.securityTreeUser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.securityTreeUser.Location = new System.Drawing.Point(407, 22);
			this.securityTreeUser.Name = "securityTreeUser";
			this.securityTreeUser.ReadOnly = true;
			this.securityTreeUser.Size = new System.Drawing.Size(434, 612);
			this.securityTreeUser.TabIndex = 11;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkShowHidden);
			this.groupBox2.Controls.Add(this.labelFilterType);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.comboShowOnly);
			this.groupBox2.Controls.Add(this.comboClinic);
			this.groupBox2.Controls.Add(this.textPowerSearch);
			this.groupBox2.Controls.Add(this.labelClinic);
			this.groupBox2.Controls.Add(this.comboGroups);
			this.groupBox2.Controls.Add(this.labelPermission);
			this.groupBox2.Controls.Add(this.comboSchoolClass);
			this.groupBox2.Controls.Add(this.labelSchoolClass);
			this.groupBox2.Location = new System.Drawing.Point(6, 6);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(395, 94);
			this.groupBox2.TabIndex = 248;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "User Filters";
			// 
			// checkShowHidden
			// 
			this.checkShowHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkShowHidden.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowHidden.Location = new System.Drawing.Point(260, 56);
			this.checkShowHidden.Name = "checkShowHidden";
			this.checkShowHidden.Size = new System.Drawing.Size(118, 32);
			this.checkShowHidden.TabIndex = 248;
			this.checkShowHidden.Text = "Show hidden users";
			this.checkShowHidden.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowHidden.Click += new System.EventHandler(this.Filter_Changed);
			// 
			// labelFilterType
			// 
			this.labelFilterType.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelFilterType.Location = new System.Drawing.Point(6, 52);
			this.labelFilterType.Name = "labelFilterType";
			this.labelFilterType.Size = new System.Drawing.Size(115, 15);
			this.labelFilterType.TabIndex = 248;
			this.labelFilterType.Text = "Username";
			this.labelFilterType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(6, 13);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(115, 15);
			this.label5.TabIndex = 247;
			this.label5.Text = "Show Only";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboShowOnly
			// 
			this.comboShowOnly.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboShowOnly.FormattingEnabled = true;
			this.comboShowOnly.Location = new System.Drawing.Point(6, 29);
			this.comboShowOnly.Name = "comboShowOnly";
			this.comboShowOnly.Size = new System.Drawing.Size(118, 21);
			this.comboShowOnly.TabIndex = 1;
			this.comboShowOnly.SelectionChangeCommitted += new System.EventHandler(this.comboShowOnly_SelectionChangeCommitted);
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(260, 29);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(118, 21);
			this.comboClinic.TabIndex = 245;
			this.comboClinic.Visible = false;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.Filter_Changed);
			// 
			// textPowerSearch
			// 
			this.textPowerSearch.Location = new System.Drawing.Point(6, 68);
			this.textPowerSearch.Name = "textPowerSearch";
			this.textPowerSearch.Size = new System.Drawing.Size(118, 20);
			this.textPowerSearch.TabIndex = 243;
			this.textPowerSearch.TextChanged += new System.EventHandler(this.Filter_Changed);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(257, 13);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(115, 15);
			this.labelClinic.TabIndex = 246;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelClinic.Visible = false;
			// 
			// comboGroups
			// 
			this.comboGroups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboGroups.Location = new System.Drawing.Point(133, 29);
			this.comboGroups.MaxDropDownItems = 30;
			this.comboGroups.Name = "comboGroups";
			this.comboGroups.Size = new System.Drawing.Size(118, 21);
			this.comboGroups.TabIndex = 245;
			this.comboGroups.SelectionChangeCommitted += new System.EventHandler(this.Filter_Changed);
			// 
			// labelPermission
			// 
			this.labelPermission.Location = new System.Drawing.Point(130, 13);
			this.labelPermission.Name = "labelPermission";
			this.labelPermission.Size = new System.Drawing.Size(115, 15);
			this.labelPermission.TabIndex = 246;
			this.labelPermission.Text = "Group";
			this.labelPermission.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboSchoolClass
			// 
			this.comboSchoolClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSchoolClass.Location = new System.Drawing.Point(133, 67);
			this.comboSchoolClass.MaxDropDownItems = 30;
			this.comboSchoolClass.Name = "comboSchoolClass";
			this.comboSchoolClass.Size = new System.Drawing.Size(118, 21);
			this.comboSchoolClass.TabIndex = 2;
			this.comboSchoolClass.Visible = false;
			this.comboSchoolClass.SelectionChangeCommitted += new System.EventHandler(this.Filter_Changed);
			// 
			// labelSchoolClass
			// 
			this.labelSchoolClass.Location = new System.Drawing.Point(130, 51);
			this.labelSchoolClass.Name = "labelSchoolClass";
			this.labelSchoolClass.Size = new System.Drawing.Size(115, 15);
			this.labelSchoolClass.TabIndex = 91;
			this.labelSchoolClass.Text = "Class";
			this.labelSchoolClass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelSchoolClass.Visible = false;
			// 
			// listUserTabUsers
			// 
			this.listUserTabUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listUserTabUsers.FormattingEnabled = true;
			this.listUserTabUsers.IntegralHeight = false;
			this.listUserTabUsers.Location = new System.Drawing.Point(7, 121);
			this.listUserTabUsers.Name = "listUserTabUsers";
			this.listUserTabUsers.Size = new System.Drawing.Size(194, 485);
			this.listUserTabUsers.TabIndex = 4;
			this.listUserTabUsers.SelectedIndexChanged += new System.EventHandler(this.listUserTabUsers_SelectedIndexChanged);
			this.listUserTabUsers.DoubleClick += new System.EventHandler(this.listUserTabUsers_DoubleClick);
			// 
			// listUserTabUserGroups
			// 
			this.listUserTabUserGroups.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listUserTabUserGroups.FormattingEnabled = true;
			this.listUserTabUserGroups.IntegralHeight = false;
			this.listUserTabUserGroups.Location = new System.Drawing.Point(207, 121);
			this.listUserTabUserGroups.Name = "listUserTabUserGroups";
			this.listUserTabUserGroups.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listUserTabUserGroups.Size = new System.Drawing.Size(194, 511);
			this.listUserTabUserGroups.TabIndex = 5;
			this.listUserTabUserGroups.SelectedIndexChanged += new System.EventHandler(this.listUserTabUserGroups_SelectedIndexChanged);
			// 
			// labelUserTabUsers
			// 
			this.labelUserTabUsers.Location = new System.Drawing.Point(7, 103);
			this.labelUserTabUsers.Name = "labelUserTabUsers";
			this.labelUserTabUsers.Size = new System.Drawing.Size(194, 15);
			this.labelUserTabUsers.TabIndex = 13;
			this.labelUserTabUsers.Text = "User:";
			this.labelUserTabUsers.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelPerm
			// 
			this.labelPerm.Location = new System.Drawing.Point(407, 4);
			this.labelPerm.Name = "labelPerm";
			this.labelPerm.Size = new System.Drawing.Size(417, 15);
			this.labelPerm.TabIndex = 10;
			this.labelPerm.Text = "Effective permissions for user: (Edit from the User Groups tab)";
			this.labelPerm.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelUserTabUserGroups
			// 
			this.labelUserTabUserGroups.Location = new System.Drawing.Point(204, 103);
			this.labelUserTabUserGroups.Name = "labelUserTabUserGroups";
			this.labelUserTabUserGroups.Size = new System.Drawing.Size(197, 15);
			this.labelUserTabUserGroups.TabIndex = 12;
			this.labelUserTabUserGroups.Text = "User Groups for User:";
			this.labelUserTabUserGroups.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// tabPageUserGroups
			// 
			this.tabPageUserGroups.Controls.Add(this.butEditGroup);
			this.tabPageUserGroups.Controls.Add(this.label3);
			this.tabPageUserGroups.Controls.Add(this.securityTreeUserGroup);
			this.tabPageUserGroups.Controls.Add(this.listAssociatedUsers);
			this.tabPageUserGroups.Controls.Add(this.label4);
			this.tabPageUserGroups.Controls.Add(this.butSetAll);
			this.tabPageUserGroups.Controls.Add(this.label6);
			this.tabPageUserGroups.Controls.Add(this.butAddGroup);
			this.tabPageUserGroups.Controls.Add(this.listUserGroupTabUserGroups);
			this.tabPageUserGroups.Location = new System.Drawing.Point(4, 22);
			this.tabPageUserGroups.Name = "tabPageUserGroups";
			this.tabPageUserGroups.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageUserGroups.Size = new System.Drawing.Size(852, 641);
			this.tabPageUserGroups.TabIndex = 0;
			this.tabPageUserGroups.Text = "User Groups";
			this.tabPageUserGroups.UseVisualStyleBackColor = true;
			// 
			// butEditGroup
			// 
			this.butEditGroup.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEditGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butEditGroup.Autosize = true;
			this.butEditGroup.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEditGroup.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEditGroup.CornerRadius = 4F;
			this.butEditGroup.Image = global::OpenDental.Properties.Resources.editPencil;
			this.butEditGroup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEditGroup.Location = new System.Drawing.Point(144, 613);
			this.butEditGroup.Name = "butEditGroup";
			this.butEditGroup.Size = new System.Drawing.Size(87, 24);
			this.butEditGroup.TabIndex = 25;
			this.butEditGroup.Text = "Edit Group";
			this.butEditGroup.Click += new System.EventHandler(this.butEditGroup_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(6, 1);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(225, 15);
			this.label3.TabIndex = 24;
			this.label3.Text = "User Group:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// securityTreeUserGroup
			// 
			this.securityTreeUserGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.securityTreeUserGroup.Location = new System.Drawing.Point(237, 19);
			this.securityTreeUserGroup.Name = "securityTreeUserGroup";
			this.securityTreeUserGroup.ReadOnly = false;
			this.securityTreeUserGroup.Size = new System.Drawing.Size(396, 591);
			this.securityTreeUserGroup.TabIndex = 23;
			this.securityTreeUserGroup.ReportPermissionChecked += new OpenDental.UserControlSecurityTree.SecurityTreeEventHandler(this.securityTreeUserGroup_ReportPermissionChecked);
			this.securityTreeUserGroup.GroupPermissionChecked += new OpenDental.UserControlSecurityTree.SecurityTreeEventHandler(this.securityTreeUserGroup_GroupPermissionChecked);
			// 
			// listAssociatedUsers
			// 
			this.listAssociatedUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listAssociatedUsers.FormattingEnabled = true;
			this.listAssociatedUsers.IntegralHeight = false;
			this.listAssociatedUsers.Location = new System.Drawing.Point(639, 19);
			this.listAssociatedUsers.Name = "listAssociatedUsers";
			this.listAssociatedUsers.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listAssociatedUsers.Size = new System.Drawing.Size(207, 589);
			this.listAssociatedUsers.TabIndex = 22;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Location = new System.Drawing.Point(636, 2);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(168, 15);
			this.label4.TabIndex = 21;
			this.label4.Text = "Users currently associated:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butSetAll
			// 
			this.butSetAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSetAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSetAll.Autosize = true;
			this.butSetAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSetAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSetAll.CornerRadius = 4F;
			this.butSetAll.Location = new System.Drawing.Point(237, 613);
			this.butSetAll.Name = "butSetAll";
			this.butSetAll.Size = new System.Drawing.Size(79, 24);
			this.butSetAll.TabIndex = 20;
			this.butSetAll.Text = "Set All";
			this.butSetAll.Click += new System.EventHandler(this.butSetAll_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(237, 1);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(285, 15);
			this.label6.TabIndex = 19;
			this.label6.Text = "Permissions for group:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butAddGroup
			// 
			this.butAddGroup.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAddGroup.Autosize = true;
			this.butAddGroup.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddGroup.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddGroup.CornerRadius = 4F;
			this.butAddGroup.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddGroup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddGroup.Location = new System.Drawing.Point(6, 613);
			this.butAddGroup.Name = "butAddGroup";
			this.butAddGroup.Size = new System.Drawing.Size(87, 24);
			this.butAddGroup.TabIndex = 18;
			this.butAddGroup.Text = "Add Group";
			this.butAddGroup.Click += new System.EventHandler(this.butAddGroup_Click);
			// 
			// listUserGroupTabUserGroups
			// 
			this.listUserGroupTabUserGroups.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listUserGroupTabUserGroups.FormattingEnabled = true;
			this.listUserGroupTabUserGroups.IntegralHeight = false;
			this.listUserGroupTabUserGroups.Location = new System.Drawing.Point(6, 19);
			this.listUserGroupTabUserGroups.Name = "listUserGroupTabUserGroups";
			this.listUserGroupTabUserGroups.Size = new System.Drawing.Size(225, 589);
			this.listUserGroupTabUserGroups.TabIndex = 17;
			this.listUserGroupTabUserGroups.SelectedIndexChanged += new System.EventHandler(this.listUserGroupTabUserGroups_SelectedIndexChanged);
			this.listUserGroupTabUserGroups.DoubleClick += new System.EventHandler(this.listUserGroupTabUserGroups_DoubleClick);
			// 
			// UserControlSecurityUserGroup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControlMain);
			this.Name = "UserControlSecurityUserGroup";
			this.Size = new System.Drawing.Size(860, 667);
			this.Load += new System.EventHandler(this.UserControlUserGroupSecurity_Load);
			this.tabControlMain.ResumeLayout(false);
			this.tabPageUsers.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabPageUserGroups.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControlMain;
		private System.Windows.Forms.TabPage tabPageUsers;
		private UI.Button butEditUser;
		private UI.Button butAddUser;
		private UserControlSecurityTree securityTreeUser;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox checkShowHidden;
		private System.Windows.Forms.Label labelFilterType;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox comboShowOnly;
		private System.Windows.Forms.ComboBox comboClinic;
		private System.Windows.Forms.TextBox textPowerSearch;
		private System.Windows.Forms.Label labelClinic;
		private System.Windows.Forms.ComboBox comboGroups;
		private System.Windows.Forms.Label labelPermission;
		private System.Windows.Forms.ComboBox comboSchoolClass;
		private System.Windows.Forms.Label labelSchoolClass;
		private System.Windows.Forms.ListBox listUserTabUsers;
		private System.Windows.Forms.ListBox listUserTabUserGroups;
		private System.Windows.Forms.Label labelUserTabUsers;
		private System.Windows.Forms.Label labelPerm;
		private System.Windows.Forms.Label labelUserTabUserGroups;
		private System.Windows.Forms.TabPage tabPageUserGroups;
		private UI.Button butEditGroup;
		private System.Windows.Forms.Label label3;
		private UserControlSecurityTree securityTreeUserGroup;
		private System.Windows.Forms.ListBox listAssociatedUsers;
		private System.Windows.Forms.Label label4;
		private UI.Button butSetAll;
		private System.Windows.Forms.Label label6;
		private UI.Button butAddGroup;
		private System.Windows.Forms.ListBox listUserGroupTabUserGroups;
	}
}
