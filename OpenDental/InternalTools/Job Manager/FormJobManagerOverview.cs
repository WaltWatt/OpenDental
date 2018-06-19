using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;
using CodeBase;

namespace OpenDental {
	public partial class FormJobManagerOverview:ODForm {
		private List<Job> _listJobsAll;

		public FormJobManagerOverview(List<Job> listJobsAll) {
			_listJobsAll=listJobsAll;
			InitializeComponent();
			Lan.F(this);
		}

		private void FormJobManagerOverview_Load(object sender,EventArgs e) {
			tabControlMain.TabPages.Remove(tabStatistics);
			FillGridHigh();
			FillGridOld();
			FillGridOverEst();
			FillGridQuoteNotStarted();
			FillGridNoHours();
		}

		public override void OnProcessSignals(List<Signalod> listSignals) {
			if(!listSignals.Exists(x => x.IType==InvalidType.Jobs || x.IType==InvalidType.Security || x.IType==InvalidType.Defs)) {
				return;//no job signals;
			}
			FillGridHigh();
			FillGridOld();
			FillGridOverEst();
			FillGridQuoteNotStarted();
			FillGridNoHours();
		}

		private void FillGridHigh() {
			List<Job> listHighPriorityJobs=_listJobsAll.Where(x => x.Priority.In(591,601)//High Priority or Urgent
			&& !x.PhaseCur.In(JobPhase.Cancelled,JobPhase.Complete,JobPhase.Documentation) 
			&& !x.Category.In(JobCategory.Bug,JobCategory.Query,JobCategory.Conversion,JobCategory.Research)).ToList();
			gridHighPriorityJobs.BeginUpdate();
			gridHighPriorityJobs.Columns.Clear();
			gridHighPriorityJobs.Columns.Add(new ODGridColumn("Job",0));
			gridHighPriorityJobs.Columns.Add(new ODGridColumn("Owner",70) { TextAlign=HorizontalAlignment.Center });
			gridHighPriorityJobs.Columns.Add(new ODGridColumn("Owner Action",90));
			gridHighPriorityJobs.Columns.Add(new ODGridColumn("Engineer",75) { TextAlign=HorizontalAlignment.Center });
			gridHighPriorityJobs.Columns.Add(new ODGridColumn("Hrs Est",75) { TextAlign=HorizontalAlignment.Center });
			gridHighPriorityJobs.Columns.Add(new ODGridColumn("Hrs So Far",75) { TextAlign=HorizontalAlignment.Center });
			gridHighPriorityJobs.Columns.Add(new ODGridColumn("Hrs Last 7 Days",90) { TextAlign=HorizontalAlignment.Center });
			gridHighPriorityJobs.Columns.Add(new ODGridColumn("Last Updated",90) { TextAlign=HorizontalAlignment.Center });
			gridHighPriorityJobs.Columns.Add(new ODGridColumn("Expert",75) { TextAlign=HorizontalAlignment.Center });
			gridHighPriorityJobs.Rows.Clear();
			foreach(Job job in listHighPriorityJobs) {
				ODGridRow row=new ODGridRow() { Tag=job };
				row.Cells.Add(job.ToString());
				row.Cells.Add(Userods.GetName(job.OwnerNum));
				row.Cells.Add(job.OwnerAction.GetDescription());
				row.Cells.Add(Userods.GetName(job.UserNumEngineer));
				row.Cells.Add(job.TimeEstimate.TotalHours.ToString());
				row.Cells.Add(job.HoursActual.ToString());
				row.Cells.Add(job.ListJobTimeLogs.Where(x => x.DateTStamp>=DateTime.Today.AddDays(-7)).Sum(y => y.TimeReview.TotalHours).ToString());
				DateTime lastUpdated=job.ListJobLogs.Where(x => x.Description!="Job Viewed").Select(y => y.DateTimeEntry).OrderByDescending(x => x.Ticks).FirstOrDefault();
				row.Cells.Add(lastUpdated==DateTime.MinValue?"N/A":lastUpdated.ToShortDateString());
				row.Cells.Add(Userods.GetName(job.UserNumExpert));
				gridHighPriorityJobs.Rows.Add(row);
			}
			gridHighPriorityJobs.EndUpdate();
		}

		private void FillGridOld() {
			List<Job> listJobsOld=_listJobsAll.Where(x => x.DateTimeEntry<=DateTime.Now.AddYears(-1)
			&& !x.PhaseCur.In(JobPhase.Cancelled,JobPhase.Complete,JobPhase.Documentation) 
			&& !x.Category.In(JobCategory.Bug,JobCategory.Query,JobCategory.Conversion,JobCategory.Research)).ToList();
			gridOldJobs.BeginUpdate();
			gridOldJobs.Columns.Clear();
			gridOldJobs.Columns.Add(new ODGridColumn("Job",0));
			gridOldJobs.Columns.Add(new ODGridColumn("DateEntry",80) { TextAlign=HorizontalAlignment.Center });
			gridOldJobs.Columns.Add(new ODGridColumn("Last Updated",80) { TextAlign=HorizontalAlignment.Center });
			gridOldJobs.Columns.Add(new ODGridColumn("Owner",70) { TextAlign=HorizontalAlignment.Center });
			gridOldJobs.Rows.Clear();
			foreach(Job job in listJobsOld) {
				ODGridRow row=new ODGridRow() { Tag=job };
				row.Cells.Add(job.ToString());
				row.Cells.Add(job.DateTimeEntry.ToShortDateString());
				DateTime lastUpdated=job.ListJobLogs.Where(x => x.Description!="Job Viewed").Select(y => y.DateTimeEntry).OrderByDescending(x => x.Ticks).FirstOrDefault();
				row.Cells.Add(lastUpdated==DateTime.MinValue?"N/A":lastUpdated.ToShortDateString());
				row.Cells.Add(Userods.GetName(job.OwnerNum));
				gridOldJobs.Rows.Add(row);
			}
			gridOldJobs.EndUpdate();
		}

