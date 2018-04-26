using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;
using CodeBase;
using System.Data;
using System.Threading;
using System.Collections.Concurrent;

namespace OpenDentBusiness {
	///<summary>Extremely a-typical cache pattern. Does not extend from CacheAbs. 
	///The goal is to limit the number of fees stored in-memory at a given time. The HQ fees will always be in the cache, but we will keep at most 5 
	///additional clinics worth of fees available in the cache at a given time. This class can make changes to the database when _hasTransStarted is 
	///set to True. This feature should only be used when the user is likely to change multiple fees at the same time.</summary>
	public class FeeCache:IFeeCache,IEnumerable<Fee> {
		///<summary>A list of the x most recently used clinics (defaults to 5). As the user cycles through clinics, we drop the oldest clinic, including 
		///the fees attached to that clinic from the cache in order to prevent the cache from growing too large. Always contains the HQ clinic and the 
		///current clinic in addition to the x recently used clinics.</summary>
		private FeeCacheQueue _queueClinicNums; 
		///<summary>A dictionary containing the fees, clinics and feesched nums currently in the cache.</summary>
		private FeeCacheDictionary _dict=new FeeCacheDictionary();
		///<summary>Most of the time this should be left untouched. In some cases where we expect the user to make bulk changes, we flip this to true. 
		///When this flag is set to true, we will store all changes made to fees in _queueFeeUpdates, so as to spare the user from having to make a 
		///separate call to the database for each change. You must call BeginTransaction() to flip this to true, and SaveToDb() to set this back to false.</summary>
		private bool _hasTransStarted=false;
		///<summary>When _hasTransStarted is true, this will be an in-order list of every change made to the FeeCache since the transaction was started.
		///When _hasTransStarted is false, this will always be empty or null, as items can only be added to this list _hasTransStarted is true, and the
		///only way to set _hasTransStarted back to false is by calling SaveToDb which will empty the list.</summary>
		private ConcurrentQueue<FeeUpdate> _queueFeeUpdates=new ConcurrentQueue<FeeUpdate>();
		
		///<summary>Construct a new FeeCache with the provided _queueClinicNums size limit. If a clinic other than HQ is currently selected, add that 
		///clinic to the queue. When doInitialize is set to true load in all of the HQ fees and the fees for the current clinic if there is one.</summary>
		public FeeCache(int numClinics=5,bool doInitialize=true) {
			_queueClinicNums=new FeeCacheQueue(numClinics);
			_queueClinicNums.CurClinicNum=Clinics.ClinicNum;
			_dict=new FeeCacheDictionary();
			if (doInitialize) {
				Initialize();
			}
		}

		///<summary>Fill a new FeeCache with the provided list of fees. The size limit of _queueClinicNums will equal the number of distinct clinic nums 
		///represented in the list of fees.</summary>
		public FeeCache(List<Fee> listFees) {
			_queueClinicNums=new FeeCacheQueue(listFees.Select(x => x.ClinicNum).Distinct().Count());
			Initialize();
			Add(listFees);
		}

		///<summary>Load the dictionary with the non-hidden fee schedule fees for each of the clinics currently in the queue.</summary>
		public void Initialize() {
			List<long> listFeeScheds=FeeScheds.GetDeepCopy(true).Select(x => x.FeeSchedNum).ToList();
			_dict=new FeeCacheDictionary(Fees.GetByFeeSchedNumsClinicNums(listFeeScheds,_queueClinicNums.ToList()));
		}

		///<summary>Returns a deep copy of fee cache including _queueFeeUdpates and the current transaction status. If the current cache contains no fees
		///we fill with the HQ and current clinic fees before making the copy.</summary>
		public IFeeCache GetCopy() {
			FeeCache retVal=new FeeCache(doInitialize:false);
			if(_dict.Count==0) {
				Initialize(); //There is nothing in the cache, at least initialize to include the default/clinic 0 fees.
			}
			//If a fee has been invalidated, we will want to retrieve it from the db before we copy the cache
			List<long> listFeeSchedsToRefresh=_dict.Where(x => x.Value==null).Select(x => x.Key.FeeSchedNum).Distinct().ToList();
			foreach(long feeSched in listFeeSchedsToRefresh) {
				RefreshInvalidFees(feeSched);
			}
			retVal._dict=new FeeCacheDictionary(_dict);
			retVal._queueClinicNums=this._queueClinicNums.Copy();
			retVal._queueFeeUpdates=new ConcurrentQueue<FeeUpdate>(this._queueFeeUpdates.Select(x => x.Copy()));
			retVal._hasTransStarted=this._hasTransStarted;
			return retVal;
		}

