/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;
using OpenDental.Bridges;
using OpenDental.UI;
using OpenDentBusiness;
using CodeBase;
using OpenDentBusiness.Crud;
using System.Linq;
using OpenDentBusiness.Eclaims;

namespace OpenDental{
///<summary></summary>
	public class FormInsPlan : ODForm {
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label28;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private OpenDental.ValidDate textDateEffect;
		private OpenDental.ValidDate textDateTerm;
		///<summary>The InsPlan is always inserted before opening this form.</summary>
		public bool IsNewPlan;
		///<summary>The PatPlan is always inserted before opening this form.</summary>
		public bool IsNewPatPlan;
		private System.Windows.Forms.CheckBox checkRelease;
		private System.Windows.Forms.TextBox textSubscriber;
		private System.Windows.Forms.GroupBox groupSubscriber;
		private OpenDental.UI.Button butDelete;
		private System.Windows.Forms.TextBox textSubscriberID;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.CheckBox checkAssign;
		/// <summary>used in the emp dropdown logic</summary>
		private string empOriginal;
		/// <summary>displayed from within code, not designer</summary>
		private System.Windows.Forms.ListBox listEmps;
		private bool mouseIsInListEmps;
		private List<Carrier> similarCars;
		private string carOriginal;
		private System.Windows.Forms.ListBox listCars;
		private bool mouseIsInListCars;
		private System.Windows.Forms.Label labelDrop;
		private OpenDental.UI.Button butDrop;
		private OpenDental.ODtextBox textSubscNote;
		private OpenDental.UI.Button butLabel;
		private System.Windows.Forms.GroupBox groupRequestBen;
		private System.Windows.Forms.Label labelTrojanID;
		private System.Windows.Forms.TextBox textTrojanID;
		private OpenDental.UI.Button butImportTrojan;
		private OpenDental.UI.Button butIapFind;
		private OpenDental.UI.Button butBenefitNotes;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.Label labelPatID;
		private Carrier _carrierCur;
		private System.Windows.Forms.ComboBox comboRelationship;
		private System.Windows.Forms.CheckBox checkIsPending;
		private System.Windows.Forms.TextBox textPatID;
		private OpenDental.UI.Button butAdjAdd;
		private System.Windows.Forms.ListBox listAdj;
		private System.Windows.Forms.Panel panelPat;
		private PatPlan PatPlanCur;
		private ArrayList AdjAL;
		private OpenDental.ValidNumber textOrdinal;
		private OpenDental.UI.ODGrid gridBenefits;
		///<summary>This is the current benefit list that displays on the form.  It does not get saved to the database until this form closes.</summary>
		private List<Benefit> benefitList;//each item is a Benefit
		private List<Benefit> benefitListOld;
		private OpenDental.UI.Button butPick;
		private ODtextBox textPlanNote;
		private Label label18;
		//<summary>Set to true if called from the list of insurance plans.  In this case, the planNum will be 0.  There will be no subscriber.  Benefits will be 'typical' rather than from one specific plan.  Upon saving, all similar plans will be set to be exactly the same as PlanCur.</summary>
		//public bool IsForAll;//Instead, just pass in a null subscriber.
		///<summary>Set to true from FormInsPlansMerge.  In this case, the insplan is read only, because it's much more complicated to allow user to change.</summary>
		//public bool IsReadOnly;
		private List<FeeSched> FeeSchedsStandard;
		private List<FeeSched> FeeSchedsCopay;
		private List<FeeSched> FeeSchedsAllowed;
		private OpenDental.UI.Button butGetElectronic;
		private TextBox textElectBenLastDate;
		private Label labelHistElect;
		private OpenDental.UI.Button butHistoryElect;
		private RadioButton radioChangeAll;
		private GroupBox groupChanges;
		private RadioButton radioCreateNew;
		private UI.Button butChange;
		private UI.Button butAudit;
		private bool _hasDropped=false;
		private bool _hasOrdinalChanged=false;
		private bool _hasCarrierChanged=false;
		private TextBox textPatPlanNum;
		private Label label27;
		private UI.Button butVerifyPatPlan;
		private ValidDate textDateLastVerifiedPatPlan;
		private UI.Button butVerifyBenefits;
		private ValidDate textDateLastVerifiedBenefits;
		private GroupBox groupBox1;
		private Label label30;
		private Label label34;
		private CheckBox checkDontVerify;
		private InsSub _subOld;
		private DateTime _dateInsPlanLastVerified;
		private DateTime _datePatPlanLastVerified;
		///<summary>The carrier num when the window was loaded.  Used to track if carrier has been changed.</summary>
		private long _carrierNumOrig;
		///<summary>The employer num when the window was loaded.  Used to track if the employer has been changed.</summary>
		private string _employerNameOrig;
		private string _employerNameCur;
		private TabControl tabControlInsPlan;
		private TabPage tabPageInsPlanInfo;
		private Panel panelPlan;
		private TextBox textInsPlanNum;
		private Label label29;
		private GroupBox groupPlan;
		private UI.Button butOtherSubscribers;
		private TextBox textBIN;
		private Label labelBIN;
		private TextBox textDivisionNo;
		private TextBox textGroupName;
		private TextBox textEmployer;
		private GroupBox groupCarrier;
		private UI.Button butPickCarrier;
		private TextBox textPhone;
		private TextBox textAddress;
		private ComboBox comboElectIDdescript;
		private TextBox textElectID;
		private UI.Button butSearch;
		private TextBox textAddress2;
		private TextBox textZip;
		private CheckBox checkNoSendElect;
		private Label label10;
		private TextBox textCity;
		private Label label7;
		private TextBox textCarrier;
		private Label labelElectronicID;
		private Label label21;
		private Label label17;
		private TextBox textState;
		private Label labelCitySTZip;
		private CheckBox checkIsMedical;
		private TextBox textGroupNum;
		private Label labelGroupNum;
		private Label label8;
		private ComboBox comboLinked;
		private TextBox textLinkedNum;
		private Label label16;
		private Label label4;
		private Label labelDivisionDash;
		private ComboBox comboPlanType;
		private Label label14;
		private TabPage tabPageOtherInsInfo;
		private Panel panelOrthInfo;
		private ComboBox comboCobRule;
		private Label label20;
		private ComboBox comboFilingCodeSubtype;
		private Label label15;
		private CheckBox checkIsHidden;
		private CheckBox checkCodeSubst;
		private CheckBox checkShowBaseUnits;
		private ComboBox comboFilingCode;
		private Label label13;
		private ComboBox comboClaimForm;
		private Label label23;
		private CheckBox checkAlternateCode;
		private CheckBox checkClaimsUseUCR;
		private TabPage tabPageCanadian;
		private Panel panelCanadian;
		private GroupBox groupCanadian;
		private Label label19;
		private TextBox textCanadianInstCode;
		private Label label9;
		private TextBox textCanadianDiagCode;
		private CheckBox checkIsPMP;
		private Label label24;
		private Label label22;
		private TextBox textPlanFlag;
		private ValidNumber textDentaide;
		private Label labelDentaide;
		private TabPage tabPageOrtho;
		private Panel panelOrtho;
		private Label labelAutoOrthoProcPeriod;
		private Label label36;
		private CheckBox checkOrthoWaitDays;
		private ComboBox comboOrthoClaimType;
		private ComboBox comboOrthoAutoProcPeriod;
		private UI.Button butPatOrtho;
		private UI.Button butPickOrthoProc;
		private TextBox textOrthoAutoProc;
		private Label label37;
		private string _electIdCur;
		private UI.Button butDefaultAutoOrthoProc;
		private Label labelOrthoAutoFee;
		private ValidDouble textOrthoAutoFee;
		private ComboBox comboFeeSched;
		private GroupBox groupCoPay;
		private Label label12;
		private ComboBox comboAllowedFeeSched;
		private Label labelCopayFeeSched;
		private Label label3;
		private ComboBox comboCopay;
		private Label label1;
		private ComboBox comboBillType;
		private Label label38;
		private UI.Button butSubstCodes;
		private ProcedureCode _orthoAutoProc;
		private List<InsFilingCode> _listInsFilingCodes;
		private List<ClaimForm> _listClaimForms;
		//<summary>This is a field that is accessed only by clicking on the button because there's not room for it otherwise.  This variable should be treated just as if it was a visible textBox.</summary>
		//private string BenefitNotes;

		///<summary>Currently selected plan in the window.</summary>
		private InsPlan _planCur;
		///<summary>This is a copy of PlanCur as it was originally when this form was opened.  
		///This is needed to determine whether plan was changed.  However, this is also reset when 'pick from list' is used.</summary>
		private InsPlan _planCurOriginal;
		///<summary>Ins sub for the currently selected plan.</summary>
		private InsSub _subCur;
		///<summary>A one to one list of defs that are displayed within the billing type combo box.  Item 0 will always be "None".</summary>
		private List<Def> _listBillingTypeDefs;
		///<summary>The plan type that is selected in comboPlanType</summary>
		private InsPlanTypeComboItem _selectedPlanType;

		///<summary>The original plan that was passed into this form. Assigned in the constructor and can never be modified.  
		///This allows intelligent decisions about how to save changes.</summary>
		private InsPlan _planOld {
			get;
		}

		public long PlanCurNum {
			get {
				return _planCur.PlanNum;
			}
		}

