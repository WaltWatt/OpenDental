using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Linq;
using System.Text;

namespace OpenDental{
///<summary></summary>
	public class FormMedical : ODForm {
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butAdd;
		private IContainer components;
		private OpenDental.UI.Button butAddDisease;// Required designer variable.
		private Patient PatCur;
		private OpenDental.UI.ODGrid gridMeds;
		///<summary>In-memory-only ODgrid used for printing.</summary>
		private OpenDental.UI.ODGrid gridMedsPrint;
		private OpenDental.UI.ODGrid gridDiseases;
		private List<Disease> DiseaseList;
		private CheckBox checkDiscontinued;
		private ODGrid gridAllergies;
		private UI.Button butAddAllergy;
		private PatientNote PatientNoteCur;
		private CheckBox checkShowInactiveAllergies;
		private List<Allergy> allergyList;
		private UI.Button butPrint;
		private List<MedicationPat> medList;
		private int pagesPrinted;
		private PrintDocument pd;
		private bool headingPrinted;
		private CheckBox checkShowInactiveProblems;
		private ImageList imageListInfoButton;
		private ODGrid gridFamilyHealth;
		private UI.Button butAddFamilyHistory;
		private List<FamilyHealth> ListFamHealth;
		private int headingPrintH;
		private long _EhrMeasureEventNum;
		private TabControl tabControlFormMedical;
		private TabPage tabProblems;
		private TabPage tabMedications;
		private TabPage tabAllergies;
		private TabPage tabFamHealthHist;
		private TabPage tabMedical;
		private Label label4;
		private Label label2;
		private Label label3;
		private Label label1;
		private ODtextBox textMedical;
		private UI.Button butPrintMedical;
		private ODtextBox textService;
		private ODtextBox textMedUrgNote;
		private CheckBox checkPremed;
		private GroupBox groupMedsDocumented;
		private RadioButton radioMedsDocumentedNo;
		private RadioButton radioMedsDocumentedYes;
		private Label label6;
		private ODtextBox textMedicalComp;
		private TabPage tabVitalSigns;
		private UI.Button butAddVitalSign;
		private ODGrid gridVitalSigns;
		private UI.Button butGrowthChart;
		private long _EhrNotPerfNum;
		private TabPage tabTobaccoUse;
		private Label label12;
		private ComboBox comboSmokeStatus;
		private Label label13;
		private List<Vitalsign> _listVitalSigns;
		///<summary>A copy of the original patient object, as it was when this form was first opened.</summary>
		private Patient _patOld;
		///<summary>List of tobacco use screening type codes.  Currently only 3 allowed SNOMED codes as of 2014.</summary>
		private List<EhrCode> _listAssessmentCodes;
		///<summary>All EhrCodes in the tobacco cessation counseling value set (2.16.840.1.113883.3.526.3.509).
		///When comboInterventionType has selected index 0, load the counseling intervention codes into comboInterventionCode.</summary>
		private List<EhrCode> _listCounselInterventionCodes;
		///<summary>All EhrCodes in the tobacco cessation medication value set (2.16.840.1.113883.3.526.3.1190).
		///When comboInterventionType has selected index 1, load the medication intervention codes into comboInterventionCode.</summary>
		private List<EhrCode> _listMedInterventionCodes;
		private List<EhrCode> _listRecentIntvCodes;
		///<summary>This list contains all of the intervention codes in the comboInterventionCode, counsel, medication, both.</summary>
		private List<EhrCode> _listInterventionCodes;
		///<summary>All EhrCodes in the tobacco user value set (2.16.840.1.113883.3.526.3.1170).
		///When radioAll or radioUser is selected, comboTobaccoStatuses will be filled with this list.</summary>
		private List<EhrCode> _listUserCodes;
		///<summary>All EhrCodes in the tobacco non-user value set (2.16.840.1.113883.3.526.3.1189).
		///When radioAll or radioNonUser is selected, comboTobaccoStatuses will be filled with this list.</summary>
		private List<EhrCode> _listNonUserCodes;
		///<summary>List of tobacco statuses selected from the SNOMED list for this pat that aren't in the list of EHR user and non-user codes</summary>
		private List<EhrCode> _listCustomTobaccoCodes;
		///<summary>List of recently used tobacco statuses, ordered by a date used weighted sum of number of times used.  Codes used the most will be
		///first in the list, with more recent EhrMeasureEvents having a heavier weight.</summary>
		private List<EhrCode> _listRecentTobaccoCodes;
		///<summary>This list contains all of the tobacco statuses in the comboTobaccoStatus, user, non-user, or both.  This list may also contain
		///statuses that the user has selected from the SNOMED list that are not a user or non-user code.</summary>
		private List<EhrCode> _listTobaccoStatuses;
		private ToolTip _comboToolTip;
		private TabControl tabControl1;
		private TabPage tabPage1;
		private RadioButton radioRecentStatuses;
		private RadioButton radioUserStatuses;
		private Label label9;
		private ComboBox comboTobaccoStatus;
		private ODGrid gridAssessments;
		private Label labelTobaccoStatus;
		private RadioButton radioAllStatuses;
		private UI.Button butAddAssessment;
		private ComboBox comboAssessmentType;
		private TextBox textDateAssessed;
		private RadioButton radioNonUserStatuses;
		private Label label11;
		private Label label10;
		private TabPage tabPage2;
		private RadioButton radioRecentInterventions;
		private ComboBox comboInterventionCode;
		private CheckBox checkPatientDeclined;
		private Label label8;
		private RadioButton radioAllInterventions;
		private TextBox textDateIntervention;
		private ODGrid gridInterventions;
		private Label label5;
		private RadioButton radioMedInterventions;
		private Label label7;
		private UI.Button butAddIntervention;
		private RadioButton radioCounselInterventions;

		///<summary>Tab name to pre-select when form loads.
		///i.e. "tabMedical", "tabProblems", "tabMedications", "tabAllergies", "tabFamHealthHist", "tabVitalSigns", or "tabTobaccoUse".</summary>
		private string _selectedTab;


		///<summary></summary>
		public FormMedical(PatientNote patientNoteCur,Patient patCur,string selectedTab=""){
			InitializeComponent();// Required for Windows Form Designer support
			PatCur=patCur;
			PatientNoteCur=patientNoteCur;
			_selectedTab=selectedTab;
			Lan.F(this);
		}

