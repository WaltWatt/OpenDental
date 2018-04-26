using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary>In ProcGroupItems the ProcNum is a procedure in a group and GroupNum is the group the procedure is in. GroupNum is a FK to the Procedure table. There is a special type of procedure with the procedure code "~GRP~" that is used to indicate this is a group Procedure.</summary>
	public class ProcGroupItems{
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


		///<summary></summary>
		public static List<ProcGroupItem> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ProcGroupItem>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT procgroupitem.* FROM procgroupitem "
					+"INNER JOIN procedurelog ON procedurelog.ProcNum=procgroupitem.GroupNum AND procedurelog.PatNum="+POut.Long(patNum);
			return Crud.ProcGroupItemCrud.SelectMany(command);
		}

		///<summary>Gets all the ProcGroupItems for a Procedure Group.</summary>
		public static List<ProcGroupItem> GetForGroup(long groupNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ProcGroupItem>>(MethodBase.GetCurrentMethod(),groupNum);
			}
			string command="SELECT * FROM procgroupitem WHERE GroupNum = "+POut.Long(groupNum)+" ORDER BY ProcNum ASC";//Order is important for creating signature key in FormProcGroup.cs.
			return Crud.ProcGroupItemCrud.SelectMany(command);
		}

		///<summary>Adds a procedure to a group.</summary>
		public static long Insert(ProcGroupItem procGroupItem){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				procGroupItem.ProcGroupItemNum=Meth.GetLong(MethodBase.GetCurrentMethod(),procGroupItem);
				return procGroupItem.ProcGroupItemNum;
			}
			return Crud.ProcGroupItemCrud.Insert(procGroupItem);
		}

		///<summary>Deletes a ProcGroupItem based on its procGroupItemNum.</summary>
		public static void Delete(long procGroupItemNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),procGroupItemNum);
				return;
			}
			string command= "DELETE FROM procgroupitem WHERE ProcGroupItemNum = "+POut.Long(procGroupItemNum);
			Db.NonQ(command);
		}

		///<summary>Returns a count of the number of C, EC, and EO procedures attached to a group note.  Takes the ProcNum of a group note.
		///Used when deleting group notes to determine which permission to check.</summary>
		public static int GetCountCompletedProcsForGroup(long groupNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod(),groupNum);
			}
			string command = "SELECT COUNT(*) FROM procgroupitem "
				+"INNER JOIN procedurelog ON procedurelog.ProcNum=procgroupitem.ProcNum "
				+"AND procedurelog.ProcStatus IN ("+POut.Int((int)ProcStat.C)+", "+POut.Int((int)ProcStat.EO)+", "+POut.Int((int)ProcStat.EC)+") "
				+"WHERE GroupNum = "+POut.Long(groupNum);
			return PIn.Int(Db.GetCount(command));
		}

		/*
		#region CachePattern

		private class ProcGroupItemCache : CacheListAbs<ProcGroupItem> {
			protected override List<ProcGroupItem> GetCacheFromDb() {
				string command="SELECT * FROM ProcGroupItem ORDER BY ItemOrder";
				return Crud.ProcGroupItemCrud.SelectMany(command);
			}
			protected override List<ProcGroupItem> TableToList(DataTable table) {
				return Crud.ProcGroupItemCrud.TableToList(table);
			}
			protected override ProcGroupItem Copy(ProcGroupItem ProcGroupItem) {
				return ProcGroupItem.Clone();
			}
			protected override DataTable ListToTable(List<ProcGroupItem> listProcGroupItems) {
				return Crud.ProcGroupItemCrud.ListToTable(listProcGroupItems,"ProcGroupItem");
			}
			protected override void FillCacheIfNeeded() {
				ProcGroupItems.GetTableFromCache(false);
			}
			protected override bool IsInListShort(ProcGroupItem ProcGroupItem) {
				return !ProcGroupItem.IsHidden;
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static ProcGroupItemCache _ProcGroupItemCache=new ProcGroupItemCache();

		///<summary>A list of all ProcGroupItems. Returns a deep copy.</summary>
		public static List<ProcGroupItem> ListDeep {
			get {
				return _ProcGroupItemCache.ListDeep;
			}
		}

		///<summary>A list of all visible ProcGroupItems. Returns a deep copy.</summary>
		public static List<ProcGroupItem> ListShortDeep {
			get {
				return _ProcGroupItemCache.ListShortDeep;
			}
		}

		///<summary>A list of all ProcGroupItems. Returns a shallow copy.</summary>
		public static List<ProcGroupItem> ListShallow {
			get {
				return _ProcGroupItemCache.ListShallow;
			}
		}

		///<summary>A list of all visible ProcGroupItems. Returns a shallow copy.</summary>
		public static List<ProcGroupItem> ListShort {
			get {
				return _ProcGroupItemCache.ListShallowShort;
			}
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_appointmentTypeCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_appointmentTypeCache.FillCacheFromTable(table);
				return table;
			}
			return _appointmentTypeCache.GetTableFromCache(doRefreshCache);
		}

		#endregion
		*/

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		

		///<summary>Gets one ProcGroupItem from the db.</summary>
		public static ProcGroupItem GetOne(long procGroupItemNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<ProcGroupItem>(MethodBase.GetCurrentMethod(),procGroupItemNum);
			}
			return Crud.ProcGroupItemCrud.SelectOne(procGroupItemNum);
		}

		///<summary></summary>
		public static void Update(ProcGroupItem procGroupItem){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),procGroupItem);
				return;
			}
			Crud.ProcGroupItemCrud.Update(procGroupItem);
		}

		///<summary></summary>
		public static void Delete(long procGroupItemNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),procGroupItemNum);
				return;
			}
			string command= "DELETE FROM procgroupitem WHERE ProcGroupItemNum = "+POut.Long(procGroupItemNum);
			Db.NonQ(command);
		}
		*/



	}
}