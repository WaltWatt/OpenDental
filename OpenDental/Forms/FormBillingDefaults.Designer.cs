namespace OpenDental{
	partial class FormBillingDefaults {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBillingDefaults));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.listModesToText = new System.Windows.Forms.ListBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textSmsTemplate = new OpenDental.ODtextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.checkSinglePatient = new System.Windows.Forms.CheckBox();
			this.textInvoiceNote = new OpenDental.ODtextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.textBillingEmailBody = new OpenDental.ODtextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.textBillingEmailSubject = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.groupBoxBilling = new System.Windows.Forms.GroupBox();
			this.checkCreatePDF = new System.Windows.Forms.CheckBox();
			this.labelBlankForDefault = new System.Windows.Forms.Label();
			this.textStatementURL = new System.Windows.Forms.TextBox();
			this.labelStatementURL = new System.Windows.Forms.Label();
			this.comboPracticeAddr = new System.Windows.Forms.ComboBox();
			this.comboRemitAddr = new System.Windows.Forms.ComboBox();
			this.labelPracticeAddr = new System.Windows.Forms.Label();
			this.labelRemitAddr = new System.Windows.Forms.Label();
			this.labelClinic = new System.Windows.Forms.Label();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.listElectBilling = new System.Windows.Forms.ListBox();
			this.textPassword = new System.Windows.Forms.TextBox();
			this.labelPassword = new System.Windows.Forms.Label();
			this.textUserName = new System.Windows.Forms.TextBox();
			this.labelUserName = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkAmEx = new System.Windows.Forms.CheckBox();
			this.checkD = new System.Windows.Forms.CheckBox();
			this.checkV = new System.Windows.Forms.CheckBox();
			this.checkMC = new System.Windows.Forms.CheckBox();
			this.textClientAcctNumber = new System.Windows.Forms.TextBox();
			this.labelAcctNum = new System.Windows.Forms.Label();
			this.textVendorPMScode = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textVendorId = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textDays = new OpenDental.ValidNum();
			this.checkIntermingled = new System.Windows.Forms.CheckBox();
			this.labelStartDate = new System.Windows.Forms.Label();
			this.textNote = new OpenDental.ODtextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.checkBoxBillShowTransSinceZero = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBoxBilling.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.listModesToText);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.textSmsTemplate);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Location = new System.Drawing.Point(12, 669);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(630, 157);
			this.groupBox1.TabIndex = 250;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "SMS Statements";
			// 
			// listModesToText
			// 
			this.listModesToText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.listModesToText.FormattingEnabled = true;
			this.listModesToText.Location = new System.Drawing.Point(426, 59);
			this.listModesToText.Name = "listModesToText";
			this.listModesToText.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listModesToText.Size = new System.Drawing.Size(113, 56);
			this.listModesToText.TabIndex = 257;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.Location = new System.Drawing.Point(425, 40);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(195, 16);
			this.label6.TabIndex = 256;
			this.label6.Text = "Send text message for these modes";
			this.label6.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textSmsTemplate
			// 
			this.textSmsTemplate.AcceptsTab = true;
			this.textSmsTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textSmsTemplate.BackColor = System.Drawing.SystemColors.Window;
			this.textSmsTemplate.DetectLinksEnabled = false;
			this.textSmsTemplate.DetectUrls = false;
			this.textSmsTemplate.Location = new System.Drawing.Point(12, 59);
			this.textSmsTemplate.Name = "textSmsTemplate";
			this.textSmsTemplate.QuickPasteType = OpenDentBusiness.QuickPasteType.Statement;
			this.textSmsTemplate.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textSmsTemplate.Size = new System.Drawing.Size(390, 85);
			this.textSmsTemplate.TabIndex = 1;
			this.textSmsTemplate.Text = "";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(11, 42);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(87, 16);
			this.label5.TabIndex = 240;
			this.label5.Text = "Message";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label7.Location = new System.Drawing.Point(11, 13);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(607, 29);
			this.label7.TabIndex = 249;
			this.label7.Text = "These variables may be used: [nameF], [namePref], [PatNum], [currentMonth], [Offi" +
    "ceName], [OfficePhone], [StatementURL], and [StatementShortURL].";
			// 
			// checkSinglePatient
			// 
			this.checkSinglePatient.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSinglePatient.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkSinglePatient.Location = new System.Drawing.Point(25, 31);
			this.checkSinglePatient.Name = "checkSinglePatient";
			this.checkSinglePatient.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkSinglePatient.Size = new System.Drawing.Size(158, 17);
			this.checkSinglePatient.TabIndex = 252;
			this.checkSinglePatient.Text = "Single patient only";
			this.checkSinglePatient.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkSinglePatient.Visible = false;
			// 
			// textInvoiceNote
			// 
			this.textInvoiceNote.AcceptsTab = true;
			this.textInvoiceNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textInvoiceNote.BackColor = System.Drawing.SystemColors.Window;
			this.textInvoiceNote.DetectLinksEnabled = false;
			this.textInvoiceNote.DetectUrls = false;
			this.textInvoiceNote.Location = new System.Drawing.Point(24, 573);
			this.textInvoiceNote.Name = "textInvoiceNote";
			this.textInvoiceNote.QuickPasteType = OpenDentBusiness.QuickPasteType.Statement;
			this.textInvoiceNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textInvoiceNote.Size = new System.Drawing.Size(611, 90);
			this.textInvoiceNote.TabIndex = 251;
			this.textInvoiceNote.Text = "";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(23, 556);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(152, 16);
			this.label11.TabIndex = 250;
			this.label11.Text = "Invoice Note";
			this.label11.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.textBillingEmailBody);
			this.groupBox3.Controls.Add(this.label8);
			this.groupBox3.Controls.Add(this.label9);
			this.groupBox3.Controls.Add(this.textBillingEmailSubject);
			this.groupBox3.Controls.Add(this.label10);
			this.groupBox3.Location = new System.Drawing.Point(12, 350);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(630, 205);
			this.groupBox3.TabIndex = 248;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Email Statements";
			// 
			// textBillingEmailBody
			// 
			this.textBillingEmailBody.AcceptsTab = true;
			this.textBillingEmailBody.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBillingEmailBody.BackColor = System.Drawing.SystemColors.Window;
			this.textBillingEmailBody.DetectLinksEnabled = false;
			this.textBillingEmailBody.DetectUrls = false;
			this.textBillingEmailBody.Location = new System.Drawing.Point(12, 98);
			this.textBillingEmailBody.Name = "textBillingEmailBody";
			this.textBillingEmailBody.QuickPasteType = OpenDentBusiness.QuickPasteType.Statement;
			this.textBillingEmailBody.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textBillingEmailBody.Size = new System.Drawing.Size(611, 102);
			this.textBillingEmailBody.TabIndex = 1;
			this.textBillingEmailBody.Text = "";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(11, 81);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(87, 16);
			this.label8.TabIndex = 240;
			this.label8.Text = "Body";
			this.label8.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(11, 43);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(218, 16);
			this.label9.TabIndex = 240;
			this.label9.Text = "Subject";
			this.label9.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// textBillingEmailSubject
			// 
			this.textBillingEmailSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBillingEmailSubject.Location = new System.Drawing.Point(12, 60);
			this.textBillingEmailSubject.MaxLength = 200;
			this.textBillingEmailSubject.Name = "textBillingEmailSubject";
			this.textBillingEmailSubject.Size = new System.Drawing.Size(609, 20);
			this.textBillingEmailSubject.TabIndex = 0;
			// 
			// label10
			// 
			this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label10.Location = new System.Drawing.Point(11, 13);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(607, 29);
			this.label10.TabIndex = 249;
			this.label10.Text = "These variables may be used in either box: [monthlyCardsOnFile], [nameF], [nameFL" +
    "], [nameFLnoPref], [namePref], [PatNum], [currentMonth], and [StatementURL].";
			// 
			// groupBoxBilling
			// 
			this.groupBoxBilling.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxBilling.Controls.Add(this.checkCreatePDF);
			this.groupBoxBilling.Controls.Add(this.labelBlankForDefault);
			this.groupBoxBilling.Controls.Add(this.textStatementURL);
			this.groupBoxBilling.Controls.Add(this.labelStatementURL);
			this.groupBoxBilling.Controls.Add(this.comboPracticeAddr);
			this.groupBoxBilling.Controls.Add(this.comboRemitAddr);
			this.groupBoxBilling.Controls.Add(this.labelPracticeAddr);
			this.groupBoxBilling.Controls.Add(this.labelRemitAddr);
			this.groupBoxBilling.Controls.Add(this.labelClinic);
			this.groupBoxBilling.Controls.Add(this.comboClinic);
			this.groupBoxBilling.Controls.Add(this.listElectBilling);
			this.groupBoxBilling.Controls.Add(this.textPassword);
			this.groupBoxBilling.Controls.Add(this.labelPassword);
			this.groupBoxBilling.Controls.Add(this.textUserName);
			this.groupBoxBilling.Controls.Add(this.labelUserName);
			this.groupBoxBilling.Controls.Add(this.groupBox2);
			this.groupBoxBilling.Controls.Add(this.textClientAcctNumber);
			this.groupBoxBilling.Controls.Add(this.labelAcctNum);
			this.groupBoxBilling.Controls.Add(this.textVendorPMScode);
			this.groupBoxBilling.Controls.Add(this.label3);
			this.groupBoxBilling.Controls.Add(this.textVendorId);
			this.groupBoxBilling.Controls.Add(this.label2);
			this.groupBoxBilling.Location = new System.Drawing.Point(12, 180);
			this.groupBoxBilling.Name = "groupBoxBilling";
			this.groupBoxBilling.Size = new System.Drawing.Size(630, 170);
			this.groupBoxBilling.TabIndex = 247;
			this.groupBoxBilling.TabStop = false;
			this.groupBoxBilling.Text = "Electronic Billing";
			// 
			// checkCreatePDF
			// 
			this.checkCreatePDF.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkCreatePDF.Enabled = false;
			this.checkCreatePDF.Location = new System.Drawing.Point(166, 145);
			this.checkCreatePDF.Name = "checkCreatePDF";
			this.checkCreatePDF.Size = new System.Drawing.Size(112, 16);
			this.checkCreatePDF.TabIndex = 4;
			this.checkCreatePDF.Text = "Generate PDF";
			this.checkCreatePDF.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkCreatePDF.UseVisualStyleBackColor = true;
			// 
			// labelBlankForDefault
			// 
			this.labelBlankForDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelBlankForDefault.Location = new System.Drawing.Point(524, 125);
			this.labelBlankForDefault.Name = "labelBlankForDefault";
			this.labelBlankForDefault.Size = new System.Drawing.Size(99, 16);
			this.labelBlankForDefault.TabIndex = 263;
			this.labelBlankForDefault.Text = "(blank for default)";
			this.labelBlankForDefault.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelBlankForDefault.Visible = false;
			// 
			// textStatementURL
			// 
			this.textStatementURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textStatementURL.Location = new System.Drawing.Point(262, 124);
			this.textStatementURL.Name = "textStatementURL";
			this.textStatementURL.Size = new System.Drawing.Size(256, 20);
			this.textStatementURL.TabIndex = 261;
			this.textStatementURL.Text = "https://prelive.dentalxchange.com/dci/upload.svl";
			this.textStatementURL.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textStatementURL_KeyUp);
			// 
			// labelStatementURL
			// 
			this.labelStatementURL.Location = new System.Drawing.Point(160, 124);
			this.labelStatementURL.Name = "labelStatementURL";
			this.labelStatementURL.Size = new System.Drawing.Size(101, 16);
			this.labelStatementURL.TabIndex = 262;
			this.labelStatementURL.Text = "URL Override";
			this.labelStatementURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboPracticeAddr
			// 
			this.comboPracticeAddr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboPracticeAddr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPracticeAddr.FormattingEnabled = true;
			this.comboPracticeAddr.Location = new System.Drawing.Point(473, 24);
			this.comboPracticeAddr.Name = "comboPracticeAddr";
			this.comboPracticeAddr.Size = new System.Drawing.Size(150, 21);
			this.comboPracticeAddr.TabIndex = 260;
			// 
			// comboRemitAddr
			// 
			this.comboRemitAddr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboRemitAddr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboRemitAddr.Enabled = false;
			this.comboRemitAddr.FormattingEnabled = true;
			this.comboRemitAddr.Location = new System.Drawing.Point(473, 46);
			this.comboRemitAddr.Name = "comboRemitAddr";
			this.comboRemitAddr.Size = new System.Drawing.Size(150, 21);
			this.comboRemitAddr.TabIndex = 259;
			// 
			// labelPracticeAddr
			// 
			this.labelPracticeAddr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelPracticeAddr.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelPracticeAddr.Location = new System.Drawing.Point(366, 26);
			this.labelPracticeAddr.Name = "labelPracticeAddr";
			this.labelPracticeAddr.Size = new System.Drawing.Size(106, 16);
			this.labelPracticeAddr.TabIndex = 258;
			this.labelPracticeAddr.Text = "Practice Address";
			this.labelPracticeAddr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelRemitAddr
			// 
			this.labelRemitAddr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelRemitAddr.Location = new System.Drawing.Point(366, 48);
			this.labelRemitAddr.Name = "labelRemitAddr";
			this.labelRemitAddr.Size = new System.Drawing.Size(106, 16);
			this.labelRemitAddr.TabIndex = 257;
			this.labelRemitAddr.Text = "Remit Address";
			this.labelRemitAddr.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelClinic
			// 
			this.labelClinic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelClinic.Location = new System.Drawing.Point(366, 70);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(106, 16);
			this.labelClinic.TabIndex = 256;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelClinic.Visible = false;
			// 
			// comboClinic
			// 
			this.comboClinic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.FormattingEnabled = true;
			this.comboClinic.Location = new System.Drawing.Point(473, 68);
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(150, 21);
			this.comboClinic.TabIndex = 255;
			this.comboClinic.Visible = false;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.comboClinic_SelectionChangeCommitted);
			// 
			// listElectBilling
			// 
			this.listElectBilling.FormattingEnabled = true;
			this.listElectBilling.Items.AddRange(new object[] {
            "No electronic billing",
            "Dental X Change",
            "Output to file",
            "ClaimX / ExtraDent",
            "EDS"});
			this.listElectBilling.Location = new System.Drawing.Point(12, 19);
			this.listElectBilling.Name = "listElectBilling";
			this.listElectBilling.Size = new System.Drawing.Size(120, 69);
			this.listElectBilling.TabIndex = 0;
			this.listElectBilling.SelectedIndexChanged += new System.EventHandler(this.listElectBilling_SelectedIndexChanged);
			// 
			// textPassword
			// 
			this.textPassword.Location = new System.Drawing.Point(262, 104);
			this.textPassword.Name = "textPassword";
			this.textPassword.Size = new System.Drawing.Size(100, 20);
			this.textPassword.TabIndex = 5;
			// 
			// labelPassword
			// 
			this.labelPassword.Location = new System.Drawing.Point(160, 104);
			this.labelPassword.Name = "labelPassword";
			this.labelPassword.Size = new System.Drawing.Size(101, 16);
			this.labelPassword.TabIndex = 254;
			this.labelPassword.Text = "Password";
			this.labelPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textUserName
			// 
			this.textUserName.Location = new System.Drawing.Point(262, 84);
			this.textUserName.Name = "textUserName";
			this.textUserName.Size = new System.Drawing.Size(100, 20);
			this.textUserName.TabIndex = 4;
			// 
			// labelUserName
			// 
			this.labelUserName.Location = new System.Drawing.Point(163, 84);
			this.labelUserName.Name = "labelUserName";
			this.labelUserName.Size = new System.Drawing.Size(98, 16);
			this.labelUserName.TabIndex = 252;
			this.labelUserName.Text = "User Name";
			this.labelUserName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkAmEx);
			this.groupBox2.Controls.Add(this.checkD);
			this.groupBox2.Controls.Add(this.checkV);
			this.groupBox2.Controls.Add(this.checkMC);
			this.groupBox2.Location = new System.Drawing.Point(13, 90);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(144, 76);
			this.groupBox2.TabIndex = 251;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Credit Card Choices";
			// 
			// checkAmEx
			// 
			this.checkAmEx.Location = new System.Drawing.Point(9, 58);
			this.checkAmEx.Name = "checkAmEx";
			this.checkAmEx.Size = new System.Drawing.Size(132, 16);
			this.checkAmEx.TabIndex = 3;
			this.checkAmEx.Text = "American Express";
			this.checkAmEx.UseVisualStyleBackColor = true;
			// 
			// checkD
			// 
			this.checkD.Location = new System.Drawing.Point(9, 43);
			this.checkD.Name = "checkD";
			this.checkD.Size = new System.Drawing.Size(132, 16);
			this.checkD.TabIndex = 2;
			this.checkD.Text = "Discover";
			this.checkD.UseVisualStyleBackColor = true;
			// 
			// checkV
			// 
			this.checkV.Location = new System.Drawing.Point(9, 28);
			this.checkV.Name = "checkV";
			this.checkV.Size = new System.Drawing.Size(132, 16);
			this.checkV.TabIndex = 1;
			this.checkV.Text = "Visa";
			this.checkV.UseVisualStyleBackColor = true;
			// 
			// checkMC
			// 
			this.checkMC.Location = new System.Drawing.Point(9, 13);
			this.checkMC.Name = "checkMC";
			this.checkMC.Size = new System.Drawing.Size(132, 16);
			this.checkMC.TabIndex = 0;
			this.checkMC.Text = "Master Card";
			this.checkMC.UseVisualStyleBackColor = true;
			// 
			// textClientAcctNumber
			// 
			this.textClientAcctNumber.Location = new System.Drawing.Point(262, 64);
			this.textClientAcctNumber.Name = "textClientAcctNumber";
			this.textClientAcctNumber.Size = new System.Drawing.Size(100, 20);
			this.textClientAcctNumber.TabIndex = 3;
			// 
			// labelAcctNum
			// 
			this.labelAcctNum.Location = new System.Drawing.Point(160, 64);
			this.labelAcctNum.Name = "labelAcctNum";
			this.labelAcctNum.Size = new System.Drawing.Size(101, 16);
			this.labelAcctNum.TabIndex = 249;
			this.labelAcctNum.Text = "Account Number";
			this.labelAcctNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textVendorPMScode
			// 
			this.textVendorPMScode.Location = new System.Drawing.Point(262, 44);
			this.textVendorPMScode.Name = "textVendorPMScode";
			this.textVendorPMScode.Size = new System.Drawing.Size(100, 20);
			this.textVendorPMScode.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(160, 44);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(101, 16);
			this.label3.TabIndex = 247;
			this.label3.Text = "Vendor PMS Code";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textVendorId
			// 
			this.textVendorId.Location = new System.Drawing.Point(262, 24);
			this.textVendorId.Name = "textVendorId";
			this.textVendorId.Size = new System.Drawing.Size(100, 20);
			this.textVendorId.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(160, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(101, 16);
			this.label2.TabIndex = 245;
			this.label2.Text = "Vendor ID";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(435, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(65, 14);
			this.label1.TabIndex = 245;
			this.label1.Text = "Days";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textDays
			// 
			this.textDays.Location = new System.Drawing.Point(389, 8);
			this.textDays.MaxVal = 255;
			this.textDays.MinVal = 0;
			this.textDays.Name = "textDays";
			this.textDays.Size = new System.Drawing.Size(44, 20);
			this.textDays.TabIndex = 0;
			// 
			// checkIntermingled
			// 
			this.checkIntermingled.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIntermingled.Location = new System.Drawing.Point(25, 9);
			this.checkIntermingled.Name = "checkIntermingled";
			this.checkIntermingled.Size = new System.Drawing.Size(216, 20);
			this.checkIntermingled.TabIndex = 1;
			this.checkIntermingled.Text = "Intermingle family members";
			// 
			// labelStartDate
			// 
			this.labelStartDate.Location = new System.Drawing.Point(240, 10);
			this.labelStartDate.Name = "labelStartDate";
			this.labelStartDate.Size = new System.Drawing.Size(147, 14);
			this.labelStartDate.TabIndex = 221;
			this.labelStartDate.Text = "Start Date Last";
			this.labelStartDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textNote
			// 
			this.textNote.AcceptsTab = true;
			this.textNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textNote.BackColor = System.Drawing.SystemColors.Window;
			this.textNote.DetectLinksEnabled = false;
			this.textNote.DetectUrls = false;
			this.textNote.Location = new System.Drawing.Point(24, 78);
			this.textNote.Name = "textNote";
			this.textNote.QuickPasteType = OpenDentBusiness.QuickPasteType.Statement;
			this.textNote.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textNote.Size = new System.Drawing.Size(611, 102);
			this.textNote.TabIndex = 2;
			this.textNote.Text = "";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(22, 45);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(575, 32);
			this.label4.TabIndex = 240;
			this.label4.Text = "General Message (in addition to any dunning messages and appointment messages).\r\n" +
    "You may use the variable [InstallmentPlanTerms] below to show the terms of the i" +
    "nstallment plan.";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(477, 834);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
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
			this.butCancel.Location = new System.Drawing.Point(558, 834);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 4;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// checkBoxBillShowTransSinceZero
			// 
			this.checkBoxBillShowTransSinceZero.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBoxBillShowTransSinceZero.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBoxBillShowTransSinceZero.Location = new System.Drawing.Point(312, 30);
			this.checkBoxBillShowTransSinceZero.Name = "checkBoxBillShowTransSinceZero";
			this.checkBoxBillShowTransSinceZero.Size = new System.Drawing.Size(274, 18);
			this.checkBoxBillShowTransSinceZero.TabIndex = 253;
			this.checkBoxBillShowTransSinceZero.Text = "Show all transactions since zero balance";
			// 
			// FormBillingDefaults
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(657, 864);
			this.Controls.Add(this.checkBoxBillShowTransSinceZero);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.checkSinglePatient);
			this.Controls.Add(this.textInvoiceNote);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBoxBilling);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textDays);
			this.Controls.Add(this.checkIntermingled);
			this.Controls.Add(this.labelStartDate);
			this.Controls.Add(this.textNote);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(673, 735);
			this.Name = "FormBillingDefaults";
			this.Text = "Billing Defaults";
			this.Load += new System.EventHandler(this.FormBillingDefaults_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBoxBilling.ResumeLayout(false);
			this.groupBoxBilling.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private System.Windows.Forms.CheckBox checkIntermingled;
		private System.Windows.Forms.Label labelStartDate;
		private ODtextBox textNote;
		private System.Windows.Forms.Label label4;
		private ValidNum textDays;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBoxBilling;
		private System.Windows.Forms.TextBox textPassword;
		private System.Windows.Forms.Label labelPassword;
		private System.Windows.Forms.TextBox textBillingEmailSubject;
		private System.Windows.Forms.TextBox textUserName;
		private System.Windows.Forms.Label labelUserName;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox checkAmEx;
		private System.Windows.Forms.CheckBox checkD;
		private System.Windows.Forms.CheckBox checkV;
		private System.Windows.Forms.CheckBox checkMC;
		private ODtextBox textBillingEmailBody;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textClientAcctNumber;
		private System.Windows.Forms.Label labelAcctNum;
		private System.Windows.Forms.TextBox textVendorPMScode;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textVendorId;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.ListBox listElectBilling;
		private System.Windows.Forms.Label label11;
		private ODtextBox textInvoiceNote;
		private System.Windows.Forms.Label labelClinic;
		private System.Windows.Forms.ComboBox comboClinic;
		private System.Windows.Forms.Label labelPracticeAddr;
		private System.Windows.Forms.Label labelRemitAddr;
		private System.Windows.Forms.ComboBox comboPracticeAddr;
		private System.Windows.Forms.ComboBox comboRemitAddr;
		private System.Windows.Forms.TextBox textStatementURL;
		private System.Windows.Forms.Label labelStatementURL;
		private System.Windows.Forms.Label labelBlankForDefault;
		private System.Windows.Forms.CheckBox checkCreatePDF;
		private System.Windows.Forms.CheckBox checkSinglePatient;
		private System.Windows.Forms.GroupBox groupBox1;
		private ODtextBox textSmsTemplate;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ListBox listModesToText;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox checkBoxBillShowTransSinceZero;
	}
}