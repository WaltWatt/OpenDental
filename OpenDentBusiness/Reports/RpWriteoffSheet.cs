using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness {
	public class RpWriteoffSheet {
		///<summary>If not using clinics then supply an empty list of clinicNums.</summary>
		public static DataTable GetWriteoffTable(DateTime dateStart,DateTime dateEnd,List<long> listProvNums,List<long> listClinicNums
			,bool hasAllClinics,bool hasClinicsEnabled,bool hasWriteoffPay) 
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateStart,dateEnd,listProvNums,listClinicNums,hasAllClinics
					,hasClinicsEnabled,hasWriteoffPay);
			}
			string whereProv="";
			if(listProvNums.Count > 0) {
				whereProv+=" AND claimproc.ProvNum IN("+string.Join(",",listProvNums)+") ";
			}
			string whereClin="";
			if(hasClinicsEnabled && listClinicNums.Count > 0) {//Using clinics
				whereClin+=" AND claimproc.ClinicNum IN("+string.Join(",",listClinicNums)+") ";
			}
			string query="SET @FromDate="+POut.Date(dateStart)+", @ToDate="+POut.Date(dateEnd)+";";
			if(hasWriteoffPay) {
				query+="SELECT "+DbHelper.DtimeToDate("claimproc.DateCP")+" date,"
					+DbHelper.Concat("patient.LName","', '","patient.FName","' '","patient.MiddleI")+","
					+"carrier.CarrierName,"
					+"provider.Abbr,";
				if(hasClinicsEnabled) {
					query+="clinic.Description,";
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					query+="SUM(claimproc.WriteOff) $amount,";
				}
				else {//Oracle needs quotes.
					query+="SUM(claimproc.WriteOff) \"$amount\",";
				}
				query+="claimproc.ClaimNum "
					+"FROM claimproc "//,insplan,patient,carrier,provider "
					+"LEFT JOIN insplan ON claimproc.PlanNum = insplan.PlanNum "
					+"LEFT JOIN patient ON claimproc.PatNum = patient.PatNum "
					+"LEFT JOIN carrier ON carrier.CarrierNum = insplan.CarrierNum "
					+"LEFT JOIN provider ON provider.ProvNum = claimproc.ProvNum "
					+"LEFT JOIN clinic ON clinic.ClinicNum=claimproc.ClinicNum "
					+"WHERE (claimproc.Status=1 OR claimproc.Status=4) " /*received or supplemental*/
					+whereProv
					+whereClin
					+"AND claimproc.DateCP >= @FromDate "
					+"AND claimproc.DateCP <= @ToDate "
					+"AND claimproc.WriteOff > 0 "
					+"GROUP BY claimproc.ProvNum,claimproc.DateCP,claimproc.ClinicNum,claimproc.PatNum "
					+"ORDER BY claimproc.DateCP,claimproc.PatNum";
			}
			else{//using procedure date
				query+="SELECT "+DbHelper.DtimeToDate("claimproc.ProcDate")+" date, "
					+DbHelper.Concat("patient.LName","', '","patient.FName","' '","patient.MiddleI")+", "
					+"carrier.CarrierName, "
					+"provider.Abbr,";
				if(hasClinicsEnabled) {
					query+="clinic.Description,";
				}
				if(DataConnection.DBtype==DatabaseType.MySql) {
					query+="SUM(claimproc.WriteOff) $amount ";
				}
				else {//Oracle needs quotes.
					query+="SUM(claimproc.WriteOff) \"$amount\" ";
				}
				query+="FROM claimproc "//,insplan,patient,carrier,provider "
					+"LEFT JOIN insplan ON claimproc.PlanNum = insplan.PlanNum "
					+"LEFT JOIN patient ON claimproc.PatNum = patient.PatNum "
					+"LEFT JOIN carrier ON carrier.CarrierNum = insplan.CarrierNum "
					+"LEFT JOIN provider ON provider.ProvNum = claimproc.ProvNum "
					+"LEFT JOIN clinic ON clinic.ClinicNum=claimproc.ClinicNum "
					+"WHERE (claimproc.Status=1 OR claimproc.Status=4 OR claimproc.Status=0) " /*received or supplemental or notreceived*/
					+whereProv
					+whereClin
					+"AND claimproc.ProcDate >= @FromDate "
					+"AND claimproc.ProcDate <= @ToDate "
					+"AND claimproc.WriteOff > 0 "
					+"GROUP BY claimproc.ProvNum,claimproc.ProcDate,claimproc.ClinicNum,claimproc.PatNum "
					+"ORDER BY claimproc.ProcDate,claimproc.PatNum";
			}
			return ReportsComplex.RunFuncOnReportServer(() => ReportsComplex.GetTable(query));
		}	
	}

}
