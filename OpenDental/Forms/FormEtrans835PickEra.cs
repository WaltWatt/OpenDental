using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEtrans835PickEra:ODForm {
		private List<Etrans> _listEtrans;

		public FormEtrans835PickEra(List<Etrans> listEtrans) {
			InitializeComponent();
			Lan.F(this);
			_listEtrans=listEtrans;
		}
		
		private void FormEtrans835PickEra_Load(object sender,EventArgs e) {
			FillGridEras();
		}

		private void FillGridEras() {
			gridEras.BeginUpdate();
			gridEras.Columns.Clear();
			gridEras.Columns.Add(new UI.ODGridColumn(Lan.g(this,"Date Received"),0));
			gridEras.Rows.Clear();
			for(int i=0;i<_listEtrans.Count;i++) {
				UI.ODGridRow row=new UI.ODGridRow();
				row.Cells.Add(_listEtrans[i].DateTimeTrans.ToShortDateString());
				gridEras.Rows.Add(row);
			}
			gridEras.EndUpdate();
		}

		private void gridEras_CellDoubleClick(object sender,UI.ODGridClickEventArgs e) {
			FormEtrans835Edit.ShowEra(_listEtrans[gridEras.SelectedIndices[0]]);
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

	}
}