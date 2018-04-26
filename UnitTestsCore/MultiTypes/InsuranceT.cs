using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class InsuranceT {
		public static InsuranceInfo AddInsurance(Patient pat,string carrierName,string planType="",long feeSchedNum=0,int ordinal=1,bool isMedical=false,
			EnumCobRule cobRule=EnumCobRule.Basic,long copayFeeSchedNum=0) 
		{
			Carrier carrier=CarrierT.CreateCarrier(carrierName);
			InsPlan plan=InsPlanT.CreateInsPlan(carrier.CarrierNum,cobRule);
			InsPlan planOld=plan.Copy();
			plan.PlanType=planType;
			plan.FeeSched=feeSchedNum;
			plan.IsMedical=isMedical;
			plan.CopayFeeSched=copayFeeSchedNum;
			InsPlans.Update(plan,planOld);
			InsSub sub=InsSubT.CreateInsSub(pat.PatNum,plan.PlanNum);
			PatPlan patPlan=PatPlanT.CreatePatPlan((byte)ordinal,pat.PatNum,sub.InsSubNum);
			return new InsuranceInfo {
				ListCarriers=new List<Carrier> { carrier },
				ListInsPlans=new List<InsPlan> { plan },
				ListInsSubs=new List<InsSub> { sub },
				ListPatPlans=new List<PatPlan> { patPlan },
			};
		}

	}

	public class InsuranceInfo {
		public List<PatPlan> ListPatPlans=new List<PatPlan>();
		public List<InsSub> ListInsSubs=new List<InsSub>();
		public List<InsPlan> ListInsPlans=new List<InsPlan>();
		public List<Carrier> ListCarriers=new List<Carrier>();
		public List<Benefit> ListBenefits=new List<Benefit>();

		public PatPlan PrimaryPatPlan {
			get {
				return ListPatPlans.FirstOrDefault(x => x.Ordinal==PatPlans.GetOrdinal(PriSecMed.Primary,ListPatPlans,ListInsPlans,ListInsSubs));
			}
		}

		public InsSub PrimaryInsSub {
			get {
				long subNum=PatPlans.GetInsSubNum(ListPatPlans,PatPlans.GetOrdinal(PriSecMed.Primary,ListPatPlans,ListInsPlans,ListInsSubs));
				return InsSubs.GetSub(subNum,ListInsSubs);
			}
		}

		public InsPlan PrimaryInsPlan {
			get {
				return InsPlans.GetPlan(PrimaryInsSub.PlanNum,ListInsPlans);
			}
		}

		public void RefreshBenefits() {
			ListBenefits=Benefits.Refresh(ListPatPlans,ListInsSubs);
		}
	}
}
