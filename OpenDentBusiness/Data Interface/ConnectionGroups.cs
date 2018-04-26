using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ConnectionGroups{
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

		#region CachePattern

		private class ConnectionGroupCache : CacheListAbs<ConnectionGroup> {
			protected override List<ConnectionGroup> GetCacheFromDb() {
				string command="SELECT * FROM connectiongroup ORDER BY Description";
				return Crud.ConnectionGroupCrud.SelectMany(command);
			}
			protected override List<ConnectionGroup> TableToList(DataTable table) {
				return Crud.ConnectionGroupCrud.TableToList(table);
			}
			protected override ConnectionGroup Copy(ConnectionGroup connectionGroup) {
				return connectionGroup.Clone();
			}
			protected override DataTable ListToTable(List<ConnectionGroup> listConnectionGroups) {
				return Crud.ConnectionGroupCrud.ListToTable(listConnectionGroups,"ConnectionGroup");
			}
			protected override void FillCacheIfNeeded() {
				ConnectionGroups.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static ConnectionGroupCache _connectionGroupCache=new ConnectionGroupCache();

		public static List<ConnectionGroup> GetDeepCopy(bool isShort=false) {
			return _connectionGroupCache.GetDeepCopy(isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_connectionGroupCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_connectionGroupCache.FillCacheFromTable(table);
				return table;
			}
			return _connectionGroupCache.GetTableFromCache(doRefreshCache);
		}

		#endregion

		///<summary></summary>
		public static List<ConnectionGroup> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ConnectionGroup>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM connectiongroup ORDER BY Description";
			return Crud.ConnectionGroupCrud.SelectMany(command);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.</summary>
		public static void Sync(List<ConnectionGroup> listNew) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listNew);
				return;
			}
			ConnectionGroups.RefreshCache();
			Crud.ConnectionGroupCrud.Sync(listNew,ConnectionGroups.GetDeepCopy());
		}

		///<summary>Gets one ConnectionGroup from the db based on the ConnectionGroupNum.</summary>
		public static ConnectionGroup GetOne(long connectionGroupNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<ConnectionGroup>(MethodBase.GetCurrentMethod(),connectionGroupNum);
			}
			return Crud.ConnectionGroupCrud.SelectOne(connectionGroupNum);
		}

		///<summary>Gets ConnectionGroups based on description.</summary>
		public static List<ConnectionGroup> GetByDescription(string description) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ConnectionGroup>>(MethodBase.GetCurrentMethod(),description);
			}
			string command="SELECT * FROM connectiongroup WHERE Description LIKE '%"+POut.String(description)+"%'";
			return Crud.ConnectionGroupCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(ConnectionGroup connectionGroup){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				connectionGroup.ConnectionGroupNum=Meth.GetLong(MethodBase.GetCurrentMethod(),connectionGroup);
				return connectionGroup.ConnectionGroupNum;
			}
			return Crud.ConnectionGroupCrud.Insert(connectionGroup);
		}

		///<summary></summary>
		public static void Update(ConnectionGroup connectionGroup){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),connectionGroup);
				return;
			}
			Crud.ConnectionGroupCrud.Update(connectionGroup);
		}

		///<summary></summary>
		public static void Delete(long connectionGroupNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),connectionGroupNum);
				return;
			}
			string command= "DELETE FROM connectiongroup WHERE ConnectionGroupNum = "+POut.Long(connectionGroupNum);
			Db.NonQ(command);
		}
	}
}