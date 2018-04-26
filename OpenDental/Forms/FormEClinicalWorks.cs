using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary> </summary>
	public class FormEClinicalWorks:ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.CheckBox checkEnabled;
		private IContainer components;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textProgName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textProgDesc;// Required designer variable.
		/// <summary>This Program link is new.</summary>
		public bool IsNew;
		public Program ProgramCur;
		private List<ProgramProperty> PropertyList;
		//private static Thread thread;
		private TextBox textHL7FolderIn;
		private TextBox textHL7FolderOut;
		private Label labelHL7FolderOut;
		private GroupBox groupBox1;
		private Label label5;
		private Label labelDefaultUserGroup;
		private ComboBox comboDefaultUserGroup;
		private CheckBox checkShowImages;
		private RadioButton radioModeTight;
		private RadioButton radioModeStandalone;
		private CheckBox checkFeeSchedules;
		private localhost.Service1 service11;
		private Label labelHL7Warning;
		private RadioButton radioModeFull;
		private UI.Button butDiagnostic;
		private TextBox textECWServer;
		private Label label3;
		private TextBox textODServer;
		private Label label6;
		private TextBox textHL7ServiceName;
		private Label label4;
		private TextBox textHL7Server;
		private Label label7;
		private Label label8;
		private Label label9;
		private Label labelDefEnabledWarning;
		private Label label10;
		private TextBox textMedPanelURL;
		private Label label11;
		private CheckBox checkQuadAsToothNum;
		private CheckBox checkLBSessionId;
		private Label label12;
		private CheckBox checkHideButChartRx;
		private Label labelHL7FolderIn;
		private CheckBox checkProcRequireSignature;
		private CheckBox checkProcNotesNoIncomplete;
		private List<UserGroup> _listUserGroups;

		///<summary></summary>
		public FormEClinicalWorks() {
			components=new System.ComponentModel.Container();
			//
			// Required for Windows Form Designer support
			//
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEClinicalWorks));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.checkEnabled = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textProgName = new System.Windows.Forms.TextBox();
			this.textProgDesc = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textHL7FolderIn = new System.Windows.Forms.TextBox();
			this.labelHL7FolderIn = new System.Windows.Forms.Label();
			this.textHL7FolderOut = new System.Windows.Forms.TextBox();
			this.labelHL7FolderOut = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.labelDefaultUserGroup = new System.Windows.Forms.Label();
			this.comboDefaultUserGroup = new System.Windows.Forms.ComboBox();
			this.checkShowImages = new System.Windows.Forms.CheckBox();
			this.radioModeTight = new System.Windows.Forms.RadioButton();
			this.radioModeStandalone = new System.Windows.Forms.RadioButton();
			this.checkFeeSchedules = new System.Windows.Forms.CheckBox();
			this.labelHL7Warning = new System.Windows.Forms.Label();
			this.radioModeFull = new System.Windows.Forms.RadioButton();
			this.butDiagnostic = new OpenDental.UI.Button();
			this.textECWServer = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textODServer = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.textHL7ServiceName = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textHL7Server = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.service11 = new OpenDental.localhost.Service1();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.labelDefEnabledWarning = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.textMedPanelURL = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.checkQuadAsToothNum = new System.Windows.Forms.CheckBox();
			this.checkLBSessionId = new System.Windows.Forms.CheckBox();
			this.label12 = new System.Windows.Forms.Label();
			this.checkHideButChartRx = new System.Windows.Forms.CheckBox();
			this.checkProcRequireSignature = new System.Windows.Forms.CheckBox();
			this.checkProcNotesNoIncomplete = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
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
			this.butCancel.Location = new System.Drawing.Point(563, 592);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 0;
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
			this.butOK.Location = new System.Drawing.Point(482, 592);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// checkEnabled
			// 
			this.checkEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEnabled.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkEnabled.Location = new System.Drawing.Point(124, 60);
			this.checkEnabled.Name = "checkEnabled";
			this.checkEnabled.Size = new System.Drawing.Size(98, 18);
			this.checkEnabled.TabIndex = 41;
			this.checkEnabled.Text = "Enabled";
			this.checkEnabled.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkEnabled.Click += new System.EventHandler(this.checkEnabled_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(21, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(187, 18);
			this.label1.TabIndex = 44;
			this.label1.Text = "Internal Name";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textProgName
			// 
			this.textProgName.Location = new System.Drawing.Point(209, 9);
			this.textProgName.Name = "textProgName";
			this.textProgName.ReadOnly = true;
			this.textProgName.Size = new System.Drawing.Size(275, 20);
			this.textProgName.TabIndex = 45;
			// 
			// textProgDesc
			// 
			this.textProgDesc.Location = new System.Drawing.Point(209, 34);
			this.textProgDesc.Name = "textProgDesc";
			this.textProgDesc.Size = new System.Drawing.Size(275, 20);
			this.textProgDesc.TabIndex = 47;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(20, 35);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(187, 18);
			this.label2.TabIndex = 46;
			this.label2.Text = "Description";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textHL7FolderIn
			// 
			this.textHL7FolderIn.Location = new System.Drawing.Point(197, 43);
			this.textHL7FolderIn.Name = "textHL7FolderIn";
			this.textHL7FolderIn.Size = new System.Drawing.Size(275, 20);
			this.textHL7FolderIn.TabIndex = 49;
			// 
			// labelHL7FolderIn
			// 
			this.labelHL7FolderIn.Location = new System.Drawing.Point(9, 44);
			this.labelHL7FolderIn.Name = "labelHL7FolderIn";
			this.labelHL7FolderIn.Size = new System.Drawing.Size(186, 18);
			this.labelHL7FolderIn.TabIndex = 48;
			this.labelHL7FolderIn.Text = "In to eClinicalWorks";
			this.labelHL7FolderIn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textHL7FolderOut
			// 
			this.textHL7FolderOut.Location = new System.Drawing.Point(197, 69);
			this.textHL7FolderOut.Name = "textHL7FolderOut";
			this.textHL7FolderOut.Size = new System.Drawing.Size(275, 20);
			this.textHL7FolderOut.TabIndex = 51;
			// 
			// labelHL7FolderOut
			// 
			this.labelHL7FolderOut.Location = new System.Drawing.Point(9, 70);
			this.labelHL7FolderOut.Name = "labelHL7FolderOut";
			this.labelHL7FolderOut.Size = new System.Drawing.Size(186, 18);
			this.labelHL7FolderOut.TabIndex = 50;
			this.labelHL7FolderOut.Text = "Out from eClinicalWorks";
			this.labelHL7FolderOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.textHL7FolderOut);
			this.groupBox1.Controls.Add(this.textHL7FolderIn);
			this.groupBox1.Controls.Add(this.labelHL7FolderOut);
			this.groupBox1.Controls.Add(this.labelHL7FolderIn);
			this.groupBox1.Location = new System.Drawing.Point(12, 336);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(538, 101);
			this.groupBox1.TabIndex = 52;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "HL7 Synch Folders";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(6, 19);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(478, 18);
			this.label5.TabIndex = 45;
			this.label5.Text = "Folder locations must be valid on the computer where the HL7 process is running";
			// 
			// labelDefaultUserGroup
			// 
			this.labelDefaultUserGroup.Location = new System.Drawing.Point(21, 452);
			this.labelDefaultUserGroup.Name = "labelDefaultUserGroup";
			this.labelDefaultUserGroup.Size = new System.Drawing.Size(186, 18);
			this.labelDefaultUserGroup.TabIndex = 53;
			this.labelDefaultUserGroup.Text = "Default User Group for new users";
			this.labelDefaultUserGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboDefaultUserGroup
			// 
			this.comboDefaultUserGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDefaultUserGroup.FormattingEnabled = true;
			this.comboDefaultUserGroup.Location = new System.Drawing.Point(209, 452);
			this.comboDefaultUserGroup.Name = "comboDefaultUserGroup";
			this.comboDefaultUserGroup.Size = new System.Drawing.Size(215, 21);
			this.comboDefaultUserGroup.TabIndex = 54;
			// 
			// checkShowImages
			// 
			this.checkShowImages.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowImages.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowImages.Location = new System.Drawing.Point(23, 479);
			this.checkShowImages.Name = "checkShowImages";
			this.checkShowImages.Size = new System.Drawing.Size(199, 18);
			this.checkShowImages.TabIndex = 55;
			this.checkShowImages.Text = "Show Images Module";
			this.checkShowImages.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowImages.Click += new System.EventHandler(this.checkShowImages_Click);
			// 
			// radioModeTight
			// 
			this.radioModeTight.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioModeTight.Location = new System.Drawing.Point(40, 84);
			this.radioModeTight.Name = "radioModeTight";
			this.radioModeTight.Size = new System.Drawing.Size(182, 18);
			this.radioModeTight.TabIndex = 56;
			this.radioModeTight.TabStop = true;
			this.radioModeTight.Text = "Tight Integration";
			this.radioModeTight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioModeTight.UseVisualStyleBackColor = true;
			this.radioModeTight.Click += new System.EventHandler(this.radioModeTight_Click);
			// 
			// radioModeStandalone
			// 
			this.radioModeStandalone.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioModeStandalone.Location = new System.Drawing.Point(40, 104);
			this.radioModeStandalone.Name = "radioModeStandalone";
			this.radioModeStandalone.Size = new System.Drawing.Size(182, 18);
			this.radioModeStandalone.TabIndex = 57;
			this.radioModeStandalone.TabStop = true;
			this.radioModeStandalone.Text = "Standalone";
			this.radioModeStandalone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioModeStandalone.UseVisualStyleBackColor = true;
			this.radioModeStandalone.Click += new System.EventHandler(this.radioModeStandalone_Click);
			// 
			// checkFeeSchedules
			// 
			this.checkFeeSchedules.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkFeeSchedules.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkFeeSchedules.Location = new System.Drawing.Point(23, 500);
			this.checkFeeSchedules.Name = "checkFeeSchedules";
			this.checkFeeSchedules.Size = new System.Drawing.Size(199, 18);
			this.checkFeeSchedules.TabIndex = 58;
			this.checkFeeSchedules.Text = "Patient Fee Schedules Set Manually";
			this.checkFeeSchedules.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelHL7Warning
			// 
			this.labelHL7Warning.Location = new System.Drawing.Point(227, 499);
			this.labelHL7Warning.Name = "labelHL7Warning";
			this.labelHL7Warning.Size = new System.Drawing.Size(170, 18);
			this.labelHL7Warning.TabIndex = 59;
			this.labelHL7Warning.Text = "(instead of importing from HL7)";
			this.labelHL7Warning.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// radioModeFull
			// 
			this.radioModeFull.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioModeFull.Location = new System.Drawing.Point(40, 124);
			this.radioModeFull.Name = "radioModeFull";
			this.radioModeFull.Size = new System.Drawing.Size(182, 18);
			this.radioModeFull.TabIndex = 60;
			this.radioModeFull.TabStop = true;
			this.radioModeFull.Text = "Full";
			this.radioModeFull.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.radioModeFull.UseVisualStyleBackColor = true;
			this.radioModeFull.Click += new System.EventHandler(this.radioModeFull_Click);
			// 
			// butDiagnostic
			// 
			this.butDiagnostic.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDiagnostic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDiagnostic.Autosize = true;
			this.butDiagnostic.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDiagnostic.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDiagnostic.CornerRadius = 4F;
			this.butDiagnostic.Location = new System.Drawing.Point(299, 592);
			this.butDiagnostic.Name = "butDiagnostic";
			this.butDiagnostic.Size = new System.Drawing.Size(90, 24);
			this.butDiagnostic.TabIndex = 61;
			this.butDiagnostic.Text = "Diagnostic Tool";
			this.butDiagnostic.Click += new System.EventHandler(this.butDiagnostic_Click);
			// 
			// textECWServer
			// 
			this.textECWServer.Location = new System.Drawing.Point(209, 148);
			this.textECWServer.Name = "textECWServer";
			this.textECWServer.Size = new System.Drawing.Size(181, 20);
			this.textECWServer.TabIndex = 53;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(24, 148);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(179, 18);
			this.label3.TabIndex = 52;
			this.label3.Text = "eCW Database Server";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textODServer
			// 
			this.textODServer.BackColor = System.Drawing.SystemColors.Window;
			this.textODServer.Location = new System.Drawing.Point(209, 174);
			this.textODServer.Name = "textODServer";
			this.textODServer.ReadOnly = true;
			this.textODServer.Size = new System.Drawing.Size(181, 20);
			this.textODServer.TabIndex = 65;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(24, 174);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(179, 18);
			this.label6.TabIndex = 64;
			this.label6.Text = "OpenDental Database Server";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textHL7ServiceName
			// 
			this.textHL7ServiceName.Location = new System.Drawing.Point(209, 226);
			this.textHL7ServiceName.Name = "textHL7ServiceName";
			this.textHL7ServiceName.Size = new System.Drawing.Size(181, 20);
			this.textHL7ServiceName.TabIndex = 69;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(24, 226);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(179, 18);
			this.label4.TabIndex = 68;
			this.label4.Text = "HL7 Service Name";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textHL7Server
			// 
			this.textHL7Server.Location = new System.Drawing.Point(209, 200);
			this.textHL7Server.Name = "textHL7Server";
			this.textHL7Server.Size = new System.Drawing.Size(181, 20);
			this.textHL7Server.TabIndex = 67;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(24, 200);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(179, 18);
			this.label7.TabIndex = 66;
			this.label7.Text = "OpenDental HL7 Server";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// service11
			// 
			this.service11.Url = "http://localhost:3824/Service1.asmx";
			this.service11.UseDefaultCredentials = true;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(396, 196);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(200, 28);
			this.label8.TabIndex = 70;
			this.label8.Text = "The computer name (not IP) where the HL7 Service is running.";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(396, 226);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(200, 18);
			this.label9.TabIndex = 71;
			this.label9.Text = "Typically OpenDentalHL7.";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelDefEnabledWarning
			// 
			this.labelDefEnabledWarning.ForeColor = System.Drawing.Color.Red;
			this.labelDefEnabledWarning.Location = new System.Drawing.Point(40, 301);
			this.labelDefEnabledWarning.Name = "labelDefEnabledWarning";
			this.labelDefEnabledWarning.Size = new System.Drawing.Size(569, 32);
			this.labelDefEnabledWarning.TabIndex = 72;
			this.labelDefEnabledWarning.Text = resources.GetString("labelDefEnabledWarning.Text");
			this.labelDefEnabledWarning.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelDefEnabledWarning.Visible = false;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(502, 252);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(94, 18);
			this.label10.TabIndex = 75;
			this.label10.Text = "Typically blank.";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textMedPanelURL
			// 
			this.textMedPanelURL.Location = new System.Drawing.Point(209, 252);
			this.textMedPanelURL.Name = "textMedPanelURL";
			this.textMedPanelURL.Size = new System.Drawing.Size(287, 20);
			this.textMedPanelURL.TabIndex = 74;
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(24, 252);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(179, 18);
			this.label11.TabIndex = 73;
			this.label11.Text = "Medical Panel URL";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkQuadAsToothNum
			// 
			this.checkQuadAsToothNum.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkQuadAsToothNum.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkQuadAsToothNum.Location = new System.Drawing.Point(23, 519);
			this.checkQuadAsToothNum.Name = "checkQuadAsToothNum";
			this.checkQuadAsToothNum.Size = new System.Drawing.Size(199, 18);
			this.checkQuadAsToothNum.TabIndex = 76;
			this.checkQuadAsToothNum.Text = "Send Quadrant as Tooth Number";
			this.checkQuadAsToothNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkLBSessionId
			// 
			this.checkLBSessionId.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkLBSessionId.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkLBSessionId.Location = new System.Drawing.Point(23, 278);
			this.checkLBSessionId.Name = "checkLBSessionId";
			this.checkLBSessionId.Size = new System.Drawing.Size(199, 18);
			this.checkLBSessionId.TabIndex = 77;
			this.checkLBSessionId.Text = "Exclude LBSESSIONID";
			this.checkLBSessionId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(227, 278);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(256, 17);
			this.label12.TabIndex = 78;
			this.label12.Text = "Check this box if the medical panel is not working.";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// checkHideButChartRx
			// 
			this.checkHideButChartRx.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHideButChartRx.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkHideButChartRx.Location = new System.Drawing.Point(11, 538);
			this.checkHideButChartRx.Name = "checkHideButChartRx";
			this.checkHideButChartRx.Size = new System.Drawing.Size(211, 21);
			this.checkHideButChartRx.TabIndex = 79;
			this.checkHideButChartRx.Text = "Hide Chart Rx Buttons";
			this.checkHideButChartRx.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkHideButChartRx.UseVisualStyleBackColor = true;
			// 
			// checkProcRequireSignature
			// 
			this.checkProcRequireSignature.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcRequireSignature.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkProcRequireSignature.Location = new System.Drawing.Point(1, 558);
			this.checkProcRequireSignature.Name = "checkProcRequireSignature";
			this.checkProcRequireSignature.Size = new System.Drawing.Size(221, 21);
			this.checkProcRequireSignature.TabIndex = 80;
			this.checkProcRequireSignature.Text = "Require Signatures for Procedure Notes";
			this.checkProcRequireSignature.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcRequireSignature.UseVisualStyleBackColor = true;
			// 
			// checkProcNotesNoIncomplete
			// 
			this.checkProcNotesNoIncomplete.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcNotesNoIncomplete.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkProcNotesNoIncomplete.Location = new System.Drawing.Point(1, 578);
			this.checkProcNotesNoIncomplete.Name = "checkProcNotesNoIncomplete";
			this.checkProcNotesNoIncomplete.Size = new System.Drawing.Size(221, 21);
			this.checkProcNotesNoIncomplete.TabIndex = 81;
			this.checkProcNotesNoIncomplete.Text = "Don\'t Allow Incomplete Procedure Notes";
			this.checkProcNotesNoIncomplete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkProcNotesNoIncomplete.UseVisualStyleBackColor = true;
			// 
			// FormEClinicalWorks
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(650, 628);
			this.Controls.Add(this.checkProcNotesNoIncomplete);
			this.Controls.Add(this.checkProcRequireSignature);
			this.Controls.Add(this.checkHideButChartRx);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.checkLBSessionId);
			this.Controls.Add(this.checkQuadAsToothNum);
			this.Controls.Add(this.textMedPanelURL);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.labelDefEnabledWarning);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.textHL7ServiceName);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textHL7Server);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textODServer);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textECWServer);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.butDiagnostic);
			this.Controls.Add(this.radioModeFull);
			this.Controls.Add(this.labelHL7Warning);
			this.Controls.Add(this.checkFeeSchedules);
			this.Controls.Add(this.radioModeStandalone);
			this.Controls.Add(this.radioModeTight);
			this.Controls.Add(this.checkShowImages);
			this.Controls.Add(this.comboDefaultUserGroup);
			this.Controls.Add(this.labelDefaultUserGroup);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.textProgDesc);
			this.Controls.Add(this.textProgName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkEnabled);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label10);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormEClinicalWorks";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "eClinicalWorks Setup";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormProgramLinkEdit_Closing);
			this.Load += new System.EventHandler(this.FormEClinicalWorks_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormEClinicalWorks_Load(object sender, System.EventArgs e) {
			FillForm();
			if(HL7Defs.IsExistingHL7Enabled()) {
				//Instead of using these, we will use the ones that are part of the HL7Def
				//These will be filled with those values.
				textHL7Server.ReadOnly=true;
				textHL7ServiceName.ReadOnly=true;
				textHL7FolderIn.ReadOnly=true;
				textHL7FolderOut.ReadOnly=true;
				labelDefEnabledWarning.Visible=true;
				checkQuadAsToothNum.Enabled=false;
			}
		}

		private void FillForm(){
			ProgramProperties.RefreshCache();
			PropertyList=ProgramProperties.GetForProgram(ProgramCur.ProgramNum);
			textProgName.Text=ProgramCur.ProgName;
			textProgDesc.Text=ProgramCur.ProgDesc;
			checkEnabled.Checked=ProgramCur.Enabled;
			if(GetProp("HideChartRxButtons")=="1") {
				checkHideButChartRx.Checked=true;
			}
			else {
				checkHideButChartRx.Checked=false;
			}
			if(GetProp("ProcRequireSignature")=="1") {
				checkProcRequireSignature.Checked=true;
			}
			else {
				checkProcRequireSignature.Checked=false;
			}
			if(GetProp("ProcNotesNoIncomplete")=="1") {
				checkProcNotesNoIncomplete.Checked=true;
			}
			else {
				checkProcNotesNoIncomplete.Checked=false;
			}
			SetModeRadioButtons(GetProp("eClinicalWorksMode"));
			SetModeVisibilities();
			textECWServer.Text=GetProp("eCWServer");//this property will not exist if using Oracle, eCW will never use Oracle
			if(HL7Defs.IsExistingHL7Enabled()) {
				HL7Def def=HL7Defs.GetOneDeepEnabled();
				textHL7Server.Text=def.HL7Server;
				textHL7ServiceName.Text=def.HL7ServiceName;
				textHL7FolderIn.Text=def.OutgoingFolder;//because these are the opposite of the way they are in the HL7Def
				textHL7FolderOut.Text=def.IncomingFolder;
				checkQuadAsToothNum.Checked=def.IsQuadAsToothNum;
			}
			else {
				textHL7Server.Text=GetProp("HL7Server");//this property will not exist if using Oracle, eCW will never use Oracle
				textHL7ServiceName.Text=GetProp("HL7ServiceName");//this property will not exist if using Oracle, eCW will never use Oracle
				textHL7FolderIn.Text=PrefC.GetString(PrefName.HL7FolderIn);
				textHL7FolderOut.Text=PrefC.GetString(PrefName.HL7FolderOut);
				//if a def is enabled, the value associated with the def will override this setting
				checkQuadAsToothNum.Checked=GetProp("IsQuadAsToothNum")=="1";//this property will not exist if using Oracle, eCW will never use Oracle
			}
			textODServer.Text=MiscData.GetODServer();
			comboDefaultUserGroup.Items.Clear();
			_listUserGroups=UserGroups.GetList();
			for(int i=0;i<_listUserGroups.Count;i++) {
				comboDefaultUserGroup.Items.Add(_listUserGroups[i].Description);
				if(GetProp("DefaultUserGroup")==_listUserGroups[i].UserGroupNum.ToString()) {
					comboDefaultUserGroup.SelectedIndex=i;
				}
			}
			checkShowImages.Checked=GetProp("ShowImagesModule")=="1";
			checkFeeSchedules.Checked=GetProp("FeeSchedulesSetManually")=="1";
			textMedPanelURL.Text=GetProp("MedicalPanelUrl");//this property will not exist if using Oracle, eCW will never use Oracle
			checkLBSessionId.Checked=GetProp("IsLBSessionIdExcluded")=="1";
		}

		private string GetProp(string desc){
			for(int i=0;i<PropertyList.Count;i++){
				if(PropertyList[i].PropertyDesc==desc){
					return PropertyList[i].PropertyValue;
				}
			}
			throw new ApplicationException("Property not found: "+desc);
		}

		private void checkEnabled_Click(object sender,EventArgs e) {
			bool isHL7Enabled=HL7Defs.IsExistingHL7Enabled();
			if(isHL7Enabled && checkEnabled.Checked) {
				MsgBox.Show(this,"An HL7Def is enabled.  The enabled HL7 definition will control the HL7 messages not this program link.");
				textHL7Server.ReadOnly=true;
				textHL7ServiceName.ReadOnly=true;
				textHL7FolderIn.ReadOnly=true;
				textHL7FolderOut.ReadOnly=true;
				labelDefEnabledWarning.Visible=true;
				checkQuadAsToothNum.Enabled=false;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Warning!  Read the manual carefully before turning this bridge on or off.  Make sure you understand the difference between the bridging modes and how it will affect patient accounts.  Continue anyway?")) {
				checkEnabled.Checked=!checkEnabled.Checked;
				return;
			}
			MsgBox.Show(this,"You will need to restart Open Dental to see the effects.");
		}

		private void radioModeTight_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Warning!  Read the manual carefully before changing this option.  Make sure you understand the difference between the bridging modes and how it will affect patient accounts.  Continue anyway?")) {
				//set radio buttons according to what they already are in the db
				SetModeRadioButtons(GetProp("eClinicalWorksMode"));
				SetModeVisibilities();
				return;
			}
			SetModeVisibilities();
		}

		private void radioModeStandalone_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Warning!  Read the manual carefully before changing this option.  Make sure you understand the difference between the bridging modes and how it will affect patient accounts.  Continue anyway?")) {
				//set radio buttons according to what they already are in the db
				SetModeRadioButtons(GetProp("eClinicalWorksMode"));
				SetModeVisibilities();
				return;
			}
			SetModeVisibilities();
		}

		private void radioModeFull_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Warning!  Read the manual carefully before changing this option.  Make sure you understand the difference between the bridging modes and how it will affect patient accounts.  Continue anyway?")) {
				//set radio buttons according to what they already are in the db
				SetModeRadioButtons(GetProp("eClinicalWorksMode"));
				SetModeVisibilities();
				return;
			}
			SetModeVisibilities();
		}

		private void SetModeVisibilities() {
			if(radioModeTight.Checked || radioModeFull.Checked) {
				labelHL7FolderIn.Visible=true;
				textHL7FolderIn.Visible=true;
				labelDefaultUserGroup.Visible=true;
				comboDefaultUserGroup.Visible=true;
				checkShowImages.Visible=true;
				checkFeeSchedules.Visible=true;
				labelHL7Warning.Visible=true;
			}
			else if(radioModeStandalone.Checked) {
				labelHL7FolderIn.Visible=false;
				textHL7FolderIn.Visible=false;
				labelDefaultUserGroup.Visible=false;
				comboDefaultUserGroup.Visible=false;
				checkShowImages.Visible=false;
				checkFeeSchedules.Visible=false;
				labelHL7Warning.Visible=false;
			}
		}

		///<summary>Pass in the desired eCW mode.  0=Tight,1=Standalone,2=Full.  Defaults to Tight.</summary>
		private void SetModeRadioButtons(string eClinicalWorksMode) {
			switch(eClinicalWorksMode) {
				case "0":
					radioModeTight.Checked=true;
					break;
				case "1":
					radioModeStandalone.Checked=true;
					break;
				case "2":
					radioModeFull.Checked=true;
					break;
				default:
					radioModeTight.Checked=true;
					break;
			}
		}

		private void checkShowImages_Click(object sender,EventArgs e) {
			MsgBox.Show(this,"You will need to restart Open Dental to see the effects.");
		}

		private bool SaveToDb() {
			if((radioModeTight.Checked || radioModeFull.Checked) && comboDefaultUserGroup.SelectedIndex==-1) {
				MsgBox.Show(this,"Please select a default user group first.");
				return false;
			}
			if(checkEnabled.Checked) {
				if(textProgDesc.Text=="") {
					MsgBox.Show(this,"Description may not be blank.");
					return false;
				}
				if(!HL7Defs.IsExistingHL7Enabled()) {
					if((radioModeTight.Checked || radioModeFull.Checked) && textHL7FolderIn.Text=="") {
						MsgBox.Show(this,"HL7 in folder may not be blank.");
						return false;
					}
					if(textHL7FolderOut.Text=="") {
						MsgBox.Show(this,"HL7 out folder may not be blank.");
						return false;
					}
					if(textHL7Server.Text=="") {
						MsgBox.Show(this,"HL7 Server may not be blank.");
						return false;
					}
					if(textHL7ServiceName.Text=="") {
						MsgBox.Show(this,"HL7 Service Name may not be blank.");
						return false;
					}
				}
			}
			ProgramCur.ProgDesc=textProgDesc.Text;
			ProgramCur.Enabled=checkEnabled.Checked;
			Programs.Update(ProgramCur);
			Prefs.UpdateString(PrefName.HL7FolderOut,textHL7FolderOut.Text);
			ProgramProperties.SetProperty(ProgramCur.ProgramNum,"HL7Server",textHL7Server.Text);//this property will not exist if using Oracle, eCW will never use Oracle
			ProgramProperties.SetProperty(ProgramCur.ProgramNum,"HL7ServiceName",textHL7ServiceName.Text);//this property will not exist if using Oracle, eCW will never use Oracle
			ProgramProperties.SetProperty(ProgramCur.ProgramNum,"MedicalPanelUrl",textMedPanelURL.Text);//this property will not exist if using Oracle, eCW will never use Oracle
			if(checkLBSessionId.Checked) {
				ProgramProperties.SetProperty(ProgramCur.ProgramNum,"IsLBSessionIdExcluded","1");
			}
			else {
				ProgramProperties.SetProperty(ProgramCur.ProgramNum,"IsLBSessionIdExcluded","0");
			}
			if(checkQuadAsToothNum.Checked) {
				ProgramProperties.SetProperty(ProgramCur.ProgramNum,"IsQuadAsToothNum","1");//this property will not exist if using Oracle, eCW will never use Oracle
			}
			else {
				ProgramProperties.SetProperty(ProgramCur.ProgramNum,"IsQuadAsToothNum","0");//this property will not exist if using Oracle, eCW will never use Oracle
			}
			if(checkHideButChartRx.Checked) {
				ProgramProperties.SetProperty(ProgramCur.ProgramNum,"HideChartRxButtons","1");//this property will not exist if using Oracle, eCW will never use Oracle
			}
			else {
				ProgramProperties.SetProperty(ProgramCur.ProgramNum,"HideChartRxButtons","0");//this property will not exist if using Oracle, eCW will never use Oracle
			}
			if(checkProcRequireSignature.Checked) {
				ProgramProperties.SetProperty(ProgramCur.ProgramNum,"ProcRequireSignature","1");
			}
			else {
				ProgramProperties.SetProperty(ProgramCur.ProgramNum,"ProcRequireSignature","0");
			}
			if(checkProcNotesNoIncomplete.Checked) {
				ProgramProperties.SetProperty(ProgramCur.ProgramNum,"ProcNotesNoIncomplete","1");
			}
			else {
				ProgramProperties.SetProperty(ProgramCur.ProgramNum,"ProcNotesNoIncomplete","0");
			}
			if(radioModeTight.Checked || radioModeFull.Checked) {
				if(radioModeTight.Checked) {
					ProgramProperties.SetProperty(ProgramCur.ProgramNum,"eClinicalWorksMode","0");//Tight
				}
				else {
					ProgramProperties.SetProperty(ProgramCur.ProgramNum,"eClinicalWorksMode","2");//Full
				}
				ProgramProperties.SetProperty(ProgramCur.ProgramNum,"eCWServer",textECWServer.Text);//this property will not exist if using Oracle, eCW will never use Oracle
				Prefs.UpdateString(PrefName.HL7FolderIn,textHL7FolderIn.Text);
				ProgramProperties.SetProperty(ProgramCur.ProgramNum,"DefaultUserGroup",
					_listUserGroups[comboDefaultUserGroup.SelectedIndex].UserGroupNum.ToString());
				if(checkShowImages.Checked) {
					ProgramProperties.SetProperty(ProgramCur.ProgramNum,"ShowImagesModule","1");
				}
				else {
					ProgramProperties.SetProperty(ProgramCur.ProgramNum,"ShowImagesModule","0");
				}
				if(this.checkFeeSchedules.Checked) {
					ProgramProperties.SetProperty(ProgramCur.ProgramNum,"FeeSchedulesSetManually","1");
				}
				else {
					ProgramProperties.SetProperty(ProgramCur.ProgramNum,"FeeSchedulesSetManually","0");
				}
			}
			else if(radioModeStandalone.Checked) {
				ProgramProperties.SetProperty(ProgramCur.ProgramNum,"eClinicalWorksMode","1");
				Prefs.UpdateString(PrefName.HL7FolderIn,"");
				ProgramProperties.SetProperty(ProgramCur.ProgramNum,"DefaultUserGroup","0");
				ProgramProperties.SetProperty(ProgramCur.ProgramNum,"ShowImagesModule","1");
				ProgramProperties.SetProperty(ProgramCur.ProgramNum,"FeeSchedulesSetManually","0");
			}
			DataValid.SetInvalid(InvalidType.Programs,InvalidType.Prefs);
			return true;
		}

		private void butDiagnostic_Click(object sender,EventArgs e) {
			//no need to validate all the other fields on the page.
			ProgramProperties.SetProperty(ProgramCur.ProgramNum,"eCWServer",textECWServer.Text);//this property will not exist if using Oracle, eCW will never use Oracle
			DataValid.SetInvalid(InvalidType.Programs);
			FormEcwDiag FormECWD=new FormEcwDiag();
			FormECWD.ShowDialog();
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(!SaveToDb()){
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormProgramLinkEdit_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			
		}

		

	

	

		

		

	

		

		

		
		


	}
}





















