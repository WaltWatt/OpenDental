using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class FeeScheds{
		#region Get Methods
		///<summary>Gets the fee sched from the first insplan, the patient, or the provider in that order.  Uses procProvNum if>0, otherwise pat.PriProv.
		///Either returns a fee schedule (fk to definition.DefNum) or 0.</summary>
		public static long GetFeeSched(Patient pat,List<InsPlan> planList,List<PatPlan> patPlans,List<InsSub> subList,long procProvNum) {
			//No need to check RemotingRole; no call to db.
			//there's not really a good place to put this function, so it's here.
			long priPlanFeeSched=0;
			PatPlan patPlanPri = patPlans.FirstOrDefault(x => x.Ordinal==1);
			if(patPlanPri!=null) {
				InsPlan planCur=InsPlans.GetPlan(InsSubs.GetSub(patPlanPri.InsSubNum,subList).PlanNum,planList);
				if(planCur!=null) {
					priPlanFeeSched=planCur.FeeSched;
				}
			}
			return GetFeeSched(priPlanFeeSched,pat.FeeSched,procProvNum!=0?procProvNum:pat.PriProv);//use procProvNum, but if 0 then default to pat.PriProv
		}

		///<summary>A simpler version of the same function above.  The required numbers can be obtained in a fairly simple query.
		///Might return a 0 if the primary provider does not have a fee schedule set.</summary>
		public static long GetFeeSched(long priPlanFeeSched,long patFeeSched,long provNum) {
			//No need to check RemotingRole; no call to db.
			long provFeeSched=(Providers.GetFirstOrDefault(x => x.ProvNum==provNum)??new Provider()).FeeSched;//defaults to 0
			return new[] { priPlanFeeSched,patFeeSched,provFeeSched }.FirstOrDefault(x => x>0);//defaults to 0 if all fee scheds are 0
		}

		///<summary>Gets the fee schedule from the primary MEDICAL insurance plan, 
		///the first insurance plan, the patient, or the provider in that order.</summary>
		public static long GetMedFeeSched(Patient pat,List<InsPlan> planList,List<PatPlan> patPlans,List<InsSub> subList,long procProvNum) {
			//No need to check RemotingRole; no call to db.
			long retVal = 0;
			if(PatPlans.GetInsSubNum(patPlans,1) != 0){
				//Pick the medinsplan with the ordinal closest to zero
				int planOrdinal=10; //This is a hack, but I doubt anyone would have more than 10 plans
				bool hasMedIns=false; //Keep track of whether we found a medical insurance plan, if not use dental insurance fee schedule.
				InsSub subCur;
				foreach(PatPlan patplan in patPlans){
					subCur=InsSubs.GetSub(patplan.InsSubNum,subList);
					if(patplan.Ordinal<planOrdinal && InsPlans.GetPlan(subCur.PlanNum,planList).IsMedical) {
						planOrdinal=patplan.Ordinal;
						hasMedIns=true;
					}
				}
				if(!hasMedIns) { //If this patient doesn't have medical insurance (under ordinal 10)
					return GetFeeSched(pat,planList,patPlans,subList,procProvNum);  //Use dental insurance fee schedule
				}
				subCur=InsSubs.GetSub(PatPlans.GetInsSubNum(patPlans,planOrdinal),subList);
				InsPlan PlanCur=InsPlans.GetPlan(subCur.PlanNum, planList);
				if (PlanCur==null){
					retVal=0;
				} 
				else {
					retVal=PlanCur.FeeSched;
				}
			}
			if (retVal==0){
				if (pat.FeeSched!=0){
					retVal=pat.FeeSched;
				} 
				else {
					if (pat.PriProv==0){
						retVal=Providers.GetFirst(true).FeeSched;
					} 
					else {
						Provider providerFirst=Providers.GetFirst();//Used in order to preserve old behavior...  If this fails, then old code would have failed.
						Provider provider=Providers.GetFirstOrDefault(x => x.ProvNum==pat.PriProv)??providerFirst;
						retVal=provider.FeeSched;
					}
				}
			}
			return retVal;
		}
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

		private class FeeSchedCache : CacheListAbs<FeeSched> {
			protected override List<FeeSched> GetCacheFromDb() {
				string command="SELECT * FROM feesched ORDER BY ItemOrder";
				return Crud.FeeSchedCrud.SelectMany(command);
			}
			protected override List<FeeSched> TableToList(DataTable table) {
				return Crud.FeeSchedCrud.TableToList(table);
			}
			protected override FeeSched Copy(FeeSched feeSched) {
				return feeSched.Copy();
			}
			protected override DataTable ListToTable(List<FeeSched> listFeeScheds) {
				return Crud.FeeSchedCrud.ListToTable(listFeeScheds,"FeeSched");
			}
			protected override void FillCacheIfNeeded() {
				FeeScheds.GetTableFromCache(false);
			}
			protected override bool IsInListShort(FeeSched feeSched) {
				return !feeSched.IsHidden;
			}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static FeeSchedCache _feeSchedCache=new FeeSchedCache();

		public static int GetCount(bool isShort=false) {
			return _feeSchedCache.GetCount(isShort);
		}

		public static List<FeeSched> GetDeepCopy(bool isShort=false) {
			return _feeSchedCache.GetDeepCopy(isShort);
		}

		public static FeeSched GetFirst(bool isShort=true) {
			return _feeSchedCache.GetFirst(isShort);
		}

		public static FeeSched GetFirst(Func<FeeSched,bool> match,bool isShort=true) {
			return _feeSchedCache.GetFirst(match,isShort);
		}

		public static FeeSched GetFirstOrDefault(Func<FeeSched,bool> match,bool isShort=false) {
			return _feeSchedCache.GetFirstOrDefault(match,isShort);
		}

		public static List<FeeSched> GetWhere(Predicate<FeeSched> match,bool isShort=false) {
			return _feeSchedCache.GetWhere(match,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_feeSchedCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_feeSchedCache.FillCacheFromTable(table);
				return table;
			}
			return _feeSchedCache.GetTableFromCache(doRefreshCache);
		}

		#endregion Cache Pattern

		///<summary></summary>
		public static long Insert(FeeSched feeSched) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				feeSched.SecUserNumEntry=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				feeSched.FeeSchedNum=Meth.GetLong(MethodBase.GetCurrentMethod(),feeSched);
				return feeSched.FeeSchedNum;
			}
			return Crud.FeeSchedCrud.Insert(feeSched);
		}

		///<summary></summary>
		public static void Update(FeeSched feeSched) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),feeSched);
				return;
			}
			Crud.FeeSchedCrud.Update(feeSched);
		}

		///<summary>Inserts, updates, or deletes database rows to match supplied list.  No need to pass in userNum, it's set before remoting role check
		///and passed to the server if necessary.</summary>
		public static bool Sync(List<FeeSched> listNew,List<FeeSched> listOld,long userNum=0) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				userNum=Security.CurUser.UserNum;
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),listNew,listOld,userNum);
			}
			return Crud.FeeSchedCrud.Sync(listNew,listOld,userNum);
		}

		///<summary>Returns the description of the fee schedule.  Appends (hidden) if the fee schedule has been hidden.</summary>
		public static string GetDescription(long feeSchedNum) {
			//No need to check RemotingRole; no call to db.
			string feeSchedDesc="";
			FeeSched feeSched=GetFirstOrDefault(x => x.FeeSchedNum==feeSchedNum);
			if(feeSched!=null) {
				feeSchedDesc=feeSched.Description+(feeSched.IsHidden ? " ("+Lans.g("FeeScheds","hidden")+")" : "");
			}
			return feeSchedDesc;
		}

		///<summary>Returns whether the FeeSched is hidden.  Defaults to true if not found.</summary>
		public static bool GetIsHidden(long feeSchedNum) {
			//No need to check RemotingRole; no call to db.
			FeeSched feeSched=GetFirstOrDefault(x => x.FeeSchedNum==feeSchedNum);
			return (feeSched==null ? true : feeSched.IsHidden);
		}

		///<summary>Returns whether the FeeSched has IsGlobal set to true.  Defaults to false if not found.</summary>
		public static bool IsGlobal(long feeSchedNum) {
			//No need to check RemotingRole; no call to db.
			FeeSched feeSched=GetFirstOrDefault(x => x.FeeSchedNum==feeSchedNum);
			return (feeSched==null ? false : feeSched.IsGlobal);
		}

		///<summary>Will return null if exact name not found.</summary>
		public static FeeSched GetByExactName(string description){
			//No need to check RemotingRole; no call to db.
			return GetFirstOrDefault(x => x.Description==description);
		}

		///<summary>Will return null if exact name not found.</summary>
		public static FeeSched GetByExactName(string description,FeeScheduleType feeSchedType){
			//No need to check RemotingRole; no call to db.
			return GetFirstOrDefault(x => x.FeeSchedType==feeSchedType && x.Description==description);
		}

		///<summary>Used to find FeeScheds of a certain type from within a given list.</summary>
		public static List<FeeSched> GetListForType(FeeScheduleType feeSchedType,bool includeHidden,List<FeeSched> listFeeScheds=null) {
			//No need to check RemotingRole; no call to db.
			listFeeScheds=listFeeScheds??GetDeepCopy();
			List<FeeSched> retVal=new List<FeeSched>();
			for(int i=0;i<listFeeScheds.Count;i++) {
				if(!includeHidden && listFeeScheds[i].IsHidden){
					continue;
				}
				if(listFeeScheds[i].FeeSchedType==feeSchedType){
					retVal.Add(listFeeScheds[i]);
				}
			}
			return retVal;
		}

		///<summary>Deletes FeeScheds that are hidden and not attached to any insurance plans.  Returns the number of deleted fee scheds.</summary>
		public static long CleanupAllowedScheds() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod());
			}
			long result;
			//Detach allowed FeeSchedules from any hidden InsPlans.
			string command="UPDATE insplan "
				+"SET AllowedFeeSched=0 "
				+"WHERE IsHidden=1";
			Db.NonQ(command);
			//Delete unattached FeeSchedules.
			command="DELETE FROM feesched "
				+"WHERE FeeSchedNum NOT IN (SELECT AllowedFeeSched FROM insplan) "
				+"AND FeeSchedType="+POut.Int((int)FeeScheduleType.OutNetwork);
			result=Db.NonQ(command);
			//Delete all orphaned fees.
			command="SELECT FeeNum FROM fee "
				+"WHERE FeeSched NOT IN (SELECT FeeSchedNum FROM feesched)";
			List<long> listFeeNums=Db.GetListLong(command);
			Fees.DeleteMany(listFeeNums);
			return result;
		}
	}
}