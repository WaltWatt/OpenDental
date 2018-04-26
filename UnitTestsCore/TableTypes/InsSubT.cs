using System;
using System.Collections.Generic;
using System.Text;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class InsSubT {
		///<summary></summary>
		public static InsSub CreateInsSub(long subscriberNum,long planNum){
			InsSub sub=new InsSub();
			sub.Subscriber=subscriberNum;
			sub.PlanNum=planNum;
			sub.SubscriberID="1234";
			InsSubs.Insert(sub);
			return sub;
		}

		public static List<InsSub> GetInsSubs(Patient pat) {
			Family fam=Patients.GetFamily(pat.PatNum);
			return InsSubs.RefreshForFam(fam);
		}



	}
}
