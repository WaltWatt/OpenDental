using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.UI;
using OpenDentBusiness.HL7;
using CodeBase;

namespace OpenDental{
	///<summary></summary>
	public class FormApptsOther : ODForm {
		private System.Windows.Forms.CheckBox checkDone;
		private OpenDental.UI.Button butCancel;
		private System.ComponentModel.Container components = null;
		///<summary>The result of the window.  In other words, which button was clicked to exit the window.</summary>
		public OtherResult oResult;
		private System.Windows.Forms.TextBox textApptModNote;
		private System.Windows.Forms.Label label1;
		private OpenDental.UI.Button butGoTo;
		private OpenDental.UI.Button butPin;
		private OpenDental.UI.Button butNew;
		private System.Windows.Forms.Label label2;
		///<summary>True if user double clicked on a blank area of appt module to get to this point.</summary>
		public bool InitialClick;
		private System.Windows.Forms.ListView listFamily;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private Appointment[] ApptList;
		private List<Recall> RecallList;
		private Patient PatCur;
		private OpenDental.UI.Button butOK;
		private Family FamCur;
		///<summary>Almost always false.  Only set to true from TaskList to allow selecting one appointment for a patient.</summary>
		public bool SelectOnly;
		private OpenDental.UI.Button butRecall;
		//<summary>This will contain a selected appointment upon closing of the form in some situations.  Used when picking an appointment for task lists.  Also used if the GoTo or Create new buttons are clicked.</summary>
		//public int AptSelected;
		///<summary>After closing, this may contain aptNums of appointments that should be placed on the pinboard. Used when picking an appointment for task lists.  Also used if the GoTo, Create new, or Recall buttons are pushed.</summary>
		public List<long> AptNumsSelected;
		///<summary>When this form closes, this will be the patNum of the last patient viewed.  The calling form should then make use of this to refresh to that patient.  If 0, then calling form should not refresh.</summary>
		public long SelectedPatNum;
		private TextBox textFinUrg;
		private Label label3;
		private OpenDental.UI.Button butNote;
		private OpenDental.UI.Button butRecallFamily;
		private UI.ODGrid gridMain;
		private CheckBox checkShowCompletePlanned;

		///<summary>If oResult=PinboardAndSearch, then when closing this form, this will contain the date to jump to when beginning the search.  If oResult=GoTo, then this will also contain the date.  Can't use DateTime type because C# complains about marshal by reference.</summary>
		public string DateJumpToString;

