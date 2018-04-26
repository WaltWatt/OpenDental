using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using CodeBase;

namespace OpenDentBusiness{
	///<summary>Handles database commands related to the adjustment table in the db.</summary>
	public class Adjustments {
		#region Get Methods
		#endregion

		#region Modification Methods
		
		#region Insert
		#endregion

		#region Update
		#endregion

		#region Delete
		#endregion

		#region Insert

		#endregion
		#region Udate

		#endregion
		#region Delete
		#endregion
		#endregion
		#region Misc Methods
		#endregion

		///<summary>Gets all adjustments for a single patient.</summary>
		public static Adjustment[] Refresh(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Adjustment[]>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command=
				"SELECT * FROM adjustment"
				+" WHERE PatNum = "+POut.Long(patNum)+" ORDER BY AdjDate";
			return Crud.AdjustmentCrud.SelectMany(command).ToArray();
		}

		///<summary>Gets one adjustment from the db.</summary>
		public static Adjustment GetOne(long adjNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Adjustment>(MethodBase.GetCurrentMethod(),adjNum);
			}
			string command=
				"SELECT * FROM adjustment"
				+" WHERE AdjNum = "+POut.Long(adjNum);
			return Crud.AdjustmentCrud.SelectOne(adjNum);
		}
		public static void DetachFromInvoice(long statementNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),statementNum);
				return;
			}
			string command="UPDATE adjustment SET StatementNum=0 WHERE StatementNum="+POut.Long(statementNum)+"";
			Db.NonQ(command);
		}

		public static void DetachAllFromInvoices(List<long> listStatementNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listStatementNums);
				return;
			}
			if(listStatementNums==null || listStatementNums.Count==0) {
				return;
			}
			string command="UPDATE adjustment SET StatementNum=0 WHERE StatementNum IN ("+string.Join(",",listStatementNums.Select(x => POut.Long(x)))+")";
			Db.NonQ(command);
		}

		///<summary>Gets all negative or positive adjustments for a patient depending on how isPositive is set.</summary>
		public static List<Adjustment> GetAdjustForPats(List<long> listPatNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Adjustment>>(MethodBase.GetCurrentMethod(),listPatNums);
			}
			string command="SELECT * FROM adjustment "
				+"WHERE PatNum IN("+String.Join(", ",listPatNums)+") ";
			return Crud.AdjustmentCrud.SelectMany(command);
		}

		///<summary></summary>
		public static void Update(Adjustment adj){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),adj);
				return;
			}
			Crud.AdjustmentCrud.Update(adj);
		}

		///<summary></summary>
		public static long Insert(Adjustment adj) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				adj.SecUserNumEntry=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				adj.AdjNum=Meth.GetLong(MethodBase.GetCurrentMethod(),adj);
				return adj.AdjNum;
			}
			return Crud.AdjustmentCrud.Insert(adj);
		}

		///<summary>This will soon be eliminated or changed to only allow deleting on same day as EntryDate.</summary>
		public static void Delete(Adjustment adj){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),adj);
				return;
			}
			Crud.AdjustmentCrud.Delete(adj.AdjNum);
		}

		///<summary>Loops through the supplied list of adjustments and returns an ArrayList of adjustments for the given proc.</summary>
		public static ArrayList GetForProc(long procNum,Adjustment[] List) {
			//No need to check RemotingRole; no call to db.
			ArrayList retVal=new ArrayList();
			for(int i=0;i<List.Length;i++){
				if(List[i].ProcNum==procNum){
					retVal.Add(List[i]);
				}
			}
			return retVal;
		}

		public static List<Adjustment> GetForProcs(List<long> listProcNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Adjustment>>(MethodBase.GetCurrentMethod(),listProcNums);
			}
			List<Adjustment> listAdjustments=new List<Adjustment>();
			if(listProcNums==null || listProcNums.Count < 1) {
				return listAdjustments;
			}
			string command="SELECT * FROM adjustment WHERE ProcNum IN("+string.Join(",",listProcNums)+")";
			return Crud.AdjustmentCrud.SelectMany(command);
		}

		///<summary>Sums all adjustments for a proc then returns that sum.</summary>
		public static double GetTotForProc(long procNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetDouble(MethodBase.GetCurrentMethod(),procNum);
			}
			string command="SELECT SUM(AdjAmt) FROM adjustment"
				+" WHERE ProcNum="+POut.Long(procNum);
			return PIn.Double(Db.GetScalar(command));
		}

		///<summary>Creates a new discount adjustment for the given procedure.</summary>
		public static void CreateAdjustmentForDiscount(Procedure procedure) {
			//No need to check RemotingRole; no call to db.
			Adjustment AdjustmentCur=new Adjustment();
			AdjustmentCur.DateEntry=DateTime.Today;
			AdjustmentCur.AdjDate=DateTime.Today;
			AdjustmentCur.ProcDate=procedure.ProcDate;
			AdjustmentCur.ProvNum=procedure.ProvNum;
			AdjustmentCur.PatNum=procedure.PatNum;
			AdjustmentCur.AdjType=PrefC.GetLong(PrefName.TreatPlanDiscountAdjustmentType);
			AdjustmentCur.ClinicNum=procedure.ClinicNum;
			AdjustmentCur.AdjAmt=-procedure.Discount;//Discount must be negative here.
			AdjustmentCur.ProcNum=procedure.ProcNum;
			Adjustments.Insert(AdjustmentCur);
		}

		///<summary>Creates a new discount adjustment for the given procedure using the discount plan fee.</summary>
		public static void CreateAdjustmentForDiscountPlan(Procedure procedure) {
			//No need to check RemotingRole; no call to db.
			DiscountPlan discountPlan=DiscountPlans.GetPlan(Patients.GetPat(procedure.PatNum).DiscountPlanNum);
			if(discountPlan==null) {
				return;//No discount plan.
			}
			//Figure out how much the patient saved and make an adjustment for the difference so that the office find how much money they wrote off.
			double discountAmt=Fees.GetAmount(procedure.CodeNum,discountPlan.FeeSchedNum,procedure.ClinicNum,procedure.ProvNum);
			if(discountAmt==-1) {
				return;//No fee entered, don't make adjustment.
			}
			double adjAmt=procedure.ProcFee-discountAmt;
			if(adjAmt <= 0) {
				return;//We do not need to create adjustments for 0 dollars.
			}
			Adjustment adjustmentCur=new Adjustment();
			adjustmentCur.DateEntry=DateTime.Today;
			adjustmentCur.AdjDate=DateTime.Today;
			adjustmentCur.ProcDate=procedure.ProcDate;
			adjustmentCur.ProvNum=procedure.ProvNum;
			adjustmentCur.PatNum=procedure.PatNum;
			adjustmentCur.AdjType=discountPlan.DefNum;
			adjustmentCur.ClinicNum=procedure.ClinicNum;
			adjustmentCur.AdjAmt=(-adjAmt);
			adjustmentCur.ProcNum=procedure.ProcNum;
			Adjustments.Insert(adjustmentCur);
			SecurityLogs.MakeLogEntry(Permissions.AdjustmentCreate,procedure.PatNum,"Adjustment made for discount plan: "+adjustmentCur.AdjAmt.ToString("f"));
		}

		///<summary>Deletes all adjustments for a procedure</summary>
		public static void DeleteForProcedure(long procNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),procNum);
				return;
			}
			//Create log for each adjustment that is going to be deleted.
			string command="SELECT * FROM adjustment WHERE ProcNum = "+POut.Long(procNum); //query for all adjustments of a procedure 
			List<Adjustment> listAdjustments=Crud.AdjustmentCrud.SelectMany(command);
			for(int i=0;i<listAdjustments.Count;i++) { //loops through the rows
				SecurityLogs.MakeLogEntry(Permissions.AdjustmentEdit,listAdjustments[i].PatNum, //and creates audit trail entry for every row to be deleted
				"Delete adjustment for patient: "
				+Patients.GetLim(listAdjustments[i].PatNum).GetNameLF()+", "
				+(listAdjustments[i].AdjAmt).ToString("c"),0,listAdjustments[i].SecDateTEdit);
			}
			//Delete each adjustment for the procedure.
			command="DELETE FROM adjustment WHERE ProcNum = "+POut.Long(procNum);
			Db.NonQ(command);
		}

		/// <summary>Returns a DataTable of adjustments of a given adjustment type and for a given pat</summary>
		public static List<Adjustment> GetAdjustForPatByType(long patNum,long adjType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Adjustment>>(MethodBase.GetCurrentMethod(),patNum,adjType);
			}
			string queryBrokenApts="SELECT * FROM adjustment WHERE PatNum="+POut.Long(patNum)
				+" AND AdjType="+POut.Long(adjType);
			return Crud.AdjustmentCrud.SelectMany(queryBrokenApts);
		}

		/// <summary>Returns a dictionary of adjustments of a given adjustment type and for the given pats such that the key is the patNum.
		/// Every patNum given will exist as key with a list in the returned dictonary.
		/// Only considers adjs where AdjDate is strictly less than the given maxAdjDate.</summary>
		public static SerializableDictionary<long,List<Adjustment>> GetAdjustForPatsByType(List<long> listPatNums,long adjType,DateTime maxAdjDate) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<SerializableDictionary<long,List<Adjustment>>>(MethodBase.GetCurrentMethod(),listPatNums,adjType,maxAdjDate);
			}
			if(listPatNums==null || listPatNums.Count==0) {
				return new SerializableDictionary<long, List<Adjustment>>();
			}
			string queryBrokenApts="SELECT * FROM adjustment "
				+"WHERE PatNum IN ("+string.Join(",",listPatNums)+") "
				+"AND AdjType="+POut.Long(adjType)+" "
				+"AND "+DbHelper.DateTConditionColumn("AdjDate",ConditionOperator.LessThan,maxAdjDate);
			List<Adjustment> listAdjs=Crud.AdjustmentCrud.SelectMany(queryBrokenApts);
			SerializableDictionary<long,List<Adjustment>> retVal=new SerializableDictionary<long,List<Adjustment>>();
			foreach(long patNum in listPatNums) {
				retVal[patNum]=listAdjs.FindAll(x => x.PatNum==patNum);
			}
			return retVal;
		}

		///<summary>Used from ContrAccount and ProcEdit to display and calculate adjustments attached to procs.</summary>
		public static double GetTotForProc(long procNum,Adjustment[] List,long excludedNum=0) {
			//No need to check RemotingRole; no call to db.
			double retVal=0;
			for(int i=0;i<List.Length;i++){
				if((List[i].AdjNum==excludedNum)) {
					continue;
				}
				if(List[i].ProcNum==procNum){
					retVal+=List[i].AdjAmt;
				}
			}
			return retVal;
		}

		/*
		///<summary>Must make sure Refresh is done first.  Returns the sum of all adjustments for this patient.  Amount might be pos or neg.</summary>
		public static double ComputeBal(Adjustment[] List){
			double retVal=0;
			for(int i=0;i<List.Length;i++){
				retVal+=List[i].AdjAmt;
			}
			return retVal;
		}*/

		///<summary>Returns the number of finance or billing charges deleted.</summary>
		public static long UndoFinanceOrBillingCharges(DateTime dateUndo,bool isBillingCharges) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod(),dateUndo,isBillingCharges);
			}
			string adjTypeStr="Finance";
			long adjTypeDefNum=PrefC.GetLong(PrefName.FinanceChargeAdjustmentType);
			if(isBillingCharges) {
				adjTypeStr="Billing";
				adjTypeDefNum=PrefC.GetLong(PrefName.BillingChargeAdjustmentType);
			}
			string command="SELECT adjustment.AdjAmt,patient.PatNum,patient.Guarantor,patient.LName,patient.FName,patient.Preferred,patient.MiddleI,"
				+"adjustment.SecDateTEdit "
				+"FROM adjustment "
				+"INNER JOIN patient ON patient.PatNum=adjustment.PatNum "
				+"WHERE AdjDate="+POut.Date(dateUndo)+" "
				+"AND AdjType="+POut.Long(adjTypeDefNum);
			DataTable table=Db.GetTable(command);
			List<Action> listActions=new List<Action>();
			int loopCount=0;
			foreach(DataRow row in table.Rows) {//loops through the rows and creates audit trail entry for every row to be deleted
				listActions.Add(new Action(() => {
					SecurityLogs.MakeLogEntry(Permissions.AdjustmentEdit,PIn.Long(row["PatNum"].ToString()),
						"Delete adjustment for patient, undo "+adjTypeStr.ToLower()+" charges: "
						+Patients.GetNameLF(row["LName"].ToString(),row["FName"].ToString(),row["Preferred"].ToString(),row["MiddleI"].ToString())
						+", "+PIn.Double(row["AdjAmt"].ToString()).ToString("c"),0,PIn.DateT(row["SecDateTEdit"].ToString()));
					if(++loopCount%5==0) {
						ODEvent.Fire(new ODEventArgs(adjTypeStr+"Charge",Lans.g("FinanceCharge","Creating log entries for "+adjTypeStr.ToLower()+" charges")
							+": "+loopCount+" out of "+table.Rows.Count));
					}
				}));
			}
			ODThread.RunParallel(listActions,TimeSpan.FromMinutes(2));
			command="DELETE FROM adjustment WHERE AdjDate="+POut.Date(dateUndo)+" AND AdjType="+POut.Long(adjTypeDefNum);
			ODEvent.Fire(new ODEventArgs(adjTypeStr+"Charge",Lans.g("FinanceCharge","Deleting")+" "+table.Rows.Count+" "
				+Lans.g("FinanceCharge",adjTypeStr.ToLower()+" charge adjustments")+"..."));
			return Db.NonQ(command);
		}

	}

	


	


}










