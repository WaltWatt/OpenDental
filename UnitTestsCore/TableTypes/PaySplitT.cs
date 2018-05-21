using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenDentBusiness;
using CodeBase;

namespace UnitTestsCore {
	public class PaySplitT {

		public static PaySplit CreateSplit(long clinicNum,long patNum,long payNum,long payplanNum,DateTime procDate,long procNum,long provNum
			,double splitAmt,long unearnedType) {
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

		public static PaySplit CreatePrepayment(long patNum,int amt,DateTime datePay,long provNum = 0,long clinicNum = 0) {
			Def unearnedType=Defs.GetDefByExactName(DefCat.PaySplitUnearnedType,"PrePayment")??DefinitionT.CreateDefinition(DefCat.PaySplitUnearnedType,"PrePayment");
			Def payType=Defs.GetDefByExactName(DefCat.PaymentTypes,"Check")??DefinitionT.CreateDefinition(DefCat.PaymentTypes,"Check");
			Payment pay=PaymentT.MakePaymentNoSplits(patNum,amt,datePay,clinicNum:clinicNum,payType:payType.DefNum);
			PaySplit split=new PaySplit();
			split.PayNum=pay.PayNum;
			split.PatNum=pay.PatNum;
			split.DatePay=datePay;
			split.ClinicNum=pay.ClinicNum;
			split.PayPlanNum=0;
			split.ProvNum=provNum;
			split.ProcNum=0;
			split.SplitAmt=amt;
			split.DateEntry=datePay;
			split.UnearnedType=unearnedType.DefNum;
			PaySplits.Insert(split);
			return split;
		}

		public static List<PaySplit> CreatePaySplitsForPrepayment(Procedure proc,double amtToUse,PaySplit prePaySplit = null,Clinic clinic = null,long prov = 0) {
			List<PaySplit> retVal=new List<PaySplit>();
			long clinicNum=prePaySplit?.ClinicNum??clinic?.ClinicNum??proc.ClinicNum;
			long provNum=prePaySplit?.ProvNum??prov;
			if(clinic!=null) {
				clinicNum=clinic.ClinicNum;
			}
			if(prov!=0) {
				provNum=prov;
			}
			Def unearnedType=Defs.GetDefByExactName(DefCat.PaySplitUnearnedType,"PrePayment")??DefinitionT.CreateDefinition(DefCat.PaySplitUnearnedType,"PrePayment");
			Def payType=Defs.GetDefByExactName(DefCat.PaymentTypes,"Check")??DefinitionT.CreateDefinition(DefCat.PaymentTypes,"Check");
			Payment pay=PaymentT.MakePaymentNoSplits(proc.PatNum,amtToUse,payDate:DateTime.Now,clinicNum:clinicNum,payType:unearnedType.DefNum);
			PaySplit splitNeg=new PaySplit();
			splitNeg.PatNum=prePaySplit?.PatNum??proc.PatNum;
			splitNeg.PayNum=pay.PayNum;
			splitNeg.FSplitNum=prePaySplit?.SplitNum??0;
			splitNeg.ClinicNum=clinicNum;
			splitNeg.ProvNum=provNum;
			splitNeg.SplitAmt=0-amtToUse;
			splitNeg.UnearnedType=prePaySplit?.UnearnedType??unearnedType.DefNum;
			splitNeg.DatePay=DateTime.Now;
			PaySplits.Insert(splitNeg);
			retVal.Add(splitNeg);
			//Make a different paysplit attached to proc and prov they want to use it for.
			PaySplit splitPos=new PaySplit();
			splitPos.PatNum=prePaySplit?.PatNum??proc.PatNum;
			splitPos.PayNum=pay.PayNum;
			splitPos.FSplitNum=splitNeg.SplitNum;
			splitPos.ProvNum=provNum;
			splitPos.ClinicNum=clinicNum;
			splitPos.SplitAmt=amtToUse;
			splitPos.DatePay=DateTime.Now;
			splitPos.ProcNum=proc.ProcNum;
			PaySplits.Insert(splitPos);
			retVal.Add(splitPos);
			return retVal;
		}

		public static void InsertPrepaymentSplits(List<PaySplit> listPaysplits,List<PaySplits.PaySplitAssociated> listPaySplitsAssociated) {
			//Insert paysplits
			foreach(PaySplit ps in listPaysplits) {
				PaySplits.Insert(ps);
			}
			//Associate prepayments
			foreach(PaySplits.PaySplitAssociated split in listPaySplitsAssociated) {
				//Update the FSplitNum after inserts are made. 
				if(split.PaySplitLinked!=null && split.PaySplitOrig!=null) {
					PaySplits.UpdateFSplitNum(split.PaySplitOrig.SplitNum,split.PaySplitLinked.SplitNum);
				}
			}
		}
	}
}