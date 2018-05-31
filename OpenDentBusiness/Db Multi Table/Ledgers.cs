using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDentBusiness{

	///<summary>This does not correspond to any table in the database.  It works with a variety of tables to calculate aging.</summary>
	public class Ledgers {

		///<summary>Returns a rough guess on how long RunAging() will take in milliseconds based on the amount of data within certain tables that are used to compute aging.</summary>
		public static double GetAgingComputationTime() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetDouble(MethodBase.GetCurrentMethod());
			}
			//Factor of 0.0042680625638876 was discovered by timing aging on a large database.  It proved to be very accurate when tested on other databases.
			//A large database with 6091757 rows in the following tables took on average 26 seconds (26000 ms) to run aging.  26000(ms) / 6091757(rows) = 0.0042680625638876
			string command=@"SELECT ((SELECT COUNT(*) FROM patient)
				+ (SELECT COUNT(*) FROM procedurelog)
				+ (SELECT COUNT(*) FROM paysplit)
				+ (SELECT COUNT(*) FROM adjustment)
				+ (SELECT COUNT(*) FROM claimproc)
				+ (SELECT COUNT(*) FROM payplan)
				+ (SELECT COUNT(*) FROM payplancharge)) * 0.0042680625638876 AgingInMilliseconds";
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				command+=" FROM dual";//Oracle requires a FROM clause be present.
			}
			return PIn.Double(Db.GetScalar(command));
		}

		///<summary>This runs aging for all patients.  If using monthly aging, it always just runs the aging as of the last date again.  If using daily aging, it runs it as of today.  This logic used to be in FormAging, but is now centralized.</summary>
		public static void RunAging() {
			//No need to check RemotingRole; no call to db.
			if(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)) {
				ComputeAging(0,PrefC.GetDate(PrefName.DateLastAging));
			}
			else {
				ComputeAging(0,DateTime.Today);
				if(PrefC.GetDate(PrefName.DateLastAging) != DateTime.Today) {
					Prefs.UpdateString(PrefName.DateLastAging,POut.Date(DateTime.Today,false));
					//Since this is always called from UI, the above line works fine to keep the prefs cache current.
				}
			}
		}

		///<summary>Computes aging for the family specified. Specify guarantor=0 in order to calculate aging for all families. Gets all info from db.
		///<para>The aging calculation will use the following rules within each family:</para>
		///<para>1) The aging "buckets" (0 to 30, 31 to 60, 61 to 90 and Over 90) ONLY include account activity on or before AsOfDate.</para>
		///<para>2) BalTotal includes all account activity, even future entries. If historical, BalTotal excludes entries after AsOfDate.</para>
		///<para>3) InsEst includes all insurance estimates, even future estimates. If historical, InsEst excludes ins est after AsOfDate.</para>
		///<para>4) PayPlanDue includes all payplan charges minus credits. If historical, PayPlanDue excludes charges and credits after AsOfDate.</para></summary>
		public static void ComputeAging(long guarantor,DateTime asOfDate) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),guarantor,asOfDate);
				return;
			}
			bool isMySqlDb=(DataConnection.DBtype==DatabaseType.MySql);
			List<long> listGuars=null;
			if(guarantor>0) {
				listGuars=new List<long>() { guarantor };
			}
			string command="";
			if(PrefC.GetBool(PrefName.AgingIsEnterprise)) {
				#region Using FamAging Table
				if(guarantor==0) {
					command="TRUNCATE TABLE famaging;"
						+"INSERT INTO famaging (PatNum,BalOver90,Bal_61_90,Bal_31_60,Bal_0_30,BalTotal,InsEst,PayPlanDue) "
						+"("
							+GetAgingQueryString(asOfDate)
							+(isMySqlDb?" HAVING BalOver90!=0 OR Bal_61_90!=0 OR Bal_31_60!=0 OR Bal_0_30!=0 OR BalTotal!=0 OR PayPlanDue!=0 OR InsEst!=0":"")
						+");"
					+"UPDATE patient p "
					+"LEFT JOIN famaging f ON p.PatNum=f.PatNum "//left join so non-guarantors and those with no transactions will be coalesced to 0
					+"SET "
					+"p.BalOver90  = COALESCE(f.BalOver90,0),"
					+"p.Bal_61_90  = COALESCE(f.Bal_61_90,0),"
					+"p.Bal_31_60  = COALESCE(f.Bal_31_60,0),"
					+"p.Bal_0_30   = COALESCE(f.Bal_0_30,0),"
					+"p.BalTotal   = COALESCE(f.BalTotal,0),"
					+"p.InsEst     = COALESCE(f.InsEst,0),"
					+"p.PayPlanDue = COALESCE(f.PayPlanDue,0);"
					+"TRUNCATE TABLE famaging;";
				}
				else {
					FamAging famAgingCur=Crud.FamAgingCrud.SelectOne(GetAgingQueryString(asOfDate,listGuars));
					command="UPDATE patient p SET "
						+"p.BalOver90 =CASE WHEN p.Guarantor!=p.PatNum THEN 0 ELSE "+famAgingCur.BalOver90 +" END,"
						+"p.Bal_61_90 =CASE WHEN p.Guarantor!=p.PatNum THEN 0 ELSE "+famAgingCur.Bal_61_90 +" END,"
						+"p.Bal_31_60 =CASE WHEN p.Guarantor!=p.PatNum THEN 0 ELSE "+famAgingCur.Bal_31_60 +" END,"
						+"p.Bal_0_30  =CASE WHEN p.Guarantor!=p.PatNum THEN 0 ELSE "+famAgingCur.Bal_0_30  +" END,"
						+"p.BalTotal  =CASE WHEN p.Guarantor!=p.PatNum THEN 0 ELSE "+famAgingCur.BalTotal  +" END,"
						+"p.InsEst    =CASE WHEN p.Guarantor!=p.PatNum THEN 0 ELSE "+famAgingCur.InsEst    +" END,"
						+"p.PayPlanDue=CASE WHEN p.Guarantor!=p.PatNum THEN 0 ELSE "+famAgingCur.PayPlanDue+" END "
						+"WHERE p.Guarantor="+guarantor;
				}
				#endregion Using FamAging Table
			}
			else {
				#region Not Using FamAging Table
				//If is for all patients, not single family, zero out all aged bals in order to catch former guarantors.  Zeroing out for a single family is
				//handled in the query below. (see the region "Get All Family PatNums")  Unioning is too slow for all patients, so run this statement first.
				//Added to the same query string to force Galera Cluster to process both queries on the same node to prevent a deadlock error.
				if(guarantor==0) {
					command="UPDATE patient SET "
					+"Bal_0_30   = 0,"
					+"Bal_31_60  = 0,"
					+"Bal_61_90  = 0,"
					+"BalOver90  = 0,"
					+"InsEst     = 0,"
					+"BalTotal   = 0,"
					+"PayPlanDue = 0;";
				}
				command+=(isMySqlDb?"UPDATE patient p,":"MERGE INTO patient p USING ")
					+"("+GetAgingGuarTransQuery(asOfDate,listGuars)+") famSums "
					+(isMySqlDb?"":"ON (p.Guarantor=famSums.PatNum) WHEN MATCHED THEN UPDATE ")
					//Update the patient table based on the family amounts summed from 'famSums', and distribute the payments into the oldest balances first.
					+"SET p.BalOver90=(CASE WHEN p.Guarantor != p.PatNum THEN 0 "//zero out non-guarantors
						+"ELSE ROUND(CASE WHEN famSums.TotalCredits >= famSums.ChargesOver90 THEN 0 "//over 90 day bal paid in full
						+"ELSE famSums.ChargesOver90-famSums.TotalCredits END,3) END),"//over 90 day bal partially paid or unpaid.
					+"p.Bal_61_90=(CASE WHEN p.Guarantor != p.PatNum THEN 0 "//zero out non-guarantors
						+"ELSE ROUND(CASE WHEN famSums.TotalCredits <= famSums.ChargesOver90 THEN famSums.Charges_61_90 "//61-90 day bal unpaid
						+"WHEN famSums.ChargesOver90+famSums.Charges_61_90 <= famSums.TotalCredits THEN 0 "//61-90 day bal paid in full
						+"ELSE famSums.ChargesOver90+famSums.Charges_61_90-famSums.TotalCredits END,3) END),"//61-90 day bal partially paid
					+"p.Bal_31_60=(CASE WHEN p.Guarantor != p.PatNum THEN 0 "//zero out non-guarantors
						+"ELSE ROUND(CASE WHEN famSums.TotalCredits < famSums.ChargesOver90+famSums.Charges_61_90 "
						+"THEN famSums.Charges_31_60 "//31-60 day bal unpaid
						+"WHEN famSums.ChargesOver90+famSums.Charges_61_90+famSums.Charges_31_60 <= famSums.TotalCredits THEN 0 "//31-60 day bal paid in full
						+"ELSE famSums.ChargesOver90+famSums.Charges_61_90+famSums.Charges_31_60-famSums.TotalCredits END,3) END),"//31-60 day bal partially paid
					+"p.Bal_0_30=(CASE WHEN p.Guarantor != p.PatNum THEN 0 "//zero out non-guarantors
						+"ELSE ROUND(CASE WHEN famSums.TotalCredits < famSums.ChargesOver90+famSums.Charges_61_90+famSums.Charges_31_60 "
						+"THEN famSums.Charges_0_30 "//0-30 day bal unpaid
						+"WHEN famSums.ChargesOver90+famSums.Charges_61_90+famSums.Charges_31_60+famSums.Charges_0_30 <= famSums.TotalCredits "
						+"THEN 0 "//0-30 day bal paid in full
						+"ELSE famSums.ChargesOver90+famSums.Charges_61_90+famSums.Charges_31_60+famSums.Charges_0_30-famSums.TotalCredits "
						+"END,3) END),"//0-30 day bal partially paid
					+"p.BalTotal=(CASE WHEN p.Guarantor != p.PatNum THEN 0 "//zero out non-guarantors
						+"ELSE ROUND(famSums.BalTotal,3) END),"
					+"p.InsEst=(CASE WHEN p.Guarantor != p.PatNum THEN 0 "//zero out non-guarantors
						+"ELSE ROUND(famSums.InsPayEst+famSums.InsWoEst,3) END),"
					+"p.PayPlanDue=(CASE WHEN p.Guarantor != p.PatNum THEN 0 "//zero out non-guarantors
						+"ELSE ROUND(famSums.PayPlanDue,3) END)"
					+(isMySqlDb?" WHERE p.Guarantor=famSums.PatNum":"");//Aging calculations only apply to guarantors, zero out non-guarantor bals
				#endregion Not Using FamAging Table
			}
			Db.NonQ(command);
		}

		///<summary>Generates a dictionary where the Key:PatNum and Val:FamilyBalance for passed-in guarantors.</summary>
		public static Dictionary<long,double> GetBalancesForFamilies(List<long> listGuarantorNums) {
			string command = GetAgingQueryString(DateTime.Today,listGuarantorNums);
			return Db.GetTable(command).Rows.OfType<DataRow>().ToDictionary(x => PIn.Long(x["PatNum"].ToString()),y => PIn.Double(y["BalTotal"].ToString()));
		}

		///<summary>Returns a query string for selecting the guarantor and aged bals with InsEst and PayPlanDue.
		///<para>For enterprise aging: isAllPats=>the results are used to populate the famaging table; !isAllPats=>returns 1 row used to update family.</para>
		///If !isWOAged, query results will exactly match what the patient table would be updated to if aging was run with the same settings and writeoff
		///estimates will be in a separate column, optionally combined with InsPayEst.
		///<para>If isWOAged then the writeoff estimates are aged and retrieved based on the value of woAgedType as follows:
		///WriteoffNotAged: Writeoffs not aged, reported in separate column 'InsWoEst';
		///WriteoffLive: Writeoff estimates from claimprocs;
		///WriteoffOrig: Writeoff estimates from claimsnapshot if exists, otherwise writeoff estimates not aged;
		///WriteoffPreferOrig: Writeoff estimates from claimsnapshot if exists, otherwise from claimproc.</para></summary>
		public static string GetAgingQueryString(DateTime asOfDate,List<long> listGuarantors=null,bool isHistoric=false,bool isInsPayWoCombined=true,
			bool hasDateLastPay=false,bool isGroupByGuar=true,bool isWoAged=false,bool? isForceAgeNegAdj=null,bool doAgePatPayPlanPayments=false)
		{
			//No need to check RemotingRole; no call to db.
			//Returns family amounts summed from 'tSums', with payments distributed into the oldest balances first.
			string command="SELECT tSums.PatNum,"//if grouped by guar, this is the guar's PatNum; if grouped by patient.PatNum rows are individual pats
				+"ROUND(CASE WHEN tSums.TotalCredits >= tSums.ChargesOver90 THEN 0 "//over 90 day paid in full
					+"ELSE tSums.ChargesOver90 - tSums.TotalCredits END,3) BalOver90,"//over 90 day partially paid or unpaid.
				+"ROUND(CASE WHEN tSums.TotalCredits <= tSums.ChargesOver90 THEN tSums.Charges_61_90 "//61-90 day unpaid
					+"WHEN tSums.ChargesOver90 + tSums.Charges_61_90 <= tSums.TotalCredits THEN 0 "//61-90 day paid in full
					+"ELSE tSums.ChargesOver90 + tSums.Charges_61_90 - tSums.TotalCredits END,3) Bal_61_90,"//61-90 day partially paid
				+"ROUND(CASE WHEN tSums.TotalCredits < tSums.ChargesOver90 + tSums.Charges_61_90 THEN tSums.Charges_31_60 "//31-60 day unpaid
					+"WHEN tSums.ChargesOver90 + tSums.Charges_61_90 + tSums.Charges_31_60 <= tSums.TotalCredits THEN 0 "//31-60 day paid in full
					+"ELSE tSums.ChargesOver90 + tSums.Charges_61_90 + tSums.Charges_31_60 - tSums.TotalCredits END,3) Bal_31_60,"//31-60 day partially paid
				+"ROUND(CASE WHEN tSums.TotalCredits < tSums.ChargesOver90 + tSums.Charges_61_90 + tSums.Charges_31_60 THEN tSums.Charges_0_30 "//0-30 day unpaid
					+"WHEN tSums.ChargesOver90 + tSums.Charges_61_90 + tSums.Charges_31_60 + tSums.Charges_0_30 <= tSums.TotalCredits THEN 0 "//0-30 day paid in full
					+"ELSE tSums.ChargesOver90 + tSums.Charges_61_90 + tSums.Charges_31_60 + tSums.Charges_0_30 - tSums.TotalCredits END,3) Bal_0_30,"//0-30 day partially paid
				+"ROUND(tSums.BalTotal,3) BalTotal,";
			if(isInsPayWoCombined) {
				command+="ROUND(tSums.InsPayEst+tSums.InsWoEst,3) InsEst,";
			}
			else {
				command+="ROUND(tSums.InsWoEst,3) InsWoEst,"
					+"ROUND(tSums.InsPayEst,3) InsPayEst,";
			}
			command+="ROUND(tSums.PayPlanDue,3) PayPlanDue"//PayPlanDue included for enterprise aging use
				+(hasDateLastPay?",tSums.DateLastPay ":" ")
				+"FROM ("+GetAgingGuarTransQuery(asOfDate,listGuarantors,hasDateLastPay,isHistoric,isGroupByGuar,isWoAged,isForceAgeNegAdj,
					doAgePatPayPlanPayments)+") tSums";
			return command;
		}

		///<summary>Returns a query string.</summary>
		private static string GetAgingGuarTransQuery(DateTime asOfDate,List<long> listGuarantors,bool hasDateLastPay=false,bool isHistoric=false,
			bool isGroupByGuar=true,bool isWoAged=false,bool? isForceAgeNegAdj=null,bool doAgePatPayPlanPayments=false)
		{
			//No need to check RemotingRole; no call to db.
			if(asOfDate.Year<1880) {
				asOfDate=DateTime.Today;
			}
			string asOfDateStr=POut.Date(asOfDate);
			string thirtyDaysAgo=POut.Date(asOfDate.AddDays(-30));
			string sixtyDaysAgo=POut.Date(asOfDate.AddDays(-60));
			string ninetyDaysAgo=POut.Date(asOfDate.AddDays(-90));
			string familyPatNums="";
			string command="";
			if(listGuarantors!=null && listGuarantors.Any(x => x>0)) {
				familyPatNums=string.Join(",",Patients.GetAllFamilyPatNumsForGuars(listGuarantors.FindAll(x => x>0)));//list is not null with at least 1 non-zero guarNum in it
			}
			command="";
			bool isAllPats=string.IsNullOrWhiteSpace(familyPatNums);//true if guarantor==0 or invalid, meaning for all patients not just one family
			//Negative adjustments can optionally be overridden in order to ignore the global preference.
			bool isNegAdjAged=(isForceAgeNegAdj??PrefC.GetBool(PrefName.AgingNegativeAdjsByAdjDate));//if isForceAgeNegAdj==null, use the pref
			if(isWoAged || isNegAdjAged) {
				//WriteoffOrig and/or negative Adjs are included in the charges buckets.  Since that could reduce a bucket to less than 0 we need to move any
				//excess to the TotalCredits bucket to be applied to the oldest charge first
				command+="SELECT transSums.PatNum,"
					+"GREATEST(transSums.ChargesOver90,0) ChargesOver90,"//if credit reduced an aged column to <0, add the neg amount to the total credits
					+"GREATEST(transSums.Charges_61_90,0) Charges_61_90,"
					+"GREATEST(transSums.Charges_31_60,0) Charges_31_60,"
					+"GREATEST(transSums.Charges_0_30,0) Charges_0_30,"
					+"transSums.TotalCredits "
						+"- LEAST(transSums.ChargesOver90,0) "//minus because TotalCredits is positive and we want to increase it (i.e. minus a negative = +)
						+"- LEAST(transSums.Charges_61_90,0) "
						+"- LEAST(transSums.Charges_31_60,0) "
						+"- LEAST(transSums.Charges_0_30,0) TotalCredits,"
					+"transSums.BalTotal,"
					+"transSums.InsWoEst,"
					+"transSums.InsPayEst,"
					+"transSums.PayPlanDue"
					+(hasDateLastPay?",transSums.DateLastPay ":" ")
					+"FROM (";
			}
			command+="SELECT "+(isGroupByGuar?"p.Guarantor PatNum,":"trans.PatNum,")
				+"SUM(CASE WHEN (trans.TranAmount > 0 OR trans.TranType IN ('WriteoffOrig'"+(isNegAdjAged?",'Adj'":"")+")) "
					+"AND trans.TranDate < "+ninetyDaysAgo+" THEN trans.TranAmount ELSE 0 END) ChargesOver90,"
				+"SUM(CASE WHEN (trans.TranAmount > 0 OR trans.TranType IN ('WriteoffOrig'"+(isNegAdjAged?",'Adj'":"")+")) "
					+"AND trans.TranDate < "+sixtyDaysAgo+" AND trans.TranDate >= "+ninetyDaysAgo+" THEN trans.TranAmount ELSE 0 END) Charges_61_90,"
				+"SUM(CASE WHEN (trans.TranAmount > 0 OR trans.TranType IN ('WriteoffOrig'"+(isNegAdjAged?",'Adj'":"")+")) "
					+"AND trans.TranDate < "+thirtyDaysAgo+" AND trans.TranDate >= "+sixtyDaysAgo+" THEN trans.TranAmount ELSE 0 END) Charges_31_60,"
				+"SUM(CASE WHEN (trans.TranAmount > 0 OR trans.TranType IN ('WriteoffOrig'"+(isNegAdjAged?",'Adj'":"")+")) "
					+"AND trans.TranDate <= "+asOfDateStr+" AND trans.TranDate >= "+thirtyDaysAgo+" THEN trans.TranAmount ELSE 0 END) Charges_0_30,"
				+"-SUM(CASE WHEN trans.TranAmount < 0 AND trans.TranType NOT IN ('WriteoffOrig'"+(isNegAdjAged?",'Adj'":"")+") "
					+"AND trans.TranDate <= "+asOfDateStr+" THEN trans.TranAmount ELSE 0 END) TotalCredits,"
				+"SUM(CASE WHEN trans.TranAmount != 0"+(isHistoric?(" AND trans.TranDate <= "+asOfDateStr):"")+" THEN trans.TranAmount ELSE 0 END) BalTotal,"
				+"SUM(trans.InsWoEst) InsWoEst,"
				+"SUM(trans.InsPayEst) InsPayEst,"
				+"SUM(trans.PayPlanAmount) PayPlanDue"
				+(hasDateLastPay?",MAX(CASE WHEN trans.TranType='PatPay' THEN trans.TranDate ELSE '0001-01-01' END) DateLastPay ":" ")
				+"FROM ("
					+GetTransQueryString(asOfDate,familyPatNums,isWoAged,isHistoric,doAgePatPayPlanPayments)
				+") trans ";
			if(isGroupByGuar) {
				command+="INNER JOIN patient p ON p.PatNum=trans.PatNum "
					+"GROUP BY p.Guarantor";
				if(!isAllPats || !PrefC.GetBool(PrefName.AgingIsEnterprise)) {//only if for one fam or if not using famaging table
					command+=" ORDER BY NULL";
				}
			}
			else {
				command+="GROUP BY trans.PatNum";
			}
			if(isWoAged || isNegAdjAged) {
				command+=") transSums";
			}
			return command;
		}

		///<summary>Returns the transaction query string used in calculating aging.  string familyPatNums is usually a comma delimited list of PatNums for
		///a family, but can be a comma delimited list of patients from many families or null/empty.  Returns the query string used to select the trans
		///for calculating aging for the pats in the familyPatNums string.  If familyPatNums is null/empty the query string will be for all pats.</summary>
		public static string GetTransQueryString(DateTime asOfDate,string familyPatNums,bool isWoAged=false,bool isHistoric=false,
			bool doAgePatPayPlanPayments=false)
		{
			//No need to check RemotingRole; no call to db.
			string billInAdvanceDate;
			if(isHistoric) {
				//This if statement never really does anything.  The only places that call this function with historic=true don't look at the
				//patient.payplandue amount, and patient aging gets reset after the reports are generated.  In the future if we start looking at payment plan
				//due amounts when historic=true we may need to revaluate this if statement.
				billInAdvanceDate=POut.Date(DateTime.Today.AddDays(PrefC.GetLong(PrefName.PayPlansBillInAdvanceDays)));
			}
			else {
				billInAdvanceDate=POut.Date(asOfDate.AddDays(PrefC.GetLong(PrefName.PayPlansBillInAdvanceDays)));
			}
			string asOfDateStr=POut.Date(asOfDate);
			PayPlanVersions payPlanVersionCur=(PayPlanVersions)PrefC.GetInt(PrefName.PayPlansVersion);
			bool isAllPats=string.IsNullOrWhiteSpace(familyPatNums);
			string command="";
			#region Completed Procs
			command+="SELECT 'Proc' TranType,pl.ProcNum PriKey,pl.PatNum,pl.ProcDate TranDate,pl.ProcFee*(pl.UnitQty+pl.BaseUnits) TranAmount,"
				+"0 PayPlanAmount,0 InsWoEst,0 InsPayEst "
				+"FROM procedurelog pl "
				+"WHERE pl.ProcStatus=2 "
				+"AND pl.ProcFee != 0 "
				+(isAllPats?"":("AND pl.PatNum IN ("+familyPatNums+") "))
			#endregion Completed Procs
				+"UNION ALL "
			#region Insurance Payments and WriteOffs, PayPlan Ins Payments, and InsPayEst
			#region Regular Claimproc By DateCP
				+"SELECT 'Claimproc' TranType,cp.ClaimProcNum PriKey,cp.PatNum,cp.DateCP TranDate,"
				+"(CASE WHEN cp.Status != 0 THEN (CASE WHEN cp.PayPlanNum = 0 THEN -cp.InsPayAmt ELSE 0 END)"
					+(isWoAged?"":" - cp.WriteOff")+" ELSE 0 END) TranAmount,"
				+"(CASE WHEN cp.PayPlanNum != 0 AND cp.Status IN(1,4,5) "//Received,Supplemental,CapClaim attached to payplan
				+(payPlanVersionCur==PayPlanVersions.AgeCreditsAndDebits ? "AND COALESCE(pp.IsClosed,0)=0 ":"") //ignore closed payment plans on v2
					+(isHistoric?("AND cp.DateCP <= "+asOfDateStr+" "):"")+"THEN -cp.InsPayAmt ELSE 0 END) PayPlanAmount,"//Claim payments tracked by payplan
				+(isWoAged?"0":("(CASE WHEN "+(isHistoric?("cp.ProcDate <= "+asOfDateStr+" "//historic=NotRcvd OR Rcvd and DateCp>asOfDate
					+"AND (cp.Status = 0 OR (cp.Status = 1 AND cp.DateCP > "+asOfDateStr+"))"):"cp.Status = 0")+" "//not historic=NotReceived
					+"THEN cp.WriteOff ELSE 0 END)"))+" InsWoEst,"//writeoff
				+"(CASE WHEN "+(isHistoric?("cp.ProcDate <= "+asOfDateStr+" "//historic=NotRcvd OR Rcvd and DateCp>asOfDate
					+"AND (cp.Status = 0 OR (cp.Status = 1 AND cp.DateCP > "+asOfDateStr+"))"):"cp.Status = 0")+" "//not historic=NotReceived
					+"THEN cp.InsPayEst ELSE 0 END) InsPayEst "//inspayest
				+"FROM claimproc cp "
				+(payPlanVersionCur==PayPlanVersions.AgeCreditsAndDebits?"LEFT JOIN payplan pp ON cp.PayPlanNum=pp.PayPlanNum ":"")
				+"WHERE cp.status IN (0,1,4,5,7) "//NotReceived,Received,Supplemental,CapClaim,CapComplete
				+(isAllPats?"":("AND cp.PatNum IN ("+familyPatNums+") "))
				//efficiency improvement for MySQL only.
				+(DataConnection.DBtype==DatabaseType.MySql?"HAVING TranAmount != 0 OR PayPlanAmount != 0 OR InsWoEst != 0 OR InsPayEst != 0 ":"");
			#endregion Regular Claimproc By DateCP
			#region Original and Current Writeoff/Delta
			//Only included if writeoffs are aged.  Requires joining to the claimsnapshot table.
			if(isWoAged) {
				command+="UNION ALL "
					//This union is for aging the snapshot w/o if one exists, otherwise the claimproc w/o, using ProcDate.
					+"SELECT 'WriteoffOrig' TranType,cp.ClaimProcNum PriKey,cp.PatNum,cp.ProcDate TranDate,"//use ProcDate
					+"COALESCE(CASE WHEN css.Writeoff = -1 THEN 0 ELSE -css.Writeoff END,"//Rcvd and NotRcvd, age snapshot w/o if snapshot exists and w/o != -1
						+"CASE WHEN cp.Status!=0 THEN -cp.WriteOff ELSE 0 END) TranAmount,"//if Rcvd and no snapshot exists, age claimproc w/o
					+"0 PayPlanAmount,"
					//Include in InsWoEst column either claimproc w/o if no snapshot or claimproc w/o - snapshot w/o (delta) if snapshot exists and either
						//1. not historic and NotRcvd or
						//2. historic and ProcDate<=asOfDate and either NotRcvd or Rcvd with DateCp>asOfDate (i.e. Rcvd after the asOfDate)
					+"(CASE WHEN "+(isHistoric?("cp.ProcDate <= "+asOfDateStr+" "//historic=ProcDate<=asOfDate and either NotRcvd OR Rcvd with DateCp>asOfDate
						+"AND (cp.Status = 0 OR (cp.Status = 1 AND cp.DateCP > "+asOfDateStr+")) "):"cp.Status = 0 ")//not historic=NotReceived
						+"THEN cp.Writeoff - COALESCE(CASE WHEN css.Writeoff=-1 THEN 0 ELSE css.Writeoff END,0) ELSE 0 END) InsWoEst,"
					+"0 InsPayEst "
					+"FROM claimproc cp "
					+"LEFT JOIN claimsnapshot css ON cp.ClaimProcNum=css.ClaimProcNum "
					+"WHERE cp.status IN (0,1,4,5,7) "//NotReceived,Received,Supplemental,CapClaim,CapComplete
					+(isAllPats?"":("AND cp.PatNum IN ("+familyPatNums+") "))
					+(DataConnection.DBtype==DatabaseType.MySql?"HAVING TranAmount != 0 OR InsWoEst != 0 ":"")//efficiency improvement for MySQL only.
					+"UNION ALL "
					//This union is for Rcvd claims with snapshots only and is the claimproc w/o's - snapshot w/o's (delta) using DateCp
					+"SELECT 'Writeoff' TranType,cp.ClaimProcNum PriKey,cp.PatNum,cp.DateCP TranDate,"//use DateCP
					//If Rcvd and snapshot exists, age claimproc w/o - snapshot w/o (delta)
					+"-(cp.Writeoff - (CASE WHEN css.Writeoff = -1 THEN 0 ELSE css.Writeoff END)) TranAmount,"
					+"0 PayPlanAmount,0 InsWoEst,0 InsPayEst "
					+"FROM claimproc cp "
					+"INNER JOIN claimsnapshot css ON cp.ClaimProcNum=css.ClaimProcNum "
					+"WHERE cp.status IN (1,4,5,7) "//Received,Supplemental,CapClaim,CapComplete
					+(isAllPats?"":("AND cp.PatNum IN ("+familyPatNums+") "))
					+(DataConnection.DBtype==DatabaseType.MySql?"HAVING TranAmount != 0 ":"");//efficiency improvement for MySQL only.
			}
			#endregion Original and Current Writeoff/Delta
			#endregion Insurance Payments and WriteOffs, PayPlan Ins Payments, and InsPayEst
			command+="UNION ALL "
			#region Adjustments
				+"SELECT 'Adj' TranType,a.AdjNum PriKey,a.PatNum,a.AdjDate TranDate,a.AdjAmt TranAmount,0 PayPlanAmount,0 InsWoEst,0 InsPayEst "
				+"FROM adjustment a "
				+"WHERE a.AdjAmt != 0 "
				+(isAllPats?"":("AND a.PatNum IN ("+familyPatNums+") "))
			#endregion Adjustments
				+"UNION ALL "
			#region Paysplits and PayPlan Paysplits
				+"SELECT 'PatPay' TranType,ps.SplitNum PriKey,ps.PatNum,ps.DatePay TranDate,";
			//v1 and v3: splits not attached to payment plans, v2 or doAgePatPayPlanPayments: all splits for pat/fam
			if(payPlanVersionCur.In(PayPlanVersions.DoNotAge,PayPlanVersions.AgeCreditsOnly) && !doAgePatPayPlanPayments) {
				command+="(CASE WHEN ps.PayPlanNum=0 THEN -ps.SplitAmt ELSE 0 END) ";
			}
			else {
				command+="-ps.SplitAmt ";
			}
			command+="TranAmount,"
				//We cannot exclude payments made outside the specified family, since payment plan guarantors can be in another family.
				+"(CASE WHEN ps.PayPlanNum != 0 "
					+(payPlanVersionCur==PayPlanVersions.AgeCreditsAndDebits?"AND COALESCE(pp.IsClosed,0)=0 ":"") //ignore closed payment plans on v2
					+"THEN -ps.SplitAmt ELSE 0 END) PayPlanAmount,"//Paysplits attached to payment plans
					+"0 InsWoEst,0 InsPayEst "
				+"FROM paysplit ps "
				+(payPlanVersionCur==PayPlanVersions.AgeCreditsAndDebits?"LEFT JOIN payplan pp ON ps.PayPlanNum=pp.PayPlanNum ":"")
				+"WHERE ps.SplitAmt != 0 "
				+(isHistoric?("AND ps.DatePay <= "+asOfDateStr+" "):"")
				+(isAllPats?"":("AND ps.PatNum IN ("+familyPatNums+") "))
			#endregion Paysplits and PayPlan Paysplits
				+"UNION ALL "
			#region PayPlan Charges
				//Calculate the payment plan charges for each payment plan guarantor on or before date considering the PayPlansBillInAdvanceDays setting.
				//Ignore pay plan charges for a different family, since payment plan guarantors might be in another family.
				+"SELECT 'PPCharge' TranType,ppc.PayPlanChargeNum PriKey,ppc.Guarantor PatNum,ppc.ChargeDate TranDate,0 TranAmount,";
				if(payPlanVersionCur==PayPlanVersions.AgeCreditsAndDebits) {
					command+="(CASE WHEN COALESCE(pp.IsClosed,0)=0 THEN COALESCE(ppc.Principal+ppc.Interest,0) ELSE 0 END) PayPlanAmount,";
				}
				else {
					command+="COALESCE(ppc.Principal+ppc.Interest,0) PayPlanAmount,";
				}
				command+="0 InsWoEst,0 InsPayEst "
				+"FROM payplancharge ppc "
				+(payPlanVersionCur==PayPlanVersions.AgeCreditsAndDebits?"LEFT JOIN payplan pp ON ppc.PayPlanNum=pp.PayPlanNum ":"")
				+"WHERE ppc.ChargeDate <= "+billInAdvanceDate+" "//accounts for historic vs current because of how it's set above
				+"AND ppc.ChargeType="+POut.Int((int)PayPlanChargeType.Debit)+" "
				+(isAllPats?"":("AND ppc.Guarantor IN ("+familyPatNums+") "))
				+"AND COALESCE(ppc.Principal+ppc.Interest,0) != 0 ";
			#endregion PayPlan Charges
			#region PayPlan Principal and Interest/CompletedAmt
			#region PayPlan Version 1
			if(payPlanVersionCur==PayPlanVersions.DoNotAge && !doAgePatPayPlanPayments) {
				//v1 and NOT doAgePatPayPlanPayments: aging the entire payment plan, not the payPlanCharges
				//if aging patient payplan payments, don't age the CompletedAmt or it will duplicate the credits aged
				command+="UNION ALL "
					+"SELECT 'PPComplete' TranType,pp.PayPlanNum PriKey,pp.PatNum,pp.PayPlanDate TranDate,-pp.CompletedAmt TranAmount,"
					+"0 PayPlanAmount,0 InsWoEst,0 InsPayEst "
					+"FROM payplan pp "
					+"WHERE pp.CompletedAmt != 0 "
					+(isAllPats?"":("AND pp.PatNum IN ("+familyPatNums+") "));
			}
			#endregion PayPlan Version 1
			#region PayPlan Version 2
			else if(payPlanVersionCur==PayPlanVersions.AgeCreditsAndDebits) {//v2, we should be looking for payplancharges and aging those as patient debits/credits accordingly
				//For credits, use the patient on the payment plan (because they need to have their account balance reduced).
				//For debits, use the guarantor on the payplancharge (because that is the person that needs to be paying on the payment plan).
				command+="UNION ALL "
					+"SELECT 'PPCComplete' TranType,ppc.PayPlanChargeNum PriKey,"
					+"(CASE WHEN ppc.ChargeType = "+POut.Int((int)PayPlanChargeType.Debit)+" THEN ppc.Guarantor ELSE pp.PatNum END) PatNum,"
					+"ppc.ChargeDate TranDate,"
					+"(CASE WHEN ppc.ChargeType != "+POut.Int((int)PayPlanChargeType.Debit)+" THEN -ppc.Principal "
						+"WHEN pp.PlanNum=0 THEN ppc.Principal+ppc.Interest ELSE 0 END) TranAmount,0 PayPlanAmount,0 InsWoEst,0 InsPayEst "
					+"FROM payplancharge ppc "
					+"LEFT JOIN payplan pp ON pp.PayPlanNum=ppc.PayPlanNum "
					+"WHERE ppc.ChargeDate <= "+asOfDateStr+" "
					+(isAllPats?"":("AND ppc.Guarantor IN ("+familyPatNums+") "));
			}
			#endregion PayPlan Version 2
			#region PayPlan Version 3
			else if(payPlanVersionCur==PayPlanVersions.AgeCreditsOnly) {//v3, we should only be aging payplancharge credits
				//Use the patient on the payplan because that patient needs to have their account balance reduced.
				command+="UNION ALL "
					+"SELECT 'PPCComplete' TranType,ppc.PayPlanChargeNum PriKey,pp.PatNum,ppc.ChargeDate TranDate,"
					+"-ppc.Principal TranAmount,0 PayPlanAmount,0 InsWoEst,0 InsPayEst "
					+"FROM payplancharge ppc "
					+"LEFT JOIN payplan pp ON pp.PayPlanNum=ppc.PayPlanNum "
					+"WHERE ppc.ChargeDate <= "+asOfDateStr+" "
					+"AND ppc.ChargeType = "+POut.Int((int)PayPlanChargeType.Credit)+" "
					+(isAllPats?"":("AND ppc.Guarantor IN ("+familyPatNums+") "));
			}
			#endregion PayPlan Version 3
			#region Payment Plans Version 4 - No Charges
			else if(payPlanVersionCur==PayPlanVersions.NoCharges) {
				//For No Charges payment plans, payment plan charges DO NOT affect account balances.  This is intentional.
			}
			#endregion
			#endregion PayPlan Principal and Interest/CompletedAmt
			#region Get All Family PatNums
			if(!isAllPats) {
				//get all family PatNums in case there are no transactions for the family in order to clear out the family balance
				command+="UNION ALL "
					+"SELECT 'FamPatNums' TranType,PatNum PriKey,PatNum,NULL TranDate,0 TranAmount,0 PayPlanAmount,0 InsWoEst,0 InsPayEst "
					+"FROM patient "
					+"WHERE PatNum IN ("+familyPatNums+")";
			}
			#endregion Get All Family PatNums
			return command;
		}

		///<summary>Returns dictionary with key=Guarantor, value=dictionary with key=Tuple&lt;TsiFKeyType,FKey>, value=TsiTrans.  This links a guarantor
		///to all transactions that comprise the family's aging, grouped by guarantor and transaction type (i.e. procedures, paysplits, etc.).</summary>
		public static Dictionary<long,Dictionary<Tuple<TsiFKeyType,long>,TsiTrans>> GetDictTransForGuars(List<long> listGuarNums) {
			//No need to check RemotingRole; no call to db.
			return GetTransForGuars(listGuarNums).GroupBy(x => x.Guarantor)
				.ToDictionary(x => x.Key,x => x.ToDictionary(y => new Tuple<TsiFKeyType,long>(y.KeyType,y.PriKey)));
		}

		public static List<TsiTrans> GetTransForGuars(List<long> listGuarNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<TsiTrans>>(MethodBase.GetCurrentMethod(),listGuarNums);
			}
			string familyPatNums="";
			string command="";
			if(listGuarNums!=null && listGuarNums.Count>0) {
				command="SELECT p.PatNum FROM patient p WHERE p.Guarantor IN ("+string.Join(",",listGuarNums)+")";
				familyPatNums=string.Join(",",Db.GetListLong(command));//will contain at least one patnum (the guarantor)
			}
			command="SELECT patient.Guarantor,trans.TranType,trans.PriKey,trans.PatNum,trans.TranDate,trans.TranAmount,trans.InsWoEst+trans.InsPayEst InsEst "
				+"FROM ("+GetTransQueryString(DateTime.Today,familyPatNums)+") trans "
				+"INNER JOIN patient ON patient.PatNum=trans.PatNum";
			Dictionary<string,TsiFKeyType> dictTranTypes=new Dictionary<string,TsiFKeyType>() {
				{ "Proc",TsiFKeyType.Procedure },
				{ "Claimproc",TsiFKeyType.Claimproc },//will most likely be neg
				{ "Adj",TsiFKeyType.Adjustment },//could be pos or neg
				{ "PatPay",TsiFKeyType.PaySplit },//will be neg, v1 and v3: tranamount=splits not attached to payplans,v2: tranamount=all splits
				{ "PPComplete",TsiFKeyType.PayPlan },//v1: tranamount=-pp.CompletedAmt
				{ "PPCComplete",TsiFKeyType.PayPlanCharge }//if (v2 & debit) OR (v3 & credit) then tranamount=-ppc.Principal, else if v2 & plannum==0 then tranamount=ppc.Principal+ppc.Interest
			};
			return Db.GetTable(command).Select()
				.Where(x => dictTranTypes.ContainsKey(x["TranType"].ToString()))
				.Select(x => new TsiTrans(
					PIn.Long(x["PriKey"].ToString()),
					dictTranTypes[x["TranType"].ToString()],
					PIn.Long(x["PatNum"].ToString()),
					PIn.Long(x["Guarantor"].ToString()),
					PIn.Date(x["TranDate"].ToString()),
					PIn.Double(x["TranAmount"].ToString())-PIn.Double(x["InsEst"].ToString())//have to subtract InsEst so that balance due will match the PatAging.AmountDue
				)).ToList();
		}
		
		///<summary>Gets the earliest date of any portion of the current balance for the family.
		///Returns a data table with two columns: PatNum and DateAccountAge.</summary>
		public static DataTable GetDateBalanceBegan(List<PatAging> listGuarantors,DateTime dateAsOf,bool isSuperBills) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),listGuarantors,dateAsOf,isSuperBills);
			}
			DataTable retval=new DataTable();
			retval.Columns.Add("PatNum");
			retval.Columns.Add("DateAccountAge");
			retval.Columns.Add("DateZeroBal");
			//key=SuperFamily (PatNum); value=list of PatNums for the guars of each fam on a superbill, or if not a superbill, a list containing the GuarNum
			Dictionary<long,List<long>> dictSuperFamGNums=new Dictionary<long,List<long>>();
			foreach(PatAging patAgeCur in listGuarantors) {
				//if making superBills and this guarantor has super billing and is also the superhead for the super family,
				//fill dict with all guarnums and add all family members for all guars included in the superbill to the all patnums list
				if(isSuperBills && patAgeCur.HasSuperBilling && patAgeCur.SuperFamily==patAgeCur.PatNum) {
					dictSuperFamGNums[patAgeCur.SuperFamily]=Patients.GetSuperFamilyGuarantors(patAgeCur.SuperFamily)
						.FindAll(x => x.HasSuperBilling)
						.Select(x => x.PatNum).ToList();
				}
				else {//not a superBill, just add all family members for this guarantor
					dictSuperFamGNums[patAgeCur.PatNum]=new List<long>() { patAgeCur.PatNum };
				}
			}
			//list of all family member PatNums for each guarantor/superhead for all statements being generated
			List<long> listPatNumsAll=Patients.GetAllFamilyPatNums(dictSuperFamGNums.SelectMany(x => x.Value).ToList());
			if(listPatNumsAll.Count<1) {//should never happen
				return retval;
			}
			string patNumStr=string.Join(",",listPatNumsAll);
			string command="SELECT patient.Guarantor AS PatNum,TranDate,SUM(CASE WHEN TranAmount>0 THEN TranAmount ELSE 0 END) AS ChargeForDate,"
				+"SUM(CASE WHEN TranAmount<0 THEN TranAmount ELSE 0 END) AS PayForDate "
				+"FROM ("
					//Get the completed procedure dates and charges for the fam
					+"SELECT PatNum,ProcDate AS TranDate,ProcFee*(UnitQty+BaseUnits) AS TranAmount "
					+"FROM procedurelog "
					+"WHERE PatNum IN ("+patNumStr+") "
					+"AND ProcStatus="+POut.Int((int)ProcStat.C)+" "
					+"AND "+DbHelper.DtimeToDate("ProcDate")+"<="+POut.Date(dateAsOf)+" "
					+"UNION ALL "
					//Paysplits for the fam
					+"SELECT PatNum,DatePay AS TranDate,-SplitAmt AS TranAmount "
					+"FROM paysplit "
					+"WHERE PatNum IN ("+patNumStr+") "
					+"AND PayPlanNum=0 "//Only splits not attached to payment plans
					+"AND "+DbHelper.DtimeToDate("DatePay")+"<="+POut.Date(dateAsOf)+" "
					+"UNION ALL "
					//Get the adjustment dates and amounts for the fam
					+"SELECT PatNum,AdjDate AS TranDate,AdjAmt AS TranAmount "
					+"FROM adjustment "
					+"WHERE PatNum IN ("+patNumStr+") "
					+"AND "+DbHelper.DtimeToDate("AdjDate")+"<="+POut.Date(dateAsOf)+" "
					+"UNION ALL "
					//Claim payments for the fam
					+"SELECT PatNum,DateCp AS TranDate,-InsPayAmt-Writeoff AS TranAmount "
					+"FROM claimproc "
					+"WHERE PatNum IN ("+patNumStr+") "
					+"AND STATUS IN("+POut.Int((int)ClaimProcStatus.Received)
						+","+POut.Int((int)ClaimProcStatus.Supplemental)
						+","+POut.Int((int)ClaimProcStatus.CapClaim)
						+","+POut.Int((int)ClaimProcStatus.CapComplete)+") "
					+"AND PayPlanNum=0 "//Only ins payments not attached to payment plans
					+"AND "+DbHelper.DtimeToDate("DateCp")+"<="+POut.Date(dateAsOf)+" "
					+"UNION ALL "
					//Payment plan principal for the fam
					+"SELECT PatNum,PayPlanDate AS TranDate,-CompletedAmt AS TranAmount "
					+"FROM payplan "
					+"WHERE PatNum IN ("+patNumStr+") "
					+"AND "+DbHelper.DtimeToDate("PayPlanDate")+"<="+POut.Date(dateAsOf)
				+") RawPatTrans "
				+"INNER JOIN patient ON patient.PatNum=RawPatTrans.PatNum "
				+"GROUP BY patient.Guarantor,TranDate "
				+"ORDER BY patient.Guarantor,TranDate";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count<1) {
				return retval;
			}
			double runningChargesToDate=0;
			double runningCreditsToDate=0;
			long guarNumCur;
			Dictionary<long,double> dictGuarCreditTotals=table.Rows.OfType<DataRow>()
				.GroupBy(x => PIn.Long(x["PatNum"].ToString()),x => PIn.Double(x["PayForDate"].ToString()))
				.ToDictionary(x => x.Key,y => y.Sum());
			//Create a dictionary that tells a story about the transactions and their dates for each family.
			Dictionary<long,Dictionary<DateTime,double>> dictGuarDateBal=new Dictionary<long,Dictionary<DateTime,double>>();
			Dictionary<long,Dictionary<DateTime,double>> dictGuarDateBalZero=new Dictionary<long,Dictionary<DateTime,double>>();
			foreach(DataRow rowCur in table.Rows) {
				guarNumCur=PIn.Long(rowCur["PatNum"].ToString());
				if(!dictGuarDateBalZero.ContainsKey(guarNumCur)) {
					dictGuarDateBalZero[guarNumCur]=new Dictionary<DateTime,double>();
					dictGuarDateBal[guarNumCur]=new Dictionary<DateTime,double>();
					runningChargesToDate=0;
					runningCreditsToDate=0;
				}
				runningChargesToDate+=PIn.Double(rowCur["ChargeForDate"].ToString());
				runningCreditsToDate+=PIn.Double(rowCur["PayForDate"].ToString());
				dictGuarDateBalZero[guarNumCur][PIn.Date(rowCur["TranDate"].ToString())]=runningChargesToDate+runningCreditsToDate;
				dictGuarDateBal[guarNumCur][PIn.Date(rowCur["TranDate"].ToString())]=runningChargesToDate+dictGuarCreditTotals[guarNumCur];
			}
			DataRow row;
			List<DateTime> listDateBals;
			List<DateTime> listDateZeroBals;
			List<long> listGuarNums;
			//find the earliest trans that uses up the account credits and is therefore the trans date for which the account balance is "first" positive
			foreach(PatAging patAgeCur in listGuarantors) {
				if(isSuperBills && patAgeCur.HasSuperBilling && patAgeCur.PatNum!=patAgeCur.SuperFamily) {
					continue;
				}
				if(!isSuperBills || !patAgeCur.HasSuperBilling) {
					listGuarNums=new List<long>() { patAgeCur.PatNum };
				}
				else {//must be superbill and this is the superhead
					if(!dictSuperFamGNums.ContainsKey(patAgeCur.PatNum)) {
						continue;//should never happen
					}
					listGuarNums=dictSuperFamGNums[patAgeCur.PatNum];
				}
				//dateLastZero=DateTime.MinValue;
				listDateBals=new List<DateTime>();
				listDateZeroBals=new List<DateTime>();
				foreach(long guarNum in listGuarNums) {
					if(!dictGuarDateBal.ContainsKey(guarNum) || !dictGuarDateBalZero.ContainsKey(guarNum)) {//should never happen
						continue;
					}
					listDateBals.Add(dictGuarDateBal[guarNum].Where(x => x.Value>0.005).Select(x => x.Key).DefaultIfEmpty(DateTime.MinValue).Min());
					//list of guars, or if not a super statement a list of one guar, and the date of the trans that used up the last of the acct credits
					listDateZeroBals.Add(dictGuarDateBalZero[guarNum]
						//Get dates greater than the most recent date the balance was 0
						.Where(x => x.Key>dictGuarDateBalZero[guarNum].Where(y => y.Value.Between(-0.005,0.005)).Select(y => y.Key).DefaultIfEmpty(DateTime.MinValue).Max())
						//get the earliest date that was after the most recent 0 bal date. Defaults to DateTime.MinValue if patient has a zero balance.
						.Select(x => x.Key).DefaultIfEmpty(DateTime.MinValue).Min());
				}
				row=retval.NewRow();
				row["PatNum"]=POut.Long(patAgeCur.PatNum);
				//set to the oldest balance date for all guarantors on this superbill, or if not a super bill, the oldest balance date for this guarantor
				//could be DateTime.MinValue if their credits pay for all of their charges
				row["DateAccountAge"]=listDateBals.Where(x => x>DateTime.MinValue).DefaultIfEmpty(DateTime.MinValue).Min();
				row["DateZeroBal"]=listDateZeroBals.Where(x => x>DateTime.MinValue).DefaultIfEmpty(DateTime.MinValue).Min();
				retval.Rows.Add(row);
			}
			return retval;
		}
	}

}