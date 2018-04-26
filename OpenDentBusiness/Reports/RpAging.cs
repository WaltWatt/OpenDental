using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness {
	public class RpAging {
		public static DataTable GetAgingTable(DateTime asOfDate,bool isWoAged,bool hasDateLastPay,bool isGroupByFam,bool isOnlyNeg,AgeOfAccount accountAge,
			bool isIncludeNeg,bool isExcludeInactive,bool isExcludeBadAddress,List<long> listProv,List<long> listClinicNums,List<long> listBillType,
			bool isExcludeArchive,bool isIncludeInsNoBal,bool isOnlyInsNoBal,bool? isForceAgeNegAdj,bool isForInsAging,bool doAgePatPayPlanPayments) 
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),asOfDate,isWoAged,hasDateLastPay,isGroupByFam,isOnlyNeg,accountAge,isIncludeNeg,
					isExcludeInactive,isExcludeBadAddress,listProv,listClinicNums,listBillType,isExcludeArchive,isIncludeInsNoBal,isOnlyInsNoBal,
					isForceAgeNegAdj,isForInsAging,doAgePatPayPlanPayments);
			}
			string queryAg=GetQueryString(asOfDate,isWoAged,hasDateLastPay,isGroupByFam,isOnlyNeg,accountAge,isIncludeNeg,isExcludeInactive,
				isExcludeBadAddress,listProv,listClinicNums,listBillType,isExcludeArchive,isIncludeInsNoBal,isOnlyInsNoBal,isForceAgeNegAdj,isForInsAging
				,doAgePatPayPlanPayments);
			return ReportsComplex.RunFuncOnReportServer(() => Db.GetTable(queryAg));
		}

		public static string GetQueryString(DateTime asOfDate,bool isWoAged,bool hasDateLastPay,bool isGroupByFam,bool isOnlyNeg,AgeOfAccount accountAge,
			bool isIncludeNeg,bool isExcludeInactive,bool isExcludeBadAddress,List<long> listProv,List<long> listClinicNums,List<long> listBillType,
			bool isExcludeArchive,bool isIncludeInsNoBal,bool isOnlyInsNoBal,bool? isForceAgeNegAdj,bool isForInsAging,bool doAgePatPayPlanPayments) 
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),asOfDate,isWoAged,hasDateLastPay,isGroupByFam,isOnlyNeg,accountAge,isIncludeNeg,
					isExcludeInactive,isExcludeBadAddress,listProv,listClinicNums,listBillType,isExcludeArchive,isIncludeInsNoBal,isOnlyInsNoBal,
					isForceAgeNegAdj,isForInsAging,doAgePatPayPlanPayments);
			}
			//patient aging---------------------------------------------------------------------------
			//The aging report always shows historical numbers based on the date entered.
			//the selected columns have to remain in this order due to the way the report complex populates the returned sheet
			string queryAg = "SELECT ";
			if(isForInsAging) { //get patNum for insAgingReport only
				queryAg+="patient.PatNum, ";
			}
			if(ReportsComplex.RunFuncOnReportServer(() => (Prefs.GetBoolNoCache(PrefName.ReportsShowPatNum)))) {
				queryAg+=DbHelper.Concat("patient.PatNum","' - '","patient.LName","', '","patient.FName","' '","patient.MiddleI");
			}
			else {
				queryAg+=DbHelper.Concat("patient.LName","', '","patient.FName","' '","patient.MiddleI");
			}
			queryAg+="patName,guarAging.Bal_0_30,guarAging.Bal_31_60,guarAging.Bal_61_90,guarAging.BalOver90,guarAging.BalTotal,"
				+"guarAging.InsWoEst,guarAging.InsPayEst,guarAging.BalTotal-guarAging.InsPayEst-guarAging.InsWoEst AS ";
			if(DataConnection.DBtype==DatabaseType.MySql) {
				queryAg+="$pat";
			}
			else { //Oracle needs quotes.
				queryAg+="\"$pat\"";
			}
			bool isHistoric=(asOfDate.Date!=DateTime.Today);
			bool isInsPayWoCombined=false;
			//Must select "blankCol" for use with reportComplex to fix spacing of final column
			queryAg+=(hasDateLastPay ? ",'' blankCol,guarAging.DateLastPay " : " ")
				+"FROM ("
					+ReportsComplex.RunFuncOnReportServer(() => Ledgers.GetAgingQueryString(asOfDate,null,isHistoric,isInsPayWoCombined,hasDateLastPay,
						isGroupByFam,isWoAged,isForceAgeNegAdj,doAgePatPayPlanPayments))
				+") guarAging "
				+"INNER JOIN patient ON patient.PatNum=guarAging.PatNum "
				+"WHERE ";
			List<string> listAgeOrs=new List<string>();
			if(isIncludeNeg || isOnlyNeg) {
				listAgeOrs.Add("guarAging.BalTotal < -0.005");
			}
			if(isIncludeInsNoBal || isOnlyInsNoBal) {
				listAgeOrs.Add("((guarAging.InsPayEst > 0.005 OR guarAging.InsWoEst > 0.005) AND guarAging.Bal_0_30 < 0.005 AND guarAging.Bal_31_60 < 0.005 "
					+"AND guarAging.Bal_61_90 < 0.005 AND guarAging.BalOver90 < 0.005)");
			}
			if(!isOnlyNeg && !isOnlyInsNoBal) {
				if(accountAge<=AgeOfAccount.Over90) {
					listAgeOrs.Add("guarAging.BalOver90 > 0.005");
				}
				if(accountAge<=AgeOfAccount.Over60) {
					listAgeOrs.Add("guarAging.Bal_61_90 > 0.005");
				}
				if(accountAge<=AgeOfAccount.Over30) {
					listAgeOrs.Add("guarAging.Bal_31_60 > 0.005");
				}
				if(accountAge<=AgeOfAccount.Any) {
					listAgeOrs.Add("guarAging.Bal_0_30 > 0.005");
				}
			}
			queryAg+="("+string.Join(" OR ",listAgeOrs)+") ";
			if(isExcludeInactive) {
				queryAg+="AND patient.PatStatus != "+ (int)PatientStatus.Inactive + " ";
			}
			if(isExcludeArchive) {
				queryAg+="AND patient.PatStatus != "+ (int)PatientStatus.Archived + " ";
			}
			if(isExcludeBadAddress) {
				queryAg+="AND patient.Zip != '' ";
			}
			if(listBillType.Count>0) {//if all bill types is selected, list will be empty
				queryAg+="AND patient.BillingType IN ("+string.Join(",",listBillType.Select(x => POut.Long(x)))+") ";
			}
			if(listProv.Count>0) {//if all provs is selected, list will be empty
				queryAg+="AND patient.PriProv IN ("+string.Join(",",listProv.Select(x => POut.Long(x)))+") ";
			}
			if(ReportsComplex.RunFuncOnReportServer(() => (!Prefs.GetBoolNoCache(PrefName.EasyNoClinics)))) {//validated to have at least one clinic selected if clinics are enabled above
																																																			 //listClin may contain "Unassigned" clinic with ClinicNum 0, in which case it will also be in the query string
				queryAg+="AND patient.ClinicNum IN ("+string.Join(",",listClinicNums.Select(x => POut.Long(x)))+") ";
			}
			queryAg+="ORDER BY patient.LName,patient.FName";
			return queryAg;
		}

	}

	public enum AgeOfAccount {
		///<summary>0</summary>
		Any,
		///<summary>1</summary>
		Over30,
		///<summary>2</summary>
		Over60,
		///<summary>3</summary>
		Over90,
	}
}
