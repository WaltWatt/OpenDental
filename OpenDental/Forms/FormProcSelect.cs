using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;

namespace OpenDental{
	/// <summary></summary>
	public class FormProcSelect : ODForm {
		#region Designer Variables
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private ODGrid gridMain;
		private GroupBox groupCreditLogic;
		private RadioButton radioExcludeAllCredits;
		private RadioButton radioOnlyAllocatedCredits;
		private RadioButton radioIncludeAllCredits;
		private GroupBox groupBreakdown;
		private Label label12;
		private Label labelCurrentSplits;
		private Label label8;
		private Label label9;
		private Label labelAmtEnd;
		private Label labelTitleWriteOffEst;
		private Label labelWriteOffEst;
		private Label label10;
		private Label label4;
		private Label label6;
		private Label labelWriteOff;
		private Label labelTitleInsEst;
		private Label labelInsEst;
		private Label label1;
		private Label labelAmtStart;
		private Label labelPaySplits;
		private Label labelAmtOriginal;
		private Label label2;
		private Label labelPositiveAdjs;
		private Label label13;
		private Label label3;
		private Label labelInsPay;
		private Label labelNegativeAdjs;
		private Label label7;
		private Label labelPayPlanCredits;
		private Label label5;
		private Label labelTitleOther;
		private Label labelOther;
		#endregion

		#region Private Variables
		private long _patNumCur;
		///<summary>A list of completed procedures that are associated to this patient or their payment plans.</summary>
		private List<Procedure> _listProcedures;
		private List<PaySplit> _listPaySplits;
		private List<Adjustment> _listAdjustments;
		private List<PayPlanCharge> _listPayPlanCharges;
		private List<ClaimProc> _listInsPayAsTotal;
		private List<ClaimProc> _listClaimProcs;
		private List<AccountEntry> _listAccountCharges;
		///<summary>Does not perform FIFO logic.</summary>
		private bool _isSimpleView;
		///<summary>Set to true to enable multiple procedure selection mode.</summary>
		private bool _isMultiSelect;
		///<summary>If form closes with OK, this contains selected proc num.</summary>
		private List<Procedure> _listSelectedProcs;
		private Label labelUnallocated;
		private bool _doShowUnallocatedLabel;
		#endregion

		#region Public Variables
		///<summary>List of paysplits for the current payment.</summary>
		public List<PaySplit> ListSplitsCur = new List<PaySplit>();
		#endregion

		#region Public Properties
		///<summary>If form closes with OK, this contains selected procedures.</summary>
		public List<Procedure> ListSelectedProcs {
			get { return _listSelectedProcs; }
		}
		#endregion

