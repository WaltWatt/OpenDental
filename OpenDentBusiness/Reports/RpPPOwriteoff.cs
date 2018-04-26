using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness {
	public class RpPPOwriteoff {
		///<summary></summary>
		public static DataTable GetWriteoffTable(DateTime dateStart,DateTime dateEnd,bool isIndividual,string carrierText, bool isWriteoffPay) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateStart,dateEnd,isIndividual,carrierText,isWriteoffPay);
			}
			string queryText="";
			//individual
			if(isIndividual) {
				queryText="SET @DateFrom="+POut.Date(dateStart)+", @DateTo="+POut.Date(dateEnd)
				+", @CarrierName='%"+POut.String(carrierText)+"%';";
				if(isWriteoffPay) {
					queryText+=@"SELECT claimproc.DateCP,
					CONCAT(CONCAT(CONCAT(CONCAT(patient.LName,', '),patient.FName),' '),patient.MiddleI),
					carrier.CarrierName,
					provider.Abbr,
					SUM(claimproc.FeeBilled),
					SUM(claimproc.FeeBilled-claimproc.WriteOff),
					SUM(claimproc.WriteOff),
					claimproc.ClaimNum
					FROM claimproc,insplan,patient,carrier,provider
					WHERE provider.ProvNum = claimproc.ProvNum
					AND claimproc.PlanNum = insplan.PlanNum
					AND claimproc.PatNum = patient.PatNum
					AND carrier.CarrierNum = insplan.CarrierNum
					AND (claimproc.Status=1 OR claimproc.Status=4) /*received or supplemental*/
					AND claimproc.DateCP >= @DateFrom
					AND claimproc.DateCP <= @DateTo
					AND insplan.PlanType='p'
					AND carrier.CarrierName LIKE @CarrierName
					GROUP BY claimproc.ClaimNum 
					ORDER BY claimproc.DateCP";
				}
				else {//use procedure date
					queryText+=@"SELECT claimproc.ProcDate,
					CONCAT(CONCAT(CONCAT(CONCAT(patient.LName,', '),patient.FName),' '),patient.MiddleI),
					carrier.CarrierName,
					provider.Abbr,
					SUM(claimproc.FeeBilled),
					SUM(claimproc.FeeBilled-claimproc.WriteOff),
					SUM(claimproc.WriteOff),
					claimproc.ClaimNum
					FROM claimproc,insplan,patient,carrier,provider
					WHERE provider.ProvNum = claimproc.ProvNum
					AND claimproc.PlanNum = insplan.PlanNum
					AND claimproc.PatNum = patient.PatNum
					AND carrier.CarrierNum = insplan.CarrierNum
					AND (claimproc.Status=1 OR claimproc.Status=4 OR claimproc.Status=0) /*received or supplemental or notreceived*/
					AND claimproc.ProcDate >= @DateFrom
					AND claimproc.ProcDate <= @DateTo
					AND insplan.PlanType='p'
					AND carrier.CarrierName LIKE @CarrierName
					GROUP BY claimproc.ClaimNum 
					ORDER BY claimproc.ProcDate";
				}
			}
			else {
				//group
				if(isWriteoffPay) {
					queryText="SET @DateFrom="+POut.Date(dateStart)+", @DateTo="+POut.Date(dateEnd)
						+", @CarrierName='%"+POut.String(carrierText)+"%';"
						+@"SELECT carrier.CarrierName,
						SUM(claimproc.FeeBilled),
						SUM(claimproc.FeeBilled-claimproc.WriteOff),
						SUM(claimproc.WriteOff),
						claimproc.ClaimNum
						FROM claimproc,insplan,carrier
						WHERE claimproc.PlanNum = insplan.PlanNum
						AND carrier.CarrierNum = insplan.CarrierNum
						AND (claimproc.Status=1 OR claimproc.Status=4) /*received or supplemental*/
						AND claimproc.DateCP >= @DateFrom
						AND claimproc.DateCP <= @DateTo
						AND insplan.PlanType='p'
						AND carrier.CarrierName LIKE @CarrierName
						GROUP BY carrier.CarrierNum 
						ORDER BY carrier.CarrierName";
				}
				else {
					queryText="SET @DateFrom="+POut.Date(dateStart)+", @DateTo="+POut.Date(dateEnd)
						+", @CarrierName='%"+POut.String(carrierText)+"%';"
						+@"SELECT carrier.CarrierName,
						SUM(claimproc.FeeBilled),
						SUM(claimproc.FeeBilled-claimproc.WriteOff),
						SUM(claimproc.WriteOff),
						claimproc.ClaimNum
						FROM claimproc,insplan,carrier
						WHERE claimproc.PlanNum = insplan.PlanNum
						AND carrier.CarrierNum = insplan.CarrierNum
						AND (claimproc.Status=1 OR claimproc.Status=4 OR claimproc.Status=0) /*received or supplemental or notreceived*/
						AND claimproc.ProcDate >= @DateFrom
						AND claimproc.ProcDate <= @DateTo
						AND insplan.PlanType='p'
						AND carrier.CarrierName LIKE @CarrierName
						GROUP BY carrier.CarrierNum 
						ORDER BY carrier.CarrierName";
				}
			}
			return ReportsComplex.RunFuncOnReportServer(() => ReportsComplex.GetTable(queryText));
		}	
	}

}
