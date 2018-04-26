using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ProviderClinics{
		//If this table type will exist as cached data, uncomment the Cache Pattern region below and edit.
		/*
		#region Cache Pattern
		//This region can be eliminated if this is not a table type with cached data.
		//If leaving this region in place, be sure to add GetTableFromCache and FillCacheFromTable to the Cache.cs file with all the other Cache types.
		//Also, consider making an invalid type for this class in Cache.GetAllCachedInvalidTypes() if needed.

		private class ProviderClinicCache : CacheListAbs<ProviderClinic> {
			protected override List<ProviderClinic> GetCacheFromDb() {
				string command="SELECT * FROM providerclinic";
				return Crud.ProviderClinicCrud.SelectMany(command);
			}
			protected override List<ProviderClinic> TableToList(DataTable table) {
				return Crud.ProviderClinicCrud.TableToList(table);
			}
			protected override ProviderClinic Copy(ProviderClinic providerClinic) {
				return providerClinic.Copy();
			}
			protected override DataTable ListToTable(List<ProviderClinic> listProviderClinics) {
				return Crud.ProviderClinicCrud.ListToTable(listProviderClinics,"ProviderClinic");
			}
			protected override void FillCacheIfNeeded() {
				ProviderClinics.GetTableFromCache(false);
			}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static ProviderClinicCache _providerClinicCache=new ProviderClinicCache();

		public static List<ProviderClinic> GetDeepCopy(bool isShort=false) {
			return _providerClinicCache.GetDeepCopy(isShort);
		}

		public static int GetCount(bool isShort=false) {
			return _providerClinicCache.GetCount(isShort);
		}

		public static bool GetExists(Predicate<ProviderClinic> match,bool isShort=false) {
			return _providerClinicCache.GetExists(match,isShort);
		}

		public static int GetFindIndex(Predicate<ProviderClinic> match,bool isShort=false) {
			return _providerClinicCache.GetFindIndex(match,isShort);
		}

		public static ProviderClinic GetFirst(bool isShort=false) {
			return _providerClinicCache.GetFirst(isShort);
		}

		public static ProviderClinic GetFirst(Func<ProviderClinic,bool> match,bool isShort=false) {
			return _providerClinicCache.GetFirst(match,isShort);
		}

		public static ProviderClinic GetFirstOrDefault(Func<ProviderClinic,bool> match,bool isShort=false) {
			return _providerClinicCache.GetFirstOrDefault(match,isShort);
		}

		public static ProviderClinic GetLast(bool isShort=false) {
			return _providerClinicCache.GetLast(isShort);
		}

		public static ProviderClinic GetLastOrDefault(Func<ProviderClinic,bool> match,bool isShort=false) {
			return _providerClinicCache.GetLastOrDefault(match,isShort);
		}

		public static List<ProviderClinic> GetWhere(Predicate<ProviderClinic> match,bool isShort=false) {
			return _providerClinicCache.GetWhere(match,isShort);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_providerClinicCache.FillCacheFromTable(table);
		}

		///<summary>Returns the cache in the form of a DataTable. Always refreshes the ClientWeb's cache.</summary>
		///<param name="doRefreshCache">If true, will refresh the cache if RemotingRole is ClientDirect or ServerWeb.</param> 
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_providerClinicCache.FillCacheFromTable(table);
				return table;
			}
			return _providerClinicCache.GetTableFromCache(doRefreshCache);
		}
		#endregion Cache Pattern
		*/
		#region Get Methods
		///<summary>Gets one ProviderClinic from the db. Can be null.</summary>
		public static ProviderClinic GetOne(long providerClinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<ProviderClinic>(MethodBase.GetCurrentMethod(),providerClinicNum);
			}
			return Crud.ProviderClinicCrud.SelectOne(providerClinicNum);
		}

		///<summary>Gets one ProviderClinic from the db. Can be null.</summary>
		public static ProviderClinic GetOne(long provNum, long clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<ProviderClinic>(MethodBase.GetCurrentMethod(),provNum,clinicNum);
			}
			string command="SELECT * FROM providerclinic WHERE ProvNum = "+POut.Long(provNum)+" AND ClinicNum = "+POut.Long(clinicNum);
			return Crud.ProviderClinicCrud.SelectOne(command);
		}

		///<summary>Gets one DEANum from the db. If the DEANum for the specified clinic is not set, will return the default DEANum(clinicNum=0). Returns empty string if none set.</summary>
		public static string GetDEANum(long provNum,long clinicNum=0) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<string>(MethodBase.GetCurrentMethod(),provNum,clinicNum);
			}
			string command="SELECT DEANum FROM providerclinic WHERE ProvNum = "+POut.Long(provNum)+" AND ClinicNum = "+POut.Long(clinicNum);
			string retVal=Db.GetScalar(command);
			if(clinicNum!=0 && string.IsNullOrWhiteSpace(retVal)) {
				retVal=GetDEANum(provNum);
			}
			return retVal;
		}

		///<summary>Gets a list of ProviderClinics from the db.</summary>
		public static List<ProviderClinic> GetListForProvider(long provNum,List<long> listClinicNums=null) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ProviderClinic>>(MethodBase.GetCurrentMethod(),provNum,listClinicNums);
			}
			string command="SELECT * FROM providerclinic WHERE ProvNum = "+POut.Long(provNum);
			if(listClinicNums!=null && listClinicNums.Count>0) {
				command+=" AND ClinicNum IN("+String.Join(", ",listClinicNums)+") ";
			}
			return Crud.ProviderClinicCrud.SelectMany(command);
		}
		#endregion

		///<summary>Inserts, updates, or deletes db rows to match listNew.  No need to pass in userNum, it's set before remoting role check and passed to
		///the server if necessary.  Doesn't create ApptComm items, but will delete them.  If you use Sync, you must create new AlertCategories items.</summary>
		public static bool Sync(List<ProviderClinic> listNew,List<ProviderClinic> listOld) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),listNew,listOld);
			}
			return Crud.ProviderClinicCrud.Sync(listNew,listOld);
		}
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.
		#region Get Methods
		///<summary></summary>
		public static List<ProviderClinic> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ProviderClinic>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM providerclinic WHERE PatNum = "+POut.Long(patNum);
			return Crud.ProviderClinicCrud.SelectMany(command);
		}
		
		
		#endregion
		#region Modification Methods
			#region Insert
		///<summary></summary>
		public static long Insert(ProviderClinic providerClinic){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				providerClinic.ProviderClinicNum=Meth.GetLong(MethodBase.GetCurrentMethod(),providerClinic);
				return providerClinic.ProviderClinicNum;
			}
			return Crud.ProviderClinicCrud.Insert(providerClinic);
		}
			#endregion
			#region Update
		///<summary></summary>
		public static void Update(ProviderClinic providerClinic){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),providerClinic);
				return;
			}
			Crud.ProviderClinicCrud.Update(providerClinic);
		}
			#endregion
			#region Delete
		///<summary></summary>
		public static void Delete(long providerClinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),providerClinicNum);
				return;
			}
			Crud.ProviderClinicCrud.Delete(providerClinicNum);
		}
			#endregion
		#endregion
		#region Misc Methods
		

		
		#endregion
		*/



	}
}