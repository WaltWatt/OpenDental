namespace OpenDental {
	partial class FormEtrans835s {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing&&(components!=null)) {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEtrans835s));
			this.imageListCalendar = new System.Windows.Forms.ImageList(this.components);
			this.smsService1 = new OpenDental.CallFireService.SMSService();
			this.listStatus = new System.Windows.Forms.ListBox();
			this.calendarTo = new System.Windows.Forms.MonthCalendar();
			this.calendarFrom = new System.Windows.Forms.MonthCalendar();
			this.textCarrier = new OpenDental.ODtextBox();
			this.comboClinics = new OpenDental.UI.ComboBoxMulti();
			this.butRefresh = new OpenDental.UI.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.textRangeMin = new OpenDental.ValidNum();
			this.label3 = new System.Windows.Forms.Label();
			this.textRangeMax = new OpenDental.ValidNum();
			this.labelDaysOldMax = new System.Windows.Forms.Label();
			this.labelDaysOldMin = new System.Windows.Forms.Label();
			this.textCheckTrace = new OpenDental.ODtextBox();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butClose = new OpenDental.UI.Button();
			this.labelClinic = new System.Windows.Forms.Label();
			this.butDropTo = new OpenDental.UI.Button();
			this.labelCheckTrace = new System.Windows.Forms.Label();
			this.labelCarrier = new System.Windows.Forms.Label();
			this.butDropFrom = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.labelStatus = new System.Windows.Forms.Label();
			this.textDateTo = new OpenDental.ValidDate();
			this.textDateFrom = new OpenDental.ValidDate();
			this.butWeekPrevious = new OpenDental.UI.Button();
			this.butWeekNext = new OpenDental.UI.Button();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageListCalendar
			// 
			this.imageListCalendar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListCalendar.ImageStream")));
			this.imageListCalendar.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListCalendar.Images.SetKeyName(0, "arrowDownTriangle.gif");
			this.imageListCalendar.Images.SetKeyName(1, "arrowUpTriangle.gif");
			// 
			// smsService1
			// 
			this.smsService1.Credentials = null;
			this.smsService1.Url = "https://www.callfire.com/service/SMSService";
			this.smsService1.UseDefaultCredentials = false;
			// 
			// listStatus
			// 
			this.listStatus.FormattingEnabled = true;
			this.listStatus.Location = new System.Drawing.Point(805, 8);
			this.listStatus.Name = "listStatus";
			this.listStatus.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listStatus.Size = new System.Drawing.Size(120, 56);
			this.listStatus.TabIndex = 106;
			this.listStatus.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listStatus_MouseClick);
			// 
			// calendarTo
			// 
			this.calendarTo.Location = new System.Drawing.Point(239, 27);
			this.calendarTo.Name = "calendarTo";
			this.calendarTo.TabIndex = 75;
			this.calendarTo.Visible = false;
			this.calendarTo.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.calendarTo_DateSelected);
			// 
			// calendarFrom
			// 
			this.calendarFrom.Location = new System.Drawing.Point(13, 27);
			this.calendarFrom.MaxSelectionCount = 1;
			this.calendarFrom.Name = "calendarFrom";
			this.calendarFrom.TabIndex = 74;
			this.calendarFrom.Visible = false;
			this.calendarFrom.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.calendarFrom_DateSelected);
			// 
			// textCarrier
			// 
			this.textCarrier.AcceptsTab = true;
			this.textCarrier.BackColor = System.Drawing.SystemColors.Window;
			this.textCarrier.DetectLinksEnabled = false;
			this.textCarrier.DetectUrls = false;
			this.textCarrier.Location = new System.Drawing.Point(586, 8);
			this.textCarrier.Name = "textCarrier";
			this.textCarrier.QuickPasteType = OpenDentBusiness.QuickPasteType.InsPlan;
			this.textCarrier.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textCarrier.Size = new System.Drawing.Size(161, 21);
			this.textCarrier.TabIndex = 105;
			this.textCarrier.Text = "";
			this.textCarrier.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textCarrier_KeyUp);
			// 
			// comboClinics
			// 
			this.comboClinics.ArraySelectedIndices = new int[0];
			this.comboClinics.BackColor = System.Drawing.SystemColors.Window;
			this.comboClinics.Items = ((System.Collections.ArrayList)(resources.GetObject("comboClinics.Items")));
			this.comboClinics.Location = new System.Drawing.Point(586, 50);
			this.comboClinics.Name = "comboClinics";
			this.comboClinics.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboClinics.SelectedIndices")));
			this.comboClinics.Size = new System.Drawing.Size(161, 21);
			this.comboClinics.TabIndex = 7;
			this.comboClinics.Visible = false;
			this.comboClinics.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.comboClinics_SelectionChangeCommitted);
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(947, 54);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 26);
			this.butRefresh.TabIndex = 82;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.UseVisualStyleBackColor = true;
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.textRangeMin);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.textRangeMax);
			this.groupBox2.Controls.Add(this.labelDaysOldMax);
			this.groupBox2.Controls.Add(this.labelDaysOldMin);
			this.groupBox2.Location = new System.Drawing.Point(19, 28);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(394, 40);
			this.groupBox2.TabIndex = 104;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Amount Range";
			// 
			// textRangeMin
			// 
			this.textRangeMin.CausesValidation = false;
			this.textRangeMin.Location = new System.Drawing.Point(48, 15);
			this.textRangeMin.MaxVal = 50000;
			this.textRangeMin.MinVal = 0;
			this.textRangeMin.Name = "textRangeMin";
			this.textRangeMin.Size = new System.Drawing.Size(60, 20);
			this.textRangeMin.TabIndex = 8;
			this.textRangeMin.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textRangeMin_KeyUp);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(212, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(162, 19);
			this.label3.TabIndex = 103;
			this.label3.Text = "(leave both blank to show all)";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textRangeMax
			// 
			this.textRangeMax.CausesValidation = false;
			this.textRangeMax.Location = new System.Drawing.Point(149, 15);
			this.textRangeMax.MaxVal = 50000;
			this.textRangeMax.MinVal = 0;
			this.textRangeMax.Name = "textRangeMax";
			this.textRangeMax.Size = new System.Drawing.Size(60, 20);
			this.textRangeMax.TabIndex = 9;
			this.textRangeMax.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textRangeMax_KeyUp);
			// 
			// labelDaysOldMax
			// 
			this.labelDaysOldMax.Location = new System.Drawing.Point(109, 16);
			this.labelDaysOldMax.Name = "labelDaysOldMax";
			this.labelDaysOldMax.Size = new System.Drawing.Size(40, 18);
			this.labelDaysOldMax.TabIndex = 99;
			this.labelDaysOldMax.Text = "Max";
			this.labelDaysOldMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelDaysOldMin
			// 
			this.labelDaysOldMin.Location = new System.Drawing.Point(3, 16);
			this.labelDaysOldMin.Name = "labelDaysOldMin";
			this.labelDaysOldMin.Size = new System.Drawing.Size(45, 18);
			this.labelDaysOldMin.TabIndex = 100;
			this.labelDaysOldMin.Text = "Min";
			this.labelDaysOldMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textCheckTrace
			// 
			this.textCheckTrace.AcceptsTab = true;
			this.textCheckTrace.BackColor = System.Drawing.SystemColors.Window;
			this.textCheckTrace.DetectLinksEnabled = false;
			this.textCheckTrace.DetectUrls = false;
			this.textCheckTrace.Location = new System.Drawing.Point(586, 29);
			this.textCheckTrace.Name = "textCheckTrace";
			this.textCheckTrace.QuickPasteType = OpenDentBusiness.QuickPasteType.FinancialNotes;
			this.textCheckTrace.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textCheckTrace.Size = new System.Drawing.Size(161, 21);
			this.textCheckTrace.TabIndex = 10;
			this.textCheckTrace.Text = "";
			this.textCheckTrace.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textCheckTrace_KeyUp);
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
			this.gridMain.Location = new System.Drawing.Point(19, 83);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(1003, 669);
			this.gridMain.TabIndex = 8;
			this.gridMain.Title = "ERAs";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableEtrans835s";
			this.gridMain.DoubleClick += new System.EventHandler(this.gridMain_DoubleClick);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(947, 757);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 6;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(535, 52);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(50, 16);
			this.labelClinic.TabIndex = 81;
			this.labelClinic.Text = "Clinics";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelClinic.Visible = false;
			// 
			// butDropTo
			// 
			this.butDropTo.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDropTo.Autosize = true;
			this.butDropTo.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDropTo.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDropTo.CornerRadius = 4F;
			this.butDropTo.ImageIndex = 0;
			this.butDropTo.ImageList = this.imageListCalendar;
			this.butDropTo.Location = new System.Drawing.Point(395, 7);
			this.butDropTo.Name = "butDropTo";
			this.butDropTo.Size = new System.Drawing.Size(17, 18);
			this.butDropTo.TabIndex = 92;
			this.butDropTo.UseVisualStyleBackColor = true;
			this.butDropTo.Click += new System.EventHandler(this.butDropTo_Click);
			// 
			// labelCheckTrace
			// 
			this.labelCheckTrace.Location = new System.Drawing.Point(425, 31);
			this.labelCheckTrace.Name = "labelCheckTrace";
			this.labelCheckTrace.Size = new System.Drawing.Size(161, 16);
			this.labelCheckTrace.TabIndex = 96;
			this.labelCheckTrace.Text = "Check# or EFT Trace#";
			this.labelCheckTrace.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelCarrier
			// 
			this.labelCarrier.Location = new System.Drawing.Point(531, 10);
			this.labelCarrier.Name = "labelCarrier";
			this.labelCarrier.Size = new System.Drawing.Size(55, 16);
			this.labelCarrier.TabIndex = 84;
			this.labelCarrier.Text = "Carrier";
			this.labelCarrier.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butDropFrom
			// 
			this.butDropFrom.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDropFrom.Autosize = true;
			this.butDropFrom.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDropFrom.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDropFrom.CornerRadius = 4F;
			this.butDropFrom.ImageIndex = 0;
			this.butDropFrom.ImageList = this.imageListCalendar;
			this.butDropFrom.Location = new System.Drawing.Point(168, 7);
			this.butDropFrom.Name = "butDropFrom";
			this.butDropFrom.Size = new System.Drawing.Size(17, 18);
			this.butDropFrom.TabIndex = 89;
			this.butDropFrom.UseVisualStyleBackColor = true;
			this.butDropFrom.Click += new System.EventHandler(this.butDropFrom_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(273, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(37, 16);
			this.label2.TabIndex = 90;
			this.label2.Text = "To";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(19, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(62, 16);
			this.label1.TabIndex = 87;
			this.label1.Text = "From";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelStatus
			// 
			this.labelStatus.Location = new System.Drawing.Point(755, 8);
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.Size = new System.Drawing.Size(49, 16);
			this.labelStatus.TabIndex = 86;
			this.labelStatus.Text = "Status";
			this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDateTo
			// 
			this.textDateTo.Location = new System.Drawing.Point(311, 6);
			this.textDateTo.Name = "textDateTo";
			this.textDateTo.Size = new System.Drawing.Size(102, 20);
			this.textDateTo.TabIndex = 4;
			// 
			// textDateFrom
			// 
			this.textDateFrom.Location = new System.Drawing.Point(84, 6);
			this.textDateFrom.Name = "textDateFrom";
			this.textDateFrom.Size = new System.Drawing.Size(102, 20);
			this.textDateFrom.TabIndex = 1;
			// 
			// butWeekPrevious
			// 
			this.butWeekPrevious.AdjustImageLocation = new System.Drawing.Point(-3, -1);
			this.butWeekPrevious.Autosize = true;
			this.butWeekPrevious.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butWeekPrevious.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butWeekPrevious.CornerRadius = 4F;
			this.butWeekPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butWeekPrevious.Image = ((System.Drawing.Image)(resources.GetObject("butWeekPrevious.Image")));
			this.butWeekPrevious.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butWeekPrevious.Location = new System.Drawing.Point(204, 6);
			this.butWeekPrevious.Name = "butWeekPrevious";
			this.butWeekPrevious.Size = new System.Drawing.Size(33, 22);
			this.butWeekPrevious.TabIndex = 108;
			this.butWeekPrevious.Text = "W";
			this.butWeekPrevious.Click += new System.EventHandler(this.butWeekPrevious_Click);
			// 
			// butWeekNext
			// 
			this.butWeekNext.AdjustImageLocation = new System.Drawing.Point(5, -1);
			this.butWeekNext.Autosize = false;
			this.butWeekNext.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butWeekNext.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butWeekNext.CornerRadius = 4F;
			this.butWeekNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butWeekNext.Image = ((System.Drawing.Image)(resources.GetObject("butWeekNext.Image")));
			this.butWeekNext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.butWeekNext.Location = new System.Drawing.Point(240, 6);
			this.butWeekNext.Name = "butWeekNext";
			this.butWeekNext.Size = new System.Drawing.Size(33, 22);
			this.butWeekNext.TabIndex = 107;
			this.butWeekNext.Text = "W";
			this.butWeekNext.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butWeekNext.Click += new System.EventHandler(this.butWeekNext_Click);
			// 
			// FormEtrans835s
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1034, 788);
			this.Controls.Add(this.listStatus);
			this.Controls.Add(this.calendarTo);
			this.Controls.Add(this.calendarFrom);
			this.Controls.Add(this.textCarrier);
			this.Controls.Add(this.comboClinics);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.textCheckTrace);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.butDropTo);
			this.Controls.Add(this.labelCheckTrace);
			this.Controls.Add(this.labelCarrier);
			this.Controls.Add(this.butDropFrom);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.labelStatus);
			this.Controls.Add(this.textDateTo);
			this.Controls.Add(this.textDateFrom);
			this.Controls.Add(this.butWeekPrevious);
			this.Controls.Add(this.butWeekNext);
			this.MinimumSize = new System.Drawing.Size(1050, 827);
			this.Name = "FormEtrans835s";
			this.Text = "Electronic EOBs - ERA 835s";
			this.Load += new System.EventHandler(this.FormEtrans835s_Load);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private UI.ODGrid gridMain;
		private UI.Button butClose;
		private UI.Button butRefresh;
		private System.Windows.Forms.MonthCalendar calendarTo;
		private System.Windows.Forms.MonthCalendar calendarFrom;
		private System.Windows.Forms.ImageList imageListCalendar;
		private System.Windows.Forms.Label labelCheckTrace;
		private ValidDate textDateFrom;
		private UI.Button butDropFrom;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labelStatus;
		private ValidDate textDateTo;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label labelCarrier;
		private UI.Button butDropTo;
		private System.Windows.Forms.Label labelClinic;
		private ODtextBox textCheckTrace;
		private System.Windows.Forms.GroupBox groupBox2;
		private ValidNum textRangeMin;
		private System.Windows.Forms.Label label3;
		private ValidNum textRangeMax;
		private System.Windows.Forms.Label labelDaysOldMax;
		private System.Windows.Forms.Label labelDaysOldMin;
		private UI.ComboBoxMulti comboClinics;
		private ODtextBox textCarrier;
		private CallFireService.SMSService smsService1;
		private System.Windows.Forms.ListBox listStatus;
		private UI.Button butWeekPrevious;
		private UI.Button butWeekNext;
	}
}