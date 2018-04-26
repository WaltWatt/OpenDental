using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using OpenDental.ReportingComplex;
using OpenDental.UI;
using OpenDentBusiness;
using System.Linq;
using CodeBase;
using System.Text;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormPayPlan : ODForm {
		#region Private Designer Variables
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label labelDateAgreement;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.GroupBox groupTerms;
		private OpenDental.ValidDate textDate;
		private OpenDental.ValidDouble textAmount;
		private OpenDental.ValidDate textDateFirstPay;
		private OpenDental.ValidDouble textAPR;
		private OpenDental.UI.Button butPrint;
		private System.Windows.Forms.TextBox textGuarantor;
		private OpenDental.UI.Button butGoToGuar;
		private OpenDental.UI.Button butGoToPat;
		private System.Windows.Forms.TextBox textPatient;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.GroupBox groupBox3;
		private OpenDental.ValidDouble textDownPayment;
		private System.Drawing.Printing.PrintDocument pd2;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.TextBox textTotalCost;
		private System.Windows.Forms.Label label10;
		private OpenDental.UI.Button butDelete;
		private OpenDental.ODtextBox textNote;
		private System.Windows.Forms.TextBox textAccumulatedDue;
		private OpenDental.UI.Button butCreateSched;
		private OpenDental.ValidDouble textPeriodPayment;
		private OpenDental.UI.Button butChangeGuar;
		private System.Windows.Forms.TextBox textInsPlan;
		private OpenDental.UI.Button butChangePlan;
		private System.Windows.Forms.Label labelGuarantor;
		private System.Windows.Forms.Label labelInsPlan;
		private OpenDental.UI.ODGrid gridCharges;
		private OpenDental.UI.Button butClear;
		private OpenDental.UI.Button butAdd;
		private System.Windows.Forms.TextBox textAmtPaid;
		private System.Windows.Forms.TextBox textPrincPaid;
		private System.Windows.Forms.Label label14;
		private Label label1;
		private ValidDouble textCompletedAmt;
		private Label labelTxAmtInfo;
		private OpenDental.UI.Button butPickProv;
		private ComboBox comboProv;
		private ComboBox comboClinic;
		private Label labelClinic;
		private Label label16;
		private GroupBox groupProvClin;
		private UI.Button butMoreOptions;
		private ValidDouble textBalance;
		private TextBox textInterest;
		private ValidDouble textPayment;
		private ValidDouble textPrincipal;
		private Label labelTotals;
		private UI.Button butRecalculate;
		private TextBox textDue;
		private UI.Button butAddTxCredits;
		private Label labelTotalTx;
		private ValidDouble textTotalTxAmt;
		private UI.Button butClosePlan;
		private Label labelClosed;
		private UI.Button butSignPrint;
		private SignatureBoxWrapper signatureBoxWrapper;
		private GroupBox groupBox4;
		private ValidNumber textPaymentCount;
		private CheckBox checkExcludePast;
		private ComboBox comboCategory;
		private Label label2;
		private UI.Button butAdj;
		private ValidDouble textAdjustment;
		#endregion

		private Patient PatCur;
		private PayPlan _payPlanCur;
		private PayPlan _payPlanOld;
		///<summary>Only used for new payment plan.  Pass in the starting amount.  Usually the patient account balance.</summary>
		public double TotalAmt;
		///<summary>Family for the patient of this payplan.  Used to display insurance info.</summary>
		private Family FamCur;
		///<summary>Used to display insurance info.</summary>
		private List <InsPlan> InsPlanList;
		//private List<PayPlanCharge> ChargeList;
		private double AmtPaid;
		private double TotPrinc;
		private double TotInt;
		private double TotPrincIntAdj;
		private List<InsSub> SubList;
		///<summary>This form is reused as long as this parent form remains open.</summary>
		private FormPaymentPlanOptions FormPayPlanOpts;
		///<summary>Cached list of PayPlanCharges.</summary>
		private List<PayPlanCharge> _listPayPlanCharges;
		private Def[] _arrayAccountColors;//Putting this here so we do one DB call for colors instead of many.  They'll never change.
		private FormPayPlanRecalculate _formPayPlanRecalculate;
		private List<PaySplit> _listPaySplits;
		private DataTable _bundledClaimProcs;
		private string _payPlanNote;
		private int _roundDec=CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits;
		///<summary>Cached list of clinics available to user. Also includes a dummy Clinic at index 0 for "none".</summary>
		private List<Clinic> _listClinics;
		///<summary>Filtered list of providers based on which clinic is selected. If no clinic is selected displays all providers. Also includes a dummy clinic at index 0 for "none"</summary>
		private List<Provider> _listProviders;
		///<summary>Used to keep track of the current clinic selected. This is because it may be a clinic that is not in _listClinics.</summary>
		private long _selectedClinicNum;
		///<summary>Instead of relying on _listProviders[comboProv.SelectedIndex] to determine the selected Provider we use this variable to store it explicitly.</summary>
		private long _selectedProvNum;
		private bool _isSigOldValid;		
		///<summary>Go to the specified patnum.  Upon dialog close, if this number is not 0, then patients.Cur will be changed to this new patnum, and Account refreshed to the new patient.</summary>
		public long GotoPatNum;
		///<summary>If true this plan tracks expected insurance payments. If false it tracks patient payments.</summary>
		public bool IsInsPayPlan;
		///<summary></summary>
		public bool IsNew;
		///<summary>List of Negative adjustments. Will be inserted into DB as negative adjustment. </summary>
		private List<Adjustment> _listAdjustments=new List<Adjustment>();

		///<summary>Keep a total of our negative adjustments for this payment plan.</summary>
		private double _totalNegAdjAmt {
			get {
				return _listPayPlanCharges.FindAll(x => x.ChargeType==PayPlanChargeType.Debit && x.Principal<0).Sum(x => x.Principal);
			}
		}

		///<summary>Total of the adjustments made to the payment plan that have not come due yet. </summary>
		private double _totalNegFutureAdjs {
			get {
				return _listPayPlanCharges.FindAll(x => x.ChargeType==PayPlanChargeType.Debit && x.Principal<0 
					&& x.ChargeDate > DateTime.Today).Sum(x => x.Principal);
			}
		}

		///<summary>Running total of the amount of future debits.</summary>
		private double _totalFutureDebits {
			get {
				return _listPayPlanCharges.FindAll(x => x.ChargeType==PayPlanChargeType.Debit && x.Principal>=0 
					&& x.ChargeDate > DateTime.Today).Sum(x => x.Principal);
			}
		}

		///<summary>The supplied payment plan should already have been saved in the database.</summary>
		public FormPayPlan(PayPlan payPlanCur) {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			PatCur=Patients.GetPat(payPlanCur.PatNum);
			_payPlanCur=payPlanCur.Copy();
			_payPlanOld=_payPlanCur.Copy();
			FamCur=Patients.GetFamily(PatCur.PatNum);
			SubList=InsSubs.RefreshForFam(FamCur);
			InsPlanList=InsPlans.RefreshForSubList(SubList);
			FormPayPlanOpts=new FormPaymentPlanOptions(_payPlanCur.PaySchedule);
			_formPayPlanRecalculate=new FormPayPlanRecalculate();
			if(_payPlanCur.PlanNum!=0) {
				IsInsPayPlan=true;//This can also be set to true on the way in before a PlanNum has been assigned.
			}
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPayPlan));
			this.pd2 = new System.Drawing.Printing.PrintDocument();
			this.labelTotalTx = new System.Windows.Forms.Label();
			this.textTotalTxAmt = new OpenDental.ValidDouble();
			this.butAddTxCredits = new OpenDental.UI.Button();
			this.textDue = new System.Windows.Forms.TextBox();
			this.textBalance = new OpenDental.ValidDouble();
			this.textInterest = new System.Windows.Forms.TextBox();
			this.textPayment = new OpenDental.ValidDouble();
			this.textPrincipal = new OpenDental.ValidDouble();
			this.labelTotals = new System.Windows.Forms.Label();
			this.groupProvClin = new System.Windows.Forms.GroupBox();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.butPickProv = new OpenDental.UI.Button();
			this.comboProv = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.labelTxAmtInfo = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textCompletedAmt = new OpenDental.ValidDouble();
			this.textPrincPaid = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.butAdd = new OpenDental.UI.Button();
			this.butClear = new OpenDental.UI.Button();
			this.butChangePlan = new OpenDental.UI.Button();
			this.textInsPlan = new System.Windows.Forms.TextBox();
			this.labelInsPlan = new System.Windows.Forms.Label();
			this.gridCharges = new OpenDental.UI.ODGrid();
			this.textNote = new OpenDental.ODtextBox();
			this.butDelete = new OpenDental.UI.Button();
			this.textAccumulatedDue = new System.Windows.Forms.TextBox();
			this.textAmtPaid = new System.Windows.Forms.TextBox();
			this.butGoToPat = new OpenDental.UI.Button();
			this.textPatient = new System.Windows.Forms.TextBox();
			this.butGoToGuar = new OpenDental.UI.Button();
			this.textDate = new OpenDental.ValidDate();
			this.butChangeGuar = new OpenDental.UI.Button();
			this.textGuarantor = new System.Windows.Forms.TextBox();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.groupTerms = new System.Windows.Forms.GroupBox();
			this.butRecalculate = new OpenDental.UI.Button();
			this.butMoreOptions = new OpenDental.UI.Button();
			this.textAPR = new OpenDental.ValidDouble();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.textPaymentCount = new OpenDental.ValidNumber();
			this.label7 = new System.Windows.Forms.Label();
			this.textPeriodPayment = new OpenDental.ValidDouble();
			this.label8 = new System.Windows.Forms.Label();
			this.textDownPayment = new OpenDental.ValidDouble();
			this.label11 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.textDateFirstPay = new OpenDental.ValidDate();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textAmount = new OpenDental.ValidDouble();
			this.butCreateSched = new OpenDental.UI.Button();
			this.labelDateAgreement = new System.Windows.Forms.Label();
			this.labelGuarantor = new System.Windows.Forms.Label();
			this.textTotalCost = new System.Windows.Forms.TextBox();
			this.label15 = new System.Windows.Forms.Label();
			this.butPrint = new OpenDental.UI.Button();
			this.butClosePlan = new OpenDental.UI.Button();
			this.labelClosed = new System.Windows.Forms.Label();
			this.butSignPrint = new OpenDental.UI.Button();
			this.signatureBoxWrapper = new OpenDental.UI.SignatureBoxWrapper();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.checkExcludePast = new System.Windows.Forms.CheckBox();
			this.comboCategory = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.butAdj = new OpenDental.UI.Button();
			this.textAdjustment = new OpenDental.ValidDouble();
			this.groupProvClin.SuspendLayout();
			this.groupTerms.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelTotalTx
			// 
			this.labelTotalTx.Location = new System.Drawing.Point(4, 497);
			this.labelTotalTx.Name = "labelTotalTx";
			this.labelTotalTx.Size = new System.Drawing.Size(141, 17);
			this.labelTotalTx.TabIndex = 147;
			this.labelTotalTx.Text = "Total Tx Amt";
			this.labelTotalTx.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTotalTxAmt
			// 
			this.textTotalTxAmt.Location = new System.Drawing.Point(146, 495);
			this.textTotalTxAmt.MaxVal = 100000000D;
			this.textTotalTxAmt.MinVal = -100000000D;
			this.textTotalTxAmt.Name = "textTotalTxAmt";
			this.textTotalTxAmt.ReadOnly = true;
			this.textTotalTxAmt.Size = new System.Drawing.Size(85, 20);
			this.textTotalTxAmt.TabIndex = 148;
			// 
			// butAddTxCredits
			// 
			this.butAddTxCredits.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddTxCredits.Autosize = true;
			this.butAddTxCredits.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddTxCredits.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddTxCredits.CornerRadius = 4F;
			this.butAddTxCredits.Location = new System.Drawing.Point(239, 495);
			this.butAddTxCredits.Name = "butAddTxCredits";
			this.butAddTxCredits.Size = new System.Drawing.Size(93, 19);
			this.butAddTxCredits.TabIndex = 146;
			this.butAddTxCredits.Text = "View Tx Credits";
			this.butAddTxCredits.Click += new System.EventHandler(this.butPayPlanTx_Click);
			// 
			// textDue
			// 
			this.textDue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textDue.Location = new System.Drawing.Point(737, 507);
			this.textDue.Name = "textDue";
			this.textDue.ReadOnly = true;
			this.textDue.Size = new System.Drawing.Size(60, 20);
			this.textDue.TabIndex = 145;
			this.textDue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textBalance
			// 
			this.textBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textBalance.Location = new System.Drawing.Point(928, 507);
			this.textBalance.MaxVal = 100000000D;
			this.textBalance.MinVal = -100000000D;
			this.textBalance.Name = "textBalance";
			this.textBalance.ReadOnly = true;
			this.textBalance.Size = new System.Drawing.Size(73, 20);
			this.textBalance.TabIndex = 144;
			this.textBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textInterest
			// 
			this.textInterest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textInterest.Location = new System.Drawing.Point(685, 507);
			this.textInterest.Name = "textInterest";
			this.textInterest.ReadOnly = true;
			this.textInterest.Size = new System.Drawing.Size(52, 20);
			this.textInterest.TabIndex = 141;
			this.textInterest.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textPayment
			// 
			this.textPayment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textPayment.Location = new System.Drawing.Point(797, 507);
			this.textPayment.MaxVal = 100000000D;
			this.textPayment.MinVal = -100000000D;
			this.textPayment.Name = "textPayment";
			this.textPayment.ReadOnly = true;
			this.textPayment.Size = new System.Drawing.Size(60, 20);
			this.textPayment.TabIndex = 140;
			this.textPayment.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textPrincipal
			// 
			this.textPrincipal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textPrincipal.Location = new System.Drawing.Point(625, 507);
			this.textPrincipal.MaxVal = 100000000D;
			this.textPrincipal.MinVal = -100000000D;
			this.textPrincipal.Name = "textPrincipal";
			this.textPrincipal.ReadOnly = true;
			this.textPrincipal.Size = new System.Drawing.Size(60, 20);
			this.textPrincipal.TabIndex = 139;
			this.textPrincipal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelTotals
			// 
			this.labelTotals.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelTotals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTotals.Location = new System.Drawing.Point(513, 510);
			this.labelTotals.Name = "labelTotals";
			this.labelTotals.Size = new System.Drawing.Size(112, 17);
			this.labelTotals.TabIndex = 142;
			this.labelTotals.Text = "Current Totals";
			this.labelTotals.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupProvClin
			// 
			this.groupProvClin.Controls.Add(this.comboClinic);
			this.groupProvClin.Controls.Add(this.butPickProv);
			this.groupProvClin.Controls.Add(this.comboProv);
			this.groupProvClin.Controls.Add(this.labelClinic);
			this.groupProvClin.Controls.Add(this.label16);
			this.groupProvClin.Location = new System.Drawing.Point(4, 99);
			this.groupProvClin.Name = "groupProvClin";
			this.groupProvClin.Size = new System.Drawing.Size(349, 65);
			this.groupProvClin.TabIndex = 13;
			this.groupProvClin.TabStop = false;
			this.groupProvClin.Text = "Same for all charges";
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(134, 39);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(177, 21);
			this.comboClinic.TabIndex = 3;
			this.comboClinic.SelectedIndexChanged += new System.EventHandler(this.comboClinic_SelectedIndexChanged);
			// 
			// butPickProv
			// 
			this.butPickProv.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickProv.Autosize = false;
			this.butPickProv.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickProv.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickProv.CornerRadius = 2F;
			this.butPickProv.Location = new System.Drawing.Point(317, 14);
			this.butPickProv.Name = "butPickProv";
			this.butPickProv.Size = new System.Drawing.Size(18, 21);
			this.butPickProv.TabIndex = 2;
			this.butPickProv.Text = "...";
			this.butPickProv.Click += new System.EventHandler(this.butPickProv_Click);
			// 
			// comboProv
			// 
			this.comboProv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProv.Location = new System.Drawing.Point(134, 14);
			this.comboProv.MaxDropDownItems = 30;
			this.comboProv.Name = "comboProv";
			this.comboProv.Size = new System.Drawing.Size(177, 21);
			this.comboProv.TabIndex = 1;
			this.comboProv.SelectedIndexChanged += new System.EventHandler(this.comboProv_SelectedIndexChanged);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(36, 41);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(96, 16);
			this.labelClinic.TabIndex = 0;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(33, 18);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(100, 16);
			this.label16.TabIndex = 0;
			this.label16.Text = "Provider";
			this.label16.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelTxAmtInfo
			// 
			this.labelTxAmtInfo.Location = new System.Drawing.Point(143, 514);
			this.labelTxAmtInfo.Name = "labelTxAmtInfo";
			this.labelTxAmtInfo.Size = new System.Drawing.Size(163, 28);
			this.labelTxAmtInfo.TabIndex = 0;
			this.labelTxAmtInfo.Text = "This should usually match the total amount of the pay plan.";
			this.labelTxAmtInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(4, 475);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(141, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Tx Completed Amt";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textCompletedAmt
			// 
			this.textCompletedAmt.Location = new System.Drawing.Point(146, 473);
			this.textCompletedAmt.MaxVal = 100000000D;
			this.textCompletedAmt.MinVal = -100000000D;
			this.textCompletedAmt.Name = "textCompletedAmt";
			this.textCompletedAmt.ReadOnly = true;
			this.textCompletedAmt.Size = new System.Drawing.Size(85, 20);
			this.textCompletedAmt.TabIndex = 2;
			// 
			// textPrincPaid
			// 
			this.textPrincPaid.Location = new System.Drawing.Point(146, 451);
			this.textPrincPaid.Name = "textPrincPaid";
			this.textPrincPaid.ReadOnly = true;
			this.textPrincPaid.Size = new System.Drawing.Size(85, 20);
			this.textPrincPaid.TabIndex = 0;
			this.textPrincPaid.TabStop = false;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(4, 453);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(141, 17);
			this.label14.TabIndex = 0;
			this.label14.Text = "Principal paid so far";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(361, 661);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(84, 24);
			this.butAdd.TabIndex = 4;
			this.butAdd.Text = "Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butClear
			// 
			this.butClear.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClear.Autosize = true;
			this.butClear.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClear.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClear.CornerRadius = 4F;
			this.butClear.Location = new System.Drawing.Point(564, 661);
			this.butClear.Name = "butClear";
			this.butClear.Size = new System.Drawing.Size(99, 24);
			this.butClear.TabIndex = 5;
			this.butClear.Text = "Clear Schedule";
			this.butClear.Click += new System.EventHandler(this.butClear_Click);
			// 
			// butChangePlan
			// 
			this.butChangePlan.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChangePlan.Autosize = true;
			this.butChangePlan.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChangePlan.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChangePlan.CornerRadius = 4F;
			this.butChangePlan.Location = new System.Drawing.Point(299, 170);
			this.butChangePlan.Name = "butChangePlan";
			this.butChangePlan.Size = new System.Drawing.Size(75, 22);
			this.butChangePlan.TabIndex = 15;
			this.butChangePlan.Text = "C&hange";
			this.butChangePlan.Click += new System.EventHandler(this.butChangePlan_Click);
			// 
			// textInsPlan
			// 
			this.textInsPlan.Location = new System.Drawing.Point(123, 171);
			this.textInsPlan.Name = "textInsPlan";
			this.textInsPlan.ReadOnly = true;
			this.textInsPlan.Size = new System.Drawing.Size(177, 20);
			this.textInsPlan.TabIndex = 0;
			this.textInsPlan.TabStop = false;
			// 
			// labelInsPlan
			// 
			this.labelInsPlan.Location = new System.Drawing.Point(4, 171);
			this.labelInsPlan.Name = "labelInsPlan";
			this.labelInsPlan.Size = new System.Drawing.Size(116, 17);
			this.labelInsPlan.TabIndex = 0;
			this.labelInsPlan.Text = "Insurance Plan";
			this.labelInsPlan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// gridCharges
			// 
			this.gridCharges.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridCharges.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridCharges.HasAddButton = false;
			this.gridCharges.HasDropDowns = false;
			this.gridCharges.HasMultilineHeaders = false;
			this.gridCharges.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridCharges.HeaderHeight = 15;
			this.gridCharges.HScrollVisible = false;
			this.gridCharges.Location = new System.Drawing.Point(380, 12);
			this.gridCharges.Name = "gridCharges";
			this.gridCharges.ScrollValue = 0;
			this.gridCharges.Size = new System.Drawing.Size(640, 495);
			this.gridCharges.TabIndex = 41;
			this.gridCharges.Title = "Amortization Schedule";
			this.gridCharges.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridCharges.TitleHeight = 18;
			this.gridCharges.TranslationName = "PayPlanAmortization";
			this.gridCharges.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridCharges_CellDoubleClick);
			// 
			// textNote
			// 
			this.textNote.AcceptsTab = true;
			this.textNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.textNote.BackColor = System.Drawing.SystemColors.Window;
			this.textNote.DetectLinksEnabled = false;
			this.textNote.DetectUrls = false;
			this.textNote.Location = new System.Drawing.Point(380, 557);
			this.textNote.Name = "textNote";
			this.textNote.QuickPasteType = OpenDentBusiness.QuickPasteType.PayPlan;
			this.textNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textNote.Size = new System.Drawing.Size(594, 81);
			this.textNote.SpellCheckIsEnabled = false;
			this.textNote.TabIndex = 3;
			this.textNote.TabStop = false;
			this.textNote.Text = "";
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
			this.butDelete.Location = new System.Drawing.Point(12, 661);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(84, 24);
			this.butDelete.TabIndex = 9;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// textAccumulatedDue
			// 
			this.textAccumulatedDue.Location = new System.Drawing.Point(146, 407);
			this.textAccumulatedDue.Name = "textAccumulatedDue";
			this.textAccumulatedDue.ReadOnly = true;
			this.textAccumulatedDue.Size = new System.Drawing.Size(85, 20);
			this.textAccumulatedDue.TabIndex = 0;
			this.textAccumulatedDue.TabStop = false;
			// 
			// textAmtPaid
			// 
			this.textAmtPaid.Location = new System.Drawing.Point(146, 429);
			this.textAmtPaid.Name = "textAmtPaid";
			this.textAmtPaid.ReadOnly = true;
			this.textAmtPaid.Size = new System.Drawing.Size(85, 20);
			this.textAmtPaid.TabIndex = 0;
			this.textAmtPaid.TabStop = false;
			// 
			// butGoToPat
			// 
			this.butGoToPat.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGoToPat.Autosize = true;
			this.butGoToPat.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGoToPat.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGoToPat.CornerRadius = 4F;
			this.butGoToPat.Location = new System.Drawing.Point(280, 32);
			this.butGoToPat.Name = "butGoToPat";
			this.butGoToPat.Size = new System.Drawing.Size(75, 22);
			this.butGoToPat.TabIndex = 10;
			this.butGoToPat.Text = "&Go To";
			this.butGoToPat.Click += new System.EventHandler(this.butGoToPat_Click);
			// 
			// textPatient
			// 
			this.textPatient.Location = new System.Drawing.Point(102, 33);
			this.textPatient.Name = "textPatient";
			this.textPatient.ReadOnly = true;
			this.textPatient.Size = new System.Drawing.Size(177, 20);
			this.textPatient.TabIndex = 0;
			this.textPatient.TabStop = false;
			// 
			// butGoToGuar
			// 
			this.butGoToGuar.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGoToGuar.Autosize = true;
			this.butGoToGuar.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGoToGuar.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGoToGuar.CornerRadius = 4F;
			this.butGoToGuar.Location = new System.Drawing.Point(280, 54);
			this.butGoToGuar.Name = "butGoToGuar";
			this.butGoToGuar.Size = new System.Drawing.Size(75, 22);
			this.butGoToGuar.TabIndex = 11;
			this.butGoToGuar.Text = "Go &To";
			this.butGoToGuar.Click += new System.EventHandler(this.butGoTo_Click);
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(123, 193);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(85, 20);
			this.textDate.TabIndex = 16;
			// 
			// butChangeGuar
			// 
			this.butChangeGuar.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChangeGuar.Autosize = true;
			this.butChangeGuar.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChangeGuar.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChangeGuar.CornerRadius = 4F;
			this.butChangeGuar.Location = new System.Drawing.Point(280, 76);
			this.butChangeGuar.Name = "butChangeGuar";
			this.butChangeGuar.Size = new System.Drawing.Size(75, 22);
			this.butChangeGuar.TabIndex = 12;
			this.butChangeGuar.Text = "C&hange";
			this.butChangeGuar.Click += new System.EventHandler(this.butChangeGuar_Click);
			// 
			// textGuarantor
			// 
			this.textGuarantor.Location = new System.Drawing.Point(102, 55);
			this.textGuarantor.Name = "textGuarantor";
			this.textGuarantor.ReadOnly = true;
			this.textGuarantor.Size = new System.Drawing.Size(177, 20);
			this.textGuarantor.TabIndex = 0;
			this.textGuarantor.TabStop = false;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(851, 661);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 7;
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
			this.butCancel.Location = new System.Drawing.Point(929, 661);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 8;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(379, 537);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(92, 17);
			this.label10.TabIndex = 0;
			this.label10.Text = "Note";
			this.label10.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(4, 409);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(141, 17);
			this.label13.TabIndex = 0;
			this.label13.Text = "Accumulated Due";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(4, 431);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(141, 17);
			this.label12.TabIndex = 0;
			this.label12.Text = "Paid so far";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(6, 33);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(94, 17);
			this.label9.TabIndex = 0;
			this.label9.Text = "Patient";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupTerms
			// 
			this.groupTerms.Controls.Add(this.butRecalculate);
			this.groupTerms.Controls.Add(this.butMoreOptions);
			this.groupTerms.Controls.Add(this.textAPR);
			this.groupTerms.Controls.Add(this.groupBox3);
			this.groupTerms.Controls.Add(this.textDownPayment);
			this.groupTerms.Controls.Add(this.label11);
			this.groupTerms.Controls.Add(this.label6);
			this.groupTerms.Controls.Add(this.textDateFirstPay);
			this.groupTerms.Controls.Add(this.label5);
			this.groupTerms.Controls.Add(this.label4);
			this.groupTerms.Controls.Add(this.textAmount);
			this.groupTerms.Controls.Add(this.butCreateSched);
			this.groupTerms.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupTerms.Location = new System.Drawing.Point(4, 212);
			this.groupTerms.Name = "groupTerms";
			this.groupTerms.Size = new System.Drawing.Size(370, 170);
			this.groupTerms.TabIndex = 1;
			this.groupTerms.TabStop = false;
			this.groupTerms.Text = "Terms";
			// 
			// butRecalculate
			// 
			this.butRecalculate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRecalculate.Autosize = true;
			this.butRecalculate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRecalculate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRecalculate.CornerRadius = 4F;
			this.butRecalculate.Location = new System.Drawing.Point(250, 82);
			this.butRecalculate.Name = "butRecalculate";
			this.butRecalculate.Size = new System.Drawing.Size(99, 24);
			this.butRecalculate.TabIndex = 145;
			this.butRecalculate.Text = "Recalculate";
			this.butRecalculate.UseVisualStyleBackColor = true;
			this.butRecalculate.Click += new System.EventHandler(this.butRecalculate_Click);
			// 
			// butMoreOptions
			// 
			this.butMoreOptions.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMoreOptions.Autosize = true;
			this.butMoreOptions.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMoreOptions.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMoreOptions.CornerRadius = 4F;
			this.butMoreOptions.Location = new System.Drawing.Point(250, 110);
			this.butMoreOptions.Name = "butMoreOptions";
			this.butMoreOptions.Size = new System.Drawing.Size(99, 24);
			this.butMoreOptions.TabIndex = 7;
			this.butMoreOptions.Text = "More Options";
			this.butMoreOptions.Click += new System.EventHandler(this.butMoreOptions_Click);
			// 
			// textAPR
			// 
			this.textAPR.Location = new System.Drawing.Point(142, 78);
			this.textAPR.MaxVal = 100000000D;
			this.textAPR.MinVal = 0D;
			this.textAPR.Name = "textAPR";
			this.textAPR.Size = new System.Drawing.Size(47, 20);
			this.textAPR.TabIndex = 4;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.textPaymentCount);
			this.groupBox3.Controls.Add(this.label7);
			this.groupBox3.Controls.Add(this.textPeriodPayment);
			this.groupBox3.Controls.Add(this.label8);
			this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox3.Location = new System.Drawing.Point(9, 101);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(235, 64);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Either";
			// 
			// textPaymentCount
			// 
			this.textPaymentCount.Location = new System.Drawing.Point(133, 17);
			this.textPaymentCount.MaxVal = 255;
			this.textPaymentCount.MinVal = 1;
			this.textPaymentCount.Name = "textPaymentCount";
			this.textPaymentCount.Size = new System.Drawing.Size(47, 20);
			this.textPaymentCount.TabIndex = 1;
			this.textPaymentCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textPaymentCount_KeyPress);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 40);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(122, 17);
			this.label7.TabIndex = 0;
			this.label7.Text = "Payment Amt";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPeriodPayment
			// 
			this.textPeriodPayment.Location = new System.Drawing.Point(133, 39);
			this.textPeriodPayment.MaxVal = 100000000D;
			this.textPeriodPayment.MinVal = 0.01D;
			this.textPeriodPayment.Name = "textPeriodPayment";
			this.textPeriodPayment.Size = new System.Drawing.Size(85, 20);
			this.textPeriodPayment.TabIndex = 2;
			this.textPeriodPayment.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textPeriodPayment_KeyPress);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(7, 18);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(124, 17);
			this.label8.TabIndex = 0;
			this.label8.Text = "Number of Payments";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDownPayment
			// 
			this.textDownPayment.Location = new System.Drawing.Point(142, 56);
			this.textDownPayment.MaxVal = 100000000D;
			this.textDownPayment.MinVal = 0D;
			this.textDownPayment.Name = "textDownPayment";
			this.textDownPayment.Size = new System.Drawing.Size(85, 20);
			this.textDownPayment.TabIndex = 3;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(4, 59);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(136, 17);
			this.label11.TabIndex = 0;
			this.label11.Text = "Down Payment";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(3, 80);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(138, 17);
			this.label6.TabIndex = 0;
			this.label6.Text = "APR (for example 0 or 18)";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDateFirstPay
			// 
			this.textDateFirstPay.Location = new System.Drawing.Point(142, 34);
			this.textDateFirstPay.Name = "textDateFirstPay";
			this.textDateFirstPay.Size = new System.Drawing.Size(85, 20);
			this.textDateFirstPay.TabIndex = 2;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(5, 36);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(135, 17);
			this.label5.TabIndex = 0;
			this.label5.Text = "Date of First Payment";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(5, 14);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(134, 17);
			this.label4.TabIndex = 0;
			this.label4.Text = "Total Amount";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textAmount
			// 
			this.textAmount.Location = new System.Drawing.Point(142, 13);
			this.textAmount.MaxVal = 100000000D;
			this.textAmount.MinVal = 0.01D;
			this.textAmount.Name = "textAmount";
			this.textAmount.Size = new System.Drawing.Size(85, 20);
			this.textAmount.TabIndex = 1;
			this.textAmount.Validating += new System.ComponentModel.CancelEventHandler(this.textAmount_Validating);
			// 
			// butCreateSched
			// 
			this.butCreateSched.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCreateSched.Autosize = true;
			this.butCreateSched.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCreateSched.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCreateSched.CornerRadius = 4F;
			this.butCreateSched.Location = new System.Drawing.Point(250, 138);
			this.butCreateSched.Name = "butCreateSched";
			this.butCreateSched.Size = new System.Drawing.Size(99, 24);
			this.butCreateSched.TabIndex = 6;
			this.butCreateSched.Text = "Create Schedule";
			this.butCreateSched.Click += new System.EventHandler(this.butCreateSched_Click);
			// 
			// labelDateAgreement
			// 
			this.labelDateAgreement.Location = new System.Drawing.Point(4, 194);
			this.labelDateAgreement.Name = "labelDateAgreement";
			this.labelDateAgreement.Size = new System.Drawing.Size(117, 17);
			this.labelDateAgreement.TabIndex = 0;
			this.labelDateAgreement.Text = "Date of Agreement";
			this.labelDateAgreement.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelGuarantor
			// 
			this.labelGuarantor.Location = new System.Drawing.Point(6, 55);
			this.labelGuarantor.Name = "labelGuarantor";
			this.labelGuarantor.Size = new System.Drawing.Size(98, 17);
			this.labelGuarantor.TabIndex = 0;
			this.labelGuarantor.Text = "Guarantor";
			this.labelGuarantor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTotalCost
			// 
			this.textTotalCost.Location = new System.Drawing.Point(146, 385);
			this.textTotalCost.Name = "textTotalCost";
			this.textTotalCost.ReadOnly = true;
			this.textTotalCost.Size = new System.Drawing.Size(85, 20);
			this.textTotalCost.TabIndex = 0;
			this.textTotalCost.TabStop = false;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(4, 385);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(139, 17);
			this.label15.TabIndex = 0;
			this.label15.Text = "Total Cost of Loan";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
			this.butPrint.Location = new System.Drawing.Point(763, 661);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(85, 24);
			this.butPrint.TabIndex = 6;
			this.butPrint.Text = "&Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// butClosePlan
			// 
			this.butClosePlan.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClosePlan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butClosePlan.Autosize = true;
			this.butClosePlan.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClosePlan.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClosePlan.CornerRadius = 4F;
			this.butClosePlan.Image = global::OpenDental.Properties.Resources.close_door;
			this.butClosePlan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butClosePlan.Location = new System.Drawing.Point(102, 661);
			this.butClosePlan.Name = "butClosePlan";
			this.butClosePlan.Size = new System.Drawing.Size(84, 24);
			this.butClosePlan.TabIndex = 149;
			this.butClosePlan.Text = "Close Plan";
			this.butClosePlan.Click += new System.EventHandler(this.butCloseOut_Click);
			// 
			// labelClosed
			// 
			this.labelClosed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.labelClosed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelClosed.ForeColor = System.Drawing.Color.Red;
			this.labelClosed.Location = new System.Drawing.Point(487, 638);
			this.labelClosed.Name = "labelClosed";
			this.labelClosed.Size = new System.Drawing.Size(512, 15);
			this.labelClosed.TabIndex = 150;
			this.labelClosed.Text = "This payment plan is closed. You must click \"Reopen\" before editing it.";
			this.labelClosed.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.labelClosed.Visible = false;
			// 
			// butSignPrint
			// 
			this.butSignPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSignPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butSignPrint.Autosize = true;
			this.butSignPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSignPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSignPrint.CornerRadius = 4F;
			this.butSignPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSignPrint.Location = new System.Drawing.Point(667, 661);
			this.butSignPrint.Name = "butSignPrint";
			this.butSignPrint.Size = new System.Drawing.Size(92, 24);
			this.butSignPrint.TabIndex = 151;
			this.butSignPrint.Text = "Sign && Print";
			this.butSignPrint.Visible = false;
			this.butSignPrint.Click += new System.EventHandler(this.butSignPrint_Click);
			// 
			// signatureBoxWrapper
			// 
			this.signatureBoxWrapper.BackColor = System.Drawing.SystemColors.ControlDark;
			this.signatureBoxWrapper.Enabled = false;
			this.signatureBoxWrapper.Location = new System.Drawing.Point(6, 19);
			this.signatureBoxWrapper.Name = "signatureBoxWrapper";
			this.signatureBoxWrapper.SignatureMode = OpenDental.UI.SignatureBoxWrapper.SigMode.Default;
			this.signatureBoxWrapper.Size = new System.Drawing.Size(351, 65);
			this.signatureBoxWrapper.TabIndex = 183;
			this.signatureBoxWrapper.UserSig = null;
			this.signatureBoxWrapper.Visible = false;
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Controls.Add(this.signatureBoxWrapper);
			this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox4.Location = new System.Drawing.Point(59, 542);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(363, 96);
			this.groupBox4.TabIndex = 184;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Signature";
			this.groupBox4.Visible = false;
			// 
			// checkExcludePast
			// 
			this.checkExcludePast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.checkExcludePast.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkExcludePast.Location = new System.Drawing.Point(379, 510);
			this.checkExcludePast.Name = "checkExcludePast";
			this.checkExcludePast.Size = new System.Drawing.Size(134, 17);
			this.checkExcludePast.TabIndex = 186;
			this.checkExcludePast.Text = "Exclude past activity";
			this.checkExcludePast.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.checkExcludePast.UseVisualStyleBackColor = true;
			this.checkExcludePast.CheckedChanged += new System.EventHandler(this.checkExcludePast_CheckedChanged);
			// 
			// comboCategory
			// 
			this.comboCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCategory.Location = new System.Drawing.Point(102, 8);
			this.comboCategory.MaxDropDownItems = 30;
			this.comboCategory.Name = "comboCategory";
			this.comboCategory.Size = new System.Drawing.Size(177, 21);
			this.comboCategory.TabIndex = 187;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(94, 17);
			this.label2.TabIndex = 188;
			this.label2.Text = "Category";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butAdj
			// 
			this.butAdj.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdj.Autosize = true;
			this.butAdj.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdj.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdj.CornerRadius = 4F;
			this.butAdj.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdj.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdj.Location = new System.Drawing.Point(449, 661);
			this.butAdj.Name = "butAdj";
			this.butAdj.Size = new System.Drawing.Size(111, 24);
			this.butAdj.TabIndex = 189;
			this.butAdj.Text = "Add Adjustment";
			this.butAdj.Click += new System.EventHandler(this.butAdj_Click);
			// 
			// textAdjustment
			// 
			this.textAdjustment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.textAdjustment.Location = new System.Drawing.Point(857, 507);
			this.textAdjustment.MaxVal = 100000000D;
			this.textAdjustment.MinVal = -100000000D;
			this.textAdjustment.Name = "textAdjustment";
			this.textAdjustment.ReadOnly = true;
			this.textAdjustment.Size = new System.Drawing.Size(71, 20);
			this.textAdjustment.TabIndex = 190;
			this.textAdjustment.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// FormPayPlan
			// 
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(1023, 696);
			this.Controls.Add(this.textAdjustment);
			this.Controls.Add(this.butAdj);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.comboCategory);
			this.Controls.Add(this.checkExcludePast);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.butSignPrint);
			this.Controls.Add(this.labelClosed);
			this.Controls.Add(this.butClosePlan);
			this.Controls.Add(this.labelTotalTx);
			this.Controls.Add(this.textTotalTxAmt);
			this.Controls.Add(this.butAddTxCredits);
			this.Controls.Add(this.textDue);
			this.Controls.Add(this.textBalance);
			this.Controls.Add(this.textInterest);
			this.Controls.Add(this.textPayment);
			this.Controls.Add(this.textPrincipal);
			this.Controls.Add(this.labelTotals);
			this.Controls.Add(this.groupProvClin);
			this.Controls.Add(this.labelTxAmtInfo);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textCompletedAmt);
			this.Controls.Add(this.textPrincPaid);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butClear);
			this.Controls.Add(this.butChangePlan);
			this.Controls.Add(this.textInsPlan);
			this.Controls.Add(this.labelInsPlan);
			this.Controls.Add(this.gridCharges);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.textAccumulatedDue);
			this.Controls.Add(this.textAmtPaid);
			this.Controls.Add(this.butGoToPat);
			this.Controls.Add(this.textPatient);
			this.Controls.Add(this.butGoToGuar);
			this.Controls.Add(this.textDate);
			this.Controls.Add(this.butChangeGuar);
			this.Controls.Add(this.textGuarantor);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.groupTerms);
			this.Controls.Add(this.labelDateAgreement);
			this.Controls.Add(this.labelGuarantor);
			this.Controls.Add(this.textTotalCost);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.butPrint);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(990, 735);
			this.Name = "FormPayPlan";
			this.ShowInTaskbar = false;
			this.Text = "Payment Plan";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormPayPlan_Closing);
			this.Load += new System.EventHandler(this.FormPayPlan_Load);
			this.groupProvClin.ResumeLayout(false);
			this.groupTerms.ResumeLayout(false);
			this.groupTerms.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormPayPlan_Load(object sender,System.EventArgs e) {
			List<Def> listCats=Defs.GetDefsForCategory(DefCat.PayPlanCategories,true);
			comboCategory.Items.Add(new ODBoxItem<long>("None",0));
			foreach(Def category in listCats) {
				ODBoxItem<long> comboItem=new ODBoxItem<long>(category.ItemName,category.DefNum);
				comboCategory.Items.Add(comboItem);
				if(category.DefNum==_payPlanCur.PlanCategory) {
					comboCategory.SelectedItem=comboItem;
				}
			}
			if(_payPlanCur.PlanCategory <= 0) {
				comboCategory.SelectedIndex=0;
			}
			textPatient.Text=Patients.GetLim(_payPlanCur.PatNum).GetNameLF();
			textGuarantor.Text=Patients.GetLim(_payPlanCur.Guarantor).GetNameLF();
			if(_payPlanCur.NumberOfPayments!=0) {
				textPaymentCount.Text=_payPlanCur.NumberOfPayments.ToString();
			}
			else {
				textPeriodPayment.Text=_payPlanCur.PayAmt.ToString("f");
			}
			textDownPayment.Text=_payPlanCur.DownPayment.ToString("f");
			textDate.Text=_payPlanCur.PayPlanDate.ToShortDateString();
			if(IsNew) {
				textAmount.Text=TotalAmt.ToString("f");//it won't get filled in FillCharges because there are no charges yet
				//If a plan is created "today" with the customer making their first payment on the spot, they will over pay interest.  
				//If there  is a larger gap than 1 month before the first payment, interest will be under calculated.
				//For now, our temporary solution is to prefill the date of first payment box starting with next months date which is the most accurate for calculating interest.
				textDateFirstPay.Text=DateTime.Now.AddMonths(1).ToShortDateString();
				_listPayPlanCharges=new List<PayPlanCharge>();
			}
			else {
				_listPayPlanCharges=PayPlanCharges.GetForPayPlan(_payPlanCur.PayPlanNum);
			}
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				labelClinic.Visible=false;
				comboClinic.Visible=false;
			}
			else {
				_listClinics=new List<Clinic>() { new Clinic() { Abbr=Lan.g(this,"None") } }; //Seed with "None"
				Clinics.GetForUserod(Security.CurUser).ForEach(x => _listClinics.Add(x));//do not re-organize from cache. They could either be alphabetizeded or sorted by item order.
				_listClinics.ForEach(x => comboClinic.Items.Add(x.Abbr));
				if(IsNew) {
					_selectedClinicNum=PatCur.ClinicNum;
				}
				else if(_listPayPlanCharges.Count==0) {
					_selectedClinicNum=0;
				}
				else {
					_selectedClinicNum=_listPayPlanCharges[0].ClinicNum;
				}
				comboClinic.IndexSelectOrSetText(_listClinics.FindIndex(x => x.ClinicNum==_selectedClinicNum),() => { return Clinics.GetAbbr(_selectedClinicNum); });
			}
			if(_listPayPlanCharges.Count>0) {
				_selectedProvNum=_listPayPlanCharges[0].ProvNum;
			}
			else {
				_selectedProvNum=PatCur.PriProv;
			}
			comboProv.SelectedIndex=-1;
			FillComboProv();
			textAPR.Text=_payPlanCur.APR.ToString();
			AmtPaid=PayPlans.GetAmtPaid(_payPlanCur);
			textAmtPaid.Text=AmtPaid.ToString("f");
			textCompletedAmt.Text=_payPlanCur.CompletedAmt.ToString("f");
			textNote.Text=_payPlanCur.Note;
			_payPlanNote=textNote.Text;
			if(IsInsPayPlan) {
				Text=Lan.g(this,"Insurance Payment Plan");
				textInsPlan.Text=InsPlans.GetDescript(_payPlanCur.PlanNum,FamCur,InsPlanList,_payPlanCur.InsSubNum,SubList);
				labelGuarantor.Visible=false;
				textGuarantor.Visible=false;
				butGoToGuar.Visible=false;
				butChangeGuar.Visible=false;
				textCompletedAmt.ReadOnly=false;
				butAddTxCredits.Visible=false;
				textTotalTxAmt.Visible=false;
				labelTotalTx.Visible=false;
				labelTxAmtInfo.Location=new Point(labelTxAmtInfo.Location.X,labelTxAmtInfo.Location.Y-20);
			}
			else {
				Text=Lan.g(this,"Patient Payment Plan");
				labelInsPlan.Visible=false;
				textInsPlan.Visible=false;
				butChangePlan.Visible=false;
				textDate.Location=new Point(textDate.Location.X+22,textDate.Location.Y);//line up with text boxes below
				labelDateAgreement.Location=new Point(labelDateAgreement.Location.X+22,labelDateAgreement.Location.Y);
			}
			_arrayAccountColors=Defs.GetDefsForCategory(DefCat.AccountColors,true).ToArray();
			//If the amort schedule has been created and the first payment date has passed, don't allow user to change the first payment date or downpayment
			//until the schedule is cleared.
			if(!IsNew && PIn.Date(textDateFirstPay.Text)<DateTime.Today) {
				textDateFirstPay.ReadOnly=true;
				textDownPayment.ReadOnly=true;
			}
			else {
				butRecalculate.Enabled=false;//Don't allow a plan that hasn't started to be recalculated.
			}
			textTotalTxAmt.Text=PayPlans.GetTxTotalAmt(_listPayPlanCharges).ToString("f");
			if(_payPlanCur.IsClosed) {
				butOK.Text=Lan.g(this,"Reopen");
				butDelete.Enabled=false;
				butClosePlan.Enabled=false;
				labelClosed.Visible=true;
			}
			if(PrefC.GetBool(PrefName.PayPlansUseSheets)) {
				Sheet sheetPP=null;
				sheetPP=PayPlanToSheet(_payPlanCur);
				//check to see if sig box is on the sheet
				//hides butPrint and adds butSignPrint,groupbox,and sigwrapper
				for(int i = 0;i<sheetPP.SheetFields.Count;i++) {
					if(sheetPP.SheetFields[i].FieldType==SheetFieldType.SigBox) {
						butPrint.Visible=false;
						butSignPrint.Visible=true;					
					}
				}
			}
			checkExcludePast.Checked=PrefC.GetBool(PrefName.PayPlansExcludePastActivity);
			FillCharges();
			if(_payPlanCur.Signature!="" && _payPlanCur.Signature!=null) {
				//check to see if sheet is signed before showing
				signatureBoxWrapper.Visible=true;
				groupBox4.Visible=true;
				butSignPrint.Text="View && Print";
				signatureBoxWrapper.FillSignature(_payPlanCur.SigIsTopaz,GetKeyDataForSignature(),_payPlanCur.Signature); //fill signature
			}
			if(string.IsNullOrEmpty(_payPlanCur.Signature) || !signatureBoxWrapper.IsValid || signatureBoxWrapper.SigIsBlank) {
				_isSigOldValid=false;
			}
			else {
				_isSigOldValid=true;
			}
			if(IsNew) {
				butDelete.Visible=false;
				butClosePlan.Visible=false;
			}
			if(!Security.IsAuthorized(Permissions.PayPlanEdit)) {
				DisableForm(butGoToGuar,butGoToPat,butCancel,butPrint,checkExcludePast,butAddTxCredits);
			}
		}

		///<summary>Gets the hashstring for generating signatures.</summary>
		public string GetKeyDataForSignature() {
			//strb is a concatenation of the following:
			//pp: DateOfAgreement+ Total Amt+ APR+ Num of Payments+ Payment Amt + Note
			StringBuilder strb = new StringBuilder();
			Sheet sheetPP=null;
			sheetPP=PayPlanToSheet(_payPlanCur);
			strb.Append(_payPlanCur.PayPlanDate.ToShortDateString());
			strb.Append(textAmount.Text);
			strb.Append(_payPlanCur.APR.ToString());
			strb.Append(_payPlanCur.NumberOfPayments.ToString());
			strb.Append(_payPlanCur.PayAmt.ToString("f"));
			strb.Append(textNote.Text);
			strb.Append(Sheets.GetSignatureKey(sheetPP));
			return PayPlans.GetHashStringForSignature(strb.ToString());
		}

		private void comboClinic_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboClinic.SelectedIndex>-1) {
				_selectedClinicNum=_listClinics[comboClinic.SelectedIndex].ClinicNum;
			}
			FillComboProv();
		}

		private void comboProv_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboProv.SelectedIndex>-1) {
				_selectedProvNum=_listProviders[comboProv.SelectedIndex].ProvNum;
			}
		}

		private void butPickProv_Click(object sender,EventArgs e) {
			FormProviderPick formp = new FormProviderPick(_listProviders);
			formp.SelectedProvNum=_selectedProvNum;
			formp.ShowDialog();
			if(formp.DialogResult!=DialogResult.OK) {
				return;
			}
			_selectedProvNum=formp.SelectedProvNum;
			comboProv.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvNum),() => { return Providers.GetLongDesc(_selectedProvNum); });
		}

		///<summary>Fills combo provider based on which clinic is selected and attempts to preserve provider selection if any.</summary>
		private void FillComboProv() {
			if(comboProv.SelectedIndex>-1) {
				_selectedProvNum = _listProviders[comboProv.SelectedIndex].ProvNum;
			}
			_listProviders=Providers.GetProvsForClinic(_selectedClinicNum).OrderBy(x => x.GetLongDesc()).ToList();
			//Select Provider
			comboProv.Items.Clear();
			_listProviders.ForEach(x => comboProv.Items.Add(x.GetLongDesc()));
			comboProv.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvNum),() => { return Providers.GetLongDesc(_selectedProvNum); });
		}

		/// <summary>Called 5 times.  This also fills prov and clinic based on the first charge if not new.</summary>
		private void FillCharges() {
			gridCharges.BeginUpdate();
			gridCharges.Columns.Clear();
			ODGridColumn col;
			//If this column is changed from a date column then the comparer method (ComparePayPlanRows) needs to be updated.
			//If changes are made to the order of the grid, changes need to also be made for butPrint_Click
			col=new ODGridColumn(Lan.g("PayPlanAmortization","Date"),64,HorizontalAlignment.Center);//0
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g("PayPlanAmortization","Provider"),50);//1
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g("PayPlanAmortization","Description"),130);//2
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g("PayPlanAmortization","Principal"),60,HorizontalAlignment.Right);//3
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g("PayPlanAmortization","Interest"),52,HorizontalAlignment.Right);//4
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g("PayPlanAmortization","Due"),60,HorizontalAlignment.Right);//5
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g("PayPlanAmortization","Payment"),60,HorizontalAlignment.Right);//6
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g("PayPlanAmortization","Adjustment"),70,HorizontalAlignment.Right);//7
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g("PayPlanAmortization","Balance"),60,HorizontalAlignment.Right);//8
			gridCharges.Columns.Add(col);
			gridCharges.Rows.Clear();
			List<ODGridRow> listPayPlanRows=new List<ODGridRow>();
			int numCharges=1;
			for(int i=0;i<_listPayPlanCharges.Count;i++) {//Payplan Charges
				if(_listPayPlanCharges[i].ChargeType==PayPlanChargeType.Credit) {
					continue;//hide credits from the amortization grid.
				}
				listPayPlanRows.Add(CreateRowForPayPlanCharge(_listPayPlanCharges[i],numCharges));
				if(!_listPayPlanCharges[i].Note.Trim().ToLower().Contains("recalculated based on")) {//Don't increment the charge # for recalculated charges, since they won't have a #.
					numCharges++;
				}
			}
			if(_payPlanCur.PlanNum==0) {//Normal payplan
				_listPaySplits=new List<PaySplit>();
				DataTable bundledPayments=PaySplits.GetForPayPlan(_payPlanCur.PayPlanNum);
				_listPaySplits=PaySplits.GetFromBundled(bundledPayments);
				for(int i=0;i<_listPaySplits.Count;i++) {
					listPayPlanRows.Add(CreateRowForPaySplit(bundledPayments.Rows[i],_listPaySplits[i]));
				}
			}
			else {//Insurance payplan
				_bundledClaimProcs=ClaimProcs.GetBundlesForPayPlan(_payPlanCur.PayPlanNum);
				for(int i=0;i<_bundledClaimProcs.Rows.Count;i++) {
					listPayPlanRows.Add(CreateRowForClaimProcs(_bundledClaimProcs.Rows[i]));
				}
			}
			listPayPlanRows.Sort(ComparePayPlanRows);
			int totalsRowIndex = -1; //if -1, then don't show a bold line as the first charge showing has not come due yet.
			double balanceAmt=0;
			double negAdjAmt=0;
			TotPrinc=0;
			TotInt=0;
			double TotPay=0;
			for(int i=0;i<listPayPlanRows.Count;i++) {
				if(!checkExcludePast.Checked || DateTime.Parse(listPayPlanRows[i].Cells[0].Text)>DateTime.Today) {
					//Add the row if we aren't excluding past activity or the activity is in the future.
					gridCharges.Rows.Add(listPayPlanRows[i]);
				}
				if(listPayPlanRows[i].Cells[3].Text!="") {//Principal
					TotPrinc+=PIn.Double(listPayPlanRows[i].Cells[3].Text);
					balanceAmt+=PIn.Double(listPayPlanRows[i].Cells[3].Text);
				}
				if(listPayPlanRows[i].Cells[4].Text!="") {//Interest
					TotInt+=PIn.Double(listPayPlanRows[i].Cells[4].Text);
					balanceAmt+=PIn.Double(listPayPlanRows[i].Cells[4].Text);
				}
				else if(listPayPlanRows[i].Cells[6].Text!="") {//Payment
					TotPay+=PIn.Double(listPayPlanRows[i].Cells[6].Text);
					balanceAmt-=PIn.Double(listPayPlanRows[i].Cells[6].Text);
				}
				if(listPayPlanRows[i].Cells[7].Text!="") { //adjustment
					balanceAmt+=PIn.Double(listPayPlanRows[i].Cells[7].Text);//+ because adjustments are negatvie, decrement
					negAdjAmt-=PIn.Double(listPayPlanRows[i].Cells[7].Text);//+ because adjustments are negatvie, increment
				}
				if(!checkExcludePast.Checked || DateTime.Parse(listPayPlanRows[i].Cells[0].Text)>DateTime.Today) {
					gridCharges.Rows[gridCharges.Rows.Count-1].Cells[8].Text=balanceAmt.ToString("f");
				}
				if(DateTime.Parse(listPayPlanRows[i].Cells[0].Text)<=DateTime.Today) {//Totals row
					textPrincipal.Text=TotPrinc.ToString("f");
					textInterest.Text=TotInt.ToString("f");
					textDue.Text=(TotPrinc+TotInt).ToString("f");
					textPayment.Text=TotPay.ToString("f");
					textAdjustment.Text=negAdjAmt.ToString("f");
					textBalance.Text=balanceAmt.ToString("f");
					if(gridCharges.Rows.Count>0) {
						totalsRowIndex=gridCharges.Rows.Count-1;
					}
				}
			}
			TotPrinc=0;
			TotInt=0;
			int countDebits=0;
			for(int i=0;i<_listPayPlanCharges.Count;i++) {
				if(_listPayPlanCharges[i].ChargeType==PayPlanChargeType.Credit) {
					continue;//don't include credits when calculating the total loan cost, but do include adjustments
				}
				countDebits++;
				if(_listPayPlanCharges[i].ChargeType==PayPlanChargeType.Debit && _listPayPlanCharges[i].Principal >= 0) {//Not an adjustment
					TotPrinc+=_listPayPlanCharges[i].Principal;
					TotInt+=_listPayPlanCharges[i].Interest;
				}
			}
			TotPrincIntAdj=TotPrinc+TotInt+_totalNegAdjAmt;
			if(countDebits==0) {
				//don't damage what's already present in textAmount.Text
			}
			else {
				textAmount.Text=(TotPrinc+_totalNegAdjAmt).ToString("f");
			}
			textTotalCost.Text=TotPrincIntAdj.ToString("f");
			List<PayPlanCharge> listDebits = _listPayPlanCharges.FindAll(x => x.ChargeType == PayPlanChargeType.Debit).OrderBy(x => x.ChargeDate).ToList();
			if(listDebits.Count>0) {
				textDateFirstPay.Text=listDebits[0].ChargeDate.ToShortDateString();
			}
			else {
				//don't damage what's already in textDateFirstPay.Text
			}
			gridCharges.EndUpdate();
			if(gridCharges.Rows.Count>0 && totalsRowIndex != -1) {
				gridCharges.Rows[totalsRowIndex].ColorLborder=Color.Black;
				gridCharges.Rows[totalsRowIndex].Cells[6].Bold=YN.Yes;
			}
			textAccumulatedDue.Text=PayPlans.GetAccumDue(_payPlanCur.PayPlanNum,_listPayPlanCharges).ToString("f");
			textPrincPaid.Text=PayPlans.GetPrincPaid(AmtPaid,_payPlanCur.PayPlanNum,_listPayPlanCharges).ToString("f");
		}

		private ODGridRow CreateRowForPayPlanCharge(PayPlanCharge payPlanCharge,int payPlanChargeOrdinal) {
			string descript="#"+payPlanChargeOrdinal;
			if(payPlanCharge.Note!="") {
				descript+=" "+payPlanCharge.Note;
				//Don't add a # if it's a recalculated charge because they aren't "true" payplan charges.
				if(payPlanCharge.Note.Trim().ToLower().Contains("recalculated based on")) {
					descript=payPlanCharge.Note;
				}
			}
			ODGridRow row=new ODGridRow();//Charge row
			row.Cells.Add(payPlanCharge.ChargeDate.ToShortDateString());//0 Date
			row.Cells.Add(Providers.GetAbbr(payPlanCharge.ProvNum));//1 Prov Abbr
			if(payPlanCharge.Principal<0) {//adjustment
				row.Cells.Add(descript);//descript -- this has to show since we don't have a type of 'adjustment'. If not, the next debit charge is one off.
				row.Cells.Add("");//principal
				row.Cells.Add("");//interest
				row.Cells.Add("");//due
				row.Cells.Add("");//payment
				row.ColorText=_arrayAccountColors[1].ItemColor;//1 => Adjustment Account Color def.
				row.Cells.Add((payPlanCharge.Principal).ToString("n")); //adjustment
			}
			else {//regular charge
				row.Cells.Add(descript);//2 Descript
				row.Cells.Add((payPlanCharge.Principal).ToString("n"));//3 Principal
				row.Cells.Add(payPlanCharge.Interest.ToString("n"));//4 Interest
				row.Cells.Add((payPlanCharge.Principal+payPlanCharge.Interest).ToString("n"));//5 Due
				row.Cells.Add("");//6 Payment
				row.Cells.Add("");//7 Adjustment
			}
			row.Cells.Add("");//8 Balance (filled later)
			row.Tag=payPlanCharge;
			return row;
		}

		///<summary>Creates charge rows for display on the form from the data table input. Similar to CreateRowForPayPlanCharge but for use with sheets/datatables.</summary>
		public static DataRow CreateRowForPayPlanChargeDT(DataTable table,PayPlanCharge payPlanCharge,int payPlanChargeOrdinal) {
			DataRow retVal=table.NewRow();
			string descript="#"+(payPlanChargeOrdinal + 1);
			if(payPlanCharge.Note!="") {
				descript+=" "+payPlanCharge.Note;
				//Don't add a # if it's a recalculated charge because they aren't "true" payplan charges.
				if(payPlanCharge.Note.Trim().ToLower().Contains("recalculated based on")) {
					descript=payPlanCharge.Note;
				}
			}
			retVal["ChargeDate"]=payPlanCharge.ChargeDate.ToShortDateString();//0 Date
			retVal["Provider"]=Providers.GetAbbr(payPlanCharge.ProvNum);//1 Prov Abbr
			retVal["Description"]=descript;//2 Descript
			if(payPlanCharge.Principal<0) { //it's an adjustment
				retVal["Principal"]="";
				retVal["Interest"]="";//4 Interest
				retVal["Due"]="";//5 Due
				retVal["Payment"]="";//6 Payment (filled later)
				retVal["Adjustment"]=payPlanCharge.Principal.ToString("f");//7
			}
			else {//regular pay plan charge
				retVal["Principal"]=payPlanCharge.Principal.ToString("f");//3 Principal
				retVal["Interest"]=payPlanCharge.Interest.ToString("f");//4 Interest
				retVal["Due"]=(payPlanCharge.Principal+payPlanCharge.Interest).ToString("n");//5 Due
				retVal["Payment"]="";//6 Payment (filled later)
				retVal["Adjustment"]=0.ToString("f");//7
			}
			retVal["Balance"]="";//8 Balance (filled later)
			if(payPlanCharge.Principal<0) {
				retVal["Type"]="adjustment";
			}
			else {
				retVal["Type"]="charge";
			}
			return retVal;
		}

		///<summary>Creates pay plan rows for display on the form from the data table input.</summary>
		public static DataRow CreateRowForPayPlanListDT(DataTable table,DataRow payPlanList,int payPlanListOrdinal) {
			DataRow retVal=table.NewRow();
			string descript;
			if(payPlanList[2].ToString().Trim().ToLower().Contains("downpayment")) {//description
				descript=payPlanList[2].ToString();
			}
			else if(payPlanList[2].ToString().Trim().ToLower().Contains("increased interest:")) {//description
				descript="#"+(payPlanListOrdinal + 1)+" Increased Interest:";
			}
			else if(PIn.Double(payPlanList[6].ToString())>0) {//payment
				descript=payPlanList[2].ToString();
			}
			else {
				descript="#"+(payPlanListOrdinal + 1);
			}
			retVal[0]=payPlanList[0].ToString();//0 Date
			retVal[1]=payPlanList[1].ToString(); //1 Prov Abbr
			retVal[2]=descript;//2 Descript
			retVal[3]=payPlanList[3].ToString();//3 Principal
			retVal[4]=payPlanList[4].ToString();//4 Interest
			retVal[5]=payPlanList[5].ToString();//5 Due
			retVal[6]=payPlanList[6].ToString();//6 Payment (filled later)
			retVal[7]=payPlanList[7].ToString();//7 adjustment
			retVal[8]=payPlanList[8].ToString();//8 Balance (filled later)
			return retVal;
		}

		private ODGridRow CreateRowForPaySplit(DataRow rowBundlePayment,PaySplit paySplit) {
			string descript=Defs.GetName(DefCat.PaymentTypes,PIn.Long(rowBundlePayment["PayType"].ToString()));
			if(rowBundlePayment["CheckNum"].ToString()!="") {
				descript+=" #"+rowBundlePayment["CheckNum"].ToString();
			}
			descript+=" "+paySplit.SplitAmt.ToString("c");//Not sure if we really want to convert from string to double then back to string.. maybe a better way to format this?
			if(PIn.Double(rowBundlePayment["PayAmt"].ToString())!=paySplit.SplitAmt) {
				descript+=Lans.g(this,"(split)");
			}
			ODGridRow row=new ODGridRow();
			row.Cells.Add(paySplit.DatePay.ToShortDateString());//0 Date
			row.Cells.Add(Providers.GetAbbr(PIn.Long(rowBundlePayment["ProvNum"].ToString())));//1 Prov Abbr
			row.Cells.Add(descript);//2 Descript
			row.Cells.Add("");//3 Principal
			row.Cells.Add("");//4 Interest
			row.Cells.Add("");//5 Due
			row.Cells.Add(paySplit.SplitAmt.ToString("n"));//6 Payment
			row.Cells.Add("");//7 Adjustment
			row.Cells.Add("");//8 Balance (filled later)
			row.Tag=paySplit;
			row.ColorText=_arrayAccountColors[3].ItemColor;//Setup | Definitions | Account Colors | Payment;
			return row;
		}

		///<summary>Creates pay plan split rows for display on the form from the data table input. Similar to CreateRowForPayPlanSplit but for use with sheets/datatables.</summary>
		public static DataRow CreateRowForPayPlanSplitDT(DataTable table,PaySplit payPlanSplit,DataRow rowBundlePayment, int payPlanChargeOrdinal) {
			DataRow retVal=table.NewRow();
			string descript=Defs.GetName(DefCat.PaymentTypes,PIn.Long(rowBundlePayment["PayType"].ToString()));
			if(rowBundlePayment["CheckNum"].ToString()!="") {
				descript+=" #"+rowBundlePayment["CheckNum"].ToString();
			}
			descript+=" "+payPlanSplit.SplitAmt.ToString("c");
			if(PIn.Double(rowBundlePayment["PayAmt"].ToString())!=payPlanSplit.SplitAmt) {
				descript+=Lans.g("FormPayPlan","(split)");
			}
			retVal["ChargeDate"]=payPlanSplit.DatePay.ToShortDateString();//0 Date
			retVal["Provider"]=Providers.GetAbbr(PIn.Long(rowBundlePayment["ProvNum"].ToString()));//1 Prov Abbr
			retVal["Description"]=descript;//2 Descript
			retVal["Principal"]=0.ToString("f");//3 Principal
			retVal["Interest"]=0.ToString("f");//4 Interest 
			retVal["Due"]=0.ToString("f");//5 Due (filled later)
			retVal["Payment"]=payPlanSplit.SplitAmt.ToString("f");// Payment
			retVal["Adjustment"]=0.ToString("f");//7 Adjustment
			retVal["Balance"]=("");//8 Balance (filled later)
			retVal["Type"]="pay";
			return retVal;
		}

		private ODGridRow CreateRowForClaimProcs(DataRow rowBundleClaimProc) {//Either a claimpayment or a bundle of claimprocs with no claimpayment that were on the same date.
			string descript=Defs.GetName(DefCat.InsurancePaymentType,PIn.Long(rowBundleClaimProc["PayType"].ToString()));
			if(rowBundleClaimProc["CheckNum"].ToString()!="") {
				descript+=" #"+rowBundleClaimProc["CheckNum"];
			}
			if(PIn.Long(rowBundleClaimProc["ClaimPaymentNum"].ToString())==0) {
				descript+="No Finalized Payment";
			}
			else {
				double checkAmt=PIn.Double(rowBundleClaimProc["CheckAmt"].ToString());
				descript+=" "+checkAmt.ToString("c");
				double insPayAmt=PIn.Double(rowBundleClaimProc["InsPayAmt"].ToString());
				if(checkAmt!=insPayAmt) {
					descript+=" "+Lans.g(this,"(split)");
				}
			}
			ODGridRow row=new ODGridRow();
			row.Cells.Add(PIn.DateT(rowBundleClaimProc["DateCP"].ToString()).ToShortDateString());//0 Date
			row.Cells.Add(Providers.GetLName(PIn.Long(rowBundleClaimProc["ProvNum"].ToString())));//1 Prov Abbr
			row.Cells.Add(descript);//2 Descript
			row.Cells.Add("");//3 Principal
			row.Cells.Add("");//4 Interest
			row.Cells.Add("");//5 Due
			row.Cells.Add(PIn.Double(rowBundleClaimProc["InsPayAmt"].ToString()).ToString("n"));//6 Payment
			row.Cells.Add("");//7 Adjustment
			row.Cells.Add("");//8 Balance (filled later)
			row.Tag=rowBundleClaimProc;
			row.ColorText=_arrayAccountColors[7].ItemColor;//Setup | Definitions | Account Colors | Insurance Payment
			return row;
		}

		///<summary>Creates pay plan split rows for display on the form from the data table input. Similar to CreateRowForClaimProcs but for use with sheets/datatables.</summary>
		public static DataRow CreateRowForClaimProcsDT(DataTable table,DataRow rowBundleClaimProc) {//Either a claimpayment or a bundle of claimprocs with no claimpayment that were on the same date.
			DataRow retVal=table.NewRow();
			string descript=Defs.GetName(DefCat.InsurancePaymentType,PIn.Long(rowBundleClaimProc["PayType"].ToString()));
			if(rowBundleClaimProc["CheckNum"].ToString()!="") {
				descript+=" #"+rowBundleClaimProc["CheckNum"];
			}
			if(PIn.Long(rowBundleClaimProc["ClaimPaymentNum"].ToString())==0) {
				descript+="No Finalized Payment";
			}
			else {
				double checkAmt=PIn.Double(rowBundleClaimProc["CheckAmt"].ToString());
				descript+=" "+checkAmt.ToString("c");
				double insPayAmt=PIn.Double(rowBundleClaimProc["InsPayAmt"].ToString());
				if(checkAmt!=insPayAmt) {
					descript+=" "+Lans.g("FormPayPlan","(split)");
				}
			}
			retVal["ChargeDate"]=PIn.DateT(rowBundleClaimProc["DateCP"].ToString()).ToShortDateString();//0 Date
			retVal["Provider"]=Providers.GetLName(PIn.Long(rowBundleClaimProc["ProvNum"].ToString()));//1 Prov Abbr
			retVal["Description"]=descript;//2 Descript
			retVal["Principal"]="";//3 Principal
			retVal["Interest"]="";//4 Interest
			retVal["Due"]="";//5 Due
			retVal["Payment"]=PIn.Double(rowBundleClaimProc["InsPayAmt"].ToString()).ToString("n");// Payment
			retVal["Adjustment"]="";//7
			retVal["Balance"]=("");//8 Balance (filled later)
			retVal["Type"]="pay";
			return retVal;
		}

		private void butGoToPat_Click(object sender,System.EventArgs e) {
			if(HasErrors()) {
				return;
			}
			SaveData();
			GotoPatNum=_payPlanCur.PatNum;
			DialogResult=DialogResult.OK;
		}

		private void butGoTo_Click(object sender,System.EventArgs e) {
			if(HasErrors()) {
				return;
			}
			SaveData();
			GotoPatNum=_payPlanCur.Guarantor;
			DialogResult=DialogResult.OK;
		}

		private void butChangeGuar_Click(object sender,System.EventArgs e) {
			if(PayPlans.GetAmtPaid(_payPlanCur)!=0) {
				MsgBox.Show(this,"Not allowed to change the guarantor because payments are attached.");
				return;
			}
			if(gridCharges.Rows.Count>0) {
				MsgBox.Show(this,"Not allowed to change the guarantor without first clearing the amortization schedule.");
				return;
			}
			FormPatientSelect FormPS=new FormPatientSelect();
			FormPS.SelectionModeOnly=true;
			FormPS.ShowDialog();
			if(FormPS.DialogResult!=DialogResult.OK) {
				return;
			}
			_payPlanCur.Guarantor=FormPS.SelectedPatNum;
			textGuarantor.Text=Patients.GetLim(_payPlanCur.Guarantor).GetNameLF();
		}
		
		private void textAmount_Validating(object sender,CancelEventArgs e) {
			if(textCompletedAmt.Text=="") {
				return;
			}
			if(PIn.Double(textCompletedAmt.Text)==PIn.Double(textAmount.Text)) {
				return;
			}
		}

		private void butChangePlan_Click(object sender,System.EventArgs e) {
			FormInsPlanSelect FormI=new FormInsPlanSelect(_payPlanCur.PatNum);
			FormI.ShowDialog();
			if(FormI.DialogResult==DialogResult.Cancel) {
				return;
			}
			_payPlanCur.PlanNum=FormI.SelectedPlan.PlanNum;
			_payPlanCur.InsSubNum=FormI.SelectedSub.InsSubNum;
			textInsPlan.Text=InsPlans.GetDescript(_payPlanCur.PlanNum,Patients.GetFamily(_payPlanCur.PatNum),InsPlanList,_payPlanCur.InsSubNum,SubList);
		}

		private void textPaymentCount_KeyPress(object sender,System.Windows.Forms.KeyPressEventArgs e) {
			textPeriodPayment.Text="";
		}

		private void textPeriodPayment_KeyPress(object sender,System.Windows.Forms.KeyPressEventArgs e) {
			textPaymentCount.Text="";
		}

		private void butMoreOptions_Click(object sender,EventArgs e) {
			FormPayPlanOpts.ShowDialog();
		}

		private void butCreateSched_Click(object sender,System.EventArgs e) {
			//this is also where the terms get saved
			if(textDate.errorProvider1.GetError(textDate)!=""
				|| textAmount.errorProvider1.GetError(textAmount)!=""
				|| textDateFirstPay.errorProvider1.GetError(textDateFirstPay)!=""
				|| textDownPayment.errorProvider1.GetError(textDownPayment)!=""
				|| textAPR.errorProvider1.GetError(textAPR)!=""
				|| textPaymentCount.errorProvider1.GetError(textPaymentCount)!=""
				|| textPeriodPayment.errorProvider1.GetError(textPeriodPayment)!=""
				|| textCompletedAmt.errorProvider1.GetError(textCompletedAmt)!=""
				) {
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return;
			}
			if(textAmount.Text=="" || PIn.Double(textAmount.Text)==0) {
				MsgBox.Show(this,"Please enter an amount first.");
				return;
			}
			if(textDateFirstPay.Text=="") {
				textDateFirstPay.Text=DateTime.Today.ToShortDateString();
			}
			if(textDownPayment.Text=="") {
				textDownPayment.Text="0";
			}
			if(textAPR.Text=="") {
				textAPR.Text="0";
			}
			if(textPaymentCount.Text=="" && textPeriodPayment.Text=="") {
				MsgBox.Show(this,"Please enter a term or payment amount first.");
				return;
			}
			if(textPaymentCount.Text=="" && PIn.Double(textPeriodPayment.Text)==0) {
				MsgBox.Show(this,"Payment cannot be 0.");
				return;
			}
			if(textPaymentCount.Text!="" && textPeriodPayment.Text!="") {
				MsgBox.Show(this,"Please choose either Number of Payments or Payment Amt.");
				return;
			}
			if(textPeriodPayment.Text=="" && PIn.Long(textPaymentCount.Text)<1) {
				MsgBox.Show(this,"Term cannot be less than 1.");
				return;
			}
			if(PIn.Double(textAmount.Text)-PIn.Double(textDownPayment.Text)<0) {
				MsgBox.Show(this,"Down payment must be less than or equal to total amount.");
				return;
			}
			//If there are any debits, this button is going to delete them and replace them all with a new amortization schedule
			if(_listPayPlanCharges.FindAll(x => x.ChargeType==PayPlanChargeType.Debit).Count>0) {
				if(!MsgBox.Show(this,true,"Replace existing amortization schedule?")) {
					return;
				}
				_listPayPlanCharges.RemoveAll(x => x.ChargeType==PayPlanChargeType.Debit); //for version 1, debits are the only chargetype available.
			}
			CreateScheduleCharges(false);
			//fill signature. most likely will invalidate the signature if new schedule was created
			signatureBoxWrapper.FillSignature(_payPlanCur.SigIsTopaz,GetKeyDataForSignature(),_payPlanCur.Signature); //fill signature		
		}

		private void butRecalculate_Click(object sender,EventArgs e) {
			if(textDate.errorProvider1.GetError(textDate)!=""
				|| textAmount.errorProvider1.GetError(textAmount)!=""
				|| textDateFirstPay.errorProvider1.GetError(textDateFirstPay)!=""
				|| textDownPayment.errorProvider1.GetError(textDownPayment)!=""
				|| textAPR.errorProvider1.GetError(textAPR)!=""
				|| textPaymentCount.errorProvider1.GetError(textPaymentCount)!=""
				|| textPeriodPayment.errorProvider1.GetError(textPeriodPayment)!=""
				|| textCompletedAmt.errorProvider1.GetError(textCompletedAmt)!="") {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(_listPayPlanCharges.Count(x => x.ChargeType==PayPlanChargeType.Debit)==0) {//This is only possible if they manually delete all of their rows and try to press recalculate.
				MsgBox.Show(this,"There is no payment plan to recalculate.");
				return;
			}
			if(IsInsPayPlan) {
				MsgBox.Show(this,"Insurance payment plans can't be recalculated.");
				return;
			}
			if(PIn.Double(textTotalCost.Text)<=PIn.Double(textAmtPaid.Text)) {
				MsgBox.Show(this,"The payment plan has been completely paid and can't be recalculated.");
				return;
			}
			_formPayPlanRecalculate.ShowDialog();
			if(_formPayPlanRecalculate.DialogResult==DialogResult.OK) {
				CreateScheduleCharges(true);
			}
			signatureBoxWrapper.FillSignature(_payPlanCur.SigIsTopaz,GetKeyDataForSignature(),_payPlanCur.Signature); //fill signature
		}

		///<summary>For example, date is the 3rd Friday of the month, then this returns 3.</summary>
		private int GetOrdinalOfMonth(DateTime date) {
			if(date.AddDays(-28).Month==date.Month) {
				return 4;//treat a 5 like a 4
			}
			else if(date.AddDays(-21).Month==date.Month) {//4
				return 4;
			}
			else if(date.AddDays(-14).Month==date.Month) {
				return 3;
			}
			if(date.AddDays(-7).Month==date.Month) {
				return 2;
			}
			return 1;
		}

		private void gridCharges_CellDoubleClick(object sender,OpenDental.UI.ODGridClickEventArgs e) { 
			if(gridCharges.Rows[e.Row].Tag==null) {//Prevent double clicking on the "Current Totals" row
				return;
			}
			if(gridCharges.Rows[e.Row].Tag.GetType()==typeof(PayPlanCharge)) {
				PayPlanCharge payPlanCharge=(PayPlanCharge)gridCharges.Rows[e.Row].Tag;
				double payPlanChargeOldAmt=payPlanCharge.Principal;
				FormPayPlanChargeEdit FormP=new FormPayPlanChargeEdit(payPlanCharge);//This automatically takes care of our in-memory list because the Tag is referencing our list of objects.
				FormP.ShowDialog();
				if(FormP.DialogResult==DialogResult.Cancel) {
					return;
				}
				if(FormP.PayPlanChargeCur==null) {//The user deleted the payplancharge.
					_listPayPlanCharges.Remove(payPlanCharge);//We know the payPlanCharge object is inside _listPayPlanCharges.
					if(payPlanCharge.Principal<0) {//adjustment
						textAmount.Text=(PIn.Double(textAmount.Text)-(payPlanChargeOldAmt)).ToString("f");//charge will be negative, - to add the amount back
					}
					gridCharges.BeginUpdate();
					gridCharges.Rows.RemoveAt(e.Row);
					gridCharges.EndUpdate();
					return;
				}
				//modifying the existing charge
				if(payPlanCharge.Principal<0) {//adjustment
					double amtChanged=0;
					if(payPlanCharge.Principal>payPlanChargeOldAmt) {
						//negative amounts. We decreased the adjustment amount. Total Amount needs to grow.
						amtChanged=(payPlanChargeOldAmt-payPlanCharge.Principal)*-1;//amt should be +
					}
					else {
						//We increased the adjustment. Total Amount needs to shrink. 
						amtChanged=payPlanCharge.Principal-payPlanChargeOldAmt;//amt should be -
					}
					textAmount.Text=(PIn.Double(textAmount.Text)+(amtChanged)).ToString("f");
				}
			}
			else if(gridCharges.Rows[e.Row].Tag.GetType()==typeof(PaySplit)) {
				PaySplit paySplit=(PaySplit)gridCharges.Rows[e.Row].Tag;
				Payment pay=Payments.GetPayment(paySplit.PayNum);
				if(pay==null) {
					MessageBox.Show(Lans.g(this,"No payment exists.  Please run database maintenance method")+" "+nameof(DatabaseMaintenances.PaySplitWithInvalidPayNum));
					return;
				}
				FormPayment FormPayment2=new FormPayment(PatCur,FamCur,pay,false);//FormPayment may inserts and/or update the paysplits. 
				FormPayment2.IsNew=false;
				FormPayment2.ShowDialog();
				if(FormPayment2.DialogResult==DialogResult.Cancel) {
					return;
				}
			}
			else if(gridCharges.Rows[e.Row].Tag.GetType()==typeof(DataRow)) {//Claim payment or bundle.
				DataRow bundledClaimProc=(DataRow)gridCharges.Rows[e.Row].Tag;
				Claim claimCur=Claims.GetClaim(PIn.Long(bundledClaimProc["ClaimNum"].ToString()));
				if(claimCur==null) {
					MsgBox.Show(this,"The claim has been deleted.");
				}
				else {
					if(!Security.IsAuthorized(Permissions.ClaimView)) {
						return;
					}
					FormClaimEdit FormCE=new FormClaimEdit(claimCur,PatCur,FamCur);//FormClaimEdit inserts and/or updates the claim and/or claimprocs, which could potentially change the bundle.
					FormCE.IsNew=false;
					FormCE.ShowDialog();
					//Cancel from FormClaimEdit does not cancel payment edits, fill grid every time
				}
			}
			FillCharges();
		}

		///<summary>Adds a debit.</summary>
		private void butAdd_Click(object sender,System.EventArgs e) {
			PayPlanCharge ppCharge=CreateDebitCharge(0,0,DateTime.Today,"");
			FormPayPlanChargeEdit FormP=new FormPayPlanChargeEdit(ppCharge);
			FormP.IsNew=true;
			FormP.ShowDialog();
			if(FormP.DialogResult==DialogResult.Cancel) {
				return;
			}
			_listPayPlanCharges.Add(ppCharge);
			FillCharges();
			//fills signature. Most likely will invalidate the signature due to changes to PP
			signatureBoxWrapper.FillSignature(_payPlanCur.SigIsTopaz,GetKeyDataForSignature(),_payPlanCur.Signature); //fill signature
		}

		private void butClear_Click(object sender,System.EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Clear all charges and adjustments from amortization schedule?  Credits will not be cleared.")) {
				return;
			}
			textAmount.Text=TotPrinc.ToString("f");//give the total amount back it's original value w/o adjustments.
			_listPayPlanCharges.RemoveAll(x => x.ChargeType==PayPlanChargeType.Debit); 
			textDateFirstPay.ReadOnly=false;
			textDownPayment.ReadOnly=false;
			gridCharges.BeginUpdate();
			gridCharges.Rows.Clear();
			gridCharges.EndUpdate();
		}

		private void butSignPrint_Click(object sender,EventArgs e) {
			if(HasErrors()) {
				return;
			}
			SaveData();
			Sheet sheetPP=null;
			sheetPP=PayPlanToSheet(_payPlanCur);
			_payPlanCur.IsNew=IsNew;
			string keyData=GetKeyDataForSignature();
			SheetParameter.SetParameter(sheetPP,"keyData",keyData);
			SheetUtil.CalculateHeights(sheetPP);
			FormSheetFillEdit FormSF=new FormSheetFillEdit(sheetPP);
			FormSF.ShowDialog();
			if(FormSF.DialogResult==DialogResult.OK) {//save signature
				if(_payPlanCur.Signature=="") {//clear signature and hide sigbox if blank sig was saved
					signatureBoxWrapper.ClearSignature();
					butSignPrint.Text="Sign && Print";
					signatureBoxWrapper.Visible=false;
					groupBox4.Visible=false;
				}
				else {
					signatureBoxWrapper.Visible=true;//show after PP has been signed for the first time
					groupBox4.Visible=true;
					butSignPrint.Text="View && Print";
					signatureBoxWrapper.FillSignature(_payPlanCur.SigIsTopaz,keyData,_payPlanCur.Signature); //fill signature on form
				}
			}
		}


		private void butPrint_Click(object sender,System.EventArgs e) {
			if(HasErrors()) {
				return;
			}
			SaveData();
			if(PrefC.GetBool(PrefName.PayPlansUseSheets)) {
				Sheet sheetPP=null;
				sheetPP=PayPlanToSheet(_payPlanCur);
				SheetPrinting.Print(sheetPP);
			}
			else {
				Font font=new Font("Tahoma",9);
				Font fontBold=new Font("Tahoma",9,FontStyle.Bold);
				Font fontTitle=new Font("Tahoma",17,FontStyle.Bold);
				Font fontSubTitle=new Font("Tahoma",10,FontStyle.Bold);
				ReportComplex report=new ReportComplex(false,false);
				report.AddTitle("Title",Lan.g(this,"Payment Plan Terms"),fontTitle);
				report.AddSubTitle("PracTitle",PrefC.GetString(PrefName.PracticeTitle),fontSubTitle);
				report.AddSubTitle("Date SubTitle",DateTime.Today.ToShortDateString(),fontSubTitle);
				AreaSectionType sectType=AreaSectionType.ReportHeader;
				Section section=report.Sections[AreaSectionType.ReportHeader];
				//int sectIndex=report.Sections.GetIndexOfKind(AreaSectionKind.ReportHeader);
				Size size=new Size(300,20);//big enough for any text
				ContentAlignment alignL=ContentAlignment.MiddleLeft;
				ContentAlignment alignR=ContentAlignment.MiddleRight;
				int yPos=140;
				int space=30;
				int x1=175;
				int x2=275;
				report.ReportObjects.Add(new ReportObject
					("Patient Title",sectType,new Point(x1,yPos),size,"Patient",font,alignL));
				report.ReportObjects.Add(new ReportObject
					("Patient Detail",sectType,new Point(x2,yPos),size,textPatient.Text,font,alignR));
				yPos+=space;
				report.ReportObjects.Add(new ReportObject
					("Guarantor Title",sectType,new Point(x1,yPos),size,"Guarantor",font,alignL));
				report.ReportObjects.Add(new ReportObject
					("Guarantor Detail",sectType,new Point(x2,yPos),size,textGuarantor.Text,font,alignR));
				yPos+=space;
				report.ReportObjects.Add(new ReportObject
					("Date of Agreement Title",sectType,new Point(x1,yPos),size,"Date of Agreement",font,alignL));
				report.ReportObjects.Add(new ReportObject
					("Date of Agreement Detail",sectType,new Point(x2,yPos),size,_payPlanCur.PayPlanDate.ToString("d"),font,alignR));
				yPos+=space;
				report.ReportObjects.Add(new ReportObject
					("Principal Title",sectType,new Point(x1,yPos),size,"Principal",font,alignL));
				report.ReportObjects.Add(new ReportObject
					("Principal Detail",sectType,new Point(x2,yPos),size,TotPrinc.ToString("n"),font,alignR));
				yPos+=space;
				report.ReportObjects.Add(new ReportObject
					("Annual Percentage Rate Title",sectType,new Point(x1,yPos),size,"Annual Percentage Rate",font,alignL));
				report.ReportObjects.Add(new ReportObject
					("Annual Percentage Rate Detail",sectType,new Point(x2,yPos),size,_payPlanCur.APR.ToString("f1"),font,alignR));
				yPos+=space;
				report.ReportObjects.Add(new ReportObject
					("Total Finance Charges Title",sectType,new Point(x1,yPos),size,"Total Finance Charges",font,alignL));
				report.ReportObjects.Add(new ReportObject
					("Total Finance Charges Detail",sectType,new Point(x2,yPos),size,TotInt.ToString("n"),font,alignR));
				yPos+=space;
				report.ReportObjects.Add(new ReportObject
					("Total Cost of Loan Title",sectType,new Point(x1,yPos),size,"Total Adjustments",font,alignL));
				report.ReportObjects.Add(new ReportObject
					("Total Cost of Loan Detail",sectType,new Point(x2,yPos),size,_totalNegAdjAmt.ToString("n"),font,alignR));
				yPos+=space;
				report.ReportObjects.Add(new ReportObject
					("Total Cost of Loan Title",sectType,new Point(x1,yPos),size,"Total Cost of Loan",font,alignL));
				report.ReportObjects.Add(new ReportObject
					("Total Cost of Loan Detail",sectType,new Point(x2,yPos),size,(TotPrincIntAdj).ToString("n"),font,alignR));
				yPos+=space;
				section.Height=yPos+30;
				DataTable tbl=new DataTable();
				tbl.Columns.Add("date");
				tbl.Columns.Add("prov");
				tbl.Columns.Add("description");
				tbl.Columns.Add("principal");
				tbl.Columns.Add("interest");
				tbl.Columns.Add("due");
				tbl.Columns.Add("payment");
				tbl.Columns.Add("adjustment");
				tbl.Columns.Add("balance");
				DataRow row;
				for(int i = 0;i<gridCharges.Rows.Count;i++) {
					row=tbl.NewRow();
					row["date"]=gridCharges.Rows[i].Cells[0].Text;
					row["prov"]=gridCharges.Rows[i].Cells[1].Text;
					row["description"]=gridCharges.Rows[i].Cells[2].Text;
					row["principal"]=gridCharges.Rows[i].Cells[3].Text;
					row["interest"]=gridCharges.Rows[i].Cells[4].Text;
					row["due"]=gridCharges.Rows[i].Cells[5].Text;
					row["payment"]=gridCharges.Rows[i].Cells[6].Text;
					row["adjustment"]=gridCharges.Rows[i].Cells[7].Text;
					row["balance"]=gridCharges.Rows[i].Cells[8].Text;
					tbl.Rows.Add(row);
				}
				QueryObject query=report.AddQuery(tbl,"","",SplitByKind.None,1,true);
				query.AddColumn("ChargeDate",80,FieldValueType.Date,font);
				query.GetColumnHeader("ChargeDate").StaticText="Date";
				query.AddColumn("Provider",75,FieldValueType.String,font);
				query.AddColumn("Description",130,FieldValueType.String,font);
				query.AddColumn("Principal",60,FieldValueType.Number,font);
				query.AddColumn("Interest",52,FieldValueType.Number,font);
				query.AddColumn("Due",60,FieldValueType.Number,font);
				query.AddColumn("Payment",60,FieldValueType.Number,font);
				query.AddColumn("Adjustment",75,FieldValueType.Number,font);
				query.AddColumn("Balance",60,FieldValueType.String,font);
				query.GetColumnHeader("Balance").ContentAlignment=ContentAlignment.MiddleRight;
				query.GetColumnDetail("Balance").ContentAlignment=ContentAlignment.MiddleRight;
				report.ReportObjects.Add(new ReportObject("Note",AreaSectionType.ReportFooter,new Point(x1,20),new Size(500,200),textNote.Text,font,ContentAlignment.TopLeft));
				report.ReportObjects.Add(new ReportObject("Signature",AreaSectionType.ReportFooter,new Point(x1,220),new Size(500,20),"Signature of Guarantor: ____________________________________________",font,alignL));
				if(!report.SubmitQueries()) {
					return;
				}
				FormReportComplex FormR=new FormReportComplex(report);
				FormR.ShowDialog();
			}
		}

		private void pd2_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			int xPos=15;//starting pos
			int yPos=(int)27.5;//starting pos
			e.Graphics.DrawString("Payment Plan Truth in Lending Statement"
				,new Font("Arial",8),Brushes.Black,(float)xPos,(float)yPos);
			//e.Graphics.DrawImage(imageTemp,xPos,yPos);
		}

		private void butPayPlanTx_Click(object sender,EventArgs e) {
			FormPayPlanCredits FormPPC=new FormPayPlanCredits(_payPlanCur);
			FormPPC.ListPayPlanCreditsCur=_listPayPlanCharges.Where(x => x.ChargeType==PayPlanChargeType.Credit).Select(x => x.Copy()).ToList();
			FormPPC.ShowDialog();
			if(FormPPC.DialogResult!=DialogResult.OK) {
				return;
			}
			_listPayPlanCharges.RemoveAll(x => x.ChargeType==PayPlanChargeType.Credit);
			_listPayPlanCharges.AddRange(FormPPC.ListPayPlanCreditsCur);
			double txCompleteAmt=0;
			foreach(PayPlanCharge credit in FormPPC.ListPayPlanCreditsCur) {
				if(credit.ChargeDate.Date!=DateTime.MaxValue.Date) { //do not take into account maxvalue (tp'd) charges
					txCompleteAmt+=credit.Principal;
				}
			}
			textCompletedAmt.Text=txCompleteAmt.ToString("f");
			double txTotalAmt=PayPlans.GetTxTotalAmt(_listPayPlanCharges);
			textTotalTxAmt.Text=txTotalAmt.ToString("f");
      double amt = PIn.Double(textTotalTxAmt.Text);
      double compl = PIn.Double(textAmount.Text);
      //only attempt to change the total amt of the payment plan if an amortization schedule doesn't already exist.
      if(_listPayPlanCharges.Count(x => x.ChargeType==PayPlanChargeType.Debit)==0//amortization schedule does not exist
				&& PIn.Double(textTotalTxAmt.Text)!=PIn.Double(textAmount.Text)//Total treatment amount does not match term amount.
				&& MsgBox.Show(this,MsgBoxButtons.YesNo,"Change term Total Amount to match Total Tx Amount?")) {
				textAmount.Text=txTotalAmt.ToString("f");
			}
			FillCharges();
		}

		private void butCloseOut_Click(object sender,EventArgs e) {
			if(HasErrors()) {
				return;
			}
			if(!IsInsPayPlan) {//Patient Payment Plan
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Closing out this payment plan will remove interest and adjustments from all future charges "
					+"and make them due immediately.  Do you want to continue?")) {
					return;
				}
				if(_totalNegFutureAdjs!=0 && PIn.Double(textTotalTxAmt.Text)>0) {//have adjustments and tx credits
					//make another credit to offset the amount of adjustments that didn't come due (take them off the account, don't count towards close out charge)
					PayPlanCharge adjustmentOffset=new PayPlanCharge() {
						PayPlanNum=_payPlanCur.PayPlanNum,
						Guarantor=_payPlanCur.PatNum,
						PatNum=_payPlanCur.PatNum,
						ChargeDate=DateTimeOD.Today,
						Interest=0,
						Principal=_totalNegAdjAmt*-1,//enter as a postive to offset the negative adjustment
						Note=Lan.g(this,"Adjustments taken off for close out charge"),
						ChargeType=PayPlanChargeType.Credit
					};
					_listPayPlanCharges.Add(adjustmentOffset);//if they made an adjustment to the account, they will need to delete it. Not sure if we should remind them, or try to do it programatically or neither. 
				}
				double sumPastDebits = _listPayPlanCharges
				.Where(x => x.ChargeType==PayPlanChargeType.Debit)
				.Where(x => x.ChargeDate <= DateTimeOD.Today.Date)
				.Sum(x => x.Principal);
				double sumCredits = _listPayPlanCharges
				.Where(x => x.ChargeType==PayPlanChargeType.Credit)
				.Where(x => x.ChargeDate != DateTime.MaxValue.Date) //only count non-TP credits
				.Sum(x => x.Principal);
				PayPlanCharge closeoutCharge=new PayPlanCharge() {
					PayPlanNum=_payPlanCur.PayPlanNum,
					Guarantor=_payPlanCur.PatNum, //the closeout charge should always appear on the patient of the payment plan.
					PatNum=_payPlanCur.PatNum,
					ChargeDate=DateTimeOD.Today,
					Interest=0,
					Principal=sumCredits-sumPastDebits,
					Note=Lan.g(this,"Close Out Charge"),
					//ProvNum=PatCur.PriProv,//will be changed in SaveData()
					//ClinicNum=PatCur.ClinicNum,//will be changed in SaveData()
					ChargeType=PayPlanChargeType.Debit,
				};
				_listPayPlanCharges.RemoveAll(x => x.ChargeDate > DateTimeOD.Today.Date); //also removes TP Procs
				_listPayPlanCharges.Add(closeoutCharge);
			}
			else {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Closing out an insurance payment plan will change the Tx Completed Amt to match the amount"
					+" insurance actually paid.  Do you want to continue?")) {
					return;
				}
				double insPaidTotal=0;
				for(int i=0;i < _bundledClaimProcs.Rows.Count;i++) {
					insPaidTotal+=PIn.Double(_bundledClaimProcs.Rows[i]["InsPayAmt"].ToString());
				}
				textCompletedAmt.Text=insPaidTotal.ToString("f");
			}
			butClosePlan.Enabled=false;
			_payPlanCur.IsClosed=true;
			FillCharges();
			SaveData();
			DialogResult=DialogResult.OK;
		}

		private bool HasErrors() {
			if(textDate.errorProvider1.GetError(textDate)!=""
			|| textCompletedAmt.errorProvider1.GetError(textCompletedAmt)!="") {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return true;
			}
			if(gridCharges.Rows.Count==0) {
				MsgBox.Show(this,"An amortization schedule must be created first.");
				return true;
			}
			if(_selectedProvNum==0) {
				MsgBox.Show(this,"A provider must be selected first.");
				return true;
			}
			if(_payPlanCur.PayPlanDate > DateTime.Today.Date && !PrefC.GetBool(PrefName.FutureTransDatesAllowed)) {
				MsgBox.Show(this,"Payment plan date cannot be set for the future.");
				return true;
			}
			return false;
		}

		///<summary>Sorts by the first column, as a date column, in ascending order.</summary>
		private int ComparePayPlanRows(ODGridRow x,ODGridRow y) {
			DateTime dateTimeX=DateTime.Parse(x.Cells[0].Text);
			DateTime dateTimeY=DateTime.Parse(y.Cells[0].Text);
			if(dateTimeX<dateTimeY) {
				return -1;
			}
			else if(dateTimeX>dateTimeY) {
				return 1;
			}
			//dateTimeX==dateTimeY
			//We want to put recalculated charges to the bottom of the current date.  This is a "final" point when recalculating and needs to be at the end.
			if(x.Cells[2].Text.Trim().ToLower().Contains("recalculated based on") && !y.Cells[2].Text.Trim().ToLower().Contains("recalculated based on")) {
				return 1;
			}
			if(!x.Cells[2].Text.Trim().ToLower().Contains("recalculated based on") && y.Cells[2].Text.Trim().ToLower().Contains("recalculated based on")) {
				return -1;
			}
			//If there is more than one recalculate charge, sort by descending charge amount. This only matters if one of the recalculated charges is 0
			if(x.Cells[2].Text.Trim().ToLower().Contains("recalculated based on") && y.Cells[2].Text.Trim().ToLower().Contains("recalculated based on")) {
				if(PIn.Double(x.Cells[3].Text)<PIn.Double(y.Cells[3].Text)) {
					return 1;
				}
				return -1;
			}
			//Show charges before Payment on the same date.
			if(x.Tag.GetType()==typeof(PayPlanCharge)) {//x is charge (Type.Equals doesn't seem to work in sorters for some reason)
				if(y.Tag.GetType()==typeof(PaySplit) || y.Tag.GetType()==typeof(DataRow)) {//y is credit, x goes first
					return -1;
				}
				else {//x and y are both charges (Not likely, they shouldn't have same dates) unless they are adjustments
					if(string.IsNullOrEmpty(x.Cells[7].Text) && !string.IsNullOrEmpty(y.Cells[7].Text)) {
						return -1;
					}
					else if(!string.IsNullOrEmpty(x.Cells[7].Text) && string.IsNullOrEmpty(y.Cells[7].Text)) {
						return 1;
					}
				}
			}
			else {//x is credit
				if(y.Tag.GetType()==typeof(PayPlanCharge)) {//y is charge
					return 1;
				}
				//x and y are both Payments
			}
			return x.Cells[2].Text.CompareTo(y.Cells[2].Text);//Sort by description.  This orders the payment plan charges which are on the same date by their charge number.  Might order payments by check number as well.
		}

		///<summary>Performs same function as ComparePayPlanRows but for use with DataTables/Sheets.</summary>
		public static int ComparePayPlanRowsDT(DataRow x,DataRow y) {
			DateTime dateTimeX=PIn.Date(x["ChargeDate"].ToString());
			DateTime dateTimeY=PIn.Date(y["ChargeDate"].ToString()); 
			if(dateTimeX.Date!=dateTimeY.Date) {
				return dateTimeX.CompareTo(dateTimeY);// sort by date
			}
			bool xIsRecalc=x["Description"].ToString().ToLower().Contains("recalculated based on");
			bool yIsRecalc=y["Description"].ToString().ToLower().Contains("recalculated based on");
			if(xIsRecalc!=yIsRecalc) {
				return xIsRecalc.CompareTo(yIsRecalc);// recalculated charges to the bottom of the current date.
			}
			if(xIsRecalc && yIsRecalc) { 
				return PIn.Double(x["Principal"].ToString()).CompareTo(PIn.Double(y["Principal"].ToString()));// sort by principal amounts if both are recalculated charges
			}
			if(x["Type"].ToString()!=y["Type"].ToString()) {
				return x["Type"].ToString().CompareTo(y["Type"].ToString());//charges first; I.e. "charge".CompareTo("pay") will return charge first
			}
			return x["Description"].ToString().CompareTo(y["Description"].ToString());//Sort by description. 
		}

		///<summary>Creates/recalculates the amortization schedule.  Uses the textfield term values and not the ones stored in the database.</summary>
		private void CreateScheduleCharges(bool isRecalculate) {
			PayPlanCharge ppCharge=new PayPlanCharge();
			//down payment
			double downpayment=PIn.Double(textDownPayment.Text);
			if(downpayment!=0 && !isRecalculate) {
				_listPayPlanCharges.Add(CreateDebitCharge(downpayment,0,DateTimeOD.Today,Lan.g(this,"Downpayment")));
			}
			double principalAmt=PIn.Double(textAmount.Text);
			//Skip downpayment subtraction if recalculating.  The downpayment will get subtracted as a payplan charge later.
			if(!isRecalculate) {
				principalAmt-=PIn.Double(textDownPayment.Text);//principal is always >= 0 due to validation.  
				_payPlanCur.DownPayment=downpayment;
			}
			double periodRate=CalcPeriodRate();
			int countPayPlanCharges=0;
			double interestFutureUnpaidAmt=0;//Only used if recalculating.
			DateTime dateFirst=PIn.Date(textDateFirstPay.Text);
			double pastDueTotal=0;
			const int payPlanChargesCeiling=2000;//This is the maximum number of payplan charges allowed to be made plus 1.
			double interestInterimAmt=0;//This will be used to replace the next payplancharge with the correctly calculated interest amount.
			double overPaidAmt=0;//Only used if recalculating.
			int nextChargeIdx=-1;
			if(isRecalculate) {
				List<PayPlanCharge> listPayPlanChargesCopy=new List<PayPlanCharge>(_listPayPlanCharges);
				_listPayPlanCharges.Clear();
				for(int i=0;i<listPayPlanChargesCopy.Count;i++) {
					PayPlanCharge chargeCur=listPayPlanChargesCopy[i];
					if(chargeCur.ChargeDate<=DateTime.Today) {//Historical pay plan charge.
						_listPayPlanCharges.Add(chargeCur);
						if(chargeCur.ChargeType!=PayPlanChargeType.Debit) {
							continue;
						}
						pastDueTotal+=chargeCur.Principal+chargeCur.Interest;
						principalAmt-=chargeCur.Principal;
						//Don't count charges that we made in addition to the original terms
						if(!chargeCur.Note.Trim().ToLower().Contains("recalculated based on") && !chargeCur.Note.Trim().ToLower().Contains("downpayment")) {
							countPayPlanCharges++;
						}
					}
					else {//Future pay plan charge.
						if(chargeCur.ChargeType!=PayPlanChargeType.Debit) {
							_listPayPlanCharges.Add(chargeCur);
							continue;
						}
						interestFutureUnpaidAmt+=chargeCur.Interest;//Only used if not recalculating interest
					}
				}
				listPayPlanChargesCopy.RemoveAll(x => x.ChargeType != PayPlanChargeType.Debit);
				//Find the first pay plan charge in the future which has not happened yet, or if all else fails get the date on the last charge.
				for(int i=0;i<listPayPlanChargesCopy.Count;i++) {
					nextChargeIdx=i;
					if(listPayPlanChargesCopy[i].ChargeDate>DateTime.Today) {
						break;//The first future charge has been located.
					}
				}
				dateFirst=listPayPlanChargesCopy[nextChargeIdx].ChargeDate;
				while(dateFirst<=DateTime.Today) {
					dateFirst=CalcNextPeriodDate(dateFirst,1);//1 will get the next period date
				}
				double paidTotal=0;
				for(int i=0;i<_listPaySplits.Count;i++) {
					if(_listPaySplits[i].DatePay>DateTime.Today) {
						break;
					}
					paidTotal+=_listPaySplits[i].SplitAmt;
				}
				if(paidTotal>=pastDueTotal) {//Overpaid
					overPaidAmt=paidTotal-pastDueTotal;
					if(_formPayPlanRecalculate.IsPrepay) {
						_listPayPlanCharges.Add(CreateDebitCharge(overPaidAmt,0,DateTimeOD.Today,Lan.g(this,"Recalculated based on prepayment")));
					}
					else {
						//Only deduct the overpaid amount from principal if we aren't prepaying, otherwise the payamount per month will be different than expected.
						principalAmt-=overPaidAmt;
						_listPayPlanCharges.Add(CreateDebitCharge(overPaidAmt,0,DateTimeOD.Today,Lan.g(this,"Recalculated based on pay on principal")));
						overPaidAmt=0;
					}
				}
				if(pastDueTotal>paidTotal) {//The patient currently owes more than they have paid.  There is an amount past due.
					interestInterimAmt=(principalAmt+pastDueTotal-paidTotal)*periodRate;
					if(nextChargeIdx==listPayPlanChargesCopy.Count-1) {//The original payment plan schedule has finished, but patinet still owes monie$.
						_listPayPlanCharges.Add(CreateDebitCharge(0,interestInterimAmt,dateFirst,Lan.g(this,"Increased interest")+": "+interestInterimAmt.ToString("c")));
						FillCharges();
						SetNote();
						return;
					}
				}
			}
			int paymentCount=PIn.Int(textPaymentCount.Text)-countPayPlanCharges;
			decimal periodPaymentAmt=CalcPeriodPayment(principalAmt,periodRate,interestFutureUnpaidAmt,countPayPlanCharges,isRecalculate);
			decimal overPaidDecrementingAmt=(decimal)overPaidAmt;
			decimal principalDecrementingAmt=(decimal)principalAmt;//The principal which will be decreased to zero.  Always starts >= 0, due to validation.
			decimal interestUnpaidDecrementingAmt=(decimal)interestFutureUnpaidAmt;//Only used if recalculating and _formPayPlanRecalculate.isRecalculateInterest=false
			int chargesCount=0;//Not the same as _listPayPlanCharges.Count
			int skippedChargesCount=0;
			while(principalDecrementingAmt>0 && chargesCount<payPlanChargesCeiling) {//the ceiling prevents infinite loop
				if(decimal.Compare(principalDecrementingAmt,(decimal)principalAmt)>0  
					&& ppCharge.Interest>0) //Check interest to make sure 2nd time through loop
				{
					//The principal is actually increasing or staying the same with each payment.
					MessageBox.Show(Lan.g(this,"This payment plan will never be paid off. The interest being charged on each payment is")+" "
						+ppCharge.Interest.ToString("f")+" "+Lan.g(this,"and the payment amount is")+" "+periodPaymentAmt.ToString("f")+". "
						+Lan.g(this,"Choose a lower interest rate or a higher payment amount."));
					_listPayPlanCharges.Clear();//We don't want to leave charges in this list since we're stopping our calculations.
					break;
				}
				ppCharge=CreateDebitCharge(0,0,CalcNextPeriodDate(dateFirst,chargesCount),"");
				if(isRecalculate && !_formPayPlanRecalculate.IsRecalculateInterest) {
					//Spread the unpaid interest out over the term
					if(paymentCount>0) {//Specified number of payments when creating the plan
						ppCharge.Interest=Math.Round(interestFutureUnpaidAmt/paymentCount,_roundDec);
					}
					else {
						//This will take the total interest unpaid, and divide it by the calculated term, which is total amount/amount per month
						ppCharge.Interest=Math.Round(interestFutureUnpaidAmt/((principalAmt+interestFutureUnpaidAmt)/(double)periodPaymentAmt),_roundDec);
					}
				}
				else {//Either not recalculating or is recalculating but also recalculating interest
					ppCharge.Interest=Math.Round(((double)principalDecrementingAmt*periodRate),_roundDec);//2 decimals
				}
				if(principalDecrementingAmt<periodPaymentAmt-(decimal)ppCharge.Interest) {
					ppCharge.Principal=(double)principalDecrementingAmt;
				}
				else { 
					ppCharge.Principal=(double)periodPaymentAmt-ppCharge.Interest;
				}
				if(isRecalculate && (double)overPaidDecrementingAmt>=ppCharge.Principal+ppCharge.Interest) {//Will only happen for prepay.  Skips a payplan charge.
					paymentCount--;//This will ensure that non-recalculated interest gets accurately distributed as well as keeps the number of payments accurate.
					chargesCount++;
					skippedChargesCount++;
					overPaidDecrementingAmt-=(decimal)(ppCharge.Principal+ppCharge.Interest);
					_listPayPlanCharges.Add(CreateDebitCharge(0,0,ppCharge.ChargeDate,Lan.g(this,"Prepaid")));//placeholder
					if(overPaidDecrementingAmt==0) {
						principalDecrementingAmt-=(decimal)overPaidAmt;//Remove the amount overpaid from current principal balance to recalculate correct interest
					}
					continue;
				}
				if(isRecalculate && overPaidDecrementingAmt>0) {//Partial prepayment
					principalDecrementingAmt-=(decimal)overPaidAmt;//Remove the amount overpaid from current principal balance to recalculate correct interest
					ppCharge.Principal-=(double)overPaidDecrementingAmt;//Since this was a partial payment, reduce the overpayment amount from the first month of new plan.
					overPaidAmt=0;
					overPaidDecrementingAmt=0;
					if(_formPayPlanRecalculate.IsRecalculateInterest) {//Calculate interest based off of the balance AFTER removing the prepayment amount.
						ppCharge.Interest=Math.Round(((double)principalDecrementingAmt*periodRate),_roundDec);//2 decimals
					}
				}
				if(paymentCount>0 && (chargesCount-skippedChargesCount)==(paymentCount-1)) {//Using # payments method and this is the last payment.
																																										//The purpose of this code block is to fix any rounding issues.  Corrects principal when off by a few pennies.  Principal will decrease slightly and interest will increase slightly to keep payment amounts consistent.
					ppCharge.Principal=(double)principalDecrementingAmt;//All remaining principal.  Causes loop to exit.  This is where the rounding error is eliminated.
					if(periodRate!=0 && !isRecalculate) {//Interest amount on last entry must stay zero for payment plans with zero APR. When APR is zero, the interest amount is set to zero above, and the last payment amount might be less than the other payment amounts.
						ppCharge.Interest=((double)periodPaymentAmt)-ppCharge.Principal;//Force the payment amount to match the rest of the period payments.
					}
					if(isRecalculate && !_formPayPlanRecalculate.IsRecalculateInterest) {
						ppCharge.Interest=(double)interestUnpaidDecrementingAmt;
					}
				}
				else if(paymentCount==0 && principalDecrementingAmt+(decimal)ppCharge.Interest<=periodPaymentAmt) {//Payment amount method, last payment.
					ppCharge.Principal=(double)principalDecrementingAmt;//All remaining principal.  Causes loop to exit.
																															//Interest was calculated above.
				}
				principalDecrementingAmt-=(decimal)ppCharge.Principal;
				interestUnpaidDecrementingAmt-=(decimal)ppCharge.Interest;//Only matters if not recalculating interest
																																	//If somehow principalDecrementing was slightly negative right here due to rounding errors, then at worst the last charge amount would wrong by a few pennies and the loop would immediately exit.
				_listPayPlanCharges.Add(ppCharge);
				chargesCount++;
			}
			if(isRecalculate && nextChargeIdx<payPlanChargesCeiling 
				&& interestInterimAmt>0 && _listPayPlanCharges.Count>nextChargeIdx) 
			{//If they are recalculating in the middle of a payplan schedule
				double increasedInterestAmt=interestInterimAmt-_listPayPlanCharges[nextChargeIdx].Interest;
				_listPayPlanCharges[nextChargeIdx].Interest=interestInterimAmt;
				_listPayPlanCharges[nextChargeIdx].Note=Lan.g(this,"Increased interest")+": "+increasedInterestAmt.ToString("c");
			}
			FillCharges();
			SetNote();
		}

		private void SetNote() {
			textNote.Text=_payPlanNote+DateTime.Today.ToShortDateString()
				+" - "+Lan.g(this,"Date of Agreement")+": "+textDate.Text
				+", "+Lan.g(this,"Total Amount")+": "+textAmount.Text
				+", "+Lan.g(this,"APR")+": "+textAPR.Text
				+", "+Lan.g(this,"Total Cost of Loan")+": "+textTotalCost.Text;
			if(_totalNegAdjAmt!=0) {
				textNote.Text+=", "+Lan.g(this,"Adjustment")+": "+_totalNegAdjAmt.ToString("f");
			}
		}

		private decimal CalcPeriodPayment(double principalAmt,double periodRate,double interestUnpaidAmt,int payPlanChargesCount,bool isRecalculate) {
			decimal periodPaymentAmt=0;
			if(textPaymentCount.Text=="") {//Use a specified payment amount.
				periodPaymentAmt=PIn.Decimal(textPeriodPayment.Text);
				_payPlanCur.PayAmt=(double)periodPaymentAmt;
			}
			else {//Use the given number of payments.
				int paymentCount=PIn.Int(textPaymentCount.Text)-payPlanChargesCount;//countPayPlanCharges will be 0 unless isRecalculate=true
				double periodExactAmt=0;
				if(PIn.Double(textAPR.Text)==0) {
					periodExactAmt=principalAmt/paymentCount;
				}
				else if(isRecalculate && !_formPayPlanRecalculate.IsRecalculateInterest) {//APR applies, but the user wants to keep the old interest.
					periodExactAmt=(principalAmt+interestUnpaidAmt)/paymentCount;
				}
				else {//APR applies and the user wants to recalculate the interest.
					periodExactAmt=principalAmt*periodRate/(1-Math.Pow(1+periodRate,-paymentCount));
				}
				//Round up to the nearest penny (or international equivalent).  
				//This causes the principal on the last payment to be less than or equal to the other principal amounts.
				periodPaymentAmt=(decimal)(Math.Ceiling(periodExactAmt*Math.Pow(10,_roundDec))/Math.Pow(10,_roundDec));
				_payPlanCur.NumberOfPayments=paymentCount+payPlanChargesCount;//countPayPlanCharges will 0 unless isRecalculate=true
			}
			return periodPaymentAmt;
		}

		///<summary>Creates a new sheet from a given Pay plan.</summary>
		private Sheet PayPlanToSheet(PayPlan payPlan) {
			Sheet sheetPP=SheetUtil.CreateSheet(SheetDefs.GetInternalOrCustom(SheetInternalType.PaymentPlan),PatCur.PatNum);
			sheetPP.Parameters.Add(new SheetParameter(true,"payplan") { ParamValue=payPlan });
			sheetPP.Parameters.Add(new SheetParameter(true,"Principal") { ParamValue=TotPrinc.ToString("n") });
			sheetPP.Parameters.Add(new SheetParameter(true,"totalFinanceCharge") { ParamValue=TotInt });
			sheetPP.Parameters.Add(new SheetParameter(true,"totalCostOfLoan") { ParamValue=TotPrincIntAdj.ToString("n") });
			SheetFiller.FillFields(sheetPP);
			return sheetPP;
		}

		private double CalcPeriodRate() {
			double APR=PIn.Double(textAPR.Text);
			_payPlanCur.APR=APR;
			double periodRate;
			if(APR==0) {
				periodRate=0;
			}
			else {
				if(FormPayPlanOpts.radioWeekly.Checked) {
					periodRate=APR/100/52;
					_payPlanCur.PaySchedule=PaymentSchedule.Weekly;
				}
				else if(FormPayPlanOpts.radioEveryOtherWeek.Checked) {
					periodRate=APR/100/26;
					_payPlanCur.PaySchedule=PaymentSchedule.BiWeekly;
				}
				else if(FormPayPlanOpts.radioOrdinalWeekday.Checked) {
					periodRate=APR/100/12;
					_payPlanCur.PaySchedule=PaymentSchedule.MonthlyDayOfWeek;
				}
				else if(FormPayPlanOpts.radioMonthly.Checked) {
					periodRate=APR/100/12;
					_payPlanCur.PaySchedule=PaymentSchedule.Monthly;
				}
				else {//quarterly
					periodRate=APR/100/4;
					_payPlanCur.PaySchedule=PaymentSchedule.Quarterly;
				}
			}
			return periodRate;
		}

		///<summary>periodNum is zero-based.</summary>
		private DateTime CalcNextPeriodDate(DateTime firstDate,int periodNum) {
			DateTime retVal=DateTime.Today;
			if(FormPayPlanOpts.radioWeekly.Checked) {
				retVal=firstDate.AddDays(7*periodNum);
			}
			else if(FormPayPlanOpts.radioEveryOtherWeek.Checked) {
				retVal=firstDate.AddDays(14*periodNum);
			}
			else if(FormPayPlanOpts.radioOrdinalWeekday.Checked) {//First/second/etc Mon/Tue/etc of month
				DateTime roughMonth=firstDate.AddMonths(1*periodNum);//this just gets us into the correct month and year
				DayOfWeek dayOfWeekFirstDate=firstDate.DayOfWeek;
				//find the starting point for the given month: the first day that matches day of week
				DayOfWeek dayOfWeekFirstMonth=(new DateTime(roughMonth.Year,roughMonth.Month,1)).DayOfWeek;
				if(dayOfWeekFirstMonth==dayOfWeekFirstDate) {//1st is the proper day of the week
					retVal=new DateTime(roughMonth.Year,roughMonth.Month,1);
				}
				else if(dayOfWeekFirstMonth<dayOfWeekFirstDate) {//Example, 1st is a Tues (2), but we need to start on a Thursday (4)
					retVal=new DateTime(roughMonth.Year,roughMonth.Month,dayOfWeekFirstDate-dayOfWeekFirstMonth+1);//4-2+1=3.  The 3rd is a Thursday
				}
				else {//Example, 1st is a Thursday (4), but we need to start on a Monday (1) 
					retVal=new DateTime(roughMonth.Year,roughMonth.Month,7-(dayOfWeekFirstMonth-dayOfWeekFirstDate)+1);//7-(4-1)+1=5.  The 5th is a Monday
				}
				int ordinalOfMonth=GetOrdinalOfMonth(firstDate);//for example 3 if it's supposed to be the 3rd Friday of each month
				retVal=retVal.AddDays(7*(ordinalOfMonth-1));//to get to the 3rd Friday, and starting from the 1st Friday, we add 2 weeks.
			}
			else if(FormPayPlanOpts.radioMonthly.Checked) {
				retVal=firstDate.AddMonths(1*periodNum);
			}
			else {//quarterly
				retVal=firstDate.AddMonths(3*periodNum);
			}
			return retVal;
		}

		///<summary>Helper method to create a debit charge which will be associated to the current payment plan.</summary>
		private PayPlanCharge CreateDebitCharge(double principalAmt,double interestAmt,DateTime dateCharge,string note) {
			PayPlanCharge ppCharge=new PayPlanCharge();
			ppCharge.PayPlanNum=_payPlanCur.PayPlanNum;
			//FamCur is the family of the patient, so check to see if the guarantor is in the patient's family. 
			//If the guar and pat are in the same family, then use the patnum. else, use guarantor.
			if(FamCur.ListPats.Select(x => x.PatNum).Contains(_payPlanCur.Guarantor)) {
				ppCharge.Guarantor=_payPlanCur.PatNum;
			}
			else {
				ppCharge.Guarantor=_payPlanCur.Guarantor;
			}
			ppCharge.PatNum=_payPlanCur.PatNum;
			ppCharge.ChargeDate=dateCharge;
			ppCharge.Interest=interestAmt;
			ppCharge.Principal=principalAmt;
			ppCharge.Note=note;
			ppCharge.ChargeType=PayPlanChargeType.Debit;
			ppCharge.ProvNum=_selectedProvNum;
			ppCharge.ClinicNum=_selectedClinicNum;
			return ppCharge;
		}

		private void checkExcludePast_CheckedChanged(object sender,EventArgs e) {
			FillCharges();
		}

		private void butAdj_Click(object sender,EventArgs e) {
			if(_payPlanCur.IsClosed) {
				MsgBox.Show(this,"Cannot add adjustments to closed payment plans.");
				return;
			}
			if(PrefC.GetInt(PrefName.PayPlanAdjType)==0) {
				MsgBox.Show(this,"Adjustments cannot be created for payment plans until a default adjustment type has been selected in account preferences.");
				return;
			}
			InputBox inputBox=new InputBox(new List<InputBoxParam>() 
			{
				new InputBoxParam(InputBoxType.ValidDouble,Lan.g(this,"Please enter an amount:")+" "),
				new InputBoxParam(InputBoxType.CheckBox,"",Lan.g(this,"Also make line item in Account Module"),new Size(250,30)),
			}
				,new Func<string,bool>((text) => {
					Double amount=PIn.Double(text);
					if(amount==0) {
						MsgBox.Show(this,"Please enter a valid value");
						return false;
					}
					if(amount<0) {
						MsgBox.Show(this,"Please enter a positive value for the negative adjustment");
						return false;
					}
					return true;
				})
			);
			inputBox.setTitle(Lan.g(this,"Negative Pay Plan Adjustment"));
			inputBox.Size=new Size(350,170);
			if(inputBox.ShowDialog()!=DialogResult.OK) {
				return;
			}
			double negAdjAmt=-(PIn.Double(inputBox.textResult.Text));
			if((_totalNegFutureAdjs+negAdjAmt)*-1 > _totalFutureDebits) {//make negative to compare _totalFutureDebits
				MsgBox.Show(this,"Cannot add an adjustment totaling more than remaining balance due");
				return;
			}
			CreatePayPlanAdjustments(negAdjAmt);
			if(inputBox.checkBoxResult.Checked) {//Make adjustment visible in account module.
				//set the information here, insert to the db upon saving
				Adjustment adj=new Adjustment();
				adj.AdjAmt=negAdjAmt; 
				adj.DateEntry=DateTime.Now.Date;
				adj.AdjDate=DateTime.Now.Date;
				adj.PatNum=PatCur.PatNum;
				adj.AdjNote=Lan.g(this,"Payment plan adjustment");
				adj.SecUserNumEntry=Security.CurUser.UserNum;
				adj.SecDateTEdit=DateTime.Now;
				if(Defs.GetDef(DefCat.AdjTypes,PrefC.GetLong(PrefName.PayPlanAdjType))!=null) {
					adj.AdjType=Defs.GetDef(DefCat.AdjTypes,PrefC.GetLong(PrefName.PayPlanAdjType)).DefNum;
				}
				_listAdjustments.Add(adj);
			}
			//add negative tx credit offset to tx credits if there is a completed amount
			if(PIn.Double(textCompletedAmt.Text)>0) {
				PayPlanCharge txOffset=new PayPlanCharge() {
					ChargeDate=DateTime.Now.Date,
					ChargeType=PayPlanChargeType.Credit,//needs to be saved as a credit to show in Tx Form
					Guarantor=PatCur.PatNum,
					Note=Lan.g(this,"Adjustment"),
					PatNum=PatCur.PatNum,
					PayPlanNum=_payPlanCur.PayPlanNum,
					Principal=negAdjAmt,
					ProcNum=0,
				};
				_listPayPlanCharges.Add(txOffset);
				double txTotalAmt=PayPlans.GetTxTotalAmt(_listPayPlanCharges);
				textTotalTxAmt.Text=txTotalAmt.ToString("f");
			}
			FillCharges();
			SetNote();
		}

		/// <summary>Allocate money for payplan adjustments for one or multiple adjustments.</summary>
		private void CreatePayPlanAdjustments(double negAdjAmt) {
			double moneyToAllocate=(_totalNegFutureAdjs+negAdjAmt)*-1;//the total amount of ALL future adjustments, existing + new. 
			//Get a list of all our charges for this payplan.
			List<PayPlanCharge> listRemainingCharges=_listPayPlanCharges.FindAll(x => x.ChargeType==PayPlanChargeType.Debit && x.Principal >= 0)
				.OrderByDescending(x => x.ChargeDate).ToList();
			//Get a list of all current adjustments for the payment plan. (Existing before the newly added adjustment).
			List<PayPlanCharge> listCurAdjs=_listPayPlanCharges.FindAll(x => x.ChargeType==PayPlanChargeType.Debit && x.Principal<0 
				&& x.ChargeDate > DateTime.Today);
			//Remove all the adjustments so we can more easily move money around and redistribute if necessary.
			_listPayPlanCharges.RemoveAll(x => x.ChargeType==PayPlanChargeType.Debit && x.Principal<0 && x.ChargeDate > DateTime.Today);
			foreach(PayPlanCharge charge in listRemainingCharges) {
				double chargeAmtRemaining=(charge.Principal);//The amount that can still be adjusted off of this charge, how much is left.
				#region Re-allocate existing adjustments
				List<PayPlanCharge> listExistNegAdjsForCharge=listCurAdjs.FindAll(x => x.ChargeDate.Date==charge.ChargeDate.Date)
					.OrderBy(x => x.ChargeDate).ToList();
				if(listExistNegAdjsForCharge.Count!=0) {//There exists adjustments for the currrent charge/debit row.
					double existingNegAdjTotal=listExistNegAdjsForCharge.Sum(x => x.Principal);
					if(existingNegAdjTotal*(-1)==chargeAmtRemaining){
						//all of these adjustments apply to this charge. Add them all back, no need to keep looping through the adjs.
						_listPayPlanCharges.AddRange(listExistNegAdjsForCharge);
						moneyToAllocate-=chargeAmtRemaining;
						continue; 
					}
					//first loop through the existing adjustments and see what can be applied
					foreach(PayPlanCharge existingAdj in listExistNegAdjsForCharge) {
						if(chargeAmtRemaining==0) {//All adjustments have been allocated for this charge.
							break;
						}
						else if(moneyToAllocate < existingAdj.Principal*-1) {
							//Remaining amount is less then the current adjustment
							//We won't add this adjustment back in but we will create a new adjustment down below to account for this plus the new adj we are adding.
							continue;
						}
						if(existingAdj.Principal*(-1) <= chargeAmtRemaining) { 
							_listPayPlanCharges.Add(existingAdj);
							chargeAmtRemaining-=existingAdj.Principal*(-1);
							moneyToAllocate-=existingAdj.Principal*-1;
						}
						else {//adjustment is bigger that amt avaialable to be applied. Keep removed, add amount back into the total make a new adjustment later.
							moneyToAllocate+=existingAdj.Principal*-1;
						}
						//Leave adjustment gone, only allocate for what we have remaining. 
					}
				}
				#endregion
				if(moneyToAllocate==0) {
					break;
				}
				PayPlanCharge ppc=new PayPlanCharge();
				ppc.ChargeDate=charge.ChargeDate;
				ppc.PatNum=charge.PatNum;
				ppc.PayPlanNum=charge.PayPlanNum;
				ppc.Guarantor=charge.Guarantor;
				ppc.ProvNum=charge.ProvNum;
				ppc.ProcNum=charge.ProcNum;
				ppc.Note="";
				ppc.ChargeType=PayPlanChargeType.Debit;
				if(moneyToAllocate<=chargeAmtRemaining) {
					//Balance is equal to adjustment, apply whole adj amt to charge and be done.
					//Or adj won't cover the whole remaining charge, apply what we can and be done. 
					ppc.Principal=moneyToAllocate*-1;
					_listPayPlanCharges.Add(ppc);
					break;//moneyToAllocate should be 0 here. No need to keep looping. 
				}
				else {//Entire principal can be allocated to this adjustment. Move money to the principal for entire charge amount and keep looping. 
					ppc.Principal=chargeAmtRemaining*-1;
					moneyToAllocate-=chargeAmtRemaining;
					_listPayPlanCharges.Add(ppc);
				}
			}
		}

		///<summary></summary>
		private void SaveData() {
			if(textAPR.Text=="") {
				textAPR.Text="0";
			}
			//PatNum not editable.
			//Guarantor set already
			_payPlanCur.PayPlanDate=PIn.Date(textDate.Text);
			//The following variables were handled when the amortization schedule was created.
			//PayPlanCur.APR
			//PayPlanCur.PaySchedule
			//PayPlanCur.NumberOfPayments
			//PayPlanCur.PayAmt
			//PayPlanCur.DownPayment
			_payPlanCur.Note=textNote.Text;
			_payPlanCur.CompletedAmt=PIn.Double(textCompletedAmt.Text);
			if(comboCategory.SelectedIndex <= 0) {
				_payPlanCur.PlanCategory=0;
			}
			else {
				_payPlanCur.PlanCategory=comboCategory.SelectedTag<long>();
			}
			//PlanNum set already
			if(IsInsPayPlan) { //if insurance payment plan, remove all other credits and create one credit for the completed amt.
				_listPayPlanCharges.RemoveAll(x => x.ChargeType==PayPlanChargeType.Credit);//remove all credits
				PayPlanCharge addCharge=new PayPlanCharge();
				addCharge.ChargeDate=PIn.Date(textDate.Text);
				addCharge.ChargeType=PayPlanChargeType.Credit;
				addCharge.Guarantor=_payPlanCur.Guarantor; //credits always show in the account of the patient that the payplan was for.
				addCharge.Interest=0;
				addCharge.Note=Lan.g(this,"Expected Payments from")+" "+textInsPlan.Text;
				addCharge.PatNum=_payPlanCur.PatNum;
				addCharge.PayPlanNum=_payPlanCur.PayPlanNum;
				addCharge.Principal=PIn.Double(textCompletedAmt.Text);
				addCharge.ProcNum=0;
				//addCharge.ProvNum=0; //handled below
				//addCharge.ClinicNum=0; //handled below
				_listPayPlanCharges.Add(addCharge);
			}
			if(_listAdjustments.Count>0) {
				_listAdjustments.ForEach(x => Adjustments.Insert(x));
			}
			if(PayPlans.GetOne(_payPlanCur.PayPlanNum)==null) {
				//The payment plan no longer exists in the database. 
				MsgBox.Show(this,"This payment plan has been deleted by another user.");
				return;
			}
			PayPlans.Update(_payPlanCur);//always saved to db before opening this form
			MakeSecLogEntries();
			foreach(PayPlanCharge charge in _listPayPlanCharges) {
				charge.ClinicNum=_selectedClinicNum;
				charge.ProvNum=_selectedProvNum;
			}
			for(int i = 0;i<_listPayPlanCharges.Count;i++) {
				_listPayPlanCharges[i].ClinicNum=_selectedClinicNum;
				_listPayPlanCharges[i].ProvNum=_selectedProvNum;
			}
			PayPlanCharges.Sync(_listPayPlanCharges,_payPlanCur.PayPlanNum);
		}

		///<summary>Changes to the database are all made when the form is closed, so all securitylog entries have been consolidated into this one method.</summary>
		private void MakeSecLogEntries() {
			//logs creating, closing out, deleting, signing, and printing of payment plan.
			//deleted logs are in butDelete_click since that method doesn't call SaveData.
			//new
			if(IsNew) {
				SecurityLogs.MakeLogEntry(Permissions.PayPlanEdit,PatCur.PatNum,
							(_payPlanCur.PlanNum == 0 ? "Patient" : "Insurance") + " Payment Plan created.",_payPlanOld.PayPlanNum,DateTime.MinValue);
				return;
			}
			//closed
			if(!_payPlanCur.IsClosed && _payPlanOld.IsClosed) {
				SecurityLogs.MakeLogEntry(Permissions.PayPlanEdit,PatCur.PatNum,
							(_payPlanCur.PlanNum == 0 ? "Patient" : "Insurance") + " Payment Plan reopened.",_payPlanOld.PayPlanNum,DateTime.MinValue);
			}
			if(signatureBoxWrapper.GetSigChanged()) {
				//signed
				if(!_isSigOldValid && !signatureBoxWrapper.SigIsBlank && signatureBoxWrapper.IsValid) {
					SecurityLogs.MakeLogEntry(Permissions.PayPlanEdit,PatCur.PatNum,
								(_payPlanCur.PlanNum == 0 ? "Patient" : "Insurance") + " Payment Plan signed.",_payPlanOld.PayPlanNum,DateTime.MinValue);
				}
				//sig invalidated
				if(_isSigOldValid && (!signatureBoxWrapper.IsValid || signatureBoxWrapper.SigIsBlank)) {
					SecurityLogs.MakeLogEntry(Permissions.PayPlanEdit,PatCur.PatNum,
								(_payPlanCur.PlanNum == 0 ? "Patient" : "Insurance") + " Payment Plan signature invalidated.",_payPlanOld.PayPlanNum,DateTime.MinValue);
				}
			}
			//guarantor changed
			if(_payPlanOld.Guarantor != _payPlanCur.Guarantor) {
				SecurityLogs.MakeLogEntry(Permissions.PayPlanEdit,PatCur.PatNum,
							(_payPlanCur.PlanNum == 0 ? "Patient" : "Insurance") + " Payment Plan guarantor changed from "
							+Patients.GetNameLF(_payPlanOld.Guarantor)+" to "+Patients.GetNameLF(_payPlanCur.Guarantor)+".",_payPlanOld.PayPlanNum,DateTime.MinValue);
			}
			//Completed Amt Changed
			if(_payPlanOld.CompletedAmt != _payPlanCur.CompletedAmt) {
				SecurityLogs.MakeLogEntry(Permissions.PayPlanEdit,PatCur.PatNum,
							(_payPlanCur.PlanNum == 0 ? "Patient" : "Insurance") + " Payment Plan completed amount changed.",_payPlanOld.PayPlanNum,DateTime.MinValue);
			}
			//Ins Plan Changed
			if(_payPlanOld.PlanNum != _payPlanCur.PlanNum) {
				SecurityLogs.MakeLogEntry(Permissions.PayPlanEdit,PatCur.PatNum,
							(_payPlanCur.PlanNum == 0 ? "Patient" : "Insurance") + " Payment Plan ins plan changed.",_payPlanOld.PayPlanNum,DateTime.MinValue);
			}
			//Note Changed
			if(_payPlanOld.Note != _payPlanCur.Note) {
				SecurityLogs.MakeLogEntry(Permissions.PayPlanEdit,PatCur.PatNum,
							(_payPlanCur.PlanNum == 0 ? "Patient" : "Insurance") + " Payment Plan note changed.",_payPlanOld.PayPlanNum,DateTime.MinValue);
			}
			//closed
			if(_payPlanCur.IsClosed && !_payPlanOld.IsClosed) {
				SecurityLogs.MakeLogEntry(Permissions.PayPlanEdit,PatCur.PatNum,
							(_payPlanCur.PlanNum == 0 ? "Patient" : "Insurance") + " Payment Plan closed.",_payPlanOld.PayPlanNum,DateTime.MinValue);
			}
		}

		private void butDelete_Click(object sender,System.EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete payment plan?  All debits and credits will also be deleted.")) {
				return;
			}
			//later improvement if needed: possibly prevent deletion of some charges like older ones.
			try {
				PayPlans.Delete(_payPlanCur);
				//Delete log here since this button doesn't call SaveData().
				SecurityLogs.MakeLogEntry(Permissions.PayPlanEdit,PatCur.PatNum,
							(_payPlanOld.PlanNum == 0 ? "Patient" : "Insurance") + " Payment Plan deleted.",_payPlanOld.PayPlanNum,DateTime.MinValue);
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(_payPlanCur.IsClosed) {
				butOK.Text="OK";
				butDelete.Enabled=true;
				butClosePlan.Enabled=true;
				labelClosed.Visible=false;
				_payPlanCur.IsClosed=false;
				return;
			}
			if(HasErrors()) {
				return;
			}
			if(IsInsPayPlan && _payPlanCur.PlanNum==0) {
				MsgBox.Show(this,"An insurance plan must be selected.");
				return;
			}
			//insurance payment plans use the CompletedAmt text box, regular payment plans use totalTxAmt text box for validation.
			if(IsInsPayPlan && PIn.Double(textCompletedAmt.Text)!=PIn.Double(textAmount.Text)) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Tx Completed Amt and Total Amount do not match, continue?")) {
					return;
				}
			}
			else if(!IsInsPayPlan && PIn.Double(textTotalTxAmt.Text)!=PIn.Double(textAmount.Text) 
				&& PrefC.GetInt(PrefName.PayPlansVersion)!=(int)PayPlanVersions.NoCharges) //Credits do not matter in ppv4
			{
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Total Tx Amt and Total Amount do not match, continue?")) {
					return;
				}
			}
			SaveData();
			DialogResult=DialogResult.OK;
			Plugins.HookAddCode(this,"FormPaymentPlan.butOK_Click_end",PatCur,_payPlanCur,IsNew);
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormPayPlan_Closing(object sender,System.ComponentModel.CancelEventArgs e) {
			if(DialogResult==DialogResult.OK) {
				return;
			}
			if(IsNew){
				try{
					PayPlans.Delete(_payPlanCur);
				}
				catch(Exception ex){
					MessageBox.Show(ex.Message);
					e.Cancel=true;
					return;
				}
			}
		}

	}
}
