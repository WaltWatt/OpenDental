using CodeBase;
using System;
using System.Data;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace OpenDentBusiness {
	///<summary></summary>
	public class Benefits {
		#region Get Methods
		#endregion

		#region Modification Methods
		
		#region Insert
		#endregion

		#region Update
		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods
		#endregion

		///<summary>Gets a list of all benefits for a given list of patplans for one patient.</summary>
		public static List <Benefit> Refresh(List<PatPlan> listForPat,List<InsSub> subList) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Benefit>>(MethodBase.GetCurrentMethod(),listForPat,subList);
			}
			//Only need to check listForPat for null / empty because InsSubs.GetSub() handles a null / empty subList.
			if(listForPat==null || listForPat.Count==0) {
				return new List<Benefit>();
			}
			string command="SELECT * FROM benefit "
				//null safe, returns new InsSub with PlanNum 0 if GetSub doesn't find a match in neither subList nor the db
				+"WHERE PlanNum IN ("+string.Join(",",listForPat.Select(x => POut.Long(InsSubs.GetSub(x.InsSubNum,subList).PlanNum)))+") "
				+"OR PatPlanNum IN ("+string.Join(",",listForPat.Select(x => POut.Long(x.PatPlanNum)))+")";
			List<Benefit> list=Crud.BenefitCrud.SelectMany(command);
			list.Sort(SortBenefits);
			return list;
		}

		public static int SortBenefits(Benefit b1,Benefit b2){
			return b1.ToString().CompareTo(b2.ToString());
		}

		///<summary>Used in the PlanEdit and FormClaimProc to get a list of benefits for specified plan and patPlan.  patPlanNum can be 0.</summary>
		public static List<Benefit> RefreshForPlan(long planNum,long patPlanNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Benefit>>(MethodBase.GetCurrentMethod(),planNum,patPlanNum);
			}
			string command="SELECT *"//,IFNULL(covcat.CovCatNum,0) AS covorder "
				+" FROM benefit"
				//+" LEFT JOIN covcat ON covcat.CovCatNum=benefit.CovCatNum"
				+" WHERE PlanNum = "+POut.Long(planNum);
			if(patPlanNum!=0) {
				command+=" OR PatPlanNum = "+POut.Long(patPlanNum);
			}
			List<Benefit> list=Crud.BenefitCrud.SelectMany(command);
			list.Sort(SortBenefits);
			return list;
		}

		///<summary>No need to pass in usernum, it is set before the remoting role and passed in for logging.</summary>
		public static void Update(Benefit ben,Benefit benOld, long userNum = 0) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				userNum=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),ben,benOld,userNum);
				return;
			}
			Crud.BenefitCrud.Update(ben,benOld);
			InsEditLogs.MakeLogEntry(ben,benOld,InsEditLogType.Benefit,userNum);
		}

		///<summary>No need to pass in usernum, it is set before the remoting role and passed in for logging.</summary>
		public static long Insert(Benefit ben, long userNum = 0) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				userNum=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				ben.BenefitNum=Meth.GetLong(MethodBase.GetCurrentMethod(),ben,userNum);
				return ben.BenefitNum;
			}
			long benNum=Crud.BenefitCrud.Insert(ben);
			if(ben.PlanNum != 0) {//Does not log PatPlan benefits
				InsEditLogs.MakeLogEntry(ben,null,InsEditLogType.Benefit,userNum);
			}
			return benNum;
		}

		///<summary>No need to pass in usernum, it is set before the remoting role and passed in for logging.</summary>
		public static void Delete(Benefit ben,long userNum = 0) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				userNum=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),ben,userNum);
				return;
			}
			string command="DELETE FROM benefit WHERE BenefitNum ="+POut.Long(ben.BenefitNum);
			Db.NonQ(command);
			InsEditLogs.MakeLogEntry(null,ben,InsEditLogType.Benefit,userNum);
		}

		///<summary>Only for display purposes rather than any calculations.  Gets an annual max from the supplied list of benefits.  Ignores benefits that do not match either the planNum or the patPlanNum.  Because it starts at the top of the benefit list, it will get the most general limitation first.  Returns -1 if none found.  Usually, set isFam to false unless we are specifically interested in that value.</summary>
		public static double GetAnnualMaxDisplay(List<Benefit> benList,long planNum,long patPlanNum,bool isFam) {
			//No need to check RemotingRole; no call to db.
			List<Benefit> matchingBens=new List<Benefit>();
			for(int i=0;i<benList.Count;i++) {
				if(benList[i].PlanNum==0 && benList[i].PatPlanNum!=patPlanNum) {
					continue;
				}
				if(benList[i].PatPlanNum==0 && benList[i].PlanNum!=planNum) {
					continue;
				}
				if(benList[i].BenefitType!=InsBenefitType.Limitations) {
					continue;
				}
				if(benList[i].QuantityQualifier!=BenefitQuantity.None) {
					continue;
				}
				if(benList[i].TimePeriod!=BenefitTimePeriod.CalendarYear && benList[i].TimePeriod!=BenefitTimePeriod.ServiceYear) {
					continue;
				}
				if(isFam){
					if(benList[i].CoverageLevel!=BenefitCoverageLevel.Family){//individ or none
						continue;
					}
				}
				else{
					if(benList[i].CoverageLevel!=BenefitCoverageLevel.Individual) {//Family or None
						continue;
					}
				}
				//coverage level?
				if(benList[i].CodeNum != 0) {
					continue;
				}
				if(benList[i].CovCatNum != 0) {
					EbenefitCategory eben=CovCats.GetEbenCat(benList[i].CovCatNum);
					if(eben != EbenefitCategory.General && eben != EbenefitCategory.None) {
						continue;
					}
				}
				matchingBens.Add(benList[i]);
			}
			if(matchingBens.Count==0) {
				return -1;
			}
			//Get minimum benefit amount, preferring benefits with no catagory.
			return matchingBens.OrderBy(x => x.CovCatNum!=0).ThenBy(x => x.MonetaryAmt).First().MonetaryAmt;
		}

		///<summary>Only for display purposes rather than any calculations.  Gets a general deductible from the supplied list of benefits.  Ignores benefits that do not match either the planNum or the patPlanNum.</summary>
		public static double GetDeductGeneralDisplay(List<Benefit> benList,long planNum,long patPlanNum,BenefitCoverageLevel level) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<benList.Count;i++) {
				if(benList[i].PlanNum==0 && benList[i].PatPlanNum!=patPlanNum) {
					continue;
				}
				if(benList[i].PatPlanNum==0 && benList[i].PlanNum!=planNum) {
					continue;
				}
				if(benList[i].BenefitType!=InsBenefitType.Deductible) {
					continue;
				}
				if(benList[i].QuantityQualifier!=BenefitQuantity.None) {
					continue;
				}
				if(benList[i].TimePeriod!=BenefitTimePeriod.CalendarYear && benList[i].TimePeriod!=BenefitTimePeriod.ServiceYear) {
					continue;
				}
				if(benList[i].CoverageLevel != level) {
					continue;
				}
				if(benList[i].CodeNum != 0) {
					continue;
				}
				if(benList[i].CovCatNum != 0) {
					EbenefitCategory eben=CovCats.GetEbenCat(benList[i].CovCatNum);
					if(eben != EbenefitCategory.General && eben != EbenefitCategory.None) {
						continue;
					}
				}
				return benList[i].MonetaryAmt;
			}
			return -1;
		}

		///<summary>Used only in ClaimProcs.ComputeBaseEst.  Gets a deductible amount from the supplied list of benefits.  Ignores benefits that do not match either the planNum or the patPlanNum.  It figures out how much was already used and reduces the returned value by that amount.  Both individual and family deductibles will reduce the returned value independently.  Works for individual procs, categories, and general.</summary>
		public static double GetDeductibleByCode(List<Benefit> benList,long planNum,long patPlanNum,DateTime procDate,string procCode,List<ClaimProcHist> histList,List<ClaimProcHist> loopList,InsPlan plan,long patNum) {
			//No need to check RemotingRole; no call to db.
			#region filter benList for deductibles
			//first, create a much shorter list with only relevant benefits in it.
			List<Benefit> listShort=new List<Benefit>();
			for(int i=0;i<benList.Count;i++) {
				if(benList[i].PlanNum==0 && benList[i].PatPlanNum!=patPlanNum) {
					continue;
				}
				if(benList[i].PatPlanNum==0 && benList[i].PlanNum!=planNum) {
					continue;
				}
				if(benList[i].BenefitType!=InsBenefitType.Deductible) {
					continue;
				}
				//if(benList[i].QuantityQualifier!=BenefitQuantity.None) {
				//	continue;
				//}
				if(benList[i].TimePeriod!=BenefitTimePeriod.CalendarYear 
					&& benList[i].TimePeriod!=BenefitTimePeriod.ServiceYear
					&& benList[i].TimePeriod!=BenefitTimePeriod.Lifetime)//this is probably only going to be used in annual max, though
				{
					continue;
				}
				listShort.Add(benList[i]);
			}
			#endregion
			#region get individual deductibles
			//look for the best matching individual deduct----------------------------------------------------------------
			Benefit benIndGeneral=null;
			Benefit benInd=null;
			#region no category
			for(int i=0;i<listShort.Count;i++){
				if(listShort[i].CoverageLevel == BenefitCoverageLevel.Family){
					continue;
				}
				if(listShort[i].CodeNum>0){
					continue;
				}
				if(listShort[i].CovCatNum==0){
					benInd=listShort[i];
					//This deductible must be a general deductible since it has no associated category
					benIndGeneral=listShort[i];//sum of deductibles should not exceed this amount, even if benInd is less.
				}
			}
			#endregion
			#region specific category.
			CovSpan[] spansForCat;
			for(int i=0;i<listShort.Count;i++){
				if(listShort[i].CoverageLevel == BenefitCoverageLevel.Family){
					continue;
				}
				if(listShort[i].CodeNum>0){
					continue;
				}
				if(listShort[i].CovCatNum!=0){
					//see if the span matches
					spansForCat=CovSpans.GetForCat(listShort[i].CovCatNum);
					bool isMatch=false;
					for(int j=0;j<spansForCat.Length;j++){
						if(String.Compare(procCode,spansForCat[j].FromCode)>=0 && String.Compare(procCode,spansForCat[j].ToCode)<=0){
							isMatch=true;
							break;
						}
					}
					if(!isMatch) {
						continue;//no match
					}
					if(benInd != null && benInd.CovCatNum!=0){//must compare
						//only use the new one if the item order is larger
						if(CovCats.GetOrderShort(listShort[i].CovCatNum) > CovCats.GetOrderShort(benInd.CovCatNum)){
							benInd=listShort[i];
						}
					}
					else{//first one encountered for a category
						benInd=listShort[i];
					}
				}
			}
			#endregion
			#region specific code
			for(int i=0;i<listShort.Count;i++){
				if(listShort[i].CoverageLevel == BenefitCoverageLevel.Family){
					continue;
				}
				if(listShort[i].CodeNum==0){
					continue;
				}
				if(procCode==ProcedureCodes.GetStringProcCode(listShort[i].CodeNum)){
					benInd=listShort[i];
				}
			}
			#endregion
			#endregion
			#region get family deductibles
			//look for the best matching family deduct----------------------------------------------------------------
			Benefit benFam=null;
			#region no category
			for(int i=0;i<listShort.Count;i++) {
				if(listShort[i].CoverageLevel != BenefitCoverageLevel.Family) {
					continue;
				}
				if(listShort[i].CodeNum>0) {
					continue;
				}
				if(listShort[i].CovCatNum==0) {
					benFam=listShort[i];
				}
			}
			#endregion
			#region specific category.
			for(int i=0;i<listShort.Count;i++) {
				if(listShort[i].CoverageLevel != BenefitCoverageLevel.Family) {
					continue;
				}
				if(listShort[i].CodeNum>0) {
					continue;
				}
				if(listShort[i].CovCatNum!=0) {
					//see if the span matches
					spansForCat=CovSpans.GetForCat(listShort[i].CovCatNum);
					bool isMatch=false;
					for(int j=0;j<spansForCat.Length;j++) {
						if(String.Compare(procCode,spansForCat[j].FromCode)>=0 && String.Compare(procCode,spansForCat[j].ToCode)<=0) {
							isMatch=true;
							break;
						}
					}
					if(!isMatch) {
						continue;//no match
					}
					if(benFam != null && benFam.CovCatNum!=0) {//must compare
						//only use the new one if the item order is larger
						if(CovCats.GetOrderShort(listShort[i].CovCatNum) > CovCats.GetOrderShort(benFam.CovCatNum)) {
							benFam=listShort[i];
						}
					}
					else {//first one encountered for a category
						benFam=listShort[i];
					}
				}
			}
			#endregion
			#region specific code
			for(int i=0;i<listShort.Count;i++) {
				if(listShort[i].CoverageLevel != BenefitCoverageLevel.Family) {
					continue;
				}
				if(listShort[i].CodeNum==0) {
					continue;
				}
				if(procCode==ProcedureCodes.GetStringProcCode(listShort[i].CodeNum)) {
					benFam=listShort[i];
				}
			}
			#endregion
			#endregion
			//example. $50 individual deduct, $150 family deduct.
			//Only individual deductibles make sense as the starting point.
			//Family deductible just limits the sum of individual deductibles.
			//If there is no individual deductible that matches, then return 0.
			if(benInd==null || benInd.MonetaryAmt==-1 || benInd.MonetaryAmt==0) {
				return 0;
			}
			double retVal=benInd.MonetaryAmt;
			//Reduce by the sum of the deductibles on claim procs associated with claims in the benefit period for individual or family as appropriate
			#region reduce by amount individual already paid this year			
			//establish date range for procedures to consider
			DateTime dateStart=BenefitLogic.ComputeRenewDate(procDate,plan.MonthRenew);
			DateTime dateEnd=dateStart.AddYears(1).AddDays(-1);//Consider all claim procs with a ProcDate within one year of the renew date
			if(benInd.TimePeriod==BenefitTimePeriod.Lifetime) {
				dateStart=DateTime.MinValue;
			}
			for(int i=0;i<histList.Count;i++) {
				if(histList[i].PlanNum != planNum) {
					continue;//different plan
				}
				if(histList[i].ProcDate<dateStart || histList[i].ProcDate>dateEnd) {
					continue;
				}
				if(histList[i].PatNum != patNum) {
					continue;//this is for someone else in the family
				}
				//Procedure specific deductibles need to take all deductibles into consideration, not just deductibles that have been applied towards the specific procedure.
				//if(benInd.CodeNum!=0) {//specific code
				//	if(ProcedureCodes.GetStringProcCode(benInd.CodeNum)!=histList[i].StrProcCode) {
				//		continue;
				//	}
				//}
				else if(histList[i].Status==ClaimProcStatus.Adjustment) {
					//We always want to apply the deductible for any adjustment to insurance benefits.
					//This code must be directly above the following "else if(benInd.CovCatNum!=0) {" because adjustments do not have procedure codes to compare.
					//Therefore, the following else if block will not find a match and will continue looping through histList instead of correctly applying deductible.
					if(benInd.CovCatNum!=0 && CovCats.GetEbenCat(benInd.CovCatNum)!=EbenefitCategory.General) {//This ensures that the deductible adjustment will only apply to General and None categories.
						continue;
					}
				}
				else if(benInd.CovCatNum!=0) {//specific category
					spansForCat=CovSpans.GetForCat(benInd.CovCatNum);
					bool isMatch=false;
					for(int j=0;j<spansForCat.Length;j++) {
						if(String.Compare(histList[i].StrProcCode,spansForCat[j].FromCode)>=0 
							&& String.Compare(histList[i].StrProcCode,spansForCat[j].ToCode)<=0) 
						{
							isMatch=true;
							break;
						}
					}
					if(!isMatch) {
						continue;
					}
				}
				//if no category, then benefits are not restricted by proc code.
				if(histList[i].Deduct==-1) {
					continue;
				}
				retVal-=histList[i].Deduct;
			}
			#endregion
			#region reduce by amount individual already paid in loopList
			//now, do a similar thing with loopList, individ-----------------------------------------------------------------------
			#region diagnostic/preventive workaround
			//There is a kludgey workaround in the loop below.
			//It handles the very specific problem of a single diagnostic/preventive deductible even though we have two separate benefits for it.
			//The better solution will be benefits with multiple categories or spans.
			//This workaround is only applied if there are both a diagnostic and a preventive deductible for the same amount.
			//If the benefit is diagnostic, then also check the spans of the preventive category
			CovSpan[] otherSpans=null;
			if(CovCats.GetEbenCat(benInd.CovCatNum)==EbenefitCategory.Diagnostic){
				for(int i=0;i<listShort.Count;i++){//look through the benefits again
					if(listShort[i].CoverageLevel!=BenefitCoverageLevel.Individual){
						continue;
					}
					if(CovCats.GetEbenCat(listShort[i].CovCatNum)!=EbenefitCategory.RoutinePreventive){
						continue;
					}
					otherSpans=CovSpans.GetForCat(listShort[i].CovCatNum);
				}
			}
			//if the benefit is preventive, then also check the spans of the diagnostic category
			if(CovCats.GetEbenCat(benInd.CovCatNum)==EbenefitCategory.RoutinePreventive){
				for(int i=0;i<listShort.Count;i++){//look through the benefits again
					if(listShort[i].CoverageLevel!=BenefitCoverageLevel.Individual){
						continue;
					}
					if(CovCats.GetEbenCat(listShort[i].CovCatNum)!=EbenefitCategory.Diagnostic){
						continue;
					}
					otherSpans=CovSpans.GetForCat(listShort[i].CovCatNum);
				}
			}
			#endregion
			for(int i=0;i<loopList.Count;i++) {
				#region filter loopList
				//no date restriction, since all TP or part of current claim
				//if(histList[i].ProcDate<dateStart || histList[i].ProcDate>dateEnd) {
				//	continue;
				//}
				if(loopList[i].PlanNum != planNum) {
					continue;//different plan.  Even the loop list can contain info for multiple plans.
				}
				if(loopList[i].PatNum != patNum) {
					continue;//this is for someone else in the family
				}
				//Loop list needs to consider procedure specific codes so that deductibles apply to other procedures within a TP if necessary.  E.g. Unit Test 16
				if(benInd.CodeNum!=0) {//specific code
					if(ProcedureCodes.GetStringProcCode(benInd.CodeNum)!=loopList[i].StrProcCode) {
						continue;
					}
				}
				else if(benInd.CovCatNum!=0) {//specific category
					spansForCat=CovSpans.GetForCat(benInd.CovCatNum);
					bool isMatch=false;
					for(int j=0;j<spansForCat.Length;j++) {
						if(String.Compare(loopList[i].StrProcCode,spansForCat[j].FromCode)>=0 
							&& String.Compare(loopList[i].StrProcCode,spansForCat[j].ToCode)<=0) {
							isMatch=true;
							break;
						}
					}
					if(otherSpans!=null){
						for(int j=0;j<otherSpans.Length;j++) {
							if(String.Compare(loopList[i].StrProcCode,otherSpans[j].FromCode)>=0 
								&& String.Compare(loopList[i].StrProcCode,otherSpans[j].ToCode)<=0) {
								isMatch=true;
								break;
							}
						}
					}
					if(!isMatch) {
						continue;
					}
				}
				//if no category, then benefits are not restricted by proc code.
				if(loopList[i].Deduct==-1) {
					continue;
				}
				#endregion
				retVal-=loopList[i].Deduct;
			}
			#endregion
			if(retVal<=0) {
				return 0;
			}
			double deductUsedInLoopList=0;//sum up the deductibles in looplist for the current plan
			List<ClaimProcHist> loopListInsPlan=loopList.FindAll(x => x.PlanNum==planNum && x.PatNum==patNum);
			foreach(ClaimProcHist loopListItem in loopListInsPlan) {
				deductUsedInLoopList+=Math.Max(loopListItem.Deduct,0);
			}
			if(benIndGeneral!=null) {//if there exists a general deductible
				if((retVal + deductUsedInLoopList) > benIndGeneral.MonetaryAmt) {
					//If the potential deductible amount plus the deductible amount that has been allocated to other TP procedures is greater than the general 
					//deductible amount, reduce the potential deductible by the amount that the potential deductible plus the deductible amount for other TP 
					//procedures exceeds the general deductible. Example: if (25+45) > 50, then return 50-45=5. Where 25 is the potential deductible amount, 
					//45 is the amount that has been allocated to other TP procedures, and 50 is the general deductible amount.
					retVal=benIndGeneral.MonetaryAmt - deductUsedInLoopList;// (fix for Unit Test 16)
				}
			}
			//if there is still a deductible, we might still reduce it based on family ded used.
			if(benFam==null || benFam.MonetaryAmt==-1) {
				return retVal;
			}
			double famded=benFam.MonetaryAmt;
			#region reduce by amount family already paid this year
			//reduce the family deductible by amounts already used----------------------------------------------------------
			for(int i=0;i<histList.Count;i++) {
				if(histList[i].ProcDate<dateStart || histList[i].ProcDate>dateEnd) {
					continue;
				}
				if(histList[i].PlanNum != planNum) {
					continue;//different plan
				}
				//now, we do want to see all family members.
				//if(histList[i].PatNum != patNum) {
				//	continue;//this is for someone else in the family
				//}
				//Procedure specific deductibles need to take all deductibles into consideration, not just deductibles that have been applied towards the specific procedure.
				//if(benFam.CodeNum!=0) {//specific code
				//	if(ProcedureCodes.GetStringProcCode(benFam.CodeNum)!=histList[i].StrProcCode) {
				//		continue;
				//	}
				//}
				else if(benFam.CovCatNum!=0) {//specific category
					spansForCat=CovSpans.GetForCat(benFam.CovCatNum);
					bool isMatch=false;
					for(int j=0;j<spansForCat.Length;j++) {
						if(String.Compare(histList[i].StrProcCode,spansForCat[j].FromCode)>=0 
							&& String.Compare(histList[i].StrProcCode,spansForCat[j].ToCode)<=0) {
							isMatch=true;
							break;
						}
					}
					if(!isMatch) {
						continue;
					}
				}
				//if no category, then benefits are not restricted by proc code.
				if(histList[i].Deduct==-1) {
					continue;
				}
				famded-=histList[i].Deduct;
			}
			#endregion
			#region reduce by amount family already paid in loopList
			//reduce family ded by amounts already used in loop---------------------------------------------------------------
			for(int i=0;i<loopList.Count;i++) {
				if(loopList[i].PlanNum != planNum) {
					continue;//different plan
				}
				//Loop list needs to consider procedure specific codes so that deductibles apply to other procedures within a TP if necessary.
				if(benFam.CodeNum!=0) {//specific code
					if(ProcedureCodes.GetStringProcCode(benFam.CodeNum)!=loopList[i].StrProcCode) {
						continue;
					}
				}
				else if(benFam.CovCatNum!=0) {//specific category
					spansForCat=CovSpans.GetForCat(benFam.CovCatNum);
					bool isMatch=false;
					for(int j=0;j<spansForCat.Length;j++) {
						if(String.Compare(loopList[i].StrProcCode,spansForCat[j].FromCode)>=0 
							&& String.Compare(loopList[i].StrProcCode,spansForCat[j].ToCode)<=0) {
							isMatch=true;
							break;
						}
					}
					if(!isMatch) {
						continue;
					}
				}
				//if no category, then benefits are not restricted by proc code.
				if(loopList[i].Deduct==-1) {
					continue;
				}
				famded-=loopList[i].Deduct;
			}
			#endregion
			//if the family deductible has all been used up on other procs
			if(famded<=0) {
				return 0;//then no deductible, regardless of what we computed for individual
			}
			if(retVal > famded) {//example; retInd=$50, but 120 of 150 family ded has been used.  famded=30.  We need to return 30.
				return famded;
			}
			return retVal;
		}

		///<summary>Used only in ClaimProcs.ComputeBaseEst.  Calculates the most specific limitation for the specified code.  
		///This is usually an annual max, ortho max, or fluoride limitation (only if age match).  
		///Ignores benefits that do not match either the planNum or the patPlanNum.  
		///It figures out how much was already used and reduces the returned value by that amount.  
		///Both individual and family limitations will reduce the returned value independently.  
		///Works for individual procs, categories, and general.  Also outputs a string description of the limitation.  
		///There don't seem to be any situations where multiple limitations would each partially reduce coverage for a single code, other than ind/fam.  
		///The returned value will be the original insEstTotal passed in unless there was some limitation that reduced it.
		///Considers InsEstTotalOverride when dynamically writing the EstimateNote.</summary>
		public static double GetLimitationByCode(List<Benefit> benList,long planNum,long patPlanNum,DateTime procDate,string procCodeStr,List<ClaimProcHist> histList,List<ClaimProcHist> loopList,InsPlan plan,long patNum,out string note,double insEstTotal,int patientAge,long insSubNum,double insEstTotalOverride) {
			//No need to check RemotingRole;no call to db.
			note ="";
			//first, create a much shorter list with only relevant benefits in it.
			List<Benefit> listShort=new List<Benefit>();
			for(int i=0;i<benList.Count;i++) {
				if(benList[i].PlanNum==0 && benList[i].PatPlanNum!=patPlanNum) {
					continue;
				}
				if(benList[i].PatPlanNum==0 && benList[i].PlanNum!=planNum) {
					continue;
				}
				if(benList[i].BenefitType!=InsBenefitType.Limitations) {
					continue;
				}
				//if(benList[i].TimePeriod!=BenefitTimePeriod.CalendarYear 
				//	&& benList[i].TimePeriod!=BenefitTimePeriod.ServiceYear
				//	&& benList[i].TimePeriod!=BenefitTimePeriod.Lifetime)
				//{
				//	continue;
				//}
				listShort.Add(benList[i]);
			}
			//look for the best matching individual limitation----------------------------------------------------------------
			Benefit benInd=null;
			//start with no category
			for(int i=0;i<listShort.Count;i++) {
				if(listShort[i].CoverageLevel == BenefitCoverageLevel.Family) {
					continue;
				}
				if(listShort[i].CodeNum>0) {
					continue;
				}
				if(listShort[i].CovCatNum==0) {
					if(!IsCodeInGeneralSpan(procCodeStr)) {
						continue;
					}
					//Leave this outside of the isOutsideGeneralSpan check incase there is no general category.
					benInd=listShort[i];
				}
			}
			//then, specific category.
			CovSpan[] spansForCat;
			for(int i=0;i<listShort.Count;i++) {
				if(listShort[i].CoverageLevel == BenefitCoverageLevel.Family) {
					continue;
				}
				if(listShort[i].CodeNum>0) {
					continue;
				}
				if(listShort[i].CovCatNum!=0) {
					//see if the span matches
					spansForCat=CovSpans.GetForCat(listShort[i].CovCatNum);
					bool isMatch=false;
					for(int j=0;j<spansForCat.Length;j++) {
						if(String.Compare(procCodeStr,spansForCat[j].FromCode)>=0 && String.Compare(procCodeStr,spansForCat[j].ToCode)<=0) {
							isMatch=true;
							break;
						}
					}
					if(!isMatch) {
						continue;//no match
					}
					if(listShort[i].QuantityQualifier==BenefitQuantity.NumberOfServices
						|| listShort[i].QuantityQualifier==BenefitQuantity.Months
						|| listShort[i].QuantityQualifier==BenefitQuantity.Years) 
					{
						continue;//exclude frequencies
					}
					//If it's an age based limitation, then make sure the patient age matches.
					//If we have an age match, then we exit the method right here.
					if(listShort[i].QuantityQualifier==BenefitQuantity.AgeLimit && listShort[i].Quantity > 0) {
						if(patientAge > listShort[i].Quantity) {
							note=Lans.g("Benefits","Age limitation:")+" "+listShort[i].Quantity.ToString();
							return 0;//not covered if too old.
						}
						continue;//don't use an age limitation for the match if the patient has not reached the age limit
					}
					if(benInd != null && benInd.CovCatNum!=0) {//must compare
						//only use the new one if the item order is larger
						if(CovCats.GetOrderShort(listShort[i].CovCatNum) > CovCats.GetOrderShort(benInd.CovCatNum)) {
							benInd=listShort[i];
						}
					}
					else {//first one encountered for a category
						benInd=listShort[i];
					}
				}
			}
			//then, specific code
			for(int i=0;i<listShort.Count;i++) {
				if(listShort[i].CoverageLevel == BenefitCoverageLevel.Family) {
					continue;
				}
				if(listShort[i].CodeNum==0) {
					continue;
				}
				if(procCodeStr!=ProcedureCodes.GetStringProcCode(listShort[i].CodeNum)) {
					continue;
				}
				if(listShort[i].QuantityQualifier==BenefitQuantity.NumberOfServices
					|| listShort[i].QuantityQualifier==BenefitQuantity.Months
					|| listShort[i].QuantityQualifier==BenefitQuantity.Years) 
				{
					continue;//exclude frequencies
				}
				//if it's an age based limitation, then make sure the patient age matches.
				//If we have an age match, then we exit the method right here.
				if(listShort[i].QuantityQualifier==BenefitQuantity.AgeLimit && listShort[i].Quantity > 0){
					if(patientAge > listShort[i].Quantity){
						note=Lans.g("Benefits","Age limitation:")+" "+listShort[i].Quantity.ToString();
						return 0;//not covered if too old.
					}
				}
				else{//anything but an age limit
					benInd=listShort[i];
				}
			}
			//look for the best matching family limitation----------------------------------------------------------------
			Benefit benFam=null;
			//start with no category
			for(int i=0;i<listShort.Count;i++) {
				if(listShort[i].CoverageLevel != BenefitCoverageLevel.Family) {
					continue;
				}
				if(listShort[i].CodeNum>0) {
					continue;
				}
				if(listShort[i].CovCatNum==0) {
					if(!IsCodeInGeneralSpan(procCodeStr)) {
						continue;
					}
					//Leave this outside of the IsCodeInGeneralSpan check incase there is no general category.
					benFam=listShort[i];
				}
			}
			//then, specific category.
			for(int i=0;i<listShort.Count;i++) {
				if(listShort[i].CoverageLevel != BenefitCoverageLevel.Family) {
					continue;
				}
				if(listShort[i].CodeNum>0) {
					continue;
				}
				if(listShort[i].CovCatNum!=0) {
					//see if the span matches
					spansForCat=CovSpans.GetForCat(listShort[i].CovCatNum);
					bool isMatch=false;
					for(int j=0;j<spansForCat.Length;j++) {
						if(String.Compare(procCodeStr,spansForCat[j].FromCode)>=0 && String.Compare(procCodeStr,spansForCat[j].ToCode)<=0) {
							isMatch=true;
							break;
						}
					}
					if(!isMatch) {
						continue;//no match
					}
					if(benFam != null && benFam.CovCatNum!=0) {//must compare
						//only use the new one if the item order is larger
						if(CovCats.GetOrderShort(listShort[i].CovCatNum) > CovCats.GetOrderShort(benFam.CovCatNum)) {
							benFam=listShort[i];
						}
					}
					else {//first one encountered for a category
						benFam=listShort[i];
					}
				}
			}
			//then, specific code
			for(int i=0;i<listShort.Count;i++) {
				if(listShort[i].CoverageLevel != BenefitCoverageLevel.Family) {
					continue;
				}
				if(listShort[i].CodeNum==0) {
					continue;
				}
				if(procCodeStr==ProcedureCodes.GetStringProcCode(listShort[i].CodeNum)) {
					benFam=listShort[i];
				}
			}
			//example. $1000 individual max, $3000 family max.
			//Only individual max makes sense as the starting point.								Only family max is now being considered as well.
			//Family max just limits the sum of individual maxes.										Family max may be the only cap being used.
			//If there is no individual limitation that matches, then return 0.     No longer valid. Return amount covered by ins, whether individual or family max.
			//fluoride age limit already handled, so all that's left is maximums.   ...
			if((benInd==null || benInd.MonetaryAmt==-1 || benInd.MonetaryAmt==0) && (benFam==null || benFam.MonetaryAmt==-1 || benFam.MonetaryAmt==0)){ 
			//if(benInd==null || benInd.MonetaryAmt==-1 || benInd.MonetaryAmt==0) {
				return insEstTotal;//no max found for this code.
			}
			double maxInd=0;
			if(benInd!=null) {
				maxInd=benInd.MonetaryAmt;
			}
			//reduce individual max by amount already paid this year/lifetime---------------------------------------------------
			//establish date range for procedures to consider
			DateTime dateStart=BenefitLogic.ComputeRenewDate(procDate,plan.MonthRenew);
			DateTime dateEnd=dateStart.AddYears(1).AddDays(-1);//Consider all claim procs with a ProcDate within one year of the renew date
			//Get deep copies of some cache classes so that we do not waste time in loops down below.
			if(benInd!=null) {
				if(benInd.TimePeriod==BenefitTimePeriod.Lifetime) {
					dateStart=DateTime.MinValue;
				}
				for(int i=0;i<histList.Count;i++) {
					if(histList[i].InsSubNum != insSubNum) {
						continue;//different plan
					}
					if(histList[i].ProcDate<dateStart || histList[i].ProcDate>dateEnd) {
						continue;
					}
					if(histList[i].PatNum != patNum) {
						continue;//this is for someone else in the family //SHOULD PROBABLY NOT SKIP THIS IN THE CASE OF FAM BUT NO IND MAX. :(
					}
					if(benInd.CodeNum!=0) {//specific code
						//Enhance this later when code spans are supported.
						if(ProcedureCodes.GetStringProcCode(benInd.CodeNum)!=histList[i].StrProcCode) {
							continue;
						}
					}
					else if(benInd.CovCatNum!=0) {//specific category
						spansForCat=CovSpans.GetForCat(benInd.CovCatNum);
						bool isMatch=false;
						if(histList[i].StrProcCode=="") {//If this was a 'total' payment that was not attached to a procedure
							if(CovCats.GetEbenCat(benInd.CovCatNum)==EbenefitCategory.General) {//And if this is the general category
								//Then it should affect this max.
								isMatch=true;
							}
						}
						else {//If the payment was attached to a proc, then the proc must be in the coderange of this annual max benefit
							for(int j=0;j<spansForCat.Length;j++) {
								if(String.Compare(histList[i].StrProcCode,spansForCat[j].FromCode)>=0 
								&& String.Compare(histList[i].StrProcCode,spansForCat[j].ToCode)<=0) {
									isMatch=true;
									break;
								}
							}
						}
						if(!isMatch) {
							continue;
						}
					}
					//if no category, then benefits are not restricted by proc code.
					//In other words, the benefit applies to all codes.
					//At this point, we know that the proc in the loopList falls within this max benefit.
					//But it may also fall within a more restrictive benefit which would take precedence over this one.
					if(TighterLimitExists(listShort,benInd,histList[i])) {
						continue;
					}
					maxInd-=histList[i].Amount;
				}
			}
			//reduce individual max by amount in loop ------------------------------------------------------------------
			if(benInd!=null) {
				for(int i=0;i<loopList.Count;i++) {
					//no date restriction, since all TP or part of current claim
					//if(histList[i].ProcDate<dateStart || histList[i].ProcDate>dateEnd) {
					//	continue;
					//}
					if(loopList[i].InsSubNum != insSubNum) {
						continue;//different plan.  Even the loop list can contain info for multiple plans.
					}
					if(loopList[i].PatNum != patNum) {
						continue;//this is for someone else in the family
					}
					if(benInd.CodeNum!=0) {//specific code
						//Enhance this later when code spans are supported.
						if(ProcedureCodes.GetStringProcCode(benInd.CodeNum)!=loopList[i].StrProcCode) {
							continue;
						}
					}
					else if(benInd.CovCatNum!=0) {//specific category
						spansForCat=CovSpans.GetForCat(benInd.CovCatNum);
						bool isMatch=false;
						if(loopList[i].StrProcCode=="") {//If this was a 'total' payment that was not attached to a procedure
							if(CovCats.GetEbenCat(benInd.CovCatNum)==EbenefitCategory.General) {//And if this is the general category
								//Then it should affect this max.
								isMatch=true;
							}
						}
						else {//If the payment was attached to a proc, then the proc must be in the coderange of this annual max benefit
							for(int j=0;j<spansForCat.Length;j++) {
								if(String.Compare(loopList[i].StrProcCode,spansForCat[j].FromCode)>=0 
								&& String.Compare(loopList[i].StrProcCode,spansForCat[j].ToCode)<=0) {
									isMatch=true;
									break;
								}
							}
						}
						if(!isMatch) {
							continue;
						}
					}
					else {//if no category, then benefits are not normally restricted by proc code.
						//The problem is that if the amount in the loop is from an ortho proc, then the general category will exclude ortho.
						//But sometimes, the annual max is in the system as no category instead of general category.
						CovCat generalCat=CovCats.GetForEbenCat(EbenefitCategory.General);
						if(generalCat!=null) {//If there is a general category, then we only consider codes within it.  This is how we exclude ortho.
							CovSpan[] covSpanArray=CovSpans.GetForCat(generalCat.CovCatNum);
							if(loopList[i].StrProcCode!="" && !CovSpans.IsCodeInSpans(loopList[i].StrProcCode,covSpanArray)) {//for example, ortho
								continue;
							}
						}
					}
					//At this point, we know that the proc in the loopList falls within this max benefit.
					//But it may also fall within a more restrictive benefit which would take precedence over this one.
					if(TighterLimitExists(listShort,benInd,loopList[i])) {
						continue;
					}
					maxInd-=loopList[i].Amount;
				}
			}
			if(benInd!=null) {
				if(maxInd <= 0) {//then patient has used up all of their annual max, so no coverage.
					if(benInd.TimePeriod==BenefitTimePeriod.Lifetime) {
						note+=Lans.g("Benefits","Over lifetime max");
					}
					else if(benInd.TimePeriod==BenefitTimePeriod.CalendarYear
						|| benInd.TimePeriod==BenefitTimePeriod.ServiceYear) {
						note+=Lans.g("Benefits","Over annual max");
					}
					return 0;
				}
			}
			double retVal=insEstTotal;
			if(benInd!=null){
				if(maxInd < insEstTotal) {//if there's not enough left in the individual annual max to cover this proc.
					retVal=maxInd;//insurance will only cover up to the remaining annual max
				}
			}
			double insRemainingOverride=insEstTotalOverride;  //insEstTotalOverride or the amount ind remaining.  Same concept as retVal but never returned.
			if(benInd!=null) {
				if(maxInd < insEstTotalOverride) {//if there's not enough left in the individual annual max to cover this proc.
					insRemainingOverride=maxInd;//insurance will only cover up to the remaining annual max
				}
			}
			//Three situations:
			//1. Ind only. 
 			//Handled in the next 10 lines, then return.
			//
			//2. Ind and Fam max present.  
			//There seems to be enough to cover at least part of this procedure.
			//There may also be a family max that has been met which may partially or completely reduce coverage of this proc.
			//
			//3. Fam only.  We don't know how much is left.
			//maxInd=-1
			//benInd=null
			if(benFam==null || benFam.MonetaryAmt==-1) {//if no family max.  Ind only.
				//If there is an override, calculate annual max note from it instead
				if(insEstTotalOverride==-1 && retVal != insEstTotal){//and procedure is not fully covered by ind max
					if(benInd!=null) {//redundant
						if(benInd.TimePeriod==BenefitTimePeriod.Lifetime) {
							note+=Lans.g("Benefits","Over lifetime max");
						}
						else if(benInd.TimePeriod==BenefitTimePeriod.CalendarYear
							|| benInd.TimePeriod==BenefitTimePeriod.ServiceYear) {
							note+=Lans.g("Benefits","Over annual max");
						}
					}
				}
				else if(insEstTotalOverride!=-1 && insRemainingOverride != insEstTotalOverride) {
					if(benInd!=null) {//redundant
						if(benInd.TimePeriod==BenefitTimePeriod.Lifetime) {
							note+=Lans.g("Benefits","Over lifetime max");
						}
						else if(benInd.TimePeriod==BenefitTimePeriod.CalendarYear
								|| benInd.TimePeriod==BenefitTimePeriod.ServiceYear) {
							note+=Lans.g("Benefits","Over annual max");
						}
					}
				}
				return retVal;//no family max anyway, so no need to go further.
			}
			double maxFam=benFam.MonetaryAmt;
			//reduce the family max by amounts already used----------------------------------------------------------
			for(int i=0;i<histList.Count;i++) {
				if(histList[i].ProcDate<dateStart || histList[i].ProcDate>dateEnd) {
					continue;
				}
				if(histList[i].PlanNum != planNum) {
					continue;//different plan
				}
				//now, we do want to see all family members.
				//if(histList[i].PatNum != patNum) {
				//	continue;//this is for someone else in the family
				//}
				if(benFam.CodeNum!=0) {//specific code
					if(ProcedureCodes.GetStringProcCode(benFam.CodeNum)!=histList[i].StrProcCode) {
						continue;
					}
				}
				else if(benFam.CovCatNum!=0) {//specific category
					spansForCat=CovSpans.GetForCat(benFam.CovCatNum);
					bool isMatch=false;
					if(histList[i].StrProcCode=="") {//If this was a 'total' payment that was not attached to a procedure
						if(CovCats.GetEbenCat(benFam.CovCatNum)==EbenefitCategory.General) {//And if this is the general category
							//Then it should affect this max.
							isMatch=true;
						}
					}
					else {
						for(int j=0;j<spansForCat.Length;j++) {
							if(String.Compare(histList[i].StrProcCode,spansForCat[j].FromCode)>=0 
							&& String.Compare(histList[i].StrProcCode,spansForCat[j].ToCode)<=0) {
								isMatch=true;
								break;
							}
						}
					}
					if(!isMatch) {
						continue;
					}
				}
				//if no category, then benefits are not restricted by proc code.
				maxFam-=histList[i].Amount;
			}
			//reduce family max by amounts already used in loop---------------------------------------------------------------
			for(int i=0;i<loopList.Count;i++) {
				if(loopList[i].PlanNum != planNum) {
					continue;//different plan
				}
				if(benFam.CodeNum!=0) {//specific code
					if(ProcedureCodes.GetStringProcCode(benFam.CodeNum)!=loopList[i].StrProcCode) {
						continue;
					}
				}
				else if(benFam.CovCatNum!=0) {//specific category
					spansForCat=CovSpans.GetForCat(benFam.CovCatNum);
					bool isMatch=false;
					if(loopList[i].StrProcCode=="") {//If this was a 'total' payment that was not attached to a procedure
						if(CovCats.GetEbenCat(benFam.CovCatNum)==EbenefitCategory.General) {//And if this is the general category
							//Then it should affect this max.
							isMatch=true;
						}
					}
					else {
						for(int j=0;j<spansForCat.Length;j++) {
							if(String.Compare(loopList[i].StrProcCode,spansForCat[j].FromCode)>=0 
							&& String.Compare(loopList[i].StrProcCode,spansForCat[j].ToCode)<=0) {
								isMatch=true;
								break;
							}
						}
					}
					if(!isMatch) {
						continue;
					}
				}
				//If the used family max is an umbrella max, 
				//don't subtract the estimates from procedures that are outside of the general coverage category 
				if(benFam.CovCatNum==0 && !IsCodeInGeneralSpan(loopList[i].StrProcCode)) {
					continue;
				}
				//if no category, then benefits are not restricted by proc code.
				maxFam-=loopList[i].Amount;
			}
			//if the family max has all been used up on other procs 
			if(maxFam<=0) {
				if(benInd==null) {
					note+=Lans.g("Benefits","Over family max");
				}
				else{//and there is an individual max.
					if(benInd.TimePeriod==BenefitTimePeriod.Lifetime) {
						note+=Lans.g("Benefits","Over family lifetime max");
					}
					else if(benInd.TimePeriod==BenefitTimePeriod.CalendarYear
					|| benInd.TimePeriod==BenefitTimePeriod.ServiceYear) {
						note+=Lans.g("Benefits","Over family annual max");
					}
				}
				return 0;//then no coverage, regardless of what we computed for individual
			}
			//This section was causing a bug. I'm really not even sure what it was attempting to do, since it's common for family max to be greater than indiv max
			/*if(maxFam > maxInd) {//restrict by maxInd
				//which we already calculated
				if(benInd.TimePeriod==BenefitTimePeriod.Lifetime) {
					note+=Lans.g("Benefits","Over lifetime max");
				}
				else if(benInd.TimePeriod==BenefitTimePeriod.CalendarYear
					|| benInd.TimePeriod==BenefitTimePeriod.ServiceYear) {
					note+=Lans.g("Benefits","Over annual max");
				}
				return retVal;
			}*/
			if((benInd==null) || (maxFam < maxInd)) {//restrict by maxFam
				if(insEstTotalOverride==-1) {//No insEstTotalOverride used, use normal insEstTotal
					if(maxFam < retVal) {//if there's not enough left in the annual max to cover this proc.
						//example. retVal=$70.  But 2970 of 3000 family max has been used.  maxFam=30.  We need to return 30.
						if(benInd==null) {
							note+=Lans.g("Benefits","Over family max");
						}
						else {//both ind and fam
							if(benInd.TimePeriod==BenefitTimePeriod.Lifetime) {
								note+=Lans.g("Benefits","Over family lifetime max");
							}
							else if(benInd.TimePeriod==BenefitTimePeriod.CalendarYear
								|| benInd.TimePeriod==BenefitTimePeriod.ServiceYear) {
								note+=Lans.g("Benefits","Over family annual max");
							}
						}
						return maxFam;//insurance will only cover up to the remaining annual max
					}
				}
				else {//If there is an override, calculate family max note from it instead
					//First calculate the note using override
					if(maxFam < insRemainingOverride) {//if there's not enough left in the annual max to cover this proc.
						//example. insRemainingOverride=$70.  But 2970 of 3000 family max has been used.  maxFam=30.
						if(benInd==null) {
							note+=Lans.g("Benefits","Over family max");
						}
						else {//both ind and fam
							if(benInd.TimePeriod==BenefitTimePeriod.Lifetime) {
								note+=Lans.g("Benefits","Over family lifetime max");
							}
							else if(benInd.TimePeriod==BenefitTimePeriod.CalendarYear
								|| benInd.TimePeriod==BenefitTimePeriod.ServiceYear) {
								note+=Lans.g("Benefits","Over family annual max");
							}
						}
					}
					//Now return the same amount you would have if there wasn't an override
					if(maxFam < retVal) {
						return maxFam;
					}
				}
			}
			if(insEstTotalOverride==-1) {//No insEstTotalOverride used, use normal insEstTotal
				if(retVal < insEstTotal) {//must have been an individual restriction
					if(benInd==null) {//js I don't understand this situation. It will probably not happen, but this is safe.
						note+=Lans.g("Benefits","Over annual max");
					}
					else {
						if(benInd.TimePeriod==BenefitTimePeriod.Lifetime) {
							note+=Lans.g("Benefits","Over lifetime max");
						}
						else if(benInd.TimePeriod==BenefitTimePeriod.CalendarYear
							|| benInd.TimePeriod==BenefitTimePeriod.ServiceYear) {
							note+=Lans.g("Benefits","Over annual max");
						}
					}
				}
			}
			//If there is an override, calculate max note from it instead.  Must have been an individual restriction.
			else if(insRemainingOverride < insEstTotalOverride) {
				if(benInd==null) {//js I don't understand this situation. It will probably not happen, but this is safe.
					note+=Lans.g("Benefits","Over annual max");
				}
				else {
					if(benInd.TimePeriod==BenefitTimePeriod.Lifetime) {
						note+=Lans.g("Benefits","Over lifetime max");
					}
					else if(benInd.TimePeriod==BenefitTimePeriod.CalendarYear
						|| benInd.TimePeriod==BenefitTimePeriod.ServiceYear) {
						note+=Lans.g("Benefits","Over annual max");
					}
				}
			}
			return retVal;
		}
		
		///<summary>Returns true if the passed in code is within the "General" insurance coverage category.</summary>
		private static bool IsCodeInGeneralSpan(string procCodeStr) {
			bool retVal=true;
			CovCat genCat=CovCats.GetForEbenCat(EbenefitCategory.General);
			if(genCat!=null) {//Should always happen.  This would mean they are missing the General Coverage Category.
				CovSpan[] spansForGeneral=CovSpans.GetForCat(genCat.CovCatNum);
				bool isMatch=false;
				for(int i=0;i<spansForGeneral.Length;i++) {
					if(String.Compare(procCodeStr,spansForGeneral[i].FromCode)>=0 && String.Compare(procCodeStr,spansForGeneral[i].ToCode)<=0) {
						isMatch=true;
						break;
					}
				}
				if(isMatch) {
					retVal=true;
				}
				else {
					retVal=false;
					//no match for the code in the general category.  This will specifically happen for ortho codes, 
					//which should never consider general limitations (per Nathan, see TaskNum 807256)
				}
			}
			return retVal;
		}

		private static bool TighterLimitExists(List<Benefit> listShort,Benefit benefit,ClaimProcHist claimProcHist) 
		{
			if(claimProcHist.StrProcCode=="") {//If this was a 'total' payment that was not attached to a procedure
				//there won't be anything more restrictive.
				return false;
			}
			//The list is not in order by restrictive/broad, so tests will need to be done.
			if(benefit.CodeNum!=0) {//The benefit is already restrictive.  There isn't currently a way to have a more restrictive benefit, although in the future when code ranges are supported, a tighter code range would be more restrictive.
				return false;
			}
			for(int b=0;b<listShort.Count;b++) {
				if(listShort[b].BenefitNum==benefit.BenefitNum) {
					continue;//skip self.
				}
				if(listShort[b].QuantityQualifier!=BenefitQuantity.None) {
					continue;//it must be some other kind of limitation other than an amount limit.  For example, BW frequency.
				}
				if(listShort[b].CodeNum != 0) {
					long codeNum=0;
					if(ProcedureCodes.GetContainsKey(claimProcHist.StrProcCode)) {
						codeNum=ProcedureCodes.GetOne(claimProcHist.StrProcCode).CodeNum;
					}
					if(listShort[b].CodeNum==codeNum) {//Enhance later for code ranges when supported by program
						return true;//a tighter limitation benefit exists for this specific procedure code.
					}
				}
				else if(listShort[b].CovCatNum!=0) {//specific category
					if(benefit.CovCatNum!=0) {//If benefit is a category limitation,
						//then we can only consider categories that are more restrictive (further down on list).
						//either of these could be -1 if isHidden
						int orderBenefit=CovCats.GetOrderShort(benefit.CovCatNum);
						int orderTest=CovCats.GetOrderShort(listShort[b].CovCatNum);
						if(orderBenefit==-1) {
							//nothing to do here.  Treat it like a general limitation 
						}
						else if(orderTest<orderBenefit) {//the CovCat of listShort is further up in the list and less restrictive, so should not be considered.
							//this handles orderTest=-1, skipping the hidden category
							continue;
						}
					}
					else {//But if this is a general limitation,
						//then we don't need to do the above check because any match can be considered more restrictive.
					}
					//see if the claimProcHist is in this more restrictive category
					CovSpan[] spansForCat=CovSpans.GetForCat(listShort[b].CovCatNum);//get the spans 
					//This must be a payment that was attached to a proc, so test the proc to be in the coderange of this annual max benefit
					for(int j=0;j<spansForCat.Length;j++) {
						if(String.Compare(claimProcHist.StrProcCode,spansForCat[j].FromCode)>=0 
							&& String.Compare(claimProcHist.StrProcCode,spansForCat[j].ToCode)<=0) 
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		///<summary>Only used from InsPlans.GetInsUsedDisplay.  If a procedure is handled by some limitation other than a general annual max, then we don't want it to count towards the annual max.</summary>
		public static bool LimitationExistsNotGeneral(List<Benefit> benList,long planNum,long patPlanNum,string strProcCode) {
			EbenefitCategory eben;
			CovSpan[] covSpanArray;
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<benList.Count;i++) {
				if(benList[i].PlanNum==0 && benList[i].PatPlanNum!=patPlanNum) {
					continue;
				}
				if(benList[i].PatPlanNum==0 && benList[i].PlanNum!=planNum) {
					continue;
				}
				if(benList[i].BenefitType!=InsBenefitType.Limitations) {
					continue;
				}
				if(benList[i].QuantityQualifier!=BenefitQuantity.None) {
					continue;
				}
				if(benList[i].TimePeriod!=BenefitTimePeriod.CalendarYear 
					&& benList[i].TimePeriod!=BenefitTimePeriod.ServiceYear
					&& benList[i].TimePeriod!=BenefitTimePeriod.Lifetime)//allows ortho max to be caught further down 
				{
					continue;
				}
				//coverage level?
				if(benList[i].CodeNum != 0) {
					if(benList[i].CodeNum==ProcedureCodes.GetCodeNum(strProcCode)) {//Enhance later for code ranges when supported by program
						return true;//a limitation benefit exists for this specific procedure code.
					}
				}
				if(benList[i].CovCatNum != 0) {
					eben=CovCats.GetEbenCat(benList[i].CovCatNum);
					if(eben==EbenefitCategory.General || eben==EbenefitCategory.None) {
						continue;//ignore this general benefit
					}
					covSpanArray=CovSpans.GetForCat(benList[i].CovCatNum);
					if(CovSpans.IsCodeInSpans(strProcCode,covSpanArray)) {
						return true;//a limitation benefit exists for a category that contains this procedure code.
					}
				}
			}
			return false;
		}

		///<summary>Used from ClaimProc.ComputeBaseEst and in sheet output. This is a low level function to get the percent to store in a claimproc.  It does not consider any percentOverride.  Always returns a number between 0 and 100.  Handles general, category, or procedure level.  Does not handle pat vs family coveragelevel.  Does handle patient override by using patplan.  Does not need to be aware of procedure history or loop history.</summary>
		public static int GetPercent(string procCodeStr,string planType,long planNum,long patPlanNum,List<Benefit> benList) {
			//No need to check RemotingRole; no call to db.
			if(planType=="f" || planType=="c"){
				return 100;//flat and cap are always covered 100%
			}
			//first, create a much shorter list with only relevant benefits in it.
			List<Benefit> listShort=new List<Benefit>();
			for(int i=0;i<benList.Count;i++) {
				if(benList[i].PlanNum==0 && benList[i].PatPlanNum!=patPlanNum) {
					continue;
				}
				if(benList[i].PatPlanNum==0 && benList[i].PlanNum!=planNum) {
					continue;
				}
				if(benList[i].BenefitType!=InsBenefitType.CoInsurance) {
					continue;
				}
				if(benList[i].Percent == -1) {
					continue;
				}
				listShort.Add(benList[i]);
			}
			//Find the best benefit matches.
			//Plan and Pat here indicate patplan override and have nothing to do with pat vs family coverage level.
			Benefit benPlan=null;
			Benefit benPat=null;
			//start with no category
			for(int i=0;i<listShort.Count;i++) {
				if(listShort[i].CodeNum > 0) {
					continue;
				}
				if(listShort[i].CovCatNum != 0) {
					continue;
				}
				if(listShort[i].PatPlanNum !=0) {
					benPat=listShort[i];
				}
				else {
					benPlan=listShort[i];
				}
			}
			//then, specific category.
			CovSpan[] spansForCat;
			for(int i=0;i<listShort.Count;i++) {
				if(listShort[i].CodeNum>0) {
					continue;
				}
				if(listShort[i].CovCatNum==0) {
					continue;
				}
				//see if the span matches
				spansForCat=CovSpans.GetForCat(listShort[i].CovCatNum);
				bool isMatch=false;
				for(int j=0;j<spansForCat.Length;j++) {
					if(String.Compare(procCodeStr,spansForCat[j].FromCode)>=0 && String.Compare(procCodeStr,spansForCat[j].ToCode)<=0) {
						isMatch=true;
						break;
					}
				}
				if(!isMatch) {
					continue;//no match
				}
				if(listShort[i].PatPlanNum !=0) {
					if(benPat != null && benPat.CovCatNum!=0) {//must compare
						//only use the new one if the item order is larger
						if(CovCats.GetOrderShort(listShort[i].CovCatNum) > CovCats.GetOrderShort(benPat.CovCatNum)) {
							benPat=listShort[i];
						}
					}
					else {//first one encountered for a category
						benPat=listShort[i];
					}
				}
				else {
					if(benPlan != null && benPlan.CovCatNum!=0) {//must compare
						//only use the new one if the item order is larger
						if(CovCats.GetOrderShort(listShort[i].CovCatNum) > CovCats.GetOrderShort(benPlan.CovCatNum)) {
							benPlan=listShort[i];
						}
					}
					else {//first one encountered for a category
						benPlan=listShort[i];
					}
				}
			}
			//then, specific code
			for(int i=0;i<listShort.Count;i++) {
				if(listShort[i].CodeNum==0) {
					continue;
				}
				if(procCodeStr!=ProcedureCodes.GetStringProcCode(listShort[i].CodeNum)) {
					continue;
				}
				if(benList[i].PatPlanNum !=0) {
					benPat=listShort[i];
				}
				else {
					benPlan=listShort[i];
				}				
			}
			if(benPat != null) {
				return benPat.Percent;
			}
			if(benPlan != null) {
				return benPlan.Percent;
			}
			return 0;
		}

		public static List<Benefit> GetForPatPlansAndProcs(List<long> listPatPlanNums,List<long> listCodeNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Benefit>>(MethodBase.GetCurrentMethod(),listPatPlanNums,listCodeNums);
			}
			if(listPatPlanNums.Count==0 || listCodeNums.Count==0) {
				return new List<Benefit>();
			}
			string command="SELECT * FROM benefit WHERE PatPlanNum IN("+string.Join(",",listPatPlanNums)+") AND CodeNum IN("+string.Join(",",listCodeNums)+")";
			return Crud.BenefitCrud.SelectMany(command);
		}

		///<summary>Only used from ClaimProc.ComputeBaseEst. This is a low level function to determine if a given procedure is completely excluded from coverage.  It does not consider any dates of service or history.</summary>
		public static bool IsExcluded(string strProcCode,List<Benefit> benList,long planNum,long patPlanNum) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<benList.Count;i++) {
				if(benList[i].PlanNum==0 && benList[i].PatPlanNum!=patPlanNum) {
					continue;
				}
				if(benList[i].PatPlanNum==0 && benList[i].PlanNum!=planNum) {
					continue;
				}
				if(benList[i].BenefitType!=InsBenefitType.Exclusions) {
					continue;
				}
				if(benList[i].CodeNum > 0) {
					if(strProcCode==ProcedureCodes.GetStringProcCode(benList[i].CodeNum)) {
						return true;//specific procedure code excluded
					}
					continue;
				}
				if(benList[i].CovCatNum==0) {
					continue;
					//General exclusion with no category.  This is considered an unsupported type of benefit.
					//Nobody should be able to exclude everything with one benefit entry.
				}
				//see if the span matches
				CovSpan[] spansForCat=CovSpans.GetForCat(benList[i].CovCatNum);
				//bool isMatch=false;
				for(int j=0;j<spansForCat.Length;j++) {
					if(String.Compare(strProcCode,spansForCat[j].FromCode)>=0 && String.Compare(strProcCode,spansForCat[j].ToCode)<=0) {
						return true;//span matches
					}
				}
			}
			return false;//no exclusions found for this code
		}

		///<summary>Used in FormInsPlan to sych database with changes user made to the benefit list for a plan.  
		///Must supply an old list for comparison.  Only the differences are saved.</summary>
		public static void UpdateList(List<Benefit> oldBenefitList,List<Benefit> newBenefitList){
			//No need to check RemotingRole; no call to db.
			Benefit newBenefit;
			for(int i=0;i<oldBenefitList.Count;i++){//loop through the old list
				newBenefit=null;
				for(int j=0;j<newBenefitList.Count;j++){
					if(newBenefitList[j]==null || newBenefitList[j].BenefitNum==0){
						continue;
					}
					if(oldBenefitList[i].BenefitNum==newBenefitList[j].BenefitNum){
						newBenefit=newBenefitList[j];
						break;
					}
				}
				if(newBenefit==null){
					//benefit with matching benefitNum was not found, so it must have been deleted
					Delete(oldBenefitList[i]);
					continue;
				}
				//benefit was found with matching benefitNum, so check for changes
				if(newBenefit.PlanNum != oldBenefitList[i].PlanNum
					|| newBenefit.PatPlanNum != oldBenefitList[i].PatPlanNum
					|| newBenefit.CovCatNum != oldBenefitList[i].CovCatNum
					|| newBenefit.BenefitType != oldBenefitList[i].BenefitType
					|| newBenefit.Percent != oldBenefitList[i].Percent
					|| newBenefit.MonetaryAmt != oldBenefitList[i].MonetaryAmt
					|| newBenefit.TimePeriod != oldBenefitList[i].TimePeriod
					|| newBenefit.QuantityQualifier != oldBenefitList[i].QuantityQualifier
					|| newBenefit.Quantity != oldBenefitList[i].Quantity
					|| newBenefit.CodeNum != oldBenefitList[i].CodeNum
					|| newBenefit.CoverageLevel != oldBenefitList[i].CoverageLevel)
				{
					Benefits.Update(newBenefit,oldBenefitList[i]); //logging taken care of in update
				}
			}
			for(int i=0;i<newBenefitList.Count;i++){//loop through the new list
				if(newBenefitList[i]==null){
					continue;	
				}
				if(newBenefitList[i].BenefitNum!=0){
					continue;
				}
				//benefit with benefitNum=0, so it's new
				Benefits.Insert(newBenefitList[i]); //logging taken care of in Insert method.
			}
		}

		///<summary>Used in family module display to get a list of benefits.  
		///The main purpose of this function is to group similar benefits for each plan on the same row, making it easier to display in a simple grid.  
		///Supply a list of all benefits for the patient, and the patPlans for the patient.</summary>
		public static Benefit[,] GetDisplayMatrix(List<Benefit> bensForPat,List<PatPlan> patPlanList,List<InsSub> subList){
			//No need to check RemotingRole; no call to db.
			Benefit[] row;//Closely related benefits, one entry for each pat plan. Entries can be null.
			//Dictionary's value is a list of arrays of benefits that are closely related. The key is a benefit representative of the ones in the value.
			Dictionary<Benefit,List<Benefit[]>> dictBens=new Dictionary<Benefit,List<Benefit[]>>();
			InsSub sub;
			for(int i=0;i<bensForPat.Count;i++) {
				for(int j=0;j<patPlanList.Count;j++) {//loop through columns
					sub=InsSubs.GetSub(patPlanList[j].InsSubNum,subList);
					if(patPlanList[j].PatPlanNum!=bensForPat[i].PatPlanNum
						&& sub.PlanNum!=bensForPat[i].PlanNum) 
					{
						continue;//Benefit doesn't apply to this column
					}
					List<Benefit[]>  listBenRows=null;
					dictBens.TryGetValue(bensForPat[i],out listBenRows);
					//if no matching type found, add a row
					if(listBenRows==null || listBenRows.Count==0) {
						row=new Benefit[patPlanList.Count];
						row[j]=bensForPat[i].Copy();
						dictBens.Add(bensForPat[i].Copy(),new List<Benefit[]> { row });
						continue;
					}
					//if the column for the matching row is null, then use that row
					if(listBenRows[0][j]==null) {
						listBenRows[0][j]=bensForPat[i].Copy();
						continue;
					}
					//if not null, then add another row.
					row=new Benefit[patPlanList.Count];
					row[j]=bensForPat[i].Copy();
					dictBens[bensForPat[i]].Add(row);
				}
			}
			ArrayList AL=new ArrayList();//each object is a Benefit[]
			foreach(Benefit[] arrBen in dictBens.SelectMany(x => x.Value)) {
				AL.Add(arrBen);
			}
			IComparer myComparer = new BenefitArraySorter();
			AL.Sort(myComparer);
			Benefit[,] retVal=new Benefit[patPlanList.Count,AL.Count];
			for(int y=0;y<AL.Count;y++){
				for(int x=0;x<patPlanList.Count;x++){
					if(((Benefit[])AL[y])[x]!=null){
						retVal[x,y]=((Benefit[])AL[y])[x].Copy();
					}
				}
			}
			return retVal;
		}

		///<summary>Deletes all benefits for a plan from the database.  Only used in FormInsPlan when picking a plan from the list.  
		///Need to clear out benefits so that they won't be picked up when choosing benefits for all.
		///No need to pass in usernum, it is set before the remoting role and passed in for logging.</summary>
		public static void DeleteForPlan(long planNum, long userNum=0) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				userNum=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),planNum,userNum);
				return;
			}
			string command="SELECT * FROM benefit WHERE PlanNum="+POut.Long(planNum);
			List<Benefit> listBenefits = Crud.BenefitCrud.SelectMany(command);
			command ="DELETE FROM benefit WHERE PlanNum="+POut.Long(planNum);
			Db.NonQ(command);
			listBenefits.ForEach(x => {
				InsEditLogs.MakeLogEntry(null,x,InsEditLogType.Benefit,userNum);
			});
		}

		///<summary>Get the string for displaying the frequency for the specified type for the specified plan (primary, secondary).</summary>
		public static string GetFrequencyDisplay(FrequencyType freqType,List<Benefit> benList, long planNum) {
			//No need to check RemotingRole; no call to db.
			string retVal="";
			bool bwFreqAdded=false;
			bool examFreqAdded=false;
			bool panoFMXFreqAdded=false;
			for(int i=0;i<benList.Count;i++){
				Benefit ben=benList[i];
				if(ben.PlanNum!=planNum) {
					continue;
				}
				switch(freqType) {
					//NOTE:  Any changes to detecting the benefits in this section of code needs to be reflected in Procedures.ComputeForOrdinal()--------------
					case FrequencyType.BW:
						if(!bwFreqAdded && ben.CodeNum==ProcedureCodes.GetCodeNum("D0274")
							&& ben.BenefitType==InsBenefitType.Limitations
							&& ben.MonetaryAmt==-1
							&& ben.PatPlanNum==0
							&& ben.Percent==-1
							&& (ben.QuantityQualifier==BenefitQuantity.Months 
								|| ben.QuantityQualifier==BenefitQuantity.Years
								|| ben.QuantityQualifier==BenefitQuantity.NumberOfServices))
						{
							if(ben.QuantityQualifier==BenefitQuantity.Months) {
								if(ben.Quantity==1) {
									retVal+=Lans.g("Benefits","Once every month.")+"\r\n";
								}
								else {
									retVal+=Lans.g("Benefits","Once every")+" "+ben.Quantity+" "+Lans.g("Benefits","months.")+"\r\n";
								}
							}
							else if(ben.QuantityQualifier==BenefitQuantity.Years) {
								if(ben.Quantity==1) {
									retVal+=Lans.g("Benefits","Once every year.")+"\r\n";
								}
								else {
									retVal+=Lans.g("Benefits","Once every")+" "+ben.Quantity+" "+Lans.g("Benefits","years.")+"\r\n";
								}
							}
							else {//number of services
								if(ben.Quantity==1) {
									retVal+=Lans.g("Benefits","Once per year.")+"\r\n";
								}
								else {
									retVal+=ben.Quantity+" "+Lans.g("Benefits","times per year.")+"\r\n";
								}
							}
							bwFreqAdded=true;
						}
						continue;
					case FrequencyType.PanoFMX:
						if(!panoFMXFreqAdded && ben.CodeNum==ProcedureCodes.GetCodeNum("D0330")
							&& ben.BenefitType==InsBenefitType.Limitations
							&& ben.MonetaryAmt==-1
							&& ben.PatPlanNum==0
							&& ben.Percent==-1
							&& (ben.QuantityQualifier==BenefitQuantity.Months 
								|| ben.QuantityQualifier==BenefitQuantity.Years
								|| ben.QuantityQualifier==BenefitQuantity.NumberOfServices))
						{
							if(ben.QuantityQualifier==BenefitQuantity.Months) {
								if(ben.Quantity==1) {
									retVal+=Lans.g("Benefits","Once every month.")+"\r\n";
								}
								else {
									retVal+=Lans.g("Benefits","Once every")+" "+ben.Quantity+" "+Lans.g("Benefits","months.")+"\r\n";
								}
							}
							else if(ben.QuantityQualifier==BenefitQuantity.Years) {
								if(ben.Quantity==1) {
									retVal+=Lans.g("Benefits","Once every year.")+"\r\n";
								}
								else {//number of services
									retVal+=Lans.g("Benefits","Once every")+" "+ben.Quantity+" "+Lans.g("Benefits","years.")+"\r\n";
								}
							}
							else {
								if(ben.Quantity==1) {
									retVal+=Lans.g("Benefits","Once per year.")+"\r\n";
								}
								else {
									retVal+=ben.Quantity+" "+Lans.g("Benefits","times per year.")+"\r\n";
								}
							}
							panoFMXFreqAdded=true;
						}
						continue;
					case FrequencyType.Exam:
						if(!examFreqAdded 
							&& CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive)!=null
							&& ben.CodeNum==0
							&& ben.BenefitType==InsBenefitType.Limitations
							&& ben.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive).CovCatNum
							&& ben.MonetaryAmt==-1
							&& ben.PatPlanNum==0
							&& ben.Percent==-1
							&& (ben.QuantityQualifier==BenefitQuantity.Months
									|| ben.QuantityQualifier==BenefitQuantity.Years
									|| ben.QuantityQualifier==BenefitQuantity.NumberOfServices))
						{
							if(ben.QuantityQualifier==BenefitQuantity.Months) {
								if(ben.Quantity==1) {
									retVal+=Lans.g("Benefits","Once every month.")+"\r\n";
								}
								else {
									retVal+=Lans.g("Benefits","Once every")+" "+ben.Quantity+" "+Lans.g("Benefits","months.")+"\r\n";
								}
							}
							else if(ben.QuantityQualifier==BenefitQuantity.Years) {
								if(ben.Quantity==1) {
									retVal+=Lans.g("Benefits","Once every year.")+"\r\n";
								}
								else {
									retVal+=Lans.g("Benefits","Once every")+" "+ben.Quantity+" "+Lans.g("Benefits","years.")+"\r\n";
								}
							}
							else {//number of services
								if(ben.Quantity==1) {
									retVal+=Lans.g("Benefits","Once per year.")+"\r\n";
								}
								else {
									retVal+=ben.Quantity+" "+Lans.g("Benefits","times per year.")+"\r\n";
								}
							}
							examFreqAdded=true;
						}
						continue;
				}//end switch
			}//end i loop
			return retVal;
		}

		///<summary>Gets the string that displays in the "Category" column of the benefits table in FormInsPlans. 
		///Pass in a list of CovCats to not make multiple deep copies of the cache.</summary>
		public static string GetCategoryString(Benefit benefit) {
			//No need to check RemotingRole; no call to db.
			string retVal = "";
			if(benefit.CodeNum==0) {
				//EXAMS-------------------------------------------------------------------------------------------------------------------------------------
				if(benefit.BenefitType==InsBenefitType.Limitations
					&& CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive)!=null
					&& benefit.CovCatNum==CovCats.GetForEbenCat(EbenefitCategory.RoutinePreventive).CovCatNum
					&& benefit.MonetaryAmt==-1
					&& benefit.PatPlanNum==0
					&& benefit.Percent==-1
					&& (benefit.QuantityQualifier==BenefitQuantity.Months
						|| benefit.QuantityQualifier==BenefitQuantity.Years
						|| benefit.QuantityQualifier==BenefitQuantity.NumberOfServices)) {
					retVal=Lans.g("benefitCategory","Exams");
				}
				else {//Non-exam limitation benefit
					retVal = CovCats.GetDesc(benefit.CovCatNum);
				}
			}
			else {
				ProcedureCode proccode = ProcedureCodes.GetProcCode(benefit.CodeNum);
				//BW----------------------------------------------------------------------------------------------------------------------------------------
				if(proccode.ProcCode=="D0274"//4BW
					&& benefit.BenefitType==InsBenefitType.Limitations
					&& benefit.MonetaryAmt==-1
					&& benefit.PatPlanNum==0
					&& benefit.Percent==-1
					&& (benefit.QuantityQualifier==BenefitQuantity.Months
						|| benefit.QuantityQualifier==BenefitQuantity.Years
						|| benefit.QuantityQualifier==BenefitQuantity.NumberOfServices)) {
					retVal = Lans.g("benefitCategory","Bitewings");
				}
				//PANO/FMX----------------------------------------------------------------------------------------------------------------------------------
				else if(proccode.ProcCode=="D0330"//Pano
					&& benefit.BenefitType==InsBenefitType.Limitations
					&& benefit.MonetaryAmt==-1
					&& benefit.PatPlanNum==0
					&& benefit.Percent==-1
					&& (benefit.QuantityQualifier==BenefitQuantity.Months
						|| benefit.QuantityQualifier==BenefitQuantity.Years
						|| benefit.QuantityQualifier==BenefitQuantity.NumberOfServices)) {
					retVal = Lans.g("benefitCategory","Pano/FMX");
				}
				//DEFAULT-----------------------------------------------------------------------------------------------------------------------------------
				else {//Everything else
					retVal = proccode.ProcCode+"-"+proccode.AbbrDesc;
				}
			}
			return retVal;
		}
	}

	public enum FrequencyType {
		BW,
		PanoFMX,
		Exam
	}


		



		
	

	

	


}










