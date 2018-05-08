using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;
using CodeBase;

namespace OpenDental{
///<summary></summary>
	public class FormClaimPayEdit:ODForm {
		private OpenDental.ValidDouble textAmount;
		private OpenDental.ValidDate textDate;
		private System.Windows.Forms.TextBox textBankBranch;
		private System.Windows.Forms.TextBox textCheckNum;
		private System.Windows.Forms.TextBox textNote;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.ComponentModel.Container components = null;
		///<summary></summary>
		public bool IsNew;
		private System.Windows.Forms.ComboBox comboClinic;
		private System.Windows.Forms.Label labelClinic;
		private System.Windows.Forms.TextBox textCarrierName;
		private System.Windows.Forms.Label label7;
		private ClaimPayment ClaimPaymentCur;
		private Label labelDateIssued;
		private ValidDate textDateIssued;
		private UI.Button butCarrierSelect;
		private Label label8;
		private Label label9;
		private Label label10;
		private ComboBox comboPayType;
		private Label label11;
		private Label label1;
		private UI.Button butPickPaymentGroup;
		private ComboBox comboPayGroup;
		private Label labelClaimPaymentGroup;
		///<summary>List of defs of type ClaimPaymentGroup</summary>
		private List<Def> _listCPGroups;
		///<summary>Used to tell if a InsPayCreate log is necessary instead of a InsPayEdit log when IsNew is set to false.</summary>
		public bool IsCreateLogEntry;
		private Panel panelXcharge;
		private GroupBox groupPrepaid;
		private UI.Button butPayConnect;
		private List<Clinic> _listClinics;
		private GroupBox groupBoxDeposit;
		private ValidDate validDepositDate;
		private ValidDouble validDoubleDepositAmt;
		private Label labelDepositAmount;
		private Label labelDepositDate;
		private UI.Button butDepositEdit;
		private Label labelBatchNum;
		private TextBox textBoxBatchNum;
		private Label labelDepositAccountNum;
		private ComboBox comboDepositAccountNum;
		private List<Def> _listInsurancePaymentTypeDefs;
		///<summary>This is the deposit that was originally associated to the claimpayment OR is set to a deposit that came back from the Deposit Edit window via the Edit button.
		///Can be null if no deposit was associated to the claimpayment passed in or if the user deletes the deposit via the Edit window.</summary>
		private Deposit _depositOld;
		///<summary>Set to the value of PrefName.ShowAutoDeposit on load.</summary>
		private bool _hasAutoDeposit;
		private UI.Button butPaySimple;

		///<summary>Gets set to true when the user deletes the Deposit from the Edit Deposit window.</summary>
		private bool _IsAutoDepositDeleted;