		///<summary>Displays completed procedures for the passed-in pat. 
		///Pass in true for isSimpleView to show all completed procedures, 
		///otherwise the user will be able to pick between credit allocation strategies (FIFO, Explicit, All).</summary>
		public FormProcSelect(long patNum, bool isSimpleView, bool isMultiSelect=false,bool doShowUnallocatedLabel=false)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			_patNumCur=patNum;
			_isSimpleView=isSimpleView;
			_isMultiSelect=isMultiSelect;
			_doShowUnallocatedLabel=doShowUnallocatedLabel;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProcSelect));
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupCreditLogic = new System.Windows.Forms.GroupBox();
			this.radioExcludeAllCredits = new System.Windows.Forms.RadioButton();
			this.radioOnlyAllocatedCredits = new System.Windows.Forms.RadioButton();
			this.radioIncludeAllCredits = new System.Windows.Forms.RadioButton();
			this.groupBreakdown = new System.Windows.Forms.GroupBox();
			this.labelTitleOther = new System.Windows.Forms.Label();
			this.labelOther = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.labelCurrentSplits = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.labelAmtEnd = new System.Windows.Forms.Label();
			this.labelTitleWriteOffEst = new System.Windows.Forms.Label();
			this.labelWriteOffEst = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.labelWriteOff = new System.Windows.Forms.Label();
			this.labelTitleInsEst = new System.Windows.Forms.Label();
			this.labelInsEst = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.labelAmtStart = new System.Windows.Forms.Label();
			this.labelPaySplits = new System.Windows.Forms.Label();
			this.labelAmtOriginal = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.labelPositiveAdjs = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.labelInsPay = new System.Windows.Forms.Label();
			this.labelNegativeAdjs = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.labelPayPlanCredits = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.labelUnallocated = new System.Windows.Forms.Label();
			this.groupCreditLogic.SuspendLayout();
			this.groupBreakdown.SuspendLayout();
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
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(15, 76);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(675, 503);
			this.gridMain.TabIndex = 140;
			this.gridMain.Title = "Procedures";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableProcSelect";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellClick);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(705, 553);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
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
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(786, 553);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// groupCreditLogic
			// 
			this.groupCreditLogic.Controls.Add(this.radioExcludeAllCredits);
			this.groupCreditLogic.Controls.Add(this.radioOnlyAllocatedCredits);
			this.groupCreditLogic.Controls.Add(this.radioIncludeAllCredits);
			this.groupCreditLogic.Location = new System.Drawing.Point(15, 1);
			this.groupCreditLogic.Name = "groupCreditLogic";
			this.groupCreditLogic.Size = new System.Drawing.Size(331, 73);
			this.groupCreditLogic.TabIndex = 143;
			this.groupCreditLogic.TabStop = false;
			this.groupCreditLogic.Text = "Credit Filter";
			// 
			// radioExcludeAllCredits
			// 
			this.radioExcludeAllCredits.Location = new System.Drawing.Point(20, 49);
			this.radioExcludeAllCredits.Name = "radioExcludeAllCredits";
			this.radioExcludeAllCredits.Size = new System.Drawing.Size(305, 17);
			this.radioExcludeAllCredits.TabIndex = 2;
			this.radioExcludeAllCredits.TabStop = true;
			this.radioExcludeAllCredits.Text = "Exclude all credits";
			this.radioExcludeAllCredits.UseVisualStyleBackColor = true;
			this.radioExcludeAllCredits.Click += new System.EventHandler(this.radioCreditCalc_Click);
			// 
			// radioOnlyAllocatedCredits
			// 
			this.radioOnlyAllocatedCredits.Checked = true;
			this.radioOnlyAllocatedCredits.Location = new System.Drawing.Point(20, 15);
			this.radioOnlyAllocatedCredits.Name = "radioOnlyAllocatedCredits";
			this.radioOnlyAllocatedCredits.Size = new System.Drawing.Size(305, 17);
			this.radioOnlyAllocatedCredits.TabIndex = 0;
			this.radioOnlyAllocatedCredits.TabStop = true;
			this.radioOnlyAllocatedCredits.Text = "Only allocated credits";
			this.radioOnlyAllocatedCredits.UseVisualStyleBackColor = true;
			this.radioOnlyAllocatedCredits.Click += new System.EventHandler(this.radioCreditCalc_Click);
			// 
			// radioIncludeAllCredits
			// 
			this.radioIncludeAllCredits.Location = new System.Drawing.Point(20, 32);
			this.radioIncludeAllCredits.Name = "radioIncludeAllCredits";
			this.radioIncludeAllCredits.Size = new System.Drawing.Size(305, 17);
			this.radioIncludeAllCredits.TabIndex = 1;
			this.radioIncludeAllCredits.TabStop = true;
			this.radioIncludeAllCredits.Text = "Include all credits";
			this.radioIncludeAllCredits.UseVisualStyleBackColor = true;
			this.radioIncludeAllCredits.Click += new System.EventHandler(this.radioCreditCalc_Click);
			// 
			// groupBreakdown
			// 
			this.groupBreakdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBreakdown.Controls.Add(this.labelTitleOther);
			this.groupBreakdown.Controls.Add(this.labelOther);
			this.groupBreakdown.Controls.Add(this.label12);
			this.groupBreakdown.Controls.Add(this.labelCurrentSplits);
			this.groupBreakdown.Controls.Add(this.label8);
			this.groupBreakdown.Controls.Add(this.label9);
			this.groupBreakdown.Controls.Add(this.labelAmtEnd);
			this.groupBreakdown.Controls.Add(this.labelTitleWriteOffEst);
			this.groupBreakdown.Controls.Add(this.labelWriteOffEst);
			this.groupBreakdown.Controls.Add(this.label10);
			this.groupBreakdown.Controls.Add(this.label4);
			this.groupBreakdown.Controls.Add(this.label6);
			this.groupBreakdown.Controls.Add(this.labelWriteOff);
			this.groupBreakdown.Controls.Add(this.labelTitleInsEst);
			this.groupBreakdown.Controls.Add(this.labelInsEst);
			this.groupBreakdown.Controls.Add(this.label1);
			this.groupBreakdown.Controls.Add(this.labelAmtStart);
			this.groupBreakdown.Controls.Add(this.labelPaySplits);
			this.groupBreakdown.Controls.Add(this.labelAmtOriginal);
			this.groupBreakdown.Controls.Add(this.label2);
			this.groupBreakdown.Controls.Add(this.labelPositiveAdjs);
			this.groupBreakdown.Controls.Add(this.label13);
			this.groupBreakdown.Controls.Add(this.label3);
			this.groupBreakdown.Controls.Add(this.labelInsPay);
			this.groupBreakdown.Controls.Add(this.labelNegativeAdjs);
			this.groupBreakdown.Controls.Add(this.label7);
			this.groupBreakdown.Controls.Add(this.labelPayPlanCredits);
			this.groupBreakdown.Controls.Add(this.label5);
			this.groupBreakdown.Location = new System.Drawing.Point(694, 76);
			this.groupBreakdown.Name = "groupBreakdown";
			this.groupBreakdown.Size = new System.Drawing.Size(167, 319);
			this.groupBreakdown.TabIndex = 144;
			this.groupBreakdown.TabStop = false;
			this.groupBreakdown.Text = "Breakdown";
			// 
			// labelTitleOther
			// 
			this.labelTitleOther.Location = new System.Drawing.Point(7, 216);
			this.labelTitleOther.Name = "labelTitleOther";
			this.labelTitleOther.Size = new System.Drawing.Size(86, 18);
			this.labelTitleOther.TabIndex = 154;
			this.labelTitleOther.Text = "Other:";
			this.labelTitleOther.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelOther
			// 
			this.labelOther.Location = new System.Drawing.Point(99, 216);
			this.labelOther.Name = "labelOther";
			this.labelOther.Size = new System.Drawing.Size(61, 18);
			this.labelOther.TabIndex = 153;
			this.labelOther.Text = "0.00";
			this.labelOther.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label12
			// 
			this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label12.Location = new System.Drawing.Point(7, 265);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(86, 18);
			this.label12.TabIndex = 152;
			this.label12.Text = "Current Splits:";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelCurrentSplits
			// 
			this.labelCurrentSplits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelCurrentSplits.Location = new System.Drawing.Point(99, 265);
			this.labelCurrentSplits.Name = "labelCurrentSplits";
			this.labelCurrentSplits.Size = new System.Drawing.Size(61, 18);
			this.labelCurrentSplits.TabIndex = 151;
			this.labelCurrentSplits.Text = "0.00";
			this.labelCurrentSplits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label8.Location = new System.Drawing.Point(5, 288);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(155, 2);
			this.label8.TabIndex = 150;
			// 
			// label9
			// 
			this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(7, 294);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(86, 18);
			this.label9.TabIndex = 149;
			this.label9.Text = "Amt End:";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelAmtEnd
			// 
			this.labelAmtEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelAmtEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelAmtEnd.Location = new System.Drawing.Point(99, 294);
			this.labelAmtEnd.Name = "labelAmtEnd";
			this.labelAmtEnd.Size = new System.Drawing.Size(61, 18);
			this.labelAmtEnd.TabIndex = 148;
			this.labelAmtEnd.Text = "0.00";
			this.labelAmtEnd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelTitleWriteOffEst
			// 
			this.labelTitleWriteOffEst.Location = new System.Drawing.Point(7, 194);
			this.labelTitleWriteOffEst.Name = "labelTitleWriteOffEst";
			this.labelTitleWriteOffEst.Size = new System.Drawing.Size(86, 18);
			this.labelTitleWriteOffEst.TabIndex = 147;
			this.labelTitleWriteOffEst.Text = "WriteOff Ests:";
			this.labelTitleWriteOffEst.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelWriteOffEst
			// 
			this.labelWriteOffEst.Location = new System.Drawing.Point(99, 194);
			this.labelWriteOffEst.Name = "labelWriteOffEst";
			this.labelWriteOffEst.Size = new System.Drawing.Size(61, 18);
			this.labelWriteOffEst.TabIndex = 146;
			this.labelWriteOffEst.Text = "0.00";
			this.labelWriteOffEst.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(7, 150);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(86, 18);
			this.label10.TabIndex = 145;
			this.label10.Text = "WriteOffs:";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label4.Location = new System.Drawing.Point(5, 237);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(155, 2);
			this.label4.TabIndex = 143;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(7, 243);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(86, 18);
			this.label6.TabIndex = 15;
			this.label6.Text = "Amt Start:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelWriteOff
			// 
			this.labelWriteOff.Location = new System.Drawing.Point(99, 150);
			this.labelWriteOff.Name = "labelWriteOff";
			this.labelWriteOff.Size = new System.Drawing.Size(61, 18);
			this.labelWriteOff.TabIndex = 144;
			this.labelWriteOff.Text = "0.00";
			this.labelWriteOff.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelTitleInsEst
			// 
			this.labelTitleInsEst.Location = new System.Drawing.Point(7, 172);
			this.labelTitleInsEst.Name = "labelTitleInsEst";
			this.labelTitleInsEst.Size = new System.Drawing.Size(86, 18);
			this.labelTitleInsEst.TabIndex = 23;
			this.labelTitleInsEst.Text = "Ins Ests:";
			this.labelTitleInsEst.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelInsEst
			// 
			this.labelInsEst.Location = new System.Drawing.Point(99, 172);
			this.labelInsEst.Name = "labelInsEst";
			this.labelInsEst.Size = new System.Drawing.Size(61, 18);
			this.labelInsEst.TabIndex = 22;
			this.labelInsEst.Text = "0.00";
			this.labelInsEst.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(7, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(86, 18);
			this.label1.TabIndex = 27;
			this.label1.Text = "Amt Original:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelAmtStart
			// 
			this.labelAmtStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelAmtStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelAmtStart.Location = new System.Drawing.Point(99, 243);
			this.labelAmtStart.Name = "labelAmtStart";
			this.labelAmtStart.Size = new System.Drawing.Size(61, 18);
			this.labelAmtStart.TabIndex = 14;
			this.labelAmtStart.Text = "0.00";
			this.labelAmtStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelPaySplits
			// 
			this.labelPaySplits.Location = new System.Drawing.Point(99, 106);
			this.labelPaySplits.Name = "labelPaySplits";
			this.labelPaySplits.Size = new System.Drawing.Size(61, 18);
			this.labelPaySplits.TabIndex = 14;
			this.labelPaySplits.Text = "0.00";
			this.labelPaySplits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelAmtOriginal
			// 
			this.labelAmtOriginal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelAmtOriginal.Location = new System.Drawing.Point(99, 18);
			this.labelAmtOriginal.Name = "labelAmtOriginal";
			this.labelAmtOriginal.Size = new System.Drawing.Size(61, 18);
			this.labelAmtOriginal.TabIndex = 26;
			this.labelAmtOriginal.Text = "0.00";
			this.labelAmtOriginal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(7, 106);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(86, 18);
			this.label2.TabIndex = 15;
			this.label2.Text = "PaySplits:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelPositiveAdjs
			// 
			this.labelPositiveAdjs.Location = new System.Drawing.Point(99, 40);
			this.labelPositiveAdjs.Name = "labelPositiveAdjs";
			this.labelPositiveAdjs.Size = new System.Drawing.Size(61, 18);
			this.labelPositiveAdjs.TabIndex = 16;
			this.labelPositiveAdjs.Text = "0.00";
			this.labelPositiveAdjs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(7, 128);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(86, 18);
			this.label13.TabIndex = 25;
			this.label13.Text = "Ins Payments:";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(7, 40);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(86, 18);
			this.label3.TabIndex = 17;
			this.label3.Text = "Positive Adjs:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelInsPay
			// 
			this.labelInsPay.Location = new System.Drawing.Point(99, 128);
			this.labelInsPay.Name = "labelInsPay";
			this.labelInsPay.Size = new System.Drawing.Size(61, 18);
			this.labelInsPay.TabIndex = 24;
			this.labelInsPay.Text = "0.00";
			this.labelInsPay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelNegativeAdjs
			// 
			this.labelNegativeAdjs.Location = new System.Drawing.Point(99, 62);
			this.labelNegativeAdjs.Name = "labelNegativeAdjs";
			this.labelNegativeAdjs.Size = new System.Drawing.Size(61, 18);
			this.labelNegativeAdjs.TabIndex = 18;
			this.labelNegativeAdjs.Text = "0.00";
			this.labelNegativeAdjs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(7, 62);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(86, 18);
			this.label7.TabIndex = 19;
			this.label7.Text = "Negative Adjs:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelPayPlanCredits
			// 
			this.labelPayPlanCredits.Location = new System.Drawing.Point(99, 84);
			this.labelPayPlanCredits.Name = "labelPayPlanCredits";
			this.labelPayPlanCredits.Size = new System.Drawing.Size(61, 18);
			this.labelPayPlanCredits.TabIndex = 20;
			this.labelPayPlanCredits.Text = "0.00";
			this.labelPayPlanCredits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(7, 84);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(86, 18);
			this.label5.TabIndex = 21;
			this.label5.Text = "PayPlan Credits:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelUnallocated
			// 
			this.labelUnallocated.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelUnallocated.Location = new System.Drawing.Point(390, 16);
			this.labelUnallocated.Name = "labelUnallocated";
			this.labelUnallocated.Size = new System.Drawing.Size(420, 45);
			this.labelUnallocated.TabIndex = 155;
			this.labelUnallocated.Text = "This patient has unallocated unearned income. Please select procedures to allocat" +
    "e this income towards.";
			this.labelUnallocated.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelUnallocated.Visible = false;
			// 
			// FormProcSelect
			// 
			this.ClientSize = new System.Drawing.Size(873, 586);
			this.Controls.Add(this.labelUnallocated);
			this.Controls.Add(this.groupBreakdown);
			this.Controls.Add(this.groupCreditLogic);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormProcSelect";
			this.ShowInTaskbar = false;
			this.Text = "Select Procedure";
			this.Load += new System.EventHandler(this.FormProcSelect_Load);
			this.groupCreditLogic.ResumeLayout(false);
			this.groupBreakdown.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormProcSelect_Load(object sender,System.EventArgs e) {
			if(_isMultiSelect) {
				gridMain.SelectionMode=OpenDental.UI.GridSelectionMode.MultiExtended;
			}
			_listSelectedProcs=new List<Procedure>();
			_listProcedures=Procedures.GetCompleteForPats(new List<long> { _patNumCur });
			_listAdjustments=Adjustments.GetAdjustForPats(new List<long> { _patNumCur });
			_listPayPlanCharges=PayPlanCharges.GetDueForPayPlans(PayPlans.GetForPats(null,_patNumCur),_patNumCur).ToList();//Does not get charges for the future.
			_listPaySplits=PaySplits.GetForPats(new List<long> { _patNumCur });//Might contain payplan payments.
			_listInsPayAsTotal=ClaimProcs.GetByTotForPats(new List<long> { _patNumCur });
			_listClaimProcs=ClaimProcs.GetForProcs(_listProcedures.Select(x => x.ProcNum).ToList());
			labelUnallocated.Visible=_doShowUnallocatedLabel;
			if(PrefC.GetInt(PrefName.RigorousAdjustments)==(int)RigorousAdjustments.DontEnforce) {
				radioIncludeAllCredits.Checked=true;
			}
			else {
				radioOnlyAllocatedCredits.Checked=true;
			}
			FillGrid();
		}

		private void FillGrid(){
			CreditCalcType credCalc;
			if(_isSimpleView) {
				credCalc = CreditCalcType.ExcludeAll;
				groupBreakdown.Visible=false;
				groupCreditLogic.Visible=false;
			}
			else if(radioIncludeAllCredits.Checked) {
				credCalc = CreditCalcType.IncludeAll;
			}
			else if(radioOnlyAllocatedCredits.Checked) {
				credCalc = CreditCalcType.AllocatedOnly;
			}
			else {
				credCalc= CreditCalcType.ExcludeAll;
			}
      _listAccountCharges=AccountModules.GetListUnpaidAccountCharges(_listProcedures, _listAdjustments,
				_listPaySplits, _listClaimProcs, _listPayPlanCharges, _listInsPayAsTotal, credCalc, ListSplitsCur);
      gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableProcSelect","Date"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcSelect","Prov"),55);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcSelect","Code"),55);
			gridMain.Columns.Add(col);
			if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
				col=new ODGridColumn(Lan.g("TableProcSelect","Description"),290);
				gridMain.Columns.Add(col);
			}
			else {
				col=new ODGridColumn(Lan.g("TableProcSelect","Tooth"),40);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableProcSelect","Description"),250);
				gridMain.Columns.Add(col);
      }
			if(credCalc == CreditCalcType.ExcludeAll) {
				col=new ODGridColumn(Lan.g("TableProcSelect","Amt"),0,HorizontalAlignment.Right);
				gridMain.Columns.Add(col);
			}
			else {
				col=new ODGridColumn(Lan.g("TableProcSelect","Amt Orig"),60,HorizontalAlignment.Right);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableProcSelect","Amt Start"),60,HorizontalAlignment.Right);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableProcSelect","Amt End"),60,HorizontalAlignment.Right);
				gridMain.Columns.Add(col);
			}
      gridMain.Rows.Clear();
			ODGridRow row;
			foreach(AccountEntry entry in _listAccountCharges) {
				if(entry.GetType()!=typeof(ProcExtended)  || Math.Round(entry.AmountEnd,3) == 0) {
					continue;
				}
				Procedure procCur = ((ProcExtended)entry.Tag).Proc;
				ProcedureCode procCodeCur = ProcedureCodes.GetProcCode(procCur.CodeNum);
				row=new ODGridRow();
				row.Cells.Add(procCur.ProcDate.ToShortDateString());
				row.Cells.Add(Providers.GetAbbr(entry.ProvNum));
				row.Cells.Add(procCodeCur.ProcCode);
				if(!Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
					row.Cells.Add(Tooth.ToInternat(procCur.ToothNum));
				}
				row.Cells.Add(procCodeCur.Descript);
				row.Cells.Add(entry.AmountOriginal.ToString("f"));
				if(credCalc != CreditCalcType.ExcludeAll) {
					row.Cells.Add(entry.AmountStart.ToString("f"));
					row.Cells.Add(entry.AmountEnd.ToString("f"));
				}
				row.Tag=entry;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			if(!_isSimpleView) {
				RefreshBreakdown();
			}
		}

		private void RefreshBreakdown() {
			if(gridMain.GetSelectedIndex()==-1) {
				labelAmtOriginal.Text=(0).ToString("f");
				labelPositiveAdjs.Text=(0).ToString("f");
				labelNegativeAdjs.Text=(0).ToString("f");
				labelPayPlanCredits.Text=(0).ToString("f");
				labelPaySplits.Text=(0).ToString("f");
				labelInsEst.Text=(0).ToString("f");
				labelInsPay.Text=(0).ToString("f");
				labelOther.Text=(0).ToString("f");
				labelAmtStart.Text=(0).ToString("f");
				labelWriteOff.Text=(0).ToString("f");
				labelWriteOffEst.Text=(0).ToString("f");
				labelCurrentSplits.Text=(0).ToString("f");
				labelAmtEnd.Text=(0).ToString("f");
				return;
			}
			//there could be more than one proc selected if IsMultiSelect = true.
			List<AccountEntry> clickedEntries = gridMain.SelectedGridRows
				.Select(x => (AccountEntry)x.Tag)
				.Where(x => x.GetType() == typeof(ProcExtended))
				.ToList();
			List<ProcExtended> listSelectedProcE = clickedEntries.Select(x => (ProcExtended)x.Tag).ToList();
			labelAmtOriginal.Text=    listSelectedProcE.Sum(x => x.AmountOriginal).ToString("f");
			labelPositiveAdjs.Text=   Stringify(listSelectedProcE.Sum(x => x.PositiveAdjTotal));
			labelNegativeAdjs.Text=   Stringify((listSelectedProcE.Sum(x => x.NegativeAdjTotals)));
			labelPayPlanCredits.Text= Stringify((-listSelectedProcE.Sum(x => x.PayPlanCreditTotal)));
			labelPaySplits.Text=      Stringify((-listSelectedProcE.Sum(x => x.PaySplitTotal)));
			labelInsEst.Text=         Stringify((-listSelectedProcE.Sum(x => x.InsEstTotal)));
			labelInsPay.Text=         Stringify((-listSelectedProcE.Sum(x => x.InsPayTotal)));
			labelWriteOff.Text=       Stringify((-listSelectedProcE.Sum(x => x.WriteOffTotal)));
			labelWriteOffEst.Text=    Stringify((-listSelectedProcE.Sum(x => x.WriteOffEstTotal)));
			//other credits apply when calculating using FIFO.
			labelOther.Text=          Stringify((double)(-(listSelectedProcE.Sum(x =>(decimal)x.AmountStart) - clickedEntries.Sum(y => y.AmountStart))));
			labelAmtStart.Text=       clickedEntries.Sum(y => y.AmountStart).ToString("f");
			labelCurrentSplits.Text=  Stringify((-listSelectedProcE.Sum(x => x.SplitsCurTotal)));
			labelAmtEnd.Text=         clickedEntries.Sum(y => y.AmountEnd).ToString("f");
		}
		

		private string Stringify(double amt) {
			if(amt > 0) {
				return "+"+amt.ToString("f");
			}
			return amt.ToString("f");
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			RefreshBreakdown();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			_listSelectedProcs.Add(((ProcExtended)((AccountEntry)gridMain.Rows[e.Row].Tag).Tag).Proc);
			DialogResult=DialogResult.OK;
		}

		private void radioCreditCalc_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1){
				MsgBox.Show(this,"Please select an item first.");
				return;
			}
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				_listSelectedProcs.Add(((ProcExtended)((AccountEntry)gridMain.Rows[gridMain.SelectedIndices[i]].Tag).Tag).Proc);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
    }
	}
}





















