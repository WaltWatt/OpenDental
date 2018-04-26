using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Linq;

namespace OpenDental{
	/// <summary>
	/// </summary>
	public class FormDiseaseDefs:ODForm {
		private OpenDental.UI.Button butClose;
		private System.ComponentModel.IContainer components;
		private OpenDental.UI.Button butDown;
		private System.Windows.Forms.ToolTip toolTip1;
		private OpenDental.UI.Button butOK;
		///<summary>Set to true when user is using this to select a disease def. Currently used when adding Alerts to Rx.</summary>
		public bool IsSelectionMode;
		///<summary>Set to true when user is using FormMedical to allow multiple problems to be selected at once.</summary>
		public bool IsMultiSelect;
		///<summary>On FormClosing, if IsSelectionMode, this will contain the selected DiseaseDefs.  Unless IsMultiSelect is true, it will only contain one.</summary>
		public List<DiseaseDef> ListSelectedDiseaseDefs;
		private ODGrid gridMain;
		private bool IsChanged;
		///<summary>A complete list of disease defs including hidden.  Only used when not in selection mode (item orders can change).  
		///It's main purpose is to keep track of the item order for the life of the window 
		///so that we do not have to make unnecessary update calls to the database every time the up and down buttons are clicked.</summary>
		private List<DiseaseDef> _listDiseaseDefs;
		///<summary>Stale deep copy of _listDiseaseDefs to use with sync.</summary>
		private List<DiseaseDef> _listDiseaseDefsOld;
		///<summary>List of diseaseDefs currently available in the grid after filtering.</summary>
		private List<DiseaseDef> _listDiseaseDefsShowing;
		///<summary>List of all the DiseaseDefNums that cannot be deleted because they could be in use by other tables.</summary>
		private List<long> _listDiseaseDefsNumsNotDeletable;
		///<summary>List of messages returned by FormDiseaseDefEdit for creating log messages after syncing.  All messages in this list use ProblemEdit
		///permission.</summary>
		private List<string> _listSecurityLogMsgs;
		///<summary>A copy of ListSelectedDefs taken when the form loads. This is the list of diseases that are colored in the grid,
		///and should not change once the form is loaded.</summary>
		private List<long> _listDiseaseDefNumsColored;
		private GroupBox groupBox1;
		private ODtextBox textSnoMed;
		private ODtextBox textDescript;
		private Label label3;
		private Label label2;
		private ODtextBox textICD10;
		private Label label1;
		private ODtextBox textICD9;
		private Label label4;
		private UI.Button butUp;
		private UI.Button butAdd;
		private Timer timerSearch;
		private CheckBox checkShowHidden;
		private UI.Button butAlphabetize;
		private Label labelAlphabetize;

