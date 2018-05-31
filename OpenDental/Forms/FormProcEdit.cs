/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using OpenDentBusiness;
using CodeBase;
using SparksToothChart;
using OpenDental.UI;
using System.Threading;
using System.Linq;
using System.Text.RegularExpressions;

namespace OpenDental{
///<summary></summary>
	public class FormProcEdit : ODForm {
		private System.Windows.Forms.Label labelDate;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label labelAmount;
		private System.Windows.Forms.TextBox textProc;
		private System.Windows.Forms.TextBox textSurfaces;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox textDesc;
		private System.Windows.Forms.Label label7;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butDelete;
		private System.Windows.Forms.TextBox textRange;
		private System.Windows.Forms.Label labelTooth;
		private System.Windows.Forms.Label labelRange;
		private System.Windows.Forms.Label labelSurfaces;
		private System.Windows.Forms.GroupBox groupQuadrant;
		private System.Windows.Forms.RadioButton radioUR;
		private System.Windows.Forms.RadioButton radioUL;
		private System.Windows.Forms.RadioButton radioLL;
		private System.Windows.Forms.RadioButton radioLR;
		private System.Windows.Forms.GroupBox groupArch;
		private System.Windows.Forms.RadioButton radioU;
		private System.Windows.Forms.RadioButton radioL;
		private System.Windows.Forms.GroupBox groupSextant;
		private System.Windows.Forms.RadioButton radioS1;
		private System.Windows.Forms.RadioButton radioS3;
		private System.Windows.Forms.RadioButton radioS2;
		private System.Windows.Forms.RadioButton radioS4;
		private System.Windows.Forms.RadioButton radioS5;
		private System.Windows.Forms.RadioButton radioS6;
		private System.Windows.Forms.Label label9;
		///<summary>Mostly used for permissions.</summary>
		public bool IsNew;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label labelClaim;
		private System.Windows.Forms.ListBox listBoxTeeth;
		private System.Windows.Forms.ListBox listBoxTeeth2;
		private OpenDental.UI.Button butChange;
		//private ProcStat OriginalStatus;
		private ErrorProvider errorProvider2=new ErrorProvider();
		private System.Windows.Forms.TextBox textTooth;
		private OpenDental.UI.Button butEditAnyway;
		private System.Windows.Forms.Label labelDx;
		private System.Windows.Forms.ComboBox comboPlaceService;
		private System.Windows.Forms.Label labelPlaceService;
		private OpenDental.UI.Button butSetComplete;
		private System.Windows.Forms.Label labelPriority;
		private ProcedureCode ProcedureCode2;
		private System.Windows.Forms.Label labelSetComplete;
		private OpenDental.UI.Button butAddEstimate;
		private Procedure ProcCur;
		private Procedure ProcOld;
		//private List<ClaimProc> ClaimProcList;
		private OpenDental.ValidDouble textProcFee;
		private System.Windows.Forms.CheckBox checkNoBillIns;
		private OpenDental.ODtextBox textNotes;
		private List<ClaimProc> ClaimProcsForProc;
		//private Adjustment[] AdjForProc;
		private ArrayList PaySplitsForProc;
		private ArrayList AdjustmentsForProc;
		private Patient PatCur;
		private Family FamCur;
		private OpenDental.UI.Button butAddAdjust;
		private List <InsPlan> PlanList;
		///<summary>List of substitution links.  Lazy loaded, do not directly use this variable, use the property instead.</summary>
		private List<SubstitutionLink> _listSubLinks=null;
		private System.Windows.Forms.Label labelIncomplete;
		private OpenDental.ValidDate textDateEntry;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ComboBox comboClinic;
		private System.Windows.Forms.Label labelClinic;
		///<summary>List of all payments (not paysplits) that this procedure is attached to.</summary>
		private List<Payment> PaymentsForProc;
		//private uint m_autoAPIMsg;//ENP
		private const string APPBAR_AUTOMATION_API_MESSAGE = "EZNotes.AppBarStandalone.Auto.API.Message"; 
		private const uint MSG_RESTORE=2;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textMedicalCode;
		private System.Windows.Forms.Label labelDiagnosticCode1;
		private System.Windows.Forms.TextBox textDiagnosticCode;//ENP
		private const uint MSG_GETLASTNOTE=3;
		private System.Windows.Forms.CheckBox checkIsPrincDiag;//ENP
		private List<PatPlan> PatPlanList;
		private Label label14;
		private Label label15;
		private Label label16;
		private List <Benefit> BenefitList;
		private bool SigChanged;
		private ComboBox comboProv;
		private ComboBox comboDx;
		private ComboBox comboPriority;
		private TextBox textUser;
		//private Label label17;
		//private Label label18;
		private Label label19;
		private TextBox textCodeMod1;
		private ComboBox comboBillingTypeTwo;
		private Label labelBillingTypeTwo;
		private ComboBox comboBillingTypeOne;
		private Label labelBillingTypeOne;
		private TextBox textCodeMod4;
		private TextBox textCodeMod3;
		private TextBox textCodeMod2;
		private TextBox textRevCode;
		private Label label22;
		private TextBox textUnitQty;
		private Label label21;
		private OpenDental.UI.Button buttonUseAutoNote;
		private ToolTip toolTip1;
		private IContainer components;
		private ValidDate textDateTP;
		private Label label27;
		///<summary>This keeps the noteChanged event from erasing the signature when first loading.</summary>
		private bool IsStartingUp;
		private List<Claim> ClaimList;
		//private OpenDental.UI.Button butTopazSign;
		private Panel panelSurfaces;
		private OpenDental.UI.Button butD;
		private OpenDental.UI.Button butBF;
		private OpenDental.UI.Button butL;
		private OpenDental.UI.Button butM;
		private OpenDental.UI.Button butV;
		private OpenDental.UI.Button butOI;
    //private bool allowTopaz;
		private OpenDental.UI.Button butPickSite;
		private TextBox textSite;
		private Label labelSite;
		private ODGrid gridIns;
		private bool StartedAttachedToClaim;
		public List<ClaimProcHist> HistList;
		private OpenDental.UI.Button butPickProv;
		private CheckBox checkHideGraphics;
		private Label label3;
		private Label label4;
		private ValidDate textDateOriginalProsth;
		private ListBox listProsth;
		private GroupBox groupProsth;
		private CheckBox checkTypeCodeA;
		private CheckBox checkTypeCodeB;
		private CheckBox checkTypeCodeC;
		private CheckBox checkTypeCodeE;
		private CheckBox checkTypeCodeL;
		private CheckBox checkTypeCodeX;
		private CheckBox checkTypeCodeS;
		private GroupBox groupCanadianProcTypeCode;
		private Label labelDateStop;
		private Label labelDateSched;
		private Label labelDPC;
		private Label labelStatus;
		private ComboBox comboStatus;
		private ValidDate textDateScheduled;
		private ComboBox comboDPC;
		private ValidDate textDateStop;
		private CheckBox checkIsEffComm;
		private CheckBox checkIsOnCall;
		private CheckBox checkIsRepair;
		public List<ClaimProcHist> LoopList;
		private Label labelEndTime;
		private OpenDental.UI.Button butNow;
		private ValidDate textDate;
		private TextBox textTimeEnd;
		private Label labelScheduleBy;
		private OrionProc OrionProcCur;
		private OrionProc OrionProcOld;
		private DateTime CancelledScheduleByDate;
		public long OrionProvNum;
		public bool OrionDentist;
		private TextBox textTimeStart;
		private Label labelStartTime;
		private Label labelDPCpost;
		private ComboBox comboDPCpost;
		private ComboBox comboPrognosis;
		private Label labelPrognosis;
		private ComboBox comboProcStatus;
		private ComboBox comboDrugUnit;
		private Label label1;
		private Label label5;
		private TextBox textDrugNDC;
		private Label label10;
		private TextBox textDrugQty;
		private Label label13;
		private TextBox textReferral;
		private UI.Button butReferral;
		private Label labelClaimNote;
		private ODtextBox textClaimNote;
		private TextBox textTimeFinal;
		private Label labelTimeFinal;
		private TabControl tabControl;
		private TabPage tabPageFinancial;
		private TabPage tabPageMedical;
		private TabPage tabPageMisc;
		private TabPage tabPageCanada;
		private TabPage tabPageOrion;
		private Label label17;
		private ComboBox comboUnitType;
		private Label labelCanadaLabFee2;
		private Label labelCanadaLabFee1;
		private ValidDouble textCanadaLabFee2;
		private ValidDouble textCanadaLabFee1;
		private List<InsSub> SubList;
		private Label labelLocked;
		private UI.Button butAppend;
		private UI.Button butLock;
		private UI.Button butInvalidate;
		private Label label18;
		private TextBox textBillingNote;
		private UI.Button butSearch;
		private UI.Button butSnomedBodySiteSelect;
		private Label labelSnomedCtBodySite;
		private TextBox textSnomedBodySite;
		private List<Procedure> canadaLabFees;
		private UI.Button butNoneSnomedBodySite;
		private TextBox textDiagnosticCode2;
		private Label labelDiagnosticCode2;
		private TextBox textDiagnosticCode3;
		private Label labelDiagnosticCode3;
		private TextBox textDiagnosticCode4;
		private Label labelDiagnosticCode4;
		private UI.Button butNoneDiagnosisCode1;
		private UI.Button butDiagnosisCode1;
		private UI.Button butNoneDiagnosisCode2;
		private UI.Button butDiagnosisCode2;
		private UI.Button butNoneDiagnosisCode4;
		private UI.Button butDiagnosisCode4;
		private UI.Button butNoneDiagnosisCode3;
		private UI.Button butDiagnosisCode3;
		private Label label20;
		private ValidDouble textDiscount;
		private Snomed _snomedBodySite=null;
		private CheckBox checkIsDateProsthEst;
		private CheckBox checkIcdVersion;
		private Label label11;
		private ODGrid gridAdj;
		private ODGrid gridPay;
		private SignatureBoxWrapper signatureBoxWrapper;
		private UI.Button butChangeUser;
		private bool _isQuickAdd=false;
		///<summary>Users can temporarily log in on this form.  Defaults to Security.CurUser.</summary>
		private Userod _curUser=Security.CurUser;
		private UI.Button butAddExistAdj;

		///<summary>True if the user clicked the Change User button.</summary>
		private bool _hasUserChanged;
		///<summary>Cached list of clinics available to user. Also includes a dummy Clinic at index 0 for "none".</summary>
		private List<Clinic> _listClinics;
		///<summary>Filtered list of providers based on which clinic is selected. If no clinic is selected displays all providers. Also includes a dummy clinic at index 0 for "none"</summary>
		private List<Provider> _listProviders;
		///<summary>Used to keep track of the current clinic selected. This is because it may be a clinic that is not in _listClinics.</summary>
		private long _selectedClinicNum;
		///<summary>Instead of relying on _listProviders[comboProv.SelectedIndex] to determine the selected Provider we use this variable to store it explicitly.</summary>
		private long _selectedProvNum;
		///<summary>Instead of relying on _listProviders[comboProvHyg.SelectedIndex] to determine the selected Provider we use this variable to store it explicitly.</summary>
		private long _selectedProvOrderNum;
		///<summary>If this procedure is attached to an ordering referral, then this varible will not be null.</summary>
		private Referral _referralOrdering=null;
		private ValidDate textOrigDateComp;
		private Label labelOrigDateComp;
		private ODtextBox textOrderingProviderOverride;
		private UI.Button butPickOrderProvReferral;
		private UI.Button butNoneOrderProv;
		private UI.Button butPickOrderProvInternal;
		private GroupBox groupBox1;
		private Label labelPermAlert;
		private UI.Button butEditAutoNote;
		private const string _autoNotePromptRegex=@"\[Prompt:""[a-zA-Z_0-9 ]+""\]";
		///<summary>True only when modifications to this canadian lab proc will affect the attached parent proc ins estimate.</summary>
		private bool _isEstimateRecompute=false;
		private List<Def> _listDiagnosisDefs;
		private List<Def> _listPrognosisDefs;
		private List<Def> _listTxPriorityDefs;
		private List<Def> _listBillingTypeDefs;
		///<summary>Most of the data necessary to load this form.</summary>
		private ProcEdit.LoadData _loadData;

		private List<SubstitutionLink> ListSubLinks {
			get {
				if(_listSubLinks==null) {
					_listSubLinks=SubstitutionLinks.GetAllForPlans(PlanList);
				}
				return _listSubLinks;
			}
		}

