namespace OpenDental{
	partial class FormArManager {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormArManager));
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuItemGoTo = new System.Windows.Forms.ToolStripMenuItem();
			this.timerFillGrid = new System.Windows.Forms.Timer(this.components);
			this.butClose = new OpenDental.UI.Button();
			this.tabControlMain = new System.Windows.Forms.TabControl();
			this.tabUnsent = new System.Windows.Forms.TabPage();
			this.labelUnsentTotalNumAccts = new System.Windows.Forms.Label();
			this.textUnsentPayPlanDue = new OpenDental.ValidDouble();
			this.textUnsentPatient = new OpenDental.ValidDouble();
			this.textUnsentInsEst = new OpenDental.ValidDouble();
			this.textUnsentTotal = new OpenDental.ValidDouble();
			this.textUnsentOver90 = new OpenDental.ValidDouble();
			this.textUnsent31to60 = new OpenDental.ValidDouble();
			this.labelUnsentTotals = new System.Windows.Forms.Label();
			this.butRunAging = new OpenDental.UI.Button();
			this.groupPlaceAccounts = new System.Windows.Forms.GroupBox();
			this.butSend = new OpenDental.UI.Button();
			this.comboDemandType = new System.Windows.Forms.ComboBox();
			this.labelDemandType = new System.Windows.Forms.Label();
			this.butUnsentPrint = new OpenDental.UI.Button();
			this.butUnsentNone = new OpenDental.UI.Button();
			this.butUnsentAll = new OpenDental.UI.Button();
			this.gridUnsent = new OpenDental.UI.ODGrid();
			this.groupUnsentFilters = new System.Windows.Forms.GroupBox();
			this.comboUnsentAccountAge = new System.Windows.Forms.ComboBox();
			this.comboBoxMultiUnsentProvs = new OpenDental.UI.ComboBoxMulti();
			this.butUnsentSaveDefault = new OpenDental.UI.Button();
			this.checkExcludeBadAddress = new System.Windows.Forms.CheckBox();
			this.checkExcludeInsPending = new System.Windows.Forms.CheckBox();
			this.checkExcludeIfProcs = new System.Windows.Forms.CheckBox();
			this.textUnsentDaysLastPay = new OpenDental.ValidNumber();
			this.labelUnsentDaysLastPay = new System.Windows.Forms.Label();
			this.labelUnsentBillTypes = new System.Windows.Forms.Label();
			this.labelUnsentProvs = new System.Windows.Forms.Label();
			this.labelUnsentAccountAge = new System.Windows.Forms.Label();
			this.textUnsentMinBal = new OpenDental.ValidDouble();
			this.labelUnsentMinBal = new System.Windows.Forms.Label();
			this.labelUnsentClinics = new System.Windows.Forms.Label();
			this.comboBoxMultiUnsentClinics = new OpenDental.UI.ComboBoxMulti();
			this.comboBoxMultiBillTypes = new OpenDental.UI.ComboBoxMulti();
			this.tabSent = new System.Windows.Forms.TabPage();
			this.groupSentFIlters = new System.Windows.Forms.GroupBox();
			this.comboBoxMultiSentClinics = new OpenDental.UI.ComboBoxMulti();
			this.comboBoxMultiLastTransType = new OpenDental.UI.ComboBoxMulti();
			this.butSentSaveDefaults = new OpenDental.UI.Button();
			this.textSentDaysLastPay = new OpenDental.ValidNumber();
			this.labelSentDaysLastPay = new System.Windows.Forms.Label();
			this.labelLastTransType = new System.Windows.Forms.Label();
			this.labelSentProvs = new System.Windows.Forms.Label();
			this.labelSentAccountAge = new System.Windows.Forms.Label();
			this.textSentMinBal = new OpenDental.ValidDouble();
			this.labelSentMinBal = new System.Windows.Forms.Label();
			this.labelSentClinics = new System.Windows.Forms.Label();
			this.comboSentAccountAge = new System.Windows.Forms.ComboBox();
			this.comboBoxMultiSentProvs = new OpenDental.UI.ComboBoxMulti();
			this.butSentPrint = new OpenDental.UI.Button();
			this.butSentNone = new OpenDental.UI.Button();
			this.butSentAll = new OpenDental.UI.Button();
			this.groupUpdateAccounts = new System.Windows.Forms.GroupBox();
			this.labelNewBillType = new System.Windows.Forms.Label();
			this.comboNewBillType = new System.Windows.Forms.ComboBox();
			this.butUpdateStatus = new OpenDental.UI.Button();
			this.labelNewStatus = new System.Windows.Forms.Label();
			this.comboNewStatus = new System.Windows.Forms.ComboBox();
			this.gridSent = new OpenDental.UI.ODGrid();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
			this.butTsiOcp = new OpenDental.UI.Button();
			this.labelTsiOcp = new System.Windows.Forms.Label();
			this.textUnsentTotalNumAccts = new OpenDental.ValidNumber();
			this.textUnsent61to90 = new OpenDental.ValidDouble();
			this.textUnsent0to30 = new OpenDental.ValidDouble();
			this.textSentTotalNumAccts = new OpenDental.ValidNumber();
			this.textSent61to90 = new OpenDental.ValidDouble();
			this.textSent0to30 = new OpenDental.ValidDouble();
			this.labelSentTotalNumAccts = new System.Windows.Forms.Label();
			this.textSentPayPlanDue = new OpenDental.ValidDouble();
			this.textSentPatient = new OpenDental.ValidDouble();
			this.textSentInsEst = new OpenDental.ValidDouble();
			this.textSentTotal = new OpenDental.ValidDouble();
			this.textSentOver90 = new OpenDental.ValidDouble();
			this.textSent31to60 = new OpenDental.ValidDouble();
			this.labelSentTotals = new System.Windows.Forms.Label();
			this.contextMenu.SuspendLayout();
			this.tabControlMain.SuspendLayout();
			this.tabUnsent.SuspendLayout();
			this.groupPlaceAccounts.SuspendLayout();
			this.groupUnsentFilters.SuspendLayout();
			this.tabSent.SuspendLayout();
			this.groupSentFIlters.SuspendLayout();
			this.groupUpdateAccounts.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
			this.SuspendLayout();
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemGoTo});
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(106, 26);
			// 
			// menuItemGoTo
			// 
			this.menuItemGoTo.Name = "menuItemGoTo";
			this.menuItemGoTo.Size = new System.Drawing.Size(105, 22);
			this.menuItemGoTo.Text = "Go To";
			this.menuItemGoTo.Click += new System.EventHandler(this.menuItemGoTo_Click);
			// 
			// timerFillGrid
			// 
			this.timerFillGrid.Interval = 1000;
			this.timerFillGrid.Tick += new System.EventHandler(this.timerFillGrid_Tick);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(1143, 660);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 3;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// tabControlMain
			// 
			this.tabControlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControlMain.Controls.Add(this.tabUnsent);
			this.tabControlMain.Controls.Add(this.tabSent);
			this.tabControlMain.Location = new System.Drawing.Point(12, 12);
			this.tabControlMain.Name = "tabControlMain";
			this.tabControlMain.SelectedIndex = 0;
			this.tabControlMain.Size = new System.Drawing.Size(1206, 642);
			this.tabControlMain.TabIndex = 1;
			this.tabControlMain.SelectedIndexChanged += new System.EventHandler(this.tabControlMain_SelectedIndexChanged);
			// 
			// tabUnsent
			// 
			this.tabUnsent.BackColor = System.Drawing.SystemColors.Control;
			this.tabUnsent.Controls.Add(this.textUnsentTotalNumAccts);
			this.tabUnsent.Controls.Add(this.textUnsent61to90);
			this.tabUnsent.Controls.Add(this.textUnsent0to30);
			this.tabUnsent.Controls.Add(this.labelUnsentTotalNumAccts);
			this.tabUnsent.Controls.Add(this.textUnsentPayPlanDue);
			this.tabUnsent.Controls.Add(this.textUnsentPatient);
			this.tabUnsent.Controls.Add(this.textUnsentInsEst);
			this.tabUnsent.Controls.Add(this.textUnsentTotal);
			this.tabUnsent.Controls.Add(this.textUnsentOver90);
			this.tabUnsent.Controls.Add(this.textUnsent31to60);
			this.tabUnsent.Controls.Add(this.labelUnsentTotals);
			this.tabUnsent.Controls.Add(this.butRunAging);
			this.tabUnsent.Controls.Add(this.groupPlaceAccounts);
			this.tabUnsent.Controls.Add(this.butUnsentPrint);
			this.tabUnsent.Controls.Add(this.butUnsentNone);
			this.tabUnsent.Controls.Add(this.butUnsentAll);
			this.tabUnsent.Controls.Add(this.gridUnsent);
			this.tabUnsent.Controls.Add(this.groupUnsentFilters);
			this.tabUnsent.Location = new System.Drawing.Point(4, 22);
			this.tabUnsent.Name = "tabUnsent";
			this.tabUnsent.Size = new System.Drawing.Size(1198, 616);
			this.tabUnsent.TabIndex = 0;
			this.tabUnsent.Text = "Unsent Accounts";
			// 
			// labelUnsentTotalNumAccts
			// 
			this.labelUnsentTotalNumAccts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelUnsentTotalNumAccts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelUnsentTotalNumAccts.Location = new System.Drawing.Point(3, 541);
			this.labelUnsentTotalNumAccts.Name = "labelUnsentTotalNumAccts";
			this.labelUnsentTotalNumAccts.Size = new System.Drawing.Size(116, 17);
			this.labelUnsentTotalNumAccts.TabIndex = 123;
			this.labelUnsentTotalNumAccts.Text = "Guarantor Count";
			this.labelUnsentTotalNumAccts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textUnsentPayPlanDue
			// 
			this.textUnsentPayPlanDue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textUnsentPayPlanDue.Location = new System.Drawing.Point(873, 539);
			this.textUnsentPayPlanDue.MaxVal = 100000000D;
			this.textUnsentPayPlanDue.MinVal = -100000000D;
			this.textUnsentPayPlanDue.Name = "textUnsentPayPlanDue";
			this.textUnsentPayPlanDue.ReadOnly = true;
			this.textUnsentPayPlanDue.Size = new System.Drawing.Size(80, 20);
			this.textUnsentPayPlanDue.TabIndex = 122;
			this.textUnsentPayPlanDue.TabStop = false;
			this.textUnsentPayPlanDue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textUnsentPatient
			// 
			this.textUnsentPatient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textUnsentPatient.Location = new System.Drawing.Point(809, 539);
			this.textUnsentPatient.MaxVal = 100000000D;
			this.textUnsentPatient.MinVal = -100000000D;
			this.textUnsentPatient.Name = "textUnsentPatient";
			this.textUnsentPatient.ReadOnly = true;
			this.textUnsentPatient.Size = new System.Drawing.Size(65, 20);
			this.textUnsentPatient.TabIndex = 121;
			this.textUnsentPatient.TabStop = false;
			this.textUnsentPatient.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textUnsentInsEst
			// 
			this.textUnsentInsEst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textUnsentInsEst.Location = new System.Drawing.Point(745, 539);
			this.textUnsentInsEst.MaxVal = 100000000D;
			this.textUnsentInsEst.MinVal = -100000000D;
			this.textUnsentInsEst.Name = "textUnsentInsEst";
			this.textUnsentInsEst.ReadOnly = true;
			this.textUnsentInsEst.Size = new System.Drawing.Size(65, 20);
			this.textUnsentInsEst.TabIndex = 120;
			this.textUnsentInsEst.TabStop = false;
			this.textUnsentInsEst.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textUnsentTotal
			// 
			this.textUnsentTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textUnsentTotal.Location = new System.Drawing.Point(686, 539);
			this.textUnsentTotal.MaxVal = 100000000D;
			this.textUnsentTotal.MinVal = -100000000D;
			this.textUnsentTotal.Name = "textUnsentTotal";
			this.textUnsentTotal.ReadOnly = true;
			this.textUnsentTotal.Size = new System.Drawing.Size(60, 20);
			this.textUnsentTotal.TabIndex = 119;
			this.textUnsentTotal.TabStop = false;
			this.textUnsentTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textUnsentOver90
			// 
			this.textUnsentOver90.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textUnsentOver90.Location = new System.Drawing.Point(614, 539);
			this.textUnsentOver90.MaxVal = 100000000D;
			this.textUnsentOver90.MinVal = -100000000D;
			this.textUnsentOver90.Name = "textUnsentOver90";
			this.textUnsentOver90.ReadOnly = true;
			this.textUnsentOver90.Size = new System.Drawing.Size(73, 20);
			this.textUnsentOver90.TabIndex = 115;
			this.textUnsentOver90.TabStop = false;
			this.textUnsentOver90.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textUnsent31to60
			// 
			this.textUnsent31to60.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textUnsent31to60.Location = new System.Drawing.Point(466, 539);
			this.textUnsent31to60.MaxVal = 100000000D;
			this.textUnsent31to60.MinVal = -100000000D;
			this.textUnsent31to60.Name = "textUnsent31to60";
			this.textUnsent31to60.ReadOnly = true;
			this.textUnsent31to60.Size = new System.Drawing.Size(75, 20);
			this.textUnsent31to60.TabIndex = 114;
			this.textUnsent31to60.TabStop = false;
			this.textUnsent31to60.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelUnsentTotals
			// 
			this.labelUnsentTotals.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelUnsentTotals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelUnsentTotals.Location = new System.Drawing.Point(276, 541);
			this.labelUnsentTotals.Name = "labelUnsentTotals";
			this.labelUnsentTotals.Size = new System.Drawing.Size(116, 17);
			this.labelUnsentTotals.TabIndex = 117;
			this.labelUnsentTotals.Text = "Totals";
			this.labelUnsentTotals.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butRunAging
			// 
			this.butRunAging.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRunAging.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butRunAging.Autosize = true;
			this.butRunAging.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRunAging.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRunAging.CornerRadius = 4F;
			this.butRunAging.Location = new System.Drawing.Point(1117, 586);
			this.butRunAging.Name = "butRunAging";
			this.butRunAging.Size = new System.Drawing.Size(75, 24);
			this.butRunAging.TabIndex = 7;
			this.butRunAging.Text = "Run Aging";
			this.butRunAging.Click += new System.EventHandler(this.butRunAging_Click);
			// 
			// groupPlaceAccounts
			// 
			this.groupPlaceAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupPlaceAccounts.Controls.Add(this.butSend);
			this.groupPlaceAccounts.Controls.Add(this.comboDemandType);
			this.groupPlaceAccounts.Controls.Add(this.labelDemandType);
			this.groupPlaceAccounts.Location = new System.Drawing.Point(426, 565);
			this.groupPlaceAccounts.Name = "groupPlaceAccounts";
			this.groupPlaceAccounts.Size = new System.Drawing.Size(347, 45);
			this.groupPlaceAccounts.TabIndex = 5;
			this.groupPlaceAccounts.TabStop = false;
			this.groupPlaceAccounts.Text = "Account Placement";
			// 
			// butSend
			// 
			this.butSend.AdjustImageLocation = new System.Drawing.Point(-4, 0);
			this.butSend.Autosize = false;
			this.butSend.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSend.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSend.CornerRadius = 4F;
			this.butSend.Image = ((System.Drawing.Image)(resources.GetObject("butSend.Image")));
			this.butSend.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSend.Location = new System.Drawing.Point(253, 14);
			this.butSend.Name = "butSend";
			this.butSend.Size = new System.Drawing.Size(88, 25);
			this.butSend.TabIndex = 2;
			this.butSend.Text = "Send to TSI";
			this.butSend.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSend.Click += new System.EventHandler(this.butSend_Click);
			// 
			// comboDemandType
			// 
			this.comboDemandType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDemandType.Location = new System.Drawing.Point(87, 16);
			this.comboDemandType.Name = "comboDemandType";
			this.comboDemandType.Size = new System.Drawing.Size(160, 21);
			this.comboDemandType.TabIndex = 1;
			// 
			// labelDemandType
			// 
			this.labelDemandType.Location = new System.Drawing.Point(6, 18);
			this.labelDemandType.Name = "labelDemandType";
			this.labelDemandType.Size = new System.Drawing.Size(80, 17);
			this.labelDemandType.TabIndex = 0;
			this.labelDemandType.Text = "Demand Type";
			this.labelDemandType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butUnsentPrint
			// 
			this.butUnsentPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUnsentPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butUnsentPrint.Autosize = true;
			this.butUnsentPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUnsentPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUnsentPrint.CornerRadius = 4F;
			this.butUnsentPrint.Image = global::OpenDental.Properties.Resources.butPrint;
			this.butUnsentPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUnsentPrint.Location = new System.Drawing.Point(1036, 586);
			this.butUnsentPrint.Name = "butUnsentPrint";
			this.butUnsentPrint.Size = new System.Drawing.Size(75, 24);
			this.butUnsentPrint.TabIndex = 6;
			this.butUnsentPrint.Text = "Print";
			this.butUnsentPrint.Visible = false;
			this.butUnsentPrint.Click += new System.EventHandler(this.butUnsentPrint_Click);
			// 
			// butUnsentNone
			// 
			this.butUnsentNone.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUnsentNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butUnsentNone.Autosize = true;
			this.butUnsentNone.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUnsentNone.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUnsentNone.CornerRadius = 4F;
			this.butUnsentNone.Location = new System.Drawing.Point(87, 586);
			this.butUnsentNone.Name = "butUnsentNone";
			this.butUnsentNone.Size = new System.Drawing.Size(75, 24);
			this.butUnsentNone.TabIndex = 4;
			this.butUnsentNone.Text = "None";
			this.butUnsentNone.Click += new System.EventHandler(this.butUnsentNone_Click);
			// 
			// butUnsentAll
			// 
			this.butUnsentAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUnsentAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butUnsentAll.Autosize = true;
			this.butUnsentAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUnsentAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUnsentAll.CornerRadius = 4F;
			this.butUnsentAll.Location = new System.Drawing.Point(6, 586);
			this.butUnsentAll.Name = "butUnsentAll";
			this.butUnsentAll.Size = new System.Drawing.Size(75, 24);
			this.butUnsentAll.TabIndex = 3;
			this.butUnsentAll.Text = "All";
			this.butUnsentAll.Click += new System.EventHandler(this.butUnsentAll_Click);
			// 
			// gridUnsent
			// 
			this.gridUnsent.AllowSortingByColumn = true;
			this.gridUnsent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridUnsent.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridUnsent.ContextMenuStrip = this.contextMenu;
			this.gridUnsent.HasAddButton = false;
			this.gridUnsent.HasDropDowns = false;
			this.gridUnsent.HasMultilineHeaders = false;
			this.gridUnsent.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridUnsent.HeaderHeight = 15;
			this.gridUnsent.HScrollVisible = false;
			this.gridUnsent.Location = new System.Drawing.Point(0, 78);
			this.gridUnsent.Name = "gridUnsent";
			this.gridUnsent.ScrollValue = 0;
			this.gridUnsent.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridUnsent.Size = new System.Drawing.Size(1198, 457);
			this.gridUnsent.TabIndex = 2;
			this.gridUnsent.Title = "Guarantors - Not Sent to TSI";
			this.gridUnsent.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridUnsent.TitleHeight = 18;
			this.gridUnsent.TranslationName = "TableNotSent";
			this.gridUnsent.OnSortByColumn += new System.EventHandler(this.gridUnsentMain_OnSortByColumn);
			this.gridUnsent.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridUnsentMain_MouseDown);
			this.gridUnsent.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gridUnsent_MouseMove);
			// 
			// groupUnsentFilters
			// 
			this.groupUnsentFilters.Controls.Add(this.comboUnsentAccountAge);
			this.groupUnsentFilters.Controls.Add(this.comboBoxMultiUnsentProvs);
			this.groupUnsentFilters.Controls.Add(this.butUnsentSaveDefault);
			this.groupUnsentFilters.Controls.Add(this.checkExcludeBadAddress);
			this.groupUnsentFilters.Controls.Add(this.checkExcludeInsPending);
			this.groupUnsentFilters.Controls.Add(this.checkExcludeIfProcs);
			this.groupUnsentFilters.Controls.Add(this.textUnsentDaysLastPay);
			this.groupUnsentFilters.Controls.Add(this.labelUnsentDaysLastPay);
			this.groupUnsentFilters.Controls.Add(this.labelUnsentBillTypes);
			this.groupUnsentFilters.Controls.Add(this.labelUnsentProvs);
			this.groupUnsentFilters.Controls.Add(this.labelUnsentAccountAge);
			this.groupUnsentFilters.Controls.Add(this.textUnsentMinBal);
			this.groupUnsentFilters.Controls.Add(this.labelUnsentMinBal);
			this.groupUnsentFilters.Controls.Add(this.labelUnsentClinics);
			this.groupUnsentFilters.Controls.Add(this.comboBoxMultiUnsentClinics);
			this.groupUnsentFilters.Controls.Add(this.comboBoxMultiBillTypes);
			this.groupUnsentFilters.Location = new System.Drawing.Point(6, 6);
			this.groupUnsentFilters.Name = "groupUnsentFilters";
			this.groupUnsentFilters.Size = new System.Drawing.Size(1186, 66);
			this.groupUnsentFilters.TabIndex = 1;
			this.groupUnsentFilters.TabStop = false;
			this.groupUnsentFilters.Text = "Account Filters";
			// 
			// comboUnsentAccountAge
			// 
			this.comboUnsentAccountAge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboUnsentAccountAge.FormattingEnabled = true;
			this.comboUnsentAccountAge.Location = new System.Drawing.Point(372, 38);
			this.comboUnsentAccountAge.Name = "comboUnsentAccountAge";
			this.comboUnsentAccountAge.Size = new System.Drawing.Size(160, 21);
			this.comboUnsentAccountAge.TabIndex = 4;
			this.comboUnsentAccountAge.SelectedIndexChanged += new System.EventHandler(this.comboUnsentAccountAge_SelectedIndexChanged);
			// 
			// comboBoxMultiUnsentProvs
			// 
			this.comboBoxMultiUnsentProvs.ArraySelectedIndices = new int[0];
			this.comboBoxMultiUnsentProvs.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiUnsentProvs.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiUnsentProvs.Items")));
			this.comboBoxMultiUnsentProvs.Location = new System.Drawing.Point(97, 38);
			this.comboBoxMultiUnsentProvs.Name = "comboBoxMultiUnsentProvs";
			this.comboBoxMultiUnsentProvs.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiUnsentProvs.SelectedIndices")));
			this.comboBoxMultiUnsentProvs.Size = new System.Drawing.Size(160, 21);
			this.comboBoxMultiUnsentProvs.TabIndex = 2;
			this.comboBoxMultiUnsentProvs.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.comboBoxMultiUnsentProvs_SelectionChangeCommitted);
			this.comboBoxMultiUnsentProvs.Leave += new System.EventHandler(this.comboBoxMultiUnsentProvs_Leave);
			// 
			// butUnsentSaveDefault
			// 
			this.butUnsentSaveDefault.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUnsentSaveDefault.Autosize = true;
			this.butUnsentSaveDefault.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUnsentSaveDefault.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUnsentSaveDefault.CornerRadius = 4F;
			this.butUnsentSaveDefault.Location = new System.Drawing.Point(1082, 36);
			this.butUnsentSaveDefault.Name = "butUnsentSaveDefault";
			this.butUnsentSaveDefault.Size = new System.Drawing.Size(98, 24);
			this.butUnsentSaveDefault.TabIndex = 10;
			this.butUnsentSaveDefault.Text = "&Save As Default";
			this.butUnsentSaveDefault.Click += new System.EventHandler(this.butUnsentSaveDefault_Click);
			// 
			// checkExcludeBadAddress
			// 
			this.checkExcludeBadAddress.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkExcludeBadAddress.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkExcludeBadAddress.Location = new System.Drawing.Point(962, 18);
			this.checkExcludeBadAddress.Name = "checkExcludeBadAddress";
			this.checkExcludeBadAddress.Size = new System.Drawing.Size(218, 16);
			this.checkExcludeBadAddress.TabIndex = 9;
			this.checkExcludeBadAddress.Text = "Exclude bad addresses (missing data)";
			this.checkExcludeBadAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkExcludeBadAddress.CheckedChanged += new System.EventHandler(this.checkExcludeBadAddress_CheckedChanged);
			// 
			// checkExcludeInsPending
			// 
			this.checkExcludeInsPending.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkExcludeInsPending.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkExcludeInsPending.Location = new System.Drawing.Point(773, 18);
			this.checkExcludeInsPending.Name = "checkExcludeInsPending";
			this.checkExcludeInsPending.Size = new System.Drawing.Size(183, 16);
			this.checkExcludeInsPending.TabIndex = 7;
			this.checkExcludeInsPending.Text = "Exclude if insurance pending";
			this.checkExcludeInsPending.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkExcludeInsPending.CheckedChanged += new System.EventHandler(this.checkExcludeInsPending_CheckedChanged);
			// 
			// checkExcludeIfProcs
			// 
			this.checkExcludeIfProcs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkExcludeIfProcs.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkExcludeIfProcs.Location = new System.Drawing.Point(773, 40);
			this.checkExcludeIfProcs.Name = "checkExcludeIfProcs";
			this.checkExcludeIfProcs.Size = new System.Drawing.Size(183, 16);
			this.checkExcludeIfProcs.TabIndex = 8;
			this.checkExcludeIfProcs.Text = "Exclude if unsent procedures";
			this.checkExcludeIfProcs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkExcludeIfProcs.CheckedChanged += new System.EventHandler(this.checkExcludeIfProcs_CheckedChanged);
			// 
			// textUnsentDaysLastPay
			// 
			this.textUnsentDaysLastPay.Location = new System.Drawing.Point(692, 38);
			this.textUnsentDaysLastPay.MaxVal = 99999;
			this.textUnsentDaysLastPay.MinVal = 0;
			this.textUnsentDaysLastPay.Name = "textUnsentDaysLastPay";
			this.textUnsentDaysLastPay.Size = new System.Drawing.Size(75, 20);
			this.textUnsentDaysLastPay.TabIndex = 6;
			this.textUnsentDaysLastPay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textUnsentDaysLastPay.TextChanged += new System.EventHandler(this.textUnsentDaysLastPay_TextChanged);
			// 
			// labelUnsentDaysLastPay
			// 
			this.labelUnsentDaysLastPay.Location = new System.Drawing.Point(538, 40);
			this.labelUnsentDaysLastPay.Name = "labelUnsentDaysLastPay";
			this.labelUnsentDaysLastPay.Size = new System.Drawing.Size(153, 17);
			this.labelUnsentDaysLastPay.TabIndex = 0;
			this.labelUnsentDaysLastPay.Text = "Days Since Last Payment";
			this.labelUnsentDaysLastPay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelUnsentBillTypes
			// 
			this.labelUnsentBillTypes.Location = new System.Drawing.Point(263, 18);
			this.labelUnsentBillTypes.Name = "labelUnsentBillTypes";
			this.labelUnsentBillTypes.Size = new System.Drawing.Size(108, 17);
			this.labelUnsentBillTypes.TabIndex = 0;
			this.labelUnsentBillTypes.Text = "Billing Types";
			this.labelUnsentBillTypes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelUnsentProvs
			// 
			this.labelUnsentProvs.Location = new System.Drawing.Point(6, 40);
			this.labelUnsentProvs.Name = "labelUnsentProvs";
			this.labelUnsentProvs.Size = new System.Drawing.Size(90, 17);
			this.labelUnsentProvs.TabIndex = 0;
			this.labelUnsentProvs.Text = "Providers";
			this.labelUnsentProvs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelUnsentAccountAge
			// 
			this.labelUnsentAccountAge.Location = new System.Drawing.Point(263, 40);
			this.labelUnsentAccountAge.Name = "labelUnsentAccountAge";
			this.labelUnsentAccountAge.Size = new System.Drawing.Size(108, 17);
			this.labelUnsentAccountAge.TabIndex = 0;
			this.labelUnsentAccountAge.Text = "Account Age";
			this.labelUnsentAccountAge.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textUnsentMinBal
			// 
			this.textUnsentMinBal.Location = new System.Drawing.Point(692, 16);
			this.textUnsentMinBal.MaxVal = 999999D;
			this.textUnsentMinBal.MinVal = -999999D;
			this.textUnsentMinBal.Name = "textUnsentMinBal";
			this.textUnsentMinBal.Size = new System.Drawing.Size(75, 20);
			this.textUnsentMinBal.TabIndex = 5;
			this.textUnsentMinBal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textUnsentMinBal.TextChanged += new System.EventHandler(this.textUnsentMinBal_TextChanged);
			// 
			// labelUnsentMinBal
			// 
			this.labelUnsentMinBal.Location = new System.Drawing.Point(538, 18);
			this.labelUnsentMinBal.Name = "labelUnsentMinBal";
			this.labelUnsentMinBal.Size = new System.Drawing.Size(153, 17);
			this.labelUnsentMinBal.TabIndex = 0;
			this.labelUnsentMinBal.Text = "Minimum Balance";
			this.labelUnsentMinBal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelUnsentClinics
			// 
			this.labelUnsentClinics.Location = new System.Drawing.Point(6, 18);
			this.labelUnsentClinics.Name = "labelUnsentClinics";
			this.labelUnsentClinics.Size = new System.Drawing.Size(90, 17);
			this.labelUnsentClinics.TabIndex = 0;
			this.labelUnsentClinics.Text = "Clinics";
			this.labelUnsentClinics.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelUnsentClinics.Visible = false;
			// 
			// comboBoxMultiUnsentClinics
			// 
			this.comboBoxMultiUnsentClinics.ArraySelectedIndices = new int[0];
			this.comboBoxMultiUnsentClinics.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiUnsentClinics.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiUnsentClinics.Items")));
			this.comboBoxMultiUnsentClinics.Location = new System.Drawing.Point(97, 16);
			this.comboBoxMultiUnsentClinics.Name = "comboBoxMultiUnsentClinics";
			this.comboBoxMultiUnsentClinics.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiUnsentClinics.SelectedIndices")));
			this.comboBoxMultiUnsentClinics.Size = new System.Drawing.Size(160, 21);
			this.comboBoxMultiUnsentClinics.TabIndex = 1;
			this.comboBoxMultiUnsentClinics.Visible = false;
			this.comboBoxMultiUnsentClinics.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.comboBoxMultiUnsentClinics_SelectionChangeCommitted);
			this.comboBoxMultiUnsentClinics.Leave += new System.EventHandler(this.comboBoxMultiUnsentClinics_Leave);
			// 
			// comboBoxMultiBillTypes
			// 
			this.comboBoxMultiBillTypes.ArraySelectedIndices = new int[0];
			this.comboBoxMultiBillTypes.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiBillTypes.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiBillTypes.Items")));
			this.comboBoxMultiBillTypes.Location = new System.Drawing.Point(372, 16);
			this.comboBoxMultiBillTypes.Name = "comboBoxMultiBillTypes";
			this.comboBoxMultiBillTypes.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiBillTypes.SelectedIndices")));
			this.comboBoxMultiBillTypes.Size = new System.Drawing.Size(160, 21);
			this.comboBoxMultiBillTypes.TabIndex = 3;
			this.comboBoxMultiBillTypes.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.comboBoxMultiUnsentBillTypes_SelectionChangeCommitted);
			this.comboBoxMultiBillTypes.Leave += new System.EventHandler(this.comboBoxMultiBillTypes_Leave);
			// 
			// tabSent
			// 
			this.tabSent.BackColor = System.Drawing.SystemColors.Control;
			this.tabSent.Controls.Add(this.textSentTotalNumAccts);
			this.tabSent.Controls.Add(this.textSent61to90);
			this.tabSent.Controls.Add(this.textSent0to30);
			this.tabSent.Controls.Add(this.labelSentTotalNumAccts);
			this.tabSent.Controls.Add(this.textSentPayPlanDue);
			this.tabSent.Controls.Add(this.textSentPatient);
			this.tabSent.Controls.Add(this.textSentInsEst);
			this.tabSent.Controls.Add(this.textSentTotal);
			this.tabSent.Controls.Add(this.textSentOver90);
			this.tabSent.Controls.Add(this.textSent31to60);
			this.tabSent.Controls.Add(this.labelSentTotals);
			this.tabSent.Controls.Add(this.groupSentFIlters);
			this.tabSent.Controls.Add(this.butSentPrint);
			this.tabSent.Controls.Add(this.butSentNone);
			this.tabSent.Controls.Add(this.butSentAll);
			this.tabSent.Controls.Add(this.groupUpdateAccounts);
			this.tabSent.Controls.Add(this.gridSent);
			this.tabSent.Location = new System.Drawing.Point(4, 22);
			this.tabSent.Name = "tabSent";
			this.tabSent.Padding = new System.Windows.Forms.Padding(3);
			this.tabSent.Size = new System.Drawing.Size(1198, 616);
			this.tabSent.TabIndex = 1;
			this.tabSent.Text = "Sent Accounts";
			// 
			// groupSentFIlters
			// 
			this.groupSentFIlters.Controls.Add(this.comboBoxMultiSentClinics);
			this.groupSentFIlters.Controls.Add(this.comboBoxMultiLastTransType);
			this.groupSentFIlters.Controls.Add(this.butSentSaveDefaults);
			this.groupSentFIlters.Controls.Add(this.textSentDaysLastPay);
			this.groupSentFIlters.Controls.Add(this.labelSentDaysLastPay);
			this.groupSentFIlters.Controls.Add(this.labelLastTransType);
			this.groupSentFIlters.Controls.Add(this.labelSentProvs);
			this.groupSentFIlters.Controls.Add(this.labelSentAccountAge);
			this.groupSentFIlters.Controls.Add(this.textSentMinBal);
			this.groupSentFIlters.Controls.Add(this.labelSentMinBal);
			this.groupSentFIlters.Controls.Add(this.labelSentClinics);
			this.groupSentFIlters.Controls.Add(this.comboSentAccountAge);
			this.groupSentFIlters.Controls.Add(this.comboBoxMultiSentProvs);
			this.groupSentFIlters.Location = new System.Drawing.Point(6, 6);
			this.groupSentFIlters.Name = "groupSentFIlters";
			this.groupSentFIlters.Size = new System.Drawing.Size(1186, 66);
			this.groupSentFIlters.TabIndex = 1;
			this.groupSentFIlters.TabStop = false;
			this.groupSentFIlters.Text = "Account Filters";
			// 
			// comboBoxMultiSentClinics
			// 
			this.comboBoxMultiSentClinics.ArraySelectedIndices = new int[0];
			this.comboBoxMultiSentClinics.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiSentClinics.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiSentClinics.Items")));
			this.comboBoxMultiSentClinics.Location = new System.Drawing.Point(97, 16);
			this.comboBoxMultiSentClinics.Name = "comboBoxMultiSentClinics";
			this.comboBoxMultiSentClinics.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiSentClinics.SelectedIndices")));
			this.comboBoxMultiSentClinics.Size = new System.Drawing.Size(160, 21);
			this.comboBoxMultiSentClinics.TabIndex = 1;
			this.comboBoxMultiSentClinics.Visible = false;
			this.comboBoxMultiSentClinics.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.comboBoxMultiSentClinics_SelectionChangeCommitted);
			this.comboBoxMultiSentClinics.Leave += new System.EventHandler(this.comboBoxMultiSentClinics_Leave);
			// 
			// comboBoxMultiLastTransType
			// 
			this.comboBoxMultiLastTransType.ArraySelectedIndices = new int[0];
			this.comboBoxMultiLastTransType.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiLastTransType.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiLastTransType.Items")));
			this.comboBoxMultiLastTransType.Location = new System.Drawing.Point(372, 16);
			this.comboBoxMultiLastTransType.Name = "comboBoxMultiLastTransType";
			this.comboBoxMultiLastTransType.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiLastTransType.SelectedIndices")));
			this.comboBoxMultiLastTransType.Size = new System.Drawing.Size(160, 21);
			this.comboBoxMultiLastTransType.TabIndex = 3;
			this.comboBoxMultiLastTransType.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.comboBoxMultiLastTransType_SelectionChangeCommitted);
			this.comboBoxMultiLastTransType.Leave += new System.EventHandler(this.comboBoxMultiLastTransType_Leave);
			// 
			// butSentSaveDefaults
			// 
			this.butSentSaveDefaults.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSentSaveDefaults.Autosize = true;
			this.butSentSaveDefaults.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSentSaveDefaults.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSentSaveDefaults.CornerRadius = 4F;
			this.butSentSaveDefaults.Location = new System.Drawing.Point(1082, 36);
			this.butSentSaveDefaults.Name = "butSentSaveDefaults";
			this.butSentSaveDefaults.Size = new System.Drawing.Size(98, 24);
			this.butSentSaveDefaults.TabIndex = 7;
			this.butSentSaveDefaults.Text = "&Save As Default";
			this.butSentSaveDefaults.Click += new System.EventHandler(this.butSentSaveDefaults_Click);
			// 
			// textSentDaysLastPay
			// 
			this.textSentDaysLastPay.Location = new System.Drawing.Point(692, 38);
			this.textSentDaysLastPay.MaxVal = 99999;
			this.textSentDaysLastPay.MinVal = 0;
			this.textSentDaysLastPay.Name = "textSentDaysLastPay";
			this.textSentDaysLastPay.Size = new System.Drawing.Size(75, 20);
			this.textSentDaysLastPay.TabIndex = 6;
			this.textSentDaysLastPay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textSentDaysLastPay.TextChanged += new System.EventHandler(this.textSentDaysLastPay_TextChanged);
			// 
			// labelSentDaysLastPay
			// 
			this.labelSentDaysLastPay.Location = new System.Drawing.Point(538, 40);
			this.labelSentDaysLastPay.Name = "labelSentDaysLastPay";
			this.labelSentDaysLastPay.Size = new System.Drawing.Size(153, 17);
			this.labelSentDaysLastPay.TabIndex = 0;
			this.labelSentDaysLastPay.Text = "Days Since Last Payment";
			this.labelSentDaysLastPay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelLastTransType
			// 
			this.labelLastTransType.Location = new System.Drawing.Point(263, 18);
			this.labelLastTransType.Name = "labelLastTransType";
			this.labelLastTransType.Size = new System.Drawing.Size(108, 17);
			this.labelLastTransType.TabIndex = 0;
			this.labelLastTransType.Text = "Last Trans Type";
			this.labelLastTransType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelSentProvs
			// 
			this.labelSentProvs.Location = new System.Drawing.Point(6, 40);
			this.labelSentProvs.Name = "labelSentProvs";
			this.labelSentProvs.Size = new System.Drawing.Size(90, 17);
			this.labelSentProvs.TabIndex = 0;
			this.labelSentProvs.Text = "Providers";
			this.labelSentProvs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelSentAccountAge
			// 
			this.labelSentAccountAge.Location = new System.Drawing.Point(263, 40);
			this.labelSentAccountAge.Name = "labelSentAccountAge";
			this.labelSentAccountAge.Size = new System.Drawing.Size(108, 17);
			this.labelSentAccountAge.TabIndex = 0;
			this.labelSentAccountAge.Text = "Account Age";
			this.labelSentAccountAge.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textSentMinBal
			// 
			this.textSentMinBal.Location = new System.Drawing.Point(692, 16);
			this.textSentMinBal.MaxVal = 999999D;
			this.textSentMinBal.MinVal = -999999D;
			this.textSentMinBal.Name = "textSentMinBal";
			this.textSentMinBal.Size = new System.Drawing.Size(75, 20);
			this.textSentMinBal.TabIndex = 5;
			this.textSentMinBal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textSentMinBal.TextChanged += new System.EventHandler(this.textSentMinBal_TextChanged);
			// 
			// labelSentMinBal
			// 
			this.labelSentMinBal.Location = new System.Drawing.Point(538, 18);
			this.labelSentMinBal.Name = "labelSentMinBal";
			this.labelSentMinBal.Size = new System.Drawing.Size(153, 17);
			this.labelSentMinBal.TabIndex = 0;
			this.labelSentMinBal.Text = "Minimum Balance";
			this.labelSentMinBal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelSentClinics
			// 
			this.labelSentClinics.Location = new System.Drawing.Point(6, 18);
			this.labelSentClinics.Name = "labelSentClinics";
			this.labelSentClinics.Size = new System.Drawing.Size(90, 17);
			this.labelSentClinics.TabIndex = 0;
			this.labelSentClinics.Text = "Clinics";
			this.labelSentClinics.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelSentClinics.Visible = false;
			// 
			// comboSentAccountAge
			// 
			this.comboSentAccountAge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSentAccountAge.FormattingEnabled = true;
			this.comboSentAccountAge.Location = new System.Drawing.Point(372, 38);
			this.comboSentAccountAge.Name = "comboSentAccountAge";
			this.comboSentAccountAge.Size = new System.Drawing.Size(160, 21);
			this.comboSentAccountAge.TabIndex = 4;
			this.comboSentAccountAge.SelectedIndexChanged += new System.EventHandler(this.comboSentAccountAge_SelectedIndexChanged);
			// 
			// comboBoxMultiSentProvs
			// 
			this.comboBoxMultiSentProvs.ArraySelectedIndices = new int[0];
			this.comboBoxMultiSentProvs.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiSentProvs.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiSentProvs.Items")));
			this.comboBoxMultiSentProvs.Location = new System.Drawing.Point(97, 38);
			this.comboBoxMultiSentProvs.Name = "comboBoxMultiSentProvs";
			this.comboBoxMultiSentProvs.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiSentProvs.SelectedIndices")));
			this.comboBoxMultiSentProvs.Size = new System.Drawing.Size(160, 21);
			this.comboBoxMultiSentProvs.TabIndex = 2;
			this.comboBoxMultiSentProvs.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.comboBoxMultiSentProvs_SelectionChangeCommitted);
			this.comboBoxMultiSentProvs.Leave += new System.EventHandler(this.comboBoxMultiSentProvs_Leave);
			// 
			// butSentPrint
			// 
			this.butSentPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSentPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSentPrint.Autosize = true;
			this.butSentPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSentPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSentPrint.CornerRadius = 4F;
			this.butSentPrint.Image = global::OpenDental.Properties.Resources.butPrint;
			this.butSentPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSentPrint.Location = new System.Drawing.Point(1117, 586);
			this.butSentPrint.Name = "butSentPrint";
			this.butSentPrint.Size = new System.Drawing.Size(75, 24);
			this.butSentPrint.TabIndex = 6;
			this.butSentPrint.Text = "Print";
			this.butSentPrint.Visible = false;
			this.butSentPrint.Click += new System.EventHandler(this.butSentPrint_Click);
			// 
			// butSentNone
			// 
			this.butSentNone.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSentNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSentNone.Autosize = true;
			this.butSentNone.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSentNone.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSentNone.CornerRadius = 4F;
			this.butSentNone.Location = new System.Drawing.Point(87, 586);
			this.butSentNone.Name = "butSentNone";
			this.butSentNone.Size = new System.Drawing.Size(75, 24);
			this.butSentNone.TabIndex = 4;
			this.butSentNone.Text = "None";
			this.butSentNone.Click += new System.EventHandler(this.butSentNone_Click);
			// 
			// butSentAll
			// 
			this.butSentAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSentAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butSentAll.Autosize = true;
			this.butSentAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSentAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSentAll.CornerRadius = 4F;
			this.butSentAll.Location = new System.Drawing.Point(6, 586);
			this.butSentAll.Name = "butSentAll";
			this.butSentAll.Size = new System.Drawing.Size(75, 24);
			this.butSentAll.TabIndex = 3;
			this.butSentAll.Text = "All";
			this.butSentAll.Click += new System.EventHandler(this.butSentAll_Click);
			// 
			// groupUpdateAccounts
			// 
			this.groupUpdateAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupUpdateAccounts.Controls.Add(this.labelNewBillType);
			this.groupUpdateAccounts.Controls.Add(this.comboNewBillType);
			this.groupUpdateAccounts.Controls.Add(this.butUpdateStatus);
			this.groupUpdateAccounts.Controls.Add(this.labelNewStatus);
			this.groupUpdateAccounts.Controls.Add(this.comboNewStatus);
			this.groupUpdateAccounts.Location = new System.Drawing.Point(340, 565);
			this.groupUpdateAccounts.Name = "groupUpdateAccounts";
			this.groupUpdateAccounts.Size = new System.Drawing.Size(518, 45);
			this.groupUpdateAccounts.TabIndex = 5;
			this.groupUpdateAccounts.TabStop = false;
			this.groupUpdateAccounts.Text = "Account Status Updates";
			// 
			// labelNewBillType
			// 
			this.labelNewBillType.Location = new System.Drawing.Point(192, 18);
			this.labelNewBillType.Name = "labelNewBillType";
			this.labelNewBillType.Size = new System.Drawing.Size(105, 17);
			this.labelNewBillType.TabIndex = 0;
			this.labelNewBillType.Text = "New Billing Type";
			this.labelNewBillType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboNewBillType
			// 
			this.comboNewBillType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboNewBillType.Location = new System.Drawing.Point(298, 16);
			this.comboNewBillType.Name = "comboNewBillType";
			this.comboNewBillType.Size = new System.Drawing.Size(120, 21);
			this.comboNewBillType.TabIndex = 2;
			this.comboNewBillType.SelectionChangeCommitted += new System.EventHandler(this.comboNewBillType_SelectionChangeCommitted);
			// 
			// butUpdateStatus
			// 
			this.butUpdateStatus.AdjustImageLocation = new System.Drawing.Point(-5, 0);
			this.butUpdateStatus.Autosize = true;
			this.butUpdateStatus.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUpdateStatus.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUpdateStatus.CornerRadius = 4F;
			this.butUpdateStatus.Image = ((System.Drawing.Image)(resources.GetObject("butUpdateStatus.Image")));
			this.butUpdateStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUpdateStatus.Location = new System.Drawing.Point(424, 14);
			this.butUpdateStatus.Name = "butUpdateStatus";
			this.butUpdateStatus.Size = new System.Drawing.Size(88, 25);
			this.butUpdateStatus.TabIndex = 3;
			this.butUpdateStatus.Text = "Update TSI";
			this.butUpdateStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUpdateStatus.Click += new System.EventHandler(this.butUpdateStatus_Click);
			// 
			// labelNewStatus
			// 
			this.labelNewStatus.Location = new System.Drawing.Point(6, 18);
			this.labelNewStatus.Name = "labelNewStatus";
			this.labelNewStatus.Size = new System.Drawing.Size(80, 17);
			this.labelNewStatus.TabIndex = 0;
			this.labelNewStatus.Text = "New Status";
			this.labelNewStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboNewStatus
			// 
			this.comboNewStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboNewStatus.FormattingEnabled = true;
			this.comboNewStatus.Location = new System.Drawing.Point(87, 16);
			this.comboNewStatus.Name = "comboNewStatus";
			this.comboNewStatus.Size = new System.Drawing.Size(99, 21);
			this.comboNewStatus.TabIndex = 1;
			this.comboNewStatus.SelectedIndexChanged += new System.EventHandler(this.comboNewStatus_SelectedIndexChanged);
			// 
			// gridSent
			// 
			this.gridSent.AllowSortingByColumn = true;
			this.gridSent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridSent.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridSent.ContextMenuStrip = this.contextMenu;
			this.gridSent.HasAddButton = false;
			this.gridSent.HasDropDowns = false;
			this.gridSent.HasMultilineHeaders = false;
			this.gridSent.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridSent.HeaderHeight = 15;
			this.gridSent.HScrollVisible = false;
			this.gridSent.Location = new System.Drawing.Point(0, 78);
			this.gridSent.Name = "gridSent";
			this.gridSent.ScrollValue = 0;
			this.gridSent.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridSent.Size = new System.Drawing.Size(1198, 457);
			this.gridSent.TabIndex = 2;
			this.gridSent.Title = "Guarantors - Sent To TSI";
			this.gridSent.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridSent.TitleHeight = 18;
			this.gridSent.TranslationName = "TableSent";
			this.gridSent.OnSortByColumn += new System.EventHandler(this.gridSentMain_OnSortByColumn);
			this.gridSent.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridSentMain_MouseDown);
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// butTsiOcp
			// 
			this.butTsiOcp.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butTsiOcp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butTsiOcp.Autosize = true;
			this.butTsiOcp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTsiOcp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTsiOcp.CornerRadius = 4F;
			this.butTsiOcp.Image = global::OpenDental.Properties.Resources.TSI_Icon;
			this.butTsiOcp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butTsiOcp.Location = new System.Drawing.Point(12, 660);
			this.butTsiOcp.Name = "butTsiOcp";
			this.butTsiOcp.Size = new System.Drawing.Size(75, 26);
			this.butTsiOcp.TabIndex = 32;
			this.butTsiOcp.Text = "OCP";
			this.butTsiOcp.Click += new System.EventHandler(this.butTsiOcp_Click);
			// 
			// labelTsiOcp
			// 
			this.labelTsiOcp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelTsiOcp.Location = new System.Drawing.Point(93, 664);
			this.labelTsiOcp.Name = "labelTsiOcp";
			this.labelTsiOcp.Size = new System.Drawing.Size(230, 17);
			this.labelTsiOcp.TabIndex = 3;
			this.labelTsiOcp.Text = "Launch the TSI Online Client Portal";
			this.labelTsiOcp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textUnsentTotalNumAccts
			// 
			this.textUnsentTotalNumAccts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textUnsentTotalNumAccts.BackColor = System.Drawing.SystemColors.Control;
			this.textUnsentTotalNumAccts.Location = new System.Drawing.Point(121, 539);
			this.textUnsentTotalNumAccts.MaxVal = 100000000;
			this.textUnsentTotalNumAccts.MinVal = 0;
			this.textUnsentTotalNumAccts.Name = "textUnsentTotalNumAccts";
			this.textUnsentTotalNumAccts.ReadOnly = true;
			this.textUnsentTotalNumAccts.Size = new System.Drawing.Size(60, 20);
			this.textUnsentTotalNumAccts.TabIndex = 124;
			this.textUnsentTotalNumAccts.TabStop = false;
			this.textUnsentTotalNumAccts.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textUnsent61to90
			// 
			this.textUnsent61to90.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textUnsent61to90.Location = new System.Drawing.Point(540, 539);
			this.textUnsent61to90.MaxVal = 100000000D;
			this.textUnsent61to90.MinVal = -100000000D;
			this.textUnsent61to90.Name = "textUnsent61to90";
			this.textUnsent61to90.ReadOnly = true;
			this.textUnsent61to90.Size = new System.Drawing.Size(75, 20);
			this.textUnsent61to90.TabIndex = 116;
			this.textUnsent61to90.TabStop = false;
			this.textUnsent61to90.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textUnsent0to30
			// 
			this.textUnsent0to30.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textUnsent0to30.Location = new System.Drawing.Point(394, 539);
			this.textUnsent0to30.MaxVal = 100000000D;
			this.textUnsent0to30.MinVal = -100000000D;
			this.textUnsent0to30.Name = "textUnsent0to30";
			this.textUnsent0to30.ReadOnly = true;
			this.textUnsent0to30.Size = new System.Drawing.Size(73, 20);
			this.textUnsent0to30.TabIndex = 118;
			this.textUnsent0to30.TabStop = false;
			this.textUnsent0to30.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textSentTotalNumAccts
			// 
			this.textSentTotalNumAccts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textSentTotalNumAccts.BackColor = System.Drawing.SystemColors.Control;
			this.textSentTotalNumAccts.Location = new System.Drawing.Point(121, 539);
			this.textSentTotalNumAccts.MaxVal = 100000000;
			this.textSentTotalNumAccts.MinVal = 0;
			this.textSentTotalNumAccts.Name = "textSentTotalNumAccts";
			this.textSentTotalNumAccts.ReadOnly = true;
			this.textSentTotalNumAccts.Size = new System.Drawing.Size(60, 20);
			this.textSentTotalNumAccts.TabIndex = 135;
			this.textSentTotalNumAccts.TabStop = false;
			this.textSentTotalNumAccts.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textSent61to90
			// 
			this.textSent61to90.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textSent61to90.Location = new System.Drawing.Point(540, 539);
			this.textSent61to90.MaxVal = 100000000D;
			this.textSent61to90.MinVal = -100000000D;
			this.textSent61to90.Name = "textSent61to90";
			this.textSent61to90.ReadOnly = true;
			this.textSent61to90.Size = new System.Drawing.Size(75, 20);
			this.textSent61to90.TabIndex = 127;
			this.textSent61to90.TabStop = false;
			this.textSent61to90.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textSent0to30
			// 
			this.textSent0to30.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textSent0to30.Location = new System.Drawing.Point(394, 539);
			this.textSent0to30.MaxVal = 100000000D;
			this.textSent0to30.MinVal = -100000000D;
			this.textSent0to30.Name = "textSent0to30";
			this.textSent0to30.ReadOnly = true;
			this.textSent0to30.Size = new System.Drawing.Size(73, 20);
			this.textSent0to30.TabIndex = 129;
			this.textSent0to30.TabStop = false;
			this.textSent0to30.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelSentTotalNumAccts
			// 
			this.labelSentTotalNumAccts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelSentTotalNumAccts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSentTotalNumAccts.Location = new System.Drawing.Point(3, 541);
			this.labelSentTotalNumAccts.Name = "labelSentTotalNumAccts";
			this.labelSentTotalNumAccts.Size = new System.Drawing.Size(116, 17);
			this.labelSentTotalNumAccts.TabIndex = 134;
			this.labelSentTotalNumAccts.Text = "Guarantor Count";
			this.labelSentTotalNumAccts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textSentPayPlanDue
			// 
			this.textSentPayPlanDue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textSentPayPlanDue.Location = new System.Drawing.Point(873, 539);
			this.textSentPayPlanDue.MaxVal = 100000000D;
			this.textSentPayPlanDue.MinVal = -100000000D;
			this.textSentPayPlanDue.Name = "textSentPayPlanDue";
			this.textSentPayPlanDue.ReadOnly = true;
			this.textSentPayPlanDue.Size = new System.Drawing.Size(80, 20);
			this.textSentPayPlanDue.TabIndex = 133;
			this.textSentPayPlanDue.TabStop = false;
			this.textSentPayPlanDue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textSentPatient
			// 
			this.textSentPatient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textSentPatient.Location = new System.Drawing.Point(809, 539);
			this.textSentPatient.MaxVal = 100000000D;
			this.textSentPatient.MinVal = -100000000D;
			this.textSentPatient.Name = "textSentPatient";
			this.textSentPatient.ReadOnly = true;
			this.textSentPatient.Size = new System.Drawing.Size(65, 20);
			this.textSentPatient.TabIndex = 132;
			this.textSentPatient.TabStop = false;
			this.textSentPatient.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textSentInsEst
			// 
			this.textSentInsEst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textSentInsEst.Location = new System.Drawing.Point(745, 539);
			this.textSentInsEst.MaxVal = 100000000D;
			this.textSentInsEst.MinVal = -100000000D;
			this.textSentInsEst.Name = "textSentInsEst";
			this.textSentInsEst.ReadOnly = true;
			this.textSentInsEst.Size = new System.Drawing.Size(65, 20);
			this.textSentInsEst.TabIndex = 131;
			this.textSentInsEst.TabStop = false;
			this.textSentInsEst.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textSentTotal
			// 
			this.textSentTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textSentTotal.Location = new System.Drawing.Point(686, 539);
			this.textSentTotal.MaxVal = 100000000D;
			this.textSentTotal.MinVal = -100000000D;
			this.textSentTotal.Name = "textSentTotal";
			this.textSentTotal.ReadOnly = true;
			this.textSentTotal.Size = new System.Drawing.Size(60, 20);
			this.textSentTotal.TabIndex = 130;
			this.textSentTotal.TabStop = false;
			this.textSentTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textSentOver90
			// 
			this.textSentOver90.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textSentOver90.Location = new System.Drawing.Point(614, 539);
			this.textSentOver90.MaxVal = 100000000D;
			this.textSentOver90.MinVal = -100000000D;
			this.textSentOver90.Name = "textSentOver90";
			this.textSentOver90.ReadOnly = true;
			this.textSentOver90.Size = new System.Drawing.Size(73, 20);
			this.textSentOver90.TabIndex = 126;
			this.textSentOver90.TabStop = false;
			this.textSentOver90.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textSent31to60
			// 
			this.textSent31to60.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textSent31to60.Location = new System.Drawing.Point(466, 539);
			this.textSent31to60.MaxVal = 100000000D;
			this.textSent31to60.MinVal = -100000000D;
			this.textSent31to60.Name = "textSent31to60";
			this.textSent31to60.ReadOnly = true;
			this.textSent31to60.Size = new System.Drawing.Size(75, 20);
			this.textSent31to60.TabIndex = 125;
			this.textSent31to60.TabStop = false;
			this.textSent31to60.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelSentTotals
			// 
			this.labelSentTotals.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelSentTotals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSentTotals.Location = new System.Drawing.Point(276, 541);
			this.labelSentTotals.Name = "labelSentTotals";
			this.labelSentTotals.Size = new System.Drawing.Size(116, 17);
			this.labelSentTotals.TabIndex = 128;
			this.labelSentTotals.Text = "Totals";
			this.labelSentTotals.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormArManager
			// 
			this.ClientSize = new System.Drawing.Size(1230, 696);
			this.Controls.Add(this.labelTsiOcp);
			this.Controls.Add(this.butTsiOcp);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.tabControlMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(1246, 330);
			this.Name = "FormArManager";
			this.Text = "Accounts Receivable Manager";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormArManager_FormClosing);
			this.Load += new System.EventHandler(this.FormArManager_Load);
			this.ResizeBegin += new System.EventHandler(this.FormArManager_ResizeBegin);
			this.ResizeEnd += new System.EventHandler(this.FormArManager_ResizeEnd);
			this.Resize += new System.EventHandler(this.FormArManager_Resize);
			this.contextMenu.ResumeLayout(false);
			this.tabControlMain.ResumeLayout(false);
			this.tabUnsent.ResumeLayout(false);
			this.tabUnsent.PerformLayout();
			this.groupPlaceAccounts.ResumeLayout(false);
			this.groupUnsentFilters.ResumeLayout(false);
			this.groupUnsentFilters.PerformLayout();
			this.tabSent.ResumeLayout(false);
			this.tabSent.PerformLayout();
			this.groupSentFIlters.ResumeLayout(false);
			this.groupSentFIlters.PerformLayout();
			this.groupUpdateAccounts.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private OpenDental.UI.Button butClose;
		private UI.ODGrid gridUnsent;
		private System.Windows.Forms.GroupBox groupUnsentFilters;
		private System.Windows.Forms.Label labelUnsentMinBal;
		private ValidDouble textUnsentMinBal;
		private System.Windows.Forms.Label labelUnsentAccountAge;
		private System.Windows.Forms.ComboBox comboUnsentAccountAge;
		private System.Windows.Forms.CheckBox checkExcludeIfProcs;
		private System.Windows.Forms.CheckBox checkExcludeInsPending;
		private System.Windows.Forms.CheckBox checkExcludeBadAddress;
		private UI.ComboBoxMulti comboBoxMultiBillTypes;
		private UI.ComboBoxMulti comboBoxMultiUnsentProvs;
		private System.Windows.Forms.Label labelUnsentBillTypes;
		private System.Windows.Forms.Label labelUnsentProvs;
		private System.Windows.Forms.Label labelUnsentClinics;
		private UI.ComboBoxMulti comboBoxMultiUnsentClinics;
		private UI.Button butSend;
		private System.Windows.Forms.TabControl tabControlMain;
		private System.Windows.Forms.TabPage tabUnsent;
		private System.Windows.Forms.TabPage tabSent;
		private UI.ODGrid gridSent;
		private System.Windows.Forms.GroupBox groupUpdateAccounts;
		private System.Windows.Forms.Label labelNewStatus;
		private System.Windows.Forms.ComboBox comboNewStatus;
		private UI.Button butUpdateStatus;
		private UI.Button butUnsentNone;
		private UI.Button butUnsentAll;
		private UI.Button butSentNone;
		private UI.Button butSentAll;
		private UI.Button butUnsentPrint;
		private UI.Button butSentPrint;
		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem menuItemGoTo;
		private System.Windows.Forms.GroupBox groupPlaceAccounts;
		private System.Windows.Forms.Label labelDemandType;
		private System.Windows.Forms.ComboBox comboDemandType;
		private System.Windows.Forms.Label labelUnsentDaysLastPay;
		private ValidNumber textUnsentDaysLastPay;
		private System.Windows.Forms.Timer timerFillGrid;
		private UI.Button butRunAging;
		private UI.Button butUnsentSaveDefault;
		private System.Windows.Forms.Label labelNewBillType;
		private System.Windows.Forms.ComboBox comboNewBillType;
		private System.Windows.Forms.GroupBox groupSentFIlters;
		private ValidNumber textSentDaysLastPay;
		private System.Windows.Forms.Label labelSentDaysLastPay;
		private UI.ComboBoxMulti comboBoxMultiLastTransType;
		private UI.ComboBoxMulti comboBoxMultiSentProvs;
		private System.Windows.Forms.Label labelLastTransType;
		private System.Windows.Forms.Label labelSentProvs;
		private System.Windows.Forms.Label labelSentAccountAge;
		private System.Windows.Forms.ComboBox comboSentAccountAge;
		private ValidDouble textSentMinBal;
		private System.Windows.Forms.Label labelSentMinBal;
		private System.Windows.Forms.Label labelSentClinics;
		private UI.ComboBoxMulti comboBoxMultiSentClinics;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private UI.Button butSentSaveDefaults;
		private System.Windows.Forms.Label labelTsiOcp;
		private UI.Button butTsiOcp;
		private ValidDouble textUnsentTotal;
		private ValidDouble textUnsentOver90;
		private ValidDouble textUnsent31to60;
		private System.Windows.Forms.Label labelUnsentTotals;
		private System.Windows.Forms.Label labelUnsentTotalNumAccts;
		private ValidDouble textUnsentPayPlanDue;
		private ValidDouble textUnsentPatient;
		private ValidDouble textUnsentInsEst;
		private ValidNumber textUnsentTotalNumAccts;
		private ValidDouble textUnsent61to90;
		private ValidDouble textUnsent0to30;
		private ValidNumber textSentTotalNumAccts;
		private ValidDouble textSent61to90;
		private ValidDouble textSent0to30;
		private System.Windows.Forms.Label labelSentTotalNumAccts;
		private ValidDouble textSentPayPlanDue;
		private ValidDouble textSentPatient;
		private ValidDouble textSentInsEst;
		private ValidDouble textSentTotal;
		private ValidDouble textSentOver90;
		private ValidDouble textSent31to60;
		private System.Windows.Forms.Label labelSentTotals;
	}
}