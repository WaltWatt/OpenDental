using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using CodeBase;

namespace OpenDental {
	public partial class FormOrthoSetup:ODForm {
		private bool _hasChanges;
		///<summary>Set to the OrthoAutoProcCodeNum Pref value on load.  Can be changed by the user via this form.</summary>
		private long _orthoAutoProcCodeNum;
		///<summary>Filled upon load.</summary>
		private List<long> _listOrthoPlacementCodeNums= new List<long>();

		public FormOrthoSetup() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormOrthoSetup_Load(object sender,EventArgs e) {
			checkPatClone.Checked=PrefC.GetBool(PrefName.ShowFeaturePatientClone);
			checkApptModuleShowOrthoChartItem.Checked=PrefC.GetBool(PrefName.ApptModuleShowOrthoChartItem);
			checkOrthoEnabled.Checked=PrefC.GetBool(PrefName.OrthoEnabled);
			checkOrthoFinancialInfoInChart.Checked=PrefC.GetBool(PrefName.OrthoCaseInfoInOrthoChart);
			checkOrthoClaimMarkAsOrtho.Checked=PrefC.GetBool(PrefName.OrthoClaimMarkAsOrtho);
			checkOrthoClaimUseDatePlacement.Checked=PrefC.GetBool(PrefName.OrthoClaimUseDatePlacement);
			textOrthoMonthsTreat.Text=PrefC.GetByte(PrefName.OrthoDefaultMonthsTreat).ToString();
			_orthoAutoProcCodeNum=PrefC.GetLong(PrefName.OrthoAutoProcCodeNum);
			textOrthoAutoProc.Text=ProcedureCodes.GetStringProcCode(_orthoAutoProcCodeNum);
			checkConsolidateInsPayment.Checked=PrefC.GetBool(PrefName.OrthoInsPayConsolidated);
			string strListOrthoNums = PrefC.GetString(PrefName.OrthoPlacementProcsList);
			if(strListOrthoNums!="") {
				_listOrthoPlacementCodeNums.AddRange(strListOrthoNums.Split(new char[] { ',' }).ToList().Select(x => PIn.Long(x)));
			}
			RefreshListBoxProcs();
		}

		private void RefreshListBoxProcs() {
			listboxOrthoPlacementProcs.Items.Clear();
			foreach(long orthoProcCodeNum in _listOrthoPlacementCodeNums) {
				ProcedureCode procCodeCur = ProcedureCodes.GetProcCode(orthoProcCodeNum);
				ODBoxItem<ProcedureCode> listBoxItem = new ODBoxItem<ProcedureCode>(procCodeCur.ProcCode,procCodeCur);
				listboxOrthoPlacementProcs.Items.Add(listBoxItem);
			}
		}

		private void butOrthoDisplayFields_Click(object sender,EventArgs e) {
			FormDisplayFieldsOrthoChart FormDFOC = new FormDisplayFieldsOrthoChart();
			FormDFOC.ShowDialog();
		}

		private void butPickOrthoProc_Click(object sender,EventArgs e) {
			FormProcCodes FormPC = new FormProcCodes();
			FormPC.IsSelectionMode=true;
			FormPC.ShowDialog();
			if(FormPC.DialogResult == DialogResult.OK) {
				_orthoAutoProcCodeNum=FormPC.SelectedCodeNum;
				textOrthoAutoProc.Text=ProcedureCodes.GetStringProcCode(_orthoAutoProcCodeNum);
			}
		}

		private void butPlacementProcsEdit_Click(object sender,EventArgs e) {
			FormProcCodes FormPC = new FormProcCodes();
			FormPC.IsSelectionMode = true;
			FormPC.ShowDialog();
			if(FormPC.DialogResult == DialogResult.OK) {
				_listOrthoPlacementCodeNums.Add(FormPC.SelectedCodeNum);
			}
			RefreshListBoxProcs();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(listboxOrthoPlacementProcs.SelectedIndices.Count == 0) {
				MsgBox.Show(this,"Select an item to delete.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Are you sure you want to delete the selected items?")) {
				return;
			}
			foreach(ODBoxItem<ProcedureCode> boxItem in listboxOrthoPlacementProcs.SelectedItems) {
				_listOrthoPlacementCodeNums.Remove(boxItem.Tag.CodeNum);
			}
			RefreshListBoxProcs();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textOrthoMonthsTreat.errorProvider1.GetError(textOrthoMonthsTreat)!="") {
				MsgBox.Show(this,"Default months treatment must be between 0 and 255 months.");
				return;
			}
			if(PrefC.GetBool(PrefName.ShowFeaturePatientClone)!=checkPatClone.Checked) {
				MsgBox.Show(this,"You will need to restart OpenDental for this change to take effect.");
			}
			if(Prefs.UpdateBool(PrefName.ShowFeaturePatientClone,checkPatClone.Checked)
			| Prefs.UpdateBool(PrefName.ApptModuleShowOrthoChartItem,checkApptModuleShowOrthoChartItem.Checked)
			| Prefs.UpdateBool(PrefName.OrthoEnabled,checkOrthoEnabled.Checked)
			| Prefs.UpdateBool(PrefName.OrthoCaseInfoInOrthoChart,checkOrthoFinancialInfoInChart.Checked)
			| Prefs.UpdateBool(PrefName.OrthoClaimMarkAsOrtho,checkOrthoClaimMarkAsOrtho.Checked)
			| Prefs.UpdateBool(PrefName.OrthoClaimUseDatePlacement,checkOrthoClaimUseDatePlacement.Checked)
			| Prefs.UpdateByte(PrefName.OrthoDefaultMonthsTreat,PIn.Byte(textOrthoMonthsTreat.Text))
			| Prefs.UpdateBool(PrefName.OrthoInsPayConsolidated,checkConsolidateInsPayment.Checked)
			| Prefs.UpdateLong(PrefName.OrthoAutoProcCodeNum,_orthoAutoProcCodeNum)
			| Prefs.UpdateString(PrefName.OrthoPlacementProcsList,string.Join(",",_listOrthoPlacementCodeNums))
			) {
				_hasChanges=true;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			_hasChanges=false;
			DialogResult=DialogResult.Cancel;
		}

		private void FormOrthoSetup_FormClosing(object sender,FormClosingEventArgs e) {
			if(_hasChanges) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}
	}
}