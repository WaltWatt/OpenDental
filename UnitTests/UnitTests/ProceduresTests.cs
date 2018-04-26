using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using UnitTestsCore;

namespace UnitTests {
	[TestClass]
	public class ProceduresTest:TestBase {
		
		#region Medicaid COB
		///<summary>Secondary insurance has a COB rule of Medicaid. Tests that secondary writeoff is correct.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_MedicaidCOBSecWO1() {
			ComputeEstimatesMedicaidCOB(MethodBase.GetCurrentMethod().Name,100,70,20,50,100,
				((claimProcPri,claimProcSec,proc) => {
					Assert.AreEqual(35,claimProcSec.WriteOffEst);
				})
			);
		}

		///<summary>Secondary insurance has a COB rule of Medicaid. Tests that patient portion is correct.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_MedicaidCOBPatPort1() {
			ComputeEstimatesMedicaidCOB(MethodBase.GetCurrentMethod().Name,100,70,20,50,100,
				((claimProcPri,claimProcSec,proc) => {
					double patPort=proc.ProcFee*(proc.BaseUnits+proc.UnitQty)-claimProcPri.InsEstTotal-claimProcPri.WriteOffEst
						-claimProcSec.InsEstTotal-claimProcSec.WriteOffEst;
					Assert.AreEqual(0,patPort);
				})
			);
		}

		///<summary>Secondary insurance has a COB rule of Medicaid. Tests that secondary writeoff is correct.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_MedicaidCOBSecWO2() {
			ComputeEstimatesMedicaidCOB(MethodBase.GetCurrentMethod().Name,100,40,30,50,100,
				((claimProcPri,claimProcSec,proc) => {
					Assert.AreEqual(10,claimProcSec.WriteOffEst);
				})
			);
		}

		///<summary>Secondary insurance has a COB rule of Medicaid. Tests that secondary ins pay is correct.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_MedicaidCOBSecInsPay2() {
			ComputeEstimatesMedicaidCOB(MethodBase.GetCurrentMethod().Name,100,40,30,50,100,
				((claimProcPri,claimProcSec,proc) => {
					Assert.AreEqual(10,claimProcSec.InsPayEst);
				})
			);
		}

		///<summary>Secondary insurance has a COB rule of Medicaid. Tests that patient portion is correct.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_MedicaidCOBPatPort2() {
			ComputeEstimatesMedicaidCOB(MethodBase.GetCurrentMethod().Name,100,40,30,50,100,
				((claimProcPri,claimProcSec,proc) => {
					double patPort=proc.ProcFee*(proc.BaseUnits+proc.UnitQty)-claimProcPri.InsPayEst-claimProcPri.WriteOffEst
						-claimProcSec.InsPayEst-claimProcSec.WriteOffEst;
					Assert.AreEqual(0,patPort);
				})
			);
		}

		///<summary>Creates a procedure and computes estimates for a patient where the secondary insurance has a COB rule of Medicaid.</summary>
		private void ComputeEstimatesMedicaidCOB(string suffix,double procFee,double priAllowed,double secAllowed,int priPercentCovered,
			int secPercentCovered,Action<ClaimProc/*Primary*/,ClaimProc/*Secondary*/,Procedure> assertAct) 
		{
			Patient pat=PatientT.CreatePatient(suffix);
			long ppoFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"PPO "+suffix);
			InsuranceT.AddInsurance(pat,suffix,"p",ppoFeeSchedNum);
			long medicaidFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"PPO "+suffix);
			InsuranceT.AddInsurance(pat,suffix,"p",medicaidFeeSchedNum,2,cobRule: EnumCobRule.SecondaryMedicaid);
			List<InsSub> listSubs=InsSubT.GetInsSubs(pat);
			List<InsPlan> listPlans=InsPlans.RefreshForSubList(listSubs);
			List<PatPlan> listPatPlans=PatPlans.Refresh(pat.PatNum);
			InsPlan priPlan=InsPlanT.GetPlanForPriSecMed(PriSecMed.Primary,listPatPlans,listPlans,listSubs);
			BenefitT.CreateCategoryPercent(priPlan.PlanNum,EbenefitCategory.Diagnostic,priPercentCovered);
			InsPlan secPlan=InsPlanT.GetPlanForPriSecMed(PriSecMed.Secondary,listPatPlans,listPlans,listSubs);
			BenefitT.CreateCategoryPercent(secPlan.PlanNum,EbenefitCategory.Diagnostic,secPercentCovered);
			List<Benefit> listBens=Benefits.Refresh(listPatPlans,listSubs);
			string procStr="D0150";
			Procedure proc=ProcedureT.CreateProcedure(pat,procStr,ProcStat.TP,"",procFee);
			ProcedureCode procCode=ProcedureCodes.GetProcCode(procStr);
			FeeT.CreateFee(ppoFeeSchedNum,procCode.CodeNum,priAllowed);
			FeeT.CreateFee(medicaidFeeSchedNum,procCode.CodeNum,secAllowed);
			Procedures.ComputeEstimates(proc,pat.PatNum,new List<ClaimProc>(),false,listPlans,listPatPlans,listBens,pat.Age,listSubs);
			List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			assertAct(listClaimProcs.FirstOrDefault(x => x.PlanNum==priPlan.PlanNum),listClaimProcs.FirstOrDefault(x => x.PlanNum==secPlan.PlanNum),proc);
		}

		#endregion Medicaid COB

		#region Fixed Benefits

		///<summary>.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_FixedBenefitNoFixedBenefitFeeAmtNoPpoFee() {
			ComputeEstimatesFixedBenefits(MethodBase.GetCurrentMethod().Name,100,-1,0,-1,false,0,
				((assertItem) => {
					Assert.AreEqual(0,assertItem.PrimaryClaimProc.InsEstTotal);
					Assert.AreEqual(100,assertItem.PrimaryClaimProc.CopayAmt);
					Assert.AreEqual(0,assertItem.PrimaryClaimProc.WriteOffEst);
				})
			);
		}

		///<summary>.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_FixedBenefitNoFixedBenefitFeeAmt() {
			ComputeEstimatesFixedBenefits(MethodBase.GetCurrentMethod().Name,100,55,0,-1,false,0,
				((assertItem) => {
					Assert.AreEqual(0,assertItem.PrimaryClaimProc.InsEstTotal);
					Assert.AreEqual(55,assertItem.PrimaryClaimProc.CopayAmt);
					Assert.AreEqual(45,assertItem.PrimaryClaimProc.WriteOffEst);
				})
			);
		}

		///<summary>.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_FixedBenefitNoPpoFee() {
			ComputeEstimatesFixedBenefits(MethodBase.GetCurrentMethod().Name,100,-1,12,-1,false,0,
				((assertItem) => {
					Assert.AreEqual(12,assertItem.PrimaryClaimProc.InsEstTotal);
					Assert.AreEqual(88,assertItem.PrimaryClaimProc.CopayAmt);
					Assert.AreEqual(0,assertItem.PrimaryClaimProc.WriteOffEst);
				})
			);
		}

		///<summary>.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_FixedBenefitWithAllFees() {
			ComputeEstimatesFixedBenefits(MethodBase.GetCurrentMethod().Name,100,55,12,-1,false,0,
				((assertItem) => {
					Assert.AreEqual(12,assertItem.PrimaryClaimProc.InsEstTotal);
					Assert.AreEqual(43,assertItem.PrimaryClaimProc.CopayAmt);
					Assert.AreEqual(45,assertItem.PrimaryClaimProc.WriteOffEst);
				})
			);
		}

		///<summary>.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_FixedBenefitWithPercentOverride() {
			ComputeEstimatesFixedBenefits(MethodBase.GetCurrentMethod().Name,100,55,12,25,false,0,
				((assertItem) => {
					Assert.AreEqual(3,assertItem.PrimaryClaimProc.InsEstTotal);
					Assert.AreEqual(43,assertItem.PrimaryClaimProc.CopayAmt);
					Assert.AreEqual(45,assertItem.PrimaryClaimProc.WriteOffEst);
				})
			);
		}

		///<summary>.</summary>
		[TestMethod]
		public void Procedures_ComputeEstimates_FixedBenefitWithSecondaryIns() {
			ComputeEstimatesFixedBenefits(MethodBase.GetCurrentMethod().Name,100,55,12,-1,true,15,
				((assertItem) => {
					Assert.AreEqual(12,assertItem.PrimaryClaimProc.InsEstTotal);
					Assert.AreEqual(43,assertItem.PrimaryClaimProc.CopayAmt);
					Assert.AreEqual(45,assertItem.PrimaryClaimProc.WriteOffEst);
					Assert.AreEqual(43,assertItem.SecondaryClaimProc.InsEstTotal);
				})
			);
		}

		///<summary>Creates a procedure and computes estimates for a patient where the secondary insurance has a COB rule of Medicaid.</summary>
		private void ComputeEstimatesFixedBenefits(string suffix,double procFee,double ppoFee,double fixedBenefitFee
			,int priPercentCoveredOverride,bool hasSecondary,double secFee,Action<FixedBenefitAssertItem> assertAct)
		{
			Patient pat=PatientT.CreatePatient(suffix);
			string procStr="D0150";
			Procedure proc=ProcedureT.CreateProcedure(pat,procStr,ProcStat.TP,"",procFee);
			ProcedureCode procCode=ProcedureCodes.GetProcCode(procStr);
			long ppoFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"PPO "+suffix);
			long catPercFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"Category % "+suffix);
			long fixedBenefitFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.FixedBenefit,"Fixed Benefit "+suffix);
			if(ppoFee>-1) {
				FeeT.CreateFee(ppoFeeSchedNum,procCode.CodeNum,ppoFee);
			}
			FeeT.CreateFee(fixedBenefitFeeSchedNum,procCode.CodeNum,fixedBenefitFee);
			InsuranceT.AddInsurance(pat,suffix,"p",ppoFeeSchedNum,copayFeeSchedNum: fixedBenefitFeeSchedNum);
			if(hasSecondary) {
				FeeT.CreateFee(catPercFeeSchedNum,procCode.CodeNum,secFee);
				InsuranceT.AddInsurance(pat,suffix,"",catPercFeeSchedNum,2,false,EnumCobRule.Standard);
			}
			List<InsSub> listSubs=InsSubT.GetInsSubs(pat);
			List<InsPlan> listPlans=InsPlans.RefreshForSubList(listSubs);
			List<PatPlan> listPatPlans=PatPlans.Refresh(pat.PatNum);
			InsPlan priPlan=InsPlanT.GetPlanForPriSecMed(PriSecMed.Primary,listPatPlans,listPlans,listSubs);
			InsPlan secPlan=null;
			if(hasSecondary) {
				secPlan=InsPlanT.GetPlanForPriSecMed(PriSecMed.Secondary,listPatPlans,listPlans,listSubs);
				//TODO:  Add diagnostic code benefit for 100%
				BenefitT.CreateCategoryPercent(secPlan.PlanNum,EbenefitCategory.Diagnostic,100);
			}
			List<Benefit> listBens=Benefits.Refresh(listPatPlans,listSubs);
			Procedures.ComputeEstimates(proc,pat.PatNum,new List<ClaimProc>(),false,listPlans,listPatPlans,listBens,pat.Age,listSubs);
			List<ClaimProc> listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			if(priPercentCoveredOverride>0) {
				foreach(ClaimProc cpCur in listClaimProcs) {
					cpCur.PercentOverride=priPercentCoveredOverride;
					ClaimProcs.Update(cpCur);
				}
				Procedures.ComputeEstimates(proc,pat.PatNum,new List<ClaimProc>(),false,listPlans,listPatPlans,listBens,pat.Age,listSubs);
				listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			}
			foreach(ClaimProc cpCur in listClaimProcs) {
				cpCur.PercentOverride=priPercentCoveredOverride;
				ClaimProcs.Update(cpCur);
			}
			Procedures.ComputeEstimates(proc,pat.PatNum,listClaimProcs,false,listPlans,listPatPlans,listBens,pat.Age,listSubs);
			listClaimProcs=ClaimProcs.Refresh(pat.PatNum);
			assertAct(new FixedBenefitAssertItem() {
				Procedure=proc,
				PrimaryClaimProc=listClaimProcs.FirstOrDefault(x => x.PlanNum==priPlan.PlanNum),
				SecondaryClaimProc=secPlan==null ? null : listClaimProcs.FirstOrDefault(x => x.PlanNum==secPlan.PlanNum),
			});
		}

		private class FixedBenefitAssertItem {
			public Procedure Procedure;
			public ClaimProc PrimaryClaimProc;
			public ClaimProc SecondaryClaimProc;
		}

		#endregion

		#region GetProcFee

		///<summary>The procedure fee for a medical insurance ppo is the UCR fee.</summary>
		[TestMethod]
		public void Procedures_GetProcFee_WithMedicalInsurance() {
			double procFee=GetProcFee(MethodBase.GetCurrentMethod().Name,false);
			Assert.AreEqual(300,procFee);
		}

		///<summary>The procedure fee for a medical insurance ppo is the UCR fee of the medical code.</summary>
		[TestMethod]
		public void Procedures_GetProcFee_WithMedicalCode() {
			double procFee=GetProcFee(MethodBase.GetCurrentMethod().Name,true);
			Assert.AreEqual(300,procFee);
		}

		///<summary>Creates a procedure and returns its procedure fee.</summary>
		private double GetProcFee(string suffix,bool doUseMedicalCode) {
			Prefs.UpdateBool(PrefName.InsPpoAlwaysUseUcrFee,true);
			Prefs.UpdateBool(PrefName.MedicalFeeUsedForNewProcs,doUseMedicalCode);
			Patient pat=PatientT.CreatePatient(suffix);
			long ucrFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"UCR "+suffix);
			FeeSchedT.UpdateUCRFeeSched(pat,ucrFeeSchedNum);
			long ppoFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"PPO "+suffix);
			InsuranceT.AddInsurance(pat,suffix,"p",ppoFeeSchedNum,1,true);
			List<InsSub> listSubs=InsSubT.GetInsSubs(pat);
			List<InsPlan> listPlans=InsPlans.RefreshForSubList(listSubs);
			List<PatPlan> listPatPlans=PatPlans.Refresh(pat.PatNum);
			string procStr="D0150";
			string procStrMed="D0120";
			ProcedureCode procCode=ProcedureCodes.GetProcCode(procStr);
			ProcedureCode procCodeMed=ProcedureCodes.GetProcCode(procStrMed);
			procCode.MedicalCode=procCodeMed.ProcCode;
			FeeT.CreateFee(ucrFeeSchedNum,procCode.CodeNum,300);
			FeeT.CreateFee(ppoFeeSchedNum,procCode.CodeNum,120);
			FeeT.CreateFee(ucrFeeSchedNum,procCodeMed.CodeNum,175);
			FeeT.CreateFee(ppoFeeSchedNum,procCodeMed.CodeNum,85);
			Procedure proc=ProcedureT.CreateProcedure(pat,procStr,ProcStat.TP,"",300);
			return Procedures.GetProcFee(pat,listPatPlans,listSubs,listPlans,procCode.CodeNum,proc.ProvNum,proc.ClinicNum,procCode.MedicalCode);
		}

		#endregion GetProcFee

		///<sumary>Tests that frequency limitations are computed correctly for toothnum, tooth range, and surface. This method assumes that, when checking for frequencies,
		///a past procedure has been found that falls in the frequency date range, has the same patnum, and the same insubnum as the current procedure. This is to simplify 
		///the unit test and avoid tedious set up.</sumary>
		#region Frequency Limitations
		[TestMethod]
		public void Procedures_IsSameProcedureArea_SameToothNums() {
			Assert.IsTrue(Procedures.IsSameProcedureArea("2","2","","","",""));
		}
		
		[TestMethod]
		public void Procedures_IsSameProcedureArea_DifferentToothNums() {
			Assert.IsFalse(Procedures.IsSameProcedureArea("","1","","","",""));
		}
		
		[TestMethod]
		public void Procedures_IsSameProcedureArea_PartialMatchToothNums() {
			Assert.IsFalse(Procedures.IsSameProcedureArea("22","2","","","",""));
		}
		
		[TestMethod]
		public void Procedures_IsSameProcedureArea_EmptyProcCurToothRange() {
			Assert.IsFalse(Procedures.IsSameProcedureArea("","","2,4,7","","",""));
		}
		
		[TestMethod]
		public void Procedures_IsSameProcedureArea_PartialMatchToothRanges() {
			Assert.IsFalse(Procedures.IsSameProcedureArea("","","7,16,22","2","",""));
		}
		
		[TestMethod]
		public void Procedures_IsSameProcedureArea_ToothRangeMatchingTails() {
			Assert.IsTrue(Procedures.IsSameProcedureArea("","","7,3","15,7,3","",""));
		}
		
		[TestMethod]
		public void Procedures_IsSameProcedureArea_ToothRangeMatchingElementsOutOfOrder() {
			Assert.IsTrue(Procedures.IsSameProcedureArea("","","15,7,3,4","7,15","",""));
		}

		[TestMethod]
		public void Procedures_IsSameProcedureArea_OneToothRangeBlank() {
			Assert.IsFalse(Procedures.IsSameProcedureArea("","","1,4","","",""));
		}

		[TestMethod]
		public void Procedures_IsSameProcedureArea_InvalidToothRangeEntry() {
			Assert.IsFalse(Procedures.IsSameProcedureArea("","","1,,5,","","",""));
		}

		[TestMethod]
		public void Procedures_IsSameProcedureArea_InvalidEntryWithMatch() {
			Assert.IsTrue(Procedures.IsSameProcedureArea("","","1,,4","4","",""));
		}
		
		[TestMethod]
		public void Procedures_IsSameProcedureArea_SameToothNumEmptySurf() {
			Assert.IsFalse(Procedures.IsSameProcedureArea("4","4","","","","M"));
		}
		
		[TestMethod]
		public void Procedures_IsSameProcedureArea_SameToothNumSameSurf() {
			Assert.IsTrue(Procedures.IsSameProcedureArea("9","9","","","L","L"));
		}
		
		[TestMethod]
		public void Procedures_IsSameProcedureArea_SameToothNumPartialSurfMatch() {
			Assert.IsFalse(Procedures.IsSameProcedureArea("5","5","","","ML","M"));
		}
		
		[TestMethod]
		public void Procedures_IsSameProcedureArea_SameToothNumEmptySurfs() {
			Assert.IsTrue(Procedures.IsSameProcedureArea("14","14","","","",""));
		}

		[TestMethod]
		public void Procedures_IsSameProcedureArea_HasNoArea() {
			//E.g. exams and BW's will not have any 'procedure areas' and should return true
			Assert.IsTrue(Procedures.IsSameProcedureArea("","","","","",""));
		}
		#endregion
	}

}

