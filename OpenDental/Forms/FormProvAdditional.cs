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
	public partial class FormProvAdditional:ODForm {
		/// <summary>This is a list of providerclinic rows that were given to this form, containing any modifications that were made while in FormProvAdditional.</summary>
		public List<ProviderClinic> ListProviderClinicOut=new List<ProviderClinic>();
		private Provider _provCur;
		private List<ProviderClinic> _listProvClinic;

		public FormProvAdditional(List<ProviderClinic> listProvClinic,Provider provCur) {
			InitializeComponent();
			Lan.F(this);
			_listProvClinic=listProvClinic.Select(x => x.Copy()).ToList();
			_provCur=provCur;
		}

		private void FormProvAdditional_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid() {
			Cursor=Cursors.WaitCursor;
			gridProvProperties.BeginUpdate();
			gridProvProperties.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableProviderProperties","Clinic"),120);
			gridProvProperties.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableProviderProperties","DEA Num"),120,true);
			gridProvProperties.Columns.Add(col);
			gridProvProperties.Rows.Clear();
			ODGridRow row;
			ProviderClinic provClinicDefault=_listProvClinic.Find(x => x.ClinicNum==0);
			//Didn't have an HQ row
			if(provClinicDefault==null) {//Doesn't exist in list
				provClinicDefault=ProviderClinics.GetOne(_provCur.ProvNum,0);
				if(provClinicDefault==null) {//Doesn't exist in database
					provClinicDefault=new ProviderClinic {
						ProvNum=_provCur.ProvNum,
						ClinicNum=0,
						DEANum="",
					};
				}
				_listProvClinic.Add(provClinicDefault);//If not in list, add to list.
			}
			row=new ODGridRow();
			row.Cells.Add("Default");
			row.Cells.Add(provClinicDefault.DEANum);
			row.Tag=provClinicDefault;
			gridProvProperties.Rows.Add(row);
			if(PrefC.HasClinicsEnabled) {
				foreach(Clinic clinicCur in Clinics.GetForUserod(Security.CurUser)) {
					row=new ODGridRow();
					ProviderClinic provClinic=_listProvClinic.Find(x => x.ClinicNum == clinicCur.ClinicNum);
					//Doesn't exist in Db, create a new one
					if(provClinic==null) {
						provClinic=ProviderClinics.GetOne(_provCur.ProvNum,clinicCur.ClinicNum);
						if(provClinic==null) {
							provClinic=new ProviderClinic {
								ProvNum=_provCur.ProvNum,
								ClinicNum=clinicCur.ClinicNum,
								DEANum="",
							};
						}
						_listProvClinic.Add(provClinic);
					}
					row.Cells.Add(clinicCur.Abbr);
					row.Cells.Add(provClinic.DEANum);
					row.Tag=provClinic;
					gridProvProperties.Rows.Add(row);
				}
			}
			gridProvProperties.EndUpdate();
			Cursor=Cursors.Default;
		}

		private void gridProvProperties_CellLeave(object sender,ODGridClickEventArgs e) {
			string newDEA=PIn.String(gridProvProperties.Rows[e.Row].Cells[e.Col].Text);
			ProviderClinic provClin=(ProviderClinic)gridProvProperties.Rows[e.Row].Tag;
			provClin.DEANum=newDEA;
		}

		private void butOK_Click(object sender,EventArgs e) {
			ListProviderClinicOut=new List<ProviderClinic>();
			foreach(ODGridRow row in gridProvProperties.Rows) {
				ListProviderClinicOut.Add((ProviderClinic)row.Tag);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}