using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormProcTools : ODForm {
		private OpenDental.UI.Button butClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private CheckBox checkAutocodes;
		private CheckBox checkTcodes;
		private CheckBox checkDcodes;
		private CheckBox checkNcodes;
		private Label label5;
		private CheckBox checkProcButtons;
		private OpenDental.UI.Button butRun;
		public bool Changed;
		private CheckBox checkApptProcsQuickAdd;
		private CheckBox checkRecallTypes;
		private Label labelLineOne;
		private Label labelLineTwo;

		///<summary>The actual list of ADA codes as published by the ADA.  Only available on our compiled releases.  There is no other way to get this info.  For Canada, list will get filled on Run click by downloading code list from our website.</summary>
		private List<ProcedureCode> _codeList;

		///<summary></summary>
		public FormProcTools()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProcTools));
			this.checkAutocodes = new System.Windows.Forms.CheckBox();
			this.checkTcodes = new System.Windows.Forms.CheckBox();
			this.checkDcodes = new System.Windows.Forms.CheckBox();
			this.checkNcodes = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.checkProcButtons = new System.Windows.Forms.CheckBox();
			this.checkApptProcsQuickAdd = new System.Windows.Forms.CheckBox();
			this.checkRecallTypes = new System.Windows.Forms.CheckBox();
			this.butRun = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.labelLineOne = new System.Windows.Forms.Label();
			this.labelLineTwo = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// checkAutocodes
			// 
			this.checkAutocodes.Location = new System.Drawing.Point(15, 213);
			this.checkAutocodes.Name = "checkAutocodes";
			this.checkAutocodes.Size = new System.Drawing.Size(646, 36);
			this.checkAutocodes.TabIndex = 43;
			this.checkAutocodes.Text = "Autocodes - Deletes all current autocodes and then adds the default autocodes.  P" +
    "rocedure codes must have already been entered or they cannot be added as an auto" +
    "code.";
			this.checkAutocodes.UseVisualStyleBackColor = true;
			// 
			// checkTcodes
			// 
			this.checkTcodes.Location = new System.Drawing.Point(15, 66);
			this.checkTcodes.Name = "checkTcodes";
			this.checkTcodes.Size = new System.Drawing.Size(646, 36);
			this.checkTcodes.TabIndex = 44;
			this.checkTcodes.Text = "T codes - Remove temp codes, codes that start with \"T\", which were only needed fo" +
    "r the trial version.  If a T code has already been used, then this moves it to t" +
    "he obsolete category.";
			this.checkTcodes.UseVisualStyleBackColor = true;
			// 
			// checkDcodes
			// 
			this.checkDcodes.Checked = true;
			this.checkDcodes.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkDcodes.Location = new System.Drawing.Point(15, 141);
			this.checkDcodes.Name = "checkDcodes";
			this.checkDcodes.Size = new System.Drawing.Size(646, 36);
			this.checkDcodes.TabIndex = 45;
			this.checkDcodes.Text = "D codes - Add any missing 2018 ADA codes and fix descriptions of existing codes. " +
    " This option does not work in the trial version or compiled version.";
			this.checkDcodes.UseVisualStyleBackColor = true;
			// 
			// checkNcodes
			// 
			this.checkNcodes.Location = new System.Drawing.Point(15, 108);
			this.checkNcodes.Name = "checkNcodes";
			this.checkNcodes.Size = new System.Drawing.Size(646, 36);
			this.checkNcodes.TabIndex = 46;
			this.checkNcodes.Text = "N codes - Add any missing no-fee codes.";
			this.checkNcodes.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(12, 9);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(666, 54);
			this.label5.TabIndex = 48;
			this.label5.Text = resources.GetString("label5.Text");
			// 
			// checkProcButtons
			// 
			this.checkProcButtons.Location = new System.Drawing.Point(15, 255);
			this.checkProcButtons.Name = "checkProcButtons";
			this.checkProcButtons.Size = new System.Drawing.Size(646, 36);
			this.checkProcButtons.TabIndex = 49;
			this.checkProcButtons.Text = resources.GetString("checkProcButtons.Text");
			this.checkProcButtons.UseVisualStyleBackColor = true;
			// 
			// checkApptProcsQuickAdd
			// 
			this.checkApptProcsQuickAdd.Location = new System.Drawing.Point(15, 297);
			this.checkApptProcsQuickAdd.Name = "checkApptProcsQuickAdd";
			this.checkApptProcsQuickAdd.Size = new System.Drawing.Size(646, 36);
			this.checkApptProcsQuickAdd.TabIndex = 51;
			this.checkApptProcsQuickAdd.Text = "Appt Procs Quick Add - This is the list of procedures that you pick from within t" +
    "he appt edit window.  This resets the list to default.";
			this.checkApptProcsQuickAdd.UseVisualStyleBackColor = true;
			// 
			// checkRecallTypes
			// 
			this.checkRecallTypes.Location = new System.Drawing.Point(15, 339);
			this.checkRecallTypes.Name = "checkRecallTypes";
			this.checkRecallTypes.Size = new System.Drawing.Size(646, 36);
			this.checkRecallTypes.TabIndex = 52;
			this.checkRecallTypes.Text = "Recall Types - Resets the recall types and triggers to default.  Replaces any T c" +
    "odes with D codes.";
			this.checkRecallTypes.UseVisualStyleBackColor = true;
			// 
			// butRun
			// 
			this.butRun.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butRun.Autosize = true;
			this.butRun.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRun.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRun.CornerRadius = 4F;
			this.butRun.Location = new System.Drawing.Point(477, 381);
			this.butRun.Name = "butRun";
			this.butRun.Size = new System.Drawing.Size(82, 26);
			this.butRun.TabIndex = 50;
			this.butRun.Text = "Run Now";
			this.butRun.Click += new System.EventHandler(this.butRun_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(586, 381);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(82, 26);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// labelLineOne
			// 
			this.labelLineOne.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelLineOne.Location = new System.Drawing.Point(15, 185);
			this.labelLineOne.Name = "labelLineOne";
			this.labelLineOne.Size = new System.Drawing.Size(670, 2);
			this.labelLineOne.TabIndex = 53;
			// 
			// labelLineTwo
			// 
			this.labelLineTwo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelLineTwo.Location = new System.Drawing.Point(15, 200);
			this.labelLineTwo.Name = "labelLineTwo";
			this.labelLineTwo.Size = new System.Drawing.Size(670, 2);
			this.labelLineTwo.TabIndex = 54;
			// 
			// FormProcTools
			// 
			this.ClientSize = new System.Drawing.Size(698, 431);
			this.Controls.Add(this.labelLineTwo);
			this.Controls.Add(this.labelLineOne);
			this.Controls.Add(this.checkRecallTypes);
			this.Controls.Add(this.checkApptProcsQuickAdd);
			this.Controls.Add(this.butRun);
			this.Controls.Add(this.checkProcButtons);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.checkNcodes);
			this.Controls.Add(this.checkDcodes);
			this.Controls.Add(this.checkTcodes);
			this.Controls.Add(this.checkAutocodes);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormProcTools";
			this.ShowInTaskbar = false;
			this.Text = "Procedure Code Tools";
			this.Load += new System.EventHandler(this.FormProcTools_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormProcTools_Load(object sender,EventArgs e) {
			#if TRIALONLY
				checkTcodes.Checked=false;
				checkNcodes.Checked=false;
				checkDcodes.Checked=false;
				checkAutocodes.Checked=false;
				checkProcButtons.Checked=false;
				checkApptProcsQuickAdd.Checked=false;
				checkTcodes.Enabled=false;
				//checkNcodes.Enabled=false;
				checkDcodes.Enabled=false;
				checkAutocodes.Enabled=false;
				checkProcButtons.Enabled=false;
				checkApptProcsQuickAdd.Enabled=false;
				checkRecallTypes.Enabled=false;
			#endif
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				//Tcodes remain enabled
				//Ncodes remain enabled
				checkDcodes.Text="CDA codes - Add any missing 2014 CDA codes.  This option does not work in the trial version.";
				checkProcButtons.Enabled=false;
				checkApptProcsQuickAdd.Enabled=false;
				checkRecallTypes.Enabled=false;
				checkRecallTypes.Text="Recall Types - Resets the recall types and triggers to default.  Replaces any T codes with CDA codes.";
				_codeList=null;//Is only filled when the code tool runs because the user might not need to download the codes.
			}
			else { //USA
				_codeList=CDT.Class1.GetADAcodes();
				//If this is not the full USA release version, then disable the D-code import because the CDT codes will not be available.
				if(_codeList==null || _codeList.Count==0) {
					checkDcodes.Checked=false;
					checkDcodes.Enabled=false;
				}
			}
		}

		private void butUncheck_Click(object sender,EventArgs e) {
			checkTcodes.Checked=false;
			checkNcodes.Checked=false;
			checkDcodes.Checked=false;
			checkAutocodes.Checked=false;
			checkProcButtons.Checked=false;
			checkApptProcsQuickAdd.Checked=false;
			checkRecallTypes.Checked=false;
		}

		///<summary>Downloads Canadian procedure codes from our website and updates _codeList accordingly.</summary>
		private void CanadaDownloadProcedureCodes() {
			Cursor=Cursors.WaitCursor;
			_codeList=new List<ProcedureCode>();
			string url=@"http://www.opendental.com/feescanada/procedurecodes.txt";
			string tempFile=PrefC.GetRandomTempFile(".tmp");
			WebClient myWebClient=new WebClient();
			try {
				myWebClient.DownloadFile(url,tempFile);
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"Failed to download procedure codes")+":\r\n"+ex.Message);
				Cursor=Cursors.Default;
				return;
			}
			string codeData=File.ReadAllText(tempFile);
			File.Delete(tempFile);
			string[] codeLines=codeData.Split('\n');
			for(int i=0;i<codeLines.Length;i++) {
				string[] fields=codeLines[i].Split('\t');
				if(fields.Length<1) {//Skip blank lines if they exist.
					continue;
				}
				ProcedureCode procCode=new ProcedureCode();
				procCode.ProcCode=PIn.String(fields[0]);//0 ProcCode
				procCode.Descript=PIn.String(fields[1]);//1 Description
				procCode.TreatArea=(TreatmentArea)PIn.Int(fields[2]);//2 TreatArea
				procCode.NoBillIns=PIn.Bool(fields[3]);//3 NoBillIns
				procCode.IsProsth=PIn.Bool(fields[4]);//4 IsProsth
				procCode.IsHygiene=PIn.Bool(fields[5]);//5 IsHygiene
				procCode.PaintType=(ToothPaintingType)PIn.Int(fields[6]);//6 PaintType
				procCode.ProcCatDescript=PIn.String(fields[7]);//7 ProcCatDescript
				procCode.ProcTime=PIn.String(fields[8]);//8 ProcTime
				procCode.AbbrDesc=PIn.String(fields[9]);//9 AbbrDesc
				_codeList.Add(procCode);
			}
			Cursor=Cursors.Default;
		}

		private void butRun_Click(object sender,EventArgs e) {
			if(!checkTcodes.Checked && !checkNcodes.Checked && !checkDcodes.Checked && !checkAutocodes.Checked 
				&& !checkProcButtons.Checked && !checkApptProcsQuickAdd.Checked && !checkRecallTypes.Checked)
			{
				MsgBox.Show(this,"Please select at least one tool first.");
				return;
			}
			Changed=false;
			int rowsInserted=0;
			#region N Codes
			if(checkNcodes.Checked) {
				try {
					rowsInserted+=FormProcCodes.ImportProcCodes("",null,Properties.Resources.NoFeeProcCodes);
				}
				catch(ApplicationException ex) {
					MessageBox.Show(ex.Message);
				}
				Changed=true;
				DataValid.SetInvalid(InvalidType.Defs, InvalidType.ProcCodes, InvalidType.Fees);
				//fees are included because they are grouped by defs.
			}
			#endregion
			#region D Codes
			if(checkDcodes.Checked) {
				try {
					if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
						if(_codeList==null) {
							CanadaDownloadProcedureCodes();
						}
					}
					rowsInserted+=FormProcCodes.ImportProcCodes("",_codeList,"");
					Changed=true;
					int descriptionsFixed=ProcedureCodes.ResetADAdescriptions();
					MessageBox.Show("Procedure code descriptions updated: "+descriptionsFixed.ToString());
				}
				catch(ApplicationException ex) {
					MessageBox.Show(ex.Message);
				}
				DataValid.SetInvalid(InvalidType.Defs, InvalidType.ProcCodes, InvalidType.Fees);
			}
			#endregion
			if(checkNcodes.Checked || checkDcodes.Checked){
				MessageBox.Show("Procedure codes inserted: "+rowsInserted);
			}
			#region Auto Codes
			if(checkAutocodes.Checked) {
				//checking for any AutoCodes and prompting the user if they exist
				if(AutoCodes.GetCount() > 0) {
					string msgText=Lan.g(this,"This tool will delete all current autocodes and then add in the default autocodes.")+"\r\n";
					//If the proc tool isn't going to put the procedure buttons back to default, warn them that they will need to reassociate them.
					if(!checkProcButtons.Checked) {
						msgText+=Lan.g(this,"Any procedure buttons associated with the current autocodes will be dissociated and will need to be reassociated manually.")+"\r\n";
					}
					msgText+=Lan.g(this,"Continue?");
					if(MsgBox.Show(this,MsgBoxButtons.YesNo,msgText)) {
						AutoCodes.SetToDefault();
						Changed=true;
						DataValid.SetInvalid(InvalidType.AutoCodes);
					}
					else {
						checkAutocodes.Checked=false; //if the user hits no on the popup, uncheck and continue 
					}
				}
				//If there are no autocodes then add the defaults
				else {
					AutoCodes.SetToDefault();
					Changed=true;
					DataValid.SetInvalid(InvalidType.AutoCodes);
				}
			}
			#endregion
			#region Proc Buttons
			if(checkProcButtons.Checked) {
				//checking for any custom proc button categories and prompting the user if they exist
				if(Defs.HasCustomCategories()) {
					if(MsgBox.Show(this,MsgBoxButtons.YesNo,"This tool will delete all current ProcButtons from the Chart Module and add in the defaults. Continue?")) {
						ProcButtons.SetToDefault();
						Changed=true;
						DataValid.SetInvalid(InvalidType.ProcButtons,InvalidType.Defs);
					}
					else {
						checkProcButtons.Checked=false;//continue and uncheck if user hits no on the popup
					}
				}
				//no ProcButtons found, run normally
				else {
					ProcButtons.SetToDefault();
					Changed=true;
					DataValid.SetInvalid(InvalidType.ProcButtons,InvalidType.Defs);
				}
			}
			#endregion
			#region Appt Procs Quick Add
			if(checkApptProcsQuickAdd.Checked) {
				//checking for any ApptProcsQuickAdd and prompting the user if they exist
				if(Defs.GetDefsForCategory(DefCat.ApptProcsQuickAdd).Count>0) {
					if(MsgBox.Show(this,MsgBoxButtons.YesNo,"This tool will reset the list of procedures in the appointment edit window to the defaults. Continue?")) {
						ProcedureCodes.ResetApptProcsQuickAdd();
						Changed=true;
						DataValid.SetInvalid(InvalidType.Defs);
					}
					else {
						checkApptProcsQuickAdd.Checked=false;//uncheck and continue if no is selected on the popup
					}
				}
				//run normally if no customizations are found
				else {
					ProcedureCodes.ResetApptProcsQuickAdd();
					Changed=true;
					DataValid.SetInvalid(InvalidType.Defs);
				}
			}
			#endregion
			#region Recall Types
			if(checkRecallTypes.Checked 
				&& (!RecallTypes.IsUsingManuallyAddedTypes() //If they have any manually added types, ask them if they are sure they want to delete them.
				|| MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will delete all patient recalls for recall types which were manually added.  Continue?")))
			{
				RecallTypes.SetToDefault();
				Changed=true;
				DataValid.SetInvalid(InvalidType.RecallTypes,InvalidType.Prefs);				
				SecurityLogs.MakeLogEntry(Permissions.RecallEdit,0,"Recall types set to default.");
			}
			#endregion
			#region T Codes
			if(checkTcodes.Checked){//Even though this is first in the interface, we need to run it last, since other regions need the T codes above.
				ProcedureCodes.TcodesClear();
				Changed=true;
				//yes, this really does refresh before moving on.
				DataValid.SetInvalid(InvalidType.Defs, InvalidType.ProcCodes, InvalidType.Fees);
				SecurityLogs.MakeLogEntry(Permissions.ProcCodeEdit,0,"T-Codes deleted.");
			}
			#endregion
			if(Changed) {
				MessageBox.Show(Lan.g(this,"Done."));
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"New Customer Procedure codes tool was run.");
			}
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

	

	

		

		

		

		


	}
}





















