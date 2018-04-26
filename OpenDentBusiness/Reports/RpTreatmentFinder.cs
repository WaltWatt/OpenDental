using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness {
	public class RpTreatmentFinder {

		///<summary>Gets the DataTable to display for treatment finder report</summary>
		///<param name="listProviders">Include '0' in the list to get for all providers.</param>
		///<param name="listBilling">Include '0' in the list to get for all billing types.</param>
		///<param name="listClinicNums">Pass in an empty list to get for all clinics.</param>
		public static DataTable GetTreatmentFinderList(bool noIns,bool patsWithAppts,int monthStart,DateTime dateSince,double aboveAmount,
			List<long> listProviders,List<long> listBilling,string code1,string code2,List<long> listClinicNums,bool isProcsGeneral) 
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),noIns,patsWithAppts,monthStart,dateSince,aboveAmount,listProviders,listBilling,code1,code2,
					listClinicNums,isProcsGeneral);
			}
#if DEBUG
			Stopwatch sw=Stopwatch.StartNew();
#endif
			DataTable table=new DataTable();
			DataRow row;
			//columns that start with lowercase are altered for display rather than being raw data.
			table.Columns.Add("PatNum");
			table.Columns.Add("LName");
			table.Columns.Add("FName");
			table.Columns.Add("contactMethod");
			table.Columns.Add("address");
			table.Columns.Add("City");
			table.Columns.Add("State");
			table.Columns.Add("Zip");
			table.Columns.Add("annualMaxInd");
			table.Columns.Add("annualMaxFam");
			table.Columns.Add("amountUsedInd");
			table.Columns.Add("amountUsedFam");
			table.Columns.Add("amountPendingInd");
			table.Columns.Add("amountPendingFam");
			table.Columns.Add("amountRemainingInd");
			table.Columns.Add("amountRemainingFam");
			table.Columns.Add("treatmentPlan");
			table.Columns.Add("carrierName");
			table.Columns.Add("clinicAbbr");
			List<DataRow> rows=new List<DataRow>();
			string command="";
			string joinAnnualMax="";
			string joinCoverageInfo="";
			string joinIndInfo="";
			string joinFamInfo="";
			string subSelectPlanned="";
			string cmdFutureApt=@" AND patient.PatNum NOT IN (
					SELECT PatNum FROM appointment WHERE AptStatus="+POut.Int((int)ApptStatus.Scheduled)+@"
					AND AptDateTime>="+DbHelper.Curdate()+")";
			DateTime renewDate=BenefitLogic.ComputeRenewDate(DateTime.Now,monthStart);
			List<long> listPatNums=new List<long>();
			if((!listProviders.Contains(0) || !listBilling.Contains(0) || listClinicNums.Count > 0)) {
				string cmdPatients="SELECT PatNum from patient ";
				string patWhere="";
				if(!listProviders.Contains(0)) {
					patWhere=" AND patient.PriProv IN ("+string.Join(",",listProviders)+") ";
				}
				if(!listBilling.Contains(0)) {
					patWhere=" AND patient.BillingType IN ("+string.Join(",",listBilling)+") ";
				}
				if(listClinicNums.Count > 0) {
					patWhere+=" AND patient.ClinicNum IN ("+string.Join(",",listClinicNums)+") ";
				}
				if(!patsWithAppts) {
					patWhere+=cmdFutureApt;
				}
				cmdPatients+="WHERE TRUE "+patWhere;
				listPatNums=Db.GetListLong(cmdPatients);
				if(listPatNums.Count==0) {
					return table;
				}
			}
			joinCoverageInfo=@"
				SELECT patplan.PatPlanNum,claimproc.InsSubNum,
				SUM(CASE WHEN claimproc.Status="+POut.Int((int)ClaimProcStatus.NotReceived)+@" AND claimproc.InsPayAmt=0 
				THEN claimproc.InsPayEst ELSE 0 END) AmtPending,
				SUM(CASE WHEN claimproc.Status IN ("+POut.Int((int)ClaimProcStatus.Received)+","
				+POut.Int((int)ClaimProcStatus.Adjustment)+","
				+POut.Int((int)ClaimProcStatus.Supplemental)+@"
				) THEN claimproc.InsPayAmt ELSE 0 END) AmtUsed
				FROM claimproc
				INNER JOIN patient ON patient.PatNum=claimproc.PatNum
				LEFT JOIN patplan ON patplan.PatNum=claimproc.PatNum
					AND patplan.InsSubNum=claimproc.InsSubNum
				LEFT JOIN procedurelog pl ON pl.ProcNum=claimproc.ProcNum
				LEFT JOIN procedurecode pc ON pc.CodeNum=pl.CodeNum ";
			if(!isProcsGeneral) {
				joinCoverageInfo+=@"
					LEFT JOIN (
							SELECT isub.InsSubNum,
							COALESCE(cp.FromCode,pc.ProcCode) AS FromCode,
							COALESCE(cp.ToCode,pc.ProcCode) AS ToCode
							FROM inssub isub
							INNER JOIN benefit b ON b.PlanNum=isub.PlanNum
								AND b.BenefitType="+(int)InsBenefitType.Limitations+@"
								AND b.QuantityQualifier="+(int)BenefitQuantity.None+@" 
								AND b.TimePeriod IN ("+(int)BenefitTimePeriod.ServiceYear+","+(int)BenefitTimePeriod.CalendarYear+@")
							LEFT JOIN covcat cc ON cc.CovCatNum=b.CovCatNum 
							LEFT JOIN covspan cp ON cp.CovCatNum=cc.CovCatNum
							LEFT JOIN procedurecode pc ON pc.CodeNum=b.CodeNum
							WHERE (cc.CovCatNum IS NOT NULL OR b.CodeNum!=0) 
							)ProcCheck ON ProcCheck.InsSubNum=claimproc.InsSubNum
							 AND pc.ProcCode BETWEEN ProcCheck.FromCode AND ProcCheck.ToCode ";
			}
			joinCoverageInfo+="WHERE claimproc.Status IN ("+(int)ClaimProcStatus.NotReceived+", "+(int)ClaimProcStatus.Received
				+", "+(int)ClaimProcStatus.Adjustment+", "+(int)ClaimProcStatus.Supplemental+") ";
			if(!isProcsGeneral) {
				joinCoverageInfo+="AND ProcCheck.InsSubNum IS NULL ";
			}
			joinCoverageInfo+="AND claimproc.ProcDate BETWEEN  "+POut.Date(renewDate)+@" AND "+POut.Date(renewDate.AddYears(1))+@" ";
			if(listPatNums.Count>0) {
				joinCoverageInfo+=@"AND patient.PatNum IN ("+string.Join(",",listPatNums)+") ";
			}
			else if(!patsWithAppts) {
				joinCoverageInfo+=cmdFutureApt;
			}
			joinIndInfo=joinCoverageInfo+" GROUP BY patplan.PatPlanNum ";
			joinFamInfo=joinCoverageInfo+" GROUP BY claimproc.InsSubNum ";
			subSelectPlanned=@"
				(SELECT COALESCE(SUM(ProcFee),0) AmtPlanned
				FROM procedurelog ";
			if(code1!="") {
				subSelectPlanned+="INNER JOIN procedurecode ON procedurecode.CodeNum=procedurelog.CodeNum ";
			}
			subSelectPlanned+="WHERE ProcStatus="+(int)ProcStat.TP+" ";
			if(code1!="") {
				subSelectPlanned+="AND procedurecode.ProcCode>='"+POut.String(code1)+"' "
					+" AND procedurecode.ProcCode<='"+POut.String(code2)+"' ";
			}
			if(dateSince.Year>1880) {
				subSelectPlanned+="AND procedurelog.DateTP>="+POut.DateT(dateSince)+" ";
			}
			subSelectPlanned+="AND PatNum=patient.PatNum ";
			subSelectPlanned+="GROUP BY PatNum) ";
			joinAnnualMax=@"
				SELECT insplan.PlanNum, MAX(CASE WHEN CoverageLevel!="+POut.Int((int)BenefitCoverageLevel.Family)+@"
				THEN MonetaryAmt ELSE -1 END) AnnualMaxInd/*for oracle in case there's more than one*/, 
				MAX(CASE WHEN CoverageLevel="+POut.Int((int)BenefitCoverageLevel.Family)+@"
				THEN MonetaryAmt ELSE -1 END) AnnualMaxFam/*for oracle in case there's more than one*/
				FROM benefit
				INNER JOIN insplan ON insplan.PlanNum=benefit.PlanNum 
				INNER JOIN inssub ON inssub.PlanNum=benefit.PlanNum
				INNER JOIN patplan ON patplan.InsSubNum=inssub.InsSubNum
				INNER JOIN patient ON patient.PatNum=patplan.PatNum
				LEFT JOIN covcat ON benefit.CovCatNum=covcat.CovCatNum
				WHERE (covcat.EbenefitCat="+(int)EbenefitCategory.General+@" OR ISNULL(covcat.EbenefitCat))
				AND benefit.BenefitType="+(int)InsBenefitType.Limitations+@" 
				AND benefit.MonetaryAmt > 0
				AND benefit.QuantityQualifier="+(int)BenefitQuantity.None+" ";
			if(listPatNums.Count>0) {
				joinAnnualMax+=@"AND patient.PatNum IN ("+string.Join(",",listPatNums)+") ";
			}
			else if(!patsWithAppts) {
				joinAnnualMax+=cmdFutureApt;
			}
			joinAnnualMax+=@"GROUP BY insplan.PlanNum";
			command=@"SELECT patient.PatNum, patient.LName, patient.FName,
				patient.Email, patient.HmPhone, patient.PreferRecallMethod,
				patient.WirelessPhone, patient.WkPhone, patient.Address,
				patient.Address2, patient.City, patient.State, patient.Zip,
				patient.PriProv, patient.BillingType,
				COALESCE(annualMax.AnnualMaxInd,0) ""AnnualMaxInd"",
				COALESCE(annualMax.AnnualMaxFam,0) ""AnnualMaxFam"",
				IndividualInfo.AmtUsed ""AmountUsedInd"",
				FamilyInfo.AmtUsed ""AmountUsedFam"",
				IndividualInfo.AmtPending ""AmountPendingInd"",
				FamilyInfo.AmtPending ""AmountPendingFam"",
				COALESCE(annualMax.AnnualMaxInd,0)-COALESCE(IndividualInfo.AmtUsed,0)-COALESCE(IndividualInfo.AmtPending,0) AS ""$AmtRemainingInd"",
				COALESCE(annualMax.AnnualMaxFam,0)-COALESCE(FamilyInfo.AmtUsed,0)-COALESCE(FamilyInfo.AmtPending,0) AS ""$AmtRemainingFam"","+
				subSelectPlanned+@"""$TreatmentPlan"", carrier.CarrierName,COALESCE(clinic.Abbr,'Unassigned') clinicAbbr
				FROM patient
				LEFT JOIN patplan ON patient.PatNum=patplan.PatNum
				LEFT JOIN inssub ON patplan.InsSubNum=inssub.InsSubNum
				LEFT JOIN insplan ON insplan.PlanNum=inssub.PlanNum
				LEFT JOIN carrier ON insplan.CarrierNum=carrier.CarrierNum
				LEFT JOIN ("
					+joinIndInfo
				+@")IndividualInfo ON IndividualInfo.PatPlanNum=patplan.PatPlanNum
				LEFT JOIN ("
					+joinFamInfo
				+@")FamilyInfo ON FamilyInfo.InsSubNum=inssub.InsSubNum
				LEFT JOIN ("
					+joinAnnualMax
				+@") annualMax ON annualMax.PlanNum=inssub.PlanNum
				AND (annualMax.AnnualMaxInd>0 OR annualMax.AnnualMaxFam>0)/*may not be necessary*/
				LEFT JOIN clinic ON clinic.ClinicNum=patient.ClinicNum
				WHERE TRUE 
				AND patient.PatStatus="+POut.Int((int)PatientStatus.Patient)+" ";
			if(!noIns) {//if we don't want patients without insurance
				command+=" AND patplan.Ordinal=1 AND insplan.MonthRenew="+POut.Int(monthStart)+" ";
			}
			if(aboveAmount>0) {
				command+=" AND (annualMax.PlanNum IS NULL OR ((annualMax.AnnualMaxInd=-1 OR annualMax.AnnualMaxInd-COALESCE(IndividualInfo.AmtUsed,0) > "
					+POut.Double(aboveAmount)+@")
					AND (annualMax.AnnualMaxFam=-1 OR annualMax.AnnualMaxFam-COALESCE(FamilyInfo.AmtUsed,0) > "+POut.Double(aboveAmount)+"))) ";
			}
			if(listPatNums.Count>0) {
				command+=" AND patient.PatNum IN ("+string.Join(",",listPatNums)+") ";
			}
			else if(!patsWithAppts) {
				command+=cmdFutureApt;
			}
			command+=@"HAVING $TreatmentPlan > 0 ";
			command+=@"ORDER BY $TreatmentPlan DESC";
			DataTable rawtable=Db.GetTable(command);
#if DEBUG
			sw.Stop();
			Console.WriteLine("Finishing retreiving query: {0}",sw.ElapsedMilliseconds);
			sw=Stopwatch.StartNew();
#endif
			ContactMethod contmeth;
			for(int i=0;i<rawtable.Rows.Count;i++) {
				row=table.NewRow();
				row["PatNum"]=PIn.Long(rawtable.Rows[i]["PatNum"].ToString());
				row["LName"]=rawtable.Rows[i]["LName"].ToString();
				row["FName"]=rawtable.Rows[i]["FName"].ToString();
				contmeth=(ContactMethod)PIn.Long(rawtable.Rows[i]["PreferRecallMethod"].ToString());
				if(contmeth==ContactMethod.None) {
					if(PrefC.GetBool(PrefName.RecallUseEmailIfHasEmailAddress)) {//if user only wants to use email if contact method is email
						if(rawtable.Rows[i]["Email"].ToString() != "") {
							row["contactMethod"]=rawtable.Rows[i]["Email"].ToString();
						}
						else {
							row["contactMethod"]=Lans.g("FormRecallList","Hm:")+rawtable.Rows[i]["HmPhone"].ToString();
						}
					}
					else {
						row["contactMethod"]=Lans.g("FormRecallList","Hm:")+rawtable.Rows[i]["HmPhone"].ToString();
					}
				}
				else if(contmeth==ContactMethod.HmPhone) {
					row["contactMethod"]=Lans.g("FormRecallList","Hm:")+rawtable.Rows[i]["HmPhone"].ToString();
				}
				else if(contmeth==ContactMethod.WkPhone) {
					row["contactMethod"]=Lans.g("FormRecallList","Wk:")+rawtable.Rows[i]["WkPhone"].ToString();
				}
				else if(contmeth==ContactMethod.WirelessPh) {
					row["contactMethod"]=Lans.g("FormRecallList","Cell:")+rawtable.Rows[i]["WirelessPhone"].ToString();
				}
				else if(contmeth==ContactMethod.Email) {
					row["contactMethod"]=rawtable.Rows[i]["Email"].ToString();
				}
				else if(contmeth==ContactMethod.Mail) {
					row["contactMethod"]=Lans.g("FormRecallList","Mail");
				}
				else if(contmeth==ContactMethod.DoNotCall || contmeth==ContactMethod.SeeNotes) {
					row["contactMethod"]=Lans.g("enumContactMethod",contmeth.ToString());
				}
				row["address"]=rawtable.Rows[i]["Address"].ToString();
				if(rawtable.Rows[i]["Address2"].ToString()!="") {
					row["address"]+="\r\n"+rawtable.Rows[i]["Address2"].ToString();
				}
				row["City"]=rawtable.Rows[i]["City"].ToString();
				row["State"]=rawtable.Rows[i]["State"].ToString();
				row["Zip"]=rawtable.Rows[i]["Zip"].ToString();
				row["annualMaxInd"]=(PIn.Double(rawtable.Rows[i]["AnnualMaxInd"].ToString())).ToString("N");
				row["annualMaxFam"]=(PIn.Double(rawtable.Rows[i]["AnnualMaxFam"].ToString())).ToString("N");
				row["amountUsedInd"]=(PIn.Double(rawtable.Rows[i]["AmountUsedInd"].ToString())).ToString("N");
				row["amountUsedFam"]=(PIn.Double(rawtable.Rows[i]["AmountUsedFam"].ToString())).ToString("N");
				row["amountPendingInd"]=(PIn.Double(rawtable.Rows[i]["AmountPendingInd"].ToString())).ToString("N");
				row["amountPendingFam"]=(PIn.Double(rawtable.Rows[i]["AmountPendingFam"].ToString())).ToString("N");
				row["amountRemainingInd"]=(PIn.Double(rawtable.Rows[i]["$AmtRemainingInd"].ToString())).ToString("N");
				row["amountRemainingFam"]=(PIn.Double(rawtable.Rows[i]["$AmtRemainingFam"].ToString())).ToString("N");
				row["treatmentPlan"]=(PIn.Double(rawtable.Rows[i]["$TreatmentPlan"].ToString())).ToString("N");
				row["carrierName"]=rawtable.Rows[i]["CarrierName"].ToString();
				row["clinicAbbr"]=rawtable.Rows[i]["clinicAbbr"].ToString();
				rows.Add(row);
			}
			for(int i=0;i<rows.Count;i++) {
				table.Rows.Add(rows[i]);
			}
#if DEBUG
			sw.Stop();
			Console.WriteLine("Finished Filling query result: {0}",sw.ElapsedMilliseconds);
#endif
			return table;
		}
	}
}
