using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Linq;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
///<summary></summary>
	public class FormRxSelect : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label labelInstructions;
		private OpenDental.UI.Button butBlank;
		private System.ComponentModel.Container components = null;// Required designer variable.
		private Patient PatCur;
		private OpenDental.UI.ODGrid gridMain;
		private GroupBox groupBox1;
		private Label label2;
		private Label label1;
		private CheckBox checkControlledOnly;
		private TextBox textDisp;
		private TextBox textDrug;
		private UI.Button butRefresh;
		///<summary>A list of all RxDefs.  Filled on load and on butRefresh click.</summary>
		private RxDef[] _arrayRxDefs;

		/// <summary>This is set for any medical orders that are selected.</summary>
		public long _medOrderNum;

		///<summary></summary>
		public FormRxSelect(Patient patCur){
			InitializeComponent();// Required for Windows Form Designer support
			PatCur=patCur;
			Lan.F(this);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRxSelect));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.labelInstructions = new System.Windows.Forms.Label();
			this.butBlank = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.butRefresh = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.checkControlledOnly = new System.Windows.Forms.CheckBox();
			this.textDisp = new System.Windows.Forms.TextBox();
			this.textDrug = new System.Windows.Forms.TextBox();
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
			this.butCancel.Location = new System.Drawing.Point(848, 636);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 3;
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
			this.butOK.Location = new System.Drawing.Point(756, 636);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 2;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// labelInstructions
			// 
			this.labelInstructions.Location = new System.Drawing.Point(9, 643);
			this.labelInstructions.Name = "labelInstructions";
			this.labelInstructions.Size = new System.Drawing.Size(470, 16);
			this.labelInstructions.TabIndex = 15;
			this.labelInstructions.Text = "Please select a Prescription from the list or click Blank to start with a blank p" +
    "rescription.";
			// 
			// butBlank
			// 
			this.butBlank.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBlank.Autosize = true;
			this.butBlank.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBlank.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBlank.CornerRadius = 4F;
			this.butBlank.Location = new System.Drawing.Point(436, 636);
			this.butBlank.Name = "butBlank";
			this.butBlank.Size = new System.Drawing.Size(75, 26);
			this.butBlank.TabIndex = 0;
			this.butBlank.Text = "&Blank";
			this.butBlank.Click += new System.EventHandler(this.butBlank_Click);
			// 
			// gridMain
			// 
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(738, 611);
			this.gridMain.TabIndex = 16;
			this.gridMain.Title = "Prescriptions";
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableRxSetup";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.butRefresh);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.checkControlledOnly);
			this.groupBox1.Controls.Add(this.textDisp);
			this.groupBox1.Controls.Add(this.textDrug);
			this.groupBox1.Location = new System.Drawing.Point(756, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(174, 173);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Search";
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(93, 140);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 26);
			this.butRefresh.TabIndex = 3;
			this.butRefresh.Text = "Search";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 67);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "Disp";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Drug";
			// 
			// checkControlledOnly
			// 
			this.checkControlledOnly.Location = new System.Drawing.Point(11, 110);
			this.checkControlledOnly.Name = "checkControlledOnly";
			this.checkControlledOnly.Size = new System.Drawing.Size(157, 24);
			this.checkControlledOnly.TabIndex = 2;
			this.checkControlledOnly.Text = "Controlled Only";
			this.checkControlledOnly.UseVisualStyleBackColor = true;
			// 
			// textDisp
			// 
			this.textDisp.Location = new System.Drawing.Point(8, 84);
			this.textDisp.Name = "textDisp";
			this.textDisp.Size = new System.Drawing.Size(160, 20);
			this.textDisp.TabIndex = 1;
			this.textDisp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textDisp_KeyDown);
			// 
			// textDrug
			// 
			this.textDrug.Location = new System.Drawing.Point(7, 42);
			this.textDrug.Name = "textDrug";
			this.textDrug.Size = new System.Drawing.Size(160, 20);
			this.textDrug.TabIndex = 0;
			this.textDrug.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textDrug_KeyDown);
			// 
			// FormRxSelect
			// 
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(942, 674);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butBlank);
			this.Controls.Add(this.labelInstructions);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormRxSelect";
			this.ShowInTaskbar = false;
			this.Text = "Select Prescription";
			this.Load += new System.EventHandler(this.FormRxSelect_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormRxSelect_Load(object sender, System.EventArgs e) {
			_arrayRxDefs=RxDefs.Refresh();
			FillGrid();
			if(PrefC.GetBool(PrefName.ShowFeatureEhr)) {
				//We cannot allow blank prescription when using EHR, because each prescription created in this window must have an RxCui.
				//If we allowed blank, we would not know where to pull the RxCui from.
				butBlank.Visible=false;
				labelInstructions.Text=Lan.g(this,"Please select a Prescription from the list.");
			}
		}

		private void FillGrid() {
			RxDef[] arrayRxDefs=_arrayRxDefs;
			if(textDrug.Text!="") {
				string[] arraySearchTerms=textDrug.Text.Split(' ');
				foreach(string searchTerm in arraySearchTerms) {
					arrayRxDefs=arrayRxDefs.Where(x => x.Drug.ToLower().Contains(searchTerm.ToLower())).ToArray();
				}
			}
			if(textDisp.Text!="") {
				string[] arraySearchTerms=textDisp.Text.Split(' ');
				foreach(string searchTerm in arraySearchTerms) {
					arrayRxDefs=arrayRxDefs.Where(x => x.Disp.ToLower().Contains(searchTerm.ToLower())).ToArray();
				}
			}
			if(checkControlledOnly.Checked) {
				arrayRxDefs=arrayRxDefs.Where(x => x.IsControlled).ToArray();
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableRxSetup","Drug"),140);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRxSetup","Controlled"),70,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRxSetup","Sig"),250);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRxSetup","Disp"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRxSetup","Refills"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRxSetup","Notes"),300);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<arrayRxDefs.Length;i++) {
				row=new ODGridRow();
				row.Cells.Add(arrayRxDefs[i].Drug);
				if(arrayRxDefs[i].IsControlled){
					row.Cells.Add("X");
				}
				else{
					row.Cells.Add("");
				}
				row.Cells.Add(arrayRxDefs[i].Sig);
				row.Cells.Add(arrayRxDefs[i].Disp);
				row.Cells.Add(arrayRxDefs[i].Refills);
				row.Cells.Add(arrayRxDefs[i].Notes);
				gridMain.Rows.Add(row);
				row.Tag=arrayRxDefs[i];
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,OpenDental.UI.ODGridClickEventArgs e) {
			RxSelected();
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			_arrayRxDefs=RxDefs.Refresh();
			FillGrid();
		}

		private void textDrug_KeyDown(object sender,KeyEventArgs e) {
			if(e.KeyCode!=Keys.Enter) {
				return;
			}
			e.Handled=true;
			e.SuppressKeyPress=true;
			FillGrid();
		}

		private void textDisp_KeyDown(object sender,KeyEventArgs e) {
			if(e.KeyCode!=Keys.Enter) {
				return;
			}
			e.Handled=true;
			e.SuppressKeyPress=true;
			FillGrid();
		}

		private void RxSelected(){
			if(gridMain.GetSelectedIndex()==-1) {
				//this should never happen
				return;
			}
			RxDef RxDefCur=(RxDef)gridMain.Rows[gridMain.GetSelectedIndex()].Tag;
			if(PrefC.GetBool(PrefName.ShowFeatureEhr) && RxDefCur.RxCui==0) {
				string strMsgText=Lan.g(this,"The selected prescription is missing an RxNorm")+".\r\n"
					+Lan.g(this,"Prescriptions without RxNorms cannot be exported in EHR documents")+".\r\n";
				if(!Security.IsAuthorized(Permissions.RxEdit,true)) {
					//Show the message but don't allow to edit. Continue creating rx
					MessageBox.Show(strMsgText);
				}
				else if(MessageBox.Show(strMsgText+Lan.g(this,"Edit RxNorm in Rx Template?"),"",MessageBoxButtons.YesNo)==DialogResult.Yes) {
					FormRxDefEdit form=new FormRxDefEdit(RxDefCur);
					form.ShowDialog();
					RxDefCur=RxDefs.GetOne(RxDefCur.RxDefNum);//FormRxDefEdit does not modify the RxDefCur object, so we must get the updated RxCui from the db.
				}
			}
			//Alert
			if(!RxAlertL.DisplayAlerts(PatCur.PatNum,RxDefCur.RxDefNum)){
				return;
			}
			//User OK with alert
			RxPat RxPatCur=new RxPat();
			RxPatCur.RxDate=DateTime.Today;
			RxPatCur.PatNum=PatCur.PatNum;
			RxPatCur.Drug=RxDefCur.Drug;
			RxPatCur.IsControlled=RxDefCur.IsControlled;
			if(PrefC.GetBool(PrefName.RxHasProc) && (Clinics.ClinicNum==0 || Clinics.GetClinic(Clinics.ClinicNum).HasProcOnRx)) {
				RxPatCur.IsProcRequired=RxDefCur.IsProcRequired;
			}
			RxPatCur.Sig=RxDefCur.Sig;
			RxPatCur.Disp=RxDefCur.Disp;
			RxPatCur.Refills=RxDefCur.Refills;
			if(PrefC.GetBool(PrefName.RxSendNewToQueue)) {
				RxPatCur.SendStatus=RxSendStatus.InElectQueue;
			}
			else {
				RxPatCur.SendStatus=RxSendStatus.Unsent;
			}
			//Notes not copied: we don't want these kinds of notes cluttering things
			FormRxEdit FormE=new FormRxEdit(PatCur,RxPatCur);
			FormE.IsNew=true;
			FormE.ShowDialog();
			if(FormE.DialogResult!=DialogResult.OK){
				return;
			}
			bool isProvOrder=false;
			if(Security.CurUser.ProvNum!=0) {//The user who is currently logged in is a provider.
				isProvOrder=true;
			}
			_medOrderNum=MedicationPats.InsertOrUpdateMedOrderForRx(RxPatCur,RxDefCur.RxCui,isProvOrder);//RxDefCur.RxCui can be 0.
			EhrMeasureEvent newMeasureEvent=new EhrMeasureEvent();
			newMeasureEvent.DateTEvent=DateTime.Now;
			newMeasureEvent.EventType=EhrMeasureEventType.CPOE_MedOrdered;
			newMeasureEvent.PatNum=PatCur.PatNum;
			newMeasureEvent.MoreInfo="";
			newMeasureEvent.FKey=_medOrderNum;
			EhrMeasureEvents.Insert(newMeasureEvent);
			DialogResult=DialogResult.OK;
		}

		private void butBlank_Click(object sender, System.EventArgs e) {
			RxPat RxPatCur=new RxPat();
			RxPatCur.RxDate=DateTime.Today;
			RxPatCur.PatNum=PatCur.PatNum;
			if(PrefC.GetBool(PrefName.RxSendNewToQueue)) {
				RxPatCur.SendStatus=RxSendStatus.InElectQueue;
			}
			else {
				RxPatCur.SendStatus=RxSendStatus.Unsent;
			}
			FormRxEdit FormE=new FormRxEdit(PatCur,RxPatCur);
			FormE.IsNew=true;
			FormE.ShowDialog();
			if(FormE.DialogResult!=DialogResult.OK){
				return;
			}
			//We do not need to make a medical order here, because butBlank is not visible in EHR mode.
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1){
				if(PrefC.GetBool(PrefName.ShowFeatureEhr)) {
					MsgBox.Show(this,"Please select Rx first.");
				}
				else {
					MsgBox.Show(this,"Please select Rx first or click Blank");
				}
				return;
			}
			RxSelected();
		}

	}
}
