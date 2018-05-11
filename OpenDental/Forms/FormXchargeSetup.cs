using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;
using OpenDental.Bridges;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormXchargeSetup : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private LinkLabel linkLabel1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private CheckBox checkEnabled;
		private TextBox textPath;
		private Label labelPath;
		private Label labelPaymentType;
		private ComboBox comboPaymentType;
		private TextBox textPassword;
		private Label labelPassword;
		private TextBox textUsername;
		private TextBox textOverride;
		private Label labelOverride;
		private Label labelUsername;
		private GroupBox groupXWeb;
		private Label labelXwebDesc;
		private TextBox textAuthKey;
		private Label labelAuthKey;
		private TextBox textXWebID;
		private Label labelXWebID;
		private TextBox textTerminalID;
		private Label labelTerminalID;
		private CheckBox checkPrintReceipt;
		private CheckBox checkPromptSig;
		private Label labelClinicEnable;
		private ComboBox comboClinic;
		private Label labelClinic;
		private GroupBox groupPaySettings;
		private Program _progCur;
		///<summary>Local cache of all of the clinic nums the current user has permission to access at the time the form loads.  Filled at the same time
		///as comboClinic and is used to set programproperty.ClinicNum when saving.</summary>
		private List<long> _listUserClinicNums;
		///<summary>List of X-Charge prog props for all clinics.  Includes props with ClinicNum=0 for headquarters/props unassigned to a clinic.</summary>
		private List<ProgramProperty> _listProgProps;
		private CheckBox checkWebPayEnabled;
		private CheckBox checkForceDuplicate;

		///<summary>Used to revert the clinic drop down selected index if the user tries to change clinics and the payment type hasn't been set.</summary>
		private int _indexClinicRevert;
		private List<Def> _listPayTypeDefs;

		///<summary></summary>
		public FormXchargeSetup()
		{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormXchargeSetup));
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.checkEnabled = new System.Windows.Forms.CheckBox();
			this.textPath = new System.Windows.Forms.TextBox();
			this.labelPath = new System.Windows.Forms.Label();
			this.labelPaymentType = new System.Windows.Forms.Label();
			this.comboPaymentType = new System.Windows.Forms.ComboBox();
			this.textPassword = new System.Windows.Forms.TextBox();
			this.labelPassword = new System.Windows.Forms.Label();
			this.textUsername = new System.Windows.Forms.TextBox();
			this.labelUsername = new System.Windows.Forms.Label();
			this.textOverride = new System.Windows.Forms.TextBox();
			this.labelOverride = new System.Windows.Forms.Label();
			this.groupXWeb = new System.Windows.Forms.GroupBox();
			this.checkWebPayEnabled = new System.Windows.Forms.CheckBox();
			this.textTerminalID = new System.Windows.Forms.TextBox();
			this.labelTerminalID = new System.Windows.Forms.Label();
			this.labelXwebDesc = new System.Windows.Forms.Label();
			this.textAuthKey = new System.Windows.Forms.TextBox();
			this.labelAuthKey = new System.Windows.Forms.Label();
			this.textXWebID = new System.Windows.Forms.TextBox();
			this.labelXWebID = new System.Windows.Forms.Label();
			this.checkPrintReceipt = new System.Windows.Forms.CheckBox();
			this.checkPromptSig = new System.Windows.Forms.CheckBox();
			this.labelClinicEnable = new System.Windows.Forms.Label();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.groupPaySettings = new System.Windows.Forms.GroupBox();
			this.checkForceDuplicate = new System.Windows.Forms.CheckBox();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupXWeb.SuspendLayout();
			this.groupPaySettings.SuspendLayout();
			this.SuspendLayout();
			// 
			// linkLabel1
			// 
			this.linkLabel1.LinkArea = new System.Windows.Forms.LinkArea(27, 40);
			this.linkLabel1.Location = new System.Drawing.Point(24, 13);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(436, 16);
			this.linkLabel1.TabIndex = 1;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "The X-Charge website is at https://openedgepayments.com/opendental/";
			this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.linkLabel1.UseCompatibleTextRendering = true;
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// checkEnabled
			// 
			this.checkEnabled.Location = new System.Drawing.Point(187, 45);
			this.checkEnabled.Name = "checkEnabled";
			this.checkEnabled.Size = new System.Drawing.Size(273, 17);
			this.checkEnabled.TabIndex = 2;
			this.checkEnabled.Text = "Enabled (affects all clinics)";
			this.checkEnabled.UseVisualStyleBackColor = true;
			// 
			// textPath
			// 
			this.textPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textPath.Location = new System.Drawing.Point(187, 65);
			this.textPath.Name = "textPath";
			this.textPath.Size = new System.Drawing.Size(273, 20);
			this.textPath.TabIndex = 3;
			// 
			// labelPath
			// 
			this.labelPath.Location = new System.Drawing.Point(24, 67);
			this.labelPath.Name = "labelPath";
			this.labelPath.Size = new System.Drawing.Size(162, 16);
			this.labelPath.TabIndex = 0;
			this.labelPath.Text = "Program Path";
			this.labelPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelPaymentType
			// 
			this.labelPaymentType.Location = new System.Drawing.Point(12, 67);
			this.labelPaymentType.Name = "labelPaymentType";
			this.labelPaymentType.Size = new System.Drawing.Size(162, 16);
			this.labelPaymentType.TabIndex = 0;
			this.labelPaymentType.Text = "Payment Type";
			this.labelPaymentType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboPaymentType
			// 
			this.comboPaymentType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboPaymentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPaymentType.FormattingEnabled = true;
			this.comboPaymentType.Location = new System.Drawing.Point(175, 65);
			this.comboPaymentType.MaxDropDownItems = 25;
			this.comboPaymentType.Name = "comboPaymentType";
			this.comboPaymentType.Size = new System.Drawing.Size(192, 21);
			this.comboPaymentType.TabIndex = 3;
			// 
			// textPassword
			// 
			this.textPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textPassword.Location = new System.Drawing.Point(175, 42);
			this.textPassword.Name = "textPassword";
			this.textPassword.Size = new System.Drawing.Size(273, 20);
			this.textPassword.TabIndex = 2;
			this.textPassword.UseSystemPasswordChar = true;
			this.textPassword.TextChanged += new System.EventHandler(this.textPassword_TextChanged);
			// 
			// labelPassword
			// 
			this.labelPassword.Location = new System.Drawing.Point(12, 44);
			this.labelPassword.Name = "labelPassword";
			this.labelPassword.Size = new System.Drawing.Size(162, 16);
			this.labelPassword.TabIndex = 0;
			this.labelPassword.Text = "Password";
			this.labelPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textUsername
			// 
			this.textUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textUsername.Location = new System.Drawing.Point(175, 19);
			this.textUsername.Name = "textUsername";
			this.textUsername.Size = new System.Drawing.Size(273, 20);
			this.textUsername.TabIndex = 1;
			// 
			// labelUsername
			// 
			this.labelUsername.Location = new System.Drawing.Point(12, 21);
			this.labelUsername.Name = "labelUsername";
			this.labelUsername.Size = new System.Drawing.Size(162, 16);
			this.labelUsername.TabIndex = 0;
			this.labelUsername.Text = "Username";
			this.labelUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textOverride
			// 
			this.textOverride.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textOverride.Location = new System.Drawing.Point(187, 88);
			this.textOverride.Name = "textOverride";
			this.textOverride.Size = new System.Drawing.Size(273, 20);
			this.textOverride.TabIndex = 4;
			// 
			// labelOverride
			// 
			this.labelOverride.Location = new System.Drawing.Point(6, 90);
			this.labelOverride.Name = "labelOverride";
			this.labelOverride.Size = new System.Drawing.Size(180, 16);
			this.labelOverride.TabIndex = 0;
			this.labelOverride.Text = "Local Path Override (usually blank)";
			this.labelOverride.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupXWeb
			// 
			this.groupXWeb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupXWeb.Controls.Add(this.checkWebPayEnabled);
			this.groupXWeb.Controls.Add(this.textTerminalID);
			this.groupXWeb.Controls.Add(this.labelTerminalID);
			this.groupXWeb.Controls.Add(this.labelXwebDesc);
			this.groupXWeb.Controls.Add(this.textAuthKey);
			this.groupXWeb.Controls.Add(this.labelAuthKey);
			this.groupXWeb.Controls.Add(this.textXWebID);
			this.groupXWeb.Controls.Add(this.labelXWebID);
			this.groupXWeb.Location = new System.Drawing.Point(6, 149);
			this.groupXWeb.Name = "groupXWeb";
			this.groupXWeb.Size = new System.Drawing.Size(448, 145);
			this.groupXWeb.TabIndex = 6;
			this.groupXWeb.TabStop = false;
			this.groupXWeb.Text = "X-Web";
			// 
			// checkWebPayEnabled
			// 
			this.checkWebPayEnabled.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkWebPayEnabled.Location = new System.Drawing.Point(169, 122);
			this.checkWebPayEnabled.Name = "checkWebPayEnabled";
			this.checkWebPayEnabled.Size = new System.Drawing.Size(273, 17);
			this.checkWebPayEnabled.TabIndex = 7;
			this.checkWebPayEnabled.Text = "Enable X-Web for patient portal payments";
			// 
			// textTerminalID
			// 
			this.textTerminalID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textTerminalID.Location = new System.Drawing.Point(169, 99);
			this.textTerminalID.Name = "textTerminalID";
			this.textTerminalID.Size = new System.Drawing.Size(273, 20);
			this.textTerminalID.TabIndex = 3;
			// 
			// labelTerminalID
			// 
			this.labelTerminalID.Location = new System.Drawing.Point(6, 101);
			this.labelTerminalID.Name = "labelTerminalID";
			this.labelTerminalID.Size = new System.Drawing.Size(162, 16);
			this.labelTerminalID.TabIndex = 0;
			this.labelTerminalID.Text = "Terminal ID";
			this.labelTerminalID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelXwebDesc
			// 
			this.labelXwebDesc.Location = new System.Drawing.Point(6, 19);
			this.labelXwebDesc.Name = "labelXwebDesc";
			this.labelXwebDesc.Size = new System.Drawing.Size(436, 31);
			this.labelXwebDesc.TabIndex = 0;
			this.labelXwebDesc.Text = "The following settings are required to enable receiving online payments via the P" +
    "atient Portal.  These settings are provided by X-Charge when you sign up for X-W" +
    "eb.";
			this.labelXwebDesc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// textAuthKey
			// 
			this.textAuthKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textAuthKey.Location = new System.Drawing.Point(169, 76);
			this.textAuthKey.Name = "textAuthKey";
			this.textAuthKey.Size = new System.Drawing.Size(273, 20);
			this.textAuthKey.TabIndex = 2;
			this.textAuthKey.TextChanged += new System.EventHandler(this.textAuthKey_TextChanged);
			// 
			// labelAuthKey
			// 
			this.labelAuthKey.Location = new System.Drawing.Point(6, 78);
			this.labelAuthKey.Name = "labelAuthKey";
			this.labelAuthKey.Size = new System.Drawing.Size(162, 16);
			this.labelAuthKey.TabIndex = 0;
			this.labelAuthKey.Text = "Auth Key";
			this.labelAuthKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textXWebID
			// 
			this.textXWebID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textXWebID.Location = new System.Drawing.Point(169, 53);
			this.textXWebID.Name = "textXWebID";
			this.textXWebID.Size = new System.Drawing.Size(273, 20);
			this.textXWebID.TabIndex = 1;
			// 
			// labelXWebID
			// 
			this.labelXWebID.Location = new System.Drawing.Point(6, 55);
			this.labelXWebID.Name = "labelXWebID";
			this.labelXWebID.Size = new System.Drawing.Size(162, 16);
			this.labelXWebID.TabIndex = 0;
			this.labelXWebID.Text = "XWeb ID";
			this.labelXWebID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkPrintReceipt
			// 
			this.checkPrintReceipt.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkPrintReceipt.Location = new System.Drawing.Point(175, 109);
			this.checkPrintReceipt.Name = "checkPrintReceipt";
			this.checkPrintReceipt.Size = new System.Drawing.Size(273, 17);
			this.checkPrintReceipt.TabIndex = 5;
			this.checkPrintReceipt.Text = "Print receipts by default";
			// 
			// checkPromptSig
			// 
			this.checkPromptSig.Location = new System.Drawing.Point(175, 89);
			this.checkPromptSig.Name = "checkPromptSig";
			this.checkPromptSig.Size = new System.Drawing.Size(273, 17);
			this.checkPromptSig.TabIndex = 4;
			this.checkPromptSig.Text = "Prompt for signature on CC trans by default";
			// 
			// labelClinicEnable
			// 
			this.labelClinicEnable.Location = new System.Drawing.Point(24, 118);
			this.labelClinicEnable.Name = "labelClinicEnable";
			this.labelClinicEnable.Size = new System.Drawing.Size(436, 16);
			this.labelClinicEnable.TabIndex = 0;
			this.labelClinicEnable.Text = "To enable X-Charge for a clinic, set the User ID and Password for that clinic.";
			this.labelClinicEnable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// comboClinic
			// 
			this.comboClinic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(187, 144);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(192, 21);
			this.comboClinic.TabIndex = 5;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.comboClinic_SelectionChangeCommitted);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(24, 147);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(162, 16);
			this.labelClinic.TabIndex = 0;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupPaySettings
			// 
			this.groupPaySettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupPaySettings.Controls.Add(this.checkForceDuplicate);
			this.groupPaySettings.Controls.Add(this.checkPrintReceipt);
			this.groupPaySettings.Controls.Add(this.checkPromptSig);
			this.groupPaySettings.Controls.Add(this.groupXWeb);
			this.groupPaySettings.Controls.Add(this.textUsername);
			this.groupPaySettings.Controls.Add(this.textPassword);
			this.groupPaySettings.Controls.Add(this.comboPaymentType);
			this.groupPaySettings.Controls.Add(this.labelUsername);
			this.groupPaySettings.Controls.Add(this.labelPassword);
			this.groupPaySettings.Controls.Add(this.labelPaymentType);
			this.groupPaySettings.Location = new System.Drawing.Point(12, 171);
			this.groupPaySettings.Name = "groupPaySettings";
			this.groupPaySettings.Size = new System.Drawing.Size(460, 300);
			this.groupPaySettings.TabIndex = 6;
			this.groupPaySettings.TabStop = false;
			this.groupPaySettings.Text = "Clinic Payment Settings";
			// 
			// checkForceDuplicate
			// 
			this.checkForceDuplicate.Location = new System.Drawing.Point(175, 128);
			this.checkForceDuplicate.Name = "checkForceDuplicate";
			this.checkForceDuplicate.Size = new System.Drawing.Size(273, 17);
			this.checkForceDuplicate.TabIndex = 7;
			this.checkForceDuplicate.Text = "Recurring charge list force duplicates by default";
			this.checkForceDuplicate.UseVisualStyleBackColor = true;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(311, 478);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 7;
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
			this.butCancel.Location = new System.Drawing.Point(397, 478);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 8;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormXchargeSetup
			// 
			this.ClientSize = new System.Drawing.Size(484, 516);
			this.Controls.Add(this.groupPaySettings);
			this.Controls.Add(this.labelClinicEnable);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.textOverride);
			this.Controls.Add(this.labelOverride);
			this.Controls.Add(this.textPath);
			this.Controls.Add(this.labelPath);
			this.Controls.Add(this.checkEnabled);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(500, 540);
			this.Name = "FormXchargeSetup";
			this.ShowInTaskbar = false;
			this.Text = "X-Charge Setup";
			this.Load += new System.EventHandler(this.FormXchargeSetup_Load);
			this.groupXWeb.ResumeLayout(false);
			this.groupXWeb.PerformLayout();
			this.groupPaySettings.ResumeLayout(false);
			this.groupPaySettings.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormXchargeSetup_Load(object sender,EventArgs e) {
			_progCur=Programs.GetCur(ProgramName.Xcharge);
			if(_progCur==null) {
				return;//should never happen
			}
			if(PrefC.HasClinicsEnabled) {
				groupPaySettings.Text=Lan.g(this,"Clinic Payment Settings");
				_listUserClinicNums=new List<long>();
				comboClinic.Items.Clear();
				if(Security.CurUser.ClinicIsRestricted) {
					//if program link is enabled, disable the enable check box so the restricted user cannot disable for all clinics
					checkEnabled.Enabled=!_progCur.Enabled;
				}
				else {
					comboClinic.Items.Add(Lan.g(this,"Headquarters"));
					//this way both lists have the same number of items in it and if 'Headquarters' is selected the programproperty.ClinicNum will be set to 0
					_listUserClinicNums.Add(0);
					comboClinic.SelectedIndex=0;
				}
				List<Clinic> listClinics=Clinics.GetForUserod(Security.CurUser);
				for(int i=0;i<listClinics.Count;i++) {
					comboClinic.Items.Add(listClinics[i].Abbr);
					_listUserClinicNums.Add(listClinics[i].ClinicNum);
					if(Clinics.ClinicNum==listClinics[i].ClinicNum) {
						comboClinic.SelectedIndex=i;
						if(!Security.CurUser.ClinicIsRestricted) {
							comboClinic.SelectedIndex++;//increment SelectedIndex for 'Headquarters' in the list at position 0 if the user is not restricted
						}
					}
				}
				_indexClinicRevert=comboClinic.SelectedIndex;
			}
			else {//clinics not enabled
				checkEnabled.Text=Lan.g(this,"Enabled");
				labelClinicEnable.Visible=false;
				labelClinic.Visible=false;
				comboClinic.Visible=false;
				groupPaySettings.Text=Lan.g(this,"Payment Settings");
				_listUserClinicNums=new List<long>() { 0 };//if clinics are disabled, programproperty.ClinicNum will be set to 0
			}
			checkEnabled.Checked=_progCur.Enabled;
			textPath.Text=_progCur.Path;
			textOverride.Text=ProgramProperties.GetLocalPathOverrideForProgram(_progCur.ProgramNum);
			_listProgProps=ProgramProperties.GetForProgram(_progCur.ProgramNum);
			FillFields();
		}

		///<summary>Fills all but comboClinic, checkEnabled, textPath, and textOverride which are filled on load.</summary>
		private void FillFields() {
			long clinicNum=0;
			if(PrefC.HasClinicsEnabled && comboClinic.SelectedIndex>-1) {
				clinicNum=_listUserClinicNums[comboClinic.SelectedIndex];
			}
			//Password
			string password=ProgramProperties.GetPropValFromList(_listProgProps,"Password",clinicNum);
			if(password.Length>0) {
				password=CodeBase.MiscUtils.Decrypt(password);
				textPassword.UseSystemPasswordChar=true;
			}
			textPassword.Text=password;
			//AuthKey had previously been stored as obfuscated text (prior to 16.2). 
			//The XWeb feature was not publicly available for any of these versions so it safe to remove that restriction.
			//It was determined that storing in plain-text is good enough as the obfuscation wasn't really making the key any more secure.
			textAuthKey.Text=ProgramProperties.GetPropValFromList(_listProgProps,"AuthKey",clinicNum);
			//PaymentType ComboBox
			string payTypeDefNum=ProgramProperties.GetPropValFromList(_listProgProps,"PaymentType",clinicNum);
			comboPaymentType.Items.Clear();
			_listPayTypeDefs=Defs.GetDefsForCategory(DefCat.PaymentTypes,true);
			_listPayTypeDefs.ForEach(x => comboPaymentType.Items.Add(x.ItemName));
			comboPaymentType.SelectedIndex=_listPayTypeDefs.FindIndex(x => x.DefNum.ToString()==payTypeDefNum);
			//Other text boxes and check boxes
			textUsername.Text=ProgramProperties.GetPropValFromList(_listProgProps,"Username",clinicNum);
			textXWebID.Text=ProgramProperties.GetPropValFromList(_listProgProps,"XWebID",clinicNum);
			textTerminalID.Text=ProgramProperties.GetPropValFromList(_listProgProps,"TerminalID",clinicNum);
			checkWebPayEnabled.Checked=PIn.Bool(ProgramProperties.GetPropValFromList(_listProgProps,"IsOnlinePaymentsEnabled",clinicNum));
			checkPromptSig.Checked=PIn.Bool(ProgramProperties.GetPropValFromList(_listProgProps,"PromptSignature",clinicNum));
			checkPrintReceipt.Checked=PIn.Bool(ProgramProperties.GetPropValFromList(_listProgProps,"PrintReceipt",clinicNum));
			checkForceDuplicate.Checked=PIn.Bool(ProgramProperties.GetPropValFromList(_listProgProps,
				XCharge.ProgramProperties.XChargeForceRecurringCharge,clinicNum));
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboClinic.SelectedIndex==_indexClinicRevert) {//didn't change the selected clinic
				return;
			}
			//XWeb will be enabled for the clinic if the enabled checkbox is checked and the 3 XWeb fields are not blank.  Don't let them switch clinics or
			//close the form with only 1 or 2 of the three fields filled in.  If they fill in 1, they must fill in the other 2.  Per JasonS - 10/12/2015
			bool isXWebEnabled=checkEnabled.Checked && (textXWebID.Text.Trim().Length>0 || textAuthKey.Text.Trim().Length>0  || textTerminalID.Text.Trim().Length>0);
			if(isXWebEnabled && !ValidateXWeb()) {//error message box displayed in ValidateXWeb()
				comboClinic.SelectedIndex=_indexClinicRevert;//validation didn't pass, revert clinic choice so they have to fix it
				return;//if any of the X-Web fields do not pass validation, return
			}
			//if the payment type currently set is not valid and X-Charge is enabled, revert the clinic and return, message box shown in ValidatePaymentTypes
			if(!ValidatePaymentTypes(false)) {
				comboClinic.SelectedIndex=_indexClinicRevert;//revert clinic selection, X-Charge is enabled and the payment type is not valid
				return;
			}
			SyncWithHQ();//if the user just modified the HQ credentials, change any credentials that were the same as HQ to keep them synched
			string passwordEncrypted="";
			if(textPassword.Text.Trim().Length>0) {
				passwordEncrypted=CodeBase.MiscUtils.Encrypt(textPassword.Text.Trim());
			}			
			string payTypeCur="";
			if(comboPaymentType.SelectedIndex>-1) {
				payTypeCur=_listPayTypeDefs[comboPaymentType.SelectedIndex].DefNum.ToString();
			}
			_listProgProps.FindAll(x => x.ClinicNum==_listUserClinicNums[_indexClinicRevert] && x.PropertyDesc=="Username")
				.ForEach(x => x.PropertyValue=textUsername.Text.Trim());//always 1 item, null safe
			_listProgProps.FindAll(x => x.ClinicNum==_listUserClinicNums[_indexClinicRevert] && x.PropertyDesc=="Password")
				.ForEach(x => x.PropertyValue=passwordEncrypted);//always 1 item, null safe
			_listProgProps.FindAll(x => x.ClinicNum==_listUserClinicNums[_indexClinicRevert] && x.PropertyDesc=="PromptSignature")
				.ForEach(x => x.PropertyValue=POut.Bool(checkPromptSig.Checked));//always 1 item, null safe
			_listProgProps.FindAll(x => x.ClinicNum==_listUserClinicNums[_indexClinicRevert] && x.PropertyDesc=="PrintReceipt")
				.ForEach(x => x.PropertyValue=POut.Bool(checkPrintReceipt.Checked));//always 1 item, null safe
			_listProgProps.FindAll(x => x.ClinicNum==_listUserClinicNums[_indexClinicRevert] && x.PropertyDesc=="XWebID")
				.ForEach(x => x.PropertyValue=textXWebID.Text.Trim());//always 1 item, null safe
			_listProgProps.FindAll(x => x.ClinicNum==_listUserClinicNums[_indexClinicRevert] && x.PropertyDesc=="AuthKey")
				.ForEach(x => x.PropertyValue=textAuthKey.Text.Trim());//always 1 item, null safe
			_listProgProps.FindAll(x => x.ClinicNum==_listUserClinicNums[_indexClinicRevert] && x.PropertyDesc=="TerminalID")
				.ForEach(x => x.PropertyValue=textTerminalID.Text.Trim());//always 1 item, null safe
			_listProgProps.FindAll(x => x.ClinicNum==_listUserClinicNums[_indexClinicRevert] && x.PropertyDesc=="IsOnlinePaymentsEnabled")
				.ForEach(x => x.PropertyValue=POut.Bool(checkWebPayEnabled.Checked));//always 1 item, null safe
			_listProgProps.FindAll(x => x.ClinicNum==_listUserClinicNums[_indexClinicRevert] && x.PropertyDesc=="PaymentType")//payment type already validated
				.ForEach(x => x.PropertyValue=payTypeCur);//always 1 item, null safe
			_listProgProps.FindAll(x => x.ClinicNum==_listUserClinicNums[_indexClinicRevert] && 
				x.PropertyDesc==XCharge.ProgramProperties.XChargeForceRecurringCharge)
				.ForEach(x => x.PropertyValue=POut.Bool(checkForceDuplicate.Checked));
			_indexClinicRevert=comboClinic.SelectedIndex;//now that we've updated the values for the clinic we're switching from, update _indexClinicRevert
			textPassword.UseSystemPasswordChar=false;//FillFields will set this to true if the clinic being selected has a password set
			textAuthKey.UseSystemPasswordChar=false;//FillFields will set this to true if the clinic being selected has an AuthKey entered
			FillFields();
		}

		//This method will be used if we want to start allowing use of Usernames and Passwords in lieu of the Auth Key.  Called from validate in
		//butOK_Click, but for now we don't allow users to enter Username and Password in liew of Auth Key, so not in use.
		/////<summary>Call to validate that the password typed in meets the X-Web password strength requirements.  Passwords must be between 8 and 15
		/////characters in length, and must contain at least one letter, one number, and one of these special characters: $%^&+=</summary>
		//private bool IsPasswordXWebValid() {
		//	string password=textPassword.Text.Trim();
		//	if(password.Length < 8 || password.Length > 15) {//between 8 - 15 chars
		//		return false;
		//	}
		//	if(!Regex.IsMatch(password,"[A-Za-z]+")) {//must contain at least one letter
		//		return false;
		//	}
		//	if(!Regex.IsMatch(password,"[0-9]+")) {//must contain at least one number
		//		return false;
		//	}
		//	if(!Regex.IsMatch(password,"[$%^&+=]+")) {//must contain at least one special character
		//		return false;
		//	}
		//	return true;
		//}

		private void textPassword_TextChanged(object sender,EventArgs e) {
			//Let the users see what they are typing if they clear out the password field completely
			if(textPassword.Text.Trim().Length==0) {
				textPassword.UseSystemPasswordChar=false;
			}
		}

		private void textAuthKey_TextChanged(object sender,EventArgs e) {
			//We want to let users see what they are typing in if they cleared out the AuthKey field completely or are typing in for the first time.
			//X-Charge does this in their server settings window.  We shall do the same.
			if(textAuthKey.Text.Trim().Length==0) {
				textAuthKey.UseSystemPasswordChar=false;
			}
		}

		private void linkLabel1_LinkClicked(object sender,LinkLabelLinkClickedEventArgs e) {
			Process.Start("https://openedgepayments.com/opendental/");
		}

		///<summary>Validate XWebID, AuthKey, and TerminalID.  XWebID and TerminalID must be numbers only, 12 digits and 8 digits long respectively.
		///AuthKey must be alphanumeric, 32 digits long.</summary>
		private bool ValidateXWeb(bool suppressMessage=false) {
			//Validate ALL XWebID, AuthKey, and TerminalID.  Each is required for X-Web to work.
			if(!Regex.IsMatch(textXWebID.Text.Trim(),"^[0-9]{12}$")) {
				if(!suppressMessage) {
					MsgBox.Show(this,"XWeb ID must be 12 digits.");
				}
				return false;
			}
			if(!Regex.IsMatch(textAuthKey.Text.Trim(),"^[A-Za-z0-9]{32}$")) {
				if(!suppressMessage) {
					MsgBox.Show(this,"Auth Key must be 32 alphanumeric characters.");
				}
				return false;
			}
			if(!Regex.IsMatch(textTerminalID.Text.Trim(),"^[0-9]{8}$")) {
				if(!suppressMessage) {
					MsgBox.Show(this,"Terminal ID must be 8 digits.");
				}
				return false;
			}
			////We are not going to give the option for users to use their Username and Password.
			////The following password strength requirement would need to be enforced if we want to start allowing use of
			////Usernames and Passwords in lieu of the Auth Key.
			////XWebID and TerminalID are valid.  Make sure the password meets the required complexity for XWeb.
			//if(!IsPasswordXWebValid()) {
			//	MessageBox.Show(this,Lan.g(this,"Passwords must be between 8 and 15 characters in length and must contain at least one letter, "
			//		+"one number, and one of these special characters")+": $%^&+=");
			//	return;
			//}
			return true;
		}

		///<summary>If isValidateAllClinics is true, validates the PaymentType for all clinics with X-Charge enabled.
		///<para>If the current user is restricted to a clinic or if clinics are not enabled and the enabled checkbox is checked,
		///or if isValidateAllClinics is false, only comboPaymentType.SelectedIndex will be validated.</para>
		///<para>If clinics are enabled and isValidateAllClinics is true and the current user is not restricted to a clinic, the PaymentType for any
		///clinic with Username and Password set or with any of the XWeb settings set will be validated.</para></summary>
		private bool ValidatePaymentTypes(bool isAllClinics) {
			//if not enabled, don't worry about invalid payment type
			if(!checkEnabled.Checked) {
				return true;
			}
			//XWeb will be enabled for the clinic if the XWeb enabled checkbox is checked and the 3 XWeb fields are not blank.  Don't let them switch clinics or
			//close the form with only 1 or 2 of the three fields filled in.  If they fill in 1, they must fill in the other 2.  Per JasonS - 10/12/2015
			bool isXWebEnabled=checkWebPayEnabled.Checked 
				&& (textXWebID.Text.Trim().Length>0 || textAuthKey.Text.Trim().Length>0  || textTerminalID.Text.Trim().Length>0);
			//X-Charge will be enabled if the enabled checkbox is checked and either clinics are disabled OR both Username and Password are set
			bool isClientEnabled=!PrefC.HasClinicsEnabled || (textUsername.Text.Trim().Length>0 && textPassword.Text.Trim().Length>0);
			if((isClientEnabled || isXWebEnabled) && comboPaymentType.SelectedIndex<0) {
				MsgBox.Show(this,"Please select a payment type first.");
				return false;
			}
			if(!isAllClinics || !PrefC.HasClinicsEnabled || Security.CurUser.ClinicIsRestricted) {
				return true;
			}
			//only validate payment types for all clinics if isAllClinics==true and clinics are enabled and the current user is not restricted to a clinic
			string payTypeCur="";
			//make sure all clinics with X-Charge enabled also have a payment type selected
			for(int i=0;i<_listUserClinicNums.Count;i++) {
				payTypeCur=ProgramProperties.GetPropValFromList(_listProgProps,"PaymentType",_listUserClinicNums[i]);
				//isClientEnabled will be true if both username and password are set for this clinic
				isClientEnabled=ProgramProperties.GetPropValFromList(_listProgProps,"Username",_listUserClinicNums[i]).Length>0
					&& ProgramProperties.GetPropValFromList(_listProgProps,"Password",_listUserClinicNums[i]).Length>0;
				//isXWebEnabled will be true if any of the XWeb values are set
				isXWebEnabled=checkWebPayEnabled.Checked
					&& (ProgramProperties.GetPropValFromList(_listProgProps,"XWebID",_listUserClinicNums[i]).Length>0
					|| ProgramProperties.GetPropValFromList(_listProgProps,"AuthKey",_listUserClinicNums[i]).Length>0
					|| ProgramProperties.GetPropValFromList(_listProgProps,"TerminalID",_listUserClinicNums[i]).Length>0);
				//if the program is enabled and the username and password fields are not blank for client, or XWebID, AuthKey, and TerminalID are not blank
				//for XWeb, then X-Charge is enabled for this clinic so make sure the payment type is also set
				if((isClientEnabled || isXWebEnabled)	&& !_listPayTypeDefs.Any(x => x.DefNum.ToString()==payTypeCur)) {
					MsgBox.Show(this,"Please select the payment type for all clinics with X-Charge enabled.");
					return false;
				}
			}
			return true;
		}

		///<summary>Updates the values in the local list of program properties for each clinic.
		///Only modifies other clinics if _listUserClinicNums[_indexClinicRevert]=0, meaning user just modified the HQ values.
		///If the clinic X-Charge client Username and Password are the same as HQ, the clinic values will be updated with the values entered.
		///If the clinic XWeb values are the same as HQ, the clinic XWeb values will be updated with the values entered.
		///If both the X-Charge client values and the XWeb values are the same as HQ, the payment type will be updated.
		///The values in the local list for HQ, or for the clinic modified if it was not HQ, have to be updated after calling this method.</summary>
		private void SyncWithHQ() {
			if(!PrefC.HasClinicsEnabled || _listUserClinicNums[_indexClinicRevert]>0) {
				return;
			}
			string hqUsername=ProgramProperties.GetPropValFromList(_listProgProps,"Username",0);//HQ Username before updating to value in textbox
			string hqPassword=ProgramProperties.GetPropValFromList(_listProgProps,"Password",0);//HQ Password before updating to value in textbox
			string hqXWebID=ProgramProperties.GetPropValFromList(_listProgProps,"XWebID",0);//HQ XWebID before updating to value in textbox
			string hqAuthKey=ProgramProperties.GetPropValFromList(_listProgProps,"AuthKey",0);//HQ AuthKey before updating to value in textbox
			string hqTerminalID=ProgramProperties.GetPropValFromList(_listProgProps,"TerminalID",0);//HQ TerminalID before updating to value in textbox
			string hqPayType=ProgramProperties.GetPropValFromList(_listProgProps,"PaymentType",0);//HQ PaymentType before updating to combo box selection
			//IsOnlinePaymentsEnabled will not be synced with HQ so specific clinics can be disabled for patient portal payments.
			string payTypeCur="";
			if(comboPaymentType.SelectedIndex>-1) {
				payTypeCur=_listPayTypeDefs[comboPaymentType.SelectedIndex].DefNum.ToString();
			}
			string passwordEncrypted="";
			if(textPassword.Text.Trim().Length>0) {
				passwordEncrypted=CodeBase.MiscUtils.Encrypt(textPassword.Text.Trim());
			}
			//for each distinct ClinicNum in the prog property list for X-Charge except HQ
			foreach(long clinicNum in _listProgProps.Select(x => x.ClinicNum).Where(x => x>0).Distinct()) {
				//Updates the PaymentType in both if checks, in case the other isn't met so the payment type will be synched if either condition is true.
				//if this clinic has the same Username and Password, update them
				bool isClientSynch=_listProgProps.Exists(x => x.ClinicNum==clinicNum && x.PropertyDesc=="Username" && x.PropertyValue==hqUsername)
					&& _listProgProps.Exists(x => x.ClinicNum==clinicNum && x.PropertyDesc=="Password" && x.PropertyValue==hqPassword);
				//only if all three XWeb HQ values are not blank
				bool isXWebSynch=_listProgProps.Exists(x => x.ClinicNum==clinicNum && x.PropertyDesc=="XWebID" && x.PropertyValue==hqXWebID)
					&& _listProgProps.Exists(x => x.ClinicNum==clinicNum && x.PropertyDesc=="AuthKey" && x.PropertyValue==hqAuthKey)
					&& _listProgProps.Exists(x => x.ClinicNum==clinicNum && x.PropertyDesc=="TerminalID" && x.PropertyValue==hqTerminalID);
				if(!isClientSynch && !isXWebSynch) {
					continue;
				}
				if(isClientSynch) {
					//update the username and password to keep it synched with HQ
					_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="Username")
						.ForEach(x => x.PropertyValue=textUsername.Text.Trim());//always 1 item; null safe
					_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="Password")
						.ForEach(x => x.PropertyValue=passwordEncrypted);//always 1 item; null safe
				}
				if(isXWebSynch) {
					//update the XWebID, AuthKey, and TerminalID to keep it synched with HQ
					_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="XWebID")
						.ForEach(x => x.PropertyValue=textXWebID.Text.Trim());//always 1 item; null safe
					_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="AuthKey")
						.ForEach(x => x.PropertyValue=textAuthKey.Text.Trim());//always 1 item; null safe
					_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="TerminalID")
						.ForEach(x => x.PropertyValue=textTerminalID.Text.Trim());//always 1 item; null safe
				}
				//only synch payment type if both client and XWeb values are the same as HQ and the payment type is valid
				if(isClientSynch && isXWebSynch && !string.IsNullOrEmpty(payTypeCur)) {
					_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="PaymentType" && x.PropertyValue==hqPayType)
						.ForEach(x => x.PropertyValue=payTypeCur);//always 1 item; null safe
				}
			}
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			#region Validation and Update Local List
			if(_progCur==null) {//should never happen
				MsgBox.Show(this,"X-Charge entry is missing from the database.");
				return;
			}
			#region Validate Path and Local Path Override
			//Check for the path override first.
			if(checkEnabled.Checked && textOverride.Text.Trim().Length>0) {
				if(!File.Exists(textOverride.Text.Trim())) {
					MsgBox.Show(this,"Local Path Override is not valid.");
					return;
				}
			}
			//Check the global path if no override was entered.
			else if(checkEnabled.Checked && textOverride.Text.Trim().Length==0) {
				//No override was entered so validate the global path which is required at this point.
				if((textPath.Text.Trim().Length==0 || !File.Exists(textPath.Text.Trim()))
					&& !(checkWebPayEnabled.Checked && ValidateXWeb(true) && textPath.Text.Trim().Length==0)) 
				{
					MsgBox.Show(this,"Program Path is not valid.");
					return;
				}
			}
			#endregion Validate Path and Local Path Override
			#region Validate Username and Password
			//If clinics are not enabled and the X-Charge program link is enabled, make sure there is a username and password set.
			//If clinics are enabled, the program link can be enabled with blank username and/or password fields for some clinics.
			//X-Charge will be disabled for any clinic with a blank username or password.
			if(checkEnabled.Checked && !PrefC.HasClinicsEnabled && (textUsername.Text.Trim().Length==0 || textPassword.Text.Trim().Length==0)) {
				MsgBox.Show(this,"Please enter a username and password first.");
				return;
			}
			#endregion Validate Username and Password
			#region Validate X-Web WebID, AuthKey, and TerminalID
			//Check to see if ANY X-Web settings have been set, and if so validate them
			//XWeb will be enabled for the clinic if the enabled checkbox is checked and the 3 XWeb fields are not blank.  Don't let them switch clinics or
			//close the form with only 1 or 2 of the three fields filled in.  If they fill in 1, they must fill in the other 2.  Per JasonS - 10/12/2015
			if(checkEnabled.Checked
				&& (textXWebID.Text.Trim().Length>0 || textAuthKey.Text.Trim().Length>0  || textTerminalID.Text.Trim().Length>0)
				&& !ValidateXWeb()) //error message box displayed in ValidateXWeb()
			{
				return;
			}
			#endregion Validate X-Web WebID, AuthKey, and TerminalID
			#region Update Local List of Program Properties
			//if the user just modified the HQ credentials, change any credentials that were the same as HQ to keep them synched
			SyncWithHQ();
			//get selected ClinicNum (if enabled), PaymentType, encrypted Password, and encrypted AuthKey
			long clinicNum=0;
			if(PrefC.HasClinicsEnabled) {
				clinicNum=_listUserClinicNums[comboClinic.SelectedIndex];
			}
			string passwordEncrypted="";
			if(textPassword.Text.Trim().Length>0) {
				passwordEncrypted=CodeBase.MiscUtils.Encrypt(textPassword.Text.Trim());
			}			
			string payTypeCur="";
			if(comboPaymentType.SelectedIndex>-1) {
				payTypeCur=_listPayTypeDefs[comboPaymentType.SelectedIndex].DefNum.ToString();
			}
			//set the values in the list for this clinic
			_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="Username")
				.ForEach(x => x.PropertyValue=textUsername.Text.Trim());//always 1 item; null safe
			_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="Password")
				.ForEach(x => x.PropertyValue=passwordEncrypted);//always 1 item; null safe
			_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="PromptSignature")
				.ForEach(x => x.PropertyValue=POut.Bool(checkPromptSig.Checked));//always 1 item, null safe
			_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="PrintReceipt")
				.ForEach(x => x.PropertyValue=POut.Bool(checkPrintReceipt.Checked));//always 1 item, null safe
			_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="XWebID")
				.ForEach(x => x.PropertyValue=textXWebID.Text.Trim());//always 1 item, null safe
			_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="AuthKey")
				.ForEach(x => x.PropertyValue=textAuthKey.Text.Trim());//always 1 item, null safe
			_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="TerminalID")
				.ForEach(x => x.PropertyValue=textTerminalID.Text.Trim());//always 1 item, null safe
			_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="IsOnlinePaymentsEnabled")
				.ForEach(x => x.PropertyValue=POut.Bool(checkWebPayEnabled.Checked));//always 1 item, null safe
			_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="PaymentType")
				.ForEach(x => x.PropertyValue=payTypeCur);//always 1 item; null safe
			_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc==XCharge.ProgramProperties.XChargeForceRecurringCharge)
				.ForEach(x => x.PropertyValue=POut.Bool(checkForceDuplicate.Checked));
			#endregion Update Local List of Program Properties
			#region Validate PaymentTypes For All Clinics
			//validate the payment type set for all clinics with X-Charge enabled
			if(!ValidatePaymentTypes(true)) {//if validation fails, message box will already have been shown
				return;
			}
			#endregion Validate PaymentTypes For All Clinics
			#endregion Validation and Update Local List
			#region Save
			if(_progCur.Enabled!=checkEnabled.Checked || _progCur.Path!=textPath.Text.Trim()) {//update the program if the IsEnabled flag or Path has changed
				_progCur.Enabled=checkEnabled.Checked;
				_progCur.Path=textPath.Text.Trim();
				Programs.Update(_progCur);
			}
			if(ProgramProperties.GetLocalPathOverrideForProgram(_progCur.ProgramNum)!=textOverride.Text.Trim()) {
				ProgramProperties.InsertOrUpdateLocalOverridePath(_progCur.ProgramNum,textOverride.Text.Trim());
			}
			ProgramProperties.Sync(_listProgProps,_progCur.ProgramNum);
			#endregion Save
			DataValid.SetInvalid(InvalidType.Programs);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}





















