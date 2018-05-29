using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;
using CodeBase;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormClearinghouseEdit : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label labelExportPath;
		private System.Windows.Forms.Label label6;
		private OpenDental.UI.Button butDelete;
		private System.Windows.Forms.TextBox textDescription;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textPayors;
		///<summary></summary>
		public bool IsNew;
		private System.Windows.Forms.TextBox textExportPath;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textISA08;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label labelReportPath;
		private System.Windows.Forms.Label labelPassword;
		private System.Windows.Forms.ComboBox comboFormat;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox textPassword;
		private System.Windows.Forms.Label labelClientProgram;
		private System.Windows.Forms.TextBox textResponsePath;
		private System.Windows.Forms.ComboBox comboCommBridge;
		private System.Windows.Forms.TextBox textClientProgram;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox textModemPort;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox textLoginID;
		private System.Windows.Forms.Label labelLoginID;
		private GroupBox groupBox1;
		private Label label9;
		private TextBox textISA05;
		private Label label16;
		private TextBox textSenderTIN;
		private Label label19;
		private TextBox textISA07;
		private Label label20;
		private Label label21;
		private Label label22;
		private TextBox textISA15;
		private Label label23;
		private GroupBox groupBox2;
		private Label labelSenderTelephone;
		private Label labelSenderName;
		private Label labelTIN;
		private TextBox textSenderTelephone;
		private TextBox textSenderName;
		private RadioButton radioSenderBelow;
		private RadioButton radioSenderOD;
		private Label label17;
		private Label label18;
		private TextBox textGS03;
		private Label label24;
		private Label label29;
		private Label label30;
		private Label label31;
		private TextBox textISA02;
		private Label label32;
		private Label label3;
		private TextBox textISA04;
		private Label label28;
		private GroupBox groupBox3;
		private Label label33;
		private TextBox textSeparatorData;
		private Label label34;
		private Label label35;
		private TextBox textISA16;
		private Label label36;
		private Label label37;
		private TextBox textSeparatorSegment;
		private Label label38;
		private Label labelInfo;
		private ComboBox comboClinic;
		private Clearinghouse ClearinghouseClin;
		///<summary>This must be set externally before opening the form.  The HQ version of the clearinghouse.  
		///This may not be null.  Assign a new clearinghouse object to this if creating a new clearinghouse.</summary>
		public Clearinghouse ClearinghouseHq;
		///<summary>This is never edited from within this form.  Set externally for reference to use in the Sync() method.</summary>		
		///This may not be null.  Assign a new clearinghouse object to this if creating a new clearinghouse.</summary>
		public Clearinghouse ClearinghouseHqOld;
		///<summary>This must be set externally before opening the form.  This is the clearinghouse used to display, with properly overridden fields.
		///This may not be null.  Assign a new clearinghouse object to this if creating a new clearinghouse.</summary>
		public Clearinghouse ClearinghouseCur;
		///<summary>Set this outside of the form.  0 for HQ.</summary>
		public long ClinicNum;
		/// <summary>List of all available clinics.  Cannot be null.</summary>
		public List<Clinic> ListClinics;
		///<summary>List of all clinic-level clearinghouses for the current clearinghousenum.  
		///May not be null.  Assign a new list of clearinghouse objects to this if creating a new clearinghouse.</summary>
		public List<Clearinghouse> ListClearinghousesClinCur;
		///<summary>This is never edited from within this form.  Set externally for reference to use in the Sync() method.
		///May not be null.  Assign a new list of clearinghouse objects to this if creating a new clearinghouse.</summary>
		public List<Clearinghouse> ListClearinghousesClinOld;
		private Label labelClinic;
		private CheckBox checkIsClaimExportAllowed;
		private ListBox listBoxEraBehavior;
		private int clinicSelectionLastIndex=-1;

		///<summary></summary>
		public FormClearinghouseEdit()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			Lan.C(this, new System.Windows.Forms.Control[]
			{
				this.textBox2
			});
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClearinghouseEdit));
			this.textDescription = new System.Windows.Forms.TextBox();
			this.labelExportPath = new System.Windows.Forms.Label();
			this.textExportPath = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textPayors = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textISA08 = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textResponsePath = new System.Windows.Forms.TextBox();
			this.labelReportPath = new System.Windows.Forms.Label();
			this.textPassword = new System.Windows.Forms.TextBox();
			this.labelPassword = new System.Windows.Forms.Label();
			this.comboFormat = new System.Windows.Forms.ComboBox();
			this.comboCommBridge = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.textClientProgram = new System.Windows.Forms.TextBox();
			this.labelClientProgram = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.textModemPort = new System.Windows.Forms.TextBox();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.textLoginID = new System.Windows.Forms.TextBox();
			this.labelLoginID = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label37 = new System.Windows.Forms.Label();
			this.textSeparatorSegment = new System.Windows.Forms.TextBox();
			this.label38 = new System.Windows.Forms.Label();
			this.label35 = new System.Windows.Forms.Label();
			this.textISA16 = new System.Windows.Forms.TextBox();
			this.label36 = new System.Windows.Forms.Label();
			this.label33 = new System.Windows.Forms.Label();
			this.textSeparatorData = new System.Windows.Forms.TextBox();
			this.label34 = new System.Windows.Forms.Label();
			this.label31 = new System.Windows.Forms.Label();
			this.textISA02 = new System.Windows.Forms.TextBox();
			this.label32 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textISA04 = new System.Windows.Forms.TextBox();
			this.label28 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.textGS03 = new System.Windows.Forms.TextBox();
			this.label24 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label30 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.textSenderTelephone = new System.Windows.Forms.TextBox();
			this.textSenderName = new System.Windows.Forms.TextBox();
			this.radioSenderBelow = new System.Windows.Forms.RadioButton();
			this.radioSenderOD = new System.Windows.Forms.RadioButton();
			this.labelSenderTelephone = new System.Windows.Forms.Label();
			this.labelSenderName = new System.Windows.Forms.Label();
			this.labelTIN = new System.Windows.Forms.Label();
			this.textSenderTIN = new System.Windows.Forms.TextBox();
			this.label22 = new System.Windows.Forms.Label();
			this.textISA15 = new System.Windows.Forms.TextBox();
			this.label23 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.textISA07 = new System.Windows.Forms.TextBox();
			this.label20 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.textISA05 = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.labelInfo = new System.Windows.Forms.Label();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.butDelete = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.labelClinic = new System.Windows.Forms.Label();
			this.checkIsClaimExportAllowed = new System.Windows.Forms.CheckBox();
			this.listBoxEraBehavior = new System.Windows.Forms.ListBox();
			this.groupBox1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// textDescription
			// 
			this.textDescription.Location = new System.Drawing.Point(251, 26);
			this.textDescription.MaxLength = 255;
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(226, 20);
			this.textDescription.TabIndex = 1;
			// 
			// labelExportPath
			// 
			this.labelExportPath.Location = new System.Drawing.Point(78, 416);
			this.labelExportPath.Name = "labelExportPath";
			this.labelExportPath.Size = new System.Drawing.Size(172, 17);
			this.labelExportPath.TabIndex = 0;
			this.labelExportPath.Text = "Claim Export Path";
			this.labelExportPath.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textExportPath
			// 
			this.textExportPath.Location = new System.Drawing.Point(251, 413);
			this.textExportPath.MaxLength = 255;
			this.textExportPath.Name = "textExportPath";
			this.textExportPath.Size = new System.Drawing.Size(317, 20);
			this.textExportPath.TabIndex = 5;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(35, 29);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(214, 17);
			this.label6.TabIndex = 0;
			this.label6.Text = "Description";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textPayors
			// 
			this.textPayors.Location = new System.Drawing.Point(251, 541);
			this.textPayors.MaxLength = 255;
			this.textPayors.Multiline = true;
			this.textPayors.Name = "textPayors";
			this.textPayors.Size = new System.Drawing.Size(377, 60);
			this.textPayors.TabIndex = 11;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(99, 544);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(151, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Payors";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textBox2
			// 
			this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox2.Location = new System.Drawing.Point(251, 604);
			this.textBox2.MaxLength = 255;
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.ReadOnly = true;
			this.textBox2.Size = new System.Drawing.Size(390, 44);
			this.textBox2.TabIndex = 0;
			this.textBox2.TabStop = false;
			this.textBox2.Text = "The list of payor IDs should be separated by commas with no spaces or other punct" +
    "uation.  For instance: 01234,23456 is valid.  You do not have to enter any payor" +
    " ID\'s for a default Clearinghouse.";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(99, 457);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(151, 17);
			this.label4.TabIndex = 0;
			this.label4.Text = "Format";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textISA08
			// 
			this.textISA08.Location = new System.Drawing.Point(242, 231);
			this.textISA08.MaxLength = 255;
			this.textISA08.Name = "textISA08";
			this.textISA08.Size = new System.Drawing.Size(96, 20);
			this.textISA08.TabIndex = 7;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(9, 232);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(232, 17);
			this.label5.TabIndex = 0;
			this.label5.Text = "Clearinghouse ID (ISA08)";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textResponsePath
			// 
			this.textResponsePath.Location = new System.Drawing.Point(251, 434);
			this.textResponsePath.MaxLength = 255;
			this.textResponsePath.Name = "textResponsePath";
			this.textResponsePath.Size = new System.Drawing.Size(317, 20);
			this.textResponsePath.TabIndex = 6;
			// 
			// labelReportPath
			// 
			this.labelReportPath.Location = new System.Drawing.Point(78, 437);
			this.labelReportPath.Name = "labelReportPath";
			this.labelReportPath.Size = new System.Drawing.Size(172, 17);
			this.labelReportPath.TabIndex = 0;
			this.labelReportPath.Text = "Report Path";
			this.labelReportPath.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textPassword
			// 
			this.textPassword.Location = new System.Drawing.Point(251, 392);
			this.textPassword.MaxLength = 255;
			this.textPassword.Name = "textPassword";
			this.textPassword.PasswordChar = '*';
			this.textPassword.Size = new System.Drawing.Size(96, 20);
			this.textPassword.TabIndex = 4;
			// 
			// labelPassword
			// 
			this.labelPassword.Location = new System.Drawing.Point(98, 395);
			this.labelPassword.Name = "labelPassword";
			this.labelPassword.Size = new System.Drawing.Size(151, 17);
			this.labelPassword.TabIndex = 0;
			this.labelPassword.Text = "Password";
			this.labelPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboFormat
			// 
			this.comboFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboFormat.Location = new System.Drawing.Point(251, 455);
			this.comboFormat.Name = "comboFormat";
			this.comboFormat.Size = new System.Drawing.Size(145, 21);
			this.comboFormat.TabIndex = 7;
			// 
			// comboCommBridge
			// 
			this.comboCommBridge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboCommBridge.Location = new System.Drawing.Point(251, 477);
			this.comboCommBridge.MaxDropDownItems = 20;
			this.comboCommBridge.Name = "comboCommBridge";
			this.comboCommBridge.Size = new System.Drawing.Size(145, 21);
			this.comboCommBridge.TabIndex = 8;
			this.comboCommBridge.SelectionChangeCommitted += new System.EventHandler(this.comboCommBridge_SelectionChangeCommitted);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(98, 480);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(151, 17);
			this.label7.TabIndex = 0;
			this.label7.Text = "Comm Bridge";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textClientProgram
			// 
			this.textClientProgram.Location = new System.Drawing.Point(251, 520);
			this.textClientProgram.MaxLength = 255;
			this.textClientProgram.Name = "textClientProgram";
			this.textClientProgram.Size = new System.Drawing.Size(317, 20);
			this.textClientProgram.TabIndex = 10;
			// 
			// labelClientProgram
			// 
			this.labelClientProgram.Location = new System.Drawing.Point(78, 523);
			this.labelClientProgram.Name = "labelClientProgram";
			this.labelClientProgram.Size = new System.Drawing.Size(172, 17);
			this.labelClientProgram.TabIndex = 0;
			this.labelClientProgram.Text = "Launch Client Program";
			this.labelClientProgram.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(248, 350);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(358, 18);
			this.label12.TabIndex = 0;
			this.label12.Text = "Not all values are required by each clearinghouse / carrier.";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textModemPort
			// 
			this.textModemPort.Location = new System.Drawing.Point(251, 499);
			this.textModemPort.MaxLength = 255;
			this.textModemPort.Name = "textModemPort";
			this.textModemPort.Size = new System.Drawing.Size(32, 20);
			this.textModemPort.TabIndex = 9;
			this.textModemPort.Visible = false;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(78, 502);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(172, 17);
			this.label13.TabIndex = 0;
			this.label13.Text = "Modem Port (1-4)";
			this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.label13.Visible = false;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(287, 503);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(267, 17);
			this.label14.TabIndex = 0;
			this.label14.Text = "(only if dialup)";
			this.label14.Visible = false;
			// 
			// textLoginID
			// 
			this.textLoginID.Location = new System.Drawing.Point(251, 371);
			this.textLoginID.MaxLength = 255;
			this.textLoginID.Name = "textLoginID";
			this.textLoginID.Size = new System.Drawing.Size(96, 20);
			this.textLoginID.TabIndex = 3;
			// 
			// labelLoginID
			// 
			this.labelLoginID.Location = new System.Drawing.Point(98, 374);
			this.labelLoginID.Name = "labelLoginID";
			this.labelLoginID.Size = new System.Drawing.Size(151, 17);
			this.labelLoginID.TabIndex = 0;
			this.labelLoginID.Text = "Login ID";
			this.labelLoginID.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.groupBox3);
			this.groupBox1.Controls.Add(this.label31);
			this.groupBox1.Controls.Add(this.textISA02);
			this.groupBox1.Controls.Add(this.label32);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.textISA04);
			this.groupBox1.Controls.Add(this.label28);
			this.groupBox1.Controls.Add(this.label18);
			this.groupBox1.Controls.Add(this.textGS03);
			this.groupBox1.Controls.Add(this.label24);
			this.groupBox1.Controls.Add(this.groupBox2);
			this.groupBox1.Controls.Add(this.label22);
			this.groupBox1.Controls.Add(this.textISA15);
			this.groupBox1.Controls.Add(this.label23);
			this.groupBox1.Controls.Add(this.label21);
			this.groupBox1.Controls.Add(this.label19);
			this.groupBox1.Controls.Add(this.textISA07);
			this.groupBox1.Controls.Add(this.label20);
			this.groupBox1.Controls.Add(this.label16);
			this.groupBox1.Controls.Add(this.textISA05);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.textISA08);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Location = new System.Drawing.Point(9, 50);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(924, 298);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "X12 Required Fields - Provided by Clearinghouse or Carrier";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label37);
			this.groupBox3.Controls.Add(this.textSeparatorSegment);
			this.groupBox3.Controls.Add(this.label38);
			this.groupBox3.Controls.Add(this.label35);
			this.groupBox3.Controls.Add(this.textISA16);
			this.groupBox3.Controls.Add(this.label36);
			this.groupBox3.Controls.Add(this.label33);
			this.groupBox3.Controls.Add(this.textSeparatorData);
			this.groupBox3.Controls.Add(this.label34);
			this.groupBox3.Location = new System.Drawing.Point(484, 83);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(435, 124);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Hexadecimal Delimiters (Always blank except for Denti-Cal)";
			// 
			// label37
			// 
			this.label37.Location = new System.Drawing.Point(318, 62);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(107, 15);
			this.label37.TabIndex = 0;
			this.label37.Text = "\"1C\" for Denti-Cal.";
			this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textSeparatorSegment
			// 
			this.textSeparatorSegment.Location = new System.Drawing.Point(221, 60);
			this.textSeparatorSegment.MaxLength = 255;
			this.textSeparatorSegment.Name = "textSeparatorSegment";
			this.textSeparatorSegment.Size = new System.Drawing.Size(96, 20);
			this.textSeparatorSegment.TabIndex = 3;
			// 
			// label38
			// 
			this.label38.Location = new System.Drawing.Point(6, 61);
			this.label38.Name = "label38";
			this.label38.Size = new System.Drawing.Size(209, 16);
			this.label38.TabIndex = 0;
			this.label38.Text = "Segment Terminator";
			this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label35
			// 
			this.label35.Location = new System.Drawing.Point(318, 40);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(107, 15);
			this.label35.TabIndex = 0;
			this.label35.Text = "\"22\" for Denti-Cal.";
			this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textISA16
			// 
			this.textISA16.Location = new System.Drawing.Point(221, 38);
			this.textISA16.MaxLength = 255;
			this.textISA16.Name = "textISA16";
			this.textISA16.Size = new System.Drawing.Size(96, 20);
			this.textISA16.TabIndex = 2;
			// 
			// label36
			// 
			this.label36.Location = new System.Drawing.Point(6, 39);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(209, 16);
			this.label36.TabIndex = 0;
			this.label36.Text = "Component Element Separator (ISA16)";
			this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label33
			// 
			this.label33.Location = new System.Drawing.Point(318, 18);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(107, 18);
			this.label33.TabIndex = 0;
			this.label33.Text = "\"1D\" for Denti-Cal.";
			this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textSeparatorData
			// 
			this.textSeparatorData.Location = new System.Drawing.Point(221, 16);
			this.textSeparatorData.MaxLength = 255;
			this.textSeparatorData.Name = "textSeparatorData";
			this.textSeparatorData.Size = new System.Drawing.Size(96, 20);
			this.textSeparatorData.TabIndex = 1;
			// 
			// label34
			// 
			this.label34.Location = new System.Drawing.Point(9, 17);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(206, 16);
			this.label34.TabIndex = 0;
			this.label34.Text = "Data Element Separator";
			this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label31
			// 
			this.label31.Location = new System.Drawing.Point(339, 17);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(231, 15);
			this.label31.TabIndex = 0;
			this.label31.Text = "Usually blank. \"DENTICAL\" for Denti-Cal.";
			this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textISA02
			// 
			this.textISA02.Location = new System.Drawing.Point(242, 15);
			this.textISA02.MaxLength = 255;
			this.textISA02.Name = "textISA02";
			this.textISA02.Size = new System.Drawing.Size(96, 20);
			this.textISA02.TabIndex = 1;
			// 
			// label32
			// 
			this.label32.Location = new System.Drawing.Point(6, 16);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(235, 17);
			this.label32.TabIndex = 0;
			this.label32.Text = "Authorization Information (ISA02)";
			this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(339, 39);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(231, 15);
			this.label3.TabIndex = 0;
			this.label3.Text = "Usually blank. \"NONE\" for Denti-Cal.";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textISA04
			// 
			this.textISA04.Location = new System.Drawing.Point(242, 37);
			this.textISA04.MaxLength = 255;
			this.textISA04.Name = "textISA04";
			this.textISA04.Size = new System.Drawing.Size(96, 20);
			this.textISA04.TabIndex = 2;
			// 
			// label28
			// 
			this.label28.Location = new System.Drawing.Point(6, 38);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(235, 17);
			this.label28.TabIndex = 0;
			this.label28.Text = "Security Information (ISA04)";
			this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(339, 254);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(231, 15);
			this.label18.TabIndex = 0;
			this.label18.Text = "Usually the same as ISA08.";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textGS03
			// 
			this.textGS03.Location = new System.Drawing.Point(242, 252);
			this.textGS03.MaxLength = 255;
			this.textGS03.Name = "textGS03";
			this.textGS03.Size = new System.Drawing.Size(96, 20);
			this.textGS03.TabIndex = 8;
			// 
			// label24
			// 
			this.label24.Location = new System.Drawing.Point(9, 253);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(232, 17);
			this.label24.TabIndex = 0;
			this.label24.Text = "GS03";
			this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label30);
			this.groupBox2.Controls.Add(this.label29);
			this.groupBox2.Controls.Add(this.textSenderTelephone);
			this.groupBox2.Controls.Add(this.textSenderName);
			this.groupBox2.Controls.Add(this.radioSenderBelow);
			this.groupBox2.Controls.Add(this.radioSenderOD);
			this.groupBox2.Controls.Add(this.labelSenderTelephone);
			this.groupBox2.Controls.Add(this.labelSenderName);
			this.groupBox2.Controls.Add(this.labelTIN);
			this.groupBox2.Controls.Add(this.textSenderTIN);
			this.groupBox2.Location = new System.Drawing.Point(12, 83);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(466, 124);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Sender ID - Used in ISA06, GS02, 1000A NM1, and 1000A PER";
			// 
			// label30
			// 
			this.label30.Location = new System.Drawing.Point(248, 17);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(208, 15);
			this.label30.TabIndex = 0;
			this.label30.Text = "(use this for Emdeon)";
			this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label29
			// 
			this.label29.Location = new System.Drawing.Point(248, 37);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(208, 17);
			this.label29.TabIndex = 0;
			this.label29.Text = "(much more common)";
			this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textSenderTelephone
			// 
			this.textSenderTelephone.Location = new System.Drawing.Point(230, 99);
			this.textSenderTelephone.MaxLength = 255;
			this.textSenderTelephone.Name = "textSenderTelephone";
			this.textSenderTelephone.Size = new System.Drawing.Size(145, 20);
			this.textSenderTelephone.TabIndex = 5;
			// 
			// textSenderName
			// 
			this.textSenderName.Location = new System.Drawing.Point(230, 78);
			this.textSenderName.MaxLength = 255;
			this.textSenderName.Name = "textSenderName";
			this.textSenderName.Size = new System.Drawing.Size(145, 20);
			this.textSenderName.TabIndex = 4;
			// 
			// radioSenderBelow
			// 
			this.radioSenderBelow.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioSenderBelow.Checked = true;
			this.radioSenderBelow.Location = new System.Drawing.Point(1, 36);
			this.radioSenderBelow.Name = "radioSenderBelow";
			this.radioSenderBelow.Size = new System.Drawing.Size(242, 18);
			this.radioSenderBelow.TabIndex = 2;
			this.radioSenderBelow.TabStop = true;
			this.radioSenderBelow.Text = "The information below identifies the sender";
			this.radioSenderBelow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioSenderBelow.UseVisualStyleBackColor = true;
			this.radioSenderBelow.Click += new System.EventHandler(this.radio_Click);
			// 
			// radioSenderOD
			// 
			this.radioSenderOD.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioSenderOD.Location = new System.Drawing.Point(22, 17);
			this.radioSenderOD.Name = "radioSenderOD";
			this.radioSenderOD.Size = new System.Drawing.Size(221, 18);
			this.radioSenderOD.TabIndex = 1;
			this.radioSenderOD.TabStop = true;
			this.radioSenderOD.Text = "This software is the \"sender\"";
			this.radioSenderOD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioSenderOD.UseVisualStyleBackColor = true;
			this.radioSenderOD.Click += new System.EventHandler(this.radio_Click);
			// 
			// labelSenderTelephone
			// 
			this.labelSenderTelephone.Location = new System.Drawing.Point(37, 100);
			this.labelSenderTelephone.Name = "labelSenderTelephone";
			this.labelSenderTelephone.Size = new System.Drawing.Size(191, 17);
			this.labelSenderTelephone.TabIndex = 0;
			this.labelSenderTelephone.Text = "Telephone Number";
			this.labelSenderTelephone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelSenderName
			// 
			this.labelSenderName.Location = new System.Drawing.Point(37, 79);
			this.labelSenderName.Name = "labelSenderName";
			this.labelSenderName.Size = new System.Drawing.Size(191, 17);
			this.labelSenderName.TabIndex = 0;
			this.labelSenderName.Text = "Name";
			this.labelSenderName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelTIN
			// 
			this.labelTIN.Location = new System.Drawing.Point(37, 58);
			this.labelTIN.Name = "labelTIN";
			this.labelTIN.Size = new System.Drawing.Size(191, 17);
			this.labelTIN.TabIndex = 0;
			this.labelTIN.Text = "Tax ID Number";
			this.labelTIN.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textSenderTIN
			// 
			this.textSenderTIN.Location = new System.Drawing.Point(230, 57);
			this.textSenderTIN.MaxLength = 255;
			this.textSenderTIN.Name = "textSenderTIN";
			this.textSenderTIN.Size = new System.Drawing.Size(145, 20);
			this.textSenderTIN.TabIndex = 3;
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(290, 275);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(280, 15);
			this.label22.TabIndex = 0;
			this.label22.Text = "\"P\" for Production,  \"T\" for Test.";
			this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textISA15
			// 
			this.textISA15.Location = new System.Drawing.Point(242, 273);
			this.textISA15.MaxLength = 255;
			this.textISA15.Name = "textISA15";
			this.textISA15.Size = new System.Drawing.Size(42, 20);
			this.textISA15.TabIndex = 9;
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(9, 274);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(232, 17);
			this.label23.TabIndex = 0;
			this.label23.Text = "Test or Production (ISA15)";
			this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(339, 233);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(231, 15);
			this.label21.TabIndex = 0;
			this.label21.Text = "Also used in 1000B NM109. ";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(339, 212);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(231, 15);
			this.label19.TabIndex = 0;
			this.label19.Text = "Usually \"ZZ\", sometimes \"30\".";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textISA07
			// 
			this.textISA07.Location = new System.Drawing.Point(242, 210);
			this.textISA07.MaxLength = 255;
			this.textISA07.Name = "textISA07";
			this.textISA07.Size = new System.Drawing.Size(96, 20);
			this.textISA07.TabIndex = 6;
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(6, 211);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(235, 17);
			this.label20.TabIndex = 0;
			this.label20.Text = "Receiver ID Qualifier (ISA07)";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(339, 61);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(231, 15);
			this.label16.TabIndex = 0;
			this.label16.Text = "Usually \"ZZ\", sometimes \"30\".";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textISA05
			// 
			this.textISA05.Location = new System.Drawing.Point(242, 59);
			this.textISA05.MaxLength = 255;
			this.textISA05.Name = "textISA05";
			this.textISA05.Size = new System.Drawing.Size(96, 20);
			this.textISA05.TabIndex = 3;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(6, 60);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(235, 17);
			this.label9.TabIndex = 0;
			this.label9.Text = "Sender ID Qualifier (ISA05)";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(479, 28);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(243, 15);
			this.label17.TabIndex = 0;
			this.label17.Text = "Also used in X12 1000B NM103";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelInfo
			// 
			this.labelInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelInfo.Location = new System.Drawing.Point(248, 6);
			this.labelInfo.Name = "labelInfo";
			this.labelInfo.Size = new System.Drawing.Size(698, 17);
			this.labelInfo.TabIndex = 0;
			this.labelInfo.Text = "Bolded fields are unique for each clinic.  Other fields are not editable unless U" +
    "nassigned/Default is selected.";
			this.labelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelInfo.Visible = false;
			// 
			// comboClinic
			// 
			this.comboClinic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(768, 34);
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(165, 21);
			this.comboClinic.TabIndex = 15;
			this.comboClinic.Visible = false;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.comboClinic_SelectionChangeCommitted);
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
			this.butDelete.Location = new System.Drawing.Point(9, 658);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(90, 26);
			this.butDelete.TabIndex = 14;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(784, 658);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(78, 26);
			this.butOK.TabIndex = 12;
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
			this.butCancel.Location = new System.Drawing.Point(868, 658);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(78, 26);
			this.butCancel.TabIndex = 13;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// labelClinic
			// 
			this.labelClinic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelClinic.Location = new System.Drawing.Point(697, 35);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(70, 18);
			this.labelClinic.TabIndex = 16;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelClinic.Visible = false;
			// 
			// checkIsClaimExportAllowed
			// 
			this.checkIsClaimExportAllowed.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsClaimExportAllowed.Checked = true;
			this.checkIsClaimExportAllowed.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkIsClaimExportAllowed.Enabled = false;
			this.checkIsClaimExportAllowed.Location = new System.Drawing.Point(583, 411);
			this.checkIsClaimExportAllowed.Name = "checkIsClaimExportAllowed";
			this.checkIsClaimExportAllowed.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkIsClaimExportAllowed.Size = new System.Drawing.Size(157, 20);
			this.checkIsClaimExportAllowed.TabIndex = 18;
			this.checkIsClaimExportAllowed.Text = "Use Claim Export Path";
			this.checkIsClaimExportAllowed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsClaimExportAllowed.UseVisualStyleBackColor = true;
			// 
			// listBoxEraBehavior
			// 
			this.listBoxEraBehavior.FormattingEnabled = true;
			this.listBoxEraBehavior.Location = new System.Drawing.Point(583, 434);
			this.listBoxEraBehavior.Name = "listBoxEraBehavior";
			this.listBoxEraBehavior.Size = new System.Drawing.Size(264, 43);
			this.listBoxEraBehavior.TabIndex = 20;
			this.listBoxEraBehavior.Enabled = false;
			// 
			// FormClearinghouseEdit
			// 
			this.ClientSize = new System.Drawing.Size(958, 696);
			this.Controls.Add(this.listBoxEraBehavior);
			this.Controls.Add(this.checkIsClaimExportAllowed);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelInfo);
			this.Controls.Add(this.label17);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.textLoginID);
			this.Controls.Add(this.textModemPort);
			this.Controls.Add(this.textClientProgram);
			this.Controls.Add(this.textPassword);
			this.Controls.Add(this.textResponsePath);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.textPayors);
			this.Controls.Add(this.textExportPath);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.labelLoginID);
			this.Controls.Add(this.comboCommBridge);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.labelClientProgram);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.comboFormat);
			this.Controls.Add(this.labelPassword);
			this.Controls.Add(this.labelReportPath);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.labelExportPath);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormClearinghouseEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Clearinghouse or Direct Carrier";
			this.Load += new System.EventHandler(this.FormClearinghouseEdit_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormClearinghouseEdit_Load(object sender,System.EventArgs e) {
			for(int i=0;i<Enum.GetNames(typeof(ElectronicClaimFormat)).Length;i++) {
				comboFormat.Items.Add(Lan.g("enumElectronicClaimFormat",Enum.GetNames(typeof(ElectronicClaimFormat))[i]));
			}
			for(int i=0;i<Enum.GetNames(typeof(EclaimsCommBridge)).Length;i++) {
				string translatedCommBridgeName=Lan.g("enumEclaimsCommBridge",Enum.GetNames(typeof(EclaimsCommBridge))[i]);
				comboCommBridge.Items.Add(new ODBoxItem<EclaimsCommBridge>(translatedCommBridgeName,(EclaimsCommBridge)i));
			}
			if(PrefC.HasClinicsEnabled) {
				FillClinics();
			}
			FillFields();
		}

		private void FillFields() {
			ClearinghouseClin=null;//Default to null so that if no clinic is found below we will use the defualt HQ options.
			for(int i=0;i<ListClearinghousesClinCur.Count;i++) {
				if(ListClearinghousesClinCur[i].ClinicNum==ClinicNum)	{
					ClearinghouseClin=ListClearinghousesClinCur[i];
				}
			}
			ClearinghouseCur=Clearinghouses.OverrideFields(ClearinghouseHq,ClearinghouseClin);
			textDescription.Text=ClearinghouseCur.Description;
			textISA02.Text=ClearinghouseCur.ISA02;
			textISA04.Text=ClearinghouseCur.ISA04;
			textISA05.Text=ClearinghouseCur.ISA05;
			if(ClearinghouseCur.SenderTIN==""){
				radioSenderOD.Checked=true;
				radioSenderBelow.Checked=false;
				textSenderTIN.Text="";
				textSenderName.Text="";
				textSenderTelephone.Text="";
			}
			else{
				radioSenderOD.Checked=false;
				radioSenderBelow.Checked=true;
				textSenderTIN.Text=ClearinghouseCur.SenderTIN;
				textSenderName.Text=ClearinghouseCur.SenderName;
				textSenderTelephone.Text=ClearinghouseCur.SenderTelephone;
			}
			textISA07.Text=ClearinghouseCur.ISA07;
			textISA08.Text=ClearinghouseCur.ISA08;
			textISA15.Text=ClearinghouseCur.ISA15;
			textGS03.Text=ClearinghouseCur.GS03;
			textSeparatorData.Text=ClearinghouseCur.SeparatorData;
			textISA16.Text=ClearinghouseCur.ISA16;
			textSeparatorSegment.Text=ClearinghouseCur.SeparatorSegment;
			textLoginID.Text=ClearinghouseCur.LoginID;
			textPassword.Text=ClearinghouseCur.Password;
			textExportPath.Text=ClearinghouseCur.ExportPath;
			textResponsePath.Text=ClearinghouseCur.ResponsePath;
			ODException.SwallowAnyException(() => { comboFormat.SelectedIndex=(int)ClearinghouseCur.Eformat; });
			ODException.SwallowAnyException(() => { comboCommBridge.SelectedIndex=(int)ClearinghouseCur.CommBridge; });
			textModemPort.Text=ClearinghouseCur.ModemPort.ToString();
			textClientProgram.Text=ClearinghouseCur.ClientProgram;
			//checkIsDefault.Checked=ClearinghouseCur.IsDefault;
			textPayors.Text=ClearinghouseCur.Payors;
			if(PrefC.HasClinicsEnabled) {
				labelInfo.Visible=true;
				labelTIN.Font=new System.Drawing.Font(labelTIN.Font,FontStyle.Bold);
				labelSenderName.Font=new System.Drawing.Font(labelSenderName.Font,FontStyle.Bold);
				labelSenderTelephone.Font=new System.Drawing.Font(labelSenderTelephone.Font,FontStyle.Bold);
				labelLoginID.Font=new System.Drawing.Font(labelLoginID.Font,FontStyle.Bold);
				labelPassword.Font=new System.Drawing.Font(labelPassword.Font,FontStyle.Bold);
				labelExportPath.Font=new System.Drawing.Font(labelExportPath.Font,FontStyle.Bold);
				labelReportPath.Font=new System.Drawing.Font(labelReportPath.Font,FontStyle.Bold);
				labelClientProgram.Font=new System.Drawing.Font(labelClientProgram.Font,FontStyle.Bold);
				labelClinic.Visible=true;
				comboClinic.Visible=true;
			}
			if(ClinicNum!=0) {
				textDescription.ReadOnly=true;
				textISA02.ReadOnly=true;
				textISA04.ReadOnly=true;
				textISA05.ReadOnly=true;
				textSeparatorData.ReadOnly=true;
				textISA16.ReadOnly=true; ;
				textSeparatorSegment.ReadOnly=true;
				textISA07.ReadOnly=true;
				textISA08.ReadOnly=true;
				textGS03.ReadOnly=true;
				textISA15.ReadOnly=true;
				comboFormat.Enabled=false;
				comboCommBridge.Enabled=false;
				textModemPort.ReadOnly=true;
				textPayors.ReadOnly=true;
				butDelete.Enabled=false;
			}
			else {
				textDescription.ReadOnly=false;
				textISA02.ReadOnly=false;
				textISA04.ReadOnly=false;
				textISA05.ReadOnly=false;
				textSeparatorData.ReadOnly=false;
				textISA16.ReadOnly=false;
				textSeparatorSegment.ReadOnly=false;
				textISA07.ReadOnly=false;
				textISA08.ReadOnly=false;
				textGS03.ReadOnly=false;
				textISA15.ReadOnly=false;
				comboFormat.Enabled=true;
				comboCommBridge.Enabled=true;
				textModemPort.ReadOnly=false;
				textPayors.ReadOnly=false;
				butDelete.Enabled=true;
			}
			FillListBoxEraBehavior();
			if(ClearinghouseCur.CommBridge.In(EclaimsCommBridge.ClaimConnect,EclaimsCommBridge.EDS,EclaimsCommBridge.Claimstream,EclaimsCommBridge.ITRANS)) {
				listBoxEraBehavior.Enabled=true;
				checkIsClaimExportAllowed.Enabled=true;
				checkIsClaimExportAllowed.Checked=ClearinghouseCur.IsClaimExportAllowed;
			}		
		}

		private void FillClinics() {
			for(int i=0;i<ListClinics.Count;i++) {
				comboClinic.Items.Add(ListClinics[i].Abbr);
				if(ListClinics[i].ClinicNum==ClinicNum) {
					comboClinic.SelectedIndex=i;
				}
			}
			if(comboClinic.SelectedIndex==-1) {//This should not happen, but just in case.
				comboClinic.SelectedIndex=0;
			}
			clinicSelectionLastIndex=comboClinic.SelectedIndex;
		}

		private void FillListBoxEraBehavior() {
			if(!comboCommBridge.SelectedTag<EclaimsCommBridge>().In(EclaimsCommBridge.Claimstream,EclaimsCommBridge.ITRANS)) {//If US
				listBoxEraBehavior.SetBounds(0,0,0,30,BoundsSpecified.Height);
			}
			else {
				listBoxEraBehavior.SetBounds(0,0,0,43,BoundsSpecified.Height);
			}
			listBoxEraBehavior.Items.Clear();
			EraBehaviors[] arrayEraBehaviors=(EraBehaviors[])Enum.GetValues(typeof(EraBehaviors));
			for(int i=0;i<arrayEraBehaviors.Length;i++) {
				if(i==2 && !comboCommBridge.SelectedTag<EclaimsCommBridge>().In(EclaimsCommBridge.Claimstream,EclaimsCommBridge.ITRANS)) {//If US
					continue;
				}
				string description=arrayEraBehaviors[i].GetDescription();
				//use comboCommBridge selection to determine if ERA->EOB "translation" necessary
				if(comboCommBridge.SelectedTag<EclaimsCommBridge>().In(EclaimsCommBridge.Claimstream,EclaimsCommBridge.ITRANS)) {//If Canada
					description=Regex.Replace(description,"ERA","EOB");
				}
				else if(i==1) {
					description="Download ERAs";
				}
				description=Lan.g("enumEraBehavior",description); //make sure the ERA->EOB replace gets run through translation
				ODBoxItem<EraBehaviors> listBoxItem=new ODBoxItem<EraBehaviors>(description,arrayEraBehaviors[i]);
				listBoxEraBehavior.Items.Add(listBoxItem);
				if(arrayEraBehaviors[i]==ClearinghouseCur.IsEraDownloadAllowed) {
					listBoxEraBehavior.SelectedIndex=i;
				}
			}
			if(listBoxEraBehavior.SelectedItems.Count==0) {//shouldn't happen because Clearinghouse.EraBehavior has a default in DB
				if(!comboCommBridge.SelectedTag<EclaimsCommBridge>().In(EclaimsCommBridge.Claimstream,EclaimsCommBridge.ITRANS)) {//If US
					listBoxEraBehavior.SelectedIndex=(int)EraBehaviors.DownloadDoNotReceive;
				}
				else {//Canada
					listBoxEraBehavior.SelectedIndex=(int)EraBehaviors.DownloadAndReceive;//default
				}
			}
		}

		///<summary>Pass in a clearinghouse with an unconcealed password. Will do nothing if the password is blank.</summary>
		private void ConcealClearinghousePass(Clearinghouse clearinghouse) {
			string concealedPassword = "";
			if(string.IsNullOrEmpty(clearinghouse.Password)) {
				return;
			}
			if(CDT.Class1.ConcealClearinghouse(clearinghouse.Password,out concealedPassword)) {
				clearinghouse.Password=concealedPassword;
			}
		}

		///<summary>Pass in a clearinghouse with a concealed password. Will do nothing if the password is blank.</summary>
		private void RevealClearinghousePass(Clearinghouse clearinghouse) {
			string revealedClearinghousePass = Clearinghouses.GetRevealPassword(clearinghouse.Password);
			if(revealedClearinghousePass!="") {
				clearinghouse.Password=revealedClearinghousePass;
			}
		}

		///<summary>All cached clearinghouses' passwords are NOT hashed. 
		///Hashing in this form only happens when transferring data between the database and the program.</summary>
		private bool SaveToCache() {
			if(!ValidateFields()) {
				return false;
			}
			if(ClinicNum==0) {
				ClearinghouseHq.Description=textDescription.Text;
				ClearinghouseHq.ISA02=textISA02.Text;
				ClearinghouseHq.ISA04=textISA04.Text;
				ClearinghouseHq.ISA05=textISA05.Text;
				ClearinghouseHq.SenderTIN=textSenderTIN.Text;
				ClearinghouseHq.SenderName=textSenderName.Text;
				ClearinghouseHq.SenderTelephone=textSenderTelephone.Text;
				ClearinghouseHq.ISA07=textISA07.Text;
				ClearinghouseHq.ISA08=textISA08.Text;
				ClearinghouseHq.ISA15=textISA15.Text;
				ClearinghouseHq.GS03=textGS03.Text;
				ClearinghouseHq.SeparatorData=textSeparatorData.Text;
				ClearinghouseHq.ISA16=textISA16.Text;
				ClearinghouseHq.SeparatorSegment=textSeparatorSegment.Text;
				ClearinghouseHq.LoginID=textLoginID.Text;
				ClearinghouseHq.Password=textPassword.Text;
				ClearinghouseHq.ExportPath=textExportPath.Text;
				ClearinghouseHq.ResponsePath=textResponsePath.Text;
				ClearinghouseHq.Eformat=(ElectronicClaimFormat)(comboFormat.SelectedIndex);
				ClearinghouseHq.CommBridge=(EclaimsCommBridge)(comboCommBridge.SelectedIndex);
				ClearinghouseHq.ModemPort=PIn.Byte(textModemPort.Text);
				ClearinghouseHq.ClientProgram=textClientProgram.Text;
				//ClearinghouseHq.IsDefault=checkIsDefault.Checked;
				ClearinghouseHq.Payors=textPayors.Text;
				ClearinghouseHq.IsClaimExportAllowed=checkIsClaimExportAllowed.Checked;
				ClearinghouseHq.IsEraDownloadAllowed=((ODBoxItem<EraBehaviors>)listBoxEraBehavior.SelectedItem).Tag;
			}
			else { 
				if(ClearinghouseClin==null) {
					ClearinghouseClin=new Clearinghouse();
				}
				//Save Clin Values to ClearinghouseClin, then update ListClinicClearinghousesCur.
				ClearinghouseClin.HqClearinghouseNum=ClearinghouseHq.ClearinghouseNum;
				ClearinghouseClin.ClinicNum=ClinicNum;
				ClearinghouseClin.IsClaimExportAllowed=checkIsClaimExportAllowed.Checked;
				ClearinghouseClin.IsEraDownloadAllowed=((ODBoxItem<EraBehaviors>)listBoxEraBehavior.SelectedItem).Tag;
				if(textExportPath.Text==ClearinghouseHq.ExportPath) {
					ClearinghouseClin.ExportPath="";//The value is the same as the default.  Save blank so that default can be updated dynamically.
				}
				else {
					ClearinghouseClin.ExportPath=textExportPath.Text;
				}
				if(textSenderTIN.Text==ClearinghouseHq.SenderTIN) {
					ClearinghouseClin.SenderTIN="";//The value is the same as the default.  Save blank so that default can be updated dynamically.
				}
				else {
					ClearinghouseClin.SenderTIN=textSenderTIN.Text;
				}
				if(textPassword.Text==ClearinghouseHq.Password) {
					ClearinghouseClin.Password="";//The value is the same as the default.  Save blank so that default can be updated dynamically.
				}
				else {
					ClearinghouseClin.Password=textPassword.Text;
				}
				if(textResponsePath.Text==ClearinghouseHq.ResponsePath) {
					ClearinghouseClin.ResponsePath="";//The value is the same as the default.  Save blank so that default can be updated dynamically.
				}
				else {
					ClearinghouseClin.ResponsePath=textResponsePath.Text;
				}
				if(textClientProgram.Text==ClearinghouseHq.ClientProgram) {
					ClearinghouseClin.ClientProgram="";//The value is the same as the default.  Save blank so that default can be updated dynamically.
				}
				else {
					ClearinghouseClin.ClientProgram=textClientProgram.Text;
				}
				if(textLoginID.Text==ClearinghouseHq.LoginID) {
					ClearinghouseClin.LoginID="";//The value is the same as the default.  Save blank so that default can be updated dynamically.
				}
				else {
					ClearinghouseClin.LoginID=textLoginID.Text;
				}
				if(textSenderName.Text==ClearinghouseHq.SenderName) {
					ClearinghouseClin.SenderName="";//The value is the same as the default.  Save blank so that default can be updated dynamically.
				}
				else {
					ClearinghouseClin.SenderName=textSenderName.Text;
				}
				if(textSenderTelephone.Text==ClearinghouseHq.SenderTelephone) {
					ClearinghouseClin.SenderTelephone="";//The value is the same as the default.  Save blank so that default can be updated dynamically.
				}
				else {
					ClearinghouseClin.SenderTelephone=textSenderTelephone.Text;
				}
				int index=-1;
				for(int i=0;i<ListClearinghousesClinCur.Count;i++) {
					if(ListClearinghousesClinCur[i].ClinicNum==ClinicNum) {
						index=i;
						break;
					}
				}
				if(index==-1) {
					ListClearinghousesClinCur.Add(ClearinghouseClin);
				}
				else {
					ListClearinghousesClinCur[index]=ClearinghouseClin;
				}
			}
			if(ClearinghouseCur.CommBridge==EclaimsCommBridge.ClaimConnect && (ClearinghouseCur.LoginID!=textLoginID.Text || ClearinghouseCur.Password!=textPassword.Text)){
				Program progPayConnect=Programs.GetCur(ProgramName.PayConnect);
				int billingUseDentalExchangeIdx=PrefC.GetInt(PrefName.BillingUseElectronic);//idx of 1= DentalXChange.
				if(progPayConnect.Enabled || billingUseDentalExchangeIdx==1) {
					MsgBox.Show(this,"ClaimConnect, PayConnect, and Electronic Billing credentials are usually all changed at the same time when using DentalXChange.");
				}
			}
			return true;
		}

		private bool ValidateFields() {
			if(comboFormat.SelectedIndex==-1) {
				MsgBox.Show(this,"Invalid Format.");
				return false;
			}
			if(comboCommBridge.SelectedIndex==-1) {
				MsgBox.Show(this,"Invalid Comm Bridge.");
				return false;
			}
			if(ClinicNum==0) {//HQ
				if(textDescription.Text=="") {
					MsgBox.Show(this,"Description cannot be blank.");//HQ
					return false;
				}
				if(comboFormat.SelectedIndex==(int)ElectronicClaimFormat.x837D_4010 //HQ
				|| comboFormat.SelectedIndex==(int)ElectronicClaimFormat.x837D_5010_dental
				|| comboFormat.SelectedIndex==(int)ElectronicClaimFormat.x837_5010_med_inst) {
					if(textISA02.Text.Length>10) {
						MsgBox.Show(this,"ISA02 must be 10 characters or less.");
						return false;
					}
					if(textISA04.Text.Length>10) {
						MsgBox.Show(this,"ISA04 must be 10 characters or less.");
						return false;
					}
					if(textISA05.Text=="") {
						MsgBox.Show(this,"ISA05 is required.");
						return false;
					}
					if(textISA05.Text!="01" && textISA05.Text!="14" && textISA05.Text!="20" && textISA05.Text!="27" && textISA05.Text!="28"//HQ
					&& textISA05.Text!="29" && textISA05.Text!="30" && textISA05.Text!="33" && textISA05.Text!="ZZ") {
						MsgBox.Show(this,"ISA05 is not valid.");
						return false;
					}
					if(textISA07.Text=="") {//HQ
						MsgBox.Show(this,"ISA07 is required.");
						return false;
					}
					if(textISA07.Text!="01" && textISA07.Text!="14" && textISA07.Text!="20" && textISA07.Text!="27" && textISA07.Text!="28"//HQ
					&& textISA07.Text!="29" && textISA07.Text!="30" && textISA07.Text!="33" && textISA07.Text!="ZZ") {
						MsgBox.Show(this,"ISA07 not valid.");
						return false;
					}
					if(textISA08.Text.Length<2) {//HQ
						MsgBox.Show(this,"ISA08 not valid.");
						return false;
					}
					if(textISA15.Text!="T" && textISA15.Text!="P") {//HQ
						MsgBox.Show(this,"ISA15 not valid.");
						return false;
					}
					if(textGS03.Text.Length<2) {//HQ
						MsgBox.Show(this,"GS03 is required.");
						return false;
					}
					if(textSeparatorData.Text!="" && !Regex.IsMatch(textSeparatorData.Text,"^[0-9A-F]{2}$",RegexOptions.IgnoreCase)) {//HQ
						MsgBox.Show(this,"Data element separator must be a valid 2 digit hexadecimal number or blank.");
						return false;
					}
					if(textISA16.Text!="" && !Regex.IsMatch(textISA16.Text,"^[0-9A-F]{2}$",RegexOptions.IgnoreCase)) {//HQ
						MsgBox.Show(this,"Component element separator must be a valid 2 digit hexadecimal number or blank.");
						return false;
					}
					if(textSeparatorSegment.Text!="" && !Regex.IsMatch(textSeparatorSegment.Text,"^[0-9A-F]{2}$",RegexOptions.IgnoreCase)) {//HQ
						MsgBox.Show(this,"Segment terminator must be a valid 2 digit hexadecimal number or blank.");
						return false;
					}
					if(comboFormat.SelectedIndex==0) {//HQ
						if(!MsgBox.Show(this,true,"Format not selected. Claims will not send. Continue anyway?")) {
							return false;
						}
					}
				}
			}//end HQ
			if(textISA08.Text=="0135WCH00" && !radioSenderOD.Checked) {//Clinic
				MsgBox.Show(this,"When using Emdeon, this software must be the sender.");
				return false;
			}
			if(comboFormat.SelectedIndex==(int)ElectronicClaimFormat.x837D_4010
				|| comboFormat.SelectedIndex==(int)ElectronicClaimFormat.x837D_5010_dental
				|| comboFormat.SelectedIndex==(int)ElectronicClaimFormat.x837_5010_med_inst)
			{//Clinic
				if(radioSenderBelow.Checked) {
					if(textSenderTIN.Text.Length<2) {
						MsgBox.Show(this,"Sender TIN is required.");
						return false;
					}
					if(textSenderName.Text=="") {
						MsgBox.Show(this,"Sender Name is required.");
						return false;
					}
					if(!Regex.IsMatch(textSenderTelephone.Text,@"^\d{10}$")) {
						MsgBox.Show(this,"Sender telephone must be 10 digits with no punctuation.");
						return false;
					}
				}	
			}
			//todo: Check all parts of program to allow either trailing slash or not
			if(checkIsClaimExportAllowed.Checked && textExportPath.Text!="" && !Directory.Exists(textExportPath.Text)) {
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Export path does not exist. Attempt to create?")) {
					try{
						Directory.CreateDirectory(textExportPath.Text);
						MsgBox.Show(this,"Folder created.");
					}
					catch{
						if(!MsgBox.Show(this,true,"Not able to create folder. Continue anyway?")){
							return false;
						}
					}
				}
			}
			if(textResponsePath.Text!="" && !Directory.Exists(textResponsePath.Text)) {//Clinic
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Report path does not exist. Attempt to create?")) {
					try {
						Directory.CreateDirectory(textResponsePath.Text);
						MsgBox.Show(this,"Folder created.");
					}
					catch {
						if(!MsgBox.Show(this,true,"Not able to create folder. Continue anyway?")) {
							return false;
						}
					}
				}
			}
			/*if(comboFormat.SelectedIndex==(int)ElectronicClaimFormat.X12){
				if(textISA08.Text!="BCBSGA"
					&& textISA08.Text!="100000"//Medicaid of GA
					&& textISA08.Text!="0135WCH00"//WebMD
					&& textISA08.Text!="330989922"//WebClaim
					&& textISA08.Text!="RECS"
					&& textISA08.Text!="AOS"
					&& textISA08.Text!="PostnTrack"
					)
				{
					if(!MsgBox.Show(this,true,"Clearinghouse ID not recognized. Continue anyway?")){
						return;
					}
				}
			}*/
			return true;
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			if(clinicSelectionLastIndex==comboClinic.SelectedIndex) {
				return;//Selection did not change.
			}
			if(!SaveToCache()) {//Validation failed.
				comboClinic.SelectedIndex=clinicSelectionLastIndex;//Revert selection.
				return;
			}
			clinicSelectionLastIndex=comboClinic.SelectedIndex;
			ClinicNum=ListClinics[clinicSelectionLastIndex].ClinicNum;
			FillFields();
		}
		
		private void comboCommBridge_SelectionChangeCommitted(object sender,EventArgs e) {
			FillListBoxEraBehavior();//Update ERA/EOB list box, specifically for if we print ERA vs EOB.
			bool hasEclaimsEnabled=comboCommBridge.SelectedTag<EclaimsCommBridge>().In(
				EclaimsCommBridge.ClaimConnect,EclaimsCommBridge.EDS,EclaimsCommBridge.Claimstream,EclaimsCommBridge.ITRANS);
			listBoxEraBehavior.Enabled=hasEclaimsEnabled;
			checkIsClaimExportAllowed.Enabled=hasEclaimsEnabled;
		}

		private void radio_Click(object sender,EventArgs e) {
			if(radioSenderOD.Checked) {
				textSenderTIN.Text="";
				textSenderName.Text="";
				textSenderTelephone.Text="";
			}
			else {
				textSenderTIN.Text=ClearinghouseCur.SenderTIN;
				textSenderName.Text=ClearinghouseCur.SenderName;
				textSenderTelephone.Text=ClearinghouseCur.SenderTelephone;
			}
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This clearinghouse will be deleted for all clinics.  Continue?")){
				return;
			}
			Clearinghouses.Delete(ClearinghouseHq);
			if(PrefC.GetLong(PrefName.ClearinghouseDefaultDent)==ClearinghouseHq.ClearinghouseNum) {
				Prefs.UpdateLong(PrefName.ClearinghouseDefaultDent,0);
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			if(PrefC.GetLong(PrefName.ClearinghouseDefaultMed)==ClearinghouseHq.ClearinghouseNum) {
				Prefs.UpdateLong(PrefName.ClearinghouseDefaultMed,0);
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			ClearinghouseCur=null;
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(!SaveToCache()) {//Validation failed.
				return;//Block user from leaving.
			}
			//When saving, hash all passwords.
			ConcealClearinghousePass(ClearinghouseHq);
			ListClearinghousesClinCur.ForEach(x => {
				ConcealClearinghousePass(x);
			});
			ConcealClearinghousePass(ClearinghouseHqOld);
			ListClearinghousesClinOld.ForEach(x => {
				ConcealClearinghousePass(x);
			});
			if(IsNew) {
				long clearinghouseNumNew=Clearinghouses.Insert(ClearinghouseHq);
				for(int i=0;i<ListClearinghousesClinCur.Count;i++) {
					ListClearinghousesClinCur[i].HqClearinghouseNum=clearinghouseNumNew;
				}
			}
			else {
				Clearinghouses.Update(ClearinghouseHq,ClearinghouseHqOld);
			}
			Clearinghouses.Sync(ListClearinghousesClinCur,ListClearinghousesClinOld);
			//After saving, reveal all passwords.
			RevealClearinghousePass(ClearinghouseHq);
			foreach(Clearinghouse c in ListClearinghousesClinCur) {
				RevealClearinghousePass(c);
			}
			//Reveal the "olds" just in case someone uses them outside this form.
			RevealClearinghousePass(ClearinghouseHqOld);
			foreach(Clearinghouse c in ListClearinghousesClinOld) {
				RevealClearinghousePass(c);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}





















