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
	public partial class FormAlertCategorySetup:ODForm {

		private List<AlertCategory> _listInternalAlertCategory=new List<AlertCategory>();
		private List<AlertCategory> _listCustomAlertCategory=new List<AlertCategory>();

		public FormAlertCategorySetup() {
			InitializeComponent();
			Lan.F(this);
		}
		
		private void FormAlertCategorySetup_Load(object sender,EventArgs e) {
			FillGrids();
		}

		private void FillGrids(long selectedIneranlKey=0,long selectedCustomKey=0) {
			_listCustomAlertCategory.Clear();
			_listInternalAlertCategory.Clear();
			AlertCategories.GetDeepCopy().ForEach(x =>
			{
				if(x.IsHQCategory) {
					_listInternalAlertCategory.Add(x);
				}
				else {
					_listCustomAlertCategory.Add(x);
				}
			});
			_listInternalAlertCategory.OrderBy(x => x.InternalName);
			_listCustomAlertCategory.OrderBy(x => x.InternalName);
			FillInternalGrid(selectedIneranlKey);
			FillCustomGrid(selectedCustomKey);
		}

		private void FillInternalGrid(long selectedIneranlKey) {
			gridInternal.BeginUpdate();
			gridInternal.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Description"),100);
			gridInternal.Columns.Add(col);
			gridInternal.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listInternalAlertCategory.Count;i++){
				row=new ODGridRow();
				row.Cells.Add(_listInternalAlertCategory[i].Description);
				row.Tag=_listInternalAlertCategory[i].AlertCategoryNum;
				int index=gridInternal.Rows.Add(row);
				if(selectedIneranlKey==_listInternalAlertCategory[i].AlertCategoryNum) {
					gridCustom.SetSelected(index,true);
				}
			}
			gridInternal.EndUpdate();
		}
		
		private void FillCustomGrid(long selectedCustomKey) {
			gridCustom.BeginUpdate();
			gridCustom.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Description"),100);
			gridCustom.Columns.Add(col);
			gridCustom.Rows.Clear();
			ODGridRow row;
			int index=0;
			for(int i=0;i<_listCustomAlertCategory.Count;i++){
				row=new ODGridRow();
				row.Cells.Add(_listCustomAlertCategory[i].Description);
				row.Tag=_listCustomAlertCategory[i].AlertCategoryNum;
				index=gridCustom.Rows.Add(row);
				if(selectedCustomKey!=_listCustomAlertCategory[i].AlertCategoryNum) {
					index=0;
				}
			}
			if(index!=0) {
				gridCustom.SetSelected(index,true);
			}
			gridCustom.EndUpdate();
		}
		
		private void gridInternal_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormAlertCategoryEdit formACE=new FormAlertCategoryEdit(_listInternalAlertCategory[e.Row]);
			if(formACE.ShowDialog()==DialogResult.OK) {
				FillGrids();
			}
		}

		private void gridCustom_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormAlertCategoryEdit formACE=new FormAlertCategoryEdit(_listCustomAlertCategory[e.Row]);
			if(formACE.ShowDialog()==DialogResult.OK) {
				FillGrids();
			}
		}

		private void butCopy_Click(object sender,EventArgs e) {
			if(gridInternal.GetSelectedIndex()==-1){
				MsgBox.Show(this,"Please select an internal alert category from the list first.");
				return;
			}
			List<AlertCategory> listNewAlertCategories=_listCustomAlertCategory.Select(x => x.Copy()).ToList();
			AlertCategory alertCat=_listInternalAlertCategory[gridInternal.GetSelectedIndex()].Copy();
			alertCat.IsHQCategory=false;
			alertCat.Description+=Lan.g(this,"(Copy)");
			listNewAlertCategories.Add(alertCat);
			//alertCat.AlertCategoryNum reflects the original pre-copied PK. After sync this will be a new PK for the new row.
			List<AlertCategoryLink> listAlertCategoryType=AlertCategoryLinks.GetForCategory(alertCat.AlertCategoryNum);
			if(AlertCategories.Sync(listNewAlertCategories,_listCustomAlertCategory)) {
				//At this point alertCat has a new PK, so we need to update and insert our new copied alertCategoryLinks
				listAlertCategoryType.ForEach(x => {
					x.AlertCategoryNum=alertCat.AlertCategoryNum;
					AlertCategoryLinks.Insert(x);
				});
				DataValid.SetInvalid(InvalidType.AlertCategories,InvalidType.AlertCategoryLinks);
				FillGrids();
			}
		}

		private void butDuplicate_Click(object sender,EventArgs e) {
			if(gridCustom.GetSelectedIndex()==-1){
				MsgBox.Show(this,"Please select an custom alert category from the list first.");
				return;
			}
			List<AlertCategory> listNewAlertCategories=_listInternalAlertCategory.Select(x => x.Copy()).ToList();
			AlertCategory alertCat=_listCustomAlertCategory[gridCustom.GetSelectedIndex()].Copy();
			alertCat.Description+=Lan.g(this,"(Copy)");
			listNewAlertCategories.Add(alertCat);
			//alertCat.AlertCategoryNum reflects the original pre-copied PK. After sync this will be a new PK for the new row.
			List<AlertCategoryLink> listAlertCategoryType=AlertCategoryLinks.GetForCategory(alertCat.AlertCategoryNum);
			if(AlertCategories.Sync(listNewAlertCategories,_listInternalAlertCategory)) {
				//At this point alertCat has a new PK, so we need to update and insert our new copied alertCategoryLinks
				listAlertCategoryType.ForEach(x => {
					x.AlertCategoryNum=alertCat.AlertCategoryNum;
					AlertCategoryLinks.Insert(x);
				});
				DataValid.SetInvalid(InvalidType.AlertCategories,InvalidType.AlertCategoryLinks);
				FillGrids(selectedCustomKey: alertCat.AlertCategoryNum);
			}
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}