namespace OpenDental {
	partial class FormDatabaseMaintenancePat {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDatabaseMaintenancePat));
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butFix = new OpenDental.UI.Button();
			this.butCheck = new OpenDental.UI.Button();
			this.labelSkipCheckTable = new System.Windows.Forms.Label();
			this.butClose = new OpenDental.UI.Button();
			this.textPatient = new System.Windows.Forms.TextBox();
			this.butPatientSelect = new OpenDental.UI.Button();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.checkShow = new System.Windows.Forms.CheckBox();
			this.butNone = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = true;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 64);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(810, 418);
			this.gridMain.TabIndex = 4;
			this.gridMain.Title = "Patient Specific Database Checks";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableDatabaseMaintPat";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butFix
			// 
			this.butFix.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butFix.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.butFix.Autosize = true;
			this.butFix.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butFix.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butFix.CornerRadius = 4F;
			this.butFix.Location = new System.Drawing.Point(442, 564);
			this.butFix.Name = "butFix";
			this.butFix.Size = new System.Drawing.Size(75, 26);
			this.butFix.TabIndex = 7;
			this.butFix.Text = "&Fix";
			this.butFix.Click += new System.EventHandler(this.butFix_Click);
			// 
			// butCheck
			// 
			this.butCheck.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCheck.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.butCheck.Autosize = true;
			this.butCheck.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCheck.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCheck.CornerRadius = 4F;
			this.butCheck.Location = new System.Drawing.Point(317, 564);
			this.butCheck.Name = "butCheck";
			this.butCheck.Size = new System.Drawing.Size(75, 26);
			this.butCheck.TabIndex = 6;
			this.butCheck.Text = "C&heck";
			this.butCheck.Click += new System.EventHandler(this.butCheck_Click);
			// 
			// labelSkipCheckTable
			// 
			this.labelSkipCheckTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelSkipCheckTable.ForeColor = System.Drawing.Color.Red;
			this.labelSkipCheckTable.Location = new System.Drawing.Point(597, 574);
			this.labelSkipCheckTable.Name = "labelSkipCheckTable";
			this.labelSkipCheckTable.Size = new System.Drawing.Size(144, 16);
			this.labelSkipCheckTable.TabIndex = 9;
			this.labelSkipCheckTable.Text = "Table check is disabled";
			this.labelSkipCheckTable.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.labelSkipCheckTable.Visible = false;
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
			this.butClose.Location = new System.Drawing.Point(747, 567);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 8;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// textPatient
			// 
			this.textPatient.Location = new System.Drawing.Point(317, 25);
			this.textPatient.Name = "textPatient";
			this.textPatient.ReadOnly = true;
			this.textPatient.Size = new System.Drawing.Size(200, 20);
			this.textPatient.TabIndex = 10;
			// 
			// butPatientSelect
			// 
			this.butPatientSelect.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPatientSelect.Autosize = true;
			this.butPatientSelect.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPatientSelect.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPatientSelect.CornerRadius = 4F;
			this.butPatientSelect.Location = new System.Drawing.Point(523, 25);
			this.butPatientSelect.Name = "butPatientSelect";
			this.butPatientSelect.Size = new System.Drawing.Size(26, 20);
			this.butPatientSelect.TabIndex = 11;
			this.butPatientSelect.Text = "...";
			this.butPatientSelect.Click += new System.EventHandler(this.butPatientSelect_Click);
			// 
			// textBox2
			// 
			this.textBox2.BackColor = System.Drawing.SystemColors.Control;
			this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox2.Location = new System.Drawing.Point(101, 28);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(210, 16);
			this.textBox2.TabIndex = 100;
			this.textBox2.TabStop = false;
			this.textBox2.Text = "Patient\r\n";
			this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.BackColor = System.Drawing.SystemColors.Control;
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox1.Location = new System.Drawing.Point(366, 488);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(365, 26);
			this.textBox1.TabIndex = 103;
			this.textBox1.TabStop = false;
			this.textBox1.Text = "No selections will cause all database checks to run.\r\nOtherwise only selected che" +
    "cks will run.\r\n";
			this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// checkShow
			// 
			this.checkShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkShow.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShow.Location = new System.Drawing.Point(22, 488);
			this.checkShow.Name = "checkShow";
			this.checkShow.Size = new System.Drawing.Size(447, 20);
			this.checkShow.TabIndex = 101;
			this.checkShow.Text = "Show me everything in the log  (only for advanced users)";
			// 
			// butNone
			// 
			this.butNone.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butNone.Autosize = true;
			this.butNone.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNone.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNone.CornerRadius = 4F;
			this.butNone.Location = new System.Drawing.Point(737, 488);
			this.butNone.Name = "butNone";
			this.butNone.Size = new System.Drawing.Size(75, 26);
			this.butNone.TabIndex = 102;
			this.butNone.Text = "None";
			this.butNone.Click += new System.EventHandler(this.butNone_Click);
			// 
			// FormDatabaseMaintenancePat
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(834, 605);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.checkShow);
			this.Controls.Add(this.butNone);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.butPatientSelect);
			this.Controls.Add(this.textPatient);
			this.Controls.Add(this.labelSkipCheckTable);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.butFix);
			this.Controls.Add(this.butCheck);
			this.Controls.Add(this.gridMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(716, 344);
			this.Name = "FormDatabaseMaintenancePat";
			this.Text = "Database Maintenance for Patient";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDatabaseMaintenancePat_FormClosing);
			this.Load += new System.EventHandler(this.FormDatabaseMaintenancePat_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private UI.ODGrid gridMain;
		private UI.Button butFix;
		private UI.Button butCheck;
		private System.Windows.Forms.Label labelSkipCheckTable;
		private UI.Button butClose;
		private System.Windows.Forms.TextBox textPatient;
		private UI.Button butPatientSelect;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.CheckBox checkShow;
		private UI.Button butNone;
	}
}