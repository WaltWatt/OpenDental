using CodeBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness {
	public class RpPatPortionUncollected {
		///<summary></summary>
		public static DataTable GetPatUncollected(DateTime dateFrom,DateTime dateTo,List<long> listClinicNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateFrom,dateTo,listClinicNums);
			}
			bool hasClinicsEnabled=ReportsComplex.RunFuncOnReportServer(() =>(!Prefs.GetBoolNoCache(PrefName.EasyNoClinics)));
			string query="SELECT procedurelog.ProcDate,CONCAT(patient.LName,', ',patient.FName)AS Patient, procedurecode.AbbrDesc, "
				+"procedurelog.ProcFee*(procedurelog.BaseUnits+procedurelog.UnitQty)-IFNULL(cp.CapWriteOff,0) Fee,"
				+"IFNULL((procedurelog.ProcFee*(procedurelog.BaseUnits+procedurelog.UnitQty))-IFNULL(cp.CapWriteOff,0)-IFNULL(cp.InsAmt,0)"
				+"-IFNULL(cp.WriteOff,0),0) AS PatPortion,"
				+"IFNULL(adj.AdjAmt,0) AS Adjustment,"
				+"IFNULL(pay.SplitAmt,0) AS Payment," 
				+"IFNULL((procedurelog.ProcFee*(procedurelog.BaseUnits+procedurelog.UnitQty))-IFNULL(cp.CapWriteOff,0)-IFNULL(cp.InsAmt,0)-"
				+"IFNULL(cp.WriteOff,0),0)+IFNULL(adj.AdjAmt,0)-IFNULL(pay.SplitAmt,0) Uncollected "
				+"FROM procedurelog "
				+"INNER JOIN patient ON patient.PatNum=procedurelog.PatNum "
				+"INNER JOIN procedurecode ON procedurecode.CodeNum=procedurelog.CodeNum "
				+"LEFT JOIN ( "
					+"SELECT SUM(adjustment.AdjAmt) AdjAmt, adjustment.ProcNum "
					+"FROM adjustment "
					+"WHERE adjustment.ProcNum != 0 "
					+"GROUP BY ProcNum "
				+")adj ON adj.ProcNum=procedurelog.ProcNum "
				+"LEFT JOIN ( "
					+"SELECT claimproc.Status, SUM(CASE WHEN claimproc.Status = "+POut.Int((int)ClaimProcStatus.NotReceived)+" THEN claimproc.InsPayEst "
					+"WHEN claimproc.Status IN("+POut.Int((int)ClaimProcStatus.Received)+","+POut.Int((int)ClaimProcStatus.Supplemental)+") "
					+"THEN claimproc.InsPayAmt "
					+"ELSE 0 END) AS InsAmt, "
					+"SUM(IF(claimproc.Status!="+POut.Int((int)ClaimProcStatus.CapComplete)+",claimproc.WriteOff,0)) AS WriteOff, "
					+"SUM(IF(claimproc.Status="+POut.Int((int)ClaimProcStatus.CapComplete)+",claimproc.WriteOff,0)) AS CapWriteOff, "
					+"claimproc.ProcNum "
					+"FROM claimproc "
					+"WHERE claimproc.Status IN("+POut.Int((int)ClaimProcStatus.NotReceived)+","
						+POut.Int((int)ClaimProcStatus.Received)+","
						+POut.Int((int)ClaimProcStatus.Supplemental)+","
						+POut.Int((int)ClaimProcStatus.CapComplete)+") "
					+"GROUP BY claimproc.ProcNum "
				+")cp ON cp.ProcNum=procedurelog.ProcNum "
				+"LEFT JOIN ( "
					+"SELECT SUM(paysplit.SplitAmt) SplitAmt, paysplit.ProcNum "
					+"FROM paysplit "
					+"WHERE paysplit.ProcNum != 0 "
					+"GROUP BY paysplit.ProcNum "
				+")pay ON pay.ProcNum=procedurelog.ProcNum "
				+"WHERE procedurelog.ProcStatus=2 "
				+"AND procedurelog.ProcDate BETWEEN "+POut.Date(dateFrom)+" AND  "+POut.Date(dateTo)+" ";
				if(hasClinicsEnabled && listClinicNums.Count>0) {
					query+="AND procedurelog.ClinicNum IN("+string.Join(",",listClinicNums.Select(x => POut.Long(x)))+") ";
				}
				query+="HAVING Uncollected > 0.005 "
				+"ORDER BY procedurelog.ProcDate,patient.LName,patient.FName,procedurecode.ProcCode";
			return ReportsComplex.RunFuncOnReportServer(() => Db.GetTable(query));
		}
	}
}
