using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using CodeBase;
using Ionic.Zip;
using OpenDentBusiness;
using OpenDental.Bridges;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormPayConnectSetup : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private LinkLabel linkLabel1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private CheckBox checkEnabled;
		private Label label1;
		private ComboBox comboPaymentType;
		private Label label2;
		private TextBox textUsername;
		private Label label3;
		private TextBox textPassword;
		private ComboBox comboClinic;
		private Label labelClinic;
		private GroupBox groupPaySettings;
		private Label labelClinicEnable;
		private Program _progCur;
		///<summary>Local cache of all of the clinic nums the current user has permission to access at the time the form loads.  Filled at the same time
		///as comboClinic and is used to set programproperty.ClinicNum when saving.</summary>
		private List<long> _listUserClinicNums;
		///<summary>List of PayConnect program properties for all clinics.
		///Includes properties with ClinicNum=0, the headquarters props or props not assigned to a clinic.</summary>
		private List<ProgramProperty> _listProgProps;
		private CheckBox checkWebPayEnabled;
		private CheckBox checkTerminal;
		private UI.Button butDownloadDriver;
		private CheckBox checkForceRecurring;
		private ComboBox comboDefaultProcessing;
		private Label labelDefaultProcMethod;

		///<summary>Used to revert the slected index in the clinic drop down box if the user tries to change clinics
		///and the payment type has not been set.</summary>
		private int _indexClinicRevert;
		private List<Def> _listPaymentTypeDefs;

		///<summary></summary>
		public FormPayConnectSetup()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPayConnectSetup));
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.checkEnabled = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.comboPaymentType = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textUsername = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textPassword = new System.Windows.Forms.TextBox();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.groupPaySettings = new System.Windows.Forms.GroupBox();
			this.comboDefaultProcessing = new System.Windows.Forms.ComboBox();
			this.labelDefaultProcMethod = new System.Windows.Forms.Label();
			this.checkForceRecurring = new System.Windows.Forms.CheckBox();
			this.checkTerminal = new System.Windows.Forms.CheckBox();
			this.checkWebPayEnabled = new System.Windows.Forms.CheckBox();
			this.butDownloadDriver = new OpenDental.UI.Button();
			this.labelClinicEnable = new System.Windows.Forms.Label();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupPaySettings.SuspendLayout();
			this.SuspendLayout();
			// 
			// linkLabel1
			// 
			this.linkLabel1.LinkArea = new System.Windows.Forms.LinkArea(29, 28);
			this.linkLabel1.Location = new System.Drawing.Point(10, 13);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(312, 16);
			this.linkLabel1.TabIndex = 0;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "The PayConnect website is at www.dentalxchange.com";
			this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.linkLabel1.UseCompatibleTextRendering = true;
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// checkEnabled
			// 
			this.checkEnabled.Location = new System.Drawing.Point(10, 37);
			this.checkEnabled.Name = "checkEnabled";
			this.checkEnabled.Size = new System.Drawing.Size(226, 18);
			this.checkEnabled.TabIndex = 1;
			this.checkEnabled.Text = "Enabled (affects all clinics)";
			this.checkEnabled.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.checkEnabled.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(124, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Payment Type";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboPaymentType
			// 
			this.comboPaymentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPaymentType.FormattingEnabled = true;
			this.comboPaymentType.Location = new System.Drawing.Point(131, 19);
			this.comboPaymentType.MaxDropDownItems = 25;
			this.comboPaymentType.Name = "comboPaymentType";
			this.comboPaymentType.Size = new System.Drawing.Size(175, 21);
			this.comboPaymentType.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 75);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(124, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "Username";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textUsername
			// 
			this.textUsername.Location = new System.Drawing.Point(131, 73);
			this.textUsername.Name = "textUsername";
			this.textUsername.Size = new System.Drawing.Size(175, 20);
			this.textUsername.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(6, 98);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(124, 16);
			this.label3.TabIndex = 0;
			this.label3.Text = "Password";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textPassword
			// 
			this.textPassword.Location = new System.Drawing.Point(131, 96);
			this.textPassword.Name = "textPassword";
			this.textPassword.Size = new System.Drawing.Size(175, 20);
			this.textPassword.TabIndex = 2;
			this.textPassword.UseSystemPasswordChar = true;
			this.textPassword.TextChanged += new System.EventHandler(this.textPassword_TextChanged);
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(141, 95);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(175, 21);
			this.comboClinic.TabIndex = 3;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.comboClinic_SelectionChangeCommitted);
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(10, 98);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(130, 16);
			this.labelClinic.TabIndex = 0;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupPaySettings
			// 
			this.groupPaySettings.Controls.Add(this.comboDefaultProcessing);
			this.groupPaySettings.Controls.Add(this.labelDefaultProcMethod);
			this.groupPaySettings.Controls.Add(this.checkForceRecurring);
			this.groupPaySettings.Controls.Add(this.checkTerminal);
			this.groupPaySettings.Controls.Add(this.checkWebPayEnabled);
			this.groupPaySettings.Controls.Add(this.textPassword);
			this.groupPaySettings.Controls.Add(this.label3);
			this.groupPaySettings.Controls.Add(this.textUsername);
			this.groupPaySettings.Controls.Add(this.label2);
			this.groupPaySettings.Controls.Add(this.comboPaymentType);
			this.groupPaySettings.Controls.Add(this.label1);
			this.groupPaySettings.Location = new System.Drawing.Point(10, 122);
			this.groupPaySettings.Name = "groupPaySettings";
			this.groupPaySettings.Size = new System.Drawing.Size(355, 195);
			this.groupPaySettings.TabIndex = 4;
			this.groupPaySettings.TabStop = false;
			this.groupPaySettings.Text = "Clinic Payment Settings";
			// 
			// comboDefaultProcessing
			// 
			this.comboDefaultProcessing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboDefaultProcessing.FormattingEnabled = true;
			this.comboDefaultProcessing.Location = new System.Drawing.Point(131, 46);
			this.comboDefaultProcessing.MaxDropDownItems = 25;
			this.comboDefaultProcessing.Name = "comboDefaultProcessing";
			this.comboDefaultProcessing.Size = new System.Drawing.Size(175, 21);
			this.comboDefaultProcessing.TabIndex = 6;
			// 
			// labelDefaultProcMethod
			// 
			this.labelDefaultProcMethod.Location = new System.Drawing.Point(6, 43);
			this.labelDefaultProcMethod.Name = "labelDefaultProcMethod";
			this.labelDefaultProcMethod.Size = new System.Drawing.Size(124, 26);
			this.labelDefaultProcMethod.TabIndex = 7;
			this.labelDefaultProcMethod.Text = "Default Processing Method";
			this.labelDefaultProcMethod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkForceRecurring
			// 
			this.checkForceRecurring.Location = new System.Drawing.Point(131, 158);
			this.checkForceRecurring.Name = "checkForceRecurring";
			this.checkForceRecurring.Size = new System.Drawing.Size(218, 30);
			this.checkForceRecurring.TabIndex = 5;
			this.checkForceRecurring.Text = "Recurring charge list force duplicates by default";
			this.checkForceRecurring.UseVisualStyleBackColor = true;
			// 
			// checkTerminal
			// 
			this.checkTerminal.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkTerminal.Location = new System.Drawing.Point(131, 121);
			this.checkTerminal.Name = "checkTerminal";
			this.checkTerminal.Size = new System.Drawing.Size(218, 17);
			this.checkTerminal.TabIndex = 3;
			this.checkTerminal.Text = "Enable terminal processing";
			this.checkTerminal.CheckedChanged += new System.EventHandler(this.checkTerminal_CheckedChanged);
			// 
			// checkWebPayEnabled
			// 
			this.checkWebPayEnabled.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkWebPayEnabled.Location = new System.Drawing.Point(131, 140);
			this.checkWebPayEnabled.Name = "checkWebPayEnabled";
			this.checkWebPayEnabled.Size = new System.Drawing.Size(218, 17);
			this.checkWebPayEnabled.TabIndex = 4;
			this.checkWebPayEnabled.Text = "Enable for patient portal payments";
			// 
			// butDownloadDriver
			// 
			this.butDownloadDriver.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDownloadDriver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDownloadDriver.Autosize = true;
			this.butDownloadDriver.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDownloadDriver.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDownloadDriver.CornerRadius = 4F;
			this.butDownloadDriver.Location = new System.Drawing.Point(10, 326);
			this.butDownloadDriver.Name = "butDownloadDriver";
			this.butDownloadDriver.Size = new System.Drawing.Size(93, 26);
			this.butDownloadDriver.TabIndex = 5;
			this.butDownloadDriver.Text = "Download Driver";
			this.butDownloadDriver.Visible = false;
			this.butDownloadDriver.Click += new System.EventHandler(this.butDownloadDriver_Click);
			// 
			// labelClinicEnable
			// 
			this.labelClinicEnable.Location = new System.Drawing.Point(44, 58);
			this.labelClinicEnable.Name = "labelClinicEnable";
			this.labelClinicEnable.Size = new System.Drawing.Size(246, 28);
			this.labelClinicEnable.TabIndex = 2;
			this.labelClinicEnable.Text = "To enable PayConnect for a clinic, set the Username and Password for that clinic." +
    "";
			this.labelClinicEnable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(204, 326);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 6;
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
			this.butCancel.Location = new System.Drawing.Point(290, 326);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 7;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormPayConnectSetup
			// 
			this.ClientSize = new System.Drawing.Size(377, 364);
			this.Controls.Add(this.butDownloadDriver);
			this.Controls.Add(this.labelClinicEnable);
			this.Controls.Add(this.groupPaySettings);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.checkEnabled);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(393, 340);
			this.Name = "FormPayConnectSetup";
			this.ShowInTaskbar = false;
			this.Text = "PayConnect Setup";
			this.Load += new System.EventHandler(this.FormPayConnectSetup_Load);
			this.groupPaySettings.ResumeLayout(false);
			this.groupPaySettings.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormPayConnectSetup_Load(object sender,EventArgs e) {
			//This feature is not yet available for PayConnect.  The programproperty IsOnlinePaymentsEnabled has been added for PayConnect but the ability
			//to use it in the patient portal has not yet been implemented.
			checkWebPayEnabled.Visible=false;
			_progCur=Programs.GetCur(ProgramName.PayConnect);
			if(_progCur==null) {
				MsgBox.Show(this,"The PayConnect entry is missing from the database.");//should never happen
				return;
			}
			checkEnabled.Checked=_progCur.Enabled;
			if(!PrefC.HasClinicsEnabled) {//clinics are not enabled, use ClinicNum 0 to indicate 'Headquarters' or practice level program properties
				checkEnabled.Text=Lan.g(this,"Enabled");
				groupPaySettings.Text=Lan.g(this,"Payment Settings");
				comboClinic.Visible=false;
				labelClinic.Visible=false;
				labelClinicEnable.Visible=false;
				_listUserClinicNums=new List<long>() { 0 };//if clinics are disabled, programproperty.ClinicNum will be set to 0
			}
			else {//Using clinics
				groupPaySettings.Text=Lan.g(this,"Clinic Payment Settings");
				_listUserClinicNums=new List<long>();
				comboClinic.Items.Clear();
				//if PayConnect is enabled and the user is restricted to a clinic, don't allow the user to disable for all clinics
				if(Security.CurUser.ClinicIsRestricted) {
					if(checkEnabled.Checked) {
						checkEnabled.Enabled=false;
					}
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
							comboClinic.SelectedIndex++;//increment the SelectedIndex to account for 'Headquarters' in the list at position 0 if the user is not restricted.
						}
					}
				}
				_indexClinicRevert=comboClinic.SelectedIndex;
			}
			_listProgProps=ProgramProperties.GetForProgram(_progCur.ProgramNum);
			FillFields();
		}

		private void FillFields() {
			long clinicNum=0;
			if(PrefC.HasClinicsEnabled) {
				clinicNum=_listUserClinicNums[comboClinic.SelectedIndex];
			}
			textUsername.Text=ProgramProperties.GetPropValFromList(_listProgProps,"Username",clinicNum);
			textPassword.Text=ProgramProperties.GetPropValFromList(_listProgProps,"Password",clinicNum);
			string payTypeDefNum=ProgramProperties.GetPropValFromList(_listProgProps,"PaymentType",clinicNum);
			string processingMethod=ProgramProperties.GetPropValFromList(_listProgProps,PayConnect.ProgramProperties.DefaultProcessingMethod,clinicNum);
			checkTerminal.Checked=PIn.Bool(ProgramProperties.GetPropValFromList(_listProgProps,"TerminalProcessingEnabled",clinicNum));
			checkForceRecurring.Checked=PIn.Bool(ProgramProperties.GetPropValFromList(_listProgProps,
				PayConnect.ProgramProperties.PayConnectForceRecurringCharge,clinicNum));
			//Patient portal payments with PayConnect are currently not supported, checkWebPayEnabled is never visible, so always set to 0,
			//but we'll leave this here for future functionality
			//checkWebPayEnabled.Checked=PIn.Bool(ProgramProperties.GetPropValFromList(_listProgProps,"IsOnlinePaymentsEnabled",clinicNum));
			comboPaymentType.Items.Clear();
			_listPaymentTypeDefs=Defs.GetDefsForCategory(DefCat.PaymentTypes,true);
			for(int i=0;i<_listPaymentTypeDefs.Count;i++) {
				comboPaymentType.Items.Add(_listPaymentTypeDefs[i].ItemName);
				if(_listPaymentTypeDefs[i].DefNum.ToString()==payTypeDefNum) {
					comboPaymentType.SelectedIndex=i;
				}
			}
			comboDefaultProcessing.Items.Clear();
			comboDefaultProcessing.Items.Add(Lan.g(this,PayConnectProcessingMethod.WebService.GetDescription()));
			comboDefaultProcessing.Items.Add(Lan.g(this,PayConnectProcessingMethod.Terminal.GetDescription()));
			if(processingMethod=="0" || processingMethod=="1") {
				comboDefaultProcessing.SelectedIndex=PIn.Int(processingMethod);
			}
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboClinic.SelectedIndex==_indexClinicRevert) {//didn't change the selected clinic
				return;
			}
			//if PayConnect is enabled and the username and password are set for the current clinic,
			//make the user select a payment type before switching clinics
			if(checkEnabled.Checked && textUsername.Text!="" && textPassword.Text!="" && comboPaymentType.SelectedIndex==-1) {
				comboClinic.SelectedIndex=_indexClinicRevert;
				MsgBox.Show(this,"Please select a payment type first.");
				return;
			}
			SynchWithHQ();//if the user just modified the HQ credentials, change any credentials that were the same as HQ to keep them synched
			//set the values in the list for the clinic we are switching from, at _clinicIndexRevert
			_listProgProps.FindAll(x => x.ClinicNum==_listUserClinicNums[_indexClinicRevert] && x.PropertyDesc=="Username")
				.ForEach(x => x.PropertyValue=textUsername.Text);//always 1 item; null safe
			_listProgProps.FindAll(x => x.ClinicNum==_listUserClinicNums[_indexClinicRevert] && x.PropertyDesc=="Password")
				.ForEach(x => x.PropertyValue=textPassword.Text);//always 1 item; null safe
			_listProgProps.FindAll(x => x.ClinicNum==_listUserClinicNums[_indexClinicRevert] && x.PropertyDesc=="PaymentType" && comboPaymentType.SelectedIndex>-1)
				.ForEach(x => x.PropertyValue=_listPaymentTypeDefs[comboPaymentType.SelectedIndex].DefNum.ToString());//always 1 item; null safe
			_indexClinicRevert=comboClinic.SelectedIndex;//now that we've updated the values for the clinic we're switching from, update _indexClinicRevert
			FillFields();
		}

		///<summary>For each clinic, if the Username and Password are the same as the HQ (ClinicNum=0) Username and Password, update the clinic with the
		///values in the text boxes.  Only modifies other clinics if _indexClinicRevert=0, meaning user just modified the HQ clinic credentials.</summary>
		private void SynchWithHQ() {
			if(!PrefC.HasClinicsEnabled || _listUserClinicNums[_indexClinicRevert]>0) {//using clinics, and modifying the HQ clinic. otherwise return.
				return;
			}
			string hqUsername=ProgramProperties.GetPropValFromList(_listProgProps,"Username",0);//HQ Username before updating to value in textbox
			string hqPassword=ProgramProperties.GetPropValFromList(_listProgProps,"Password",0);//HQ Password before updating to value in textbox
			string hqPayType=ProgramProperties.GetPropValFromList(_listProgProps,"PaymentType",0);//HQ PaymentType before updating to combo box selection
			//IsOnlinePaymentsEnabled will not be synced with HQ, since some clinics may need to disable patient portal payments
			string payTypeCur="";
			if(comboPaymentType.SelectedIndex>-1) {
				payTypeCur=_listPaymentTypeDefs[comboPaymentType.SelectedIndex].DefNum.ToString();
			}
			//for each distinct ClinicNum in the prog property list for PayConnect except HQ
			foreach(long clinicNum in _listProgProps.Select(x => x.ClinicNum).Where(x => x>0).Distinct()) {
				//if this clinic has a different username or password, skip it
				if(!_listProgProps.Exists(x => x.ClinicNum==clinicNum && x.PropertyDesc=="Username" && x.PropertyValue==hqUsername)
					|| !_listProgProps.Exists(x => x.ClinicNum==clinicNum && x.PropertyDesc=="Password" && x.PropertyValue==hqPassword))
				{
					continue;
				}
				//this clinic had a matching username and password, so update the username and password to keep it synched with HQ
				_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="Username")
					.ForEach(x => x.PropertyValue=textUsername.Text);//always 1 item; null safe
				_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="Password")
					.ForEach(x => x.PropertyValue=textPassword.Text);//always 1 item; null safe
				if(string.IsNullOrEmpty(payTypeCur)) {
					continue;
				}
				//update clinic payment type if it originally matched HQ's payment type and the selected payment type is valid
				_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="PaymentType" && x.PropertyValue==hqPayType)
					.ForEach(x => x.PropertyValue=payTypeCur);//always 1 item; null safe
			}
		}

		private void linkLabel1_LinkClicked(object sender,LinkLabelLinkClickedEventArgs e) {
			Process.Start("http://www.dentalxchange.com");
		}

		private void checkTerminal_CheckedChanged(object sender,EventArgs e) {
			butDownloadDriver.Visible=checkTerminal.Checked;
		}

		private void butDownloadDriver_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			WebClient client=new WebClient();
			//The VeriFone driver is necessary for PayConnect users to process payments on the VeriFone terminal.
			string zipFileName=ODFileUtils.CombinePaths(PrefC.GetTempFolderPath(),"VeriFoneUSBUARTDriver_Vx_1.0.0.52_B5.zip");
			try {
				client.DownloadFile(@"http://www.opendental.com/download/drivers/VeriFoneUSBUARTDriver_Vx_1.0.0.52_B5.zip",zipFileName);
			}
			catch(Exception ex) {
				Cursor=Cursors.Default;
				MessageBox.Show(Lan.g(this,"Unable to download driver. Error message")+": "+ex.Message);
				return;
			}
			MemoryStream ms=new MemoryStream();
			string setupFileName="";
			using(ZipFile unzipped=ZipFile.Read(zipFileName)) {
				for(int unzipIndex=0;unzipIndex<unzipped.Count;unzipIndex++) {//Unzip/write all files to the temp directory
					ZipEntry ze=unzipped[unzipIndex];
					if(ze.FileName.ToLower()=="setup.exe") {
						setupFileName=ze.FileName;
					}
					ze.Extract(PrefC.GetTempFolderPath(),ExtractExistingFileAction.OverwriteSilently);
				}
			}
			Cursor=Cursors.Default;
			if(setupFileName=="") {
				MsgBox.Show(this,"Unable to install driver. Setup.exe file not found.");
				return;
			}
			//Run the setup.exe file
			Process.Start(ODFileUtils.CombinePaths(PrefC.GetTempFolderPath(),setupFileName));
			MessageBox.Show(Lans.g(this,"Download complete. Run the Setup.exe file in")+" "+PrefC.GetTempFolderPath()+" "
				+Lans.g(this,"if it does not start automatically."));
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			#region Validation
			//if clinics are not enabled and the PayConnect program link is enabled, make sure there is a username and password set
			//if clinics are enabled, the program link can be enabled with blank username and/or password fields for some clinics
			//clinics with blank username and/or password will essentially not have PayConnect enabled
			//if 'Enable terminal processing' is checked then the practice/clinic will not need a username and password.
			if(checkEnabled.Checked && !checkTerminal.Checked && !PrefC.HasClinicsEnabled && 
				(textUsername.Text=="" || textPassword.Text=="")) 
			{
				MsgBox.Show(this,"Please enter a username and password first.");
				return;
			}
			if(checkEnabled.Checked //if PayConnect is enabled
				&& comboPaymentType.SelectedIndex<0 //and the payment type is not set
				&& (!PrefC.HasClinicsEnabled  //and either clinics are not enabled (meaning this is the only set of username, password, payment type values)
				|| (textUsername.Text!="" && textPassword.Text!=""))) //or clinics are enabled and this clinic's link has a username and password set
			{
				MsgBox.Show(this,"Please select a payment type first.");
				return;
			}
			if(checkEnabled.Checked //if PayConnect is enabled
				&& comboDefaultProcessing.SelectedIndex < 0)
			{
				MsgBox.Show(this,"Please select a default processing method type first.");
				return;
			}
			SynchWithHQ();//if the user changes the HQ credentials, any clinic that had the same credentials will be kept in synch with HQ
			long clinicNum=0;
			if(PrefC.HasClinicsEnabled) {
				clinicNum=_listUserClinicNums[comboClinic.SelectedIndex];
			}
			string payTypeSelected="";
			if(comboPaymentType.SelectedIndex>-1) {
				payTypeSelected=_listPaymentTypeDefs[comboPaymentType.SelectedIndex].DefNum.ToString();
			}
			string processingMethodSelected=comboDefaultProcessing.SelectedIndex.ToString();
			//set the values in the list for this clinic
			_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="Username")
				.ForEach(x => x.PropertyValue=textUsername.Text);//always 1 item; null safe
			_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="Password")
				.ForEach(x => x.PropertyValue=textPassword.Text);//always 1 item; null safe
			_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="PaymentType")
				.ForEach(x => x.PropertyValue=payTypeSelected);//always 1 item; null safe
			_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc==PayConnect.ProgramProperties.DefaultProcessingMethod)
				.ForEach(x => x.PropertyValue=processingMethodSelected);
			_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="TerminalProcessingEnabled")
				.ForEach(x => x.PropertyValue=POut.Bool(checkTerminal.Checked));//always 1 item; null safe
			_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc==PayConnect.ProgramProperties.PayConnectForceRecurringCharge)
				.ForEach(x => x.PropertyValue=POut.Bool(checkForceRecurring.Checked));
			//Patient portal payments with PayConnect are currently not supported, checkWebPayEnabled is never visible, so always set to 0,
			//but we'll leave this here for future functionality
			//_listProgProps.FindAll(x => x.ClinicNum==clinicNum && x.PropertyDesc=="IsOnlinePaymentsEnabled")
			//	.ForEach(x => x.PropertyValue=POut.Bool(checkWebPayEnabled.Checked));//always 1 item; null safe
			string payTypeCur;
			//make sure any other clinics with PayConnect enabled also have a payment type selected
			for(int i=0;i<_listUserClinicNums.Count;i++) {
				if(!checkEnabled.Checked) {//if program link is not enabled, do not bother checking the payment type selected
					break;
				}
				payTypeCur=ProgramProperties.GetPropValFromList(_listProgProps,"PaymentType",_listUserClinicNums[i]);
				//if the program is enabled and the username and password fields are not blank,
				//PayConnect is enabled for this clinic so make sure the payment type is also set
				if(((ProgramProperties.GetPropValFromList(_listProgProps,"Username",_listUserClinicNums[i])!="" //if username set
					&& ProgramProperties.GetPropValFromList(_listProgProps,"Password",_listUserClinicNums[i])!="") //and password set
					|| ProgramProperties.GetPropValFromList(_listProgProps,"TerminalProcessingEnabled",_listUserClinicNums[i])=="1")//or terminal enabled
					&& !_listPaymentTypeDefs.Any(x => x.DefNum.ToString()==payTypeCur)) //and paytype is not a valid DefNum
				{
					MsgBox.Show(this,"Please select the payment type for all clinics with PayConnect username and password set.");
					return;
				}
			}
			#endregion Validation
			#region Save
			if(_progCur.Enabled!=checkEnabled.Checked) {//only update the program if the IsEnabled flag has changed
				_progCur.Enabled=checkEnabled.Checked;
				Programs.Update(_progCur);
			}
			ProgramProperties.Sync(_listProgProps,_progCur.ProgramNum);
			#endregion Save
			DataValid.SetInvalid(InvalidType.Programs);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void textPassword_TextChanged(object sender,EventArgs e) {
			//Let the users see what they are typing if they clear out the password field completely
			if(textPassword.Text.Trim().Length==0) {
				textPassword.UseSystemPasswordChar=false;
			}
		}

	}
}