		///<summary></summary>
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMedical));
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.butAddDisease = new OpenDental.UI.Button();
			this.gridMeds = new OpenDental.UI.ODGrid();
			this.gridDiseases = new OpenDental.UI.ODGrid();
			this.checkDiscontinued = new System.Windows.Forms.CheckBox();
			this.gridAllergies = new OpenDental.UI.ODGrid();
			this.butAddAllergy = new OpenDental.UI.Button();
			this.checkShowInactiveAllergies = new System.Windows.Forms.CheckBox();
			this.butPrint = new OpenDental.UI.Button();
			this.checkShowInactiveProblems = new System.Windows.Forms.CheckBox();
			this.imageListInfoButton = new System.Windows.Forms.ImageList(this.components);
			this.gridFamilyHealth = new OpenDental.UI.ODGrid();
			this.butAddFamilyHistory = new OpenDental.UI.Button();
			this.tabControlFormMedical = new System.Windows.Forms.TabControl();
			this.tabMedical = new System.Windows.Forms.TabPage();
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textMedical = new OpenDental.ODtextBox();
			this.butPrintMedical = new OpenDental.UI.Button();
			this.textService = new OpenDental.ODtextBox();
			this.textMedUrgNote = new OpenDental.ODtextBox();
			this.checkPremed = new System.Windows.Forms.CheckBox();
			this.groupMedsDocumented = new System.Windows.Forms.GroupBox();
			this.radioMedsDocumentedNo = new System.Windows.Forms.RadioButton();
			this.radioMedsDocumentedYes = new System.Windows.Forms.RadioButton();
			this.label6 = new System.Windows.Forms.Label();
			this.textMedicalComp = new OpenDental.ODtextBox();
			this.tabProblems = new System.Windows.Forms.TabPage();
			this.tabMedications = new System.Windows.Forms.TabPage();
			this.tabAllergies = new System.Windows.Forms.TabPage();
			this.tabFamHealthHist = new System.Windows.Forms.TabPage();
			this.tabVitalSigns = new System.Windows.Forms.TabPage();
			this.butGrowthChart = new OpenDental.UI.Button();
			this.butAddVitalSign = new OpenDental.UI.Button();
			this.gridVitalSigns = new OpenDental.UI.ODGrid();
			this.tabTobaccoUse = new System.Windows.Forms.TabPage();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.radioRecentStatuses = new System.Windows.Forms.RadioButton();
			this.radioUserStatuses = new System.Windows.Forms.RadioButton();
			this.label9 = new System.Windows.Forms.Label();
			this.comboTobaccoStatus = new System.Windows.Forms.ComboBox();
			this.gridAssessments = new OpenDental.UI.ODGrid();
			this.labelTobaccoStatus = new System.Windows.Forms.Label();
			this.radioAllStatuses = new System.Windows.Forms.RadioButton();
			this.butAddAssessment = new OpenDental.UI.Button();
			this.comboAssessmentType = new System.Windows.Forms.ComboBox();
			this.textDateAssessed = new System.Windows.Forms.TextBox();
			this.radioNonUserStatuses = new System.Windows.Forms.RadioButton();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.radioRecentInterventions = new System.Windows.Forms.RadioButton();
			this.comboInterventionCode = new System.Windows.Forms.ComboBox();
			this.checkPatientDeclined = new System.Windows.Forms.CheckBox();
			this.label8 = new System.Windows.Forms.Label();
			this.radioAllInterventions = new System.Windows.Forms.RadioButton();
			this.textDateIntervention = new System.Windows.Forms.TextBox();
			this.gridInterventions = new OpenDental.UI.ODGrid();
			this.label5 = new System.Windows.Forms.Label();
			this.radioMedInterventions = new System.Windows.Forms.RadioButton();
			this.label7 = new System.Windows.Forms.Label();
			this.butAddIntervention = new OpenDental.UI.Button();
			this.radioCounselInterventions = new System.Windows.Forms.RadioButton();
			this.label12 = new System.Windows.Forms.Label();
			this.comboSmokeStatus = new System.Windows.Forms.ComboBox();
			this.label13 = new System.Windows.Forms.Label();
			this.tabControlFormMedical.SuspendLayout();
			this.tabMedical.SuspendLayout();
			this.groupMedsDocumented.SuspendLayout();
			this.tabProblems.SuspendLayout();
			this.tabMedications.SuspendLayout();
			this.tabAllergies.SuspendLayout();
			this.tabFamHealthHist.SuspendLayout();
			this.tabVitalSigns.SuspendLayout();
			this.tabTobaccoUse.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(624, 461);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 25);
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
			this.butCancel.Location = new System.Drawing.Point(717, 461);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 25);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(6, 6);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(123, 23);
			this.butAdd.TabIndex = 51;
			this.butAdd.Text = "&Add Medication";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butAddDisease
			// 
			this.butAddDisease.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butAddDisease.Autosize = true;
			this.butAddDisease.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddDisease.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddDisease.CornerRadius = 4F;
			this.butAddDisease.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddDisease.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddDisease.Location = new System.Drawing.Point(6, 6);
			this.butAddDisease.Name = "butAddDisease";
			this.butAddDisease.Size = new System.Drawing.Size(98, 23);
			this.butAddDisease.TabIndex = 58;
			this.butAddDisease.Text = "Add Problem";
			this.butAddDisease.Click += new System.EventHandler(this.butAddProblem_Click);
			// 
			// gridMeds
			// 
			this.gridMeds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMeds.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMeds.HasAddButton = false;
			this.gridMeds.HasDropDowns = false;
			this.gridMeds.HasMultilineHeaders = false;
			this.gridMeds.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMeds.HeaderHeight = 15;
			this.gridMeds.HScrollVisible = false;
			this.gridMeds.Location = new System.Drawing.Point(6, 35);
			this.gridMeds.Name = "gridMeds";
			this.gridMeds.ScrollValue = 0;
			this.gridMeds.Size = new System.Drawing.Size(771, 386);
			this.gridMeds.TabIndex = 59;
			this.gridMeds.Title = "Medications";
			this.gridMeds.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMeds.TitleHeight = 18;
			this.gridMeds.TranslationName = "TableMedications";
			this.gridMeds.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMeds_CellDoubleClick);
			this.gridMeds.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMeds_CellClick);
			// 
			// gridDiseases
			// 
			this.gridDiseases.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridDiseases.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridDiseases.HasAddButton = false;
			this.gridDiseases.HasDropDowns = false;
			this.gridDiseases.HasMultilineHeaders = false;
			this.gridDiseases.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridDiseases.HeaderHeight = 15;
			this.gridDiseases.HScrollVisible = false;
			this.gridDiseases.Location = new System.Drawing.Point(6, 35);
			this.gridDiseases.Name = "gridDiseases";
			this.gridDiseases.ScrollValue = 0;
			this.gridDiseases.Size = new System.Drawing.Size(771, 386);
			this.gridDiseases.TabIndex = 60;
			this.gridDiseases.Title = "Problems";
			this.gridDiseases.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridDiseases.TitleHeight = 18;
			this.gridDiseases.TranslationName = "TableDiseases";
			this.gridDiseases.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridDiseases_CellDoubleClick);
			this.gridDiseases.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridDiseases_CellClick);
			// 
			// checkDiscontinued
			// 
			this.checkDiscontinued.Location = new System.Drawing.Point(155, 11);
			this.checkDiscontinued.Name = "checkDiscontinued";
			this.checkDiscontinued.Size = new System.Drawing.Size(201, 18);
			this.checkDiscontinued.TabIndex = 61;
			this.checkDiscontinued.Tag = "";
			this.checkDiscontinued.Text = "Show Discontinued Medications";
			this.checkDiscontinued.UseVisualStyleBackColor = true;
			this.checkDiscontinued.KeyUp += new System.Windows.Forms.KeyEventHandler(this.checkDiscontinued_KeyUp);
			this.checkDiscontinued.MouseUp += new System.Windows.Forms.MouseEventHandler(this.checkShowDiscontinuedMeds_MouseUp);
			// 
			// gridAllergies
			// 
			this.gridAllergies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridAllergies.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridAllergies.HasAddButton = false;
			this.gridAllergies.HasDropDowns = false;
			this.gridAllergies.HasMultilineHeaders = false;
			this.gridAllergies.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridAllergies.HeaderHeight = 15;
			this.gridAllergies.HScrollVisible = false;
			this.gridAllergies.Location = new System.Drawing.Point(6, 35);
			this.gridAllergies.Name = "gridAllergies";
			this.gridAllergies.ScrollValue = 0;
			this.gridAllergies.Size = new System.Drawing.Size(771, 386);
			this.gridAllergies.TabIndex = 63;
			this.gridAllergies.Title = "Allergies";
			this.gridAllergies.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridAllergies.TitleHeight = 18;
			this.gridAllergies.TranslationName = "TableDiseases";
			this.gridAllergies.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAllergies_CellDoubleClick);
			this.gridAllergies.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAllergies_CellClick);
			// 
			// butAddAllergy
			// 
			this.butAddAllergy.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butAddAllergy.Autosize = true;
			this.butAddAllergy.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddAllergy.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddAllergy.CornerRadius = 4F;
			this.butAddAllergy.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddAllergy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddAllergy.Location = new System.Drawing.Point(6, 6);
			this.butAddAllergy.Name = "butAddAllergy";
			this.butAddAllergy.Size = new System.Drawing.Size(98, 23);
			this.butAddAllergy.TabIndex = 64;
			this.butAddAllergy.Text = "Add Allergy";
			this.butAddAllergy.Click += new System.EventHandler(this.butAddAllergy_Click);
			// 
			// checkShowInactiveAllergies
			// 
			this.checkShowInactiveAllergies.Location = new System.Drawing.Point(155, 11);
			this.checkShowInactiveAllergies.Name = "checkShowInactiveAllergies";
			this.checkShowInactiveAllergies.Size = new System.Drawing.Size(201, 18);
			this.checkShowInactiveAllergies.TabIndex = 65;
			this.checkShowInactiveAllergies.Tag = "";
			this.checkShowInactiveAllergies.Text = "Show Inactive Allergies";
			this.checkShowInactiveAllergies.UseVisualStyleBackColor = true;
			this.checkShowInactiveAllergies.CheckedChanged += new System.EventHandler(this.checkShowInactiveAllergies_CheckedChanged);
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(491, 6);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(116, 24);
			this.butPrint.TabIndex = 66;
			this.butPrint.Text = "Print Medications";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// checkShowInactiveProblems
			// 
			this.checkShowInactiveProblems.Location = new System.Drawing.Point(155, 11);
			this.checkShowInactiveProblems.Name = "checkShowInactiveProblems";
			this.checkShowInactiveProblems.Size = new System.Drawing.Size(201, 18);
			this.checkShowInactiveProblems.TabIndex = 65;
			this.checkShowInactiveProblems.Tag = "";
			this.checkShowInactiveProblems.Text = "Show Inactive Problems";
			this.checkShowInactiveProblems.UseVisualStyleBackColor = true;
			this.checkShowInactiveProblems.CheckedChanged += new System.EventHandler(this.checkShowInactiveProblems_CheckedChanged);
			// 
			// imageListInfoButton
			// 
			this.imageListInfoButton.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListInfoButton.ImageStream")));
			this.imageListInfoButton.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListInfoButton.Images.SetKeyName(0, "iButton_16px.png");
			// 
			// gridFamilyHealth
			// 
			this.gridFamilyHealth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridFamilyHealth.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridFamilyHealth.HasAddButton = false;
			this.gridFamilyHealth.HasDropDowns = false;
			this.gridFamilyHealth.HasMultilineHeaders = false;
			this.gridFamilyHealth.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridFamilyHealth.HeaderHeight = 15;
			this.gridFamilyHealth.HScrollVisible = false;
			this.gridFamilyHealth.Location = new System.Drawing.Point(6, 35);
			this.gridFamilyHealth.Name = "gridFamilyHealth";
			this.gridFamilyHealth.ScrollValue = 0;
			this.gridFamilyHealth.Size = new System.Drawing.Size(771, 386);
			this.gridFamilyHealth.TabIndex = 69;
			this.gridFamilyHealth.Title = "Family Health History";
			this.gridFamilyHealth.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridFamilyHealth.TitleHeight = 18;
			this.gridFamilyHealth.TranslationName = "TableDiseases";
			this.gridFamilyHealth.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridFamilyHealth_CellDoubleClick);
			// 
			// butAddFamilyHistory
			// 
			this.butAddFamilyHistory.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butAddFamilyHistory.Autosize = true;
			this.butAddFamilyHistory.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddFamilyHistory.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddFamilyHistory.CornerRadius = 4F;
			this.butAddFamilyHistory.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddFamilyHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddFamilyHistory.Location = new System.Drawing.Point(6, 6);
			this.butAddFamilyHistory.Name = "butAddFamilyHistory";
			this.butAddFamilyHistory.Size = new System.Drawing.Size(137, 23);
			this.butAddFamilyHistory.TabIndex = 70;
			this.butAddFamilyHistory.Text = "Add Family History";
			this.butAddFamilyHistory.Click += new System.EventHandler(this.butAddFamilyHistory_Click);
			// 
			// tabControlFormMedical
			// 
			this.tabControlFormMedical.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControlFormMedical.Controls.Add(this.tabMedical);
			this.tabControlFormMedical.Controls.Add(this.tabProblems);
			this.tabControlFormMedical.Controls.Add(this.tabMedications);
			this.tabControlFormMedical.Controls.Add(this.tabAllergies);
			this.tabControlFormMedical.Controls.Add(this.tabFamHealthHist);
			this.tabControlFormMedical.Controls.Add(this.tabVitalSigns);
			this.tabControlFormMedical.Controls.Add(this.tabTobaccoUse);
			this.tabControlFormMedical.Location = new System.Drawing.Point(4, 3);
			this.tabControlFormMedical.Name = "tabControlFormMedical";
			this.tabControlFormMedical.SelectedIndex = 0;
			this.tabControlFormMedical.Size = new System.Drawing.Size(791, 453);
			this.tabControlFormMedical.TabIndex = 73;
			this.tabControlFormMedical.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControlFormMedical_Selecting);
			// 
			// tabMedical
			// 
			this.tabMedical.Controls.Add(this.label4);
			this.tabMedical.Controls.Add(this.label2);
			this.tabMedical.Controls.Add(this.label3);
			this.tabMedical.Controls.Add(this.label1);
			this.tabMedical.Controls.Add(this.textMedical);
			this.tabMedical.Controls.Add(this.butPrintMedical);
			this.tabMedical.Controls.Add(this.textService);
			this.tabMedical.Controls.Add(this.textMedUrgNote);
			this.tabMedical.Controls.Add(this.checkPremed);
			this.tabMedical.Controls.Add(this.groupMedsDocumented);
			this.tabMedical.Controls.Add(this.label6);
			this.tabMedical.Controls.Add(this.textMedicalComp);
			this.tabMedical.Location = new System.Drawing.Point(4, 22);
			this.tabMedical.Name = "tabMedical";
			this.tabMedical.Padding = new System.Windows.Forms.Padding(3);
			this.tabMedical.Size = new System.Drawing.Size(783, 427);
			this.tabMedical.TabIndex = 4;
			this.tabMedical.Text = "Medical Info";
			this.tabMedical.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(6, 128);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(131, 18);
			this.label4.TabIndex = 85;
			this.label4.Text = "Medical Summary";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(7, 34);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(131, 18);
			this.label2.TabIndex = 86;
			this.label2.Text = "Med Urgent";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(288, 34);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(151, 18);
			this.label3.TabIndex = 87;
			this.label3.Text = "Service Notes";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(121, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(390, 18);
			this.label1.TabIndex = 93;
			this.label1.Text = "To print medications, use button in Medications tab.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textMedical
			// 
			this.textMedical.AcceptsTab = true;
			this.textMedical.BackColor = System.Drawing.SystemColors.Window;
			this.textMedical.DetectLinksEnabled = false;
			this.textMedical.DetectUrls = false;
			this.textMedical.Location = new System.Drawing.Point(6, 147);
			this.textMedical.Name = "textMedical";
			this.textMedical.QuickPasteType = OpenDentBusiness.QuickPasteType.MedicalSummary;
			this.textMedical.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textMedical.Size = new System.Drawing.Size(275, 77);
			this.textMedical.TabIndex = 2;
			this.textMedical.Text = "";
			// 
			// butPrintMedical
			// 
			this.butPrintMedical.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrintMedical.Autosize = true;
			this.butPrintMedical.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrintMedical.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrintMedical.CornerRadius = 4F;
			this.butPrintMedical.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPrintMedical.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrintMedical.Location = new System.Drawing.Point(6, 6);
			this.butPrintMedical.Name = "butPrintMedical";
			this.butPrintMedical.Size = new System.Drawing.Size(112, 24);
			this.butPrintMedical.TabIndex = 92;
			this.butPrintMedical.Text = "Print Medical";
			this.butPrintMedical.Click += new System.EventHandler(this.butPrintMedical_Click);
			// 
			// textService
			// 
			this.textService.AcceptsTab = true;
			this.textService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textService.BackColor = System.Drawing.SystemColors.Window;
			this.textService.DetectLinksEnabled = false;
			this.textService.DetectUrls = false;
			this.textService.Location = new System.Drawing.Point(288, 53);
			this.textService.Name = "textService";
			this.textService.QuickPasteType = OpenDentBusiness.QuickPasteType.ServiceNotes;
			this.textService.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textService.Size = new System.Drawing.Size(486, 171);
			this.textService.TabIndex = 3;
			this.textService.Text = "";
			// 
			// textMedUrgNote
			// 
			this.textMedUrgNote.AcceptsTab = true;
			this.textMedUrgNote.BackColor = System.Drawing.SystemColors.Window;
			this.textMedUrgNote.DetectLinksEnabled = false;
			this.textMedUrgNote.DetectUrls = false;
			this.textMedUrgNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textMedUrgNote.ForeColor = System.Drawing.Color.Red;
			this.textMedUrgNote.Location = new System.Drawing.Point(7, 53);
			this.textMedUrgNote.Name = "textMedUrgNote";
			this.textMedUrgNote.QuickPasteType = OpenDentBusiness.QuickPasteType.MedicalUrgent;
			this.textMedUrgNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textMedUrgNote.Size = new System.Drawing.Size(275, 72);
			this.textMedUrgNote.TabIndex = 1;
			this.textMedUrgNote.Text = "";
			// 
			// checkPremed
			// 
			this.checkPremed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkPremed.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPremed.Location = new System.Drawing.Point(401, 29);
			this.checkPremed.Name = "checkPremed";
			this.checkPremed.Size = new System.Drawing.Size(195, 18);
			this.checkPremed.TabIndex = 5;
			this.checkPremed.Text = "Premedicate (PAC or other)";
			this.checkPremed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPremed.UseVisualStyleBackColor = true;
			// 
			// groupMedsDocumented
			// 
			this.groupMedsDocumented.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupMedsDocumented.Controls.Add(this.radioMedsDocumentedNo);
			this.groupMedsDocumented.Controls.Add(this.radioMedsDocumentedYes);
			this.groupMedsDocumented.Location = new System.Drawing.Point(615, 14);
			this.groupMedsDocumented.Name = "groupMedsDocumented";
			this.groupMedsDocumented.Size = new System.Drawing.Size(159, 33);
			this.groupMedsDocumented.TabIndex = 6;
			this.groupMedsDocumented.TabStop = false;
			this.groupMedsDocumented.Text = "Current Meds Documented";
			// 
			// radioMedsDocumentedNo
			// 
			this.radioMedsDocumentedNo.Location = new System.Drawing.Point(93, 13);
			this.radioMedsDocumentedNo.Name = "radioMedsDocumentedNo";
			this.radioMedsDocumentedNo.Size = new System.Drawing.Size(60, 18);
			this.radioMedsDocumentedNo.TabIndex = 1;
			this.radioMedsDocumentedNo.Text = "No";
			this.radioMedsDocumentedNo.UseVisualStyleBackColor = true;
			// 
			// radioMedsDocumentedYes
			// 
			this.radioMedsDocumentedYes.Checked = true;
			this.radioMedsDocumentedYes.Location = new System.Drawing.Point(23, 13);
			this.radioMedsDocumentedYes.Name = "radioMedsDocumentedYes";
			this.radioMedsDocumentedYes.Size = new System.Drawing.Size(66, 18);
			this.radioMedsDocumentedYes.TabIndex = 0;
			this.radioMedsDocumentedYes.TabStop = true;
			this.radioMedsDocumentedYes.Text = "Yes";
			this.radioMedsDocumentedYes.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(9, 227);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(607, 18);
			this.label6.TabIndex = 82;
			this.label6.Text = "Medical History - Complete and Detailed (does not show in chart)";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textMedicalComp
			// 
			this.textMedicalComp.AcceptsTab = true;
			this.textMedicalComp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textMedicalComp.BackColor = System.Drawing.SystemColors.Window;
			this.textMedicalComp.DetectLinksEnabled = false;
			this.textMedicalComp.DetectUrls = false;
			this.textMedicalComp.Location = new System.Drawing.Point(9, 246);
			this.textMedicalComp.Name = "textMedicalComp";
			this.textMedicalComp.QuickPasteType = OpenDentBusiness.QuickPasteType.MedicalHistory;
			this.textMedicalComp.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textMedicalComp.Size = new System.Drawing.Size(765, 170);
			this.textMedicalComp.TabIndex = 4;
			this.textMedicalComp.Text = "";
			// 
			// tabProblems
			// 
			this.tabProblems.Controls.Add(this.gridDiseases);
			this.tabProblems.Controls.Add(this.butAddDisease);
			this.tabProblems.Controls.Add(this.checkShowInactiveProblems);
			this.tabProblems.Location = new System.Drawing.Point(4, 22);
			this.tabProblems.Name = "tabProblems";
			this.tabProblems.Padding = new System.Windows.Forms.Padding(3);
			this.tabProblems.Size = new System.Drawing.Size(783, 427);
			this.tabProblems.TabIndex = 0;
			this.tabProblems.Text = "Problems";
			this.tabProblems.UseVisualStyleBackColor = true;
			// 
			// tabMedications
			// 
			this.tabMedications.Controls.Add(this.butAdd);
			this.tabMedications.Controls.Add(this.gridMeds);
			this.tabMedications.Controls.Add(this.checkDiscontinued);
			this.tabMedications.Controls.Add(this.butPrint);
			this.tabMedications.Location = new System.Drawing.Point(4, 22);
			this.tabMedications.Name = "tabMedications";
			this.tabMedications.Padding = new System.Windows.Forms.Padding(3);
			this.tabMedications.Size = new System.Drawing.Size(783, 427);
			this.tabMedications.TabIndex = 1;
			this.tabMedications.Text = "Medications";
			this.tabMedications.UseVisualStyleBackColor = true;
			// 
			// tabAllergies
			// 
			this.tabAllergies.Controls.Add(this.gridAllergies);
			this.tabAllergies.Controls.Add(this.checkShowInactiveAllergies);
			this.tabAllergies.Controls.Add(this.butAddAllergy);
			this.tabAllergies.Location = new System.Drawing.Point(4, 22);
			this.tabAllergies.Name = "tabAllergies";
			this.tabAllergies.Padding = new System.Windows.Forms.Padding(3);
			this.tabAllergies.Size = new System.Drawing.Size(783, 427);
			this.tabAllergies.TabIndex = 2;
			this.tabAllergies.Text = "Allergies";
			this.tabAllergies.UseVisualStyleBackColor = true;
			// 
			// tabFamHealthHist
			// 
			this.tabFamHealthHist.Controls.Add(this.gridFamilyHealth);
			this.tabFamHealthHist.Controls.Add(this.butAddFamilyHistory);
			this.tabFamHealthHist.Location = new System.Drawing.Point(4, 22);
			this.tabFamHealthHist.Name = "tabFamHealthHist";
			this.tabFamHealthHist.Padding = new System.Windows.Forms.Padding(3);
			this.tabFamHealthHist.Size = new System.Drawing.Size(783, 427);
			this.tabFamHealthHist.TabIndex = 3;
			this.tabFamHealthHist.Text = "Family Health History";
			this.tabFamHealthHist.UseVisualStyleBackColor = true;
			// 
			// tabVitalSigns
			// 
			this.tabVitalSigns.Controls.Add(this.butGrowthChart);
			this.tabVitalSigns.Controls.Add(this.butAddVitalSign);
			this.tabVitalSigns.Controls.Add(this.gridVitalSigns);
			this.tabVitalSigns.Location = new System.Drawing.Point(4, 22);
			this.tabVitalSigns.Name = "tabVitalSigns";
			this.tabVitalSigns.Padding = new System.Windows.Forms.Padding(3);
			this.tabVitalSigns.Size = new System.Drawing.Size(783, 427);
			this.tabVitalSigns.TabIndex = 5;
			this.tabVitalSigns.Text = "Vital Signs";
			this.tabVitalSigns.UseVisualStyleBackColor = true;
			// 
			// butGrowthChart
			// 
			this.butGrowthChart.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butGrowthChart.Autosize = true;
			this.butGrowthChart.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGrowthChart.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGrowthChart.CornerRadius = 4F;
			this.butGrowthChart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butGrowthChart.Location = new System.Drawing.Point(122, 6);
			this.butGrowthChart.Name = "butGrowthChart";
			this.butGrowthChart.Size = new System.Drawing.Size(92, 23);
			this.butGrowthChart.TabIndex = 72;
			this.butGrowthChart.Text = "Growth Chart";
			this.butGrowthChart.Click += new System.EventHandler(this.butGrowthChart_Click);
			// 
			// butAddVitalSign
			// 
			this.butAddVitalSign.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butAddVitalSign.Autosize = true;
			this.butAddVitalSign.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddVitalSign.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddVitalSign.CornerRadius = 4F;
			this.butAddVitalSign.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddVitalSign.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddVitalSign.Location = new System.Drawing.Point(6, 6);
			this.butAddVitalSign.Name = "butAddVitalSign";
			this.butAddVitalSign.Size = new System.Drawing.Size(110, 23);
			this.butAddVitalSign.TabIndex = 71;
			this.butAddVitalSign.Text = "Add Vital Sign";
			this.butAddVitalSign.Click += new System.EventHandler(this.butAddVitalSign_Click);
			// 
			// gridVitalSigns
			// 
			this.gridVitalSigns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridVitalSigns.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridVitalSigns.HasAddButton = false;
			this.gridVitalSigns.HasDropDowns = false;
			this.gridVitalSigns.HasMultilineHeaders = false;
			this.gridVitalSigns.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridVitalSigns.HeaderHeight = 15;
			this.gridVitalSigns.HScrollVisible = false;
			this.gridVitalSigns.Location = new System.Drawing.Point(6, 35);
			this.gridVitalSigns.Name = "gridVitalSigns";
			this.gridVitalSigns.ScrollValue = 0;
			this.gridVitalSigns.Size = new System.Drawing.Size(771, 386);
			this.gridVitalSigns.TabIndex = 4;
			this.gridVitalSigns.Title = "Vital Signs";
			this.gridVitalSigns.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridVitalSigns.TitleHeight = 18;
			this.gridVitalSigns.TranslationName = "TableVitals";
			this.gridVitalSigns.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridVitalSigns_CellDoubleClick);
			// 
			// tabTobaccoUse
			// 
			this.tabTobaccoUse.Controls.Add(this.tabControl1);
			this.tabTobaccoUse.Controls.Add(this.label12);
			this.tabTobaccoUse.Controls.Add(this.comboSmokeStatus);
			this.tabTobaccoUse.Controls.Add(this.label13);
			this.tabTobaccoUse.Location = new System.Drawing.Point(4, 22);
			this.tabTobaccoUse.Name = "tabTobaccoUse";
			this.tabTobaccoUse.Padding = new System.Windows.Forms.Padding(3);
			this.tabTobaccoUse.Size = new System.Drawing.Size(783, 427);
			this.tabTobaccoUse.TabIndex = 0;
			this.tabTobaccoUse.Text = "Tobacco Use";
			this.tabTobaccoUse.UseVisualStyleBackColor = true;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(9, 73);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(768, 348);
			this.tabControl1.TabIndex = 7;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.radioRecentStatuses);
			this.tabPage1.Controls.Add(this.radioUserStatuses);
			this.tabPage1.Controls.Add(this.label9);
			this.tabPage1.Controls.Add(this.comboTobaccoStatus);
			this.tabPage1.Controls.Add(this.gridAssessments);
			this.tabPage1.Controls.Add(this.labelTobaccoStatus);
			this.tabPage1.Controls.Add(this.radioAllStatuses);
			this.tabPage1.Controls.Add(this.butAddAssessment);
			this.tabPage1.Controls.Add(this.comboAssessmentType);
			this.tabPage1.Controls.Add(this.textDateAssessed);
			this.tabPage1.Controls.Add(this.radioNonUserStatuses);
			this.tabPage1.Controls.Add(this.label11);
			this.tabPage1.Controls.Add(this.label10);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(760, 322);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Tobacco Use Assessment";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// radioRecentStatuses
			// 
			this.radioRecentStatuses.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioRecentStatuses.Location = new System.Drawing.Point(297, 62);
			this.radioRecentStatuses.Name = "radioRecentStatuses";
			this.radioRecentStatuses.Size = new System.Drawing.Size(67, 16);
			this.radioRecentStatuses.TabIndex = 6;
			this.radioRecentStatuses.TabStop = true;
			this.radioRecentStatuses.Text = "Frequent";
			this.radioRecentStatuses.CheckedChanged += new System.EventHandler(this.radioTobaccoStatuses_CheckedChanged);
			// 
			// radioUserStatuses
			// 
			this.radioUserStatuses.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioUserStatuses.Location = new System.Drawing.Point(157, 61);
			this.radioUserStatuses.Name = "radioUserStatuses";
			this.radioUserStatuses.Size = new System.Drawing.Size(55, 16);
			this.radioUserStatuses.TabIndex = 4;
			this.radioUserStatuses.TabStop = true;
			this.radioUserStatuses.Text = "User";
			this.radioUserStatuses.CheckedChanged += new System.EventHandler(this.radioTobaccoStatuses_CheckedChanged);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(10, 61);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(93, 16);
			this.label9.TabIndex = 0;
			this.label9.Text = "Filter Statuses By";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboTobaccoStatus
			// 
			this.comboTobaccoStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboTobaccoStatus.DropDownWidth = 325;
			this.comboTobaccoStatus.FormattingEnabled = true;
			this.comboTobaccoStatus.Location = new System.Drawing.Point(104, 83);
			this.comboTobaccoStatus.MaxDropDownItems = 30;
			this.comboTobaccoStatus.Name = "comboTobaccoStatus";
			this.comboTobaccoStatus.Size = new System.Drawing.Size(260, 21);
			this.comboTobaccoStatus.TabIndex = 7;
			this.comboTobaccoStatus.SelectionChangeCommitted += new System.EventHandler(this.comboTobaccoStatus_SelectionChangeCommitted);
			// 
			// gridAssessments
			// 
			this.gridAssessments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridAssessments.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridAssessments.HasAddButton = false;
			this.gridAssessments.HasDropDowns = false;
			this.gridAssessments.HasMultilineHeaders = false;
			this.gridAssessments.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridAssessments.HeaderHeight = 15;
			this.gridAssessments.HScrollVisible = false;
			this.gridAssessments.Location = new System.Drawing.Point(13, 139);
			this.gridAssessments.Name = "gridAssessments";
			this.gridAssessments.ScrollValue = 0;
			this.gridAssessments.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridAssessments.Size = new System.Drawing.Size(730, 177);
			this.gridAssessments.TabIndex = 9;
			this.gridAssessments.Title = "Assessment History";
			this.gridAssessments.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridAssessments.TitleHeight = 18;
			this.gridAssessments.TranslationName = "TableAssessment";
			this.gridAssessments.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAssessments_CellDoubleClick);
			// 
			// labelTobaccoStatus
			// 
			this.labelTobaccoStatus.Location = new System.Drawing.Point(10, 84);
			this.labelTobaccoStatus.Name = "labelTobaccoStatus";
			this.labelTobaccoStatus.Size = new System.Drawing.Size(93, 16);
			this.labelTobaccoStatus.TabIndex = 0;
			this.labelTobaccoStatus.Text = "Tobacco Status";
			this.labelTobaccoStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// radioAllStatuses
			// 
			this.radioAllStatuses.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioAllStatuses.Location = new System.Drawing.Point(104, 61);
			this.radioAllStatuses.Name = "radioAllStatuses";
			this.radioAllStatuses.Size = new System.Drawing.Size(47, 16);
			this.radioAllStatuses.TabIndex = 3;
			this.radioAllStatuses.TabStop = true;
			this.radioAllStatuses.Text = "All";
			this.radioAllStatuses.CheckedChanged += new System.EventHandler(this.radioTobaccoStatuses_CheckedChanged);
			// 
			// butAddAssessment
			// 
			this.butAddAssessment.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddAssessment.Autosize = true;
			this.butAddAssessment.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddAssessment.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddAssessment.CornerRadius = 4F;
			this.butAddAssessment.Location = new System.Drawing.Point(104, 110);
			this.butAddAssessment.Name = "butAddAssessment";
			this.butAddAssessment.Size = new System.Drawing.Size(100, 23);
			this.butAddAssessment.TabIndex = 8;
			this.butAddAssessment.Text = "Add Assessment";
			this.butAddAssessment.Click += new System.EventHandler(this.butAssessed_Click);
			// 
			// comboAssessmentType
			// 
			this.comboAssessmentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAssessmentType.DropDownWidth = 350;
			this.comboAssessmentType.FormattingEnabled = true;
			this.comboAssessmentType.Location = new System.Drawing.Point(104, 34);
			this.comboAssessmentType.MaxDropDownItems = 30;
			this.comboAssessmentType.Name = "comboAssessmentType";
			this.comboAssessmentType.Size = new System.Drawing.Size(260, 21);
			this.comboAssessmentType.TabIndex = 2;
			// 
			// textDateAssessed
			// 
			this.textDateAssessed.Location = new System.Drawing.Point(104, 8);
			this.textDateAssessed.Name = "textDateAssessed";
			this.textDateAssessed.ReadOnly = true;
			this.textDateAssessed.Size = new System.Drawing.Size(140, 20);
			this.textDateAssessed.TabIndex = 1;
			// 
			// radioNonUserStatuses
			// 
			this.radioNonUserStatuses.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioNonUserStatuses.Location = new System.Drawing.Point(217, 61);
			this.radioNonUserStatuses.Name = "radioNonUserStatuses";
			this.radioNonUserStatuses.Size = new System.Drawing.Size(73, 16);
			this.radioNonUserStatuses.TabIndex = 5;
			this.radioNonUserStatuses.TabStop = true;
			this.radioNonUserStatuses.Text = "Non-user";
			this.radioNonUserStatuses.CheckedChanged += new System.EventHandler(this.radioTobaccoStatuses_CheckedChanged);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(10, 9);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(93, 16);
			this.label11.TabIndex = 0;
			this.label11.Text = "Date Assessed";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(10, 35);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(93, 16);
			this.label10.TabIndex = 0;
			this.label10.Text = "Assessment Type";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.radioRecentInterventions);
			this.tabPage2.Controls.Add(this.comboInterventionCode);
			this.tabPage2.Controls.Add(this.checkPatientDeclined);
			this.tabPage2.Controls.Add(this.label8);
			this.tabPage2.Controls.Add(this.radioAllInterventions);
			this.tabPage2.Controls.Add(this.textDateIntervention);
			this.tabPage2.Controls.Add(this.gridInterventions);
			this.tabPage2.Controls.Add(this.label5);
			this.tabPage2.Controls.Add(this.radioMedInterventions);
			this.tabPage2.Controls.Add(this.label7);
			this.tabPage2.Controls.Add(this.butAddIntervention);
			this.tabPage2.Controls.Add(this.radioCounselInterventions);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(760, 322);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Cessation Intervention";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// radioRecentInterventions
			// 
			this.radioRecentInterventions.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioRecentInterventions.Location = new System.Drawing.Point(297, 37);
			this.radioRecentInterventions.Name = "radioRecentInterventions";
			this.radioRecentInterventions.Size = new System.Drawing.Size(67, 16);
			this.radioRecentInterventions.TabIndex = 5;
			this.radioRecentInterventions.TabStop = true;
			this.radioRecentInterventions.Text = "Frequent";
			this.radioRecentInterventions.CheckedChanged += new System.EventHandler(this.radioInterventions_CheckedChanged);
			// 
			// comboInterventionCode
			// 
			this.comboInterventionCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboInterventionCode.DropDownWidth = 340;
			this.comboInterventionCode.FormattingEnabled = true;
			this.comboInterventionCode.Location = new System.Drawing.Point(104, 59);
			this.comboInterventionCode.MaxDropDownItems = 30;
			this.comboInterventionCode.Name = "comboInterventionCode";
			this.comboInterventionCode.Size = new System.Drawing.Size(260, 21);
			this.comboInterventionCode.TabIndex = 6;
			this.comboInterventionCode.SelectionChangeCommitted += new System.EventHandler(this.comboInterventionCode_SelectionChangeCommitted);
			// 
			// checkPatientDeclined
			// 
			this.checkPatientDeclined.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkPatientDeclined.Location = new System.Drawing.Point(104, 86);
			this.checkPatientDeclined.Name = "checkPatientDeclined";
			this.checkPatientDeclined.Size = new System.Drawing.Size(154, 18);
			this.checkPatientDeclined.TabIndex = 7;
			this.checkPatientDeclined.Text = "Patient Declined";
			this.checkPatientDeclined.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(10, 60);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(93, 16);
			this.label8.TabIndex = 0;
			this.label8.Text = "Intervention Code";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// radioAllInterventions
			// 
			this.radioAllInterventions.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioAllInterventions.Location = new System.Drawing.Point(104, 37);
			this.radioAllInterventions.Name = "radioAllInterventions";
			this.radioAllInterventions.Size = new System.Drawing.Size(47, 16);
			this.radioAllInterventions.TabIndex = 2;
			this.radioAllInterventions.TabStop = true;
			this.radioAllInterventions.Text = "All";
			this.radioAllInterventions.CheckedChanged += new System.EventHandler(this.radioInterventions_CheckedChanged);
			// 
			// textDateIntervention
			// 
			this.textDateIntervention.Location = new System.Drawing.Point(104, 8);
			this.textDateIntervention.Name = "textDateIntervention";
			this.textDateIntervention.ReadOnly = true;
			this.textDateIntervention.Size = new System.Drawing.Size(140, 20);
			this.textDateIntervention.TabIndex = 1;
			// 
			// gridInterventions
			// 
			this.gridInterventions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridInterventions.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridInterventions.HasAddButton = false;
			this.gridInterventions.HasDropDowns = false;
			this.gridInterventions.HasMultilineHeaders = false;
			this.gridInterventions.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridInterventions.HeaderHeight = 15;
			this.gridInterventions.HScrollVisible = false;
			this.gridInterventions.Location = new System.Drawing.Point(13, 139);
			this.gridInterventions.Name = "gridInterventions";
			this.gridInterventions.ScrollValue = 0;
			this.gridInterventions.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridInterventions.Size = new System.Drawing.Size(730, 177);
			this.gridInterventions.TabIndex = 9;
			this.gridInterventions.Title = "Intervention History";
			this.gridInterventions.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridInterventions.TitleHeight = 18;
			this.gridInterventions.TranslationName = "TableIntervention";
			this.gridInterventions.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridInterventions_CellDoubleClick);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(10, 9);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(93, 16);
			this.label5.TabIndex = 0;
			this.label5.Text = "Date Intervened";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// radioMedInterventions
			// 
			this.radioMedInterventions.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioMedInterventions.Location = new System.Drawing.Point(157, 37);
			this.radioMedInterventions.Name = "radioMedInterventions";
			this.radioMedInterventions.Size = new System.Drawing.Size(55, 16);
			this.radioMedInterventions.TabIndex = 3;
			this.radioMedInterventions.TabStop = true;
			this.radioMedInterventions.Text = "Med";
			this.radioMedInterventions.CheckedChanged += new System.EventHandler(this.radioInterventions_CheckedChanged);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(10, 37);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(93, 16);
			this.label7.TabIndex = 0;
			this.label7.Text = "Filter Codes By";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butAddIntervention
			// 
			this.butAddIntervention.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddIntervention.Autosize = true;
			this.butAddIntervention.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddIntervention.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddIntervention.CornerRadius = 4F;
			this.butAddIntervention.Location = new System.Drawing.Point(104, 110);
			this.butAddIntervention.Name = "butAddIntervention";
			this.butAddIntervention.Size = new System.Drawing.Size(100, 23);
			this.butAddIntervention.TabIndex = 8;
			this.butAddIntervention.Text = "Add Intervention";
			this.butAddIntervention.Click += new System.EventHandler(this.butIntervention_Click);
			// 
			// radioCounselInterventions
			// 
			this.radioCounselInterventions.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioCounselInterventions.Location = new System.Drawing.Point(217, 37);
			this.radioCounselInterventions.Name = "radioCounselInterventions";
			this.radioCounselInterventions.Size = new System.Drawing.Size(73, 16);
			this.radioCounselInterventions.TabIndex = 4;
			this.radioCounselInterventions.TabStop = true;
			this.radioCounselInterventions.Text = "Counsel";
			this.radioCounselInterventions.CheckedChanged += new System.EventHandler(this.radioInterventions_CheckedChanged);
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(378, 27);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(383, 16);
			this.label12.TabIndex = 4;
			this.label12.Text = "Used for calculating MU measures.";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboSmokeStatus
			// 
			this.comboSmokeStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSmokeStatus.FormattingEnabled = true;
			this.comboSmokeStatus.Location = new System.Drawing.Point(147, 26);
			this.comboSmokeStatus.MaxDropDownItems = 30;
			this.comboSmokeStatus.Name = "comboSmokeStatus";
			this.comboSmokeStatus.Size = new System.Drawing.Size(225, 21);
			this.comboSmokeStatus.TabIndex = 6;
			this.comboSmokeStatus.SelectionChangeCommitted += new System.EventHandler(this.comboSmokeStatus_SelectionChangeCommitted);
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(6, 27);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(135, 16);
			this.label13.TabIndex = 5;
			this.label13.Text = "Current Smoking Status";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormMedical
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(802, 494);
			this.Controls.Add(this.tabControlFormMedical);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(818, 533);
			this.Name = "FormMedical";
			this.ShowInTaskbar = false;
			this.Text = "Medical";
			this.Load += new System.EventHandler(this.FormMedical_Load);
			this.ResizeEnd += new System.EventHandler(this.FormMedical_ResizeEnd);
			this.tabControlFormMedical.ResumeLayout(false);
			this.tabMedical.ResumeLayout(false);
			this.groupMedsDocumented.ResumeLayout(false);
			this.tabProblems.ResumeLayout(false);
			this.tabMedications.ResumeLayout(false);
			this.tabAllergies.ResumeLayout(false);
			this.tabFamHealthHist.ResumeLayout(false);
			this.tabVitalSigns.ResumeLayout(false);
			this.tabTobaccoUse.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormMedical_Load(object sender, System.EventArgs e){
			SecurityLogs.MakeLogEntry(Permissions.MedicalInfoViewed,PatCur.PatNum,"Patient medical information viewed");
			_patOld=PatCur.Copy();
			checkPremed.Checked=PatCur.Premed;
			textMedUrgNote.Text=PatCur.MedUrgNote;
			textMedical.Text=PatientNoteCur.Medical;
			textMedicalComp.Text=PatientNoteCur.MedicalComp;
			textService.Text=PatientNoteCur.Service;
			FillMeds();
			FillProblems();
			FillAllergies();
			FillVitalSigns();
			if(PrefC.GetBool(PrefName.ShowFeatureEhr)) {
				FillFamilyHealth();
				TobaccoUseTabLoad();
			}
			else {
				//remove EHR only tabs if ShowFeatureEHR is not enabled.
				tabControlFormMedical.TabPages.RemoveByKey("tabFamHealthHist");
				tabControlFormMedical.TabPages.RemoveByKey("tabTobaccoUse");
			}
			List<EhrMeasureEvent> listDocumentedMedEvents=EhrMeasureEvents.RefreshByType(PatCur.PatNum,EhrMeasureEventType.CurrentMedsDocumented);
			_EhrMeasureEventNum=0;
			for(int i=0;i<listDocumentedMedEvents.Count;i++) {
				if(listDocumentedMedEvents[i].DateTEvent.Date==DateTime.Today) {
					radioMedsDocumentedYes.Checked=true;
					_EhrMeasureEventNum=listDocumentedMedEvents[i].EhrMeasureEventNum;
					break;
				}
			}
			_EhrNotPerfNum=0;
			List<EhrNotPerformed> listNotPerfs=EhrNotPerformeds.Refresh(PatCur.PatNum);
			for(int i=0;i<listNotPerfs.Count;i++) {
				if(listNotPerfs[i].CodeValue!="428191000124101") {//this is the only allowed code for Current Meds Documented procedure
					continue;
				}
				if(listNotPerfs[i].DateEntry.Date==DateTime.Today) {
					radioMedsDocumentedNo.Checked=!radioMedsDocumentedYes.Checked;//only check the No radio button if the Yes radio button is not already set
					_EhrNotPerfNum=listNotPerfs[i].EhrNotPerformedNum;
					break;
				}
			}
			//_selectedTab is set and tab wasn't removed from TabPages, i.e. EHR show feature enabled
			if(_selectedTab!="" && tabControlFormMedical.TabPages.ContainsKey(_selectedTab)) {
				//If tab is disabled, i.e. tabTobaccoUse disabled due to LOINC table missing, tabControlFormMedical_Selecting event handler will cancel
				tabControlFormMedical.SelectTab(_selectedTab);
			}
		}

		#region Medications Tab
		private void FillMeds(bool isForPrinting = false) {
			ODGrid gridToFill=isForPrinting?gridMedsPrint:gridMeds;
			Medications.RefreshCache();
			medList=MedicationPats.Refresh(PatCur.PatNum,checkDiscontinued.Checked);
			gridToFill.BeginUpdate();
			gridToFill.Columns.Clear();
			ODGridColumn col;
			if(CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowInfobutton && !isForPrinting) {//Security.IsAuthorized(Permissions.EhrInfoButton,true)) {
				col=new ODGridColumn("",18);//infoButton
				col.ImageList=imageListInfoButton;
				gridToFill.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g("TableMedications","Medication"),200);
			gridToFill.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableMedications","Notes"),-1);//-1 width forces ODGrid to dynamically size this column
			gridToFill.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableMedications","Notes for Patient"),-1);//-1 width forces ODGrid to dynamically size this column
			gridToFill.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableMedications","Status"),45,HorizontalAlignment.Center);
			gridToFill.Columns.Add(col);
			if(!isForPrinting) {
				col=new ODGridColumn(Lan.g("TableMedications","Source"),60);
				gridToFill.Columns.Add(col);
			}
			gridToFill.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<medList.Count;i++) {
				row=new ODGridRow();
				if(CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowInfobutton && !isForPrinting) {//Security.IsAuthorized(Permissions.EhrInfoButton,true)) {
					row.Cells.Add("0");//index of infobutton
				}
				if(medList[i].MedicationNum==0) {
					row.Cells.Add(medList[i].MedDescript);
					row.Cells.Add("");//generic notes
				}
				else {
					Medication generic=Medications.GetGeneric(medList[i].MedicationNum);
					string medName=Medications.GetMedication(medList[i].MedicationNum).MedName;
					if(generic.MedicationNum!=medList[i].MedicationNum) {//not generic
						medName+=" ("+generic.MedName+")";
					}
					row.Cells.Add(medName);
					row.Cells.Add(Medications.GetGeneric(medList[i].MedicationNum).Notes);
				}
				row.Cells.Add(medList[i].PatNote);
				if(MedicationPats.IsMedActive(medList[i])) {
					row.Cells.Add("Active");
				}
				else {
					row.Cells.Add("Inactive");
				}
				if(!isForPrinting) {
					if(Erx.IsFromNewCrop(medList[i].ErxGuid)) {
						row.Cells.Add("Legacy");
					}
					else if(Erx.IsFromDoseSpot(medList[i].ErxGuid) || Erx.IsDoseSpotPatReported(medList[i].ErxGuid)) {
						row.Cells.Add("DoseSpot");
					}
					else {
						row.Cells.Add("");
					}
				}
				gridToFill.Rows.Add(row);
			}
			gridToFill.EndUpdate();
		}

		private void gridMeds_CellClick(object sender,ODGridClickEventArgs e) {
			if(!CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowInfobutton) {//Security.IsAuthorized(Permissions.EhrInfoButton,true)) {
				return;
			}
			if(e.Col!=0) {
				return;
			}
			List<KnowledgeRequest> listKnowledgeRequests;
			MedicationPat medPat=medList[e.Row];
			if(medPat.MedicationNum==0) {//Medication orders returned from NewCrop
				listKnowledgeRequests=new List<KnowledgeRequest> {
					new KnowledgeRequest {
						Type="Medication",
						Code=POut.Long(medPat.RxCui),
						CodeSystem=CodeSyst.RxNorm,
						Description=medPat.MedDescript
					}
				};
			}
			else {
				listKnowledgeRequests=EhrTriggers.ConvertToKnowledgeRequests(medList[e.Row]);				
			}
			FormInfobutton FormIB=new FormInfobutton(listKnowledgeRequests);
			FormIB.PatCur=PatCur;
			//FormInfoButton allows MedicationCur to be null, so this will still work for medication orders returned from NewCrop (because MedicationNum will be 0).
			FormIB.ShowDialog();
			//Nothing to do with Dialog Result yet.
		}

		private void gridMeds_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormMedPat FormMP=new FormMedPat();
			FormMP.MedicationPatCur=medList[e.Row];
			FormMP.ShowDialog();
			if(FormMP.DialogResult==DialogResult.OK 
				&& FormMP.MedicationPatCur!=null //Can get be null if the user removed the medication from the patient.
				&& CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowCDS 
				&& CDSPermissions.GetForUser(Security.CurUser.UserNum).MedicationCDS) 
			{
				FormCDSIntervention FormCDSI=new FormCDSIntervention();
				if(FormMP.MedicationPatCur.MedicationNum > 0) {//0 indicats the med is from NewCrop.
					Medication medication=Medications.GetMedication(FormMP.MedicationPatCur.MedicationNum);
					if(medication!=null) {
						FormCDSI.ListCDSI=EhrTriggers.TriggerMatch(medication,PatCur);
						FormCDSI.ShowIfRequired(false);
					}
				}
				else if(FormMP.MedicationPatCur.RxCui > 0) {//Meds from NewCrop might have a valid RxNorm.
					RxNorm rxNorm=RxNorms.GetByRxCUI(FormMP.MedicationPatCur.RxCui.ToString());
					if(rxNorm!=null) {
						FormCDSI.ListCDSI=EhrTriggers.TriggerMatch(rxNorm,PatCur);
						FormCDSI.ShowIfRequired(false);
					}
				}
			}
			FillMeds();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			//select medication from list.  Additional meds can be added to the list from within that dlg
			FormMedications FormM=new FormMedications();
			FormM.IsSelectionMode=true;
			FormM.ShowDialog();
			if(FormM.DialogResult!=DialogResult.OK){
				return;
			} 
			if(CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowCDS && CDSPermissions.GetForUser(Security.CurUser.UserNum).MedicationCDS) {
				FormCDSIntervention FormCDSI=new FormCDSIntervention();
				FormCDSI.ListCDSI=EhrTriggers.TriggerMatch(Medications.GetMedication(FormM.SelectedMedicationNum),PatCur);
				FormCDSI.ShowIfRequired();
				if(FormCDSI.DialogResult==DialogResult.Abort) {
					return;//do not add medication
				}
			}
			MedicationPat MedicationPatCur=new MedicationPat();
			MedicationPatCur.PatNum=PatCur.PatNum;
			MedicationPatCur.MedicationNum=FormM.SelectedMedicationNum;
			MedicationPatCur.RxCui=Medications.GetMedication(FormM.SelectedMedicationNum).RxCui;
			MedicationPatCur.ProvNum=PatCur.PriProv;
			FormMedPat FormMP=new FormMedPat();
			FormMP.MedicationPatCur=MedicationPatCur;
			FormMP.IsNew=true;
			FormMP.ShowDialog();
			if(FormMP.DialogResult!=DialogResult.OK){
				return;
			}
			FillMeds();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			gridMedsPrint=new ODGrid() { Width=800,TranslationName=""};
			FillMeds(isForPrinting:true);//not nessecary to explicity name parameter but makes code easier to read.
			pagesPrinted=0;
			pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			//pd.OriginAtMargins=true;
			//pd.DefaultPageSettings.Landscape=true;
			if(pd.DefaultPageSettings.PrintableArea.Height==0) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			headingPrinted=false;
			try {
#if DEBUG
        FormRpPrintPreview pView = new FormRpPrintPreview();
        pView.printPreviewControl2.Document=pd;
		pView.TopMost=true;
        pView.ShowDialog();
#else
					if(PrinterL.SetPrinter(pd,PrintSituation.Default,PatCur.PatNum,"Medications printed")) {
						pd.Print();
					}
#endif
			}
			catch {
				MessageBox.Show(Lan.g(this,"Printer not available"));
			}
			Cursor=Cursors.Default;
		}

		private void pd_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
			//new Rectangle(50,40,800,1035);//Some printers can handle up to 1042
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			if(!headingPrinted) {
				text=Lan.g(this,"Medications List For ")+PatCur.FName+" "+PatCur.LName;
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				text=Lan.g(this,"Created ")+DateTime.Now.ToString();
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=20;
				headingPrinted=true;
				headingPrintH=yPos;
			}
			#endregion
			yPos=gridMedsPrint.PrintPage(g,pagesPrinted,bounds,headingPrintH);
			pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
			}
			g.Dispose();
		}

		private void checkShowDiscontinuedMeds_MouseUp(object sender,MouseEventArgs e) {
			FillMeds();
		}

		private void checkDiscontinued_KeyUp(object sender,KeyEventArgs e) {
			FillMeds();
		}

		private void butMedicationReconcile_Click(object sender,EventArgs e) {
			FormMedicationReconcile FormMR=new FormMedicationReconcile();
			FormMR.PatCur=PatCur;
			FormMR.ShowDialog();
			FillMeds();
		}
		#endregion Medications Tab

		#region Medical Info Tab
		/// <summary>This report is a brute force, one page medical history report. It is not designed to handle more than one page. It does not print service notes or medications.</summary>
		private void butPrintMedical_Click(object sender,EventArgs e) {
			pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPageMedical);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			pd.OriginAtMargins=true;
			if(pd.DefaultPageSettings.PrintableArea.Height==0) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			try {
#if DEBUG
        FormRpPrintPreview pView = new FormRpPrintPreview();
        pView.printPreviewControl2.Document=pd;
        pView.ShowDialog();
#else
					if(PrinterL.SetPrinter(pd,PrintSituation.Default,PatCur.PatNum,"Medical information printed")) {
						pd.Print();
					}
#endif
			}
			catch {
				MessageBox.Show(Lan.g(this,"Printer not available"));
			}
		}

		private void pd_PrintPageMedical(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			Font bodyFont=new Font(FontFamily.GenericSansSerif,10);
			StringFormat format=new StringFormat();
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			int textHeight;
			RectangleF textRect;
			text=Lan.g(this,"Medical History For ")+PatCur.FName+" "+PatCur.LName;
			g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
			yPos+=(int)g.MeasureString(text,headingFont).Height;
			text=Lan.g(this,"Birthdate: ")+PatCur.Birthdate.ToShortDateString();
			g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
			yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
			text=Lan.g(this,"Created ")+DateTime.Now.ToString();
			g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
			yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
			yPos+=25;
			if(gridDiseases.Rows.Count>0) {
				text=Lan.g(this,"Problems");
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				yPos+=2;
				yPos=gridDiseases.PrintPage(g,0,bounds,yPos);
				yPos+=25;
			}
			if(gridAllergies.Rows.Count>0) {
				text=Lan.g(this,"Allergies");
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				yPos+=2;
				yPos=gridAllergies.PrintPage(g,0,bounds,yPos);
				yPos+=25;
			}
			text=Lan.g(this,"Premedicate (PAC or other): ")+(checkPremed.Checked?"Y":"N");
			textHeight=(int)g.MeasureString(text,bodyFont,bounds.Width).Height;
			textRect=new Rectangle(bounds.Left,yPos,bounds.Width,textHeight);
			g.DrawString(text,subHeadingFont,Brushes.Black,textRect);
			yPos+=textHeight;
			yPos+=10;
			text=Lan.g(this,"Medical Urgent Note");
			g.DrawString(text,subHeadingFont,Brushes.Black,bounds.Left,yPos);
			yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
			text=textMedUrgNote.Text;
			textHeight=(int)g.MeasureString(text,bodyFont,bounds.Width).Height;
			textRect=new Rectangle(bounds.Left,yPos,bounds.Width,textHeight);
			g.DrawString(text,bodyFont,Brushes.Black,textRect);//maybe red?
			yPos+=textHeight;
			yPos+=10;
			text=Lan.g(this,"Medical Summary");
			g.DrawString(text,subHeadingFont,Brushes.Black,bounds.Left,yPos);
			yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
			text=textMedical.Text;
			textHeight=(int)g.MeasureString(text,bodyFont,bounds.Width).Height;
			textRect=new Rectangle(bounds.Left,yPos,bounds.Width,textHeight);
			g.DrawString(text,bodyFont,Brushes.Black,textRect);
			yPos+=textHeight;
			yPos+=10;
			text=Lan.g(this,"Medical History - Complete and Detailed");
			g.DrawString(text,subHeadingFont,Brushes.Black,bounds.Left,yPos);
			yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
			text=textMedicalComp.Text;
			textHeight=(int)g.MeasureString(text,bodyFont,bounds.Width).Height;
			textRect=new Rectangle(bounds.Left,yPos,bounds.Width,textHeight);
			g.DrawString(text,bodyFont,Brushes.Black,textRect);
			yPos+=textHeight;
			g.Dispose();
		}
		#endregion Medical Info Tab

		#region Family Health History Tab
		private void FillFamilyHealth() {
			ListFamHealth=FamilyHealths.GetFamilyHealthForPat(PatCur.PatNum);
			gridFamilyHealth.BeginUpdate();
			gridFamilyHealth.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableFamilyHealth","Relationship"),150,HorizontalAlignment.Center);
			gridFamilyHealth.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableFamilyHealth","Name"),150);
			gridFamilyHealth.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableFamilyHealth","Problem"),180);
			gridFamilyHealth.Columns.Add(col);
			gridFamilyHealth.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<ListFamHealth.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(Lan.g("enumFamilyRelationship",ListFamHealth[i].Relationship.ToString()));
				row.Cells.Add(ListFamHealth[i].PersonName);
				row.Cells.Add(DiseaseDefs.GetName(ListFamHealth[i].DiseaseDefNum));
				gridFamilyHealth.Rows.Add(row);
			}
			gridFamilyHealth.EndUpdate();
		}

		private void gridFamilyHealth_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormFamilyHealthEdit FormFHE=new FormFamilyHealthEdit();
			FormFHE.FamilyHealthCur=ListFamHealth[e.Row];
			FormFHE.ShowDialog();
			FillFamilyHealth();
		}

		private void butAddFamilyHistory_Click(object sender,EventArgs e) {
			FormFamilyHealthEdit FormFHE=new FormFamilyHealthEdit();
			FamilyHealth famH=new FamilyHealth();
			famH.PatNum=PatCur.PatNum;
			famH.IsNew=true;
			FormFHE.FamilyHealthCur=famH;
			FormFHE.ShowDialog();
			if(FormFHE.DialogResult!=DialogResult.OK) {
				return;
			}
			FillFamilyHealth();
		}
		#endregion Family Health History Tab

		#region Problems Tab
		private void FillProblems(){
			DiseaseList=Diseases.Refresh(checkShowInactiveProblems.Checked,PatCur.PatNum);
			gridDiseases.BeginUpdate();
			gridDiseases.Columns.Clear();
			ODGridColumn col;
			if(CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowInfobutton) {//Security.IsAuthorized(Permissions.EhrInfoButton,true)) {
				col=new ODGridColumn("",18);//infoButton
				col.ImageList=imageListInfoButton;
				gridDiseases.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g("TableDiseases","Name"),200);//total is about 325
			gridDiseases.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDiseases","Patient Note"),450);
			gridDiseases.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableDisease","Status"),40,HorizontalAlignment.Center);
			gridDiseases.Columns.Add(col);
			gridDiseases.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<DiseaseList.Count;i++){
				row=new ODGridRow();
				if(CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowInfobutton) {//Security.IsAuthorized(Permissions.EhrInfoButton,true)) {
					row.Cells.Add("0");//index of infobutton
				}
				if(DiseaseList[i].DiseaseDefNum!=0) {
					row.Cells.Add(DiseaseDefs.GetName(DiseaseList[i].DiseaseDefNum));
				}
				else {
					row.Cells.Add(DiseaseDefs.GetName(DiseaseList[i].DiseaseDefNum));
				}
				row.Cells.Add(DiseaseList[i].PatNote);
				row.Cells.Add(DiseaseList[i].ProbStatus.ToString());
				gridDiseases.Rows.Add(row);
			}
			gridDiseases.EndUpdate();
		}

		private void butAddProblem_Click(object sender,EventArgs e) {
			//get the list of disease def nums that should be highlighted in FormDiseaseDefs.
			List<long> listDiseaseDefNums=DiseaseList.Where(x => x.ProbStatus == ProblemStatus.Active).ToList().Select(x => x.DiseaseDefNum).ToList();
			FormDiseaseDefs FormDD=new FormDiseaseDefs(listDiseaseDefNums);
			FormDD.IsSelectionMode=true;
			FormDD.IsMultiSelect=true;
			FormDD.ListSelectedDiseaseDefs=new List<DiseaseDef>();

			FormDD.ShowDialog();
			if(FormDD.DialogResult!=DialogResult.OK) {
				return;
			}
			for(int i=0;i<FormDD.ListSelectedDiseaseDefs.Count;i++) {
				Disease disease=new Disease();
				disease.PatNum=PatCur.PatNum;
				disease.DiseaseDefNum=FormDD.ListSelectedDiseaseDefs[i].DiseaseDefNum;
				Diseases.Insert(disease);
				if(CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowCDS && CDSPermissions.GetForUser(Security.CurUser.UserNum).ProblemCDS){
					FormCDSIntervention FormCDSI=new FormCDSIntervention();
					FormCDSI.ListCDSI=EhrTriggers.TriggerMatch(FormDD.ListSelectedDiseaseDefs[i],PatCur);
					FormCDSI.ShowIfRequired();
					if(FormCDSI.DialogResult==DialogResult.Abort) {
						Diseases.Delete(disease);
						continue;//cancel 
					}
				}
				SecurityLogs.MakeLogEntry(Permissions.PatProblemListEdit,PatCur.PatNum,FormDD.ListSelectedDiseaseDefs[i].DiseaseName+" added"); //Audit log made outside form because the form is just a list of problems and is called from many places.
			}
			FillProblems();
		}

		/*private void butIcd9_Click(object sender,EventArgs e) {
			FormIcd9s formI=new FormIcd9s();
			formI.IsSelectionMode=true;
			formI.ShowDialog();
			if(formI.DialogResult!=DialogResult.OK) {
				return;
			}
			Disease disease=new Disease();
			disease.PatNum=PatCur.PatNum;
			disease.ICD9Num=formI.SelectedIcd9Num;
			Diseases.Insert(disease);
			FillProblems();
		}*/

		private void gridDiseases_CellClick(object sender,ODGridClickEventArgs e) {
			if(!CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowInfobutton) {//Security.IsAuthorized(Permissions.EhrInfoButton,true)) {
				return;
			}
			if(e.Col!=0) {
				return;
			}
			List<KnowledgeRequest> listKnowledgeRequests=EhrTriggers.ConvertToKnowledgeRequests(DiseaseDefs.GetItem(DiseaseList[e.Row].DiseaseDefNum));
			FormInfobutton FormIB=new FormInfobutton(listKnowledgeRequests);
			FormIB.PatCur=PatCur;
			FormIB.ShowDialog();
			//Nothing to do with Dialog Result yet.
		}

		private void gridDiseases_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormDiseaseEdit FormD=new FormDiseaseEdit(DiseaseList[e.Row]);
			FormD.ShowDialog();
			if(FormD.DialogResult==DialogResult.OK 
				&& CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowCDS 
				&& CDSPermissions.GetForUser(Security.CurUser.UserNum).ProblemCDS) 
			{
				FormCDSIntervention FormCDSI=new FormCDSIntervention();
				FormCDSI.ListCDSI=EhrTriggers.TriggerMatch(DiseaseDefs.GetItem(DiseaseList[e.Row].DiseaseDefNum),PatCur);
				FormCDSI.ShowIfRequired(false);
			}
			FillProblems();
		}

		private void checkShowInactiveProblems_CheckedChanged(object sender,EventArgs e) {
			FillProblems();
		}
		#endregion Problems Tab

		#region Allergies Tab
		private void FillAllergies() {
			allergyList=Allergies.GetAll(PatCur.PatNum,checkShowInactiveAllergies.Checked);
			gridAllergies.BeginUpdate();
			gridAllergies.Columns.Clear();
			ODGridColumn col;
			if(CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowInfobutton) {//Security.IsAuthorized(Permissions.EhrInfoButton,true)) {
				col=new ODGridColumn("",18);//infoButton
				col.ImageList=imageListInfoButton;
				gridAllergies.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g("TableAllergies","Allergy"),150);
			gridAllergies.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableAllergies","Reaction"),500);
			gridAllergies.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableAllergies","Status"),40,HorizontalAlignment.Center);
			gridAllergies.Columns.Add(col);
			gridAllergies.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<allergyList.Count;i++){
				row=new ODGridRow();
				if(CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowInfobutton) {//Security.IsAuthorized(Permissions.EhrInfoButton,true)) {
					row.Cells.Add("0");//index of infobutton
				}
				AllergyDef allergyDef=AllergyDefs.GetOne(allergyList[i].AllergyDefNum);
				row.Cells.Add(allergyDef.Description);
				if(allergyList[i].DateAdverseReaction<DateTime.Parse("1-1-1800")) {
					row.Cells.Add(allergyList[i].Reaction);
				}
				else {
					row.Cells.Add(allergyList[i].Reaction+" "+allergyList[i].DateAdverseReaction.ToShortDateString());
				}
				if(allergyList[i].StatusIsActive) {
					row.Cells.Add("Active");
				}
				else {
					row.Cells.Add("Inactive");
				}
				gridAllergies.Rows.Add(row);
			}
			gridAllergies.EndUpdate();
		}

		private void gridAllergies_CellClick(object sender,ODGridClickEventArgs e) {
			if(!CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowInfobutton) {//Security.IsAuthorized(Permissions.EhrInfoButton,true)) {
				return;
			}
			if(e.Col!=0) {
				return;
			}
			List<KnowledgeRequest> listKnowledgeRequests=EhrTriggers.ConvertToKnowledgeRequests(AllergyDefs.GetOne(allergyList[e.Row].AllergyDefNum));
			FormInfobutton FormIB=new FormInfobutton(listKnowledgeRequests);
			FormIB.PatCur=PatCur;
			FormIB.ShowDialog();
			//Nothing to do with Dialog Result yet.
		}

		private void gridAllergies_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormAllergyEdit FAE=new FormAllergyEdit();
			FAE.AllergyCur=allergyList[gridAllergies.GetSelectedIndex()];
			FAE.ShowDialog();
			if(FAE.DialogResult==DialogResult.OK 
				&& CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowCDS 
				&& CDSPermissions.GetForUser(Security.CurUser.UserNum).AllergyCDS) 
			{
				FormCDSIntervention FormCDSI=new FormCDSIntervention();
				FormCDSI.ListCDSI=EhrTriggers.TriggerMatch(AllergyDefs.GetOne(FAE.AllergyCur.AllergyDefNum),PatCur);
				FormCDSI.ShowIfRequired(false);
			}
			FillAllergies();
		}
		
		private void checkShowInactiveAllergies_CheckedChanged(object sender,EventArgs e) {
			FillAllergies();
		}

		private void butAddAllergy_Click(object sender,EventArgs e) {
			FormAllergyEdit formA=new FormAllergyEdit();
			formA.AllergyCur=new Allergy();
			formA.AllergyCur.StatusIsActive=true;
			formA.AllergyCur.PatNum=PatCur.PatNum;
			formA.AllergyCur.IsNew=true;
			formA.ShowDialog();
			if(formA.DialogResult!=DialogResult.OK) {
				return;
			}
			if(CDSPermissions.GetForUser(Security.CurUser.UserNum).ShowCDS && CDSPermissions.GetForUser(Security.CurUser.UserNum).AllergyCDS) {
				FormCDSIntervention FormCDSI=new FormCDSIntervention();
				FormCDSI.ListCDSI=EhrTriggers.TriggerMatch(AllergyDefs.GetOne(formA.AllergyCur.AllergyDefNum),PatCur);
				FormCDSI.ShowIfRequired(false);
			}
			FillAllergies();
		}
		#endregion Allergies Tab

		#region Vital Signs Tab
		private void FillVitalSigns() {
			gridVitalSigns.BeginUpdate();
			gridVitalSigns.Columns.Clear();
			ODGridColumn col=new ODGridColumn("Date",80);
			gridVitalSigns.Columns.Add(col);
			col=new ODGridColumn("Pulse",55);
			gridVitalSigns.Columns.Add(col);
			col=new ODGridColumn("Height",55);
			gridVitalSigns.Columns.Add(col);
			col=new ODGridColumn("Weight",55);
			gridVitalSigns.Columns.Add(col);
			col=new ODGridColumn("BP",55);
			gridVitalSigns.Columns.Add(col);
			col=new ODGridColumn("BMI",55);
			gridVitalSigns.Columns.Add(col);
			col=new ODGridColumn("Documentation for Followup or Ineligible",150);
			gridVitalSigns.Columns.Add(col);
			_listVitalSigns=Vitalsigns.Refresh(PatCur.PatNum);
			gridVitalSigns.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listVitalSigns.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listVitalSigns[i].DateTaken.ToShortDateString());
				row.Cells.Add(_listVitalSigns[i].Pulse.ToString()+" bpm");
				row.Cells.Add(_listVitalSigns[i].Height==0 ? "" : _listVitalSigns[i].Height+" in.");
				row.Cells.Add(_listVitalSigns[i].Weight==0 ? "" : _listVitalSigns[i].Weight+" lbs.");
				string bp="";
				if(_listVitalSigns[i].BpSystolic!=0 || _listVitalSigns[i].BpDiastolic!=0) {
					bp=_listVitalSigns[i].BpSystolic.ToString()+"/"+_listVitalSigns[i].BpDiastolic.ToString();
				}
				row.Cells.Add(bp);
				//BMI = (lbs*703)/(in^2)
				float bmi=Vitalsigns.CalcBMI(_listVitalSigns[i].Weight,_listVitalSigns[i].Height);
				if(bmi!=0) {
					row.Cells.Add(bmi.ToString("n1"));
				}
				else {//leave cell blank because there is not a valid bmi
					row.Cells.Add("");
				}
				row.Cells.Add(_listVitalSigns[i].Documentation);
				gridVitalSigns.Rows.Add(row);
			}
			gridVitalSigns.EndUpdate();
		}

		private void butAddVitalSign_Click(object sender,EventArgs e) {
			FormVitalsignEdit2014 FormVSE=new FormVitalsignEdit2014();
			FormVSE.VitalsignCur=new Vitalsign();
			FormVSE.VitalsignCur.PatNum=PatCur.PatNum;
			FormVSE.VitalsignCur.DateTaken=DateTime.Today;
			FormVSE.VitalsignCur.IsNew=true;
			FormVSE.ShowDialog();
			FillVitalSigns();
		}

		private void gridVitalSigns_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormVitalsignEdit2014 FormVSE=new FormVitalsignEdit2014();
			FormVSE.VitalsignCur=_listVitalSigns[e.Row];
			FormVSE.ShowDialog();
			FillVitalSigns();
		}

		private void butGrowthChart_Click(object sender,EventArgs e) {
			FormEhrGrowthCharts FormGC=new FormEhrGrowthCharts();
			FormGC.PatNum=PatCur.PatNum;
			FormGC.ShowDialog();
		}
		#endregion Vital Signs Tab

		#region Tobacco Use Tab
		private void TobaccoUseTabLoad() {
			textDateAssessed.Text=DateTime.Now.ToString();
			textDateIntervention.Text=DateTime.Now.ToString();
			#region ComboSmokeStatus
			comboSmokeStatus.Items.Add("None");//First and default index
			//Smoking statuses add in the same order as they appear in the SmokingSnoMed enum (Starting at comboSmokeStatus index 1).
			//Changes to the enum order will change the order added so they will always match
			for(int i=0;i<Enum.GetNames(typeof(SmokingSnoMed)).Length;i++) {
				//if snomed code exists in the snomed table, use the snomed description for the combo box, otherwise use the original abbreviated description
				Snomed smokeCur=Snomeds.GetByCode(((SmokingSnoMed)i).ToString().Substring(1));
				if(smokeCur!=null) {
					comboSmokeStatus.Items.Add(smokeCur.Description);
				}
				else {
					switch((SmokingSnoMed)i) {
						case SmokingSnoMed._266927001:
							comboSmokeStatus.Items.Add("UnknownIfEver");
							break;
						case SmokingSnoMed._77176002:
							comboSmokeStatus.Items.Add("SmokerUnknownCurrent");
							break;
						case SmokingSnoMed._266919005:
							comboSmokeStatus.Items.Add("NeverSmoked");
							break;
						case SmokingSnoMed._8517006:
							comboSmokeStatus.Items.Add("FormerSmoker");
							break;
						case SmokingSnoMed._428041000124106:
							comboSmokeStatus.Items.Add("CurrentSomeDay");
							break;
						case SmokingSnoMed._449868002:
							comboSmokeStatus.Items.Add("CurrentEveryDay");
							break;
						case SmokingSnoMed._428061000124105:
							comboSmokeStatus.Items.Add("LightSmoker");
							break;
						case SmokingSnoMed._428071000124103:
							comboSmokeStatus.Items.Add("HeavySmoker");
							break;
					}
				}
			}
			comboSmokeStatus.SelectedIndex=0;//None
			try {
				comboSmokeStatus.SelectedIndex=(int)Enum.Parse(typeof(SmokingSnoMed),"_"+PatCur.SmokingSnoMed,true)+1;
			}
			catch {
				//if not one of the statuses in the enum, get the Snomed object from the patient's current smoking snomed code
				Snomed smokeCur=Snomeds.GetByCode(PatCur.SmokingSnoMed);
				if(smokeCur!=null) {//valid snomed code, set the combo box text to this snomed description
					comboSmokeStatus.SelectedIndex=-1;
					comboSmokeStatus.Text=smokeCur.Description;
				}
			}
			#endregion
			//This takes a while the first time the window loads due to Code Systems.
			Cursor=Cursors.WaitCursor;
			FillGridAssessments();
			FillGridInterventions();
			Cursor=Cursors.Default;
			#region ComboAssessmentType
			_listAssessmentCodes=EhrCodes.GetForValueSetOIDs(new List<string> { "2.16.840.1.113883.3.526.3.1278" },true);//'Tobacco Use Screening' value set
			//Should only happen if the EHR.dll doesn't exist or the codes in the ehrcode list don't exist in the corresponding table
			if(_listAssessmentCodes.Count==0) {
				//disable the tobacco use tab, message box will show if the user tries to select it
				((Control)tabControlFormMedical.TabPages[tabControlFormMedical.TabPages.IndexOfKey("tabTobaccoUse")]).Enabled=false;
				return;
			}
			_listAssessmentCodes.ForEach(x => comboAssessmentType.Items.Add(x.Description));
			string mostRecentAssessmentCode="";
			if(gridAssessments.Rows.Count>1) {
				//gridAssessments.Rows are tagged with all TobaccoUseAssessed events for the patient ordered by DateTEvent, last is most recent
				mostRecentAssessmentCode=((EhrMeasureEvent)gridAssessments.Rows[gridAssessments.Rows.Count-1].Tag).CodeValueResult;
			}
			//use Math.Max so that if _listAssessmentCodes doesn't contain the mostRecentAssessment code the combobox will default to the first in the list
			comboAssessmentType.SelectedIndex=Math.Max(0,_listAssessmentCodes.FindIndex(x => x.CodeValue==mostRecentAssessmentCode));
			#endregion ComboAssessmentType
			#region ComboTobaccoStatus
			//list is filled with the EhrCodes for all tobacco user statuses using the CQM value set
			_listUserCodes=EhrCodes.GetForValueSetOIDs(new List<string> { "2.16.840.1.113883.3.526.3.1170" },true).OrderBy(x => x.Description).ToList();
			//list is filled with the EhrCodes for all tobacco non-user statuses using the CQM value set
			_listNonUserCodes=EhrCodes.GetForValueSetOIDs(new List<string> { "2.16.840.1.113883.3.526.3.1189" },true).OrderBy(x => x.Description).ToList();
			_listRecentTobaccoCodes=EhrCodes.GetForEventTypeByUse(EhrMeasureEventType.TobaccoUseAssessed);
			//list is filled with any SNOMEDCT codes that are attached to EhrMeasureEvents for the patient that are not in the User and NonUser lists
			_listCustomTobaccoCodes=new List<EhrCode>();
			//codeValues is an array of all user and non-user tobacco codes
			string[] codeValues=_listUserCodes.Concat(_listNonUserCodes).Concat(_listRecentTobaccoCodes).Select(x => x.CodeValue).ToArray();
			//listEventCodes will contain all unique tobacco codes that are not in the user and non-user lists
			List<string> listEventCodes=new List<string>();
			foreach(ODGridRow row in gridAssessments.Rows) {
				string eventCodeCur=((EhrMeasureEvent)row.Tag).CodeValueResult;
				if(codeValues.Contains(eventCodeCur) || listEventCodes.Contains(eventCodeCur)) {
					continue;
				}
				listEventCodes.Add(eventCodeCur);
			}
			Snomed sCur;
			foreach(string eventCode in listEventCodes.OrderBy(x => x)) {
				sCur=Snomeds.GetByCode(eventCode);
				if(sCur==null) {//don't add invalid SNOMEDCT codes
					continue;
				}
				_listCustomTobaccoCodes.Add(new EhrCode { CodeValue=sCur.SnomedCode,Description=sCur.Description });
			}
			_listCustomTobaccoCodes=_listCustomTobaccoCodes.OrderBy(x => x.Description).ToList();
			//list will contain all of the tobacco status EhrCodes currently in comboTobaccoStatus
			_listTobaccoStatuses=new List<EhrCode>();
			//default to all tobacco statuses (custom, user, and non-user) in the status dropdown box
			radioRecentStatuses.Checked=true;//causes combo box and _listTobaccoStatuses to be filled with all statuses
			#endregion ComboTobaccoStatus
			#region ComboInterventionType and ComboInterventionCode
			//list is filled with EhrCodes for counseling interventions using the CQM value set
			_listCounselInterventionCodes=EhrCodes.GetForValueSetOIDs(new List<string> { "2.16.840.1.113883.3.526.3.509" },true).OrderBy(x => x.Description).ToList();
			//list is filled with EhrCodes for medication interventions using the CQM value set
			_listMedInterventionCodes=EhrCodes.GetForValueSetOIDs(new List<string> { "2.16.840.1.113883.3.526.3.1190" },true).OrderBy(x => x.Description).ToList();
			_listRecentIntvCodes=EhrCodes.GetForIntervAndMedByUse(InterventionCodeSet.TobaccoCessation,new List<string> { "2.16.840.1.113883.3.526.3.1190" });
			_listInterventionCodes=new List<EhrCode>();
			//default to all interventions (couseling and medication) in the intervention dropdown box
			radioRecentInterventions.Checked=true;//causes combo box and _listInterventionCodes to be filled with all intervention codes
			#endregion ComboInterventionType and ComboInterventionCode
			_comboToolTip=new ToolTip() { InitialDelay=1000,ReshowDelay=1000,ShowAlways=true };
		}

		private void FillGridAssessments() {
			gridAssessments.BeginUpdate();
			gridAssessments.Columns.Clear();
			gridAssessments.Columns.Add(new ODGridColumn("Date",70));
			gridAssessments.Columns.Add(new ODGridColumn("Type",170));
			gridAssessments.Columns.Add(new ODGridColumn("Description",170));
			gridAssessments.Columns.Add(new ODGridColumn("Documentation",170));
			gridAssessments.Rows.Clear();
			ODGridRow row;
			Loinc lCur;
			Snomed sCur;
			List<EhrMeasureEvent> listEvents=EhrMeasureEvents.RefreshByType(PatCur.PatNum,EhrMeasureEventType.TobaccoUseAssessed);
			foreach(EhrMeasureEvent eventCur in listEvents) {
				row=new ODGridRow();
				row.Cells.Add(eventCur.DateTEvent.ToShortDateString());
				lCur=Loincs.GetByCode(eventCur.CodeValueEvent);//TobaccoUseAssessed events can be one of three types, all LOINC codes
				row.Cells.Add(lCur!=null?lCur.NameLongCommon:eventCur.EventType.ToString());
				sCur=Snomeds.GetByCode(eventCur.CodeValueResult);
				row.Cells.Add(sCur!=null?sCur.Description:"");
				row.Cells.Add(eventCur.MoreInfo);
				row.Tag=eventCur;
				gridAssessments.Rows.Add(row);
			}
			gridAssessments.EndUpdate();
		}

		private void FillGridInterventions() {
			gridInterventions.BeginUpdate();
			gridInterventions.Columns.Clear();
			gridInterventions.Columns.Add(new ODGridColumn("Date",70));
			gridInterventions.Columns.Add(new ODGridColumn("Type",150));
			gridInterventions.Columns.Add(new ODGridColumn("Description",160));
			gridInterventions.Columns.Add(new ODGridColumn("Declined",60) { TextAlign=HorizontalAlignment.Center });
			gridInterventions.Columns.Add(new ODGridColumn("Documentation",140));
			gridInterventions.Rows.Clear();
			//build list of rows of CessationInterventions and CessationMedications so we can order the list by date and type before filling the grid
			List<ODGridRow> listRows=new List<ODGridRow>();
			ODGridRow row;
			#region CessationInterventions
			Cpt cptCur;
			Snomed sCur;
			RxNorm rCur;
			string type;
			string descript;
			List<Intervention> listInterventions=Interventions.Refresh(PatCur.PatNum,InterventionCodeSet.TobaccoCessation);
			foreach(Intervention iCur in listInterventions) {
				row=new ODGridRow();
				row.Cells.Add(iCur.DateEntry.ToShortDateString());
				type=InterventionCodeSet.TobaccoCessation.ToString()+" Counseling";
				descript="";
				switch(iCur.CodeSystem) {
					case "CPT":
						cptCur=Cpts.GetByCode(iCur.CodeValue);
						descript=cptCur!=null?cptCur.Description:"";
						break;
					case "SNOMEDCT":
						sCur=Snomeds.GetByCode(iCur.CodeValue);
						descript=sCur!=null?sCur.Description:"";
						break;
					case "RXNORM":
						//if the user checks the "Patient Declined" checkbox, we enter the tobacco cessation medication as an intervention that was declined
						type=InterventionCodeSet.TobaccoCessation.ToString()+" Medication";
						rCur=RxNorms.GetByRxCUI(iCur.CodeValue);
						descript=rCur!=null?rCur.Description:"";
						break;
				}
				row.Cells.Add(type);
				row.Cells.Add(descript);
				row.Cells.Add(iCur.IsPatDeclined?"X":"");
				row.Cells.Add(iCur.Note);
				row.Tag=iCur;
				listRows.Add(row);
			}
			#endregion
			#region CessationMedications
			//Tobacco Use Cessation Pharmacotherapy Value Set
			string[] arrayRxCuiStrings=EhrCodes.GetForValueSetOIDs(new List<string> { "2.16.840.1.113883.3.526.3.1190" },true)
				.Select(x => x.CodeValue).ToArray();
			//arrayRxCuiStrings will contain 41 RxCui strings for tobacco cessation medications if those exist in the rxnorm table
			List<MedicationPat> listMedPats=MedicationPats.Refresh(PatCur.PatNum,true).FindAll(x => arrayRxCuiStrings.Contains(x.RxCui.ToString()));
			foreach(MedicationPat medPatCur in listMedPats) {
				row=new ODGridRow();
				List<string> listMedDates=new List<string>();
				if(medPatCur.DateStart.Year>1880) {
					listMedDates.Add(medPatCur.DateStart.ToShortDateString());
				}
				if(medPatCur.DateStop.Year>1880) {
					listMedDates.Add(medPatCur.DateStop.ToShortDateString());
				}
				if(listMedDates.Count==0) {
					listMedDates.Add(medPatCur.DateTStamp.ToShortDateString());
				}
				row.Cells.Add(listMedDates.Count==0?"":string.Join(" - ",listMedDates));
				row.Cells.Add(InterventionCodeSet.TobaccoCessation.ToString()+" Medication");
				row.Cells.Add(RxNorms.GetDescByRxCui(medPatCur.RxCui.ToString()));
				row.Cells.Add(medPatCur.PatNote);
				row.Tag=medPatCur;
				listRows.Add(row);
			}
			#endregion
			listRows.OrderBy(x => PIn.Date(x.Cells[0].Text))//rows ordered by date, oldest first
				.ThenBy(x => x.Cells[3].Text!="")
				//interventions at the top, declined med interventions below normal interventions
				.ThenBy(x => x.Tag.GetType().Name!="Intervention" || ((Intervention)x.Tag).CodeSystem=="RXNORM").ToList()
				.ForEach(x => gridInterventions.Rows.Add(x));//then add rows to gridInterventions
			gridInterventions.EndUpdate();
		}

		private void gridAssessments_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			//we will allow them to change the DateTEvent, but not the status or more info box
			FormEhrMeasureEventEdit FormM=new FormEhrMeasureEventEdit((EhrMeasureEvent)gridAssessments.Rows[e.Row].Tag);
			FormM.ShowDialog();
			FillGridAssessments();
		}

		private void gridInterventions_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Object objCur=gridInterventions.Rows[e.Row].Tag;
			//the intervention grid will be filled with Interventions and MedicationPats, load form accordingly
			if(objCur is Intervention) {
				FormInterventionEdit FormI=new FormInterventionEdit();
				FormI.InterventionCur=(Intervention)objCur;
				FormI.IsAllTypes=false;
				FormI.IsSelectionMode=false;
				FormI.InterventionCur.IsNew=false;
				FormI.ShowDialog();
			}
			else if(objCur is MedicationPat) {
				FormMedPat FormMP=new FormMedPat();
				FormMP.MedicationPatCur=(MedicationPat)objCur;
				FormMP.IsNew=false;
				FormMP.ShowDialog();
			}
			FillGridInterventions();
		}

		private void comboSmokeStatus_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboSmokeStatus.SelectedIndex<1) {//If None or text set to other selected Snomed code so -1, do not create an event
				return;
			}
			//Insert measure event if one does not already exist for this date
			DateTime dateTEntered=PIn.DateT(textDateAssessed.Text);//will be set to DateTime.Now when form loads
			EhrMeasureEvent eventCur;
			foreach(ODGridRow row in gridAssessments.Rows) {
				eventCur=(EhrMeasureEvent)row.Tag;
				if(eventCur.DateTEvent.Date==dateTEntered.Date) {//one already exists for this date, don't auto insert event
					return;
				}
			}
			//no entry for the date entered, so insert one
			eventCur=new EhrMeasureEvent();
			eventCur.DateTEvent=dateTEntered;
			eventCur.EventType=EhrMeasureEventType.TobaccoUseAssessed;
			eventCur.PatNum=PatCur.PatNum;
			eventCur.CodeValueEvent=_listAssessmentCodes[comboAssessmentType.SelectedIndex].CodeValue;
			eventCur.CodeSystemEvent=_listAssessmentCodes[comboAssessmentType.SelectedIndex].CodeSystem;
			//SelectedIndex guaranteed to be greater than 0
			eventCur.CodeValueResult=((SmokingSnoMed)comboSmokeStatus.SelectedIndex-1).ToString().Substring(1);
			eventCur.CodeSystemResult="SNOMEDCT";//only allow SNOMEDCT codes for now.
			eventCur.MoreInfo="";
			EhrMeasureEvents.Insert(eventCur);
			FillGridAssessments();
		}

		private void comboTobaccoStatus_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboTobaccoStatus.SelectedIndex<_listTobaccoStatuses.Count) {//user selected a code in the list, just return
				return;
			}
			if(comboTobaccoStatus.SelectedIndex==_listTobaccoStatuses.Count
				&& !MsgBox.Show(this,MsgBoxButtons.OKCancel,"Selecting a code that is not in the recommended list of codes may make "
					+"it more difficult to meet CQM's."))
			{
				comboTobaccoStatus.SelectedIndex=-1;
				return;
			}
			//user wants to select a custom status from the SNOMED list
			FormSnomeds FormS=new FormSnomeds();
			FormS.IsSelectionMode=true;
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK) {
				comboTobaccoStatus.SelectedIndex=-1;
				return;
			}
			if(!_listTobaccoStatuses.Any(x => x.CodeValue==FormS.SelectedSnomed.SnomedCode)) {
				_listCustomTobaccoCodes.Add(new EhrCode() { CodeValue=FormS.SelectedSnomed.SnomedCode,Description=FormS.SelectedSnomed.Description });
				_listCustomTobaccoCodes=_listCustomTobaccoCodes.OrderBy(x => x.Description).ToList();
				radioTobaccoStatuses_CheckedChanged(new[] { radioUserStatuses,radioNonUserStatuses }.Where(x => x.Checked)
					.DefaultIfEmpty(radioAllStatuses).FirstOrDefault()
					,new EventArgs());//refills drop down with newly added custom code
			}
			//selected code guaranteed to exist in the drop down at this point
			comboTobaccoStatus.Items.Clear();
			comboTobaccoStatus.Items.AddRange(_listTobaccoStatuses.Select(x => x.Description).ToArray());
			comboTobaccoStatus.Items.Add(Lan.g(this,"Choose from all SNOMED CT codes")+"...");
			comboTobaccoStatus.SelectedIndex=_listTobaccoStatuses.FindIndex(x => x.CodeValue==FormS.SelectedSnomed.SnomedCode);//add 1 for ...choose from
		}

		private void comboInterventionCode_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboInterventionCode.SelectedIndex>=0) {
				_comboToolTip.SetToolTip(comboInterventionCode,_listInterventionCodes[comboInterventionCode.SelectedIndex].Description);
			}
		}

		///<summary>Fill comboInterventionCode with counseling and medication intervention codes using _listCounselInterventionCodes
		///and/or _listMedInterventionCodes depending on which radio button is selected.</summary>
		private void radioInterventions_CheckedChanged(object sender,EventArgs e) {
			RadioButton radButCur=(RadioButton)sender;
			if(!radButCur.Checked) {//if not checked, do nothing, caused by another radio button being checked
				return;
			}
			_listInterventionCodes.Clear();
			if(radButCur.Name==radioRecentInterventions.Name) {
				_listInterventionCodes.AddRange(_listRecentIntvCodes);
			}
			if(new[] { radioAllInterventions.Name,radioCounselInterventions.Name }.Contains(radButCur.Name)) {
				_listInterventionCodes.AddRange(_listCounselInterventionCodes);
			}
			if(new[] { radioAllInterventions.Name,radioMedInterventions.Name }.Contains(radButCur.Name)) {
				_listInterventionCodes.AddRange(_listMedInterventionCodes);
			}
			_listInterventionCodes=_listInterventionCodes.OrderBy(x => x.Description).ToList();
			comboInterventionCode.Items.Clear();
			//this is the max width of the description, minus the width of "..." and, if > 30 items in the list, the width of the vertical scroll bar
			int maxItemWidth=comboInterventionCode.DropDownWidth-(_listInterventionCodes.Count>30?25:8);//8 for just "...", 25 for scroll bar plus "..."
			foreach(EhrCode code in _listInterventionCodes) {
				if(TextRenderer.MeasureText(code.Description,comboInterventionCode.Font).Width<comboInterventionCode.DropDownWidth-15
					|| code.Description.Length<3)
				{
					comboInterventionCode.Items.Add(code.Description);
					continue;
				}
				StringBuilder abbrDesc=new StringBuilder();
				foreach(char c in code.Description) {
					if(TextRenderer.MeasureText(abbrDesc.ToString()+c,comboInterventionCode.Font).Width<maxItemWidth) {
						abbrDesc.Append(c);
						continue;
					}
					comboInterventionCode.Items.Add(abbrDesc.ToString()+"...");
					break;
				}
			}
		}

		///<summary>Fill comboTobaccoStatus with user and non-user tobacco status codes using _listUserCodes and/or _listNonUserCodes
		///depending on which radio button is selected.</summary>
		private void radioTobaccoStatuses_CheckedChanged(object sender,EventArgs e) {
			RadioButton radButCur=(RadioButton)sender;
			if(!radButCur.Checked) {
				return;
			}
			_listTobaccoStatuses.Clear();
			if(_listCustomTobaccoCodes.Count>0) {
				_listTobaccoStatuses.AddRange(_listCustomTobaccoCodes);
			}
			if(radButCur.Name==radioRecentStatuses.Name) {
				_listTobaccoStatuses.AddRange(_listRecentTobaccoCodes);
			}
			else {
				if(new[] { radioAllStatuses.Name,radioUserStatuses.Name }.Contains(radButCur.Name)) {
					_listTobaccoStatuses.AddRange(_listUserCodes);
				}
				if(new[] { radioAllStatuses.Name,radioNonUserStatuses.Name }.Contains(radButCur.Name)) {
					_listTobaccoStatuses.AddRange(_listNonUserCodes);
				}
			}
			_listTobaccoStatuses=_listTobaccoStatuses.OrderBy(x => x.Description).ToList();
			comboTobaccoStatus.Items.Clear();
			comboTobaccoStatus.Items.AddRange(_listTobaccoStatuses.Select(x => x.Description).ToArray());
			comboTobaccoStatus.Items.Add(Lan.g(this,"Choose from all SNOMED CT codes")+"...");
		}

		///<summary>If the LOINC table has not been imported, the Tobacco Use tab is disabled, but we want it to remain visible like the other EHR show
		///feature enabled tabs.  But since the combo boxes etc. cannot be filled without the LOINC table, don't allow selecting the tab.</summary>
		private void tabControlFormMedical_Selecting(object sender,TabControlCancelEventArgs e) {
			if(!((Control)e.TabPage).Enabled) {
				e.Cancel=true;
				MsgBox.Show(this,"The codes used for Tobacco Use Screening assessments do not exist in the LOINC table in your database.  You must run the "
					+"Code System Importer tool in Setup | Chart | EHR to import this code set before accessing the Tobacco Use Tab.");
			}
		}

		private void butAssessed_Click(object sender,EventArgs e) {
			if(comboTobaccoStatus.SelectedIndex<0 || comboTobaccoStatus.SelectedIndex>=_listTobaccoStatuses.Count) {
				MsgBox.Show(this,"You must select a tobacco status.");
				return;
			}
			DateTime dateTEntered=PIn.DateT(textDateAssessed.Text);
			EhrMeasureEvent meas=new EhrMeasureEvent();
			meas.DateTEvent=dateTEntered;
			meas.EventType=EhrMeasureEventType.TobaccoUseAssessed;
			meas.PatNum=PatCur.PatNum;
			meas.CodeValueEvent=_listAssessmentCodes[comboAssessmentType.SelectedIndex].CodeValue;
			meas.CodeSystemEvent=_listAssessmentCodes[comboAssessmentType.SelectedIndex].CodeSystem;
			meas.CodeValueResult=_listTobaccoStatuses[comboTobaccoStatus.SelectedIndex].CodeValue;
			meas.CodeSystemResult="SNOMEDCT";//only allow SNOMEDCT codes for now.
			meas.MoreInfo="";
			EhrMeasureEvents.Insert(meas);
			comboTobaccoStatus.SelectedIndex=-1;
			FillGridAssessments();
		}

		private void butIntervention_Click(object sender,EventArgs e) {
			if(comboInterventionCode.SelectedIndex<0) {
				MsgBox.Show(this,"You must select an intervention code.");
				return;
			}
			EhrCode iCodeCur=_listInterventionCodes[comboInterventionCode.SelectedIndex];
			DateTime dateCur=PIn.Date(textDateIntervention.Text);
			if(iCodeCur.CodeSystem=="RXNORM" && !checkPatientDeclined.Checked) {//if patient declines the medication, enter as a declined intervention
				//codeVal will be RxCui of medication, see if it already exists in Medication table
				Medication medCur=Medications.GetMedicationFromDbByRxCui(PIn.Long(iCodeCur.CodeValue));
				if(medCur==null) {//no med with this RxCui, create one
					medCur=new Medication();
					Medications.Insert(medCur);//so that we will have the primary key
					medCur.GenericNum=medCur.MedicationNum;
					medCur.RxCui=PIn.Long(iCodeCur.CodeValue);
					medCur.MedName=RxNorms.GetDescByRxCui(iCodeCur.CodeValue);
					Medications.Update(medCur);
					Medications.RefreshCache();//refresh cache to include new medication
				}
				MedicationPat medPatCur=new MedicationPat();
				medPatCur.PatNum=PatCur.PatNum;
				medPatCur.ProvNum=PatCur.PriProv;
				medPatCur.MedicationNum=medCur.MedicationNum;
				medPatCur.RxCui=medCur.RxCui;
				medPatCur.DateStart=dateCur;
				FormMedPat FormMP=new FormMedPat();
				FormMP.MedicationPatCur=medPatCur;
				FormMP.IsNew=true;
				FormMP.ShowDialog();
				if(FormMP.DialogResult!=DialogResult.OK) {
					return;
				}
				if(FormMP.MedicationPatCur.DateStart.Date<dateCur.AddMonths(-6).Date || FormMP.MedicationPatCur.DateStart.Date>dateCur.Date) {
					MsgBox.Show(this,"The medication order just entered is not within the 6 months prior to the date of this intervention.  You can modify the "
						+"date of the medication order in the patient's medical history section.");
				}
			}
			else {
				Intervention iCur=new Intervention();
				iCur.PatNum=PatCur.PatNum;
				iCur.ProvNum=PatCur.PriProv;
				iCur.DateEntry=dateCur;
				iCur.CodeValue=iCodeCur.CodeValue;
				iCur.CodeSystem=iCodeCur.CodeSystem;
				iCur.CodeSet=InterventionCodeSet.TobaccoCessation;
				iCur.IsPatDeclined=checkPatientDeclined.Checked;
				Interventions.Insert(iCur);
			}
			comboInterventionCode.SelectedIndex=-1;
			FillGridInterventions();
		}
		#endregion Tobacco Use Tab

		private void FormMedical_ResizeEnd(object sender,EventArgs e) {
			FillMeds();
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(comboSmokeStatus.SelectedIndex==0) {//None
				PatCur.SmokingSnoMed="";
			}
			else {
				PatCur.SmokingSnoMed=((SmokingSnoMed)comboSmokeStatus.SelectedIndex-1).ToString().Substring(1);
			}
			PatCur.Premed=checkPremed.Checked;
			PatCur.MedUrgNote=textMedUrgNote.Text;
			Patients.Update(PatCur,_patOld);
			PatientNoteCur.Medical=textMedical.Text;
			PatientNoteCur.Service=textService.Text;
			PatientNoteCur.MedicalComp=textMedicalComp.Text;
			PatientNotes.Update(PatientNoteCur, PatCur.Guarantor);
			//Insert an ehrmeasureevent for CurrentMedsDocumented if user selected Yes and there isn't one with today's date
			if(radioMedsDocumentedYes.Checked && _EhrMeasureEventNum==0) {
				EhrMeasureEvent ehrMeasureEventCur=new EhrMeasureEvent();
				ehrMeasureEventCur.PatNum=PatCur.PatNum;
				ehrMeasureEventCur.DateTEvent=DateTime.Now;
				ehrMeasureEventCur.EventType=EhrMeasureEventType.CurrentMedsDocumented;
				ehrMeasureEventCur.CodeValueEvent="428191000124101";//SNOMEDCT code for document current meds procedure
				ehrMeasureEventCur.CodeSystemEvent="SNOMEDCT";
				EhrMeasureEvents.Insert(ehrMeasureEventCur);
			}
			//No is selected, if no EhrNotPerformed item for current meds documented, launch not performed edit window to allow user to select valid reason.
			if(radioMedsDocumentedNo.Checked) {
				if(_EhrNotPerfNum==0) {
					FormEhrNotPerformedEdit FormNP=new FormEhrNotPerformedEdit();
					FormNP.EhrNotPerfCur=new EhrNotPerformed();
					FormNP.EhrNotPerfCur.IsNew=true;
					FormNP.EhrNotPerfCur.PatNum=PatCur.PatNum;
					FormNP.EhrNotPerfCur.ProvNum=PatCur.PriProv;
					FormNP.SelectedItemIndex=(int)EhrNotPerformedItem.DocumentCurrentMeds;
					FormNP.EhrNotPerfCur.DateEntry=DateTime.Today;
					FormNP.IsDateReadOnly=true;
					FormNP.ShowDialog();
					if(FormNP.DialogResult==DialogResult.OK) {//if they just inserted a not performed item, set the private class-wide variable for the next if statement
						_EhrNotPerfNum=FormNP.EhrNotPerfCur.EhrNotPerformedNum;
					}
				}
				if(_EhrNotPerfNum>0 && _EhrMeasureEventNum>0) {//if not performed item is entered with today's date, delete existing performed item
					EhrMeasureEvents.Delete(_EhrMeasureEventNum);
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}
