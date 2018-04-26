using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class PaymentT {
		public static Payment MakePayment(long patNum,double payAmt,DateTime payDate,long payPlanNum=0,long provNum=0,long procNum=0,long payType=0,
			long clinicNum=0) 
		{
			Payment payment=new Payment();
			payment.PatNum=patNum;
			payment.PayDate=payDate;
			payment.PayAmt=payAmt;
			payment.PayType=payType;
			payment.ClinicNum=clinicNum;
			payment.DateEntry=payDate;
			Payments.Insert(payment);
			PaySplit split=new PaySplit();
			split.PayNum=payment.PayNum;
			split.PatNum=payment.PatNum;
			split.DatePay=payDate;
			split.ClinicNum=payment.ClinicNum;
			split.PayPlanNum=payPlanNum;
			split.ProvNum=provNum;
			split.ProcNum=procNum;
			split.SplitAmt=payAmt;
			split.DateEntry=payDate;
			PaySplits.Insert(split);
			return payment;
		}

		public static Payment MakePaymentNoSplits(long patNum,double payAmt,DateTime payDate=default(DateTime),bool isNew=false,long payType=0
			,long clinicNum=0) 
		{//payType defaulted to non-income transfer
			if(payDate==default(DateTime)) {
				payDate=DateTime.Today;
			}
			Payment payment=new Payment();
			payment.PatNum=patNum;
			payment.PayDate=payDate;
			payment.PayAmt=payAmt;
			payment.IsNew=isNew;
			payment.ClinicNum=clinicNum;
			payment.PayType=payType;
			Payments.Insert(payment);
			return payment;
		}

		public static PaySplit CreateSplit(long clinicNum,long patNum,long payNum,long payplanNum,DateTime procDate,long procNum,long provNum
			,double splitAmt,long unearnedType) 
		{
			PaySplit paysplit=new PaySplit() {
				ClinicNum=clinicNum,
				PatNum=patNum,
				PayNum=payNum,
				PayPlanNum=payplanNum,
				ProcDate=procDate,
				ProcNum=procNum,
				ProvNum=provNum,
				SplitAmt=splitAmt,
				UnearnedType=unearnedType
			};
			PaySplits.Insert(paysplit);
			return paysplit;
		}
	}
}
