using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	class BugSubmissionL {
		
		///<summary>Attempts to add a bug for the given sub and job.
		///Job can be null, only used for pre appending "(Enhancement)" to bug description.
		///Returns null if user canceled.</summary>
		public static Bug AddBug(BugSubmission sub,Job job=null) {
			FormBugEdit formBE;
			formBE=new FormBugEdit(new List<BugSubmission>() { sub });
			formBE.IsNew=true;
			formBE.BugCur=Bugs.GetNewBugForUser();
			if(job!=null && job.Category==JobCategory.Enhancement) {
				formBE.BugCur.Description="(Enhancement)";
			}
			if(formBE.ShowDialog()!=DialogResult.OK) {
				return null;
			}
			BugSubmissions.UpdateBugIds(formBE.BugCur.BugId,new List<long> { sub.BugSubmissionNum });
			return formBE.BugCur;
		}

		public static long CreateTask(Patient pat,BugSubmission sub) {
			//Button is only enabled if _patCur is not null (user has 1 row selected).
			//Mimics FormOpenDental.OnTask_Click()
			FormTaskListSelect FormT=new FormTaskListSelect(TaskObjectType.Patient);
			//FormT.Location=new Point(50,50);
			FormT.Text=Lan.g(FormT,"Add Task")+" - "+FormT.Text;
			FormT.ShowDialog();
			if(FormT.DialogResult!=DialogResult.OK) {
				return 0;
			}
			Task task=new Task();
			task.TaskListNum=-1;//don't show it in any list yet.
			Tasks.Insert(task);
			Task taskOld=task.Copy();
			task.KeyNum=pat.PatNum;
			task.ObjectType=TaskObjectType.Patient;
			task.TaskListNum=FormT.ListSelectedLists[0];
			task.UserNum=Security.CurUser.UserNum;
			//Mimics the ?bug quick note at HQ.
			task.Descript=BugSubmissions.GetSubmissionDescription(pat,sub);
			FormTaskEdit FormTE=new FormTaskEdit(task,taskOld);
			FormTE.IsNew=true;
			FormTE.ShowDialog();
			return task.TaskNum;
		}

		public static Bug AddBugAndJob(ODForm form,List<BugSubmission> listSelectedSubs,Patient pat) {
			if(!JobPermissions.IsAuthorized(JobPerm.Concept,true)
				&& !JobPermissions.IsAuthorized(JobPerm.NotifyCustomer,true)
				&& !JobPermissions.IsAuthorized(JobPerm.FeatureManager,true)
				&& !JobPermissions.IsAuthorized(JobPerm.Documentation,true)) 
			{
				return null;
			}
			if(listSelectedSubs.Count==0) {
				MsgBox.Show(form,"You must select a bug submission to create a job for.");
				return null;
			}
			Job jobNew=new Job();
			jobNew.Category=JobCategory.Bug;
			InputBox titleBox=new InputBox("Provide a brief title for the job.");
			if(titleBox.ShowDialog()!=DialogResult.OK) {
				return null;
			}
			if(String.IsNullOrEmpty(titleBox.textResult.Text)) {
				MsgBox.Show(form,"You must type a title to create a job.");
				return null;
			}
			List<Def> listJobPriorities=Defs.GetDefsForCategory(DefCat.JobPriorities,true);
			if(listJobPriorities.Count==0) {
				MsgBox.Show(form,"You have no priorities setup in definitions.");
				return null;
			}
			jobNew.Title=titleBox.textResult.Text;
			long priorityNum=0;
			priorityNum=listJobPriorities.FirstOrDefault(x => x.ItemValue.Contains("BugDefault")).DefNum;
			jobNew.Priority=priorityNum==0?listJobPriorities.First().DefNum:priorityNum;
			jobNew.PhaseCur=JobPhase.Concept;
			jobNew.UserNumConcept=Security.CurUser.UserNum;
			Bug bugNew=new Bug();
			bugNew=Bugs.GetNewBugForUser();
			InputBox bugDescription=new InputBox("Provide a brief description for the bug. This will appear in the bug tracker.",jobNew.Title);
			if(bugDescription.ShowDialog()!=DialogResult.OK) {
				return null;
			}
			if(String.IsNullOrEmpty(bugDescription.textResult.Text)) {
				MsgBox.Show(form,"You must type a description to create a bug.");
				return null;
			}
			FormVersionPrompt FormVP=new FormVersionPrompt("Enter versions found");
			FormVP.ShowDialog();
			if(FormVP.DialogResult!=DialogResult.OK || string.IsNullOrEmpty(FormVP.VersionText)) {
				MsgBox.Show(form,"You must enter a version to create a bug.");
				return null;
			}
			bugNew.Status_=BugStatus.Accepted;
			bugNew.VersionsFound=FormVP.VersionText;
			bugNew.Description=bugDescription.textResult.Text;
			BugSubmission sub=listSelectedSubs.First();
			jobNew.Requirements=BugSubmissions.GetSubmissionDescription(pat,sub);
			Jobs.Insert(jobNew);
			JobLink jobLinkNew=new JobLink();
			jobLinkNew.LinkType=JobLinkType.Bug;
			jobLinkNew.JobNum=jobNew.JobNum;
			jobLinkNew.FKey=Bugs.Insert(bugNew);
			JobLinks.Insert(jobLinkNew);
			BugSubmissions.UpdateBugIds(bugNew.BugId,listSelectedSubs.Select(x => x.BugSubmissionNum).ToList());
			if(MsgBox.Show(form,MsgBoxButtons.YesNo,"Would you like to create a task too?")) {
				long taskNum=CreateTask(pat,sub);
				if(taskNum!=0) {
					jobLinkNew=new JobLink();
					jobLinkNew.LinkType=JobLinkType.Task;
					jobLinkNew.JobNum=jobNew.JobNum;
					jobLinkNew.FKey=taskNum;
					JobLinks.Insert(jobLinkNew);
				}
			}
			Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,jobNew.JobNum);
			FormOpenDental.S_GoToJob(jobNew.JobNum);
			return bugNew;
		}
		
	}
}
