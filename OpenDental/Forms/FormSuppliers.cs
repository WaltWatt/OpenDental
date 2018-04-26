using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormSuppliers:ODForm {
		private List<Supplier> _listSuppliers;

		public FormSuppliers() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSuppliers_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid(){
			_listSuppliers=Suppliers.GetAll();
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Name"),110);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Phone"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"CustomerID"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Website"),180);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"UserName"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Password"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Note"),150);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listSuppliers.Count;i++){
				row=new ODGridRow();
				row.Cells.Add(_listSuppliers[i].Name);
				row.Cells.Add(_listSuppliers[i].Phone);
				row.Cells.Add(_listSuppliers[i].CustomerId);
				row.Cells.Add(_listSuppliers[i].Website);
				row.Cells.Add(_listSuppliers[i].UserName);
				row.Cells.Add(_listSuppliers[i].Password);
				row.Cells.Add(_listSuppliers[i].Note);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			Supplier supp=new Supplier();
			supp.IsNew=true;
			FormSupplierEdit FormS=new FormSupplierEdit();
			FormS.Supp=supp;
			FormS.ShowDialog();
			if(FormS.DialogResult==DialogResult.OK) {
				FillGrid();
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormSupplierEdit FormS=new FormSupplierEdit();
			FormS.Supp=_listSuppliers[e.Row];
			FormS.ShowDialog();
			if(FormS.DialogResult==DialogResult.OK) {
				FillGrid();
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

		

		

		
	}
}