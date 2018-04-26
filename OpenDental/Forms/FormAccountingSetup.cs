using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.Bridges;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormAccountingSetup : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private Label label1;
		private OpenDental.UI.Button butAdd;
		private GroupBox groupBox1;
		private Label label2;
		private OpenDental.UI.Button butChange;
		private Label label3;
		private TextBox textAccountInc;
		private OpenDental.UI.Button butRemove;
		private ListBox listAccountsDep;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		///<summary>Each item in the array list is an int for an AccountNum for the deposit accounts.</summary>
		private ArrayList depAL;
		private GroupBox groupAutomaticPayment;
		private OpenDental.UI.Button butChangeCash;
		private Label label4;
		private TextBox textAccountCashInc;
		private Label label5;
		private OpenDental.UI.Button butAddPay;
		private long PickedDepAccountNum;
		//private ArrayList cashAL;
		private long PickedPayAccountNum;
		private OpenDental.UI.ODGrid gridMain;
		private ListBox listSoftware;
		private Label labelSoftware;
		private Panel panelQB;
		private Panel panelOD;
		private GroupBox groupQB;
		private Label labelDepositsQB;
		private ListBox listBoxDepositAccountsQB;
		private UI.Button butRemoveDepositQB;
		private UI.Button butAddDepositQB;
		private Label label7;
		private UI.Button butConnectQB;
		private UI.Button butBrowseQB;
		private Label labelCompanyFile;
		private TextBox textCompanyFileQB;
		private Label labelWarning;
		private Label labelIncomeAccountQB;
		private Label labelConnectQB;
		///<summary>Arraylist of AccountingAutoPays.</summary>
		private List<AccountingAutoPay> payList;
		private AccountingSoftware AcctSoftware;
		private Label labelQuickBooksTitle;
		private UI.Button butRemoveIncomeQB;
		private UI.Button butAddIncomeQB;
		private ListBox listBoxIncomeAccountsQB;
		private List<string> listDepositAccountsQB;
		private CheckBox checkQuickBooksClassRefsEnabled;
		private UI.Button buttonRemoveClassRefQB;
		private UI.Button buttonAddClassRefQB;
		private ListBox listBoxClassRefsQB;
		private Label labelClass;
		private List<string> listIncomeAccountsQB;
		///<summary>The list of currently available QuickBook classes.  Stored in a preference.</summary>
		private List<string> _listClassRefsQB;

		///<summary></summary>
		public FormAccountingSetup()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAccountingSetup));
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.listAccountsDep = new System.Windows.Forms.ListBox();
			this.butRemove = new OpenDental.UI.Button();
			this.butChange = new OpenDental.UI.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.textAccountInc = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.butAdd = new OpenDental.UI.Button();
			this.groupAutomaticPayment = new System.Windows.Forms.GroupBox();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butChangeCash = new OpenDental.UI.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.textAccountCashInc = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.butAddPay = new OpenDental.UI.Button();
			this.listSoftware = new System.Windows.Forms.ListBox();
			this.labelSoftware = new System.Windows.Forms.Label();
			this.panelQB = new System.Windows.Forms.Panel();
			this.groupQB = new System.Windows.Forms.GroupBox();
			this.buttonRemoveClassRefQB = new OpenDental.UI.Button();
			this.buttonAddClassRefQB = new OpenDental.UI.Button();
			this.listBoxClassRefsQB = new System.Windows.Forms.ListBox();
			this.labelClass = new System.Windows.Forms.Label();
			this.butRemoveIncomeQB = new OpenDental.UI.Button();
			this.butAddIncomeQB = new OpenDental.UI.Button();
			this.listBoxIncomeAccountsQB = new System.Windows.Forms.ListBox();
			this.labelQuickBooksTitle = new System.Windows.Forms.Label();
			this.labelConnectQB = new System.Windows.Forms.Label();
			this.labelIncomeAccountQB = new System.Windows.Forms.Label();
			this.labelWarning = new System.Windows.Forms.Label();
			this.butConnectQB = new OpenDental.UI.Button();
			this.butBrowseQB = new OpenDental.UI.Button();
			this.labelCompanyFile = new System.Windows.Forms.Label();
			this.textCompanyFileQB = new System.Windows.Forms.TextBox();
			this.listBoxDepositAccountsQB = new System.Windows.Forms.ListBox();
			this.butRemoveDepositQB = new OpenDental.UI.Button();
			this.butAddDepositQB = new OpenDental.UI.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.labelDepositsQB = new System.Windows.Forms.Label();
			this.panelOD = new System.Windows.Forms.Panel();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.checkQuickBooksClassRefsEnabled = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupAutomaticPayment.SuspendLayout();
			this.panelQB.SuspendLayout();
			this.groupQB.SuspendLayout();
			this.panelOD.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 61);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(168, 53);
			this.label1.TabIndex = 2;
			this.label1.Text = "User will get to pick from this list of accounts to deposit into";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.listAccountsDep);
			this.groupBox1.Controls.Add(this.butRemove);
			this.groupBox1.Controls.Add(this.butChange);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.textAccountInc);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.butAdd);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(519, 222);
			this.groupBox1.TabIndex = 32;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Automatic Deposit Entries";
			// 
			// listAccountsDep
			// 
			this.listAccountsDep.FormattingEnabled = true;
			this.listAccountsDep.Location = new System.Drawing.Point(182, 61);
			this.listAccountsDep.Name = "listAccountsDep";
			this.listAccountsDep.Size = new System.Drawing.Size(230, 108);
			this.listAccountsDep.TabIndex = 37;
			// 
			// butRemove
			// 
			this.butRemove.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRemove.Autosize = true;
			this.butRemove.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRemove.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRemove.CornerRadius = 4F;
			this.butRemove.Location = new System.Drawing.Point(417, 91);
			this.butRemove.Name = "butRemove";
			this.butRemove.Size = new System.Drawing.Size(75, 24);
			this.butRemove.TabIndex = 36;
			this.butRemove.Text = "Remove";
			this.butRemove.Click += new System.EventHandler(this.butRemove_Click);
			// 
			// butChange
			// 
			this.butChange.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChange.Autosize = true;
			this.butChange.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChange.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChange.CornerRadius = 4F;
			this.butChange.Location = new System.Drawing.Point(418, 176);
			this.butChange.Name = "butChange";
			this.butChange.Size = new System.Drawing.Size(75, 24);
			this.butChange.TabIndex = 35;
			this.butChange.Text = "Change";
			this.butChange.Click += new System.EventHandler(this.butChange_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(12, 179);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(168, 19);
			this.label3.TabIndex = 33;
			this.label3.Text = "Income Account";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textAccountInc
			// 
			this.textAccountInc.Location = new System.Drawing.Point(182, 179);
			this.textAccountInc.Name = "textAccountInc";
			this.textAccountInc.ReadOnly = true;
			this.textAccountInc.Size = new System.Drawing.Size(230, 20);
			this.textAccountInc.TabIndex = 34;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(19, 26);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(492, 27);
			this.label2.TabIndex = 32;
			this.label2.Text = "Every time a deposit is created, an accounting transaction will also be automatic" +
    "ally created.";
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Location = new System.Drawing.Point(418, 61);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(75, 24);
			this.butAdd.TabIndex = 30;
			this.butAdd.Text = "Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// groupAutomaticPayment
			// 
			this.groupAutomaticPayment.Controls.Add(this.gridMain);
			this.groupAutomaticPayment.Controls.Add(this.butChangeCash);
			this.groupAutomaticPayment.Controls.Add(this.label4);
			this.groupAutomaticPayment.Controls.Add(this.textAccountCashInc);
			this.groupAutomaticPayment.Controls.Add(this.label5);
			this.groupAutomaticPayment.Controls.Add(this.butAddPay);
			this.groupAutomaticPayment.Location = new System.Drawing.Point(27, 280);
			this.groupAutomaticPayment.Name = "groupAutomaticPayment";
			this.groupAutomaticPayment.Size = new System.Drawing.Size(519, 353);
			this.groupAutomaticPayment.TabIndex = 33;
			this.groupAutomaticPayment.TabStop = false;
			this.groupAutomaticPayment.Text = "Automatic Payment Entries";
			// 
			// gridMain
			// 
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(22, 112);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(471, 177);
			this.gridMain.TabIndex = 40;
			this.gridMain.Title = "Auto Payment Entries";
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableAccountingAutoPay";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butChangeCash
			// 
			this.butChangeCash.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChangeCash.Autosize = true;
			this.butChangeCash.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChangeCash.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChangeCash.CornerRadius = 4F;
			this.butChangeCash.Location = new System.Drawing.Point(418, 307);
			this.butChangeCash.Name = "butChangeCash";
			this.butChangeCash.Size = new System.Drawing.Size(75, 24);
			this.butChangeCash.TabIndex = 35;
			this.butChangeCash.Text = "Change";
			this.butChangeCash.Click += new System.EventHandler(this.butChangeCash_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(12, 310);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(168, 19);
			this.label4.TabIndex = 33;
			this.label4.Text = "Income Account";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textAccountCashInc
			// 
			this.textAccountCashInc.Location = new System.Drawing.Point(182, 310);
			this.textAccountCashInc.Name = "textAccountCashInc";
			this.textAccountCashInc.ReadOnly = true;
			this.textAccountCashInc.Size = new System.Drawing.Size(230, 20);
			this.textAccountCashInc.TabIndex = 34;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(19, 26);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(492, 47);
			this.label5.TabIndex = 32;
			this.label5.Text = resources.GetString("label5.Text");
			// 
			// butAddPay
			// 
			this.butAddPay.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddPay.Autosize = true;
			this.butAddPay.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddPay.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddPay.CornerRadius = 4F;
			this.butAddPay.Location = new System.Drawing.Point(418, 83);
			this.butAddPay.Name = "butAddPay";
			this.butAddPay.Size = new System.Drawing.Size(75, 24);
			this.butAddPay.TabIndex = 30;
			this.butAddPay.Text = "Add";
			this.butAddPay.Click += new System.EventHandler(this.butAddPay_Click);
			// 
			// listSoftware
			// 
			this.listSoftware.FormattingEnabled = true;
			this.listSoftware.Items.AddRange(new object[] {
            "Open Dental",
            "QuickBooks"});
			this.listSoftware.Location = new System.Drawing.Point(585, 53);
			this.listSoftware.Name = "listSoftware";
			this.listSoftware.Size = new System.Drawing.Size(102, 30);
			this.listSoftware.TabIndex = 34;
			this.listSoftware.Click += new System.EventHandler(this.listSoftware_Click);
			// 
			// labelSoftware
			// 
			this.labelSoftware.Location = new System.Drawing.Point(552, 32);
			this.labelSoftware.Name = "labelSoftware";
			this.labelSoftware.Size = new System.Drawing.Size(135, 19);
			this.labelSoftware.TabIndex = 38;
			this.labelSoftware.Text = "Deposit Software";
			this.labelSoftware.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panelQB
			// 
			this.panelQB.Controls.Add(this.groupQB);
			this.panelQB.Location = new System.Drawing.Point(27, 12);
			this.panelQB.Name = "panelQB";
			this.panelQB.Size = new System.Drawing.Size(218, 225);
			this.panelQB.TabIndex = 39;
			// 
			// groupQB
			// 
			this.groupQB.Controls.Add(this.buttonRemoveClassRefQB);
			this.groupQB.Controls.Add(this.buttonAddClassRefQB);
			this.groupQB.Controls.Add(this.listBoxClassRefsQB);
			this.groupQB.Controls.Add(this.labelClass);
			this.groupQB.Controls.Add(this.butRemoveIncomeQB);
			this.groupQB.Controls.Add(this.checkQuickBooksClassRefsEnabled);
			this.groupQB.Controls.Add(this.butAddIncomeQB);
			this.groupQB.Controls.Add(this.listBoxIncomeAccountsQB);
			this.groupQB.Controls.Add(this.labelQuickBooksTitle);
			this.groupQB.Controls.Add(this.labelConnectQB);
			this.groupQB.Controls.Add(this.labelIncomeAccountQB);
			this.groupQB.Controls.Add(this.labelWarning);
			this.groupQB.Controls.Add(this.butConnectQB);
			this.groupQB.Controls.Add(this.butBrowseQB);
			this.groupQB.Controls.Add(this.labelCompanyFile);
			this.groupQB.Controls.Add(this.textCompanyFileQB);
			this.groupQB.Controls.Add(this.listBoxDepositAccountsQB);
			this.groupQB.Controls.Add(this.butRemoveDepositQB);
			this.groupQB.Controls.Add(this.butAddDepositQB);
			this.groupQB.Controls.Add(this.label7);
			this.groupQB.Controls.Add(this.labelDepositsQB);
			this.groupQB.Location = new System.Drawing.Point(0, 0);
			this.groupQB.Name = "groupQB";
			this.groupQB.Size = new System.Drawing.Size(519, 606);
			this.groupQB.TabIndex = 0;
			this.groupQB.TabStop = false;
			this.groupQB.Text = "QuickBooks";
			// 
			// buttonRemoveClassRefQB
			// 
			this.buttonRemoveClassRefQB.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.buttonRemoveClassRefQB.Autosize = true;
			this.buttonRemoveClassRefQB.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.buttonRemoveClassRefQB.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.buttonRemoveClassRefQB.CornerRadius = 4F;
			this.buttonRemoveClassRefQB.Location = new System.Drawing.Point(418, 475);
			this.buttonRemoveClassRefQB.Name = "buttonRemoveClassRefQB";
			this.buttonRemoveClassRefQB.Size = new System.Drawing.Size(75, 24);
			this.buttonRemoveClassRefQB.TabIndex = 60;
			this.buttonRemoveClassRefQB.Text = "Remove";
			this.buttonRemoveClassRefQB.Visible = false;
			this.buttonRemoveClassRefQB.Click += new System.EventHandler(this.buttonRemoveClassRefQB_Click);
			// 
			// buttonAddClassRefQB
			// 
			this.buttonAddClassRefQB.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.buttonAddClassRefQB.Autosize = true;
			this.buttonAddClassRefQB.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.buttonAddClassRefQB.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.buttonAddClassRefQB.CornerRadius = 4F;
			this.buttonAddClassRefQB.Location = new System.Drawing.Point(418, 445);
			this.buttonAddClassRefQB.Name = "buttonAddClassRefQB";
			this.buttonAddClassRefQB.Size = new System.Drawing.Size(75, 24);
			this.buttonAddClassRefQB.TabIndex = 59;
			this.buttonAddClassRefQB.Text = "Add";
			this.buttonAddClassRefQB.Visible = false;
			this.buttonAddClassRefQB.Click += new System.EventHandler(this.buttonAddClassRefQB_Click);
			// 
			// listBoxClassRefsQB
			// 
			this.listBoxClassRefsQB.FormattingEnabled = true;
			this.listBoxClassRefsQB.Location = new System.Drawing.Point(182, 445);
			this.listBoxClassRefsQB.Name = "listBoxClassRefsQB";
			this.listBoxClassRefsQB.Size = new System.Drawing.Size(230, 108);
			this.listBoxClassRefsQB.TabIndex = 58;
			this.listBoxClassRefsQB.Visible = false;
			// 
			// labelClass
			// 
			this.labelClass.Location = new System.Drawing.Point(3, 446);
			this.labelClass.Name = "labelClass";
			this.labelClass.Size = new System.Drawing.Size(177, 53);
			this.labelClass.TabIndex = 57;
			this.labelClass.Text = "Class List";
			this.labelClass.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.labelClass.Visible = false;
			// 
			// butRemoveIncomeQB
			// 
			this.butRemoveIncomeQB.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRemoveIncomeQB.Autosize = true;
			this.butRemoveIncomeQB.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRemoveIncomeQB.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRemoveIncomeQB.CornerRadius = 4F;
			this.butRemoveIncomeQB.Location = new System.Drawing.Point(418, 336);
			this.butRemoveIncomeQB.Name = "butRemoveIncomeQB";
			this.butRemoveIncomeQB.Size = new System.Drawing.Size(75, 24);
			this.butRemoveIncomeQB.TabIndex = 56;
			this.butRemoveIncomeQB.Text = "Remove";
			this.butRemoveIncomeQB.Click += new System.EventHandler(this.butRemoveIncomeQB_Click);
			// 
			// butAddIncomeQB
			// 
			this.butAddIncomeQB.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddIncomeQB.Autosize = true;
			this.butAddIncomeQB.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddIncomeQB.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddIncomeQB.CornerRadius = 4F;
			this.butAddIncomeQB.Location = new System.Drawing.Point(418, 306);
			this.butAddIncomeQB.Name = "butAddIncomeQB";
			this.butAddIncomeQB.Size = new System.Drawing.Size(75, 24);
			this.butAddIncomeQB.TabIndex = 55;
			this.butAddIncomeQB.Text = "Add";
			this.butAddIncomeQB.Click += new System.EventHandler(this.butAddIncomeQB_Click);
			// 
			// listBoxIncomeAccountsQB
			// 
			this.listBoxIncomeAccountsQB.FormattingEnabled = true;
			this.listBoxIncomeAccountsQB.Location = new System.Drawing.Point(182, 306);
			this.listBoxIncomeAccountsQB.Name = "listBoxIncomeAccountsQB";
			this.listBoxIncomeAccountsQB.Size = new System.Drawing.Size(230, 108);
			this.listBoxIncomeAccountsQB.TabIndex = 54;
			// 
			// labelQuickBooksTitle
			// 
			this.labelQuickBooksTitle.Location = new System.Drawing.Point(15, 20);
			this.labelQuickBooksTitle.Name = "labelQuickBooksTitle";
			this.labelQuickBooksTitle.Size = new System.Drawing.Size(478, 34);
			this.labelQuickBooksTitle.TabIndex = 53;
			this.labelQuickBooksTitle.Text = "QuickBooks and the QuickBooks Foundation Class must be installed on this computer" +
    ". \r\nGo to the QuickBooks page on our website to download the QBFC installer.";
			this.labelQuickBooksTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelConnectQB
			// 
			this.labelConnectQB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelConnectQB.Location = new System.Drawing.Point(15, 82);
			this.labelConnectQB.Name = "labelConnectQB";
			this.labelConnectQB.Size = new System.Drawing.Size(397, 46);
			this.labelConnectQB.TabIndex = 52;
			this.labelConnectQB.Text = "Push the connect button with QuickBooks running in the background if this is your" +
    " first time using QuickBooks with Open Dental. (Only need to do this once)";
			this.labelConnectQB.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelIncomeAccountQB
			// 
			this.labelIncomeAccountQB.Location = new System.Drawing.Point(3, 307);
			this.labelIncomeAccountQB.Name = "labelIncomeAccountQB";
			this.labelIncomeAccountQB.Size = new System.Drawing.Size(177, 53);
			this.labelIncomeAccountQB.TabIndex = 51;
			this.labelIncomeAccountQB.Text = "User will get to pick from this list of income accounts.";
			this.labelIncomeAccountQB.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelWarning
			// 
			this.labelWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelWarning.Location = new System.Drawing.Point(35, 569);
			this.labelWarning.Name = "labelWarning";
			this.labelWarning.Size = new System.Drawing.Size(448, 27);
			this.labelWarning.TabIndex = 50;
			this.labelWarning.Text = "Open Dental will run faster if your QuickBooks company file is open in the backgr" +
    "ound.";
			this.labelWarning.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// butConnectQB
			// 
			this.butConnectQB.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butConnectQB.Autosize = true;
			this.butConnectQB.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butConnectQB.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butConnectQB.CornerRadius = 4F;
			this.butConnectQB.Location = new System.Drawing.Point(418, 93);
			this.butConnectQB.Name = "butConnectQB";
			this.butConnectQB.Size = new System.Drawing.Size(75, 24);
			this.butConnectQB.TabIndex = 49;
			this.butConnectQB.Text = "Connect";
			this.butConnectQB.Click += new System.EventHandler(this.butConnectQB_Click);
			// 
			// butBrowseQB
			// 
			this.butBrowseQB.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBrowseQB.Autosize = true;
			this.butBrowseQB.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBrowseQB.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBrowseQB.CornerRadius = 4F;
			this.butBrowseQB.Location = new System.Drawing.Point(418, 57);
			this.butBrowseQB.Name = "butBrowseQB";
			this.butBrowseQB.Size = new System.Drawing.Size(75, 24);
			this.butBrowseQB.TabIndex = 48;
			this.butBrowseQB.Text = "Browse";
			this.butBrowseQB.Click += new System.EventHandler(this.butBrowseQB_Click);
			// 
			// labelCompanyFile
			// 
			this.labelCompanyFile.Location = new System.Drawing.Point(12, 59);
			this.labelCompanyFile.Name = "labelCompanyFile";
			this.labelCompanyFile.Size = new System.Drawing.Size(105, 19);
			this.labelCompanyFile.TabIndex = 46;
			this.labelCompanyFile.Text = "Company File";
			this.labelCompanyFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textCompanyFileQB
			// 
			this.textCompanyFileQB.Location = new System.Drawing.Point(123, 59);
			this.textCompanyFileQB.Name = "textCompanyFileQB";
			this.textCompanyFileQB.Size = new System.Drawing.Size(289, 20);
			this.textCompanyFileQB.TabIndex = 47;
			// 
			// listBoxDepositAccountsQB
			// 
			this.listBoxDepositAccountsQB.FormattingEnabled = true;
			this.listBoxDepositAccountsQB.Location = new System.Drawing.Point(182, 191);
			this.listBoxDepositAccountsQB.Name = "listBoxDepositAccountsQB";
			this.listBoxDepositAccountsQB.Size = new System.Drawing.Size(230, 108);
			this.listBoxDepositAccountsQB.TabIndex = 44;
			// 
			// butRemoveDepositQB
			// 
			this.butRemoveDepositQB.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRemoveDepositQB.Autosize = true;
			this.butRemoveDepositQB.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRemoveDepositQB.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRemoveDepositQB.CornerRadius = 4F;
			this.butRemoveDepositQB.Location = new System.Drawing.Point(418, 220);
			this.butRemoveDepositQB.Name = "butRemoveDepositQB";
			this.butRemoveDepositQB.Size = new System.Drawing.Size(75, 24);
			this.butRemoveDepositQB.TabIndex = 43;
			this.butRemoveDepositQB.Text = "Remove";
			this.butRemoveDepositQB.Click += new System.EventHandler(this.butRemoveDepositQB_Click);
			// 
			// butAddDepositQB
			// 
			this.butAddDepositQB.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddDepositQB.Autosize = true;
			this.butAddDepositQB.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddDepositQB.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddDepositQB.CornerRadius = 4F;
			this.butAddDepositQB.Location = new System.Drawing.Point(418, 190);
			this.butAddDepositQB.Name = "butAddDepositQB";
			this.butAddDepositQB.Size = new System.Drawing.Size(75, 24);
			this.butAddDepositQB.TabIndex = 39;
			this.butAddDepositQB.Text = "Add";
			this.butAddDepositQB.Click += new System.EventHandler(this.butAddDepositQB_Click);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(3, 191);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(177, 53);
			this.label7.TabIndex = 38;
			this.label7.Text = "User will get to pick from this list of accounts to deposit to.";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelDepositsQB
			// 
			this.labelDepositsQB.Location = new System.Drawing.Point(12, 152);
			this.labelDepositsQB.Name = "labelDepositsQB";
			this.labelDepositsQB.Size = new System.Drawing.Size(492, 27);
			this.labelDepositsQB.TabIndex = 33;
			this.labelDepositsQB.Text = "Every time a deposit is created, a deposit will be created within QuickBooks usin" +
    "g these settings.\r\n(Commas must be removed from account names within QuickBooks)" +
    "";
			this.labelDepositsQB.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// panelOD
			// 
			this.panelOD.Controls.Add(this.groupBox1);
			this.panelOD.Location = new System.Drawing.Point(281, 15);
			this.panelOD.Name = "panelOD";
			this.panelOD.Size = new System.Drawing.Size(265, 201);
			this.panelOD.TabIndex = 40;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(607, 565);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(607, 606);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// checkQuickBooksClassRefsEnabled
			// 
			this.checkQuickBooksClassRefsEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkQuickBooksClassRefsEnabled.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkQuickBooksClassRefsEnabled.Location = new System.Drawing.Point(154, 420);
			this.checkQuickBooksClassRefsEnabled.Name = "checkQuickBooksClassRefsEnabled";
			this.checkQuickBooksClassRefsEnabled.Size = new System.Drawing.Size(258, 19);
			this.checkQuickBooksClassRefsEnabled.TabIndex = 41;
			this.checkQuickBooksClassRefsEnabled.Text = "Enable QuickBooks Class Refs";
			this.checkQuickBooksClassRefsEnabled.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkQuickBooksClassRefsEnabled.CheckedChanged += new System.EventHandler(this.checkQuickBooksClassRefsEnabled_CheckedChanged);
			// 
			// FormAccountingSetup
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(715, 657);
			this.Controls.Add(this.panelOD);
			this.Controls.Add(this.panelQB);
			this.Controls.Add(this.labelSoftware);
			this.Controls.Add(this.listSoftware);
			this.Controls.Add(this.groupAutomaticPayment);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAccountingSetup";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Setup Accounting";
			this.Load += new System.EventHandler(this.FormAccountingSetup_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupAutomaticPayment.ResumeLayout(false);
			this.groupAutomaticPayment.PerformLayout();
			this.panelQB.ResumeLayout(false);
			this.groupQB.ResumeLayout(false);
			this.groupQB.PerformLayout();
			this.panelOD.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormAccountingSetup_Load(object sender,EventArgs e) {
			AcctSoftware=(AccountingSoftware)PrefC.GetInt(PrefName.AccountingSoftware);
			if(AcctSoftware==AccountingSoftware.QuickBooks) {
				PanelLayoutQB();
			}
			else {
				PanelLayoutOD();
			}
			listSoftware.SelectedIndex=(int)AcctSoftware;
		}

		private void FillDepList(){
			listAccountsDep.Items.Clear();
			for(int i=0;i<depAL.Count;i++){
				listAccountsDep.Items.Add(Accounts.GetDescript((long)depAL[i]));
			}
		}

		private void FillPayGrid(){
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableAccountingAutoPay","Payment Type"),200);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableAccountingAutoPay","Pick List"),250);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<payList.Count;i++){
				row=new ODGridRow();
				row.Cells.Add(Defs.GetName(DefCat.PaymentTypes,payList[i].PayType));
				row.Cells.Add(AccountingAutoPays.GetPickListDesc(payList[i]));
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void FillQBLists() {
			listBoxDepositAccountsQB.Items.Clear();
			for(int i=0;i<listDepositAccountsQB.Count;i++) {
			  listBoxDepositAccountsQB.Items.Add(listDepositAccountsQB[i]);
			}
			listBoxIncomeAccountsQB.Items.Clear();
			for(int i=0;i<listIncomeAccountsQB.Count;i++) {
				listBoxIncomeAccountsQB.Items.Add(listIncomeAccountsQB[i]);
			}
			listBoxClassRefsQB.Items.Clear();
			for(int i=0;i<_listClassRefsQB.Count;i++) {
				listBoxClassRefsQB.Items.Add(_listClassRefsQB[i]);
			}
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormAccountPick FormA=new FormAccountPick();
			FormA.ShowDialog();
			if(FormA.DialogResult!=DialogResult.OK) {
				return;
			}
			depAL.Add(FormA.SelectedAccount.AccountNum);
			FillDepList();
		}

		private void butRemove_Click(object sender,EventArgs e) {
			if(listAccountsDep.SelectedIndex==-1){
				MsgBox.Show(this,"Please select an item first.");
				return;
			}
			depAL.RemoveAt(listAccountsDep.SelectedIndex);
			FillDepList();
		}

		private void butChange_Click(object sender,EventArgs e) {
			FormAccountPick FormA=new FormAccountPick();
			FormA.ShowDialog();
			if(FormA.DialogResult!=DialogResult.OK) {
				return;
			}
			PickedDepAccountNum=FormA.SelectedAccount.AccountNum;
			textAccountInc.Text=Accounts.GetDescript(PickedDepAccountNum);
		}

		private void butBrowseQB_Click(object sender,EventArgs e) {
			OpenFileDialog fdlg=new OpenFileDialog();
			fdlg.Title="QuickBooks Company File";
			fdlg.InitialDirectory=@"C:\";
			fdlg.Filter="QuickBooks|*.qbw";
			fdlg.RestoreDirectory=true;
			if(fdlg.ShowDialog()==DialogResult.OK) {
				textCompanyFileQB.Text=fdlg.FileName;
			}
		}

		private void butConnectQB_Click(object sender,EventArgs e) {
			if(textCompanyFileQB.Text.Trim()=="") {
				MsgBox.Show(this,"Browse to your QuickBooks company file first.");
				return;
			}
			Cursor.Current=Cursors.WaitCursor;
			string result=QuickBooks.TestConnection(textCompanyFileQB.Text);
			Cursor.Current=Cursors.Default;
			MessageBox.Show(result);
		}

		private void butAddDepositQB_Click(object sender,EventArgs e) {
			List<string> depositList=GetAccountsQB();
			if(depositList!=null) {
			  listDepositAccountsQB.AddRange(depositList);
			  FillQBLists();
			}
		}

		private void butRemoveDepositQB_Click(object sender,EventArgs e) {
			if(listBoxDepositAccountsQB.SelectedIndex==-1){
			  MsgBox.Show(this,"Please select an item first.");
			  return;
			}
			listDepositAccountsQB.RemoveAt(listBoxDepositAccountsQB.SelectedIndex);
			FillQBLists();
		}

		private void butAddIncomeQB_Click(object sender,EventArgs e) {
			List<string> incomeList=GetAccountsQB();
			if(incomeList!=null) {
			  listIncomeAccountsQB.AddRange(incomeList);
			  FillQBLists();
			}
		}

		private void butRemoveIncomeQB_Click(object sender,EventArgs e) {
			if(listBoxIncomeAccountsQB.SelectedIndex==-1){
			  MsgBox.Show(this,"Please select an item first.");
			  return;
			}
			listIncomeAccountsQB.RemoveAt(listBoxIncomeAccountsQB.SelectedIndex);
			FillQBLists();
		}
		
		private void buttonAddClassRefQB_Click(object sender,EventArgs e) {
			if(textCompanyFileQB.Text.Trim()=="") {
				MsgBox.Show(this,"Browse to your QuickBooks company file first.");
				return;
			}
			if(Prefs.UpdateString(PrefName.QuickBooksCompanyFile,textCompanyFileQB.Text)) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			List<string> listClasses=new List<string>();
			try {
				listClasses=QuickBooks.GetListOfClasses();
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			InputBox FormChooseClasses=new InputBox(Lan.g(this,"Choose a class"),listClasses,true);
			FormChooseClasses.TopLevel=true;
			if(FormChooseClasses.ShowDialog()!=DialogResult.OK) {
				return;
			}
			if(FormChooseClasses.SelectedIndices.Count < 1) {
				MsgBox.Show(this,"You must choose a class.");
				return;
			}
			foreach(int i in FormChooseClasses.SelectedIndices) {
				string classCur=listClasses[i];
				if(_listClassRefsQB.Contains(classCur)) {
					continue;
				}
				_listClassRefsQB.Add(classCur);
			}
			FillQBLists();
		}

		private void buttonRemoveClassRefQB_Click(object sender,EventArgs e) {
			if(listBoxClassRefsQB.SelectedIndex==-1) {
				MsgBox.Show(this,"Please select an item first.");
				return;
			}
			_listClassRefsQB.RemoveAt(listBoxClassRefsQB.SelectedIndex);
			FillQBLists();
		}

		///<summary>Launches the account pick window and lets user choose several accounts.  Returns null if anything went wrong or user canceled out.</summary>
		private List<string> GetAccountsQB() {
			if(textCompanyFileQB.Text.Trim()=="") {
				MsgBox.Show(this,"Browse to your QuickBooks company file first.");
				return null;
			}
			if(Prefs.UpdateString(PrefName.QuickBooksCompanyFile,textCompanyFileQB.Text)) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			FormAccountPick FormA=new FormAccountPick();
			FormA.IsQuickBooks=true;
			FormA.ShowDialog();
			if(FormA.DialogResult!=DialogResult.OK) {
			  return null;
			}
			if(FormA.SelectedAccountsQB!=null) {
				return FormA.SelectedAccountsQB;
			}
			return null;
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormAccountingAutoPayEdit FormA=new FormAccountingAutoPayEdit();
			FormA.AutoPayCur=payList[e.Row];
			FormA.ShowDialog();
			if(FormA.AutoPayCur==null){//user deleted
				payList.RemoveAt(e.Row);
			}
			FillPayGrid();
		}

		private void listSoftware_Click(object sender,EventArgs e) {
			int index=listSoftware.SelectedIndex;
			if(index==-1){
				return;
			}
			if(index==0){//Open Dental
				PanelLayoutOD();
				return;
			}
			else {//QuickBooks
				PanelLayoutQB();
				return;
			}
		}

		///<summary>Changes the window visually for Open Dental accounting users.</summary>
		private void PanelLayoutOD() {
			groupAutomaticPayment.Visible=true;
			panelQB.Visible=false;
			panelOD.Visible=true;
			panelOD.Location=new Point(27,27);
			panelOD.Size=new Size(519,222);
			groupAutomaticPayment.Visible=true;
			//Update the grids for this layout.
			string depStr=PrefC.GetString(PrefName.AccountingDepositAccounts);
			string[] depStrArray=depStr.Split(new char[] { ',' });
			depAL=new ArrayList();
			for(int i=0;i<depStrArray.Length;i++) {
				if(depStrArray[i]=="") {
					continue;
				}
				depAL.Add(PIn.Long(depStrArray[i]));
			}
			FillDepList();
			PickedDepAccountNum=PrefC.GetLong(PrefName.AccountingIncomeAccount);
			textAccountInc.Text=Accounts.GetDescript(PickedDepAccountNum);
			//pay----------------------------------------------------------
			payList=AccountingAutoPays.GetDeepCopy();
			FillPayGrid();
			PickedPayAccountNum=PrefC.GetLong(PrefName.AccountingCashIncomeAccount);
			textAccountCashInc.Text=Accounts.GetDescript(PickedPayAccountNum);
		}

		///<summary>Changes the window visually for QuickBooks users.</summary>
		private void PanelLayoutQB() {
			groupAutomaticPayment.Visible=false;
			panelOD.Visible=false;
			panelQB.Visible=true;
			panelQB.Location=new Point(27,27);
			panelQB.Size=new Size(519,606);
			groupAutomaticPayment.Visible=false;
			textCompanyFileQB.Text=PrefC.GetString(PrefName.QuickBooksCompanyFile);
			checkQuickBooksClassRefsEnabled.Checked=PrefC.GetBool(PrefName.QuickBooksClassRefsEnabled);
			string acctStr=PrefC.GetString(PrefName.QuickBooksIncomeAccount);
			string[] acctStrArray=acctStr.Split(new char[] { ',' });
			listIncomeAccountsQB=new List<string>();
			for(int i=0;i<acctStrArray.Length;i++) {
				if(acctStrArray[i]=="") {
					continue;
				}
				listIncomeAccountsQB.Add(acctStrArray[i]);
			}
			string depStr=PrefC.GetString(PrefName.QuickBooksDepositAccounts);
			string[] depStrArray=depStr.Split(new char[] { ',' });
			listDepositAccountsQB=new List<string>();
			for(int i=0;i<depStrArray.Length;i++) {
				if(depStrArray[i]=="") {
					continue;
				}
				listDepositAccountsQB.Add(depStrArray[i]);
			}
			string classStr=PrefC.GetString(PrefName.QuickBooksClassRefs);
			string[] classStrArray=classStr.Split(new char[] { ',' });
			_listClassRefsQB=new List<string>();
			for(int i = 0;i<classStrArray.Length;i++) {
				if(classStrArray[i]=="") {
					continue;
				}
				_listClassRefsQB.Add(classStrArray[i]);
			}
			FillQBLists();
		}

		private void checkQuickBooksClassRefsEnabled_CheckedChanged(object sender,EventArgs e) {
			if(checkQuickBooksClassRefsEnabled.Checked) {
				labelClass.Visible=true;
				listBoxClassRefsQB.Visible=true;
				buttonAddClassRefQB.Visible=true;
				buttonRemoveClassRefQB.Visible=true;
			}
			else {
				labelClass.Visible=false;
				listBoxClassRefsQB.Visible=false;
				buttonAddClassRefQB.Visible=false;
				buttonRemoveClassRefQB.Visible=false;
			}
		}

		private void butAddPay_Click(object sender,EventArgs e) {
			AccountingAutoPay autoPay=new AccountingAutoPay();
			FormAccountingAutoPayEdit FormA=new FormAccountingAutoPayEdit();
			FormA.AutoPayCur=autoPay;
			FormA.IsNew=true;
			if(FormA.ShowDialog()!=DialogResult.OK) {
				return;
			}
			payList.Add(autoPay);
			FillPayGrid();
		}

		private void butChangeCash_Click(object sender,EventArgs e) {
			FormAccountPick FormA=new FormAccountPick();
			FormA.ShowDialog();
			if(FormA.DialogResult!=DialogResult.OK) {
				return;
			}
			PickedPayAccountNum=FormA.SelectedAccount.AccountNum;
			textAccountCashInc.Text=Accounts.GetDescript(PickedPayAccountNum);
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(listSoftware.SelectedIndex==-1) {
				MsgBox.Show(this,"Must select an accounting software.");
				return;
			}
			if(listSoftware.SelectedIndex==0) {//Open Dental
				string depStr="";
				for(int i=0;i<depAL.Count;i++) {
					if(i>0) {
						depStr+=",";
					}
					depStr+=depAL[i].ToString();
				}
				if(Prefs.UpdateString(PrefName.AccountingDepositAccounts,depStr)) {
					DataValid.SetInvalid(InvalidType.Prefs);
				}
				if(Prefs.UpdateLong(PrefName.AccountingIncomeAccount,PickedDepAccountNum)) {
					DataValid.SetInvalid(InvalidType.Prefs);
				}
				//pay------------------------------------------------------------------------------------------
				AccountingAutoPays.SaveList(payList);//just deletes them all and starts over
				DataValid.SetInvalid(InvalidType.AccountingAutoPays);
				if(Prefs.UpdateLong(PrefName.AccountingCashIncomeAccount,PickedPayAccountNum)) {
					DataValid.SetInvalid(InvalidType.Prefs);
				}
			}
			else {//QuickBooks
				string depStr="";
				for(int i=0;i<listBoxDepositAccountsQB.Items.Count;i++) {
					if(i>0) {
						depStr+=",";
					}
					depStr+=listBoxDepositAccountsQB.Items[i].ToString();
				}
				string incomeStr="";
				for(int i=0;i<listBoxIncomeAccountsQB.Items.Count;i++) {
					if(i>0) {
						incomeStr+=",";
					}
					incomeStr+=listBoxIncomeAccountsQB.Items[i].ToString();
				}
				string classStr="";
				for(int i = 0;i<listBoxClassRefsQB.Items.Count;i++) {
					if(i>0) {
						classStr+=",";
					}
					classStr+=listBoxClassRefsQB.Items[i].ToString();
				}
				if(Prefs.UpdateString(PrefName.QuickBooksCompanyFile,textCompanyFileQB.Text)
					| Prefs.UpdateString(PrefName.QuickBooksDepositAccounts,depStr)
					| Prefs.UpdateString(PrefName.QuickBooksIncomeAccount,incomeStr)
					| Prefs.UpdateBool(PrefName.QuickBooksClassRefsEnabled,checkQuickBooksClassRefsEnabled.Checked)
					| Prefs.UpdateString(PrefName.QuickBooksClassRefs,classStr)) 
				{
					DataValid.SetInvalid(InvalidType.Prefs);
				}
			}
			//Update the selected accounting software.
			if(Prefs.UpdateInt(PrefName.AccountingSoftware,listSoftware.SelectedIndex)) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		

		

		


	}
}





















