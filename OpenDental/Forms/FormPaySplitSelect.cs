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
	public partial class FormPaySplitSelect:ODForm {
		private List<PaySplit> _listPaySplits;
		private ODGrid gridMain;
		private long _patNum;
		public bool IsPrePay;
		public long SplitNumCur;
		///<summary>This list will contain all selected PaySplits when OK is pressed.</summary>
		public List<PaySplit> ListSelectedSplits;
		public List<PaySplit> ListUnallocatedSplits;
		public decimal AmtAllocated;
		public FormPaySplitSelect(long patNum) {
			_patNum=patNum;
			InitializeComponent();
		}

		private void FormPaySplitSelect_Load(object sender, System.EventArgs e) {
			FindSplits(false);
			FillGrid(false);
		}

		private void FindSplits(bool isShowAll) {
			ListSelectedSplits=new List<PaySplit>();
			if(IsPrePay) {
				if(isShowAll) {//this will find all splits for the family, even if they have already been allocated.
					GetSplitsForPats(isShowAll);
					return;
				}
				if(ListUnallocatedSplits==null) {
					_listPaySplits=PaySplits.GetPrepayForFam(Patients.GetFamily(_patNum));
					List<PaySplit> listSplitsForSplits=PaySplits.GetSplitsForPrepay(_listPaySplits);
					for(int i = _listPaySplits.Count-1;i>=0;i--) {
						PaySplit split=_listPaySplits[i];
						List<PaySplit> listSplitsForSplit=listSplitsForSplits.FindAll(x => x.FSplitNum==split.SplitNum);
						decimal splitTotal=0;
						foreach(PaySplit paySplit in listSplitsForSplit) {
							if(paySplit.SplitNum==SplitNumCur) {
								continue;
							}
							splitTotal+=(decimal)paySplit.SplitAmt;
						}
						decimal leftOverAmt=(decimal)split.SplitAmt+splitTotal+AmtAllocated; //splitTotal should be negative.
						if(leftOverAmt<=0) {
							_listPaySplits.Remove(split);
						}
						else {
							split.SplitAmt=(double)leftOverAmt;//This will cause the left over amount to show up in the grid.  We don't do any saving in this form.
						}
					}
				}
				else {
					_listPaySplits=ListUnallocatedSplits;
				}
			}
			else {
				GetSplitsForPats(isShowAll);
			}
		}

		/// <summary>Get all splits for family. If show all is checked, find all that are positive prepayments even if already attached.</summary>
		private void GetSplitsForPats(bool isShowAll) {
			Family fam=Patients.GetFamily(_patNum);
			_listPaySplits=PaySplits.GetForPats(fam.ListPats.Select(x => x.PatNum).ToList());
			if(isShowAll) {
				_listPaySplits=_listPaySplits.FindAll(x => x.UnearnedType!=0 && x.SplitAmt>0);//don't show the automated negative offset
			}
		}

		private void FillGrid(bool isShowAll){
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableUnallocatedPaysplits","Date"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableUnallocatedPaysplits","Patient Name"),120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableUnallocatedPaysplits","Prov"),120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableUnallocatedPaysplits","UnearnedType"),150);
			gridMain.Columns.Add(col);
			if(isShowAll) {
				//Original because we don't know how much has been allocated
				col=new ODGridColumn(Lan.g("TableUnallocatedPaysplits","Amt Original"),60,HorizontalAlignment.Right);
			}
			else {
				col=new ODGridColumn(Lan.g("TableUnallocatedPaysplits","Amt Left"),60,HorizontalAlignment.Right);
			}
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			foreach(PaySplit paySplit in _listPaySplits) {
				row=new ODGridRow();
				row.Cells.Add(paySplit.DatePay.ToShortDateString());
				row.Cells.Add(Patients.GetNameLF(paySplit.PatNum));
				row.Cells.Add(Providers.GetAbbr(paySplit.ProvNum));
				row.Cells.Add(Defs.GetName(DefCat.PaySplitUnearnedType,paySplit.UnearnedType));
				row.Cells.Add(paySplit.SplitAmt.ToString("F"));
				row.Tag=paySplit;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			PaySplit paySplit=(PaySplit)gridMain.Rows[e.Row].Tag;
			ListSelectedSplits.Clear();
			ListSelectedSplits.Add(paySplit);
			DialogResult=DialogResult.OK;
		}

		private void checkShowAll_CheckedChanged(object sender,EventArgs e) {
			FindSplits(checkShowAll.Checked);
			FillGrid(checkShowAll.Checked);
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1){
				MsgBox.Show(this,"Please select an item first.");
				return;
			}
			ListSelectedSplits.Clear();
			PaySplit paySplit=(PaySplit)gridMain.Rows[gridMain.GetSelectedIndex()].Tag;
			ListSelectedSplits.Add(paySplit);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}