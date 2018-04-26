using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness {
	public class RpInsAging {
			public static DataTable GetInsAgingTable(DateTime asOfDate,bool isGroupByFam,AgeOfAccount accountAge,
				bool hasBillTypesAll,bool hasProvAll,List<long> listProv,List<long> listClinicNums,List<long> listBillType) 
			{			
				if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
					return Meth.GetTable(MethodBase.GetCurrentMethod(),asOfDate,isGroupByFam,
						accountAge,hasBillTypesAll,hasProvAll,listProv,listClinicNums,listBillType);
				}
				//ins aging---------------------------------------------------------------------------
				if(asOfDate.Year<1880) {
					asOfDate=DateTime.Today;
				}
				string asOfDateStr=POut.Date(asOfDate);
				string thirtyDaysAgo=POut.Date(asOfDate.AddDays(-30));
				string sixtyDaysAgo=POut.Date(asOfDate.AddDays(-60));
				string ninetyDaysAgo=POut.Date(asOfDate.AddDays(-90));
				string command="SELECT patient.PatNum, ";
				if(ReportsComplex.RunFuncOnReportServer(() =>(Prefs.GetBoolNoCache(PrefName.ReportsShowPatNum)))){
					command+=DbHelper.Concat("patient.PatNum","' - '","patient.LName","', '","patient.FName","' '","patient.MiddleI");
				}
				else{
					command+=DbHelper.Concat("patient.LName","', '","patient.FName","' '","patient.MiddleI");
				}
				command+=@"patName,
				guarAging.InsEst0_30 AS InsBal_0_30,
				guarAging.InsEst31_60 AS InsBal_31_60,
				guarAging.InsEst61_90 AS InsBal_61_90,
				guarAging.InsEst90 AS InsBal_90,
				guarAging.InsPayEst AS InsBalTotal 
				FROM (
					SELECT tSums.PatNum,"//if grouped by guar, this is the guar's PatNum; if grouped by patient.PatNum rows are individual pats
					+@"tSums.InsPayEst InsPayEst, 
					tSums.InsOver90 InsEst90, 
					tSums.Ins61_90 InsEst61_90, 
					tSums.Ins31_60 InsEst31_60, 
					tSums.Ins0_30 InsEst0_30 
					FROM(";
					command+="SELECT "+(isGroupByFam?"p.Guarantor PatNum,":"trans.PatNum,");
					command+="SUM(CASE WHEN trans.TranDate <= "+asOfDateStr+@" AND trans.InsPayEst != 0 THEN trans.InsPayEst ELSE 0 END) InsPayEst,
					SUM(CASE WHEN trans.TranDate < "+ninetyDaysAgo+@" THEN IFNULL(trans.InsPayEst,0) ELSE 0 END) InsOver90,
					SUM(CASE WHEN trans.TranDate < "+sixtyDaysAgo+@" AND trans.TranDate >= "+ninetyDaysAgo+@" THEN IFNULL(trans.InsPayEst,0) ELSE 0 END) Ins61_90,
					SUM(CASE WHEN trans.TranDate < "+thirtyDaysAgo+@" AND trans.TranDate >= "+sixtyDaysAgo+@" THEN IFNULL(trans.InsPayEst,0) ELSE 0 END) Ins31_60,
					SUM(CASE WHEN trans.TranDate <= "+asOfDateStr+@" AND trans.TranDate >= "+thirtyDaysAgo+@" THEN IFNULL(trans.InsPayEst,0) ELSE 0 END) Ins0_30 
					FROM(";
				#region Regular Claimproc By DateCP
					command+=@" 
						SELECT cp.ProcDate TranDate,
						(CASE WHEN cp.ProcDate <= "+asOfDateStr + @"
							AND (cp.Status = "+(int)ClaimProcStatus.NotReceived+@" OR (cp.Status = "+(int)ClaimProcStatus.Received+" AND cp.DateCP > "+asOfDateStr+@")) 
							THEN cp.InsPayEst ELSE 0 END) InsPayEst,
						cp.PatNum
						FROM claimproc cp 
						WHERE cp.status IN ("+(int)ClaimProcStatus.NotReceived+","+(int)ClaimProcStatus.Received+") ";
			#endregion Regular Claimproc By DateCP
			command+=") trans ";
				if(isGroupByFam) {
					command+="INNER JOIN patient p ON p.PatNum=trans.PatNum "
						+"GROUP BY p.Guarantor";
					if(!PrefC.GetBool(PrefName.AgingIsEnterprise)) {//only if for one fam or if not using famaging table
						command+=" ORDER BY NULL";
					}
				}	
				else {
					command+="GROUP BY trans.PatNum";
				}
				command+=") tSums"
				+") guarAging "
				+"INNER JOIN patient ON patient.PatNum=guarAging.PatNum ";
				command+="WHERE TRUE ";
				if(listBillType.Count>0){//if all bill types is selected, list will be empty
					command+="AND patient.BillingType IN ("+string.Join(",",listBillType.Select(x => POut.Long(x)))+") ";
				}
				if(listProv.Count>0) {//if all provs is selected, list will be empty
					command+="AND patient.PriProv IN ("+string.Join(",",listProv.Select(x => POut.Long(x)))+") ";
				}
				if(listClinicNums.Count>0) {
					//listClin may contain "Unassigned" clinic with ClinicNum 0, in which case it will also be in the query string
					command+="AND patient.ClinicNum IN ("+string.Join(",",listClinicNums.Select(x => POut.Long(x)))+") ";
				}
				command+="AND (guarAging.InsEst0_30 > 0.005 OR guarAging.InsEst31_60 > 0.005 OR guarAging.InsEst61_90 > 0.005 OR guarAging.InsEst90 > 0.005 "
					+"OR guarAging.InsPayEst > 0.005) ";
				command+="ORDER BY patient.LName,patient.FName";
				DataTable insTable = ReportsComplex.RunFuncOnReportServer(() => Db.GetTable(command));
			//-- regular aging table --------------------------------------------------
				bool isOnlyNeg=false;
				bool isWoAged=true;
				bool isIncludeNeg=true;
				bool hasDateLastPay=false;
				bool isExcludeInactive=false;
				bool isExcludeBadAddress=false;
				bool isExcludeArchived=false;
				bool isOnlyInsNoBal=false;
				bool isIncludeInsNoBal=false;
				bool? isForceAgeNegAdj=null;
				bool isForInsAging=true;
				bool doAgePatPayPlanPayments=false;
				DataTable regAging = ReportsComplex.RunFuncOnReportServer(() => RpAging.GetAgingTable(asOfDate,isWoAged,hasDateLastPay,isGroupByFam,isOnlyNeg,
					AgeOfAccount.Any,isIncludeNeg,isExcludeInactive,isExcludeBadAddress,listProv,listClinicNums,listBillType,isExcludeArchived,isIncludeInsNoBal,
					isOnlyInsNoBal,isForceAgeNegAdj,doAgePatPayPlanPayments,isForInsAging));
			//------------ Combine Tables ---------------------------------------------
			DataTable insAgingTable = new DataTable();
			//define columns here
			insAgingTable.Columns.Add("PatName");
			insAgingTable.Columns.Add("InsBal_0_30");
			insAgingTable.Columns.Add("InsBal_31_60");
			insAgingTable.Columns.Add("InsBal_61_90");
			insAgingTable.Columns.Add("InsBal_90");
			insAgingTable.Columns.Add("InsBal_Total");
			insAgingTable.Columns.Add("PatBal_0_30");
			insAgingTable.Columns.Add("PatBal_31_60");
			insAgingTable.Columns.Add("PatBal_61_90");
			insAgingTable.Columns.Add("PatBal_90");
			insAgingTable.Columns.Add("PatBal_Total");
			insAgingTable.Columns.Add("PatNum"); //this will not show, for correlating the two (ins and pat) aging tables to each other.
			List<DataRow> listInsAgingRows = new List<DataRow>();
			//loop through the insurance aging table
			foreach(DataRow insRow in insTable.Rows) {
				//create a new row with the structure of the new table
				DataRow newRow=insAgingTable.NewRow();
				//copy the insurance aging table's values over to the new row. (also fill the patient bal columns with -(insurance estimate) for the appropriate bucket.)
				newRow["PatNum"]=insRow["patNum"];
				newRow["PatName"]=insRow["patName"];
				newRow["InsBal_0_30"]=insRow["InsBal_0_30"];
				newRow["InsBal_31_60"]=insRow["InsBal_31_60"];
				newRow["InsBal_61_90"]=insRow["InsBal_61_90"];
				newRow["InsBal_90"]=insRow["InsBal_90"];
				newRow["InsBal_Total"]=insRow["InsBalTotal"];
				newRow["PatBal_0_30"]=-PIn.Double(insRow["InsBal_0_30"].ToString());
				newRow["PatBal_31_60"]=-PIn.Double(insRow["InsBal_31_60"].ToString());
				newRow["PatBal_61_90"]=-PIn.Double(insRow["InsBal_61_90"].ToString());
				newRow["PatBal_90"]=-PIn.Double(insRow["InsBal_90"].ToString());
				newRow["PatBal_Total"]=-PIn.Double(insRow["InsBalTotal"].ToString());
				listInsAgingRows.Add(newRow);
			}
//-------------------------------------------------------------------------------------------------------------------------------------
			//loop through rows in the regular aging table
			foreach(DataRow row in regAging.Rows) {
				DataRow insAgingRow = listInsAgingRows.FirstOrDefault(x => x["PatNum"].ToString() == row["PatNum"].ToString());
				//check to see if that patient exists in the insurance aging report
				if(insAgingRow!=null) {
					insAgingRow["PatBal_0_30"]=PIn.Double(insAgingRow["PatBal_0_30"].ToString()) + PIn.Double(row["Bal_0_30"].ToString());
					insAgingRow["PatBal_31_60"]=PIn.Double(insAgingRow["PatBal_31_60"].ToString()) + PIn.Double(row["Bal_31_60"].ToString());
					insAgingRow["PatBal_61_90"]=PIn.Double(insAgingRow["PatBal_61_90"].ToString()) + PIn.Double(row["Bal_61_90"].ToString());
					insAgingRow["PatBal_90"]=PIn.Double(insAgingRow["PatBal_90"].ToString()) + PIn.Double(row["BalOver90"].ToString());
					insAgingRow["PatBal_Total"]=PIn.Double(insAgingRow["PatBal_Total"].ToString()) + PIn.Double(row["BalTotal"].ToString());
				}
				else {//if it doesn't create a new row with 0.00 insurance values and fill the patient aging values.
					DataRow newRow=insAgingTable.NewRow();
					newRow["PatName"]=row["patName"];
					newRow["InsBal_0_30"]=PIn.Double("0.00");
					newRow["InsBal_31_60"]=PIn.Double("0.00");
					newRow["InsBal_61_90"]=PIn.Double("0.00");
					newRow["InsBal_90"]=PIn.Double("0.00");
					newRow["InsBal_Total"]=PIn.Double("0.00");
					newRow["PatBal_0_30"]=PIn.Double(row["Bal_0_30"].ToString());
					newRow["PatBal_31_60"]=PIn.Double(row["Bal_31_60"].ToString());
					newRow["PatBal_61_90"]=PIn.Double(row["Bal_61_90"].ToString());
					newRow["PatBal_90"]=PIn.Double(row["BalOver90"].ToString());
					newRow["PatBal_Total"]=PIn.Double(row["BalTotal"].ToString());
					listInsAgingRows.Add(newRow);
				}
			}
			listInsAgingRows=listInsAgingRows.OrderBy(x => x["PatName"]).ToList();
			foreach(DataRow rowCur in listInsAgingRows) {
				if(accountAge == AgeOfAccount.Any) {
					insAgingTable.Rows.Add(rowCur);
				}
				else if(accountAge == AgeOfAccount.Over30) {
					if(PIn.Double(rowCur["PatBal_31_60"].ToString()) != 0
						|| PIn.Double(rowCur["InsBal_31_60"].ToString()) != 0
						|| PIn.Double(rowCur["PatBal_61_90"].ToString()) != 0
						|| PIn.Double(rowCur["InsBal_61_90"].ToString()) != 0
						|| PIn.Double(rowCur["PatBal_90"].ToString()) != 0
						|| PIn.Double(rowCur["InsBal_90"].ToString()) != 0) 
					{
						insAgingTable.Rows.Add(rowCur);
					}
				}
				else if(accountAge == AgeOfAccount.Over60) {
					if(PIn.Double(rowCur["PatBal_61_90"].ToString()) != 0
						|| PIn.Double(rowCur["InsBal_61_90"].ToString()) != 0
						|| PIn.Double(rowCur["PatBal_90"].ToString()) != 0
						|| PIn.Double(rowCur["InsBal_90"].ToString()) != 0) 
					{
						insAgingTable.Rows.Add(rowCur);
					}
				}
				else {
					if(PIn.Double(rowCur["PatBal_90"].ToString()) != 0
						|| PIn.Double(rowCur["InsBal_90"].ToString()) != 0) 
					{
						insAgingTable.Rows.Add(rowCur);
					}
				}
			}
			return insAgingTable;
		}
	}
}
