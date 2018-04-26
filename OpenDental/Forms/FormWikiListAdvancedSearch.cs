using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormWikiListAdvancedSearch:ODForm {
		public int[] SelectedColumnIndices=new int[0];
		private List<WikiListHeaderWidth> _listColHeaders;

		public FormWikiListAdvancedSearch(List<WikiListHeaderWidth> headers) {
			InitializeComponent();
			_listColHeaders=headers;
			Lan.F(this);
		}

		private void FormWikiListAdvancedSearch_Load(object sender,EventArgs e) {
			FillGrid();
		}

		/// <summary>Populates the grid with the current Wiki's column headers</summary>
		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn("",80);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			foreach(WikiListHeaderWidth header in _listColHeaders){
				row=new ODGridRow(header.ColName);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butOkay_Click(object sender,EventArgs e) {
			SelectedColumnIndices=gridMain.SelectedIndices;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}
