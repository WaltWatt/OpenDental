using System;
using System.Collections.Generic;
using System.Text;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class ProviderT {

		///<summary>Inserts the new provider, refreshes the cache and then returns ProvNum</summary>
		public static long CreateProvider(string abbr,string fName="",string lName="",long feeSchedNum=0,bool isSecondary=false,bool isHidden=false) {
			Provider prov=new Provider();
			prov.Abbr=abbr;
			prov.FName=fName;
			prov.LName=lName;
			prov.FeeSched=feeSchedNum;
			prov.IsSecondary=isSecondary;
			prov.IsHidden=isHidden;
			Providers.Insert(prov);
			Providers.RefreshCache();
			return prov.ProvNum;
		}




	}
}