		///<summary></summary>
		public FormClaimPayEdit(ClaimPayment claimPaymentCur) {
			InitializeComponent();// Required for Windows Form Designer support
			ClaimPaymentCur=claimPaymentCur;
			_depositOld=Deposits.GetOne(claimPaymentCur.DepositNum);
			Lan.F(this);
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClaimPayEdit));
			this.textAmount = new OpenDental.ValidDouble();
			this.textDate = new OpenDental.ValidDate();
			this.textBankBranch = new System.Windows.Forms.TextBox();
			this.textCheckNum = new System.Windows.Forms.TextBox();
			this.textNote = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.textCarrierName = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.labelDateIssued = new System.Windows.Forms.Label();
			this.textDateIssued = new OpenDental.ValidDate();
			this.label1 = new System.Windows.Forms.Label();
			this.butCarrierSelect = new OpenDental.UI.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.comboPayType = new System.Windows.Forms.ComboBox();
			this.label11 = new System.Windows.Forms.Label();
			this.butPickPaymentGroup = new OpenDental.UI.Button();
			this.comboPayGroup = new System.Windows.Forms.ComboBox();
			this.labelClaimPaymentGroup = new System.Windows.Forms.Label();
			this.panelXcharge = new System.Windows.Forms.Panel();
			this.groupPrepaid = new System.Windows.Forms.GroupBox();
			this.butPayConnect = new OpenDental.UI.Button();
			this.groupBoxDeposit = new System.Windows.Forms.GroupBox();
			this.labelDepositAccountNum = new System.Windows.Forms.Label();
			this.comboDepositAccountNum = new System.Windows.Forms.ComboBox();
			this.textBoxBatchNum = new System.Windows.Forms.TextBox();
			this.butDepositEdit = new OpenDental.UI.Button();
			this.labelBatchNum = new System.Windows.Forms.Label();
			this.labelDepositDate = new System.Windows.Forms.Label();
			this.validDepositDate = new OpenDental.ValidDate();
			this.labelDepositAmount = new System.Windows.Forms.Label();
			this.validDoubleDepositAmt = new OpenDental.ValidDouble();
			this.butPaySimple = new OpenDental.UI.Button();
			this.groupPrepaid.SuspendLayout();
			this.groupBoxDeposit.SuspendLayout();
			this.SuspendLayout();
			// 
			// textAmount
			// 
			this.textAmount.Location = new System.Drawing.Point(159, 135);
			this.textAmount.MaxVal = 100000000D;
			this.textAmount.MinVal = -100000000D;
			this.textAmount.Name = "textAmount";
			this.textAmount.Size = new System.Drawing.Size(68, 20);
			this.textAmount.TabIndex = 0;
			this.textAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textAmount.Leave += new System.EventHandler(this.TextAmount_Leave);
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(159, 93);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(68, 20);
			this.textDate.TabIndex = 6;
			this.textDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textBankBranch
			// 
			this.textBankBranch.Location = new System.Drawing.Point(159, 177);
			this.textBankBranch.MaxLength = 25;
			this.textBankBranch.Name = "textBankBranch";
			this.textBankBranch.Size = new System.Drawing.Size(100, 20);
			this.textBankBranch.TabIndex = 2;
			// 
			// textCheckNum
			// 
			this.textCheckNum.Location = new System.Drawing.Point(159, 156);
			this.textCheckNum.MaxLength = 25;
			this.textCheckNum.Name = "textCheckNum";
			this.textCheckNum.Size = new System.Drawing.Size(100, 20);
			this.textCheckNum.TabIndex = 1;
			// 
			// textNote
			// 
			this.textNote.Location = new System.Drawing.Point(159, 247);
			this.textNote.MaxLength = 255;
			this.textNote.Multiline = true;
			this.textNote.Name = "textNote";
			this.textNote.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textNote.Size = new System.Drawing.Size(335, 73);
			this.textNote.TabIndex = 4;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(12, 97);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(144, 16);
			this.label6.TabIndex = 37;
			this.label6.Text = "Payment Posting Date";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(37, 139);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(120, 16);
			this.label5.TabIndex = 36;
			this.label5.Text = "Amount";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(37, 158);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(119, 16);
			this.label4.TabIndex = 35;
			this.label4.Text = "Check #";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(37, 180);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(121, 16);
			this.label3.TabIndex = 34;
			this.label3.Text = "Bank-Branch";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(40, 248);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(118, 16);
			this.label2.TabIndex = 33;
			this.label2.Text = "Note";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(425, 519);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 9;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(334, 519);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 5;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(159, 21);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(209, 21);
			this.comboClinic.TabIndex = 0;
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(37, 25);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(118, 14);
			this.labelClinic.TabIndex = 91;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textCarrierName
			// 
			this.textCarrierName.Location = new System.Drawing.Point(159, 198);
			this.textCarrierName.MaxLength = 25;
			this.textCarrierName.Name = "textCarrierName";
			this.textCarrierName.Size = new System.Drawing.Size(263, 20);
			this.textCarrierName.TabIndex = 3;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(37, 201);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(121, 16);
			this.label7.TabIndex = 94;
			this.label7.Text = "Carrier Name";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelDateIssued
			// 
			this.labelDateIssued.Location = new System.Drawing.Point(15, 118);
			this.labelDateIssued.Name = "labelDateIssued";
			this.labelDateIssued.Size = new System.Drawing.Size(142, 16);
			this.labelDateIssued.TabIndex = 37;
			this.labelDateIssued.Text = "Check EFT Issue Date";
			this.labelDateIssued.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDateIssued
			// 
			this.textDateIssued.Location = new System.Drawing.Point(159, 114);
			this.textDateIssued.Name = "textDateIssued";
			this.textDateIssued.Size = new System.Drawing.Size(68, 20);
			this.textDateIssued.TabIndex = 7;
			this.textDateIssued.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(228, 115);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(94, 16);
			this.label1.TabIndex = 95;
			this.label1.Text = "(optional)";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butCarrierSelect
			// 
			this.butCarrierSelect.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCarrierSelect.Autosize = true;
			this.butCarrierSelect.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCarrierSelect.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCarrierSelect.CornerRadius = 4F;
			this.butCarrierSelect.Location = new System.Drawing.Point(425, 196);
			this.butCarrierSelect.Name = "butCarrierSelect";
			this.butCarrierSelect.Size = new System.Drawing.Size(69, 23);
			this.butCarrierSelect.TabIndex = 8;
			this.butCarrierSelect.Text = "Pick";
			this.butCarrierSelect.Click += new System.EventHandler(this.butCarrierSelect_Click);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(156, 221);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(200, 16);
			this.label8.TabIndex = 96;
			this.label8.Text = "(does not need to be exact)";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(262, 178);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(94, 16);
			this.label9.TabIndex = 97;
			this.label9.Text = "(optional)";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(262, 157);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(94, 16);
			this.label10.TabIndex = 98;
			this.label10.Text = "(optional)";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboPayType
			// 
			this.comboPayType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPayType.Location = new System.Drawing.Point(159, 44);
			this.comboPayType.MaxDropDownItems = 30;
			this.comboPayType.Name = "comboPayType";
			this.comboPayType.Size = new System.Drawing.Size(209, 21);
			this.comboPayType.TabIndex = 99;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(32, 47);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(118, 14);
			this.label11.TabIndex = 100;
			this.label11.Text = "Payment Type";
			this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butPickPaymentGroup
			// 
			this.butPickPaymentGroup.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickPaymentGroup.Autosize = false;
			this.butPickPaymentGroup.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickPaymentGroup.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickPaymentGroup.CornerRadius = 2F;
			this.butPickPaymentGroup.Location = new System.Drawing.Point(369, 67);
			this.butPickPaymentGroup.Name = "butPickPaymentGroup";
			this.butPickPaymentGroup.Size = new System.Drawing.Size(23, 21);
			this.butPickPaymentGroup.TabIndex = 103;
			this.butPickPaymentGroup.Text = "...";
			this.butPickPaymentGroup.Click += new System.EventHandler(this.butPickPaymentGroup_Click);
			// 
			// comboPayGroup
			// 
			this.comboPayGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPayGroup.Location = new System.Drawing.Point(159, 67);
			this.comboPayGroup.MaxDropDownItems = 40;
			this.comboPayGroup.Name = "comboPayGroup";
			this.comboPayGroup.Size = new System.Drawing.Size(209, 21);
			this.comboPayGroup.TabIndex = 102;
			// 
			// labelClaimPaymentGroup
			// 
			this.labelClaimPaymentGroup.Location = new System.Drawing.Point(12, 70);
			this.labelClaimPaymentGroup.Name = "labelClaimPaymentGroup";
			this.labelClaimPaymentGroup.Size = new System.Drawing.Size(143, 14);
			this.labelClaimPaymentGroup.TabIndex = 101;
			this.labelClaimPaymentGroup.Text = "Claim Payment Group";
			this.labelClaimPaymentGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panelXcharge
			// 
			this.panelXcharge.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelXcharge.BackgroundImage")));
			this.panelXcharge.Location = new System.Drawing.Point(23, 21);
			this.panelXcharge.Name = "panelXcharge";
			this.panelXcharge.Size = new System.Drawing.Size(59, 26);
			this.panelXcharge.TabIndex = 119;
			this.panelXcharge.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelXcharge_MouseClick);
			// 
			// groupPrepaid
			// 
			this.groupPrepaid.Controls.Add(this.butPaySimple);
			this.groupPrepaid.Controls.Add(this.butPayConnect);
			this.groupPrepaid.Controls.Add(this.panelXcharge);
			this.groupPrepaid.Location = new System.Drawing.Point(159, 326);
			this.groupPrepaid.Name = "groupPrepaid";
			this.groupPrepaid.Size = new System.Drawing.Size(335, 57);
			this.groupPrepaid.TabIndex = 120;
			this.groupPrepaid.TabStop = false;
			this.groupPrepaid.Text = "Virtual Credit Card Payment";
			// 
			// butPayConnect
			// 
			this.butPayConnect.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPayConnect.Autosize = false;
			this.butPayConnect.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPayConnect.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPayConnect.CornerRadius = 4F;
			this.butPayConnect.Location = new System.Drawing.Point(122, 21);
			this.butPayConnect.Name = "butPayConnect";
			this.butPayConnect.Size = new System.Drawing.Size(75, 24);
			this.butPayConnect.TabIndex = 130;
			this.butPayConnect.Text = "PayConnect";
			this.butPayConnect.Click += new System.EventHandler(this.butPayConnect_Click);
			// 
			// groupBoxDeposit
			// 
			this.groupBoxDeposit.Controls.Add(this.labelDepositAccountNum);
			this.groupBoxDeposit.Controls.Add(this.comboDepositAccountNum);
			this.groupBoxDeposit.Controls.Add(this.textBoxBatchNum);
			this.groupBoxDeposit.Controls.Add(this.butDepositEdit);
			this.groupBoxDeposit.Controls.Add(this.labelBatchNum);
			this.groupBoxDeposit.Controls.Add(this.labelDepositDate);
			this.groupBoxDeposit.Controls.Add(this.validDepositDate);
			this.groupBoxDeposit.Controls.Add(this.labelDepositAmount);
			this.groupBoxDeposit.Controls.Add(this.validDoubleDepositAmt);
			this.groupBoxDeposit.Location = new System.Drawing.Point(15, 389);
			this.groupBoxDeposit.Name = "groupBoxDeposit";
			this.groupBoxDeposit.Size = new System.Drawing.Size(493, 118);
			this.groupBoxDeposit.TabIndex = 121;
			this.groupBoxDeposit.TabStop = false;
			this.groupBoxDeposit.Text = "Deposit Details";
			// 
			// labelDepositAccountNum
			// 
			this.labelDepositAccountNum.Location = new System.Drawing.Point(6, 89);
			this.labelDepositAccountNum.Name = "labelDepositAccountNum";
			this.labelDepositAccountNum.Size = new System.Drawing.Size(137, 14);
			this.labelDepositAccountNum.TabIndex = 111;
			this.labelDepositAccountNum.Text = "Auto Deposit Account";
			this.labelDepositAccountNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboDepositAccountNum
			// 
			this.comboDepositAccountNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDepositAccountNum.Location = new System.Drawing.Point(144, 87);
			this.comboDepositAccountNum.MaxDropDownItems = 40;
			this.comboDepositAccountNum.Name = "comboDepositAccountNum";
			this.comboDepositAccountNum.Size = new System.Drawing.Size(209, 21);
			this.comboDepositAccountNum.TabIndex = 110;
			// 
			// textBoxBatchNum
			// 
			this.textBoxBatchNum.Location = new System.Drawing.Point(144, 62);
			this.textBoxBatchNum.MaxLength = 25;
			this.textBoxBatchNum.Name = "textBoxBatchNum";
			this.textBoxBatchNum.Size = new System.Drawing.Size(209, 20);
			this.textBoxBatchNum.TabIndex = 109;
			// 
			// butDepositEdit
			// 
			this.butDepositEdit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDepositEdit.Autosize = true;
			this.butDepositEdit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDepositEdit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDepositEdit.CornerRadius = 4F;
			this.butDepositEdit.Location = new System.Drawing.Point(410, 14);
			this.butDepositEdit.Name = "butDepositEdit";
			this.butDepositEdit.Size = new System.Drawing.Size(69, 23);
			this.butDepositEdit.TabIndex = 107;
			this.butDepositEdit.Text = "Edit";
			this.butDepositEdit.UseVisualStyleBackColor = true;
			this.butDepositEdit.Visible = false;
			this.butDepositEdit.Click += new System.EventHandler(this.butDepositEdit_Click);
			// 
			// labelBatchNum
			// 
			this.labelBatchNum.Location = new System.Drawing.Point(54, 63);
			this.labelBatchNum.Name = "labelBatchNum";
			this.labelBatchNum.Size = new System.Drawing.Size(87, 16);
			this.labelBatchNum.TabIndex = 108;
			this.labelBatchNum.Text = "Batch #";
			this.labelBatchNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelDepositDate
			// 
			this.labelDepositDate.Location = new System.Drawing.Point(60, 21);
			this.labelDepositDate.Name = "labelDepositDate";
			this.labelDepositDate.Size = new System.Drawing.Size(82, 16);
			this.labelDepositDate.TabIndex = 105;
			this.labelDepositDate.Text = "Date";
			this.labelDepositDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// validDepositDate
			// 
			this.validDepositDate.Location = new System.Drawing.Point(144, 20);
			this.validDepositDate.Name = "validDepositDate";
			this.validDepositDate.Size = new System.Drawing.Size(68, 20);
			this.validDepositDate.TabIndex = 102;
			this.validDepositDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelDepositAmount
			// 
			this.labelDepositAmount.Location = new System.Drawing.Point(57, 42);
			this.labelDepositAmount.Name = "labelDepositAmount";
			this.labelDepositAmount.Size = new System.Drawing.Size(84, 16);
			this.labelDepositAmount.TabIndex = 106;
			this.labelDepositAmount.Text = "Amount";
			this.labelDepositAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// validDoubleDepositAmt
			// 
			this.validDoubleDepositAmt.Enabled = false;
			this.validDoubleDepositAmt.Location = new System.Drawing.Point(144, 41);
			this.validDoubleDepositAmt.MaxVal = 100000000D;
			this.validDoubleDepositAmt.MinVal = -100000000D;
			this.validDoubleDepositAmt.Name = "validDoubleDepositAmt";
			this.validDoubleDepositAmt.Size = new System.Drawing.Size(68, 20);
			this.validDoubleDepositAmt.TabIndex = 104;
			this.validDoubleDepositAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// butPaySimple
			// 
			this.butPaySimple.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPaySimple.Autosize = false;
			this.butPaySimple.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPaySimple.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPaySimple.CornerRadius = 4F;
			this.butPaySimple.Location = new System.Drawing.Point(236, 21);
			this.butPaySimple.Name = "butPaySimple";
			this.butPaySimple.Size = new System.Drawing.Size(75, 24);
			this.butPaySimple.TabIndex = 131;
			this.butPaySimple.Text = "PaySimple";
			this.butPaySimple.Click += new System.EventHandler(this.butPaySimple_Click);
			// 
			// FormClaimPayEdit
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(520, 554);
			this.Controls.Add(this.groupBoxDeposit);
			this.Controls.Add(this.groupPrepaid);
			this.Controls.Add(this.butPickPaymentGroup);
			this.Controls.Add(this.comboPayGroup);
			this.Controls.Add(this.labelClaimPaymentGroup);
			this.Controls.Add(this.comboPayType);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textCarrierName);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.textAmount);
			this.Controls.Add(this.textDateIssued);
			this.Controls.Add(this.textDate);
			this.Controls.Add(this.textBankBranch);
			this.Controls.Add(this.textCheckNum);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.labelDateIssued);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butCarrierSelect);
			this.Controls.Add(this.butOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormClaimPayEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Insurance Payment";
			this.Load += new System.EventHandler(this.FormClaimPayEdit_Load);
			this.groupPrepaid.ResumeLayout(false);
			this.groupBoxDeposit.ResumeLayout(false);
			this.groupBoxDeposit.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormClaimPayEdit_Load(object sender, System.EventArgs e) {
			//ClaimPayment gets inserted into db when OK in this form if new
			if(IsNew){
				//security already checked before this form opens
			}
			else{
				textCheckNum.Select();//If new, then the amount would have been selected.
				if(!Security.IsAuthorized(Permissions.InsPayEdit,ClaimPaymentCur.CheckDate)){
					butOK.Enabled=false;
				}
			}
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				comboClinic.Visible=false;
				labelClinic.Visible=false;
			}
			else {
				_listClinics=Clinics.GetDeepCopy(true);
				comboClinic.Items.Add(Lan.g(this,"None"));
				comboClinic.SelectedIndex=0;
				for(int i=0;i<_listClinics.Count;i++) {
					comboClinic.Items.Add(_listClinics[i].Abbr);
					if(_listClinics[i].ClinicNum==ClaimPaymentCur.ClinicNum) {
						comboClinic.SelectedIndex=i+1;
					}
				}
			}
			_listInsurancePaymentTypeDefs=Defs.GetDefsForCategory(DefCat.InsurancePaymentType,true);
			for(int i=0;i<_listInsurancePaymentTypeDefs.Count;i++) {
				comboPayType.Items.Add(_listInsurancePaymentTypeDefs[i].ItemName);
				if(_listInsurancePaymentTypeDefs[i].DefNum==ClaimPaymentCur.PayType) {
					comboPayType.SelectedIndex=i;
				}
			}
			if(comboPayType.Items.Count > 0 && comboPayType.SelectedIndex < 0) {//There are InsurancePaymentTypes and none are selected.  Should never happen.
				comboPayType.SelectedIndex=0;//Select the first one in the list.
			}
			if(ClaimPaymentCur.CheckDate.Year>1880) {
				textDate.Text=ClaimPaymentCur.CheckDate.ToShortDateString();
			}
			if(ClaimPaymentCur.DateIssued.Year>1880) {
				textDateIssued.Text=ClaimPaymentCur.DateIssued.ToShortDateString();
			}
			textCheckNum.Text=ClaimPaymentCur.CheckNum;
			textBankBranch.Text=ClaimPaymentCur.BankBranch;
			textAmount.Text=ClaimPaymentCur.CheckAmt.ToString("F");
			textCarrierName.Text=ClaimPaymentCur.CarrierName;
			textNote.Text=ClaimPaymentCur.Note;
			_listCPGroups=Defs.GetDefsForCategory(DefCat.ClaimPaymentGroups,true);
			FillComboPaymentGroup(ClaimPaymentCur.PayGroup);
			CheckUIState();
			_hasAutoDeposit=PrefC.GetBool(PrefName.ShowAutoDeposit);
			FillAutoDepositDetails();
		}

		///<summary>Fills auto deposit group box for the attached deposit. Also handles the logic of hiding itself depending on the preference 'ShowAutoDeposit'.</summary>
		private void FillAutoDepositDetails() {
			//Do not show the Auto Deposit Details when users have the preference turned off and there is no deposit currently attached.
			if(_IsAutoDepositDeleted || (!_hasAutoDeposit && _depositOld==null)) {
				groupBoxDeposit.Visible=false;
				return;
			}
			//Alter the text on the group box if the deposit associated to this claim payment was an auto deposit.
			groupBoxDeposit.Text=Lan.g(this,"Auto Deposit Details");
			if(_depositOld!=null && _depositOld.DepositAccountNum==0) {
				groupBoxDeposit.Text=Lan.g(this,"Deposit Details");
			}
			//Fill deposit account num drop down
			comboDepositAccountNum.Items.Clear();
			List<Def> listAutoDepositDefsAll=Defs.GetDefsForCategory(DefCat.AutoDeposit);
			foreach(Def defDepositAccount in listAutoDepositDefsAll.Where(x => !x.IsHidden)) {
				comboDepositAccountNum.Items.Add(new ODBoxItem<Def>(defDepositAccount.ItemName,defDepositAccount));
			}
			//Auto deposit pref enabled and the Claim Payment IsNew or had its Auto Deposit deleted.
			if(_hasAutoDeposit && _depositOld==null) {
				//Prefill some fields within the auto deposit details from the insurance payment fields to be nice.
				if(string.IsNullOrWhiteSpace(validDepositDate.Text)) {
					validDepositDate.Text=DateTime.Now.ToShortDateString();
				}
				if(string.IsNullOrWhiteSpace(validDoubleDepositAmt.Text)) {
					validDoubleDepositAmt.Text=textAmount.Text;
				}
			}
			if(_depositOld!=null) {//check for an existing deposit
				//Disable all controls within the Auto Deposit Details group box except the Edit button.
				//Per Mark we don't want users to edit directly in this form, but rather open FormDepositEdit.
				foreach(Control ctrl in this.groupBoxDeposit.Controls) {
					if(ctrl==butDepositEdit) {
						continue;
					}
					ODException.SwallowAnyException(() => { ctrl.Enabled=(_depositOld==null); });
				}
			}
			//Any values within _depositOld should ALWAYS override claim payment values mainly because the user could have changed depositOld via the Edit button.
			if(_depositOld!=null) {
				butDepositEdit.Visible=true;
				validDepositDate.Text=_depositOld.DateDeposit.ToShortDateString();
				validDoubleDepositAmt.Text=_depositOld.Amount.ToString();
				textBoxBatchNum.Text=_depositOld.Batch;
				Def defAutoDeposit=listAutoDepositDefsAll.FirstOrDefault(x => x.DefNum==_depositOld.DepositAccountNum);
				comboDepositAccountNum.IndexSelectOrSetText(listAutoDepositDefsAll.FindIndex(x => x.DefNum==_depositOld.DepositAccountNum)
					,() => { return (defAutoDeposit!=null ? defAutoDeposit.ItemName+" "+Lan.g(this,"(hidden)") : ""); });
			}
		}

		///<summary>Returns a new deposit object from the UI values on the form.
		///Any values that do not have a UI in this window will be inherited from _depositOld if it is not null.</summary>
		private Deposit GetDepositCur() {
			Deposit depositCur=new Deposit();
			if(_depositOld!=null) {//Maintain the values of the existing deposit if it exists
				depositCur=_depositOld.Copy();
			}
			//Update UI values
			depositCur.Amount=PIn.Double(validDoubleDepositAmt.Text);
			depositCur.DateDeposit=PIn.Date(validDepositDate.Text);
			depositCur.Batch=PIn.String(textBoxBatchNum.Text);
			if(comboDepositAccountNum.SelectedIndex > -1) {
				depositCur.DepositAccountNum=((ODBoxItem<Def>)comboDepositAccountNum.SelectedItem).Tag.DefNum;
			}
			return depositCur;
		}

		///<summary>Mimics FormPayment.CheckUIState().</summary>
		private void CheckUIState() {
			Program progXcharge=Programs.GetCur(ProgramName.Xcharge);
			Program progPayConnect=Programs.GetCur(ProgramName.PayConnect);
			Program progPaySimple=Programs.GetCur(ProgramName.PaySimple);
			if(progXcharge==null || progPayConnect==null || progPaySimple==null) {//Should not happen.
				panelXcharge.Visible=(progXcharge!=null);
				butPayConnect.Visible=(progPayConnect!=null);
				butPaySimple.Visible=(progPaySimple!=null);
				groupPrepaid.Visible=(panelXcharge.Visible || butPayConnect.Visible || butPaySimple.Visible);
				return;
			}
			panelXcharge.Visible=false;
			butPayConnect.Visible=false;
			butPaySimple.Visible=false;
			if(!progPayConnect.Enabled && !progXcharge.Enabled && !progPaySimple.Enabled) {//if none enabled
				//show all so user can pick
				panelXcharge.Visible=true;
				butPayConnect.Visible=true;
				butPaySimple.Visible=true;
				groupPrepaid.Visible=true;
				return;
			}
			long clinicNum=GetClinicNumSelected();
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.PaymentTypes,true);
			//Show if enabled.  User could have all enabled.
			if(progPayConnect.Enabled) {
				//if clinics are disabled, PayConnect is enabled if marked enabled
				if(!PrefC.HasClinicsEnabled) {
					butPayConnect.Visible=true;
				}
				else {//if clinics are enabled, PayConnect is enabled if the PaymentType is valid and the Username and Password are not blank
					string paymentType=ProgramProperties.GetPropVal(progPayConnect.ProgramNum,"PaymentType",clinicNum);
					if(!string.IsNullOrEmpty(ProgramProperties.GetPropVal(progPayConnect.ProgramNum,"Username",clinicNum))
						&& !string.IsNullOrEmpty(ProgramProperties.GetPropVal(progPayConnect.ProgramNum,"Password",clinicNum))
						&& listDefs.Any(x => x.DefNum.ToString()==paymentType))
					{
						butPayConnect.Visible=true;
					}
				}
			}
			//show if enabled.  User could have both enabled.
			if(progXcharge.Enabled) {
				//if clinics are disabled, X-Charge is enabled if marked enabled
				if(!PrefC.HasClinicsEnabled) {
					panelXcharge.Visible=true;
				}
				else {//if clinics are enabled, X-Charge is enabled if the PaymentType is valid and the Username and Password are not blank
					string paymentType=ProgramProperties.GetPropVal(progXcharge.ProgramNum,"PaymentType",clinicNum);
					if(!string.IsNullOrEmpty(ProgramProperties.GetPropVal(progXcharge.ProgramNum,"Username",clinicNum))
						&& !string.IsNullOrEmpty(ProgramProperties.GetPropVal(progXcharge.ProgramNum,"Password",clinicNum))
						&& listDefs.Any(x => x.DefNum.ToString()==paymentType))
					{
						panelXcharge.Visible=true;
					}
				}
			}
			if(progPaySimple.Enabled) {
				//if clinics are disabled, PayConnect is enabled if marked enabled
				if(!PrefC.HasClinicsEnabled) {
					butPaySimple.Visible=true;
				}
				else {//if clinics are enabled, PayConnect is enabled if the PaymentType is valid and the Username and Password are not blank
					string paymentType=ProgramProperties.GetPropValForClinicOrDefault(progPaySimple.ProgramNum,PaySimple.PropertyDescs.PaySimplePayType,clinicNum);
					if(!string.IsNullOrEmpty(ProgramProperties.GetPropValForClinicOrDefault(progPaySimple.ProgramNum,PaySimple.PropertyDescs.PaySimpleApiUserName,clinicNum))
						&& !string.IsNullOrEmpty(ProgramProperties.GetPropValForClinicOrDefault(progPaySimple.ProgramNum,PaySimple.PropertyDescs.PaySimpleApiKey,clinicNum))
						&& listDefs.Any(x => x.DefNum.ToString()==paymentType))
					{
						butPaySimple.Visible=true;
					}
				}
			}
			groupPrepaid.Visible=(panelXcharge.Visible || butPayConnect.Visible || butPaySimple.Visible);
		}

		private long GetClinicNumSelected() {
			if(!PrefC.HasClinicsEnabled) {
				return 0;
			}
			if(comboClinic.SelectedIndex==0){
				return 0;
			}
			return _listClinics[comboClinic.SelectedIndex-1].ClinicNum;
		}

		private void butCarrierSelect_Click(object sender,EventArgs e) {
			CheckUIState();
			FormCarriers formC=new FormCarriers();
			formC.IsSelectMode=true;
			formC.ShowDialog();
			if(formC.DialogResult==DialogResult.OK) {
				textCarrierName.Text=formC.SelectedCarrier.CarrierName;
			}
		}

		private void FillComboPaymentGroup(long selectedDefNum=0) {
			comboPayGroup.Items.Clear();
			//If there are no claim payment group defs, hide the options per Nathan's request.
			if(_listCPGroups.Count==0) {
				comboPayGroup.Visible=false;
				butPickPaymentGroup.Visible=false;
				labelClaimPaymentGroup.Visible=false;
				return;
			}
			for(int i = 0;i<_listCPGroups.Count;i++) {
				Def defCur=_listCPGroups[i];
				comboPayGroup.Items.Add(defCur.ItemName);
				if(selectedDefNum==defCur.DefNum) {
					comboPayGroup.SelectedIndex=i;
				}
			}
			if(selectedDefNum==0) {
				comboPayGroup.SelectedIndex=0; //there should always be one selected.
			}
		}

		///<summary>Used to keep the Claim Payment amount and Auto Deposit amount in sync. They should always match when creating a new Claim Payment.</summary>
		private void TextAmount_Leave(object sender,EventArgs e) {
				validDoubleDepositAmt.Text=textAmount.Text;
		}

		private void butDepositEdit_Click(object sender,EventArgs e) {
			//The user may have edited fields before hitting the "Edit" button.
			FormDepositEdit formDE=new FormDepositEdit(GetDepositCur());
			formDE.ShowDialog();
			if(formDE.DialogResult==DialogResult.OK) {//Made changes, update deposit values
				//Get the deposit associated to our claimpayment and update this form with it.
				_depositOld=Deposits.GetOne(ClaimPaymentCur.DepositNum);
				//User deleted the deposit so dissaociate it from the claim payment.
				if(_depositOld==null) {
					ClaimPaymentCur.DepositNum=0;
					_IsAutoDepositDeleted=true;
				}
				FillAutoDepositDetails();
			}
		}

		private void butPickPaymentGroup_Click(object sender,EventArgs e) {
			FormDefinitionPicker FormDP=new FormDefinitionPicker(DefCat.ClaimPaymentGroups);
			FormDP.ShowDialog();
			if(FormDP.DialogResult==DialogResult.OK) {
				FillComboPaymentGroup(FormDP.ListSelectedDefs[0].DefNum);
			}
		}

		///<summary>The contents of this event mimic FormPayment.panelXcharge_MouseClick().</summary>
		private void panelXcharge_MouseClick(object sender,MouseEventArgs e) {
			if(e.Button != MouseButtons.Left) {
				return;
			}
			if(textAmount.Text=="" || PIn.Double(textAmount.Text)==0) {
				MsgBox.Show(this,"Please enter an amount first.");
				textAmount.Focus();
				return;
			}
			Payment pay=new Payment();
			pay.ClinicNum=GetClinicNumSelected();
			FormPayment form=new FormPayment(new Patient(),new Family(),pay,false);
			try {
				string tranDetail=form.MakeXChargeTransaction(PIn.Double(textAmount.Text));
				if(tranDetail!=null) {
					if(textNote.Text!="") {
						textNote.Text+="\r\n";
					}
					textNote.Text+=tranDetail;
				}
			}
			catch(Exception ex) {
				FriendlyException.Show(Lan.g(this,"Error processing transaction.\r\n\r\nPlease contact support with the details of this error:")
					//The rest of the message is not translated on purpose because we here at HQ need to always be able to quickly read this part.
					+"\r\nLast valid milestone reached: "+form.XchargeMilestone,ex);
			}
		}

		private void butPayConnect_Click(object sender,EventArgs e) {
			if(textAmount.Text=="" || PIn.Double(textAmount.Text)==0) {
				MsgBox.Show(this,"Please enter an amount first.");
				textAmount.Focus();
				return;
			}
			Payment pay=new Payment();
			pay.ClinicNum=GetClinicNumSelected();
			FormPayment form=new FormPayment(new Patient(),new Family(),pay,false);
			string tranDetail=form.MakePayConnectTransaction(PIn.Double(textAmount.Text));
			if(tranDetail!=null) {
				if(textNote.Text!="") {
					textNote.Text+="\r\n";
				}
				textNote.Text+=tranDetail;
			}
		}

		private void butPaySimple_Click(object sender,EventArgs e) {
			if(textAmount.Text=="" || PIn.Double(textAmount.Text)==0) {
				MsgBox.Show(this,"Please enter an amount first.");
				textAmount.Focus();
				return;
			}
			Payment pay=new Payment();
			pay.ClinicNum=GetClinicNumSelected();
			FormPayment form=new FormPayment(new Patient(),new Family(),pay,false);
			string tranDetail=form.MakePaySimpleTransaction(PIn.Double(textAmount.Text));
			if(tranDetail!=null) {
				if(textNote.Text!="") {
					textNote.Text+="\r\n";
				}
				textNote.Text+=tranDetail;
			}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textDate.Text=="") {
				MsgBox.Show(this,"Please enter a date.");
				return;
			}
			if(PIn.Date(textDate.Text) > DateTime.Today
				&& !PrefC.GetBool(PrefName.FutureTransDatesAllowed) 
				&& !PrefC.GetBool(PrefName.AllowFutureInsPayments)) 
			{
				MsgBox.Show(this,"Payment date cannot be in the future.");
				return;
			}
			if(textCarrierName.Text=="") {
				MsgBox.Show(this,"Please enter a carrier.");
				return;
			}
			if(textDate.errorProvider1.GetError(textDate)!="" 
				|| textAmount.errorProvider1.GetError(textAmount)!=""
				|| textDateIssued.errorProvider1.GetError(textDateIssued)!=""
				|| validDepositDate.errorProvider1.GetError(validDepositDate)!=""
				|| validDoubleDepositAmt.errorProvider1.GetError(validDoubleDepositAmt)!="")
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(!PrefC.GetBool(PrefName.AllowFutureInsPayments) && PIn.Date(textDate.Text).Date>MiscData.GetNowDateTime().Date) {
				MsgBox.Show(this,"Insurance Payment Date must not be a future date.");
				return;
			}
			if(textAmount.Text=="") {
				textAmount.Text="0.00";
				return;
			}
			//Insert if the claim payment is new or if the claim payment does not have an attached Auto Deposit.
			if(!_IsAutoDepositDeleted && _hasAutoDeposit && _depositOld==null) {
				//Insert the deposit, this must happen first as the claimpayment FK's to deposit.
				//The deposit cannot be updated in this form, that is handled by the edit button.
				Deposit depositCur=GetDepositCur();
				//Double check that there is a valid date entered into the form before trying to insert the deposit into the database.
				if(depositCur.DateDeposit.Year < 1880) {
					MsgBox.Show(this,"Please enter a valid Auto Deposit Date.");
					return;
				}
				if(!Security.IsAuthorized(Permissions.DepositSlips,depositCur.DateDeposit)) {
					return;
				}
				ClaimPaymentCur.DepositNum=Deposits.Insert(depositCur);
				SecurityLogs.MakeLogEntry(Permissions.DepositSlips,0
					,Lan.g(this,"Auto Deposit created via the Edit Insurance Payment window:")+" "+depositCur.DateDeposit.ToShortDateString()
					+" "+Lan.g(this,"New")+" "+depositCur.Amount.ToString("c"));
			}
			double amt=PIn.Double(textAmount.Text);
			if(IsNew){
				//prevents backdating of initial check
				if(!Security.IsAuthorized(Permissions.InsPayCreate,PIn.Date(textDate.Text))){
					return;
				}
				//prevents attaching claimprocs with a date that is older than allowed by security.
			}
			else{
				//Editing an old entry will already be blocked if the date was too old, and user will not be able to click OK button.
				//This catches it if user changed the date to be older.
				if(!Security.IsAuthorized(Permissions.InsPayEdit,PIn.Date(textDate.Text))){
					return;
				}
				//Check that the attached payments match the payment amount.
				List<ClaimPaySplit> listClaimPaySplit=Claims.GetAttachedToPayment(ClaimPaymentCur.ClaimPaymentNum);
				double insPayTotal=0;
				for(int i=0; i<listClaimPaySplit.Count; i++) {
					insPayTotal+=listClaimPaySplit[i].InsPayAmt;
				}
				if(!insPayTotal.IsEqual(amt)) {
					if(MsgBox.Show(this,MsgBoxButtons.OKCancel,"Amount entered does not match Total Payments attached.\r\n"
						+"If you choose to continue, this insurance payment will be flagged as a partial payment, which will affect reports.\r\n"
						+"Click OK to continue, or click Cancel to edit Amount."))
					{
						ClaimPaymentCur.IsPartial=true;
					}
					else {
						//FormClaimPayBatch will set IsPartial back to false.
						return;
					}
				}
			}
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				if(comboClinic.SelectedIndex==0){
					ClaimPaymentCur.ClinicNum=0;
				}
				else{
					ClaimPaymentCur.ClinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;
				}
			}
			ClaimPaymentCur.PayType=(Defs.GetDefsForCategory(DefCat.InsurancePaymentType,true)[comboPayType.SelectedIndex].DefNum);
			if(comboPayGroup.SelectedIndex>0) {//If they didn't select anything, leave what was originally there
				ClaimPaymentCur.PayGroup=_listCPGroups[comboPayGroup.SelectedIndex].DefNum;
			}
			ClaimPaymentCur.CheckDate=PIn.Date(textDate.Text);
			ClaimPaymentCur.DateIssued=PIn.Date(textDateIssued.Text);
			ClaimPaymentCur.CheckAmt=PIn.Double(textAmount.Text);
			ClaimPaymentCur.CheckNum=textCheckNum.Text;
			ClaimPaymentCur.BankBranch=textBankBranch.Text;
			ClaimPaymentCur.CarrierName=textCarrierName.Text;
			ClaimPaymentCur.Note=textNote.Text;
			try{
				if(IsNew) {
					ClaimPayments.Insert(ClaimPaymentCur);//error thrown if trying to change amount and already attached to a deposit.
					SecurityLogs.MakeLogEntry(Permissions.InsPayCreate,0,
						Lan.g(this,"Carrier Name: ")+ClaimPaymentCur.CarrierName+", "
						+Lan.g(this,"Total Amount: ")+ClaimPaymentCur.CheckAmt.ToString("c")+", "
						+Lan.g(this,"Check Date: ")+ClaimPaymentCur.CheckDate.ToShortDateString()+", "//Date the check is entered in the system (i.e. today)
						+"ClaimPaymentNum: "+ClaimPaymentCur.ClaimPaymentNum);//Column name, not translated.
				}
				else {
					ClaimPayments.Update(ClaimPaymentCur);//error thrown if trying to change amount and already attached to a deposit.
					if(IsCreateLogEntry) { //need a InsPayCreate Log entry because it just was pre-inserted.
						SecurityLogs.MakeLogEntry(Permissions.InsPayCreate,0,
							Lan.g(this,"Carrier Name: ")+ClaimPaymentCur.CarrierName+", "
							+Lan.g(this,"Total Amount: ")+ClaimPaymentCur.CheckAmt.ToString("c")+", "
							+Lan.g(this,"Check Date: ")+ClaimPaymentCur.CheckDate.ToShortDateString()+", "//Date the check is entered in the system (i.e. today)
							+"ClaimPaymentNum: "+ClaimPaymentCur.ClaimPaymentNum);//Column name, not translated.
					}
					else {
						SecurityLogs.MakeLogEntry(Permissions.InsPayEdit,0,
							Lan.g(this,"Carrier Name: ")+ClaimPaymentCur.CarrierName+", "
							+Lan.g(this,"Total Amount: ")+ClaimPaymentCur.CheckAmt.ToString("c")+", "
							+Lan.g(this,"Check Date: ")+ClaimPaymentCur.CheckDate.ToShortDateString()+", "//Date the check is entered in the system
							+"ClaimPaymentNum: "+ClaimPaymentCur.ClaimPaymentNum);//Column name, not translated.
					}
				}
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
				return;
			}
			ClaimProcs.SynchDateCP(ClaimPaymentCur.ClaimPaymentNum,ClaimPaymentCur.CheckDate);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}

}