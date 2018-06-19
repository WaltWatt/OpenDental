namespace OpenDental{
	partial class FormJobManagerOverview {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJobManagerOverview));
			this.tabControlMain = new System.Windows.Forms.TabControl();
			this.tabHighPriority = new System.Windows.Forms.TabPage();
			this.gridHighPriorityJobs = new OpenDental.UI.ODGrid();
			this.tabJobsToWatch = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.gridOldJobs = new OpenDental.UI.ODGrid();
			this.gridNoHours = new OpenDental.UI.ODGrid();
			this.gridOverEstimate = new OpenDental.UI.ODGrid();
			this.gridQuoteNotStarted = new OpenDental.UI.ODGrid();
			this.tabStatistics = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.userControlJobVersionStatisticsPrev3 = new OpenDental.UserControlJobVersionStatistics();
			this.userControlJobVersionStatisticsPrev2 = new OpenDental.UserControlJobVersionStatistics();
			this.userControlJobVersionStatisticsPrev1 = new OpenDental.UserControlJobVersionStatistics();
			this.gridStatisticsHead = new OpenDental.UI.ODGrid();
			this.gridStatisticsPrev3 = new OpenDental.UI.ODGrid();
			this.gridStatisticsPrev1 = new OpenDental.UI.ODGrid();
			this.gridStatisticsPrev2 = new OpenDental.UI.ODGrid();
			this.userControlJobVersionStatisticsHead = new OpenDental.UserControlJobVersionStatistics();
			this.tabControlMain.SuspendLayout();
			this.tabHighPriority.SuspendLayout();
			this.tabJobsToWatch.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tabStatistics.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControlMain
			// 
			this.tabControlMain.Controls.Add(this.tabHighPriority);
			this.tabControlMain.Controls.Add(this.tabJobsToWatch);
			this.tabControlMain.Controls.Add(this.tabStatistics);
			this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlMain.Location = new System.Drawing.Point(0, 0);
			this.tabControlMain.Name = "tabControlMain";
			this.tabControlMain.SelectedIndex = 0;
			this.tabControlMain.Size = new System.Drawing.Size(1186, 474);
			this.tabControlMain.TabIndex = 2;
			// 
			// tabHighPriority
			// 
			this.tabHighPriority.Controls.Add(this.gridHighPriorityJobs);
			this.tabHighPriority.Location = new System.Drawing.Point(4, 22);
			this.tabHighPriority.Name = "tabHighPriority";
			this.tabHighPriority.Padding = new System.Windows.Forms.Padding(3);
			this.tabHighPriority.Size = new System.Drawing.Size(1178, 448);
			this.tabHighPriority.TabIndex = 2;
			this.tabHighPriority.Text = "High Priority Jobs";
			this.tabHighPriority.UseVisualStyleBackColor = true;
			// 
			// gridHighPriorityJobs
			// 
			this.gridHighPriorityJobs.AllowSortingByColumn = true;
			this.gridHighPriorityJobs.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridHighPriorityJobs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridHighPriorityJobs.HasAddButton = false;
			this.gridHighPriorityJobs.HasDropDowns = false;
			this.gridHighPriorityJobs.HasMultilineHeaders = false;
			this.gridHighPriorityJobs.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridHighPriorityJobs.HeaderHeight = 15;
			this.gridHighPriorityJobs.HScrollVisible = false;
			this.gridHighPriorityJobs.Location = new System.Drawing.Point(3, 3);
			this.gridHighPriorityJobs.Name = "gridHighPriorityJobs";
			this.gridHighPriorityJobs.ScrollValue = 0;
			this.gridHighPriorityJobs.Size = new System.Drawing.Size(1172, 442);
			this.gridHighPriorityJobs.TabIndex = 3;
			this.gridHighPriorityJobs.Title = "High/Urgent Jobs";
			this.gridHighPriorityJobs.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridHighPriorityJobs.TitleHeight = 18;
			this.gridHighPriorityJobs.TranslationName = "Jobs";
			this.gridHighPriorityJobs.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridHighPriorityJobs_CellDoubleClick);
			// 
			// tabJobsToWatch
			// 
			this.tabJobsToWatch.Controls.Add(this.tableLayoutPanel2);
			this.tabJobsToWatch.Location = new System.Drawing.Point(4, 22);
			this.tabJobsToWatch.Name = "tabJobsToWatch";
			this.tabJobsToWatch.Padding = new System.Windows.Forms.Padding(3);
			this.tabJobsToWatch.Size = new System.Drawing.Size(1178, 448);
			this.tabJobsToWatch.TabIndex = 4;
			this.tabJobsToWatch.Text = "Jobs To Watch";
			this.tabJobsToWatch.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 4;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel2.Controls.Add(this.gridOldJobs, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.gridNoHours, 3, 0);
			this.tableLayoutPanel2.Controls.Add(this.gridOverEstimate, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.gridQuoteNotStarted, 2, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(1172, 442);
			this.tableLayoutPanel2.TabIndex = 8;
			// 
			// gridOldJobs
			// 
			this.gridOldJobs.AllowSortingByColumn = true;
			this.gridOldJobs.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridOldJobs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridOldJobs.HasAddButton = false;
			this.gridOldJobs.HasDropDowns = false;
			this.gridOldJobs.HasMultilineHeaders = false;
			this.gridOldJobs.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridOldJobs.HeaderHeight = 15;
			this.gridOldJobs.HScrollVisible = false;
			this.gridOldJobs.Location = new System.Drawing.Point(3, 3);
			this.gridOldJobs.Name = "gridOldJobs";
			this.gridOldJobs.ScrollValue = 0;
			this.gridOldJobs.Size = new System.Drawing.Size(287, 436);
			this.gridOldJobs.TabIndex = 4;
			this.gridOldJobs.Title = "Over 1 Year Old";
			this.gridOldJobs.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridOldJobs.TitleHeight = 18;
			this.gridOldJobs.TranslationName = "Jobs";
			this.gridOldJobs.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridOldJobs_CellDoubleClick);
			// 
			// gridNoHours
			// 
			this.gridNoHours.AllowSortingByColumn = true;
			this.gridNoHours.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridNoHours.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridNoHours.HasAddButton = false;
			this.gridNoHours.HasDropDowns = false;
			this.gridNoHours.HasMultilineHeaders = false;
			this.gridNoHours.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridNoHours.HeaderHeight = 15;
			this.gridNoHours.HScrollVisible = false;
			this.gridNoHours.Location = new System.Drawing.Point(882, 3);
			this.gridNoHours.Name = "gridNoHours";
			this.gridNoHours.ScrollValue = 0;
			this.gridNoHours.Size = new System.Drawing.Size(287, 436);
			this.gridNoHours.TabIndex = 7;
			this.gridNoHours.Title = "In Development w/ No Hours Worked";
			this.gridNoHours.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridNoHours.TitleHeight = 18;
			this.gridNoHours.TranslationName = "Jobs";
			this.gridNoHours.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridNoHours_CellDoubleClick);
			// 
			// gridOverEstimate
			// 
			this.gridOverEstimate.AllowSortingByColumn = true;
			this.gridOverEstimate.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridOverEstimate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridOverEstimate.HasAddButton = false;
			this.gridOverEstimate.HasDropDowns = false;
			this.gridOverEstimate.HasMultilineHeaders = false;
			this.gridOverEstimate.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridOverEstimate.HeaderHeight = 15;
			this.gridOverEstimate.HScrollVisible = false;
			this.gridOverEstimate.Location = new System.Drawing.Point(296, 3);
			this.gridOverEstimate.Name = "gridOverEstimate";
			this.gridOverEstimate.ScrollValue = 0;
			this.gridOverEstimate.Size = new System.Drawing.Size(287, 436);
			this.gridOverEstimate.TabIndex = 5;
			this.gridOverEstimate.Title = "Over Estimated Time";
			this.gridOverEstimate.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridOverEstimate.TitleHeight = 18;
			this.gridOverEstimate.TranslationName = "Jobs";
			this.gridOverEstimate.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridOverEstimate_CellDoubleClick);
			// 
			// gridQuoteNotStarted
			// 
			this.gridQuoteNotStarted.AllowSortingByColumn = true;
			this.gridQuoteNotStarted.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridQuoteNotStarted.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridQuoteNotStarted.HasAddButton = false;
			this.gridQuoteNotStarted.HasDropDowns = false;
			this.gridQuoteNotStarted.HasMultilineHeaders = false;
			this.gridQuoteNotStarted.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridQuoteNotStarted.HeaderHeight = 15;
			this.gridQuoteNotStarted.HScrollVisible = false;
			this.gridQuoteNotStarted.Location = new System.Drawing.Point(589, 3);
			this.gridQuoteNotStarted.Name = "gridQuoteNotStarted";
			this.gridQuoteNotStarted.ScrollValue = 0;
			this.gridQuoteNotStarted.Size = new System.Drawing.Size(287, 436);
			this.gridQuoteNotStarted.TabIndex = 6;
			this.gridQuoteNotStarted.Title = "Quoted w/ No Hours Worked";
			this.gridQuoteNotStarted.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridQuoteNotStarted.TitleHeight = 18;
			this.gridQuoteNotStarted.TranslationName = "Jobs";
			this.gridQuoteNotStarted.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridQuoteOld_CellDoubleClick);
			// 
			// tabStatistics
			// 
			this.tabStatistics.Controls.Add(this.tableLayoutPanel1);
			this.tabStatistics.Location = new System.Drawing.Point(4, 22);
			this.tabStatistics.Name = "tabStatistics";
			this.tabStatistics.Padding = new System.Windows.Forms.Padding(3);
			this.tabStatistics.Size = new System.Drawing.Size(1178, 448);
			this.tabStatistics.TabIndex = 3;
			this.tabStatistics.Text = "Statistics";
			this.tabStatistics.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.Controls.Add(this.userControlJobVersionStatisticsPrev3, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.userControlJobVersionStatisticsPrev2, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.userControlJobVersionStatisticsPrev1, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.gridStatisticsHead, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.gridStatisticsPrev3, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.gridStatisticsPrev1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.gridStatisticsPrev2, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.userControlJobVersionStatisticsHead, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1172, 442);
			this.tableLayoutPanel1.TabIndex = 4;
			// 
			// userControlJobVersionStatisticsPrev3
			// 
			this.userControlJobVersionStatisticsPrev3.AutoSize = true;
			this.userControlJobVersionStatisticsPrev3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userControlJobVersionStatisticsPrev3.Location = new System.Drawing.Point(882, 224);
			this.userControlJobVersionStatisticsPrev3.Name = "userControlJobVersionStatisticsPrev3";
			this.userControlJobVersionStatisticsPrev3.Size = new System.Drawing.Size(287, 215);
			this.userControlJobVersionStatisticsPrev3.TabIndex = 11;
			// 
			// userControlJobVersionStatisticsPrev2
			// 
			this.userControlJobVersionStatisticsPrev2.AutoSize = true;
			this.userControlJobVersionStatisticsPrev2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userControlJobVersionStatisticsPrev2.Location = new System.Drawing.Point(589, 224);
			this.userControlJobVersionStatisticsPrev2.Name = "userControlJobVersionStatisticsPrev2";
			this.userControlJobVersionStatisticsPrev2.Size = new System.Drawing.Size(287, 215);
			this.userControlJobVersionStatisticsPrev2.TabIndex = 10;
			// 
			// userControlJobVersionStatisticsPrev1
			// 
			this.userControlJobVersionStatisticsPrev1.AutoSize = true;
			this.userControlJobVersionStatisticsPrev1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userControlJobVersionStatisticsPrev1.Location = new System.Drawing.Point(296, 224);
			this.userControlJobVersionStatisticsPrev1.Name = "userControlJobVersionStatisticsPrev1";
			this.userControlJobVersionStatisticsPrev1.Size = new System.Drawing.Size(287, 215);
			this.userControlJobVersionStatisticsPrev1.TabIndex = 9;
			// 
			// gridStatisticsHead
			// 
			this.gridStatisticsHead.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridStatisticsHead.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridStatisticsHead.HasAddButton = false;
			this.gridStatisticsHead.HasDropDowns = false;
			this.gridStatisticsHead.HasMultilineHeaders = false;
			this.gridStatisticsHead.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridStatisticsHead.HeaderHeight = 15;
			this.gridStatisticsHead.HScrollVisible = false;
			this.gridStatisticsHead.Location = new System.Drawing.Point(3, 3);
			this.gridStatisticsHead.Name = "gridStatisticsHead";
			this.gridStatisticsHead.ScrollValue = 0;
			this.gridStatisticsHead.Size = new System.Drawing.Size(287, 215);
			this.gridStatisticsHead.TabIndex = 0;
			this.gridStatisticsHead.Title = "HEAD Statistics";
			this.gridStatisticsHead.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridStatisticsHead.TitleHeight = 18;
			this.gridStatisticsHead.TranslationName = "Jobs";
			// 
			// gridStatisticsPrev3
			// 
			this.gridStatisticsPrev3.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridStatisticsPrev3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridStatisticsPrev3.HasAddButton = false;
			this.gridStatisticsPrev3.HasDropDowns = false;
			this.gridStatisticsPrev3.HasMultilineHeaders = false;
			this.gridStatisticsPrev3.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridStatisticsPrev3.HeaderHeight = 15;
			this.gridStatisticsPrev3.HScrollVisible = false;
			this.gridStatisticsPrev3.Location = new System.Drawing.Point(882, 3);
			this.gridStatisticsPrev3.Name = "gridStatisticsPrev3";
			this.gridStatisticsPrev3.ScrollValue = 0;
			this.gridStatisticsPrev3.Size = new System.Drawing.Size(287, 215);
			this.gridStatisticsPrev3.TabIndex = 3;
			this.gridStatisticsPrev3.Title = "17.3 Statistics";
			this.gridStatisticsPrev3.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridStatisticsPrev3.TitleHeight = 18;
			this.gridStatisticsPrev3.TranslationName = "Jobs";
			// 
			// gridStatisticsPrev1
			// 
			this.gridStatisticsPrev1.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridStatisticsPrev1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridStatisticsPrev1.HasAddButton = false;
			this.gridStatisticsPrev1.HasDropDowns = false;
			this.gridStatisticsPrev1.HasMultilineHeaders = false;
			this.gridStatisticsPrev1.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridStatisticsPrev1.HeaderHeight = 15;
			this.gridStatisticsPrev1.HScrollVisible = false;
			this.gridStatisticsPrev1.Location = new System.Drawing.Point(296, 3);
			this.gridStatisticsPrev1.Name = "gridStatisticsPrev1";
			this.gridStatisticsPrev1.ScrollValue = 0;
			this.gridStatisticsPrev1.Size = new System.Drawing.Size(287, 215);
			this.gridStatisticsPrev1.TabIndex = 1;
			this.gridStatisticsPrev1.Title = "18.1 Statistics";
			this.gridStatisticsPrev1.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridStatisticsPrev1.TitleHeight = 18;
			this.gridStatisticsPrev1.TranslationName = "Jobs";
			// 
			// gridStatisticsPrev2
			// 
			this.gridStatisticsPrev2.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridStatisticsPrev2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridStatisticsPrev2.HasAddButton = false;
			this.gridStatisticsPrev2.HasDropDowns = false;
			this.gridStatisticsPrev2.HasMultilineHeaders = false;
			this.gridStatisticsPrev2.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridStatisticsPrev2.HeaderHeight = 15;
			this.gridStatisticsPrev2.HScrollVisible = false;
			this.gridStatisticsPrev2.Location = new System.Drawing.Point(589, 3);
			this.gridStatisticsPrev2.Name = "gridStatisticsPrev2";
			this.gridStatisticsPrev2.ScrollValue = 0;
			this.gridStatisticsPrev2.Size = new System.Drawing.Size(287, 215);
			this.gridStatisticsPrev2.TabIndex = 2;
			this.gridStatisticsPrev2.Title = "17.4 Statistics";
			this.gridStatisticsPrev2.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridStatisticsPrev2.TitleHeight = 18;
			this.gridStatisticsPrev2.TranslationName = "Jobs";
			// 
			// userControlJobVersionStatisticsHead
			// 
			this.userControlJobVersionStatisticsHead.AutoSize = true;
			this.userControlJobVersionStatisticsHead.Dock = System.Windows.Forms.DockStyle.Fill;
			this.userControlJobVersionStatisticsHead.Location = new System.Drawing.Point(3, 224);
			this.userControlJobVersionStatisticsHead.Name = "userControlJobVersionStatisticsHead";
			this.userControlJobVersionStatisticsHead.Size = new System.Drawing.Size(287, 215);
			this.userControlJobVersionStatisticsHead.TabIndex = 8;
			// 
			// FormJobManagerOverview
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1186, 474);
			this.Controls.Add(this.tabControlMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormJobManagerOverview";
			this.Text = "Job Manager Overview";
			this.Load += new System.EventHandler(this.FormJobManagerOverview_Load);
			this.tabControlMain.ResumeLayout(false);
			this.tabHighPriority.ResumeLayout(false);
			this.tabJobsToWatch.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tabStatistics.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControlMain;
		private System.Windows.Forms.TabPage tabHighPriority;
		private UI.ODGrid gridHighPriorityJobs;
		private System.Windows.Forms.TabPage tabStatistics;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private UI.ODGrid gridStatisticsHead;
		private UI.ODGrid gridStatisticsPrev3;
		private UI.ODGrid gridStatisticsPrev1;
		private UI.ODGrid gridStatisticsPrev2;
		private System.Windows.Forms.TabPage tabJobsToWatch;
		private UserControlJobVersionStatistics userControlJobVersionStatisticsPrev3;
		private UserControlJobVersionStatistics userControlJobVersionStatisticsPrev2;
		private UserControlJobVersionStatistics userControlJobVersionStatisticsPrev1;
		private UserControlJobVersionStatistics userControlJobVersionStatisticsHead;
		private UI.ODGrid gridNoHours;
		private UI.ODGrid gridQuoteNotStarted;
		private UI.ODGrid gridOverEstimate;
		private UI.ODGrid gridOldJobs;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
	}
}