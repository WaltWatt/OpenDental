using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness {
	public class RpPayPlan {

		///<summary>If not using clinics then supply an empty list of clinicNums.</summary>
		public static DataSet GetPayPlanTable(DateTime dateStart,DateTime dateEnd,List<long> listProvNums,List<long> listClinicNums,
			bool hasAllProvs,DisplayPayPlanType displayPayPlanType,bool hideCompletedPlans,bool showFamilyBalance,bool hasDateRange,bool isPayPlanV2) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetDS(MethodBase.GetCurrentMethod(),dateStart,dateEnd,listProvNums,listClinicNums,hasAllProvs,displayPayPlanType,
					hideCompletedPlans,showFamilyBalance,hasDateRange,isPayPlanV2);
			}
			string whereProv="";
			if(!hasAllProvs) {
				whereProv+=" AND payplancharge.ProvNum IN(";
				for(int i=0;i<listProvNums.Count;i++) {
					if(i>0) {
						whereProv+=",";
					}
					whereProv+=POut.Long(listProvNums[i]);
				}
				whereProv+=") ";
			}
			string whereClin="";
			bool hasClinicsEnabled=ReportsComplex.RunFuncOnReportServer(() =>(!Prefs.GetBoolNoCache(PrefName.EasyNoClinics)));
			if(hasClinicsEnabled) {//Using clinics
				whereClin+=" AND payplancharge.ClinicNum IN(";
				for(int i=0;i<listClinicNums.Count;i++) {
					if(i>0) {
						whereClin+=",";
					}
					whereClin+=POut.Long(listClinicNums[i]);
				}
				whereClin+=") ";
			}
			DataSet ds=new DataSet();
			DataTable table=new DataTable("Clinic");
			table.Columns.Add("provider");
			table.Columns.Add("guarantor");
			table.Columns.Add("ins");
			table.Columns.Add("princ");
			table.Columns.Add("accumInt");
			table.Columns.Add("paid");
			table.Columns.Add("balance");
			table.Columns.Add("due");
			if(isPayPlanV2) {
				table.Columns.Add("notDue");
			}
			table.Columns.Add("famBal");
			table.Columns.Add("clinicName");
			DataTable tableTotals=new DataTable("Total");
			tableTotals.Columns.Add("clinicName");
			tableTotals.Columns.Add("princ");
			tableTotals.Columns.Add("accumInt");
			tableTotals.Columns.Add("paid");
			tableTotals.Columns.Add("balance");
			tableTotals.Columns.Add("due");
			if(isPayPlanV2) {
				tableTotals.Columns.Add("notDue");
			}
			tableTotals.Columns.Add("famBal");
			DataRow row;
			string datesql="CURDATE()";//This is used to find out how much people owe currently and has nothing to do with the selected range
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				datesql="(SELECT CURRENT_DATE FROM dual)";
			}
			//Oracle TODO:  Either put entire query without GROUP BY in SUBSELECT and then GROUP BY outside, or rewrite query to use joins instead of subselects.
			string command="SELECT FName,LName,MiddleI,PlanNum,Preferred,PlanNum, "
				+"COALESCE((SELECT SUM(Principal+Interest) FROM payplancharge WHERE payplancharge.PayPlanNum=payplan.PayPlanNum "
				+"AND payplancharge.ChargeType="+POut.Int((int)PayPlanChargeType.Debit)+" "//for v1, debits are the only ChargeType.
					+"AND ChargeDate <= "+datesql+@"),0) '_accumDue', ";
			command+="COALESCE((SELECT SUM(Interest) FROM payplancharge WHERE payplancharge.PayPlanNum=payplan.PayPlanNum "
				+"AND payplancharge.ChargeType="+POut.Int((int)PayPlanChargeType.Debit)+" "//for v1, debits are the only ChargeType.
					+"AND ChargeDate <= "+datesql+@"),0) '_accumInt', ";
			command+="COALESCE((SELECT SUM(SplitAmt) FROM paysplit WHERE paysplit.PayPlanNum=payplan.PayPlanNum AND paysplit.PayPlanNum!=0),0) '_paid', ";
			command+="COALESCE((SELECT SUM(InsPayAmt) FROM claimproc WHERE claimproc.PayPlanNum=payplan.PayPlanNum "
					+"AND claimproc.Status IN("
					+POut.Int((int)ClaimProcStatus.Received)+","
					+POut.Int((int)ClaimProcStatus.Supplemental)+","
					+POut.Int((int)ClaimProcStatus.CapClaim)
					+") AND claimproc.PayPlanNum!=0),0) '_insPaid', ";
			command+="COALESCE((SELECT SUM(Principal) FROM payplancharge WHERE payplancharge.PayPlanNum=payplan.PayPlanNum "
				+"AND payplancharge.ChargeType="+POut.Int((int)PayPlanChargeType.Debit)+"),0) '_principal', "//for v1, debits are the only ChargeType.
				+"COALESCE((SELECT SUM(Principal) FROM payplancharge WHERE payplancharge.PayPlanNum=payplan.PayPlanNum "
				+"AND payplancharge.ChargeType="+POut.Int((int)PayPlanChargeType.Credit)+"),0) '_credits', "//for v1, will always be 0.
				+"COALESCE((SELECT SUM(Principal) FROM payplancharge WHERE payplancharge.PayPlanNum=payplan.PayPlanNum "
				+"AND payplancharge.ChargeType="+POut.Int((int)PayPlanChargeType.Credit)+" AND ChargeDate > "+datesql+"),0) '_notDue', "
				+"patient.PatNum PatNum, "
				+"payplancharge.ProvNum ProvNum ";
			if(hasClinicsEnabled) {
				command+=", payplancharge.ClinicNum ClinicNum ";
			}
			//In order to determine if the patient has completely paid off their payment plan we need to get the total amount of interest as of today.
			//Then, after the query has run, we'll add the interest up until today with the total principal for the entire payment plan.
			//For this reason, we cannot use _accumDue which only gets the principle up until today and not the entire payment plan principle.
			command+=",COALESCE((SELECT SUM(Interest) FROM payplancharge WHERE payplancharge.PayPlanNum=payplan.PayPlanNum "
					+"AND payplancharge.ChargeType="+POut.Int((int)PayPlanChargeType.Debit)+" "//for v1, debits are the only ChargeType.
					+"AND ChargeDate <= "+datesql+@"),0) '_interest' "
				+"FROM payplan "
				+"LEFT JOIN patient ON patient.PatNum=payplan.Guarantor "
				+"LEFT JOIN payplancharge ON payplan.PayPlanNum=payplancharge.PayPlanNum "
				+"WHERE TRUE ";//Always include true, so that the WHERE clause may always be present.
			if(hasDateRange) {
				command+="AND payplan.PayPlanDate >= "+POut.Date(dateStart)+" "
				+"AND payplan.PayPlanDate <= "+POut.Date(dateEnd)+" ";
			}
			command+=whereProv
				+whereClin;
			if(displayPayPlanType==DisplayPayPlanType.Insurance) {
				command+="AND payplan.PlanNum!=0 ";
			}
			else if(displayPayPlanType==DisplayPayPlanType.Patient) {
				command+="AND payplan.PlanNum=0 ";
			}
			else if(displayPayPlanType==DisplayPayPlanType.Both) {
				//Do not filter the query at all which will show both insurance and patient payment plan types.
			}
			if(hideCompletedPlans) {
				command+="AND payplan.IsClosed=0 ";
			}
			command+="GROUP BY FName,LName,MiddleI,Preferred,payplan.PayPlanNum ";
			if(hasClinicsEnabled) {
				command+="ORDER BY ClinicNum,LName,FName";
			}
			else {
				command+="ORDER BY LName,FName";
			}
			DataTable raw=ReportsComplex.RunFuncOnReportServer(() => ReportsComplex.GetTable(command));
			List<Provider> listProvs=ReportsComplex.RunFuncOnReportServer(() => Providers.GetAll());
			//DateTime payplanDate;
			Patient pat;
			double princ;
			double paid;
			double interest;
			double accumDue;
			double notDue;
			decimal famBal=0;
			double princTot=0;
			double paidTot=0;
			double interestTot=0;
			double balanceTot=0;
			double accumDueTot=0;
			double notDueTot=0;
			decimal famBalTot=0;
			string clinicDescOld="";
			for(int i=0;i<raw.Rows.Count;i++) {
				princ=PIn.Double(raw.Rows[i]["_principal"].ToString());
				interest=PIn.Double(raw.Rows[i]["_accumInt"].ToString());
				if(raw.Rows[i]["PlanNum"].ToString()=="0") {//pat payplan
					paid=PIn.Double(raw.Rows[i]["_paid"].ToString());
				}
				else {//ins payplan
					paid=PIn.Double(raw.Rows[i]["_insPaid"].ToString());
				}
				accumDue=PIn.Double(raw.Rows[i]["_accumDue"].ToString());
				notDue=PIn.Double(raw.Rows[i]["_notDue"].ToString());
				row=table.NewRow();
				//payplanDate=PIn.PDate(raw.Rows[i]["PayPlanDate"].ToString());
				//row["date"]=raw.Rows[i]["PayPlanDate"].ToString();//payplanDate.ToShortDateString();
				pat=new Patient();
				pat.LName=raw.Rows[i]["LName"].ToString();
				pat.FName=raw.Rows[i]["FName"].ToString();
				pat.MiddleI=raw.Rows[i]["MiddleI"].ToString();
				pat.Preferred=raw.Rows[i]["Preferred"].ToString();
				row["provider"]=Providers.GetLName(PIn.Long(raw.Rows[i]["ProvNum"].ToString()),listProvs);
				row["guarantor"]=pat.GetNameLF();
				if(raw.Rows[i]["PlanNum"].ToString()=="0") {
					row["ins"]="";
				}
				else {
					row["ins"]="X";
				}
				row["princ"]=princ.ToString("f");
				row["accumInt"]=interest.ToString("f");
				row["paid"]=paid.ToString("f");
				row["balance"]=(princ+interest-paid).ToString("f");
				row["due"]=(accumDue-paid).ToString("f");
				if(isPayPlanV2) {
					row["notDue"]=((princ+interest-paid)-(accumDue-paid)).ToString("f");
				}
				if(showFamilyBalance) {
					Family famCur=ReportsComplex.RunFuncOnReportServer(() => Patients.GetFamily(PIn.Long(raw.Rows[i]["PatNum"].ToString())));
					famBal=(decimal)famCur.ListPats[0].BalTotal;
					row["famBal"]=(famBal - (decimal)famCur.ListPats[0].InsEst).ToString("F");
				}
				if(hasClinicsEnabled) {//Using clinics
					List<Clinic> listClinics=ReportsComplex.RunFuncOnReportServer(() => Clinics.GetClinicsNoCache());
					string clinicDesc=Clinics.GetDesc(PIn.Long(raw.Rows[i]["ClinicNum"].ToString()),listClinics);
					clinicDesc=(clinicDesc=="")?Lans.g("FormRpPayPlans","Unassigned"):clinicDesc;
					if(!String.IsNullOrEmpty(clinicDescOld) && clinicDesc!=clinicDescOld) {//Reset all the total values
						DataRow rowTot=tableTotals.NewRow();
						rowTot["clinicName"]=clinicDescOld;
						rowTot["princ"]=princTot.ToString();
						rowTot["accumInt"]=interestTot.ToString();
						rowTot["paid"]=paidTot.ToString();
						rowTot["balance"]=balanceTot.ToString();
						rowTot["due"]=accumDueTot.ToString();
						if(isPayPlanV2) {
							rowTot["notDue"]=notDueTot.ToString();
						}	
						rowTot["famBal"]=famBalTot.ToString();
						tableTotals.Rows.Add(rowTot);
						princTot=0;
						paidTot=0;
						interestTot=0;
						accumDueTot=0;
						balanceTot=0;
						notDueTot=0;
						famBalTot=0;
					}
					row["clinicName"]=clinicDesc;
					clinicDescOld=clinicDesc;
					princTot+=princ;
					paidTot+=paid;
					interestTot+=interest;
					accumDueTot+=(accumDue-paid);
					balanceTot+=(princ+interest-paid);
					notDueTot+=((princ+interest-paid)-(accumDue-paid));
					famBalTot+=famBal;
					if(i==raw.Rows.Count-1) {
						DataRow rowTot=tableTotals.NewRow();
						rowTot["clinicName"]=clinicDescOld;
						rowTot["princ"]=princTot.ToString();
						rowTot["accumInt"]=interestTot.ToString();
						rowTot["paid"]=paidTot.ToString();
						rowTot["balance"]=balanceTot.ToString();
						rowTot["due"]=accumDueTot.ToString();
						if(isPayPlanV2) {
							rowTot["notDue"]=notDueTot.ToString();
						}	
						rowTot["famBal"]=famBalTot.ToString();
						tableTotals.Rows.Add(rowTot);
					}
				}
				table.Rows.Add(row);
			}
			ds.Tables.Add(table);
			ds.Tables.Add(tableTotals);
			return ds;
		}

	}

	///<summary>Used to dictate which payment plan types are shown in the payment plan report.</summary>
	public enum DisplayPayPlanType {
		///<summary>0</summary>
		Patient,
		///<summary>1</summary>
		Insurance,
		///<summary>2</summary>
		Both
	}
}
