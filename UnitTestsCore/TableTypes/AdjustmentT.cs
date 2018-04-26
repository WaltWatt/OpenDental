using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class AdjustmentT {
		public static Adjustment MakeAdjustment(long patNum,double adjAmt,DateTime adjDate=default(DateTime),DateTime procDate=default(DateTime)
			,long procNum=0,long provNum=0) 
		{
			Adjustment adjustment=new Adjustment();
			if(adjDate==default(DateTime)) {
				adjDate=DateTime.Today;
			}
			if(procDate==default(DateTime)) {
				procDate=DateTime.Today;
			}
			adjustment.PatNum=patNum;
			adjustment.AdjAmt=adjAmt;
			adjustment.ProcNum=procNum;
			adjustment.ProvNum=provNum;
			adjustment.AdjDate=adjDate;
			adjustment.ProcDate=procDate;
			Adjustments.Insert(adjustment);
			return adjustment;
		}

	}
}
