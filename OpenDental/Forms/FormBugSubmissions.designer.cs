namespace OpenDental{
	partial class FormBugSubmissions {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBugSubmissions));
			this.butAddBug = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.gridSubs = new OpenDental.UI.ODGrid();
			this.textStack = new OpenDental.ODtextBox();
			this.comboVersions = new OpenDental.UI.ComboBoxMulti();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textMsgText = new OpenDental.ODtextBox();
			this.textStackFilter = new OpenDental.ODtextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.comboGrouping = new System.Windows.Forms.ComboBox();
			this.label13 = new System.Windows.Forms.Label();
			this.textPatNums = new OpenDental.ODtextBox();
			this.checkExcludeHQ = new System.Windows.Forms.CheckBox();
			this.comboRegKeys = new OpenDental.UI.ComboBoxMulti();
			this.butRefresh = new OpenDental.UI.Button();
			this.checkShowAttached = new System.Windows.Forms.CheckBox();
			this.dateRangePicker = new OpenDental.UI.ODDateRangePicker();
			this.label7 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.butBugTask = new OpenDental.UI.Button();
			this.labelLastCall = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.labelSubNum = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.butGoToAccount = new OpenDental.UI.Button();
			this.labelCustomerNum = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.labelCustomerPhone = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.labelCustomerState = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.labelCustomerName = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.gridCustomerSubs = new OpenDental.UI.ODGrid();
			this.gridOfficeInfo = new OpenDental.UI.ODGrid();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// butAddBug
			// 
			this.butAddBug.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddBug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAddBug.Autosize = true;
			this.butAddBug.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddBug.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddBug.CornerRadius = 4F;
			this.butAddBug.Location = new System.Drawing.Point(1044, 700);
			this.butAddBug.Name = "butAddBug";
			this.butAddBug.Size = new System.Drawing.Size(85, 24);
			this.butAddBug.TabIndex = 3;
			this.butAddBug.Text = "&Add Job";
			this.butAddBug.Click += new System.EventHandler(this.butAdd_Click);
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
			this.butCancel.Location = new System.Drawing.Point(1135, 700);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(85, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// gridSubs
			// 
			this.gridSubs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridSubs.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridSubs.HasAddButton = false;
			this.gridSubs.HasDropDowns = false;
			this.gridSubs.HasMultilineHeaders = false;
			this.gridSubs.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridSubs.HeaderHeight = 15;
			this.gridSubs.HScrollVisible = false;
			this.gridSubs.Location = new System.Drawing.Point(12, 92);
			this.gridSubs.Name = "gridSubs";
			this.gridSubs.ScrollValue = 0;
			this.gridSubs.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridSubs.Size = new System.Drawing.Size(532, 632);
			this.gridSubs.TabIndex = 4;
			this.gridSubs.Title = "Submissions";
			this.gridSubs.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridSubs.TitleHeight = 18;
			this.gridSubs.TranslationName = "TableSubmissions";
			this.gridSubs.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridSubs_CellDoubleClick);
			this.gridSubs.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridSubs_CellClick);
			// 
			// textStack
			// 
			this.textStack.AcceptsTab = true;
			this.textStack.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textStack.BackColor = System.Drawing.SystemColors.Window;
			this.textStack.DetectLinksEnabled = false;
			this.textStack.DetectUrls = false;
			this.textStack.Location = new System.Drawing.Point(548, 109);
			this.textStack.Name = "textStack";
			this.textStack.QuickPasteType = OpenDentBusiness.QuickPasteType.JobManager;
			this.textStack.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textStack.Size = new System.Drawing.Size(411, 461);
			this.textStack.SpellCheckIsEnabled = false;
			this.textStack.TabIndex = 5;
			this.textStack.Text = "";
			// 
			// comboVersions
			// 
			this.comboVersions.ArraySelectedIndices = new int[0];
			this.comboVersions.BackColor = System.Drawing.SystemColors.Window;
			this.comboVersions.Items = ((System.Collections.ArrayList)(resources.GetObject("comboVersions.Items")));
			this.comboVersions.Location = new System.Drawing.Point(666, 13);
			this.comboVersions.Name = "comboVersions";
			this.comboVersions.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboVersions.SelectedIndices")));
			this.comboVersions.Size = new System.Drawing.Size(177, 21);
			this.comboVersions.TabIndex = 6;
			this.comboVersions.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.comboVersions_SelectionChangeCommitted);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(566, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(94, 14);
			this.label1.TabIndex = 7;
			this.label1.Text = "Reported Version";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(-16, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(93, 14);
			this.label2.TabIndex = 8;
			this.label2.Text = "Message Text";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(563, 51);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(97, 14);
			this.label3.TabIndex = 9;
			this.label3.Text = "Stack Trace (CSV)";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textMsgText
			// 
			this.textMsgText.AcceptsTab = true;
			this.textMsgText.BackColor = System.Drawing.SystemColors.Window;
			this.textMsgText.DetectLinksEnabled = false;
			this.textMsgText.DetectUrls = false;
			this.textMsgText.Location = new System.Drawing.Point(83, 13);
			this.textMsgText.Name = "textMsgText";
			this.textMsgText.QuickPasteType = OpenDentBusiness.QuickPasteType.JobManager;
			this.textMsgText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textMsgText.Size = new System.Drawing.Size(187, 21);
			this.textMsgText.TabIndex = 10;
			this.textMsgText.Text = "";
			this.textMsgText.TextChanged += new System.EventHandler(this.Filters_TextChanged);
			// 
			// textStackFilter
			// 
			this.textStackFilter.AcceptsTab = true;
			this.textStackFilter.BackColor = System.Drawing.SystemColors.Window;
			this.textStackFilter.DetectLinksEnabled = false;
			this.textStackFilter.DetectUrls = false;
			this.textStackFilter.Location = new System.Drawing.Point(666, 48);
			this.textStackFilter.Name = "textStackFilter";
			this.textStackFilter.QuickPasteType = OpenDentBusiness.QuickPasteType.JobManager;
			this.textStackFilter.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textStackFilter.Size = new System.Drawing.Size(177, 21);
			this.textStackFilter.TabIndex = 11;
			this.textStackFilter.Text = "";
			this.textStackFilter.TextChanged += new System.EventHandler(this.Filters_TextChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(276, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(97, 14);
			this.label4.TabIndex = 12;
			this.label4.Text = "Registration Key";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.comboGrouping);
			this.groupBox1.Controls.Add(this.label13);
			this.groupBox1.Controls.Add(this.textPatNums);
			this.groupBox1.Controls.Add(this.checkExcludeHQ);
			this.groupBox1.Controls.Add(this.comboRegKeys);
			this.groupBox1.Controls.Add(this.butRefresh);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.checkShowAttached);
			this.groupBox1.Controls.Add(this.dateRangePicker);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.comboVersions);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.textStackFilter);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.textMsgText);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Location = new System.Drawing.Point(12, 9);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(1208, 77);
			this.groupBox1.TabIndex = 14;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Filters";
			// 
			// comboGrouping
			// 
			this.comboGrouping.FormattingEnabled = true;
			this.comboGrouping.Location = new System.Drawing.Point(903, 48);
			this.comboGrouping.Name = "comboGrouping";
			this.comboGrouping.Size = new System.Drawing.Size(102, 21);
			this.comboGrouping.TabIndex = 32;
			this.comboGrouping.SelectionChangeCommitted += new System.EventHandler(this.comboVersions_SelectionChangeCommitted);
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(849, 53);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(54, 13);
			this.label13.TabIndex = 31;
			this.label13.Text = "Group By:";
			// 
			// textPatNums
			// 
			this.textPatNums.AcceptsTab = true;
			this.textPatNums.BackColor = System.Drawing.SystemColors.Window;
			this.textPatNums.DetectLinksEnabled = false;
			this.textPatNums.DetectUrls = false;
			this.textPatNums.Location = new System.Drawing.Point(903, 12);
			this.textPatNums.Name = "textPatNums";
			this.textPatNums.QuickPasteType = OpenDentBusiness.QuickPasteType.JobManager;
			this.textPatNums.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textPatNums.Size = new System.Drawing.Size(102, 22);
			this.textPatNums.TabIndex = 29;
			this.textPatNums.Text = "";
			this.textPatNums.TextChanged += new System.EventHandler(this.Filters_TextChanged);
			// 
			// checkExcludeHQ
			// 
			this.checkExcludeHQ.Location = new System.Drawing.Point(1016, 13);
			this.checkExcludeHQ.Name = "checkExcludeHQ";
			this.checkExcludeHQ.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkExcludeHQ.Size = new System.Drawing.Size(87, 24);
			this.checkExcludeHQ.TabIndex = 28;
			this.checkExcludeHQ.Text = "Exclude HQ";
			this.checkExcludeHQ.UseVisualStyleBackColor = true;
			this.checkExcludeHQ.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
			// 
			// comboRegKeys
			// 
			this.comboRegKeys.ArraySelectedIndices = new int[0];
			this.comboRegKeys.BackColor = System.Drawing.SystemColors.Window;
			this.comboRegKeys.Items = ((System.Collections.ArrayList)(resources.GetObject("comboRegKeys.Items")));
			this.comboRegKeys.Location = new System.Drawing.Point(379, 13);
			this.comboRegKeys.Name = "comboRegKeys";
			this.comboRegKeys.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboRegKeys.SelectedIndices")));
			this.comboRegKeys.Size = new System.Drawing.Size(187, 21);
			this.comboRegKeys.TabIndex = 27;
			this.comboRegKeys.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.comboVersions_SelectionChangeCommitted);
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(1123, 41);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(79, 24);
			this.butRefresh.TabIndex = 26;
			this.butRefresh.Text = "&Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// checkShowAttached
			// 
			this.checkShowAttached.Location = new System.Drawing.Point(1001, 48);
			this.checkShowAttached.Name = "checkShowAttached";
			this.checkShowAttached.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkShowAttached.Size = new System.Drawing.Size(102, 24);
			this.checkShowAttached.TabIndex = 17;
			this.checkShowAttached.Text = "Show Attached";
			this.checkShowAttached.UseVisualStyleBackColor = true;
			this.checkShowAttached.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
			// 
			// dateRangePicker
			// 
			this.dateRangePicker.BackColor = System.Drawing.SystemColors.Control;
			this.dateRangePicker.DefaultDateTimeFrom = new System.DateTime(2017, 1, 1, 0, 0, 0, 0);
			this.dateRangePicker.DefaultDateTimeTo = new System.DateTime(2017, 11, 1, 0, 0, 0, 0);
			this.dateRangePicker.EnableWeekButtons = false;
			this.dateRangePicker.Location = new System.Drawing.Point(12, 43);
			this.dateRangePicker.MaximumSize = new System.Drawing.Size(0, 185);
			this.dateRangePicker.MinimumSize = new System.Drawing.Size(453, 22);
			this.dateRangePicker.Name = "dateRangePicker";
			this.dateRangePicker.Size = new System.Drawing.Size(513, 22);
			this.dateRangePicker.TabIndex = 14;
			this.dateRangePicker.CalendarClosed += new OpenDental.UI.CalendarClosedHandler(this.dateRangePicker_CalendarClosed);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(849, 17);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(56, 13);
			this.label7.TabIndex = 30;
			this.label7.Text = "PatNum(s)";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(547, 92);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(76, 14);
			this.label5.TabIndex = 14;
			this.label5.Text = "Stack Trace";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox2.Controls.Add(this.butBugTask);
			this.groupBox2.Controls.Add(this.labelLastCall);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Controls.Add(this.labelSubNum);
			this.groupBox2.Controls.Add(this.label10);
			this.groupBox2.Controls.Add(this.butGoToAccount);
			this.groupBox2.Controls.Add(this.labelCustomerNum);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.labelCustomerPhone);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.labelCustomerState);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.labelCustomerName);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Location = new System.Drawing.Point(548, 576);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(411, 148);
			this.groupBox2.TabIndex = 17;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Submitter Info.";
			// 
			// butBugTask
			// 
			this.butBugTask.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBugTask.Autosize = true;
			this.butBugTask.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBugTask.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBugTask.CornerRadius = 4F;
			this.butBugTask.Location = new System.Drawing.Point(115, 122);
			this.butBugTask.Name = "butBugTask";
			this.butBugTask.Size = new System.Drawing.Size(104, 24);
			this.butBugTask.TabIndex = 30;
			this.butBugTask.Text = "&Create Task";
			this.butBugTask.Click += new System.EventHandler(this.butBugTask_Click);
			// 
			// labelLastCall
			// 
			this.labelLastCall.Location = new System.Drawing.Point(78, 106);
			this.labelLastCall.Name = "labelLastCall";
			this.labelLastCall.Size = new System.Drawing.Size(215, 13);
			this.labelLastCall.TabIndex = 29;
			this.labelLastCall.Text = "XXXXX";
			this.labelLastCall.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label12
			// 
			this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.Location = new System.Drawing.Point(7, 106);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(65, 13);
			this.label12.TabIndex = 28;
			this.label12.Text = "Last Call:";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelSubNum
			// 
			this.labelSubNum.Location = new System.Drawing.Point(78, 88);
			this.labelSubNum.Name = "labelSubNum";
			this.labelSubNum.Size = new System.Drawing.Size(215, 13);
			this.labelSubNum.TabIndex = 27;
			this.labelSubNum.Text = "XXXXX";
			this.labelSubNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.Location = new System.Drawing.Point(7, 88);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(65, 13);
			this.label10.TabIndex = 26;
			this.label10.Text = "Sub Num:";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butGoToAccount
			// 
			this.butGoToAccount.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGoToAccount.Autosize = true;
			this.butGoToAccount.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGoToAccount.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGoToAccount.CornerRadius = 4F;
			this.butGoToAccount.Location = new System.Drawing.Point(6, 122);
			this.butGoToAccount.Name = "butGoToAccount";
			this.butGoToAccount.Size = new System.Drawing.Size(103, 24);
			this.butGoToAccount.TabIndex = 21;
			this.butGoToAccount.Text = "&Go To Account";
			this.butGoToAccount.Click += new System.EventHandler(this.butGoToAccount_Click);
			// 
			// labelCustomerNum
			// 
			this.labelCustomerNum.Location = new System.Drawing.Point(78, 16);
			this.labelCustomerNum.Name = "labelCustomerNum";
			this.labelCustomerNum.Size = new System.Drawing.Size(215, 13);
			this.labelCustomerNum.TabIndex = 25;
			this.labelCustomerNum.Text = "XXXXX";
			this.labelCustomerNum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label11
			// 
			this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.Location = new System.Drawing.Point(7, 16);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(65, 13);
			this.label11.TabIndex = 24;
			this.label11.Text = "Pat Num:";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelCustomerPhone
			// 
			this.labelCustomerPhone.Location = new System.Drawing.Point(78, 70);
			this.labelCustomerPhone.Name = "labelCustomerPhone";
			this.labelCustomerPhone.Size = new System.Drawing.Size(215, 13);
			this.labelCustomerPhone.TabIndex = 23;
			this.labelCustomerPhone.Text = "(555)555-5555";
			this.labelCustomerPhone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(7, 70);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(65, 13);
			this.label9.TabIndex = 22;
			this.label9.Text = "Work Phone:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelCustomerState
			// 
			this.labelCustomerState.Location = new System.Drawing.Point(78, 52);
			this.labelCustomerState.Name = "labelCustomerState";
			this.labelCustomerState.Size = new System.Drawing.Size(215, 13);
			this.labelCustomerState.TabIndex = 21;
			this.labelCustomerState.Text = "OR";
			this.labelCustomerState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(7, 52);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(65, 13);
			this.label8.TabIndex = 20;
			this.label8.Text = "State:";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelCustomerName
			// 
			this.labelCustomerName.Location = new System.Drawing.Point(78, 34);
			this.labelCustomerName.Name = "labelCustomerName";
			this.labelCustomerName.Size = new System.Drawing.Size(215, 13);
			this.labelCustomerName.TabIndex = 19;
			this.labelCustomerName.Text = "John Doe";
			this.labelCustomerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(7, 34);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(65, 13);
			this.label6.TabIndex = 18;
			this.label6.Text = "Name:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// gridCustomerSubs
			// 
			this.gridCustomerSubs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.gridCustomerSubs.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridCustomerSubs.HasAddButton = false;
			this.gridCustomerSubs.HasDropDowns = false;
			this.gridCustomerSubs.HasLinkDetect = false;
			this.gridCustomerSubs.HasMultilineHeaders = false;
			this.gridCustomerSubs.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridCustomerSubs.HeaderHeight = 0;
			this.gridCustomerSubs.HScrollVisible = false;
			this.gridCustomerSubs.Location = new System.Drawing.Point(965, 576);
			this.gridCustomerSubs.Name = "gridCustomerSubs";
			this.gridCustomerSubs.ScrollValue = 0;
			this.gridCustomerSubs.Size = new System.Drawing.Size(255, 118);
			this.gridCustomerSubs.TabIndex = 27;
			this.gridCustomerSubs.Title = "Customer Submissions";
			this.gridCustomerSubs.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridCustomerSubs.TitleHeight = 18;
			this.gridCustomerSubs.TranslationName = "TableSubmissions";
			this.gridCustomerSubs.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridCustomerSubs_CellDoubleClick);
			this.gridCustomerSubs.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridCustomerSubs_CellClick);
			// 
			// gridOfficeInfo
			// 
			this.gridOfficeInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridOfficeInfo.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridOfficeInfo.HasAddButton = false;
			this.gridOfficeInfo.HasDropDowns = false;
			this.gridOfficeInfo.HasMultilineHeaders = false;
			this.gridOfficeInfo.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridOfficeInfo.HeaderHeight = 15;
			this.gridOfficeInfo.HScrollVisible = false;
			this.gridOfficeInfo.Location = new System.Drawing.Point(965, 109);
			this.gridOfficeInfo.Name = "gridOfficeInfo";
			this.gridOfficeInfo.ScrollValue = 0;
			this.gridOfficeInfo.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridOfficeInfo.Size = new System.Drawing.Size(255, 461);
			this.gridOfficeInfo.TabIndex = 20;
			this.gridOfficeInfo.Title = "Office Info";
			this.gridOfficeInfo.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridOfficeInfo.TitleHeight = 18;
			this.gridOfficeInfo.TranslationName = "TableOfficeInfo";
			// 
			// FormBugSubmissions
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(1232, 736);
			this.Controls.Add(this.gridCustomerSubs);
			this.Controls.Add(this.gridOfficeInfo);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.textStack);
			this.Controls.Add(this.gridSubs);
			this.Controls.Add(this.butAddBug);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(887, 622);
			this.Name = "FormBugSubmissions";
			this.Text = "Bug Submissions";
			this.Load += new System.EventHandler(this.FormBugSubmissions_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butAddBug;
		private OpenDental.UI.Button butCancel;
		private UI.ODGrid gridSubs;
		private ODtextBox textStack;
		private UI.ComboBoxMulti comboVersions;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private ODtextBox textMsgText;
		private ODtextBox textStackFilter;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label5;
		private UI.ODDateRangePicker dateRangePicker;
		private System.Windows.Forms.CheckBox checkShowAttached;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label labelCustomerName;
		private System.Windows.Forms.Label labelCustomerPhone;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label labelCustomerState;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label labelCustomerNum;
		private System.Windows.Forms.Label label11;
		private UI.ODGrid gridOfficeInfo;
		private UI.Button butGoToAccount;
		private UI.Button butRefresh;
		private UI.ODGrid gridCustomerSubs;
		private System.Windows.Forms.Label labelSubNum;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label labelLastCall;
		private System.Windows.Forms.Label label12;
		private UI.ComboBoxMulti comboRegKeys;
		private System.Windows.Forms.CheckBox checkExcludeHQ;
		private System.Windows.Forms.Label label7;
		private UI.Button butBugTask;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.ComboBox comboGrouping;
		private ODtextBox textPatNums;
	}
}