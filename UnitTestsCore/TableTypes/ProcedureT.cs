using System;
using System.Collections.Generic;
using System.Text;
using OpenDentBusiness;
using System.Linq;

namespace UnitTestsCore {
	public class ProcedureT {
		///<summary>Returns the proc</summary>
		///<param name="procDate">If not included, will be set to DateTime.Now.</param>
		public static Procedure CreateProcedure(Patient pat,string procCodeStr,ProcStat procStatus,string toothNum,double procFee,
			DateTime procDate=default(DateTime),int priority=0,long plannedAptNum=0,long provNum=0)
		{
			Procedure proc=new Procedure();
			proc.CodeNum=ProcedureCodes.GetCodeNum(procCodeStr);
			proc.PatNum=pat.PatNum;
			if(procDate==default(DateTime)) {
				proc.ProcDate=DateTime.Today;
			}
			else {
				proc.ProcDate=procDate;
			}
			proc.ProcStatus=procStatus;
			proc.ProvNum=provNum;
			if(provNum==0) {
				proc.ProvNum=pat.PriProv;
			}
			proc.ProcFee=procFee;
			proc.ToothNum=toothNum;
			proc.Prosthesis="I";
			proc.Priority=Defs.GetDefsForCategory(DefCat.TxPriorities,true)[priority].DefNum;
			proc.PlannedAptNum=plannedAptNum;
			proc.ClinicNum=pat.ClinicNum;
			Procedures.Insert(proc);
			return proc;
		}

		/*public static void SetToothNum(Procedure procedure,string toothNum){
			Procedure oldProcedure=procedure.Copy();
			procedure.ToothNum=toothNum;
			Procedures.Update(procedure,oldProcedure);
		}*/

		public static void SetPriority(Procedure procedure,int priority){
			Procedure oldProcedure=procedure.Copy();
			procedure.Priority=Defs.GetDefsForCategory(DefCat.TxPriorities,true)[priority].DefNum;
			Procedures.Update(procedure,oldProcedure);
		}

		public static void SetComplete(Procedure proc,Patient pat,List<InsPlan> planList,List<PatPlan> patPlanList,List<ClaimProc> claimProcList,List<Benefit> benefitList,List<InsSub> subList) {
			Procedure procOld=proc.Copy();
			ProcedureCode procCode=ProcedureCodes.GetProcCode(proc.CodeNum);
			proc.DateEntryC=DateTime.Now;
			proc.ProcStatus=ProcStat.C;
			Procedures.Update(proc,procOld);
			Procedures.ComputeEstimates(proc,proc.PatNum,claimProcList,false,planList,patPlanList,benefitList,pat.Age,subList);

		}

		///<summary>Computes the claimproc estimates for all TP procedures. Returns the claimprocs.</summary>
		public static List<ClaimProc> ComputeEstimates(Patient pat,InsuranceInfo insInfo) {
			return ComputeEstimates(pat,insInfo.ListPatPlans,insInfo.ListInsPlans,insInfo.ListInsSubs,insInfo.ListBenefits);
		}

		///<summary>Computes the claimproc estimates for all TP procedures. Returns the claimprocs.</summary>
		public static List<ClaimProc> ComputeEstimates(Patient pat,List<PatPlan> listPatPlans,List<InsPlan> listPlans,List<InsSub> listSubs,
			List<Benefit> listBens) 
		{
			List<ClaimProc> claimProcs=ClaimProcs.Refresh(pat.PatNum);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();
			List<ClaimProcHist> histList=ClaimProcs.GetHistList(pat.PatNum,listBens,listPatPlans,listPlans,DateTime.Now,listSubs);
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			List<Procedure> ProcList=Procedures.Refresh(pat.PatNum);
			Procedure[] ProcListTP=Procedures.GetListTPandTPi(ProcList);//sorted by priority, then toothnum
			for(int i=0;i<ProcListTP.Length;i++) {
				Procedures.ComputeEstimates(ProcListTP[i],pat.PatNum,ref claimProcs,false,listPlans,listPatPlans,listBens,
					histList,loopList,false,pat.Age,listSubs);
				//then, add this information to loopList so that the next procedure is aware of it.
				loopList.AddRange(ClaimProcs.GetHistForProc(claimProcs,ProcListTP[i].ProcNum,ProcListTP[i].CodeNum));
			}
			//save changes in the list to the database
			ClaimProcs.Synch(ref claimProcs,claimProcListOld);
			return ClaimProcs.Refresh(pat.PatNum);
		}


	}
}