		#region Cache Pattern
		//The FeeCache doesn't directly implement CacheAbs, but we still implement some of the same methods
		///<summary>Fills the local cache with the passed in DataTable. If the table contains more distinct clinics than we allow in the queue, we will 
		///not expand the size of the queue to fit them, but instead continue to dequeue the oldest clinics when the queue size limit is exceeded.</summary>
		public void FillCacheFromTable(DataTable table) {
			List<Fee> listFees=Crud.FeeCrud.TableToList(table);
			Add(listFees);
		}

		///<summary>Return the fees currently in the cache as a DataTable. If doRefreshCache is true, we will reload the HQ fees and fees for all of the 
		///clinics in the queue before returning the table.</summary>
		public DataTable GetTableFromCache(bool doRefreshCache) {
			if(doRefreshCache) {
				Initialize();
			}
			return Crud.FeeCrud.ListToTable(ToList(),"Fee");
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public void FillCacheIfNeeded() {
			Fees.GetTableFromCache(false);
		}
		#endregion Cache Pattern

		#region Get Methods
		///<summary>Attempts to return the fee that best matches the provided codeNum, feeSchedNum, clinicNum, and provNum.
		///When doGetExactMatch is true, this will return either the fee that matches the parameters exactly, or null if no such fee exists.
		///When doGetExactMatch is false, and the fee schedule is global, we ignore the clinicNum and provNum and return the HQ fee that matches the given codeNum and feeSchedNum.
		///When doGetExactMatch is false, and the fee schedule is not global, and no exact match exists we attempt to return the closest matching fee in this order:
		///1 - The fee with the same codeNum, feeSchedNum, and providerNum, with a clinicNum of 0
		///2 - The fee with the same codeNum, feeSchedNum, and clinicNum, with a providerNum of 0
		///3 - The fee with the same codeNum, feeSchedNum, and both a clinicNum and providerNum of 0
		///If no partial match can be found, return null.</summary>
		public Fee GetFee(long codeNum,long feeSchedNum,long clinicNum=0,long provNum=0,bool doGetExactMatch=false) {
			AddClinicNum(clinicNum);
			//Global fee schedules will never have clinicNum or provNum, so force them to equal 0 if they are not.
			if(FeeScheds.IsGlobal(feeSchedNum) && !doGetExactMatch) {
				clinicNum=0;
				provNum=0;
			}
			if(!_dict.ContainsFeeSched(feeSchedNum)) {
				//This is likely a hidden fee sched. We will add the fee sched to the cache now.
				List<Fee> listFeeSchedFees=Fees.GetByFeeSchedNumsClinicNums(new List<long> { feeSchedNum },this._queueClinicNums.ToList());
				if(listFeeSchedFees.Count==0) {
					_dict.AddFeeSchedOnly(feeSchedNum);
				}
				else {
					this.Add(listFeeSchedFees);
				}
			}
			if(!_dict.ContainsFeeSchedAndClinic(feeSchedNum,clinicNum)) {
				_dict.AddFeeSchedClinicNumOnly(feeSchedNum,clinicNum);
			}
			RefreshInvalidFees(feeSchedNum);
			//If the logic changes here, then we need to change FeeNoCache.GetFee.
			FeeKey feeKey=new FeeKey(feeSchedNum,clinicNum,codeNum,provNum);
			Fee exactMatch;
			if(_dict.TryGetValue(feeKey,out exactMatch)) {
				return exactMatch;//Found an exact match on clinic and provider
			}
			//Can't find exact an exact match
			if(doGetExactMatch) {
				return null;
			}
			//We don't need an exact match, try same provider and codeNum with default clinic.
			feeKey=new FeeKey(feeSchedNum,0,codeNum,provNum);
			Fee partialMatch;
			if(_dict.TryGetValue(feeKey,out partialMatch))	{
				return partialMatch;
			}
			//Can't find a match for the provider, try same clinic with no provider.
			feeKey=new FeeKey(feeSchedNum,clinicNum,codeNum,0);
			if(_dict.TryGetValue(feeKey,out partialMatch)) {
				return partialMatch;
			}
			//Can't find a match for the clinic, try the default for this code.
			feeKey=new FeeKey(feeSchedNum,0,codeNum,0);
			if(_dict.TryGetValue(feeKey,out partialMatch)) {
				return partialMatch;
			}
			return null;//This fee sched does not have a default fee for this code.
		}

		///<summary>Return the list of fees for the feeSchedNum. Pass in clinic num to ensure the clinic we want to use is loaded into the cache.</summary>
		public List<Fee> GetFeesForFeeSchedNum(long feeSchedNum,long clinicNum) {
			AddClinicNum(clinicNum);
			return this.Where(x => x.FeeSched==feeSchedNum).ToList();
		}

		///<summary>Return the list of fees for the clinicNum. The clinicNum will be added to the cache if needed.</summary>
		public List<Fee> GetFeesForClinic(long clinicNum) {
			AddClinicNum(clinicNum);
			return this.Where(x => x.ClinicNum==clinicNum).ToList();
		}

		///<summary>Return the list of fees for the list of clinicNums. Each clinic in the list will be added to the cache before its fees are added to 
		///the return list.Even if the number of distinct clinics in listClinicNums exceeds the limit of the clinic queue, we will not lose fees as they
		///are dropped from the cache.</summary>
		public List<Fee> GetFeesForClinics(IEnumerable<long> listClinicNums) {
			List<Fee> listFees=new List<Fee>();
			foreach(long clinicNum in listClinicNums.Distinct()) {
				listFees.AddRange(GetFeesForClinic(clinicNum));
			}
			return listFees;
		}

		///<summary>Return the list of fees matching all three of the provided parameters.</summary>
		public List<Fee> GetListFees(long feeSchedNum,long clinicNum,long provNum) {
			AddClinicNum(clinicNum);
			return this.Where(x => x.FeeSched==feeSchedNum && x.ClinicNum==clinicNum && x.ProvNum==provNum).ToList();
		}

		///<summary>Returns the amount for the fee that best matches the provided parameters (according to the rules of GetFee() when doGetExactMatch
		///is false. Returns 0 if the fee can't be found.</summary>
		public double GetAmount0(long codeNum,long feeSchedNum,long clinicNum = 0,long provNum = 0) {
			//No need to check RemotingRole; no call to db.
			AddClinicNum(clinicNum);
			if(FeeScheds.GetIsHidden(feeSchedNum)) {
				return 0;//you cannot obtain fees for hidden fee schedules
			}
			Fee fee = GetFee(codeNum,feeSchedNum,clinicNum,provNum);
			if(fee==null) {
				return 0;
			}
			return fee.Amount;
		}
		#endregion Get Methods

		#region Modification Methods

		#region Insert
		///<summary>If _hasTransStarted is true, add the fee to _queueFeeUpdates as an Insert. If the clinic for the fee is not currently in the clinic
		///queue, add that clinic to the queue. Finally always add the fee to the cache.</summary>
		public void Add(Fee fee) {
			UpdateFeeChangedStatus(fee,FeeUpdateType.Add);
			AddClinicNum(fee.ClinicNum);
			AddToCacheOnly(fee);
		}

		///<summary>Cannot be used as part of a transaction (fees will never be added to _queueFeeUpdates). Before adding the fees to the cache, attempt
		///to add the distinct clinics to the clinic queue if needed, and then add each fee to the cache.</summary>
		private void Add(List<Fee> listFees) {
			listFees.GroupBy(x => x.ClinicNum).ToList()
				.ForEach(x=>_queueClinicNums.Enqueue(x.Key));
			foreach(Fee fee in listFees) {
				AddToCacheOnly(fee);
			}
		}

		///<summary>Insert a fee into the cache without processing clinic changes or transactions. 
		///Only use to populate the existing _dict with additonal fees from the db. Returns true if the operation was successful, false if the _dict 
		///already contains the key.</summary>
		private bool AddToCacheOnly(Fee fee) {
			return _dict.TryAdd(new FeeKey(fee),(FeeLim)fee);
		}

		///<summary>Add a clinic to the queue, udpate the currently selected clinic if needed, and remove the oldest clinic if the number of queued 
		///clinics is more than x + 2 (HQ and current Clinic). If we add the clinic to the queue, we also get the fees for that clinic and add them to 
		///the cache. If we remove a clinic from the queue, we also remove the fees for that clinic from the cache.</summary>
		private void AddClinicNum(long clinicNum) {
			long removedClinic;
			if(_queueClinicNums.CurClinicNum!=Clinics.ClinicNum) {
				if(_queueClinicNums.Contains(Clinics.ClinicNum)) {
					_queueClinicNums.CurClinicNum=Clinics.ClinicNum;					
				}
				else {
					long oldCurClinic=_queueClinicNums.CurClinicNum;
					if(_queueClinicNums.Enqueue(oldCurClinic,out removedClinic)) {
						RemoveClinicFees(removedClinic);
					}	
					AddClinicFees(Clinics.ClinicNum);
					_queueClinicNums.CurClinicNum=Clinics.ClinicNum;
				}
			}
			if(_queueClinicNums.Contains(clinicNum)) {
				return;
			}
			if(_queueClinicNums.Enqueue(clinicNum,out removedClinic)) {
				RemoveClinicFees(removedClinic);
			}		
			AddClinicFees(clinicNum);	
		}

		///<summary>Get all non-hidden fee sched fees for a clinic and add to the cache. Cannot be used as part of a transaction.</summary>
		private void AddClinicFees(long clinicNum) {
			List<long> listFeeScheds=FeeScheds.GetDeepCopy(true).Select(x => x.FeeSchedNum).ToList();
			List<Fee> listFees=Fees.GetByFeeSchedNumsClinicNums(listFeeScheds,new List<long>() { clinicNum });
			Add(listFees);
		}

		///<summary>Get the list of fees for a list of feeSchedules and their clinicNums and add to the cache. Cannot be used as part of a transaction.</summary>
		private void AddFeeSchedFees(List<long> listFeeScheds,List<long> listClinicNums) {
			List<Fee> listFees=Fees.GetByFeeSchedNumsClinicNums(listFeeScheds,listClinicNums);
			Add(listFees);
		}
		#endregion Insert

		#region Update
		///<summary>If _hasTransStarted is true, add the fee to _queueFeeUpdates as an Update. If the fee's clinic is not currently in the queue, add the
		///clinic to the queue. Finally, always attempt to update the fee amount for the existing fee in the cache. If the fee does not exist in the cache,
		///add to the cache instead of updating.</summary>
		public void Update(Fee fee) {
			UpdateFeeChangedStatus(fee,FeeUpdateType.Update);
			AddClinicNum(fee.ClinicNum);
			try {
				RefreshInvalidFees(fee.FeeSched);
				_dict[new FeeKey(fee)].Amount=fee.Amount;
			}
			catch(Exception e) {
				AddToCacheOnly(fee);
				e.DoNothing();
			}
		}
		#endregion Update

		#region Delete
		///<summary>If _hasTransStarted is true, add the fee to _queueFeeUpdates as a Delete. If the fee's clinic is not currently in the queue, add the 
		///clinic to the queue. Finally, always attempt to remove the fee from the cache. Returns true if successful, false if the fee is not found in the dictionary. </summary>
		public bool Remove(Fee fee) {
			UpdateFeeChangedStatus(fee,FeeUpdateType.Remove);
			AddClinicNum(fee.ClinicNum);
			FeeLim removedFee;
			return _dict.TryRemove(new FeeKey(fee),out removedFee);
		}

		///<summary>If the clinic is 0, do nothing, as we always want to keep HQ fees in the cache.
		///If _hasTransaction is true, call SaveToDb() before removing the clinic fees so that changes are not lost, then call BeginTransaction() again to
		///restart the transaction. Finally, always attempt to remove all of the fees for the clinic from the cache.</summary>
		private void RemoveClinicFees(long clinicNum) {
			if(clinicNum==0) { //We always want to have the fees for ClinicNum 0 in the cache.
				return;
			}
			//If a transaction has started, save any in-memory changes before removing them from the cache
			if(_hasTransStarted) {
				SaveToDb();
				BeginTransaction();
			}
			foreach(FeeKey key in _dict.Keys.Where(x => x.ClinicNum==clinicNum)) {
				FeeLim outFee;
				_dict.TryRemove(key,out outFee);
			}	
		}

		/// <summary>If the clinicNum is not in the queue, attempt to add it to the cache before calling remove.
		/// If _hasTransStarted is true, each remove will be added to _queueFeeUpdates as a Delete. Finally, always attempt to remove all fees for 
		/// the specified FeeSchedule,ClinicNum,and ProvNum. </summary>
		public void RemoveFees(long feeSchedNum,long clinicNum,long provNum) {
			AddClinicNum(clinicNum);
			List<Fee> listFee=GetListFees(feeSchedNum,clinicNum,provNum);
			foreach(Fee fee in listFee) {
				Remove(fee);
			}
		}
		#endregion Delete

		#endregion Modification Methods

		#region Transaction Methods

		/// <summary>Sets the _hasTransStarted flag to true. If there are currently items in _queueFeeUpdates, use those, otherwise create an empty queue.
		/// While _hasTransStarted is true, Add/Update/Delete methods in the cache will record their changes to _queueFeeUpdates for saving to the db later.</summary>
		public void BeginTransaction() {
			_hasTransStarted=true;
			_queueFeeUpdates=_queueFeeUpdates??new ConcurrentQueue<FeeUpdate>();
		}

		/// <summary>If _hasTransStarted is true, add the fee to _queueFeeUpdates with the specified UpdateType (Insert,Update,or Delete).</summary>
		private void UpdateFeeChangedStatus(Fee fee,FeeUpdateType updateType) {
			if(_hasTransStarted) { //Only do this if BeginTransaction has been called
				_queueFeeUpdates.Enqueue(new FeeUpdate(fee.Copy(),updateType));
			}
		}

		///<summary>If there is nothing in _queueFeeUdpates, reset _hasTransStarted to false and return.
		///If all items in _queueFeeUpdates are Inserts, do a bulk insert to save time.
		///If all items in _queueFeeUpdates are Deletes, do a bulk delete to save time.
		///Otherwise, we need to loop through the list of updates and save them to the db, as order matters for these operations.
		///Set signalods invalid for each distinct fee schedule represented in _queueFeeUpdates and re-initialize the static cache used by the Fees class.
		///Finally, set _hasTransStarted back to false and clear _queueFeeUpdates.</summary>
		public void SaveToDb() {
			if(_queueFeeUpdates==null || _queueFeeUpdates.Count==0) {
				_hasTransStarted=false;
				return;
			}
			List<long> listFeeScheds=new List<long>();
			if(_queueFeeUpdates.All(x => x.UpdateType==FeeUpdateType.Add)) { //all updates are inserts, so do bulk insert
				listFeeScheds=_queueFeeUpdates.Select(x => x.Fee.FeeSched).Distinct().ToList();
				Fees.InsertMany(_queueFeeUpdates.Select(x => (Fee)x.Fee).ToList());
			}
			else if(_queueFeeUpdates.All(x => x.UpdateType==FeeUpdateType.Remove)) { //all updates are removes, so do bulk delete
				listFeeScheds=_queueFeeUpdates.Select(x => x.Fee.FeeSched).Distinct().ToList();
				Fees.DeleteMany(_queueFeeUpdates.Select(x => x.Fee.FeeNum).ToList());
			}
			else { //There is a mixture of Add, Removes, and Updates
				listFeeScheds=Fees.UpdateFromCache(_queueFeeUpdates.ToList());
			}
			foreach(long feeSched in listFeeScheds) {
				Signalods.SetInvalid(InvalidType.Fees,KeyType.FeeSched,feeSched);
			}
			Fees.InvalidateFeeSchedules(listFeeScheds);
			_hasTransStarted=false;
			_queueFeeUpdates=new ConcurrentQueue<FeeUpdate>();//clear the list of updates
		}

		///<summary>Null out an existing feeSchedule in the dictionary to trigger an update later.</summary>
		public void Invalidate(long feeSchedNum) {
			_dict.InvalidateFeeSched(feeSchedNum);
		}
		#endregion Transaction Methods

		#region Misc Methods
		///<summary>Return a copy of all fees in the cache as a list.</summary>
		public List<Fee> ToList() {
			return _dict.Select(x => (Fee)x.Value).ToList();
		}

		///<summary>Implement the IEnumerator interface to allow the cache to be used with Linq.</summary>
		public IEnumerator<Fee> GetEnumerator() {
			foreach(Fee fee in ToList()) {//We need to call ToList() so that we can iterate through a copy of the fees that no one else will be modifying.
				yield return fee;
			}
		}

		///<summary>Implement the IEnumerator interface to allow the cache to be used with Linq.</summary>
		IEnumerator IEnumerable.GetEnumerator() {
			return this.GetEnumerator();
		}

		///<summary>Returns true if there are any fees in the cache representing the given fee sched, otherwise false.</summary>
		public bool ContainsFeeSched(long feeSchedNum) {
			return _dict.ContainsFeeSched(feeSchedNum);
		}

		/// <summary>Checks if the Fee Schedule is invalidated. If it is invalidated, refresh the fees for that Fee Sched and the clinics in the queue
		/// from the db.</summary>
		public void RefreshInvalidFees(long feeSchedNum) {
			if(_dict.IsFeeSchedInvalidated(feeSchedNum)) {
				List<Fee> listFeeSchedFees=Fees.GetByFeeSchedNumsClinicNums(new List<long> { feeSchedNum },this._queueClinicNums.ToList());
				_dict.RefreshFeeSched(feeSchedNum,listFeeSchedFees);
			}
		}
		#endregion Misc Methods

		#region Private Classes
		///<summary>A wrapper of the LimitedSizeQueue class for use with the FeeCache. Maintain a queue of x clinic nums, plus the HQ 
		///clinic num (0), and the current clinicNum. When attempting to add a new clinic num to the queue, if the add operation would exceed the size 
		///limit x of the queue, remove the oldest clinic from the queue. The underlying implementation uses a ConcurrentQueue for thread safety.</summary>
		private class FeeCacheQueue:LimitedSizeQueue<long> {
			///<summary>HQ Clinic Num.</summary>
			private const long _hqClinicNum=0;
			///<summary>The clinic that we *think* is the current clinic. The cache accounts for the possibility of this being out of date when handling 
			///clinic operations.</summary>
			public long CurClinicNum { get; set; }

			///<summary>Construct a FeeCacheQueue with the provided size limit.</summary>
			public FeeCacheQueue(int limit):base(limit) {}

			///<summary>Returns true if the clinic is either in the queue, or if it is the currently selected clinic. Hides LimitedSizeQueue.Contains().</summary>
			public new bool Contains(long clinicNum) {
				if(clinicNum==CurClinicNum || clinicNum==_hqClinicNum) {
					return true;
				}
				return base.Contains(clinicNum);
			}

			///<summary>Returns the list of clinicNums including the HQ and Selected ClinicNums. Hides LimitedSizeQueue.ToList()</summary>
			public new List<long> ToList() {
				List<long> list=new List<long>() { _hqClinicNum,CurClinicNum };
				return list.Union(base.ToList()).Distinct().ToList();
			}

			///<summary>Return a copy of the queue.</summary>
			public FeeCacheQueue Copy() {
				FeeCacheQueue retVal=new FeeCacheQueue(this.Limit);
				retVal.CurClinicNum=this.CurClinicNum;
				foreach(long clinicNum in this) {
					retVal.Enqueue(clinicNum);
				}
				return retVal;
			}
		}

		///<summary>A limited fee class that eliminates unused fields. Used by the FeeCache to keep memory usage down.</summary>
		private class FeeLim {
			///<summary>Primary key.</summary>
			public long FeeNum;
			///<summary>The amount usually charged.  If an amount is unknown, then the entire Fee entry is deleted from the database.  
			///The absence of a fee is shown in the user interface as a blank entry.
			///For clinic and/or provider fees, amount can be set to -1 which indicates that their fee should be blank and not use the default fee.
			///</summary>
			public double Amount;
			///<summary>FK to feesched.FeeSchedNum.</summary>
			public long FeeSched;
			///<summary>FK to procedurecode.CodeNum.</summary>
			public long CodeNum;
			///<summary>FK to clinic.ClinicNum. (Used only if localization of fees for a feesched is enabled)</summary>
			public long ClinicNum;
			///<summary>FK to provider.ProvNum. (Used only if localization of fees for a feesched is enabled)</summary>
			public long ProvNum;
			///<summary>Date. Used in securitylogs to track when fees are updated or modified.</summary>
			public DateTime SecDateTEdit;

			/// <summary>Enable casting between Fee and Cached Fee. Will fail if attempting to cast a null. Casting will always return a deep copy.</summary>
			public static explicit operator Fee(FeeLim f) {
				return new Fee()
				{
					FeeNum=f.FeeNum,
					Amount=f.Amount,
					FeeSched=f.FeeSched,
					CodeNum=f.CodeNum,
					ClinicNum=f.ClinicNum,
					ProvNum=f.ProvNum,
					SecDateTEdit=f.SecDateTEdit
				};
			}

			/// <summary>Enable casting between Fee and Cached Fee. Will fail if attempting to cast a null. Casting will always return a deep copy.</summary>
			public static explicit operator FeeLim(Fee f) {
				return new FeeLim()
				{
					FeeNum=f.FeeNum,
					Amount=f.Amount,
					FeeSched=f.FeeSched,
					CodeNum=f.CodeNum,
					ClinicNum=f.ClinicNum,
					ProvNum=f.ProvNum,
					SecDateTEdit=f.SecDateTEdit
				};
			}

			///<summary>Return a deep copy.</summary>
			public FeeLim Copy() {
				return (FeeLim)this.MemberwiseClone();
			}
		}

		///<summary>Composite Key for the FeeCacheDictionary Class, made up of FeeSchedNum, ClinicNum, CodeNum, and ProvNum. A wrapper class for Tuple&lt;long,long,long,long&gt;</summary>
		private class FeeKey:Tuple<long,long,long,long> {
			///<summary>Fee.FeeSchedNum</summary>
			public long FeeSchedNum { get { return base.Item1; } }
			///<summary>Fee.ClinicNum</summary>
			public long ClinicNum { get { return base.Item2; } }
			///<summary>Fee.CodeNum</summary>
			public long CodeNum { get { return base.Item3; } }
			///<summary>Fee.ProvNum</summary>
			public long ProvNum { get { return base.Item4; } }

			///<summary>Construct a new fee key from the given parameters.</summary>
			public FeeKey(long feeSchedNum,long clinicNum,long codeNum,long provNum) : base(feeSchedNum,clinicNum,codeNum,provNum) { }

			///<summary>Construct a new fee key using the properties of the given limited fee.</summary>
			public FeeKey(FeeLim fee) : base(fee.FeeSched,fee.ClinicNum,fee.CodeNum,fee.ProvNum) { }

			///<summary>Construct a new fee key using the properties of the given fee.</summary>
			public FeeKey(Fee fee) : base(fee.FeeSched,fee.ClinicNum,fee.CodeNum,fee.ProvNum) { }
		}

		///<summary>The Dictionary used by the FeeCache. Uses a ConcurrentDictionary to store FeeLims under FeeKeys. No additional locking is required
		///when adding and removing fees from this dictionary. The class also maintains two other dictionaries, _dictFeeSchedNums and 
		///_dictFeeSchedClinicNums, to facilitate faster lookup times when checking if the cache contains any fees for a specific FeeSchedNum or 
		///FeeSchedNum/ClinicNum combo. Updating these dictionaries requires some additional locking so that if multiple threads try to update the counts
		///in these dictionaries, they do not interfere with each other/overwrite each others counts.</summary>
		private class FeeCacheDictionary:ConcurrentDictionary<FeeKey,FeeLim> {
			/// <summary>A concurrent dictionary where the key is a FeeSchedNum currently in the cache and the value is the total number of fees stored 
			/// under that FeeSchedNum.</summary>
			private ConcurrentDictionary<long,int> _dictFeeSchedNums=new ConcurrentDictionary<long,int>();
			/// <summary>A conurrent dictionary where the key is a Tuple of FeeSchedNum, ClinicNum currently in the cache and the value is the total number
			/// of fees stored under that FeeSchedNum. The memory storage of this dictionary should not be an issue, as at most we maintain x clinics in
			/// cache (so maximum number of entries is x * (# Fee Scheds).</summary>
			private ConcurrentDictionary<Tuple<long,long>,int> _dictFeeSchedClinicNums=new ConcurrentDictionary<Tuple<long,long>,int>();
			///<summary>The fee schedules that have been invalidated. We store this so that the IsFeeSchedInvalidated check will always be O(1) time.</summary>
			private HashSet<long> _setInvalidFeeScheds=new HashSet<long>();
			///<summary>A lock object. Used when updating the counts for _dictFeeSchedNums and _dictFeeSchedClinicNums, 
			///to prevent other threads from overwriting in the middle of an add or remove operation.
			///Also used for locking _setInvalidFeeScheds.</summary>
			private object _lockObj=new object();

			///<summary>Construct an empty dictionary.</summary>
			public FeeCacheDictionary():base() {}

			///<summary>Fill the dictionary with the list of fees. Additionally adds the unique Fee Schedules to _dictFeeSchedNums, and the unique
			///FeeSchedNum/ClinicNum combinations to _dictFeeSchedClinicNums.</summary>
			public FeeCacheDictionary(IEnumerable<Fee> listFees) {
				foreach(Fee fee in listFees) {
					TryAdd(new FeeKey(fee),(FeeLim)fee);
				}
			}

			///<summary>Copy the provided dictionary into the current dictionary, including _dictFeeSchedNums, and _dictFeeSchedClinicNums.</summary>
			public FeeCacheDictionary(FeeCacheDictionary dict):base(dict.Where(x => x.Value!=null).ToDictionary(x => x.Key, x => x.Value.Copy())) {
				_dictFeeSchedNums=new ConcurrentDictionary<long, int>(dict._dictFeeSchedNums);
				_dictFeeSchedClinicNums=new ConcurrentDictionary<Tuple<long, long>, int>(dict._dictFeeSchedClinicNums);
			}

			/// <summary>Override the dict[key] getter and setter. In the setter specifically, we want to be able to update the counts in 
			/// _dictFeeSchedNums, and _dictFeeSchedClinicNums, to reflect the added fee.</summary>
			public new FeeLim this[FeeKey key] {
				get { return base[key]; }
				set {
					if(!ContainsKey(key)) {
						IncreaseFeeCounts(key);
					}
					base[key]=value;
				}
			}	

			/// <summary>Attempts to add the specified key and value to the dictionary. Additionally either updates or adds the feeSchedNum and 
			/// feeSchedClinicNum pair to _dictFeeSchedNums and _dictFeeSchedClinicNums respectively.
			/// Returns true if the key/value pair was added successfully; false if the key already exists.</summary>
			public new bool TryAdd(FeeKey key, FeeLim fee) {
				bool isSuccessful=base.TryAdd(key,fee);
				if(isSuccessful) {
					IncreaseFeeCounts(key);
				}
				return isSuccessful;
			}

			/// <summary>Attempts to remove and return the value that has the specified key from the dictionary. Additionally either updates or removes the 
			/// feeSchedNum and feeSchedClinicNum pair from _dictFeeSchedNums and _dictFeeSchedClinicNums respectively.
			/// Returns true if the key/value pair was added successfully, and returns the removed fee as an out parameter. Hides ConcurrentDictionary.TryRemove().</summary>
			public new bool TryRemove(FeeKey key,out FeeLim fee) {
				FeeLim retVal;
				bool isSuccessful=base.TryRemove(key,out retVal);
				if(isSuccessful) {
					lock(_lockObj) {
						_dictFeeSchedNums[key.FeeSchedNum]--;
						if(_dictFeeSchedNums[key.FeeSchedNum]<=0) {
							int count;
							_dictFeeSchedNums.TryRemove(key.FeeSchedNum,out count);
						}
					}
					Tuple<long,long> fscKey=Tuple.Create<long,long>(key.FeeSchedNum,key.ClinicNum);
					lock(_lockObj) {
						_dictFeeSchedClinicNums[fscKey]--;
						if(_dictFeeSchedClinicNums[fscKey]<=0) {
							int count;
							_dictFeeSchedClinicNums.TryRemove(fscKey,out count);
						}
					}
				}
				fee=retVal;
				return isSuccessful;
			}

			///<summary>Attempts to add the feeSchedNum to only _dictFeeSchedNums, with a count of 0 to save the FeeCache from having to check if there are
			///fees for this fee schedule multiple times in a row. Returns true if the add is succesful, false if the feeSchedNum already exists.</summary>
			public bool AddFeeSchedOnly(long feeSchedNum) {
				lock(_lockObj) {
					return _dictFeeSchedNums.TryAdd(feeSchedNum,0);
				}
			}

			///<summary>Attempts to add the FeeSchedNum/ClinicNum combo to only _dictFeeSchedClinicNums, with a count of 0 to save the FeeCache from having
			///to check if there are fees for this composite key multiple times in a row. Returns true if the add is succesful, false otherwise.</summary>
			public bool AddFeeSchedClinicNumOnly(long feeSchedNum, long clinicNum) {
				lock(_lockObj) {
					return _dictFeeSchedClinicNums.TryAdd(Tuple.Create<long,long>(feeSchedNum,clinicNum),0);
				}
			}

			/// <summary>Attempts to get the value associated with the specified key from the dictionary. Returns true if the key was found in dictionary; 
			/// otherwise, false. Returns the requested fee as an out parameter. Casts the FeeLim to a fee, so it will always return a deep copy. Hides
			/// ConcurrentDictionary.TryGetValue().</summary>
			public new bool TryGetValue(FeeKey key, out Fee fee) {
				FeeLim feeLim;
				bool result=base.TryGetValue(key,out feeLim);
				fee=feeLim!=null ? (Fee)feeLim : null;
				return result;
			}

			/// <summary>Returns true if the requested feeSchedNum is in _dictFeeSchedNums, regardless of the count (which could be 0).</summary>
			public bool ContainsFeeSched(long feeSchedNum) {
				int count;
				return _dictFeeSchedNums.TryGetValue(feeSchedNum,out count);
			}

			/// <summary>Returns true if the requested feeSchedNum/clinicNum pair is in _dictFeeSchedClinicNums,, regardless of the count (which could be 0)</summary>
			public bool ContainsFeeSchedAndClinic(long feeSchedNum,long clinicNum) {
				int count;
				return _dictFeeSchedClinicNums.TryGetValue(Tuple.Create<long,long>(feeSchedNum,clinicNum),out count);
			}

			///<summary>Attempts to increase the counts stored in _dictFeeSchedNums and _dictFeeSchedClinicNums. 
			///If the key.FeeSchedNum isn't in _dictFeeSchedNums, add it with a count of 0, and then always increment. If the key.FeeSchedNum/fee.ClinicNum combo isn't in
			///_dictFeeSchedClinicNums, add it with a count of 0, and then always increment.</summary>
			private void IncreaseFeeCounts(FeeKey key) {
				lock(_lockObj) {
					if(_dictFeeSchedNums.GetOrAdd(key.FeeSchedNum,0)>=0) {
						_dictFeeSchedNums[key.FeeSchedNum]++;
					}
					if(_dictFeeSchedClinicNums.GetOrAdd(Tuple.Create<long,long>(key.FeeSchedNum,key.ClinicNum),0)>=0) {
						_dictFeeSchedClinicNums[Tuple.Create<long,long>(key.FeeSchedNum,key.ClinicNum)]++;
					}
				}
			}

			///<summary>Sets all fees to null in the dictionary for the given feeSchedNum.</summary>
			public void InvalidateFeeSched(long feeSchedNum) {
				foreach(FeeKey key in Keys.Where(x => x.FeeSchedNum==feeSchedNum)) {
					this[key]=null;
				}
				lock(_lockObj) {
					_setInvalidFeeScheds.Add(feeSchedNum);
				}
			}

			///<summary>Returns true if the fee sched has been invalidated and needs to be refreshed.</summary>
			public bool IsFeeSchedInvalidated(long feeSchedNum) {
				lock(_lockObj) {
					return _setInvalidFeeScheds.Contains(feeSchedNum);
				}
			}

			///<summary></summary>
			public void RefreshFeeSched(long feeSchedNum,List<Fee> listFeeSchedFees) {
				lock(_lockObj) {
					foreach(Fee fee in listFeeSchedFees) {
						this[new FeeKey(fee)]=(FeeLim)fee;
					}
					_setInvalidFeeScheds.Remove(feeSchedNum);
				}
			}
		}
		#endregion Private Classes
	}
}
