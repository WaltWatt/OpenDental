using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ClinicPrefs{
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

		private class ClinicPrefCache : CacheListAbs<ClinicPref> {
			protected override List<ClinicPref> GetCacheFromDb() {
				string command="SELECT * FROM clinicpref";
				return Crud.ClinicPrefCrud.SelectMany(command);
			}
			protected override List<ClinicPref> TableToList(DataTable table) {
				return Crud.ClinicPrefCrud.TableToList(table);
			}
			protected override ClinicPref Copy(ClinicPref clinicPref) {
				return clinicPref.Clone();
			}
			protected override DataTable ListToTable(List<ClinicPref> listClinicPrefs) {
				return Crud.ClinicPrefCrud.ListToTable(listClinicPrefs,"ClinicPref");
			}
			protected override void FillCacheIfNeeded() {
				ClinicPrefs.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static ClinicPrefCache _clinicPrefCache=new ClinicPrefCache();

		public static List<ClinicPref> GetWhere(Predicate<ClinicPref> match,bool isShort=false) {
			return _clinicPrefCache.GetWhere(match,isShort);
		}

		private static ClinicPref GetFirstOrDefault(Func<ClinicPref,bool> match,bool isShort=false) {
			return _clinicPrefCache.GetFirstOrDefault(match,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_clinicPrefCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_clinicPrefCache.FillCacheFromTable(table);
				return table;
			}
			return _clinicPrefCache.GetTableFromCache(doRefreshCache);
		}

		#endregion Cache Pattern

		///<summary>Gets one ClinicPref from the db.</summary>
		public static ClinicPref GetOne(long clinicPrefNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<ClinicPref>(MethodBase.GetCurrentMethod(),clinicPrefNum);
			}
			return Crud.ClinicPrefCrud.SelectOne(clinicPrefNum);
		}

		///<summary></summary>
		public static long Insert(ClinicPref clinicPref){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				clinicPref.ClinicPrefNum=Meth.GetLong(MethodBase.GetCurrentMethod(),clinicPref);
				return clinicPref.ClinicPrefNum;
			}
			return Crud.ClinicPrefCrud.Insert(clinicPref);
		}

		///<summary></summary>
		public static void Update(ClinicPref clinicPref){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),clinicPref);
				return;
			}
			Crud.ClinicPrefCrud.Update(clinicPref);
		}

		public static void Update(ClinicPref newPref, ClinicPref oldPref) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),newPref,oldPref);
				return;
			}
			Crud.ClinicPrefCrud.Update(newPref,oldPref);
		}

		///<summary></summary>
		public static void Delete(long clinicPrefNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),clinicPrefNum);
				return;
			}
			Crud.ClinicPrefCrud.Delete(clinicPrefNum);
		}

		///<summary>Inserts, updates, or deletes db rows to match listNew.  No need to pass in userNum, it's set before remoting role check and passed to
		///the server if necessary.  Doesn't create ApptComm items, but will delete them.  If you use Sync, you must create new Apptcomm items.</summary>
		public static bool Sync(List<ClinicPref> listNew,List<ClinicPref> listOld) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),listNew,listOld);
			}
			return Crud.ClinicPrefCrud.Sync(listNew,listOld);
		}

		///<summary></summary>
		public static List<ClinicPref> GetAllPrefs(long clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClinicPref>>(MethodBase.GetCurrentMethod(),clinicNum);
			}
			string command="SELECT * FROM clinicpref WHERE ClinicNum="+POut.Long(clinicNum);
			return Crud.ClinicPrefCrud.SelectMany(command);
		}

		///<summary></summary>
		public static List<ClinicPref> GetPrefAllClinics(PrefName pref) {
			//No need to check RemotingRole; no call to db.
			return GetWhere(x => x.PrefName==pref);
		}

		///<summary></summary>
		public static ClinicPref GetPref(PrefName pref,long clinicNum) {
			//No need to check RemotingRole; no call to db.
			return GetPrefAllClinics(pref).FirstOrDefault(x => x.ClinicNum==clinicNum);
		}

		///<summary>Gets the ValueString for this clinic's pref or gets the actual preference if it does not exist.</summary>
		public static string GetPrefValue(PrefName pref,long clinicNum) {
			//No need to check RemotingRole; no call to db.
			ClinicPref clinicPref=GetPrefAllClinics(pref).FirstOrDefault(x => x.ClinicNum==clinicNum);
			if(clinicPref==null) {
				return PrefC.GetString(pref);
			}
			return clinicPref.ValueString;
		}

		///<summary>Returns 0 if there is no clinicpref entry for the specified pref.</summary>
		public static long GetLong(PrefName prefName,long clinicNum) {
			//No need to check RemotingRole; no call to db.
			ClinicPref pref=GetPref(prefName,clinicNum);
			if(pref==null) {
				return 0;
			}
			return PIn.Long(pref.ValueString);
		}


		///<summary>Gets the ValueString as a boolean for this clinic's pref or gets the actual preference if it does not exist.</summary>
		public static bool GetBool(PrefName prefName,long clinicNum) {
			ClinicPref pref=GetPref(prefName,clinicNum);
			if(pref==null) {
				return PrefC.GetBool(prefName);
			}
			return PIn.Bool(pref.ValueString);
		}

		///<summary>Inserts a pref of type long for the specified clinic.  Throws an exception if the preference already exists.</summary>
		public static void InsertPref(PrefName prefName,long clinicNum,string valueAsString) {
			//No need to check RemotingRole; no call to db.
			if(GetFirstOrDefault(x => x.ClinicNum==clinicNum && x.PrefName==prefName)!=null) {
				throw new ApplicationException("The PrefName "+prefName+" already exists for ClinicNum: "+clinicNum);
			}
			ClinicPref clinicPrefToInsert=new ClinicPref();
			clinicPrefToInsert.PrefName=prefName;
			clinicPrefToInsert.ValueString=valueAsString;
			clinicPrefToInsert.ClinicNum=clinicNum;
			Insert(clinicPrefToInsert);
		}

		///<summary>Updates a pref of type long for the specified clinic.  Returns true if a change was required, or false if no change needed.</summary>
		public static bool UpdateLong(PrefName prefName,long clinicNum,long newValue) {
			//Very unusual.  Involves cache, so Meth is used further down instead of here at the top.
			ClinicPref clinicPref=GetFirstOrDefault(x => x.ClinicNum==clinicNum && x.PrefName==prefName);
			if(clinicPref==null) {
				throw new ApplicationException("The PrefName "+prefName+" does not exist for ClinicNum: "+clinicNum);
			}
			if(PIn.Long(clinicPref.ValueString)==newValue) {
				return false;//no change needed
			}
			string command="UPDATE clinicpref SET ValueString='"+POut.Long(newValue)+"' "
				+"WHERE PrefName='"+POut.String(prefName.ToString())+"' "
				+"AND ClinicNum='"+POut.Long(clinicNum)+"'";
			bool retVal=true;
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				retVal=Meth.GetBool(MethodBase.GetCurrentMethod(),prefName,newValue);
			}
			else {
				Db.NonQ(command);
			}
			//Update local cache even though we should be invalidating the cache outside of this method.
			ClinicPref cachedClinicPref=clinicPref;
			cachedClinicPref.PrefName=prefName;
			cachedClinicPref.ValueString=newValue.ToString();
			cachedClinicPref.ClinicNum=clinicNum;
			return retVal;
		}

		///<summary>Inserts a new clinic pref or updates the existing one.</summary>
		///<returns>True if an insert or update was made, false otherwise.</returns>
		public static bool Upsert(PrefName pref,long clinicNum,string newValue) {
			//No need to check RemotingRole; no call to db.
			ClinicPref clinicPref=GetPref(pref,clinicNum);
			if(clinicPref==null) {
				InsertPref(pref,clinicNum,newValue);
				return true;
			}
			if(clinicPref.ValueString==newValue) {
				return false;
			}
			clinicPref.ValueString=newValue;
			Update(clinicPref);
			return true;
		}

		///<summary>Deletes the prefs for this clinic. If any pref does not exist, then nothing will be done with that pref.</summary>
		public static void DeletePrefs(long clinicNum,List<PrefName> listPrefs) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),clinicNum,listPrefs);
				return;
			}
			List<ClinicPref> listClinicPrefs=new List<ClinicPref>();
			foreach(PrefName pref in listPrefs) {
				ClinicPref clinicPref=GetPref(pref,clinicNum);
				if(clinicPref!=null) {
					listClinicPrefs.Add(clinicPref);
				}
			}
			if(listClinicPrefs.Count==0) {
				return;
			}
			string command="DELETE FROM clinicpref WHERE ClinicPrefNum IN("+string.Join(",",listClinicPrefs.Select(x => x.ClinicPrefNum))+")";
			Db.NonQ(command);
		}

	}
}