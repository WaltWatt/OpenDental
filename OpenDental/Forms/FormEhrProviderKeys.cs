using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEhrProviderKeys:ODForm {
		private List<EhrProvKey> _listKeys;

		public FormEhrProviderKeys() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormEhrProviderKeys_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid(){
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn("Last Name",80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("First Name",80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Year",30);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Key",100);
			gridMain.Columns.Add(col);
			_listKeys=EhrProvKeys.GetAllKeys();
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listKeys.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listKeys[i].LName);
				row.Cells.Add(_listKeys[i].FName);
				row.Cells.Add(_listKeys[i].YearValue.ToString());
				row.Cells.Add(_listKeys[i].ProvKey);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			EhrProvKey keycur=_listKeys[e.Row];
			keycur.IsNew=false;
			FormEhrProviderKeyEdit formE=new FormEhrProviderKeyEdit(keycur);
			formE.ShowDialog();
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			EhrProvKey keycur=new EhrProvKey();
			keycur.IsNew=true;
			FormEhrProviderKeyEdit formE=new FormEhrProviderKeyEdit(keycur);
			formE.ShowDialog();
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

	

		
	}
}