		///<summary> On initialization, you may pass in a list of diseaseDefNums that you want colored in this list. 
		///Currently, this is used from FormMedical to show the user which active problems are assigned to the current patient.</summary>
		public FormDiseaseDefs(List<long> listDiseaseDefNums=null)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			if(listDiseaseDefNums!=null) {
				_listDiseaseDefNumsColored=listDiseaseDefNums;
			}
			else {
				_listDiseaseDefNumsColored=new List<long>();
			}
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDiseaseDefs));
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.timerSearch = new System.Windows.Forms.Timer(this.components);
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.checkShowHidden = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textSnoMed = new OpenDental.ODtextBox();
			this.textDescript = new OpenDental.ODtextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.textICD10 = new OpenDental.ODtextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.textICD9 = new OpenDental.ODtextBox();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butAdd = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.butAlphabetize = new OpenDental.UI.Button();
			this.labelAlphabetize = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// timerSearch
			// 
			this.timerSearch.Interval = 500;
			this.timerSearch.Tick += new System.EventHandler(this.timerSearch_Tick);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.checkShowHidden);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.textSnoMed);
			this.groupBox1.Controls.Add(this.textDescript);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.textICD10);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.textICD9);
			this.groupBox1.Location = new System.Drawing.Point(18, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(537, 128);
			this.groupBox1.TabIndex = 20;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Search";
			// 
			// checkShowHidden
			// 
			this.checkShowHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkShowHidden.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowHidden.Checked = true;
			this.checkShowHidden.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowHidden.Location = new System.Drawing.Point(277, 106);
			this.checkShowHidden.Name = "checkShowHidden";
			this.checkShowHidden.Size = new System.Drawing.Size(252, 18);
			this.checkShowHidden.TabIndex = 28;
			this.checkShowHidden.Text = "Show Hidden";
			this.checkShowHidden.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowHidden.CheckedChanged += new System.EventHandler(this.checkShowHidden_CheckedChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(7, 78);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(79, 20);
			this.label4.TabIndex = 27;
			this.label4.Text = "Description";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textSnoMed
			// 
			this.textSnoMed.AcceptsTab = true;
			this.textSnoMed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textSnoMed.BackColor = System.Drawing.SystemColors.Window;
			this.textSnoMed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textSnoMed.DetectLinksEnabled = false;
			this.textSnoMed.DetectUrls = false;
			this.textSnoMed.Location = new System.Drawing.Point(88, 59);
			this.textSnoMed.Multiline = false;
			this.textSnoMed.Name = "textSnoMed";
			this.textSnoMed.QuickPasteType = OpenDentBusiness.QuickPasteType.MedicationEdit;
			this.textSnoMed.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textSnoMed.Size = new System.Drawing.Size(234, 20);
			this.textSnoMed.TabIndex = 26;
			this.textSnoMed.Text = "";
			this.textSnoMed.TextChanged += new System.EventHandler(this.textSearchBoxes_TextChanged);
			// 
			// textDescript
			// 
			this.textDescript.AcceptsTab = true;
			this.textDescript.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textDescript.BackColor = System.Drawing.SystemColors.Window;
			this.textDescript.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textDescript.DetectLinksEnabled = false;
			this.textDescript.DetectUrls = false;
			this.textDescript.Location = new System.Drawing.Point(88, 80);
			this.textDescript.Multiline = false;
			this.textDescript.Name = "textDescript";
			this.textDescript.QuickPasteType = OpenDentBusiness.QuickPasteType.MedicationEdit;
			this.textDescript.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textDescript.Size = new System.Drawing.Size(441, 20);
			this.textDescript.TabIndex = 25;
			this.textDescript.Text = "";
			this.textDescript.TextChanged += new System.EventHandler(this.textSearchBoxes_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(7, 59);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(79, 20);
			this.label3.TabIndex = 24;
			this.label3.Text = "SNOMED";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(7, 39);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(79, 20);
			this.label2.TabIndex = 22;
			this.label2.Text = "ICD10";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textICD10
			// 
			this.textICD10.AcceptsTab = true;
			this.textICD10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textICD10.BackColor = System.Drawing.SystemColors.Window;
			this.textICD10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textICD10.DetectLinksEnabled = false;
			this.textICD10.DetectUrls = false;
			this.textICD10.Location = new System.Drawing.Point(88, 39);
			this.textICD10.Multiline = false;
			this.textICD10.Name = "textICD10";
			this.textICD10.QuickPasteType = OpenDentBusiness.QuickPasteType.MedicationEdit;
			this.textICD10.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textICD10.Size = new System.Drawing.Size(234, 20);
			this.textICD10.TabIndex = 21;
			this.textICD10.Text = "";
			this.textICD10.TextChanged += new System.EventHandler(this.textSearchBoxes_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(7, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(79, 20);
			this.label1.TabIndex = 20;
			this.label1.Text = "ICD9";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textICD9
			// 
			this.textICD9.AcceptsTab = true;
			this.textICD9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textICD9.BackColor = System.Drawing.SystemColors.Window;
			this.textICD9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textICD9.DetectLinksEnabled = false;
			this.textICD9.DetectUrls = false;
			this.textICD9.Location = new System.Drawing.Point(88, 19);
			this.textICD9.Multiline = false;
			this.textICD9.Name = "textICD9";
			this.textICD9.QuickPasteType = OpenDentBusiness.QuickPasteType.MedicationEdit;
			this.textICD9.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.textICD9.Size = new System.Drawing.Size(234, 20);
			this.textICD9.TabIndex = 19;
			this.textICD9.Text = "";
			this.textICD9.TextChanged += new System.EventHandler(this.textSearchBoxes_TextChanged);
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
			this.gridMain.Location = new System.Drawing.Point(18, 137);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(537, 548);
			this.gridMain.TabIndex = 16;
			this.gridMain.Title = null;
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableProblems";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(561, 137);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(79, 26);
			this.butAdd.TabIndex = 21;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(592, 629);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(79, 26);
			this.butOK.TabIndex = 15;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(561, 426);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(79, 26);
			this.butDown.TabIndex = 14;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(561, 394);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(79, 26);
			this.butUp.TabIndex = 13;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(592, 661);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(79, 26);
			this.butClose.TabIndex = 1;
			this.butClose.Text = "Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butAlphabetize
			// 
			this.butAlphabetize.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butAlphabetize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAlphabetize.Autosize = true;
			this.butAlphabetize.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAlphabetize.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAlphabetize.CornerRadius = 4F;
			this.butAlphabetize.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAlphabetize.Location = new System.Drawing.Point(561, 245);
			this.butAlphabetize.Name = "butAlphabetize";
			this.butAlphabetize.Size = new System.Drawing.Size(79, 26);
			this.butAlphabetize.TabIndex = 22;
			this.butAlphabetize.Text = "Alphabetize";
			this.butAlphabetize.Click += new System.EventHandler(this.butAlphabetize_Click);
			// 
			// labelAlphabetize
			// 
			this.labelAlphabetize.Location = new System.Drawing.Point(561, 209);
			this.labelAlphabetize.Name = "labelAlphabetize";
			this.labelAlphabetize.Size = new System.Drawing.Size(118, 33);
			this.labelAlphabetize.TabIndex = 29;
			this.labelAlphabetize.Text = "Orders the problem list alphabetically";
			this.labelAlphabetize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FormDiseaseDefs
			// 
			this.ClientSize = new System.Drawing.Size(680, 697);
			this.Controls.Add(this.labelAlphabetize);
			this.Controls.Add(this.butAlphabetize);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormDiseaseDefs";
			this.ShowInTaskbar = false;
			this.Text = "Problems";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDiseaseDefs_FormClosing);
			this.Load += new System.EventHandler(this.FormDiseaseDefs_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormDiseaseDefs_Load(object sender, System.EventArgs e) {
			if(DiseaseDefs.FixItemOrders()) {
				DataValid.SetInvalid(InvalidType.Diseases);
			}
			_listSecurityLogMsgs=new List<string>();
			if(IsSelectionMode){
				//Hide and change UI buttons
				butClose.Text=Lan.g(this,"Cancel");
				butDown.Visible=false;
				butUp.Visible=false;
				labelAlphabetize.Visible=false;
				butAlphabetize.Visible=false;
				checkShowHidden.Visible=false;
				if(IsMultiSelect) {
					gridMain.SelectionMode=GridSelectionMode.MultiExtended;
				}
				//show only non-hidden items.
				_listDiseaseDefs=DiseaseDefs.GetDeepCopy(true);
			}
			else{
				//change UI
				butOK.Visible=false;
				//show all items, including hidden.
				_listDiseaseDefs=DiseaseDefs.GetDeepCopy();
			}
			//If the user has passed in DiseaseDefs, those should be highlighted. Otherwise, initialize a new List<DiseaseDef>.
			if(ListSelectedDiseaseDefs==null) {
				ListSelectedDiseaseDefs=new List<DiseaseDef>();
			}
			_listDiseaseDefsOld=_listDiseaseDefs.Select(x => x.Copy()).ToList();
			_listDiseaseDefsShowing=new List<DiseaseDef>();//fillGrid takes care of filling this.
			_listDiseaseDefsNumsNotDeletable=DiseaseDefs.ValidateDeleteList(_listDiseaseDefs.Select(x => x.DiseaseDefNum).ToList());
			FillGrid();
		}

		private void FillGrid() {
			//get the list of disease defs currently showing based on the search terms.
			_listDiseaseDefsShowing=FilterList(_listDiseaseDefs);
			//disable the up/down buttons if not all the problems are showing.
			if(_listDiseaseDefsShowing.Count!=_listDiseaseDefs.Count) {
				butUp.Enabled=false;
				butDown.Enabled=false;
				butAlphabetize.Enabled=false;
			}
			else {
				butUp.Enabled=true;
				butDown.Enabled=true;
				butAlphabetize.Enabled=true;
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"ICD-9"),50);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"ICD-10"),50);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"SNOMED CT"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Description"),250);
			gridMain.Columns.Add(col);
			if(!IsSelectionMode) {
				col=new ODGridColumn(Lan.g(this,"Hidden"),50,HorizontalAlignment.Center);
				gridMain.Columns.Add(col);
			}
			gridMain.Rows.Clear();
			ODGridRow row;
			foreach(DiseaseDef defCur in _listDiseaseDefsShowing) {
				row=new ODGridRow();
				row.Cells.Add(defCur.ICD9Code);
				row.Cells.Add(defCur.Icd10Code);
				row.Cells.Add(defCur.SnomedCode);
				row.Cells.Add(defCur.DiseaseName);
				if(!IsSelectionMode) {
					row.Cells.Add(defCur.IsHidden ? "X" : "");
				}
				row.Tag=defCur;
				if(_listDiseaseDefNumsColored.Contains(defCur.DiseaseDefNum)) {
					row.ColorBackG=Color.LightCyan;
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		///<summary>Starts with the passed-in list of DiseaseDefs and cascades down to filter based on the search criteria without making db calls. 
		///Returns the filtered list. Normally, pass in the full list of DiseaseDefs.</summary>
		private List<DiseaseDef> FilterList(List<DiseaseDef> listCur) {
			string[] listTerms=new string[] {""}; //list of terms to filter on.
			List<DiseaseDef> listICD9=new List<DiseaseDef>();
			//ICD9
			listTerms=textICD9.Text.Split(new char[] { ' ' }); //each space is a new filter item. we could add other punctuation here if we wanted to. 
			if(listTerms.Length!=0) {
				foreach(DiseaseDef defCur in listCur) { //take the starting list, and for each element...
					if(listTerms.All(x => defCur.ICD9Code.ToLower().Contains(x.ToLower()))) { //if every filter item matches something in the disease's ICD9Code field..
						if(!listICD9.Contains(defCur)) {
							listICD9.Add(defCur); //add it to the result list.
						}
					}
				}
			}
			else {
				listICD9=listCur;
			}
			List<DiseaseDef> listICD10=new List<DiseaseDef>();
			//ICD10
			listTerms=textICD10.Text.Split(new char[] { ' ' });
			if(listTerms.Length!=0) {
				foreach(DiseaseDef defCur in listICD9) { //use the result list from above and further filter it based on the text entered into this search box.
					if(listTerms.All(x => defCur.Icd10Code.ToLower().Contains(x.ToLower()))) {
						if(!listICD10.Contains(defCur)) {
							listICD10.Add(defCur);
						}
					}
				}
			}
			else {
				listICD10=listICD9;
			}
			List<DiseaseDef> listSnoMed=new List<DiseaseDef>();
			//SNOMED
			listTerms=textSnoMed.Text.Split(new char[] { ' ' });
			if(listTerms.Length!=0) {
				foreach(DiseaseDef defCur in listICD10) {//use the result list from above and further filter it based on the text entered into this search box.
					if(listTerms.All(x => defCur.SnomedCode.ToLower().Contains(x.ToLower()))) {
						if(!listSnoMed.Contains(defCur)) {
							listSnoMed.Add(defCur);
						}
					}
				}
			}
			else {
				listSnoMed=listICD10;
			}
			List<DiseaseDef> listDesc=new List<DiseaseDef>();
			//DESCRIPTION
			listTerms=textDescript.Text.Split(new char[] { ' ' });
			if(listTerms.Length!=0) {
				foreach(DiseaseDef defCur in listSnoMed) {//use the result list from above and further filter it based on the text entered into this search box.
					if(listTerms.All(x => defCur.DiseaseName.ToLower().Contains(x.ToLower()))) {
						if(!listDesc.Contains(defCur)) {
							listDesc.Add(defCur);
						}
					}
				}
			}
			else {
				listDesc=listSnoMed;
			}
			//HIDDEN
			if(!checkShowHidden.Checked) {//use the result list from above and further filter it based on the whether it's hidden or not.
				listDesc=listDesc.Where(x => !x.IsHidden).ToList();
			}
			return listDesc; //return the completely filtered list.
		}

		///<summary>If IsSelectionMode, doubleclicking closes the form and returns the selected diseasedef.
		///If !IsSelectionMode, doubleclicking opens FormDiseaseDefEdit and allows the user to edit or delete the selected diseasedef.
		///Either way, validation always occurs first.</summary>
		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			#region Validation
			if(!IsSelectionMode && !Security.IsAuthorized(Permissions.ProblemEdit)) {//trying to double click to edit, but no permission.
				return;
			}
			if(gridMain.SelectedIndices.Length==0) {
				return;
			}
			#endregion
			DiseaseDef selectedDiseaseDef = (DiseaseDef)gridMain.Rows[gridMain.GetSelectedIndex()].Tag;
			#region Selection Mode. Close the Form
			if(IsSelectionMode) {//selection mode.
				if(!IsMultiSelect && Snomeds.GetByCode(selectedDiseaseDef.SnomedCode)==null) {
					MsgBox.Show(this,"You have selected a problem with an unofficial SNOMED CT code.  Please correct the problem definition by going to "
						+"Lists | Problems and choosing an official code from the SNOMED CT list.");
					return;
				}
				DialogResult=DialogResult.OK;//FormClosing takes care of filling ListSelectedDiseaseDefs.
				return;
			}
			#endregion
			#region Not Selection Mode. Open FormDiseaseDefEdit
			//not selection mode. double-click to edit.
			bool hasDelete=true;
			if(_listDiseaseDefsNumsNotDeletable.Contains(selectedDiseaseDef.DiseaseDefNum)) {
				hasDelete=false;
			}
			//everything below this point is _not_ selection mode.  User guaranteed to have permission for ProblemEdit.
			FormDiseaseDefEdit FormD=new FormDiseaseDefEdit(selectedDiseaseDef,hasDelete);
			FormD.ShowDialog();
			//Security log entry made inside that form.
			if(FormD.DialogResult!=DialogResult.OK) {
				return;
			}
			#endregion
			_listSecurityLogMsgs.Add(FormD.SecurityLogMsgText);
			if(FormD.DiseaseDefCur==null) {//User deleted the DiseaseDef.
				_listDiseaseDefs.Remove(selectedDiseaseDef);
			}
			IsChanged=true;
			FillGrid();
		}

		///<summary>Adds a new disease. New diseases get blank (not null) fields for ICD9, ICD10, and SnoMedCodes 
		///if they are not specified from FormDiseaseDefEdit so that we can do string searches on them.</summary>
		private void butAdd_Click(object sender,System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.ProblemEdit)) {
				return;
			}
			//initialise the new DiseaseDef with blank fields instead of null so we can filter on them.
			DiseaseDef def=new DiseaseDef() {
				ICD9Code="",
				Icd10Code="",
				SnomedCode="",
				ItemOrder=DiseaseDefs.GetCount()
			};
			FormDiseaseDefEdit FormD=new FormDiseaseDefEdit(def,true);//also sets ItemOrder correctly if using alphabetical during the insert diseaseDef call.
			FormD.IsNew=true;
			FormD.ShowDialog();
			if(FormD.DialogResult!=DialogResult.OK) {
				return;
			}
			_listSecurityLogMsgs.Add(FormD.SecurityLogMsgText);
			//Need to invalidate cache for selection mode so that the new problem shows up.
			if(IsSelectionMode) {
				DataValid.SetInvalid(InvalidType.Diseases);
			}
			//Items are already in the right order in the DB, re-order in memory list to match
			_listDiseaseDefs.FindAll(x => x.ItemOrder>=def.ItemOrder).ForEach(x => x.ItemOrder++);
			_listDiseaseDefs.Add(def);
			_listDiseaseDefs.Sort(DiseaseDefs.SortItemOrder);
			IsChanged=true;
			FillGrid();
		}

		///<summary>Only visible when !IsSelectionMode, and disabled if any filtering has been done via the search boxes. 
		///Resets ALL the DiseaseDefs' ItemOrders to be in alphabetical order. Not reversible once done.</summary>
		private void butAlphabetize_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Problems will be ordered alphabetically by description.  This cannot be undone.  Continue?")) {
				return;
			}
			_listDiseaseDefs.Sort(DiseaseDefs.SortAlphabetically);
			for(int i=0;i<_listDiseaseDefs.Count;i++) {
				_listDiseaseDefs[i].ItemOrder=i;
			}
			IsChanged=true;
			FillGrid();
		}

		///<summary>Only visible when !IsSelectionMode, and disabled if any filtering has been done via the search boxes. </summary>
		private void butUp_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item in the grid first.");
				return;
			}
			List<int> listSelectedIndexes=gridMain.SelectedIndices.ToList();
			if(listSelectedIndexes.First()==0) {
				return;
			}
			listSelectedIndexes.ForEach(x => _listDiseaseDefs.Reverse(x-1,2));
			for(int i=0;i<_listDiseaseDefs.Count;i++) {
				_listDiseaseDefs[i].ItemOrder=i;//change itemOrder to reflect order changes.
			}
			FillGrid();
			listSelectedIndexes.ForEach(x => gridMain.SetSelected(x-1,true));
			IsChanged=true;
		}

		///<summary>Only visible when !IsSelectionMode, and disabled if any filtering has been done via the search boxes. </summary>
		private void butDown_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item in the grid first.");
				return;
			}
			List<int> listSelectedIndexes=gridMain.SelectedIndices.ToList();
			if(listSelectedIndexes.Last()==_listDiseaseDefs.Count-1) {
				return;
			}
			listSelectedIndexes.Reverse<int>().ToList().ForEach(x => _listDiseaseDefs.Reverse(x,2));
			for(int i=0;i<_listDiseaseDefs.Count;i++) {
				_listDiseaseDefs[i].ItemOrder=i;//change itemOrder to reflect order changes.
			}
			FillGrid();
			listSelectedIndexes.ForEach(x => gridMain.SetSelected(x+1,true));
			IsChanged=true;
		}
		
		///<summary>Set at 500ms. Filtering only happens on timer-tick instead of on every keystroke.</summary>
		private void timerSearch_Tick(object sender,EventArgs e) {
			FillGrid();
			timerSearch.Stop();
		}

		private void textSearchBoxes_TextChanged(object sender,EventArgs e) {
			timerSearch.Stop();
			timerSearch.Start();
		}

		private void checkShowHidden_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
		}

		///<summary>Only visible when using Selection Mode. Most of the actual logic is done on FormClosing.</summary>
		private void butOK_Click(object sender,EventArgs e) {
			//not even visible unless IsSelectionMode
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item first.");
				return;
			}
			if(IsSelectionMode && !IsMultiSelect) {
				if(Snomeds.GetByCode(_listDiseaseDefs[gridMain.GetSelectedIndex()].SnomedCode)==null) {
					MsgBox.Show(this,"You have selected a problem containing an invalid SNOMED CT.");
					return;
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormDiseaseDefs_FormClosing(object sender,FormClosingEventArgs e) {
			if(IsChanged) {
				DiseaseDefs.Sync(_listDiseaseDefs,_listDiseaseDefsOld);//Update if anything has changed, even in selection mode.
				//old securitylog pattern pasted from FormDiseaseDefEdit
				_listSecurityLogMsgs.FindAll(x => !string.IsNullOrEmpty(x)).ForEach(x => SecurityLogs.MakeLogEntry(Permissions.ProblemEdit,0,x));
				DataValid.SetInvalid(InvalidType.Diseases);//refreshes cache
				_listDiseaseDefs=DiseaseDefs.GetDeepCopy(IsSelectionMode);//uses newly refreshed cache if changes were made
			}
			ListSelectedDiseaseDefs.Clear();
			for(int i=0;i < gridMain.SelectedIndices.Count();i++) {
				ListSelectedDiseaseDefs.Add((DiseaseDef)(gridMain.Rows[gridMain.SelectedIndices[i]].Tag));
			}
		}
	}
}



























