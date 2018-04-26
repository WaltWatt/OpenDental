using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;
using OpenDental.UI;
using System.Linq;

namespace OpenDental{
	/// <summary>
	/// </summary>
	public class FormPatFieldDefs:ODForm {
		private OpenDental.UI.Button butClose;
		private OpenDental.UI.Button butAdd;
		private System.ComponentModel.IContainer components;
		private Label label1;
		private System.Windows.Forms.ToolTip toolTip1;
		private UI.ODGrid gridMain;
		private bool _hasChanged;
		private UI.Button butDown;
		private UI.Button butUp;
		private List<PatFieldDef> _listPatFieldDefs;
		private MainMenu mainMenu1;
		private MenuItem menuItemSetup;
		private CheckBox checkDisplayRenamed;

		///<summary>Stale deep copy of _listPatFieldDefs to use with sync.</summary>
		private List<PatFieldDef> _listPatFieldDefsOld;
		///<summary></summary>
		public FormPatFieldDefs()
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPatFieldDefs));
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butClose = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.menuItemSetup = new System.Windows.Forms.MenuItem();
			this.checkDisplayRenamed = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(15, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(373, 39);
			this.label1.TabIndex = 8;
			this.label1.Text = "This is a list of extra fields that you can setup for patients.  After adding fie" +
    "lds to this list, you can set the value in the Family module.";
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(18, 72);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(370, 305);
			this.gridMain.TabIndex = 9;
			this.gridMain.Title = "Patient Field Defs";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TablePatientDefs";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(414, 351);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(79, 26);
			this.butClose.TabIndex = 1;
			this.butClose.Text = "Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(414, 72);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(79, 26);
			this.butAdd.TabIndex = 7;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(414, 210);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(79, 24);
			this.butDown.TabIndex = 161;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(414, 180);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(79, 24);
			this.butUp.TabIndex = 160;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSetup});
			// 
			// menuItemSetup
			// 
			this.menuItemSetup.Index = 0;
			this.menuItemSetup.Text = "Setup";
			this.menuItemSetup.Click += new System.EventHandler(this.menuItemSetup_Click);
			// 
			// checkDisplayRenamed
			// 
			this.checkDisplayRenamed.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
			this.checkDisplayRenamed.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkDisplayRenamed.Location = new System.Drawing.Point(18, 51);
			this.checkDisplayRenamed.Name = "checkDisplayRenamed";
			this.checkDisplayRenamed.Size = new System.Drawing.Size(370, 15);
			this.checkDisplayRenamed.TabIndex = 162;
			this.checkDisplayRenamed.TabStop = false;
			this.checkDisplayRenamed.Text = "Display patient fields that have been renamed";
			// 
			// FormPatFieldDefs
			// 
			this.ClientSize = new System.Drawing.Size(512, 389);
			this.Controls.Add(this.checkDisplayRenamed);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.butAdd);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Menu = this.mainMenu1;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(528, 400);
			this.Name = "FormPatFieldDefs";
			this.ShowInTaskbar = false;
			this.Text = "Patient Field Defs";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPatFieldDefs_FormClosing);
			this.Load += new System.EventHandler(this.FormPatFieldDefs_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormPatFieldDefs_Load(object sender, System.EventArgs e) {
			_listPatFieldDefs=PatFieldDefs.GetDeepCopy();
			_listPatFieldDefsOld=_listPatFieldDefs.Select(x => x.Copy()).ToList();
			checkDisplayRenamed.Checked=PrefC.GetBool(PrefName.DisplayRenamedPatFields);
			FillGrid();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			gridMain.AllowSortingByColumn=true;
			col=new ODGridColumn(Lan.g(this,"Field Name"),200);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Field Type"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Hidden"),150,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listPatFieldDefs.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listPatFieldDefs[i].FieldName);
				row.Cells.Add(_listPatFieldDefs[i].FieldType.ToString());
				row.Cells.Add(_listPatFieldDefs[i].IsHidden?"X":"");
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormPatFieldDefEdit FormPFDE=new FormPatFieldDefEdit();
			FormPFDE.FieldDef=_listPatFieldDefs[e.Row];
			FormPFDE.ShowDialog();
			if(FormPFDE.DialogResult==DialogResult.OK) {
				if(FormPFDE.FieldDef==null) {
					_listPatFieldDefs.Remove(_listPatFieldDefs[e.Row]);
				}
				else {
					_listPatFieldDefs[e.Row]=FormPFDE.FieldDef;
				}
				_hasChanged=true;
				FillGrid();
			}
		}

		private void menuItemSetup_Click(object sender,EventArgs e) {
			FormFieldDefLink FormFDL=new FormFieldDefLink(FieldLocations.Family);
			FormFDL.ShowDialog();
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item in the grid first.");
				return;
			}
			int[] selected=new int[gridMain.SelectedIndices.Length];
			Array.Copy(gridMain.SelectedIndices,selected,gridMain.SelectedIndices.Length);
			if(selected[0]==0) {
				return;
			}
			for(int i=0;i<selected.Length;i++) {
				_listPatFieldDefs.Reverse(selected[i]-1,2);
			}
			for(int i=0;i<_listPatFieldDefs.Count;i++) {
				_listPatFieldDefs[i].ItemOrder=i;//change itemOrder to reflect order changes.
			}
			FillGrid();
			for(int i=0;i<selected.Length;i++) {
				gridMain.SetSelected(selected[i]-1,true);
			}
			_hasChanged=true;
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an item in the grid first.");
				return;
			}
			int[] selected=new int[gridMain.SelectedIndices.Length];
			Array.Copy(gridMain.SelectedIndices,selected,gridMain.SelectedIndices.Length);
			if(selected[selected.Length-1]==_listPatFieldDefs.Count-1) {
				return;
			}
			for(int i=selected.Length-1;i>=0;i--) {//go backwards
				_listPatFieldDefs.Reverse(selected[i],2);
			}
			for(int i=0;i<_listPatFieldDefs.Count;i++) {
				_listPatFieldDefs[i].ItemOrder=i;//change itemOrder to reflect order changes.
			}
			FillGrid();
			for(int i=0;i<selected.Length;i++) {
				gridMain.SetSelected(selected[i]+1,true);
			}
			_hasChanged=true;
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			//Employers.Cur=new Employer();
			PatFieldDef def=new PatFieldDef();
			def.ItemOrder=_listPatFieldDefs.Count;
			FormPatFieldDefEdit FormPFDE=new FormPatFieldDefEdit();
			FormPFDE.FieldDef=def;
			FormPFDE.IsNew=true;
			FormPFDE.ShowDialog();
			if(FormPFDE.DialogResult==DialogResult.OK) {
				_hasChanged=true;
				_listPatFieldDefs.Add(FormPFDE.FieldDef);
				FillGrid();
			}
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			if(Prefs.UpdateBool(PrefName.DisplayRenamedPatFields,checkDisplayRenamed.Checked)) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			Close();
		}

		private void FormPatFieldDefs_FormClosing(object sender,FormClosingEventArgs e) {
			//Fix the item order just in case there was a duplicate.
			for(int i=0;i<_listPatFieldDefs.Count;i++) {
				if(_listPatFieldDefs[i].ItemOrder!=i) {
					_hasChanged=true;
				}
				_listPatFieldDefs[i].ItemOrder=i;
			}
			if(_hasChanged) {
				PatFieldDefs.Sync(_listPatFieldDefs,_listPatFieldDefsOld);//Update if anything has changed
				DataValid.SetInvalid(InvalidType.PatFields);
			}
		}
	}
}



























