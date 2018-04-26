using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDental.ReportingComplex;
using OpenDentBusiness;
using System.Linq;
using CodeBase;

namespace OpenDental {
	public partial class FormRpTreatPlanPresentationStatistics:ODForm {
		private List<Userod> _listUsers;
		private List<Clinic> _listClinics;
		public FormRpTreatPlanPresentationStatistics() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormRpTreatPlanPresenter_Load(object sender,EventArgs e) {
			date1.SelectionStart=new DateTime(DateTime.Today.Year,DateTime.Today.Month,1).AddMonths(-1);
			date2.SelectionStart=new DateTime(DateTime.Today.Year,DateTime.Today.Month,1).AddDays(-1);
			_listUsers=Userods.GetDeepCopy(true);
			listUser.Items.AddRange(_listUsers.Select(x => x.UserName).ToArray());
			checkAllUsers.Checked=true;
			if(PrefC.HasClinicsEnabled) {
				if(!Security.CurUser.ClinicIsRestricted) {
					listClin.Items.Add(Lan.g(this,"Unassigned"));
				}
				_listClinics=Clinics.GetForUserod(Security.CurUser);
				listClin.Items.AddRange(_listClinics.Select(x => x.Abbr).ToArray());
				checkAllClinics.Checked=true;
			}
			else {
				listClin.Visible=false;
				checkAllClinics.Visible=false;
				labelClin.Visible=false;
				groupGrossNet.Location=new Point(185,225);
				groupOrder.Location=new Point(185,295);
				groupUser.Location=new Point(185,365);
				listUser.Width+=30;
			}
		}

		private void RunReport(List<long> listUserNums,List<long> listClinicsNums) {
			ReportComplex report = new ReportComplex(true,false);
			report.AddTitle("Title",Lan.g(this,"Presented Procedure Totals"));
			report.AddSubTitle("PracTitle",PrefC.GetString(PrefName.PracticeTitle));
			report.AddSubTitle("Date",date1.SelectionStart.ToShortDateString()+" - "+date2.SelectionStart.ToShortDateString());
			List<Userod> listSelectedUsers = new List<Userod>();
			if(checkAllUsers.Checked) {
				report.AddSubTitle("Users",Lan.g(this,"All Users"));
				listSelectedUsers.AddRange(_listUsers); //add all users
			}
			else {
				for(int i = 0;i < listUser.SelectedIndices.Count;i++) {
					listSelectedUsers.Add(_listUsers[listUser.SelectedIndices[i]]); //add selected users
				}
				report.AddSubTitle("Users",string.Join(",",listSelectedUsers.Select(x => x.UserName)));
			}
			List<Clinic> listSelectedClinics = new List<Clinic>();
			if(PrefC.HasClinicsEnabled) {
				if(checkAllClinics.Checked) {
					report.AddSubTitle("Clinics",Lan.g(this,"All Clinics"));
					listSelectedClinics.Add(new Clinic() {
						ClinicNum = 0,
						Description = "Unassigned"
					});
					listSelectedClinics.AddRange(_listClinics); //add all clinics and the unassigned clinic.
				}
				else {
					for(int i = 0;i<listClin.SelectedIndices.Count;i++) {
						if(Security.CurUser.ClinicIsRestricted) {
							listSelectedClinics.Add(_listClinics[listClin.SelectedIndices[i]]);
						}
						else {
							if(listClin.SelectedIndices[i]==0) {
								listSelectedClinics.Add(new Clinic() {
									ClinicNum = 0,
									Description = "Unassigned"
								});
							}
							else {
								listSelectedClinics.Add(_listClinics[listClin.SelectedIndices[i]-1]);//Minus 1 from the selected index
							}
						}
					}
					report.AddSubTitle("Clinics",string.Join(",",listSelectedClinics.Select(x => x.Description)));
				}
			}
			List<long> clinicNums = listSelectedClinics.Select(y => y.ClinicNum).ToList();
			List<long> userNums = listSelectedUsers.Select(y => y.UserNum).ToList();
			DataTable table=RpTreatPlanPresentationStatistics.GetTreatPlanPresentationStatistics(date1.SelectionStart,date2.SelectionStart,radioFirstPresented.Checked
				,checkAllClinics.Checked,PrefC.HasClinicsEnabled,radioPresenter.Checked,radioGross.Checked,checkAllUsers.Checked,userNums,clinicNums);			
			QueryObject query=report.AddQuery(table,"","",SplitByKind.None,1,true);
			query.AddColumn(Lan.g(this,"Presenter"),100,FieldValueType.String);
			query.AddColumn(Lan.g(this,"# of Plans"),85,FieldValueType.Integer);
			query.AddColumn(Lan.g(this,"# of Procs"),85,FieldValueType.Integer);
			query.AddColumn(Lan.g(this,"# of ProcsSched"),100,FieldValueType.Integer);
			query.AddColumn(Lan.g(this,"# of ProcsComp"),100,FieldValueType.Integer);
			if(radioGross.Checked) {
				query.AddColumn(Lan.g(this,"GrossTPAmt"),95,FieldValueType.Number);
				query.AddColumn(Lan.g(this,"GrossSchedAmt"),95,FieldValueType.Number);
				query.AddColumn(Lan.g(this,"GrossCompAmt"),95,FieldValueType.Number);
			}
			else {
				query.AddColumn(Lan.g(this,"NetTPAmt"),95,FieldValueType.Number);
				query.AddColumn(Lan.g(this,"NetSchedAmt"),95,FieldValueType.Number);
				query.AddColumn(Lan.g(this,"NetCompAmt"),95,FieldValueType.Number);
			}
			if(!report.SubmitQueries()) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			FormReportComplex FormR=new FormReportComplex(report);
			FormR.ShowDialog();
			//DialogResult=DialogResult.OK;
		}	

