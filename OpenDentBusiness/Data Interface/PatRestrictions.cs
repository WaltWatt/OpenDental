using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness {
	///<summary></summary>
	public class PatRestrictions{
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

		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern

		private class PatRestrictionCache : CacheListAbs<PatRestriction> {
			protected override List<PatRestriction> GetCacheFromDb() {
				string command="SELECT * FROM PatRestriction ORDER BY ItemOrder";
				return Crud.PatRestrictionCrud.SelectMany(command);
			}
			protected override List<PatRestriction> TableToList(DataTable table) {
				return Crud.PatRestrictionCrud.TableToList(table);
			}
			protected override PatRestriction Copy(PatRestriction PatRestriction) {
				return PatRestriction.Clone();
			}
			protected override DataTable ListToTable(List<PatRestriction> listPatRestrictions) {
				return Crud.PatRestrictionCrud.ListToTable(listPatRestrictions,"PatRestriction");
			}
			protected override void FillCacheIfNeeded() {
				PatRestrictions.GetTableFromCache(false);
			}
			protected override bool IsInListShort(PatRestriction PatRestriction) {
				return !PatRestriction.IsHidden;
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static PatRestrictionCache _PatRestrictionCache=new PatRestrictionCache();

		///<summary>A list of all PatRestrictions. Returns a deep copy.</summary>
		public static List<PatRestriction> ListDeep {
			get {
				return _PatRestrictionCache.ListDeep;
			}
		}

		///<summary>A list of all visible PatRestrictions. Returns a deep copy.</summary>
		public static List<PatRestriction> ListShortDeep {
			get {
				return _PatRestrictionCache.ListShortDeep;
			}
		}

		///<summary>A list of all PatRestrictions. Returns a shallow copy.</summary>
		public static List<PatRestriction> ListShallow {
			get {
				return _PatRestrictionCache.ListShallow;
			}
		}

		///<summary>A list of all visible PatRestrictions. Returns a shallow copy.</summary>
		public static List<PatRestriction> ListShort {
			get {
				return _PatRestrictionCache.ListShallowShort;
			}
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_PatRestrictionCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_PatRestrictionCache.FillCacheFromTable(table);
				return table;
			}
			return _PatRestrictionCache.GetTableFromCache(doRefreshCache);
		}

		#endregion
		*/

		///<summary>Gets all patrestrictions for the specified patient.</summary>
		public static List<PatRestriction> GetAllForPat(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PatRestriction>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM patrestriction WHERE PatNum="+POut.Long(patNum);
			return Crud.PatRestrictionCrud.SelectMany(command);
		}

		///<summary>This will only insert a new PatRestriction if there is not already an existing PatRestriction in the db for this patient and type.
		///If exists, returns the PatRestrictionNum of the first one found.  Otherwise returns the PatRestrictionNum of the newly inserted one.</summary>
		public static long Upsert(long patNum,PatRestrict patRestrictType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetLong(MethodBase.GetCurrentMethod(),patNum,patRestrictType);
			}
			List<PatRestriction> listPatRestricts=GetAllForPat(patNum).FindAll(x => x.PatRestrictType==patRestrictType);
			if(listPatRestricts.Count>0) {
				return listPatRestricts[0].PatRestrictionNum;
			}
			return Crud.PatRestrictionCrud.Insert(new PatRestriction() { PatNum=patNum,PatRestrictType=patRestrictType });
		}

		///<summary>Checks for an existing patrestriction for the specified patient and PatRestrictType.
		///If one exists, returns true (IsRestricted).  If none exist, returns false (!IsRestricted).</summary>
		public static bool IsRestricted(long patNum,PatRestrict patRestrictType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetBool(MethodBase.GetCurrentMethod(),patNum,patRestrictType);
			}
			string command="SELECT COUNT(*) FROM patrestriction WHERE PatNum="+POut.Long(patNum)+" AND PatRestrictType="+POut.Int((int)patRestrictType);
			if(PIn.Int(Db.GetCount(command))>0) {
				return true;
			}
			else {
				return false;
			}
		}

		///<summary>Gets the human readable description of the patrestriction, passed through Lans.g.</summary>
		///Returns empty string if the enum was not found in the switch statement.</summary>
		public static string GetPatRestrictDesc(PatRestrict patRestrictType) {
			switch(patRestrictType) {
				case PatRestrict.ApptSchedule:
					return Lans.g("patRestrictEnum","Appointment Scheduling");
				case PatRestrict.None:
				default:
					return "";
			}
		}

		///<summary>Deletes any patrestrictions for the specified patient and type.</summary>
		public static void RemovePatRestriction(long patNum,PatRestrict patRestrictType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patNum,patRestrictType);
				return;
			}
			string command="DELETE FROM patrestriction WHERE PatNum="+POut.Long(patNum)+" AND PatRestrictType="+POut.Int((int)patRestrictType);
			Db.NonQ(command);
			return;
		}

		//Only pull out the methods below as you need them.  Otherwise, leave them commented out.
		/*
		///<summary>Gets one PatRestriction from the db.</summary>
		public static PatRestriction GetOne(long patRestrictionNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<PatRestriction>(MethodBase.GetCurrentMethod(),patRestrictionNum);
			}
			return Crud.PatRestrictionCrud.SelectOne(patRestrictionNum);
		}

		///<summary></summary>
		public static void Update(PatRestrict patRestrict){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patRestrict);
				return;
			}
			Crud.PatRestrictionCrud.Update(patRestrict);
		}

		///<summary></summary>
		public static void Delete(long patRestrictionNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patRestrictionNum);
				return;
			}
			Crud.PatRestrictionCrud.Delete(patRestrictionNum);
		}		
		*/



	}
}