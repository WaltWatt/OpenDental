using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class JobReviews{
		
		///<summary></summary>
		public static List<JobReview> GetForReviewer(long userNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<JobReview>>(MethodBase.GetCurrentMethod(),userNum);
			}
			string command="SELECT * FROM jobreview WHERE Reviewer = "+POut.Long(userNum);
			return Crud.JobReviewCrud.SelectMany(command);
		}

		///<summary></summary>
		public static List<JobReview> GetForJob(long jobNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<JobReview>>(MethodBase.GetCurrentMethod(),jobNum);
			}
			string command="SELECT * FROM jobreview WHERE JobNum = "+POut.Long(jobNum);
			return Crud.JobReviewCrud.SelectMany(command);
		}

		///<summary>Gets one JobReview from the db.</summary>
		public static JobReview GetOne(long jobReviewNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<JobReview>(MethodBase.GetCurrentMethod(),jobReviewNum);
			}
			return Crud.JobReviewCrud.SelectOne(jobReviewNum);
		}


		public static DataTable GetOutstandingForUser(long userNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),userNum);
			}
			string command="SELECT jobreview.*,job.JobNum,job.Owner,job.Title FROM jobreview "
				+"INNER JOIN joblink ON jobreview.JobReviewNum=joblink.FKey "
				+"INNER JOIN job ON joblink.JobNum=job.JobNum "
				+"WHERE jobreview.Reviewer="+POut.Long(userNum)+" "
				+"AND jobreview.ReviewStatus IN("+POut.Long((int)JobReviewStatus.Sent)
				+","+POut.Long((int)JobReviewStatus.Seen)+","+POut.Long((int)JobReviewStatus.UnderReview)+") ";
				//+"AND joblink.LinkType="+POut.Long((int)JobLinkType.Review);
			return Db.GetTable(command);
		}

		public static void SetSeen(long reviewNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),reviewNum);
			}
			string command="UPDATE jobreview SET ReviewStatus="+POut.Long((int)JobReviewStatus.Seen)+" "
				+"WHERE JobReviewNum="+POut.Long(reviewNum);
			Db.NonQ(command);
		}

		///<summary></summary>
		public static long Insert(JobReview jobReview){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				jobReview.JobReviewNum=Meth.GetLong(MethodBase.GetCurrentMethod(),jobReview);
				return jobReview.JobReviewNum;
			}
			return Crud.JobReviewCrud.Insert(jobReview);
		}

		///<summary></summary>
		public static void Update(JobReview jobReview){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobReview);
				return;
			}
			Crud.JobReviewCrud.Update(jobReview);
		}

		///<summary>Deletes a joblink of the specified type and num.</summary>
		public static void Delete(long jobReviewNum){//, JobLinkType jobLinkType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobReviewNum);
				return;
			}
			Crud.JobReviewCrud.Delete(jobReviewNum);
		}

		public static void DeleteForJob(long jobNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),jobNum);
				return;
			}
			string command="DELETE FROM jobquote WHERE JobNum="+POut.Long(jobNum);
			Db.NonQ(command);
		}

		public static void Sync(List<JobReview> listNew,long jobNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listNew,jobNum);
				return;
			}
			List<JobReview> listDB=GetForJob(jobNum);
			Crud.JobReviewCrud.Sync(listNew,listDB);
		}

		public static List<JobReview> GetJobReviewsForJobs(List<long> jobNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<JobReview>>(MethodBase.GetCurrentMethod(),jobNums);
			}
			if(jobNums==null || jobNums.Count==0) {
				return new List<JobReview>();
			}
			string command="SELECT * FROM jobreview WHERE JobNum IN ("+string.Join(",",jobNums)+") "
				+"ORDER BY DateTStamp";
			return Crud.JobReviewCrud.SelectMany(command);
		}


	}
}