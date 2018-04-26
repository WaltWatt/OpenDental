namespace OpenDental{
	partial class FormTimeCardManage {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTimeCardManage));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.textDatePaycheck = new System.Windows.Forms.TextBox();
			this.textDateStop = new System.Windows.Forms.TextBox();
			this.textDateStart = new System.Windows.Forms.TextBox();
			this.butRight = new OpenDental.UI.Button();
			this.butLeft = new OpenDental.UI.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butDaily = new OpenDental.UI.Button();
			this.butCompute = new OpenDental.UI.Button();
			this.butPrintAll = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.butClearAuto = new OpenDental.UI.Button();
			this.butClearManual = new OpenDental.UI.Button();
			this.butPrintSelected = new OpenDental.UI.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.butTimeCardBenefits = new OpenDental.UI.Button();
			this.menuMain = new System.Windows.Forms.MenuStrip();
			this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.printGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportADPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.menuMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.textDatePaycheck);
			this.groupBox1.Controls.Add(this.textDateStop);
			this.groupBox1.Controls.Add(this.textDateStart);
			this.groupBox1.Controls.Add(this.butRight);
			this.groupBox1.Controls.Add(this.butLeft);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Location = new System.Drawing.Point(12, 33);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(587, 51);
			this.groupBox1.TabIndex = 15;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Pay Period";
			// 
			// textDatePaycheck
			// 
			this.textDatePaycheck.Location = new System.Drawing.Point(473, 19);
			this.textDatePaycheck.Name = "textDatePaycheck";
			this.textDatePaycheck.ReadOnly = true;
			this.textDatePaycheck.Size = new System.Drawing.Size(100, 20);
			this.textDatePaycheck.TabIndex = 14;
			// 
			// textDateStop
			// 
			this.textDateStop.Location = new System.Drawing.Point(244, 29);
			this.textDateStop.Name = "textDateStop";
			this.textDateStop.ReadOnly = true;
			this.textDateStop.Size = new System.Drawing.Size(100, 20);
			this.textDateStop.TabIndex = 13;
			// 
			// textDateStart
			// 
			this.textDateStart.Location = new System.Drawing.Point(244, 8);
			this.textDateStart.Name = "textDateStart";
			this.textDateStart.ReadOnly = true;
			this.textDateStart.Size = new System.Drawing.Size(100, 20);
			this.textDateStart.TabIndex = 12;
			// 
			// butRight
			// 
			this.butRight.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRight.Autosize = true;
			this.butRight.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRight.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRight.CornerRadius = 4F;
			this.butRight.Image = global::OpenDental.Properties.Resources.Right;
			this.butRight.Location = new System.Drawing.Point(63, 18);
			this.butRight.Name = "butRight";
			this.butRight.Size = new System.Drawing.Size(39, 24);
			this.butRight.TabIndex = 11;
			this.butRight.Click += new System.EventHandler(this.butRight_Click);
			// 
			// butLeft
			// 
			this.butLeft.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butLeft.Autosize = true;
			this.butLeft.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLeft.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLeft.CornerRadius = 4F;
			this.butLeft.Image = global::OpenDental.Properties.Resources.Left;
			this.butLeft.Location = new System.Drawing.Point(13, 18);
			this.butLeft.Name = "butLeft";
			this.butLeft.Size = new System.Drawing.Size(39, 24);
			this.butLeft.TabIndex = 10;
			this.butLeft.Click += new System.EventHandler(this.butLeft_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(354, 19);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(117, 18);
			this.label4.TabIndex = 9;
			this.label4.Text = "Paycheck Date";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(146, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 18);
			this.label2.TabIndex = 6;
			this.label2.Text = "Start Date";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(143, 29);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(99, 18);
			this.label3.TabIndex = 8;
			this.label3.Text = "End Date";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
			this.gridMain.Location = new System.Drawing.Point(12, 90);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(950, 541);
			this.gridMain.TabIndex = 16;
			this.gridMain.Title = "Employee Time Cards";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableTimeCard";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butDaily
			// 
			this.butDaily.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDaily.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDaily.Autosize = true;
			this.butDaily.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDaily.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDaily.CornerRadius = 4F;
			this.butDaily.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDaily.Location = new System.Drawing.Point(6, 18);
			this.butDaily.Name = "butDaily";
			this.butDaily.Size = new System.Drawing.Size(69, 24);
			this.butDaily.TabIndex = 119;
			this.butDaily.Text = "Daily";
			this.butDaily.Click += new System.EventHandler(this.butDaily_Click);
			// 
			// butCompute
			// 
			this.butCompute.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCompute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butCompute.Autosize = true;
			this.butCompute.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCompute.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCompute.CornerRadius = 4F;
			this.butCompute.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCompute.Location = new System.Drawing.Point(81, 18);
			this.butCompute.Name = "butCompute";
			this.butCompute.Size = new System.Drawing.Size(72, 24);
			this.butCompute.TabIndex = 118;
			this.butCompute.Text = "Weekly";
			this.butCompute.Click += new System.EventHandler(this.butWeekly_Click);
			// 
			// butPrintAll
			// 
			this.butPrintAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrintAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPrintAll.Autosize = true;
			this.butPrintAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrintAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrintAll.CornerRadius = 4F;
			this.butPrintAll.Image = global::OpenDental.Properties.Resources.butPrint;
			this.butPrintAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrintAll.Location = new System.Drawing.Point(6, 18);
			this.butPrintAll.Name = "butPrintAll";
			this.butPrintAll.Size = new System.Drawing.Size(87, 24);
			this.butPrintAll.TabIndex = 116;
			this.butPrintAll.Text = "&Print All";
			this.butPrintAll.Click += new System.EventHandler(this.butPrintAll_Click);
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
			this.butClose.Location = new System.Drawing.Point(887, 655);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 2;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butClearAuto
			// 
			this.butClearAuto.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClearAuto.Autosize = true;
			this.butClearAuto.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClearAuto.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClearAuto.CornerRadius = 4F;
			this.butClearAuto.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butClearAuto.Location = new System.Drawing.Point(845, 60);
			this.butClearAuto.Name = "butClearAuto";
			this.butClearAuto.Size = new System.Drawing.Size(117, 24);
			this.butClearAuto.TabIndex = 122;
			this.butClearAuto.Text = "Clear Auto Adjusts";
			this.butClearAuto.Click += new System.EventHandler(this.butClearAuto_Click);
			// 
			// butClearManual
			// 
			this.butClearManual.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClearManual.Autosize = true;
			this.butClearManual.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClearManual.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClearManual.CornerRadius = 4F;
			this.butClearManual.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butClearManual.Location = new System.Drawing.Point(845, 33);
			this.butClearManual.Name = "butClearManual";
			this.butClearManual.Size = new System.Drawing.Size(117, 24);
			this.butClearManual.TabIndex = 123;
			this.butClearManual.Text = "Clear Manual Adjusts";
			this.butClearManual.Click += new System.EventHandler(this.butClearManual_Click);
			// 
			// butPrintSelected
			// 
			this.butPrintSelected.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrintSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPrintSelected.Autosize = true;
			this.butPrintSelected.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrintSelected.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrintSelected.CornerRadius = 4F;
			this.butPrintSelected.Image = global::OpenDental.Properties.Resources.butPrint;
			this.butPrintSelected.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrintSelected.Location = new System.Drawing.Point(99, 18);
			this.butPrintSelected.Name = "butPrintSelected";
			this.butPrintSelected.Size = new System.Drawing.Size(109, 24);
			this.butPrintSelected.TabIndex = 124;
			this.butPrintSelected.Text = "Print Selected";
			this.butPrintSelected.Click += new System.EventHandler(this.butPrintSelected_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox2.Controls.Add(this.butDaily);
			this.groupBox2.Controls.Add(this.butCompute);
			this.groupBox2.Location = new System.Drawing.Point(12, 637);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(160, 48);
			this.groupBox2.TabIndex = 16;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Calculations";
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox3.Controls.Add(this.butPrintAll);
			this.groupBox3.Controls.Add(this.butPrintSelected);
			this.groupBox3.Location = new System.Drawing.Point(178, 637);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(215, 48);
			this.groupBox3.TabIndex = 125;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Time Cards";
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(641, 61);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(162, 21);
			this.comboClinic.TabIndex = 128;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.comboClinic_SelectionChangeCommitted);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(642, 45);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(58, 13);
			this.labelClinic.TabIndex = 127;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butTimeCardBenefits
			// 
			this.butTimeCardBenefits.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butTimeCardBenefits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butTimeCardBenefits.Autosize = true;
			this.butTimeCardBenefits.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTimeCardBenefits.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTimeCardBenefits.CornerRadius = 4F;
			this.butTimeCardBenefits.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butTimeCardBenefits.Location = new System.Drawing.Point(785, 655);
			this.butTimeCardBenefits.Name = "butTimeCardBenefits";
			this.butTimeCardBenefits.Size = new System.Drawing.Size(70, 24);
			this.butTimeCardBenefits.TabIndex = 119;
			this.butTimeCardBenefits.Text = "Benefits";
			this.butTimeCardBenefits.Visible = false;
			this.butTimeCardBenefits.Click += new System.EventHandler(this.butTimeCardBenefits_Click);
			// 
			// menuMain
			// 
			this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setupToolStripMenuItem,
            this.reportsToolStripMenuItem});
			this.menuMain.Location = new System.Drawing.Point(0, 0);
			this.menuMain.Name = "menuMain";
			this.menuMain.Size = new System.Drawing.Size(974, 24);
			this.menuMain.TabIndex = 129;
			// 
			// setupToolStripMenuItem
			// 
			this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
			this.setupToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
			this.setupToolStripMenuItem.Text = "Setup";
			this.setupToolStripMenuItem.Click += new System.EventHandler(this.setupToolStripMenuItem_Click);
			// 
			// reportsToolStripMenuItem
			// 
			this.reportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printGridToolStripMenuItem,
            this.exportGridToolStripMenuItem,
            this.exportADPToolStripMenuItem});
			this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
			this.reportsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
			this.reportsToolStripMenuItem.Text = "Reports";
			// 
			// printGridToolStripMenuItem
			// 
			this.printGridToolStripMenuItem.Name = "printGridToolStripMenuItem";
			this.printGridToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.printGridToolStripMenuItem.Text = "Print Grid";
			this.printGridToolStripMenuItem.Click += new System.EventHandler(this.butPrintGrid_Click);
			// 
			// exportGridToolStripMenuItem
			// 
			this.exportGridToolStripMenuItem.Name = "exportGridToolStripMenuItem";
			this.exportGridToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.exportGridToolStripMenuItem.Text = "Export Grid";
			this.exportGridToolStripMenuItem.Click += new System.EventHandler(this.butExportGrid_Click);
			// 
			// exportADPToolStripMenuItem
			// 
			this.exportADPToolStripMenuItem.Name = "exportADPToolStripMenuItem";
			this.exportADPToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.exportADPToolStripMenuItem.Text = "Export ADP";
			this.exportADPToolStripMenuItem.Click += new System.EventHandler(this.butExportADP_Click);
			// 
			// FormTimeCardManage
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(974, 696);
			this.Controls.Add(this.menuMain);
			this.Controls.Add(this.butTimeCardBenefits);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.butClearManual);
			this.Controls.Add(this.butClearAuto);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormTimeCardManage";
			this.Text = "Manage Time Cards";
			this.Load += new System.EventHandler(this.FormTimeCardManage_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.menuMain.ResumeLayout(false);
			this.menuMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox textDatePaycheck;
		private System.Windows.Forms.TextBox textDateStop;
		private System.Windows.Forms.TextBox textDateStart;
		private UI.Button butRight;
		private UI.Button butLeft;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private UI.ODGrid gridMain;
		private UI.Button butPrintAll;
		private UI.Button butDaily;
		private UI.Button butCompute;
		private UI.Button butClearAuto;
		private UI.Button butClearManual;
		private UI.Button butPrintSelected;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.ComboBox comboClinic;
		private System.Windows.Forms.Label labelClinic;
		private UI.Button butTimeCardBenefits;
		private System.Windows.Forms.MenuStrip menuMain;
		private System.Windows.Forms.ToolStripMenuItem setupToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem printGridToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportGridToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportADPToolStripMenuItem;
	}
}