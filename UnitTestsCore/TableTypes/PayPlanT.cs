using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class PayPlanT {
		public static PayPlan CreatePayPlan(long patNum,double totalAmt,double payAmt,DateTime datePayStart,long provNum) {
			PayPlan payPlan=new PayPlan();
			payPlan.Guarantor=patNum;
			payPlan.PatNum=patNum;
			payPlan.PayAmt=totalAmt;
			payPlan.PayPlanDate=DateTime.Today;
			payPlan.PayAmt=totalAmt;
			PayPlans.Insert(payPlan);
			PayPlanCharge charge=new PayPlanCharge();
			charge.PayPlanNum=payPlan.PayPlanNum;
			charge.PatNum=patNum;
			charge.ChargeDate=datePayStart;
			charge.Principal=totalAmt;
			charge.ChargeType=PayPlanChargeType.Credit;
			double sumCharges=0;
			int countPayments=0;
			while(sumCharges < totalAmt) { 
				charge=new PayPlanCharge();
				charge.ChargeDate=datePayStart.AddMonths(countPayments);
				charge.PatNum=patNum;
				charge.Guarantor=patNum;
				charge.PayPlanNum=payPlan.PayPlanNum;
				charge.Principal=Math.Min(payAmt,totalAmt-sumCharges);
				charge.ProvNum=provNum;
				sumCharges+=charge.Principal;
				charge.ChargeType=PayPlanChargeType.Debit;
				PayPlanCharges.Insert(charge);
				countPayments++;
			}
			return payPlan;
		}

		public static PayPlan CreatePayPlanNoCharges(long patNum,double totalAmt,DateTime payPlanDate,long guarantorNum) {
			PayPlan payPlan=new PayPlan();
			payPlan.Guarantor=patNum;
			payPlan.PatNum=patNum;
			payPlan.PayAmt=totalAmt;
			payPlan.PayPlanDate=payPlanDate;
			payPlan.PayAmt=totalAmt;
			payPlan.Guarantor=guarantorNum;
			PayPlans.Insert(payPlan);
			return payPlan;
		}

		public static PayPlanCharge CreatePayPlanCharge(DateTime chargeDate,long patNum,long guarantor,long payplanNum,double prinicpal,long provNum
			,long procNum,PayPlanChargeType chargeType) 
		{
			PayPlanCharge ppc=new PayPlanCharge() {
				PayPlanNum=payplanNum,
				PatNum=patNum,
				ChargeDate=chargeDate,
				Principal=prinicpal,
				Guarantor=guarantor,
				ProvNum=provNum,
				ProcNum=procNum,
				ChargeType=chargeType
			};
			PayPlanCharges.Insert(ppc);
			return ppc;
		}

		/// <summary>Creates a payplan and payplan charges with credits. Credit amount generated based off the total amount of the procedures in the list.
		/// If credits are not attached,list of procedures must be null and a total amount must be specified.</summary>
		public static PayPlan CreatePayPlanWithCredits(long patNum,double payAmt,DateTime datePayStart,long provNum=0,List<Procedure> listProcs=null
			,double totalAmt=0,long guarantorNum=0,long clinicNum=0)
		{
			double totalAmount;
			guarantorNum=guarantorNum==0?patNum:guarantorNum;//if it's 0, default to the patNum. 
			if(listProcs!=null) {
				totalAmount=listProcs.Sum(x => x.ProcFee);
			}
			else {
				totalAmount=totalAmt;
			}
			PayPlan payPlan=CreatePayPlanNoCharges(patNum,totalAmount,datePayStart,guarantorNum);//create charges later depending on if attached to procs or not.
			if(listProcs!=null) {
				foreach(Procedure proc in listProcs) {
					PayPlanCharge credit=new PayPlanCharge();
					credit.PayPlanNum=payPlan.PayPlanNum;
					credit.PatNum=patNum;
					credit.ProcNum=proc.ProcNum;
					credit.ProvNum=proc.ProvNum;
					credit.Guarantor=patNum;//credits should always appear on the patient of the payment plan.
					credit.ChargeDate=datePayStart;
					credit.ClinicNum=clinicNum;
					credit.Principal=proc.ProcFee;
					credit.ChargeType=PayPlanChargeType.Credit;
					PayPlanCharges.Insert(credit);//attach the credit for the proc amount. 
				}
			}
			else {//make one credit for the lump sum.
				PayPlanCharge credit=new PayPlanCharge();
				credit.PayPlanNum=payPlan.PayPlanNum;
				credit.PatNum=patNum;
				credit.ChargeDate=datePayStart;
				credit.ProvNum=provNum;
				credit.ClinicNum=clinicNum;
				credit.Guarantor=patNum;//credits should always appear on the patient of the payment plan.
				credit.Principal=totalAmount;
				credit.ChargeType=PayPlanChargeType.Credit;
				PayPlanCharges.Insert(credit);//attach the credit for the total amount.
			}
			//make debit charges for the payment plan
			double sumCharges=0;
			int countPayments=0;
			while(sumCharges < totalAmount) { 
				PayPlanCharge charge=new PayPlanCharge();
				charge.ChargeDate=datePayStart.AddMonths(countPayments);
				charge.PatNum=patNum;
				charge.Guarantor=guarantorNum;
				charge.ClinicNum=clinicNum;
				charge.PayPlanNum=payPlan.PayPlanNum;
				charge.Principal=Math.Min(payAmt,totalAmount-sumCharges);
				charge.ProvNum=provNum;
				sumCharges+=charge.Principal;
				charge.ChargeType=PayPlanChargeType.Debit;
				PayPlanCharges.Insert(charge);
				countPayments++;
			}
			return payPlan;
		}

		public static void CreatePayPlanWithAdjustmentsAndCredits(long patNum,double payAmt,DateTime datePayStart,long provNum=0,
			List<Procedure> listProcs=null,double totalAmt=0,long guarantorNum=0,long clinicNum=0,double adjAmt=0)
		{
			//Create Base Pay Plan
			PayPlan payPlan=CreatePayPlanWithCredits(patNum,payAmt,datePayStart,provNum,listProcs,totalAmt,guarantorNum,clinicNum);
			//Add Adjustments (Negative Debits) to the payplan
			//TODO: Move payplan logic into business layer
		}

	}	
}
