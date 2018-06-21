using CodeBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Bugs{
		private const int THREAD_TIMEOUT=5000;

		///<summary>Returns a list of bugs for given bugIds.</summary>
		public static List<Bug> GetMany(List<long> listBugIds) {
			if(listBugIds==null || listBugIds.Count==0) {
				return new List<Bug>();
			}
			List<Bug> listBugs=new List<Bug>();
			DataAction.RunBugsHQ(() => {
				listBugs=Crud.BugCrud.TableToList(DataCore.GetTable("SELECT * FROM bug WHERE BugID IN ("+string.Join(",",listBugIds)+")"));
			},false);
			return listBugs;
		}

		///<summary>Must pass in version as "Maj" or "Maj.Min" or "Maj.Min.Rev". Uses like operator. Can return null if the thread fails to join.</summary>
		public static List<Bug> GetByVersion(string versionMajMin,string filter="") {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Bug>>(MethodBase.GetCurrentMethod(),versionMajMin,filter);
			}
			//Create an ODThread so that we can safely change the database connection settings without affecting the calling method's connection.
			ODThread odThread=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				//Always set the thread static database connection variables to set the serviceshq db conn.
#if DEBUG
				new DataConnection().SetDbT("localhost","bugs","root","","","",DatabaseType.MySql,true);
#else
				new DataConnection().SetDbT("server","bugs","root","","","",DatabaseType.MySql,true);
#endif
				string command="SELECT * FROM bug WHERE (VersionsFound LIKE '"+POut.String(versionMajMin)+"%' OR VersionsFound LIKE '%;"+POut.String(versionMajMin)+"%' OR "+
				"VersionsFixed LIKE '"+POut.String(versionMajMin)+"%' OR VersionsFixed LIKE '%;"+POut.String(versionMajMin)+"%') ";
				if(filter!="") {
					command+="AND Description LIKE '%"+POut.String(filter)+"%'";
				}
				o.Tag=Crud.BugCrud.SelectMany(command);
			}));
			odThread.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception e) => { }));//Do nothing
			odThread.Name="bugsGetByVersionThread";
			odThread.Start(true);
			if(!odThread.Join(THREAD_TIMEOUT)) {
				return null;
			}
			return (List<Bug>)odThread.Tag;
		}

		///<summary>Can return null if the thread fails to join.</summary>
		public static List<Bug> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Bug>>(MethodBase.GetCurrentMethod());
			}
			//Create an ODThread so that we can safely change the database connection settings without affecting the calling method's connection.
			ODThread odThread=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				//Always set the thread static database connection variables to set the serviceshq db conn.
#if DEBUG
				new DataConnection().SetDbT("localhost","bugs","root","","","",DatabaseType.MySql,true);
#else
				new DataConnection().SetDbT("server","bugs","root","","","",DatabaseType.MySql,true);
#endif
				string command="SELECT * FROM bug ORDER BY CreationDate DESC";
				o.Tag=Crud.BugCrud.TableToList(DataCore.GetTable(command));
			}));
			odThread.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception e) => { }));//Do nothing
			odThread.Name="bugsGetAllThread";
			odThread.Start(true);
			if(!odThread.Join(THREAD_TIMEOUT)) {
				return null;
			}
			return (List<Bug>)odThread.Tag;
		}

		///<summary>Gets one Bug from the db. Can return null if the thread fails to join.</summary>
		public static Bug GetOne(long bugId) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Bug>(MethodBase.GetCurrentMethod(),bugId);
			}
			//Create an ODThread so that we can safely change the database connection settings without affecting the calling method's connection.
			ODThread odThread=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				//Always set the thread static database connection variables to set the serviceshq db conn.
#if DEBUG
				new DataConnection().SetDbT("localhost","bugs","root","","","",DatabaseType.MySql,true);
#else
				new DataConnection().SetDbT("server","bugs","root","","","",DatabaseType.MySql,true);
