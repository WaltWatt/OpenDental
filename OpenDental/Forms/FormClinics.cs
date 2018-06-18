using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness; 
using CodeBase;

namespace OpenDental {
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormClinics:ODForm {
		private OpenDental.UI.Button butAdd;
		private OpenDental.UI.Button butClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label label1;
		private UI.ODGrid gridMain;
		///<summary>Set to true to open the form in selection mode. This will cause the 'Show hidden' checkbox to be hidden.</summary>
		public bool IsSelectionMode;
		public long SelectedClinicNum;
		private UI.Button butOK;
		private CheckBox checkOrderAlphabetical;
		///<summary>Set this list prior to loading this window to use a custom list of clinics.  Otherwise, uses the cache.</summary>
		public List<Clinic> ListClinics;
		///<summary>This list will be a copy of ListClinics and is used for syncing on window closed.</summary>
		public List<Clinic> ListClinicsOld;
		private CheckBox checkShowHidden;
		///<summary>A deep copy of all Clinic Specialty defs when this window loaded.  Used for display purposes.</summary>
		private List<Def> _listClinicSpecialtyDefs=new List<Def>();
		private GroupBox groupMovePats;
		private UI.Button butMovePats;
		private UI.Button butClinicPick;
		private TextBox textMoveTo;
		private Label label2;
		private GroupBox groupClinicOrder;
		private UI.Button butUp;
		private UI.Button butDown;
		private long _clinicNumTo=-1;
		///<summary>Set to true prior to loading to include a 'Headquarters' option.</summary>
		public bool IncludeHQInList;
		private SerializableDictionary<long,int> _dictClinicalCounts;
		private List<DefLink> _listClinicDefLinksAll;
		private List<DefLink> _listClinicDefLinksAllOld;
		///<summary>List of all DefLinkClinic helper objects so that we know which Specialties each clinic has.</summary>
		private List<DefLinkClinic> _listDefLinkClinicSpecialties;
		private UI.Button butSelectAll;
		private UI.Button butSelectNone;
		///<summary>Pass in a list of clinics that should be pre-selected. 
		///When this form is closed, this list will be the list of clinics that the user selected.</summary>
		public List<long> ListSelectedClinicNums = new List<long>();
		///<summary>Set to true if the user can select multiple clinics.</summary>
		public bool IsMultiSelect;

		///<summary></summary>
		public FormClinics()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClinics));
			this.groupClinicOrder = new System.Windows.Forms.GroupBox();
			this.butUp = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.checkOrderAlphabetical = new System.Windows.Forms.CheckBox();
			this.groupMovePats = new System.Windows.Forms.GroupBox();
			this.butMovePats = new OpenDental.UI.Button();
			this.butClinicPick = new OpenDental.UI.Button();
			this.textMoveTo = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.checkShowHidden = new System.Windows.Forms.CheckBox();
			this.butOK = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.label1 = new System.Windows.Forms.Label();
			this.butAdd = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.butSelectAll = new OpenDental.UI.Button();
			this.butSelectNone = new OpenDental.UI.Button();
			this.groupClinicOrder.SuspendLayout();
			this.groupMovePats.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupClinicOrder
			// 
			this.groupClinicOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupClinicOrder.Controls.Add(this.butUp);
			this.groupClinicOrder.Controls.Add(this.butDown);
			this.groupClinicOrder.Controls.Add(this.checkOrderAlphabetical);
			this.groupClinicOrder.Location = new System.Drawing.Point(642, 126);
			this.groupClinicOrder.Name = "groupClinicOrder";
			this.groupClinicOrder.Size = new System.Drawing.Size(266, 91);
			this.groupClinicOrder.TabIndex = 20;
			this.groupClinicOrder.TabStop = false;
			this.groupClinicOrder.Text = "Clinic Order";
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(6, 19);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(75, 26);
			this.butUp.TabIndex = 4;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(6, 54);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(75, 26);
			this.butDown.TabIndex = 5;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// checkOrderAlphabetical
			// 
			this.checkOrderAlphabetical.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkOrderAlphabetical.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkOrderAlphabetical.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkOrderAlphabetical.Location = new System.Drawing.Point(128, 40);
			this.checkOrderAlphabetical.Name = "checkOrderAlphabetical";
			this.checkOrderAlphabetical.Size = new System.Drawing.Size(132, 18);
			this.checkOrderAlphabetical.TabIndex = 16;
			this.checkOrderAlphabetical.Text = "Order Alphabetical";
			this.checkOrderAlphabetical.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkOrderAlphabetical.UseVisualStyleBackColor = true;
			this.checkOrderAlphabetical.Click += new System.EventHandler(this.checkOrderAlphabetical_Click);
			// 
			// groupMovePats
			// 
			this.groupMovePats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupMovePats.Controls.Add(this.butMovePats);
			this.groupMovePats.Controls.Add(this.butClinicPick);
			this.groupMovePats.Controls.Add(this.textMoveTo);
			this.groupMovePats.Controls.Add(this.label2);
			this.groupMovePats.Location = new System.Drawing.Point(642, 37);
			this.groupMovePats.Name = "groupMovePats";
			this.groupMovePats.Size = new System.Drawing.Size(266, 83);
			this.groupMovePats.TabIndex = 18;
			this.groupMovePats.TabStop = false;
			this.groupMovePats.Text = "Move Patients";
			// 
			// butMovePats
			// 
			this.butMovePats.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMovePats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butMovePats.Autosize = true;
			this.butMovePats.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMovePats.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMovePats.CornerRadius = 4F;
			this.butMovePats.Location = new System.Drawing.Point(185, 46);
			this.butMovePats.Name = "butMovePats";
			this.butMovePats.Size = new System.Drawing.Size(75, 26);
			this.butMovePats.TabIndex = 15;
			this.butMovePats.Text = "&Move";
			this.butMovePats.UseVisualStyleBackColor = true;
			this.butMovePats.Click += new System.EventHandler(this.butMovePats_Click);
			// 
			// butClinicPick
			// 
			this.butClinicPick.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClinicPick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butClinicPick.Autosize = true;
			this.butClinicPick.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClinicPick.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClinicPick.CornerRadius = 4F;
			this.butClinicPick.Location = new System.Drawing.Point(237, 20);
			this.butClinicPick.Name = "butClinicPick";
			this.butClinicPick.Size = new System.Drawing.Size(23, 20);
			this.butClinicPick.TabIndex = 23;
			this.butClinicPick.Text = "...";
			this.butClinicPick.Click += new System.EventHandler(this.butClinicPick_Click);
			// 
			// textMoveTo
			// 
			this.textMoveTo.Location = new System.Drawing.Point(98, 20);
			this.textMoveTo.MaxLength = 15;
			this.textMoveTo.Name = "textMoveTo";
			this.textMoveTo.ReadOnly = true;
			this.textMoveTo.Size = new System.Drawing.Size(135, 20);
			this.textMoveTo.TabIndex = 22;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(6, 21);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(91, 18);
			this.label2.TabIndex = 18;
			this.label2.Text = "To Clinic";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// checkShowHidden
			// 
			this.checkShowHidden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkShowHidden.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowHidden.Checked = true;
			this.checkShowHidden.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkShowHidden.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowHidden.Location = new System.Drawing.Point(512, 12);
			this.checkShowHidden.Name = "checkShowHidden";
			this.checkShowHidden.Size = new System.Drawing.Size(124, 18);
			this.checkShowHidden.TabIndex = 17;
			this.checkShowHidden.Text = "Show Hidden";
			this.checkShowHidden.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowHidden.UseVisualStyleBackColor = true;
			this.checkShowHidden.CheckedChanged += new System.EventHandler(this.checkShowHidden_CheckedChanged);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(833, 541);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 13;
			this.butOK.Text = "&OK";
			this.butOK.Visible = false;
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
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
			this.gridMain.Location = new System.Drawing.Point(12, 37);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(624, 562);
			this.gridMain.TabIndex = 0;
			this.gridMain.Title = "Clinics";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableClinics";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(351, 18);
			this.label1.TabIndex = 11;
			this.label1.Text = "This is usually only used if you have multiple locations";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(833, 223);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(75, 26);
			this.butAdd.TabIndex = 10;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(833, 573);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 1;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butSelectAll
			// 
			this.butSelectAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSelectAll.Autosize = true;
			this.butSelectAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSelectAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSelectAll.CornerRadius = 4F;
			this.butSelectAll.Location = new System.Drawing.Point(642, 311);
			this.butSelectAll.Name = "butSelectAll";
			this.butSelectAll.Size = new System.Drawing.Size(81, 26);
			this.butSelectAll.TabIndex = 21;
			this.butSelectAll.Text = "Select All";
			this.butSelectAll.UseVisualStyleBackColor = true;
			this.butSelectAll.Visible = false;
			this.butSelectAll.Click += new System.EventHandler(this.butSelectAll_Click);
			// 
			// butSelectNone
			// 
			this.butSelectNone.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSelectNone.Autosize = true;
			this.butSelectNone.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSelectNone.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSelectNone.CornerRadius = 4F;
			this.butSelectNone.Location = new System.Drawing.Point(642, 343);
			this.butSelectNone.Name = "butSelectNone";
			this.butSelectNone.Size = new System.Drawing.Size(81, 26);
			this.butSelectNone.TabIndex = 22;
			this.butSelectNone.Text = "Select None";
			this.butSelectNone.UseVisualStyleBackColor = true;
			this.butSelectNone.Visible = false;
			this.butSelectNone.Click += new System.EventHandler(this.butSelectNone_Click);
			// 
			// FormClinics
			// 
			this.ClientSize = new System.Drawing.Size(920, 611);
			this.Controls.Add(this.butSelectNone);
			this.Controls.Add(this.butSelectAll);
			this.Controls.Add(this.groupClinicOrder);
			this.Controls.Add(this.groupMovePats);
			this.Controls.Add(this.checkShowHidden);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(930, 650);
			this.Name = "FormClinics";
			this.ShowInTaskbar = false;
			this.Text = "Clinics";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormClinics_Closing);
			this.Load += new System.EventHandler(this.FormClinics_Load);
			this.groupClinicOrder.ResumeLayout(false);
			this.groupMovePats.ResumeLayout(false);
			this.groupMovePats.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormClinics_Load(object sender, System.EventArgs e) {
			if(ListClinics==null) {
				ListClinics=Clinics.GetAllForUserod(Security.CurUser);
			}
			if(_listClinicDefLinksAll==null) {
				_listClinicDefLinksAll=DefLinks.GetDefLinksByType(DefLinkType.Clinic);
			}
			ListClinicsOld=ListClinics.Select(x => x.Copy()).ToList();
			_listClinicDefLinksAllOld=_listClinicDefLinksAll.Select(x => x.Copy()).ToList();
			_listDefLinkClinicSpecialties=GetDefLinkClinicList();
			_listClinicSpecialtyDefs=Defs.GetDefsForCategory(DefCat.ClinicSpecialty);
			checkOrderAlphabetical.Checked=PrefC.GetBool(PrefName.ClinicListIsAlphabetical);
			_dictClinicalCounts=Clinics.GetClinicalPatientCount();
			if(IsSelectionMode) {
				butAdd.Visible=false;
				butOK.Visible=true;
				groupClinicOrder.Visible=false;
				groupMovePats.Visible=false;
				int widthDiff=(groupClinicOrder.Width-butOK.Width);
				this.MinimumSize=new Size(this.MinimumSize.Width-widthDiff,this.MinimumSize.Height);
				this.Width-=widthDiff;
				gridMain.Width+=widthDiff;
				checkShowHidden.Visible=false;
				checkShowHidden.Checked=false;
			}
			else {
				if(checkOrderAlphabetical.Checked) {
					butUp.Enabled=false;
					butDown.Enabled=false;
				}
				else {
					butUp.Enabled=true;
					butDown.Enabled=true;
				}
			}
			if(IsMultiSelect) {
				butSelectAll.Visible=true;
				butSelectNone.Visible=true;
				gridMain.SelectionMode=GridSelectionMode.MultiExtended;
			}
			FillGrid();
			for(int i = 0;i<gridMain.Rows.Count;i++) {
				if(ListSelectedClinicNums.Contains(((Clinic)gridMain.Rows[i].Tag).ClinicNum)) {
					gridMain.SetSelected(i,true);
				}
			}
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableClinics","Abbr"),120));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableClinics","Description"),200));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableClinics","Specialty"),150));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableClinics","Pat Count"),80,HorizontalAlignment.Center));
			if(!IsSelectionMode) {
				gridMain.Columns.Add(new ODGridColumn(Lan.g("TableClinics","Hidden"),0,HorizontalAlignment.Center));
			}
			gridMain.Rows.Clear();
			ODGridRow row;
			int patCount=0;
			if(IncludeHQInList) {
				row=new ODGridRow();
				row.Tag=new Clinic {//creating new clinic with Headquarters as description. The clinic will not get inserted into the db.
					ClinicNum=0,
					Description="Headquarters",
					Abbr="HQ"
				};//With a ClinicNum of 0, this will act as Headquarters.
				row.Cells.Add("");
				row.Cells.Add(Lan.g("TableClinics","Headquarters"));
				row.Cells.Add("");
				if(_dictClinicalCounts.ContainsKey(0)) {
					patCount=_dictClinicalCounts[0];
				}
				row.Cells.Add(POut.Int(patCount));
				if(!IsSelectionMode) {
					row.Cells.Add("");
				}
				gridMain.Rows.Add(row);
			}
			for(int i=0;i<ListClinics.Count;i++) {
				if(!checkShowHidden.Checked && ListClinics[i].IsHidden) {
					continue;
				}
				string specialty="";
				DefLinkClinic defLinkClinic=_listDefLinkClinicSpecialties.FirstOrDefault(x => x.Clinic.ClinicNum==ListClinics[i].ClinicNum);
				if(defLinkClinic!=null) {
					specialty=string.Join(",",defLinkClinic.ListDefLink.Select(x => Defs.GetName(DefCat.ClinicSpecialty,x.DefNum)));
				}
				row=new ODGridRow();
				row.Tag=ListClinics[i];
				row.Cells.Add(ListClinics[i].Abbr);
				row.Cells.Add(ListClinics[i].Description);
				row.Cells.Add(specialty);
				patCount=0;
				if(ListClinics[i].IsNew) {//a new clinic was just added
					patCount=0;
				}
				else if(_dictClinicalCounts.ContainsKey(ListClinics[i].ClinicNum)) {
					patCount=_dictClinicalCounts[ListClinics[i].ClinicNum];
				}
				row.Cells.Add(POut.Int(patCount));
				if(!IsSelectionMode) {
					row.Cells.Add(ListClinics[i].IsHidden ? "X" : "");
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		///<summary>Gets a list of DefLinkClinic helper objects for the current clinics.</summary>
		private List<DefLinkClinic> GetDefLinkClinicList() {
			List<DefLinkClinic> retVal=new List<DefLinkClinic>();
			foreach(Clinic clinic in ListClinics) {
				retVal.Add(new DefLinkClinic(clinic,_listClinicDefLinksAll.FindAll(x => x.FKey==clinic.ClinicNum).ToList()));
			}
			return retVal;
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			Clinic ClinicCur=new Clinic();
			ClinicCur.IsNew=true;
			DefLinkClinic defLinkClinic=new DefLinkClinic(ClinicCur,new List<DefLink>());
			if(PrefC.GetBool(PrefName.PracticeIsMedicalOnly)) {
				ClinicCur.IsMedicalOnly=true;
			}
			ClinicCur.ItemOrder=gridMain.Rows.Count;//Set it last in the last position.
			FormClinicEdit FormCE=new FormClinicEdit(ClinicCur,defLinkClinic);
			FormCE.IsNew=true;
			if(FormCE.ShowDialog()==DialogResult.OK) {
				ListClinics.Add(ClinicCur);
				_listDefLinkClinicSpecialties.Add(FormCE.DefLinkClinicSpecialties);
			}
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(gridMain.Rows.Count==0){
				return;
			}
			if(IsSelectionMode) {
				SelectedClinicNum=((Clinic)gridMain.Rows[e.Row].Tag).ClinicNum;
				DialogResult=DialogResult.OK;
				return;
			}
			if(IncludeHQInList && e.Row==0) {
				return;
			}
			Clinic clinic=(Clinic)gridMain.Rows[e.Row].Tag;
			DefLinkClinic defLinkClinic=_listDefLinkClinicSpecialties.Find(x=>x.Clinic.ClinicNum==clinic.ClinicNum);
			FormClinicEdit FormCE=new FormClinicEdit(((Clinic)gridMain.Rows[e.Row].Tag).Copy(),defLinkClinic);
			if(FormCE.ShowDialog()==DialogResult.OK) {
				if(FormCE.ClinicCur==null) {//Clinic was deleted
					//Fix ItemOrders
					for(int i=0;i<ListClinics.Count;i++) {
						if(ListClinics[i].ItemOrder>(clinic.ItemOrder)) {
							ListClinics[i].ItemOrder--;//Fix item orders
						}
					}
					ListClinics.Remove(clinic);
					//ListDefLinkClinic.Remove()
				}
				else { 
					ListClinics[ListClinics.IndexOf(clinic)]=FormCE.ClinicCur;
					defLinkClinic=_listDefLinkClinicSpecialties.Find(x=>x.Clinic.ClinicNum==clinic.ClinicNum);
					if(defLinkClinic!=null) {
						defLinkClinic=FormCE.DefLinkClinicSpecialties;
					}
				}
			}
			FillGrid();			
		}
		
		private void butClinicPick_Click(object sender,EventArgs e) {
			List<Clinic> listClinics=gridMain.Rows.Select(x => x.Tag as Clinic).ToList();
			FormClinics formC=new FormClinics();
			formC.ListClinics=listClinics;
			formC.IsSelectionMode=true;
			if(formC.ShowDialog()!=DialogResult.OK) {
				return;
			}
			_clinicNumTo=formC.SelectedClinicNum;
			textMoveTo.Text=listClinics.FirstOrDefault(x => x.ClinicNum==_clinicNumTo).Abbr;
		}

		private void butMovePats_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length<1) {
				MsgBox.Show(this,"You must select at least one clinic to move patients from.");
				return;
			}
			List<Clinic> listClinicsFrom=gridMain.SelectedIndices.OfType<int>().Select(x => (Clinic)gridMain.Rows[x].Tag).ToList();
			List<Clinic> listClinicsTo=gridMain.Rows.Select(x => x.Tag as Clinic).ToList();
			if(_clinicNumTo==-1){
				MsgBox.Show(this,"You must pick a 'To' clinic in the box above to move patients to.");
				return;
			}
			Clinic clinicTo=listClinicsTo.FirstOrDefault(x => x.ClinicNum==_clinicNumTo);
			if(clinicTo==null) {
				MsgBox.Show(this,"The clinic could not be found.");
				return;
			}
			Action actionCloseProgress=ODProgressOld.ShowProgressStatus("ClinicReassign",this,Lan.g(this,"Gathering patient data")+"...");
			Dictionary<long,List<long>> dictClinicPats=Patients.GetPatNumsByClinic(listClinicsFrom.Select(x => x.ClinicNum).ToList()).Select()
				.GroupBy(x => PIn.Long(x["ClinicNum"].ToString()),x => PIn.Long(x["PatNum"].ToString()))
				.ToDictionary(x => x.Key,x => x.ToList());
			actionCloseProgress?.Invoke();
			int totalPatCount=dictClinicPats.Sum(x => x.Value.Count);
			if(totalPatCount==0) {
				MsgBox.Show(this,"The selected clinics are not clinics for any patients.");
				return;
			}
			string strClinicFromDesc=string.Join(", ",listClinicsFrom.FindAll(x => dictClinicPats.ContainsKey(x.ClinicNum)).Select(x => (x.ClinicNum==0?"HQ":x.Abbr)));
			string strClinicToDesc=clinicTo.Abbr;
			string msg=Lan.g(this,"Move all patients to")+" "+strClinicToDesc+" "+Lan.g(this,"from the following clinics")+": "+strClinicFromDesc+"?";
			if(MessageBox.Show(msg,"",MessageBoxButtons.OKCancel)!=DialogResult.OK) {
				return;
			}
			actionCloseProgress=ODProgressOld.ShowProgressStatus("ClinicReassign",this,Lan.g(this,"Moving patients")+"...");
			int patsMoved=0;
			List<Action> listActions=dictClinicPats.Select(x => new Action(() => {
				patsMoved+=x.Value.Count;
				ODEvent.Fire(new ODEventArgs("ClinicReassign",Lan.g(this,"Moving patients")+": "+patsMoved+" out of "+totalPatCount));
				Patients.ChangeClinicsForAll(x.Key,clinicTo.ClinicNum);//update all clinicNums to new clinic
				SecurityLogs.MakeLogEntry(Permissions.PatientEdit,0,"Clinic changed for "+x.Value.Count+" patients from "
					+(x.Key==0 ? "HQ" : Clinics.GetAbbr(x.Key))+" to "+clinicTo.Abbr+".");
			})).ToList();
			ODThread.RunParallel(listActions,TimeSpan.FromMinutes(2));
			actionCloseProgress?.Invoke();
			_dictClinicalCounts=Clinics.GetClinicalPatientCount();
			FillGrid();
			MsgBox.Show(this,"Done");
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select a clinic first.");
				return;
			}
			//Already at the top of the list
			if(gridMain.GetSelectedIndex()==0) {
				return;
			}
			int selectedIdx=gridMain.GetSelectedIndex();
			//Swap clinic ItemOrders
			Clinic sourceClin = ((Clinic)gridMain.Rows[selectedIdx].Tag);
			Clinic destClin = ((Clinic)gridMain.Rows[selectedIdx-1].Tag);
			int sourceOrder=sourceClin.ItemOrder;
			sourceClin.ItemOrder = destClin.ItemOrder;
			destClin.ItemOrder = sourceOrder;
			//Move selected clinic up
			ListClinics.Sort(ClinicSort);
			FillGrid();
			//Reselect the clinic that was moved
			gridMain.SetSelected(selectedIdx-1,true);
			gridMain.SetSelected(selectedIdx,false);
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select a clinic first.");
				return;
			}
			//Already at the bottom of the list
			if(gridMain.GetSelectedIndex()==gridMain.Rows.Count-1) {
				return;
			}
			int selectedIdx=gridMain.GetSelectedIndex();
			//Swap clinic ItemOrders
			Clinic sourceClin = ((Clinic)gridMain.Rows[selectedIdx].Tag);
			Clinic destClin = ((Clinic)gridMain.Rows[selectedIdx+1].Tag);
			int sourceOrder=sourceClin.ItemOrder;
			sourceClin.ItemOrder = destClin.ItemOrder;
			destClin.ItemOrder = sourceOrder;
			//Move selected clinic down
			ListClinics.Sort(ClinicSort);
			FillGrid();
			//Reselect the clinic that was moved
			gridMain.SetSelected(selectedIdx+1,true);
			gridMain.SetSelected(selectedIdx,false);
		}

		private void checkOrderAlphabetical_Click(object sender,EventArgs e) {
			if(checkOrderAlphabetical.Checked) {
				butUp.Enabled=false;
				butDown.Enabled=false;
			}
			else {
				butUp.Enabled=true;
				butDown.Enabled=true;
			}
			ListClinics.Sort(ClinicSort);//Sorts based on the status of the checkbox.
			FillGrid();
		}

		private int ClinicSort(Clinic x,Clinic y) {
			if(checkOrderAlphabetical.Checked) {
				return x.Abbr.CompareTo(y.Abbr);
			}
			return x.ItemOrder.CompareTo(y.ItemOrder);
		}

		private void checkShowHidden_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void butSelectAll_Click(object sender,EventArgs e) {
			gridMain.SetSelected(true);
			gridMain.Focus();//Allows user to use ODGrid CTRL functionality.
		}

		private void butSelectNone_Click(object sender,EventArgs e) {
			gridMain.SetSelected(false);
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(IsSelectionMode && gridMain.SelectedIndices.Length>0) {
				SelectedClinicNum=((Clinic)gridMain.Rows[gridMain.GetSelectedIndex()].Tag).ClinicNum;
				ListSelectedClinicNums=gridMain.SelectedGridRows.Select(x => ((Clinic)x.Tag).ClinicNum).ToList();
				DialogResult=DialogResult.OK;
			}
			Close();
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			SelectedClinicNum=0;
			Close();
		}

		private void FormClinics_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(IsSelectionMode) {
				return;
			}
			bool hasClinicChanges=false;
			if(Clinics.Sync(ListClinics,ListClinicsOld)) {
				hasClinicChanges=true;
			}
			if(Prefs.UpdateBool(PrefName.ClinicListIsAlphabetical,checkOrderAlphabetical.Checked)) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			_listClinicDefLinksAll.Clear();
			foreach(DefLinkClinic defLinkClinic in _listDefLinkClinicSpecialties) {
				if(defLinkClinic.ListDefLink.Exists(x => x.DefLinkNum==0)) {
					defLinkClinic.ListDefLink.ForEach(x => x.FKey=defLinkClinic.Clinic.ClinicNum);
				}
				_listClinicDefLinksAll.AddRange(defLinkClinic.ListDefLink);
			}
			if(DefLinks.Sync(_listClinicDefLinksAll,_listClinicDefLinksAllOld)) {
				hasClinicChanges=true;
			}
			//Joe - Now that we have called sync on ListClinics we want to make sure that each clinic has program properties for PayConnect and XCharge
			//We are doing this because of a previous bug that caused some customers to have over 3.4 million duplicate rows in their programproperty table
			long payConnectProgNum=Programs.GetProgramNum(ProgramName.PayConnect);
			long xChargeProgNum=Programs.GetProgramNum(ProgramName.Xcharge);
			//Don't need to do this for PaySimple, because these will get generated as needed in FormPaySimpleSetup
			bool hasChanges=ProgramProperties.InsertForClinic(payConnectProgNum,
				ListClinics.Select(x => x.ClinicNum)
					.Where(x => ProgramProperties.GetListForProgramAndClinic(payConnectProgNum,x).Count==0).ToList());
			hasChanges=ProgramProperties.InsertForClinic(xChargeProgNum,
				ListClinics.Select(x => x.ClinicNum)
					.Where(x => ProgramProperties.GetListForProgramAndClinic(xChargeProgNum,x).Count==0).ToList()) || hasChanges;//prevent short curcuit
			if(hasChanges) {
				DataValid.SetInvalid(InvalidType.Programs);
			}
			if(hasClinicChanges) {
				DataValid.SetInvalid(InvalidType.Providers);
			}
		}
	}
}





















