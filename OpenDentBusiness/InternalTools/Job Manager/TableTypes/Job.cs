using CodeBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace OpenDentBusiness {
	///<summary>This table is not part of the general release.  User would have to add it manually.  All schema changes are done directly on our live database as needed.
	/// Base object for use in the job tracking system.</summary>
	[Serializable]
	[CrudTable(IsMissingInGeneral=true)]
	public class Job:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey = true)]
		public long JobNum;
		///<summary>FK to userod.UserNum.  Used for Query Creator as well.</summary>
		public long UserNumConcept;
		///<summary>FK to userod.UserNum.  Used for Query Reviewer as well.</summary>
		public long UserNumExpert;
		///<summary>FK to userod.UserNum.  Used for Query Owner as well.</summary>
		public long UserNumEngineer;
		///<summary>FK to userod.UserNum.</summary>
		public long UserNumApproverConcept;
		///<summary>FK to userod.UserNum.</summary>
		public long UserNumApproverJob;
		///<summary>FK to userod.UserNum.</summary>
		public long UserNumApproverChange;
		///<summary>FK to userod.UserNum.</summary>
		public long UserNumDocumenter;
		///<summary>FK to userod.UserNum.</summary>
		public long UserNumCustContact;
		///<summary>FK to userod.UserNum. IF set, this is the user currently editing the job. Once saved, this is set to 0.</summary>
		public long UserNumCheckout;
		///<summary>FK to userod.UserNum. If this is set, then the job is waiting on clarification from the user indicated. Once clarified, this is set back to 0. Not actually used yet.</summary>
		public long UserNumInfo;
		///<summary>FK to job.JobNum.</summary>
		public long ParentNum;
		///<summary>The date/time that the customer was contacted.</summary>
		[CrudColumn(SpecialType = CrudSpecialColType.DateT)]
		public DateTime DateTimeCustContact;
		///<summary>FK to def.DefNum</summary>
		public long Priority;
		///<summary>The type of the job.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.EnumAsString)]
		public JobCategory Category;
		///<summary>The version the job is for. Example: 15.4.19, 16.1.1</summary>
		public string JobVersion;
		///<summary>The estimated time a job will take.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TimeSpanLong)]
		public TimeSpan TimeEstimate;
		///<summary>Deprecated: The actual time a job took.  Use HoursActual (or something similar to the logic within) instead.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TimeSpanLong)]
		public TimeSpan TimeActual;
		///<summary>The date/time that the job was created.  Not user editable.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateTEntry)]
		public DateTime DateTimeEntry;
		///<summary>The implementation of the job. RTF content of the main body of the Job.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]//Text
		public string Implementation;
		///<summary>Used to record what was documented for this job and where it was documented.</summary>
		[CrudColumn(SpecialType = CrudSpecialColType.TextIsClob)]//Text
		public string Documentation;
		///<summary>The short title of the job.</summary>
		public string Title;
		///<summary>The current status of the job.  Historical statuses for this job can be found in the jobevent table.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.EnumAsString)]
		public JobPhase PhaseCur;
		///<summary>Applies to Several status.</summary>
		public bool IsApprovalNeeded;
		///<summary>Not yet used. Will be used for tracking acknowledgement of Bugs by Nathan. Should not halt development.</summary>
		[CrudColumn(SpecialType = CrudSpecialColType.DateT)]
		public DateTime AckDateTime;
		///<summary>FK to userod.UserNum.</summary>
		public long UserNumQuoter;
		///<summary>FK to userod.UserNum.</summary>
		public long UserNumApproverQuote;
		///<summary>FK to userod.UserNum.</summary>
		public long UserNumCustQuote;
		///<summary>The requirements of the job. RTF content of the main requirements of the Job.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]//Text
		public string Requirements;
		///<summary>FK to userod.UserNum.  The user that has taken or was assigned this job in order to perform testing.</summary>
		public long UserNumTester;
		///<summary>FK to definition.DefNum.  Since testing can happen at during any phase, the testing department has this separate Priority.</summary>
		public long PriorityTesting;

		//The following varables should be filled by the class that uses them. Not filled in S class. 
		//Just a convenient way to package a job for passing around in the job manager.
		///<summary>Not a data column.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public List<JobLink> ListJobLinks=new List<JobLink>();
		///<summary>Not a data column.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public List<JobNote> ListJobNotes=new List<JobNote>();
		///<summary>Not a data column.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public List<JobReview> ListJobReviews=new List<JobReview>();
		///<summary>Not a data column.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public List<JobReview> ListJobTimeLogs=new List<JobReview>();
		///<summary>Not a data column.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public List<JobQuote> ListJobQuotes=new List<JobQuote>();
		///<summary>Not a data column.</summary>
		[CrudColumn(IsNotDbColumn = true)]
		public List<JobLog> ListJobLogs = new List<JobLog>();

		public Job() {
			JobVersion="";
			Requirements="";
			Implementation="";
			Title="";
		}

		///<summary></summary>
		public Job Copy() {
			Job job=(Job)this.MemberwiseClone();
			job.ListJobLinks=this.ListJobLinks.Select(x => x.Copy()).ToList();
			job.ListJobNotes=this.ListJobNotes.Select(x => x.Copy()).ToList();
			job.ListJobReviews=this.ListJobReviews.Select(x => x.Copy()).ToList();
			job.ListJobTimeLogs=this.ListJobTimeLogs.Select(x => x.Copy()).ToList();
			job.ListJobQuotes=this.ListJobQuotes.Select(x => x.Copy()).ToList();
			job.ListJobLogs=this.ListJobLogs.Select(x => x.Copy()).ToList();
			return job;
		}

		///<summary>The actual hours a job has taken so far.</summary>
		public double HoursActual {
			get {
				return ListJobTimeLogs.Sum(x => x.TimeReview.TotalHours);
			}
		}

		///<summary>The estimated hours a job will take.</summary>
		public double HoursEstimate {
			get {
				return TimeEstimate.TotalHours;
			}
			set {
				TimeEstimate=TimeSpan.FromHours(value);
			}
		}

		///<summary>Returns userNum of the person assigned to the next task for a job, 0 if unnassigned.</summary>
		public long OwnerNum {
			get {
				if(UserNumInfo>0) {
					return UserNumInfo;
				}
				switch(PhaseCur) {
					case JobPhase.Concept:
						if(IsApprovalNeeded) {
							return UserNumApproverConcept;
						}
						return UserNumConcept;
					case JobPhase.Quote:
						if(IsApprovalNeeded) {
							return UserNumApproverQuote;
						}
						else if(UserNumApproverQuote==0) {
							return UserNumQuoter;
						}
						return UserNumCustContact;
					case JobPhase.Definition:
						if(IsApprovalNeeded || UserNumExpert==0) {
							return UserNumApproverJob;
						}
						return UserNumExpert;
					case JobPhase.Development:
						if(IsApprovalNeeded || UserNumExpert==0) {
							return UserNumApproverJob;
						}
						if(UserNumEngineer==0) {
							return UserNumExpert;
						}
						if(ListJobReviews.Any(x => x.ReviewStatus!=JobReviewStatus.Done 
								&& x.ReviewStatus!=JobReviewStatus.NeedsAdditionalWork 
								&& x.ReviewStatus!=JobReviewStatus.NeedsAdditionalReview 
								&& x.ReviewStatus!=JobReviewStatus.SaveCommit 
								&& x.ReviewStatus!=JobReviewStatus.SaveCommitted))
						{
							JobReview review=ListJobReviews.FirstOrDefault(x => x.ReviewStatus!=JobReviewStatus.Done 
								&& x.ReviewStatus!=JobReviewStatus.NeedsAdditionalWork 
								&& x.ReviewStatus!=JobReviewStatus.NeedsAdditionalReview
								&& x.ReviewStatus!=JobReviewStatus.SaveCommit 
								&& x.ReviewStatus!=JobReviewStatus.SaveCommitted);
							if(review!=null) {
								return review.ReviewerNum;
							}
							return 0;
						}
						return UserNumEngineer;
					case JobPhase.Documentation:
						return UserNumDocumenter;
					case JobPhase.Complete:
						if(DateTimeCustContact.Year<1880) {
							return UserNumCustContact;
						}
						return 0;
					case JobPhase.Cancelled:
					default:
						return 0;
				}
			}
		}

		///<summary>Same as GetOwnerAction() but wrapped in a Property for convenience. </summary>
		public JobAction OwnerAction {
			get {
				if(this.UserNumInfo>0) {
					return JobAction.AnswerQuestion;
				}
				switch(this.PhaseCur) {
					case JobPhase.Concept:
						if(this.IsApprovalNeeded) {
							return JobAction.ApproveConcept;
						}
						return JobAction.WriteConcept;
					case JobPhase.Quote:
						if(this.IsApprovalNeeded) {
							return JobAction.ApproveQuote;
						}
						else if(this.UserNumCustQuote==0) {
							return JobAction.NeedsQuote;
						}
						return JobAction.NeedsCustomerQuoteApproval;
					case JobPhase.Definition:
						if(this.IsApprovalNeeded) {
							return JobAction.ApproveJob;
						}
						if(this.UserNumExpert==0) {
							return JobAction.AssignExpert;
						}
						return JobAction.WriteJob;
					case JobPhase.Development:
						if(this.IsApprovalNeeded) {
							return JobAction.ApproveChanges;
						}
						if(this.UserNumExpert==0) {
							return JobAction.AssignExpert;
						}
						if(this.UserNumEngineer==0) {
							return JobAction.AssignEngineer;
						}
						//If at least one review is marked done then the job needs no more reviews.
						if(this.ListJobReviews.Any(x => x.ReviewStatus==JobReviewStatus.Done)) {
							return JobAction.WriteCode;
						}
						if(this.ListJobReviews.Any(x => x.ReviewStatus!=JobReviewStatus.Done 
							&& x.ReviewStatus!=JobReviewStatus.NeedsAdditionalWork 
							&& x.ReviewStatus!=JobReviewStatus.NeedsAdditionalReview
							&& x.ReviewStatus!=JobReviewStatus.SaveCommit
							&& x.ReviewStatus!=JobReviewStatus.SaveCommitted)) //.Any on empty list returns false.
						{
							return JobAction.ReviewCode;
						}
						return JobAction.WriteCode;
					case JobPhase.Documentation:
						if(UserNumDocumenter!=0) {
							return JobAction.Document;
						}
						else {
							return JobAction.NeedsTechnicalWriter;
						}
					case JobPhase.Complete:
						if(DateTimeCustContact.Year<1880) {
							return JobAction.ContactCustomer;
						}
						return JobAction.None;
					case JobPhase.Cancelled:
						return JobAction.None;
					default:
						return JobAction.UnknownJobPhase;
				}
			}
		}

		///<summary>Similar To owner action, but allows you to specify a user.</summary>
		public JobAction ActionForUser(Userod user) {
			if(user==null) {
				return OwnerAction;//should not happen, just a precaution
			}
			if(user.UserNum==0) {
				if(OwnerNum==0) {
					return OwnerAction;
				}
				return JobAction.None;
			}
			JobAction baseAction = OwnerAction;
			switch(baseAction) {
				case JobAction.WriteConcept:
					if(JobPermissions.IsAuthorized(JobPerm.Concept,true,user.UserNum) && (this.UserNumConcept==0 || this.UserNumConcept==user.UserNum)) {
						return baseAction;
					}
					return JobAction.Undefined;
				case JobAction.AnswerQuestion:
					if(this.UserNumInfo==user.UserNum) {
						return baseAction;
					}
					return JobAction.Undefined;
				case JobAction.NeedsQuote:
					if(JobPermissions.IsAuthorized(JobPerm.Writeup,true,user.UserNum) && this.UserNumQuoter==user.UserNum) {
						return baseAction;
					}
					return JobAction.Undefined;
				case JobAction.WaitForQuote:
					if(JobPermissions.IsAuthorized(JobPerm.Approval,true,user.UserNum)) {
						return baseAction;
					}
					return JobAction.Undefined;
				case JobAction.ApproveQuote:
					if(JobPermissions.IsAuthorized(JobPerm.Approval,true,user.UserNum)) {
						return baseAction;
					}
					return JobAction.Undefined;
				case JobAction.NeedsCustomerQuoteApproval:
					if(JobPermissions.IsAuthorized(JobPerm.NotifyCustomer,true,user.UserNum) && this.UserNumCustQuote==user.UserNum) {
						return baseAction;
					}
					if(JobPermissions.IsAuthorized(JobPerm.Approval,true,user.UserNum)) {
						return JobAction.WaitForQuote;
					}
					return JobAction.Undefined;
				case JobAction.WriteJob:
					if(JobPermissions.IsAuthorized(JobPerm.Writeup,true,user.UserNum) && (this.UserNumExpert==0 || this.UserNumExpert==user.UserNum)) {
						return baseAction;
					}
					return JobAction.Undefined;
				case JobAction.AssignEngineer:
					if(this.UserNumExpert==user.UserNum) {
						return baseAction;
					}
					if(JobPermissions.IsAuthorized(JobPerm.Engineer,true,user.UserNum)) {
						return JobAction.TakeJob;
					}
					return JobAction.Undefined;
				case JobAction.WriteCode:
					if(this.UserNumEngineer==user.UserNum) {
						return baseAction;
					}
					if(this.UserNumExpert==user.UserNum) {
						return JobAction.Advise;
					}
					return JobAction.Undefined;
				case JobAction.ReviewCode:
					if(this.ListJobReviews.Any(x=> !x.ReviewStatus.In(JobReviewStatus.Done,JobReviewStatus.NeedsAdditionalReview,JobReviewStatus.NeedsAdditionalWork,JobReviewStatus.SaveCommit,JobReviewStatus.SaveCommitted) 
					&& (x.ReviewerNum==user.UserNum || (x.ReviewerNum==0 && JobPermissions.IsAuthorized(JobPerm.Writeup,true,user.UserNum))))) {
						return baseAction;
					}
					if(this.UserNumEngineer==user.UserNum) {
						return JobAction.WaitForReview;
					}
					return JobAction.Undefined;
				case JobAction.ApproveConcept:
				case JobAction.ApproveJob:
				case JobAction.ApproveChanges:
					if(JobPermissions.IsAuthorized(JobPerm.Approval,true,user.UserNum)) {
						return baseAction;
					}
					if((this.UserNumConcept==user.UserNum && PhaseCur==JobPhase.Concept) || this.UserNumEngineer==user.UserNum || this.UserNumExpert==user.UserNum) {
						return JobAction.WaitForApproval;
					}
					return JobAction.Undefined;
				case JobAction.AssignExpert:	
					if(JobPermissions.IsAuthorized(JobPerm.Approval,true,user.UserNum)) {
						return baseAction;
					}
					return JobAction.Undefined;
				case JobAction.NeedsTechnicalWriter:
				case JobAction.Document:
					if(JobPermissions.IsAuthorized(JobPerm.Documentation,true,user.UserNum)) {
						return baseAction;
					}
					if(JobPermissions.IsAuthorized(JobPerm.NotifyCustomer,true,user.UserNum) && (UserNumCustContact==0 || UserNumCustContact==user.UserNum) && DateTimeCustContact.Year<1880) {
						return JobAction.ContactCustomerPreDoc;
					}
					return JobAction.Undefined;
				case JobAction.ContactCustomer:
					if(JobPermissions.IsAuthorized(JobPerm.NotifyCustomer,true,user.UserNum) && (UserNumCustContact==0 || UserNumCustContact==user.UserNum)) {
						return baseAction;
					}
					return JobAction.Undefined;
				default:
					if(ListJobLinks.Exists(x => x.LinkType==JobLinkType.Subscriber && x.FKey==user.UserNum && !PhaseCur.In(JobPhase.Cancelled,JobPhase.Complete))) {
						return JobAction.Watching;
					}
					return baseAction;
			}
		}

		///<summary>Used primarily to display a Job in the tree view.</summary>
		public override string ToString() {
			return Category.ToString().Substring(0,1)+JobNum+" - "+Title;
		}
	}


	public enum JobPhase {
		///<summary>0 -</summary>
		Concept, //From Concept, NeedsConceptApproval.
		///<summary>1 -</summary>
		Definition, //From ConceptApproved, CurrentlyWriting, NeedsJobApproval, NeedsJobClarification
		///<summary>2 -</summary>
		Development, //From JobApproved,Assigned, CurrentlyWorkingOn, OnHoldExpert, ReadyForReview, OnHoldEngineer, ReadyToAssign
		///<summary>3 -</summary>
		Documentation, //From ReadyToBeDocumented, NeedsDocumentationClarification
		///<summary>4 -</summary>
		Complete, //From Complete, NotifyCustomer
		///<summary>5 -</summary>
		Cancelled, //From Rescinded, Deleted
		///<summary>6 -</summary>
		Quote,
	}

	///<summary>Never Stored in the DB. Only used for sorting and displaying. The order these values are ordered in this enum is the order they will be displayed in.</summary>
	public enum JobAction {
		[Description("Review Code")]
		ReviewCode,
		[Description("Needs Quote")]
		NeedsQuote,
		[Description("Approve Quote")]
		ApproveQuote,
		[Description("Approve Changes")]
		ApproveChanges,
		[Description("Approve Concept")]
		ApproveConcept,
		[Description("Approve Job")]
		ApproveJob,
		[Description("Answer Question")]
		AnswerQuestion,
		[Description("Write Concept")]
		WriteConcept,
		[Description("Write Job")]
		WriteJob,
		[Description("Customer Quote")]
		NeedsCustomerQuoteApproval,
		[Description("Assign Expert")]
		AssignExpert,
		[Description("Assign Engineer")]
		AssignEngineer,
		[Description("Write Code")]
		WriteCode,
		[Description("Take Job")]
		TakeJob,
		[Description("Document")]
		Document,
		[Description("Assign Technical Writer")]
		NeedsTechnicalWriter,
		[Description("Advise")]
		Advise,
		[Description("Wait for Approval")]
		WaitForApproval,
		[Description("Wait for Quote")]
		WaitForQuote,
		[Description("Wait for Review")]
		WaitForReview,
		[Description("Contact Customer Pre-Documentation")]
		ContactCustomerPreDoc,
		[Description("Contact Customer")]
		ContactCustomer,
		[Description("Watching")]
		Watching,
		[Description("None")]
		None,
		[Description("Unknown Job Phase")]
		UnknownJobPhase,
		[Description("")]
		Undefined
	}


	public enum JobStatus {
		///<summary>0 -</summary>
		Concept, //From Concept, NeedsConceptApproval.
		///<summary>1 -</summary>
		Definiton, //Writeup, //From ConceptApproved, CurrentlyWriting, NeedsJobApproval, NeedsJobClarification
		///<summary>2 -</summary>
		Development, //From JobApproved,Assigned, CurrentlyWorkingOn, OnHoldExpert, ReadyForReview, OnHoldEngineer, ReadyToAssign
		///<summary>3 -</summary>
		Documentation, //From ReadyToBeDocumented, NeedsDocumentationClarification
		///<summary>4 -</summary>
		Complete, //From Complete, NotifyCustomer
		///<summary>5 -</summary>
		Cancelled //From Rescinded, Deleted
	}

	public enum JobCategory {
		///<summary>0 -</summary>
		Feature,
		///<summary>1 -</summary>
		Bug,
		///<summary>2 -</summary>
		Enhancement,
		///<summary>3 -</summary>
		Query,
		///<summary>4 -</summary>
		ProgramBridge,
		///<summary>5 -</summary>
		InternalRequest,
		///<summary>6 -</summary>
		HqRequest,
		///<summary>7 -</summary>
		Conversion,
		///<summary>8 -</summary>
		Research
	}

}


