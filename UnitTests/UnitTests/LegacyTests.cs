using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CodeBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using UnitTestsCore;

namespace UnitTests.UnitTests {
	[TestClass]
	public class LegacyTests:TestBase {

		[TestMethod]
		public void Legacy_TestOneTwo() {
			//if(specificTest != 0 && specificTest != 1 && specificTest != 2){
			//	return"";
			//}
			string suffix="1";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			long feeSchedNum1=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix);
			long feeSchedNum2=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix+"b");
			//Standard Fee
			FeeT.RefreshCache();
			long codeNum=ProcedureCodes.GetCodeNum("D2750");
			Fee fee=Fees.GetFee(codeNum,53,0,0);
			if(fee==null) {
				fee=new Fee();
				fee.CodeNum=codeNum;
				fee.FeeSched=53;
				fee.Amount=1200;
				Fees.Insert(fee);
			}
			else {
				fee.Amount=1200;
				Fees.Update(fee);
			}
			//PPO fees
			fee=new Fee();
			fee.CodeNum=codeNum;
			fee.FeeSched=feeSchedNum1;
			fee.Amount=900;
			Fees.Insert(fee);
			fee=new Fee();
			fee.CodeNum=codeNum;
			fee.FeeSched=feeSchedNum2;
			fee.Amount=650;
			Fees.Insert(fee);
			FeeT.RefreshCache();
			//Carrier
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			long planNum1=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedNum1).PlanNum;
			long planNum2=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedNum2).PlanNum;
			InsSub sub1=InsSubT.CreateInsSub(pat.PatNum,planNum1);
			long subNum1=sub1.InsSubNum;
			InsSub sub2=InsSubT.CreateInsSub(pat.PatNum,planNum2);
			long subNum2=sub2.InsSubNum;
			BenefitT.CreateCategoryPercent(planNum1,EbenefitCategory.Crowns,50);
			BenefitT.CreateCategoryPercent(planNum2,EbenefitCategory.Crowns,50);
			PatPlanT.CreatePatPlan(1,patNum,subNum1);
			PatPlanT.CreatePatPlan(2,patNum,subNum2);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D2750",ProcStat.TP,"8",Fees.GetAmount0(codeNum,53));//crown on 8
			long procNum=proc.ProcNum;
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(patNum);
			Family fam=Patients.GetFamily(patNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(patNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			//Validate
			string retVal="";
			ClaimProc claimProc;
			//if(specificTest==0 || specificTest==1){
			Procedures.ComputeEstimates(proc,patNum,ref claimProcs,false,planList,patPlans,benefitList,histList,loopList,true,pat.Age,subList);
			claimProcs=ClaimProcs.Refresh(patNum);
			claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum1,subNum1);
			//I don't think allowed can be easily tested on the fly, and it's not that important.
			//if(claimProc.InsEstTotal!=450) {
			//	throw new Exception("Should be 450. \r\n");
			//}
			Assert.AreEqual(450,claimProc.InsEstTotal);
			//if(claimProc.WriteOffEst!=300) {
			//	throw new Exception("Should be 300. \r\n");
			//}
			Assert.AreEqual(300,claimProc.WriteOffEst);
			claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum2,subNum2);
			//if(claimProc.InsEstTotal!=200) {
			//	throw new Exception("Should be 200. \r\n");
			//}
			Assert.AreEqual(200,claimProc.InsEstTotal);
			//if(claimProc.WriteOffEst!=0) {
			//	throw new Exception("Should be 0. \r\n");
			//}
			Assert.AreEqual(0,claimProc.WriteOffEst);
			//retVal+="1: Passed.  Claim proc estimates for dual PPO ins.  Allowed1 greater than Allowed2.\r\n";
			//}
			//Test 2----------------------------------------------------------------------------------------------------
			//if(specificTest==0 || specificTest==2){
			//s	witch the fees
			fee=Fees.GetFee(codeNum,feeSchedNum1,0,0);
			fee.Amount=650;
			Fees.Update(fee);
			fee=Fees.GetFee(codeNum,feeSchedNum2,0,0);
			fee.Amount=900;
			Fees.Update(fee);
			FeeT.RefreshCache();
			Procedures.ComputeEstimates(proc,patNum,ref claimProcs,false,planList,patPlans,benefitList,histList,loopList,true,pat.Age,subList);
			//Validate
			claimProcs=ClaimProcs.Refresh(patNum);
			claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum1,subNum1);
			//if(claimProc.InsEstTotal!=325) {
			//	throw new Exception("Should be 325. \r\n");
			//}
			Assert.AreEqual(325,claimProc.InsEstTotal);
			//if(claimProc.WriteOffEst!=425) {
			//	throw new Exception("Should be 425.\r\n");
			//}
			Assert.AreEqual(425,claimProc.WriteOffEst);
			claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum2,subNum2);
			//if(claimProc.InsEstTotal!=450) {
			//	throw new Exception("Should be 450.\r\n");
			//}
			Assert.AreEqual(450,claimProc.InsEstTotal);
			//if(claimProc.WriteOffEst!=0) {
			//	throw new Exception("Should be 0. \r\n");
			//}
			Assert.AreEqual(0,claimProc.WriteOffEst);
			retVal+="2: Passed.  Basic COB with PPOs.  Allowed2 greater than Allowed1.\r\n";
			//}
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestThree() {
			//if(specificTest != 0 && specificTest !=3){
			//	return"";
			//}
			string suffix="3";
			Patient pat=PatientT.CreatePatient(suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);//guarantor is subscriber
			BenefitT.CreateAnnualMax(plan.PlanNum,1000);	
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Crowns,100);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Diagnostic,100);
			BenefitT.CreateFrequencyProc(plan.PlanNum,"D0274",BenefitQuantity.Years,1);//BW frequency every 1 year
			PatPlanT.CreatePatPlan(1,pat.PatNum,sub.InsSubNum);
			//proc1 - Crown
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D2750",ProcStat.TP,"8",1100);
			ProcedureT.SetPriority(proc1,0);
			//proc2 - 4BW
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0274",ProcStat.TP,"8",50);
			ProcedureT.SetPriority(proc2,1);
			//Lists:
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<Procedure>	ProcList=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP=Procedures.GetListTPandTPi(ProcList);//sorted by priority, then toothnum
			//Validate
			string retVal="";
			for(int i=0;i<ProcListTP.Length;i++){
				Procedures.ComputeEstimates(ProcListTP[i],pat.PatNum,ref claimProcs,false,planList,patPlans,benefitList,
					histList,loopList,false,pat.Age,subList);
				//then, add this information to loopList so that the next procedure is aware of it.
				loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs,ProcListTP[i].ProcNum,ProcListTP[i].CodeNum));
			}
			//save changes in the list to the database
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			ClaimProc claimProc=ClaimProcs.GetEstimate(claimProcs,proc2.ProcNum,plan.PlanNum,sub.InsSubNum);
			//I don't think allowed can be easily tested on the fly, and it's not that important.
			//if(claimProc.InsEstTotal!=0) {//Insurance should not cover because over annual max.
			//	throw new Exception("Should be 0. \r\n");
			//}
			Assert.AreEqual(0,claimProc.InsEstTotal);
			retVal+="3: Passed.  Insurance show zero coverage over annual max.  Not affected by a frequency.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestFour() {
			//if(specificTest != 0 && specificTest !=4){
			//	return"";
			//}
			string suffix="4";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Patient pat2=PatientT.CreatePatient(suffix);
			PatientT.SetGuarantor(pat2,pat.PatNum);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			long planNum=plan.PlanNum;
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,planNum);//guarantor is subscriber
			long subNum=sub.InsSubNum;
			PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			PatPlanT.CreatePatPlan(1,pat2.PatNum,subNum);//both patients have the same plan
			BenefitT.CreateAnnualMax(planNum,1000);	
			BenefitT.CreateAnnualMaxFamily(planNum,2500);	
			BenefitT.CreateCategoryPercent(planNum,EbenefitCategory.Crowns,100);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D2750",ProcStat.TP,"8",830);
			long procNum=proc.ProcNum;
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(patNum);
			Family fam=Patients.GetFamily(patNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(patNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			//Validate
			Procedures.ComputeEstimates(proc,patNum,ref claimProcs,false,planList,patPlans,benefitList,histList,loopList,true,pat.Age,subList);
			claimProcs=ClaimProcs.Refresh(patNum);
			ClaimProc claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum,subNum);
			//if(claimProc.InsEstTotal!=830) {
			//	throw new Exception("Should be 830. \r\n");
			//}
			Assert.AreEqual(830,claimProc.InsEstTotal);
			//if(claimProc.EstimateNote!="") {
			//	throw new Exception("EstimateNote should be blank.");
			//}
			Assert.AreEqual("",claimProc.EstimateNote);
			//return "4: Passed.  When family benefits, does not show 'over annual max' until max reached.\r\n";
		}

		[TestMethod]
		public void Legacy_TestFive() {
			//if(specificTest != 0 && specificTest !=5){
			//	return"";
			//}
			string suffix="5";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Patient pat2=PatientT.CreatePatient(suffix);
			PatientT.SetGuarantor(pat2,pat.PatNum);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			long planNum=plan.PlanNum;
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,planNum);//guarantor is subscriber
			long subNum=sub.InsSubNum;
			PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			PatPlanT.CreatePatPlan(1,pat2.PatNum,subNum);//both patients have the same plan
			BenefitT.CreateAnnualMax(planNum,1000);	
			BenefitT.CreateAnnualMaxFamily(planNum,2500);	
			BenefitT.CreateCategoryPercent(planNum,EbenefitCategory.Crowns,100);
			ClaimProcT.AddInsUsedAdjustment(pat2.PatNum,planNum,2000,subNum,0);//Adjustment goes on the second patient
			Procedure proc=ProcedureT.CreateProcedure(pat2,"D2750",ProcStat.TP,"8",830);//crown and testing is for the first patient
			long procNum=proc.ProcNum;
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(patNum);
			Family fam=Patients.GetFamily(patNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(patNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=ClaimProcs.GetHistList(patNum,benefitList,patPlans,planList,DateTime.Today,subList);
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			//Validate
			Procedures.ComputeEstimates(proc,patNum,ref claimProcs,false,planList,patPlans,benefitList,histList,loopList,true,pat.Age,subList);
			claimProcs=ClaimProcs.Refresh(patNum);
			ClaimProc claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum,subNum);
			//if(claimProc.InsEstTotal!=500) {
			//	throw new Exception("Should be 500. \r\n");
			//}
			Assert.AreEqual(500,claimProc.InsEstTotal);
			//if(claimProc.EstimateNote!="Over family annual max") {//this explains estimate was reduced.
			//	throw new Exception("EstimateNote not matching expected.");
			//}
			Assert.AreEqual("Over family annual max",claimProc.EstimateNote);
			//return "5: Passed.  Both individual and family max taken into account.\r\n";
		}

		[TestMethod]
		public void Legacy_TestSix() {
			//if(specificTest != 0 && specificTest !=6){
			//	return"";
			//}
			string suffix="6";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			long planNum=plan.PlanNum;
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,planNum);//guarantor is subscriber
			long subNum=sub.InsSubNum;
			long patPlanNum=PatPlanT.CreatePatPlan(1,pat.PatNum,subNum).PatPlanNum;
			BenefitT.CreateAnnualMax(planNum,1000);	
			BenefitT.CreateLimitation(planNum,EbenefitCategory.Diagnostic,1000);	
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",50);//An exam
			long procNum=proc.ProcNum;
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D2750",ProcStat.C,"8",830);//create a crown
			ClaimProcT.AddInsPaid(patNum,planNum,procNum,50,subNum,0,0);
			ClaimProcT.AddInsPaid(patNum,planNum,proc2.ProcNum,400,subNum,0,0);
			//Lists
			Family fam=Patients.GetFamily(patNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(patNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=ClaimProcs.GetHistList(patNum,benefitList,patPlans,planList,DateTime.Today,subList);
			//Validate
			double insUsed=InsPlans.GetInsUsedDisplay(histList,DateTime.Today,planNum,patPlanNum,-1,planList,benefitList,patNum,subNum);
			//if(insUsed!=400){
			//	throw new Exception("Should be 400. \r\n");
			//}
			Assert.AreEqual(400,insUsed);
			//Patient has one insurance plan, subscriber self. Benefits: annual max 1000, diagnostic max 1000. One completed procedure, an exam for $50. Sent to insurance and insurance paid $50. Ins used should still show 0 because the ins used value should only be concerned with annual max . 
			//return "6: Passed.  Limitations override more general limitations.\r\n";
		}

		[TestMethod]
		public void Legacy_TestSeven() {
			//if(specificTest != 0 && specificTest !=7){
			//	return"";
			//}
			string suffix="7";
			Patient pat=PatientT.CreatePatient(suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			long subNum=sub.InsSubNum;
			BenefitT.CreateAnnualMax(plan.PlanNum,1000);	
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.RoutinePreventive,100);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Diagnostic,100);
			BenefitT.CreateDeductibleGeneral(plan.PlanNum,BenefitCoverageLevel.Individual,50);
			BenefitT.CreateDeductible(plan.PlanNum,EbenefitCategory.RoutinePreventive,25);
			BenefitT.CreateDeductible(plan.PlanNum,EbenefitCategory.Diagnostic,25);
			PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			//proc1 - PerExam
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.TP,"",60);
			ProcedureT.SetPriority(proc1,0);
			//proc2 - Prophy
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.TP,"",70);
			ProcedureT.SetPriority(proc2,1);
			//Lists:
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<Procedure>	ProcList=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP=Procedures.GetListTPandTPi(ProcList);//sorted by priority, then toothnum
			//Validate
			string retVal="";
			for(int i=0;i<ProcListTP.Length;i++){
				Procedures.ComputeEstimates(ProcListTP[i],pat.PatNum,ref claimProcs,false,planList,patPlans,benefitList,
					histList,loopList,false,pat.Age,subList);
				//then, add this information to loopList so that the next procedure is aware of it.
				loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs,ProcListTP[i].ProcNum,ProcListTP[i].CodeNum));
			}
			//save changes in the list to the database
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			ClaimProc claimProc=ClaimProcs.GetEstimate(claimProcs,proc2.ProcNum,plan.PlanNum,subNum);
			//if(claimProc.DedEst!=0) {//Second procedure should show no deductible.
			//	throw new Exception("Should be 0. \r\n");
			//}
			Assert.AreEqual(0,claimProc.DedEst);
			retVal+="7: Passed.  A deductible for preventive/diagnostic is only included once.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestEight() {
			//if(specificTest != 0 && specificTest !=8){
			//	return"";
			//}
			string suffix="8";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			long feeSchedNum1=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix);
			long feeSchedNum2=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix+"b");
			//Standard Fee
			FeeT.RefreshCache();
			long codeNum=ProcedureCodes.GetCodeNum("D2750");
			Fee fee=Fees.GetFee(codeNum,53,0,0);
			if(fee==null) {
				fee=new Fee();
				fee.CodeNum=codeNum;
				fee.FeeSched=53;
				fee.Amount=1200;
				Fees.Insert(fee);
			}
			else {
				fee.Amount=1200;
				Fees.Update(fee);
			}
			//PPO fees
			fee=new Fee();
			fee.CodeNum=codeNum;
			fee.FeeSched=feeSchedNum1;
			fee.Amount=600;
			Fees.Insert(fee);
			fee=new Fee();
			fee.CodeNum=codeNum;
			fee.FeeSched=feeSchedNum2;
			fee.Amount=800;
			Fees.Insert(fee);
			FeeT.RefreshCache();
			//Carrier
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			long planNum1=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedNum1).PlanNum;
			long planNum2=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedNum2).PlanNum;
			InsSub sub1=InsSubT.CreateInsSub(pat.PatNum,planNum1);
			long subNum1=sub1.InsSubNum;
			InsSub sub2=InsSubT.CreateInsSub(pat.PatNum,planNum2);
			long subNum2=sub2.InsSubNum;
			BenefitT.CreateCategoryPercent(planNum1,EbenefitCategory.Crowns,50);
			BenefitT.CreateCategoryPercent(planNum2,EbenefitCategory.Crowns,50);
			BenefitT.CreateAnnualMax(planNum1,1000);
			BenefitT.CreateAnnualMax(planNum2,1000);
			PatPlanT.CreatePatPlan(1,patNum,subNum1);
			PatPlanT.CreatePatPlan(2,patNum,subNum2);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D2750",ProcStat.TP,"8",Fees.GetAmount0(codeNum,53));//crown on 8
			long procNum=proc.ProcNum;
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(patNum);
			Family fam=Patients.GetFamily(patNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(patNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<Procedure> procList=Procedures.Refresh(patNum);
			//Set complete and attach to claim
			ProcedureT.SetComplete(proc,pat,planList,patPlans,claimProcs,benefitList,subList);
			claimProcs=ClaimProcs.Refresh(patNum);
			List<Procedure> procsForClaim=new List<Procedure>();
			procsForClaim.Add(proc);
			Claim claim=ClaimT.CreateClaim("P",patPlans,planList,claimProcs,procList,pat,procsForClaim,benefitList,subList);
			//Validate
			string retVal="";
			//if(claim.WriteOff!=500) {
			//	throw new Exception("Should be 500. \r\n");
			//}
			Assert.AreEqual(500,claim.WriteOff);
			retVal+="8: Passed.  Completed writeoffs same as estimates for dual PPO ins when Allowed2 greater than Allowed1.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestNine() {
			//if(specificTest != 0 && specificTest !=9) {
			//	return "";
			//}
			string suffix="9";
			Patient pat=PatientT.CreatePatient(suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			long subNum=sub.InsSubNum;
			BenefitT.CreateAnnualMax(plan.PlanNum,200);
			BenefitT.CreateLimitationProc(plan.PlanNum,"D2161",2000);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Restorative,80);
			PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			//proc1 - D2161 (4-surf amalgam)
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D2161",ProcStat.TP,"3",300);
			ProcedureT.SetPriority(proc1,0);
			//proc2 - D2160 (3-surf amalgam)
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D2160",ProcStat.TP,"4",300);
			ProcedureT.SetPriority(proc2,1);
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<Procedure> ProcList=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP=Procedures.GetListTPandTPi(ProcList);//sorted by priority, then toothnum
			//Validate
			string retVal="";
			for(int i=0;i<ProcListTP.Length;i++) {
				Procedures.ComputeEstimates(ProcListTP[i],pat.PatNum,ref claimProcs,false,planList,patPlans,benefitList,
					histList,loopList,false,pat.Age,subList);
				//then, add this information to loopList so that the next procedure is aware of it.
				loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs,ProcListTP[i].ProcNum,ProcListTP[i].CodeNum));
			}
			//save changes in the list to the database
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			ClaimProc claimProc=ClaimProcs.GetEstimate(claimProcs,proc2.ProcNum,plan.PlanNum,subNum);
			//if(claimProc.InsEstTotal!=200) {//Insurance should cover.
			//	throw new Exception("Should be 200. \r\n");
			//}
			Assert.AreEqual(200,claimProc.InsEstTotal);
			retVal+="9: Passed.  Limitations should override more general limitations for any benefit.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestTen() {
			//if(specificTest != 0 && specificTest !=10) {
			//	return "";
			//}
			string suffix="10";
			Patient pat=PatientT.CreatePatient(suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			long subNum=sub.InsSubNum;
			BenefitT.CreateAnnualMax(plan.PlanNum,400);
			BenefitT.CreateFrequencyCategory(plan.PlanNum,EbenefitCategory.RoutinePreventive,BenefitQuantity.Years,2);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.RoutinePreventive,100);
			PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			//procs - D1515 (space maintainers)
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D1515",ProcStat.TP,"3",500);
			ProcedureT.SetPriority(proc1,0);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D1515",ProcStat.TP,"3",500);
			ProcedureT.SetPriority(proc2,1);
			//Procedure proc3=ProcedureT.CreateProcedure(pat,"D1515",ProcStat.TP,"3",500);
			//ProcedureT.SetPriority(proc3,2);
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<Procedure> ProcList=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP=Procedures.GetListTPandTPi(ProcList);//sorted by priority, then toothnum
			//Validate
			string retVal="";
			for(int i=0;i<ProcListTP.Length;i++) {
				Procedures.ComputeEstimates(ProcListTP[i],pat.PatNum,ref claimProcs,false,planList,patPlans,benefitList,
					histList,loopList,false,pat.Age,subList);
				//then, add this information to loopList so that the next procedure is aware of it.
				loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs,ProcListTP[i].ProcNum,ProcListTP[i].CodeNum));
			}
			//save changes in the list to the database
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			ClaimProc claimProc1=ClaimProcs.GetEstimate(claimProcs,proc1.ProcNum,plan.PlanNum,subNum);
			//if(claimProc1.InsEstTotal!=400) {//Insurance should partially cover.
			//	throw new Exception("Should be 400. \r\n");
			//}
			Assert.AreEqual(400,claimProc1.InsEstTotal);
			ClaimProc claimProc2=ClaimProcs.GetEstimate(claimProcs,proc2.ProcNum,plan.PlanNum,subNum);
			//if(claimProc2.InsEstTotal!=0) {//Insurance should not cover.
			//	throw new Exception("Should be 0. \r\n");
			//}
			Assert.AreEqual(0,claimProc2.InsEstTotal);
			retVal+="10: Passed.  Once max is reached, additional procs show 0 coverage even if preventive frequency exists.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestEleven() {
			//if(specificTest != 0 && specificTest !=11) {
			//	return "";
			//}
			string suffix="11";
			Patient pat=PatientT.CreatePatient(suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			long subNum=sub.InsSubNum;
			BenefitT.CreateAnnualMaxFamily(plan.PlanNum,400);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Restorative,100);
			PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			//procs - D2140 (amalgum fillings)
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D2140",ProcStat.TP,"18",500);
			//Procedure proc1=ProcedureT.CreateProcedure(pat,"D1515",ProcStat.TP,"3",500);
			ProcedureT.SetPriority(proc1,0);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D2140",ProcStat.TP,"19",500);
			//Procedure proc2=ProcedureT.CreateProcedure(pat,"D1515",ProcStat.TP,"3",500);
			ProcedureT.SetPriority(proc2,1);
			//Procedure proc3=ProcedureT.CreateProcedure(pat,"D1515",ProcStat.TP,"3",500);
			//ProcedureT.SetPriority(proc3,2);
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<Procedure> ProcList=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP=Procedures.GetListTPandTPi(ProcList);//sorted by priority, then toothnum
			//Validate
			string retVal="";
			for(int i=0;i<ProcListTP.Length;i++) {
				Procedures.ComputeEstimates(ProcListTP[i],pat.PatNum,ref claimProcs,false,planList,patPlans,benefitList,
					histList,loopList,false,pat.Age,subList);
				//then, add this information to loopList so that the next procedure is aware of it.
				loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs,ProcListTP[i].ProcNum,ProcListTP[i].CodeNum));
			}
			//save changes in the list to the database
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			ClaimProc claimProc1=ClaimProcs.GetEstimate(claimProcs,proc1.ProcNum,plan.PlanNum,patPlans[0].InsSubNum);
			//if(claimProc1.InsEstTotal!=400) {//Insurance should partially cover.
			//	throw new Exception("Claim 1 was "+claimProc1.InsEstTotal+", should be 400.\r\n");
			//}
			Assert.AreEqual(400,claimProc1.InsEstTotal);
			ClaimProc claimProc2=ClaimProcs.GetEstimate(claimProcs,proc2.ProcNum,plan.PlanNum,patPlans[0].InsSubNum);
			//if(claimProc2.InsEstTotal!=0) {//Insurance should not cover.
			//	throw new Exception("Claim 2 was "+claimProc2.InsEstTotal+", should be 0.\r\n");
			//}
			Assert.AreEqual(0,claimProc2.InsEstTotal);
			retVal+="11: Passed.  Once family max is reached, additional procs show 0 coverage.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestTwelve() {
			//if(specificTest != 0 && specificTest !=12){
			//	return"";
			//}
			string suffix="12";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Patient pat2=PatientT.CreatePatient(suffix);
			PatientT.SetGuarantor(pat2,pat.PatNum);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			long feeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix);
			//Standard Fee
			FeeT.RefreshCache();
			long codeNum=ProcedureCodes.GetCodeNum("D2750");
			Fee fee=Fees.GetFee(codeNum,53,0,0);
			if(fee==null) {
				fee=new Fee();
				fee.CodeNum=codeNum;
				fee.FeeSched=53;
				fee.Amount=1400;
				Fees.Insert(fee);
			}
			else {
				fee.Amount=1400;
				Fees.Update(fee);
			}
			//PPO fees
			fee=new Fee();
			fee.CodeNum=codeNum;
			fee.FeeSched=feeSchedNum;
			fee.Amount=1100;
			Fees.Insert(fee);
			FeeT.RefreshCache();
			InsPlan plan=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedNum);
			long planNum=plan.PlanNum;
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,planNum);//patient is subscriber for plan 1
			long subNum=sub.InsSubNum;
			PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			InsSub sub2=InsSubT.CreateInsSub(pat2.PatNum,planNum);//spouse is subscriber for plan 2
			long subNum2=sub2.InsSubNum;
			PatPlanT.CreatePatPlan(2,pat.PatNum,subNum2);//patient also has spouse's coverage
			BenefitT.CreateAnnualMax(planNum,1200);
			BenefitT.CreateDeductibleGeneral(planNum,BenefitCoverageLevel.Individual,0);
			BenefitT.CreateCategoryPercent(planNum,EbenefitCategory.Crowns,100);//2700-2799
			Procedure proc=ProcedureT.CreateProcedure(pat,"D2750",ProcStat.TP,"19",1400);
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();//empty, not used for calcs.
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();//empty, not used for calcs.
			List<Procedure> ProcList=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP=Procedures.GetListTPandTPi(ProcList);//sorted by priority, then toothnum
			//Validate
			string retVal="";
			Procedures.ComputeEstimates(ProcListTP[0],pat.PatNum,ref claimProcs,false,planList,patPlans,benefitList,
				histList,loopList,false,pat.Age,subList);
			//save changes in the list to the database
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			ClaimProc claimProc1=ClaimProcs.GetEstimate(claimProcs,proc.ProcNum,plan.PlanNum,subNum);
			//if(claimProc1.InsEstTotal!=1100) {
			//	throw new Exception("Primary Estimate was "+claimProc1.InsEstTotal+", should be 1100.\r\n");
			//}
			Assert.AreEqual(1100,claimProc1.InsEstTotal);
			ClaimProc claimProc2=ClaimProcs.GetEstimate(claimProcs,proc.ProcNum,plan.PlanNum,subNum2);
			//if(claimProc2.InsEstTotal!=0) {//Insurance should not cover.
			//	throw new Exception("Secondary Estimate was "+claimProc2.InsEstTotal+", should be 0.\r\n");
			//}
			Assert.AreEqual(0,claimProc2.InsEstTotal);
			retVal+="12: Passed.  Once family max is reached, additional procs show 0 coverage.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestThirteen() {
			//if(specificTest != 0 && specificTest !=13){
			//	return"";
			//}
			string suffix="13";
			Patient pat=PatientT.CreatePatient(suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			long subNum=sub.InsSubNum;
			PatPlan patPlan=PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			BenefitT.CreateAnnualMax(plan.PlanNum,100);
			BenefitT.CreateOrthoMax(plan.PlanNum,500);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Diagnostic,100);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Orthodontics,100);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0140",ProcStat.C,"",59);//limEx
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D8090",ProcStat.C,"",348);//Comprehensive ortho
			ClaimProcT.AddInsPaid(pat.PatNum,plan.PlanNum,proc1.ProcNum,59,subNum,0,0);
			ClaimProcT.AddInsPaid(pat.PatNum,plan.PlanNum,proc2.ProcNum,348,subNum,0,0);
			//Lists
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=ClaimProcs.GetHistList(pat.PatNum,benefitList,patPlans,planList,DateTime.Today,subList);
			//Validate
			double insUsed=InsPlans.GetInsUsedDisplay(histList,DateTime.Today,plan.PlanNum,patPlan.PatPlanNum,-1,planList,benefitList,pat.PatNum,subNum);
			//if(insUsed!=59) {
			//	throw new Exception("Should be 59. \r\n");
			//}
			Assert.AreEqual(59,insUsed);
			//return "13: Passed.  Ortho procedures should not affect insurance used section at lower right of TP module.\r\n";
		}

		[TestMethod]
		public void Legacy_TestFourteen() {
			//if(specificTest != 0 && specificTest !=14) {
			//	return "";
			//}
			string suffix="14";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			long feeSchedNum1=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix);
			long feeSchedNum2=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix+"b");
			//Standard Fee
			FeeT.RefreshCache();
			long codeNum=ProcedureCodes.GetCodeNum("D2160");
			Fee fee=Fees.GetFee(codeNum,53,0,0);
			if(fee==null) {
				fee=new Fee();
				fee.CodeNum=codeNum;
				fee.FeeSched=53;
				fee.Amount=1279;
				Fees.Insert(fee);
			}
			else {
				fee.Amount=1279;
				Fees.Update(fee);
			}
			//PPO fees
			fee=new Fee();
			fee.CodeNum=codeNum;
			fee.FeeSched=feeSchedNum1;
			fee.Amount=1279;
			Fees.Insert(fee);
			fee=new Fee();
			fee.CodeNum=codeNum;
			fee.FeeSched=feeSchedNum2;
			fee.Amount=110;
			Fees.Insert(fee);
			FeeT.RefreshCache();
			//Carrier
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			long planNum1=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedNum1).PlanNum;
			long planNum2=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedNum2).PlanNum;
			InsSub sub1=InsSubT.CreateInsSub(pat.PatNum,planNum1);
			long subNum1=sub1.InsSubNum;
			InsSub sub2=InsSubT.CreateInsSub(pat.PatNum,planNum2);
			long subNum2=sub2.InsSubNum;
			BenefitT.CreateCategoryPercent(planNum1,EbenefitCategory.Restorative,80);
			BenefitT.CreateCategoryPercent(planNum2,EbenefitCategory.Restorative,80);
			BenefitT.CreateAnnualMax(planNum1,1200);
			BenefitT.CreateAnnualMax(planNum2,1200);
			PatPlanT.CreatePatPlan(1,patNum,subNum1);
			PatPlanT.CreatePatPlan(2,patNum,subNum2);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D2160",ProcStat.TP,"19",Fees.GetAmount0(codeNum,53));//amalgam on 19
			long procNum=proc.ProcNum;
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(patNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(patNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(patNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();//empty, not used for calcs.
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();//empty, not used for calcs.
			List<Procedure> procList=Procedures.Refresh(patNum);
			Procedure[] ProcListTP=Procedures.GetListTPandTPi(procList);//sorted by priority, then toothnum
			//Set complete and attach to claim
			ProcedureT.SetComplete(proc,pat,planList,patPlans,claimProcs,benefitList,subList);
			claimProcs=ClaimProcs.Refresh(patNum);
			List<Procedure> procsForClaim=new List<Procedure>();
			procsForClaim.Add(proc);
			Claim claim1=ClaimT.CreateClaim("P",patPlans,planList,claimProcs,procList,pat,procsForClaim,benefitList,subList);
			Claim claim2=ClaimT.CreateClaim("S",patPlans,planList,claimProcs,procList,pat,procsForClaim,benefitList,subList);
			//Validate
			string retVal="";
			Procedures.ComputeEstimates(ProcListTP[0],pat.PatNum,ref claimProcs,false,planList,patPlans,benefitList,
				histList,loopList,false,pat.Age,subList);
			//save changes in the list to the database
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			ClaimProc claimProc1=ClaimProcs.GetEstimate(claimProcs,proc.ProcNum,planNum1,subNum1);
			if(claimProc1.InsEstTotal!=1023.20) {
				throw new Exception("Primary Estimate was "+claimProc1.InsEstTotal+", should be 1023.20.\r\n");
			}
			Assert.AreEqual(1023.20,claimProc1.InsEstTotal);
			ClaimProc claimProc2=ClaimProcs.GetEstimate(claimProcs,proc.ProcNum,planNum2,subNum2);
			/*Is this ok, or do we need to take another look?
			if(claimProc2.WriteOff!=0) {//Insurance should not cover.
				throw new Exception("Secondary writeoff was "+claimProc2.WriteOff+", should be 0.\r\n");
			}
			if(claimProc2.InsEstTotal!=0) {//Insurance should not cover.
				throw new Exception("Secondary Estimate was "+claimProc2.InsEstTotal+", should be 0.\r\n");
			}*/
			retVal+="14: Passed. Primary estimate are not affected by secondary claim.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestFifteen() {
			//if(specificTest != 0 && specificTest !=15){
			//	return"";
			//}
			string suffix="15";
			Patient pat=PatientT.CreatePatient(suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			BenefitT.CreateAnnualMax(plan.PlanNum,1000);
			BenefitT.CreateDeductibleGeneral(plan.PlanNum,BenefitCoverageLevel.Individual,50);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.RoutinePreventive,100);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Diagnostic,100);
			BenefitT.CreateDeductible(plan.PlanNum,EbenefitCategory.RoutinePreventive,0);
			BenefitT.CreateDeductible(plan.PlanNum,EbenefitCategory.Diagnostic,0);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Restorative,80);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Endodontics,80);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Periodontics,80);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.OralSurgery,80);
			BenefitT.CreateDeductible(plan.PlanNum,"D0330",45);
			PatPlanT.CreatePatPlan(1,pat.PatNum,sub.InsSubNum);
			//proc1 - Pano
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0330",ProcStat.TP,"",95);
			ProcedureT.SetPriority(proc1,0);
			//proc2 - Amalg
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D2150",ProcStat.TP,"30",200);
			ProcedureT.SetPriority(proc2,1);
			//Lists:
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<Procedure>	ProcList=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP=Procedures.GetListTPandTPi(ProcList);//sorted by priority, then toothnum
			//Validate
			string retVal="";
			for(int i=0;i<ProcListTP.Length;i++){
				Procedures.ComputeEstimates(ProcListTP[i],pat.PatNum,ref claimProcs,false,planList,patPlans,benefitList,
					histList,loopList,false,pat.Age,subList);
				//then, add this information to loopList so that the next procedure is aware of it.
				loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs,ProcListTP[i].ProcNum,ProcListTP[i].CodeNum));
			}
			//save changes in the list to the database
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			ClaimProc claimProc1=ClaimProcs.GetEstimate(claimProcs,proc1.ProcNum,plan.PlanNum,sub.InsSubNum);
			ClaimProc claimProc2=ClaimProcs.GetEstimate(claimProcs,proc2.ProcNum,plan.PlanNum,sub.InsSubNum);
			//if(claimProc1.DedEst!=45){
			//	throw new Exception("Estimate 1 should be 45. Is " + claimProc1.DedEst + ".\r\n");
			//}
			Assert.AreEqual(45,claimProc1.DedEst);
			//if(claimProc2.DedEst!=5) {
			//	throw new Exception("Estimate 2 should be 5. Is " + claimProc2.DedEst + ".\r\n");
			//}
			Assert.AreEqual(5,claimProc2.DedEst);
			retVal+="15: Passed. Deductibles can be created to override the regular deductible.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestSixteen() {
			//if(specificTest != 0 && specificTest !=16){
			//	return"";
			//}
			string suffix="16";
			Patient pat=PatientT.CreatePatient(suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);//guarantor is subscriber
			//BenefitT.CreateAnnualMax(plan.PlanNum,1000);//Irrelevant benefits bog down debugging.
			BenefitT.CreateDeductibleGeneral(plan.PlanNum,BenefitCoverageLevel.Individual,50);
			//BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.RoutinePreventive,100);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Diagnostic,100);
			//BenefitT.CreateDeductible(plan.PlanNum,EbenefitCategory.RoutinePreventive,0);
			BenefitT.CreateDeductible(plan.PlanNum,EbenefitCategory.Diagnostic,0);
			//BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Restorative,80);
			//BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Endodontics,80);
			//BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Periodontics,80);
			//BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.OralSurgery,80);
			BenefitT.CreateDeductible(plan.PlanNum,"D0330",45);
			BenefitT.CreateDeductible(plan.PlanNum,"D0220",25);
			PatPlanT.CreatePatPlan(1,pat.PatNum,sub.InsSubNum);
			//proc1 - Pano
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0330",ProcStat.TP,"",100);
			ProcedureT.SetPriority(proc1,0);
			//proc2 - Intraoral - periapical first film
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.TP,"",75);
			ProcedureT.SetPriority(proc2,1);
			//Lists:
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<Procedure>	ProcList=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP=Procedures.GetListTPandTPi(ProcList);//sorted by priority, then toothnum
			//Validate
			string retVal="";
			for(int i=0;i<ProcListTP.Length;i++){
				Procedures.ComputeEstimates(ProcListTP[i],pat.PatNum,ref claimProcs,false,planList,patPlans,benefitList,
					histList,loopList,false,pat.Age,subList);
				//then, add this information to loopList so that the next procedure is aware of it.
				loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs,ProcListTP[i].ProcNum,ProcListTP[i].CodeNum));
			}
			//save changes in the list to the database
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			ClaimProc claimProc1=ClaimProcs.GetEstimate(claimProcs,proc1.ProcNum,plan.PlanNum,sub.InsSubNum);
			ClaimProc claimProc2=ClaimProcs.GetEstimate(claimProcs,proc2.ProcNum,plan.PlanNum,sub.InsSubNum);
			//
			//if(claimProc1.DedEst!=45){
			//	throw new Exception("Estimate 1 should be 45. Is " + claimProc1.DedEst + ".\r\n");
			//}
			Assert.AreEqual(45,claimProc1.DedEst);
			//if(claimProc2.DedEst!=5) {
			//	throw new Exception("Estimate 2 should be 5. Is " + claimProc2.DedEst + ".\r\n");
			//}
			Assert.AreEqual(5,claimProc2.DedEst);
			retVal+="16: Passed. Multiple deductibles for categories do not exceed the regular deductible.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestSeventeen() {
			//if(specificTest != 0 && specificTest != 17){
			//	return"";
			//}
			string suffix="17";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			long feeSchedNum1=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix);
			long feeSchedNum2=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix+"b");
			//Standard Fee
			FeeT.RefreshCache();
			long codeNum=ProcedureCodes.GetCodeNum("D2750");
			Fee fee=Fees.GetFee(codeNum,53,0,0);
			if(fee==null) {
				fee=new Fee();
				fee.CodeNum=codeNum;
				fee.FeeSched=53;
				fee.Amount=1200;
				Fees.Insert(fee);
			}
			else {
				fee.Amount=1200;
				Fees.Update(fee);
			}
			//PPO fees
			fee=new Fee();
			fee.CodeNum=codeNum;
			fee.FeeSched=feeSchedNum1;
			fee.Amount=900;
			Fees.Insert(fee);
			fee=new Fee();
			fee.CodeNum=codeNum;
			fee.FeeSched=feeSchedNum2;
			fee.Amount=650;
			Fees.Insert(fee);
			FeeT.RefreshCache();
			//Carrier
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			long planNum1=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedNum1,EnumCobRule.Standard).PlanNum;
			long planNum2=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedNum2,EnumCobRule.Standard).PlanNum;
			InsSub sub1=InsSubT.CreateInsSub(pat.PatNum,planNum1);
			long subNum1=sub1.InsSubNum;
			InsSub sub2=InsSubT.CreateInsSub(pat.PatNum,planNum2);
			long subNum2=sub2.InsSubNum;
			BenefitT.CreateCategoryPercent(planNum1,EbenefitCategory.Crowns,50);
			BenefitT.CreateCategoryPercent(planNum2,EbenefitCategory.Crowns,50);
			PatPlanT.CreatePatPlan(1,patNum,subNum1);
			PatPlanT.CreatePatPlan(2,patNum,subNum2);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D2750",ProcStat.TP,"8",Fees.GetAmount0(codeNum,53));//crown on 8
			long procNum=proc.ProcNum;
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(patNum);
			Family fam=Patients.GetFamily(patNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(patNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			//Validate
			string retVal="";
			ClaimProc claimProc;
			//Test 17 Part 1 (copied from Unit Test 1)----------------------------------------------------------------------------------------------------
			Procedures.ComputeEstimates(proc,patNum,ref claimProcs,false,planList,patPlans,benefitList,histList,loopList,true,pat.Age,subList);
			claimProcs=ClaimProcs.Refresh(patNum);
			claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum1,subNum1);
			//I don't think allowed can be easily tested on the fly, and it's not that important.
			//if(claimProc.InsEstTotal!=450) {
			//	throw new Exception("Should be 450. \r\n");
			//}
			Assert.AreEqual(450,claimProc.InsEstTotal);
			//if(claimProc.WriteOffEst!=300) {
			//	throw new Exception("Should be 300. \r\n");
			//}
			Assert.AreEqual(300,claimProc.WriteOffEst);
			claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum2,subNum2);
			//if(claimProc.InsEstTotal!=325) {
			//	throw new Exception("Should be 325. \r\n");
			//}
			Assert.AreEqual(325,claimProc.InsEstTotal);
			//if(claimProc.WriteOffEst!=0) {
			//	throw new Exception("Should be 0. \r\n");
			//}
			Assert.AreEqual(0,claimProc.WriteOffEst);
			//Test 17 Part 2 (copied from Unit Test 2)----------------------------------------------------------------------------------------------------
			//switch the fees
			fee=Fees.GetFee(codeNum,feeSchedNum1,0,0);
			fee.Amount=650;
			Fees.Update(fee);
			fee=Fees.GetFee(codeNum,feeSchedNum2,0,0);
			fee.Amount=900;
			Fees.Update(fee);
			FeeT.RefreshCache();
			Procedures.ComputeEstimates(proc,patNum,ref claimProcs,false,planList,patPlans,benefitList,histList,loopList,true,pat.Age,subList);
			//Validate
			claimProcs=ClaimProcs.Refresh(patNum);
			claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum1,subNum1);
			//if(claimProc.InsEstTotal!=325) {
			//	throw new Exception("Should be 325. \r\n");
			//}
			Assert.AreEqual(325,claimProc.InsEstTotal);
			//if(claimProc.WriteOffEst!=550) {
			//	throw new Exception("Should be 550. \r\n");
			//}
			Assert.AreEqual(550,claimProc.WriteOffEst);
			claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum2,subNum2);
			//if(claimProc.InsEstTotal!=325) {
			//	throw new Exception("Should be 325. \r\n");
			//}
			Assert.AreEqual(325,claimProc.InsEstTotal);
			//if(claimProc.WriteOffEst!=0) {
			//	throw new Exception("Should be 0. \r\n");
			//}
			Assert.AreEqual(0,claimProc.WriteOffEst);
			retVal+="17: Passed.  Standard COB with PPOs.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestEighteen() {
			//if(specificTest != 0 && specificTest != 18){
			//	return"";
			//}
			string suffix="18";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			//long feeSchedNum1=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix);
			//long feeSchedNum2=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix+"b");
			//Carrier
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			long planNum1=InsPlanT.CreateInsPlan(carrier.CarrierNum,EnumCobRule.CarveOut).PlanNum;
			long planNum2=InsPlanT.CreateInsPlan(carrier.CarrierNum,EnumCobRule.CarveOut).PlanNum;
			InsSub sub1=InsSubT.CreateInsSub(pat.PatNum,planNum1);
			long subNum1=sub1.InsSubNum;
			InsSub sub2=InsSubT.CreateInsSub(pat.PatNum,planNum2);
			long subNum2=sub2.InsSubNum;
			BenefitT.CreateCategoryPercent(planNum1,EbenefitCategory.Crowns,50);
			BenefitT.CreateCategoryPercent(planNum2,EbenefitCategory.Crowns,75);
			PatPlanT.CreatePatPlan(1,patNum,subNum1);
			PatPlanT.CreatePatPlan(2,patNum,subNum2);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D2750",ProcStat.TP,"8",1200);//crown on 8
			long procNum=proc.ProcNum;
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(patNum);
			Family fam=Patients.GetFamily(patNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(patNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			//Validate
			string retVal="";
			ClaimProc claimProc;
			//Test 18 Part 1 (copied from Unit Test 1)----------------------------------------------------------------------------------------------------
			Procedures.ComputeEstimates(proc,patNum,ref claimProcs,false,planList,patPlans,benefitList,histList,loopList,true,pat.Age,subList);
			claimProcs=ClaimProcs.Refresh(patNum);
			claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum1,subNum1);
			//I don't think allowed can be easily tested on the fly, and it's not that important.
			//if(claimProc.InsEstTotal!=600) {
			//	throw new Exception("Should be 600. \r\n");
			//}
			Assert.AreEqual(600,claimProc.InsEstTotal);
			claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum2,subNum2);
			//if(claimProc.InsEstTotal!=300) {
			//	throw new Exception("Should be 300. \r\n");
			//}
			Assert.AreEqual(300,claimProc.InsEstTotal);
			retVal+="18: Passed. CarveOut using category percentage.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestNineteen() {
			//if(specificTest != 0 && specificTest !=19){
			//	return"";
			//}
			string suffix="19";
			Patient pat=PatientT.CreatePatient(suffix);
			Carrier carrier1=CarrierT.CreateCarrier(suffix);
			Carrier carrier2=CarrierT.CreateCarrier(suffix);
			InsPlan plan1=InsPlanT.CreateInsPlan(carrier1.CarrierNum);
			InsPlan plan2=InsPlanT.CreateInsPlan(carrier2.CarrierNum);
			InsSub sub1=InsSubT.CreateInsSub(pat.PatNum,plan1.PlanNum);
			InsSub sub2=InsSubT.CreateInsSub(pat.PatNum,plan2.PlanNum);
			long subNum1=sub1.InsSubNum;
			long subNum2=sub2.InsSubNum;
			//plans
			BenefitT.CreateCategoryPercent(plan1.PlanNum,EbenefitCategory.Diagnostic,50);
			BenefitT.CreateCategoryPercent(plan2.PlanNum,EbenefitCategory.Diagnostic,50);
			BenefitT.CreateDeductibleGeneral(plan1.PlanNum,BenefitCoverageLevel.Individual,50);
			BenefitT.CreateDeductibleGeneral(plan2.PlanNum,BenefitCoverageLevel.Individual,50);
			PatPlanT.CreatePatPlan(1,pat.PatNum,subNum1);
			PatPlanT.CreatePatPlan(2,pat.PatNum,subNum2);
			//proc1 - PerExam
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.TP,"",150);
			//Lists:
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<Procedure>	ProcList=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP=Procedures.GetListTPandTPi(ProcList);//sorted by priority, then toothnum
			//Validate
			string retVal="";
			for(int i=0;i<ProcListTP.Length;i++){
				Procedures.ComputeEstimates(ProcListTP[i],pat.PatNum,ref claimProcs,false,planList,patPlans,benefitList,
					histList,loopList,false,pat.Age,subList);
				//then, add this information to loopList so that the next procedure is aware of it.
				loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs,ProcListTP[i].ProcNum,ProcListTP[i].CodeNum));
			}
			//save changes in the list to the database
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			ClaimProc claimProc1=ClaimProcs.GetEstimate(claimProcs,proc.ProcNum,plan1.PlanNum,subNum1);
			ClaimProc claimProc2=ClaimProcs.GetEstimate(claimProcs,proc.ProcNum,plan2.PlanNum,subNum2);
			//if(claimProc1.DedEst!=50) {//$50 deductible
			//	throw new Exception("Should be 50. \r\n");
			//}
			Assert.AreEqual(50,claimProc1.DedEst);
			//if(claimProc1.InsEstTotal!=50) {//Ins1 pays 40% of (fee - deductible) = .4 * (150 - 50)
			//	throw new Exception("Should be 50. \r\n");
			//}
			Assert.AreEqual(50,claimProc1.InsEstTotal);
			//if(claimProc2.DedEst!=50) {//$50 deductible
			//	throw new Exception("Should be 50. \r\n");
			//}
			Assert.AreEqual(50,claimProc2.DedEst);
			//if(claimProc2.InsEstTotal!=50) {//Ins2 pays 
			//	throw new Exception("Should be 50. \r\n");
			//}
			Assert.AreEqual(50,claimProc2.InsEstTotal);
			retVal+="19: Passed.  Multiple deductibles are accounted for.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestTwenty() {
			//if(specificTest != 0 && specificTest !=20) {
			//	return "";
			//}
			string suffix="20";
			Patient pat=PatientT.CreatePatient(suffix);//guarantor
			long patNum=pat.PatNum;
			Patient pat2=PatientT.CreatePatient(suffix);
			PatientT.SetGuarantor(pat2,pat.PatNum);
			Patient pat3=PatientT.CreatePatient(suffix);
			PatientT.SetGuarantor(pat3,pat.PatNum);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			long planNum=plan.PlanNum;
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,planNum);//guarantor is subscriber
			long subNum=sub.InsSubNum;
			PatPlan patPlan=PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);//all three patients have the same plan
			PatPlan patPlan2=PatPlanT.CreatePatPlan(1,pat2.PatNum,subNum);//all three patients have the same plan
			PatPlan patPlan3=PatPlanT.CreatePatPlan(1,pat3.PatNum,subNum);//all three patients have the same plan
			BenefitT.CreateDeductibleGeneral(planNum,BenefitCoverageLevel.Individual,75);
			BenefitT.CreateDeductibleGeneral(planNum,BenefitCoverageLevel.Family,150);
			ClaimProcT.AddInsUsedAdjustment(pat3.PatNum,planNum,0,subNum,75);//Adjustment goes on the third patient
			Procedure proc=ProcedureT.CreateProcedure(pat2,"D2750",ProcStat.C,"20",1280);//proc for second patient with a deductible already applied.
			ClaimProcT.AddInsPaid(pat2.PatNum,planNum,proc.ProcNum,304,subNum,50,597);
			proc=ProcedureT.CreateProcedure(pat,"D4355",ProcStat.TP,"",135);//proc is for the first patient
			long procNum=proc.ProcNum;
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(patNum);
			Family fam=Patients.GetFamily(patNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(patNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=ClaimProcs.GetHistList(patNum,benefitList,patPlans,planList,DateTime.Today,subList);
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			//Validate
			List<ClaimProcHist> HistList=ClaimProcs.GetHistList(pat.PatNum,benefitList,patPlans,planList,DateTime.Today,subList);
			double dedFam=Benefits.GetDeductGeneralDisplay(benefitList,planNum,patPlan.PatPlanNum,BenefitCoverageLevel.Family);
			double ded=Benefits.GetDeductGeneralDisplay(benefitList,planNum,patPlan.PatPlanNum,BenefitCoverageLevel.Individual);
			double dedRem=InsPlans.GetDedRemainDisplay(HistList,DateTime.Today,planNum,patPlan.PatPlanNum,-1,planList,pat.PatNum,ded,dedFam);//test family and individual deductible together
			//if(dedRem!=25) { //guarantor deductible
			//	throw new Exception("Guarantor combination deductible remaining should be 25.\r\n");
			//}
			Assert.AreEqual(25,dedRem);
			dedRem=InsPlans.GetDedRemainDisplay(HistList,DateTime.Today,planNum,patPlan.PatPlanNum,-1,planList,pat.PatNum,ded,-1);//test individual deductible by itself
			//if(dedRem!=75) { //guarantor deductible
			//	throw new Exception("Guarantor individual deductible remaining should be 75.\r\n");
			//}
			Assert.AreEqual(75,dedRem);
			//return "20: Passed.  Both individual and family deductibles taken into account.\r\n";
		}

		[TestMethod]
		public void Legacy_TestTwentyOne() {
			//if(specificTest != 0 && specificTest !=21){
			//	return"";
			//}
			string suffix="21";
			Patient pat=PatientT.CreatePatient(suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);//guarantor is subscriber
			BenefitT.CreateAnnualMax(plan.PlanNum,1000);
			BenefitT.CreateDeductibleGeneral(plan.PlanNum,BenefitCoverageLevel.Individual,50);
			PatPlanT.CreatePatPlan(1,pat.PatNum,sub.InsSubNum);
			//proc - Exam
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.TP,"",55);
			ProcedureT.SetPriority(proc1,0);
			//Lists:
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<Procedure>	ProcList=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP=Procedures.GetListTPandTPi(ProcList);//sorted by priority, then toothnum
			//Validate
			string retVal="";
			for(int i=0;i<ProcListTP.Length;i++){
				Procedures.ComputeEstimates(ProcListTP[i],pat.PatNum,ref claimProcs,false,planList,patPlans,benefitList,
					histList,loopList,false,pat.Age,subList);
				//then, add this information to loopList so that the next procedure is aware of it.
				loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs,ProcListTP[i].ProcNum,ProcListTP[i].CodeNum));
			}
			//save changes in the list to the database
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			ClaimProc claimProc1=ClaimProcs.GetEstimate(claimProcs,proc1.ProcNum,plan.PlanNum,sub.InsSubNum);
			//Check
			//if(claimProc1.DedEst!=0){
			//	throw new Exception("Estimated deduction should be 0, is " + claimProc1.DedEst + ".\r\n");
			//}
			Assert.AreEqual(0,claimProc1.DedEst);
			retVal+="21: Passed. Deductibles are not applied to procedures that are not covered.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestTwentyTwo() {
			//Deprecated
		}

		[TestMethod]
		public void Legacy_TestTwentyThree() {
			//Deprecated
		}

		[TestMethod]
		public void Legacy_TestTwentyFour() {
			//if(specificTest != 0 && specificTest !=24) {
			//	return "";
			//}
			//Why was this test deprecated. This should be documented somewhere, if not here.
			string suffix="24";
			DateTime startDate=DateTime.Parse("2001-01-01");
			Employee emp = EmployeeT.CreateEmployee(suffix);
			PayPeriod payP1 = PayPeriodT.CreateTwoWeekPayPeriodIfNotExists(startDate);
			PayPeriods.RefreshCache();
			Prefs.UpdateInt(PrefName.TimeCardOvertimeFirstDayOfWeek,0);
			Prefs.UpdateBool(PrefName.TimeCardsMakesAdjustmentsForOverBreaks,true);
			TimeCardRuleT.CreateHoursTimeRule(emp.EmployeeNum,TimeSpan.FromHours(10));
			TimeCardRules.RefreshCache();
			long clockEvent1 = ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddHours(8),startDate.AddHours(13),0);
			long clockEvent2 = ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddHours(14),startDate.AddHours(21),0);
			ClockEventT.InsertBreak(emp.EmployeeNum,startDate.AddHours(10),20,0);
			ClockEventT.InsertBreak(emp.EmployeeNum,startDate.AddHours(16),20,0);
			TimeCardRules.CalculateDailyOvertime(emp,payP1.DateStart,payP1.DateStop);
			//Validate
			string retVal="";
			//Check
			//if(ClockEvents.GetOne(clockEvent2).AdjustAuto!=TimeSpan.FromMinutes(-10)) {
			//	throw new Exception("Clock adjustment should be -10 minutes, instead it is " + ClockEvents.GetOne(clockEvent2).AdjustAuto.TotalMinutes + " minutes.\r\n");
			//}
			Assert.AreEqual(ClockEvents.GetOne(clockEvent2).AdjustAuto,TimeSpan.FromMinutes(-10));
			//if(ClockEvents.GetOne(clockEvent2).OTimeAuto!=TimeSpan.FromMinutes(110)) {
			//	throw new Exception("Clock ovetime should be 110 minutes, instead it is " + ClockEvents.GetOne(clockEvent2).OTimeAuto.TotalMinutes + " minutes.\r\n");
			//}
			Assert.AreEqual(ClockEvents.GetOne(clockEvent2).AdjustAuto,TimeSpan.FromMinutes(-10));
			retVal+="24: Passed. Overtime calculated properly for total hours per day.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestTwentyFive() {
			//if(specificTest != 0 && specificTest !=25) {
			//	return "";
			//}
			//Why was this test deprecated. This should be documented somewhere, if not here.
			string suffix="25";
			DateTime startDate=DateTime.Parse("2001-01-01");
			Employee emp = EmployeeT.CreateEmployee(suffix);
			PayPeriod payP1 = PayPeriodT.CreateTwoWeekPayPeriodIfNotExists(startDate);
			PayPeriods.RefreshCache();
			Prefs.UpdateInt(PrefName.TimeCardOvertimeFirstDayOfWeek,0);
			TimeCardRules.RefreshCache();
			long clockEvent1 = ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(0).AddHours(6),startDate.AddDays(0).AddHours(17),0);
			long clockEvent2 = ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(1).AddHours(6),startDate.AddDays(1).AddHours(17),0);
			long clockEvent3 = ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(2).AddHours(6),startDate.AddDays(2).AddHours(17),0);
			long clockEvent4 = ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(3).AddHours(6),startDate.AddDays(3).AddHours(17),0);
			TimeCardRules.CalculateWeeklyOvertime(emp,payP1.DateStart,payP1.DateStop);
			//Validate
			string retVal="";
			//Check
			TimeAdjust result = TimeAdjusts.Refresh(emp.EmployeeNum,startDate,startDate.AddDays(13))[0];
			//if(result.RegHours!=TimeSpan.FromHours(-4)){
			//	throw new Exception("Time adjustment to regular hours should be -4 hours, instead it is " + result.RegHours.TotalHours + " hours.\r\n");
			//}
			Assert.AreEqual(result.RegHours,TimeSpan.FromHours(-4));
			//if(result.OTimeHours!=TimeSpan.FromHours(4)) {
			//	throw new Exception("Time adjustment to OT hours should be 4 hours, instead it is " + result.OTimeHours.TotalHours + " hours.\r\n");
			//}
			Assert.AreEqual(result.OTimeHours,TimeSpan.FromHours(4));
			retVal+="25: Passed. Overtime calculated properly for normal 40 hour work week.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestTwentySix() {
			//if(specificTest != 0 && specificTest !=26) {
			//	return "";
			//}
			//Why was this test deprecated. This should be documented somewhere, if not here.
			string suffix="26";
			DateTime startDate=DateTime.Parse("2001-02-01");//This will create a pay period that splits a work week.
			Employee emp = EmployeeT.CreateEmployee(suffix);
			PayPeriod payP1 = PayPeriodT.CreateTwoWeekPayPeriodIfNotExists(startDate);
			PayPeriod payP2 = PayPeriodT.CreateTwoWeekPayPeriodIfNotExists(startDate.AddDays(14));
			PayPeriods.RefreshCache();
			Prefs.UpdateInt(PrefName.TimeCardOvertimeFirstDayOfWeek,0);
			TimeCardRules.RefreshCache();
			long clockEvent1 = ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(10).AddHours(6),startDate.AddDays(10).AddHours(17),0);
			long clockEvent2 = ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(11).AddHours(6),startDate.AddDays(11).AddHours(17),0);
			long clockEvent3 = ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(12).AddHours(6),startDate.AddDays(12).AddHours(17),0);
			//new pay period
			long clockEvent4 = ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(14).AddHours(6),startDate.AddDays(14).AddHours(17),0);
			TimeCardRules.CalculateWeeklyOvertime(emp,payP1.DateStart,payP1.DateStop);
			TimeCardRules.CalculateWeeklyOvertime(emp,payP2.DateStart,payP2.DateStop);
			//Validate
			string retVal="";
			//Check
			List<TimeAdjust> resultList=TimeAdjusts.Refresh(emp.EmployeeNum,startDate,startDate.AddDays(28));
			//if(resultList.Count < 1) {
			//	throw new Exception("No time adjustments were found.  Should never happen.\r\n");
			//}
			Assert.IsFalse(resultList.Count < 1);
			TimeAdjust result=resultList[0];
			//if(result.RegHours!=TimeSpan.FromHours(-4)) {
			//	throw new Exception("Time adjustment to regular hours should be -4 hours, instead it is " + result.RegHours.TotalHours + " hours.\r\n");
			//}
			Assert.AreEqual(result.RegHours,TimeSpan.FromHours(-4));
			//if(result.OTimeHours!=TimeSpan.FromHours(4)) {
			//	throw new Exception("Time adjustment to OT hours should be 4 hours, instead it is " + result.OTimeHours.TotalHours + " hours.\r\n");
			//}
			Assert.AreEqual(result.OTimeHours,TimeSpan.FromHours(4));
			retVal+="26: Passed. Overtime calculated properly for work week spanning 2 pay periods.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestTwentySeven() {
			//if(specificTest != 0 && specificTest !=27) {
			//	return "";
			//}
			//Why was this test deprecated. This should be documented somewhere, if not here.
			string suffix="27";
			DateTime startDate=DateTime.Parse("2001-01-01");
			Employee emp = EmployeeT.CreateEmployee(suffix);
			PayPeriod payP1 = PayPeriodT.CreateTwoWeekPayPeriodIfNotExists(startDate);
			PayPeriods.RefreshCache();
			Prefs.UpdateInt(PrefName.TimeCardOvertimeFirstDayOfWeek,3);
			TimeCardRules.RefreshCache();
			long clockEvent1 = ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(0).AddHours(6),startDate.AddDays(0).AddHours(17),0);
			long clockEvent2 = ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(1).AddHours(6),startDate.AddDays(1).AddHours(17),0);
			//new work week
			long clockEvent3 = ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(2).AddHours(6),startDate.AddDays(2).AddHours(17),0);
			long clockEvent4 = ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(3).AddHours(6),startDate.AddDays(3).AddHours(17),0);
			long clockEvent5 = ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(4).AddHours(6),startDate.AddDays(4).AddHours(17),0);
			long clockEvent6 = ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(5).AddHours(6),startDate.AddDays(5).AddHours(17),0);
			TimeCardRules.CalculateWeeklyOvertime(emp,payP1.DateStart,payP1.DateStop);
			//Validate
			string retVal="";
			//Check
			TimeAdjust result = TimeAdjusts.Refresh(emp.EmployeeNum,startDate,startDate.AddDays(28))[0];
			//if(result.RegHours!=TimeSpan.FromHours(-4)) {
			//	throw new Exception("Time adjustment to regular hours should be -4 hours, instead it is " + result.RegHours.TotalHours + " hours.\r\n");
			//}
			Assert.AreEqual(result.RegHours,TimeSpan.FromHours(-4));
			//if(result.OTimeHours!=TimeSpan.FromHours(4)) {
			//	throw new Exception("Time adjustment to OT hours should be 4 hours, instead it is " + result.OTimeHours.TotalHours + " hours.\r\n");
			//}
			Assert.AreEqual(result.OTimeHours,TimeSpan.FromHours(4));
			retVal+="27: Passed. Overtime calculated properly for work week not starting on Sunday.\r\n";
			//return retVal;
		}

		///<summary>This unit test is the first one that looks at the values showing in the claimproc window.  This catches situations where the only "bug" is just a display issue in that window. Validates the values in the claimproc window when opened from the Chart module.</summary>
		[TestMethod]
		public void Legacy_TestTwentyEight() {
			//if(specificTest != 0 && specificTest !=28) {
			//	return "";
			//}
			string suffix="28";
			Patient pat=PatientT.CreatePatient(suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			long subNum=sub.InsSubNum;
			BenefitT.CreateAnnualMax(plan.PlanNum,1300);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Crowns,50);
			BenefitT.CreateDeductibleGeneral(plan.PlanNum,BenefitCoverageLevel.Individual,25);
			PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			//proc1 - crown
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D2790",ProcStat.TP,"1",800);//Tooth 1
			ProcedureT.SetPriority(proc1,0);//Priority 1
			//proc2 - crown
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D2790",ProcStat.TP,"9",800);//Tooth 9
			ProcedureT.SetPriority(proc2,1);//Priority 2
			//Lists:
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<Procedure> ProcList=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP=Procedures.GetListTPandTPi(ProcList);//sorted by priority, then toothnum
			//Validate
			//Mimick the TP module estimate calculations when the TP module is loaded. We expect the user to refresh the TP module to calculate insurance estimates for all other areas of the program.
			string retVal="";
			for(int i=0;i<ProcListTP.Length;i++) {
				Procedures.ComputeEstimates(ProcListTP[i],pat.PatNum,ref claimProcs,false,planList,patPlans,benefitList,
					histList,loopList,false,pat.Age,subList);
				//then, add this information to loopList so that the next procedure is aware of it.
				loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs,ProcListTP[i].ProcNum,ProcListTP[i].CodeNum));
			}
			//Save changes in the list to the database, just like the TP module does when loaded. Then the values can be referenced elsewhere in the program instead of recalculating.
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			//Validate the estimates within the Edit Claim Proc window are correct when opened from inside of the Chart module by passing in a null histlist and a null looplist just like the Chart module would.
			List<ClaimProcHist> histListNull=null;
			List<ClaimProcHist> loopListNull=null;
			ClaimProc claimProc1=ClaimProcs.GetEstimate(claimProcs,proc1.ProcNum,plan.PlanNum,subNum);
			FormClaimProc formCP1=new FormClaimProc(claimProc1,proc1,fam,pat,planList,histListNull,ref loopListNull,patPlans,false,subList);
			formCP1.Initialize();
			string dedEst1=formCP1.GetTextValue("textDedEst");
			//if(dedEst1!="25.00") {
			//	throw new Exception("Deductible estimate in Claim Proc Edit window is $"+dedEst1+" but should be $25.00 for proc1 from Chart module. \r\n");
			//}
			Assert.AreEqual("25.00",dedEst1);
			string patPortCP1=formCP1.GetTextValue("textPatPortion1");
			//if(patPortCP1!="412.50") {
			//	throw new Exception("Estimated patient portion in Claim Proc Edit window is $"+patPortCP1+" but should be $412.50 for proc1 from Chart module. \r\n");
			//}
			Assert.AreEqual("412.50",patPortCP1);
			ClaimProc claimProc2=ClaimProcs.GetEstimate(claimProcs,proc2.ProcNum,plan.PlanNum,subNum);
			FormClaimProc formCP2=new FormClaimProc(claimProc2,proc2,fam,pat,planList,histListNull,ref loopListNull,patPlans,false,subList);
			formCP2.Initialize();
			string dedEst2=formCP2.GetTextValue("textDedEst");
			//if(dedEst2!="0.00") {
			//	throw new Exception("Deductible estimate in Claim Proc Edit window is $"+dedEst2+" but should be $0.00 for proc2 from Chart module. \r\n");
			//}
			Assert.AreEqual("0.00",dedEst2);
			string patPortCP2=formCP2.GetTextValue("textPatPortion1");
			//if(patPortCP2!="400.00") {
			//	throw new Exception("Estimated patient portion in Claim Proc Edit window is $"+patPortCP2+" but should be $400.00 for proc2 from Chart module. \r\n");
			//}
			Assert.AreEqual("400.00",patPortCP2);
			retVal+="28: Passed.  Claim Procedure Edit window estimates correct from Chart module.\r\n";
			//return retVal;
		}

		///<summary>Validates the values in the claimproc window when opened from the Claim Edit window.</summary>
		[TestMethod]
		public void Legacy_TestTwentyNine() {
			//if(specificTest != 0 && specificTest !=29) {
			//	return "";
			//}
			string suffix="29";
			Patient pat=PatientT.CreatePatient(suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			long subNum=sub.InsSubNum;
			BenefitT.CreateAnnualMax(plan.PlanNum,1300);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Crowns,50);
			BenefitT.CreateDeductibleGeneral(plan.PlanNum,BenefitCoverageLevel.Individual,25);
			PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			//proc1 - crown
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D2790",ProcStat.C,"1",800);//Tooth 1
			ProcedureT.SetPriority(proc1,0);//Priority 1
			//proc2 - crown
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D2790",ProcStat.C,"9",800);//Tooth 9
			ProcedureT.SetPriority(proc2,1);//Priority 2
			//Lists:
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<Procedure> ProcList=Procedures.Refresh(pat.PatNum);
			string retVal="";
			Claim claim=ClaimT.CreateClaim("P",patPlans,planList,claimProcs,ProcList,pat,ProcList,benefitList,subList);//Creates the claim in the same manner as the account module, including estimates.
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			//Validate the estimates as they would appear inside of the Claim Proc Edit window when opened from inside of the Edit Claim window by passing in the null histlist and null looplist that the Claim Edit window would send in.
			List<ClaimProcHist> histList=null;
			List<ClaimProcHist> loopList=null;
			ClaimProc claimProc1=ClaimProcs.GetEstimate(claimProcs,proc1.ProcNum,plan.PlanNum,subNum);
			FormClaimProc formCP1=new FormClaimProc(claimProc1,proc1,fam,pat,planList,histList,ref loopList,patPlans,false,subList);
			formCP1.IsInClaim=true;
			formCP1.Initialize();
			string dedEst1=formCP1.GetTextValue("textDedEst");
			//if(dedEst1!="25.00") {
			//	throw new Exception("Deductible estimate in Claim Proc Edit window is $"+dedEst1+" but should be $25.00 for proc1 from Edit Claim Window. \r\n");
			//}
			Assert.AreEqual("25.00",dedEst1);
			string patPortCP1=formCP1.GetTextValue("textPatPortion1");
			//if(patPortCP1!="412.50") {
			//	throw new Exception("Estimated patient portion in Claim Proc Edit window is $"+patPortCP1+" but should be $412.50 for proc1 from Edit Claim Window. \r\n");
			//}
			Assert.AreEqual("412.50",patPortCP1);
			ClaimProc claimProc2=ClaimProcs.GetEstimate(claimProcs,proc2.ProcNum,plan.PlanNum,subNum);
			FormClaimProc formCP2=new FormClaimProc(claimProc2,proc2,fam,pat,planList,histList,ref loopList,patPlans,false,subList);
			formCP2.IsInClaim=true;
			formCP2.Initialize();
			string dedEst2=formCP2.GetTextValue("textDedEst");
			//if(dedEst2!="0.00") {
			//	throw new Exception("Deductible estimate in Claim Proc Edit window is $"+dedEst2+" but should be $0.00 for proc2 from Edit Claim Window. \r\n");
			//}
			Assert.AreEqual("0.00",dedEst2);
			string patPortCP2=formCP2.GetTextValue("textPatPortion1");
			//if(patPortCP2!="400.00") {
			//	throw new Exception("Estimated patient portion in Claim Proc Edit window is $"+patPortCP2+" but should be $400.00 for proc2 from Edit Claim Window. \r\n");
			//}
			Assert.AreEqual("400.00",patPortCP2);
			retVal+="29: Passed.  Claim Procedure Edit window estimates correct from Claim Edit window.\r\n";
			//return retVal;
		}

		///<summary>Validates the values in the claimproc window when opened from the TP module.</summary>
		[TestMethod]
		public void Legacy_TestThirty() {
			//if(specificTest != 0 && specificTest !=30) {
			//	return "";
			//}
			string suffix="30";
			Patient pat=PatientT.CreatePatient(suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			long subNum=sub.InsSubNum;
			BenefitT.CreateAnnualMax(plan.PlanNum,1300);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Crowns,50);
			BenefitT.CreateDeductibleGeneral(plan.PlanNum,BenefitCoverageLevel.Individual,25);
			PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			//proc1 - crown
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D2790",ProcStat.TP,"1",800);//Tooth 1
			ProcedureT.SetPriority(proc1,0);//Priority 1
			//proc2 - crown
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D2790",ProcStat.TP,"9",800);//Tooth 9
			ProcedureT.SetPriority(proc2,1);//Priority 2
			//Lists:
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<Procedure> ProcList=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP=Procedures.GetListTPandTPi(ProcList);//sorted by priority, then toothnum
			//Validate
			//Mimick the TP module estimate calculations when the TP module is loaded.
			string retVal="";
			for(int i=0;i<ProcListTP.Length;i++) {
				Procedures.ComputeEstimates(ProcListTP[i],pat.PatNum,ref claimProcs,false,planList,patPlans,benefitList,
					histList,loopList,false,pat.Age,subList);
				//then, add this information to loopList so that the next procedure is aware of it.
				loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs,ProcListTP[i].ProcNum,ProcListTP[i].CodeNum));
			}
			//Save changes in the list to the database, just like the TP module does when loaded.
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			//Validate the estimates within the Edit Claim Proc window are correct when opened from inside of the TP module by passing in same histlist and loop list that the TP module would.
			histList=ClaimProcs.GetHistList(pat.PatNum,benefitList,patPlans,planList,DateTime.Today,subList);//The history list is fetched when the TP module is loaded and is passed in the same for all claimprocs.
			loopList=new List<ClaimProcHist>();//Always empty for the first claimproc.
			ClaimProc claimProc1=ClaimProcs.GetEstimate(claimProcs,proc1.ProcNum,plan.PlanNum,subNum);
			FormClaimProc formCP1=new FormClaimProc(claimProc1,proc1,fam,pat,planList,histList,ref loopList,patPlans,false,subList);
			formCP1.Initialize();
			string dedEst1=formCP1.GetTextValue("textDedEst");
			//if(dedEst1!="25.00") {
			//	throw new Exception("Deductible estimate in Claim Proc Edit window is $"+dedEst1+" but should be $25.00 for proc1 from TP module. \r\n");
			//}
			Assert.AreEqual("25.00",dedEst1);
			string patPortCP1=formCP1.GetTextValue("textPatPortion1");
			//if(patPortCP1!="412.50") {
			//	throw new Exception("Estimated patient portion in Claim Proc Edit window is $"+patPortCP1+" but should be $412.50 for proc1 from TP module. \r\n");
			//}
			Assert.AreEqual("412.50",patPortCP1);
			ClaimProc claimProc2=ClaimProcs.GetEstimate(claimProcs,proc2.ProcNum,plan.PlanNum,subNum);
			histList=ClaimProcs.GetHistList(pat.PatNum,benefitList,patPlans,planList,DateTime.Today,subList);//The history list is fetched when the TP module is loaded and is passed in the same for all claimprocs.
			loopList=new List<ClaimProcHist>();
			loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs,proc1.ProcNum,proc1.CodeNum));
			FormClaimProc formCP2=new FormClaimProc(claimProc2,proc2,fam,pat,planList,histList,ref loopList,patPlans,false,subList);
			formCP2.Initialize();
			string dedEst2=formCP2.GetTextValue("textDedEst");
			//if(dedEst2!="0.00") {
			//	throw new Exception("Deductible estimate in Claim Proc Edit window is $"+dedEst2+" but should be $0.00 for proc2 from TP module. \r\n");
			//}
			Assert.AreEqual("0.00",dedEst2);
			string patPortCP2=formCP2.GetTextValue("textPatPortion1");
			//if(patPortCP2!="400.00") {
			//	throw new Exception("Estimated patient portion in Claim Proc Edit window is $"+patPortCP2+" but should be $400.00 for proc2 from TP module. \r\n");
			//}
			Assert.AreEqual("400.00",patPortCP2);
			retVal+="30: Passed.  Claim Procedure Edit window estimates correct from TP module.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestThirtyOne() {
			//if(specificTest != 0 && specificTest !=31) {
			//	return "";
			//}
			string suffix="31";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			long planNum=plan.PlanNum;
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,planNum);//guarantor is subscriber
			long subNum=sub.InsSubNum;
			long patPlanNum=PatPlanT.CreatePatPlan(1,pat.PatNum,subNum).PatPlanNum;
			BenefitT.CreateAnnualMax(planNum,1000);
			BenefitT.CreateCategoryPercent(planNum,EbenefitCategory.RoutinePreventive,100);
			BenefitT.CreateLimitation(planNum,EbenefitCategory.RoutinePreventive,1000);//Changing this amount would affect patient portion vs ins portion.  But regardless of the amount, this should prevent any pending from showing in the box, which is for general pending only.
			Procedure proc=ProcedureT.CreateProcedure(pat,"D1110",ProcStat.C,"",125);//Prophy
			long procNum=proc.ProcNum;
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			Family fam=Patients.GetFamily(patNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(patNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<Procedure> ProcList=Procedures.Refresh(pat.PatNum);
			Claim claim=ClaimT.CreateClaim("P",patPlans,planList,claimProcs,ProcList,pat,ProcList,benefitList,subList);//Creates the claim in the same manner as the account module, including estimates and status NotReceived.
			List<ClaimProcHist> histList=ClaimProcs.GetHistList(patNum,benefitList,patPlans,planList,DateTime.Today,subList);
			//Validate
			double pending=InsPlans.GetPendingDisplay(histList,DateTime.Today,plan,patPlanNum,-1,patNum,subNum,benefitList);
			//The a limitation for preventive should override the general limitation.
			//The 125 should apply to preventive, not general.
			//This display box that we are looking at is only supposed to represent general.
			//if(pending!=0) {
			//	throw new Exception("Pending amount should be 0.\r\n");
			//}
			Assert.AreEqual(0,pending);
			//return "31: Passed.  Limitations override more general limitations for pending insurance.\r\n";
		}

		[TestMethod]
		public void Legacy_TestThirtyTwo() {
			//if(specificTest != 0 && specificTest !=32) {
			//	return "";
			//}
			string suffix="32";
			DateTime startDate=DateTime.Parse("2001-01-01");
			Employee emp = EmployeeT.CreateEmployee(suffix);
			PayPeriod payP1 = PayPeriodT.CreateTwoWeekPayPeriodIfNotExists(startDate);
			PayPeriods.RefreshCache();
			Prefs.UpdateInt(PrefName.TimeCardOvertimeFirstDayOfWeek,0);
			Prefs.UpdateBool(PrefName.TimeCardsMakesAdjustmentsForOverBreaks,true);
			Prefs.RefreshCache();
			TimeCardRuleT.CreatePMTimeRule(emp.EmployeeNum,TimeSpan.FromHours(16));
			TimeCardRules.RefreshCache();
			long clockEvent1 = ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddHours(8),startDate.AddHours(16).AddMinutes(40),0);
			ClockEventT.InsertBreak(emp.EmployeeNum,startDate.AddHours(11),40,0);
			TimeCardRules.CalculateDailyOvertime(emp,payP1.DateStart,payP1.DateStop);
			//Validate
			string retVal="";
			//Check
			//if(ClockEvents.GetOne(clockEvent1).AdjustAuto!=TimeSpan.FromMinutes(-10)) {
			//	throw new Exception("Clock adjustment should be -10 minutes, instead it is " + ClockEvents.GetOne(clockEvent1).AdjustAuto.TotalMinutes + " minutes.\r\n");
			//}
			Assert.AreEqual(ClockEvents.GetOne(clockEvent1).AdjustAuto,TimeSpan.FromMinutes(-10));
			//if(ClockEvents.GetOne(clockEvent1).Rate2Auto!=TimeSpan.FromMinutes(40)) {
			//	throw new Exception("Clock differential should be 40 minutes, instead it is " + ClockEvents.GetOne(clockEvent1).Rate2Auto.TotalMinutes + " minutes.\r\n");
			//}
			Assert.AreEqual(ClockEvents.GetOne(clockEvent1).Rate2Auto,TimeSpan.FromMinutes(40));
			retVal+="32: Passed. Differential calculated properly after time of day.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestThirtyThree() {
			//if(specificTest != 0 && specificTest !=33) {
			//	return "";
			//}
			string suffix="33";
			DateTime startDate=DateTime.Parse("2001-01-01");
			Employee emp = EmployeeT.CreateEmployee(suffix);
			PayPeriod payP1 = PayPeriodT.CreateTwoWeekPayPeriodIfNotExists(startDate);
			PayPeriods.RefreshCache();
			Prefs.UpdateInt(PrefName.TimeCardOvertimeFirstDayOfWeek,0);
			Prefs.UpdateBool(PrefName.TimeCardsMakesAdjustmentsForOverBreaks,true);
			TimeCardRuleT.CreateAMTimeRule(emp.EmployeeNum,TimeSpan.FromHours(7.5));
			TimeCardRules.RefreshCache();
			long clockEvent1 = ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddHours(6),startDate.AddHours(16),0);
			ClockEventT.InsertBreak(emp.EmployeeNum,startDate.AddHours(11),40,0);
			TimeCardRules.CalculateDailyOvertime(emp,payP1.DateStart,payP1.DateStop);
			//Validate
			string retVal="";
			//Check
			//if(ClockEvents.GetOne(clockEvent1).AdjustAuto!=TimeSpan.FromMinutes(-10)) {
			//	throw new Exception("Clock adjustment should be -10 minutes, instead it is " + ClockEvents.GetOne(clockEvent1).AdjustAuto.TotalMinutes + " minutes.\r\n");
			//}
			Assert.AreEqual(ClockEvents.GetOne(clockEvent1).AdjustAuto,TimeSpan.FromMinutes(-10));
			//if(ClockEvents.GetOne(clockEvent1).Rate2Auto!=TimeSpan.FromMinutes(90)) {
			//	throw new Exception("Clock differential should be 90 minutes, instead it is " + ClockEvents.GetOne(clockEvent1).Rate2Auto.TotalMinutes + " minutes.\r\n");
			//}
			Assert.AreEqual(ClockEvents.GetOne(clockEvent1).Rate2Auto,TimeSpan.FromMinutes(90));
			retVal+="33: Passed. Differential calculated properly before time of day.\r\n";
			//return retVal;
		}

		///<summary>Validates that procedure specific deductibles take general deductibles into consideration.</summary>
		[TestMethod]
		public void Legacy_TestThirtyFour() {
			//if(specificTest != 0 && specificTest !=34) {
			//	return "";
			//}
			string suffix="34";
			Patient pat=PatientT.CreatePatient(suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			long subNum=sub.InsSubNum;
			BenefitT.CreateAnnualMax(plan.PlanNum,1000);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Diagnostic,100);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.RoutinePreventive,100);
			BenefitT.CreateDeductible(plan.PlanNum,EbenefitCategory.RoutinePreventive,0);
			BenefitT.CreateDeductibleGeneral(plan.PlanNum,BenefitCoverageLevel.Individual,50);
			BenefitT.CreateDeductible(plan.PlanNum,"D1351",50);
			PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			//proc1 - PerExam
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"",45);
			//proc2 - Sealant
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D1351",ProcStat.TP,"5",54);
			//Lists:
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<Procedure> ProcList=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP=Procedures.GetListTPandTPi(ProcList);//sorted by priority, then toothnum
			//Attach to claim
			ClaimProcT.AddInsPaid(pat.PatNum,plan.PlanNum,proc1.ProcNum,0,subNum,45,0);
			//Validate
			string retVal="";
			histList=ClaimProcs.GetHistList(pat.PatNum,benefitList,patPlans,planList,DateTime.Today,subList);
			Procedures.ComputeEstimates(proc2,pat.PatNum,ref claimProcs,false,planList,patPlans,benefitList,
				histList,loopList,false,pat.Age,subList);
			//save changes in the list to the database
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			ClaimProc claimProc=ClaimProcs.GetEstimate(claimProcs,proc2.ProcNum,plan.PlanNum,subNum);
			//if(claimProc.DedEst!=5) {//Second procedure should show deductible of 5.
			//	throw new Exception("Should be 5. \r\n");
			//}
			Assert.AreEqual(5,claimProc.DedEst);
			retVal+="34: Passed.  General deductibles are taken into consideration when computing procedure specific deductibles.\r\n";
			//return retVal;
		}

		///<summary>Validates that insurance plan deductible adjustments only count towards the None or General deductibles.</summary>
		[TestMethod]
		public void Legacy_TestThirtyFive() {
			//if(specificTest != 0 && specificTest !=35) {
			//	return "";
			//}
			string suffix="35";
			string retVal="";
			Patient pat=PatientT.CreatePatient(suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			long subNum=sub.InsSubNum;
			BenefitT.CreateAnnualMax(plan.PlanNum,1000);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Diagnostic,100);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.RoutinePreventive,100);
			BenefitT.CreateDeductible(plan.PlanNum,EbenefitCategory.RoutinePreventive,50);
			//There are two "general" deductibles here because the Category General and the BenCat of 0 are not the same and need to be tested seperately.
			BenefitT.CreateDeductible(plan.PlanNum,EbenefitCategory.General,50);
			BenefitT.CreateDeductibleGeneral(plan.PlanNum,BenefitCoverageLevel.Individual,50);
			PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			//proc1 - PerExam
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0120",ProcStat.TP,"",200);
			//proc2 - Sealant
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D1351",ProcStat.TP,"5",200);
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			//Add insurance adjustment of $150 deductible.  This allows us to check that proc2's deductible amount didn't get removed.
			ClaimProcT.AddInsUsedAdjustment(pat.PatNum,plan.PlanNum,0,sub.InsSubNum,150);
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<Procedure> ProcList=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP=Procedures.GetListTPandTPi(ProcList);//sorted by priority, then toothnum
			for(int i=0;i<ProcListTP.Length;i++) {
				Procedures.ComputeEstimates(ProcListTP[i],pat.PatNum,ref claimProcs,false,planList,patPlans,benefitList,
					histList,loopList,false,pat.Age,subList);
				//then, add this information to loopList so that the next procedure is aware of it.
				loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs,ProcListTP[i].ProcNum,ProcListTP[i].CodeNum));
			}
			//Save changes in the list to the database, just like the TP module does when loaded.
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			//Validate the estimates within the Edit Claim Proc window are correct when opened from inside of the TP module by passing in same histlist and loop list that the TP module would.
			histList=ClaimProcs.GetHistList(pat.PatNum,benefitList,patPlans,planList,DateTime.Today,subList);//The history list is fetched when the TP module is loaded and is passed in the same for all claimprocs.
			loopList=new List<ClaimProcHist>();//Always empty for the first claimproc.
			ClaimProc claimProc1=ClaimProcs.GetEstimate(claimProcs,proc1.ProcNum,plan.PlanNum,subNum);
			FormClaimProc formCP1=new FormClaimProc(claimProc1,proc1,fam,pat,planList,histList,ref loopList,patPlans,false,subList);
			formCP1.Initialize();
			string dedEst1=formCP1.GetTextValue("textDedEst");
			//if(dedEst1!="0.00") {
			//	throw new Exception("Deductible estimates in Treatment Plan Procedure Grid and Claim Proc Edit Window are $"+dedEst1+" but should be $0.00 for proc1 from TP module. \r\n");
			//}
			Assert.AreEqual("0.00",dedEst1);
			ClaimProc claimProc2=ClaimProcs.GetEstimate(claimProcs,proc2.ProcNum,plan.PlanNum,subNum);
			histList=ClaimProcs.GetHistList(pat.PatNum,benefitList,patPlans,planList,DateTime.Today,subList);//The history list is fetched when the TP module is loaded and is passed in the same for all claimprocs.
			loopList=new List<ClaimProcHist>();
			loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs,proc1.ProcNum,proc1.CodeNum));
			FormClaimProc formCP2=new FormClaimProc(claimProc2,proc2,fam,pat,planList,histList,ref loopList,patPlans,false,subList);
			formCP2.Initialize();
			string dedEst2=formCP2.GetTextValue("textDedEst");
			//if(dedEst2!="50.00") {
			//	throw new Exception("Deductible estimates in Treatment Plan Procedure Grid and Claim Proc Edit Window are $"+dedEst2+" but should be $50.00 for proc2 from TP module. \r\n");
			//}
			Assert.AreEqual("50.00",dedEst2);
			retVal+="35: Passed.  Insurance adjustments only apply to None and General deductibles.\r\n";
			//return retVal;
		}

		///<summary>Similar to tests 1 and 2.</summary>
		[TestMethod]
		public void Legacy_TestThirtySix() {
			//if(specificTest != 0 && specificTest != 36) {
			//	return "";
			//}
			string suffix="36";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			long feeSchedNum1=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix);
			long feeSchedNum2=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix+"b");
			//Standard Fee (we only insert this value to test that it is not used in the calculations).
			FeeT.RefreshCache();
			long codeNum=ProcedureCodes.GetCodeNum("D4341");
			Fee fee=Fees.GetFee(codeNum,53,0,0);
			if(fee==null) {
				fee=new Fee();
				fee.CodeNum=codeNum;
				fee.FeeSched=53;
				fee.Amount=1200;
				Fees.Insert(fee);
			}
			else {
				fee.Amount=1200;
				Fees.Update(fee);
			}
			//PPO fees
			fee=new Fee();
			fee.CodeNum=codeNum;
			fee.FeeSched=feeSchedNum1;
			fee.Amount=206;
			Fees.Insert(fee);
			fee=new Fee();
			fee.CodeNum=codeNum;
			fee.FeeSched=feeSchedNum2;
			fee.Amount=117;
			Fees.Insert(fee);
			FeeT.RefreshCache();
			//Carrier
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			long planNum1=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedNum1,EnumCobRule.Standard).PlanNum;
			long planNum2=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedNum2,EnumCobRule.Standard).PlanNum;
			InsSub sub1=InsSubT.CreateInsSub(pat.PatNum,planNum1);
			long subNum1=sub1.InsSubNum;
			InsSub sub2=InsSubT.CreateInsSub(pat.PatNum,planNum2);
			long subNum2=sub2.InsSubNum;
			BenefitT.CreateCategoryPercent(planNum1,EbenefitCategory.Periodontics,50);
			BenefitT.CreateCategoryPercent(planNum2,EbenefitCategory.Periodontics,80);
			PatPlanT.CreatePatPlan(1,patNum,subNum1);
			PatPlanT.CreatePatPlan(2,patNum,subNum2);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D4341",ProcStat.TP,"",206);//Scaling in undefined/any quadrant.
			long procNum=proc.ProcNum;
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(patNum);
			Family fam=Patients.GetFamily(patNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(patNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			//Validate
			string retVal="";
			ClaimProc claimProc;
			//if(specificTest==0 || specificTest==36) {
			Procedures.ComputeEstimates(proc,patNum,ref claimProcs,false,planList,patPlans,benefitList,histList,loopList,true,pat.Age,subList);
			claimProcs=ClaimProcs.Refresh(patNum);
			claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum1,subNum1);
			//I don't think allowed can be easily tested on the fly, and it's not that important.
			//if(claimProc.InsEstTotal!=103) {
			//	throw new Exception("Primary total estimate should be 103. \r\n");
			//}
			Assert.AreEqual(103,claimProc.InsEstTotal);
			//if(claimProc.WriteOffEst!=0) {
			//	throw new Exception("Primary writeoff estimate should be 0. \r\n");
			//}
			Assert.AreEqual(0,claimProc.WriteOffEst);
			claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum2,subNum2);
			//if(claimProc.InsEstTotal!=93.6) {
			//	throw new Exception("Secondary total estimate should be 93.60. \r\n");
			//}
			Assert.AreEqual(93.6,claimProc.InsEstTotal);
			//if(claimProc.WriteOffEst!=0) {
			//	throw new Exception("Secondary writeoff estimate should be 0. \r\n");
			//}
			Assert.AreEqual(0,claimProc.WriteOffEst);
			retVal+="36: Passed.  Claim proc estimates for dual PPO ins when primary writeoff is zero.\r\n";
			//}
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestThirtySeven() {
			//if(specificTest != 0 && specificTest != 37) {
			//	return "";
			//}
			string suffix="37";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			long feeSchedNum1=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix);
			//Standard Fee (we only insert this value to test that it is not used in the calculations).
			FeeT.RefreshCache();
			long codeNum=ProcedureCodes.GetCodeNum("D0270");//1BW
			//PPO fee
			Fee fee=new Fee();
			fee.CodeNum=codeNum;
			fee.FeeSched=feeSchedNum1;
			fee.Amount=40;
			Fees.Insert(fee);
			FeeT.RefreshCache();
			//Copay fee schedule
			long feeSchedNumCopay=FeeSchedT.CreateFeeSched(FeeScheduleType.CoPay,suffix);
			fee=new Fee();
			fee.CodeNum=codeNum;
			fee.FeeSched=feeSchedNumCopay;
			fee.Amount=5;
			Fees.Insert(fee);
			FeeT.RefreshCache();
			//Carrier
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			long planNum1=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedNum1,EnumCobRule.Basic).PlanNum;
			BenefitT.CreateDeductibleGeneral(planNum1,BenefitCoverageLevel.Individual,10);
			InsSub sub1=InsSubT.CreateInsSub(pat.PatNum,planNum1);
			long subNum1=sub1.InsSubNum;
			BenefitT.CreateCategoryPercent(planNum1,EbenefitCategory.DiagnosticXRay,80);
			PatPlanT.CreatePatPlan(1,patNum,subNum1);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0270",ProcStat.C,"",50);//1BW
			Procedure procOld=proc.Copy();
			proc.UnitQty=3;
			Procedures.Update(proc,procOld);//1BW x 3
			long procNum=proc.ProcNum;
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(patNum);
			Family fam=Patients.GetFamily(patNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			InsPlan insPlan1=InsPlans.GetPlan(planNum1,planList);
			InsPlan planOld = insPlan1.Copy();
			insPlan1.CopayFeeSched=feeSchedNumCopay;
			InsPlans.Update(insPlan1,planOld);
			List<PatPlan> patPlans=PatPlans.Refresh(patNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			//Validate
			string retVal="";
			ClaimProc claimProc;
			//if(specificTest==0 || specificTest==37) {
			Procedures.ComputeEstimates(proc,patNum,ref claimProcs,false,planList,patPlans,benefitList,histList,loopList,true,pat.Age,subList);
			claimProcs=ClaimProcs.Refresh(patNum);
			claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum1,subNum1);
			//if(claimProc.InsEstTotal!=76) {
			//	throw new Exception("Primary total estimate should be 76.\r\n");
			//}
			Assert.AreEqual(76,claimProc.InsEstTotal);
			//if(claimProc.WriteOffEst!=30) {
			//	throw new Exception("Primary writeoff estimate should be 30.\r\n");
			//}
			Assert.AreEqual(30,claimProc.WriteOffEst);
			retVal+="37: Passed.  PPO insurance estimates for procedures with multiple units.\r\n";
			//}
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestThirtyEight() {
			//if(specificTest != 0 && specificTest != 38) {
			//	return "";
			//}
			string suffix="38";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			//Carrier
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			long planNum1=InsPlanT.CreateInsPlan(carrier.CarrierNum).PlanNum;
			InsSub sub1=InsSubT.CreateInsSub(pat.PatNum,planNum1);
			long subNum1=sub1.InsSubNum;
			BenefitT.CreateCategoryPercent(planNum1,EbenefitCategory.DiagnosticXRay,80);
			PatPlanT.CreatePatPlan(1,patNum,subNum1);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0270",ProcStat.C,"",50);//1BW
			Procedure procOld=proc.Copy();
			proc.UnitQty=2;
			Procedures.Update(proc,procOld);//1BW x 2
			long procNum=proc.ProcNum;
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(patNum);
			Family fam=Patients.GetFamily(patNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(patNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			//Validate
			string retVal="";
			ClaimProc claimProc;
			//if(specificTest==0 || specificTest==38) {
			Procedures.ComputeEstimates(proc,patNum,ref claimProcs,false,planList,patPlans,benefitList,histList,loopList,true,pat.Age,subList);
			claimProcs=ClaimProcs.Refresh(patNum);
			claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum1,subNum1);
			if(claimProc.InsEstTotal!=80) {
				throw new Exception("Primary total estimate should be 80.\r\n");
			}
			Assert.AreEqual(80,claimProc.InsEstTotal);
			retVal+="38: Passed.  Category percentage insurance estimates for procedures with multiple units.\r\n";
			//}
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestThirtyNine() {
			//if(specificTest != 0 && specificTest != 39) {
			//	return "";
			//}
			string suffix="39";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			long feeSchedNum1=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix);
			long feeSchedNum2=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix+"b");
			//Standard Fee (we only insert this value to test that it is not used in the calculations).
			FeeT.RefreshCache();
			long codeNum=ProcedureCodes.GetCodeNum("D0270");
			//PPO fees
			Fee fee=new Fee();
			fee.CodeNum=codeNum;
			fee.FeeSched=feeSchedNum1;
			fee.Amount=40;
			Fees.Insert(fee);
			fee=new Fee();
			fee.CodeNum=codeNum;
			fee.FeeSched=feeSchedNum2;
			fee.Amount=30;
			Fees.Insert(fee);
			FeeT.RefreshCache();
			//Carrier
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			long planNum1=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedNum1,EnumCobRule.Basic).PlanNum;
			long planNum2=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedNum2,EnumCobRule.Basic).PlanNum;
			InsSub sub1=InsSubT.CreateInsSub(pat.PatNum,planNum1);
			long subNum1=sub1.InsSubNum;
			InsSub sub2=InsSubT.CreateInsSub(pat.PatNum,planNum2);
			long subNum2=sub2.InsSubNum;
			BenefitT.CreateCategoryPercent(planNum1,EbenefitCategory.DiagnosticXRay,80);
			BenefitT.CreateCategoryPercent(planNum2,EbenefitCategory.DiagnosticXRay,80);
			PatPlanT.CreatePatPlan(1,patNum,subNum1);
			PatPlanT.CreatePatPlan(2,patNum,subNum2);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0270",ProcStat.TP,"",50);//Scaling in undefined/any quadrant.
			Procedure procOld=proc.Copy();
			proc.UnitQty=4;
			Procedures.Update(proc,procOld);//1BW x 4
			long procNum=proc.ProcNum;
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(patNum);
			Family fam=Patients.GetFamily(patNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(patNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			//Validate
			string retVal="";
			ClaimProc claimProc;
			//if(specificTest==0 || specificTest==39) {
			Procedures.ComputeEstimates(proc,patNum,ref claimProcs,false,planList,patPlans,benefitList,histList,loopList,true,pat.Age,subList);
			claimProcs=ClaimProcs.Refresh(patNum);
			claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum1,subNum1);
			//if(claimProc.InsEstTotal!=128) {
			//	throw new Exception("Primary total estimate should be 128. \r\n");
			//}
			Assert.AreEqual(128,claimProc.InsEstTotal);
			//if(claimProc.WriteOffEst!=40) {
			//	throw new Exception("Primary writeoff estimate should be 40. \r\n");
			//}
			Assert.AreEqual(40,claimProc.WriteOffEst);
			claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum2,subNum2);
			//if(claimProc.InsEstTotal!=0) {
			//	throw new Exception("Secondary total estimate should be 0. \r\n");
			//}
			Assert.AreEqual(0,claimProc.InsEstTotal);
			//if(claimProc.WriteOffEst!=0) {
			//	throw new Exception("Secondary writeoff estimate should be 0. \r\n");
			//}
			Assert.AreEqual(0,claimProc.WriteOffEst);
			retVal+="39: Passed.  Claim proc writeoff estimates for procedures with multiple units.\r\n";
			//}
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestFourty() {
			//if(specificTest != 0 && specificTest != 40) {
			//	return "";
			//}
			string suffix="40";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			long feeSchedAllowedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.OutNetwork,suffix+"-allowed");
			long feeSchedNum2=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix+"b");
			//Standard Fee (we only insert this value to test that it is not used in the calculations).
			FeeT.RefreshCache();
			long codeNum=ProcedureCodes.GetCodeNum("D0272");
			Fee fee=Fees.GetFee(codeNum,feeSchedAllowedNum,0,0);
			if(fee==null) {
				fee=new Fee();
				fee.CodeNum=codeNum;
				fee.FeeSched=feeSchedAllowedNum;
				fee.Amount=152;
				Fees.Insert(fee);
			}
			else {
				fee.Amount=152;
				Fees.Update(fee);
			}
			//PPO fees
			fee=new Fee();
			fee.CodeNum=codeNum;
			fee.FeeSched=feeSchedNum2;
			fee.Amount=87.99;
			Fees.Insert(fee);
			FeeT.RefreshCache();
			//Carrier
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			//Plan 1 - Category Percentage
			long planNum1=InsPlanT.CreateInsPlan(carrier.CarrierNum).PlanNum;
			InsPlan insPlan1=InsPlans.RefreshOne(planNum1);
			InsPlan planOld = insPlan1.Copy();
			insPlan1.FeeSched=0;
			insPlan1.AllowedFeeSched=feeSchedAllowedNum;
			InsPlans.Update(insPlan1,planOld);
			InsSub sub1=InsSubT.CreateInsSub(pat.PatNum,planNum1);
			long subNum1=sub1.InsSubNum;
			BenefitT.CreateCategoryPercent(planNum1,EbenefitCategory.DiagnosticXRay,80);
			BenefitT.CreateDeductibleGeneral(planNum1,BenefitCoverageLevel.Individual,50);
			PatPlanT.CreatePatPlan(1,patNum,subNum1);
			//Plan 2 - PPO
			long planNum2=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedNum2,EnumCobRule.Basic).PlanNum;
			InsSub sub2=InsSubT.CreateInsSub(pat.PatNum,planNum2);
			long subNum2=sub2.InsSubNum;
			BenefitT.CreateCategoryPercent(planNum2,EbenefitCategory.DiagnosticXRay,100);
			PatPlanT.CreatePatPlan(2,patNum,subNum2);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0272",ProcStat.TP,"",236);
			long procNum=proc.ProcNum;
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(patNum);
			Family fam=Patients.GetFamily(patNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(patNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			//Validate
			string retVal="";
			ClaimProc claimProc;
			//if(specificTest==0 || specificTest==40) {
			Procedures.ComputeEstimates(proc,patNum,ref claimProcs,false,planList,patPlans,benefitList,histList,loopList,true,pat.Age,subList);
			claimProcs=ClaimProcs.Refresh(patNum);
			claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum1,subNum1);
			//Test insurance numbers without calculating secondary PPO insurance writeoffs
			//if(claimProc.InsEstTotal!=81.6) {
			//	throw new Exception("Primary total estimate should be 81.60. \r\n");
			//}
			Assert.AreEqual(81.6,claimProc.InsEstTotal);
			//if(claimProc.WriteOffEst!=-1) {
			//	throw new Exception("Primary writeoff estimate should be -1. \r\n");
			//}
			Assert.AreEqual(-1,claimProc.WriteOffEst);
			claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum2,subNum2);
			//if(claimProc.InsEstTotal!=6.39) {
			//	throw new Exception("Secondary total estimate should be 6.39. \r\n");
			//}
			Assert.AreEqual(6.39,claimProc.InsEstTotal);
			//if(claimProc.WriteOffEst!=0) {
			//	throw new Exception("Secondary writeoff estimate should be 0. \r\n");
			//}
			Assert.AreEqual(0,claimProc.WriteOffEst);
			//Now test insurance numbers with calculating secondary PPO insurance writeoffs
			Prefs.UpdateBool(PrefName.InsPPOsecWriteoffs,true);
			Procedures.ComputeEstimates(proc,patNum,ref claimProcs,false,planList,patPlans,benefitList,histList,loopList,true,pat.Age,subList);
			claimProcs=ClaimProcs.Refresh(patNum);
			claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum1,subNum1);
			//if(claimProc.InsEstTotal!=81.6) {
			//	throw new Exception("Primary total estimate should be 81.60. \r\n");
			//}
			Assert.AreEqual(81.6,claimProc.InsEstTotal);
			//if(claimProc.WriteOffEst!=-1) {
			//	throw new Exception("Primary writeoff estimate should be -1. \r\n");
			//}
			Assert.AreEqual(-1,claimProc.WriteOffEst);
			claimProc=ClaimProcs.GetEstimate(claimProcs,procNum,planNum2,subNum2);
			//if(claimProc.InsEstTotal!=6.39) {
			//	throw new Exception("Secondary total estimate should be 6.39. \r\n");
			//}
			Assert.AreEqual(6.39,claimProc.InsEstTotal);
			//if(claimProc.WriteOffEst!=148.01) {
			//	throw new Exception("Secondary writeoff estimate should be 148.01. \r\n");
			//}
			Assert.AreEqual(148.01,claimProc.WriteOffEst);
			retVal+="40: Passed.  Dual insurance with secondary PPO insurance writeoffs calculated based on preference.\r\n";
			//}
			Prefs.UpdateBool(PrefName.InsPPOsecWriteoffs,false);
			//return retVal;
		}

		//Tests 41-48 moved to PaymentsTests

		[TestMethod]
		public void Legacy_TestFourtyNine() {
			//if(specificTest != 0 && specificTest !=49) {
			//	return "";
			//}
			string suffix="49";
			Patient pat=PatientT.CreatePatient(suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			long subNum=sub.InsSubNum;
			BenefitT.CreateAnnualMaxFamily(plan.PlanNum,400);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Restorative,100);
			PatPlan pplan=PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			//procs - D2140 (amalgum fillings)
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D2140",ProcStat.TP,"18",500);
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<Procedure> ProcList=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP=Procedures.GetListTPandTPi(ProcList);//sorted by priority, then toothnum
			//Validate
			string retVal="";
			for(int i=0;i<ProcListTP.Length;i++) {
				Procedures.ComputeEstimates(ProcListTP[i],pat.PatNum,ref claimProcs,false,planList,patPlans,benefitList,
					histList,loopList,false,pat.Age,subList);
				//then, add this information to loopList so that the next procedure is aware of it.
				loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs,ProcListTP[i].ProcNum,ProcListTP[i].CodeNum));
			}
			//save changes in the list to the database
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			//change override
			claimProcs[0].InsEstTotalOverride=399;
			ClaimProcs.Update(claimProcs[0]);
			//Lists2
			List<ClaimProc> claimProcs2=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld2=ClaimProcs.Refresh(pat.PatNum);
			Family fam2=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList2=InsSubs.RefreshForFam(fam2);
			List<InsPlan> planList2=InsPlans.RefreshForSubList(subList2);
			List<PatPlan> patPlans2=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList2=Benefits.Refresh(patPlans2,subList2);
			List<ClaimProcHist> histList2=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList2=new List<ClaimProcHist>();
			List<Procedure> ProcList2=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP2=Procedures.GetListTPandTPi(ProcList2);//sorted by priority, then toothnum
			//Validate again
			for(int i=0;i<ProcListTP.Length;i++) {
				Procedures.ComputeEstimates(ProcListTP2[i],pat.PatNum,ref claimProcs2,false,planList2,patPlans2,benefitList2,
					histList2,loopList2,false,pat.Age,subList2);
				//then, add this information to loopList so that the next procedure is aware of it.
				loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs2,ProcListTP2[i].ProcNum,ProcListTP2[i].CodeNum));
			}
			ClaimProcs.Synch(ref claimProcs2,claimProcListOld2);
			claimProcs2=ClaimProcs.Refresh(pat.PatNum);
			//Check to see if note still says over annual max
			//if(claimProcs2[0].EstimateNote!="") {//The override should be under the family annual max of 400 so no there should be no EstimateNote.
			//	throw new Exception("Claimproc's EstimateNote was "+claimProcs2[0].EstimateNote+", should be blank.\r\n");
			//}
			Assert.AreEqual("",claimProcs2[0].EstimateNote);
			retVal+="49: Passed.  Insurance estimate with override under family max had blank EstimateNote.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestFifty() {
			//if(specificTest != 0 && specificTest !=50) {
			//	return "";
			//}
			string suffix="50";
			Patient pat=PatientT.CreatePatient(suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			long subNum=sub.InsSubNum;
			BenefitT.CreateAnnualMaxFamily(plan.PlanNum,400);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Restorative,100);
			PatPlan pplan=PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			//procs - D2140 (amalgum fillings)
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D2140",ProcStat.TP,"18",500);
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<Procedure> ProcList=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP=Procedures.GetListTPandTPi(ProcList);//sorted by priority, then toothnum
			//Validate
			string retVal="";
			for(int i=0;i<ProcListTP.Length;i++) {
				Procedures.ComputeEstimates(ProcListTP[i],pat.PatNum,ref claimProcs,false,planList,patPlans,benefitList,
					histList,loopList,false,pat.Age,subList);
				//then, add this information to loopList so that the next procedure is aware of it.
				loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs,ProcListTP[i].ProcNum,ProcListTP[i].CodeNum));
			}
			//save changes in the list to the database
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			//change override
			claimProcs[0].InsEstTotalOverride=401;
			ClaimProcs.Update(claimProcs[0]);
			//Lists2
			List<ClaimProc> claimProcs2=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld2=ClaimProcs.Refresh(pat.PatNum);
			Family fam2=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList2=InsSubs.RefreshForFam(fam2);
			List<InsPlan> planList2=InsPlans.RefreshForSubList(subList2);
			List<PatPlan> patPlans2=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList2=Benefits.Refresh(patPlans2,subList2);
			List<ClaimProcHist> histList2=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList2=new List<ClaimProcHist>();
			List<Procedure> ProcList2=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP2=Procedures.GetListTPandTPi(ProcList2);//sorted by priority, then toothnum
			//Validate again
			for(int i=0;i<ProcListTP.Length;i++) {
				Procedures.ComputeEstimates(ProcListTP2[i],pat.PatNum,ref claimProcs2,false,planList2,patPlans2,benefitList2,
					histList2,loopList2,false,pat.Age,subList2);
				//then, add this information to loopList so that the next procedure is aware of it.
				loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs2,ProcListTP2[i].ProcNum,ProcListTP2[i].CodeNum));
			}
			ClaimProcs.Synch(ref claimProcs2,claimProcListOld2);
			claimProcs2=ClaimProcs.Refresh(pat.PatNum);
			//Check to see if note still says over annual max
			//if(claimProcs2[0].EstimateNote!="Over family max") {//The override should be under the family annual max of 400 so no there should be no remarks.
			//	throw new Exception("Claimproc's EstimateNote was "+claimProcs2[0].EstimateNote+", should be \"Over family max\".\r\n");
			//}
			Assert.AreEqual("Over family max",claimProcs2[0].EstimateNote);
			retVal+="50: Passed.  Insurance estimate with override over family max showed \"over family max\" EstimateNote.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestFiftyOne() {
			//if(specificTest!=0 && specificTest!=51) {
			//	return "";
			//}
			string suffix="51";
			Patient pat=PatientT.CreatePatient(suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsPlan planOld = plan.Copy();
			plan.IsMedical=true;
			InsPlans.Update(plan,planOld);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			long subNum=sub.InsSubNum;
			BenefitT.CreateDeductibleGeneral(plan.PlanNum,BenefitCoverageLevel.None,50.00);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.OralSurgery,80);
			PatPlan pplan=PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			//procs - D7140 (extraction)
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D7140",ProcStat.TP,"18",500);
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<Procedure> ProcList=Procedures.Refresh(pat.PatNum);
			string retVal="";
			Procedures.ComputeEstimates(ProcList[0],pat.PatNum,ref claimProcs,false,planList,patPlans,benefitList,
				histList,loopList,false,pat.Age,subList);
			//save changes in the list to the database
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			Claim ClaimCur=ClaimT.CreateClaim("Med",patPlans,planList,claimProcs,ProcList,pat,ProcList,benefitList,subList);
			List<ClaimProc> ClaimProcList=ClaimProcs.Refresh(pat.PatNum);
			ClaimCur.ClaimStatus="W";
			ClaimCur.DateSent=DateTimeOD.Today;
			Claims.CalculateAndUpdate(ProcList,planList,ClaimCur,patPlans,benefitList,pat.Age,subList);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			//Check to see if deductible applied correctly
			//if(claimProcs[0].DedApplied!=50.00) {//The deductible applied should be $50.00
			//	throw new Exception("Claimproc's DedApplied was "+claimProcs[0].DedApplied+", should be $50.00.\r\n");
			//}
			Assert.AreEqual(50.00,claimProcs[0].DedApplied);
			retVal+="51: Passed.  Medical insurance estimate deductible applied calculated correctly.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestFiftyTwo() {
			//if(specificTest!=0 && specificTest!=52) {
			//	return "";
			//}
			string suffix = "52";
			Patient pat = PatientT.CreatePatient(suffix);
			List<int> listFailedTests = new List<int>();
			OpenDental.UI.SignatureBoxWrapper sbw = new OpenDental.UI.SignatureBoxWrapper();
			//1: Normal signature with \r\n
			sbw.FillSignature(false,"15.4 Normal\r\n15.4 Normal\r\n15.4 Normal3","AgsiXrWoTDupa0Nz19AoZ/HpRGAW+oJIvoAWqvJFFCPiLbBMa14AcopOYoS1OhIg0v5jQI9epUODrXtLElhyngtKMcnnvWZ9CqD3Jzolw7wik38VmpiWUSJiKUYMCzXEGQdkGnv6Sfa20I7XlYzlRhgRFizd5CuV6b/iuReK9PYj21gG0MxWq8r0f01BxkxSxrGO7kM0xnScd2MXcPx2y0sdXpo4fGOcFf9HPONca0YR3ihhloTNw9uPyvtSXUE0jjHSuE/XQBs/6za6T7FwnlFGlvJAq8AxvPKd2sPF7Li+ypa5Tb9mHhye6neMzsyoZHflDp9r7FipFrMJOiDvDTfThg4gU6KR5sTObNVBjB+uZClBJSRkNux9Q49nTUQlBnLEBPZreIkwl68bi6CW8o2cz4tJTQXUcCmzH4yxvo6DzAXSU4swPK67Lo9l0i+ZbeyicSmi77H+OVJb7w9e06PodWWYr9HpnuwwEvGtSUHyH3YatWF1/nXAWaNlwlsGSu0TzXVpGzCANzw/CmOtbYnNqyRvKTHY5pggs4Y/OFWtyMklSezdIpP/VPfLhTLcEuF15ybxi7z9GhjiSgu23T43LKu5D2E2g5L8AMkzgdk5oEgfMxPT1wxL4FYSXeGCHySNq5RXJvVw9sixv340hr4htZSYzJLXbCNy5EjEZbKNv7l9AQ7qlzIExQ9GZ8X6vEIK+Hdscpl7gtpBRd9UJlIe7maCcqp5QD8NtIERcJ1zgxcun/VgtLVFxyEuaC1SXbBbJLgQYIhWRfQCRmd8/DUojiRg9L70J53kPHsEQBppgSLoNzxoUVfkLK74HG/hYgTa9ss703LsRgm3jBAIFj1EAmfno4WG55s9QHFoiLGTWMqgLXsMz2izL9S9Szss6p15dkw6q+xW7TGnOnCsiPlovx5TYKWgpd1Z/BjVu5ArF0mzTatg+NUy7oHA0GUmbPNTlzQkDvLAb8CK7O3O0+sdOtoBMIZZi3KutUKYYQo/f9Bfefyq3YmQWvXjEH3c+RavU2GV7C7GNigtkdQ1rUm/J6RCCnn/IotPH3MkU9xhhf4OTt6sL7Je4Rkz8hEQiNA/jfpu2X61GiUUgV/oYgEPu6dnlB13SjoTB5NQx4OugxadykF2c+y6TKE7xtAn+hbOuRST43J6FuHlCDrrBPQnVejvSZ+0WSNX0hnPr7c7/Z2pkkGaBG4IdwAx7dcDPr/2JxsKJobD6XQ97jqdbDOM8AcHc5f5J1H9Fe4c33F54Iv/QNixbyWQcM4y6GtXyuXiAt7hSZgvHH43cmY+2hZhDRdQAMxZ1fm/fa8VqugSAGocky1Rt2TP4kL66RFGP+3UcsC3Y67Ek1yMcOMNYr9Gn2HIMkXUDmeAYbk3Bllh+Z05NTc+ooJXl5bDzp+ZHce3wxHvXHbXYAT1hq8oTBGBf5farbDwhh5XGtw5aahpvNrDdQzjYtSM+noFmfirtW3SplfCvHxXJY5+6Ow0VOwtX6vTPHz41JVv4IObAl8yOPdap8igVMzaWQGpcdacAbU+Bge0gLT0n/fripC+WlNBQQ7XVfvqdu+FECtfX7piaE6FTcKlP8S6Rz3mXdMM9NyXQLM8TJ84sHHZh3CXsHJKARoc+Z1ekDzRr7Eg9CJqFDvMFYvuLbCp+YaWQWh18tSXbUGm83qFYDCz/GfZWfQOZBvRX9syyO8Nu42hcwsGaLrKYiN+dUvRENr/naBmNo1o0WIStxTfBvk8iMuDqj0mAd17XBjGg/Qn2NyzjA7CLvTs+ocAYzvqAG1TfOHBaqD1K6d5bqzxRWTrjxVrMQU/L7988G4OIY8D63XQ02XtRcVWv46alUJMaETMg18ZjqGs70IxvrVVErxJMhXCQss7tHXp84wRGCNF5H+kwsUdQrl9ecHEy5cHHBR5iHz49m9RjM23VlWlhVgCXZWdiS9NRVsofv4LfU+YfquMkeBhAHoUG9rtLYBWD5oBpRqRtXm4G4M+S0HhFcht43YFKfBP1Jf+C6jPD7srPuyEoNXQBo/GGThl2+ER+a5bwMwwemudJktcl5wzzp9w3z3yQIiqsz+jAW+ARu5976JmHryI+OcN03AwWG//zfGUppxARR9Sy1QI/+kH22ypfwmhy3H++IoM5w7dS2FLbYYlzUQrx3SHXcD5uG0htbBwVvmzR0GiN7cZhejffH7VMG9HlG1nxbLJA4hwUeYWXo+jwNTORoDSLPRj/55PAiA6xiLEzcpMWVF367RvTw34bgJHHw==");
			if(sbw.GetNumberOfTabletPoints(false)==0) {
				listFailedTests.Add(1);
			}
			//2: Normal signature, middle tier with \r\n
			sbw.FillSignature(false,"15.4 Normal MiddleTier\r\n15.4 Normal MiddleTier\r\n15.4 Normal MiddleTier3","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");
			if(sbw.GetNumberOfTabletPoints(false)==0) {
				listFailedTests.Add(2);
			}
			//3: Topaz signature with \r\n
			sbw.FillSignature(true,"15.4 Topaz\r\n15.4 Topaz\r\n15.4 Topaz3","FFFFFFFFED1D7C6A37000000040000003E6A277C10525BF095781A4505E1A9814FB05B0B7930F49EDDE09B95B6743B4EF11B47C4E7DF151AA5C031BC4487067C897FA35C3E9A5FD9DAF32CEC3F9479C6CDB793CC5202BCE8714DDADDE228986E39440B31B76E940496E9F63EFFED1C3F896DE2B7EA5129CA08C9B22066C48CF27FEEBE00E107A3901498ABE9FA9F301FB028748CE0BBE01CC2A4628E94AF9854FFA4A15D60F3C488698D5F2F14379F99D764B27013B4FFB18336AA20B132DE18A85060544F5F92F4A259803FC2CA9384DF30B784703C80868C6F1D2EADCF55345F0380CC9A5F0C757594FF128F66E6F7BC6D655FF2592935EDB3CA52C642FC9C4A64B8623F43015F3810CC51A580081C50DC9FF1ADDFEF3FBD06E0C9E0DD47F1B87E1061C760099E6EA055A1533C6134700527723C62DDFD8B267D6CECC6399CCA149BA5B234B0A5EA44DF3223941DF7AFF6D4B5551366F71103B7B48EE7D2B6122B9495654DE257478FBED6680F4DE2A0C91EFFE5E95E785E7ADF0AED06BC384B69915DC4BA281980F9684AF119FB0B242EF4CCC82E3AE733C05F08933DD71E8D54F403C6C8A1A48D54F403C6C8A1A48D54F403C6C8A1A48D54F403C6C8A1A48D54F403C6C8A1A4");
			if(sbw.GetNumberOfTabletPoints(true)==0) {
				listFailedTests.Add(3);
			}
			//4: Topaz signature, middle tier with \r\n
			sbw.FillSignature(true,"15.4 Topaz MiddleTier\r\n15.4 Topaz MiddleTier\r\n15.4 Topaz MiddleTier3","FFFFFFFFAD1F4B693A00000000000000DEB4BBA8050C7F6E66DD8CC844BE7EEC6F3102C5E6B6F4977E482CE5F5E82ADCEEFFC822D77A6AB9529E824C363E0822ED65EBC841D98F55C7FF5CF269D8CBEF37B4E68BE56095D172699A8A58D8F71E366D6F65DA865F186454B4C9219D159147294001EBE30DF244B970B7B015C0FAEBC74BDDA61551412F6761D295D5C754A58B2B94781E067E7FEC2FEE0AAD6F0463D2C48F1EF21AD068CFE314CE2738949A01A19D29564D16A8C3D1CE462FEA754AD769CAFDEC057F9CF7AEB8BC68CDBDE965B83D75226294C44B14F4391BC07C32D908F409247E80C9C4C9DFE27C123459651FB5240C922841254DD77107DB1E6A5A525AFA10947BB1488D058C88E127DD6790A3F30A5013D9643AFDB03762875EA5D7D336EE6076B4AB7BD089872BD687300CF16AA32DC9606A91CD726E0244D91B760EA76C904B27180D271CCE952B80C4C65E39EBD2E41B84B7A7309AB3D953E673E788E063F624568F8C936B440254D832245B2DF3730066B0D32CBBD779AF1299037B3A14C72376D13B3B65E87D078FFB0FB98E2F53E6E02939A697AFAF585853EDB281D283765A15B1CE10060C4DC8A32D40884F8C4F5DEA3FA31345073F7C73434A9AE2913F7C73434A9AE2913F7C73434A9AE2913F7C73434A9AE291");
			if(sbw.GetNumberOfTabletPoints(true)==0) {
				listFailedTests.Add(4);
			}
			//5: Normal signature with \n
			sbw.FillSignature(false,"15.4 Normal\n15.4 Normal\n15.4 Normal3","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");
			if(sbw.GetNumberOfTabletPoints(false)==0) {
				listFailedTests.Add(5);
			}
			//6: Normal signature, middle tier with \n
			sbw.FillSignature(false,"15.4 Normal MiddleTier\n15.4 Normal MiddleTier\n15.4 Normal MiddleTier3","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");
			if(sbw.GetNumberOfTabletPoints(false)==0) {
				listFailedTests.Add(6);
			}
			//7: Topaz signature with \n
			sbw.FillSignature(true,"15.4 Topaz\n15.4 Topaz\n15.4 Topaz3","FFFFFFFFED1D7C6A37000000040000003E6A277C10525BF095781A4505E1A9814FB05B0B7930F49EDDE09B95B6743B4EF11B47C4E7DF151AA5C031BC4487067C897FA35C3E9A5FD9DAF32CEC3F9479C6CDB793CC5202BCE8714DDADDE228986E39440B31B76E940496E9F63EFFED1C3F896DE2B7EA5129CA08C9B22066C48CF27FEEBE00E107A3901498ABE9FA9F301FB028748CE0BBE01CC2A4628E94AF9854FFA4A15D60F3C488698D5F2F14379F99D764B27013B4FFB18336AA20B132DE18A85060544F5F92F4A259803FC2CA9384DF30B784703C80868C6F1D2EADCF55345F0380CC9A5F0C757594FF128F66E6F7BC6D655FF2592935EDB3CA52C642FC9C4A64B8623F43015F3810CC51A580081C50DC9FF1ADDFEF3FBD06E0C9E0DD47F1B87E1061C760099E6EA055A1533C6134700527723C62DDFD8B267D6CECC6399CCA149BA5B234B0A5EA44DF3223941DF7AFF6D4B5551366F71103B7B48EE7D2B6122B9495654DE257478FBED6680F4DE2A0C91EFFE5E95E785E7ADF0AED06BC384B69915DC4BA281980F9684AF119FB0B242EF4CCC82E3AE733C05F08933DD71E8D54F403C6C8A1A48D54F403C6C8A1A48D54F403C6C8A1A48D54F403C6C8A1A48D54F403C6C8A1A4");
			if(sbw.GetNumberOfTabletPoints(true)==0) {
				listFailedTests.Add(7);
			}
			//8: Topaz signature, middle tier with \n
			sbw.FillSignature(true,"15.4 Topaz MiddleTier\n15.4 Topaz MiddleTier\n15.4 Topaz MiddleTier3","FFFFFFFFAD1F4B693A00000000000000DEB4BBA8050C7F6E66DD8CC844BE7EEC6F3102C5E6B6F4977E482CE5F5E82ADCEEFFC822D77A6AB9529E824C363E0822ED65EBC841D98F55C7FF5CF269D8CBEF37B4E68BE56095D172699A8A58D8F71E366D6F65DA865F186454B4C9219D159147294001EBE30DF244B970B7B015C0FAEBC74BDDA61551412F6761D295D5C754A58B2B94781E067E7FEC2FEE0AAD6F0463D2C48F1EF21AD068CFE314CE2738949A01A19D29564D16A8C3D1CE462FEA754AD769CAFDEC057F9CF7AEB8BC68CDBDE965B83D75226294C44B14F4391BC07C32D908F409247E80C9C4C9DFE27C123459651FB5240C922841254DD77107DB1E6A5A525AFA10947BB1488D058C88E127DD6790A3F30A5013D9643AFDB03762875EA5D7D336EE6076B4AB7BD089872BD687300CF16AA32DC9606A91CD726E0244D91B760EA76C904B27180D271CCE952B80C4C65E39EBD2E41B84B7A7309AB3D953E673E788E063F624568F8C936B440254D832245B2DF3730066B0D32CBBD779AF1299037B3A14C72376D13B3B65E87D078FFB0FB98E2F53E6E02939A697AFAF585853EDB281D283765A15B1CE10060C4DC8A32D40884F8C4F5DEA3FA31345073F7C73434A9AE2913F7C73434A9AE2913F7C73434A9AE2913F7C73434A9AE291");
			if(sbw.GetNumberOfTabletPoints(true)==0) {
				listFailedTests.Add(8);
			}
			//9:  Invalid Signature (wrong key)
			sbw.FillSignature(false,"15.4 Normal\r\n15.4 Normal\r\n15.4 Normal4","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");
			if(sbw.GetNumberOfTabletPoints(false)!=0) {//This test is meant to be invalid intentionally.
				listFailedTests.Add(9);
			}
			//if(listFailedTests.Count>0) {
			//	throw new Exception("Signature tests "+String.Join(",",listFailedTests)+" failed.\r\n");
			//}
			Assert.AreEqual(0,listFailedTests.Count);
			//return "52: Passed.  SignatureBoxWrapper signatures validated correctly.\r\n";
		}

		///<summary>Fees logic: #1: For PPOInsPlan1, Dr. Jones, Dr. Smith, and Dr. Wilson have different fees.</summary>
		[TestMethod]
		public void Legacy_TestFiftySeven() {
			//if(specificTest!=0 && specificTest!=57) {
			//	return "";
			//}
			Patient pat=PatientT.CreatePatient("57");
			long feeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"PPOInsPlan1",false);
			long codeNum=ProcedureCodes.GetCodeNum("D2750");
			long provNum1=ProviderT.CreateProvider("1-57");
			long provNum2=ProviderT.CreateProvider("2-57");
			long provNum3=ProviderT.CreateProvider("3-57");
			FeeT.CreateFee(feeSchedNum,codeNum,50,0,provNum1);
			FeeT.CreateFee(feeSchedNum,codeNum,55,0,provNum2);
			FeeT.CreateFee(feeSchedNum,codeNum,60,0,provNum3);
			double fee1=Fees.GetFee(codeNum,feeSchedNum,0,provNum1).Amount;
			double fee2=Fees.GetFee(codeNum,feeSchedNum,0,provNum2).Amount;
			double fee3=Fees.GetFee(codeNum,feeSchedNum,0,provNum3).Amount;
			//if(fee1!=50
			//	|| fee2!=55
			//	|| fee3!=60) 
			//{
			//	throw new Exception("Incorrect fees returned:\r\n"
			//		+"\tFee #1 should be $50, returned value:"+fee1.ToString("C")+"\r\n"
			//		+"\tFee #2 should be $55, returned value:"+fee2.ToString("C")+"\r\n"
			//		+"\tFee #3 should be $60, returned value:"+fee3.ToString("C")+"\r\n");
			//}
			Assert.AreEqual(50,fee1);
			Assert.AreEqual(55,fee2);
			Assert.AreEqual(60,fee3);
			//return "57: Passed.  Provider specific fees were correctly returned.\r\n";
		}

		///<summary>Fees logic: #2: Clinic A, B, and C have different standard UCR fees.</summary>
		[TestMethod]
		public void Legacy_TestFiftyEight() {
			//if(specificTest!=0 && specificTest!=58) {
			//	return "";
			//}
			Patient pat=PatientT.CreatePatient("58");
			long feeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"Standard UCR",false);
			long codeNum=ProcedureCodes.GetCodeNum("D2750");
			long clinicNum2=ClinicT.CreateClinic("2-58").ClinicNum;
			long clinicNum3=ClinicT.CreateClinic("3-58").ClinicNum;
			long clinicNum1=ClinicT.CreateClinic("1-58").ClinicNum;
			FeeT.CreateFee(feeSchedNum,codeNum,65,clinicNum1,0);
			FeeT.CreateFee(feeSchedNum,codeNum,70,clinicNum2,0);
			FeeT.CreateFee(feeSchedNum,codeNum,75,clinicNum3,0);
			double fee1=Fees.GetFee(codeNum,feeSchedNum,clinicNum1,0).Amount;
			double fee2=Fees.GetFee(codeNum,feeSchedNum,clinicNum2,0).Amount;
			double fee3=Fees.GetFee(codeNum,feeSchedNum,clinicNum3,0).Amount;
			//if(fee1!=65
			//	|| fee2!=70
			//	|| fee3!=75) 
			//{
			//	throw new Exception("Incorrect fees returned:\r\n"
			//		+"\tFee #1 should be $65, returned value:"+fee1.ToString("C")+"\r\n"
			//		+"\tFee #2 should be $70, returned value:"+fee2.ToString("C")+"\r\n"
			//		+"\tFee #3 should be $75, returned value:"+fee3.ToString("C")+"\r\n");
			//}
			Assert.AreEqual(fee1,65);
			Assert.AreEqual(fee2,70);
			Assert.AreEqual(fee3,75);
			//return "58: Passed.  Clinic specific fees were correctly returned.\r\n";
		}

		///<summary>Fees logic: #3: Dr. Jane and Dr. George have different standard UCR fees. Dr. George's works in two clinics (A and B),
		///and his standard fees are different depending on the clinic.</summary>
		[TestMethod]
		public void Legacy_TestFiftyNine() {
			//if(specificTest!=0 && specificTest!=59) {
			//	return "";
			//}
			Patient pat=PatientT.CreatePatient("59");
			long feeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"Standard",false);
			long codeNum=ProcedureCodes.GetCodeNum("D2750");
			long provNum1=ProviderT.CreateProvider("1-59");
			long provNum2=ProviderT.CreateProvider("2-59");
			long clinicNum1=ClinicT.CreateClinic("1-59").ClinicNum;
			long clinicNum2=ClinicT.CreateClinic("2-59").ClinicNum;
			FeeT.CreateFee(feeSchedNum,codeNum,80,clinicNum1,provNum1);
			FeeT.CreateFee(feeSchedNum,codeNum,85,clinicNum1,provNum2);
			FeeT.CreateFee(feeSchedNum,codeNum,90,clinicNum2,provNum2);
			double fee1=Fees.GetFee(codeNum,feeSchedNum,clinicNum1,provNum1).Amount;
			double fee2=Fees.GetFee(codeNum,feeSchedNum,clinicNum1,provNum2).Amount;
			double fee3=Fees.GetFee(codeNum,feeSchedNum,clinicNum2,provNum2).Amount;
			//if(fee1!=80
			//	|| fee2!=85
			//	|| fee3!=90) 
			//{
			//	throw new Exception("Incorrect fees returned:\r\n"
			//		+"\tFee #1 should be $80, returned value:"+fee1.ToString("C")+"\r\n"
			//		+"\tFee #2 should be $85, returned value:"+fee2.ToString("C")+"\r\n"
			//		+"\tFee #3 should be $90, returned value:"+fee3.ToString("C")+"\r\n");
			//}
			Assert.AreEqual(fee1,80);
			Assert.AreEqual(fee2,85);
			Assert.AreEqual(fee3,90);
			//return "59: Passed.  The mixture of providers with multiple clinic specific fees were correctly returned.\r\n";
		}

		///<summary>Downgrade insurance estimates #1. The PPO fee schedule has a blank fee for the downgraded code.</summary>
		[TestMethod]
		public void Legacy_TestSixty() {
			//if(specificTest != 0 && specificTest != 60) {
			//	return "";
			//}
			string suffix="60";
			Patient pat=PatientT.CreatePatient(suffix);
			long ucrFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"UCR Fees"+suffix);
			long ppoFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"PPO Downgrades"+suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			long subNum=sub.InsSubNum;
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Restorative,100);
			PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			ProcedureCode originalProcCode=ProcedureCodes.GetProcCode("D2393");
			ProcedureCode downgradeProcCode=ProcedureCodes.GetProcCode("D2160");
			originalProcCode.SubstitutionCode="D2160";
			originalProcCode.SubstOnlyIf=SubstitutionCondition.Always;
			ProcedureCodes.Update(originalProcCode);
			FeeT.CreateFee(ucrFeeSchedNum,originalProcCode.CodeNum,300);
			FeeT.CreateFee(ucrFeeSchedNum,downgradeProcCode.CodeNum,100);
			FeeT.CreateFee(ppoFeeSchedNum,originalProcCode.CodeNum,120);
			//No fee entered for D2160 in PPO Downgrades
			Procedure proc=ProcedureT.CreateProcedure(pat,"D2393",ProcStat.C,"1",300);//Tooth 1
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<Procedure> ProcList=Procedures.Refresh(pat.PatNum);
			InsPlan insPlan = planList[0];//Should only be one
			InsPlan planOld = insPlan.Copy();//Should only be one
			insPlan.PlanType="p";
			insPlan.FeeSched=ppoFeeSchedNum;
			InsPlans.Update(insPlan,planOld);
			//Creates the claim in the same manner as the account module, including estimates.
			Claim claim=ClaimT.CreateClaim("P",patPlans,planList,claimProcs,ProcList,pat,ProcList,benefitList,subList);
			ClaimProc clProc=ClaimProcs.Refresh(pat.PatNum)[0];//Should only be one
			//if(clProc.InsEstTotal != 120 || clProc.WriteOff != 180) {
			//	throw new Exception("Incorrect claim proc values returned:\r\n"
			//		+"\tClaim proc Ins Est should be $120, returned value:"+clProc.InsEstTotal.ToString("C")+"\r\n"
			//		+"\tClaim proc Writeoff should be $180, returned value:"+clProc.WriteOff.ToString("C")+"\r\n");
			//}
			Assert.IsFalse(clProc.InsEstTotal != 120 || clProc.WriteOff != 180);
			//return "60: Passed. Procedure code downgrades function properly when the downgrade fee is blank.\r\n";
		}

		///<summary>Downgrade insurance estimates #2. The PPO fee schedule has a higher fee for the downgraded code than for the original code.</summary>
		[TestMethod]
		public void Legacy_TestSixtyOne() {
			//if(specificTest != 0 && specificTest != 61) {
			//	return "";
			//}
			string suffix="61";
			Patient pat=PatientT.CreatePatient(suffix);
			long ucrFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"UCR Fees"+suffix);
			long ppoFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"PPO Downgrades"+suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			long subNum=sub.InsSubNum;
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Restorative,100);
			PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			ProcedureCode originalProcCode=ProcedureCodes.GetProcCode("D2391");
			ProcedureCode downgradeProcCode=ProcedureCodes.GetProcCode("D2140");
			originalProcCode.SubstitutionCode="D2140";
			originalProcCode.SubstOnlyIf=SubstitutionCondition.Always;
			ProcedureCodes.Update(originalProcCode);
			FeeT.CreateFee(ucrFeeSchedNum,originalProcCode.CodeNum,140);
			FeeT.CreateFee(ucrFeeSchedNum,downgradeProcCode.CodeNum,120);
			FeeT.CreateFee(ppoFeeSchedNum,originalProcCode.CodeNum,80);
			FeeT.CreateFee(ppoFeeSchedNum,downgradeProcCode.CodeNum,100);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D2391",ProcStat.C,"1",140);//Tooth 1
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<Procedure> ProcList=Procedures.Refresh(pat.PatNum);
			InsPlan insPlan=planList[0];//Should only be one
			InsPlan planOld = insPlan.Copy();
			insPlan.PlanType="p";
			insPlan.FeeSched=ppoFeeSchedNum;
			InsPlans.Update(insPlan,planOld);
			//Creates the claim in the same manner as the account module, including estimates.
			Claim claim=ClaimT.CreateClaim("P",patPlans,planList,claimProcs,ProcList,pat,ProcList,benefitList,subList);
			ClaimProc clProc=ClaimProcs.Refresh(pat.PatNum)[0];//Should only be one
			//if(clProc.InsEstTotal != 80 || clProc.WriteOff != 60) {
			//	throw new Exception("Incorrect claim proc values returned:\r\n"
			//		+"\tClaim proc Ins Est should be $80, returned value:"+clProc.InsEstTotal.ToString("C")+"\r\n"
			//		+"\tClaim proc Writeoff should be $60, returned value:"+clProc.WriteOff.ToString("C")+"\r\n");
			//}
			Assert.IsFalse(clProc.InsEstTotal != 80 || clProc.WriteOff != 60);
			//return "61: Passed. Procedure code downgrades function properly when the downgraded fee minus the writeoff is less than the allowed amount.\r\n";
		}

		///<summary>Tests clinic-specific overtime hour adjustments for a single work period.</summary>
		[TestMethod]
		public void Legacy_TestSixtyTwo() {
			//if(specificTest != 0 && specificTest !=62) {
			//	return "";
			//}
			string suffix="62";
			DateTime startDate=DateTime.Parse("2001-01-01");
			Employee emp=EmployeeT.CreateEmployee(suffix);
			PayPeriod payP1=PayPeriodT.CreateTwoWeekPayPeriodIfNotExists(startDate);
			PayPeriods.RefreshCache();
			Prefs.UpdateInt(PrefName.TimeCardOvertimeFirstDayOfWeek,0);
			TimeCardRules.RefreshCache();
			//Each of these are 11 hour days. Should have 4 hours of OT with clinic 3 and 11 hours OT with clinic 4 the end of the pay period.
			long clockEvent1=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(0).AddHours(6),startDate.AddDays(0).AddHours(17),0);
			long clockEvent2=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(1).AddHours(6),startDate.AddDays(1).AddHours(17),1);
			long clockEvent3=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(2).AddHours(6),startDate.AddDays(2).AddHours(17),2);
			long clockEvent4=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(3).AddHours(6),startDate.AddDays(3).AddHours(17),3);
			long clockEvent5=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(4).AddHours(6),startDate.AddDays(4).AddHours(17),4);
			TimeCardRules.CalculateWeeklyOvertime(emp,payP1.DateStart,payP1.DateStop);
			//Validate
			string retVal="";
			//Check
			List<TimeAdjust> listAdjusts=TimeAdjusts.GetValidList(emp.EmployeeNum,startDate,startDate.AddDays(5)).OrderBy(x=>x.OTimeHours).ToList();
			//if(listAdjusts.Count!=2) {
			//	throw new Exception("Incorrect number of OT adjustments created.  There should be two.");
			//}
			Assert.AreEqual(2,listAdjusts.Count);
			//if(listAdjusts[0].RegHours!=TimeSpan.FromHours(-4)) {
			//	throw new Exception("First adjustment to regular hours should be -4 hours, instead it is for "+listAdjusts[0].RegHours.TotalHours+" hours.\r\n");
			//}
			Assert.IsFalse(listAdjusts[0].RegHours!=TimeSpan.FromHours(-4));
			//if(listAdjusts[0].ClinicNum!=3) {
			//	throw new Exception("First adjustment should be for clinic 3.  Instead it is for clinic "+listAdjusts[0].ClinicNum+"\r\n");
			//}
			Assert.IsFalse(listAdjusts[0].ClinicNum!=3);
			//if(listAdjusts[0].OTimeHours!=TimeSpan.FromHours(4)) {
			//	throw new Exception("First adjustment to OT hours should be 4 hours, instead it is "+listAdjusts[0].OTimeHours.TotalHours+" hours.\r\n");
			//}
			Assert.IsFalse(listAdjusts[0].OTimeHours!=TimeSpan.FromHours(4));
			//if(listAdjusts[1].RegHours!=TimeSpan.FromHours(-11)) {
			//	throw new Exception("Second adjustment to regular hours should be -11 hours, instead it is for "+listAdjusts[1].RegHours.TotalHours+" hours.\r\n");
			//}
			Assert.IsFalse(listAdjusts[1].RegHours!=TimeSpan.FromHours(-11));
			//if(listAdjusts[1].ClinicNum!=4) {
			//	throw new Exception("Second adjustment should be for clinic 4.  Instead it is for clinic "+listAdjusts[1].ClinicNum+"\r\n");
			//}
			Assert.IsFalse(listAdjusts[1].ClinicNum!=4);
			//if(listAdjusts[1].OTimeHours!=TimeSpan.FromHours(11)) {
			//	throw new Exception("Second adjustment to OT hours should be 11 hours, instead it is "+listAdjusts[1].OTimeHours.TotalHours+" hours.\r\n");
			//}
			Assert.IsFalse(listAdjusts[1].OTimeHours!=TimeSpan.FromHours(11));
			retVal+="62: Passed. Overtime calculated properly for normal 40 hour work week, using clinics.\r\n";
			//return retVal;
		}

		///<summary>Tests clinic-specific overtime hour adjustments for work week spanning two pay periods.</summary>
		[TestMethod]
		public void Legacy_TestSixtyThree() {
			//if(specificTest != 0 && specificTest !=63) {
			//	return "";
			//}
			string suffix="63";
			DateTime startDate=DateTime.Parse("2001-02-01");//This will create a pay period that splits a work week.
			Employee emp=EmployeeT.CreateEmployee(suffix);
			PayPeriod payP1=PayPeriodT.CreateTwoWeekPayPeriodIfNotExists(startDate);
			PayPeriod payP2=PayPeriodT.CreateTwoWeekPayPeriodIfNotExists(startDate.AddDays(14));
			PayPeriods.RefreshCache();
			Prefs.UpdateInt(PrefName.TimeCardOvertimeFirstDayOfWeek,0);
			TimeCardRules.RefreshCache();
			//Each of these are 11 hour days. Should have 4 hours of OT with clinic 3 in the second pay period.
			long clockEvent1=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(10).AddHours(6),startDate.AddDays(10).AddHours(17),0);
			long clockEvent2=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(11).AddHours(6),startDate.AddDays(11).AddHours(17),1);
			long clockEvent3=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(12).AddHours(6),startDate.AddDays(12).AddHours(17),2);
			//new pay period
			long clockEvent4=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(14).AddHours(6),startDate.AddDays(14).AddHours(17),3);
			TimeCardRules.CalculateWeeklyOvertime(emp,payP1.DateStart,payP1.DateStop);
			TimeCardRules.CalculateWeeklyOvertime(emp,payP2.DateStart,payP2.DateStop);
			//Validate
			string retVal="";
			//Check
			List<TimeAdjust> listAdjusts=TimeAdjusts.GetValidList(emp.EmployeeNum,startDate,startDate.AddDays(28));
			//if(listAdjusts.Count!=1) {
			//	throw new Exception("Incorrect number of OT adjustments created.  There should be one.\r\n");
			//}
			Assert.AreEqual(1,listAdjusts.Count);
			//if(listAdjusts[0].RegHours!=TimeSpan.FromHours(-4)) {
			//	throw new Exception("The adjustment to regular hours should be -4 hours, instead it is for "+listAdjusts[0].RegHours.TotalHours+" hours.\r\n");
			//}
			Assert.IsFalse(listAdjusts[0].RegHours!=TimeSpan.FromHours(-4));
			//if(listAdjusts[0].ClinicNum!=3) {
			//	throw new Exception("The adjustment should be for clinic 3.  Instead it is for clinic "+listAdjusts[0].ClinicNum+"\r\n");
			//}
			Assert.AreEqual(3,listAdjusts[0].ClinicNum);
			//if(listAdjusts[0].OTimeHours!=TimeSpan.FromHours(4)) {
			//	throw new Exception("The adjustment to OT hours should be 4 hours, instead it is "+listAdjusts[0].OTimeHours.TotalHours+" hours.\r\n");
			//}
			Assert.AreEqual(listAdjusts[0].OTimeHours,TimeSpan.FromHours(4));
			retVal+="63: Passed. Overtime calculated properly for work week spanning 2 pay periods, using clinics.\r\n";
			//return retVal;
		}

		///<summary>Tests clinic-specific overtime hour adjustments for work week spanning two pay periods and expecting adjustments for multiple clinics.</summary>
		[TestMethod]
		public void Legacy_TestSixtyFour() {
			//if(specificTest != 0 && specificTest !=64) {
			//	return "";
			//}
			string suffix="64";
			DateTime startDate=DateTime.Parse("2001-02-01");//This will create a pay period that splits a work week.
			Employee emp=EmployeeT.CreateEmployee(suffix);
			PayPeriod payP1=PayPeriodT.CreateTwoWeekPayPeriodIfNotExists(startDate);
			PayPeriod payP2=PayPeriodT.CreateTwoWeekPayPeriodIfNotExists(startDate.AddDays(14));
			PayPeriods.RefreshCache();
			Prefs.UpdateInt(PrefName.TimeCardOvertimeFirstDayOfWeek,0);
			TimeCardRules.RefreshCache();
			//Each of these are 11 hour days. Should have 4 hours of OT with clinic 3 in the second pay period and 11 hours for clinic 4.
			long clockEvent1=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(10).AddHours(6),startDate.AddDays(10).AddHours(17),0);//Sun
			long clockEvent2=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(11).AddHours(6),startDate.AddDays(11).AddHours(17),1);//Mon
			long clockEvent3=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(12).AddHours(6),startDate.AddDays(12).AddHours(17),2);//Tue
			//new pay period
			long clockEvent4=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(14).AddHours(6),startDate.AddDays(14).AddHours(17),3);//Wed
			long clockEvent5=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(15).AddHours(6),startDate.AddDays(15).AddHours(17),4);//Thurs
			TimeCardRules.CalculateWeeklyOvertime(emp,payP1.DateStart,payP1.DateStop);
			TimeCardRules.CalculateWeeklyOvertime(emp,payP2.DateStart,payP2.DateStop);
			//Validate
			string retVal="";
			//Check
			List<TimeAdjust> listAdjusts=TimeAdjusts.GetValidList(emp.EmployeeNum,startDate,startDate.AddDays(28)).OrderBy(x=>x.OTimeHours).ToList();
			//if(listAdjusts.Count!=2) {
			//	throw new Exception("Incorrect number of OT adjustments created.  There should be two.");
			//}
			Assert.AreEqual(2,listAdjusts.Count);
			//Adjust 4 hours for clinic 3
			//if(listAdjusts[0].RegHours!=TimeSpan.FromHours(-4)) {
			//	throw new Exception("The adjustment to regular hours should be -4 hours, instead it is for "+listAdjusts[0].RegHours.TotalHours+" hours.\r\n");
			//}
			Assert.IsFalse(listAdjusts[0].RegHours!=TimeSpan.FromHours(-4));
			//if(listAdjusts[0].ClinicNum!=3) {
			//	throw new Exception("The adjustment should be for clinic 3.  Instead it is for clinic "+listAdjusts[0].ClinicNum+"\r\n");
			//}
			Assert.IsFalse(listAdjusts[0].ClinicNum!=3);
			//if(listAdjusts[0].OTimeHours!=TimeSpan.FromHours(4)) {
			//	throw new Exception("The adjustment to OT hours should be 4 hours, instead it is "+listAdjusts[0].OTimeHours.TotalHours+" hours.\r\n");
			//}
			Assert.IsFalse(listAdjusts[0].OTimeHours!=TimeSpan.FromHours(4));
			//Adjust 11 hours for clinic 4
			//if(listAdjusts[1].RegHours!=TimeSpan.FromHours(-11)) {
			//	throw new Exception("The adjustment to regular hours should be -11 hours, instead it is for "+listAdjusts[1].RegHours.TotalHours+" hours.\r\n");
			//}
			Assert.IsFalse(listAdjusts[1].RegHours!=TimeSpan.FromHours(-11));
			//if(listAdjusts[1].ClinicNum!=4) {
			//	throw new Exception("The adjustment should be for clinic 4.  Instead it is for clinic "+listAdjusts[1].ClinicNum+"\r\n");
			//}
			Assert.IsFalse(listAdjusts[1].ClinicNum!=4);
			//if(listAdjusts[1].OTimeHours!=TimeSpan.FromHours(11)) {
			//	throw new Exception("The adjustment to OT hours should be 11 hours, instead it is "+listAdjusts[1].OTimeHours.TotalHours+" hours.\r\n");
			//}
			Assert.IsFalse(listAdjusts[1].OTimeHours!=TimeSpan.FromHours(11));
			retVal+="64: Passed. Overtime calculated properly for work week spanning 2 pay periods, and expecting adjustments for multiple clinics.\r\n";
			//return retVal;
		}

		///<summary>A patient has Medicaid/FlatCopay as secondary insurance. It should use its own fee schedule when calculating its coverage.</summary>
		[TestMethod]
		public void Legacy_TestSixtyFive() {
			//if(specificTest!=0 && specificTest!=65) {
			//	return "";
			//}
			//need one patient
			//one office fee schedule
			//crowns cost 1500
			//one PPO ins fee sched
			//crowns cost 1000
			//one medicaid fee sched
			//crowns cost 0.00
			//one ppo insurance
			//covers crowns at 50%
			//annual max of 500
			//one medicaid ins
			//crown is charted
			//output should be:
			//PPO covers 500
			//writeoff is 500
			//Medicaid covers 0.00
			//writeoff is 500.00
			long officeFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"Office");
			long ppoFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"PPO");
			long medicaidFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"Medicaid");
			long copayFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.CoPay,"CoPay");
			long codeNum=ProcedureCodes.GetCodeNum("D2750");
			long provNum=ProviderT.CreateProvider("Prov","","",officeFeeSchedNum);
			Patient pat=PatientT.CreatePatient("60",provNum);
			FeeT.CreateFee(officeFeeSchedNum,codeNum,1500); //office fee is 1500.
			FeeT.CreateFee(ppoFeeSchedNum,codeNum,1100); //ppo fee is 1100. writeoff should be 400
			FeeT.CreateFee(medicaidFeeSchedNum,codeNum,30); //medicaid fee is 30.
			FeeT.CreateFee(copayFeeSchedNum,codeNum,15); //copay is 15, so ins est should be 30 - 15 = 15.
																									 //Carrier
			Carrier ppoCarrier=CarrierT.CreateCarrier("PPO");
			Carrier medicaidCarrier=CarrierT.CreateCarrier("Medicaid");
			long planPPO=InsPlanT.CreateInsPlanPPO(ppoCarrier.CarrierNum,ppoFeeSchedNum).PlanNum;
			long planMedi=InsPlanT.CreateInsPlanMediFlatCopay(medicaidCarrier.CarrierNum,medicaidFeeSchedNum,copayFeeSchedNum).PlanNum;
			InsSub subPPO=InsSubT.CreateInsSub(pat.PatNum,planPPO);
			long subNumPPO=subPPO.InsSubNum;
			InsSub subMedi=InsSubT.CreateInsSub(pat.PatNum,planMedi);
			long subNumMedi=subMedi.InsSubNum;
			BenefitT.CreateCategoryPercent(planPPO,EbenefitCategory.Crowns,50);
			BenefitT.CreateAnnualMax(planPPO,500);
			PatPlanT.CreatePatPlan(1,pat.PatNum,subNumPPO);
			PatPlanT.CreatePatPlan(2,pat.PatNum,subNumMedi);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D2750",ProcStat.TP,"14",Fees.GetAmount0(codeNum,officeFeeSchedNum));
			long procNum=proc.ProcNum;
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			//this is what's really being tested.
			//must pass in the empty histList and loopList (instead of null) or annual max's don't get considered.
			Procedures.ComputeEstimates(proc,pat.PatNum,ref claimProcs,true,planList,patPlans,benefitList,histList,loopList,true,pat.Age,subList);
			List<ClaimProc> listResult=ClaimProcs.RefreshForProc(procNum);
			ClaimProc ppoClaimProc=listResult.Where(x => x.PlanNum == planPPO).First();
			ClaimProc medicaidClaimProc=listResult.Where(x => x.PlanNum == planMedi).First();
			//if(ppoClaimProc.InsEstTotal != 500 || medicaidClaimProc.InsEstTotal != 15
			//	|| medicaidClaimProc.WriteOffEst !=-1 || ppoClaimProc.WriteOffEst != 400
			//	|| medicaidClaimProc.CopayAmt != 15) {
			//	throw new Exception("Incorrect Estimates returned. "
			//		+"\r\nPPOClaimProc InsEst Total = "+ppoClaimProc.InsEstTotal+"; should be 500. "
			//		+"\r\nPPOClaimProc Writeoff = "+ppoClaimProc.WriteOffEst+"; should be 400. "
			//		+"\r\nMedicaidClaimProc InsEst Total = "+medicaidClaimProc.InsEstTotal+"; should be 15. "
			//		+"\r\nMedicaidClaimProc Writeoff = "+medicaidClaimProc.WriteOffEst+"; should be -1. "
			//		+"\r\nMedicaidClaimProc Copay = "+medicaidClaimProc.CopayAmt+"; should be 15. "
			//		);
			Assert.IsFalse(ppoClaimProc.InsEstTotal != 500 || medicaidClaimProc.InsEstTotal != 15
				|| medicaidClaimProc.WriteOffEst !=-1 || ppoClaimProc.WriteOffEst != 400
				|| medicaidClaimProc.CopayAmt != 15);
			//}
			//else {
			//	return "65: Passed.  The secondary medicaid/flat copay insurance plan used its own fee schedule when calculating estimates.\r\n";
			//}
		}

		///<summary>Tests a normal work week with a start of the week in the previous pay period with break adjustments.
		///Note: This unit test was based on real data from a real set of timecard entries.</summary>
		[TestMethod]
		public void Legacy_TestSixtySix() {
			//if(specificTest!=0 && specificTest!=66) {
			//	return "";
			//}
			string suffix="66";
			DateTime startDate=DateTime.Parse("2016-05-09");//This will create a pay period that splits a work week.
			Employee emp=EmployeeT.CreateEmployee(suffix);
			PayPeriod payP1=PayPeriodT.CreateTwoWeekPayPeriodIfNotExists(startDate);
			PayPeriods.RefreshCache();
			Prefs.UpdateInt(PrefName.TimeCardOvertimeFirstDayOfWeek,0);
			TimeCardRules.RefreshCache();
			//Each of these are 11 hour days. Should have 4 hours of OT with clinic 3 in the second pay period and 11 hours for clinic 4.
			//Week 1 - 40.4 hours
			long clockEvent1=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(0).AddHours(6),startDate.AddDays(0).AddHours(6+8),0);//Mon
			long clockEvent2=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(1).AddHours(6),startDate.AddDays(1).AddHours(6+8),0);//Tue
			long clockEvent3=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(2).AddHours(6),startDate.AddDays(2).AddHours(6+8.76),0,0.06);//Wed
			long clockEvent4=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(3).AddHours(6),startDate.AddDays(3).AddHours(6+8.72),0,0.73);//Thurs
			long clockEvent5=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(4).AddHours(6),startDate.AddDays(4).AddHours(6+8.12),0,0.41);//Fri
			//Week 2 - 41.23 hours
			long clockEvent6=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(7).AddHours(6),startDate.AddDays(7).AddHours(6+8.79),0,0.4);//Mon
			long clockEvent7=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(8).AddHours(6),startDate.AddDays(8).AddHours(6+8.85),0,0.38);//Tue
			long clockEvent8=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(9).AddHours(6),startDate.AddDays(9).AddHours(6+7.78),0,0.29);//Wed
			long clockEvent9=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(10).AddHours(6),startDate.AddDays(10).AddHours(6+8.88),0,0.02);//Thurs
			long clockEvent10=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(11).AddHours(6),startDate.AddDays(11).AddHours(6+8.59),0,0.57);//Fri
			TimeCardRules.CalculateWeeklyOvertime(emp,payP1.DateStart,payP1.DateStop);
			//Validate
			string retVal="";
			//Check
			List<TimeAdjust> listAdjusts=TimeAdjusts.GetValidList(emp.EmployeeNum,startDate,startDate.AddDays(28)).OrderBy(x=>x.OTimeHours).ToList();
			//if(listAdjusts.Count!=2) {
			//	throw new Exception("Incorrect number of OT adjustments created.  There should be two.");
			//}
			Assert.IsFalse(listAdjusts.Count!=2);
			//if(listAdjusts[0].RegHours!=TimeSpan.FromHours(-0.4)) {
			//	throw new Exception("The adjustment to regular hours should be -0.4 hours, instead it is for "+listAdjusts[0].RegHours.TotalHours+" hours.\r\n");
			//}
			Assert.IsFalse(listAdjusts[0].RegHours!=TimeSpan.FromHours(-0.4));
			//if(listAdjusts[0].OTimeHours!=TimeSpan.FromHours(0.4)) {
			//	throw new Exception("The adjustment to OT hours should be 0.4 hours, instead it is "+listAdjusts[0].OTimeHours.TotalHours+" hours.\r\n");
			//}
			Assert.IsFalse(listAdjusts[0].OTimeHours!=TimeSpan.FromHours(0.4));
			//if(listAdjusts[1].RegHours!=TimeSpan.FromHours(-1.23)) {
			//	throw new Exception("The adjustment to regular hours should be -1.23 hours, instead it is for "+listAdjusts[1].RegHours.TotalHours+" hours.\r\n");
			//}
			Assert.IsFalse(listAdjusts[1].RegHours!=TimeSpan.FromHours(-1.23));
			//if(listAdjusts[1].OTimeHours!=TimeSpan.FromHours(1.23)) {
			//	throw new Exception("The adjustment to OT hours should be 1.23 hours, instead it is "+listAdjusts[1].OTimeHours.TotalHours+" hours.\r\n");
			//}
			Assert.IsFalse(listAdjusts[1].OTimeHours!=TimeSpan.FromHours(1.23));
			retVal+="66: Passed. Overtime calculated properly for work week spanning 2 pay periods, with included breaks.\r\n";
			//return retVal;
		}

		///<summary>Test work week with manual overtime hours.
		///Note: This unit test was based on real data from a real set of timecard entries including dates, timespans, and the like.</summary>
		[TestMethod]
		public void Legacy_TestSixtySeven() {
			//if(specificTest!=0 && specificTest!=67) {
			//	return "";
			//}
			string suffix="67";
			DateTime startDate=DateTime.Parse("2016-03-14");
			Employee emp=EmployeeT.CreateEmployee(suffix);
			PayPeriod payP1=PayPeriodT.CreateTwoWeekPayPeriodIfNotExists(startDate);
			PayPeriods.RefreshCache();
			Prefs.UpdateInt(PrefName.TimeCardOvertimeFirstDayOfWeek,0);
			TimeCardRules.RefreshCache();
			//Each of these are 11 hour days. Should have 4 hours of OT with clinic 3 in the second pay period and 11 hours for clinic 4.
			//Week 1 - 40.13 (Note: These appear as they should after CalculateDaily is run.)
			long clockEvent1=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(0).AddHours(6),startDate.AddDays(0).AddHours(6+8.06),0);//Mon
			long clockEvent2=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(1).AddHours(6),startDate.AddDays(1).AddHours(6+8),0);//Tue
			long clockEvent3=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(2).AddHours(6),startDate.AddDays(2).AddHours(6+8.08),0);//Wed
			long clockEvent4=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(3).AddHours(6),startDate.AddDays(3).AddHours(6+8),0,0.02);//Thurs
			long clockEvent5=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(4).AddHours(6),startDate.AddDays(4).AddHours(6+8.01),0);//Fri
			//SATURDAY - 4.1 HRS OF OVERTIME 
			ClockEvent ce=new ClockEvent();
			ce.ClinicNum=0;
			ce.ClockStatus=TimeClockStatus.Home;
			ce.EmployeeNum=emp.EmployeeNum;
			ce.OTimeHours=TimeSpan.FromHours(4.1);
			ce.TimeDisplayed1=new DateTime(startDate.Year,startDate.Month,startDate.AddDays(5).Day,6,54,0);
			ce.TimeDisplayed2=new DateTime(startDate.Year,startDate.Month,startDate.AddDays(5).Day,11,0,0);
			ce.TimeEntered1=ce.TimeDisplayed1;
			ce.TimeEntered2=ce.TimeDisplayed2;
			ce.ClockEventNum=ClockEvents.Insert(ce);
			ClockEvents.Update(ce);
			//Week 2 - 41.06
			long clockEvent6=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(7).AddHours(6),startDate.AddDays(7).AddHours(6+8.02),0);//Mon
			long clockEvent7=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(8).AddHours(6),startDate.AddDays(8).AddHours(6+8),0);//Tue
			long clockEvent8=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(9).AddHours(6),startDate.AddDays(9).AddHours(6+8),0);//Wed
			long clockEvent9=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(10).AddHours(6),startDate.AddDays(10).AddHours(6+9.04),0);//Thurs
			long clockEvent10=ClockEventT.InsertWorkPeriod(emp.EmployeeNum,startDate.AddDays(11).AddHours(6),startDate.AddDays(11).AddHours(6+8),0);//Fri
			TimeCardRules.CalculateWeeklyOvertime(emp,payP1.DateStart,payP1.DateStop);
			//Validate
			string retVal="";
			//Check
			List<TimeAdjust> listAdjusts=TimeAdjusts.GetValidList(emp.EmployeeNum,startDate,startDate.AddDays(28)).OrderBy(x=>x.OTimeHours).ToList();
			//if(listAdjusts.Count!=2) {
			//	throw new Exception("Incorrect number of OT adjustments created.  There should be two.");
			//}
			Assert.IsFalse(listAdjusts.Count!=2);
			//if(listAdjusts[0].RegHours!=TimeSpan.FromHours(-0.13)) {
			//	throw new Exception("The adjustment to regular hours should be -0.12 hours, instead it is for "+listAdjusts[0].RegHours.TotalHours+" hours.\r\n");
			//}
			Assert.IsFalse(listAdjusts[0].RegHours!=TimeSpan.FromHours(-0.13));
			//if(listAdjusts[0].OTimeHours!=TimeSpan.FromHours(0.13)) {
			//	throw new Exception("The adjustment to OT hours should be 0.12 hours, instead it is "+listAdjusts[0].OTimeHours.TotalHours+" hours.\r\n");
			//}
			Assert.IsFalse(listAdjusts[0].OTimeHours!=TimeSpan.FromHours(0.13));
			//if(listAdjusts[1].RegHours!=TimeSpan.FromHours(-1.06)) {
			//	throw new Exception("The adjustment to regular hours should be -1.06 hours, instead it is for "+listAdjusts[1].RegHours.TotalHours+" hours.\r\n");
			//}
			Assert.IsFalse(listAdjusts[1].RegHours!=TimeSpan.FromHours(-1.06));
			//if(listAdjusts[1].OTimeHours!=TimeSpan.FromHours(1.06)) {
			//	throw new Exception("The adjustment to OT hours should be 1.06 hours, instead it is "+listAdjusts[1].OTimeHours.TotalHours+" hours.\r\n");
			//}
			Assert.IsFalse(listAdjusts[1].OTimeHours!=TimeSpan.FromHours(1.06));
			retVal+="67: Passed. Overtime calculated properly for work week, with a clockevent with an overtime override amount.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestSeventyThree() {
			//if(specificTest!=0 && specificTest!=73) {
			//	return "";
			//}
			CultureInfo cultureOld=Thread.CurrentThread.CurrentCulture;
			CultureInfo uiCultureOld=Thread.CurrentThread.CurrentUICulture;
			Thread.CurrentThread.CurrentCulture=new CultureInfo("en-CA");//Canada
			Thread.CurrentThread.CurrentUICulture=new CultureInfo("en-CA");
			//Mimics TestThirtyEight(...)
			string suffix="73";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			//Set up insurace.
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			long planNum=InsPlanT.CreateInsPlan(carrier.CarrierNum).PlanNum;
			InsSub sub=InsSubT.CreateInsSub(patNum,planNum);
			long subNum1=sub.InsSubNum;
			BenefitT.CreateCategoryPercent(planNum,EbenefitCategory.General,50);
			PatPlanT.CreatePatPlan(1,patNum,subNum1);
			//Procedure 1 - Parent Proc
			Procedure procParent=ProcedureT.CreateProcedure(pat,"D0270",ProcStat.TP,"",100);
			Procedure procOld=procParent.Copy();
			Procedures.Update(procParent,procOld);
			//Procedure 2 - Lab Fee
			Procedure procLabFee=ProcedureT.CreateProcedure(pat,"D0272",ProcStat.TP,"",10);
			procOld=procLabFee.Copy();
			procLabFee.ProcNumLab=procParent.ProcNum;
			Procedures.Update(procLabFee,procOld);
			long procNum=procParent.ProcNum;
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(patNum);
			Family fam=Patients.GetFamily(patNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(patNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			//Validate
			Procedures.ComputeEstimates(procParent,patNum,ref claimProcs,false,planList,patPlans,benefitList,histList,loopList,true,pat.Age,subList);//Triggers claimProc creation for lab too.
			Thread.CurrentThread.CurrentCulture=cultureOld;
			Thread.CurrentThread.CurrentUICulture=uiCultureOld;
			claimProcs=ClaimProcs.Refresh(patNum);
			ClaimProc claimProcParent=ClaimProcs.GetEstimate(claimProcs,procNum,planNum,subNum1);
			ClaimProc claimProcLab=ClaimProcs.GetEstimate(claimProcs,procLabFee.ProcNum,planNum,subNum1);
			//if(claimProcParent.InsEstTotal != 50 || claimProcLab.InsEstTotal != 5) {
			//	throw new Exception("Primary total estimate should be 50 for the parent procedure and 5 for the lab procedure.\r\n");
			//}
			Assert.IsFalse(claimProcParent.InsEstTotal != 50 || claimProcLab.InsEstTotal != 5);
			//return "73: Passed.  Category Percentage insurance estimates for procedures with canadian lab fee.\r\n";
		}

		[TestMethod]
		public void Legacy_TestSeventyFour() {
			//if(specificTest!=0 && specificTest!=74) {
			//	return "";
			//}
			//Joe - This Unit Test mimics an old bug scenario described in task# 809503.
			string suffix="74";
			Patient pat=PatientT.CreatePatient(suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			long subNum=sub.InsSubNum;
			//Patient Annual Max.
			BenefitT.CreateAnnualMax(plan.PlanNum,15000);
			//Procedure sub set benefit.
			Benefit ben=new Benefit();
			ben.PlanNum=plan.PlanNum;
			ben.BenefitType=InsBenefitType.Limitations;
			ben.CovCatNum=1;
			ben.CoverageLevel=BenefitCoverageLevel.Individual;
			ben.MonetaryAmt=1399;
			ben.TimePeriod=BenefitTimePeriod.CalendarYear;
			Benefits.Insert(ben);
			PatPlan patPlan=PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			List<Benefit> listPatBenefits=Benefits.RefreshForPlan(plan.PlanNum,patPlan.PatPlanNum);
			double patAnnualMax=Benefits.GetAnnualMaxDisplay(listPatBenefits,plan.PlanNum,patPlan.PatPlanNum,false);
			//if(patAnnualMax!=15000) {
			//	throw new Exception("Annual Max should be 15,000.\r\n");
			//}
			Assert.AreEqual(15000,patAnnualMax);
			//return "74: Passed.  Annual max correctly calculated with CalendarYear benefit.\r\n";
		}

		[TestMethod]
		public void Legacy_TestSeventyFive() {
			//if(specificTest!=0 && specificTest!=75) {
			//	return "";
			//}
			string suffix="75";
			Patient pat=PatientT.CreatePatient(suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			long subNum=sub.InsSubNum;
			BenefitT.CreateAnnualMaxFamily(plan.PlanNum,400);
			BenefitT.CreateAnnualMax(plan.PlanNum,200);
			BenefitT.CreateOrthoMax(plan.PlanNum,1500);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Orthodontics,100);
			PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			//procs - D2140 (amalgum fillings)
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D8090",ProcStat.TP,"",2000);//Comprehensive ortho
			ProcedureT.SetPriority(proc1,0);
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<Procedure> ProcList=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP=Procedures.GetListTPandTPi(ProcList);//sorted by priority, then toothnum
			//Validate
			string retVal="";
			for(int i=0;i<ProcListTP.Length;i++) {
				Procedures.ComputeEstimates(ProcListTP[i],pat.PatNum,ref claimProcs,false,planList,patPlans,benefitList,
					histList,loopList,false,pat.Age,subList);
				//then, add this information to loopList so that the next procedure is aware of it.
				loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs,ProcListTP[i].ProcNum,ProcListTP[i].CodeNum));
			}
			//save changes in the list to the database
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			ClaimProc claimProc1=ClaimProcs.GetEstimate(claimProcs,proc1.ProcNum,plan.PlanNum,patPlans[0].InsSubNum);
			//if(claimProc1.InsEstTotal!=1500) {//Ortho max should be separate from all other maxes that are not Ortho category specific.
			//	throw new Exception("Claim 1 was "+claimProc1.InsEstTotal+", should be 1500.\r\n");
			//}
			Assert.AreEqual(1500,claimProc1.InsEstTotal);
			retVal+="75: Passed.  Ortho max is calculated separately from general individual and family maxes.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestSeventySix() {
			//if(specificTest!=0 && specificTest!=76) {
			//	return "";
			//}
			string suffix="76";
			Patient pat=PatientT.CreatePatient(suffix);
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			long subNum=sub.InsSubNum;
			BenefitT.CreateAnnualMaxFamily(plan.PlanNum,400);
			BenefitT.CreateAnnualMax(plan.PlanNum,200);
			BenefitT.CreateOrthoMax(plan.PlanNum,1500);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Orthodontics,100);
			PatPlanT.CreatePatPlan(1,pat.PatNum,subNum);
			BenefitT.CreateOrthoFamilyMax(plan.PlanNum,1200);
			//procs - D2140 (amalgum fillings)
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D8090",ProcStat.TP,"",2000);//Comprehensive ortho
			ProcedureT.SetPriority(proc1,0);
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<Procedure> ProcList=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP=Procedures.GetListTPandTPi(ProcList);//sorted by priority, then toothnum
			//Validate
			string retVal="";
			for(int i=0;i<ProcListTP.Length;i++) {
				Procedures.ComputeEstimates(ProcListTP[i],pat.PatNum,ref claimProcs,false,planList,patPlans,benefitList,
					histList,loopList,false,pat.Age,subList);
				//then, add this information to loopList so that the next procedure is aware of it.
				loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs,ProcListTP[i].ProcNum,ProcListTP[i].CodeNum));
			}
			//save changes in the list to the database
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			ClaimProc claimProc1=ClaimProcs.GetEstimate(claimProcs,proc1.ProcNum,plan.PlanNum,patPlans[0].InsSubNum);
			//if(claimProc1.InsEstTotal!=1200) {//Ortho max should be separate from all other maxes that are not Ortho category specific.
			//	throw new Exception("Claim 1 was "+claimProc1.InsEstTotal+", should be 1200.\r\n");
			//}
			Assert.AreEqual(1200,claimProc1.InsEstTotal);
			retVal+="76: Passed.  Ortho family max is used over the individual when the family max is a smaller amount.\r\n";
			//return retVal;
		}

		///<summary>Testing appointment time pattern logic.</summary>
		[TestMethod]
		public void Legacy_TestSeventyNine() {
			//if(specificTest!=0 && specificTest!=79) {
			//	return "";
			//}
			string suffix="79";
			string pattern01="X";
			string pattern02="X/";
			string pattern03="/X";
			string pattern04="/X/";
			string pattern05="/X//";
			string pattern06="/XX/";
			string pattern07="/XXX/";
			string pattern08="//XXX";
			string pattern09="//XXX/";
			string pattern10="/XXXX////";
			string pattern11="/XX/////XXX//";
			/*  X + X = XX  */
			string result1=Appointments.GetApptTimePatternFromProcPatterns(
				new List<string>() {	pattern01	/*  X  */
														, pattern01	/*  X  */
				});
			/*  /X + X = /XX  */
			string result2=Appointments.GetApptTimePatternFromProcPatterns(
				new List<string>() {	pattern03	/*  /X  */
														, pattern01	/*  X  */
				});
			/*  //XXX + X/ = //XXXX/  */
			string result3=Appointments.GetApptTimePatternFromProcPatterns(
				new List<string>() {	pattern08	/*  //XXX  */
														, pattern02	/*  X/  */
				});
			/*  X + X + X/ = XXX/  */
			string result4=Appointments.GetApptTimePatternFromProcPatterns(
				new List<string>() {	pattern01	/*  X  */
														, pattern01	/*  X  */
														, pattern02	/*  X/  */
				});
			/*  /X/ + /X/ + /XX/ + /XX/ = /XXXXXX/  */
			string result5=Appointments.GetApptTimePatternFromProcPatterns(
				new List<string>() {  pattern04	/*  /X/  */
														, pattern04	/*  /X/  */
														, pattern06	/*  /XX/  */
														, pattern06	/*  /XX/  */
				});
			/*  /XXX/ + /X/ + /XXXX//// = /XXXXXXXX////  */
			string result6=Appointments.GetApptTimePatternFromProcPatterns(
				new List<string>() {  pattern07	/*  /XXX/  */
														, pattern04	/*  /X/  */
														, pattern10	/*  /XXXX////  */
				});
			/*  //XXX/ + X/ = //XXXX/  */
			string result7=Appointments.GetApptTimePatternFromProcPatterns(
				new List<string>() {  pattern09	/*  //XXX/  */
														, pattern02	/*  X/  */
				});
			/*  //XXX/ + /X// = //XXXX//  */
			string result8=Appointments.GetApptTimePatternFromProcPatterns(
				new List<string>() {  pattern09	/*  //XXX/  */
														, pattern05	/*  /X//  */
				});
			/*  /X/ + /XX/////XXX// = /XXX/////XXX//  */
			string result9=Appointments.GetApptTimePatternFromProcPatterns(
				new List<string>() {  pattern04	/*  /X/  */
														, pattern11	/*  /XX/////XXX//  */
				});
			string error="";
			if(result1!="XX") {
				error+="\r\n	Time pattern result 1 not converted as expected.  Got: "+result1+"  Expected: XX";
			}
			if(result2!="/XX") {
				error+="\r\n	Time pattern result 2 not converted as expected.  Got: "+result2+"  Expected: /XX";
			}
			if(result3!="//XXXX/") {
				error+="\r\n	Time pattern result 3 not converted as expected.  Got: "+result3+"  Expected: //XXXX/";
			}
			if(result4!="XXX/") {
				error+="\r\n	Time pattern result 4 not converted as expected.  Got: "+result4+"  Expected: XXX/";
			}
			if(result5!="/XXXXXX/") {
				error+="\r\n	Time pattern result 5 not converted as expected.  Got: "+result5+"  Expected: /XXXXXX/";
			}
			if(result6!="/XXXXXXXX////") {
				error+="\r\n	Time pattern result 6 not converted as expected.  Got: "+result6+"  Expected: /XXXXXXXX////";
			}
			if(result7!="//XXXX/") {
				error+="\r\n	Time pattern result 7 not converted as expected.  Got: "+result7+"  Expected: //XXXX/";
			}
			if(result8!="//XXXX//") {
				error+="\r\n	Time pattern result 8 not converted as expected.  Got: "+result8+"  Expected: //XXXX//";
			}
			if(result9!="/XXX/////XXX//") {
				error+="\r\n	Time pattern result 8 not converted as expected.  Got: "+result9+"  Expected: /XXX/////XXX//";
			}
			//if(!string.IsNullOrEmpty(error)) {
			//	throw new Exception(error+"\r\n");
			//}
			Assert.IsFalse(!string.IsNullOrEmpty(error));
			//return suffix+": Passed.  Appointment procedure time patterns converted correctly.\r\n";
		}

		///<summary>Making sure that deductible estimates from both insurances are not applied to the same procedure when calculating deductibles for
		///subsequent procedures.</summary>
		[TestMethod]
		public void Legacy_TestEighty() {
			//if(specificTest!=0 && specificTest!=80) {
			//	return "";
			//}
			string suffix="80";
			Patient pat=PatientT.CreatePatient(suffix);
			//Add primary and secondary insurance with a $100 deductible
			Carrier carrier1=CarrierT.CreateCarrier(suffix+"_pri");
			InsPlan plan1=InsPlanT.CreateInsPlan(carrier1.CarrierNum);
			InsSub sub1=InsSubT.CreateInsSub(pat.PatNum,plan1.PlanNum);
			BenefitT.CreateDeductibleGeneral(plan1.PlanNum,BenefitCoverageLevel.Individual,100);
			BenefitT.CreateCategoryPercent(plan1.PlanNum,EbenefitCategory.General,80);
			PatPlanT.CreatePatPlan(1,pat.PatNum,sub1.InsSubNum);
			Carrier carrier2=CarrierT.CreateCarrier(suffix+"_sec");
			InsPlan plan2=InsPlanT.CreateInsPlan(carrier2.CarrierNum);
			InsSub sub2=InsSubT.CreateInsSub(pat.PatNum,plan2.PlanNum);
			BenefitT.CreateDeductibleGeneral(plan2.PlanNum,BenefitCoverageLevel.Individual,100);
			BenefitT.CreateCategoryPercent(plan2.PlanNum,EbenefitCategory.General,70);
			PatPlanT.CreatePatPlan(2,pat.PatNum,sub2.InsSubNum);
			//Add two procedures
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D2391",ProcStat.TP,"28",90);//Composite
			ProcedureT.SetPriority(proc1,1);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D2391",ProcStat.TP,"29",90);//Composite
			ProcedureT.SetPriority(proc2,1);
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			Family fam=Patients.GetFamily(pat.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(pat.PatNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<Procedure> ProcList=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP=Procedures.GetListTPandTPi(ProcList);//sorted by priority, then toothnum
			//Calculate estimates
			for(int i=0;i<ProcListTP.Length;i++) {
				Procedures.ComputeEstimates(ProcListTP[i],pat.PatNum,ref claimProcs,false,planList,patPlans,benefitList,
					histList,loopList,false,pat.Age,subList);
				//then, add this information to loopList so that the next procedure is aware of it.
				loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs,ProcListTP[i].ProcNum,ProcListTP[i].CodeNum));
			}
			//save changes in the list to the database
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			claimProcs=ClaimProcs.Refresh(pat.PatNum);
			//Validate the deductibles on the second procedure
			ClaimProc claimProc1=ClaimProcs.GetEstimate(claimProcs,proc2.ProcNum,plan1.PlanNum,sub1.InsSubNum);
			ClaimProc claimProc2=ClaimProcs.GetEstimate(claimProcs,proc2.ProcNum,plan2.PlanNum,sub2.InsSubNum);
			string retVal="";
			string errorMessage="";
			if(claimProc1.DedEst!=10) {
				errorMessage+="Deductible 1 was "+claimProc1.DedEst+". Should be 10.\r\n";
			}
			if(claimProc1.DedEst!=10) {
				errorMessage+="Deductible 2 was "+claimProc2.DedEst+". Should be 10.\r\n";
			}
			//if(errorMessage!="") {
			//	throw new Exception(errorMessage);
			//}
			Assert.AreEqual("",errorMessage);
			retVal+="80: Passed.  Dual insurance coverage with deductibles does not create a negative deductible.\r\n";
			//return retVal;
		}

		///<summary>Makes sure that if two claimprocs are created for the same frequency limitation group and one is marked received, the 
		///estimate claimproc will show as 0 coverage because the limitation is met.</summary>
		[TestMethod]
		public void Legacy_TestEightyOne() {
			//if(specificTest!=0 && specificTest != 81){
			//	return"";
			//}
			string suffix="81";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			long feeSchedNum1=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix);
			//Standard Fee
			FeeT.RefreshCache();
			long codeNum=ProcedureCodes.GetCodeNum("D0270");
			Fee fee=Fees.GetFee(codeNum,53,0,0);
			if(fee==null) {
				fee=new Fee();
				fee.CodeNum=codeNum;
				fee.FeeSched=53;
				fee.Amount=1200;
				Fees.Insert(fee);
			}
			else {
				fee.Amount=1200;
				Fees.Update(fee);
			}
			long codeNum2=ProcedureCodes.GetCodeNum("D0272");
			Fee fee2=Fees.GetFee(codeNum2,53,0,0);
			if(fee2==null) {
				fee2=new Fee();
				fee2.CodeNum=codeNum2;
				fee2.FeeSched=53;
				fee2.Amount=100;
				Fees.Insert(fee2);
			}
			else {
				fee2.Amount=100;
				Fees.Update(fee2);
			}
			//Carrier
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			long planNum1=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedNum1).PlanNum;
			InsSub sub1=InsSubT.CreateInsSub(pat.PatNum,planNum1);
			long subNum1=sub1.InsSubNum;
			//BW Frequency group - D0270, D0272, D0273, D0274
			BenefitT.CreateFrequencyProc(planNum1,"D0270",BenefitQuantity.Months,1);
			BenefitT.CreateFrequencyProc(planNum1,"D0272",BenefitQuantity.Months,1);
			PatPlanT.CreatePatPlan(1,patNum,subNum1);
			Procedure proc=ProcedureT.CreateProcedure(pat,"D0270",ProcStat.C,"",Fees.GetAmount0(codeNum,53),DateTime.Today.AddDays(-1));
			ClaimProcT.AddInsPaid(proc.PatNum,planNum1,proc.ProcNum,proc.ProcFee,subNum1,0,0,proc.ProcDate);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D0272",ProcStat.TP,"",Fees.GetAmount0(codeNum2,53));
			//Lists
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(patNum);
			Family fam=Patients.GetFamily(patNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(patNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=ClaimProcs.GetHistList(patNum,benefitList,patPlans,new List<InsPlan>() {InsPlans.GetPlan(planNum1,null)},proc2.ProcDate,subList);
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			//Validate
			ClaimProc claimProc;
			//Doesn't create a claimproc for the second one.
			Procedures.ComputeEstimates(proc2,patNum,ref claimProcs,false,planList,patPlans,benefitList,histList,loopList,true,pat.Age,subList);
			claimProcs=ClaimProcs.Refresh(patNum);
			claimProc=ClaimProcs.GetEstimate(claimProcs,proc2.ProcNum,planNum1,subNum1);
			//I don't think allowed can be easily tested on the fly, and it's not that important.
			//if(claimProc.InsEstTotal!=0) {
			//	throw new Exception("Estimate Should be 0. \r\n");
			//}
			Assert.AreEqual(0,claimProc.InsEstTotal);
			//return "81: Passed.  Claimproc has estimate of zero due to frequency limitation on a prior claimproc within the same group.\r\n";
		}

		///<summary>Makes sure that if two claimprocs are created for the same frequency limitation group and one is marked received, the 
		///estimate claimproc will show as 0 coverage because the limitation is met.</summary>
		[TestMethod]
		public void Legacy_TestEightyTwo() {
			//if(specificTest!=0 && specificTest != 82) {
			//	return "";
			//}
			string suffix="82";
			Patient patA=PatientT.CreatePatient(suffix+"a");
			Patient oldPat=patA.Copy();
			patA.Birthdate=new DateTime(1971,6,28);
			patA.WirelessPhone="5413635432";
			patA.Email="joe@schmoe.com";
			Patients.Update(patA,oldPat);
			Patient patB=PatientT.CreatePatient(suffix+"b");
			oldPat=patB.Copy();
			patB.Birthdate=new DateTime(2000,6,28);
			patB.HmPhone="5413631111";
			patB.Email="chuck@schmuck.com";
			patB.Guarantor=patA.PatNum;
			Patients.Update(patB,oldPat);
			Patient patA2=PatientT.CreatePatient(suffix+"a");
			oldPat=patA2.Copy();
			patA2.Birthdate=new DateTime(1971,6,28);
			patA2.WirelessPhone="5478982525";
			patA2.Email=patA.Email;
			Patients.Update(patA2,oldPat);
			//Match using email on family member's account
			List<long> listMatchingPatNums=Patients.GetPatNumsByNameBirthdayEmailAndPhone(patA.LName,patA.FName,patA.Birthdate,patB.Email,
				new List<string>());
			//if(listMatchingPatNums.Count != 1 || !listMatchingPatNums.Contains(patA.PatNum)) {
			//	throw new Exception("Match using email on family member's account. \r\n");
			//}
			Assert.IsFalse(listMatchingPatNums.Count != 1 || !listMatchingPatNums.Contains(patA.PatNum));
			//Match two patients based on name and birthdate
			listMatchingPatNums=Patients.GetPatNumsByNameBirthdayEmailAndPhone(patA.LName,patA.FName,patA.Birthdate,"",
				new List<string>());
			//if(listMatchingPatNums.Count != 2 || !listMatchingPatNums.Contains(patA.PatNum) || !listMatchingPatNums.Contains(patA2.PatNum)) {
			//	throw new Exception("Match two patients based on name and birthdate. \r\n");
			//}
			Assert.IsFalse(listMatchingPatNums.Count != 2 || !listMatchingPatNums.Contains(patA.PatNum) || !listMatchingPatNums.Contains(patA2.PatNum));
			//Match two patients based on email
			listMatchingPatNums=Patients.GetPatNumsByNameBirthdayEmailAndPhone(patA.LName,patA.FName,patA.Birthdate,patA.Email,
				new List<string>());
			//if(listMatchingPatNums.Count != 2 || !listMatchingPatNums.Contains(patA.PatNum) || !listMatchingPatNums.Contains(patA2.PatNum)) {
			//	throw new Exception("Match two patients based on email. \r\n");
			//}
			Assert.IsFalse(listMatchingPatNums.Count != 2 || !listMatchingPatNums.Contains(patA.PatNum) || !listMatchingPatNums.Contains(patA2.PatNum));
			//Match two patients based on phone numbers
			listMatchingPatNums=Patients.GetPatNumsByNameBirthdayEmailAndPhone(patA.LName,patA.FName,patA.Birthdate,"",
				new List<string> { patB.HmPhone,patA2.WirelessPhone });
			//if(listMatchingPatNums.Count != 2 || !listMatchingPatNums.Contains(patA.PatNum) || !listMatchingPatNums.Contains(patA2.PatNum)) {
			//	throw new Exception("Match two patients based on phone numbers. \r\n");
			//}
			Assert.IsFalse(listMatchingPatNums.Count != 2 || !listMatchingPatNums.Contains(patA.PatNum) || !listMatchingPatNums.Contains(patA2.PatNum));
			//Match one patient based on phone numbers
			listMatchingPatNums=Patients.GetPatNumsByNameBirthdayEmailAndPhone(patA.LName,patA.FName,patA.Birthdate,"",
				new List<string> { patA2.WirelessPhone });
			//if(listMatchingPatNums.Count != 1 || !listMatchingPatNums.Contains(patA2.PatNum)) {
			//	throw new Exception("Match one patient based on phone numbers. \r\n");
			//}
			Assert.IsFalse(listMatchingPatNums.Count != 1 || !listMatchingPatNums.Contains(patA2.PatNum));
			//Match one patient based on name and birthdate
			listMatchingPatNums=Patients.GetPatNumsByNameBirthdayEmailAndPhone(patB.LName,patB.FName,patB.Birthdate,"",
				new List<string>());
			//if(listMatchingPatNums.Count != 1 || !listMatchingPatNums.Contains(patB.PatNum)) {
			//	throw new Exception("Match one patient based on name and birthdate. \r\n");
			//}
			Assert.IsFalse(listMatchingPatNums.Count != 1 || !listMatchingPatNums.Contains(patB.PatNum));
			//return "82: Passed.  Patients properly matched by name, birthdate, phone number, and email.\r\n";
		}

		[TestMethod]
		public void Legacy_TestEightyThree() {
			//if(specificTest!=0 && specificTest!=83) {
			//	return"";
			//}
			string suffix="83";
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			long feeSchedPPO=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix);
			//Standard Fee
			FeeT.RefreshCache();
			long codeNum1=ProcedureCodes.GetCodeNum("D2740");
			Fee fee=Fees.GetFee(codeNum1,53,0,0);
			if(fee==null) {
				fee=new Fee();
				fee.CodeNum=codeNum1;
				fee.FeeSched=53;
				fee.Amount=2000;
				Fees.Insert(fee);
			}
			else {
				fee.Amount=2000;
				Fees.Update(fee);
			}
			//PPO fee
			fee=new Fee();
			fee.CodeNum=codeNum1;
			fee.FeeSched=feeSchedPPO;
			fee.Amount=900;
			Fees.Insert(fee);
			long codeNum2=ProcedureCodes.GetCodeNum("D2750");
			fee=Fees.GetFee(codeNum2,53,0,0);
			if(fee==null) {
				fee=new Fee();
				fee.CodeNum=codeNum2;
				fee.FeeSched=53;
				fee.Amount=1000;
				Fees.Insert(fee);
			}
			else {
				fee.Amount=1000;
				Fees.Update(fee);
			}
			//PPO fee
			fee=new Fee();
			fee.CodeNum=codeNum2;
			fee.FeeSched=feeSchedPPO;
			fee.Amount=500;
			Fees.Insert(fee);
			FeeT.RefreshCache();
			//Carrier
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			long planNum1=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedPPO).PlanNum;
			long planNum2=InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,feeSchedPPO).PlanNum;
			InsSub sub1=InsSubT.CreateInsSub(pat.PatNum,planNum1);
			long subNum1=sub1.InsSubNum;
			InsSub sub2=InsSubT.CreateInsSub(pat.PatNum,planNum2);
			long subNum2=sub2.InsSubNum;
			BenefitT.CreateAnnualMax(planNum1,1000);
			BenefitT.CreateDeductibleGeneral(planNum1,BenefitCoverageLevel.Individual,50);
			BenefitT.CreateCategoryPercent(planNum1,EbenefitCategory.Crowns,100);
			BenefitT.CreateAnnualMax(planNum2,250);
			BenefitT.CreateCategoryPercent(planNum2,EbenefitCategory.Crowns,100);
			PatPlanT.CreatePatPlan(1,patNum,subNum1);
			PatPlanT.CreatePatPlan(2,patNum,subNum2);
			Procedure proc2740=ProcedureT.CreateProcedure(pat,"D2740",ProcStat.TP,"7",Fees.GetAmount0(codeNum1,53));//crown on 7
			Procedure proc2750=ProcedureT.CreateProcedure(pat,"D2750",ProcStat.TP,"8",Fees.GetAmount0(codeNum2,53));//crown on 8
			//Lists
			List<Procedure> procedures=new List<Procedure>() { proc2740,proc2750 };
			List<ClaimProc> claimProcs=new List<ClaimProc>();
			Family fam=Patients.GetFamily(patNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(patNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			//Validate the estimates and write off amounts before creating a claim.
			string retVal="";
			Procedures.ComputeEstimates(proc2740,patNum,ref claimProcs,false,planList,patPlans,benefitList,histList,loopList,true,pat.Age,subList);
			Procedures.ComputeEstimates(proc2750,patNum,ref claimProcs,false,planList,patPlans,benefitList,histList,loopList,true,pat.Age,subList);
			ClaimProc claimProc2740Pri=ClaimProcs.GetEstimate(claimProcs,proc2740.ProcNum,planNum1,subNum1);
			ClaimProc claimProc2750Pri=ClaimProcs.GetEstimate(claimProcs,proc2750.ProcNum,planNum1,subNum1);
			//if(claimProc2740Pri.InsEstTotal!=850) {
			//	throw new Exception("claimProc2740 should have an insurance estimate of 900 prior to claim creation.\r\n");
			//}
			Assert.AreEqual(850,claimProc2740Pri.InsEstTotal);
			//if(claimProc2740Pri.WriteOffEst!=1100) {
			//	throw new Exception("claimProc2740 should have an write off estimate of 1100 prior to claim creation.\r\n");
			//}
			Assert.AreEqual(1100,claimProc2740Pri.WriteOffEst);
			//if(claimProc2750Pri.InsEstTotal!=450) {
			//	throw new Exception("claimProc2750 should have an insurance estimate of 500 prior to claim creation.\r\n");
			//}
			Assert.AreEqual(450,claimProc2750Pri.InsEstTotal);
			//if(claimProc2750Pri.WriteOffEst!=500) {
			//	throw new Exception("claimProc2750 should have an write off estimate of 500 prior to claim creation.\r\n");
			//}
			Assert.AreEqual(500,claimProc2750Pri.WriteOffEst);
			//Create a claim for the two procedures.
			Claim claim=ClaimT.CreateClaim("P",patPlans,planList,claimProcs,procedures,pat,procedures,benefitList,subList);

			//Running this next line fixes the WriteOffEst column somehow but it should already be correct via ClaimT.CreateClaim().
			//Claims.CalculateAndUpdate(procedures,planList,claim,patPlans,benefitList,pat.Age,subList);

			//Grab the new estimates, they should be corrected so that they know about eachother (annual max).
			claimProcs=ClaimProcs.Refresh(patNum);
			claimProc2740Pri=ClaimProcs.GetEstimate(claimProcs,proc2740.ProcNum,planNum1,subNum1);
			claimProc2750Pri=ClaimProcs.GetEstimate(claimProcs,proc2750.ProcNum,planNum1,subNum1);
			//if(claimProc2740Pri.InsEstTotal!=850) {
			//	throw new Exception("claimProc2740 should have an insurance estimate of 900 after claim creation.\r\n");
			//}
			Assert.AreEqual(850,claimProc2740Pri.InsEstTotal);
			//if(claimProc2740Pri.WriteOffEst!=1100) {
			//	throw new Exception("claimProc2740 should have an write off estimate of 1100 after claim creation.\r\n");
			//}
			Assert.AreEqual(1100,claimProc2740Pri.WriteOffEst);
			//if(claimProc2750Pri.InsEstTotal!=150) {
			//	throw new Exception("claimProc2750 should have an insurance estimate of 150 after claim creation.\r\n");
			//}
			Assert.AreEqual(150,claimProc2750Pri.InsEstTotal);
			//if(claimProc2750Pri.WriteOffEst!=500) {
			//	throw new Exception("claimProc2750 should have an write off estimate of 500 after claim creation.\r\n");
			//}
			Assert.AreEqual(500,claimProc2750Pri.WriteOffEst);
			retVal+="83: Passed.  Claim proc estimates and writeoffs are correct for multiple procedures on one claim that exceed primary annual max.\r\n";
			//return retVal;
		}

		[TestMethod]
		public void Legacy_TestEightyFour() {
			int thisTestNumber=84;
			//if(specificTest!=0 && specificTest!=thisTestNumber) {
			//	return "";
			//}
			string suffix=thisTestNumber.ToString();
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			long feeSchedPPO=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,suffix);
			//Standard Fee
			FeeT.RefreshCache();
			long codeNum1=ProcedureCodes.GetCodeNum("D2740");
			Fee fee=Fees.GetFee(codeNum1,53,0,0);
			if(fee==null) {
				fee=new Fee();
				fee.CodeNum=codeNum1;
				fee.FeeSched=53;
				fee.Amount=2000;
				Fees.Insert(fee);
			}
			else {
				fee.Amount=2000;
				Fees.Update(fee);
			}
			FeeT.RefreshCache();
			//Carrier
			Carrier carrier=CarrierT.CreateCarrier(suffix);
			long planNum1=InsPlanT.CreateInsPlan(carrier.CarrierNum).PlanNum;
			long planNum2=InsPlanT.CreateInsPlan(carrier.CarrierNum,EnumCobRule.CarveOut).PlanNum;
			InsSub sub1=InsSubT.CreateInsSub(pat.PatNum,planNum1);
			long subNum1=sub1.InsSubNum;
			InsSub sub2=InsSubT.CreateInsSub(pat.PatNum,planNum2);
			long subNum2=sub2.InsSubNum;
			BenefitT.CreateDeductibleGeneral(planNum2,BenefitCoverageLevel.Individual,50);
			BenefitT.CreateCategoryPercent(planNum2,EbenefitCategory.Crowns,80);
			PatPlanT.CreatePatPlan(1,patNum,subNum1);
			PatPlanT.CreatePatPlan(2,patNum,subNum2);
			Procedure proc2740=ProcedureT.CreateProcedure(pat,"D2740",ProcStat.C,"7",1200);//crown on 7
			List<Procedure> procedures=new List<Procedure>() { proc2740 };
			List<ClaimProc> claimProcs=new List<ClaimProc>();
			Family fam=Patients.GetFamily(patNum);
			List<InsSub> subList=InsSubs.RefreshForFam(fam);
			List<InsPlan> planList=InsPlans.RefreshForSubList(subList);
			List<PatPlan> patPlans=PatPlans.Refresh(patNum);
			List<Benefit> benefitList=Benefits.Refresh(patPlans,subList);
			List<ClaimProcHist> histList=new List<ClaimProcHist>();
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			string retVal="";
			Procedures.ComputeEstimates(proc2740,patNum,ref claimProcs,false,planList,patPlans,benefitList,histList,loopList,true,pat.Age,subList);
			//Create both claims for the procedure.
			Claim claim1=ClaimT.CreateClaim("P",patPlans,planList,claimProcs,procedures,pat,procedures,benefitList,subList);
			ClaimProcT.AddInsPaid(patNum,planNum1,proc2740.ProcNum,750,subNum1,0,0);
			Claims.CalculateAndUpdate(procedures,planList,claim1,patPlans,benefitList,pat.Age,subList);
			Claim claim2=ClaimT.CreateClaim("S",patPlans,planList,claimProcs,procedures,pat,procedures,benefitList,subList);
			claimProcs=ClaimProcs.Refresh(patNum);
			ClaimProc claimProc2740Sec=ClaimProcs.GetEstimate(claimProcs,proc2740.ProcNum,planNum2,subNum2);
			//Secondary InsEst = (Secondary Allowed - Secondary Deductible) * Secondary Percentage - PaidOther
			//170 = (1200 - 50) * .8 - 750
			//if(claimProc2740Sec.InsEstTotal != 170) {
			//	throw new Exception("claimProc2740 should have an insurance estimate of 170 after claim creation. Value is: "
			//		+claimProc2740Sec.InsEstTotal+"\r\n");
			//}
			Assert.AreEqual(170,claimProc2740Sec.InsEstTotal);
			//if(claimProc2740Sec.DedEst != 50) {
			//	throw new Exception("claimProc2740 should have a deductible estimate of 50 after claim creation. Value is: "
			//		+claimProc2740Sec.DedEst+"\r\n");
			//}
			Assert.AreEqual(50,claimProc2740Sec.DedEst);
			retVal+=thisTestNumber+": Passed.  Secondary insurance with carve out calculates correctly with deductible.\r\n";
			//return retVal;
		}

		private void OpenFormInvisibly(Form form) {
			//All this junk makes the window not visible when it loads.
			form.FormBorderStyle=FormBorderStyle.FixedToolWindow;
			form.ShowInTaskbar=false;
			form.StartPosition=FormStartPosition.Manual;
			form.Location=new System.Drawing.Point(-2000,-2000);
			form.Size=new System.Drawing.Size(1,1);
			form.Show();
		}

		[TestMethod]
		public void Legacy_TestEightyFive() {
			int thisTestNumber=85;
			//if(specificTest!=0 && specificTest!=thisTestNumber) {
			//	return "";
			//}
			string suffix=thisTestNumber.ToString();
			OpenDentBusiness.Program prog=Programs.GetProgram(Programs.GetProgramNum(ProgramName.Xcharge));
			prog.Path=@"C:\Program Files (x86)\X-Charge\XCharge.exe";
			prog.Enabled=true;
			Programs.Update(prog);
			Programs.RefreshCache();
			//Scenario 1: Pay plan due equals family balance which is less than repeat amount
			Patient pat=PatientT.CreatePatient(suffix);
			long patNum=pat.PatNum;
			PayPlan payPlan=PayPlanT.CreatePayPlan(patNum,550,100,DateTime.Today.AddMonths(-6),pat.PriProv);
			PaymentT.MakePayment(patNum,500,DateTime.Today.AddMonths(-3),payPlan.PayPlanNum);
			CreditCardT.CreateCard(patNum,100,DateTime.Today.AddMonths(-3),payPlan.PayPlanNum);
			//Scenario 2: Pay plan due with negative family balance
			suffix+="_";
			pat=PatientT.CreatePatient(suffix);
			patNum=pat.PatNum;
			payPlan=PayPlanT.CreatePayPlan(patNum,300,75,DateTime.Today.AddMonths(-8),pat.PriProv);
			PaymentT.MakePayment(patNum,225,DateTime.Today.AddMonths(-3),payPlan.PayPlanNum);
			PaymentT.MakePayment(patNum,200,DateTime.Today.AddMonths(-3));
			CreditCardT.CreateCard(patNum,75,DateTime.Today.AddMonths(-3),payPlan.PayPlanNum);
			//Scenario 3: Pay plan due less than family balance
			suffix+="_";
			pat=PatientT.CreatePatient(suffix);
			patNum=pat.PatNum;
			payPlan=PayPlanT.CreatePayPlan(patNum,250,50,DateTime.Today.AddMonths(-12),pat.PriProv);
			PaymentT.MakePayment(patNum,200,DateTime.Today.AddMonths(-3),payPlan.PayPlanNum);
			CreditCardT.CreateCard(patNum,100,DateTime.Today.AddMonths(-3),payPlan.PayPlanNum);
			ProcedureT.CreateProcedure(pat,"D0120",ProcStat.C,"0",100);
			//Scenario 4: Pay plan due more than repeat amount
			suffix+="_";
			pat=PatientT.CreatePatient(suffix);
			patNum=pat.PatNum;
			payPlan=PayPlanT.CreatePayPlan(patNum,250,50,DateTime.Today.AddMonths(-12),pat.PriProv);
			PaymentT.MakePayment(patNum,100,DateTime.Today.AddMonths(-3),payPlan.PayPlanNum);
			CreditCardT.CreateCard(patNum,50,DateTime.Today.AddMonths(-3),payPlan.PayPlanNum);
			//Test with PayPlanVersion 2--------------------------------------------------------------------------------------------------------
			Prefs.UpdateInt(PrefName.PayPlansVersion,2);
			Prefs.RefreshCache();
			Ledgers.ComputeAging(0,DateTime.Today);
			FormCreditRecurringCharges FormCRC=new FormCreditRecurringCharges();
			OpenFormInvisibly(FormCRC);			
			ODGrid gridCharges=(ODGrid)FormCRC.Controls.Find("gridMain",true)[0];
			int clinicOffset=Convert.ToInt32(PrefC.HasClinicsEnabled);
			int famBalIdx=3+clinicOffset;
			int payPlanDueIdx=4+clinicOffset;
			int totalDueIdx=5+clinicOffset;
			int chargeAmtIdx=7+clinicOffset;
			int curRow=0;
			List<int> listFailedTests=new List<int>();
			if(gridCharges.Rows[curRow].Cells[famBalIdx].Text.ToString() != 50.ToString("c")
				|| gridCharges.Rows[curRow].Cells[payPlanDueIdx].Text.ToString() != 50.ToString("c")
				|| gridCharges.Rows[curRow].Cells[totalDueIdx].Text.ToString() != 50.ToString("c")
				|| gridCharges.Rows[curRow].Cells[chargeAmtIdx].Text.ToString() != 50.ToString("c")) 
			{
				listFailedTests.Add(1);//PayPlanVersion 2 Scenario 1: Pay plan due equals family balance which is less than repeat amount
			}
			curRow++;
			if(gridCharges.Rows[curRow].Cells[famBalIdx].Text.ToString() != (-125).ToString("c")
				|| gridCharges.Rows[curRow].Cells[payPlanDueIdx].Text.ToString() != 75.ToString("c")
				|| gridCharges.Rows[curRow].Cells[totalDueIdx].Text.ToString() != 75.ToString("c")
				|| gridCharges.Rows[curRow].Cells[chargeAmtIdx].Text.ToString() != 75.ToString("c")) 
			{
				listFailedTests.Add(2);//PayPlanVersion 2 Scenario 2: Pay plan due with negative family balance
			}
			curRow++;
			if(gridCharges.Rows[curRow].Cells[famBalIdx].Text.ToString() != 150.ToString("c")
				|| gridCharges.Rows[curRow].Cells[payPlanDueIdx].Text.ToString() != 50.ToString("c")
				|| gridCharges.Rows[curRow].Cells[totalDueIdx].Text.ToString() != 150.ToString("c")
				|| gridCharges.Rows[curRow].Cells[chargeAmtIdx].Text.ToString() != 100.ToString("c")) 
			{
				listFailedTests.Add(3);//PayPlanVersion 2 Scenario 3: Pay plan due less than family balance
			}
			curRow++;
			if(gridCharges.Rows[curRow].Cells[famBalIdx].Text.ToString() != 150.ToString("c")
				|| gridCharges.Rows[curRow].Cells[payPlanDueIdx].Text.ToString() != 150.ToString("c")
				|| gridCharges.Rows[curRow].Cells[totalDueIdx].Text.ToString() != 150.ToString("c")
				|| gridCharges.Rows[curRow].Cells[chargeAmtIdx].Text.ToString() != 50.ToString("c")) 
			{
				listFailedTests.Add(4);//PayPlanVersion 2 Scenario 4: Pay plan due more than repeat amount
			}
			//Test with PayPlanVersion 1--------------------------------------------------------------------------------------------------------
			Prefs.UpdateInt(PrefName.PayPlansVersion,1);
			Prefs.RefreshCache();
			Ledgers.ComputeAging(0,DateTime.Today);
			FormCRC=new FormCreditRecurringCharges();
			//All this junk is to make the window not visible when it loads.
			OpenFormInvisibly(FormCRC);
			gridCharges=(ODGrid)FormCRC.Controls.Find("gridMain",true)[0];
			curRow=0;
			if(gridCharges.Rows[curRow].Cells[famBalIdx].Text.ToString() != 0.ToString("c")
				|| gridCharges.Rows[curRow].Cells[payPlanDueIdx].Text.ToString() != 50.ToString("c")
				|| gridCharges.Rows[curRow].Cells[totalDueIdx].Text.ToString() != 50.ToString("c")
				|| gridCharges.Rows[curRow].Cells[chargeAmtIdx].Text.ToString() != 50.ToString("c")) 
			{
				listFailedTests.Add(5);//PayPlanVersion 1 Scenario 1: Pay plan due equals family balance which is less than repeat amount
			}
			curRow++;
			if(gridCharges.Rows[curRow].Cells[famBalIdx].Text.ToString() != (-200).ToString("c")
				|| gridCharges.Rows[curRow].Cells[payPlanDueIdx].Text.ToString() != 75.ToString("c")
				|| gridCharges.Rows[curRow].Cells[totalDueIdx].Text.ToString() != 75.ToString("c")
				|| gridCharges.Rows[curRow].Cells[chargeAmtIdx].Text.ToString() != 75.ToString("c")) 
			{
				listFailedTests.Add(6);//PayPlanVersion 1 Scenario 2: Pay plan due with negative family balance
			}
			curRow++;
			if(gridCharges.Rows[curRow].Cells[famBalIdx].Text.ToString() != 100.ToString("c")
				|| gridCharges.Rows[curRow].Cells[payPlanDueIdx].Text.ToString() != 50.ToString("c")
				|| gridCharges.Rows[curRow].Cells[totalDueIdx].Text.ToString() != 150.ToString("c")
				|| gridCharges.Rows[curRow].Cells[chargeAmtIdx].Text.ToString() != 100.ToString("c")) 
			{
				listFailedTests.Add(7);//PayPlanVersion 1 Scenario 3: Pay plan due less than family balance
			}
			curRow++;
			if(gridCharges.Rows[curRow].Cells[famBalIdx].Text.ToString() != 0.ToString("c")
				|| gridCharges.Rows[curRow].Cells[payPlanDueIdx].Text.ToString() != 150.ToString("c")
				|| gridCharges.Rows[curRow].Cells[totalDueIdx].Text.ToString() != 150.ToString("c")
				|| gridCharges.Rows[curRow].Cells[chargeAmtIdx].Text.ToString() != 50.ToString("c")) 
			{
				listFailedTests.Add(8);//PayPlanVersion 1 Scenario 4: Pay plan due more than repeat amount
			}
			//if(listFailedTests.Count>0) {
			//	throw new Exception("Recurring charge tests "+string.Join(",",listFailedTests)+" failed.\r\n");
			//}
			Assert.AreEqual(0,listFailedTests.Count);
			//return thisTestNumber+": Passed.  Recurring charges for payment plans.\r\n";
		}

	}
}