		private void checkAllUsers_Click(object sender,EventArgs e) {
			if(checkAllUsers.Checked) {
				listUser.SelectedIndices.Clear();
			}
		}

		private void listUser_Click(object sender,EventArgs e) {
			if(listUser.SelectedIndices.Count>0) {
				checkAllUsers.Checked=false;
			}
		}

		private void checkAllClinics_Click(object sender,EventArgs e) {
			if(checkAllClinics.Checked) {
				listClin.SelectedIndices.Clear();
			}
		}

		private void listClin_Click(object sender,EventArgs e) {
			if(listClin.SelectedIndices.Count>0) {
				checkAllClinics.Checked=false;
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(date2.SelectionStart<date1.SelectionStart) {
				MsgBox.Show(this,"End date cannot be before start date.");
				return;
			}
			if(!checkAllUsers.Checked && listUser.SelectedIndices.Count==0) {
				MsgBox.Show(this,"Please select at least one user.");
				return;
			}
			if(PrefC.HasClinicsEnabled && !checkAllClinics.Checked && listClin.SelectedIndices.Count==0) {
				MsgBox.Show(this,"Please select at least one clinic.");
				return;
			}
			List<long> listUserNums=new List<long>();
			List<long> listClinicNums=new List<long>();
			if(checkAllUsers.Checked) {
				listUserNums=_listUsers.Select(x => x.UserNum).ToList();
			}
			else {
				listUserNums=listUser.SelectedIndices.OfType<int>().ToList().Select(x => _listUsers[x].UserNum).ToList();
			}
			if(PrefC.HasClinicsEnabled) {
				if(checkAllClinics.Checked) {
					listClinicNums=_listClinics.Select(x => x.ClinicNum).ToList();
				}
				else {
					for(int i = 0;i<listClin.SelectedIndices.Count;i++) {
						if(Security.CurUser.ClinicIsRestricted) {
							listClinicNums.Add(_listClinics[listClin.SelectedIndices[i]].ClinicNum);
						}
						else if(listClin.SelectedIndices[i]!=0) {
							listClinicNums.Add(_listClinics[listClin.SelectedIndices[i]-1].ClinicNum);
						}
					}
				}
				if(!Security.CurUser.ClinicIsRestricted && (listClin.GetSelected(0) || checkAllClinics.Checked)) {
					listClinicNums.Add(0);
				}
			}
			RunReport(listUserNums,listClinicNums);
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


	}
}