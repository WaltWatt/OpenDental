using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class TreatPlans {
		#region Get Methods
		#endregion

		#region Modification Methods
		
		#region Insert
		#endregion

		#region Update
		///<summmary>Updates all active and inactive TP's to match the patients current treatment plan type.</summmary>
		public static void UpdateTreatmentPlanType(Patient pat) {
			//No need to check RemotingRole; no call to db.
			List<TreatPlan> listTps=TreatPlans.GetAllForPat(pat.PatNum);
			listTps.RemoveAll(x => x.TPStatus==TreatPlanStatus.Saved);	//keep active and inactive tp's, not saved ones.
			TreatPlanType newType=pat.DiscountPlanNum==0 ? TreatPlanType.Insurance : TreatPlanType.Discount;
			foreach(TreatPlan tp in listTps) {
				if(tp.TPType!=newType) {
					tp.TPType=newType;
					TreatPlans.Update(tp);
				}
			}		
		}
		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods
		#endregion


		///<summary>Gets all Saved TreatPlans for a given Patient, ordered by date.</summary>
		public static List<TreatPlan> Refresh(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<TreatPlan>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM treatplan "
			  +"WHERE PatNum="+POut.Long(patNum)+" "
				+"AND TPStatus=0 "//Saved
				+"ORDER BY DateTP";
			return Crud.TreatPlanCrud.SelectMany(command);
		}

		public static List<TreatPlan> GetAllForPat(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<TreatPlan>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM treatplan "
				+"WHERE PatNum="+POut.Long(patNum)+" ";
			return Crud.TreatPlanCrud.SelectMany(command);
		}

		public static List<TreatPlan> GetAllCurrentForPat(long patNum) {
			return GetAllForPat(patNum).Where(x => x.TPStatus!=TreatPlanStatus.Saved)
				.OrderBy(x => x.TPStatus!=TreatPlanStatus.Active)
				.ThenBy(x => x.DateTP).ToList();
		}

		///<summary>A single treatplan from the DB.</summary>
		public static TreatPlan GetOne(long treatPlanNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<TreatPlan>(MethodBase.GetCurrentMethod(),treatPlanNum);
			}
			return Crud.TreatPlanCrud.SelectOne(treatPlanNum);
		}

		///<summary>Gets the first Active TP from the DB for the patient.  Returns null if no Active TP is found for this patient.</summary>
		public static TreatPlan GetActiveForPat(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<TreatPlan>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM treatplan WHERE PatNum="+POut.Long(patNum)+" AND TPStatus="+POut.Int((int)TreatPlanStatus.Active);
			return Crud.TreatPlanCrud.SelectOne(command);
		}

		///<summary></summary>
		public static void Update(TreatPlan tp){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),tp);
				return;
			}
			Crud.TreatPlanCrud.Update(tp);
		}

		///<summary></summary>
		public static long Insert(TreatPlan tp) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				tp.SecUserNumEntry=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				tp.TreatPlanNum=Meth.GetLong(MethodBase.GetCurrentMethod(),tp);
				return tp.TreatPlanNum;
			}
			return Crud.TreatPlanCrud.Insert(tp);
		}

		///<summary>Dependencies checked first and throws an exception if any found. So surround by try catch</summary>
		public static void Delete(TreatPlan tp){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),tp);
				return;
			}
			//check proctp for dependencies
			string command="SELECT * FROM proctp WHERE TreatPlanNum ="+POut.Long(tp.TreatPlanNum);
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count>0) {
				//this should never happen
				throw new InvalidProgramException(Lans.g("TreatPlans","Cannot delete treatment plan because it has ProcTP's attached"));
			}
			command= "DELETE from treatplan WHERE TreatPlanNum = '"+POut.Long(tp.TreatPlanNum)+"'";
 			Db.NonQ(command);
		}

		///<summary>Gets the hashstring for generating signatures.
		///Should only be used when saving signatures, for validating see GetKeyDataForSignatureHash() and GetHashStringForSignature()</summary>
		public static string GetKeyDataForSignatureSaving(TreatPlan tp,List<ProcTP> proclist) {
			string keyData = GetKeyDataForSignatureHash(tp,proclist);
			return GetHashStringForSignature(keyData);
		}

		///<summary>Gets the key data string needed to create a hashstring to be used later when filling the signature.
		///This is done seperate of the hashing so that new line replacements can be done when validating signatures before hashing.</summary>
		public static string GetKeyDataForSignatureHash(TreatPlan tp,List<ProcTP> proclist) {
			//No need to check RemotingRole; no call to db.
			//the key data is a concatenation of the following:
			//tp: Note, DateTP
			//each proctp: Descript,PatAmt
			//The procedures MUST be in the correct order, and we'll use ItemOrder to order them.
			StringBuilder strb = new StringBuilder();
			strb.Append(tp.Note);
			strb.Append(tp.DateTP.ToString("yyyyMMdd"));
			for(int i = 0;i<proclist.Count;i++) {
				strb.Append(proclist[i].Descript);
				strb.Append(proclist[i].PatAmt.ToString("F2"));
			}
			return strb.ToString();
		}

		///<summary>Gets the hashstring from the provided string that is typically generated from GetStringForSignatureHash().
		///This is done seperate of building the string so that new line replacements can be done when validating signatures before hashing.</summary>
		public static string GetHashStringForSignature(string str) {
			//No need to check RemotingRole; no call to db.
			return Encoding.ASCII.GetString(ODCrypt.MD5.Hash(Encoding.UTF8.GetBytes(str)));
		}

		///<summary>No need to pass in userNum, it's set before remoting role check and passed to the server if necessary.
		///<para>This is the automation behind keeping treatplans correct.  Many calls to DB, consider optimizing or calling sparingly.</para>
		///<para>Ensures patients only have one active treatplan, marks extras inactive and creates an active if necessary.</para>
		///<para>Attaches procedures to the active plan if the proc status is TP or status is TPi and the procs is attached to a sched/planned appt.</para>
		///<para>Creates an unassigned treatplan if necessary and attaches any unassigned procedures to it.</para>
		///<para>Also maintains priorities of treatplanattaches and procedures and updates the procstatus of TP and TPi procs if necessary.</para></summary>
		public static void AuditPlans(long patNum,TreatPlanType tpType,long userNum=0) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				userNum=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patNum,tpType,userNum);
				return;
			}
			#region Pseudo Code
			//Get all treatplans for the patient
			//Find active TP if it already exists
			//If more than one active TP, update all but the first to Inactive
			//Find unassigned TP if it already exists
			//Get all treatplanattaches for the treatplans
			//Get all TP and TPi procs for the patient
			//Get list of procs for the active plan, i.e. TPA exists linking to active plan or ProcStatus is TP or attached to sched/planned appt
			//Get list of inactive procs, i.e. ProcStatus is TPi and AptNum is 0 and PlannedAptNum is 0 and no TPA exists linking it to the active plan
			//Create an active plan if one doesn't exist and there are procs that need to be attached to it
			//Create an unassigned plan if one doesn't exist and there are unassigned TPi procs
			//For each proc that should be attached to the active plan
			//  update status from TPi to TP
			//  if TPA exists linking to active plan, update priority to TPA priority
			//  delete any TPA that links the proc to the unassigned plan
			//  if TPA linking the proc to the active plan does not exist, insert one with TPA priority set to the proc priority
			//For each proc that is not attached to the active plan (ProcStatus is TPi)
			//  set proc priority to 0
			//  if TPA does not exist, insert one linking the proc to the unassigned plan with TPA priority 0
			//  if multiple TPAs exist with one linking the proc to the unassigned plan, delete the TPA linking to the unassigned plan
			//Foreach TPA
			//  if TPA links proc to the unassigned plan and TPA exists linking the proc to any other plan, delete the link to the unassigned plan
			//If an unassigned plan exists and there are no TPAs pointing to it, delete the unassigned plan
			#endregion Pseudo Code
			#region Variables
			List<TreatPlan> listTreatPlans=TreatPlans.GetAllForPat(patNum);//All treatplans for the pat. [([Includes Saved Plans]}};
			TreatPlan activePlan=listTreatPlans.FirstOrDefault(x => x.TPStatus==TreatPlanStatus.Active);//can be null
			TreatPlan unassignedPlan=listTreatPlans.FirstOrDefault(x => x.TPStatus==TreatPlanStatus.Inactive && x.Heading==Lans.g("TreatPlans","Unassigned"));//can be null
			List<TreatPlanAttach> listTPAs=TreatPlanAttaches.GetAllForTPs(listTreatPlans.Select(x => x.TreatPlanNum).ToList());
			List<Procedure> listProcsTpTpi=Procedures.GetProcsByStatusForPat(patNum,new[] { ProcStat.TP,ProcStat.TPi });//All TP and TPi procs for the pat.
			List<Procedure> listProcsForActive=new List<Procedure>();//All procs that should be linked to the active plan (can be linked to inactive plans as well)
			List<Procedure> listProcsForInactive=new List<Procedure>();//All procs that should not be linked to the active plan (linked to inactive or unnasigned)
			long[] arrayTpaProcNums=listTPAs.Select(x => x.ProcNum).ToArray();//All procnums from listTPAs, makes it easier to see if a TPA exists for a proc
			#endregion Variables
			#region Fill Proc Lists and Create Active and Unassigned Plans
			foreach(Procedure procCur in listProcsTpTpi) {//puts each procedure in listProcsForActive or listProcsForInactive
				if(procCur.ProcStatus==ProcStat.TP	//all TP procs should be linked to active plan
					|| procCur.AptNum>0								//all procs attached to an appt should be linked to active plan
					|| procCur.PlannedAptNum>0				//all procs attached to a planned appt should be linked to active plan
					|| (activePlan!=null							//if active plan exists and proc is linked to it, add to list
							&& listTPAs.Any(x => x.ProcNum==procCur.ProcNum && x.TreatPlanNum==activePlan.TreatPlanNum)))
				{
					listProcsForActive.Add(procCur);
				}
				else {//TPi status, AptNum=0, PlannedAptNum=0, and not attached to active plan
					listProcsForInactive.Add(procCur);
				}
			}
			//Create active plan if needed
			if(activePlan==null && listProcsForActive.Count>0) {
				activePlan=new TreatPlan() {
					Heading=Lans.g("TreatPlans","Active Treatment Plan"),
					Note=PrefC.GetString(PrefName.TreatmentPlanNote),
					TPStatus=TreatPlanStatus.Active,
					PatNum=patNum,
					//UserNumPresenter=userNum,
					SecUserNumEntry=userNum,
					TPType=tpType
				};
				TreatPlans.Insert(activePlan);
				listTreatPlans.Add(activePlan);
			}
			//Update extra active plans to Inactive status, should only ever be one Active status plan
			//All TP procs are linked to the active plan, so proc statuses won't have to change to TPi for procs attached to an "extra" active plan
			foreach(TreatPlan tp in listTreatPlans.FindAll(x => x.TPStatus==TreatPlanStatus.Active && activePlan!=null && x.TreatPlanNum!=activePlan.TreatPlanNum)) {
				tp.TPStatus=TreatPlanStatus.Inactive;
				TreatPlans.Update(tp);
			}
			//Create unassigned plan if needed
			if(unassignedPlan==null && listProcsForInactive.Any(x => !arrayTpaProcNums.Contains(x.ProcNum))) {
				unassignedPlan=new TreatPlan() {
					Heading=Lans.g("TreatPlans","Unassigned"),
					Note=PrefC.GetString(PrefName.TreatmentPlanNote),
					TPStatus=TreatPlanStatus.Inactive,
					PatNum=patNum,
					//UserNumPresenter=userNum,
					SecUserNumEntry=userNum,
					TPType=tpType
				};
				TreatPlans.Insert(unassignedPlan);
				listTreatPlans.Add(unassignedPlan);
			}
			#endregion Fill Proc Lists and Create Active and Unassigned Plans
			#region Procs for Active Plan
			//Update proc status to TP for all procs that should be linked to the active plan.
			//For procs with an existing TPA linking it to the active plan, update proc priority to the TPA priority.
			//Remove any TPAs linking the proc to the unassigned plan.
			//Create TPAs linking the proc to the active plan if needed with TPA priority set to the proc priority.
			foreach(Procedure procActive in listProcsForActive) {
				Procedure procOld=procActive.Copy();
				procActive.ProcStatus=ProcStat.TP;
				//checking the array of ProcNums for an existing TPA is fast, so check the list first
				if(arrayTpaProcNums.Contains(procActive.ProcNum)) {
					if(unassignedPlan!=null) {//remove any TPAs linking the proc to the unassigned plan
						listTPAs.RemoveAll(x => x.ProcNum==procActive.ProcNum && x.TreatPlanNum==unassignedPlan.TreatPlanNum);
					}
					TreatPlanAttach tpaActivePlan=listTPAs.FirstOrDefault(x => x.ProcNum==procActive.ProcNum && x.TreatPlanNum==activePlan.TreatPlanNum);
					if(tpaActivePlan==null) {//no TPA linking the proc to the active plan, create one with priority equal to the proc priority
						listTPAs.Add(new TreatPlanAttach() { ProcNum=procActive.ProcNum,TreatPlanNum=activePlan.TreatPlanNum,Priority=procActive.Priority });
					}
					else {//TPA linking this proc to the active plan exists, update proc priority to equal TPA priority
						procActive.Priority=tpaActivePlan.Priority;
					}
				}
				else {//no TPAs exist for this proc, add one linking the proc to the active plan and set the TPA priority equal to the proc priority
					listTPAs.Add(new TreatPlanAttach() { ProcNum=procActive.ProcNum,TreatPlanNum=activePlan.TreatPlanNum,Priority=procActive.Priority });
				}
				Procedures.Update(procActive,procOld);
			}
			#endregion Procs for Active Plan
			#region Procs for Inactive and Unassigned Plans
			//Update proc priority to 0 for all inactive procs.
			//If no TPA exists for the proc, create a TPA with priority 0 linking the proc to the unassigned plan.
			foreach(Procedure procInactive in listProcsForInactive) {
				Procedure procOld=procInactive.Copy();
				procInactive.Priority=0;
				Procedures.Update(procInactive,procOld);
				if(unassignedPlan!=null && !arrayTpaProcNums.Contains(procInactive.ProcNum)) {
					//no TPAs for this proc, add a new one to the list linking proc to the unassigned plan
					listTPAs.Add(new TreatPlanAttach { TreatPlanNum=unassignedPlan.TreatPlanNum,ProcNum=procInactive.ProcNum,Priority=0 });
				}
			}
			#endregion Procs for Inactive and Unassigned Plans
			#region Sync and Clean-Up TreatPlanAttach List
			//Remove any TPAs if the proc isn't in listProcsTpTpi, status could've changed or possibly proc is for a different pat.
			listTPAs.RemoveAll(x => !listProcsTpTpi.Select(y => y.ProcNum).Contains(x.ProcNum));
			if(unassignedPlan!=null) {//if an unassigned plan exists
				//Remove any TPAs from the list that link a proc to the unassigned plan if there is a TPA that links the proc to any other plan
				listTPAs.RemoveAll(x => x.TreatPlanNum==unassignedPlan.TreatPlanNum
					&& listTPAs.Any(y => y.ProcNum==x.ProcNum && y.TreatPlanNum!=unassignedPlan.TreatPlanNum));
			}
			listTreatPlans.ForEach(x => TreatPlanAttaches.Sync(listTPAs.FindAll(y => y.TreatPlanNum==x.TreatPlanNum),x.TreatPlanNum));
			if(unassignedPlan!=null) {//Must happen after Sync. Delete the unassigned plan if it exists and there are no TPAs pointing to it.
				listTPAs=TreatPlanAttaches.GetAllForTreatPlan(unassignedPlan.TreatPlanNum);//from DB.
				if(listTPAs.Count==0) {//nothing attached to unassigned anymore
					Crud.TreatPlanCrud.Delete(unassignedPlan.TreatPlanNum);
				}
			}
			#endregion Sync and Clean-Up TreatPlanAttach List
		}

		public static TreatPlan GetUnassigned(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<TreatPlan>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM treatplan "
				+"WHERE PatNum="+POut.Long(patNum)+" "
				+"AND TPStatus="+POut.Int((int)TreatPlanStatus.Inactive)+" "
				+"AND Heading='"+POut.String(Lans.g("TreatPlans","Unassigned"))+"'";
			return Crud.TreatPlanCrud.SelectOne(command)??new TreatPlan();
		}

		///<summary>Called after setting the status to treatPlanCur to Active.
		///Updates the status of any other plan with Active status to Inactive.
		///If the original heading of the other plan is "Active Treatment Plan" it will be updated to "Inactive Treatment Plan".</summary>
		public static void SetOtherActiveTPsToInactive(TreatPlan treatPlanCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),treatPlanCur);
				return;
			}
			string command="SELECT * FROM treatplan "
				+"WHERE PatNum="+POut.Long(treatPlanCur.PatNum)+" "
				+"AND TPStatus="+POut.Int((int)TreatPlanStatus.Active)+" "
				+"AND TreatPlanNum!="+POut.Long(treatPlanCur.TreatPlanNum);
			//Make Active TP's inactive. Rename if TP's still have default name.
			List<TreatPlan> listActivePlans=Crud.TreatPlanCrud.SelectMany(command);
			foreach(TreatPlan tp in listActivePlans) {//should only ever be one, but just in case there are multiple this will rectify the problem.
				if(tp.Heading==Lans.g("TreatPlans","Active Treatment Plan")) {
					tp.Heading=Lans.g("TreatPlans","Inactive Treatment Plan");
				}
				tp.TPStatus=TreatPlanStatus.Inactive;
				TreatPlans.Update(tp);
			}
			//Heading is changed from within the form, if they have changed it back to Inactive Treatment Plan it was deliberate.
			//if(treatPlanCur.Heading==Lans.g("TreatPlans","Inactive Treatment Plan")) {
			//	treatPlanCur.Heading=Lans.g("TreatPlans","Active Treatment Plan");
			//}
			//Not necessary, treatPlanCur should be set to Active prior to calling this function.
			//treatPlanCur.TPStatus=TreatPlanStatus.Active;
			//TreatPlans.Update(treatPlanCur);
		}

		///<summary>May not return correct values if notes are stored with newline characters.</summary>
		public static List<long> GetNumsByNote(string oldNote) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),oldNote);
			}
			oldNote=oldNote.Replace("\r","");
			//oldNote=oldNote.Replace("\r","").Replace("\n","\r\n");
			//oldNote=oldNote.Replace("\r","").Replace("\n","*?");
			string command="SELECT TreatPlanNum FROM treatplan WHERE REPLACE(Note,'\\r','')='"+POut.String(oldNote)+"' "+
			"AND TPStatus IN ("+POut.Int((int)TreatPlanStatus.Active)+","+POut.Int((int)TreatPlanStatus.Inactive)+")";
			//string command="SELECT TreatPlanNum FROM treatplan WHERE Note='"+POut.String(oldNote)+"' "+
			//	"AND TPStatus IN ("+POut.Int((int)TreatPlanStatus.Active)+","+POut.Int((int)TreatPlanStatus.Inactive)+")";
			return Db.GetListLong(command);
		}

		/// <summary>	Updates the default note on active/inactive treatment plans with new note</summary>
		public static void UpdateNotes(string newNote, List<long> listTPNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),newNote,listTPNums);
				return;
			}
			if(listTPNums==null || listTPNums.Count==0) {
				return;
			}
			string command="UPDATE treatplan SET Note='"+POut.String(newNote)+"' "
				+"WHERE TreatPlanNum IN ("+string.Join(",",listTPNums)+")";
 			Db.NonQ(command);
		}

		public static List<TreatPlan> GetFromProcTPs(List<ProcTP> listProcTPs) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<TreatPlan>>(MethodBase.GetCurrentMethod(),listProcTPs);
			}
			List<TreatPlan> listTreatPlans = new List<TreatPlan>();
			if(listProcTPs.Count == 0) {
				return listTreatPlans;
			}
			string command = "SELECT * FROM treatplan WHERE treatplan.TreatPlanNum IN ("+string.Join(",",listProcTPs.Select(x => x.TreatPlanNum))+")";
			listTreatPlans = Crud.TreatPlanCrud.SelectMany(command);
			foreach(TreatPlan treatPlan in listTreatPlans) {
				treatPlan.ListProcTPs = listProcTPs.Where(x => x.TreatPlanNum == treatPlan.TreatPlanNum).ToList();
			}
			return listTreatPlans;
		}

		///<summary>Returns only 5 columns for all saved treatment plans.</summary>
		public static List<TreatPlan> GetAllSavedLim() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<TreatPlan>>(MethodBase.GetCurrentMethod());
			}
			string command = "SELECT TreatPlanNum, PatNum, DateTP, SecUserNumEntry, UserNumPresenter "
				+" FROM treatplan WHERE treatplan.TPStatus="+POut.Int((int)TreatPlanStatus.Saved);
			DataTable table = Db.GetTable(command);
			List<TreatPlan> listSavedTreatPlanLim = new List<TreatPlan>();
			foreach(DataRow row in table.Rows) {
				TreatPlan treatPlanCur = new TreatPlan();
				treatPlanCur.TreatPlanNum=PIn.Long(row["TreatPlanNum"].ToString());
				treatPlanCur.PatNum=PIn.Long(row["PatNum"].ToString());
				treatPlanCur.DateTP=PIn.Date(row["DateTP"].ToString());
				treatPlanCur.SecUserNumEntry=PIn.Long(row["SecUserNumEntry"].ToString());
				treatPlanCur.UserNumPresenter=PIn.Long(row["UserNumPresenter"].ToString());
				listSavedTreatPlanLim.Add(treatPlanCur);
			}
			return listSavedTreatPlanLim;
		}

	}

	

	


}