using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class ClaimProcT {
		public static void AddInsUsedAdjustment(long patNum,long planNum,double amtPaid,long subNum,double dedApplied){
			ClaimProc cp=new ClaimProc();
			cp.PatNum=patNum;
			cp.PlanNum=planNum;
			cp.InsSubNum=subNum;
			cp.ProcDate=DateTime.Today;
			cp.Status=ClaimProcStatus.Adjustment;
			cp.InsPayAmt=amtPaid;
			cp.DedApplied=dedApplied;
			ClaimProcs.Insert(cp);
		}

		public static void AddInsPaid(long patNum,long planNum,long procNum,double amtPaid,long subNum,double dedApplied,double writeOff) {
			AddInsPaid(patNum,planNum,procNum,amtPaid,subNum,dedApplied,writeOff,DateTime.Today);
		}

		///<summary>This tells the calculating logic that insurance paid on a procedure.  It avoids the creation of an actual claim.</summary>
		public static void AddInsPaid(long patNum,long planNum,long procNum,double amtPaid,long subNum,double dedApplied,double writeOff,DateTime procDate) {
			ClaimProc cp=new ClaimProc();
			cp.ProcNum=procNum;
			cp.PatNum=patNum;
			cp.PlanNum=planNum;
			cp.InsSubNum=subNum;
			cp.InsPayAmt=amtPaid;
			cp.DedApplied=dedApplied;
			cp.WriteOff=writeOff;
			cp.Status=ClaimProcStatus.Received;
			cp.DateCP=DateTime.Today;
			cp.ProcDate=procDate;
			ClaimProcs.Insert(cp);
		}




	}
}
