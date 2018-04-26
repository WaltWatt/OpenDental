using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OpenDentBusiness{
	///<summary></summary>
	public class CanadianNetworks{

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

		#region Cache Pattern

		private class CanadianNetworkCache : CacheListAbs<CanadianNetwork> {
			protected override List<CanadianNetwork> GetCacheFromDb() {
				string command="SELECT * FROM canadiannetwork ORDER BY Descript";
				return Crud.CanadianNetworkCrud.SelectMany(command);
			}
			protected override List<CanadianNetwork> TableToList(DataTable table) {
				return Crud.CanadianNetworkCrud.TableToList(table);
			}
			protected override CanadianNetwork Copy(CanadianNetwork canadianNetwork) {
				return canadianNetwork.Copy();
			}
			protected override DataTable ListToTable(List<CanadianNetwork> listCanadianNetworks) {
				return Crud.CanadianNetworkCrud.ListToTable(listCanadianNetworks,"CanadianNetwork");
			}
			protected override void FillCacheIfNeeded() {
				CanadianNetworks.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static CanadianNetworkCache _canadianNetworkCache=new CanadianNetworkCache();

		public static List<CanadianNetwork> GetDeepCopy(bool isShort=false) {
			return _canadianNetworkCache.GetDeepCopy(isShort);
		}

		private static CanadianNetwork GetFirstOfDefault(Func<CanadianNetwork,bool> match,bool isShort=false) {
			return _canadianNetworkCache.GetFirstOrDefault(match,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_canadianNetworkCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_canadianNetworkCache.FillCacheFromTable(table);
				return table;
			}
			return _canadianNetworkCache.GetTableFromCache(doRefreshCache);
		}

		#endregion Cache Pattern

		///<summary></summary>
		public static long Insert(CanadianNetwork canadianNetwork) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				canadianNetwork.CanadianNetworkNum=Meth.GetLong(MethodBase.GetCurrentMethod(),canadianNetwork);
				return canadianNetwork.CanadianNetworkNum;
			}
			return Crud.CanadianNetworkCrud.Insert(canadianNetwork);
		}

		///<summary></summary>
		public static void Update(CanadianNetwork canadianNetwork){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),canadianNetwork);
				return;
			}
			Crud.CanadianNetworkCrud.Update(canadianNetwork);
		}

		///<summary></summary>
		public static void Delete(int networkNum){

		}

		///<summary></summary>
		public static CanadianNetwork GetNetwork(long networkNum) {
			//No need to check RemotingRole; no call to db.
			return GetFirstOfDefault(x => x.CanadianNetworkNum==networkNum);
		}
	}
}













