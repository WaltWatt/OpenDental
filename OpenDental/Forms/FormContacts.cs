using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Collections.Generic;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormContacts : ODForm {
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox listCategory;
		private OpenDental.UI.Button butAdd;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private UI.ODGrid gridMain;
		private Contact[] ContactList;
		private List<Def> _listContactCategoryDefs;

		///<summary></summary>
		public FormContacts()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormContacts));
			this.butOK = new OpenDental.UI.Button();
			this.listCategory = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.butAdd = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butOK.Location = new System.Drawing.Point(799,658);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75,25);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&Close";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// listCategory
			// 
			this.listCategory.Location = new System.Drawing.Point(5,30);
			this.listCategory.Name = "listCategory";
			this.listCategory.Size = new System.Drawing.Size(101,264);
			this.listCategory.TabIndex = 2;
			this.listCategory.SelectedIndexChanged += new System.EventHandler(this.listCategory_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(5,12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100,16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Category";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Location = new System.Drawing.Point(797,494);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(75,25);
			this.butAdd.TabIndex = 5;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(117, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(669, 671);
			this.gridMain.TabIndex = 12;
			this.gridMain.Title = "Contacts";
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableContact";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// FormContacts
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butOK;
			this.ClientSize = new System.Drawing.Size(886,693);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.listCategory);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(330,344);
			this.Name = "FormContacts";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Contacts";
			this.Load += new System.EventHandler(this.FormContacts_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormContacts_Load(object sender, System.EventArgs e) {
			_listContactCategoryDefs=Defs.GetDefsForCategory(DefCat.ContactCategories,true);
			for(int i=0;i<_listContactCategoryDefs.Count;i++){
				listCategory.Items.Add(_listContactCategoryDefs[i].ItemName);
			}
			if(listCategory.Items.Count>0) {
				listCategory.SelectedIndex=0;
			}
		}

		private void listCategory_SelectedIndexChanged(object sender, System.EventArgs e) {
			FillGrid();
		}

		private void FillGrid(){
			ContactList=Contacts.Refresh(_listContactCategoryDefs[listCategory.SelectedIndex].DefNum);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableContacts","Last Name"),100));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableContacts","First Name"),100));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableContacts","Wk Phone"),90));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableContacts","Fax"),90));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableContacts","Note"),250));
			gridMain.Rows.Clear();
			ODGridRow row;
			foreach(Contact contactCur in ContactList) {
				row=new ODGridRow();
				row.Cells.Add(contactCur.LName);
				row.Cells.Add(contactCur.FName);
				row.Cells.Add(contactCur.WkPhone);
				row.Cells.Add(contactCur.Fax);
				row.Cells.Add(contactCur.Notes);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,UI.ODGridClickEventArgs e) {
			FormContactEdit FormCE=new FormContactEdit();
			FormCE.ContactCur=ContactList[e.Row];
			FormCE.ShowDialog();
			if(FormCE.DialogResult==DialogResult.OK) {
				FillGrid();
			}
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			Contact ContactCur=new Contact();
			ContactCur.Category=_listContactCategoryDefs[listCategory.SelectedIndex].DefNum;
			FormContactEdit FormCE=new FormContactEdit();
			FormCE.ContactCur=ContactCur;
			FormCE.IsNew=true;
			FormCE.ShowDialog();
			if(FormCE.DialogResult==DialogResult.OK) {
				FillGrid();
			}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		


		


	}
}






















