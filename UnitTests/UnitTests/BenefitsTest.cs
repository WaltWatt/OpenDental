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
	public class BenefitsTest:TestBase {

		///<summary>Tests that a completed procedure that has a deductible for a specific code reduces the deductible for a code that applies to the
		///general deductible.</summary>
		[TestMethod]
		public void Benefits_GetDeductibleByCode_DeductLessThanGeneral1() {
			GetDeductibleByCodeDeductLessThanGeneral(MethodBase.GetCurrentMethod().Name,
				(claimProcD2750,claimProcD0220) => {
					Assert.AreEqual(20,claimProcD2750.DedEst);
				} 
			);
		}
		
		///<summary>Creates a general deductible of $50, a deductible of $50 on D0220, sets a $30 D0220 complete and creates a claim, 
		///creates a $100 D2750, that is TP'ed, and then creates a $30 D0220 that is TP'ed.</summary>
		///<param name="actAssert">The first claimproc is for the D2750 and the second claimproc is for the second D0220.</param>
		public void GetDeductibleByCodeDeductLessThanGeneral(string suffix,Action<ClaimProc,ClaimProc> actAssert) {
			Patient pat=PatientT.CreatePatient(suffix);
			InsuranceT.AddInsurance(pat,suffix);
			List<InsSub> listSubs=InsSubT.GetInsSubs(pat);
			List<InsPlan> listPlans=InsPlans.RefreshForSubList(listSubs);
			List<PatPlan> listPatPlans=PatPlans.Refresh(pat.PatNum);
			InsPlan plan=InsPlanT.GetPlanForPriSecMed(PriSecMed.Primary,listPatPlans,listPlans,listSubs);
			BenefitT.CreateDeductibleGeneral(plan.PlanNum,BenefitCoverageLevel.Individual,50);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Diagnostic,100);
			BenefitT.CreateCategoryPercent(plan.PlanNum,EbenefitCategory.Crowns,50);
			BenefitT.CreateDeductible(plan.PlanNum,EbenefitCategory.Diagnostic,0);
			BenefitT.CreateDeductible(plan.PlanNum,"D0220",50);
			List<Benefit> listBens=Benefits.Refresh(listPatPlans,listSubs);
			Procedure proc1=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.C,"",30);//proc1 - Intraoral - periapical first film
			ClaimT.CreateClaim("P",listPatPlans,listPlans,new List<ClaimProc>(),new List<Procedure> { proc1 },pat,new List<Procedure> { proc1 },listBens,
				listSubs);
			Procedure proc2=ProcedureT.CreateProcedure(pat,"D2750",ProcStat.TP,"",100,priority:0);//proc2 - Crown
			Procedure proc3=ProcedureT.CreateProcedure(pat,"D0220",ProcStat.TP,"",30,priority:1);//proc3 - Intraoral - periapical first film
			List<ClaimProc> claimProcs=ProcedureT.ComputeEstimates(pat,listPatPlans,listPlans,listSubs,listBens);
			ClaimProc claimProc2=claimProcs.FirstOrDefault(x => x.ProcNum==proc2.ProcNum);
			ClaimProc claimProc3=claimProcs.FirstOrDefault(x => x.ProcNum==proc3.ProcNum);
			actAssert(claimProc2,claimProc3);
		}

		///<summary>Creates an ortho procedure with an age limitation and a lifetime max and calculates the insurance estimate.</summary>
		[TestMethod]
		public void Benefits_UnderAgeWithLifetimeMax() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix,birthDate: DateTime.Now.AddYears(-13));
			InsuranceInfo ins=InsuranceT.AddInsurance(pat,suffix);
			BenefitT.CreateCategoryPercent(ins.PrimaryInsPlan.PlanNum,EbenefitCategory.Orthodontics,50);
			BenefitT.CreateAgeLimitation(ins.PrimaryInsPlan.PlanNum,EbenefitCategory.Orthodontics,18,coverageLevel:BenefitCoverageLevel.Individual);
			BenefitT.CreateOrthoMax(ins.PrimaryInsPlan.PlanNum,1000);
			ins.RefreshBenefits();
			Procedure proc=ProcedureT.CreateProcedure(pat,"D8090",ProcStat.TP,"",3000);//comprehensive orthodontic treatment
			List<ClaimProc> listClaimProcs=ProcedureT.ComputeEstimates(pat,ins);
			Assert.AreEqual(1000,listClaimProcs.First().InsEstTotal);
		}

		///<summary>Creates an ortho procedure with an age limitation and a lifetime max and calculates the insurance estimate.</summary>
		[TestMethod]
		public void Benefits_OverAgeWithLifetimeMax() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			Patient pat=PatientT.CreatePatient(suffix,birthDate: DateTime.Now.AddYears(-20));
			InsuranceInfo ins=InsuranceT.AddInsurance(pat,suffix);
			BenefitT.CreateCategoryPercent(ins.PrimaryInsPlan.PlanNum,EbenefitCategory.Orthodontics,50);
			BenefitT.CreateAgeLimitation(ins.PrimaryInsPlan.PlanNum,EbenefitCategory.Orthodontics,18);
			BenefitT.CreateOrthoMax(ins.PrimaryInsPlan.PlanNum,1000);
			ins.RefreshBenefits();
			Procedure proc=ProcedureT.CreateProcedure(pat,"D8090",ProcStat.TP,"",3000);//comprehensive orthodontic treatment
			List<ClaimProc> listClaimProcs=ProcedureT.ComputeEstimates(pat,ins);
			Assert.AreEqual(0,listClaimProcs.First().InsEstTotal);
		}
	}
}
