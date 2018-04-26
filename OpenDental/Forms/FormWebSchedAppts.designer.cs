namespace OpenDental {
	partial class FormWebSchedAppts {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWebSchedAppts));
			this.butClose = new OpenDental.UI.Button();
			this.datePicker = new OpenDental.UI.ODDateRangePicker();
			this.comboBoxClinicMulti = new OpenDental.UI.ComboBoxClinicMulti();
			this.comboBoxMultiConfStatus = new OpenDental.UI.ComboBoxMulti();
			this.labelClinicsMulti = new System.Windows.Forms.Label();
			this.labelConfStatus = new System.Windows.Forms.Label();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.contextMenuMainGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mainGridMenuItemPatChart = new System.Windows.Forms.ToolStripMenuItem();
			this.mainGridMenuItemApptEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.butRefresh = new OpenDental.UI.Button();
			this.comboBoxMultiWebSchedApptType = new OpenDental.UI.ComboBoxMulti();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkWebSchedNewPat = new System.Windows.Forms.CheckBox();
			this.checkWebSchedRecall = new System.Windows.Forms.CheckBox();
			this.checkASAP = new System.Windows.Forms.CheckBox();
			this.contextMenuMainGrid.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(830, 660);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 2;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// datePicker
			// 
			this.datePicker.BackColor = System.Drawing.SystemColors.Control;
			this.datePicker.DefaultDateTimeFrom = new System.DateTime(2017, 1, 1, 0, 0, 0, 0);
			this.datePicker.DefaultDateTimeTo = new System.DateTime(2017, 8, 23, 0, 0, 0, 0);
			this.datePicker.EnableWeekButtons = false;
			this.datePicker.Location = new System.Drawing.Point(13, 12);
			this.datePicker.MaximumSize = new System.Drawing.Size(0, 185);
			this.datePicker.MinimumSize = new System.Drawing.Size(453, 22);
			this.datePicker.Name = "datePicker";
			this.datePicker.Size = new System.Drawing.Size(453, 22);
			this.datePicker.TabIndex = 4;
			// 
			// comboBoxClinicMulti
			// 
			this.comboBoxClinicMulti.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxClinicMulti.Location = new System.Drawing.Point(213, 53);
			this.comboBoxClinicMulti.Name = "comboBoxClinicMulti";
			this.comboBoxClinicMulti.Size = new System.Drawing.Size(160, 22);
			this.comboBoxClinicMulti.TabIndex = 5;
			// 
			// comboBoxMultiConfStatus
			// 
			this.comboBoxMultiConfStatus.ArraySelectedIndices = new int[0];
			this.comboBoxMultiConfStatus.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiConfStatus.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiConfStatus.Items")));
			this.comboBoxMultiConfStatus.Location = new System.Drawing.Point(213, 81);
			this.comboBoxMultiConfStatus.Name = "comboBoxMultiConfStatus";
			this.comboBoxMultiConfStatus.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiConfStatus.SelectedIndices")));
			this.comboBoxMultiConfStatus.Size = new System.Drawing.Size(160, 21);
			this.comboBoxMultiConfStatus.TabIndex = 6;
			// 
			// labelClinicsMulti
			// 
			this.labelClinicsMulti.Location = new System.Drawing.Point(97, 53);
			this.labelClinicsMulti.Name = "labelClinicsMulti";
			this.labelClinicsMulti.Size = new System.Drawing.Size(110, 23);
			this.labelClinicsMulti.TabIndex = 8;
			this.labelClinicsMulti.Text = "Clinics";
			this.labelClinicsMulti.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelConfStatus
			// 
			this.labelConfStatus.Location = new System.Drawing.Point(64, 81);
			this.labelConfStatus.Name = "labelConfStatus";
			this.labelConfStatus.Size = new System.Drawing.Size(143, 23);
			this.labelConfStatus.TabIndex = 9;
			this.labelConfStatus.Text = "Confirmation Status";
			this.labelConfStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// gridMain
			// 
			this.gridMain.AllowSortingByColumn = true;
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.ContextMenuStrip = this.contextMenuMainGrid;
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = true;
			this.gridMain.Location = new System.Drawing.Point(13, 108);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(892, 546);
			this.gridMain.TabIndex = 11;
			this.gridMain.Title = "Appointments";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableAppointments";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// contextMenuMainGrid
			// 
			this.contextMenuMainGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainGridMenuItemPatChart,
            this.mainGridMenuItemApptEdit});
			this.contextMenuMainGrid.Name = "contextMenuMainGrid";
			this.contextMenuMainGrid.Size = new System.Drawing.Size(176, 48);
			// 
			// mainGridMenuItemPatChart
			// 
			this.mainGridMenuItemPatChart.Name = "mainGridMenuItemPatChart";
			this.mainGridMenuItemPatChart.Size = new System.Drawing.Size(175, 22);
			this.mainGridMenuItemPatChart.Text = "Open Patient Chart";
			this.mainGridMenuItemPatChart.Click += new System.EventHandler(this.mainGridMenuItemPatChart_Click);
			// 
			// mainGridMenuItemApptEdit
			// 
			this.mainGridMenuItemApptEdit.Name = "mainGridMenuItemApptEdit";
			this.mainGridMenuItemApptEdit.Size = new System.Drawing.Size(175, 22);
			this.mainGridMenuItemApptEdit.Text = "Edit Appointment";
			this.mainGridMenuItemApptEdit.Click += new System.EventHandler(this.mainGridMenuItemApptEdit_Click);
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(828, 79);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(77, 23);
			this.butRefresh.TabIndex = 49;
			this.butRefresh.Text = "&Refresh List";
			this.butRefresh.UseVisualStyleBackColor = true;
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// comboBoxMultiWebSchedApptType
			// 
			this.comboBoxMultiWebSchedApptType.ArraySelectedIndices = new int[0];
			this.comboBoxMultiWebSchedApptType.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiWebSchedApptType.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiWebSchedApptType.Items")));
			this.comboBoxMultiWebSchedApptType.Location = new System.Drawing.Point(0, 0);
			this.comboBoxMultiWebSchedApptType.Name = "comboBoxMultiWebSchedApptType";
			this.comboBoxMultiWebSchedApptType.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiWebSchedApptType.SelectedIndices")));
			this.comboBoxMultiWebSchedApptType.Size = new System.Drawing.Size(120, 21);
			this.comboBoxMultiWebSchedApptType.TabIndex = 0;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkASAP);
			this.groupBox2.Controls.Add(this.checkWebSchedNewPat);
			this.groupBox2.Controls.Add(this.checkWebSchedRecall);
			this.groupBox2.Location = new System.Drawing.Point(472, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(322, 90);
			this.groupBox2.TabIndex = 50;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Web Sched Appointment Types";
			// 
			// checkWebSchedNewPat
			// 
			this.checkWebSchedNewPat.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkWebSchedNewPat.Location = new System.Drawing.Point(92, 43);
			this.checkWebSchedNewPat.Name = "checkWebSchedNewPat";
			this.checkWebSchedNewPat.Size = new System.Drawing.Size(202, 18);
			this.checkWebSchedNewPat.TabIndex = 48;
			this.checkWebSchedNewPat.Text = "Show New Patient Appointments";
			// 
			// checkWebSchedRecall
			// 
			this.checkWebSchedRecall.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkWebSchedRecall.Location = new System.Drawing.Point(92, 19);
			this.checkWebSchedRecall.Name = "checkWebSchedRecall";
			this.checkWebSchedRecall.Size = new System.Drawing.Size(202, 18);
			this.checkWebSchedRecall.TabIndex = 46;
			this.checkWebSchedRecall.Text = "Show Recall Appointments";
			// 
			// checkASAP
			// 
			this.checkASAP.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkASAP.Location = new System.Drawing.Point(92, 66);
			this.checkASAP.Name = "checkASAP";
			this.checkASAP.Size = new System.Drawing.Size(202, 18);
			this.checkASAP.TabIndex = 49;
			this.checkASAP.Text = "Show ASAP Appointments";
			// 
			// FormWebSchedAppts
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(917, 696);
			this.Controls.Add(this.datePicker);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.labelConfStatus);
			this.Controls.Add(this.labelClinicsMulti);
			this.Controls.Add(this.comboBoxMultiConfStatus);
			this.Controls.Add(this.comboBoxClinicMulti);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormWebSchedAppts";
			this.Text = "Web Sched Appointments";
			this.Load += new System.EventHandler(this.FormWebSchedAppts_Load);
			this.contextMenuMainGrid.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private OpenDental.UI.Button butClose;
		private UI.ODDateRangePicker datePicker;
		private UI.ComboBoxClinicMulti comboBoxClinicMulti;
		private UI.ComboBoxMulti comboBoxMultiConfStatus;
		private System.Windows.Forms.Label labelClinicsMulti;
		private System.Windows.Forms.Label labelConfStatus;
		private UI.ODGrid gridMain;
		private UI.Button butRefresh;
		private UI.ComboBoxMulti comboBoxMultiWebSchedApptType;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox checkWebSchedNewPat;
		private System.Windows.Forms.CheckBox checkWebSchedRecall;
		private System.Windows.Forms.ContextMenuStrip contextMenuMainGrid;
		private System.Windows.Forms.ToolStripMenuItem mainGridMenuItemPatChart;
		private System.Windows.Forms.ToolStripMenuItem mainGridMenuItemApptEdit;
		private System.Windows.Forms.CheckBox checkASAP;
	}
}