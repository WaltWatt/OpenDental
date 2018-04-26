using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormGradingScales:ODForm {
		private List<GradingScale> _listGradingScales;
		public GradingScale SelectedGradingScale;
		/// <summary>This is set before showing the window to determine the usage of the window.  Default is false.</summary>
		public bool IsSelectionMode=false;

		/// <summary>This window has two modes: Selection and Setup Mode.  By default the buttons and methods are in Setup mode.  Changing to Selection Mode will change the function of several actions in this window.  Most specifically, the grid's double click method.</summary>
		public FormGradingScales() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormGradingScales_Load(object sender,EventArgs e) {
			if(IsSelectionMode) {
				butOK.Visible=true;
				butCancel.Text="&Cancel";
			}
			FillGrid();
		}

		private void FillGrid() {
			_listGradingScales=GradingScales.RefreshList();
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("FormGradingScales","Description"),160);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listGradingScales.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listGradingScales[i].Description);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_DoubleClick(object sender,EventArgs e) {
			if(IsSelectionMode) {
				SelectedGradingScale=_listGradingScales[gridMain.GetSelectedIndex()];
				DialogResult=DialogResult.OK;
				return;
			}
			FormGradingScaleEdit FormGSE=new FormGradingScaleEdit(_listGradingScales[gridMain.GetSelectedIndex()]);
			FormGSE.ShowDialog();
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			GradingScale gradingScaleNew=new GradingScale();
			gradingScaleNew.GradingScaleNum=GradingScales.Insert(gradingScaleNew);
			gradingScaleNew.IsNew=true;
			FormGradingScaleEdit FormGSE=new FormGradingScaleEdit(gradingScaleNew);
			FormGSE.ShowDialog();
			FillGrid();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Select a grading scale first.");
				return;
			}
			SelectedGradingScale=_listGradingScales[gridMain.GetSelectedIndex()];
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}



	}
}