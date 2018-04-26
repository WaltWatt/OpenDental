using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.Bridges;
using OpenDental.UI;
using OpenDentBusiness;
using System.Diagnostics;
using System.Linq;
using CodeBase;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormDepositEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ComboBox comboClinic;
		private System.Windows.Forms.Label labelClinic;
		private System.Windows.Forms.ListBox listPayType;
		private System.Windows.Forms.Label label2;
		private Deposit _depositCur;
		private Deposit _depositOld;
		private System.Windows.Forms.Label label1;
		private OpenDental.UI.Button butDelete;
		private OpenDental.ValidDate textDate;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBankAccountInfo;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textAmount;
		private System.Windows.Forms.GroupBox groupSelect;
		private OpenDental.UI.Button butPrint;
		private OpenDental.UI.ODGrid gridPat;
		private OpenDental.UI.ODGrid gridIns;
		///<summary></summary>
		public bool IsNew;
		private ClaimPayment[] ClaimPayList;
		private OpenDental.ValidDate textDateStart;
		private System.Windows.Forms.Label label5;
		private OpenDental.UI.Button butRefresh;
		private List<Payment> PatPayList;
		private ComboBox comboDepositAccount;
		private Label labelDepositAccount;
		private bool changed;
		private TextBox textDepositAccount;
		///<summary>Only used if linking to accounts</summary>
		private long[] DepositAccounts;
		private UI.Button butSendQB;
		private TextBox textMemo;
		private Label labelMemo;
		private ListBox listInsPayType;
		private Label label6;
		///<summary>True if the accounting software pref is set to QuickBooks.</summary>
		private bool IsQuickBooks;
		private TextBox textAmountSearch;
		private TextBox textCheckNumSearch;
		private Label label7;
		private Label label8;
		private TextBox textItemNum;
		private Label label9;
		private UI.Button butPDF;
		///<summary>Used to store DefNums in a 1:1 ratio for listInsPayType</summary>
		private List<long> _insPayDefNums;
		///<summary>Used to store DefNums in a 1:1 ratio for listPayType</summary>
		private List<long> _payTypeDefNums;
		///<summary>Keeps track of whether the payment has been saved to the database since the form was opened.</summary>
		private bool _hasBeenSavedToDB;
		///<summary>A list of payNums already attached to the deposit.  When printing or showing PDF these were attached to the deposit.
		///Used on OK click to make sure we detach any procedures that might have been unselected after they've been attached in the DB.</summary>
		private List<long> _listPayNumsAttached=new List<long>();
		///<summary>A list of claimPaymentNum already attached to the deposit.  When printing or showing PDF these were attached to the deposit.
		///Used on OK click to make sure we detach any procedures that might have been unselected after they've been attached in the DB.</summary>
		private List<long> _listClaimPaymentNumAttached=new List<long>();
		///<summary>Used in UpdateToDB to detach any payments that were attached to deposit but have been deselected before clicking OK.</summary>
		private bool _isOnOKClick=false;
		private ComboBox comboClassRefs;
		private Label labelClassRef;
		private List<Clinic> _listClinics;
		private UI.Button butEmailPDF;
		private Label labelDepositAccountNum;
		private ComboBox comboDepositAccountNum;
		private string[] _arrayClassesQB;
		private TextBox textBatch;
		private Label labelBatchNum;

		///<summary></summary>
		public FormDepositEdit(Deposit depositCur)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			_depositCur=depositCur;
			_depositOld=depositCur.Copy();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDepositEdit));
			this.groupSelect = new System.Windows.Forms.GroupBox();
			this.comboClassRefs = new System.Windows.Forms.ComboBox();
			this.labelClassRef = new System.Windows.Forms.Label();
			this.listInsPayType = new System.Windows.Forms.ListBox();
			this.label6 = new System.Windows.Forms.Label();
			this.butRefresh = new OpenDental.UI.Button();
			this.textDateStart = new OpenDental.ValidDate();
			this.label5 = new System.Windows.Forms.Label();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.listPayType = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textBankAccountInfo = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textAmount = new System.Windows.Forms.TextBox();
			this.comboDepositAccount = new System.Windows.Forms.ComboBox();
			this.labelDepositAccount = new System.Windows.Forms.Label();
			this.textDepositAccount = new System.Windows.Forms.TextBox();
			this.textMemo = new System.Windows.Forms.TextBox();
			this.labelMemo = new System.Windows.Forms.Label();
			this.textAmountSearch = new System.Windows.Forms.TextBox();
			this.textCheckNumSearch = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.textItemNum = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.butSendQB = new OpenDental.UI.Button();
			this.gridIns = new OpenDental.UI.ODGrid();
			this.butPrint = new OpenDental.UI.Button();
			this.textDate = new OpenDental.ValidDate();
			this.butDelete = new OpenDental.UI.Button();
			this.gridPat = new OpenDental.UI.ODGrid();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butPDF = new OpenDental.UI.Button();
			this.butEmailPDF = new OpenDental.UI.Button();
			this.labelDepositAccountNum = new System.Windows.Forms.Label();
			this.comboDepositAccountNum = new System.Windows.Forms.ComboBox();
			this.textBatch = new System.Windows.Forms.TextBox();
			this.labelBatchNum = new System.Windows.Forms.Label();
			this.groupSelect.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupSelect
			// 
			this.groupSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupSelect.Controls.Add(this.comboClassRefs);
			this.groupSelect.Controls.Add(this.labelClassRef);
			this.groupSelect.Controls.Add(this.listInsPayType);
			this.groupSelect.Controls.Add(this.label6);
			this.groupSelect.Controls.Add(this.butRefresh);
			this.groupSelect.Controls.Add(this.textDateStart);
			this.groupSelect.Controls.Add(this.label5);
			this.groupSelect.Controls.Add(this.comboClinic);
			this.groupSelect.Controls.Add(this.labelClinic);
			this.groupSelect.Controls.Add(this.listPayType);
			this.groupSelect.Controls.Add(this.label2);
			this.groupSelect.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupSelect.Location = new System.Drawing.Point(602, 326);
			this.groupSelect.Name = "groupSelect";
			this.groupSelect.Size = new System.Drawing.Size(355, 299);
			this.groupSelect.TabIndex = 99;
			this.groupSelect.TabStop = false;
			this.groupSelect.Text = "Show";
			// 
			// comboClassRefs
			// 
			this.comboClassRefs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClassRefs.Location = new System.Drawing.Point(184, 68);
			this.comboClassRefs.MaxDropDownItems = 30;
			this.comboClassRefs.Name = "comboClassRefs";
			this.comboClassRefs.Size = new System.Drawing.Size(165, 21);
			this.comboClassRefs.TabIndex = 110;
			this.comboClassRefs.Visible = false;
			// 
			// labelClassRef
			// 
			this.labelClassRef.Location = new System.Drawing.Point(184, 51);
			this.labelClassRef.Name = "labelClassRef";
			this.labelClassRef.Size = new System.Drawing.Size(102, 16);
			this.labelClassRef.TabIndex = 109;
			this.labelClassRef.Text = "Class";
			this.labelClassRef.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.labelClassRef.Visible = false;
			// 
			// listInsPayType
			// 
			this.listInsPayType.Location = new System.Drawing.Point(184, 111);
			this.listInsPayType.Name = "listInsPayType";
			this.listInsPayType.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listInsPayType.Size = new System.Drawing.Size(165, 147);
			this.listInsPayType.TabIndex = 107;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(184, 94);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(165, 16);
			this.label6.TabIndex = 108;
			this.label6.Text = "Insurance Payment Types";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(142, 264);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 24);
			this.butRefresh.TabIndex = 106;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// textDateStart
			// 
			this.textDateStart.Location = new System.Drawing.Point(14, 31);
			this.textDateStart.Name = "textDateStart";
			this.textDateStart.Size = new System.Drawing.Size(94, 20);
			this.textDateStart.TabIndex = 105;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(14, 14);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(118, 16);
			this.label5.TabIndex = 104;
			this.label5.Text = "Start Date";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(14, 68);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(165, 21);
			this.comboClinic.TabIndex = 94;
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(14, 51);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(102, 16);
			this.labelClinic.TabIndex = 93;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listPayType
			// 
			this.listPayType.Location = new System.Drawing.Point(14, 111);
			this.listPayType.Name = "listPayType";
			this.listPayType.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listPayType.Size = new System.Drawing.Size(165, 147);
			this.listPayType.TabIndex = 96;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(14, 94);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(165, 16);
			this.label2.TabIndex = 97;
			this.label2.Text = "Patient Payment Types";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(602, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(95, 16);
			this.label1.TabIndex = 102;
			this.label1.Text = "Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Location = new System.Drawing.Point(601, 89);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(238, 16);
			this.label3.TabIndex = 104;
			this.label3.Text = "Bank Account Info";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textBankAccountInfo
			// 
			this.textBankAccountInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textBankAccountInfo.Location = new System.Drawing.Point(601, 106);
			this.textBankAccountInfo.Multiline = true;
			this.textBankAccountInfo.Name = "textBankAccountInfo";
			this.textBankAccountInfo.Size = new System.Drawing.Size(289, 59);
			this.textBankAccountInfo.TabIndex = 105;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Location = new System.Drawing.Point(702, 8);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(94, 16);
			this.label4.TabIndex = 106;
			this.label4.Text = "Amount";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textAmount
			// 
			this.textAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textAmount.Location = new System.Drawing.Point(702, 25);
			this.textAmount.Name = "textAmount";
			this.textAmount.ReadOnly = true;
			this.textAmount.Size = new System.Drawing.Size(94, 20);
			this.textAmount.TabIndex = 107;
			// 
			// comboDepositAccount
			// 
			this.comboDepositAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboDepositAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDepositAccount.FormattingEnabled = true;
			this.comboDepositAccount.Location = new System.Drawing.Point(602, 235);
			this.comboDepositAccount.Name = "comboDepositAccount";
			this.comboDepositAccount.Size = new System.Drawing.Size(289, 21);
			this.comboDepositAccount.TabIndex = 110;
			// 
			// labelDepositAccount
			// 
			this.labelDepositAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelDepositAccount.Location = new System.Drawing.Point(602, 218);
			this.labelDepositAccount.Name = "labelDepositAccount";
			this.labelDepositAccount.Size = new System.Drawing.Size(289, 16);
			this.labelDepositAccount.TabIndex = 111;
			this.labelDepositAccount.Text = "Deposit into Account";
			this.labelDepositAccount.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textDepositAccount
			// 
			this.textDepositAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textDepositAccount.Location = new System.Drawing.Point(602, 261);
			this.textDepositAccount.Name = "textDepositAccount";
			this.textDepositAccount.ReadOnly = true;
			this.textDepositAccount.Size = new System.Drawing.Size(289, 20);
			this.textDepositAccount.TabIndex = 112;
			// 
			// textMemo
			// 
			this.textMemo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textMemo.Location = new System.Drawing.Point(601, 183);
			this.textMemo.Multiline = true;
			this.textMemo.Name = "textMemo";
			this.textMemo.Size = new System.Drawing.Size(289, 35);
			this.textMemo.TabIndex = 117;
			// 
			// labelMemo
			// 
			this.labelMemo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelMemo.Location = new System.Drawing.Point(601, 166);
			this.labelMemo.Name = "labelMemo";
			this.labelMemo.Size = new System.Drawing.Size(127, 16);
			this.labelMemo.TabIndex = 116;
			this.labelMemo.Text = "Memo";
			this.labelMemo.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textAmountSearch
			// 
			this.textAmountSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textAmountSearch.Location = new System.Drawing.Point(469, 663);
			this.textAmountSearch.Name = "textAmountSearch";
			this.textAmountSearch.Size = new System.Drawing.Size(94, 20);
			this.textAmountSearch.TabIndex = 118;
			this.textAmountSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textAmountSearch_KeyUp);
			this.textAmountSearch.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textAmountSearch_MouseUp);
			// 
			// textCheckNumSearch
			// 
			this.textCheckNumSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textCheckNumSearch.Location = new System.Drawing.Point(247, 663);
			this.textCheckNumSearch.Name = "textCheckNumSearch";
			this.textCheckNumSearch.Size = new System.Drawing.Size(94, 20);
			this.textCheckNumSearch.TabIndex = 119;
			this.textCheckNumSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textCheckNumSearch_KeyUp);
			this.textCheckNumSearch.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textCheckNumSearch_MouseUp);
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label7.Location = new System.Drawing.Point(347, 663);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(121, 20);
			this.label7.TabIndex = 109;
			this.label7.Text = "Search Amount";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label8.Location = new System.Drawing.Point(98, 663);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(148, 20);
			this.label8.TabIndex = 120;
			this.label8.Text = "Search Check Number";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textItemNum
			// 
			this.textItemNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textItemNum.Location = new System.Drawing.Point(802, 25);
			this.textItemNum.Name = "textItemNum";
			this.textItemNum.ReadOnly = true;
			this.textItemNum.Size = new System.Drawing.Size(54, 20);
			this.textItemNum.TabIndex = 121;
			this.textItemNum.Text = "0";
			// 
			// label9
			// 
			this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label9.Location = new System.Drawing.Point(802, 8);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(66, 16);
			this.label9.TabIndex = 122;
			this.label9.Text = "Item Count";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butSendQB
			// 
			this.butSendQB.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSendQB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butSendQB.Autosize = true;
			this.butSendQB.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSendQB.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSendQB.CornerRadius = 4F;
			this.butSendQB.Location = new System.Drawing.Point(897, 261);
			this.butSendQB.Name = "butSendQB";
			this.butSendQB.Size = new System.Drawing.Size(75, 20);
			this.butSendQB.TabIndex = 115;
			this.butSendQB.Text = "&Send QB";
			this.butSendQB.Click += new System.EventHandler(this.butSendQB_Click);
			// 
			// gridIns
			// 
			this.gridIns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridIns.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridIns.HasAddButton = false;
			this.gridIns.HasDropDowns = false;
			this.gridIns.HasMultilineHeaders = false;
			this.gridIns.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridIns.HeaderHeight = 15;
			this.gridIns.HScrollVisible = false;
			this.gridIns.Location = new System.Drawing.Point(8, 319);
			this.gridIns.Name = "gridIns";
			this.gridIns.ScrollValue = 0;
			this.gridIns.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridIns.Size = new System.Drawing.Size(584, 306);
			this.gridIns.TabIndex = 109;
			this.gridIns.Title = "Insurance Payments";
			this.gridIns.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridIns.TitleHeight = 18;
			this.gridIns.TranslationName = "TableDepositSlipIns";
			this.gridIns.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridIns_CellClick);
			this.gridIns.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridIns_MouseUp);
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(602, 661);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(75, 24);
			this.butPrint.TabIndex = 108;
			this.butPrint.Text = "&Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// textDate
			// 
			this.textDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textDate.Location = new System.Drawing.Point(602, 25);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(94, 20);
			this.textDate.TabIndex = 103;
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(7, 660);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(85, 24);
			this.butDelete.TabIndex = 101;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// gridPat
			// 
			this.gridPat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridPat.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridPat.HasAddButton = false;
			this.gridPat.HasDropDowns = false;
			this.gridPat.HasMultilineHeaders = false;
			this.gridPat.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridPat.HeaderHeight = 15;
			this.gridPat.HScrollVisible = false;
			this.gridPat.Location = new System.Drawing.Point(8, 12);
			this.gridPat.Name = "gridPat";
			this.gridPat.ScrollValue = 0;
			this.gridPat.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridPat.Size = new System.Drawing.Size(584, 299);
			this.gridPat.TabIndex = 100;
			this.gridPat.Title = "Patient Payments";
			this.gridPat.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridPat.TitleHeight = 18;
			this.gridPat.TranslationName = "TableDepositSlipPat";
			this.gridPat.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPat_CellClick);
			this.gridPat.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridPat_MouseUp);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(881, 631);
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
			this.butCancel.Location = new System.Drawing.Point(881, 661);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butPDF
			// 
			this.butPDF.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPDF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butPDF.Autosize = true;
			this.butPDF.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPDF.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPDF.CornerRadius = 4F;
			this.butPDF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPDF.Location = new System.Drawing.Point(683, 661);
			this.butPDF.Name = "butPDF";
			this.butPDF.Size = new System.Drawing.Size(75, 24);
			this.butPDF.TabIndex = 123;
			this.butPDF.TabStop = false;
			this.butPDF.Text = "Create PDF";
			this.butPDF.Click += new System.EventHandler(this.butPDF_Click);
			// 
			// butEmailPDF
			// 
			this.butEmailPDF.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEmailPDF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butEmailPDF.Autosize = true;
			this.butEmailPDF.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEmailPDF.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEmailPDF.CornerRadius = 4F;
			this.butEmailPDF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEmailPDF.Location = new System.Drawing.Point(764, 661);
			this.butEmailPDF.Name = "butEmailPDF";
			this.butEmailPDF.Size = new System.Drawing.Size(75, 24);
			this.butEmailPDF.TabIndex = 124;
			this.butEmailPDF.TabStop = false;
			this.butEmailPDF.Text = "Email PDF";
			this.butEmailPDF.Click += new System.EventHandler(this.butEmailPDF_Click);
			// 
			// labelDepositAccountNum
			// 
			this.labelDepositAccountNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelDepositAccountNum.Location = new System.Drawing.Point(602, 284);
			this.labelDepositAccountNum.Name = "labelDepositAccountNum";
			this.labelDepositAccountNum.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.labelDepositAccountNum.Size = new System.Drawing.Size(254, 14);
			this.labelDepositAccountNum.TabIndex = 126;
			this.labelDepositAccountNum.Text = "Auto Deposit Account";
			this.labelDepositAccountNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelDepositAccountNum.Visible = false;
			// 
			// comboDepositAccountNum
			// 
			this.comboDepositAccountNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboDepositAccountNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDepositAccountNum.Location = new System.Drawing.Point(602, 299);
			this.comboDepositAccountNum.MaxDropDownItems = 40;
			this.comboDepositAccountNum.Name = "comboDepositAccountNum";
			this.comboDepositAccountNum.Size = new System.Drawing.Size(289, 21);
			this.comboDepositAccountNum.TabIndex = 125;
			this.comboDepositAccountNum.Visible = false;
			// 
			// textBatch
			// 
			this.textBatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textBatch.Location = new System.Drawing.Point(601, 66);
			this.textBatch.MaxLength = 25;
			this.textBatch.Name = "textBatch";
			this.textBatch.Size = new System.Drawing.Size(290, 20);
			this.textBatch.TabIndex = 128;
			// 
			// labelBatchNum
			// 
			this.labelBatchNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelBatchNum.Location = new System.Drawing.Point(602, 49);
			this.labelBatchNum.Name = "labelBatchNum";
			this.labelBatchNum.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.labelBatchNum.Size = new System.Drawing.Size(237, 16);
			this.labelBatchNum.TabIndex = 127;
			this.labelBatchNum.Text = "Batch #";
			this.labelBatchNum.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// FormDepositEdit
			// 
			this.ClientSize = new System.Drawing.Size(974, 696);
			this.Controls.Add(this.textBatch);
			this.Controls.Add(this.labelBatchNum);
			this.Controls.Add(this.comboDepositAccountNum);
			this.Controls.Add(this.butEmailPDF);
			this.Controls.Add(this.butPDF);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.textItemNum);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textCheckNumSearch);
			this.Controls.Add(this.textAmountSearch);
			this.Controls.Add(this.textMemo);
			this.Controls.Add(this.labelMemo);
			this.Controls.Add(this.butSendQB);
			this.Controls.Add(this.textDepositAccount);
			this.Controls.Add(this.labelDepositAccount);
			this.Controls.Add(this.comboDepositAccount);
			this.Controls.Add(this.gridIns);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.textAmount);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textBankAccountInfo);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textDate);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.gridPat);
			this.Controls.Add(this.groupSelect);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.labelDepositAccountNum);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(959, 735);
			this.Name = "FormDepositEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Deposit Slip";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormDepositEdit_Closing);
			this.Load += new System.EventHandler(this.FormDepositEdit_Load);
			this.groupSelect.ResumeLayout(false);
			this.groupSelect.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormDepositEdit_Load(object sender,System.EventArgs e) {
			butSendQB.Visible=false;
			IsQuickBooks=PrefC.GetInt(PrefName.AccountingSoftware)==(int)AccountingSoftware.QuickBooks;
			if(IsNew) {
				if(!Security.IsAuthorized(Permissions.DepositSlips,DateTime.Today)) {
					//we will check the date again when saving
					DialogResult=DialogResult.Cancel;
					return;
				}
			}
			else {
				//We enforce security here based on date displayed, not date entered
				if(!Security.IsAuthorized(Permissions.DepositSlips,_depositCur.DateDeposit)) {
					butOK.Enabled=false;
					butDelete.Enabled=false;
				}
			}
			if(PrefC.GetBool(PrefName.ShowAutoDeposit)) {
				labelDepositAccountNum.Visible=true;
				comboDepositAccountNum.Visible=true;
				List<Def> listAutoDepositDefsAll=Defs.GetDefsForCategory(DefCat.AutoDeposit);
				//Fill deposit account num drop down
				comboDepositAccountNum.Items.Clear();
				foreach(Def defDepositAccount in listAutoDepositDefsAll.Where(x => !x.IsHidden)) {
					comboDepositAccountNum.Items.Add(new ODBoxItem<Def>(defDepositAccount.ItemName,defDepositAccount));
				}
				Def defAutoDeposit=listAutoDepositDefsAll.FirstOrDefault(x => x.DefNum==_depositCur.DepositAccountNum);
				comboDepositAccountNum.IndexSelectOrSetText(listAutoDepositDefsAll.FindIndex(x => x.DefNum==_depositCur.DepositAccountNum)
					,() => { return (defAutoDeposit!=null ? defAutoDeposit.ItemName+" "+Lan.g(this,"(hidden)") : ""); });
			}
			if(IsNew) {
				textDateStart.Text=PIn.Date(PrefC.GetString(PrefName.DateDepositsStarted)).ToShortDateString();
				if(PrefC.GetBool(PrefName.EasyNoClinics)) {
					comboClinic.Visible=false;
					labelClinic.Visible=false;
				}
				comboClinic.Items.Clear();
				comboClinic.Items.Add(Lan.g(this,"All"));
				comboClinic.SelectedIndex=0;
				_listClinics=Clinics.GetForUserod(Security.CurUser);
				for(int i=0;i<_listClinics.Count;i++) {
					comboClinic.Items.Add(_listClinics[i].Abbr);
					if(_listClinics[i].ClinicNum==Clinics.ClinicNum) {
						comboClinic.SelectedIndex=i+1;//Plus 1 to account for 'All'
					}
				}
				List<Def> listPaymentTypeDefs=Defs.GetDefsForCategory(DefCat.PaymentTypes,true);
				List<Def> listInsurancePaymentTypeDefs=Defs.GetDefsForCategory(DefCat.InsurancePaymentType,true);
				_payTypeDefNums=new List<long>();
				for(int i=0;i<listPaymentTypeDefs.Count;i++) {
					if(listPaymentTypeDefs[i].ItemValue!="") {
						continue;//skip defs not selected for deposit slip
					}
					listPayType.Items.Add(listPaymentTypeDefs[i].ItemName);
					_payTypeDefNums.Add(listPaymentTypeDefs[i].DefNum);
					listPayType.SetSelected(listPayType.Items.Count-1,true);
				}
				_insPayDefNums=new List<long>();
				for(int i=0;i<listInsurancePaymentTypeDefs.Count;i++) {
					if(listInsurancePaymentTypeDefs[i].ItemValue!="") {
						continue;//skip defs not selected for deposit slip
					}
					listInsPayType.Items.Add(listInsurancePaymentTypeDefs[i].ItemName);
					_insPayDefNums.Add(listInsurancePaymentTypeDefs[i].DefNum);
					listInsPayType.SetSelected(listInsPayType.Items.Count-1,true);
				}
				textDepositAccount.Visible=false;//this is never visible for new. It's a description if already attached.
				if(Accounts.DepositsLinked() && !IsQuickBooks) {
					DepositAccounts=Accounts.GetDepositAccounts();
					for(int i=0;i<DepositAccounts.Length;i++) {
						comboDepositAccount.Items.Add(Accounts.GetDescript(DepositAccounts[i]));
					}
					comboDepositAccount.SelectedIndex=0;
				}
				else {
					labelDepositAccount.Visible=false;
					comboDepositAccount.Visible=false;
				}
			}
			else {//Not new.
				groupSelect.Visible=false;
				gridIns.SelectionMode=GridSelectionMode.None;
				gridPat.SelectionMode=GridSelectionMode.None;
				//we never again let user change the deposit linking again from here.
				//They need to detach it from within the transaction
				//Might be enhanced later to allow, but that's very complex.
				Transaction trans=Transactions.GetAttachedToDeposit(_depositCur.DepositNum);
				if(trans==null) {
					labelDepositAccount.Visible=false;
					comboDepositAccount.Visible=false;
					textDepositAccount.Visible=false;
				}
				else {
					comboDepositAccount.Enabled=false;
					labelDepositAccount.Text=Lan.g(this,"Deposited into Account");
					List<JournalEntry> jeL=JournalEntries.GetForTrans(trans.TransactionNum);
					for(int i=0;i<jeL.Count;i++) {
						if(Accounts.GetAccount(jeL[i].AccountNum).AcctType==AccountType.Asset) {
							comboDepositAccount.Items.Add(Accounts.GetDescript(jeL[i].AccountNum));
							comboDepositAccount.SelectedIndex=0;
							textDepositAccount.Text=jeL[i].DateDisplayed.ToShortDateString()
								+" "+jeL[i].DebitAmt.ToString("c");
							break;
						}
					}
				}
			}
			if(IsQuickBooks) {//If in QuickBooks mode, hide dropdown because its handled in FormQBAccountSelect.cs.
				textDepositAccount.Visible=false;
				labelDepositAccount.Visible=false;
				comboDepositAccount.Visible=false;
				comboDepositAccount.Enabled=false;
				if(Accounts.DepositsLinked() && !IsNew) {
					//Show SendQB button so that users can send old deposits into QB.
					butSendQB.Visible=true;
				}
			}
			if(PrefC.GetBool(PrefName.QuickBooksClassRefsEnabled)) {
				if(!IsNew) {
					//Show groupbox and hide all the controls except for labelClassRef and comboClassRefs
					groupSelect.Visible=true;
					label5.Visible=false;
					textDateStart.Visible=false;
					labelClinic.Visible=false;
					comboClinic.Visible=false;
					label2.Visible=false;
					label6.Visible=false;
					listInsPayType.Visible=false;
					listPayType.Visible=false;
					butRefresh.Visible=false;
				}
				labelClassRef.Visible=true;
				comboClassRefs.Visible=true;
				string classStr=PrefC.GetString(PrefName.QuickBooksClassRefs);
				_arrayClassesQB=classStr.Split(new char[] { ',' });
				for(int i = 0;i<_arrayClassesQB.Length;i++) {
					if(_arrayClassesQB[i]=="") {
						continue;
					}
					comboClassRefs.Items.Add(_arrayClassesQB[i]);
				}
			}
			textDate.Text=_depositCur.DateDeposit.ToShortDateString();
			textAmount.Text=_depositCur.Amount.ToString("F");
			textBankAccountInfo.Text=_depositCur.BankAccountInfo;
			textMemo.Text=_depositCur.Memo;
			textBatch.Text=_depositCur.Batch;
			FillGrids();
			if(IsNew) {
				gridPat.SetSelected(true);
				gridIns.SetSelected(true);
			}
			ComputeAmt();
		}

		///<summary></summary>
		private void FillGrids(){
			if(IsNew){
				DateTime dateStart=PIn.Date(textDateStart.Text);
				long clinicNum=0;
				if(comboClinic.SelectedIndex!=0){
					clinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;
				}
				List<long> payTypes=new List<long>();//[listPayType.SelectedIndices.Count];
				for(int i=0;i<listPayType.SelectedIndices.Count;i++) {
					payTypes.Add(_payTypeDefNums[listPayType.SelectedIndices[i]]);
				}
				List<long> insPayTypes=new List<long>();
				for(int i=0;i<listInsPayType.SelectedIndices.Count;i++) {
					insPayTypes.Add(_insPayDefNums[listInsPayType.SelectedIndices[i]]);
				}
				PatPayList=new List<Payment>();
				if(payTypes.Count!=0) {
					PatPayList=Payments.GetForDeposit(dateStart,clinicNum,payTypes);
				}
				ClaimPayList=new ClaimPayment[0];
				if(insPayTypes.Count!=0) {
					ClaimPayList=ClaimPayments.GetForDeposit(dateStart,clinicNum,insPayTypes);
				}
				//new deposit, but has been saved to db (pressed print/PDF/email buttons), get trans already attached to deposit in db as well as unattached
				if(_hasBeenSavedToDB && _depositCur.DepositNum>0) {
					PatPayList=PatPayList.Concat(Payments.GetForDeposit(_depositCur.DepositNum))
						.OrderBy(x => x.PayDate).ThenBy(x => x.PayNum).ToList();
					ClaimPayList=ClaimPayList.Concat(ClaimPayments.GetForDeposit(_depositCur.DepositNum))
						.OrderBy(x => x.CheckDate).ThenBy(x => x.ClaimPaymentNum).ToArray();
				}
			}
			else{
				PatPayList=Payments.GetForDeposit(_depositCur.DepositNum);
				ClaimPayList=ClaimPayments.GetForDeposit(_depositCur.DepositNum);
			}
			//Fill Patient Payment Grid---------------------------------------
			List<long> patNums=new List<long>();
			for(int i=0;i<PatPayList.Count;i++){
				patNums.Add(PatPayList[i].PatNum);
			}
			Patient[] pats=Patients.GetMultPats(patNums);
			gridPat.BeginUpdate();
			gridPat.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableDepositSlipPat","Date"),80);
			gridPat.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDepositSlipPat","Patient"),150);
			gridPat.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDepositSlipPat","Type"),70);
			gridPat.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDepositSlipPat","Check Number"),95);
			gridPat.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDepositSlipPat","Bank-Branch"),80);
			gridPat.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDepositSlipPat","Amount"),80);
			gridPat.Columns.Add(col);
			gridPat.Rows.Clear();
			OpenDental.UI.ODGridRow row;
			for(int i=0;i<PatPayList.Count;i++){
				row=new OpenDental.UI.ODGridRow();
				row.Cells.Add(PatPayList[i].PayDate.ToShortDateString());
				row.Cells.Add(Patients.GetOnePat(pats,PatPayList[i].PatNum).GetNameLF());
				row.Cells.Add(Defs.GetName(DefCat.PaymentTypes,PatPayList[i].PayType));
				row.Cells.Add(PatPayList[i].CheckNum);
				row.Cells.Add(PatPayList[i].BankBranch);
				row.Cells.Add(PatPayList[i].PayAmt.ToString("F"));
				gridPat.Rows.Add(row);
			}
			gridPat.EndUpdate();
			//Fill Insurance Payment Grid-------------------------------------
			gridIns.BeginUpdate();
			gridIns.Columns.Clear();
			col=new ODGridColumn(Lan.g("TableDepositSlipIns","Date"),80);
			gridIns.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDepositSlipIns","Carrier"),150);
			gridIns.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDepositSlipIns","Type"),70);
			gridIns.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDepositSlipIns","Check Number"),95);
			gridIns.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDepositSlipIns","Bank-Branch"),80);
			gridIns.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDepositSlipIns","Amount"),90);
			gridIns.Columns.Add(col);
			gridIns.Rows.Clear();
			for(int i=0;i<ClaimPayList.Length;i++){
				row=new OpenDental.UI.ODGridRow();
				row.Cells.Add(ClaimPayList[i].CheckDate.ToShortDateString());
				row.Cells.Add(ClaimPayList[i].CarrierName);
				row.Cells.Add(Defs.GetName(DefCat.InsurancePaymentType,ClaimPayList[i].PayType));
				row.Cells.Add(ClaimPayList[i].CheckNum);
				row.Cells.Add(ClaimPayList[i].BankBranch);
				row.Cells.Add(ClaimPayList[i].CheckAmt.ToString("F"));
				gridIns.Rows.Add(row);
			}
			gridIns.EndUpdate();
		}

		///<summary>Usually run after any selected items changed. Recalculates amt based on selected items or row count.  May get fired twice when click
		///and mouse up, harmless.</summary>
		private void ComputeAmt(){
			if(IsNew) {
				textItemNum.Text=(gridIns.SelectedIndices.Length+gridPat.SelectedIndices.Length).ToString();
			}
			else {//if not new, amount cannot be changed, return
				textItemNum.Text=(gridIns.Rows.Count+gridPat.Rows.Count).ToString();
				return;
			}
			decimal amount=0;
			for(int i=0;i<gridPat.SelectedIndices.Length;i++){
				amount+=(decimal)PatPayList[gridPat.SelectedIndices[i]].PayAmt;
			}
			for(int i=0;i<gridIns.SelectedIndices.Length;i++){
				amount+=(decimal)ClaimPayList[gridIns.SelectedIndices[i]].CheckAmt;
			}
			textAmount.Text=amount.ToString("F");
			_depositCur.Amount=(double)amount;
		}

		private void Search() {
			bool isScrollSet=false;
			for(int i=0;i<gridIns.Rows.Count;i++) {
				bool isBold=false;
				if(textAmountSearch.Text!="" && gridIns.Rows[i].Cells[5].Text.ToUpper().Contains(textAmountSearch.Text.ToUpper())) {
					isBold=true;
				}
				if(textCheckNumSearch.Text!="" && gridIns.Rows[i].Cells[3].Text.ToUpper().Contains(textCheckNumSearch.Text.ToUpper())) {
					isBold=true;
				}
				gridIns.Rows[i].Bold=isBold;
				if(isBold) {
					gridIns.Rows[i].ColorText=Color.Red;					
					if(!isScrollSet) {//scroll to the first match in the list.
						gridIns.ScrollToIndex(i);
						isScrollSet=true;
					}
				}
				else {//Standard row.
					gridIns.Rows[i].ColorText=Color.Black;
				}
			}//end i
			gridIns.Invalidate();
			bool isScrollSetPat=false;
			for(int i=0;i<gridPat.Rows.Count;i++) {
				bool isBold=false;
				if(textAmountSearch.Text!="" && gridPat.Rows[i].Cells[5].Text.ToUpper().Contains(textAmountSearch.Text.ToUpper())) {
					isBold=true;
				}
				if(textCheckNumSearch.Text!="" && gridPat.Rows[i].Cells[3].Text.ToUpper().Contains(textCheckNumSearch.Text.ToUpper())) {
					isBold=true;
				}
				gridPat.Rows[i].Bold=isBold;
				if(isBold) {
					gridPat.Rows[i].ColorText=Color.Red;
					if(!isScrollSetPat) {//scroll to the first match in the list.
						gridPat.ScrollToIndex(i);
						isScrollSetPat=true;
					}
				}
				else {//Standard row.
					gridPat.Rows[i].ColorText=Color.Black;
				}
			}//end i
			gridPat.Invalidate();
		}

		///<summary>Returns true if a deposit was created OR if the user clicked continue anyway on pop up.</summary>
		private bool CreateDepositQB(bool allowContinue) {
			try {
				FormQBAccountSelect formQBAS = new FormQBAccountSelect();
				formQBAS.ShowDialog();
				if(formQBAS.DialogResult!=DialogResult.OK) {
					throw new ApplicationException(Lans.g(this,"Deposit accounts not selected")+".");
				}
				Cursor.Current=Cursors.WaitCursor;
				string classRef="";
				if(PrefC.GetBool(PrefName.QuickBooksClassRefsEnabled)) {
					classRef=comboClassRefs.SelectedItem.ToString();
				}
				QuickBooks.CreateDeposit(_depositCur.DateDeposit
					,formQBAS.DepositAccountSelected
					,formQBAS.IncomeAccountSelected
					,_depositCur.Amount
					,textMemo.Text
					,classRef);//if classRef=="" then it will be safely ignored here
				SecurityLogs.MakeLogEntry(Permissions.DepositSlips,0,Lan.g(this,"Deposit slip sent to QuickBooks.")+"\r\n"
					+Lan.g(this,"Deposit date")+": "+_depositCur.DateDeposit.ToShortDateString()+" "+Lan.g(this,"for")+" "+_depositCur.Amount.ToString("c"));
				Cursor.Current=Cursors.Default;
				MsgBox.Show(this,"Deposit successfully sent to QuickBooks.");
				butSendQB.Enabled=false;//Don't let user send same deposit more than once.  
			}
			catch(Exception ex) {
				Cursor.Current=Cursors.Default;
				if(allowContinue) {
					if(MessageBox.Show(ex.Message+"\r\n\r\n"
						+Lan.g(this,"A deposit has not been created in QuickBooks, continue anyway?")
						,Lan.g(this,"QuickBooks Deposit Create Failed")
						,MessageBoxButtons.YesNo)!=DialogResult.Yes)
					{
						return false;
					}
				}
				else {
					MessageBox.Show(ex.Message,Lan.g(this,"QuickBooks Deposit Create Failed"));
					return false;
				}
			}
			return true;
		}

		///<summary>Saves the selected rows to database.</summary>
		private bool SaveToDB(){
			if(textDate.errorProvider1.GetError(textDate)!=""){
				MsgBox.Show(this,"Please fix data entry errors first.");
				return false;
			}
			if(IsNew && gridPat.SelectedIndices.Length==0 && gridIns.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select at least one payment for this deposit first.");
				return false;
			}
			//Prevent backdating----------------------------------------------------------------------------------------
			DateTime date=PIn.Date(textDate.Text);
			//We enforce security here based on date displayed, not date entered
			if(!Security.IsAuthorized(Permissions.DepositSlips,date)) {
				return false;
			}
			_depositCur.DateDeposit=date;
			//amount already handled.
			_depositCur.BankAccountInfo=PIn.String(textBankAccountInfo.Text);
			_depositCur.Memo=PIn.String(textMemo.Text);
			_depositCur.Batch=PIn.String(textBatch.Text);
			if(comboDepositAccountNum.SelectedIndex > -1) {
				_depositCur.DepositAccountNum=((ODBoxItem<Def>)comboDepositAccountNum.SelectedItem).Tag.DefNum;
			}
			if(IsNew){
				if(gridPat.SelectedIndices.Length+gridIns.SelectedIndices.Length>18 && IsQuickBooks) {
					if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"No more than 18 items will fit on a QuickBooks deposit slip. Continue anyway?")) {
						return false;
					}
				}
				else if(gridPat.SelectedIndices.Length+gridIns.SelectedIndices.Length>32) {
					if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"No more than 32 items will fit on a deposit slip. Continue anyway?")) {
						return false;
					}
				}
			}
			//Check DB to see if payments have been linked to another deposit already.  Build list of currently selected PayNums
			List<long> listPayNums=gridPat.SelectedIndices.OfType<int>().Select(x => PatPayList[x].PayNum).ToList();
			if(listPayNums.Count>0) {
				int alreadyAttached=Payments.GetCountAttachedToDeposit(listPayNums,_depositCur.DepositNum);//Depositnum might be 0
				if(alreadyAttached>0) {
					MessageBox.Show(this,alreadyAttached+" "+Lan.g(this,"patient payments are already attached to another deposit")+".");
					//refresh
					return false;
				}
			}
			//Check DB to see if payments have been linked to another deposit already.  Build list of currently selected ClaimPaymentNums.
			List<long> listClaimPaymentNums=gridIns.SelectedIndices.OfType<int>().Select(x => ClaimPayList[x].ClaimPaymentNum).ToList();
			if(listClaimPaymentNums.Count>0) {
				int alreadyAttached=ClaimPayments.GetCountAttachedToDeposit(listClaimPaymentNums,_depositCur.DepositNum);//Depositnum might be 0
				if(alreadyAttached>0) {
					MessageBox.Show(this,alreadyAttached+" "+Lan.g(this,"insurance payments are already attached to another deposit")+".");
					//refresh
					return false;
				}
			}
			if(IsNew && !_hasBeenSavedToDB){
				if(Accounts.DepositsLinked() && _depositCur.Amount>0
					&& IsQuickBooks && !CreateDepositQB(true)) //Create a deposit in QuickBooks
				{
					return false;
				}
				Deposits.Insert(_depositCur);
				_depositOld=_depositCur.Copy();//fresh copy to old so if changes are made they will be saved to db
			}
			else{
				Deposits.Update(_depositCur,_depositOld);
				_depositOld=_depositCur.Copy();//fresh copy to old so if changes are made they will be saved to db
			}
			if(IsNew){//never allowed to change or attach more checks after initial creation of deposit slip
				for(int i=0;i<gridPat.SelectedIndices.Length;i++){
					Payment selectedPayment=PatPayList[gridPat.SelectedIndices[i]];
					selectedPayment.DepositNum=_depositCur.DepositNum;
					Payments.Update(selectedPayment,false);//This could be enhanced with a multi row update.
					if(!_isOnOKClick) {//Print/Create PDF
						if(!_listPayNumsAttached.Contains(selectedPayment.PayNum)) {
							_listPayNumsAttached.Add(selectedPayment.PayNum);//Add this payment to list to check when clicking OK.
						}
					}
					else {//OK Click
						_listPayNumsAttached.Remove(selectedPayment.PayNum);//Remove from the list because we don't need to detach.
					}
				}
				for(int i=0;i<gridIns.SelectedIndices.Length;i++){
					ClaimPayment selectedClaimPayment=ClaimPayList[gridIns.SelectedIndices[i]];
					selectedClaimPayment.DepositNum=_depositCur.DepositNum;
					ClaimPayments.Update(selectedClaimPayment);//This could be enhanced with a multi row update.
					if(!_isOnOKClick) {//Print/Create PDF
						if(!_listClaimPaymentNumAttached.Contains(selectedClaimPayment.ClaimPaymentNum)) {
							_listClaimPaymentNumAttached.Add(selectedClaimPayment.ClaimPaymentNum);//Add this payment to list to check when clicking OK.
						}
					}
					else {//OK Click
						_listClaimPaymentNumAttached.Remove(selectedClaimPayment.ClaimPaymentNum);//Remove from the list because we don't need to detach.
					}
				}
				if(_isOnOKClick && (_listPayNumsAttached.Count!=0 || _listClaimPaymentNumAttached.Count!=0)) {
					//Detach any payments or claimpayments that were attached in the DB but no longer selected.
					Deposits.DetachFromDeposit(_depositCur.DepositNum,_listPayNumsAttached,_listClaimPaymentNumAttached);
				}
			}
			_hasBeenSavedToDB=true;//So that we don't insert the deposit slip again when clicking Print or PDF or OK
			return true;
		}

		private void gridPat_CellClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			ComputeAmt();
		}

		private void gridPat_MouseUp(object sender,MouseEventArgs e) {
			ComputeAmt();
		}

		private void gridIns_CellClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			ComputeAmt();
		}

		private void gridIns_MouseUp(object sender,MouseEventArgs e) {
			ComputeAmt();
		}

		private void textCheckNumSearch_KeyUp(object sender,KeyEventArgs e) {
			Search();
		}

		private void textCheckNumSearch_MouseUp(object sender,MouseEventArgs e) {
			Search();
		}

		private void textAmountSearch_KeyUp(object sender,KeyEventArgs e) {
			Search();
		}

		private void textAmountSearch_MouseUp(object sender,MouseEventArgs e) {
			Search();
		}

		private void butSendQB_Click(object sender,EventArgs e) {
			DateTime date=PIn.Date(textDate.Text);//We use security on the date showing.
			if(!Security.IsAuthorized(Permissions.DepositSlips,date)) {
				return;
			}
			_depositCur.DateDeposit=date;
			CreateDepositQB(false);
		}

		private void butPrint_Click(object sender, System.EventArgs e) {
			if(textDate.errorProvider1.GetError(textDate)!="") {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(IsNew){
				if(!SaveToDB()) {
					return;
				}
			}
			else{//not new
				//Only allowed to change date and bank account info, NOT attached checks.
				//We enforce security here based on date displayed, not date entered.
				//If user is trying to change date without permission:
				DateTime date=PIn.Date(textDate.Text);
				if(Security.IsAuthorized(Permissions.DepositSlips,date,true)){
					if(!SaveToDB()) {
						return;
					}
				}
				//if security.NotAuthorized, then it simply skips the save process before printing
			}
			SheetDef sheetDef=null;
			List <SheetDef> depositSheetDefs=SheetDefs.GetCustomForType(SheetTypeEnum.DepositSlip);
			if(depositSheetDefs.Count>0){
				sheetDef=depositSheetDefs[0];
				SheetDefs.GetFieldsAndParameters(sheetDef);
			}
			else{
				sheetDef=SheetsInternal.GetSheetDef(SheetInternalType.DepositSlip);
			}
			Sheet sheet=SheetUtil.CreateSheet(sheetDef,0);
			SheetParameter.SetParameter(sheet,"DepositNum",_depositCur.DepositNum);
			SheetFiller.FillFields(sheet);
			SheetUtil.CalculateHeights(sheet);
			SheetPrinting.Print(sheet);
		}
		
		private void butPDF_Click(object sender,EventArgs e) {
			if(textDate.errorProvider1.GetError(textDate)!="") {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(IsNew){
				if(!SaveToDB()) {
					return;
				}
			}
			else{//not new
				//Only allowed to change date and bank account info, NOT attached checks.
				//We enforce security here based on date displayed, not date entered.
				//If user is trying to change date without permission:
				DateTime date=PIn.Date(textDate.Text);
				if(Security.IsAuthorized(Permissions.DepositSlips,date,true)){
					if(!SaveToDB()) {
						return;
					}
				}
				//if security.NotAuthorized, then it simply skips the save process before printing
			}
			SheetDef sheetDef=null;
			List <SheetDef> depositSheetDefs=SheetDefs.GetCustomForType(SheetTypeEnum.DepositSlip);
			if(depositSheetDefs.Count>0){
				sheetDef=depositSheetDefs[0];
				SheetDefs.GetFieldsAndParameters(sheetDef);
			}
			else{
				sheetDef=SheetsInternal.GetSheetDef(SheetInternalType.DepositSlip);
			}
			//The below mimics FormSheetFillEdit.butPDF_Click() and the above butPrint_Click().
			Sheet sheet=SheetUtil.CreateSheet(sheetDef,0);//Does not insert.
			SheetParameter.SetParameter(sheet,"DepositNum",_depositCur.DepositNum);
			SheetFiller.FillFields(sheet);
			SheetUtil.CalculateHeights(sheet);
			string filePathAndName=PrefC.GetRandomTempFile(".pdf");
			SheetPrinting.CreatePdf(sheet,filePathAndName,null);
			Process.Start(filePathAndName);
		}

		private void butEmailPDF_Click(object sender,EventArgs e) {
			if(textDate.errorProvider1.GetError(textDate)!="") {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(IsNew) {
				if(!SaveToDB()) {
					return;
				}
			}
			else {//not new
						//Only allowed to change date and bank account info, NOT attached checks.
						//We enforce security here based on date displayed, not date entered.
						//If user is trying to change date without permission:
				DateTime date = PIn.Date(textDate.Text);
				if(Security.IsAuthorized(Permissions.DepositSlips,date,true)) {
					if(!SaveToDB()) {
						return;
					}
				}
				//if security.NotAuthorized, then it simply skips the save process before printing
			}
			SheetDef sheetDef=null;
			List<SheetDef> listDepositSheetDefs=SheetDefs.GetCustomForType(SheetTypeEnum.DepositSlip);
			if(listDepositSheetDefs.Count>0) {
				sheetDef=listDepositSheetDefs[0];
				SheetDefs.GetFieldsAndParameters(sheetDef);
			}
			else {
				sheetDef=SheetsInternal.GetSheetDef(SheetInternalType.DepositSlip);
			}
			//The below mimics FormSheetFillEdit.butPDF_Click() and the above butPrint_Click().
			Sheet sheet=SheetUtil.CreateSheet(sheetDef,0);//Does not insert.
			SheetParameter.SetParameter(sheet,"DepositNum",_depositCur.DepositNum);
			SheetFiller.FillFields(sheet);
			SheetUtil.CalculateHeights(sheet);
			string sheetName=sheet.Description+"_"+DateTime.Now.ToString("yyyyMMdd_hhmmssfff")+".pdf";
			string tempFile=ODFileUtils.CombinePaths(PrefC.GetTempFolderPath(),sheetName);
			string filePathAndName=FileAtoZ.CombinePaths(EmailAttaches.GetAttachPath(),sheetName);
			SheetPrinting.CreatePdf(sheet,tempFile,null);
			FileAtoZ.Copy(tempFile,filePathAndName,FileAtoZSourceDestination.LocalToAtoZ);
			EmailMessage message=new EmailMessage();
			EmailAddress address=EmailAddresses.GetByClinic(Clinics.ClinicNum);
			message.FromAddress=address.GetFrom();
			message.Subject=sheet.Description;
			EmailAttach attach=new EmailAttach();
			attach.ActualFileName=sheetName;
			attach.DisplayedFileName=sheetName;
			message.Attachments.Add(attach);
			FormEmailMessageEdit FormE=new FormEmailMessageEdit(message,address);
			FormE.IsNew=true;
			FormE.ShowDialog();
		}

		///<summary>Remember that this can only happen if IsNew</summary>
		private void butRefresh_Click(object sender, System.EventArgs e) {
			if(textDateStart.errorProvider1.GetError(textDate)!=""){
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(listInsPayType.SelectedIndices.Count==0 && listPayType.SelectedIndices.Count==0) {
				for(int i=0;i<listInsPayType.Items.Count;i++) {
					listInsPayType.SetSelected(i,true);
				}
				for(int j=0;j<listPayType.Items.Count;j++) {
					listPayType.SetSelected(j,true);
				}
			}
			FillGrids();
			gridPat.SetSelected(true);
			gridIns.SetSelected(true);
			ComputeAmt();
			if(comboClinic.SelectedIndex==0){
				textBankAccountInfo.Text=PrefC.GetString(PrefName.PracticeBankNumber);
			}
			else{
				textBankAccountInfo.Text=_listClinics[comboClinic.SelectedIndex-1].BankNumber;
			}
			if(Prefs.UpdateString(PrefName.DateDepositsStarted,POut.Date(PIn.Date(textDateStart.Text),false))){
				changed=true;
			}
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(IsNew){
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(!MsgBox.Show(this,true,"Delete?")) {
				return;
			}
			//If deposit is attached to a transaction which is more than 48 hours old, then not allowed to delete.
			//This is hard coded.  User would have to delete or detach from within transaction rather than here.
			Transaction trans=Transactions.GetAttachedToDeposit(_depositCur.DepositNum);
			if(trans != null){
				if(trans.DateTimeEntry < MiscData.GetNowDateTime().AddDays(-2) ){
					MsgBox.Show(this,"Not allowed to delete.  This deposit is already attached to an accounting transaction.  You will need to detach it from within the accounting section of the program.");
					return;
				}
				if(Transactions.IsReconciled(trans)) {
					MsgBox.Show(this,"Not allowed to delete.  This deposit is attached to an accounting transaction that has been reconciled.  You will need to detach it from within the accounting section of the program.");
					return;
				}
				try{
					Transactions.Delete(trans);
				}
				catch(ApplicationException ex){
					MessageBox.Show(ex.Message);
					return;
				}
			}
			try {
				Deposits.Delete(_depositCur);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);//Already translated.
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			_isOnOKClick=true;
			if(!SaveToDB()){
				_isOnOKClick=false;
				return;
			}
			if(IsNew) {
				if(Accounts.DepositsLinked() && _depositCur.Amount>0 && !IsQuickBooks) {
					//create a transaction here
					Transaction trans=new Transaction();
					trans.DepositNum=_depositCur.DepositNum;
					trans.UserNum=Security.CurUser.UserNum;
					Transactions.Insert(trans);
					//first the deposit entry
					JournalEntry je=new JournalEntry();
					je.AccountNum=DepositAccounts[comboDepositAccount.SelectedIndex];
					je.CheckNumber=Lan.g(this,"DEP");
					je.DateDisplayed=_depositCur.DateDeposit;//it would be nice to add security here.
					je.DebitAmt=_depositCur.Amount;
					je.Memo=Lan.g(this,"Deposit");
					je.Splits=Accounts.GetDescript(PrefC.GetLong(PrefName.AccountingIncomeAccount));
					je.TransactionNum=trans.TransactionNum;
					JournalEntries.Insert(je);
					//then, the income entry
					je=new JournalEntry();
					je.AccountNum=PrefC.GetLong(PrefName.AccountingIncomeAccount);
					//je.CheckNumber=;
					je.DateDisplayed=_depositCur.DateDeposit;//it would be nice to add security here.
					je.CreditAmt=_depositCur.Amount;
					je.Memo=Lan.g(this,"Deposit");
					je.Splits=Accounts.GetDescript(DepositAccounts[comboDepositAccount.SelectedIndex]);
					je.TransactionNum=trans.TransactionNum;
					JournalEntries.Insert(je);
				}
				SecurityLogs.MakeLogEntry(Permissions.DepositSlips,0,_depositCur.DateDeposit.ToShortDateString()+" New "+_depositCur.Amount.ToString("c"));
			}
			else {//Not new
				SecurityLogs.MakeLogEntry(Permissions.AdjustmentEdit,0,_depositCur.DateDeposit.ToShortDateString()+" "+_depositCur.Amount.ToString("c"));
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			//Deletion and detaching payments is done on Closing.
			DialogResult=DialogResult.Cancel;
		}

		private void FormDepositEdit_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(IsNew && DialogResult==DialogResult.Cancel) {
				//User might have printed this, causing an insert into the DB.
				Deposits.Delete(_depositCur);//This will handle unattaching payments from this deposit. A Transaction should not have been made yet.
			}
			if(changed){
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}
	}
}





















