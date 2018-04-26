namespace OpenDental{
	partial class FormRpInsPayPlansPastDue {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpInsPayPlansPastDue));
      this.butClose = new OpenDental.UI.Button();
      this.labelProv = new System.Windows.Forms.Label();
      this.comboBoxMultiProv = new OpenDental.UI.ComboBoxMulti();
      this.labelClinic = new System.Windows.Forms.Label();
      this.comboBoxMultiClinics = new OpenDental.UI.ComboBoxMulti();
      this.gridMain = new OpenDental.UI.ODGrid();
      this.label2 = new System.Windows.Forms.Label();
      this.textDaysPastDue = new System.Windows.Forms.TextBox();
      this.timerTypeDays = new System.Windows.Forms.Timer(this.components);
      this.butExport = new OpenDental.UI.Button();
      this.butPrint = new OpenDental.UI.Button();
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
      this.butClose.Location = new System.Drawing.Point(758, 463);
      this.butClose.Name = "butClose";
      this.butClose.Size = new System.Drawing.Size(75, 24);
      this.butClose.TabIndex = 2;
      this.butClose.Text = "&Close";
      this.butClose.Click += new System.EventHandler(this.butClose_Click);
      // 
      // labelProv
      // 
      this.labelProv.Location = new System.Drawing.Point(135, 20);
      this.labelProv.Name = "labelProv";
      this.labelProv.Size = new System.Drawing.Size(70, 16);
      this.labelProv.TabIndex = 61;
      this.labelProv.Text = "Provs:";
      this.labelProv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // comboBoxMultiProv
      // 
      this.comboBoxMultiProv.ArraySelectedIndices = new int[0];
      this.comboBoxMultiProv.BackColor = System.Drawing.SystemColors.Window;
      this.comboBoxMultiProv.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiProv.Items")));
      this.comboBoxMultiProv.Location = new System.Drawing.Point(206, 18);
      this.comboBoxMultiProv.Name = "comboBoxMultiProv";
      this.comboBoxMultiProv.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiProv.SelectedIndices")));
      this.comboBoxMultiProv.Size = new System.Drawing.Size(160, 21);
      this.comboBoxMultiProv.TabIndex = 62;
      this.comboBoxMultiProv.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.comboBoxMulti_SelectionChangeCommitted);
      // 
      // labelClinic
      // 
      this.labelClinic.Location = new System.Drawing.Point(369, 20);
      this.labelClinic.Name = "labelClinic";
      this.labelClinic.Size = new System.Drawing.Size(70, 16);
      this.labelClinic.TabIndex = 64;
      this.labelClinic.Text = "Clinics:";
      this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.labelClinic.Visible = false;
      // 
      // comboBoxMultiClinics
      // 
      this.comboBoxMultiClinics.ArraySelectedIndices = new int[0];
      this.comboBoxMultiClinics.BackColor = System.Drawing.SystemColors.Window;
      this.comboBoxMultiClinics.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiClinics.Items")));
      this.comboBoxMultiClinics.Location = new System.Drawing.Point(440, 18);
      this.comboBoxMultiClinics.Name = "comboBoxMultiClinics";
      this.comboBoxMultiClinics.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiClinics.SelectedIndices")));
      this.comboBoxMultiClinics.Size = new System.Drawing.Size(160, 21);
      this.comboBoxMultiClinics.TabIndex = 63;
      this.comboBoxMultiClinics.Visible = false;
      this.comboBoxMultiClinics.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.comboBoxMulti_SelectionChangeCommitted);
      // 
      // gridMain
      // 
      this.gridMain.AllowSortingByColumn = true;
      this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
      this.gridMain.HasAddButton = false;
      this.gridMain.HasMultilineHeaders = false;
      this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
      this.gridMain.HeaderHeight = 15;
      this.gridMain.HScrollVisible = false;
      this.gridMain.Location = new System.Drawing.Point(8, 40);
      this.gridMain.Name = "gridMain";
      this.gridMain.ScrollValue = 0;
      this.gridMain.Size = new System.Drawing.Size(825, 417);
      this.gridMain.TabIndex = 65;
      this.gridMain.Title = "Ins Pay Plans Past Due";
      this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
      this.gridMain.TitleHeight = 18;
      this.gridMain.TranslationName = "TableInsPayPlanPastDue";
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(-2, 1);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(87, 34);
      this.label2.TabIndex = 66;
      this.label2.Text = "Days past due:";
      this.label2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // textDaysPastDue
      // 
      this.textDaysPastDue.Location = new System.Drawing.Point(83, 19);
      this.textDaysPastDue.Name = "textDaysPastDue";
      this.textDaysPastDue.Size = new System.Drawing.Size(52, 20);
      this.textDaysPastDue.TabIndex = 69;
      this.textDaysPastDue.Text = "30";
      this.textDaysPastDue.TextChanged += new System.EventHandler(this.textDaysPastDue_TextChanged);
      // 
      // timerTypeDays
      // 
      this.timerTypeDays.Interval = 300;
      this.timerTypeDays.Tick += new System.EventHandler(this.timerTypeDays_Tick);
      // 
      // butExport
      // 
      this.butExport.AdjustImageLocation = new System.Drawing.Point(0, 0);
      this.butExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.butExport.Autosize = true;
      this.butExport.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
      this.butExport.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
      this.butExport.CornerRadius = 4F;
      this.butExport.Image = global::OpenDental.Properties.Resources.butExport;
      this.butExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.butExport.Location = new System.Drawing.Point(93, 463);
      this.butExport.Name = "butExport";
      this.butExport.Size = new System.Drawing.Size(79, 24);
      this.butExport.TabIndex = 71;
      this.butExport.Text = "&Export";
      this.butExport.Click += new System.EventHandler(this.butExport_Click);
      // 
      // butPrint
      // 
      this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
      this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.butPrint.Autosize = true;
      this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
      this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
      this.butPrint.CornerRadius = 4F;
      this.butPrint.Image = global::OpenDental.Properties.Resources.butPrintSmall;
      this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.butPrint.Location = new System.Drawing.Point(8, 463);
      this.butPrint.Name = "butPrint";
      this.butPrint.Size = new System.Drawing.Size(79, 24);
      this.butPrint.TabIndex = 70;
      this.butPrint.Text = "&Print";
      this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
      // 
      // FormRpInsPayPlansPastDue
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.ClientSize = new System.Drawing.Size(845, 499);
      this.Controls.Add(this.butExport);
      this.Controls.Add(this.butPrint);
      this.Controls.Add(this.textDaysPastDue);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.gridMain);
      this.Controls.Add(this.labelProv);
      this.Controls.Add(this.comboBoxMultiProv);
      this.Controls.Add(this.labelClinic);
      this.Controls.Add(this.comboBoxMultiClinics);
      this.Controls.Add(this.butClose);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "FormRpInsPayPlansPastDue";
      this.Text = "Insurance Payment Plans Past Due";
      this.Load += new System.EventHandler(this.FormRpInsPayPlansPastDue_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

		}

		#endregion
		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.Label labelProv;
		private UI.ComboBoxMulti comboBoxMultiProv;
		private System.Windows.Forms.Label labelClinic;
		private UI.ComboBoxMulti comboBoxMultiClinics;
		private UI.ODGrid gridMain;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textDaysPastDue;
		private System.Windows.Forms.Timer timerTypeDays;
		private UI.Button butExport;
		private UI.Button butPrint;
	}
}