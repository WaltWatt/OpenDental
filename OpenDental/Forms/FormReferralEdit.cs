using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
///<summary></summary>
	public class FormReferralEdit : ODForm {
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupSSN;
		private System.Windows.Forms.RadioButton radioTIN;
		private System.Windows.Forms.RadioButton radioSSN;
		private System.Windows.Forms.TextBox textSSN;
		private System.Windows.Forms.ListBox listSpecialty;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox textPhone3;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.TextBox textPhone2;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox textPhone1;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox textLName;
		private System.Windows.Forms.TextBox textFName;
		private System.Windows.Forms.TextBox textMName;
		private System.Windows.Forms.TextBox textST;
		private System.ComponentModel.Container components = null;
		///<summary></summary>
		public bool IsNew;
		///<summary></summary>
    private bool IsPatient;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textEmail;
		private System.Windows.Forms.TextBox textOtherPhone;
		private System.Windows.Forms.TextBox textZip;
		private System.Windows.Forms.TextBox textCity;
		private System.Windows.Forms.TextBox textAddress2;
		private System.Windows.Forms.TextBox textAddress;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox textTitle;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label labelPatient;
		private System.Windows.Forms.CheckBox checkNotPerson;
		private OpenDental.ODtextBox textNotes;
		private System.Windows.Forms.CheckBox checkHidden;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox textPatientsNumTo;
		private System.Windows.Forms.ComboBox comboPatientsTo;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.TextBox textPatientsNumFrom;
		private System.Windows.Forms.ComboBox comboPatientsFrom;
		private TextBox textNationalProvID;
		private Label label19;
		private Label label20;
		private Label label21;
		private ComboBox comboSlip;
		///<summary></summary>
		public Referral RefCur;
		private UI.Button butDelete;
		private CheckBox checkIsDoctor;
		private CheckBox checkEmailTrustDirect;
		private CheckBox checkIsPreferred;
		private List<SheetDef> SlipList;

		///<summary></summary>
		public FormReferralEdit(Referral refCur){
			InitializeComponent();
			RefCur=refCur;
			if(refCur.PatNum>0){
				IsPatient=true;
			}
			Lan.F(this);
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				groupSSN.Text=Lan.g(this,"CDA Number");
				radioSSN.Visible=false;
				radioTIN.Visible=false;
			}
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
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReferralEdit));
			this.textLName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textFName = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textMName = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textST = new System.Windows.Forms.TextBox();
			this.groupSSN = new System.Windows.Forms.GroupBox();
			this.radioTIN = new System.Windows.Forms.RadioButton();
			this.radioSSN = new System.Windows.Forms.RadioButton();
			this.textSSN = new System.Windows.Forms.TextBox();
			this.listSpecialty = new System.Windows.Forms.ListBox();
			this.label10 = new System.Windows.Forms.Label();
			this.textPhone3 = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.textPhone2 = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.textPhone1 = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.textTitle = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textEmail = new System.Windows.Forms.TextBox();
			this.textOtherPhone = new System.Windows.Forms.TextBox();
			this.textZip = new System.Windows.Forms.TextBox();
			this.textCity = new System.Windows.Forms.TextBox();
			this.textAddress2 = new System.Windows.Forms.TextBox();
			this.textAddress = new System.Windows.Forms.TextBox();
			this.label22 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.checkHidden = new System.Windows.Forms.CheckBox();
			this.labelPatient = new System.Windows.Forms.Label();
			this.checkNotPerson = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label18 = new System.Windows.Forms.Label();
			this.textPatientsNumFrom = new System.Windows.Forms.TextBox();
			this.comboPatientsFrom = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textPatientsNumTo = new System.Windows.Forms.TextBox();
			this.comboPatientsTo = new System.Windows.Forms.ComboBox();
			this.textNationalProvID = new System.Windows.Forms.TextBox();
			this.label19 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.comboSlip = new System.Windows.Forms.ComboBox();
			this.butDelete = new OpenDental.UI.Button();
			this.textNotes = new OpenDental.ODtextBox();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.checkIsDoctor = new System.Windows.Forms.CheckBox();
			this.checkEmailTrustDirect = new System.Windows.Forms.CheckBox();
			this.checkIsPreferred = new System.Windows.Forms.CheckBox();
			this.groupSSN.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// textLName
			// 
			this.textLName.Location = new System.Drawing.Point(129, 78);
			this.textLName.Name = "textLName";
			this.textLName.Size = new System.Drawing.Size(297, 20);
			this.textLName.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 80);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Last Name";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 106);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "First Name";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textFName
			// 
			this.textFName.Location = new System.Drawing.Point(129, 104);
			this.textFName.Name = "textFName";
			this.textFName.Size = new System.Drawing.Size(297, 20);
			this.textFName.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(24, 132);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(104, 16);
			this.label3.TabIndex = 0;
			this.label3.Text = "Middle Name";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textMName
			// 
			this.textMName.Location = new System.Drawing.Point(129, 130);
			this.textMName.Name = "textMName";
			this.textMName.Size = new System.Drawing.Size(169, 20);
			this.textMName.TabIndex = 3;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(24, 262);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(104, 16);
			this.label4.TabIndex = 0;
			this.label4.Text = "ST";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textST
			// 
			this.textST.Location = new System.Drawing.Point(129, 260);
			this.textST.Name = "textST";
			this.textST.Size = new System.Drawing.Size(118, 20);
			this.textST.TabIndex = 8;
			// 
			// groupSSN
			// 
			this.groupSSN.Controls.Add(this.radioTIN);
			this.groupSSN.Controls.Add(this.radioSSN);
			this.groupSSN.Controls.Add(this.textSSN);
			this.groupSSN.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupSSN.Location = new System.Drawing.Point(121, 409);
			this.groupSSN.Name = "groupSSN";
			this.groupSSN.Size = new System.Drawing.Size(141, 80);
			this.groupSSN.TabIndex = 0;
			this.groupSSN.TabStop = false;
			this.groupSSN.Text = "SSN or TIN (no dashes)";
			// 
			// radioTIN
			// 
			this.radioTIN.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioTIN.Location = new System.Drawing.Point(9, 34);
			this.radioTIN.Name = "radioTIN";
			this.radioTIN.Size = new System.Drawing.Size(104, 16);
			this.radioTIN.TabIndex = 18;
			this.radioTIN.Text = "TIN";
			this.radioTIN.Click += new System.EventHandler(this.radioTIN_Click);
			// 
			// radioSSN
			// 
			this.radioSSN.Checked = true;
			this.radioSSN.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioSSN.Location = new System.Drawing.Point(9, 17);
			this.radioSSN.Name = "radioSSN";
			this.radioSSN.Size = new System.Drawing.Size(104, 14);
			this.radioSSN.TabIndex = 17;
			this.radioSSN.TabStop = true;
			this.radioSSN.Text = "SSN";
			this.radioSSN.Click += new System.EventHandler(this.radioSSN_Click);
			// 
			// textSSN
			// 
			this.textSSN.Location = new System.Drawing.Point(8, 54);
			this.textSSN.Name = "textSSN";
			this.textSSN.Size = new System.Drawing.Size(100, 20);
			this.textSSN.TabIndex = 15;
			// 
			// listSpecialty
			// 
			this.listSpecialty.Items.AddRange(new object[] {
            "Dental General Practice",
            "Dental Hygienist",
            "Endodontics",
            "Pediatric Dentistry",
            "Periodontics",
            "Prosthodontics",
            "Orthodontics",
            "Denturist",
            "Surgery, Oral & Maxillofacial",
            "Dental Assistant",
            "Dental Laboratory Technician",
            "Pathology, Oral & MaxFac",
            "Public Health",
            "Radiology"});
			this.listSpecialty.Location = new System.Drawing.Point(501, 98);
			this.listSpecialty.Name = "listSpecialty";
			this.listSpecialty.Size = new System.Drawing.Size(154, 186);
			this.listSpecialty.TabIndex = 0;
			this.listSpecialty.TabStop = false;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(499, 78);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(99, 16);
			this.label10.TabIndex = 0;
			this.label10.Text = "Specialty";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textPhone3
			// 
			this.textPhone3.Location = new System.Drawing.Point(210, 312);
			this.textPhone3.MaxLength = 4;
			this.textPhone3.Name = "textPhone3";
			this.textPhone3.Size = new System.Drawing.Size(39, 20);
			this.textPhone3.TabIndex = 12;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(200, 314);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(6, 16);
			this.label13.TabIndex = 0;
			this.label13.Text = "-";
			this.label13.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// textPhone2
			// 
			this.textPhone2.Location = new System.Drawing.Point(170, 312);
			this.textPhone2.MaxLength = 3;
			this.textPhone2.Name = "textPhone2";
			this.textPhone2.Size = new System.Drawing.Size(28, 20);
			this.textPhone2.TabIndex = 11;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(158, 314);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(6, 16);
			this.label11.TabIndex = 0;
			this.label11.Text = ")";
			this.label11.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(119, 314);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(11, 16);
			this.label12.TabIndex = 0;
			this.label12.Text = "(";
			this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textPhone1
			// 
			this.textPhone1.Location = new System.Drawing.Point(129, 312);
			this.textPhone1.MaxLength = 3;
			this.textPhone1.Name = "textPhone1";
			this.textPhone1.Size = new System.Drawing.Size(28, 20);
			this.textPhone1.TabIndex = 10;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(24, 314);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(95, 16);
			this.label14.TabIndex = 0;
			this.label14.Text = "Phone";
			this.label14.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textTitle
			// 
			this.textTitle.Location = new System.Drawing.Point(129, 156);
			this.textTitle.MaxLength = 100;
			this.textTitle.Name = "textTitle";
			this.textTitle.Size = new System.Drawing.Size(70, 20);
			this.textTitle.TabIndex = 4;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(40, 158);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(89, 16);
			this.label5.TabIndex = 0;
			this.label5.Text = "Title (DDS)";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textEmail
			// 
			this.textEmail.Location = new System.Drawing.Point(129, 366);
			this.textEmail.MaxLength = 100;
			this.textEmail.Name = "textEmail";
			this.textEmail.Size = new System.Drawing.Size(297, 20);
			this.textEmail.TabIndex = 14;
			// 
			// textOtherPhone
			// 
			this.textOtherPhone.Location = new System.Drawing.Point(129, 339);
			this.textOtherPhone.MaxLength = 30;
			this.textOtherPhone.Name = "textOtherPhone";
			this.textOtherPhone.Size = new System.Drawing.Size(161, 20);
			this.textOtherPhone.TabIndex = 13;
			this.textOtherPhone.TextChanged += new System.EventHandler(this.textOtherPhone_TextChanged);
			// 
			// textZip
			// 
			this.textZip.Location = new System.Drawing.Point(129, 286);
			this.textZip.MaxLength = 10;
			this.textZip.Name = "textZip";
			this.textZip.Size = new System.Drawing.Size(161, 20);
			this.textZip.TabIndex = 9;
			// 
			// textCity
			// 
			this.textCity.Location = new System.Drawing.Point(129, 234);
			this.textCity.MaxLength = 50;
			this.textCity.Name = "textCity";
			this.textCity.Size = new System.Drawing.Size(190, 20);
			this.textCity.TabIndex = 7;
			// 
			// textAddress2
			// 
			this.textAddress2.Location = new System.Drawing.Point(129, 208);
			this.textAddress2.MaxLength = 100;
			this.textAddress2.Name = "textAddress2";
			this.textAddress2.Size = new System.Drawing.Size(297, 20);
			this.textAddress2.TabIndex = 6;
			// 
			// textAddress
			// 
			this.textAddress.Location = new System.Drawing.Point(129, 182);
			this.textAddress.MaxLength = 100;
			this.textAddress.Name = "textAddress";
			this.textAddress.Size = new System.Drawing.Size(297, 20);
			this.textAddress.TabIndex = 5;
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(24, 368);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(104, 16);
			this.label22.TabIndex = 0;
			this.label22.Text = "E-mail";
			this.label22.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(24, 342);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(104, 16);
			this.label16.TabIndex = 0;
			this.label16.Text = "Other Phone";
			this.label16.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(24, 288);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(104, 16);
			this.label15.TabIndex = 0;
			this.label15.Text = "Zip";
			this.label15.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(24, 236);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(104, 16);
			this.label7.TabIndex = 0;
			this.label7.Text = "City";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(24, 210);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(104, 16);
			this.label8.TabIndex = 0;
			this.label8.Text = "Address2";
			this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(24, 184);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(104, 16);
			this.label9.TabIndex = 0;
			this.label9.Text = "Address";
			this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(26, 539);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(101, 14);
			this.label17.TabIndex = 0;
			this.label17.Text = "Notes";
			this.label17.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// checkHidden
			// 
			this.checkHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkHidden.Location = new System.Drawing.Point(39, 29);
			this.checkHidden.Name = "checkHidden";
			this.checkHidden.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkHidden.Size = new System.Drawing.Size(104, 18);
			this.checkHidden.TabIndex = 24;
			this.checkHidden.Text = "Hidden  ";
			// 
			// labelPatient
			// 
			this.labelPatient.Location = new System.Drawing.Point(43, 11);
			this.labelPatient.Name = "labelPatient";
			this.labelPatient.Size = new System.Drawing.Size(612, 17);
			this.labelPatient.TabIndex = 70;
			this.labelPatient.Text = "This referral is a patient.  Some information can only be changed from the patien" +
    "t\'s edit form.";
			this.labelPatient.Visible = false;
			// 
			// checkNotPerson
			// 
			this.checkNotPerson.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkNotPerson.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkNotPerson.Location = new System.Drawing.Point(30, 52);
			this.checkNotPerson.Name = "checkNotPerson";
			this.checkNotPerson.Size = new System.Drawing.Size(113, 21);
			this.checkNotPerson.TabIndex = 71;
			this.checkNotPerson.Text = "Not Person";
			this.checkNotPerson.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label18);
			this.groupBox2.Controls.Add(this.textPatientsNumFrom);
			this.groupBox2.Controls.Add(this.comboPatientsFrom);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.textPatientsNumTo);
			this.groupBox2.Controls.Add(this.comboPatientsTo);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(481, 298);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(383, 109);
			this.groupBox2.TabIndex = 74;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Used By Patients";
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(15, 57);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(323, 19);
			this.label18.TabIndex = 72;
			this.label18.Text = "Patients referred FROM this referral";
			this.label18.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textPatientsNumFrom
			// 
			this.textPatientsNumFrom.BackColor = System.Drawing.Color.White;
			this.textPatientsNumFrom.Location = new System.Drawing.Point(17, 78);
			this.textPatientsNumFrom.Name = "textPatientsNumFrom";
			this.textPatientsNumFrom.ReadOnly = true;
			this.textPatientsNumFrom.Size = new System.Drawing.Size(35, 20);
			this.textPatientsNumFrom.TabIndex = 71;
			// 
			// comboPatientsFrom
			// 
			this.comboPatientsFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPatientsFrom.Location = new System.Drawing.Point(61, 78);
			this.comboPatientsFrom.MaxDropDownItems = 30;
			this.comboPatientsFrom.Name = "comboPatientsFrom";
			this.comboPatientsFrom.Size = new System.Drawing.Size(299, 21);
			this.comboPatientsFrom.TabIndex = 70;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(15, 14);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(323, 19);
			this.label6.TabIndex = 69;
			this.label6.Text = "Patients referred TO this referral";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textPatientsNumTo
			// 
			this.textPatientsNumTo.BackColor = System.Drawing.Color.White;
			this.textPatientsNumTo.Location = new System.Drawing.Point(17, 35);
			this.textPatientsNumTo.Name = "textPatientsNumTo";
			this.textPatientsNumTo.ReadOnly = true;
			this.textPatientsNumTo.Size = new System.Drawing.Size(35, 20);
			this.textPatientsNumTo.TabIndex = 68;
			// 
			// comboPatientsTo
			// 
			this.comboPatientsTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPatientsTo.Location = new System.Drawing.Point(61, 35);
			this.comboPatientsTo.MaxDropDownItems = 30;
			this.comboPatientsTo.Name = "comboPatientsTo";
			this.comboPatientsTo.Size = new System.Drawing.Size(299, 21);
			this.comboPatientsTo.TabIndex = 34;
			// 
			// textNationalProvID
			// 
			this.textNationalProvID.Location = new System.Drawing.Point(129, 495);
			this.textNationalProvID.Name = "textNationalProvID";
			this.textNationalProvID.Size = new System.Drawing.Size(100, 20);
			this.textNationalProvID.TabIndex = 75;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(11, 495);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(117, 20);
			this.label19.TabIndex = 76;
			this.label19.Text = "National Provider ID";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(157, 315);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(11, 16);
			this.label20.TabIndex = 77;
			this.label20.Text = ")";
			this.label20.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(496, 425);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(345, 16);
			this.label21.TabIndex = 78;
			this.label21.Text = "Referral Slip (custom referral slips may be added in Sheets)";
			this.label21.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// comboSlip
			// 
			this.comboSlip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSlip.Location = new System.Drawing.Point(498, 445);
			this.comboSlip.MaxDropDownItems = 30;
			this.comboSlip.Name = "comboSlip";
			this.comboSlip.Size = new System.Drawing.Size(275, 21);
			this.comboSlip.TabIndex = 79;
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
			this.butDelete.Location = new System.Drawing.Point(21, 654);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(80, 24);
			this.butDelete.TabIndex = 80;
			this.butDelete.Text = "Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// textNotes
			// 
			this.textNotes.AcceptsTab = true;
			this.textNotes.BackColor = System.Drawing.SystemColors.Window;
			this.textNotes.DetectLinksEnabled = false;
			this.textNotes.DetectUrls = false;
			this.textNotes.Location = new System.Drawing.Point(129, 539);
			this.textNotes.Name = "textNotes";
			this.textNotes.QuickPasteType = OpenDentBusiness.QuickPasteType.Referral;
			this.textNotes.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textNotes.Size = new System.Drawing.Size(412, 120);
			this.textNotes.TabIndex = 73;
			this.textNotes.Text = "";
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
			this.butCancel.Location = new System.Drawing.Point(787, 654);
			this.butCancel.Name = "butCancel";
			this.butCancel.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 18;
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
			this.butOK.Location = new System.Drawing.Point(698, 654);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 17;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// checkIsDoctor
			// 
			this.checkIsDoctor.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIsDoctor.Location = new System.Drawing.Point(502, 54);
			this.checkIsDoctor.Name = "checkIsDoctor";
			this.checkIsDoctor.Size = new System.Drawing.Size(113, 21);
			this.checkIsDoctor.TabIndex = 81;
			this.checkIsDoctor.Text = "Is Doctor";
			// 
			// checkEmailTrustDirect
			// 
			this.checkEmailTrustDirect.Location = new System.Drawing.Point(16, 390);
			this.checkEmailTrustDirect.Name = "checkEmailTrustDirect";
			this.checkEmailTrustDirect.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkEmailTrustDirect.Size = new System.Drawing.Size(127, 16);
			this.checkEmailTrustDirect.TabIndex = 82;
			this.checkEmailTrustDirect.Text = "E-mail Trust for Direct";
			this.checkEmailTrustDirect.UseVisualStyleBackColor = true;
			// 
			// checkIsPreferred
			// 
			this.checkIsPreferred.Location = new System.Drawing.Point(16, 519);
			this.checkIsPreferred.Name = "checkIsPreferred";
			this.checkIsPreferred.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkIsPreferred.Size = new System.Drawing.Size(127, 16);
			this.checkIsPreferred.TabIndex = 83;
			this.checkIsPreferred.Text = "Preferred Referral";
			this.checkIsPreferred.UseVisualStyleBackColor = true;
			// 
			// FormReferralEdit
			// 
			this.ClientSize = new System.Drawing.Size(877, 696);
			this.Controls.Add(this.checkIsPreferred);
			this.Controls.Add(this.checkEmailTrustDirect);
			this.Controls.Add(this.checkIsDoctor);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.comboSlip);
			this.Controls.Add(this.label21);
			this.Controls.Add(this.label20);
			this.Controls.Add(this.label19);
			this.Controls.Add(this.textNationalProvID);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.textNotes);
			this.Controls.Add(this.checkHidden);
			this.Controls.Add(this.checkNotPerson);
			this.Controls.Add(this.labelPatient);
			this.Controls.Add(this.textEmail);
			this.Controls.Add(this.textOtherPhone);
			this.Controls.Add(this.textZip);
			this.Controls.Add(this.textCity);
			this.Controls.Add(this.textAddress2);
			this.Controls.Add(this.textAddress);
			this.Controls.Add(this.textTitle);
			this.Controls.Add(this.textPhone3);
			this.Controls.Add(this.textPhone2);
			this.Controls.Add(this.textPhone1);
			this.Controls.Add(this.textST);
			this.Controls.Add(this.textMName);
			this.Controls.Add(this.textFName);
			this.Controls.Add(this.textLName);
			this.Controls.Add(this.label17);
			this.Controls.Add(this.label22);
			this.Controls.Add(this.label16);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.listSpecialty);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.groupSSN);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormReferralEdit";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Edit Referral";
			this.Load += new System.EventHandler(this.FormReferralEdit_Load);
			this.groupSSN.ResumeLayout(false);
			this.groupSSN.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormReferralEdit_Load(object sender, System.EventArgs e) {
			if(Plugins.HookMethod(this,"FormReferralEdit.Load",RefCur,IsNew)) {
				return;
			}
			listSpecialty.Items.Clear();
			Def[] specDefs=Defs.GetDefsForCategory(DefCat.ProviderSpecialties,true).ToArray();
			for(int i=0;i<specDefs.Length;i++) {
				listSpecialty.Items.Add(Lan.g("enumDentalSpecialty",specDefs[i].ItemName));
			}
			if(IsPatient){
				if(IsNew){
					Text=Lan.g(this,"Add Referral"); 
					Family FamCur=Patients.GetFamily(RefCur.PatNum);
					Patient PatCur=FamCur.GetPatient(RefCur.PatNum);
					RefCur.Address=PatCur.Address;
					RefCur.Address2=PatCur.Address2;	
					RefCur.City=PatCur.City;	
					RefCur.EMail=PatCur.Email;	
					RefCur.FName=PatCur.FName;	
					RefCur.LName=PatCur.LName;	
					RefCur.MName=PatCur.MiddleI;	
					//RefCur.PatNum=Patients.Cur.PatNum;//already handled
					RefCur.SSN=PatCur.SSN;
					RefCur.Telephone=TelephoneNumbers.FormatNumbersExactTen(PatCur.HmPhone);
					if(PatCur.WkPhone==""){
						RefCur.Phone2=PatCur.WirelessPhone;
					}
					else{
						RefCur.Phone2=PatCur.WkPhone;
					}
					RefCur.ST=PatCur.State;	
					RefCur.Zip=PatCur.Zip;
				}
				labelPatient.Visible=true;
				textLName.ReadOnly=true;
				textFName.ReadOnly=true;
				textMName.ReadOnly=true;
				textTitle.ReadOnly=true;
				textAddress.ReadOnly=true;
				textAddress2.ReadOnly=true;
				textCity.ReadOnly=true;         
				textST.ReadOnly=true;
				textZip.ReadOnly=true;
				checkNotPerson.Enabled=false;
				textPhone1.ReadOnly=true;
				textPhone2.ReadOnly=true;
				textPhone3.ReadOnly=true;
				textSSN.ReadOnly=true;				
				radioTIN.Enabled=false;
				textEmail.ReadOnly=true;
				listSpecialty.Enabled=false;
				listSpecialty.SelectedIndex=-1;
				checkIsDoctor.Enabled=false;
				textNotes.Select();
			}
			else{//non patient
				if(IsNew){
					this.Text=Lan.g(this,"Add Referral"); 
					RefCur=new Referral();
					RefCur.Specialty=Defs.GetByExactNameNeverZero(DefCat.ProviderSpecialties,"General");
				}
				Def specDefCur=Defs.GetDef(DefCat.ProviderSpecialties,RefCur.Specialty);
				if(specDefCur!=null) {
					for(int i=0;i<specDefs.Length;i++) {
						if(i==0 || specDefs[i].ItemName==specDefCur.ItemName) {//default to the first specialty in the list
							listSpecialty.SelectedIndex=i;
						}
					}
				}
				textLName.Select();
			}
			checkIsDoctor.Checked=RefCur.IsDoctor;
			checkNotPerson.Checked=RefCur.NotPerson;
			checkHidden.Checked=RefCur.IsHidden;
			checkEmailTrustDirect.Checked=RefCur.IsTrustedDirect;
			textLName.Text=RefCur.LName;
			textFName.Text=RefCur.FName;
			textMName.Text=RefCur.MName;
			textTitle.Text=RefCur.Title;
			textAddress.Text=RefCur.Address;
			textAddress2.Text=RefCur.Address2;
			textCity.Text=RefCur.City;         
			textST.Text=RefCur.ST;
			textZip.Text=RefCur.Zip;
			string phone=RefCur.Telephone;
			if(phone!=null && phone.Length==10){
				textPhone1.Text=phone.Substring(0,3);
				textPhone2.Text=phone.Substring(3,3);
				textPhone3.Text=phone.Substring(6);
			}
			textSSN.Text=RefCur.SSN;
			if(RefCur.UsingTIN){ 
				radioTIN.Checked=true;
			} 
			else{
				radioSSN.Checked=true;
			}
			textNationalProvID.Text=RefCur.NationalProvID;
			textOtherPhone.Text=RefCur.Phone2;  
			textEmail.Text=RefCur.EMail; 
			textNotes.Text=RefCur.Note;
			//Patients using:
			string[] patsTo=RefAttaches.GetPats(RefCur.ReferralNum,ReferralType.RefTo);
			string[] patsFrom=RefAttaches.GetPats(RefCur.ReferralNum,ReferralType.RefFrom);
			comboPatientsTo.Items.Clear();
			comboPatientsFrom.Items.Clear();
			Dictionary<string,int> dictPatRefCount=new Dictionary<string,int>();
			for(int i=0;i<patsTo.Length;i++) {
				if(dictPatRefCount.ContainsKey(patsTo[i])) {
					dictPatRefCount[patsTo[i]]+=1;
				}
				else {
					dictPatRefCount[patsTo[i]]=1;
				}
			}
			foreach(KeyValuePair<string,int> pat in dictPatRefCount) {
				//Add each patient referral once with the count of how many times they've been referred.
				comboPatientsTo.Items.Add(pat.Key+" ("+pat.Value+")");
			}
			textPatientsNumTo.Text=dictPatRefCount.Count.ToString();
			dictPatRefCount=new Dictionary<string,int>();
			for(int i=0;i<patsFrom.Length;i++) {
				if(dictPatRefCount.ContainsKey(patsFrom[i])) {
					dictPatRefCount[patsFrom[i]]+=1;
				}
				else {
					dictPatRefCount[patsFrom[i]]=1;
				}
			}
			foreach(KeyValuePair<string,int> pat in dictPatRefCount) {
				comboPatientsFrom.Items.Add(pat.Key+" ("+pat.Value+")");
			}
			textPatientsNumFrom.Text=dictPatRefCount.Count.ToString();
			if(patsTo.Length>0){
				comboPatientsTo.SelectedIndex=0;
			}
			if(patsFrom.Length>0){
				comboPatientsFrom.SelectedIndex=0;
			}
			comboSlip.Items.Add(Lan.g(this,"Default"));
			comboSlip.SelectedIndex=0;
			SlipList=SheetDefs.GetCustomForType(SheetTypeEnum.ReferralSlip);
			for(int i=0;i<SlipList.Count;i++){
				comboSlip.Items.Add(SlipList[i].Description);
				if(RefCur.Slip==SlipList[i].SheetDefNum){
					comboSlip.SelectedIndex=i+1;
				}
			}
			if(!Security.IsAuthorized(Permissions.ReferralEdit,true)) {
				butDelete.Enabled=false;
				butOK.Enabled=false;
				this.Text+=" - "+Lan.g(this,"Read Only");
			}
			checkIsPreferred.Checked=RefCur.IsPreferred;
		}

		private void radioSSN_Click(object sender, System.EventArgs e) {
			RefCur.UsingTIN=false;
		}

		private void radioTIN_Click(object sender, System.EventArgs e) {
			RefCur.UsingTIN=true;
		}

		private void textOtherPhone_TextChanged(object sender, System.EventArgs e) {
			int cursor=textOtherPhone.SelectionStart;
			int length=textOtherPhone.Text.Length;
			textOtherPhone.Text=TelephoneNumbers.AutoFormat(textOtherPhone.Text);
			if(textOtherPhone.Text.Length>length)
				cursor++;
			textOtherPhone.SelectionStart=cursor;		
		}

		private void butNone_Click(object sender, System.EventArgs e) {
			listSpecialty.SelectedIndex=-1;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete?")) {
				return;
			}
			try {
				Referrals.Delete(RefCur);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			string secLogText="Referral '"+RefCur.LName+", "+RefCur.FName+"' deleted";
			SecurityLogs.MakeLogEntry(Permissions.ReferralEdit,RefCur.PatNum,secLogText); //RefCur.PatNum should be 0 if not a patient.
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			string phone=textPhone1.Text+textPhone2.Text+textPhone3.Text;
			if(phone.Length > 0 && phone.Length < 10){
				MessageBox.Show(Lan.g(this,"Invalid phone"));
				return;
			}
			if(textLName.Text=="") {
				MsgBox.Show(this,"Please enter a last name.");
				return;
			}
			if(listSpecialty.SelectedIndex==-1 && !IsPatient) {
				MsgBox.Show(this,"Please select a specialty.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			if(checkEmailTrustDirect.Checked && !EmailMessages.TryAddTrustDirect(textEmail.Text)) {
				string trustEmailErrorMessage=Lan.g("Referral","You elected to trust this email address for Direct messaging.")
					+"  "+Lan.g("Referral","Adding trust for the address failed, because we were unable to locate the public certificate for the address.")
					+"  "+Lan.g("Referral","Check that the email address is correctly typed and is a valid Direct messaging address, then try again.")
					+"  "+Lan.g("Referral","Otherwise, uncheck the E-mail Trust for Direct checkbox before clicking OK or press Cancel if no other changes were made.");
				MessageBox.Show(trustEmailErrorMessage);
				Cursor=Cursors.Default;
				return;
			}
			Cursor=Cursors.Default;
			RefCur.IsHidden=checkHidden.Checked;
			RefCur.NotPerson=checkNotPerson.Checked;
			RefCur.IsTrustedDirect=checkEmailTrustDirect.Checked;
			RefCur.LName=textLName.Text;
			RefCur.FName=textFName.Text;
			RefCur.MName=textMName.Text;
			RefCur.Title=textTitle.Text;
      RefCur.Address=textAddress.Text;
      RefCur.Address2=textAddress2.Text;
      RefCur.City=textCity.Text;
			RefCur.ST=textST.Text;
      RefCur.Zip=textZip.Text;
			RefCur.Telephone=phone;
      RefCur.Phone2=textOtherPhone.Text;    
			RefCur.SSN=textSSN.Text;
			RefCur.NationalProvID=textNationalProvID.Text;
      RefCur.EMail=textEmail.Text;
      RefCur.Note=textNotes.Text; 
			RefCur.Slip=0;
			RefCur.IsPreferred=checkIsPreferred.Checked;
			if(comboSlip.SelectedIndex>0){
				RefCur.Slip=SlipList[comboSlip.SelectedIndex-1].SheetDefNum;
			}
			//RefCur.UsingTIN already taken care of
      if(!IsPatient){
				RefCur.IsDoctor=checkIsDoctor.Checked;
				RefCur.Specialty=Defs.GetByExactNameNeverZero(DefCat.ProviderSpecialties,listSpecialty.SelectedItem.ToString());
      }
			if(IsNew){
				if(Referrals.GetExists(x => x.LName+x.FName==RefCur.LName+RefCur.FName)) {
					if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Referral of same name exists. Add anyway?")) {
						DialogResult=DialogResult.Cancel;
						return;
					}
				}
				string secLogText="Referral '"+RefCur.LName+", "+RefCur.FName+"' created";
				SecurityLogs.MakeLogEntry(Permissions.ReferralAdd,RefCur.PatNum,secLogText);
				Referrals.Insert(RefCur);
			}
			else{
				string secLogText="Referral '"+RefCur.LName+", "+RefCur.FName+"' edited";
				SecurityLogs.MakeLogEntry(Permissions.ReferralEdit,RefCur.PatNum,secLogText);
				Referrals.Update(RefCur);
			}
			//
			Referrals.RefreshCache();
			//MessageBox.Show(RefCur.ReferralNum.ToString());
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	
		

		

		


	}
}
