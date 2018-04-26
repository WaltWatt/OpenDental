using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ProcButtonQuicks{
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

		private class ProcButtonQuickCache : CacheListAbs<ProcButtonQuick> {
			protected override List<ProcButtonQuick> GetCacheFromDb() {
				string command="SELECT * FROM ProcButtonQuick ORDER BY ItemOrder";
				return Crud.ProcButtonQuickCrud.SelectMany(command);
			}
			protected override List<ProcButtonQuick> TableToList(DataTable table) {
				return Crud.ProcButtonQuickCrud.TableToList(table);
			}
			protected override ProcButtonQuick Copy(ProcButtonQuick ProcButtonQuick) {
				return ProcButtonQuick.Clone();
			}
			protected override DataTable ListToTable(List<ProcButtonQuick> listProcButtonQuicks) {
				return Crud.ProcButtonQuickCrud.ListToTable(listProcButtonQuicks,"ProcButtonQuick");
			}
			protected override void FillCacheIfNeeded() {
				ProcButtonQuicks.GetTableFromCache(false);
			}
			protected override bool IsInListShort(ProcButtonQuick ProcButtonQuick) {
				return !ProcButtonQuick.IsHidden;
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static ProcButtonQuickCache _ProcButtonQuickCache=new ProcButtonQuickCache();

		///<summary>A list of all ProcButtonQuicks. Returns a deep copy.</summary>
		public static List<ProcButtonQuick> ListDeep {
			get {
				return _ProcButtonQuickCache.ListDeep;
			}
		}

		///<summary>A list of all visible ProcButtonQuicks. Returns a deep copy.</summary>
		public static List<ProcButtonQuick> ListShortDeep {
			get {
				return _ProcButtonQuickCache.ListShortDeep;
			}
		}

		///<summary>A list of all ProcButtonQuicks. Returns a shallow copy.</summary>
		public static List<ProcButtonQuick> ListShallow {
			get {
				return _ProcButtonQuickCache.ListShallow;
			}
		}

		///<summary>A list of all visible ProcButtonQuicks. Returns a shallow copy.</summary>
		public static List<ProcButtonQuick> ListShort {
			get {
				return _ProcButtonQuickCache.ListShallowShort;
			}
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_ProcButtonQuickCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_ProcButtonQuickCache.FillCacheFromTable(table);
				return table;
			}
			return _ProcButtonQuickCache.GetTableFromCache(doRefreshCache);
		}

		#endregion
		*/

		///<summary></summary>
		public static List<ProcButtonQuick> GetAll(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ProcButtonQuick>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM procbuttonquick";
			return Crud.ProcButtonQuickCrud.SelectMany(command);
		}

		///<summary>Sort by Y values first, then sort by X values.</summary>
		public static int sortYX(ProcButtonQuick p1,ProcButtonQuick p2) {
			//#error Move this to the S class once it is generated.
			if(p1.YPos!=p2.YPos) {
				return p1.YPos.CompareTo(p2.YPos);
			}
			return p1.ItemOrder.CompareTo(p2.ItemOrder);
		}

		///<summary></summary>
		public static long Insert(ProcButtonQuick procButtonQuick){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				procButtonQuick.ProcButtonQuickNum=Meth.GetLong(MethodBase.GetCurrentMethod(),procButtonQuick);
				return procButtonQuick.ProcButtonQuickNum;
			}
			return Crud.ProcButtonQuickCrud.Insert(procButtonQuick);
		}

		///<summary></summary>
		public static void Update(ProcButtonQuick procButtonQuick){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),procButtonQuick);
				return;
			}
			Crud.ProcButtonQuickCrud.Update(procButtonQuick);
		}

		///<summary>Ensures that Quick Buttons category exists in DB, and validates all Quick buttons in the DB. 
		///Returns false if there is something wrong with ProcButtonQuick table. (Similar to DB maint.)</summary>
		public static bool ValidateAll() {


			return true;
		}

		///<summary></summary>
		public static void Delete(long procButtonQuickNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),procButtonQuickNum);
				return;
			}
			string command= "DELETE FROM procbuttonquick WHERE ProcButtonQuickNum = "+POut.Long(procButtonQuickNum);
			Db.NonQ(command);
		}

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<ProcButtonQuick> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ProcButtonQuick>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM procbuttonquick WHERE PatNum = "+POut.Long(patNum);
			return Crud.ProcButtonQuickCrud.SelectMany(command);
		}

		///<summary>Gets one ProcButtonQuick from the db.</summary>
		public static ProcButtonQuick GetOne(long procButtonQuickNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<ProcButtonQuick>(MethodBase.GetCurrentMethod(),procButtonQuickNum);
			}
			return Crud.ProcButtonQuickCrud.SelectOne(procButtonQuickNum);
		}
		*/




	}
}