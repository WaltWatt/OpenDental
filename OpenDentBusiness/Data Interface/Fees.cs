using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using CodeBase;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Fees {
		///<summary>When the fee cache is going to be filled for the first time by a thread, 
		///make everyone wait until _cache has been filled the first time.</summary>
		public static bool IsFilledByThread=false;
		///<summary>Access _Cache instead. This is a unique cache class used for caching and manipulating fees.</summary>
		private static IFeeCache _cache;

		///<summary>This is a very unique cache class. Not generally available for use, instead either get a copy of the cache for
		///local use or use some of the functions in the S class.</summary>
		private static IFeeCache _Cache {
			get	{
				FillCacheOrWait();
				return _cache;
			}
			set	{
				_cache=value;
			}
		}

		///<summary>If the cache has not been filled, waits for the cache to fill if it is being filled by another thread, 
		///otherwise fills the cache itself.</summary>
		private static void FillCacheOrWait() {
			//No need to check RemotingRole; no call to db.
			if(_cache!=null) {
				return;
			}
			if(IsFilledByThread) {
				//The fee cache is special in the fact that we fill it for the first time within a thread that was spawned via FormOpenDental.
				//All other threads that want a copy of the fee cache need to sit here and wait until the thread has filled it for the first time.
				int loopcount=0;
				while(_cache==null) {
					loopcount++;
					if(loopcount>6000) {//~a minute, plus the time it takes to run this small while loop 6000 times.
						throw new Exception("Unable to fill fee cache.");
					}
					Thread.Sleep(10);
				}
			}
			else {//Fill the fee cache on the first time that the fee cache is being requested (old logic).
						//This was too slow for larger offices so we had to introduce IsFilledByThread so that this cache can be filled behind the scenes.
				FillCache();
			}
		}

		///<summary>Initializes the Cache, with fees for the HQ Clinic, and for the current user's selected clinic.</summary>
		public static void FillCache() {
			//No need to check RemotingRole; no call to db.
			IFeeCache cache;
			if(RemotingClient.RemotingRole==RemotingRole.ServerWeb) {
				if(PrefC.GetBool(PrefName.MiddleTierCacheFees)) {
					cache=new FeeCache(numClinics: Int32.MaxValue,doInitialize: true);//No limit to the number of clinics stored
				}
				else {
					cache=new FeeNoCache();
				}
			}
			else {
				cache=new FeeCache();
			}
			_Cache=cache;
		}

		///<summary>Fills the cache with the passed in DataTable. Note that this might push out fees from other clinics from the cache.</summary>
		public static void FillCacheFromTable(DataTable dataTable) {
			_Cache.FillCacheFromTable(dataTable);
		}

		#region Get Methods
		///<summary>Gets the list of fees by clinic num from the db.</summary>
		public static List<Fee> GetByClinicNum(long clinicNum) {
			return GetByClinicNums(new List<long>() { clinicNum });
		}

		///<summary>Gets the list of fees by clinic nums from the db.</summary>
		public static List<Fee> GetByClinicNums(List<long> listClinicNums) {
			if(listClinicNums.Count==0) {
				return new List<Fee>();
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Fee>>(MethodBase.GetCurrentMethod(),listClinicNums);
			}
			string command="SELECT * FROM fee WHERE ClinicNum IN ("+string.Join(",",listClinicNums.Select(x => POut.Long(x)))+")";
			return Crud.FeeCrud.SelectMany(command);
		}
		
		///<summary>Gets the list of fees by feeschednums and clinicnums from the db.</summary>
		public static List<Fee> GetByFeeSchedNumsClinicNums(List<long> listFeeSchedNums,List<long> listClinicNums) {
			if(listFeeSchedNums.Count==0 || listClinicNums.Count==0) {
				return new List<Fee>();
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Fee>>(MethodBase.GetCurrentMethod(),listFeeSchedNums,listClinicNums);
			}
			string command="SELECT * FROM fee WHERE FeeSched IN ("+string.Join(",",listFeeSchedNums.Select(x => POut.Long(x)))+") "
																			+"AND ClinicNum IN ("+string.Join(",",listClinicNums.Select(x => POut.Long(x)))+")";
			return Crud.FeeCrud.SelectMany(command);
		}

		///<summary>Counts the number of fees in the db for this fee sched.</summary>
		public static int GetCountByFeeSchedNum(long feeSchedNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod(),feeSchedNum);
			}
			string command="SELECT COUNT(*) FROM fee WHERE FeeSched ="+POut.Long(feeSchedNum);
			return PIn.Int(Db.GetCount(command));
		}

		///<summary>Searches the cache for the given codeNum and feeSchedNum and finds the most appropriate match for the clinicNum and provNum.</summary>
		public static Fee GetFee(long codeNum,long feeSchedNum,long clinicNum,long provNum) {
			//No need to check RemotingRole; no call to db.
			return _Cache.GetFee(codeNum,feeSchedNum,clinicNum,provNum);
		}

		///<summary>Gets a copy of the cache for local use.</summary>
		public static FeeCache GetCache() {
			return (_Cache.GetCopy() as FeeCache)??new FeeCache(doInitialize: false);//We need to coalesce to a new FeeCache in case GetCopy() doesn't 
																																							 //return a FeeCache.
		}

		///<summary>Returns all fees associated to the procedure code passed in.</summary>
		public static List<Fee> GetFeesForCode(long codeNum,long clinicNum=0) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Fee>>(MethodBase.GetCurrentMethod(),codeNum,clinicNum);
			}
			string command="SELECT * FROM fee WHERE CodeNum="+POut.Long(codeNum);
			return Crud.FeeCrud.SelectMany(command);
		}

		///<summary>Returns an amount if a fee has been entered.  Prefers local clinic fees over HQ fees.  Otherwise returns -1.
		///Not usually used directly.</summary>
		public static double GetAmount(long codeNum,long feeSchedNum,long clinicNum,long provNum) {
			//No need to check RemotingRole; no call to db.
			if(FeeScheds.GetIsHidden(feeSchedNum)) {
				return -1;//you cannot obtain fees for hidden fee schedules
			}
			Fee fee=GetFee(codeNum,feeSchedNum,clinicNum,provNum);
			if(fee==null) {
				return -1;
			}
			return fee.Amount;
		}

		///<summary>Almost the same as GetAmount.  But never returns -1;  Returns an amount if a fee has been entered.  
		///Prefers local clinic fees over HQ fees.  
		///Returns 0 if code can't be found.
		///Uses the list of fees passed in instead of the cached list of fees.</summary>
		public static double GetAmount0(long codeNum,long feeSched,long clinicNum=0,long provNum=0) {
			//No need to check RemotingRole; no call to db.
			double retVal=GetAmount(codeNum,feeSched,clinicNum,provNum);
			if(retVal==-1) {
				return 0;
			}
			return retVal;
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_Cache.FillCacheFromTable(table);
				return table;
			}
			return _Cache.GetTableFromCache(doRefreshCache);
		}

		///<summary>Gets the UCR fee for the provided procedure.</summary>
		public static double GetFeeUCR(Procedure proc) {
			//No need to check RemotingRole; no call to db.
			long provNum=proc.ProvNum;
			if(provNum==0) {//if no prov set, then use practice default.
				provNum=PrefC.GetLong(PrefName.PracticeDefaultProv);
			}
			int qty=proc.UnitQty + proc.BaseUnits;
			if(qty==0) {
				qty=1;
			}
			Provider providerFirst=Providers.GetFirst();//Used in order to preserve old behavior...  If this fails, then old code would have failed.
			Provider provider=Providers.GetFirstOrDefault(x => x.ProvNum==provNum)??providerFirst;
			//get the fee based on code and prov fee sched
			double ppoFee=Fees.GetAmount0(proc.CodeNum,provider.FeeSched,proc.ClinicNum,provNum);
			double ucrFee=proc.ProcFee;
			if(ucrFee > ppoFee) {
				return qty * ucrFee;
			}
			else {
				return qty * ppoFee;
			}
		}
		#endregion Get Methods

		#region Modification Methods

		#region Insert
		///<summary></summary>
		public static long Insert(Fee fee) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				fee.SecUserNumEntry=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				fee.FeeNum=Meth.GetLong(MethodBase.GetCurrentMethod(),fee);
				return fee.FeeNum;
			}
			return Crud.FeeCrud.Insert(fee);
		}

		/// <summary>Bulk Insert</summary>
		public static void InsertMany(List<Fee> listFees) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				listFees.ForEach(x => x.SecUserNumEntry=Security.CurUser.UserNum);//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listFees);
				return;
			}
			Crud.FeeCrud.InsertMany(listFees);
		}
		#endregion Insert

		#region Update
		///<summary></summary>
		public static void Update(Fee fee){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),fee);
				return;
			}
			Crud.FeeCrud.Update(fee);
		}

		///<summary>Commit changes logged during Cache.BeginTransaction.</summary>
		public static List<long> UpdateFromCache(List<FeeUpdate> listFeeUpdates) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),listFeeUpdates);
			}
			long feeNum=0;
			List<long> listFeeScheds=new List<long>();
			List<Fee> insertedFees=new List<Fee>();
			//We need to go through each fee in the order that they were added to the list in case the same fee is in the list multiple times.
			foreach(FeeUpdate update in listFeeUpdates) {
				try {
					switch(update.UpdateType) {
						case FeeUpdateType.Add:
							feeNum=Insert(update.Fee);
							update.Fee.FeeNum=feeNum; //We need to keep track of the feeNum in case there is a change to this fee later in the transaction
							insertedFees.Add(update.Fee);
							break;
						case FeeUpdateType.Update:
							if(update.Fee.FeeNum==0) { //this is an update for a fee that was added in this same transaction
								update.Fee.FeeNum=insertedFees.Where(x => x.FeeSched==update.Fee.FeeSched && x.ClinicNum==update.Fee.ClinicNum
									&& x.CodeNum==update.Fee.CodeNum && x.ProvNum==update.Fee.ProvNum).Select(x => x.FeeNum).LastOrDefault();
							}
							feeNum=update.Fee.FeeNum;
							Update(update.Fee);
							break;
						case FeeUpdateType.Remove:
							if(update.Fee.FeeNum==0) { //this is a delete for a fee that was added in this same transaction
								update.Fee.FeeNum=insertedFees.Where(x => x.FeeSched==update.Fee.FeeSched && x.ClinicNum==update.Fee.ClinicNum
									&& x.CodeNum==update.Fee.CodeNum && x.ProvNum==update.Fee.ProvNum).Select(x => x.FeeNum).LastOrDefault();
							}
							feeNum=update.Fee.FeeNum;
							Delete(update.Fee);
							break;
						default:
							break;
					}
					if(!listFeeScheds.Contains(update.Fee.FeeSched)) {
						listFeeScheds.Add(update.Fee.FeeSched);
					}
				}
				catch (Exception e) {
					throw new Exception("An error occurred "+update.UpdateType+"ing Fee "+feeNum +": "+e.Message, e);
				}
			}
			return listFeeScheds;
		}
		#endregion Update

		#region Delete
		///<summary></summary>
		public static void Delete(Fee fee){
			//No need to check RemotingRole; no call to db.
			Delete(fee.FeeNum);
		}

		///<summary></summary>
		public static void Delete(long feeNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),feeNum);
				return;
			}
			ClearFkey(feeNum);
			string command="DELETE FROM fee WHERE FeeNum="+feeNum;
			Db.NonQ(command);
		}

		///<summary></summary>
		public static void DeleteMany(List<long> listFeeNums) {
			if(listFeeNums.Count==0) {
				return;
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listFeeNums);
				return;
			}
			ClearFkey(listFeeNums);
			string command="DELETE FROM fee WHERE FeeNum IN ("+string.Join(",",listFeeNums)+")";
			Db.NonQ(command);
		}

		///<summary>Deletes all fees for the supplied FeeSched that aren't for the HQ clinic.</summary>
		public static void DeleteNonHQFeesForSched(long feeSchedNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),feeSchedNum);
				return;
			}
			string command="SELECT FeeNum FROM fee WHERE FeeSched="+POut.Long(feeSchedNum)+" AND ClinicNum!=0";
			List<long> listFeeNums=Db.GetListLong(command);
			DeleteMany(listFeeNums);
		}
		#endregion Delete

		#endregion Modification Methods

		#region Misc Methods
		///<summary></summary>
		public static void InvalidateFeeSchedules(List<long> listFeeScheduleNums) {
			Logger.LogAction("",LogPath.Signals,() => FillCacheOrWait());
			//Using _cache instead of _Cache because we are changing the internal dictionary.
			//if we add a preference to remove lazy loading, it would put a refreshcache call right here.
			listFeeScheduleNums.ForEach(x => _cache.Invalidate(x));
		}

		public static void InvalidateFeeSchedule(long feeScheduleNum) {
			InvalidateFeeSchedules(new List<long> { feeScheduleNum });
		}

		///<summary>Increases the fee schedule by percent.  Round should be the number of decimal places, either 0,1,or 2.
		///Returns listFees back after increasing the fees from the passed in fee schedule information.</summary>
		public static List<Fee> Increase(long feeSchedNum,int percent,int round,List<Fee> listFees,long clinicNum,long provNum) {
			//No need to check RemotingRole; no call to db.
			FeeSched feeSched=FeeScheds.GetFirst(x => x.FeeSchedNum==feeSchedNum);
			List<long> listCodeNums=new List<long>(); //Contains only the fee codeNums that have been increased.  Used for keeping track.
			foreach(Fee feeCur in listFees) {
				if(listCodeNums.Contains(feeCur.CodeNum)) {
					continue; //Skip the fee if it's associated to a procedure code that has already been increased / added.
				}
				//The best match isn't 0, and we haven't already done this CodeNum
				if(feeCur!=null && feeCur.Amount!=0) {
					double newVal=(double)feeCur.Amount*(1+(double)percent/100);
					if(round>0) {
						newVal=Math.Round(newVal,round);
					}
					else {
						newVal=Math.Round(newVal,MidpointRounding.AwayFromZero);
					}
					//The fee showing in the fee schedule is not a perfect match.  Make a new one that is.
					//E.g. We are increasing all fees for clinicNum of 1 and provNum of 5 and the best match found was for clinicNum of 3 and provNum of 7.
					//We would then need to make a copy of that fee, increase it, and then associate it to the clinicNum and provNum passed in (1 and 5).
					if(!feeSched.IsGlobal && (feeCur.ClinicNum!=clinicNum || feeCur.ProvNum!=provNum)) {
						Fee fee=new Fee();
						fee.Amount=newVal;
						fee.CodeNum=feeCur.CodeNum;
						fee.ClinicNum=clinicNum;
						fee.ProvNum=provNum;
						fee.FeeSched=feeSchedNum;
						listFees.Add(fee);
					}
					else { //Just update the match found.
						feeCur.Amount=newVal;
					}
				}
				listCodeNums.Add(feeCur.CodeNum);
			}
			return listFees;
		}

		///<summary>This method will remove and/or add a fee for the fee information passed in.
		///codeText will typically be one valid procedure code.  E.g. D1240
		///If an amt of -1 is passed in, then it indicates a "blank" entry which will cause deletion of any existing fee.
		///Returns listFees back after importing the passed in fee information.
		///Does not make any database calls.  This is left up to the user to take action on the list of fees returned.
		///Also, makes security log entries based on how the fee changed.  Does not make a log for codes that were removed (user already warned)</summary>
		public static List<Fee> Import(string codeText,double amt,long feeSchedNum,long clinicNum,long provNum,List<Fee> listFees) {
			//No need to check RemotingRole; no call to db.
			if(!ProcedureCodes.IsValidCode(codeText)){
				return listFees;//skip for now. Possibly insert a code in a future version.
			}
			string feeOldStr="";
			long codeNum = ProcedureCodes.GetCodeNum(codeText);
			Fee fee = listFees.FirstOrDefault(x => x.CodeNum==codeNum && x.FeeSched==feeSchedNum && x.ClinicNum==clinicNum && x.ProvNum==provNum);
			DateTime datePrevious=DateTime.MinValue;
			if(fee!=null) {
				feeOldStr=Lans.g("FormFeeSchedTools","Old Fee")+": "+fee.Amount.ToString("c")+", ";
				datePrevious=fee.SecDateTEdit;
				listFees.Remove(fee);
			}
			if(amt==-1) {
				return listFees;
			}
			fee=new Fee();
			fee.Amount=amt;
			fee.FeeSched=feeSchedNum;
			fee.CodeNum=ProcedureCodes.GetCodeNum(codeText);
			fee.ClinicNum=clinicNum;//Either 0 because you're importing on an HQ schedule or local clinic because the feesched is localizable.
			fee.ProvNum=provNum;
			listFees.Add(fee);//Insert new fee specific to the active clinic.
			SecurityLogs.MakeLogEntry(Permissions.ProcFeeEdit,0,Lans.g("FormFeeSchedTools","Procedure")+": "+codeText+", "+feeOldStr
				+Lans.g("FormFeeSchedTools","New Fee")+": "+amt.ToString("c")+", "
				+Lans.g("FormFeeSchedTools","Fee Schedule")+": "+FeeScheds.GetDescription(feeSchedNum)+". "
				+Lans.g("FormFeeSchedTools","Fee changed using the Import button in the Fee Tools window."),ProcedureCodes.GetCodeNum(codeText),
				DateTime.MinValue);
			SecurityLogs.MakeLogEntry(Permissions.LogFeeEdit,0,"Fee changed",fee.FeeNum,datePrevious);
			return listFees;
		}

		///<summary>Zeros securitylog FKey column for rows that are using the matching feeNum as FKey and are related to Fee.
		///Permtypes are generated from the AuditPerms property of the CrudTableAttribute within the Fee table type.</summary>
		public static void ClearFkey(long feeNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),feeNum);
				return;
			}
			Crud.FeeCrud.ClearFkey(feeNum);
		}

		///<summary>Zeros securitylog FKey column for rows that are using the matching feeNums as FKey and are related to Fee.
		///Permtypes are generated from the AuditPerms property of the CrudTableAttribute within the Fee table type.</summary>
		public static void ClearFkey(List<long> listFeeNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listFeeNums);
				return;
			}
			Crud.FeeCrud.ClearFkey(listFeeNums);
		}
		#endregion Misc Methods
	}

	///<summary>A class with a fee and an update type. 
	///Used by the FeeCache, to keep an in-memory list of pending changes for saving to the db.</summary>
	public class FeeUpdate {
		public Fee Fee {get;set;}
		/// <summary>Indicates whether the record is an Add, Update, or Delete</summary>
		public FeeUpdateType UpdateType {get;set;}

		///<summary>For serialization.</summary>
		public FeeUpdate() { }

		///<summary></summary>
		public FeeUpdate(Fee fee, FeeUpdateType updateType) {
			Fee=fee;
			UpdateType=updateType;
		}

		///<summary></summary>
		public FeeUpdate Copy() {
			return new FeeUpdate(Fee.Copy(),UpdateType);
		}
	}

	public enum FeeUpdateType {
		Add,
		Update,
		Remove
	}
}