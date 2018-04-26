using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using OpenDental.UI;
using OpenDental;
using OpenDentBusiness;
using System.IO;
using Microsoft.VisualBasic.FileIO;


namespace OpenDental.User_Controls.SetupWizard {
	public partial class UserControlSetupWizFeeSched:SetupWizControl {
		///<summary>Copy of the cached fee schedules.</summary>
		private List<FeeSched> _listFeeScheds;
		///<summary>Stale deep copy of _listFeeScheds to use with sync.</summary>
		private List<FeeSched> _listFeeSchedsOld;
		private bool _isChanged;

		public UserControlSetupWizFeeSched() {
			InitializeComponent();
			this.OnControlDone += ControlDone;
		}

		private void UserControlSetupWizFeeSched_Load(object sender,EventArgs e) {
			_listFeeScheds=FeeScheds.GetDeepCopy();
			_listFeeSchedsOld=_listFeeScheds.Select(x => x.Copy()).ToList();
			FillGrid();
		}

		private void FillGrid(){
			_listFeeScheds=_listFeeScheds.OrderBy(x => x.ItemOrder).ToList();
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableFeeScheds","Description"),145);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableFeeScheds","Type"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableFeeScheds","Hidden"),60,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			IsDone=true;
			if(_listFeeScheds.Where(x => x.Description.ToLower()!="default").ToList().Count==0) {
				IsDone=false;
			}
			foreach(FeeSched feeSched in _listFeeScheds) {
				row=new ODGridRow();
				row.Tag=feeSched;
				row.Cells.Add(feeSched.Description);
				row.Cells.Add(feeSched.FeeSchedType.ToString());
				row.Cells.Add(feeSched.IsHidden ? "X" : "");
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			FormFeeSchedEdit FormF=new FormFeeSchedEdit();
			FormF.FeeSchedCur=new FeeSched();
			FormF.FeeSchedCur.IsNew=true;
			FormF.FeeSchedCur.ItemOrder=_listFeeScheds.Count;
			FormF.ListFeeScheds=_listFeeScheds;
			FormF.ShowDialog();
			if(FormF.DialogResult==DialogResult.OK) {
				FillGrid();
				_isChanged=true;
				for(int i=0;i<_listFeeScheds.Count;i++){
					if(FormF.FeeSchedCur.FeeSchedNum==_listFeeScheds[i].FeeSchedNum){
						gridMain.SetSelected(i,true);
					}
				}
			}
			
		}

		private void butImport_Click(object sender,EventArgs e) {
			if(gridMain.SelectedGridRows.Count==0) {
				MsgBox.Show(this,"Please select a fee schedule to import fees into.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"If you want a clean slate, the current fee schedule should be cleared first.  "
				+"When imported, any fees found in the text file will overwrite values of the selected fee schedule.  Are you sure you want to continue?")) 
			{
				return;
			}
			// Set up general base fee schedules with no provider or clinic
			long provNum=0;
			long clinicNum=0;
			Cursor=Cursors.WaitCursor;
			OpenFileDialog Dlg=new OpenFileDialog();
			if(Directory.Exists(PrefC.GetString(PrefName.ExportPath))) {
				Dlg.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
			}
			else if(Directory.Exists("C:\\")) {
				Dlg.InitialDirectory="C:\\";
			}
			if(Dlg.ShowDialog()!=DialogResult.OK) {
				Cursor=Cursors.Default;
				return;
			}
			if(!File.Exists(Dlg.FileName)){
				MsgBox.Show(this,"File not found");
				Cursor=Cursors.Default;
				return;
			}
			FeeSched feeSched=((FeeSched)gridMain.SelectedGridRows[0].Tag);//get selected fee sched from grid.
			FeeL.ImportFees(feeSched.FeeSchedNum,clinicNum,provNum,Dlg.FileName,ParentForm);
			_isChanged=true;
			Cursor=Cursors.Default;
		}

		private void butEditFee_Click(object sender,EventArgs e) {
			//only allow once user has at least one valid fee schedule. 
			if(_listFeeScheds.Count(x => x.Description.ToLower()!="default")==0) {
				MsgBox.Show(this,"At least one fee schedule is required before moving on to entering and editing fees.");
			}
			FeeScheds.Sync(_listFeeScheds,_listFeeSchedsOld);
			DataValid.SetInvalid(InvalidType.FeeScheds);
			_listFeeSchedsOld=_listFeeScheds.Select(x => x.Copy()).ToList();
			FormProcCodes FormPC = new FormProcCodes();
			FormPC.ShowDialog();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormFeeSchedEdit FormF=new FormFeeSchedEdit();
			FormF.FeeSchedCur=_listFeeScheds[e.Row];
			FormF.ListFeeScheds=_listFeeScheds;
			FormF.ShowDialog();
			if(FormF.DialogResult==DialogResult.OK) {
				FillGrid();
				_isChanged=true;
				for(int i=0;i<_listFeeScheds.Count;i++){
					if(FormF.FeeSchedCur.FeeSchedNum==_listFeeScheds[i].FeeSchedNum){
						gridMain.SetSelected(i,true);
					}
				}
			}
			
		}

		private void labelAdd_Click(object sender,EventArgs e) {
			MsgBox.Show("FormSetupWizard","Add a new blank fee schedule for importing or manually entering fees.");
		}

		private void labelEdit_Click(object sender,EventArgs e) {
			MsgBox.Show("FormSetupWizard","Manually edit and input fees that are necessary to your practice.");
		}

		private void labelImport_Click(object sender,EventArgs e) {
			MsgBox.Show("FormSetupWizard","Format for fee import requires a column for the procedure code, followed by a column with the amount "
				+"for that code. These columns need to be separated with a tab delimiter.");
		}

		private void ControlDone(object sender, EventArgs e) {
			if(_isChanged) {
				FeeScheds.Sync(_listFeeScheds,_listFeeSchedsOld);
				DataValid.SetInvalid(InvalidType.FeeScheds);
			}
		}
	}
}
