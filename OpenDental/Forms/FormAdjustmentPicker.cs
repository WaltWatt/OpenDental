using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;

namespace OpenDental {
	public partial class FormAdjustmentPicker:ODForm {
		private bool _isUnattachedMode;
		private long _patNum;
		private List<Adjustment> _listAdjustments;
		private List<Adjustment> _listAdjustmentsFiltered;
		public Adjustment SelectedAdjustment;

		public FormAdjustmentPicker(long patNum,bool isUnattachedMode=false,List<Adjustment> listAdjustments=null) {
			InitializeComponent();
			Lan.F(this);
			_patNum=patNum;
			_isUnattachedMode=isUnattachedMode;
			_listAdjustments=listAdjustments;
		}

		private void FormAdjustmentPicker_Load(object sender,EventArgs e) {
			if(_isUnattachedMode) {
				checkUnattached.Checked=true;
				checkUnattached.Enabled=false;
			}
			if(_listAdjustments==null) {
				_listAdjustments=Adjustments.Refresh(_patNum).ToList();
			}
			FillGrid();
		}

		private void FillGrid(){
			_listAdjustmentsFiltered=_listAdjustments;
			if(checkUnattached.Checked) {
				_listAdjustmentsFiltered=_listAdjustments.FindAll(x => x.ProcNum==0);
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Date"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"PatNum"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Type"),120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Amount"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Has Proc"),0,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			foreach(Adjustment adjCur in _listAdjustmentsFiltered) {
				row=new ODGridRow();
				row.Cells.Add(adjCur.AdjDate.ToShortDateString());
				row.Cells.Add(adjCur.PatNum.ToString());
				row.Cells.Add(Defs.GetName(DefCat.AdjTypes,adjCur.AdjType));
				row.Cells.Add(adjCur.AdjAmt.ToString("F"));
				if(adjCur.ProcNum!=0){
					row.Cells.Add("X");
				}
				else{
					row.Cells.Add("");
				}
				row.Tag=adjCur;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void checkUnattached_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			SelectedAdjustment=_listAdjustmentsFiltered[gridMain.GetSelectedIndex()];
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			SelectedAdjustment=_listAdjustmentsFiltered[gridMain.GetSelectedIndex()];
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}