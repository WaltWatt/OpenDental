using CodeBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class JobLogs{
		#region Get Methods
		#endregion

		#region Modification Methods
		
		#region Insert
		#endregion

		#region Update
		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods
		#endregion

		/// <summary>Inserts log entry to DB and returns the resulting JobLog.</summary>
		public static JobLog MakeLogEntry(Job jobNew,Job jobOld,bool isManualLog=false) {
			if(jobNew==null) {
				return null;//should never happen
			}
			JobLog jobLog = new JobLog() {
				JobNum=jobNew.JobNum,
				UserNumChanged=Security.CurUser.UserNum,
				UserNumExpert=jobNew.UserNumExpert,
				UserNumEngineer=jobNew.UserNumEngineer,
				Description=""
			};
			if(isManualLog) {
				jobLog.Description="Manual \"last worked on\" update";
				JobLogs.Insert(jobLog);
				return JobLogs.GetOne(jobLog.JobLogNum);//to get new timestamp.
			}
			List<string> logDescriptions = new List<string>();
			if(jobOld.IsApprovalNeeded && !jobNew.IsApprovalNeeded) {
				if(jobOld.PhaseCur==JobPhase.Concept  && (jobNew.PhaseCur==JobPhase.Definition || jobNew.PhaseCur==JobPhase.Development)) {
					logDescriptions.Add("Concept approved.");
					jobLog.MainRTF=jobNew.Implementation;
					jobLog.RequirementsRTF+=jobNew.Requirements;
				}
				if((jobOld.PhaseCur==JobPhase.Concept || jobOld.PhaseCur==JobPhase.Definition) && jobNew.PhaseCur==JobPhase.Development) {
					logDescriptions.Add("Job approved.");
					jobLog.MainRTF=jobNew.Implementation;
					jobLog.RequirementsRTF+=jobNew.Requirements;
				}
				if(jobOld.PhaseCur==JobPhase.Development && jobNew.PhaseCur==JobPhase.Development) {
					logDescriptions.Add("Changes approved.");
					jobLog.MainRTF=jobNew.Implementation;
					jobLog.RequirementsRTF+=jobNew.Requirements;
				}
			}
			else if(jobNew.PhaseCur.In(JobPhase.Documentation,JobPhase.Complete) && !jobOld.PhaseCur.In(JobPhase.Documentation,JobPhase.Complete)) {
				logDescriptions.Add("Job implemented.");
				jobLog.MainRTF+=jobNew.Implementation;
				jobLog.RequirementsRTF+=jobNew.Requirements;
			}
			if(jobOld.PhaseCur>jobNew.PhaseCur && jobOld.PhaseCur!=JobPhase.Cancelled) {
				logDescriptions.Add("Job Unapproved.");//may be a chance for a false positive when using override permission.
			}
			if(jobOld.PhaseCur!=JobPhase.Cancelled && jobNew.PhaseCur==JobPhase.Cancelled) {
				logDescriptions.Add("Job Cancelled.");//may be a chance for a false positive when using override permission.
			}
			if(jobNew.UserNumExpert!=jobOld.UserNumExpert) {
				logDescriptions.Add("Expert changed.");
			}
			if(jobNew.UserNumEngineer!=jobOld.UserNumEngineer) {
				logDescriptions.Add("Engineer changed.");
			}
			if(jobOld.Requirements!=jobNew.Requirements) {
				logDescriptions.Add("Job Requirements Changed.");
				jobLog.RequirementsRTF+=jobNew.Requirements;
			}
			if(jobOld.Implementation!=jobNew.Implementation) {
				logDescriptions.Add("Job Implementation Changed.");
				jobLog.MainRTF+=jobNew.Implementation;
			}
			if(jobOld.Title!=jobNew.Title) {
				logDescriptions.Add("Job Title Changed.");
			}
			jobLog.Title=jobNew.Title;
			jobLog.Description=string.Join("\r\n",logDescriptions);
			JobLogs.Insert(jobLog);
			return JobLogs.GetOne(jobLog.JobLogNum);//to get new timestamp.
		}

		public static JobLog MakeLogEntryForView(Job job) {
			JobLog jobLog = new JobLog() {
				JobNum=job.JobNum,
				UserNumChanged=Security.CurUser.UserNum,
				UserNumExpert=job.UserNumExpert,
				UserNumEngineer=job.UserNumEngineer,
				Title=job.Title,
				Description="Job Viewed"
			};
			JobLogs.Insert(jobLog);
			return JobLogs.GetOne(jobLog.JobLogNum);//to get new timestamp.
		}

		public static JobLog MakeLogEntryForEstimateChange(Job job,double oldHours,double newHours) {
			JobLog jobLog = new JobLog() {
				JobNum=job.JobNum,
				UserNumChanged=Security.CurUser.UserNum,
				UserNumExpert=job.UserNumExpert,
				UserNumEngineer=job.UserNumEngineer,
				Title=job.Title,
				Description="Job Estimate Changed From "+oldHours+" hours To "+newHours+" hours."
			};
			JobLogs.Insert(jobLog);
			return JobLogs.GetOne(jobLog.JobLogNum);//to get new timestamp.
		}

		public static JobLog MakeLogEntryForTitleChange(Job job,string oldTitle,string newTitle) {
			JobLog jobLog = new JobLog() {
				JobNum=job.JobNum,
				UserNumChanged=Security.CurUser.UserNum,
				UserNumExpert=job.UserNumExpert,
				UserNumEngineer=job.UserNumEngineer,
				Title=newTitle,
				Description="Job Title Changed From\r\n"+oldTitle+"\r\nTo\r\n"+newTitle+"."
			};
			JobLogs.Insert(jobLog);
			return JobLogs.GetOne(jobLog.JobLogNum);//to get new timestamp.
		}

		public static void DeleteForJob(long jobNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobNum);
				return;
			}
			string command = "DELETE FROM joblog WHERE JobNum="+POut.Long(jobNum);
			Db.NonQ(command);
		}

		///<summary></summary>
		public static List<JobLog> GetJobLogsForJobs(List<long> listJobNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<JobLog>>(MethodBase.GetCurrentMethod(),listJobNums);
			}
			if(listJobNums==null || listJobNums.Count==0) {
				return new List<JobLog>();
			}
			string command = "SELECT * FROM joblog WHERE JobNum IN ("+string.Join(",",listJobNums)+") "
					+"ORDER BY DateTimeEntry";
			return Crud.JobLogCrud.SelectMany(command);
		}

		///<summary>Gets one JobLog from the db.</summary>
		public static JobLog GetOne(long jobLogNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<JobLog>(MethodBase.GetCurrentMethod(),jobLogNum);
			}
			return Crud.JobLogCrud.SelectOne(jobLogNum);
		}

		///<summary></summary>
		public static long Insert(JobLog jobLog) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				jobLog.JobLogNum=Meth.GetLong(MethodBase.GetCurrentMethod(),jobLog);
				return jobLog.JobLogNum;
			}
			return Crud.JobLogCrud.Insert(jobLog);
		}

	}
}