		///<summary></summary>
		public FormApptsOther(long patNum) {//Patient pat,Family fam){
			InitializeComponent();
			FamCur=Patients.GetFamily(patNum);
			PatCur=FamCur.GetPatient(patNum);
			Lan.F(this);
			for(int i=0;i<listFamily.Columns.Count;i++){
				listFamily.Columns[i].Text=Lan.g(this,listFamily.Columns[i].Text);
			}
			AptNumsSelected=new List<long>();
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

		private void InitializeComponent(){
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormApptsOther));
			this.checkDone = new System.Windows.Forms.CheckBox();
			this.butCancel = new OpenDental.UI.Button();
			this.textApptModNote = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.butGoTo = new OpenDental.UI.Button();
			this.butPin = new OpenDental.UI.Button();
			this.butNew = new OpenDental.UI.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.listFamily = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.butOK = new OpenDental.UI.Button();
			this.butRecall = new OpenDental.UI.Button();
			this.textFinUrg = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.butNote = new OpenDental.UI.Button();
			this.butRecallFamily = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.checkShowCompletePlanned = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// checkDone
			// 
			this.checkDone.AutoCheck = false;
			this.checkDone.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkDone.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.checkDone.Location = new System.Drawing.Point(12, 145);
			this.checkDone.Name = "checkDone";
			this.checkDone.Size = new System.Drawing.Size(168, 16);
			this.checkDone.TabIndex = 1;
			this.checkDone.TabStop = false;
			this.checkDone.Text = "Planned Appt Done";
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
			this.butCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.butCancel.Location = new System.Drawing.Point(837, 620);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 3;
			this.butCancel.Text = "&Close";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// textApptModNote
			// 
			this.textApptModNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textApptModNote.BackColor = System.Drawing.Color.White;
			this.textApptModNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textApptModNote.ForeColor = System.Drawing.Color.Red;
			this.textApptModNote.Location = new System.Drawing.Point(707, 36);
			this.textApptModNote.Multiline = true;
			this.textApptModNote.Name = "textApptModNote";
			this.textApptModNote.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textApptModNote.Size = new System.Drawing.Size(202, 36);
			this.textApptModNote.TabIndex = 44;
			this.textApptModNote.Leave += new System.EventHandler(this.textApptModNote_Leave);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label1.Location = new System.Drawing.Point(542, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(163, 21);
			this.label1.TabIndex = 45;
			this.label1.Text = "Appointment Module Note";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butGoTo
			// 
			this.butGoTo.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGoTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butGoTo.Autosize = true;
			this.butGoTo.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGoTo.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGoTo.CornerRadius = 4F;
			this.butGoTo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butGoTo.Location = new System.Drawing.Point(31, 620);
			this.butGoTo.Name = "butGoTo";
			this.butGoTo.Size = new System.Drawing.Size(125, 24);
			this.butGoTo.TabIndex = 46;
			this.butGoTo.Text = "&Go To Appt Date";
			this.butGoTo.Click += new System.EventHandler(this.butGoTo_Click);
			// 
			// butPin
			// 
			this.butPin.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPin.Autosize = true;
			this.butPin.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPin.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPin.CornerRadius = 4F;
			this.butPin.Image = global::OpenDental.Properties.Resources.butPin;
			this.butPin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPin.Location = new System.Drawing.Point(165, 620);
			this.butPin.Name = "butPin";
			this.butPin.Size = new System.Drawing.Size(134, 24);
			this.butPin.TabIndex = 47;
			this.butPin.Text = "Copy To &Pinboard";
			this.butPin.Click += new System.EventHandler(this.butPin_Click);
			// 
			// butNew
			// 
			this.butNew.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butNew.Autosize = true;
			this.butNew.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNew.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNew.CornerRadius = 4F;
			this.butNew.Image = global::OpenDental.Properties.Resources.Add;
			this.butNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butNew.Location = new System.Drawing.Point(576, 620);
			this.butNew.Name = "butNew";
			this.butNew.Size = new System.Drawing.Size(125, 24);
			this.butNew.TabIndex = 48;
			this.butNew.Text = "Create &New Appt";
			this.butNew.Click += new System.EventHandler(this.butNew_Click);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(12, 13);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(168, 17);
			this.label2.TabIndex = 57;
			this.label2.Text = "Recall for Family";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listFamily
			// 
			this.listFamily.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader4,
            this.columnHeader3,
            this.columnHeader5});
			this.listFamily.FullRowSelect = true;
			this.listFamily.GridLines = true;
			this.listFamily.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listFamily.Location = new System.Drawing.Point(12, 36);
			this.listFamily.Name = "listFamily";
			this.listFamily.Size = new System.Drawing.Size(384, 97);
			this.listFamily.TabIndex = 58;
			this.listFamily.UseCompatibleStateImageBehavior = false;
			this.listFamily.View = System.Windows.Forms.View.Details;
			this.listFamily.Click += new System.EventHandler(this.listFamily_Click);
			this.listFamily.DoubleClick += new System.EventHandler(this.listFamily_DoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Family Member";
			this.columnHeader1.Width = 120;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Age";
			this.columnHeader2.Width = 40;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Gender";
			this.columnHeader4.Width = 50;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Due Date";
			this.columnHeader3.Width = 74;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Scheduled";
			this.columnHeader5.Width = 74;
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.butOK.Location = new System.Drawing.Point(751, 620);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 59;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butRecall
			// 
			this.butRecall.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRecall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butRecall.Autosize = true;
			this.butRecall.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRecall.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRecall.CornerRadius = 4F;
			this.butRecall.Image = global::OpenDental.Properties.Resources.butRecall;
			this.butRecall.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butRecall.Location = new System.Drawing.Point(308, 620);
			this.butRecall.Name = "butRecall";
			this.butRecall.Size = new System.Drawing.Size(125, 24);
			this.butRecall.TabIndex = 60;
			this.butRecall.Text = "Schedule Recall";
			this.butRecall.Click += new System.EventHandler(this.butRecall_Click);
			// 
			// textFinUrg
			// 
			this.textFinUrg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.textFinUrg.BackColor = System.Drawing.Color.White;
			this.textFinUrg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textFinUrg.ForeColor = System.Drawing.Color.Red;
			this.textFinUrg.Location = new System.Drawing.Point(707, 78);
			this.textFinUrg.Multiline = true;
			this.textFinUrg.Name = "textFinUrg";
			this.textFinUrg.ReadOnly = true;
			this.textFinUrg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textFinUrg.Size = new System.Drawing.Size(202, 81);
			this.textFinUrg.TabIndex = 63;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label3.Location = new System.Drawing.Point(542, 81);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(163, 21);
			this.label3.TabIndex = 64;
			this.label3.Text = "Family Urgent Financial Notes";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// butNote
			// 
			this.butNote.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butNote.Autosize = true;
			this.butNote.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNote.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNote.CornerRadius = 4F;
			this.butNote.Image = ((System.Drawing.Image)(resources.GetObject("butNote.Image")));
			this.butNote.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butNote.Location = new System.Drawing.Point(442, 620);
			this.butNote.Name = "butNote";
			this.butNote.Size = new System.Drawing.Size(125, 24);
			this.butNote.TabIndex = 65;
			this.butNote.Text = "NO&TE for Patient";
			this.butNote.Click += new System.EventHandler(this.butNote_Click);
			// 
			// butRecallFamily
			// 
			this.butRecallFamily.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRecallFamily.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butRecallFamily.Autosize = true;
			this.butRecallFamily.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRecallFamily.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRecallFamily.CornerRadius = 4F;
			this.butRecallFamily.Image = global::OpenDental.Properties.Resources.butRecall;
			this.butRecallFamily.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butRecallFamily.Location = new System.Drawing.Point(308, 588);
			this.butRecallFamily.Name = "butRecallFamily";
			this.butRecallFamily.Size = new System.Drawing.Size(125, 24);
			this.butRecallFamily.TabIndex = 66;
			this.butRecallFamily.Text = "Entire Family";
			this.butRecallFamily.Click += new System.EventHandler(this.butRecallFamily_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 168);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(897, 398);
			this.gridMain.TabIndex = 67;
			this.gridMain.Title = "Appointments for Patient";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "FormDisplayFields";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// checkShowCompletePlanned
			// 
			this.checkShowCompletePlanned.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowCompletePlanned.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.checkShowCompletePlanned.Location = new System.Drawing.Point(182, 145);
			this.checkShowCompletePlanned.Name = "checkShowCompletePlanned";
			this.checkShowCompletePlanned.Size = new System.Drawing.Size(251, 16);
			this.checkShowCompletePlanned.TabIndex = 68;
			this.checkShowCompletePlanned.TabStop = false;
			this.checkShowCompletePlanned.Text = "Show Completed Planned Appts";
			this.checkShowCompletePlanned.CheckedChanged += new System.EventHandler(this.checkShowCompletePlanned_CheckedChanged);
			// 
			// FormApptsOther
			// 
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(924, 658);
			this.Controls.Add(this.checkShowCompletePlanned);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butRecallFamily);
			this.Controls.Add(this.butNote);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textFinUrg);
			this.Controls.Add(this.butRecall);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.listFamily);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butNew);
			this.Controls.Add(this.butPin);
			this.Controls.Add(this.butGoTo);
			this.Controls.Add(this.textApptModNote);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkDone);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormApptsOther";
			this.ShowInTaskbar = false;
			this.Text = "Other Appointments";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormApptsOther_Closing);
			this.Load += new System.EventHandler(this.FormApptsOther_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		///<summary></summary>
		public OtherResult OResult{
			get{return oResult;}
		}

		private void FormApptsOther_Load(object sender, System.EventArgs e) {
			Text=Lan.g(this,"Appointments for")+" "+PatCur.GetNameLF();
			textApptModNote.Text=PatCur.ApptModNote;
			if(SelectOnly){
				butGoTo.Visible=false;
				butPin.Visible=false;
				butNew.Visible=false;
				label2.Visible=false;
				listFamily.Visible=false;
			}
			else{//much more typical
				butOK.Visible=false;
			}
			FillFamily();
			FillGrid();
			gridMain.ScrollToEnd();
			CheckStatus();
		}

		private void CheckStatus(){
			if (PatCur.PatStatus == PatientStatus.Inactive
				|| PatCur.PatStatus == PatientStatus.Archived
				|| PatCur.PatStatus == PatientStatus.Prospective)
			{
				MsgBox.Show(this, "Warning. Patient is not active.");
			}
			if (PatCur.PatStatus == PatientStatus.Deceased){
				MsgBox.Show(this, "Warning. Patient is deceased.");
			}
		}

		private void FillGrid() {
			ApptList=Appointments.GetForPat(PatCur.PatNum);
			List<PlannedAppt> plannedList=PlannedAppts.Refresh(PatCur.PatNum);
			List<PlannedAppt> listPlannedIncomplete=plannedList.FindAll(x => !ApptList.ToList()
				.Exists(y => y.NextAptNum==x.AptNum && y.AptStatus==ApptStatus.Complete))
				.OrderBy(x => x.ItemOrder).ToList();
			List<Def> listProgNoteColorDefs=Defs.GetDefsForCategory(DefCat.ProgNoteColors);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("FormApptsOther","Appt Status"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormApptsOther","Prov"),50);
			gridMain.Columns.Add(col);
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				col=new ODGridColumn(Lan.g("FormApptsOther","Clinic"),80);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g("FormApptsOther","Date"),70);//If the order changes, reflect the change for dateIndex below.
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormApptsOther","Time"),70);//Must immediately follow Date column.
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormApptsOther","Min"),40);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormApptsOther","Procedures"),150);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormApptsOther","Notes"),320);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			int dateIndex=3;
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				dateIndex=2;
			}
			for(int i=0;i<ApptList.Length;i++) {
				row=new ODGridRow();
				row.Cells.Add(ApptList[i].AptStatus.ToString());
				row.Cells.Add(Providers.GetAbbr(ApptList[i].ProvNum));
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
					row.Cells.Add(Clinics.GetAbbr(ApptList[i].ClinicNum));
				}
				row.Cells.Add("");//Date
				row.Cells.Add("");//Time
				if(ApptList[i].AptDateTime.Year > 1880) {
					//only regular still scheduled appts
					if(ApptList[i].AptStatus!=ApptStatus.Planned && ApptList[i].AptStatus!=ApptStatus.PtNote 
						&& ApptList[i].AptStatus!=ApptStatus.PtNoteCompleted && ApptList[i].AptStatus!=ApptStatus.UnschedList 
						&& ApptList[i].AptStatus!=ApptStatus.Broken) 
					{
						row.Cells[dateIndex].Text=ApptList[i].AptDateTime.ToString("d");
						row.Cells[dateIndex+1].Text=ApptList[i].AptDateTime.ToString("t");
						if(ApptList[i].AptDateTime < DateTime.Today) { //Past
							row.ColorBackG=listProgNoteColorDefs[11].ItemColor;
							row.ColorText=listProgNoteColorDefs[10].ItemColor;
						}
						else if(ApptList[i].AptDateTime.Date==DateTime.Today.Date) { //Today
							row.ColorBackG=listProgNoteColorDefs[9].ItemColor;
							row.ColorText=listProgNoteColorDefs[8].ItemColor;
							row.Cells[0].Text=Lan.g(this,"Today");
						}
						else if(ApptList[i].AptDateTime > DateTime.Today) { //Future
							row.ColorBackG=listProgNoteColorDefs[13].ItemColor;
							row.ColorText=listProgNoteColorDefs[12].ItemColor;
						}
					}
					else if(ApptList[i].AptStatus==ApptStatus.Planned) { //show line for planned appt
						row.ColorBackG=listProgNoteColorDefs[17].ItemColor;
						row.ColorText=listProgNoteColorDefs[16].ItemColor;
						string txt=Lan.g("enumApptStatus","Planned")+" ";
						int plannedAptIdx=listPlannedIncomplete.FindIndex(x => x.AptNum==ApptList[i].AptNum);
						if(checkShowCompletePlanned.Checked) {
							for(int p=0;p<plannedList.Count;p++) {
								if(plannedList[p].AptNum==ApptList[i].AptNum) {
									txt+="#"+plannedList[p].ItemOrder.ToString();
								}
							}							
						}
						else {
							if(plannedAptIdx>=0) {
								txt+="#"+(plannedAptIdx+1);
							}
							else {
								continue;
							}
						}
						if(plannedAptIdx<0) {//attached to a completed appointment
							txt+=" ("+Lan.g("enumApptStatus",ApptStatus.Complete.ToString())+")";
						}
						if(ApptList.ToList().FindAll(x => x.NextAptNum==ApptList[i].AptNum)
							.Exists(x => x.AptStatus==ApptStatus.Scheduled)) //attached to a scheduled appointment
						{
							txt+=" ("+Lan.g("enumApptStatus",ApptStatus.Scheduled.ToString())+")";
						}						
						row.Cells[0].Text=txt;
					}
					else if(ApptList[i].AptStatus==ApptStatus.PtNote) {
						row.ColorBackG=listProgNoteColorDefs[19].ItemColor;
						row.ColorText=listProgNoteColorDefs[18].ItemColor;
						row.Cells[0].Text=Lan.g("enumApptStatus","PtNote");
					}
					else if(ApptList[i].AptStatus==ApptStatus.PtNoteCompleted) {
						row.ColorBackG=listProgNoteColorDefs[21].ItemColor;
						row.ColorText=listProgNoteColorDefs[20].ItemColor;
						row.Cells[0].Text=Lan.g("enumApptStatus","PtNoteCompleted");
					}
					else if(ApptList[i].AptStatus==ApptStatus.Broken) {
						row.Cells[0].Text=Lan.g("enumApptStatus","Broken");
						row.Cells[dateIndex].Text=ApptList[i].AptDateTime.ToString("d");
						row.Cells[dateIndex+1].Text=ApptList[i].AptDateTime.ToString("t");
						row.ColorBackG=listProgNoteColorDefs[15].ItemColor;
						row.ColorText=listProgNoteColorDefs[14].ItemColor;
					}
					else if(ApptList[i].AptStatus==ApptStatus.UnschedList) {
						row.Cells[0].Text=Lan.g("enumApptStatus","UnschedList");
						row.ColorBackG=listProgNoteColorDefs[15].ItemColor;
						row.ColorText=listProgNoteColorDefs[14].ItemColor;
					}
				}
				row.Cells.Add((ApptList[i].Pattern.Length * 5).ToString());
				row.Cells.Add(ApptList[i].ProcDescript);
				row.Cells.Add(ApptList[i].Note);
				row.Tag=ApptList[i].AptNum;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void FillFamily(){
			SelectedPatNum=PatCur.PatNum;//just in case user has selected a different family member
			RecallList=Recalls.GetList(FamCur.ListPats.ToList());
			//Appointment[] aptsOnePat;
			listFamily.Items.Clear();
			ListViewItem item;
			DateTime dateDue;
			DateTime dateSched;
			for(int i=0;i<FamCur.ListPats.Length;i++){
				item=new ListViewItem(FamCur.GetNameInFamFLI(i));
				if(FamCur.ListPats[i].PatNum==PatCur.PatNum){
					item.BackColor=Color.Silver;
				}
				item.SubItems.Add(FamCur.ListPats[i].Age.ToString());
				item.SubItems.Add(FamCur.ListPats[i].Gender.ToString());
				dateDue=DateTime.MinValue;
				dateSched=DateTime.MinValue;
				bool isdisabled=false;
				for(int j=0;j<RecallList.Count;j++){
					if(RecallList[j].PatNum==FamCur.ListPats[i].PatNum
						&& (RecallList[j].RecallTypeNum==RecallTypes.PerioType
						|| RecallList[j].RecallTypeNum==RecallTypes.ProphyType))
					{
						dateDue=RecallList[j].DateDue;
						dateSched=RecallList[j].DateScheduled;
						isdisabled=RecallList[j].IsDisabled;
					}
				}
				if(isdisabled){
					item.SubItems.Add(Lan.g(this,"disabled"));
				}
				else if(dateDue.Year<1880){
					item.SubItems.Add("");
				}
				else{
					item.SubItems.Add(dateDue.ToShortDateString());
				}
				if(dateDue<=DateTime.Today){
					item.ForeColor=Color.Red;
				}
				if(dateSched.Year<1880){
					item.SubItems.Add("");
				}
				else{
					item.SubItems.Add(dateSched.ToShortDateString());
				}
				listFamily.Items.Add(item);
			}
      checkDone.Checked=PatCur.PlannedIsDone;
			textFinUrg.Text=FamCur.ListPats[0].FamFinUrgNote;
		}

		private void listFamily_DoubleClick(object sender, System.EventArgs e) {
			if(listFamily.SelectedIndices.Count==0){
				return;
			}
			FormRecallsPat FormR=new FormRecallsPat();
			FormR.PatNum=PatCur.PatNum;
			FormR.ShowDialog();
			/*
			int originalPatNum=PatCur.PatNum;
			Recall recallCur=null;
			for(int i=0;i<RecallList.Count;i++){
				if(RecallList[i].PatNum==FamCur.List[listFamily.SelectedIndices[0]].PatNum){
					recallCur=RecallList[i];
				}
			}
			if(recallCur==null){
				recallCur=new Recall();
				recallCur.PatNum=FamCur.List[listFamily.SelectedIndices[0]].PatNum;
				recallCur.RecallInterval=new Interval(0,0,6,0);
			}
			FormRecallListEdit FormRLE=new FormRecallListEdit(recallCur);
			FormRLE.ShowDialog();
			if(FormRLE.PinClicked){
				oResult=OtherResult.CopyToPinBoard;
				AptNumsSelected.Add(FormRLE.AptSelected);
				DialogResult=DialogResult.OK;
			}
			else{
				FamCur=Patients.GetFamily(originalPatNum);
				PatCur=FamCur.GetPatient(originalPatNum);
				Filltb();
			}*/
		}

		private void checkShowCompletePlanned_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void butRecall_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AppointmentCreate)) {
				return;
			}
			if(PatRestrictionL.IsRestricted(PatCur.PatNum,PatRestrict.ApptSchedule)) {
				return;
			}
			if(AppointmentL.PromptForMerge(PatCur,out PatCur)) {
				FillFamily();
				FillGrid();
				FormOpenDental.S_Contr_PatientSelected(PatCur,true,false);
			}
			if(PatCur!=null && PatCur.PatStatus.In(PatientStatus.Archived,PatientStatus.Deceased)) {
				MsgBox.Show(this,"Appointments cannot be scheduled for "+PatCur.PatStatus.ToString().ToLower()+" patients.");
				return;
			}
			MakeRecallAppointment();
		}

		public void MakeRecallAppointment(){
			//List<Recall> recallList=Recalls.GetList(PatCur.PatNum);//get the recall for this pt
			//if(recallList.Count==0){
			//	MsgBox.Show(this,"This patient does not have any recall due.");
			//	return;
			//}
			//Recall recallCur=recallList[0];
			List<InsSub> subList=InsSubs.RefreshForFam(FamCur);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			Appointment apt=null;
			DateTime aptDateTime=DateTime.MinValue;
			if(this.InitialClick) {
				DateTime d;
				if(ApptDrawing.IsWeeklyView) {
					d=ContrAppt.WeekStartDate.AddDays(ContrAppt.SheetClickedonDay);
				}
				else {
					d=AppointmentL.DateSelected;
				}
				//Calculate minutes for appointment
				int minutes=(int)(ContrAppt.SheetClickedonMin/ApptDrawing.MinPerIncr)*ApptDrawing.MinPerIncr;
				aptDateTime=new DateTime(d.Year,d.Month,d.Day,ContrAppt.SheetClickedonHour,minutes,0);
			}
			try{
				apt=AppointmentL.CreateRecallApt(PatCur,planList,-1,subList,aptDateTime);
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
				return;
			}
			DateTime datePrevious=apt.DateTStamp;
			AptNumsSelected.Add(apt.AptNum);
			if(this.InitialClick) {
				Appointment oldApt=apt.Copy();
				if(PatCur.AskToArriveEarly>0){
					apt.DateTimeAskedToArrive=apt.AptDateTime.AddMinutes(-PatCur.AskToArriveEarly);
					MessageBox.Show(Lan.g(this,"Ask patient to arrive")+" "+PatCur.AskToArriveEarly
						+" "+Lan.g(this,"minutes early at")+" "+apt.DateTimeAskedToArrive.ToShortTimeString()+".");
				}
				apt.AptStatus=ApptStatus.Scheduled;
				apt.ClinicNum=PatCur.ClinicNum;
				apt.Op=ContrAppt.SheetClickedonOp;
				apt=Appointments.AssignFieldsForOperatory(apt);
				Appointments.Update(apt,oldApt);
				oResult=OtherResult.CreateNew;
				SecurityLogs.MakeLogEntry(Permissions.AppointmentCreate,apt.PatNum,apt.AptDateTime.ToString(),apt.AptNum,datePrevious);
				//If there is an existing HL7 def enabled, send a SIU message if there is an outbound SIU message defined
				if(HL7Defs.IsExistingHL7Enabled()) {
					//S12 - New Appt Booking event
					MessageHL7 messageHL7=MessageConstructor.GenerateSIU(PatCur,FamCur.GetPatient(PatCur.Guarantor),EventTypeHL7.S12,apt);
					//Will be null if there is no outbound SIU message defined, so do nothing
					if(messageHL7!=null) {
						HL7Msg hl7Msg=new HL7Msg();
						hl7Msg.AptNum=apt.AptNum;
						hl7Msg.HL7Status=HL7MessageStatus.OutPending;//it will be marked outSent by the HL7 service.
						hl7Msg.MsgText=messageHL7.ToString();
						hl7Msg.PatNum=PatCur.PatNum;
						HL7Msgs.Insert(hl7Msg);
#if DEBUG
						MessageBox.Show(this,messageHL7.ToString());
#endif
					}
				}
				DialogResult=DialogResult.OK;
				return;
			}
			//not initialClick
			oResult=OtherResult.PinboardAndSearch;
			Recall recall=Recalls.GetRecallProphyOrPerio(PatCur.PatNum);//shouldn't return null.
			if(recall.DateDue<DateTime.Today){
				DateJumpToString=DateTime.Today.ToShortDateString();//they are overdue
			}
			else{
				DateJumpToString=recall.DateDue.ToShortDateString();
			}
			//no securitylog entry needed here.  That will happen when it's dragged off pinboard.
			DialogResult=DialogResult.OK;
		}

		private void butRecallFamily_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AppointmentCreate)) {
				return;
			}
			MakeRecallFamily();
		}

		public void MakeRecallFamily(){
			List<Recall> recallList;//=Recalls.GetList(FamCur.ListPats
			List <InsPlan> planList;
			List<InsSub> subList;
			Appointment apt=null;
			Recall recall;
			int alreadySched=0;
			int noRecalls=0;
			int patsRestricted=0;
			int patsArchivedOrDeceased=0;
			for(int i=0;i<FamCur.ListPats.Length;i++){
				Patient patCur=FamCur.ListPats[i];
				if(PatRestrictionL.IsRestricted(patCur.PatNum,PatRestrict.ApptSchedule,true)) {
					patsRestricted++;
					continue;
				}
				if(patCur.PatStatus.In(PatientStatus.Archived,PatientStatus.Deceased)) {
					patsArchivedOrDeceased++;
					continue;
				}
				recallList=Recalls.GetList(patCur.PatNum);//get the recall for this pt
				//Check to see if the special type recall is disabled or already scheduled.  This is also done in AppointmentL.CreateRecallApt() below so I'm
				//	not sure why we do it here.
				List<Recall> listRecalls=recallList.FindAll(x => x.RecallTypeNum==RecallTypes.PerioType || x.RecallTypeNum==RecallTypes.ProphyType);
				if(listRecalls.Count==0 || listRecalls.Exists(x => x.IsDisabled)) {
					noRecalls++;
					continue;
				}
				if(listRecalls.Exists(x => x.DateScheduled.Year > 1880)) {
					alreadySched++;
					continue;
				}
				subList=InsSubs.RefreshForFam(FamCur);
				planList=InsPlans.RefreshForSubList(subList);
				try{
					apt=AppointmentL.CreateRecallApt(patCur,planList,-1,subList);
				}
				catch{
					//MessageBox.Show(ex.Message);
					continue;
				}
				AptNumsSelected.Add(apt.AptNum);
				oResult=OtherResult.PinboardAndSearch;
				recall=Recalls.GetRecallProphyOrPerio(patCur.PatNum);//should not return null
				if(recall.DateDue<DateTime.Today) {
					DateJumpToString=DateTime.Today.ToShortDateString();//they are overdue
				}
				else {
					DateJumpToString=recall.DateDue.ToShortDateString();
				}
				//Log will be made when appointment dragged off of the pinboard.
				//SecurityLogs.MakeLogEntry(Permissions.AppointmentCreate,apt.PatNum,apt.AptDateTime.ToString(),apt.AptNum);
			}
			List<string> listUserMsgs=new List<string>();
			if(patsRestricted>0) {
				listUserMsgs.Add(Lan.g(this,"Family members skipped due to patient restriction")+" "
					+PatRestrictions.GetPatRestrictDesc(PatRestrict.ApptSchedule)+": "+patsRestricted+".");
			}
			if(noRecalls > 0){
				listUserMsgs.Add(Lan.g(this,"Family members skipped because recall disabled: ")+noRecalls+".");
			}
			if(alreadySched > 0) {
				listUserMsgs.Add(Lan.g(this,"Family members skipped because already scheduled: ")+alreadySched+".");
			}
			if(patsArchivedOrDeceased > 0) {
				listUserMsgs.Add(Lan.g(this,"Family members skipped because status is archived or deceased: ")+patsArchivedOrDeceased+".");
			}
			if(AptNumsSelected.Count==0) {
				listUserMsgs.Add(Lan.g(this,"There are no recall appointments to schedule."));
			}
			if(listUserMsgs.Count>0) {
				MessageBox.Show(string.Join("\r\n",listUserMsgs));
				if(AptNumsSelected.Count==0) {
					return;
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void butNote_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AppointmentCreate)) {
				return;
			}
			if(PatRestrictionL.IsRestricted(PatCur.PatNum,PatRestrict.ApptSchedule)) {
				return;
			}
			Appointment AptCur=new Appointment();
			AptCur.PatNum=PatCur.PatNum;
			if(PatCur.DateFirstVisit.Year<1880
				&& !Procedures.AreAnyComplete(PatCur.PatNum))//this only runs if firstVisit blank
			{
				AptCur.IsNewPatient=true;
			}
			AptCur.Pattern="/X/";
			if(PatCur.PriProv==0) {
				AptCur.ProvNum=PrefC.GetLong(PrefName.PracticeDefaultProv);
			}
			else {
				AptCur.ProvNum=PatCur.PriProv;
			}
			AptCur.ProvHyg=PatCur.SecProv;
			AptCur.AptStatus=ApptStatus.PtNote;
			AptCur.ClinicNum=PatCur.ClinicNum;
			AptCur.TimeLocked=PrefC.GetBool(PrefName.AppointmentTimeIsLocked);
			if(InitialClick) {//initially double clicked on appt module
				DateTime d;
				if(ApptDrawing.IsWeeklyView) {
					d=ContrAppt.WeekStartDate.AddDays(ContrAppt.SheetClickedonDay);
				}
				else {
					d=AppointmentL.DateSelected;
				}
				int minutes=(int)(ContrAppt.SheetClickedonMin/ApptDrawing.MinPerIncr)
					*ApptDrawing.MinPerIncr;
				AptCur.AptDateTime=new DateTime(d.Year,d.Month,d.Day
					,ContrAppt.SheetClickedonHour,minutes,0);
				AptCur.Op=ContrAppt.SheetClickedonOp;
			}
			else {
				//new appt will be placed on pinboard instead of specific time
			}
			try {
				Appointments.Insert(AptCur);
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			FormApptEdit FormApptEdit2=new FormApptEdit(AptCur.AptNum);
			FormApptEdit2.IsNew=true;
			FormApptEdit2.ShowDialog();
			if(FormApptEdit2.DialogResult!=DialogResult.OK) {
				return;
			}
			AptNumsSelected.Add(AptCur.AptNum);
			if(InitialClick) {
				oResult=OtherResult.CreateNew;
			}
			else {
				oResult=OtherResult.NewToPinBoard;
			}
			DialogResult=DialogResult.OK;

		}

		private void butNew_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AppointmentCreate)) {
				return;
			}
			if(PatRestrictionL.IsRestricted(PatCur.PatNum,PatRestrict.ApptSchedule)) {
				return;
			}
			if(AppointmentL.PromptForMerge(PatCur,out PatCur)) {
				FillFamily();
				FillGrid();
				FormOpenDental.S_Contr_PatientSelected(PatCur,true,false);
			}
			if(PatCur!=null && PatCur.PatStatus.In(PatientStatus.Archived,PatientStatus.Deceased)) {
				MsgBox.Show(this,"Appointments cannot be scheduled for "+PatCur.PatStatus.ToString().ToLower()+" patients.");
				return;
			}
			MakeAppointment();
		}

		public void MakeAppointment(){
			FormApptEdit FormApptEdit2=new FormApptEdit(0,patNum:PatCur.PatNum,useApptDrawingSettings:InitialClick,patient:PatCur);
			FormApptEdit2.IsNew=true;
			FormApptEdit2.ShowDialog();
			if(FormApptEdit2.DialogResult!=DialogResult.OK){
				return;
			}
			Appointment AptCur=FormApptEdit2.GetAppointmentCur();
			if(InitialClick) {
				//Change PatStatus to Prospective or from Prospective.
				Operatory opCur=Operatories.GetOperatory(AptCur.Op);
				if(opCur!=null) {
					if(opCur.SetProspective && PatCur.PatStatus!=PatientStatus.Prospective) { //Don't need to prompt if patient is already prospective.
						if(MsgBox.Show(this,MsgBoxButtons.OKCancel,"Patient's status will be set to Prospective.")) {
							Patient patOld=PatCur.Copy();
							PatCur.PatStatus=PatientStatus.Prospective;
							Patients.Update(PatCur,patOld);
						}
					}
					else if(!opCur.SetProspective && PatCur.PatStatus==PatientStatus.Prospective) {
						if(MsgBox.Show(this,MsgBoxButtons.OKCancel,"Patient's status will change from Prospective to Patient.")) {
							Patient patOld=PatCur.Copy();
							PatCur.PatStatus=PatientStatus.Patient;
							Patients.Update(PatCur,patOld);
						}
					}
				}
			}
			AptNumsSelected.Add(AptCur.AptNum);
			if(InitialClick){
				oResult=OtherResult.CreateNew;
			}
			else{
				oResult=OtherResult.NewToPinBoard;
			}
			if(AptCur.IsNewPatient) {
				AutomationL.Trigger(AutomationTrigger.CreateApptNewPat,null,AptCur.PatNum,AptCur.AptNum);
			}
			AutomationL.Trigger(AutomationTrigger.CreateAppt,null,AptCur.PatNum,AptCur.AptNum);
			DialogResult=DialogResult.OK;
		}

		private void butPin_Click(object sender, System.EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1){
				MessageBox.Show(Lan.g(this,"Please select appointment first."));
				return;
			}
			if(PatRestrictionL.IsRestricted(PatCur.PatNum,PatRestrict.ApptSchedule)) {
				return;
			}
			if(!OKtoSendToPinboard(ApptList.FirstOrDefault(x => x.AptNum==(long)gridMain.Rows[gridMain.GetSelectedIndex()].Tag))) {//Tag is AptNum
				return;
			}
			AptNumsSelected.Add((long)gridMain.Rows[gridMain.GetSelectedIndex()].Tag);
			oResult=OtherResult.CopyToPinBoard;
			DialogResult=DialogResult.OK;
		}

		/// <summary>Tests the appointment to see if it is acceptable to send it to the pinboard.  Also asks user appropriate questions to verify that's what they want to do.  Returns false if it will not be going to pinboard after all.</summary>
		private bool OKtoSendToPinboard(Appointment AptCur){
			if(AptCur.AptStatus==ApptStatus.Planned){//if is a Planned appointment
				bool PlannedIsSched=false;
				for(int i=0;i<ApptList.Length;i++){
					if(ApptList[i].NextAptNum==AptCur.AptNum){//if the planned appointment is already sched
						PlannedIsSched=true;
					}
				}
				if(PlannedIsSched){
					if(MessageBox.Show(Lan.g(this,"The Planned appointment is already scheduled.  Do you wish to continue?"),"",MessageBoxButtons.OKCancel)!=DialogResult.OK){
						return false;
					}
				}
			}
			else{//if appointment is not Planned
				switch(AptCur.AptStatus){
					case ApptStatus.Complete:
						MessageBox.Show(Lan.g(this,"Not allowed to move a completed appointment from here."));
						return false;
					case ApptStatus.Scheduled:
						if(MessageBox.Show(Lan.g(this,"Do you really want to move a previously scheduled appointment?"),"",MessageBoxButtons.OKCancel)!=DialogResult.OK){
							return false;
						}
						break;
					case ApptStatus.Broken://status gets changed after dragging off pinboard.
					case ApptStatus.None:
					case ApptStatus.UnschedList://status gets changed after dragging off pinboard.
						break;
				}			
			}
			//if it's a planned appointment, the planned appointment will end up on the pinboard.  The copy will be made after dragging it off the pinboard.
			return true;
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			int currentSelection=e.Row;
			int currentScroll=gridMain.ScrollValue;
			FormApptEdit FormAE=new FormApptEdit((long)gridMain.Rows[e.Row].Tag);//Tag is AptNum
			FormAE.IsInViewPatAppts=true;
			FormAE.PinIsVisible=true;
			FormAE.ShowDialog();
			if(FormAE.DialogResult!=DialogResult.OK)
				return;
			if(FormAE.PinClicked){
				if(!OKtoSendToPinboard(ApptList.FirstOrDefault(x => x.AptNum==(long)gridMain.Rows[e.Row].Tag))) {
					return;
				}
				AptNumsSelected.Add((long)gridMain.Rows[e.Row].Tag);
				oResult=OtherResult.CopyToPinBoard;
				DialogResult=DialogResult.OK;
			}
			else{
				FillFamily();
				FillGrid();
				gridMain.SetSelected(currentSelection,true);
				gridMain.ScrollValue=currentScroll;
			}
		}

		private void listFamily_Click(object sender,EventArgs e) {
			//Changes the patient to whoever was clicked in the list 
			if(listFamily.SelectedIndices.Count==0) {
				return;
			}
			long oldPatNum=PatCur.PatNum;
			long newPatNum=FamCur.ListPats[listFamily.SelectedIndices[0]].PatNum;
			if(newPatNum==oldPatNum){
				return;
			}
			PatCur=FamCur.GetPatient(newPatNum);
			Text=Lan.g(this,"Appointments for")+" "+PatCur.GetNameLF();
			textApptModNote.Text=PatCur.ApptModNote;
			FillFamily();
			FillGrid();
			CheckStatus();
		}

		private void textApptModNote_Leave(object sender,EventArgs e) {
			if(textApptModNote.Text!=PatCur.ApptModNote){
				Patient PatOld=PatCur.Copy();
				PatCur.ApptModNote=textApptModNote.Text;
				Patients.Update(PatCur,PatOld);
			}
		}

		private void butGoTo_Click(object sender, System.EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1){
				MessageBox.Show(Lan.g(this,"Please select appointment first."));
				return;
			}
			Appointment aptSelected=ApptList.FirstOrDefault(x => x.AptNum==(long)gridMain.Rows[gridMain.GetSelectedIndex()].Tag);//Tag is AptNum
			if(aptSelected.AptDateTime.Year<1880) {
				MessageBox.Show(Lan.g(this,"Unable to go to unscheduled appointment."));
				return;
			}
			AptNumsSelected.Add(aptSelected.AptNum);
			DateJumpToString=aptSelected.AptDateTime.Date.ToShortDateString();
			oResult=OtherResult.GoTo;
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			//only used when selecting from TaskList. oResult is completely ignored in this case.
			//I didn't bother enabling double click. Maybe later.
			if(gridMain.GetSelectedIndex()==-1){
				MessageBox.Show(Lan.g(this,"Please select appointment first."));
				return;
			}
			AptNumsSelected.Add((long)gridMain.Rows[gridMain.GetSelectedIndex()].Tag);//Tag is AptNum
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormApptsOther_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(DialogResult==DialogResult.OK){
				return;
			}
			oResult=OtherResult.Cancel;
		}
	}
}
