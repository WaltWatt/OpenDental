/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.Bridges;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{

	///<summary></summary>
	public class ContrFamily : System.Windows.Forms.UserControl{
		private System.Windows.Forms.ImageList imageListToolBar;
		private System.ComponentModel.IContainer components;
		private OpenDental.UI.ODToolBar ToolBarMain;
		///<summary>All recalls for this entire family.</summary>
		private List<Recall> RecallList;
		private Patient PatCur;
		private PatientNote PatNoteCur;
		private Family FamCur;
		private OpenDental.UI.ODPictureBox picturePat;
		private List <InsPlan> PlanList;
		private OpenDental.UI.ODGrid gridIns;
		private List <PatPlan> PatPlanList;
		private ODGrid gridPat;
		private ContextMenu menuInsurance;
		private MenuItem menuPlansForFam;
		private List <Benefit> BenefitList;
		private ODGrid gridFamily;
		private ODGrid gridRecall;
		private PatField[] PatFieldList;
		private bool InitializedOnStartup;
		private ODGrid gridSuperFam;
		private List<InsSub> SubList;
		private List<Patient> SuperFamilyGuarantors;
		private List<Patient> SuperFamilyMembers;
		///<summary>Gets updated to PatCur.PatNum that the last security log was made with so that we don't make too many security logs for this patient.  When _patNumLast no longer matches PatCur.PatNum (e.g. switched to a different patient within a module), a security log will be entered.  Gets reset (cleared and the set back to PatCur.PatNum) any time a module button is clicked which will cause another security log to be entered.</summary>
		private long _patNumLast;
		private ContextMenu menuDiscount;
		private MenuItem menuItemRemoveDiscount;
		private SplitContainer splitSuperClones;
		private ODGrid gridPatientClones;
		private SortStrategy _superFamSortStrat;
		///<summary>Filled with all clones for the currently selected patient and their corresponding specialty.
		///Specialties are only important if clinics are enabled.  If clinics are disabled then the corresponding Def will be null.</summary>
		private Dictionary<Patient,Def> _dictCloneSpecialty;
		///<summary>All the data necessary to load the module.</summary>
		private FamilyModules.LoadData _loadData;

		///<summary></summary>
		public ContrFamily(){
			Logger.openlog.Log("Initializing family module...",Logger.Severity.INFO);
			InitializeComponent();// This call is required by the Windows.Forms Form Designer.
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

		#region Component Designer generated code

		private void InitializeComponent(){
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContrFamily));
			this.imageListToolBar = new System.Windows.Forms.ImageList(this.components);
			this.menuInsurance = new System.Windows.Forms.ContextMenu();
			this.menuPlansForFam = new System.Windows.Forms.MenuItem();
			this.menuDiscount = new System.Windows.Forms.ContextMenu();
			this.menuItemRemoveDiscount = new System.Windows.Forms.MenuItem();
			this.gridSuperFam = new OpenDental.UI.ODGrid();
			this.gridRecall = new OpenDental.UI.ODGrid();
			this.gridFamily = new OpenDental.UI.ODGrid();
			this.gridPat = new OpenDental.UI.ODGrid();
			this.gridIns = new OpenDental.UI.ODGrid();
			this.splitSuperClones = new System.Windows.Forms.SplitContainer();
			this.gridPatientClones = new OpenDental.UI.ODGrid();
			this.picturePat = new OpenDental.UI.ODPictureBox();
			this.ToolBarMain = new OpenDental.UI.ODToolBar();
			((System.ComponentModel.ISupportInitialize)(this.splitSuperClones)).BeginInit();
			this.splitSuperClones.Panel1.SuspendLayout();
			this.splitSuperClones.Panel2.SuspendLayout();
			this.splitSuperClones.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageListToolBar
			// 
			this.imageListToolBar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListToolBar.ImageStream")));
			this.imageListToolBar.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListToolBar.Images.SetKeyName(0, "");
			this.imageListToolBar.Images.SetKeyName(1, "");
			this.imageListToolBar.Images.SetKeyName(2, "");
			this.imageListToolBar.Images.SetKeyName(3, "");
			this.imageListToolBar.Images.SetKeyName(4, "");
			this.imageListToolBar.Images.SetKeyName(5, "");
			this.imageListToolBar.Images.SetKeyName(6, "Umbrella.gif");
			// 
			// menuInsurance
			// 
			this.menuInsurance.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuPlansForFam});
			// 
			// menuPlansForFam
			// 
			this.menuPlansForFam.Index = 0;
			this.menuPlansForFam.Text = "Plans for Family";
			this.menuPlansForFam.Click += new System.EventHandler(this.menuPlansForFam_Click);
			// 
			// menuDiscount
			// 
			this.menuDiscount.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemRemoveDiscount});
			// 
			// menuItemRemoveDiscount
			// 
			this.menuItemRemoveDiscount.Index = 0;
			this.menuItemRemoveDiscount.Text = "Drop Discount Plan";
			this.menuItemRemoveDiscount.Click += new System.EventHandler(this.menuItemRemoveDiscount_Click);
			// 
			// gridSuperFam
			// 
			this.gridSuperFam.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridSuperFam.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridSuperFam.HasAddButton = false;
			this.gridSuperFam.HasDropDowns = false;
			this.gridSuperFam.HasMultilineHeaders = false;
			this.gridSuperFam.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridSuperFam.HeaderHeight = 15;
			this.gridSuperFam.HScrollVisible = false;
			this.gridSuperFam.Location = new System.Drawing.Point(0, 0);
			this.gridSuperFam.Name = "gridSuperFam";
			this.gridSuperFam.ScrollValue = 0;
			this.gridSuperFam.Size = new System.Drawing.Size(329, 282);
			this.gridSuperFam.TabIndex = 33;
			this.gridSuperFam.Title = "Super Family";
			this.gridSuperFam.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridSuperFam.TitleHeight = 18;
			this.gridSuperFam.TranslationName = "TableSuper";
			this.gridSuperFam.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridSuperFam_CellDoubleClick);
			this.gridSuperFam.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridSuperFam_CellClick);
			// 
			// gridRecall
			// 
			this.gridRecall.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridRecall.HasAddButton = false;
			this.gridRecall.HasDropDowns = false;
			this.gridRecall.HasMultilineHeaders = false;
			this.gridRecall.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridRecall.HeaderHeight = 15;
			this.gridRecall.HScrollVisible = true;
			this.gridRecall.Location = new System.Drawing.Point(585, 27);
			this.gridRecall.Name = "gridRecall";
			this.gridRecall.ScrollValue = 0;
			this.gridRecall.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridRecall.Size = new System.Drawing.Size(525, 100);
			this.gridRecall.TabIndex = 32;
			this.gridRecall.Title = "Recall";
			this.gridRecall.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridRecall.TitleHeight = 18;
			this.gridRecall.TranslationName = "TableRecall";
			this.gridRecall.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridRecall_CellDoubleClick);
			this.gridRecall.DoubleClick += new System.EventHandler(this.gridRecall_DoubleClick);
			// 
			// gridFamily
			// 
			this.gridFamily.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridFamily.HasAddButton = false;
			this.gridFamily.HasDropDowns = false;
			this.gridFamily.HasMultilineHeaders = false;
			this.gridFamily.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridFamily.HeaderHeight = 15;
			this.gridFamily.HScrollVisible = false;
			this.gridFamily.Location = new System.Drawing.Point(103, 27);
			this.gridFamily.Name = "gridFamily";
			this.gridFamily.ScrollValue = 0;
			this.gridFamily.SelectedRowColor = System.Drawing.Color.DarkSalmon;
			this.gridFamily.Size = new System.Drawing.Size(480, 100);
			this.gridFamily.TabIndex = 31;
			this.gridFamily.Title = "Family Members";
			this.gridFamily.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridFamily.TitleHeight = 18;
			this.gridFamily.TranslationName = "TableFamily";
			this.gridFamily.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridFamily_CellDoubleClick);
			this.gridFamily.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridFamily_CellClick);
			// 
			// gridPat
			// 
			this.gridPat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridPat.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridPat.HasAddButton = false;
			this.gridPat.HasDropDowns = false;
			this.gridPat.HasMultilineHeaders = false;
			this.gridPat.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridPat.HeaderHeight = 15;
			this.gridPat.HScrollVisible = false;
			this.gridPat.Location = new System.Drawing.Point(0, 129);
			this.gridPat.Name = "gridPat";
			this.gridPat.ScrollValue = 0;
			this.gridPat.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridPat.Size = new System.Drawing.Size(252, 579);
			this.gridPat.TabIndex = 30;
			this.gridPat.Title = "Patient Information";
			this.gridPat.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridPat.TitleHeight = 18;
			this.gridPat.TranslationName = "TablePatient";
			this.gridPat.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPat_CellDoubleClick);
			this.gridPat.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPat_CellClick);
			// 
			// gridIns
			// 
			this.gridIns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridIns.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridIns.HasAddButton = false;
			this.gridIns.HasDropDowns = false;
			this.gridIns.HasMultilineHeaders = false;
			this.gridIns.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridIns.HeaderHeight = 15;
			this.gridIns.HScrollVisible = true;
			this.gridIns.Location = new System.Drawing.Point(254, 129);
			this.gridIns.Name = "gridIns";
			this.gridIns.ScrollValue = 0;
			this.gridIns.SelectionMode = OpenDental.UI.GridSelectionMode.None;
			this.gridIns.Size = new System.Drawing.Size(685, 579);
			this.gridIns.TabIndex = 29;
			this.gridIns.Title = "Insurance Plans";
			this.gridIns.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridIns.TitleHeight = 18;
			this.gridIns.TranslationName = "TableCoverage";
			this.gridIns.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridIns_CellDoubleClick);
			// 
			// splitSuperClones
			// 
			this.splitSuperClones.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.splitSuperClones.Location = new System.Drawing.Point(254, 129);
			this.splitSuperClones.Name = "splitSuperClones";
			this.splitSuperClones.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitSuperClones.Panel1
			// 
			this.splitSuperClones.Panel1.Controls.Add(this.gridSuperFam);
			// 
			// splitSuperClones.Panel2
			// 
			this.splitSuperClones.Panel2.Controls.Add(this.gridPatientClones);
			this.splitSuperClones.Size = new System.Drawing.Size(329, 579);
			this.splitSuperClones.SplitterDistance = 285;
			this.splitSuperClones.TabIndex = 34;
			// 
			// gridPatientClones
			// 
			this.gridPatientClones.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridPatientClones.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridPatientClones.HasAddButton = false;
			this.gridPatientClones.HasDropDowns = false;
			this.gridPatientClones.HasMultilineHeaders = false;
			this.gridPatientClones.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridPatientClones.HeaderHeight = 15;
			this.gridPatientClones.HScrollVisible = false;
			this.gridPatientClones.Location = new System.Drawing.Point(0, 0);
			this.gridPatientClones.Name = "gridPatientClones";
			this.gridPatientClones.ScrollValue = 0;
			this.gridPatientClones.Size = new System.Drawing.Size(329, 290);
			this.gridPatientClones.TabIndex = 34;
			this.gridPatientClones.Title = "Patient Clones";
			this.gridPatientClones.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridPatientClones.TitleHeight = 18;
			this.gridPatientClones.TranslationName = "TablePatientClones";
			this.gridPatientClones.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridPatientClone_CellClick);
			// 
			// picturePat
			// 
			this.picturePat.Location = new System.Drawing.Point(1, 27);
			this.picturePat.Name = "picturePat";
			this.picturePat.Size = new System.Drawing.Size(100, 100);
			this.picturePat.TabIndex = 28;
			this.picturePat.Text = "picturePat";
			this.picturePat.TextNullImage = "Patient Picture Unavailable";
			// 
			// ToolBarMain
			// 
			this.ToolBarMain.Dock = System.Windows.Forms.DockStyle.Top;
			this.ToolBarMain.ImageList = this.imageListToolBar;
			this.ToolBarMain.Location = new System.Drawing.Point(0, 0);
			this.ToolBarMain.Name = "ToolBarMain";
			this.ToolBarMain.Size = new System.Drawing.Size(939, 25);
			this.ToolBarMain.TabIndex = 19;
			this.ToolBarMain.ButtonClick += new OpenDental.UI.ODToolBarButtonClickEventHandler(this.ToolBarMain_ButtonClick);
			// 
			// ContrFamily
			// 
			this.Controls.Add(this.splitSuperClones);
			this.Controls.Add(this.gridRecall);
			this.Controls.Add(this.gridFamily);
			this.Controls.Add(this.gridPat);
			this.Controls.Add(this.gridIns);
			this.Controls.Add(this.picturePat);
			this.Controls.Add(this.ToolBarMain);
			this.Name = "ContrFamily";
			this.Size = new System.Drawing.Size(939, 708);
			this.Layout += new System.Windows.Forms.LayoutEventHandler(this.ContrFamily_Layout);
			this.Resize += new System.EventHandler(this.ContrFamily_Resize);
			this.splitSuperClones.Panel1.ResumeLayout(false);
			this.splitSuperClones.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitSuperClones)).EndInit();
			this.splitSuperClones.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		///<summary></summary>
		public void ModuleSelected(long patNum) {
			RefreshModuleData(patNum);
			RefreshModuleScreen();
			Plugins.HookAddCode(this,"ContrFamily.ModuleSelected_end",patNum);
		}

		///<summary></summary>
		public void ModuleUnselected(){
			FamCur=null;
			PlanList=null;
			_patNumLast=0;//Clear out the last pat num so that a security log gets entered that the module was "visited" or "refreshed".
			Plugins.HookAddCode(this,"ContrFamily.ModuleUnselected_end");
		}

		private void RefreshModuleData(long patNum) {
			if(patNum==0){
				PatCur=null;
				PatNoteCur=null;
				FamCur=null;
				PatPlanList=new List <PatPlan> (); 
				return;
			}
			bool doCreateSecLog=false;
			if(_patNumLast!=patNum) {
				doCreateSecLog=true;
				_patNumLast=patNum;//Stops module from making too many logs
			}
			_loadData=FamilyModules.GetLoadData(patNum,doCreateSecLog);
			FamCur=_loadData.Fam;
			PatCur=_loadData.Pat;
			PatNoteCur=_loadData.PatNote;
			SubList=_loadData.ListInsSubs;
			PlanList=_loadData.ListInsPlans;
			PatPlanList=_loadData.ListPatPlans;
			BenefitList=_loadData.ListBenefits;
			RecallList=_loadData.ListRecalls;
			PatFieldList=_loadData.ArrPatFields;
			SuperFamilyMembers=_loadData.SuperFamilyMembers;
			SuperFamilyGuarantors=_loadData.SuperFamilyGuarantors;
			_dictCloneSpecialty=_loadData.DictCloneSpecialities;
			//Takes the preference string and converts it to an enum object
			_superFamSortStrat=(SortStrategy)PrefC.GetInt(PrefName.SuperFamSortStrategy);
		}

		private void RefreshModuleScreen(){
			if(PatCur!=null){//if there is a patient
				//ToolBarMain.Buttons["Recall"].Enabled=true;
				ToolBarMain.Buttons["Add"].Enabled=true;
				ToolBarMain.Buttons["Delete"].Enabled=true;
				ToolBarMain.Buttons["Guarantor"].Enabled=true;
				ToolBarMain.Buttons["Move"].Enabled=true;
				if(ToolBarMain.Buttons["Ins"]!=null && !PrefC.GetBool(PrefName.EasyHideInsurance)) {
					ToolBarMain.Buttons["Ins"].Enabled=true;
					ToolBarMain.Buttons["Discount"].Enabled=true;
				}
				if(PatCur.SuperFamily!=0 || (_dictCloneSpecialty!=null && _dictCloneSpecialty.Count > 1)) {
					splitSuperClones.Visible=true;
					gridIns.Location=new Point(splitSuperClones.Right+2,gridIns.Top);
					gridIns.Width=this.Width-gridIns.Left;
				}
				else {
					splitSuperClones.Visible=false;
					gridIns.Location=splitSuperClones.Location;
					gridIns.Width=this.Width-gridIns.Left;
				}
				if(PrefC.GetBool(PrefName.ShowFeatureSuperfamilies) && ToolBarMain.Buttons["AddSuper"]!=null) {
					ToolBarMain.Buttons["AddSuper"].Enabled=true;
				}
				if(PatCur.SuperFamily==0 || ToolBarMain.Buttons["AddSuper"]==null) {
					splitSuperClones.Panel1Collapsed=true;
					splitSuperClones.Panel1.Hide();
				}
				else {
					splitSuperClones.Panel1Collapsed=false;
					splitSuperClones.Panel1.Show();
					ToolBarMain.Buttons["AddSuper"].Enabled=true;
					ToolBarMain.Buttons["RemoveSuper"].Enabled=true;
					ToolBarMain.Buttons["DisbandSuper"].Enabled=true;
				}
				if(PrefC.GetBool(PrefName.ShowFeaturePatientClone)
					&& ToolBarMain.Buttons["AddClone"]!=null)
				{
					ToolBarMain.Buttons["AddClone"].Enabled=true;
				}
				if(_dictCloneSpecialty!=null && _dictCloneSpecialty.Count > 1
					&& Patients.IsPatientACloneOrOriginal(PatCur.PatNum)
					&& ToolBarMain.Buttons["SynchClone"]!=null
					&& ToolBarMain.Buttons["BreakClone"]!=null)
				{
					ToolBarMain.Buttons["SynchClone"].Enabled=true;
					ToolBarMain.Buttons["BreakClone"].Enabled=true;
					splitSuperClones.Panel2Collapsed=false;
					splitSuperClones.Panel2.Show();
				}
				else {
					splitSuperClones.Panel2Collapsed=true;
					splitSuperClones.Panel2.Hide();
					if(ToolBarMain.Buttons["SynchClone"]!=null && ToolBarMain.Buttons["BreakClone"]!=null) {
						ToolBarMain.Buttons["SynchClone"].Enabled=false;
						ToolBarMain.Buttons["BreakClone"].Enabled=false;
					}
				}
				ToolBarMain.Invalidate();
			}
			else{//no patient selected
				//Hide super family and patient clone grids, safe to run even if panel is already hidden.
				splitSuperClones.Visible=false;
				gridIns.Location=splitSuperClones.Location;
				gridIns.Width=this.Width-gridIns.Left;
				ToolBarMain.Buttons["Add"].Enabled=false;
				ToolBarMain.Buttons["Delete"].Enabled=false;
				ToolBarMain.Buttons["Guarantor"].Enabled=false;
				ToolBarMain.Buttons["Move"].Enabled=false;
				if(ToolBarMain.Buttons["AddSuper"]!=null) {//because the toolbar only refreshes on restart.
					ToolBarMain.Buttons["AddSuper"].Enabled=false;
					ToolBarMain.Buttons["RemoveSuper"].Enabled=false;
					ToolBarMain.Buttons["DisbandSuper"].Enabled=false;
				}
				if(ToolBarMain.Buttons["Ins"]!=null && !PrefC.GetBool(PrefName.EasyHideInsurance)) {
					ToolBarMain.Buttons["Ins"].Enabled=false;
					ToolBarMain.Buttons["Discount"].Enabled=false;
				}
				if(PrefC.GetBool(PrefName.ShowFeaturePatientClone)
					&& ToolBarMain.Buttons["AddClone"]!=null
					&& ToolBarMain.Buttons["SynchClone"]!=null
					&& ToolBarMain.Buttons["BreakClone"]!=null)
				{
					ToolBarMain.Buttons["AddClone"].Enabled=false;
					ToolBarMain.Buttons["SynchClone"].Enabled=false;
					ToolBarMain.Buttons["BreakClone"].Enabled=false;
				}
				ToolBarMain.Invalidate();
			}
			if(PrefC.GetBool(PrefName.EasyHideInsurance)){
				gridIns.Visible=false;
			}
			else{
				gridIns.Visible=true;
			}
			//Cannot add new patients from OD select patient interface.  Patient must be added from HL7 message.
			if(HL7Defs.IsExistingHL7Enabled()) {
				HL7Def def=HL7Defs.GetOneDeepEnabled();
				if(def.ShowDemographics!=HL7ShowDemographics.ChangeAndAdd) {
					ToolBarMain.Buttons["Add"].Enabled=false;
					ToolBarMain.Buttons["Delete"].Enabled=false;
					if(PrefC.GetBool(PrefName.ShowFeaturePatientClone)
						&& ToolBarMain.Buttons["AddClone"]!=null
						&& ToolBarMain.Buttons["SynchClone"]!=null
						&& ToolBarMain.Buttons["BreakClone"]!=null)
					{
						ToolBarMain.Buttons["AddClone"].Enabled=false;
						ToolBarMain.Buttons["SynchClone"].Enabled=false;
						ToolBarMain.Buttons["BreakClone"].Enabled=false;
					}
				}
			}
			else {
				if(Programs.UsingEcwFullMode()) {
					ToolBarMain.Buttons["Add"].Enabled=false;
					ToolBarMain.Buttons["Delete"].Enabled=false;
					if(PrefC.GetBool(PrefName.ShowFeaturePatientClone)
						&& ToolBarMain.Buttons["AddClone"]!=null
						&& ToolBarMain.Buttons["SynchClone"]!=null
						&& ToolBarMain.Buttons["BreakClone"]!=null)
					{
						ToolBarMain.Buttons["AddClone"].Enabled=false;
						ToolBarMain.Buttons["SynchClone"].Enabled=false;
						ToolBarMain.Buttons["BreakClone"].Enabled=false;
					}
				}
			}
			FillPatientPicture();
			FillPatientData();
			FillFamilyData();
			FillGridRecall();
			FillInsData();
			FillGridSuperFam();
			FillGridPatientClones();
			Plugins.HookAddCode(this,"ContrFamily.RefreshModuleScreen_end");
		} 

		private void FillPatientPicture(){
			picturePat.Image=null;
			picturePat.TextNullImage=Lan.g(this,"Patient Picture Unavailable");
			if(PatCur==null || 
				PrefC.AtoZfolderUsed==DataStorageType.InDatabase){//Do not use patient image when A to Z folders are disabled.
				return;
			}
			try{
				Bitmap patPict;
				if(_loadData.HasPatPict==YN.Unknown) {
					Documents.GetPatPict(PatCur.PatNum,ImageStore.GetPatientFolder(PatCur,ImageStore.GetPreferredAtoZpath()),out patPict);
				}
				else {
					Documents.GetPatPict(PatCur.PatNum,ImageStore.GetPatientFolder(PatCur,ImageStore.GetPreferredAtoZpath()),_loadData.PatPict,out patPict);
				}
				picturePat.Image=patPict;
			}
			catch{
			}
		}

		///<summary></summary>
		public void InitializeOnStartup(){
			if(InitializedOnStartup) {
				return;
			}
			InitializedOnStartup=true;
			//tbFamily.InstantClasses();
			//cannot use Lan.F(this);
			Lan.C(this,new Control[]
				{
					//butPatEdit,
					//butEditPriCov,
					//butEditPriPlan,
					//butEditSecCov,
					//butEditSecPlan,
					gridFamily,
					gridRecall,
					gridPat,
					gridSuperFam,
					gridIns,
				});
			LayoutToolBar();
			//gridPat.Height=this.ClientRectangle.Bottom-gridPat.Top-2;
		}

		///<summary>Causes the toolbar to be laid out again.</summary>
		public void LayoutToolBar(){
			ToolBarMain.Buttons.Clear();
			ODToolBarButton button;
			//ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Recall"),1,"","Recall"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			button=new ODToolBarButton(Lan.g(this,"Family Members:"),-1,"","");
			button.Style=ODToolBarButtonStyle.Label;
			ToolBarMain.Buttons.Add(button);
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Add"),2,"Add Family Member","Add"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Delete"),3,Lan.g(this,"Delete Family Member"),"Delete"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Set Guarantor"),4,Lan.g(this,"Set as Guarantor"),"Guarantor"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Move"),5,Lan.g(this,"Move to Another Family"),"Move"));
			if(PrefC.GetBool(PrefName.ShowFeaturePatientClone)) {
				ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
				button=new ODToolBarButton(Lan.g(this,"Clones:"),-1,"","");
				button.Style=ODToolBarButtonStyle.Label;
				ToolBarMain.Buttons.Add(button);
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Add"),-1,Lan.g(this,"Creates a clone of the currently selected patient."),"AddClone"));
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Synch"),-1,Lan.g(this,"Synch information to the clone patient or create a clone of the currently selected patient if one does not exist"),"SynchClone"));
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Break"),-1,Lan.g(this,"Remove selected patient from the clone group."),"BreakClone"));
			}
			if(PrefC.GetBool(PrefName.ShowFeatureSuperfamilies)){
				ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
				button=new ODToolBarButton(Lan.g(this,"Super Family:"),-1,"","");
				button.Style=ODToolBarButtonStyle.Label;
				ToolBarMain.Buttons.Add(button);
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Add"),-1,"Add selected patient to a super family","AddSuper"));
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Remove"),-1,Lan.g(this,"Remove selected patient, and their family, from super family"),"RemoveSuper"));
				ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Disband"),-1,Lan.g(this,"Disband the current super family by removing all members of the super family."),"DisbandSuper"));
			}
			if(!PrefC.GetBool(PrefName.EasyHideInsurance)){
				ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
				button=new ODToolBarButton(Lan.g(this,"Add Insurance"),6,"","Ins");
				button.Style=ODToolBarButtonStyle.DropDownButton;
				button.DropDownMenu=menuInsurance;
				ToolBarMain.Buttons.Add(button);
				ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
				button=new ODToolBarButton(Lan.g(this,"Discount Plan"),-1,"","Discount");
				button.Style=ODToolBarButtonStyle.DropDownButton;
				button.DropDownMenu=menuDiscount;
				ToolBarMain.Buttons.Add(button);
			}
			ProgramL.LoadToolbar(ToolBarMain,ToolBarsAvail.FamilyModule);
			ToolBarMain.Invalidate();
			Plugins.HookAddCode(this,"ContrFamily.LayoutToolBar_end",PatCur);
		}

		private void ContrFamily_Layout(object sender, System.Windows.Forms.LayoutEventArgs e) {
			
		}

		private void ContrFamily_Resize(object sender,EventArgs e) {
			/*if(Height>gridPat.Top){
				gridPat.Height=Height-gridPat.Top-2;
				gridIns.Height=Height-gridIns.Top-2;
			}
			if(Width>gridIns.Left){
				gridIns.Width=Width-gridIns.Left-2;
			}*/
		}

		//private void butOutlook_Click(object sender, System.EventArgs e) {
			/*Process[] procsOutlook = Process.GetProcessesByName("outlook");
			if(procsOutlook.Length==0){
				try{
					Process.Start("Outlook");
				}
				catch{}
			}*/
		//}

		private void ToolBarMain_ButtonClick(object sender, OpenDental.UI.ODToolBarButtonClickEventArgs e) {
			if(e.Button.Tag.GetType()==typeof(string)){
				//standard predefined button
				switch(e.Button.Tag.ToString()){
					//case "Recall":
					//	ToolButRecall_Click();
					//	break;
					case "Add":
						ToolButAdd_Click();
						break;
					case "Delete":
						ToolButDelete_Click();
						break;
					case "Guarantor":
						ToolButGuarantor_Click();
						break;
					case "Move":
						ToolButMove_Click();
						break;
					case "Ins":
						ToolButIns_Click();
						break;
					case "Discount":
						ToolButDiscount_Click();
						break;
					case "AddSuper":
						ToolButAddSuper_Click();
						break;
					case "RemoveSuper":
						ToolButRemoveSuper_Click();
						break;
					case "DisbandSuper":
						ToolButDisbandSuper_Click();
						break;
					case "AddClone":
						ToolButAddClone_Click();
						break;
					case "SynchClone":
						ToolButSynchClone_Click();
						break;
					case "BreakClone":
						ToolButBreakClone_Click();
						break;
				}
			}
			else if(e.Button.Tag.GetType()==typeof(Program)) {
				ProgramL.Execute(((Program)e.Button.Tag).ProgramNum,PatCur);
			}
		}

		#region gridPatient

		private void gridPat_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(Plugins.HookMethod(this,"ContrFamily.gridPat_CellDoubleClick",PatCur)) {
				return;
			}
			if(TerminalActives.PatIsInUse(PatCur.PatNum)){
				MsgBox.Show(this,"Patient is currently entering info at a reception terminal.  Please try again later.");
				return;
			}
			if(gridPat.Rows[e.Row].Tag!=null){
				if(gridPat.Rows[e.Row].Tag.ToString()=="Referral"){
					//RefAttach refattach=(RefAttach)gridPat.Rows[e.Row].Tag;
					FormReferralsPatient FormRE=new FormReferralsPatient();
					FormRE.PatNum=PatCur.PatNum;
					FormRE.ShowDialog();
				}
				else if(gridPat.Rows[e.Row].Tag.ToString()=="References") {
					FormReference FormR=new FormReference();
					FormR.ShowDialog();
					if(FormR.GotoPatNum!=0) {
						Patient pat=Patients.GetPat(FormR.GotoPatNum);
						FormOpenDental.S_Contr_PatientSelected(pat,false);
						GotoModule.GotoFamily(FormR.GotoPatNum);
						return;
					}
					if(FormR.DialogResult!=DialogResult.OK) {
						return;
					}
					for(int i=0;i<FormR.SelectedCustRefs.Count;i++) {
						CustRefEntry custEntry=new CustRefEntry();
						custEntry.DateEntry=DateTime.Now;
						custEntry.PatNumCust=PatCur.PatNum;
						custEntry.PatNumRef=FormR.SelectedCustRefs[i].PatNum;
						CustRefEntries.Insert(custEntry);
					}
				}
				else if(gridPat.Rows[e.Row].Tag.GetType()==typeof(CustRefEntry)) {
					FormReferenceEntryEdit FormRE=new FormReferenceEntryEdit((CustRefEntry)gridPat.Rows[e.Row].Tag);
					FormRE.ShowDialog();
				}
				else if(gridPat.Rows[e.Row].Tag.ToString().Equals("Payor Types")) {
					FormPayorTypes FormPT = new FormPayorTypes();
					FormPT.PatCur=PatCur;
					FormPT.ShowDialog();
				}
				else if(gridPat.Rows[e.Row].Tag is PatFieldDef) {//patfield for an existing PatFieldDef
					PatFieldDef patFieldDef=(PatFieldDef)gridPat.Rows[e.Row].Tag;
					PatField field=PatFields.GetByName(patFieldDef.FieldName,PatFieldList);
					PatFieldL.OpenPatField(field,patFieldDef,PatCur.PatNum);
				}
				else if(gridPat.Rows[e.Row].Tag is PatField) {//PatField for a PatFieldDef that no longer exists
					PatField field=(PatField)gridPat.Rows[e.Row].Tag;
					FormPatFieldEdit FormPF=new FormPatFieldEdit(field);
					FormPF.ShowDialog();
				}
			}
			else{
				string email=PatCur.Email;
				long siteNum=PatCur.SiteNum;
				//
				FormPatientEdit FormP=new FormPatientEdit(PatCur,FamCur);
				FormP.IsNew=false;
				FormP.ShowDialog();
				//there are many things which may have changed that need to trigger refresh:
				//FName, LName, MiddleI, Preferred, SiteNum, or ChartNumber should refresh title bar.
				//Email change should change email but enabled.
				//Instead of checking for each of those:
				/*
				if(email!=PatCur.Email){//PatCur.EmailChanged){//do it this way later
					OnPatientSelected(PatCur.PatNum,PatCur.GetNameLF(),PatCur.Email!="",PatCur.ChartNumber);
				}
				if(siteNum!=PatCur.SiteNum){
					OnPatientSelected(PatCur.PatNum,PatCur.GetNameLF(),PatCur.Email!="",PatCur.ChartNumber);
				}*/
				if(FormP.DialogResult==DialogResult.OK) {
					FormOpenDental.S_Contr_PatientSelected(PatCur,false);
				}
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void gridPat_CellClick(object sender,ODGridClickEventArgs e) {
			ODGridCell gridCellCur=gridPat.Rows[e.Row].Cells[e.Col];
			//Only grid cells with phone numbers are blue and underlined. 
			//If we support color and underline in the future, this might be changed to a regex of the cell text.
			if(gridCellCur.ColorText==System.Drawing.Color.Blue && gridCellCur.Underline==YN.Yes && Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled) {
				DentalTek.PlaceCall(gridCellCur.Text);
			}
		}

		private void FillPatientData(){
			if(PatCur==null){
				gridPat.BeginUpdate();
				gridPat.Rows.Clear();
				gridPat.Columns.Clear();
				gridPat.EndUpdate();
				return;
			}
			gridPat.BeginUpdate();
			gridPat.Columns.Clear();
			ODGridColumn col=new ODGridColumn("",100);
			gridPat.Columns.Add(col);
			col=new ODGridColumn("",150);
			gridPat.Columns.Add(col);
			gridPat.Rows.Clear();
			ODGridRow row;
			List<DisplayField> fields=DisplayFields.GetForCategory(DisplayFieldCategory.PatientInformation);
			DisplayField fieldCur;
			List<Def> listMiscColorDefs=Defs.GetDefsForCategory(DefCat.MiscColors,true);
			for(int f=0;f<fields.Count;f++) {
				fieldCur=fields[f];
				row=new ODGridRow();
				#region Description Column
				if(fieldCur.Description==""){
					if(fieldCur.InternalName=="SS#"){
						if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
							row.Cells.Add("SIN");
						}
						else if(CultureInfo.CurrentCulture.Name.Length>=4 && CultureInfo.CurrentCulture.Name.Substring(3)=="GB") {
							row.Cells.Add("");
						}
						else{
							row.Cells.Add("SS#");
						}
					}
					else if(fieldCur.InternalName=="State"){
						if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
							row.Cells.Add("Province");
						}
						else if(CultureInfo.CurrentCulture.Name.Length>=4 && CultureInfo.CurrentCulture.Name.Substring(3)=="GB") {
							row.Cells.Add("");
						}
						else{
							row.Cells.Add("State");
						}
					}
					else if(fieldCur.InternalName=="Zip"){
						if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
							row.Cells.Add("Postal Code");
						}
						else if(CultureInfo.CurrentCulture.Name.Length>=4 && CultureInfo.CurrentCulture.Name.Substring(3)=="GB") {
							row.Cells.Add("Postcode");
						}
						else{
							row.Cells.Add(Lan.g("TablePatient","Zip"));
						}
					}
					else if(fieldCur.InternalName=="PatFields"){
						//don't add a cell
					}
					else{
						row.Cells.Add(fieldCur.InternalName);
					}
				}
				else{
					if(fieldCur.InternalName=="PatFields") {
						//don't add a cell
					}
					else {
						row.Cells.Add(fieldCur.Description);
					}
				}
				#endregion Description Column
				#region Value Column
				switch(fieldCur.InternalName){
					#region ABC0
					case "ABC0":
						row.Cells.Add(PatCur.CreditType);
						break;
					#endregion ABC0
					#region Addr/Ph Note
					case "Addr/Ph Note":
						row.Cells.Add(PatCur.AddrNote);
						if(PatCur.AddrNote!=""){
							row.ColorText=Color.Red;
							row.Bold=true;
						}
						break;
					#endregion Addr/Ph Note
					#region Address
					case "Address":
						row.Cells.Add(PatCur.Address);
						row.Bold=true;
						break;
					#endregion Address
					#region Address2
					case "Address2":
						row.Cells.Add(PatCur.Address2);
						break;
					#endregion Address2
					#region AdmitDate
					case "AdmitDate":
						row.Cells.Add(PatCur.AdmitDate.ToShortDateString());
						break;
					#endregion AdmitDate
					#region Age
					case "Age":
						row.Cells.Add(PatientLogic.DateToAgeString(PatCur.Birthdate,PatCur.DateTimeDeceased));
						break;
					#endregion Age
					#region Arrive Early
					case "Arrive Early":
						if(PatCur.AskToArriveEarly==0){
							row.Cells.Add("");
						}
						else{
							row.Cells.Add(PatCur.AskToArriveEarly.ToString());
						}
						break;
					#endregion Arrive Early
					#region Billing Type
					case "Billing Type":
						string billingtype=Defs.GetName(DefCat.BillingTypes,PatCur.BillingType);
						if(Defs.GetHidden(DefCat.BillingTypes,PatCur.BillingType)) {
							billingtype+=" "+Lan.g(this,"(hidden)");
						}						
						row.Cells.Add(billingtype);
						break;
					#endregion Billing Type
					#region Birthdate
					case "Birthdate":
						if(PatCur.Birthdate.Year < 1880){
							row.Cells.Add("");
						}
						else{
							row.Cells.Add(PatCur.Birthdate.ToString("d"));
						}
						break;
					#endregion Birthdate
					#region Chart Num
					case "Chart Num":
						row.Cells.Add(PatCur.ChartNumber);
						break;
					#endregion Chart Num
					#region City
					case "City":
						row.Cells.Add(PatCur.City);
						break;
					#endregion City
					#region Clinic
					case "Clinic":
						row.Cells.Add(Clinics.GetAbbr(PatCur.ClinicNum));
						break;
					#endregion Clinic
					#region Contact Method
					case "Contact Method":
						row.Cells.Add(PatCur.PreferContactMethod.ToString());
						if(PatCur.PreferContactMethod==ContactMethod.DoNotCall || PatCur.PreferContactMethod==ContactMethod.SeeNotes){
							row.Bold=true;
						}
						break;
					#endregion Contact Method
					#region Country
					case "Country":
						row.Cells.Add(PatCur.Country);
						break;
					#endregion Country
					#region E-mail
					case "E-mail":
						row.Cells.Add(PatCur.Email);
						if(PatCur.PreferContactMethod==ContactMethod.Email) {
							row.Bold=true;
						}
						break;
					#endregion E-mail
					#region First
					case "First":
						row.Cells.Add(PatCur.FName);
						break;
					#endregion First
					#region Gender
					case "Gender":
						row.Cells.Add(PatCur.Gender.ToString());
						break;
					#endregion Gender
					#region Guardians
					case "Guardians":
						List<Guardian> guardianList=_loadData.ListGuardians??Guardians.Refresh(PatCur.PatNum);
						string str="";
						for(int g=0;g<guardianList.Count;g++) {
							if(!guardianList[g].IsGuardian) {
								continue;
							}
							if(g>0) {
								str+=",";
							}
							str+=FamCur.GetNameInFamFirst(guardianList[g].PatNumGuardian)+Guardians.GetGuardianRelationshipStr(guardianList[g].Relationship);
						}
						row.Cells.Add(str);
						break;
					#endregion Guardians
					#region Hm Phone
					case "Hm Phone":
						row.Cells.Add(PatCur.HmPhone);
						if(PatCur.PreferContactMethod==ContactMethod.HmPhone || PatCur.PreferContactMethod==ContactMethod.None){
							row.Bold=true;
						}
						if(Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled) {
							row.Cells[row.Cells.Count-1].ColorText=Color.Blue;
							row.Cells[row.Cells.Count-1].Underline=YN.Yes;
						}
						break;
					#endregion Hm Phone
					#region ICE Name
					case "ICE Name":
						row.Cells.Add(PatNoteCur.ICEName);
						row.ColorBackG=Color.FromArgb(254,235,233);//light red;
						break;
					#endregion ICE Name
					#region ICE Phone
					case "ICE Phone":
						row.Cells.Add(PatNoteCur.ICEPhone);
						row.ColorBackG=Color.FromArgb(254,235,233);//light red;
						break;
					#endregion ICE Phone
					#region Language
					case "Language":
						if(PatCur.Language=="" || PatCur.Language==null){
							row.Cells.Add("");
						}
						else{
							try {
								row.Cells.Add(CodeBase.MiscUtils.GetCultureFromThreeLetter(PatCur.Language).DisplayName);
								//row.Cells.Add(CultureInfo.GetCultureInfo(PatCur.Language).DisplayName);
							}
							catch {
								row.Cells.Add(PatCur.Language);
							}
						}
						break;
					#endregion Language
					#region Last
					case "Last":
						row.Cells.Add(PatCur.LName);
						break;
					#endregion Last
					#region Middle
					case "Middle":
						row.Cells.Add(PatCur.MiddleI);
						break;
					#endregion Middle
					#region PatFields
					case "PatFields":
						PatFieldL.AddPatFieldsToGrid(gridPat,PatFieldList.ToList(),FieldLocations.Family,_loadData.ListPatFieldDefLinks);
						break;
					#endregion PatFields
					#region Pat Restrictions
					case "Pat Restrictions":
						List<PatRestriction> listPatRestricts=_loadData.ListPatRestricts??PatRestrictions.GetAllForPat(PatCur.PatNum);
						if(listPatRestricts.Count==0) {
							row.Cells.Add(Lan.g("TablePatient","None"));//row added outside of switch statement
						}
						for(int i=0;i<listPatRestricts.Count;i++) {
							row=new ODGridRow();
							if(string.IsNullOrWhiteSpace(fieldCur.Description)) {
								row.Cells.Add(fieldCur.InternalName);
							}
							else {
								row.Cells.Add(fieldCur.Description);
							}
							row.Cells.Add(PatRestrictions.GetPatRestrictDesc(listPatRestricts[i].PatRestrictType));
							row.ColorBackG=listMiscColorDefs[10].ItemColor;//index 10 is Patient Restrictions (hard coded in convertdatabase4)
							if(i==listPatRestricts.Count-1) {//last row added outside of switch statement
								break;
							}
							gridPat.Rows.Add(row);
						}
						break;
					#endregion Pat Restrictions
					#region Payor Types
					case "Payor Types":
						row.Tag="Payor Types";
						row.Cells.Add(_loadData.PayorTypeDesc??PayorTypes.GetCurrentDescription(PatCur.PatNum));
						break;
					#endregion Payor Types
					#region Position
					case "Position":
						row.Cells.Add(PatCur.Position.ToString());
						break;
					#endregion Position
					#region Preferred
					case "Preferred":
						row.Cells.Add(PatCur.Preferred);
						break;
					#endregion Preferred
					#region Primary Provider
					case "Primary Provider":
						if(PatCur.PriProv!=0) {
							row.Cells.Add(Providers.GetLongDesc(Patients.GetProvNum(PatCur)));
						}
						else {
							row.Cells.Add(Lan.g("TablePatient","None"));
						}
						break;
					#endregion Primary Provider
					#region References
					case "References":
						List<CustRefEntry> custREList=_loadData.ListCustRefEntries??CustRefEntries.GetEntryListForCustomer(PatCur.PatNum);
						if(custREList.Count==0) {
							row.Cells.Add(Lan.g("TablePatient","None"));
							row.Tag="References";
							row.ColorBackG=listMiscColorDefs[8].ItemColor;
						}
						else {
							row.Cells.Add(Lan.g("TablePatient",""));
							row.Tag="References";
							row.ColorBackG=listMiscColorDefs[8].ItemColor;
							gridPat.Rows.Add(row);
						}
						for(int i=0;i<custREList.Count;i++) {
							row=new ODGridRow();
							if(custREList[i].PatNumRef==PatCur.PatNum) {
								row.Cells.Add(custREList[i].DateEntry.ToShortDateString());
								row.Cells.Add("For: "+CustReferences.GetCustNameFL(custREList[i].PatNumCust));
							}
							else {
								row.Cells.Add("");
								row.Cells.Add(CustReferences.GetCustNameFL(custREList[i].PatNumRef));
							}
							row.Tag=custREList[i];
							row.ColorBackG=listMiscColorDefs[8].ItemColor;
							if(i<custREList.Count-1) {
								gridPat.Rows.Add(row);
							}
						}
						break;
					#endregion References
					#region Referrals
					case "Referrals":
						List<RefAttach> listRefs=_loadData.ListRefAttaches??RefAttaches.Refresh(PatCur.PatNum);
						List<RefAttach> listRefsFiltered= new List<RefAttach>();
						listRefsFiltered.AddRange(listRefs.Where(x => x.RefType==ReferralType.RefCustom).DistinctBy(x => x.ReferralNum).ToList());
						listRefsFiltered.AddRange(listRefs.Where(x => x.RefType==ReferralType.RefFrom).DistinctBy(x => x.ReferralNum).ToList());
						listRefsFiltered.AddRange(listRefs.Where(x => x.RefType==ReferralType.RefTo).DistinctBy(x => x.ReferralNum).ToList());
						listRefs=listRefsFiltered;
						if(listRefs.Count==0){
							row.Cells.Add(Lan.g("TablePatient","None"));
							row.Tag="Referral";
							row.ColorBackG=listMiscColorDefs[8].ItemColor;
						}
						//else{
						//	row.Cells.Add("");
						//	row.Tag="Referral";
						//	row.ColorBackG=listMiscColorDefs[8].ItemColor;
						//}
						for(int i=0;i<listRefs.Count;i++) {
							row=new ODGridRow();
							if(listRefs[i].RefType==ReferralType.RefFrom){
								row.Cells.Add(Lan.g("TablePatient","Referred From"));
							}
							else if(listRefs[i].RefType==ReferralType.RefTo) {
								row.Cells.Add(Lan.g("TablePatient","Referred To"));
							}
							else {
								if(!string.IsNullOrWhiteSpace(fieldCur.Description)) {
									row.Cells.Add(fieldCur.Description);
								}
								else {
									row.Cells.Add(Lan.g("TablePatient","Referral"));
								}
							}
							try{
								string refInfo=Referrals.GetNameLF(listRefs[i].ReferralNum);
								string phoneInfo=Referrals.GetPhone(listRefs[i].ReferralNum);
								if(phoneInfo!="" || listRefs[i].Note!=""){
									refInfo+="\r\n"+phoneInfo+" "+listRefs[i].Note;
								}
								row.Cells.Add(refInfo);
							}
							catch{
								row.Cells.Add("");//if referral is null because using random keys and had bug.
							}
							row.Tag="Referral";
							row.ColorBackG=listMiscColorDefs[8].ItemColor;
							if(i<listRefs.Count-1){
								gridPat.Rows.Add(row);
							}
						}
						break;
					#endregion Referrals
					#region ResponsParty
					case "ResponsParty":
						if(PatCur.ResponsParty==0){
							row.Cells.Add("");
						}
						else{
							row.Cells.Add((_loadData.ResponsibleParty??Patients.GetLim(PatCur.ResponsParty)).GetNameLF());
						}
						row.ColorBackG=listMiscColorDefs[8].ItemColor;
						break;
					#endregion ResponsParty
					#region Salutation
					case "Salutation":
						row.Cells.Add(PatCur.Salutation);
						break;
					#endregion Salutation
					#region Sec. Provider
					case "Sec. Provider":
						if(PatCur.SecProv != 0){
							row.Cells.Add(Providers.GetLongDesc(PatCur.SecProv));
						}
						else{
							row.Cells.Add(Lan.g("TablePatient","None"));
						}
						break;
					#endregion Sec. Provider
					#region SS#
					case "SS#":
						if(CultureInfo.CurrentCulture.Name.Length>=4 && CultureInfo.CurrentCulture.Name.Substring(3)=="US" 
							&& PatCur.SSN !=null && PatCur.SSN.Length==9)
						{
							row.Cells.Add(PatCur.SSN.Substring(0,3)+"-"+PatCur.SSN.Substring(3,2)+"-"+PatCur.SSN.Substring(5,4));
						}
						else {
							row.Cells.Add(PatCur.SSN);
						}
						break;
					#endregion SS#
					#region State
					case "State":
						row.Cells.Add(PatCur.State);
						break;
					#endregion State
					#region Status
					case "Status":
						row.Cells.Add(PatCur.PatStatus.ToString());
						if(PatCur.PatStatus==PatientStatus.Deceased) {
							row.ColorText=Color.Red;
						}
						break;
					#endregion Status
					#region Super Head
					case "Super Head":
						string fieldVal="";
						if(PatCur.SuperFamily!=0) {
							Patient supHead=_loadData.SuperFamilyGuarantors.FirstOrDefault(x => x.PatNum==PatCur.SuperFamily)??Patients.GetPat(PatCur.SuperFamily);
							fieldVal=supHead.GetNameLF()+" ("+supHead.PatNum+")";
						}
						row.Cells.Add(fieldVal);
						break;
					#endregion Super Head
					#region Title
					case "Title":
						row.Cells.Add(PatCur.Title);
						break;
					#endregion Title
					#region Ward
					case "Ward":
						row.Cells.Add(PatCur.Ward);
						break;
					#endregion Ward
					#region Wireless Ph
					case "Wireless Ph":
						row.Cells.Add(PatCur.WirelessPhone);
						if(PatCur.PreferContactMethod==ContactMethod.WirelessPh) {
							row.Bold=true;
						}
						if(Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled) {
							row.Cells[row.Cells.Count-1].ColorText=Color.Blue;
							row.Cells[row.Cells.Count-1].Underline=YN.Yes;
						}
						break;
					#endregion Wireless Ph
					#region Wk Phone
					case "Wk Phone":
						row.Cells.Add(PatCur.WkPhone);
						if(PatCur.PreferContactMethod==ContactMethod.WkPhone) {
							row.Bold=true;
						}
						if(Programs.GetCur(ProgramName.DentalTekSmartOfficePhone).Enabled) {
							row.Cells[row.Cells.Count-1].ColorText=Color.Blue;
							row.Cells[row.Cells.Count-1].Underline=YN.Yes;
						}
						break;
					#endregion Wk Phone
					#region Zip
					case "Zip":
						row.Cells.Add(PatCur.Zip);
						break;
					#endregion Zip
				}
				#endregion Value Column
				if(fieldCur.InternalName=="PatFields"){
					//don't add the row here
				}
				else{
					gridPat.Rows.Add(row);
				}
			}
			gridPat.EndUpdate();
		}

		#endregion gridPatient 

		#region gridFamily

		private void FillFamilyData(){
			gridFamily.BeginUpdate();
			gridFamily.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TablePatient","Name"),140);
			gridFamily.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePatient","Position"),65);
			gridFamily.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePatient","Gender"),55);
			gridFamily.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePatient","Status"),65);
			gridFamily.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePatient","Age"),45);
			gridFamily.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePatient","Recall Due"),80);
			gridFamily.Columns.Add(col);
			gridFamily.Rows.Clear();
			if(PatCur==null){
				gridFamily.EndUpdate();
				return;
			}
			ODGridRow row;
			DateTime recallDate;
			ODGridCell cell;
			int selectedRow=-1;
			for(int i=0;i<FamCur.ListPats.Length;i++){
				if(PatientLinks.WasPatientMerged(FamCur.ListPats[i].PatNum,_loadData.ListMergeLinks) && FamCur.ListPats[i].PatNum!=PatCur.PatNum) {
					//Hide merged patients so that new things don't get added to them. If the user really wants to find this patient, they will have to use 
					//the Select Patient window.
					continue;
				}
				row=new ODGridRow();
				row.Cells.Add(FamCur.GetNameInFamLFI(i));
				row.Cells.Add(Lan.g("enumPatientPosition",FamCur.ListPats[i].Position.ToString()));
				row.Cells.Add(Lan.g("enumPatientGender",FamCur.ListPats[i].Gender.ToString()));
				row.Cells.Add(Lan.g("enumPatientStatus",FamCur.ListPats[i].PatStatus.ToString()));
				row.Cells.Add(Patients.AgeToString(FamCur.ListPats[i].Age));
				recallDate=DateTime.MinValue;
				for(int j=0;j<RecallList.Count;j++){
					if(RecallList[j].PatNum==FamCur.ListPats[i].PatNum
						&& (RecallList[j].RecallTypeNum==PrefC.GetLong(PrefName.RecallTypeSpecialProphy)
						|| RecallList[j].RecallTypeNum==PrefC.GetLong(PrefName.RecallTypeSpecialPerio)))
					{
						recallDate=RecallList[j].DateDue;
					}
				}
				cell=new ODGridCell();
				if(recallDate.Year>1880){
					cell.Text=recallDate.ToShortDateString();
					if(recallDate<DateTime.Today){
						cell.Bold=YN.Yes;
						cell.ColorText=Color.Firebrick;
					}
				}
				row.Cells.Add(cell);
				if(i==0){//guarantor
					row.Bold=true;
				}
				row.Tag=FamCur.ListPats[i];
				int idx=gridFamily.Rows.Add(row);
				if(FamCur.ListPats[i].PatNum==PatCur.PatNum) {
					selectedRow=idx;
				}
			}
			gridFamily.EndUpdate();
			gridFamily.SetSelected(selectedRow,true);
		}

		private void gridFamily_CellClick(object sender,ODGridClickEventArgs e) {
			FormOpenDental.S_Contr_PatientSelected(gridFamily.SelectedTag<Patient>(),false);
			ModuleSelected(gridFamily.SelectedTag<Patient>().PatNum);
		}

		private void gridFamily_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormPatientEdit FormP=new FormPatientEdit(PatCur,FamCur);
			FormP.IsNew=false;
			FormP.ShowDialog();
			if(FormP.DialogResult==DialogResult.OK) {
				FormOpenDental.S_Contr_PatientSelected(PatCur,false);
			}
			ModuleSelected(PatCur.PatNum);
		}

		//private void butAddPt_Click(object sender, System.EventArgs e) {
		private void ToolButAdd_Click() {
			//At HQ, we cannot allow users to add patients to reseller families.
			if(PrefC.GetBool(PrefName.DockPhonePanelShow) && Resellers.IsResellerFamily(PatCur.Guarantor)) {
				MsgBox.Show(this,"Cannot add patients to a reseller family.");
				return;
			}
			Patient tempPat=new Patient();
			tempPat.LName      =PatCur.LName;
			tempPat.PatStatus  =PatientStatus.Patient;
			tempPat.Gender     =PatientGender.Unknown;
			tempPat.Address    =PatCur.Address;
			tempPat.Address2   =PatCur.Address2;
			tempPat.City       =PatCur.City;
			tempPat.State      =PatCur.State;
			tempPat.Zip        =PatCur.Zip;
			tempPat.HmPhone    =PatCur.HmPhone;
			tempPat.Guarantor  =PatCur.Guarantor;
			tempPat.CreditType =PatCur.CreditType;
			if(!PrefC.GetBool(PrefName.PriProvDefaultToSelectProv)) {
				tempPat.PriProv  =PatCur.PriProv;
			}
			tempPat.SecProv    =PatCur.SecProv;
			tempPat.FeeSched   =PatCur.FeeSched;
			tempPat.BillingType=PatCur.BillingType;
			tempPat.AddrNote   =PatCur.AddrNote;
			tempPat.ClinicNum  =PatCur.ClinicNum;//this is probably better in case they don't have user.ClinicNums set.
			//tempPat.ClinicNum  =Security.CurUser.ClinicNum;
			if(Patients.GetPat(tempPat.Guarantor).SuperFamily!=0) {
				tempPat.SuperFamily=PatCur.SuperFamily;
			}
			Patients.Insert(tempPat,false);
			SecurityLogs.MakeLogEntry(Permissions.PatientCreate,tempPat.PatNum,"Created from Family Module Add button.");
			CustReference custRef=new CustReference();
			custRef.PatNum=tempPat.PatNum;
			CustReferences.Insert(custRef);
			//add the tempPat to the FamCur list, but ModuleSelected below will refill the FamCur list in case the user cancels and tempPat is deleted
			//This would be a faster way to add to the array, but since it is not a pattern that is used anywhere we will use the alternate method of
			//creating a list, adding the patient, and converting back to an array
			//Array.Resize(ref FamCur.ListPats,FamCur.ListPats.Length+1);
			//FamCur.ListPats[FamCur.ListPats.Length-1]=tempPat;
			//Adding the temp patient to the FamCur.ListPats without calling GetFamily which makes a call to the db
			List<Patient> listPatsTemp=FamCur.ListPats.ToList();
			listPatsTemp.Add(tempPat);
			FamCur.ListPats=listPatsTemp.ToArray();
			FormPatientEdit FormPE=new FormPatientEdit(tempPat,FamCur);
			FormPE.IsNew=true;
			FormPE.ShowDialog();
			if(FormPE.DialogResult==DialogResult.OK){
				FormOpenDental.S_Contr_PatientSelected(tempPat,false);
				ModuleSelected(tempPat.PatNum);
			}
			else{
				ModuleSelected(PatCur.PatNum);
			}
		}

		//private void butDeletePt_Click(object sender, System.EventArgs e) {
		private void ToolButDelete_Click(){
			//this doesn't actually delete the patient, just changes their status
			//and they will never show again in the patient selection list.
			//check for plans, appointments, procedures, etc.
			List<Procedure> procList=Procedures.Refresh(PatCur.PatNum);
			List<Appointment> apptList=Appointments.GetListForPat(PatCur.PatNum);
			List<Claim> claimList=Claims.Refresh(PatCur.PatNum);
			Adjustment[] AdjustmentList=Adjustments.Refresh(PatCur.PatNum);
			PaySplit[] PaySplitList=PaySplits.Refresh(PatCur.PatNum);//
			List<ClaimProc> claimProcList=ClaimProcs.Refresh(PatCur.PatNum);
			List<Commlog> commlogList=Commlogs.Refresh(PatCur.PatNum);
			int payPlanCount=PayPlans.GetDependencyCount(PatCur.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(FamCur);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<MedicationPat> medList=MedicationPats.Refresh(PatCur.PatNum,false);
			PatPlanList=PatPlans.Refresh(PatCur.PatNum);
			//CovPats.Refresh(planList,PatPlanList);
			List<RefAttach> RefAttachList=RefAttaches.Refresh(PatCur.PatNum);
			List<Sheet> sheetList=Sheets.GetForPatient(PatCur.PatNum);
			RepeatCharge[] repeatChargeList=RepeatCharges.Refresh(PatCur.PatNum);
			List<CreditCard> listCreditCards=CreditCards.Refresh(PatCur.PatNum);
			RegistrationKey[] arrayRegistrationKeys=RegistrationKeys.GetForPatient(PatCur.PatNum);
			List<long> listPatNumClones=Patients.GetClonePatNumsAll(PatCur.PatNum);
			bool hasProcs=procList.Count>0;
			bool hasAppt=apptList.Count>0;
			bool hasClaims=claimList.Count>0;
			bool hasAdj=AdjustmentList.Length>0;
			bool hasPay=PaySplitList.Length>0;
			bool hasClaimProcs=claimProcList.Count>0;
			bool hasComm=commlogList.Count>0;
			bool hasPayPlans=payPlanCount>0;
			bool hasInsPlans=false;
			bool hasMeds=medList.Count>0;
			bool isSuperFamilyHead=PatCur.PatNum==PatCur.SuperFamily;
			for(int i=0;i<subList.Count;i++) {
				if(subList[i].Subscriber==PatCur.PatNum) {
					hasInsPlans=true;
				}
			}
			bool hasRef=RefAttachList.Count>0;
			bool hasSheets=sheetList.Count>0;
			bool hasRepeat=repeatChargeList.Length>0;
			bool hasCC=listCreditCards.Count>0;
			bool hasRegKey=arrayRegistrationKeys.Length>0;
			bool hasPerio = PerioExams.GetExamsTable(PatCur.PatNum).Rows.Count>0;
			bool hasClones=(listPatNumClones.Count > 1);//The list of "clones for all" will always include the current patient.
			if(hasProcs || hasAppt || hasClaims || hasAdj || hasPay || hasClaimProcs || hasComm || hasPayPlans
				|| hasInsPlans || hasRef || hasMeds || isSuperFamilyHead || hasSheets || hasRepeat || hasCC || hasRegKey || hasPerio || hasClones) 
			{
				string message=Lan.g(this,"You cannot delete this patient without first deleting the following data:")+"\r";
				if(hasProcs) {
					message+=Lan.g(this,"Procedures")+"\r";
				}
				if(hasAppt) {
					message+=Lan.g(this,"Appointments")+"\r";
				}
				if(hasClaims) {
					message+=Lan.g(this,"Claims")+"\r";
				}
				if(hasAdj) {
					message+=Lan.g(this,"Adjustments")+"\r";
				}
				if(hasPay) {
					message+=Lan.g(this,"Payments")+"\r";
				}
				if(hasClaimProcs) {
					message+=Lan.g(this,"Procedures attached to claims")+"\r";
				}
				if(hasComm) {
					message+=Lan.g(this,"Commlog entries")+"\r";
				}
				if(hasPayPlans) {
					message+=Lan.g(this,"Payment plans")+"\r";
				}
				if(hasInsPlans) {
					message+=Lan.g(this,"Insurance plans")+"\r";
				}
				if(hasRef) {
					message+=Lan.g(this,"References")+"\r";
				}
				if(hasMeds) {
					message+=Lan.g(this,"Medications")+"\r";
				}
				if(isSuperFamilyHead) {
					message+=Lan.g(this,"Attached Super Family")+"\r";
				}
				if(hasSheets) {
					message+=Lan.g(this,"Sheets")+"\r";
				}
				if(hasRepeat) {
					message+=Lan.g(this,"Repeating Charges")+"\r";
				}
				if(hasCC) {
					message+=Lan.g(this,"Credit Cards")+"\r";
				}
				if(hasRegKey) {
					message+=Lan.g(this,"Registration Keys")+"\r";
				}
				if(hasPerio) {
					message+=Lan.g(this,"Perio Chart")+"\r";
				}
				if(hasClones) {
					message+=Lan.g(this,"Attached Clones")+"\r";
				}
				MessageBox.Show(message);
				return;
			}
			Patient PatOld=PatCur.Copy();
			if(PatCur.PatNum==PatCur.Guarantor){//if selecting guarantor
				if(FamCur.ListPats.Length==1){
					if(!MsgBox.Show(this,true,"Delete Patient?")) {
						return;
					}
					PatCur.PatStatus=PatientStatus.Deleted;
					PatCur.ChartNumber="";
					PatCur.ClinicNum=0;
					PatCur.FeeSched=0;
					Popups.MoveForDeletePat(PatCur);
					PatCur.SuperFamily=0;
					Patients.Update(PatCur,PatOld);
					for(int i=0;i<RecallList.Count;i++){
						if(RecallList[i].PatNum==PatCur.PatNum){
							RecallList[i].IsDisabled=true;
							RecallList[i].DateDue=DateTime.MinValue;
							Recalls.Update(RecallList[i]);
						}
					}
					SecurityLogs.MakeLogEntry(Permissions.PatientEdit,PatOld.PatNum,"Patient deleted");
					FormOpenDental.S_Contr_PatientSelected(new Patient(),false);
					ModuleSelected(0);
					//does not delete notes or plans, etc.
				}
				else{
					MessageBox.Show(Lan.g(this,"You cannot delete the guarantor if there are other family members. You would have to make a different family member the guarantor first."));
				}
			}
			else{//not selecting guarantor
				if(!MsgBox.Show(this,true,"Delete Patient?")) {
					return;
				}
				PatCur.PatStatus=PatientStatus.Deleted;
				PatCur.ChartNumber="";
				PatCur.ClinicNum=0;
				PatCur.FeeSched=0;
				Popups.MoveForDeletePat(PatCur);
				PatCur.Guarantor=PatCur.PatNum;
				PatCur.SuperFamily=0;
				Patients.Update(PatCur,PatOld);
				for(int i=0;i<RecallList.Count;i++){
					if(RecallList[i].PatNum==PatCur.PatNum){
						RecallList[i].IsDisabled=true;
						RecallList[i].DateDue=DateTime.MinValue;
						Recalls.Update(RecallList[i]);
					}
				}
				SecurityLogs.MakeLogEntry(Permissions.PatientEdit,PatOld.PatNum,"Patient deleted");
				ModuleSelected(PatOld.Guarantor);//Sets PatCur to PatOld guarantor.
				FormOpenDental.S_Contr_PatientSelected(PatCur,false);//PatCur is now the Guarantor.
			}
			PatientL.RemoveFromMenu(PatOld.GetNameLF(),PatOld.PatNum);//Always remove deleted patients from the dropdown menu.
		}

		//private void butSetGuar_Click(object sender,System.EventArgs e){
		private void ToolButGuarantor_Click() {
			if(PatCur.PatNum==PatCur.Guarantor) {
				MessageBox.Show(Lan.g(this,"Patient is already the guarantor.  Please select a different family member."));
				return;
			}
			//At HQ, we cannot allow users to change the guarantor of reseller families.
			if(PrefC.GetBool(PrefName.DockPhonePanelShow) && Resellers.IsResellerFamily(PatCur.Guarantor)) {
				MsgBox.Show(this,"Cannot change the guarantor of a reseller family.");
				return;
			}
			if(MessageBox.Show(Lan.g(this,"Make the selected patient the guarantor?")
				,"",MessageBoxButtons.OKCancel)!=DialogResult.OK) {
				return;
			}
			if(PatCur.SuperFamily==PatCur.Guarantor) {//guarantor is also the head of a super family
				Patients.MoveSuperFamily(PatCur.SuperFamily,PatCur.PatNum);
			}
			Patients.ChangeGuarantorToCur(FamCur,PatCur);
			ModuleSelected(PatCur.PatNum);
		}

		//private void butMovePat_Click(object sender, System.EventArgs e) {
		private void ToolButMove_Click() {
			//At HQ, we cannot allow users to move patients of reseller families.
			if(PrefC.GetBool(PrefName.DockPhonePanelShow) && Resellers.IsResellerFamily(PatCur.Guarantor)) {
				MsgBox.Show(this,"Cannot move patients of a reseller family.");
				return;
			}
			Patient PatOld=PatCur.Copy();
			//Patient PatCur;
			if(PatCur.PatNum==PatCur.Guarantor){//if guarantor selected
				if(FamCur.ListPats.Length==1){//and no other family members
					//no need to check insurance.  It will follow.
					if(!MsgBox.Show(this,true,"Moving the guarantor will cause two families to be combined.  The financial notes for both families will be combined and may need to be edited.  The address notes will also be combined and may need to be edited. Do you wish to continue?")) {
						return;
					}
					if(!MsgBox.Show(this,true,"Select the family to move this patient to from the list that will come up next.")) {
						return;
					}
					FormPatientSelect FormPS=new FormPatientSelect();
					FormPS.SelectionModeOnly=true;
					FormPS.ShowDialog();
					if(FormPS.DialogResult!=DialogResult.OK){
						return;
					}
					Patient patInNewFam=Patients.GetPat(FormPS.SelectedPatNum);
					if(PatCur.SuperFamily!=patInNewFam.SuperFamily){//If they are moving into or out of a superfamily
						if(PatCur.SuperFamily!=0) {//If they are currently in a SuperFamily and moving out.  Otherwise, no superfamily popups to worry about.
							Popups.CopyForMovingSuperFamily(PatCur,patInNewFam.SuperFamily);
						}
					}
					PatCur.Guarantor=patInNewFam.Guarantor;
					PatCur.SuperFamily=patInNewFam.SuperFamily;
					Patients.Update(PatCur,PatOld);
					FamCur=Patients.GetFamily(PatCur.PatNum);
					Patients.CombineGuarantors(FamCur,PatCur);
				}
				else{//there are other family members
					MessageBox.Show(Lan.g(this,"You cannot move the guarantor.  If you wish to move the guarantor, you must make another family member the guarantor first."));
				}
			}
			else{//guarantor not selected
				if(!MsgBox.Show(this,true,"Preparing to move family member.  Financial notes and address notes will not be transferred.  Popups will be copied.  Proceed to next step?")){
					return;
				}
				switch(MessageBox.Show(Lan.g(this,"Create new family instead of moving to an existing family?"),"",MessageBoxButtons.YesNoCancel)){
					case DialogResult.Cancel:
						return;
					case DialogResult.Yes://new family (split)
						Popups.CopyForMovingFamilyMember(PatCur);//Copy Family Level Popups to new family. 
						//Don't need to copy SuperFamily Popups. Stays in same super family.
						PatCur.Guarantor=PatCur.PatNum;
						//keep current superfamily
						Patients.Update(PatCur,PatOld);
						//if moving a superfamily non-guar family member out as guar of their own family within the sf, and pref is set, add ins to family members if necessary
						if(PatCur.SuperFamily>0 && PrefC.GetBool(PrefName.SuperFamNewPatAddIns)) {
							AddSuperGuarPriInsToFam(PatCur.Guarantor);
						}
						SecurityLogs.MakeLogEntry(Permissions.PatientEdit,PatCur.PatNum,"Patient moved to new family.");
						break;
					case DialogResult.No://move to an existing family
						if(!MsgBox.Show(this,true,"Select the family to move this patient to from the list that will come up next.")){
							return;
						}
						FormPatientSelect FormPS=new FormPatientSelect();
						FormPS.SelectionModeOnly=true;
						FormPS.ShowDialog();
						if(FormPS.DialogResult!=DialogResult.OK){
							return;
						}						
						Patient patInNewFam=Patients.GetPat(FormPS.SelectedPatNum);
						if(patInNewFam.Guarantor==PatCur.Guarantor) {
							return;// Patient is already a part of the family.
						}
						Popups.CopyForMovingFamilyMember(PatCur);//Copy Family Level Popups to new Family. 
						if(PatCur.SuperFamily!=patInNewFam.SuperFamily){//If they are moving into or out of a superfamily
							if(PatCur.SuperFamily!=0) {//If they are currently in a SuperFamily.  Otherwise, no superfamily popups to worry about.
								Popups.CopyForMovingSuperFamily(PatCur,patInNewFam.SuperFamily);
							}
						}
						PatCur.Guarantor=patInNewFam.Guarantor;
						PatCur.SuperFamily=patInNewFam.SuperFamily;//assign to the new superfamily
						Patients.Update(PatCur,PatOld);
						SecurityLogs.MakeLogEntry(Permissions.PatientEdit,PatCur.PatNum,"Patient moved from family of '"+PatOld.Guarantor+"' "
							+"to existing family of '"+PatCur.Guarantor+"'");
						break;
				}
			}//end guarantor not selected
			ModuleSelected(PatCur.PatNum);
		}
		#endregion

		#region gridRecall
		private void FillGridRecall(){
			gridRecall.BeginUpdate();
			//standard width is 354.  Nice to grow it to 525 if space allows.
			int maxWidth=Width-gridRecall.Left;
			if(maxWidth>525){
				maxWidth=525;
			}
			if(maxWidth>354) {
				gridRecall.Width=maxWidth;
			}
			else {
				gridRecall.Width=354;
			}
			gridRecall.Columns.Clear();
			List<DisplayField> listRecallFields=DisplayFields.GetForCategory(DisplayFieldCategory.FamilyRecallGrid);
			ODGridColumn col;
			for(int i=0;i<listRecallFields.Count;i++) {
				if(listRecallFields[i].Description=="") {
					col=new ODGridColumn(listRecallFields[i].InternalName,listRecallFields[i].ColumnWidth);
				}
				else {
					col=new ODGridColumn(listRecallFields[i].Description,listRecallFields[i].ColumnWidth);
				}
				gridRecall.Columns.Add(col);
			}
			gridRecall.Rows.Clear();
			if(PatCur==null){
				gridRecall.EndUpdate();
				return;
			}
			//we just want the recall for the current patient
			List<Recall> recallListPat=new List<Recall>();
			for(int i=0;i<RecallList.Count;i++){
				if(RecallList[i].PatNum==PatCur.PatNum){
					recallListPat.Add(RecallList[i]);
				}
			}
			ODGridRow row;
			ODGridCell cell;
			for(int i=0;i<recallListPat.Count;i++){
				row=new ODGridRow();
				for(int j=0;j<listRecallFields.Count;j++) {
					switch (listRecallFields[j].InternalName) {
						case "Type":
							string cellStr=RecallTypes.GetDescription(recallListPat[i].RecallTypeNum);
							row.Cells.Add(cellStr);
							break;
						case "Due Date":
							if(recallListPat[i].DateDue.Year<1880) {
								row.Cells.Add("");
							}
							else {
								cell=new ODGridCell(recallListPat[i].DateDue.ToShortDateString());
								if(recallListPat[i].DateDue<DateTime.Today) {
									cell.Bold=YN.Yes;
									cell.ColorText=Color.Firebrick;
								}
								row.Cells.Add(cell);
							}
							break;
						case "Sched Date":
							if(recallListPat[i].DateScheduled.Year<1880) {
								row.Cells.Add("");
							}
							else {
								row.Cells.Add(recallListPat[i].DateScheduled.ToShortDateString());
							}
							break;
						case "Notes":
							cellStr="";
							if(recallListPat[i].IsDisabled) {
								cellStr+=Lan.g(this,"Disabled");
								if(recallListPat[i].DatePrevious.Year>1800) {
									cellStr+=Lan.g(this,". Previous: ")+recallListPat[i].DatePrevious.ToShortDateString();
									if(recallListPat[i].RecallInterval!=new Interval(0,0,0,0)) {
										DateTime duedate=recallListPat[i].DatePrevious+recallListPat[i].RecallInterval;
										cellStr+=Lan.g(this,". (Due): ")+duedate.ToShortDateString();
									}
								}
							}
							if(recallListPat[i].DisableUntilDate.Year>1880) {
								if(cellStr!="") {
									cellStr+=", ";
								}
								cellStr+=Lan.g(this,"Disabled until ")+recallListPat[i].DisableUntilDate.ToShortDateString();
							}
							if(recallListPat[i].DisableUntilBalance>0) {
								if(cellStr!="") {
									cellStr+=", ";
								}
								cellStr+=Lan.g(this,"Disabled until balance ")+recallListPat[i].DisableUntilBalance.ToString("c");
							}
							if(recallListPat[i].RecallStatus!=0) {
								if(cellStr!="") {
									cellStr+=", ";
								}
								cellStr+=Defs.GetName(DefCat.RecallUnschedStatus,recallListPat[i].RecallStatus);
							}
							if(recallListPat[i].Note!="") {
								if(cellStr!="") {
									cellStr+=", ";
								}
								cellStr+=recallListPat[i].Note;
							}
							row.Cells.Add(cellStr);
							break;
						case "Previous Date":
							if(recallListPat[i].DatePrevious.Year>1880) {
								row.Cells.Add(recallListPat[i].DatePrevious.ToShortDateString());
							}
							else {
								row.Cells.Add("");
							}
							break;
						case "Interval":
							row.Cells.Add(recallListPat[i].RecallInterval.ToString());
							break;
					}
				}
				gridRecall.Rows.Add(row);
			}
			gridRecall.EndUpdate();
		}

		private void gridRecall_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			//use doubleclick instead
		}

		private void gridRecall_DoubleClick(object sender,EventArgs e) {
			if(PatCur==null){
				return;
			}
			FormRecallsPat FormR=new FormRecallsPat();
			FormR.PatNum=PatCur.PatNum;
			FormR.ShowDialog();
			ModuleSelected(PatCur.PatNum);
		}
		#endregion gridRecall

		#region gridSuperFam
		private void FillGridSuperFam() {
			gridSuperFam.BeginUpdate();
			gridSuperFam.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("gridSuperFam","Name"),280);
			gridSuperFam.Columns.Add(col);
			if(PrefC.GetBool(PrefName.StatementsUseSheets)) { //Only show superstatement column if they have StatementsUseSheets enabled.
				col=new ODGridColumn(Lan.g("gridSuperFam","Stmt"),0);
				gridSuperFam.Columns.Add(col);
			}
			gridSuperFam.Rows.Clear();
			if(PatCur==null) {
				gridSuperFam.EndUpdate();
				return;
			}
			ODGridRow row;
			SuperFamilyGuarantors.Sort(sortPatientListBySuperFamily);
			SuperFamilyMembers.Sort(sortPatientListBySuperFamily);
			string superfam="";
			for(int i=0;i<SuperFamilyGuarantors.Count;i++) {
				row=new ODGridRow();
				superfam=SuperFamilyGuarantors[i].GetNameLF();
				for(int j=0;j<SuperFamilyMembers.Count;j++) {
					if(SuperFamilyMembers[j].Guarantor==SuperFamilyGuarantors[i].Guarantor && SuperFamilyMembers[j].PatNum!=SuperFamilyGuarantors[i].PatNum) {
						superfam+="\r\n   "+SuperFamilyMembers[j].GetNameLF().Left(40,true);
					}
				}
				row.Cells.Add(superfam);
				row.Tag=SuperFamilyGuarantors[i].PatNum;
				if(i==0) {
					row.Cells[0].Bold=YN.Yes;
					row.Cells[0].ColorText=Color.OrangeRed;
				}
				if(PrefC.GetBool(PrefName.StatementsUseSheets)) { //Only show superstatement column if they have StatementsUseSheets enabled.
					if(SuperFamilyGuarantors[i].HasSuperBilling) {
						row.Cells.Add("X");
					}
					else {
						row.Cells.Add("");
					}
				}
				gridSuperFam.Rows.Add(row);
			}
			gridSuperFam.EndUpdate();
			for(int i=0;i<gridSuperFam.Rows.Count;i++) {
				if((long)gridSuperFam.Rows[i].Tag==PatCur.Guarantor) {
					gridSuperFam.SetSelected(i,true);
					break;
				}
			}
		}

		private int sortPatientListBySuperFamily(Patient pat1,Patient pat2) {
			if(pat1.PatNum==pat2.PatNum) {
				return 0;
			}
			if(pat1.PatNum==pat1.SuperFamily) {//Superheads always go to the top no matter what.
						return -1;
			}
			if(pat2.PatNum==pat2.SuperFamily) {
						return 1;
			}
			switch(_superFamSortStrat) {
				case SortStrategy.NameAsc:
					return pat1.GetNameLF().CompareTo(pat2.GetNameLF());
				case SortStrategy.NameDesc:
					return pat2.GetNameLF().CompareTo(pat1.GetNameLF());
				case SortStrategy.PatNumAsc:
					return pat1.PatNum.CompareTo(pat2.PatNum);
				case SortStrategy.PatNumDesc:
					return pat2.PatNum.CompareTo(pat1.PatNum);
				default:
					return pat1.PatNum.CompareTo(pat2.PatNum);//Default behavior
			}
		}

		private void gridSuperFam_CellClick(object sender,ODGridClickEventArgs e) {
			FormOpenDental.S_Contr_PatientSelected(SuperFamilyGuarantors[e.Row],false);
			ModuleSelected(SuperFamilyGuarantors[e.Row].PatNum);
		}

		private void gridSuperFam_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			//OnPatientSelected(SuperFamilyGuarantors[e.Row].PatNum,SuperFamilyGuarantors[e.Row].GetNameLF(),SuperFamilyGuarantors[e.Row].Email!="",
			//  SuperFamilyGuarantors[e.Row].ChartNumber);
			//ModuleSelected(SuperFamilyGuarantors[e.Row].PatNum);
		}

		private void ToolButAddSuper_Click() {
			if(PatCur.SuperFamily==0) {
				Patients.AssignToSuperfamily(PatCur.Guarantor,PatCur.Guarantor);
			}
			else {//we must want to add some other family to this superfamily
				FormPatientSelect formPS = new FormPatientSelect();
				formPS.SelectionModeOnly=true;
				formPS.ShowDialog();
				if(formPS.DialogResult!=DialogResult.OK) {
					return;
				}
				Patient patSelected=Patients.GetPat(formPS.SelectedPatNum);
				if(patSelected.SuperFamily==PatCur.SuperFamily) {
					MsgBox.Show(this,"That patient is already part of this superfamily.");
					return;
				}
				Patients.AssignToSuperfamily(patSelected.Guarantor,PatCur.SuperFamily);
				if(PrefC.GetBool(PrefName.SuperFamNewPatAddIns)) {
					AddSuperGuarPriInsToFam(patSelected.Guarantor);
				}
			}
			ModuleSelected(PatCur.PatNum);
		}


		///<summary>Adds the super family guarantor's primary insurance plan to each family member in Fam.  Each family member will be their own
		///subscriber with SubscriberID set to the patient's MedicaidID if one has been entered for the patient.  If a family member does not have a 
		///MedicaidID entered, FormInsPlan will open and prompt the user to enter a SubscriberID.</summary>
		private void AddSuperGuarPriInsToFam(long guarNum) {
			Patient superFamGuar=Patients.GetPat(PatCur.SuperFamily);
			if(superFamGuar==null) {//should never happen, but just in case
				return;
			}
			List<InsSub> listInsSubsSuper=InsSubs.GetListForSubscriber(superFamGuar.PatNum);
			if(listInsSubsSuper.Count==0) {//super family guar is not the subscriber for any insplans
				return;
			}
			List<PatPlan> listPatPlansSuper=PatPlans.Refresh(superFamGuar.PatNum);
			if(listPatPlansSuper.Count==0) {//super family guar doesn't have an active insplan
				return;
			}
			List<InsPlan> listInsPlansSuper=InsPlans.RefreshForSubList(listInsSubsSuper);
			InsSub sub=InsSubs.GetSub(
				PatPlans.GetInsSubNum(listPatPlansSuper,PatPlans.GetOrdinal(PriSecMed.Primary,listPatPlansSuper,listInsPlansSuper,listInsSubsSuper)),
				listInsSubsSuper);
			if(sub.InsSubNum==0 //should never happen, an active insplan exists, GetSub should return the inssub for the pri plan, just in case
				|| !MsgBox.Show(this,MsgBoxButtons.YesNo,"Would you like to add the super family guarantor's primary insurance plan to the patients in this family?"))
			{
				return;
			}
			//super family guarantor has a primary ins plan and the user chose to add it to the patients in this family
			PatPlan patPlanNew;
			InsSub insSubCur;
			FormInsPlan FormI;
			List<PatPlan> listPatPlansForPat;
			Family famCur=Patients.GetFamily(guarNum);
			List<InsSub> listInsSubsForFam=InsSubs.RefreshForFam(famCur);
			List<InsPlan> listInsPlansForFam=InsPlans.RefreshForSubList(listInsSubsForFam);
			bool patPlanAdded=false;
			foreach(Patient pat in famCur.ListPats) {//possibly filter by PatStatus, i.e. .Where(x => x.PatStatus==PatientStatus.Patient)
				listPatPlansForPat=PatPlans.Refresh(pat.PatNum);
				insSubCur=listInsSubsForFam.FirstOrDefault(x => x.Subscriber==pat.PatNum && x.PlanNum==sub.PlanNum);
				if(insSubCur!=null) {//InsSub already exists for this Patient and InsPlan
					if(listPatPlansForPat.Any(x => x.InsSubNum==insSubCur.InsSubNum)) {//PatPlan exists for this Patient and InsSub, nothing to do
						continue;
					}
				}
				else {//insSubCur==null, no InsSub exists for this patient and plan, insert new one
					insSubCur=new InsSub();
					insSubCur.PlanNum=sub.PlanNum;
					insSubCur.Subscriber=pat.PatNum;
					insSubCur.ReleaseInfo=sub.ReleaseInfo;
					insSubCur.AssignBen=sub.AssignBen;
					//insSubNew.BenefitNotes=sub.BenefitNotes;//not the BenefitNotes, since these could be specific to a patient
					insSubCur.SubscriberID=string.IsNullOrWhiteSpace(pat.MedicaidID)?"":pat.MedicaidID;
					//insSubNew.SubscNote=sub.SubscNote;//not the subscriber note, since every patient in super family is their own subscriber to this plan
					insSubCur.InsSubNum=InsSubs.Insert(insSubCur);
					listInsSubsForFam.Add(insSubCur.Copy());
				}
				patPlanNew=new PatPlan();
				patPlanNew.Ordinal=(byte)(listPatPlansForPat.Count+1);//so the ordinal of the first entry will be 1, NOT 0.
				patPlanNew.PatNum=pat.PatNum;
				patPlanNew.InsSubNum=insSubCur.InsSubNum;
				patPlanNew.Relationship=Relat.Self;
				patPlanNew.PatPlanNum=PatPlans.Insert(patPlanNew);
				listPatPlansForPat.Add(patPlanNew.Copy());
				if(string.IsNullOrWhiteSpace(insSubCur.SubscriberID)) {
					MessageBox.Show(this,Lan.g(this,"Enter the SubscriberID for")+" "+pat.GetNameFL()+".");
					FormI=new FormInsPlan(InsPlans.GetPlan(insSubCur.PlanNum,listInsPlansForFam),patPlanNew,insSubCur);
					FormI.IsNewPlan=false;
					FormI.IsNewPatPlan=true;
					FormI.ShowDialog();//this updates estimates. If cancel, then patplan is deleted. If cancel and planIsNew, then plan and benefits are deleted
					if(FormI.DialogResult!=DialogResult.OK) {
						continue;
					}
				}
				else {
					//compute estimates with new insurance plan
					List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
					List<Procedure> listProcs=Procedures.Refresh(pat.PatNum);
					List<Benefit> listBenefits=Benefits.Refresh(listPatPlansForPat,listInsSubsForFam);
					Procedures.ComputeEstimatesForAll(pat.PatNum,listClaimProcs,listProcs,listInsPlansForFam,listPatPlansForPat,listBenefits,pat.Age,listInsSubsForFam);
				}
				patPlanAdded=true;
				if(pat.HasIns!="I") {
					Patient patOld=pat.Copy();
					pat.HasIns="I";
					Patients.Update(pat,patOld);
				}
				long planNum1=0;
				long planNum2=0;
				if(listPatPlansForPat.Count>0) {
					planNum1=SubList.FindAll(x => x.InsSubNum==listPatPlansForPat[0].InsSubNum).Select(x => x.PlanNum).FirstOrDefault();
				}
				if(listPatPlansForPat.Count>1) {
					planNum2=SubList.FindAll(x => x.InsSubNum==listPatPlansForPat[1].InsSubNum).Select(x => x.PlanNum).FirstOrDefault();
				}
				Appointments.UpdateInsPlansForPat(pat.PatNum,planNum1,planNum2);
			}
			if(patPlanAdded) {
				SecurityLogs.MakeLogEntry(Permissions.PatPlanCreate,superFamGuar.PatNum,"Inserted new PatPlans for each family member of the super family guarantor.");
			}
		}

		private void ToolButRemoveSuper_Click() {
			if(PatCur.SuperFamily==PatCur.Guarantor) {
				MsgBox.Show(this,"You cannot delete the head of a super family.");
				return;
			}
			if(PatCur.SuperFamily==0) {
				return;
			}
			for(int i=0;i<FamCur.ListPats.Length;i++) {//remove whole family
				Patient tempPat=FamCur.ListPats[i].Copy();
				Popups.CopyForMovingSuperFamily(tempPat,0);
				tempPat.SuperFamily=0;
				Patients.Update(tempPat,FamCur.ListPats[i]);
			}
			ModuleSelected(PatCur.PatNum);
		}

		private void ToolButDisbandSuper_Click() {
			if(PatCur.SuperFamily==0) {
				return;
			}
			Patient superHead = Patients.GetPat(PatCur.SuperFamily);
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Would you like to disband and remove all members in the super family of "+superHead.GetNameFL()+"?")) {
				return;
			}
			Popups.RemoveForDisbandingSuperFamily(PatCur);
			Patients.DisbandSuperFamily(superHead.PatNum);
			ModuleSelected(PatCur.PatNum);
		}

		#endregion gridSuperFam

		#region Patient Clones
		private void FillGridPatientClones() {
			gridPatientClones.BeginUpdate();
			gridPatientClones.Columns.Clear();
			gridPatientClones.Columns.Add(new ODGridColumn(Lan.g(gridPatientClones.TranslationName,"Name"),150));
			if(PrefC.HasClinicsEnabled) {
				gridPatientClones.Columns.Add(new ODGridColumn(Lan.g(gridPatientClones.TranslationName,"Clinic"),80));
			}
			gridPatientClones.Columns.Add(new ODGridColumn(Lan.g(gridPatientClones.TranslationName,"Specialty"),0));
			gridPatientClones.Rows.Clear();
			if(PatCur==null) {
				gridPatientClones.EndUpdate();
				return;
			}
			int selectedIndex=-1;
			ODGridRow row;
			foreach(KeyValuePair<Patient,Def> cloneAndSpecialty in _dictCloneSpecialty) {
				//Never add deleted patients to the grid.  Deleted patients should not be selectable.
				if(cloneAndSpecialty.Key.PatStatus==PatientStatus.Deleted) {
					continue;
				}
				row=new ODGridRow();
				row.Cells.Add(cloneAndSpecialty.Key.GetNameLF());
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(Clinics.GetAbbr(cloneAndSpecialty.Key.ClinicNum));
				}
				//Check for null because an office could have just turned on clinics and a specialty would not have been required prior.
				row.Cells.Add((cloneAndSpecialty.Value==null) ? "" : cloneAndSpecialty.Value.ItemName);
				row.Tag=cloneAndSpecialty.Key;
				//If we are about to add the clone that is currently selected, save the index of said patient so that we can select them after the update.
				if(PatCur!=null && cloneAndSpecialty.Key.PatNum==PatCur.PatNum) {
					selectedIndex=gridPatientClones.Rows.Count;
				}
				gridPatientClones.Rows.Add(row);
			}
			//The first entry will always be the original or master patient which we want to stand out a little bit much like the Super Family grid.
			if(gridPatientClones.Rows.Count > 0) {
				gridPatientClones.Rows[0].Cells[0].Bold=YN.Yes;
				gridPatientClones.Rows[0].Cells[0].ColorText=Color.OrangeRed;
			}
			gridPatientClones.EndUpdate();
			//The grid has finished refreshing and can now have it's selected index changed.
			if(selectedIndex > -1) {
				gridPatientClones.SetSelected(selectedIndex,true);
			}
		}

		private void gridPatientClone_CellClick(object sender,ODGridClickEventArgs e) {
			if(gridPatientClones.Rows[e.Row].Tag==null || gridPatientClones.Rows[e.Row].Tag.GetType()!=typeof(Patient)) {
				return;
			}
			Patient patient=(Patient)gridPatientClones.Rows[e.Row].Tag;
			FormOpenDental.S_Contr_PatientSelected(patient,false);
			ModuleSelected(patient.PatNum);
		}

		///<summary>Returns a boolean based on if the current state of the Family module is ready for acting on behalf of the clone feature.
		///If something is not ready for clone action to be taken a message will show to the user and false will be returned.</summary>
		private bool IsValidForCloneAction() {
			if(PatCur==null) {
				MsgBox.Show(this,"Select a patient to perform clone actions.");
				return false;
			}
			return true;
		}

		///<summary></summary>
		private void ToolButAddClone_Click() {
			if(!IsValidForCloneAction()) {
				return;
			}
			FormCloneAdd FormCA;
			//Check to see if the currently selected patient is a clone instead of the original or master patient.
			if(PatientLinks.IsPatientAClone(PatCur.PatNum)) {
				long patNumMaster=PatientLinks.GetOriginalPatNumFromClone(PatCur.PatNum);
				Patient patientMaster=Patients.GetPat(patNumMaster);
				//Double check that the original or master patient was found.
				if(patientMaster==null) {
					MsgBox.Show(this,"The original patient cannot be found in order to create additional clones.  Please call support.");
					return;
				}
				FormCA=new FormCloneAdd(patientMaster);
			}
			else {//The currently selected patient is the original or master patient.
				FormCA=new FormCloneAdd(PatCur,FamCur,PlanList,SubList,BenefitList);
			}
			FormCA.ShowDialog();
			//At this point we know that we have all information regarding the original or master patient.
			if(FormCA.DialogResult!=DialogResult.OK) {
				return;
			}
			//Refresh the module with the new clone if one was created.
			long patNum=PatCur.PatNum;
			if(FormCA.PatNumClone > 0) {
				patNum=FormCA.PatNumClone;
			}
			ModuleSelected(patNum);
		}

		///<summary></summary>
		private void ToolButSynchClone_Click() {
			if(!IsValidForCloneAction()) {
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Demographic and Insurance Plan information from the selected patient will get synchronized to all clones of this patient.\r\n"
				+"Continue?"))
			{
				return;
			}
			string strDataUpdated=Patients.SynchClonesWithPatient(PatCur,FamCur,PlanList,SubList,BenefitList,PatPlanList);
			ModuleSelected(PatCur.PatNum);
			if(string.IsNullOrWhiteSpace(strDataUpdated)) {
				strDataUpdated=Lan.g(this,"No changes were made, data already in synch.");
			}
			new MsgBoxCopyPaste(strDataUpdated).Show();
		}

		///<summary></summary>
		private void ToolButBreakClone_Click() {
			if(!IsValidForCloneAction()) {
				return;
			}
			if(PatientLinks.IsPatientAClone(PatCur.PatNum)) {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Break the currently selected clone from the current clone group?")) {
					return;
				}
				PatientLinks.DeletePatNumTos(PatCur.PatNum,PatientLinkType.Clone);
			}
			else {
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"The original patient clone is currently selected.  "
					+"Breaking the original patient clone will cause all clone links in the current clone group to be broken.\r\n"
					+"Continue anyway?")) 
				{
					return;
				}
				PatientLinks.DeletePatNumFroms(PatCur.PatNum,PatientLinkType.Clone);
			}
			ModuleSelected(PatCur.PatNum);
		}
		#endregion

		#region gridIns
		private void menuPlansForFam_Click(object sender,EventArgs e) {
			FormPlansForFamily FormP=new FormPlansForFamily();
			FormP.FamCur=FamCur;
			FormP.ShowDialog();
			ModuleSelected(PatCur.PatNum);
		}

		private void ToolButIns_Click(){
			if(PatCur.DiscountPlanNum!=0) {
				MsgBox.Show(this,"Cannot add insurance if patient has a discount plan.");
				return;
			}
			DialogResult result=MessageBox.Show(Lan.g(this,"Is this patient the subscriber?"),"",MessageBoxButtons.YesNoCancel);
			if(result==DialogResult.Cancel){
				return;
			}
			//Pick a subscriber------------------------------------------------------------------------------------------------
			Patient subscriber;
			if(result==DialogResult.Yes){//current patient is subscriber
				subscriber=PatCur.Copy();
			}
			else{//patient is not subscriber
				//show list of patients in this family
				FormSubscriberSelect FormS=new FormSubscriberSelect(FamCur);
				FormS.ShowDialog();
				if(FormS.DialogResult==DialogResult.Cancel){
					return;
				}
				subscriber=Patients.GetPat(FormS.SelectedPatNum);
			}
			//Subscriber has been chosen. Now, pick a plan-------------------------------------------------------------------
			InsPlan plan=null;
			InsSub sub=null;
			bool planIsNew=false;
			List<InsSub> subList=InsSubs.GetListForSubscriber(subscriber.PatNum);
			if(subList.Count==0){
				planIsNew=true;
			}
			else{
				FormInsSelectSubscr FormISS=new FormInsSelectSubscr(subscriber.PatNum,PatCur.PatNum);
				FormISS.ShowDialog();
				if(FormISS.DialogResult==DialogResult.Cancel) {
					return;
				}
				if(FormISS.SelectedInsSubNum==0){//'New' option selected.
					planIsNew=true;
				}
				else{
					sub=InsSubs.GetSub(FormISS.SelectedInsSubNum,subList);
					plan=InsPlans.GetPlan(sub.PlanNum,new List<InsPlan>());
				}
			}
			//New plan was selected instead of an existing plan.  Create the plan--------------------------------------------
			if(planIsNew){
				plan=new InsPlan();
				plan.EmployerNum=subscriber.EmployerNum;
				plan.PlanType="";
				InsPlans.Insert(plan);
				sub=new InsSub();
				sub.PlanNum=plan.PlanNum;
				sub.Subscriber=subscriber.PatNum;
				if(subscriber.MedicaidID==""){
					sub.SubscriberID=subscriber.SSN;
				}
				else{
					sub.SubscriberID=subscriber.MedicaidID;
				}
				sub.ReleaseInfo=true;
				sub.AssignBen=true;
				InsSubs.Insert(sub);
				Benefit ben;
				foreach(CovCat covCat in CovCats.GetWhere(x => x.DefaultPercent!=-1,true)) {
					ben=new Benefit();
					ben.BenefitType=InsBenefitType.CoInsurance;
					ben.CovCatNum=covCat.CovCatNum;
					ben.PlanNum=plan.PlanNum;
					ben.Percent=covCat.DefaultPercent;
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
					ben.CodeNum=0;
					Benefits.Insert(ben);
				}
				//Zero deductible diagnostic
				if(CovCats.GetForEbenCat(EbenefitCategory.Diagnostic)!=null) {
					ben=new Benefit();
					ben.CodeNum=0;
					ben.BenefitType=InsBenefitType.Deductible;
					ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.Diagnostic).CovCatNum;
					ben.PlanNum=plan.PlanNum;
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
					ben.MonetaryAmt=0;
					ben.Percent=-1;
					ben.CoverageLevel=BenefitCoverageLevel.Individual;
					Benefits.Insert(ben);
				}
				//Zero deductible preventive
				if(CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive)!=null) {
					ben=new Benefit();
					ben.CodeNum=0;
					ben.BenefitType=InsBenefitType.Deductible;
					ben.CovCatNum=CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive).CovCatNum;
					ben.PlanNum=plan.PlanNum;
					ben.TimePeriod=BenefitTimePeriod.CalendarYear;
					ben.MonetaryAmt=0;
					ben.Percent=-1;
					ben.CoverageLevel=BenefitCoverageLevel.Individual;
					Benefits.Insert(ben);
				}
			}
			//Then attach plan------------------------------------------------------------------------------------------------
			PatPlan patplan=new PatPlan();
			patplan.Ordinal=(byte)(PatPlanList.Count+1);//so the ordinal of the first entry will be 1, NOT 0.
			patplan.PatNum=PatCur.PatNum;
			patplan.InsSubNum=sub.InsSubNum;
			patplan.Relationship=Relat.Self;
			PatPlans.Insert(patplan);
			//Then, display insPlanEdit to user-------------------------------------------------------------------------------
			FormInsPlan FormI=new FormInsPlan(plan,patplan,sub);
			FormI.IsNewPlan=planIsNew;
			FormI.IsNewPatPlan=true;
			if(FormI.ShowDialog()!=DialogResult.Cancel) {
				SecurityLogs.MakeLogEntry(Permissions.PatPlanCreate,PatCur.PatNum,"Inserted new PatPlan for patient. InsPlanNum: "+FormI.PlanCurNum);
				//Update users treatment plans to tie in to insurance
				TreatPlans.UpdateTreatmentPlanType(PatCur);
			}//this updates estimates also.
			//if cancel, then patplan is deleted from within that dialog.
			//if cancel, and planIsNew, then plan and benefits are also deleted.
			ModuleSelected(PatCur.PatNum);
		}

		private void ToolButDiscount_Click() {
			if(PatPlanList.Count > 0) {
				MsgBox.Show(this,"Cannot add discount plan when patient has insurance.");
				return;
			}
			FormDiscountPlans FormDP=new FormDiscountPlans();
			if(PatCur.DiscountPlanNum!=0) {
				FormDP.SelectedPlan=DiscountPlans.GetPlan(PatCur.DiscountPlanNum);
			}
			FormDP.IsSelectionMode=true;
			if(FormDP.ShowDialog()==DialogResult.OK) {
				Patient patOld=PatCur.Copy();
				PatCur.DiscountPlanNum=FormDP.SelectedPlan.DiscountPlanNum;
				if(Patients.Update(PatCur,patOld)) {
					string logText="The discount plan "+FormDP.SelectedPlan.Description+" was added.";
					SecurityLogs.MakeLogEntry(Permissions.DiscountPlanAddDrop,PatCur.PatNum,logText);
				}
				TreatPlans.UpdateTreatmentPlanType(PatCur);
			}
			FillInsData();
		}

		private void menuItemRemoveDiscount_Click(object sender,EventArgs e) {
			Patient patOld=PatCur.Copy();
			PatCur.DiscountPlanNum=0;
			if(Patients.Update(PatCur,patOld)) { 
				string logText="The discount plan "+DiscountPlans.GetPlan(patOld.DiscountPlanNum).Description+" was dropped.";
				SecurityLogs.MakeLogEntry(Permissions.DiscountPlanAddDrop,PatCur.PatNum,logText);
			}
			FillInsData();
		}

		private void FillInsData(){
			if(PatCur!=null && PatCur.DiscountPlanNum!=0) {
				gridIns.BeginUpdate();
				gridIns.Title=Lan.g(this,"Discount Plan");
				gridIns.Columns.Clear();
				gridIns.Rows.Clear();
				gridIns.Columns.Add(new ODGridColumn("",170));
				gridIns.Columns.Add(new ODGridColumn(Lan.g(this,"Discount Plan"),170));
				DiscountPlan discountPlan;
				if(_loadData.DiscountPlan==null || _loadData.DiscountPlan.DiscountPlanNum!=PatCur.DiscountPlanNum) {
					discountPlan=DiscountPlans.GetPlan(PatCur.DiscountPlanNum);
				}
				else {
					discountPlan=_loadData.DiscountPlan;
				}
				Def adjType=Defs.GetDef(DefCat.AdjTypes,discountPlan.DefNum);
				ODGridRow discountRow=new ODGridRow();
				discountRow.Cells.Add(Lan.g("TableDiscountPlans","Description"));
				discountRow.Cells.Add(discountPlan.Description);
				discountRow.ColorBackG=Defs.GetFirstForCategory(DefCat.MiscColors).ItemColor;
				gridIns.Rows.Add(discountRow);
				discountRow=new ODGridRow();
				discountRow.Cells.Add(Lan.g("TableDiscountPlans","Adjustment Type"));
				discountRow.Cells.Add(adjType.ItemName);
				gridIns.Rows.Add(discountRow);
				discountRow=new ODGridRow();
				discountRow.Cells.Add(Lan.g("TableDiscountPlans","Fee Schedule"));
				discountRow.Cells.Add(FeeScheds.GetDescription(discountPlan.FeeSchedNum));
				gridIns.Rows.Add(discountRow);
				gridIns.EndUpdate();
				return;
			}
			else {
				gridIns.Title=Lan.g(this,"Insurance Plans");
			}
			if(PatPlanList.Count==0){
				gridIns.BeginUpdate();
				gridIns.Columns.Clear();
				gridIns.Rows.Clear();
				gridIns.EndUpdate();
				return;
			}
			List<Def> listDefs=Defs.GetDefsForCategory(DefCat.MiscColors);
			List<InsSub> subArray=new List<InsSub>();//prevents repeated calls to db.
			List<InsPlan> planArray=new List<InsPlan>();
			InsSub sub;
			for(int i=0;i<PatPlanList.Count;i++){
				sub=InsSubs.GetSub(PatPlanList[i].InsSubNum,SubList);
				subArray.Add(sub);
				planArray.Add(InsPlans.GetPlan(sub.PlanNum,PlanList));
			}
			gridIns.BeginUpdate();
			gridIns.Columns.Clear();
			gridIns.Rows.Clear();
			OpenDental.UI.ODGridColumn col;
			col=new ODGridColumn("",150);
			gridIns.Columns.Add(col);
			int dentalOrdinal=1;
			for(int i=0;i<PatPlanList.Count;i++) {
				if(planArray[i].IsMedical) {
					col=new ODGridColumn(Lan.g("TableCoverage","Medical"),170);
					gridIns.Columns.Add(col);
				}
				else { //dental
					if(dentalOrdinal==1) {
						col=new ODGridColumn(Lan.g("TableCoverage","Primary"),170);
						gridIns.Columns.Add(col);
					}
					else if(dentalOrdinal==2) {
						col=new ODGridColumn(Lan.g("TableCoverage","Secondary"),170);
						gridIns.Columns.Add(col);
					}
					else {
						col=new ODGridColumn(Lan.g("TableCoverage","Other"),170);
						gridIns.Columns.Add(col);
					}
					dentalOrdinal++;
				}
			}
			OpenDental.UI.ODGridRow row=new ODGridRow();
			//subscriber
			row.Cells.Add(Lan.g("TableCoverage","Subscriber"));
			for(int i=0;i<PatPlanList.Count;i++){
				row.Cells.Add(FamCur.GetNameInFamFL(subArray[i].Subscriber));
			}
			row.ColorBackG=listDefs[0].ItemColor;
			gridIns.Rows.Add(row);
			//subscriber ID
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Subscriber ID"));
			for(int i=0;i<PatPlanList.Count;i++) {
				row.Cells.Add(subArray[i].SubscriberID);
			}
			row.ColorBackG=listDefs[0].ItemColor;
			gridIns.Rows.Add(row);
			//relationship
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Rel'ship to Sub"));
			for(int i=0;i<PatPlanList.Count;i++){
				row.Cells.Add(Lan.g("enumRelat",PatPlanList[i].Relationship.ToString()));
			}
			row.ColorBackG=listDefs[0].ItemColor;
			gridIns.Rows.Add(row);
			//patient ID
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Patient ID"));
			for(int i=0;i<PatPlanList.Count;i++){
				row.Cells.Add(PatPlanList[i].PatID);
			}
			row.ColorBackG=listDefs[0].ItemColor;
			gridIns.Rows.Add(row);
			//pending
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Pending"));
			for(int i=0;i<PatPlanList.Count;i++){
				if(PatPlanList[i].IsPending){
					row.Cells.Add("X");
				}
				else{
					row.Cells.Add("");
				}
			}
			row.ColorBackG=listDefs[0].ItemColor;
			row.ColorLborder=Color.Black;
			gridIns.Rows.Add(row);
			//employer
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Employer"));
			for(int i=0;i<PatPlanList.Count;i++) {
				row.Cells.Add(Employers.GetName(planArray[i].EmployerNum));
			}
			gridIns.Rows.Add(row);
			//carrier
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Carrier"));
			for(int i=0;i<PatPlanList.Count;i++) {
				row.Cells.Add(InsPlans.GetCarrierName(planArray[i].PlanNum,planArray));
			}
			gridIns.Rows.Add(row);
			//group name
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Group Name"));
			for(int i=0;i<PatPlanList.Count;i++) {
				row.Cells.Add(planArray[i].GroupName);
			}
			gridIns.Rows.Add(row);
			//group number
			row=new ODGridRow();
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				row.Cells.Add(Lan.g("TableCoverage","Plan Number"));
			}
			else {
				row.Cells.Add(Lan.g("TableCoverage","Group Number"));
			}
			for(int i=0;i<PatPlanList.Count;i++) {
				row.Cells.Add(planArray[i].GroupNum);
			}
			gridIns.Rows.Add(row);
			//plan type
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Type"));
			for(int i=0;i<planArray.Count;i++) {
				switch(planArray[i].PlanType){
					default://malfunction
						row.Cells.Add("");
						break;
					case "":
						row.Cells.Add(Lan.g(this,"Category Percentage"));
						break;
					case "p":
						FeeSched feeSchedCopay=FeeScheds.GetFirstOrDefault(x => x.FeeSchedNum==planArray[i].CopayFeeSched);
						if(feeSchedCopay!=null && feeSchedCopay.FeeSchedType==FeeScheduleType.FixedBenefit) {
							row.Cells.Add(Lan.g(this,"PPO Fixed Benefit"));
						}
						else {
							row.Cells.Add(Lan.g(this,"PPO Percentage"));
						}
						break;
					case "f":
						row.Cells.Add(Lan.g(this,"Medicaid or Flat Co-pay"));
						break;
					case "c":
						row.Cells.Add(Lan.g(this,"Capitation"));
						break;
				}
			}
			gridIns.Rows.Add(row);
			//fee schedule
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Fee Schedule"));
			for(int i=0;i<planArray.Count;i++) {
				row.Cells.Add(FeeScheds.GetDescription(planArray[i].FeeSched));
			}
			row.ColorLborder=Color.Black;
			gridIns.Rows.Add(row);
			//Calendar vs service year------------------------------------------------------------------------------------
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Benefit Period"));
			for(int i=0;i<planArray.Count;i++) {
				if(planArray[i].MonthRenew==0) {
					row.Cells.Add(Lan.g("TableCoverage","Calendar Year"));
				}
				else {
					DateTime dateservice=new DateTime(2000,planArray[i].MonthRenew,1);
					row.Cells.Add(Lan.g("TableCoverage","Service year begins:")+" "+dateservice.ToString("MMMM"));
				}
			}
			gridIns.Rows.Add(row);
			//Benefits-----------------------------------------------------------------------------------------------------
			List <Benefit> bensForPat=_loadData.ListBenefits;
			Benefit[,] benMatrix=Benefits.GetDisplayMatrix(bensForPat,PatPlanList,SubList);
			string desc;
			string val;
			ProcedureCode proccode=null;
			for(int y=0;y<benMatrix.GetLength(1);y++){//rows
				bool panoFreqAdded=false;
				bool bwFreqAdded=false;
				bool examFreqAdded=false;
				row=new ODGridRow();
				desc="";
				//some of the columns might be null, but at least one will not be.  Find it.
				for(int x=0;x<benMatrix.GetLength(0);x++){//columns
					if(benMatrix[x,y]==null){
						continue;
					}
					//create a description for the benefit
					if(benMatrix[x,y].PatPlanNum!=0) {
						desc+=Lan.g(this,"(pat)")+" ";
					}
					if(benMatrix[x,y].CoverageLevel==BenefitCoverageLevel.Family) {
						desc+=Lan.g(this,"Fam")+" ";
					}
					proccode=ProcedureCodes.GetProcCode(benMatrix[x,y].CodeNum);
					if(benMatrix[x,y].BenefitType==InsBenefitType.CoInsurance && benMatrix[x,y].Percent != -1) {
						if(benMatrix[x,y].CodeNum==0) {
							desc+=CovCats.GetDesc(benMatrix[x,y].CovCatNum)+" % ";
						}
						else {
							desc+=proccode.ProcCode+"-"+proccode.AbbrDesc+" % ";
						}
					}
					else if(benMatrix[x,y].BenefitType==InsBenefitType.Deductible) {
						desc+=Lan.g(this,"Deductible")+" "+CovCats.GetDesc(benMatrix[x,y].CovCatNum)+" ";
					}
					else if(benMatrix[x,y].BenefitType==InsBenefitType.Limitations
						&& benMatrix[x,y].QuantityQualifier==BenefitQuantity.None
						&& (benMatrix[x,y].TimePeriod==BenefitTimePeriod.ServiceYear
						|| benMatrix[x,y].TimePeriod==BenefitTimePeriod.CalendarYear))
					{//annual max
						desc+=Lan.g(this,"Annual Max")+" ";
					}
					else if(benMatrix[x,y].BenefitType==InsBenefitType.Limitations
						&& CovCats.GetForEbenCat(EbenefitCategory.Orthodontics)!=null
						&& benMatrix[x,y].CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.Orthodontics).CovCatNum
						&& benMatrix[x,y].QuantityQualifier==BenefitQuantity.None
						&& benMatrix[x,y].TimePeriod==BenefitTimePeriod.Lifetime)
					{
						desc+=Lan.g(this,"Ortho Max")+" ";
					}
					else if(benMatrix[x,y].CodeNum==0
						&& benMatrix[x,y].BenefitType==InsBenefitType.Limitations
						&& CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive)!=null
						&& benMatrix[x,y].CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive).CovCatNum
						&& benMatrix[x,y].MonetaryAmt==-1
						&& benMatrix[x,y].PatPlanNum==0
						&& benMatrix[x,y].Percent==-1
						&& (benMatrix[x,y].QuantityQualifier==BenefitQuantity.Months
								|| benMatrix[x,y].QuantityQualifier==BenefitQuantity.Years
								|| benMatrix[x,y].QuantityQualifier==BenefitQuantity.NumberOfServices))
					{
						desc+="Exam frequency ";
						examFreqAdded=true;
					}
					else if(proccode.ProcCode=="D0274"
						&& benMatrix[x,y].BenefitType==InsBenefitType.Limitations
						&& benMatrix[x,y].MonetaryAmt==-1
						&& benMatrix[x,y].PatPlanNum==0
						&& benMatrix[x,y].Percent==-1
						&& (benMatrix[x,y].QuantityQualifier==BenefitQuantity.Months
								|| benMatrix[x,y].QuantityQualifier==BenefitQuantity.Years
								|| benMatrix[x,y].QuantityQualifier==BenefitQuantity.NumberOfServices))
					{
						desc+="BW frequency ";
						bwFreqAdded=true;
					}
					else if(proccode.ProcCode=="D0330"
						&& benMatrix[x,y].BenefitType==InsBenefitType.Limitations
						&& benMatrix[x,y].MonetaryAmt==-1
						&& benMatrix[x,y].PatPlanNum==0
						&& benMatrix[x,y].Percent==-1
						&& (benMatrix[x,y].QuantityQualifier==BenefitQuantity.Months
								|| benMatrix[x,y].QuantityQualifier==BenefitQuantity.Years
								|| benMatrix[x,y].QuantityQualifier==BenefitQuantity.NumberOfServices))
					{
						desc+="Pano/FMX frequency ";
						panoFreqAdded=true;
					}
					else if(benMatrix[x,y].CodeNum==0){//e.g. flo
						desc+=proccode.AbbrDesc+" ";
					}
					else{
						desc+=Lan.g("enumInsBenefitType",benMatrix[x,y].BenefitType.ToString())+" ";
					}
					row.Cells.Add(desc);
					break;
				}
				//remember that matrix does not include the description column
				for(int x=0;x<benMatrix.GetLength(0);x++){//columns
					val="";
					//this matrix cell might be null
					if(benMatrix[x,y]==null){
						row.Cells.Add("");
						continue;
					}
					if(benMatrix[x,y].Percent != -1) {
						val+=benMatrix[x,y].Percent.ToString()+"% ";
					}
					if(benMatrix[x,y].MonetaryAmt != -1) {
						val+=benMatrix[x,y].MonetaryAmt.ToString("c0")+" ";
					}
					/*
					if(benMatrix[x,y].BenefitType==InsBenefitType.CoInsurance) {
						val+=benMatrix[x,y].Percent.ToString()+" ";
					}
					else if(benMatrix[x,y].BenefitType==InsBenefitType.Deductible
						&& benMatrix[x,y].MonetaryAmt==0)
					{//deductible 0
						val+=benMatrix[x,y].MonetaryAmt.ToString("c0")+" ";
					}
					else if(benMatrix[x,y].BenefitType==InsBenefitType.Limitations
						&& benMatrix[x,y].QuantityQualifier==BenefitQuantity.None
						&& (benMatrix[x,y].TimePeriod==BenefitTimePeriod.ServiceYear
						|| benMatrix[x,y].TimePeriod==BenefitTimePeriod.CalendarYear)
						&& benMatrix[x,y].MonetaryAmt==0)
					{//annual max 0
						val+=benMatrix[x,y].MonetaryAmt.ToString("c0")+" ";
					}*/
					if(benMatrix[x,y].BenefitType==InsBenefitType.Exclusions
						|| benMatrix[x,y].BenefitType==InsBenefitType.Limitations
						&& !(examFreqAdded || panoFreqAdded || bwFreqAdded)) 
					{
						if(benMatrix[x,y].CodeNum != 0) {
							proccode=ProcedureCodes.GetProcCode(benMatrix[x,y].CodeNum);
							val+=proccode.ProcCode+"-"+proccode.AbbrDesc+" ";
						}
						else if(benMatrix[x,y].CovCatNum != 0){
							val+=CovCats.GetDesc(benMatrix[x,y].CovCatNum)+" ";
						}
					}
					if(benMatrix[x,y].QuantityQualifier==BenefitQuantity.NumberOfServices){//eg 2 times per CalendarYear
						val+=benMatrix[x,y].Quantity.ToString()+" "+Lan.g(this,"times per")+" "
							+Lan.g("enumBenefitQuantity",benMatrix[x,y].TimePeriod.ToString())+" ";
					}
					else if(benMatrix[x,y].QuantityQualifier==BenefitQuantity.Months) {//eg Every 2 months
						val+=Lan.g(this,"Every ")+benMatrix[x,y].Quantity.ToString()+" month";
						if(benMatrix[x,y].Quantity>1){
							val+="s";
						}
					}
					else if(benMatrix[x,y].QuantityQualifier==BenefitQuantity.Years) {//eg Every 2 years
						val+="Every "+benMatrix[x,y].Quantity.ToString()+" year";
						if(benMatrix[x,y].Quantity>1) {
							val+="s";
						}
					}
					else{
						if(benMatrix[x,y].QuantityQualifier!=BenefitQuantity.None){//e.g. flo
							val+=Lan.g("enumBenefitQuantity",benMatrix[x,y].QuantityQualifier.ToString())+" ";
						}
						if(benMatrix[x,y].Quantity!=0){
							val+=benMatrix[x,y].Quantity.ToString()+" ";
						}
					}
					//if(benMatrix[x,y].MonetaryAmt!=0){
					//	val+=benMatrix[x,y].MonetaryAmt.ToString("c0")+" ";
					//}
					//if(val==""){
					//	val="val";
					//}
					row.Cells.Add(val);
				}
				gridIns.Rows.Add(row);
			}
			//Plan note
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Ins Plan Note"));
			OpenDental.UI.ODGridCell cell;
			for(int i=0;i<PatPlanList.Count;i++){
				cell=new ODGridCell();
				cell.Text=planArray[i].PlanNote;
				cell.ColorText=Color.Red;
				cell.Bold=YN.Yes;
				row.Cells.Add(cell);
			}
			gridIns.Rows.Add(row);
			//Subscriber Note
			row=new ODGridRow();
			row.Cells.Add(Lan.g("TableCoverage","Subscriber Note"));
			for(int i=0;i<PatPlanList.Count;i++) {
				cell=new ODGridCell();
				cell.Text=subArray[i].SubscNote;
				cell.ColorText=Color.Red;
				cell.Bold=YN.Yes;
				row.Cells.Add(cell);
			}
			gridIns.Rows.Add(row);
			gridIns.EndUpdate();
		}

		private void gridIns_CellDoubleClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			if(PatCur.DiscountPlanNum!=0) {
				DiscountPlan discountPlan=DiscountPlans.GetPlan(Patients.GetPat(PatCur.PatNum).DiscountPlanNum);
				if(discountPlan==null) {
					MsgBox.Show(this,"Discount plan deleted by another user.");
					ModuleSelected(PatCur.PatNum);
					return;
				}
				if(discountPlan.DiscountPlanNum!=PatCur.DiscountPlanNum) {
					MsgBox.Show(this,"Discount plan changed by another user.");
					ModuleSelected(PatCur.PatNum);
					return;
				}
				FormDiscountPlanEdit FormDP=new FormDiscountPlanEdit();
				FormDP.DiscountPlanCur=discountPlan;
				FormDP.IsSelectionMode=true;
				if(FormDP.ShowDialog()==DialogResult.OK) {
					ModuleSelected(PatCur.PatNum);
				}
				return;
			}
			if(e.Col==0){
				return;
			}
			Cursor=Cursors.WaitCursor;
			//Retrieving information from database due to concurrency issues causing the Family Module to display an insurance plan that has potentially changed.
			PatPlan patPlan=PatPlans.GetByPatPlanNum(PatPlanList[e.Col-1].PatPlanNum);
			if(patPlan==null) {
				MsgBox.Show(this,"Insurance plan for this patient no longer exists.  Refresh the module.");
				return;
			}
			SubList=InsSubs.RefreshForFam(FamCur);
			PlanList=InsPlans.RefreshForSubList(SubList);//this is only here in case, if in FormModuleSetup, the InsDefaultCobRule is changed and cob changed for all plans.
			InsSub insSub=SubList.Find(x => x.InsSubNum==patPlan.InsSubNum);
			InsPlan insPlan=InsPlans.GetPlan(insSub.PlanNum,PlanList);
			FormInsPlan FormIP=new FormInsPlan(insPlan,patPlan,insSub);
			FormIP.ShowDialog();
			Cursor=Cursors.Default;
			//Module is refreshed to reflect what the most recent information in the database is, but the module doesn't refresh if the insurance plan is edited by someone else.
			ModuleSelected(PatCur.PatNum);//Should refresh insplans to display new information
		}

		#endregion gridIns
	}
}
