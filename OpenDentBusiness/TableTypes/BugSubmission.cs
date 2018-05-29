using CodeBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace OpenDentBusiness {

	///<summary>The BugSubmission table lives on the bugs database.
	///All queries should be done with the bugs database context from WebServiceMainHQ.
	///This table is only in OpenDentBusiness because the bug table from the bugs database lives here as well,
	/// and we want to follow that pattern since this table links to it.</summary>
	[Serializable]
	[CrudTable(IsMissingInGeneral=true)]
	public class BugSubmission:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long BugSubmissionNum;
		/// <summary>Automatically set to the date and time upon insert. Uses server time.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateTEntry)]
		public DateTime SubmissionDateTime;
		///<summary>FK to bug.BugId. Can be 0.</summary>
		public long BugId;
		///<summary>The value of PrefName.RegistrationKey from the submitting office's DB.
		///Automatically set in constructor.</summary>
		public string RegKey;
		///<summary>The version the customer was on when submitting the bug.</summary>
		public string DbVersion;
		///<summary>The string from an excetions.GetMessage</summary>
		public string ExceptionMessageText;
		///<summary>The raw full statck trace.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string ExceptionStackTrace;
		///<summary>A JSON object storing important key value pairs for backwards/forwards compatibility.
		///Will contain important preferences, as well as DbInfoField values, in the "key":"value" format of JSON
		///Automatically set in constructor.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string DbInfoJson;
		///<summary>Used to add general notes to a submissions.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string DevNote;
		
		[XmlIgnore,JsonIgnore]
		[CrudColumn(IsNotDbColumn=true)]
		private SubmissionInfo _info;
		
		///<summary>Any additional information gathered at the time the bug is submitted.</summary>
		[XmlIgnore,JsonIgnore]
		public SubmissionInfo Info {
			get {
				if(_info==null) {
					_info=JsonConvert.DeserializeObject<SubmissionInfo>(DbInfoJson);
				}
				return _info;
			}
			//set { } Read only property
		}

		///<summary>This constructor should only be used when obtaining existing rows from the database.</summary>
		public BugSubmission() {
			
		}

		///<summary>This constructor utilizes the Open Dental preference cache.
		///Only use this constructor from an Open Dental proper instance, never call from other entities (e.g. WebServiceMainHQ).</summary>
		public BugSubmission(Exception e,string threadName="",long patNum=-1,string moduleName="") : this() {
			ExceptionMessageText=e.Message;
			ExceptionStackTrace=MiscUtils.GetExceptionText(e);
			DbInfoJson=GetDbInfoJSON(patNum,moduleName);
			//The following lines will fail if we do not have the office's database context in DataConnection.
			RegKey=Info.DictPrefValues[PrefName.RegistrationKey];
			DbVersion=Info.DictPrefValues[PrefName.DataBaseVersion];
			Info.ThreadName=threadName;
		}
	
		public BugSubmission Copy() {
			return (BugSubmission)this.MemberwiseClone();
		}

		///<summary>Returns serialized DbInfo object as JSON string of database info from both the preference table and non preferernce table info.</summary>
		private string GetDbInfoJSON(long patNum,string moduleName) {
			_info=new BugSubmission.SubmissionInfo();
			try {
				//This list is not in a separate method because we want to ensure that future development related to bug submissions don't try to make assumptions
				//on which preferences are in an object at any given time.
				//Ex.  Let's say in version 17.4, the list doesn't contain the payplan version preference, but 17.5 does.
				//If we called the method that retrieves the used preferences from WebServiceMainHQ which in this example is on version 17.5,
				// it would think all bugsubmission rows contain the payplan version preference when that is not the case.
				List<PrefName> listPrefs=new List<PrefName>() {
					PrefName.AtoZfolderUsed,
					PrefName.ClaimSnapshotEnabled,
					PrefName.ClaimSnapshotRunTime,
					PrefName.ClaimSnapshotTriggerType,
					PrefName.CorruptedDatabase,
					PrefName.DataBaseVersion,
					PrefName.EasyNoClinics,
					PrefName.LanguageAndRegion,
					PrefName.MySqlVersion,
					PrefName.PayPlansVersion,
					PrefName.ProcessSigsIntervalInSecs,
					PrefName.ProgramVersionLastUpdated,
					PrefName.ProgramVersion,
					PrefName.RandomPrimaryKeys,
					PrefName.RegistrationKey,
					PrefName.RegistrationKeyIsDisabled,
					PrefName.ReplicationFailureAtServer_id,
					PrefName.ReportingServerCompName,
					PrefName.ReportingServerDbName,
					PrefName.ReportingServerMySqlUser,
					PrefName.ReportingServerMySqlPassHash,
					PrefName.ReportingServerURI,
					PrefName.WebServiceServerName
				};
				foreach(PrefName pref in listPrefs) {
					_info.DictPrefValues[pref]=Prefs.GetOne(pref).ValueString;
				}
				_info.CountClinics=Clinics.GetCount();
				_info.EnabledPlugins=Programs.GetWhere(x => x.Enabled && !string.IsNullOrWhiteSpace(x.PluginDllName)).Select(x => x.ProgName).ToList();
				_info.ClinicNumCur=Clinics.ClinicNum;
				_info.UserNumCur=Security.CurUser.UserNum;
				_info.PatientNumCur=patNum;
				_info.IsOfficeOnReplication=(ReplicationServers.GetCount() > 0 ? true : false);
				_info.IsOfficeUsingMiddleTier=(RemotingClient.RemotingRole==RemotingRole.ClientWeb ? true : false);
				_info.WindowsVersion=MiscData.GetOSVersionInfo();
				_info.CompName=Environment.MachineName;
				List<UpdateHistory> listHist=UpdateHistories.GetPreviouUpdateHistories(2);//Ordered by newer versions first.
				_info.PreviousUpdateVersion=listHist.Count==2 ? listHist[1].ProgramVersion : "";//Show the previous version they updated from
				_info.PreviousUpdateTime=listHist.Count>0 ? listHist[0].DateTimeUpdated : DateTime.MinValue;//Show when they updated to the current version.
				_info.ModuleNameCur=moduleName;
				_info.DatabaseName=DataConnection.GetDatabaseName();
			}
			catch(Exception ex) {
				ex.DoNothing();
			}
			return JsonConvert.SerializeObject(_info);
		}
		
		///<summary>This is not a database table.  This is the structure of the DbInfoJSON column.
		///This class doesn't throw exceptions when using JsonConvert.SerializeObject and JsonConvert.DeserializeObject
		/// for missing/additional columns that what it thinks it should get.</summary>
		public class SubmissionInfo {
			///<summary></summary>
			public Dictionary<PrefName,string> DictPrefValues=new Dictionary<PrefName, string>();
			///<summary></summary>
			public int CountClinics;
			///<summary>List of enabled plugin names.</summary>
			public List<string> EnabledPlugins;
			///<summary></summary>
			public long ClinicNumCur;
			///<summary></summary>
			public long UserNumCur;
			///<summary></summary>
			public long PatientNumCur;
			///<summary>The module the user was on when the exception occurred.</summary>
			public string ModuleNameCur;
			///<summary></summary>
			public bool IsOfficeOnReplication;
			///<summary></summary>
			public bool IsOfficeUsingMiddleTier;
			///<summary></summary>
			public string WindowsVersion;
			///<summary></summary>
			public string CompName;
			///<summary>Lets us know which version the user came from.  E.g. this will always be a version prior to CurrentUpdateVersion.
			///Helpful for conversations with the customer and gives us a timeline / story as to where the user came from and how long they were using that previous version.</summary>
			public string PreviousUpdateVersion;
			///<summary>Lets us know when the user updated to this particular version.  E.g. this will always be a date and time prior to CurrentUpdateTime.
			///Helpful for conversations with the customer and gives us a timeline / story as to where the user came from and how long they were using that previous version.</summary>
			public DateTime PreviousUpdateTime;
			///<summary>The version that the user is currently using.</summary>
			public string CurrentUpdateVersion;
			///<summary>The exact time that the user started using the current version.  Helpful in case we have a dangerous upgrade script get exposed to the public.</summary>
			public DateTime CurrentUpdateTime;
			///<summary>The thread that had the UE. This will say ProgramEntry if it was on the main thread of OD proper.</summary>
			public string ThreadName;
			///<summary></summary>
			public string DatabaseName;
			///<summary></summary>
			public string ConnectionString;//TODO: might not have variable for this.
			///<summary>Typically blank, but when the office is using middle tier, it might be helpful to get their URI so that we can connect from our end (if they make us a user).</summary>
			public string MiddleTierURI;//TODO:Both this and ConnectionString are pulled from a config file.
		}

	}

}

