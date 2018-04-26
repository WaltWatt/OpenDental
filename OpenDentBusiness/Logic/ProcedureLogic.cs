using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness {
	public class ProcedureLogic{

		///<summary>The supplied DataRows must include the following columns: ProcStatus(optional),Priority(optional),ToothRange,ToothNum,ProcCode.  This sorts procedures based on priority, then tooth number, then procCode.  It does not care about dates or status.  Currently used in Account module, appointments, and Chart module sorting.  TP uses Procedures.ProcedureComparer.</summary>
		public static int CompareProcedures(DataRow x,DataRow y) {
			//first, by status
			if(x.Table.Columns.Contains("ProcStatus") && y.Table.Columns.Contains("ProcStatus")) {
				if(x["ProcStatus"].ToString()!=y["ProcStatus"].ToString()) {
					//Cn,TP,R,EO,C,EC,D
					//EC procs will draw on top of C procs of same date in the 3D tooth chart, 
					//but this is not a problem since C procs should always have a later date than EC procs.
					//EC must come after C so that group notes will come after their procedures in Progress Notes.
					int xIdx=0;
					switch(x["ProcStatus"].ToString()) {
						case "8"://TPi
							xIdx=0;
							break;
						case "7"://Cn
							xIdx=1;
							break;
						case "1"://TP
							xIdx=2;
							break;
						case "5"://R
							xIdx=3;
							break;
						case "4"://EO
							xIdx=4;
							break;
						case "2"://C
							xIdx=5;
							break;
						case "3"://EC
							xIdx=6;
							break;
						case "6"://D
							xIdx=7;
							break;
					}
					int yIdx=0;
					switch(y["ProcStatus"].ToString()) {
						case "8"://TPi
							yIdx=0;
							break;
						case "7"://Cn
							yIdx=1;
							break;
						case "1"://TP
							yIdx=2;
							break;
						case "5"://R
							yIdx=3;
							break;
						case "4"://EO
							yIdx=4;
							break;
						case "2"://C
							yIdx=5;
							break;
						case "3"://EC
							yIdx=6;
							break;
						case "6"://D
							yIdx=7;
							break;
					}
					return xIdx.CompareTo(yIdx);
				}
			}
			//by priority
			if(x.Table.Columns.Contains("Priority") && y.Table.Columns.Contains("Priority")){
				if(x["Priority"].ToString()!=y["Priority"].ToString()) {//if priorities are different
					if(x["Priority"].ToString()=="0") {
						return 1;//x is greater than y. Priorities always come first.
					}
					if(y["Priority"].ToString()=="0") {
						return -1;//x is less than y. Priorities always come first.
					}
					return Defs.GetOrder(DefCat.TxPriorities,PIn.Long(x["Priority"].ToString())).CompareTo
						(Defs.GetOrder(DefCat.TxPriorities,PIn.Long(y["Priority"].ToString())));
				}
			}
			//priorities are the same, so sort by toothrange
			if(x["ToothRange"].ToString()!=y["ToothRange"].ToString()) {
				//empty toothranges come before filled toothrange values
				return x["ToothRange"].ToString().CompareTo(y["ToothRange"].ToString());
			}
			//toothranges are the same (usually empty), so compare toothnumbers
			if(x["ToothNum"].ToString()!=y["ToothNum"].ToString()) {
				//this also puts invalid or empty toothnumbers before the others.
				return Tooth.ToInt(x["ToothNum"].ToString()).CompareTo(Tooth.ToInt(y["ToothNum"].ToString()));
			}
			if(x["ProcCode"].ToString()!=y["ProcCode"].ToString()) {
			//priority and toothnums are the same, so sort by proccode if different.
				return x["ProcCode"].ToString().CompareTo(y["ProcCode"].ToString());
			}
			//priority, tooth number, and proccode are all the same.  Sort by ProcNum so we have a determinate order if everything else is the same.
			return x["ProcNum"].ToString().CompareTo(y["ProcNum"].ToString());
		}


		///<summary>Compares two procedures and returns the order they should appear based on status, priority, toothrange, toothnum, then proccode.  
		///Uses the same logic as the other CompareProcedures but takes Procedure objects instead of DataRows.  
		///Only used for the Appointment Edit window currently.</summary>
		public static int CompareProcedures(Procedure x,Procedure y) {
			//first by status
			if(x.ProcStatus!=y.ProcStatus) {
				//Cn,TP,R,EO,C,EC,D
				//EC procs will draw on top of C procs of same date in the 3D tooth chart, 
				//but this is not a problem since C procs should always have a later date than EC procs.
				//EC must come after C so that group notes will come after their procedures in Progress Notes.
				int xIdx, yIdx;
				List<ProcStat> sortOrder = new List<ProcStat>
				{//The order of statuses in this list is very important and determines the sort order for procedures.
					ProcStat.TPi,
					ProcStat.Cn,
					ProcStat.TP,
					ProcStat.R,
					ProcStat.EO,
					ProcStat.C,
					ProcStat.EC,
					ProcStat.D
				};
				xIdx=sortOrder.IndexOf(x.ProcStatus);
				yIdx=sortOrder.IndexOf(y.ProcStatus);
				return xIdx.CompareTo(yIdx);
			}
			//by priority
			if(x.Priority!=y.Priority) {//if priorities are different
					if(x.Priority==0) {
						return 1;//x is greater than y. Priorities always come first.
					}
					if(y.Priority==0) {
						return -1;//x is less than y. Priorities always come first.
					}
					return Defs.GetOrder(DefCat.TxPriorities,x.Priority).CompareTo(Defs.GetOrder(DefCat.TxPriorities,y.Priority));
			}
			//priorities are the same, so sort by toothrange
			if(x.ToothRange!=y.ToothRange) {
				//empty toothranges come before filled toothrange values
				return x.ToothRange.CompareTo(y.ToothRange);
			}
			//toothranges are the same (usually empty), so compare toothnumbers
			if(x.ToothNum!=y.ToothNum) {
				//this also puts invalid or empty toothnumbers before the others.
				return Tooth.ToInt(x.ToothNum).CompareTo(Tooth.ToInt(y.ToothNum));
			}
			//priority and toothnums are the same, so sort by proccode.
			if(x.CodeNum!=y.CodeNum) {
				return ProcedureCodes.GetProcCode(x.CodeNum).ProcCode.CompareTo(ProcedureCodes.GetProcCode(y.CodeNum).ProcCode);
			}
			//if everything else is the same, sort by ProcNum so sort is deterministic
			return x.ProcNum.CompareTo(y.ProcNum);
		}





	}


}
