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
			this.tabPageUserGroups = new System.Windows.Forms.TabPage();
			this.label3 = new System.Windows.Forms.Label();
			this.listAssociatedUsers = new System.Windows.Forms.ListBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.listUserGroupTabUserGroups = new System.Windows.Forms.ListBox();
			this.tabPageUsers = new System.Windows.Forms.TabPage();
			this.gridUsers = new OpenDental.UI.ODGrid();
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
			this.listUserTabUserGroups = new System.Windows.Forms.ListBox();
			this.labelPerm = new System.Windows.Forms.Label();
			this.labelUserTabUserGroups = new System.Windows.Forms.Label();
			this.tabControlMain = new System.Windows.Forms.TabControl();
			this.securityTreeUser = new OpenDental.UserControlSecurityTree();
			this.butEditGroup = new OpenDental.UI.Button();
			this.butSetAll = new OpenDental.UI.Button();
			this.butAddGroup = new OpenDental.UI.Button();
			this.securityTreeUserGroup = new OpenDental.UserControlSecurityTree();
			this.tabPageUserGroups.SuspendLayout();
			this.tabPageUsers.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabControlMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabPageUserGroups
			// 
			this.tabPageUserGroups.Controls.Add(this.butEditGroup);
			this.tabPageUserGroups.Controls.Add(this.butSetAll);
			this.tabPageUserGroups.Controls.Add(this.butAddGroup);
			this.tabPageUserGroups.Controls.Add(this.label3);
			this.tabPageUserGroups.Controls.Add(this.securityTreeUserGroup);
			this.tabPageUserGroups.Controls.Add(this.listAssociatedUsers);
			this.tabPageUserGroups.Controls.Add(this.label4);
			this.tabPageUserGroups.Controls.Add(this.label6);
			this.tabPageUserGroups.Controls.Add(this.listUserGroupTabUserGroups);
			this.tabPageUserGroups.Location = new System.Drawing.Point(4, 22);
			this.tabPageUserGroups.Name = "tabPageUserGroups";
			this.tabPageUserGroups.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageUserGroups.Size = new System.Drawing.Size(961, 641);
			this.tabPageUserGroups.TabIndex = 0;
			this.tabPageUserGroups.Text = "User Groups";
			this.tabPageUserGroups.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(3, 1);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(225, 15);
			this.label3.TabIndex = 24;
			this.label3.Text = "User Group:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listAssociatedUsers
			// 
			this.listAssociatedUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listAssociatedUsers.FormattingEnabled = true;
			this.listAssociatedUsers.IntegralHeight = false;
			this.listAssociatedUsers.Location = new System.Drawing.Point(714, 19);
			this.listAssociatedUsers.Name = "listAssociatedUsers";
			this.listAssociatedUsers.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listAssociatedUsers.Size = new System.Drawing.Size(242, 589);
			this.listAssociatedUsers.TabIndex = 22;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Location = new System.Drawing.Point(711, 1);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(210, 15);
			this.label4.TabIndex = 21;
			this.label4.Text = "Users currently associated:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(279, 1);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(258, 15);
			this.label6.TabIndex = 19;
			this.label6.Text = "Permissions for group:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listUserGroupTabUserGroups
			// 
			this.listUserGroupTabUserGroups.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listUserGroupTabUserGroups.FormattingEnabled = true;
			this.listUserGroupTabUserGroups.IntegralHeight = false;
			this.listUserGroupTabUserGroups.Location = new System.Drawing.Point(6, 19);
			this.listUserGroupTabUserGroups.Name = "listUserGroupTabUserGroups";
			this.listUserGroupTabUserGroups.Size = new System.Drawing.Size(270, 589);
			this.listUserGroupTabUserGroups.TabIndex = 17;
			this.listUserGroupTabUserGroups.SelectedIndexChanged += new System.EventHandler(this.listUserGroupTabUserGroups_SelectedIndexChanged);
			this.listUserGroupTabUserGroups.DoubleClick += new System.EventHandler(this.listUserGroupTabUserGroups_DoubleClick);
			// 
			// tabPageUsers
			// 
			this.tabPageUsers.Controls.Add(this.gridUsers);
			this.tabPageUsers.Controls.Add(this.securityTreeUser);
			this.tabPageUsers.Controls.Add(this.groupBox2);
			this.tabPageUsers.Controls.Add(this.listUserTabUserGroups);
			this.tabPageUsers.Controls.Add(this.labelPerm);
			this.tabPageUsers.Controls.Add(this.labelUserTabUserGroups);
			this.tabPageUsers.Location = new System.Drawing.Point(4, 22);
			this.tabPageUsers.Name = "tabPageUsers";
			this.tabPageUsers.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageUsers.Size = new System.Drawing.Size(961, 641);
			this.tabPageUsers.TabIndex = 1;
			this.tabPageUsers.Text = "Users";
			this.tabPageUsers.UseVisualStyleBackColor = true;
			// 
			// gridUsers
			// 
			this.gridUsers.AllowSortingByColumn = true;
			this.gridUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridUsers.AutoScroll = true;
			this.gridUsers.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridUsers.HasAddButton = true;
			this.gridUsers.HasDropDowns = false;
			this.gridUsers.HasMultilineHeaders = true;
			this.gridUsers.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridUsers.HeaderHeight = 15;
			this.gridUsers.HScrollVisible = true;
			this.gridUsers.Location = new System.Drawing.Point(0, 103);
			this.gridUsers.Name = "gridUsers";
			this.gridUsers.ScrollValue = 0;
			this.gridUsers.Size = new System.Drawing.Size(454, 537);
			this.gridUsers.TabIndex = 254;
			this.gridUsers.Title = "Users";
			this.gridUsers.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridUsers.TitleHeight = 18;
			this.gridUsers.TranslationName = "TableUsers";
			this.gridUsers.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridUsers_CellDoubleClick);
			this.gridUsers.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridUsers_CellClick);
			this.gridUsers.TitleAddClick += new System.EventHandler(this.gridUsers_TitleAddClick);
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
			this.groupBox2.Size = new System.Drawing.Size(410, 94);
			this.groupBox2.TabIndex = 248;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "User Filters";
			// 
			// checkShowHidden
			// 
			this.checkShowHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkShowHidden.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowHidden.Location = new System.Drawing.Point(280, 56);
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
			this.comboShowOnly.SelectedIndexChanged += new System.EventHandler(this.comboShowOnly_SelectionIndexChanged);
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(280, 29);
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
			this.labelClinic.Location = new System.Drawing.Point(277, 13);
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
			this.comboGroups.Location = new System.Drawing.Point(143, 29);
			this.comboGroups.MaxDropDownItems = 30;
			this.comboGroups.Name = "comboGroups";
			this.comboGroups.Size = new System.Drawing.Size(118, 21);
			this.comboGroups.TabIndex = 245;
			this.comboGroups.SelectionChangeCommitted += new System.EventHandler(this.Filter_Changed);
			// 
			// labelPermission
			// 
			this.labelPermission.Location = new System.Drawing.Point(140, 13);
			this.labelPermission.Name = "labelPermission";
			this.labelPermission.Size = new System.Drawing.Size(115, 15);
			this.labelPermission.TabIndex = 246;
			this.labelPermission.Text = "Group";
			this.labelPermission.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboSchoolClass
			// 
			this.comboSchoolClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSchoolClass.Location = new System.Drawing.Point(143, 67);
			this.comboSchoolClass.MaxDropDownItems = 30;
			this.comboSchoolClass.Name = "comboSchoolClass";
			this.comboSchoolClass.Size = new System.Drawing.Size(118, 21);
			this.comboSchoolClass.TabIndex = 2;
			this.comboSchoolClass.Visible = false;
			this.comboSchoolClass.SelectionChangeCommitted += new System.EventHandler(this.Filter_Changed);
			// 
			// labelSchoolClass
			// 
			this.labelSchoolClass.Location = new System.Drawing.Point(140, 51);
			this.labelSchoolClass.Name = "labelSchoolClass";
			this.labelSchoolClass.Size = new System.Drawing.Size(115, 15);
			this.labelSchoolClass.TabIndex = 91;
			this.labelSchoolClass.Text = "Class";
			this.labelSchoolClass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelSchoolClass.Visible = false;
			// 
			// listUserTabUserGroups
			// 
			this.listUserTabUserGroups.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.listUserTabUserGroups.FormattingEnabled = true;
			this.listUserTabUserGroups.IntegralHeight = false;
			this.listUserTabUserGroups.Location = new System.Drawing.Point(456, 103);
			this.listUserTabUserGroups.Name = "listUserTabUserGroups";
			this.listUserTabUserGroups.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listUserTabUserGroups.Size = new System.Drawing.Size(147, 537);
			this.listUserTabUserGroups.TabIndex = 5;
			this.listUserTabUserGroups.SelectedIndexChanged += new System.EventHandler(this.listUserTabUserGroups_SelectedIndexChanged);
			// 
			// labelPerm
			// 
			this.labelPerm.Location = new System.Drawing.Point(602, 5);
			this.labelPerm.Name = "labelPerm";
			this.labelPerm.Size = new System.Drawing.Size(354, 15);
			this.labelPerm.TabIndex = 10;
			this.labelPerm.Text = "Effective permissions for user: (Edit from the User Groups tab)";
			this.labelPerm.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelUserTabUserGroups
			// 
			this.labelUserTabUserGroups.Location = new System.Drawing.Point(453, 85);
			this.labelUserTabUserGroups.Name = "labelUserTabUserGroups";
			this.labelUserTabUserGroups.Size = new System.Drawing.Size(147, 15);
			this.labelUserTabUserGroups.TabIndex = 12;
			this.labelUserTabUserGroups.Text = "User Groups for User:";
			this.labelUserTabUserGroups.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// tabControlMain
			// 
			this.tabControlMain.Controls.Add(this.tabPageUsers);
			this.tabControlMain.Controls.Add(this.tabPageUserGroups);
			this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlMain.Location = new System.Drawing.Point(0, 0);
			this.tabControlMain.Name = "tabControlMain";
			this.tabControlMain.SelectedIndex = 0;
			this.tabControlMain.Size = new System.Drawing.Size(969, 667);
			this.tabControlMain.TabIndex = 251;
			this.tabControlMain.SelectedIndexChanged += new System.EventHandler(this.tabControlMain_SelectedIndexChanged);
			// 
			// securityTreeUser
			// 
			this.securityTreeUser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.securityTreeUser.Location = new System.Drawing.Point(605, 23);
			this.securityTreeUser.Name = "securityTreeUser";
			this.securityTreeUser.ReadOnly = true;
			this.securityTreeUser.Size = new System.Drawing.Size(354, 617);
			this.securityTreeUser.TabIndex = 11;
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
			this.butEditGroup.Location = new System.Drawing.Point(189, 612);
			this.butEditGroup.Name = "butEditGroup";
			this.butEditGroup.Size = new System.Drawing.Size(87, 24);
			this.butEditGroup.TabIndex = 25;
			this.butEditGroup.Text = "Edit Group";
			this.butEditGroup.Click += new System.EventHandler(this.butEditGroup_Click);
			// 
			// butSetAll
			// 
			this.butSetAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSetAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSetAll.Autosize = true;
			this.butSetAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSetAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSetAll.CornerRadius = 4F;
			this.butSetAll.Location = new System.Drawing.Point(282, 612);
			this.butSetAll.Name = "butSetAll";
			this.butSetAll.Size = new System.Drawing.Size(79, 24);
			this.butSetAll.TabIndex = 20;
			this.butSetAll.Text = "Set All";
			this.butSetAll.Click += new System.EventHandler(this.butSetAll_Click);
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
			this.butAddGroup.Location = new System.Drawing.Point(6, 612);
			this.butAddGroup.Name = "butAddGroup";
			this.butAddGroup.Size = new System.Drawing.Size(87, 24);
			this.butAddGroup.TabIndex = 18;
			this.butAddGroup.Text = "Add Group";
			this.butAddGroup.Click += new System.EventHandler(this.butAddGroup_Click);
			// 
			// securityTreeUserGroup
			// 
			this.securityTreeUserGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.securityTreeUserGroup.Location = new System.Drawing.Point(282, 19);
			this.securityTreeUserGroup.Name = "securityTreeUserGroup";
			this.securityTreeUserGroup.ReadOnly = false;
			this.securityTreeUserGroup.Size = new System.Drawing.Size(426, 589);
			this.securityTreeUserGroup.TabIndex = 23;
			this.securityTreeUserGroup.ReportPermissionChecked += new OpenDental.UserControlSecurityTree.SecurityTreeEventHandler(this.securityTreeUserGroup_ReportPermissionChecked);
			this.securityTreeUserGroup.GroupPermissionChecked += new OpenDental.UserControlSecurityTree.SecurityTreeEventHandler(this.securityTreeUserGroup_GroupPermissionChecked);
			// 
			// UserControlSecurityUserGroup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControlMain);
			this.MinimumSize = new System.Drawing.Size(914, 217);
			this.Name = "UserControlSecurityUserGroup";
			this.Size = new System.Drawing.Size(969, 667);
			this.Load += new System.EventHandler(this.UserControlUserGroupSecurity_Load);
			this.tabPageUserGroups.ResumeLayout(false);
			this.tabPageUsers.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabControlMain.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabPage tabPageUserGroups;
		private UI.Button butEditGroup;
		private UI.Button butSetAll;
		private UI.Button butAddGroup;
		private System.Windows.Forms.Label label3;
		private UserControlSecurityTree securityTreeUserGroup;
		private System.Windows.Forms.ListBox listAssociatedUsers;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ListBox listUserGroupTabUserGroups;
		private System.Windows.Forms.TabPage tabPageUsers;
		private UI.ODGrid gridUsers;
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
		private System.Windows.Forms.ListBox listUserTabUserGroups;
		private System.Windows.Forms.Label labelPerm;
		private System.Windows.Forms.Label labelUserTabUserGroups;
		private System.Windows.Forms.TabControl tabControlMain;
	}
}
