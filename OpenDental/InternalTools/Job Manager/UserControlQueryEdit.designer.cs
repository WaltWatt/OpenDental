namespace OpenDental.InternalTools.Job_Manager {
	partial class UserControlQueryEdit {
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
			this.components = new System.ComponentModel.Container();
			this.groupLinks = new System.Windows.Forms.GroupBox();
			this.gridFiles = new OpenDental.UI.ODGrid();
			this.gridAppointments = new OpenDental.UI.ODGrid();
			this.gridTasks = new OpenDental.UI.ODGrid();
			this.gridWatchers = new OpenDental.UI.ODGrid();
			this.timerTitle = new System.Windows.Forms.Timer(this.components);
			this.timerVersion = new System.Windows.Forms.Timer(this.components);
			this.panel3 = new System.Windows.Forms.Panel();
			this.gridRoles = new OpenDental.UI.ODGrid();
			this.label9 = new System.Windows.Forms.Label();
			this.textQuoteDate = new System.Windows.Forms.TextBox();
			this.checkApproved = new System.Windows.Forms.CheckBox();
			this.textQuoteAmount = new System.Windows.Forms.TextBox();
			this.textQuoteHours = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.comboPhase = new System.Windows.Forms.ComboBox();
			this.comboPriority = new System.Windows.Forms.ComboBox();
			this.textCustomer = new System.Windows.Forms.TextBox();
			this.textTitle = new System.Windows.Forms.TextBox();
			this.textDateEntry = new System.Windows.Forms.TextBox();
			this.textJobNum = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.panel4 = new System.Windows.Forms.Panel();
			this.butEmail = new OpenDental.UI.Button();
			this.butCommlog = new OpenDental.UI.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.textSchedDate = new System.Windows.Forms.TextBox();
			this.butActions = new OpenDental.UI.Button();
			this.panel5 = new System.Windows.Forms.Panel();
			this.splitContainerNoFlicker2 = new OpenDental.SplitContainerNoFlicker();
			this.textEditorMain = new OpenDental.OdtextEditor();
			this.tabControlMain = new System.Windows.Forms.TabControl();
			this.tabMain = new System.Windows.Forms.TabPage();
			this.gridNotes = new OpenDental.UI.ODGrid();
			this.tabReviews = new System.Windows.Forms.TabPage();
			this.gridReview = new OpenDental.UI.ODGrid();
			this.tabHistory = new System.Windows.Forms.TabPage();
			this.panel2 = new System.Windows.Forms.Panel();
			this.gridHistory = new OpenDental.UI.ODGrid();
			this.panel1 = new System.Windows.Forms.Panel();
			this.checkShowHistoryText = new System.Windows.Forms.CheckBox();
			this.splitContainerNoFlicker1 = new OpenDental.SplitContainerNoFlicker();
			this.groupLinks.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel4.SuspendLayout();
			this.panel5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerNoFlicker2)).BeginInit();
			this.splitContainerNoFlicker2.Panel1.SuspendLayout();
			this.splitContainerNoFlicker2.Panel2.SuspendLayout();
			this.splitContainerNoFlicker2.SuspendLayout();
			this.tabControlMain.SuspendLayout();
			this.tabMain.SuspendLayout();
			this.tabReviews.SuspendLayout();
			this.tabHistory.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerNoFlicker1)).BeginInit();
			this.splitContainerNoFlicker1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupLinks
			// 
			this.groupLinks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupLinks.Controls.Add(this.gridFiles);
			this.groupLinks.Controls.Add(this.gridAppointments);
			this.groupLinks.Controls.Add(this.gridTasks);
			this.groupLinks.Controls.Add(this.gridWatchers);
			this.groupLinks.Location = new System.Drawing.Point(4, 100);
			this.groupLinks.Name = "groupLinks";
			this.groupLinks.Size = new System.Drawing.Size(233, 619);
			this.groupLinks.TabIndex = 296;
			this.groupLinks.TabStop = false;
			this.groupLinks.Text = "Links";
			this.groupLinks.Resize += new System.EventHandler(this.groupLinks_Resize);
			// 
			// gridFiles
			// 
			this.gridFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridFiles.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridFiles.HasAddButton = true;
			this.gridFiles.HasDropDowns = false;
			this.gridFiles.HasMultilineHeaders = false;
			this.gridFiles.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridFiles.HeaderHeight = 15;
			this.gridFiles.HScrollVisible = false;
			this.gridFiles.Location = new System.Drawing.Point(4, 310);
			this.gridFiles.Name = "gridFiles";
			this.gridFiles.ScrollValue = 0;
			this.gridFiles.Size = new System.Drawing.Size(223, 91);
			this.gridFiles.TabIndex = 260;
			this.gridFiles.Title = "Files";
			this.gridFiles.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridFiles.TitleHeight = 18;
			this.gridFiles.TranslationName = "";
			this.gridFiles.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridFiles_CellDoubleClick);
			this.gridFiles.TitleAddClick += new System.EventHandler(this.gridFiles_TitleAddClick);
			this.gridFiles.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gridFiles_MouseMove);
			// 
			// gridAppointments
			// 
			this.gridAppointments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridAppointments.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridAppointments.HasAddButton = true;
			this.gridAppointments.HasDropDowns = false;
			this.gridAppointments.HasMultilineHeaders = false;
			this.gridAppointments.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridAppointments.HeaderHeight = 15;
			this.gridAppointments.HScrollVisible = false;
			this.gridAppointments.Location = new System.Drawing.Point(4, 213);
			this.gridAppointments.Name = "gridAppointments";
			this.gridAppointments.ScrollValue = 0;
			this.gridAppointments.Size = new System.Drawing.Size(223, 91);
			this.gridAppointments.TabIndex = 228;
			this.gridAppointments.Title = "Appointments";
			this.gridAppointments.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridAppointments.TitleHeight = 18;
			this.gridAppointments.TranslationName = "FormTaskEdit";
			this.gridAppointments.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAppointments_CellDoubleClick);
			this.gridAppointments.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAppointments_CellClick);
			this.gridAppointments.TitleAddClick += new System.EventHandler(this.gridAppointments_TitleAddClick);
			// 
			// gridTasks
			// 
			this.gridTasks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridTasks.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridTasks.HasAddButton = true;
			this.gridTasks.HasDropDowns = false;
			this.gridTasks.HasMultilineHeaders = false;
			this.gridTasks.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridTasks.HeaderHeight = 15;
			this.gridTasks.HScrollVisible = false;
			this.gridTasks.Location = new System.Drawing.Point(5, 116);
			this.gridTasks.Name = "gridTasks";
			this.gridTasks.ScrollValue = 0;
			this.gridTasks.Size = new System.Drawing.Size(223, 91);
			this.gridTasks.TabIndex = 227;
			this.gridTasks.Title = "Tasks";
			this.gridTasks.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridTasks.TitleHeight = 18;
			this.gridTasks.TranslationName = "FormTaskEdit";
			this.gridTasks.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridTasks_CellDoubleClick);
			this.gridTasks.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridTasks_CellClick);
			this.gridTasks.TitleAddClick += new System.EventHandler(this.gridTasks_TitleAddClick);
			// 
			// gridWatchers
			// 
			this.gridWatchers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridWatchers.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridWatchers.HasAddButton = true;
			this.gridWatchers.HasDropDowns = false;
			this.gridWatchers.HasMultilineHeaders = false;
			this.gridWatchers.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridWatchers.HeaderHeight = 15;
			this.gridWatchers.HScrollVisible = false;
			this.gridWatchers.Location = new System.Drawing.Point(5, 19);
			this.gridWatchers.Name = "gridWatchers";
			this.gridWatchers.ScrollValue = 0;
			this.gridWatchers.Size = new System.Drawing.Size(223, 91);
			this.gridWatchers.TabIndex = 225;
			this.gridWatchers.Title = "Watchers";
			this.gridWatchers.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridWatchers.TitleHeight = 18;
			this.gridWatchers.TranslationName = "FormTaskEdit";
			this.gridWatchers.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridWatchers_CellClick);
			this.gridWatchers.TitleAddClick += new System.EventHandler(this.gridWatchers_TitleAddClick);
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.gridRoles);
			this.panel3.Controls.Add(this.groupLinks);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel3.Location = new System.Drawing.Point(773, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(240, 726);
			this.panel3.TabIndex = 305;
			// 
			// gridRoles
			// 
			this.gridRoles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.gridRoles.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridRoles.HasAddButton = false;
			this.gridRoles.HasDropDowns = false;
			this.gridRoles.HasMultilineHeaders = false;
			this.gridRoles.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridRoles.HeaderHeight = 15;
			this.gridRoles.HScrollVisible = false;
			this.gridRoles.Location = new System.Drawing.Point(4, 0);
			this.gridRoles.Name = "gridRoles";
			this.gridRoles.ScrollValue = 0;
			this.gridRoles.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridRoles.Size = new System.Drawing.Size(232, 100);
			this.gridRoles.TabIndex = 304;
			this.gridRoles.Title = "Query Roles";
			this.gridRoles.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridRoles.TitleHeight = 18;
			this.gridRoles.TranslationName = "FormTaskEdit";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(345, 10);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(70, 13);
			this.label9.TabIndex = 328;
			this.label9.Text = "Quote Date";
			// 
			// textQuoteDate
			// 
			this.textQuoteDate.Location = new System.Drawing.Point(348, 26);
			this.textQuoteDate.Name = "textQuoteDate";
			this.textQuoteDate.Size = new System.Drawing.Size(101, 20);
			this.textQuoteDate.TabIndex = 327;
			this.textQuoteDate.Leave += new System.EventHandler(this.textQuoteDate_Leave);
			// 
			// checkApproved
			// 
			this.checkApproved.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkApproved.Location = new System.Drawing.Point(662, 70);
			this.checkApproved.Name = "checkApproved";
			this.checkApproved.Size = new System.Drawing.Size(74, 24);
			this.checkApproved.TabIndex = 326;
			this.checkApproved.Text = "Approved";
			this.checkApproved.UseVisualStyleBackColor = true;
			this.checkApproved.CheckedChanged += new System.EventHandler(this.checkApproved_CheckedChanged);
			// 
			// textQuoteAmount
			// 
			this.textQuoteAmount.Location = new System.Drawing.Point(267, 63);
			this.textQuoteAmount.Name = "textQuoteAmount";
			this.textQuoteAmount.Size = new System.Drawing.Size(67, 20);
			this.textQuoteAmount.TabIndex = 324;
			this.textQuoteAmount.Leave += new System.EventHandler(this.textQuoteAmount_Leave);
			// 
			// textQuoteHours
			// 
			this.textQuoteHours.Location = new System.Drawing.Point(267, 26);
			this.textQuoteHours.Name = "textQuoteHours";
			this.textQuoteHours.Size = new System.Drawing.Size(67, 20);
			this.textQuoteHours.TabIndex = 323;
			this.textQuoteHours.Leave += new System.EventHandler(this.textQuoteHours_Leave);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(264, 49);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(70, 13);
			this.label8.TabIndex = 322;
			this.label8.Text = "Quote Amt";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(264, 11);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(70, 13);
			this.label7.TabIndex = 321;
			this.label7.Text = "Quote Hours";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(123, 47);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(124, 13);
			this.label6.TabIndex = 320;
			this.label6.Text = "Phase";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(125, 10);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(122, 13);
			this.label5.TabIndex = 319;
			this.label5.Text = "Priority";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(458, 48);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(198, 13);
			this.label4.TabIndex = 318;
			this.label4.Text = "Customer";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(458, 10);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(118, 13);
			this.label3.TabIndex = 317;
			this.label3.Text = "Title";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(11, 49);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(103, 13);
			this.label2.TabIndex = 316;
			this.label2.Text = "Date Entry";
			// 
			// comboPhase
			// 
			this.comboPhase.FormattingEnabled = true;
			this.comboPhase.Location = new System.Drawing.Point(126, 63);
			this.comboPhase.Name = "comboPhase";
			this.comboPhase.Size = new System.Drawing.Size(121, 21);
			this.comboPhase.TabIndex = 315;
			this.comboPhase.SelectionChangeCommitted += new System.EventHandler(this.comboPhase_SelectionChangeCommitted);
			// 
			// comboPriority
			// 
			this.comboPriority.FormattingEnabled = true;
			this.comboPriority.Location = new System.Drawing.Point(126, 26);
			this.comboPriority.Name = "comboPriority";
			this.comboPriority.Size = new System.Drawing.Size(121, 21);
			this.comboPriority.TabIndex = 314;
			this.comboPriority.SelectionChangeCommitted += new System.EventHandler(this.comboPriority_SelectionChangeCommitted);
			// 
			// textCustomer
			// 
			this.textCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textCustomer.Location = new System.Drawing.Point(461, 64);
			this.textCustomer.Name = "textCustomer";
			this.textCustomer.ReadOnly = true;
			this.textCustomer.Size = new System.Drawing.Size(195, 20);
			this.textCustomer.TabIndex = 311;
			this.textCustomer.Click += new System.EventHandler(this.textCustomer_Click);
			// 
			// textTitle
			// 
			this.textTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textTitle.Location = new System.Drawing.Point(461, 26);
			this.textTitle.Name = "textTitle";
			this.textTitle.Size = new System.Drawing.Size(195, 20);
			this.textTitle.TabIndex = 309;
			this.textTitle.Leave += new System.EventHandler(this.textTitle_Leave);
			// 
			// textDateEntry
			// 
			this.textDateEntry.Location = new System.Drawing.Point(14, 64);
			this.textDateEntry.Name = "textDateEntry";
			this.textDateEntry.ReadOnly = true;
			this.textDateEntry.Size = new System.Drawing.Size(100, 20);
			this.textDateEntry.TabIndex = 307;
			// 
			// textJobNum
			// 
			this.textJobNum.Location = new System.Drawing.Point(14, 26);
			this.textJobNum.Name = "textJobNum";
			this.textJobNum.ReadOnly = true;
			this.textJobNum.Size = new System.Drawing.Size(100, 20);
			this.textJobNum.TabIndex = 305;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(11, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(103, 13);
			this.label1.TabIndex = 304;
			this.label1.Text = "JobNum";
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.butEmail);
			this.panel4.Controls.Add(this.butCommlog);
			this.panel4.Controls.Add(this.label10);
			this.panel4.Controls.Add(this.textSchedDate);
			this.panel4.Controls.Add(this.butActions);
			this.panel4.Controls.Add(this.comboPhase);
			this.panel4.Controls.Add(this.comboPriority);
			this.panel4.Controls.Add(this.label9);
			this.panel4.Controls.Add(this.textTitle);
			this.panel4.Controls.Add(this.label5);
			this.panel4.Controls.Add(this.label1);
			this.panel4.Controls.Add(this.textQuoteDate);
			this.panel4.Controls.Add(this.textJobNum);
			this.panel4.Controls.Add(this.label6);
			this.panel4.Controls.Add(this.textDateEntry);
			this.panel4.Controls.Add(this.textCustomer);
			this.panel4.Controls.Add(this.label7);
			this.panel4.Controls.Add(this.label4);
			this.panel4.Controls.Add(this.checkApproved);
			this.panel4.Controls.Add(this.label3);
			this.panel4.Controls.Add(this.label8);
			this.panel4.Controls.Add(this.label2);
			this.panel4.Controls.Add(this.textQuoteHours);
			this.panel4.Controls.Add(this.textQuoteAmount);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel4.Location = new System.Drawing.Point(0, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(773, 100);
			this.panel4.TabIndex = 329;
			// 
			// butEmail
			// 
			this.butEmail.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butEmail.Autosize = true;
			this.butEmail.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEmail.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEmail.CornerRadius = 4F;
			this.butEmail.Location = new System.Drawing.Point(712, 47);
			this.butEmail.Name = "butEmail";
			this.butEmail.Size = new System.Drawing.Size(58, 23);
			this.butEmail.TabIndex = 332;
			this.butEmail.Text = "Email";
			this.butEmail.UseVisualStyleBackColor = true;
			this.butEmail.Click += new System.EventHandler(this.butEmail_Click);
			// 
			// butCommlog
			// 
			this.butCommlog.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCommlog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butCommlog.Autosize = true;
			this.butCommlog.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCommlog.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCommlog.CornerRadius = 4F;
			this.butCommlog.Location = new System.Drawing.Point(712, 24);
			this.butCommlog.Name = "butCommlog";
			this.butCommlog.Size = new System.Drawing.Size(58, 23);
			this.butCommlog.TabIndex = 331;
			this.butCommlog.Text = "Commlog";
			this.butCommlog.UseVisualStyleBackColor = true;
			this.butCommlog.Click += new System.EventHandler(this.butCommlog_Click);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(345, 49);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(70, 13);
			this.label10.TabIndex = 330;
			this.label10.Text = "Sched Date";
			// 
			// textSchedDate
			// 
			this.textSchedDate.Location = new System.Drawing.Point(348, 63);
			this.textSchedDate.Name = "textSchedDate";
			this.textSchedDate.Size = new System.Drawing.Size(101, 20);
			this.textSchedDate.TabIndex = 329;
			this.textSchedDate.Leave += new System.EventHandler(this.textSchedDate_Leave);
			// 
			// butActions
			// 
			this.butActions.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butActions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butActions.Autosize = true;
			this.butActions.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butActions.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butActions.CornerRadius = 4F;
			this.butActions.Image = global::OpenDental.Properties.Resources.arrowDownTriangle;
			this.butActions.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.butActions.Location = new System.Drawing.Point(676, 0);
			this.butActions.Name = "butActions";
			this.butActions.Size = new System.Drawing.Size(95, 23);
			this.butActions.TabIndex = 303;
			this.butActions.Text = "Job Actions";
			this.butActions.Click += new System.EventHandler(this.butActions_Click);
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.splitContainerNoFlicker2);
			this.panel5.Controls.Add(this.splitContainerNoFlicker1);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel5.Location = new System.Drawing.Point(0, 100);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(773, 626);
			this.panel5.TabIndex = 330;
			// 
			// splitContainerNoFlicker2
			// 
			this.splitContainerNoFlicker2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerNoFlicker2.Location = new System.Drawing.Point(0, 0);
			this.splitContainerNoFlicker2.Name = "splitContainerNoFlicker2";
			// 
			// splitContainerNoFlicker2.Panel1
			// 
			this.splitContainerNoFlicker2.Panel1.Controls.Add(this.textEditorMain);
			// 
			// splitContainerNoFlicker2.Panel2
			// 
			this.splitContainerNoFlicker2.Panel2.Controls.Add(this.tabControlMain);
			this.splitContainerNoFlicker2.Size = new System.Drawing.Size(773, 626);
			this.splitContainerNoFlicker2.SplitterDistance = 501;
			this.splitContainerNoFlicker2.TabIndex = 308;
			// 
			// textEditorMain
			// 
			this.textEditorMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textEditorMain.HasEditorOptions = true;
			this.textEditorMain.HasSaveButton = true;
			this.textEditorMain.Location = new System.Drawing.Point(0, 0);
			this.textEditorMain.MainRtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft S" +
    "ans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs17\\par\r\n}\r\n";
			this.textEditorMain.MainText = "";
			this.textEditorMain.MinimumSize = new System.Drawing.Size(450, 120);
			this.textEditorMain.Name = "textEditorMain";
			this.textEditorMain.ReadOnly = false;
			this.textEditorMain.Size = new System.Drawing.Size(501, 626);
			this.textEditorMain.TabIndex = 306;
			this.textEditorMain.SaveClick += new OpenDental.ODtextEditorSaveEventHandler(this.textEditor_SaveClick);
			this.textEditorMain.OnTextEdited += new OpenDental.OdtextEditor.textChangedEventHandler(this.textEditorMain_OnTextEdited);
			// 
			// tabControlMain
			// 
			this.tabControlMain.Controls.Add(this.tabMain);
			this.tabControlMain.Controls.Add(this.tabReviews);
			this.tabControlMain.Controls.Add(this.tabHistory);
			this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlMain.Location = new System.Drawing.Point(0, 0);
			this.tabControlMain.Name = "tabControlMain";
			this.tabControlMain.SelectedIndex = 0;
			this.tabControlMain.Size = new System.Drawing.Size(268, 626);
			this.tabControlMain.TabIndex = 261;
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.gridNotes);
			this.tabMain.Location = new System.Drawing.Point(4, 22);
			this.tabMain.Name = "tabMain";
			this.tabMain.Padding = new System.Windows.Forms.Padding(3);
			this.tabMain.Size = new System.Drawing.Size(260, 600);
			this.tabMain.TabIndex = 0;
			this.tabMain.Text = "Discussion";
			this.tabMain.UseVisualStyleBackColor = true;
			// 
			// gridNotes
			// 
			this.gridNotes.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridNotes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridNotes.HasAddButton = true;
			this.gridNotes.HasDropDowns = false;
			this.gridNotes.HasMultilineHeaders = false;
			this.gridNotes.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridNotes.HeaderHeight = 15;
			this.gridNotes.HScrollVisible = false;
			this.gridNotes.Location = new System.Drawing.Point(3, 3);
			this.gridNotes.Name = "gridNotes";
			this.gridNotes.ScrollValue = 0;
			this.gridNotes.Size = new System.Drawing.Size(254, 594);
			this.gridNotes.TabIndex = 194;
			this.gridNotes.Title = "Discussion";
			this.gridNotes.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridNotes.TitleHeight = 18;
			this.gridNotes.TranslationName = "FormTaskEdit";
			this.gridNotes.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridNotes_CellDoubleClick);
			this.gridNotes.TitleAddClick += new System.EventHandler(this.gridNotes_TitleAddClick);
			// 
			// tabReviews
			// 
			this.tabReviews.Controls.Add(this.gridReview);
			this.tabReviews.Location = new System.Drawing.Point(4, 22);
			this.tabReviews.Name = "tabReviews";
			this.tabReviews.Padding = new System.Windows.Forms.Padding(3);
			this.tabReviews.Size = new System.Drawing.Size(260, 600);
			this.tabReviews.TabIndex = 4;
			this.tabReviews.Text = "Reviews";
			this.tabReviews.UseVisualStyleBackColor = true;
			// 
			// gridReview
			// 
			this.gridReview.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridReview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridReview.HasAddButton = true;
			this.gridReview.HasDropDowns = false;
			this.gridReview.HasMultilineHeaders = false;
			this.gridReview.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridReview.HeaderHeight = 15;
			this.gridReview.HScrollVisible = true;
			this.gridReview.Location = new System.Drawing.Point(3, 3);
			this.gridReview.Name = "gridReview";
			this.gridReview.ScrollValue = 0;
			this.gridReview.Size = new System.Drawing.Size(254, 594);
			this.gridReview.TabIndex = 22;
			this.gridReview.TabStop = false;
			this.gridReview.Title = "Reviews";
			this.gridReview.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridReview.TitleHeight = 18;
			this.gridReview.TranslationName = "TableReviews";
			this.gridReview.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridReview_CellDoubleClick);
			this.gridReview.TitleAddClick += new System.EventHandler(this.gridReview_TitleAddClick);
			// 
			// tabHistory
			// 
			this.tabHistory.BackColor = System.Drawing.SystemColors.Control;
			this.tabHistory.Controls.Add(this.panel2);
			this.tabHistory.Controls.Add(this.panel1);
			this.tabHistory.Location = new System.Drawing.Point(4, 22);
			this.tabHistory.Name = "tabHistory";
			this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
			this.tabHistory.Size = new System.Drawing.Size(260, 600);
			this.tabHistory.TabIndex = 3;
			this.tabHistory.Text = "History";
			// 
			// panel2
			// 
			this.panel2.AutoSize = true;
			this.panel2.Controls.Add(this.gridHistory);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(3, 20);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(254, 577);
			this.panel2.TabIndex = 247;
			// 
			// gridHistory
			// 
			this.gridHistory.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridHistory.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridHistory.HasAddButton = false;
			this.gridHistory.HasDropDowns = false;
			this.gridHistory.HasMultilineHeaders = false;
			this.gridHistory.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridHistory.HeaderHeight = 15;
			this.gridHistory.HScrollVisible = true;
			this.gridHistory.Location = new System.Drawing.Point(0, 0);
			this.gridHistory.Name = "gridHistory";
			this.gridHistory.ScrollValue = 0;
			this.gridHistory.Size = new System.Drawing.Size(254, 577);
			this.gridHistory.TabIndex = 19;
			this.gridHistory.TabStop = false;
			this.gridHistory.Title = "History Events";
			this.gridHistory.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridHistory.TitleHeight = 18;
			this.gridHistory.TranslationName = "TableHistory";
			this.gridHistory.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridHistory_CellDoubleClick);
			// 
			// panel1
			// 
			this.panel1.AutoSize = true;
			this.panel1.Controls.Add(this.checkShowHistoryText);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(254, 17);
			this.panel1.TabIndex = 246;
			// 
			// checkShowHistoryText
			// 
			this.checkShowHistoryText.AutoSize = true;
			this.checkShowHistoryText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.checkShowHistoryText.Location = new System.Drawing.Point(0, 0);
			this.checkShowHistoryText.Name = "checkShowHistoryText";
			this.checkShowHistoryText.Size = new System.Drawing.Size(254, 17);
			this.checkShowHistoryText.TabIndex = 245;
			this.checkShowHistoryText.Text = "Show Full Job Descriptions";
			this.checkShowHistoryText.UseVisualStyleBackColor = true;
			this.checkShowHistoryText.CheckedChanged += new System.EventHandler(this.checkShowHistoryText_CheckedChanged);
			// 
			// splitContainerNoFlicker1
			// 
			this.splitContainerNoFlicker1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerNoFlicker1.Location = new System.Drawing.Point(0, 0);
			this.splitContainerNoFlicker1.Name = "splitContainerNoFlicker1";
			this.splitContainerNoFlicker1.Size = new System.Drawing.Size(773, 626);
			this.splitContainerNoFlicker1.SplitterDistance = 257;
			this.splitContainerNoFlicker1.TabIndex = 307;
			// 
			// UserControlQueryEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel5);
			this.Controls.Add(this.panel4);
			this.Controls.Add(this.panel3);
			this.Name = "UserControlQueryEdit";
			this.Size = new System.Drawing.Size(1013, 726);
			this.groupLinks.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.panel5.ResumeLayout(false);
			this.splitContainerNoFlicker2.Panel1.ResumeLayout(false);
			this.splitContainerNoFlicker2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainerNoFlicker2)).EndInit();
			this.splitContainerNoFlicker2.ResumeLayout(false);
			this.tabControlMain.ResumeLayout(false);
			this.tabMain.ResumeLayout(false);
			this.tabReviews.ResumeLayout(false);
			this.tabHistory.ResumeLayout(false);
			this.tabHistory.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerNoFlicker1)).EndInit();
			this.splitContainerNoFlicker1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.GroupBox groupLinks;
		private UI.ODGrid gridTasks;
		private UI.ODGrid gridWatchers;
		private UI.ODGrid gridRoles;
		private System.Windows.Forms.Timer timerTitle;
		private System.Windows.Forms.Timer timerVersion;
		private UI.ODGrid gridFiles;
		private UI.ODGrid gridAppointments;
		private System.Windows.Forms.TabControl tabControlMain;
		private System.Windows.Forms.TabPage tabMain;
		private UI.ODGrid gridNotes;
		private System.Windows.Forms.TabPage tabHistory;
		private System.Windows.Forms.Panel panel2;
		private UI.ODGrid gridHistory;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.CheckBox checkShowHistoryText;
		private OdtextEditor textEditorMain;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox textQuoteDate;
		private System.Windows.Forms.CheckBox checkApproved;
		private System.Windows.Forms.TextBox textQuoteAmount;
		private System.Windows.Forms.TextBox textQuoteHours;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox comboPhase;
		private System.Windows.Forms.ComboBox comboPriority;
		private System.Windows.Forms.TextBox textCustomer;
		private System.Windows.Forms.TextBox textTitle;
		private System.Windows.Forms.TextBox textDateEntry;
		private System.Windows.Forms.TextBox textJobNum;
		private System.Windows.Forms.Label label1;
		private UI.Button butActions;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel5;
		private SplitContainerNoFlicker splitContainerNoFlicker2;
		private SplitContainerNoFlicker splitContainerNoFlicker1;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox textSchedDate;
		private System.Windows.Forms.TabPage tabReviews;
		private UI.ODGrid gridReview;
		private UI.Button butEmail;
		private UI.Button butCommlog;
	}
}
