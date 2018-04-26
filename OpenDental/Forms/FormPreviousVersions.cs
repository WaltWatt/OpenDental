using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormPreviousVersions:ODForm {

		public FormPreviousVersions() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormPreviousVersions_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Version"),117);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Date"),117);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row=null;
			List<UpdateHistory> listUpdateHistories=UpdateHistories.GetAll();
			foreach(UpdateHistory updateHistory in listUpdateHistories) {
				row=new ODGridRow();
				row.Cells.Add(updateHistory.ProgramVersion);
				row.Cells.Add(updateHistory.DateTimeUpdated.ToString());
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}
	}
}