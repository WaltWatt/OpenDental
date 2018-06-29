using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Data;
using CodeBase;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormAudit : ODForm {
		///<summary>The selected patNum.  Can be 0 to include all.</summary>
		private long PatNum;
		///<summary>This gets set externally beforehand.  Lets user quickly select audit trail for current patient.</summary>
		public long CurPatNum;
		private PrintDocument pd;
		///<summary>This alphabetizes the permissions, except for "none", which is always first.  If using a foreign language, the order will be according to the English version, not the foreign translated text.</summary>
		private List<string> permissionsAlphabetic;
		private bool headingPrinted;
		private int pagesPrinted;
		private int headingPrintH;
		private ValidDate textDateEditedFrom;
		private ValidDate textDateEditedTo;
		private Label label7;
		private Label label8;
		private ValidNum textRows;
		private Label label6;
		private UI.Button butPrint;
		private UI.Button butCurrent;
		private UI.Button butAll;
		private UI.Button butFind;
		private TextBox textPatient;
		private Label label5;
		private ComboBox comboUser;
		private Label label4;
		private Label label1;
		private ComboBox comboPermission;
		private UI.Button butRefresh;
		private ValidDate textDateFrom;
		private ValidDate textDateTo;
		private Label label2;
		private Label label3;
		private ODGrid grid;
		private CheckBox checkIncludeArchived;
		private List<Userod> _listUserods;

		///<summary></summary>
		public FormAudit()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			Permissions[] permArray=(Permissions[])Enum.GetValues(typeof(Permissions));
			permissionsAlphabetic=new List<string>();
			for(int i=1;i<permArray.Length;i++){
				if(GroupPermissions.HasAuditTrail(permArray[i])) {
					permissionsAlphabetic.Add(permArray[i].ToString());
				}
			}
			permissionsAlphabetic.Sort();
			permissionsAlphabetic.Insert(0,Permissions.None.ToString());
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAudit));
			this.textDateEditedFrom = new OpenDental.ValidDate();
			this.textDateEditedTo = new OpenDental.ValidDate();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.textRows = new OpenDental.ValidNum();
			this.label6 = new System.Windows.Forms.Label();
			this.butPrint = new OpenDental.UI.Button();
			this.butCurrent = new OpenDental.UI.Button();
			this.butAll = new OpenDental.UI.Button();
			this.butFind = new OpenDental.UI.Button();
			this.textPatient = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.comboUser = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.comboPermission = new System.Windows.Forms.ComboBox();
			this.butRefresh = new OpenDental.UI.Button();
			this.textDateFrom = new OpenDental.ValidDate();
			this.textDateTo = new OpenDental.ValidDate();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.grid = new OpenDental.UI.ODGrid();
			this.checkIncludeArchived = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// textDateEditedFrom
			// 
			this.textDateEditedFrom.Location = new System.Drawing.Point(934, 4);
			this.textDateEditedFrom.Name = "textDateEditedFrom";
			this.textDateEditedFrom.Size = new System.Drawing.Size(80, 20);
			this.textDateEditedFrom.TabIndex = 276;
			// 
			// textDateEditedTo
			// 
			this.textDateEditedTo.Location = new System.Drawing.Point(934, 27);
			this.textDateEditedTo.Name = "textDateEditedTo";
			this.textDateEditedTo.Size = new System.Drawing.Size(80, 20);
			this.textDateEditedTo.TabIndex = 272;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(814, 5);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(118, 19);
			this.label7.TabIndex = 273;
			this.label7.Text = "Previous From Date";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(868, 30);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(64, 13);
			this.label8.TabIndex = 274;
			this.label8.Text = "To Date";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textRows
			// 
			this.textRows.Location = new System.Drawing.Point(771, 26);
			this.textRows.MaxLength = 5;
			this.textRows.MaxVal = 99999;
			this.textRows.MinVal = 1;
			this.textRows.Name = "textRows";
			this.textRows.Size = new System.Drawing.Size(56, 20);
			this.textRows.TabIndex = 268;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(685, 28);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(86, 17);
			this.label6.TabIndex = 61;
			this.label6.Text = "Limit Rows:";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrint;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(1019, 27);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(82, 24);
			this.butPrint.TabIndex = 60;
			this.butPrint.Text = "Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// butCurrent
			// 
			this.butCurrent.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCurrent.Autosize = true;
			this.butCurrent.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCurrent.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCurrent.CornerRadius = 4F;
			this.butCurrent.Location = new System.Drawing.Point(473, 24);
			this.butCurrent.Name = "butCurrent";
			this.butCurrent.Size = new System.Drawing.Size(63, 24);
			this.butCurrent.TabIndex = 59;
			this.butCurrent.Text = "Current";
			this.butCurrent.Click += new System.EventHandler(this.butCurrent_Click);
			// 
			// butAll
			// 
			this.butAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAll.Autosize = true;
			this.butAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAll.CornerRadius = 4F;
			this.butAll.Location = new System.Drawing.Point(616, 24);
			this.butAll.Name = "butAll";
			this.butAll.Size = new System.Drawing.Size(63, 24);
			this.butAll.TabIndex = 58;
			this.butAll.Text = "All";
			this.butAll.Click += new System.EventHandler(this.butAll_Click);
			// 
			// butFind
			// 
			this.butFind.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butFind.Autosize = true;
			this.butFind.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butFind.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butFind.CornerRadius = 4F;
			this.butFind.Location = new System.Drawing.Point(545, 24);
			this.butFind.Name = "butFind";
			this.butFind.Size = new System.Drawing.Size(63, 24);
			this.butFind.TabIndex = 57;
			this.butFind.Text = "Find";
			this.butFind.Click += new System.EventHandler(this.butFind_Click);
			// 
			// textPatient
			// 
			this.textPatient.Location = new System.Drawing.Point(473, 4);
			this.textPatient.Name = "textPatient";
			this.textPatient.Size = new System.Drawing.Size(206, 20);
			this.textPatient.TabIndex = 56;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(158, 29);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(75, 13);
			this.label5.TabIndex = 55;
			this.label5.Text = "User";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboUser
			// 
			this.comboUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboUser.FormattingEnabled = true;
			this.comboUser.Location = new System.Drawing.Point(235, 27);
			this.comboUser.MaxDropDownItems = 40;
			this.comboUser.Name = "comboUser";
			this.comboUser.Size = new System.Drawing.Size(170, 21);
			this.comboUser.TabIndex = 54;
			this.comboUser.SelectionChangeCommitted += new System.EventHandler(this.comboUser_SelectionChangeCommitted);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(406, 8);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(65, 13);
			this.label4.TabIndex = 52;
			this.label4.Text = "Patient";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(158, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(75, 13);
			this.label1.TabIndex = 51;
			this.label1.Text = "Permission";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboPermission
			// 
			this.comboPermission.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboPermission.FormattingEnabled = true;
			this.comboPermission.Location = new System.Drawing.Point(235, 4);
			this.comboPermission.MaxDropDownItems = 40;
			this.comboPermission.Name = "comboPermission";
			this.comboPermission.Size = new System.Drawing.Size(170, 21);
			this.comboPermission.TabIndex = 50;
			this.comboPermission.SelectionChangeCommitted += new System.EventHandler(this.comboPermission_SelectionChangeCommitted);
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(1019, 1);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(82, 24);
			this.butRefresh.TabIndex = 49;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// textDateFrom
			// 
			this.textDateFrom.Location = new System.Drawing.Point(77, 4);
			this.textDateFrom.Name = "textDateFrom";
			this.textDateFrom.Size = new System.Drawing.Size(80, 20);
			this.textDateFrom.TabIndex = 47;
			// 
			// textDateTo
			// 
			this.textDateTo.Location = new System.Drawing.Point(77, 27);
			this.textDateTo.Name = "textDateTo";
			this.textDateTo.Size = new System.Drawing.Size(80, 20);
			this.textDateTo.TabIndex = 48;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(0, 7);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(75, 14);
			this.label2.TabIndex = 45;
			this.label2.Text = "From Date";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(11, 30);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 13);
			this.label3.TabIndex = 46;
			this.label3.Text = "To Date";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// grid
			// 
			this.grid.AllowSortingByColumn = true;
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grid.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.grid.HasAddButton = false;
			this.grid.HasDropDowns = false;
			this.grid.HasMultilineHeaders = false;
			this.grid.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.grid.HeaderHeight = 15;
			this.grid.HScrollVisible = false;
			this.grid.Location = new System.Drawing.Point(8, 54);
			this.grid.Name = "grid";
			this.grid.ScrollValue = 0;
			this.grid.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.grid.Size = new System.Drawing.Size(1093, 576);
			this.grid.TabIndex = 2;
			this.grid.Title = "Audit Trail";
			this.grid.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.grid.TitleHeight = 18;
			this.grid.TranslationName = "TableAudit";
			this.grid.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellDoubleClick);
			// 
			// checkIncludeArchived
			// 
			this.checkIncludeArchived.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeArchived.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkIncludeArchived.Location = new System.Drawing.Point(685, 5);
			this.checkIncludeArchived.Name = "checkIncludeArchived";
			this.checkIncludeArchived.Size = new System.Drawing.Size(100, 18);
			this.checkIncludeArchived.TabIndex = 278;
			this.checkIncludeArchived.Text = "Show Archived";
			this.checkIncludeArchived.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIncludeArchived.UseVisualStyleBackColor = true;
			this.checkIncludeArchived.CheckedChanged += new System.EventHandler(this.checkIncludeArchived_CheckedChanged);
			// 
			// FormAudit
			// 
			this.ClientSize = new System.Drawing.Size(1108, 634);
			this.Controls.Add(this.checkIncludeArchived);
			this.Controls.Add(this.grid);
			this.Controls.Add(this.textDateEditedFrom);
			this.Controls.Add(this.textDateEditedTo);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.textRows);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.butCurrent);
			this.Controls.Add(this.butAll);
			this.Controls.Add(this.butFind);
			this.Controls.Add(this.textPatient);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.comboUser);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboPermission);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.textDateFrom);
			this.Controls.Add(this.textDateTo);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(1124, 218);
			this.Name = "FormAudit";
			this.ShowInTaskbar = false;
			this.Text = "Audit Trail";
			this.Load += new System.EventHandler(this.FormAudit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormAudit_Load(object sender, System.EventArgs e) {
			textDateFrom.Text=DateTime.Today.AddDays(-10).ToShortDateString();
			textDateTo.Text=DateTime.Today.ToShortDateString();
			for(int i=0;i<permissionsAlphabetic.Count;i++){
				if(i==0){
					comboPermission.Items.Add(Lan.g(this,"All"));//None
				}
				else{
					comboPermission.Items.Add(Lan.g("enumPermissions",permissionsAlphabetic[i]));
				}
			}
			comboPermission.SelectedIndex=0;
			_listUserods=Userods.GetDeepCopy();
			comboUser.Items.Add(Lan.g(this,"All"));
			comboUser.SelectedIndex=0;
			for(int i=0;i<_listUserods.Count;i++){
				comboUser.Items.Add(_listUserods[i].UserName);
			}
			PatNum=CurPatNum;
			if(PatNum==0) {
				textPatient.Text="";
			}
			else {
				textPatient.Text=Patients.GetLim(PatNum).GetNameLF();
			}
			textRows.Text=PrefC.GetString(PrefName.AuditTrailEntriesDisplayed);
			FillGrid();
		}

		private void comboUser_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void comboPermission_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void butCurrent_Click(object sender,EventArgs e) {
			PatNum=CurPatNum;
			if(PatNum==0){
				textPatient.Text="";
			}
			else{
				textPatient.Text=Patients.GetLim(PatNum).GetNameLF();
			}
			FillGrid();
		}

		private void butFind_Click(object sender,EventArgs e) {
			FormPatientSelect FormP=new FormPatientSelect();
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK){
				return;
			}
			PatNum=FormP.SelectedPatNum;
			textPatient.Text=Patients.GetLim(PatNum).GetNameLF();
			FillGrid();
		}

		private void butAll_Click(object sender,EventArgs e) {
			PatNum=0;
			textPatient.Text="";
			FillGrid();
		}

		private void FillGrid() {
			if(textRows.errorProvider1.GetError(textRows)!="") {
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			long userNum=0;
			if(comboUser.SelectedIndex>0) {
				userNum=_listUserods[comboUser.SelectedIndex-1].UserNum;
			}
			SecurityLog[] logList=null;
			DateTime datePreviousFrom=PIn.Date(textDateEditedFrom.Text);
			DateTime datePreviousTo=DateTime.Today;
			if(textDateEditedTo.Text!="") { 
				datePreviousTo=PIn.Date(textDateEditedTo.Text);
			}
			try {
				if(comboPermission.SelectedIndex==0) {
					logList=SecurityLogs.Refresh(PIn.Date(textDateFrom.Text),PIn.Date(textDateTo.Text),
						Permissions.None,PatNum,userNum,datePreviousFrom,datePreviousTo,checkIncludeArchived.Checked,PIn.Int(textRows.Text));
				}
				else {
					logList=SecurityLogs.Refresh(PIn.Date(textDateFrom.Text),PIn.Date(textDateTo.Text),
						(Permissions)Enum.Parse(typeof(Permissions),comboPermission.SelectedItem.ToString()),PatNum,userNum,
						datePreviousFrom,datePreviousTo,checkIncludeArchived.Checked,PIn.Int(textRows.Text));
				}
			}
			catch(Exception ex) {
				FriendlyException.Show(Lan.g(this,"There was a problem refreshing the Audit Trail with the current filters."),ex);
				logList=new SecurityLog[0];
			}
			grid.BeginUpdate();
			grid.Columns.Clear();
			grid.Columns.Add(new ODGridColumn(Lan.g("TableAudit","Date"),70,GridSortingStrategy.DateParse));
			grid.Columns.Add(new ODGridColumn(Lan.g("TableAudit","Time"),60,GridSortingStrategy.DateParse));
			grid.Columns.Add(new ODGridColumn(Lan.g("TableAudit","Patient"),100));
			grid.Columns.Add(new ODGridColumn(Lan.g("TableAudit","User"),70));
			grid.Columns.Add(new ODGridColumn(Lan.g("TableAudit","Permission"),190));
			grid.Columns.Add(new ODGridColumn(Lan.g("TableAudit","Computer"),70));
			grid.Columns.Add(new ODGridColumn(Lan.g("TableAudit","Log Text"),279));
			grid.Columns.Add(new ODGridColumn(Lan.g("TableAudit","Last Edit"),100));
			grid.Rows.Clear();
			ODGridRow row;
			foreach(SecurityLog logCur in logList) {
				row=new ODGridRow();
				row.Cells.Add(logCur.LogDateTime.ToShortDateString());
				row.Cells.Add(logCur.LogDateTime.ToShortTimeString());
				row.Cells.Add(logCur.PatientName);
				//user might be null due to old bugs.
				row.Cells.Add(Userods.GetUser(logCur.UserNum)?.UserName??"unknown");
				if(logCur.PermType==Permissions.ChartModule) {
					row.Cells.Add("ChartModuleViewed");
				}
				else if(logCur.PermType==Permissions.FamilyModule) {
					row.Cells.Add("FamilyModuleViewed");
				}
				else if(logCur.PermType==Permissions.AccountModule) {
					row.Cells.Add("AccountModuleViewed");
				}
				else if(logCur.PermType==Permissions.ImagesModule) {
					row.Cells.Add("ImagesModuleViewed");
				}
				else if(logCur.PermType==Permissions.TPModule) {
					row.Cells.Add("TreatmentPlanModuleViewed");
				}
				else {
					row.Cells.Add(logCur.PermType.ToString());
				}
				row.Cells.Add(logCur.CompName);
				if(logCur.PermType!=Permissions.UserQuery) {
					row.Cells.Add(logCur.LogText);
				}
				else {
					//Only display the first snipet of very long queries. User can double click to view.
					row.Cells.Add(logCur.LogText.Left(200,true));
					row.Tag=(Action)(()=> {
						MsgBoxCopyPaste formText = new MsgBoxCopyPaste(logCur.LogText);
						formText.NormalizeContent();
						formText.Show();
					});
				}
				if(logCur.DateTPrevious.Year < 1880) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(logCur.DateTPrevious.ToString());
				}
				//Get the hash for the audit log entry from the database and rehash to compare
				if(logCur.LogHash!=SecurityLogHashes.GetHashString(logCur)) {
					row.ColorText=Color.Red; //Bad hash or no hash entry at all.  This prevents users from deleting the entire hash table to make the audit trail look valid and encrypted.
					//historical entries will show as red.
				}
				grid.Rows.Add(row);
			}
			grid.EndUpdate();
			grid.ScrollToEnd();
		}

		private void grid_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			(grid.Rows[e.Row].Tag as Action)?.Invoke();
		}

		private void butRefresh_Click(object sender, System.EventArgs e) {
			if( textDateFrom.Text=="" 
				|| textDateTo.Text==""
				|| textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!=""
				|| textRows.errorProvider1.GetError(textRows)!=""
				|| !textDateEditedFrom.IsValid
				|| !textDateEditedTo.IsValid)
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			FillGrid();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			pagesPrinted=0;
			pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			//pd.OriginAtMargins=true;
			pd.DefaultPageSettings.Landscape=true;
			if(pd.DefaultPageSettings.PrintableArea.Height==0) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			headingPrinted=false;
			#if DEBUG
				FormPrintPreview printPreview=new FormPrintPreview(PrintSituation.Default,pd,1,0,"Audit trail printed");
				printPreview.ShowDialog();
			#else
				try {
					if(PrinterL.SetPrinter(pd,PrintSituation.Default,0,"Audit trail printed")) {
						pd.Print();
					}
				}
				catch {
					MessageBox.Show(Lan.g(this,"Printer not available"));
				}
			#endif
		}

		private void checkIncludeArchived_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
		}

		///<summary>Raised for each page to be printed.</summary>
		private void pd_PrintPage(object sender,PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
				//new Rectangle(50,40,800,1035);//Some printers can handle up to 1042
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			if(!headingPrinted) {
				text=Lan.g(this,"Audit Trail");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				text=textDateFrom.Text+" "+Lan.g(this,"to")+" "+textDateTo.Text;
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=20;
				headingPrinted=true;
				headingPrintH=yPos;
			}
			#endregion
			yPos=grid.PrintPage(g,pagesPrinted,bounds,headingPrintH);
			pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
				pagesPrinted=0;
			}
			g.Dispose();
		}
	}
}





















