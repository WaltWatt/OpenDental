using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using OpenDental;

namespace OpenDental {
	public partial class FormLoincs:ODForm {
		public bool IsSelectionMode;
		public Loinc SelectedLoinc;
		private List<Loinc> listLoincSearch;
		public Loinc LoincCur;

		public FormLoincs() {
			InitializeComponent();
		}

		private void FormLoincPicker_Load(object sender,EventArgs e) {
			listLoincSearch=new List<Loinc>();
			ActiveControl=textCode;
		}

		private void fillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn("Loinc Code",80);//,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Status",80);//,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Long Name",500);//,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("UCUM Units",100);//,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Order or Observation",100);//,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			listLoincSearch=Loincs.GetBySearchString(textCode.Text);
			for(int i=0;i<listLoincSearch.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(listLoincSearch[i].LoincCode);
				row.Cells.Add(listLoincSearch[i].StatusOfCode);
				row.Cells.Add(listLoincSearch[i].NameLongCommon);
				row.Cells.Add(listLoincSearch[i].UnitsUCUM);
				row.Cells.Add(listLoincSearch[i].OrderObs);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(IsSelectionMode) {
				SelectedLoinc=listLoincSearch[e.Row];
				DialogResult=DialogResult.OK;
			}
			//Nothing to do if not selection mode
		}

		private void butSearch_Click(object sender,EventArgs e) {
			fillGrid();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select a Loinc code from the list.");
				return;
			}
			if(IsSelectionMode) {
				SelectedLoinc=listLoincSearch[gridMain.GetSelectedIndex()];
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}
