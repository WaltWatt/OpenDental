using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
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
		
		///<summary></summary>
		public static bool TryAssociateSimilarBugSubmissions(List<BugSubmission> listAllSubs,Point pointFormLocaiton) {
			List<BugSubmission> listUnattachedSubs=listAllSubs.Where(x => x.BugId==0).ToList();
			if(listUnattachedSubs.Count==0) {
				MsgBox.Show("FormBugSubmissions","All submissions are associated to bugs already.");
				return false;
			}
			//Dictionary where key is a BugId and the value is list of submissions associated to that BugID.
			//StackTraces are unique and if there are duplicate stack trace entries we select the one with the newest version.
			Dictionary<long,List<BugSubmission>> dictAttachedSubs=listAllSubs.Where(x => x.BugId!=0)
				.GroupBy(x => x.BugId)
				.ToDictionary(x => x.Key, x => 
					x.GroupBy(y => y.ExceptionStackTrace)
					//Sub dictionary of unique ExceptionStackStraces as the key and the value is the submission from the highest version.
					.ToDictionary(y => y.Key, y => y.OrderByDescending(z => new Version(z.Info.DictPrefValues[PrefName.ProgramVersion])).First())
					.Values.ToList()
				);
			Dictionary<long,List<BugSubmission>> dictSimilarBugSubs=new Dictionary<long,List<BugSubmission>>();
			List<long> listOrderedKeys=dictAttachedSubs.Keys.OrderByDescending(x => x).ToList();
			foreach(long key in listOrderedKeys) {//Loop through submissions that are already attached to bugs
				dictSimilarBugSubs[key]=new List<BugSubmission>();
				foreach(BugSubmission sub in dictAttachedSubs[key]) {//Loop through the unique exception text from the submission with thie highest reported version.
					List<BugSubmission> listSimilarBugSubs=listUnattachedSubs.Where(x => 
						x.ExceptionStackTrace==sub.ExceptionStackTrace//Find submissions that are not attached to bugs with identical ExceptionStackTrace
					).ToList();
					if(listSimilarBugSubs.Count==0) {
						continue;//All submissions with this stack trace are attached to a bug.
					}
					listUnattachedSubs.RemoveAll(x => listSimilarBugSubs.Contains(x));
					dictSimilarBugSubs[key].AddRange(listSimilarBugSubs);
				}
			}
			if(dictSimilarBugSubs.All(x => x.Value.Count==0)) {
				MsgBox.Show("FormBugSubmissions","All similar submissions are already attached to bugs.  No action needed.");
				return false;
			}
			dictSimilarBugSubs=dictSimilarBugSubs.Where(x => x.Value.Count!=0).ToDictionary(x => x.Key,x => x.Value);
			bool isAutoAssign=(MsgBox.Show("FormBugSubmissions",MsgBoxButtons.YesNo,"Click Yes to auto attach duplicate submissions to bugs with identical stack traces?"
				+"\r\nClick No to manually validate all groupings found."));
			List<long> listBugIds=listAllSubs.Where(x => x.BugId!=0).Select(x => x.BugId).ToList();
			List<JobLink> listLinks=JobLinks.GetManyForType(JobLinkType.Bug,listBugIds);
			List<Bug> listBugs=Bugs.GetMany(listBugIds);
			foreach(KeyValuePair<long,List<BugSubmission>> pair in dictSimilarBugSubs) {
				Bug bugFixed=listBugs.FirstOrDefault(x => x.BugId==pair.Key && !string.IsNullOrEmpty(x.VersionsFixed));
				if(bugFixed!=null) {
					List<BugSubmission> listIssueSubs=pair.Value.Where(x => new Version(x.Info.DictPrefValues[PrefName.ProgramVersion])>=new Version(bugFixed.VersionsFixed.Split(';').Last())).ToList();
					if(listIssueSubs.Count>0) {
						MsgBox.Show("FormBugSubmissions","There appears to be a submission on a version that is newer than the previous fixed bug version (BugId: "+POut.Long(bugFixed.BugId)+").  "
							+"These will be excluded.");
						//TODO: Allow user to view these excluded submissions somehow.
						pair.Value.RemoveAll(x => listIssueSubs.Contains(x));
						if(pair.Value.Count==0) {
							continue;
						}
					}
				}
				if(!isAutoAssign) {
					FormBugSubmissions formGroupBugSubs=new FormBugSubmissions(viewMode:FormBugSubmissionMode.ValidationMode);
					formGroupBugSubs.ListViewedSubs=pair.Value;//Add unnattached submissions to grid
					formGroupBugSubs.ListViewedSubs.AddRange(dictAttachedSubs[pair.Key]);//Add already attached submissions to grid
					formGroupBugSubs.StartPosition=FormStartPosition.Manual;
					Point newLoc=pointFormLocaiton;
					newLoc.X+=10;//Offset
					newLoc.Y+=10;
					formGroupBugSubs.Location=newLoc;
					if(formGroupBugSubs.ShowDialog()!=DialogResult.OK) {
						continue;
					}	
				}
				BugSubmissions.UpdateBugIds(pair.Key,pair.Value.Select(x => x.BugSubmissionNum).ToList());
			}
			MsgBox.Show("FormBugSubmissions","Done.");
			return true;
		}

	}
}
