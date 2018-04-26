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
	public partial class FormProcCodeEditMore:ODForm {
		private List<Fee> _listFees;
		private ProcedureCode _procCode;
		private List<FeeSched> _listFeeScheds;

		public FormProcCodeEditMore(ProcedureCode procCode) {
			InitializeComponent();
			Lan.F(this);
			_procCode=procCode;
		}

		private void FormProcCodeEditMore_Load(object sender,EventArgs e) {
			_listFeeScheds=FeeScheds.GetDeepCopy(true);
			FillAndSortListFees();
			FillGrid();
		}

		///<summary>It's unnecessary to continuously re-fill the list of fees that this window uses to display.
		///This method is a helper method that can be manually called when it is known that the fees for this code have changed.
		///This removes the need to have it within FillGrid().</summary>
		private void FillAndSortListFees() {
			_listFees=Fees.GetFeesForCode(_procCode.CodeNum);
			//Create a temporary list that will be used to keep track of the Fees after they've been sorted within each fee schedule.
			List<Fee> listFees=new List<Fee>();
			for(int i=0;i<_listFeeScheds.Count;i++) {
				listFees.AddRange(_listFees.FindAll(fee => fee.FeeSched==_listFeeScheds[i].FeeSchedNum && fee.CodeNum==_procCode.CodeNum)
					.OrderBy(fee => fee.ClinicNum)
					.ThenBy(fee => fee.ProvNum));
			}
			_listFees=new List<Fee>(listFees);//Update the class-wide list that FillGrid() uses so the fees are displayed correctly.
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				col=new ODGridColumn(Lan.g("TableProcCodeEditMore","Schedule"),200);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableProcCodeEditMore","Provider"),135);
				gridMain.Columns.Add(col);
			}
			else {//Using clinics.
				col=new ODGridColumn(Lan.g("TableProcCodeEditMore","Schedule"),130);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableProcCodeEditMore","Clinic"),130);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableProcCodeEditMore","Provider"),75);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g("TableProcCodeEditMore","Amount"),100,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			long lastFeeSched=0;
			for(int i=0;i<_listFees.Count;i++) {
				row=new ODGridRow();
				if(_listFees[i].FeeSched!=lastFeeSched) {
					row.Cells.Add(FeeScheds.GetDescription(_listFees[i].FeeSched));
					row.Bold=true;
					lastFeeSched=_listFees[i].FeeSched;
					row.ColorBackG=Color.LightBlue;
					if(_listFees[i].ClinicNum!=0 || _listFees[i].ProvNum!=0) { //FeeSched change, but not with a default fee. Insert placeholder row.
						if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
							row.Cells.Add("");
						}
						row.Cells.Add("");
						row.Cells.Add("");
						Fee fee=new Fee();
						fee.FeeSched=_listFees[i].FeeSched;
						row.Tag=fee;
						gridMain.Rows.Add(row);
						//Now that we have a placeholder for the default fee (none was found), go about adding the next row (non-default fee).
						row=new ODGridRow();
						row.Cells.Add("");
					}
				}
				else {
					row.Cells.Add("");
				}
				row.Tag=_listFees[i];
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) { //Using clinics
					row.Cells.Add(Clinics.GetAbbr(_listFees[i].ClinicNum)); //Returns "" if invalid clinicnum (ie. 0)
				}
				row.Cells.Add(Providers.GetAbbr(_listFees[i].ProvNum)); //Returns "" if invalid provnum (ie. 0)
				row.Cells.Add(_listFees[i].Amount.ToString("n"));
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,UI.ODGridClickEventArgs e) {
			Fee fee=(Fee)gridMain.Rows[e.Row].Tag;
			FormFeeEdit FormFE=new FormFeeEdit();
			if(fee.FeeNum==0) {
				FormFE.IsNew=true;
				fee.CodeNum=_procCode.CodeNum;
				Fees.Insert(fee);//Pre-insert the fee before opening the edit window.
			}
			FormFE.FeeCur=fee;
			FormFE.ShowDialog();
			if(FormFE.DialogResult==DialogResult.OK) {
				//FormFE could have manipulated the fee.  Refresh our local cache and grids to reflect the changes.
				FillAndSortListFees();
				FillGrid();
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}