#endif
				o.Tag=Crud.BugCrud.SelectOne(bugId);
			}));
			odThread.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception e) => { }));//Do nothing
			odThread.Name="bugsGetOneThread";
			odThread.Start(true);
			if(!odThread.Join(THREAD_TIMEOUT)) {
				return null;
			}
			return (Bug)odThread.Tag;
		}

		///<summary></summary>
		public static long Insert(Bug bug) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),bug);
			}
			//Create an ODThread so that we can safely change the database connection settings without affecting the calling method's connection.
			ODThread odThread=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				//Always set the thread static database connection variables to set the serviceshq db conn.
#if DEBUG
				new DataConnection().SetDbT("localhost","bugs","root","","","",DatabaseType.MySql,true);
#else
				new DataConnection().SetDbT("server","bugs","root","","","",DatabaseType.MySql,true);
#endif
				Crud.BugCrud.Insert(bug);
			}));
			odThread.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception e) => { }));//Do nothing
			odThread.Name="bugsInsertThread";
			odThread.Start(true);
			if(!odThread.Join(THREAD_TIMEOUT)) {
				return 0;
			}
			return bug.BugId;
		}

		///<summary></summary>
		public static void Update(Bug bug) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),bug);
				return;
			}
			//Create an ODThread so that we can safely change the database connection settings without affecting the calling method's connection.
			ODThread odThread=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				//Always set the thread static database connection variables to set the serviceshq db conn.
#if DEBUG
				new DataConnection().SetDbT("localhost","bugs","root","","","",DatabaseType.MySql,true);
#else
				new DataConnection().SetDbT("server","bugs","root","","","",DatabaseType.MySql,true);
#endif
				Crud.BugCrud.Update(bug);
			}));
			odThread.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception e) => { }));//Do nothing
			odThread.Name="bugsUpdateThread";
			odThread.Start(true);
			if(!odThread.Join(THREAD_TIMEOUT)) {
				return;
			}
		}

		///<summary></summary>
		public static void Delete(long bugId) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),bugId);
				return;
			}
			JobLinks.DeleteForType(JobLinkType.Bug,bugId);
			//Create an ODThread so that we can safely change the database connection settings without affecting the calling method's connection.
			ODThread odThread=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				//Always set the thread static database connection variables to set the serviceshq db conn.
#if DEBUG
				new DataConnection().SetDbT("localhost","bugs","root","","","",DatabaseType.MySql,true);
#else
				new DataConnection().SetDbT("server","bugs","root","","","",DatabaseType.MySql,true);
#endif
				Crud.BugCrud.Delete(bugId);
			}));
			odThread.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception e) => { }));//Do nothing
			odThread.Name="bugsDeleteThread";
			odThread.Start(true);
			if(!odThread.Join(THREAD_TIMEOUT)) {
				return;
			}
		}

		///<Summary>Gets name from database.  Not very efficient.</Summary>
		public static string GetSubmitterName(long submitter){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),submitter);
			}
			//Create an ODThread so that we can safely change the database connection settings without affecting the calling method's connection.
			ODThread odThread=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				//Always set the thread static database connection variables to set the serviceshq db conn.
#if DEBUG
				new DataConnection().SetDbT("localhost","bugs","root","","","",DatabaseType.MySql,true);
#else
				new DataConnection().SetDbT("server","bugs","root","","","",DatabaseType.MySql,true);
#endif
				string command="SELECT UserName FROM buguser WHERE BugUserId="+submitter;
				o.Tag=Db.GetScalar(command);
			}));
			odThread.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception e) => { }));//Do nothing
			odThread.Name="bugsGetSubmitterNameThread";
			odThread.Start(true);
			if(!odThread.Join(THREAD_TIMEOUT)) {
				return "";
			}
			return odThread.Tag.ToString();
		}

		///<Summary>Gets name from database.  Not very efficient.</Summary>
		public static SerializableDictionary<long,string> GetDictSubmitterNames(List<long> listSubmitterNums){
			if(listSubmitterNums==null || listSubmitterNums.Count==0) {
				return new SerializableDictionary<long, string>();
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<SerializableDictionary<long,string>>(MethodBase.GetCurrentMethod(),listSubmitterNums);
			}
			SerializableDictionary<long,string> dictSubmitterNames=new SerializableDictionary<long, string>();
			DataAction.RunBugsHQ(() => {
				dictSubmitterNames=Db.GetTable("SELECT BugUserId,UserName from buguser WHERE BugUserId IN ("+string.Join(",",listSubmitterNums)+")").Select()
					.ToSerializableDictionary(x => PIn.Long(x["BugUserId"].ToString()),x => PIn.String(x["UserName"].ToString()));
			},false);
			return dictSubmitterNames;
		}

		///<Summary>Checks bugIDs in list for incompletes. Returns false if incomplete exists.</Summary>
		public static bool CheckForCompletion(List<long> listBugIDs) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),listBugIDs);
			}
			//Create an ODThread so that we can safely change the database connection settings without affecting the calling method's connection.
			ODThread odThread = new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				//Always set the thread static database connection variables to set the serviceshq db conn.
