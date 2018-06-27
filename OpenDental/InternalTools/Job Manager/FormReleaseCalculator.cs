using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using CodeBase;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormReleaseCalculator:ODForm {
		private List<long> _listEngEmpNums = new List<long>() {15,34,36,64,72,74,88,94,118,121,163,173,177,179,253,257,299};
		private List<long> _listDefaultEngEmpNums = new List<long>() {15,34,36,64,72,74,88,94,118,121,163,173,179,253,257};
		private List<Tuple<long,double>> _listTopJobs=new List<Tuple<long,double>>();
		private double _avgJobHours=9.43;
		private double _jobTimePercent=0.173;
		private double _avgBreakHours=0.85;
		private List<Job> _listJobsAll;


		public FormReleaseCalculator(List<Job> listJobsAll) {
			_listJobsAll=listJobsAll;
			InitializeComponent();
			Lan.F(this);
		}

		private void FormReleaseCalculator_Load(object sender,EventArgs e) {
			textAvgJobHours.Text=_avgJobHours.ToString();
			textEngJobPercent.Text=_jobTimePercent.ToString();
			textBreakHours.Text=_avgBreakHours.ToString();
			foreach(Def def in Defs.GetDefsForCategory(DefCat.JobPriorities,true).OrderBy(x => x.ItemOrder).ToList()) {
				listPriorities.Items.Add(new ODBoxItem<Def>(def.ItemName,def));
				if(def.DefNum.In(597,601)) {
					listPriorities.SelectedIndices.Add(listPriorities.Items.Count-1);
				}
			}
			foreach(JobPhase phase in Enum.GetValues(typeof(JobPhase))) {
				listPhases.Items.Add(new ODBoxItem<JobPhase>(phase.ToString(),phase));
				if(phase.In(JobPhase.Concept,JobPhase.Definition,JobPhase.Development,JobPhase.Quote)) {
					listPhases.SelectedIndices.Add(listPhases.Items.Count-1);
				}
			}
			foreach(JobCategory category in Enum.GetValues(typeof(JobCategory))) {
				listCategories.Items.Add(new ODBoxItem<JobCategory>(category.ToString(),category));
				if(category.In(JobCategory.Enhancement,JobCategory.Feature,JobCategory.HqRequest,JobCategory.InternalRequest,JobCategory.ProgramBridge)) {
					listCategories.SelectedIndices.Add(listCategories.Items.Count-1);
				}
			}
			foreach(long engNum in _listEngEmpNums) {
				Employee emp=Employees.GetEmp(engNum);
				listEngineers.Items.Add(new ODBoxItem<Employee>(emp.FName,emp));
				if(_listDefaultEngEmpNums.Contains(engNum)) {
					listEngineers.SelectedIndices.Add(listEngineers.Items.Count-1);
				}
			}
		}

		private void butCalculate_Click(object sender,EventArgs e) {
			_listTopJobs.Clear();
			listEngNoJobs.Items.Clear();
			List<long> listEngNums=listEngineers.SelectedTags<Employee>().Select(x => x.EmployeeNum).ToList();
			List<long> listUserNums=listEngNums.Select(x => Userods.GetUserByEmployeeNum(x).UserNum).ToList();
			//Get 6 months of scheduled engineering time. Arbitrary because there should be no way we have a 6 month release cycle.
			List<Schedule> listSchedules=Schedules.RefreshPeriodForEmps(DateTime.Today,DateTime.Today.AddMonths(6),listEngNums);
			//Get all the jobs according to the selected criteria.
			//No need to fill currently, but I may want to add reviews into this to improve accuracy for unfinished jobs
			List<Job> listJobs=_listJobsAll.Where(x => x.Priority.In(listPriorities.SelectedTags<Def>().Select(y => y.DefNum)) 
				&& x.PhaseCur.In(listPhases.SelectedTags<JobPhase>())
				&& x.Category.In(listCategories.SelectedTags<JobCategory>())).ToList();
			double totalJobHours=0;
			DateTime releaseDate=DateTime.Today;
			double avgJobHours=_avgJobHours;
			double jobTimePercent=_jobTimePercent;
			double avgBreakHours=_avgBreakHours;
			Double.TryParse(textAvgJobHours.Text,out avgJobHours);
			Double.TryParse(textEngJobPercent.Text,out jobTimePercent);
			Double.TryParse(textBreakHours.Text,out avgBreakHours);
			gridCalculatedJobs.BeginUpdate();
			gridCalculatedJobs.Columns.Clear();
			gridCalculatedJobs.Columns.Add(new ODGridColumn("EstHrs",0) { TextAlign=HorizontalAlignment.Center });
			gridCalculatedJobs.Columns.Add(new ODGridColumn("ActHrs",0) { TextAlign=HorizontalAlignment.Center });
			gridCalculatedJobs.Columns.Add(new ODGridColumn("",200));
			gridCalculatedJobs.Rows.Clear();
			foreach(Job job in listJobs) {
				if(job.UserNumEngineer==0 && listUserNums.Contains(job.UserNumExpert)) {
					listUserNums.Remove(job.UserNumExpert);
				}
				if(job.UserNumEngineer!=0 && listUserNums.Contains(job.UserNumEngineer)) {
					listUserNums.Remove(job.UserNumEngineer);
				}
				//If hrsEst is 0 then use the avgJobHours as a base.
				double hrsEst=job.TimeEstimate.TotalHours==0?avgJobHours:job.TimeEstimate.TotalHours;
				//Remove the actual hours spent on the job currently
				//If negative then just use 0 (We aren't in a dimension where negative time estimates can be used for other jobs)
				double hrsCalculated=(hrsEst-job.HoursActual)<0?0:hrsEst-job.HoursActual;
				totalJobHours+=hrsCalculated;
				if(job.PhaseCur==JobPhase.Development) {
					_listTopJobs.Add(new Tuple<long,double>(job.JobNum,hrsCalculated));
				}
				gridCalculatedJobs.Rows.Add(
					new ODGridRow(
						new ODGridCell(job.TimeEstimate.TotalHours==0?"0("+_avgJobHours+")":job.TimeEstimate.TotalHours.ToString()),
						new ODGridCell(job.HoursActual.ToString()),
						new ODGridCell(job.Title)
						) {
						Tag=job
					}
					);
			}
			gridCalculatedJobs.EndUpdate();
			foreach(long engNum in listUserNums) {
				Userod eng=Userods.GetUser(engNum);
				listEngNoJobs.Items.Add(new ODBoxItem<Userod>(eng.UserName,eng));
			}
			try {
				_listTopJobs=_listTopJobs.OrderByDescending(x => x.Item2).Take(3).ToList();
				butJob1.Text="#"+_listTopJobs[0].Item1.ToString()+"-"+Math.Round(_listTopJobs[0].Item2).ToString()+" hours";
				butJob2.Text="#"+_listTopJobs[1].Item1.ToString()+"-"+Math.Round(_listTopJobs[1].Item2).ToString()+" hours";
				butJob3.Text="#"+_listTopJobs[2].Item1.ToString()+"-"+Math.Round(_listTopJobs[2].Item2).ToString()+" hours";
			}
			catch {
				panelExtra.Visible=false;
			}
			labelJobHours.Text=Math.Round(totalJobHours).ToString();
			labelJobNumber.Text=listJobs.Count.ToString();
			double schedHoursTotal=0;
			double schedHoursBreaksTotal=0;
			double schedHoursPercentTotal=0;
			foreach(Schedule sched in listSchedules) {
				//Calculate actual scheduled time
				double schedHours=(sched.StopTime-sched.StartTime).TotalHours;
				schedHoursTotal+=schedHours;
				//Remove average break time
				schedHours-=avgBreakHours;
				schedHoursBreaksTotal+=schedHours;
				//Multiply the scheduled time by the percentage of coding time for the jobs we care about
				schedHours=schedHours*jobTimePercent;
				schedHoursPercentTotal+=schedHours;
				//Remove the scheduled hours from the total job hours
				totalJobHours-=schedHours;
				if(totalJobHours<0) {
					releaseDate=sched.SchedDate;//Add a week as a buffer
					break;
				}
			}
			labelEngHours.Text=Math.Round(schedHoursTotal).ToString();
			labelAfterBreak.Text=Math.Round(schedHoursBreaksTotal).ToString();
			labelRatioHours.Text=Math.Round(schedHoursPercentTotal).ToString();
			labelReleaseDate.Text=releaseDate.ToShortDateString()+" - "+releaseDate.AddDays(7).ToShortDateString();
			labelReleaseDate.Visible=true;
			panelExtra.Visible=true;
		}

		private void butJob1_Click(object sender,EventArgs e) {
			FormOpenDental.S_GoToJob(_listTopJobs[0].Item1);
		}

		private void butJob2_Click(object sender,EventArgs e) {
			FormOpenDental.S_GoToJob(_listTopJobs[1].Item1);
		}

		private void butJob3_Click(object sender,EventArgs e) {
			FormOpenDental.S_GoToJob(_listTopJobs[2].Item1);
		}

		private void gridCalculatedJobs_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormOpenDental.S_GoToJob(((Job)gridCalculatedJobs.Rows[e.Row].Tag).JobNum);
		}
	}
}