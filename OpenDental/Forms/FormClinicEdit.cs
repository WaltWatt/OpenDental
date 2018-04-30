using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormClinicEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textDescription;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		///<summary></summary>
		public bool IsNew;
		private System.Windows.Forms.TextBox textPhone;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBankNumber;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox comboPlaceService;
		private GroupBox groupBox4;
		private ComboBox comboInsBillingProv;
		private RadioButton radioInsBillingProvSpecific;
		private RadioButton radioInsBillingProvTreat;
		private RadioButton radioInsBillingProvDefault;
		private TextBox textFax;
		private Label label8;
		private Label label9;
		private Label label10;
		private TextBox textEmail;
		public Clinic ClinicCur;
		private Label label12;
		private ComboBox comboDefaultProvider;
		private UI.Button butPickDefaultProv;
		private UI.Button butEmail;
		private UI.Button butPickInsBillingProv;
		//private List<Provider> _listProv;
		private UI.Button butNone;
		private CheckBox checkIsMedicalOnly;
		private TextBox textCity;
		private TextBox textState;
		private TextBox textZip;
		private TextBox textAddress2;
		private Label label11;
		private Label label4;
		private TextBox textAddress;
		private Label label3;
		private Label label17;
		private Label label13;
		private TextBox textPayToZip;
		private TextBox textPayToST;
		private TextBox textPayToCity;
		private TextBox textPayToAddress2;
		private TextBox textPayToAddress;
		private Label label14;
		private Label label15;
		private Label label18;
		private Label label16;
		private TextBox textBillingZip;
		private TextBox textBillingST;
		private TextBox textBillingCity;
		private TextBox textBillingAddress2;
		private TextBox textBillingAddress;
		private Label label19;
		private Label label20;
		private TextBox textClinicNum;
		private Label label21;
		private CheckBox checkUseBillingAddressOnClaims;
		private Label label22;
		private ComboBox comboRegion;
		///<summary>Filtered list of providers based on which clinic is selected. If no clinic is selected displays all providers.
		///Also includes a dummy clinic at index 0 for "none"</summary>
		private List<Provider> _listProviders;
		///<summary>Instead of relying on _listProviders[comboProv.SelectedIndex] to determine the selected Provider we use this variable to store it
		///explicitly.</summary>
		private long _selectedProvBillNum;
		private TabControl tabControl1;
		private TabPage PhysicalAddress;
		private TabPage BillingAddress;
		private TabPage PayToAddress;
		private CheckBox checkExcludeFromInsVerifyList;
		private TextBox textClinicAbbr;
		private Label label23;
		private TextBox textMedLabAcctNum;
		private Label labelMedLabAcctNum;
		///<summary>Instead of relying on _listProviders[comboProvHyg.SelectedIndex] to determine the selected Provider we use this variable to store it explicitly.</summary>
		private long _selectedProvDefNum;
		private CheckBox checkHidden;
		private TextBox textExternalID;
		private Label labelExternalID;
		private UI.Button butEmailNone;
		private TabPage tabSpecialty;
		private UI.Button butAdd;
		private UI.ODGrid gridSpecialty;
		private UI.Button butRemove;
		///<summary>Keeps track of all deflinks (specialties) for this clinic.  Filled in the constructor, never null.</summary>
		public DefLinkClinic DefLinkClinicSpecialties;
		private TextBox textSchedRules;
		private Label labelSchedRules;

		///<summary>True if an HL7Def is enabled with the type HL7InternalType.MedLabv2_3, otherwise false.</summary>
		private bool _isMedLabHL7DefEnabled;
		private CheckBox checkProcCodeRequired;
		private List<Def> _listRegionDefs;

		///<summary>Pass in an optional DefLinkClinic object.  If null is passed, this constructor will go get the information from the database.</summary>
		public FormClinicEdit(Clinic clinicCur,DefLinkClinic defLinkClinic = null)
		{
			//
			// Required for Windows Form Designer support
			//
			ClinicCur=clinicCur;
			if(defLinkClinic!=null) {
				DefLinkClinicSpecialties=defLinkClinic;
			}
			else {
				DefLinkClinicSpecialties=new DefLinkClinic(ClinicCur,DefLinks.GetListByFKey(clinicCur.ClinicNum,DefLinkType.Clinic));
			}
			InitializeComponent();
			Lan.F(this);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClinicEdit));
			this.checkHidden = new System.Windows.Forms.CheckBox();
			this.textMedLabAcctNum = new System.Windows.Forms.TextBox();
			this.labelMedLabAcctNum = new System.Windows.Forms.Label();
			this.textClinicAbbr = new System.Windows.Forms.TextBox();
			this.label23 = new System.Windows.Forms.Label();
			this.checkExcludeFromInsVerifyList = new System.Windows.Forms.CheckBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.PhysicalAddress = new System.Windows.Forms.TabPage();
			this.textAddress = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textCity = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.textState = new System.Windows.Forms.TextBox();
			this.textAddress2 = new System.Windows.Forms.TextBox();
			this.textZip = new System.Windows.Forms.TextBox();
			this.BillingAddress = new System.Windows.Forms.TabPage();
			this.checkUseBillingAddressOnClaims = new System.Windows.Forms.CheckBox();
			this.label18 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.textBillingZip = new System.Windows.Forms.TextBox();
			this.textBillingAddress = new System.Windows.Forms.TextBox();
			this.textBillingST = new System.Windows.Forms.TextBox();
			this.textBillingAddress2 = new System.Windows.Forms.TextBox();
			this.textBillingCity = new System.Windows.Forms.TextBox();
			this.PayToAddress = new System.Windows.Forms.TabPage();
			this.label17 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.textPayToZip = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.textPayToST = new System.Windows.Forms.TextBox();
			this.textPayToAddress = new System.Windows.Forms.TextBox();
			this.textPayToCity = new System.Windows.Forms.TextBox();
			this.textPayToAddress2 = new System.Windows.Forms.TextBox();
			this.tabSpecialty = new System.Windows.Forms.TabPage();
			this.butRemove = new OpenDental.UI.Button();
			this.gridSpecialty = new OpenDental.UI.ODGrid();
			this.butAdd = new OpenDental.UI.Button();
			this.comboRegion = new System.Windows.Forms.ComboBox();
			this.label22 = new System.Windows.Forms.Label();
			this.textClinicNum = new System.Windows.Forms.TextBox();
			this.label21 = new System.Windows.Forms.Label();
			this.checkIsMedicalOnly = new System.Windows.Forms.CheckBox();
			this.butNone = new OpenDental.UI.Button();
			this.butPickDefaultProv = new OpenDental.UI.Button();
			this.comboDefaultProvider = new System.Windows.Forms.ComboBox();
			this.label12 = new System.Windows.Forms.Label();
			this.textFax = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.butPickInsBillingProv = new OpenDental.UI.Button();
			this.comboInsBillingProv = new System.Windows.Forms.ComboBox();
			this.radioInsBillingProvSpecific = new System.Windows.Forms.RadioButton();
			this.radioInsBillingProvTreat = new System.Windows.Forms.RadioButton();
			this.radioInsBillingProvDefault = new System.Windows.Forms.RadioButton();
			this.label10 = new System.Windows.Forms.Label();
			this.comboPlaceService = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.textEmail = new System.Windows.Forms.TextBox();
			this.textBankNumber = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textPhone = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textDescription = new System.Windows.Forms.TextBox();
			this.butEmail = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.textExternalID = new System.Windows.Forms.TextBox();
			this.labelExternalID = new System.Windows.Forms.Label();
			this.butEmailNone = new OpenDental.UI.Button();
			this.textSchedRules = new System.Windows.Forms.TextBox();
			this.labelSchedRules = new System.Windows.Forms.Label();
			this.checkProcCodeRequired = new System.Windows.Forms.CheckBox();
			this.tabControl1.SuspendLayout();
			this.PhysicalAddress.SuspendLayout();
			this.BillingAddress.SuspendLayout();
			this.PayToAddress.SuspendLayout();
			this.tabSpecialty.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// checkHidden
			// 
			this.checkHidden.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHidden.Location = new System.Drawing.Point(80, 631);
			this.checkHidden.Name = "checkHidden";
			this.checkHidden.Size = new System.Drawing.Size(163, 16);
			this.checkHidden.TabIndex = 65;
			this.checkHidden.Text = "Is Hidden";
			this.checkHidden.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textMedLabAcctNum
			// 
			this.textMedLabAcctNum.Location = new System.Drawing.Point(229, 543);
			this.textMedLabAcctNum.MaxLength = 255;
			this.textMedLabAcctNum.Name = "textMedLabAcctNum";
			this.textMedLabAcctNum.Size = new System.Drawing.Size(216, 20);
			this.textMedLabAcctNum.TabIndex = 60;
			this.textMedLabAcctNum.Visible = false;
			// 
			// labelMedLabAcctNum
			// 
			this.labelMedLabAcctNum.Location = new System.Drawing.Point(30, 544);
			this.labelMedLabAcctNum.Name = "labelMedLabAcctNum";
			this.labelMedLabAcctNum.Size = new System.Drawing.Size(198, 17);
			this.labelMedLabAcctNum.TabIndex = 26;
			this.labelMedLabAcctNum.Text = "MedLab Account Number";
			this.labelMedLabAcctNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelMedLabAcctNum.Visible = false;
			// 
			// textClinicAbbr
			// 
			this.textClinicAbbr.Location = new System.Drawing.Point(229, 69);
			this.textClinicAbbr.Name = "textClinicAbbr";
			this.textClinicAbbr.Size = new System.Drawing.Size(157, 20);
			this.textClinicAbbr.TabIndex = 3;
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(19, 69);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(207, 17);
			this.label23.TabIndex = 24;
			this.label23.Text = "Abbreviation";
			this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkExcludeFromInsVerifyList
			// 
			this.checkExcludeFromInsVerifyList.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkExcludeFromInsVerifyList.Location = new System.Drawing.Point(11, 175);
			this.checkExcludeFromInsVerifyList.Name = "checkExcludeFromInsVerifyList";
			this.checkExcludeFromInsVerifyList.Size = new System.Drawing.Size(232, 16);
			this.checkExcludeFromInsVerifyList.TabIndex = 8;
			this.checkExcludeFromInsVerifyList.Text = "Hide From Insurance Verification List";
			this.checkExcludeFromInsVerifyList.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.PhysicalAddress);
			this.tabControl1.Controls.Add(this.BillingAddress);
			this.tabControl1.Controls.Add(this.PayToAddress);
			this.tabControl1.Controls.Add(this.tabSpecialty);
			this.tabControl1.Location = new System.Drawing.Point(120, 210);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(460, 141);
			this.tabControl1.TabIndex = 10;
			this.tabControl1.TabStop = false;
			// 
			// PhysicalAddress
			// 
			this.PhysicalAddress.BackColor = System.Drawing.Color.Transparent;
			this.PhysicalAddress.Controls.Add(this.textAddress);
			this.PhysicalAddress.Controls.Add(this.label3);
			this.PhysicalAddress.Controls.Add(this.label4);
			this.PhysicalAddress.Controls.Add(this.textCity);
			this.PhysicalAddress.Controls.Add(this.label11);
			this.PhysicalAddress.Controls.Add(this.textState);
			this.PhysicalAddress.Controls.Add(this.textAddress2);
			this.PhysicalAddress.Controls.Add(this.textZip);
			this.PhysicalAddress.Location = new System.Drawing.Point(4, 22);
			this.PhysicalAddress.Name = "PhysicalAddress";
			this.PhysicalAddress.Padding = new System.Windows.Forms.Padding(3);
			this.PhysicalAddress.Size = new System.Drawing.Size(452, 115);
			this.PhysicalAddress.TabIndex = 0;
			this.PhysicalAddress.Text = "Physical Treating Address";
			// 
			// textAddress
			// 
			this.textAddress.Location = new System.Drawing.Point(105, 42);
			this.textAddress.MaxLength = 255;
			this.textAddress.Name = "textAddress";
			this.textAddress.Size = new System.Drawing.Size(291, 20);
			this.textAddress.TabIndex = 0;
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Location = new System.Drawing.Point(9, 43);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(95, 17);
			this.label3.TabIndex = 0;
			this.label3.Text = "Address";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Location = new System.Drawing.Point(9, 64);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(95, 17);
			this.label4.TabIndex = 0;
			this.label4.Text = "Address 2";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textCity
			// 
			this.textCity.Location = new System.Drawing.Point(105, 84);
			this.textCity.MaxLength = 255;
			this.textCity.Name = "textCity";
			this.textCity.Size = new System.Drawing.Size(155, 20);
			this.textCity.TabIndex = 2;
			// 
			// label11
			// 
			this.label11.BackColor = System.Drawing.Color.Transparent;
			this.label11.Location = new System.Drawing.Point(9, 86);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(95, 15);
			this.label11.TabIndex = 0;
			this.label11.Text = "City, ST, Zip";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textState
			// 
			this.textState.Location = new System.Drawing.Point(259, 84);
			this.textState.MaxLength = 255;
			this.textState.Name = "textState";
			this.textState.Size = new System.Drawing.Size(66, 20);
			this.textState.TabIndex = 3;
			// 
			// textAddress2
			// 
			this.textAddress2.Location = new System.Drawing.Point(105, 63);
			this.textAddress2.MaxLength = 255;
			this.textAddress2.Name = "textAddress2";
			this.textAddress2.Size = new System.Drawing.Size(291, 20);
			this.textAddress2.TabIndex = 1;
			// 
			// textZip
			// 
			this.textZip.Location = new System.Drawing.Point(324, 84);
			this.textZip.MaxLength = 255;
			this.textZip.Name = "textZip";
			this.textZip.Size = new System.Drawing.Size(72, 20);
			this.textZip.TabIndex = 4;
			// 
			// BillingAddress
			// 
			this.BillingAddress.BackColor = System.Drawing.Color.Transparent;
			this.BillingAddress.Controls.Add(this.checkUseBillingAddressOnClaims);
			this.BillingAddress.Controls.Add(this.label18);
			this.BillingAddress.Controls.Add(this.label20);
			this.BillingAddress.Controls.Add(this.label16);
			this.BillingAddress.Controls.Add(this.label19);
			this.BillingAddress.Controls.Add(this.textBillingZip);
			this.BillingAddress.Controls.Add(this.textBillingAddress);
			this.BillingAddress.Controls.Add(this.textBillingST);
			this.BillingAddress.Controls.Add(this.textBillingAddress2);
			this.BillingAddress.Controls.Add(this.textBillingCity);
			this.BillingAddress.Location = new System.Drawing.Point(4, 22);
			this.BillingAddress.Name = "BillingAddress";
			this.BillingAddress.Padding = new System.Windows.Forms.Padding(3);
			this.BillingAddress.Size = new System.Drawing.Size(452, 115);
			this.BillingAddress.TabIndex = 1;
			this.BillingAddress.Text = "Billing Address";
			// 
			// checkUseBillingAddressOnClaims
			// 
			this.checkUseBillingAddressOnClaims.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUseBillingAddressOnClaims.Location = new System.Drawing.Point(8, 26);
			this.checkUseBillingAddressOnClaims.Name = "checkUseBillingAddressOnClaims";
			this.checkUseBillingAddressOnClaims.Size = new System.Drawing.Size(111, 16);
			this.checkUseBillingAddressOnClaims.TabIndex = 1;
			this.checkUseBillingAddressOnClaims.Text = "Use on Claims";
			this.checkUseBillingAddressOnClaims.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkUseBillingAddressOnClaims.UseVisualStyleBackColor = true;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(6, 5);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(440, 17);
			this.label18.TabIndex = 0;
			this.label18.Text = "Optional, for E-Claims.  Cannot be a PO Box.";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(6, 86);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(98, 15);
			this.label20.TabIndex = 0;
			this.label20.Text = "City, ST, Zip";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(7, 64);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(97, 16);
			this.label16.TabIndex = 0;
			this.label16.Text = "Address 2";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(6, 44);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(98, 14);
			this.label19.TabIndex = 0;
			this.label19.Text = "Address";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textBillingZip
			// 
			this.textBillingZip.Location = new System.Drawing.Point(323, 84);
			this.textBillingZip.Name = "textBillingZip";
			this.textBillingZip.Size = new System.Drawing.Size(73, 20);
			this.textBillingZip.TabIndex = 6;
			// 
			// textBillingAddress
			// 
			this.textBillingAddress.Location = new System.Drawing.Point(105, 42);
			this.textBillingAddress.Name = "textBillingAddress";
			this.textBillingAddress.Size = new System.Drawing.Size(291, 20);
			this.textBillingAddress.TabIndex = 2;
			// 
			// textBillingST
			// 
			this.textBillingST.Location = new System.Drawing.Point(259, 84);
			this.textBillingST.Name = "textBillingST";
			this.textBillingST.Size = new System.Drawing.Size(65, 20);
			this.textBillingST.TabIndex = 5;
			// 
			// textBillingAddress2
			// 
			this.textBillingAddress2.Location = new System.Drawing.Point(105, 63);
			this.textBillingAddress2.Name = "textBillingAddress2";
			this.textBillingAddress2.Size = new System.Drawing.Size(291, 20);
			this.textBillingAddress2.TabIndex = 3;
			// 
			// textBillingCity
			// 
			this.textBillingCity.Location = new System.Drawing.Point(105, 84);
			this.textBillingCity.Name = "textBillingCity";
			this.textBillingCity.Size = new System.Drawing.Size(155, 20);
			this.textBillingCity.TabIndex = 4;
			// 
			// PayToAddress
			// 
			this.PayToAddress.BackColor = System.Drawing.Color.Transparent;
			this.PayToAddress.Controls.Add(this.label17);
			this.PayToAddress.Controls.Add(this.label13);
			this.PayToAddress.Controls.Add(this.label15);
			this.PayToAddress.Controls.Add(this.textPayToZip);
			this.PayToAddress.Controls.Add(this.label14);
			this.PayToAddress.Controls.Add(this.textPayToST);
			this.PayToAddress.Controls.Add(this.textPayToAddress);
			this.PayToAddress.Controls.Add(this.textPayToCity);
			this.PayToAddress.Controls.Add(this.textPayToAddress2);
			this.PayToAddress.Location = new System.Drawing.Point(4, 22);
			this.PayToAddress.Name = "PayToAddress";
			this.PayToAddress.Size = new System.Drawing.Size(452, 115);
			this.PayToAddress.TabIndex = 2;
			this.PayToAddress.Text = "Pay To Address";
			// 
			// label17
			// 
			this.label17.BackColor = System.Drawing.Color.Transparent;
			this.label17.Location = new System.Drawing.Point(6, 5);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(440, 17);
			this.label17.TabIndex = 0;
			this.label17.Text = "Optional for claims.  Can be a PO Box.  Sent in addition to treating or billing a" +
    "ddress.";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(7, 64);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(97, 16);
			this.label13.TabIndex = 0;
			this.label13.Text = "Address 2";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(6, 86);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(98, 15);
			this.label15.TabIndex = 0;
			this.label15.Text = "City, ST, Zip";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPayToZip
			// 
			this.textPayToZip.Location = new System.Drawing.Point(324, 84);
			this.textPayToZip.Name = "textPayToZip";
			this.textPayToZip.Size = new System.Drawing.Size(72, 20);
			this.textPayToZip.TabIndex = 5;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(6, 44);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(98, 14);
			this.label14.TabIndex = 0;
			this.label14.Text = "Address";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPayToST
			// 
			this.textPayToST.Location = new System.Drawing.Point(259, 84);
			this.textPayToST.Name = "textPayToST";
			this.textPayToST.Size = new System.Drawing.Size(66, 20);
			this.textPayToST.TabIndex = 4;
			// 
			// textPayToAddress
			// 
			this.textPayToAddress.Location = new System.Drawing.Point(105, 42);
			this.textPayToAddress.Name = "textPayToAddress";
			this.textPayToAddress.Size = new System.Drawing.Size(291, 20);
			this.textPayToAddress.TabIndex = 1;
			// 
			// textPayToCity
			// 
			this.textPayToCity.Location = new System.Drawing.Point(105, 84);
			this.textPayToCity.Name = "textPayToCity";
			this.textPayToCity.Size = new System.Drawing.Size(155, 20);
			this.textPayToCity.TabIndex = 3;
			// 
			// textPayToAddress2
			// 
			this.textPayToAddress2.Location = new System.Drawing.Point(105, 63);
			this.textPayToAddress2.Name = "textPayToAddress2";
			this.textPayToAddress2.Size = new System.Drawing.Size(291, 20);
			this.textPayToAddress2.TabIndex = 2;
			// 
			// tabSpecialty
			// 
			this.tabSpecialty.Controls.Add(this.butRemove);
			this.tabSpecialty.Controls.Add(this.gridSpecialty);
			this.tabSpecialty.Controls.Add(this.butAdd);
			this.tabSpecialty.Location = new System.Drawing.Point(4, 22);
			this.tabSpecialty.Name = "tabSpecialty";
			this.tabSpecialty.Padding = new System.Windows.Forms.Padding(3);
			this.tabSpecialty.Size = new System.Drawing.Size(452, 115);
			this.tabSpecialty.TabIndex = 3;
			this.tabSpecialty.Text = "Specialty";
			// 
			// butRemove
			// 
			this.butRemove.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRemove.Autosize = true;
			this.butRemove.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRemove.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRemove.CornerRadius = 4F;
			this.butRemove.Location = new System.Drawing.Point(376, 36);
			this.butRemove.Name = "butRemove";
			this.butRemove.Size = new System.Drawing.Size(73, 23);
			this.butRemove.TabIndex = 77;
			this.butRemove.Text = "Remove";
			this.butRemove.UseVisualStyleBackColor = true;
			this.butRemove.Click += new System.EventHandler(this.butRemove_Click);
			// 
			// gridSpecialty
			// 
			this.gridSpecialty.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridSpecialty.HasAddButton = false;
			this.gridSpecialty.HasDropDowns = false;
			this.gridSpecialty.HasMultilineHeaders = false;
			this.gridSpecialty.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridSpecialty.HeaderHeight = 15;
			this.gridSpecialty.HScrollVisible = false;
			this.gridSpecialty.Location = new System.Drawing.Point(6, 7);
			this.gridSpecialty.Name = "gridSpecialty";
			this.gridSpecialty.ScrollValue = 0;
			this.gridSpecialty.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridSpecialty.Size = new System.Drawing.Size(368, 105);
			this.gridSpecialty.TabIndex = 0;
			this.gridSpecialty.Title = "Clinic Specialty";
			this.gridSpecialty.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridSpecialty.TitleHeight = 18;
			this.gridSpecialty.TranslationName = "TableClinicSpecialty";
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Location = new System.Drawing.Point(376, 7);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(73, 23);
			this.butAdd.TabIndex = 76;
			this.butAdd.Text = "Add";
			this.butAdd.UseVisualStyleBackColor = true;
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// comboRegion
			// 
			this.comboRegion.FormattingEnabled = true;
			this.comboRegion.Location = new System.Drawing.Point(229, 153);
			this.comboRegion.Name = "comboRegion";
			this.comboRegion.Size = new System.Drawing.Size(157, 21);
			this.comboRegion.TabIndex = 7;
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(18, 156);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(210, 17);
			this.label22.TabIndex = 20;
			this.label22.Text = "Region";
			this.label22.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textClinicNum
			// 
			this.textClinicNum.BackColor = System.Drawing.SystemColors.Control;
			this.textClinicNum.Location = new System.Drawing.Point(229, 27);
			this.textClinicNum.Name = "textClinicNum";
			this.textClinicNum.ReadOnly = true;
			this.textClinicNum.Size = new System.Drawing.Size(157, 20);
			this.textClinicNum.TabIndex = 1;
			this.textClinicNum.TabStop = false;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(19, 28);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(207, 17);
			this.label21.TabIndex = 19;
			this.label21.Text = "Clinic ID";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkIsMedicalOnly
			// 
			this.checkIsMedicalOnly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsMedicalOnly.Location = new System.Drawing.Point(86, 12);
			this.checkIsMedicalOnly.Name = "checkIsMedicalOnly";
			this.checkIsMedicalOnly.Size = new System.Drawing.Size(157, 16);
			this.checkIsMedicalOnly.TabIndex = 0;
			this.checkIsMedicalOnly.Text = "Is Medical";
			this.checkIsMedicalOnly.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butNone
			// 
			this.butNone.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNone.Autosize = true;
			this.butNone.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNone.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNone.CornerRadius = 4F;
			this.butNone.Location = new System.Drawing.Point(476, 498);
			this.butNone.Name = "butNone";
			this.butNone.Size = new System.Drawing.Size(48, 21);
			this.butNone.TabIndex = 35;
			this.butNone.Text = "None";
			this.butNone.Click += new System.EventHandler(this.butNone_Click);
			// 
			// butPickDefaultProv
			// 
			this.butPickDefaultProv.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickDefaultProv.Autosize = false;
			this.butPickDefaultProv.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickDefaultProv.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickDefaultProv.CornerRadius = 2F;
			this.butPickDefaultProv.Location = new System.Drawing.Point(447, 498);
			this.butPickDefaultProv.Name = "butPickDefaultProv";
			this.butPickDefaultProv.Size = new System.Drawing.Size(23, 21);
			this.butPickDefaultProv.TabIndex = 30;
			this.butPickDefaultProv.Text = "...";
			this.butPickDefaultProv.Click += new System.EventHandler(this.butPickDefaultProv_Click);
			// 
			// comboDefaultProvider
			// 
			this.comboDefaultProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDefaultProvider.Location = new System.Drawing.Point(229, 498);
			this.comboDefaultProvider.Name = "comboDefaultProvider";
			this.comboDefaultProvider.Size = new System.Drawing.Size(216, 21);
			this.comboDefaultProvider.TabIndex = 25;
			this.comboDefaultProvider.SelectedIndexChanged += new System.EventHandler(this.comboDefaultProvider_SelectedIndexChanged);
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(30, 500);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(198, 17);
			this.label12.TabIndex = 0;
			this.label12.Text = "Default Provider";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textFax
			// 
			this.textFax.Location = new System.Drawing.Point(229, 132);
			this.textFax.MaxLength = 255;
			this.textFax.Name = "textFax";
			this.textFax.Size = new System.Drawing.Size(157, 20);
			this.textFax.TabIndex = 6;
			this.textFax.TextChanged += new System.EventHandler(this.textFax_TextChanged);
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(18, 135);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(210, 17);
			this.label8.TabIndex = 0;
			this.label8.Text = "Fax";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(389, 150);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(144, 18);
			this.label9.TabIndex = 0;
			this.label9.Text = "(###)###-####";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.butPickInsBillingProv);
			this.groupBox4.Controls.Add(this.comboInsBillingProv);
			this.groupBox4.Controls.Add(this.radioInsBillingProvSpecific);
			this.groupBox4.Controls.Add(this.radioInsBillingProvTreat);
			this.groupBox4.Controls.Add(this.radioInsBillingProvDefault);
			this.groupBox4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox4.Location = new System.Drawing.Point(216, 395);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(262, 100);
			this.groupBox4.TabIndex = 20;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Default Insurance Billing Provider";
			// 
			// butPickInsBillingProv
			// 
			this.butPickInsBillingProv.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickInsBillingProv.Autosize = false;
			this.butPickInsBillingProv.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickInsBillingProv.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickInsBillingProv.CornerRadius = 2F;
			this.butPickInsBillingProv.Location = new System.Drawing.Point(231, 73);
			this.butPickInsBillingProv.Name = "butPickInsBillingProv";
			this.butPickInsBillingProv.Size = new System.Drawing.Size(23, 21);
			this.butPickInsBillingProv.TabIndex = 4;
			this.butPickInsBillingProv.Text = "...";
			this.butPickInsBillingProv.Click += new System.EventHandler(this.butPickInsBillingProv_Click);
			// 
			// comboInsBillingProv
			// 
			this.comboInsBillingProv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboInsBillingProv.Location = new System.Drawing.Point(13, 73);
			this.comboInsBillingProv.Name = "comboInsBillingProv";
			this.comboInsBillingProv.Size = new System.Drawing.Size(216, 21);
			this.comboInsBillingProv.TabIndex = 3;
			this.comboInsBillingProv.SelectedIndexChanged += new System.EventHandler(this.comboInsBillingProv_SelectedIndexChanged);
			// 
			// radioInsBillingProvSpecific
			// 
			this.radioInsBillingProvSpecific.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioInsBillingProvSpecific.Location = new System.Drawing.Point(13, 53);
			this.radioInsBillingProvSpecific.Name = "radioInsBillingProvSpecific";
			this.radioInsBillingProvSpecific.Size = new System.Drawing.Size(186, 19);
			this.radioInsBillingProvSpecific.TabIndex = 2;
			this.radioInsBillingProvSpecific.Text = "Specific Provider:";
			// 
			// radioInsBillingProvTreat
			// 
			this.radioInsBillingProvTreat.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioInsBillingProvTreat.Location = new System.Drawing.Point(13, 35);
			this.radioInsBillingProvTreat.Name = "radioInsBillingProvTreat";
			this.radioInsBillingProvTreat.Size = new System.Drawing.Size(186, 19);
			this.radioInsBillingProvTreat.TabIndex = 1;
			this.radioInsBillingProvTreat.Text = "Treating Provider";
			// 
			// radioInsBillingProvDefault
			// 
			this.radioInsBillingProvDefault.Checked = true;
			this.radioInsBillingProvDefault.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioInsBillingProvDefault.Location = new System.Drawing.Point(13, 17);
			this.radioInsBillingProvDefault.Name = "radioInsBillingProvDefault";
			this.radioInsBillingProvDefault.Size = new System.Drawing.Size(186, 19);
			this.radioInsBillingProvDefault.TabIndex = 0;
			this.radioInsBillingProvDefault.TabStop = true;
			this.radioInsBillingProvDefault.Text = "Default Practice Provider";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(59, 355);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(168, 17);
			this.label10.TabIndex = 0;
			this.label10.Text = "Email Address";
			this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboPlaceService
			// 
			this.comboPlaceService.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPlaceService.Location = new System.Drawing.Point(229, 520);
			this.comboPlaceService.MaxDropDownItems = 30;
			this.comboPlaceService.Name = "comboPlaceService";
			this.comboPlaceService.Size = new System.Drawing.Size(216, 21);
			this.comboPlaceService.TabIndex = 40;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(30, 522);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(198, 17);
			this.label7.TabIndex = 0;
			this.label7.Text = "Default Proc Place of Service";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textEmail
			// 
			this.textEmail.BackColor = System.Drawing.SystemColors.Window;
			this.textEmail.Location = new System.Drawing.Point(229, 352);
			this.textEmail.MaxLength = 255;
			this.textEmail.Name = "textEmail";
			this.textEmail.ReadOnly = true;
			this.textEmail.Size = new System.Drawing.Size(266, 20);
			this.textEmail.TabIndex = 11;
			// 
			// textBankNumber
			// 
			this.textBankNumber.Location = new System.Drawing.Point(229, 373);
			this.textBankNumber.MaxLength = 255;
			this.textBankNumber.Name = "textBankNumber";
			this.textBankNumber.Size = new System.Drawing.Size(291, 20);
			this.textBankNumber.TabIndex = 15;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(77, 377);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(151, 17);
			this.label5.TabIndex = 0;
			this.label5.Text = "Bank Account Number";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textPhone
			// 
			this.textPhone.Location = new System.Drawing.Point(229, 111);
			this.textPhone.MaxLength = 255;
			this.textPhone.Name = "textPhone";
			this.textPhone.Size = new System.Drawing.Size(157, 20);
			this.textPhone.TabIndex = 5;
			this.textPhone.TextChanged += new System.EventHandler(this.textPhone_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(18, 114);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(210, 17);
			this.label2.TabIndex = 0;
			this.label2.Text = "Phone";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDescription
			// 
			this.textDescription.Location = new System.Drawing.Point(229, 90);
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(263, 20);
			this.textDescription.TabIndex = 4;
			// 
			// butEmail
			// 
			this.butEmail.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEmail.Autosize = true;
			this.butEmail.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEmail.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEmail.CornerRadius = 4F;
			this.butEmail.Location = new System.Drawing.Point(497, 351);
			this.butEmail.Name = "butEmail";
			this.butEmail.Size = new System.Drawing.Size(24, 21);
			this.butEmail.TabIndex = 12;
			this.butEmail.Text = "...";
			this.butEmail.Click += new System.EventHandler(this.butEmail_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(518, 642);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 70;
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
			this.butCancel.Location = new System.Drawing.Point(599, 642);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 75;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(19, 91);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(207, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Description";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(389, 129);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(144, 18);
			this.label6.TabIndex = 0;
			this.label6.Text = "(###)###-####";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textExternalID
			// 
			this.textExternalID.Location = new System.Drawing.Point(229, 48);
			this.textExternalID.Name = "textExternalID";
			this.textExternalID.Size = new System.Drawing.Size(157, 20);
			this.textExternalID.TabIndex = 2;
			// 
			// labelExternalID
			// 
			this.labelExternalID.Location = new System.Drawing.Point(19, 48);
			this.labelExternalID.Name = "labelExternalID";
			this.labelExternalID.Size = new System.Drawing.Size(207, 17);
			this.labelExternalID.TabIndex = 28;
			this.labelExternalID.Text = "External ID";
			this.labelExternalID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butEmailNone
			// 
			this.butEmailNone.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEmailNone.Autosize = true;
			this.butEmailNone.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEmailNone.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEmailNone.CornerRadius = 4F;
			this.butEmailNone.Enabled = false;
			this.butEmailNone.Location = new System.Drawing.Point(526, 351);
			this.butEmailNone.Name = "butEmailNone";
			this.butEmailNone.Size = new System.Drawing.Size(48, 21);
			this.butEmailNone.TabIndex = 13;
			this.butEmailNone.Text = "None";
			this.butEmailNone.UseVisualStyleBackColor = true;
			this.butEmailNone.Click += new System.EventHandler(this.buttDetachEmail_Click);
			// 
			// textSchedRules
			// 
			this.textSchedRules.Location = new System.Drawing.Point(229, 565);
			this.textSchedRules.MaxLength = 255;
			this.textSchedRules.Multiline = true;
			this.textSchedRules.Name = "textSchedRules";
			this.textSchedRules.Size = new System.Drawing.Size(291, 60);
			this.textSchedRules.TabIndex = 63;
			// 
			// labelSchedRules
			// 
			this.labelSchedRules.Location = new System.Drawing.Point(56, 567);
			this.labelSchedRules.Name = "labelSchedRules";
			this.labelSchedRules.Size = new System.Drawing.Size(170, 14);
			this.labelSchedRules.TabIndex = 266;
			this.labelSchedRules.Text = "Scheduling Note";
			this.labelSchedRules.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkProcCodeRequired
			// 
			this.checkProcCodeRequired.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcCodeRequired.Enabled = false;
			this.checkProcCodeRequired.Location = new System.Drawing.Point(11, 190);
			this.checkProcCodeRequired.Name = "checkProcCodeRequired";
			this.checkProcCodeRequired.Size = new System.Drawing.Size(232, 16);
			this.checkProcCodeRequired.TabIndex = 267;
			this.checkProcCodeRequired.Text = "Proc code required on Rx from this clinic";
			this.checkProcCodeRequired.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// FormClinicEdit
			// 
			this.ClientSize = new System.Drawing.Size(686, 680);
			this.Controls.Add(this.checkProcCodeRequired);
			this.Controls.Add(this.textSchedRules);
			this.Controls.Add(this.labelSchedRules);
			this.Controls.Add(this.butEmailNone);
			this.Controls.Add(this.textExternalID);
			this.Controls.Add(this.labelExternalID);
			this.Controls.Add(this.checkHidden);
			this.Controls.Add(this.textMedLabAcctNum);
			this.Controls.Add(this.labelMedLabAcctNum);
			this.Controls.Add(this.textClinicAbbr);
			this.Controls.Add(this.label23);
			this.Controls.Add(this.checkExcludeFromInsVerifyList);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.comboRegion);
			this.Controls.Add(this.label22);
			this.Controls.Add(this.textClinicNum);
			this.Controls.Add(this.label21);
			this.Controls.Add(this.checkIsMedicalOnly);
			this.Controls.Add(this.butNone);
			this.Controls.Add(this.butPickDefaultProv);
			this.Controls.Add(this.comboDefaultProvider);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.textFax);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.comboPlaceService);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textEmail);
			this.Controls.Add(this.textBankNumber);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textPhone);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.butEmail);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label6);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(666, 719);
			this.Name = "FormClinicEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Clinic";
			this.Load += new System.EventHandler(this.FormClinicEdit_Load);
			this.tabControl1.ResumeLayout(false);
			this.PhysicalAddress.ResumeLayout(false);
			this.PhysicalAddress.PerformLayout();
			this.BillingAddress.ResumeLayout(false);
			this.BillingAddress.PerformLayout();
			this.PayToAddress.ResumeLayout(false);
			this.PayToAddress.PerformLayout();
			this.tabSpecialty.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormClinicEdit_Load(object sender, System.EventArgs e) {
			checkIsMedicalOnly.Checked=ClinicCur.IsMedicalOnly;
			if(Programs.UsingEcwTightOrFullMode()) {
				checkIsMedicalOnly.Visible=false;
			}
			if(ClinicCur.ClinicNum!=0) {
				textClinicNum.Text=ClinicCur.ClinicNum.ToString();
			}
			textExternalID.Text=ClinicCur.ExternalID.ToString();
			textDescription.Text=ClinicCur.Description;
			textClinicAbbr.Text=ClinicCur.Abbr;
			textPhone.Text=TelephoneNumbers.ReFormat(ClinicCur.Phone);
			textFax.Text=TelephoneNumbers.ReFormat(ClinicCur.Fax);
			checkUseBillingAddressOnClaims.Checked=ClinicCur.UseBillAddrOnClaims;
			checkExcludeFromInsVerifyList.Checked=ClinicCur.IsInsVerifyExcluded;
			if(PrefC.GetBool(PrefName.RxHasProc)) {
				checkProcCodeRequired.Enabled=true;
				checkProcCodeRequired.Checked=(IsNew || ClinicCur.HasProcOnRx);
			}
			checkHidden.Checked=ClinicCur.IsHidden;
			textAddress.Text=ClinicCur.Address;
			textAddress2.Text=ClinicCur.Address2;
			textCity.Text=ClinicCur.City;
			textState.Text=ClinicCur.State;
			textZip.Text=ClinicCur.Zip;
			textBillingAddress.Text=ClinicCur.BillingAddress;
			textBillingAddress2.Text=ClinicCur.BillingAddress2;
			textBillingCity.Text=ClinicCur.BillingCity;
			textBillingST.Text=ClinicCur.BillingState;
			textBillingZip.Text=ClinicCur.BillingZip;
			textPayToAddress.Text=ClinicCur.PayToAddress;
			textPayToAddress2.Text=ClinicCur.PayToAddress2;
			textPayToCity.Text=ClinicCur.PayToCity;
			textPayToST.Text=ClinicCur.PayToState;
			textPayToZip.Text=ClinicCur.PayToZip;
			textBankNumber.Text=ClinicCur.BankNumber;
			textSchedRules.Text=ClinicCur.SchedNote;
			comboPlaceService.Items.Clear();
			comboPlaceService.Items.AddRange(Enum.GetNames(typeof(PlaceOfService)));
			comboPlaceService.SelectedIndex=(int)ClinicCur.DefaultPlaceService;
			_selectedProvBillNum=ClinicCur.InsBillingProv;
			_selectedProvDefNum=ClinicCur.DefaultProv;
			comboDefaultProvider.SelectedIndex=-1;
			comboInsBillingProv.SelectedIndex=-1;
			_listProviders=Providers.GetProvsForClinic(ClinicCur.ClinicNum);
			FillComboProviders();
			FillSpecialty();
			comboRegion.Items.Clear();
			comboRegion.Items.Add(Lan.g(this,"None"));
			comboRegion.SelectedIndex=0;
			_listRegionDefs=Defs.GetDefsForCategory(DefCat.Regions,true);
			for(int i=0;i<_listRegionDefs.Count;i++) {
				comboRegion.Items.Add(_listRegionDefs[i].ItemName);
				if(_listRegionDefs[i].DefNum==ClinicCur.Region) {
					comboRegion.SelectedIndex=i+1;
				}
			}
			if(ClinicCur.InsBillingProv==0){
				radioInsBillingProvDefault.Checked=true;//default=0
			}
			else if(ClinicCur.InsBillingProv==-1){
				radioInsBillingProvTreat.Checked=true;//treat=-1
			}
			else{
				radioInsBillingProvSpecific.Checked=true;//specific=any number >0. Foreign key to ProvNum
			}
			EmailAddress emailAddress=EmailAddresses.GetOne(ClinicCur.EmailAddressNum);
			if(emailAddress!=null) {
				textEmail.Text=emailAddress.GetFrom();
				butEmailNone.Enabled=true;
			}
			_isMedLabHL7DefEnabled=HL7Defs.IsExistingHL7Enabled(0,true);
			if(_isMedLabHL7DefEnabled) {
				textMedLabAcctNum.Visible=true;
				labelMedLabAcctNum.Visible=true;
				textMedLabAcctNum.Text=ClinicCur.MedLabAccountNum;
			}
		}

		private void comboInsBillingProv_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboInsBillingProv.SelectedIndex>-1) {
				_selectedProvBillNum=_listProviders[comboInsBillingProv.SelectedIndex].ProvNum;
			}
		}

		private void comboDefaultProvider_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboDefaultProvider.SelectedIndex>-1) {
				_selectedProvDefNum=_listProviders[comboDefaultProvider.SelectedIndex].ProvNum;
			}
		}

		private void butPickInsBillingProv_Click(object sender,EventArgs e) {
			FormProviderPick FormPP = new FormProviderPick(_listProviders);
			FormPP.SelectedProvNum=_selectedProvBillNum;
			FormPP.ShowDialog();
			if(FormPP.DialogResult!=DialogResult.OK) {
				return;
			}
			_selectedProvBillNum=FormPP.SelectedProvNum;
			comboInsBillingProv.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvBillNum),
				() => { return Providers.GetAbbr(_selectedProvBillNum); });
		}

		private void butPickDefaultProv_Click(object sender,EventArgs e) {
			FormProviderPick FormPP = new FormProviderPick(_listProviders);
			FormPP.SelectedProvNum=_selectedProvDefNum;
			FormPP.ShowDialog();
			if(FormPP.DialogResult!=DialogResult.OK) {
				return;
			}
			_selectedProvDefNum=FormPP.SelectedProvNum;
			comboDefaultProvider.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvDefNum),
				() => { return Providers.GetAbbr(_selectedProvDefNum); });
		}

		private void butNone_Click(object sender,EventArgs e) {
			_selectedProvDefNum=0;
			comboDefaultProvider.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvDefNum),
				() => { return Providers.GetAbbr(_selectedProvDefNum); });
		}

		private void FillComboProviders() {
			//Fill comboInsBillingProvider
			comboInsBillingProv.Items.Clear();
			_listProviders.ForEach(x => comboInsBillingProv.Items.Add(x.Abbr));
			comboInsBillingProv.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvBillNum),() => { return Providers.GetAbbr(_selectedProvBillNum); });
			//Fill comboDefaultProvider
			comboDefaultProvider.Items.Clear();
			_listProviders.ForEach(x => comboDefaultProvider.Items.Add(x.Abbr));
			comboDefaultProvider.IndexSelectOrSetText(_listProviders.FindIndex(x => x.ProvNum==_selectedProvDefNum),() => { return Providers.GetAbbr(_selectedProvDefNum); });
		}

		private void FillSpecialty() {
			List<Def> listClinicDefs=Defs.GetDefsForCategory(DefCat.ClinicSpecialty);
			gridSpecialty.BeginUpdate();
			gridSpecialty.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(gridSpecialty.TranslationName,"Specialty"),100);
			gridSpecialty.Columns.Add(col);
			gridSpecialty.Rows.Clear();
			ODGridRow row;
			foreach(DefLink defLink in DefLinkClinicSpecialties.ListDefLink) {
				row=new ODGridRow();
				Def defCur=listClinicDefs.FirstOrDefault(x => x.DefNum==defLink.DefNum);
				if(defCur==null) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(defCur.ItemName+(defCur.IsHidden ? " "+Lan.g(this,"(hidden)") : ""));
				}
				row.Tag=defLink;
				gridSpecialty.Rows.Add(row);
			}
			gridSpecialty.EndUpdate();
		}
		
		private void butAdd_Click(object sender,EventArgs e) {
			FormDefinitionPicker FormDP = new FormDefinitionPicker(DefCat.ClinicSpecialty);
			FormDP.HasShowHiddenOption=false;
			FormDP.IsMultiSelectionMode=true;
			FormDP.ShowDialog();
			if(FormDP.DialogResult==DialogResult.OK) {
				foreach(Def defCur in FormDP.ListSelectedDefs) {
					if(DefLinkClinicSpecialties.ListDefLink.Exists(x => x.DefNum==defCur.DefNum)) {
						continue;//Definition already added to this clinic. 
					}
					DefLink defLink=new DefLink();
					defLink.DefNum=defCur.DefNum;
					defLink.FKey=ClinicCur.ClinicNum;
					defLink.LinkType=DefLinkType.Clinic;
					DefLinkClinicSpecialties.ListDefLink.Add(defLink);
				}
				FillSpecialty();
			}
		}

		private void butRemove_Click(object sender,EventArgs e) {
			if(gridSpecialty.SelectedIndices.Length==0) {
				MessageBox.Show(Lan.g(this,"Please select a specialty first."));
				return;
			}
			for(int i = 0;i<gridSpecialty.SelectedIndices.Length;i++) {
				DefLink defLink=(DefLink)gridSpecialty.Rows[gridSpecialty.SelectedIndices[i]].Tag;
				DefLinkClinicSpecialties.ListDefLink.Remove(defLink);
			}
			FillSpecialty();
		}

		private void textPhone_TextChanged(object sender,System.EventArgs e) {
			int cursor=textPhone.SelectionStart;
			int length=textPhone.Text.Length;
			textPhone.Text=TelephoneNumbers.AutoFormat(textPhone.Text);
			if(textPhone.Text.Length>length)
				cursor++;
			textPhone.SelectionStart=cursor;		
		}

		private void textFax_TextChanged(object sender,EventArgs e) {
			int cursor=textFax.SelectionStart;
			int length=textFax.Text.Length;
			textFax.Text=TelephoneNumbers.AutoFormat(textFax.Text);
			if(textFax.Text.Length>length)
				cursor++;
			textFax.SelectionStart=cursor;
		}

		private void butEmail_Click(object sender,EventArgs e) {
			FormEmailAddresses FormEA=new FormEmailAddresses();
			FormEA.IsSelectionMode=true;
			FormEA.ShowDialog();
			if(FormEA.DialogResult!=DialogResult.OK) {
				return;
			}
			ClinicCur.EmailAddressNum=FormEA.EmailAddressNum;
			textEmail.Text=EmailAddresses.GetOne(FormEA.EmailAddressNum).GetFrom();
			butEmailNone.Enabled=true;
		}

		private void buttDetachEmail_Click(object sender,EventArgs e) {
			ClinicCur.EmailAddressNum=0;
			textEmail.Text="";
			butEmailNone.Enabled=false;
		}

		//private void butDelete_Click(object sender, System.EventArgs e) {
		//	if(IsNew){
		//		DialogResult=DialogResult.Cancel;
		//		return;
		//	}
		//	if(!MsgBox.Show(this,true,"Delete Clinic?")) {
		//		return;
		//	}
		//	try{
		//		//ClinicNum will be 0 sometimes if a new clinic was added but has not been inserted into the database yet (sync happens outside this window)
		//		//The ClinicNum can be 0 if the user double clicked on the "new" clinic for editing (or in this case, deleting).
		//		//Clinics.Delete has validation checks that should not be run because no references have been made yet.  Simply null ClinicCur out.
		//		if(ClinicCur.ClinicNum!=0) {
		//			Clinics.Delete(ClinicCur);
		//		}
		//		ClinicCur=null;//Set ClinicCur to null so the calling form knows it was deleted.
		//		DialogResult=DialogResult.OK;
		//	}
		//	catch(Exception ex){
		//		MessageBox.Show(ex.Message);
		//	}
		//}

		private void butOK_Click(object sender, System.EventArgs e) {
			#region Validation 
			if(textDescription.Text==""){
				MsgBox.Show(this,"Description cannot be blank.");
				return;
			}
			if(textClinicAbbr.Text==""){
				MsgBox.Show(this,"Abbreviation cannot be blank.");
				return;
			}
			if(radioInsBillingProvSpecific.Checked && _selectedProvBillNum==0){
				MsgBox.Show(this,"You must select a provider.");
				return;
			}
			string phone=textPhone.Text;
			if(Application.CurrentCulture.Name=="en-US"){
				phone=phone.Replace("(","");
				phone=phone.Replace(")","");
				phone=phone.Replace(" ","");
				phone=phone.Replace("-","");
				if(phone.Length!=0 && phone.Length!=10){
					MsgBox.Show(this,"Invalid phone");
					return;
				}
			}
			string fax=textFax.Text;
			if(Application.CurrentCulture.Name=="en-US") {
				fax=fax.Replace("(","");
				fax=fax.Replace(")","");
				fax=fax.Replace(" ","");
				fax=fax.Replace("-","");
				if(fax.Length!=0 && fax.Length!=10) {
					MsgBox.Show(this,"Invalid fax");
					return;
				}
			}
			if(_isMedLabHL7DefEnabled //MedLab HL7 def is enabled, so textMedLabAcctNum is visible
				&& !string.IsNullOrWhiteSpace(textMedLabAcctNum.Text) //MedLabAcctNum has been entered
				&& Clinics.GetWhere(x => x.ClinicNum!=ClinicCur.ClinicNum)
						.Any(x => x.MedLabAccountNum==textMedLabAcctNum.Text.Trim())) //this account num is already in use by another Clinic
			{
				MsgBox.Show(this,"The MedLab Account Number entered is already in use by another clinic.");
				return;
			}
			if(checkHidden.Checked) {
				//ensure that there are no users who have only this clinic assigned to them.
				List<Userod> listUsersRestricted = Userods.GetUsersOnlyThisClinic(ClinicCur.ClinicNum);
				if(listUsersRestricted.Count > 0) {
					MessageBox.Show(Lan.g(this,"You may not hide this clinic as the following users are restricted to only this clinic") + ": "
						+ string.Join(", ",listUsersRestricted.Select(x => x.UserName)));
					return;
				}
			}
			long externalID=0;
			if(textExternalID.Text != "") {
				try {
					externalID=long.Parse(textExternalID.Text);
				}
				catch {
					MsgBox.Show(this,"Please fix data entry errors first."+"\r\n"+", The External ID must be a number. No letters or symbols allowed.");
					return;
				}
			}
			#endregion Validation
			#region Set Values
			ClinicCur.IsMedicalOnly=checkIsMedicalOnly.Checked;
			ClinicCur.IsInsVerifyExcluded=checkExcludeFromInsVerifyList.Checked;
			ClinicCur.HasProcOnRx=checkProcCodeRequired.Checked;
			ClinicCur.Abbr=textClinicAbbr.Text;
			ClinicCur.Description=textDescription.Text;
			ClinicCur.Phone=phone;
			ClinicCur.Fax=fax;
			ClinicCur.Address=textAddress.Text;
			ClinicCur.Address2=textAddress2.Text;
			ClinicCur.City=textCity.Text;
			ClinicCur.State=textState.Text;
			ClinicCur.Zip=textZip.Text;
			ClinicCur.BillingAddress=textBillingAddress.Text;
			ClinicCur.BillingAddress2=textBillingAddress2.Text;
			ClinicCur.BillingCity=textBillingCity.Text;
			ClinicCur.BillingState=textBillingST.Text;
			ClinicCur.BillingZip=textBillingZip.Text;
			ClinicCur.PayToAddress=textPayToAddress.Text;
			ClinicCur.PayToAddress2=textPayToAddress2.Text;
			ClinicCur.PayToCity=textPayToCity.Text;
			ClinicCur.PayToState=textPayToST.Text;
			ClinicCur.PayToZip=textPayToZip.Text;
			ClinicCur.BankNumber=textBankNumber.Text;
			ClinicCur.DefaultPlaceService=(PlaceOfService)comboPlaceService.SelectedIndex;
			ClinicCur.UseBillAddrOnClaims=checkUseBillingAddressOnClaims.Checked;
			ClinicCur.IsHidden=checkHidden.Checked;
			ClinicCur.ExternalID=externalID;
			long defNumRegion=0;
			if(comboRegion.SelectedIndex>0){
				defNumRegion=_listRegionDefs[comboRegion.SelectedIndex-1].DefNum;
			}
			ClinicCur.Region=defNumRegion;
			if(radioInsBillingProvDefault.Checked){//default=0
				ClinicCur.InsBillingProv=0;
			}
			else if(radioInsBillingProvTreat.Checked){//treat=-1
				ClinicCur.InsBillingProv=-1;
			}
			else{
				ClinicCur.InsBillingProv=_selectedProvBillNum;
			}
			ClinicCur.DefaultProv=_selectedProvDefNum;
			if(_isMedLabHL7DefEnabled) {
				ClinicCur.MedLabAccountNum=textMedLabAcctNum.Text.Trim();
			}
			ClinicCur.SchedNote=textSchedRules.Text;
			#endregion Set Values
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}