#if DEBUG
				new DataConnection().SetDbT("localhost","bugs","root","","","",DatabaseType.MySql,true);
#else
				new DataConnection().SetDbT("server","bugs","root","","","",DatabaseType.MySql,true);
#endif
				string command = "SELECT COUNT(*) FROM bug "
					+"WHERE VersionsFixed='' "
					+"AND BugId IN ("+String.Join(",",listBugIDs)+")";
				o.Tag=Db.GetCount(command);
			}));
			odThread.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception e) => {
			}));//Do nothing
			odThread.Name="bugsCheckForCompletionThread";
			odThread.Start(true);
			if(!odThread.Join(THREAD_TIMEOUT)) {
				return true;
			}
			if(PIn.Int(odThread.Tag.ToString())!=0) {
				return false;
			}
			return true;
		}

		public static Bug GetNewBugForUser() {
			Bug bug=new Bug();
			bug.CreationDate=DateTime.Now;
			bug.Status_=BugStatus.None;
			switch(System.Environment.MachineName) {
				case "ANDREW":
				case "ANDREW1":
					bug.Submitter=29;//Andrew
					break;
				case "JORDANS":
				case "JORDANS3":
					bug.Submitter=4;//jsparks
					break;
				case "JASON":
					bug.Submitter=18;//jsalmon
					break;
				case "DAVID":
					bug.Submitter=27;//david
					break;
				case "DEREK":
					bug.Submitter=1;//grahamde
					break;
				case "SAM":
					bug.Submitter=25;//sam
					break;
				case "RYAN":
				case "RYAN1":
					bug.Submitter=20;//Ryan
					break;
				case "CAMERON":
					bug.Submitter=21;//Cameron
					break;
				case "TRAVIS":
					bug.Submitter=22;//tgriswold
					break;
				case "ALLEN":
					bug.Submitter=24;//allen
					break;
				case "JOSH":
					bug.Submitter=26;//josh
					break;
				case "JOE":
					bug.Submitter=28;//joe
					break;
				case "CHRISM":
					bug.Submitter=30;//chris
					break;
				case "SAUL":
					bug.Submitter=31;//saul
					break;
				case "MATHERINL":
					bug.Submitter=32;//matherinl
					break;
				case "LINDSAYS":
					bug.Submitter=33;//linsdays
					break;
				case "BRENDANB":
					bug.Submitter=34;//brendanb
					break;
				case "KENDRAS":
					bug.Submitter=35;//kendras
					break;
				case "STEVENS":
					bug.Submitter=37;//stevens
					break;
				case "LUKEM":
					bug.Submitter=41;//lukem
					break;
				default:
					bug.Submitter=2;//Tech Support
					break;
			}
			bug.Type_=BugType.Bug;
			bug.VersionsFound=VersionReleases.GetLastReleases(2);
			return bug;
		}

		///<summary>Attempts to get the ValueString from the bug's preference table for the given PrefName.  The ValueString retrieved from the db will be converted to T.
		///Returns true if the preference was found and successfully converted to the type of T.  Otherwise retVal will be set to the default for T and false will be returned.</summary>
		public static bool TryGetPrefValue<T>(string prefName,out T retVal) {
			retVal=default(T);
			try {
				string val="";
				DataAction.RunBugsHQ(() => {
					val=Db.GetScalar("SELECT ValueString FROM preference WHERE PrefName='"+prefName+"'");
				},false);
				retVal=(T)Convert.ChangeType(val,typeof(T));
			}
			catch(Exception ex) {
				ex.DoNothing();
				return false;
			}
			return true;
		}

	}
}