		///<summary>Called from ContrFamily and FormInsPlans. Must pass in the plan, patPlan, and sub, although patPlan and sub can be null.</summary>
		public FormInsPlan(InsPlan planCur,PatPlan patPlanCur,InsSub subCur){
			Cursor=Cursors.WaitCursor;
			InitializeComponent();
			_planCur=planCur;
			_planOld=_planCur.Copy();
			PatPlanCur=patPlanCur;
			_subCur=subCur;
			listEmps=new ListBox();
			listEmps.Location=new Point(tabControlInsPlan.Left+tabPageInsPlanInfo.Left+panelPlan.Left+groupPlan.Left+textEmployer.Left,
				tabPageInsPlanInfo.Top+tabControlInsPlan.Top+panelPlan.Top+groupPlan.Top+textEmployer.Bottom);
			listEmps.Size=new Size(231,100);
			listEmps.Visible=false;
			listEmps.Click += new System.EventHandler(listEmps_Click);
			listEmps.DoubleClick += new System.EventHandler(listEmps_DoubleClick);
			listEmps.MouseEnter += new System.EventHandler(listEmps_MouseEnter);
			listEmps.MouseLeave += new System.EventHandler(listEmps_MouseLeave);
			Controls.Add(listEmps);
			listEmps.BringToFront();
			listCars=new ListBox();
			listCars.Location=new Point(tabControlInsPlan.Left+tabPageInsPlanInfo.Left+panelPlan.Left+groupPlan.Left+groupCarrier.Left+textCarrier.Left,
				tabControlInsPlan.Top+tabPageInsPlanInfo.Top+panelPlan.Top+groupPlan.Top+groupCarrier.Top+textCarrier.Bottom);
			listCars.Size=new Size(700,100);
			listCars.HorizontalScrollbar=true;
			listCars.Visible=false;
			listCars.Click += new System.EventHandler(listCars_Click);
			listCars.DoubleClick += new System.EventHandler(listCars_DoubleClick);
			listCars.MouseEnter += new System.EventHandler(listCars_MouseEnter);
			listCars.MouseLeave += new System.EventHandler(listCars_MouseLeave);
			Controls.Add(listCars);
			listCars.BringToFront();
			//tbPercentPlan.CellClicked += new OpenDental.ContrTable.CellEventHandler(tbPercentPlan_CellClicked);
			//tbPercentPat.CellClicked += new OpenDental.ContrTable.CellEventHandler(tbPercentPat_CellClicked);
			Lan.F(this);
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				labelPatID.Text=Lan.g(this,"Dependant Code");
				labelCitySTZip.Text=Lan.g(this,"City,Prov,Post");   //Postal Code";
				butSearch.Visible=false;
				labelElectronicID.Text="EDI Code";
				comboElectIDdescript.Visible=false;
				labelGroupNum.Text=Lan.g(this,"Plan Number");
				checkIsPMP.Checked=(planCur.CanadianPlanFlag!=null && planCur.CanadianPlanFlag!="");
			}
			else{
				labelDivisionDash.Visible=false;
				textDivisionNo.Visible=false;
				//groupCanadian.Visible=false;
				tabControlInsPlan.TabPages.Remove(tabPageCanadian);
			}
			if(CultureInfo.CurrentCulture.Name.Length>=4 && CultureInfo.CurrentCulture.Name.Substring(3)=="GB"){//en-GB
				labelCitySTZip.Text=Lan.g(this,"City,Postcode");
			}
			panelPat.BackColor=Defs.GetFirstForCategory(DefCat.MiscColors).ItemColor;
			//labelViewRequestDocument.Text="         ";
			//if(!PrefC.GetBool(PrefName.CustomizedForPracticeWeb")) {
			//	butEligibility.Visible=false;
			//	labelViewRequestDocument.Visible=false;
			//}
			Cursor=Cursors.Default;
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInsPlan));
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.checkAssign = new System.Windows.Forms.CheckBox();
			this.checkRelease = new System.Windows.Forms.CheckBox();
			this.textSubscriber = new System.Windows.Forms.TextBox();
			this.groupSubscriber = new System.Windows.Forms.GroupBox();
			this.butChange = new OpenDental.UI.Button();
			this.label25 = new System.Windows.Forms.Label();
			this.textSubscriberID = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textDateEffect = new OpenDental.ValidDate();
			this.textDateTerm = new OpenDental.ValidDate();
			this.textSubscNote = new OpenDental.ODtextBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.butImportTrojan = new OpenDental.UI.Button();
			this.butIapFind = new OpenDental.UI.Button();
			this.butBenefitNotes = new OpenDental.UI.Button();
			this.butHistoryElect = new OpenDental.UI.Button();
			this.butGetElectronic = new OpenDental.UI.Button();
			this.butSubstCodes = new OpenDental.UI.Button();
			this.labelDrop = new System.Windows.Forms.Label();
			this.groupRequestBen = new System.Windows.Forms.GroupBox();
			this.labelHistElect = new System.Windows.Forms.Label();
			this.textElectBenLastDate = new System.Windows.Forms.TextBox();
			this.labelTrojanID = new System.Windows.Forms.Label();
			this.textTrojanID = new System.Windows.Forms.TextBox();
			this.label26 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.comboRelationship = new System.Windows.Forms.ComboBox();
			this.label31 = new System.Windows.Forms.Label();
			this.checkIsPending = new System.Windows.Forms.CheckBox();
			this.label32 = new System.Windows.Forms.Label();
			this.label33 = new System.Windows.Forms.Label();
			this.listAdj = new System.Windows.Forms.ListBox();
			this.label35 = new System.Windows.Forms.Label();
			this.textPatID = new System.Windows.Forms.TextBox();
			this.labelPatID = new System.Windows.Forms.Label();
			this.panelPat = new System.Windows.Forms.Panel();
			this.butPatOrtho = new OpenDental.UI.Button();
			this.label30 = new System.Windows.Forms.Label();
			this.textDateLastVerifiedPatPlan = new OpenDental.ValidDate();
			this.butVerifyPatPlan = new OpenDental.UI.Button();
			this.textPatPlanNum = new System.Windows.Forms.TextBox();
			this.label27 = new System.Windows.Forms.Label();
			this.textOrdinal = new OpenDental.ValidNumber();
			this.butAdjAdd = new OpenDental.UI.Button();
			this.butDrop = new OpenDental.UI.Button();
			this.label18 = new System.Windows.Forms.Label();
			this.radioChangeAll = new System.Windows.Forms.RadioButton();
			this.groupChanges = new System.Windows.Forms.GroupBox();
			this.radioCreateNew = new System.Windows.Forms.RadioButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label34 = new System.Windows.Forms.Label();
			this.checkDontVerify = new System.Windows.Forms.CheckBox();
			this.butVerifyBenefits = new OpenDental.UI.Button();
			this.textDateLastVerifiedBenefits = new OpenDental.ValidDate();
			this.butAudit = new OpenDental.UI.Button();
			this.butPick = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butLabel = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.tabControlInsPlan = new System.Windows.Forms.TabControl();
			this.tabPageInsPlanInfo = new System.Windows.Forms.TabPage();
			this.panelPlan = new System.Windows.Forms.Panel();
			this.comboFeeSched = new System.Windows.Forms.ComboBox();
			this.groupCoPay = new System.Windows.Forms.GroupBox();
			this.label12 = new System.Windows.Forms.Label();
			this.comboAllowedFeeSched = new System.Windows.Forms.ComboBox();
			this.labelCopayFeeSched = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.comboCopay = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textInsPlanNum = new System.Windows.Forms.TextBox();
			this.label29 = new System.Windows.Forms.Label();
			this.groupPlan = new System.Windows.Forms.GroupBox();
			this.butOtherSubscribers = new OpenDental.UI.Button();
			this.textBIN = new System.Windows.Forms.TextBox();
			this.labelBIN = new System.Windows.Forms.Label();
			this.textDivisionNo = new System.Windows.Forms.TextBox();
			this.textGroupName = new System.Windows.Forms.TextBox();
			this.textEmployer = new System.Windows.Forms.TextBox();
			this.groupCarrier = new System.Windows.Forms.GroupBox();
			this.butPickCarrier = new OpenDental.UI.Button();
			this.textPhone = new System.Windows.Forms.TextBox();
			this.textAddress = new System.Windows.Forms.TextBox();
			this.comboElectIDdescript = new System.Windows.Forms.ComboBox();
			this.textElectID = new System.Windows.Forms.TextBox();
			this.butSearch = new OpenDental.UI.Button();
			this.textAddress2 = new System.Windows.Forms.TextBox();
			this.textZip = new System.Windows.Forms.TextBox();
			this.checkNoSendElect = new System.Windows.Forms.CheckBox();
			this.label10 = new System.Windows.Forms.Label();
			this.textCity = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.textCarrier = new System.Windows.Forms.TextBox();
			this.labelElectronicID = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.textState = new System.Windows.Forms.TextBox();
			this.labelCitySTZip = new System.Windows.Forms.Label();
			this.checkIsMedical = new System.Windows.Forms.CheckBox();
			this.textGroupNum = new System.Windows.Forms.TextBox();
			this.labelGroupNum = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.comboLinked = new System.Windows.Forms.ComboBox();
			this.textLinkedNum = new System.Windows.Forms.TextBox();
			this.label16 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.labelDivisionDash = new System.Windows.Forms.Label();
			this.comboPlanType = new System.Windows.Forms.ComboBox();
			this.label14 = new System.Windows.Forms.Label();
			this.tabPageOtherInsInfo = new System.Windows.Forms.TabPage();
			this.panelOrthInfo = new System.Windows.Forms.Panel();
			this.comboBillType = new System.Windows.Forms.ComboBox();
			this.label38 = new System.Windows.Forms.Label();
			this.comboCobRule = new System.Windows.Forms.ComboBox();
			this.label20 = new System.Windows.Forms.Label();
			this.comboFilingCodeSubtype = new System.Windows.Forms.ComboBox();
			this.label15 = new System.Windows.Forms.Label();
			this.checkIsHidden = new System.Windows.Forms.CheckBox();
			this.checkCodeSubst = new System.Windows.Forms.CheckBox();
			this.checkShowBaseUnits = new System.Windows.Forms.CheckBox();
			this.comboFilingCode = new System.Windows.Forms.ComboBox();
			this.label13 = new System.Windows.Forms.Label();
			this.comboClaimForm = new System.Windows.Forms.ComboBox();
			this.label23 = new System.Windows.Forms.Label();
			this.checkAlternateCode = new System.Windows.Forms.CheckBox();
			this.checkClaimsUseUCR = new System.Windows.Forms.CheckBox();
			this.tabPageCanadian = new System.Windows.Forms.TabPage();
			this.panelCanadian = new System.Windows.Forms.Panel();
			this.groupCanadian = new System.Windows.Forms.GroupBox();
			this.label19 = new System.Windows.Forms.Label();
			this.textCanadianInstCode = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.textCanadianDiagCode = new System.Windows.Forms.TextBox();
			this.checkIsPMP = new System.Windows.Forms.CheckBox();
			this.label24 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.textPlanFlag = new System.Windows.Forms.TextBox();
			this.textDentaide = new OpenDental.ValidNumber();
			this.labelDentaide = new System.Windows.Forms.Label();
			this.tabPageOrtho = new System.Windows.Forms.TabPage();
			this.panelOrtho = new System.Windows.Forms.Panel();
			this.textOrthoAutoFee = new OpenDental.ValidDouble();
			this.labelOrthoAutoFee = new System.Windows.Forms.Label();
			this.butDefaultAutoOrthoProc = new OpenDental.UI.Button();
			this.butPickOrthoProc = new OpenDental.UI.Button();
			this.textOrthoAutoProc = new System.Windows.Forms.TextBox();
			this.label37 = new System.Windows.Forms.Label();
			this.comboOrthoClaimType = new System.Windows.Forms.ComboBox();
			this.comboOrthoAutoProcPeriod = new System.Windows.Forms.ComboBox();
			this.labelAutoOrthoProcPeriod = new System.Windows.Forms.Label();
			this.label36 = new System.Windows.Forms.Label();
			this.checkOrthoWaitDays = new System.Windows.Forms.CheckBox();
			this.gridBenefits = new OpenDental.UI.ODGrid();
			this.textPlanNote = new OpenDental.ODtextBox();
			this.groupSubscriber.SuspendLayout();
			this.groupRequestBen.SuspendLayout();
			this.panelPat.SuspendLayout();
			this.groupChanges.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabControlInsPlan.SuspendLayout();
			this.tabPageInsPlanInfo.SuspendLayout();
			this.panelPlan.SuspendLayout();
			this.groupCoPay.SuspendLayout();
			this.groupPlan.SuspendLayout();
			this.groupCarrier.SuspendLayout();
			this.tabPageOtherInsInfo.SuspendLayout();
			this.panelOrthInfo.SuspendLayout();
			this.tabPageCanadian.SuspendLayout();
			this.panelCanadian.SuspendLayout();
			this.groupCanadian.SuspendLayout();
			this.tabPageOrtho.SuspendLayout();
			this.panelOrtho.SuspendLayout();
			this.SuspendLayout();
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(7, 57);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(100, 15);
			this.label5.TabIndex = 5;
			this.label5.Text = "Effective Dates";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(182, 57);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(30, 15);
			this.label6.TabIndex = 6;
			this.label6.Text = "To";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label28
			// 
			this.label28.Location = new System.Drawing.Point(2, 78);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(55, 41);
			this.label28.TabIndex = 28;
			this.label28.Text = "Note";
			this.label28.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkAssign
			// 
			this.checkAssign.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAssign.Location = new System.Drawing.Point(294, 54);
			this.checkAssign.Name = "checkAssign";
			this.checkAssign.Size = new System.Drawing.Size(205, 20);
			this.checkAssign.TabIndex = 4;
			this.checkAssign.Text = "Assignment of Benefits (pay provider)";
			// 
			// checkRelease
			// 
			this.checkRelease.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRelease.Location = new System.Drawing.Point(294, 36);
			this.checkRelease.Name = "checkRelease";
			this.checkRelease.Size = new System.Drawing.Size(205, 20);
			this.checkRelease.TabIndex = 3;
			this.checkRelease.Text = "Release of Information";
			// 
			// textSubscriber
			// 
			this.textSubscriber.Location = new System.Drawing.Point(109, 14);
			this.textSubscriber.Name = "textSubscriber";
			this.textSubscriber.ReadOnly = true;
			this.textSubscriber.Size = new System.Drawing.Size(298, 20);
			this.textSubscriber.TabIndex = 109;
			// 
			// groupSubscriber
			// 
			this.groupSubscriber.Controls.Add(this.butChange);
			this.groupSubscriber.Controls.Add(this.checkAssign);
			this.groupSubscriber.Controls.Add(this.label25);
			this.groupSubscriber.Controls.Add(this.checkRelease);
			this.groupSubscriber.Controls.Add(this.textSubscriber);
			this.groupSubscriber.Controls.Add(this.textSubscriberID);
			this.groupSubscriber.Controls.Add(this.label2);
			this.groupSubscriber.Controls.Add(this.textDateEffect);
			this.groupSubscriber.Controls.Add(this.label5);
			this.groupSubscriber.Controls.Add(this.textDateTerm);
			this.groupSubscriber.Controls.Add(this.label6);
			this.groupSubscriber.Controls.Add(this.textSubscNote);
			this.groupSubscriber.Controls.Add(this.label28);
			this.groupSubscriber.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupSubscriber.Location = new System.Drawing.Point(468, 94);
			this.groupSubscriber.Name = "groupSubscriber";
			this.groupSubscriber.Size = new System.Drawing.Size(502, 176);
			this.groupSubscriber.TabIndex = 2;
			this.groupSubscriber.TabStop = false;
			this.groupSubscriber.Text = "Subscriber Information";
			// 
			// butChange
			// 
			this.butChange.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChange.Autosize = true;
			this.butChange.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChange.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChange.CornerRadius = 4F;
			this.butChange.Location = new System.Drawing.Point(413, 13);
			this.butChange.Name = "butChange";
			this.butChange.Size = new System.Drawing.Size(73, 21);
			this.butChange.TabIndex = 121;
			this.butChange.Text = "Change";
			this.toolTip1.SetToolTip(this.butChange, "Change subscriber name");
			this.butChange.Click += new System.EventHandler(this.butChange_Click);
			// 
			// label25
			// 
			this.label25.Location = new System.Drawing.Point(8, 18);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(99, 15);
			this.label25.TabIndex = 115;
			this.label25.Text = "Name";
			this.label25.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textSubscriberID
			// 
			this.textSubscriberID.Location = new System.Drawing.Point(109, 34);
			this.textSubscriberID.MaxLength = 20;
			this.textSubscriberID.Name = "textSubscriberID";
			this.textSubscriberID.Size = new System.Drawing.Size(129, 20);
			this.textSubscriberID.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 36);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(99, 15);
			this.label2.TabIndex = 114;
			this.label2.Text = "Subscriber ID";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDateEffect
			// 
			this.textDateEffect.Location = new System.Drawing.Point(109, 54);
			this.textDateEffect.Name = "textDateEffect";
			this.textDateEffect.Size = new System.Drawing.Size(72, 20);
			this.textDateEffect.TabIndex = 1;
			// 
			// textDateTerm
			// 
			this.textDateTerm.Location = new System.Drawing.Point(215, 54);
			this.textDateTerm.Name = "textDateTerm";
			this.textDateTerm.Size = new System.Drawing.Size(72, 20);
			this.textDateTerm.TabIndex = 2;
			// 
			// textSubscNote
			// 
			this.textSubscNote.AcceptsTab = true;
			this.textSubscNote.BackColor = System.Drawing.SystemColors.Window;
			this.textSubscNote.DetectLinksEnabled = false;
			this.textSubscNote.DetectUrls = false;
			this.textSubscNote.Location = new System.Drawing.Point(57, 75);
			this.textSubscNote.Name = "textSubscNote";
			this.textSubscNote.QuickPasteType = OpenDentBusiness.QuickPasteType.InsPlan;
			this.textSubscNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textSubscNote.Size = new System.Drawing.Size(439, 98);
			this.textSubscNote.TabIndex = 5;
			this.textSubscNote.Text = "1 - InsPlan subscriber\n2\n3 lines will show here in 46 vert.\n4 lines will show her" +
    "e in 59 vert.\n5 lines in 72 vert\n6 lines in 85 vert\n7 lines in 98";
			// 
			// butImportTrojan
			// 
			this.butImportTrojan.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butImportTrojan.Autosize = true;
			this.butImportTrojan.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butImportTrojan.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butImportTrojan.CornerRadius = 4F;
			this.butImportTrojan.Location = new System.Drawing.Point(6, 14);
			this.butImportTrojan.Name = "butImportTrojan";
			this.butImportTrojan.Size = new System.Drawing.Size(55, 21);
			this.butImportTrojan.TabIndex = 0;
			this.butImportTrojan.Text = "Trojan";
			this.toolTip1.SetToolTip(this.butImportTrojan, "Edit all the similar plans at once");
			this.butImportTrojan.Click += new System.EventHandler(this.butImportTrojan_Click);
			// 
			// butIapFind
			// 
			this.butIapFind.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butIapFind.Autosize = true;
			this.butIapFind.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butIapFind.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butIapFind.CornerRadius = 4F;
			this.butIapFind.Location = new System.Drawing.Point(64, 14);
			this.butIapFind.Name = "butIapFind";
			this.butIapFind.Size = new System.Drawing.Size(55, 21);
			this.butIapFind.TabIndex = 1;
			this.butIapFind.Text = "IAP";
			this.toolTip1.SetToolTip(this.butIapFind, "Edit all the similar plans at once");
			this.butIapFind.Click += new System.EventHandler(this.butIapFind_Click);
			// 
			// butBenefitNotes
			// 
			this.butBenefitNotes.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBenefitNotes.Autosize = true;
			this.butBenefitNotes.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBenefitNotes.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBenefitNotes.CornerRadius = 4F;
			this.butBenefitNotes.Location = new System.Drawing.Point(122, 14);
			this.butBenefitNotes.Name = "butBenefitNotes";
			this.butBenefitNotes.Size = new System.Drawing.Size(60, 21);
			this.butBenefitNotes.TabIndex = 2;
			this.butBenefitNotes.Text = "Notes";
			this.toolTip1.SetToolTip(this.butBenefitNotes, "Edit all the similar plans at once");
			this.butBenefitNotes.Click += new System.EventHandler(this.butBenefitNotes_Click);
			// 
			// butHistoryElect
			// 
			this.butHistoryElect.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butHistoryElect.Autosize = true;
			this.butHistoryElect.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butHistoryElect.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butHistoryElect.CornerRadius = 4F;
			this.butHistoryElect.Location = new System.Drawing.Point(89, 38);
			this.butHistoryElect.Name = "butHistoryElect";
			this.butHistoryElect.Size = new System.Drawing.Size(70, 21);
			this.butHistoryElect.TabIndex = 120;
			this.butHistoryElect.Text = "History";
			this.toolTip1.SetToolTip(this.butHistoryElect, "Edit all the similar plans at once");
			this.butHistoryElect.Click += new System.EventHandler(this.butHistoryElect_Click);
			// 
			// butGetElectronic
			// 
			this.butGetElectronic.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGetElectronic.Autosize = true;
			this.butGetElectronic.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGetElectronic.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGetElectronic.CornerRadius = 4F;
			this.butGetElectronic.Location = new System.Drawing.Point(14, 38);
			this.butGetElectronic.Name = "butGetElectronic";
			this.butGetElectronic.Size = new System.Drawing.Size(70, 21);
			this.butGetElectronic.TabIndex = 116;
			this.butGetElectronic.Text = "Request";
			this.toolTip1.SetToolTip(this.butGetElectronic, "Edit all the similar plans at once");
			this.butGetElectronic.Click += new System.EventHandler(this.butGetElectronic_Click);
			// 
			// butSubstCodes
			// 
			this.butSubstCodes.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSubstCodes.Autosize = true;
			this.butSubstCodes.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSubstCodes.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSubstCodes.CornerRadius = 4F;
			this.butSubstCodes.Location = new System.Drawing.Point(54, 23);
			this.butSubstCodes.Name = "butSubstCodes";
			this.butSubstCodes.Size = new System.Drawing.Size(91, 20);
			this.butSubstCodes.TabIndex = 187;
			this.butSubstCodes.Text = "Subst Codes";
			this.toolTip1.SetToolTip(this.butSubstCodes, "Edit all the similar plans at once");
			this.butSubstCodes.Click += new System.EventHandler(this.butSubstCodes_Click);
			// 
			// labelDrop
			// 
			this.labelDrop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelDrop.Location = new System.Drawing.Point(80, 70);
			this.labelDrop.Name = "labelDrop";
			this.labelDrop.Size = new System.Drawing.Size(532, 15);
			this.labelDrop.TabIndex = 124;
			this.labelDrop.Text = "Drop a plan when a patient changes carriers or is no longer covered.  This does n" +
    "ot delete the plan.";
			this.labelDrop.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// groupRequestBen
			// 
			this.groupRequestBen.Controls.Add(this.butHistoryElect);
			this.groupRequestBen.Controls.Add(this.labelHistElect);
			this.groupRequestBen.Controls.Add(this.textElectBenLastDate);
			this.groupRequestBen.Controls.Add(this.butGetElectronic);
			this.groupRequestBen.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupRequestBen.Location = new System.Drawing.Point(468, 269);
			this.groupRequestBen.Name = "groupRequestBen";
			this.groupRequestBen.Size = new System.Drawing.Size(165, 63);
			this.groupRequestBen.TabIndex = 10;
			this.groupRequestBen.TabStop = false;
			this.groupRequestBen.Text = "Request Electronic Benefits";
			// 
			// labelHistElect
			// 
			this.labelHistElect.Location = new System.Drawing.Point(3, 20);
			this.labelHistElect.Name = "labelHistElect";
			this.labelHistElect.Size = new System.Drawing.Size(84, 15);
			this.labelHistElect.TabIndex = 119;
			this.labelHistElect.Text = "Last Request";
			this.labelHistElect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textElectBenLastDate
			// 
			this.textElectBenLastDate.Location = new System.Drawing.Point(89, 17);
			this.textElectBenLastDate.MaxLength = 30;
			this.textElectBenLastDate.Name = "textElectBenLastDate";
			this.textElectBenLastDate.Size = new System.Drawing.Size(70, 20);
			this.textElectBenLastDate.TabIndex = 118;
			// 
			// labelTrojanID
			// 
			this.labelTrojanID.Location = new System.Drawing.Point(192, 18);
			this.labelTrojanID.Name = "labelTrojanID";
			this.labelTrojanID.Size = new System.Drawing.Size(23, 15);
			this.labelTrojanID.TabIndex = 9;
			this.labelTrojanID.Text = "ID";
			this.labelTrojanID.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textTrojanID
			// 
			this.textTrojanID.Location = new System.Drawing.Point(217, 15);
			this.textTrojanID.MaxLength = 30;
			this.textTrojanID.Name = "textTrojanID";
			this.textTrojanID.Size = new System.Drawing.Size(113, 20);
			this.textTrojanID.TabIndex = 8;
			// 
			// label26
			// 
			this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label26.Location = new System.Drawing.Point(20, 22);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(148, 14);
			this.label26.TabIndex = 127;
			this.label26.Text = "Relationship to Subscriber";
			this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.ControlText;
			this.panel1.Location = new System.Drawing.Point(0, 90);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(988, 2);
			this.panel1.TabIndex = 128;
			// 
			// comboRelationship
			// 
			this.comboRelationship.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboRelationship.Location = new System.Drawing.Point(170, 18);
			this.comboRelationship.MaxDropDownItems = 30;
			this.comboRelationship.Name = "comboRelationship";
			this.comboRelationship.Size = new System.Drawing.Size(151, 21);
			this.comboRelationship.TabIndex = 0;
			// 
			// label31
			// 
			this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label31.Location = new System.Drawing.Point(329, 30);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(138, 17);
			this.label31.TabIndex = 130;
			this.label31.Text = "Order";
			this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkIsPending
			// 
			this.checkIsPending.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsPending.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsPending.Location = new System.Drawing.Point(515, 29);
			this.checkIsPending.Name = "checkIsPending";
			this.checkIsPending.Size = new System.Drawing.Size(97, 16);
			this.checkIsPending.TabIndex = 3;
			this.checkIsPending.Text = "Pending";
			this.checkIsPending.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label32
			// 
			this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label32.Location = new System.Drawing.Point(6, 5);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(304, 19);
			this.label32.TabIndex = 134;
			this.label32.Text = "Insurance Plan Information";
			// 
			// label33
			// 
			this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label33.Location = new System.Drawing.Point(5, 0);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(188, 19);
			this.label33.TabIndex = 135;
			this.label33.Text = "Patient Information";
			// 
			// listAdj
			// 
			this.listAdj.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listAdj.Items.AddRange(new object[] {
            "03/05/2001       Ins Used:  $124.00       Ded Used:  $50.00",
            "03/05/2002       Ins Used:  $0.00       Ded Used:  $50.00"});
			this.listAdj.Location = new System.Drawing.Point(678, 28);
			this.listAdj.Name = "listAdj";
			this.listAdj.Size = new System.Drawing.Size(296, 56);
			this.listAdj.TabIndex = 137;
			this.listAdj.DoubleClick += new System.EventHandler(this.listAdj_DoubleClick);
			// 
			// label35
			// 
			this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label35.Location = new System.Drawing.Point(678, 8);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(218, 17);
			this.label35.TabIndex = 138;
			this.label35.Text = "Adjustments to Insurance Benefits: ";
			this.label35.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textPatID
			// 
			this.textPatID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textPatID.Location = new System.Drawing.Point(170, 40);
			this.textPatID.MaxLength = 100;
			this.textPatID.Name = "textPatID";
			this.textPatID.Size = new System.Drawing.Size(151, 20);
			this.textPatID.TabIndex = 1;
			// 
			// labelPatID
			// 
			this.labelPatID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelPatID.Location = new System.Drawing.Point(30, 42);
			this.labelPatID.Name = "labelPatID";
			this.labelPatID.Size = new System.Drawing.Size(138, 16);
			this.labelPatID.TabIndex = 143;
			this.labelPatID.Text = "Optional Patient ID";
			this.labelPatID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panelPat
			// 
			this.panelPat.Controls.Add(this.butPatOrtho);
			this.panelPat.Controls.Add(this.label30);
			this.panelPat.Controls.Add(this.textDateLastVerifiedPatPlan);
			this.panelPat.Controls.Add(this.butVerifyPatPlan);
			this.panelPat.Controls.Add(this.textPatPlanNum);
			this.panelPat.Controls.Add(this.label27);
			this.panelPat.Controls.Add(this.comboRelationship);
			this.panelPat.Controls.Add(this.label33);
			this.panelPat.Controls.Add(this.textOrdinal);
			this.panelPat.Controls.Add(this.butAdjAdd);
			this.panelPat.Controls.Add(this.listAdj);
			this.panelPat.Controls.Add(this.label35);
			this.panelPat.Controls.Add(this.textPatID);
			this.panelPat.Controls.Add(this.labelPatID);
			this.panelPat.Controls.Add(this.labelDrop);
			this.panelPat.Controls.Add(this.butDrop);
			this.panelPat.Controls.Add(this.label26);
			this.panelPat.Controls.Add(this.label31);
			this.panelPat.Controls.Add(this.checkIsPending);
			this.panelPat.Location = new System.Drawing.Point(0, 0);
			this.panelPat.Name = "panelPat";
			this.panelPat.Size = new System.Drawing.Size(982, 90);
			this.panelPat.TabIndex = 15;
			// 
			// butPatOrtho
			// 
			this.butPatOrtho.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPatOrtho.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butPatOrtho.Autosize = true;
			this.butPatOrtho.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPatOrtho.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPatOrtho.CornerRadius = 4F;
			this.butPatOrtho.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butPatOrtho.Location = new System.Drawing.Point(611, 61);
			this.butPatOrtho.Name = "butPatOrtho";
			this.butPatOrtho.Size = new System.Drawing.Size(62, 23);
			this.butPatOrtho.TabIndex = 149;
			this.butPatOrtho.Text = "Ortho";
			this.butPatOrtho.Click += new System.EventHandler(this.butPatOrtho_Click);
			// 
			// label30
			// 
			this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label30.Location = new System.Drawing.Point(329, 50);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(138, 17);
			this.label30.TabIndex = 148;
			this.label30.Text = "Eligibility Last Verified";
			this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDateLastVerifiedPatPlan
			// 
			this.textDateLastVerifiedPatPlan.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.textDateLastVerifiedPatPlan.Location = new System.Drawing.Point(468, 48);
			this.textDateLastVerifiedPatPlan.Name = "textDateLastVerifiedPatPlan";
			this.textDateLastVerifiedPatPlan.Size = new System.Drawing.Size(70, 20);
			this.textDateLastVerifiedPatPlan.TabIndex = 146;
			// 
			// butVerifyPatPlan
			// 
			this.butVerifyPatPlan.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butVerifyPatPlan.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.butVerifyPatPlan.Autosize = true;
			this.butVerifyPatPlan.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butVerifyPatPlan.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butVerifyPatPlan.CornerRadius = 4F;
			this.butVerifyPatPlan.Location = new System.Drawing.Point(539, 46);
			this.butVerifyPatPlan.Name = "butVerifyPatPlan";
			this.butVerifyPatPlan.Size = new System.Drawing.Size(32, 23);
			this.butVerifyPatPlan.TabIndex = 147;
			this.butVerifyPatPlan.Text = "Now";
			this.butVerifyPatPlan.UseVisualStyleBackColor = true;
			this.butVerifyPatPlan.Click += new System.EventHandler(this.butVerifyPatPlan_Click);
			// 
			// textPatPlanNum
			// 
			this.textPatPlanNum.BackColor = System.Drawing.SystemColors.Control;
			this.textPatPlanNum.Location = new System.Drawing.Point(468, 8);
			this.textPatPlanNum.Name = "textPatPlanNum";
			this.textPatPlanNum.ReadOnly = true;
			this.textPatPlanNum.Size = new System.Drawing.Size(144, 20);
			this.textPatPlanNum.TabIndex = 144;
			// 
			// label27
			// 
			this.label27.Location = new System.Drawing.Point(329, 10);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(138, 17);
			this.label27.TabIndex = 145;
			this.label27.Text = "Patient Plan ID";
			this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textOrdinal
			// 
			this.textOrdinal.Location = new System.Drawing.Point(468, 28);
			this.textOrdinal.MaxVal = 10;
			this.textOrdinal.MinVal = 1;
			this.textOrdinal.Name = "textOrdinal";
			this.textOrdinal.Size = new System.Drawing.Size(45, 20);
			this.textOrdinal.TabIndex = 2;
			// 
			// butAdjAdd
			// 
			this.butAdjAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdjAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdjAdd.Autosize = true;
			this.butAdjAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdjAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdjAdd.CornerRadius = 4F;
			this.butAdjAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.butAdjAdd.Location = new System.Drawing.Point(915, 7);
			this.butAdjAdd.Name = "butAdjAdd";
			this.butAdjAdd.Size = new System.Drawing.Size(59, 21);
			this.butAdjAdd.TabIndex = 4;
			this.butAdjAdd.Text = "Add";
			this.butAdjAdd.Click += new System.EventHandler(this.butAdjAdd_Click);
			// 
			// butDrop
			// 
			this.butDrop.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDrop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDrop.Autosize = true;
			this.butDrop.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDrop.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDrop.CornerRadius = 4F;
			this.butDrop.Location = new System.Drawing.Point(7, 67);
			this.butDrop.Name = "butDrop";
			this.butDrop.Size = new System.Drawing.Size(72, 21);
			this.butDrop.TabIndex = 5;
			this.butDrop.Text = "Drop";
			this.butDrop.Click += new System.EventHandler(this.butDrop_Click);
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(12, 563);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(272, 15);
			this.label18.TabIndex = 156;
			this.label18.Text = "Plan Note";
			this.label18.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// radioChangeAll
			// 
			this.radioChangeAll.Location = new System.Drawing.Point(6, 25);
			this.radioChangeAll.Name = "radioChangeAll";
			this.radioChangeAll.Size = new System.Drawing.Size(211, 17);
			this.radioChangeAll.TabIndex = 158;
			this.radioChangeAll.Text = "Change Plan for all subscribers";
			this.radioChangeAll.UseVisualStyleBackColor = true;
			// 
			// groupChanges
			// 
			this.groupChanges.Controls.Add(this.radioCreateNew);
			this.groupChanges.Controls.Add(this.radioChangeAll);
			this.groupChanges.Location = new System.Drawing.Point(467, 655);
			this.groupChanges.Name = "groupChanges";
			this.groupChanges.Size = new System.Drawing.Size(240, 44);
			this.groupChanges.TabIndex = 159;
			this.groupChanges.TabStop = false;
			// 
			// radioCreateNew
			// 
			this.radioCreateNew.Checked = true;
			this.radioCreateNew.Location = new System.Drawing.Point(6, 8);
			this.radioCreateNew.Name = "radioCreateNew";
			this.radioCreateNew.Size = new System.Drawing.Size(211, 17);
			this.radioCreateNew.TabIndex = 159;
			this.radioCreateNew.TabStop = true;
			this.radioCreateNew.Text = "Create new Plan if needed";
			this.radioCreateNew.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.butImportTrojan);
			this.groupBox1.Controls.Add(this.butIapFind);
			this.groupBox1.Controls.Add(this.butBenefitNotes);
			this.groupBox1.Controls.Add(this.textTrojanID);
			this.groupBox1.Controls.Add(this.labelTrojanID);
			this.groupBox1.Location = new System.Drawing.Point(637, 269);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(333, 40);
			this.groupBox1.TabIndex = 160;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Import Benefits";
			// 
			// label34
			// 
			this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label34.Location = new System.Drawing.Point(634, 314);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(126, 17);
			this.label34.TabIndex = 149;
			this.label34.Text = "Benefits Last Verified";
			this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkDontVerify
			// 
			this.checkDontVerify.AutoSize = true;
			this.checkDontVerify.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkDontVerify.Location = new System.Drawing.Point(888, 314);
			this.checkDontVerify.Name = "checkDontVerify";
			this.checkDontVerify.Size = new System.Drawing.Size(80, 17);
			this.checkDontVerify.TabIndex = 161;
			this.checkDontVerify.Text = "Don\'t Verify";
			this.checkDontVerify.UseVisualStyleBackColor = true;
			// 
			// butVerifyBenefits
			// 
			this.butVerifyBenefits.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butVerifyBenefits.Autosize = true;
			this.butVerifyBenefits.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butVerifyBenefits.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butVerifyBenefits.CornerRadius = 4F;
			this.butVerifyBenefits.Location = new System.Drawing.Point(833, 310);
			this.butVerifyBenefits.Name = "butVerifyBenefits";
			this.butVerifyBenefits.Size = new System.Drawing.Size(32, 23);
			this.butVerifyBenefits.TabIndex = 150;
			this.butVerifyBenefits.Text = "Now";
			this.butVerifyBenefits.UseVisualStyleBackColor = true;
			this.butVerifyBenefits.Click += new System.EventHandler(this.butVerifyBenefits_Click);
			// 
			// textDateLastVerifiedBenefits
			// 
			this.textDateLastVerifiedBenefits.Location = new System.Drawing.Point(762, 312);
			this.textDateLastVerifiedBenefits.Name = "textDateLastVerifiedBenefits";
			this.textDateLastVerifiedBenefits.Size = new System.Drawing.Size(70, 20);
			this.textDateLastVerifiedBenefits.TabIndex = 149;
			// 
			// butAudit
			// 
			this.butAudit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAudit.Autosize = true;
			this.butAudit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAudit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAudit.CornerRadius = 4F;
			this.butAudit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAudit.Location = new System.Drawing.Point(230, 4);
			this.butAudit.Name = "butAudit";
			this.butAudit.Size = new System.Drawing.Size(69, 23);
			this.butAudit.TabIndex = 153;
			this.butAudit.Text = "Audit Trail";
			this.butAudit.Click += new System.EventHandler(this.butAudit_Click);
			// 
			// butPick
			// 
			this.butPick.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPick.Autosize = true;
			this.butPick.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPick.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPick.CornerRadius = 4F;
			this.butPick.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPick.Location = new System.Drawing.Point(327, 4);
			this.butPick.Name = "butPick";
			this.butPick.Size = new System.Drawing.Size(90, 23);
			this.butPick.TabIndex = 153;
			this.butPick.Text = "Pick From List";
			this.butPick.Click += new System.EventHandler(this.butPick_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(810, 673);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butLabel
			// 
			this.butLabel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butLabel.Autosize = true;
			this.butLabel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLabel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLabel.CornerRadius = 4F;
			this.butLabel.Image = global::OpenDental.Properties.Resources.butLabel;
			this.butLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butLabel.Location = new System.Drawing.Point(201, 673);
			this.butLabel.Name = "butLabel";
			this.butLabel.Size = new System.Drawing.Size(81, 24);
			this.butLabel.TabIndex = 125;
			this.butLabel.Text = "Label";
			this.butLabel.Click += new System.EventHandler(this.butLabel_Click);
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
			this.butDelete.Location = new System.Drawing.Point(13, 673);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(81, 24);
			this.butDelete.TabIndex = 112;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
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
			this.butCancel.Location = new System.Drawing.Point(896, 673);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 14;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// tabControlInsPlan
			// 
			this.tabControlInsPlan.Controls.Add(this.tabPageInsPlanInfo);
			this.tabControlInsPlan.Controls.Add(this.tabPageOtherInsInfo);
			this.tabControlInsPlan.Controls.Add(this.tabPageCanadian);
			this.tabControlInsPlan.Controls.Add(this.tabPageOrtho);
			this.tabControlInsPlan.Location = new System.Drawing.Point(7, 96);
			this.tabControlInsPlan.Name = "tabControlInsPlan";
			this.tabControlInsPlan.SelectedIndex = 0;
			this.tabControlInsPlan.Size = new System.Drawing.Size(455, 466);
			this.tabControlInsPlan.TabIndex = 122;
			// 
			// tabPageInsPlanInfo
			// 
			this.tabPageInsPlanInfo.Controls.Add(this.panelPlan);
			this.tabPageInsPlanInfo.Controls.Add(this.butAudit);
			this.tabPageInsPlanInfo.Controls.Add(this.label32);
			this.tabPageInsPlanInfo.Controls.Add(this.butPick);
			this.tabPageInsPlanInfo.Location = new System.Drawing.Point(4, 22);
			this.tabPageInsPlanInfo.Name = "tabPageInsPlanInfo";
			this.tabPageInsPlanInfo.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageInsPlanInfo.Size = new System.Drawing.Size(447, 440);
			this.tabPageInsPlanInfo.TabIndex = 0;
			this.tabPageInsPlanInfo.Text = "Plan Info";
			this.tabPageInsPlanInfo.UseVisualStyleBackColor = true;
			// 
			// panelPlan
			// 
			this.panelPlan.AutoScroll = true;
			this.panelPlan.AutoScrollMargin = new System.Drawing.Size(0, 10);
			this.panelPlan.BackColor = System.Drawing.SystemColors.Control;
			this.panelPlan.Controls.Add(this.comboFeeSched);
			this.panelPlan.Controls.Add(this.groupCoPay);
			this.panelPlan.Controls.Add(this.label1);
			this.panelPlan.Controls.Add(this.textInsPlanNum);
			this.panelPlan.Controls.Add(this.label29);
			this.panelPlan.Controls.Add(this.groupPlan);
			this.panelPlan.Controls.Add(this.comboPlanType);
			this.panelPlan.Controls.Add(this.label14);
			this.panelPlan.Location = new System.Drawing.Point(-3, 33);
			this.panelPlan.Name = "panelPlan";
			this.panelPlan.Size = new System.Drawing.Size(454, 406);
			this.panelPlan.TabIndex = 154;
			// 
			// comboFeeSched
			// 
			this.comboFeeSched.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFeeSched.Location = new System.Drawing.Point(121, 328);
			this.comboFeeSched.MaxDropDownItems = 30;
			this.comboFeeSched.Name = "comboFeeSched";
			this.comboFeeSched.Size = new System.Drawing.Size(212, 21);
			this.comboFeeSched.TabIndex = 180;
			// 
			// groupCoPay
			// 
			this.groupCoPay.Controls.Add(this.label12);
			this.groupCoPay.Controls.Add(this.comboAllowedFeeSched);
			this.groupCoPay.Controls.Add(this.labelCopayFeeSched);
			this.groupCoPay.Controls.Add(this.label3);
			this.groupCoPay.Controls.Add(this.comboCopay);
			this.groupCoPay.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupCoPay.Location = new System.Drawing.Point(14, 352);
			this.groupCoPay.Name = "groupCoPay";
			this.groupCoPay.Size = new System.Drawing.Size(404, 87);
			this.groupCoPay.TabIndex = 181;
			this.groupCoPay.TabStop = false;
			this.groupCoPay.Text = "Other Fee Schedules";
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(6, 58);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(138, 20);
			this.label12.TabIndex = 111;
			this.label12.Text = "Carrier Allowed Amounts";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboAllowedFeeSched
			// 
			this.comboAllowedFeeSched.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAllowedFeeSched.Location = new System.Drawing.Point(145, 59);
			this.comboAllowedFeeSched.MaxDropDownItems = 30;
			this.comboAllowedFeeSched.Name = "comboAllowedFeeSched";
			this.comboAllowedFeeSched.Size = new System.Drawing.Size(212, 21);
			this.comboAllowedFeeSched.TabIndex = 1;
			// 
			// labelCopayFeeSched
			// 
			this.labelCopayFeeSched.Location = new System.Drawing.Point(6, 36);
			this.labelCopayFeeSched.Name = "labelCopayFeeSched";
			this.labelCopayFeeSched.Size = new System.Drawing.Size(138, 20);
			this.labelCopayFeeSched.TabIndex = 109;
			this.labelCopayFeeSched.Text = "Patient Co-pay Amounts";
			this.labelCopayFeeSched.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(1, 19);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(390, 17);
			this.label3.TabIndex = 106;
			this.label3.Text = "Don\'t use these unless you understand how they will affect your estimates";
			// 
			// comboCopay
			// 
			this.comboCopay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCopay.Location = new System.Drawing.Point(145, 36);
			this.comboCopay.MaxDropDownItems = 30;
			this.comboCopay.Name = "comboCopay";
			this.comboCopay.Size = new System.Drawing.Size(212, 21);
			this.comboCopay.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 329);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 16);
			this.label1.TabIndex = 182;
			this.label1.Text = "Fee Schedule";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsPlanNum
			// 
			this.textInsPlanNum.BackColor = System.Drawing.SystemColors.Control;
			this.textInsPlanNum.Location = new System.Drawing.Point(121, 4);
			this.textInsPlanNum.Name = "textInsPlanNum";
			this.textInsPlanNum.ReadOnly = true;
			this.textInsPlanNum.Size = new System.Drawing.Size(144, 20);
			this.textInsPlanNum.TabIndex = 151;
			// 
			// label29
			// 
			this.label29.Location = new System.Drawing.Point(11, 4);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(109, 17);
			this.label29.TabIndex = 152;
			this.label29.Text = "Insurance Plan ID";
			this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupPlan
			// 
			this.groupPlan.Controls.Add(this.butOtherSubscribers);
			this.groupPlan.Controls.Add(this.textBIN);
			this.groupPlan.Controls.Add(this.labelBIN);
			this.groupPlan.Controls.Add(this.textDivisionNo);
			this.groupPlan.Controls.Add(this.textGroupName);
			this.groupPlan.Controls.Add(this.textEmployer);
			this.groupPlan.Controls.Add(this.groupCarrier);
			this.groupPlan.Controls.Add(this.checkIsMedical);
			this.groupPlan.Controls.Add(this.textGroupNum);
			this.groupPlan.Controls.Add(this.labelGroupNum);
			this.groupPlan.Controls.Add(this.label8);
			this.groupPlan.Controls.Add(this.comboLinked);
			this.groupPlan.Controls.Add(this.textLinkedNum);
			this.groupPlan.Controls.Add(this.label16);
			this.groupPlan.Controls.Add(this.label4);
			this.groupPlan.Controls.Add(this.labelDivisionDash);
			this.groupPlan.Location = new System.Drawing.Point(9, 21);
			this.groupPlan.Name = "groupPlan";
			this.groupPlan.Size = new System.Drawing.Size(425, 281);
			this.groupPlan.TabIndex = 148;
			this.groupPlan.TabStop = false;
			// 
			// butOtherSubscribers
			// 
			this.butOtherSubscribers.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOtherSubscribers.Autosize = true;
			this.butOtherSubscribers.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOtherSubscribers.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOtherSubscribers.CornerRadius = 4F;
			this.butOtherSubscribers.Location = new System.Drawing.Point(399, 254);
			this.butOtherSubscribers.Name = "butOtherSubscribers";
			this.butOtherSubscribers.Size = new System.Drawing.Size(108, 20);
			this.butOtherSubscribers.TabIndex = 156;
			this.butOtherSubscribers.Text = "List Subscribers";
			this.butOtherSubscribers.Visible = false;
			this.butOtherSubscribers.Click += new System.EventHandler(this.butOtherSubscribers_Click);
			// 
			// textBIN
			// 
			this.textBIN.Location = new System.Drawing.Point(341, 212);
			this.textBIN.MaxLength = 20;
			this.textBIN.Name = "textBIN";
			this.textBIN.Size = new System.Drawing.Size(62, 20);
			this.textBIN.TabIndex = 115;
			// 
			// labelBIN
			// 
			this.labelBIN.Location = new System.Drawing.Point(307, 213);
			this.labelBIN.Name = "labelBIN";
			this.labelBIN.Size = new System.Drawing.Size(32, 16);
			this.labelBIN.TabIndex = 114;
			this.labelBIN.Text = "BIN";
			this.labelBIN.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDivisionNo
			// 
			this.textDivisionNo.Location = new System.Drawing.Point(330, 233);
			this.textDivisionNo.MaxLength = 20;
			this.textDivisionNo.Name = "textDivisionNo";
			this.textDivisionNo.Size = new System.Drawing.Size(73, 20);
			this.textDivisionNo.TabIndex = 3;
			// 
			// textGroupName
			// 
			this.textGroupName.Location = new System.Drawing.Point(112, 212);
			this.textGroupName.MaxLength = 50;
			this.textGroupName.Name = "textGroupName";
			this.textGroupName.Size = new System.Drawing.Size(193, 20);
			this.textGroupName.TabIndex = 2;
			// 
			// textEmployer
			// 
			this.textEmployer.Location = new System.Drawing.Point(112, 27);
			this.textEmployer.MaxLength = 40;
			this.textEmployer.Name = "textEmployer";
			this.textEmployer.Size = new System.Drawing.Size(291, 20);
			this.textEmployer.TabIndex = 0;
			this.textEmployer.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textEmployer_KeyUp);
			this.textEmployer.Leave += new System.EventHandler(this.textEmployer_Leave);
			// 
			// groupCarrier
			// 
			this.groupCarrier.Controls.Add(this.butPickCarrier);
			this.groupCarrier.Controls.Add(this.textPhone);
			this.groupCarrier.Controls.Add(this.textAddress);
			this.groupCarrier.Controls.Add(this.comboElectIDdescript);
			this.groupCarrier.Controls.Add(this.textElectID);
			this.groupCarrier.Controls.Add(this.butSearch);
			this.groupCarrier.Controls.Add(this.textAddress2);
			this.groupCarrier.Controls.Add(this.textZip);
			this.groupCarrier.Controls.Add(this.checkNoSendElect);
			this.groupCarrier.Controls.Add(this.label10);
			this.groupCarrier.Controls.Add(this.textCity);
			this.groupCarrier.Controls.Add(this.label7);
			this.groupCarrier.Controls.Add(this.textCarrier);
			this.groupCarrier.Controls.Add(this.labelElectronicID);
			this.groupCarrier.Controls.Add(this.label21);
			this.groupCarrier.Controls.Add(this.label17);
			this.groupCarrier.Controls.Add(this.textState);
			this.groupCarrier.Controls.Add(this.labelCitySTZip);
			this.groupCarrier.Location = new System.Drawing.Point(10, 47);
			this.groupCarrier.Name = "groupCarrier";
			this.groupCarrier.Size = new System.Drawing.Size(402, 163);
			this.groupCarrier.TabIndex = 1;
			this.groupCarrier.TabStop = false;
			this.groupCarrier.Text = "Carrier";
			// 
			// butPickCarrier
			// 
			this.butPickCarrier.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickCarrier.Autosize = true;
			this.butPickCarrier.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickCarrier.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickCarrier.CornerRadius = 3F;
			this.butPickCarrier.Location = new System.Drawing.Point(376, 11);
			this.butPickCarrier.Name = "butPickCarrier";
			this.butPickCarrier.Size = new System.Drawing.Size(19, 20);
			this.butPickCarrier.TabIndex = 153;
			this.butPickCarrier.Text = "...";
			this.butPickCarrier.Click += new System.EventHandler(this.butPickCarrier_Click);
			// 
			// textPhone
			// 
			this.textPhone.Location = new System.Drawing.Point(102, 33);
			this.textPhone.MaxLength = 30;
			this.textPhone.Name = "textPhone";
			this.textPhone.Size = new System.Drawing.Size(157, 20);
			this.textPhone.TabIndex = 1;
			this.textPhone.TextChanged += new System.EventHandler(this.textPhone_TextChanged);
			// 
			// textAddress
			// 
			this.textAddress.Location = new System.Drawing.Point(102, 54);
			this.textAddress.MaxLength = 60;
			this.textAddress.Name = "textAddress";
			this.textAddress.Size = new System.Drawing.Size(291, 20);
			this.textAddress.TabIndex = 2;
			this.textAddress.TextChanged += new System.EventHandler(this.textAddress_TextChanged);
			// 
			// comboElectIDdescript
			// 
			this.comboElectIDdescript.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboElectIDdescript.Location = new System.Drawing.Point(156, 117);
			this.comboElectIDdescript.MaxDropDownItems = 30;
			this.comboElectIDdescript.Name = "comboElectIDdescript";
			this.comboElectIDdescript.Size = new System.Drawing.Size(237, 21);
			this.comboElectIDdescript.TabIndex = 125;
			this.comboElectIDdescript.SelectedIndexChanged += new System.EventHandler(this.comboElectIDdescript_SelectedIndexChanged);
			// 
			// textElectID
			// 
			this.textElectID.Location = new System.Drawing.Point(102, 117);
			this.textElectID.MaxLength = 20;
			this.textElectID.Name = "textElectID";
			this.textElectID.Size = new System.Drawing.Size(54, 20);
			this.textElectID.TabIndex = 7;
			this.textElectID.Validating += new System.ComponentModel.CancelEventHandler(this.textElectID_Validating);
			// 
			// butSearch
			// 
			this.butSearch.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSearch.Autosize = true;
			this.butSearch.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSearch.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSearch.CornerRadius = 4F;
			this.butSearch.Location = new System.Drawing.Point(86, 139);
			this.butSearch.Name = "butSearch";
			this.butSearch.Size = new System.Drawing.Size(84, 20);
			this.butSearch.TabIndex = 124;
			this.butSearch.Text = "Search IDs";
			this.butSearch.Click += new System.EventHandler(this.butSearch_Click);
			// 
			// textAddress2
			// 
			this.textAddress2.Location = new System.Drawing.Point(102, 75);
			this.textAddress2.MaxLength = 60;
			this.textAddress2.Name = "textAddress2";
			this.textAddress2.Size = new System.Drawing.Size(291, 20);
			this.textAddress2.TabIndex = 3;
			this.textAddress2.TextChanged += new System.EventHandler(this.textAddress2_TextChanged);
			// 
			// textZip
			// 
			this.textZip.Location = new System.Drawing.Point(322, 96);
			this.textZip.MaxLength = 10;
			this.textZip.Name = "textZip";
			this.textZip.Size = new System.Drawing.Size(71, 20);
			this.textZip.TabIndex = 6;
			// 
			// checkNoSendElect
			// 
			this.checkNoSendElect.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkNoSendElect.Location = new System.Drawing.Point(178, 140);
			this.checkNoSendElect.Name = "checkNoSendElect";
			this.checkNoSendElect.Size = new System.Drawing.Size(213, 17);
			this.checkNoSendElect.TabIndex = 8;
			this.checkNoSendElect.Text = "Don\'t Usually Send Electronically";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(5, 56);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(95, 15);
			this.label10.TabIndex = 10;
			this.label10.Text = "Address";
			this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textCity
			// 
			this.textCity.Location = new System.Drawing.Point(102, 96);
			this.textCity.MaxLength = 40;
			this.textCity.Name = "textCity";
			this.textCity.Size = new System.Drawing.Size(153, 20);
			this.textCity.TabIndex = 4;
			this.textCity.TextChanged += new System.EventHandler(this.textCity_TextChanged);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(5, 36);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(95, 15);
			this.label7.TabIndex = 7;
			this.label7.Text = "Phone";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textCarrier
			// 
			this.textCarrier.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textCarrier.Location = new System.Drawing.Point(102, 11);
			this.textCarrier.MaxLength = 50;
			this.textCarrier.Name = "textCarrier";
			this.textCarrier.Size = new System.Drawing.Size(273, 21);
			this.textCarrier.TabIndex = 0;
			this.textCarrier.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textCarrier_KeyUp);
			this.textCarrier.Leave += new System.EventHandler(this.textCarrier_Leave);
			// 
			// labelElectronicID
			// 
			this.labelElectronicID.Location = new System.Drawing.Point(4, 119);
			this.labelElectronicID.Name = "labelElectronicID";
			this.labelElectronicID.Size = new System.Drawing.Size(95, 15);
			this.labelElectronicID.TabIndex = 15;
			this.labelElectronicID.Text = "Electronic ID";
			this.labelElectronicID.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(5, 78);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(95, 15);
			this.label21.TabIndex = 79;
			this.label21.Text = "Address 2";
			this.label21.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(7, 13);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(94, 15);
			this.label17.TabIndex = 152;
			this.label17.Text = "Carrier";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textState
			// 
			this.textState.Location = new System.Drawing.Point(256, 96);
			this.textState.MaxLength = 2;
			this.textState.Name = "textState";
			this.textState.Size = new System.Drawing.Size(65, 20);
			this.textState.TabIndex = 5;
			this.textState.TextChanged += new System.EventHandler(this.textState_TextChanged);
			// 
			// labelCitySTZip
			// 
			this.labelCitySTZip.Location = new System.Drawing.Point(5, 98);
			this.labelCitySTZip.Name = "labelCitySTZip";
			this.labelCitySTZip.Size = new System.Drawing.Size(95, 15);
			this.labelCitySTZip.TabIndex = 11;
			this.labelCitySTZip.Text = "City,ST,Zip";
			this.labelCitySTZip.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkIsMedical
			// 
			this.checkIsMedical.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsMedical.Location = new System.Drawing.Point(112, 9);
			this.checkIsMedical.Name = "checkIsMedical";
			this.checkIsMedical.Size = new System.Drawing.Size(202, 17);
			this.checkIsMedical.TabIndex = 113;
			this.checkIsMedical.Text = "Medical Insurance";
			// 
			// textGroupNum
			// 
			this.textGroupNum.Location = new System.Drawing.Point(112, 233);
			this.textGroupNum.MaxLength = 25;
			this.textGroupNum.Name = "textGroupNum";
			this.textGroupNum.Size = new System.Drawing.Size(165, 20);
			this.textGroupNum.TabIndex = 3;
			// 
			// labelGroupNum
			// 
			this.labelGroupNum.Location = new System.Drawing.Point(16, 236);
			this.labelGroupNum.Name = "labelGroupNum";
			this.labelGroupNum.Size = new System.Drawing.Size(95, 15);
			this.labelGroupNum.TabIndex = 9;
			this.labelGroupNum.Text = "Group Num";
			this.labelGroupNum.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(16, 215);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(95, 15);
			this.label8.TabIndex = 8;
			this.label8.Text = "Group Name";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboLinked
			// 
			this.comboLinked.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboLinked.Location = new System.Drawing.Point(150, 254);
			this.comboLinked.MaxDropDownItems = 30;
			this.comboLinked.Name = "comboLinked";
			this.comboLinked.Size = new System.Drawing.Size(253, 21);
			this.comboLinked.TabIndex = 68;
			// 
			// textLinkedNum
			// 
			this.textLinkedNum.BackColor = System.Drawing.Color.White;
			this.textLinkedNum.Location = new System.Drawing.Point(112, 254);
			this.textLinkedNum.Multiline = true;
			this.textLinkedNum.Name = "textLinkedNum";
			this.textLinkedNum.ReadOnly = true;
			this.textLinkedNum.Size = new System.Drawing.Size(37, 21);
			this.textLinkedNum.TabIndex = 67;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(33, 29);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(78, 15);
			this.label16.TabIndex = 73;
			this.label16.Text = "Employer";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(6, 256);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(104, 17);
			this.label4.TabIndex = 66;
			this.label4.Text = "Other Subscribers";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelDivisionDash
			// 
			this.labelDivisionDash.Location = new System.Drawing.Point(278, 236);
			this.labelDivisionDash.Name = "labelDivisionDash";
			this.labelDivisionDash.Size = new System.Drawing.Size(53, 15);
			this.labelDivisionDash.TabIndex = 111;
			this.labelDivisionDash.Text = "Div. No.";
			this.labelDivisionDash.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboPlanType
			// 
			this.comboPlanType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPlanType.Location = new System.Drawing.Point(121, 305);
			this.comboPlanType.Name = "comboPlanType";
			this.comboPlanType.Size = new System.Drawing.Size(212, 21);
			this.comboPlanType.TabIndex = 149;
			this.comboPlanType.SelectionChangeCommitted += new System.EventHandler(this.comboPlanType_SelectionChangeCommitted);
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(26, 306);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(95, 16);
			this.label14.TabIndex = 150;
			this.label14.Text = "Plan Type";
			this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tabPageOtherInsInfo
			// 
			this.tabPageOtherInsInfo.Controls.Add(this.panelOrthInfo);
			this.tabPageOtherInsInfo.Location = new System.Drawing.Point(4, 22);
			this.tabPageOtherInsInfo.Name = "tabPageOtherInsInfo";
			this.tabPageOtherInsInfo.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageOtherInsInfo.Size = new System.Drawing.Size(447, 440);
			this.tabPageOtherInsInfo.TabIndex = 1;
			this.tabPageOtherInsInfo.Text = "Other Ins Info";
			this.tabPageOtherInsInfo.UseVisualStyleBackColor = true;
			// 
			// panelOrthInfo
			// 
			this.panelOrthInfo.AutoScroll = true;
			this.panelOrthInfo.AutoScrollMargin = new System.Drawing.Size(0, 10);
			this.panelOrthInfo.BackColor = System.Drawing.SystemColors.Control;
			this.panelOrthInfo.Controls.Add(this.butSubstCodes);
			this.panelOrthInfo.Controls.Add(this.comboBillType);
			this.panelOrthInfo.Controls.Add(this.label38);
			this.panelOrthInfo.Controls.Add(this.comboCobRule);
			this.panelOrthInfo.Controls.Add(this.label20);
			this.panelOrthInfo.Controls.Add(this.comboFilingCodeSubtype);
			this.panelOrthInfo.Controls.Add(this.label15);
			this.panelOrthInfo.Controls.Add(this.checkIsHidden);
			this.panelOrthInfo.Controls.Add(this.checkCodeSubst);
			this.panelOrthInfo.Controls.Add(this.checkShowBaseUnits);
			this.panelOrthInfo.Controls.Add(this.comboFilingCode);
			this.panelOrthInfo.Controls.Add(this.label13);
			this.panelOrthInfo.Controls.Add(this.comboClaimForm);
			this.panelOrthInfo.Controls.Add(this.label23);
			this.panelOrthInfo.Controls.Add(this.checkAlternateCode);
			this.panelOrthInfo.Controls.Add(this.checkClaimsUseUCR);
			this.panelOrthInfo.Location = new System.Drawing.Point(-3, 2);
			this.panelOrthInfo.Name = "panelOrthInfo";
			this.panelOrthInfo.Size = new System.Drawing.Size(454, 438);
			this.panelOrthInfo.TabIndex = 1;
			// 
			// comboBillType
			// 
			this.comboBillType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBillType.Location = new System.Drawing.Point(153, 194);
			this.comboBillType.MaxDropDownItems = 30;
			this.comboBillType.Name = "comboBillType";
			this.comboBillType.Size = new System.Drawing.Size(212, 21);
			this.comboBillType.TabIndex = 185;
			this.comboBillType.SelectionChangeCommitted += new System.EventHandler(this.comboBillType_SelectionChangeCommitted);
			// 
			// label38
			// 
			this.label38.Location = new System.Drawing.Point(9, 196);
			this.label38.Name = "label38";
			this.label38.Size = new System.Drawing.Size(142, 19);
			this.label38.TabIndex = 186;
			this.label38.Text = "Billing Type";
			this.label38.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboCobRule
			// 
			this.comboCobRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCobRule.Location = new System.Drawing.Point(153, 122);
			this.comboCobRule.MaxDropDownItems = 30;
			this.comboCobRule.Name = "comboCobRule";
			this.comboCobRule.Size = new System.Drawing.Size(111, 21);
			this.comboCobRule.TabIndex = 183;
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(55, 125);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(95, 15);
			this.label20.TabIndex = 184;
			this.label20.Text = "COB Rule";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboFilingCodeSubtype
			// 
			this.comboFilingCodeSubtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFilingCodeSubtype.Location = new System.Drawing.Point(153, 170);
			this.comboFilingCodeSubtype.MaxDropDownItems = 30;
			this.comboFilingCodeSubtype.Name = "comboFilingCodeSubtype";
			this.comboFilingCodeSubtype.Size = new System.Drawing.Size(212, 21);
			this.comboFilingCodeSubtype.TabIndex = 177;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(41, 172);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(110, 19);
			this.label15.TabIndex = 182;
			this.label15.Text = "Filing Code Subtype";
			this.label15.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkIsHidden
			// 
			this.checkIsHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsHidden.Location = new System.Drawing.Point(153, 62);
			this.checkIsHidden.Name = "checkIsHidden";
			this.checkIsHidden.Size = new System.Drawing.Size(275, 16);
			this.checkIsHidden.TabIndex = 172;
			this.checkIsHidden.Text = "Hidden";
			// 
			// checkCodeSubst
			// 
			this.checkCodeSubst.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkCodeSubst.Location = new System.Drawing.Point(153, 25);
			this.checkCodeSubst.Name = "checkCodeSubst";
			this.checkCodeSubst.Size = new System.Drawing.Size(275, 16);
			this.checkCodeSubst.TabIndex = 170;
			this.checkCodeSubst.Text = "Don\'t Substitute Codes (e.g. posterior composites)";
			// 
			// checkShowBaseUnits
			// 
			this.checkShowBaseUnits.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowBaseUnits.Location = new System.Drawing.Point(153, 80);
			this.checkShowBaseUnits.Name = "checkShowBaseUnits";
			this.checkShowBaseUnits.Size = new System.Drawing.Size(289, 16);
			this.checkShowBaseUnits.TabIndex = 178;
			this.checkShowBaseUnits.Text = "Claims show base units (Does not affect billed amount)";
			this.checkShowBaseUnits.UseVisualStyleBackColor = true;
			// 
			// comboFilingCode
			// 
			this.comboFilingCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFilingCode.Location = new System.Drawing.Point(153, 146);
			this.comboFilingCode.MaxDropDownItems = 30;
			this.comboFilingCode.Name = "comboFilingCode";
			this.comboFilingCode.Size = new System.Drawing.Size(212, 21);
			this.comboFilingCode.TabIndex = 176;
			this.comboFilingCode.SelectionChangeCommitted += new System.EventHandler(this.comboFilingCode_SelectionChangeCommitted);
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(51, 148);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(100, 19);
			this.label13.TabIndex = 181;
			this.label13.Text = "Filing Code";
			this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboClaimForm
			// 
			this.comboClaimForm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClaimForm.Location = new System.Drawing.Point(153, 98);
			this.comboClaimForm.MaxDropDownItems = 30;
			this.comboClaimForm.Name = "comboClaimForm";
			this.comboClaimForm.Size = new System.Drawing.Size(212, 21);
			this.comboClaimForm.TabIndex = 174;
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(55, 101);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(95, 15);
			this.label23.TabIndex = 180;
			this.label23.Text = "Claim Form";
			this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkAlternateCode
			// 
			this.checkAlternateCode.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAlternateCode.Location = new System.Drawing.Point(153, 7);
			this.checkAlternateCode.Name = "checkAlternateCode";
			this.checkAlternateCode.Size = new System.Drawing.Size(275, 16);
			this.checkAlternateCode.TabIndex = 169;
			this.checkAlternateCode.Text = "Use Alternate Code (for some Medicaid plans)";
			// 
			// checkClaimsUseUCR
			// 
			this.checkClaimsUseUCR.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClaimsUseUCR.Location = new System.Drawing.Point(153, 44);
			this.checkClaimsUseUCR.Name = "checkClaimsUseUCR";
			this.checkClaimsUseUCR.Size = new System.Drawing.Size(275, 16);
			this.checkClaimsUseUCR.TabIndex = 171;
			this.checkClaimsUseUCR.Text = "Claims show UCR fee, not billed fee";
			// 
			// tabPageCanadian
			// 
			this.tabPageCanadian.Controls.Add(this.panelCanadian);
			this.tabPageCanadian.Location = new System.Drawing.Point(4, 22);
			this.tabPageCanadian.Name = "tabPageCanadian";
			this.tabPageCanadian.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageCanadian.Size = new System.Drawing.Size(447, 440);
			this.tabPageCanadian.TabIndex = 2;
			this.tabPageCanadian.Text = "Canadian";
			this.tabPageCanadian.UseVisualStyleBackColor = true;
			// 
			// panelCanadian
			// 
			this.panelCanadian.AutoScroll = true;
			this.panelCanadian.AutoScrollMargin = new System.Drawing.Size(0, 10);
			this.panelCanadian.BackColor = System.Drawing.SystemColors.Control;
			this.panelCanadian.Controls.Add(this.groupCanadian);
			this.panelCanadian.Location = new System.Drawing.Point(-3, 1);
			this.panelCanadian.Name = "panelCanadian";
			this.panelCanadian.Size = new System.Drawing.Size(454, 438);
			this.panelCanadian.TabIndex = 2;
			// 
			// groupCanadian
			// 
			this.groupCanadian.Controls.Add(this.label19);
			this.groupCanadian.Controls.Add(this.textCanadianInstCode);
			this.groupCanadian.Controls.Add(this.label9);
			this.groupCanadian.Controls.Add(this.textCanadianDiagCode);
			this.groupCanadian.Controls.Add(this.checkIsPMP);
			this.groupCanadian.Controls.Add(this.label24);
			this.groupCanadian.Controls.Add(this.label22);
			this.groupCanadian.Controls.Add(this.textPlanFlag);
			this.groupCanadian.Controls.Add(this.textDentaide);
			this.groupCanadian.Controls.Add(this.labelDentaide);
			this.groupCanadian.Location = new System.Drawing.Point(14, 14);
			this.groupCanadian.Name = "groupCanadian";
			this.groupCanadian.Size = new System.Drawing.Size(404, 129);
			this.groupCanadian.TabIndex = 13;
			this.groupCanadian.TabStop = false;
			this.groupCanadian.Text = "Canadian";
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(37, 106);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(140, 19);
			this.label19.TabIndex = 173;
			this.label19.Text = "Institution Code";
			this.label19.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textCanadianInstCode
			// 
			this.textCanadianInstCode.Location = new System.Drawing.Point(181, 103);
			this.textCanadianInstCode.MaxLength = 20;
			this.textCanadianInstCode.Name = "textCanadianInstCode";
			this.textCanadianInstCode.Size = new System.Drawing.Size(88, 20);
			this.textCanadianInstCode.TabIndex = 172;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(37, 85);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(140, 19);
			this.label9.TabIndex = 171;
			this.label9.Text = "Diagnostic Code";
			this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textCanadianDiagCode
			// 
			this.textCanadianDiagCode.Location = new System.Drawing.Point(181, 82);
			this.textCanadianDiagCode.MaxLength = 20;
			this.textCanadianDiagCode.Name = "textCanadianDiagCode";
			this.textCanadianDiagCode.Size = new System.Drawing.Size(88, 20);
			this.textCanadianDiagCode.TabIndex = 170;
			// 
			// checkIsPMP
			// 
			this.checkIsPMP.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsPMP.Location = new System.Drawing.Point(18, 62);
			this.checkIsPMP.Name = "checkIsPMP";
			this.checkIsPMP.Size = new System.Drawing.Size(178, 17);
			this.checkIsPMP.TabIndex = 169;
			this.checkIsPMP.Text = "Is Provincial Medical Plan";
			this.checkIsPMP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsPMP.UseVisualStyleBackColor = true;
			// 
			// label24
			// 
			this.label24.Location = new System.Drawing.Point(221, 39);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(140, 19);
			this.label24.TabIndex = 168;
			this.label24.Text = "A, V, N, or blank";
			this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(37, 41);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(140, 19);
			this.label22.TabIndex = 167;
			this.label22.Text = "Plan Flag";
			this.label22.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textPlanFlag
			// 
			this.textPlanFlag.Location = new System.Drawing.Point(181, 38);
			this.textPlanFlag.MaxLength = 20;
			this.textPlanFlag.Name = "textPlanFlag";
			this.textPlanFlag.Size = new System.Drawing.Size(37, 20);
			this.textPlanFlag.TabIndex = 1;
			// 
			// textDentaide
			// 
			this.textDentaide.Location = new System.Drawing.Point(181, 17);
			this.textDentaide.MaxVal = 255;
			this.textDentaide.MinVal = 0;
			this.textDentaide.Name = "textDentaide";
			this.textDentaide.Size = new System.Drawing.Size(37, 20);
			this.textDentaide.TabIndex = 0;
			// 
			// labelDentaide
			// 
			this.labelDentaide.Location = new System.Drawing.Point(37, 20);
			this.labelDentaide.Name = "labelDentaide";
			this.labelDentaide.Size = new System.Drawing.Size(140, 19);
			this.labelDentaide.TabIndex = 160;
			this.labelDentaide.Text = "Dentaide Card Sequence";
			this.labelDentaide.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tabPageOrtho
			// 
			this.tabPageOrtho.Controls.Add(this.panelOrtho);
			this.tabPageOrtho.Location = new System.Drawing.Point(4, 22);
			this.tabPageOrtho.Name = "tabPageOrtho";
			this.tabPageOrtho.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageOrtho.Size = new System.Drawing.Size(447, 440);
			this.tabPageOrtho.TabIndex = 3;
			this.tabPageOrtho.Text = "Ortho";
			this.tabPageOrtho.UseVisualStyleBackColor = true;
			// 
			// panelOrtho
			// 
			this.panelOrtho.AutoScroll = true;
			this.panelOrtho.AutoScrollMargin = new System.Drawing.Size(0, 10);
			this.panelOrtho.BackColor = System.Drawing.SystemColors.Control;
			this.panelOrtho.Controls.Add(this.textOrthoAutoFee);
			this.panelOrtho.Controls.Add(this.labelOrthoAutoFee);
			this.panelOrtho.Controls.Add(this.butDefaultAutoOrthoProc);
			this.panelOrtho.Controls.Add(this.butPickOrthoProc);
			this.panelOrtho.Controls.Add(this.textOrthoAutoProc);
			this.panelOrtho.Controls.Add(this.label37);
			this.panelOrtho.Controls.Add(this.comboOrthoClaimType);
			this.panelOrtho.Controls.Add(this.comboOrthoAutoProcPeriod);
			this.panelOrtho.Controls.Add(this.labelAutoOrthoProcPeriod);
			this.panelOrtho.Controls.Add(this.label36);
			this.panelOrtho.Controls.Add(this.checkOrthoWaitDays);
			this.panelOrtho.Location = new System.Drawing.Point(-4, 1);
			this.panelOrtho.Name = "panelOrtho";
			this.panelOrtho.Size = new System.Drawing.Size(454, 438);
			this.panelOrtho.TabIndex = 3;
			// 
			// textOrthoAutoFee
			// 
			this.textOrthoAutoFee.Location = new System.Drawing.Point(153, 58);
			this.textOrthoAutoFee.MaxVal = 100000000D;
			this.textOrthoAutoFee.MinVal = -100000000D;
			this.textOrthoAutoFee.Name = "textOrthoAutoFee";
			this.textOrthoAutoFee.Size = new System.Drawing.Size(133, 20);
			this.textOrthoAutoFee.TabIndex = 183;
			// 
			// labelOrthoAutoFee
			// 
			this.labelOrthoAutoFee.Location = new System.Drawing.Point(13, 59);
			this.labelOrthoAutoFee.Name = "labelOrthoAutoFee";
			this.labelOrthoAutoFee.Size = new System.Drawing.Size(140, 19);
			this.labelOrthoAutoFee.TabIndex = 182;
			this.labelOrthoAutoFee.Text = "Ortho Auto Fee";
			this.labelOrthoAutoFee.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butDefaultAutoOrthoProc
			// 
			this.butDefaultAutoOrthoProc.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDefaultAutoOrthoProc.Autosize = true;
			this.butDefaultAutoOrthoProc.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDefaultAutoOrthoProc.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDefaultAutoOrthoProc.CornerRadius = 3F;
			this.butDefaultAutoOrthoProc.Location = new System.Drawing.Point(313, 36);
			this.butDefaultAutoOrthoProc.Name = "butDefaultAutoOrthoProc";
			this.butDefaultAutoOrthoProc.Size = new System.Drawing.Size(52, 20);
			this.butDefaultAutoOrthoProc.TabIndex = 180;
			this.butDefaultAutoOrthoProc.Text = "Default";
			this.butDefaultAutoOrthoProc.Click += new System.EventHandler(this.butDefaultAutoOrthoProc_Click);
			// 
			// butPickOrthoProc
			// 
			this.butPickOrthoProc.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickOrthoProc.Autosize = true;
			this.butPickOrthoProc.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickOrthoProc.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickOrthoProc.CornerRadius = 3F;
			this.butPickOrthoProc.Location = new System.Drawing.Point(289, 36);
			this.butPickOrthoProc.Name = "butPickOrthoProc";
			this.butPickOrthoProc.Size = new System.Drawing.Size(21, 20);
			this.butPickOrthoProc.TabIndex = 179;
			this.butPickOrthoProc.Text = "...";
			this.butPickOrthoProc.Click += new System.EventHandler(this.butPickOrthoProc_Click);
			// 
			// textOrthoAutoProc
			// 
			this.textOrthoAutoProc.Location = new System.Drawing.Point(153, 36);
			this.textOrthoAutoProc.Name = "textOrthoAutoProc";
			this.textOrthoAutoProc.ReadOnly = true;
			this.textOrthoAutoProc.Size = new System.Drawing.Size(133, 20);
			this.textOrthoAutoProc.TabIndex = 178;
			// 
			// label37
			// 
			this.label37.Location = new System.Drawing.Point(12, 37);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(140, 19);
			this.label37.TabIndex = 177;
			this.label37.Text = "Ortho Auto Proc";
			this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboOrthoClaimType
			// 
			this.comboOrthoClaimType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboOrthoClaimType.Location = new System.Drawing.Point(153, 12);
			this.comboOrthoClaimType.MaxDropDownItems = 30;
			this.comboOrthoClaimType.Name = "comboOrthoClaimType";
			this.comboOrthoClaimType.Size = new System.Drawing.Size(212, 21);
			this.comboOrthoClaimType.TabIndex = 175;
			this.comboOrthoClaimType.SelectionChangeCommitted += new System.EventHandler(this.comboOrthoClaimType_SelectionChangeCommitted);
			// 
			// comboOrthoAutoProcPeriod
			// 
			this.comboOrthoAutoProcPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboOrthoAutoProcPeriod.Location = new System.Drawing.Point(153, 81);
			this.comboOrthoAutoProcPeriod.MaxDropDownItems = 30;
			this.comboOrthoAutoProcPeriod.Name = "comboOrthoAutoProcPeriod";
			this.comboOrthoAutoProcPeriod.Size = new System.Drawing.Size(212, 21);
			this.comboOrthoAutoProcPeriod.TabIndex = 176;
			// 
			// labelAutoOrthoProcPeriod
			// 
			this.labelAutoOrthoProcPeriod.Location = new System.Drawing.Point(12, 83);
			this.labelAutoOrthoProcPeriod.Name = "labelAutoOrthoProcPeriod";
			this.labelAutoOrthoProcPeriod.Size = new System.Drawing.Size(140, 19);
			this.labelAutoOrthoProcPeriod.TabIndex = 163;
			this.labelAutoOrthoProcPeriod.Text = "Auto Proc Period";
			this.labelAutoOrthoProcPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label36
			// 
			this.label36.Location = new System.Drawing.Point(12, 14);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(140, 19);
			this.label36.TabIndex = 161;
			this.label36.Text = "Ortho Claim Type";
			this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkOrthoWaitDays
			// 
			this.checkOrthoWaitDays.Enabled = false;
			this.checkOrthoWaitDays.Location = new System.Drawing.Point(8, 105);
			this.checkOrthoWaitDays.Name = "checkOrthoWaitDays";
			this.checkOrthoWaitDays.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkOrthoWaitDays.Size = new System.Drawing.Size(355, 21);
			this.checkOrthoWaitDays.TabIndex = 0;
			this.checkOrthoWaitDays.Text = "Wait 30 days before creating the first automatic claim";
			this.checkOrthoWaitDays.UseVisualStyleBackColor = true;
			// 
			// gridBenefits
			// 
			this.gridBenefits.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridBenefits.HasAddButton = false;
			this.gridBenefits.HasDropDowns = false;
			this.gridBenefits.HasMultilineHeaders = false;
			this.gridBenefits.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridBenefits.HeaderHeight = 15;
			this.gridBenefits.HScrollVisible = false;
			this.gridBenefits.Location = new System.Drawing.Point(468, 334);
			this.gridBenefits.Name = "gridBenefits";
			this.gridBenefits.ScrollValue = 0;
			this.gridBenefits.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridBenefits.Size = new System.Drawing.Size(502, 326);
			this.gridBenefits.TabIndex = 146;
			this.gridBenefits.Title = "Benefit Information";
			this.gridBenefits.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridBenefits.TitleHeight = 18;
			this.gridBenefits.TranslationName = "TableBenefits";
			this.gridBenefits.DoubleClick += new System.EventHandler(this.gridBenefits_DoubleClick);
			// 
			// textPlanNote
			// 
			this.textPlanNote.AcceptsTab = true;
			this.textPlanNote.BackColor = System.Drawing.SystemColors.Window;
			this.textPlanNote.DetectLinksEnabled = false;
			this.textPlanNote.DetectUrls = false;
			this.textPlanNote.Location = new System.Drawing.Point(14, 581);
			this.textPlanNote.Name = "textPlanNote";
			this.textPlanNote.QuickPasteType = OpenDentBusiness.QuickPasteType.InsPlan;
			this.textPlanNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textPlanNote.Size = new System.Drawing.Size(395, 85);
			this.textPlanNote.TabIndex = 1;
			this.textPlanNote.Text = "1 - InsPlan\n2\n3 lines will show here in 46 vert.\n4 lines will show here in 59 ver" +
    "t.\n5 lines in 72 vert\n6 in 85";
			// 
			// FormInsPlan
			// 
			this.ClientSize = new System.Drawing.Size(982, 700);
			this.Controls.Add(this.tabControlInsPlan);
			this.Controls.Add(this.checkDontVerify);
			this.Controls.Add(this.label34);
			this.Controls.Add(this.butVerifyBenefits);
			this.Controls.Add(this.textDateLastVerifiedBenefits);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.gridBenefits);
			this.Controls.Add(this.groupChanges);
			this.Controls.Add(this.textPlanNote);
			this.Controls.Add(this.label18);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.panelPat);
			this.Controls.Add(this.butLabel);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.groupRequestBen);
			this.Controls.Add(this.groupSubscriber);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormInsPlan";
			this.ShowInTaskbar = false;
			this.Text = "Edit Insurance Plan";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormInsPlan_Closing);
			this.Load += new System.EventHandler(this.FormInsPlan_Load);
			this.groupSubscriber.ResumeLayout(false);
			this.groupSubscriber.PerformLayout();
			this.groupRequestBen.ResumeLayout(false);
			this.groupRequestBen.PerformLayout();
			this.panelPat.ResumeLayout(false);
			this.panelPat.PerformLayout();
			this.groupChanges.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabControlInsPlan.ResumeLayout(false);
			this.tabPageInsPlanInfo.ResumeLayout(false);
			this.panelPlan.ResumeLayout(false);
			this.panelPlan.PerformLayout();
			this.groupCoPay.ResumeLayout(false);
			this.groupPlan.ResumeLayout(false);
			this.groupPlan.PerformLayout();
			this.groupCarrier.ResumeLayout(false);
			this.groupCarrier.PerformLayout();
			this.tabPageOtherInsInfo.ResumeLayout(false);
			this.panelOrthInfo.ResumeLayout(false);
			this.tabPageCanadian.ResumeLayout(false);
			this.panelCanadian.ResumeLayout(false);
			this.groupCanadian.ResumeLayout(false);
			this.groupCanadian.PerformLayout();
			this.tabPageOrtho.ResumeLayout(false);
			this.panelOrtho.ResumeLayout(false);
			this.panelOrtho.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormInsPlan_Load(object sender,System.EventArgs e) {
			Cursor=Cursors.WaitCursor;
			_planCurOriginal=_planCur.Copy();
			_listInsFilingCodes=InsFilingCodes.GetDeepCopy();
			if(_subCur!=null) {
				_subOld=_subCur.Copy();
			}
			long patPlanNum=0;
			if(!Security.IsAuthorized(Permissions.InsPlanEdit,true)) {
				Label labelNoPermission=new Label();
				labelNoPermission.Text=Lan.g(this,"No Insurance Plan Edit permission.  Patient and Subscriber Information can still be saved.");
				labelNoPermission.Location=new Point(groupChanges.Location.X,groupChanges.Location.Y+10);
				labelNoPermission.Size=new Size(groupChanges.Size.Width+0,groupChanges.Size.Height);
				labelNoPermission.Visible=true;
				this.Controls.Add(labelNoPermission);
				groupChanges.Visible=false;
				//It was decided by Nathan that restricting users from pressing the "Pick From List" button 
				//was an oversight and doesn't actually modify insurance information.
				//butPick.Enabled=false;
				butPickCarrier.Enabled=false;
				comboElectIDdescript.Enabled=false;
				checkIsMedical.Enabled=false;
				textEmployer.Enabled=false;
				textCarrier.Enabled=false;
				textPhone.Enabled=false;
				textAddress.Enabled=false;
				textAddress2.Enabled=false;
				textCity.Enabled=false;
				textState.Enabled=false;
				textZip.Enabled=false;
				textElectID.Enabled=false;
				butSearch.Enabled=false;
				checkNoSendElect.Enabled=false;
				textGroupName.Enabled=false;
				textGroupNum.Enabled=false;
				textLinkedNum.Enabled=false;
				textBIN.Enabled=false;
				textDivisionNo.Enabled=false;
				comboPlanType.Enabled=false;
				butSubstCodes.Enabled=false;
				checkAlternateCode.Enabled=false;
				checkCodeSubst.Enabled=false;
				checkClaimsUseUCR.Enabled=false;
				checkIsHidden.Enabled=false;
				comboFeeSched.Enabled=false;
				comboClaimForm.Enabled=false;
				comboCopay.Enabled=false;
				comboAllowedFeeSched.Enabled=false;
				comboCobRule.Enabled=false;
				comboFilingCode.Enabled=false;
				comboFilingCodeSubtype.Enabled=false;
				comboBillType.Enabled=false;
				checkShowBaseUnits.Enabled=false;
				textDentaide.Enabled=false;
				textPlanFlag.Enabled=false;
				checkIsPMP.Enabled=false;
				textCanadianDiagCode.Enabled=false;
				textCanadianInstCode.Enabled=false;
				textPlanNote.Enabled=false;
				butGetElectronic.Enabled=false;
				butHistoryElect.Enabled=false;
				butImportTrojan.Enabled=false;
				butIapFind.Enabled=false;
				butBenefitNotes.Enabled=false;
				checkDontVerify.Enabled=false;
				textTrojanID.Enabled=false;
				//Allow users to verify that the current insurance plan information is correct.  Since this doesn't affect the insurance plan itself,
				//it is acceptable to allow them to acknowledge correct plans.
				//butVerifyBenefits.Enabled=false;
				//textDateLastVerifiedBenefits.Enabled=false;
				butDelete.Enabled=false;
			}
			if(PatPlanCur!=null) {
				patPlanNum=PatPlanCur.PatPlanNum;
			}
			if(_subCur==null) {//editing from big list
				butPick.Visible=false;//This prevents an infinite loop
				//groupRequestBen.Visible=false;//might try to make this functional later, but not now.
				//groupRequestBen:---------------------------------------------
				butGetElectronic.Visible=false;
				butHistoryElect.Visible=false;
				labelHistElect.Visible=false;
				textElectBenLastDate.Visible=false;
				butImportTrojan.Visible=false;
				butIapFind.Visible=false;
				textTrojanID.Enabled=false;//view only
				butBenefitNotes.Visible=false;
				//end of groupRequestBen
				groupSubscriber.Visible=false;
				//radioChangeAll.Checked=true;//this logic needs to be repeated in OK.
				groupChanges.Visible=false;
				//benefitList=Benefits.RefreshForAll(PlanCur);
				//if(IsReadOnly) {
				//	butOK.Enabled=false;
				//}
				butDelete.Visible=false;
			}
			else {//editing from a patient
				if(PrefC.GetBool(PrefName.InsurancePlansShared)) {
					radioChangeAll.Checked=true;
				}
			}
			checkDontVerify.Checked=_planCur.HideFromVerifyList;
			InsVerify insVerifyBenefitsCur=InsVerifies.GetOneByFKey(_planCur.PlanNum,VerifyTypes.InsuranceBenefit);
			if(insVerifyBenefitsCur!=null && insVerifyBenefitsCur.DateLastVerified.Year>1880) {//Only show a date if this insurance has ever been verified
				textDateLastVerifiedBenefits.Text=insVerifyBenefitsCur.DateLastVerified.ToShortDateString();
			}
			if(IsNewPlan) {//Regardless of whether from big list or from individual patient.  Overrides above settings.
				//radioCreateNew.Checked=true;//this logic needs to be repeated in OK.
				//groupChanges.Visible=false;//because it wouldn't make sense to apply anything to "all"
				if(PrefC.GetBool(PrefName.InsDefaultPPOpercent)) {
					_planCur.PlanType="p";
				}
				_planCur.CobRule=(EnumCobRule)PrefC.GetInt(PrefName.InsDefaultCobRule);
				textDateLastVerifiedBenefits.Text="";
			}
			benefitList=Benefits.RefreshForPlan(_planCur.PlanNum,patPlanNum);
			benefitListOld=new List<Benefit>();
			for(int i=0;i<benefitList.Count;i++){
				benefitListOld.Add(benefitList[i].Copy());
			}
			if(_planCur.PlanNum!=0) {
				textInsPlanNum.Text=_planCur.PlanNum.ToString();
			}
			if(PrefC.GetBool(PrefName.EasyHideCapitation)) {
				//groupCoPay.Visible=false;
				//comboCopay.Visible=false;
			}
			if(PrefC.GetBool(PrefName.EasyHideMedicaid)) {
				checkAlternateCode.Visible=false;
			}
			Program ProgramCur=Programs.GetCur(ProgramName.Trojan);
			if(ProgramCur!=null && ProgramCur.Enabled) {
				textTrojanID.Text=_planCur.TrojanID;
			}
			else {
				//labelTrojan.Visible=false;
				labelTrojanID.Visible=false;
				butImportTrojan.Visible=false;
				textTrojanID.Visible=false;
			}
			ProgramCur=Programs.GetCur(ProgramName.IAP);
			if(ProgramCur==null || !ProgramCur.Enabled) {
				//labelIAP.Visible=false;
				butIapFind.Visible=false;
			}
			if(!butIapFind.Visible && !butImportTrojan.Visible) {
				butBenefitNotes.Visible=false;
			}
			//FillPatData------------------------------
			if(PatPlanCur==null) {
				panelPat.Visible=false;
			}
			else {
				comboRelationship.Items.Clear();
				for(int i=0;i<Enum.GetNames(typeof(Relat)).Length;i++) {
					comboRelationship.Items.Add(Lan.g("enumRelat",Enum.GetNames(typeof(Relat))[i]));
					if((int)PatPlanCur.Relationship==i) {
						comboRelationship.SelectedIndex=i;
					}
				}
				if(PatPlanCur.PatPlanNum!=0) {
					textPatPlanNum.Text=PatPlanCur.PatPlanNum.ToString();
					if(IsNewPatPlan) {
						//Relationship is set to Self,  but the subscriber for the plan is not set to the current patient.
						if(comboRelationship.SelectedIndex==0 && _subCur.Subscriber!=PatPlanCur.PatNum) {
								comboRelationship.SelectedIndex=-1;
						}
					}
					else {
						InsVerify insVerifyPatPlanCur=InsVerifies.GetOneByFKey(PatPlanCur.PatPlanNum,VerifyTypes.PatientEnrollment);
						if(insVerifyPatPlanCur!=null && insVerifyPatPlanCur.DateLastVerified.Year>1880) {
							textDateLastVerifiedPatPlan.Text=insVerifyPatPlanCur.DateLastVerified.ToShortDateString();
						}
					}
				}
				textOrdinal.Text=PatPlanCur.Ordinal.ToString();
				checkIsPending.Checked=PatPlanCur.IsPending;
				textPatID.Text=PatPlanCur.PatID;
				FillPatientAdjustments();
			}
			if(_subCur!=null) {
				textSubscriber.Text=Patients.GetLim(_subCur.Subscriber).GetNameLF();
				textSubscriberID.Text=_subCur.SubscriberID;
				if(_subCur.DateEffective.Year < 1880) {
					textDateEffect.Text="";
				}
				else {
					textDateEffect.Text=_subCur.DateEffective.ToString("d");
				}
				if(_subCur.DateTerm.Year < 1880) {
					textDateTerm.Text="";
				}
				else {
					textDateTerm.Text=_subCur.DateTerm.ToString("d");
				}
				checkRelease.Checked=_subCur.ReleaseInfo;
				checkAssign.Checked=_subCur.AssignBen;
				textSubscNote.Text=_subCur.SubscNote;
			}
			FeeSchedsStandard=FeeScheds.GetListForType(FeeScheduleType.Normal,false);
			FeeSchedsCopay=FeeScheds.GetListForType(FeeScheduleType.CoPay,false)
				.Union(FeeScheds.GetListForType(FeeScheduleType.FixedBenefit,false))
				.ToList();
			FeeSchedsAllowed=FeeScheds.GetListForType(FeeScheduleType.OutNetwork,false);
			//Clearinghouse clearhouse=Clearinghouses.GetDefault();
			//if(clearhouse==null || clearhouse.CommBridge!=EclaimsCommBridge.ClaimConnect) {
			//	butEligibility.Visible=false;
			//}
			_employerNameOrig=Employers.GetName(_planCur.EmployerNum);
			_employerNameCur=Employers.GetName(_planCur.EmployerNum);
			_carrierNumOrig=_planCur.CarrierNum;
			_listClaimForms=ClaimForms.GetDeepCopy(true);
			FillFormWithPlanCur(false);
			FillBenefits();
			DateTime dateLast270=Etranss.GetLastDate270(_planCur.PlanNum);
			if(dateLast270.Year<1880) {
				textElectBenLastDate.Text="";
			}
			else {
				textElectBenLastDate.Text=dateLast270.ToShortDateString();
			}
			if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
				checkCodeSubst.Visible=false;
			}
			_datePatPlanLastVerified=PIn.Date(textDateLastVerifiedPatPlan.Text);
			_orthoAutoProc=_planCur.OrthoAutoProcCodeNumOverride==0 ? null : ProcedureCodes.GetProcCode(_planCur.OrthoAutoProcCodeNumOverride);
			FillOrtho();
			Cursor=Cursors.Default;
		}

		///<summary>Fills controls with ortho information.  Also hides the controls if needed.</summary>
		private void FillOrtho() {
			if(!PrefC.GetBool(PrefName.OrthoEnabled)) {
				butPatOrtho.Visible=false;
				tabControlInsPlan.TabPages.Remove(tabPageOrtho);
				return;
			}
			foreach(OrthoClaimType type in Enum.GetValues(typeof(OrthoClaimType))) {
				comboOrthoClaimType.Items.Add(Lan.g("enumOrthoClaimType",type.GetDescription()));
				if(_planCur.OrthoType==type) {
					comboOrthoClaimType.SelectedIndex = (int)type;
				}
			}
			foreach(OrthoAutoProcFrequency type in Enum.GetValues(typeof(OrthoAutoProcFrequency))) {
				comboOrthoAutoProcPeriod.Items.Add(Lan.g("enumOrthoAutoProcFrequency",type.GetDescription()));
				if(_planCur.OrthoAutoProcFreq==type) {
					comboOrthoAutoProcPeriod.SelectedIndex = (int)type;
				}
			}
			textOrthoAutoFee.Text=_planCur.OrthoAutoFeeBilled.ToString();
			checkOrthoWaitDays.Checked=_planCur.OrthoAutoClaimDaysWait > 0;
			if(_orthoAutoProc!=null) {
				textOrthoAutoProc.Text=_orthoAutoProc.ProcCode;
			}
			else {
				textOrthoAutoProc.Text=ProcedureCodes.GetProcCode(PrefC.GetLong(PrefName.OrthoAutoProcCodeNum)).ProcCode +" ("+ Lan.g(this,"Default")+")";
			}
			SetEnabledOrtho();
		}

		private void SetEnabledOrtho() {
			if(!Security.IsAuthorized(Permissions.InsPlanOrthoEdit,true)) {
				//Disable every control within the Ortho tab.
				foreach(Control control in panelOrtho.Controls) {
					ODException.SwallowAnyException(() => { control.Enabled=false; });
				}
				return;
			}
			if(comboOrthoClaimType.SelectedIndex!=(int)OrthoClaimType.InitialPlusPeriodic) {
				comboOrthoAutoProcPeriod.Enabled=false;
				checkOrthoWaitDays.Checked=false;
				checkOrthoWaitDays.Enabled=false;
				labelAutoOrthoProcPeriod.Enabled=false;
				butPickOrthoProc.Enabled=false;
				labelOrthoAutoFee.Enabled=false;
				textOrthoAutoFee.Enabled=false;
				butDefaultAutoOrthoProc.Enabled=false;
			}
			else {
				comboOrthoAutoProcPeriod.Enabled=true;
				checkOrthoWaitDays.Enabled=true;
				labelAutoOrthoProcPeriod.Enabled=true;
				butPickOrthoProc.Enabled=true;
				labelOrthoAutoFee.Enabled=true;
				textOrthoAutoFee.Enabled=true;
				butDefaultAutoOrthoProc.Enabled=true;
			}
			if(comboOrthoClaimType.SelectedIndex==-1) {
				comboOrthoClaimType.SelectedIndex=0;
			}
			if(comboOrthoAutoProcPeriod.SelectedIndex==-1) {
				comboOrthoAutoProcPeriod.SelectedIndex=0;
			}	
		}

		///<summary>Uses PlanCur to fill out the information on the form.  Called once on startup and also if user picks a plan from template list.  This does not fill from SubCur, unlike FillPlanCurFromForm().</summary>
		private void FillFormWithPlanCur(bool isPicked) {
			Cursor=Cursors.WaitCursor;
			textEmployer.Text=Employers.GetName(_planCur.EmployerNum);
			_employerNameCur=textEmployer.Text;
			textGroupName.Text=_planCur.GroupName;
			textGroupNum.Text=_planCur.GroupNum;
			if(PrefC.GetBool(PrefName.ShowFeatureEhr)) {
				textBIN.Text=_planCur.RxBIN;
			}
			else{
				labelBIN.Visible=false;
				textBIN.Visible=false;
			}
			textDivisionNo.Text=_planCur.DivisionNo;//only visible in Canada
			textTrojanID.Text=_planCur.TrojanID;
			comboPlanType.Items.Clear();
			//Items must be added in the same order in which they are listed in InsPlanTypeComboItem.
			comboPlanType.Items.Add(Lan.g(this,"Category Percentage"));
			comboPlanType.Items.Add(Lan.g(this,"PPO Percentage"));
			comboPlanType.Items.Add(Lan.g(this,"PPO Fixed Benefit"));
			comboPlanType.Items.Add(Lan.g(this,"Medicaid or Flat Co-pay"));
			//Capitation must always be last, since it is sometimes hidden.
			if(!PrefC.GetBool(PrefName.EasyHideCapitation)) {
				comboPlanType.Items.Add(Lan.g(this,"Capitation"));
				if(_planCur.PlanType=="c") {
					comboPlanType.SelectedIndex=(int)InsPlanTypeComboItem.Capitation;
				}
			}
			if(_planCur.PlanType=="") {
				comboPlanType.SelectedIndex=(int)InsPlanTypeComboItem.CategoryPercentage;
			}
			if(_planCur.PlanType=="p") {
				comboPlanType.SelectedIndex=(int)InsPlanTypeComboItem.PPO;
				FeeSched copayFeeSched=FeeScheds.GetFirstOrDefault(x => x.FeeSchedNum==_planCur.CopayFeeSched 
					&& x.FeeSchedType==FeeScheduleType.FixedBenefit);
				if(copayFeeSched!=null) {
					comboPlanType.SelectedIndex=(int)InsPlanTypeComboItem.PPOFixedBenefit;
				}
			}
			if(_planCur.PlanType=="f") {
				comboPlanType.SelectedIndex=(int)InsPlanTypeComboItem.MedicaidOrFlatCopay;
			}
			_selectedPlanType=PIn.Enum<InsPlanTypeComboItem>(comboPlanType.SelectedIndex);
			checkAlternateCode.Checked=_planCur.UseAltCode;
			checkCodeSubst.Checked=_planCur.CodeSubstNone;
			checkIsMedical.Checked=_planCur.IsMedical;
			if(!PrefC.GetBool(PrefName.ShowFeatureMedicalInsurance)) {
				checkIsMedical.Visible=false;//This line prevents most users from modifying the Medical Insurance checkbox by accident, because most offices are dental only.
			}
			checkClaimsUseUCR.Checked=_planCur.ClaimsUseUCR;
			if(IsNewPlan && _planCur.PlanType=="" && PrefC.GetBool(PrefName.InsDefaultShowUCRonClaims) && !isPicked) {
				checkClaimsUseUCR.Checked=true;
			}
			if(IsNewPlan && !PrefC.GetBool(PrefName.InsDefaultAssignBen) && !isPicked) {
				checkAssign.Checked=false;
			}
			checkIsHidden.Checked=_planCur.IsHidden;
			checkShowBaseUnits.Checked=_planCur.ShowBaseUnits;
			comboFeeSched.Items.Clear();
			comboFeeSched.Items.Add(Lan.g(this,"none"));
			comboFeeSched.SelectedIndex=0;
			int idx=-1;
			for(int i=0;i<FeeSchedsStandard.Count;i++) {
				comboFeeSched.Items.Add(FeeSchedsStandard[i].Description);
				if(FeeSchedsStandard[i].FeeSchedNum==_planCur.FeeSched)
					idx=i+1;
			}
			if(_planCur.FeeSched==0) {
				idx=0;
			}
			comboFeeSched.IndexSelectOrSetText(idx,() => FeeScheds.GetDescription(_planCur.FeeSched));
			comboClaimForm.Items.Clear();
			for(int i=0;i<_listClaimForms.Count;i++) {
				comboClaimForm.Items.Add(_listClaimForms[i].Description);
				if(_listClaimForms[i].ClaimFormNum==_planCur.ClaimFormNum) {
					comboClaimForm.SelectedIndex=i;
				}
			}
			if(comboClaimForm.Items.Count>0 && comboClaimForm.SelectedIndex==-1) {
				for(int i=0;i<_listClaimForms.Count;i++) {
					if(_listClaimForms[i].ClaimFormNum==PrefC.GetLong(PrefName.DefaultClaimForm)) {
						comboClaimForm.SelectedIndex=i;
					}
				}
			}
			FillComboCopay(FeeSchedsCopay,_selectedPlanType==InsPlanTypeComboItem.PPOFixedBenefit);
			comboAllowedFeeSched.Items.Clear();
			comboAllowedFeeSched.Items.Add(Lan.g(this,"none"));
			comboAllowedFeeSched.SelectedIndex=0;
			for(int i=0;i<FeeSchedsAllowed.Count;i++) {
				comboAllowedFeeSched.Items.Add(FeeSchedsAllowed[i].Description);
				if(FeeSchedsAllowed[i].FeeSchedNum==_planCur.AllowedFeeSched) {
					comboAllowedFeeSched.SelectedIndex=i+1;
				}
			}
			comboCobRule.Items.Clear();
			for(int i=0;i<Enum.GetNames(typeof(EnumCobRule)).Length;i++) {
				comboCobRule.Items.Add(Lan.g("enumEnumCobRule",Enum.GetNames(typeof(EnumCobRule))[i]));
			}
			comboCobRule.SelectedIndex=(int)_planCur.CobRule;			
			long selectedFilingCodeNum=_planCur.FilingCode;
			if(comboFilingCode.SelectedItem!=null) {
				selectedFilingCodeNum=((ODBoxItem<InsFilingCode>)comboFilingCode.SelectedItem).Tag.InsFilingCodeNum;
			}
			comboFilingCode.Items.Clear();
			ODBoxItem<InsFilingCode> comboItem;
			for(int i=0;i<_listInsFilingCodes.Count;i++) {
				comboItem=new ODBoxItem<InsFilingCode> (_listInsFilingCodes[i].Descript, _listInsFilingCodes[i]);
				comboFilingCode.Items.Add(comboItem);
				if(_listInsFilingCodes[i].InsFilingCodeNum==selectedFilingCodeNum) {
					comboFilingCode.SelectedIndex=i;
				}
			}
			FillComboFilingSubtype(selectedFilingCodeNum);
			comboBillType.Items.Clear();
			_listBillingTypeDefs=new List<Def>() { new Def() { DefNum=0,ItemName=Lans.g(this,"None")} };
			_listBillingTypeDefs.AddRange(Defs.GetDefsForCategory(DefCat.BillingTypes,true));
			_listBillingTypeDefs.ForEach(x => comboBillType.Items.Add(x.ItemName));
			comboBillType.IndexSelectOrSetText(_listBillingTypeDefs.FindIndex(x => x.DefNum==_planCur.BillingType),() => { 
					return Defs.GetName(DefCat.BillingTypes,_planCur.BillingType)+" "+Lans.g(this,"(hidden)");
			});
			FillCarrier(_planCur.CarrierNum);
			if(!Security.IsAuthorized(Permissions.CarrierCreate,true)) {
				textCarrier.Enabled=false;
				textPhone.Enabled=false;
				textAddress.Enabled=false;
				textAddress2.Enabled=false;
				textCity.Enabled=false;
				textState.Enabled=false;
				textZip.Enabled=false;
				textElectID.Enabled=false;
				butSearch.Enabled=false;
				checkNoSendElect.Enabled=false;
			}
			FillOtherSubscribers();
			textPlanNote.Text=_planCur.PlanNote;
			if(_planCur.DentaideCardSequence==0){
				textDentaide.Text="";
			}
			else{
				textDentaide.Text=_planCur.DentaideCardSequence.ToString();
			}
			textPlanFlag.Text=_planCur.CanadianPlanFlag;
			textCanadianDiagCode.Text=_planCur.CanadianDiagnosticCode;
			textCanadianInstCode.Text=_planCur.CanadianInstitutionCode;
			checkDontVerify.Checked=_planCur.HideFromVerifyList;
			InsVerify insVerifyBenefitsCur=InsVerifies.GetOneByFKey(_planCur.PlanNum,VerifyTypes.InsuranceBenefit);
			if(insVerifyBenefitsCur!=null && insVerifyBenefitsCur.DateLastVerified.Year>1880) {//Only show a date if this insurance has ever been verified
				textDateLastVerifiedBenefits.Text=insVerifyBenefitsCur.DateLastVerified.ToShortDateString();
				_dateInsPlanLastVerified=PIn.Date(textDateLastVerifiedBenefits.Text);
			}
			//if(PlanCur.BenefitNotes==""){
			//	butBenefitNotes.Enabled=false;
			//}
			Cursor=Cursors.Default;
		}

		private void FillComboCopay(List<FeeSched> listFeeSchedCopays,bool isFixedBenefitPlan) {
			if(isFixedBenefitPlan) {
				labelCopayFeeSched.Text=Lan.g(this,"Fixed Benefit Amounts");
			}
			else {
				labelCopayFeeSched.Text=Lan.g(this,"Patient Co-pay Amounts");
			}
			List<FeeSched> listFeeSchedFiltered=new List<FeeSched>();
			foreach(FeeSched feeSchedCur in listFeeSchedCopays) {
				bool isFixedBenefitSched=(feeSchedCur.FeeSchedType==FeeScheduleType.FixedBenefit);
				if(isFixedBenefitPlan==isFixedBenefitSched) {
					listFeeSchedFiltered.Add(feeSchedCur.Copy());
				}
			}
			comboCopay.Items.Clear();
			comboCopay.Items.Add(Lan.g(this,"none"));
			comboCopay.SelectedIndex=0;
			foreach(FeeSched feeSchedCur in listFeeSchedFiltered) {
				comboCopay.Items.Add(new ODBoxItem<FeeSched>(feeSchedCur.Description,feeSchedCur));
				if(feeSchedCur.FeeSchedNum==_planCur.CopayFeeSched) {
					comboCopay.SelectedIndex=comboCopay.Items.Count-1;
				}
			}
		}

		private void FillOtherSubscribers() {
			long excludeSub=-1;
			if(_subCur!=null){
				excludeSub=_subCur.InsSubNum;
			}
			//Even though this sub hasn't been updated to the database, this still works because SubCur.InsSubNum is valid and won't change.
			int countSubs=InsSubs.GetSubscriberCountForPlan(_planCur.PlanNum,excludeSub!=-1);
			textLinkedNum.Text=countSubs.ToString();
			if(countSubs>10000) {//10,000 per Nathan.
				comboLinked.Visible=false;
				butOtherSubscribers.Visible=true;
				butOtherSubscribers.Location=comboLinked.Location;
			}
			else {
				comboLinked.Visible=true;
				butOtherSubscribers.Visible=false;
				List<string> listSubs=InsSubs.GetSubscribersForPlan(_planCur.PlanNum,excludeSub);
				comboLinked.Items.Clear();
				comboLinked.Items.AddRange(listSubs.ToArray());
				if(listSubs.Count>0){
					comboLinked.SelectedIndex=0;
				}
			}
		}

		private void butOtherSubscribers_Click(object sender,EventArgs e) {
			ODForm form=new ODForm() {
				Size=new Size(500,400),
				Text="Other Subscribers List",
				FormBorderStyle=FormBorderStyle.FixedSingle
			};
			ODGrid grid=new ODGrid() {
				Size=new Size(475,300),
				Location=new Point(5,5),
				Title="Subscribers",
				TranslationName=""
			};
			UI.Button butClose=new UI.Button() {
				Size=new Size(75,23),
				Text="Close",
				Location=new Point(form.ClientSize.Width-80,form.ClientSize.Height-28),//subtract the button's size plus 5 pixel buffer.
			};
			butClose.Click+=(s,ex) => form.Close();//When butClose is pressed, simply close the form.  If more functionality is needed, make a method below.
			form.Controls.Add(grid);
			form.Controls.Add(butClose);
			grid.BeginUpdate();
			grid.Columns.Clear();
			grid.Columns.Add(new ODGridColumn(Lan.g(this,"Name"),0));
			grid.Rows.Clear();
			long excludeSub=-1;
			if(_subCur!=null){
				excludeSub=_subCur.InsSubNum;
			}
			List<string> listSubs=InsSubs.GetSubscribersForPlan(_planCur.PlanNum,excludeSub);
			foreach(string subName in listSubs) {
				grid.Rows.Add(new ODGridRow(subName));
			}
			grid.EndUpdate();
			form.ShowDialog();
		}
		
		private void FillPatientAdjustments() {
			List<ClaimProc> ClaimProcList=ClaimProcs.Refresh(PatPlanCur.PatNum);
			AdjAL=new ArrayList();//move selected claimprocs into ALAdj
			for(int i=0;i<ClaimProcList.Count;i++) {
				if(ClaimProcList[i].InsSubNum==_subCur.InsSubNum
					&& ClaimProcList[i].Status==ClaimProcStatus.Adjustment) {
					AdjAL.Add(ClaimProcList[i]);
				}
			}
			listAdj.Items.Clear();
			string s;
			for(int i=0;i<AdjAL.Count;i++) {
				s=((ClaimProc)AdjAL[i]).ProcDate.ToShortDateString()+"       Ins Used:  "
					+((ClaimProc)AdjAL[i]).InsPayAmt.ToString("F")+"       Ded Used:  "
					+((ClaimProc)AdjAL[i]).DedApplied.ToString("F");
				listAdj.Items.Add(s);
			}
		}

		///<summary>Fills the carrier fields on the form based on the specified carrierNum.</summary>
		private void FillCarrier(long carrierNum) {
			_carrierCur=Carriers.GetCarrier(carrierNum);
			textCarrier.Text=_carrierCur.CarrierName;
			textPhone.Text=_carrierCur.Phone;
			textAddress.Text=_carrierCur.Address;
			textAddress2.Text=_carrierCur.Address2;
			textCity.Text=_carrierCur.City;
			textState.Text=_carrierCur.State;
			textZip.Text=_carrierCur.Zip;
			textElectID.Text=_carrierCur.ElectID;
			_electIdCur=textElectID.Text;
			FillPayor();
			checkNoSendElect.Checked=_carrierCur.NoSendElect;
		}

		///<summary>Only called from FillCarrier and textElectID_Validating. Fills comboElectIDdescript as appropriate.</summary>
		private void FillPayor() {
			//textElectIDdescript.Text=ElectIDs.GetDescript(textElectID.Text);
			comboElectIDdescript.Items.Clear();
			string[] payorNames=ElectIDs.GetDescripts(textElectID.Text);
			if(payorNames.Length>1) {
				comboElectIDdescript.Items.Add("multiple payors use this ID");
			}
			for(int i=0;i<payorNames.Length;i++) {
				comboElectIDdescript.Items.Add(payorNames[i]);
			}
			if(payorNames.Length>0) {
				comboElectIDdescript.SelectedIndex=0;
			}
		}

		private void comboElectIDdescript_SelectedIndexChanged(object sender,System.EventArgs e) {
			if(comboElectIDdescript.Items.Count>0) {
				comboElectIDdescript.SelectedIndex=0;//always show the first item in the list
			}
		}

		private void comboPlanType_SelectionChangeCommitted(object sender,System.EventArgs e) {
			//MessageBox.Show(InsPlans.Cur.PlanType+","+listPlanType.SelectedIndex.ToString());
			if((_planCur.PlanType=="" || _planCur.PlanType=="p")
				&& comboPlanType.SelectedIndex.In((int)InsPlanTypeComboItem.MedicaidOrFlatCopay,(int)InsPlanTypeComboItem.Capitation)) 
			{
				if(!MsgBox.Show(this,true,"This will clear all percentages. Continue?")) {
					comboPlanType.SelectedIndex=(int)_selectedPlanType;//Undo the selection change.
					return;
				}
				//Loop through the list backwards so i will be valid.
				for(int i=benefitList.Count-1;i>=0;i--) {
					if(((Benefit)benefitList[i]).BenefitType==InsBenefitType.CoInsurance) {
						benefitList.RemoveAt(i);
					}
				}
				//benefitList=new ArrayList();
				FillBenefits();
			}
			else if(comboPlanType.SelectedIndex==(int)InsPlanTypeComboItem.PPOFixedBenefit) {
				if(!MsgBox.Show(this,true,"This will set all percentages to 100%. Continue?")) {
					comboPlanType.SelectedIndex=(int)_selectedPlanType;//Undo the selection change.
					return;
				}
				foreach(Benefit benefit in benefitList) {
					if(benefit.BenefitType==InsBenefitType.CoInsurance) {
						benefit.Percent=100;
					}
				}
				FillBenefits();
			}
			InsPlanTypeComboItem prevSelection=_selectedPlanType;
			_selectedPlanType=PIn.Enum<InsPlanTypeComboItem>(comboPlanType.SelectedIndex);
			switch(_selectedPlanType) {
				case InsPlanTypeComboItem.CategoryPercentage:
					_planCur.PlanType="";
					break;
				case InsPlanTypeComboItem.PPO:
				case InsPlanTypeComboItem.PPOFixedBenefit:
					_planCur.PlanType="p";
					break;
				case InsPlanTypeComboItem.MedicaidOrFlatCopay:
					_planCur.PlanType="f";
					break;
				case InsPlanTypeComboItem.Capitation:
					_planCur.PlanType="c";
					break;
				default:
					break;
			}
			if(PrefC.GetBool(PrefName.InsDefaultShowUCRonClaims)) {//otherwise, no automation on this field.
				if(_planCur.PlanType=="") {
					checkClaimsUseUCR.Checked=true;
				}
				else {
					checkClaimsUseUCR.Checked=false;
				}
			}
			if(prevSelection!=_selectedPlanType//Selection has actually changed
				&& (prevSelection==InsPlanTypeComboItem.PPOFixedBenefit || _selectedPlanType==InsPlanTypeComboItem.PPOFixedBenefit))//Is or was Fixed Benefit
			{
				//Fix the comboCopay list to match the new selection type if changing to or from PPO Fixed Benefits only.
				//Changing between non-fixed benefit types shouldn't refill the list because that will try to change the current selection
				FillComboCopay(FeeSchedsCopay,_selectedPlanType==InsPlanTypeComboItem.PPOFixedBenefit);
			}
		}

		private void butAdjAdd_Click(object sender,System.EventArgs e) {
			ClaimProc ClaimProcCur=new ClaimProc();
			ClaimProcCur.PatNum=PatPlanCur.PatNum;
			ClaimProcCur.ProcDate=DateTime.Today;
			ClaimProcCur.Status=ClaimProcStatus.Adjustment;
			ClaimProcCur.PlanNum=_planCur.PlanNum;
			ClaimProcCur.InsSubNum=_subCur.InsSubNum;
			FormInsAdj FormIA=new FormInsAdj(ClaimProcCur);
			FormIA.IsNew=true;
			FormIA.ShowDialog();
			FillPatientAdjustments();
		}

		private void listAdj_DoubleClick(object sender,System.EventArgs e) {
			if(listAdj.SelectedIndex==-1) {
				return;
			}
			FormInsAdj FormIA=new FormInsAdj((ClaimProc)AdjAL[listAdj.SelectedIndex]);
			FormIA.ShowDialog();
			FillPatientAdjustments();
		}

		///<summary>Button not visible if SubCur=null, editing from big list.</summary>
		private void butPick_Click(object sender,EventArgs e) {
			if(!IsNewPlan && !Security.IsAuthorized(Permissions.InsPlanPickListExisting,true)) {
				MsgBox.Show(this,"Permission required: 'Change existing Ins Plan using Pick From List'.\r\n"
					+"Alternatively, the Ins Plan can be dropped and a new plan may be added.");
				return;
			}
			FormInsPlans FormIP=new FormInsPlans();
			FormIP.empText=textEmployer.Text;
			FormIP.carrierText=textCarrier.Text;
			FormIP.IsSelectMode=true;
			FormIP.ShowDialog();
			if(FormIP.DialogResult==DialogResult.Cancel) {
				return;
			}
			if(!IsNewPlan && !MsgBox.Show(this,true,"Are you sure you want to use the selected plan?  You should NOT use this if the patient is changing insurance.  Use the Drop button instead.")) {
				return;
			}
			if(FormIP.SelectedPlan.PlanNum==0) {//user clicked Blank
				_planCur=new InsPlan();
				_planCur.PlanNum=_planOld.PlanNum;
			}
			else {//user selected an existing plan
				_planCur=FormIP.SelectedPlan;
				textInsPlanNum.Text=FormIP.SelectedPlan.PlanNum.ToString();
			}
			FillFormWithPlanCur(true);
			//We need to pass patPlanNum in to RefreshForPlan to get patient level benefits:
			long patPlanNum=0;
			if(PatPlanCur!=null){
				patPlanNum=PatPlanCur.PatPlanNum;
			}
			if(FormIP.SelectedPlan.PlanNum==0){//user clicked blank
				benefitList=new List<Benefit>();
			}
			else {//user selected an existing plan
				benefitList=Benefits.RefreshForPlan(_planCur.PlanNum,patPlanNum);
			}
			FillBenefits();
			if(IsNewPlan || FormIP.SelectedPlan.PlanNum==0) {//New plan or user clicked blank.
				//Leave benefitListOld alone so that it will trigger deletion of the orphaned benefits later.
			}
			else{
				//Replace benefitListOld so that we only cause changes to be save that are made after this point.
				benefitListOld=new List<Benefit>();
				for(int i=0;i<benefitList.Count;i++) {
					benefitListOld.Add(benefitList[i].Copy());
				}
			}
			//benefitListOld=new List<Benefit>(benefitList);//this was not the proper way to make a shallow copy.
			_planCurOriginal=_planCur.Copy();
			FillOtherSubscribers();
			FillOrtho();
			//PlanNumOriginal is NOT reset here.
			//It's now similar to if we'd just opened a new form, except for SubCur still needs to be changed.
		}

		private void textEmployer_KeyUp(object sender,System.Windows.Forms.KeyEventArgs e) {
			//key up is used because that way it will trigger AFTER the textBox has been changed.
			if(e.KeyCode==Keys.Return) {
				listEmps.Visible=false;
				textGroupName.Focus();
				return;
			}
			if(textEmployer.Text=="") {
				listEmps.Visible=false;
				return;
			}
			if(e.KeyCode==Keys.Down) {
				if(listEmps.Items.Count==0) {
					return;
				}
				if(listEmps.SelectedIndex==-1) {
					listEmps.SelectedIndex=0;
					textEmployer.Text=listEmps.SelectedItem.ToString();
				}
				else if(listEmps.SelectedIndex==listEmps.Items.Count-1) {
					listEmps.SelectedIndex=-1;
					textEmployer.Text=empOriginal;
				}
				else {
					listEmps.SelectedIndex++;
					textEmployer.Text=listEmps.SelectedItem.ToString();
				}
				textEmployer.SelectionStart=textEmployer.Text.Length;
				return;
			}
			if(e.KeyCode==Keys.Up) {
				if(listEmps.Items.Count==0) {
					return;
				}
				if(listEmps.SelectedIndex==-1) {
					listEmps.SelectedIndex=listEmps.Items.Count-1;
					textEmployer.Text=listEmps.SelectedItem.ToString();
				}
				else if(listEmps.SelectedIndex==0) {
					listEmps.SelectedIndex=-1;
					textEmployer.Text=empOriginal;
				}
				else {
					listEmps.SelectedIndex--;
					textEmployer.Text=listEmps.SelectedItem.ToString();
				}
				textEmployer.SelectionStart=textEmployer.Text.Length;
				return;
			}
			if(textEmployer.Text.Length==1) {
				textEmployer.Text=textEmployer.Text.ToUpper();
				textEmployer.SelectionStart=1;
			}
			empOriginal=textEmployer.Text;//the original text is preserved when using up and down arrows
			listEmps.Items.Clear();
			List<Employer> similarEmps=Employers.GetSimilarNames(textEmployer.Text);
			for(int i=0;i<similarEmps.Count;i++) {
				listEmps.Items.Add(similarEmps[i].EmpName);
			}
			int h=13*similarEmps.Count+5;
			if(h > ClientSize.Height-listEmps.Top){
				h=ClientSize.Height-listEmps.Top;
			}
			listEmps.Size=new Size(231,h);
			listEmps.Visible=true;
		}

		private void textEmployer_Leave(object sender,System.EventArgs e) {
			if(mouseIsInListEmps) {
				return;
			}
			listEmps.Visible=false;
		}

		private void listEmps_Click(object sender,System.EventArgs e) {
			textEmployer.Text=listEmps.SelectedItem.ToString();
			textEmployer.Focus();
			textEmployer.SelectionStart=textEmployer.Text.Length;
			listEmps.Visible=false;
		}

		private void listEmps_DoubleClick(object sender,System.EventArgs e) {
			//no longer used
			textEmployer.Text=listEmps.SelectedItem.ToString();
			textEmployer.Focus();
			textEmployer.SelectionStart=textEmployer.Text.Length;
			listEmps.Visible=false;
		}

		private void listEmps_MouseEnter(object sender,System.EventArgs e) {
			mouseIsInListEmps=true;
		}

		private void listEmps_MouseLeave(object sender,System.EventArgs e) {
			mouseIsInListEmps=false;
		}

		private void butPickCarrier_Click(object sender,EventArgs e) {
			FormCarriers formc=new FormCarriers();
			formc.IsSelectMode=true;
			formc.ShowDialog();
			if(formc.DialogResult!=DialogResult.OK) {
				return;
			}
			FillCarrier(formc.SelectedCarrier.CarrierNum);
		}

		private void textCarrier_KeyUp(object sender,System.Windows.Forms.KeyEventArgs e) {
			if(e.KeyCode==Keys.Return) {
				if(listCars.SelectedIndex==-1) {
					textPhone.Focus();
				}
				else {
					FillCarrier(similarCars[listCars.SelectedIndex].CarrierNum);
					textCarrier.Focus();
					textCarrier.SelectionStart=textCarrier.Text.Length;
				}
				listCars.Visible=false;
				return;
			}
			if(textCarrier.Text=="") {
				listCars.Visible=false;
				return;
			}
			if(e.KeyCode==Keys.Down) {
				if(listCars.Items.Count==0) {
					return;
				}
				if(listCars.SelectedIndex==-1) {
					listCars.SelectedIndex=0;
					textCarrier.Text=similarCars[listCars.SelectedIndex].CarrierName;
				}
				else if(listCars.SelectedIndex==listCars.Items.Count-1) {
					listCars.SelectedIndex=-1;
					textCarrier.Text=carOriginal;
				}
				else {
					listCars.SelectedIndex++;
					textCarrier.Text=similarCars[listCars.SelectedIndex].CarrierName;
				}
				textCarrier.SelectionStart=textCarrier.Text.Length;
				return;
			}
			if(e.KeyCode==Keys.Up) {
				if(listCars.Items.Count==0) {
					return;
				}
				if(listCars.SelectedIndex==-1) {
					listCars.SelectedIndex=listCars.Items.Count-1;
					textCarrier.Text=similarCars[listCars.SelectedIndex].CarrierName;
				}
				else if(listCars.SelectedIndex==0) {
					listCars.SelectedIndex=-1;
					textCarrier.Text=carOriginal;
				}
				else {
					listCars.SelectedIndex--;
					textCarrier.Text=similarCars[listCars.SelectedIndex].CarrierName;
				}
				textCarrier.SelectionStart=textCarrier.Text.Length;
				return;
			}
			if(textCarrier.Text.Length==1) {
				textCarrier.Text=textCarrier.Text.ToUpper();
				textCarrier.SelectionStart=1;
			}
			carOriginal=textCarrier.Text;//the original text is preserved when using up and down arrows
			listCars.Items.Clear();
			similarCars=Carriers.GetSimilarNames(textCarrier.Text);
			for(int i=0;i<similarCars.Count;i++) {
				listCars.Items.Add(similarCars[i].CarrierName+", "
					+similarCars[i].Phone+", "
					+similarCars[i].Address+", "
					+similarCars[i].Address2+", "
					+similarCars[i].City+", "
					+similarCars[i].State+", "
					+similarCars[i].Zip);
			}
			int h=13*similarCars.Count+5;
			if(h > ClientSize.Height-listCars.Top){
				h=ClientSize.Height-listCars.Top;
			}
			listCars.Size=new Size(listCars.Width,h);
			listCars.Visible=true;
		}

		private void textCarrier_Leave(object sender,System.EventArgs e) {
			if(mouseIsInListCars) {
				return;
			}
			//or if user clicked on a different text box.
			if(listCars.SelectedIndex!=-1) {
				FillCarrier(similarCars[listCars.SelectedIndex].CarrierNum);
			}
			listCars.Visible=false;
		}

		private void listCars_Click(object sender,System.EventArgs e) {
			FillCarrier(similarCars[listCars.SelectedIndex].CarrierNum);
			textCarrier.Focus();
			textCarrier.SelectionStart=textCarrier.Text.Length;
			listCars.Visible=false;
		}

		private void listCars_DoubleClick(object sender,System.EventArgs e) {
			//no longer used
		}

		private void listCars_MouseEnter(object sender,System.EventArgs e) {
			mouseIsInListCars=true;
		}

		private void listCars_MouseLeave(object sender,System.EventArgs e) {
			mouseIsInListCars=false;
		}

		private void textPhone_TextChanged(object sender,System.EventArgs e) {
			int cursor=textPhone.SelectionStart;
			int length=textPhone.Text.Length;
			textPhone.Text=TelephoneNumbers.AutoFormat(textPhone.Text);
			if(textPhone.Text.Length>length)
				cursor++;
			textPhone.SelectionStart=cursor;
		}

		private void textAddress_TextChanged(object sender,System.EventArgs e) {
			if(textAddress.Text.Length==1) {
				textAddress.Text=textAddress.Text.ToUpper();
				textAddress.SelectionStart=1;
			}
		}

		private void textAddress2_TextChanged(object sender,System.EventArgs e) {
			if(textAddress2.Text.Length==1) {
				textAddress2.Text=textAddress2.Text.ToUpper();
				textAddress2.SelectionStart=1;
			}
		}

		private void textCity_TextChanged(object sender,System.EventArgs e) {
			if(textCity.Text.Length==1) {
				textCity.Text=textCity.Text.ToUpper();
				textCity.SelectionStart=1;
			}
		}

		private void textState_TextChanged(object sender,System.EventArgs e) {
			if(CultureInfo.CurrentCulture.Name=="en-US" //if USA or Canada, capitalize first 2 letters
				|| CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				if(textState.Text.Length==1 || textState.Text.Length==2) {
					textState.Text=textState.Text.ToUpper();
					textState.SelectionStart=2;
				}
			}
			else {
				if(textState.Text.Length==1) {
					textState.Text=textState.Text.ToUpper();
					textState.SelectionStart=1;
				}
			}
		}

		private void textElectID_Validating(object sender,System.ComponentModel.CancelEventArgs e) {
			if(textElectID.Text=="" || textElectID.Text==_electIdCur) {
				return;
			}
			if(CultureInfo.CurrentCulture.Name.Length>=4 && CultureInfo.CurrentCulture.Name.Substring(3)=="CA"){//en-CA or fr-CA
				if(!Regex.IsMatch(textElectID.Text,@"^[0-9]{6}$")) {
					if(!MsgBox.Show(this,true,"Carrier ID should be six digits long.  Continue anyway?")){
						textElectID.Text=_electIdCur;//They clicked Cancel, set it back to what it was.
						e.Cancel=true;
						return;
					}
				}
			}
			//else{//anyplace including Canada
			string[] electIDs=ElectIDs.GetDescripts(textElectID.Text);
			if(electIDs.Length==0) {//if none found in the predefined list
				if(!Carriers.ElectIdInUse(textElectID.Text)){
					if(!MsgBox.Show(this,true,"Electronic ID not found. Continue anyway?")) {
						textElectID.Text=_electIdCur;//They clicked Cancel, set it back to what it was.
						e.Cancel=true;
						return;
					}
				}
			}
			_electIdCur=textElectID.Text;
			FillPayor();
			//}
		}
		
		private void butChange_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.InsPlanChangeSubsc)) {
				return;
			}
			try {
				InsSubs.ValidateNoKeys(_subCur.InsSubNum,false);
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Change subscriber?  This should not normally be needed.")) {
					return;
				}
			}
			catch(Exception ex){
				if(PrefC.GetBool(PrefName.SubscriberAllowChangeAlways)) {
					DialogResult dres=MessageBox.Show(Lan.g(this,"Warning!  Do not change unless fixing database corruption.  ")+"\r\n"+ex.Message);
					if(dres!=DialogResult.OK) {
						return;
					}
				}
				else {
					MessageBox.Show(Lan.g(this,"Not allowed to change.")+"\r\n"+ex.Message);
					return;
				}
			}
			Family fam=Patients.GetFamily(_subCur.Subscriber);
			FormFamilyMemberSelect FormF=new FormFamilyMemberSelect(fam);
			FormF.ShowDialog();
			if(FormF.DialogResult!=DialogResult.OK) {
				return;
			}
			_subCur.Subscriber=FormF.SelectedPatNum;
			Patient subsc=Patients.GetLim(FormF.SelectedPatNum);
			textSubscriber.Text=subsc.GetNameLF();
			textSubscriberID.Text=subsc.SSN;
		}

		private void butSearch_Click(object sender,System.EventArgs e) {
			FormElectIDs FormE=new FormElectIDs();
			FormE.IsSelectMode=true;
			FormE.ShowDialog();
			if(FormE.DialogResult!=DialogResult.OK) {
				return;
			}
			textElectID.Text=FormE.selectedID.PayorID;
			_electIdCur=textElectID.Text;
			FillPayor();
			//textElectIDdescript.Text=FormE.selectedID.CarrierName;
		}

		private void FillComboFilingSubtype(long selectedFilingCode) {
			comboFilingCodeSubtype.Items.Clear();
			List<InsFilingCodeSubtype> subtypeListt=InsFilingCodeSubtypes.GetForInsFilingCode(selectedFilingCode);
			for(int i=0;i<subtypeListt.Count;i++) {
				comboFilingCodeSubtype.Items.Add(new ODBoxItem<InsFilingCodeSubtype>(subtypeListt[i].Descript,subtypeListt[i]));
				if(_planCur.FilingCodeSubtype==subtypeListt[i].InsFilingCodeSubtypeNum) {
					comboFilingCodeSubtype.SelectedIndex=i;
				}
			}
		}
		
		private void comboFilingCode_SelectionChangeCommitted(object sender,EventArgs e) {
			FillComboFilingSubtype(((ODBoxItem<InsFilingCode>)comboFilingCode.SelectedItem).Tag.InsFilingCodeNum);
		}

		private void comboBillType_SelectionChangeCommitted(object sender,EventArgs e) {
			_planCur.BillingType=_listBillingTypeDefs[comboBillType.SelectedIndex].DefNum;
		}

		private void butImportTrojan_Click(object sender,System.EventArgs e) {
			//If SubCur is null, this button is not visible to click.
			if(CovCats.GetForEbenCat(EbenefitCategory.Diagnostic)==null
				|| CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive)==null
				|| CovCats.GetForEbenCat(EbenefitCategory.Restorative)==null
				|| CovCats.GetForEbenCat(EbenefitCategory.Endodontics)==null
				|| CovCats.GetForEbenCat(EbenefitCategory.Periodontics)==null
				|| CovCats.GetForEbenCat(EbenefitCategory.Prosthodontics)==null
				|| CovCats.GetForEbenCat(EbenefitCategory.Crowns)==null
				|| CovCats.GetForEbenCat(EbenefitCategory.OralSurgery)==null
				|| CovCats.GetForEbenCat(EbenefitCategory.Orthodontics)==null) 
			{
				MsgBox.Show(this,"You must first set up your insurance categories with corresponding electronic benefit categories: Diagnostic,RoutinePreventive, Restorative, Endodontics, Periodontics, Crowns, OralSurgery, Orthodontics, and Prosthodontics");
				return;
			}
