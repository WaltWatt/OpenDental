using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental{
///<summary></summary>
	public class FormModuleSetup:ODForm {
		#region UI elements
		private Label label40;
		private ComboBox comboAutoSplitPref;
		private System.Windows.Forms.CheckBox checkTreatPlanShowCompleted;
		private System.Windows.Forms.CheckBox checkEclaimsSeparateTreatProv;
		private System.Windows.Forms.CheckBox checkBalancesDontSubtractIns;
		private CheckBox checkAgingMonthly;
		private CheckBox checkProviderIncomeShows;
		private CheckBox checkAutoClearEntryStatus;
		private CheckBox checkShowFamilyCommByDefault;
		private CheckBox checkClaimFormTreatDentSaysSigOnFile;
		private CheckBox checkAllowSettingProcsComplete;
		private CheckBox checkStoreCCTokens;
		private CheckBox checkClaimsValidateACN;
		private CheckBox checkImagesModuleTreeIsCollapsed;
		private CheckBox checkRxSendNewToQueue;
		private CheckBox checkClaimMedTypeIsInstWhenInsPlanIsMedical;
		private CheckBox checkProcGroupNoteDoesAggregate;
		private CheckBox checkMedicalFeeUsedForNewProcs;
		private CheckBox checkAccountShowPaymentNums;
		private ComboBox comboSearchBehavior;
		private CheckBox checkIntermingleDefault;
		private CheckBox checkStatementShowReturnAddress;
		private CheckBox checkStatementShowProcBreakdown;
		private CheckBox checkShowCC;
		private CheckBox checkStatementShowNotes;
		private CheckBox checkStatementShowAdjNotes;
		private CheckBox checkProcLockingIsAllowed;
		private CheckBox checkTimeCardADP;
		private CheckBox checkChartNonPatientWarn;
		private CheckBox checkTreatPlanItemized;
		private CheckBox checkStatementsUseSheets;
		private CheckBox checkWaitingRoomFilterByView;
		private CheckBox checkClaimsSendWindowValidateOnLoad;
		private CheckBox checkProvColorChart;
		private CheckBox checkApptModuleDefaultToWeek;
		private CheckBox checkPerioSkipMissingTeeth;
		private CheckBox checkPerioTreatImplantsAsNotMissing;
		private CheckBox checkScreeningsUseSheets;
		private CheckBox checkBrokenApptCommLog;
		private CheckBox checkBrokenApptAdjustment;
		private CheckBox checkTPSaveSigned;
		private CheckBox checkAppointmentTimeIsLocked;
		private CheckBox checkDxIcdVersion;
		private CheckBox checkSelectProv;
		private CheckBox checkApptTimeReset;
		private CheckBox checkRecurChargPriProv;
		private CheckBox checkApptModuleAdjInProd;
		private CheckBox checkSuperFamSync;
		private CheckBox checkPayPlansUseSheets;
		private CheckBox checkPpoUseUcr;
		private CheckBox checkProcsPromptForAutoNote;
		private CheckBox checkFrequency;
		private CheckBox checkSuperFamAddIns;
		private CheckBox checkPaymentsPromptForPayType;
		private CheckBox checkBillingShowProgress;
		private CheckBox checkUseOpHygProv;
		private CheckBox checkProcEditRequireAutoCode;
		private CheckBox checkEclaimsMedicalProvTreatmentAsOrdering;
		private CheckBox checkClaimProcsAllowEstimatesOnCompl;
		private CheckBox checkApptsCheckFrequency;
		private CheckBox checkPayPlansExcludePastActivity;
		private CheckBox checkScheduleProvEmpSelectAll;
		private CheckBox checkApptModuleProductionUsesOps;
		private CheckBox checkApptsRequireProcs;
		private CheckBox checkApptAllowFutureComplete;
		private CheckBox checkApptAllowEmptyComplete;
		private CheckBox checkSignatureAllowDigital;
		private CheckBox checkClaimPaymentBatchOnly;
		private CheckBox checkClaimUseOverrideProcDescript;
		private CheckBox checkRecurringChargesUseTransDate;
		private CheckBox checkStatementInvoiceGridShowWriteoffs;
		private CheckBox checkPatInitBillingTypeFromPriInsPlan;
		private CheckBox checkClaimTrackingRequireError;
		private CheckBox checkProcProvChangesCp;
		private CheckBox checkTextMsgOkStatusTreatAsNo;
		private CheckBox checkFamPhiAccess;
		private CheckBox checkPPOpercentage;
		private CheckBox checkAllowedFeeSchedsAutomate;
		private CheckBox checkInsPPOsecWriteoffs;
		private CheckBox checkInsurancePlansShared;
		private CheckBox checkCoPayFeeScheduleBlankLikeZero;
		private CheckBox checkInsDefaultShowUCRonClaims;
		private CheckBox checkGoogleAddress;
		private CheckBox checkAllowProcAdjFromClaim;
		private CheckBox checkSuperFamCloneCreate;
		private CheckBox checkHideDueNow;
		private CheckBox checkAllowEmailCCReceipt;
		private CheckBox checkInsDefaultAssignmentOfBenefits;
		private CheckBox checkBoxRxClinicUseSelected;
		private CheckBox checkAllowFutureDebits;
		private RadioButton radioImagesModuleTreeIsExpanded;
		private RadioButton radioImagesModuleTreeIsCollapsed;
		private RadioButton radioImagesModuleTreeIsPersistentPerUser;
		private RadioButton radioTreatPlanSortTooth;
		private RadioButton radioTreatPlanSortOrder;
		private TabPage tabAppts;
		private TabPage tabFamily;
		private TabPage tabAccount;
		private TabPage tabTreatPlan;
		private TabPage tabChart;
		private TabPage tabImages;
		private TabPage tabManage;
		private System.Windows.Forms.Label label1;
		private Label label7;
		private Label label12;
		private Label label20;
		private Label label4;
		private Label label6;
		private Label label5;
		private Label label3;
		private Label label8;
		private Label label9;
		private Label label14;
		private Label label13;
		private Label label16;
		private Label label2;
		private Label label10;
		private Label label11;
		private Label label18;
		private Label label19;
		private Label label21;
		private Label label22;
		private Label label24;
		private Label label17;
		private Label label28;
		private Label label29;
		private Label label31;
		private Label label30;
		private Label label32;
		private Label label26;
		private Label label33;
		private Label label36;
		private Label label34;
		private Label label38;
		private Label label37;
		private Label label27;
		private Label label35;
		private Label label15;
		private Label label39;
		private Label labelIcdCodeDefault;
		private Label labelDiscountPercentage;
		private Label labelSuperFamSort;
		private Label labelProcFeeUpdatePrompt;
		private Label apptClickDelay;
		private Label labelClaimsReceivedDays;
		private ComboBox comboBrokenApptAdjType;
		private ComboBox comboFinanceChargeAdjType;
		private ComboBox comboTimeDismissed;
		private ComboBox comboTimeSeated;
		private ComboBox comboTimeArrived;
		private ComboBox comboBillingChargeAdjType;
		private ComboBox comboTimeCardOvertimeFirstDayOfWeek;
		private ComboBox comboUseChartNum;
		private ComboBox comboProcDiscountType;
		private ComboBox comboSuperFamSort;
		private ComboBox comboUnallocatedSplits;
		private ComboBox comboClaimSnapshotTrigger;
		private ComboBox comboProcCodeListSort;
		private ComboBox comboSalesTaxAdjType;
		private ComboBox comboDelay;
		private ComboBox comboBrokenApptProc;
		private ComboBox comboPaymentClinicSetting;
		private ComboBox comboPayPlansVersion;
		private ComboBox comboCobRule;
		private ComboBox comboRigorousAccounting;
		private ComboBox comboProcFeeUpdatePrompt;
		private TextBox textDiscountPercentage;
		private TextBox textApptBubNoteLength;
		private TextBox textWaitRoomWarn;
		private TextBox textClaimAttachPath;
		private TextBox textMedicationsIndicateNone;
		private TextBox textAllergiesIndicateNone;
		private TextBox textMedDefaultStopDays;
		private TextBox textInsWriteoffDescript;
		private TextBox textClaimSnapshotRunTime;
		private TextBox textICD9DefaultForNewProcs;
		private TextBox textTaxPercent;
		private TextBox textProblemsIndicateNone;
		private TextBox textInsPano;
		private TextBox textInsBW;
		private TextBox textInsExam;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private UI.Button butProblemsIndicateNone;
		private UI.Button butDiagnosisCode;
		private UI.Button butMedicationsIndicateNone;
		private UI.Button butAllergiesIndicateNone;
		private UI.Button butBadDebt;
		private GroupBox groupBox1;
		private GroupBox groupBox2;
		private GroupBox groupBox4;
		private GroupBox groupBox6;
		private GroupBox groupBox3;
		private GroupBox groupPayPlans;
		private GroupBox groupTreatPlanSort;
		private TabControl tabControlMain;
		private TabControl tabControlAccount;
		private TabControl tabControlFamily;
		private TabPage tabPagePayAdj;
		private TabPage tabPageIns;
		private TabPage tabPageMisc;
		private TabPage tabPageFamGen;
		private TabPage tabPageSuperFam;
		private TabPage tabPageClaimSnapShot;
		private ODtextBox textTreatNote;
		private ListBox listboxBadDebtAdjs;
		private ToolTip toolTip1;
		private ValidNumber textStatementsCalcDueDate;
		private ValidNum textPayPlansBillInAdvanceDays;
		private ValidNum textBillingElectBatchMax;
		private IContainer components;
		#endregion
		
		private List<Def> listPosAdjTypes;
		private List<Def> listNegAdjTypes;
		private bool _changed;
		private ColorDialog colorDialog;
		///<summary>Used to determine a specific tab to have opened upon load.  Only set via the constructor and only used during load.</summary>
		private int _selectedTab;
		private List<Def> _listPaySplitUnearnedType;
		private CheckBox checkAllowFuturePayments;
		private GroupBox groupBoxClaimIdPrefix;
		private UI.Button butReplacements;
		private TextBox textClaimIdentifier;
		private CheckBox checkPreferredReferrals;
		private CheckBox checkProcNoteConcurrencyMerge;
		private TabControl tabControlAppts;
		private TabPage tabApptsPageBehavior;
		private TabPage tabApptsPageAppearance;
		private CheckBox checkSolidBlockouts;
		private CheckBox checkApptExclamation;
		private CheckBox checkApptBubbleDelay;
		private Label label23;
		private Label label25;
		private Button butColor;
		private Button butApptLineColor;
		private CheckBox checkAppointmentBubblesDisabled;
		private CheckBox checkApptRefreshEveryMinute;
		private Label labelApptWithoutProcsDefaultLength;
		private ValidNumber textApptWithoutProcsDefaultLength;
		private CheckBox checkAutoFillPatEmail;
		private CheckBox checkEraOneClaimPerPage;
		private CheckBox checkIsAlertRadiologyProcsEnabled;
		private Label labelClaimCredit;
		private ComboBox comboClaimCredit;
		private List<BrokenApptProcedure> _listComboBrokenApptProcOptions=new List<BrokenApptProcedure>();
		private CheckBox checkHidePaysplits;
		private CheckBox checkAgeNegAdjsByAdjDate;
		private CheckBox checkShowAllocateUnearnedPaymentPrompt;
		private CheckBox checkAllowPatsAtHQ;
		private ComboBox comboRigorousAdjustments;
		private Label label41;
		private TabControl tabControl1;
		private TabPage tabChartBehavior;
		private TabPage tabChartAppearance;
		private ComboBox comboToothNomenclature;
		private Label labelToothNomenclature;
		private CheckBox checkShowPlannedApptPrompt;
		private GroupBox groupCommLogs;
		private CheckBox checkCommLogAutoSave;
		private ComboBox comboPayPlanAdj;
		private Label label42;
		private CheckBox checkAllowFutureTrans;
		private CheckBox checkAllowPrepayProvider;
		private CheckBox checkShowAutoDeposit;
		private ValidNumber textClaimsReceivedDays;
		private List<Def> _listApptConfirmedDefs;

		///<summary>Default constructor.  Opens the form with the Appts tab selected.</summary>
		public FormModuleSetup():this(0) {
		}

		///<summary>Opens the form with the a specific tab selected.  Currently 0-6 are the only valid values.  Defaults to Appts tab if invalid value passed in.</summary>
		///<param name="selectedTab">0=Appts, 1=Family, 2=Account, 3=Treat' Plan, 4=Chart, 5=Images, 6=Manage</param>
		public FormModuleSetup(int selectedTab) {
			InitializeComponent();
			Lan.F(this);
			if(selectedTab<0 || selectedTab>6) {
				selectedTab=0;//Default to Appts tab.
			}
			_selectedTab=selectedTab;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormModuleSetup));
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.checkIsAlertRadiologyProcsEnabled = new System.Windows.Forms.CheckBox();
			this.checkImagesModuleTreeIsCollapsed = new System.Windows.Forms.CheckBox();
			this.checkApptsCheckFrequency = new System.Windows.Forms.CheckBox();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.tabControlMain = new System.Windows.Forms.TabControl();
			this.tabAppts = new System.Windows.Forms.TabPage();
			this.tabControlAppts = new System.Windows.Forms.TabControl();
			this.tabApptsPageBehavior = new System.Windows.Forms.TabPage();
			this.textApptWithoutProcsDefaultLength = new OpenDental.ValidNumber();
			this.labelApptWithoutProcsDefaultLength = new System.Windows.Forms.Label();
			this.checkApptAllowEmptyComplete = new System.Windows.Forms.CheckBox();
			this.checkApptAllowFutureComplete = new System.Windows.Forms.CheckBox();
			this.comboTimeArrived = new System.Windows.Forms.ComboBox();
			this.checkApptsRequireProcs = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.checkApptModuleProductionUsesOps = new System.Windows.Forms.CheckBox();
			this.comboTimeSeated = new System.Windows.Forms.ComboBox();
			this.checkUseOpHygProv = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.checkApptModuleAdjInProd = new System.Windows.Forms.CheckBox();
			this.comboTimeDismissed = new System.Windows.Forms.ComboBox();
			this.checkApptTimeReset = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label37 = new System.Windows.Forms.Label();
			this.comboBrokenApptProc = new System.Windows.Forms.ComboBox();
			this.checkBrokenApptCommLog = new System.Windows.Forms.CheckBox();
			this.checkBrokenApptAdjustment = new System.Windows.Forms.CheckBox();
			this.comboBrokenApptAdjType = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.textWaitRoomWarn = new System.Windows.Forms.TextBox();
			this.checkAppointmentTimeIsLocked = new System.Windows.Forms.CheckBox();
			this.label22 = new System.Windows.Forms.Label();
			this.comboSearchBehavior = new System.Windows.Forms.ComboBox();
			this.textApptBubNoteLength = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.checkWaitingRoomFilterByView = new System.Windows.Forms.CheckBox();
			this.tabApptsPageAppearance = new System.Windows.Forms.TabPage();
			this.checkApptRefreshEveryMinute = new System.Windows.Forms.CheckBox();
			this.checkAppointmentBubblesDisabled = new System.Windows.Forms.CheckBox();
			this.comboDelay = new System.Windows.Forms.ComboBox();
			this.butColor = new System.Windows.Forms.Button();
			this.butApptLineColor = new System.Windows.Forms.Button();
			this.checkSolidBlockouts = new System.Windows.Forms.CheckBox();
			this.checkApptExclamation = new System.Windows.Forms.CheckBox();
			this.checkApptBubbleDelay = new System.Windows.Forms.CheckBox();
			this.label23 = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.checkApptModuleDefaultToWeek = new System.Windows.Forms.CheckBox();
			this.apptClickDelay = new System.Windows.Forms.Label();
			this.tabFamily = new System.Windows.Forms.TabPage();
			this.tabControlFamily = new System.Windows.Forms.TabControl();
			this.tabPageFamGen = new System.Windows.Forms.TabPage();
			this.checkAllowPatsAtHQ = new System.Windows.Forms.CheckBox();
			this.checkAutoFillPatEmail = new System.Windows.Forms.CheckBox();
			this.checkPreferredReferrals = new System.Windows.Forms.CheckBox();
			this.checkTextMsgOkStatusTreatAsNo = new System.Windows.Forms.CheckBox();
			this.checkPatInitBillingTypeFromPriInsPlan = new System.Windows.Forms.CheckBox();
			this.checkFamPhiAccess = new System.Windows.Forms.CheckBox();
			this.checkClaimTrackingRequireError = new System.Windows.Forms.CheckBox();
			this.checkPPOpercentage = new System.Windows.Forms.CheckBox();
			this.checkClaimUseOverrideProcDescript = new System.Windows.Forms.CheckBox();
			this.checkSelectProv = new System.Windows.Forms.CheckBox();
			this.checkAllowedFeeSchedsAutomate = new System.Windows.Forms.CheckBox();
			this.checkInsPPOsecWriteoffs = new System.Windows.Forms.CheckBox();
			this.checkInsurancePlansShared = new System.Windows.Forms.CheckBox();
			this.checkCoPayFeeScheduleBlankLikeZero = new System.Windows.Forms.CheckBox();
			this.checkInsDefaultShowUCRonClaims = new System.Windows.Forms.CheckBox();
			this.checkGoogleAddress = new System.Windows.Forms.CheckBox();
			this.comboCobRule = new System.Windows.Forms.ComboBox();
			this.checkInsDefaultAssignmentOfBenefits = new System.Windows.Forms.CheckBox();
			this.label15 = new System.Windows.Forms.Label();
			this.tabPageSuperFam = new System.Windows.Forms.TabPage();
			this.checkSuperFamCloneCreate = new System.Windows.Forms.CheckBox();
			this.comboSuperFamSort = new System.Windows.Forms.ComboBox();
			this.checkSuperFamAddIns = new System.Windows.Forms.CheckBox();
			this.checkSuperFamSync = new System.Windows.Forms.CheckBox();
			this.labelSuperFamSort = new System.Windows.Forms.Label();
			this.tabPageClaimSnapShot = new System.Windows.Forms.TabPage();
			this.label31 = new System.Windows.Forms.Label();
			this.label30 = new System.Windows.Forms.Label();
			this.comboClaimSnapshotTrigger = new System.Windows.Forms.ComboBox();
			this.textClaimSnapshotRunTime = new System.Windows.Forms.TextBox();
			this.tabAccount = new System.Windows.Forms.TabPage();
			this.tabControlAccount = new System.Windows.Forms.TabControl();
			this.tabPagePayAdj = new System.Windows.Forms.TabPage();
			this.checkAllowPrepayProvider = new System.Windows.Forms.CheckBox();
			this.comboPayPlanAdj = new System.Windows.Forms.ComboBox();
			this.label42 = new System.Windows.Forms.Label();
			this.comboRigorousAdjustments = new System.Windows.Forms.ComboBox();
			this.label41 = new System.Windows.Forms.Label();
			this.checkHidePaysplits = new System.Windows.Forms.CheckBox();
			this.label40 = new System.Windows.Forms.Label();
			this.comboAutoSplitPref = new System.Windows.Forms.ComboBox();
			this.comboRigorousAccounting = new System.Windows.Forms.ComboBox();
			this.label39 = new System.Windows.Forms.Label();
			this.checkAllowEmailCCReceipt = new System.Windows.Forms.CheckBox();
			this.label38 = new System.Windows.Forms.Label();
			this.comboPaymentClinicSetting = new System.Windows.Forms.ComboBox();
			this.checkAllowFutureDebits = new System.Windows.Forms.CheckBox();
			this.checkStoreCCTokens = new System.Windows.Forms.CheckBox();
			this.textTaxPercent = new System.Windows.Forms.TextBox();
			this.label26 = new System.Windows.Forms.Label();
			this.comboSalesTaxAdjType = new System.Windows.Forms.ComboBox();
			this.label33 = new System.Windows.Forms.Label();
			this.comboBillingChargeAdjType = new System.Windows.Forms.ComboBox();
			this.label12 = new System.Windows.Forms.Label();
			this.comboFinanceChargeAdjType = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.listboxBadDebtAdjs = new System.Windows.Forms.ListBox();
			this.label29 = new System.Windows.Forms.Label();
			this.butBadDebt = new OpenDental.UI.Button();
			this.checkPaymentsPromptForPayType = new System.Windows.Forms.CheckBox();
			this.comboUnallocatedSplits = new System.Windows.Forms.ComboBox();
			this.label28 = new System.Windows.Forms.Label();
			this.tabPageIns = new System.Windows.Forms.TabPage();
			this.labelClaimCredit = new System.Windows.Forms.Label();
			this.comboClaimCredit = new System.Windows.Forms.ComboBox();
			this.checkAllowFuturePayments = new System.Windows.Forms.CheckBox();
			this.groupBoxClaimIdPrefix = new System.Windows.Forms.GroupBox();
			this.butReplacements = new OpenDental.UI.Button();
			this.textClaimIdentifier = new System.Windows.Forms.TextBox();
			this.checkAllowProcAdjFromClaim = new System.Windows.Forms.CheckBox();
			this.checkProviderIncomeShows = new System.Windows.Forms.CheckBox();
			this.checkClaimFormTreatDentSaysSigOnFile = new System.Windows.Forms.CheckBox();
			this.checkClaimMedTypeIsInstWhenInsPlanIsMedical = new System.Windows.Forms.CheckBox();
			this.checkEclaimsMedicalProvTreatmentAsOrdering = new System.Windows.Forms.CheckBox();
			this.checkEclaimsSeparateTreatProv = new System.Windows.Forms.CheckBox();
			this.label20 = new System.Windows.Forms.Label();
			this.textClaimAttachPath = new System.Windows.Forms.TextBox();
			this.checkClaimsValidateACN = new System.Windows.Forms.CheckBox();
			this.textInsWriteoffDescript = new System.Windows.Forms.TextBox();
			this.label17 = new System.Windows.Forms.Label();
			this.tabPageMisc = new System.Windows.Forms.TabPage();
			this.checkAllowFutureTrans = new System.Windows.Forms.CheckBox();
			this.groupCommLogs = new System.Windows.Forms.GroupBox();
			this.checkCommLogAutoSave = new System.Windows.Forms.CheckBox();
			this.checkShowFamilyCommByDefault = new System.Windows.Forms.CheckBox();
			this.checkShowAllocateUnearnedPaymentPrompt = new System.Windows.Forms.CheckBox();
			this.checkAgeNegAdjsByAdjDate = new System.Windows.Forms.CheckBox();
			this.groupPayPlans = new System.Windows.Forms.GroupBox();
			this.label27 = new System.Windows.Forms.Label();
			this.comboPayPlansVersion = new System.Windows.Forms.ComboBox();
			this.checkHideDueNow = new System.Windows.Forms.CheckBox();
			this.checkPayPlansUseSheets = new System.Windows.Forms.CheckBox();
			this.checkPayPlansExcludePastActivity = new System.Windows.Forms.CheckBox();
			this.checkStatementInvoiceGridShowWriteoffs = new System.Windows.Forms.CheckBox();
			this.checkBalancesDontSubtractIns = new System.Windows.Forms.CheckBox();
			this.checkAgingMonthly = new System.Windows.Forms.CheckBox();
			this.checkAccountShowPaymentNums = new System.Windows.Forms.CheckBox();
			this.checkRecurringChargesUseTransDate = new System.Windows.Forms.CheckBox();
			this.checkStatementsUseSheets = new System.Windows.Forms.CheckBox();
			this.checkRecurChargPriProv = new System.Windows.Forms.CheckBox();
			this.checkPpoUseUcr = new System.Windows.Forms.CheckBox();
			this.tabTreatPlan = new System.Windows.Forms.TabPage();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.textInsExam = new System.Windows.Forms.TextBox();
			this.label35 = new System.Windows.Forms.Label();
			this.checkFrequency = new System.Windows.Forms.CheckBox();
			this.textInsBW = new System.Windows.Forms.TextBox();
			this.label34 = new System.Windows.Forms.Label();
			this.textInsPano = new System.Windows.Forms.TextBox();
			this.label36 = new System.Windows.Forms.Label();
			this.groupTreatPlanSort = new System.Windows.Forms.GroupBox();
			this.radioTreatPlanSortTooth = new System.Windows.Forms.RadioButton();
			this.radioTreatPlanSortOrder = new System.Windows.Forms.RadioButton();
			this.checkTPSaveSigned = new System.Windows.Forms.CheckBox();
			this.checkTreatPlanItemized = new System.Windows.Forms.CheckBox();
			this.textDiscountPercentage = new System.Windows.Forms.TextBox();
			this.labelDiscountPercentage = new System.Windows.Forms.Label();
			this.comboProcDiscountType = new System.Windows.Forms.ComboBox();
			this.label19 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.checkTreatPlanShowCompleted = new System.Windows.Forms.CheckBox();
			this.textTreatNote = new OpenDental.ODtextBox();
			this.tabChart = new System.Windows.Forms.TabPage();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabChartBehavior = new System.Windows.Forms.TabPage();
			this.checkShowPlannedApptPrompt = new System.Windows.Forms.CheckBox();
			this.checkAllowSettingProcsComplete = new System.Windows.Forms.CheckBox();
			this.textProblemsIndicateNone = new System.Windows.Forms.TextBox();
			this.checkBoxRxClinicUseSelected = new System.Windows.Forms.CheckBox();
			this.label8 = new System.Windows.Forms.Label();
			this.checkProcProvChangesCp = new System.Windows.Forms.CheckBox();
			this.comboProcFeeUpdatePrompt = new System.Windows.Forms.ComboBox();
			this.labelProcFeeUpdatePrompt = new System.Windows.Forms.Label();
			this.butProblemsIndicateNone = new OpenDental.UI.Button();
			this.label9 = new System.Windows.Forms.Label();
			this.textMedicationsIndicateNone = new System.Windows.Forms.TextBox();
			this.checkSignatureAllowDigital = new System.Windows.Forms.CheckBox();
			this.butMedicationsIndicateNone = new OpenDental.UI.Button();
			this.checkClaimProcsAllowEstimatesOnCompl = new System.Windows.Forms.CheckBox();
			this.checkProcEditRequireAutoCode = new System.Windows.Forms.CheckBox();
			this.label14 = new System.Windows.Forms.Label();
			this.checkProcsPromptForAutoNote = new System.Windows.Forms.CheckBox();
			this.textAllergiesIndicateNone = new System.Windows.Forms.TextBox();
			this.checkScreeningsUseSheets = new System.Windows.Forms.CheckBox();
			this.butAllergiesIndicateNone = new OpenDental.UI.Button();
			this.checkMedicalFeeUsedForNewProcs = new System.Windows.Forms.CheckBox();
			this.checkDxIcdVersion = new System.Windows.Forms.CheckBox();
			this.butDiagnosisCode = new OpenDental.UI.Button();
			this.labelIcdCodeDefault = new System.Windows.Forms.Label();
			this.textICD9DefaultForNewProcs = new System.Windows.Forms.TextBox();
			this.checkProcLockingIsAllowed = new System.Windows.Forms.CheckBox();
			this.checkChartNonPatientWarn = new System.Windows.Forms.CheckBox();
			this.label11 = new System.Windows.Forms.Label();
			this.textMedDefaultStopDays = new System.Windows.Forms.TextBox();
			this.tabChartAppearance = new System.Windows.Forms.TabPage();
			this.comboToothNomenclature = new System.Windows.Forms.ComboBox();
			this.checkProcNoteConcurrencyMerge = new System.Windows.Forms.CheckBox();
			this.labelToothNomenclature = new System.Windows.Forms.Label();
			this.checkAutoClearEntryStatus = new System.Windows.Forms.CheckBox();
			this.checkProcGroupNoteDoesAggregate = new System.Windows.Forms.CheckBox();
			this.checkProvColorChart = new System.Windows.Forms.CheckBox();
			this.checkPerioSkipMissingTeeth = new System.Windows.Forms.CheckBox();
			this.checkPerioTreatImplantsAsNotMissing = new System.Windows.Forms.CheckBox();
			this.comboProcCodeListSort = new System.Windows.Forms.ComboBox();
			this.label32 = new System.Windows.Forms.Label();
			this.tabImages = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.radioImagesModuleTreeIsPersistentPerUser = new System.Windows.Forms.RadioButton();
			this.radioImagesModuleTreeIsCollapsed = new System.Windows.Forms.RadioButton();
			this.radioImagesModuleTreeIsExpanded = new System.Windows.Forms.RadioButton();
			this.tabManage = new System.Windows.Forms.TabPage();
			this.textClaimsReceivedDays = new OpenDental.ValidNumber();
			this.checkShowAutoDeposit = new System.Windows.Forms.CheckBox();
			this.checkEraOneClaimPerPage = new System.Windows.Forms.CheckBox();
			this.checkClaimPaymentBatchOnly = new System.Windows.Forms.CheckBox();
			this.labelClaimsReceivedDays = new System.Windows.Forms.Label();
			this.checkScheduleProvEmpSelectAll = new System.Windows.Forms.CheckBox();
			this.checkClaimsSendWindowValidateOnLoad = new System.Windows.Forms.CheckBox();
			this.checkTimeCardADP = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkBillingShowProgress = new System.Windows.Forms.CheckBox();
			this.label24 = new System.Windows.Forms.Label();
			this.textBillingElectBatchMax = new OpenDental.ValidNum();
			this.checkStatementShowAdjNotes = new System.Windows.Forms.CheckBox();
			this.checkIntermingleDefault = new System.Windows.Forms.CheckBox();
			this.checkStatementShowReturnAddress = new System.Windows.Forms.CheckBox();
			this.checkStatementShowProcBreakdown = new System.Windows.Forms.CheckBox();
			this.checkShowCC = new System.Windows.Forms.CheckBox();
			this.checkStatementShowNotes = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.comboUseChartNum = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.textStatementsCalcDueDate = new OpenDental.ValidNumber();
			this.textPayPlansBillInAdvanceDays = new OpenDental.ValidNum();
			this.comboTimeCardOvertimeFirstDayOfWeek = new System.Windows.Forms.ComboBox();
			this.label16 = new System.Windows.Forms.Label();
			this.checkRxSendNewToQueue = new System.Windows.Forms.CheckBox();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.tabControlMain.SuspendLayout();
			this.tabAppts.SuspendLayout();
			this.tabControlAppts.SuspendLayout();
			this.tabApptsPageBehavior.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabApptsPageAppearance.SuspendLayout();
			this.tabFamily.SuspendLayout();
			this.tabControlFamily.SuspendLayout();
			this.tabPageFamGen.SuspendLayout();
			this.tabPageSuperFam.SuspendLayout();
			this.tabPageClaimSnapShot.SuspendLayout();
			this.tabAccount.SuspendLayout();
			this.tabControlAccount.SuspendLayout();
			this.tabPagePayAdj.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.tabPageIns.SuspendLayout();
			this.groupBoxClaimIdPrefix.SuspendLayout();
			this.tabPageMisc.SuspendLayout();
			this.groupCommLogs.SuspendLayout();
			this.groupPayPlans.SuspendLayout();
			this.tabTreatPlan.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.groupTreatPlanSort.SuspendLayout();
			this.tabChart.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabChartBehavior.SuspendLayout();
			this.tabChartAppearance.SuspendLayout();
			this.tabImages.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tabManage.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolTip1
			// 
			this.toolTip1.AutomaticDelay = 0;
			this.toolTip1.AutoPopDelay = 600000;
			this.toolTip1.InitialDelay = 0;
			this.toolTip1.IsBalloon = true;
			this.toolTip1.ReshowDelay = 0;
			this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			this.toolTip1.ToolTipTitle = "Help";
			// 
			// checkIsAlertRadiologyProcsEnabled
			// 
			this.checkIsAlertRadiologyProcsEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsAlertRadiologyProcsEnabled.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsAlertRadiologyProcsEnabled.Location = new System.Drawing.Point(6, 394);
			this.checkIsAlertRadiologyProcsEnabled.Name = "checkIsAlertRadiologyProcsEnabled";
			this.checkIsAlertRadiologyProcsEnabled.Size = new System.Drawing.Size(435, 17);
			this.checkIsAlertRadiologyProcsEnabled.TabIndex = 229;
			this.checkIsAlertRadiologyProcsEnabled.Text = "OpenDentalService alerts for scheduled non-CPOE radiology procedures";
			this.checkIsAlertRadiologyProcsEnabled.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsAlertRadiologyProcsEnabled.UseVisualStyleBackColor = true;
			// 
			// checkImagesModuleTreeIsCollapsed
			// 
			this.checkImagesModuleTreeIsCollapsed.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkImagesModuleTreeIsCollapsed.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkImagesModuleTreeIsCollapsed.Location = new System.Drawing.Point(81, 7);
			this.checkImagesModuleTreeIsCollapsed.Name = "checkImagesModuleTreeIsCollapsed";
			this.checkImagesModuleTreeIsCollapsed.Size = new System.Drawing.Size(359, 17);
			this.checkImagesModuleTreeIsCollapsed.TabIndex = 47;
			this.checkImagesModuleTreeIsCollapsed.Text = "Document tree collapses when patient changes";
			this.checkImagesModuleTreeIsCollapsed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkApptsCheckFrequency
			// 
			this.checkApptsCheckFrequency.Location = new System.Drawing.Point(0, 0);
			this.checkApptsCheckFrequency.Name = "checkApptsCheckFrequency";
			this.checkApptsCheckFrequency.Size = new System.Drawing.Size(104, 24);
			this.checkApptsCheckFrequency.TabIndex = 0;
			// 
			// tabControlMain
			// 
			this.tabControlMain.Controls.Add(this.tabAppts);
			this.tabControlMain.Controls.Add(this.tabFamily);
			this.tabControlMain.Controls.Add(this.tabAccount);
			this.tabControlMain.Controls.Add(this.tabTreatPlan);
			this.tabControlMain.Controls.Add(this.tabChart);
			this.tabControlMain.Controls.Add(this.tabImages);
			this.tabControlMain.Controls.Add(this.tabManage);
			this.tabControlMain.Location = new System.Drawing.Point(30, 10);
			this.tabControlMain.Name = "tabControlMain";
			this.tabControlMain.SelectedIndex = 0;
			this.tabControlMain.Size = new System.Drawing.Size(479, 605);
			this.tabControlMain.TabIndex = 196;
			// 
			// tabAppts
			// 
			this.tabAppts.BackColor = System.Drawing.SystemColors.Window;
			this.tabAppts.Controls.Add(this.tabControlAppts);
			this.tabAppts.Location = new System.Drawing.Point(4, 22);
			this.tabAppts.Name = "tabAppts";
			this.tabAppts.Padding = new System.Windows.Forms.Padding(3);
			this.tabAppts.Size = new System.Drawing.Size(471, 579);
			this.tabAppts.TabIndex = 0;
			this.tabAppts.Text = "Appts";
			// 
			// tabControlAppts
			// 
			this.tabControlAppts.Controls.Add(this.tabApptsPageBehavior);
			this.tabControlAppts.Controls.Add(this.tabApptsPageAppearance);
			this.tabControlAppts.Location = new System.Drawing.Point(3, 3);
			this.tabControlAppts.Name = "tabControlAppts";
			this.tabControlAppts.SelectedIndex = 0;
			this.tabControlAppts.Size = new System.Drawing.Size(462, 573);
			this.tabControlAppts.TabIndex = 234;
			// 
			// tabApptsPageBehavior
			// 
			this.tabApptsPageBehavior.Controls.Add(this.textApptWithoutProcsDefaultLength);
			this.tabApptsPageBehavior.Controls.Add(this.labelApptWithoutProcsDefaultLength);
			this.tabApptsPageBehavior.Controls.Add(this.checkApptAllowEmptyComplete);
			this.tabApptsPageBehavior.Controls.Add(this.checkApptAllowFutureComplete);
			this.tabApptsPageBehavior.Controls.Add(this.comboTimeArrived);
			this.tabApptsPageBehavior.Controls.Add(this.checkApptsRequireProcs);
			this.tabApptsPageBehavior.Controls.Add(this.label3);
			this.tabApptsPageBehavior.Controls.Add(this.checkApptModuleProductionUsesOps);
			this.tabApptsPageBehavior.Controls.Add(this.comboTimeSeated);
			this.tabApptsPageBehavior.Controls.Add(this.checkUseOpHygProv);
			this.tabApptsPageBehavior.Controls.Add(this.label5);
			this.tabApptsPageBehavior.Controls.Add(this.checkApptModuleAdjInProd);
			this.tabApptsPageBehavior.Controls.Add(this.comboTimeDismissed);
			this.tabApptsPageBehavior.Controls.Add(this.checkApptTimeReset);
			this.tabApptsPageBehavior.Controls.Add(this.label6);
			this.tabApptsPageBehavior.Controls.Add(this.groupBox2);
			this.tabApptsPageBehavior.Controls.Add(this.textWaitRoomWarn);
			this.tabApptsPageBehavior.Controls.Add(this.checkAppointmentTimeIsLocked);
			this.tabApptsPageBehavior.Controls.Add(this.label22);
			this.tabApptsPageBehavior.Controls.Add(this.comboSearchBehavior);
			this.tabApptsPageBehavior.Controls.Add(this.textApptBubNoteLength);
			this.tabApptsPageBehavior.Controls.Add(this.label13);
			this.tabApptsPageBehavior.Controls.Add(this.label21);
			this.tabApptsPageBehavior.Controls.Add(this.checkWaitingRoomFilterByView);
			this.tabApptsPageBehavior.Location = new System.Drawing.Point(4, 22);
			this.tabApptsPageBehavior.Name = "tabApptsPageBehavior";
			this.tabApptsPageBehavior.Padding = new System.Windows.Forms.Padding(3);
			this.tabApptsPageBehavior.Size = new System.Drawing.Size(454, 547);
			this.tabApptsPageBehavior.TabIndex = 0;
			this.tabApptsPageBehavior.Text = "Behavior";
			this.tabApptsPageBehavior.UseVisualStyleBackColor = true;
			// 
			// textApptWithoutProcsDefaultLength
			// 
			this.textApptWithoutProcsDefaultLength.Location = new System.Drawing.Point(332, 381);
			this.textApptWithoutProcsDefaultLength.MaxVal = 600;
			this.textApptWithoutProcsDefaultLength.MinVal = 0;
			this.textApptWithoutProcsDefaultLength.Name = "textApptWithoutProcsDefaultLength";
			this.textApptWithoutProcsDefaultLength.Size = new System.Drawing.Size(100, 20);
			this.textApptWithoutProcsDefaultLength.TabIndex = 234;
			// 
			// labelApptWithoutProcsDefaultLength
			// 
			this.labelApptWithoutProcsDefaultLength.Location = new System.Drawing.Point(11, 384);
			this.labelApptWithoutProcsDefaultLength.Name = "labelApptWithoutProcsDefaultLength";
			this.labelApptWithoutProcsDefaultLength.Size = new System.Drawing.Size(319, 16);
			this.labelApptWithoutProcsDefaultLength.TabIndex = 232;
			this.labelApptWithoutProcsDefaultLength.Text = "Appointment without procedures default length";
			this.labelApptWithoutProcsDefaultLength.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkApptAllowEmptyComplete
			// 
			this.checkApptAllowEmptyComplete.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptAllowEmptyComplete.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkApptAllowEmptyComplete.Location = new System.Drawing.Point(25, 423);
			this.checkApptAllowEmptyComplete.Name = "checkApptAllowEmptyComplete";
			this.checkApptAllowEmptyComplete.Size = new System.Drawing.Size(406, 17);
			this.checkApptAllowEmptyComplete.TabIndex = 231;
			this.checkApptAllowEmptyComplete.Text = "Allow setting appointments without procedures complete";
			this.checkApptAllowEmptyComplete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkApptAllowFutureComplete
			// 
			this.checkApptAllowFutureComplete.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptAllowFutureComplete.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkApptAllowFutureComplete.Location = new System.Drawing.Point(25, 405);
			this.checkApptAllowFutureComplete.Name = "checkApptAllowFutureComplete";
			this.checkApptAllowFutureComplete.Size = new System.Drawing.Size(406, 17);
			this.checkApptAllowFutureComplete.TabIndex = 230;
			this.checkApptAllowFutureComplete.Text = "Allow setting future appointments complete";
			this.checkApptAllowFutureComplete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboTimeArrived
			// 
			this.comboTimeArrived.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTimeArrived.FormattingEnabled = true;
			this.comboTimeArrived.Location = new System.Drawing.Point(268, 108);
			this.comboTimeArrived.MaxDropDownItems = 30;
			this.comboTimeArrived.Name = "comboTimeArrived";
			this.comboTimeArrived.Size = new System.Drawing.Size(163, 21);
			this.comboTimeArrived.TabIndex = 73;
			// 
			// checkApptsRequireProcs
			// 
			this.checkApptsRequireProcs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptsRequireProcs.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkApptsRequireProcs.Location = new System.Drawing.Point(25, 361);
			this.checkApptsRequireProcs.Name = "checkApptsRequireProcs";
			this.checkApptsRequireProcs.Size = new System.Drawing.Size(406, 17);
			this.checkApptsRequireProcs.TabIndex = 229;
			this.checkApptsRequireProcs.Text = "Appointments require procedures";
			this.checkApptsRequireProcs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptsRequireProcs.CheckedChanged += new System.EventHandler(this.checkApptsRequireProcs_CheckedChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(19, 112);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(247, 15);
			this.label3.TabIndex = 74;
			this.label3.Text = "Time Arrived trigger";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkApptModuleProductionUsesOps
			// 
			this.checkApptModuleProductionUsesOps.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptModuleProductionUsesOps.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkApptModuleProductionUsesOps.Location = new System.Drawing.Point(25, 343);
			this.checkApptModuleProductionUsesOps.Name = "checkApptModuleProductionUsesOps";
			this.checkApptModuleProductionUsesOps.Size = new System.Drawing.Size(406, 17);
			this.checkApptModuleProductionUsesOps.TabIndex = 228;
			this.checkApptModuleProductionUsesOps.Text = "Appointment module production use operatories";
			this.checkApptModuleProductionUsesOps.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboTimeSeated
			// 
			this.comboTimeSeated.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTimeSeated.FormattingEnabled = true;
			this.comboTimeSeated.Location = new System.Drawing.Point(268, 130);
			this.comboTimeSeated.MaxDropDownItems = 30;
			this.comboTimeSeated.Name = "comboTimeSeated";
			this.comboTimeSeated.Size = new System.Drawing.Size(163, 21);
			this.comboTimeSeated.TabIndex = 75;
			// 
			// checkUseOpHygProv
			// 
			this.checkUseOpHygProv.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUseOpHygProv.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkUseOpHygProv.Location = new System.Drawing.Point(25, 325);
			this.checkUseOpHygProv.Name = "checkUseOpHygProv";
			this.checkUseOpHygProv.Size = new System.Drawing.Size(406, 17);
			this.checkUseOpHygProv.TabIndex = 226;
			this.checkUseOpHygProv.Text = "Force op\'s hygiene provider as secondary provider";
			this.checkUseOpHygProv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(19, 134);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(247, 15);
			this.label5.TabIndex = 76;
			this.label5.Text = "Time Seated (in op) trigger";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkApptModuleAdjInProd
			// 
			this.checkApptModuleAdjInProd.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptModuleAdjInProd.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkApptModuleAdjInProd.Location = new System.Drawing.Point(25, 307);
			this.checkApptModuleAdjInProd.Name = "checkApptModuleAdjInProd";
			this.checkApptModuleAdjInProd.Size = new System.Drawing.Size(406, 17);
			this.checkApptModuleAdjInProd.TabIndex = 224;
			this.checkApptModuleAdjInProd.Text = "Add daily adjustments to net production";
			this.checkApptModuleAdjInProd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboTimeDismissed
			// 
			this.comboTimeDismissed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTimeDismissed.FormattingEnabled = true;
			this.comboTimeDismissed.Location = new System.Drawing.Point(268, 152);
			this.comboTimeDismissed.MaxDropDownItems = 30;
			this.comboTimeDismissed.Name = "comboTimeDismissed";
			this.comboTimeDismissed.Size = new System.Drawing.Size(163, 21);
			this.comboTimeDismissed.TabIndex = 77;
			// 
			// checkApptTimeReset
			// 
			this.checkApptTimeReset.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptTimeReset.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkApptTimeReset.Location = new System.Drawing.Point(25, 289);
			this.checkApptTimeReset.Name = "checkApptTimeReset";
			this.checkApptTimeReset.Size = new System.Drawing.Size(406, 17);
			this.checkApptTimeReset.TabIndex = 223;
			this.checkApptTimeReset.Text = "Reset Calendar to Today on Clinic Select";
			this.checkApptTimeReset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(19, 156);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(247, 15);
			this.label6.TabIndex = 78;
			this.label6.Text = "Time Dismissed trigger";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label37);
			this.groupBox2.Controls.Add(this.comboBrokenApptProc);
			this.groupBox2.Controls.Add(this.checkBrokenApptCommLog);
			this.groupBox2.Controls.Add(this.checkBrokenApptAdjustment);
			this.groupBox2.Controls.Add(this.comboBrokenApptAdjType);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Location = new System.Drawing.Point(25, 8);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(418, 93);
			this.groupBox2.TabIndex = 222;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Broken Appointment Automation";
			// 
			// label37
			// 
			this.label37.Location = new System.Drawing.Point(2, 15);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(240, 15);
			this.label37.TabIndex = 235;
			this.label37.Text = "Broken appointment procedure types";
			this.label37.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboBrokenApptProc
			// 
			this.comboBrokenApptProc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBrokenApptProc.FormattingEnabled = true;
			this.comboBrokenApptProc.Location = new System.Drawing.Point(244, 11);
			this.comboBrokenApptProc.MaxDropDownItems = 30;
			this.comboBrokenApptProc.Name = "comboBrokenApptProc";
			this.comboBrokenApptProc.Size = new System.Drawing.Size(162, 21);
			this.comboBrokenApptProc.TabIndex = 234;
			// 
			// checkBrokenApptCommLog
			// 
			this.checkBrokenApptCommLog.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBrokenApptCommLog.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBrokenApptCommLog.Location = new System.Drawing.Point(21, 32);
			this.checkBrokenApptCommLog.Name = "checkBrokenApptCommLog";
			this.checkBrokenApptCommLog.Size = new System.Drawing.Size(385, 17);
			this.checkBrokenApptCommLog.TabIndex = 61;
			this.checkBrokenApptCommLog.Text = "Make broken appointment Commlog";
			this.checkBrokenApptCommLog.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBrokenApptCommLog.UseVisualStyleBackColor = true;
			// 
			// checkBrokenApptAdjustment
			// 
			this.checkBrokenApptAdjustment.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBrokenApptAdjustment.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBrokenApptAdjustment.Location = new System.Drawing.Point(21, 48);
			this.checkBrokenApptAdjustment.Name = "checkBrokenApptAdjustment";
			this.checkBrokenApptAdjustment.Size = new System.Drawing.Size(385, 17);
			this.checkBrokenApptAdjustment.TabIndex = 217;
			this.checkBrokenApptAdjustment.Text = "Make broken appointment Adjustment";
			this.checkBrokenApptAdjustment.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBrokenApptAdjustment.UseVisualStyleBackColor = true;
			// 
			// comboBrokenApptAdjType
			// 
			this.comboBrokenApptAdjType.FormattingEnabled = true;
			this.comboBrokenApptAdjType.Location = new System.Drawing.Point(204, 66);
			this.comboBrokenApptAdjType.MaxDropDownItems = 30;
			this.comboBrokenApptAdjType.Name = "comboBrokenApptAdjType";
			this.comboBrokenApptAdjType.Size = new System.Drawing.Size(203, 21);
			this.comboBrokenApptAdjType.TabIndex = 70;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(6, 69);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(197, 15);
			this.label7.TabIndex = 71;
			this.label7.Text = "Broken appt default adj type";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textWaitRoomWarn
			// 
			this.textWaitRoomWarn.Location = new System.Drawing.Point(348, 263);
			this.textWaitRoomWarn.Name = "textWaitRoomWarn";
			this.textWaitRoomWarn.Size = new System.Drawing.Size(83, 20);
			this.textWaitRoomWarn.TabIndex = 214;
			// 
			// checkAppointmentTimeIsLocked
			// 
			this.checkAppointmentTimeIsLocked.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAppointmentTimeIsLocked.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAppointmentTimeIsLocked.Location = new System.Drawing.Point(24, 199);
			this.checkAppointmentTimeIsLocked.Name = "checkAppointmentTimeIsLocked";
			this.checkAppointmentTimeIsLocked.Size = new System.Drawing.Size(406, 17);
			this.checkAppointmentTimeIsLocked.TabIndex = 198;
			this.checkAppointmentTimeIsLocked.Text = "Appointment time locked by default";
			this.checkAppointmentTimeIsLocked.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAppointmentTimeIsLocked.MouseUp += new System.Windows.Forms.MouseEventHandler(this.checkAppointmentTimeIsLocked_MouseUp);
			// 
			// label22
			// 
			this.label22.BackColor = System.Drawing.Color.White;
			this.label22.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label22.Location = new System.Drawing.Point(99, 266);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(246, 16);
			this.label22.TabIndex = 213;
			this.label22.Text = "Waiting room alert time in minutes (0 to disable)";
			this.label22.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboSearchBehavior
			// 
			this.comboSearchBehavior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSearchBehavior.FormattingEnabled = true;
			this.comboSearchBehavior.Location = new System.Drawing.Point(228, 174);
			this.comboSearchBehavior.MaxDropDownItems = 30;
			this.comboSearchBehavior.Name = "comboSearchBehavior";
			this.comboSearchBehavior.Size = new System.Drawing.Size(203, 21);
			this.comboSearchBehavior.TabIndex = 199;
			// 
			// textApptBubNoteLength
			// 
			this.textApptBubNoteLength.Location = new System.Drawing.Point(347, 218);
			this.textApptBubNoteLength.Name = "textApptBubNoteLength";
			this.textApptBubNoteLength.Size = new System.Drawing.Size(83, 20);
			this.textApptBubNoteLength.TabIndex = 211;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(10, 177);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(217, 15);
			this.label13.TabIndex = 200;
			this.label13.Text = "Search Behavior";
			this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(99, 221);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(246, 16);
			this.label21.TabIndex = 210;
			this.label21.Text = "Appointment bubble max note length (0 for no limit)";
			this.label21.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkWaitingRoomFilterByView
			// 
			this.checkWaitingRoomFilterByView.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkWaitingRoomFilterByView.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkWaitingRoomFilterByView.Location = new System.Drawing.Point(24, 240);
			this.checkWaitingRoomFilterByView.Name = "checkWaitingRoomFilterByView";
			this.checkWaitingRoomFilterByView.Size = new System.Drawing.Size(406, 17);
			this.checkWaitingRoomFilterByView.TabIndex = 201;
			this.checkWaitingRoomFilterByView.Text = "Filter the waiting room based on the selected appointment view.";
			this.checkWaitingRoomFilterByView.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabApptsPageAppearance
			// 
			this.tabApptsPageAppearance.Controls.Add(this.checkApptRefreshEveryMinute);
			this.tabApptsPageAppearance.Controls.Add(this.checkAppointmentBubblesDisabled);
			this.tabApptsPageAppearance.Controls.Add(this.comboDelay);
			this.tabApptsPageAppearance.Controls.Add(this.butColor);
			this.tabApptsPageAppearance.Controls.Add(this.butApptLineColor);
			this.tabApptsPageAppearance.Controls.Add(this.checkSolidBlockouts);
			this.tabApptsPageAppearance.Controls.Add(this.checkApptExclamation);
			this.tabApptsPageAppearance.Controls.Add(this.checkApptBubbleDelay);
			this.tabApptsPageAppearance.Controls.Add(this.label23);
			this.tabApptsPageAppearance.Controls.Add(this.label25);
			this.tabApptsPageAppearance.Controls.Add(this.checkApptModuleDefaultToWeek);
			this.tabApptsPageAppearance.Controls.Add(this.apptClickDelay);
			this.tabApptsPageAppearance.Location = new System.Drawing.Point(4, 22);
			this.tabApptsPageAppearance.Name = "tabApptsPageAppearance";
			this.tabApptsPageAppearance.Padding = new System.Windows.Forms.Padding(3);
			this.tabApptsPageAppearance.Size = new System.Drawing.Size(454, 547);
			this.tabApptsPageAppearance.TabIndex = 1;
			this.tabApptsPageAppearance.Text = "Appearance";
			this.tabApptsPageAppearance.UseVisualStyleBackColor = true;
			// 
			// checkApptRefreshEveryMinute
			// 
			this.checkApptRefreshEveryMinute.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptRefreshEveryMinute.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkApptRefreshEveryMinute.Location = new System.Drawing.Point(27, 204);
			this.checkApptRefreshEveryMinute.Name = "checkApptRefreshEveryMinute";
			this.checkApptRefreshEveryMinute.Size = new System.Drawing.Size(406, 17);
			this.checkApptRefreshEveryMinute.TabIndex = 235;
			this.checkApptRefreshEveryMinute.Text = "Refresh every 60 seconds.  Keeps waiting room times refreshed.";
			this.checkApptRefreshEveryMinute.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkAppointmentBubblesDisabled
			// 
			this.checkAppointmentBubblesDisabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAppointmentBubblesDisabled.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAppointmentBubblesDisabled.Location = new System.Drawing.Point(6, 8);
			this.checkAppointmentBubblesDisabled.Name = "checkAppointmentBubblesDisabled";
			this.checkAppointmentBubblesDisabled.Size = new System.Drawing.Size(425, 17);
			this.checkAppointmentBubblesDisabled.TabIndex = 234;
			this.checkAppointmentBubblesDisabled.Text = "Default appointment bubble to disabled for new appointment views";
			this.checkAppointmentBubblesDisabled.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAppointmentBubblesDisabled.UseVisualStyleBackColor = true;
			// 
			// comboDelay
			// 
			this.comboDelay.AllowDrop = true;
			this.comboDelay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDelay.FormattingEnabled = true;
			this.comboDelay.Location = new System.Drawing.Point(319, 177);
			this.comboDelay.Name = "comboDelay";
			this.comboDelay.Size = new System.Drawing.Size(114, 21);
			this.comboDelay.TabIndex = 232;
			// 
			// butColor
			// 
			this.butColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butColor.Location = new System.Drawing.Point(407, 100);
			this.butColor.Name = "butColor";
			this.butColor.Size = new System.Drawing.Size(24, 21);
			this.butColor.TabIndex = 225;
			this.butColor.Click += new System.EventHandler(this.butColor_Click);
			// 
			// butApptLineColor
			// 
			this.butApptLineColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.butApptLineColor.Location = new System.Drawing.Point(407, 127);
			this.butApptLineColor.Name = "butApptLineColor";
			this.butApptLineColor.Size = new System.Drawing.Size(24, 21);
			this.butApptLineColor.TabIndex = 226;
			this.butApptLineColor.Click += new System.EventHandler(this.butApptLineColor_Click);
			// 
			// checkSolidBlockouts
			// 
			this.checkSolidBlockouts.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSolidBlockouts.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSolidBlockouts.Location = new System.Drawing.Point(23, 54);
			this.checkSolidBlockouts.Name = "checkSolidBlockouts";
			this.checkSolidBlockouts.Size = new System.Drawing.Size(408, 17);
			this.checkSolidBlockouts.TabIndex = 220;
			this.checkSolidBlockouts.Text = "Use solid blockouts instead of outlines on the appointment book";
			this.checkSolidBlockouts.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSolidBlockouts.UseVisualStyleBackColor = true;
			// 
			// checkApptExclamation
			// 
			this.checkApptExclamation.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptExclamation.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkApptExclamation.Location = new System.Drawing.Point(6, 77);
			this.checkApptExclamation.Name = "checkApptExclamation";
			this.checkApptExclamation.Size = new System.Drawing.Size(425, 17);
			this.checkApptExclamation.TabIndex = 222;
			this.checkApptExclamation.Text = "Show ! on appts for ins not sent, if added to Appt View (might cause slowdown)";
			this.checkApptExclamation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptExclamation.UseVisualStyleBackColor = true;
			// 
			// checkApptBubbleDelay
			// 
			this.checkApptBubbleDelay.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptBubbleDelay.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkApptBubbleDelay.Location = new System.Drawing.Point(6, 31);
			this.checkApptBubbleDelay.Name = "checkApptBubbleDelay";
			this.checkApptBubbleDelay.Size = new System.Drawing.Size(425, 17);
			this.checkApptBubbleDelay.TabIndex = 221;
			this.checkApptBubbleDelay.Text = "Appointment bubble popup delay";
			this.checkApptBubbleDelay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptBubbleDelay.UseVisualStyleBackColor = true;
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(155, 104);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(246, 16);
			this.label23.TabIndex = 223;
			this.label23.Text = "Waiting room alert color";
			this.label23.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label25
			// 
			this.label25.Location = new System.Drawing.Point(155, 131);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(246, 16);
			this.label25.TabIndex = 224;
			this.label25.Text = "Appointment time line color";
			this.label25.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkApptModuleDefaultToWeek
			// 
			this.checkApptModuleDefaultToWeek.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkApptModuleDefaultToWeek.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkApptModuleDefaultToWeek.Location = new System.Drawing.Point(27, 154);
			this.checkApptModuleDefaultToWeek.Name = "checkApptModuleDefaultToWeek";
			this.checkApptModuleDefaultToWeek.Size = new System.Drawing.Size(406, 17);
			this.checkApptModuleDefaultToWeek.TabIndex = 220;
			this.checkApptModuleDefaultToWeek.Text = "Appointment Module Defaults to Week View";
			this.checkApptModuleDefaultToWeek.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// apptClickDelay
			// 
			this.apptClickDelay.Location = new System.Drawing.Point(25, 175);
			this.apptClickDelay.Name = "apptClickDelay";
			this.apptClickDelay.Size = new System.Drawing.Size(296, 18);
			this.apptClickDelay.TabIndex = 233;
			this.apptClickDelay.Text = "Appointment click delay";
			this.apptClickDelay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabFamily
			// 
			this.tabFamily.BackColor = System.Drawing.SystemColors.Window;
			this.tabFamily.Controls.Add(this.tabControlFamily);
			this.tabFamily.Location = new System.Drawing.Point(4, 22);
			this.tabFamily.Name = "tabFamily";
			this.tabFamily.Padding = new System.Windows.Forms.Padding(3);
			this.tabFamily.Size = new System.Drawing.Size(471, 579);
			this.tabFamily.TabIndex = 1;
			this.tabFamily.Text = "Family";
			// 
			// tabControlFamily
			// 
			this.tabControlFamily.Controls.Add(this.tabPageFamGen);
			this.tabControlFamily.Controls.Add(this.tabPageSuperFam);
			this.tabControlFamily.Controls.Add(this.tabPageClaimSnapShot);
			this.tabControlFamily.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlFamily.Location = new System.Drawing.Point(3, 3);
			this.tabControlFamily.Name = "tabControlFamily";
			this.tabControlFamily.SelectedIndex = 0;
			this.tabControlFamily.Size = new System.Drawing.Size(465, 573);
			this.tabControlFamily.TabIndex = 227;
			// 
			// tabPageFamGen
			// 
			this.tabPageFamGen.Controls.Add(this.checkAllowPatsAtHQ);
			this.tabPageFamGen.Controls.Add(this.checkAutoFillPatEmail);
			this.tabPageFamGen.Controls.Add(this.checkPreferredReferrals);
			this.tabPageFamGen.Controls.Add(this.checkTextMsgOkStatusTreatAsNo);
			this.tabPageFamGen.Controls.Add(this.checkPatInitBillingTypeFromPriInsPlan);
			this.tabPageFamGen.Controls.Add(this.checkFamPhiAccess);
			this.tabPageFamGen.Controls.Add(this.checkClaimTrackingRequireError);
			this.tabPageFamGen.Controls.Add(this.checkPPOpercentage);
			this.tabPageFamGen.Controls.Add(this.checkClaimUseOverrideProcDescript);
			this.tabPageFamGen.Controls.Add(this.checkSelectProv);
			this.tabPageFamGen.Controls.Add(this.checkAllowedFeeSchedsAutomate);
			this.tabPageFamGen.Controls.Add(this.checkInsPPOsecWriteoffs);
			this.tabPageFamGen.Controls.Add(this.checkInsurancePlansShared);
			this.tabPageFamGen.Controls.Add(this.checkCoPayFeeScheduleBlankLikeZero);
			this.tabPageFamGen.Controls.Add(this.checkInsDefaultShowUCRonClaims);
			this.tabPageFamGen.Controls.Add(this.checkGoogleAddress);
			this.tabPageFamGen.Controls.Add(this.comboCobRule);
			this.tabPageFamGen.Controls.Add(this.checkInsDefaultAssignmentOfBenefits);
			this.tabPageFamGen.Controls.Add(this.label15);
			this.tabPageFamGen.Location = new System.Drawing.Point(4, 22);
			this.tabPageFamGen.Name = "tabPageFamGen";
			this.tabPageFamGen.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageFamGen.Size = new System.Drawing.Size(457, 547);
			this.tabPageFamGen.TabIndex = 0;
			this.tabPageFamGen.Text = "General";
			this.tabPageFamGen.UseVisualStyleBackColor = true;
			// 
			// checkAllowPatsAtHQ
			// 
			this.checkAllowPatsAtHQ.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowPatsAtHQ.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllowPatsAtHQ.Location = new System.Drawing.Point(9, 431);
			this.checkAllowPatsAtHQ.Name = "checkAllowPatsAtHQ";
			this.checkAllowPatsAtHQ.Size = new System.Drawing.Size(422, 17);
			this.checkAllowPatsAtHQ.TabIndex = 235;
			this.checkAllowPatsAtHQ.Text = "Allow new patients to be added with an unassigned clinic";
			this.checkAllowPatsAtHQ.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkAutoFillPatEmail
			// 
			this.checkAutoFillPatEmail.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAutoFillPatEmail.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAutoFillPatEmail.Location = new System.Drawing.Point(6, 408);
			this.checkAutoFillPatEmail.Name = "checkAutoFillPatEmail";
			this.checkAutoFillPatEmail.Size = new System.Drawing.Size(425, 17);
			this.checkAutoFillPatEmail.TabIndex = 230;
			this.checkAutoFillPatEmail.Text = "Autofill patient\'s email with the guarantor\'s when adding many new patients.";
			this.checkAutoFillPatEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkPreferredReferrals
			// 
			this.checkPreferredReferrals.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPreferredReferrals.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPreferredReferrals.Location = new System.Drawing.Point(6, 385);
			this.checkPreferredReferrals.Name = "checkPreferredReferrals";
			this.checkPreferredReferrals.Size = new System.Drawing.Size(425, 17);
			this.checkPreferredReferrals.TabIndex = 229;
			this.checkPreferredReferrals.Text = "Show preferred referrals only in the \'Select Referral\' window by default";
			this.checkPreferredReferrals.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkTextMsgOkStatusTreatAsNo
			// 
			this.checkTextMsgOkStatusTreatAsNo.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTextMsgOkStatusTreatAsNo.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTextMsgOkStatusTreatAsNo.Location = new System.Drawing.Point(6, 187);
			this.checkTextMsgOkStatusTreatAsNo.Name = "checkTextMsgOkStatusTreatAsNo";
			this.checkTextMsgOkStatusTreatAsNo.Size = new System.Drawing.Size(425, 17);
			this.checkTextMsgOkStatusTreatAsNo.TabIndex = 224;
			this.checkTextMsgOkStatusTreatAsNo.Text = "Text Msg OK status, treat ?? as No instead of Yes";
			this.checkTextMsgOkStatusTreatAsNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkPatInitBillingTypeFromPriInsPlan
			// 
			this.checkPatInitBillingTypeFromPriInsPlan.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPatInitBillingTypeFromPriInsPlan.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPatInitBillingTypeFromPriInsPlan.Location = new System.Drawing.Point(6, 362);
			this.checkPatInitBillingTypeFromPriInsPlan.Name = "checkPatInitBillingTypeFromPriInsPlan";
			this.checkPatInitBillingTypeFromPriInsPlan.Size = new System.Drawing.Size(425, 17);
			this.checkPatInitBillingTypeFromPriInsPlan.TabIndex = 225;
			this.checkPatInitBillingTypeFromPriInsPlan.Text = "New patient primary insurance plan sets patient billing type";
			this.checkPatInitBillingTypeFromPriInsPlan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkFamPhiAccess
			// 
			this.checkFamPhiAccess.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkFamPhiAccess.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkFamPhiAccess.Location = new System.Drawing.Point(6, 212);
			this.checkFamPhiAccess.Name = "checkFamPhiAccess";
			this.checkFamPhiAccess.Size = new System.Drawing.Size(425, 17);
			this.checkFamPhiAccess.TabIndex = 226;
			this.checkFamPhiAccess.Text = "Allow Guarantor access to family health information in patient portal";
			this.checkFamPhiAccess.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkClaimTrackingRequireError
			// 
			this.checkClaimTrackingRequireError.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClaimTrackingRequireError.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClaimTrackingRequireError.Location = new System.Drawing.Point(6, 337);
			this.checkClaimTrackingRequireError.Name = "checkClaimTrackingRequireError";
			this.checkClaimTrackingRequireError.Size = new System.Drawing.Size(425, 17);
			this.checkClaimTrackingRequireError.TabIndex = 224;
			this.checkClaimTrackingRequireError.Text = "Require error code when adding claim custom tracking status.";
			this.checkClaimTrackingRequireError.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkPPOpercentage
			// 
			this.checkPPOpercentage.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPPOpercentage.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPPOpercentage.Location = new System.Drawing.Point(6, 33);
			this.checkPPOpercentage.Name = "checkPPOpercentage";
			this.checkPPOpercentage.Size = new System.Drawing.Size(425, 17);
			this.checkPPOpercentage.TabIndex = 218;
			this.checkPPOpercentage.Text = "Insurance defaults to PPO percentage instead of category percentage plan type";
			this.checkPPOpercentage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkClaimUseOverrideProcDescript
			// 
			this.checkClaimUseOverrideProcDescript.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClaimUseOverrideProcDescript.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClaimUseOverrideProcDescript.Location = new System.Drawing.Point(6, 312);
			this.checkClaimUseOverrideProcDescript.Name = "checkClaimUseOverrideProcDescript";
			this.checkClaimUseOverrideProcDescript.Size = new System.Drawing.Size(425, 17);
			this.checkClaimUseOverrideProcDescript.TabIndex = 223;
			this.checkClaimUseOverrideProcDescript.Text = "Use the description for the charted procedure code on printed claims";
			this.checkClaimUseOverrideProcDescript.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkSelectProv
			// 
			this.checkSelectProv.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSelectProv.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSelectProv.Location = new System.Drawing.Point(6, 287);
			this.checkSelectProv.Name = "checkSelectProv";
			this.checkSelectProv.Size = new System.Drawing.Size(425, 17);
			this.checkSelectProv.TabIndex = 216;
			this.checkSelectProv.Text = "Primary Provider defaults to \'Select Provider\' in patient edit and add family";
			this.checkSelectProv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkAllowedFeeSchedsAutomate
			// 
			this.checkAllowedFeeSchedsAutomate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowedFeeSchedsAutomate.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllowedFeeSchedsAutomate.Location = new System.Drawing.Point(6, 58);
			this.checkAllowedFeeSchedsAutomate.Name = "checkAllowedFeeSchedsAutomate";
			this.checkAllowedFeeSchedsAutomate.Size = new System.Drawing.Size(425, 17);
			this.checkAllowedFeeSchedsAutomate.TabIndex = 219;
			this.checkAllowedFeeSchedsAutomate.Text = "Use Blue Book";
			this.checkAllowedFeeSchedsAutomate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowedFeeSchedsAutomate.Click += new System.EventHandler(this.checkAllowedFeeSchedsAutomate_Click);
			// 
			// checkInsPPOsecWriteoffs
			// 
			this.checkInsPPOsecWriteoffs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkInsPPOsecWriteoffs.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkInsPPOsecWriteoffs.Location = new System.Drawing.Point(6, 237);
			this.checkInsPPOsecWriteoffs.Name = "checkInsPPOsecWriteoffs";
			this.checkInsPPOsecWriteoffs.Size = new System.Drawing.Size(425, 17);
			this.checkInsPPOsecWriteoffs.TabIndex = 227;
			this.checkInsPPOsecWriteoffs.Text = "Calculate secondary insurance PPO writeoffs (not recommended, see manual)";
			this.checkInsPPOsecWriteoffs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkInsPPOsecWriteoffs.UseVisualStyleBackColor = true;
			// 
			// checkInsurancePlansShared
			// 
			this.checkInsurancePlansShared.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkInsurancePlansShared.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkInsurancePlansShared.Location = new System.Drawing.Point(6, 8);
			this.checkInsurancePlansShared.Name = "checkInsurancePlansShared";
			this.checkInsurancePlansShared.Size = new System.Drawing.Size(425, 17);
			this.checkInsurancePlansShared.TabIndex = 216;
			this.checkInsurancePlansShared.Text = "InsPlan option at bottom, \'Change Plan for all subscribers\', is default.";
			this.checkInsurancePlansShared.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkCoPayFeeScheduleBlankLikeZero
			// 
			this.checkCoPayFeeScheduleBlankLikeZero.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkCoPayFeeScheduleBlankLikeZero.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkCoPayFeeScheduleBlankLikeZero.Location = new System.Drawing.Point(6, 83);
			this.checkCoPayFeeScheduleBlankLikeZero.Name = "checkCoPayFeeScheduleBlankLikeZero";
			this.checkCoPayFeeScheduleBlankLikeZero.Size = new System.Drawing.Size(425, 17);
			this.checkCoPayFeeScheduleBlankLikeZero.TabIndex = 220;
			this.checkCoPayFeeScheduleBlankLikeZero.Text = "Co-pay fee schedules treat blank entries as zero.";
			this.checkCoPayFeeScheduleBlankLikeZero.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkInsDefaultShowUCRonClaims
			// 
			this.checkInsDefaultShowUCRonClaims.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkInsDefaultShowUCRonClaims.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkInsDefaultShowUCRonClaims.Location = new System.Drawing.Point(6, 108);
			this.checkInsDefaultShowUCRonClaims.Name = "checkInsDefaultShowUCRonClaims";
			this.checkInsDefaultShowUCRonClaims.Size = new System.Drawing.Size(425, 17);
			this.checkInsDefaultShowUCRonClaims.TabIndex = 221;
			this.checkInsDefaultShowUCRonClaims.Text = "Insurance plans default to show UCR fee on claims.";
			this.checkInsDefaultShowUCRonClaims.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkInsDefaultShowUCRonClaims.Click += new System.EventHandler(this.checkInsDefaultShowUCRonClaims_Click);
			// 
			// checkGoogleAddress
			// 
			this.checkGoogleAddress.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkGoogleAddress.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkGoogleAddress.Location = new System.Drawing.Point(6, 262);
			this.checkGoogleAddress.Name = "checkGoogleAddress";
			this.checkGoogleAddress.Size = new System.Drawing.Size(425, 17);
			this.checkGoogleAddress.TabIndex = 228;
			this.checkGoogleAddress.Text = "Show Google Maps in patient edit";
			this.checkGoogleAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboCobRule
			// 
			this.comboCobRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCobRule.FormattingEnabled = true;
			this.comboCobRule.Location = new System.Drawing.Point(303, 158);
			this.comboCobRule.MaxDropDownItems = 30;
			this.comboCobRule.Name = "comboCobRule";
			this.comboCobRule.Size = new System.Drawing.Size(128, 21);
			this.comboCobRule.TabIndex = 222;
			this.comboCobRule.SelectionChangeCommitted += new System.EventHandler(this.comboCobRule_SelectionChangeCommitted);
			// 
			// checkInsDefaultAssignmentOfBenefits
			// 
			this.checkInsDefaultAssignmentOfBenefits.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkInsDefaultAssignmentOfBenefits.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkInsDefaultAssignmentOfBenefits.Location = new System.Drawing.Point(6, 133);
			this.checkInsDefaultAssignmentOfBenefits.Name = "checkInsDefaultAssignmentOfBenefits";
			this.checkInsDefaultAssignmentOfBenefits.Size = new System.Drawing.Size(425, 17);
			this.checkInsDefaultAssignmentOfBenefits.TabIndex = 225;
			this.checkInsDefaultAssignmentOfBenefits.Text = "Insurance plans default to assignment of benefits.";
			this.checkInsDefaultAssignmentOfBenefits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkInsDefaultAssignmentOfBenefits.Click += new System.EventHandler(this.checkInsDefaultAssignmentOfBenefits_Click);
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(6, 161);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(296, 17);
			this.label15.TabIndex = 223;
			this.label15.Text = "Coordination of Benefits (COB) Rule";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabPageSuperFam
			// 
			this.tabPageSuperFam.Controls.Add(this.checkSuperFamCloneCreate);
			this.tabPageSuperFam.Controls.Add(this.comboSuperFamSort);
			this.tabPageSuperFam.Controls.Add(this.checkSuperFamAddIns);
			this.tabPageSuperFam.Controls.Add(this.checkSuperFamSync);
			this.tabPageSuperFam.Controls.Add(this.labelSuperFamSort);
			this.tabPageSuperFam.Location = new System.Drawing.Point(4, 22);
			this.tabPageSuperFam.Name = "tabPageSuperFam";
			this.tabPageSuperFam.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageSuperFam.Size = new System.Drawing.Size(457, 547);
			this.tabPageSuperFam.TabIndex = 1;
			this.tabPageSuperFam.Text = "Super Family";
			this.tabPageSuperFam.UseVisualStyleBackColor = true;
			// 
			// checkSuperFamCloneCreate
			// 
			this.checkSuperFamCloneCreate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSuperFamCloneCreate.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSuperFamCloneCreate.Location = new System.Drawing.Point(6, 87);
			this.checkSuperFamCloneCreate.Name = "checkSuperFamCloneCreate";
			this.checkSuperFamCloneCreate.Size = new System.Drawing.Size(429, 17);
			this.checkSuperFamCloneCreate.TabIndex = 227;
			this.checkSuperFamCloneCreate.Text = "New patient clones use super family instead of regular family";
			this.checkSuperFamCloneCreate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboSuperFamSort
			// 
			this.comboSuperFamSort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboSuperFamSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSuperFamSort.FormattingEnabled = true;
			this.comboSuperFamSort.Location = new System.Drawing.Point(307, 16);
			this.comboSuperFamSort.MaxDropDownItems = 30;
			this.comboSuperFamSort.Name = "comboSuperFamSort";
			this.comboSuperFamSort.Size = new System.Drawing.Size(128, 21);
			this.comboSuperFamSort.TabIndex = 217;
			// 
			// checkSuperFamAddIns
			// 
			this.checkSuperFamAddIns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkSuperFamAddIns.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSuperFamAddIns.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSuperFamAddIns.Location = new System.Drawing.Point(6, 65);
			this.checkSuperFamAddIns.Name = "checkSuperFamAddIns";
			this.checkSuperFamAddIns.Size = new System.Drawing.Size(429, 17);
			this.checkSuperFamAddIns.TabIndex = 221;
			this.checkSuperFamAddIns.Text = "Copy super guarantor\'s primary insurance to all new super family members";
			this.checkSuperFamAddIns.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkSuperFamSync
			// 
			this.checkSuperFamSync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkSuperFamSync.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSuperFamSync.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSuperFamSync.Location = new System.Drawing.Point(6, 43);
			this.checkSuperFamSync.Name = "checkSuperFamSync";
			this.checkSuperFamSync.Size = new System.Drawing.Size(429, 17);
			this.checkSuperFamSync.TabIndex = 219;
			this.checkSuperFamSync.Text = "Allow syncing patient information to all super family members";
			this.checkSuperFamSync.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelSuperFamSort
			// 
			this.labelSuperFamSort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelSuperFamSort.Location = new System.Drawing.Point(6, 18);
			this.labelSuperFamSort.Name = "labelSuperFamSort";
			this.labelSuperFamSort.Size = new System.Drawing.Size(300, 17);
			this.labelSuperFamSort.TabIndex = 218;
			this.labelSuperFamSort.Text = "Super family sorting strategy";
			this.labelSuperFamSort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabPageClaimSnapShot
			// 
			this.tabPageClaimSnapShot.Controls.Add(this.label31);
			this.tabPageClaimSnapShot.Controls.Add(this.label30);
			this.tabPageClaimSnapShot.Controls.Add(this.comboClaimSnapshotTrigger);
			this.tabPageClaimSnapShot.Controls.Add(this.textClaimSnapshotRunTime);
			this.tabPageClaimSnapShot.Location = new System.Drawing.Point(4, 22);
			this.tabPageClaimSnapShot.Name = "tabPageClaimSnapShot";
			this.tabPageClaimSnapShot.Size = new System.Drawing.Size(457, 547);
			this.tabPageClaimSnapShot.TabIndex = 2;
			this.tabPageClaimSnapShot.Text = "Claim Snapshot";
			this.tabPageClaimSnapShot.UseVisualStyleBackColor = true;
			// 
			// label31
			// 
			this.label31.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label31.Location = new System.Drawing.Point(6, 13);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(286, 17);
			this.label31.TabIndex = 224;
			this.label31.Text = "Snapshot Trigger";
			this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label30
			// 
			this.label30.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label30.Location = new System.Drawing.Point(6, 38);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(324, 17);
			this.label30.TabIndex = 223;
			this.label30.Text = "Service Run Time";
			this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboClaimSnapshotTrigger
			// 
			this.comboClaimSnapshotTrigger.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboClaimSnapshotTrigger.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClaimSnapshotTrigger.FormattingEnabled = true;
			this.comboClaimSnapshotTrigger.Location = new System.Drawing.Point(293, 11);
			this.comboClaimSnapshotTrigger.MaxDropDownItems = 30;
			this.comboClaimSnapshotTrigger.Name = "comboClaimSnapshotTrigger";
			this.comboClaimSnapshotTrigger.Size = new System.Drawing.Size(148, 21);
			this.comboClaimSnapshotTrigger.TabIndex = 221;
			// 
			// textClaimSnapshotRunTime
			// 
			this.textClaimSnapshotRunTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textClaimSnapshotRunTime.Location = new System.Drawing.Point(331, 37);
			this.textClaimSnapshotRunTime.Name = "textClaimSnapshotRunTime";
			this.textClaimSnapshotRunTime.Size = new System.Drawing.Size(110, 20);
			this.textClaimSnapshotRunTime.TabIndex = 222;
			// 
			// tabAccount
			// 
			this.tabAccount.BackColor = System.Drawing.SystemColors.Window;
			this.tabAccount.Controls.Add(this.tabControlAccount);
			this.tabAccount.Location = new System.Drawing.Point(4, 22);
			this.tabAccount.Name = "tabAccount";
			this.tabAccount.Padding = new System.Windows.Forms.Padding(3);
			this.tabAccount.Size = new System.Drawing.Size(471, 579);
			this.tabAccount.TabIndex = 2;
			this.tabAccount.Text = "Account";
			// 
			// tabControlAccount
			// 
			this.tabControlAccount.Controls.Add(this.tabPagePayAdj);
			this.tabControlAccount.Controls.Add(this.tabPageIns);
			this.tabControlAccount.Controls.Add(this.tabPageMisc);
			this.tabControlAccount.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlAccount.Location = new System.Drawing.Point(3, 3);
			this.tabControlAccount.Name = "tabControlAccount";
			this.tabControlAccount.SelectedIndex = 0;
			this.tabControlAccount.Size = new System.Drawing.Size(465, 573);
			this.tabControlAccount.TabIndex = 238;
			// 
			// tabPagePayAdj
			// 
			this.tabPagePayAdj.Controls.Add(this.checkAllowPrepayProvider);
			this.tabPagePayAdj.Controls.Add(this.comboPayPlanAdj);
			this.tabPagePayAdj.Controls.Add(this.label42);
			this.tabPagePayAdj.Controls.Add(this.comboRigorousAdjustments);
			this.tabPagePayAdj.Controls.Add(this.label41);
			this.tabPagePayAdj.Controls.Add(this.checkHidePaysplits);
			this.tabPagePayAdj.Controls.Add(this.label40);
			this.tabPagePayAdj.Controls.Add(this.comboAutoSplitPref);
			this.tabPagePayAdj.Controls.Add(this.comboRigorousAccounting);
			this.tabPagePayAdj.Controls.Add(this.label39);
			this.tabPagePayAdj.Controls.Add(this.checkAllowEmailCCReceipt);
			this.tabPagePayAdj.Controls.Add(this.label38);
			this.tabPagePayAdj.Controls.Add(this.comboPaymentClinicSetting);
			this.tabPagePayAdj.Controls.Add(this.checkAllowFutureDebits);
			this.tabPagePayAdj.Controls.Add(this.checkStoreCCTokens);
			this.tabPagePayAdj.Controls.Add(this.textTaxPercent);
			this.tabPagePayAdj.Controls.Add(this.label26);
			this.tabPagePayAdj.Controls.Add(this.comboSalesTaxAdjType);
			this.tabPagePayAdj.Controls.Add(this.label33);
			this.tabPagePayAdj.Controls.Add(this.comboBillingChargeAdjType);
			this.tabPagePayAdj.Controls.Add(this.label12);
			this.tabPagePayAdj.Controls.Add(this.comboFinanceChargeAdjType);
			this.tabPagePayAdj.Controls.Add(this.label4);
			this.tabPagePayAdj.Controls.Add(this.groupBox4);
			this.tabPagePayAdj.Controls.Add(this.checkPaymentsPromptForPayType);
			this.tabPagePayAdj.Controls.Add(this.comboUnallocatedSplits);
			this.tabPagePayAdj.Controls.Add(this.label28);
			this.tabPagePayAdj.Location = new System.Drawing.Point(4, 22);
			this.tabPagePayAdj.Name = "tabPagePayAdj";
			this.tabPagePayAdj.Padding = new System.Windows.Forms.Padding(3);
			this.tabPagePayAdj.Size = new System.Drawing.Size(457, 547);
			this.tabPagePayAdj.TabIndex = 0;
			this.tabPagePayAdj.Text = "Pay/Adj";
			this.tabPagePayAdj.UseVisualStyleBackColor = true;
			// 
			// checkAllowPrepayProvider
			// 
			this.checkAllowPrepayProvider.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowPrepayProvider.Location = new System.Drawing.Point(12, 354);
			this.checkAllowPrepayProvider.Name = "checkAllowPrepayProvider";
			this.checkAllowPrepayProvider.Size = new System.Drawing.Size(433, 17);
			this.checkAllowPrepayProvider.TabIndex = 249;
			this.checkAllowPrepayProvider.Text = "Allow prepayments to providers";
			this.checkAllowPrepayProvider.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowPrepayProvider.UseVisualStyleBackColor = true;
			this.checkAllowPrepayProvider.Visible = false;
			// 
			// comboPayPlanAdj
			// 
			this.comboPayPlanAdj.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPayPlanAdj.FormattingEnabled = true;
			this.comboPayPlanAdj.Location = new System.Drawing.Point(282, 179);
			this.comboPayPlanAdj.MaxDropDownItems = 30;
			this.comboPayPlanAdj.Name = "comboPayPlanAdj";
			this.comboPayPlanAdj.Size = new System.Drawing.Size(163, 21);
			this.comboPayPlanAdj.TabIndex = 248;
			// 
			// label42
			// 
			this.label42.BackColor = System.Drawing.SystemColors.Window;
			this.label42.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label42.Location = new System.Drawing.Point(45, 182);
			this.label42.Name = "label42";
			this.label42.Size = new System.Drawing.Size(234, 15);
			this.label42.TabIndex = 247;
			this.label42.Text = "Payment Plan adj type";
			this.label42.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboRigorousAdjustments
			// 
			this.comboRigorousAdjustments.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboRigorousAdjustments.Location = new System.Drawing.Point(282, 373);
			this.comboRigorousAdjustments.MaxDropDownItems = 30;
			this.comboRigorousAdjustments.Name = "comboRigorousAdjustments";
			this.comboRigorousAdjustments.Size = new System.Drawing.Size(163, 21);
			this.comboRigorousAdjustments.TabIndex = 246;
			// 
			// label41
			// 
			this.label41.BackColor = System.Drawing.SystemColors.Window;
			this.label41.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label41.Location = new System.Drawing.Point(43, 377);
			this.label41.Name = "label41";
			this.label41.Size = new System.Drawing.Size(234, 15);
			this.label41.TabIndex = 245;
			this.label41.Text = "Enforce Valid Adjustments";
			this.label41.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkHidePaysplits
			// 
			this.checkHidePaysplits.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHidePaysplits.Location = new System.Drawing.Point(12, 426);
			this.checkHidePaysplits.Name = "checkHidePaysplits";
			this.checkHidePaysplits.Size = new System.Drawing.Size(433, 17);
			this.checkHidePaysplits.TabIndex = 244;
			this.checkHidePaysplits.Text = "Hide paysplits from payment window by default";
			this.checkHidePaysplits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHidePaysplits.UseVisualStyleBackColor = true;
			// 
			// label40
			// 
			this.label40.BackColor = System.Drawing.SystemColors.Window;
			this.label40.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label40.Location = new System.Drawing.Point(43, 403);
			this.label40.Name = "label40";
			this.label40.Size = new System.Drawing.Size(234, 15);
			this.label40.TabIndex = 243;
			this.label40.Text = "Auto-split payments preferring:";
			this.label40.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboAutoSplitPref
			// 
			this.comboAutoSplitPref.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAutoSplitPref.FormattingEnabled = true;
			this.comboAutoSplitPref.Location = new System.Drawing.Point(282, 400);
			this.comboAutoSplitPref.MaxDropDownItems = 30;
			this.comboAutoSplitPref.Name = "comboAutoSplitPref";
			this.comboAutoSplitPref.Size = new System.Drawing.Size(163, 21);
			this.comboAutoSplitPref.TabIndex = 242;
			// 
			// comboRigorousAccounting
			// 
			this.comboRigorousAccounting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboRigorousAccounting.FormattingEnabled = true;
			this.comboRigorousAccounting.Location = new System.Drawing.Point(282, 330);
			this.comboRigorousAccounting.MaxDropDownItems = 30;
			this.comboRigorousAccounting.Name = "comboRigorousAccounting";
			this.comboRigorousAccounting.Size = new System.Drawing.Size(163, 21);
			this.comboRigorousAccounting.TabIndex = 241;
			this.comboRigorousAccounting.SelectedIndexChanged += new System.EventHandler(this.comboRigorousAccounting_SelectedIndexChanged);
			// 
			// label39
			// 
			this.label39.BackColor = System.Drawing.SystemColors.Window;
			this.label39.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label39.Location = new System.Drawing.Point(43, 334);
			this.label39.Name = "label39";
			this.label39.Size = new System.Drawing.Size(234, 15);
			this.label39.TabIndex = 240;
			this.label39.Text = "Enforce Valid Paysplits";
			this.label39.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkAllowEmailCCReceipt
			// 
			this.checkAllowEmailCCReceipt.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowEmailCCReceipt.Location = new System.Drawing.Point(12, 309);
			this.checkAllowEmailCCReceipt.Name = "checkAllowEmailCCReceipt";
			this.checkAllowEmailCCReceipt.Size = new System.Drawing.Size(433, 17);
			this.checkAllowEmailCCReceipt.TabIndex = 239;
			this.checkAllowEmailCCReceipt.Text = "Allow emailing credit card receipts";
			this.checkAllowEmailCCReceipt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowEmailCCReceipt.UseVisualStyleBackColor = true;
			// 
			// label38
			// 
			this.label38.Location = new System.Drawing.Point(46, 28);
			this.label38.Margin = new System.Windows.Forms.Padding(0);
			this.label38.Name = "label38";
			this.label38.Size = new System.Drawing.Size(234, 21);
			this.label38.TabIndex = 238;
			this.label38.Text = "Patient Payments Use";
			this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboPaymentClinicSetting
			// 
			this.comboPaymentClinicSetting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPaymentClinicSetting.FormattingEnabled = true;
			this.comboPaymentClinicSetting.Location = new System.Drawing.Point(282, 28);
			this.comboPaymentClinicSetting.MaxDropDownItems = 30;
			this.comboPaymentClinicSetting.Name = "comboPaymentClinicSetting";
			this.comboPaymentClinicSetting.Size = new System.Drawing.Size(163, 21);
			this.comboPaymentClinicSetting.TabIndex = 236;
			// 
			// checkAllowFutureDebits
			// 
			this.checkAllowFutureDebits.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowFutureDebits.Location = new System.Drawing.Point(11, 291);
			this.checkAllowFutureDebits.Name = "checkAllowFutureDebits";
			this.checkAllowFutureDebits.Size = new System.Drawing.Size(434, 17);
			this.checkAllowFutureDebits.TabIndex = 235;
			this.checkAllowFutureDebits.Text = "Allow future dated payments (not recommended, see manual)";
			this.checkAllowFutureDebits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowFutureDebits.UseVisualStyleBackColor = true;
			// 
			// checkStoreCCTokens
			// 
			this.checkStoreCCTokens.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkStoreCCTokens.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkStoreCCTokens.Location = new System.Drawing.Point(6, 6);
			this.checkStoreCCTokens.Name = "checkStoreCCTokens";
			this.checkStoreCCTokens.Size = new System.Drawing.Size(439, 17);
			this.checkStoreCCTokens.TabIndex = 203;
			this.checkStoreCCTokens.Text = "Automatically store credit card tokens";
			this.checkStoreCCTokens.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkStoreCCTokens.UseVisualStyleBackColor = true;
			// 
			// textTaxPercent
			// 
			this.textTaxPercent.Location = new System.Drawing.Point(392, 206);
			this.textTaxPercent.Name = "textTaxPercent";
			this.textTaxPercent.Size = new System.Drawing.Size(53, 20);
			this.textTaxPercent.TabIndex = 231;
			// 
			// label26
			// 
			this.label26.BackColor = System.Drawing.SystemColors.Window;
			this.label26.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label26.Location = new System.Drawing.Point(43, 208);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(344, 16);
			this.label26.TabIndex = 232;
			this.label26.Text = "Sales Tax Percentage";
			this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboSalesTaxAdjType
			// 
			this.comboSalesTaxAdjType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSalesTaxAdjType.FormattingEnabled = true;
			this.comboSalesTaxAdjType.Location = new System.Drawing.Point(282, 153);
			this.comboSalesTaxAdjType.MaxDropDownItems = 30;
			this.comboSalesTaxAdjType.Name = "comboSalesTaxAdjType";
			this.comboSalesTaxAdjType.Size = new System.Drawing.Size(163, 21);
			this.comboSalesTaxAdjType.TabIndex = 234;
			// 
			// label33
			// 
			this.label33.BackColor = System.Drawing.SystemColors.Window;
			this.label33.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label33.Location = new System.Drawing.Point(43, 156);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(234, 15);
			this.label33.TabIndex = 233;
			this.label33.Text = "Sales Tax adj type";
			this.label33.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboBillingChargeAdjType
			// 
			this.comboBillingChargeAdjType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBillingChargeAdjType.FormattingEnabled = true;
			this.comboBillingChargeAdjType.Location = new System.Drawing.Point(282, 127);
			this.comboBillingChargeAdjType.MaxDropDownItems = 30;
			this.comboBillingChargeAdjType.Name = "comboBillingChargeAdjType";
			this.comboBillingChargeAdjType.Size = new System.Drawing.Size(163, 21);
			this.comboBillingChargeAdjType.TabIndex = 199;
			// 
			// label12
			// 
			this.label12.BackColor = System.Drawing.SystemColors.Window;
			this.label12.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label12.Location = new System.Drawing.Point(43, 131);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(234, 15);
			this.label12.TabIndex = 73;
			this.label12.Text = "Billing charge adj type";
			this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboFinanceChargeAdjType
			// 
			this.comboFinanceChargeAdjType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFinanceChargeAdjType.FormattingEnabled = true;
			this.comboFinanceChargeAdjType.Location = new System.Drawing.Point(282, 101);
			this.comboFinanceChargeAdjType.MaxDropDownItems = 30;
			this.comboFinanceChargeAdjType.Name = "comboFinanceChargeAdjType";
			this.comboFinanceChargeAdjType.Size = new System.Drawing.Size(163, 21);
			this.comboFinanceChargeAdjType.TabIndex = 72;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.SystemColors.Window;
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label4.Location = new System.Drawing.Point(43, 104);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(234, 15);
			this.label4.TabIndex = 198;
			this.label4.Text = "Finance charge adj type";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.listboxBadDebtAdjs);
			this.groupBox4.Controls.Add(this.label29);
			this.groupBox4.Controls.Add(this.butBadDebt);
			this.groupBox4.Location = new System.Drawing.Point(131, 226);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(314, 59);
			this.groupBox4.TabIndex = 224;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Bad Debt Adjustments";
			// 
			// listboxBadDebtAdjs
			// 
			this.listboxBadDebtAdjs.FormattingEnabled = true;
			this.listboxBadDebtAdjs.Location = new System.Drawing.Point(189, 11);
			this.listboxBadDebtAdjs.Name = "listboxBadDebtAdjs";
			this.listboxBadDebtAdjs.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listboxBadDebtAdjs.Size = new System.Drawing.Size(120, 43);
			this.listboxBadDebtAdjs.TabIndex = 197;
			// 
			// label29
			// 
			this.label29.Location = new System.Drawing.Point(4, 12);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(184, 20);
			this.label29.TabIndex = 223;
			this.label29.Text = "Current Bad Debt Adj Types:";
			this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butBadDebt
			// 
			this.butBadDebt.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBadDebt.Autosize = true;
			this.butBadDebt.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBadDebt.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBadDebt.CornerRadius = 4F;
			this.butBadDebt.Location = new System.Drawing.Point(117, 33);
			this.butBadDebt.Name = "butBadDebt";
			this.butBadDebt.Size = new System.Drawing.Size(68, 21);
			this.butBadDebt.TabIndex = 197;
			this.butBadDebt.Text = "Edit";
			this.butBadDebt.Click += new System.EventHandler(this.butBadDebt_Click);
			// 
			// checkPaymentsPromptForPayType
			// 
			this.checkPaymentsPromptForPayType.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPaymentsPromptForPayType.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPaymentsPromptForPayType.Location = new System.Drawing.Point(49, 53);
			this.checkPaymentsPromptForPayType.Name = "checkPaymentsPromptForPayType";
			this.checkPaymentsPromptForPayType.Size = new System.Drawing.Size(396, 17);
			this.checkPaymentsPromptForPayType.TabIndex = 229;
			this.checkPaymentsPromptForPayType.Text = "Payments prompt for Payment Type";
			this.checkPaymentsPromptForPayType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboUnallocatedSplits
			// 
			this.comboUnallocatedSplits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboUnallocatedSplits.FormattingEnabled = true;
			this.comboUnallocatedSplits.Location = new System.Drawing.Point(282, 75);
			this.comboUnallocatedSplits.MaxDropDownItems = 30;
			this.comboUnallocatedSplits.Name = "comboUnallocatedSplits";
			this.comboUnallocatedSplits.Size = new System.Drawing.Size(163, 21);
			this.comboUnallocatedSplits.TabIndex = 221;
			// 
			// label28
			// 
			this.label28.BackColor = System.Drawing.SystemColors.Window;
			this.label28.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label28.Location = new System.Drawing.Point(43, 78);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(234, 15);
			this.label28.TabIndex = 222;
			this.label28.Text = "Default unearned type for unallocated paysplits";
			this.label28.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tabPageIns
			// 
			this.tabPageIns.Controls.Add(this.labelClaimCredit);
			this.tabPageIns.Controls.Add(this.comboClaimCredit);
			this.tabPageIns.Controls.Add(this.checkAllowFuturePayments);
			this.tabPageIns.Controls.Add(this.groupBoxClaimIdPrefix);
			this.tabPageIns.Controls.Add(this.checkAllowProcAdjFromClaim);
			this.tabPageIns.Controls.Add(this.checkProviderIncomeShows);
			this.tabPageIns.Controls.Add(this.checkClaimFormTreatDentSaysSigOnFile);
			this.tabPageIns.Controls.Add(this.checkClaimMedTypeIsInstWhenInsPlanIsMedical);
			this.tabPageIns.Controls.Add(this.checkEclaimsMedicalProvTreatmentAsOrdering);
			this.tabPageIns.Controls.Add(this.checkEclaimsSeparateTreatProv);
			this.tabPageIns.Controls.Add(this.label20);
			this.tabPageIns.Controls.Add(this.textClaimAttachPath);
			this.tabPageIns.Controls.Add(this.checkClaimsValidateACN);
			this.tabPageIns.Controls.Add(this.textInsWriteoffDescript);
			this.tabPageIns.Controls.Add(this.label17);
			this.tabPageIns.Location = new System.Drawing.Point(4, 22);
			this.tabPageIns.Name = "tabPageIns";
			this.tabPageIns.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageIns.Size = new System.Drawing.Size(457, 547);
			this.tabPageIns.TabIndex = 2;
			this.tabPageIns.Text = "Insurance";
			this.tabPageIns.UseVisualStyleBackColor = true;
			// 
			// labelClaimCredit
			// 
			this.labelClaimCredit.Location = new System.Drawing.Point(13, 191);
			this.labelClaimCredit.Name = "labelClaimCredit";
			this.labelClaimCredit.Size = new System.Drawing.Size(257, 17);
			this.labelClaimCredit.TabIndex = 244;
			this.labelClaimCredit.Text = "Credits greater than proc fee";
			this.labelClaimCredit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboClaimCredit
			// 
			this.comboClaimCredit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClaimCredit.FormattingEnabled = true;
			this.comboClaimCredit.Location = new System.Drawing.Point(277, 190);
			this.comboClaimCredit.Name = "comboClaimCredit";
			this.comboClaimCredit.Size = new System.Drawing.Size(168, 21);
			this.comboClaimCredit.TabIndex = 243;
			// 
			// checkAllowFuturePayments
			// 
			this.checkAllowFuturePayments.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowFuturePayments.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllowFuturePayments.Location = new System.Drawing.Point(23, 216);
			this.checkAllowFuturePayments.Name = "checkAllowFuturePayments";
			this.checkAllowFuturePayments.Size = new System.Drawing.Size(422, 17);
			this.checkAllowFuturePayments.TabIndex = 237;
			this.checkAllowFuturePayments.Text = "Allow Future Payments";
			this.checkAllowFuturePayments.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBoxClaimIdPrefix
			// 
			this.groupBoxClaimIdPrefix.Controls.Add(this.butReplacements);
			this.groupBoxClaimIdPrefix.Controls.Add(this.textClaimIdentifier);
			this.groupBoxClaimIdPrefix.Location = new System.Drawing.Point(266, 239);
			this.groupBoxClaimIdPrefix.Name = "groupBoxClaimIdPrefix";
			this.groupBoxClaimIdPrefix.Size = new System.Drawing.Size(179, 71);
			this.groupBoxClaimIdPrefix.TabIndex = 237;
			this.groupBoxClaimIdPrefix.TabStop = false;
			this.groupBoxClaimIdPrefix.Text = "Claim Identification Prefix";
			// 
			// butReplacements
			// 
			this.butReplacements.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butReplacements.Autosize = true;
			this.butReplacements.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butReplacements.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butReplacements.CornerRadius = 4F;
			this.butReplacements.Location = new System.Drawing.Point(68, 42);
			this.butReplacements.Name = "butReplacements";
			this.butReplacements.Size = new System.Drawing.Size(107, 23);
			this.butReplacements.TabIndex = 240;
			this.butReplacements.Text = "Replacements";
			this.butReplacements.UseVisualStyleBackColor = true;
			this.butReplacements.Click += new System.EventHandler(this.butReplacements_Click);
			// 
			// textClaimIdentifier
			// 
			this.textClaimIdentifier.Location = new System.Drawing.Point(4, 19);
			this.textClaimIdentifier.Name = "textClaimIdentifier";
			this.textClaimIdentifier.Size = new System.Drawing.Size(171, 20);
			this.textClaimIdentifier.TabIndex = 238;
			// 
			// checkAllowProcAdjFromClaim
			// 
			this.checkAllowProcAdjFromClaim.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowProcAdjFromClaim.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllowProcAdjFromClaim.Location = new System.Drawing.Point(23, 168);
			this.checkAllowProcAdjFromClaim.Name = "checkAllowProcAdjFromClaim";
			this.checkAllowProcAdjFromClaim.Size = new System.Drawing.Size(422, 17);
			this.checkAllowProcAdjFromClaim.TabIndex = 236;
			this.checkAllowProcAdjFromClaim.Text = "Allow procedure adjustments from claim window";
			this.checkAllowProcAdjFromClaim.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkProviderIncomeShows
			// 
			this.checkProviderIncomeShows.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProviderIncomeShows.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkProviderIncomeShows.Location = new System.Drawing.Point(6, 6);
			this.checkProviderIncomeShows.Name = "checkProviderIncomeShows";
			this.checkProviderIncomeShows.Size = new System.Drawing.Size(439, 17);
			this.checkProviderIncomeShows.TabIndex = 74;
			this.checkProviderIncomeShows.Text = "Show provider income transfer window after entering insurance payment";
			this.checkProviderIncomeShows.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkClaimFormTreatDentSaysSigOnFile
			// 
			this.checkClaimFormTreatDentSaysSigOnFile.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClaimFormTreatDentSaysSigOnFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClaimFormTreatDentSaysSigOnFile.Location = new System.Drawing.Point(23, 42);
			this.checkClaimFormTreatDentSaysSigOnFile.Name = "checkClaimFormTreatDentSaysSigOnFile";
			this.checkClaimFormTreatDentSaysSigOnFile.Size = new System.Drawing.Size(422, 17);
			this.checkClaimFormTreatDentSaysSigOnFile.TabIndex = 197;
			this.checkClaimFormTreatDentSaysSigOnFile.Text = "Claim Form treating provider shows Signature On File rather than name";
			this.checkClaimFormTreatDentSaysSigOnFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkClaimMedTypeIsInstWhenInsPlanIsMedical
			// 
			this.checkClaimMedTypeIsInstWhenInsPlanIsMedical.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClaimMedTypeIsInstWhenInsPlanIsMedical.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClaimMedTypeIsInstWhenInsPlanIsMedical.Location = new System.Drawing.Point(6, 24);
			this.checkClaimMedTypeIsInstWhenInsPlanIsMedical.Name = "checkClaimMedTypeIsInstWhenInsPlanIsMedical";
			this.checkClaimMedTypeIsInstWhenInsPlanIsMedical.Size = new System.Drawing.Size(439, 17);
			this.checkClaimMedTypeIsInstWhenInsPlanIsMedical.TabIndex = 194;
			this.checkClaimMedTypeIsInstWhenInsPlanIsMedical.Text = "Set medical claims to institutional when using medical insurance";
			this.checkClaimMedTypeIsInstWhenInsPlanIsMedical.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkEclaimsMedicalProvTreatmentAsOrdering
			// 
			this.checkEclaimsMedicalProvTreatmentAsOrdering.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEclaimsMedicalProvTreatmentAsOrdering.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkEclaimsMedicalProvTreatmentAsOrdering.Location = new System.Drawing.Point(23, 114);
			this.checkEclaimsMedicalProvTreatmentAsOrdering.Name = "checkEclaimsMedicalProvTreatmentAsOrdering";
			this.checkEclaimsMedicalProvTreatmentAsOrdering.Size = new System.Drawing.Size(422, 17);
			this.checkEclaimsMedicalProvTreatmentAsOrdering.TabIndex = 235;
			this.checkEclaimsMedicalProvTreatmentAsOrdering.Text = "On medical e-claims, send treating provider as ordering provider by default";
			this.checkEclaimsMedicalProvTreatmentAsOrdering.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkEclaimsSeparateTreatProv
			// 
			this.checkEclaimsSeparateTreatProv.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEclaimsSeparateTreatProv.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkEclaimsSeparateTreatProv.Location = new System.Drawing.Point(23, 132);
			this.checkEclaimsSeparateTreatProv.Name = "checkEclaimsSeparateTreatProv";
			this.checkEclaimsSeparateTreatProv.Size = new System.Drawing.Size(422, 17);
			this.checkEclaimsSeparateTreatProv.TabIndex = 53;
			this.checkEclaimsSeparateTreatProv.Text = "On e-claims, send treating provider info for each separate procedure";
			this.checkEclaimsSeparateTreatProv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label20
			// 
			this.label20.BackColor = System.Drawing.SystemColors.Window;
			this.label20.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label20.Location = new System.Drawing.Point(18, 91);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(224, 17);
			this.label20.TabIndex = 190;
			this.label20.Text = "Claim Attachment Export Path";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textClaimAttachPath
			// 
			this.textClaimAttachPath.Location = new System.Drawing.Point(248, 88);
			this.textClaimAttachPath.Name = "textClaimAttachPath";
			this.textClaimAttachPath.Size = new System.Drawing.Size(197, 20);
			this.textClaimAttachPath.TabIndex = 189;
			// 
			// checkClaimsValidateACN
			// 
			this.checkClaimsValidateACN.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClaimsValidateACN.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClaimsValidateACN.Location = new System.Drawing.Point(23, 150);
			this.checkClaimsValidateACN.Name = "checkClaimsValidateACN";
			this.checkClaimsValidateACN.Size = new System.Drawing.Size(422, 17);
			this.checkClaimsValidateACN.TabIndex = 194;
			this.checkClaimsValidateACN.Text = "Require ACN# in remarks on claims with ADDP group name";
			this.checkClaimsValidateACN.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsWriteoffDescript
			// 
			this.textInsWriteoffDescript.Location = new System.Drawing.Point(282, 64);
			this.textInsWriteoffDescript.Name = "textInsWriteoffDescript";
			this.textInsWriteoffDescript.Size = new System.Drawing.Size(163, 20);
			this.textInsWriteoffDescript.TabIndex = 207;
			// 
			// label17
			// 
			this.label17.BackColor = System.Drawing.SystemColors.Window;
			this.label17.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label17.Location = new System.Drawing.Point(18, 66);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(257, 17);
			this.label17.TabIndex = 208;
			this.label17.Text = "PPO writeoff description (blank for \"Writeoff\")";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabPageMisc
			// 
			this.tabPageMisc.Controls.Add(this.checkAllowFutureTrans);
			this.tabPageMisc.Controls.Add(this.groupCommLogs);
			this.tabPageMisc.Controls.Add(this.checkShowAllocateUnearnedPaymentPrompt);
			this.tabPageMisc.Controls.Add(this.checkAgeNegAdjsByAdjDate);
			this.tabPageMisc.Controls.Add(this.groupPayPlans);
			this.tabPageMisc.Controls.Add(this.checkStatementInvoiceGridShowWriteoffs);
			this.tabPageMisc.Controls.Add(this.checkBalancesDontSubtractIns);
			this.tabPageMisc.Controls.Add(this.checkAgingMonthly);
			this.tabPageMisc.Controls.Add(this.checkAccountShowPaymentNums);
			this.tabPageMisc.Controls.Add(this.checkRecurringChargesUseTransDate);
			this.tabPageMisc.Controls.Add(this.checkStatementsUseSheets);
			this.tabPageMisc.Controls.Add(this.checkRecurChargPriProv);
			this.tabPageMisc.Controls.Add(this.checkPpoUseUcr);
			this.tabPageMisc.Location = new System.Drawing.Point(4, 22);
			this.tabPageMisc.Name = "tabPageMisc";
			this.tabPageMisc.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageMisc.Size = new System.Drawing.Size(457, 547);
			this.tabPageMisc.TabIndex = 3;
			this.tabPageMisc.Text = "Misc Account";
			this.tabPageMisc.UseVisualStyleBackColor = true;
			// 
			// checkAllowFutureTrans
			// 
			this.checkAllowFutureTrans.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowFutureTrans.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllowFutureTrans.Location = new System.Drawing.Point(6, 186);
			this.checkAllowFutureTrans.Name = "checkAllowFutureTrans";
			this.checkAllowFutureTrans.Size = new System.Drawing.Size(439, 17);
			this.checkAllowFutureTrans.TabIndex = 244;
			this.checkAllowFutureTrans.Text = "Allow future dated transactions";
			this.checkAllowFutureTrans.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupCommLogs
			// 
			this.groupCommLogs.Controls.Add(this.checkCommLogAutoSave);
			this.groupCommLogs.Controls.Add(this.checkShowFamilyCommByDefault);
			this.groupCommLogs.Location = new System.Drawing.Point(6, 307);
			this.groupCommLogs.Name = "groupCommLogs";
			this.groupCommLogs.Size = new System.Drawing.Size(445, 54);
			this.groupCommLogs.TabIndex = 243;
			this.groupCommLogs.TabStop = false;
			this.groupCommLogs.Text = "Comm Logs";
			// 
			// checkCommLogAutoSave
			// 
			this.checkCommLogAutoSave.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkCommLogAutoSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkCommLogAutoSave.Location = new System.Drawing.Point(4, 12);
			this.checkCommLogAutoSave.Name = "checkCommLogAutoSave";
			this.checkCommLogAutoSave.Size = new System.Drawing.Size(435, 17);
			this.checkCommLogAutoSave.TabIndex = 225;
			this.checkCommLogAutoSave.Text = "Commlogs Auto Save";
			this.checkCommLogAutoSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkCommLogAutoSave.UseVisualStyleBackColor = true;
			// 
			// checkShowFamilyCommByDefault
			// 
			this.checkShowFamilyCommByDefault.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowFamilyCommByDefault.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowFamilyCommByDefault.Location = new System.Drawing.Point(0, 30);
			this.checkShowFamilyCommByDefault.Name = "checkShowFamilyCommByDefault";
			this.checkShowFamilyCommByDefault.Size = new System.Drawing.Size(439, 17);
			this.checkShowFamilyCommByDefault.TabIndex = 75;
			this.checkShowFamilyCommByDefault.Text = "Show Family Comm Entries By Default";
			this.checkShowFamilyCommByDefault.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkShowAllocateUnearnedPaymentPrompt
			// 
			this.checkShowAllocateUnearnedPaymentPrompt.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowAllocateUnearnedPaymentPrompt.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowAllocateUnearnedPaymentPrompt.Location = new System.Drawing.Point(6, 168);
			this.checkShowAllocateUnearnedPaymentPrompt.Name = "checkShowAllocateUnearnedPaymentPrompt";
			this.checkShowAllocateUnearnedPaymentPrompt.Size = new System.Drawing.Size(439, 17);
			this.checkShowAllocateUnearnedPaymentPrompt.TabIndex = 242;
			this.checkShowAllocateUnearnedPaymentPrompt.Text = "Prompt user to allocate unearned income after creating a claim";
			this.checkShowAllocateUnearnedPaymentPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkAgeNegAdjsByAdjDate
			// 
			this.checkAgeNegAdjsByAdjDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAgeNegAdjsByAdjDate.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAgeNegAdjsByAdjDate.Location = new System.Drawing.Point(6, 150);
			this.checkAgeNegAdjsByAdjDate.Name = "checkAgeNegAdjsByAdjDate";
			this.checkAgeNegAdjsByAdjDate.Size = new System.Drawing.Size(439, 17);
			this.checkAgeNegAdjsByAdjDate.TabIndex = 241;
			this.checkAgeNegAdjsByAdjDate.Text = "Age negative adjustments by adjustment date";
			this.checkAgeNegAdjsByAdjDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAgeNegAdjsByAdjDate.Click += new System.EventHandler(this.checkAgeAdjByAdjDate_Click);
			// 
			// groupPayPlans
			// 
			this.groupPayPlans.Controls.Add(this.label27);
			this.groupPayPlans.Controls.Add(this.comboPayPlansVersion);
			this.groupPayPlans.Controls.Add(this.checkHideDueNow);
			this.groupPayPlans.Controls.Add(this.checkPayPlansUseSheets);
			this.groupPayPlans.Controls.Add(this.checkPayPlansExcludePastActivity);
			this.groupPayPlans.Location = new System.Drawing.Point(6, 204);
			this.groupPayPlans.Name = "groupPayPlans";
			this.groupPayPlans.Size = new System.Drawing.Size(445, 100);
			this.groupPayPlans.TabIndex = 240;
			this.groupPayPlans.TabStop = false;
			this.groupPayPlans.Text = "Pay Plans";
			// 
			// label27
			// 
			this.label27.BackColor = System.Drawing.SystemColors.Window;
			this.label27.Location = new System.Drawing.Point(6, 54);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(238, 17);
			this.label27.TabIndex = 242;
			this.label27.Text = "Pay Plan Charge Logic:";
			this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboPayPlansVersion
			// 
			this.comboPayPlansVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPayPlansVersion.FormattingEnabled = true;
			this.comboPayPlansVersion.Location = new System.Drawing.Point(245, 51);
			this.comboPayPlansVersion.Name = "comboPayPlansVersion";
			this.comboPayPlansVersion.Size = new System.Drawing.Size(194, 21);
			this.comboPayPlansVersion.TabIndex = 241;
			this.comboPayPlansVersion.SelectionChangeCommitted += new System.EventHandler(this.comboPayPlansVersion_SelectionChangeCommitted);
			// 
			// checkHideDueNow
			// 
			this.checkHideDueNow.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHideDueNow.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkHideDueNow.Location = new System.Drawing.Point(6, 76);
			this.checkHideDueNow.Name = "checkHideDueNow";
			this.checkHideDueNow.Size = new System.Drawing.Size(433, 17);
			this.checkHideDueNow.TabIndex = 239;
			this.checkHideDueNow.Text = "Hide \"Due Now\" in Payment Plans Grid";
			this.checkHideDueNow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkPayPlansUseSheets
			// 
			this.checkPayPlansUseSheets.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPayPlansUseSheets.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPayPlansUseSheets.Location = new System.Drawing.Point(6, 30);
			this.checkPayPlansUseSheets.Name = "checkPayPlansUseSheets";
			this.checkPayPlansUseSheets.Size = new System.Drawing.Size(433, 17);
			this.checkPayPlansUseSheets.TabIndex = 227;
			this.checkPayPlansUseSheets.Text = "Pay Plans use Sheets";
			this.checkPayPlansUseSheets.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkPayPlansExcludePastActivity
			// 
			this.checkPayPlansExcludePastActivity.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPayPlansExcludePastActivity.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPayPlansExcludePastActivity.Location = new System.Drawing.Point(6, 12);
			this.checkPayPlansExcludePastActivity.Name = "checkPayPlansExcludePastActivity";
			this.checkPayPlansExcludePastActivity.Size = new System.Drawing.Size(433, 17);
			this.checkPayPlansExcludePastActivity.TabIndex = 236;
			this.checkPayPlansExcludePastActivity.Text = "Payment Plans exclude past activity by default";
			this.checkPayPlansExcludePastActivity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkStatementInvoiceGridShowWriteoffs
			// 
			this.checkStatementInvoiceGridShowWriteoffs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkStatementInvoiceGridShowWriteoffs.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkStatementInvoiceGridShowWriteoffs.Location = new System.Drawing.Point(6, 132);
			this.checkStatementInvoiceGridShowWriteoffs.Name = "checkStatementInvoiceGridShowWriteoffs";
			this.checkStatementInvoiceGridShowWriteoffs.Size = new System.Drawing.Size(439, 17);
			this.checkStatementInvoiceGridShowWriteoffs.TabIndex = 238;
			this.checkStatementInvoiceGridShowWriteoffs.Text = "Invoices\' payments grid show writeoffs\r\n";
			this.checkStatementInvoiceGridShowWriteoffs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkBalancesDontSubtractIns
			// 
			this.checkBalancesDontSubtractIns.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBalancesDontSubtractIns.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBalancesDontSubtractIns.Location = new System.Drawing.Point(6, 6);
			this.checkBalancesDontSubtractIns.Name = "checkBalancesDontSubtractIns";
			this.checkBalancesDontSubtractIns.Size = new System.Drawing.Size(439, 17);
			this.checkBalancesDontSubtractIns.TabIndex = 55;
			this.checkBalancesDontSubtractIns.Text = "Balances don\'t subtract insurance estimate";
			this.checkBalancesDontSubtractIns.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkAgingMonthly
			// 
			this.checkAgingMonthly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAgingMonthly.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAgingMonthly.Location = new System.Drawing.Point(6, 24);
			this.checkAgingMonthly.Name = "checkAgingMonthly";
			this.checkAgingMonthly.Size = new System.Drawing.Size(439, 17);
			this.checkAgingMonthly.TabIndex = 57;
			this.checkAgingMonthly.Text = "Aging calculated monthly instead of daily";
			this.checkAgingMonthly.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkAccountShowPaymentNums
			// 
			this.checkAccountShowPaymentNums.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAccountShowPaymentNums.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAccountShowPaymentNums.Location = new System.Drawing.Point(6, 42);
			this.checkAccountShowPaymentNums.Name = "checkAccountShowPaymentNums";
			this.checkAccountShowPaymentNums.Size = new System.Drawing.Size(439, 17);
			this.checkAccountShowPaymentNums.TabIndex = 194;
			this.checkAccountShowPaymentNums.Text = "Show Payment Numbers in Account Module";
			this.checkAccountShowPaymentNums.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkRecurringChargesUseTransDate
			// 
			this.checkRecurringChargesUseTransDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRecurringChargesUseTransDate.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRecurringChargesUseTransDate.Location = new System.Drawing.Point(6, 114);
			this.checkRecurringChargesUseTransDate.Name = "checkRecurringChargesUseTransDate";
			this.checkRecurringChargesUseTransDate.Size = new System.Drawing.Size(439, 17);
			this.checkRecurringChargesUseTransDate.TabIndex = 237;
			this.checkRecurringChargesUseTransDate.Text = "Recurring charges use transaction date";
			this.checkRecurringChargesUseTransDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkStatementsUseSheets
			// 
			this.checkStatementsUseSheets.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkStatementsUseSheets.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkStatementsUseSheets.Location = new System.Drawing.Point(6, 60);
			this.checkStatementsUseSheets.Name = "checkStatementsUseSheets";
			this.checkStatementsUseSheets.Size = new System.Drawing.Size(439, 17);
			this.checkStatementsUseSheets.TabIndex = 204;
			this.checkStatementsUseSheets.Text = "Statements use Sheets";
			this.checkStatementsUseSheets.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkRecurChargPriProv
			// 
			this.checkRecurChargPriProv.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRecurChargPriProv.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRecurChargPriProv.Location = new System.Drawing.Point(6, 78);
			this.checkRecurChargPriProv.Name = "checkRecurChargPriProv";
			this.checkRecurChargPriProv.Size = new System.Drawing.Size(439, 17);
			this.checkRecurChargPriProv.TabIndex = 209;
			this.checkRecurChargPriProv.Text = "Recurring charges use primary provider";
			this.checkRecurChargPriProv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkPpoUseUcr
			// 
			this.checkPpoUseUcr.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPpoUseUcr.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPpoUseUcr.Location = new System.Drawing.Point(6, 96);
			this.checkPpoUseUcr.Name = "checkPpoUseUcr";
			this.checkPpoUseUcr.Size = new System.Drawing.Size(439, 17);
			this.checkPpoUseUcr.TabIndex = 228;
			this.checkPpoUseUcr.Text = "Use UCR fee for billed fee even if PPO fee is higher";
			this.checkPpoUseUcr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabTreatPlan
			// 
			this.tabTreatPlan.BackColor = System.Drawing.SystemColors.Window;
			this.tabTreatPlan.Controls.Add(this.groupBox6);
			this.tabTreatPlan.Controls.Add(this.groupTreatPlanSort);
			this.tabTreatPlan.Controls.Add(this.checkTPSaveSigned);
			this.tabTreatPlan.Controls.Add(this.checkTreatPlanItemized);
			this.tabTreatPlan.Controls.Add(this.textDiscountPercentage);
			this.tabTreatPlan.Controls.Add(this.labelDiscountPercentage);
			this.tabTreatPlan.Controls.Add(this.comboProcDiscountType);
			this.tabTreatPlan.Controls.Add(this.label19);
			this.tabTreatPlan.Controls.Add(this.label1);
			this.tabTreatPlan.Controls.Add(this.checkTreatPlanShowCompleted);
			this.tabTreatPlan.Controls.Add(this.textTreatNote);
			this.tabTreatPlan.Location = new System.Drawing.Point(4, 22);
			this.tabTreatPlan.Name = "tabTreatPlan";
			this.tabTreatPlan.Padding = new System.Windows.Forms.Padding(3);
			this.tabTreatPlan.Size = new System.Drawing.Size(471, 579);
			this.tabTreatPlan.TabIndex = 3;
			this.tabTreatPlan.Text = "Treat\' Plan";
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.textInsExam);
			this.groupBox6.Controls.Add(this.label35);
			this.groupBox6.Controls.Add(this.checkFrequency);
			this.groupBox6.Controls.Add(this.textInsBW);
			this.groupBox6.Controls.Add(this.label34);
			this.groupBox6.Controls.Add(this.textInsPano);
			this.groupBox6.Controls.Add(this.label36);
			this.groupBox6.Location = new System.Drawing.Point(6, 189);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(438, 127);
			this.groupBox6.TabIndex = 231;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Frequency Limitations";
			// 
			// textInsExam
			// 
			this.textInsExam.Location = new System.Drawing.Point(261, 101);
			this.textInsExam.Name = "textInsExam";
			this.textInsExam.Size = new System.Drawing.Size(173, 20);
			this.textInsExam.TabIndex = 225;
			// 
			// label35
			// 
			this.label35.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label35.Location = new System.Drawing.Point(14, 79);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(246, 17);
			this.label35.TabIndex = 229;
			this.label35.Text = "Pano/FMX Codes (comma separated)";
			this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkFrequency
			// 
			this.checkFrequency.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkFrequency.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkFrequency.Location = new System.Drawing.Point(132, 15);
			this.checkFrequency.Name = "checkFrequency";
			this.checkFrequency.Size = new System.Drawing.Size(302, 17);
			this.checkFrequency.TabIndex = 216;
			this.checkFrequency.Text = "Enable Insurance Frequency Checking";
			this.checkFrequency.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkFrequency.UseVisualStyleBackColor = false;
			this.checkFrequency.Click += new System.EventHandler(this.checkFrequency_Click);
			// 
			// textInsBW
			// 
			this.textInsBW.Location = new System.Drawing.Point(261, 55);
			this.textInsBW.Name = "textInsBW";
			this.textInsBW.Size = new System.Drawing.Size(173, 20);
			this.textInsBW.TabIndex = 223;
			// 
			// label34
			// 
			this.label34.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label34.Location = new System.Drawing.Point(14, 102);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(246, 17);
			this.label34.TabIndex = 228;
			this.label34.Text = "Exam Codes (comma separated)";
			this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textInsPano
			// 
			this.textInsPano.Location = new System.Drawing.Point(261, 78);
			this.textInsPano.Name = "textInsPano";
			this.textInsPano.Size = new System.Drawing.Size(173, 20);
			this.textInsPano.TabIndex = 224;
			// 
			// label36
			// 
			this.label36.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label36.Location = new System.Drawing.Point(14, 56);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(246, 17);
			this.label36.TabIndex = 227;
			this.label36.Text = "Bitewing Codes (comma separated)";
			this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupTreatPlanSort
			// 
			this.groupTreatPlanSort.Controls.Add(this.radioTreatPlanSortTooth);
			this.groupTreatPlanSort.Controls.Add(this.radioTreatPlanSortOrder);
			this.groupTreatPlanSort.Location = new System.Drawing.Point(273, 319);
			this.groupTreatPlanSort.Name = "groupTreatPlanSort";
			this.groupTreatPlanSort.Size = new System.Drawing.Size(171, 55);
			this.groupTreatPlanSort.TabIndex = 218;
			this.groupTreatPlanSort.TabStop = false;
			this.groupTreatPlanSort.Text = "Sort Procedures By";
			// 
			// radioTreatPlanSortTooth
			// 
			this.radioTreatPlanSortTooth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.radioTreatPlanSortTooth.Location = new System.Drawing.Point(8, 16);
			this.radioTreatPlanSortTooth.Name = "radioTreatPlanSortTooth";
			this.radioTreatPlanSortTooth.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.radioTreatPlanSortTooth.Size = new System.Drawing.Size(157, 15);
			this.radioTreatPlanSortTooth.TabIndex = 54;
			this.radioTreatPlanSortTooth.Text = "Tooth";
			this.radioTreatPlanSortTooth.UseVisualStyleBackColor = true;
			this.radioTreatPlanSortTooth.Click += new System.EventHandler(this.radioTreatPlanSortTooth_Click);
			// 
			// radioTreatPlanSortOrder
			// 
			this.radioTreatPlanSortOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.radioTreatPlanSortOrder.Checked = true;
			this.radioTreatPlanSortOrder.Location = new System.Drawing.Point(8, 33);
			this.radioTreatPlanSortOrder.Name = "radioTreatPlanSortOrder";
			this.radioTreatPlanSortOrder.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.radioTreatPlanSortOrder.Size = new System.Drawing.Size(157, 15);
			this.radioTreatPlanSortOrder.TabIndex = 53;
			this.radioTreatPlanSortOrder.TabStop = true;
			this.radioTreatPlanSortOrder.Text = "Order Entered";
			this.radioTreatPlanSortOrder.UseVisualStyleBackColor = true;
			this.radioTreatPlanSortOrder.Click += new System.EventHandler(this.radioTreatPlanSortOrder_Click);
			// 
			// checkTPSaveSigned
			// 
			this.checkTPSaveSigned.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTPSaveSigned.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTPSaveSigned.Location = new System.Drawing.Point(138, 169);
			this.checkTPSaveSigned.Name = "checkTPSaveSigned";
			this.checkTPSaveSigned.Size = new System.Drawing.Size(302, 17);
			this.checkTPSaveSigned.TabIndex = 213;
			this.checkTPSaveSigned.Text = "Save Signed Treatment Plans to PDF";
			this.checkTPSaveSigned.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTPSaveSigned.UseVisualStyleBackColor = false;
			// 
			// checkTreatPlanItemized
			// 
			this.checkTreatPlanItemized.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTreatPlanItemized.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTreatPlanItemized.Location = new System.Drawing.Point(300, 152);
			this.checkTreatPlanItemized.Name = "checkTreatPlanItemized";
			this.checkTreatPlanItemized.Size = new System.Drawing.Size(140, 17);
			this.checkTreatPlanItemized.TabIndex = 212;
			this.checkTreatPlanItemized.Text = "Itemize Treatment Plan";
			this.checkTreatPlanItemized.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTreatPlanItemized.UseVisualStyleBackColor = false;
			this.checkTreatPlanItemized.Click += new System.EventHandler(this.checkTreatPlanItemized_Click);
			// 
			// textDiscountPercentage
			// 
			this.textDiscountPercentage.Location = new System.Drawing.Point(387, 129);
			this.textDiscountPercentage.Name = "textDiscountPercentage";
			this.textDiscountPercentage.Size = new System.Drawing.Size(53, 20);
			this.textDiscountPercentage.TabIndex = 211;
			// 
			// labelDiscountPercentage
			// 
			this.labelDiscountPercentage.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelDiscountPercentage.Location = new System.Drawing.Point(135, 132);
			this.labelDiscountPercentage.Name = "labelDiscountPercentage";
			this.labelDiscountPercentage.Size = new System.Drawing.Size(246, 16);
			this.labelDiscountPercentage.TabIndex = 210;
			this.labelDiscountPercentage.Text = "Procedure discount percentage";
			this.labelDiscountPercentage.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboProcDiscountType
			// 
			this.comboProcDiscountType.FormattingEnabled = true;
			this.comboProcDiscountType.Location = new System.Drawing.Point(277, 102);
			this.comboProcDiscountType.MaxDropDownItems = 30;
			this.comboProcDiscountType.Name = "comboProcDiscountType";
			this.comboProcDiscountType.Size = new System.Drawing.Size(163, 21);
			this.comboProcDiscountType.TabIndex = 201;
			// 
			// label19
			// 
			this.label19.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label19.Location = new System.Drawing.Point(55, 105);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(221, 15);
			this.label19.TabIndex = 200;
			this.label19.Text = "Procedure discount adj type";
			this.label19.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(28, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 52);
			this.label1.TabIndex = 35;
			this.label1.Text = "Default Note";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkTreatPlanShowCompleted
			// 
			this.checkTreatPlanShowCompleted.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTreatPlanShowCompleted.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTreatPlanShowCompleted.Location = new System.Drawing.Point(81, 79);
			this.checkTreatPlanShowCompleted.Name = "checkTreatPlanShowCompleted";
			this.checkTreatPlanShowCompleted.Size = new System.Drawing.Size(359, 17);
			this.checkTreatPlanShowCompleted.TabIndex = 47;
			this.checkTreatPlanShowCompleted.Text = "Show Completed Work on Graphical Tooth Chart";
			this.checkTreatPlanShowCompleted.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textTreatNote
			// 
			this.textTreatNote.AcceptsTab = true;
			this.textTreatNote.BackColor = System.Drawing.SystemColors.Window;
			this.textTreatNote.DetectLinksEnabled = false;
			this.textTreatNote.DetectUrls = false;
			this.textTreatNote.Location = new System.Drawing.Point(77, 7);
			this.textTreatNote.MaxLength = 32767;
			this.textTreatNote.Name = "textTreatNote";
			this.textTreatNote.QuickPasteType = OpenDentBusiness.QuickPasteType.TreatPlan;
			this.textTreatNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textTreatNote.Size = new System.Drawing.Size(363, 66);
			this.textTreatNote.TabIndex = 215;
			this.textTreatNote.Text = "";
			// 
			// tabChart
			// 
			this.tabChart.BackColor = System.Drawing.SystemColors.Window;
			this.tabChart.Controls.Add(this.tabControl1);
			this.tabChart.Location = new System.Drawing.Point(4, 22);
			this.tabChart.Name = "tabChart";
			this.tabChart.Padding = new System.Windows.Forms.Padding(3);
			this.tabChart.Size = new System.Drawing.Size(471, 579);
			this.tabChart.TabIndex = 4;
			this.tabChart.Text = "Chart";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabChartBehavior);
			this.tabControl1.Controls.Add(this.tabChartAppearance);
			this.tabControl1.Location = new System.Drawing.Point(3, 3);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(465, 573);
			this.tabControl1.TabIndex = 230;
			// 
			// tabChartBehavior
			// 
			this.tabChartBehavior.Controls.Add(this.checkShowPlannedApptPrompt);
			this.tabChartBehavior.Controls.Add(this.checkAllowSettingProcsComplete);
			this.tabChartBehavior.Controls.Add(this.checkIsAlertRadiologyProcsEnabled);
			this.tabChartBehavior.Controls.Add(this.textProblemsIndicateNone);
			this.tabChartBehavior.Controls.Add(this.checkBoxRxClinicUseSelected);
			this.tabChartBehavior.Controls.Add(this.label8);
			this.tabChartBehavior.Controls.Add(this.checkProcProvChangesCp);
			this.tabChartBehavior.Controls.Add(this.comboProcFeeUpdatePrompt);
			this.tabChartBehavior.Controls.Add(this.labelProcFeeUpdatePrompt);
			this.tabChartBehavior.Controls.Add(this.butProblemsIndicateNone);
			this.tabChartBehavior.Controls.Add(this.label9);
			this.tabChartBehavior.Controls.Add(this.textMedicationsIndicateNone);
			this.tabChartBehavior.Controls.Add(this.checkSignatureAllowDigital);
			this.tabChartBehavior.Controls.Add(this.butMedicationsIndicateNone);
			this.tabChartBehavior.Controls.Add(this.checkClaimProcsAllowEstimatesOnCompl);
			this.tabChartBehavior.Controls.Add(this.checkProcEditRequireAutoCode);
			this.tabChartBehavior.Controls.Add(this.label14);
			this.tabChartBehavior.Controls.Add(this.checkProcsPromptForAutoNote);
			this.tabChartBehavior.Controls.Add(this.textAllergiesIndicateNone);
			this.tabChartBehavior.Controls.Add(this.checkScreeningsUseSheets);
			this.tabChartBehavior.Controls.Add(this.butAllergiesIndicateNone);
			this.tabChartBehavior.Controls.Add(this.checkMedicalFeeUsedForNewProcs);
			this.tabChartBehavior.Controls.Add(this.checkDxIcdVersion);
			this.tabChartBehavior.Controls.Add(this.butDiagnosisCode);
			this.tabChartBehavior.Controls.Add(this.labelIcdCodeDefault);
			this.tabChartBehavior.Controls.Add(this.textICD9DefaultForNewProcs);
			this.tabChartBehavior.Controls.Add(this.checkProcLockingIsAllowed);
			this.tabChartBehavior.Controls.Add(this.checkChartNonPatientWarn);
			this.tabChartBehavior.Controls.Add(this.label11);
			this.tabChartBehavior.Controls.Add(this.textMedDefaultStopDays);
			this.tabChartBehavior.Location = new System.Drawing.Point(4, 22);
			this.tabChartBehavior.Name = "tabChartBehavior";
			this.tabChartBehavior.Padding = new System.Windows.Forms.Padding(3);
			this.tabChartBehavior.Size = new System.Drawing.Size(457, 547);
			this.tabChartBehavior.TabIndex = 0;
			this.tabChartBehavior.Text = "Behavior";
			this.tabChartBehavior.UseVisualStyleBackColor = true;
			// 
			// checkShowPlannedApptPrompt
			// 
			this.checkShowPlannedApptPrompt.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowPlannedApptPrompt.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowPlannedApptPrompt.Location = new System.Drawing.Point(6, 412);
			this.checkShowPlannedApptPrompt.Name = "checkShowPlannedApptPrompt";
			this.checkShowPlannedApptPrompt.Size = new System.Drawing.Size(435, 17);
			this.checkShowPlannedApptPrompt.TabIndex = 230;
			this.checkShowPlannedApptPrompt.Text = "Prompt for Planned Appointment";
			this.checkShowPlannedApptPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowPlannedApptPrompt.UseVisualStyleBackColor = true;
			// 
			// checkAllowSettingProcsComplete
			// 
			this.checkAllowSettingProcsComplete.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowSettingProcsComplete.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllowSettingProcsComplete.Location = new System.Drawing.Point(6, 6);
			this.checkAllowSettingProcsComplete.Name = "checkAllowSettingProcsComplete";
			this.checkAllowSettingProcsComplete.Size = new System.Drawing.Size(435, 17);
			this.checkAllowSettingProcsComplete.TabIndex = 74;
			this.checkAllowSettingProcsComplete.Text = "Allow setting procedures complete.  (It\'s better to only set appointments complet" +
    "e)";
			this.checkAllowSettingProcsComplete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAllowSettingProcsComplete.UseVisualStyleBackColor = true;
			// 
			// textProblemsIndicateNone
			// 
			this.textProblemsIndicateNone.Location = new System.Drawing.Point(270, 27);
			this.textProblemsIndicateNone.Name = "textProblemsIndicateNone";
			this.textProblemsIndicateNone.ReadOnly = true;
			this.textProblemsIndicateNone.Size = new System.Drawing.Size(146, 20);
			this.textProblemsIndicateNone.TabIndex = 198;
			// 
			// checkBoxRxClinicUseSelected
			// 
			this.checkBoxRxClinicUseSelected.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBoxRxClinicUseSelected.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxRxClinicUseSelected.Location = new System.Drawing.Point(6, 376);
			this.checkBoxRxClinicUseSelected.Name = "checkBoxRxClinicUseSelected";
			this.checkBoxRxClinicUseSelected.Size = new System.Drawing.Size(435, 17);
			this.checkBoxRxClinicUseSelected.TabIndex = 228;
			this.checkBoxRxClinicUseSelected.Text = "Rx use selected clinic from Clinics menu instead of selected patient\'s default cl" +
    "inic";
			this.checkBoxRxClinicUseSelected.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBoxRxClinicUseSelected.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(5, 30);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(263, 15);
			this.label8.TabIndex = 197;
			this.label8.Text = "Indicator that patient has No Problems";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkProcProvChangesCp
			// 
			this.checkProcProvChangesCp.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcProvChangesCp.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkProcProvChangesCp.Location = new System.Drawing.Point(6, 358);
			this.checkProcProvChangesCp.Name = "checkProcProvChangesCp";
			this.checkProcProvChangesCp.Size = new System.Drawing.Size(435, 17);
			this.checkProcProvChangesCp.TabIndex = 227;
			this.checkProcProvChangesCp.Text = "Do not allow different procedure and claim procedure providers when attached to c" +
    "laim";
			this.checkProcProvChangesCp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcProvChangesCp.UseVisualStyleBackColor = true;
			// 
			// comboProcFeeUpdatePrompt
			// 
			this.comboProcFeeUpdatePrompt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProcFeeUpdatePrompt.FormattingEnabled = true;
			this.comboProcFeeUpdatePrompt.Location = new System.Drawing.Point(222, 332);
			this.comboProcFeeUpdatePrompt.Name = "comboProcFeeUpdatePrompt";
			this.comboProcFeeUpdatePrompt.Size = new System.Drawing.Size(219, 21);
			this.comboProcFeeUpdatePrompt.TabIndex = 225;
			// 
			// labelProcFeeUpdatePrompt
			// 
			this.labelProcFeeUpdatePrompt.Location = new System.Drawing.Point(5, 335);
			this.labelProcFeeUpdatePrompt.Name = "labelProcFeeUpdatePrompt";
			this.labelProcFeeUpdatePrompt.Size = new System.Drawing.Size(215, 15);
			this.labelProcFeeUpdatePrompt.TabIndex = 226;
			this.labelProcFeeUpdatePrompt.Text = "Procedure Fee Update Behavior";
			this.labelProcFeeUpdatePrompt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butProblemsIndicateNone
			// 
			this.butProblemsIndicateNone.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butProblemsIndicateNone.Autosize = true;
			this.butProblemsIndicateNone.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butProblemsIndicateNone.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butProblemsIndicateNone.CornerRadius = 4F;
			this.butProblemsIndicateNone.Location = new System.Drawing.Point(420, 27);
			this.butProblemsIndicateNone.Name = "butProblemsIndicateNone";
			this.butProblemsIndicateNone.Size = new System.Drawing.Size(21, 21);
			this.butProblemsIndicateNone.TabIndex = 199;
			this.butProblemsIndicateNone.Text = "...";
			this.butProblemsIndicateNone.Click += new System.EventHandler(this.butProblemsIndicateNone_Click);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(5, 55);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(263, 15);
			this.label9.TabIndex = 200;
			this.label9.Text = "Indicator that patient has No Medications";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textMedicationsIndicateNone
			// 
			this.textMedicationsIndicateNone.Location = new System.Drawing.Point(270, 52);
			this.textMedicationsIndicateNone.Name = "textMedicationsIndicateNone";
			this.textMedicationsIndicateNone.ReadOnly = true;
			this.textMedicationsIndicateNone.Size = new System.Drawing.Size(146, 20);
			this.textMedicationsIndicateNone.TabIndex = 201;
			// 
			// checkSignatureAllowDigital
			// 
			this.checkSignatureAllowDigital.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSignatureAllowDigital.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSignatureAllowDigital.Location = new System.Drawing.Point(6, 310);
			this.checkSignatureAllowDigital.Name = "checkSignatureAllowDigital";
			this.checkSignatureAllowDigital.Size = new System.Drawing.Size(435, 17);
			this.checkSignatureAllowDigital.TabIndex = 223;
			this.checkSignatureAllowDigital.Text = "Allow digital signatures";
			this.checkSignatureAllowDigital.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSignatureAllowDigital.UseVisualStyleBackColor = true;
			// 
			// butMedicationsIndicateNone
			// 
			this.butMedicationsIndicateNone.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMedicationsIndicateNone.Autosize = true;
			this.butMedicationsIndicateNone.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMedicationsIndicateNone.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMedicationsIndicateNone.CornerRadius = 4F;
			this.butMedicationsIndicateNone.Location = new System.Drawing.Point(420, 52);
			this.butMedicationsIndicateNone.Name = "butMedicationsIndicateNone";
			this.butMedicationsIndicateNone.Size = new System.Drawing.Size(21, 21);
			this.butMedicationsIndicateNone.TabIndex = 202;
			this.butMedicationsIndicateNone.Text = "...";
			this.butMedicationsIndicateNone.Click += new System.EventHandler(this.butMedicationsIndicateNone_Click);
			// 
			// checkClaimProcsAllowEstimatesOnCompl
			// 
			this.checkClaimProcsAllowEstimatesOnCompl.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClaimProcsAllowEstimatesOnCompl.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClaimProcsAllowEstimatesOnCompl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.checkClaimProcsAllowEstimatesOnCompl.Location = new System.Drawing.Point(6, 282);
			this.checkClaimProcsAllowEstimatesOnCompl.Name = "checkClaimProcsAllowEstimatesOnCompl";
			this.checkClaimProcsAllowEstimatesOnCompl.Size = new System.Drawing.Size(435, 25);
			this.checkClaimProcsAllowEstimatesOnCompl.TabIndex = 222;
			this.checkClaimProcsAllowEstimatesOnCompl.Text = "Allow estimates to be created for backdated completed procedures\r\n(not recommende" +
    "d, see manual)";
			this.checkClaimProcsAllowEstimatesOnCompl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClaimProcsAllowEstimatesOnCompl.UseVisualStyleBackColor = true;
			// 
			// checkProcEditRequireAutoCode
			// 
			this.checkProcEditRequireAutoCode.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcEditRequireAutoCode.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkProcEditRequireAutoCode.Location = new System.Drawing.Point(6, 264);
			this.checkProcEditRequireAutoCode.Name = "checkProcEditRequireAutoCode";
			this.checkProcEditRequireAutoCode.Size = new System.Drawing.Size(435, 17);
			this.checkProcEditRequireAutoCode.TabIndex = 221;
			this.checkProcEditRequireAutoCode.Text = "Require use of suggested auto codes";
			this.checkProcEditRequireAutoCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcEditRequireAutoCode.UseVisualStyleBackColor = true;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(5, 80);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(263, 15);
			this.label14.TabIndex = 203;
			this.label14.Text = "Indicator that patient has No Allergies";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkProcsPromptForAutoNote
			// 
			this.checkProcsPromptForAutoNote.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcsPromptForAutoNote.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkProcsPromptForAutoNote.Location = new System.Drawing.Point(6, 246);
			this.checkProcsPromptForAutoNote.Name = "checkProcsPromptForAutoNote";
			this.checkProcsPromptForAutoNote.Size = new System.Drawing.Size(435, 17);
			this.checkProcsPromptForAutoNote.TabIndex = 218;
			this.checkProcsPromptForAutoNote.Text = "Procedures Prompt For Auto Note";
			this.checkProcsPromptForAutoNote.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcsPromptForAutoNote.UseVisualStyleBackColor = true;
			// 
			// textAllergiesIndicateNone
			// 
			this.textAllergiesIndicateNone.Location = new System.Drawing.Point(270, 77);
			this.textAllergiesIndicateNone.Name = "textAllergiesIndicateNone";
			this.textAllergiesIndicateNone.ReadOnly = true;
			this.textAllergiesIndicateNone.Size = new System.Drawing.Size(146, 20);
			this.textAllergiesIndicateNone.TabIndex = 204;
			// 
			// checkScreeningsUseSheets
			// 
			this.checkScreeningsUseSheets.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkScreeningsUseSheets.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkScreeningsUseSheets.Location = new System.Drawing.Point(6, 228);
			this.checkScreeningsUseSheets.Name = "checkScreeningsUseSheets";
			this.checkScreeningsUseSheets.Size = new System.Drawing.Size(435, 17);
			this.checkScreeningsUseSheets.TabIndex = 217;
			this.checkScreeningsUseSheets.Text = "Screenings use Sheets";
			this.checkScreeningsUseSheets.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkScreeningsUseSheets.UseVisualStyleBackColor = true;
			// 
			// butAllergiesIndicateNone
			// 
			this.butAllergiesIndicateNone.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAllergiesIndicateNone.Autosize = true;
			this.butAllergiesIndicateNone.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAllergiesIndicateNone.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAllergiesIndicateNone.CornerRadius = 4F;
			this.butAllergiesIndicateNone.Location = new System.Drawing.Point(420, 77);
			this.butAllergiesIndicateNone.Name = "butAllergiesIndicateNone";
			this.butAllergiesIndicateNone.Size = new System.Drawing.Size(21, 21);
			this.butAllergiesIndicateNone.TabIndex = 205;
			this.butAllergiesIndicateNone.Text = "...";
			this.butAllergiesIndicateNone.Click += new System.EventHandler(this.butAllergiesIndicateNone_Click);
			// 
			// checkMedicalFeeUsedForNewProcs
			// 
			this.checkMedicalFeeUsedForNewProcs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkMedicalFeeUsedForNewProcs.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkMedicalFeeUsedForNewProcs.Location = new System.Drawing.Point(6, 103);
			this.checkMedicalFeeUsedForNewProcs.Name = "checkMedicalFeeUsedForNewProcs";
			this.checkMedicalFeeUsedForNewProcs.Size = new System.Drawing.Size(435, 17);
			this.checkMedicalFeeUsedForNewProcs.TabIndex = 208;
			this.checkMedicalFeeUsedForNewProcs.Text = "Use medical fee for new procedures";
			this.checkMedicalFeeUsedForNewProcs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkMedicalFeeUsedForNewProcs.UseVisualStyleBackColor = true;
			// 
			// checkDxIcdVersion
			// 
			this.checkDxIcdVersion.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkDxIcdVersion.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkDxIcdVersion.Location = new System.Drawing.Point(6, 121);
			this.checkDxIcdVersion.Name = "checkDxIcdVersion";
			this.checkDxIcdVersion.Size = new System.Drawing.Size(435, 17);
			this.checkDxIcdVersion.TabIndex = 212;
			this.checkDxIcdVersion.Text = "Use ICD-10 Diagnosis Codes (uncheck for ICD-9)";
			this.checkDxIcdVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkDxIcdVersion.UseVisualStyleBackColor = true;
			this.checkDxIcdVersion.Click += new System.EventHandler(this.checkDxIcdVersion_Click);
			// 
			// butDiagnosisCode
			// 
			this.butDiagnosisCode.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDiagnosisCode.Autosize = true;
			this.butDiagnosisCode.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDiagnosisCode.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDiagnosisCode.CornerRadius = 4F;
			this.butDiagnosisCode.Location = new System.Drawing.Point(420, 140);
			this.butDiagnosisCode.Name = "butDiagnosisCode";
			this.butDiagnosisCode.Size = new System.Drawing.Size(21, 21);
			this.butDiagnosisCode.TabIndex = 213;
			this.butDiagnosisCode.Text = "...";
			this.butDiagnosisCode.Click += new System.EventHandler(this.butDiagnosisCode_Click);
			// 
			// labelIcdCodeDefault
			// 
			this.labelIcdCodeDefault.Location = new System.Drawing.Point(4, 143);
			this.labelIcdCodeDefault.Name = "labelIcdCodeDefault";
			this.labelIcdCodeDefault.Size = new System.Drawing.Size(326, 15);
			this.labelIcdCodeDefault.TabIndex = 203;
			this.labelIcdCodeDefault.Text = "Default ICD-10 code for new procedures";
			this.labelIcdCodeDefault.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textICD9DefaultForNewProcs
			// 
			this.textICD9DefaultForNewProcs.Location = new System.Drawing.Point(331, 140);
			this.textICD9DefaultForNewProcs.Name = "textICD9DefaultForNewProcs";
			this.textICD9DefaultForNewProcs.Size = new System.Drawing.Size(85, 20);
			this.textICD9DefaultForNewProcs.TabIndex = 209;
			// 
			// checkProcLockingIsAllowed
			// 
			this.checkProcLockingIsAllowed.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcLockingIsAllowed.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkProcLockingIsAllowed.Location = new System.Drawing.Point(6, 166);
			this.checkProcLockingIsAllowed.Name = "checkProcLockingIsAllowed";
			this.checkProcLockingIsAllowed.Size = new System.Drawing.Size(435, 17);
			this.checkProcLockingIsAllowed.TabIndex = 210;
			this.checkProcLockingIsAllowed.Text = "Procedure locking is allowed";
			this.checkProcLockingIsAllowed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcLockingIsAllowed.UseVisualStyleBackColor = true;
			this.checkProcLockingIsAllowed.Click += new System.EventHandler(this.checkProcLockingIsAllowed_Click);
			// 
			// checkChartNonPatientWarn
			// 
			this.checkChartNonPatientWarn.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkChartNonPatientWarn.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkChartNonPatientWarn.Location = new System.Drawing.Point(6, 185);
			this.checkChartNonPatientWarn.Name = "checkChartNonPatientWarn";
			this.checkChartNonPatientWarn.Size = new System.Drawing.Size(435, 17);
			this.checkChartNonPatientWarn.TabIndex = 211;
			this.checkChartNonPatientWarn.Text = "Non Patient Warning";
			this.checkChartNonPatientWarn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkChartNonPatientWarn.UseVisualStyleBackColor = true;
			this.checkChartNonPatientWarn.Click += new System.EventHandler(this.checkChartNonPatientWarn_Click);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(6, 208);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(395, 15);
			this.label11.TabIndex = 213;
			this.label11.Text = "Medication order default days until stop date (0 for no automatic stop date)";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textMedDefaultStopDays
			// 
			this.textMedDefaultStopDays.Location = new System.Drawing.Point(402, 205);
			this.textMedDefaultStopDays.Name = "textMedDefaultStopDays";
			this.textMedDefaultStopDays.Size = new System.Drawing.Size(39, 20);
			this.textMedDefaultStopDays.TabIndex = 212;
			// 
			// tabChartAppearance
			// 
			this.tabChartAppearance.Controls.Add(this.comboToothNomenclature);
			this.tabChartAppearance.Controls.Add(this.checkProcNoteConcurrencyMerge);
			this.tabChartAppearance.Controls.Add(this.labelToothNomenclature);
			this.tabChartAppearance.Controls.Add(this.checkAutoClearEntryStatus);
			this.tabChartAppearance.Controls.Add(this.checkProcGroupNoteDoesAggregate);
			this.tabChartAppearance.Controls.Add(this.checkProvColorChart);
			this.tabChartAppearance.Controls.Add(this.checkPerioSkipMissingTeeth);
			this.tabChartAppearance.Controls.Add(this.checkPerioTreatImplantsAsNotMissing);
			this.tabChartAppearance.Controls.Add(this.comboProcCodeListSort);
			this.tabChartAppearance.Controls.Add(this.label32);
			this.tabChartAppearance.Location = new System.Drawing.Point(4, 22);
			this.tabChartAppearance.Name = "tabChartAppearance";
			this.tabChartAppearance.Padding = new System.Windows.Forms.Padding(3);
			this.tabChartAppearance.Size = new System.Drawing.Size(457, 547);
			this.tabChartAppearance.TabIndex = 1;
			this.tabChartAppearance.Text = "Appearance";
			this.tabChartAppearance.UseVisualStyleBackColor = true;
			// 
			// comboToothNomenclature
			// 
			this.comboToothNomenclature.FormattingEnabled = true;
			this.comboToothNomenclature.Location = new System.Drawing.Point(188, 27);
			this.comboToothNomenclature.Name = "comboToothNomenclature";
			this.comboToothNomenclature.Size = new System.Drawing.Size(255, 21);
			this.comboToothNomenclature.TabIndex = 195;
			// 
			// checkProcNoteConcurrencyMerge
			// 
			this.checkProcNoteConcurrencyMerge.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcNoteConcurrencyMerge.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkProcNoteConcurrencyMerge.Location = new System.Drawing.Point(5, 155);
			this.checkProcNoteConcurrencyMerge.Name = "checkProcNoteConcurrencyMerge";
			this.checkProcNoteConcurrencyMerge.Size = new System.Drawing.Size(436, 15);
			this.checkProcNoteConcurrencyMerge.TabIndex = 229;
			this.checkProcNoteConcurrencyMerge.Text = "Procedure notes merge together when concurrency issues occur";
			this.checkProcNoteConcurrencyMerge.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcNoteConcurrencyMerge.UseVisualStyleBackColor = true;
			// 
			// labelToothNomenclature
			// 
			this.labelToothNomenclature.Location = new System.Drawing.Point(7, 30);
			this.labelToothNomenclature.Name = "labelToothNomenclature";
			this.labelToothNomenclature.Size = new System.Drawing.Size(179, 15);
			this.labelToothNomenclature.TabIndex = 196;
			this.labelToothNomenclature.Text = "Tooth Nomenclature";
			this.labelToothNomenclature.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkAutoClearEntryStatus
			// 
			this.checkAutoClearEntryStatus.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAutoClearEntryStatus.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAutoClearEntryStatus.Location = new System.Drawing.Point(5, 7);
			this.checkAutoClearEntryStatus.Name = "checkAutoClearEntryStatus";
			this.checkAutoClearEntryStatus.Size = new System.Drawing.Size(436, 15);
			this.checkAutoClearEntryStatus.TabIndex = 73;
			this.checkAutoClearEntryStatus.Text = "Reset entry status to TreatPlan when switching patients";
			this.checkAutoClearEntryStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAutoClearEntryStatus.UseVisualStyleBackColor = true;
			// 
			// checkProcGroupNoteDoesAggregate
			// 
			this.checkProcGroupNoteDoesAggregate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcGroupNoteDoesAggregate.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkProcGroupNoteDoesAggregate.Location = new System.Drawing.Point(5, 54);
			this.checkProcGroupNoteDoesAggregate.Name = "checkProcGroupNoteDoesAggregate";
			this.checkProcGroupNoteDoesAggregate.Size = new System.Drawing.Size(436, 15);
			this.checkProcGroupNoteDoesAggregate.TabIndex = 206;
			this.checkProcGroupNoteDoesAggregate.Text = "Procedure Group Note Does Aggregate";
			this.checkProcGroupNoteDoesAggregate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcGroupNoteDoesAggregate.UseVisualStyleBackColor = true;
			// 
			// checkProvColorChart
			// 
			this.checkProvColorChart.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProvColorChart.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkProvColorChart.Location = new System.Drawing.Point(5, 72);
			this.checkProvColorChart.Name = "checkProvColorChart";
			this.checkProvColorChart.Size = new System.Drawing.Size(436, 15);
			this.checkProvColorChart.TabIndex = 214;
			this.checkProvColorChart.Text = "Use Provider Color in Chart";
			this.checkProvColorChart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProvColorChart.UseVisualStyleBackColor = true;
			// 
			// checkPerioSkipMissingTeeth
			// 
			this.checkPerioSkipMissingTeeth.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPerioSkipMissingTeeth.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPerioSkipMissingTeeth.Location = new System.Drawing.Point(5, 90);
			this.checkPerioSkipMissingTeeth.Name = "checkPerioSkipMissingTeeth";
			this.checkPerioSkipMissingTeeth.Size = new System.Drawing.Size(436, 15);
			this.checkPerioSkipMissingTeeth.TabIndex = 215;
			this.checkPerioSkipMissingTeeth.Text = "Perio exams always skip missing teeth";
			this.checkPerioSkipMissingTeeth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPerioSkipMissingTeeth.UseVisualStyleBackColor = true;
			// 
			// checkPerioTreatImplantsAsNotMissing
			// 
			this.checkPerioTreatImplantsAsNotMissing.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPerioTreatImplantsAsNotMissing.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPerioTreatImplantsAsNotMissing.Location = new System.Drawing.Point(5, 108);
			this.checkPerioTreatImplantsAsNotMissing.Name = "checkPerioTreatImplantsAsNotMissing";
			this.checkPerioTreatImplantsAsNotMissing.Size = new System.Drawing.Size(436, 15);
			this.checkPerioTreatImplantsAsNotMissing.TabIndex = 216;
			this.checkPerioTreatImplantsAsNotMissing.Text = "Perio exams treat implants as not missing";
			this.checkPerioTreatImplantsAsNotMissing.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPerioTreatImplantsAsNotMissing.UseVisualStyleBackColor = true;
			// 
			// comboProcCodeListSort
			// 
			this.comboProcCodeListSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProcCodeListSort.FormattingEnabled = true;
			this.comboProcCodeListSort.Location = new System.Drawing.Point(279, 128);
			this.comboProcCodeListSort.MaxDropDownItems = 30;
			this.comboProcCodeListSort.Name = "comboProcCodeListSort";
			this.comboProcCodeListSort.Size = new System.Drawing.Size(164, 21);
			this.comboProcCodeListSort.TabIndex = 219;
			// 
			// label32
			// 
			this.label32.Location = new System.Drawing.Point(7, 131);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(270, 15);
			this.label32.TabIndex = 220;
			this.label32.Text = "Procedure Code List Sort";
			this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabImages
			// 
			this.tabImages.BackColor = System.Drawing.SystemColors.Window;
			this.tabImages.Controls.Add(this.groupBox3);
			this.tabImages.Location = new System.Drawing.Point(4, 22);
			this.tabImages.Name = "tabImages";
			this.tabImages.Padding = new System.Windows.Forms.Padding(3);
			this.tabImages.Size = new System.Drawing.Size(471, 579);
			this.tabImages.TabIndex = 5;
			this.tabImages.Text = "Images";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.radioImagesModuleTreeIsPersistentPerUser);
			this.groupBox3.Controls.Add(this.radioImagesModuleTreeIsCollapsed);
			this.groupBox3.Controls.Add(this.radioImagesModuleTreeIsExpanded);
			this.groupBox3.Location = new System.Drawing.Point(3, 6);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(460, 76);
			this.groupBox3.TabIndex = 51;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Folder Expansion Preference";
			// 
			// radioImagesModuleTreeIsPersistentPerUser
			// 
			this.radioImagesModuleTreeIsPersistentPerUser.Location = new System.Drawing.Point(117, 51);
			this.radioImagesModuleTreeIsPersistentPerUser.Name = "radioImagesModuleTreeIsPersistentPerUser";
			this.radioImagesModuleTreeIsPersistentPerUser.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.radioImagesModuleTreeIsPersistentPerUser.Size = new System.Drawing.Size(337, 17);
			this.radioImagesModuleTreeIsPersistentPerUser.TabIndex = 54;
			this.radioImagesModuleTreeIsPersistentPerUser.TabStop = true;
			this.radioImagesModuleTreeIsPersistentPerUser.Text = "Document tree folders persistent expand/collapse per user";
			this.radioImagesModuleTreeIsPersistentPerUser.UseVisualStyleBackColor = true;
			// 
			// radioImagesModuleTreeIsCollapsed
			// 
			this.radioImagesModuleTreeIsCollapsed.Location = new System.Drawing.Point(117, 34);
			this.radioImagesModuleTreeIsCollapsed.Name = "radioImagesModuleTreeIsCollapsed";
			this.radioImagesModuleTreeIsCollapsed.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.radioImagesModuleTreeIsCollapsed.Size = new System.Drawing.Size(337, 17);
			this.radioImagesModuleTreeIsCollapsed.TabIndex = 53;
			this.radioImagesModuleTreeIsCollapsed.TabStop = true;
			this.radioImagesModuleTreeIsCollapsed.Text = "Document tree collapses when patient changes";
			this.radioImagesModuleTreeIsCollapsed.UseVisualStyleBackColor = true;
			// 
			// radioImagesModuleTreeIsExpanded
			// 
			this.radioImagesModuleTreeIsExpanded.Location = new System.Drawing.Point(72, 17);
			this.radioImagesModuleTreeIsExpanded.Name = "radioImagesModuleTreeIsExpanded";
			this.radioImagesModuleTreeIsExpanded.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.radioImagesModuleTreeIsExpanded.Size = new System.Drawing.Size(382, 17);
			this.radioImagesModuleTreeIsExpanded.TabIndex = 0;
			this.radioImagesModuleTreeIsExpanded.TabStop = true;
			this.radioImagesModuleTreeIsExpanded.Text = "Expand the document tree each time the Images module is visited";
			this.radioImagesModuleTreeIsExpanded.UseVisualStyleBackColor = true;
			// 
			// tabManage
			// 
			this.tabManage.BackColor = System.Drawing.SystemColors.Window;
			this.tabManage.Controls.Add(this.textClaimsReceivedDays);
			this.tabManage.Controls.Add(this.checkShowAutoDeposit);
			this.tabManage.Controls.Add(this.checkEraOneClaimPerPage);
			this.tabManage.Controls.Add(this.checkClaimPaymentBatchOnly);
			this.tabManage.Controls.Add(this.labelClaimsReceivedDays);
			this.tabManage.Controls.Add(this.checkScheduleProvEmpSelectAll);
			this.tabManage.Controls.Add(this.checkClaimsSendWindowValidateOnLoad);
			this.tabManage.Controls.Add(this.checkTimeCardADP);
			this.tabManage.Controls.Add(this.groupBox1);
			this.tabManage.Controls.Add(this.comboTimeCardOvertimeFirstDayOfWeek);
			this.tabManage.Controls.Add(this.label16);
			this.tabManage.Controls.Add(this.checkRxSendNewToQueue);
			this.tabManage.Location = new System.Drawing.Point(4, 22);
			this.tabManage.Name = "tabManage";
			this.tabManage.Padding = new System.Windows.Forms.Padding(3);
			this.tabManage.Size = new System.Drawing.Size(471, 579);
			this.tabManage.TabIndex = 6;
			this.tabManage.Text = "Manage";
			// 
			// textClaimsReceivedDays
			// 
			this.textClaimsReceivedDays.Location = new System.Drawing.Point(380, 372);
			this.textClaimsReceivedDays.MaxVal = 999999;
			this.textClaimsReceivedDays.MinVal = 1;
			this.textClaimsReceivedDays.Name = "textClaimsReceivedDays";
			this.textClaimsReceivedDays.Size = new System.Drawing.Size(60, 20);
			this.textClaimsReceivedDays.TabIndex = 248;
			this.textClaimsReceivedDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// checkShowAutoDeposit
			// 
			this.checkShowAutoDeposit.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowAutoDeposit.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowAutoDeposit.Location = new System.Drawing.Point(18, 430);
			this.checkShowAutoDeposit.Name = "checkShowAutoDeposit";
			this.checkShowAutoDeposit.Size = new System.Drawing.Size(422, 17);
			this.checkShowAutoDeposit.TabIndex = 246;
			this.checkShowAutoDeposit.Text = "Insurance Payments: Show Auto Deposit";
			this.checkShowAutoDeposit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkEraOneClaimPerPage
			// 
			this.checkEraOneClaimPerPage.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEraOneClaimPerPage.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkEraOneClaimPerPage.Location = new System.Drawing.Point(19, 412);
			this.checkEraOneClaimPerPage.Name = "checkEraOneClaimPerPage";
			this.checkEraOneClaimPerPage.Size = new System.Drawing.Size(421, 17);
			this.checkEraOneClaimPerPage.TabIndex = 206;
			this.checkEraOneClaimPerPage.Text = "ERAs print one page per claim";
			this.checkEraOneClaimPerPage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkClaimPaymentBatchOnly
			// 
			this.checkClaimPaymentBatchOnly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClaimPaymentBatchOnly.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClaimPaymentBatchOnly.Location = new System.Drawing.Point(19, 395);
			this.checkClaimPaymentBatchOnly.Name = "checkClaimPaymentBatchOnly";
			this.checkClaimPaymentBatchOnly.Size = new System.Drawing.Size(421, 17);
			this.checkClaimPaymentBatchOnly.TabIndex = 205;
			this.checkClaimPaymentBatchOnly.Text = "Finalize Claim Payments in Batch Insurance window only";
			this.checkClaimPaymentBatchOnly.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelClaimsReceivedDays
			// 
			this.labelClaimsReceivedDays.Location = new System.Drawing.Point(18, 372);
			this.labelClaimsReceivedDays.MaximumSize = new System.Drawing.Size(1000, 300);
			this.labelClaimsReceivedDays.Name = "labelClaimsReceivedDays";
			this.labelClaimsReceivedDays.Size = new System.Drawing.Size(361, 20);
			this.labelClaimsReceivedDays.TabIndex = 203;
			this.labelClaimsReceivedDays.Text = "Show claims received after days (blank to disable)";
			this.labelClaimsReceivedDays.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkScheduleProvEmpSelectAll
			// 
			this.checkScheduleProvEmpSelectAll.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkScheduleProvEmpSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkScheduleProvEmpSelectAll.Location = new System.Drawing.Point(20, 91);
			this.checkScheduleProvEmpSelectAll.Name = "checkScheduleProvEmpSelectAll";
			this.checkScheduleProvEmpSelectAll.Size = new System.Drawing.Size(421, 17);
			this.checkScheduleProvEmpSelectAll.TabIndex = 202;
			this.checkScheduleProvEmpSelectAll.Text = "Select all provider/employees when loading schedules";
			this.checkScheduleProvEmpSelectAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkClaimsSendWindowValidateOnLoad
			// 
			this.checkClaimsSendWindowValidateOnLoad.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkClaimsSendWindowValidateOnLoad.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkClaimsSendWindowValidateOnLoad.Location = new System.Drawing.Point(20, 74);
			this.checkClaimsSendWindowValidateOnLoad.Name = "checkClaimsSendWindowValidateOnLoad";
			this.checkClaimsSendWindowValidateOnLoad.Size = new System.Drawing.Size(421, 17);
			this.checkClaimsSendWindowValidateOnLoad.TabIndex = 199;
			this.checkClaimsSendWindowValidateOnLoad.Text = "Claims Send window validate on load (can cause slowness)";
			this.checkClaimsSendWindowValidateOnLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkTimeCardADP
			// 
			this.checkTimeCardADP.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTimeCardADP.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkTimeCardADP.Location = new System.Drawing.Point(82, 57);
			this.checkTimeCardADP.Name = "checkTimeCardADP";
			this.checkTimeCardADP.Size = new System.Drawing.Size(359, 17);
			this.checkTimeCardADP.TabIndex = 198;
			this.checkTimeCardADP.Text = "ADP export includes employee name";
			this.checkTimeCardADP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.checkBillingShowProgress);
			this.groupBox1.Controls.Add(this.label24);
			this.groupBox1.Controls.Add(this.textBillingElectBatchMax);
			this.groupBox1.Controls.Add(this.checkStatementShowAdjNotes);
			this.groupBox1.Controls.Add(this.checkIntermingleDefault);
			this.groupBox1.Controls.Add(this.checkStatementShowReturnAddress);
			this.groupBox1.Controls.Add(this.checkStatementShowProcBreakdown);
			this.groupBox1.Controls.Add(this.checkShowCC);
			this.groupBox1.Controls.Add(this.checkStatementShowNotes);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.comboUseChartNum);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label18);
			this.groupBox1.Controls.Add(this.textStatementsCalcDueDate);
			this.groupBox1.Controls.Add(this.textPayPlansBillInAdvanceDays);
			this.groupBox1.Location = new System.Drawing.Point(38, 108);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(413, 259);
			this.groupBox1.TabIndex = 197;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Billing and Statements";
			// 
			// checkBillingShowProgress
			// 
			this.checkBillingShowProgress.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBillingShowProgress.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBillingShowProgress.Location = new System.Drawing.Point(25, 237);
			this.checkBillingShowProgress.Name = "checkBillingShowProgress";
			this.checkBillingShowProgress.Size = new System.Drawing.Size(377, 16);
			this.checkBillingShowProgress.TabIndex = 218;
			this.checkBillingShowProgress.Text = "Show progress when sending statements";
			this.checkBillingShowProgress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label24
			// 
			this.label24.Location = new System.Drawing.Point(25, 210);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(316, 20);
			this.label24.TabIndex = 217;
			this.label24.Text = "Max number of statements per batch (0 for no limit)";
			this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textBillingElectBatchMax
			// 
			this.textBillingElectBatchMax.Location = new System.Drawing.Point(342, 211);
			this.textBillingElectBatchMax.MaxVal = 255;
			this.textBillingElectBatchMax.MinVal = 0;
			this.textBillingElectBatchMax.Name = "textBillingElectBatchMax";
			this.textBillingElectBatchMax.Size = new System.Drawing.Size(60, 20);
			this.textBillingElectBatchMax.TabIndex = 216;
			this.textBillingElectBatchMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// checkStatementShowAdjNotes
			// 
			this.checkStatementShowAdjNotes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkStatementShowAdjNotes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkStatementShowAdjNotes.Location = new System.Drawing.Point(34, 62);
			this.checkStatementShowAdjNotes.Name = "checkStatementShowAdjNotes";
			this.checkStatementShowAdjNotes.Size = new System.Drawing.Size(368, 17);
			this.checkStatementShowAdjNotes.TabIndex = 215;
			this.checkStatementShowAdjNotes.Text = "Show notes for adjustments";
			this.checkStatementShowAdjNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkIntermingleDefault
			// 
			this.checkIntermingleDefault.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIntermingleDefault.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIntermingleDefault.Location = new System.Drawing.Point(25, 189);
			this.checkIntermingleDefault.Name = "checkIntermingleDefault";
			this.checkIntermingleDefault.Size = new System.Drawing.Size(377, 16);
			this.checkIntermingleDefault.TabIndex = 214;
			this.checkIntermingleDefault.Text = "Account module statements default to intermingled mode";
			this.checkIntermingleDefault.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkStatementShowReturnAddress
			// 
			this.checkStatementShowReturnAddress.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkStatementShowReturnAddress.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkStatementShowReturnAddress.Location = new System.Drawing.Point(125, 11);
			this.checkStatementShowReturnAddress.Name = "checkStatementShowReturnAddress";
			this.checkStatementShowReturnAddress.Size = new System.Drawing.Size(277, 17);
			this.checkStatementShowReturnAddress.TabIndex = 206;
			this.checkStatementShowReturnAddress.Text = "Show return address";
			this.checkStatementShowReturnAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkStatementShowProcBreakdown
			// 
			this.checkStatementShowProcBreakdown.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkStatementShowProcBreakdown.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkStatementShowProcBreakdown.Location = new System.Drawing.Point(34, 79);
			this.checkStatementShowProcBreakdown.Name = "checkStatementShowProcBreakdown";
			this.checkStatementShowProcBreakdown.Size = new System.Drawing.Size(368, 17);
			this.checkStatementShowProcBreakdown.TabIndex = 212;
			this.checkStatementShowProcBreakdown.Text = "Show procedure breakdown";
			this.checkStatementShowProcBreakdown.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkShowCC
			// 
			this.checkShowCC.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowCC.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowCC.Location = new System.Drawing.Point(34, 28);
			this.checkShowCC.Name = "checkShowCC";
			this.checkShowCC.Size = new System.Drawing.Size(368, 17);
			this.checkShowCC.TabIndex = 203;
			this.checkShowCC.Text = "Show credit card info";
			this.checkShowCC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkStatementShowNotes
			// 
			this.checkStatementShowNotes.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkStatementShowNotes.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkStatementShowNotes.Location = new System.Drawing.Point(34, 45);
			this.checkStatementShowNotes.Name = "checkStatementShowNotes";
			this.checkStatementShowNotes.Size = new System.Drawing.Size(368, 17);
			this.checkStatementShowNotes.TabIndex = 211;
			this.checkStatementShowNotes.Text = "Show notes for payments";
			this.checkStatementShowNotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(22, 126);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(318, 27);
			this.label2.TabIndex = 204;
			this.label2.Text = "Days to calculate due date.  Usually 10 or 15.  Leave blank to show \"Due on Recei" +
    "pt\"";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboUseChartNum
			// 
			this.comboUseChartNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboUseChartNum.FormattingEnabled = true;
			this.comboUseChartNum.Location = new System.Drawing.Point(273, 99);
			this.comboUseChartNum.Name = "comboUseChartNum";
			this.comboUseChartNum.Size = new System.Drawing.Size(130, 21);
			this.comboUseChartNum.TabIndex = 207;
			// 
			// label10
			// 
			this.label10.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label10.Location = new System.Drawing.Point(76, 102);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(195, 15);
			this.label10.TabIndex = 208;
			this.label10.Text = "Account Numbers use";
			this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label18
			// 
			this.label18.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label18.Location = new System.Drawing.Point(23, 158);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(318, 27);
			this.label18.TabIndex = 209;
			this.label18.Text = "Days in advance to bill payment plan amounts due.\r\nUsually 10 or 15.";
			this.label18.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textStatementsCalcDueDate
			// 
			this.textStatementsCalcDueDate.Location = new System.Drawing.Point(343, 130);
			this.textStatementsCalcDueDate.MaxVal = 255;
			this.textStatementsCalcDueDate.MinVal = 0;
			this.textStatementsCalcDueDate.Name = "textStatementsCalcDueDate";
			this.textStatementsCalcDueDate.Size = new System.Drawing.Size(60, 20);
			this.textStatementsCalcDueDate.TabIndex = 205;
			this.textStatementsCalcDueDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textPayPlansBillInAdvanceDays
			// 
			this.textPayPlansBillInAdvanceDays.Location = new System.Drawing.Point(343, 162);
			this.textPayPlansBillInAdvanceDays.MaxVal = 255;
			this.textPayPlansBillInAdvanceDays.MinVal = 0;
			this.textPayPlansBillInAdvanceDays.Name = "textPayPlansBillInAdvanceDays";
			this.textPayPlansBillInAdvanceDays.Size = new System.Drawing.Size(60, 20);
			this.textPayPlansBillInAdvanceDays.TabIndex = 210;
			this.textPayPlansBillInAdvanceDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// comboTimeCardOvertimeFirstDayOfWeek
			// 
			this.comboTimeCardOvertimeFirstDayOfWeek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTimeCardOvertimeFirstDayOfWeek.FormattingEnabled = true;
			this.comboTimeCardOvertimeFirstDayOfWeek.Location = new System.Drawing.Point(270, 30);
			this.comboTimeCardOvertimeFirstDayOfWeek.Name = "comboTimeCardOvertimeFirstDayOfWeek";
			this.comboTimeCardOvertimeFirstDayOfWeek.Size = new System.Drawing.Size(170, 21);
			this.comboTimeCardOvertimeFirstDayOfWeek.TabIndex = 195;
			// 
			// label16
			// 
			this.label16.BackColor = System.Drawing.SystemColors.Window;
			this.label16.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label16.Location = new System.Drawing.Point(17, 34);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(248, 13);
			this.label16.TabIndex = 196;
			this.label16.Text = "Time Card first day of week for overtime";
			this.label16.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkRxSendNewToQueue
			// 
			this.checkRxSendNewToQueue.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkRxSendNewToQueue.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkRxSendNewToQueue.Location = new System.Drawing.Point(81, 7);
			this.checkRxSendNewToQueue.Name = "checkRxSendNewToQueue";
			this.checkRxSendNewToQueue.Size = new System.Drawing.Size(359, 17);
			this.checkRxSendNewToQueue.TabIndex = 47;
			this.checkRxSendNewToQueue.Text = "Send all new prescriptions to electronic queue";
			this.checkRxSendNewToQueue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
			this.butCancel.Location = new System.Drawing.Point(434, 621);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 8;
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
			this.butOK.Location = new System.Drawing.Point(353, 621);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 7;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// FormModuleSetup
			// 
			this.ClientSize = new System.Drawing.Size(538, 659);
			this.Controls.Add(this.tabControlMain);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(554, 697);
			this.Name = "FormModuleSetup";
			this.ShowInTaskbar = false;
			this.Text = "Module Preferences";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormModuleSetup_FormClosing);
			this.Load += new System.EventHandler(this.FormModuleSetup_Load);
			this.tabControlMain.ResumeLayout(false);
			this.tabAppts.ResumeLayout(false);
			this.tabControlAppts.ResumeLayout(false);
			this.tabApptsPageBehavior.ResumeLayout(false);
			this.tabApptsPageBehavior.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.tabApptsPageAppearance.ResumeLayout(false);
			this.tabFamily.ResumeLayout(false);
			this.tabControlFamily.ResumeLayout(false);
			this.tabPageFamGen.ResumeLayout(false);
			this.tabPageSuperFam.ResumeLayout(false);
			this.tabPageClaimSnapShot.ResumeLayout(false);
			this.tabPageClaimSnapShot.PerformLayout();
			this.tabAccount.ResumeLayout(false);
			this.tabControlAccount.ResumeLayout(false);
			this.tabPagePayAdj.ResumeLayout(false);
			this.tabPagePayAdj.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.tabPageIns.ResumeLayout(false);
			this.tabPageIns.PerformLayout();
			this.groupBoxClaimIdPrefix.ResumeLayout(false);
			this.groupBoxClaimIdPrefix.PerformLayout();
			this.tabPageMisc.ResumeLayout(false);
			this.groupCommLogs.ResumeLayout(false);
			this.groupPayPlans.ResumeLayout(false);
			this.tabTreatPlan.ResumeLayout(false);
			this.tabTreatPlan.PerformLayout();
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.groupTreatPlanSort.ResumeLayout(false);
			this.tabChart.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabChartBehavior.ResumeLayout(false);
			this.tabChartBehavior.PerformLayout();
			this.tabChartAppearance.ResumeLayout(false);
			this.tabImages.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.tabManage.ResumeLayout(false);
			this.tabManage.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormModuleSetup_Load(object sender, System.EventArgs e) {
			try {//try/catch used to prevent setup form from partially loading and filling controls.  Causes UEs, Example: TimeCardOvertimeFirstDayOfWeek set to -1 because UI control not filled properly.
				FillControlsHelper();
			}
			catch(Exception ex) {
				FriendlyException.Show(Lan.g(this,"An error has occured while attempting to load preferences.  Run database maintenance and try again."),ex);
				DialogResult=DialogResult.Abort;
				return;
			}
			//Now that all the tabs are filled, use _selectedTab to open a specific tab that the user is trying to view.
			tabControlMain.SelectedTab=tabControlMain.TabPages[_selectedTab];//Garunteed to be a valid tab.  Validated in constructor.
			if(PrefC.RandomKeys) {
				groupTreatPlanSort.Visible=false;
			}
			Plugins.HookAddCode(this,"FormModuleSetup.FormModuleSetup_Load_end");
		}

		private void FillControlsHelper() {
			_changed=false;
			#region Appointment Module
			//Appointment module---------------------------------------------------------------
			BrokenApptProcedure brokenApptCodeDB=(BrokenApptProcedure)PrefC.GetInt(PrefName.BrokenApptProcedure);
			foreach(BrokenApptProcedure option in Enum.GetValues(typeof(BrokenApptProcedure))) {
				if(option==BrokenApptProcedure.Missed && !ProcedureCodes.HasMissedCode()) {
					continue;
				}
				if(option==BrokenApptProcedure.Cancelled && !ProcedureCodes.HasCancelledCode()) {
					continue;
				}
				if(option==BrokenApptProcedure.Both && (!ProcedureCodes.HasMissedCode() || !ProcedureCodes.HasCancelledCode())) {
					continue;
				}
				_listComboBrokenApptProcOptions.Add(option);
				int index=comboBrokenApptProc.Items.Add(Lans.g(this,option.ToString()));
				if(option==brokenApptCodeDB) {
					comboBrokenApptProc.SelectedIndex=index;
				}
			}
			if(comboBrokenApptProc.Items.Count==1) {//None
				comboBrokenApptProc.SelectedIndex=0;
				comboBrokenApptProc.Enabled=false;
			}
			checkSolidBlockouts.Checked=PrefC.GetBool(PrefName.SolidBlockouts);
			checkBrokenApptAdjustment.Checked=PrefC.GetBool(PrefName.BrokenApptAdjustment);
			checkBrokenApptCommLog.Checked=PrefC.GetBool(PrefName.BrokenApptCommLog);
			//checkBrokenApptNote.Checked=PrefC.GetBool(PrefName.BrokenApptCommLogNotAdjustment);
			checkApptBubbleDelay.Checked = PrefC.GetBool(PrefName.ApptBubbleDelay);
			checkAppointmentBubblesDisabled.Checked=PrefC.GetBool(PrefName.AppointmentBubblesDisabled);
			listPosAdjTypes=Defs.GetPositiveAdjTypes();
			listNegAdjTypes=Defs.GetNegativeAdjTypes();
			long financeChargeAdjDefNum=PrefC.GetLong(PrefName.FinanceChargeAdjustmentType);
			long billingChargeAdjDefNum=PrefC.GetLong(PrefName.BillingChargeAdjustmentType);
			long brokenApptAdjDefNum=PrefC.GetLong(PrefName.BrokenAppointmentAdjustmentType);
			long payPlanAdjDefNum=PrefC.GetLong(PrefName.PayPlanAdjType);
			long salesTaxAdjDefNum=PrefC.GetLong(PrefName.SalesTaxAdjustmentType);
			for(int i=0;i<listPosAdjTypes.Count;i++) {
				comboBrokenApptAdjType.Items.Add(listPosAdjTypes[i].ItemName);
			}
			comboBrokenApptAdjType.IndexSelectOrSetText(listPosAdjTypes.FindIndex(x => x.DefNum==brokenApptAdjDefNum),
				() => { return brokenApptAdjDefNum==0 ? "" : Defs.GetName(DefCat.AdjTypes,brokenApptAdjDefNum)+" ("+Lan.g(this,"hidden")+")"; });
			long treatPlanDiscountAdjDefNum=PrefC.GetLong(PrefName.TreatPlanDiscountAdjustmentType);
			for(int i=0;i<listNegAdjTypes.Count;i++) {
				comboProcDiscountType.Items.Add(listNegAdjTypes[i].ItemName);
			}
			comboProcDiscountType.IndexSelectOrSetText(listNegAdjTypes.FindIndex(x => x.DefNum==treatPlanDiscountAdjDefNum),
				() => { return treatPlanDiscountAdjDefNum==0 ? "" : Defs.GetName(DefCat.AdjTypes,treatPlanDiscountAdjDefNum)+" ("+Lan.g(this,"hidden")+")"; });
			textDiscountPercentage.Text=PrefC.GetDouble(PrefName.TreatPlanDiscountPercent).ToString();
			checkApptExclamation.Checked=PrefC.GetBool(PrefName.ApptExclamationShowForUnsentIns);
			long timeArrivedPrefNum=PrefC.GetLong(PrefName.AppointmentTimeArrivedTrigger);
			comboTimeArrived.Items.Add(Lan.g(this,"None"));
			comboTimeArrived.SelectedIndex=0;
			long timeSeatedPrefNum=PrefC.GetLong(PrefName.AppointmentTimeSeatedTrigger);
			comboTimeSeated.Items.Add(Lan.g(this,"None"));
			comboTimeSeated.SelectedIndex=0;
			long timeDismissedDefNum=PrefC.GetLong(PrefName.AppointmentTimeDismissedTrigger);
			comboTimeDismissed.Items.Add(Lan.g(this,"None"));
			comboTimeDismissed.SelectedIndex=0;
			_listApptConfirmedDefs=Defs.GetDefsForCategory(DefCat.ApptConfirmed,true);
			for(int i=0;i<_listApptConfirmedDefs.Count;i++) {
				comboTimeArrived.Items.Add(_listApptConfirmedDefs[i].ItemName);
				comboTimeSeated.Items.Add(_listApptConfirmedDefs[i].ItemName);
				comboTimeDismissed.Items.Add(_listApptConfirmedDefs[i].ItemName);
			}
			comboTimeArrived.IndexSelectOrSetText(_listApptConfirmedDefs.FindIndex(x => x.DefNum==timeArrivedPrefNum)+1,
				() => { return timeArrivedPrefNum==0 ? "" : Defs.GetName(DefCat.ApptConfirmed,timeArrivedPrefNum)+" ("+Lan.g(this,"hidden")+")"; });
			comboTimeSeated.IndexSelectOrSetText(_listApptConfirmedDefs.FindIndex(x => x.DefNum==timeSeatedPrefNum)+1,
				() => { return timeSeatedPrefNum==0 ? "" : Defs.GetName(DefCat.ApptConfirmed,timeSeatedPrefNum)+" ("+Lan.g(this,"hidden")+")"; });
			comboTimeDismissed.IndexSelectOrSetText(_listApptConfirmedDefs.FindIndex(x => x.DefNum==timeDismissedDefNum)+1,
				() => { return timeDismissedDefNum==0 ? "" : Defs.GetName(DefCat.ApptConfirmed,timeDismissedDefNum)+" ("+Lan.g(this,"hidden")+")"; });
			checkApptRefreshEveryMinute.Checked=PrefC.GetBool(PrefName.ApptModuleRefreshesEveryMinute);
			for(int i=0;i<Enum.GetNames(typeof(SearchBehaviorCriteria)).Length;i++) {
				comboSearchBehavior.Items.Add(Enum.GetNames(typeof(SearchBehaviorCriteria))[i]);
			}
			ODBoxItem<double> comboItem;
			for(int i=0;i<11;i++) {
				double seconds=(double)i/10;
				if(i==0) {
					comboItem = new ODBoxItem<double>(Lan.g(this,"No delay"),seconds);
				}
				else {
					comboItem = new ODBoxItem<double>(seconds.ToString("f1") + " "+Lan.g(this,"seconds"),seconds);
				}
				comboDelay.Items.Add(comboItem);
				if(PrefC.GetDouble(PrefName.FormClickDelay)==seconds) {
					comboDelay.SelectedIndex = i;
				}
			}
			comboSearchBehavior.SelectedIndex=PrefC.GetInt(PrefName.AppointmentSearchBehavior);
			checkAppointmentTimeIsLocked.Checked=PrefC.GetBool(PrefName.AppointmentTimeIsLocked);
			textApptBubNoteLength.Text=PrefC.GetInt(PrefName.AppointmentBubblesNoteLength).ToString();
			checkWaitingRoomFilterByView.Checked=PrefC.GetBool(PrefName.WaitingRoomFilterByView);
			textWaitRoomWarn.Text=PrefC.GetInt(PrefName.WaitingRoomAlertTime).ToString();
			butColor.BackColor=PrefC.GetColor(PrefName.WaitingRoomAlertColor);
			butApptLineColor.BackColor=PrefC.GetColor(PrefName.AppointmentTimeLineColor);
			checkApptModuleDefaultToWeek.Checked=PrefC.GetBool(PrefName.ApptModuleDefaultToWeek);
			checkApptTimeReset.Checked=PrefC.GetBool(PrefName.AppointmentClinicTimeReset);
			checkApptModuleAdjInProd.Checked=PrefC.GetBool(PrefName.ApptModuleAdjustmentsInProd);
			checkUseOpHygProv.Checked=PrefC.GetBool(PrefName.ApptSecondaryProviderConsiderOpOnly);
			checkApptModuleProductionUsesOps.Checked=PrefC.GetBool(PrefName.ApptModuleProductionUsesOps);
			checkApptsRequireProcs.Checked=PrefC.GetBool(PrefName.ApptsRequireProc);
			checkApptAllowFutureComplete.Checked=PrefC.GetBool(PrefName.ApptAllowFutureComplete);
			checkApptAllowEmptyComplete.Checked=PrefC.GetBool(PrefName.ApptAllowEmptyComplete);
			textApptWithoutProcsDefaultLength.Text=PrefC.GetString(PrefName.AppointmentWithoutProcsDefaultLength);
			#endregion
			#region Family Module
			//Family module-----------------------------------------------------------------------
			checkInsurancePlansShared.Checked=PrefC.GetBool(PrefName.InsurancePlansShared);
			checkPPOpercentage.Checked=PrefC.GetBool(PrefName.InsDefaultPPOpercent);
			checkAllowedFeeSchedsAutomate.Checked=PrefC.GetBool(PrefName.AllowedFeeSchedsAutomate);
			checkCoPayFeeScheduleBlankLikeZero.Checked=PrefC.GetBool(PrefName.CoPay_FeeSchedule_BlankLikeZero);
			checkInsDefaultShowUCRonClaims.Checked=PrefC.GetBool(PrefName.InsDefaultShowUCRonClaims);
			checkInsDefaultAssignmentOfBenefits.Checked=PrefC.GetBool(PrefName.InsDefaultAssignBen);
			checkInsPPOsecWriteoffs.Checked=PrefC.GetBool(PrefName.InsPPOsecWriteoffs);
			for(int i=0;i<Enum.GetNames(typeof(EnumCobRule)).Length;i++) {
				comboCobRule.Items.Add(Lan.g("enumEnumCobRule",Enum.GetNames(typeof(EnumCobRule))[i]));
			}
			comboCobRule.SelectedIndex=PrefC.GetInt(PrefName.InsDefaultCobRule);
			checkTextMsgOkStatusTreatAsNo.Checked=PrefC.GetBool(PrefName.TextMsgOkStatusTreatAsNo);
			checkFamPhiAccess.Checked=PrefC.GetBool(PrefName.FamPhiAccess);
			checkGoogleAddress.Checked=PrefC.GetBool(PrefName.ShowFeatureGoogleMaps);
			checkSelectProv.Checked=PrefC.GetBool(PrefName.PriProvDefaultToSelectProv);
			if(!PrefC.GetBool(PrefName.ShowFeatureSuperfamilies)) {
				tabControlFamily.TabPages.Remove(tabPageSuperFam);
			}
			else {
				foreach(string option in Enum.GetNames(typeof(SortStrategy))) {
					comboSuperFamSort.Items.Add(option);
				}
				comboSuperFamSort.SelectedIndex=PrefC.GetInt(PrefName.SuperFamSortStrategy);
				checkSuperFamSync.Checked=PrefC.GetBool(PrefName.PatientAllSuperFamilySync);
				checkSuperFamAddIns.Checked=PrefC.GetBool(PrefName.SuperFamNewPatAddIns);
				checkSuperFamCloneCreate.Checked=PrefC.GetBool(PrefName.CloneCreateSuperFamily);
			}
			//users should only see the claimsnapshot tab page if they have it set to something other than ClaimCreate.
			//if a user wants to be able to change claimsnapshot settings, the following MySQL statement should be run:
			//UPDATE preference SET ValueString = 'Service'	 WHERE PrefName = 'ClaimSnapshotTriggerType'
			if(PIn.Enum<ClaimSnapshotTrigger>(PrefC.GetString(PrefName.ClaimSnapshotTriggerType),true) == ClaimSnapshotTrigger.ClaimCreate) {
				tabControlFamily.TabPages.Remove(tabPageClaimSnapShot);
			}
			foreach(ClaimSnapshotTrigger trigger in Enum.GetValues(typeof(ClaimSnapshotTrigger))) {
				comboClaimSnapshotTrigger.Items.Add(trigger.GetDescription());
			}
			comboClaimSnapshotTrigger.SelectedIndex=(int)PIn.Enum<ClaimSnapshotTrigger>(PrefC.GetString(PrefName.ClaimSnapshotTriggerType),true);
			textClaimSnapshotRunTime.Text=PrefC.GetDateT(PrefName.ClaimSnapshotRunTime).ToShortTimeString();
			checkClaimUseOverrideProcDescript.Checked=PrefC.GetBool(PrefName.ClaimPrintProcChartedDesc);
			checkClaimTrackingRequireError.Checked=PrefC.GetBool(PrefName.ClaimTrackingRequiresError);
			checkPatInitBillingTypeFromPriInsPlan.Checked=PrefC.GetBool(PrefName.PatInitBillingTypeFromPriInsPlan);
			checkPreferredReferrals.Checked=PrefC.GetBool(PrefName.ShowPreferedReferrals);
			checkAutoFillPatEmail.Checked=PrefC.GetBool(PrefName.AddFamilyInheritsEmail);
			if(!PrefC.HasClinicsEnabled) {
				checkAllowPatsAtHQ.Visible=false;
			}
			checkAllowPatsAtHQ.Checked=PrefC.GetBool(PrefName.ClinicAllowPatientsAtHeadquarters);
			#endregion
			#region Account Module
			#region Pay/Adj Tab
			checkStoreCCTokens.Checked=PrefC.GetBool(PrefName.StoreCCtokens);
			foreach(PayClinicSetting prompt in Enum.GetValues(typeof(PayClinicSetting))) {
				comboPaymentClinicSetting.Items.Add(Lan.g(this,prompt.GetDescription()));
			}
			comboPaymentClinicSetting.SelectedIndex=PrefC.GetInt(PrefName.PaymentClinicSetting);
			checkPaymentsPromptForPayType.Checked=PrefC.GetBool(PrefName.PaymentsPromptForPayType);
			_listPaySplitUnearnedType=Defs.GetDefsForCategory(DefCat.PaySplitUnearnedType,true);
			long defNum=PrefC.GetLong(PrefName.PrepaymentUnearnedType);
			for(int i=0;i<_listPaySplitUnearnedType.Count;i++) {
				comboUnallocatedSplits.Items.Add(_listPaySplitUnearnedType[i].ItemName);//fill combo
			}
			comboUnallocatedSplits.IndexSelectOrSetText(_listPaySplitUnearnedType.FindIndex(x => x.DefNum==defNum),
				() => { return defNum==0 ? "" : Defs.GetName(DefCat.PaySplitUnearnedType,defNum)+" ("+Lan.g(this,"hidden")+")"; });
			for(int i=0;i<listPosAdjTypes.Count;i++) {
				comboFinanceChargeAdjType.Items.Add(listPosAdjTypes[i].ItemName);
				comboBillingChargeAdjType.Items.Add(listPosAdjTypes[i].ItemName);
				comboSalesTaxAdjType.Items.Add(listPosAdjTypes[i].ItemName);
			}
			for(int i=0;i<listNegAdjTypes.Count;i++) {
				comboPayPlanAdj.Items.Add(listNegAdjTypes[i].ItemName);
			}
			comboPayPlanAdj.IndexSelectOrSetText(listNegAdjTypes.FindIndex(x => x.DefNum==payPlanAdjDefNum),
				() => {return payPlanAdjDefNum==0 ? "" : Defs.GetName(DefCat.AdjTypes,payPlanAdjDefNum)+ " ("+Lan.g(this,"hidden")+")"; });
			comboFinanceChargeAdjType.IndexSelectOrSetText(listPosAdjTypes.FindIndex(x => x.DefNum==financeChargeAdjDefNum),
				() => { return financeChargeAdjDefNum==0 ? "" : Defs.GetName(DefCat.AdjTypes,financeChargeAdjDefNum)+" ("+Lan.g(this,"hidden")+")"; });
			comboBillingChargeAdjType.IndexSelectOrSetText(listPosAdjTypes.FindIndex(x => x.DefNum==billingChargeAdjDefNum),
				() => { return billingChargeAdjDefNum==0 ? "" : Defs.GetName(DefCat.AdjTypes,billingChargeAdjDefNum)+" ("+Lan.g(this,"hidden")+")"; });
			comboSalesTaxAdjType.IndexSelectOrSetText(listPosAdjTypes.FindIndex(x => x.DefNum==salesTaxAdjDefNum),
				() => { return salesTaxAdjDefNum==0 ? "" : Defs.GetName(DefCat.AdjTypes,salesTaxAdjDefNum)+" ("+Lan.g(this,"hidden")+")"; });
			textTaxPercent.Text=PrefC.GetDouble(PrefName.SalesTaxPercentage).ToString();
			string[] arrayDefNums=PrefC.GetString(PrefName.BadDebtAdjustmentTypes).Split(new char[] {','}); //comma-delimited list.
			List<long> listBadAdjDefNums = new List<long>();
			foreach(string strDefNum in arrayDefNums) {
				listBadAdjDefNums.Add(PIn.Long(strDefNum));
			}
			FillListboxBadDebt(Defs.GetDefs(DefCat.AdjTypes,listBadAdjDefNums));
			checkAllowFutureDebits.Checked=PrefC.GetBool(PrefName.AccountAllowFutureDebits);
			checkAllowEmailCCReceipt.Checked=PrefC.GetBool(PrefName.AllowEmailCCReceipt);
			List<RigorousAccounting> listEnums=Enum.GetValues(typeof(RigorousAccounting)).OfType<RigorousAccounting>().ToList();
			for(int i=0;i<listEnums.Count;i++) {
				comboRigorousAccounting.Items.Add(listEnums[i].GetDescription());
			}
			comboRigorousAccounting.SelectedIndex=PrefC.GetInt(PrefName.RigorousAccounting);
			List<RigorousAdjustments> listAdjEnums=Enum.GetValues(typeof(RigorousAdjustments)).OfType<RigorousAdjustments>().ToList();
			for(int i=0;i<listAdjEnums.Count;i++) {
				comboRigorousAdjustments.Items.Add(listAdjEnums[i].GetDescription());
			}
			comboRigorousAdjustments.SelectedIndex=PrefC.GetInt(PrefName.RigorousAdjustments);
			for(int i = 0;i<Enum.GetNames(typeof(AutoSplitPreference)).Length;i++) {
				comboAutoSplitPref.Items.Add(Lans.g(this,Enum.GetNames(typeof(AutoSplitPreference))[i]));
			}
			comboAutoSplitPref.SelectedIndex=PrefC.GetInt(PrefName.AutoSplitLogic);
			checkHidePaysplits.Checked=PrefC.GetBool(PrefName.PaymentWindowDefaultHideSplits);
			checkAllowPrepayProvider.Checked=PrefC.GetBool(PrefName.AllowPrepayProvider);
			#endregion Pay/Adj Tab
			#region Insurance Tab
			checkProviderIncomeShows.Checked=PrefC.GetBool(PrefName.ProviderIncomeTransferShows);
			checkClaimMedTypeIsInstWhenInsPlanIsMedical.Checked=PrefC.GetBool(PrefName.ClaimMedTypeIsInstWhenInsPlanIsMedical);
			checkClaimFormTreatDentSaysSigOnFile.Checked=PrefC.GetBool(PrefName.ClaimFormTreatDentSaysSigOnFile);
			textInsWriteoffDescript.Text=PrefC.GetString(PrefName.InsWriteoffDescript);
			textClaimAttachPath.Text=PrefC.GetString(PrefName.ClaimAttachExportPath);
			checkEclaimsMedicalProvTreatmentAsOrdering.Checked=PrefC.GetBool(PrefName.ClaimMedProvTreatmentAsOrdering);
			checkEclaimsSeparateTreatProv.Checked=PrefC.GetBool(PrefName.EclaimsSeparateTreatProv);
			checkClaimsValidateACN.Checked=PrefC.GetBool(PrefName.ClaimsValidateACN);
			checkAllowProcAdjFromClaim.Checked=PrefC.GetBool(PrefName.AllowProcAdjFromClaim);
			comboClaimCredit.Items.AddRange(Enum.GetNames(typeof(ClaimProcCreditsGreaterThanProcFee)));
			comboClaimCredit.SelectedIndex=PrefC.GetInt(PrefName.ClaimProcAllowCreditsGreaterThanProcFee);
			checkAllowFuturePayments.Checked=PrefC.GetBool(PrefName.AllowFutureInsPayments);
			textClaimIdentifier.Text=PrefC.GetString(PrefName.ClaimIdPrefix);
			#endregion Insurance Tab
			#region Misc Account Tab
			checkBalancesDontSubtractIns.Checked=PrefC.GetBool(PrefName.BalancesDontSubtractIns);
			checkAgingMonthly.Checked=PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily);
			if(PrefC.GetBool(PrefName.AgingIsEnterprise)) {//AgingIsEnterprise requires aging to be daily
				checkAgingMonthly.Text+="("+Lan.g(this,"not available with enterprise aging")+")";
				checkAgingMonthly.Enabled=false;
			}
			checkAccountShowPaymentNums.Checked=PrefC.GetBool(PrefName.AccountShowPaymentNums);
			checkStatementsUseSheets.Checked=PrefC.GetBool(PrefName.StatementsUseSheets);
			checkShowFamilyCommByDefault.Checked=PrefC.GetBool(PrefName.ShowAccountFamilyCommEntries);
			checkRecurChargPriProv.Checked=PrefC.GetBool(PrefName.RecurringChargesUsePriProv);
			checkPpoUseUcr.Checked=PrefC.GetBool(PrefName.InsPpoAlwaysUseUcrFee);
			checkRecurringChargesUseTransDate.Checked=PrefC.GetBool(PrefName.RecurringChargesUseTransDate);
			checkStatementInvoiceGridShowWriteoffs.Checked=PrefC.GetBool(PrefName.InvoicePaymentsGridShowNetProd);
			checkAgeNegAdjsByAdjDate.Checked=PrefC.GetBool(PrefName.AgingNegativeAdjsByAdjDate);
			checkShowAllocateUnearnedPaymentPrompt.Checked=PrefC.GetBool(PrefName.ShowAllocateUnearnedPaymentPrompt);
			checkPayPlansExcludePastActivity.Checked=PrefC.GetBool(PrefName.PayPlansExcludePastActivity);
			checkPayPlansUseSheets.Checked=PrefC.GetBool(PrefName.PayPlansUseSheets);
			checkAllowFutureTrans.Checked=PrefC.GetBool(PrefName.FutureTransDatesAllowed);
			foreach(PayPlanVersions version in Enum.GetValues(typeof(PayPlanVersions))) {
				comboPayPlansVersion.Items.Add(Lan.g("enumPayPlanVersions",version.GetDescription()));
			}
			comboPayPlansVersion.SelectedIndex=PrefC.GetInt(PrefName.PayPlansVersion) - 1;
			if(comboPayPlansVersion.SelectedIndex==(int)PayPlanVersions.AgeCreditsAndDebits-1) {//Minus 1 because the enum starts at 1.
				checkHideDueNow.Visible=true;
				checkHideDueNow.Checked=PrefC.GetBool(PrefName.PayPlanHideDueNow);
			}
			else {
				checkHideDueNow.Visible=false;
				checkHideDueNow.Checked=false;
			}
			#endregion Misc Account Tab
			#endregion
			#region TP Module
			//TP module-----------------------------------------------------------------------
			textTreatNote.Text=PrefC.GetString(PrefName.TreatmentPlanNote);
			checkTreatPlanShowCompleted.Checked=PrefC.GetBool(PrefName.TreatPlanShowCompleted);
			if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
				checkTreatPlanShowCompleted.Visible=false;
			}
			else {
				checkTreatPlanShowCompleted.Checked=PrefC.GetBool(PrefName.TreatPlanShowCompleted);
			}
			checkTreatPlanItemized.Checked=PrefC.GetBool(PrefName.TreatPlanItemized);
			checkTPSaveSigned.Checked=PrefC.GetBool(PrefName.TreatPlanSaveSignedToPdf);
			checkFrequency.Checked=PrefC.GetBool(PrefName.InsChecksFrequency);
			textInsBW.Text=PrefC.GetString(PrefName.InsBenBWCodes);
			textInsPano.Text=PrefC.GetString(PrefName.InsBenPanoCodes);
			textInsExam.Text=PrefC.GetString(PrefName.InsBenExamCodes);
			if(!checkFrequency.Checked) {
				textInsBW.Enabled=false;
				textInsPano.Enabled=false;
				textInsExam.Enabled=false;
			}
			radioTreatPlanSortTooth.Checked=PrefC.GetBool(PrefName.TreatPlanSortByTooth) || PrefC.RandomKeys;
			//Currently, the TreatPlanSortByTooth preference gets overridden by 
			//the RandomPrimaryKeys preferece due to "Order Entered" being based on the ProcNum
			groupTreatPlanSort.Enabled=!PrefC.RandomKeys;
			#endregion
			#region Chart Module
			//Chart module-----------------------------------------------------------------------
			comboToothNomenclature.Items.Add(Lan.g(this,"Universal (Common in the US, 1-32)"));
			comboToothNomenclature.Items.Add(Lan.g(this,"FDI Notation (International, 11-48)"));
			comboToothNomenclature.Items.Add(Lan.g(this,"Haderup (Danish)"));
			comboToothNomenclature.Items.Add(Lan.g(this,"Palmer (Ortho)"));
			comboToothNomenclature.SelectedIndex = PrefC.GetInt(PrefName.UseInternationalToothNumbers);
			if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
				labelToothNomenclature.Visible=false;
				comboToothNomenclature.Visible=false;
			}
			checkAutoClearEntryStatus.Checked=PrefC.GetBool(PrefName.AutoResetTPEntryStatus);
			checkAllowSettingProcsComplete.Checked=PrefC.GetBool(PrefName.AllowSettingProcsComplete);
			//checkChartQuickAddHideAmalgam.Checked=PrefC.GetBool(PrefName.ChartQuickAddHideAmalgam); //Deprecated.
			//checkToothChartMoveMenuToRight.Checked=PrefC.GetBool(PrefName.ToothChartMoveMenuToRight);
			textProblemsIndicateNone.Text		=DiseaseDefs.GetName(PrefC.GetLong(PrefName.ProblemsIndicateNone)); //DB maint to fix corruption
			textMedicationsIndicateNone.Text=Medications.GetDescription(PrefC.GetLong(PrefName.MedicationsIndicateNone)); //DB maint to fix corruption
			textAllergiesIndicateNone.Text	=AllergyDefs.GetDescription(PrefC.GetLong(PrefName.AllergiesIndicateNone)); //DB maint to fix corruption
			checkProcGroupNoteDoesAggregate.Checked=PrefC.GetBool(PrefName.ProcGroupNoteDoesAggregate);
			checkChartNonPatientWarn.Checked=PrefC.GetBool(PrefName.ChartNonPatientWarn);
			//checkChartAddProcNoRefreshGrid.Checked=PrefC.GetBool(PrefName.ChartAddProcNoRefreshGrid);//Not implemented.  May revisit some day.
			checkMedicalFeeUsedForNewProcs.Checked=PrefC.GetBool(PrefName.MedicalFeeUsedForNewProcs);
			checkProvColorChart.Checked=PrefC.GetBool(PrefName.UseProviderColorsInChart);
			checkPerioSkipMissingTeeth.Checked=PrefC.GetBool(PrefName.PerioSkipMissingTeeth);
			checkPerioTreatImplantsAsNotMissing.Checked=PrefC.GetBool(PrefName.PerioTreatImplantsAsNotMissing);
			if(PrefC.GetByte(PrefName.DxIcdVersion)==9) {
				checkDxIcdVersion.Checked=false;
			}
			else {//ICD-10
				checkDxIcdVersion.Checked=true;
			}
			SetIcdLabels();
			textICD9DefaultForNewProcs.Text=PrefC.GetString(PrefName.ICD9DefaultForNewProcs);
			checkProcLockingIsAllowed.Checked=PrefC.GetBool(PrefName.ProcLockingIsAllowed);
			textMedDefaultStopDays.Text=PrefC.GetString(PrefName.MedDefaultStopDays);
			checkScreeningsUseSheets.Checked=PrefC.GetBool(PrefName.ScreeningsUseSheets);
			checkProcsPromptForAutoNote.Checked=PrefC.GetBool(PrefName.ProcPromptForAutoNote);
			for(int i=0;i<Enum.GetNames(typeof(ProcCodeListSort)).Length;i++) {
				comboProcCodeListSort.Items.Add(Enum.GetNames(typeof(ProcCodeListSort))[i]);
			}
			comboProcCodeListSort.SelectedIndex=PrefC.GetInt(PrefName.ProcCodeListSortOrder);
			checkProcEditRequireAutoCode.Checked=PrefC.GetBool(PrefName.ProcEditRequireAutoCodes);
			checkClaimProcsAllowEstimatesOnCompl.Checked=PrefC.GetBool(PrefName.ClaimProcsAllowedToBackdate);
			checkSignatureAllowDigital.Checked=PrefC.GetBool(PrefName.SignatureAllowDigital);
			checkCommLogAutoSave.Checked=PrefC.GetBool(PrefName.CommLogAutoSave);
			comboProcFeeUpdatePrompt.Items.Add(Lan.g(this,"No prompt, don't change fee"));
			comboProcFeeUpdatePrompt.Items.Add(Lan.g(this,"No prompt, always change fee"));
			comboProcFeeUpdatePrompt.Items.Add(Lan.g(this,"Prompt - When patient portion changes"));
			comboProcFeeUpdatePrompt.Items.Add(Lan.g(this,"Prompt - Always"));
			comboProcFeeUpdatePrompt.SelectedIndex=PrefC.GetInt(PrefName.ProcFeeUpdatePrompt);
			checkProcProvChangesCp.Checked=PrefC.GetBool(PrefName.ProcProvChangesClaimProcWithClaim);
			checkBoxRxClinicUseSelected.Checked=PrefC.GetBool(PrefName.ElectronicRxClinicUseSelected);
			checkProcNoteConcurrencyMerge.Checked=PrefC.GetBool(PrefName.ProcNoteConcurrencyMerge);
			checkIsAlertRadiologyProcsEnabled.Checked=PrefC.GetBool(PrefName.IsAlertRadiologyProcsEnabled);
			checkShowPlannedApptPrompt.Checked=PrefC.GetBool(PrefName.ShowPlannedAppointmentPrompt);
			#endregion
			#region Image Module
			//Image module-----------------------------------------------------------------------
			switch(PrefC.GetInt(PrefName.ImagesModuleTreeIsCollapsed)) {
				case 0:
					radioImagesModuleTreeIsExpanded.Checked=true;
					break;
				case 1:
					radioImagesModuleTreeIsCollapsed.Checked=true;
					break;
				case 2:
					radioImagesModuleTreeIsPersistentPerUser.Checked=true;
					break;
			}
			#endregion
			#region Manage Module
			//Manage module----------------------------------------------------------------------
			checkRxSendNewToQueue.Checked=PrefC.GetBool(PrefName.RxSendNewToQueue);
			int claimZeroPayRollingDays=PrefC.GetInt(PrefName.ClaimPaymentNoShowZeroDate);
			if(claimZeroPayRollingDays>=0) {
				textClaimsReceivedDays.Text=(claimZeroPayRollingDays+1).ToString();//The minimum value is now 1 ("today"), to match other areas of OD.
			}
			for(int i=0;i<7;i++) {
				comboTimeCardOvertimeFirstDayOfWeek.Items.Add(Lan.g("enumDayOfWeek",Enum.GetNames(typeof(DayOfWeek))[i]));
			}
			comboTimeCardOvertimeFirstDayOfWeek.SelectedIndex=PrefC.GetInt(PrefName.TimeCardOvertimeFirstDayOfWeek);
			checkTimeCardADP.Checked=PrefC.GetBool(PrefName.TimeCardADPExportIncludesName);
			checkClaimsSendWindowValidateOnLoad.Checked=PrefC.GetBool(PrefName.ClaimsSendWindowValidatesOnLoad);
			checkScheduleProvEmpSelectAll.Checked=PrefC.GetBool(PrefName.ScheduleProvEmpSelectAll);
			//Statements
			checkStatementShowReturnAddress.Checked=PrefC.GetBool(PrefName.StatementShowReturnAddress);
			checkShowCC.Checked=PrefC.GetBool(PrefName.StatementShowCreditCard);
			checkStatementShowNotes.Checked=PrefC.GetBool(PrefName.StatementShowNotes);
			checkStatementShowAdjNotes.Checked=PrefC.GetBool(PrefName.StatementShowAdjNotes);
			checkStatementShowProcBreakdown.Checked=PrefC.GetBool(PrefName.StatementShowProcBreakdown);
			comboUseChartNum.Items.Add(Lan.g(this,"PatNum"));
			comboUseChartNum.Items.Add(Lan.g(this,"ChartNumber"));
			if(PrefC.GetBool(PrefName.StatementAccountsUseChartNumber)) {
				comboUseChartNum.SelectedIndex=1;
			}
			else {
				comboUseChartNum.SelectedIndex=0;
			}
			if(PrefC.GetLong(PrefName.StatementsCalcDueDate)!=-1) {
				textStatementsCalcDueDate.Text=PrefC.GetLong(PrefName.StatementsCalcDueDate).ToString();
			}
			textPayPlansBillInAdvanceDays.Text=PrefC.GetLong(PrefName.PayPlansBillInAdvanceDays).ToString();
			textBillingElectBatchMax.Text=PrefC.GetInt(PrefName.BillingElectBatchMax).ToString();
			checkIntermingleDefault.Checked=PrefC.GetBool(PrefName.IntermingleFamilyDefault);
			checkBillingShowProgress.Checked=PrefC.GetBool(PrefName.BillingShowSendProgress);
			checkClaimPaymentBatchOnly.Checked=PrefC.GetBool(PrefName.ClaimPaymentBatchOnly);
			checkEraOneClaimPerPage.Checked=PrefC.GetBool(PrefName.EraPrintOneClaimPerPage);
			checkShowAutoDeposit.Checked=PrefC.GetBool(PrefName.ShowAutoDeposit);
			#endregion
		}

		#region Appointments
		private void checkApptsRequireProcs_CheckedChanged(object sender,EventArgs e) {
			textApptWithoutProcsDefaultLength.Enabled=(!checkApptsRequireProcs.Checked);
		}
		#endregion Appointments

		private void checkAllowedFeeSchedsAutomate_Click(object sender,EventArgs e) {
			if(!checkAllowedFeeSchedsAutomate.Checked){
				return;
			}
			if(!MsgBox.Show(this,true,"Allowed fee schedules will now be set up for all insurance plans that do not already have one.\r\nThe name of each fee schedule will exactly match the name of the carrier.\r\nOnce created, allowed fee schedules can be easily managed from the fee schedules window.\r\nContinue?")){
				checkAllowedFeeSchedsAutomate.Checked=false;
				return;
			}
			Cursor=Cursors.WaitCursor;
			long schedsAdded=InsPlans.GenerateAllowedFeeSchedules();
			Cursor=Cursors.Default;
			MessageBox.Show(Lan.g(this,"Done.  Allowed fee schedules added: ")+schedsAdded.ToString());
			DataValid.SetInvalid(InvalidType.FeeScheds);
		}

		private void checkInsDefaultShowUCRonClaims_Click(object sender,EventArgs e) {
			if(!checkInsDefaultShowUCRonClaims.Checked) {
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Would you like to immediately change all plans to show office UCR fees on claims?")) {
				return;
			}
			long plansAffected=InsPlans.SetAllPlansToShowUCR();
			MessageBox.Show(Lan.g(this,"Plans affected: ")+plansAffected.ToString());
		}

		private void checkInsDefaultAssignmentOfBenefits_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.InsPlanChangeAssign)) {
				return;
			}
			long subsAffected;
			if(checkInsDefaultAssignmentOfBenefits.Checked) {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Would you like to immediately change all plans to use assignment of benefits?")) {
					return;
				}
				subsAffected=InsSubs.SetAllSubsAssignBen(true);
				MessageBox.Show(Lan.g(this,"Plans affected:")+" "+subsAffected.ToString());
			}
			else {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Would you like to immediately change all plans to not use assignment of benefits?")) {
					return;
				}
				subsAffected=InsSubs.SetAllSubsAssignBen(false);
				MessageBox.Show(Lan.g(this,"Plans affected:")+" "+subsAffected.ToString());
			}
		}

		private void butProblemsIndicateNone_Click(object sender,EventArgs e) {
			FormDiseaseDefs formD=new FormDiseaseDefs();
			formD.IsSelectionMode=true;
			formD.ShowDialog();
			if(formD.DialogResult!=DialogResult.OK) {
				return;
			}
			//the list should only ever contain one item.
			if(Prefs.UpdateLong(PrefName.ProblemsIndicateNone,formD.ListSelectedDiseaseDefs[0].DiseaseDefNum)) {
				_changed=true;
			}
			textProblemsIndicateNone.Text=formD.ListSelectedDiseaseDefs[0].DiseaseName;
		}

		private void butMedicationsIndicateNone_Click(object sender,EventArgs e) {
			FormMedications formM=new FormMedications();
			formM.IsSelectionMode=true;
			formM.ShowDialog();
			if(formM.DialogResult!=DialogResult.OK) {
				return;
			}
			if(Prefs.UpdateLong(PrefName.MedicationsIndicateNone,formM.SelectedMedicationNum)) {
				_changed=true;
			}
			textMedicationsIndicateNone.Text=Medications.GetDescription(formM.SelectedMedicationNum);
		}

		private void butAllergiesIndicateNone_Click(object sender,EventArgs e) {
			FormAllergySetup formA=new FormAllergySetup();
			formA.IsSelectionMode=true;
			formA.ShowDialog();
			if(formA.DialogResult!=DialogResult.OK) {
				return;
			}
			if(Prefs.UpdateLong(PrefName.AllergiesIndicateNone,formA.SelectedAllergyDefNum)) {
				_changed=true;
			}
			textAllergiesIndicateNone.Text=AllergyDefs.GetOne(formA.SelectedAllergyDefNum).Description;
		}

		private void butColor_Click(object sender,EventArgs e) {
			colorDialog.Color=butColor.BackColor;//Pre-select current pref color
			if(colorDialog.ShowDialog()==DialogResult.OK) {
				butColor.BackColor=colorDialog.Color;
			}
		}

		private void butApptLineColor_Click(object sender,EventArgs e) {
			colorDialog.Color=butColor.BackColor;//Pre-select current pref color
			if(colorDialog.ShowDialog()==DialogResult.OK) {
				butApptLineColor.BackColor=colorDialog.Color;
			}
		}

		private void checkChartNonPatientWarn_Click(object sender,EventArgs e) {
			if(Prefs.UpdateBool(PrefName.ChartNonPatientWarn,checkChartNonPatientWarn.Checked)) {
				_changed=true;
			}
		}

		private void checkTreatPlanItemized_Click(object sender,EventArgs e) {
			if(Prefs.UpdateBool(PrefName.TreatPlanItemized,checkTreatPlanItemized.Checked)) {
				_changed=true;
			}
		}

		private void checkAppointmentTimeIsLocked_MouseUp(object sender,MouseEventArgs e) {
			if(checkAppointmentTimeIsLocked.Checked) {
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Would you like to lock appointment times for all existing appointments?")){
					Appointments.SetAptTimeLocked();
				}
			}
		}

		private void comboCobRule_SelectionChangeCommitted(object sender,EventArgs e) {
			if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Would you like to change the COB rule for all existing insurance plans?")) {
				InsPlans.UpdateCobRuleForAll((EnumCobRule)comboCobRule.SelectedIndex);
			}
		}

		private void comboRigorousAccounting_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboRigorousAccounting.SelectedIndex==(int)RigorousAccounting.EnforceFully) {
				checkAllowPrepayProvider.Visible=true;
				checkAllowPrepayProvider.Checked=PrefC.GetBool(PrefName.AllowPrepayProvider);
			}
			else {
				checkAllowPrepayProvider.Visible=false;
				checkAllowPrepayProvider.Checked=false;
			}
		}

		private void checkProcLockingIsAllowed_Click(object sender,EventArgs e) {
			if(checkProcLockingIsAllowed.Checked) {//if user is checking box			
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This option is not normally used, because all notes are already locked internally, and all changes to notes are viewable in the audit mode of the Chart module.  This option is only for offices that insist on locking each procedure and only allowing notes to be appended.  Using this option, there really is no way to unlock a procedure, regardless of security permission.  So locked procedures can instead be marked as invalid in the case of mistakes.  But it's a hassle to mark procedures invalid, and they also cause clutter.  This option can be turned off later, but locked procedures will remain locked.\r\n\r\nContinue anyway?")) {
					checkProcLockingIsAllowed.Checked=false;
				}
			}
			else {//unchecking box
				MsgBox.Show(this,"Turning off this option will not affect any procedures that are already locked or invalidated.");
			}
		}

		private void comboPayPlansVersion_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboPayPlansVersion.SelectedIndex==(int)PayPlanVersions.AgeCreditsAndDebits-1) {//Minus 1 because the enum starts at 1.
				checkHideDueNow.Visible=true;
				checkHideDueNow.Checked=PrefC.GetBool(PrefName.PayPlanHideDueNow);
			}
			else {
				checkHideDueNow.Visible=false;
				checkHideDueNow.Checked=false;
			}
		}

		private void radioTreatPlanSortTooth_Click(object sender,EventArgs e) {
			if(PrefC.GetBool(PrefName.TreatPlanSortByTooth)!=radioTreatPlanSortTooth.Checked) {
				MsgBox.Show(this,"You will need to restart the program for the change to take effect.");
			}
		}

		private void radioTreatPlanSortOrder_Click(object sender,EventArgs e) {
			//Sort by order is a false 
			if(PrefC.GetBool(PrefName.TreatPlanSortByTooth)==radioTreatPlanSortOrder.Checked) {
				MsgBox.Show(this,"You will need to restart the program for the change to take effect.");
			}
		}

		private void butDiagnosisCode_Click(object sender,EventArgs e) {
			if(checkDxIcdVersion.Checked) {//ICD-10
				FormIcd10s formI=new FormIcd10s();
				formI.IsSelectionMode=true;
				if(formI.ShowDialog()==DialogResult.OK) {
					textICD9DefaultForNewProcs.Text=formI.SelectedIcd10.Icd10Code;
				}
			}
			else {//ICD-9
				FormIcd9s formI=new FormIcd9s();
				formI.IsSelectionMode=true;
				if(formI.ShowDialog()==DialogResult.OK) {
					textICD9DefaultForNewProcs.Text=formI.SelectedIcd9.ICD9Code;
				}
			}
		}

		private void SetIcdLabels() {
			byte icdVersion=9;
			if(checkDxIcdVersion.Checked) {
				icdVersion=10;
			}
			labelIcdCodeDefault.Text=Lan.g(this,"Default ICD")+"-"+icdVersion+" "+Lan.g(this,"code for new procedures");
		}

		private void checkDxIcdVersion_Click(object sender,EventArgs e) {
			SetIcdLabels();
		}

		private void checkFrequency_Click(object sender,EventArgs e) {
			textInsBW.Enabled=checkFrequency.Checked;
			textInsPano.Enabled=checkFrequency.Checked;
			textInsExam.Enabled=checkFrequency.Checked;
		}

		private void checkAgeAdjByAdjDate_Click(object sender,EventArgs e) {
			if(checkAgeNegAdjsByAdjDate.Checked) {
				checkAgeNegAdjsByAdjDate.Checked=MsgBox.Show(this,MsgBoxButtons.YesNo,
					"This will change the way aging is calculated and may affect historical and future accounts receivable.  Continue?");
			}
		}

		private void butReplacements_Click(object sender,EventArgs e) {
			FormMessageReplacements form=new FormMessageReplacements(MessageReplaceType.Patient);
			form.IsSelectionMode=true;
			form.ShowDialog();
			if(form.DialogResult!=DialogResult.OK) {
				return;
			}
			textClaimIdentifier.Focus();
			int cursorIndex=textClaimIdentifier.SelectionStart;
			textClaimIdentifier.Text=textClaimIdentifier.Text.Insert(cursorIndex,form.Replacement);
			textClaimIdentifier.SelectionStart=cursorIndex+form.Replacement.Length;
		}

		private void butBadDebt_Click(object sender,EventArgs e) {
			string[] arrayDefNums=PrefC.GetString(PrefName.BadDebtAdjustmentTypes).Split(new char[] {','}); //comma-delimited list.
			List<long> listBadAdjDefNums = new List<long>();
			foreach(string strDefNum in arrayDefNums) {
				listBadAdjDefNums.Add(PIn.Long(strDefNum));
			}
			List<Def> listBadAdjDefs=Defs.GetDefs(DefCat.AdjTypes,listBadAdjDefNums);
			FormDefinitionPicker FormDP = new FormDefinitionPicker(DefCat.AdjTypes,listBadAdjDefs);
			FormDP.HasShowHiddenOption=true;
			FormDP.IsMultiSelectionMode=true;
			FormDP.ShowDialog();
			if(FormDP.DialogResult==DialogResult.OK) {
				FillListboxBadDebt(FormDP.ListSelectedDefs);
				string strListBadDebtAdjTypes=String.Join(",",FormDP.ListSelectedDefs.Select(x => x.DefNum));
				Prefs.UpdateString(PrefName.BadDebtAdjustmentTypes,strListBadDebtAdjTypes);
			}
		}

		private void FillListboxBadDebt(List<Def> listSelectedDefs) {
			listboxBadDebtAdjs.Items.Clear();
			foreach(Def defCur in listSelectedDefs) {
				listboxBadDebtAdjs.Items.Add(defCur.ItemName);
			}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			float percent=0;//Placeholder
			if(!float.TryParse(textDiscountPercentage.Text,out percent)) {
				MsgBox.Show(this,"Procedure discount percent is invalid. Please enter a valid number to continue.");
				return;
			}
			if(textStatementsCalcDueDate.errorProvider1.GetError(textStatementsCalcDueDate)!=""
				| textPayPlansBillInAdvanceDays.errorProvider1.GetError(textPayPlansBillInAdvanceDays)!=""
				| textBillingElectBatchMax.errorProvider1.GetError(textBillingElectBatchMax)!=""
				| textApptWithoutProcsDefaultLength.errorProvider1.GetError(textApptWithoutProcsDefaultLength)!="")
			{
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return;
			}
			int noteLength=0;//Placeholder
			if(!int.TryParse(textApptBubNoteLength.Text,out noteLength)) {
				MsgBox.Show(this,"Max appointment note length is invalid. Please enter a valid number to continue.");
				return;
			}
			if(noteLength<0) {
				MsgBox.Show(this,"Max appointment note length cannot be a negative number.");
				return;
			}
			int waitingRoomAlertTime=0;
			try {
				waitingRoomAlertTime=PIn.Int(textWaitRoomWarn.Text);
				if(waitingRoomAlertTime<0) {
					throw new ApplicationException("Waiting room time cannot be negative");//User never sees this message.
				}
			}
			catch {
				MsgBox.Show(this,"Waiting room alert time is invalid.");
				return;
			}
			int daysStop=0;
			if(!int.TryParse(textMedDefaultStopDays.Text,out daysStop)) {
				MsgBox.Show(this,"Days until medication order stop date entered was is invalid. Please enter a valid number to continue.");
				return;
			}
			if(daysStop<0) {
				MsgBox.Show(this,"Days until medication order stop date cannot be a negative number.");
				return;
			}
			double taxPercent=0;
			if(!double.TryParse(textTaxPercent.Text,out taxPercent)) {
				MsgBox.Show(this,"Sales Tax percent is invalid.  Please enter a valid number to continue.");
				return;
			}
			if(taxPercent<0) {
				MsgBox.Show(this,"Sales Tax percent cannot be a negative number.");
				return;
			}
			DateTime claimSnapshotRunTime=DateTime.MinValue;
			if(!DateTime.TryParse(textClaimSnapshotRunTime.Text,out claimSnapshotRunTime)) {
				MsgBox.Show(this,"Service Snapshot Run Time must be a valid time value.");
				return;
			}
			if(textClaimsReceivedDays.errorProvider1.GetError(textClaimsReceivedDays)!="") {
				MsgBox.Show(this,"Show claims received after days must be a positive integer or blank.");
				return;
			}
			claimSnapshotRunTime=new DateTime(1881,01,01,claimSnapshotRunTime.Hour,claimSnapshotRunTime.Minute,claimSnapshotRunTime.Second);
			int imageModuleIsCollapsedVal=0;
			if(radioImagesModuleTreeIsExpanded.Checked) {
				imageModuleIsCollapsedVal=0;
			}
			else if(radioImagesModuleTreeIsCollapsed.Checked) {
				imageModuleIsCollapsedVal=1;
			}
			else if(radioImagesModuleTreeIsPersistentPerUser.Checked) {
				imageModuleIsCollapsedVal=2;
			}
			if(PrefC.GetString(PrefName.TreatmentPlanNote)!=textTreatNote.Text) {
				List<long> listTreatPlanNums=TreatPlans.GetNumsByNote(PrefC.GetString(PrefName.TreatmentPlanNote));//Find active/inactive TP's that match exactly.
				if(listTreatPlanNums.Count>0) {
					DialogResult dr=MessageBox.Show(Lan.g(this,"Unsaved treatment plans found with default notes")+": "+listTreatPlanNums.Count+"\r\n"
						+Lan.g(this,"Would you like to change them now?"),"",MessageBoxButtons.YesNoCancel);
					switch(dr) {
						case DialogResult.Cancel:
							return;
						case DialogResult.Yes:
						case DialogResult.OK:
							TreatPlans.UpdateNotes(textTreatNote.Text,listTreatPlanNums);//change tp notes
							break;
						default://includes "No"
							//do nothing
							break;
					}
				}
			}//end if TP Note Changed
			//int payPlanVersion=1;
			//if(PrefC.GetInt(PrefName.PayPlansVersion) != 2) {
			//	payPlanVersion=2;
			//}
			if(
				#region Appointment Module
				Prefs.UpdateBool(PrefName.AppointmentBubblesDisabled,checkAppointmentBubblesDisabled.Checked)
				| Prefs.UpdateBool(PrefName.ApptBubbleDelay,checkApptBubbleDelay.Checked)
				| Prefs.UpdateBool(PrefName.SolidBlockouts,checkSolidBlockouts.Checked)
				//| Prefs.UpdateBool(PrefName.BrokenApptCommLogNotAdjustment,checkBrokenApptNote.Checked) //Deprecated
				| Prefs.UpdateBool(PrefName.BrokenApptAdjustment,checkBrokenApptAdjustment.Checked)
				| Prefs.UpdateBool(PrefName.BrokenApptCommLog,checkBrokenApptCommLog.Checked)
				| Prefs.UpdateInt(PrefName.BrokenApptProcedure,(int)_listComboBrokenApptProcOptions[comboBrokenApptProc.SelectedIndex])
				| Prefs.UpdateBool(PrefName.ApptExclamationShowForUnsentIns,checkApptExclamation.Checked)
				| Prefs.UpdateBool(PrefName.ApptModuleRefreshesEveryMinute,checkApptRefreshEveryMinute.Checked)
				| Prefs.UpdateInt(PrefName.AppointmentSearchBehavior,comboSearchBehavior.SelectedIndex)
				| Prefs.UpdateBool(PrefName.AppointmentTimeIsLocked,checkAppointmentTimeIsLocked.Checked)
				| Prefs.UpdateInt(PrefName.AppointmentBubblesNoteLength,noteLength)
				| Prefs.UpdateBool(PrefName.WaitingRoomFilterByView,checkWaitingRoomFilterByView.Checked)
				| Prefs.UpdateInt(PrefName.WaitingRoomAlertTime,waitingRoomAlertTime)
				| Prefs.UpdateInt(PrefName.WaitingRoomAlertColor,butColor.BackColor.ToArgb())
				| Prefs.UpdateInt(PrefName.AppointmentTimeLineColor,butApptLineColor.BackColor.ToArgb())
				| Prefs.UpdateBool(PrefName.ApptModuleDefaultToWeek,checkApptModuleDefaultToWeek.Checked)
				| Prefs.UpdateBool(PrefName.AppointmentClinicTimeReset,checkApptTimeReset.Checked)
				| Prefs.UpdateBool(PrefName.ApptModuleAdjustmentsInProd,checkApptModuleAdjInProd.Checked)
				| Prefs.UpdateBool(PrefName.ApptSecondaryProviderConsiderOpOnly,checkUseOpHygProv.Checked)
				| Prefs.UpdateBool(PrefName.ApptModuleProductionUsesOps,checkApptModuleProductionUsesOps.Checked)
				| Prefs.UpdateBool(PrefName.ApptsRequireProc,checkApptsRequireProcs.Checked)
				| Prefs.UpdateBool(PrefName.ApptAllowFutureComplete,checkApptAllowFutureComplete.Checked)
				| Prefs.UpdateBool(PrefName.ApptAllowEmptyComplete,checkApptAllowEmptyComplete.Checked)
				| Prefs.UpdateDouble(PrefName.FormClickDelay,((ODBoxItem<double>)comboDelay.Items[comboDelay.SelectedIndex]).Tag)
				| Prefs.UpdateString(PrefName.AppointmentWithoutProcsDefaultLength,textApptWithoutProcsDefaultLength.Text)
			#endregion
				#region Family Module
				| Prefs.UpdateBool(PrefName.InsurancePlansShared,checkInsurancePlansShared.Checked)
				| Prefs.UpdateBool(PrefName.InsDefaultPPOpercent,checkPPOpercentage.Checked)
				| Prefs.UpdateBool(PrefName.AllowedFeeSchedsAutomate,checkAllowedFeeSchedsAutomate.Checked)
				| Prefs.UpdateBool(PrefName.CoPay_FeeSchedule_BlankLikeZero,checkCoPayFeeScheduleBlankLikeZero.Checked)
				| Prefs.UpdateBool(PrefName.InsDefaultShowUCRonClaims,checkInsDefaultShowUCRonClaims.Checked)
				| Prefs.UpdateBool(PrefName.InsDefaultAssignBen,checkInsDefaultAssignmentOfBenefits.Checked)
				| Prefs.UpdateInt(PrefName.InsDefaultCobRule,comboCobRule.SelectedIndex)
				| Prefs.UpdateBool(PrefName.TextMsgOkStatusTreatAsNo,checkTextMsgOkStatusTreatAsNo.Checked)
				| Prefs.UpdateBool(PrefName.FamPhiAccess,checkFamPhiAccess.Checked)
				| Prefs.UpdateBool(PrefName.InsPPOsecWriteoffs,checkInsPPOsecWriteoffs.Checked)
				| Prefs.UpdateBool(PrefName.ShowFeatureGoogleMaps,checkGoogleAddress.Checked)
				| Prefs.UpdateBool(PrefName.PriProvDefaultToSelectProv,checkSelectProv.Checked)
				| Prefs.UpdateInt(PrefName.SuperFamSortStrategy,comboSuperFamSort.SelectedIndex)
				| Prefs.UpdateBool(PrefName.PatientAllSuperFamilySync,checkSuperFamSync.Checked)
				| Prefs.UpdateBool(PrefName.SuperFamNewPatAddIns,checkSuperFamAddIns.Checked)
				| Prefs.UpdateBool(PrefName.CloneCreateSuperFamily,checkSuperFamCloneCreate.Checked)
				| Prefs.UpdateDateT(PrefName.ClaimSnapshotRunTime,claimSnapshotRunTime)
				| Prefs.UpdateBool(PrefName.ClaimPrintProcChartedDesc,checkClaimUseOverrideProcDescript.Checked)
				| Prefs.UpdateBool(PrefName.ClaimTrackingRequiresError,checkClaimTrackingRequireError.Checked)
				| Prefs.UpdateBool(PrefName.PatInitBillingTypeFromPriInsPlan,checkPatInitBillingTypeFromPriInsPlan.Checked)
				| Prefs.UpdateBool(PrefName.ShowPreferedReferrals,checkPreferredReferrals.Checked)
				| Prefs.UpdateBool(PrefName.AddFamilyInheritsEmail,checkAutoFillPatEmail.Checked)
				| Prefs.UpdateBool(PrefName.ClinicAllowPatientsAtHeadquarters,checkAllowPatsAtHQ.Checked)
				#endregion
				#region Account Module
				| Prefs.UpdateBool(PrefName.BalancesDontSubtractIns,checkBalancesDontSubtractIns.Checked)
				| Prefs.UpdateBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily,checkAgingMonthly.Checked)
				| Prefs.UpdateBool(PrefName.StoreCCtokens,checkStoreCCTokens.Checked)
				| Prefs.UpdateBool(PrefName.ProviderIncomeTransferShows,checkProviderIncomeShows.Checked)
				| Prefs.UpdateBool(PrefName.ShowAccountFamilyCommEntries,checkShowFamilyCommByDefault.Checked)
				| Prefs.UpdateBool(PrefName.ClaimFormTreatDentSaysSigOnFile,checkClaimFormTreatDentSaysSigOnFile.Checked)
				| Prefs.UpdateString(PrefName.ClaimAttachExportPath,textClaimAttachPath.Text)
				| Prefs.UpdateBool(PrefName.EclaimsSeparateTreatProv,checkEclaimsSeparateTreatProv.Checked)
				| Prefs.UpdateBool(PrefName.ClaimsValidateACN,checkClaimsValidateACN.Checked)
				| Prefs.UpdateBool(PrefName.ClaimMedTypeIsInstWhenInsPlanIsMedical,checkClaimMedTypeIsInstWhenInsPlanIsMedical.Checked)
				| Prefs.UpdateBool(PrefName.AccountShowPaymentNums,checkAccountShowPaymentNums.Checked)
				| Prefs.UpdateBool(PrefName.StatementsUseSheets,checkStatementsUseSheets.Checked)
				| Prefs.UpdateBool(PrefName.PayPlansUseSheets,checkPayPlansUseSheets.Checked)
				| Prefs.UpdateBool(PrefName.RecurringChargesUsePriProv,checkRecurChargPriProv.Checked)
				| Prefs.UpdateString(PrefName.InsWriteoffDescript,textInsWriteoffDescript.Text)
				| Prefs.UpdateInt(PrefName.PaymentClinicSetting,comboPaymentClinicSetting.SelectedIndex)
				| Prefs.UpdateBool(PrefName.PaymentsPromptForPayType,checkPaymentsPromptForPayType.Checked)
				| Prefs.UpdateInt(PrefName.PayPlansVersion,comboPayPlansVersion.SelectedIndex+1)
				| Prefs.UpdateBool(PrefName.ClaimMedProvTreatmentAsOrdering,checkEclaimsMedicalProvTreatmentAsOrdering.Checked)
				| Prefs.UpdateBool(PrefName.InsPpoAlwaysUseUcrFee,checkPpoUseUcr.Checked)
				| Prefs.UpdateBool(PrefName.ProcPromptForAutoNote,checkProcsPromptForAutoNote.Checked)
				| Prefs.UpdateDouble(PrefName.SalesTaxPercentage,taxPercent,false)//Do not round this double for Hawaii
				| Prefs.UpdateBool(PrefName.PayPlansExcludePastActivity,checkPayPlansExcludePastActivity.Checked)
				| Prefs.UpdateBool(PrefName.RecurringChargesUseTransDate,checkRecurringChargesUseTransDate.Checked)
				| Prefs.UpdateBool(PrefName.InvoicePaymentsGridShowNetProd,checkStatementInvoiceGridShowWriteoffs.Checked)
				| Prefs.UpdateBool(PrefName.AccountAllowFutureDebits,checkAllowFutureDebits.Checked)
				| Prefs.UpdateBool(PrefName.PayPlanHideDueNow,checkHideDueNow.Checked)
				| Prefs.UpdateBool(PrefName.AllowProcAdjFromClaim,checkAllowProcAdjFromClaim.Checked)
				| Prefs.UpdateInt(PrefName.ClaimProcAllowCreditsGreaterThanProcFee,comboClaimCredit.SelectedIndex)
				| Prefs.UpdateBool(PrefName.AllowEmailCCReceipt,checkAllowEmailCCReceipt.Checked)
				| Prefs.UpdateInt(PrefName.AutoSplitLogic,comboAutoSplitPref.SelectedIndex)
				| Prefs.UpdateString(PrefName.ClaimIdPrefix,textClaimIdentifier.Text)
				//| Prefs.UpdateInt(PrefName.RigorousAccounting,comboRigorousAccounting.SelectedIndex) //Checked later in order to make audit entry
				//| Prefs.UpdateBool(PrefName.PayPlanHideDebitsFromAccountModule,checkHidePayPlanDebits.Checked)
				| Prefs.UpdateBool(PrefName.AllowFutureInsPayments,checkAllowFuturePayments.Checked)
				| Prefs.UpdateBool(PrefName.AgingNegativeAdjsByAdjDate,checkAgeNegAdjsByAdjDate.Checked)
				| Prefs.UpdateBool(PrefName.PaymentWindowDefaultHideSplits,checkHidePaysplits.Checked)
				| Prefs.UpdateBool(PrefName.ShowAllocateUnearnedPaymentPrompt,checkShowAllocateUnearnedPaymentPrompt.Checked)
				| Prefs.UpdateBool(PrefName.FutureTransDatesAllowed,checkAllowFutureTrans.Checked)
				| Prefs.UpdateBool(PrefName.AllowPrepayProvider,checkAllowPrepayProvider.Checked)
				#endregion
				#region TP Module
				| Prefs.UpdateString(PrefName.TreatmentPlanNote,textTreatNote.Text)
				| Prefs.UpdateBool(PrefName.TreatPlanShowCompleted,checkTreatPlanShowCompleted.Checked)
				| Prefs.UpdateDouble(PrefName.TreatPlanDiscountPercent,percent)
				| Prefs.UpdateBool(PrefName.TreatPlanSaveSignedToPdf,checkTPSaveSigned.Checked)
				| Prefs.UpdateBool(PrefName.InsChecksFrequency,checkFrequency.Checked)
				| Prefs.UpdateString(PrefName.InsBenBWCodes,textInsBW.Text)
				| Prefs.UpdateString(PrefName.InsBenPanoCodes,textInsPano.Text)
				| Prefs.UpdateString(PrefName.InsBenExamCodes,textInsExam.Text)
				| Prefs.UpdateBool(PrefName.TreatPlanSortByTooth,radioTreatPlanSortTooth.Checked || PrefC.RandomKeys)
				#endregion
				#region Chart Module
				| Prefs.UpdateBool(PrefName.AutoResetTPEntryStatus,checkAutoClearEntryStatus.Checked)
				| Prefs.UpdateBool(PrefName.AllowSettingProcsComplete,checkAllowSettingProcsComplete.Checked)
				| Prefs.UpdateLong(PrefName.UseInternationalToothNumbers,comboToothNomenclature.SelectedIndex)
				| Prefs.UpdateBool(PrefName.ProcGroupNoteDoesAggregate,checkProcGroupNoteDoesAggregate.Checked)
				| Prefs.UpdateBool(PrefName.MedicalFeeUsedForNewProcs,checkMedicalFeeUsedForNewProcs.Checked)
				| Prefs.UpdateByte(PrefName.DxIcdVersion,(byte)(checkDxIcdVersion.Checked?10:9))
				| Prefs.UpdateString(PrefName.ICD9DefaultForNewProcs,textICD9DefaultForNewProcs.Text)
				| Prefs.UpdateBool(PrefName.ProcLockingIsAllowed,checkProcLockingIsAllowed.Checked)
				| Prefs.UpdateInt(PrefName.MedDefaultStopDays,daysStop)
				| Prefs.UpdateBool(PrefName.UseProviderColorsInChart,checkProvColorChart.Checked)
				| Prefs.UpdateBool(PrefName.PerioSkipMissingTeeth,checkPerioSkipMissingTeeth.Checked)
				| Prefs.UpdateBool(PrefName.PerioTreatImplantsAsNotMissing,checkPerioTreatImplantsAsNotMissing.Checked)
				| Prefs.UpdateBool(PrefName.ScreeningsUseSheets,checkScreeningsUseSheets.Checked)
				| Prefs.UpdateInt(PrefName.ProcCodeListSortOrder,comboProcCodeListSort.SelectedIndex)
				| Prefs.UpdateBool(PrefName.ProcEditRequireAutoCodes,checkProcEditRequireAutoCode.Checked)
				| Prefs.UpdateBool(PrefName.ClaimProcsAllowedToBackdate,checkClaimProcsAllowEstimatesOnCompl.Checked)
				| Prefs.UpdateBool(PrefName.SignatureAllowDigital,checkSignatureAllowDigital.Checked)
				| Prefs.UpdateBool(PrefName.CommLogAutoSave,checkCommLogAutoSave.Checked)
				| Prefs.UpdateLong(PrefName.ProcFeeUpdatePrompt,comboProcFeeUpdatePrompt.SelectedIndex)
				//| Prefs.UpdateBool(PrefName.ToothChartMoveMenuToRight,checkToothChartMoveMenuToRight.Checked)
				//| Prefs.UpdateBool(PrefName.ChartQuickAddHideAmalgam, checkChartQuickAddHideAmalgam.Checked) //Deprecated.
				//| Prefs.UpdateBool(PrefName.ChartAddProcNoRefreshGrid,checkChartAddProcNoRefreshGrid.Checked)//Not implemented.  May revisit someday.
				| Prefs.UpdateBool(PrefName.ProcProvChangesClaimProcWithClaim,checkProcProvChangesCp.Checked)
				| Prefs.UpdateBool(PrefName.ElectronicRxClinicUseSelected,checkBoxRxClinicUseSelected.Checked)
				| Prefs.UpdateBool(PrefName.ProcNoteConcurrencyMerge,checkProcNoteConcurrencyMerge.Checked)
				| Prefs.UpdateBool(PrefName.IsAlertRadiologyProcsEnabled,checkIsAlertRadiologyProcsEnabled.Checked)
				| Prefs.UpdateBool(PrefName.ShowPlannedAppointmentPrompt,checkShowPlannedApptPrompt.Checked)
				#endregion
				#region Image Module
				| Prefs.UpdateInt(PrefName.ImagesModuleTreeIsCollapsed,imageModuleIsCollapsedVal)
				#endregion
				#region Manage Module
				| Prefs.UpdateBool(PrefName.RxSendNewToQueue,checkRxSendNewToQueue.Checked)
				| Prefs.UpdateInt(PrefName.TimeCardOvertimeFirstDayOfWeek,comboTimeCardOvertimeFirstDayOfWeek.SelectedIndex)
				| Prefs.UpdateBool(PrefName.TimeCardADPExportIncludesName,checkTimeCardADP.Checked)
				| Prefs.UpdateBool(PrefName.ClaimsSendWindowValidatesOnLoad,checkClaimsSendWindowValidateOnLoad.Checked)
				| Prefs.UpdateBool(PrefName.StatementShowReturnAddress,checkStatementShowReturnAddress.Checked)
				| Prefs.UpdateBool(PrefName.StatementShowCreditCard,checkShowCC.Checked)
				| Prefs.UpdateBool(PrefName.ScheduleProvEmpSelectAll,checkScheduleProvEmpSelectAll.Checked)
				| Prefs.UpdateBool(PrefName.StatementShowNotes,checkStatementShowNotes.Checked)
				| Prefs.UpdateBool(PrefName.StatementShowAdjNotes,checkStatementShowAdjNotes.Checked)
				| Prefs.UpdateBool(PrefName.StatementShowProcBreakdown,checkStatementShowProcBreakdown.Checked)
				| Prefs.UpdateBool(PrefName.StatementAccountsUseChartNumber,comboUseChartNum.SelectedIndex==1)
				| Prefs.UpdateLong(PrefName.PayPlansBillInAdvanceDays,PIn.Long(textPayPlansBillInAdvanceDays.Text))
				| Prefs.UpdateBool(PrefName.IntermingleFamilyDefault,checkIntermingleDefault.Checked)
				| Prefs.UpdateInt(PrefName.BillingElectBatchMax,PIn.Int(textBillingElectBatchMax.Text))
				| Prefs.UpdateBool(PrefName.BillingShowSendProgress,checkBillingShowProgress.Checked)
				| Prefs.UpdateInt(PrefName.ClaimPaymentNoShowZeroDate,(textClaimsReceivedDays.Text=="")?-1:(PIn.Int(textClaimsReceivedDays.Text)-1))
				| Prefs.UpdateBool(PrefName.ClaimPaymentBatchOnly,checkClaimPaymentBatchOnly.Checked)
				| Prefs.UpdateBool(PrefName.EraPrintOneClaimPerPage,checkEraOneClaimPerPage.Checked)
				| Prefs.UpdateBool(PrefName.ShowAutoDeposit,checkShowAutoDeposit.Checked)
			#endregion
				)//end big if statement
			{
				_changed=true;
			}
			if(comboFinanceChargeAdjType.SelectedIndex!=-1) {
				_changed|=Prefs.UpdateLong(PrefName.FinanceChargeAdjustmentType,listPosAdjTypes[comboFinanceChargeAdjType.SelectedIndex].DefNum);
			}
			if(comboBillingChargeAdjType.SelectedIndex!=-1) {
				_changed|=Prefs.UpdateLong(PrefName.BillingChargeAdjustmentType,listPosAdjTypes[comboBillingChargeAdjType.SelectedIndex].DefNum);
			}
			if(comboSalesTaxAdjType.SelectedIndex!=-1) {
				_changed|=Prefs.UpdateLong(PrefName.SalesTaxAdjustmentType,listPosAdjTypes[comboSalesTaxAdjType.SelectedIndex].DefNum);
			}
			if(comboPayPlanAdj.SelectedIndex!=-1) {
				_changed|=Prefs.UpdateLong(PrefName.PayPlanAdjType,listNegAdjTypes[comboPayPlanAdj.SelectedIndex].DefNum);
			}
			if(comboBrokenApptAdjType.SelectedIndex!=-1) {
				_changed|=Prefs.UpdateLong(PrefName.BrokenAppointmentAdjustmentType,listPosAdjTypes[comboBrokenApptAdjType.SelectedIndex].DefNum);
			}
			if(comboProcDiscountType.SelectedIndex!=-1) {
				_changed|=Prefs.UpdateLong(PrefName.TreatPlanDiscountAdjustmentType,listNegAdjTypes[comboProcDiscountType.SelectedIndex].DefNum);
			}
			if(comboUnallocatedSplits.SelectedIndex!=-1) {
				_changed|=Prefs.UpdateLong(PrefName.PrepaymentUnearnedType,_listPaySplitUnearnedType[comboUnallocatedSplits.SelectedIndex].DefNum);
			}
			int prefRigorousAccounting=PrefC.GetInt(PrefName.RigorousAccounting);
			if(Prefs.UpdateInt(PrefName.RigorousAccounting,comboRigorousAccounting.SelectedIndex)) {
				_changed=true;
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Rigorous accounting changed from "+
					((RigorousAccounting)prefRigorousAccounting).GetDescription()+" to "
					+((RigorousAccounting)comboRigorousAccounting.SelectedIndex).GetDescription()+".");
			}
			int prefRigorousAdjustments=PrefC.GetInt(PrefName.RigorousAdjustments);
			if(Prefs.UpdateInt(PrefName.RigorousAdjustments,comboRigorousAdjustments.SelectedIndex)) {
				_changed=true;
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Rigorous adjustments changed from "+
					((RigorousAdjustments)prefRigorousAdjustments).GetDescription()+" to "
					+((RigorousAdjustments)comboRigorousAdjustments.SelectedIndex).GetDescription()+".");
			}
			if(textStatementsCalcDueDate.Text==""){
				if(Prefs.UpdateLong(PrefName.StatementsCalcDueDate,-1)){
					_changed=true;
				}
			}
			else{
				if(Prefs.UpdateLong(PrefName.StatementsCalcDueDate,PIn.Long(textStatementsCalcDueDate.Text))){
					_changed=true;
				}
			}
			foreach(ClaimSnapshotTrigger trigger in Enum.GetValues(typeof(ClaimSnapshotTrigger))) {
				if(trigger.GetDescription()==comboClaimSnapshotTrigger.Text) {
					if(Prefs.UpdateString(PrefName.ClaimSnapshotTriggerType,trigger.ToString())) {
						_changed=true;
					}
					break;
				}
			}
			long timeArrivedTrigger=0;
			if(comboTimeArrived.SelectedIndex>0){
				timeArrivedTrigger=_listApptConfirmedDefs[comboTimeArrived.SelectedIndex-1].DefNum;
			}
			List<string> listTriggerNewNums=new List<string>();
			if(Prefs.UpdateLong(PrefName.AppointmentTimeArrivedTrigger,timeArrivedTrigger)){
				listTriggerNewNums.Add(POut.Long(timeArrivedTrigger));
				_changed=true;
			}
			long timeSeatedTrigger=0;
			if(comboTimeSeated.SelectedIndex>0){
				timeSeatedTrigger=_listApptConfirmedDefs[comboTimeSeated.SelectedIndex-1].DefNum;
			}
			if(Prefs.UpdateLong(PrefName.AppointmentTimeSeatedTrigger,timeSeatedTrigger)){
				listTriggerNewNums.Add(POut.Long(timeSeatedTrigger));
				_changed=true;
			}
			long timeDismissedTrigger=0;
			if(comboTimeDismissed.SelectedIndex>0){
				timeDismissedTrigger=_listApptConfirmedDefs[comboTimeDismissed.SelectedIndex-1].DefNum;
			}
			if(Prefs.UpdateLong(PrefName.AppointmentTimeDismissedTrigger,timeDismissedTrigger)){
				listTriggerNewNums.Add(POut.Long(timeDismissedTrigger));
				_changed=true;
			}
			if(listTriggerNewNums.Count>0) {
				//Adds the appointment triggers to the list of confirmation statuses excluded from sending eConfirms and eReminders.
				List<string> listEConfirm=PrefC.GetString(PrefName.ApptConfirmExcludeEConfirm).Split(',')
					.Where(x => !string.IsNullOrWhiteSpace(x))
					.Union(listTriggerNewNums).ToList();
				List<string> listESend=PrefC.GetString(PrefName.ApptConfirmExcludeESend).Split(',')
					.Where(x => !string.IsNullOrWhiteSpace(x))
					.Union(listTriggerNewNums).ToList();
				List<string> listERemind=PrefC.GetString(PrefName.ApptConfirmExcludeERemind).Split(',')
					.Where(x => !string.IsNullOrWhiteSpace(x))
					.Union(listTriggerNewNums).ToList();
				//Update new Value strings in database.  We don't remove the old ones.
				Prefs.UpdateString(PrefName.ApptConfirmExcludeEConfirm,string.Join(",",listEConfirm));
				Prefs.UpdateString(PrefName.ApptConfirmExcludeESend,string.Join(",",listESend));
				Prefs.UpdateString(PrefName.ApptConfirmExcludeERemind,string.Join(",",listERemind));
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormModuleSetup_FormClosing(object sender,FormClosingEventArgs e) {
			if(_changed){
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}
	}
}






