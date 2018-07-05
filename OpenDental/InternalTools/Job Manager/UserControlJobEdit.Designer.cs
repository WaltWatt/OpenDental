namespace OpenDental.InternalTools.Job_Manager {
	partial class UserControlJobEdit {
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
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.label10 = new System.Windows.Forms.Label();
			this.textVersion = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textDateEntry = new System.Windows.Forms.TextBox();
			this.comboStatus = new System.Windows.Forms.ComboBox();
			this.comboCategory = new System.Windows.Forms.ComboBox();
			this.label12 = new System.Windows.Forms.Label();
			this.comboPriority = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.textJobNum = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textApprove = new System.Windows.Forms.TextBox();
			this.timerTitle = new System.Windows.Forms.Timer(this.components);
			this.timerVersion = new System.Windows.Forms.Timer(this.components);
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.timerEstimate = new System.Windows.Forms.Timer(this.components);
			this.gridCustomers = new OpenDental.UI.ODGrid();
			this.gridFiles = new OpenDental.UI.ODGrid();
			this.gridSubscribers = new OpenDental.UI.ODGrid();
			this.gridBugs = new OpenDental.UI.ODGrid();
			this.gridQuotes = new OpenDental.UI.ODGrid();
			this.gridFeatureReq = new OpenDental.UI.ODGrid();
			this.gridTasks = new OpenDental.UI.ODGrid();
			this.gridAppointments = new OpenDental.UI.ODGrid();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.tabControlMain = new System.Windows.Forms.TabControl();
			this.tabMain = new System.Windows.Forms.TabPage();
			this.gridNotes = new OpenDental.UI.ODGrid();
			this.tabReviews = new System.Windows.Forms.TabPage();
			this.gridReview = new OpenDental.UI.ODGrid();
			this.tabDocumentation = new System.Windows.Forms.TabPage();
			this.tabHistory = new System.Windows.Forms.TabPage();
			this.panel2 = new System.Windows.Forms.Panel();
			this.gridLog = new OpenDental.UI.ODGrid();
			this.panel1 = new System.Windows.Forms.Panel();
			this.checkShowHistoryText = new System.Windows.Forms.CheckBox();
			this.tabTesting = new System.Windows.Forms.TabPage();
			this.label9 = new System.Windows.Forms.Label();
			this.comboPriorityTesting = new System.Windows.Forms.ComboBox();
			this.gridTestingNotes = new OpenDental.UI.ODGrid();
			this.textTitle = new OpenDental.ODtextBox();
			this.butVersionPrompt = new OpenDental.UI.Button();
			this.splitContainerNoFlicker1 = new OpenDental.SplitContainerNoFlicker();
			this.gridRoles = new OpenDental.UI.ODGrid();
			this.labelRelatedJobs = new System.Windows.Forms.Label();
			this.treeRelatedJobs = new System.Windows.Forms.TreeView();
			this.butActions = new OpenDental.UI.Button();
			this.butAddTime = new OpenDental.UI.Button();
			this.textJobEditor = new OpenDental.ODjobTextEditor();
			this.butParentPick = new OpenDental.UI.Button();
			this.textParent = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textActualHours = new OpenDental.ValidDouble();
			this.label8 = new System.Windows.Forms.Label();
			this.butParentRemove = new OpenDental.UI.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.textEstHours = new OpenDental.ValidDouble();
			this.textEditorDocumentation = new OpenDental.OdtextEditor();
			this.groupLinks.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.tabControlMain.SuspendLayout();
			this.tabMain.SuspendLayout();
			this.tabReviews.SuspendLayout();
			this.tabDocumentation.SuspendLayout();
			this.tabHistory.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tabTesting.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerNoFlicker1)).BeginInit();
			this.splitContainerNoFlicker1.Panel1.SuspendLayout();
			this.splitContainerNoFlicker1.Panel2.SuspendLayout();
			this.splitContainerNoFlicker1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupLinks
			// 
			this.groupLinks.Controls.Add(this.tableLayoutPanel2);
			this.groupLinks.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupLinks.Location = new System.Drawing.Point(756, 63);
			this.groupLinks.Name = "groupLinks";
			this.groupLinks.Size = new System.Drawing.Size(254, 660);
			this.groupLinks.TabIndex = 296;
			this.groupLinks.TabStop = false;
			this.groupLinks.Text = "Links";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Controls.Add(this.gridCustomers, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.gridFiles, 0, 7);
			this.tableLayoutPanel2.Controls.Add(this.gridSubscribers, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.gridBugs, 0, 6);
			this.tableLayoutPanel2.Controls.Add(this.gridQuotes, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.gridFeatureReq, 0, 5);
			this.tableLayoutPanel2.Controls.Add(this.gridTasks, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.gridAppointments, 0, 4);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 8;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(248, 641);
			this.tableLayoutPanel2.TabIndex = 262;
			// 
			// label10
			// 
			this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label10.Location = new System.Drawing.Point(725, 4);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(63, 16);
			this.label10.TabIndex = 291;
			this.label10.Text = "Date Entry";
			this.label10.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textVersion
			// 
			this.textVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textVersion.Location = new System.Drawing.Point(832, 21);
			this.textVersion.MaxLength = 100;
			this.textVersion.Name = "textVersion";
			this.textVersion.Size = new System.Drawing.Size(146, 20);
			this.textVersion.TabIndex = 294;
			this.textVersion.TextChanged += new System.EventHandler(this.textVersion_TextChanged);
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.Location = new System.Drawing.Point(829, 4);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(63, 16);
			this.label6.TabIndex = 292;
			this.label6.Text = "Version";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textDateEntry
			// 
			this.textDateEntry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textDateEntry.Location = new System.Drawing.Point(728, 21);
			this.textDateEntry.MaxLength = 100;
			this.textDateEntry.Name = "textDateEntry";
			this.textDateEntry.ReadOnly = true;
			this.textDateEntry.Size = new System.Drawing.Size(98, 20);
			this.textDateEntry.TabIndex = 293;
			this.textDateEntry.TabStop = false;
			// 
			// comboStatus
			// 
			this.comboStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboStatus.Enabled = false;
			this.comboStatus.FormattingEnabled = true;
			this.comboStatus.Location = new System.Drawing.Point(402, 21);
			this.comboStatus.Name = "comboStatus";
			this.comboStatus.Size = new System.Drawing.Size(117, 21);
			this.comboStatus.TabIndex = 290;
			this.comboStatus.SelectionChangeCommitted += new System.EventHandler(this.comboStatus_SelectionChangeCommitted);
			// 
			// comboCategory
			// 
			this.comboCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCategory.FormattingEnabled = true;
			this.comboCategory.Location = new System.Drawing.Point(605, 21);
			this.comboCategory.Name = "comboCategory";
			this.comboCategory.Size = new System.Drawing.Size(117, 21);
			this.comboCategory.TabIndex = 287;
			this.comboCategory.SelectionChangeCommitted += new System.EventHandler(this.comboCategory_SelectionChangeCommitted);
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(1, 4);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(65, 16);
			this.label12.TabIndex = 289;
			this.label12.Text = "Title";
			this.label12.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboPriority
			// 
			this.comboPriority.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPriority.FormattingEnabled = true;
			this.comboPriority.Location = new System.Drawing.Point(279, 21);
			this.comboPriority.Name = "comboPriority";
			this.comboPriority.Size = new System.Drawing.Size(117, 21);
			this.comboPriority.TabIndex = 286;
			this.comboPriority.SelectionChangeCommitted += new System.EventHandler(this.comboPriority_SelectionChangeCommitted);
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Location = new System.Drawing.Point(602, 4);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(105, 16);
			this.label4.TabIndex = 282;
			this.label4.Text = "Category";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.Location = new System.Drawing.Point(399, 4);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(67, 16);
			this.label5.TabIndex = 283;
			this.label5.Text = "Phase";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Location = new System.Drawing.Point(276, 4);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(57, 16);
			this.label3.TabIndex = 281;
			this.label3.Text = "Priority";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label19
			// 
			this.label19.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label19.Location = new System.Drawing.Point(188, 4);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(61, 16);
			this.label19.TabIndex = 284;
			this.label19.Text = "JobNum";
			this.label19.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textJobNum
			// 
			this.textJobNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textJobNum.Location = new System.Drawing.Point(188, 21);
			this.textJobNum.MaxLength = 100;
			this.textJobNum.Name = "textJobNum";
			this.textJobNum.ReadOnly = true;
			this.textJobNum.Size = new System.Drawing.Size(85, 20);
			this.textJobNum.TabIndex = 285;
			this.textJobNum.TabStop = false;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Location = new System.Drawing.Point(525, 4);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(71, 16);
			this.label2.TabIndex = 302;
			this.label2.Text = "Approved";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textApprove
			// 
			this.textApprove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textApprove.Location = new System.Drawing.Point(525, 21);
			this.textApprove.MaxLength = 100;
			this.textApprove.Name = "textApprove";
			this.textApprove.ReadOnly = true;
			this.textApprove.Size = new System.Drawing.Size(74, 20);
			this.textApprove.TabIndex = 303;
			this.textApprove.TabStop = false;
			// 
			// timerTitle
			// 
			this.timerTitle.Interval = 3000;
			this.timerTitle.Tick += new System.EventHandler(this.timerTitle_Tick);
			// 
			// timerVersion
			// 
			this.timerVersion.Interval = 3000;
			this.timerVersion.Tick += new System.EventHandler(this.timerVersion_Tick);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260F));
			this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.groupLinks, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.splitContainer2, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1013, 726);
			this.tableLayoutPanel1.TabIndex = 309;
			// 
			// panel3
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.panel3, 2);
			this.panel3.Controls.Add(this.textTitle);
			this.panel3.Controls.Add(this.textVersion);
			this.panel3.Controls.Add(this.textJobNum);
			this.panel3.Controls.Add(this.butVersionPrompt);
			this.panel3.Controls.Add(this.label19);
			this.panel3.Controls.Add(this.label10);
			this.panel3.Controls.Add(this.label2);
			this.panel3.Controls.Add(this.label3);
			this.panel3.Controls.Add(this.textApprove);
			this.panel3.Controls.Add(this.label5);
			this.panel3.Controls.Add(this.label4);
			this.panel3.Controls.Add(this.comboPriority);
			this.panel3.Controls.Add(this.label12);
			this.panel3.Controls.Add(this.label6);
			this.panel3.Controls.Add(this.comboCategory);
			this.panel3.Controls.Add(this.textDateEntry);
			this.panel3.Controls.Add(this.comboStatus);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(3, 3);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(1007, 54);
			this.panel3.TabIndex = 308;
			// 
			// timerEstimate
			// 
			this.timerEstimate.Interval = 3000;
			this.timerEstimate.Tick += new System.EventHandler(this.timerEstimate_Tick);
			// 
			// gridCustomers
			// 
			this.gridCustomers.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridCustomers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridCustomers.HasAddButton = true;
			this.gridCustomers.HasDropDowns = false;
			this.gridCustomers.HasMultilineHeaders = false;
			this.gridCustomers.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridCustomers.HeaderHeight = 15;
			this.gridCustomers.HScrollVisible = false;
			this.gridCustomers.Location = new System.Drawing.Point(3, 3);
			this.gridCustomers.Name = "gridCustomers";
			this.gridCustomers.ScrollValue = 0;
			this.gridCustomers.Size = new System.Drawing.Size(242, 74);
			this.gridCustomers.TabIndex = 262;
			this.gridCustomers.Title = "Customers";
			this.gridCustomers.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridCustomers.TitleHeight = 18;
			this.gridCustomers.TranslationName = "FormTaskEdit";
			this.gridCustomers.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridCustomers_CellClick);
			this.gridCustomers.TitleAddClick += new System.EventHandler(this.gridCustomers_TitleAddClick);
			// 
			// gridFiles
			// 
			this.gridFiles.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridFiles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridFiles.HasAddButton = true;
			this.gridFiles.HasDropDowns = false;
			this.gridFiles.HasMultilineHeaders = false;
			this.gridFiles.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridFiles.HeaderHeight = 15;
			this.gridFiles.HScrollVisible = false;
			this.gridFiles.Location = new System.Drawing.Point(3, 563);
			this.gridFiles.Name = "gridFiles";
			this.gridFiles.ScrollValue = 0;
			this.gridFiles.Size = new System.Drawing.Size(242, 75);
			this.gridFiles.TabIndex = 260;
			this.gridFiles.Title = "Files";
			this.gridFiles.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridFiles.TitleHeight = 18;
			this.gridFiles.TranslationName = "";
			this.gridFiles.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridFiles_CellDoubleClick);
			this.gridFiles.TitleAddClick += new System.EventHandler(this.gridFiles_TitleAddClick);
			// 
			// gridSubscribers
			// 
			this.gridSubscribers.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridSubscribers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridSubscribers.HasAddButton = true;
			this.gridSubscribers.HasDropDowns = false;
			this.gridSubscribers.HasMultilineHeaders = false;
			this.gridSubscribers.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridSubscribers.HeaderHeight = 15;
			this.gridSubscribers.HScrollVisible = false;
			this.gridSubscribers.Location = new System.Drawing.Point(3, 83);
			this.gridSubscribers.Name = "gridSubscribers";
			this.gridSubscribers.ScrollValue = 0;
			this.gridSubscribers.Size = new System.Drawing.Size(242, 74);
			this.gridSubscribers.TabIndex = 225;
			this.gridSubscribers.Title = "Subscribers";
			this.gridSubscribers.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridSubscribers.TitleHeight = 18;
			this.gridSubscribers.TranslationName = "FormTaskEdit";
			this.gridSubscribers.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridWatchers_CellClick);
			this.gridSubscribers.TitleAddClick += new System.EventHandler(this.gridWatchers_TitleAddClick);
			// 
			// gridBugs
			// 
			this.gridBugs.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridBugs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridBugs.HasAddButton = true;
			this.gridBugs.HasDropDowns = false;
			this.gridBugs.HasMultilineHeaders = false;
			this.gridBugs.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridBugs.HeaderHeight = 15;
			this.gridBugs.HScrollVisible = false;
			this.gridBugs.Location = new System.Drawing.Point(3, 483);
			this.gridBugs.Name = "gridBugs";
			this.gridBugs.ScrollValue = 0;
			this.gridBugs.Size = new System.Drawing.Size(242, 74);
			this.gridBugs.TabIndex = 259;
			this.gridBugs.Title = "Bugs/Enhancements";
			this.gridBugs.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridBugs.TitleHeight = 18;
			this.gridBugs.TranslationName = "FormTaskEdit";
			this.gridBugs.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridBugs_CellDoubleClick);
			this.gridBugs.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridBugs_CellClick);
			this.gridBugs.TitleAddClick += new System.EventHandler(this.gridBugs_TitleAddClick);
			// 
			// gridQuotes
			// 
			this.gridQuotes.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridQuotes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridQuotes.HasAddButton = true;
			this.gridQuotes.HasDropDowns = false;
			this.gridQuotes.HasMultilineHeaders = false;
			this.gridQuotes.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridQuotes.HeaderHeight = 15;
			this.gridQuotes.HScrollVisible = false;
			this.gridQuotes.Location = new System.Drawing.Point(3, 163);
			this.gridQuotes.Name = "gridQuotes";
			this.gridQuotes.ScrollValue = 0;
			this.gridQuotes.Size = new System.Drawing.Size(242, 74);
			this.gridQuotes.TabIndex = 226;
			this.gridQuotes.Title = "Quotes";
			this.gridQuotes.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridQuotes.TitleHeight = 18;
			this.gridQuotes.TranslationName = "FormTaskEdit";
			this.gridQuotes.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridCustomerQuotes_CellDoubleClick);
			this.gridQuotes.TitleAddClick += new System.EventHandler(this.gridCustomerQuotes_TitleAddClick);
			// 
			// gridFeatureReq
			// 
			this.gridFeatureReq.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridFeatureReq.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridFeatureReq.HasAddButton = true;
			this.gridFeatureReq.HasDropDowns = false;
			this.gridFeatureReq.HasMultilineHeaders = false;
			this.gridFeatureReq.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridFeatureReq.HeaderHeight = 15;
			this.gridFeatureReq.HScrollVisible = false;
			this.gridFeatureReq.Location = new System.Drawing.Point(3, 403);
			this.gridFeatureReq.Name = "gridFeatureReq";
			this.gridFeatureReq.ScrollValue = 0;
			this.gridFeatureReq.Size = new System.Drawing.Size(242, 74);
			this.gridFeatureReq.TabIndex = 228;
			this.gridFeatureReq.Title = "Feature Requests";
			this.gridFeatureReq.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridFeatureReq.TitleHeight = 18;
			this.gridFeatureReq.TranslationName = "FormTaskEdit";
			this.gridFeatureReq.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridFeatureReq_CellDoubleClick);
			this.gridFeatureReq.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridFeatureReq_CellClick);
			this.gridFeatureReq.TitleAddClick += new System.EventHandler(this.gridFeatureReq_TitleAddClick);
			// 
			// gridTasks
			// 
			this.gridTasks.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridTasks.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridTasks.HasAddButton = true;
			this.gridTasks.HasDropDowns = false;
			this.gridTasks.HasMultilineHeaders = false;
			this.gridTasks.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridTasks.HeaderHeight = 15;
			this.gridTasks.HScrollVisible = false;
			this.gridTasks.Location = new System.Drawing.Point(3, 243);
			this.gridTasks.Name = "gridTasks";
			this.gridTasks.ScrollValue = 0;
			this.gridTasks.Size = new System.Drawing.Size(242, 74);
			this.gridTasks.TabIndex = 227;
			this.gridTasks.Title = "Tasks";
			this.gridTasks.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridTasks.TitleHeight = 18;
			this.gridTasks.TranslationName = "FormTaskEdit";
			this.gridTasks.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridTasks_CellDoubleClick);
			this.gridTasks.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridTasks_CellClick);
			this.gridTasks.TitleAddClick += new System.EventHandler(this.gridTasks_TitleAddClick);
			// 
			// gridAppointments
			// 
			this.gridAppointments.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridAppointments.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridAppointments.HasAddButton = true;
			this.gridAppointments.HasDropDowns = false;
			this.gridAppointments.HasMultilineHeaders = false;
			this.gridAppointments.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridAppointments.HeaderHeight = 15;
			this.gridAppointments.HScrollVisible = false;
			this.gridAppointments.Location = new System.Drawing.Point(3, 323);
			this.gridAppointments.Name = "gridAppointments";
			this.gridAppointments.ScrollValue = 0;
			this.gridAppointments.Size = new System.Drawing.Size(242, 74);
			this.gridAppointments.TabIndex = 261;
			this.gridAppointments.Title = "Appointments";
			this.gridAppointments.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridAppointments.TitleHeight = 18;
			this.gridAppointments.TranslationName = "gridAppts";
			this.gridAppointments.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAppointments_CellDoubleClick);
			this.gridAppointments.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAppointments_CellClick);
			this.gridAppointments.TitleAddClick += new System.EventHandler(this.gridAppointments_TitleAddClick);
			// 
			// splitContainer2
			// 
			this.splitContainer2.BackColor = System.Drawing.SystemColors.ControlDark;
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(3, 63);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.Control;
			this.splitContainer2.Panel1.Controls.Add(this.splitContainerNoFlicker1);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.Control;
			this.splitContainer2.Panel2.Controls.Add(this.tabControlMain);
			this.splitContainer2.Panel2MinSize = 250;
			this.splitContainer2.Size = new System.Drawing.Size(747, 660);
			this.splitContainer2.SplitterDistance = 373;
			this.splitContainer2.TabIndex = 301;
			// 
			// tabControlMain
			// 
			this.tabControlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControlMain.Controls.Add(this.tabMain);
			this.tabControlMain.Controls.Add(this.tabReviews);
			this.tabControlMain.Controls.Add(this.tabDocumentation);
			this.tabControlMain.Controls.Add(this.tabHistory);
			this.tabControlMain.Controls.Add(this.tabTesting);
			this.tabControlMain.Location = new System.Drawing.Point(0, 0);
			this.tabControlMain.Name = "tabControlMain";
			this.tabControlMain.SelectedIndex = 0;
			this.tabControlMain.Size = new System.Drawing.Size(747, 281);
			this.tabControlMain.TabIndex = 261;
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.gridNotes);
			this.tabMain.Location = new System.Drawing.Point(4, 22);
			this.tabMain.Name = "tabMain";
			this.tabMain.Padding = new System.Windows.Forms.Padding(3);
			this.tabMain.Size = new System.Drawing.Size(739, 255);
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
			this.gridNotes.Size = new System.Drawing.Size(733, 249);
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
			this.tabReviews.BackColor = System.Drawing.SystemColors.Control;
			this.tabReviews.Controls.Add(this.gridReview);
			this.tabReviews.Location = new System.Drawing.Point(4, 22);
			this.tabReviews.Name = "tabReviews";
			this.tabReviews.Padding = new System.Windows.Forms.Padding(3);
			this.tabReviews.Size = new System.Drawing.Size(739, 255);
			this.tabReviews.TabIndex = 2;
			this.tabReviews.Text = "Reviews";
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
			this.gridReview.HScrollVisible = false;
			this.gridReview.Location = new System.Drawing.Point(3, 3);
			this.gridReview.Name = "gridReview";
			this.gridReview.ScrollValue = 0;
			this.gridReview.Size = new System.Drawing.Size(733, 249);
			this.gridReview.TabIndex = 21;
			this.gridReview.TabStop = false;
			this.gridReview.Title = "Reviews";
			this.gridReview.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridReview.TitleHeight = 18;
			this.gridReview.TranslationName = "TableReviews";
			this.gridReview.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridReview_CellDoubleClick);
			this.gridReview.TitleAddClick += new System.EventHandler(this.gridReview_TitleAddClick);
			// 
			// tabDocumentation
			// 
			this.tabDocumentation.BackColor = System.Drawing.SystemColors.Control;
			this.tabDocumentation.Controls.Add(this.textEditorDocumentation);
			this.tabDocumentation.Location = new System.Drawing.Point(4, 22);
			this.tabDocumentation.Name = "tabDocumentation";
			this.tabDocumentation.Padding = new System.Windows.Forms.Padding(3);
			this.tabDocumentation.Size = new System.Drawing.Size(739, 255);
			this.tabDocumentation.TabIndex = 4;
			this.tabDocumentation.Text = "Documentation";
			// 
			// tabHistory
			// 
			this.tabHistory.BackColor = System.Drawing.SystemColors.Control;
			this.tabHistory.Controls.Add(this.panel2);
			this.tabHistory.Controls.Add(this.panel1);
			this.tabHistory.Location = new System.Drawing.Point(4, 22);
			this.tabHistory.Name = "tabHistory";
			this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
			this.tabHistory.Size = new System.Drawing.Size(739, 255);
			this.tabHistory.TabIndex = 3;
			this.tabHistory.Text = "Log Events";
			// 
			// panel2
			// 
			this.panel2.AutoSize = true;
			this.panel2.Controls.Add(this.gridLog);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(3, 20);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(733, 232);
			this.panel2.TabIndex = 247;
			// 
			// gridLog
			// 
			this.gridLog.AutoSize = true;
			this.gridLog.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridLog.HasAddButton = false;
			this.gridLog.HasDropDowns = false;
			this.gridLog.HasMultilineHeaders = false;
			this.gridLog.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridLog.HeaderHeight = 15;
			this.gridLog.HScrollVisible = false;
			this.gridLog.Location = new System.Drawing.Point(0, 0);
			this.gridLog.Name = "gridLog";
			this.gridLog.ScrollValue = 0;
			this.gridLog.Size = new System.Drawing.Size(733, 232);
			this.gridLog.TabIndex = 19;
			this.gridLog.TabStop = false;
			this.gridLog.Title = "Log Events";
			this.gridLog.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridLog.TitleHeight = 18;
			this.gridLog.TranslationName = "TableHistoryEvents";
			this.gridLog.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridHistory_CellDoubleClick);
			// 
			// panel1
			// 
			this.panel1.AutoSize = true;
			this.panel1.Controls.Add(this.checkShowHistoryText);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(733, 17);
			this.panel1.TabIndex = 246;
			// 
			// checkShowHistoryText
			// 
			this.checkShowHistoryText.AutoSize = true;
			this.checkShowHistoryText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.checkShowHistoryText.Location = new System.Drawing.Point(0, 0);
			this.checkShowHistoryText.Name = "checkShowHistoryText";
			this.checkShowHistoryText.Size = new System.Drawing.Size(733, 17);
			this.checkShowHistoryText.TabIndex = 245;
			this.checkShowHistoryText.Text = "Show Full Job Descriptions";
			this.checkShowHistoryText.UseVisualStyleBackColor = true;
			this.checkShowHistoryText.CheckedChanged += new System.EventHandler(this.checkShowHistoryText_CheckedChanged);
			// 
			// tabTesting
			// 
			this.tabTesting.BackColor = System.Drawing.SystemColors.Control;
			this.tabTesting.Controls.Add(this.label9);
			this.tabTesting.Controls.Add(this.comboPriorityTesting);
			this.tabTesting.Controls.Add(this.gridTestingNotes);
			this.tabTesting.Location = new System.Drawing.Point(4, 22);
			this.tabTesting.Name = "tabTesting";
			this.tabTesting.Padding = new System.Windows.Forms.Padding(3);
			this.tabTesting.Size = new System.Drawing.Size(739, 255);
			this.tabTesting.TabIndex = 5;
			this.tabTesting.Text = "Testing";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(129, 8);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(57, 16);
			this.label9.TabIndex = 288;
			this.label9.Text = "Priority";
			this.label9.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboPriorityTesting
			// 
			this.comboPriorityTesting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPriorityTesting.FormattingEnabled = true;
			this.comboPriorityTesting.Location = new System.Drawing.Point(6, 7);
			this.comboPriorityTesting.Name = "comboPriorityTesting";
			this.comboPriorityTesting.Size = new System.Drawing.Size(117, 21);
			this.comboPriorityTesting.TabIndex = 287;
			this.comboPriorityTesting.SelectionChangeCommitted += new System.EventHandler(this.comboPriorityTesting_SelectionChangeCommitted);
			// 
			// gridTestingNotes
			// 
			this.gridTestingNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridTestingNotes.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridTestingNotes.HasAddButton = true;
			this.gridTestingNotes.HasDropDowns = false;
			this.gridTestingNotes.HasMultilineHeaders = false;
			this.gridTestingNotes.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridTestingNotes.HeaderHeight = 15;
			this.gridTestingNotes.HScrollVisible = false;
			this.gridTestingNotes.Location = new System.Drawing.Point(3, 33);
			this.gridTestingNotes.Name = "gridTestingNotes";
			this.gridTestingNotes.ScrollValue = 0;
			this.gridTestingNotes.Size = new System.Drawing.Size(733, 219);
			this.gridTestingNotes.TabIndex = 195;
			this.gridTestingNotes.Title = "Notes";
			this.gridTestingNotes.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridTestingNotes.TitleHeight = 18;
			this.gridTestingNotes.TranslationName = "TableTestingNotes";
			this.gridTestingNotes.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridTestingNotes_CellDoubleClick);
			this.gridTestingNotes.TitleAddClick += new System.EventHandler(this.gridTestingNotes_TitleAddClick);
			// 
			// textTitle
			// 
			this.textTitle.AcceptsTab = true;
			this.textTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textTitle.BackColor = System.Drawing.SystemColors.Window;
			this.textTitle.DetectLinksEnabled = false;
			this.textTitle.DetectUrls = false;
			this.textTitle.Location = new System.Drawing.Point(7, 21);
			this.textTitle.Multiline = false;
			this.textTitle.Name = "textTitle";
			this.textTitle.QuickPasteType = OpenDentBusiness.QuickPasteType.JobManager;
			this.textTitle.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textTitle.Size = new System.Drawing.Size(175, 21);
			this.textTitle.TabIndex = 309;
			this.textTitle.Text = "";
			this.textTitle.TextChanged += new System.EventHandler(this.textTitle_TextChanged);
			// 
			// butVersionPrompt
			// 
			this.butVersionPrompt.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butVersionPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butVersionPrompt.Autosize = true;
			this.butVersionPrompt.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butVersionPrompt.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butVersionPrompt.CornerRadius = 4F;
			this.butVersionPrompt.Location = new System.Drawing.Point(981, 21);
			this.butVersionPrompt.Name = "butVersionPrompt";
			this.butVersionPrompt.Size = new System.Drawing.Size(23, 20);
			this.butVersionPrompt.TabIndex = 308;
			this.butVersionPrompt.Text = "...";
			this.butVersionPrompt.Click += new System.EventHandler(this.butVersionPrompt_Click);
			// 
			// splitContainerNoFlicker1
			// 
			this.splitContainerNoFlicker1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerNoFlicker1.Location = new System.Drawing.Point(0, 0);
			this.splitContainerNoFlicker1.Name = "splitContainerNoFlicker1";
			// 
			// splitContainerNoFlicker1.Panel1
			// 
			this.splitContainerNoFlicker1.Panel1.Controls.Add(this.gridRoles);
			this.splitContainerNoFlicker1.Panel1.Controls.Add(this.labelRelatedJobs);
			this.splitContainerNoFlicker1.Panel1.Controls.Add(this.treeRelatedJobs);
			this.splitContainerNoFlicker1.Panel1.Controls.Add(this.butActions);
			// 
			// splitContainerNoFlicker1.Panel2
			// 
			this.splitContainerNoFlicker1.Panel2.Controls.Add(this.butAddTime);
			this.splitContainerNoFlicker1.Panel2.Controls.Add(this.textJobEditor);
			this.splitContainerNoFlicker1.Panel2.Controls.Add(this.butParentPick);
			this.splitContainerNoFlicker1.Panel2.Controls.Add(this.textParent);
			this.splitContainerNoFlicker1.Panel2.Controls.Add(this.label1);
			this.splitContainerNoFlicker1.Panel2.Controls.Add(this.textActualHours);
			this.splitContainerNoFlicker1.Panel2.Controls.Add(this.label8);
			this.splitContainerNoFlicker1.Panel2.Controls.Add(this.butParentRemove);
			this.splitContainerNoFlicker1.Panel2.Controls.Add(this.label7);
			this.splitContainerNoFlicker1.Panel2.Controls.Add(this.textEstHours);
			this.splitContainerNoFlicker1.Size = new System.Drawing.Size(747, 373);
			this.splitContainerNoFlicker1.SplitterDistance = 179;
			this.splitContainerNoFlicker1.TabIndex = 310;
			// 
			// gridRoles
			// 
			this.gridRoles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridRoles.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridRoles.HasAddButton = false;
			this.gridRoles.HasDropDowns = false;
			this.gridRoles.HasMultilineHeaders = false;
			this.gridRoles.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridRoles.HeaderHeight = 15;
			this.gridRoles.HScrollVisible = false;
			this.gridRoles.Location = new System.Drawing.Point(0, 31);
			this.gridRoles.Name = "gridRoles";
			this.gridRoles.ScrollValue = 0;
			this.gridRoles.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridRoles.Size = new System.Drawing.Size(177, 244);
			this.gridRoles.TabIndex = 304;
			this.gridRoles.Title = "JobRoles";
			this.gridRoles.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridRoles.TitleHeight = 18;
			this.gridRoles.TranslationName = "FormTaskEdit";
			// 
			// labelRelatedJobs
			// 
			this.labelRelatedJobs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelRelatedJobs.Location = new System.Drawing.Point(0, 278);
			this.labelRelatedJobs.Name = "labelRelatedJobs";
			this.labelRelatedJobs.Size = new System.Drawing.Size(177, 20);
			this.labelRelatedJobs.TabIndex = 309;
			this.labelRelatedJobs.Text = "Related Jobs";
			this.labelRelatedJobs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// treeRelatedJobs
			// 
			this.treeRelatedJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treeRelatedJobs.Indent = 9;
			this.treeRelatedJobs.Location = new System.Drawing.Point(0, 301);
			this.treeRelatedJobs.Name = "treeRelatedJobs";
			this.treeRelatedJobs.Size = new System.Drawing.Size(177, 64);
			this.treeRelatedJobs.TabIndex = 308;
			this.treeRelatedJobs.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeRelatedJobs_AfterSelect);
			this.treeRelatedJobs.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeRelatedJobs_NodeMouseClick);
			// 
			// butActions
			// 
			this.butActions.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butActions.Autosize = true;
			this.butActions.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butActions.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butActions.CornerRadius = 4F;
			this.butActions.Image = global::OpenDental.Properties.Resources.arrowDownTriangle;
			this.butActions.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.butActions.Location = new System.Drawing.Point(0, 3);
			this.butActions.Name = "butActions";
			this.butActions.Size = new System.Drawing.Size(95, 24);
			this.butActions.TabIndex = 303;
			this.butActions.Text = "Job Actions";
			this.butActions.Click += new System.EventHandler(this.butActions_Click);
			// 
			// butAddTime
			// 
			this.butAddTime.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAddTime.Autosize = true;
			this.butAddTime.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddTime.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddTime.CornerRadius = 4F;
			this.butAddTime.Location = new System.Drawing.Point(245, 348);
			this.butAddTime.Name = "butAddTime";
			this.butAddTime.Size = new System.Drawing.Size(58, 20);
			this.butAddTime.TabIndex = 308;
			this.butAddTime.Text = "Add Time";
			this.butAddTime.Click += new System.EventHandler(this.butAddTime_Click);
			// 
			// textJobEditor
			// 
			this.textJobEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textJobEditor.HasEditorOptions = true;
			this.textJobEditor.HasSaveButton = true;
			this.textJobEditor.ImplementationRtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft S" +
    "ans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs17\\par\r\n}\r\n";
			this.textJobEditor.ImplementationText = "";
			this.textJobEditor.Location = new System.Drawing.Point(0, 0);
			this.textJobEditor.Name = "textJobEditor";
			this.textJobEditor.ReadOnlyImplementation = false;
			this.textJobEditor.ReadOnlyRequirements = false;
			this.textJobEditor.RequirementsRtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft S" +
    "ans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs17\\par\r\n}\r\n";
			this.textJobEditor.RequirementsText = "";
			this.textJobEditor.Size = new System.Drawing.Size(564, 345);
			this.textJobEditor.TabIndex = 20;
			this.textJobEditor.SaveClick += new OpenDental.ODtextEditorSaveEventHandler(this.textEditor_SaveClick);
			this.textJobEditor.OnTextEdited += new OpenDental.ODjobTextEditor.textChangedEventHandler(this.textEditor_OnTextEdited);
			// 
			// butParentPick
			// 
			this.butParentPick.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butParentPick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butParentPick.Autosize = true;
			this.butParentPick.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butParentPick.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butParentPick.CornerRadius = 4F;
			this.butParentPick.Location = new System.Drawing.Point(515, 348);
			this.butParentPick.Name = "butParentPick";
			this.butParentPick.Size = new System.Drawing.Size(23, 20);
			this.butParentPick.TabIndex = 307;
			this.butParentPick.Text = "...";
			this.butParentPick.Click += new System.EventHandler(this.butParentPick_Click);
			// 
			// textParent
			// 
			this.textParent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textParent.Location = new System.Drawing.Point(373, 348);
			this.textParent.MaxLength = 100;
			this.textParent.Name = "textParent";
			this.textParent.ReadOnly = true;
			this.textParent.Size = new System.Drawing.Size(142, 20);
			this.textParent.TabIndex = 304;
			this.textParent.TabStop = false;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.Location = new System.Drawing.Point(309, 348);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(63, 20);
			this.label1.TabIndex = 305;
			this.label1.Text = "Parent Job";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textActualHours
			// 
			this.textActualHours.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textActualHours.Location = new System.Drawing.Point(193, 348);
			this.textActualHours.MaxVal = 1000000D;
			this.textActualHours.MinVal = 0D;
			this.textActualHours.Name = "textActualHours";
			this.textActualHours.ReadOnly = true;
			this.textActualHours.Size = new System.Drawing.Size(46, 20);
			this.textActualHours.TabIndex = 270;
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label8.Location = new System.Drawing.Point(128, 347);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(65, 20);
			this.label8.TabIndex = 265;
			this.label8.Text = "Hrs. So Far";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butParentRemove
			// 
			this.butParentRemove.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butParentRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butParentRemove.Autosize = true;
			this.butParentRemove.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butParentRemove.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butParentRemove.CornerRadius = 4F;
			this.butParentRemove.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butParentRemove.Location = new System.Drawing.Point(538, 348);
			this.butParentRemove.Name = "butParentRemove";
			this.butParentRemove.Size = new System.Drawing.Size(23, 20);
			this.butParentRemove.TabIndex = 306;
			this.butParentRemove.Click += new System.EventHandler(this.butParentRemove_Click);
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label7.Location = new System.Drawing.Point(6, 347);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(65, 20);
			this.label7.TabIndex = 264;
			this.label7.Text = "Hrs. Est.";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textEstHours
			// 
			this.textEstHours.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textEstHours.Location = new System.Drawing.Point(73, 348);
			this.textEstHours.MaxVal = 1000000D;
			this.textEstHours.MinVal = 0D;
			this.textEstHours.Name = "textEstHours";
			this.textEstHours.Size = new System.Drawing.Size(46, 20);
			this.textEstHours.TabIndex = 269;
			this.textEstHours.TextChanged += new System.EventHandler(this.textEstHours_TextChanged);
			// 
			// textEditorDocumentation
			// 
			this.textEditorDocumentation.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textEditorDocumentation.HasEditorOptions = true;
			this.textEditorDocumentation.HasSaveButton = true;
			this.textEditorDocumentation.Location = new System.Drawing.Point(3, 3);
			this.textEditorDocumentation.MainRtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang1033{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft S" +
    "ans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs17\\par\r\n}\r\n";
			this.textEditorDocumentation.MainText = "";
			this.textEditorDocumentation.MinimumSize = new System.Drawing.Size(450, 120);
			this.textEditorDocumentation.Name = "textEditorDocumentation";
			this.textEditorDocumentation.ReadOnly = false;
			this.textEditorDocumentation.Size = new System.Drawing.Size(733, 249);
			this.textEditorDocumentation.TabIndex = 261;
			this.textEditorDocumentation.SaveClick += new OpenDental.ODtextEditorSaveEventHandler(this.textEditor_SaveClick);
			this.textEditorDocumentation.OnTextEdited += new OpenDental.OdtextEditor.textChangedEventHandler(this.textEditor_OnTextEdited);
			// 
			// UserControlJobEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "UserControlJobEdit";
			this.Size = new System.Drawing.Size(1013, 726);
			this.groupLinks.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.tabControlMain.ResumeLayout(false);
			this.tabMain.ResumeLayout(false);
			this.tabReviews.ResumeLayout(false);
			this.tabDocumentation.ResumeLayout(false);
			this.tabHistory.ResumeLayout(false);
			this.tabHistory.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.tabTesting.ResumeLayout(false);
			this.splitContainerNoFlicker1.Panel1.ResumeLayout(false);
			this.splitContainerNoFlicker1.Panel2.ResumeLayout(false);
			this.splitContainerNoFlicker1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainerNoFlicker1)).EndInit();
			this.splitContainerNoFlicker1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.GroupBox groupLinks;
		private UI.ODGrid gridBugs;
		private UI.ODGrid gridFeatureReq;
		private UI.ODGrid gridTasks;
		private UI.ODGrid gridQuotes;
		private UI.ODGrid gridSubscribers;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox textVersion;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textDateEntry;
		private System.Windows.Forms.ComboBox comboStatus;
		private System.Windows.Forms.ComboBox comboCategory;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ComboBox comboPriority;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.TextBox textJobNum;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textApprove;
		private System.Windows.Forms.Timer timerTitle;
		private System.Windows.Forms.Timer timerVersion;
		private UI.ODGrid gridFiles;
		private UI.Button butVersionPrompt;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panel3;
		private UI.ODGrid gridAppointments;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private UI.ODGrid gridCustomers;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private SplitContainerNoFlicker splitContainerNoFlicker1;
		private UI.ODGrid gridRoles;
		private System.Windows.Forms.Label labelRelatedJobs;
		private System.Windows.Forms.TreeView treeRelatedJobs;
		private UI.Button butActions;
		private ODjobTextEditor textJobEditor;
		private UI.Button butParentPick;
		private System.Windows.Forms.TextBox textParent;
		private System.Windows.Forms.Label label1;
		private ValidDouble textActualHours;
		private System.Windows.Forms.Label label8;
		private UI.Button butParentRemove;
		private System.Windows.Forms.Label label7;
		private ValidDouble textEstHours;
		private System.Windows.Forms.TabControl tabControlMain;
		private System.Windows.Forms.TabPage tabMain;
		private UI.ODGrid gridNotes;
		private System.Windows.Forms.TabPage tabReviews;
		private UI.ODGrid gridReview;
		private System.Windows.Forms.TabPage tabDocumentation;
		private OdtextEditor textEditorDocumentation;
		private System.Windows.Forms.TabPage tabHistory;
		private System.Windows.Forms.Panel panel2;
		private UI.ODGrid gridLog;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.CheckBox checkShowHistoryText;
		private ODtextBox textTitle;
		private UI.Button butAddTime;
		private System.Windows.Forms.Timer timerEstimate;
		private System.Windows.Forms.TabPage tabTesting;
		private UI.ODGrid gridTestingNotes;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox comboPriorityTesting;
	}
}