		private void FillGridOverEst() {
			List<Job> listJobsOverEst=_listJobsAll.Where(x => x.TimeEstimate.TotalHours<x.HoursActual
			&& !x.PhaseCur.In(JobPhase.Cancelled,JobPhase.Complete,JobPhase.Documentation) 
			&& !x.Category.In(JobCategory.Bug,JobCategory.Query,JobCategory.Conversion,JobCategory.Research)).ToList();
			gridOverEstimate.BeginUpdate();
			gridOverEstimate.Columns.Clear();
			gridOverEstimate.Columns.Add(new ODGridColumn("Job",0));
			gridOverEstimate.Columns.Add(new ODGridColumn("Owner",60) { TextAlign=HorizontalAlignment.Center });
			gridOverEstimate.Columns.Add(new ODGridColumn("Hrs Est",60) { TextAlign=HorizontalAlignment.Center });
			gridOverEstimate.Columns.Add(new ODGridColumn("Hrs So Far",60) { TextAlign=HorizontalAlignment.Center });
			gridOverEstimate.Rows.Clear();
			foreach(Job job in listJobsOverEst) {
				ODGridRow row=new ODGridRow() { Tag=job };
				row.Cells.Add(job.ToString());
				row.Cells.Add(Userods.GetName(job.OwnerNum));
				row.Cells.Add(job.TimeEstimate.TotalHours.ToString());
				row.Cells.Add(job.HoursActual.ToString());
				gridOverEstimate.Rows.Add(row);
			}
			gridOverEstimate.EndUpdate();
		}

		private void FillGridQuoteNotStarted() {
			List<Job> listJobsQuoteNotStarted=_listJobsAll.Where(x => x.ListJobQuotes.Exists(y => y.IsCustomerApproved==true)
			&& x.HoursActual<=0
			&& !x.PhaseCur.In(JobPhase.Cancelled,JobPhase.Complete,JobPhase.Documentation) 
			&& !x.Category.In(JobCategory.Bug,JobCategory.Query,JobCategory.Conversion,JobCategory.Research)).ToList();
			gridQuoteNotStarted.BeginUpdate();
			gridQuoteNotStarted.Columns.Clear();
			gridQuoteNotStarted.Columns.Add(new ODGridColumn("Job",0));
			gridQuoteNotStarted.Columns.Add(new ODGridColumn("Owner",70) { TextAlign=HorizontalAlignment.Center });
			gridQuoteNotStarted.Columns.Add(new ODGridColumn("Owner Action",90));
			gridQuoteNotStarted.Rows.Clear();
			foreach(Job job in listJobsQuoteNotStarted) {
				ODGridRow row=new ODGridRow() { Tag=job };
				row.Cells.Add(job.ToString());
				row.Cells.Add(Userods.GetName(job.OwnerNum));
				row.Cells.Add(job.OwnerAction.GetDescription());
				gridQuoteNotStarted.Rows.Add(row);
			}
			gridQuoteNotStarted.EndUpdate();
		}

		private void FillGridNoHours() {
			List<Job> listJobsInDevelopmentNoHours=_listJobsAll.Where(x => x.HoursActual<=0
			&& x.PhaseCur.In(JobPhase.Development) 
			&& !x.Category.In(JobCategory.Bug,JobCategory.Query,JobCategory.Conversion,JobCategory.Research)).ToList();
			gridNoHours.BeginUpdate();
			gridNoHours.Columns.Clear();
			gridNoHours.Columns.Add(new ODGridColumn("Job",0));
			gridNoHours.Columns.Add(new ODGridColumn("Owner",70) { TextAlign=HorizontalAlignment.Center });
			gridNoHours.Columns.Add(new ODGridColumn("Owner Action",90));
			gridNoHours.Rows.Clear();
			foreach(Job job in listJobsInDevelopmentNoHours) {
				ODGridRow row=new ODGridRow() { Tag=job };
				row.Cells.Add(job.ToString());
				row.Cells.Add(Userods.GetName(job.OwnerNum));
				row.Cells.Add(job.OwnerAction.GetDescription());
				gridNoHours.Rows.Add(row);
			}
			gridNoHours.EndUpdate();
		}

		private void gridHighPriorityJobs_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormOpenDental.S_GoToJob(((Job)gridHighPriorityJobs.Rows[e.Row].Tag).JobNum);
		}

		private void gridOldJobs_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormOpenDental.S_GoToJob(((Job)gridOldJobs.Rows[e.Row].Tag).JobNum);
		}

		private void gridOverEstimate_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormOpenDental.S_GoToJob(((Job)gridOverEstimate.Rows[e.Row].Tag).JobNum);
		}

		private void gridQuoteOld_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormOpenDental.S_GoToJob(((Job)gridQuoteNotStarted.Rows[e.Row].Tag).JobNum);
		}

		private void gridNoHours_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormOpenDental.S_GoToJob(((Job)gridNoHours.Rows[e.Row].Tag).JobNum);
		}
	}
}