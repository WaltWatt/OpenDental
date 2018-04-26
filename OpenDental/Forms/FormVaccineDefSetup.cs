using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormVaccineDefSetup:ODForm {
		private List<VaccineDef> _listVaccineDefs;

		public FormVaccineDefSetup() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormVaccineDefSetup_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid() {
			VaccineDefs.RefreshCache();
			listMain.Items.Clear();
			_listVaccineDefs=VaccineDefs.GetDeepCopy();
			for(int i=0;i<_listVaccineDefs.Count;i++) {
				listMain.Items.Add(_listVaccineDefs[i].CVXCode + " - " + _listVaccineDefs[i].VaccineName);
			}
		}

		private void listMain_DoubleClick(object sender,EventArgs e) {
			if(listMain.SelectedIndex==-1) {
				return;
			}
			FormVaccineDefEdit FormV=new FormVaccineDefEdit();
			FormV.VaccineDefCur=_listVaccineDefs[listMain.SelectedIndex];
			FormV.ShowDialog();
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormVaccineDefEdit FormV=new FormVaccineDefEdit();
			FormV.VaccineDefCur=new VaccineDef();
			FormV.IsNew=true;
			FormV.ShowDialog();
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}