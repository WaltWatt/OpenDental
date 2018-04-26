using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness {
	public class RpReferralAnalysis {
		///<summary></summary>
		public static DataTable GetReferralTable(DateTime dateStart,DateTime dateEnd,List<long> listProvNums
			,bool hasAddress,bool hasOnlyNewPats) 
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateStart,dateEnd,listProvNums,hasAddress,hasOnlyNewPats);
			}
			string whereProv="";
			if(listProvNums.Count > 0) {
				whereProv+=" AND procedurelog.ProvNum IN("+string.Join(",",listProvNums)+") ";
			}
			string query=@"SELECT referral.LName,referral.FName,
				COUNT(DISTINCT attach.PatNum) AS HowMany,
				SUM(procedurelog.ProcFee*(procedurelog.UnitQty+procedurelog.BaseUnits)) AS '$HowMuch'";
			if(hasAddress) {
				query+=",referral.Title,referral.Address,referral.Address2,referral.City,referral.ST,referral.Zip,definition.ItemName AS Specialty";
			}
			query+=" FROM referral"
				+" INNER JOIN("
					+" SELECT refattach.ReferralNum, refattach.PatNum FROM refattach"
					+" WHERE refattach.RefType="+POut.Int((int)ReferralType.RefFrom)
					+" GROUP BY refattach.PatNum, refattach.ReferralNum"
				+") attach ON attach.ReferralNum = referral.ReferralNum"
				+" INNER JOIN procedurelog ON procedurelog.PatNum=attach.PatNum"
					+" AND procedurelog.ProcDate BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateEnd)
					+" AND procedurelog.ProcStatus=" +POut.Int((int)ProcStat.C)
				+" INNER JOIN procedurecode ON procedurecode.CodeNum=procedurelog.CodeNum"
					+" AND ProcCode NOT IN ('D9986','D9987')"//Do not count missed or canceled appointments
					+" "+whereProv
				+" INNER JOIN patient ON patient.PatNum=procedurelog.PatNum";
			if(hasOnlyNewPats) {
				query+=" AND patient.DateFirstVisit BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateEnd);
			}
			if(hasAddress) {
				query+=" LEFT JOIN definition ON referral.Specialty=definition.DefNum";
			}
			query+=" GROUP BY referral.ReferralNum"
				+" ORDER BY HowMany Desc";
			return ReportsComplex.RunFuncOnReportServer(() => ReportsComplex.GetTable(query));
		}
	}

}
