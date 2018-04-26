using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	///<summary>Form used to manage kiosk terminals and load/clear patient forms displayed on the kiosks.</summary>
	public class FormTerminalManager:ODForm {
		private IContainer components=null;
		private OpenDental.UI.ODGrid gridMain;
		private Label label1;
		private Label label2;
		private TextBox textPassword;
		private OpenDental.UI.Button butSave;
		private OpenDental.UI.Button butLoad;
		private GroupBox groupBox1;
		private ListBox listSheets;
		private Label labelSheets;
		private Label labelPatient;
		private GroupBox groupBox2;
		private OpenDental.UI.Button butClear;
		private OpenDental.UI.Button butDelete;
		private OpenDental.UI.Button butClose;
		///<summary>Local cache of all active kiosks.  Used to fill the grid and display kiosk computer and client names as well as currently loaded patients.</summary>
		private List<TerminalActive> _terminalList;

		///<summary>Used to keep track of the currently loaded patient so the form doesn't call Patients.GetLim to fill the patient label every time the
		///form processes a signal if the FormOpenDental.CurPatNum is the same as the currently loaded patient.</summary>
		private long _patNumCur;

		///<summary></summary>
		public FormTerminalManager() {
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

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTerminalManager));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textPassword = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.labelSheets = new System.Windows.Forms.Label();
			this.labelPatient = new System.Windows.Forms.Label();
			this.listSheets = new System.Windows.Forms.ListBox();
			this.butLoad = new OpenDental.UI.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.butSave = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butClear = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(18, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(437, 49);
			this.label1.TabIndex = 3;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(7, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(327, 31);
			this.label2.TabIndex = 4;
			this.label2.Text = "To close a kiosk, go to that computer and click the hidden button at the lower ri" +
    "ght.  You will need to enter this password:";
			// 
			// textPassword
			// 
			this.textPassword.Location = new System.Drawing.Point(10, 50);
			this.textPassword.Name = "textPassword";
			this.textPassword.Size = new System.Drawing.Size(129, 20);
			this.textPassword.TabIndex = 5;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.labelSheets);
			this.groupBox1.Controls.Add(this.labelPatient);
			this.groupBox1.Controls.Add(this.listSheets);
			this.groupBox1.Controls.Add(this.butLoad);
			this.groupBox1.Location = new System.Drawing.Point(475, 64);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(168, 249);
			this.groupBox1.TabIndex = 11;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Current Patient";
			// 
			// labelSheets
			// 
			this.labelSheets.Location = new System.Drawing.Point(11, 41);
			this.labelSheets.Name = "labelSheets";
			this.labelSheets.Size = new System.Drawing.Size(123, 17);
			this.labelSheets.TabIndex = 10;
			this.labelSheets.Text = "Forms for Kiosk";
			this.labelSheets.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelPatient
			// 
			this.labelPatient.Location = new System.Drawing.Point(11, 19);
			this.labelPatient.Name = "labelPatient";
			this.labelPatient.Size = new System.Drawing.Size(147, 18);
			this.labelPatient.TabIndex = 9;
			this.labelPatient.Text = "Fname Lname";
			// 
			// listSheets
			// 
			this.listSheets.FormattingEnabled = true;
			this.listSheets.Location = new System.Drawing.Point(11, 62);
			this.listSheets.Name = "listSheets";
			this.listSheets.Size = new System.Drawing.Size(146, 147);
			this.listSheets.TabIndex = 8;
			// 
			// butLoad
			// 
			this.butLoad.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butLoad.Autosize = true;
			this.butLoad.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLoad.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLoad.CornerRadius = 4F;
			this.butLoad.Location = new System.Drawing.Point(11, 215);
			this.butLoad.Name = "butLoad";
			this.butLoad.Size = new System.Drawing.Size(84, 24);
			this.butLoad.TabIndex = 7;
			this.butLoad.Text = "Load Patient";
			this.butLoad.UseVisualStyleBackColor = true;
			this.butLoad.Click += new System.EventHandler(this.butLoad_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.textPassword);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.butSave);
			this.groupBox2.Location = new System.Drawing.Point(21, 318);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(344, 80);
			this.groupBox2.TabIndex = 12;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Password";
			// 
			// butSave
			// 
			this.butSave.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSave.Autosize = true;
			this.butSave.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSave.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSave.CornerRadius = 4F;
			this.butSave.Location = new System.Drawing.Point(145, 48);
			this.butSave.Name = "butSave";
			this.butSave.Size = new System.Drawing.Size(97, 24);
			this.butSave.TabIndex = 6;
			this.butSave.Text = "Save Password";
			this.butSave.UseVisualStyleBackColor = true;
			this.butSave.Click += new System.EventHandler(this.butSave_Click);
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(21, 279);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(84, 24);
			this.butDelete.TabIndex = 14;
			this.butDelete.Text = "Delete";
			this.butDelete.UseVisualStyleBackColor = true;
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// gridMain
			// 
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(21, 67);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(424, 206);
			this.gridMain.TabIndex = 2;
			this.gridMain.Title = "Active Kiosks";
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableTerminals";
			// 
			// butClear
			// 
			this.butClear.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClear.Autosize = true;
			this.butClear.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClear.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClear.CornerRadius = 4F;
			this.butClear.Location = new System.Drawing.Point(361, 279);
			this.butClear.Name = "butClear";
			this.butClear.Size = new System.Drawing.Size(84, 24);
			this.butClear.TabIndex = 13;
			this.butClear.Text = "Clear Patient";
			this.butClear.UseVisualStyleBackColor = true;
			this.butClear.Click += new System.EventHandler(this.butClear_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(578, 374);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 15;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// FormTerminalManager
			// 
			this.ClientSize = new System.Drawing.Size(665, 410);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butClear);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormTerminalManager";
			this.Text = "Kiosk Manager";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTerminalManager_FormClosing);
			this.Load += new System.EventHandler(this.FormTerminalManager_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormTerminalManager_Load(object sender,EventArgs e) {
			PatientChangedEvent.Fired+=PatientChangedEvent_Fired;
			textPassword.Text=PrefC.GetString(PrefName.TerminalClosePassword);
			FillGrid();
			FillPat();
		}

		public void PatientChangedEvent_Fired(ODEventArgs e) {
			if(e.Name!="FormOpenDental" || e.Tag.GetType()!=typeof(long) || this.IsDisposed) {
				return;
			}
			FillPat();
		}

		public override void OnProcessSignals(List<Signalod> listSignals) {
			if(listSignals.Any(x => x.IType==InvalidType.Prefs)) {
				textPassword.Text=PrefC.GetString(PrefName.TerminalClosePassword);
			}
			int processIdCur=Process.GetCurrentProcess().Id;
			if(listSignals.All(x => x.IType!=InvalidType.Kiosk || (x.FKeyType==KeyType.ProcessId && x.FKey==processIdCur))) {
				return;
			}
			FillGrid();
			FillPat();//fill patient, to refill sheets in case there are new ones added.  Pat name label will not be updated from db unless pat changes.
		}

		private void FillGrid(){
			long selectedTermNum=-1;
			if(gridMain.GetSelectedIndex()>-1) {
				selectedTermNum=(long)gridMain.Rows[gridMain.GetSelectedIndex()].Tag;//set selectedTermNum before refreshing the _terminalList
			}
			try{
				_terminalList=TerminalActives.Refresh();
			}
			catch(Exception) {//SocketException if db connection gets lost.
				return;
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableTerminals","Computer Name"),110));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableTerminals","Session Name"),110));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableTerminals","Patient"),185));
			gridMain.Rows.Clear();
			ODGridRow row;
			int selectedIndex=-1;
			foreach(TerminalActive termCur in _terminalList) {
				row=new ODGridRow();
				row.Cells.Add(termCur.ComputerName);
				row.Cells.Add(termCur.SessionName);
				row.Cells.Add(termCur.PatNum>0?Patients.GetLim(termCur.PatNum).GetNameLF():"");
				row.Tag=termCur.TerminalActiveNum;
				gridMain.Rows.Add(row);
				if(termCur.TerminalActiveNum==selectedTermNum) {
					selectedIndex=gridMain.Rows.Count-1;
				}
			}
			gridMain.EndUpdate();
			gridMain.SetSelected(Math.Max(selectedIndex,0),true);//selectedIndex could be -1 if the selected term is not in the list, default to row 0
		}

		///<summary>Refills form based on FormOpenDental.CurPatNum</summary>
		private void FillPat() {
			if(FormOpenDental.CurPatNum==0) {
				_patNumCur=0;
				labelPatient.Text=Lan.g(this,"none");
				labelSheets.Visible=false;
				listSheets.Visible=false;
				butLoad.Visible=false;
			}
			else {
				if(FormOpenDental.CurPatNum!=_patNumCur) {//if pat changed or on load, get the patient name from db
					_patNumCur=FormOpenDental.CurPatNum;
					labelPatient.Text=Patients.GetLim(_patNumCur).GetNameFL();
				}
				labelSheets.Visible=true;
				listSheets.Visible=true;
				butLoad.Visible=true;
				listSheets.Items.Clear();
				//refresh the sheet list for the current patient even if the patient loaded has not changed, in case they've added some sheets in OD
				Sheets.GetForTerminal(_patNumCur).ForEach(x => listSheets.Items.Add(x.Description));
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1 || gridMain.GetSelectedIndex()>=_terminalList.Count) {
				MsgBox.Show(this,"Please select a terminal first.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"A terminal row should not be deleted unless it is showing erroneously and there really is no "
				+"terminal running on the computer shown.  Continue anyway?"))
			{
				return;
			}
			TerminalActive selectedTerm=_terminalList[gridMain.GetSelectedIndex()];
			TerminalActives.DeleteForCmptrSessionAndId(selectedTerm.ComputerName,selectedTerm.SessionId,processId:selectedTerm.ProcessId);
			Signalods.SetInvalid(InvalidType.Kiosk,KeyType.ProcessId,Process.GetCurrentProcess().Id);
			FillGrid();
			FillPat();
		}

		private void butClear_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1 || gridMain.GetSelectedIndex()>=_terminalList.Count) {
				MsgBox.Show(this,"Please select a terminal first.");
				return;
			}
			TerminalActive terminal=_terminalList[gridMain.GetSelectedIndex()];
			if(terminal.PatNum==0) {
				return;
			}
			if(!MsgBox.Show(this,true,"A patient is currently using the terminal.  If you continue, they will lose the information that is on their screen.  "
				+"Continue anyway?"))
			{
				return;
			}
			TerminalActives.SetPatNum(terminal.TerminalActiveNum,0);
			Signalods.SetInvalid(InvalidType.Kiosk,KeyType.ProcessId,Process.GetCurrentProcess().Id);
			FillGrid();
			FillPat();
		}

		private void butLoad_Click(object sender,EventArgs e) {
			if(listSheets.Items.Count==0) {
				MsgBox.Show(this,"There are no sheets to send to the kiosk for the current patient.");
				return;
			}
			if(gridMain.GetSelectedIndex()==-1 || gridMain.GetSelectedIndex()>=_terminalList.Count) {
				MsgBox.Show(this,"Please select a terminal first.");
				return;
			}
			TerminalActive terminal=_terminalList[gridMain.GetSelectedIndex()];
			if(terminal.PatNum!=0
				&& !MsgBox.Show(this,true,"A patient is currently using the terminal.  If you continue, they will lose the information that is on their "
					+"screen.  Continue anyway?"))
			{
				return;
			}
			TerminalActives.SetPatNum(terminal.TerminalActiveNum,_patNumCur);
			Signalods.SetInvalid(InvalidType.Kiosk,KeyType.ProcessId,Process.GetCurrentProcess().Id);
			FillGrid();
			FillPat();
		}

		private void butSave_Click(object sender,EventArgs e) {
			if(Prefs.UpdateString(PrefName.TerminalClosePassword,textPassword.Text)){
				Signalods.SetInvalid(InvalidType.Prefs);
			}
			MsgBox.Show(this,"Done.");
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

		private void FormTerminalManager_FormClosing(object sender,FormClosingEventArgs e) {
			PatientChangedEvent.Fired-=PatientChangedEvent_Fired;
			if(Prefs.UpdateString(PrefName.TerminalClosePassword,textPassword.Text)){
				Signalods.SetInvalid(InvalidType.Prefs);
			}
		}

	}
}