/*
if(DataConnection.DBtype==DatabaseType.MySql) {
	command="DROP TABLE IF EXISTS bugsubmission";
	Db.NonQ(command);
	command=@"CREATE TABLE bugsubmission (
		BugSubmissionNum bigint NOT NULL auto_increment PRIMARY KEY,
		SubmissionDateTime datetime NOT NULL DEFAULT '0001-01-01 00:00:00',
		BugId bigint NOT NULL,
		RegKey varchar(255) NOT NULL,
		DbVersion varchar(255) NOT NULL,
		ExceptionMessageText varchar(255) NOT NULL,
		ExceptionStackTrace text NOT NULL,
		DbInfoJson text NOT NULL,
		INDEX(BugId)
		) DEFAULT CHARSET=utf8";
	Db.NonQ(command);
}
else {//oracle
	command="BEGIN EXECUTE IMMEDIATE 'DROP TABLE bugsubmission'; EXCEPTION WHEN OTHERS THEN NULL; END;";
	Db.NonQ(command);
	command=@"CREATE TABLE bugsubmission (
		BugSubmissionNum number(20) NOT NULL,
		SubmissionDateTime date DEFAULT TO_DATE('0001-01-01','YYYY-MM-DD') NOT NULL,
		BugId number(20) NOT NULL,
		RegKey varchar2(255),
		DbVersion varchar2(255),
		ExceptionMessageText varchar2(255),
		ExceptionStackTrace clob,
		DbInfoJson clob,
		CONSTRAINT bugsubmission_BugSubmissionNum PRIMARY KEY (BugSubmissionNum)
		)";
	Db.NonQ(command);
	command=@"CREATE INDEX bugsubmission_BugId ON bugsubmission (BugId)";
	Db.NonQ(command);
}
*/
