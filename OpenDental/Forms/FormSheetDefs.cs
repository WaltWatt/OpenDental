using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental{
	/// <summary></summary>
	public class FormSheetDefs:ODForm {
		private OpenDental.UI.Button butNew;
		private OpenDental.UI.Button butClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.ODGrid grid2;
		private ODGrid grid1;
		private OpenDental.UI.Button butCopy;
		//private bool changed;
		//public bool IsSelectionMode;
		//<summary>Only used if IsSelectionMode.  On OK, contains selected siteNum.  Can be 0.  Can also be set ahead of time externally.</summary>
		//public int SelectedSiteNum;
		private List<SheetDef> internalList;
		private Label label1;
		private ComboBox comboLabel;
		private bool changed;
		private Label label2;
		private OpenDental.UI.Button butCopy2;
		private UI.Button butTools;
		private UI.Button butDefault;
		List<SheetDef> LabelList;
		private List<SheetDef> _listSheetDefs;

		///<summary></summary>
		public FormSheetDefs()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSheetDefs));
			this.label1 = new System.Windows.Forms.Label();
			this.comboLabel = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.grid1 = new OpenDental.UI.ODGrid();
			this.grid2 = new OpenDental.UI.ODGrid();
			this.butCopy2 = new OpenDental.UI.Button();
			this.butCopy = new OpenDental.UI.Button();
			this.butNew = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.butTools = new OpenDental.UI.Button();
			this.butDefault = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(205, 15);
			this.label1.TabIndex = 16;
			this.label1.Text = "Label assigned to patient button";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboLabel
			// 
			this.comboLabel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboLabel.FormattingEnabled = true;
			this.comboLabel.Location = new System.Drawing.Point(223, 8);
			this.comboLabel.MaxDropDownItems = 20;
			this.comboLabel.Name = "comboLabel";
			this.comboLabel.Size = new System.Drawing.Size(185, 21);
			this.comboLabel.TabIndex = 1;
			this.comboLabel.DropDown += new System.EventHandler(this.comboLabel_DropDown);
			this.comboLabel.SelectionChangeCommitted += new System.EventHandler(this.comboLabel_SelectionChangeCommitted);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(414, 6);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(428, 33);
			this.label2.TabIndex = 18;
			this.label2.Text = "Most other sheet types are assigned simply by creating custom sheets of the same " +
    "type.  Referral slips are set in the referral edit window of each referral.";
			// 
			// grid1
			// 
			this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.grid1.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.grid1.HasAddButton = false;
			this.grid1.HasDropDowns = false;
			this.grid1.HasMultilineHeaders = false;
			this.grid1.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.grid1.HeaderHeight = 15;
			this.grid1.HScrollVisible = false;
			this.grid1.Location = new System.Drawing.Point(12, 42);
			this.grid1.Name = "grid1";
			this.grid1.ScrollValue = 0;
			this.grid1.Size = new System.Drawing.Size(370, 583);
			this.grid1.TabIndex = 2;
			this.grid1.Title = "Internal";
			this.grid1.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.grid1.TitleHeight = 18;
			this.grid1.TranslationName = "TableInternal";
			this.grid1.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.grid1_CellDoubleClick);
			this.grid1.Click += new System.EventHandler(this.grid1_Click);
			// 
			// grid2
			// 
			this.grid2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grid2.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.grid2.HasAddButton = false;
			this.grid2.HasDropDowns = false;
			this.grid2.HasMultilineHeaders = false;
			this.grid2.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.grid2.HeaderHeight = 15;
			this.grid2.HScrollVisible = false;
			this.grid2.Location = new System.Drawing.Point(493, 42);
			this.grid2.Name = "grid2";
			this.grid2.ScrollValue = 0;
			this.grid2.Size = new System.Drawing.Size(376, 583);
			this.grid2.TabIndex = 3;
			this.grid2.Title = "Custom";
			this.grid2.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.grid2.TitleHeight = 18;
			this.grid2.TranslationName = "TableCustom";
			this.grid2.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.grid2_CellDoubleClick);
			this.grid2.Click += new System.EventHandler(this.grid2_Click);
			// 
			// butCopy2
			// 
			this.butCopy2.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCopy2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCopy2.Autosize = true;
			this.butCopy2.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCopy2.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCopy2.CornerRadius = 4F;
			this.butCopy2.Image = global::OpenDental.Properties.Resources.Add;
			this.butCopy2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCopy2.Location = new System.Drawing.Point(700, 635);
			this.butCopy2.Name = "butCopy2";
			this.butCopy2.Size = new System.Drawing.Size(89, 24);
			this.butCopy2.TabIndex = 7;
			this.butCopy2.Text = "Duplicate";
			this.butCopy2.Click += new System.EventHandler(this.butCopy2_Click);
			// 
			// butCopy
			// 
			this.butCopy.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCopy.Autosize = true;
			this.butCopy.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCopy.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCopy.CornerRadius = 4F;
			this.butCopy.Image = global::OpenDental.Properties.Resources.Right;
			this.butCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCopy.Location = new System.Drawing.Point(400, 322);
			this.butCopy.Name = "butCopy";
			this.butCopy.Size = new System.Drawing.Size(75, 24);
			this.butCopy.TabIndex = 4;
			this.butCopy.Text = "Copy";
			this.butCopy.Click += new System.EventHandler(this.butCopy_Click);
			// 
			// butNew
			// 
			this.butNew.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butNew.Autosize = true;
			this.butNew.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNew.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNew.CornerRadius = 4F;
			this.butNew.Image = global::OpenDental.Properties.Resources.Add;
			this.butNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butNew.Location = new System.Drawing.Point(615, 635);
			this.butNew.Name = "butNew";
			this.butNew.Size = new System.Drawing.Size(80, 24);
			this.butNew.TabIndex = 6;
			this.butNew.Text = "New";
			this.butNew.Click += new System.EventHandler(this.butNew_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(794, 635);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 8;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butTools
			// 
			this.butTools.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butTools.Autosize = true;
			this.butTools.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTools.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTools.CornerRadius = 4F;
			this.butTools.Location = new System.Drawing.Point(400, 635);
			this.butTools.Name = "butTools";
			this.butTools.Size = new System.Drawing.Size(75, 24);
			this.butTools.TabIndex = 5;
			this.butTools.Text = "Tools";
			this.butTools.Click += new System.EventHandler(this.butTools_Click);
			// 
			// butDefault
			// 
			this.butDefault.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDefault.Autosize = true;
			this.butDefault.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDefault.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDefault.CornerRadius = 4F;
			this.butDefault.Location = new System.Drawing.Point(15, 635);
			this.butDefault.Name = "butDefault";
			this.butDefault.Size = new System.Drawing.Size(75, 24);
			this.butDefault.TabIndex = 19;
			this.butDefault.Text = "Defaults";
			this.butDefault.Click += new System.EventHandler(this.butDefault_Click);
			// 
			// FormSheetDefs
			// 
			this.ClientSize = new System.Drawing.Size(881, 669);
			this.Controls.Add(this.butDefault);
			this.Controls.Add(this.butTools);
			this.Controls.Add(this.butCopy2);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.comboLabel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butCopy);
			this.Controls.Add(this.grid1);
			this.Controls.Add(this.grid2);
			this.Controls.Add(this.butNew);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormSheetDefs";
			this.ShowInTaskbar = false;
			this.Text = "Sheet Defs";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSheetDefs_FormClosing);
			this.Load += new System.EventHandler(this.FormSheetDefs_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormSheetDefs_Load(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup,true)){
				butNew.Enabled=false;
				butCopy.Enabled=false;
				butCopy2.Enabled=false;
				grid2.Enabled=false;
			}
			FillGrid1();
			FillGrid2();
			comboLabel.Items.Clear();
			comboLabel.Items.Add(Lan.g(this,"Default"));
			comboLabel.SelectedIndex=0;
			LabelList=new List<SheetDef>();
			for(int i=0;i<_listSheetDefs.Count;i++){
				if(_listSheetDefs[i].SheetType==SheetTypeEnum.LabelPatient){
					LabelList.Add(_listSheetDefs[i].Copy());
				}
			}
			for(int i=0;i<LabelList.Count;i++){
				comboLabel.Items.Add(LabelList[i].Description);
				if(PrefC.GetLong(PrefName.LabelPatientDefaultSheetDefNum)==LabelList[i].SheetDefNum){
					comboLabel.SelectedIndex=i+1;
				}
			}
		}

		private void FillGrid1(){
			grid1.BeginUpdate();
			grid1.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableSheetDef","Description"),170);
			grid1.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableSheetDef","Type"),100);
			grid1.Columns.Add(col);
			grid1.Rows.Clear();
			ODGridRow row;
			internalList=SheetsInternal.GetAllInternal();
			for(int i=0;i<internalList.Count;i++){
				row=new ODGridRow();
				row.Cells.Add(internalList[i].Description);//Enum.GetNames(typeof(SheetInternalType))[i]);
				row.Cells.Add(internalList[i].SheetType.ToString());
				grid1.Rows.Add(row);
			}
			grid1.EndUpdate();
		}

		private void FillGrid2(){
			SheetDefs.RefreshCache();
			SheetFieldDefs.RefreshCache();
			_listSheetDefs=SheetDefs.GetDeepCopy();
			grid2.BeginUpdate();
			grid2.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableSheetDef","Description"),170);
			grid2.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableSheetDef","Type"),100);
			grid2.Columns.Add(col);
			grid2.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listSheetDefs.Count;i++){
				row=new ODGridRow();
				row.Cells.Add(_listSheetDefs[i].Description);
				row.Cells.Add(_listSheetDefs[i].SheetType.ToString());
				grid2.Rows.Add(row);
			}
			grid2.EndUpdate();
		}

		private void butNew_Click(object sender, System.EventArgs e) {
			//This button is not enabled unless user has appropriate permission for setup.
			//Not allowed to change sheettype once a sheet is created, so we need to let user pick.
			FormSheetDef FormS=new FormSheetDef();
			FormS.IsInitial=true;
			FormS.IsReadOnly=false;
			SheetDef sheetdef=new SheetDef();
			sheetdef.FontName="Microsoft Sans Serif";
			sheetdef.FontSize=9;
			sheetdef.Height=1100;
			sheetdef.Width=850;
			FormS.SheetDefCur=sheetdef;
			FormS.ShowDialog();
			if(FormS.DialogResult!=DialogResult.OK){
				return;
			}
			//what about parameters?
			sheetdef.SheetFieldDefs=new List<SheetFieldDef>();
			sheetdef.IsNew=true;
			FormSheetDefEdit FormSD=new FormSheetDefEdit(sheetdef);
			FormSD.ShowDialog();//It will be saved to db inside this form.
			FillGrid2();
			for(int i=0;i<_listSheetDefs.Count;i++){
				if(_listSheetDefs[i].SheetDefNum==sheetdef.SheetDefNum){
					grid2.SetSelected(i,true);
				}
			}
			changed=true;
		}
		
		private void butCopy2_Click(object sender, EventArgs e) {
			//This button is not enabled unless user has appropriate permission for setup.
			if(grid2.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select a sheet from the list above first.");
				return;
			}
			SheetDef sheetdef=_listSheetDefs[grid2.GetSelectedIndex()].Copy();
			sheetdef.Description=sheetdef.Description+"2";
			SheetDefs.GetFieldsAndParameters(sheetdef);
			sheetdef.IsNew=true;
			SheetDefs.InsertOrUpdate(sheetdef);
			FillGrid2();
			for(int i=0;i<_listSheetDefs.Count;i++){
				if(_listSheetDefs[i].SheetDefNum==sheetdef.SheetDefNum) {
					grid2.SetSelected(i,true);
				}
			}
		}

		private void butCopy_Click(object sender,EventArgs e) {
			if(grid1.GetSelectedIndex()==-1){
				MsgBox.Show(this,"Please select an internal sheet from the list above first.");
				return;
			}
			SheetDef sheetdef=internalList[grid1.GetSelectedIndex()].Copy();
			sheetdef.IsNew=true;
			SheetDefs.InsertOrUpdate(sheetdef);
			if(sheetdef.SheetType==SheetTypeEnum.MedicalHistory
				&& (sheetdef.Description=="Medical History New Patient" || sheetdef.Description=="Medical History Update")) 
			{
				MsgBox.Show(this,"This is just a template, it may contain allergies and problems that do not exist in your setup.");
			}
			grid1.SetSelected(false);
			FillGrid2();
			for(int i=0;i<_listSheetDefs.Count;i++){
				if(_listSheetDefs[i].SheetDefNum==sheetdef.SheetDefNum){
					grid2.SetSelected(i,true);
				}
			}
		}

		private void butDefault_Click(object sender,EventArgs e) {
			FormSheetDefDefaults FormSDD=new FormSheetDefDefaults();
			FormSDD.ShowDialog();
		}

		private void grid1_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormSheetDefEdit FormS=new FormSheetDefEdit(internalList[e.Row]);
			FormS.IsInternal=true;
			FormS.ShowDialog();
		}

		private void grid1_Click(object sender,EventArgs e) {
			if(grid1.GetSelectedIndex()>-1) {
				grid2.SetSelected(false);
			}
		}
		
		private void grid2_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			SheetDef sheetdef=_listSheetDefs[e.Row];
			SheetDefs.GetFieldsAndParameters(sheetdef);
			FormSheetDefEdit FormS=new FormSheetDefEdit(sheetdef);
			FormS.ShowDialog();
			FillGrid2();
			for(int i=0;i<_listSheetDefs.Count;i++){
				if(_listSheetDefs[i].SheetDefNum==sheetdef.SheetDefNum){
					grid2.SetSelected(i,true);
				}
			}
			changed=true;
		}

		private void grid2_Click(object sender,EventArgs e) {
			if(grid2.GetSelectedIndex()>-1) {
				grid1.SetSelected(false);
			}
		}

		private void comboLabel_DropDown(object sender,EventArgs e) {
			comboLabel.Items.Clear();
			comboLabel.Items.Add(Lan.g(this,"Default"));
			comboLabel.SelectedIndex=0;
			LabelList=new List<SheetDef>();
			for(int i=0;i<_listSheetDefs.Count;i++){
				if(_listSheetDefs[i].SheetType==SheetTypeEnum.LabelPatient){
					LabelList.Add(_listSheetDefs[i].Copy());
				}
			}
			for(int i=0;i<LabelList.Count;i++){
				comboLabel.Items.Add(LabelList[i].Description);
				if(PrefC.GetLong(PrefName.LabelPatientDefaultSheetDefNum)==LabelList[i].SheetDefNum){
					comboLabel.SelectedIndex=i+1;
				}
			}
		}

		private void comboLabel_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboLabel.SelectedIndex==0){
				Prefs.UpdateLong(PrefName.LabelPatientDefaultSheetDefNum,0);
			}
			else{
				Prefs.UpdateLong(PrefName.LabelPatientDefaultSheetDefNum,LabelList[comboLabel.SelectedIndex-1].SheetDefNum);
			}
			DataValid.SetInvalid(InvalidType.Prefs);
		}

		private void butTools_Click(object sender,EventArgs e) {
			FormSheetTools formST=new FormSheetTools();
			formST.ShowDialog();
			if(formST.HasSheetsChanged) {
				FillGrid2();
				if(formST.ImportedSheetDefNum==0) {
					return;
				}
				for(int i=0;i<_listSheetDefs.Count;i++) {
					if(_listSheetDefs[i].SheetDefNum==formST.ImportedSheetDefNum) {
						grid2.SetSelected(i,true);
					}
				}
			}
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void FormSheetDefs_FormClosing(object sender,FormClosingEventArgs e) {
			if(changed){
				DataValid.SetInvalid(InvalidType.Sheets);
			}
		}
	}
}





















