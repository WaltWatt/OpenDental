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
	public class InsPlansTests:TestBase {

		private static List<ProcedureCode> _listProcCodes;

		[ClassInitialize]
		public static void SetUp(TestContext context) {
			_listProcCodes=ProcedureCodes.GetAllCodes();
		}

		/// <summary>Get the copay value for when there is no patient copay</summary>
		[TestMethod]
		public void InsPlans_GetCopay_Blank() {
			ProcedureCode procCode=_listProcCodes[0];
			InsPlan plan=GenerateMediFlatInsPlan(MethodBase.GetCurrentMethod().Name);
			double amt=InsPlans.GetCopay(procCode.CodeNum,plan.FeeSched,plan.CopayFeeSched,plan.CodeSubstNone,"",0,0);
			Assert.AreEqual(-1,amt);
		}

		///<summary>Get the copay amount when there is no exact fee on the copay schedule but there is a fee in the default schedule.</summary>
		[TestMethod]
		public void InsPlans_GetCopay_NoExactFeeUseDefault() {
			ProcedureCode procCode=_listProcCodes[1];
			InsPlan plan=GenerateMediFlatInsPlan(MethodBase.GetCurrentMethod().Name);
			Fee feeDefault=FeeT.GetNewFee(plan.FeeSched,procCode.CodeNum,25);
			Prefs.UpdateBool(PrefName.CoPay_FeeSchedule_BlankLikeZero,false);
			double amt=InsPlans.GetCopay(procCode.CodeNum,plan.FeeSched,plan.CopayFeeSched,plan.CodeSubstNone,"",0,0);
			Assert.AreEqual(feeDefault.Amount,amt);
		}

		///<summary>Get the copay amount where there is no exact fee and the Preference CoPay_FeeSchedule_BlankLikeZero is true.</summary>
		[TestMethod]
		public void InsPlans_GetCopay_NoExactFeeUseZero() {
			ProcedureCode procCode=_listProcCodes[2];
			InsPlan plan=GenerateMediFlatInsPlan(MethodBase.GetCurrentMethod().Name);
			Fee feeDefault=FeeT.GetNewFee(plan.FeeSched,procCode.CodeNum,35);
			Prefs.UpdateBool(PrefName.CoPay_FeeSchedule_BlankLikeZero,true);
			double amt=InsPlans.GetCopay(procCode.CodeNum,plan.FeeSched,plan.CopayFeeSched,plan.CodeSubstNone,"",0,0);
			Assert.AreEqual(-1,amt);
			Prefs.UpdateBool(PrefName.CoPay_FeeSchedule_BlankLikeZero,false);
		}

		///<summary>Get the copay value for when there is no substitute fee and the exact copay fee exists.</summary>
		[TestMethod]
		public void InsPlans_GetCopay_ExactFee() {
			ProcedureCode procCode=_listProcCodes[3];
			InsPlan plan=GenerateMediFlatInsPlan(MethodBase.GetCurrentMethod().Name);
			Fee feeDefault=FeeT.GetNewFee(plan.FeeSched,procCode.CodeNum,50);
			Fee feeCopay=FeeT.GetNewFee(plan.CopayFeeSched,procCode.CodeNum,15);
			double amt=InsPlans.GetCopay(procCode.CodeNum,plan.FeeSched,plan.CopayFeeSched,plan.CodeSubstNone,"",0,0);
			Assert.AreEqual(feeCopay.Amount,amt);
		}

		///<summary>Get the copay value for when there is a substitute fee.</summary>
		[TestMethod]
		public void InsPlans_GetCopay_SubstituteFee() {
			ProcedureCode procCode=_listProcCodes[4];
			procCode.SubstitutionCode=_listProcCodes[5].ProcCode;
			ProcedureCodes.Update(procCode);
			InsPlan plan=GenerateMediFlatInsPlan(MethodBase.GetCurrentMethod().Name,false);
			Fee feeDefault=FeeT.GetNewFee(plan.FeeSched,procCode.CodeNum,100);
			Fee feeSubstitute=FeeT.GetNewFee(plan.FeeSched,ProcedureCodes.GetSubstituteCodeNum(procCode.ProcCode,""),45);
			double amt=InsPlans.GetCopay(procCode.CodeNum,plan.FeeSched,plan.CopayFeeSched,plan.CodeSubstNone,"",0,0);
			Assert.AreEqual(feeSubstitute.Amount,amt);
		}

		///<summary>Get the allowed amount for the procedure code for the PPO plan.</summary>
		[TestMethod]
		public void InsPlans_GetAllowed_PPOExact() {
			InsPlan plan=GeneratePPOPlan(MethodBase.GetCurrentMethod().Name);
			ProcedureCode procCode=_listProcCodes[6];
			Fee fee=FeeT.GetNewFee(plan.FeeSched,procCode.CodeNum,65);
			double allowed=InsPlans.GetAllowed(procCode.ProcCode,plan.FeeSched,plan.AllowedFeeSched,plan.CodeSubstNone,plan.PlanType,"",0,0);
			Assert.AreEqual(fee.Amount,allowed);
		}

		///<summary>Get the allowed amount when there is a substitution code for the PPO plan.</summary>
		[TestMethod]
		public void InsPlans_GetAllowed_PPOSubstitute() {
			InsPlan plan=GeneratePPOPlan(MethodBase.GetCurrentMethod().Name,false);
			ProcedureCode procCode=_listProcCodes[7];
			procCode.SubstitutionCode=_listProcCodes[8].ProcCode;
			ProcedureCodes.Update(procCode);
			Fee feeOrig=FeeT.GetNewFee(plan.FeeSched,procCode.CodeNum,85);
			Fee feeSubs=FeeT.GetNewFee(plan.FeeSched,ProcedureCodes.GetSubstituteCodeNum(procCode.ProcCode,""),20);
			double allowed=InsPlans.GetAllowed(procCode.ProcCode,plan.FeeSched,plan.AllowedFeeSched,plan.CodeSubstNone,plan.PlanType,"",0,0);
			Assert.AreEqual(feeSubs.Amount,allowed);
		}

		///<summary>Get the allowed amount where there is a substitution code that is more expensive than the original code for the PPO plan.</summary>
		[TestMethod]
		public void InsPlans_GetAllowed_PPOSubstituteMoreExpensive() {
			InsPlan plan=GeneratePPOPlan(MethodBase.GetCurrentMethod().Name,false);
			ProcedureCode procCode=_listProcCodes[9];
			procCode.SubstitutionCode=_listProcCodes[10].ProcCode;
			ProcedureCodes.Update(procCode);
			Fee feeOrig=FeeT.GetNewFee(plan.FeeSched,procCode.CodeNum,85);
			Fee feeSubs=FeeT.GetNewFee(plan.FeeSched,ProcedureCodes.GetSubstituteCodeNum(procCode.SubstitutionCode,""),200);
			double allowed=InsPlans.GetAllowed(procCode.ProcCode,plan.FeeSched,plan.AllowedFeeSched,plan.CodeSubstNone,plan.PlanType,"",0,0);
			Assert.AreEqual(feeOrig.Amount,allowed);
		}

		///<summary>Get the allowed amount for a capitation plan that has an allowed fee schedule.</summary>
		[TestMethod]
		public void InsPlans_GetAllowed_CapAllowedFeeSched() {
			InsPlan plan=GenerateCapPlan(MethodBase.GetCurrentMethod().Name);
			ProcedureCode procCode=_listProcCodes[11];
			Fee feeAllowed=FeeT.GetNewFee(plan.AllowedFeeSched,procCode.CodeNum,70);
			double amt=InsPlans.GetAllowed(procCode.ProcCode,plan.FeeSched,plan.AllowedFeeSched,plan.CodeSubstNone,plan.PlanType,"",0,0);
			Assert.AreEqual(feeAllowed.Amount,amt);
		}

		///<summary>Get the allowed amount for a capitation plan where there is no allowed fee schedule and there is no substitution code.</summary>
		[TestMethod]
		public void InsPlan_GetAllowed_CapNoAllowedNoSubs() {
			InsPlan plan=GenerateCapPlan(MethodBase.GetCurrentMethod().Name,false);
			ProcedureCode procCode=_listProcCodes[12];
			double amt=InsPlans.GetAllowed(procCode.ProcCode,plan.FeeSched,plan.AllowedFeeSched,plan.CodeSubstNone,plan.PlanType,"",0,0);
			Assert.AreEqual(-1,amt);
		}

		///<summary>Get the allowed amount for a capitation plan where there is no fee schedule assigned to the plan</summary>
		[TestMethod]
		public void InsPlans_GetAllowed_NoFeeSched() {
			Carrier carrier=CarrierT.CreateCarrier(MethodBase.GetCurrentMethod().Name);
			InsPlan plan=new InsPlan();
			plan.CarrierNum=carrier.CarrierNum;
			plan.PlanType="";
			plan.CobRule=EnumCobRule.Basic;
			plan.PlanNum=InsPlans.Insert(plan);
			ProcedureCode procCode=_listProcCodes[13];
			procCode.SubstitutionCode=_listProcCodes[14].ProcCode;
			ProcedureCodes.Update(procCode);
			Provider prov=Providers.GetProv(PrefC.GetLong(PrefName.PracticeDefaultProv));
			long provFeeSched=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,MethodBase.GetCurrentMethod().Name);
			prov.FeeSched=provFeeSched;
			Providers.Update(prov);
			Providers.RefreshCache();
			Fee defaultFee=FeeT.GetNewFee(Providers.GetProv(PrefC.GetLong(PrefName.PracticeDefaultProv)).FeeSched,ProcedureCodes.GetSubstituteCodeNum(procCode.ProcCode,""),80);
			double amt=InsPlans.GetAllowed(procCode.ProcCode,plan.FeeSched,plan.AllowedFeeSched,plan.CodeSubstNone,plan.PlanType,"",0,0);
			Assert.AreEqual(defaultFee.Amount,amt);
		}

		#region Factory Methods

		private InsPlan GenerateMediFlatInsPlan(string suffix,bool codeSubstNone=true) {
			long baseFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"Normal_"+suffix,true);
			long copayFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.CoPay,"Copay_"+suffix,true);
			Carrier carrier = CarrierT.CreateCarrier("Carrier_"+suffix);
			return InsPlanT.CreateInsPlanMediFlatCopay(carrier.CarrierNum,baseFeeSchedNum,copayFeeSchedNum,codeSubstNone);
		}

		private InsPlan GeneratePPOPlan(string suffix,bool codeSubstNone=true) {
			long baseFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"Normal_"+suffix,true);
			Carrier carrier=CarrierT.CreateCarrier("Carrier_"+suffix);
			return InsPlanT.CreateInsPlanPPO(carrier.CarrierNum,baseFeeSchedNum,codeSubstNone);
		}

		private InsPlan GenerateCapPlan(string suffix,bool createAllowed=true,bool codeSubstNone=true) {
			long baseFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"Normal_"+suffix,true);
			long allowedFeeSchedNum=0;
			if(createAllowed) {
				allowedFeeSchedNum=FeeSchedT.CreateFeeSched(FeeScheduleType.Normal,"Allowed_"+suffix,true);
			}
			Carrier carrier=CarrierT.CreateCarrier("Carrier_"+suffix);
			return InsPlanT.CreateInsPlanCapitation(carrier.CarrierNum,baseFeeSchedNum,allowedFeeSchedNum,codeSubstNone);
		}

		#endregion

	}
}
