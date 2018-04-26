using System;
using System.Collections.Generic;
using System.Text;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class InsPlanT {
		///<summary>Creats an insurance plan with the default fee schedule of 53.</summary>
		public static InsPlan CreateInsPlan(long carrierNum) {
			return CreateInsPlan(carrierNum,EnumCobRule.Basic);
		}

		///<summary>Creates an insurance plan with the default fee schedule of 53.</summary>
		public static InsPlan CreateInsPlan(long carrierNum,EnumCobRule cobRule) {
			InsPlan plan=new InsPlan();
			plan.CarrierNum=carrierNum;
			plan.PlanType="";
			plan.FeeSched=53;
			plan.CobRule=cobRule;
			InsPlans.Insert(plan);
			return plan;
		}

		///<summary>Creats an insurance plan with the default fee schedule of 53.</summary>
		public static InsPlan CreateInsPlanPPO(long carrierNum,long feeSchedNum,bool codeSubstNone=true){
			return CreateInsPlanPPO(carrierNum,feeSchedNum,EnumCobRule.Basic,codeSubstNone);
		}

		///<summary>Creates an insurance plan with the default fee schedule of 53.</summary>
		public static InsPlan CreateInsPlanPPO(long carrierNum,long feeSchedNum,EnumCobRule cobRule,bool codeSubstNone=true) {
			InsPlan plan=new InsPlan();
			plan.CarrierNum=carrierNum;
			plan.PlanType="p";
			plan.FeeSched=feeSchedNum;
			plan.CobRule=cobRule;
			plan.CodeSubstNone=codeSubstNone;
			InsPlans.Insert(plan);
			return plan;
		}

		///<summary>Creats a Medicaid/Flat Copay insurance plan.</summary>
		public static InsPlan CreateInsPlanMediFlatCopay(long carrierNum,long feeSchedNum,long copayFeeSchedNum = 0,bool codeSubstNone=true) {
			InsPlan plan=new InsPlan();
			plan.CarrierNum=carrierNum;
			plan.PlanType="f";
			plan.FeeSched=feeSchedNum;
			plan.CobRule=EnumCobRule.Standard;
			plan.CopayFeeSched=copayFeeSchedNum;
			plan.CodeSubstNone=codeSubstNone;
			InsPlans.Insert(plan);
			return plan;
		}

		public static InsPlan CreateInsPlanCapitation(long carrierNum,long feeSchedNum,long allowedFeeSchedNum=0,bool codeSubstNone=true) {
			InsPlan plan=new InsPlan();
			plan.CarrierNum=carrierNum;
			plan.PlanType="c";
			plan.FeeSched=feeSchedNum;
			plan.CobRule=EnumCobRule.Standard;
			plan.AllowedFeeSched=allowedFeeSchedNum;
			plan.CodeSubstNone=codeSubstNone;
			plan.PlanNum=InsPlans.Insert(plan);
			return plan;
		}

		public static InsPlan GetPlanForPriSecMed(PriSecMed priSecMed,List<PatPlan> listPatPlans,List<InsPlan> listPlans,List<InsSub> listSubs) {
			long subNum=PatPlans.GetInsSubNum(listPatPlans,PatPlans.GetOrdinal(priSecMed,listPatPlans,listPlans,listSubs));
			InsSub sub=InsSubs.GetSub(subNum,listSubs);
			return InsPlans.GetPlan(sub.PlanNum,listPlans);
		}
	}
}
