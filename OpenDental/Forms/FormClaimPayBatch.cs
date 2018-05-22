using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;

namespace OpenDental{
///<summary></summary>
	public class FormClaimPayBatch:ODForm {
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
		private OpenDental.UI.Button butClose;
		private IContainer components=null;
		//private bool ControlDown;
		///<summary></summary>
		public bool IsNew;
		private OpenDental.UI.Button butDelete;
		///<summary>The list of splits to display in the grid.</summary>
		private System.Windows.Forms.Label labelClinic;
		private System.Windows.Forms.TextBox textCarrierName;
		private System.Windows.Forms.Label label7;
		private ClaimPayment ClaimPaymentCur;
		private OpenDental.UI.ODGrid gridAttached;
		private ValidDate textDateIssued;
		private Label labelDateIssued;
		private TextBox textClinic;
		private GroupBox groupBox1;
		private UI.Button butClaimPayEdit;
		private ODGrid gridOut;
		List<ClaimPaySplit> ClaimsAttached;
		List<ClaimPaySplit> ClaimsOutstanding;
		private UI.Button butDetach;
		private ValidDouble textTotal;
		private Label label8;
		private Label labelInstruct1;
		private UI.Button butDown;
		private UI.Button butUp;
		private Label labelInstruct2;
		private ContextMenu menuRightAttached;
		private MenuItem menuItemGotoAccount;
		private ContextMenu menuRightOut;
		private MenuItem menuItemGotoOut;
		private bool IsDeleting;
		///<summary>If this is not zero upon closing, then we will jump to the account module of that patient and highlight the claim.</summary>
		public long GotoClaimNum;
		///<summary>If this is not zero upon closing, then we will jump to the account module of that patient and highlight the claim.</summary>
		public long GotoPatNum;
		private UI.Button butOK;
		private Label label1;
		private UI.Button butView;
		private TextBox textEobIsScanned;
		private UI.Button butAttach;
		private TextBox textCarrier;
		private Label label9;
		private UI.Button butRefresh;
		private Label label10;
		private TextBox textPayType;
		private TextBox textClaimID;
		private TextBox textName;
		private Label label11;
		private Label label12;
		private GroupBox groupFilters;
		private Label label13;
		private TextBox textPayGroup;

		///<summary>Set to true if the batch list was accessed originally by going through a claim.  This disables the GotoAccount feature.  It also causes OK/Cancel buttons to show so that user can cancel out of a brand new check creation.</summary>
		public bool IsFromClaim;

