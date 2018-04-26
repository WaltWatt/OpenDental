using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {

	public partial class FormAutoNoteControls:ODForm {
		///<summary>If OK, then this is the control that the user selected.</summary>
		public long SelectedControlNum;
		private List<AutoNoteControl> _listAutoNoteControls;

		public FormAutoNoteControls() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		private void FormAutoNoteControls_Load(object sender, EventArgs e) {
			FillGrid();
		}

		private void FillGrid(){
			AutoNoteControls.RefreshCache();
			_listAutoNoteControls=AutoNoteControls.GetDeepCopy();
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("FormAutoNoteControls","Description"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormAutoNoteControls","Type"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormAutoNoteControls","Prompt Text"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("FormAutoNoteControls","Options"),100);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listAutoNoteControls.Count;i++){
				row=new ODGridRow();
				row.Cells.Add(_listAutoNoteControls[i].Descript);
				row.Cells.Add(_listAutoNoteControls[i].ControlType);
				row.Cells.Add(_listAutoNoteControls[i].ControlLabel);
				row.Cells.Add(_listAutoNoteControls[i].ControlOptions);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,OpenDental.UI.ODGridClickEventArgs e) {
			//do nothing
		}

		private void butEdit_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select an item first.");
				return;
			}
			FormAutoNoteControlEdit FormA=new FormAutoNoteControlEdit();
			FormA.ControlCur=_listAutoNoteControls[gridMain.GetSelectedIndex()];
			FormA.ShowDialog();
			if(FormA.DialogResult!=DialogResult.OK) {
				return;
			}
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormAutoNoteControlEdit FormA=new FormAutoNoteControlEdit();
			FormA.IsNew=true;
			FormA.ControlCur=new AutoNoteControl();
			FormA.ShowDialog();
			if(FormA.DialogResult!=DialogResult.OK) {
				return;
			}
			FillGrid();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select an item first.");
				return;
			}
			SelectedControlNum=_listAutoNoteControls[gridMain.GetSelectedIndex()].AutoNoteControlNum;
			DialogResult=DialogResult.OK;
		}


		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		

		

        
	}
}