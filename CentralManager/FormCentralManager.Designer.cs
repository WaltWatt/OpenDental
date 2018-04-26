namespace CentralManager {
	partial class FormCentralManager {
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
			this.components = new System.ComponentModel.Container();
			this.textConnSearch = new System.Windows.Forms.TextBox();
			this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
			this.menuItemLogoff = new System.Windows.Forms.MenuItem();
			this.menuItemFile = new System.Windows.Forms.MenuItem();
			this.menuItemPassword = new System.Windows.Forms.MenuItem();
			this.menuItemSetup = new System.Windows.Forms.MenuItem();
			this.menuItemConnections = new System.Windows.Forms.MenuItem();
			this.menuItemGroups = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItemSecurity = new System.Windows.Forms.MenuItem();
			this.menuItemDisplayFields = new System.Windows.Forms.MenuItem();
			this.menuItemReports = new System.Windows.Forms.MenuItem();
			this.menuItemAnnualPI = new System.Windows.Forms.MenuItem();
			this.label1 = new System.Windows.Forms.Label();
			this.comboConnectionGroups = new System.Windows.Forms.ComboBox();
			this.groupBoxSync = new System.Windows.Forms.GroupBox();
			this.butLocks = new OpenDental.UI.Button();
			this.butUsers = new OpenDental.UI.Button();
			this.butSecurity = new OpenDental.UI.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.butPtSearch = new OpenDental.UI.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.labelSyncMiddleTier = new System.Windows.Forms.Label();
			this.butSyncManual = new OpenDental.UI.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.butRefresh = new OpenDental.UI.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.textProviderSearch = new System.Windows.Forms.TextBox();
			this.textClinicSearch = new System.Windows.Forms.TextBox();
			this.butEdit = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butRefreshConns = new OpenDental.UI.Button();
			this.labelVersion = new System.Windows.Forms.Label();
			this.checkAutoLog = new System.Windows.Forms.CheckBox();
			this.groupBoxSync.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// textConnSearch
			// 
			this.textConnSearch.Location = new System.Drawing.Point(6, 29);
			this.textConnSearch.Name = "textConnSearch";
			this.textConnSearch.Size = new System.Drawing.Size(157, 20);
			this.textConnSearch.TabIndex = 211;
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemLogoff,
            this.menuItemFile,
            this.menuItemSetup,
            this.menuItemReports});
			// 
			// menuItemLogoff
			// 
			this.menuItemLogoff.Index = 0;
			this.menuItemLogoff.Text = "Logoff";
			this.menuItemLogoff.Click += new System.EventHandler(this.menuItemLogoff_Click);
			// 
			// menuItemFile
			// 
			this.menuItemFile.Index = 1;
			this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemPassword});
			this.menuItemFile.Text = "File";
			// 
			// menuItemPassword
			// 
			this.menuItemPassword.Index = 0;
			this.menuItemPassword.Text = "Change Password";
			this.menuItemPassword.Click += new System.EventHandler(this.menuItemPassword_Click);
			// 
			// menuItemSetup
			// 
			this.menuItemSetup.Index = 2;
			this.menuItemSetup.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemConnections,
            this.menuItemGroups,
            this.menuItem1,
            this.menuItemSecurity,
            this.menuItemDisplayFields});
			this.menuItemSetup.Text = "Setup";
			// 
			// menuItemConnections
			// 
			this.menuItemConnections.Index = 0;
			this.menuItemConnections.Text = "Connections";
			this.menuItemConnections.Click += new System.EventHandler(this.menuConnSetup_Click);
			// 
			// menuItemGroups
			// 
			this.menuItemGroups.Index = 1;
			this.menuItemGroups.Text = "Groups";
			this.menuItemGroups.Click += new System.EventHandler(this.menuGroups_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 2;
			this.menuItem1.Text = "Report Permissions";
			this.menuItem1.Click += new System.EventHandler(this.menuItemReportSetup_Click);
			// 
			// menuItemSecurity
			// 
			this.menuItemSecurity.Index = 3;
			this.menuItemSecurity.Text = "Security";
			this.menuItemSecurity.Click += new System.EventHandler(this.menuItemSecurity_Click);
			// 
			// menuItemDisplayFields
			// 
			this.menuItemDisplayFields.Index = 4;
			this.menuItemDisplayFields.Text = "Display Fields";
			this.menuItemDisplayFields.Click += new System.EventHandler(this.menuItemDisplayFields_Click);
			// 
			// menuItemReports
			// 
			this.menuItemReports.Index = 3;
			this.menuItemReports.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAnnualPI});
			this.menuItemReports.Text = "Reports";
			// 
			// menuItemAnnualPI
			// 
			this.menuItemAnnualPI.Index = 0;
			this.menuItemAnnualPI.Text = "Production and Income";
			this.menuItemAnnualPI.Click += new System.EventHandler(this.menuProdInc_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(793, 179);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(169, 15);
			this.label1.TabIndex = 213;
			this.label1.Text = "Connection Groups";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// comboConnectionGroups
			// 
			this.comboConnectionGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboConnectionGroups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboConnectionGroups.FormattingEnabled = true;
			this.comboConnectionGroups.Location = new System.Drawing.Point(793, 197);
			this.comboConnectionGroups.MaxDropDownItems = 20;
			this.comboConnectionGroups.Name = "comboConnectionGroups";
			this.comboConnectionGroups.Size = new System.Drawing.Size(169, 21);
			this.comboConnectionGroups.TabIndex = 214;
			this.comboConnectionGroups.SelectionChangeCommitted += new System.EventHandler(this.comboConnectionGroups_SelectionChangeCommitted);
			// 
			// groupBoxSync
			// 
			this.groupBoxSync.Controls.Add(this.butLocks);
			this.groupBoxSync.Controls.Add(this.butUsers);
			this.groupBoxSync.Controls.Add(this.butSecurity);
			this.groupBoxSync.Location = new System.Drawing.Point(35, 124);
			this.groupBoxSync.Name = "groupBoxSync";
			this.groupBoxSync.Size = new System.Drawing.Size(98, 106);
			this.groupBoxSync.TabIndex = 218;
			this.groupBoxSync.TabStop = false;
			this.groupBoxSync.Text = "Sync";
			// 
			// butLocks
			// 
			this.butLocks.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butLocks.Autosize = true;
			this.butLocks.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLocks.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLocks.CornerRadius = 4F;
			this.butLocks.Location = new System.Drawing.Point(12, 19);
			this.butLocks.Name = "butLocks";
			this.butLocks.Size = new System.Drawing.Size(75, 23);
			this.butLocks.TabIndex = 221;
			this.butLocks.Text = "Locks";
			this.butLocks.UseVisualStyleBackColor = true;
			this.butLocks.Click += new System.EventHandler(this.butLocks_Click);
			// 
			// butUsers
			// 
			this.butUsers.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUsers.Autosize = true;
			this.butUsers.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUsers.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUsers.CornerRadius = 4F;
			this.butUsers.Location = new System.Drawing.Point(12, 76);
			this.butUsers.Name = "butUsers";
			this.butUsers.Size = new System.Drawing.Size(75, 23);
			this.butUsers.TabIndex = 220;
			this.butUsers.Text = "Users";
			this.butUsers.UseVisualStyleBackColor = true;
			this.butUsers.Click += new System.EventHandler(this.butUsers_Click);
			// 
			// butSecurity
			// 
			this.butSecurity.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSecurity.Autosize = true;
			this.butSecurity.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSecurity.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSecurity.CornerRadius = 4F;
			this.butSecurity.Location = new System.Drawing.Point(12, 48);
			this.butSecurity.Name = "butSecurity";
			this.butSecurity.Size = new System.Drawing.Size(75, 23);
			this.butSecurity.TabIndex = 219;
			this.butSecurity.Text = "Security";
			this.butSecurity.UseVisualStyleBackColor = true;
			this.butSecurity.Click += new System.EventHandler(this.butSecurity_Click);
			// 
			// label3
			// 
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label3.Location = new System.Drawing.Point(7, 22);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(154, 32);
			this.label3.TabIndex = 219;
			this.label3.Text = "Select connections before running any tools";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.butPtSearch);
			this.groupBox1.Location = new System.Drawing.Point(35, 58);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(98, 60);
			this.groupBox1.TabIndex = 220;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Search";
			// 
			// butPtSearch
			// 
			this.butPtSearch.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPtSearch.Autosize = true;
			this.butPtSearch.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPtSearch.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPtSearch.CornerRadius = 4F;
			this.butPtSearch.Location = new System.Drawing.Point(12, 21);
			this.butPtSearch.Name = "butPtSearch";
			this.butPtSearch.Size = new System.Drawing.Size(75, 23);
			this.butPtSearch.TabIndex = 215;
			this.butPtSearch.Text = "Patients";
			this.butPtSearch.UseVisualStyleBackColor = true;
			this.butPtSearch.Click += new System.EventHandler(this.butPtSearch_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.labelSyncMiddleTier);
			this.groupBox2.Controls.Add(this.butSyncManual);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.groupBox1);
			this.groupBox2.Controls.Add(this.groupBoxSync);
			this.groupBox2.Location = new System.Drawing.Point(793, 226);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(169, 280);
			this.groupBox2.TabIndex = 221;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Connection Tools";
			// 
			// labelSyncMiddleTier
			// 
			this.labelSyncMiddleTier.Location = new System.Drawing.Point(6, 233);
			this.labelSyncMiddleTier.Name = "labelSyncMiddleTier";
			this.labelSyncMiddleTier.Size = new System.Drawing.Size(157, 13);
			this.labelSyncMiddleTier.TabIndex = 223;
			this.labelSyncMiddleTier.Text = "Middle Tier Only";
			this.labelSyncMiddleTier.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.labelSyncMiddleTier.Visible = false;
			// 
			// butSyncManual
			// 
			this.butSyncManual.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSyncManual.Autosize = true;
			this.butSyncManual.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSyncManual.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSyncManual.CornerRadius = 4F;
			this.butSyncManual.Location = new System.Drawing.Point(47, 249);
			this.butSyncManual.Name = "butSyncManual";
			this.butSyncManual.Size = new System.Drawing.Size(75, 23);
			this.butSyncManual.TabIndex = 222;
			this.butSyncManual.Text = "Sync Manual";
			this.butSyncManual.UseVisualStyleBackColor = true;
			this.butSyncManual.Visible = false;
			this.butSyncManual.Click += new System.EventHandler(this.butSyncManual_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.butRefresh);
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Controls.Add(this.label4);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Controls.Add(this.textProviderSearch);
			this.groupBox3.Controls.Add(this.textClinicSearch);
			this.groupBox3.Controls.Add(this.textConnSearch);
			this.groupBox3.Location = new System.Drawing.Point(796, 22);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(169, 154);
			this.groupBox3.TabIndex = 222;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Search Connections";
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(88, 126);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 23);
			this.butRefresh.TabIndex = 216;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.UseVisualStyleBackColor = true;
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// label6
			// 
			this.label6.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label6.Location = new System.Drawing.Point(6, 87);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(157, 15);
			this.label6.TabIndex = 226;
			this.label6.Text = "Provider";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label4
			// 
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label4.Location = new System.Drawing.Point(6, 50);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(157, 15);
			this.label4.TabIndex = 224;
			this.label4.Text = "Clinic";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label5
			// 
			this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label5.Location = new System.Drawing.Point(6, 13);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(157, 15);
			this.label5.TabIndex = 225;
			this.label5.Text = "Connection Name";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// textProviderSearch
			// 
			this.textProviderSearch.Location = new System.Drawing.Point(6, 103);
			this.textProviderSearch.Name = "textProviderSearch";
			this.textProviderSearch.Size = new System.Drawing.Size(157, 20);
			this.textProviderSearch.TabIndex = 213;
			// 
			// textClinicSearch
			// 
			this.textClinicSearch.Location = new System.Drawing.Point(6, 66);
			this.textClinicSearch.Name = "textClinicSearch";
			this.textClinicSearch.Size = new System.Drawing.Size(157, 20);
			this.textClinicSearch.TabIndex = 212;
			// 
			// butEdit
			// 
			this.butEdit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butEdit.Autosize = true;
			this.butEdit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEdit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEdit.CornerRadius = 4F;
			this.butEdit.Location = new System.Drawing.Point(93, 493);
			this.butEdit.Name = "butEdit";
			this.butEdit.Size = new System.Drawing.Size(75, 23);
			this.butEdit.TabIndex = 217;
			this.butEdit.Text = "Edit";
			this.butEdit.UseVisualStyleBackColor = true;
			this.butEdit.Click += new System.EventHandler(this.butEdit_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Location = new System.Drawing.Point(12, 493);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(75, 23);
			this.butAdd.TabIndex = 216;
			this.butAdd.Text = "Add";
			this.butAdd.UseVisualStyleBackColor = true;
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 22);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(775, 461);
			this.gridMain.TabIndex = 5;
			this.gridMain.Title = "Connections - double click to launch";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butRefreshConns
			// 
			this.butRefreshConns.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefreshConns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butRefreshConns.Autosize = true;
			this.butRefreshConns.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefreshConns.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefreshConns.CornerRadius = 4F;
			this.butRefreshConns.Location = new System.Drawing.Point(227, 493);
			this.butRefreshConns.Name = "butRefreshConns";
			this.butRefreshConns.Size = new System.Drawing.Size(115, 23);
			this.butRefreshConns.TabIndex = 227;
			this.butRefreshConns.Text = "Refresh Connections";
			this.butRefreshConns.UseVisualStyleBackColor = true;
			this.butRefreshConns.Click += new System.EventHandler(this.butRefreshConns_Click);
			// 
			// labelVersion
			// 
			this.labelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelVersion.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelVersion.Location = new System.Drawing.Point(600, 5);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(169, 12);
			this.labelVersion.TabIndex = 227;
			this.labelVersion.Text = "Version";
			this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkAutoLog
			// 
			this.checkAutoLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.checkAutoLog.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAutoLog.Location = new System.Drawing.Point(522, 489);
			this.checkAutoLog.Name = "checkAutoLog";
			this.checkAutoLog.Size = new System.Drawing.Size(265, 17);
			this.checkAutoLog.TabIndex = 228;
			this.checkAutoLog.Text = "Automatically Log-On";
			this.checkAutoLog.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAutoLog.UseVisualStyleBackColor = true;
			// 
			// FormCentralManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(974, 527);
			this.Controls.Add(this.checkAutoLog);
			this.Controls.Add(this.labelVersion);
			this.Controls.Add(this.butRefreshConns);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.butEdit);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.comboConnectionGroups);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gridMain);
			this.Menu = this.mainMenu;
			this.MinimumSize = new System.Drawing.Size(799, 431);
			this.Name = "FormCentralManager";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Central Manager";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCentralManager_FormClosing);
			this.Load += new System.EventHandler(this.FormCentralManager_Load);
			this.groupBoxSync.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.ODGrid gridMain;
		private System.Windows.Forms.TextBox textConnSearch;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuItemSetup;
		private System.Windows.Forms.MenuItem menuItemReports;
		private System.Windows.Forms.MenuItem menuItemConnections;
		private System.Windows.Forms.MenuItem menuItemSecurity;
		private System.Windows.Forms.MenuItem menuItemAnnualPI;
		private System.Windows.Forms.MenuItem menuItemGroups;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboConnectionGroups;
		private OpenDental.UI.Button butPtSearch;
		private System.Windows.Forms.MenuItem menuItemLogoff;
		private System.Windows.Forms.MenuItem menuItemFile;
		private System.Windows.Forms.MenuItem menuItemPassword;
		private OpenDental.UI.Button butEdit;
		private OpenDental.UI.Button butAdd;
		private System.Windows.Forms.GroupBox groupBoxSync;
		private OpenDental.UI.Button butUsers;
		private OpenDental.UI.Button butSecurity;
		private OpenDental.UI.Button butLocks;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private OpenDental.UI.Button butRefresh;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textProviderSearch;
		private System.Windows.Forms.TextBox textClinicSearch;
		private OpenDental.UI.Button butRefreshConns;
		private System.Windows.Forms.Label labelVersion;
    private System.Windows.Forms.CheckBox checkAutoLog;
		private OpenDental.UI.Button butSyncManual;
		private System.Windows.Forms.Label labelSyncMiddleTier;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItemDisplayFields;
	}
}