		///<summary></summary>
		public FormClaimPayBatch(ClaimPayment claimPaymentCur) {
			InitializeComponent();// Required for Windows Form Designer support
			ClaimPaymentCur=claimPaymentCur;
			gridAttached.ContextMenu=menuRightAttached;
			gridOut.ContextMenu=menuRightOut;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClaimPayBatch));
			this.menuRightAttached = new System.Windows.Forms.ContextMenu();
			this.menuItemGotoAccount = new System.Windows.Forms.MenuItem();
			this.menuRightOut = new System.Windows.Forms.ContextMenu();
			this.menuItemGotoOut = new System.Windows.Forms.MenuItem();
			this.groupFilters = new System.Windows.Forms.GroupBox();
			this.textCarrier = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.textName = new System.Windows.Forms.TextBox();
			this.textClaimID = new System.Windows.Forms.TextBox();
			this.butRefresh = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.textTotal = new OpenDental.ValidDouble();
			this.butAttach = new OpenDental.UI.Button();
			this.textEobIsScanned = new System.Windows.Forms.TextBox();
			this.butView = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.butOK = new OpenDental.UI.Button();
			this.labelInstruct2 = new System.Windows.Forms.Label();
			this.butUp = new OpenDental.UI.Button();
			this.labelInstruct1 = new System.Windows.Forms.Label();
			this.butDetach = new OpenDental.UI.Button();
			this.gridOut = new OpenDental.UI.ODGrid();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label13 = new System.Windows.Forms.Label();
			this.textPayGroup = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.textPayType = new System.Windows.Forms.TextBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.textDateIssued = new OpenDental.ValidDate();
			this.label2 = new System.Windows.Forms.Label();
			this.labelDateIssued = new System.Windows.Forms.Label();
			this.butClaimPayEdit = new OpenDental.UI.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textClinic = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textCarrierName = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.textNote = new System.Windows.Forms.TextBox();
			this.textCheckNum = new System.Windows.Forms.TextBox();
			this.textBankBranch = new System.Windows.Forms.TextBox();
			this.textAmount = new OpenDental.ValidDouble();
			this.textDate = new OpenDental.ValidDate();
			this.gridAttached = new OpenDental.UI.ODGrid();
			this.butDelete = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.groupFilters.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuRightAttached
			// 
			this.menuRightAttached.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemGotoAccount});
			// 
			// menuItemGotoAccount
			// 
			this.menuItemGotoAccount.Index = 0;
			this.menuItemGotoAccount.Text = "Go to Account";
			this.menuItemGotoAccount.Click += new System.EventHandler(this.menuItemGotoAccount_Click);
			// 
			// menuRightOut
			// 
			this.menuRightOut.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemGotoOut});
			// 
			// menuItemGotoOut
			// 
			this.menuItemGotoOut.Index = 0;
			this.menuItemGotoOut.Text = "Go to Account";
			this.menuItemGotoOut.Click += new System.EventHandler(this.menuItemGotoOut_Click);
			// 
			// groupFilters
			// 
			this.groupFilters.Controls.Add(this.textCarrier);
			this.groupFilters.Controls.Add(this.label12);
			this.groupFilters.Controls.Add(this.label9);
			this.groupFilters.Controls.Add(this.label11);
			this.groupFilters.Controls.Add(this.textName);
			this.groupFilters.Controls.Add(this.textClaimID);
			this.groupFilters.Location = new System.Drawing.Point(230, 367);
			this.groupFilters.Name = "groupFilters";
			this.groupFilters.Size = new System.Drawing.Size(732, 45);
			this.groupFilters.TabIndex = 119;
			this.groupFilters.TabStop = false;
			this.groupFilters.Text = "Filters";
			// 
			// textCarrier
			// 
			this.textCarrier.Location = new System.Drawing.Point(101, 15);
			this.textCarrier.Name = "textCarrier";
			this.textCarrier.Size = new System.Drawing.Size(132, 20);
			this.textCarrier.TabIndex = 112;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(235, 17);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(106, 16);
			this.label12.TabIndex = 118;
			this.label12.Text = "Name";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(6, 17);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(95, 16);
			this.label9.TabIndex = 113;
			this.label9.Text = "Carrier";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(479, 16);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(103, 16);
			this.label11.TabIndex = 117;
			this.label11.Text = "Claim ID";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textName
			// 
			this.textName.Location = new System.Drawing.Point(341, 15);
			this.textName.Name = "textName";
			this.textName.Size = new System.Drawing.Size(132, 20);
			this.textName.TabIndex = 113;
			// 
			// textClaimID
			// 
			this.textClaimID.Location = new System.Drawing.Point(582, 15);
			this.textClaimID.Name = "textClaimID";
			this.textClaimID.Size = new System.Drawing.Size(132, 20);
			this.textClaimID.TabIndex = 114;
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butRefresh.Location = new System.Drawing.Point(479, 341);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(54, 24);
			this.butRefresh.TabIndex = 114;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(271, 341);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(39, 24);
			this.butDown.TabIndex = 104;
			this.butDown.Text = "#";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// textTotal
			// 
			this.textTotal.Location = new System.Drawing.Point(863, 344);
			this.textTotal.MaxVal = 100000000D;
			this.textTotal.MinVal = -100000000D;
			this.textTotal.Name = "textTotal";
			this.textTotal.ReadOnly = true;
			this.textTotal.Size = new System.Drawing.Size(81, 20);
			this.textTotal.TabIndex = 0;
			this.textTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// butAttach
			// 
			this.butAttach.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAttach.Autosize = true;
			this.butAttach.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAttach.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAttach.CornerRadius = 4F;
			this.butAttach.Image = global::OpenDental.Properties.Resources.up;
			this.butAttach.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAttach.Location = new System.Drawing.Point(539, 341);
			this.butAttach.Name = "butAttach";
			this.butAttach.Size = new System.Drawing.Size(71, 24);
			this.butAttach.TabIndex = 111;
			this.butAttach.Text = "Attach";
			this.butAttach.Click += new System.EventHandler(this.butAttach_Click);
			// 
			// textEobIsScanned
			// 
			this.textEobIsScanned.Location = new System.Drawing.Point(145, 580);
			this.textEobIsScanned.MaxLength = 25;
			this.textEobIsScanned.Name = "textEobIsScanned";
			this.textEobIsScanned.ReadOnly = true;
			this.textEobIsScanned.Size = new System.Drawing.Size(72, 20);
			this.textEobIsScanned.TabIndex = 110;
			// 
			// butView
			// 
			this.butView.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butView.Autosize = true;
			this.butView.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butView.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butView.CornerRadius = 4F;
			this.butView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butView.Location = new System.Drawing.Point(145, 606);
			this.butView.Name = "butView";
			this.butView.Size = new System.Drawing.Size(72, 24);
			this.butView.TabIndex = 109;
			this.butView.Text = "View EOB";
			this.butView.Click += new System.EventHandler(this.butView_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(21, 583);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(123, 16);
			this.label1.TabIndex = 108;
			this.label1.Text = "EOB is Scanned";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(805, 646);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 107;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// labelInstruct2
			// 
			this.labelInstruct2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelInstruct2.Location = new System.Drawing.Point(10, 27);
			this.labelInstruct2.Name = "labelInstruct2";
			this.labelInstruct2.Size = new System.Drawing.Size(207, 523);
			this.labelInstruct2.TabIndex = 105;
			this.labelInstruct2.Text = resources.GetString("labelInstruct2.Text");
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(230, 341);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(39, 24);
			this.butUp.TabIndex = 103;
			this.butUp.Text = "#";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// labelInstruct1
			// 
			this.labelInstruct1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelInstruct1.Location = new System.Drawing.Point(9, 1);
			this.labelInstruct1.Name = "labelInstruct1";
			this.labelInstruct1.Size = new System.Drawing.Size(177, 20);
			this.labelInstruct1.TabIndex = 102;
			this.labelInstruct1.Text = "Instructions";
			this.labelInstruct1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butDetach
			// 
			this.butDetach.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDetach.Autosize = true;
			this.butDetach.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDetach.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDetach.CornerRadius = 4F;
			this.butDetach.Image = global::OpenDental.Properties.Resources.down;
			this.butDetach.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDetach.Location = new System.Drawing.Point(612, 341);
			this.butDetach.Name = "butDetach";
			this.butDetach.Size = new System.Drawing.Size(71, 24);
			this.butDetach.TabIndex = 101;
			this.butDetach.Text = "Detach";
			this.butDetach.Click += new System.EventHandler(this.butDetach_Click);
			// 
			// gridOut
			// 
			this.gridOut.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridOut.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridOut.HasAddButton = false;
			this.gridOut.HasDropDowns = false;
			this.gridOut.HasMultilineHeaders = false;
			this.gridOut.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridOut.HeaderHeight = 15;
			this.gridOut.HScrollVisible = false;
			this.gridOut.Location = new System.Drawing.Point(230, 418);
			this.gridOut.Name = "gridOut";
			this.gridOut.ScrollValue = 0;
			this.gridOut.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridOut.Size = new System.Drawing.Size(732, 212);
			this.gridOut.TabIndex = 99;
			this.gridOut.Title = "All Outstanding Claims";
			this.gridOut.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridOut.TitleHeight = 18;
			this.gridOut.TranslationName = "TableClaimPaySplits";
			this.gridOut.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridOut_CellDoubleClick);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label13);
			this.groupBox1.Controls.Add(this.textPayGroup);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.textPayType);
			this.groupBox1.Controls.Add(this.labelClinic);
			this.groupBox1.Controls.Add(this.textDateIssued);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.labelDateIssued);
			this.groupBox1.Controls.Add(this.butClaimPayEdit);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.textClinic);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.textCarrierName);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.textNote);
			this.groupBox1.Controls.Add(this.textCheckNum);
			this.groupBox1.Controls.Add(this.textBankBranch);
			this.groupBox1.Controls.Add(this.textAmount);
			this.groupBox1.Controls.Add(this.textDate);
			this.groupBox1.Location = new System.Drawing.Point(230, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(731, 110);
			this.groupBox1.TabIndex = 98;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Payment Details";
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(452, 86);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(81, 16);
			this.label13.TabIndex = 101;
			this.label13.Text = "Group";
			this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textPayGroup
			// 
			this.textPayGroup.Location = new System.Drawing.Point(537, 83);
			this.textPayGroup.MaxLength = 25;
			this.textPayGroup.Name = "textPayGroup";
			this.textPayGroup.ReadOnly = true;
			this.textPayGroup.Size = new System.Drawing.Size(97, 20);
			this.textPayGroup.TabIndex = 100;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(452, 65);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(81, 16);
			this.label10.TabIndex = 99;
			this.label10.Text = "Type";
			this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textPayType
			// 
			this.textPayType.Location = new System.Drawing.Point(537, 62);
			this.textPayType.MaxLength = 25;
			this.textPayType.Name = "textPayType";
			this.textPayType.ReadOnly = true;
			this.textPayType.Size = new System.Drawing.Size(97, 20);
			this.textPayType.TabIndex = 98;
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(21, 22);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(86, 14);
			this.labelClinic.TabIndex = 91;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDateIssued
			// 
			this.textDateIssued.Location = new System.Drawing.Point(110, 61);
			this.textDateIssued.Name = "textDateIssued";
			this.textDateIssued.ReadOnly = true;
			this.textDateIssued.Size = new System.Drawing.Size(68, 20);
			this.textDateIssued.TabIndex = 96;
			this.textDateIssued.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(238, 41);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 16);
			this.label2.TabIndex = 33;
			this.label2.Text = "Note";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelDateIssued
			// 
			this.labelDateIssued.Location = new System.Drawing.Point(12, 65);
			this.labelDateIssued.Name = "labelDateIssued";
			this.labelDateIssued.Size = new System.Drawing.Size(96, 16);
			this.labelDateIssued.TabIndex = 97;
			this.labelDateIssued.Text = "Date Issued";
			this.labelDateIssued.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butClaimPayEdit
			// 
			this.butClaimPayEdit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClaimPayEdit.Autosize = true;
			this.butClaimPayEdit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClaimPayEdit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClaimPayEdit.CornerRadius = 4F;
			this.butClaimPayEdit.Location = new System.Drawing.Point(650, 78);
			this.butClaimPayEdit.Name = "butClaimPayEdit";
			this.butClaimPayEdit.Size = new System.Drawing.Size(75, 24);
			this.butClaimPayEdit.TabIndex = 6;
			this.butClaimPayEdit.Text = "Edit";
			this.butClaimPayEdit.Click += new System.EventHandler(this.butClaimPayEdit_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(254, 85);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(91, 16);
			this.label3.TabIndex = 34;
			this.label3.Text = "Bank-Branch";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(253, 63);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(90, 16);
			this.label4.TabIndex = 35;
			this.label4.Text = "Check #";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textClinic
			// 
			this.textClinic.Location = new System.Drawing.Point(110, 19);
			this.textClinic.MaxLength = 25;
			this.textClinic.Name = "textClinic";
			this.textClinic.ReadOnly = true;
			this.textClinic.Size = new System.Drawing.Size(123, 20);
			this.textClinic.TabIndex = 93;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(13, 86);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(95, 16);
			this.label5.TabIndex = 36;
			this.label5.Text = "Amount";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textCarrierName
			// 
			this.textCarrierName.Location = new System.Drawing.Point(346, 19);
			this.textCarrierName.MaxLength = 25;
			this.textCarrierName.Name = "textCarrierName";
			this.textCarrierName.ReadOnly = true;
			this.textCarrierName.Size = new System.Drawing.Size(288, 20);
			this.textCarrierName.TabIndex = 93;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(12, 44);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(96, 16);
			this.label6.TabIndex = 37;
			this.label6.Text = "Payment Date";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(236, 21);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(109, 16);
			this.label7.TabIndex = 94;
			this.label7.Text = "Carrier Name";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textNote
			// 
			this.textNote.Location = new System.Drawing.Point(346, 40);
			this.textNote.MaxLength = 255;
			this.textNote.Multiline = true;
			this.textNote.Name = "textNote";
			this.textNote.ReadOnly = true;
			this.textNote.Size = new System.Drawing.Size(288, 20);
			this.textNote.TabIndex = 3;
			// 
			// textCheckNum
			// 
			this.textCheckNum.Location = new System.Drawing.Point(346, 61);
			this.textCheckNum.MaxLength = 25;
			this.textCheckNum.Name = "textCheckNum";
			this.textCheckNum.ReadOnly = true;
			this.textCheckNum.Size = new System.Drawing.Size(100, 20);
			this.textCheckNum.TabIndex = 1;
			// 
			// textBankBranch
			// 
			this.textBankBranch.Location = new System.Drawing.Point(346, 82);
			this.textBankBranch.MaxLength = 25;
			this.textBankBranch.Name = "textBankBranch";
			this.textBankBranch.ReadOnly = true;
			this.textBankBranch.Size = new System.Drawing.Size(100, 20);
			this.textBankBranch.TabIndex = 2;
			// 
			// textAmount
			// 
			this.textAmount.Location = new System.Drawing.Point(110, 82);
			this.textAmount.MaxVal = 100000000D;
			this.textAmount.MinVal = -100000000D;
			this.textAmount.Name = "textAmount";
			this.textAmount.ReadOnly = true;
			this.textAmount.Size = new System.Drawing.Size(68, 20);
			this.textAmount.TabIndex = 0;
			this.textAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(110, 40);
			this.textDate.Name = "textDate";
			this.textDate.ReadOnly = true;
			this.textDate.Size = new System.Drawing.Size(68, 20);
			this.textDate.TabIndex = 3;
			this.textDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// gridAttached
			// 
			this.gridAttached.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridAttached.HasAddButton = false;
			this.gridAttached.HasDropDowns = false;
			this.gridAttached.HasMultilineHeaders = false;
			this.gridAttached.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridAttached.HeaderHeight = 15;
			this.gridAttached.HScrollVisible = false;
			this.gridAttached.Location = new System.Drawing.Point(230, 125);
			this.gridAttached.Name = "gridAttached";
			this.gridAttached.ScrollValue = 0;
			this.gridAttached.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridAttached.Size = new System.Drawing.Size(732, 209);
			this.gridAttached.TabIndex = 95;
			this.gridAttached.Title = "Attached to this Payment";
			this.gridAttached.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridAttached.TitleHeight = 18;
			this.gridAttached.TranslationName = "TableClaimPaySplits";
			this.gridAttached.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAttached_CellDoubleClick);
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
			this.butDelete.Location = new System.Drawing.Point(13, 646);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(79, 24);
			this.butDelete.TabIndex = 52;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(886, 646);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "Cancel";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(772, 347);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(92, 16);
			this.label8.TabIndex = 36;
			this.label8.Text = "Total Payments";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// FormClaimPayBatch
			// 
			this.ClientSize = new System.Drawing.Size(974, 676);
			this.Controls.Add(this.groupFilters);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.textTotal);
			this.Controls.Add(this.butAttach);
			this.Controls.Add(this.textEobIsScanned);
			this.Controls.Add(this.butView);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.labelInstruct2);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.labelInstruct1);
			this.Controls.Add(this.butDetach);
			this.Controls.Add(this.gridOut);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.gridAttached);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.label8);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.Name = "FormClaimPayBatch";
			this.Text = "Insurance Payment (EOB)";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormClaimPayBatch_FormClosing);
			this.Load += new System.EventHandler(this.FormClaimPayEdit_Load);
			this.groupFilters.ResumeLayout(false);
			this.groupFilters.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormClaimPayEdit_Load(object sender, System.EventArgs e) {
			if(IsFromClaim && IsNew) {
				//ok and cancel
				labelInstruct1.Visible=false;
				labelInstruct2.Visible=false;
				gridOut.Visible=false;
				groupFilters.Visible=false;
				butAttach.Visible=false;
			}
			else {
				butOK.Visible=false;
				butClose.Text=Lan.g(this,"Close");
			}
			if(IsFromClaim) {
				//Remove context menus from the grids.  This preserves old functionality.
				gridAttached.ContextMenu=null;
				gridOut.ContextMenu=null;
			}
			FillClaimPayment();
			textCarrier.Text=textCarrierName.Text;
			FillGrids();
			if(ClaimPaymentCur.IsPartial){
				//an incomplete payment that's not yet locked
			}
			else{//locked
				if(!Security.IsAuthorized(Permissions.InsPayEdit,ClaimPaymentCur.CheckDate)) {
					butDelete.Enabled=false;
					gridAttached.Enabled=false;
					butClaimPayEdit.Enabled=false;
					butUp.Visible=false;
					butDown.Visible=false;
				}
				//someone with permission can double click on the top grid to edit amounts and can edit the object fields as well.
				butDetach.Visible=false;
				gridOut.Visible=false;
				groupFilters.Visible=false;
				labelInstruct1.Visible=false;
				labelInstruct2.Visible=false;
				butAttach.Visible=false;
			}
			if(EobAttaches.Exists(ClaimPaymentCur.ClaimPaymentNum)) {
				textEobIsScanned.Text=Lan.g(this,"Yes");
				butView.Text="View EOB";
			}
			else {
				textEobIsScanned.Text=Lan.g(this,"No");
				butView.Text="Scan EOB";
			}
		}

		private void FillClaimPayment() {
			textClinic.Text=Clinics.GetAbbr(ClaimPaymentCur.ClinicNum);
			if(ClaimPaymentCur.CheckDate.Year>1800) {
				textDate.Text=ClaimPaymentCur.CheckDate.ToShortDateString();
			}
			if(ClaimPaymentCur.DateIssued.Year>1800) {
				textDateIssued.Text=ClaimPaymentCur.DateIssued.ToShortDateString();
			}
			textAmount.Text=ClaimPaymentCur.CheckAmt.ToString("F");
			textCheckNum.Text=ClaimPaymentCur.CheckNum;
			textBankBranch.Text=ClaimPaymentCur.BankBranch;
			textCarrierName.Text=ClaimPaymentCur.CarrierName;
			textNote.Text=ClaimPaymentCur.Note;
			textPayType.Text=Defs.GetName(DefCat.InsurancePaymentType,ClaimPaymentCur.PayType);
			textPayGroup.Text=Defs.GetName(DefCat.ClaimPaymentGroups,ClaimPaymentCur.PayGroup);
		}

		private void FillGrids(){
			Cursor.Current=Cursors.WaitCursor;
			#region gridAttached
			ClaimsAttached=Claims.GetAttachedToPayment(ClaimPaymentCur.ClaimPaymentNum);
			List<long> listClaimNumsToUpdate=new List<long>();
			List<int> listPaymentRows=new List<int>();
			for(int i=0;i<ClaimsAttached.Count;i++) {
				if(ClaimsAttached[i].PaymentRow!=i+1) {
					listClaimNumsToUpdate.Add(ClaimsAttached[i].ClaimNum);
					listPaymentRows.Add(i+1);
					ClaimsAttached[i].PaymentRow=i+1;
				}
			}
			ClaimProcs.SetPaymentRow(listClaimNumsToUpdate,ClaimPaymentCur.ClaimPaymentNum,listPaymentRows);
			gridAttached.BeginUpdate();
			gridAttached.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"#"),25);
			gridAttached.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Service Date"),80);
			gridAttached.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Clinic"),70);
			gridAttached.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Claim Status"),80);
			gridAttached.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Carrier"),186);
			gridAttached.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Patient"),130);
			gridAttached.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Fee"),70,HorizontalAlignment.Right);
			gridAttached.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Payment"),70,HorizontalAlignment.Right);
			gridAttached.Columns.Add(col); 
			gridAttached.Rows.Clear();
			ODGridRow row;
			double total=0;
			foreach(ClaimPaySplit claimPS in ClaimsAttached) {
				total+=claimPS.InsPayAmt;
				row=new ODGridRow();
				row.Cells.Add(claimPS.PaymentRow.ToString());
				row.Cells.Add(claimPS.DateClaim.ToShortDateString());
				row.Cells.Add(claimPS.ClinicDesc);
				if(claimPS.ClaimStatus=="S") {
					row.Cells.Add("Sent");
				}
				else if(claimPS.ClaimStatus=="R") {
					row.Cells.Add("Received");
				}
				else {
					row.Cells.Add("Unknown");
				}
				row.Cells.Add(claimPS.Carrier);
				row.Cells.Add(claimPS.PatName);
				row.Cells.Add(claimPS.FeeBilled.ToString("F"));
				row.Cells.Add(claimPS.InsPayAmt.ToString("F"));
				gridAttached.Rows.Add(row);
			}
			gridAttached.EndUpdate();
			textTotal.Text=total.ToString("F");
			#endregion gridAttached
			if((IsFromClaim && IsNew) || !ClaimPaymentCur.IsPartial) {//gridOut isn't visible
				//if new batch claim payment opened from a claim, or if it's locked (not partial), gridOut isn't visible so no need to fill it
				Cursor.Current=Cursors.Default;
				return;
			}
			#region gridOutstanding
			int scrollValue=gridOut.ScrollValue;
			int selectedIdx=gridOut.GetSelectedIndex();
			ClaimsOutstanding=Claims.GetOutstandingClaims(textCarrier.Text,textName.Text,PrefC.GetDate(PrefName.ClaimPaymentNoShowZeroDate),textClaimID.Text);
			gridOut.BeginUpdate();
			gridOut.Columns.Clear();
			col=new ODGridColumn("",25);//so that it lines up with the grid above
			gridOut.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Service Date"),80);
			gridOut.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Clinic"),70);
			gridOut.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Claim Status"),80);
			gridOut.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Carrier"),186);
			gridOut.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Patient"),130);
			gridOut.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Fee"),70,HorizontalAlignment.Right);
			gridOut.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Payment"),70,HorizontalAlignment.Right);
			gridOut.Columns.Add(col);
			gridOut.Rows.Clear();
			foreach(ClaimPaySplit claimPS in ClaimsOutstanding) {
				row=new ODGridRow();
				row.Cells.Add("");
				row.Cells.Add(claimPS.DateClaim.ToShortDateString());
				row.Cells.Add(claimPS.ClinicDesc);
				if(claimPS.ClaimStatus=="S") {
					row.Cells.Add("Sent");
				}
				else if(claimPS.ClaimStatus=="R") {
					row.Cells.Add("Received");
				}
				else {
					row.Cells.Add("Unknown");
				}
				row.Cells.Add(claimPS.Carrier);
				row.Cells.Add(claimPS.PatName);
				row.Cells.Add(claimPS.FeeBilled.ToString("F"));
				row.Cells.Add(claimPS.InsPayAmt.ToString("F"));
				gridOut.Rows.Add(row);
			}
			gridOut.EndUpdate();
			gridOut.ScrollValue=scrollValue;
			gridOut.SetSelected(selectedIdx,true);
			#endregion gridOutstanding
			Cursor.Current=Cursors.Default;
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			FillGrids();
		}

		private void butClaimPayEdit_Click(object sender,EventArgs e) {
			FormClaimPayEdit FormCPE=new FormClaimPayEdit(ClaimPaymentCur);
			FormCPE.ShowDialog();
			FillClaimPayment();
			FillGrids();//For customer 5769, who was getting ocassional Chinese chars in the Amount boxes.
		}

		private void butAttach_Click(object sender,EventArgs e) {
			if(gridOut.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select at least one paid claim from the Outstanding Claims grid below.");
				return;
			}
			int paymentRow=ClaimsAttached.Count;//1-indexed
			for(int i=0;i<gridOut.SelectedIndices.Length;i++) {
				paymentRow++;
				ClaimProcs.AttachToPayment(ClaimsOutstanding[gridOut.SelectedIndices[i]].ClaimNum,ClaimPaymentCur.ClaimPaymentNum,ClaimPaymentCur.CheckDate,paymentRow);
			}
			FillGrids();	
		}

		private void butDetach_Click(object sender,EventArgs e) {
			if(gridAttached.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select a claim from the attached claims grid above.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Remove selected claims from this check?")) {
				return;
			}
			List<long> listDetachClaimNums=gridAttached.SelectedIndices.Select(x=>ClaimsAttached[x].ClaimNum).ToList();
			ClaimProcs.DetachFromPayment(listDetachClaimNums,ClaimPaymentCur.ClaimPaymentNum);
			FillGrids();
			bool didReorder=false;
			for(int i=0;i<ClaimsAttached.Count;i++) {
				if(ClaimsAttached[i].PaymentRow!=i+1) {
					ClaimProcs.SetPaymentRow(ClaimsAttached[i].ClaimNum,ClaimPaymentCur.ClaimPaymentNum,i+1);
					didReorder=true;
				}
			}
			if(didReorder) {
				FillGrids();
			}
		}

		private void gridAttached_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!Security.IsAuthorized(Permissions.ClaimView)) {
				return;
			}
			//top grid
			//bring up claimedit window.  User should be able to edit if not locked.
			Claim claimCur=Claims.GetClaim(ClaimsAttached[gridAttached.GetSelectedIndex()].ClaimNum);
			if(claimCur==null) {
				MsgBox.Show(this,"The claim has been deleted.");
				FillGrids();
				return;
			}
			FormClaimEdit FormCE=new FormClaimEdit(claimCur,Patients.GetPat(claimCur.PatNum),Patients.GetFamily(claimCur.PatNum));
			FormCE.IsFromBatchWindow=true;
			FormCE.ShowDialog();
			FillGrids();	
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(gridAttached.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item in the grid first.");
				return;
			}
			int[] selected=new int[gridAttached.SelectedIndices.Length];//remember the selected rows so that we can reselect them
			for(int i=0;i<gridAttached.SelectedIndices.Length;i++) {
				selected[i]=gridAttached.SelectedIndices[i];
			}
			if(selected[0]==0) {//can't go up
				return;
			}
			for(int i=0;i<selected.Length;i++) {
				//In the db, move the one above down to the current pos
				ClaimProcs.SetPaymentRow(ClaimsAttached[selected[i]-1].ClaimNum,ClaimPaymentCur.ClaimPaymentNum,selected[i]+1);
				//and move this row up one
				ClaimProcs.SetPaymentRow(ClaimsAttached[selected[i]].ClaimNum,ClaimPaymentCur.ClaimPaymentNum,selected[i]);
				//Then, swap them in the cached list.
				//ClaimsAttached.Reverse(selected[i]-1,2);
			}
			FillGrids();
			for(int i=0;i<selected.Length;i++) {
				gridAttached.SetSelected(selected[i]-1,true);
			}
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(gridAttached.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item in the grid first.");
				return;
			}
			int[] selected=new int[gridAttached.SelectedIndices.Length];
			for(int i=0;i<gridAttached.SelectedIndices.Length;i++) {
				selected[i]=gridAttached.SelectedIndices[i];
			}
			if(selected[selected.Length-1]==ClaimsAttached.Count-1) {//already at the bottom
				return;
			}
			for(int i=selected.Length-1;i>=0;i--) {//go backwards
				//In the db, move the one below up to the current pos
				ClaimProcs.SetPaymentRow(ClaimsAttached[selected[i]+1].ClaimNum,ClaimPaymentCur.ClaimPaymentNum,selected[i]+1);
				//and move this row down one
				ClaimProcs.SetPaymentRow(ClaimsAttached[selected[i]].ClaimNum,ClaimPaymentCur.ClaimPaymentNum,selected[i]+2);
				//Then, swap them in the cached list.
				//ClaimsAttached.Reverse(selected[i],2);
			}
			FillGrids();
			for(int i=0;i<selected.Length;i++) {
				gridAttached.SetSelected(selected[i]+1,true);
			}
		}

		private void gridOut_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!Security.IsAuthorized(Permissions.ClaimView)) {
				return;
			}
			//bottom grid
			//bring up claimedit window
			//after returning from the claim edit window, use a query to get a list of all the claimprocs that have amounts entered for that claim, but have ClaimPaymentNumber of 0.
			//Set all those claimprocs to be attached.
			Claim claimCur=Claims.GetClaim(ClaimsOutstanding[e.Row].ClaimNum);
			if(claimCur==null) {
				MsgBox.Show(this,"The claim has been deleted.");
				FillGrids();
				return;
			}
			FormClaimEdit FormCE=new FormClaimEdit(claimCur,Patients.GetPat(claimCur.PatNum),Patients.GetFamily(claimCur.PatNum));
			FormCE.IsFromBatchWindow=true;
			FormCE.ShowDialog();
			if(FormCE.DialogResult!=DialogResult.OK){
				return;
			}
			ClaimProcs.AttachToPayment(claimCur.ClaimNum,ClaimPaymentCur.ClaimPaymentNum,ClaimPaymentCur.CheckDate,ClaimsAttached.Count+1);
			FillGrids();			
		}

		private void menuItemGotoAccount_Click(object sender,EventArgs e) {
			//for the upper grid
			if(gridAttached.SelectedIndices.Length!=1 || !Security.IsAuthorized(Permissions.AccountModule)) {
				return;
			}
			GotoPatNum=ClaimsAttached[gridAttached.SelectedIndices[0]].PatNum;
			GotoClaimNum=ClaimsAttached[gridAttached.SelectedIndices[0]].ClaimNum;
			Patient pat=Patients.GetPat(GotoPatNum);
			FormOpenDental.S_Contr_PatientSelected(pat,false);
			GotoModule.GotoClaim(GotoClaimNum);
		}

		private void menuItemGotoOut_Click(object sender,EventArgs e) {
			//for the lower grid
			if(gridOut.SelectedIndices.Length!=1 || !Security.IsAuthorized(Permissions.AccountModule)) {
				return;
			}
			GotoPatNum=ClaimsOutstanding[gridOut.SelectedIndices[0]].PatNum;
			GotoClaimNum=ClaimsOutstanding[gridOut.SelectedIndices[0]].ClaimNum;
			Patient pat=Patients.GetPat(GotoPatNum);
			FormOpenDental.S_Contr_PatientSelected(pat,false);
			GotoModule.GotoClaim(GotoClaimNum);
		}

		//private void menuItemGoToAccount_Click(object sender,EventArgs e) {
			
			//Patient pat=Patients.GetPat(FormCS.GotoPatNum);
			//OnPatientSelected(FormCS.GotoPatNum,pat.GetNameLF(),pat.Email!="",pat.ChartNumber);
			//GotoModule.GotoClaim(FormCS.GotoClaimNum);
		//}

		private void ShowSecondaryClaims() {
			DataTable secCPs=Claims.GetSecondaryClaims(ClaimsAttached);
			if(secCPs.Rows.Count==0) {
				return;
			}
			string message="Some of the payments have secondary claims: \r\n"
				+"Date | PatNum | Patient Name";
			for(int i=0;i<secCPs.Rows.Count;i++) {
				//claimProc=secondaryClaims[i];
				message+="\r\n"+PIn.Date(secCPs.Rows[i]["DateCP"].ToString()).ToShortDateString()
					+" | "+secCPs.Rows[i]["PatNum"].ToString()
					+" | "+Patients.GetPat(PIn.Long(secCPs.Rows[i]["PatNum"].ToString())).GetNameLF();
			}
			message+="\r\n\r\nPrint this list, then use it to review and send secondary claims.";
			MsgBoxCopyPaste msgBox=new MsgBoxCopyPaste(message);
			msgBox.ShowDialog();
		}

		private void butView_Click(object sender,EventArgs e) {
			FormImages formI=new FormImages();
			formI.ClaimPaymentNum=ClaimPaymentCur.ClaimPaymentNum;
			formI.ShowDialog();
			if(EobAttaches.Exists(ClaimPaymentCur.ClaimPaymentNum)) {
				textEobIsScanned.Text=Lan.g(this,"Yes");
				butView.Text="View EOB";
			}
			else {
				textEobIsScanned.Text=Lan.g(this,"No");
				butView.Text="Scan EOB";
			}
			FillClaimPayment();//For customer 5769, who was getting ocassional Chinese chars in the Amount boxes.
			FillGrids();//ditto
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(!MsgBox.Show(this,true,"Delete this insurance check?")){
				return;
			}
			if(ClaimPaymentCur.IsPartial) {//probably new
				//everyone should have permission to delete a partial payment
			}
			else {//locked
				//this delete button already disabled if no permission
			}
			try{
				ClaimPayments.Delete(ClaimPaymentCur);
			}
			catch(ApplicationException ex){
				MessageBox.Show(ex.Message);
				return;
			}
			IsDeleting=true;
			Close();
		}

		private void butOK_Click(object sender,EventArgs e) {
			//only visible if IsFromClaim and IsNew
			if(!PIn.Double(textAmount.Text).IsEqual(PIn.Double(textTotal.Text))) { //PIn to fix amounts which are somehow sometimes different.
				MsgBox.Show(this,"Amounts do not match.");
				return;
			}
			if(gridAttached.Rows.Count==0) {
				MsgBox.Show(this,"At least one claim must be attached to this insurance payment.");
				return;
			}
			if(!PrefC.GetBool(PrefName.AllowFutureInsPayments)
				&& !PrefC.GetBool(PrefName.FutureTransDatesAllowed)
				&& ClaimPaymentCur.CheckDate>MiscData.GetNowDateTime().Date)
			{
				MsgBox.Show(this,"Insurance Payment Date must not be a future date.");
				return;
			}
			//No need to prompt user about secondary claims because they already went into each Account individually.
			DialogResult=DialogResult.OK;
			Close();
		}

		private void butClose_Click(object sender,System.EventArgs e) {
			DialogResult=DialogResult.Cancel;			
			Close();
		}

		private void FormClaimPayBatch_FormClosing(object sender,FormClosingEventArgs e) {
			if(DialogResult==DialogResult.Cancel && IsFromClaim && IsNew) {//This acts as a Cancel button. Happens when butClose or the red x is clicked.
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Delete this payment?")) {
					e.Cancel=true;
					return;
				}
				IsDeleting=true;//the actual deletion will be handled in FormClaimEdit.
			}
			if(IsDeleting){//This is here because the delete button could also set this.
				return;
			}
			if(IsDisposed) {//This should only happen if interupted by an Auto-Logoff.
				return; //Leave the payment as partial so the user can come back and edit.
			}
			if(ClaimPaymentCur.IsPartial) {
				if(PIn.Double(textAmount.Text).IsEqual(PIn.Double(textTotal.Text))) { //PIn to fix amounts which are somehow sometimes different.
					if(ClaimsAttached.Count>0) {
						ShowSecondaryClaims();//always continues after this dlg
					}
					ClaimPaymentCur.IsPartial=false;
					ClaimPayments.Update(ClaimPaymentCur);
				}
			}
			else {//locked
				if(!PIn.Double(textAmount.Text).IsEqual(PIn.Double(textTotal.Text))) { //PIn to fix amounts which are somehow sometimes different.
					//Someone edited a locked payment
					if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Amounts do not match.  Continue anyway?")) {
						e.Cancel=true;
						return;
					}
				}
			}
			if(IsNew) {
				SecurityLogs.MakeLogEntry(Permissions.InsPayCreate,0,"Claim Payment: "+ClaimPaymentCur.ClaimPaymentNum);
			}
			else {
				SecurityLogs.MakeLogEntry(Permissions.InsPayEdit,0,"Claim Payment: "+ClaimPaymentCur.ClaimPaymentNum);
			}
		}
	}
}