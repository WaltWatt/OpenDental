using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CodeBase;
using System.Xml;
using Newtonsoft.Json;

namespace OpenDentBusiness {
	///<summary></summary>
	public class BugSubmissions {
		///<summary>A list of strings that represent parts of unhandled exceptions that should not be uploaded to HQ.
		///E.g. we never want to hear about corrupted tables because there is nothing we can do programmatically to help the customer.
		///They would have to call in to support in order to get help with such an error.</summary>
		private static readonly List<string> _listInvalidExceptionText=new List<string>() {
			"is marked as crashed and should be repaired",
			"for key 'PRIMARY'",
		};

		#region Get Methods
		///<summary></summary>
		public static List<BugSubmission> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<BugSubmission>>(MethodBase.GetCurrentMethod());
			}
			List<BugSubmission> listBugSubs=new List<BugSubmission>();
			DataAction.RunBugsHQ(() => { 
				string command="SELECT * FROM bugsubmission";
				listBugSubs=Crud.BugSubmissionCrud.TableToList(DataCore.GetTable(command));
			},false);
			return listBugSubs;
		}

		///<summary></summary>
		public static List<BugSubmission> GetAllInRange(DateTime dateFrom,DateTime dateTo) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<BugSubmission>>(MethodBase.GetCurrentMethod());
			}
			List<BugSubmission> listBugSubs=new List<BugSubmission>();
			DataAction.RunBugsHQ(() => { 
				string command="SELECT * FROM bugsubmission WHERE "+DbHelper.BetweenDates("SubmissionDateTime",dateFrom,dateTo);
				listBugSubs=Crud.BugSubmissionCrud.TableToList(DataCore.GetTable(command));
			},false);
			return listBugSubs;
		}

		public static List<BugSubmission> GetForBugId(long bugId) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<BugSubmission>>(MethodBase.GetCurrentMethod(),bugId);
			}
			List<BugSubmission> listBugSubs=new List<BugSubmission>();
			if(bugId==0) {
				return listBugSubs;
			}
			DataAction.RunBugsHQ(() => { 
				string command="SELECT * FROM bugsubmission WHERE BugId="+POut.Long(bugId);
				listBugSubs=Crud.BugSubmissionCrud.TableToList(DataCore.GetTable(command));
			},false);
			return listBugSubs;
		}

		///<summary>Gets one BugSubmission from the db.</summary>
		public static BugSubmission GetOne(long bugSubmissionId) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<BugSubmission>(MethodBase.GetCurrentMethod(),bugSubmissionId);
			}
			BugSubmission bugSub=null;
			DataAction.RunBugsHQ(() => { 
				bugSub=Crud.BugSubmissionCrud.SelectOne(bugSubmissionId);
			},false);
			return bugSub;
		}
		
		#endregion

		#region Modification Methods

		#region Insert
		///<summary></summary>
		public static long Insert(BugSubmission bugSubmission) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),bugSubmission);
			}
			long retVal=0;
			//Always use the connection store config file because creating BugSubmissions should always happen via OpenDentalWebServiceHQ.
			DataAction.RunBugsHQ(() => { 
				retVal=Crud.BugSubmissionCrud.Insert(bugSubmission);
			});
			return retVal;
		}
		#endregion

		#region Update
		///<summary></summary>
		public static void Update(BugSubmission bugSubmission) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),bugSubmission);
				return;
			}
			DataAction.RunBugsHQ(() => { 
				Crud.BugSubmissionCrud.Update(bugSubmission);
			},false);
		}
		
		///<summary>Updates all bugIds for given bugSubmissionNums.</summary>
		public static void UpdateBugIds(long bugId,List<long> listBugSubmissionNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),bugId,listBugSubmissionNums);
				return;
			}
			if(listBugSubmissionNums==null || listBugSubmissionNums.Count==0) {
				return;
			}
			DataAction.RunBugsHQ(() => { 
				Db.NonQ("UPDATE bugsubmission SET BugId="+POut.Long(bugId)
					+" WHERE BugSubmissionNum IN ("+string.Join(",",listBugSubmissionNums)+")");
			},false);
		}
		#endregion

		#region Delete
		///<summary></summary>
		public static void Delete(long bugSubmissionId) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),bugSubmissionId);
				return;
			}
			DataAction.RunBugsHQ(() => { 
				Crud.BugSubmissionCrud.Delete(bugSubmissionId);
			},false);
		}
		#endregion

		#endregion

		#region Misc Methods
		
		///<summary>Attempts to submit an exception to HQ.
		///Checks PrefName.SendUnhandledExceptionsToHQ prior to web call.
		///Returns BugSubmissionResult.UpdateRequired when submitter is not on most recent stable or any version of the beta.
		///Returns BugSubmissionResult.Failed when an error occured in the web call method.
		///Returns BugSubmissionResult.Success when bugSubmissions was successfully created at HQ.</summary>
		public static BugSubmissionResult SubmitException(Exception ex,string threadName="",long patNumCur=-1,string moduleName="") {			
			if(!PrefC.GetBool(PrefName.SendUnhandledExceptionsToHQ)
				|| _listInvalidExceptionText.Any(x => ex.Message.ToLower().Contains(x.ToLower()))) 
			{
				return BugSubmissionResult.None;
			}
			return BugSubmissions.ParseBugSubmissionResult(
				WebServiceMainHQProxy.GetWebServiceMainHQInstance().SubmitUnhandledException(
					WebServiceMainHQProxy.CreateWebServiceHQPayload(
						WebServiceMainHQProxy.CreatePayloadContent(
							new BugSubmission(ex,threadName,patNumCur,moduleName),"bugSubmission")
						,eServiceCode.BugSubmission)));
		}

		///<summary>After calling WebServiceMainHQ.SubmitUnhandledException(...) this will digest the result and provide the result.</summary>
		public static BugSubmissionResult ParseBugSubmissionResult(string result) {
			XmlDocument doc=new XmlDocument();
			doc.LoadXml(result);
			if(doc.SelectSingleNode("//Error")!=null) {
				return BugSubmissionResult.Failed;
			}
			XmlNode responseNode=doc.SelectSingleNode("//Success");
			if(responseNode!=null) {
				return (responseNode.InnerText=="true"? BugSubmissionResult.UpdateRequired : BugSubmissionResult.Success);
			}
			return BugSubmissionResult.None;//Just in case;
		}
		
		public static string GetSubmissionDescription(Patient patCur,BugSubmission sub) {
			return "Caller Name and #: "+patCur.GetNameLF() +" (work) "+patCur.WkPhone+"\r\n"
				+"Quick desc: "+sub.ExceptionMessageText+"\r\n"
				+"OD version: "+sub.Info.DictPrefValues[PrefName.ProgramVersion]+"\r\n"
				+"Windows version: "+sub.Info.WindowsVersion+"\r\n"
				+"Comps affected: "+sub.Info.CompName+"\r\n"
				+"Database name: "+sub.Info.DatabaseName+"\r\n"
				+"Example PatNum: " +sub.Info.PatientNumCur+"\r\n"
				+"Details: "+"\r\n"
				+"Duplicable?: "+"\r\n"
				+"Steps to duplicate: "+"\r\n"
				+"Exception:  "+sub.ExceptionStackTrace;
		}
		#endregion
		
	}

	public enum BugSubmissionResult {
		///<summary></summary>
		None,
		///<summary>Submitter is not on support or there was an exception in the web method</summary>
		Failed,
		///<summary>Submitter must be on the most recent stable or any beta version.</summary>
		UpdateRequired,
		///<summary>Submitter sucesfully insert a bugSubmission at HQ</summary>
		Success,
	}
}