using System.Data;

namespace OpenDentBusiness {
	///<summary>This interfaces represents the functionality of a cache that stores fees.</summary>
	public interface IFeeCache {
		///<summary>Initializes the cache.</summary>
		void Initialize();
		///<summary>Gets the fee that matches the given parameters. If doGetExactMatch is false, then the fee returned might have a clinic num or prov
		///num of 0.</summary>
		Fee GetFee(long codeNum,long feeSchedNum,long clinicNum=0,long provNum=0,bool doGetExactMatch=false);
		///<summary>Invalidates the fees stored for the given fee schedule.</summary>
		void Invalidate(long feeSchedNum);
		///<summary>Returns a copy of itself.</summary>
		IFeeCache GetCopy();
		///<summary>Fills the cache with the passed in datatable.</summary>
		void FillCacheFromTable(DataTable table);
		///<summary>Gets the fees in the cache in the form of a DataTable.</summary>
		DataTable GetTableFromCache(bool doRefreshCache);
	}
}
