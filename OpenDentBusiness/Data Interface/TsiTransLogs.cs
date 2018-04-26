using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using CodeBase;
using OpenDentalCloud;

namespace OpenDentBusiness{
	///<summary></summary>
	public class TsiTransLogs{
		//If this table type will exist as cached data, uncomment the Cache Pattern region below and edit.
		/*
		#region Cache Pattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add GetTableFromCache and FillCacheFromTable to the Cache.cs file with all the other Cache types.
		//Also, consider making an invalid type for this class in Cache.GetAllCachedInvalidTypes() if needed.

		private class TsiTransLogCache : CacheListAbs<TsiTransLog> {
			protected override List<TsiTransLog> GetCacheFromDb() {
				string command="SELECT * FROM tsitranslog";
				return Crud.TsiTransLogCrud.SelectMany(command);
			}
			protected override List<TsiTransLog> TableToList(DataTable table) {
				return Crud.TsiTransLogCrud.TableToList(table);
			}
			protected override TsiTransLog Copy(TsiTransLog tsiTransLog) {
				return tsiTransLog.Copy();
			}
			protected override DataTable ListToTable(List<TsiTransLog> listTsiTransLogs) {
				return Crud.TsiTransLogCrud.ListToTable(listTsiTransLogs,"TsiTransLog");
			}
			protected override void FillCacheIfNeeded() {
				TsiTransLogs.GetTableFromCache(false);
			}
			protected override bool IsInListShort(TsiTransLog tsiTransLog) {
				return true;//Either change this method or delete it.
			}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static TsiTransLogCache _tsiTransLogCache=new TsiTransLogCache();

		///<summary>A list of all TsiTransLogs. Returns a deep copy.</summary>
		public static List<TsiTransLog> ListDeep {
			get {
				return _tsiTransLogCache.ListDeep;
			}
		}

		///<summary>A list of all non-hidden TsiTransLogs. Returns a deep copy.</summary>
		public static List<TsiTransLog> ListShortDeep {
			get {
				return _tsiTransLogCache.ListShortDeep;
			}
		}

		///<summary>A list of all TsiTransLogs. Returns a shallow copy.</summary>
		public static List<TsiTransLog> ListShallow {
			get {
				return _tsiTransLogCache.ListShallow;
			}
		}

		///<summary>A list of all non-hidden TsiTransLogs. Returns a shallow copy.</summary>
		public static List<TsiTransLog> ListShortShallow {
			get {
				return _tsiTransLogCache.ListShallowShort;
			}
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_tsiTransLogCache.FillCacheFromTable(table);
		}

		///<summary>Returns the cache in the form of a DataTable. Always refreshes the ClientWeb's cache.</summary>
		///<param name="doRefreshCache">If true, will refresh the cache if RemotingRole is ClientDirect or ServerWeb.</param> 
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_tsiTransLogCache.FillCacheFromTable(table);
				return table;
			}
			return _tsiTransLogCache.GetTableFromCache(doRefreshCache);
		}
		#endregion Cache Pattern
		*/
		#region Get Methods

		///<summary>Returns all tsitranslogs for the patients in listPatNums.  Returns empty list if listPatNums is empty or null.</summary>
		public static List<TsiTransLog> SelectMany(List<long> listPatNums){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<TsiTransLog>>(MethodBase.GetCurrentMethod(),listPatNums);
			}
			if(listPatNums==null || listPatNums.Count<1) {
				return new List<TsiTransLog>();
			}
			string command="SELECT * FROM tsitranslog "
				+"WHERE PatNum IN ("+string.Join(",",listPatNums.Select(x => POut.Long(x)))+")";
			return Crud.TsiTransLogCrud.SelectMany(command);
		}

