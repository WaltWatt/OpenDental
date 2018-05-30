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
			this.butAddJob = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.gridSubs = new OpenDental.UI.ODGrid();
			this.comboVersions = new OpenDental.UI.ComboBoxMulti();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textMsgText = new OpenDental.ODtextBox();
			this.textStackFilter = new OpenDental.ODtextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.groupFilters = new System.Windows.Forms.GroupBox();
			this.textDevNoteFilter = new OpenDental.ODtextBox();
			this.label15 = new System.Windows.Forms.Label();
			this.textPatNums = new OpenDental.ODtextBox();
			this.checkExcludeHQ = new System.Windows.Forms.CheckBox();
			this.comboRegKeys = new OpenDental.UI.ComboBoxMulti();
			this.butRefresh = new OpenDental.UI.Button();
			this.checkShowAttached = new System.Windows.Forms.CheckBox();
			this.dateRangePicker = new OpenDental.UI.ODDateRangePicker();
			this.label7 = new System.Windows.Forms.Label();
			this.menuOptions = new System.Windows.Forms.MenuStrip();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.findPreviouslyFixedSubmisisonsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.comboSortBy = new System.Windows.Forms.ComboBox();
			this.label14 = new System.Windows.Forms.Label();
			this.comboGrouping = new System.Windows.Forms.ComboBox();
			this.label13 = new System.Windows.Forms.Label();
			this.bugSubmissionControl = new OpenDental.UI.BugSubmissionControl();
			this.groupFilters.SuspendLayout();
			this.menuOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// butAddJob
			// 
			this.butAddJob.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAddJob.Autosize = true;
			this.butAddJob.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddJob.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddJob.CornerRadius = 4F;
			this.butAddJob.Location = new System.Drawing.Point(1043, 723);
			this.butAddJob.Name = "butAddJob";
			this.butAddJob.Size = new System.Drawing.Size(85, 24);
			this.butAddJob.TabIndex = 3;
			this.butAddJob.Text = "&Add Job";
			this.butAddJob.Click += new System.EventHandler(this.butAdd_Click);
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
			this.butCancel.Location = new System.Drawing.Point(1134, 723);
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
			this.gridSubs.Location = new System.Drawing.Point(12, 127);
			this.gridSubs.Name = "gridSubs";
			this.gridSubs.ScrollValue = 0;
			this.gridSubs.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridSubs.Size = new System.Drawing.Size(532, 619);
			this.gridSubs.TabIndex = 4;
			this.gridSubs.Title = "Submissions";
			this.gridSubs.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridSubs.TitleHeight = 18;
			this.gridSubs.TranslationName = "TableSubmissions";
			this.gridSubs.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridSubs_CellDoubleClick);
			this.gridSubs.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridSubs_CellClick);
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
			this.label2.Location = new System.Drawing.Point(0, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(77, 14);
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
			this.textMsgText.Multiline = false;
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
			this.textStackFilter.Multiline = false;
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
			// groupFilters
			// 
			this.groupFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupFilters.Controls.Add(this.textDevNoteFilter);
			this.groupFilters.Controls.Add(this.label15);
			this.groupFilters.Controls.Add(this.textPatNums);
			this.groupFilters.Controls.Add(this.checkExcludeHQ);
			this.groupFilters.Controls.Add(this.comboRegKeys);
			this.groupFilters.Controls.Add(this.butRefresh);
			this.groupFilters.Controls.Add(this.label3);
			this.groupFilters.Controls.Add(this.checkShowAttached);
			this.groupFilters.Controls.Add(this.dateRangePicker);
			this.groupFilters.Controls.Add(this.label4);
			this.groupFilters.Controls.Add(this.comboVersions);
			this.groupFilters.Controls.Add(this.label1);
			this.groupFilters.Controls.Add(this.textStackFilter);
			this.groupFilters.Controls.Add(this.label2);
			this.groupFilters.Controls.Add(this.textMsgText);
			this.groupFilters.Controls.Add(this.label7);
			this.groupFilters.Location = new System.Drawing.Point(12, 27);
			this.groupFilters.Name = "groupFilters";
			this.groupFilters.Size = new System.Drawing.Size(1208, 77);
			this.groupFilters.TabIndex = 14;
			this.groupFilters.TabStop = false;
			this.groupFilters.Text = "Filters";
			// 
			// textDevNoteFilter
			// 
			this.textDevNoteFilter.AcceptsTab = true;
			this.textDevNoteFilter.BackColor = System.Drawing.SystemColors.Window;
			this.textDevNoteFilter.DetectLinksEnabled = false;
			this.textDevNoteFilter.DetectUrls = false;
			this.textDevNoteFilter.Location = new System.Drawing.Point(903, 47);
			this.textDevNoteFilter.Multiline = false;
			this.textDevNoteFilter.Name = "textDevNoteFilter";
			this.textDevNoteFilter.QuickPasteType = OpenDentBusiness.QuickPasteType.JobManager;
			this.textDevNoteFilter.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textDevNoteFilter.Size = new System.Drawing.Size(102, 22);
			this.textDevNoteFilter.TabIndex = 31;
			this.textDevNoteFilter.Text = "";
			this.textDevNoteFilter.TextChanged += new System.EventHandler(this.Filters_TextChanged);
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(849, 52);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(50, 13);
			this.label15.TabIndex = 32;
			this.label15.Text = "DevNote";
			// 
			// textPatNums
			// 
			this.textPatNums.AcceptsTab = true;
			this.textPatNums.BackColor = System.Drawing.SystemColors.Window;
			this.textPatNums.DetectLinksEnabled = false;
			this.textPatNums.DetectUrls = false;
			this.textPatNums.Location = new System.Drawing.Point(903, 12);
			this.textPatNums.Multiline = false;
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
			this.checkExcludeHQ.Click += new System.EventHandler(this.checkBox_CheckedChanged);
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
			this.butRefresh.Location = new System.Drawing.Point(1123, 48);
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
			this.checkShowAttached.Click += new System.EventHandler(this.checkBox_CheckedChanged);
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
			// menuOptions
			// 
			this.menuOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem});
			this.menuOptions.Location = new System.Drawing.Point(0, 0);
			this.menuOptions.Name = "menuOptions";
			this.menuOptions.Size = new System.Drawing.Size(1232, 24);
			this.menuOptions.TabIndex = 28;
			this.menuOptions.Text = "menuStrip1";
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findPreviouslyFixedSubmisisonsToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
			this.toolsToolStripMenuItem.Text = "Tools";
			// 
			// findPreviouslyFixedSubmisisonsToolStripMenuItem
			// 
			this.findPreviouslyFixedSubmisisonsToolStripMenuItem.Name = "findPreviouslyFixedSubmisisonsToolStripMenuItem";
			this.findPreviouslyFixedSubmisisonsToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
			this.findPreviouslyFixedSubmisisonsToolStripMenuItem.Text = "Find Similar Submisisons";
			this.findPreviouslyFixedSubmisisonsToolStripMenuItem.Click += new System.EventHandler(this.findPreviouslyFixedSubmisisonsToolStripMenuItem_Click);
			// 
			// comboSortBy
			// 
			this.comboSortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSortBy.FormattingEnabled = true;
			this.comboSortBy.Location = new System.Drawing.Point(219, 104);
			this.comboSortBy.Name = "comboSortBy";
			this.comboSortBy.Size = new System.Drawing.Size(102, 21);
			this.comboSortBy.TabIndex = 38;
			this.comboSortBy.SelectionChangeCommitted += new System.EventHandler(this.comboVersions_SelectionChangeCommitted);
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(175, 109);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(44, 13);
			this.label14.TabIndex = 37;
			this.label14.Text = "Sort By:";
			// 
			// comboGrouping
			// 
			this.comboGrouping.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboGrouping.FormattingEnabled = true;
			this.comboGrouping.Location = new System.Drawing.Point(65, 104);
			this.comboGrouping.Name = "comboGrouping";
			this.comboGrouping.Size = new System.Drawing.Size(102, 21);
			this.comboGrouping.TabIndex = 36;
			this.comboGrouping.SelectionChangeCommitted += new System.EventHandler(this.comboVersions_SelectionChangeCommitted);
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(12, 109);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(54, 13);
			this.label13.TabIndex = 35;
			this.label13.Text = "Group By:";
			// 
			// bugSubmissionControl
			// 
			this.bugSubmissionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.bugSubmissionControl.ControlMode = BugSubmissionControlMode.General;
			this.bugSubmissionControl.Location = new System.Drawing.Point(550, 124);
			this.bugSubmissionControl.MinimumSize = new System.Drawing.Size(594, 521);
			this.bugSubmissionControl.Name = "bugSubmissionControl";
			this.bugSubmissionControl.Size = new System.Drawing.Size(673, 624);
			this.bugSubmissionControl.TabIndex = 40;
			// 
			// FormBugSubmissions
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(1232, 760);
			this.Controls.Add(this.comboSortBy);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.comboGrouping);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.menuOptions);
			this.Controls.Add(this.groupFilters);
			this.Controls.Add(this.gridSubs);
			this.Controls.Add(this.butAddJob);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.bugSubmissionControl);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuOptions;
			this.MinimumSize = new System.Drawing.Size(887, 622);
			this.Name = "FormBugSubmissions";
			this.Text = "Bug Submissions";
			this.Load += new System.EventHandler(this.FormBugSubmissions_Load);
			this.groupFilters.ResumeLayout(false);
			this.groupFilters.PerformLayout();
			this.menuOptions.ResumeLayout(false);
			this.menuOptions.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butAddJob;
		private OpenDental.UI.Button butCancel;
		private UI.ODGrid gridSubs;
		private UI.ComboBoxMulti comboVersions;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private ODtextBox textMsgText;
		private ODtextBox textStackFilter;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupFilters;
		private UI.ODDateRangePicker dateRangePicker;
		private System.Windows.Forms.CheckBox checkShowAttached;
		private UI.Button butRefresh;
		private UI.ComboBoxMulti comboRegKeys;
		private System.Windows.Forms.CheckBox checkExcludeHQ;
		private System.Windows.Forms.Label label7;
		private ODtextBox textPatNums;
		private System.Windows.Forms.MenuStrip menuOptions;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem findPreviouslyFixedSubmisisonsToolStripMenuItem;
		private System.Windows.Forms.ComboBox comboSortBy;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.ComboBox comboGrouping;
		private System.Windows.Forms.Label label13;
		private ODtextBox textDevNoteFilter;
		private System.Windows.Forms.Label label15;
		private UI.BugSubmissionControl bugSubmissionControl;
	}
}