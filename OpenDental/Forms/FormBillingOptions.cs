using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using OpenDental.UI;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental{
	///<summary></summary>
	public class FormBillingOptions : ODForm {
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butCancel;
		//private FormQuery FormQuery2;
		private System.Windows.Forms.Label label1;
		private OpenDental.UI.Button butSaveDefault;
		private OpenDental.ValidDouble textExcludeLessThan;
		private System.Windows.Forms.CheckBox checkExcludeInactive;
		private System.Windows.Forms.GroupBox groupBox2;
		private OpenDental.UI.Button butDunningSetup;
		private System.Windows.Forms.Label label3;
		private OpenDental.UI.ODGrid gridDun;
		private System.Windows.Forms.Label label4;
		private OpenDental.ODtextBox textNote;
		private System.Windows.Forms.CheckBox checkBadAddress;
		private System.Windows.Forms.CheckBox checkShowNegative;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox checkIncludeChanged;
		private OpenDental.ValidDate textLastStatement;
		private OpenDental.UI.Button butCreate;
		private CheckBox checkExcludeInsPending;
		private GroupBox groupDateRange;
		private ValidDate textDateStart;
		private Label labelStartDate;
		private Label labelEndDate;
		private ValidDate textDateEnd;
		private OpenDental.UI.Button but45days;
		private OpenDental.UI.Button but90days;
		private OpenDental.UI.Button butDatesAll;
		private CheckBox checkIntermingled;
		private OpenDental.UI.Button butDefaults;
		private OpenDental.UI.Button but30days;
		private ComboBox comboAge;
		private Label label6;
		private Label label7;
		private ListBox listBillType;
		private Label labelSaveDefaults;
		private OpenDental.UI.Button butUndo;
		private CheckBox checkIgnoreInPerson;
		private CheckBox checkExcludeIfProcs;
		private Label labelClinic;
		private List<Dunning> _listDunnings;
		public long ClinicNum;
		private CheckBox checkSuperFam;
		private CheckBox checkUseClinicDefaults;
		///<summary>Key: ClinicNum, Value: List of ClinicPrefs for clinic.
		///List contains all existing ClinicPrefs.</summary>
		private Dictionary<long,List<ClinicPref>> _dictClinicPrefsOld;
		///<summary>Key: ClinicNum, Value: List of ClinicPrefs for clinic.
		///Starts off as a copy of _ListClinicPrefsOld, then is modified.</summary>
		private Dictionary<long,List<ClinicPref>> _dictClinicPrefsNew;
		///<summary>When creating list for all clinics, this stores text we show after completed.</summary>
		private string _popUpMessage;
		private CheckBox checkBoxBillShowTransSinceZero;
		private ComboBoxMulti comboClinic;
		private UI.Button butPickClinic;
		private CheckBox checkSinglePatient;

		///<summary>Do not pass a list of clinics in.  This list gets filled on load based on the user logged in.  ListClinics is used in other forms so it is public.</summary>
		private List<Clinic> _listClinics;
		private Label labelModesToText;
		private ListBox listModeToText;
		private Label labelMultiClinicGenMsg;
		private List<Def> _listBillingTypeDefs;

		///<summary></summary>
		public FormBillingOptions(){
			InitializeComponent();
			Lan.F(this);
		}

		///<summary></summary>
		protected override void Dispose(bool disposing){
			if(disposing){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBillingOptions));
			this.butCancel = new OpenDental.UI.Button();
			this.butCreate = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.butSaveDefault = new OpenDental.UI.Button();
			this.textExcludeLessThan = new OpenDental.ValidDouble();
			this.checkExcludeInactive = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.butPickClinic = new OpenDental.UI.Button();
			this.comboClinic = new OpenDental.UI.ComboBoxMulti();
			this.checkBoxBillShowTransSinceZero = new System.Windows.Forms.CheckBox();
			this.checkUseClinicDefaults = new System.Windows.Forms.CheckBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.checkExcludeIfProcs = new System.Windows.Forms.CheckBox();
			this.checkIgnoreInPerson = new System.Windows.Forms.CheckBox();
			this.labelSaveDefaults = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.comboAge = new System.Windows.Forms.ComboBox();
			this.checkExcludeInsPending = new System.Windows.Forms.CheckBox();
			this.checkIncludeChanged = new System.Windows.Forms.CheckBox();
			this.textLastStatement = new OpenDental.ValidDate();
			this.label5 = new System.Windows.Forms.Label();
			this.checkShowNegative = new System.Windows.Forms.CheckBox();
			this.checkBadAddress = new System.Windows.Forms.CheckBox();
			this.listBillType = new System.Windows.Forms.ListBox();
			this.gridDun = new OpenDental.UI.ODGrid();
			this.butDunningSetup = new OpenDental.UI.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textNote = new OpenDental.ODtextBox();
			this.groupDateRange = new System.Windows.Forms.GroupBox();
			this.but30days = new OpenDental.UI.Button();
			this.textDateStart = new OpenDental.ValidDate();
			this.labelStartDate = new System.Windows.Forms.Label();
			this.labelEndDate = new System.Windows.Forms.Label();
			this.textDateEnd = new OpenDental.ValidDate();
			this.but45days = new OpenDental.UI.Button();
			this.but90days = new OpenDental.UI.Button();
			this.butDatesAll = new OpenDental.UI.Button();
			this.checkIntermingled = new System.Windows.Forms.CheckBox();
			this.butDefaults = new OpenDental.UI.Button();
			this.butUndo = new OpenDental.UI.Button();
			this.checkSuperFam = new System.Windows.Forms.CheckBox();
			this.checkSinglePatient = new System.Windows.Forms.CheckBox();
			this.labelModesToText = new System.Windows.Forms.Label();
			this.listModeToText = new System.Windows.Forms.ListBox();
			this.labelMultiClinicGenMsg = new System.Windows.Forms.Label();
			this.groupBox2.SuspendLayout();
			this.groupDateRange.SuspendLayout();
			this.SuspendLayout();
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
			this.butCancel.Location = new System.Drawing.Point(806, 676);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(79, 24);
			this.butCancel.TabIndex = 9;
			this.butCancel.Text = "&Close";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butCreate
			// 
			this.butCreate.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCreate.Autosize = true;
			this.butCreate.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCreate.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCreate.CornerRadius = 4F;
			this.butCreate.Location = new System.Drawing.Point(693, 676);
			this.butCreate.Name = "butCreate";
			this.butCreate.Size = new System.Drawing.Size(92, 24);
			this.butCreate.TabIndex = 8;
			this.butCreate.Text = "Create &List";
			this.butCreate.Click += new System.EventHandler(this.butCreate_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(5, 186);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(192, 16);
			this.label1.TabIndex = 18;
			this.label1.Text = "Exclude if Balance is less than";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butSaveDefault
			// 
			this.butSaveDefault.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSaveDefault.Autosize = true;
			this.butSaveDefault.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSaveDefault.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSaveDefault.CornerRadius = 4F;
			this.butSaveDefault.Location = new System.Drawing.Point(169, 578);
			this.butSaveDefault.Name = "butSaveDefault";
			this.butSaveDefault.Size = new System.Drawing.Size(108, 24);
			this.butSaveDefault.TabIndex = 12;
			this.butSaveDefault.Text = "&Save As Default";
			this.butSaveDefault.Click += new System.EventHandler(this.butSaveDefault_Click);
			// 
			// textExcludeLessThan
			// 
			this.textExcludeLessThan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textExcludeLessThan.Location = new System.Drawing.Point(199, 185);
			this.textExcludeLessThan.MaxVal = 100000000D;
			this.textExcludeLessThan.MinVal = -100000000D;
			this.textExcludeLessThan.Name = "textExcludeLessThan";
			this.textExcludeLessThan.Size = new System.Drawing.Size(77, 20);
			this.textExcludeLessThan.TabIndex = 8;
			// 
			// checkExcludeInactive
			// 
			this.checkExcludeInactive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkExcludeInactive.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkExcludeInactive.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkExcludeInactive.Location = new System.Drawing.Point(45, 122);
			this.checkExcludeInactive.Name = "checkExcludeInactive";
			this.checkExcludeInactive.Size = new System.Drawing.Size(231, 18);
			this.checkExcludeInactive.TabIndex = 4;
			this.checkExcludeInactive.Text = "Exclude inactive families";
			this.checkExcludeInactive.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.butPickClinic);
			this.groupBox2.Controls.Add(this.comboClinic);
			this.groupBox2.Controls.Add(this.checkUseClinicDefaults);
			this.groupBox2.Controls.Add(this.labelClinic);
			this.groupBox2.Controls.Add(this.checkExcludeIfProcs);
			this.groupBox2.Controls.Add(this.checkIgnoreInPerson);
			this.groupBox2.Controls.Add(this.labelSaveDefaults);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.comboAge);
			this.groupBox2.Controls.Add(this.checkExcludeInsPending);
			this.groupBox2.Controls.Add(this.checkIncludeChanged);
			this.groupBox2.Controls.Add(this.textLastStatement);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.checkShowNegative);
			this.groupBox2.Controls.Add(this.checkBadAddress);
			this.groupBox2.Controls.Add(this.checkExcludeInactive);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.textExcludeLessThan);
			this.groupBox2.Controls.Add(this.butSaveDefault);
			this.groupBox2.Controls.Add(this.listBillType);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(7, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(283, 631);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Filter";
			// 
			// butPickClinic
			// 
			this.butPickClinic.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickClinic.Autosize = true;
			this.butPickClinic.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickClinic.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickClinic.CornerRadius = 2F;
			this.butPickClinic.Location = new System.Drawing.Point(255, 500);
			this.butPickClinic.Name = "butPickClinic";
			this.butPickClinic.Size = new System.Drawing.Size(21, 21);
			this.butPickClinic.TabIndex = 253;
			this.butPickClinic.Text = "...";
			this.butPickClinic.Visible = false;
			this.butPickClinic.Click += new System.EventHandler(this.butPickClinic_Click);
			// 
			// comboClinic
			// 
			this.comboClinic.ArraySelectedIndices = new int[0];
			this.comboClinic.BackColor = System.Drawing.SystemColors.Window;
			this.comboClinic.Items = ((System.Collections.ArrayList)(resources.GetObject("comboClinic.Items")));
			this.comboClinic.Location = new System.Drawing.Point(107, 500);
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboClinic.SelectedIndices")));
			this.comboClinic.Size = new System.Drawing.Size(144, 21);
			this.comboClinic.TabIndex = 27;
			this.comboClinic.Visible = false;
			this.comboClinic.ComboDelimiter = ComboMultiDelimiter.None;
			this.comboClinic.SelectionChangeCommitted += new OpenDental.UI.ComboBoxMulti.SelectionChangeCommittedHandler(this.comboClinic_SelectedIndexChanged);
			// 
			// checkBoxBillShowTransSinceZero
			// 
			this.checkBoxBillShowTransSinceZero.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBoxBillShowTransSinceZero.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxBillShowTransSinceZero.Location = new System.Drawing.Point(75,62);
			this.checkBoxBillShowTransSinceZero.Name = "checkBoxBillShowTransSinceZero";
			this.checkBoxBillShowTransSinceZero.Size = new System.Drawing.Size(238,18);
			this.checkBoxBillShowTransSinceZero.TabIndex = 252;
			this.checkBoxBillShowTransSinceZero.Text = "Show all transactions since zero balance";
			this.checkBoxBillShowTransSinceZero.CheckedChanged += new System.EventHandler(this.checkBoxBillShowTransSinceZero_CheckedChanged);
			// 
			// checkUseClinicDefaults
			// 
			this.checkUseClinicDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkUseClinicDefaults.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUseClinicDefaults.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkUseClinicDefaults.Location = new System.Drawing.Point(45, 530);
			this.checkUseClinicDefaults.Name = "checkUseClinicDefaults";
			this.checkUseClinicDefaults.Size = new System.Drawing.Size(231, 18);
			this.checkUseClinicDefaults.TabIndex = 251;
			this.checkUseClinicDefaults.Text = "Use clinic default billing options";
			this.checkUseClinicDefaults.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUseClinicDefaults.CheckedChanged += new System.EventHandler(this.checkUseClinicDefaults_CheckedChanged);
			// 
			// labelClinic
			// 
			this.labelClinic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelClinic.Location = new System.Drawing.Point(22, 502);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(84, 16);
			this.labelClinic.TabIndex = 250;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelClinic.Visible = false;
			// 
			// checkExcludeIfProcs
			// 
			this.checkExcludeIfProcs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkExcludeIfProcs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkExcludeIfProcs.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkExcludeIfProcs.Location = new System.Drawing.Point(45, 162);
			this.checkExcludeIfProcs.Name = "checkExcludeIfProcs";
			this.checkExcludeIfProcs.Size = new System.Drawing.Size(231, 18);
			this.checkExcludeIfProcs.TabIndex = 7;
			this.checkExcludeIfProcs.Text = "Exclude if unsent procedures";
			this.checkExcludeIfProcs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkIgnoreInPerson
			// 
			this.checkIgnoreInPerson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkIgnoreInPerson.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIgnoreInPerson.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIgnoreInPerson.Location = new System.Drawing.Point(2, 229);
			this.checkIgnoreInPerson.Name = "checkIgnoreInPerson";
			this.checkIgnoreInPerson.Size = new System.Drawing.Size(274, 18);
			this.checkIgnoreInPerson.TabIndex = 9;
			this.checkIgnoreInPerson.Text = "Ignore walkout (InPerson) statements";
			this.checkIgnoreInPerson.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelSaveDefaults
			// 
			this.labelSaveDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelSaveDefaults.Location = new System.Drawing.Point(7, 607);
			this.labelSaveDefaults.Name = "labelSaveDefaults";
			this.labelSaveDefaults.Size = new System.Drawing.Size(270, 16);
			this.labelSaveDefaults.TabIndex = 246;
			this.labelSaveDefaults.Text = "(except the date at the top)";
			this.labelSaveDefaults.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label7.Location = new System.Drawing.Point(5, 269);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(101, 16);
			this.label7.TabIndex = 245;
			this.label7.Text = "Billing Types";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.Location = new System.Drawing.Point(3, 75);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(128, 16);
			this.label6.TabIndex = 243;
			this.label6.Text = "Age of Account";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboAge
			// 
			this.comboAge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboAge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAge.FormattingEnabled = true;
			this.comboAge.Location = new System.Drawing.Point(132, 73);
			this.comboAge.Name = "comboAge";
			this.comboAge.Size = new System.Drawing.Size(145, 21);
			this.comboAge.TabIndex = 2;
			// 
			// checkExcludeInsPending
			// 
			this.checkExcludeInsPending.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkExcludeInsPending.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkExcludeInsPending.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkExcludeInsPending.Location = new System.Drawing.Point(45, 142);
			this.checkExcludeInsPending.Name = "checkExcludeInsPending";
			this.checkExcludeInsPending.Size = new System.Drawing.Size(231, 18);
			this.checkExcludeInsPending.TabIndex = 6;
			this.checkExcludeInsPending.Text = "Exclude if insurance pending";
			this.checkExcludeInsPending.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkIncludeChanged
			// 
			this.checkIncludeChanged.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkIncludeChanged.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeChanged.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIncludeChanged.Location = new System.Drawing.Point(3, 39);
			this.checkIncludeChanged.Name = "checkIncludeChanged";
			this.checkIncludeChanged.Size = new System.Drawing.Size(273, 28);
			this.checkIncludeChanged.TabIndex = 1;
			this.checkIncludeChanged.Text = "Include any accounts with insurance payments, procedures, or payplan charges sinc" +
    "e the last bill";
			this.checkIncludeChanged.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textLastStatement
			// 
			this.textLastStatement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textLastStatement.Location = new System.Drawing.Point(183, 13);
			this.textLastStatement.Name = "textLastStatement";
			this.textLastStatement.Size = new System.Drawing.Size(94, 20);
			this.textLastStatement.TabIndex = 0;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.Location = new System.Drawing.Point(6, 15);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(176, 16);
			this.label5.TabIndex = 24;
			this.label5.Text = "Include anyone not billed since";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkShowNegative
			// 
			this.checkShowNegative.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkShowNegative.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowNegative.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowNegative.Location = new System.Drawing.Point(45, 209);
			this.checkShowNegative.Name = "checkShowNegative";
			this.checkShowNegative.Size = new System.Drawing.Size(231, 18);
			this.checkShowNegative.TabIndex = 5;
			this.checkShowNegative.Text = "Show negative balances (credits)";
			this.checkShowNegative.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkBadAddress
			// 
			this.checkBadAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBadAddress.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkBadAddress.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBadAddress.Location = new System.Drawing.Point(45, 102);
			this.checkBadAddress.Name = "checkBadAddress";
			this.checkBadAddress.Size = new System.Drawing.Size(231, 18);
			this.checkBadAddress.TabIndex = 3;
			this.checkBadAddress.Text = "Exclude bad addresses (no zipcode)";
			this.checkBadAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// listBillType
			// 
			this.listBillType.Location = new System.Drawing.Point(107, 269);
			this.listBillType.Name = "listBillType";
			this.listBillType.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBillType.Size = new System.Drawing.Size(169, 225);
			this.listBillType.TabIndex = 10;
			// 
			// gridDun
			// 
			this.gridDun.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridDun.HasAddButton = false;
			this.gridDun.HasDropDowns = false;
			this.gridDun.HasMultilineHeaders = false;
			this.gridDun.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridDun.HeaderHeight = 15;
			this.gridDun.HScrollVisible = false;
			this.gridDun.Location = new System.Drawing.Point(331, 31);
			this.gridDun.Name = "gridDun";
			this.gridDun.ScrollValue = 0;
			this.gridDun.Size = new System.Drawing.Size(561, 366);
			this.gridDun.TabIndex = 1;
			this.gridDun.Title = "Dunning Messages";
			this.gridDun.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridDun.TitleHeight = 18;
			this.gridDun.TranslationName = "TableBillingMessages";
			this.gridDun.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridDun_CellDoubleClick);
			// 
			// butDunningSetup
			// 
			this.butDunningSetup.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDunningSetup.Autosize = true;
			this.butDunningSetup.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDunningSetup.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDunningSetup.CornerRadius = 4F;
			this.butDunningSetup.Location = new System.Drawing.Point(806, 403);
			this.butDunningSetup.Name = "butDunningSetup";
			this.butDunningSetup.Size = new System.Drawing.Size(86, 24);
			this.butDunningSetup.TabIndex = 2;
			this.butDunningSetup.Text = "Setup Dunning";
			this.butDunningSetup.Click += new System.EventHandler(this.butDunningSetup_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(328, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(564, 16);
			this.label3.TabIndex = 25;
			this.label3.Text = "Items higher in the list are more general.  Items lower in the list take preceden" +
    "ce .";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(328, 549);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(564, 16);
			this.label4.TabIndex = 26;
			this.label4.Text = "General Message (in addition to any dunning messages and appointment reminders, [" +
    "InstallmentPlanTerms] allowed)";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textNote
			// 
			this.textNote.AcceptsTab = true;
			this.textNote.BackColor = System.Drawing.SystemColors.Window;
			this.textNote.DetectLinksEnabled = false;
			this.textNote.DetectUrls = false;
			this.textNote.Location = new System.Drawing.Point(331, 568);
			this.textNote.Name = "textNote";
			this.textNote.QuickPasteType = OpenDentBusiness.QuickPasteType.Statement;
			this.textNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textNote.Size = new System.Drawing.Size(561, 102);
			this.textNote.TabIndex = 7;
			this.textNote.Text = "";
			// 
			// groupDateRange
			// 
			this.groupDateRange.Controls.Add(this.but30days);
			this.groupDateRange.Controls.Add(this.textDateStart);
			this.groupDateRange.Controls.Add(this.checkBoxBillShowTransSinceZero);
			this.groupDateRange.Controls.Add(this.labelStartDate);
			this.groupDateRange.Controls.Add(this.labelEndDate);
			this.groupDateRange.Controls.Add(this.textDateEnd);
			this.groupDateRange.Controls.Add(this.but45days);
			this.groupDateRange.Controls.Add(this.but90days);
			this.groupDateRange.Controls.Add(this.butDatesAll);
			this.groupDateRange.Location = new System.Drawing.Point(331, 403);
			this.groupDateRange.Name = "groupDateRange";
			this.groupDateRange.Size = new System.Drawing.Size(319, 86);
			this.groupDateRange.TabIndex = 3;
			this.groupDateRange.TabStop = false;
			this.groupDateRange.Text = "Account History Date Range";
			// 
			// but30days
			// 
			this.but30days.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.but30days.Autosize = true;
			this.but30days.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.but30days.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.but30days.CornerRadius = 4F;
			this.but30days.Location = new System.Drawing.Point(154, 13);
			this.but30days.Name = "but30days";
			this.but30days.Size = new System.Drawing.Size(77, 24);
			this.but30days.TabIndex = 2;
			this.but30days.Text = "Last 30 Days";
			this.but30days.Click += new System.EventHandler(this.but30days_Click);
			// 
			// textDateStart
			// 
			this.textDateStart.BackColor = System.Drawing.SystemColors.Window;
			this.textDateStart.ForeColor = System.Drawing.SystemColors.WindowText;
			this.textDateStart.Location = new System.Drawing.Point(75, 16);
			this.textDateStart.Name = "textDateStart";
			this.textDateStart.Size = new System.Drawing.Size(77, 20);
			this.textDateStart.TabIndex = 0;
			// 
			// labelStartDate
			// 
			this.labelStartDate.Location = new System.Drawing.Point(6, 19);
			this.labelStartDate.Name = "labelStartDate";
			this.labelStartDate.Size = new System.Drawing.Size(69, 14);
			this.labelStartDate.TabIndex = 221;
			this.labelStartDate.Text = "Start Date";
			this.labelStartDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelEndDate
			// 
			this.labelEndDate.Location = new System.Drawing.Point(6, 42);
			this.labelEndDate.Name = "labelEndDate";
			this.labelEndDate.Size = new System.Drawing.Size(69, 14);
			this.labelEndDate.TabIndex = 222;
			this.labelEndDate.Text = "End Date";
			this.labelEndDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDateEnd
			// 
			this.textDateEnd.Location = new System.Drawing.Point(75, 39);
			this.textDateEnd.Name = "textDateEnd";
			this.textDateEnd.Size = new System.Drawing.Size(77, 20);
			this.textDateEnd.TabIndex = 1;
			// 
			// but45days
			// 
			this.but45days.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.but45days.Autosize = true;
			this.but45days.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.but45days.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.but45days.CornerRadius = 4F;
			this.but45days.Location = new System.Drawing.Point(154, 37);
			this.but45days.Name = "but45days";
			this.but45days.Size = new System.Drawing.Size(77, 24);
			this.but45days.TabIndex = 3;
			this.but45days.Text = "Last 45 Days";
			this.but45days.Click += new System.EventHandler(this.but45days_Click);
			// 
			// but90days
			// 
			this.but90days.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.but90days.Autosize = true;
			this.but90days.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.but90days.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.but90days.CornerRadius = 4F;
			this.but90days.Location = new System.Drawing.Point(233, 37);
			this.but90days.Name = "but90days";
			this.but90days.Size = new System.Drawing.Size(77, 24);
			this.but90days.TabIndex = 5;
			this.but90days.Text = "Last 90 Days";
			this.but90days.Click += new System.EventHandler(this.but90days_Click);
			// 
			// butDatesAll
			// 
			this.butDatesAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDatesAll.Autosize = true;
			this.butDatesAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDatesAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDatesAll.CornerRadius = 4F;
			this.butDatesAll.Location = new System.Drawing.Point(233, 13);
			this.butDatesAll.Name = "butDatesAll";
			this.butDatesAll.Size = new System.Drawing.Size(77, 24);
			this.butDatesAll.TabIndex = 4;
			this.butDatesAll.Text = "All Dates";
			this.butDatesAll.Click += new System.EventHandler(this.butDatesAll_Click);
			// 
			// checkIntermingled
			// 
			this.checkIntermingled.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIntermingled.Location = new System.Drawing.Point(331, 493);
			this.checkIntermingled.Name = "checkIntermingled";
			this.checkIntermingled.Size = new System.Drawing.Size(231, 20);
			this.checkIntermingled.TabIndex = 5;
			this.checkIntermingled.Text = "Intermingle family members";
			this.checkIntermingled.Click += new System.EventHandler(this.checkIntermingled_Click);
			// 
			// butDefaults
			// 
			this.butDefaults.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDefaults.Autosize = true;
			this.butDefaults.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDefaults.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDefaults.CornerRadius = 4F;
			this.butDefaults.Location = new System.Drawing.Point(656, 440);
			this.butDefaults.Name = "butDefaults";
			this.butDefaults.Size = new System.Drawing.Size(76, 24);
			this.butDefaults.TabIndex = 4;
			this.butDefaults.Text = "Defaults";
			this.butDefaults.Click += new System.EventHandler(this.butDefaults_Click);
			// 
			// butUndo
			// 
			this.butUndo.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUndo.Autosize = true;
			this.butUndo.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUndo.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUndo.CornerRadius = 4F;
			this.butUndo.Location = new System.Drawing.Point(7, 676);
			this.butUndo.Name = "butUndo";
			this.butUndo.Size = new System.Drawing.Size(88, 24);
			this.butUndo.TabIndex = 10;
			this.butUndo.Text = "Undo Billing";
			this.butUndo.Click += new System.EventHandler(this.butUndo_Click);
			// 
			// checkSuperFam
			// 
			this.checkSuperFam.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSuperFam.Location = new System.Drawing.Point(331, 512);
			this.checkSuperFam.Name = "checkSuperFam";
			this.checkSuperFam.Size = new System.Drawing.Size(258, 20);
			this.checkSuperFam.TabIndex = 6;
			this.checkSuperFam.Text = "Group by Super Family";
			this.checkSuperFam.UseVisualStyleBackColor = true;
			this.checkSuperFam.CheckedChanged += new System.EventHandler(this.checkSuperFam_CheckedChanged);
			// 
			// checkSinglePatient
			// 
			this.checkSinglePatient.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSinglePatient.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSinglePatient.Location = new System.Drawing.Point(331, 531);
			this.checkSinglePatient.Name = "checkSinglePatient";
			this.checkSinglePatient.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkSinglePatient.Size = new System.Drawing.Size(219, 20);
			this.checkSinglePatient.TabIndex = 27;
			this.checkSinglePatient.Text = "Single patient only";
			this.checkSinglePatient.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSinglePatient.Visible = false;
			this.checkSinglePatient.Click += new System.EventHandler(this.checkSinglePatient_Click);
			// 
			// labelModesToText
			// 
			this.labelModesToText.Location = new System.Drawing.Point(563, 492);
			this.labelModesToText.Name = "labelModesToText";
			this.labelModesToText.Size = new System.Drawing.Size(195, 16);
			this.labelModesToText.TabIndex = 254;
			this.labelModesToText.Text = "Send text message for these modes";
			this.labelModesToText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// listModeToText
			// 
			this.listModeToText.FormattingEnabled = true;
			this.listModeToText.Location = new System.Drawing.Point(760, 492);
			this.listModeToText.Name = "listModeToText";
			this.listModeToText.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listModeToText.Size = new System.Drawing.Size(113, 56);
			this.listModeToText.TabIndex = 255;
			// 
			// labelMultiClinicGenMsg
			// 
			this.labelMultiClinicGenMsg.Location = new System.Drawing.Point(353, 609);
			this.labelMultiClinicGenMsg.Name = "labelMultiClinicGenMsg";
			this.labelMultiClinicGenMsg.Size = new System.Drawing.Size(517, 16);
			this.labelMultiClinicGenMsg.TabIndex = 256;
			this.labelMultiClinicGenMsg.Text = "Practice General Message or Clinic General message(s) will be used, filter for on" +
    "ly one clinic to see message";
			this.labelMultiClinicGenMsg.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.labelMultiClinicGenMsg.Visible = false;
			// 
			// FormBillingOptions
			// 
			this.AcceptButton = this.butCreate;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(898, 711);
			this.Controls.Add(this.labelMultiClinicGenMsg);
			this.Controls.Add(this.listModeToText);
			this.Controls.Add(this.labelModesToText);
			this.Controls.Add(this.checkSinglePatient);
			this.Controls.Add(this.checkSuperFam);
			this.Controls.Add(this.butUndo);
			this.Controls.Add(this.butDefaults);
			this.Controls.Add(this.checkIntermingled);
			this.Controls.Add(this.groupDateRange);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butCreate);
			this.Controls.Add(this.butDunningSetup);
			this.Controls.Add(this.gridDun);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(914,750);
			this.Name = "FormBillingOptions";
			this.ShowInTaskbar = false;
			this.Text = "Billing Options";
			this.Load += new System.EventHandler(this.FormBillingOptions_Load);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupDateRange.ResumeLayout(false);
			this.groupDateRange.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormBillingOptions_Load(object sender, System.EventArgs e) {
			textLastStatement.Text=DateTime.Today.AddMonths(-1).ToShortDateString();
			checkUseClinicDefaults.Visible=false;
			if(PrefC.HasClinicsEnabled) {
				RefreshClinicPrefs();
				labelSaveDefaults.Text="("+Lan.g(this,"except the date at the top and clinic at the bottom")+")";
				labelClinic.Visible=true;
				comboClinic.Visible=true;
				butPickClinic.Visible=true;
				//ClinicNum: -1 for All
				comboClinic.Items.Add(new ODBoxItem<Clinic>(Lan.g(this,"All"),new Clinic() {ClinicNum = -1,Abbr = "All",Description = "All"}));
				comboClinic.SetSelected(0,true); //select 'All' by default
				_listClinics=Clinics.GetForUserod(Security.CurUser);
				if(!Security.CurUser.ClinicIsRestricted) {
					_listClinics.Insert(0,new Clinic() { ClinicNum = 0,Abbr = "Unassigned",Description = "Unassigned" });
				}
			foreach(Clinic clinic in _listClinics) {
					comboClinic.Items.Add(new ODBoxItem<Clinic>(clinic.Abbr,clinic));
					if(ClinicNum!=0 && clinic.ClinicNum==ClinicNum) {//If ClinicNum=0 then maintain default All selection rather than Unassigned.
						comboClinic.SetSelected(false);
						comboClinic.SetSelected(comboClinic.Items.Count-1,true);
					}
				}
			}
			_listBillingTypeDefs=Defs.GetDefsForCategory(DefCat.BillingTypes,true);
			listBillType.Items.Add(Lan.g(this,"(all)"));
			listBillType.Items.AddRange(_listBillingTypeDefs.Select(x => x.ItemName).ToArray());
			comboAge.Items.Add(Lan.g(this,"Any Balance"));
			comboAge.Items.Add(Lan.g(this,"Over 30 Days"));
			comboAge.Items.Add(Lan.g(this,"Over 60 Days"));
			comboAge.Items.Add(Lan.g(this,"Over 90 Days"));
			listModeToText.Items.Clear();
			if(SmsPhones.IsIntegratedTextingEnabled()) {
				foreach(StatementMode stateMode in Enum.GetValues(typeof(StatementMode))) {
					listModeToText.Items.Add(new ODBoxItem<StatementMode>(Lan.g("enumStatementMode",stateMode.GetDescription()),stateMode));
				}
			}
			else {
				listModeToText.Visible=false;
				labelModesToText.Visible=false;
			}
			SetFiltersForClinicNums(GetSelectedClinicNums());
			if(!PrefC.HasSuperStatementsEnabled) {
				checkSuperFam.Visible=false;
			}
			//blank is allowed
			FillDunning();
			SetDefaults();
		}

		///<summary>Call when you want to populate/update _dicClinicPrefsOld and _dicClinicPrefsNew.</summary>
		private void RefreshClinicPrefs() {
			List<PrefName> listBillingPrefs=new List<PrefName>() {
				PrefName.BillingIncludeChanged,
				PrefName.BillingSelectBillingTypes,
				PrefName.BillingAgeOfAccount,
				PrefName.BillingExcludeBadAddresses,
				PrefName.BillingExcludeInactive,
				PrefName.BillingExcludeNegative,
				PrefName.BillingExcludeInsPending,
				PrefName.BillingExcludeIfUnsentProcs,
				PrefName.BillingExcludeLessThan,
				PrefName.BillingIgnoreInPerson,
				PrefName.BillingShowTransSinceBalZero,
				PrefName.BillingDefaultsNote
			};
			_dictClinicPrefsOld=ClinicPrefs.GetWhere(x => listBillingPrefs.Contains(x.PrefName))
				.GroupBy(x => x.ClinicNum)
				.ToDictionary(x => x.Key,x => x.ToList());
			_dictClinicPrefsNew=_dictClinicPrefsOld.ToDictionary(x => x.Key,x => x.Value.Select(y => y.Clone()).ToList());
			//Originally all ClinicPrefs were inserted together, so you either had none or all.
			//We have since added new ClincPrefs to this list so we need to identify missing clinicPrefs
			//Missing ClincPrefs will default to the standard preference table value.
			foreach(long clincNum in _dictClinicPrefsNew.Keys) {
				List<PrefName> listExistingClinicPrefs=_dictClinicPrefsNew[clincNum].Select(x => x.PrefName).ToList();
				List<PrefName> listMissingClincPrefs=listBillingPrefs.FindAll(x => !listExistingClinicPrefs.Contains(x));
				foreach(PrefName prefName in listMissingClincPrefs) {
					switch(prefName.GetAttributeOrDefault<PrefNameAttribute>().ValueType) {
						case PrefValueType.BOOL:
							bool defaultBool=PrefC.GetBool(prefName);
							_dictClinicPrefsNew[clincNum].Add(new ClinicPref(clincNum,prefName,defaultBool));
						break;
						case PrefValueType.ENUM:
							//Currently not used.
						break;
						case PrefValueType.STRING:
							string defaultStr=PrefC.GetString(prefName);
							_dictClinicPrefsNew[clincNum].Add(new ClinicPref(clincNum,prefName,defaultStr));
						break;
					}
				}
			}
		}
		
		///<summary>Called when we need to update the filter options.
		///If All, the unassigned clinic, or more than one clinic is selected, or _dicClinicPrefsNew does not 
		///contain a key for the current selected clinic, the standard preference based defaults will load.</summary>
		private void SetFiltersForClinicNums(List<long> listClinicNums,bool isTextNoteExcluded=false) {
			FillDunning();
			if(listClinicNums.Count != 1 || listClinicNums.Contains(-1) || listClinicNums.Contains(0) 
				|| !_dictClinicPrefsNew.ContainsKey(listClinicNums[0]))//They have not saved their default filter options for the selected clinic. Use default prefs.
			{
				checkIncludeChanged.Checked=PrefC.GetBool(PrefName.BillingIncludeChanged);
				#region BillTypes
				listBillType.ClearSelected();
				string[] selectedBillTypes=PrefC.GetString(PrefName.BillingSelectBillingTypes).Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries);
				foreach(string billTypeDefNum in selectedBillTypes) {
					try{
						int order=Defs.GetOrder(DefCat.BillingTypes,Convert.ToInt64(billTypeDefNum));
						if(order==-1) {
							continue;
						}
						listBillType.SetSelected(order+1,true);
					}
					catch(Exception) {//cannot convert string to int, just continue
						continue;
					}
				}
				if(listBillType.SelectedIndices.Count==0){
					listBillType.SelectedIndex=0;
				}
				#endregion
				#region Age
				switch(PrefC.GetString(PrefName.BillingAgeOfAccount)){
					default:
						comboAge.SelectedIndex=0;
						break;
					case "30":
						comboAge.SelectedIndex=1;
						break;
					case "60":
						comboAge.SelectedIndex=2;
						break;
					case "90":
						comboAge.SelectedIndex=3;
						break;
				}
				#endregion
				checkBadAddress.Checked=PrefC.GetBool(PrefName.BillingExcludeBadAddresses);
				checkExcludeInactive.Checked=PrefC.GetBool(PrefName.BillingExcludeInactive);
				checkShowNegative.Checked=!PrefC.GetBool(PrefName.BillingExcludeNegative);
				checkExcludeInsPending.Checked=PrefC.GetBool(PrefName.BillingExcludeInsPending);
				checkExcludeIfProcs.Checked=PrefC.GetBool(PrefName.BillingExcludeIfUnsentProcs);
				textExcludeLessThan.Text=PrefC.GetString(PrefName.BillingExcludeLessThan);
				checkIgnoreInPerson.Checked=PrefC.GetBool(PrefName.BillingIgnoreInPerson);
				checkBoxBillShowTransSinceZero.Checked=PrefC.GetBool(PrefName.BillingShowTransSinceBalZero);
				if(!isTextNoteExcluded) {
					textNote.Text=PrefC.GetString(PrefName.BillingDefaultsNote);
				}
				return;
			}
			else {//Update filter UI to reflect ClinicPrefs. //there has to be ONE item in ClinicNums. It MUST be in the dictionary and it MUST NOT be -1 or 0.
				List<ClinicPref> listClinicPrefs=_dictClinicPrefsNew[listClinicNums[0]];//By definition of how ClinicPrefs are created, First will always return a result.
				checkIncludeChanged.Checked=PIn.Bool(listClinicPrefs.First(x => x.PrefName==PrefName.BillingIncludeChanged).ValueString);
				#region BillTypes
				listBillType.ClearSelected();
				string[] selectedBillTypes=listClinicPrefs.First(x => x.PrefName==PrefName.BillingSelectBillingTypes).ValueString.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries);
				foreach(string billTypeDefNum in selectedBillTypes) {
					try {
						int order=Defs.GetOrder(DefCat.BillingTypes,Convert.ToInt64(billTypeDefNum));
						if(order==-1) {
							continue;
						}
						listBillType.SetSelected(order+1,true);
					}
					catch(Exception) {//cannot convert string to int, just continue
						continue;
					}
				}
				if(listBillType.SelectedIndices.Count==0) {
					listBillType.SelectedIndex=0;
				}
				#endregion
				#region Age
				switch(listClinicPrefs.First(x => x.PrefName==PrefName.BillingAgeOfAccount).ValueString) {
					default:
						comboAge.SelectedIndex=0;
						break;
					case "30":
						comboAge.SelectedIndex=1;
						break;
					case "60":
						comboAge.SelectedIndex=2;
						break;
					case "90":
						comboAge.SelectedIndex=3;
						break;
				}
				#endregion
				checkBadAddress.Checked=PIn.Bool(listClinicPrefs.First(x => x.PrefName==PrefName.BillingExcludeBadAddresses).ValueString);
				checkExcludeInactive.Checked=PIn.Bool(listClinicPrefs.First(x => x.PrefName==PrefName.BillingExcludeInactive).ValueString);
				checkShowNegative.Checked=!PIn.Bool(listClinicPrefs.First(x => x.PrefName==PrefName.BillingExcludeNegative).ValueString);
				checkExcludeInsPending.Checked=PIn.Bool(listClinicPrefs.First(x => x.PrefName==PrefName.BillingExcludeInsPending).ValueString);
				checkExcludeIfProcs.Checked=PIn.Bool(listClinicPrefs.First(x => x.PrefName==PrefName.BillingExcludeIfUnsentProcs).ValueString);
				textExcludeLessThan.Text=listClinicPrefs.First(x => x.PrefName==PrefName.BillingExcludeLessThan).ValueString;
				checkIgnoreInPerson.Checked=PIn.Bool(listClinicPrefs.First(x => x.PrefName==PrefName.BillingIgnoreInPerson).ValueString);
				checkBoxBillShowTransSinceZero.Checked=PIn.Bool(listClinicPrefs.First(x => x.PrefName==PrefName.BillingShowTransSinceBalZero).ValueString);
				if(!isTextNoteExcluded) {
					textNote.Text=listClinicPrefs.First(x => x.PrefName==PrefName.BillingDefaultsNote).ValueString;
				}
			}
		}

		///<summary>Returns the ClinicNums for the selected clinics in comboClinic.
		///If clinics are not enabled, returns an empty list.
		///If All is selected, returns -1.
		///If Unassigned is selected, returns 0.</summary>
		private List<long> GetSelectedClinicNums() {
			if(!PrefC.HasClinicsEnabled) {
				return new List<long>();
			}
			return comboClinic.ListSelectedItems.OfType<ODBoxItem<Clinic>>().Select(x => x.Tag.ClinicNum).ToList();
		}

		private void butSaveDefault_Click(object sender,System.EventArgs e) {
			if(textExcludeLessThan.errorProvider1.GetError(textExcludeLessThan)!=""
				|| textLastStatement.errorProvider1.GetError(textLastStatement)!=""
				)
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			if(listBillType.SelectedIndices.Count==0) {
				MsgBox.Show(this,"Please select at least one billing type first.");
				return;
			}
			string selectedBillingTypes="";//indicates all.
			if(listBillType.SelectedIndices.Count>0 && !listBillType.SelectedIndices.Contains(0)) {
				selectedBillingTypes=string.Join(",",listBillType.SelectedIndices.OfType<int>().Select(x => _listBillingTypeDefs[x-1].DefNum));
			}
			string ageOfAccount="";
			if(comboAge.SelectedIndex.In(1,2,3)) {
				ageOfAccount=(30*comboAge.SelectedIndex).ToString();//ageOfAccount is 30, 60, or 90
			}
			List<long> listClinicNums = GetSelectedClinicNums();
			if(listClinicNums.Count != 1 || listClinicNums.Contains(-1) || listClinicNums.Contains(0))//Clinics not enabled or 'All' selected or 'Unassigned' selected.
			{
				if(Prefs.UpdateBool(PrefName.BillingIncludeChanged,checkIncludeChanged.Checked)
					|Prefs.UpdateString(PrefName.BillingSelectBillingTypes,selectedBillingTypes)
					|Prefs.UpdateString(PrefName.BillingAgeOfAccount,ageOfAccount)
					|Prefs.UpdateBool(PrefName.BillingExcludeBadAddresses,checkBadAddress.Checked)
					|Prefs.UpdateBool(PrefName.BillingExcludeInactive,checkExcludeInactive.Checked)
					|Prefs.UpdateBool(PrefName.BillingExcludeNegative,!checkShowNegative.Checked)
					|Prefs.UpdateBool(PrefName.BillingExcludeInsPending,checkExcludeInsPending.Checked)
					|Prefs.UpdateBool(PrefName.BillingExcludeIfUnsentProcs,checkExcludeIfProcs.Checked)
					|Prefs.UpdateString(PrefName.BillingExcludeLessThan,textExcludeLessThan.Text)
					|Prefs.UpdateBool(PrefName.BillingIgnoreInPerson,checkIgnoreInPerson.Checked)
					|Prefs.UpdateString(PrefName.BillingDefaultsNote,textNote.Text))
				{
					DataValid.SetInvalid(InvalidType.Prefs);
				}
				return;
			}
			else if(_dictClinicPrefsNew.Keys.Contains(listClinicNums[0])) {//ClincPrefs exist, update them.
				_dictClinicPrefsNew[listClinicNums[0]].First(x => x.PrefName==PrefName.BillingIncludeChanged).ValueString=POut.Bool(checkIncludeChanged.Checked);
				_dictClinicPrefsNew[listClinicNums[0]].First(x => x.PrefName==PrefName.BillingSelectBillingTypes).ValueString=selectedBillingTypes;
				_dictClinicPrefsNew[listClinicNums[0]].First(x => x.PrefName==PrefName.BillingAgeOfAccount).ValueString=ageOfAccount;
				_dictClinicPrefsNew[listClinicNums[0]].First(x => x.PrefName==PrefName.BillingExcludeBadAddresses).ValueString=POut.Bool(checkBadAddress.Checked);
				_dictClinicPrefsNew[listClinicNums[0]].First(x => x.PrefName==PrefName.BillingExcludeInactive).ValueString=POut.Bool(checkExcludeInactive.Checked);
				_dictClinicPrefsNew[listClinicNums[0]].First(x => x.PrefName==PrefName.BillingExcludeNegative).ValueString=POut.Bool(!checkShowNegative.Checked);
				_dictClinicPrefsNew[listClinicNums[0]].First(x => x.PrefName==PrefName.BillingExcludeInsPending).ValueString=POut.Bool(checkExcludeInsPending.Checked);
				_dictClinicPrefsNew[listClinicNums[0]].First(x => x.PrefName==PrefName.BillingExcludeIfUnsentProcs).ValueString=POut.Bool(checkExcludeIfProcs.Checked);
				_dictClinicPrefsNew[listClinicNums[0]].First(x => x.PrefName==PrefName.BillingExcludeLessThan).ValueString=textExcludeLessThan.Text;
				_dictClinicPrefsNew[listClinicNums[0]].First(x => x.PrefName==PrefName.BillingIgnoreInPerson).ValueString=POut.Bool(checkIgnoreInPerson.Checked);
				_dictClinicPrefsNew[listClinicNums[0]].First(x => x.PrefName==PrefName.BillingShowTransSinceBalZero).ValueString=POut.Bool(checkBoxBillShowTransSinceZero.Checked);
				_dictClinicPrefsNew[listClinicNums[0]].First(x => x.PrefName==PrefName.BillingDefaultsNote).ValueString=textNote.Text;
			}
			else {//No existing ClinicPrefs for the currently selected clinic in comboClinics.
				_dictClinicPrefsNew.Add(listClinicNums[0],new List<ClinicPref>());
				//Inserts new ClinicPrefs for each of the Filter options for the currently selected clinic.
				_dictClinicPrefsNew[listClinicNums[0]].Add(new ClinicPref(listClinicNums[0],PrefName.BillingIncludeChanged,checkIncludeChanged.Checked));
				_dictClinicPrefsNew[listClinicNums[0]].Add(new ClinicPref(listClinicNums[0],PrefName.BillingSelectBillingTypes,selectedBillingTypes));
				_dictClinicPrefsNew[listClinicNums[0]].Add(new ClinicPref(listClinicNums[0],PrefName.BillingAgeOfAccount,ageOfAccount));
				_dictClinicPrefsNew[listClinicNums[0]].Add(new ClinicPref(listClinicNums[0],PrefName.BillingExcludeBadAddresses,checkBadAddress.Checked));
				_dictClinicPrefsNew[listClinicNums[0]].Add(new ClinicPref(listClinicNums[0],PrefName.BillingExcludeInactive,checkExcludeInactive.Checked));
				_dictClinicPrefsNew[listClinicNums[0]].Add(new ClinicPref(listClinicNums[0],PrefName.BillingExcludeNegative,!checkShowNegative.Checked));
				_dictClinicPrefsNew[listClinicNums[0]].Add(new ClinicPref(listClinicNums[0],PrefName.BillingExcludeInsPending,checkExcludeInsPending.Checked));
				_dictClinicPrefsNew[listClinicNums[0]].Add(new ClinicPref(listClinicNums[0],PrefName.BillingExcludeIfUnsentProcs,checkExcludeIfProcs.Checked));
				_dictClinicPrefsNew[listClinicNums[0]].Add(new ClinicPref(listClinicNums[0],PrefName.BillingExcludeLessThan,textExcludeLessThan.Text));
				_dictClinicPrefsNew[listClinicNums[0]].Add(new ClinicPref(listClinicNums[0],PrefName.BillingIgnoreInPerson,checkIgnoreInPerson.Checked));
				_dictClinicPrefsNew[listClinicNums[0]].Add(new ClinicPref(listClinicNums[0],PrefName.BillingShowTransSinceBalZero,checkBoxBillShowTransSinceZero.Checked));
				_dictClinicPrefsNew[listClinicNums[0]].Add(new ClinicPref(listClinicNums[0],PrefName.BillingDefaultsNote,textNote.Text));
			}
			if(ClinicPrefs.Sync(_dictClinicPrefsNew.SelectMany(x => x.Value).ToList(),_dictClinicPrefsOld.SelectMany(x => x.Value).ToList())) {
				DataValid.SetInvalid(InvalidType.ClinicPrefs);
				RefreshClinicPrefs();
			}
		}

		private void SetClinicFilters() {
			if(GetSelectedClinicNums().Count > 1) {
				butSaveDefault.Enabled=false;
			}
			else {
				butSaveDefault.Enabled=true;
			}
			checkUseClinicDefaults.Visible=(GetSelectedClinicNums().Contains(-1) || GetSelectedClinicNums().Count > 1);//Only visible when 'All' or multiple clinics are selected in comboClinic.
			List<long> listSelectedClinicNums=GetSelectedClinicNums();
			UpdateGenMsgUI(listSelectedClinicNums);
			SetFiltersForClinicNums(listSelectedClinicNums);//When textNote is visible then we are allowing overrides.
		}

		private void UpdateGenMsgUI(List<long> listSelectedClinicNums) {
			//-1 such that 'All' selected or multiple clinics selected.
			labelMultiClinicGenMsg.Visible=(checkUseClinicDefaults.Checked && (listSelectedClinicNums.Contains(-1) || listSelectedClinicNums.Count>1));
			textNote.Visible=(!labelMultiClinicGenMsg.Visible);//Hide if labelMultiClinicGenMsg is visible, allows text to still be set.
		}

		private void comboClinic_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboClinic.Items.Count==0) {//combo box has no been initialized yet.
				return;
			}
			SetClinicFilters();
		}

		private void butPickClinic_Click(object sender,EventArgs e) {
			FormClinics FormC = new FormClinics();
			FormC.IsSelectionMode=true;
			FormC.IsMultiSelect=true;
			FormC.ListClinics=_listClinics;//To include 'Unassigned'
			FormC.ListSelectedClinicNums=comboClinic.ListSelectedItems.OfType<ODBoxItem<Clinic>>().Select(x => x.Tag.ClinicNum).ToList();
			FormC.ShowDialog();
			if(FormC.DialogResult==DialogResult.OK) {
				comboClinic.SetSelected(false);
				foreach(long clinCur in FormC.ListSelectedClinicNums) {
					for(int i = 0;i < comboClinic.Items.Count;i++) {
						long comboClinicNum = ((ODBoxItem<Clinic>)(comboClinic.Items[i])).Tag.ClinicNum;
						if(clinCur == comboClinicNum) {
							comboClinic.SetSelected(i,true);
						}
					}
				}
				SetClinicFilters();
			}
		}
		
		private void checkUseClinicDefaults_CheckedChanged(object sender,EventArgs e) {
			UpdateGenMsgUI(GetSelectedClinicNums());
		}

		private void FillDunning(){
			List<long> listClinicNums=GetSelectedClinicNums();
			if(listClinicNums.Contains(-1)) {//All
				listClinicNums=new List<long>();//Empty list to allow query to run for all clinics.
			}
			else {
				//Running for specific clinic(s), can include unassigned (0).
			}
			_listDunnings=Dunnings.Refresh(listClinicNums);
			gridDun.BeginUpdate();
			gridDun.Columns.Clear();
			ODGridColumn col=new ODGridColumn("Billing Type",80);
			gridDun.Columns.Add(col);
			col=new ODGridColumn("Aging",70);
			gridDun.Columns.Add(col);
			col=new ODGridColumn("Ins",40);
			gridDun.Columns.Add(col);
			col=new ODGridColumn("Message",150);
			gridDun.Columns.Add(col);
			col=new ODGridColumn("Bold Message",150);
			gridDun.Columns.Add(col);
			col=new ODGridColumn("Email",30, HorizontalAlignment.Center);
			gridDun.Columns.Add(col);
			gridDun.Rows.Clear();
			ODGridRow row;
			foreach(Dunning dunnCur in _listDunnings) {
				row=new ODGridRow();
				if(dunnCur.BillingType==0){
					row.Cells.Add(Lan.g(this,"all"));
				}
				else{
					row.Cells.Add(Defs.GetName(DefCat.BillingTypes,dunnCur.BillingType));
				}
				if(dunnCur.AgeAccount==0){
					row.Cells.Add(Lan.g(this,"any"));
				}
				else{
					row.Cells.Add(Lan.g(this,"Over ")+dunnCur.AgeAccount.ToString());
				}
				if(dunnCur.InsIsPending==YN.Yes) {
					row.Cells.Add(Lan.g(this,"Y"));
				}
				else if(dunnCur.InsIsPending==YN.No) {
					row.Cells.Add(Lan.g(this,"N"));
				}
				else {//YN.Unknown
					row.Cells.Add(Lan.g(this,"any"));
				}
				row.Cells.Add(dunnCur.DunMessage);
				row.Cells.Add(new ODGridCell(dunnCur.MessageBold) { Bold=YN.Yes,ColorText=Color.DarkRed });
				if(dunnCur.EmailBody!="" || dunnCur.EmailSubject!="") {
					row.Cells.Add("X");
				}
				else {
					row.Cells.Add("");
				}
				gridDun.Rows.Add(row);
			}
			gridDun.EndUpdate();
		}

		private void gridDun_CellDoubleClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			FormDunningEdit formD=new FormDunningEdit(_listDunnings[e.Row]);
			formD.ShowDialog();
			FillDunning();
		}

		private void checkSuperFam_CheckedChanged(object sender,EventArgs e) {
			if(checkSuperFam.Checked) {
				checkIntermingled.Checked=false;
				checkIntermingled.Enabled=false;
				checkSinglePatient.Checked=false;
			}
			else {
				checkIntermingled.Enabled=true;
			}
		}

		private void butDunningSetup_Click(object sender, System.EventArgs e) {
			FormDunningSetup formSetup=new FormDunningSetup();
			formSetup.ShowDialog();
			FillDunning();
		}

		private void butDefaults_Click(object sender,EventArgs e) {
			FormBillingDefaults FormB=new FormBillingDefaults();
			FormB.ShowDialog();
			if(FormB.DialogResult==DialogResult.OK){
				SetDefaults();
			}
		}

		private void SetDefaults(){
			textDateStart.Text=DateTime.Today.AddDays(-PrefC.GetLong(PrefName.BillingDefaultsLastDays)).ToShortDateString();
			textDateEnd.Text=DateTime.Today.ToShortDateString();
			checkIntermingled.Checked=PrefC.GetBool(PrefName.BillingDefaultsIntermingle);
			checkSinglePatient.Checked=PrefC.GetBool(PrefName.BillingDefaultsSinglePatient);
			if(SmsPhones.IsIntegratedTextingEnabled()) {
				foreach(string modeIdx in PrefC.GetString(PrefName.BillingDefaultsModesToText)
					.Split(new string[] { "," },StringSplitOptions.RemoveEmptyEntries)) {
					listModeToText.SetSelected(PIn.Int(modeIdx),true);
				}
			}
		}

		private void checkIntermingled_Click(object sender,EventArgs e) {
			if(checkIntermingled.Checked) {
				checkSinglePatient.Checked=false;
			}
		}

		private void checkSinglePatient_Click(object sender,EventArgs e) {
			if(checkSinglePatient.Checked) {
				checkIntermingled.Checked=false;
				checkSuperFam.Checked=false;
			}
		}

		private void checkBoxBillShowTransSinceZero_CheckedChanged(object sender,EventArgs e) {
			if(checkBoxBillShowTransSinceZero.Checked) {
				textDateStart.Enabled=false;
				textDateEnd.Enabled=false;
			}
			else {
				textDateStart.Enabled=true;
				textDateEnd.Enabled=true;
			}
		}

		private void but30days_Click(object sender,EventArgs e) {
			SetAccountHistoryControl();
			textDateStart.Text=DateTime.Today.AddDays(-30).ToShortDateString();
			textDateEnd.Text=DateTime.Today.ToShortDateString();
		}

		private void but45days_Click(object sender,EventArgs e) {
			SetAccountHistoryControl();
			textDateStart.Text=DateTime.Today.AddDays(-45).ToShortDateString();
			textDateEnd.Text=DateTime.Today.ToShortDateString();
		}

		private void but90days_Click(object sender,EventArgs e) {
			SetAccountHistoryControl();
			textDateStart.Text=DateTime.Today.AddDays(-90).ToShortDateString();
			textDateEnd.Text=DateTime.Today.ToShortDateString();
		}

		private void butDatesAll_Click(object sender,EventArgs e) {
			SetAccountHistoryControl();
			textDateStart.Text="";
			textDateEnd.Text=DateTime.Today.ToShortDateString();
		}

		private void SetAccountHistoryControl() {
			checkBoxBillShowTransSinceZero.Checked=false;
		}

		private void butUndo_Click(object sender,EventArgs e) {
			MsgBox.Show(this,"When the billing list comes up, use the radio button at the top to show the 'Sent' bills.\r\nThen, change their status back to unsent.\r\nYou can edit them as a group using the button at the right.");
			DialogResult=DialogResult.OK;//will trigger ContrStaff to bring up the billing window
		}

		private bool RunAgingEnterprise() {
			DateTime dtNow=MiscData.GetNowDateTime();
			DateTime dtToday=dtNow.Date;
			DateTime dateLastAging=PrefC.GetDate(PrefName.DateLastAging);
			if(dateLastAging.Date==dtToday) {
				return true;//already ran aging for this date, just move on
			}
			Prefs.RefreshCache();
			DateTime dateTAgingBeganPref=PrefC.GetDateT(PrefName.AgingBeginDateTime);
			if(dateTAgingBeganPref>DateTime.MinValue) {
				MessageBox.Show(this,Lan.g(this,"In order to create statments, aging must be calculated, but you cannot run aging until it has finished the "
					+"current calculations which began on")+" "+dateTAgingBeganPref.ToString()+".\r\n"+Lans.g(this,"If you believe the current aging process "
					+"has finished, a user with SecurityAdmin permission can manually clear the date and time by going to Setup | Miscellaneous and pressing "
					+"the 'Clear' button."));
				return false;
			}
			Prefs.UpdateString(PrefName.AgingBeginDateTime,POut.DateT(dtNow,false));//get lock on pref to block others
			Signalods.SetInvalid(InvalidType.Prefs);//signal a cache refresh so other computers will have the updated pref as quickly as possible
			Action actionCloseAgingProgress=null;
			Cursor=Cursors.WaitCursor;
			try {
				actionCloseAgingProgress=ODProgressOld.ShowProgressStatus("ComputeAging",this,
					Lan.g(this,"Calculating enterprise aging for all patients as of")+" "+dtToday.ToShortDateString()+"...");
				Ledgers.ComputeAging(0,dtToday);
				Prefs.UpdateString(PrefName.DateLastAging,POut.Date(dtToday,false));
			}
			catch(MySqlException ex) {
				actionCloseAgingProgress?.Invoke();//terminates progress bar
				Cursor=Cursors.Default;
				if(ex==null || ex.Number!=1213) {//not a deadlock error, just throw
					throw;
				}
				MsgBox.Show(this,"Deadlock error detected in aging transaction and rolled back. Try again later.");
				return false;
			}
			finally {
				actionCloseAgingProgress?.Invoke();//terminates progress bar
				Cursor=Cursors.Default;
				Prefs.UpdateString(PrefName.AgingBeginDateTime,"");//clear lock on pref whether aging was successful or not
				Signalods.SetInvalid(InvalidType.Prefs);
			}
			return true;
		}

		private void butCreate_Click(object sender, System.EventArgs e) {
			if( textExcludeLessThan.errorProvider1.GetError(textExcludeLessThan)!=""
				|| textLastStatement.errorProvider1.GetError(textLastStatement)!=""
				|| textDateStart.errorProvider1.GetError(textDateStart)!=""
				|| textDateEnd.errorProvider1.GetError(textDateEnd)!=""
				)
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			Action actionCloseAgingProgress=null;
			if(PrefC.GetBool(PrefName.AgingIsEnterprise)) {
				if(!RunAgingEnterprise()) {
					return;
				}
			}
			else if(!PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)) {
				try {
					DateTime asOfDate=(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)?PrefC.GetDate(PrefName.DateLastAging):DateTime.Today);
					actionCloseAgingProgress=ODProgressOld.ShowProgressStatus("ComputeAging",this,Lan.g(this,"Calculating aging for all patients as of")+" "
						+asOfDate.ToShortDateString()+"...");
					Cursor=Cursors.WaitCursor;
					Ledgers.RunAging();
				}
				catch(MySqlException ex) {
					actionCloseAgingProgress?.Invoke();//terminates progress bar
					Cursor=Cursors.Default;
					if(ex==null || ex.Number!=1213) {//not a deadlock error, just throw
						throw;
					}
					MsgBox.Show(this,"Deadlock error detected in aging transaction and rolled back. Try again later.");
					return;
				}
				finally {
					actionCloseAgingProgress?.Invoke();//terminates progress bar
					Cursor=Cursors.Default;
				}
			}
			Cursor=Cursors.WaitCursor;
			//All places in the program that have the ability to run aging against the entire database require the Setup permission because it can take a long time.
			if(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily) 
				&& Security.IsAuthorized(Permissions.Setup,true)
				&& PrefC.GetDate(PrefName.DateLastAging) < DateTime.Today.AddDays(-15)) 
			{
				MsgBox.Show(this,"Last aging date seems old, so you will now be given a chance to update it.  The billing process will continue whether or not aging gets updated.");
				FormAging FormA=new FormAging();
				FormA.BringToFront();
				FormA.ShowDialog();
			}
			if(GetSelectedClinicNums().Contains(-1)) {//'All' selected
				CreateManyHelper();
			}
			else if(GetSelectedClinicNums().Count > 1) { //'All' is not selected and there are multiple clinics selected.
				CreateManyHelper(GetSelectedClinicNums());
			}
			else if(GetSelectedClinicNums().Count == 1) { //'All' is not selected and one clinic is selected.
				CreateHelper(GetSelectedClinicNums()[0]);
			}
			else { //Clinics are not enabled.
				CreateHelper(-2);
			}
			Cursor=Cursors.Default;
			DialogResult=DialogResult.OK;
		}

		///<summary>Used when clinics are enabled and either multiple clinics are selected or 'All' is selected.</summary>
		private void CreateManyHelper(List<long> listClinicNums=null) {
			if(listClinicNums==null || listClinicNums.Contains(-1)) { //'All' is selected
				listClinicNums=_listClinics.Select(x => x.ClinicNum).ToList();
			}
			_popUpMessage="";
			foreach(long clinicNum in listClinicNums) {
				CreateHelper(clinicNum,checkUseClinicDefaults.Checked,true);
			}
			if(string.IsNullOrEmpty(_popUpMessage)) {
				return;
			}
			MsgBoxCopyPaste msgBox=new MsgBoxCopyPaste(_popUpMessage);
			msgBox.Text=Lan.g(this,"Billing List Results");
			msgBox.ShowDialog();
		}
		
		///<summary></summary>
		private void CreateHelper(long clinicNum, bool useClinicPrefs=false, bool suppressPopup=false) {
			if(useClinicPrefs) {//Clincs must be enabled.
				//If textNote is visible then one of the following must be true;
				//- Clinics are not enabled (does not apply since useClincPrefs is only true when using clinics)
				//- Clinics enabled, 'All' is NOT selected
				//- Clinics enabled, single clinic or unassigned is selected
				//If textNote is visible then we must be overriding the PrefName.BillingDefaultsNote with whatever is currently typed
				//,so do not update/change textNote in SetFiltersForClinicNums(...).
				SetFiltersForClinicNums(new List<long>() { clinicNum },textNote.Visible);
			}
			else {
				//Maintain current filter selections.
			}
			List<long> listClinicNums=new List<long>();
			if(PrefC.HasClinicsEnabled && clinicNum >= 0) {
				listClinicNums.Add(clinicNum);
			}
			DateTime lastStatement=PIn.Date(textLastStatement.Text);
			if(textLastStatement.Text=="") {
				lastStatement=DateTimeOD.Today;
			}
			string getAge="";
			if(comboAge.SelectedIndex==1) getAge="30";
			else if(comboAge.SelectedIndex==2) getAge="60";
			else if(comboAge.SelectedIndex==3) getAge="90";
			List<long> billingNums=new List<long>();//[listBillType.SelectedIndices.Count];
			for(int i=0;i<listBillType.SelectedIndices.Count;i++){
				if(listBillType.SelectedIndices[i]==0){//if (all) is selected, then ignore any other selections
					billingNums.Clear();
					break;
				}
				billingNums.Add(_listBillingTypeDefs[listBillType.SelectedIndices[i]-1].DefNum);
			}
			List<PatAging> listPatAging=new List<PatAging>();
			try {
				listPatAging=Patients.GetAgingList(getAge,lastStatement,billingNums,checkBadAddress.Checked,
					!checkShowNegative.Checked,PIn.Double(textExcludeLessThan.Text),
					checkExcludeInactive.Checked,checkIncludeChanged.Checked,checkExcludeInsPending.Checked,
					checkExcludeIfProcs.Checked,checkIgnoreInPerson.Checked,listClinicNums,checkSuperFam.Checked,checkSinglePatient.Checked);
			}
			catch (Exception ex){
				string text=Lan.g(this,"Error getting list:")+" "+ex.Message+"\r\n\n\n"+ex.StackTrace;
				if(ex.InnerException!=null) {
					text+="\r\n\r\nInner Exception: "+ex.InnerException.Message+"\r\n\r\n"+ex.InnerException.StackTrace;
				}
				MsgBoxCopyPaste msgBox=new MsgBoxCopyPaste(text);
				msgBox.ShowDialog();
				return;
			}
			List<Patient> listSuperHeadPats=new List<Patient>();
			//If making a super family bill, we need to manipulate the agingList to only contain super family head members.
			//It can also contain regular family members, but should not contain any individual super family members other than the head.
			if(checkSuperFam.Checked) {
				List<PatAging> listSuperAgings=new List<PatAging>();
				for(int i=listPatAging.Count-1;i>=0;i--) {//Go through each PatAging in the retrieved list
					if(listPatAging[i].SuperFamily==0 || !listPatAging[i].HasSuperBilling) {
						continue;//It is okay to leave non-super billing PatAgings in the list.
					}
					Patient superHead=listSuperHeadPats.FirstOrDefault(x => x.PatNum==listPatAging[i].SuperFamily);
					if(superHead==null) {
						superHead=Patients.GetPat(listPatAging[i].SuperFamily);
						listSuperHeadPats.Add(superHead);
					}
					if(!superHead.HasSuperBilling) {
						listPatAging[i].HasSuperBilling=false;//Family guarantor has super billing but superhead doesn't, so no super bill.  Mark statement as no superbill.
						continue;
					}
					//If the guar has super billing enabled and the superhead has superbilling, this entry needs to be merged to superbill.
					if(listPatAging[i].HasSuperBilling && superHead.HasSuperBilling) {
						PatAging patA=listSuperAgings.FirstOrDefault(x => x.PatNum==superHead.PatNum);//Attempt to find an existing PatAging for the superhead.
						if(patA==null) {
							//Create new PatAging object using SuperHead "credentials" but the guarantor's balance information.
							patA=new PatAging();
							patA.AmountDue=listPatAging[i].AmountDue;
							patA.BalTotal=listPatAging[i].BalTotal;
							patA.Bal_0_30=listPatAging[i].Bal_0_30;
							patA.Bal_31_60=listPatAging[i].Bal_31_60;
							patA.Bal_61_90=listPatAging[i].Bal_61_90;
							patA.BalOver90=listPatAging[i].BalOver90;
							patA.InsEst=listPatAging[i].InsEst;
							patA.PatName=superHead.GetNameLF();
							patA.DateLastStatement=listPatAging[i].DateLastStatement;
							patA.BillingType=superHead.BillingType;
							patA.PayPlanDue=listPatAging[i].PayPlanDue;
							patA.PatNum=superHead.PatNum;
							patA.HasSuperBilling=listPatAging[i].HasSuperBilling;//true
							patA.SuperFamily=listPatAging[i].SuperFamily;//Same as superHead.PatNum
							listSuperAgings.Add(patA);
						}
						else {
							//Sum the information together for all guarantors of the superfamily.
							patA.AmountDue+=listPatAging[i].AmountDue;
							patA.BalTotal+=listPatAging[i].BalTotal;
							patA.Bal_0_30+=listPatAging[i].Bal_0_30;
							patA.Bal_31_60+=listPatAging[i].Bal_31_60;
							patA.Bal_61_90+=listPatAging[i].Bal_61_90;
							patA.BalOver90+=listPatAging[i].BalOver90;
							patA.InsEst+=listPatAging[i].InsEst;
							patA.PayPlanDue+=listPatAging[i].PayPlanDue;
						}
						listPatAging.RemoveAt(i);//Remove the individual guarantor statement from the aging list since it's been combined with a superstatement.
					}
				}
				listPatAging.AddRange(listSuperAgings);
			}
			#region Message Construction
			if(PrefC.HasClinicsEnabled) {
				string clinicAbbr;
				switch(clinicNum) {
					case -1://All
						clinicAbbr=Lan.g(this,"All");
						break;
					case 0://Unassigned
						clinicAbbr=Lan.g(this,"Unassigned");
						break;
					default:
						clinicAbbr=_listClinics.First(x => x.ClinicNum==clinicNum).Abbr;
						break;
				}
				_popUpMessage+=Lan.g(this,clinicAbbr)+" - ";
			}
			if(listPatAging.Count==0){
				if(!suppressPopup) {
					MsgBox.Show(this,"List of created bills is empty.");
				}
				else {
					_popUpMessage+=Lan.g(this,"List of created bills is empty.")+"\r\n";
				}
				return;
			}
			else {
				_popUpMessage+=Lan.g(this,"Statements created")+": "+POut.Int(listPatAging.Count)+"\r\n";
			}
			#endregion
			DateTime dateRangeFrom=PIn.Date(textDateStart.Text);
			DateTime dateRangeTo=DateTimeOD.Today;//Needed for payplan accuracy.//new DateTime(2200,1,1);
			if(textDateEnd.Text!=""){
				dateRangeTo=PIn.Date(textDateEnd.Text);
			}
			Statement stmt;
			DateTime dateAsOf=DateTime.Today;//used to determine when the balance on this date began
			if(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)) {//if aging calculated monthly, use the last aging date instead of today
				dateAsOf=PrefC.GetDate(PrefName.DateLastAging);
			}
			//make lookup dict of key=PatNum, value=DateBalBegan
			DataTable table=Ledgers.GetDateBalanceBegan(listPatAging,dateAsOf,checkSuperFam.Checked);
			Dictionary<long,DateTime> dictPatNumDateBalBegan=table.Select().ToDictionary(x => PIn.Long(x["PatNum"].ToString()),x => PIn.Date(x["DateAccountAge"].ToString()));
			Dictionary<long,DateTime> dictPatNumDateBalZero=table.Select().ToDictionary(x => PIn.Long(x["PatNum"].ToString()),x => PIn.Date(x["DateZeroBal"].ToString()));
			Dictionary<long,PatComm> dictPatComms=Patients.GetPatComms(table.Select().Select(x => PIn.Long(x["PatNum"].ToString())).ToList(),
					Clinics.GetFirstOrDefault(x => x.ClinicNum==clinicNum)??Clinics.GetPracticeAsClinicZero())
				.GroupBy(x => x.PatNum)
				.ToDictionary(x => x.Key,y => y.First());
			Stack<WebServiceMainHQProxy.ShortGuidResult> stackShortGuidUrls=new Stack<WebServiceMainHQProxy.ShortGuidResult>();
			SheetDef stmtSheet=SheetUtil.GetStatementSheetDef();
			if(//They are going to send texts
				listModeToText.SelectedIndices.Count > 0 
				//Or the email body has a statement URL
				|| PrefC.GetString(PrefName.BillingEmailBodyText).ToLower().Contains("[statementurl]")
				|| PrefC.GetString(PrefName.BillingEmailBodyText).ToLower().Contains("[statementshorturl]")
				//Or the statement sheet has a URL field
				|| (stmtSheet!=null && stmtSheet.SheetFieldDefs.Any(x => x.FieldType==SheetFieldType.OutputText 
							&& (x.FieldValue.ToLower().Contains("[statementurl]") || x.FieldValue.ToLower().Contains("[statemenshortturl]"))))) 
			{
				//Then get some short GUIDs and URLs from OD HQ.
				try {
					stackShortGuidUrls=new Stack<WebServiceMainHQProxy.ShortGuidResult>(WebServiceMainHQProxy.GetShortGUIDs(listPatAging.Count,clinicNum,
						eServiceCode.PatientPortalViewStatement));
				}
				catch(Exception ex) {
					FriendlyException.Show(Lans.g(this,"Unable to create a unique URL for each statement. The Patient Portal URL will be used instead."),ex);
					for(int i=0;i<listPatAging.Count;i++) {
						stackShortGuidUrls.Push(new WebServiceMainHQProxy.ShortGuidResult {
							MediumURL=PrefC.GetString(PrefName.PatientPortalURL),
							ShortURL=PrefC.GetString(PrefName.PatientPortalURL),
							ShortGuid=""
						});
					}
				}
			}
			DateTime dateBalBeganCur;
			foreach(PatAging patAgeCur in listPatAging) {
				stmt=new Statement();
				stmt.DateRangeFrom=dateRangeFrom;
				stmt.DateRangeTo=dateRangeTo;
				stmt.DateSent=DateTimeOD.Today;
				stmt.DocNum=0;
				stmt.HidePayment=false;
				stmt.Intermingled=checkIntermingled.Checked;
				stmt.IsSent=false;
				if(PrefC.GetInt(PrefName.BillingUseElectronic).In(1,2,3,4))
				{
					stmt.Mode_=StatementMode.Electronic;
					stmt.Intermingled=true;
				}
				else {
					stmt.Mode_=StatementMode.Mail;
				}
				Def billingType=Defs.GetDef(DefCat.BillingTypes,patAgeCur.BillingType);
				if(billingType != null && billingType.ItemValue=="E") {
					stmt.Mode_=StatementMode.Email;
				}
				bool doSendSms=false;
				PatComm patComm;
				if(stmt.Mode_.In(listModeToText.SelectedTags<StatementMode>())
					&& dictPatComms.TryGetValue(patAgeCur.PatNum,out patComm)
					&& patComm.IsSmsAnOption) 
				{
					doSendSms=true;
				}
				stmt.SmsSendStatus=(doSendSms ? AutoCommStatus.SendNotAttempted : AutoCommStatus.DoNotSend);
				stmt.ShortGUID="";
				stmt.StatementURL="";
				stmt.StatementShortURL="";
				if(stackShortGuidUrls.Count > 0) {
					WebServiceMainHQProxy.ShortGuidResult shortGuidUrl=stackShortGuidUrls.Pop();
					stmt.ShortGUID=shortGuidUrl.ShortGuid;
					stmt.StatementURL=shortGuidUrl.MediumURL;
					stmt.StatementShortURL=shortGuidUrl.ShortURL;
				}
				InstallmentPlan installPlan=InstallmentPlans.GetOneForFam(patAgeCur.PatNum);
				stmt.Note=textNote.Text;
				if(installPlan!=null) {
					stmt.Note=textNote.Text.Replace("[InstallmentPlanTerms]","Installment Plan\r\n"
						+"Date First Payment: "+installPlan.DateFirstPayment.ToShortDateString()+"\r\n"
						+"Monthly Payment: "+installPlan.MonthlyPayment.ToString("c")+"\r\n"
						+"APR: "+(installPlan.APR/100).ToString("P")+"\r\n"
						+"Note: "+installPlan.Note);
				}
				//appointment reminders are not handled here since it would be too slow.
				DateTime dateBalZeroCur=DateTime.MinValue;
				dictPatNumDateBalZero.TryGetValue(patAgeCur.PatNum,out dateBalZeroCur);//dateBalBeganCur will be DateTime.MinValue if PatNum isn't in dict
				if(checkBoxBillShowTransSinceZero.Checked && dateBalZeroCur.Year > 1880) {
					stmt.DateRangeFrom=dateBalZeroCur;
				}
				//dateBalBegan is first transaction date for a charge that consumed the last of the credits for the account, so first transaction that isn't
				//fully paid for based on oldest paid first logic
				dateBalBeganCur=DateTime.MinValue;
				dictPatNumDateBalBegan.TryGetValue(patAgeCur.PatNum,out dateBalBeganCur);//dateBalBeganCur will be DateTime.MinValue if PatNum isn't in dict
				int ageAccount=0;
				//ageAccount is number of days between the day the account first started to have a positive bal and the asOf date
				if(dateBalBeganCur>DateTime.MinValue) {
					ageAccount=(dateAsOf-dateBalBeganCur).Days;
				}
				Dunning dunning=_listDunnings.LastOrDefault(x => (x.BillingType==0 || x.BillingType==patAgeCur.BillingType) //same billing type
					&& x.ClinicNum==patAgeCur.ClinicNum
					&& ageAccount>=x.AgeAccount-x.DaysInAdvance //old enough to qualify for this dunning message, taking into account DaysInAdvance
					&& (x.InsIsPending==YN.Unknown || x.InsIsPending==(patAgeCur.InsEst>0?YN.Yes:YN.No)));//dunning msg ins pending=unkown or matches this acct
				if(dunning!=null){
					if(stmt.Note!=""){
						stmt.Note+="\r\n\r\n";//leave one empty line
					}
					stmt.Note+=dunning.DunMessage;
					stmt.NoteBold=dunning.MessageBold;
					stmt.EmailSubject=dunning.EmailSubject;					
					stmt.EmailBody=dunning.EmailBody;
				}
				stmt.PatNum=patAgeCur.PatNum;
				stmt.SinglePatient=checkSinglePatient.Checked;
				//If this bill is for the superhead and has superbill enabled, it's a superbill.  Flag it as such.
				if(patAgeCur.HasSuperBilling && patAgeCur.PatNum==patAgeCur.SuperFamily && checkSuperFam.Checked) {
					stmt.SuperFamily=patAgeCur.SuperFamily;
				}
				stmt.IsBalValid=true;
				stmt.BalTotal=patAgeCur.BalTotal;
				stmt.InsEst=patAgeCur.InsEst;
				Statements.Insert(stmt);
			}
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}




}