		///<summary>Inserts are not done within this dialog, but must be done ahead of time from outside.  You must specify a procedure to edit, and only the changes that are made in this dialog get saved.  Only used when double click in Account, Chart, TP, and in ContrChart.AddProcedure().  The procedure may be deleted if new, and user hits Cancel.</summary>
		public FormProcEdit(Procedure proc,Patient patCur,Family famCur,bool isQuickAdd=false) {
			ProcCur=proc;
			ProcOld=proc.Copy();
			PatCur=patCur;
			FamCur=famCur;
			//HistList=null;
			//LoopList=null;
			InitializeComponent();
			Lan.F(this);
			if(!PrefC.GetBool(PrefName.ShowFeatureMedicalInsurance)) {
				tabControl.TabPages.Remove(tabPageMedical);
				//groupMedical.Visible=false;
			}
			_isQuickAdd=isQuickAdd;
			if(isQuickAdd) {
				this.WindowState=FormWindowState.Minimized;
			}
		}

		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProcEdit));
			this.labelDate = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.labelTooth = new System.Windows.Forms.Label();
			this.labelSurfaces = new System.Windows.Forms.Label();
			this.labelAmount = new System.Windows.Forms.Label();
			this.textProc = new System.Windows.Forms.TextBox();
			this.textTooth = new System.Windows.Forms.TextBox();
			this.textSurfaces = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textDesc = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.labelRange = new System.Windows.Forms.Label();
			this.textRange = new System.Windows.Forms.TextBox();
			this.groupQuadrant = new System.Windows.Forms.GroupBox();
			this.radioLR = new System.Windows.Forms.RadioButton();
			this.radioLL = new System.Windows.Forms.RadioButton();
			this.radioUL = new System.Windows.Forms.RadioButton();
			this.radioUR = new System.Windows.Forms.RadioButton();
			this.groupArch = new System.Windows.Forms.GroupBox();
			this.radioL = new System.Windows.Forms.RadioButton();
			this.radioU = new System.Windows.Forms.RadioButton();
			this.panelSurfaces = new System.Windows.Forms.Panel();
			this.butD = new OpenDental.UI.Button();
			this.butBF = new OpenDental.UI.Button();
			this.butL = new OpenDental.UI.Button();
			this.butM = new OpenDental.UI.Button();
			this.butV = new OpenDental.UI.Button();
			this.butOI = new OpenDental.UI.Button();
			this.groupSextant = new System.Windows.Forms.GroupBox();
			this.radioS6 = new System.Windows.Forms.RadioButton();
			this.radioS5 = new System.Windows.Forms.RadioButton();
			this.radioS4 = new System.Windows.Forms.RadioButton();
			this.radioS2 = new System.Windows.Forms.RadioButton();
			this.radioS3 = new System.Windows.Forms.RadioButton();
			this.radioS1 = new System.Windows.Forms.RadioButton();
			this.label9 = new System.Windows.Forms.Label();
			this.labelDx = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.textOrigDateComp = new OpenDental.ValidDate();
			this.labelOrigDateComp = new System.Windows.Forms.Label();
			this.textTimeFinal = new System.Windows.Forms.TextBox();
			this.textTimeStart = new System.Windows.Forms.TextBox();
			this.textTimeEnd = new System.Windows.Forms.TextBox();
			this.textDate = new OpenDental.ValidDate();
			this.butNow = new OpenDental.UI.Button();
			this.textDateTP = new OpenDental.ValidDate();
			this.label27 = new System.Windows.Forms.Label();
			this.listBoxTeeth = new System.Windows.Forms.ListBox();
			this.textDateEntry = new OpenDental.ValidDate();
			this.label12 = new System.Windows.Forms.Label();
			this.textProcFee = new OpenDental.ValidDouble();
			this.labelStartTime = new System.Windows.Forms.Label();
			this.labelEndTime = new System.Windows.Forms.Label();
			this.listBoxTeeth2 = new System.Windows.Forms.ListBox();
			this.butChange = new OpenDental.UI.Button();
			this.labelTimeFinal = new System.Windows.Forms.Label();
			this.textDrugQty = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.textDrugNDC = new System.Windows.Forms.TextBox();
			this.comboDrugUnit = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textRevCode = new System.Windows.Forms.TextBox();
			this.label22 = new System.Windows.Forms.Label();
			this.textUnitQty = new System.Windows.Forms.TextBox();
			this.label21 = new System.Windows.Forms.Label();
			this.textCodeMod4 = new System.Windows.Forms.TextBox();
			this.textCodeMod3 = new System.Windows.Forms.TextBox();
			this.textCodeMod2 = new System.Windows.Forms.TextBox();
			this.label19 = new System.Windows.Forms.Label();
			this.textCodeMod1 = new System.Windows.Forms.TextBox();
			this.checkIsPrincDiag = new System.Windows.Forms.CheckBox();
			this.labelDiagnosticCode1 = new System.Windows.Forms.Label();
			this.textDiagnosticCode = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.textMedicalCode = new System.Windows.Forms.TextBox();
			this.labelClaim = new System.Windows.Forms.Label();
			this.comboPlaceService = new System.Windows.Forms.ComboBox();
			this.labelPlaceService = new System.Windows.Forms.Label();
			this.labelPriority = new System.Windows.Forms.Label();
			this.labelSetComplete = new System.Windows.Forms.Label();
			this.checkNoBillIns = new System.Windows.Forms.CheckBox();
			this.labelIncomplete = new System.Windows.Forms.Label();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.comboPriority = new System.Windows.Forms.ComboBox();
			this.comboDx = new System.Windows.Forms.ComboBox();
			this.comboProv = new System.Windows.Forms.ComboBox();
			this.textUser = new System.Windows.Forms.TextBox();
			this.comboBillingTypeTwo = new System.Windows.Forms.ComboBox();
			this.labelBillingTypeTwo = new System.Windows.Forms.Label();
			this.comboBillingTypeOne = new System.Windows.Forms.ComboBox();
			this.labelBillingTypeOne = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.textSite = new System.Windows.Forms.TextBox();
			this.labelSite = new System.Windows.Forms.Label();
			this.checkHideGraphics = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.listProsth = new System.Windows.Forms.ListBox();
			this.groupProsth = new System.Windows.Forms.GroupBox();
			this.checkIsDateProsthEst = new System.Windows.Forms.CheckBox();
			this.textDateOriginalProsth = new OpenDental.ValidDate();
			this.checkTypeCodeA = new System.Windows.Forms.CheckBox();
			this.checkTypeCodeB = new System.Windows.Forms.CheckBox();
			this.checkTypeCodeC = new System.Windows.Forms.CheckBox();
			this.checkTypeCodeE = new System.Windows.Forms.CheckBox();
			this.checkTypeCodeL = new System.Windows.Forms.CheckBox();
			this.checkTypeCodeX = new System.Windows.Forms.CheckBox();
			this.checkTypeCodeS = new System.Windows.Forms.CheckBox();
			this.groupCanadianProcTypeCode = new System.Windows.Forms.GroupBox();
			this.labelDPCpost = new System.Windows.Forms.Label();
			this.comboDPCpost = new System.Windows.Forms.ComboBox();
			this.labelScheduleBy = new System.Windows.Forms.Label();
			this.checkIsRepair = new System.Windows.Forms.CheckBox();
			this.checkIsEffComm = new System.Windows.Forms.CheckBox();
			this.checkIsOnCall = new System.Windows.Forms.CheckBox();
			this.comboDPC = new System.Windows.Forms.ComboBox();
			this.comboStatus = new System.Windows.Forms.ComboBox();
			this.labelStatus = new System.Windows.Forms.Label();
			this.labelDateStop = new System.Windows.Forms.Label();
			this.labelDateSched = new System.Windows.Forms.Label();
			this.labelDPC = new System.Windows.Forms.Label();
			this.comboPrognosis = new System.Windows.Forms.ComboBox();
			this.labelPrognosis = new System.Windows.Forms.Label();
			this.comboProcStatus = new System.Windows.Forms.ComboBox();
			this.label13 = new System.Windows.Forms.Label();
			this.textReferral = new System.Windows.Forms.TextBox();
			this.labelClaimNote = new System.Windows.Forms.Label();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPageFinancial = new System.Windows.Forms.TabPage();
			this.butAddExistAdj = new OpenDental.UI.Button();
			this.gridPay = new OpenDental.UI.ODGrid();
			this.gridAdj = new OpenDental.UI.ODGrid();
			this.label20 = new System.Windows.Forms.Label();
			this.textDiscount = new OpenDental.ValidDouble();
			this.butAddEstimate = new OpenDental.UI.Button();
			this.butAddAdjust = new OpenDental.UI.Button();
			this.gridIns = new OpenDental.UI.ODGrid();
			this.tabPageMedical = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.butPickOrderProvInternal = new OpenDental.UI.Button();
			this.textOrderingProviderOverride = new OpenDental.ODtextBox();
			this.butPickOrderProvReferral = new OpenDental.UI.Button();
			this.butNoneOrderProv = new OpenDental.UI.Button();
			this.label11 = new System.Windows.Forms.Label();
			this.checkIcdVersion = new System.Windows.Forms.CheckBox();
			this.butNoneDiagnosisCode1 = new OpenDental.UI.Button();
			this.butDiagnosisCode1 = new OpenDental.UI.Button();
			this.butNoneDiagnosisCode2 = new OpenDental.UI.Button();
			this.butDiagnosisCode2 = new OpenDental.UI.Button();
			this.butNoneDiagnosisCode4 = new OpenDental.UI.Button();
			this.butDiagnosisCode4 = new OpenDental.UI.Button();
			this.butNoneDiagnosisCode3 = new OpenDental.UI.Button();
			this.butDiagnosisCode3 = new OpenDental.UI.Button();
			this.textDiagnosticCode2 = new System.Windows.Forms.TextBox();
			this.labelDiagnosticCode2 = new System.Windows.Forms.Label();
			this.textDiagnosticCode3 = new System.Windows.Forms.TextBox();
			this.labelDiagnosticCode3 = new System.Windows.Forms.Label();
			this.textDiagnosticCode4 = new System.Windows.Forms.TextBox();
			this.labelDiagnosticCode4 = new System.Windows.Forms.Label();
			this.butNoneSnomedBodySite = new OpenDental.UI.Button();
			this.butSnomedBodySiteSelect = new OpenDental.UI.Button();
			this.labelSnomedCtBodySite = new System.Windows.Forms.Label();
			this.textSnomedBodySite = new System.Windows.Forms.TextBox();
			this.label17 = new System.Windows.Forms.Label();
			this.comboUnitType = new System.Windows.Forms.ComboBox();
			this.tabPageMisc = new System.Windows.Forms.TabPage();
			this.textBillingNote = new System.Windows.Forms.TextBox();
			this.label18 = new System.Windows.Forms.Label();
			this.butPickSite = new OpenDental.UI.Button();
			this.tabPageCanada = new System.Windows.Forms.TabPage();
			this.labelCanadaLabFee2 = new System.Windows.Forms.Label();
			this.labelCanadaLabFee1 = new System.Windows.Forms.Label();
			this.textCanadaLabFee2 = new OpenDental.ValidDouble();
			this.textCanadaLabFee1 = new OpenDental.ValidDouble();
			this.tabPageOrion = new System.Windows.Forms.TabPage();
			this.textDateStop = new OpenDental.ValidDate();
			this.textDateScheduled = new OpenDental.ValidDate();
			this.labelLocked = new System.Windows.Forms.Label();
			this.butSearch = new OpenDental.UI.Button();
			this.butLock = new OpenDental.UI.Button();
			this.butInvalidate = new OpenDental.UI.Button();
			this.butAppend = new OpenDental.UI.Button();
			this.textClaimNote = new OpenDental.ODtextBox();
			this.butReferral = new OpenDental.UI.Button();
			this.butPickProv = new OpenDental.UI.Button();
			this.buttonUseAutoNote = new OpenDental.UI.Button();
			this.textNotes = new OpenDental.ODtextBox();
			this.butSetComplete = new OpenDental.UI.Button();
			this.butEditAnyway = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.signatureBoxWrapper = new OpenDental.UI.SignatureBoxWrapper();
			this.butChangeUser = new OpenDental.UI.Button();
			this.labelPermAlert = new System.Windows.Forms.Label();
			this.butEditAutoNote = new OpenDental.UI.Button();
			this.groupQuadrant.SuspendLayout();
			this.groupArch.SuspendLayout();
			this.panelSurfaces.SuspendLayout();
			this.groupSextant.SuspendLayout();
			this.panel1.SuspendLayout();
			this.groupProsth.SuspendLayout();
			this.groupCanadianProcTypeCode.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.tabPageFinancial.SuspendLayout();
			this.tabPageMedical.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabPageMisc.SuspendLayout();
			this.tabPageCanada.SuspendLayout();
			this.tabPageOrion.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelDate
			// 
			this.labelDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelDate.Location = new System.Drawing.Point(8, 45);
			this.labelDate.Name = "labelDate";
			this.labelDate.Size = new System.Drawing.Size(96, 14);
			this.labelDate.TabIndex = 0;
			this.labelDate.Text = "Date";
			this.labelDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(26, 63);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(79, 12);
			this.label2.TabIndex = 1;
			this.label2.Text = "Procedure";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelTooth
			// 
			this.labelTooth.Location = new System.Drawing.Point(68, 107);
			this.labelTooth.Name = "labelTooth";
			this.labelTooth.Size = new System.Drawing.Size(36, 12);
			this.labelTooth.TabIndex = 2;
			this.labelTooth.Text = "Tooth";
			this.labelTooth.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.labelTooth.Visible = false;
			// 
			// labelSurfaces
			// 
			this.labelSurfaces.Location = new System.Drawing.Point(33, 135);
			this.labelSurfaces.Name = "labelSurfaces";
			this.labelSurfaces.Size = new System.Drawing.Size(73, 16);
			this.labelSurfaces.TabIndex = 3;
			this.labelSurfaces.Text = "Surfaces";
			this.labelSurfaces.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.labelSurfaces.Visible = false;
			// 
			// labelAmount
			// 
			this.labelAmount.Location = new System.Drawing.Point(30, 158);
			this.labelAmount.Name = "labelAmount";
			this.labelAmount.Size = new System.Drawing.Size(75, 16);
			this.labelAmount.TabIndex = 4;
			this.labelAmount.Text = "Amount";
			this.labelAmount.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textProc
			// 
			this.textProc.Location = new System.Drawing.Point(106, 61);
			this.textProc.Name = "textProc";
			this.textProc.ReadOnly = true;
			this.textProc.Size = new System.Drawing.Size(76, 20);
			this.textProc.TabIndex = 6;
			// 
			// textTooth
			// 
			this.textTooth.Location = new System.Drawing.Point(106, 105);
			this.textTooth.Name = "textTooth";
			this.textTooth.Size = new System.Drawing.Size(35, 20);
			this.textTooth.TabIndex = 7;
			this.textTooth.Visible = false;
			this.textTooth.Validating += new System.ComponentModel.CancelEventHandler(this.textTooth_Validating);
			// 
			// textSurfaces
			// 
			this.textSurfaces.Location = new System.Drawing.Point(106, 133);
			this.textSurfaces.Name = "textSurfaces";
			this.textSurfaces.Size = new System.Drawing.Size(68, 20);
			this.textSurfaces.TabIndex = 4;
			this.textSurfaces.Visible = false;
			this.textSurfaces.TextChanged += new System.EventHandler(this.textSurfaces_TextChanged);
			this.textSurfaces.Validating += new System.ComponentModel.CancelEventHandler(this.textSurfaces_Validating);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(2, 81);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(103, 16);
			this.label6.TabIndex = 13;
			this.label6.Text = "Description";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDesc
			// 
			this.textDesc.BackColor = System.Drawing.SystemColors.Control;
			this.textDesc.Location = new System.Drawing.Point(106, 81);
			this.textDesc.Name = "textDesc";
			this.textDesc.Size = new System.Drawing.Size(216, 20);
			this.textDesc.TabIndex = 16;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(429, 163);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(73, 16);
			this.label7.TabIndex = 0;
			this.label7.Text = "&Notes";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelRange
			// 
			this.labelRange.Location = new System.Drawing.Point(24, 107);
			this.labelRange.Name = "labelRange";
			this.labelRange.Size = new System.Drawing.Size(82, 16);
			this.labelRange.TabIndex = 33;
			this.labelRange.Text = "Tooth Range";
			this.labelRange.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.labelRange.Visible = false;
			// 
			// textRange
			// 
			this.textRange.Location = new System.Drawing.Point(106, 105);
			this.textRange.Name = "textRange";
			this.textRange.Size = new System.Drawing.Size(100, 20);
			this.textRange.TabIndex = 34;
			this.textRange.Visible = false;
			// 
			// groupQuadrant
			// 
			this.groupQuadrant.Controls.Add(this.radioLR);
			this.groupQuadrant.Controls.Add(this.radioLL);
			this.groupQuadrant.Controls.Add(this.radioUL);
			this.groupQuadrant.Controls.Add(this.radioUR);
			this.groupQuadrant.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupQuadrant.Location = new System.Drawing.Point(104, 99);
			this.groupQuadrant.Name = "groupQuadrant";
			this.groupQuadrant.Size = new System.Drawing.Size(108, 56);
			this.groupQuadrant.TabIndex = 36;
			this.groupQuadrant.TabStop = false;
			this.groupQuadrant.Text = "Quadrant";
			this.groupQuadrant.Visible = false;
			// 
			// radioLR
			// 
			this.radioLR.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioLR.Location = new System.Drawing.Point(12, 36);
			this.radioLR.Name = "radioLR";
			this.radioLR.Size = new System.Drawing.Size(40, 16);
			this.radioLR.TabIndex = 3;
			this.radioLR.Text = "LR";
			this.radioLR.Click += new System.EventHandler(this.radioLR_Click);
			// 
			// radioLL
			// 
			this.radioLL.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioLL.Location = new System.Drawing.Point(64, 36);
			this.radioLL.Name = "radioLL";
			this.radioLL.Size = new System.Drawing.Size(40, 16);
			this.radioLL.TabIndex = 1;
			this.radioLL.Text = "LL";
			this.radioLL.Click += new System.EventHandler(this.radioLL_Click);
			// 
			// radioUL
			// 
			this.radioUL.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioUL.Location = new System.Drawing.Point(64, 16);
			this.radioUL.Name = "radioUL";
			this.radioUL.Size = new System.Drawing.Size(40, 16);
			this.radioUL.TabIndex = 0;
			this.radioUL.Text = "UL";
			this.radioUL.Click += new System.EventHandler(this.radioUL_Click);
			// 
			// radioUR
			// 
			this.radioUR.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioUR.Location = new System.Drawing.Point(12, 16);
			this.radioUR.Name = "radioUR";
			this.radioUR.Size = new System.Drawing.Size(40, 16);
			this.radioUR.TabIndex = 0;
			this.radioUR.Text = "UR";
			this.radioUR.Click += new System.EventHandler(this.radioUR_Click);
			// 
			// groupArch
			// 
			this.groupArch.Controls.Add(this.radioL);
			this.groupArch.Controls.Add(this.radioU);
			this.groupArch.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupArch.Location = new System.Drawing.Point(104, 99);
			this.groupArch.Name = "groupArch";
			this.groupArch.Size = new System.Drawing.Size(60, 56);
			this.groupArch.TabIndex = 3;
			this.groupArch.TabStop = false;
			this.groupArch.Text = "Arch";
			this.groupArch.Visible = false;
			this.groupArch.Validating += new System.ComponentModel.CancelEventHandler(this.groupArch_Validating);
			// 
			// radioL
			// 
			this.radioL.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioL.Location = new System.Drawing.Point(12, 36);
			this.radioL.Name = "radioL";
			this.radioL.Size = new System.Drawing.Size(28, 16);
			this.radioL.TabIndex = 1;
			this.radioL.Text = "L";
			this.radioL.Click += new System.EventHandler(this.radioL_Click);
			// 
			// radioU
			// 
			this.radioU.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioU.Location = new System.Drawing.Point(12, 16);
			this.radioU.Name = "radioU";
			this.radioU.Size = new System.Drawing.Size(32, 16);
			this.radioU.TabIndex = 0;
			this.radioU.Text = "U";
			this.radioU.Click += new System.EventHandler(this.radioU_Click);
			// 
			// panelSurfaces
			// 
			this.panelSurfaces.Controls.Add(this.butD);
			this.panelSurfaces.Controls.Add(this.butBF);
			this.panelSurfaces.Controls.Add(this.butL);
			this.panelSurfaces.Controls.Add(this.butM);
			this.panelSurfaces.Controls.Add(this.butV);
			this.panelSurfaces.Controls.Add(this.butOI);
			this.panelSurfaces.Location = new System.Drawing.Point(188, 106);
			this.panelSurfaces.Name = "panelSurfaces";
			this.panelSurfaces.Size = new System.Drawing.Size(96, 66);
			this.panelSurfaces.TabIndex = 100;
			this.panelSurfaces.Visible = false;
			// 
			// butD
			// 
			this.butD.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butD.Autosize = true;
			this.butD.BackColor = System.Drawing.SystemColors.Control;
			this.butD.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butD.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butD.CornerRadius = 4F;
			this.butD.Location = new System.Drawing.Point(61, 23);
			this.butD.Name = "butD";
			this.butD.Size = new System.Drawing.Size(24, 20);
			this.butD.TabIndex = 27;
			this.butD.Text = "D";
			this.butD.UseVisualStyleBackColor = false;
			this.butD.Click += new System.EventHandler(this.butD_Click);
			// 
			// butBF
			// 
			this.butBF.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBF.Autosize = true;
			this.butBF.BackColor = System.Drawing.SystemColors.Control;
			this.butBF.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBF.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBF.CornerRadius = 4F;
			this.butBF.Location = new System.Drawing.Point(22, 3);
			this.butBF.Name = "butBF";
			this.butBF.Size = new System.Drawing.Size(28, 20);
			this.butBF.TabIndex = 28;
			this.butBF.Text = "B/F";
			this.butBF.UseVisualStyleBackColor = false;
			this.butBF.Click += new System.EventHandler(this.butBF_Click);
			// 
			// butL
			// 
			this.butL.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butL.Autosize = true;
			this.butL.BackColor = System.Drawing.SystemColors.Control;
			this.butL.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butL.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butL.CornerRadius = 4F;
			this.butL.Location = new System.Drawing.Point(32, 43);
			this.butL.Name = "butL";
			this.butL.Size = new System.Drawing.Size(24, 20);
			this.butL.TabIndex = 29;
			this.butL.Text = "L";
			this.butL.UseVisualStyleBackColor = false;
			this.butL.Click += new System.EventHandler(this.butL_Click);
			// 
			// butM
			// 
			this.butM.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butM.Autosize = true;
			this.butM.BackColor = System.Drawing.SystemColors.Control;
			this.butM.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butM.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butM.CornerRadius = 4F;
			this.butM.Location = new System.Drawing.Point(3, 23);
			this.butM.Name = "butM";
			this.butM.Size = new System.Drawing.Size(24, 20);
			this.butM.TabIndex = 25;
			this.butM.Text = "M";
			this.butM.UseVisualStyleBackColor = false;
			this.butM.Click += new System.EventHandler(this.butM_Click);
			// 
			// butV
			// 
			this.butV.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butV.Autosize = true;
			this.butV.BackColor = System.Drawing.SystemColors.Control;
			this.butV.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butV.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butV.CornerRadius = 4F;
			this.butV.Location = new System.Drawing.Point(50, 3);
			this.butV.Name = "butV";
			this.butV.Size = new System.Drawing.Size(17, 20);
			this.butV.TabIndex = 30;
			this.butV.Text = "V";
			this.butV.UseVisualStyleBackColor = false;
			this.butV.Click += new System.EventHandler(this.butV_Click);
			// 
			// butOI
			// 
			this.butOI.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOI.Autosize = true;
			this.butOI.BackColor = System.Drawing.SystemColors.Control;
			this.butOI.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOI.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOI.CornerRadius = 4F;
			this.butOI.Location = new System.Drawing.Point(27, 23);
			this.butOI.Name = "butOI";
			this.butOI.Size = new System.Drawing.Size(34, 20);
			this.butOI.TabIndex = 26;
			this.butOI.Text = "O/I";
			this.butOI.UseVisualStyleBackColor = false;
			this.butOI.Click += new System.EventHandler(this.butOI_Click);
			// 
			// groupSextant
			// 
			this.groupSextant.Controls.Add(this.radioS6);
			this.groupSextant.Controls.Add(this.radioS5);
			this.groupSextant.Controls.Add(this.radioS4);
			this.groupSextant.Controls.Add(this.radioS2);
			this.groupSextant.Controls.Add(this.radioS3);
			this.groupSextant.Controls.Add(this.radioS1);
			this.groupSextant.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupSextant.Location = new System.Drawing.Point(104, 99);
			this.groupSextant.Name = "groupSextant";
			this.groupSextant.Size = new System.Drawing.Size(156, 56);
			this.groupSextant.TabIndex = 5;
			this.groupSextant.TabStop = false;
			this.groupSextant.Text = "Sextant";
			this.groupSextant.Visible = false;
			this.groupSextant.Validating += new System.ComponentModel.CancelEventHandler(this.groupSextant_Validating);
			// 
			// radioS6
			// 
			this.radioS6.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioS6.Location = new System.Drawing.Point(12, 36);
			this.radioS6.Name = "radioS6";
			this.radioS6.Size = new System.Drawing.Size(36, 16);
			this.radioS6.TabIndex = 5;
			this.radioS6.Text = "6";
			this.radioS6.Click += new System.EventHandler(this.radioS6_Click);
			// 
			// radioS5
			// 
			this.radioS5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioS5.Location = new System.Drawing.Point(60, 36);
			this.radioS5.Name = "radioS5";
			this.radioS5.Size = new System.Drawing.Size(36, 16);
			this.radioS5.TabIndex = 4;
			this.radioS5.Text = "5";
			this.radioS5.Click += new System.EventHandler(this.radioS5_Click);
			// 
			// radioS4
			// 
			this.radioS4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioS4.Location = new System.Drawing.Point(108, 36);
			this.radioS4.Name = "radioS4";
			this.radioS4.Size = new System.Drawing.Size(36, 16);
			this.radioS4.TabIndex = 1;
			this.radioS4.Text = "4";
			this.radioS4.Click += new System.EventHandler(this.radioS4_Click);
			// 
			// radioS2
			// 
			this.radioS2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioS2.Location = new System.Drawing.Point(60, 16);
			this.radioS2.Name = "radioS2";
			this.radioS2.Size = new System.Drawing.Size(36, 16);
			this.radioS2.TabIndex = 2;
			this.radioS2.Text = "2";
			this.radioS2.Click += new System.EventHandler(this.radioS2_Click);
			// 
			// radioS3
			// 
			this.radioS3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioS3.Location = new System.Drawing.Point(108, 16);
			this.radioS3.Name = "radioS3";
			this.radioS3.Size = new System.Drawing.Size(36, 16);
			this.radioS3.TabIndex = 0;
			this.radioS3.Text = "3";
			this.radioS3.Click += new System.EventHandler(this.radioS3_Click);
			// 
			// radioS1
			// 
			this.radioS1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioS1.Location = new System.Drawing.Point(12, 16);
			this.radioS1.Name = "radioS1";
			this.radioS1.Size = new System.Drawing.Size(36, 16);
			this.radioS1.TabIndex = 0;
			this.radioS1.Text = "1";
			this.radioS1.Click += new System.EventHandler(this.radioS1_Click);
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(5, 221);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(100, 14);
			this.label9.TabIndex = 45;
			this.label9.Text = "Provider";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelDx
			// 
			this.labelDx.Location = new System.Drawing.Point(5, 242);
			this.labelDx.Name = "labelDx";
			this.labelDx.Size = new System.Drawing.Size(100, 14);
			this.labelDx.TabIndex = 46;
			this.labelDx.Text = "Diagnosis";
			this.labelDx.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panel1
			// 
			this.panel1.AllowDrop = true;
			this.panel1.Controls.Add(this.textOrigDateComp);
			this.panel1.Controls.Add(this.labelOrigDateComp);
			this.panel1.Controls.Add(this.textTimeFinal);
			this.panel1.Controls.Add(this.textTimeStart);
			this.panel1.Controls.Add(this.textTimeEnd);
			this.panel1.Controls.Add(this.textDate);
			this.panel1.Controls.Add(this.butNow);
			this.panel1.Controls.Add(this.panelSurfaces);
			this.panel1.Controls.Add(this.textDateTP);
			this.panel1.Controls.Add(this.label27);
			this.panel1.Controls.Add(this.listBoxTeeth);
			this.panel1.Controls.Add(this.textDesc);
			this.panel1.Controls.Add(this.textDateEntry);
			this.panel1.Controls.Add(this.label12);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.labelTooth);
			this.panel1.Controls.Add(this.labelSurfaces);
			this.panel1.Controls.Add(this.labelAmount);
			this.panel1.Controls.Add(this.textSurfaces);
			this.panel1.Controls.Add(this.label6);
			this.panel1.Controls.Add(this.groupQuadrant);
			this.panel1.Controls.Add(this.textProcFee);
			this.panel1.Controls.Add(this.textTooth);
			this.panel1.Controls.Add(this.labelStartTime);
			this.panel1.Controls.Add(this.labelEndTime);
			this.panel1.Controls.Add(this.labelRange);
			this.panel1.Controls.Add(this.labelDate);
			this.panel1.Controls.Add(this.textProc);
			this.panel1.Controls.Add(this.listBoxTeeth2);
			this.panel1.Controls.Add(this.textRange);
			this.panel1.Controls.Add(this.butChange);
			this.panel1.Controls.Add(this.groupSextant);
			this.panel1.Controls.Add(this.groupArch);
			this.panel1.Controls.Add(this.labelTimeFinal);
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(397, 177);
			this.panel1.TabIndex = 2;
			// 
			// textOrigDateComp
			// 
			this.textOrigDateComp.Location = new System.Drawing.Point(314, 1);
			this.textOrigDateComp.Name = "textOrigDateComp";
			this.textOrigDateComp.ReadOnly = true;
			this.textOrigDateComp.Size = new System.Drawing.Size(76, 20);
			this.textOrigDateComp.TabIndex = 105;
			// 
			// labelOrigDateComp
			// 
			this.labelOrigDateComp.Location = new System.Drawing.Point(205, 3);
			this.labelOrigDateComp.Name = "labelOrigDateComp";
			this.labelOrigDateComp.Size = new System.Drawing.Size(109, 18);
			this.labelOrigDateComp.TabIndex = 106;
			this.labelOrigDateComp.Text = "Original Date Comp";
			this.labelOrigDateComp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTimeFinal
			// 
			this.textTimeFinal.Location = new System.Drawing.Point(314, 61);
			this.textTimeFinal.Name = "textTimeFinal";
			this.textTimeFinal.Size = new System.Drawing.Size(55, 20);
			this.textTimeFinal.TabIndex = 104;
			this.textTimeFinal.Visible = false;
			// 
			// textTimeStart
			// 
			this.textTimeStart.Location = new System.Drawing.Point(236, 41);
			this.textTimeStart.Name = "textTimeStart";
			this.textTimeStart.Size = new System.Drawing.Size(55, 20);
			this.textTimeStart.TabIndex = 102;
			this.textTimeStart.TextChanged += new System.EventHandler(this.textTimeStart_TextChanged);
			// 
			// textTimeEnd
			// 
			this.textTimeEnd.Location = new System.Drawing.Point(314, 41);
			this.textTimeEnd.Name = "textTimeEnd";
			this.textTimeEnd.Size = new System.Drawing.Size(55, 20);
			this.textTimeEnd.TabIndex = 102;
			this.textTimeEnd.Visible = false;
			this.textTimeEnd.TextChanged += new System.EventHandler(this.textTimeEnd_TextChanged);
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(106, 41);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(76, 20);
			this.textDate.TabIndex = 102;
			// 
			// butNow
			// 
			this.butNow.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNow.Autosize = false;
			this.butNow.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNow.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNow.CornerRadius = 4F;
			this.butNow.Location = new System.Drawing.Point(369, 41);
			this.butNow.Name = "butNow";
			this.butNow.Size = new System.Drawing.Size(27, 20);
			this.butNow.TabIndex = 101;
			this.butNow.Text = "Now";
			this.butNow.UseVisualStyleBackColor = true;
			this.butNow.Visible = false;
			this.butNow.Click += new System.EventHandler(this.butNow_Click);
			// 
			// textDateTP
			// 
			this.textDateTP.Location = new System.Drawing.Point(106, 21);
			this.textDateTP.Name = "textDateTP";
			this.textDateTP.Size = new System.Drawing.Size(76, 20);
			this.textDateTP.TabIndex = 99;
			// 
			// label27
			// 
			this.label27.Location = new System.Drawing.Point(34, 25);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(70, 14);
			this.label27.TabIndex = 98;
			this.label27.Text = "Date TP";
			this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// listBoxTeeth
			// 
			this.listBoxTeeth.AllowDrop = true;
			this.listBoxTeeth.ColumnWidth = 16;
			this.listBoxTeeth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listBoxTeeth.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16"});
			this.listBoxTeeth.Location = new System.Drawing.Point(106, 101);
			this.listBoxTeeth.MultiColumn = true;
			this.listBoxTeeth.Name = "listBoxTeeth";
			this.listBoxTeeth.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.listBoxTeeth.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBoxTeeth.Size = new System.Drawing.Size(272, 17);
			this.listBoxTeeth.TabIndex = 1;
			this.listBoxTeeth.Visible = false;
			this.listBoxTeeth.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxTeeth_MouseDown);
			// 
			// textDateEntry
			// 
			this.textDateEntry.Location = new System.Drawing.Point(106, 1);
			this.textDateEntry.Name = "textDateEntry";
			this.textDateEntry.ReadOnly = true;
			this.textDateEntry.Size = new System.Drawing.Size(76, 20);
			this.textDateEntry.TabIndex = 95;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(3, 3);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(103, 18);
			this.label12.TabIndex = 96;
			this.label12.Text = "Date Entry";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textProcFee
			// 
			this.textProcFee.Location = new System.Drawing.Point(106, 155);
			this.textProcFee.MaxVal = 100000000D;
			this.textProcFee.MinVal = -100000000D;
			this.textProcFee.Name = "textProcFee";
			this.textProcFee.Size = new System.Drawing.Size(68, 20);
			this.textProcFee.TabIndex = 6;
			this.textProcFee.Validating += new System.ComponentModel.CancelEventHandler(this.textProcFee_Validating);
			// 
			// labelStartTime
			// 
			this.labelStartTime.Location = new System.Drawing.Point(181, 44);
			this.labelStartTime.Name = "labelStartTime";
			this.labelStartTime.Size = new System.Drawing.Size(56, 14);
			this.labelStartTime.TabIndex = 0;
			this.labelStartTime.Text = "Time Start";
			this.labelStartTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelEndTime
			// 
			this.labelEndTime.Location = new System.Drawing.Point(259, 44);
			this.labelEndTime.Name = "labelEndTime";
			this.labelEndTime.Size = new System.Drawing.Size(56, 14);
			this.labelEndTime.TabIndex = 0;
			this.labelEndTime.Text = "End";
			this.labelEndTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelEndTime.Visible = false;
			// 
			// listBoxTeeth2
			// 
			this.listBoxTeeth2.ColumnWidth = 16;
			this.listBoxTeeth2.Items.AddRange(new object[] {
            "32",
            "31",
            "30",
            "29",
            "28",
            "27",
            "26",
            "25",
            "24",
            "23",
            "22",
            "21",
            "20",
            "19",
            "18",
            "17"});
			this.listBoxTeeth2.Location = new System.Drawing.Point(106, 115);
			this.listBoxTeeth2.MultiColumn = true;
			this.listBoxTeeth2.Name = "listBoxTeeth2";
			this.listBoxTeeth2.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBoxTeeth2.Size = new System.Drawing.Size(272, 17);
			this.listBoxTeeth2.TabIndex = 2;
			this.listBoxTeeth2.Visible = false;
			this.listBoxTeeth2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxTeeth2_MouseDown);
			// 
			// butChange
			// 
			this.butChange.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChange.Autosize = true;
			this.butChange.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChange.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChange.CornerRadius = 4F;
			this.butChange.Location = new System.Drawing.Point(184, 61);
			this.butChange.Name = "butChange";
			this.butChange.Size = new System.Drawing.Size(74, 20);
			this.butChange.TabIndex = 37;
			this.butChange.Text = "C&hange";
			this.butChange.Click += new System.EventHandler(this.butChange_Click);
			// 
			// labelTimeFinal
			// 
			this.labelTimeFinal.Location = new System.Drawing.Point(259, 65);
			this.labelTimeFinal.Name = "labelTimeFinal";
			this.labelTimeFinal.Size = new System.Drawing.Size(56, 14);
			this.labelTimeFinal.TabIndex = 103;
			this.labelTimeFinal.Text = "Final";
			this.labelTimeFinal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelTimeFinal.Visible = false;
			// 
			// textDrugQty
			// 
			this.textDrugQty.Location = new System.Drawing.Point(123, 149);
			this.textDrugQty.Name = "textDrugQty";
			this.textDrugQty.Size = new System.Drawing.Size(59, 20);
			this.textDrugQty.TabIndex = 174;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(4, 150);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(118, 16);
			this.label10.TabIndex = 173;
			this.label10.Text = "Drug Qty";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(7, 110);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(115, 16);
			this.label5.TabIndex = 170;
			this.label5.Text = "Drug NDC";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDrugNDC
			// 
			this.textDrugNDC.Location = new System.Drawing.Point(123, 108);
			this.textDrugNDC.Name = "textDrugNDC";
			this.textDrugNDC.ReadOnly = true;
			this.textDrugNDC.Size = new System.Drawing.Size(109, 20);
			this.textDrugNDC.TabIndex = 171;
			this.textDrugNDC.Text = "12345678901";
			// 
			// comboDrugUnit
			// 
			this.comboDrugUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDrugUnit.FormattingEnabled = true;
			this.comboDrugUnit.Location = new System.Drawing.Point(123, 128);
			this.comboDrugUnit.Name = "comboDrugUnit";
			this.comboDrugUnit.Size = new System.Drawing.Size(92, 21);
			this.comboDrugUnit.TabIndex = 169;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(7, 130);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(115, 16);
			this.label1.TabIndex = 168;
			this.label1.Text = "Drug Unit";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textRevCode
			// 
			this.textRevCode.Location = new System.Drawing.Point(123, 88);
			this.textRevCode.MaxLength = 48;
			this.textRevCode.Name = "textRevCode";
			this.textRevCode.Size = new System.Drawing.Size(59, 20);
			this.textRevCode.TabIndex = 112;
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(7, 90);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(115, 17);
			this.label22.TabIndex = 111;
			this.label22.Text = "Revenue Code";
			this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textUnitQty
			// 
			this.textUnitQty.Location = new System.Drawing.Point(123, 47);
			this.textUnitQty.MaxLength = 15;
			this.textUnitQty.Name = "textUnitQty";
			this.textUnitQty.Size = new System.Drawing.Size(29, 20);
			this.textUnitQty.TabIndex = 110;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(7, 49);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(115, 17);
			this.label21.TabIndex = 108;
			this.label21.Text = "Unit Quantity";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textCodeMod4
			// 
			this.textCodeMod4.Location = new System.Drawing.Point(210, 27);
			this.textCodeMod4.MaxLength = 2;
			this.textCodeMod4.Name = "textCodeMod4";
			this.textCodeMod4.Size = new System.Drawing.Size(29, 20);
			this.textCodeMod4.TabIndex = 106;
			this.textCodeMod4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// textCodeMod3
			// 
			this.textCodeMod3.Location = new System.Drawing.Point(181, 27);
			this.textCodeMod3.MaxLength = 2;
			this.textCodeMod3.Name = "textCodeMod3";
			this.textCodeMod3.Size = new System.Drawing.Size(29, 20);
			this.textCodeMod3.TabIndex = 105;
			this.textCodeMod3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// textCodeMod2
			// 
			this.textCodeMod2.Location = new System.Drawing.Point(152, 27);
			this.textCodeMod2.MaxLength = 2;
			this.textCodeMod2.Name = "textCodeMod2";
			this.textCodeMod2.Size = new System.Drawing.Size(29, 20);
			this.textCodeMod2.TabIndex = 104;
			this.textCodeMod2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(7, 29);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(115, 17);
			this.label19.TabIndex = 102;
			this.label19.Text = "Mods";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textCodeMod1
			// 
			this.textCodeMod1.Location = new System.Drawing.Point(123, 27);
			this.textCodeMod1.MaxLength = 2;
			this.textCodeMod1.Name = "textCodeMod1";
			this.textCodeMod1.Size = new System.Drawing.Size(29, 20);
			this.textCodeMod1.TabIndex = 103;
			this.textCodeMod1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// checkIsPrincDiag
			// 
			this.checkIsPrincDiag.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsPrincDiag.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsPrincDiag.Location = new System.Drawing.Point(347, 28);
			this.checkIsPrincDiag.Name = "checkIsPrincDiag";
			this.checkIsPrincDiag.Size = new System.Drawing.Size(166, 15);
			this.checkIsPrincDiag.TabIndex = 101;
			this.checkIsPrincDiag.Text = "Princ Diag";
			this.checkIsPrincDiag.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelDiagnosticCode1
			// 
			this.labelDiagnosticCode1.Location = new System.Drawing.Point(336, 66);
			this.labelDiagnosticCode1.Name = "labelDiagnosticCode1";
			this.labelDiagnosticCode1.Size = new System.Drawing.Size(164, 16);
			this.labelDiagnosticCode1.TabIndex = 99;
			this.labelDiagnosticCode1.Text = "ICD-10 Diagnosis Code 1";
			this.labelDiagnosticCode1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDiagnosticCode
			// 
			this.textDiagnosticCode.Location = new System.Drawing.Point(501, 64);
			this.textDiagnosticCode.Name = "textDiagnosticCode";
			this.textDiagnosticCode.Size = new System.Drawing.Size(76, 20);
			this.textDiagnosticCode.TabIndex = 100;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(7, 9);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(115, 16);
			this.label8.TabIndex = 97;
			this.label8.Text = "Medical Code";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textMedicalCode
			// 
			this.textMedicalCode.Location = new System.Drawing.Point(123, 6);
			this.textMedicalCode.Name = "textMedicalCode";
			this.textMedicalCode.Size = new System.Drawing.Size(76, 20);
			this.textMedicalCode.TabIndex = 98;
			// 
			// labelClaim
			// 
			this.labelClaim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelClaim.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelClaim.Location = new System.Drawing.Point(111, 652);
			this.labelClaim.Name = "labelClaim";
			this.labelClaim.Size = new System.Drawing.Size(480, 44);
			this.labelClaim.TabIndex = 50;
			this.labelClaim.Text = "This procedure is attached to a claim, so certain fields should not be edited.  Y" +
    "ou should reprint the claim if any significant changes are made.";
			this.labelClaim.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.labelClaim.Visible = false;
			// 
			// comboPlaceService
			// 
			this.comboPlaceService.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPlaceService.Location = new System.Drawing.Point(119, 98);
			this.comboPlaceService.MaxDropDownItems = 30;
			this.comboPlaceService.Name = "comboPlaceService";
			this.comboPlaceService.Size = new System.Drawing.Size(177, 21);
			this.comboPlaceService.TabIndex = 6;
			// 
			// labelPlaceService
			// 
			this.labelPlaceService.Location = new System.Drawing.Point(4, 101);
			this.labelPlaceService.Name = "labelPlaceService";
			this.labelPlaceService.Size = new System.Drawing.Size(114, 16);
			this.labelPlaceService.TabIndex = 53;
			this.labelPlaceService.Text = "Place of Service";
			this.labelPlaceService.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelPriority
			// 
			this.labelPriority.Location = new System.Drawing.Point(32, 263);
			this.labelPriority.Name = "labelPriority";
			this.labelPriority.Size = new System.Drawing.Size(72, 16);
			this.labelPriority.TabIndex = 56;
			this.labelPriority.Text = "Priority";
			this.labelPriority.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelSetComplete
			// 
			this.labelSetComplete.Location = new System.Drawing.Point(724, 23);
			this.labelSetComplete.Name = "labelSetComplete";
			this.labelSetComplete.Size = new System.Drawing.Size(157, 16);
			this.labelSetComplete.TabIndex = 58;
			this.labelSetComplete.Text = "changes date and adds note.";
			// 
			// checkNoBillIns
			// 
			this.checkNoBillIns.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkNoBillIns.Location = new System.Drawing.Point(142, 12);
			this.checkNoBillIns.Name = "checkNoBillIns";
			this.checkNoBillIns.Size = new System.Drawing.Size(152, 18);
			this.checkNoBillIns.TabIndex = 9;
			this.checkNoBillIns.Text = "Do Not Bill to Ins";
			this.checkNoBillIns.ThreeState = true;
			this.checkNoBillIns.Click += new System.EventHandler(this.checkNoBillIns_Click);
			// 
			// labelIncomplete
			// 
			this.labelIncomplete.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelIncomplete.ForeColor = System.Drawing.Color.DarkRed;
			this.labelIncomplete.Location = new System.Drawing.Point(547, 115);
			this.labelIncomplete.Name = "labelIncomplete";
			this.labelIncomplete.Size = new System.Drawing.Size(155, 18);
			this.labelIncomplete.TabIndex = 73;
			this.labelIncomplete.Text = "Incomplete";
			this.labelIncomplete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(106, 196);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(177, 21);
			this.comboClinic.TabIndex = 74;
			this.comboClinic.SelectedIndexChanged += new System.EventHandler(this.comboClinic_SelectedIndexChanged);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(5, 200);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(99, 16);
			this.labelClinic.TabIndex = 75;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(403, 20);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(99, 16);
			this.label14.TabIndex = 77;
			this.label14.Text = "Procedure Status";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(389, 327);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(110, 41);
			this.label15.TabIndex = 79;
			this.label15.Text = "Signature /\r\nInitials";
			this.label15.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(429, 138);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(73, 16);
			this.label16.TabIndex = 80;
			this.label16.Text = "User";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboPriority
			// 
			this.comboPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPriority.Location = new System.Drawing.Point(106, 259);
			this.comboPriority.MaxDropDownItems = 30;
			this.comboPriority.Name = "comboPriority";
			this.comboPriority.Size = new System.Drawing.Size(177, 21);
			this.comboPriority.TabIndex = 98;
			// 
			// comboDx
			// 
			this.comboDx.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDx.Location = new System.Drawing.Point(106, 238);
			this.comboDx.MaxDropDownItems = 30;
			this.comboDx.Name = "comboDx";
			this.comboDx.Size = new System.Drawing.Size(177, 21);
			this.comboDx.TabIndex = 99;
			// 
			// comboProv
			// 
			this.comboProv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProv.Location = new System.Drawing.Point(106, 217);
			this.comboProv.MaxDropDownItems = 30;
			this.comboProv.Name = "comboProv";
			this.comboProv.Size = new System.Drawing.Size(158, 21);
			this.comboProv.TabIndex = 100;
			this.comboProv.SelectedIndexChanged += new System.EventHandler(this.comboProv_SelectedIndexChanged);
			// 
			// textUser
			// 
			this.textUser.Location = new System.Drawing.Point(504, 137);
			this.textUser.Name = "textUser";
			this.textUser.ReadOnly = true;
			this.textUser.Size = new System.Drawing.Size(116, 20);
			this.textUser.TabIndex = 101;
			// 
			// comboBillingTypeTwo
			// 
			this.comboBillingTypeTwo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBillingTypeTwo.FormattingEnabled = true;
			this.comboBillingTypeTwo.Location = new System.Drawing.Point(119, 33);
			this.comboBillingTypeTwo.MaxDropDownItems = 30;
			this.comboBillingTypeTwo.Name = "comboBillingTypeTwo";
			this.comboBillingTypeTwo.Size = new System.Drawing.Size(198, 21);
			this.comboBillingTypeTwo.TabIndex = 102;
			// 
			// labelBillingTypeTwo
			// 
			this.labelBillingTypeTwo.Location = new System.Drawing.Point(15, 35);
			this.labelBillingTypeTwo.Name = "labelBillingTypeTwo";
			this.labelBillingTypeTwo.Size = new System.Drawing.Size(102, 16);
			this.labelBillingTypeTwo.TabIndex = 103;
			this.labelBillingTypeTwo.Text = "Billing Type 2";
			this.labelBillingTypeTwo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboBillingTypeOne
			// 
			this.comboBillingTypeOne.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBillingTypeOne.FormattingEnabled = true;
			this.comboBillingTypeOne.Location = new System.Drawing.Point(119, 12);
			this.comboBillingTypeOne.MaxDropDownItems = 30;
			this.comboBillingTypeOne.Name = "comboBillingTypeOne";
			this.comboBillingTypeOne.Size = new System.Drawing.Size(198, 21);
			this.comboBillingTypeOne.TabIndex = 104;
			// 
			// labelBillingTypeOne
			// 
			this.labelBillingTypeOne.Location = new System.Drawing.Point(13, 14);
			this.labelBillingTypeOne.Name = "labelBillingTypeOne";
			this.labelBillingTypeOne.Size = new System.Drawing.Size(104, 16);
			this.labelBillingTypeOne.TabIndex = 105;
			this.labelBillingTypeOne.Text = "Billing Type 1";
			this.labelBillingTypeOne.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textSite
			// 
			this.textSite.AcceptsReturn = true;
			this.textSite.Location = new System.Drawing.Point(119, 77);
			this.textSite.Name = "textSite";
			this.textSite.ReadOnly = true;
			this.textSite.Size = new System.Drawing.Size(153, 20);
			this.textSite.TabIndex = 111;
			// 
			// labelSite
			// 
			this.labelSite.Location = new System.Drawing.Point(4, 78);
			this.labelSite.Name = "labelSite";
			this.labelSite.Size = new System.Drawing.Size(114, 17);
			this.labelSite.TabIndex = 110;
			this.labelSite.Text = "Site";
			this.labelSite.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkHideGraphics
			// 
			this.checkHideGraphics.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHideGraphics.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkHideGraphics.Location = new System.Drawing.Point(5, 178);
			this.checkHideGraphics.Name = "checkHideGraphics";
			this.checkHideGraphics.Size = new System.Drawing.Size(114, 18);
			this.checkHideGraphics.TabIndex = 162;
			this.checkHideGraphics.Text = "Hide Graphics";
			this.checkHideGraphics.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(2, 14);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(90, 41);
			this.label3.TabIndex = 0;
			this.label3.Text = "Crown, Bridge, Denture, or RPD";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(5, 61);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(84, 16);
			this.label4.TabIndex = 4;
			this.label4.Text = "Original Date";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// listProsth
			// 
			this.listProsth.Location = new System.Drawing.Point(91, 14);
			this.listProsth.Name = "listProsth";
			this.listProsth.Size = new System.Drawing.Size(163, 43);
			this.listProsth.TabIndex = 0;
			// 
			// groupProsth
			// 
			this.groupProsth.Controls.Add(this.checkIsDateProsthEst);
			this.groupProsth.Controls.Add(this.listProsth);
			this.groupProsth.Controls.Add(this.textDateOriginalProsth);
			this.groupProsth.Controls.Add(this.label4);
			this.groupProsth.Controls.Add(this.label3);
			this.groupProsth.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupProsth.Location = new System.Drawing.Point(15, 283);
			this.groupProsth.Name = "groupProsth";
			this.groupProsth.Size = new System.Drawing.Size(269, 80);
			this.groupProsth.TabIndex = 7;
			this.groupProsth.TabStop = false;
			this.groupProsth.Text = "Prosthesis Replacement";
			// 
			// checkIsDateProsthEst
			// 
			this.checkIsDateProsthEst.Location = new System.Drawing.Point(169, 61);
			this.checkIsDateProsthEst.Name = "checkIsDateProsthEst";
			this.checkIsDateProsthEst.Size = new System.Drawing.Size(96, 16);
			this.checkIsDateProsthEst.TabIndex = 181;
			this.checkIsDateProsthEst.Text = "Is Estimated";
			this.checkIsDateProsthEst.UseVisualStyleBackColor = true;
			// 
			// textDateOriginalProsth
			// 
			this.textDateOriginalProsth.Location = new System.Drawing.Point(91, 58);
			this.textDateOriginalProsth.Name = "textDateOriginalProsth";
			this.textDateOriginalProsth.Size = new System.Drawing.Size(73, 20);
			this.textDateOriginalProsth.TabIndex = 1;
			// 
			// checkTypeCodeA
			// 
			this.checkTypeCodeA.Location = new System.Drawing.Point(10, 16);
			this.checkTypeCodeA.Name = "checkTypeCodeA";
			this.checkTypeCodeA.Size = new System.Drawing.Size(268, 17);
			this.checkTypeCodeA.TabIndex = 0;
			this.checkTypeCodeA.Text = "Not initial placement.  Repair of a prior service.";
			this.checkTypeCodeA.UseVisualStyleBackColor = true;
			// 
			// checkTypeCodeB
			// 
			this.checkTypeCodeB.Location = new System.Drawing.Point(10, 33);
			this.checkTypeCodeB.Name = "checkTypeCodeB";
			this.checkTypeCodeB.Size = new System.Drawing.Size(239, 17);
			this.checkTypeCodeB.TabIndex = 1;
			this.checkTypeCodeB.Text = "Temporary placement or service.";
			this.checkTypeCodeB.UseVisualStyleBackColor = true;
			// 
			// checkTypeCodeC
			// 
			this.checkTypeCodeC.Location = new System.Drawing.Point(10, 50);
			this.checkTypeCodeC.Name = "checkTypeCodeC";
			this.checkTypeCodeC.Size = new System.Drawing.Size(239, 17);
			this.checkTypeCodeC.TabIndex = 2;
			this.checkTypeCodeC.Text = "Correction of TMJ";
			this.checkTypeCodeC.UseVisualStyleBackColor = true;
			// 
			// checkTypeCodeE
			// 
			this.checkTypeCodeE.Location = new System.Drawing.Point(10, 67);
			this.checkTypeCodeE.Name = "checkTypeCodeE";
			this.checkTypeCodeE.Size = new System.Drawing.Size(268, 17);
			this.checkTypeCodeE.TabIndex = 3;
			this.checkTypeCodeE.Text = "Implant, or in conjunction with implants";
			this.checkTypeCodeE.UseVisualStyleBackColor = true;
			// 
			// checkTypeCodeL
			// 
			this.checkTypeCodeL.Location = new System.Drawing.Point(10, 84);
			this.checkTypeCodeL.Name = "checkTypeCodeL";
			this.checkTypeCodeL.Size = new System.Drawing.Size(113, 17);
			this.checkTypeCodeL.TabIndex = 4;
			this.checkTypeCodeL.Text = "Appliance lost";
			this.checkTypeCodeL.UseVisualStyleBackColor = true;
			// 
			// checkTypeCodeX
			// 
			this.checkTypeCodeX.Location = new System.Drawing.Point(10, 118);
			this.checkTypeCodeX.Name = "checkTypeCodeX";
			this.checkTypeCodeX.Size = new System.Drawing.Size(239, 17);
			this.checkTypeCodeX.TabIndex = 5;
			this.checkTypeCodeX.Text = "None of the above are applicable";
			this.checkTypeCodeX.UseVisualStyleBackColor = true;
			// 
			// checkTypeCodeS
			// 
			this.checkTypeCodeS.Location = new System.Drawing.Point(10, 101);
			this.checkTypeCodeS.Name = "checkTypeCodeS";
			this.checkTypeCodeS.Size = new System.Drawing.Size(113, 17);
			this.checkTypeCodeS.TabIndex = 6;
			this.checkTypeCodeS.Text = "Appliance stolen";
			this.checkTypeCodeS.UseVisualStyleBackColor = true;
			// 
			// groupCanadianProcTypeCode
			// 
			this.groupCanadianProcTypeCode.Controls.Add(this.checkTypeCodeS);
			this.groupCanadianProcTypeCode.Controls.Add(this.checkTypeCodeX);
			this.groupCanadianProcTypeCode.Controls.Add(this.checkTypeCodeL);
			this.groupCanadianProcTypeCode.Controls.Add(this.checkTypeCodeE);
			this.groupCanadianProcTypeCode.Controls.Add(this.checkTypeCodeC);
			this.groupCanadianProcTypeCode.Controls.Add(this.checkTypeCodeB);
			this.groupCanadianProcTypeCode.Controls.Add(this.checkTypeCodeA);
			this.groupCanadianProcTypeCode.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupCanadianProcTypeCode.Location = new System.Drawing.Point(18, 16);
			this.groupCanadianProcTypeCode.Name = "groupCanadianProcTypeCode";
			this.groupCanadianProcTypeCode.Size = new System.Drawing.Size(316, 142);
			this.groupCanadianProcTypeCode.TabIndex = 163;
			this.groupCanadianProcTypeCode.TabStop = false;
			this.groupCanadianProcTypeCode.Text = "Procedure Type Code";
			// 
			// labelDPCpost
			// 
			this.labelDPCpost.Location = new System.Drawing.Point(9, 28);
			this.labelDPCpost.Name = "labelDPCpost";
			this.labelDPCpost.Size = new System.Drawing.Size(100, 16);
			this.labelDPCpost.TabIndex = 107;
			this.labelDPCpost.Text = "DPC Post Visit";
			this.labelDPCpost.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboDPCpost
			// 
			this.comboDPCpost.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDPCpost.DropDownWidth = 177;
			this.comboDPCpost.FormattingEnabled = true;
			this.comboDPCpost.Location = new System.Drawing.Point(111, 27);
			this.comboDPCpost.MaxDropDownItems = 30;
			this.comboDPCpost.Name = "comboDPCpost";
			this.comboDPCpost.Size = new System.Drawing.Size(177, 21);
			this.comboDPCpost.TabIndex = 106;
			// 
			// labelScheduleBy
			// 
			this.labelScheduleBy.Location = new System.Drawing.Point(193, 70);
			this.labelScheduleBy.Name = "labelScheduleBy";
			this.labelScheduleBy.Size = new System.Drawing.Size(148, 16);
			this.labelScheduleBy.TabIndex = 105;
			this.labelScheduleBy.Text = "No Schedule by Date";
			this.labelScheduleBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelScheduleBy.Visible = false;
			// 
			// checkIsRepair
			// 
			this.checkIsRepair.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsRepair.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsRepair.Location = new System.Drawing.Point(10, 145);
			this.checkIsRepair.Name = "checkIsRepair";
			this.checkIsRepair.Size = new System.Drawing.Size(114, 18);
			this.checkIsRepair.TabIndex = 104;
			this.checkIsRepair.Text = "Repair";
			this.checkIsRepair.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkIsEffComm
			// 
			this.checkIsEffComm.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsEffComm.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsEffComm.Location = new System.Drawing.Point(10, 128);
			this.checkIsEffComm.Name = "checkIsEffComm";
			this.checkIsEffComm.Size = new System.Drawing.Size(114, 18);
			this.checkIsEffComm.TabIndex = 103;
			this.checkIsEffComm.Text = "Effective Comm";
			this.checkIsEffComm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkIsOnCall
			// 
			this.checkIsOnCall.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsOnCall.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsOnCall.Location = new System.Drawing.Point(10, 111);
			this.checkIsOnCall.Name = "checkIsOnCall";
			this.checkIsOnCall.Size = new System.Drawing.Size(114, 18);
			this.checkIsOnCall.TabIndex = 102;
			this.checkIsOnCall.Text = "On Call";
			this.checkIsOnCall.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboDPC
			// 
			this.comboDPC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDPC.DropDownWidth = 177;
			this.comboDPC.FormattingEnabled = true;
			this.comboDPC.Location = new System.Drawing.Point(111, 6);
			this.comboDPC.MaxDropDownItems = 30;
			this.comboDPC.Name = "comboDPC";
			this.comboDPC.Size = new System.Drawing.Size(177, 21);
			this.comboDPC.TabIndex = 8;
			this.comboDPC.SelectionChangeCommitted += new System.EventHandler(this.comboDPC_SelectionChangeCommitted);
			// 
			// comboStatus
			// 
			this.comboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboStatus.DropDownWidth = 230;
			this.comboStatus.FormattingEnabled = true;
			this.comboStatus.Location = new System.Drawing.Point(111, 48);
			this.comboStatus.MaxDropDownItems = 30;
			this.comboStatus.Name = "comboStatus";
			this.comboStatus.Size = new System.Drawing.Size(230, 21);
			this.comboStatus.TabIndex = 7;
			this.comboStatus.SelectionChangeCommitted += new System.EventHandler(this.comboStatus_SelectionChangeCommitted);
			// 
			// labelStatus
			// 
			this.labelStatus.Location = new System.Drawing.Point(9, 49);
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.Size = new System.Drawing.Size(100, 16);
			this.labelStatus.TabIndex = 3;
			this.labelStatus.Text = "Status";
			this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelDateStop
			// 
			this.labelDateStop.Location = new System.Drawing.Point(11, 90);
			this.labelDateStop.Name = "labelDateStop";
			this.labelDateStop.Size = new System.Drawing.Size(100, 16);
			this.labelDateStop.TabIndex = 2;
			this.labelDateStop.Text = "Date Stop Clock";
			this.labelDateStop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelDateSched
			// 
			this.labelDateSched.Location = new System.Drawing.Point(10, 70);
			this.labelDateSched.Name = "labelDateSched";
			this.labelDateSched.Size = new System.Drawing.Size(100, 16);
			this.labelDateSched.TabIndex = 1;
			this.labelDateSched.Text = "Scheduled By";
			this.labelDateSched.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelDPC
			// 
			this.labelDPC.Location = new System.Drawing.Point(9, 7);
			this.labelDPC.Name = "labelDPC";
			this.labelDPC.Size = new System.Drawing.Size(100, 16);
			this.labelDPC.TabIndex = 0;
			this.labelDPC.Text = "DPC";
			this.labelDPC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboPrognosis
			// 
			this.comboPrognosis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPrognosis.Location = new System.Drawing.Point(119, 55);
			this.comboPrognosis.MaxDropDownItems = 30;
			this.comboPrognosis.Name = "comboPrognosis";
			this.comboPrognosis.Size = new System.Drawing.Size(153, 21);
			this.comboPrognosis.TabIndex = 165;
			// 
			// labelPrognosis
			// 
			this.labelPrognosis.Location = new System.Drawing.Point(3, 57);
			this.labelPrognosis.Name = "labelPrognosis";
			this.labelPrognosis.Size = new System.Drawing.Size(114, 17);
			this.labelPrognosis.TabIndex = 166;
			this.labelPrognosis.Text = "Prognosis";
			this.labelPrognosis.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboProcStatus
			// 
			this.comboProcStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProcStatus.FormattingEnabled = true;
			this.comboProcStatus.Location = new System.Drawing.Point(504, 19);
			this.comboProcStatus.Name = "comboProcStatus";
			this.comboProcStatus.Size = new System.Drawing.Size(133, 21);
			this.comboProcStatus.TabIndex = 167;
			this.comboProcStatus.SelectionChangeCommitted += new System.EventHandler(this.comboProcStatus_SelectionChangeCommitted);
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(418, 80);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(84, 16);
			this.label13.TabIndex = 168;
			this.label13.Text = "Referral";
			this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textReferral
			// 
			this.textReferral.BackColor = System.Drawing.SystemColors.Control;
			this.textReferral.ForeColor = System.Drawing.Color.DarkRed;
			this.textReferral.Location = new System.Drawing.Point(504, 77);
			this.textReferral.Name = "textReferral";
			this.textReferral.ReadOnly = true;
			this.textReferral.Size = new System.Drawing.Size(198, 20);
			this.textReferral.TabIndex = 169;
			this.textReferral.Text = "test";
			// 
			// labelClaimNote
			// 
			this.labelClaimNote.Location = new System.Drawing.Point(0, 364);
			this.labelClaimNote.Name = "labelClaimNote";
			this.labelClaimNote.Size = new System.Drawing.Size(104, 41);
			this.labelClaimNote.TabIndex = 174;
			this.labelClaimNote.Text = "E-claim Note (keep it very short)";
			this.labelClaimNote.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabPageFinancial);
			this.tabControl.Controls.Add(this.tabPageMedical);
			this.tabControl.Controls.Add(this.tabPageMisc);
			this.tabControl.Controls.Add(this.tabPageCanada);
			this.tabControl.Controls.Add(this.tabPageOrion);
			this.tabControl.Location = new System.Drawing.Point(1, 424);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(962, 244);
			this.tabControl.TabIndex = 175;
			// 
			// tabPageFinancial
			// 
			this.tabPageFinancial.Controls.Add(this.butAddExistAdj);
			this.tabPageFinancial.Controls.Add(this.gridPay);
			this.tabPageFinancial.Controls.Add(this.gridAdj);
			this.tabPageFinancial.Controls.Add(this.label20);
			this.tabPageFinancial.Controls.Add(this.textDiscount);
			this.tabPageFinancial.Controls.Add(this.butAddEstimate);
			this.tabPageFinancial.Controls.Add(this.checkNoBillIns);
			this.tabPageFinancial.Controls.Add(this.butAddAdjust);
			this.tabPageFinancial.Controls.Add(this.gridIns);
			this.tabPageFinancial.Location = new System.Drawing.Point(4, 22);
			this.tabPageFinancial.Name = "tabPageFinancial";
			this.tabPageFinancial.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageFinancial.Size = new System.Drawing.Size(954, 218);
			this.tabPageFinancial.TabIndex = 0;
			this.tabPageFinancial.Text = "Financial";
			this.tabPageFinancial.UseVisualStyleBackColor = true;
			// 
			// butAddExistAdj
			// 
			this.butAddExistAdj.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddExistAdj.Autosize = true;
			this.butAddExistAdj.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddExistAdj.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddExistAdj.CornerRadius = 4F;
			this.butAddExistAdj.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddExistAdj.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddExistAdj.Location = new System.Drawing.Point(589, 6);
			this.butAddExistAdj.Name = "butAddExistAdj";
			this.butAddExistAdj.Size = new System.Drawing.Size(126, 24);
			this.butAddExistAdj.TabIndex = 118;
			this.butAddExistAdj.Text = "Link Existing Adj";
			this.butAddExistAdj.Click += new System.EventHandler(this.butAddExistAdj_Click);
			// 
			// gridPay
			// 
			this.gridPay.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridPay.HasAddButton = false;
			this.gridPay.HasDropDowns = false;
			this.gridPay.HasMultilineHeaders = false;
			this.gridPay.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridPay.HeaderHeight = 15;
			this.gridPay.HScrollVisible = false;
			this.gridPay.Location = new System.Drawing.Point(3, 137);
			this.gridPay.Name = "gridPay";
			this.gridPay.ScrollValue = 0;
			this.gridPay.Size = new System.Drawing.Size(449, 72);
			this.gridPay.TabIndex = 117;
			this.gridPay.Title = "Patient Payments";
			this.gridPay.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridPay.TitleHeight = 18;
			this.gridPay.TranslationName = "TableProcPay";
			this.gridPay.WrapText = false;
			this.gridPay.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPay_CellDoubleClick);
			// 
			// gridAdj
			// 
			this.gridAdj.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridAdj.HasAddButton = false;
			this.gridAdj.HasDropDowns = false;
			this.gridAdj.HasMultilineHeaders = false;
			this.gridAdj.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridAdj.HeaderHeight = 15;
			this.gridAdj.HScrollVisible = false;
			this.gridAdj.Location = new System.Drawing.Point(458, 137);
			this.gridAdj.Name = "gridAdj";
			this.gridAdj.ScrollValue = 0;
			this.gridAdj.Size = new System.Drawing.Size(494, 72);
			this.gridAdj.TabIndex = 116;
			this.gridAdj.Title = "Adjustments";
			this.gridAdj.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridAdj.TitleHeight = 18;
			this.gridAdj.TranslationName = "TableProcAdj";
			this.gridAdj.WrapText = false;
			this.gridAdj.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAdj_CellDoubleClick);
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(807, 12);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(75, 16);
			this.label20.TabIndex = 114;
			this.label20.Text = "Discount";
			this.label20.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDiscount
			// 
			this.textDiscount.Location = new System.Drawing.Point(883, 9);
			this.textDiscount.MaxVal = 100000000D;
			this.textDiscount.MinVal = -100000000D;
			this.textDiscount.Name = "textDiscount";
			this.textDiscount.Size = new System.Drawing.Size(68, 20);
			this.textDiscount.TabIndex = 115;
			// 
			// butAddEstimate
			// 
			this.butAddEstimate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddEstimate.Autosize = true;
			this.butAddEstimate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddEstimate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddEstimate.CornerRadius = 4F;
			this.butAddEstimate.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddEstimate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddEstimate.Location = new System.Drawing.Point(3, 6);
			this.butAddEstimate.Name = "butAddEstimate";
			this.butAddEstimate.Size = new System.Drawing.Size(111, 24);
			this.butAddEstimate.TabIndex = 60;
			this.butAddEstimate.Text = "Add Estimate";
			this.butAddEstimate.Click += new System.EventHandler(this.butAddEstimate_Click);
			// 
			// butAddAdjust
			// 
			this.butAddAdjust.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddAdjust.Autosize = true;
			this.butAddAdjust.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddAdjust.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddAdjust.CornerRadius = 4F;
			this.butAddAdjust.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddAdjust.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddAdjust.Location = new System.Drawing.Point(458, 6);
			this.butAddAdjust.Name = "butAddAdjust";
			this.butAddAdjust.Size = new System.Drawing.Size(126, 24);
			this.butAddAdjust.TabIndex = 72;
			this.butAddAdjust.Text = "Add New Adj";
			this.butAddAdjust.Click += new System.EventHandler(this.butAddAdjust_Click);
			// 
			// gridIns
			// 
			this.gridIns.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridIns.HasAddButton = false;
			this.gridIns.HasDropDowns = false;
			this.gridIns.HasMultilineHeaders = false;
			this.gridIns.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridIns.HeaderHeight = 15;
			this.gridIns.HScrollVisible = false;
			this.gridIns.Location = new System.Drawing.Point(3, 32);
			this.gridIns.Name = "gridIns";
			this.gridIns.ScrollValue = 0;
			this.gridIns.Size = new System.Drawing.Size(949, 102);
			this.gridIns.TabIndex = 113;
			this.gridIns.Title = "Insurance Estimates and Payments";
			this.gridIns.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridIns.TitleHeight = 18;
			this.gridIns.TranslationName = "TableProcIns";
			this.gridIns.WrapText = false;
			this.gridIns.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridIns_CellDoubleClick);
			// 
			// tabPageMedical
			// 
			this.tabPageMedical.Controls.Add(this.groupBox1);
			this.tabPageMedical.Controls.Add(this.label11);
			this.tabPageMedical.Controls.Add(this.checkIcdVersion);
			this.tabPageMedical.Controls.Add(this.butNoneDiagnosisCode1);
			this.tabPageMedical.Controls.Add(this.butDiagnosisCode1);
			this.tabPageMedical.Controls.Add(this.butNoneDiagnosisCode2);
			this.tabPageMedical.Controls.Add(this.butDiagnosisCode2);
			this.tabPageMedical.Controls.Add(this.butNoneDiagnosisCode4);
			this.tabPageMedical.Controls.Add(this.butDiagnosisCode4);
			this.tabPageMedical.Controls.Add(this.butNoneDiagnosisCode3);
			this.tabPageMedical.Controls.Add(this.butDiagnosisCode3);
			this.tabPageMedical.Controls.Add(this.textDiagnosticCode2);
			this.tabPageMedical.Controls.Add(this.labelDiagnosticCode2);
			this.tabPageMedical.Controls.Add(this.textDiagnosticCode3);
			this.tabPageMedical.Controls.Add(this.labelDiagnosticCode3);
			this.tabPageMedical.Controls.Add(this.textDiagnosticCode4);
			this.tabPageMedical.Controls.Add(this.labelDiagnosticCode4);
			this.tabPageMedical.Controls.Add(this.butNoneSnomedBodySite);
			this.tabPageMedical.Controls.Add(this.butSnomedBodySiteSelect);
			this.tabPageMedical.Controls.Add(this.labelSnomedCtBodySite);
			this.tabPageMedical.Controls.Add(this.textSnomedBodySite);
			this.tabPageMedical.Controls.Add(this.label17);
			this.tabPageMedical.Controls.Add(this.comboUnitType);
			this.tabPageMedical.Controls.Add(this.textDrugQty);
			this.tabPageMedical.Controls.Add(this.label10);
			this.tabPageMedical.Controls.Add(this.label8);
			this.tabPageMedical.Controls.Add(this.label5);
			this.tabPageMedical.Controls.Add(this.textMedicalCode);
			this.tabPageMedical.Controls.Add(this.textDrugNDC);
			this.tabPageMedical.Controls.Add(this.textDiagnosticCode);
			this.tabPageMedical.Controls.Add(this.comboDrugUnit);
			this.tabPageMedical.Controls.Add(this.labelDiagnosticCode1);
			this.tabPageMedical.Controls.Add(this.label1);
			this.tabPageMedical.Controls.Add(this.checkIsPrincDiag);
			this.tabPageMedical.Controls.Add(this.textRevCode);
			this.tabPageMedical.Controls.Add(this.textCodeMod1);
			this.tabPageMedical.Controls.Add(this.label22);
			this.tabPageMedical.Controls.Add(this.label19);
			this.tabPageMedical.Controls.Add(this.textUnitQty);
			this.tabPageMedical.Controls.Add(this.textCodeMod2);
			this.tabPageMedical.Controls.Add(this.label21);
			this.tabPageMedical.Controls.Add(this.textCodeMod3);
			this.tabPageMedical.Controls.Add(this.textCodeMod4);
			this.tabPageMedical.Location = new System.Drawing.Point(4, 22);
			this.tabPageMedical.Name = "tabPageMedical";
			this.tabPageMedical.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageMedical.Size = new System.Drawing.Size(954, 218);
			this.tabPageMedical.TabIndex = 3;
			this.tabPageMedical.Text = "Medical";
			this.tabPageMedical.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.butPickOrderProvInternal);
			this.groupBox1.Controls.Add(this.textOrderingProviderOverride);
			this.groupBox1.Controls.Add(this.butPickOrderProvReferral);
			this.groupBox1.Controls.Add(this.butNoneOrderProv);
			this.groupBox1.Location = new System.Drawing.Point(501, 151);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(275, 64);
			this.groupBox1.TabIndex = 300;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Ordering Provider Override";
			// 
			// butPickOrderProvInternal
			// 
			this.butPickOrderProvInternal.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickOrderProvInternal.Autosize = true;
			this.butPickOrderProvInternal.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickOrderProvInternal.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickOrderProvInternal.CornerRadius = 4F;
			this.butPickOrderProvInternal.Location = new System.Drawing.Point(6, 14);
			this.butPickOrderProvInternal.Name = "butPickOrderProvInternal";
			this.butPickOrderProvInternal.Size = new System.Drawing.Size(61, 21);
			this.butPickOrderProvInternal.TabIndex = 294;
			this.butPickOrderProvInternal.Text = "Internal";
			this.butPickOrderProvInternal.Click += new System.EventHandler(this.butPickOrderProvInternal_Click);
			// 
			// textOrderingProviderOverride
			// 
			this.textOrderingProviderOverride.AcceptsTab = true;
			this.textOrderingProviderOverride.BackColor = System.Drawing.SystemColors.Control;
			this.textOrderingProviderOverride.DetectLinksEnabled = false;
			this.textOrderingProviderOverride.DetectUrls = false;
			this.textOrderingProviderOverride.Location = new System.Drawing.Point(6, 37);
			this.textOrderingProviderOverride.Name = "textOrderingProviderOverride";
			this.textOrderingProviderOverride.QuickPasteType = OpenDentBusiness.QuickPasteType.ReadOnly;
			this.textOrderingProviderOverride.ReadOnly = true;
			this.textOrderingProviderOverride.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textOrderingProviderOverride.Size = new System.Drawing.Size(266, 21);
			this.textOrderingProviderOverride.SpellCheckIsEnabled = false;
			this.textOrderingProviderOverride.TabIndex = 299;
			this.textOrderingProviderOverride.Text = "";
			// 
			// butPickOrderProvReferral
			// 
			this.butPickOrderProvReferral.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickOrderProvReferral.Autosize = false;
			this.butPickOrderProvReferral.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickOrderProvReferral.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickOrderProvReferral.CornerRadius = 4F;
			this.butPickOrderProvReferral.Location = new System.Drawing.Point(70, 14);
			this.butPickOrderProvReferral.Name = "butPickOrderProvReferral";
			this.butPickOrderProvReferral.Size = new System.Drawing.Size(61, 21);
			this.butPickOrderProvReferral.TabIndex = 297;
			this.butPickOrderProvReferral.Text = "Referral";
			this.butPickOrderProvReferral.Click += new System.EventHandler(this.butPickOrderProvReferral_Click);
			// 
			// butNoneOrderProv
			// 
			this.butNoneOrderProv.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNoneOrderProv.Autosize = false;
			this.butNoneOrderProv.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNoneOrderProv.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNoneOrderProv.CornerRadius = 4F;
			this.butNoneOrderProv.Location = new System.Drawing.Point(134, 14);
			this.butNoneOrderProv.Name = "butNoneOrderProv";
			this.butNoneOrderProv.Size = new System.Drawing.Size(61, 21);
			this.butNoneOrderProv.TabIndex = 298;
			this.butNoneOrderProv.Text = "None";
			this.butNoneOrderProv.Click += new System.EventHandler(this.butNoneOrderProv_Click);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(514, 45);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(115, 15);
			this.label11.TabIndex = 288;
			this.label11.Text = "(uncheck for ICD-9)";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkIcdVersion
			// 
			this.checkIcdVersion.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIcdVersion.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIcdVersion.Location = new System.Drawing.Point(307, 45);
			this.checkIcdVersion.Name = "checkIcdVersion";
			this.checkIcdVersion.Size = new System.Drawing.Size(206, 15);
			this.checkIcdVersion.TabIndex = 287;
			this.checkIcdVersion.Text = "Use ICD-10 Diagnosis Codes";
			this.checkIcdVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIcdVersion.Click += new System.EventHandler(this.checkIcdVersion_Click);
			// 
			// butNoneDiagnosisCode1
			// 
			this.butNoneDiagnosisCode1.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNoneDiagnosisCode1.Autosize = true;
			this.butNoneDiagnosisCode1.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNoneDiagnosisCode1.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNoneDiagnosisCode1.CornerRadius = 4F;
			this.butNoneDiagnosisCode1.Location = new System.Drawing.Point(605, 61);
			this.butNoneDiagnosisCode1.Name = "butNoneDiagnosisCode1";
			this.butNoneDiagnosisCode1.Size = new System.Drawing.Size(51, 22);
			this.butNoneDiagnosisCode1.TabIndex = 194;
			this.butNoneDiagnosisCode1.Text = "None";
			this.butNoneDiagnosisCode1.Click += new System.EventHandler(this.butNoneDiagnosisCode1_Click);
			// 
			// butDiagnosisCode1
			// 
			this.butDiagnosisCode1.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDiagnosisCode1.Autosize = true;
			this.butDiagnosisCode1.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDiagnosisCode1.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDiagnosisCode1.CornerRadius = 4F;
			this.butDiagnosisCode1.Location = new System.Drawing.Point(580, 61);
			this.butDiagnosisCode1.Name = "butDiagnosisCode1";
			this.butDiagnosisCode1.Size = new System.Drawing.Size(22, 22);
			this.butDiagnosisCode1.TabIndex = 193;
			this.butDiagnosisCode1.Text = "...";
			this.butDiagnosisCode1.Click += new System.EventHandler(this.butDiagnosisCode1_Click);
			// 
			// butNoneDiagnosisCode2
			// 
			this.butNoneDiagnosisCode2.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNoneDiagnosisCode2.Autosize = true;
			this.butNoneDiagnosisCode2.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNoneDiagnosisCode2.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNoneDiagnosisCode2.CornerRadius = 4F;
			this.butNoneDiagnosisCode2.Location = new System.Drawing.Point(605, 83);
			this.butNoneDiagnosisCode2.Name = "butNoneDiagnosisCode2";
			this.butNoneDiagnosisCode2.Size = new System.Drawing.Size(51, 22);
			this.butNoneDiagnosisCode2.TabIndex = 192;
			this.butNoneDiagnosisCode2.Text = "None";
			this.butNoneDiagnosisCode2.Click += new System.EventHandler(this.butNoneDiagnosisCode2_Click);
			// 
			// butDiagnosisCode2
			// 
			this.butDiagnosisCode2.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDiagnosisCode2.Autosize = true;
			this.butDiagnosisCode2.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDiagnosisCode2.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDiagnosisCode2.CornerRadius = 4F;
			this.butDiagnosisCode2.Location = new System.Drawing.Point(580, 83);
			this.butDiagnosisCode2.Name = "butDiagnosisCode2";
			this.butDiagnosisCode2.Size = new System.Drawing.Size(22, 22);
			this.butDiagnosisCode2.TabIndex = 191;
			this.butDiagnosisCode2.Text = "...";
			this.butDiagnosisCode2.Click += new System.EventHandler(this.butDiagnosisCode2_Click);
			// 
			// butNoneDiagnosisCode4
			// 
			this.butNoneDiagnosisCode4.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNoneDiagnosisCode4.Autosize = true;
			this.butNoneDiagnosisCode4.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNoneDiagnosisCode4.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNoneDiagnosisCode4.CornerRadius = 4F;
			this.butNoneDiagnosisCode4.Location = new System.Drawing.Point(605, 127);
			this.butNoneDiagnosisCode4.Name = "butNoneDiagnosisCode4";
			this.butNoneDiagnosisCode4.Size = new System.Drawing.Size(51, 22);
			this.butNoneDiagnosisCode4.TabIndex = 190;
			this.butNoneDiagnosisCode4.Text = "None";
			this.butNoneDiagnosisCode4.Click += new System.EventHandler(this.butNoneDiagnosisCode4_Click);
			// 
			// butDiagnosisCode4
			// 
			this.butDiagnosisCode4.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDiagnosisCode4.Autosize = true;
			this.butDiagnosisCode4.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDiagnosisCode4.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDiagnosisCode4.CornerRadius = 4F;
			this.butDiagnosisCode4.Location = new System.Drawing.Point(580, 127);
			this.butDiagnosisCode4.Name = "butDiagnosisCode4";
			this.butDiagnosisCode4.Size = new System.Drawing.Size(22, 22);
			this.butDiagnosisCode4.TabIndex = 189;
			this.butDiagnosisCode4.Text = "...";
			this.butDiagnosisCode4.Click += new System.EventHandler(this.butDiagnosisCode4_Click);
			// 
			// butNoneDiagnosisCode3
			// 
			this.butNoneDiagnosisCode3.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNoneDiagnosisCode3.Autosize = true;
			this.butNoneDiagnosisCode3.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNoneDiagnosisCode3.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNoneDiagnosisCode3.CornerRadius = 4F;
			this.butNoneDiagnosisCode3.Location = new System.Drawing.Point(605, 105);
			this.butNoneDiagnosisCode3.Name = "butNoneDiagnosisCode3";
			this.butNoneDiagnosisCode3.Size = new System.Drawing.Size(51, 22);
			this.butNoneDiagnosisCode3.TabIndex = 188;
			this.butNoneDiagnosisCode3.Text = "None";
			this.butNoneDiagnosisCode3.Click += new System.EventHandler(this.butNoneDiagnosisCode3_Click);
			// 
			// butDiagnosisCode3
			// 
			this.butDiagnosisCode3.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDiagnosisCode3.Autosize = true;
			this.butDiagnosisCode3.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDiagnosisCode3.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDiagnosisCode3.CornerRadius = 4F;
			this.butDiagnosisCode3.Location = new System.Drawing.Point(580, 105);
			this.butDiagnosisCode3.Name = "butDiagnosisCode3";
			this.butDiagnosisCode3.Size = new System.Drawing.Size(22, 22);
			this.butDiagnosisCode3.TabIndex = 187;
			this.butDiagnosisCode3.Text = "...";
			this.butDiagnosisCode3.Click += new System.EventHandler(this.butDiagnosisCode3_Click);
			// 
			// textDiagnosticCode2
			// 
			this.textDiagnosticCode2.Location = new System.Drawing.Point(501, 85);
			this.textDiagnosticCode2.Name = "textDiagnosticCode2";
			this.textDiagnosticCode2.Size = new System.Drawing.Size(76, 20);
			this.textDiagnosticCode2.TabIndex = 186;
			// 
			// labelDiagnosticCode2
			// 
			this.labelDiagnosticCode2.Location = new System.Drawing.Point(336, 87);
			this.labelDiagnosticCode2.Name = "labelDiagnosticCode2";
			this.labelDiagnosticCode2.Size = new System.Drawing.Size(164, 16);
			this.labelDiagnosticCode2.TabIndex = 185;
			this.labelDiagnosticCode2.Text = "ICD-10 Diagnosis Code 2";
			this.labelDiagnosticCode2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDiagnosticCode3
			// 
			this.textDiagnosticCode3.Location = new System.Drawing.Point(501, 106);
			this.textDiagnosticCode3.Name = "textDiagnosticCode3";
			this.textDiagnosticCode3.Size = new System.Drawing.Size(76, 20);
			this.textDiagnosticCode3.TabIndex = 184;
			// 
			// labelDiagnosticCode3
			// 
			this.labelDiagnosticCode3.Location = new System.Drawing.Point(336, 108);
			this.labelDiagnosticCode3.Name = "labelDiagnosticCode3";
			this.labelDiagnosticCode3.Size = new System.Drawing.Size(164, 16);
			this.labelDiagnosticCode3.TabIndex = 183;
			this.labelDiagnosticCode3.Text = "ICD-10 Diagnosis Code 3";
			this.labelDiagnosticCode3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDiagnosticCode4
			// 
			this.textDiagnosticCode4.Location = new System.Drawing.Point(501, 126);
			this.textDiagnosticCode4.Name = "textDiagnosticCode4";
			this.textDiagnosticCode4.Size = new System.Drawing.Size(76, 20);
			this.textDiagnosticCode4.TabIndex = 182;
			// 
			// labelDiagnosticCode4
			// 
			this.labelDiagnosticCode4.Location = new System.Drawing.Point(336, 128);
			this.labelDiagnosticCode4.Name = "labelDiagnosticCode4";
			this.labelDiagnosticCode4.Size = new System.Drawing.Size(164, 16);
			this.labelDiagnosticCode4.TabIndex = 181;
			this.labelDiagnosticCode4.Text = "ICD-10 Diagnosis Code 4";
			this.labelDiagnosticCode4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butNoneSnomedBodySite
			// 
			this.butNoneSnomedBodySite.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNoneSnomedBodySite.Autosize = true;
			this.butNoneSnomedBodySite.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNoneSnomedBodySite.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNoneSnomedBodySite.CornerRadius = 4F;
			this.butNoneSnomedBodySite.Location = new System.Drawing.Point(801, 6);
			this.butNoneSnomedBodySite.Name = "butNoneSnomedBodySite";
			this.butNoneSnomedBodySite.Size = new System.Drawing.Size(51, 22);
			this.butNoneSnomedBodySite.TabIndex = 180;
			this.butNoneSnomedBodySite.Text = "None";
			this.butNoneSnomedBodySite.Click += new System.EventHandler(this.butNoneSnomedBodySite_Click);
			// 
			// butSnomedBodySiteSelect
			// 
			this.butSnomedBodySiteSelect.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSnomedBodySiteSelect.Autosize = true;
			this.butSnomedBodySiteSelect.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSnomedBodySiteSelect.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSnomedBodySiteSelect.CornerRadius = 4F;
			this.butSnomedBodySiteSelect.Location = new System.Drawing.Point(776, 6);
			this.butSnomedBodySiteSelect.Name = "butSnomedBodySiteSelect";
			this.butSnomedBodySiteSelect.Size = new System.Drawing.Size(22, 22);
			this.butSnomedBodySiteSelect.TabIndex = 179;
			this.butSnomedBodySiteSelect.Text = "...";
			this.butSnomedBodySiteSelect.Click += new System.EventHandler(this.butSnomedBodySiteSelect_Click);
			// 
			// labelSnomedCtBodySite
			// 
			this.labelSnomedCtBodySite.Location = new System.Drawing.Point(326, 7);
			this.labelSnomedCtBodySite.Name = "labelSnomedCtBodySite";
			this.labelSnomedCtBodySite.Size = new System.Drawing.Size(172, 20);
			this.labelSnomedCtBodySite.TabIndex = 178;
			this.labelSnomedCtBodySite.Text = "SNOMED CT Body Site";
			this.labelSnomedCtBodySite.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textSnomedBodySite
			// 
			this.textSnomedBodySite.Location = new System.Drawing.Point(501, 7);
			this.textSnomedBodySite.Name = "textSnomedBodySite";
			this.textSnomedBodySite.ReadOnly = true;
			this.textSnomedBodySite.Size = new System.Drawing.Size(272, 20);
			this.textSnomedBodySite.TabIndex = 177;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(7, 69);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(115, 17);
			this.label17.TabIndex = 176;
			this.label17.Text = "Unit Type";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboUnitType
			// 
			this.comboUnitType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboUnitType.FormattingEnabled = true;
			this.comboUnitType.Location = new System.Drawing.Point(123, 67);
			this.comboUnitType.Name = "comboUnitType";
			this.comboUnitType.Size = new System.Drawing.Size(117, 21);
			this.comboUnitType.TabIndex = 175;
			// 
			// tabPageMisc
			// 
			this.tabPageMisc.Controls.Add(this.textBillingNote);
			this.tabPageMisc.Controls.Add(this.label18);
			this.tabPageMisc.Controls.Add(this.comboBillingTypeOne);
			this.tabPageMisc.Controls.Add(this.labelBillingTypeTwo);
			this.tabPageMisc.Controls.Add(this.comboBillingTypeTwo);
			this.tabPageMisc.Controls.Add(this.labelBillingTypeOne);
			this.tabPageMisc.Controls.Add(this.comboPrognosis);
			this.tabPageMisc.Controls.Add(this.labelPrognosis);
			this.tabPageMisc.Controls.Add(this.textSite);
			this.tabPageMisc.Controls.Add(this.labelSite);
			this.tabPageMisc.Controls.Add(this.butPickSite);
			this.tabPageMisc.Controls.Add(this.comboPlaceService);
			this.tabPageMisc.Controls.Add(this.labelPlaceService);
			this.tabPageMisc.Location = new System.Drawing.Point(4, 22);
			this.tabPageMisc.Name = "tabPageMisc";
			this.tabPageMisc.Size = new System.Drawing.Size(954, 218);
			this.tabPageMisc.TabIndex = 4;
			this.tabPageMisc.Text = "Misc";
			this.tabPageMisc.UseVisualStyleBackColor = true;
			// 
			// textBillingNote
			// 
			this.textBillingNote.Location = new System.Drawing.Point(119, 120);
			this.textBillingNote.Multiline = true;
			this.textBillingNote.Name = "textBillingNote";
			this.textBillingNote.Size = new System.Drawing.Size(259, 83);
			this.textBillingNote.TabIndex = 168;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(6, 122);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(111, 14);
			this.label18.TabIndex = 167;
			this.label18.Text = "Billing Note";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butPickSite
			// 
			this.butPickSite.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickSite.Autosize = true;
			this.butPickSite.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickSite.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickSite.CornerRadius = 2F;
			this.butPickSite.Location = new System.Drawing.Point(273, 76);
			this.butPickSite.Name = "butPickSite";
			this.butPickSite.Size = new System.Drawing.Size(19, 21);
			this.butPickSite.TabIndex = 112;
			this.butPickSite.TabStop = false;
			this.butPickSite.Text = "...";
			this.butPickSite.Click += new System.EventHandler(this.butPickSite_Click);
			// 
			// tabPageCanada
			// 
			this.tabPageCanada.Controls.Add(this.labelCanadaLabFee2);
			this.tabPageCanada.Controls.Add(this.labelCanadaLabFee1);
			this.tabPageCanada.Controls.Add(this.groupCanadianProcTypeCode);
			this.tabPageCanada.Controls.Add(this.textCanadaLabFee2);
			this.tabPageCanada.Controls.Add(this.textCanadaLabFee1);
			this.tabPageCanada.Location = new System.Drawing.Point(4, 22);
			this.tabPageCanada.Name = "tabPageCanada";
			this.tabPageCanada.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageCanada.Size = new System.Drawing.Size(954, 218);
			this.tabPageCanada.TabIndex = 1;
			this.tabPageCanada.Text = "Canada";
			this.tabPageCanada.UseVisualStyleBackColor = true;
			// 
			// labelCanadaLabFee2
			// 
			this.labelCanadaLabFee2.Location = new System.Drawing.Point(340, 37);
			this.labelCanadaLabFee2.Name = "labelCanadaLabFee2";
			this.labelCanadaLabFee2.Size = new System.Drawing.Size(75, 20);
			this.labelCanadaLabFee2.TabIndex = 167;
			this.labelCanadaLabFee2.Text = "Lab Fee 2";
			this.labelCanadaLabFee2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelCanadaLabFee1
			// 
			this.labelCanadaLabFee1.Location = new System.Drawing.Point(340, 16);
			this.labelCanadaLabFee1.Name = "labelCanadaLabFee1";
			this.labelCanadaLabFee1.Size = new System.Drawing.Size(75, 20);
			this.labelCanadaLabFee1.TabIndex = 166;
			this.labelCanadaLabFee1.Text = "Lab Fee 1";
			this.labelCanadaLabFee1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textCanadaLabFee2
			// 
			this.textCanadaLabFee2.Location = new System.Drawing.Point(421, 37);
			this.textCanadaLabFee2.MaxVal = 100000000D;
			this.textCanadaLabFee2.MinVal = -100000000D;
			this.textCanadaLabFee2.Name = "textCanadaLabFee2";
			this.textCanadaLabFee2.Size = new System.Drawing.Size(68, 20);
			this.textCanadaLabFee2.TabIndex = 165;
			// 
			// textCanadaLabFee1
			// 
			this.textCanadaLabFee1.Location = new System.Drawing.Point(421, 16);
			this.textCanadaLabFee1.MaxVal = 100000000D;
			this.textCanadaLabFee1.MinVal = -100000000D;
			this.textCanadaLabFee1.Name = "textCanadaLabFee1";
			this.textCanadaLabFee1.Size = new System.Drawing.Size(68, 20);
			this.textCanadaLabFee1.TabIndex = 164;
			// 
			// tabPageOrion
			// 
			this.tabPageOrion.Controls.Add(this.labelDPCpost);
			this.tabPageOrion.Controls.Add(this.comboDPCpost);
			this.tabPageOrion.Controls.Add(this.labelDPC);
			this.tabPageOrion.Controls.Add(this.labelScheduleBy);
			this.tabPageOrion.Controls.Add(this.labelDateSched);
			this.tabPageOrion.Controls.Add(this.checkIsRepair);
			this.tabPageOrion.Controls.Add(this.labelDateStop);
			this.tabPageOrion.Controls.Add(this.checkIsEffComm);
			this.tabPageOrion.Controls.Add(this.labelStatus);
			this.tabPageOrion.Controls.Add(this.checkIsOnCall);
			this.tabPageOrion.Controls.Add(this.comboStatus);
			this.tabPageOrion.Controls.Add(this.comboDPC);
			this.tabPageOrion.Controls.Add(this.textDateStop);
			this.tabPageOrion.Controls.Add(this.textDateScheduled);
			this.tabPageOrion.Location = new System.Drawing.Point(4, 22);
			this.tabPageOrion.Name = "tabPageOrion";
			this.tabPageOrion.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageOrion.Size = new System.Drawing.Size(954, 218);
			this.tabPageOrion.TabIndex = 2;
			this.tabPageOrion.Text = "Orion";
			this.tabPageOrion.UseVisualStyleBackColor = true;
			// 
			// textDateStop
			// 
			this.textDateStop.Location = new System.Drawing.Point(111, 89);
			this.textDateStop.Name = "textDateStop";
			this.textDateStop.Size = new System.Drawing.Size(76, 20);
			this.textDateStop.TabIndex = 10;
			// 
			// textDateScheduled
			// 
			this.textDateScheduled.Location = new System.Drawing.Point(111, 69);
			this.textDateScheduled.Name = "textDateScheduled";
			this.textDateScheduled.ReadOnly = true;
			this.textDateScheduled.Size = new System.Drawing.Size(76, 20);
			this.textDateScheduled.TabIndex = 9;
			// 
			// labelLocked
			// 
			this.labelLocked.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelLocked.ForeColor = System.Drawing.Color.DarkRed;
			this.labelLocked.Location = new System.Drawing.Point(834, 115);
			this.labelLocked.Name = "labelLocked";
			this.labelLocked.Size = new System.Drawing.Size(123, 18);
			this.labelLocked.TabIndex = 176;
			this.labelLocked.Text = "Locked";
			this.labelLocked.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			this.labelLocked.Visible = false;
			// 
			// butSearch
			// 
			this.butSearch.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSearch.Autosize = true;
			this.butSearch.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSearch.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSearch.CornerRadius = 4F;
			this.butSearch.Location = new System.Drawing.Point(443, 232);
			this.butSearch.Name = "butSearch";
			this.butSearch.Size = new System.Drawing.Size(59, 24);
			this.butSearch.TabIndex = 180;
			this.butSearch.Text = "Search";
			this.butSearch.Click += new System.EventHandler(this.butSearch_Click);
			// 
			// butLock
			// 
			this.butLock.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butLock.Autosize = true;
			this.butLock.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLock.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLock.CornerRadius = 4F;
			this.butLock.Location = new System.Drawing.Point(874, 91);
			this.butLock.Name = "butLock";
			this.butLock.Size = new System.Drawing.Size(80, 22);
			this.butLock.TabIndex = 178;
			this.butLock.Text = "Lock";
			this.butLock.Click += new System.EventHandler(this.butLock_Click);
			// 
			// butInvalidate
			// 
			this.butInvalidate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butInvalidate.Autosize = true;
			this.butInvalidate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butInvalidate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butInvalidate.CornerRadius = 4F;
			this.butInvalidate.Location = new System.Drawing.Point(879, 77);
			this.butInvalidate.Name = "butInvalidate";
			this.butInvalidate.Size = new System.Drawing.Size(80, 22);
			this.butInvalidate.TabIndex = 179;
			this.butInvalidate.Text = "Invalidate";
			this.butInvalidate.Visible = false;
			this.butInvalidate.Click += new System.EventHandler(this.butInvalidate_Click);
			// 
			// butAppend
			// 
			this.butAppend.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAppend.Autosize = true;
			this.butAppend.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAppend.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAppend.CornerRadius = 4F;
			this.butAppend.Location = new System.Drawing.Point(874, 136);
			this.butAppend.Name = "butAppend";
			this.butAppend.Size = new System.Drawing.Size(80, 22);
			this.butAppend.TabIndex = 177;
			this.butAppend.Text = "Append";
			this.butAppend.Visible = false;
			this.butAppend.Click += new System.EventHandler(this.butAppend_Click);
			// 
			// textClaimNote
			// 
			this.textClaimNote.AcceptsTab = true;
			this.textClaimNote.BackColor = System.Drawing.SystemColors.Window;
			this.textClaimNote.DetectLinksEnabled = false;
			this.textClaimNote.DetectUrls = false;
			this.textClaimNote.Location = new System.Drawing.Point(106, 364);
			this.textClaimNote.MaxLength = 80;
			this.textClaimNote.Name = "textClaimNote";
			this.textClaimNote.QuickPasteType = OpenDentBusiness.QuickPasteType.Procedure;
			this.textClaimNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textClaimNote.Size = new System.Drawing.Size(277, 43);
			this.textClaimNote.TabIndex = 173;
			this.textClaimNote.Text = "";
			// 
			// butReferral
			// 
			this.butReferral.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butReferral.Autosize = false;
			this.butReferral.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butReferral.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butReferral.CornerRadius = 2F;
			this.butReferral.Location = new System.Drawing.Point(707, 77);
			this.butReferral.Name = "butReferral";
			this.butReferral.Size = new System.Drawing.Size(18, 21);
			this.butReferral.TabIndex = 170;
			this.butReferral.Text = "...";
			this.butReferral.Click += new System.EventHandler(this.butReferral_Click);
			// 
			// butPickProv
			// 
			this.butPickProv.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickProv.Autosize = false;
			this.butPickProv.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickProv.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickProv.CornerRadius = 2F;
			this.butPickProv.Location = new System.Drawing.Point(265, 217);
			this.butPickProv.Name = "butPickProv";
			this.butPickProv.Size = new System.Drawing.Size(18, 21);
			this.butPickProv.TabIndex = 161;
			this.butPickProv.Text = "...";
			this.butPickProv.Click += new System.EventHandler(this.butPickProv_Click);
			// 
			// buttonUseAutoNote
			// 
			this.buttonUseAutoNote.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.buttonUseAutoNote.Autosize = true;
			this.buttonUseAutoNote.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.buttonUseAutoNote.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.buttonUseAutoNote.CornerRadius = 4F;
			this.buttonUseAutoNote.Location = new System.Drawing.Point(663, 135);
			this.buttonUseAutoNote.Name = "buttonUseAutoNote";
			this.buttonUseAutoNote.Size = new System.Drawing.Size(80, 22);
			this.buttonUseAutoNote.TabIndex = 106;
			this.buttonUseAutoNote.Text = "Auto Note";
			this.buttonUseAutoNote.Click += new System.EventHandler(this.buttonUseAutoNote_Click);
			// 
			// textNotes
			// 
			this.textNotes.AcceptsTab = true;
			this.textNotes.BackColor = System.Drawing.SystemColors.Window;
			this.textNotes.DetectLinksEnabled = false;
			this.textNotes.DetectUrls = false;
			this.textNotes.Location = new System.Drawing.Point(504, 157);
			this.textNotes.Name = "textNotes";
			this.textNotes.QuickPasteType = OpenDentBusiness.QuickPasteType.Procedure;
			this.textNotes.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textNotes.Size = new System.Drawing.Size(450, 164);
			this.textNotes.TabIndex = 1;
			this.textNotes.Text = "";
			this.textNotes.TextChanged += new System.EventHandler(this.textNotes_TextChanged);
			// 
			// butSetComplete
			// 
			this.butSetComplete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSetComplete.Autosize = true;
			this.butSetComplete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSetComplete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSetComplete.CornerRadius = 4F;
			this.butSetComplete.Location = new System.Drawing.Point(643, 19);
			this.butSetComplete.Name = "butSetComplete";
			this.butSetComplete.Size = new System.Drawing.Size(79, 22);
			this.butSetComplete.TabIndex = 54;
			this.butSetComplete.Text = "Set Complete";
			this.butSetComplete.Click += new System.EventHandler(this.butSetComplete_Click);
			// 
			// butEditAnyway
			// 
			this.butEditAnyway.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEditAnyway.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butEditAnyway.Autosize = true;
			this.butEditAnyway.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEditAnyway.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEditAnyway.CornerRadius = 4F;
			this.butEditAnyway.Location = new System.Drawing.Point(594, 671);
			this.butEditAnyway.Name = "butEditAnyway";
			this.butEditAnyway.Size = new System.Drawing.Size(104, 24);
			this.butEditAnyway.TabIndex = 51;
			this.butEditAnyway.Text = "&Edit Anyway";
			this.butEditAnyway.Visible = false;
			this.butEditAnyway.Click += new System.EventHandler(this.butEditAnyway_Click);
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
			this.butDelete.Location = new System.Drawing.Point(2, 671);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(83, 24);
			this.butDelete.TabIndex = 8;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(870, 671);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(76, 24);
			this.butCancel.TabIndex = 13;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(779, 671);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(76, 24);
			this.butOK.TabIndex = 12;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// signatureBoxWrapper
			// 
			this.signatureBoxWrapper.BackColor = System.Drawing.SystemColors.ControlDark;
			this.signatureBoxWrapper.Location = new System.Drawing.Point(505, 325);
			this.signatureBoxWrapper.Name = "signatureBoxWrapper";
			this.signatureBoxWrapper.SignatureMode = OpenDental.UI.SignatureBoxWrapper.SigMode.Default;
			this.signatureBoxWrapper.Size = new System.Drawing.Size(364, 81);
			this.signatureBoxWrapper.TabIndex = 181;
			this.signatureBoxWrapper.UserSig = null;
			this.signatureBoxWrapper.SignatureChanged += new System.EventHandler(this.signatureBoxWrapper_SignatureChanged);
			// 
			// butChangeUser
			// 
			this.butChangeUser.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butChangeUser.Autosize = true;
			this.butChangeUser.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butChangeUser.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butChangeUser.CornerRadius = 4F;
			this.butChangeUser.Location = new System.Drawing.Point(622, 135);
			this.butChangeUser.Name = "butChangeUser";
			this.butChangeUser.Size = new System.Drawing.Size(23, 22);
			this.butChangeUser.TabIndex = 182;
			this.butChangeUser.Text = "...";
			this.butChangeUser.Click += new System.EventHandler(this.butChangeUser_Click);
			// 
			// labelPermAlert
			// 
			this.labelPermAlert.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelPermAlert.ForeColor = System.Drawing.Color.DarkRed;
			this.labelPermAlert.Location = new System.Drawing.Point(504, 409);
			this.labelPermAlert.Name = "labelPermAlert";
			this.labelPermAlert.Size = new System.Drawing.Size(450, 27);
			this.labelPermAlert.TabIndex = 210;
			this.labelPermAlert.Text = "Notes and Signature locked.  \r\nNeed either ProcedureNoteFull or ProcedureNoteUser" +
    " to edit.";
			this.labelPermAlert.Visible = false;
			// 
			// butEditAutoNote
			// 
			this.butEditAutoNote.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butEditAutoNote.Autosize = true;
			this.butEditAutoNote.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEditAutoNote.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEditAutoNote.CornerRadius = 4F;
			this.butEditAutoNote.Location = new System.Drawing.Point(749,135);
			this.butEditAutoNote.Name = "butEditAutoNote";
			this.butEditAutoNote.Size = new System.Drawing.Size(93,22);
			this.butEditAutoNote.TabIndex = 211;
			this.butEditAutoNote.Text = "Edit Auto Note";
			this.butEditAutoNote.Click += new System.EventHandler(this.butEditAutoNote_Click);
			// 
			// FormProcEdit
			// 
			this.ClientSize = new System.Drawing.Size(962, 696);
			this.Controls.Add(this.butEditAutoNote);
			this.Controls.Add(this.labelPermAlert);
			this.Controls.Add(this.butChangeUser);
			this.Controls.Add(this.signatureBoxWrapper);
			this.Controls.Add(this.butSearch);
			this.Controls.Add(this.butLock);
			this.Controls.Add(this.butInvalidate);
			this.Controls.Add(this.butAppend);
			this.Controls.Add(this.labelLocked);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.labelClaimNote);
			this.Controls.Add(this.textClaimNote);
			this.Controls.Add(this.butReferral);
			this.Controls.Add(this.textReferral);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.comboProcStatus);
			this.Controls.Add(this.comboProv);
			this.Controls.Add(this.checkHideGraphics);
			this.Controls.Add(this.butPickProv);
			this.Controls.Add(this.buttonUseAutoNote);
			this.Controls.Add(this.textUser);
			this.Controls.Add(this.comboDx);
			this.Controls.Add(this.comboPriority);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.labelPriority);
			this.Controls.Add(this.labelDx);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.groupProsth);
			this.Controls.Add(this.textNotes);
			this.Controls.Add(this.label16);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.labelIncomplete);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.butSetComplete);
			this.Controls.Add(this.butEditAnyway);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.labelSetComplete);
			this.Controls.Add(this.labelClaim);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormProcEdit";
			this.ShowInTaskbar = false;
			this.Text = "Procedure Info";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormProcEdit_FormClosing);
			this.Load += new System.EventHandler(this.FormProcInfo_Load);
			this.Shown += new System.EventHandler(this.FormProcEdit_Shown);
			this.groupQuadrant.ResumeLayout(false);
			this.groupArch.ResumeLayout(false);
			this.panelSurfaces.ResumeLayout(false);
			this.groupSextant.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.groupProsth.ResumeLayout(false);
			this.groupProsth.PerformLayout();
			this.groupCanadianProcTypeCode.ResumeLayout(false);
			this.tabControl.ResumeLayout(false);
			this.tabPageFinancial.ResumeLayout(false);
			this.tabPageFinancial.PerformLayout();
			this.tabPageMedical.ResumeLayout(false);
			this.tabPageMedical.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.tabPageMisc.ResumeLayout(false);
			this.tabPageMisc.PerformLayout();
			this.tabPageCanada.ResumeLayout(false);
			this.tabPageCanada.PerformLayout();
			this.tabPageOrion.ResumeLayout(false);
			this.tabPageOrion.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormProcInfo_Load(object sender,System.EventArgs e) {
			_loadData=ProcEdit.GetLoadData(ProcCur,PatCur,FamCur);
			SubList=_loadData.ListInsSubs;
			PlanList=_loadData.ListInsPlans;
			signatureBoxWrapper.SetAllowDigitalSig(true);
			ClaimProcsForProc=new List<ClaimProc>();
			//Set the title bar to show the patient's name much like the main screen does.
			this.Text+=" - "+PatCur.GetNameLF();
			textDateEntry.Text=ProcCur.DateEntryC.ToShortDateString();
			if(PrefC.GetBool(PrefName.EasyHidePublicHealth)){
				labelPlaceService.Visible=false;
				comboPlaceService.Visible=false;
				labelSite.Visible=false;
				textSite.Visible=false;
				butPickSite.Visible=false;
			}
			if(PrefC.GetLong(PrefName.UseInternationalToothNumbers)==1){
				listBoxTeeth.Items.Clear();
				listBoxTeeth.Items.AddRange(new string[] {"18","17","16","15","14","13","12","11","21","22","23","24","25","26","27","28"});
				listBoxTeeth2.Items.Clear();
				listBoxTeeth2.Items.AddRange(new string[] {"48","47","46","45","44","43","42","41","31","32","33","34","35","36","37","38"});
			}
			if(PrefC.GetLong(PrefName.UseInternationalToothNumbers)==3){
				listBoxTeeth.Items.Clear();
				listBoxTeeth.Items.AddRange(new string[] {"8","7","6","5","4","3","2","1","1","2","3","4","5","6","7","8"});
				listBoxTeeth2.Items.Clear();
				listBoxTeeth2.Items.AddRange(new string[] {"8","7","6","5","4","3","2","1","1","2","3","4","5","6","7","8"});
			}
			if(!Security.IsAuthorized(Permissions.ProcEditShowFee,true)){
				labelAmount.Visible=false;
				textProcFee.Visible=false;
			}
			ClaimList=_loadData.ListClaims;
			ProcedureCode2=ProcedureCodes.GetProcCode(ProcCur.CodeNum);
			if(ProcCur.ProcStatus==ProcStat.C && PrefC.GetBool(PrefName.ProcLockingIsAllowed) && !ProcCur.IsLocked) {
				butLock.Visible=true;
			}
			else {
				butLock.Visible=false;
			}
			if(IsNew){
				if(ProcCur.ProcStatus==ProcStat.C){
					if(!_isQuickAdd && !Security.IsAuthorized(Permissions.ProcComplCreate)){
						DialogResult=DialogResult.Cancel;
						return;
					}
				}
				//SetControls();
				//return;
			}
			else{
				if(ProcCur.ProcStatus==ProcStat.C){
					textDiscount.Enabled=false;
					if(ProcCur.IsLocked) {//Whether locking is currently allowed, this proc may have been locked previously.
						butOK.Enabled=false;//use this state to cascade permission to any form opened from here
						butDelete.Enabled=false;
						butChange.Enabled=false;
						butEditAnyway.Enabled=false;
						butSetComplete.Enabled=false;
						butSnomedBodySiteSelect.Enabled=false;
						butNoneSnomedBodySite.Enabled=false;
						labelLocked.Visible=true;
						butAppend.Visible=true;
						textNotes.ReadOnly=true;//just for visual cue.  No way to save changes, anyway.
						textNotes.BackColor=SystemColors.Control;
						butInvalidate.Visible=true;
						butInvalidate.Location=butLock.Location;
					}
					else{
						butInvalidate.Visible=false;
					}
				}
			}
			//ClaimProcList=ClaimProcs.Refresh(PatCur.PatNum);
			ClaimProcsForProc=_loadData.ListClaimProcsForProc;
			PatPlanList=_loadData.ListPatPlans;
			BenefitList=_loadData.ListBenefits;
			if(Procedures.IsAttachedToClaim(ProcCur,ClaimProcsForProc)){
				StartedAttachedToClaim=true;
				//however, this doesn't stop someone from creating a claim while this window is open,
				//so this is checked at the end, too.
				panel1.Enabled=false;
				comboProcStatus.Enabled=false;
				checkNoBillIns.Enabled=false;
				butChange.Enabled=false;
				butEditAnyway.Visible=true;
				butSetComplete.Enabled=false;
			}
			if(Procedures.IsAttachedToClaim(ProcCur,ClaimProcsForProc,false)) {
				butDelete.Enabled=false;
				labelClaim.Visible=true;
				butAddEstimate.Enabled=false;
			}
			if(PrefC.GetBool(PrefName.EasyHideClinical)){
				labelDx.Visible=false;
				comboDx.Visible=false;
				labelPrognosis.Visible=false;
				comboPrognosis.Visible=false;
			}
			if(PrefC.GetBool(PrefName.EasyHideMedicaid)) {
				comboBillingTypeOne.Visible=false;
				labelBillingTypeOne.Visible=false;
				comboBillingTypeTwo.Visible=false;
				labelBillingTypeTwo.Visible=false;
			}
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				//groupCanadianProcType.Location=new Point(106,301);
				groupProsth.Visible=false;
				labelClaimNote.Visible=false;
				textClaimNote.Visible=false;
				butBF.Text=Lan.g(this,"B/V");//vestibular instead of facial
				butV.Text=Lan.g(this,"5");
				if(ProcedureCode2.IsCanadianLab) { //Prevent lab fees from having lab fees attached.
					labelCanadaLabFee1.Visible=false;
					textCanadaLabFee1.Visible=false;
					labelCanadaLabFee2.Visible=false;
					textCanadaLabFee2.Visible=false;
				}
				else {
					canadaLabFees=Procedures.GetCanadianLabFees(ProcCur.ProcNum);
					if(canadaLabFees.Count>0) {
						textCanadaLabFee1.Text=canadaLabFees[0].ProcFee.ToString("n");
						if(canadaLabFees[0].ProcStatus==ProcStat.C) {
							textCanadaLabFee1.ReadOnly=true;
						}
					}
					if(canadaLabFees.Count>1) {
						textCanadaLabFee2.Text=canadaLabFees[1].ProcFee.ToString("n");
						if(canadaLabFees[1].ProcStatus==ProcStat.C) {
							textCanadaLabFee2.ReadOnly=true;
						}
					}
				}
			}
			else {
				tabControl.Controls.Remove(tabPageCanada);
				//groupCanadianProcType.Visible=false;
			}
			if(Programs.UsingOrion) {
				if(IsNew) {
					OrionProcCur=new OrionProc();
					OrionProcCur.ProcNum=ProcCur.ProcNum;
					if(ProcCur.ProcStatus==ProcStat.EO) {
						OrionProcCur.Status2=OrionStatus.E;
					}
					else {
						OrionProcCur.Status2=OrionStatus.TP;
					}
				}
				else {
					OrionProcCur=OrionProcs.GetOneByProcNum(ProcCur.ProcNum);
					if(ProcCur.DateTP<MiscData.GetNowDateTime().Date && 
						(OrionProcCur.Status2==OrionStatus.CA_EPRD
						|| OrionProcCur.Status2==OrionStatus.CA_PD
						|| OrionProcCur.Status2==OrionStatus.CA_Tx
						|| OrionProcCur.Status2==OrionStatus.R)) {//Not allowed to edit procedures with these statuses that are older than a day.
						MsgBox.Show(this,"You cannot edit refused or cancelled procedures.");
						DialogResult=DialogResult.Cancel;
					}
					if(OrionProcCur.Status2==OrionStatus.C || OrionProcCur.Status2==OrionStatus.CR || OrionProcCur.Status2==OrionStatus.CS) {
						textNotes.Enabled=false;
					}
				}
				textDateTP.ReadOnly=true;
				//panelOrion.Visible=true;
				butAddEstimate.Visible=false;
				checkNoBillIns.Visible=false;
				gridIns.Visible=false;
				butAddAdjust.Visible=false;
				gridPay.Visible=false;
				gridAdj.Visible=false;
				comboProcStatus.Enabled=false;
				labelAmount.Visible=false;
				textProcFee.Visible=false;
				labelPriority.Visible=false;
				comboPriority.Visible=false;
				butSetComplete.Visible=false;
				labelSetComplete.Visible=false;
			}
			else {
				tabControl.Controls.Remove(tabPageOrion);
			}
			if(Programs.UsingOrion || PrefC.GetBool(PrefName.ShowFeatureMedicalInsurance)) {
				labelEndTime.Visible=true;
				textTimeEnd.Visible=true;
				butNow.Visible=true;
				labelTimeFinal.Visible=true;
				textTimeFinal.Visible=true;
			}
			if(PrefC.GetBool(PrefName.ShowFeatureEhr)) {
				textNotes.HideSelection=false;//When text is selected programmatically using our Search function, this causes the selection to be visible to the users.
			}
			else {
				butSearch.Visible=false;
				labelSnomedCtBodySite.Visible=false;
				textSnomedBodySite.Visible=false;
				butSnomedBodySiteSelect.Visible=false;
				butNoneSnomedBodySite.Visible=false;
			}
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				radioS1.Text="03";//Sextant 1 in the United States is sextant 03 in Canada.
				radioS2.Text="04";//Sextant 2 in the United States is sextant 04 in Canada.
				radioS3.Text="05";//Sextant 3 in the United States is sextant 05 in Canada.
				radioS4.Text="06";//Sextant 4 in the United States is sextant 06 in Canada.
				radioS5.Text="07";//Sextant 5 in the United States is sextant 07 in Canada.
				radioS6.Text="08";//Sextant 6 in the United States is sextant 08 in Canada.
			}
			//Set Selected Nums
			_selectedClinicNum=ProcCur.ClinicNum;
			_selectedProvNum=ProcCur.ProvNum;
			SetOrderingProvider(null);//Clears both the internal ordering and referral ordering providers.
			if(ProcCur.ProvOrderOverride!=0) {
				SetOrderingProvider(Providers.GetProv(ProcCur.ProvOrderOverride));
			}
			else if(ProcCur.OrderingReferralNum!=0) {
				Referral referral;
				Referrals.TryGetReferral(ProcCur.OrderingReferralNum,out referral);
				SetOrderingReferral(referral);
			}
			//Prep Selected Indexs
			comboClinic.SelectedIndex=-1;
			comboProv.SelectedIndex=-1;
			//Fill comboClinic, which subsequently fills comboProv and comboOverride
			FillClinicCombo();
			IsStartingUp=true;
			FillControlsOnStartup();
			SetControlsUpperLeft();
			SetControlsEnabled(_isQuickAdd);
			FillReferral(false);
			FillIns(false);
			FillPayments(false);
			FillAdj();
			IsStartingUp=false;
			bool canEditNote=false;
			if(Security.IsAuthorized(Permissions.ProcedureNoteFull,true)) {
				canEditNote=true;
			}
			else if(Security.IsAuthorized(Permissions.ProcedureNoteUser,true) && (ProcCur.UserNum==Security.CurUser.UserNum || signatureBoxWrapper.SigIsBlank)) {
				canEditNote=true;//They have limited permission and this is their note that they signed.
			}
			if(!canEditNote) {
				textNotes.ReadOnly=true;
				buttonUseAutoNote.Enabled=false;
				butEditAutoNote.Enabled=false;
				signatureBoxWrapper.Enabled=false;
				labelPermAlert.Visible=true;
				butAppend.Enabled=false;//don't allow appending notes either.
				butChangeUser.Enabled=false;
			}
			bool hasAutoNotePrompt=Regex.IsMatch(textNotes.Text,_autoNotePromptRegex);
			butEditAutoNote.Visible=hasAutoNotePrompt;
			//string retVal=ProcCur.Note+ProcCur.UserNum.ToString();
			//MsgBoxCopyPaste msgb=new MsgBoxCopyPaste(retVal);
			//msgb.ShowDialog();
			if(_isQuickAdd) {
				textDate.Enabled=false;
				ProcNoteUiHelper();//Add any default notes.
				butOK_Click(this,new EventArgs());
				if(this.DialogResult!=DialogResult.OK) {
					this.WindowState=FormWindowState.Normal;
					this.CenterToScreen();
					this.BringToFront();
				}
			}
		}

		private void FillClinicCombo() {
			int idxOld = comboClinic.SelectedIndex;
			if(comboClinic.SelectedIndex>-1) {
				_selectedClinicNum=_listClinics[comboClinic.SelectedIndex].ClinicNum;
			}
			_listClinics=new List<Clinic>() { new Clinic() { Abbr=Lan.g(this,"None") } };
			_listClinics.AddRange(Clinics.GetForUserod(_curUser));//Not Security.CurUser
			bool isListAlpha = PrefC.GetBool(PrefName.ClinicListIsAlphabetical);
			_listClinics=_listClinics.OrderBy(x => x.ClinicNum>0)
				.ThenBy(x => isListAlpha?x.Abbr:x.ItemOrder.ToString().PadLeft(6,'0'))//sort by Abbr or Item order based on pref. string sort.
				.ToList();
			//Fill comboClinic
			comboClinic.Items.Clear();
			_listClinics.ForEach(x => comboClinic.Items.Add(x.Abbr));
			comboClinic.IndexSelectOrSetText(_listClinics.FindIndex(x => x.ClinicNum==_selectedClinicNum)
				,() => { return Clinics.GetAbbr(_selectedClinicNum); });
			if(idxOld==-1 && comboClinic.SelectedIndex==-1) {
				FillCombosForProviders();//because the comboClinic_SelectedIndexChanged will not fire.
			}
		}

		private void comboClinic_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboClinic.SelectedIndex>-1) {
				_selectedClinicNum=_listClinics[comboClinic.SelectedIndex].ClinicNum;
			}
			FillCombosForProviders();
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
			comboProv.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvNum),() => { return Providers.GetAbbr(_selectedProvNum); });
		}

		private void butPickOrderProvInternal_Click(object sender,EventArgs e) {
			FormProviderPick formP = new FormProviderPick(_listProviders);
			formP.SelectedProvNum=_selectedProvOrderNum;
			formP.ShowDialog();
			if(formP.DialogResult!=DialogResult.OK) {
				return;
			}
			SetOrderingProvider(Providers.GetProv(formP.SelectedProvNum));
		}

		private void butPickOrderProvReferral_Click(object sender,EventArgs e) {
			FormReferralSelect form=new FormReferralSelect();
			form.IsSelectionMode=true;
			form.IsDoctorSelectionMode=true;
			form.IsShowPat=false;
			form.IsShowDoc=true;
			form.IsShowOther=false;
			form.ShowDialog();
			if(form.DialogResult!=DialogResult.OK) {
				return;
			}
			SetOrderingReferral(form.SelectedReferral);
		}

		private void butNoneOrderProv_Click(object sender,EventArgs e) {
			SetOrderingProvider(null);//Clears both the internal ordering and referral ordering providers.
		}

		private void SetOrderingProvider(Provider prov) {
			if(prov==null) {
				_selectedProvOrderNum=0;
				textOrderingProviderOverride.Text="";
			}
			else {
				_selectedProvOrderNum=prov.ProvNum;
				textOrderingProviderOverride.Text=prov.GetFormalName()+"  NPI: "+(prov.NationalProvID.Trim()==""?"Missing":prov.NationalProvID);
			}
			_referralOrdering=null;
		}

		private void SetOrderingReferral(Referral referral) {
			_referralOrdering=referral;
			if(referral==null) {
				textOrderingProviderOverride.Text="";
			}
			else {
				textOrderingProviderOverride.Text=referral.GetNameFL()+"  NPI: "+(referral.NationalProvID.Trim()==""?"Missing":referral.NationalProvID);
			}
			_selectedProvOrderNum=0;
		}

		///<summary>Fills combo provider based on which clinic is selected and attempts to preserve provider selection if any.</summary>
		private void FillCombosForProviders() {
			if(comboProv.SelectedIndex>-1) {//valid prov selected, non none or nothing.
				_selectedProvNum = _listProviders[comboProv.SelectedIndex].ProvNum;
			}
			_listProviders=Providers.GetProvsForClinic(_selectedClinicNum).OrderBy(x => x.ProvNum>0).ThenBy(x => x.ItemOrder).ToList();//order list properly, None first
			//Provider
			comboProv.Items.Clear();
			_listProviders.ForEach(x => comboProv.Items.Add(x.Abbr));
			comboProv.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvNum),() => { return Providers.GetAbbr(_selectedProvNum); });
		}

		///<summary>ONLY run on startup. Fills the basic controls, except not the ones in the upper left panel which are handled in SetControlsUpperLeft.</summary>
		private void FillControlsOnStartup(){
			comboProcStatus.Items.Clear();
			comboProcStatus.Items.Add(Lan.g(this,"Treatment Planned"));
			comboProcStatus.Items.Add(Lan.g(this,"Complete"));
			if(!PrefC.GetBool(PrefName.EasyHideClinical)) {
				comboProcStatus.Items.Add(Lan.g(this,"Existing-Current Prov"));
				comboProcStatus.Items.Add(Lan.g(this,"Existing-Other Prov"));
				comboProcStatus.Items.Add(Lan.g(this,"Referred Out"));
				//comboProcStatus.Items.Add(Lan.g(this,"Deleted"));
				comboProcStatus.Items.Add(Lan.g(this,"Condition"));
			}
			if(ProcCur.ProcStatus==ProcStat.TP){
				comboProcStatus.SelectedIndex=0;
			}
			if(ProcCur.ProcStatus==ProcStat.C) {
				comboProcStatus.SelectedIndex=1;
			}
			if(!PrefC.GetBool(PrefName.EasyHideClinical)) {
				if(ProcCur.ProcStatus==ProcStat.EC) {
					comboProcStatus.SelectedIndex=2;
				}
				if(ProcCur.ProcStatus==ProcStat.EO) {
					comboProcStatus.SelectedIndex=3;
				}
				if(ProcCur.ProcStatus==ProcStat.R) {
					comboProcStatus.SelectedIndex=4;
				}
				if(ProcCur.ProcStatus==ProcStat.Cn) {
					comboProcStatus.SelectedIndex=5;
				}
			}
			if(ProcCur.ProcStatus==ProcStat.TPi) {
				comboProcStatus.Items.Add(Lan.g("TreatPlan","Treatment Planned Inactive"));
				comboProcStatus.SelectedIndex=comboProcStatus.Items.Count-1;
				comboProcStatus.Enabled=false;
				butSetComplete.Enabled=false;
			}
			if(ProcCur.ProcStatus==ProcStat.D && ProcCur.IsLocked){//an invalidated proc
				comboProcStatus.Items.Clear();
				comboProcStatus.Items.Add(Lan.g(this,"Invalidated"));
				comboProcStatus.SelectedIndex=0;
				comboProcStatus.Enabled=false;
				butInvalidate.Visible=false;
				butOK.Enabled=false;
				butDelete.Enabled=false;
				butChange.Enabled=false;
				butEditAnyway.Enabled=false;
				butSetComplete.Enabled=false;
				butAddEstimate.Enabled=false;
				butAddAdjust.Enabled=false;
			}
			//if clinical is hidden, then there's a chance that no item is selected at this point.
			_listDiagnosisDefs=Defs.GetDefsForCategory(DefCat.Diagnosis,true);
			_listPrognosisDefs=Defs.GetDefsForCategory(DefCat.Prognosis,true);
			_listTxPriorityDefs=Defs.GetDefsForCategory(DefCat.TxPriorities,true);
			_listBillingTypeDefs=Defs.GetDefsForCategory(DefCat.BillingTypes,true);
			comboDx.Items.Clear();
			for(int i=0;i<_listDiagnosisDefs.Count;i++){
				comboDx.Items.Add(_listDiagnosisDefs[i].ItemName);
				if(_listDiagnosisDefs[i].DefNum==ProcCur.Dx)
					comboDx.SelectedIndex=i;
			}
			comboPrognosis.Items.Clear();
			comboPrognosis.Items.Add(Lan.g(this,"no prognosis"));
			comboPrognosis.SelectedIndex=0;
			for(int i=0;i<_listPrognosisDefs.Count;i++) {
				comboPrognosis.Items.Add(_listPrognosisDefs[i].ItemName);
				if(_listPrognosisDefs[i].DefNum==ProcCur.Prognosis)
					comboPrognosis.SelectedIndex=i+1;
			}
			checkHideGraphics.Checked=ProcCur.HideGraphics;
			if(Programs.UsingOrion && this.IsNew && !OrionDentist){
				_selectedProvNum=Providers.GetOrionProvNum(ProcCur.ProvNum);
				ProcCur.ProvNum=_selectedProvNum;//Returns 0 if logged in as non provider.
				FillCombosForProviders();//Second time this is called, but only if using Orion.
			}//ProvNum of 0 will be required to change before closing form.
			comboPriority.Items.Clear();
			comboPriority.Items.Add(Lan.g(this,"no priority"));
			comboPriority.SelectedIndex=0;
			for(int i=0;i<_listTxPriorityDefs.Count;i++){
				comboPriority.Items.Add(_listTxPriorityDefs[i].ItemName);
				if(_listTxPriorityDefs[i].DefNum==ProcCur.Priority)
					comboPriority.SelectedIndex=i+1;
			}
			comboBillingTypeOne.Items.Clear();
			comboBillingTypeOne.Items.Add(Lan.g(this,"none"));
			comboBillingTypeOne.SelectedIndex=0;
			for(int i=0;i<_listBillingTypeDefs.Count;i++) {
				comboBillingTypeOne.Items.Add(_listBillingTypeDefs[i].ItemName);
				if(_listBillingTypeDefs[i].DefNum==ProcCur.BillingTypeOne)
					comboBillingTypeOne.SelectedIndex=i+1;
			}
			comboBillingTypeTwo.Items.Clear();
			comboBillingTypeTwo.Items.Add(Lan.g(this,"none"));
			comboBillingTypeTwo.SelectedIndex=0;
			for(int i=0;i<_listBillingTypeDefs.Count;i++) {
				comboBillingTypeTwo.Items.Add(_listBillingTypeDefs[i].ItemName);
				if(_listBillingTypeDefs[i].DefNum==ProcCur.BillingTypeTwo)
					comboBillingTypeTwo.SelectedIndex=i+1;
			}
			textBillingNote.Text=ProcCur.BillingNote;
			textNotes.Text=ProcCur.Note;
			comboPlaceService.Items.Clear();
			comboPlaceService.Items.AddRange(Enum.GetNames(typeof(PlaceOfService)));
			comboPlaceService.SelectedIndex=(int)ProcCur.PlaceService;
			//checkHideGraphical.Checked=ProcCur.HideGraphical;
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				comboClinic.Visible=false;
				labelClinic.Visible=false;
			}
			textSite.Text=Sites.GetDescription(ProcCur.SiteNum);
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				if(ProcCur.CanadianTypeCodes==null || ProcCur.CanadianTypeCodes=="") {
					checkTypeCodeX.Checked=true;
				}
				else {
					if(ProcCur.CanadianTypeCodes.Contains("A")) {
						checkTypeCodeA.Checked=true;
					}
					if(ProcCur.CanadianTypeCodes.Contains("B")) {
						checkTypeCodeB.Checked=true;
					}
					if(ProcCur.CanadianTypeCodes.Contains("C")) {
						checkTypeCodeC.Checked=true;
					}
					if(ProcCur.CanadianTypeCodes.Contains("E")) {
						checkTypeCodeE.Checked=true;
					}
					if(ProcCur.CanadianTypeCodes.Contains("L")) {
						checkTypeCodeL.Checked=true;
					}
					if(ProcCur.CanadianTypeCodes.Contains("S")) {
						checkTypeCodeS.Checked=true;
					}
					if(ProcCur.CanadianTypeCodes.Contains("X")) {
						checkTypeCodeX.Checked=true;
					}
				}
			}
			else{
				if(ProcedureCode2.IsProsth){
					listProsth.Items.Add(Lan.g(this,"No"));
					listProsth.Items.Add(Lan.g(this,"Initial"));
					listProsth.Items.Add(Lan.g(this,"Replacement"));
					switch(ProcCur.Prosthesis){
						case "":
							listProsth.SelectedIndex=0;
							break;
						case "I":
							listProsth.SelectedIndex=1;
							break;
						case "R":
							listProsth.SelectedIndex=2;
							break;
					}
					if(ProcCur.DateOriginalProsth.Year>1880){
						textDateOriginalProsth.Text=ProcCur.DateOriginalProsth.ToShortDateString();
					}
					checkIsDateProsthEst.Checked=ProcCur.IsDateProsthEst;
				}
				else{
					groupProsth.Visible=false;
				}
			}
			textDiscount.Text=ProcCur.Discount.ToString("f");
			//medical
			textMedicalCode.Text=ProcCur.MedicalCode;
			if(ProcCur.IcdVersion==9) {
				checkIcdVersion.Checked=false;
			}
			else {//ICD-10
				checkIcdVersion.Checked=true;
			}
			SetIcdLabels();
			textDiagnosticCode.Text=ProcCur.DiagnosticCode;
			textDiagnosticCode2.Text=ProcCur.DiagnosticCode2;
			textDiagnosticCode3.Text=ProcCur.DiagnosticCode3;
			textDiagnosticCode4.Text=ProcCur.DiagnosticCode4;
			checkIsPrincDiag.Checked=ProcCur.IsPrincDiag;
			textCodeMod1.Text = ProcCur.CodeMod1;
			textCodeMod2.Text = ProcCur.CodeMod2;
			textCodeMod3.Text = ProcCur.CodeMod3;
			textCodeMod4.Text = ProcCur.CodeMod4;
			textUnitQty.Text = ProcCur.UnitQty.ToString();
			comboUnitType.Items.Clear();
			_snomedBodySite=Snomeds.GetByCode(ProcCur.SnomedBodySite);
			if(_snomedBodySite==null) {
				textSnomedBodySite.Text="";
			}
			else {
				textSnomedBodySite.Text=_snomedBodySite.Description;
			}
			for(int i=0;i<Enum.GetNames(typeof(ProcUnitQtyType)).Length;i++) {
				comboUnitType.Items.Add(Enum.GetNames(typeof(ProcUnitQtyType))[i]);
			}
			comboUnitType.SelectedIndex=(int)ProcCur.UnitQtyType;
			textRevCode.Text = ProcCur.RevCode;
			//DrugNDC is handled in SetControlsUpperLeft
			comboDrugUnit.Items.Clear();
			for(int i=0;i<Enum.GetNames(typeof(EnumProcDrugUnit)).Length;i++){
				comboDrugUnit.Items.Add(Enum.GetNames(typeof(EnumProcDrugUnit))[i]);
			}
			comboDrugUnit.SelectedIndex=(int)ProcCur.DrugUnit;
			if(ProcCur.DrugQty!=0){
				textDrugQty.Text=ProcCur.DrugQty.ToString();
			}
			textClaimNote.Text=ProcCur.ClaimNote;
			textUser.Text=Userods.GetName(ProcCur.UserNum);//might be blank. Will change automatically if user changes note or alters sig.
			string keyData=GetSignatureKey();
			signatureBoxWrapper.FillSignature(ProcCur.SigIsTopaz,keyData,ProcCur.Signature);
			if(Programs.UsingOrion) {//panelOrion.Visible) {
				comboDPC.Items.Clear();
				//comboDPC.Items.AddRange(Enum.GetNames(typeof(OrionDPC)));
				comboDPC.Items.Add("Not Specified");
				comboDPC.Items.Add("None");
				comboDPC.Items.Add("1A-within 1 day");
				comboDPC.Items.Add("1B-within 30 days");
				comboDPC.Items.Add("1C-within 60 days");
				comboDPC.Items.Add("2-within 120 days");
				comboDPC.Items.Add("3-within 1 year");
				comboDPC.Items.Add("4-no further treatment/appt");
				comboDPC.Items.Add("5-no appointment needed");
				comboDPCpost.Items.Clear();
				comboDPCpost.Items.Add("Not Specified");
				comboDPCpost.Items.Add("None");
				comboDPCpost.Items.Add("1A-within 1 day");
				comboDPCpost.Items.Add("1B-within 30 days");
				comboDPCpost.Items.Add("1C-within 60 days");
				comboDPCpost.Items.Add("2-within 120 days");
				comboDPCpost.Items.Add("3-within 1 year");
				comboDPCpost.Items.Add("4-no further treatment/appt");
				comboDPCpost.Items.Add("5-no appointment needed");
				comboStatus.Items.Clear();
				comboStatus.Items.Add("TP-treatment planned");
				comboStatus.Items.Add("C-completed");
				comboStatus.Items.Add("E-existing prior to incarceration");
				comboStatus.Items.Add("R-refused treatment");
				comboStatus.Items.Add("RO-referred out to specialist");
				comboStatus.Items.Add("CS-completed by specialist");
				comboStatus.Items.Add("CR-completed by registry");
				comboStatus.Items.Add("CA_Tx-cancelled, tx plan changed");
				comboStatus.Items.Add("CA_EPRD-cancelled, eligible parole");
				comboStatus.Items.Add("CA_P/D-cancelled, parole/discharge");
				comboStatus.Items.Add("S-suspended, unacceptable plaque");
				comboStatus.Items.Add("ST-stop clock, multi visit");
				comboStatus.Items.Add("W-watch");
				comboStatus.Items.Add("A-alternative");
				comboStatus.SelectedIndex=0;
				ProcedureCode pc=ProcedureCodes.GetProcCodeFromDb(ProcCur.CodeNum);
				checkIsRepair.Visible=pc.IsProsth;
				//DateTP doesn't get set sometimes and calculations are made based on the DateTP. So set it to the current date as fail-safe.
				if(ProcCur.DateTP.Year<1880) {
					textDateTP.Text=MiscData.GetNowDateTime().ToShortDateString();
				}
				else {
					textDateTP.Text=ProcCur.DateTP.ToShortDateString();
				}
				BitArray ba=new BitArray(new int[] { (int)OrionProcCur.Status2 });//should nearly always be non-zero
				for(int i=0;i<ba.Length;i++) {
					if(ba[i]) {
						comboStatus.SelectedIndex=i;
						break;
					}
				}
				if(!IsNew) {
					OrionProcOld=OrionProcCur.Copy();
					comboDPC.SelectedIndex=(int)OrionProcCur.DPC;
					comboDPCpost.SelectedIndex=(int)OrionProcCur.DPCpost;
					if(OrionProcCur.DPC==OrionDPC.NotSpecified ||
						OrionProcCur.DPC==OrionDPC.None ||
						OrionProcCur.DPC==OrionDPC._4 ||
						OrionProcCur.DPC==OrionDPC._5) {
						labelScheduleBy.Visible=true;
					}
					if(OrionProcCur.DateScheduleBy.Year>1880) {
						textDateScheduled.Text=OrionProcCur.DateScheduleBy.ToShortDateString();
					}
					if(OrionProcCur.DateStopClock.Year>1880) {
						textDateStop.Text=OrionProcCur.DateStopClock.ToShortDateString();
					}
					checkIsOnCall.Checked=OrionProcCur.IsOnCall;
					checkIsEffComm.Checked=OrionProcCur.IsEffectiveComm;
					checkIsRepair.Checked=OrionProcCur.IsRepair;
				}
				else {
					labelScheduleBy.Visible=true;
					comboDPC.SelectedIndex=0;
					comboDPCpost.SelectedIndex=0;
					textDateStop.Text="";
				}
			}
		}

		private void FormProcEdit_Shown(object sender,EventArgs e) {
			//Prompt users for auto notes if they have the preference set.
			if(PrefC.GetBool(PrefName.ProcPromptForAutoNote)) {//Replace [[text]] sections within the note with AutoNotes.
				PromptForAutoNotes();
			}
			//Scroll to the end of the note for procedures for today (or completed today).
			if(ProcCur.DateEntryC.Date==DateTime.Today) {
				textNotes.Select(textNotes.Text.Length,0);
			}
			CheckForCompleteNote();
		}

		///<summary>Loops through textNotes.Text and will insert auto notes and prompt them for prompting auto notes.</summary>
		private void PromptForAutoNotes() {
			List<Match> listMatches=Regex.Matches(textNotes.Text,@"\[\[.+?\]\]").OfType<Match>().ToList();
			listMatches.RemoveAll(x => AutoNotes.GetByTitle(x.Value.TrimStart('[').TrimEnd(']'))=="");//remove matches that are not autonotes.
			int loc=0;
			foreach(Match match in listMatches) {
				string autoNoteTitle=match.Value.TrimStart('[').TrimEnd(']');
				string note=AutoNotes.GetByTitle(autoNoteTitle);
				int matchloc=textNotes.Text.IndexOf(match.Value,loc);
				FormAutoNoteCompose FormA=new FormAutoNoteCompose();
				FormA.MainTextNote=note;
				FormA.ShowDialog();
				if(FormA.DialogResult==DialogResult.Cancel) {
					loc=matchloc+match.Value.Length;
					continue;//if they cancel, go to the next autonote.
				}
				if(FormA.DialogResult==DialogResult.OK) {
					//When setting the Text on a RichTextBox, \r\n is replaced with \n, so we need to do the same so that our location variable is correct.
					loc=matchloc+FormA.CompletedNote.Replace("\r\n","\n").Length;
					string resultstr=textNotes.Text.Substring(0,matchloc)+FormA.CompletedNote;
					if(textNotes.Text.Length > matchloc+match.Value.Length) {
						resultstr+=textNotes.Text.Substring(matchloc+match.Value.Length);
					}
					textNotes.Text=resultstr;
				}
			}
			bool hasAutoNotePrompt=Regex.IsMatch(textNotes.Text,_autoNotePromptRegex);
			butEditAutoNote.Visible=hasAutoNotePrompt;
		}

		private string GetSignatureKey() {
			string keyData=ProcCur.Note;
			keyData+=ProcCur.UserNum.ToString();
			return keyData;
		}

		private void SetSurfButtons(){
			if(textSurfaces.Text.Contains("B") || textSurfaces.Text.Contains("F")) butBF.BackColor=Color.White;
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				if(textSurfaces.Text.Contains("V")) butBF.BackColor=Color.White;
			}
			if(textSurfaces.Text.Contains("O") || textSurfaces.Text.Contains("I")) butOI.BackColor=Color.White;
			if(textSurfaces.Text.Contains("M")) butM.BackColor=Color.White;
			if(textSurfaces.Text.Contains("D")) butD.BackColor=Color.White;
			if(textSurfaces.Text.Contains("L")) butL.BackColor=Color.White;
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				if(textSurfaces.Text.Contains("5")) butV.BackColor=Color.White;
			}
			else{
				if(textSurfaces.Text.Contains("V")) butV.BackColor=Color.White;
			}
		}

		///<summary>Called on open and after changing code.  Sets the visibilities and the data of all the fields in the upper left panel.</summary>
		private void SetControlsUpperLeft(){
			textDateTP.Text=ProcCur.DateTP.ToString("d");
			DateTime dateT;
			if(IsStartingUp){
				textDate.Text=ProcCur.ProcDate.ToString("d");
				if(ProcCur.ProcDate.Date!=ProcCur.DateComplete.Date && ProcCur.DateComplete.Year>1880) {
					//show proc date Original if the date is different than proc date and set.
					labelOrigDateComp.Visible=true;
					textOrigDateComp.Visible=true;
					textOrigDateComp.Text=ProcCur.DateComplete.ToString("d");
				}
				else {//Hide Orig Date Comp if same as current procedure date.
					labelOrigDateComp.Visible=false;
					textOrigDateComp.Visible=false;
				}
				dateT=PIn.DateT(ProcCur.ProcTime.ToString());
				if(dateT.ToShortTimeString()!="12:00 AM"){
					textTimeStart.Text+=dateT.ToShortTimeString();
				}
				if(Programs.UsingOrion || PrefC.GetBool(PrefName.ShowFeatureMedicalInsurance)) {
					dateT=PIn.DateT(ProcCur.ProcTimeEnd.ToString());
					if(dateT.ToShortTimeString()!="12:00 AM") {
						textTimeEnd.Text=dateT.ToShortTimeString();
					}
					UpdateFinalMin();			
					if(ProcCur.DateTP.Year<1880) {
						textDateTP.Text=MiscData.GetNowDateTime().ToShortDateString();
					}
				}
			}
			textProc.Text=ProcedureCode2.ProcCode;
			textDesc.Text=ProcedureCode2.Descript;
			textDrugNDC.Text=ProcedureCode2.DrugNDC;
			switch (ProcedureCode2.TreatArea){
				case TreatmentArea.Surf:
					this.textTooth.Visible=true;
					this.labelTooth.Visible=true;
					this.textSurfaces.Visible=true;
					this.labelSurfaces.Visible=true;
					this.panelSurfaces.Visible=true;
					if(Tooth.IsValidDB(ProcCur.ToothNum)) {
						errorProvider2.SetError(textTooth,"");
						textTooth.Text=Tooth.ToInternat(ProcCur.ToothNum);
						textSurfaces.Text=Tooth.SurfTidyFromDbToDisplay(ProcCur.Surf,ProcCur.ToothNum);
						SetSurfButtons();
					}
					else{
						errorProvider2.SetError(textTooth,Lan.g(this,"Invalid tooth number."));
						textTooth.Text=ProcCur.ToothNum;
						//textSurfaces.Text=Tooth.SurfTidy(ProcCur.Surf,"");//only valid toothnums allowed
					}
					if(textSurfaces.Text=="")
						errorProvider2.SetError(textSurfaces,"No surfaces selected.");
					else
						errorProvider2.SetError(textSurfaces,"");
					break;
				case TreatmentArea.Tooth:
					this.textTooth.Visible=true;
					this.labelTooth.Visible=true;
					if(Tooth.IsValidDB(ProcCur.ToothNum)){
						errorProvider2.SetError(textTooth,"");
						textTooth.Text=Tooth.ToInternat(ProcCur.ToothNum);
					}
					else{
						errorProvider2.SetError(textTooth,Lan.g(this,"Invalid tooth number."));
						textTooth.Text=ProcCur.ToothNum;
					}
					break;
				case TreatmentArea.Mouth:
						break;
				case TreatmentArea.Quad:
					this.groupQuadrant.Visible=true;
					switch (ProcCur.Surf){
						case "UR": this.radioUR.Checked=true; break;
						case "UL": this.radioUL.Checked=true; break;
						case "LR": this.radioLR.Checked=true; break;
						case "LL": this.radioLL.Checked=true; break;
						//default : 
					}
					break;
				case TreatmentArea.Sextant:
					this.groupSextant.Visible=true;
					switch (ProcCur.Surf){
						case "1": this.radioS1.Checked=true; break;
						case "2": this.radioS2.Checked=true; break;
						case "3": this.radioS3.Checked=true; break;
						case "4": this.radioS4.Checked=true; break;
						case "5": this.radioS5.Checked=true; break;
						case "6": this.radioS6.Checked=true; break;
							//default:
					}
					if(IsSextantSelected()) {
						errorProvider2.SetError(groupSextant,"");
					}
					else {
						errorProvider2.SetError(groupSextant,Lan.g(this,"Please select a sextant treatment area."));
					}
					break;
				case TreatmentArea.Arch:
					this.groupArch.Visible=true;
					switch (ProcCur.Surf){
						case "U": this.radioU.Checked=true; break;
						case "L": this.radioL.Checked=true; break;
					}
					if(IsArchSelected()) {
						errorProvider2.SetError(groupArch,"");
					}
					else {
						errorProvider2.SetError(groupArch,Lan.g(this,"Please select a arch treatment area."));
					}
					break;
				case TreatmentArea.ToothRange:
					this.labelRange.Visible=true;
					this.listBoxTeeth.Visible=true;
					this.listBoxTeeth2.Visible=true;
					listBoxTeeth.SelectionMode=SelectionMode.MultiExtended;
					listBoxTeeth2.SelectionMode=SelectionMode.MultiExtended;
					if(ProcCur.ToothRange==null){
						break;
					}
   			  string[] sArray=ProcCur.ToothRange.Split(',');//in american
					int idxAmer;
          for(int i=0;i<sArray.Length;i++)  {
						idxAmer=Array.IndexOf(Tooth.labelsUniversal,sArray[i]);
						if(idxAmer==-1){
							continue;
						}
						if(idxAmer<16){
							listBoxTeeth.SetSelected(idxAmer,true);
						}
						else if(idxAmer<32){//ignore anything after 32.
							listBoxTeeth2.SetSelected(idxAmer-16,true);
						}
            /*for(int j=0;j<listBoxTeeth.Items.Count;j++)  {
              if(Tooth.ToInternat(sArray[i])==listBoxTeeth.Items[j].ToString())
				 		    listBoxTeeth.SelectedItem=Tooth.ToInternat(sArray[i]);
					  }
  			    for(int j=0;j<listBoxTeeth2.Items.Count;j++)  {
              if(Tooth.ToInternat(sArray[i])==listBoxTeeth2.Items[j].ToString())
				 		    listBoxTeeth2.SelectedItem=Tooth.ToInternat(sArray[i]);
            }*/
					} 
					break;
			}//end switch
			textProcFee.Text=ProcCur.ProcFee.ToString("n");
		}

		///<summary>Enable/disable controls based on permissions ProcComplEdit and ProcComplEditLimited.</summary>
		private void SetControlsEnabled(bool isSilent) {
			//Return if the current procedure isn't considered complete (C, EC, EO).
			//Don't allow adding an estimate, since a new estimate could change the total writeoff amount for the proc.
			if(!ProcCur.ProcStatus.In(ProcStat.C,ProcStat.EO,ProcStat.EC)) {
				return;
			}
			//Use ProcDate to compare to the date/days newer restriction.
			Permissions perm=Permissions.ProcComplEdit;
			if(ProcCur.ProcStatus.In(ProcStat.EO,ProcStat.EC)) {
				perm=Permissions.ProcExistingEdit;
			}
			bool hasDateLock=Security.IsGlobalDateLock(perm,ProcCur.ProcDate,isSilent);//really only used to silence other security messages.
			bool hasProcEditLim=Security.IsAuthorized(Permissions.ProcComplEditLimited,ProcCur.ProcDate,isSilent);
			bool hasProcEditComp=Security.IsAuthorized(perm,ProcCur.ProcDate,isSilent||hasDateLock||!hasProcEditLim,hasDateLock);
			if(!hasProcEditLim) {//always silent. All permissions messages generated at top of method.
				//user doesn't have limited or full proc complete edit permission
				List<Control> listControls=Controls.OfType<Control>().Where(x => x!=butCancel && x!=butSearch).ToList();//leave the cancel and search buttons enabled
				listControls.AddRange(tabControl.TabPages.OfType<TabPage>().SelectMany(x => x.Controls.OfType<Control>()).ToList());
				foreach(Control cCur in listControls) {
					if(cCur is TextBox || cCur is ValidDate || cCur is ValidDouble) {
						((TextBox)cCur).ReadOnly=true;
						cCur.BackColor=SystemColors.Control;
					}
					else if(cCur is ODtextBox) {
						((ODtextBox)cCur).ReadOnly=true;
						cCur.BackColor=SystemColors.Control;
					}
					else if(cCur is UI.Button || cCur is ComboBox || cCur is CheckBox || cCur is GroupBox || cCur is Panel || cCur is SignatureBoxWrapper) {
						cCur.Enabled=false;
					}
				}
			}
			if(!hasProcEditComp) {//always silent. All permissions messages generated at top of method.
				//list of controls enabled for those with ProcComplEditLimited permission
				List<Control> listControlsEnabled=new List<Control>() {
					panel1,panelSurfaces,groupArch,listBoxTeeth,listBoxTeeth2,textTooth,textSurfaces,//controls enabled within panel1
					checkHideGraphics,comboDx,groupProsth,textClaimNote,checkNoBillIns,//controls enabled below panel1 and on tabControl
					textNotes,signatureBoxWrapper,buttonUseAutoNote,butEditAutoNote,butChangeUser,butAppend,//proc note controls
					butSearch,butCancel,butOK,butEditAnyway,butAddAdjust//buttons enabled
				};
				//list of all controls on the form that will be disabled since user doesn't have ProcComplEdit (full) permission
				List<Control> listControlsDisabled=Controls.OfType<Control>().ToList();
				//add all controls on the tabControl.TabPages except for the Misc and Medical tabs
				listControlsDisabled.AddRange(tabControl.TabPages.OfType<TabPage>().Where(x => x!=tabPageMisc && x!=tabPageMedical)
					.SelectMany(x => x.Controls.OfType<Control>()));
				//add panel1's (upper left corner panel) controls
				listControlsDisabled.AddRange(panel1.Controls.OfType<Control>());
				//now remove any of the controls in the enabled list
				listControlsDisabled.RemoveAll(x => listControlsEnabled.Contains(x));
				foreach(Control cCur in listControlsDisabled) {
					if(cCur is TextBox || cCur is ValidDate || cCur is ValidDouble) {
						((TextBox)cCur).ReadOnly=true;
						cCur.BackColor=SystemColors.Control;
					}
					else if(cCur is ODtextBox) {
						((ODtextBox)cCur).ReadOnly=true;
						cCur.BackColor=SystemColors.Control;
					}
					else if(cCur is UI.Button || cCur is ComboBox || cCur is CheckBox || cCur is GroupBox || cCur is Panel || cCur is SignatureBoxWrapper) {
						cCur.Enabled=false;
					}
				}
			}
			if(!Security.IsAuthorized(perm,ProcCur.ProcDate,true,true)
				&& Security.IsAuthorized(perm,ProcCur.ProcDate,true,true,ProcCur.CodeNum,ProcCur.ProcFee,0,0)) 
			{
				//This is a $0 procedure for a proc code marked as bypassed.
				butDelete.Enabled=true;
			}
		}//end SetControls

		private void FillReferral(bool doRefreshData=true) {
			if(doRefreshData) {
				_loadData.ListRefAttaches=RefAttaches.RefreshFiltered(ProcCur.PatNum,false,ProcCur.ProcNum);
			}
			List<RefAttach> refsList=_loadData.ListRefAttaches;
			if(refsList.Count==0) {
				textReferral.Text="";
			}
			else {
				Referral referral;
				if(Referrals.TryGetReferral(refsList[0].ReferralNum,out referral)) {
					textReferral.Text=referral.LName+", ";
				}
				if(refsList[0].DateProcComplete.Year<1880) {
					textReferral.Text+=refsList[0].RefDate.ToShortDateString();
				}
				else{
					textReferral.Text+=Lan.g(this,"done:")+refsList[0].DateProcComplete.ToShortDateString();
				}
				if(refsList[0].RefToStatus!=ReferralToStatus.None){
					textReferral.Text+=refsList[0].RefToStatus.ToString();
				}
			}
		}

		private void butReferral_Click(object sender,EventArgs e) {
			FormReferralsPatient FormRP=new FormReferralsPatient();
			FormRP.PatNum=ProcCur.PatNum;
			FormRP.ProcNum=ProcCur.ProcNum;
			FormRP.ShowDialog();
			FillReferral();
		}

		private void FillIns(){
			FillIns(true);
		}

		private void FillIns(bool refreshClaimProcsFirst){
			if(refreshClaimProcsFirst) {
				//ClaimProcList=ClaimProcs.Refresh(PatCur.PatNum);
				//}
				ClaimProcsForProc=ClaimProcs.RefreshForProc(ProcCur.ProcNum);
			}
			gridIns.BeginUpdate();
			gridIns.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableProcIns","Ins Plan"),190);
			gridIns.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcIns","Pri/Sec"),50,HorizontalAlignment.Center);
			gridIns.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcIns","Status"),50,HorizontalAlignment.Center);
			gridIns.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcIns","NoBill"),45,HorizontalAlignment.Center);
			gridIns.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcIns","Copay"),55,HorizontalAlignment.Right);
			gridIns.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcIns","Deduct"),55,HorizontalAlignment.Right);
			gridIns.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcIns","Percent"),55,HorizontalAlignment.Center);
			gridIns.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcIns","Ins Est"),55,HorizontalAlignment.Right);
			gridIns.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcIns","Ins Pay"),55,HorizontalAlignment.Right);
			gridIns.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcIns","WriteOff"),55,HorizontalAlignment.Right);
			gridIns.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcIns","Estimate Note"),100);
			gridIns.Columns.Add(col);		 
			col=new ODGridColumn(Lan.g("TableProcIns","Remarks"),165);
			gridIns.Columns.Add(col);		 
			gridIns.Rows.Clear();
			ODGridRow row;
			checkNoBillIns.CheckState=CheckState.Unchecked;
			bool allNoBillIns=true;
			InsPlan plan;
			//ODGridCell cell;
			for(int i=0;i<ClaimProcsForProc.Count;i++) {
				if(ClaimProcsForProc[i].NoBillIns){
					checkNoBillIns.CheckState=CheckState.Indeterminate;
				}
				else{
					allNoBillIns=false;
				}
				row=new ODGridRow();
				row.Cells.Add(InsPlans.GetDescript(ClaimProcsForProc[i].PlanNum,FamCur,PlanList,ClaimProcsForProc[i].InsSubNum,SubList));
				plan=InsPlans.GetPlan(ClaimProcsForProc[i].PlanNum,PlanList);
				if(plan==null) {
					MsgBox.Show(this,"No insurance plan exists for this claim proc.  Please run database maintenance.");
					return;
				}
				if(plan.IsMedical) {
					row.Cells.Add("Med");
				}
				else if(ClaimProcsForProc[i].InsSubNum==PatPlans.GetInsSubNum(PatPlanList,PatPlans.GetOrdinal(PriSecMed.Primary,PatPlanList,PlanList,SubList))){
					row.Cells.Add("Pri");
				}
				else if(ClaimProcsForProc[i].InsSubNum==PatPlans.GetInsSubNum(PatPlanList,PatPlans.GetOrdinal(PriSecMed.Secondary,PatPlanList,PlanList,SubList))) {
					row.Cells.Add("Sec");
				}
				else {
					row.Cells.Add("");
				}
				switch(ClaimProcsForProc[i].Status) {
					case ClaimProcStatus.Received:
						row.Cells.Add("Recd");
						break;
					case ClaimProcStatus.NotReceived:
						row.Cells.Add("NotRec");
						break;
					//adjustment would never show here
					case ClaimProcStatus.Preauth:
						row.Cells.Add("PreA");
						break;
					case ClaimProcStatus.Supplemental:
						row.Cells.Add("Supp");
						break;
					case ClaimProcStatus.CapClaim:
						row.Cells.Add("CapClaim");
						break;
					case ClaimProcStatus.Estimate:
						row.Cells.Add("Est");
						break;
					case ClaimProcStatus.CapEstimate:
						row.Cells.Add("CapEst");
						break;
					case ClaimProcStatus.CapComplete:
						row.Cells.Add("CapComp");
						break;
					default:
						row.Cells.Add("");
						break;
				}
				if(ClaimProcsForProc[i].NoBillIns) {
					row.Cells.Add("X");
					if(ClaimProcsForProc[i].Status!=ClaimProcStatus.CapComplete	&& ClaimProcsForProc[i].Status!=ClaimProcStatus.CapEstimate) {
						row.Cells.Add("");
						row.Cells.Add("");
						row.Cells.Add("");
						row.Cells.Add("");
						row.Cells.Add("");
						row.Cells.Add("");
						row.Cells.Add("");
						row.Cells.Add("");
						row.Cells.Add("");
						gridIns.Rows.Add(row);
						continue;
					}
				}
				else {
					row.Cells.Add("");
				}
				row.Cells.Add(ClaimProcs.GetCopayDisplay(ClaimProcsForProc[i]));
				double ded=ClaimProcs.GetDeductibleDisplay(ClaimProcsForProc[i]);
				if(ded>0) {
					row.Cells.Add(ded.ToString("n"));
				}
				else {
					row.Cells.Add("");
				}
				row.Cells.Add(ClaimProcs.GetPercentageDisplay(ClaimProcsForProc[i]));
				row.Cells.Add(ClaimProcs.GetEstimateDisplay(ClaimProcsForProc[i]));
				if(ClaimProcsForProc[i].Status==ClaimProcStatus.Estimate
					|| ClaimProcsForProc[i].Status==ClaimProcStatus.CapEstimate) 
				{
					row.Cells.Add("");
					row.Cells.Add(ClaimProcs.GetWriteOffEstimateDisplay(ClaimProcsForProc[i]));
				}
				else {
					row.Cells.Add(ClaimProcsForProc[i].InsPayAmt.ToString("n"));
					row.Cells.Add(ClaimProcsForProc[i].WriteOff.ToString("n"));
				}
				row.Cells.Add(ClaimProcsForProc[i].EstimateNote);
				row.Cells.Add(ClaimProcsForProc[i].Remarks);			  
				gridIns.Rows.Add(row);
			}
			gridIns.EndUpdate();
			if(ClaimProcsForProc.Count==0) {
				checkNoBillIns.CheckState=CheckState.Unchecked;
			}
			else if(allNoBillIns) {
				checkNoBillIns.CheckState=CheckState.Checked;
			}
		}

		private void gridIns_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormClaimProc FormC=new FormClaimProc(ClaimProcsForProc[e.Row],ProcCur,FamCur,PatCur,PlanList,HistList,ref LoopList,PatPlanList,true,SubList);
			if(ProcCur.IsLocked || !Procedures.IsProcComplEditAuthorized(ProcCur)) {
				FormC.NoPermissionProc=true;
			}
			FormC.ShowDialog();
			FillIns();
		}

		void butNow_Click(object sender,EventArgs e) {
			if(textTimeStart.Text.Trim()=="") {
				textTimeStart.Text=MiscData.GetNowDateTime().ToShortTimeString();
			}
			else {
				textTimeEnd.Text=MiscData.GetNowDateTime().ToShortTimeString();
			}
		}

		private void butAddEstimate_Click(object sender, System.EventArgs e) {
			if(ProcCur.ProcNumLab!=0) {
				MsgBox.Show(this,"Estimates cannot be added directly to labs.  Lab estimates will be created automatically when the parent procedure estimates are calculated.");
				return;
			}
			if(!Security.IsAuthorized(Permissions.InsWriteOffEdit,ProcCur.DateEntryC)) {
				return;
			}
			FormInsPlanSelect FormIS=new FormInsPlanSelect(PatCur.PatNum);
			if(FormIS.SelectedPlan==null) {
				FormIS.ShowDialog();
				if(FormIS.DialogResult==DialogResult.Cancel){
					return;
				}
			}
			InsPlan plan=FormIS.SelectedPlan;
			InsSub sub=FormIS.SelectedSub;
			ClaimProcsForProc=ClaimProcs.RefreshForProc(ProcCur.ProcNum);
			ClaimProc claimProcForProcInsPlan=ClaimProcsForProc
				.Where(x => x.PlanNum == plan.PlanNum)
				.Where(x => x.Status != ClaimProcStatus.Preauth)
				.FirstOrDefault();
			ClaimProc cp = new ClaimProc();
			cp.IsNew=true;
			if(claimProcForProcInsPlan!=null) {
				cp = claimProcForProcInsPlan;
				cp.IsNew=false;
			}
			else {
				List<Benefit> benList = Benefits.Refresh(PatPlanList,SubList);
				ClaimProcs.CreateEst(cp,ProcCur,plan,sub);
				if(plan.PlanType=="c") {//capitation
					double allowed = PIn.Double(textProcFee.Text);
					cp.BaseEst=allowed;
					cp.InsEstTotal=allowed;
					cp.CopayAmt=InsPlans.GetCopay(ProcCur.CodeNum,plan.FeeSched,plan.CopayFeeSched,!SubstitutionLinks.HasSubstCodeForPlan(plan,ProcCur.CodeNum,ListSubLinks),ProcCur.ToothNum,ProcCur.ClinicNum,ProcCur.ProvNum);
					if(cp.CopayAmt > allowed) {//if the copay is greater than the allowed fee calculated above
						cp.CopayAmt=allowed;//reduce the copay
					}
					if(cp.CopayAmt==-1) {
						cp.CopayAmt=0;
					}
					cp.WriteOffEst=cp.BaseEst-cp.CopayAmt;
					if(cp.WriteOffEst<0) {
						cp.WriteOffEst=0;
					}
					cp.WriteOff=cp.WriteOffEst;
					ClaimProcs.Update(cp);
				}
				long patPlanNum = PatPlans.GetPatPlanNum(sub.InsSubNum,PatPlanList);
				if(patPlanNum > 0) {
					double paidOtherInsTotal = ClaimProcs.GetPaidOtherInsTotal(cp,PatPlanList);
					double writeOffOtherIns = ClaimProcs.GetWriteOffOtherIns(cp,PatPlanList);
					ClaimProcs.ComputeBaseEst(cp,ProcCur,plan,patPlanNum,benList,
						HistList,LoopList,PatPlanList,paidOtherInsTotal,paidOtherInsTotal,PatCur.Age,writeOffOtherIns,PlanList,SubList,ListSubLinks);
				}
			}
			FormClaimProc FormC=new FormClaimProc(cp,ProcCur,FamCur,PatCur,PlanList,HistList,ref LoopList,PatPlanList,true,SubList);
			//FormC.NoPermission not needed because butAddEstimate not enabled
			FormC.ShowDialog();
			if(FormC.DialogResult==DialogResult.Cancel && cp.IsNew){
				ClaimProcs.Delete(cp);
			}
			FillIns();
		}

		private void FillPayments(bool doRefreshData=true){
			if(doRefreshData) {
				_loadData.ArrPaySplits=PaySplits.Refresh(ProcCur.PatNum);
				List<long> listPayNums=_loadData.ArrPaySplits.Where(x => x.ProcNum==ProcCur.ProcNum).Select(x => x.PayNum).ToList();
				_loadData.ListPaymentsForProc=Payments.GetPayments(listPayNums);
			}
			PaymentsForProc=_loadData.ListPaymentsForProc;
			PaySplitsForProc=PaySplits.GetForProc(ProcCur.ProcNum,_loadData.ArrPaySplits);
			gridPay.BeginUpdate();
			gridPay.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableProcPay","Entry Date"),70,HorizontalAlignment.Center);
			gridPay.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcPay","Amount"),55,HorizontalAlignment.Right);
			gridPay.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcPay","Tot Amt"),55,HorizontalAlignment.Right);
			gridPay.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcPay","Note"),250,HorizontalAlignment.Left);
			gridPay.Columns.Add(col);
			gridPay.Rows.Clear();
			ODGridRow row;
			Payment PaymentCur;//used in loop
			for(int i=0;i<PaySplitsForProc.Count;i++){
				row=new ODGridRow();
				row.Cells.Add(((PaySplit)PaySplitsForProc[i]).DatePay.ToShortDateString());
				row.Cells.Add(((PaySplit)PaySplitsForProc[i]).SplitAmt.ToString("F"));
				row.Cells[row.Cells.Count-1].Bold=YN.Yes;
				PaymentCur=Payments.GetFromList(((PaySplit)PaySplitsForProc[i]).PayNum,PaymentsForProc);
				row.Cells.Add(PaymentCur.PayAmt.ToString("F"));
				row.Cells.Add(PaymentCur.PayNote);
				gridPay.Rows.Add(row);
			}
			gridPay.EndUpdate();
		}

		private void gridPay_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Payment PaymentCur=Payments.GetFromList(((PaySplit)PaySplitsForProc[e.Row]).PayNum,PaymentsForProc);
			FormPayment FormP=new FormPayment(PatCur,FamCur,PaymentCur,false);
			FormP.InitialPaySplitNum=((PaySplit)PaySplitsForProc[e.Row]).SplitNum;
			FormP.ShowDialog();
			FillPayments();
		}

		private void FillAdj(){
			Adjustment[] AdjustmentList=_loadData.ArrAdjustments;
			AdjustmentsForProc=Adjustments.GetForProc(ProcCur.ProcNum,AdjustmentList);
			gridAdj.BeginUpdate();
			gridAdj.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableProcAdj","Entry Date"),70,HorizontalAlignment.Center);
			gridAdj.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcAdj","Amount"),55,HorizontalAlignment.Right);
			gridAdj.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcAdj","Type"),100,HorizontalAlignment.Left);
			gridAdj.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProcAdj","Note"),250,HorizontalAlignment.Left);
			gridAdj.Columns.Add(col);
			gridAdj.Rows.Clear();
			ODGridRow row;
			double discountAmt=0;//Total discount amount from all adjustments of default type.
			for(int i=0;i<AdjustmentsForProc.Count;i++){
				row=new ODGridRow();
				Adjustment adjustment=(Adjustment)AdjustmentsForProc[i];
				row.Cells.Add(adjustment.AdjDate.ToShortDateString());
				row.Cells.Add(adjustment.AdjAmt.ToString("F"));
				row.Cells[row.Cells.Count-1].Bold=YN.Yes;
				row.Cells.Add(Defs.GetName(DefCat.AdjTypes,adjustment.AdjType));
				row.Cells.Add(adjustment.AdjNote);
				gridAdj.Rows.Add(row);
				if(adjustment.AdjType==PrefC.GetLong(PrefName.TreatPlanDiscountAdjustmentType)) {
					discountAmt-=adjustment.AdjAmt;//Discounts are stored as negatives, we want a positive discount value.
				}
			}
			gridAdj.EndUpdate();
			//Because we keep the discount field in sync with the discount adjustment when the procedure has a status of TP,
			//we considered it a bug that the opposite didn't happen once the procedure was set complete.
			if(ProcCur.ProcStatus==ProcStat.C) {
				//Updating the discount text box will cause the procedure to get updated if the user clicks OK.
				//This is fine because the Discount column is not designed for accuracy (after being set complete) and is loosely kept updated.
				textDiscount.Text=discountAmt.ToString("F");//Calculated based on all adjustments of type if complete
			}
		}

		private void gridAdj_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormAdjust FormA=new FormAdjust(PatCur,(Adjustment)AdjustmentsForProc[e.Row]);
			if(FormA.ShowDialog()!=DialogResult.OK) {
				return;
			}
			_loadData.ArrAdjustments=Adjustments.GetForProcs(new List<long>() { ProcCur.ProcNum }).ToArray();
			FillAdj();
		}

		private void butAddAdjust_Click(object sender, System.EventArgs e) {
			if(ProcCur.ProcStatus!=ProcStat.C){
				MsgBox.Show(this,"Adjustments may only be added to completed procedures.");
				return;
			}
			bool isTsiAdj=(TsiTransLogs.IsTransworldEnabled(PatCur.ClinicNum)
				&& Patients.IsGuarCollections(PatCur.Guarantor)
				&& MsgBox.Show(this,MsgBoxButtons.YesNo,"The guarantor of this family has been sent to Transworld for collection.\r\n"
					+"Is this an adjustment due to a payment received from Transworld?"));
			Adjustment adj=new Adjustment();
			adj.PatNum=PatCur.PatNum;
			adj.ProvNum=_selectedProvNum;
			adj.DateEntry=DateTime.Today;//but will get overwritten to server date
			adj.AdjDate=DateTime.Today;
			adj.ProcDate=ProcCur.ProcDate;
			adj.ProcNum=ProcCur.ProcNum;
			adj.ClinicNum=ProcCur.ClinicNum;
			FormAdjust FormA=new FormAdjust(PatCur,adj,isTsiAdj);
			FormA.IsNew=true;
			if(FormA.ShowDialog()!=DialogResult.OK) {
				return;
			}
			_loadData.ArrAdjustments=Adjustments.GetForProcs(new List<long>() { ProcCur.ProcNum }).ToArray();
			FillAdj();
		}

		private void butAddExistAdj_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AdjustmentEdit)) {
				return;
			}
			if(ProcCur.ProcStatus!=ProcStat.C){
				MsgBox.Show(this,"Adjustments may only be added to completed procedures.");
				return;
			}
			FormAdjustmentPicker FormAP=new FormAdjustmentPicker(PatCur.PatNum,true);
			if(FormAP.ShowDialog()!=DialogResult.OK) {
				return;
			}
			if(!Security.IsAuthorized(Permissions.AdjustmentEdit,FormAP.SelectedAdjustment.AdjDate)) {
				return;
			}
			FormAP.SelectedAdjustment.ProcNum=ProcCur.ProcNum;
			FormAP.SelectedAdjustment.ProcDate=ProcCur.ProcDate;
			Adjustments.Update(FormAP.SelectedAdjustment);
			_loadData.ArrAdjustments=Adjustments.GetForProcs(new List<long>() { ProcCur.ProcNum }).ToArray();
			FillAdj();
		}

		private void textProcFee_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			if(textProcFee.errorProvider1.GetError(textProcFee)!=""){
				return;
			}
			double procFee;
			if(textProcFee.Text==""){
				procFee=0;
			}
			else{
				procFee=PIn.Double(textProcFee.Text);
			}
			if(ProcCur.ProcFee==procFee){
				return;
			}
			ProcCur.ProcFee=procFee;
			_isEstimateRecompute=true;
			Procedures.ComputeEstimates(ProcCur,PatCur.PatNum,ClaimProcsForProc,false,PlanList,PatPlanList,BenefitList,PatCur.Age,SubList);
			FillIns();
		}

		private void textTooth_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			textTooth.Text=textTooth.Text.ToUpper();
			if(!Tooth.IsValidEntry(textTooth.Text))
				errorProvider2.SetError(textTooth,Lan.g(this,"Invalid tooth number."));
			else
				errorProvider2.SetError(textTooth,"");
		}

		private void textSurfaces_TextChanged(object sender, System.EventArgs e) {
			int cursorPos = textSurfaces.SelectionStart;
			textSurfaces.Text=textSurfaces.Text.ToUpper();
			textSurfaces.SelectionStart=cursorPos;
		}

		private void textSurfaces_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
			if(Tooth.IsValidEntry(textTooth.Text)){
				textSurfaces.Text=Tooth.SurfTidyForDisplay(textSurfaces.Text,Tooth.FromInternat(textTooth.Text));
			}
			else{
				textSurfaces.Text=Tooth.SurfTidyForDisplay(textSurfaces.Text,"");
			}
			if(textSurfaces.Text=="")
				errorProvider2.SetError(textSurfaces,"No surfaces selected.");
			else
				errorProvider2.SetError(textSurfaces,"");
		}

		private void groupSextant_Validating(object sender,CancelEventArgs e) {
			if(IsSextantSelected()) {
				errorProvider2.SetError(groupSextant,"");
			}
			else {
				errorProvider2.SetError(groupSextant,Lan.g(this,"Please select a sextant treatment area."));
			}
		}

		private bool IsSextantSelected() {
			return groupSextant.Controls.OfType<RadioButton>().Any(x => x.Checked);
		}

		private void groupArch_Validating(object sender,CancelEventArgs e) {
			if(IsArchSelected()) {
				errorProvider2.SetError(groupArch,"");
			}
			else {
				errorProvider2.SetError(groupArch,Lan.g(this,"Please select a arch treatment area."));
			}
		}

		private bool IsArchSelected() {
			return groupArch.Controls.OfType<RadioButton>().Any(x => x.Checked);
		}

		private void listBoxTeeth2_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
		  listBoxTeeth.SelectedIndex=-1;
		}

		private void listBoxTeeth_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
		  listBoxTeeth2.SelectedIndex=-1;
		}

		private void butChange_Click(object sender, System.EventArgs e) {
			FormProcCodes FormP=new FormProcCodes();
      FormP.IsSelectionMode=true;
      FormP.ShowDialog();
      if(FormP.DialogResult!=DialogResult.OK){
				return;
			}
			ProcedureCode procCodeOld=ProcedureCodes.GetProcCode(ProcCur.CodeNum);
			ProcedureCode procCodeNew=ProcedureCodes.GetProcCode(FormP.SelectedCodeNum);
			if(procCodeOld.TreatArea != procCodeNew.TreatArea) {
				MsgBox.Show(this,"Not allowed due to treatment area mismatch.");
				return;
			}
      ProcCur.CodeNum=FormP.SelectedCodeNum;
      ProcedureCode2=ProcedureCodes.GetProcCode(FormP.SelectedCodeNum);
      textDesc.Text=ProcedureCode2.Descript;
			long priSubNum=PatPlans.GetInsSubNum(PatPlanList,1);
			InsSub prisub=InsSubs.GetSub(priSubNum,SubList);//can handle an inssubnum=0
			//long priPlanNum=PatPlans.GetPlanNum(PatPlanList,1);
			InsPlan priplan=InsPlans.GetPlan(prisub.PlanNum,PlanList);//can handle a plannum=0
			ProcCur.ProcFee=Fees.GetAmount0(ProcCur.CodeNum,FeeScheds.GetFeeSched(PatCur,PlanList,PatPlanList,SubList,ProcCur.ProvNum),ProcCur.ClinicNum,ProcCur.ProvNum);
			if(priplan!=null && priplan.PlanType=="p") {//PPO
				double standardfee=Fees.GetAmount0(ProcCur.CodeNum,Providers.GetProv(Patients.GetProvNum(PatCur)).FeeSched,ProcCur.ClinicNum,ProcCur.ProvNum);
				ProcCur.ProcFee=Math.Max(ProcCur.ProcFee,standardfee);
			}
			switch(ProcedureCode2.TreatArea){ 
				case TreatmentArea.Quad:
					ProcCur.Surf="UR";
					radioUR.Checked=true;
					break;
				case TreatmentArea.Sextant:
					ProcCur.Surf="1";
					radioS1.Checked=true;
					break;
				case TreatmentArea.Arch:
					ProcCur.Surf="U";
					radioU.Checked=true;
					break;
			}
			for(int i=0;i<ClaimProcsForProc.Count;i++) {
				if(ClaimProcsForProc[i].ClaimPaymentNum!=0) {
					continue;//this shouldn't be possible, but it's a good check to make.
				}
				ClaimProcs.Delete(ClaimProcsForProc[i]);//that way, completely new ones will be added back, and NoBillIns will be accurate.
			}
			ClaimProcsForProc=new List<ClaimProc>();
			Procedures.ComputeEstimates(ProcCur,PatCur.PatNum,ClaimProcsForProc,false,PlanList,PatPlanList,BenefitList,PatCur.Age,SubList);
			FillIns();
      SetControlsUpperLeft();
		}

		private void butEditAnyway_Click(object sender, System.EventArgs e) {
			DateTime dateOldestClaim=Procedures.GetOldestClaimDate(ClaimProcsForProc);
			if(!Security.IsAuthorized(Permissions.ClaimSentEdit,dateOldestClaim)) {
				return;
			}
			panel1.Enabled=true;
			comboProcStatus.Enabled=true;
			checkNoBillIns.Enabled=true;
			butDelete.Enabled=true;
			butSetComplete.Enabled=true;
			SetControlsEnabled(true);//enables/disables controls based on whether or not the user has permission (limited and/or full) to edit completed procs
			//Disable controls that may have been overzealously enabled.
			textTooth.BackColor=SystemColors.Control;
			textTooth.ReadOnly=true;
			textSurfaces.BackColor=SystemColors.Control;
			textSurfaces.ReadOnly=true;
			butAddEstimate.Enabled=true;
			radioL.Enabled=false;
			radioU.Enabled=false;
			radioLL.Enabled=false;
			radioLR.Enabled=false;
			radioUL.Enabled=false;
			radioUR.Enabled=false;
			radioS1.Enabled=false;
			radioS2.Enabled=false;
			radioS3.Enabled=false;
			radioS4.Enabled=false;
			radioS5.Enabled=false;
			radioS6.Enabled=false;
			listBoxTeeth.Enabled=false;
			listBoxTeeth2.Enabled=false;
			panelSurfaces.Enabled=false;
			//butChange.Enabled=true;//No. We no longer allow this because part of "change" is to delete all the claimprocs.  This is a terrible idea for a completed proc attached to a claim.
			//checkIsCovIns.Enabled=true;
		}

		private void listProcStatus_Click(object sender,EventArgs e) {
			
		}

		private void comboProcStatus_SelectionChangeCommitted(object sender,EventArgs e) {
			//status cannot be changed for completed procedures attached to a claim, except we allow changing status for preauths.
			//cannot edit status for TPi procedures.
			if(Procedures.IsAttachedToClaim(ProcOld,ClaimProcsForProc,false) && ProcOld.ProcStatus==ProcStat.C) {
				MsgBox.Show(this,"This is a completed procedure that is attached to a claim.  You must remove the procedure from the claim"+
					" or delete the claim before editing the status.");
				comboProcStatus.SelectedIndex=1;//Complete
				return;
			}
			if(comboProcStatus.SelectedIndex==0) {//fee starts out 0 if EO, EC, etc.  This updates fee if changing to TP so it won't stay 0.
				ProcCur.ProcStatus=ProcStat.TP;
				if(ProcCur.ProcFee==0) {
					ProcCur.ProcFee=Procedures.GetProcFee(PatCur,PatPlanList,SubList,PlanList,ProcCur.CodeNum,ProcCur.ProvNum,ProcCur.ClinicNum,
						ProcCur.MedicalCode);
					textProcFee.Text=ProcCur.ProcFee.ToString("f");
				}
			}
			if(comboProcStatus.SelectedIndex==1) {//C
				bool isAllowedToCompl=true;
				if(!PrefC.GetBool(PrefName.AllowSettingProcsComplete)) {
					MsgBox.Show(this,"Set the procedure complete by setting the appointment complete.  "
						+"If you want to be able to set procedures complete, you must turn on that option in Setup | Chart | Chart Preferences.");
					isAllowedToCompl=false;
				}
				//else if so that we don't give multiple notifications to the user.
				else if(!Security.IsAuthorized(Permissions.ProcComplCreate,PIn.Date(textDate.Text),ProcCur.CodeNum,PIn.Double(textProcFee.Text))) {
					isAllowedToCompl=false;
				}
				//Check to see if the user is allowed to set the procedure complete.
				if(!isAllowedToCompl) {
					//User not allowed to complete procedures so set it back to whatever it was before
					if(ProcCur.ProcStatus==ProcStat.TP) {
						comboProcStatus.SelectedIndex=0;
					}
					else if(PrefC.GetBool(PrefName.EasyHideClinical)) {
						comboProcStatus.SelectedIndex=-1;//original status must not be visible
					}
					else {
						if(ProcCur.ProcStatus==ProcStat.EC) {
							comboProcStatus.SelectedIndex=2;
						}
						if(ProcCur.ProcStatus==ProcStat.EO) {
							comboProcStatus.SelectedIndex=3;
						}
						if(ProcCur.ProcStatus==ProcStat.R) {
							comboProcStatus.SelectedIndex=4;
						}
						if(ProcCur.ProcStatus==ProcStat.Cn) {
							comboProcStatus.SelectedIndex=5;
						}
					}
					return;
				}
				if(ProcCur.AptNum!=0) {//if attached to an appointment
					Appointment apt=Appointments.GetOneApt(ProcCur.AptNum);
					if(apt.AptDateTime.Date > MiscData.GetNowDateTime().Date) {//if appointment is in the future
						MessageBox.Show(Lan.g(this,"Not allowed because procedure is attached to a future appointment with a date of ")
							+apt.AptDateTime.ToShortDateString());
						return;
					}
					if(apt.AptDateTime.Year<1880) {
						textDate.Text=MiscData.GetNowDateTime().ToShortDateString();
					}
					else {
						textDate.Text=apt.AptDateTime.ToShortDateString();
					}
				}
				else {
					textDate.Text=MiscData.GetNowDateTime().ToShortDateString();
				}
				//broken appointment procedure codes shouldn't trigger DateFirstVisit update.
				if(ProcedureCodes.GetStringProcCode(ProcCur.CodeNum)!="D9986" && ProcedureCodes.GetStringProcCode(ProcCur.CodeNum)!="D9987") {
					Procedures.SetDateFirstVisit(DateTimeOD.Today,2,PatCur);
				}
				ProcCur.ProcStatus=ProcStat.C;
				ProcNoteUiHelper();
			}
			if(comboProcStatus.SelectedIndex==2) {
				ProcCur.ProcStatus=ProcStat.EC;
			}
			if(comboProcStatus.SelectedIndex==3) {
				ProcCur.ProcStatus=ProcStat.EO;
			}
			if(comboProcStatus.SelectedIndex==4) {
				ProcCur.ProcStatus=ProcStat.R;
			}
			if(comboProcStatus.SelectedIndex==5) {
				ProcCur.ProcStatus=ProcStat.Cn;
			}
			//If it's already locked, there's simply no way to save the changes made to this control.
			//If status was just changed to C, then we should show the lock button.
			if(ProcCur.ProcStatus==ProcStat.C) {
				if(PrefC.GetBool(PrefName.ProcLockingIsAllowed) && !ProcCur.IsLocked) {
					butLock.Visible=true;
				}
			}
		}

		private void butSetComplete_Click(object sender, System.EventArgs e) {
			//can't get to here if attached to a claim, even if use the Edit Anyway button.
			if(ProcOld.ProcStatus==ProcStat.C){
				MsgBox.Show(this,"Procedure was already set complete.");
				return;
			}
			if(!PrefC.GetBool(PrefName.AllowSettingProcsComplete)) {
				MsgBox.Show(this,"Set the procedure complete by setting the appointment complete.  "
					+"If you want to be able to set procedures complete, you must turn on that option in Setup | Chart | Chart Preferences.");
				return;
			}
			if(!Security.IsAuthorized(Permissions.ProcComplCreate,PIn.Date(textDate.Text),ProcCur.CodeNum,PIn.Double(textProcFee.Text))) {
				return;
			}
			//If user is trying to change status to complete and using eCW.
			if((IsNew || ProcOld.ProcStatus!=ProcStat.C) && Programs.UsingEcwTightOrFullMode()) {
				MsgBox.Show(this,"Procedures cannot be set complete in this window.  Set the procedure complete by setting the appointment complete.");
				return;
			}
			//broken appointment procedure codes shouldn't trigger DateFirstVisit update.
			if(ProcedureCodes.GetStringProcCode(ProcCur.CodeNum)!="D9986" && ProcedureCodes.GetStringProcCode(ProcCur.CodeNum)!="D9987") {
				Procedures.SetDateFirstVisit(DateTimeOD.Today,2,PatCur);
			}
			if(ProcCur.AptNum!=0){//if attached to an appointment
				Appointment apt=Appointments.GetOneApt(ProcCur.AptNum);
				if(apt.AptDateTime.Date > MiscData.GetNowDateTime().Date){//if appointment is in the future
					MessageBox.Show(Lan.g(this,"Not allowed because procedure is attached to a future appointment with a date of ")
						+apt.AptDateTime.ToShortDateString());
					return;
				}
				if(apt.AptDateTime.Year<1880) {
					textDate.Text=MiscData.GetNowDateTime().ToShortDateString();
				}
				else {
					textDate.Text=apt.AptDateTime.ToShortDateString();
				}
			}
			else{
				textDate.Text=MiscData.GetNowDateTime().ToShortDateString();
			}
			if(ProcedureCode2.PaintType==ToothPaintingType.Extraction){//if an extraction, then mark previous procs hidden
				//Procedures.SetHideGraphical(ProcCur);//might not matter anymore
				ToothInitials.SetValue(ProcCur.PatNum,ProcCur.ToothNum,ToothInitialType.Missing);
			}
			ProcNoteUiHelper();
			Plugins.HookAddCode(this,"FormProcEdit.butSetComplete_Click_end",ProcCur,ProcOld,textNotes);
			comboProcStatus.SelectedIndex=-1;
			ProcCur.ProcStatus=ProcStat.C;
			ProcCur.SiteNum=PatCur.SiteNum;
			comboPlaceService.SelectedIndex=PrefC.GetInt(PrefName.DefaultProcedurePlaceService);
			if(EntriesAreValid()){
				SaveAndClose();
			}
		}

		///<summary>Sets the UI textNotes.Text to the default proc note if any.
		///Also checks PrefName.ProcPromptForAutoNote and remots auto notes if needed.</summary>
		private void ProcNoteUiHelper() {
			string procNoteDefault="";
			if(_isQuickAdd) {//Quick Procs should insert both TP Default Note and C Default Note.
				procNoteDefault=ProcCodeNotes.GetNote(_selectedProvNum,ProcCur.CodeNum,ProcStat.TP);
				if(!string.IsNullOrEmpty(procNoteDefault)) {
					procNoteDefault+="\r\n";
				}
			}
			procNoteDefault+=ProcCodeNotes.GetNote(_selectedProvNum,ProcCur.CodeNum,ProcStat.C);
			if(textNotes.Text!="" && procNoteDefault!="") { //check to see if a default note is defined.
				textNotes.Text+="\r\n"; //add a new line if there was already a ProcNote on the procedure.
			}
			if(!string.IsNullOrEmpty(procNoteDefault)) {
				textNotes.Text+=procNoteDefault;
			}
			if(!PrefC.GetBool(PrefName.ProcPromptForAutoNote)) {
				//Users do not want to be prompted for auto notes, so remove them all from the procedure note.
				textNotes.Text=Regex.Replace(textNotes.Text,@"\[\[.+?\]\]","");
			}
		}

		private void radioUR_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="UR";
		}

		private void radioUL_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="UL";
		}

		private void radioLR_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="LR";
		}

		private void radioLL_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="LL";
		}

		private void radioU_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="U";
			errorProvider2.SetError(groupArch,"");
		}

		private void radioL_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="L";
			errorProvider2.SetError(groupArch,"");
		}

		private void radioS1_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="1";
			errorProvider2.SetError(groupSextant,"");
		}

		private void radioS2_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="2";
			errorProvider2.SetError(groupSextant,"");
		}

		private void radioS3_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="3";
			errorProvider2.SetError(groupSextant,"");
		}

		private void radioS4_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="4";
			errorProvider2.SetError(groupSextant,"");
		}

		private void radioS5_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="5";
			errorProvider2.SetError(groupSextant,"");
		}

		private void radioS6_Click(object sender, System.EventArgs e) {
			ProcCur.Surf="6";
			errorProvider2.SetError(groupSextant,"");
		}

		private void checkNoBillIns_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.InsWriteOffEdit,ProcCur.DateEntryC)) {
				checkNoBillIns.CheckState=checkNoBillIns.Checked ? CheckState.Unchecked : CheckState.Checked;
				return;
			}
			if(checkNoBillIns.CheckState==CheckState.Indeterminate){
				//not allowed to set to indeterminate, so move on
				checkNoBillIns.CheckState=CheckState.Unchecked;
			}
			for(int i=0;i<ClaimProcsForProc.Count;i++) {
				//ignore CapClaim,NotReceived,PreAuth,Received,Supplemental
				if(ClaimProcsForProc[i].Status==ClaimProcStatus.Estimate
					|| ClaimProcsForProc[i].Status==ClaimProcStatus.CapComplete
					|| ClaimProcsForProc[i].Status==ClaimProcStatus.CapEstimate)
				{
					if(checkNoBillIns.CheckState==CheckState.Checked){
						ClaimProcsForProc[i].NoBillIns=true;
					}
					else{//unchecked
						ClaimProcsForProc[i].NoBillIns=false;
					}
					ClaimProcs.Update(ClaimProcsForProc[i]);
				}
			}
			//next line is needed to recalc BaseEst, etc, for claimprocs that are no longer NoBillIns
			//also, if they are NoBillIns, then it clears out the other values.
			Procedures.ComputeEstimates(ProcCur,PatCur.PatNum,ClaimProcsForProc,false,PlanList,PatPlanList,BenefitList,PatCur.Age,SubList);
			FillIns();
		}

		private void textNotes_TextChanged(object sender, System.EventArgs e) {
			CheckForCompleteNote();
			if(!IsStartingUp//so this happens only if user changes the note
				&& !SigChanged)//and the original signature is still showing.
			{
				signatureBoxWrapper.ClearSignature();
				//this will call OnSignatureChanged to set UserNum, textUser, and SigChanged
			}
		}

		private void CheckForCompleteNote(){
			if(textNotes.Text.IndexOf("\"\"")==-1){
				//no occurances of ""
				labelIncomplete.Visible=false;
			}
			else{
				labelIncomplete.Visible=true;
			}
		}

		private void butSearch_Click(object sender,EventArgs e) {
			InputBox input=new InputBox(Lan.g(this,"Search for"));
			input.ShowDialog();
			if(input.DialogResult!=DialogResult.OK) {
				return;
			}
			string searchText=input.textResult.Text;
			int index=textNotes.Find(input.textResult.Text);//Gets the location of the first character in the control.
			if(index<0) {//-1 is returned when the text is not found.
				textNotes.DeselectAll();
				MessageBox.Show("\""+searchText+"\"\r\n"+Lan.g(this,"was not found in the notes")+".");
				return;
			}
			textNotes.Select(index,searchText.Length);
		}

		private void signatureBoxWrapper_SignatureChanged(object sender,EventArgs e) {
			SigChanged=true;
			ProcCur.UserNum=_curUser.UserNum;
			textUser.Text=_curUser.UserName;
		}

		private void buttonUseAutoNote_Click(object sender,EventArgs e) {
			FormAutoNoteCompose FormA=new FormAutoNoteCompose();
			FormA.ShowDialog();
			if(FormA.DialogResult==DialogResult.OK) {
				textNotes.AppendText(FormA.CompletedNote);
				bool hasAutoNotePrompt=Regex.IsMatch(textNotes.Text,_autoNotePromptRegex);
				butEditAutoNote.Visible=hasAutoNotePrompt;
			}
		}

		private void butEditAutoNote_Click(object sender,EventArgs e) {
			if(Regex.IsMatch(textNotes.Text,_autoNotePromptRegex)) {
				FormAutoNoteCompose FormA=new FormAutoNoteCompose();
				FormA.MainTextNote=textNotes.Text;
				FormA.ShowDialog();
				if(FormA.DialogResult==DialogResult.OK) {
					textNotes.Text=FormA.CompletedNote;
					bool hasAutoNotePrompt=Regex.IsMatch(textNotes.Text,_autoNotePromptRegex);
					butEditAutoNote.Visible=hasAutoNotePrompt;
				}
			}
			else {
				MessageBox.Show(Lan.g(this,"No Auto Note available to edit."));
			}
		}

		/*private void butShowMedical_Click(object sender,EventArgs e) {
			if(groupMedical.Height<200) {
				groupMedical.Height=200;
				butShowMedical.Text="^";
			}
			else {
				groupMedical.Height=170;
				butShowMedical.Text="V";
			}
		}*/

		private void comboDPC_SelectionChangeCommitted(object sender,EventArgs e) {
			DateTime tempDate=PIn.Date(textDateTP.Text);
			switch(comboDPC.SelectedIndex) {
				case 2:
					tempDate=tempDate.Date.AddDays(1);
					if(CancelledScheduleByDate.Year>1880 && CancelledScheduleByDate<tempDate) {
						tempDate=CancelledScheduleByDate;
					}
					break;
				case 3:
					tempDate=tempDate.Date.AddDays(30);
					if(CancelledScheduleByDate.Year>1880 && CancelledScheduleByDate<tempDate) {
						tempDate=CancelledScheduleByDate;
					}
					break;
				case 4:
					tempDate=tempDate.Date.AddDays(60);
					if(CancelledScheduleByDate.Year>1880 && CancelledScheduleByDate<tempDate) {
						tempDate=CancelledScheduleByDate;
					}
					break;
				case 5:
					tempDate=tempDate.Date.AddDays(120);
					if(CancelledScheduleByDate.Year>1880 && CancelledScheduleByDate<tempDate) {
						tempDate=CancelledScheduleByDate;
					}
					break;
				case 6:
					tempDate=tempDate.Date.AddYears(1);
					if(CancelledScheduleByDate.Year>1880 && CancelledScheduleByDate<tempDate) {
						tempDate=CancelledScheduleByDate;
					}
					break;
			}
			textDateScheduled.Text=tempDate.ToShortDateString();
			labelScheduleBy.Visible=false;
			if(comboDPC.SelectedIndex==0
				|| comboDPC.SelectedIndex==1
				|| comboDPC.SelectedIndex==7
				|| comboDPC.SelectedIndex==8) {
				textDateScheduled.Text="";
				labelScheduleBy.Visible=true;
			}
		}

		private void comboStatus_SelectionChangeCommitted(object sender,EventArgs e) {
			switch(comboStatus.SelectedIndex) {
				case 0:
					comboProcStatus.SelectedIndex=0;
					ProcCur.ProcStatus=ProcStat.TP;
					break;
				case 1:
					if(!Security.IsAuthorized(Permissions.ProcComplCreate,PIn.Date(textDate.Text),ProcCur.CodeNum,PIn.Double(textProcFee.Text))) {
						//set it back to whatever it was before
						if(OrionProcCur.Status2==OrionStatus.TP) {
							comboStatus.SelectedIndex=0;
						}
						if(OrionProcCur.Status2==OrionStatus.E) {
							comboStatus.SelectedIndex=2;
						}
						if(OrionProcCur.Status2==OrionStatus.R) {
							comboStatus.SelectedIndex=3;
						}
						if(OrionProcCur.Status2==OrionStatus.RO) {
							comboStatus.SelectedIndex=4;
						}
						if(OrionProcCur.Status2==OrionStatus.CS) {
							comboStatus.SelectedIndex=5;
						}
						if(OrionProcCur.Status2==OrionStatus.CR) {
							comboStatus.SelectedIndex=6;
						}
						if(OrionProcCur.Status2==OrionStatus.CA_Tx) {
							comboStatus.SelectedIndex=7;
						}
						if(OrionProcCur.Status2==OrionStatus.CA_EPRD) {
							comboStatus.SelectedIndex=8;
						}
						if(OrionProcCur.Status2==OrionStatus.CA_PD) {
							comboStatus.SelectedIndex=9;
						}
						if(OrionProcCur.Status2==OrionStatus.S) {
							comboStatus.SelectedIndex=10;
						}
						if(OrionProcCur.Status2==OrionStatus.ST) {
							comboStatus.SelectedIndex=11;
						}
						if(OrionProcCur.Status2==OrionStatus.W) {
							comboStatus.SelectedIndex=12;
						}
						if(OrionProcCur.Status2==OrionStatus.A) {
							comboStatus.SelectedIndex=13;
						}
						return;
					}
					comboProcStatus.SelectedIndex=1;
					ProcCur.ProcStatus=ProcStat.C;
					textTimeStart.Text=MiscData.GetNowDateTime().ToShortTimeString();
					break;
				case 2:
					comboProcStatus.SelectedIndex=3;
					ProcCur.ProcStatus=ProcStat.EO;
					break;
				case 3:
					comboProcStatus.SelectedIndex=0;
					ProcCur.ProcStatus=ProcStat.TP;
					break;
				case 4:
					comboProcStatus.SelectedIndex=4;
					ProcCur.ProcStatus=ProcStat.R;
					break;
				case 5:
					comboProcStatus.SelectedIndex=3;
					ProcCur.ProcStatus=ProcStat.EO;
					break;
				case 6:
					comboProcStatus.SelectedIndex=3;
					ProcCur.ProcStatus=ProcStat.EO;
					break;
				case 7:
					comboProcStatus.SelectedIndex=0;
					ProcCur.ProcStatus=ProcStat.TP;
					break;
				case 8:
					comboProcStatus.SelectedIndex=0;
					ProcCur.ProcStatus=ProcStat.TP;
					break;
				case 9:
					comboProcStatus.SelectedIndex=0;
					ProcCur.ProcStatus=ProcStat.TP;
					break;
				case 10:
					comboProcStatus.SelectedIndex=0;
					ProcCur.ProcStatus=ProcStat.TP;
					break;
				case 11:
					comboProcStatus.SelectedIndex=0;
					ProcCur.ProcStatus=ProcStat.TP;
					break;
				case 12:
					comboProcStatus.SelectedIndex=5;
					ProcCur.ProcStatus=ProcStat.Cn;
					break;
				case 13:
					comboProcStatus.SelectedIndex=0;
					ProcCur.ProcStatus=ProcStat.TP;
					break;
			}
			OrionProcCur.Status2=(OrionStatus)((int)(Math.Pow(2d,(double)(comboStatus.SelectedIndex))));
			//Do not automatically set the stop clock date if status is set to treatment planned, existing, or watch.
			if(OrionProcCur.Status2==OrionStatus.TP || OrionProcCur.Status2==OrionStatus.E || OrionProcCur.Status2==OrionStatus.W) {
				//Clear the stop the clock date if there was no stop the clock date defined in a previous edit. Therefore, for a new procedure, always clear.
				if(OrionProcCur.DateStopClock.Year<1880){
					textDateStop.Text="";
				}
			}
			else {//Set the stop the clock date for all other statuses.
				//Use the previously set stop the clock date if one exists. Will never be true if this is a new procedure.
				if(OrionProcCur.DateStopClock.Year>1880) {
					textDateStop.Text=OrionProcCur.DateStopClock.ToShortDateString();
				}
				else {
					//When the stop the clock date has not already been set, set to the ProcDate for the procedure.
					textDateStop.Text=textDate.Text.Trim();
				}
			}
		}

		private void textTimeStart_TextChanged(object sender,EventArgs e) {
			UpdateFinalMin();			
		}

		private void textTimeEnd_TextChanged(object sender,EventArgs e) {
			UpdateFinalMin();
		}

		///<summary>Populates final time box with total number of minutes.</summary>
		private void UpdateFinalMin() {
			if(textTimeStart.Text=="" || textTimeEnd.Text=="") {
				return;
			}
			int startTime=0;
			int stopTime=0;
			try {
				startTime=PIn.Int(textTimeStart.Text);
			}
			catch { 
				try {//Try DateTime format.
					DateTime sTime=DateTime.Parse(textTimeStart.Text);
					startTime=(sTime.Hour*(int)Math.Pow(10,2))+sTime.Minute;
				}
				catch {//Not a valid time.
					return;
				}
			}
			try {
				stopTime=PIn.Int(textTimeEnd.Text);
			}
			catch { 
				try {//Try DateTime format.
					DateTime eTime=DateTime.Parse(textTimeEnd.Text);
					stopTime=(eTime.Hour*(int)Math.Pow(10,2))+eTime.Minute;
				}
				catch {//Not a valid time.
					return;
				}
			}
			int total=(((stopTime/100)*60)+(stopTime%100))-(((startTime/100)*60)+(startTime%100));
			textTimeFinal.Text=total.ToString();
		}

		///<summary>Empty string is considered valid.</summary>
		private bool ValidateTime(string time) {
			string militaryTime=time;
			if(militaryTime=="") {
				return true;
			}
			if(militaryTime.Length<4) {
				militaryTime=militaryTime.PadLeft(4,'0');
			}
			//Test if user typed in military time. Ex: 0830 or 1536
			try {
				int hour=PIn.Int(militaryTime.Substring(0,2));
				int minute=PIn.Int(militaryTime.Substring(2,2));
				if(hour>23) {
					return false;
				}
				if(minute>59) {
					return false;
				}
				return true;
			}
			catch { }
			//Test typical DateTime format. Ex: 1:00 PM
			try { 
				DateTime.Parse(time);
				return true;
			}
			catch { 
				return false;
			}
		}

		///<summary>Returns min value if blank or invalid string passed in.</summary>
		private DateTime ParseTime(string time) {
			string militaryTime=time;
			DateTime dTime=DateTime.MinValue;
			if(militaryTime=="") {
				return dTime;
			}
			if(militaryTime.Length<4) {
				militaryTime=militaryTime.PadLeft(4,'0');
			}
			//Test if user typed in military time. Ex: 0830 or 1536
			try {
				int hour=PIn.Int(militaryTime.Substring(0,2));
				int minute=PIn.Int(militaryTime.Substring(2,2));
				dTime=new DateTime(1,1,1,hour,minute,0);
				return dTime;
			}
			catch { }
			//Test if user typed in a typical DateTime format. Ex: 1:00 PM
			try { 
				return DateTime.Parse(time);
			}
			catch { }
			return dTime;
		}

		private void UpdateSurf() {
			if(!Tooth.IsValidEntry(textTooth.Text)){
				return;
			}
			errorProvider2.SetError(textSurfaces,"");
			textSurfaces.Text="";
			if(butM.BackColor==Color.White) {
				textSurfaces.AppendText("M");
			}
			if(butOI.BackColor==Color.White) {
				//if(ToothGraphic.IsAnterior(Tooth.FromInternat(textTooth.Text))) {
				if(Tooth.IsAnterior(Tooth.FromInternat(textTooth.Text))) {
					textSurfaces.AppendText("I");
				}
				else {
					textSurfaces.AppendText("O");
				}
			}
			if(butD.BackColor==Color.White) {
				textSurfaces.AppendText("D");
			}
			if(butV.BackColor==Color.White) {
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
					textSurfaces.AppendText("5");
				}
				else {
					textSurfaces.AppendText("V");
				}
			}
			if(butBF.BackColor==Color.White) {
				//if(ToothGraphic.IsAnterior(Tooth.FromInternat(textTooth.Text))) {
				if(Tooth.IsAnterior(Tooth.FromInternat(textTooth.Text))) {
					if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
						textSurfaces.AppendText("V");//vestibular
					}
					else {
						textSurfaces.AppendText("F");
					}
				}
				else {
					textSurfaces.AppendText("B");
				}
			}
			if(butL.BackColor==Color.White) {
				textSurfaces.AppendText("L");
			}
		}

		private void butM_Click(object sender,EventArgs e) {
			if(butM.BackColor==Color.White) {
				butM.BackColor=SystemColors.Control;
			}
			else {
				butM.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butOI_Click(object sender,EventArgs e) {
			if(butOI.BackColor==Color.White) {
				butOI.BackColor=SystemColors.Control;
			}
			else {
				butOI.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butL_Click(object sender,EventArgs e) {
			if(butL.BackColor==Color.White) {
				butL.BackColor=SystemColors.Control;
			}
			else {
				butL.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butV_Click(object sender,EventArgs e) {
			if(butV.BackColor==Color.White) {
				butV.BackColor=SystemColors.Control;
			}
			else {
				butV.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butBF_Click(object sender,EventArgs e) {
			if(butBF.BackColor==Color.White) {
				butBF.BackColor=SystemColors.Control;
			}
			else {
				butBF.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butD_Click(object sender,EventArgs e) {
			if(butD.BackColor==Color.White) {
				butD.BackColor=SystemColors.Control;
			}
			else {
				butD.BackColor=Color.White;
			}
			UpdateSurf();
		}

		private void butPickSite_Click(object sender,EventArgs e) {
			FormSites FormS=new FormSites();
			FormS.IsSelectionMode=true;
			FormS.SelectedSiteNum=ProcCur.SiteNum;
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK){
				return;
			}
			ProcCur.SiteNum=FormS.SelectedSiteNum;
			textSite.Text=Sites.GetDescription(ProcCur.SiteNum);
		}

		///<summary>This button is only visible if 1. Pref ProcLockingIsAllowed is true, 2. Proc isn't already locked, 3. Proc status is C.</summary>
		private void butLock_Click(object sender,EventArgs e) {
			if(!EntriesAreValid()) {
				return;
			}
			ProcCur.IsLocked=true;
			SaveAndClose();//saves all the other various changes that the user made
			DialogResult=DialogResult.OK;
		}

		///<summary>This button is only visible when proc IsLocked.</summary>
		private void butInvalidate_Click(object sender,EventArgs e) {
			Permissions perm=Permissions.ProcComplEdit;
			if(ProcCur.ProcStatus.In(ProcStat.EO,ProcStat.EC)) {
				perm=Permissions.ProcExistingEdit;
			}
			//What this will really do is "delete" the procedure.
			if(!Security.IsAuthorized(perm,ProcCur.ProcDate)) {
				return;
			}
			if(Procedures.IsAttachedToClaim(ProcCur,ClaimProcsForProc)) {
				MsgBox.Show(this,"This procedure is attached to a claim and cannot be invalidated without first deleting the claim.");
				return;
			}
			try {
				Procedures.Delete(ProcCur.ProcNum);//also deletes any claimprocs (other than ins payments of course).
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			SecurityLogs.MakeLogEntry(perm,PatCur.PatNum,Lan.g(this,"Invalidated: ")+
				ProcedureCodes.GetStringProcCode(ProcCur.CodeNum).ToString()+" ("+ProcCur.ProcStatus+"), "
				+ProcCur.ProcDate.ToShortDateString()+", "+ProcCur.ProcFee.ToString("c")+", Deleted");
			DialogResult=DialogResult.OK;
		}

		///<summary>This button is only visible when proc IsLocked.</summary>
		private void butAppend_Click(object sender,EventArgs e) {
			FormProcNoteAppend formPNA=new FormProcNoteAppend();
			formPNA.ProcCur=ProcCur;
			formPNA.ShowDialog();
			if(formPNA.DialogResult!=DialogResult.OK) {
				return;
			}
			DialogResult=DialogResult.OK;//exit out of this window.  Change already saved, and OK button is disabled in this window, anyway.
		}

		private void butSnomedBodySiteSelect_Click(object sender,EventArgs e) {
			FormSnomeds formS=new FormSnomeds();
			formS.IsSelectionMode=true;
			if(formS.ShowDialog()==DialogResult.OK) {
				_snomedBodySite=formS.SelectedSnomed;
				textSnomedBodySite.Text=_snomedBodySite.Description;
			}
		}

		private void butNoneSnomedBodySite_Click(object sender,EventArgs e) {
			_snomedBodySite=null;
			textSnomedBodySite.Text="";
		}

		private void SetIcdLabels() {
			byte icdVersion=9;
			if(checkIcdVersion.Checked) {
				icdVersion=10;
			}
			labelDiagnosticCode1.Text=Lan.g(this,"ICD")+"-"+icdVersion+" "+Lan.g(this,"Diagnosis Code 1");
			labelDiagnosticCode2.Text=Lan.g(this,"ICD")+"-"+icdVersion+" "+Lan.g(this,"Diagnosis Code 2");
			labelDiagnosticCode3.Text=Lan.g(this,"ICD")+"-"+icdVersion+" "+Lan.g(this,"Diagnosis Code 3");
			labelDiagnosticCode4.Text=Lan.g(this,"ICD")+"-"+icdVersion+" "+Lan.g(this,"Diagnosis Code 4");
		}

		private void checkIcdVersion_Click(object sender,EventArgs e) {
			SetIcdLabels();
		}

		private void PickDiagnosisCode(TextBox textBoxDiagnosisCode) {
			if(checkIcdVersion.Checked) {//ICD-10
				FormIcd10s formI=new FormIcd10s();
				formI.IsSelectionMode=true;
				if(formI.ShowDialog()==DialogResult.OK) {
					textBoxDiagnosisCode.Text=formI.SelectedIcd10.Icd10Code;
				}
			}
			else {//ICD-9
				FormIcd9s formI=new FormIcd9s();
				formI.IsSelectionMode=true;
				if(formI.ShowDialog()==DialogResult.OK) {
					textBoxDiagnosisCode.Text=formI.SelectedIcd9.ICD9Code;
				}
			}
		}

		private void butDiagnosisCode1_Click(object sender,EventArgs e) {
			PickDiagnosisCode(textDiagnosticCode);
		}

		private void butNoneDiagnosisCode1_Click(object sender,EventArgs e) {
			textDiagnosticCode.Text="";
		}

		private void butDiagnosisCode2_Click(object sender,EventArgs e) {
			PickDiagnosisCode(textDiagnosticCode2);
		}

		private void butNoneDiagnosisCode2_Click(object sender,EventArgs e) {
			textDiagnosticCode2.Text="";
		}

		private void butDiagnosisCode3_Click(object sender,EventArgs e) {
			PickDiagnosisCode(textDiagnosticCode3);
		}

		private void butNoneDiagnosisCode3_Click(object sender,EventArgs e) {
			textDiagnosticCode3.Text="";
		}

		private void butDiagnosisCode4_Click(object sender,EventArgs e) {
			PickDiagnosisCode(textDiagnosticCode4);
		}

		private void butNoneDiagnosisCode4_Click(object sender,EventArgs e) {
			textDiagnosticCode4.Text="";
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(IsNew) {
				DialogResult=DialogResult.Cancel;//verified that this triggers a delete when window closed from all places where FormProcEdit is used, and where proc could be new.
				return;
			}
			//If this is an existing completed proc, then this delete button is only enabled if the user has permission for ProcComplEdit based on the ProcDate.
			if(!ProcOld.ProcStatus.In(ProcStat.C,ProcStat.EO,ProcStat.EC)
				&& !Security.IsAuthorized(Permissions.ProcDelete,ProcCur.ProcDate)) //This should be a much more lenient permission since completed procedures are already safeguarded.
			{
				return;
			}
			if(!Procedures.IsProcComplEditAuthorized(ProcOld,true)) {
				return;
			}
			if(MessageBox.Show(Lan.g(this,"Delete Procedure?"),"",MessageBoxButtons.OKCancel)!=DialogResult.OK){
				return;
			}
			try{
				Procedures.Delete(ProcCur.ProcNum);//also deletes the claimProcs and adjustments. Might throw exception.
				_isEstimateRecompute=true;
				Recalls.Synch(ProcCur.PatNum);//needs to be moved into Procedures.Delete
				Permissions perm=Permissions.ProcDelete;
				string tag="";
				switch(ProcOld.ProcStatus) {
					case ProcStat.C:
						perm=Permissions.ProcComplEdit;
						tag=", "+Lan.g(this,"Deleted");
						break;
					case ProcStat.EO:
					case ProcStat.EC:
						perm=Permissions.ProcExistingEdit;
						tag=", "+Lan.g(this,"Deleted");
						break;
				}
				SecurityLogs.MakeLogEntry(perm,ProcOld.PatNum,
					ProcedureCodes.GetProcCode(ProcOld.CodeNum).ProcCode+" ("+ProcOld.ProcStatus+"), "+ProcOld.ProcFee.ToString("c")+tag);
				DialogResult=DialogResult.OK;
				Plugins.HookAddCode(this,"FormProcEdit.butDelete_Click_end",ProcCur);
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
			}
		}

		private bool EntriesAreValid(){
			if(  textDateTP.errorProvider1.GetError(textDateTP)!=""
				|| textDate.errorProvider1.GetError(textDate)!=""
				|| textProcFee.errorProvider1.GetError(textProcFee)!=""
				|| textDateOriginalProsth.errorProvider1.GetError(textDateOriginalProsth)!=""
				|| textDiscount.errorProvider1.GetError(textDiscount)!=""
				){
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return false;
			}
			if(textDate.Text==""){
				MessageBox.Show(Lan.g(this,"Please enter a date first."));
				return false;
			}
			//There have been 2 or 3 cases where a customer entered a note with thousands of new lines and when OD tries to display such a note in the chart, a GDI exception occurs because the progress notes grid is very tall and takes up too much video memory. To help prevent this issue, we block the user from entering any note where there are 50 or more consecutive new lines anywhere in the note. Any number of new lines less than 50 are considered to be intentional.
			StringBuilder tooManyNewLines=new StringBuilder();
			for(int i=0;i<50;i++) {
				tooManyNewLines.Append("\r\n");
			}
			if(textNotes.Text.Contains(tooManyNewLines.ToString())) {
				MsgBox.Show(this,"The notes contain 50 or more consecutive blank lines. Probably unintentional and must be fixed.");
				return false;
			}
			if(Programs.UsingOrion || PrefC.GetBool(PrefName.ShowFeatureMedicalInsurance)) {
				if(!ValidateTime(textTimeStart.Text)) {
					MessageBox.Show(Lan.g(this,"Start time is invalid."));
					return false;
				}
				if(!ValidateTime(textTimeEnd.Text)) {
					MessageBox.Show(Lan.g(this,"End time is invalid."));
					return false;
				}
			}
			else {
				if(textTimeStart.Text!="") {
					try {
						DateTime.Parse(textTimeStart.Text);
					}
					catch {
						MessageBox.Show(Lan.g(this,"Start time is invalid."));
						return false;
					}
				}
			}
			try{
				int unitqty=int.Parse(textUnitQty.Text);
				if(unitqty<1){
					MsgBox.Show(this,"Qty not valid.  Typical value is 1.");
					return false;
				}
			}
			catch{
				MsgBox.Show(this,"Qty not valid.  Typical value is 1.");
				return false;
			}
			if(_selectedProvNum==0){//_selectedProvNum gets changed on combo box change.
				MsgBox.Show(this,"You must select a provider first.");
				return false;
			}
			if(errorProvider2.GetError(textSurfaces)!=""
				|| errorProvider2.GetError(textTooth)!="")
			{
				MsgBox.Show(this,"Please fix tooth number or surfaces first.");
				return false;
			}
			if(errorProvider2.GetError(groupSextant)!=""
			 || errorProvider2.GetError(groupArch)!="") 
			{
				MsgBox.Show(this,"Please fix arch or sextant first.");
				return false;
			}
			if(textMedicalCode.Text!="" && !ProcedureCodes.GetContainsKey(textMedicalCode.Text)){
				MsgBox.Show(this,"Invalid medical code.  It must refer to an existing procedure code.");
				return false;
			}
			if(textDrugNDC.Text!=""){
				if(comboDrugUnit.SelectedIndex==(int)EnumProcDrugUnit.None || textDrugQty.Text==""){
					if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Drug quantity and unit are not entered.  Continue anyway?")){
						return false;
					}
				}
			}
			if(textDrugQty.Text!=""){
				try{
					float.Parse(textDrugQty.Text);
				}
				catch{
					MsgBox.Show(this,"Please fix drug qty first.");
					return false;
				}
			}
			//If user is trying to change status to complete and using eCW.
			if(ProcCur.ProcStatus==ProcStat.C && (IsNew || ProcOld.ProcStatus!=ProcStat.C) && Programs.UsingEcwTightOrFullMode()) {
				MsgBox.Show(this,"Procedures cannot be set complete in this window.  Set the procedure complete by setting the appointment complete.");
				return false;
			}
			if(ProcCur.ProcStatus==ProcStat.C && PIn.Date(textDate.Text) > DateTime.Today && !PrefC.GetBool(PrefName.FutureTransDatesAllowed)) {
				MsgBox.Show(this,"Completed procedures cannot have future dates.");
				return false;
			}
			if(ProcOld.ProcStatus!=ProcStat.C && ProcCur.ProcStatus==ProcStat.C){//if status was changed to complete
				if(ProcCur.AptNum!=0) {//if attached to an appointment
					Appointment apt=Appointments.GetOneApt(ProcCur.AptNum);
					if(apt.AptDateTime.Date > MiscData.GetNowDateTime().Date) {//if appointment is in the future
						MessageBox.Show(Lan.g(this,"Not allowed because procedure is attached to a future appointment with a date of ")
							+apt.AptDateTime.ToShortDateString());
						return false;
					}
					if(apt.AptDateTime.Year>=1880) {
						textDate.Text=apt.AptDateTime.ToShortDateString();
					}
				}
				if(!_isQuickAdd && !Security.IsAuthorized(Permissions.ProcComplCreate,PIn.Date(textDate.Text))){//use the new date
					return false;
				}
			}
			else if(!_isQuickAdd && IsNew && ProcCur.ProcStatus==ProcStat.C) {//if new procedure is complete
				if(!Security.IsAuthorized(Permissions.ProcComplCreate,PIn.Date(textDate.Text),ProcCur.CodeNum,PIn.Double(textProcFee.Text))){
					return false;
				}
			}
			else if(!IsNew){//an old procedure
				if(ProcOld.ProcStatus.In(ProcStat.C,ProcStat.EO,ProcStat.EC)) {//that was already complete
					//It's not possible for the user to get to this point unless they have permission for ProcComplEditLimited based on the DateEntryC.
					//The following 2 checks are not redundant because they check different dates.
					//ProcComplEditLimited does not check Global Lock date but checks the permission specific date. 
					DateTime dateToUseProcOld=ProcOld.ProcDate;
					if(ProcOld.ProcStatus.In(ProcStat.EC,ProcStat.EO)){
						dateToUseProcOld=DateTime.Today;//ignore date limitation for EO/EC procedures on Edit Completed Procedure (limited) permissions.
					}
					if(!Security.IsAuthorized(Permissions.ProcComplEditLimited,dateToUseProcOld)){//block old date
						return false;
					}
					if(ProcCur.ProcStatus.In(ProcStat.C,ProcStat.EO,ProcStat.EC)) {//if it's still complete
						DateTime dateToUseProcCur=PIn.Date(textDate.Text);
						if(ProcCur.ProcStatus.In(ProcStat.EC,ProcStat.EO)) {
							dateToUseProcCur=DateTime.Today;//ignore date limitation for EO/EC procedures on Edit Completed Procedure (full) and Edit Completed Procedure (limited) permissions.
						}
						if(!Security.IsAuthorized(Permissions.ProcComplEditLimited,dateToUseProcCur)){//block new date, too
							return false;
						}
					}
					if(ProcCur.ProcStatus.In(ProcStat.C,ProcStat.EO,ProcStat.EC)
						&& (ProcOld.ProcDate!=PIn.Date(textDate.Text) //If user changed the procedure date
						|| !ProcOld.ProcFee.IsEqual(PIn.Double(textProcFee.Text)) //If user changed the procedure fee
						|| ProcOld.CodeNum != ProcCur.CodeNum)) //If user changed the procedure code
					{
						Permissions perm=Permissions.ProcComplEdit;
						if(ProcCur.ProcStatus.In(ProcStat.EO,ProcStat.EC)) {
							perm=Permissions.ProcExistingEdit;
						}
						if(!Security.IsAuthorized(perm,PIn.Date(textDate.Text),ProcCur.CodeNum,PIn.Double(textProcFee.Text))) {
							return false;
						}
					}
				}
			}
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				if(checkTypeCodeX.Checked) {
					if(checkTypeCodeA.Checked
						|| checkTypeCodeB.Checked
						|| checkTypeCodeC.Checked
						|| checkTypeCodeE.Checked
						|| checkTypeCodeL.Checked
						|| checkTypeCodeS.Checked) 
					{
						MsgBox.Show(this,"If type code 'none' is checked, no other type codes may be checked.");
						return false;
					}
				}
				if(ProcedureCode2.IsProsth
					&& !checkTypeCodeA.Checked
					&& !checkTypeCodeB.Checked
					&& !checkTypeCodeC.Checked
					&& !checkTypeCodeE.Checked
					&& !checkTypeCodeL.Checked
					&& !checkTypeCodeS.Checked
					&& !checkTypeCodeX.Checked) 
				{
					if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"At least one type code should be checked for prosthesis.  Continue anyway?")) {
						return false;
					}
				}
				if(textCanadaLabFee1.errorProvider1.GetError(textCanadaLabFee1)!="" || textCanadaLabFee2.errorProvider1.GetError(textCanadaLabFee2)!="") {
					MessageBox.Show(Lan.g(this,"Please fix lab fees."));
					return false;
				}
			}
			else {
				if(ProcedureCode2.IsProsth) {
					if(listProsth.SelectedIndex==0
					|| (listProsth.SelectedIndex==2 && textDateOriginalProsth.Text=="")) {
						if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Prosthesis date not entered. Continue anyway?")){
							return false;
						}
					}
				}
			}
			if(Programs.UsingOrion) {
			//if(panelOrion.Visible) {
				if(comboStatus.SelectedIndex==-1) {
					MsgBox.Show(this,"Invalid status.");
					return false;
				}
				if(textDateScheduled.errorProvider1.GetError(textDateScheduled)!="") {
					MsgBox.Show(this,"Invalid schedule date.");
					return false;
				}
				if(textDateStop.errorProvider1.GetError(textDateStop)!="") {
					MsgBox.Show(this,"Invalid stop clock date.");
					return false;
				}
			}
			if(ProcedureCode2.TreatArea==TreatmentArea.Quad) {
				if(!radioUL.Checked && !radioUR.Checked && !radioLL.Checked && !radioLR.Checked) {
					MsgBox.Show(this,"Please select a quadrant.");
					return false;
				}
			}
			//Customers have been complaining about procedurelog entries changing their CodeNum column to 0.
			//Based on a security log provided by a customer, we were able to determine that this is one of two potential violators.
			//The following code is here simply to try and get the user to call us so that we can have proof and hopefully find the core of the issue.
			long verifyCode=ProcedureCodes.GetProcCode(textProc.Text).CodeNum;
			try {
				if(verifyCode < 1) {
					throw new ApplicationException("Invalid Procedure Text");
				}
			}
			catch(ApplicationException ae) {
				string error="Please notify support with the following information.\r\n"
					+"Error: "+ae.Message+"\r\n"
					+"verifyCode: "+verifyCode.ToString()+"\r\n"
					+"textProc.Text: "+textProc.Text+"\r\n"
					+"ProcOld.CodeNum: "+(ProcOld==null ? "NULL" : ProcOld.CodeNum.ToString())+"\r\n"
					+"ProcCur.CodeNum: "+(ProcCur==null ? "NULL" : ProcCur.CodeNum.ToString())+"\r\n"
					+"ProcedureCode2.CodeNum: "+(ProcedureCode2==null ? "NULL" : ProcedureCode2.CodeNum.ToString())+"\r\n"
					+"\r\n"
					+"StackTrace:\r\n"+ae.StackTrace;
				MsgBoxCopyPaste MsgBCP=new MsgBoxCopyPaste(error);
				MsgBCP.Text="Fatal Error!!!";
				MsgBCP.Show();//Use .Show() to make it easy for the user to keep this window open while they call in.
				return false;
			}
			return true;
		}

		///<summary>MUST call EntriesAreValid first.  Used from OK_Click and from butSetComplete_Click</summary>
		private void SaveAndClose(){
			if(textProcFee.Text==""){
				textProcFee.Text="0";
			}
			ProcCur.PatNum=PatCur.PatNum;
			//ProcCur.Code=this.textProc.Text;
			ProcedureCode2=ProcedureCodes.GetProcCode(textProc.Text);
			ProcCur.CodeNum=ProcedureCode2.CodeNum;
			ProcCur.MedicalCode=textMedicalCode.Text;
			ProcCur.Discount=PIn.Double(textDiscount.Text);
			if(_snomedBodySite==null) {
				ProcCur.SnomedBodySite="";
			}
			else {
				ProcCur.SnomedBodySite=_snomedBodySite.SnomedCode;
			}
			ProcCur.IcdVersion=9;
			if(checkIcdVersion.Checked) {
				ProcCur.IcdVersion=10;
			}
			ProcCur.DiagnosticCode="";
			ProcCur.DiagnosticCode2="";
			ProcCur.DiagnosticCode3="";
			ProcCur.DiagnosticCode4="";
			List<string> diagnosticCodes=new List<string>();//A list of all the diagnostic code boxes.
			if(textDiagnosticCode.Text!="") {
				diagnosticCodes.Add(textDiagnosticCode.Text);
			}
			if(textDiagnosticCode2.Text!="") {
				diagnosticCodes.Add(textDiagnosticCode2.Text);
			}
			if(textDiagnosticCode3.Text!="") {
				diagnosticCodes.Add(textDiagnosticCode3.Text);
			}
			if(textDiagnosticCode4.Text!="") {
				diagnosticCodes.Add(textDiagnosticCode4.Text);
			}
			if(diagnosticCodes.Count>0) {
				ProcCur.DiagnosticCode=diagnosticCodes[0];
			}
			if(diagnosticCodes.Count>1) {
				ProcCur.DiagnosticCode2=diagnosticCodes[1];
			}
			if(diagnosticCodes.Count>2) {
				ProcCur.DiagnosticCode3=diagnosticCodes[2];
			}
			if(diagnosticCodes.Count>3) {
				ProcCur.DiagnosticCode4=diagnosticCodes[3];
			}
			ProcCur.IsPrincDiag=checkIsPrincDiag.Checked;
			ProcCur.ProvOrderOverride=_selectedProvOrderNum;
			if(_referralOrdering==null) {
				ProcCur.OrderingReferralNum=0;
			}
			else {
				ProcCur.OrderingReferralNum=_referralOrdering.ReferralNum;
			}
			ProcCur.CodeMod1 = textCodeMod1.Text;
			ProcCur.CodeMod2 = textCodeMod2.Text;
			ProcCur.CodeMod3 = textCodeMod3.Text;
			ProcCur.CodeMod4 = textCodeMod4.Text;
			ProcCur.UnitQty = PIn.Int(textUnitQty.Text);
			ProcCur.UnitQtyType=(ProcUnitQtyType)comboUnitType.SelectedIndex;
			ProcCur.RevCode = textRevCode.Text;
			ProcCur.DrugUnit=(EnumProcDrugUnit)comboDrugUnit.SelectedIndex;
			ProcCur.DrugQty=PIn.Float(textDrugQty.Text);
			ProcCur.ProvNum=_selectedProvNum;
			if(PrefC.GetBool(PrefName.ProcProvChangesClaimProcWithClaim)) {
				ClaimProcsForProc.FindAll(x => !x.Status.In(ClaimProcStatus.Received,ClaimProcStatus.Supplemental)).ForEach(x => x.ProvNum=_selectedProvNum);
			}
			else {
				foreach(ClaimProc claimProcCur in ClaimProcsForProc) {
					//Change claimproc provnum only if not attached to Claim.
					if(!claimProcCur.Status.In(ClaimProcStatus.Received,ClaimProcStatus.NotReceived,ClaimProcStatus.CapClaim)) {
						claimProcCur.ProvNum=_selectedProvNum;
					}
				}
			}
			ProcCur.ClinicNum=_selectedClinicNum;
			bool hasSplitProvChanged=false;
			bool hasAdjProvChanged=false;
			if(ProcCur.ProvNum!=ProcOld.ProvNum) {
				if(PaySplits.IsPaySplitAttached(ProcCur.ProcNum)) {
					List<PaySplit> listPaySplit=PaySplits.GetPaySplitsFromProc(ProcCur.ProcNum);
					foreach(PaySplit paySplit in listPaySplit) {
						if(!Security.IsAuthorized(Permissions.PaymentEdit,Payments.GetPayment(paySplit.PayNum).PayDate)) {
							return;
						}
						if(ProcCur.ProvNum != paySplit.ProvNum) {
							hasSplitProvChanged=true;
						}
					}
					if(hasSplitProvChanged 
						&& !MsgBox.Show(this,MsgBoxButtons.OKCancel,"The provider for the associated payment splits will be changed to match the provider on the procedure."))
					{
						return;
					}
				}
				List<Adjustment> listAdjusts=AdjustmentsForProc.Cast<Adjustment>().ToList();
				foreach(Adjustment adjust in listAdjusts) {
					if(!Security.IsAuthorized(Permissions.AdjustmentEdit,adjust.AdjDate)) {
						return;
					}
					if(ProcCur.ProvNum!=adjust.ProvNum && PrefC.GetInt(PrefName.RigorousAdjustments)==(int)RigorousAdjustments.EnforceFully) {
						hasAdjProvChanged=true;
					}
				}
				if(hasAdjProvChanged 
					&& !MsgBox.Show(this,MsgBoxButtons.OKCancel,"The provider for the associated adjustments will be changed to match the provider on the procedure.")) 
				{
					return;
				}
			}
			double sumPaySplits=0;
			for(int i = 0;i<PaySplitsForProc.Count;i++) {
				sumPaySplits+=((PaySplit)PaySplitsForProc[i]).SplitAmt;
			}
			if(ProcOld.ProcStatus!=ProcStat.C && ProcCur.ProcStatus==ProcStat.C){//Proc set complete.
				ProcCur.DateEntryC=DateTime.Now;//this triggers it to set to server time NOW().
				if(ProcCur.DiagnosticCode=="") {
					ProcCur.DiagnosticCode=PrefC.GetString(PrefName.ICD9DefaultForNewProcs);
				}
			}
			else if(ProcOld.ProcStatus==ProcStat.C && ProcCur.ProcStatus!=ProcStat.C 
				&& Adjustments.GetForProc(ProcCur.ProcNum,Adjustments.Refresh(ProcCur.PatNum)).Count!=0
				&&!MsgBox.Show(this,MsgBoxButtons.YesNo,"This procedure has adjustments attached to it. Changing the status from completed will delete any adjustments for the procedure. Continue?")) 
			{
				return;
			}
			else if(ProcOld.ProcStatus==ProcStat.C && ProcCur.ProcStatus!=ProcStat.C && sumPaySplits!=0) {
				MsgBox.Show(this,"Not allowed to modify the status of a procedure that has payments attached to it. Detach payments from the procedure first.");
				return;
			}
			ProcCur.DateTP=PIn.Date(this.textDateTP.Text);
			ProcCur.ProcDate=PIn.Date(this.textDate.Text);
			DateTime dateT=PIn.DateT(this.textTimeStart.Text);
			ProcCur.ProcTime=new TimeSpan(dateT.Hour,dateT.Minute,0);
			if(Programs.UsingOrion || PrefC.GetBool(PrefName.ShowFeatureMedicalInsurance)) {
				dateT=ParseTime(textTimeStart.Text);
				ProcCur.ProcTime=new TimeSpan(dateT.Hour,dateT.Minute,0);
				dateT=ParseTime(textTimeEnd.Text);
				ProcCur.ProcTimeEnd=new TimeSpan(dateT.Hour,dateT.Minute,0);
			}
			ProcCur.ProcFee=PIn.Double(textProcFee.Text);
			//ProcCur.LabFee=PIn.PDouble(textLabFee.Text);
			//ProcCur.LabProcCode=textLabCode.Text;
			//MessageBox.Show(ProcCur.ProcFee.ToString());
			//Dx taken care of when radio pushed
			switch(ProcedureCode2.TreatArea){
				case TreatmentArea.Surf:
					ProcCur.ToothNum=Tooth.FromInternat(textTooth.Text);
					ProcCur.Surf=Tooth.SurfTidyFromDisplayToDb(textSurfaces.Text,ProcCur.ToothNum);
					break;
				case TreatmentArea.Tooth:
					ProcCur.Surf="";
					ProcCur.ToothNum=Tooth.FromInternat(textTooth.Text);
					break;
				case TreatmentArea.Mouth:
					ProcCur.Surf="";
					ProcCur.ToothNum="";	
					break;
				case TreatmentArea.Quad:
					//surf set when radio pushed
					ProcCur.ToothNum="";	
					break;
				case TreatmentArea.Sextant:
					//surf taken care of when radio pushed
					ProcCur.ToothNum="";	
					break;
				case TreatmentArea.Arch:
					//don't HAVE to select arch
					//taken care of when radio pushed
					ProcCur.ToothNum="";	
					break;
				case TreatmentArea.ToothRange:
					if (listBoxTeeth.SelectedItems.Count<1 && listBoxTeeth2.SelectedItems.Count<1) {
						MessageBox.Show(Lan.g(this,"Must pick at least 1 tooth"));
						return;
					}
          string range="";
					int idxAmer;
					for(int j=0;j<listBoxTeeth.SelectedIndices.Count;j++){
						idxAmer=listBoxTeeth.SelectedIndices[j];
						if(j!=0){
							range+=",";
						}
            range+=Tooth.labelsUniversal[idxAmer];
					}
					for(int j=0;j<listBoxTeeth2.SelectedIndices.Count;j++){
						idxAmer=listBoxTeeth2.SelectedIndices[j]+16;
						if(j!=0){
							range+=",";
						}
            range+=Tooth.labelsUniversal[idxAmer];
          }
			    ProcCur.ToothRange=range;
					ProcCur.Surf="";
					ProcCur.ToothNum="";	
					break;
			}
			//Status taken care of when list pushed
			ProcCur.Note=this.textNotes.Text;
			//Larger offices have trouble with doctors editing specific procedure notes at the same time.
			//One of our customers paid for custom programming that will merge the two notes together in a specific fashion if there was concurrency issues.
			//A specific preference was added because this functionality is so custom.  Typical users can just use the Chart View Audit mode for this info.
			if(ProcOld.ProcNum > 0 && PrefC.GetBool(PrefName.ProcNoteConcurrencyMerge)) {
				//Go to the database to get the most recent version of the current procedure's note and check it against ProcOld.Note to see if they differ.
				List<ProcNote> listProcNotes=ProcNotes.GetProcNotesForProc(ProcOld.ProcNum)
					.OrderByDescending(x => x.EntryDateTime)
					.ThenBy(x => x.ProcNoteNum)//Just in case two notes were entered at the "same time" (current version of MySQL can't handle milliseconds)
					.ToList();
				//If there are notes for the current procedure, get the most recent note and compare it to ProcOld.Note.
				//If the current database note differs from the ProcOld.Note then there was a concurrency issue and we have to merge the db note.
				if(listProcNotes.Count > 0 && ProcOld.Note!=listProcNotes[0].Note) {
					//Manipulate ProcCur.Note to include the most recent note in its entirety with some custom information required by job #2484
					//Use DateTime.Now because the ProcNote won't get inserted until farther down in this method but we have to do this manipulation before sig.
					ProcCur.Note=DateTime.Now.ToString()+"  "+Userods.GetName(ProcCur.UserNum)+"\r\n"+ProcCur.Note;
					//Now we need to append the old note from the database in the same format.
					ProcCur.Note+="\r\n------------------------------------------------------\r\n"
						+listProcNotes[0].EntryDateTime.ToString()+"  "+Userods.GetName(listProcNotes[0].UserNum)
						+"\r\n"+listProcNotes[0].Note;
				}
			}
			try {
				SaveSignature();
			}
			catch(Exception ex){
				MessageBox.Show(Lan.g(this,"Error saving signature.")+"\r\n"+ex.Message);
				//and continue with the rest of this method
			}
			#region Update paysplits
			if(hasSplitProvChanged) {
				PaySplits.UpdateAttachedPaySplits(ProcCur);//update the attached paysplits.
			}
			if(hasAdjProvChanged) {
				foreach(Adjustment adjust in AdjustmentsForProc) {//update the attached adjustments
					adjust.ProvNum=ProcCur.ProvNum;
					Adjustments.Update(adjust);
				}
			}
			#endregion
			ProcCur.HideGraphics=checkHideGraphics.Checked;
			if(comboDx.SelectedIndex!=-1) {
				ProcCur.Dx=_listDiagnosisDefs[comboDx.SelectedIndex].DefNum;
			}
			if(comboPrognosis.SelectedIndex==0) {
				ProcCur.Prognosis=0;
			}
			else {
				ProcCur.Prognosis=_listPrognosisDefs[comboPrognosis.SelectedIndex-1].DefNum;
			}
			if(comboPriority.SelectedIndex==0) {
				ProcCur.Priority=0;
			}
			else {
				ProcCur.Priority=_listTxPriorityDefs[comboPriority.SelectedIndex-1].DefNum;
			}
			ProcCur.PlaceService=(PlaceOfService)comboPlaceService.SelectedIndex;
			//site set when user picks from list.
			if(comboBillingTypeOne.SelectedIndex==0){
				ProcCur.BillingTypeOne=0;
			}
			else{
				ProcCur.BillingTypeOne=_listBillingTypeDefs[comboBillingTypeOne.SelectedIndex-1].DefNum;
			}
			if(comboBillingTypeTwo.SelectedIndex==0) {
				ProcCur.BillingTypeTwo=0;
			}
			else {
				ProcCur.BillingTypeTwo=_listBillingTypeDefs[comboBillingTypeTwo.SelectedIndex-1].DefNum;
			}
			ProcCur.BillingNote=textBillingNote.Text;
			//ProcCur.HideGraphical=checkHideGraphical.Checked;
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				ProcCur.CanadianTypeCodes="";
				if(checkTypeCodeA.Checked) {
					ProcCur.CanadianTypeCodes+="A";
				}
				if(checkTypeCodeB.Checked) {
					ProcCur.CanadianTypeCodes+="B";
				}
				if(checkTypeCodeC.Checked) {
					ProcCur.CanadianTypeCodes+="C";
				}
				if(checkTypeCodeE.Checked) {
					ProcCur.CanadianTypeCodes+="E";
				}
				if(checkTypeCodeL.Checked) {
					ProcCur.CanadianTypeCodes+="L";
				}
				if(checkTypeCodeS.Checked) {
					ProcCur.CanadianTypeCodes+="S";
				}
				if(checkTypeCodeX.Checked) {
					ProcCur.CanadianTypeCodes+="X";
				}
				double canadaLabFee1=0;
				if(textCanadaLabFee1.Text!="") {
					canadaLabFee1=PIn.Double(textCanadaLabFee1.Text);
				}
				if(canadaLabFee1==0) {
					if(textCanadaLabFee1.Visible && canadaLabFees.Count>0) { //Don't worry about deleting child lab fees if we are editing a lab fee. No such concept.
						Procedures.Delete(canadaLabFees[0].ProcNum);
					}
				}
				else { //canadaLabFee1!=0
					if(canadaLabFees.Count>0) { //Retain the old lab code if present.
						Procedure labFee1Old=canadaLabFees[0].Copy();
						canadaLabFees[0].ProcFee=canadaLabFee1;
						Procedures.Update(canadaLabFees[0],labFee1Old);
					}
					else {
						Procedure labFee1=new Procedure();
						labFee1.PatNum=ProcCur.PatNum;
						labFee1.ProcDate=ProcCur.ProcDate;
						labFee1.ProcFee=canadaLabFee1;
						labFee1.ProcStatus=ProcCur.ProcStatus;
						labFee1.ProvNum=ProcCur.ProvNum;
						labFee1.DateEntryC=DateTime.Now;
						labFee1.ClinicNum=ProcCur.ClinicNum;
						labFee1.ProcNumLab=ProcCur.ProcNum;
						labFee1.CodeNum=ProcedureCodes.GetCodeNum("99111");
						//Not sure if Place of Service is required for canadian labs. (I don't see any reason why this would/could/should break anything.)
						labFee1.PlaceService=(PlaceOfService)PrefC.GetInt(PrefName.DefaultProcedurePlaceService);//Default proc place of service for the Practice is used.
						if(labFee1.CodeNum==0) { //Code does not exist.
							ProcedureCode code99111=new ProcedureCode();
							code99111.IsCanadianLab=true;
							code99111.ProcCode="99111";
							code99111.Descript="+L Commercial Laboratory Procedures";
							code99111.AbbrDesc="Lab Fee";
							code99111.ProcCat=Defs.GetByExactNameNeverZero(DefCat.ProcCodeCats,"Adjunctive General Services");
							ProcedureCodes.Insert(code99111);
							labFee1.CodeNum=code99111.CodeNum;
							ProcedureCodes.RefreshCache();
						}
						Procedures.Insert(labFee1);
					}
				}
				double canadaLabFee2=0;
				if(textCanadaLabFee2.Text!="") {
					canadaLabFee2=PIn.Double(textCanadaLabFee2.Text);
				}
				if(canadaLabFee2==0) {
					if(textCanadaLabFee2.Visible && canadaLabFees.Count>1) { //Don't worry about deleting child lab fees if we are editing a lab fee. No such concept.
						Procedures.Delete(canadaLabFees[1].ProcNum);
					}
				}
				else { //canadaLabFee2!=0
					if(canadaLabFees.Count>1) { //Retain the old lab code if present.
						Procedure labFee2Old=canadaLabFees[1].Copy();
						canadaLabFees[1].ProcFee=canadaLabFee2;
						Procedures.Update(canadaLabFees[1],labFee2Old);
					}
					else {
						Procedure labFee2=new Procedure();
						labFee2.PatNum=ProcCur.PatNum;
						labFee2.ProcDate=ProcCur.ProcDate;
						labFee2.ProcFee=canadaLabFee2;
						labFee2.ProcStatus=ProcCur.ProcStatus;
						labFee2.ProvNum=ProcCur.ProvNum;
						labFee2.DateEntryC=DateTime.Now;
						labFee2.ClinicNum=ProcCur.ClinicNum;
						labFee2.ProcNumLab=ProcCur.ProcNum;
						labFee2.CodeNum=ProcedureCodes.GetCodeNum("99111");
						//Not sure if Place of Service is required for canadian labs. (I don't see any reason why this would/could/should break anything.)
						labFee2.PlaceService=(PlaceOfService)PrefC.GetInt(PrefName.DefaultProcedurePlaceService);//Default proc place of service for the Practice is used.
						if(labFee2.CodeNum==0) { //Code does not exist.
							ProcedureCode code99111=new ProcedureCode();
							code99111.IsCanadianLab=true;
							code99111.ProcCode="99111";
							code99111.Descript="+L Commercial Laboratory Procedures";
							code99111.AbbrDesc="Lab Fee";
							code99111.ProcCat=Defs.GetByExactNameNeverZero(DefCat.ProcCodeCats,"Adjunctive General Services");
							ProcedureCodes.Insert(code99111);
							labFee2.CodeNum=code99111.CodeNum;
							ProcedureCodes.RefreshCache();
						}
						Procedures.Insert(labFee2);
					}
				}
			}
			else {
				if(ProcedureCode2.IsProsth) {
					switch(listProsth.SelectedIndex) {
						case 0:
							ProcCur.Prosthesis="";
							break;
						case 1:
							ProcCur.Prosthesis="I";
							break;
						case 2:
							ProcCur.Prosthesis="R";
							break;
					}
					ProcCur.DateOriginalProsth=PIn.Date(textDateOriginalProsth.Text);
					ProcCur.IsDateProsthEst=checkIsDateProsthEst.Checked;
				}
				else {
					ProcCur.Prosthesis="";
					ProcCur.DateOriginalProsth=DateTime.MinValue;
					ProcCur.IsDateProsthEst=false;
				}
			}
			ProcCur.ClaimNote=textClaimNote.Text;
			//Last chance to run this code before Proc gets updated.
			if(Programs.UsingOrion){//Ask for an explanation. If they hit cancel here, return and don't save.
				OrionProcCur.DPC=(OrionDPC)comboDPC.SelectedIndex;
				OrionProcCur.DPCpost=(OrionDPC)comboDPCpost.SelectedIndex;
				OrionProcCur.DateScheduleBy=PIn.Date(textDateScheduled.Text);
				OrionProcCur.DateStopClock=PIn.Date(textDateStop.Text);
				OrionProcCur.IsOnCall=checkIsOnCall.Checked;
				OrionProcCur.IsEffectiveComm=checkIsEffComm.Checked;
				OrionProcCur.IsRepair=checkIsRepair.Checked;
				if(IsNew) {
					OrionProcs.Insert(OrionProcCur);
				}
				else {//Is not new.
					if(FormProcEditExplain.GetChanges(ProcCur,ProcOld,OrionProcCur,OrionProcOld)!="") {//Checks if any changes were made. Also sets static variable Changes.
						//If a day old and the orion procedure status did not go from TP to C, CS or CR, then show explaination window.
						if((ProcOld.DateTP.Date<MiscData.GetNowDateTime().Date &&
							(OrionProcOld.Status2!=OrionStatus.TP || (OrionProcCur.Status2!=OrionStatus.C && OrionProcCur.Status2!=OrionStatus.CS && OrionProcCur.Status2!=OrionStatus.CR))))
						{
							FormProcEditExplain FormP=new FormProcEditExplain();
							FormP.dpcChange=((int)OrionProcOld.DPC!=(int)OrionProcCur.DPC);
							if(FormP.ShowDialog()!=DialogResult.OK) {
								return;
							}
							Procedure ProcPreExplain=ProcOld.Copy();
							ProcOld.Note=FormProcEditExplain.Explanation;
							Procedures.Update(ProcOld,ProcPreExplain);
							Thread.Sleep(1100);
						}
					}
					OrionProcs.Update(OrionProcCur);
					//Date entry needs to be updated when status changes to cancelled or refused and at least a day old.
					if(ProcOld.DateTP.Date<MiscData.GetNowDateTime().Date &&
						OrionProcCur.Status2==OrionStatus.CA_EPRD ||
						OrionProcCur.Status2==OrionStatus.CA_PD ||
						OrionProcCur.Status2==OrionStatus.CA_Tx ||
						OrionProcCur.Status2==OrionStatus.R) 
					{
						ProcCur.DateEntryC=MiscData.GetNowDateTime().Date;
					}
				}//End of "is not new."
			}
			if(ProcCur.ProvNum!=ProcOld.ProvNum 
				&& ProcCur.ProcFee==ProcOld.ProcFee)
			{
				InsPlan insPlanPrimary=null;//We only care about updating fees with the primary insurance plan.
				if(PatPlanList.Count>0) {
					InsSub insSubPrimary=InsSubs.GetSub(PatPlanList[0].InsSubNum,SubList);
					insPlanPrimary=InsPlans.GetPlan(insSubPrimary.PlanNum,PlanList);
				}
				string promptText="";
				bool isUpdatingFee=Procedures.FeeUpdatePromptHelper(new List<Procedure>() { ProcCur.Copy() },new List<Procedure>() { ProcOld.Copy() },
					insPlanPrimary,ref promptText);
				if(isUpdatingFee) {//Made it pass the pref check.
					if(promptText!="" && !MsgBox.Show(this,MsgBoxButtons.YesNo,promptText)) {
							isUpdatingFee=false;
					}
					if(isUpdatingFee) {
						ProcCur.ProcFee=Fees.GetAmount0(ProcCur.CodeNum,Providers.GetProv(ProcCur.ProvNum).FeeSched,ProcCur.ClinicNum,ProcCur.ProvNum);
					}
				}
			}
			//Autocodes----------------------------------------------------------------------------------------------------------------------------------------
			Permissions perm=Permissions.ProcComplEdit;
			if(ProcCur.ProcStatus.In(ProcStat.EC,ProcStat.EO)) {
				perm=Permissions.ProcExistingEdit;
			}
			if(!ProcOld.ProcStatus.In(ProcStat.C,ProcStat.EO,ProcStat.EC)
				|| Security.IsAuthorized(perm,ProcCur.ProcDate,true)) {
				//Only check auto codes if the procedure is not complete or the user has permission to edit completed procedures.
				long verifyCode;
				bool isMandibular=(listBoxTeeth.SelectedIndices.Count < 1);
				if(AutoCodeItems.ShouldPromptForCodeChange(ProcCur,ProcedureCode2,PatCur,isMandibular,ClaimProcsForProc,out verifyCode)) {
					FormAutoCodeLessIntrusive FormACLI=new FormAutoCodeLessIntrusive(PatCur,ProcCur,ProcedureCode2,verifyCode,PatPlanList,SubList,PlanList,
						BenefitList,ClaimProcsForProc,listBoxTeeth.Text);
					if(FormACLI.ShowDialog() != DialogResult.OK
						&& PrefC.GetBool(PrefName.ProcEditRequireAutoCodes)) 
					{
						return;//send user back to fix information or use suggested auto code.
					}
					ProcCur=FormACLI.Proc;
					ClaimProcsForProc=ClaimProcs.RefreshForProc(ProcCur.ProcNum);//FormAutoCodeLessIntrusive may have added claimprocs.
				}
			}
			//The actual update----------------------------------------------------------------------------------------------------------------------------------
			Procedures.Update(ProcCur,ProcOld);
			if(ProcCur.ProcStatus==ProcStat.TP) {
				//if proc is TP status, update priority on any TreatPlanAttach objects if they are attaching this proc to the active TP
				TreatPlan activePlan=TreatPlans.GetActiveForPat(ProcCur.PatNum);
				if(activePlan!=null) {
					List<TreatPlanAttach> listTpAttaches=TreatPlanAttaches.GetAllForTreatPlan(activePlan.TreatPlanNum);
					//should only be 0 or one TPAttach on this TP with this ProcNum
					listTpAttaches.FindAll(x => x.ProcNum==ProcCur.ProcNum).ForEach(x => x.Priority=ProcCur.Priority);
					TreatPlanAttaches.Sync(listTpAttaches,activePlan.TreatPlanNum);
				}
			}
			if(ProcCur.AptNum>0 || ProcCur.PlannedAptNum>0) {
				//Update the ProcDescript on the appointment if procedure is attached to one.
				Appointment apt;
				if(ProcCur.AptNum>0) {
					apt=Appointments.GetOneApt(ProcCur.AptNum);
				}
				else {
					apt=Appointments.GetOneApt(ProcCur.PlannedAptNum);
				}
				Appointments.UpdateProcDescriptForAppts(new List<Appointment>() { apt });
			}
			ClaimProcsForProc.ForEach(x => x.ClinicNum=_selectedClinicNum);//These changes save in Form_Closing ComputeEstimates depending on DialogResult
			//Recall synch---------------------------------------------------------------------------------------------------------------------------------
			Recalls.Synch(ProcCur.PatNum);
			//Security logs--------------------------------------------------------------------------------------------------------------------------------
			if(ProcCur.Discount!=ProcOld.Discount) {//Discount was changed
				string message=Lan.g(this,"Discount created or changed from Proc Edit window for procedure")
						+": "+ProcedureCodes.GetProcCode(ProcCur.CodeNum).ProcCode+"  "+Lan.g(this,"Dated")
						+": "+ProcCur.ProcDate.ToShortDateString()+"  "+Lan.g(this,"With a Fee of")+": "+ProcCur.ProcFee.ToString("c")+".  "
						+Lan.g(this,"Changed the discount value from")+" "+ProcOld.Discount.ToString("c")+" "+Lan.g(this,"to")+" "+ProcCur.Discount.ToString("c");
				SecurityLogs.MakeLogEntry(Permissions.TreatPlanDiscountEdit,ProcCur.PatNum,message);
			}
			if(ProcOld.ProcStatus!=ProcStat.C && ProcCur.ProcStatus==ProcStat.C){
				//Auto-insert default encounter -------------------------------------------------------------------------------------------------------------
				Encounters.InsertDefaultEncounter(ProcCur.PatNum,ProcCur.ProvNum,ProcCur.ProcDate);
				//OrthoProcedures ---------------------------------------------------------------------------------------------------------------------------
				Procedures.SetOrthoProcComplete(ProcCur,ProcedureCode2);
				//if status was changed to complete
				Procedures.LogProcComplCreate(PatCur.PatNum,ProcCur,ProcCur.ToothNum);
				List<string> procCodeList=new List<string>();
				procCodeList.Add(ProcedureCodes.GetStringProcCode(ProcCur.CodeNum));
				AutomationL.Trigger(AutomationTrigger.CompleteProcedure,procCodeList,ProcCur.PatNum);
			}
			else if(IsNew && ProcCur.ProcStatus==ProcStat.C){
				//if new procedure is complete
				Procedures.LogProcComplCreate(PatCur.PatNum,ProcCur,ProcCur.ToothNum);
			}
			else if(!IsNew && ProcOld.ProcStatus.In(ProcStat.C,ProcStat.EO,ProcStat.EC)){
				perm=Permissions.ProcComplEdit;//If was complete before the window loaded.
				string logText=ProcedureCode2.ProcCode+" ("+ProcOld.ProcStatus+"), ";
				if(listBoxTeeth.Text!=null && listBoxTeeth.Text.Trim()!="") {
					logText+=Lan.g("FormProcEdit","Teeth")+": "+listBoxTeeth.Text+", ";
				}
				logText+=Lan.g("FormProcEdit","Fee")+": "+ProcCur.ProcFee.ToString("F")+", "+ProcedureCode2.Descript;
				if(ProcOld.ProcStatus.In(ProcStat.EO,ProcStat.EC)) {
					perm=Permissions.ProcExistingEdit;
				}
				SecurityLogs.MakeLogEntry(perm,PatCur.PatNum,logText);
			}
			if((ProcCur.ProcStatus==ProcStat.C || ProcCur.ProcStatus==ProcStat.EC || ProcCur.ProcStatus==ProcStat.EO)
				&& ProcedureCodes.GetProcCode(ProcCur.CodeNum).PaintType==ToothPaintingType.Extraction) {
				//if an extraction, then mark previous procs hidden
				//Procedures.SetHideGraphical(ProcCur);//might not matter anymore
				ToothInitials.SetValue(ProcCur.PatNum,ProcCur.ToothNum,ToothInitialType.Missing);
			}
			//Canadian lab fees complete-----------------------------------------------------------------------------------------------------------------------
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA") && ProcCur.ProcStatus==ProcStat.C) {//Canada
				Procedures.SetCanadianLabFeesCompleteForProc(ProcCur);
			}
			//Canadian lab fees not complete-----------------------------------------------------------------------------------------------------------------------
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA") && ProcCur.ProcStatus!=ProcStat.C) {//Canada
				Procedures.SetCanadianLabFeesStatusForProc(ProcCur);
			}
      DialogResult=DialogResult.OK;
			//it is assumed that we will do an immediate refresh after closing this window.
		}

		private void butChangeUser_Click(object sender,EventArgs e) {
			FormLogOn FormChangeUser=new FormLogOn();
			FormChangeUser.IsSimpleSwitch=true;
			FormChangeUser.ShowDialog();
			if(FormChangeUser.DialogResult==DialogResult.OK) { //if successful
				_curUser=FormChangeUser.CurUserSimpleSwitch; //assign temp user
				FillClinicCombo();//update clinics and providers if needed
				signatureBoxWrapper.ClearSignature(); //clear sig
				signatureBoxWrapper.UserSig=_curUser;
				textUser.Text=_curUser.UserName; //update user textbox.
				SigChanged=true;
				_hasUserChanged=true;
			}
		}

		private void SaveSignature(){
			if(SigChanged){
				string keyData=GetSignatureKey();
				ProcCur.Signature=signatureBoxWrapper.GetSignature(keyData);
				ProcCur.SigIsTopaz=signatureBoxWrapper.GetSigIsTopaz();
			}
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(!EntriesAreValid()) {
				return;
			}
			if(Programs.UsingOrion) {
				if(OrionProcOld!=null && OrionProcOld.DateStopClock.Year>1880) {
					if(PIn.Date(textDateStop.Text)>OrionProcOld.DateStopClock.Date) {
						MsgBox.Show(this,"Future date not allowed for Date Stop Clock.");
						return;
					}
				}
				else if(PIn.Date(textDateStop.Text)>MiscData.GetNowDateTime().Date) {
					MsgBox.Show(this,"Future date not allowed for Date Stop Clock.");
					return;
				}
				//Strange logic for Orion for setting sched by date to a scheduled date from a previously cancelled TP on the same tooth/surf.
				if(ProcCur.Surf!="" || textTooth.Text!="" || textSurfaces.Text!="") {
					DataTable table=OrionProcs.GetCancelledScheduleDateByToothOrSurf(ProcCur.PatNum,textTooth.Text.ToString(),ProcCur.Surf);
					if(table.Rows.Count>0) {
						if(textDateScheduled.Text!="" && DateTime.Parse(textDateScheduled.Text)>PIn.DateT(table.Rows[0]["DateScheduleBy"].ToString())) {
							//If the cancelled sched by date is in the past then do nothing.
							if(PIn.DateT(table.Rows[0]["DateScheduleBy"].ToString())>MiscData.GetNowDateTime().Date) {
								textDateScheduled.Text=((DateTime)table.Rows[0]["DateScheduleBy"]).ToShortDateString();
								CancelledScheduleByDate=DateTime.Parse(textDateScheduled.Text);
								MsgBox.Show(this,"Schedule by date cannot be later than: "+textDateScheduled.Text+".");
								return;
							}
						}
					}
				}
				if(comboDPC.SelectedIndex==0) {
					MsgBox.Show(this,"DPC should not be \"Not Specified\".");
					return;
				}
			}
			if(_hasUserChanged && signatureBoxWrapper.SigIsBlank && !MsgBox.Show(this,MsgBoxButtons.OKCancel,
				"The signature box has not been re-signed.  Continuing will remove the previous signature from this procedure.  Exit anyway?")) {
				return;
			}
			SaveAndClose();
			Plugins.HookAddCode(this,"FormProcEdit.butOK_Click_end",ProcCur); 
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormProcEdit_FormClosing(object sender,FormClosingEventArgs e) {
			//We need to update the CPOE status even if the user is cancelling out of the window.
			if(Userods.IsUserCpoe(_curUser) && !ProcOld.IsCpoe) {
				//There's a possibility that we are making a second, unnecessary call to the database here but it is worth it to help meet EHR measures.
				Procedures.UpdateCpoeForProc(ProcCur.ProcNum,true);
				//Make a log that we edited this procedure's CPOE flag.
				SecurityLogs.MakeLogEntry(Permissions.ProcEdit,ProcCur.PatNum,ProcedureCodes.GetProcCode(ProcCur.CodeNum).ProcCode
					+", "+ProcCur.ProcFee.ToString("c")+", "+Lan.g(this,"automatically flagged as CPOE."));
			}
			if(DialogResult==DialogResult.OK) {
				//this catches date,prov,fee,status,etc for all claimProcs attached to this proc.
				if(StartedAttachedToClaim!=Procedures.IsAttachedToClaim(ProcCur.ProcNum)) {
					//unless they got attached to a claim while this window was open.  Then it doesn't touch them.
					//We don't want to allow ComputeEstimates to reattach the procedure to the old claim which could have deleted.
					return;
				}
				//Now we have to double check that every single claimproc is attached to the same claim that they were originally attached to.
				if(ClaimProcs.IsAttachedToDifferentClaim(ProcCur.ProcNum,ClaimProcsForProc)) {
					return;//The claimproc is not attached to the same claim it was originally pointing to.  Do not run ComputeEstimates which would point it back to the old (potentially deleted) claim.
				}
				List<ClaimProcHist> histList=ClaimProcs.GetHistList(ProcCur.PatNum,BenefitList,PatPlanList,PlanList,-1,DateTimeOD.Today,SubList);
				//We don't want already existing claim procs on this procedure to affect the calculation for this procedure.
				histList.RemoveAll(x => x.ProcNum==ProcCur.ProcNum);
				Procedures.ComputeEstimates(ProcCur,PatCur.PatNum,ref ClaimProcsForProc,_isQuickAdd,PlanList,PatPlanList,BenefitList,histList,
					new List<ClaimProcHist> { },true,PatCur.Age,SubList);
				if(_isEstimateRecompute
					&& ProcCur.ProcNumLab!=0//By definition of procedure.ProcNumLab, this will only happen in Canada and if ProcCur is a lab fee.
					&& !Procedures.IsAttachedToClaim(ProcCur.ProcNumLab))//If attached to a claim, then user should recreate claim because estimates will be inaccurate not matter what.
				{
					Procedure procParent=Procedures.GetOneProc(ProcCur.ProcNumLab,false);
					if(procParent!=null) {//A null parent proc could happen in rare cases for older databases.
						List<ClaimProc> listParentClaimProcs=ClaimProcs.RefreshForProc(procParent.ProcNum);
						Procedures.ComputeEstimates(procParent,PatCur.PatNum,listParentClaimProcs,false,PlanList,PatPlanList,BenefitList,PatCur.Age,SubList);
					}
				}
				return;
			}
			if(IsNew) {//if cancelling on a new procedure
				//delete any newly created claimprocs
				for(int i=0;i<ClaimProcsForProc.Count;i++) {
					//if(ClaimProcsForProc[i].ProcNum==ProcCur.ProcNum) {
					ClaimProcs.Delete(ClaimProcsForProc[i]);
					//}
				}
			}
		}
	}
}