		///<summary>Returns a list of PatNums for guars who have a TsiTransLog with type SS (suspend) less than 50 days ago who don't have a TsiTransLog
		///with type CN (cancel), PF (paid in full), PT (paid in full, thank you), or PL (placement) with a more recent date, since this would change the
		///account status from suspended to either closed/canceled or if the more recent message had type PL (placement) back to active.</summary>
		public static List<long> GetSuspendedGuarNums() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod());
			}
			int[] arrayStatusTransTypes=new[] { (int)TsiTransType.SS,(int)TsiTransType.CN,(int)TsiTransType.RI,(int)TsiTransType.PF,(int)TsiTransType.PT,(int)TsiTransType.PL };
			string command="SELECT DISTINCT tsitranslog.PatNum "
				+"FROM tsitranslog "
				+"INNER JOIN ("
					+"SELECT PatNum,MAX(TransDateTime) transDateTime "
					+"FROM tsitranslog "
					+"WHERE TransType IN("+string.Join(",",arrayStatusTransTypes)+") "
					+"AND TransDateTime>"+POut.DateT(DateTime.Now.AddDays(-50))+" "
					+"GROUP BY PatNum"
				+") mostRecentTrans ON tsitranslog.PatNum=mostRecentTrans.PatNum "
					+"AND tsitranslog.TransDateTime=mostRecentTrans.transDateTime "
				+"WHERE tsitranslog.TransType="+(int)TsiTransType.SS;
			return Db.GetListLong(command);
		}

		public static bool IsGuarSuspended(long guarNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),guarNum);
			}
			int[] arrayStatusTransTypes=new[] { (int)TsiTransType.SS,(int)TsiTransType.CN,(int)TsiTransType.RI,(int)TsiTransType.PF,(int)TsiTransType.PT,(int)TsiTransType.PL };
			string command="SELECT (CASE WHEN tsitranslog.TransType="+(int)TsiTransType.SS+" THEN 1 ELSE 0 END) isGuarSuspended "
				+"FROM tsitranslog "
				+"INNER JOIN ("
					+"SELECT PatNum,MAX(TransDateTime) transDateTime "
					+"FROM tsitranslog "
					+"WHERE PatNum="+POut.Long(guarNum)+" " 
					+"AND TransType IN("+string.Join(",",arrayStatusTransTypes)+") "
					+"AND TransDateTime>"+POut.DateT(DateTime.Now.AddDays(-50))+" "
					+"GROUP BY PatNum"
				+") mostRecentLog ON tsitranslog.PatNum=mostRecentLog.PatNum AND tsitranslog.TransDateTime=mostRecentLog.transDateTime";
			return PIn.Bool(Db.GetScalar(command));
		}

		#endregion Get Methods
		#region Modification Methods
		#region Insert

		public static void InsertMany(List<TsiTransLog> listTsiTransLogs) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listTsiTransLogs);
				return;
			}
			Crud.TsiTransLogCrud.InsertMany(listTsiTransLogs);
		}
		
		#endregion Insert
		#region Update

		///<summary></summary>
		public static void Update(TsiTransLog tsiTransLog,TsiTransLog tsiTransLogOld){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),tsiTransLog,tsiTransLogOld);
				return;
			}
			Crud.TsiTransLogCrud.Update(tsiTransLog,tsiTransLogOld);
		}

		#endregion Update
		#region Delete

		#endregion Delete
		#endregion Modification Methods
		#region Misc Methods

		public static bool ValidateClinicSftpDetails(List<ProgramProperty> listPropsForClinic,bool doTestConnection=true) {
			//No need to check RemotingRole;no call to db.
			string sftpAddress=listPropsForClinic.Find(x => x.PropertyDesc=="SftpServerAddress")?.PropertyValue;
			int sftpPort;
			if(!int.TryParse(listPropsForClinic.Find(x => x.PropertyDesc=="SftpServerPort")?.PropertyValue,out sftpPort)
				|| sftpPort<ushort.MinValue//0
				|| sftpPort>ushort.MaxValue)//65,535
			{
				sftpPort=22;//default to port 22
			}
			string userName=listPropsForClinic.Find(x => x.PropertyDesc=="SftpUsername")?.PropertyValue;
			string userPassword=listPropsForClinic.Find(x => x.PropertyDesc=="SftpPassword")?.PropertyValue;
			if(string.IsNullOrWhiteSpace(sftpAddress) || string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(userPassword)) {
				return false;
			}
			if(doTestConnection) {
				return Sftp.IsConnectionValid(sftpAddress,userName,userPassword,sftpPort);
			}
			return true;
		}

		public static bool IsTransworldEnabled(long clinicNum) {
			//No need to check RemotingRole;no call to db.
			Program progCur=Programs.GetCur(ProgramName.Transworld);
			if(progCur==null || !progCur.Enabled) {
				return false;
			}
			Dictionary<long,List<ProgramProperty>> dictAllProps=ProgramProperties.GetForProgram(progCur.ProgramNum)
				.GroupBy(x => x.ClinicNum)
				.ToDictionary(x => x.Key,x => x.ToList());
			if(dictAllProps.Count==0) {
				return false;
			}
			List<long> listDisabledClinicNums=new List<long>();
			if(PrefC.HasClinicsEnabled) {
				List<Clinic> listAllClinics=Clinics.GetDeepCopy();
				listDisabledClinicNums.AddRange(dictAllProps.Where(x => !TsiTransLogs.ValidateClinicSftpDetails(x.Value,false)).Select(x => x.Key));
				listDisabledClinicNums.AddRange(listAllClinics
					.FindAll(x => x.IsHidden || (listDisabledClinicNums.Contains(0) && !dictAllProps.ContainsKey(x.ClinicNum)))//if no props for HQ, skip other clinics without props
					.Select(x => x.ClinicNum)
				);
			}
			else {
				if(!TsiTransLogs.ValidateClinicSftpDetails(dictAllProps[0],false)) {
					listDisabledClinicNums.Add(0);
				}
			}
			return !listDisabledClinicNums.Contains(clinicNum);
		}

		#endregion Misc Methods

	}

}