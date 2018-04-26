using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness {
	public class FeeNoCache:IFeeCache {

		///<summary>Does nothing.</summary>
		public void Initialize() {
			//No need to initialize anything
		}

		///<summary>Gets the fee directly from the database every time.</summary>
		public Fee GetFee(long codeNum,long feeSchedNum,long clinicNum=0,long provNum=0,bool doGetExactMatch=false) {
			if(FeeScheds.IsGlobal(feeSchedNum) && !doGetExactMatch) {
				clinicNum=0;
				provNum=0;
			}
			//If the logic changes here, then we need to change FeeCache.GetFee.
			string command=
				//Search for exact match first
				@"SELECT fee.*
				FROM fee
				WHERE fee.CodeNum="+POut.Long(codeNum)+@"
				AND fee.FeeSched="+POut.Long(feeSchedNum)+@"
				AND fee.ClinicNum="+POut.Long(clinicNum)+@"
				AND fee.ProvNum="+POut.Long(provNum);
			if(!doGetExactMatch) {
				//Then try same provider and codeNum with default clinic. 
				command+=@"
					UNION ALL
					SELECT fee.*
					FROM fee
					WHERE fee.CodeNum="+POut.Long(codeNum)+@"
					AND fee.FeeSched="+POut.Long(feeSchedNum)+@"
					AND fee.ClinicNum=0
					AND fee.ProvNum="+POut.Long(provNum);
				//Then try same clinic with no provider.
				command+=@"
					UNION ALL
					SELECT fee.*
					FROM fee
					WHERE fee.CodeNum="+POut.Long(codeNum)+@"
					AND fee.FeeSched="+POut.Long(feeSchedNum)+@"
					AND fee.ClinicNum="+POut.Long(clinicNum)+@"
					AND fee.ProvNum=0";
				//Then try default clinic with no provider.
				command+=@"
					UNION ALL
					SELECT fee.*
					FROM fee
					WHERE fee.CodeNum="+POut.Long(codeNum)+@"
					AND fee.FeeSched="+POut.Long(feeSchedNum)+@"
					AND fee.ClinicNum=0
					AND fee.ProvNum=0";
			}
			return Crud.FeeCrud.SelectOne(command);
		}

		///<summary>Returns a reference to itself. This class stores no state, so there is no need to make a deep copy.</summary>
		public IFeeCache GetCopy() {
			return this;
		}

		///<summary>Does nothing.</summary>
		public void FillCacheFromTable(DataTable table) {
			//No need to fill anything
		}

		///<summary>Returns an empty DataTable.</summary>
		public DataTable GetTableFromCache(bool doRefreshCache) {
			return new DataTable();
		}

		///<summary>Does nothing.</summary>
		public void Invalidate(long feeSchedNum) {
			//No need to invalidate anything since we're not caching anything
		}
	}
}
