using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using CodeBase;
using OpenDentBusiness.Crud;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ClaimProcs{
		#region Get Methods
		#endregion

		#region Modification Methods

		#region Insert

		///<summary>Inserts the ClaimProcs passed in. Does set ClaimProcNum.</summary>
		public static List<ClaimProc> InsertMany(List<ClaimProc> listInsert) {
			if(listInsert.Count==0) {
				return new List<ClaimProc>();
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimProc>>(MethodBase.GetCurrentMethod(),listInsert);
			}
			//Not using Crud.InsertMany because we need to set the PrimaryKeys
			listInsert.ForEach(x => Insert(x));
			return listInsert;
		}

		#endregion

		#region Update
		///<summary>Updates the ClaimProcs passed in.</summary>
		public static void UpdateMany(List<ClaimProc> listUpdate) {
			if(listUpdate.Count==0) {
				return;
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listUpdate);
				return;
			}
			listUpdate.ForEach(x => Update(x));
		}


		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods
		#endregion


		///<summary></summary>
		public static List<ClaimProc> Refresh(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimProc>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command=
				"SELECT * from claimproc "
				+"WHERE PatNum = '"+patNum.ToString()+"' ORDER BY LineNumber";
			return Crud.ClaimProcCrud.SelectMany(command);
		}

		///<summary>Gets the ClaimProcs for a list of patients.</summary>
		public static List<ClaimProc> Refresh(List<long> listPatNums) {
			if(listPatNums.Count==0) {
				return new List<ClaimProc>();
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimProc>>(MethodBase.GetCurrentMethod(),listPatNums);
			}
			string command=
				"SELECT * FROM claimproc "
				+"WHERE PatNum IN("+string.Join(",",listPatNums.Select(x => POut.Long(x)))+")";
			return Crud.ClaimProcCrud.SelectMany(command);
		}

		///<summary>For a given PayPlan, returns a list of Claimprocs associated to that PayPlan. Pass in claim proc status for filtering.</summary>
		public static List<ClaimProc> GetForPayPlans(List<long> listPayPlanNums,List<ClaimProcStatus> listClaimProcStatus=null) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimProc>>(MethodBase.GetCurrentMethod(),listPayPlanNums,listClaimProcStatus);
			}
			string command="SELECT claimproc.* "
					+"FROM claimproc "
					+"WHERE claimproc.PayPlanNum IN ("+POut.String(String.Join(",",listPayPlanNums))+") ";
					if(listClaimProcStatus!=null && listClaimProcStatus.Count>0) {
						command+="AND claimproc.Status IN ("+string.Join(",",listClaimProcStatus.Select(x => (int)x))+") ";
					}
					command+="ORDER BY claimproc.DateCP";
			List<ClaimProc> listCP=Crud.ClaimProcCrud.SelectMany(command);
			return listCP;
		}

		///<summary>When using family deduct or max, this gets all claimprocs for the given plan.  This info is needed to compute used and pending insurance.</summary>
		public static List<ClaimProc> RefreshFam(long planNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimProc>>(MethodBase.GetCurrentMethod(),planNum);
			}
			string command=
				"SELECT * FROM claimproc "
				+"WHERE PlanNum = "+POut.Long(planNum);
				//+" OR PatPlanNum = "+POut.PInt(patPlanNum);
			return Crud.ClaimProcCrud.SelectMany(command);
		}

		///<summary>Gets a list of ClaimProcs for one claim.</summary>
		public static List<ClaimProc> RefreshForClaim(long claimNum,List<Procedure> listProcsForClaim=null,List<ClaimProc> listClaimProcs=null) {
			//No need to check RemotingRole; no call to db.
			List<ClaimProc> listClaimProcsForClaim;
			if(listClaimProcs==null) {
				listClaimProcsForClaim=RefreshForClaims(new List<long> { claimNum }).OrderBy(x => x.LineNumber).ToList();
			}
			else {
				listClaimProcsForClaim=listClaimProcs.Where(x => x.ClaimNum==claimNum).OrderBy(x => x.LineNumber).ToList();
			}
			//In Canada, we must remove any claimprocs which are directly associated to labs, because labs go out on the same line as the attached parent proc.
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA") && listClaimProcsForClaim.Count>0) {
				if(listProcsForClaim==null) {
					listProcsForClaim=Procedures.Refresh(listClaimProcsForClaim[0].PatNum);
				}
				foreach(Procedure proc in listProcsForClaim) {
					if(proc.ProcNumLab==0) {
						continue;
					}
					listClaimProcsForClaim.RemoveAll(x => x.ProcNum==proc.ProcNum);
				}
			}
			return listClaimProcsForClaim;
		}

		///<summary>Gets a list of ClaimProcs for one claim.</summary>
		public static List<ClaimProc> RefreshForClaims(List <long> listClaimNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimProc>>(MethodBase.GetCurrentMethod(),listClaimNums);
			}
			if(listClaimNums.Count==0) {
				return new List<ClaimProc>();
			}
			List <string> listClaimNumStrs=listClaimNums.Select(x => POut.Long(x)).ToList();
			string command=
				"SELECT * FROM claimproc "
				+"WHERE ClaimNum IN("+String.Join(",",listClaimNumStrs)+")";
			return Crud.ClaimProcCrud.SelectMany(command);
		}

		///<summary>Gets a list of ClaimProcs with status of estimate.</summary>
		public static List<ClaimProc> RefreshForTP(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimProc>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command=
				"SELECT * FROM claimproc "
				+"WHERE (Status="+POut.Long((int)ClaimProcStatus.Estimate)
				+" OR Status="+POut.Long((int)ClaimProcStatus.CapEstimate)+") "
				+"AND PatNum = "+POut.Long(patNum);
			return Crud.ClaimProcCrud.SelectMany(command);
		}

		///<summary>Gets a list of ClaimProcs for one proc.</summary>
		public static List<ClaimProc> RefreshForProc(long procNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimProc>>(MethodBase.GetCurrentMethod(),procNum);
			}
			string command=
				"SELECT * FROM claimproc "
				+"WHERE ProcNum="+POut.Long(procNum);
			return Crud.ClaimProcCrud.SelectMany(command);
		}

		///<summary>Gets a list of ClaimProcs for one proc.</summary>
		public static List<ClaimProc> RefreshForProcs(List<long> listProcNums) {
			if(listProcNums==null || listProcNums.Count==0) {
					return new List<ClaimProc>();//No point going to middle tier.
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimProc>>(MethodBase.GetCurrentMethod(),listProcNums);
			}
			//TODO: Use new CRUD function to prevent issue with IN statement when using Oracle.  Derek will provide function.
			string command=
				"SELECT * FROM claimproc "
				+"WHERE ProcNum IN ("+string.Join(",",listProcNums.Select(x => POut.Long(x)))+")";
			return Crud.ClaimProcCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(ClaimProc cp) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				cp.SecUserNumEntry=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				cp.ClaimProcNum=Meth.GetLong(MethodBase.GetCurrentMethod(),cp);
				return cp.ClaimProcNum;
			}
			if(new[] { ClaimProcStatus.Received,ClaimProcStatus.Supplemental}.Contains(cp.Status)) {
				cp.DateSuppReceived=DateTime.Today;
			}
			else {//In case someone tried to programmatically set the DateSuppReceived when they shouldn't have
				cp.DateSuppReceived=DateTime.MinValue;
			}
			return Crud.ClaimProcCrud.Insert(cp);
		}

		///<summary></summary>
		public static void Update(ClaimProc cp) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),cp);
				return;
			}
			if(new[] { ClaimProcStatus.Received,ClaimProcStatus.Supplemental }.Contains(cp.Status) && cp.DateSuppReceived.Year<1880) {
				cp.DateSuppReceived=DateTime.Today;
			}
			else if(!new[] { ClaimProcStatus.Received,ClaimProcStatus.Supplemental }.Contains(cp.Status) && cp.DateSuppReceived.Date==DateTime.Today.Date) {
				cp.DateSuppReceived=DateTime.MinValue;//db only field used by one customer and this is how they requested it.  PatNum #19191
			}
			Crud.ClaimProcCrud.Update(cp);
		}

		///<summary>Updates the ProcDate on the ClaimProcs for a given list of ProcNums.</summary>
		public static void UpdateProcDate(List<long> listProcNums,DateTime procDate) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listProcNums,procDate);
				return;
			}
			if(listProcNums.Count==0) {
				return;
			}
			string command="UPDATE claimproc SET ProcDate="+POut.Date(procDate)+" "
				+"WHERE ProcNum IN ("+string.Join(",",listProcNums.Select(x => POut.Long(x)))+")";
			Db.NonQ(command);
		}

		///<summary></summary>
		public static void Delete(ClaimProc cp) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),cp);
				return;
			}
			string command= "DELETE FROM claimproc WHERE ClaimProcNum = "+POut.Long(cp.ClaimProcNum);
			Db.NonQ(command);
		}

		///<summary>Surround with try/catch.  If there are any dependencies, then this will throw an exception.  This is currently only called from FormClaimProc.</summary>
		public static void DeleteAfterValidating(ClaimProc cp) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),cp);
				return;
			}
			string command;
			if(cp.ClaimNum!=0 && cp.Status!=ClaimProcStatus.Supplemental) {
				command="SELECT COUNT(*) FROM claimproc WHERE ProcNum="+POut.Long(cp.ProcNum)+" AND ClaimNum="+POut.Long(cp.ClaimNum)+" AND Status="+(int)ClaimProcStatus.Supplemental;
				long supplementalCP=PIn.Long(Db.GetCount(command));
				if(supplementalCP!=0) {
					throw new ApplicationException(Lans.g("ClaimProcs","Not allowed to delete this procedure until all supplementals for this procedure are deleted first."));
				}
			}
			//Validate: make sure this is not the last claimproc on the claim.  If cp is not attached to a claim no need to validate.
			if(cp.ClaimNum!=0) {
				command="SELECT COUNT(*) FROM claimproc WHERE ClaimNum= "+POut.Long(cp.ClaimNum)+" AND ClaimProcNum!= "+POut.Long(cp.ClaimProcNum);
				long remainingCP=PIn.Long(Db.GetCount(command));
				if(remainingCP==0) {
					throw new ApplicationException(Lans.g("ClaimProcs","Not allowed to delete the last procedure from a claim.  The entire claim would have to be deleted."));
				}
			}
			//end of validation
			command= "DELETE FROM claimproc WHERE ClaimProcNum = "+POut.Long(cp.ClaimProcNum);
			Db.NonQ(command);
		}

		///<summary>Used when creating a claim to create any missing claimProcs. Also used in FormProcEdit if click button to add Estimate.  Inserts it into db. It will still be altered after this to fill in the fields that actually attach it to the claim.</summary>
		public static void CreateEst(ClaimProc cp, Procedure proc, InsPlan plan,InsSub sub,double baseEstAmt=0,double insEstTotalAmt=0,
			bool isInsertNeeded=true,bool isPreauth=false) 
		{
			//No need to check RemotingRole; no call to db.
			cp.ProcNum=proc.ProcNum;
			//claimnum
			cp.PatNum=proc.PatNum;
			cp.ProvNum=proc.ProvNum;
			if(isPreauth) {
				cp.Status=ClaimProcStatus.Preauth;
			}
			else if(plan.PlanType=="c") {//capitation
				if(proc.ProcStatus==ProcStat.C) {//complete
					cp.Status=ClaimProcStatus.CapComplete;//in this case, a copy will be made later.
				}
				else {//usually TP status
					cp.Status=ClaimProcStatus.CapEstimate;
				}
			}
			else {
				cp.Status=ClaimProcStatus.Estimate;
			}
			cp.PlanNum=plan.PlanNum;
			cp.InsSubNum=sub.InsSubNum;
			//Writeoff=0
			cp.AllowedOverride=-1;
			cp.Percentage=-1;
			cp.PercentOverride=-1;
			cp.CopayAmt=-1;
			cp.NoBillIns=ProcedureCodes.GetProcCode(proc.CodeNum).NoBillIns;
			cp.PaidOtherIns=-1;
			cp.BaseEst=baseEstAmt;
			cp.DedEst=-1;
			cp.DedEstOverride=-1;
			cp.InsEstTotal=insEstTotalAmt;
			cp.InsEstTotalOverride=-1;
			cp.CopayOverride=-1;
			cp.PaidOtherInsOverride=-1;
			cp.WriteOffEst=-1;
			cp.WriteOffEstOverride=-1;
			cp.ClinicNum=proc.ClinicNum;
			cp.EstimateNote="";
			if(!isPreauth) {
				cp.DateCP=proc.ProcDate;
				cp.ProcDate=proc.ProcDate;
			}
			if(isInsertNeeded) {
				Insert(cp);
			}
		}

		///<summary>Creates and inserts supplemental claimprocs for given listClaimProcs.
		///Ignores claimProcs that are not recieved and "By Total" claimProcs.</summary>
		public static List<ClaimProc> CreateSuppClaimProcs(List<ClaimProc> listClaimProcs,bool isReversalClaim=false,bool isOriginalClaim=true) {
			List<ClaimProc> listCLaimProcs=new List<ClaimProc>();
			for(int i=0;i<listClaimProcs.Count;i++) {
				ClaimProc claimProc=listClaimProcs[i].Copy();//Don't modify original list.
				if(claimProc.Status!=ClaimProcStatus.Received || claimProc.ProcNum==0) {//Is not received or is a "By Total" payment.
					continue;//Mimics FormClaimEdit.MakeSuppPayment(...) validation logic
				}
				if(isReversalClaim) {
					//Used for matching logic, see Hx835_Claim.GetPaymentsForClaimProcs(...), is set to 0 after matching.
					claimProc.FeeBilled=-claimProc.FeeBilled;
				}
				else if(isOriginalClaim) {
					claimProc.FeeBilled=0;
				}
				else {//Correction
					claimProc.FeeBilled=listClaimProcs[i].FeeBilled;
				}
				claimProc.ClaimPaymentNum=0;//no payment attached
				//claimprocnum will be overwritten
				claimProc.DedApplied=0;
				claimProc.InsPayAmt=0;
				claimProc.InsPayEst=0;
				claimProc.Remarks="";
				claimProc.Status=ClaimProcStatus.Supplemental;
				claimProc.WriteOff=0;
				claimProc.DateCP=DateTime.Today;
				claimProc.DateEntry=DateTime.Today;
				claimProc.DateInsFinalized=DateTime.MinValue;
				ClaimProcs.Insert(claimProc);//this inserts a copy of the original with the changes as above.
				listCLaimProcs.Add(claimProc);
			}
			return listCLaimProcs;
		}

		///<summary>This compares the two lists and saves all the changes to the database.  It also removes all the items marked doDelete.</summary>
		public static void Synch(ref List<ClaimProc> ClaimProcList,List<ClaimProc> claimProcListOld) {
			//No need to check RemotingRole; no call to db.
			List<ClaimProc> listDelete=new List<ClaimProc>();
			List<ClaimProc> listInsert=new List<ClaimProc>();
			List<ClaimProc> listUpdate=new List<ClaimProc>();
			for(int i=0;i<ClaimProcList.Count;i++) {
				if(ClaimProcList[i].DoDelete) {
					listDelete.Add(ClaimProcList[i]);
					continue;
				}
				//new procs
				if(i>=claimProcListOld.Count) {
					listInsert.Add(ClaimProcList[i]);
					continue;
				}
				//changed procs
				if(!ClaimProcList[i].Equals(claimProcListOld[i])) {
					listUpdate.Add(ClaimProcList[i]);
				}
			}
			ClaimProcs.DeleteMany(listDelete);
			List<ClaimProc> listNewlyInserted=ClaimProcs.InsertMany(listInsert);
			//There will be a one-to-one mapping from listInsert to listNewlyInserted.
			for(int i=0;i<listInsert.Count;i++) {
				listInsert[i].ClaimProcNum=listNewlyInserted[i].ClaimProcNum;
			}
			ClaimProcs.UpdateMany(listUpdate);
			//go backwards to actually remove the deleted items.
			for(int i=ClaimProcList.Count-1;i>=0;i--) {
				if(ClaimProcList[i].DoDelete) {
					ClaimProcList.RemoveAt(i);
				}
			}
		}

		///<summary>Gets all as total insurance payments for a family.</summary>
		public static List<ClaimProc> GetByTotForPats(List<long> listPatNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimProc>>(MethodBase.GetCurrentMethod(),listPatNums);
			}
			string command="SELECT * from claimproc WHERE PatNum IN("+String.Join(", ",listPatNums)+") "
				+"AND ProcNum=0 AND (InsPayAmt!=0 OR WriteOff!=0) "
				+"AND Status IN ("+POut.Int((int)ClaimProcStatus.Received)+","+POut.Int((int)ClaimProcStatus.Supplemental)+","+POut.Int((int)ClaimProcStatus.CapClaim)+")";
			return Crud.ClaimProcCrud.SelectMany(command);
		}

		///<summary>Gets the patient portion due of this procedure.  It is many times different than the proc fee.</summary>
		public static double GetPatPortion(Procedure proc,List<ClaimProc> listClaimProcs,List<Adjustment> listAdjusts) {
			//No need to check RemotingRole; no call to db.
			//The following code is designed to duplicate the Procedures section of the GetAccount method in AccountModules.cs
			//We believe that Capitation Writeoffs are being counted twice due to the way the query gets and uses each column.
			//In the future we should evaluate and test if this is correct behavior.
			List<ClaimProc> listClaimProcsForProc=ClaimProcs.GetForProc(listClaimProcs,proc.ProcNum);
			List<Adjustment> listAdjustsForProc=listAdjusts.FindAll(x => x.ProcNum==proc.ProcNum);
			double capWriteoff=0;
			double insPayAmt=0;
			double insPayEst=0;
			double writeOff=0;
			double adjAmt=0;
			for(int i=0;i<listClaimProcsForProc.Count;i++) {
				ClaimProc claimProc=listClaimProcsForProc[i];
				if(claimProc.Status==ClaimProcStatus.Preauth) {
					continue;
				}
				if(claimProc.Status==ClaimProcStatus.CapComplete) {
					capWriteoff+=claimProc.WriteOff;
				}
				if(claimProc.InsPayAmt!=0) {//Always use InsPayAmt if there is one regardless of status.
					insPayAmt+=claimProc.InsPayAmt;
				}
				else if (claimProc.Status==ClaimProcStatus.NotReceived) {//Only use InsPayEst if it's not received.
					insPayEst+=claimProc.InsPayEst;
				}
				if(claimProc.ClaimNum!=0) {
					writeOff+=claimProc.WriteOff;
				}
			}
			adjAmt=listAdjustsForProc.Select(x => x.AdjAmt).Sum();//Will be a negative number usually
			return Math.Round(proc.ProcFee*Math.Max(1,proc.BaseUnits+proc.UnitQty)-capWriteoff-insPayAmt-insPayEst-writeOff+adjAmt,2);
		}
				
		///<summary>Returns the patient portion for a claim.</summary>
		public static decimal GetPatPortionEst(Claim claim) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<decimal>(MethodBase.GetCurrentMethod(),claim);
			}
			List<Procedure> listProcs=Procedures.GetCompleteForPats(new List<long> { claim.PatNum });
			List<ClaimProc> listClaimProcs=RefreshForClaim(claim.ClaimNum,listProcs);
			List<Adjustment> listAdjusts=Adjustments.GetForProcs(listProcs.Select(x => x.ProcNum).ToList());
			decimal totalPatPort=0;
			//Go through our procs that are attached to the claim and add up the patient portion.
			foreach(Procedure proc in listProcs.Where(x => listClaimProcs.Any(y => y.ProcNum==x.ProcNum))) {
				double patPortion=ClaimProcs.GetPatPortion(proc,listClaimProcs,listAdjusts);
				totalPatPort+=(decimal)patPortion;
			}
			return totalPatPort;
		}

		///<summary>Gets all ClaimProc bundles for the given PayPlanNum. Bundles claimprocs by Date and then by ClaimPaymentNum.</summary>
		public static DataTable GetBundlesForPayPlan(long payPlanNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),payPlanNum);
			}
			//MAX functions added to preserve behavior in Oracle.  We may use ProcDate instead of DateCP in the future.
			string command="SELECT claimproc.ClaimNum,MAX(claimpayment.CheckNum) CheckNum,claimproc.DateCP,MAX(claimpayment.CheckAmt) CheckAmt,claimproc.ClaimPaymentNum,MAX(claimpayment.PayType) PayType, claimproc.ProvNum"
					+",SUM(claimproc.InsPayAmt) InsPayAmt "
				+"FROM claimproc "
				+"LEFT JOIN claimpayment ON claimproc.ClaimPaymentNum=claimpayment.ClaimPaymentNum "
				+"WHERE PayPlanNum="+POut.Long(payPlanNum)+" "
				+"AND claimproc.Status IN ("+POut.Long((long)ClaimProcStatus.Received)+","+POut.Long((long)ClaimProcStatus.Supplemental)+","+POut.Long((long)ClaimProcStatus.CapClaim)+") "
				+"GROUP BY claimproc.ClaimNum,claimproc.DateCP,claimproc.ClaimPaymentNum,claimproc.ProvNum "
				+"ORDER BY claimproc.DateCP";
			return Db.GetTable(command);
		}

		///<summary>When sending or printing a claim, this converts the supplied list into a list of ClaimProcs that need to be sent.</summary>
		public static List<ClaimProc> GetForSendClaim(List<ClaimProc> claimProcList,long claimNum) {
			//No need to check RemotingRole; no call to db.
			//MessageBox.Show(List.Length.ToString());
			List<long> listLabProcNums=new List<long>();
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {
				listLabProcNums=Procedures.GetCanadianLabFees(claimProcList.Select(x=>x.ProcNum).Where(x => x!=0).ToList()).Select(x => x.ProcNum).ToList();
			}
			List<ClaimProc> retVal=new List<ClaimProc>();
			bool includeThis;
			for(int i=0;i<claimProcList.Count;i++) {
				if(claimProcList[i].ClaimNum!=claimNum) {
					continue;
				}
				if(claimProcList[i].ProcNum==0) {
					continue;//skip payments
				}
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA") //Canada
					&& listLabProcNums.Contains(claimProcList[i].ProcNum)) //Current claimProc is associated to a lab.
				{
					continue;
				}
				includeThis=true;
				for(int j=0;j<retVal.Count;j++){//loop through existing claimprocs
					if(retVal[j].ProcNum==claimProcList[i].ProcNum) {
						includeThis=false;//skip duplicate procedures
					}
				}
				if(includeThis) {
					retVal.Add(claimProcList[i]);
				}
			}
			return retVal;
		}

		///<summary>Gets all ClaimProcs for the current Procedure. The List must be all ClaimProcs for this patient.</summary>
		public static List<ClaimProc> GetForProc(List<ClaimProc> claimProcList,long procNum) {
			//No need to check RemotingRole; no call to db.
			//MessageBox.Show(List.Length.ToString());
			//ArrayList ALForProc=new ArrayList();
			List<ClaimProc> retVal=new List<ClaimProc>();
			for(int i=0;i<claimProcList.Count;i++) {
				if(claimProcList[i].ProcNum==procNum) {
					retVal.Add(claimProcList[i]);  
				}
			}
			//need to sort by pri, sec, etc.  BUT,
			//the only way to do it would be to add an ordinal field to claimprocs or something similar.
			//Then a sorter could be built.  Otherwise, we don't know which order to put them in.
			//Maybe supply PatPlanList to this function, because it's ordered.
			//But, then if patient changes ins, it will 'forget' which is pri and which is sec.
			//ClaimProc[] ForProc=new ClaimProc[ALForProc.Count];
			//for(int i=0;i<ALForProc.Count;i++){
			//	ForProc[i]=(ClaimProc)ALForProc[i];
			//}
			//return ForProc;
			return retVal;
		}
		
		///<summary>Loops through listClaimProcs for a claimProc associated to the given claimProcNum.
		///If not found returns null </summary>
		public static ClaimProc GetFromList(List<ClaimProc> listClaimProcs,long claimProcNum) {
			List<ClaimProc> retVal=new List<ClaimProc>();
			for(int i=0;i<listClaimProcs.Count;i++) {
				if(listClaimProcs[i].ClaimProcNum==claimProcNum) {
					return listClaimProcs[i];
				}
			}
			return null;
		}

		///<summary>Gets all ClaimProcs for the current procedure.</summary>
		public static List<ClaimProc> GetForProcs(List<long> listProcNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimProc>>(MethodBase.GetCurrentMethod(),listProcNums);
			}
			List<ClaimProc> listClaimProcs=new List<ClaimProc>();
			if(listProcNums==null || listProcNums.Count < 1) {
				return listClaimProcs;
			}
			string command="SELECT * FROM claimproc WHERE ProcNum IN("+string.Join(",",listProcNums)+")";
			return Crud.ClaimProcCrud.SelectMany(command);
		}

		///<summary> </summary>
		public static List<ClaimProc> GetForProcsWithOrdinal(List<long> listProcNums,int ordinal) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimProc>>(MethodBase.GetCurrentMethod(),listProcNums,ordinal);
			}
			List<ClaimProc> listClaimProcs=new List<ClaimProc>();
			if(listProcNums==null || listProcNums.Count<1) {
				return listClaimProcs;
			}
			string command="SELECT claimproc.* "
				+"FROM claimproc "
				+"INNER JOIN patplan ON patplan.InsSubNum=claimproc.InsSubNum "
					+"AND patplan.PatNum=claimproc.PatNum "
					+"AND patplan.Ordinal="+POut.Int(ordinal)+" "
				+"WHERE ProcNum IN("+string.Join(",",listProcNums)+")";
			return Crud.ClaimProcCrud.SelectMany(command);
		}

		///<summary>Mimics GetForProcsWithOrdinal(...) but for cached information.</summary>
		public static List<ClaimProc> GetForProcsWithOrdinalFromList(List<long> listProcNums,int ordinal,List<PatPlan> listAllPatPlans,List<ClaimProc> listAllClaimProcs) {
			//No need to check RemotingRole; no call to db.
			if(listProcNums==null || listProcNums.Count<1) {
				return new List<ClaimProc>();
			}
			return listAllClaimProcs.Where(x =>
				listProcNums.Contains(x.ProcNum) && listAllPatPlans.FirstOrDefault(y => y.InsSubNum==x.InsSubNum && y.PatNum==x.PatNum && y.Ordinal==ordinal)!=null
			).ToList();
		}

		///<summary> </summary>
		public static ClaimProc GetForProcWithOrdinal(long procNum,int ordinal) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<ClaimProc>(MethodBase.GetCurrentMethod(),procNum,ordinal);
			}
			string command="SELECT claimproc.* "
				+"FROM claimproc "
				+"INNER JOIN patplan ON patplan.InsSubNum=claimproc.InsSubNum "
					+"AND patplan.PatNum=claimproc.PatNum "
					+"AND patplan.Ordinal="+POut.Int(ordinal)+" "
				+"WHERE ProcNum = "+POut.Long(procNum);
			return Crud.ClaimProcCrud.SelectOne(command);
		}

		///<summary>Used in TP module to get one estimate. The List must be all ClaimProcs for this patient. If estimate can't be found, then return null.  The procedure is always status TP, so there shouldn't be more than one estimate for one plan.</summary>
		public static ClaimProc GetEstimate(List<ClaimProc> claimProcList,long procNum,long planNum,long subNum) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<claimProcList.Count;i++) {
				if(claimProcList[i].Status==ClaimProcStatus.Preauth) {
					continue;
				}
				if(claimProcList[i].ProcNum==procNum && claimProcList[i].PlanNum==planNum && claimProcList[i].InsSubNum==subNum) {
					return claimProcList[i];
				}
			}
			return null;
		}

		///<summary>Used once in Account.  The insurance estimate based on all claimprocs with this procNum that are attached to claims. Includes status of NotReceived,Received, and Supplemental. The list can be all ClaimProcs for patient, or just those for this procedure.</summary>
		public static string ProcDisplayInsEst(ClaimProc[] List,long procNum) {
			//No need to check RemotingRole; no call to db.
			double retVal=0;
			for(int i=0;i<List.Length;i++){
				if(List[i].ProcNum==procNum
					//adj ignored
					//capClaim has no insEst yet
					&& (List[i].Status==ClaimProcStatus.NotReceived
					|| List[i].Status==ClaimProcStatus.Received
					|| List[i].Status==ClaimProcStatus.Supplemental)
					){
					retVal+=List[i].InsPayEst;
				}
			}
			return retVal.ToString("F");
		}

		///<summary>Used in Account and in PaySplitEdit. The insurance estimate based on all claimprocs with this procNum, but only for those claimprocs that are not received yet. The list can be all ClaimProcs for patient, or just those for this procedure.</summary>
		public static double ProcEstNotReceived(List<ClaimProc> claimProcList,long procNum) {
			//No need to check RemotingRole; no call to db.
			return claimProcList.Where(x => x.ProcNum==procNum && x.Status==ClaimProcStatus.NotReceived).Select(x => x.InsPayEst).Sum();
		}
		
		///<summary>Used in PaySplitEdit. The insurance amount paid based on all claimprocs with this procNum. The list can be all ClaimProcs for patient, or just those for this procedure.</summary>
		public static double ProcInsPay(List<ClaimProc> claimProcList,long procNum) {
			//No need to check RemotingRole; no call to db.
			return claimProcList.Where(x => x.ProcNum==procNum)
				.Where(x => !x.Status.In(ClaimProcStatus.Preauth,ClaimProcStatus.CapEstimate,ClaimProcStatus.CapComplete,ClaimProcStatus.Estimate))
				.Select(x => x.InsPayAmt).Sum();
		}

		///<summary>Used in PaySplitEdit. The insurance writeoff based on all claimprocs with this procNum. The list can be all ClaimProcs for patient, or just those for this procedure.</summary>
		public static double ProcWriteoff(List<ClaimProc> claimProcList,long procNum) {
			//No need to check RemotingRole; no call to db.
			return claimProcList.Where(x => x.ProcNum==procNum)
				.Where(x => !x.Status.In(ClaimProcStatus.Preauth,ClaimProcStatus.CapEstimate,ClaimProcStatus.CapComplete,ClaimProcStatus.Estimate))
				.Select(x => x.WriteOff).Sum();
		}

		///<summary>Used in E-claims to get the amount paid by primary. The insurance amount paid by other subNums based on all claimprocs with this procNum. The list can be all ClaimProcs for patient, or just those for this procedure.</summary>
		public static double ProcInsPayPri(List<ClaimProc> claimProcList,long procNum,long subNumExclude) {
			//No need to check RemotingRole; no call to db.
			double retVal=0;
			for(int i=0;i<claimProcList.Count;i++) {
				if(claimProcList[i].ProcNum==procNum
					&& claimProcList[i].InsSubNum!=subNumExclude
					&& claimProcList[i].Status!=ClaimProcStatus.Preauth
					&& claimProcList[i].Status!=ClaimProcStatus.CapEstimate
					&& claimProcList[i].Status!=ClaimProcStatus.CapComplete
					&& claimProcList[i].Status!=ClaimProcStatus.Estimate)
				{
					retVal+=claimProcList[i].InsPayAmt;
				}
			}
			return retVal;
		}

		public static bool IsValidClaimAdj(ClaimProc claimProc,long procNum,long subNumExclude) {
			//No need to check RemotingRole; no call to db.
			if(claimProc.ProcNum!=procNum) {
				return false;
			}
			if(claimProc.InsSubNum==subNumExclude) {
				return false;
			}
			if(claimProc.Status==ClaimProcStatus.CapClaim 
				//|| claimProc.Status==ClaimProcStatus.NotReceived //7/9/2013 Was causing paid amounts to show on primary claims when the patient had secondary insurance, because this is the starting status of secondary claimprocs when the New Claim button is pressed.
				|| claimProc.Status==ClaimProcStatus.Received 
				|| claimProc.Status==ClaimProcStatus.Supplemental)
				//Adjustment never attached to proc. Preauth, CapEstimate, CapComplete, and Estimate never paid. 
			{
				return true;
			}
			else {
				return false;
			}
		}

		///<summary>Used in E-claims to get the most recent date paid (by primary?). The insurance amount paid by the planNum based on all claimprocs with this procNum. The list can be all ClaimProcs for patient, or just those for this procedure.</summary>
		public static DateTime GetDatePaid(List<ClaimProc> claimProcList,long procNum,long planNum) {
			//No need to check RemotingRole; no call to db.
			DateTime retVal=DateTime.MinValue;
			for(int i=0;i<claimProcList.Count;i++) {
				if(claimProcList[i].ProcNum==procNum
					&& claimProcList[i].PlanNum==planNum
					&& claimProcList[i].Status!=ClaimProcStatus.Preauth
					&& claimProcList[i].Status!=ClaimProcStatus.CapEstimate
					&& claimProcList[i].Status!=ClaimProcStatus.CapComplete
					&& claimProcList[i].Status!=ClaimProcStatus.Estimate) 
				{
					if(claimProcList[i].DateCP > retVal) {
						retVal=claimProcList[i].DateCP;
					}
				}
			}
			return retVal;
		}

		///<summary>Used once in Account on the Claim line.  The amount paid on a claim only by total, not including by procedure.  The list can be all ClaimProcs for patient, or just those for this claim.</summary>
		public static double ClaimByTotalOnly(ClaimProc[] List,long claimNum) {
			//No need to check RemotingRole; no call to db.
			double retVal=0;
			for(int i=0;i<List.Length;i++){
				if(List[i].ClaimNum==claimNum
					&& List[i].ProcNum==0
					&& List[i].Status!=ClaimProcStatus.Preauth){
					retVal+=List[i].InsPayAmt;
				}
			}
			return retVal;
		}

		///<summary>Used once in Account on the Claim line.  The writeoff amount on a claim only by total, not including by procedure.  The list can be all ClaimProcs for patient, or just those for this claim.</summary>
		public static double ClaimWriteoffByTotalOnly(ClaimProc[] List,long claimNum) {
			//No need to check RemotingRole; no call to db.
			double retVal=0;
			for(int i=0;i<List.Length;i++) {
				if(List[i].ClaimNum==claimNum
					&& List[i].ProcNum==0
					&& List[i].Status!=ClaimProcStatus.Preauth)
				{
					retVal+=List[i].WriteOff;
				}
			}
			return retVal;
		}

		///<summary>Returns the sum of all claimproc writeoff amounts for the specified claim.  If there are claimprocs provided in the list it will not include those in the sum.</summary>
		public static double GetClaimWriteOffTotal(long claimNum,long procNum,List<ClaimProc> listClaimProcsExclude) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetDouble(MethodBase.GetCurrentMethod(),claimNum,procNum,listClaimProcsExclude);
			}
			string command="SELECT * FROM claimproc WHERE ClaimNum="+POut.Long(claimNum)+" AND ProcNum="+POut.Long(procNum)+" AND Status IN("+(int)ClaimProcStatus.Received+","+(int)ClaimProcStatus.Supplemental+")";
			List<ClaimProc> listClaimProcs=Crud.ClaimProcCrud.SelectMany(command);
			decimal writeoffTotal=0;//decimal used to prevent rounding errors.
			foreach(ClaimProc claimProc in listClaimProcs) {
				if(listClaimProcsExclude.Exists(x => x.ClaimProcNum==claimProc.ClaimProcNum)) {
					continue;//Don't sum together the current claimprocs that are being edited.
				}
				writeoffTotal+=(decimal)claimProc.WriteOff;
			}
			return (double)writeoffTotal;
		}

		///<summary>Attaches or detaches claimprocs from the specified claimPayment. Updates all claimprocs on a claim with one query.  It also updates their DateCP's to match the claimpayment date.</summary>
		public static void SetForClaimOld(long claimNum,long claimPaymentNum,DateTime date,bool setAttached) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),claimNum,claimPaymentNum,date,setAttached);
				return;
			}
			string command= "UPDATE claimproc SET ClaimPaymentNum = ";
			if(setAttached){
				command+=""+POut.Long(claimPaymentNum)+" ";
			}
			else{
				command+="0 ";
			}
			command+=",DateCP="+POut.Date(date)+" "
				+"WHERE ClaimNum="+POut.Long(claimNum)+" AND "
				+"InsPayAmt!=0 AND ("
				+"ClaimPaymentNum="+POut.Long(claimPaymentNum)+" OR ClaimPaymentNum=0)";
			//MessageBox.Show(string command);
 			Db.NonQ(command);
		}

		///<summary>Attaches claimprocs to the specified claimPayment. Updates all claimprocs on a claim with one query.  It also updates their DateCP's to match the claimpayment date.</summary>
		public static void AttachToPayment(long claimNum,long claimPaymentNum,DateTime date,int paymentRow) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),claimNum,claimPaymentNum,date,paymentRow);
				return;
			}
			string command= "UPDATE claimproc SET ClaimPaymentNum="+POut.Long(claimPaymentNum)+", "
				+"DateCP="+POut.Date(date)+", "
				+"PaymentRow="+POut.Int(paymentRow)+", "
				+"DateInsFinalized = (CASE DateInsFinalized WHEN '0001-01-01' THEN "+DbHelper.Now()+" ELSE DateInsFinalized END) "
				+"WHERE ClaimNum="+POut.Long(claimNum)+" "
				+"AND ClaimPaymentNum=0";
			Db.NonQ(command);
		}

		///<summary>Detaches claimprocs from the specified claimPayment. Updates all claimprocs on a list of claims with one query.</summary>
		public static void DetachFromPayment(List<long> listClaimNums,long claimPaymentNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listClaimNums,claimPaymentNum);
				return;
			}
			if(listClaimNums==null || listClaimNums.Count==0) {
				return;
			}
			string command="UPDATE claimproc SET "
				+"DateInsFinalized='0001-01-01' "
				+"WHERE ClaimPaymentNum="+POut.Long(claimPaymentNum)+" "
				+"AND (SELECT SecDateEntry FROM claimpayment WHERE ClaimPaymentNum="+POut.Long(claimPaymentNum)+")="+DbHelper.Curdate()+" "
				+"AND ClaimNum IN("+string.Join(",",listClaimNums.Select(x => POut.Long(x)))+")";
			Db.NonQ(command);
			command="UPDATE claimproc SET ClaimPaymentNum=0, "
				//+"DateCP="+POut.Date(DateTime.MinValue)+", "
				+"PaymentRow=0 "
				+"WHERE ClaimNum IN ("+string.Join(",",listClaimNums.Select(x => POut.Long(x)))+") "
				+"AND ClaimPaymentNum="+POut.Long(claimPaymentNum);
			Db.NonQ(command);
		}

		///<summary>Synchs all claimproc DateCP's attached to the claim payment.  Used when an insurance check's date is changed.</summary>
		public static void SynchDateCP(long claimPaymentNum,DateTime date) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),claimPaymentNum,date);
				return;
			}
			string command= "UPDATE claimproc SET "
				+"DateCP="+POut.Date(date)+" "
				+"WHERE ClaimPaymentNum="+POut.Long(claimPaymentNum);
			Db.NonQ(command);
		}

		/*
		///<summary>Detaches claimprocs from the specified claimPayment. Updates all claimprocs on a claim with one query.  Sets DateCP to min and InsPayAmt to 0.</summary>
		public static void DettachClaimPayment(long claimNum,long claimPaymentNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),claimNum,claimPaymentNum);
				return;
			}
			string command="UPDATE claimproc "
				+"SET ClaimPaymentNum = 0,"
				+"DateCP = "+POut.Date(DateTime.MinValue)+","
				+"InsPayAmt = 0, "
				+"Status = "+POut.Int((int)ClaimProcStatus.NotReceived)+" "
				+"WHERE ClaimNum="+POut.Long(claimNum)+" "
				+"AND ClaimPaymentNum="+POut.Long(claimPaymentNum);
 			Db.NonQ(command);
			command="UPDATE claim "
				+"SET ClaimStatus = 'S' "
				+"WHERE ClaimNum="+POut.Long(claimNum);
			Db.NonQ(command);
		}*/

		/*
		///<summary></summary>
		public static double ComputeBal(ClaimProc[] List){
			//No need to check RemotingRole; no call to db.
			double retVal=0;
			//double pat;
			for(int i=0;i<List.Length;i++){
				if(List[i].Status==ClaimProcStatus.Adjustment//ins adjustments do not affect patient balance
					|| List[i].Status==ClaimProcStatus.Preauth//preauthorizations do not affect patient balance
					|| List[i].Status==ClaimProcStatus.Estimate//estimates do not affect patient balance
					|| List[i].Status==ClaimProcStatus.CapEstimate//CapEstimates do not affect patient balance
					){
					continue;
				}
				if(List[i].Status==ClaimProcStatus.Received
					|| List[i].Status==ClaimProcStatus.Supplemental//because supplemental are always received
					|| List[i].Status==ClaimProcStatus.CapClaim)//would only have a payamt if received
				{
					retVal-=List[i].InsPayAmt;
					retVal-=List[i].WriteOff;
				}
				else if(List[i].Status==ClaimProcStatus.NotReceived) {
					if(!PrefC.GetBool(PrefName.BalancesDontSubtractIns")) {//this typically happens
						retVal-=List[i].InsPayEst;
						retVal-=List[i].WriteOff;
					}
				}
			}
			return retVal;
		}*/

		///<summary>After entering estimates from a preauth, this routine is called for each proc to override the ins est.</summary>
		public static void SetInsEstTotalOverride(long procNum,long planNum,double insPayEst,List<ClaimProc> claimProcList) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<claimProcList.Count;i++) {
				if(procNum!=claimProcList[i].ProcNum) {
					continue;
				}
				if(planNum!=claimProcList[i].PlanNum) {
					continue;
				}
				if(claimProcList[i].Status!=ClaimProcStatus.Estimate) {
					continue;
				}
				claimProcList[i].InsEstTotalOverride=insPayEst;
				Update(claimProcList[i]);
			}
		}

		///<summary>Calculates the Base estimate, InsEstTotal, and all the other insurance numbers for a single claimproc.  This is not done on the fly.
		///Use Procedure.GetEst to later retrieve the estimate. This function replaces all of the upper estimating logic that was within FormClaimProc.
		///BaseEst=((fee or allowedOverride)-Copay) x (percentage or percentOverride).
		///The calling class must have already created the claimProc, this function simply updates the BaseEst field of that claimproc. pst.Tot not used.
		///For Estimate and CapEstimate, all the estimate fields will be recalculated except the overrides.  histList and loopList can be null.
		///If so, then deductible and annual max will not be recalculated.  histList and loopList may only make sense in TP module and claimEdit.
		///loopList contains all claimprocs in the current list (TP or claim) that come before this procedure.
		///PaidOtherInsTot should only contain sum of InsEstTotal/Override, or paid, depending on the status.
		///PaidOtherInsBase also includes actual payments.</summary>
		public static void ComputeBaseEst(ClaimProc cp,Procedure proc,InsPlan plan,long patPlanNum,List<Benefit> benList,List<ClaimProcHist> histList
			,List<ClaimProcHist> loopList,List<PatPlan> patPlanList,double paidOtherInsTot,double paidOtherInsBase,int patientAge,double writeOffOtherIns
			,List<InsPlan> listInsPlans,List<InsSub> listInsSubs,List<SubstitutionLink> listSubLinks,bool useProcDateOnProc=false) 
		{
			//No need to check RemotingRole; no call to db.
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA") 
				&& plan.PlanType=="" 
				&& proc.ProcNumLab!=0) 
			{
				//This is a lab. Do not allow it to calculate its own estimates.
				//Instead use parents associated claimProc to calcualte estimates.
				Procedure procParent=Procedures.GetOneProc(proc.ProcNumLab,false);
				List<ClaimProc> listClaimProcs=ClaimProcs.RefreshForProc(procParent.ProcNum);
				Procedures.ComputeEstimates(procParent,procParent.PatNum,listClaimProcs,false,listInsPlans,patPlanList,benList,patientAge,listInsSubs);
				return;
			}
			double procFee=proc.ProcFee*Math.Max(1,proc.BaseUnits+proc.UnitQty);
			string toothNum=proc.ToothNum;
			long codeNum=proc.CodeNum;
			if(cp.Status==ClaimProcStatus.CapClaim
				|| cp.Status==ClaimProcStatus.CapComplete
				|| cp.Status==ClaimProcStatus.Preauth
				|| cp.Status==ClaimProcStatus.Supplemental) {
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA") && plan.PlanType=="" && cp.Status==ClaimProcStatus.Preauth) {//Canada and Category Percentage
					List<Procedure> listLabFees=Procedures.GetCanadianLabFees(proc.ProcNum);
					foreach(Procedure procCur in listLabFees) {
						CanadianLabBaseEstHelper(cp,procCur,plan,cp.InsSubNum,proc,benList,patPlanNum,histList,loopList,patientAge,useProcDateOnProc);
					}
				}
				return;//never compute estimates for those types listed above.
			}
			if(plan.PlanType=="c"//if capitation plan
				&& cp.Status==ClaimProcStatus.Estimate)//and ordinary estimate
			{
				cp.Status=ClaimProcStatus.CapEstimate;
			}
			if(plan.PlanType!="c"//if not capitation plan
				&& cp.Status==ClaimProcStatus.CapEstimate)//and estimate is a capitation estimate
			{
				cp.Status=ClaimProcStatus.Estimate;
			}
			//NoBillIns is only calculated when creating the claimproc, even if resetAll is true.
			//If user then changes a procCode, it does not cause an update of all procedures with that code.
			if(cp.NoBillIns) {
				cp.AllowedOverride=-1;
				cp.CopayAmt=0;
				cp.CopayOverride=-1;
				cp.Percentage=-1;
				cp.PercentOverride=-1;
				cp.DedEst=-1;
				cp.DedEstOverride=-1;
				cp.PaidOtherIns=-1;
				cp.BaseEst=0;
				cp.InsEstTotal=0;
				cp.InsEstTotalOverride=-1;
				cp.WriteOff=0;
				cp.PaidOtherInsOverride=-1;
				cp.WriteOffEst=-1;
				cp.WriteOffEstOverride=-1;
				cp.EstimateNote="";
				return;
			}
			cp.EstimateNote="";
			//This function is called every time a ProcFee is changed,
			//so the BaseEst does reflect the new ProcFee.
			//ProcFee----------------------------------------------------------------------------------------------
			cp.BaseEst=procFee;
			cp.InsEstTotal=procFee;
			//Allowed----------------------------------------------------------------------------------------------
			double allowed=procFee;//could be fee, or could be a little less.  Used further down in paidOtherIns.
			bool codeSubstNone=(!SubstitutionLinks.HasSubstCodeForPlan(plan,codeNum,listSubLinks));//Left variable name alone when substitution links added.
			if(cp.AllowedOverride!=-1) {
				if(cp.AllowedOverride > procFee){
					cp.AllowedOverride=procFee;
				}
				allowed=cp.AllowedOverride;
				cp.BaseEst=cp.AllowedOverride;
				cp.InsEstTotal=cp.AllowedOverride;
			}
			else if(plan.PlanType=="c"){//capitation estimate.  No allowed fee sched.  No substitute codes.
				allowed=procFee;
				cp.BaseEst=procFee;
				cp.InsEstTotal=procFee;
			}
			else {
				//no point in wasting time calculating this unless it's needed.
				double carrierAllowed=InsPlans.GetAllowed(ProcedureCodes.GetProcCode(codeNum).ProcCode,plan.FeeSched,plan.AllowedFeeSched,
					codeSubstNone,plan.PlanType,toothNum,proc.ProvNum,proc.ClinicNum);
				if(carrierAllowed != -1) {
					carrierAllowed=carrierAllowed*Math.Max(1,proc.BaseUnits+proc.UnitQty);
					if(carrierAllowed > procFee) {
						allowed=procFee;
						cp.BaseEst=procFee;
						cp.InsEstTotal=procFee;
					}
					else {
						allowed=carrierAllowed;
						cp.BaseEst=carrierAllowed;
						cp.InsEstTotal=carrierAllowed;
					}
				}
			}
			//Copay----------------------------------------------------------------------------------------------
			FeeSched feeSchedCopay=FeeScheds.GetFirstOrDefault(x => x.FeeSchedNum==plan.CopayFeeSched);
			if(plan.PlanType=="p" && feeSchedCopay!=null && feeSchedCopay.FeeSchedType==FeeScheduleType.FixedBenefit) {
				Fee feeFixedBenefit=Fees.GetFee(codeNum,feeSchedCopay.FeeSchedNum,proc.ClinicNum,proc.ProvNum);
				Fee feePpo=Fees.GetFee(codeNum,plan.FeeSched,proc.ClinicNum,proc.ProvNum);
				if(feePpo==null) {
					//No fee defined for this procedure, use ProcFee because it is assumed to be a 100% coverage with PPO plans
					cp.CopayAmt=procFee;
				}
				else {
					cp.CopayAmt=feePpo.Amount;
				}
				if(feeFixedBenefit!=null && feeFixedBenefit.Amount>0) {
					cp.CopayAmt-=feeFixedBenefit.Amount;//Deduct the fixed benefit amount from the proc fee to determine the copay for this claimproc.
				}
			}
			else {
				cp.CopayAmt=InsPlans.GetCopay(codeNum,plan.FeeSched,plan.CopayFeeSched,codeSubstNone,toothNum,proc.ClinicNum,proc.ProvNum);
			}
			if(cp.CopayAmt!=-1) {
				cp.CopayAmt=cp.CopayAmt*Math.Max(1,proc.BaseUnits+proc.UnitQty);
			}
			if(cp.CopayAmt > allowed) {//if the copay is greater than the allowed fee calculated above
				cp.CopayAmt=allowed;//reduce the copay
			}
			if(cp.CopayOverride > allowed) {//or if the copay override is greater than the allowed fee calculated above
				cp.CopayOverride=allowed;//reduce the override
			}
			if(cp.Status==ClaimProcStatus.CapEstimate) {
				//this does automate the Writeoff. If user does not want writeoff automated,
				//then they will have to complete the procedure first. (very rare)
				if(cp.CopayAmt==-1) {
					cp.CopayAmt=0;
				}
				if(cp.CopayOverride != -1) {//override the copay
					cp.WriteOffEst=cp.BaseEst-cp.CopayOverride;
				}
				else if(cp.CopayAmt!=-1) {//use the calculated copay
					cp.WriteOffEst=cp.BaseEst-cp.CopayAmt;
				}
				if(cp.WriteOffEst<0) {
					cp.WriteOffEst=0;
				}
				cp.WriteOff=cp.WriteOffEst;
				cp.DedApplied=0;
				cp.DedEst=0;
				cp.Percentage=-1;
				cp.PercentOverride=-1;
				cp.BaseEst=0;
				cp.InsEstTotal=0;
				return;
			}
			if(cp.CopayOverride != -1) {//subtract copay if override
				cp.BaseEst-=cp.CopayOverride;
				cp.InsEstTotal-=cp.CopayOverride;
			}
			else if(cp.CopayAmt != -1) {//otherwise subtract calculated copay
				cp.BaseEst-=cp.CopayAmt;
				cp.InsEstTotal-=cp.CopayAmt;
			}
			//Deductible----------------------------------------------------------------------------------------
			//The code below handles partial usage of available deductible. 
			DateTime procDate;
			if(useProcDateOnProc) {
				procDate=proc.ProcDate;
			}
			else if(cp.Status==ClaimProcStatus.Estimate) {
				procDate=DateTime.Today;
			}
			else {
				procDate=cp.ProcDate;
			}
			if(loopList!=null && histList!=null) {
				cp.DedEst=Benefits.GetDeductibleByCode(benList,plan.PlanNum,patPlanNum,procDate,ProcedureCodes.GetStringProcCode(codeNum)
					,histList,loopList,plan,cp.PatNum);
			}
			if(Benefits.GetPercent(ProcedureCodes.GetProcCode(codeNum).ProcCode,plan.PlanType,plan.PlanNum,patPlanNum,benList)==0) {//this is binary
				cp.DedEst=0;//Procedure is not covered. Do not apply deductible. This does not take into account percent override.
			}
			if(cp.DedEst > cp.InsEstTotal){//if the deductible is more than the fee
				cp.DedEst=cp.InsEstTotal;//reduce the deductible
			}
			if(cp.DedEstOverride > cp.InsEstTotal) {//if the deductible override is more than the fee
				cp.DedEstOverride=cp.InsEstTotal;//reduce the override.
			}
			if(cp.DedEstOverride != -1) {//use the override
				cp.InsEstTotal-=cp.DedEstOverride;//subtract
			}
			else if(cp.DedEst != -1){//use the calculated deductible
				cp.InsEstTotal-=cp.DedEst;
			}
			//Percentage----------------------------------------------------------------------------------------
			if(plan.PlanType=="p" && feeSchedCopay!=null && feeSchedCopay.FeeSchedType==FeeScheduleType.FixedBenefit) {
				cp.Percentage=100;
			}
			else {
				cp.Percentage=Benefits.GetPercent(ProcedureCodes.GetProcCode(codeNum).ProcCode,plan.PlanType,plan.PlanNum,patPlanNum,benList);//will never =-1
			}
			if(cp.PercentOverride != -1) {//override, so use PercentOverride
				cp.BaseEst=cp.BaseEst*(double)cp.PercentOverride/100d;
				cp.InsEstTotal=cp.InsEstTotal*(double)cp.PercentOverride/100d;
			}
			else if(cp.Percentage != -1) {//use calculated Percentage
				cp.BaseEst=cp.BaseEst*(double)cp.Percentage/100d;
				cp.InsEstTotal=cp.InsEstTotal*(double)cp.Percentage/100d;
			}
			//PaidOtherIns----------------------------------------------------------------------------------------
			//double paidOtherInsActual=GetPaidOtherIns(cp,patPlanList,patPlanNum,histList);//can return -1 for primary
			PatPlan pp=PatPlans.GetFromList(patPlanList.ToArray(),patPlanNum);
			//if -1, that indicates primary ins, not a proc, or no histlist.  We should not alter it in this case.
			//if(paidOtherInsActual!=-1) {
			//An older restriction was that histList must not be null.  But since this is now straight from db, that's not restriction.
			if(pp==null) {
				//corruption.  Do nothing.
			}
			else if(pp.Ordinal==1 || cp.ProcNum==0){
				cp.PaidOtherIns=0;
			}
			else{//if secondary or greater
				//The normal calculation uses the InsEstTotal from the primary ins.
				//But in TP module, if not using max and deduct, then the amount estimated to be paid by primary will be different.
				//It will use the primary BaseEst instead of the primary InsEstTotal.
				//Since the only use of BaseEst here is to handle this alternate viewing in the TP,
				//the secondary BaseEst should use the primary BaseEst when calculating paidOtherIns.
				//The BaseEst will, however, use PaidOtherInsOverride, if user has entered one.
				//This calculation doesn't need to be accurate unless viewing TP,
				//so it's ok to pass in a dummy value, like paidOtherInsTotal.
				//We do InsEstTotal first
				//cp.PaidOtherIns=paidOtherInsActual+paidOtherInsEstTotal;
				cp.PaidOtherIns=paidOtherInsTot;
				double paidOtherInsTotTemp=cp.PaidOtherIns;
				if(cp.PaidOtherInsOverride != -1) {//use the override
					paidOtherInsTotTemp=cp.PaidOtherInsOverride;
				}
				//example: Fee:200, InsEstT:80, BaseEst:100, PaidOI:110.
				//So... MaxPtP:90.
				//Since InsEstT is not greater than MaxPtoP, no change.
				//Since BaseEst is greater than MaxPtoP, BaseEst changed to 90.
				if(paidOtherInsTotTemp != -1) {
					double maxPossibleToPay=0;
					if(plan.CobRule.In(EnumCobRule.Basic,EnumCobRule.SecondaryMedicaid)) {
						maxPossibleToPay=allowed-paidOtherInsTotTemp;
					}
					else if(plan.CobRule==EnumCobRule.Standard) {
						double patPortionTot=procFee - paidOtherInsTotTemp - writeOffOtherIns;//patPortion for InsEstTotal
						maxPossibleToPay=Math.Min(cp.BaseEst,patPortionTot);//The lesser of what insurance would pay if they were primary, and the patient portion.
					}
					else{//plan.CobRule==EnumCobRule.CarveOut
						maxPossibleToPay=cp.InsEstTotal - paidOtherInsTotTemp;
					}
					if(maxPossibleToPay<0) {
						maxPossibleToPay=0;
					}
					if(cp.InsEstTotal > maxPossibleToPay) {
						cp.InsEstTotal=maxPossibleToPay;//reduce the estimate
					}
				}
				//Then, we do BaseEst
				double paidOtherInsBaseTemp=paidOtherInsBase;//paidOtherInsActual+paidOtherInsBaseEst;
				if(cp.PaidOtherInsOverride != -1) {//use the override
					paidOtherInsBaseTemp=cp.PaidOtherInsOverride;
				}
				if(paidOtherInsBaseTemp != -1) {
					double maxPossibleToPay=0;
					if(plan.CobRule.In(EnumCobRule.Basic,EnumCobRule.SecondaryMedicaid)) {
						maxPossibleToPay=allowed-paidOtherInsBaseTemp;
					}
					else if(plan.CobRule==EnumCobRule.Standard) {
						double patPortionBase=procFee - paidOtherInsBaseTemp - writeOffOtherIns;//patPortion for BaseEst
						maxPossibleToPay=Math.Min(cp.BaseEst,patPortionBase);
					}
					else {//plan.CobRule==EnumCobRule.CarveOut
						maxPossibleToPay=cp.BaseEst - paidOtherInsBaseTemp;
					}
					if(maxPossibleToPay<0) {
						maxPossibleToPay=0;
					}
					if(cp.BaseEst > maxPossibleToPay) {
						cp.BaseEst=maxPossibleToPay;//reduce the base est
					}
				}
			}
			//Canadian Lab Fee Estimates-------------------------------------------------------------------------------------------------------------------
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA") && plan.PlanType==""){//Canada and Category Percentage
				List<Procedure> listLabFees=Procedures.GetCanadianLabFees(proc.ProcNum);
				foreach(Procedure procCur in listLabFees) {
					CanadianLabBaseEstHelper(cp,procCur,plan,cp.InsSubNum,proc,benList,patPlanNum,histList,loopList,patientAge,useProcDateOnProc);
				}
			}
			//Exclusions---------------------------------------------------------------------------------------
			//We are not going to consider date of proc.  Just simple exclusions
			if(Benefits.IsExcluded(ProcedureCodes.GetStringProcCode(codeNum),benList,plan.PlanNum,patPlanNum)) {
				cp.BaseEst=0;
				cp.InsEstTotal=0;
				if(cp.EstimateNote!="") {
					cp.EstimateNote+=", ";
				}
				cp.EstimateNote+=Lans.g("ClaimProcs","Exclusion");
			}
			//base estimate is now done and will not be altered further.  From here out, we are only altering insEstTotal
			//annual max and other limitations--------------------------------------------------------------------------------
			if(loopList!=null && histList!=null) {
				string note="";
				cp.InsEstTotal=Benefits.GetLimitationByCode(benList,plan.PlanNum,patPlanNum,procDate
					,ProcedureCodes.GetStringProcCode(codeNum),histList,loopList,plan,cp.PatNum,out note,cp.InsEstTotal,patientAge
					,cp.InsSubNum,cp.InsEstTotalOverride);
				if(note != "") {
					if(cp.EstimateNote != "") {
						cp.EstimateNote+=", ";
					}
					cp.EstimateNote+=note;
				}
			}
			//procDate;//was already calculated in the deductible section.
			//Writeoff Estimate------------------------------------------------------------------------------------------
			if(plan.CobRule==EnumCobRule.SecondaryMedicaid && PatPlans.GetOrdinal(cp.InsSubNum,patPlanList)!=1) {
				//If a plan is Secondary Medicaid, any amount that has not been written off by another insurance or paid will be written off. This should
				//cause the patient portion to be 0.
				cp.WriteOffEst=procFee-paidOtherInsTot-writeOffOtherIns-cp.InsEstTotal;
			}
			else if(plan.PlanType=="p") {//PPO
				//we can't use the allowed previously calculated, because it might be the allowed of a substituted code.
				//so we will calculate the allowed all over again, but this time, without using a substitution code.
				//AllowedFeeSched and toothNum do not need to be passed in.  codeSubstNone is set to true to not subst.
				double carrierAllowedNoSubst=InsPlans.GetAllowed(ProcedureCodes.GetProcCode(codeNum).ProcCode,plan.FeeSched,0,
					true,"p","",proc.ProvNum,proc.ClinicNum);
				double allowedNoSubst=procFee;
				if(carrierAllowedNoSubst != -1) {
					carrierAllowedNoSubst=carrierAllowedNoSubst*Math.Max(1,proc.BaseUnits+proc.UnitQty);
					if(carrierAllowedNoSubst > procFee) {
						allowedNoSubst=procFee;
					}
					else {
						allowedNoSubst=carrierAllowedNoSubst;
					}
				}
				double normalWriteOff=procFee-allowedNoSubst;//This is what the normal writeoff would be if no other insurance was involved.
				if(normalWriteOff<0) {
					normalWriteOff=0;
				}
				double remainingWriteOff=procFee-paidOtherInsTot-writeOffOtherIns;//This is the fee minus whatever other ins has already paid or written off.
				if(remainingWriteOff<0) {
					remainingWriteOff=0;
				}
				if((!PrefC.GetBool(PrefName.InsPPOsecWriteoffs) && paidOtherInsTot>0) || writeOffOtherIns>0) {
					//This pref solves a conflict between two customers.  One customer paid for a past feature request.
					//They need this new preference because they have a non-PPO as primary and a pseudo-PPO (Medicaid flagged as PPO) as secondary.
					//When the pref is true, then secondary writeoffs are only included if other insurance has no writeoffs already.  This is how we used to calculate secondary writeoffs for everyone.
					//When the pref is false (default), then no secondary writeoff estimates allowed.  Only primary may have writeoffs.  If no other insurance payments/estimates/writeoffs, then the current writeoff is calculated as primary.
					cp.WriteOffEst=0;//The reasoning for this is covered in the manual under Unit Test #1 and COB.
				}
				//We can't go over either number.  We must use the smaller of the two.  If one of them is zero, then the writeoff is zero.
				else if(remainingWriteOff==0 || normalWriteOff==0) {
					cp.WriteOffEst=0;
				}
				else if(remainingWriteOff<=normalWriteOff) {
					cp.WriteOffEst=remainingWriteOff;
				}
				else {
					cp.WriteOffEst=normalWriteOff;
				}
			}
			//capitation calculation never makes it this far:
			//else if(plan.PlanType=="c") {//capitation
			//	cp.WriteOffEst=cp.WriteOff;//this probably needs to change
			//}
			else {
				cp.WriteOffEst=-1;
			}
			//Round now to prevent the sum of the InsEstTotal and the Patient Portion from being 1 cent more than the Proc Fee
			cp.BaseEst=Math.Round(cp.BaseEst,2);
			cp.InsEstTotal=Math.Round(cp.InsEstTotal,2);
			//Calculations done, copy over estimates from InsEstTotal into InsPayEst.  
			//This could potentially be limited to claimprocs status Recieved or NotReceived, but there likely is no harm in doing it for all claimprocs.
			if(!cp.InsEstTotalOverride.IsEqual(-1)) {
				cp.InsPayEst=cp.InsEstTotalOverride;
			}
			else {
				cp.InsPayEst=cp.InsEstTotal;
			}
		}

		///<summary>Determine which date to use for procDate when computing base estimate.</summary>
		private static DateTime GetProcDate(Procedure proc,ClaimProc cp,bool useProcDateOnProc) {
			DateTime procDate;
			if(useProcDateOnProc) {
				procDate=proc.ProcDate;
			}
			else if(cp.Status==ClaimProcStatus.Estimate) {
				procDate=DateTime.Today;
			}
			else {
				procDate=cp.ProcDate;
			}
			return procDate;
		}

		///<summary>Append a string to the ClaimProc's EstimateNote field.</summary>
		private static void AppendToEstimateNote(ClaimProc cp,string note) {
			if(string.IsNullOrEmpty(note)) {
				return;
			}
			if(cp.EstimateNote!="") {
				cp.EstimateNote+=", ";
			}
			cp.EstimateNote+=note;
		}

		///<summary>Update or create a claimProc for the given Canadian lab procedure (procLab).
		///Calculations are based on the percentage from the parent claim procs percentage (procParent).</summary>
		private static void CanadianLabBaseEstHelper(ClaimProc claimProcParent,Procedure procLab,InsPlan plan,long insSubNum,
			Procedure procParent,List<Benefit> listBenefits,long patPlanNum,List<ClaimProcHist> histList,List<ClaimProcHist> loopList,
			int patientAge,bool useProcDateOnProc)
		{
			//No need to check RemotingRole; no call to db.
			if(histList==null) {
				histList=new List<ClaimProcHist>();
			}
			if(loopList==null) {
				loopList=new List<ClaimProcHist>();
			}
			double percentage=(double)claimProcParent.Percentage;
			if(claimProcParent.PercentOverride!=-1) {
				percentage=(double)claimProcParent.PercentOverride;
			}
			string note="";
			double estAmt=procLab.ProcFee*percentage/100d;
			bool isPreauth=(claimProcParent.Status==ClaimProcStatus.Preauth);
			List<ClaimProc> listClaimProcs=ClaimProcs.RefreshForProc(procLab.ProcNum)//Get all claimProcs for this lab (0, 1 or 2).
				.FindAll(x => x.InsSubNum==claimProcParent.InsSubNum && x.PlanNum==claimProcParent.PlanNum//Same subscriber and insurance.
					&& x.Status==claimProcParent.Status//Ensure parent proc and lab proc have same status.
					&& (isPreauth?x.ClaimNum==claimProcParent.ClaimNum:true));//Preauths claimProcs can be sent to same insurance many times, claim specific.
			if(listClaimProcs.Count > 0) {//There exists 1 or 2 estimates for the current lab proc.
				listClaimProcs.ForEach(x => { 
					x.BaseEst=estAmt;
					x.InsEstTotal=estAmt;
					x.InsPayEst=ClaimProcs.GetInsEstTotal(x);
					x.FeeBilled=procLab.ProcFee;
					x.Status=claimProcParent.Status;
					x.ProvNum=claimProcParent.ProvNum;
					x.Percentage=claimProcParent.Percentage;
					x.PercentOverride=claimProcParent.PercentOverride;
					if(x.Status.In(ClaimProcStatus.Received,ClaimProcStatus.Supplemental)) {
						x.DateEntry=claimProcParent.DateEntry;
					}
					x.ClaimNum=claimProcParent.ClaimNum;
					x.EstimateNote="";//Cannot be edited by user, no risk of deleting user input.
					note="";
					x.InsEstTotal=Benefits.GetLimitationByCode(listBenefits,plan.PlanNum,patPlanNum//Consider annual/lifetime max, benefits, etc
						,GetProcDate(procParent,claimProcParent,useProcDateOnProc),x.CodeSent,histList
						,loopList,plan,x.PatNum,out note,x.InsEstTotal,patientAge
						,x.InsSubNum,x.InsEstTotalOverride);
					AppendToEstimateNote(x,note);
					//next proc needs to know this one is already added, 
					loopList.AddRange(ClaimProcs.GetHistForProc(new List<ClaimProc> () {x},procLab.ProcNum,procLab.CodeNum));
					Update(x);
				});
				return;
			}
			//Create a new ClaimProc since we couldn't find one for this Lab.
			ClaimProc claimProcLab=new ClaimProc();
			claimProcLab.InsPayEst=estAmt;
			InsSub sub=new InsSub();
			sub.InsSubNum=insSubNum;
			CreateEst(claimProcLab,procLab,plan,sub,estAmt,estAmt);
			claimProcLab.Status=claimProcParent.Status;
			claimProcLab.Percentage=claimProcParent.Percentage;
			claimProcLab.PercentOverride=claimProcParent.PercentOverride;
			if(claimProcLab.Status.In(ClaimProcStatus.Received,ClaimProcStatus.Supplemental)) {
				claimProcLab.DateEntry=claimProcParent.DateEntry;
			}
			claimProcLab.ClaimNum=claimProcParent.ClaimNum;
			claimProcLab.CodeSent=ProcedureCodes.GetStringProcCode(procLab.CodeNum);
			if(claimProcLab.CodeSent.Length>5) { //In Canadian electronic claims, codes can contain letters or numbers and cannot be longer than 5 characters.
				claimProcLab.CodeSent=claimProcLab.CodeSent.Substring(0,5);
			}
			claimProcLab.EstimateNote="";//Cannot be edited by user, no risk of deleting user input.
			note="";
			claimProcLab.InsEstTotal=Benefits.GetLimitationByCode(listBenefits,plan.PlanNum,patPlanNum
				,GetProcDate(procParent,claimProcParent,useProcDateOnProc),claimProcLab.CodeSent,histList
				,loopList,plan,claimProcLab.PatNum,out note,claimProcLab.InsEstTotal,patientAge
				,claimProcLab.InsSubNum,claimProcLab.InsEstTotalOverride);
			AppendToEstimateNote(claimProcLab,note);
			loopList.AddRange(ClaimProcs.GetHistForProc(new List<ClaimProc>() {claimProcLab},procLab.ProcNum,procLab.CodeNum));
			Update(claimProcLab);
		}

		/*
		///<summary>We don't care about a looplist because those would be for different procedures.  So this calculation really only makes sense when calculating secondary insurance in the claim edit window or when calculating secondary estimates in the TP module.  HistList will include actual payments and estimated pending payments for this proc, but it will not include primary estimates.  Estimates are not handled here, but are instead passed in to ComputeBaseEst</summary>
		private static double GetPaidOtherIns(ClaimProc cp,List<PatPlan> patPlanList,long patPlanNum,List<ClaimProcHist> histList) {
			if(cp.ProcNum==0) {
				return -1;
			}
			if(histList==null) {
				return -1;
			}
			PatPlan pp=PatPlans.GetFromList(patPlanList.ToArray(),patPlanNum);
			if(pp==null) {
				return -1;
			}
			int thisOrdinal=pp.Ordinal;
			if(thisOrdinal==1) {
				return -1;
			}
			double retVal=0;
			int ordinal;
			for(int i=0;i<histList.Count;i++) {
				ordinal=PatPlans.GetOrdinal(patPlanList,cp.PlanNum);
				if(ordinal >= thisOrdinal){
					continue;
				}
				retVal+=histList[i].Amount;
			}
			return retVal;
		}*/

		///<summary>Only useful if secondary ins or greater.  For one procedure, it gets the sum of InsEstTotal/Override for other insurances with lower ordinals.  Either estimates or actual payments.  Will return 0 if ordinal of this claimproc is 1.</summary>
		public static double GetPaidOtherInsTotal(ClaimProc cp,List<PatPlan> patPlanList) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<double>(MethodBase.GetCurrentMethod(),cp,patPlanList);
			}
			if(cp.ProcNum==0) {
				return 0;
			}
			int thisOrdinal=PatPlans.GetOrdinal(cp.InsSubNum,patPlanList);
			if(thisOrdinal==1) {
				return 0;
			}
			string command="SELECT InsSubNum,InsEstTotal,InsEstTotalOverride,InsPayAmt,Status FROM claimproc WHERE ProcNum="+POut.Long(cp.ProcNum);
			DataTable table=Db.GetTable(command);
			double retVal=0;
			long subNum;
			int ordinal;
			double insEstTotal;
			double insEstTotalOverride;
			double insPayAmt;
			ClaimProcStatus status;
			for(int i=0;i<table.Rows.Count;i++) {
				subNum=PIn.Long(table.Rows[i]["InsSubNum"].ToString());
				ordinal=PatPlans.GetOrdinal(subNum,patPlanList);
				if(ordinal >= thisOrdinal) {
					continue;
				}
				insEstTotal=PIn.Double(table.Rows[i]["InsEstTotal"].ToString());
				insEstTotalOverride=PIn.Double(table.Rows[i]["InsEstTotalOverride"].ToString());
				insPayAmt=PIn.Double(table.Rows[i]["InsPayAmt"].ToString());
				status=(ClaimProcStatus)PIn.Int(table.Rows[i]["Status"].ToString());
				if(status==ClaimProcStatus.Received || status==ClaimProcStatus.Supplemental) 
				{
					retVal+=insPayAmt;
				}
				if(status==ClaimProcStatus.Estimate || status==ClaimProcStatus.NotReceived) 
				{
					if(insEstTotalOverride != -1) {
						retVal+=insEstTotalOverride;
					}
					else {
						retVal+=insEstTotal;
					}
				}
			}
			return retVal;
		}

		///<summary>Only useful if secondary ins or greater.  For one procedure, it gets the sum of BaseEst for other insurances with lower ordinals.  Either estimates or actual payments.  Will return 0 if ordinal of this claimproc is 1.</summary>
		public static double GetPaidOtherInsBaseEst(ClaimProc cp,List<PatPlan> patPlanList) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<double>(MethodBase.GetCurrentMethod(),cp,patPlanList);
			}
			if(cp.ProcNum==0) {
				return 0;
			}
			int thisOrdinal=PatPlans.GetOrdinal(cp.InsSubNum,patPlanList);
			if(thisOrdinal==1) {
				return 0;
			}
			string command="SELECT InsSubNum,BaseEst,InsPayAmt,Status FROM claimproc WHERE ProcNum="+POut.Long(cp.ProcNum);
			DataTable table=Db.GetTable(command);
			double retVal=0;
			long subNum;
			int ordinal;
			double baseEst;
			double insPayAmt;
			ClaimProcStatus status;
			for(int i=0;i<table.Rows.Count;i++) {
				subNum=PIn.Long(table.Rows[i]["InsSubNum"].ToString());
				ordinal=PatPlans.GetOrdinal(subNum,patPlanList);
				if(ordinal >= thisOrdinal) {
					continue;
				}
				baseEst=PIn.Double(table.Rows[i]["BaseEst"].ToString());
				insPayAmt=PIn.Double(table.Rows[i]["InsPayAmt"].ToString());
				status=(ClaimProcStatus)PIn.Int(table.Rows[i]["Status"].ToString());
				if(status==ClaimProcStatus.Received || status==ClaimProcStatus.Supplemental) {
					retVal+=insPayAmt;
				}
				if(status==ClaimProcStatus.Estimate || status==ClaimProcStatus.NotReceived) {
					retVal+=baseEst;
				}
			}
			return retVal;
		}

		///<summary>Only useful if secondary ins or greater.  For one procedure, it gets the sum of WriteOffEstimates/Override for other insurances with lower ordinals.  Either estimates or actual writeoffs.  Will return 0 if ordinal of this claimproc is 1.</summary>
		public static double GetWriteOffOtherIns(ClaimProc cp,List<PatPlan> patPlanList) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<double>(MethodBase.GetCurrentMethod(),cp,patPlanList);
			}
			if(cp.ProcNum==0) {
				return 0;
			}
			int thisOrdinal=PatPlans.GetOrdinal(cp.InsSubNum,patPlanList);
			if(thisOrdinal==1) {
				return 0;
			}
			string command="SELECT InsSubNum,WriteOffEst,WriteOffEstOverride,WriteOff,Status FROM claimproc WHERE ProcNum="+POut.Long(cp.ProcNum);
			DataTable table=Db.GetTable(command);
			double retVal=0;
			long subNum;
			int ordinal;
			double writeOffEst;
			double writeOffEstOverride;
			double writeOff;
			ClaimProcStatus status;
			for(int i=0;i<table.Rows.Count;i++) {
				subNum=PIn.Long(table.Rows[i]["InsSubNum"].ToString());
				ordinal=PatPlans.GetOrdinal(subNum,patPlanList);
				if(ordinal >= thisOrdinal) {
					continue;
				}
				writeOffEst=PIn.Double(table.Rows[i]["WriteOffEst"].ToString());
				writeOffEstOverride=PIn.Double(table.Rows[i]["WriteOffEstOverride"].ToString());
				writeOff=PIn.Double(table.Rows[i]["WriteOff"].ToString());
				status=(ClaimProcStatus)PIn.Int(table.Rows[i]["Status"].ToString());
				if(status==ClaimProcStatus.Received || status==ClaimProcStatus.Supplemental) {
					retVal+=writeOff;
				}
				if(status==ClaimProcStatus.Estimate || status==ClaimProcStatus.NotReceived) {
					if(writeOffEstOverride != -1) {
						retVal+=writeOffEstOverride;
					}
					else if(writeOffEst !=-1){
						retVal+=writeOffEst;
					}
				}
			}
			return retVal;
		}

		/////<summary>Only useful if secondary ins or greater.  For one procedure, it gets the sum of WriteOffEstimates/Override for other insurances with lower ordinals.  Either estimates or actual writeoffs.  Will return 0 if ordinal of this claimproc is 1.</summary>
		//public static double GetDeductibleOtherIns(ClaimProc cp,List<PatPlan> patPlanList) {
		//  if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
		//    return Meth.GetObject<double>(MethodBase.GetCurrentMethod(),cp,patPlanList);
		//  }
		//  if(cp.ProcNum==0) {
		//    return 0;
		//  }
		//  int thisOrdinal=PatPlans.GetOrdinal(cp.InsSubNum,patPlanList);
		//  if(thisOrdinal==1) {
		//    return 0;
		//  }
		//  string command="SELECT InsSubNum,DedEst,DedEstOverride,DedApplied,Status FROM claimproc WHERE ProcNum="+POut.Long(cp.ProcNum);
		//  DataTable table=Db.GetTable(command);
		//  double retVal=0;
		//  long subNum;
		//  int ordinal;
		//  double dedEst;
		//  double dedEstOverride;
		//  double dedApplied;
		//  ClaimProcStatus status;
		//  for(int i=0;i<table.Rows.Count;i++) {
		//    subNum=PIn.Long(table.Rows[i]["InsSubNum"].ToString());
		//    ordinal=PatPlans.GetOrdinal(subNum,patPlanList);
		//    if(ordinal >= thisOrdinal) {
		//      continue;
		//    }
		//    dedEst=PIn.Double(table.Rows[i]["DedEst"].ToString());
		//    dedEstOverride=PIn.Double(table.Rows[i]["DedEstOverride"].ToString());
		//    dedApplied=PIn.Double(table.Rows[i]["DedApplied"].ToString());
		//    status=(ClaimProcStatus)PIn.Int(table.Rows[i]["Status"].ToString());
		//    if(status==ClaimProcStatus.Received || status==ClaimProcStatus.Supplemental) {
		//      retVal+=dedApplied;
		//    }
		//    if(status==ClaimProcStatus.Estimate || status==ClaimProcStatus.NotReceived) {
		//      if(dedEstOverride != -1) {
		//        retVal+=dedEst;
		//      }
		//      else if(dedEst !=-1){
		//        retVal+=dedEst;
		//      }
		//    }
		//  }
		//  return retVal;
		//}

		///<summary>Simply gets insEstTotal or its override if applicable.</summary>
		public static double GetInsEstTotal(ClaimProc cp) {
			//No need to check RemotingRole; no call to db.
			if(cp.InsEstTotalOverride!=-1) {
				return cp.InsEstTotalOverride;
			}
			return cp.InsEstTotal;
		}

		///<summary>Simply gets dedEst or its override if applicable.  Can return 0, but never -1.</summary>
		public static double GetDedEst(ClaimProc cp) {
			//No need to check RemotingRole; no call to db.
			if(cp.DedEstOverride!=-1) {
				return cp.DedEstOverride;
			}
			else if(cp.DedEst!=-1) {
				return cp.DedEst;
			}
			return 0;
		}

		///<summary>Gets either the override or the calculated writeoff estimate.  Or zero if neither.</summary>
		public static double GetWriteOffEstimate(ClaimProc cp) {
			//No need to check RemotingRole; no call to db.
			if(cp.WriteOffEstOverride!=-1) {
				return cp.WriteOffEstOverride;
			}
			else if(cp.WriteOffEst!=-1) {
				return cp.WriteOffEst;
			}
			return 0;
		}

		public static string GetPercentageDisplay(ClaimProc cp) {
			//No need to check RemotingRole; no call to db.
			if(cp.Status==ClaimProcStatus.CapEstimate || cp.Status==ClaimProcStatus.CapComplete) {
				return "";
			}
			if(cp.PercentOverride!=-1) {
				return cp.PercentOverride.ToString();
			}
			else if(cp.Percentage!=-1) {
				return cp.Percentage.ToString();
			}
			return "";
		}

		public static string GetCopayDisplay(ClaimProc cp) {
			//No need to check RemotingRole; no call to db.
			if(cp.CopayOverride!=-1) {
				return cp.CopayOverride.ToString("f");
			}
			else if(cp.CopayAmt!=-1) {
				return cp.CopayAmt.ToString("f");
			}
			return "";
		}

		///<summary></summary>
		public static string GetWriteOffEstimateDisplay(ClaimProc cp) {
			//No need to check RemotingRole; no call to db.
			if(cp.WriteOffEstOverride!=-1) {
				return cp.WriteOffEstOverride.ToString("f");
			}
			else if(cp.WriteOffEst!=-1) {
				return cp.WriteOffEst.ToString("f");
			}
			return "";
		}

		public static string GetEstimateDisplay(ClaimProc cp) {
			//No need to check RemotingRole; no call to db.
			if(cp.Status==ClaimProcStatus.CapEstimate || cp.Status==ClaimProcStatus.CapComplete) {
				return "";
			}
			if(cp.Status==ClaimProcStatus.Estimate) {
				if(cp.InsEstTotalOverride!=-1) {
					return cp.InsEstTotalOverride.ToString("f");
				}
				else{//shows even if 0.
					return cp.InsEstTotal.ToString("f");
				}
			}
			return cp.InsPayEst.ToString("f");
		}

		///<summary>Returns 0 or -1 if no deduct.</summary>
		public static double GetDeductibleDisplay(ClaimProc cp) {
			//No need to check RemotingRole; no call to db.
			if(cp.Status==ClaimProcStatus.CapEstimate || cp.Status==ClaimProcStatus.CapComplete) {
				return -1;
			}
			if(cp.Status==ClaimProcStatus.Estimate) {
				if(cp.DedEstOverride != -1) {
					return cp.DedEstOverride;
				}
				//else if(cp.DedEst > 0) {
					return cp.DedEst;//could be -1
				//}
				//else {
				//	return "";
				//}
			}
			return cp.DedApplied;
		}

		///<summary>Used in TP module.  Gets all estimate notes for this proc.</summary>
		public static string GetEstimateNotes(long procNum,List<ClaimProc> cpList) {
			string retVal="";
			for(int i=0;i<cpList.Count;i++) {
				if(cpList[i].ProcNum!=procNum) {
					continue;
				}
				if(cpList[i].EstimateNote==""){
					continue;
				}
				if(retVal!="") {
					retVal+=", ";
				}
				retVal+=cpList[i].EstimateNote;
			}
			return retVal;
		}

		public static double GetTotalWriteOffEstimateDisplay(List<ClaimProc> cpList,long procNum) {
			//No need to check RemotingRole; no call to db.
			double retVal=0;
			for(int i=0;i<cpList.Count;i++) {
				if(cpList[i].ProcNum!=procNum) {
					continue;
				}
				if(cpList[i].WriteOffEstOverride!=-1) {
					retVal+=cpList[i].WriteOffEstOverride;
				}
				else if(cpList[i].WriteOffEst!=-1) {
					retVal+=cpList[i].WriteOffEst;
				}
			}
			return retVal;
		}

		public static List<ClaimProcHist> GetHistList(long patNum,List<Benefit> benList,List<PatPlan> patPlanList,List<InsPlan> planList,DateTime procDate,List<InsSub> subList) {
			//No need to check RemotingRole; no call to db.
			return GetHistList(patNum,benList,patPlanList,planList,-1,procDate,subList);
		}

		///<summary>We pass in the benefit list so that we know whether to include family.  We are getting a simplified list of claimprocs.
		///History of payments and pending payments.  If the patient has multiple insurance, then this info will be for all of their insurance plans.
		///It runs a separate query for each plan because that's the only way to handle family history.
		///For some plans, the benefits will indicate entire family, but not for other plans.  And the date ranges can be different as well.
		///When this list is processed later, it is again filtered, but it can't have missing information.  Use excludeClaimNum=-1 to not exclude a claim.
		///A claim is excluded if editing from inside that claim.</summary>
		public static List<ClaimProcHist> GetHistList(long patNum,List<Benefit> benList,List<PatPlan> patPlanList,List<InsPlan> planList
			,long excludeClaimNum,DateTime procDate,List<InsSub> subList) 
		{
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimProcHist>>(MethodBase.GetCurrentMethod(),patNum,benList,patPlanList,planList,excludeClaimNum,procDate,subList);
			}
			List<ClaimProcHist> retVal=new List<ClaimProcHist>();
			InsSub sub;
			InsPlan plan;
			bool isFam;
			bool isLife;
			DateTime dateStart;
			DataTable table;
			ClaimProcHist cph;
			List<Benefit> listLimitations=new List<Benefit>();
			for(int p=0;p<patPlanList.Count;p++) {//loop through each plan that this patient is covered by
				sub=InsSubs.GetSub(patPlanList[p].InsSubNum,subList);
				//get the plan for the given patPlan
				plan=InsPlans.GetPlan(sub.PlanNum,planList);
				if(plan==null) {
					continue;
				}
				//test benefits for fam and life
				isFam=false;
				isLife=false;
				for(int i=0;i<benList.Count;i++) {
					if(benList[i].PlanNum==0 && benList[i].PatPlanNum!=patPlanList[p].PatPlanNum) {
						continue;
					}
					if(benList[i].PatPlanNum==0 && benList[i].PlanNum!=plan.PlanNum) {
						continue;
					}
					else if(benList[i].TimePeriod==BenefitTimePeriod.Lifetime) {
						isLife=true;
					}
					if(benList[i].CoverageLevel==BenefitCoverageLevel.Family) {
						isFam=true;
					}
					if(benList[i].BenefitType==InsBenefitType.Limitations //BW, Pano/FW, Exam, and Custom category frequency limitations
						&& benList[i].MonetaryAmt==-1
						&& benList[i].PatPlanNum==0
						&& benList[i].Percent==-1
						&& (benList[i].QuantityQualifier==BenefitQuantity.Months
						|| benList[i].QuantityQualifier==BenefitQuantity.Years
						|| benList[i].QuantityQualifier==BenefitQuantity.NumberOfServices)) 
					{
						listLimitations.Add(benList[i]);
					}
				}
				if(isLife) {
					dateStart=new DateTime(1880,1,1);
				}
				else {
					//unsure what date to use to start.  DateTime.Today?  That might miss procs from late last year when doing secondary claim, InsPaidOther.
					//If we use the proc date, then it will indeed get an accurate history.  And future procedures just don't matter when calculating things.
					dateStart=BenefitLogic.ComputeRenewDate(procDate,plan.MonthRenew);
				}
				DateTime dateRenew=dateStart;
				if(listLimitations.Count>0 && procDate!=DateTime.MinValue) {
					//If there are limitation benefits, calculate the dateStart based on the limitation quantity and quantityqualifier.
					//If the limitation dictates the dateStart is prior to the renew date, use that instead.
					foreach(Benefit ben in listLimitations) {
						DateTime date=procDate;
						if(ben.QuantityQualifier==BenefitQuantity.Months) {
							date=date.AddMonths(0-ben.Quantity);
						}
						else if(ben.QuantityQualifier==BenefitQuantity.Years) {
							date=date.AddYears(0-ben.Quantity);
						}
						else if(ben.QuantityQualifier==BenefitQuantity.NumberOfServices) {
							date=date.AddYears(-1);
						}
						if(date<dateStart) {
							dateStart=date;
						}
					}
				}
				string command="SELECT claimproc.ProcDate,CodeNum,InsPayEst,InsPayAmt,DedApplied,claimproc.PatNum,Status,ClaimNum,claimproc.InsSubNum,claimproc.ProcNum, "
					+"procedurelog.Surf, procedurelog.ToothRange, procedurelog.ToothNum "
					+"FROM claimproc "
					+"LEFT JOIN procedurelog on claimproc.ProcNum=procedurelog.ProcNum "//to get the codenum
					+"WHERE claimproc.InsSubNum="+POut.Long(patPlanList[p].InsSubNum)
					+" AND claimproc.ProcDate >= "+POut.Date(dateStart)//no upper limit on date.
					+" AND claimproc.Status IN("
						+POut.Long((int)ClaimProcStatus.NotReceived)+","
						+POut.Long((int)ClaimProcStatus.Adjustment)+","//insPayAmt and DedApplied
						+POut.Long((int)ClaimProcStatus.Received)+","
						+POut.Long((int)ClaimProcStatus.Supplemental)+")";
				if(!isFam) {
					//we include patnum because this one query can get results for multiple patients that all have this one subsriber.
					command+=" AND claimproc.PatNum="+POut.Long(patNum);
				}
				if(excludeClaimNum != -1) {
					command+=" AND claimproc.ClaimNum != "+POut.Long(excludeClaimNum);
				}
				table=Db.GetTable(command);
				for(int i=0;i<table.Rows.Count;i++) {
					DateTime claimProcDate=PIn.Date(table.Rows[i]["ProcDate"].ToString());
					if(claimProcDate < dateRenew && !listLimitations.Exists(x => x.CodeNum==PIn.Long(table.Rows[i]["CodeNum"].ToString()))) {
						continue;//If it's a claimproc that's older than the renew date, which means it may be for a frequency, but it doesn't have a frequency codenum, don't make a hist for it.
					}
					cph=new ClaimProcHist();
					cph.ProcDate   = claimProcDate;
					cph.StrProcCode= ProcedureCodes.GetStringProcCode(PIn.Long(table.Rows[i]["CodeNum"].ToString()));
					cph.Status     = (ClaimProcStatus)PIn.Long(table.Rows[i]["Status"].ToString());
					if(cph.Status==ClaimProcStatus.NotReceived) {
						cph.Amount   = PIn.Double(table.Rows[i]["InsPayEst"].ToString());
					}
					else {
						cph.Amount   = PIn.Double(table.Rows[i]["InsPayAmt"].ToString());
					}
					cph.Deduct     = PIn.Double(table.Rows[i]["DedApplied"].ToString());
					cph.PatNum     = PIn.Long(table.Rows[i]["PatNum"].ToString());
					cph.ClaimNum   = PIn.Long(table.Rows[i]["ClaimNum"].ToString());
					cph.InsSubNum  = PIn.Long(table.Rows[i]["InsSubNum"].ToString());
					cph.ProcNum    = PIn.Long(table.Rows[i]["ProcNum"].ToString());
					cph.PlanNum		 = plan.PlanNum;
					cph.Surf       = PIn.String(table.Rows[i]["Surf"].ToString());
					cph.ToothRange = PIn.String(table.Rows[i]["ToothRange"].ToString());
					cph.ToothNum   = PIn.String(table.Rows[i]["ToothNum"].ToString());
					retVal.Add(cph);
				}
			}
			return retVal;
		}

		/// <summary>Used in creation of the loopList.  Used in TP list estimation and in claim creation.  Some of the items in the claimProcList passed in will not have been saved to the database yet.</summary>
		public static List<ClaimProcHist> GetHistForProc(List<ClaimProc> claimProcList,long procNum,long codeNum) {
			List<ClaimProcHist> retVal=new List<ClaimProcHist>();
			ClaimProcHist cph;
			for(int i=0;i<claimProcList.Count;i++) {
				if(claimProcList[i].ProcNum != procNum) {
					continue;
				}
				cph=new ClaimProcHist();
				cph.Amount=ClaimProcs.GetInsEstTotal(claimProcList[i]);
				cph.ClaimNum=0;
				if(claimProcList[i].DedEstOverride != -1) {
					cph.Deduct=claimProcList[i].DedEstOverride;
				}
				else {
					cph.Deduct=claimProcList[i].DedEst;
				}
				cph.PatNum=claimProcList[i].PatNum;
				cph.PlanNum=claimProcList[i].PlanNum;
				cph.InsSubNum=claimProcList[i].InsSubNum;
				cph.ProcDate=DateTime.Today;
				cph.Status=ClaimProcStatus.Estimate;
				cph.StrProcCode=ProcedureCodes.GetStringProcCode(codeNum);
				retVal.Add(cph);
			}
			return retVal;
		}

		///<summary>Does not make call to db unless necessary.</summary>
		public static void SetProvForProc(Procedure proc,List<ClaimProc> ClaimProcList) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),proc,ClaimProcList);
				return;
			}
			for(int i=0;i<ClaimProcList.Count;i++) {
				if(ClaimProcList[i].ProcNum!=proc.ProcNum) {
					continue;
				}
				if(ClaimProcList[i].ProvNum==proc.ProvNum) {
					continue;//no change needed
				}
				ClaimProcList[i].ProvNum=proc.ProvNum;
				Update(ClaimProcList[i]);
			}
		}

		///<summary>For moving rows up and down the batch insurance window.</summary>
		public static void SetPaymentRow(long claimNum,long claimPaymentNum,int paymentRow) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),claimNum,claimPaymentNum,paymentRow);
				return;
			}
			string command="UPDATE claimproc SET PaymentRow="+POut.Int(paymentRow)+" "
				+"WHERE ClaimNum="+POut.Long(claimNum)+" "
				+"AND ClaimPaymentNum="+POut.Long(claimPaymentNum);
			Db.NonQ(command);
		}

		///<summary>For moving rows up and down the batch insurance window. For each ClaimNum in listClaimNums, the value at the corresponding index in
		///listPaymentRows will be set as the PaymentRow. Be sure that listClaimNums and listPaymentRows have the same number of items.</summary>
		public static void SetPaymentRow(List<long> listClaimNums,long claimPaymentNum,List<int> listPaymentRows) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listClaimNums,claimPaymentNum,listPaymentRows);
				return;
			}
			if(listClaimNums==null || listClaimNums.Count==0 || listPaymentRows==null || listClaimNums.Count!=listPaymentRows.Count) {
				return;
			}
			string command="UPDATE claimproc SET PaymentRow=(CASE ClaimNum ";
			for(int i=0;i<listClaimNums.Count;i++) {//Expected up to 10; To actual limit, one for each claim in FormClaimPayBatch.GridMain (100s or 1000s)
				command+="WHEN "+POut.Long(listClaimNums[i])+" THEN "+POut.Int(listPaymentRows[i])+" ";
			}
			command+="END) WHERE ClaimNum IN ("+string.Join(",",listClaimNums.Select(x => POut.Long(x)))+") "
				+"AND ClaimPaymentNum="+POut.Long(claimPaymentNum);
			Db.NonQ(command);
		}

		///<summary>Attaches all claimprocs that have an InsPayAmt entered to the specified ClaimPayment, 
		///and then returns the sum amount of all the attached payments.  The claimprocs must be currently unattached.
		///Used from Edit Claim window (FormClaimEdit) when user is not doing the batch entry.
		///To finalize a single claim, set onlyOneClaimNum to the claimNum to finalize.</summary>
		public static double AttachAllOutstandingToPayment(long claimPaymentNum,DateTime dateClaimPayZero,long onlyOneClaimNum=0) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<double>(MethodBase.GetCurrentMethod(),claimPaymentNum,dateClaimPayZero,onlyOneClaimNum);
			}
			//See job #7423.
			//The claimproc.DateCP is essentially the same as the claim.DateReceived.
			//We used to use the claimproc.ProcDate, which is essentially the same as the claim.DateService.
			//Since the service date could be weeks or months in the past, it makes more sense to use the received date, which will be more recent.
			//Additionally, users found using the date of service to be unintuitive.
			//STRONG CAUTION not to use the claimproc.ProcDate here in the future.
			string command="UPDATE claimproc SET ClaimPaymentNum="+POut.Long(claimPaymentNum)+" "
				+"WHERE ClaimPaymentNum=0 "
				+"AND (claimproc.Status = '1' OR claimproc.Status = '4' OR claimproc.Status='5') "//received or supplemental or capclaim
				+"AND (InsPayAmt != 0 ";
			//See job #7517.
			//Always exclude NO PAYMENT claims and $0 claimprocs for batch payments created from the Edit Claim window.
			//Include $0 claimprocs on or after rolling date (if enabled) when finalizing an individual claim.
			if(onlyOneClaimNum!=0 && dateClaimPayZero.Year > 1880) {
				command+="OR DateCP >= "+POut.Date(dateClaimPayZero);
			}
			command+=") ";
			if(onlyOneClaimNum!=0) {//Finalizing individual claim.
				command+="AND ClaimNum="+POut.Long(onlyOneClaimNum);
			}
			Db.NonQ(command);
			command="SELECT SUM(InsPayAmt) FROM claimproc WHERE ClaimPaymentNum="+POut.Long(claimPaymentNum);
			return PIn.Double(Db.GetScalar(command));
		}

		///<summary>Called when we want to try and update claimProc.DateInsFinalized for claimProcs associated to the given claimPaymentNum.
		///The filters inside this function mimic AttachAllOutstandingToPayment() above.
		///If finalizing a single claim, set onlyOneClaimNum to the claimNum to finalize.</summary>
		public static void DateInsFinalizedHelper(long claimPaymentNum,DateTime dateClaimPayZero,long onlyOneClaimNum=0) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),claimPaymentNum,dateClaimPayZero,onlyOneClaimNum);
				return;
			}
			//See job #7423.
			//The claimproc.DateCP is essentially the same as the claim.DateReceived.
			//We used to use the claimproc.ProcDate, which is essentially the same as the claim.DateService.
			//Since the service date could be weeks or months in the past, it makes more sense to use the received date, which will be more recent.
			//Additionally, users found using the date of service to be unintuitive.
			//STRONG CAUTION not to use the claimproc.ProcDate here in the future.
			string command="UPDATE claimproc SET "
				+"DateInsFinalized = (CASE DateInsFinalized WHEN '0001-01-01' THEN "+DbHelper.Now()+" ELSE DateInsFinalized END) "
				+"WHERE ClaimPaymentNum="+POut.Long(claimPaymentNum)+" "
				+"AND (claimproc.Status = '1' OR claimproc.Status = '4' OR claimproc.Status='5') "//received or supplemental or capclaim
				+"AND (InsPayAmt != 0 ";
			//See job #7517.
			//Always exclude NO PAYMENT claims and $0 claimprocs for batch payments created from the Edit Claim window.
			//Include $0 claimprocs on or after rolling date (if enabled) when finalizing an individual claim.
			if(onlyOneClaimNum!=0 && dateClaimPayZero.Year > 1880) {
				command+="OR DateCP >= "+POut.Date(dateClaimPayZero);
			}
			command+=") ";
			if(onlyOneClaimNum!=0) {//Finalizing individual claim.
				command+="AND ClaimNum="+POut.Long(onlyOneClaimNum);
			}
			Db.NonQ(command);
			return;
		}

		///<summary>Pass in a cached or potentially "stale" list of claim procs and this method will check the ClaimNum against the num stored in the database to make sure they still match.  Returns true if any of the claim procs are not pointing to the same claim.</summary>
		public static bool IsAttachedToDifferentClaim(long procNum,List<ClaimProc> listClaimProcsFromCache) {
			//No need to check RemotingRole; no call to db.
			List<ClaimProc> listClaimProcsFromDB=RefreshForProc(procNum);
			for(int i=0;i<listClaimProcsFromCache.Count;i++) {
				for(int j=0;j<listClaimProcsFromDB.Count;j++) {
					if(listClaimProcsFromCache[i].ClaimProcNum!=listClaimProcsFromDB[j].ClaimProcNum) {
						continue;
					}
					if(listClaimProcsFromCache[i].ClaimNum!=listClaimProcsFromDB[j].ClaimNum) {
						return true;
					}
				}
			}
			return false;
		}

		///<summary>Gets the ProcNum, Status, and WriteOff for the passed in procedures.</summary>
		public static List<ClaimProc> GetForProcsLimited(List<long> listProcNums,params ClaimProcStatus[] listStatuses) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimProc>>(MethodBase.GetCurrentMethod(),listProcNums,listStatuses);
			}
			List<ClaimProc> listClaimProcs = new List<ClaimProc>();
			if(listProcNums==null || listProcNums.Count < 1 || listStatuses.Count()==0) {
				return listClaimProcs;
			}
			string command = "SELECT ProcNum,Status,WriteOff FROM claimproc WHERE ProcNum IN("+string.Join(",",listProcNums)+") "
				+"AND Status IN("+string.Join(",",listStatuses.Select(x => (int)x))+")";
			DataTable table = Db.GetTable(command);
			foreach(DataRow row in table.Rows) {
				ClaimProc claimProc = new ClaimProc();
				claimProc.ProcNum=PIn.Long(row["ProcNum"].ToString());
				claimProc.Status=(ClaimProcStatus)PIn.Int(row["Status"].ToString());
				claimProc.WriteOff=PIn.Double(row["WriteOff"].ToString());
				listClaimProcs.Add(claimProc);
			}
			return listClaimProcs;
		}

		public static void DeleteMany(List<ClaimProc> listClaimProcs) {
			if(listClaimProcs.Count==0) {
				return;
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listClaimProcs);
				return;
			}
			string command="DELETE FROM claimproc WHERE ClaimProcNum IN("+string.Join(",",listClaimProcs.Select(x => x.ClaimProcNum))+")";
			Db.NonQ(command);
		}
	}

	///<summary>During the ClaimProc.ComputeBaseEst() and related sections, this holds historical payment information for one procedure or an adjustment to insurance benefits from patplan.</summary>
	public class ClaimProcHist {
		public DateTime ProcDate;
		public string StrProcCode;
		///<summary>Insurance paid or est, depending on the status.</summary>
		public double Amount;
		///<summary>Deductible paid or est.</summary>
		public double Deduct;
		///<summary>Because a list can store info for an entire family.</summary>
		public long PatNum;
		///<summary>Because a list can store info about multiple plans.</summary>
		public long PlanNum;
		///<summary>So that we can exclude history from the claim that we are in.</summary>
		public long ClaimNum;
		///<summary>Only 4 statuses get used anyway.  This helps us filter the pending items sometimes.</summary>
		public ClaimProcStatus Status;
		///<summary></summary>
		public long InsSubNum;
		///<summary>This is needed to filter out primary histList entries on secondary claims.</summary>
		public long ProcNum;
		///<summary>This value is formatted exactly the same as it is within the database for procedurelog.Surf</summary>
		public string Surf;
		///<summary>This value is formatted exactly the same as it is within the database for procedurelog.ToothRange</summary>
		public string ToothRange;
		///<summary>This value is formatted exactly the same as it is within the database for procedurelog.ToothNum</summary>
		public string ToothNum;

		public override string ToString() {
			return StrProcCode+" "+Status.ToString()+" "+Amount.ToString()+" ded:"+Deduct.ToString();
		}
	}


}









