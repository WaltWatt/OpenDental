using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;
using OpenDental.UI;

namespace OpenDental{
///<summary></summary>
	public class FormZipCodes : ODForm {
		private OpenDental.UI.Button butAdd;
		private OpenDental.UI.Button butDelete;
		private OpenDental.UI.Button butClose;
		private System.ComponentModel.Container components = null;
		private bool changed;
		private UI.ODGrid gridZipCode;
		private List<ZipCode> _listZipCodes;

		///<summary></summary>
		public FormZipCodes(){
			InitializeComponent();
			Lan.F(this);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormZipCodes));
			this.butAdd = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.gridZipCode = new OpenDental.UI.ODGrid();
			this.SuspendLayout();
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(615, 374);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(76, 26);
			this.butAdd.TabIndex = 28;
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
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(615, 513);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(76, 26);
			this.butClose.TabIndex = 26;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Location = new System.Drawing.Point(615, 410);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(76, 26);
			this.butDelete.TabIndex = 31;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// gridZipCode
			// 
			this.gridZipCode.AllowSortingByColumn = true;
			this.gridZipCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridZipCode.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridZipCode.HasAddButton = false;
			this.gridZipCode.HasDropDowns = false;
			this.gridZipCode.HasMultilineHeaders = false;
			this.gridZipCode.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridZipCode.HeaderHeight = 15;
			this.gridZipCode.HScrollVisible = false;
			this.gridZipCode.Location = new System.Drawing.Point(12, 17);
			this.gridZipCode.Name = "gridZipCode";
			this.gridZipCode.ScrollValue = 0;
			this.gridZipCode.Size = new System.Drawing.Size(581, 522);
			this.gridZipCode.TabIndex = 32;
			this.gridZipCode.Title = "Zip Codes";
			this.gridZipCode.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridZipCode.TitleHeight = 18;
			this.gridZipCode.TranslationName = "TableZipCodes";
			this.gridZipCode.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridZipCode_CellDoubleClick);
			// 
			// FormZipCodes
			// 
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(715, 563);
			this.Controls.Add(this.gridZipCode);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormZipCodes";
			this.ShowInTaskbar = false;
			this.Text = "Zip Codes";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormZipCodes_Closing);
			this.Load += new System.EventHandler(this.FormZipCodes_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormZipCodes_Load(object sender, System.EventArgs e) {
		  FillGrid();
		}

		private void FillGrid(){
			ZipCodes.RefreshCache();
			_listZipCodes=ZipCodes.GetDeepCopy();
			gridZipCode.BeginUpdate();
			gridZipCode.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"ZipCode"),75);
			gridZipCode.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"City"),270);
			gridZipCode.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"State"),50);
			gridZipCode.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Frequent"),80);
			gridZipCode.Columns.Add(col);
			gridZipCode.Rows.Clear();
			ODGridRow row;
			foreach(ZipCode zip in _listZipCodes) {
				row=new ODGridRow();
				row.Cells.Add(zip.ZipCodeDigits);
				row.Cells.Add(zip.City);
				row.Cells.Add(zip.State);
				row.Cells.Add((zip.IsFrequent ? "X" : ""));
				row.Tag=zip;
				gridZipCode.Rows.Add(row);
			}
			gridZipCode.EndUpdate();
		}

		private void gridZipCode_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(gridZipCode.SelectedIndices.Length==0) {
				return;
			}
			FormZipCodeEdit FormZCE=new FormZipCodeEdit();
			FormZCE.ZipCodeCur=(ZipCode)gridZipCode.Rows[e.Row].Tag;
			FormZCE.ShowDialog();
			if(FormZCE.DialogResult!=DialogResult.OK) {
				return;
			}
			changed=true;
			FillGrid();
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(gridZipCode.SelectedIndices.Length==0) {
				MessageBox.Show(Lan.g(this,"Please select an item first."));
				return;
			}	
			ZipCode ZipCur=gridZipCode.SelectedTag<ZipCode>();
			if(MessageBox.Show(Lan.g(this,"Delete Zipcode?"),"",MessageBoxButtons.OKCancel)!=DialogResult.OK){
				return;   
			}
			changed=true;
			ZipCodes.Delete(ZipCur);
			FillGrid();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			FormZipCodeEdit FormZCE=new FormZipCodeEdit();
			FormZCE.ZipCodeCur=new ZipCode();
			FormZCE.IsNew=true;
			FormZCE.ShowDialog();
			if(FormZCE.DialogResult!=DialogResult.OK){
				return;
			}
			changed=true;
			FillGrid(); 				
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void FormZipCodes_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(changed){
				DataValid.SetInvalid(InvalidType.ZipCodes);
			}
		}
	

	}
}
