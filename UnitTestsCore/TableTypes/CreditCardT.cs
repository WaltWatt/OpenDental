using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class CreditCardT {
		public static CreditCard CreateCard(long patNum,double chargeAmt,DateTime dateStart,long payPlanNum) {
			CreditCard card=new CreditCard();
			card.PatNum=patNum;
			card.ChargeAmt=chargeAmt;
			card.DateStart=dateStart;
			card.PayPlanNum=payPlanNum;
			card.CCExpiration=DateTime.Today.AddYears(3);
			card.CCNumberMasked="XXXXXXXXXXXXX1234";
			CreditCards.Insert(card);
			return card;
		}

	}
}
