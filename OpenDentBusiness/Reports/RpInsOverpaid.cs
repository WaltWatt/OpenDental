using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness {
	public class RpInsOverpaid {
		///<summary>If not using clinics then supply an empty list of clinicNums.  listClinicNums must have at least one item if using clinics.</summary>
		public static DataTable GetInsuranceOverpaid(DateTime dateStart,DateTime dateEnd,List<long> listClinicNums,bool groupByProc) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateStart,dateEnd,listClinicNums,groupByProc);
			}
			string query=@"SELECT "+DbHelper.Concat("patient.LName","', '","patient.FName")+@" patname,
				procs.ProcDate,procs.fee ""$sumfee"",cp.PayAmt ""$PaidAndWriteoff""
				FROM (
					SELECT MIN(procedurelog.ProcNum) ProcNum,MIN(procedurelog.PatNum) PatNum,MIN(procedurelog.ProcDate) ProcDate,
					SUM(procedurelog.ProcFee*(procedurelog.UnitQty+procedurelog.BaseUnits)) fee
					FROM procedurelog
					WHERE procedurelog.ProcDate BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateEnd)+@"
					AND procedurelog.ProcStatus="+POut.Int((int)ProcStat.C)+@"
					AND procedurelog.ProcFee>0 ";/*Negative proc fees should not show up on this report.*/  
																		  /*We have one office that uses negative proc fees as internal adjustments*/
			if(listClinicNums.Count>0) {
				query+="AND procedurelog.ClinicNum IN ("+string.Join(",",listClinicNums)+") ";
			}
			if(groupByProc) {
				query+="GROUP BY procedurelog.ProcNum ";
			}
			else {//Group by patient, proc date
				query+="GROUP BY procedurelog.PatNum,procedurelog.ProcDate ";
			}
			query+=@") procs
				INNER JOIN (
					SELECT MIN(claimproc.ProcNum) ProcNum,MIN(claimproc.PatNum) PatNum,MIN(claimproc.ProcDate) ProcDate,
					SUM(claimproc.InsPayAmt+claimproc.Writeoff) PayAmt
					FROM claimproc
					WHERE claimproc.Status IN ("
					+POut.Int((int)ClaimProcStatus.Received)+","
					+POut.Int((int)ClaimProcStatus.Supplemental)+","
					+POut.Int((int)ClaimProcStatus.CapClaim)+","
					+POut.Int((int)ClaimProcStatus.CapComplete)+@")
					AND claimproc.ProcDate BETWEEN "+POut.Date(dateStart)+" AND "+POut.Date(dateEnd)+" ";
			if(groupByProc) {
				query+="GROUP BY claimproc.ProcNum ";
			}
			else {//Group by patient, proc date
				query+="GROUP BY claimproc.PatNum,claimproc.ProcDate ";
			}
			query+=@"HAVING SUM(claimproc.InsPayAmt+claimproc.Writeoff)>0"/*ProcFee must be >0 and PayAmt must be >ProcFee, ergo PayAmt must be >0*/+@"
					ORDER BY NULL
				) cp ON ";
			if(groupByProc) {
				query+="cp.ProcNum=procs.ProcNum ";
			}
			else {//Group by patient, proc date
				query+=@"cp.PatNum=procs.PatNum
					AND cp.ProcDate=procs.ProcDate ";
			}
			query+=@"INNER JOIN patient ON patient.PatNum=procs.PatNum
				WHERE ROUND(procs.fee,3) < ROUND(cp.PayAmt,3)
				ORDER BY patient.LName,patient.FName,procs.ProcDate ";
			return ReportsComplex.RunFuncOnReportServer(() => Db.GetTable(query));
		}
		
	}
}