#if DEBUG
			string file=@"C:\Trojan\ETW\Planout.txt";
#else
			RegistryKey regKey=Registry.LocalMachine.OpenSubKey("Software\\TROJAN BENEFIT SERVICE");
			if(regKey==null) {//dmg Unix OS will exit here.
				MessageBox.Show("Trojan not installed properly.");
				return;
			}
			//C:\ETW
			if(regKey.GetValue("INSTALLDIR")==null) {
				MessageBox.Show(@"Registry entry is missing and should be added manually.  LocalMachine\Software\TROJAN BENEFIT SERVICE. StringValue.  Name='INSTALLDIR', value= path where the Trojan program is located.  Full path to directory, without trailing slash.");
				return;
			}
			string file=ODFileUtils.CombinePaths(regKey.GetValue("INSTALLDIR").ToString(),"Planout.txt");
#endif
			if(!File.Exists(file)) {
				MessageBox.Show(file+" not found.  You should export from Trojan first.");
				return;
			}
			TrojanObject troj=Trojan.ProcessTextToObject(File.ReadAllText(file));
			textTrojanID.Text=troj.TROJANID;
			textEmployer.Text=troj.ENAME;
			textGroupName.Text=troj.PLANDESC;
			textPhone.Text=troj.ELIGPHONE;
			textGroupNum.Text=troj.POLICYNO;
			//checkNoSendElect.Checked=!troj.ECLAIMS;//Ignore this.  Even if Trojan says paper, most offices still send by clearinghouse.
			textElectID.Text=troj.PAYERID;
			_electIdCur=textElectID.Text;
			textCarrier.Text=troj.MAILTO;
			textAddress.Text=troj.MAILTOST;
			textCity.Text=troj.MAILCITYONLY;
			textState.Text=troj.MAILSTATEONLY;
			textZip.Text=troj.MAILZIPONLY;
			_planCur.MonthRenew=(byte)troj.MonthRenewal;
			if(_subCur.BenefitNotes!="") {
				_subCur.BenefitNotes+="\r\n--------------------------------\r\n";
			}
			_subCur.BenefitNotes+=troj.BenefitNotes;
			if(troj.PlanNote!=""){
				if(textPlanNote.Text=="") {
					textPlanNote.Text=troj.PlanNote;
				}
				else {//must let user pick final note
					string[] noteArray=new string[2];
					noteArray[0]=textPlanNote.Text;
					noteArray[1]=troj.PlanNote;
					FormNotePick FormN=new FormNotePick(noteArray);
					FormN.UseTrojanImportDescription=true;
					FormN.ShowDialog();
					if(FormN.DialogResult==DialogResult.OK) {
						textPlanNote.Text=FormN.SelectedNote;
					}
				}
			}
			//clear exising benefits from screen, not db:
			benefitList=new List<Benefit>();
			for(int i=0;i<troj.BenefitList.Count;i++){
				//if(fields[2]=="Anniversary year") {
				//	usesAnnivers=true;
				//	MessageBox.Show("Warning.  Plan uses Anniversary year rather than Calendar year.  Please verify the Plan Start Date.");
				//}
				troj.BenefitList[i].PlanNum=_planCur.PlanNum;
				benefitList.Add(troj.BenefitList[i].Copy());
			}
			#if !DEBUG
				File.Delete(file);
			#endif
			butBenefitNotes.Enabled=true;
			FillBenefits();
			/*if(resetFeeSched){
				FeeSchedsStandard=FeeScheds.GetListForType(FeeScheduleType.Normal,false);
				FeeSchedsCopay=FeeScheds.GetListForType(FeeScheduleType.CoPay,false);
				FeeSchedsAllowed=FeeScheds.GetListForType(FeeScheduleType.Allowed,false);
				//if managed care, then do it a bit differently
				comboFeeSched.Items.Clear();
				comboFeeSched.Items.Add(Lan.g(this,"none"));
				comboFeeSched.SelectedIndex=0;
				for(int i=0;i<FeeSchedsStandard.Count;i++) {
					comboFeeSched.Items.Add(FeeSchedsStandard[i].Description);
					if(FeeSchedsStandard[i].FeeSchedNum==feeSchedNum)
						comboFeeSched.SelectedIndex=i+1;
				}
				comboCopay.Items.Clear();
				comboCopay.Items.Add(Lan.g(this,"none"));
				comboCopay.SelectedIndex=0;
				for(int i=0;i<FeeSchedsCopay.Count;i++) {
					comboCopay.Items.Add(FeeSchedsCopay[i].Description);
					//This will get set for managed care
					//if(FeeSchedsCopay[i].DefNum==PlanCur.CopayFeeSched)
					//	comboCopay.SelectedIndex=i+1;
				}
				comboAllowedFeeSched.Items.Clear();
				comboAllowedFeeSched.Items.Add(Lan.g(this,"none"));
				comboAllowedFeeSched.SelectedIndex=0;
				for(int i=0;i<FeeSchedsAllowed.Count;i++) {
					comboAllowedFeeSched.Items.Add(FeeSchedsAllowed[i].Description);
					//I would have set allowed for PPO, but we are probably going to deprecate this when we do coverage tables.
					//if(FeeSchedsAllowed[i].DefNum==PlanCur.AllowedFeeSched)
					//	comboAllowedFeeSched.SelectedIndex=i+1;
				}
			}*/
		}

		private void butIapFind_Click(object sender,System.EventArgs e) {
			//If SubCur is null, this button is not visible to click.
			FormIap FormI=new FormIap();
			FormI.ShowDialog();
			if(FormI.DialogResult==DialogResult.Cancel) {
				return;
			}
			Benefit ben;
			//clear exising benefits from screen, not db:
			benefitList=new List<Benefit>();
			string plan=FormI.selectedPlan;
			string field=null;
			string[] splitField;//if a field is a sentence with more than one word, we can split it for analysis
			int percent;
			try {
				Iap.ReadRecord(plan);
				for(int i=1;i<122;i++) {
					field=Iap.ReadField(i);
					if(field==null){
						field="";
					}
					switch(i) {
						default:
							//do nothing
							break;
						case Iap.Employer:
							if(_subCur.BenefitNotes!="") {
								_subCur.BenefitNotes+="\r\n";
							}
							_subCur.BenefitNotes+="Employer: "+field;
							textEmployer.Text=field;
							break;
						case Iap.Phone:
							_subCur.BenefitNotes+="\r\n"+"Phone: "+field;
							break;
						case Iap.InsUnder:
							_subCur.BenefitNotes+="\r\n"+"InsUnder: "+field;
							break;
						case Iap.Carrier:
							_subCur.BenefitNotes+="\r\n"+"Carrier: "+field;
							textCarrier.Text=field;
							break;
						case Iap.CarrierPh:
							_subCur.BenefitNotes+="\r\n"+"CarrierPh: "+field;
							textPhone.Text=field;
							break;
						case Iap.Group://seems to be used as groupnum
							_subCur.BenefitNotes+="\r\n"+"Group: "+field;
							textGroupNum.Text=field;
							break;
						case Iap.MailTo://the carrier name again
							_subCur.BenefitNotes+="\r\n"+"MailTo: "+field;
							break;
						case Iap.MailTo2://address
							_subCur.BenefitNotes+="\r\n"+"MailTo2: "+field;
							textAddress.Text=field;
							break;
						case Iap.MailTo3://address2
							_subCur.BenefitNotes+="\r\n"+"MailTo3: "+field;
							textAddress2.Text=field;
							break;
						case Iap.EClaims:
							_subCur.BenefitNotes+="\r\n"+"EClaims: "+field;//this contains the PayorID at the end, but also a bunch of other drivel.
							int payorIDloc=field.LastIndexOf("Payor ID#:");
							if(payorIDloc!=-1 && field.Length>payorIDloc+10) {
								textElectID.Text=field.Substring(payorIDloc+10);
								_electIdCur=textElectID.Text;
							}
							break;
						case Iap.FAXClaims:
							_subCur.BenefitNotes+="\r\n"+"FAXClaims: "+field;
							break;
						case Iap.DMOOption:
							_subCur.BenefitNotes+="\r\n"+"DMOOption: "+field;
							break;
						case Iap.Medical:
							_subCur.BenefitNotes+="\r\n"+"Medical: "+field;
							break;
						case Iap.GroupNum://not used.  They seem to use the group field instead
							_subCur.BenefitNotes+="\r\n"+"GroupNum: "+field;
							break;
						case Iap.Phone2://?
							_subCur.BenefitNotes+="\r\n"+"Phone2: "+field;
							break;
						case Iap.Deductible:
							_subCur.BenefitNotes+="\r\n"+"Deductible: "+field;
							if(field.StartsWith("$")) {
								splitField=field.Split(new char[] { ' ' });
								ben=new Benefit();
								ben.BenefitType=InsBenefitType.Deductible;
								ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.General).CovCatNum;
								ben.PlanNum=_planCur.PlanNum;
								ben.TimePeriod=BenefitTimePeriod.CalendarYear;
								ben.MonetaryAmt=PIn.Double(splitField[0].Remove(0,1));//removes the $
								benefitList.Add(ben.Copy());
							}
							break;
						case Iap.FamilyDed:
							_subCur.BenefitNotes+="\r\n"+"FamilyDed: "+field;
							break;
						case Iap.Maximum:
							_subCur.BenefitNotes+="\r\n"+"Maximum: "+field;
							if(field.StartsWith("$")) {
								splitField=field.Split(new char[] { ' ' });
								ben=new Benefit();
								ben.BenefitType=InsBenefitType.Limitations;
								ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.General).CovCatNum;
								ben.PlanNum=_planCur.PlanNum;
								ben.TimePeriod=BenefitTimePeriod.CalendarYear;
								ben.MonetaryAmt=PIn.Double(splitField[0].Remove(0,1));//removes the $
								benefitList.Add(ben.Copy());
							}
							break;
						case Iap.BenefitYear://text is too complex to parse
							_subCur.BenefitNotes+="\r\n"+"BenefitYear: "+field;
							break;
						case Iap.DependentAge://too complex to parse
							_subCur.BenefitNotes+="\r\n"+"DependentAge: "+field;
							break;
						case Iap.Preventive:
							_subCur.BenefitNotes+="\r\n"+"Preventive: "+field;
							splitField=field.Split(new char[] { ' ' });
							if(splitField.Length==0 || !splitField[0].EndsWith("%")) {
								break;
							}
							splitField[0]=splitField[0].Remove(splitField[0].Length-1,1);//remove %
							percent=PIn.Int(splitField[0]);
							if(percent<0 || percent>100) {
								break;
							}
							ben=new Benefit();
							ben.BenefitType=InsBenefitType.CoInsurance;
							ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive).CovCatNum;
							ben.PlanNum=_planCur.PlanNum;
							ben.TimePeriod=BenefitTimePeriod.CalendarYear;
							ben.Percent=percent;
							benefitList.Add(ben.Copy());
							break;
						case Iap.Basic:
							_subCur.BenefitNotes+="\r\n"+"Basic: "+field;
							splitField=field.Split(new char[] { ' ' });
							if(splitField.Length==0 || !splitField[0].EndsWith("%")) {
								break;
							}
							splitField[0]=splitField[0].Remove(splitField[0].Length-1,1);//remove %
							percent=PIn.Int(splitField[0]);
							if(percent<0 || percent>100) {
								break;
							}
							ben=new Benefit();
							ben.BenefitType=InsBenefitType.CoInsurance;
							ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Restorative).CovCatNum;
							ben.PlanNum=_planCur.PlanNum;
							ben.TimePeriod=BenefitTimePeriod.CalendarYear;
							ben.Percent=percent;
							benefitList.Add(ben.Copy());
							ben=new Benefit();
							ben.BenefitType=InsBenefitType.CoInsurance;
							ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Endodontics).CovCatNum;
							ben.PlanNum=_planCur.PlanNum;
							ben.TimePeriod=BenefitTimePeriod.CalendarYear;
							ben.Percent=percent;
							benefitList.Add(ben.Copy());
							ben=new Benefit();
							ben.BenefitType=InsBenefitType.CoInsurance;
							ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Periodontics).CovCatNum;
							ben.PlanNum=_planCur.PlanNum;
							ben.TimePeriod=BenefitTimePeriod.CalendarYear;
							ben.Percent=percent;
							benefitList.Add(ben.Copy());
							ben=new Benefit();
							ben.BenefitType=InsBenefitType.CoInsurance;
							ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.OralSurgery).CovCatNum;
							ben.PlanNum=_planCur.PlanNum;
							ben.TimePeriod=BenefitTimePeriod.CalendarYear;
							ben.Percent=percent;
							benefitList.Add(ben.Copy());
							break;
						case Iap.Major:
							_subCur.BenefitNotes+="\r\n"+"Major: "+field;
							splitField=field.Split(new char[] { ' ' });
							if(splitField.Length==0 || !splitField[0].EndsWith("%")) {
								break;
							}
							splitField[0]=splitField[0].Remove(splitField[0].Length-1,1);//remove %
							percent=PIn.Int(splitField[0]);
							if(percent<0 || percent>100) {
								break;
							}
							ben=new Benefit();
							ben.BenefitType=InsBenefitType.CoInsurance;
							ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Prosthodontics).CovCatNum;//includes crowns?
							ben.PlanNum=_planCur.PlanNum;
							ben.TimePeriod=BenefitTimePeriod.CalendarYear;
							ben.Percent=percent;
							benefitList.Add(ben.Copy());
							break;
						case Iap.InitialPlacement:
							_subCur.BenefitNotes+="\r\n"+"InitialPlacement: "+field;
							break;
						case Iap.ExtractionClause:
							_subCur.BenefitNotes+="\r\n"+"ExtractionClause: "+field;
							break;
						case Iap.Replacement:
							_subCur.BenefitNotes+="\r\n"+"Replacement: "+field;
							break;
						case Iap.Other:
							_subCur.BenefitNotes+="\r\n"+"Other: "+field;
							break;
						case Iap.Orthodontics:
							_subCur.BenefitNotes+="\r\n"+"Orthodontics: "+field;
							splitField=field.Split(new char[] { ' ' });
							if(splitField.Length==0 || !splitField[0].EndsWith("%")) {
								break;
							}
							splitField[0]=splitField[0].Remove(splitField[0].Length-1,1);//remove %
							percent=PIn.Int(splitField[0]);
							if(percent<0 || percent>100) {
								break;
							}
							ben=new Benefit();
							ben.BenefitType=InsBenefitType.CoInsurance;
							ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Orthodontics).CovCatNum;
							ben.PlanNum=_planCur.PlanNum;
							ben.TimePeriod=BenefitTimePeriod.CalendarYear;
							ben.Percent=percent;
							benefitList.Add(ben.Copy());
							break;
						case Iap.Deductible2:
							_subCur.BenefitNotes+="\r\n"+"Deductible2: "+field;
							break;
						case Iap.Maximum2://ortho Max
							_subCur.BenefitNotes+="\r\n"+"Maximum2: "+field;
							if(field.StartsWith("$")) {
								splitField=field.Split(new char[] { ' ' });
								ben=new Benefit();
								ben.BenefitType=InsBenefitType.Limitations;
								ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Orthodontics).CovCatNum;
								ben.PlanNum=_planCur.PlanNum;
								ben.TimePeriod=BenefitTimePeriod.CalendarYear;
								ben.MonetaryAmt=PIn.Double(splitField[0].Remove(0,1));//removes the $
								benefitList.Add(ben.Copy());
							}
							break;
						case Iap.PymtSchedule:
							_subCur.BenefitNotes+="\r\n"+"PymtSchedule: "+field;
							break;
						case Iap.AgeLimit:
							_subCur.BenefitNotes+="\r\n"+"AgeLimit: "+field;
							break;
						case Iap.SignatureonFile:
							_subCur.BenefitNotes+="\r\n"+"SignatureonFile: "+field;
							break;
						case Iap.StandardADAForm:
							_subCur.BenefitNotes+="\r\n"+"StandardADAForm: "+field;
							break;
						case Iap.CoordinationRule:
							_subCur.BenefitNotes+="\r\n"+"CoordinationRule: "+field;
							break;
						case Iap.CoordinationCOB:
							_subCur.BenefitNotes+="\r\n"+"CoordinationCOB: "+field;
							break;
						case Iap.NightguardsforBruxism:
							_subCur.BenefitNotes+="\r\n"+"NightguardsforBruxism: "+field;
							break;
						case Iap.OcclusalAdjustments:
							_subCur.BenefitNotes+="\r\n"+"OcclusalAdjustments: "+field;
							break;
						case Iap.XXXXXX:
							_subCur.BenefitNotes+="\r\n"+"XXXXXX: "+field;
							break;
						case Iap.TMJNonSurgical:
							_subCur.BenefitNotes+="\r\n"+"TMJNonSurgical: "+field;
							break;
						case Iap.Implants:
							_subCur.BenefitNotes+="\r\n"+"Implants: "+field;
							break;
						case Iap.InfectionControl:
							_subCur.BenefitNotes+="\r\n"+"InfectionControl: "+field;
							break;
						case Iap.Cleanings:
							_subCur.BenefitNotes+="\r\n"+"Cleanings: "+field;
							break;
						case Iap.OralEvaluation:
							_subCur.BenefitNotes+="\r\n"+"OralEvaluation: "+field;
							break;
						case Iap.Fluoride1200s:
							_subCur.BenefitNotes+="\r\n"+"Fluoride1200s: "+field;
							break;
						case Iap.Code0220:
							_subCur.BenefitNotes+="\r\n"+"Code0220: "+field;
							break;
						case Iap.Code0272_0274:
							_subCur.BenefitNotes+="\r\n"+"Code0272_0274: "+field;
							break;
						case Iap.Code0210:
							_subCur.BenefitNotes+="\r\n"+"Code0210: "+field;
							break;
						case Iap.Code0330:
							_subCur.BenefitNotes+="\r\n"+"Code0330: "+field;
							break;
						case Iap.SpaceMaintainers:
							_subCur.BenefitNotes+="\r\n"+"SpaceMaintainers: "+field;
							break;
						case Iap.EmergencyExams:
							_subCur.BenefitNotes+="\r\n"+"EmergencyExams: "+field;
							break;
						case Iap.EmergencyTreatment:
							_subCur.BenefitNotes+="\r\n"+"EmergencyTreatment: "+field;
							break;
						case Iap.Sealants1351:
							_subCur.BenefitNotes+="\r\n"+"Sealants1351: "+field;
							break;
						case Iap.Fillings2100:
							_subCur.BenefitNotes+="\r\n"+"Fillings2100: "+field;
							break;
						case Iap.Extractions:
							_subCur.BenefitNotes+="\r\n"+"Extractions: "+field;
							break;
						case Iap.RootCanals:
							_subCur.BenefitNotes+="\r\n"+"RootCanals: "+field;
							break;
						case Iap.MolarRootCanal:
							_subCur.BenefitNotes+="\r\n"+"MolarRootCanal: "+field;
							break;
						case Iap.OralSurgery:
							_subCur.BenefitNotes+="\r\n"+"OralSurgery: "+field;
							break;
						case Iap.ImpactionSoftTissue:
							_subCur.BenefitNotes+="\r\n"+"ImpactionSoftTissue: "+field;
							break;
						case Iap.ImpactionPartialBony:
							_subCur.BenefitNotes+="\r\n"+"ImpactionPartialBony: "+field;
							break;
						case Iap.ImpactionCompleteBony:
							_subCur.BenefitNotes+="\r\n"+"ImpactionCompleteBony: "+field;
							break;
						case Iap.SurgicalProceduresGeneral:
							_subCur.BenefitNotes+="\r\n"+"SurgicalProceduresGeneral: "+field;
							break;
						case Iap.PerioSurgicalPerioOsseous:
							_subCur.BenefitNotes+="\r\n"+"PerioSurgicalPerioOsseous: "+field;
							break;
						case Iap.SurgicalPerioOther:
							_subCur.BenefitNotes+="\r\n"+"SurgicalPerioOther: "+field;
							break;
						case Iap.RootPlaning:
							_subCur.BenefitNotes+="\r\n"+"RootPlaning: "+field;
							break;
						case Iap.Scaling4345:
							_subCur.BenefitNotes+="\r\n"+"Scaling4345: "+field;
							break;
						case Iap.PerioPx:
							_subCur.BenefitNotes+="\r\n"+"PerioPx: "+field;
							break;
						case Iap.PerioComment:
							_subCur.BenefitNotes+="\r\n"+"PerioComment: "+field;
							break;
						case Iap.IVSedation:
							_subCur.BenefitNotes+="\r\n"+"IVSedation: "+field;
							break;
						case Iap.General9220:
							_subCur.BenefitNotes+="\r\n"+"General9220: "+field;
							break;
						case Iap.Relines5700s:
							_subCur.BenefitNotes+="\r\n"+"Relines5700s: "+field;
							break;
						case Iap.StainlessSteelCrowns:
							_subCur.BenefitNotes+="\r\n"+"StainlessSteelCrowns: "+field;
							break;
						case Iap.Crowns2700s:
							_subCur.BenefitNotes+="\r\n"+"Crowns2700s: "+field;
							break;
						case Iap.Bridges6200:
							_subCur.BenefitNotes+="\r\n"+"Bridges6200: "+field;
							break;
						case Iap.Partials5200s:
							_subCur.BenefitNotes+="\r\n"+"Partials5200s: "+field;
							break;
						case Iap.Dentures5100s:
							_subCur.BenefitNotes+="\r\n"+"Dentures5100s: "+field;
							break;
						case Iap.EmpNumberXXX:
							_subCur.BenefitNotes+="\r\n"+"EmpNumberXXX: "+field;
							break;
						case Iap.DateXXX:
							_subCur.BenefitNotes+="\r\n"+"DateXXX: "+field;
							break;
						case Iap.Line4://city state
							_subCur.BenefitNotes+="\r\n"+"Line4: "+field;
							field=field.Replace("  "," ");//get rid of double space before zip
							splitField=field.Split(new char[] { ' ' });
							if(splitField.Length<3) {
								break;
							}
							textCity.Text=splitField[0].Replace(",","");//gets rid of the comma on the end of city
							textState.Text=splitField[1];
							textZip.Text=splitField[2];
							break;
						case Iap.Note:
							_subCur.BenefitNotes+="\r\n"+"Note: "+field;
							break;
						case Iap.Plan://?
							_subCur.BenefitNotes+="\r\n"+"Plan: "+field;
							break;
						case Iap.BuildUps:
							_subCur.BenefitNotes+="\r\n"+"BuildUps: "+field;
							break;
						case Iap.PosteriorComposites:
							_subCur.BenefitNotes+="\r\n"+"PosteriorComposites: "+field;
							break;
					}
				}
				Iap.CloseDatabase();
				butBenefitNotes.Enabled=true;
			}
			catch(ApplicationException ex) {
				Iap.CloseDatabase();
				MessageBox.Show(ex.Message);
			}
			catch(Exception ex) {
				Iap.CloseDatabase();
				MessageBox.Show("Error: "+ex.Message);
			}
			FillBenefits();
		}

		private void EligibilityCheckCanada() {
			if(!FillPlanCurFromForm()) {
				return;
			}
			Carrier carrier=Carriers.GetCarrier(_planCur.CarrierNum);
			if(!carrier.IsCDA){
				MsgBox.Show(this,"Eligibility only supported for CDAnet carriers.");
				return;
			}
			if((carrier.CanadianSupportedTypes & CanSupTransTypes.EligibilityTransaction_08) != CanSupTransTypes.EligibilityTransaction_08) {
				MsgBox.Show(this,"Eligibility not supported by this carrier.");
				return;
			}
			Clearinghouse clearinghouseHq=Canadian.GetCanadianClearinghouseHq(carrier);
			Clearinghouse clearinghouseClin=Clearinghouses.OverrideFields(clearinghouseHq,Clinics.ClinicNum);
			Cursor=Cursors.WaitCursor;
			//string result="";
			DateTime date=DateTime.Today;
#if DEBUG
			date=new DateTime(1999,1,4);//TODO: Remove after Canadian claim certification is complete.
#endif
			Relat relat=(Relat)comboRelationship.SelectedIndex;
			string patID=textPatID.Text;
			try {
				CanadianOutput.SendElegibility(clearinghouseClin,PatPlanCur.PatNum,_planCur,date,relat,patID,true,_subCur,false,FormCCDPrint.PrintCCD);
				//textSubscriberID.Text,textPatID.Text,(Relat)comboRelationship.SelectedIndex,PlanCur.Subscriber,textDentaide.Text);
				//printout will happen in the line above.
			}
			catch(ApplicationException ex) {
				Cursor=Cursors.Default;
				MessageBox.Show(ex.Message);
				return;
			}
			//PlanCur.BenefitNotes+=result;
			//butBenefitNotes.Enabled=true;
			Cursor=Cursors.Default;
			DateTime dateLast270=Etranss.GetLastDate270(_planCur.PlanNum);
			if(dateLast270.Year<1880) {
				textElectBenLastDate.Text="";
			}
			else {
				textElectBenLastDate.Text=dateLast270.ToShortDateString();
			}
		}

		///<summary>This button is only visible if Trojan or IAP is enabled.  Always active.  Button not visible if SubCur==null.</summary>
		private void butBenefitNotes_Click(object sender,System.EventArgs e) {
			string otherBenNote="";
			if(_subCur.BenefitNotes=="") {
				//try to find some other similar notes. Never includes the current subscriber.
				//List<long> samePlans=InsPlans.GetPlanNumsOfSamePlans(textEmployer.Text,textGroupName.Text,textGroupNum.Text,
				//	textDivisionNo.Text,textCarrier.Text,checkIsMedical.Checked,PlanCur.PlanNum,false);
				otherBenNote=InsSubs.GetBenefitNotes(_planCur.PlanNum,_subCur.InsSubNum);
				if(otherBenNote=="") {
					MsgBox.Show(this,"No benefit note found.  Benefit notes are created when importing Trojan or IAP benefit information and are frequently read-only.  Store your own notes in the subscriber note instead.");
					return;
				}
				MsgBox.Show(this,"This plan does not have a benefit note, but a note was found for another subsriber of this plan.  You will be able to view this note, but not change it.");
			}
			FormInsBenefitNotes FormI=new FormInsBenefitNotes();
			if(_subCur.BenefitNotes!="") {
				FormI.BenefitNotes=_subCur.BenefitNotes;
			}
			else {
				FormI.BenefitNotes=otherBenNote;
			}
			FormI.ShowDialog();
			if(FormI.DialogResult==DialogResult.Cancel) {
				return;
			}
			if(_subCur.BenefitNotes!="") {
				_subCur.BenefitNotes=FormI.BenefitNotes;
			}
		}

		private void butDelete_Click(object sender,System.EventArgs e) {
			string logText="";
			//this is a dual purpose button.  It sometimes deletes subscribers (inssubs), and sometimes the plan itself. 
			if(IsNewPlan) {
				DialogResult=DialogResult.Cancel;//original plan will get deleted in closing event.
				return;
			}
			string warningMsg="This plan doesn't have a carrier attached and probably is being created by another user right now.  Click OK to delete plan anyway.";
			if(_carrierCur.CarrierNum==0 && !MsgBox.Show(this,MsgBoxButtons.YesNo,warningMsg)) {
				return;
			}
			//1. Delete Subscriber---------------------------------------------------------------------------------------------------
			//Can only do this if there are other subscribers present.  If this is the last subscriber, then it attempts to delete the plan itself, down below.
			if(comboLinked.Items.Count>0) {//Other subscribers are present.  
				if(_subCur==null) {//viewing from big list
					MsgBox.Show(this,"Subscribers must be removed individually before deleting plan.");//by dropping, then using this same delete button.
					return;
				}
				else {//Came into here through a patient.
					DateTime dateSubChange=_subCur.SecDateTEdit;
					if(PatPlanCur!=null) {
						if(!MsgBox.Show(this,true,"All patients attached to this subscription will be dropped and the subscription for this plan will be deleted. Continue?")) {
							return;
						}
					}
					//drop the plan


					//detach subscriber.
					try {
						InsSubs.Delete(_subCur.InsSubNum);//Checks dependencies first;  If none, deletes the inssub, claimprocs, patplans, and recomputes all estimates.
					}
					catch(ApplicationException ex) {
						MessageBox.Show(ex.Message);
						return;
					}
					logText=Lan.g(this,"The subscriber")+" "+Patients.GetPat(_subCur.Subscriber).GetNameFLnoPref()+" "
						+Lan.g(this,"with the Subscriber ID")+" "+_subCur.SubscriberID+" "+Lan.g(this,"was deleted.");
					//PatPlanCur will be null if editing insurance plans from Lists > Insurance Plans.
					SecurityLogs.MakeLogEntry(Permissions.InsPlanEdit,(PatPlanCur==null)?0:PatPlanCur.PatNum,logText,(_planCur==null)?0:_planCur.PlanNum,
						_planCur.SecDateTEdit);
					DialogResult=DialogResult.OK;
					return;
				}
			}
			//or
			//2. Delete the plan itself-------------------------------------------------------------------------------------------------
			//This is the only subscriber, so delete inssub and insplan
			//Or this is the big list and there are no subscribers, so just delete the insplan.
			if(!MsgBox.Show(this,true,"Delete Plan?")) {
				return;
			}
			DateTime datePrevious=_planCur.SecDateTEdit;
			try {
				InsPlans.Delete(_planCur);//Checks dependencies first;  If none, deletes insplan, inssub, benefits, claimprocs, patplans, and recomputes all estimates.
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			logText=Lan.g(this,"The insurance plan for the carrier")+" "+Carriers.GetCarrier(_planCur.CarrierNum).CarrierName+" "+Lan.g(this,"was deleted.");
			//PatPlanCur will be null if editing insurance plans from Lists > Insurance Plans.
			SecurityLogs.MakeLogEntry(Permissions.InsPlanEdit,(PatPlanCur==null)?0:PatPlanCur.PatNum,logText,(_planCur==null)?0:_planCur.PlanNum,
				datePrevious);
			DialogResult=DialogResult.OK;
		}

		private void butDrop_Click(object sender,System.EventArgs e) {
			DropClickHelper();
		}

		///<summary>Returns true when successfully dropped.</summary>
		private bool DropClickHelper() {
			//Treat the Drop button just like the Delete and Cancel buttons if this is a new plan.
			if(IsNewPlan) {
				DialogResult=DialogResult.Cancel;//original plan will get deleted in closing event.
				return false;
			}
			string warningMsg="This plan doesn't have a carrier attached and probably is being created by another user right now.  Click OK to drop plan anyway.";
			if(_carrierCur.CarrierNum==0 && !MsgBox.Show(this,MsgBoxButtons.YesNo,warningMsg)) {
				return false;
			}
			//should we save the plan info first?  Probably not.
			//--
			//If they have a claim for this ins with today's date, don't let them drop.
			//We already have code in place to delete claimprocs when we drop ins, but the claimprocs attached to claims are protected.
			//The claim clearly needs to be deleted if they are dropping.  We need the user to delete the claim before they drop the plan.
			//We also have code in place to add new claimprocs when they add the correct insurance.
			List<Claim> claimList=Claims.Refresh(PatPlanCur.PatNum);
			for(int j=0;j<claimList.Count;j++) {
				if(claimList[j].PlanNum!=_planCur.PlanNum) {//different insplan
					continue;
				}
				if(claimList[j].DateService!=DateTime.Today) {//not today
					continue;
				}
				//Patient currently has a claim for the insplan they are trying to drop
				MsgBox.Show(this,"Please delete all of today's claims for this patient before dropping this plan.");
				return false;
			}
			PatPlans.Delete(PatPlanCur.PatPlanNum);//Estimates recomputed within Delete()
			//PlanCur.ComputeEstimatesForCur();
			_hasDropped=true;
			string logText=Lan.g(this,"The insurance plan for the carrier")+" "+Carriers.GetCarrier(_planCur.CarrierNum).CarrierName+" "+Lan.g(this,"was dropped.");
			SecurityLogs.MakeLogEntry(Permissions.InsPlanEdit,(PatPlanCur==null)?0:PatPlanCur.PatNum,logText,(_planCur==null)?0:_planCur.PlanNum,
				_planCur.SecDateTEdit);
			DialogResult=DialogResult.OK;
			return true;
		}

		private void butLabel_Click(object sender,System.EventArgs e) {
			//LabelSingle label=new LabelSingle();
			PrintDocument pd=new PrintDocument();//only used to pass printerName
			long patNumCur = PatPlanCur!=null ? PatPlanCur.PatNum : 0;
			if(!PrinterL.SetPrinter(pd,PrintSituation.LabelSingle,patNumCur,textCarrier.Text+" insurance plan label printed")) {
				return;
			}
			Carrier carrier=new Carrier();
			carrier.CarrierName=textCarrier.Text;
			carrier.Phone=textPhone.Text;
			carrier.Address=textAddress.Text;
			carrier.Address2=textAddress2.Text;
			carrier.City=textCity.Text;
			carrier.State=textState.Text;
			carrier.Zip=textZip.Text;
			carrier.ElectID=textElectID.Text;
			carrier.NoSendElect=checkNoSendElect.Checked;
			try {
				carrier=Carriers.GetIdentical(carrier);
			}
			catch(ApplicationException ex) {
				//the catch is just to display a message to the user.  It doesn't affect the success of the function.
				MessageBox.Show(ex.Message);
			}	
			LabelSingle.PrintCarrier(carrier.CarrierNum);//,pd.PrinterSettings.PrinterName);
		}

		///<summary>This only fills the grid on the screen.  It does not get any data from the database.</summary>
		private void FillBenefits() {
			benefitList.Sort();
			gridBenefits.BeginUpdate();
			gridBenefits.Columns.Clear();
			ODGridColumn col=new ODGridColumn("Pat",28);
			gridBenefits.Columns.Add(col);
			col=new ODGridColumn("Level",60);
			gridBenefits.Columns.Add(col);
			col=new ODGridColumn("Type",70);
			gridBenefits.Columns.Add(col);
			col=new ODGridColumn("Category",70);
			gridBenefits.Columns.Add(col);
			col=new ODGridColumn("%",30);//,HorizontalAlignment.Right);
			gridBenefits.Columns.Add(col);
			col=new ODGridColumn("Amt",40);//,HorizontalAlignment.Right);
			gridBenefits.Columns.Add(col);
			col=new ODGridColumn("Time Period",80);
			gridBenefits.Columns.Add(col);
			col=new ODGridColumn("Quantity",115);
			gridBenefits.Columns.Add(col);
			gridBenefits.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<benefitList.Count;i++) {
				row=new ODGridRow();
				if(benefitList[i].PatPlanNum==0) {//attached to plan
					row.Cells.Add("");
				}
				else {
					row.Cells.Add("X");
				}
				if(benefitList[i].CoverageLevel==BenefitCoverageLevel.None) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(Lan.g("enumBenefitCoverageLevel",benefitList[i].CoverageLevel.ToString()));
				}
				if(benefitList[i].BenefitType==InsBenefitType.CoInsurance && benefitList[i].Percent != -1) {
					row.Cells.Add("%");
				}
				else {
					row.Cells.Add(Lan.g("enumInsBenefitType",benefitList[i].BenefitType.ToString()));
				}
				row.Cells.Add(Benefits.GetCategoryString(benefitList[i])); //already translated
				if(benefitList[i].Percent==-1 ) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(benefitList[i].Percent.ToString());
				}
				if(benefitList[i].MonetaryAmt == -1) {
					//if(((Benefit)benefitList[i]).BenefitType==InsBenefitType.Deductible) {
					//	row.Cells.Add(((Benefit)benefitList[i]).MonetaryAmt.ToString("n0"));
					//}
					//else {
					row.Cells.Add("");
					//}
				}
				else {
					row.Cells.Add(benefitList[i].MonetaryAmt.ToString("n0"));
				}
				if(benefitList[i].TimePeriod==BenefitTimePeriod.None) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(Lan.g("enumBenefitTimePeriod",benefitList[i].TimePeriod.ToString()));
				}
				if(benefitList[i].Quantity>0) {
					if(benefitList[i].QuantityQualifier==BenefitQuantity.NumberOfServices
						&&(benefitList[i].TimePeriod==BenefitTimePeriod.ServiceYear
						|| benefitList[i].TimePeriod==BenefitTimePeriod.CalendarYear))
					{
						row.Cells.Add(benefitList[i].Quantity.ToString()+" "+Lan.g(this,"times per year")+" ");
					}
					else {
						row.Cells.Add(benefitList[i].Quantity.ToString()+" "
							+Lan.g("enumBenefitQuantity",benefitList[i].QuantityQualifier.ToString()));
					}
				}
				else {
					row.Cells.Add("");
				}
				gridBenefits.Rows.Add(row);
			}
			gridBenefits.EndUpdate();
			/*if(allCalendarYear){
				checkCalendarYear.CheckState=CheckState.Checked;
			}
			else if(allServiceYear){
				checkCalendarYear.CheckState=CheckState.Unchecked;
			}
			else{
				checkCalendarYear.CheckState=CheckState.Indeterminate;
			}*/
		}

		private void gridBenefits_DoubleClick(object sender,EventArgs e) {
			if(IsNewPlan && _planCur.PlanNum != _planOld.PlanNum) {  //If adding a new plan and picked existing plan from list
				//==Travis 05/06/2015:  Allowing users to edit insurance benefits for new plans that were picked from the list was causing problems with 
				//	duplicating benefits.  This was the fix we decided to go with, as the issue didn't seem to be affecting existing plans for a patient.
				MessageBox.Show(Lan.g(this,"You have picked an existing insurance plan and changes cannot be made to benefits until you have saved the plan for this new subscriber.")
					+"\r\n"+Lan.g(this,"To edit, click OK and then open the edit insurance plan window again."));
				return;
			}
			long patPlanNum=0;
			if(PatPlanCur!=null) {
				patPlanNum=PatPlanCur.PatPlanNum;
			}
			FormInsBenefits FormI=new FormInsBenefits(_planCur.PlanNum,patPlanNum);
			FormI.OriginalBenList=benefitList;
			FormI.Note=textSubscNote.Text;
			FormI.MonthRenew=_planCur.MonthRenew;
			FormI.SubCur=_subCur;
			FormI.ShowDialog();
			if(FormI.DialogResult!=DialogResult.OK) {
				return;
			}
			FillBenefits();
			textSubscNote.Text=FormI.Note;
			_planCur.MonthRenew=FormI.MonthRenew;
		}

		///<summary>Gets an employerNum based on the name entered. Called from FillCur</summary>
		private void GetEmployerNum() {
			if(_planCur.EmployerNum==0) {//no employer was previously entered.
				if(textEmployer.Text=="") {
					//no change - Use what's in the database if they truly didn't change anything (PlanCur has no emp, text is blank, and text was always blank, they didn't switch insplans)
					if(PatPlanCur!=null && _employerNameOrig=="" && _planCur.PlanNum==_planCurOriginal.PlanNum) {
						PatPlan patPlanDB=PatPlans.GetByPatPlanNum(PatPlanCur.PatPlanNum);
						InsSub insSubDB=InsSubs.GetOne(patPlanDB.InsSubNum);
						InsPlan insPlanDB=InsPlans.GetPlan(insSubDB.PlanNum,null);
						_planCur.EmployerNum=insPlanDB.EmployerNum;
						_planCurOriginal.EmployerNum=insPlanDB.EmployerNum;
					}
				}
				else {
					_planCur.EmployerNum=Employers.GetEmployerNum(textEmployer.Text);
				}
			}
			else {//an employer was previously entered
				if(textEmployer.Text=="") {
					_planCur.EmployerNum=0;
				}
				//if text has changed - 
				else if(_employerNameOrig!=textEmployer.Text) {
					_planCur.EmployerNum=Employers.GetEmployerNum(textEmployer.Text);
				}
				else {
					//no change - Use what's in the database
					if(PatPlanCur!=null) {
						PatPlan patPlanDB=PatPlans.GetByPatPlanNum(PatPlanCur.PatPlanNum);
						InsSub insSubDB=InsSubs.GetOne(patPlanDB.InsSubNum);
						InsPlan insPlanDB=InsPlans.GetPlan(insSubDB.PlanNum,null);
						_planCur.EmployerNum=insPlanDB.EmployerNum;
						_planCurOriginal.EmployerNum=insPlanDB.EmployerNum;
					}
				}
			}
		}

		private void butGetElectronic_Click(object sender,EventArgs e) {
			//button not visible if SubCur is null
			if(PrefC.GetBool(PrefName.CustomizedForPracticeWeb)) {
				EligibilityCheckDentalXchange();
				return;
			}
			//Visible for everyone.
			Clearinghouse clearinghouseHq=Clearinghouses.GetDefaultEligibility();
			if(clearinghouseHq==null){
				MsgBox.Show(this,"No clearinghouse is set as default.");
				return;
			}
			if((clearinghouseHq.CommBridge!=EclaimsCommBridge.ClaimConnect 
				&& clearinghouseHq.CommBridge!=EclaimsCommBridge.EDS
				&& clearinghouseHq.CommBridge!=EclaimsCommBridge.WebMD)
				&& clearinghouseHq.Eformat!=ElectronicClaimFormat.Canadian)
			{
				MsgBox.Show(this,"So far, eligibility checks only work with ClaimConnect, EDS, WebMD (Emdeon Dental), and CDAnet.");
				return;
			}
			if(clearinghouseHq.Eformat==ElectronicClaimFormat.Canadian) {
				EligibilityCheckCanada();
				return;
			}
			//Validate the 271 settings before sending the request, otherwise the request might take 10-20 seconds to run, then the user might be blocked after waiting.
			//It is nicer to the user to not make them wait when they can fix the settings beforehand.
			string settingErrors271=X271.ValidateSettings();
			if(settingErrors271!="") {
				MessageBox.Show(settingErrors271);
				return;
			}
			if(!FillPlanCurFromForm()) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			try {
				Clearinghouse clearinghouseClin=Clearinghouses.OverrideFields(clearinghouseHq,Clinics.ClinicNum);
				Etrans etrans=x270Controller.RequestBenefits(clearinghouseClin,_planCur,PatPlanCur.PatNum,_carrierCur,benefitList,
					PatPlanCur.PatPlanNum,_subCur);
				if(etrans != null) {
					//show the user a list of benefits to pick from for import--------------------------
					bool isDependentRequest=(PatPlanCur.PatNum!=_subCur.Subscriber);
					Carrier carrierCur=Carriers.GetCarrier(_planCur.CarrierNum);
					FormEtrans270Edit formE=new FormEtrans270Edit(PatPlanCur.PatPlanNum,_planCur.PlanNum,_subCur.InsSubNum,isDependentRequest,_subCur.Subscriber,carrierCur.IsCoinsuranceInverted);
					formE.EtransCur=etrans;
					formE.IsInitialResponse=true;
					formE.benList=benefitList;
					if(formE.ShowDialog()==DialogResult.OK) {
						#region Plan Notes
						string patName=Patients.GetNameLF(PatPlanCur.PatNum);
						DateTime planEndDate=DateTime.MinValue;
						List<DTP271> listDates=formE.ListDTP;
						foreach(DTP271 date in listDates) {
							string dtpDateStr=DTP271.GetDateStr(date.Segment.Get(2),date.Segment.Get(3));
							if(date.Segment.Get(1)=="347") {//347 => Plan End
								planEndDate=X12Parse.ToDate(date.Segment.Get(3));
								if(!isDependentRequest) {
									textDateTerm.Text=dtpDateStr;
								}
							}
							if(isDependentRequest || date.Segment.Get(1)!="347"){
								string dtpDescript=DTP271.GetQualifierDescript(date.Segment.Get(1));
								string note="As of "+DateTime.Today.ToShortDateString()+" - "+patName+": "+Lan.g(this,dtpDescript)+", "+dtpDateStr+"\n";
								textSubscNote.Text=textSubscNote.Text.Insert(0,note);
							}
						}
						#endregion Plan Notes
						#region Drop plan and add popup
						if(isDependentRequest
							&& planEndDate.Year > 1900 && planEndDate < DateTime.Today
							&& MsgBox.Show(this,true,"The plan has ended.  Would you like to drop this plan?"))
						{
							if(DropClickHelper()
								&& MsgBox.Show(this,true,"Would you like to add a popup to collect new insurance information from patient?"))
							{
								Popup popup=new Popup();
								popup.PatNum=PatPlanCur.PatNum;
								popup.PopupLevel=EnumPopupLevel.Patient;
								popup.IsNew=true;
								popup.Description=Lan.g(this,"Insurance expired.  Collect new insurance information.");
								FormPopupEdit FormPE=new FormPopupEdit();
								FormPE.PopupCur=popup;
								FormPE.ShowDialog();
							}
						}
						#endregion Drop plan and add popup
					}
				}
			}
			catch(Exception ex) {//although many errors will be caught and result in a response etrans.
				//this also catches validation errors such as missing info.
				Cursor=Cursors.Default;
				if(ex.Message.Contains("AAA*N**79*")){
					MsgBox.Show(this,"There is a problem with your benefits request. Check with your clearinghouse to ensure"
						+" they support Real Time Eligibility for this carrier and verify that the correct electronic ID is entered.");
				}
				else{
					CodeBase.MsgBoxCopyPaste msgbox=new CodeBase.MsgBoxCopyPaste(ex.Message);
					msgbox.ShowDialog();
				}
			}
			Cursor=Cursors.Default;
			DateTime dateLast270=Etranss.GetLastDate270(_planCur.PlanNum);
			if(dateLast270.Year<1880) {
				textElectBenLastDate.Text="";
			}
			else {
				textElectBenLastDate.Text=dateLast270.ToShortDateString();
			}
			FillBenefits();
		}

		private void butHistoryElect_Click(object sender,EventArgs e) {
			//button not visible if SubCur is null
			FormBenefitElectHistory formB=new FormBenefitElectHistory(_planCur.PlanNum,PatPlanCur.PatPlanNum,_subCur.InsSubNum,_subCur.Subscriber,_planCur.CarrierNum);
			formB.BenList=benefitList;
			formB.ShowDialog();
			DateTime dateLast270=Etranss.GetLastDate270(_planCur.PlanNum);
			if(dateLast270.Year<1880) {
				textElectBenLastDate.Text="";
			}
			else {
				textElectBenLastDate.Text=dateLast270.ToShortDateString();
			}
			FillBenefits();
		}

		#region EligibilityCheckDentalXchange
		//This is not our code.   Added SPK/AAD 10/06 for eligibility check.-------------------------------------------------------------------------
		private void EligibilityCheckDentalXchange() {
			Cursor = Cursors.WaitCursor;
			OpenDental.com.dentalxchange.webservices.WebServiceService DCIService 
				= new OpenDental.com.dentalxchange.webservices.WebServiceService();
			OpenDental.com.dentalxchange.webservices.Credentials DCICredential 
				= new OpenDental.com.dentalxchange.webservices.Credentials();
			OpenDental.com.dentalxchange.webservices.Request DCIRequest = new OpenDental.com.dentalxchange.webservices.Request();
			OpenDental.com.dentalxchange.webservices.Response DCIResponse = new OpenDental.com.dentalxchange.webservices.Response();
			string loginID;
			string passWord;
			// Get Login / Password
			Clearinghouse clearinghouseHq=Clearinghouses.GetDefaultDental();
			Clearinghouse clearinghouseClin=Clearinghouses.OverrideFields(clearinghouseHq,Clinics.ClinicNum);
			if(clearinghouseClin!=null) {
				loginID=clearinghouseClin.LoginID;
				passWord=clearinghouseClin.Password;
			}
			else {
				loginID = "";
				passWord = "";
			}
			if(loginID == "") {
				MessageBox.Show("ClaimConnect login ID and password are required to check eligibility.");
				Cursor = Cursors.Default;
				return;
			}
			// Set Credentials
			DCICredential.serviceID = "DCI Web Service ID: 001513";
			DCICredential.username = loginID;   // ABCuser
			DCICredential.password = passWord;  // testing1
			DCICredential.client = "Practice-Web";
			DCICredential.version = "1";
			// Set Request Document
			//textAddress.Text = PrepareEligibilityRequest();
			DCIRequest.content = PrepareEligibilityRequestDentalXchange(loginID,passWord);
			try {
				DCIResponse = DCIService.lookupEligibility(DCICredential,DCIRequest);
				//DisplayEligibilityStatus();
				ProcessEligibilityResponseDentalXchange(DCIResponse.content.ToString());
			}
			catch{//Exception ex) {
				// SPK /AAD 8/16/08 Display more user friendly error message
				MessageBox.Show("Error : Inadequate data for response. Payer site may be unavailable.");
			}
			Cursor = Cursors.Default;
		}

		private string PrepareEligibilityRequestDentalXchange(string loginID,string passWord) {
			DataTable table;
			string infoReceiverLastName;
			string infoReceiverFirstName;
			string practiceAddress1;
			string practiceAddress2;
			string practicePhone;
			string practiceCity;
			string practiceState;
			string practiceZip;
			string renderingProviderLastName;
			string renderingProviderFirstName;
			string GenderCode;
			string TaxoCode;
			string RelationShip;
			XmlDocument doc = new XmlDocument();
			XmlNode EligNode = doc.CreateNode(XmlNodeType.Element,"EligRequest","");
			doc.AppendChild(EligNode);
			// Prepare Namespace Attribute
			XmlAttribute nameSpaceAttribute = doc.CreateAttribute("xmlns","xsi","http://www.w3.org/2000/xmlns/");
			nameSpaceAttribute.Value = "http://www.w3.org/2001/XMLSchema-instance";
			doc.DocumentElement.SetAttributeNode(nameSpaceAttribute);
			// Prepare noNamespace Schema Location Attribute
			XmlAttribute noNameSpaceSchemaLocation = doc.CreateAttribute("xsi","noNamespaceSchemaLocation","http://www.w3.org/2001/XMLSchema-instance");
			//dmg Not sure what this is for. This path will not exist on Unix and will fail. In fact, this path
			//will either not exist or be read-only on most Windows boxes, so this path specification is probably
			//a bug, but has not caused any user complaints thus far.
			noNameSpaceSchemaLocation.Value = @"D:\eligreq.xsd";
			doc.DocumentElement.SetAttributeNode(noNameSpaceSchemaLocation);
			//  Prepare AuthInfo Node
			XmlNode AuthInfoNode = doc.CreateNode(XmlNodeType.Element,"AuthInfo","");
			//  Create UserName / Password ChildNode for AuthInfoNode
			XmlNode UserName = doc.CreateNode(XmlNodeType.Element,"UserName","");
			XmlNode Password = doc.CreateNode(XmlNodeType.Element,"Password","");
			//  Set Value of UserID / Password
			UserName.InnerText = loginID;
			Password.InnerText = passWord;
			//  Append UserName / Password to AuthInfoNode
			AuthInfoNode.AppendChild(UserName);
			AuthInfoNode.AppendChild(Password);
			//  Append AuthInfoNode To EligNode
			EligNode.AppendChild(AuthInfoNode);
			//  Prepare Information Receiver Node
			XmlNode InfoReceiver = doc.CreateNode(XmlNodeType.Element,"InformationReceiver","");
			XmlNode InfoAddress = doc.CreateNode(XmlNodeType.Element,"Address","");
			XmlNode InfoAddressName = doc.CreateNode(XmlNodeType.Element,"Name","");
			XmlNode InfoAddressFirstName = doc.CreateNode(XmlNodeType.Element,"FirstName","");
			XmlNode InfoAddressLastName = doc.CreateNode(XmlNodeType.Element,"LastName","");
			// Get Provider Information
			table = Providers.GetDefaultPracticeProvider2();
			if(table.Rows.Count != 0) {
				infoReceiverFirstName = PIn.String(table.Rows[0][0].ToString());
				infoReceiverLastName = PIn.String(table.Rows[0][1].ToString());
				// Case statement for TaxoCode
				switch(PIn.Long(table.Rows[0][2].ToString())) {
					case 1:
						TaxoCode = "124Q00000X";
						break;
					case 2:
						TaxoCode = "1223D0001X";
						break;
					case 3:
						TaxoCode = "1223E0200X";
						break;
					case 4:
						TaxoCode = "1223P0106X";
						break;
					case 5:
						TaxoCode = "1223D0008X";
						break;
					case 6:
						TaxoCode = "1223S0112X";
						break;
					case 7:
						TaxoCode = "1223X0400X";
						break;
					case 8:
						TaxoCode = "1223P0221X";
						break;
					case 9:
						TaxoCode = "1223P0300X";
						break;
					case 10:
						TaxoCode = "1223P0700X";
						break;
					default:
						TaxoCode = "1223G0001X";
						break;
				}
			}
			else {
				infoReceiverFirstName = "Unknown";
				infoReceiverLastName = "Unknown";
				TaxoCode = "Unknown";
			};
			InfoAddressFirstName.InnerText = infoReceiverLastName;
			InfoAddressLastName.InnerText = infoReceiverFirstName;
			InfoAddressName.AppendChild(InfoAddressFirstName);
			InfoAddressName.AppendChild(InfoAddressLastName);
			XmlNode InfoAddressLine1 = doc.CreateNode(XmlNodeType.Element,"AddressLine1","");
			XmlNode InfoAddressLine2 = doc.CreateNode(XmlNodeType.Element,"AddressLine2","");
			XmlNode InfoPhone = doc.CreateNode(XmlNodeType.Element,"Phone","");
			XmlNode InfoCity = doc.CreateNode(XmlNodeType.Element,"City","");
			XmlNode InfoState = doc.CreateNode(XmlNodeType.Element,"State","");
			XmlNode InfoZip = doc.CreateNode(XmlNodeType.Element,"Zip","");
			//  Populate Practioner demographic from hash table
			practiceAddress1 = PrefC.GetString(PrefName.PracticeAddress);
			practiceAddress2 = PrefC.GetString(PrefName.PracticeAddress2);
			// Format Phone
			if(PrefC.GetString(PrefName.PracticePhone).Length == 10) {
				practicePhone = PrefC.GetString(PrefName.PracticePhone).Substring(0,3)
                                    + "-" + PrefC.GetString(PrefName.PracticePhone).Substring(3,3)
                                    + "-" + PrefC.GetString(PrefName.PracticePhone).Substring(6);
			}
			else {
				practicePhone = PrefC.GetString(PrefName.PracticePhone);
			}
			practiceCity = PrefC.GetString(PrefName.PracticeCity);
			practiceState = PrefC.GetString(PrefName.PracticeST);
			practiceZip = PrefC.GetString(PrefName.PracticeZip);
			InfoAddressLine1.InnerText = practiceAddress1;
			InfoAddressLine2.InnerText = practiceAddress2;
			InfoPhone.InnerText = practicePhone;
			InfoCity.InnerText = practiceCity;
			InfoState.InnerText = practiceState;
			InfoZip.InnerText = practiceZip;
			InfoAddress.AppendChild(InfoAddressName);
			InfoAddress.AppendChild(InfoAddressLine1);
			InfoAddress.AppendChild(InfoAddressLine2);
			InfoAddress.AppendChild(InfoPhone);
			InfoAddress.AppendChild(InfoCity);
			InfoAddress.AppendChild(InfoState);
			InfoAddress.AppendChild(InfoZip);
			InfoReceiver.AppendChild(InfoAddress);
			//SPK / AAD 8/13/08 Add NPI -- Begin
			XmlNode InfoReceiverProviderNPI = doc.CreateNode(XmlNodeType.Element,"NPI","");
			//Get Provider NPI #
			table = Providers.GetDefaultPracticeProvider3();
			if(table.Rows.Count != 0) {
				InfoReceiverProviderNPI.InnerText = PIn.String(table.Rows[0][0].ToString());
			};
			InfoReceiver.AppendChild(InfoReceiverProviderNPI);
			//SPK / AAD 8/13/08 Add NPI -- End
			XmlNode InfoCredential = doc.CreateNode(XmlNodeType.Element,"Credential","");
			XmlNode InfoCredentialType = doc.CreateNode(XmlNodeType.Element,"Type","");
			XmlNode InfoCredentialValue = doc.CreateNode(XmlNodeType.Element,"Value","");
			InfoCredentialType.InnerText = "TJ";
			InfoCredentialValue.InnerText = "123456789";
			InfoCredential.AppendChild(InfoCredentialType);
			InfoCredential.AppendChild(InfoCredentialValue);
			InfoReceiver.AppendChild(InfoCredential);
			XmlNode InfoTaxonomyCode = doc.CreateNode(XmlNodeType.Element,"TaxonomyCode","");
			InfoTaxonomyCode.InnerText = TaxoCode;
			InfoReceiver.AppendChild(InfoTaxonomyCode);
			//  Append InfoReceiver To EligNode
			EligNode.AppendChild(InfoReceiver);
			//  Payer Info
			XmlNode InfoPayer = doc.CreateNode(XmlNodeType.Element,"Payer","");
			XmlNode InfoPayerNEIC = doc.CreateNode(XmlNodeType.Element,"PayerNEIC","");
			InfoPayerNEIC.InnerText = textElectID.Text;
			InfoPayer.AppendChild(InfoPayerNEIC);
			EligNode.AppendChild(InfoPayer);
			//  Patient
			XmlNode Patient = doc.CreateNode(XmlNodeType.Element,"Patient","");
			XmlNode PatientName = doc.CreateNode(XmlNodeType.Element,"Name","");
			XmlNode PatientFirstName = doc.CreateNode(XmlNodeType.Element,"FirstName","");
			XmlNode PatientLastName = doc.CreateNode(XmlNodeType.Element,"LastName","");
			XmlNode PatientDOB = doc.CreateNode(XmlNodeType.Element,"DOB","");
			XmlNode PatientSubscriber = doc.CreateNode(XmlNodeType.Element,"SubscriberID","");
			XmlNode PatientRelationship = doc.CreateNode(XmlNodeType.Element,"RelationshipCode","");
			XmlNode PatientGender = doc.CreateNode(XmlNodeType.Element,"Gender","");
			// Read Patient FName,LName,DOB, and Gender from Patient Table
			table = Patients.GetPartialPatientData(PatPlanCur.PatNum);
			if(table.Rows.Count != 0) {
				PatientFirstName.InnerText = PIn.String(table.Rows[0][0].ToString());
				PatientLastName.InnerText = PIn.String(table.Rows[0][1].ToString());
				PatientDOB.InnerText = PIn.String(table.Rows[0][2].ToString());
				switch(comboRelationship.Text) {
					case "Self":
						RelationShip = "18";
						break;
					case "Spouse":
						RelationShip = "01";
						break;
					case "Child":
						RelationShip = "19";
						break;
					default:
						RelationShip = "34";
						break;
				}
				switch(PIn.String(table.Rows[0][3].ToString())) {
					case "1":
						GenderCode = "F";
						break;
					default:
						GenderCode = "M";
						break;
				}
			}
			else {
				PatientFirstName.InnerText = "Unknown";
				PatientLastName.InnerText = "Unknown";
				PatientDOB.InnerText = "99/99/9999";
				RelationShip = "??";
				GenderCode = "?";
			}
			PatientName.AppendChild(PatientFirstName);
			PatientName.AppendChild(PatientLastName);
			PatientSubscriber.InnerText = textSubscriberID.Text;
			PatientRelationship.InnerText = RelationShip;
			PatientGender.InnerText = GenderCode;
			Patient.AppendChild(PatientName);
			Patient.AppendChild(PatientDOB);
			Patient.AppendChild(PatientSubscriber);
			Patient.AppendChild(PatientRelationship);
			Patient.AppendChild(PatientGender);
			EligNode.AppendChild(Patient);
			//  Subscriber
			XmlNode Subscriber = doc.CreateNode(XmlNodeType.Element,"Subscriber","");
			XmlNode SubscriberName = doc.CreateNode(XmlNodeType.Element,"Name","");
			XmlNode SubscriberFirstName = doc.CreateNode(XmlNodeType.Element,"FirstName","");
			XmlNode SubscriberLastName = doc.CreateNode(XmlNodeType.Element,"LastName","");
			XmlNode SubscriberDOB = doc.CreateNode(XmlNodeType.Element,"DOB","");
			XmlNode SubscriberSubscriber = doc.CreateNode(XmlNodeType.Element,"SubscriberID","");
			XmlNode SubscriberRelationship = doc.CreateNode(XmlNodeType.Element,"RelationshipCode","");
			XmlNode SubscriberGender = doc.CreateNode(XmlNodeType.Element,"Gender","");
			// Read Subscriber FName,LName,DOB, and Gender from Patient Table
			table=Patients.GetPartialPatientData2(PatPlanCur.PatNum);
			if(table.Rows.Count != 0) {
				SubscriberFirstName.InnerText = PIn.String(table.Rows[0][0].ToString());
				SubscriberLastName.InnerText = PIn.String(table.Rows[0][1].ToString());
				SubscriberDOB.InnerText = PIn.String(table.Rows[0][2].ToString());
				switch(PIn.String(table.Rows[0][3].ToString())) {
					case "1":
						GenderCode = "F";
						break;
					default:
						GenderCode = "M";
						break;
				}
			}
			else {
				SubscriberFirstName.InnerText = "Unknown";
				SubscriberLastName.InnerText = "Unknown";
				SubscriberDOB.InnerText = "99/99/9999";
				GenderCode = "?";
			}
			SubscriberName.AppendChild(SubscriberFirstName);
			SubscriberName.AppendChild(SubscriberLastName);
			SubscriberSubscriber.InnerText = textSubscriberID.Text;
			SubscriberRelationship.InnerText = RelationShip;
			SubscriberGender.InnerText = GenderCode;
			Subscriber.AppendChild(SubscriberName);
			Subscriber.AppendChild(SubscriberDOB);
			Subscriber.AppendChild(SubscriberSubscriber);
			Subscriber.AppendChild(SubscriberRelationship);
			Subscriber.AppendChild(SubscriberGender);
			EligNode.AppendChild(Subscriber);
			//  Prepare Information Receiver Node
			XmlNode RenderingProvider = doc.CreateNode(XmlNodeType.Element,"RenderingProvider","");
			// SPK / AAD 8/13/08 Add Rendering Provider NPI It is same as Info Receiver NPI -- Start
			XmlNode RenderingProviderNPI = doc.CreateNode(XmlNodeType.Element,"NPI","");
			// SPK / AAD 8/13/08 Add Rendering Provider NPI It is same as Info Receiver NPI -- End
			XmlNode RenderingAddress = doc.CreateNode(XmlNodeType.Element,"Address","");
			XmlNode RenderingAddressName = doc.CreateNode(XmlNodeType.Element,"Name","");
			XmlNode RenderingAddressFirstName = doc.CreateNode(XmlNodeType.Element,"FirstName","");
			XmlNode RenderingAddressLastName = doc.CreateNode(XmlNodeType.Element,"LastName","");
			// Get Rendering Provider first and lastname
			// Read Patient FName,LName,DOB, and Gender from Patient Table
			table=Providers.GetPrimaryProviders(PatPlanCur.PatNum);
			if(table.Rows.Count != 0) {
				renderingProviderFirstName = PIn.String(table.Rows[0][0].ToString());
				renderingProviderLastName = PIn.String(table.Rows[0][1].ToString());
			}
			else {
				renderingProviderFirstName = infoReceiverFirstName;
				renderingProviderLastName = infoReceiverLastName;
			};
			RenderingAddressFirstName.InnerText = renderingProviderFirstName;
			RenderingAddressLastName.InnerText = renderingProviderLastName;
			RenderingAddressName.AppendChild(RenderingAddressFirstName);
			RenderingAddressName.AppendChild(RenderingAddressLastName);
			XmlNode RenderingAddressLine1 = doc.CreateNode(XmlNodeType.Element,"AddressLine1","");
			XmlNode RenderingAddressLine2 = doc.CreateNode(XmlNodeType.Element,"AddressLine2","");
			XmlNode RenderingPhone = doc.CreateNode(XmlNodeType.Element,"Phone","");
			XmlNode RenderingCity = doc.CreateNode(XmlNodeType.Element,"City","");
			XmlNode RenderingState = doc.CreateNode(XmlNodeType.Element,"State","");
			XmlNode RenderingZip = doc.CreateNode(XmlNodeType.Element,"Zip","");
			RenderingProviderNPI.InnerText = InfoReceiverProviderNPI.InnerText;
			RenderingAddressLine1.InnerText = practiceAddress1;
			RenderingAddressLine2.InnerText = practiceAddress2;
			RenderingPhone.InnerText = practicePhone;
			RenderingCity.InnerText = practiceCity;
			RenderingState.InnerText = practiceState;
			RenderingZip.InnerText = practiceZip;
			RenderingAddress.AppendChild(RenderingAddressName);
			RenderingAddress.AppendChild(RenderingAddressLine1);
			RenderingAddress.AppendChild(RenderingAddressLine2);
			RenderingAddress.AppendChild(RenderingPhone);
			RenderingAddress.AppendChild(RenderingCity);
			RenderingAddress.AppendChild(RenderingState);
			RenderingAddress.AppendChild(RenderingZip);
			XmlNode RenderingCredential = doc.CreateNode(XmlNodeType.Element,"Credential","");
			XmlNode RenderingCredentialType = doc.CreateNode(XmlNodeType.Element,"Type","");
			XmlNode RenderingCredentialValue = doc.CreateNode(XmlNodeType.Element,"Value","");
			RenderingCredentialType.InnerText = "TJ";
			RenderingCredentialValue.InnerText = "123456789";
			RenderingCredential.AppendChild(RenderingCredentialType);
			RenderingCredential.AppendChild(RenderingCredentialValue);
			XmlNode RenderingTaxonomyCode = doc.CreateNode(XmlNodeType.Element,"TaxonomyCode","");
			RenderingTaxonomyCode.InnerText = TaxoCode;
			RenderingProvider.AppendChild(RenderingAddress);
			// SPK / AAD 8/13/08 Add Rendering Provider NPI It is same as Info Receiver NPI -- Start
			RenderingProvider.AppendChild(RenderingProviderNPI);
			// SPK / AAD 8/13/08 Add NPI -- End
			RenderingProvider.AppendChild(RenderingCredential);
			RenderingProvider.AppendChild(RenderingTaxonomyCode);
			//  Append RenderingProvider To EligNode
			EligNode.AppendChild(RenderingProvider);
			return doc.OuterXml;
		}

		private void ProcessEligibilityResponseDentalXchange(string DCIResponse) {
			XmlDocument doc = new XmlDocument();
			XmlNode IsEligibleNode;
			string IsEligibleStatus;
			doc.LoadXml(DCIResponse);
			IsEligibleNode = doc.SelectSingleNode("EligBenefitResponse/isEligible");
			switch(IsEligibleNode.InnerText) {
				case "0": // SPK
					// HINA Added 9/2. 
					// Open new form to display complete response Detail
					Form formDisplayEligibilityResponse = new FormEligibilityResponseDisplay(doc,PatPlanCur.PatNum);
					formDisplayEligibilityResponse.ShowDialog();
					break;
				case "1": // SPK
					// Process Error code and Message Node AAD
					XmlNode ErrorCode;
					XmlNode ErrorMessage;
					ErrorCode = doc.SelectSingleNode("EligBenefitResponse/Response/ErrorCode");
					ErrorMessage = doc.SelectSingleNode("EligBenefitResponse/Response/ErrorMsg");
					IsEligibleStatus = textSubscriber.Text + " is Not Eligible. Error Code:";
					IsEligibleStatus += ErrorCode.InnerText + " Error Description:" + ErrorMessage.InnerText;
					MessageBox.Show(IsEligibleStatus);
					break;
				default:
					IsEligibleStatus = textSubscriber.Text + " Eligibility status is Unknown";
					MessageBox.Show(IsEligibleStatus);
					break;
			}
		}

		#endregion

		private bool IsEmployerValid() {
			PatPlan patPlanDB=PatPlans.GetByPatPlanNum(PatPlanCur.PatPlanNum);
			InsSub insSubDB=InsSubs.GetOne(patPlanDB.InsSubNum);
			InsPlan insPlanDB=InsPlans.GetPlan(insSubDB.PlanNum,null);
			bool hasExistingEmployerChanged=(insPlanDB.CarrierNum!=0 && insPlanDB.EmployerNum!=_planCur.EmployerNum && insPlanDB.PlanNum==_planCur.PlanNum);//not new insplan and employer db not same as selection and insplan still used.
			if(_employerNameOrig=="") {//no employer was previously entered.
				if(textEmployer.Text=="") {
					//no change
				}
				else {
					if(!Security.IsAuthorized(Permissions.InsPlanEdit,true)) {//Employer was changed and they don't have perms to make new insplan (they picked plan from list).
						//Validate plan's employer in DB
						Employer employerDB=Employers.GetByName(textEmployer.Text);
						if(employerDB==null) {
							MsgBox.Show(this,"The Employer for this insurance plan has been combined or deleted since the plan was loaded.  Please choose another insurance plan.");
							return false;
						}
						if(hasExistingEmployerChanged) {//not a new insplan, and the employer was changed compared to what's in the DB.
							MsgBox.Show(this,"The Employer for this insurance plan has been changed since the plan was loaded.  Please choose another insurance plan.");
							return false;
						}
					}
					else { 
						//They do have perms and they chose an insurance plan or entered employer manually.  Could have entered in emp manually or picked from list, we don't care
					}
				}
			}
			else {//an employer was previously entered
				if(textEmployer.Text=="") {
					if(!Security.IsAuthorized(Permissions.InsPlanEdit,true)) {//Employer is now empty.  Need to see if the insplan in DB also has empty employer, or if someone else put one on it.
						if(hasExistingEmployerChanged) {//Not a new insplan and employer was changed
							MsgBox.Show(this,"The Employer for this insurance plan has been changed since the plan was loaded.  Please choose another insurance plan.");
							return false;
						}
					}
				}
				//if text has changed
				else if(_employerNameOrig!=textEmployer.Text) {//Employer text was changed since the window was loaded (picked from list or manually edited)
					if(!Security.IsAuthorized(Permissions.InsPlanEdit,true)) {//Without permission, they must have picked from list.  Verify employer still exists.  If it does, verify the insplan still has same employer.
						Employer employerDB=Employers.GetByName(textEmployer.Text);
						if(employerDB==null) {
							MsgBox.Show(this,"The Employer for this insurance plan has been combined or deleted since the plan was loaded.  Please choose another insurance plan.");
							return false;
						}						
						if(hasExistingEmployerChanged) {//Not a new insplan and employer was changed.
							MsgBox.Show(this,"The Employer for this insurance plan has been changed since the plan was loaded.  Please choose another insurance plan.");
							return false;
						}
					}
					else {//Are authorized
						if(_employerNameCur==textEmployer.Text) { //They picked from list and didn't change it manually.
							Employer employerDB=Employers.GetByName(textEmployer.Text);
							if(employerDB==null && !MsgBox.Show(this,MsgBoxButtons.YesNo,"The Employer entered for this insurance plan has been combined or deleted since the plan was loaded.  Do you want to override those changes?")) {
								return false;
							}
						
							if(hasExistingEmployerChanged && !MsgBox.Show(this,MsgBoxButtons.YesNo,"The Employer for this insurance plan has been changed since the plan was loaded.  Do you want to override those changes?")) {
								return false;
							}
						}
						else {
							//They changed it manually we don't care if it exists or not.
						}
					}
				}
				else {
					//Employer wasn't changed.
				}
			}
			return true;
		}

		private bool IsCarrierValid() {
			Carrier carrierForm=GetCarrierFromForm();
			PatPlan patPlanDB=PatPlans.GetByPatPlanNum(PatPlanCur.PatPlanNum);
			InsSub insSubDB=InsSubs.GetOne(patPlanDB.InsSubNum);
			InsPlan insPlanDB=InsPlans.GetPlan(insSubDB.PlanNum,null);//Can have CarrierNum==0 if this is a new plan.
			bool hasExistingCarrierChanged=(insPlanDB.CarrierNum!=_carrierCur.CarrierNum && insPlanDB.CarrierNum!=0 && insPlanDB.CarrierNum!=_carrierNumOrig);
			if(_carrierCur.CarrierNum!=_carrierNumOrig && Carriers.Compare(carrierForm,_carrierCur)) {//Carrier was changed via "Pick From List" and not edited manually
				if(Security.IsAuthorized(Permissions.InsPlanEdit,true)) {
					if(Carriers.GetCarrierDB(_carrierCur.CarrierNum)==null && !MsgBox.Show(this,MsgBoxButtons.YesNo,"The Carrier selected has been combined or deleted since it was last picked.  Would you like to override those changes?")) {//Someone deleted/combined the carrier while the window was open.
						return false;
					}
					if(hasExistingCarrierChanged && !MsgBox.Show(this,MsgBoxButtons.YesNo,"The selected insurance plan has had its Carrier changed since it was loaded.  Would you like to override those changes?")) {//Someone changed this insplan's carrier while the window was open.
						return false;
					}
				}
				else {//Not authorized
					if(Carriers.GetCarrierDB(_carrierCur.CarrierNum)==null) {
						MsgBox.Show(this,"The selected insurance plan has had its carrier combined or deleted since it was last picked.  Please choose another.");
						return false;
					}
					if(hasExistingCarrierChanged) {//Someone changed this insplan's carrier while the window was open.
						MsgBox.Show(this,"The selected insurance plan has had its Carrier changed since it was loaded.  Please choose another.");
						return false;
					}
				}
			}
			else if(!Carriers.Compare(carrierForm,_carrierCur)) {//Carrier edited manually (doesn't matter if it was picked from list or not, user without perms can't edit manually)
				if(hasExistingCarrierChanged && !MsgBox.Show(this,MsgBoxButtons.YesNo,"The selected insurance plan has had its Carrier changed since it was loaded.  Would you like to override those changes?")) {//Someone changed this insplan's carrier while the window was open.
					return false;
				}
				//No need to look up if the carrier entered manually exists.  We can't tell if it doesn't exist or if it was deleted while the form was open.
				//If we look up a carrier using the info in the form, if it exists, then fine.  If it doesn't exist, is it because it was manually edited, or because someone else deleted it.
			}
			else if(_planOld.PlanNum!=_planCur.PlanNum) {//Plan was picked from list
				if(Security.IsAuthorized(Permissions.InsPlanEdit,true)) {
					if(insPlanDB.CarrierNum!=_carrierCur.CarrierNum && !MsgBox.Show(this,MsgBoxButtons.YesNo,"The selected insurance plan has had its Carrier changed since it was loaded.  Would you like to override those changes?")) {
						return false;
					}
				}
				else {//Not authorized
					if(insPlanDB.CarrierNum!=_carrierCur.CarrierNum) {
						MsgBox.Show(this,"The selected insurance plan has had its Carrier changed since it was loaded.  Please choose another.");
						return false;
					}
				}
			}
			else {//Carrier information is the same from when it was loaded into the form and "Pick From List" wasn't used to change information.
				//Always use what's in the DB, no warnings
			}
			return true;
		}

		private Carrier GetCarrierFromForm() {
			Carrier carrier=new Carrier();
			carrier.CarrierName=textCarrier.Text;
			carrier.Phone=textPhone.Text;
			carrier.Address=textAddress.Text;
			carrier.Address2=textAddress2.Text;
			carrier.City=textCity.Text;
			carrier.State=textState.Text;
			carrier.Zip=textZip.Text;
			carrier.ElectID=textElectID.Text;
			carrier.NoSendElect=checkNoSendElect.Checked;
			return carrier;
		}

		///<summary>Used from butGetElectronic_Click and from butOK_Click.  Returns false if unable to complete.  Also fills SubCur if not null.</summary>
		private bool FillPlanCurFromForm(){
			if(textDateEffect.errorProvider1.GetError(textDateEffect)!=""
				|| textDateTerm.errorProvider1.GetError(textDateTerm)!=""
				|| textDentaide.errorProvider1.GetError(textDentaide)!=""
				|| textDateLastVerifiedBenefits.errorProvider1.GetError(textDateLastVerifiedBenefits)!="" 
				|| textDateLastVerifiedPatPlan.errorProvider1.GetError(textDateLastVerifiedPatPlan)!="")
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return false;
			}
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				if(textPlanFlag.Text!="" && textPlanFlag.Text!="A" && textPlanFlag.Text!="V" && textPlanFlag.Text!="N") {
					MsgBox.Show(this,"Plan flag must be A, V, N, or blank.");
					return false;
				}
				if(textPlanFlag.Text=="") {
					if(checkIsPMP.Checked) {
						MsgBox.Show(this,"The provincial medical plan checkbox must be unchecked when the plan flag is blank.");
						return false;
					}
				}
				else {
					if(!checkIsPMP.Checked) {
						MsgBox.Show(this,"The provincial medical plan checkbox must be checked when the plan flag is not blank.");
						return false;
					}
					if(textPlanFlag.Text=="A") {
						if(textCanadianDiagCode.Text=="" || textCanadianDiagCode.Text!=Canadian.TidyAN(textCanadianDiagCode.Text,textCanadianDiagCode.Text.Length,true)) {
							MsgBox.Show(this,"When plan flag is set to A, diagnostic code must be set and must be 6 characters or less in length.");
							return false;
						}
						if(textCanadianInstCode.Text=="" || textCanadianInstCode.Text!=Canadian.TidyAN(textCanadianInstCode.Text,textCanadianInstCode.Text.Length,true)) {
							MsgBox.Show(this,"When plan flag is set to A, institution code must be set and must be 6 characters or less in length.");
							return false;
						}
					}
				}
			}
			if(textSubscriberID.Text=="" && _subCur!=null) {
				MsgBox.Show(this,"Subscriber ID not allowed to be blank.");
				return false;
			}
			if(textCarrier.Text=="") {
				MsgBox.Show(this,"Carrier not allowed to be blank.");
				return false;
			}
			if(PatPlanCur!=null && textOrdinal.errorProvider1.GetError(textOrdinal)!=""){
				MsgBox.Show(this,"Please fix data entry errors first.");
				return false;
			}
			if(comboRelationship.SelectedIndex==-1 && comboRelationship.Items.Count>0) {
				MsgBox.Show(this,"Relationship to Subscriber is not allowed to be blank.");
				return false;
			}
			if(PatPlanCur!=null && !IsEmployerValid()) {
				return false;
			}
			if(PatPlanCur!=null && !IsCarrierValid()) {
				return false;
			}
			if(_subCur!=null) {
				//Subscriber: Only changed when user clicks change button.
				_subCur.SubscriberID=textSubscriberID.Text;
				_subCur.DateEffective=PIn.Date(textDateEffect.Text);
				_subCur.DateTerm=PIn.Date(textDateTerm.Text);
				_subCur.ReleaseInfo=checkRelease.Checked;
				_subCur.AssignBen=checkAssign.Checked;
				_subCur.SubscNote=textSubscNote.Text;
				//MonthRenew already handled inside benefit window.
			}
			GetEmployerNum();
			_planCur.GroupName=textGroupName.Text;
			_planCur.GroupNum=textGroupNum.Text;
			_planCur.RxBIN=textBIN.Text;
			_planCur.DivisionNo=textDivisionNo.Text;//only visible in Canada
			//carrier-----------------------------------------------------------------------------------------------------
			if(Security.IsAuthorized(Permissions.InsPlanEdit,true)) {//User has the ability to edit carrier information.  Check for matches, create new Carrier if applicable.
				Carrier carrierForm=GetCarrierFromForm();
				Carrier carrierOld=_carrierCur.Copy();
				if(_carrierCur.CarrierNum==_carrierNumOrig && Carriers.Compare(carrierForm,_carrierCur) && _planCur.PlanNum==_planOld.PlanNum) {
					//carrier is the same as it was originally, use what's in db if editing a patient's patplan.
					if(PatPlanCur!=null) {
						PatPlan patPlanDB=PatPlans.GetByPatPlanNum(PatPlanCur.PatPlanNum);
						InsSub insSubDB=InsSubs.GetOne(patPlanDB.InsSubNum);
						InsPlan insPlanDB=InsPlans.GetPlan(insSubDB.PlanNum,null);
						_carrierCur=Carriers.GetCarrier(insPlanDB.CarrierNum);
						_planCurOriginal.CarrierNum=_carrierCur.CarrierNum;
					}
					else {
						//Someone could have changed the insplan while the user was editing this window, do not overwrite the other users changes.
						InsPlan insPlanDB=InsPlans.GetPlan(_planCur.PlanNum,null);
						if(insPlanDB.PlanNum==0) {
							MsgBox.Show(this,"Insurance plan has been combined or deleted since the window was opened.  Please press Cancel to continue and refresh the list of insurance plans.");
							return false;
						}
						_carrierCur=Carriers.GetCarrier(insPlanDB.CarrierNum);
						_planCurOriginal.CarrierNum=_carrierCur.CarrierNum;
					}
				}
				else {
					_carrierCur=carrierForm;
					if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
						bool carrierFound=true;
						try {
							_carrierCur=Carriers.GetIdentical(_carrierCur);
						}
						catch {//match not found
							carrierFound=false;
						}
						if(!carrierFound) {
							if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Carrier not found.  Create new carrier?")) {
								return false;
							}
							FormCarrierEdit formCE=new FormCarrierEdit();
							formCE.IsNew=true;
							formCE.CarrierCur=_carrierCur;
							formCE.ShowDialog();
							if(formCE.DialogResult!=DialogResult.OK) {
								return false;
							}
						}
					}
					else {
						_carrierCur=Carriers.GetIdentical(_carrierCur,carrierOld: carrierOld);
					}
				}
				_planCur.CarrierNum=_carrierCur.CarrierNum;
			}
			else {//User does not have permission to edit carrier information.  
				//We don't care if carrier info is changed, only if it's removed.  
				//If it's removed, have them choose another.  If it's simply changed, just use the same prikey.
				if(Carriers.GetCarrier(_carrierCur.CarrierNum).CarrierName=="" && _planCur.PlanNum!=_planOld.PlanNum) {//Carrier not found, it must have been deleted or combined
					MsgBox.Show(this,"Selected carrier has been combined or deleted.  Please choose another insurance plan.");
					return false;
				}
				else if(_planCur.PlanNum==_planOld.PlanNum) {//Didn't switch insplan, they were only viewing.
					long planNumDb=_planOld.PlanNum;
					if(PatPlanCur!=null) {
						//Another user could have edited this patient's plan at the same time and could have changed something about the pat plan so we need
						//to go to the database an make sure that we are "not changing anything" by saving potentially stale data to the db.
						//If we don't do this, then we would end up overriding any changes that other users did while we were in this edit window.
						PatPlan patPlanDB=PatPlans.GetByPatPlanNum(PatPlanCur.PatPlanNum);
						InsSub insSubDB=InsSubs.GetOne(patPlanDB.InsSubNum);
						planNumDb=insSubDB.PlanNum;
					}
					InsPlan insPlanDB=InsPlans.GetPlan(planNumDb,null);
					_carrierCur=Carriers.GetCarrier(insPlanDB.CarrierNum);
					_planCurOriginal.CarrierNum=_carrierCur.CarrierNum;
					_planCur.CarrierNum=_carrierCur.CarrierNum;
				}
				else { 
					_planCur.CarrierNum=_carrierCur.CarrierNum;
				}
			}
			//plantype already handled.
			if(comboClaimForm.SelectedIndex!=-1){
				_planCur.ClaimFormNum=_listClaimForms[comboClaimForm.SelectedIndex].ClaimFormNum;
			}
			_planCur.UseAltCode=checkAlternateCode.Checked;
			_planCur.CodeSubstNone=checkCodeSubst.Checked;
			_planCur.IsMedical=checkIsMedical.Checked;
			_planCur.ClaimsUseUCR=checkClaimsUseUCR.Checked;
			_planCur.IsHidden=checkIsHidden.Checked;
			_planCur.ShowBaseUnits=checkShowBaseUnits.Checked;
			if(comboFeeSched.SelectedIndex==0) {
				_planCur.FeeSched=0;
			}
			else if(comboFeeSched.SelectedIndex == -1) {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"The selected fee schedule has been hidden. Are you sure you want to continue?")) {
					return false;
				}
				_planCur.FeeSched=_planCurOriginal.FeeSched;
			}
			else{
				_planCur.FeeSched=FeeSchedsStandard[comboFeeSched.SelectedIndex-1].FeeSchedNum;
			}
			if(comboCopay.SelectedIndex==0){
				_planCur.CopayFeeSched=0;//none
			}
			else{
				_planCur.CopayFeeSched=((ODBoxItem<FeeSched>)comboCopay.Items[comboCopay.SelectedIndex]).Tag.FeeSchedNum;
			}
			if(comboAllowedFeeSched.SelectedIndex==0){
				if(IsNewPlan
					&& _planCur.PlanType==""//percentage
					&& PrefC.GetBool(PrefName.AllowedFeeSchedsAutomate)){
					//add a fee schedule if needed
					FeeSched sched=FeeScheds.GetByExactName(_carrierCur.CarrierName,FeeScheduleType.OutNetwork);
					if(sched==null){
						sched=new FeeSched();
						sched.Description=_carrierCur.CarrierName;
						sched.FeeSchedType=FeeScheduleType.OutNetwork;
						//sched.IsNew=true;
						sched.IsGlobal=true;
						sched.ItemOrder=FeeScheds.GetCount();
						FeeScheds.Insert(sched);
						DataValid.SetInvalid(InvalidType.FeeScheds);
					}
					_planCur.AllowedFeeSched=sched.FeeSchedNum;
				}
				else{
					_planCur.AllowedFeeSched=0;
				}
			}
			else{
				_planCur.AllowedFeeSched=FeeSchedsAllowed[comboAllowedFeeSched.SelectedIndex-1].FeeSchedNum;
			}
			_planCur.CobRule=(EnumCobRule)comboCobRule.SelectedIndex;
			//Canadian------------------------------------------------------------------------------------------
			_planCur.DentaideCardSequence=PIn.Byte(textDentaide.Text);
			_planCur.CanadianPlanFlag=textPlanFlag.Text;//validated
			_planCur.CanadianDiagnosticCode=textCanadianDiagCode.Text;//validated
			_planCur.CanadianInstitutionCode=textCanadianInstCode.Text;//validated
			//Canadian end---------------------------------------------------------------------------------------
			_planCur.TrojanID=textTrojanID.Text;
			_planCur.PlanNote=textPlanNote.Text;
			_planCur.HideFromVerifyList=checkDontVerify.Checked;
			//Ortho----------------------------------------------------------------------------------------------
			_planCur.OrthoType=comboOrthoClaimType.SelectedIndex==-1 ? 0 : (OrthoClaimType)Enum.GetValues(typeof(OrthoClaimType)).GetValue(comboOrthoClaimType.SelectedIndex);
			if(_orthoAutoProc!=null) {
				_planCur.OrthoAutoProcCodeNumOverride=_orthoAutoProc.CodeNum;
			}
			else {
				_planCur.OrthoAutoProcCodeNumOverride=0; //overridden by practice default.
			}
			_planCur.OrthoAutoProcFreq=comboOrthoAutoProcPeriod.SelectedIndex==-1 ? 0 : (OrthoAutoProcFrequency)Enum.GetValues(typeof(OrthoAutoProcFrequency)).GetValue(comboOrthoAutoProcPeriod.SelectedIndex);
			_planCur.OrthoAutoClaimDaysWait=checkOrthoWaitDays.Checked ? 30 : 0;
			_planCur.OrthoAutoFeeBilled=PIn.Double(textOrthoAutoFee.Text);
			return true;
		}

		///<summary>Warns user if there are received claims for this plan.  Returns true if user wants to proceed, or if there are no received claims for this plan.  Returns false if the user aborts.</summary>
		private bool CheckForReceivedClaims() {
			long patNum=0;
			if(PatPlanCur!=null) {//PatPlanCur will be null if editing insurance plans from Lists > Insurance Plans.
				patNum=PatPlanCur.PatNum;
			}
			int claimCount=0;
			if(patNum==0) {//Editing insurance plans from Lists > Insurance Plans.
				//Check all claims for plan
				claimCount=Claims.GetCountReceived(_planCurOriginal.PlanNum);
				if(claimCount!=0) {
					if(MessageBox.Show(Lan.g(this,"There are")+" "+claimCount+" "+Lan.g(this,"received claims for this insurance plan that will have the carrier changed")+".  "+Lan.g(this,"You should NOT do this if the patient is changing insurance")+".  "+Lan.g(this,"Use the Drop button instead")+".  "+Lan.g(this,"Continue")+"?","",MessageBoxButtons.OKCancel)==DialogResult.Cancel) {
						return false; //abort
					}
				}
			}
			else {//Editing insurance plans from Family module.
				if(radioChangeAll.Checked==true) {//Check radio button
					claimCount=Claims.GetCountReceived(_planCurOriginal.PlanNum);
					if(claimCount!=0) {//Check all claims for plan
						if(MessageBox.Show(Lan.g(this,"There are")+" "+claimCount+" "+Lan.g(this,"received claims for this insurance plan that will have the carrier changed")+".  "+Lan.g(this,"You should NOT do this if the patient is changing insurance")+".  "+Lan.g(this,"Use the Drop button instead")+".  "+Lan.g(this,"Continue")+"?","",MessageBoxButtons.OKCancel)==DialogResult.Cancel) {
							return false; //abort
						}
					}
				}
				else {//Check claims for plan and patient only
					claimCount=Claims.GetCountReceived(_planCurOriginal.PlanNum,PatPlanCur.InsSubNum);
					if(claimCount!=0) {
						if(MessageBox.Show(Lan.g(this,"There are")+" "+claimCount+" "+Lan.g(this,"received claims for this insurance plan that will have the carrier changed")+".  "+Lan.g(this,"You should NOT do this if the patient is changing insurance")+".  "+Lan.g(this,"Use the Drop button instead")+".  "+Lan.g(this,"Continue")+"?","",MessageBoxButtons.OKCancel)==DialogResult.Cancel) {
							return false; //abort
						}
					}
				}
			}
			return true;
		}

		private void butSubstCodes_Click(object sender,EventArgs e) {
			FormInsPlanSubstitution FormInsSubst=new FormInsPlanSubstitution(_planCur);
			if(FormInsSubst.ShowDialog()==DialogResult.OK) {
				checkCodeSubst.Checked=_planCur.CodeSubstNone;//Since the user can change this flag in the other window.
			}
		}
		
		private void butVerifyPatPlan_Click(object sender,EventArgs e) {
			textDateLastVerifiedPatPlan.Text=DateTime.Today.ToShortDateString();
		}

		private void butVerifyBenefits_Click(object sender,EventArgs e) {
			textDateLastVerifiedBenefits.Text=DateTime.Today.ToShortDateString();
		}

		private void butAudit_Click(object sender,EventArgs e) {
			GetEmployerNum();
			FormInsEditLogs FormIEL = new FormInsEditLogs(_planCur,benefitList);
			FormIEL.ShowDialog();
		}

		private void comboOrthoClaimType_SelectionChangeCommitted(object sender,EventArgs e) {
			SetEnabledOrtho();
		}

		private void butPatOrtho_Click(object sender,EventArgs e) {
			if(comboOrthoClaimType.SelectedIndex != (int)OrthoClaimType.InitialPlusPeriodic) {
				MsgBox.Show(this,"To view this setup window, the insurance plan must be set to have an Ortho Claim Type of Initial Plus Periodic.");
				return;
			}
			double defaultFee=PIn.Double(textOrthoAutoFee.Text);
			string carrierName=PIn.String(textCarrier.Text);
			string subID=PIn.String(textSubscriberID.Text);
			if(defaultFee==0) {
				defaultFee=_planCur.OrthoAutoFeeBilled;
			}
			FormOrthoPat FormOP = new FormOrthoPat(PatPlanCur,_planCur,carrierName,subID,defaultFee);
			FormOP.ShowDialog();
		}

		private void butPickOrthoProc_Click(object sender,EventArgs e) {
			FormProcCodes FormPC = new FormProcCodes();
			FormPC.IsSelectionMode=true;
			FormPC.ShowDialog();
			if(FormPC.DialogResult == DialogResult.OK) {
				_orthoAutoProc=ProcedureCodes.GetProcCode(FormPC.SelectedCodeNum);
				textOrthoAutoProc.Text=_orthoAutoProc.ProcCode;
			}
		}

		private void butDefaultAutoOrthoProc_Click(object sender,EventArgs e) {
			_orthoAutoProc=null;
			textOrthoAutoProc.Text=ProcedureCodes.GetProcCode(PrefC.GetLong(PrefName.OrthoAutoProcCodeNum)).ProcCode+" ("+Lan.g(this,"Default")+")";
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			bool removeLogs=false;
			if(PatPlanCur!=null) {
				PatPlan ppExists=PatPlans.GetByPatPlanNum(PatPlanCur.PatPlanNum);
				if(ppExists==null) {
					MsgBox.Show(this,"This plan was removed by another user and no longer exists.");
					DialogResult=DialogResult.Cancel;
					return;
				}
			}
			if((radioChangeAll.Checked || (radioCreateNew.Checked && comboLinked.Items.Count==0)) //These are the two scenarios in which InsPlans.Update will be called instead of Insert.
				&& (_planCur==null || InsPlans.GetPlan(_planCur.PlanNum,new List<InsPlan>())==null)) 
			{
				MsgBox.Show(this,"The selected insurance plan was removed by another user and no longer exists.  Open insurance plan again to edit.");
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(_subCur!=null && InsPlans.GetPlan(_subCur.PlanNum,new List<InsPlan>())==null) {
				MsgBox.Show(this,"The subscriber's insurance plan was merged by another user and no longer exists.  Open insurance plan again to edit.");
				DialogResult=DialogResult.Cancel;
				return;
			}
			long selectedFilingCodeNum=0;
			if(comboFilingCode.SelectedItem!=null) {
				selectedFilingCodeNum=((ODBoxItem<InsFilingCode>)comboFilingCode.SelectedItem).Tag.InsFilingCodeNum;
			}
			_planCur.FilingCode=selectedFilingCodeNum;
			_planCur.FilingCodeSubtype=0;
			if(comboFilingCodeSubtype.SelectedItem != null) {
				_planCur.FilingCodeSubtype=((ODBoxItem<InsFilingCodeSubtype>)comboFilingCodeSubtype.SelectedItem).Tag.InsFilingCodeSubtypeNum;
			}
			#region 1
			try {
			if(!FillPlanCurFromForm()) {//also fills SubCur if not null
				return;
			}
			if(_planCur.CarrierNum!=_planCurOriginal.CarrierNum) {
				long patNum=0;
				if(PatPlanCur!=null) {//PatPlanCur will be null if editing insurance plans from Lists > Insurance Plans.
					patNum=PatPlanCur.PatNum;
				}
				string carrierNameOrig=Carriers.GetCarrier(_planCurOriginal.CarrierNum).CarrierName;
				string carrierNameNew=Carriers.GetCarrier(_planCur.CarrierNum).CarrierName;
				if(carrierNameOrig!=carrierNameNew) {//The CarrierNum could have changed but the CarrierName might not have changed.  Only warn the name changed.
					if(!CheckForReceivedClaims()) {
						return;
					}
				}
			}
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Error Code 1")+".  "+Lan.g(this,"Please contact support")+"\r\n"+"\r\n"+ex.Message+"\r\n"+ex.StackTrace);
				return;
			}
			#endregion 1
			#region 2
			try {
			//We do not want to block users from creating new plans for subscribers if they do not have the InsPlanChangeAssign permission.
			//Therefore, we will only check the permission if they are editing an old plan.
			if(_subOld!=null) {//Editing an old plan for a subscriber.
				if(_subOld.AssignBen!=checkAssign.Checked) {
					if(!Security.IsAuthorized(Permissions.InsPlanChangeAssign)) {
						return;
					}
					//It is very possible that the user changed the patient associated to the ins sub.
					//We need to make a security log for the most recent patient (_subCur.Subscriber) instead of the original patient (_subOld.Subscriber) that was passed in.
					SecurityLogs.MakeLogEntry(Permissions.InsPlanChangeAssign,_subCur.Subscriber,Lan.g(this,"Assignment of Benefits (pay dentist) changed from")
						+" "+(_subOld.AssignBen?Lan.g(this,"checked"):Lan.g(this,"unchecked"))+" "
						+Lan.g(this,"to")
						+" "+(checkAssign.Checked?Lan.g(this,"checked"):Lan.g(this,"unchecked"))+" for plan "
						+Carriers.GetCarrier(_planCur.CarrierNum).CarrierName);
				}
			}
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Error Code 2")+".  "+Lan.g(this,"Please contact support")+"\r\n"+"\r\n"+ex.Message+"\r\n"+ex.StackTrace);
				return;
			}
			#endregion 2
			#region 3
			try {
				//Validation is finished at this point.
				//PatPlan-------------------------------------------------------------------------------------------
				if(PatPlanCur!=null) {
					if(PIn.Long(textOrdinal.Text)!=PatPlanCur.Ordinal) {//Ordinal changed by user
						PatPlanCur.Ordinal=(byte)(PatPlans.SetOrdinal(PatPlanCur.PatPlanNum,PIn.Int(textOrdinal.Text)));
						_hasOrdinalChanged=true;
					}
					else if(PIn.Long(textOrdinal.Text)!=PatPlans.GetByPatPlanNum(PatPlanCur.PatPlanNum).Ordinal) {
						//PatPlan's ordinal changed by somebody else and not this user, set it to what's in the DB for this update.
						PatPlanCur.Ordinal=PatPlans.GetByPatPlanNum(PatPlanCur.PatPlanNum).Ordinal;
					}
					PatPlanCur.IsPending=checkIsPending.Checked;
					PatPlanCur.Relationship=(Relat)comboRelationship.SelectedIndex;
					PatPlanCur.PatID=textPatID.Text;
					PatPlans.Update(PatPlanCur);
					if(!PIn.Date(textDateLastVerifiedPatPlan.Text).Date.Equals(_datePatPlanLastVerified.Date)) {
						InsVerify insVerify=InsVerifies.GetOneByFKey(PatPlanCur.PatPlanNum,VerifyTypes.PatientEnrollment);
						if(insVerify!=null) {
							insVerify.DateLastVerified=PIn.Date(textDateLastVerifiedPatPlan.Text);
							InsVerifyHists.InsertFromInsVerify(insVerify);
						}
					}
				}
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Error Code 3")+".  "+Lan.g(this,"Please contact support")+"\r\n"+"\r\n"+ex.Message+"\r\n"+ex.StackTrace);
				return;
			}
			//It is okay to set the plan num on the subscriber object at this point.
			//This is mainly for users that do not have the InsPlanEdit permission which should be allowed to manipulate subscribers.
			//The plan num could change again farther down if the user actually has permission to manipulate the ins plan.
			if(_subCur!=null) {
				_subCur.PlanNum=_planCur.PlanNum;
			}
			#endregion 3
			//Sections 4 - 10 all deal with manipulating the insurance plan so make sure the user has permission to do so.
			#region InsPlanEdit permission
			if(Security.IsAuthorized(Permissions.InsPlanEdit,true)) {
				//InsPlan-----------------------------------------------------------------------------------------
				if(_subCur==null) {//editing from big list.  No subscriber.  'pick from list' button not visible, making logic easier.
					#region 4
					try {
						//if(IsNewPlan) {//not yet implemented
						//	if(InsPlans.AreEqualValue(PlanCur,PlanCurOriginal)) {//If no changes

						//	}
						//	else {//changes were made

						//	}
						//}
						//else {//editing an existing plan from big list
						if(InsPlans.AreEqualValue(_planCur,_planCurOriginal)) {//If no changes
																																	 //pick button doesn't complicate things.  Simply nothing to do.
																																	 //Also, no SubCur, so just close the form.
							DialogResult=DialogResult.OK;
						}
						else {//changes were made
							InsPlans.Update(_planCur);
							DialogResult=DialogResult.OK;
						}
						//}
					}
					catch(Exception ex) {
						MessageBox.Show(Lan.g(this,"Error Code 4")+".  "+Lan.g(this,"Please contact support")+"\r\n"+"\r\n"+ex.Message+"\r\n"+ex.StackTrace);
						return;
					}
					#endregion 4
				}
				else {//(subCur!=null) editing from within patient
							//Be very careful here.  User could have clicked 'pick from list' button, which would have changed PlanNum.
							//So we always compare with PlanNumOriginal.
					if(IsNewPlan) {
						if(InsPlans.AreEqualValue(_planCur,_planCurOriginal)) {//New plan, no changes
							#region 5 - If the logic in this region changes, then also change region 5a below.
							try {
								if(_planCur.PlanNum != _planOld.PlanNum) {//clicked 'pick from list' button
																													//No need to update PlanCur because no changes.
																													//delete original plan.
									try {
										//does dependency checking, throws if dependencies exist. the inssub should NOT be deleted as it is used below.
										InsPlans.Delete(_planOld,canDeleteInsSub: false);
										removeLogs=true;
									}
									catch(ApplicationException ex) {
										MessageBox.Show(ex.Message);
										//do not need to update PlanCur because no changes were made.
										SecurityLogs.MakeLogEntry(Permissions.InsPlanEdit,(PatPlanCur==null) ? 0 : PatPlanCur.PatNum
											,Lan.g(this,"FormInsPlan region 5 delete validation failed.  Plan was not deleted."),_planOld.PlanNum,
											DateTime.MinValue); //new plan, no date needed.
										Close();
										return;
									}
									_subCur.PlanNum=_planCur.PlanNum;
									//PatPlanCur.PlanNum=PlanCur.PlanNum;
									//PatPlans.Update(PatPlanCur);
									//When 'pick from list' button was pushed, benfitList was filled with benefits from the picked plan.
									//Then, those benefits may or may not have been changed.  
									//benefitListOld will still contain the original defaults for a new plan, but they will be orphaned.
									//So all the original benefits will be automatically deleted because they won't be found in the newlist.
									//If any benefits were changed after picking, the synch further down will trigger updates for the benefits on the picked plan.
								}
								else {//new plan, no changes, not picked from list.
											//do not need to update PlanCur because no changes were made.
								}
							}
							catch(Exception ex) { //catch any other exceptions and display
								MessageBox.Show(Lan.g(this,"Error Code 5")+".  "+Lan.g(this,"Please contact support")+"\r\n"+"\r\n"+ex.Message+"\r\n"+ex.StackTrace);
								return;
							}
							#endregion 5
						}
						else {//new plan, changes were made
							if(_planCur.PlanNum != _planOld.PlanNum) {//clicked 'pick from list' button, and then made changes
								#region 6 - If the logic in this region changes, then also change region 6a below.
								try {
									if(radioChangeAll.Checked) {
										InsPlans.Update(_planCur);//they might not realize that they would be changing an existing plan. Oh well.
										try {
											//does dependency checking, throws if dependencies exist. the inssub should NOT be deleted as it is used below.
											InsPlans.Delete(_planOld,canDeleteInsSub: false);
											removeLogs=true;
										}
										catch(ApplicationException ex) {
											MessageBox.Show(ex.Message);
											SecurityLogs.MakeLogEntry(Permissions.InsPlanEdit,(PatPlanCur==null) ? 0 : PatPlanCur.PatNum
												,Lan.g(this,"FormInsPlan region 6 delete validation failed.  Plan was not deleted."),_planOld.PlanNum,
												DateTime.MinValue); //new plan, no date needed.
											Close();
											return;
										}
										_subCur.PlanNum=_planCur.PlanNum;
										//PatPlanCur.PlanNum=PlanCur.PlanNum;
										//PatPlans.Update(PatPlanCur);
										//Same logic applies to benefit list as the section above.
									}
									else {//option is checked for "create new plan if needed"
										_planCur.PlanNum=_planOld.PlanNum;
										InsPlans.Update(_planCur);
										_subCur.PlanNum=_planCur.PlanNum;
										//no need to update PatPlan.  Same old PlanNum.
										//When 'pick from list' button was pushed, benfitList was filled with benefits from the picked plan.
										//benefitListOld was not touched and still contains the old benefits.  So the original benefits will be automatically deleted.
										//We force copies to be made in the database, but with different PlanNums.
										//Any other changes will be preserved.
										for(int i = 0;i<benefitList.Count;i++) {
											if(benefitList[i].PlanNum>0) {
												benefitList[i].PlanNum=_planCur.PlanNum;
												benefitList[i].BenefitNum=0;//triggers insert during synch.
											}
										}
									}
								}
								catch(Exception ex) {
									MessageBox.Show(Lan.g(this,"Error Code 6")+".  "+Lan.g(this,"Please contact support")+"\r\n"+"\r\n"+ex.Message+"\r\n"+ex.StackTrace);
									return;
								}
								#endregion 6
							}
							else {//new plan, changes made, not picked from list.
								InsPlans.Update(_planCur);
							}
						}
					}
					else {//editing an existing plan from within patient
						if(InsPlans.AreEqualValue(_planCur,_planCurOriginal)) {//If no changes
							#region 7
							try {
								if(_planCur.PlanNum != _planOld.PlanNum) {//clicked 'pick from list' button, then made no changes
																													//do not need to update PlanCur because no changes were made.
									_subCur.PlanNum=_planCur.PlanNum;
									//PatPlanCur.PlanNum=PlanCur.PlanNum;
									//PatPlans.Update(PatPlanCur);
									//When 'pick from list' button was pushed, benefitListOld was filled with a shallow copy of the benefits from the picked list.
									//So if any benefits were changed, the synch further down will trigger updates for the benefits on the picked plan.
								}
								else {//existing plan, no changes, not picked from list.
											//do not need to update PlanCur because no changes were made.
								}
							}
							catch(Exception ex) {
								MessageBox.Show(Lan.g(this,"Error Code 7")+".  "+Lan.g(this,"Please contact support")+"\r\n"+"\r\n"+ex.Message+"\r\n"+ex.StackTrace);
								return;
							}
							#endregion 7
						}
						else {//changes were made
							if(_planCur.PlanNum != _planOld.PlanNum) {//clicked 'pick from list' button, and then made changes
								if(radioChangeAll.Checked) {
									#region 8
									try {
										//warn user here?
										InsPlans.Update(_planCur);
										_subCur.PlanNum=_planCur.PlanNum;
										//PatPlanCur.PlanNum=PlanCur.PlanNum;
										//PatPlans.Update(PatPlanCur);
										//When 'pick from list' button was pushed, benefitListOld was filled with a shallow copy of the benefits from the picked list.
										//So if any benefits were changed, the synch further down will trigger updates for the benefits on the picked plan.
									}
									catch(Exception ex) {
										MessageBox.Show(Lan.g(this,"Error Code 8")+".  "+Lan.g(this,"Please contact support")+"\r\n"+"\r\n"+ex.Message+"\r\n"+ex.StackTrace);
										return;
									}
									#endregion 8
								}
								else {//option is checked for "create new plan if needed"
									#region 9
									try {
										if(comboLinked.Items.Count==0) {//if this is the only subscriber
											InsPlans.Update(_planCur);
											_subCur.PlanNum=_planCur.PlanNum;
											//PatPlanCur.PlanNum=PlanCur.PlanNum;
											//PatPlans.Update(PatPlanCur);
											//When 'pick from list' button was pushed, benefitListOld was filled with a shallow copy of the benefits from the picked list.
											//So if any benefits were changed, the synch further down will trigger updates for the benefits on the picked plan.
										}
										else {//if there are other subscribers
											InsPlans.Insert(_planCur);//this gives it a new primary key.
											_subCur.PlanNum=_planCur.PlanNum;
											//PatPlanCur.PlanNum=PlanCur.PlanNum;
											//PatPlans.Update(PatPlanCur);
											//When 'pick from list' button was pushed, benefitListOld was filled with a shallow copy of the benefits from the picked list.
											//We must clear the benefitListOld to prevent deletion of those benefits.
											benefitListOld=new List<Benefit>();
											//Force copies to be made in the database, but with different PlanNum;
											for(int i = 0;i<benefitList.Count;i++) {
												if(benefitList[i].PlanNum>0) {
													benefitList[i].PlanNum=_planCur.PlanNum;
													benefitList[i].BenefitNum=0;//triggers insert during synch.
												}
											}
										}
									}
									catch(Exception ex) {
										MessageBox.Show(Lan.g(this,"Error Code 9")+".  "+Lan.g(this,"Please contact support")+"\r\n"+"\r\n"+ex.Message+"\r\n"+ex.StackTrace);
										return;
									}
									#endregion 9
								}
							}
							else {//existing plan, changes made, not picked from list.
								#region 10
								try {
									if(radioChangeAll.Checked) {
										InsPlans.Update(_planCur);
									}
									else {//option is checked for "create new plan if needed"
										if(comboLinked.Items.Count==0) {//if this is the only subscriber
											InsPlans.Update(_planCur);
										}
										else {//if there are other subscribers
											InsPlans.Insert(_planCur);//this gives it a new primary key.
											_subCur.PlanNum=_planCur.PlanNum;
											//PatPlanCur.PlanNum=PlanCur.PlanNum;
											//PatPlans.Update(PatPlanCur);
											//make copies of all the benefits
											benefitListOld=new List<Benefit>();
											for(int i = 0;i<benefitList.Count;i++) {
												if(benefitList[i].PlanNum>0) {
													benefitList[i].PlanNum=_planCur.PlanNum;
													benefitList[i].BenefitNum=0;//triggers insert.
												}
											}
										}
									}
								}
								catch(Exception ex) {
									MessageBox.Show(Lan.g(this,"Error Code 10")+".  "+Lan.g(this,"Please contact support")+"\r\n"+"\r\n"+ex.Message+"\r\n"+ex.StackTrace);
									return;
								}
								#endregion 10
							}
						}
					}
				}
			}//End InsPlanEdit permission check
			else {//User does not have the InsPlanEdit permission.
				if(_subCur!=null) {
					if(IsNewPlan) {
						#region 5a - If the logic in this region changes, then also change region 5 above.
						try {
							if(_planCur.PlanNum != _planOld.PlanNum) {//user clicked the "pick from list" button. 
								//In a previous version, a user could still change some things about the plan even if they had no permissions to do so.
								//This was causing empty insurance plans to get saved to the db.
								//Even if they somehow managed to change something about the insurance plan they picked, we always just want to do the following:
								//1. Update the inssub to be the current insplan. (which happens above)
								//2. Delete the empty insurance plan (which happens here)
								try {
									//does dependency checking, throws if dependencies exist. the inssub should NOT be deleted as it is used below.
									InsPlans.Delete(_planOld,canDeleteInsSub: false);
									removeLogs=true;
								}
								catch(ApplicationException ex) {
									MessageBox.Show(ex.Message);
									SecurityLogs.MakeLogEntry(Permissions.InsPlanEdit,(PatPlanCur==null) ? 0 : PatPlanCur.PatNum
										,Lan.g(this,"FormInsPlan region 5a delete validation failed.  Plan was not deleted."),
										_planOld.PlanNum,DateTime.MinValue); //new plan, no date needed.
									Close();
									return;
								}
							}
						}
						catch(Exception ex) {
							MessageBox.Show(Lan.g(this,"Error Code 5a")+".  "+Lan.g(this,"Please contact support")+"\r\n"+"\r\n"+ex.Message+"\r\n"+ex.StackTrace);
							return;
						}
						#endregion
					}
				}
			}
			#endregion
			#region 11
			try {
			if(!PIn.Date(textDateLastVerifiedBenefits.Text).Date.Equals(_dateInsPlanLastVerified.Date)) {
				InsVerify insVerify=InsVerifies.GetOneByFKey(_planCur.PlanNum,VerifyTypes.InsuranceBenefit);
				insVerify.DateLastVerified=PIn.Date(textDateLastVerifiedBenefits.Text);
				InsVerifyHists.InsertFromInsVerify(insVerify);
			}
			//PatPlanCur.InsSubNum is already set before opening this window.  There is no possible way to change it from within this window.  Even if PlanNum changes, it's still the same inssub.  And even if inssub.Subscriber changes, it's still the same inssub.  So no change to PatPlanCur.InsSubNum is ever require from within this window.
			//Synch benefits-----------------------------------------------------------------------------------------------
			Benefits.UpdateList(benefitListOld,benefitList);
			if(removeLogs) {
				InsEditLogs.DeletePreInsertedLogsForPlanNum(_planOld.PlanNum);
			}
			//Update SubCur if needed-------------------------------------------------------------------------------------
			if(_subCur!=null) {
				//SubCur.PlanNum=PlanCur.PlanNum;//done above
				InsSubs.Update(_subCur);//also saves the other fields besides PlanNum
				//Udate all claims, claimprocs, payplans, and etrans that are pointing at the inssub.InsSubNum since it may now be pointing at a new insplan.PlanNum.
				InsSubs.SynchPlanNumsForNewPlan(_subCur);
				InsPlans.ComputeEstimatesForSubscriber(_subCur.Subscriber);
			}
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Error Code 11")+".  "+Lan.g(this,"Please contact support")+"\r\n"+"\r\n"+ex.Message+"\r\n"+ex.StackTrace);
				return;
			}
			#endregion 11
			#region 12
			try {
			//Check for changes in the carrier
			if(_planCur.CarrierNum!=_planCurOriginal.CarrierNum) {
				_hasCarrierChanged=true;
				long patNum=0;
				if(PatPlanCur!=null) {//PatPlanCur will be null if editing insurance plans from Lists > Insurance Plans.
					patNum=PatPlanCur.PatNum;
				}
				string carrierNameOrig=Carriers.GetCarrier(_planCurOriginal.CarrierNum).CarrierName;
				string carrierNameNew=Carriers.GetCarrier(_planCur.CarrierNum).CarrierName;
				if(carrierNameOrig!=carrierNameNew) {//The CarrierNum could have changed but the CarrierName might not have changed.  Only make an audit entry if the name changed.
					SecurityLogs.MakeLogEntry(Permissions.InsPlanChangeCarrierName,patNum,Lan.g(this,"Carrier name changed in Edit Insurance Plan window from")+" "
					+carrierNameOrig+" "+Lan.g(this,"to")+" "+carrierNameNew,_planCur.PlanNum,_planCurOriginal.SecDateTEdit);
				}
			}
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Error Code 12")+".  "+Lan.g(this,"Please contact support")+"\r\n"+"\r\n"+ex.Message+"\r\n"+ex.StackTrace);
				return;
			}
			#endregion 12
			#region 13
			try {
			Carrier carrierCur=Carriers.GetCarrier(_planCur.CarrierNum);
			if(_planCurOriginal.FeeSched!=0 && _planCurOriginal.FeeSched!=_planCur.FeeSched) {
				string feeSchedOld=FeeScheds.GetDescription(_planCurOriginal.FeeSched);
				string feeSchedNew=FeeScheds.GetDescription(_planCur.FeeSched);
				string logText=Lan.g(this,"The fee schedule associated with insurance plan number")+" "+_planCur.PlanNum.ToString()+" "+Lan.g(this,"for the carrier")+" "+carrierCur.CarrierName+" "+Lan.g(this,"was changed from")+" "+feeSchedOld+" "+Lan.g(this,"to")+" "+feeSchedNew;
				SecurityLogs.MakeLogEntry(Permissions.InsPlanEdit,PatPlanCur==null?0:PatPlanCur.PatNum,logText,(_planCur==null)?0:_planCur.PlanNum,
					_planCurOriginal.SecDateTEdit);
			}
			if(InsPlanCrud.UpdateComparison(_planCurOriginal,_planCur)) {
				string logText=Lan.g(this,"Insurance plan")+" "+_planCur.PlanNum.ToString()+" "+Lan.g(this,"for the carrier")+" "+carrierCur.CarrierName+" "+Lan.g(this,"has changed.");
				SecurityLogs.MakeLogEntry(Permissions.InsPlanEdit,PatPlanCur==null?0:PatPlanCur.PatNum,logText,(_planCur==null)?0:_planCur.PlanNum,
					_planCurOriginal.SecDateTEdit);
			}
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Error Code 13")+".  "+Lan.g(this,"Please contact support")+"\r\n"+"\r\n"+ex.Message+"\r\n"+ex.StackTrace);
				return;
			}
			#endregion 13
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormInsPlan_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(DialogResult==DialogResult.OK) {
				if(PatPlanCur!=null && (_hasDropped || _hasOrdinalChanged || _hasCarrierChanged || IsNewPatPlan || IsNewPlan)) {
					List<PatPlan> listPatPlans=PatPlans.Refresh(PatPlanCur.PatNum);
					InsSub sub1=new InsSub();
					InsSub sub2=new InsSub();
					sub1.PlanNum=0;
					sub2.PlanNum=0;
					if(listPatPlans.Count>=1) {
						sub1=InsSubs.GetOne(listPatPlans[0].InsSubNum);
					}
					if(listPatPlans.Count>=2) {
						sub2=InsSubs.GetOne(listPatPlans[1].InsSubNum);
					}
					Appointments.UpdateInsPlansForPat(PatPlanCur.PatNum,sub1.PlanNum,sub2.PlanNum);
				}
				if(IsNewPatPlan//Only when assigning new insurance
					&& PatPlanCur.Ordinal==1//Primary insurance.
					&& _planCur.BillingType!=0//Selection made.
					&& Security.IsAuthorized(Permissions.PatientBillingEdit,true)
					&& PrefC.GetBool(PrefName.PatInitBillingTypeFromPriInsPlan))
				{
					Patient patOld=Patients.GetPat(PatPlanCur.PatNum);
					if(patOld.BillingType!=_planCur.BillingType) {
						Patient patNew=patOld.Copy();
						patNew.BillingType=_planCur.BillingType;
						Patients.Update(patNew,patOld);
						//This needs to be the last call due to automation possibily leaving the form in a closing limbo.
						AutomationL.Trigger(AutomationTrigger.SetBillingType,null,patNew.PatNum);
					}
				}
				return;
			}
			//So, user cancelled a new entry
			if(IsNewPlan){//this would also be new coverage
				//warning: If user clicked 'pick from list' button, then we don't want to delete an existing plan used by others
				try {
					if(_subCur!=null) {
						InsSubs.Delete(_subCur.InsSubNum);
					}
					InsPlans.Delete(_planOld,canDeleteInsSub: false);//does dependency checking.
					Benefits.DeleteForPlan(_planOld.PlanNum);
					InsEditLogs.DeletePreInsertedLogsForPlanNum(_planOld.PlanNum);
					//Ok to delete these adjustments because we deleted the benefits in Benefits.DeleteForPlan().
					ClaimProcs.DeleteMany(AdjAL.ToArray().Cast<ClaimProc>().ToList());
				}
				catch(ApplicationException ex) {
					MessageBox.Show(ex.Message);
					SecurityLogs.MakeLogEntry(Permissions.InsPlanEdit,(PatPlanCur==null)?0:PatPlanCur.PatNum
						,Lan.g(this,"FormInsPlan_Closing delete validation failed.  Plan was not deleted."),_planOld.PlanNum,DateTime.MinValue);//new plan, no date needed.
					return;
				}
			}
			if(IsNewPatPlan){
				PatPlans.Delete(PatPlanCur.PatPlanNum);//no need to check dependencies.  Maintains ordinals and recomputes estimates.
			}
		}

		///<summary>This is related to insplan.PlanType, but that column is a string.
		///We should have used an enum instead of string values for insplan.PlanType to begin with.
		///However, too late to change now.  This enum makes this form more human readable.</summary>
		private enum InsPlanTypeComboItem {
			///<summary>0</summary>
			CategoryPercentage,
			///<summary>1</summary>
			PPO,
			///<summary>2</summary>
			PPOFixedBenefit,
			///<summary>3</summary>
			MedicaidOrFlatCopay,
			///<summary>4</summary>
			Capitation,
		}
	}
}
