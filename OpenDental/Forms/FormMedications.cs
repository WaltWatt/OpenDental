using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormMedications : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.Container components = null;
		///<summary></summary>
		public bool IsSelectionMode;
		private OpenDental.UI.Button butAddGeneric;
		private OpenDental.UI.Button butAddBrand;
		private OpenDental.UI.ODGrid gridAllMedications;
		public TextBox textSearch;
		private Label label1;
		///<summary>the number returned if using select mode.</summary>
		public long SelectedMedicationNum;
		private TabControl tabMedications;
		private TabPage tabAllMedications;
		private TabPage tabMissing;
		private ODGrid gridMissing;
		private UI.Button butConvertBrand;
		private UI.Button butConvertGeneric;

		///<summary>Set isAll to true to start in the All Medications tab, or false to start in the Meds In Use tab.</summary>
		public FormMedications() {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMedications));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butAddGeneric = new OpenDental.UI.Button();
			this.butAddBrand = new OpenDental.UI.Button();
			this.gridAllMedications = new OpenDental.UI.ODGrid();
			this.textSearch = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabMedications = new System.Windows.Forms.TabControl();
			this.tabAllMedications = new System.Windows.Forms.TabPage();
			this.tabMissing = new System.Windows.Forms.TabPage();
			this.butConvertBrand = new OpenDental.UI.Button();
			this.butConvertGeneric = new OpenDental.UI.Button();
			this.gridMissing = new OpenDental.UI.ODGrid();
			this.tabMedications.SuspendLayout();
			this.tabAllMedications.SuspendLayout();
			this.tabMissing.SuspendLayout();
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
			this.butCancel.Location = new System.Drawing.Point(858, 635);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "Cancel";
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
			this.butOK.Location = new System.Drawing.Point(777, 635);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butAddGeneric
			// 
			this.butAddGeneric.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddGeneric.Autosize = true;
			this.butAddGeneric.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddGeneric.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddGeneric.CornerRadius = 4F;
			this.butAddGeneric.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddGeneric.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddGeneric.Location = new System.Drawing.Point(6, 6);
			this.butAddGeneric.Name = "butAddGeneric";
			this.butAddGeneric.Size = new System.Drawing.Size(113, 26);
			this.butAddGeneric.TabIndex = 33;
			this.butAddGeneric.Text = "Add Generic";
			this.butAddGeneric.Click += new System.EventHandler(this.butAddGeneric_Click);
			// 
			// butAddBrand
			// 
			this.butAddBrand.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAddBrand.Autosize = true;
			this.butAddBrand.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAddBrand.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAddBrand.CornerRadius = 4F;
			this.butAddBrand.Image = global::OpenDental.Properties.Resources.Add;
			this.butAddBrand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAddBrand.Location = new System.Drawing.Point(125, 6);
			this.butAddBrand.Name = "butAddBrand";
			this.butAddBrand.Size = new System.Drawing.Size(113, 26);
			this.butAddBrand.TabIndex = 34;
			this.butAddBrand.Text = "Add Brand";
			this.butAddBrand.Click += new System.EventHandler(this.butAddBrand_Click);
			// 
			// gridAllMedications
			// 
			this.gridAllMedications.AllowSortingByColumn = true;
			this.gridAllMedications.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridAllMedications.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridAllMedications.HasAddButton = false;
			this.gridAllMedications.HasDropDowns = false;
			this.gridAllMedications.HasMultilineHeaders = false;
			this.gridAllMedications.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridAllMedications.HeaderHeight = 15;
			this.gridAllMedications.HScrollVisible = false;
			this.gridAllMedications.Location = new System.Drawing.Point(5, 38);
			this.gridAllMedications.Name = "gridAllMedications";
			this.gridAllMedications.ScrollValue = 0;
			this.gridAllMedications.Size = new System.Drawing.Size(907, 558);
			this.gridAllMedications.TabIndex = 37;
			this.gridAllMedications.Title = "All Medications";
			this.gridAllMedications.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridAllMedications.TitleHeight = 18;
			this.gridAllMedications.TranslationName = "FormMedications";
			this.gridAllMedications.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAllMedications_CellDoubleClick);
			this.gridAllMedications.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.gridAllMedications_CellClick);
			// 
			// textSearch
			// 
			this.textSearch.Location = new System.Drawing.Point(367, 9);
			this.textSearch.Name = "textSearch";
			this.textSearch.Size = new System.Drawing.Size(195, 20);
			this.textSearch.TabIndex = 0;
			this.textSearch.TextChanged += new System.EventHandler(this.textSearch_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(239, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(127, 17);
			this.label1.TabIndex = 39;
			this.label1.Text = "Search";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tabMedications
			// 
			this.tabMedications.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabMedications.Controls.Add(this.tabAllMedications);
			this.tabMedications.Controls.Add(this.tabMissing);
			this.tabMedications.Location = new System.Drawing.Point(9, 3);
			this.tabMedications.Name = "tabMedications";
			this.tabMedications.SelectedIndex = 0;
			this.tabMedications.Size = new System.Drawing.Size(924, 626);
			this.tabMedications.TabIndex = 40;
			this.tabMedications.SelectedIndexChanged += new System.EventHandler(this.tabMedications_SelectedIndexChanged);
			// 
			// tabAllMedications
			// 
			this.tabAllMedications.Controls.Add(this.gridAllMedications);
			this.tabAllMedications.Controls.Add(this.textSearch);
			this.tabAllMedications.Controls.Add(this.butAddGeneric);
			this.tabAllMedications.Controls.Add(this.label1);
			this.tabAllMedications.Controls.Add(this.butAddBrand);
			this.tabAllMedications.Location = new System.Drawing.Point(4, 22);
			this.tabAllMedications.Name = "tabAllMedications";
			this.tabAllMedications.Padding = new System.Windows.Forms.Padding(3);
			this.tabAllMedications.Size = new System.Drawing.Size(916, 600);
			this.tabAllMedications.TabIndex = 0;
			this.tabAllMedications.Text = "All Medications";
			this.tabAllMedications.UseVisualStyleBackColor = true;
			// 
			// tabMissing
			// 
			this.tabMissing.Controls.Add(this.butConvertBrand);
			this.tabMissing.Controls.Add(this.butConvertGeneric);
			this.tabMissing.Controls.Add(this.gridMissing);
			this.tabMissing.Location = new System.Drawing.Point(4, 22);
			this.tabMissing.Name = "tabMissing";
			this.tabMissing.Size = new System.Drawing.Size(916, 600);
			this.tabMissing.TabIndex = 2;
			this.tabMissing.Text = "Missing Generic/Brand";
			this.tabMissing.UseVisualStyleBackColor = true;
			// 
			// butConvertBrand
			// 
			this.butConvertBrand.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butConvertBrand.Autosize = true;
			this.butConvertBrand.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butConvertBrand.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butConvertBrand.CornerRadius = 4F;
			this.butConvertBrand.Image = global::OpenDental.Properties.Resources.Add;
			this.butConvertBrand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butConvertBrand.Location = new System.Drawing.Point(161, 6);
			this.butConvertBrand.Name = "butConvertBrand";
			this.butConvertBrand.Size = new System.Drawing.Size(150, 26);
			this.butConvertBrand.TabIndex = 40;
			this.butConvertBrand.Text = "Convert To Brand";
			this.butConvertBrand.Click += new System.EventHandler(this.butConvertBrand_Click);
			// 
			// butConvertGeneric
			// 
			this.butConvertGeneric.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butConvertGeneric.Autosize = true;
			this.butConvertGeneric.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butConvertGeneric.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butConvertGeneric.CornerRadius = 4F;
			this.butConvertGeneric.Image = global::OpenDental.Properties.Resources.Add;
			this.butConvertGeneric.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butConvertGeneric.Location = new System.Drawing.Point(5, 6);
			this.butConvertGeneric.Name = "butConvertGeneric";
			this.butConvertGeneric.Size = new System.Drawing.Size(150, 26);
			this.butConvertGeneric.TabIndex = 39;
			this.butConvertGeneric.Text = "Convert To Generic";
			this.butConvertGeneric.Click += new System.EventHandler(this.butConvertGeneric_Click);
			// 
			// gridMissing
			// 
			this.gridMissing.AllowSortingByColumn = true;
			this.gridMissing.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMissing.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMissing.HasAddButton = false;
			this.gridMissing.HasDropDowns = false;
			this.gridMissing.HasMultilineHeaders = false;
			this.gridMissing.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMissing.HeaderHeight = 15;
			this.gridMissing.HScrollVisible = false;
			this.gridMissing.Location = new System.Drawing.Point(5, 38);
			this.gridMissing.Name = "gridMissing";
			this.gridMissing.ScrollValue = 0;
			this.gridMissing.Size = new System.Drawing.Size(907, 559);
			this.gridMissing.TabIndex = 38;
			this.gridMissing.Title = "Medications Missing Generic or Brand";
			this.gridMissing.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMissing.TitleHeight = 18;
			this.gridMissing.TranslationName = "FormMedications";
			// 
			// FormMedications
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(941, 671);
			this.Controls.Add(this.tabMedications);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(600, 400);
			this.Name = "FormMedications";
			this.ShowInTaskbar = false;
			this.Text = "Medications";
			this.Load += new System.EventHandler(this.FormMedications_Load);
			this.tabMedications.ResumeLayout(false);
			this.tabAllMedications.ResumeLayout(false);
			this.tabAllMedications.PerformLayout();
			this.tabMissing.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormMedications_Load(object sender, System.EventArgs e) {
			if(!CultureInfo.CurrentCulture.Name.EndsWith("US")) {//Not United States
				//Medications missing generic/brand are not visible for foreigners because there will be no data available.
				//This type of data can only be created in the United States for customers using NewCrop.
				tabMedications.TabPages.Remove(tabMissing);
			}
			FillTab();
			if(IsSelectionMode){
				this.Text=Lan.g(this,"Select Medication");
			}
			else{
				butOK.Visible=false;
				butCancel.Text=Lan.g(this,"Close");
			}
		}

		private void tabMedications_SelectedIndexChanged(object sender,EventArgs e) {
			FillTab();
		}

		private void FillTab() {
			if(tabMedications.SelectedIndex==0) {
				FillGridAllMedications();
			}
			else if(tabMedications.SelectedIndex==1) {
				FillGridMissing();
			}
		}

		private void FillGridAllMedications(bool shouldRetainSelection=true){
			Medication medSelected=null;
			if(shouldRetainSelection && gridAllMedications.GetSelectedIndex()!=-1) {
				medSelected=(Medication)gridAllMedications.Rows[gridAllMedications.GetSelectedIndex()].Tag;
			}
			List <long> listInUseMedicationNums=Medications.GetAllInUseMedicationNums();
			int sortColIndex=gridAllMedications.SortedByColumnIdx;
			bool isSortAscending=gridAllMedications.SortedIsAscending;
			gridAllMedications.BeginUpdate();
			gridAllMedications.Columns.Clear();
			//The order of these columns is important.  See gridAllMedications_CellClick()
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Drug Name"),120,GridSortingStrategy.StringCompare);
			gridAllMedications.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Generic Name"),120,GridSortingStrategy.StringCompare);
			gridAllMedications.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"InUse"),55,HorizontalAlignment.Center,GridSortingStrategy.StringCompare);
			gridAllMedications.Columns.Add(col);
			if(CultureInfo.CurrentCulture.Name.EndsWith("US")) {//United States
				col=new ODGridColumn(Lan.g(this,"RxNorm"),70,GridSortingStrategy.StringCompare);
				gridAllMedications.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g(this,"Notes for Generic"),250,GridSortingStrategy.StringCompare);
			gridAllMedications.Columns.Add(col);
			gridAllMedications.Rows.Clear();
			List <Medication> listMeds=Medications.GetList(textSearch.Text);
			foreach(Medication med in listMeds) {
				ODGridRow row=new ODGridRow();
				row.Tag=med;
				if(med.MedicationNum==med.GenericNum) {//isGeneric
					row.Cells.Add(med.MedName);
					row.Cells.Add("");
				}
				else{
					row.Cells.Add(med.MedName);
					row.Cells.Add(Medications.GetGenericName(med.GenericNum));
				}
				if(listInUseMedicationNums.Contains(med.MedicationNum)) {
					row.Cells.Add("X");//InUse
				}
				else {
					row.Cells.Add("");//InUse
				}
				if(CultureInfo.CurrentCulture.Name.EndsWith("US")) {//United States
					if(med.RxCui==0) {
						row.Cells.Add(Lan.g(this,"(select)"));
						row.Cells[row.Cells.Count-1].Bold=YN.Yes;
					}
					else {
						row.Cells.Add(med.RxCui.ToString());
					}
				}
				row.Cells.Add(med.Notes);
				gridAllMedications.Rows.Add(row);
			}
			gridAllMedications.EndUpdate();
			gridAllMedications.SortForced(sortColIndex,isSortAscending);
			if(medSelected!=null) {//Will be null if nothing is selected.
				for(int i=0;i<gridAllMedications.Rows.Count;i++) {
					Medication medCur=(Medication)gridAllMedications.Rows[i].Tag;
					if(medCur.MedicationNum==medSelected.MedicationNum) {
						gridAllMedications.SetSelected(i,true);
						break;
					}
				}
			}
		}

		private void FillGridMissing() {
			int sortColIndex=gridMissing.SortedByColumnIdx;
			bool isSortAscending=gridMissing.SortedIsAscending;
			gridMissing.BeginUpdate();
			gridMissing.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"RxNorm"),70,GridSortingStrategy.StringCompare);
			gridMissing.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Drug Description"),0,GridSortingStrategy.StringCompare);
			gridMissing.Columns.Add(col);
			gridMissing.Rows.Clear();
			List<MedicationPat> listMedPats=MedicationPats.GetAllMissingMedications();
			Dictionary <string,List<MedicationPat>> dictMissingUnique=new Dictionary<string,List<MedicationPat>>();
			foreach(MedicationPat medPat in listMedPats) {
				string key=medPat.RxCui.ToString()+" - "+medPat.MedDescript.ToLower().Trim();
				if(!dictMissingUnique.ContainsKey(key)) {
					dictMissingUnique[key]=new List<MedicationPat>();
					ODGridRow row=new ODGridRow();
					row.Tag=dictMissingUnique[key];
					if(medPat.RxCui==0) {
						row.Cells.Add("");
					}
					else {
						row.Cells.Add(medPat.RxCui.ToString());
					}
					row.Cells.Add(medPat.MedDescript);
					gridMissing.Rows.Add(row);
				}
				dictMissingUnique[key].Add(medPat);
			}
			gridMissing.EndUpdate();
			gridMissing.SortForced(sortColIndex,isSortAscending);
		}

		private void butAddGeneric_Click(object sender, System.EventArgs e) {
			Medication MedicationCur=new Medication();
			Medications.Insert(MedicationCur);//so that we will have the primary key
			MedicationCur.GenericNum=MedicationCur.MedicationNum;
			FormMedicationEdit FormME=new FormMedicationEdit();
			FormME.MedicationCur=MedicationCur;
			FormME.IsNew=true;
			FormME.ShowDialog();//This window refreshes the Medication cache if the user clicked OK.
			FillTab();
		}

		private void butAddBrand_Click(object sender, System.EventArgs e) {
			if(gridAllMedications.GetSelectedIndex()==-1){
				MessageBox.Show(Lan.g(this,"You must first highlight the generic medication from the list.  If it is not already on the list, then you must add it first."));
				return;
			}
			Medication medSelected=(Medication)gridAllMedications.Rows[gridAllMedications.GetSelectedIndex()].Tag;
			if(medSelected.MedicationNum!=medSelected.GenericNum){
				MessageBox.Show(Lan.g(this,"The selected medication is not generic."));
				return;
			}
			Medication MedicationCur=new Medication();
			Medications.Insert(MedicationCur);//so that we will have the primary key
			MedicationCur.GenericNum=medSelected.MedicationNum;
			FormMedicationEdit FormME=new FormMedicationEdit();
			FormME.MedicationCur=MedicationCur;
			FormME.IsNew=true;
			FormME.ShowDialog();//This window refreshes the Medication cache if the user clicked OK.
			FillTab();
		}

		private void textSearch_TextChanged(object sender,EventArgs e) {
			FillTab();
		}

		private void gridAllMedications_CellClick(object sender,ODGridClickEventArgs e) {
			Medication med=(Medication)gridAllMedications.Rows[e.Row].Tag;
			if(CultureInfo.CurrentCulture.Name.EndsWith("US") && e.Col==3) {//United States RxNorm Column
				FormRxNorms formRxNorm=new FormRxNorms();
				formRxNorm.IsSelectionMode=true;
				formRxNorm.InitSearchCodeOrDescript=med.MedName;
				formRxNorm.ShowDialog();
				if(formRxNorm.DialogResult==DialogResult.OK) {
					med.RxCui=PIn.Long(formRxNorm.SelectedRxNorm.RxCui);
					//The following behavior mimics FormMedicationEdit OK click.
					Medications.Update(med);
					MedicationPats.UpdateRxCuiForMedication(med.MedicationNum,med.RxCui);
					DataValid.SetInvalid(InvalidType.Medications);
				}
				FillTab();
			}
		}

		private void gridAllMedications_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Medication med=(Medication)gridAllMedications.Rows[e.Row].Tag;
			med=Medications.GetMedication(med.MedicationNum);
			if(med==null) {//Possible to delete the medication from a separate WS while medication loaded in memory.
				MsgBox.Show(this,"An error occurred loading medication.");
				return;
			}
			if(IsSelectionMode){
				SelectedMedicationNum=med.MedicationNum;
				DialogResult=DialogResult.OK;
			}
			else{//normal mode from main menu
				if(!CultureInfo.CurrentCulture.Name.EndsWith("US") || e.Col!=3) {//Not United States RxNorm Column
					FormMedicationEdit FormME=new FormMedicationEdit();
					FormME.MedicationCur=med;
					FormME.ShowDialog();//This window refreshes the Medication cache if the user clicked OK.
					FillTab();
				}
			}
		}

		private void butConvertGeneric_Click(object sender,EventArgs e) {
			if(gridMissing.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item from the list before attempting to convert.");
				return;
			}
			List<MedicationPat> listMedPats=(List<MedicationPat>)gridMissing.Rows[gridMissing.SelectedIndices[0]].Tag;
			List<Medication> listRxCuiMeds=null;
			Medication medGeneric=null;
			if(listMedPats[0].RxCui!=0) {
				listRxCuiMeds=Medications.GetAllMedsByRxCui(listMedPats[0].RxCui);
				medGeneric=listRxCuiMeds.FirstOrDefault(x => x.MedicationNum==x.GenericNum);
				if(medGeneric==null && listRxCuiMeds.FirstOrDefault(x => x.MedicationNum!=x.GenericNum)!=null) {//A Brand Medication exists with matching RxCui.
					MsgBox.Show(this,"A brand medication matching the RxNorm of the selected medication already exists in the medication list.  "
						+"You cannot create a generic for the selected medication.  Use the Convert to Brand button instead.");
					return;
				}
			}
			if(listRxCuiMeds==null || listRxCuiMeds.Count==0) {//No medications found matching the RxCui
				medGeneric=new Medication();
				medGeneric.MedName=listMedPats[0].MedDescript;
				medGeneric.RxCui=listMedPats[0].RxCui;
				Medications.Insert(medGeneric);//To get primary key.
				medGeneric.GenericNum=medGeneric.MedicationNum;
				Medications.Update(medGeneric);//Now that we have primary key, flag the medication as a generic.
				FormMedicationEdit FormME=new FormMedicationEdit();
				FormME.MedicationCur=medGeneric;
				FormME.IsNew=true;
				FormME.ShowDialog();//This window refreshes the Medication cache if the user clicked OK.
				if(FormME.DialogResult!=DialogResult.OK) {
					return;//User canceled.
				}
			}
			else if(medGeneric!=null &&
				!MsgBox.Show(this,true,"A generic medication matching the RxNorm of the selected medication already exists in the medication list.  "
					+"Click OK to use the existing medication as the generic for the selected medication, or click Cancel to abort."))
			{
				return;
			}
			Cursor=Cursors.WaitCursor;
			MedicationPats.UpdateMedicationNumForMany(medGeneric.MedicationNum,listMedPats.Select(x => x.MedicationPatNum).ToList());
			FillTab();
			Cursor=Cursors.Default;
			MsgBox.Show(this,"Done.");
		}

		private void butConvertBrand_Click(object sender,EventArgs e) {
			if(gridMissing.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item from the list before attempting to convert.");
				return;
			}
			List<MedicationPat> listMedPats=(List<MedicationPat>)gridMissing.Rows[gridMissing.SelectedIndices[0]].Tag;
			List<Medication> listRxCuiMeds=null;
			Medication medBrand=null;
			if(listMedPats[0].RxCui!=0) {
				listRxCuiMeds=Medications.GetAllMedsByRxCui(listMedPats[0].RxCui);
				medBrand=listRxCuiMeds.FirstOrDefault(x => x.MedicationNum!=x.GenericNum);
				if(medBrand==null && listRxCuiMeds.FirstOrDefault(x => x.MedicationNum==x.GenericNum)!=null) {//A Generic Medication exists with matching RxCui.
					MsgBox.Show(this,"A generic medication matching the RxNorm of the selected medication already exists in the medication list.  "
						+"You cannot create a brand for the selected medication.  Use the Convert to Generic button instead.");
					return;
				}
			}
			if(listRxCuiMeds==null || listRxCuiMeds.Count==0) {//No medications found matching the RxCui
				Medication medGeneric=null;
				if(gridAllMedications.SelectedIndices.Length > 0) {
					medGeneric=(Medication)gridAllMedications.Rows[gridAllMedications.SelectedIndices[0]].Tag;
					if(medGeneric.MedicationNum!=medGeneric.GenericNum) {
						medGeneric=null;//The selected medication is a brand medication, not a generic medication.
					}
				}
				if(medGeneric==null) {
					MsgBox.Show(this,"Please select a generic medication from the All Medications tab before attempting to convert.  "
						+"The selected medication will be used as the generic medication for the new brand medication.");
					return;
				}
				medBrand=new Medication();
				medBrand.MedName=listMedPats[0].MedDescript;
				medBrand.RxCui=listMedPats[0].RxCui;
				medBrand.GenericNum=medGeneric.MedicationNum;
				Medications.Insert(medBrand);
				FormMedicationEdit FormME=new FormMedicationEdit();
				FormME.MedicationCur=medBrand;
				FormME.IsNew=true;
				FormME.ShowDialog();//This window refreshes the Medication cache if the user clicked OK.
				if(FormME.DialogResult!=DialogResult.OK) {
					return;//User canceled.
				}
			}
			else if(medBrand!=null &&
				!MsgBox.Show(this,true,"A brand medication matching the RxNorm of the selected medication already exists in the medication list.  "
					+"Click OK to use the existing medication as the brand for the selected medication, or click Cancel to abort."))
			{
				return;
			}
			Cursor=Cursors.WaitCursor;
			MedicationPats.UpdateMedicationNumForMany(medBrand.MedicationNum,listMedPats.Select(x => x.MedicationPatNum).ToList());
			FillTab();
			Cursor=Cursors.Default;
			MsgBox.Show(this,"Done.");
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			//this button is not visible if not selection mode.
			if(gridAllMedications.GetSelectedIndex()==-1) {
				MessageBox.Show(Lan.g(this,"Please select an item first."));
				return;
			}
			SelectedMedicationNum=((Medication)gridAllMedications.Rows[gridAllMedications.GetSelectedIndex()].Tag).MedicationNum;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}





















