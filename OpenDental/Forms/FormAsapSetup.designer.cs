namespace OpenDental{
	partial class FormAsapSetup {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAsapSetup));
			this.butClose = new OpenDental.UI.Button();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.checkUseDefaults = new System.Windows.Forms.CheckBox();
			this.textWebSchedPerDay = new OpenDental.ValidNumber();
			this.labelMaxWebSched = new System.Windows.Forms.Label();
			this.labelLeaveBlank = new System.Windows.Forms.Label();
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
			this.butClose.Location = new System.Drawing.Point(597, 401);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 3;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(153, 32);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(194, 21);
			this.comboClinic.TabIndex = 167;
			this.comboClinic.SelectedIndexChanged += new System.EventHandler(this.comboClinic_SelectedIndexChanged);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(90, 33);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(57, 16);
			this.labelClinic.TabIndex = 168;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// gridMain
			// 
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = true;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 63);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(660, 292);
			this.gridMain.TabIndex = 166;
			this.gridMain.Title = "Templates";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableTemplates";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// checkUseDefaults
			// 
			this.checkUseDefaults.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkUseDefaults.Location = new System.Drawing.Point(353, 33);
			this.checkUseDefaults.Name = "checkUseDefaults";
			this.checkUseDefaults.Size = new System.Drawing.Size(105, 19);
			this.checkUseDefaults.TabIndex = 264;
			this.checkUseDefaults.Text = "Use Defaults";
			this.checkUseDefaults.Click += new System.EventHandler(this.checkUseDefaults_Click);
			// 
			// textWebSchedPerDay
			// 
			this.textWebSchedPerDay.Location = new System.Drawing.Point(226, 372);
			this.textWebSchedPerDay.MaxVal = 10000000;
			this.textWebSchedPerDay.MinVal = 0;
			this.textWebSchedPerDay.Name = "textWebSchedPerDay";
			this.textWebSchedPerDay.Size = new System.Drawing.Size(39, 20);
			this.textWebSchedPerDay.TabIndex = 265;
			this.textWebSchedPerDay.Leave += new System.EventHandler(this.textWebSchedPerDay_Leave);
			// 
			// labelMaxWebSched
			// 
			this.labelMaxWebSched.Location = new System.Drawing.Point(15, 365);
			this.labelMaxWebSched.Name = "labelMaxWebSched";
			this.labelMaxWebSched.Size = new System.Drawing.Size(205, 33);
			this.labelMaxWebSched.TabIndex = 266;
			this.labelMaxWebSched.Text = "Maximum number of texts to send to a patient in a day via Web Sched";
			this.labelMaxWebSched.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelLeaveBlank
			// 
			this.labelLeaveBlank.Location = new System.Drawing.Point(271, 374);
			this.labelLeaveBlank.Name = "labelLeaveBlank";
			this.labelLeaveBlank.Size = new System.Drawing.Size(146, 16);
			this.labelLeaveBlank.TabIndex = 267;
			this.labelLeaveBlank.Text = "(leave blank for no limit)";
			this.labelLeaveBlank.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FormAsapSetup
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(684, 437);
			this.Controls.Add(this.labelLeaveBlank);
			this.Controls.Add(this.labelMaxWebSched);
			this.Controls.Add(this.textWebSchedPerDay);
			this.Controls.Add(this.checkUseDefaults);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormAsapSetup";
			this.Text = "ASAP List Setup";
			this.Load += new System.EventHandler(this.FormAsapSetup_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.ComboBox comboClinic;
		private System.Windows.Forms.Label labelClinic;
		private UI.ODGrid gridMain;
		private System.Windows.Forms.CheckBox checkUseDefaults;
		private ValidNumber textWebSchedPerDay;
		private System.Windows.Forms.Label labelMaxWebSched;
		private System.Windows.Forms.Label labelLeaveBlank;
	}
}