using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using OpenDentBusiness.Crud;
using CodeBase;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Claims{
		#region Get Methods
		#endregion

		#region Modification Methods
		
		#region Insert
		#endregion

		#region Update
		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods
		#endregion

		
		///<summary>Gets claimpaysplits attached to a claimpayment with the associated patient, insplan, and carrier. If showUnattached it also shows all claimpaysplits that have not been attached to a claimpayment. Pass (0,true) to just get all unattached (outstanding) claimpaysplits.</summary>
		public static List<ClaimPaySplit> RefreshByCheckOld(long claimPaymentNum,bool showUnattached) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimPaySplit>>(MethodBase.GetCurrentMethod(),claimPaymentNum,showUnattached);
			}
			string command=
				"SELECT claim.DateService,claim.ProvTreat,CONCAT(CONCAT(patient.LName,', '),patient.FName) patName_"//Changed from \"_patName\" to patName_ for MySQL 5.5. Also added checks for #<table> and $<table>
				+",carrier.CarrierName,SUM(claimproc.FeeBilled) feeBilled_,SUM(claimproc.InsPayAmt) insPayAmt_,claim.ClaimNum"
				+",claimproc.ClaimPaymentNum,(SELECT clinic.Description FROM clinic WHERE claimproc.ClinicNum = clinic.ClinicNum) Description,claim.PatNum,PaymentRow,claim.ClaimStatus "
				+" FROM claim,patient,insplan,carrier,claimproc"
				+" WHERE claimproc.ClaimNum = claim.ClaimNum"
				+" AND patient.PatNum = claim.PatNum"
				+" AND insplan.PlanNum = claim.PlanNum"
				+" AND insplan.CarrierNum = carrier.CarrierNum"
				+" AND (claimproc.Status = '1' OR claimproc.Status = '4' OR claimproc.Status=5)"//received or supplemental or capclaim
 				+" AND (claimproc.ClaimPaymentNum = '"+POut.Long(claimPaymentNum)+"'";
			if(showUnattached){
				command+=" OR (claimproc.InsPayAmt != 0 AND claimproc.ClaimPaymentNum = '0')";
			}
			//else shows only items attached to this payment
			command+=")"
				+" GROUP BY claim.DateService,claim.ProvTreat,CONCAT(CONCAT(patient.LName,', '),patient.FName) "
				+",carrier.CarrierName,claim.ClaimNum"
				+",claimproc.ClaimPaymentNum,claim.PatNum";
			command+=" ORDER BY patName_";
			DataTable table=Db.GetTable(command);
			return ClaimPaySplitTableToList(table);
		}

		///<summary></summary>
		public static List<Claim> GetClaimsByCheck(long claimPaymentNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Claim>>(MethodBase.GetCurrentMethod(),claimPaymentNum);
			}
			string command=
				"SELECT * "
				+"FROM claim "
				+"WHERE claim.ClaimNum IN "
				+"(SELECT DISTINCT claimproc.ClaimNum "
				+"FROM claimproc "
				+"WHERE claimproc.ClaimPaymentNum="+claimPaymentNum+")";
			return ClaimCrud.SelectMany(command);
		}

		///<summary>Gets all outstanding claims for the batch payment window.
		///If carrierName is not empty, then will return claims with matching or partially matching carrier name.
		///If Name is not empty, then will return claims for the specified patient names (space delimited list of names).
		///If claimID is not empty, then will return claims with matching or partially matching ClaimIdentifier.</summary>
		public static List<ClaimPaySplit> GetOutstandingClaims(string carrierName,string Name,DateTime claimPayDate,string claimID) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimPaySplit>>(MethodBase.GetCurrentMethod(),carrierName,Name,claimPayDate,claimID);
			}
			//Per Nathan, it is OK to return the DateService in the query result to display in the batch insurance window,
			//because that is the date which will be displayed in the Account module when you use the GoTo feature from batch insurance window.
			string command="SELECT * FROM ("
				+"SELECT claim.DateService,claim.ProvTreat,'' AS patName_,carrierA.CarrierName,claim.ClaimFee feeBilled_,claim.ClaimStatus,"
				+"SUM(claimproc.InsPayAmt) insPayAmt_,claim.ClaimNum,0 AS ClaimPaymentNum,claim.ClinicNum,'' AS Description,claim.PatNum,0 AS PaymentRow,"
				+"SUM(CASE WHEN claimproc.ClaimPaymentNum=0 THEN 0 ELSE 1 END) AttachedCount,"
				+"SUM(CASE WHEN claimproc.ClaimPaymentNum=0 AND claimproc.InsPayAmt!=0 THEN 1 ELSE 0 END) UnattachedPayCount "
				+"FROM ("
					+"SELECT insplan.PlanNum,carrier.CarrierName "
					+"FROM carrier "
					+"INNER JOIN insplan ON carrier.CarrierNum=insplan.CarrierNum "
					+"WHERE carrier.CarrierName LIKE '%"+POut.String(carrierName)+"%'"
				+") carrierA "
				+"INNER JOIN claim ON carrierA.PlanNum=claim.PlanNum AND claim.ClaimType!='PreAuth' ";
			if(claimID!="") {
				command+=" AND claim.ClaimIdentifier LIKE '%"+POut.String(claimID)+"%' ";
			}
			//See job #7423.
			//The claimproc.DateCP is essentially the same as the claim.DateReceived.
			//We used to use the claimproc.ProcDate, which is essentially the same as the claim.DateService.
			//Since the service date could be weeks or months in the past, it makes more sense to use the received date, which will be more recent.
			//Additionally, users found using the date of service to be unintuitive.
			//STRONG CAUTION not to use the claimproc.ProcDate here in the future.
			command+="INNER JOIN claimproc ON claimproc.ClaimNum=claim.ClaimNum "
				+"WHERE (claim.ClaimStatus='S' OR "
				+"(claim.ClaimStatus='R' AND (claimproc.InsPayAmt!=0 "+((claimPayDate.Year>1880)?("OR claimproc.DateCP>="+POut.Date(claimPayDate)):"")+"))) ";
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command+="GROUP BY claim.ClaimNum";
			}
			else {//oracle
				command+="GROUP BY claim.ClaimNum,claim.DateService,claim.ProvTreat,carrierA.CarrierName,claim.ClaimFee,claim.ClaimStatus,claim.ClinicNum,claim.PatNum";
			}
			command+=") outstanding WHERE UnattachedPayCount > 0 "//Either unfinalized ins pay amounts on at least one claimproc on the claim,
				//or if preference is enabled with a specific date, also include received "NO PAYMENT" claims.
				//Always show Sent claims regardless of preference to match version 16.4 behavior (see job B8189).
				+"OR (AttachedCount=0"+((claimPayDate.Year>1880)?"":" AND ClaimStatus='S'")+")";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0) {
				return new List<ClaimPaySplit>();//no claims
			}
			List<string> listNames=Name.Split(' ').ToList();
			command="SELECT PatNum,LName,FName FROM patient "
				+"WHERE PatNum IN("+string.Join(",",table.Rows.OfType<DataRow>().Select(x => x["PatNum"].ToString()))+") ";
			if(listNames.Count > 0) {
				command+="AND "+DbHelper.Concat("FName","LName")+" LIKE '%"+string.Join("%",listNames.Select(x => POut.String(x)))+"%' ";
			}
			DataTable patTable=Db.GetTable(command);
			if(patTable.Rows.Count==0) {
				return new List<ClaimPaySplit>();//all patients filtered out, return empty list
			}
			//make dictionary of key=PatNum, value=LName, FName, used to fill table patName_ column
			Dictionary<long,string> dictPatNames=patTable.Rows.OfType<DataRow>()
				.ToDictionary(x => PIn.Long(x["PatNum"].ToString()),x => x["LName"].ToString()+", "+x["FName"].ToString());
			//create dictionary of key=ClinicNum, value=Description, used to fill table Description column
			Dictionary<long,string> dictClinicNames=new Dictionary<long, string>();
			if(PrefC.HasClinicsEnabled && Clinics.GetCount() > 0) {
				dictClinicNames=Clinics.GetDeepCopy().ToDictionary(x => x.ClinicNum,x => x.Description);
			}
			long patNumCur;
			long clinicNumCur;
			for(int i=table.Rows.Count-1;i>-1;i--) {//itterate backwards
				patNumCur=PIn.Long(table.Rows[i]["PatNum"].ToString());
				if(!dictPatNames.ContainsKey(patNumCur)) {
					table.Rows.RemoveAt(i);//list filtered by name, remove from results
					continue;
				}
				table.Rows[i]["patName_"]=dictPatNames[patNumCur];
				clinicNumCur=PIn.Long(table.Rows[i]["ClinicNum"].ToString());
				if(dictClinicNames.ContainsKey(clinicNumCur)) {
					table.Rows[i]["Description"]=dictClinicNames[clinicNumCur];
				}
			}
			return ClaimPaySplitTableToList(table).OrderByDescending(x => x.Carrier.StartsWith(carrierName)).ThenBy(x => x.Carrier).ThenBy(x => x.PatName).ToList();
		}

		/// <summary>Gets all 'claims' attached to the claimpayment.</summary>
		public static List<ClaimPaySplit> GetAttachedToPayment(long claimPaymentNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimPaySplit>>(MethodBase.GetCurrentMethod(),claimPaymentNum);
			}
			string command=
				"SELECT claim.DateService,claim.ProvTreat,"+DbHelper.Concat("patient.LName","', '","patient.FName")+" patName_,"
				+"carrier.CarrierName,ClaimFee feeBilled_,SUM(claimproc.InsPayAmt) insPayAmt_,claim.ClaimNum,claim.ClaimStatus,"
				+"claimproc.ClaimPaymentNum,clinic.Description,claim.PatNum,PaymentRow "
				+" FROM claim,patient,insplan,carrier,claimproc"
				+" LEFT JOIN clinic ON clinic.ClinicNum = claimproc.ClinicNum"
				+" WHERE claimproc.ClaimNum = claim.ClaimNum"
				+" AND patient.PatNum = claim.PatNum"
				+" AND insplan.PlanNum = claim.PlanNum"
				+" AND insplan.CarrierNum = carrier.CarrierNum"
				+" AND claimproc.ClaimPaymentNum = "+claimPaymentNum+" ";
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command+="GROUP BY claim.ClaimNum ";
			}
			else {//oracle
				command+="GROUP BY claim.DateService,claim.ProvTreat,"+DbHelper.Concat("patient.LName","', '","patient.FName")
					+",carrier.CarrierName,claim.ClaimNum,claimproc.ClaimPaymentNum,claim.PatNum,ClaimFee,clinic.Description,PaymentRow ";
			}
			command+="ORDER BY claimproc.PaymentRow";
			DataTable table=Db.GetTable(command);
			return ClaimPaySplitTableToList(table);
		}

		/// <summary>Gets all secondary claims for the related ClaimPaySplits. Called after a payment has been received.</summary>
		public static DataTable GetSecondaryClaims(List<ClaimPaySplit> claimsAttached) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),claimsAttached);
			}
			string command="SELECT DISTINCT ProcNum FROM claimproc WHERE ClaimNum IN (";
			string claimNums="";//used twice
			for(int i=0;i<claimsAttached.Count;i++) {
				if(i>0) {
					claimNums+=",";
				}
				claimNums+=claimsAttached[i].ClaimNum;
			}
			command+=claimNums+") AND ProcNum!=0";
			//List<ClaimProc> tempClaimProcs=ClaimProcCrud.SelectMany(command);
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0) {
				return new DataTable();//No procedures are attached to these claims.  This frequently happens in conversions.  No need to look for related secondary claims.
			}
			command="SELECT claimproc.PatNum,claimproc.ProcDate"
				+" FROM claimproc"
				+" JOIN claim ON claimproc.ClaimNum=claim.ClaimNum"
				+" WHERE ProcNum IN (";
			for(int i=0;i<table.Rows.Count;i++) {
				if(i>0) {
					command+=",";
				}
				command+=table.Rows[i]["ProcNum"].ToString();
			}
			command+=") AND claimproc.ClaimNum NOT IN ("+claimNums+")"
				+" AND ClaimType = 'S'"
				+" GROUP BY claimproc.ClaimNum,claimproc.PatNum,claimproc.ProcDate";
			DataTable secondaryClaims=Db.GetTable(command);
			return secondaryClaims;
		}

		///<summary></summary>
		public static List<ClaimPaySplit> GetInsPayNotAttachedForFixTool() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ClaimPaySplit>>(MethodBase.GetCurrentMethod());
			}
			string command=
				"SELECT claim.DateService,claim.ProvTreat,CONCAT(CONCAT(patient.LName,', '),patient.FName) patName_"
				+",carrier.CarrierName,SUM(claimproc.FeeBilled) feeBilled_,SUM(claimproc.InsPayAmt) insPayAmt_,claim.ClaimNum,claim.ClaimStatus"
				+",claimproc.ClaimPaymentNum,(SELECT clinic.Description FROM clinic WHERE claimproc.ClinicNum = clinic.ClinicNum) Description,claim.PatNum,PaymentRow "
				+" FROM claim,patient,insplan,carrier,claimproc"
				+" WHERE claimproc.ClaimNum = claim.ClaimNum"
				+" AND patient.PatNum = claim.PatNum"
				+" AND insplan.PlanNum = claim.PlanNum"
				+" AND insplan.CarrierNum = carrier.CarrierNum"
				+" AND (claimproc.Status = '1' OR claimproc.Status = '4' OR claimproc.Status=5)"//received or supplemental or capclaim
				+" AND (claimproc.InsPayAmt != 0 AND claimproc.ClaimPaymentNum = '0')"
				+" GROUP BY claim.DateService,claim.ProvTreat,CONCAT(CONCAT(patient.LName,', '),patient.FName)"
				+",carrier.CarrierName,claim.ClaimNum,claimproc.ClaimPaymentNum,claim.PatNum"
				+" ORDER BY patName_";
			DataTable table=Db.GetTable(command);
			return ClaimPaySplitTableToList(table);
		}

		///<summary></summary>
		private static List<ClaimPaySplit> ClaimPaySplitTableToList(DataTable table) {
			//No need to check RemotingRole; no call to db.
			List<ClaimPaySplit> splits=new List<ClaimPaySplit>();
			ClaimPaySplit split;
			for(int i=0;i<table.Rows.Count;i++){
				split=new ClaimPaySplit();
				split.DateClaim      =PIn.Date  (table.Rows[i]["DateService"].ToString());
				split.ProvAbbr       =Providers.GetAbbr(PIn.Long(table.Rows[i]["ProvTreat"].ToString()));
				split.PatName        =PIn.String(table.Rows[i]["patName_"].ToString());
				split.PatNum         =PIn.Long  (table.Rows[i]["PatNum"].ToString());
				split.Carrier        =PIn.String(table.Rows[i]["CarrierName"].ToString());
				split.FeeBilled      =PIn.Double(table.Rows[i]["feeBilled_"].ToString());
				split.InsPayAmt      =PIn.Double(table.Rows[i]["insPayAmt_"].ToString());
				split.ClaimNum       =PIn.Long  (table.Rows[i]["ClaimNum"].ToString());
				split.ClaimPaymentNum=PIn.Long  (table.Rows[i]["ClaimPaymentNum"].ToString());
				split.PaymentRow     =PIn.Int   (table.Rows[i]["PaymentRow"].ToString());
				split.ClinicDesc	 =PIn.String(table.Rows[i]["Description"].ToString());
				split.ClaimStatus    =PIn.String(table.Rows[i]["ClaimStatus"].ToString());
				splits.Add(split);
			}
			return splits;
		}

		///<summary>Gets the specified claim from the database.  Can be null.</summary>
		public static Claim GetClaim(long claimNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Claim>(MethodBase.GetCurrentMethod(),claimNum);
			}
			string command="SELECT * FROM claim"
				+" WHERE ClaimNum = "+claimNum.ToString();
			Claim retClaim=Crud.ClaimCrud.SelectOne(command);
			if(retClaim==null){
				return null;
			}
			command="SELECT * FROM claimattach WHERE ClaimNum = "+POut.Long(claimNum);
			retClaim.Attachments=Crud.ClaimAttachCrud.SelectMany(command);
			return retClaim;
		}

		public static List<Claim> GetClaimsFromClaimNums(List<long> listClaimNums) {
			DataTable claimTable=GetClaimTableFromClaimNums(listClaimNums);
			List<Claim> list=Crud.ClaimCrud.TableToList(claimTable);
			return list;
		}

		public static DataTable GetClaimTableFromClaimNums(List<long> listClaimNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),listClaimNums);
			}
			if(listClaimNums.Count==0) {
				return new DataTable();
			}
			string command="SELECT * FROM claim WHERE ClaimNum IN (";
			string claimNums="";//used twice
			for(int i = 0;i<listClaimNums.Count;i++) {
				if(i>0) {
					claimNums+=",";
				}
				claimNums+=listClaimNums[i];
			}
			command+=claimNums+")";
			DataTable table=Db.GetTable(command);
			return table;
		}

		///<summary>Gets all claims for the specified patient. But without any attachments.</summary>
		public static List<Claim> Refresh(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Claim>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command=
				"SELECT * FROM claim"
				+" WHERE PatNum = "+patNum.ToString()
				+" ORDER BY dateservice";
			return Crud.ClaimCrud.SelectMany(command);
		}

		public static Claim GetFromList(List<Claim> list,long claimNum) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<list.Count;i++) {
				if(list[i].ClaimNum==claimNum) {
					return list[i].Copy();
				}
			}
			return null;
		}

		///<summary></summary>
		public static long Insert(Claim claim) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				claim.SecUserNumEntry=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				claim.ClaimNum=Meth.GetLong(MethodBase.GetCurrentMethod(),claim);
				return claim.ClaimNum;
			}
			return Crud.ClaimCrud.Insert(claim);
		}

		///<summary></summary>
		public static void Update(Claim claim){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),claim);
				return;
			}
			Crud.ClaimCrud.Update(claim);
			//now, delete all attachments and recreate.
			string command="DELETE FROM claimattach WHERE ClaimNum="+POut.Long(claim.ClaimNum);
			Db.NonQ(command);
			for(int i=0;i<claim.Attachments.Count;i++) {
				claim.Attachments[i].ClaimNum=claim.ClaimNum;
				ClaimAttaches.Insert(claim.Attachments[i]);
			}
		}

		///<summary></summary>
		public static void Delete(Claim claim){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),claim);
				return;
			}
			Crud.ClaimCrud.Delete(claim.ClaimNum);
		}

		///<summary></summary>
		public static void DetachProcsFromClaim(Claim Cur){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),Cur);
				return;
			}
			string command = "UPDATE procedurelog SET "
				+"claimnum = '0' "
				+"WHERE claimnum = '"+POut.Long(Cur.ClaimNum)+"'";
			//MessageBox.Show(string command);
			Db.NonQ(command);
		}

		/*
		///<summary>Called from claimsend window and from Claim edit window.  Use 0 to get all waiting claims, or an actual claimnum to get just one claim.</summary>
		public static ClaimSendQueueItem[] GetQueueList(){
			return GetQueueList(0,0);
		}*/

		///<summary>Called from claimsend window and from Claim edit window.  Use 0 to get all waiting claims, or an actual claimnum to get just one claim.</summary>
		public static ClaimSendQueueItem[] GetQueueList(long claimNum,long clinicNum,long customTracking) {
			List<long> listClaimNums=new List<long>();
			if(claimNum!=0) {
				listClaimNums.Add(claimNum);
			}
			return GetQueueList(listClaimNums,clinicNum,customTracking);
		}

		///<summary>Called from claimsend window and from Claim edit window.  Use an empty listClaimNums to get all waiting claims.</summary>
		public static ClaimSendQueueItem[] GetQueueList(List<long> listClaimNums,long clinicNum,long customTracking) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<ClaimSendQueueItem[]>(MethodBase.GetCurrentMethod(),listClaimNums,clinicNum,customTracking);
			}
			string icd9Command=@"IFNULL((SELECT COUNT(*)
						FROM procedurelog
						INNER JOIN claimproc ON claimproc.ProcNum=procedurelog.ProcNum
						WHERE procedurelog.IcdVersion=9
						AND (procedurelog.DiagnosticCode!='' OR procedurelog.DiagnosticCode2!='' OR procedurelog.DiagnosticCode3!='' 
							OR procedurelog.DiagnosticCode4!='')
						AND claimproc.ClaimNum=claim.ClaimNum
						),FALSE) HasIcd9";
			string command=
				"SELECT claim.ClaimNum,carrier.NoSendElect"
				+",CONCAT(CONCAT(CONCAT(concat(patient.LName,', '),patient.FName),' '),patient.MiddleI)"
				+",claim.ClaimStatus,carrier.CarrierName,patient.PatNum,carrier.ElectID,MedType,claim.DateService,claim.ClinicNum,claim.CustomTracking,"+
				icd9Command+" "
				+"FROM claim "
				+"Left join insplan on claim.PlanNum = insplan.PlanNum "
				+"Left join carrier on insplan.CarrierNum = carrier.CarrierNum "
				+"Left join patient on patient.PatNum = claim.PatNum ";
			if(listClaimNums.Count==0){
				command+="WHERE (claim.ClaimStatus = 'W' OR claim.ClaimStatus = 'P') ";
			}
			else{
				command+="WHERE claim.ClaimNum IN("+string.Join(",",listClaimNums)+") ";
			}
			if(clinicNum>0) {
				command+="AND claim.ClinicNum="+POut.Long(clinicNum)+" ";
			}
			if(customTracking>0) {
				command+="AND claim.CustomTracking="+POut.Long(customTracking)+" ";
			}
			command+="ORDER BY claim.DateService,patient.LName";
			DataTable table=Db.GetTable(command);
			ClaimSendQueueItem[] listQueue=new ClaimSendQueueItem[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++){
				listQueue[i]=new ClaimSendQueueItem();
				listQueue[i].ClaimNum        = PIn.Long  (table.Rows[i][0].ToString());
				listQueue[i].NoSendElect     = PIn.Bool  (table.Rows[i][1].ToString());
				listQueue[i].PatName         = PIn.String(table.Rows[i][2].ToString());
				listQueue[i].ClaimStatus     = PIn.String(table.Rows[i][3].ToString());
				listQueue[i].Carrier         = PIn.String(table.Rows[i][4].ToString());
				listQueue[i].PatNum          = PIn.Long  (table.Rows[i][5].ToString());
				string payorID=PIn.String(table.Rows[i]["ElectID"].ToString());
				EnumClaimMedType medType=(EnumClaimMedType)PIn.Int(table.Rows[i]["MedType"].ToString());
				listQueue[i].ClearinghouseNum=Clearinghouses.AutomateClearinghouseHqSelection(payorID,medType);
				listQueue[i].MedType=medType;
				listQueue[i].DateService     = PIn.Date  (table.Rows[i]["DateService"].ToString());
				listQueue[i].ClinicNum		 = PIn.Long	 (table.Rows[i]["ClinicNum"].ToString());
				listQueue[i].CustomTracking		= PIn.Long (table.Rows[i]["CustomTracking"].ToString());
				listQueue[i].HasIcd9   = PIn.Bool(table.Rows[i]["HasIcd9"].ToString());
			}
			return listQueue;
		}

		///<summary>Supply claimnums. Called from X12 to begin the sorting process on claims going to one clearinghouse.</summary>
		public static List<X12TransactionItem> GetX12TransactionInfo(long claimNum) {
			//No need to check RemotingRole; no call to db.
			List<long> claimNums=new List<long>();
			claimNums.Add(claimNum);
			return GetX12TransactionInfo(claimNums);
		}

		///<summary>Supply claimnums. Called from X12 to begin the sorting process on claims going to one clearinghouse.</summary>
		public static List<X12TransactionItem> GetX12TransactionInfo(List<long> claimNums) {//ArrayList queueItemss){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<X12TransactionItem>>(MethodBase.GetCurrentMethod(),claimNums);
			}
			List<X12TransactionItem> retVal=new List<X12TransactionItem>();
			if(claimNums.Count<1) {
				return retVal;
			}
			string command;
			command="SELECT carrier.ElectID,claim.ProvBill,inssub.Subscriber,"
				+"claim.PatNum,claim.ClaimNum,CASE WHEN inssub.Subscriber!=claim.PatNum THEN 1 ELSE 0 END AS subscNotPatient "
				+"FROM claim,insplan,inssub,carrier "
				+"WHERE claim.PlanNum=insplan.PlanNum "
				+"AND claim.InsSubNum=inssub.InsSubNum "
				+"AND carrier.CarrierNum=insplan.CarrierNum "
				+"AND claim.ClaimNum IN ("+String.Join(",",claimNums)+") "
				+"ORDER BY carrier.ElectID,claim.ProvBill,inssub.Subscriber,subscNotPatient,claim.PatNum";
			DataTable table=Db.GetTable(command);
			//object[,] myA=new object[5,table.Rows.Count];
			X12TransactionItem item;
			for(int i=0;i<table.Rows.Count;i++){
				item=new X12TransactionItem();
				item.PayorId0=PIn.String(table.Rows[i][0].ToString());
				item.ProvBill1=PIn.Long   (table.Rows[i][1].ToString());
				item.Subscriber2=PIn.Long   (table.Rows[i][2].ToString());
				item.PatNum3=PIn.Long   (table.Rows[i][3].ToString());
				item.ClaimNum4=PIn.Long   (table.Rows[i][4].ToString());
				retVal.Add(item);
			}
			return retVal;
		}

		///<summary>Also sets the DateSent to today.</summary>
		public static void SetCanadianClaimSent(long claimNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),claimNum);
				return;
			}
			string command="UPDATE claim SET ClaimStatus = 'S',"
					+"DateSent= "+POut.Date(MiscData.GetNowDateTime())+", "
					+" DateSentOrig= "+POut.Date(MiscData.GetNowDateTime())
					+" WHERE ClaimNum = "+POut.Long(claimNum);
			Db.NonQ(command);
		}

		public static bool IsClaimIdentifierInUse(string claimIdentifier,long claimNumExclude,string claimType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),claimIdentifier,claimNumExclude,claimType);
			}
			string command="SELECT COUNT(*) FROM claim WHERE ClaimIdentifier='"+POut.String(claimIdentifier)+"' AND ClaimNum<>"+POut.Long(claimNumExclude);
			if(claimType=="PreAuth") {
				command+=" AND ClaimType='PreAuth'";
			}
			else {
				command+=" AND ClaimType!='PreAuth'";
			}
			return (Db.GetTable(command).Rows[0][0].ToString()!="0");
		}

		public static bool IsReferralAttached(long referralNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),referralNum);
			}
			string command="SELECT COUNT(*) FROM claim WHERE OrderingReferralNum="+POut.Long(referralNum);
 			if(Db.GetCount(command)=="0") {
				return false;
			}
			return true;
		}

		///<summary>Returns a list of claimnums matching the list of x12claims given.
		///The returned list is always same length as the list of x12claims, unless there is an error, in which case null is returned.
		///If a claim in the database is not found for a specific x12claim, then a value of 0 will be placed into the return list for that x12claim.
		///Each matched claim will either begin with the specified claimIdentifier, or will be for the patient name and subscriber ID specified.</summary>
		public static List <long> GetClaimFromX12(List <X12ClaimMatch> listX12claims) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),listX12claims);
			}
			if(listX12claims.Count==0) {
				return null;
			}
			//Usually claims from the same ERA will all have dates of service within a few weeks of each other.
			DateTime dateMin=DateTime.MinValue;
			if(listX12claims.Where(x => x.DateServiceStart!=DateTime.MinValue).Count() > 0) {
				dateMin=listX12claims.Where(x => x.DateServiceStart!=DateTime.MinValue).Select(x => x.DateServiceStart).Min();//DateServiceStart can be min value for PreAuths.
			}
			DateTime dateMax=DateTime.MinValue;
			if(listX12claims.Where(x => x.DateServiceEnd!=DateTime.MinValue).Count() > 0) {
				dateMax=listX12claims.Where(x => x.DateServiceEnd!=DateTime.MinValue).Select(x => x.DateServiceEnd).Max();//DateServiceEnd can be min value for PreAuths.
			}
			//Dictionary such that:
			//Key => EtransNum
			//Value => Dictionary such that: key => ClaimIdentifier and value => list of X12ClaimMatches.
			Dictionary<long,Dictionary<string,List<X12ClaimMatch>>> dictMatchesPerClaimId=listX12claims
				.GroupBy(x => x.EtransNum)
				.ToDictionary(
					x => x.Key,//EtransNum
					x => x.GroupBy(y => y.ClaimIdentifier)
						.ToDictionary(
							y => y.Key,//ClaimIdentifier
							y => y.ToList()//List of X12ClaimMatches
						)
				);
			double feeMin=0;
			double feeMax=0;
			Dictionary<long,Dictionary<string,double>> dictTotalClaimFee=new Dictionary<long,Dictionary<string,double>>();
			foreach(long etransNum in dictMatchesPerClaimId.Keys) {
				dictTotalClaimFee[etransNum]=new Dictionary<string,double>();
				foreach(string claimIdentifier in dictMatchesPerClaimId[etransNum].Keys) {
					double claimFee=dictMatchesPerClaimId[etransNum][claimIdentifier]
						.Sum(x => (x.Is835Reversal?0:x.ClaimFee));//Ignore claim reversals, because they negate the original claim fee.
					feeMin=Math.Min(feeMin,claimFee);
					feeMax=Math.Max(feeMax,claimFee);
					dictTotalClaimFee[etransNum][claimIdentifier]=claimFee;
				}
			}
			if(dateMin.Year<1880 || dateMax.Year<1880) {
				//Service dates are required for us to continue.
				//In 227s, the claim dates of service are required and should be present.
				//In 835s, we pull the procedure dates up into the claim dates of service if the claim dates are of service are not present.
				return null;
			}
			//We always require the claim fee and dates of service to match, then we use other criteria below to wisely choose from the shorter list of claims.
			//The list of claims with matching fee and date of service should be very short.  Worst case, the list would contain all of the appointments for a few days if every claim had the same fee (rare).
			string command="SELECT claim.ClaimNum,claim.ClaimIdentifier,claim.ClaimStatus,ROUND(ClaimFee,2) ClaimFee,claim.DateService,"
					+"patient.LName,patient.FName,inssub.SubscriberID "
				+"FROM claim "
				+"INNER JOIN patient ON patient.PatNum=claim.PatNum "
				+"INNER JOIN inssub ON inssub.InsSubNum=claim.InsSubNum AND claim.PlanNum=inssub.PlanNum "
				+"WHERE "+DbHelper.DtimeToDate("DateService")+">="+POut.Date(dateMin)+" AND "+DbHelper.DtimeToDate("DateService")+"<="+POut.Date(dateMax)+" "
				+"AND ROUND(ClaimFee,2)>="+POut.Double(feeMin)+" AND ROUND(ClaimFee,2)<="+POut.Double(feeMax);
			DataTable tableClaims=Db.GetTable(command);
			Dictionary<DateTime,List<DataRow>> dictClaims=new Dictionary<DateTime, List<DataRow>>();
			foreach(DataRow row in tableClaims.Rows) {
				DateTime key=PIn.Date(row["DateService"].ToString());
				if(!dictClaims.ContainsKey(key)) {
					dictClaims.Add(key,new List<DataRow>());
				}
				dictClaims[key].Add(row);
			}
			List<long> listAllEraProcNums=dictMatchesPerClaimId
				.SelectMany(x => x.Value.SelectMany(y => y.Value))//x.Value => Dictionary<string,List<X12ClaimMatch>> to one big List<X12ClaimMatch>
				.SelectMany(y => y.ListProcNums).Distinct().ToList();//List<X12ClaimMatch> to List<ProcNums>
			List<ClaimProcStatus> listClaimProcStatuses=new List<ClaimProcStatus>();//ClaimProcStatuses that have procNums.
			listClaimProcStatuses.Add(ClaimProcStatus.NotReceived);
			listClaimProcStatuses.Add(ClaimProcStatus.Received);
			listClaimProcStatuses.Add(ClaimProcStatus.Preauth);
			listClaimProcStatuses.Add(ClaimProcStatus.CapClaim);
			listClaimProcStatuses.Add(ClaimProcStatus.CapComplete);
			List<ClaimProc> listAllClaimProcs=ClaimProcs.GetForProcs(listAllEraProcNums)//Only runs query if list contains items.
				.Where(x => x.Status.In(listClaimProcStatuses))
				.ToList();
			List<ClaimProc> listAccountClaimProcs=listAllClaimProcs.Where(x => x.Status!=ClaimProcStatus.Preauth).ToList();
			List<ClaimProc> listTreatPlanClaimProcs=listAllClaimProcs.Where(x => x.Status==ClaimProcStatus.Preauth).ToList();
			List<PatPlan> listPatPlans=PatPlans.GetListByInsSubNums(listAllClaimProcs.Select(x => x.InsSubNum).ToList());//Only runs query if list contains items.
			List <long> listClaimNums=new List<long>(new long[listX12claims.Count]);//Done this way to guarantee that each claimnum is initialized to 0.
			//For each provided etrans, we look at 1 group such that the key is the claimIdentifier and the value is the list of all claim matches assocaited to the claimIdentifier.
			//This means that each entry in the list of claim matches should share many fields like, claimIdentifier, patient FName, patient LName and subscriber ID.
			int matchCount=0;
			foreach(long etransNum in dictMatchesPerClaimId.Keys) {
				foreach(string claimIdentifier in dictMatchesPerClaimId[etransNum].Keys) {
					X12ClaimMatch xclaim=dictMatchesPerClaimId[etransNum][claimIdentifier].First();
					List<long> listEraProcNums=dictMatchesPerClaimId[etransNum][claimIdentifier].SelectMany(x => x.ListProcNums).ToList();//All identified procNums reported.
					List<ClaimProc> listClaimProcs=new List<ClaimProc>();
					switch(xclaim.CodeClp02) {
						case "1"://"Processed as Primary"
						case "19"://"Processed as Primary, Forwarded to Additional Payer(s)"
							listClaimProcs=ClaimProcs.GetForProcsWithOrdinalFromList(listEraProcNums,1,listPatPlans,listAccountClaimProcs);
							break;
						case "2"://"Processed as Secondary"
						case "20"://"Processed as Secondary, Forwarded to Additional Payer(s)"
							listClaimProcs=ClaimProcs.GetForProcsWithOrdinalFromList(listEraProcNums,2,listPatPlans,listAccountClaimProcs);
							break;
						case "3"://"Processed as Tertiary"
						case "21"://"Processed as Tertiary, Forwarded to Additional Payer(s)"
							listClaimProcs=ClaimProcs.GetForProcsWithOrdinalFromList(listEraProcNums,3,listPatPlans,listAccountClaimProcs);
							break;
						case "4"://"Denied"
						case "22"://"Reversal of Previous Payment"
						case "23"://"Not Our Claim, Forwarded to Additional Payer(s)"
							//The odds of all the claim nums matching here is lower, because we could match both primary and secondary.
							listClaimProcs=listAccountClaimProcs.Where(x => listEraProcNums.Contains(x.ProcNum)).ToList();
							break;
						case "25"://"Predetermination Pricing Only - No Payment"
							listClaimProcs=listTreatPlanClaimProcs.Where(x => listEraProcNums.Contains(x.ProcNum)).ToList();
							break;
					}
					if(listClaimProcs.Count>0) {//Successfully found internal claimProcs.
						long claimNumKey=listClaimProcs.First().ClaimNum;
						if(listClaimProcs.All(x => x.ClaimNum==claimNumKey)) {//All claimNums must match.
							matchCount++;
							foreach(X12ClaimMatch match in dictMatchesPerClaimId[etransNum][claimIdentifier]) {
								int index=listX12claims.IndexOf(match);
								listClaimNums[index]=claimNumKey;
							}
							continue;
						}
					}
					//Begin with basic filtering by date of service and claim total fee.
					List <DataRow> listDbClaims=new List<DataRow>();
					for(DateTime d=xclaim.DateServiceStart;d<=xclaim.DateServiceEnd;d=d.AddDays(1)) {
						if(dictClaims.ContainsKey(d)) {
							listDbClaims.AddRange(dictClaims[d].FindAll(x => PIn.Double(x["ClaimFee"].ToString())==dictTotalClaimFee[etransNum][claimIdentifier]));
						}
					}
					//Look for claim matched by full or partial claim identifier.
					List<int> listIndiciesForIdentifier=new List<int>();
					if(xclaim.ClaimIdentifier.Length > 0 && xclaim.ClaimIdentifier!="0") {//Ensure an ID is present and that it is not for a printed claim (when ID=="0").
						//Look for a single exact match by claim identifier.  This step is first, so that the user can override claim association to the 835 or 277 by changing the claim identifier if desired.
						for(int i=0;i<listDbClaims.Count;i++) {
							string claimId=PIn.String(listDbClaims[i]["ClaimIdentifier"].ToString());
							if(claimId==xclaim.ClaimIdentifier) {
								listIndiciesForIdentifier.Add(i);
							}
						}
						if(listIndiciesForIdentifier.Count==0 && xclaim.ClaimIdentifier.Length>15) {//No exact match found.  Look for similar claim identifiers if the identifer was possibly truncated when sent out.
							//Our claim identifiers can be longer than 20 characters (mostly when using replication). When the claim identifier is sent out on the claim, it is truncated to 20
							//characters. Therefore, if the claim identifier is longer than 20 characters, then it was truncated when sent out, so we have to look for claims beginning with the 
							//claim identifier given if there is not an exact match.  We also send shorter identifiers for some clearinghouses.  For example, the maximum claim identifier length
							//for Denti-Cal is 17 characters.
							for(int i=0;i<listDbClaims.Count;i++) {
								string claimId=PIn.String(listDbClaims[i]["ClaimIdentifier"].ToString());
								if(claimId.StartsWith(xclaim.ClaimIdentifier)) {
									listIndiciesForIdentifier.Add(i);
								}
							}
						}
					}
					if(listIndiciesForIdentifier.Count==0) {
						//No matches were found for the identifier.  Continue to more advanced matching below.
					}
					else if(listIndiciesForIdentifier.Count==1) {
						//A single match based on claim identifier, claim date of service, and claim fee.
						long claimNum=PIn.Long(listDbClaims[listIndiciesForIdentifier[0]]["ClaimNum"].ToString());
						foreach(X12ClaimMatch match in dictMatchesPerClaimId[etransNum][claimIdentifier]) {
							int index=listX12claims.IndexOf(match);
							listClaimNums[index]=claimNum;
						}
						continue;
					}
					else if(listIndiciesForIdentifier.Count>1) {//Edge case.
						//Multiple matches for the specified claim identifier AND date service AND fee.  The claim must have been split (rare because the split claims must have the same fee).
						//Continue to more advanced matching below, although it probably will not help.  We could enhance this specific scenario by picking a claim based on the procedures attached, but that is not a guarantee either.
					}
					//Locate claims exactly matching patient last name.
					List<DataRow> listMatches=new List<DataRow>();
					string patLname=xclaim.PatLname.Trim().ToLower();
					for(int i=0;i<listDbClaims.Count;i++) {
						string lastNameInDb=PIn.String(listDbClaims[i]["LName"].ToString()).Trim().ToLower();
						if(lastNameInDb==patLname) {
							listMatches.Add(listDbClaims[i]);
						}
					}
					//Locate claims matching exact first name or partial first name, with a preference for exact match.
					List<DataRow> listExactFirst=new List<DataRow>();
					List<DataRow> listPartFirst=new List<DataRow>();
					string patFname=xclaim.PatFname.Trim().ToLower();
					for(int i=0;i<listMatches.Count;i++) {
						string firstNameInDb=PIn.String(listMatches[i]["FName"].ToString()).Trim().ToLower();
						if(firstNameInDb==patFname) {
							listExactFirst.Add(listMatches[i]);
						}
						else if(firstNameInDb.Length>=2 && patFname.StartsWith(firstNameInDb)) {
							//Unfortunately, in the real world, we have observed carriers returning the patients first name followed by a space followed by the patient middle name all within the first name field.
							//This issue is probably due to human error when the carrier's staff typed the patient name into their system.  All we can do is try to cope with this situation.
							listPartFirst.Add(listMatches[i]);
						}
					}
					if(listExactFirst.Count>0) {
						listMatches=listExactFirst;//One or more exact matches found.  Ignore any partial matches.
					}
					else {
						listMatches=listPartFirst;//Use partial matches only if no exact matches were found.
					}
					//Locate claims matching exact subscriber ID or partial subscriber ID, with a preference for exact match.
					List<DataRow> listExactId=new List<DataRow>();
					List<DataRow> listPartId=new List<DataRow>();
					string subscriberId=xclaim.SubscriberId.Trim().ToUpper();
					for(int i=0;i<listMatches.Count;i++) {
						string subIdInDb=PIn.String(listMatches[i]["SubscriberID"].ToString()).Trim().ToUpper();
						if(subIdInDb==subscriberId) {
							listExactId.Add(listMatches[i]);
						}
						else if(subIdInDb.Length>=3 && (subscriberId==subIdInDb.Substring(0,subIdInDb.Length-1) || subscriberId==subIdInDb.Substring(0,subIdInDb.Length-2))) {
							//Partial subscriber ID matches are somewhat common.
							//Insurance companies sometimes create a base subscriber ID for all family members, then append a one or two digit number to make IDs unique for each family member.
							//We have seen at least one real world example where the ERA contained the base subscriber ID instead of the patient specific ID.
							//We also check that the subscriber ID in OD is at least 3 characters long, because we must allow for the 2 optional ending characters and we require an extra leading character to avoid matching blank IDs.
							listPartId.Add(listMatches[i]);
						}
						else if(subscriberId.Length>=3 && (subIdInDb==subscriberId.Substring(0,subscriberId.Length-1) || subIdInDb==subscriberId.Substring(0,subscriberId.Length-2))) {
							//Partial match in the other direction.  Comparable to the scenario above.
							listPartId.Add(listMatches[i]);
						}
						else if(subscriberId.Length >= 3 && subIdInDb.TrimStart('0')==subscriberId) {
							//Allow matches for leading zeros.
							listPartId.Add(listMatches[i]);
						}
						else if(subIdInDb.Length >= 3 && subscriberId.TrimStart('0')==subIdInDb) {
							//Allow matches for leading zeros.
							listPartId.Add(listMatches[i]);
						}
					}
					if(listExactId.Count>0) {
						listMatches=listExactId;//One or more exact matches found.  Ignore any partial matches.
					}
					else {
						listMatches=listPartId;//Use partial matches only if no exact matches were found.
					}
					long matchClaimNum=0;
					//We have finished locating the matches.  Now decide what to do based on the number of matches found.
					if(listMatches.Count==0) {
						matchClaimNum=0;
					}
					else if(listMatches.Count==1) {
						//A single match based on patient first name, patient last name, subscriber ID, claim date of service, and claim fee.
						matchClaimNum=PIn.Long(listMatches[0]["ClaimNum"].ToString());
					}
					else if(listMatches.Count>1) {//Edge case.
						//Multiple matches (rare).  We might be able to pick the correct claim based on the attached procedures, but we can worry about this situation later if it happens more than we expect.
						matchClaimNum=0;
					}
					foreach(X12ClaimMatch match in dictMatchesPerClaimId[etransNum][claimIdentifier]) {
						int index=listX12claims.IndexOf(match);
						listClaimNums[index]=matchClaimNum;
					}
				}//end foreach claim identifier
			}//end foreach etrans/835
			return listClaimNums;
		}

		///<summary>Returns the number of received claims attached to specified insplan.</summary>
		public static int GetCountReceived(long planNum) {
			//No need to check RemotingRole; no call to db.
			return GetCountReceived(planNum,0);
		}

		///<summary>Returns the number of received claims attached to specified subscriber with specified insplan.  Set insSubNum to zero to check all claims for all patients for the plan.</summary>
		public static int GetCountReceived(long planNum,long insSubNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod(),planNum,insSubNum);
			}
			string command;
			command="SELECT COUNT(*) "
				+"FROM claim "
				+"WHERE claim.ClaimStatus='R' "
				+"AND claim.PlanNum="+POut.Long(planNum)+" ";
			if(insSubNum!=0) {
				command+="AND claim.InsSubNum="+POut.Long(insSubNum);
			}
			return PIn.Int(Db.GetCount(command));
		}

		///<summary>Returns a human readable ClaimStatus string.</summary>
		public static string GetClaimStatusString(string claimStatus) {
			string retVal="";
			switch(claimStatus){
				case "U":
					retVal="Unsent";
					break;
				case "H":
					retVal="Hold until Pri received";
					break;
				case "W":
					retVal="Waiting to Send";
					break;
				case "P":
					retVal="Probably Sent";
					break;
				case "S":
					retVal="Sent - Verified";
					break;
				case "R":
					retVal="Received";
					break;
			}
			return retVal;
		}

		///<summary>Updates ClaimIdentifier for specified claim.</summary>
		public static void UpdateClaimIdentifier(long claimNum,string claimIdentifier) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),claimNum,claimIdentifier);
				return;
			}
			string command="UPDATE claim SET ClaimIdentifier='"+POut.String(claimIdentifier)+"' WHERE ClaimNum="+POut.Long(claimNum);
			Db.NonQ(command);
		}
		
		///<summary>Updates all claimproc estimates and also updates claim totals to db. Must supply procList which includes all procedures that this 
		///claim is linked to.  Will also need to refresh afterwards to see the results</summary>
		public static void CalculateAndUpdate(List<Procedure> procList,List <InsPlan> planList,Claim claimCur,List <PatPlan> patPlans,List <Benefit> benefitList,int patientAge,List<InsSub> subList){
			//No remoting role check; no call to db
			//we need more than just the claimprocs for this claim.
			//in order to run Procedures.ComputeEstimates, we need all claimprocs for all procedures attached to this claim
			List<ClaimProc> ClaimProcsAll=ClaimProcs.Refresh(claimCur.PatNum);
			List<ClaimProc> ClaimProcsForClaim=ClaimProcs.RefreshForClaim(claimCur.ClaimNum);//will be ordered by line number.
			double claimFee=0;
			double dedApplied=0;
			double insPayEst=0;
			double insPayAmt=0;
			double writeoff=0;
			InsPlan plan=InsPlans.GetPlan(claimCur.PlanNum,planList);
			if(plan==null){
				return;
			}
			long patPlanNum=PatPlans.GetPatPlanNum(claimCur.InsSubNum,patPlans);
			//first loop handles totals for received items.
			for(int i=0;i<ClaimProcsForClaim.Count;i++){
				if(ClaimProcsForClaim[i].Status!=ClaimProcStatus.Received){
					continue;//disregard any status except Receieved.
				}
				claimFee+=ClaimProcsForClaim[i].FeeBilled;
				dedApplied+=ClaimProcsForClaim[i].DedApplied;
				insPayEst+=ClaimProcsForClaim[i].InsPayEst;
				insPayAmt+=ClaimProcsForClaim[i].InsPayAmt;
				writeoff+=ClaimProcsForClaim[i].WriteOff;
			}
			//loop again only for procs not received.
			//And for preauth.
			Procedure ProcCur;
			//InsPlan plan=InsPlans.GetPlan(claimCur.PlanNum,planList);
			List<ClaimProcHist> histList=ClaimProcs.GetHistList(claimCur.PatNum,benefitList,patPlans,planList,claimCur.ClaimNum,claimCur.DateService,subList);
			List<ClaimProc> claimProcListOld=new List<ClaimProc>();//make a copy
			for(int i=0;i<ClaimProcsAll.Count;i++) {
				claimProcListOld.Add(ClaimProcsAll[i].Copy());
			}
			List<ClaimProcHist> loopList=new List<ClaimProcHist>();
			for(int i=0;i<ClaimProcsForClaim.Count;i++) {//loop through each proc
				ProcCur=Procedures.GetProcFromList(procList,ClaimProcsForClaim[i].ProcNum);
				//in order for ComputeEstimates to give accurate Writeoff when creating a claim, InsPayEst must be filled for the claimproc with status of NotReceived.
				//So, we must set it here.  We need to set it in the claimProcsAll list.  Find the matching one.
				for(int j=0;j<ClaimProcsAll.Count;j++){
					if(ClaimProcsAll[j].ClaimProcNum==ClaimProcsForClaim[i].ClaimProcNum){//same claimproc in a different list
						if(ClaimProcsForClaim[i].Status==ClaimProcStatus.NotReceived) {//ignores received, etc
							ClaimProcsAll[j].InsPayEst=ClaimProcs.GetInsEstTotal(ClaimProcsAll[j]);
						}
					}
				}
				//When this is the secondary claim, HistList includes the primary estimates, which is something we don't want because the primary calculations gets confused.
				//So, we must remove those bad entries from histList.
				for(int h=histList.Count-1;h>=0;h--) {//loop through the histList backwards
					if(histList[h].ProcNum!=ProcCur.ProcNum) {
						continue;//Makes sure we will only be excluding histList entries for procs on this claim.
					}
					//we already excluded this claimNum when getting the histList.
					if(histList[h].Status!=ClaimProcStatus.NotReceived) {
						continue;//The only ones that are a problem are the ones on the primary claim not received yet.
					}
					histList.RemoveAt(h);
				}
				Procedures.ComputeEstimates(ProcCur,claimCur.PatNum,ref ClaimProcsAll,false,planList,patPlans,benefitList,histList,loopList,false,patientAge
					,subList);
				//then, add this information to loopList so that the next procedure is aware of it.
				//Exclude preauths becase thier estimates would incorrectly add both NotRecieved and Preauth estimates when calculating limitations.
				List<ClaimProc> listClaimProcs=ClaimProcsAll.Where(x => x.ProcNum==ProcCur.ProcNum && x.Status!=ClaimProcStatus.Preauth).ToList();
				loopList.AddRange(ClaimProcs.GetHistForProc(listClaimProcs,ProcCur.ProcNum,ProcCur.CodeNum));
			}
			//save changes in the list to the database
			ClaimProcs.Synch(ref ClaimProcsAll,claimProcListOld);
			ClaimProcsForClaim=ClaimProcs.RefreshForClaim(claimCur.ClaimNum);
			//But ClaimProcsAll has not been refreshed.
			for(int i=0;i<ClaimProcsForClaim.Count;i++) {
				if(ClaimProcsForClaim[i].Status!=ClaimProcStatus.NotReceived
					&& ClaimProcsForClaim[i].Status!=ClaimProcStatus.Preauth
					&& ClaimProcsForClaim[i].Status!=ClaimProcStatus.CapClaim) {
					continue;
				}
				ProcCur=Procedures.GetProcFromList(procList,ClaimProcsForClaim[i].ProcNum);
				if(ProcCur.ProcNum==0) {
					continue;//ignores payments, etc
				}
				//fee:
				int qty=ProcCur.UnitQty + ProcCur.BaseUnits;
				if(qty==0) {
					qty=1;
				}
				if(plan.ClaimsUseUCR) {//use UCR for the provider of the procedure
					long provNum=ProcCur.ProvNum;
					if(provNum==0) {//if no prov set, then use practice default.
						provNum=PrefC.GetLong(PrefName.PracticeDefaultProv);
					}
					Provider providerFirst=Providers.GetFirst();//Used in order to preserve old behavior...  If this fails, then old code would have failed.
					Provider provider=Providers.GetFirstOrDefault(x => x.ProvNum==provNum)??providerFirst;
					//get the fee based on code and prov fee sched
					double ppoFee=Fees.GetAmount0(ProcCur.CodeNum,provider.FeeSched,ProcCur.ClinicNum,provNum);
					double ucrFee=ProcCur.ProcFee;//Usual Customary and Regular (UCR) fee.  Also known as billed fee.
					if(ucrFee > ppoFee) {
						ClaimProcsForClaim[i].FeeBilled=qty*ucrFee;
					}
					else {
						ClaimProcsForClaim[i].FeeBilled=qty*ppoFee;
					}
				}
				//else if(claimCur.ClaimType=="Cap") {//Even for capitation, use the proc fee.
				//	ClaimProcsForClaim[i].FeeBilled=0;
				//}
				else {//don't use ucr.  Use the procedure fee instead.
					ClaimProcsForClaim[i].FeeBilled=qty*ProcCur.ProcFee;
				}
				claimFee+=ClaimProcsForClaim[i].FeeBilled;
				if(claimCur.ClaimType=="PreAuth" || claimCur.ClaimType=="Cap" || (claimCur.ClaimType=="Other" && !plan.IsMedical)) {
					//12-18-2015 ==tg:  We added medical plans as an exclusion to the above logic.  In past versions Medical plans did not copy over values into
					//the claimproc InsPayEst, DedApplied, or Writeoff columns.  DG and I determined that for now this is acceptable.	 If we ever implement a 
					//medical claimtype in the future, or if there are issues with claims this will need to be changed.
					ClaimProcs.Update(ClaimProcsForClaim[i]);//only the fee gets calculated, the rest does not
					continue;
				}
				//ClaimProcs.ComputeBaseEst(ClaimProcsForClaim[i],ProcCur.ProcFee,ProcCur.ToothNum,ProcCur.CodeNum,plan,patPlanNum,benefitList,histList,loopList);
				ClaimProcsForClaim[i].InsPayEst=ClaimProcs.GetInsEstTotal(ClaimProcsForClaim[i]);//Yes, this is duplicated from further up.
				ClaimProcsForClaim[i].DedApplied=ClaimProcs.GetDedEst(ClaimProcsForClaim[i]);
				if(ClaimProcsForClaim[i].Status==ClaimProcStatus.NotReceived){//(vs preauth)
					ClaimProcsForClaim[i].WriteOff=ClaimProcs.GetWriteOffEstimate(ClaimProcsForClaim[i]);
					writeoff+=ClaimProcsForClaim[i].WriteOff;
					/*
					ClaimProcsForClaim[i].WriteOff=0;
					if(claimCur.ClaimType=="P" && plan.PlanType=="p") {//Primary && PPO
						double insplanAllowed=Fees.GetAmount(ProcCur.CodeNum,plan.FeeSched);
						if(insplanAllowed!=-1) {
							ClaimProcsForClaim[i].WriteOff=ProcCur.ProcFee-insplanAllowed;
						}
						//else, if -1 fee not found, then do not show a writeoff. User can change writeoff if they disagree.
					}
					writeoff+=ClaimProcsForClaim[i].WriteOff;*/
				}
				dedApplied+=ClaimProcsForClaim[i].DedApplied;
				insPayEst+=ClaimProcsForClaim[i].InsPayEst;
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA") && procList.Exists(x => x.ProcNumLab==ClaimProcsForClaim[i].ProcNum)) {
					//In Canada we will need to consider lab insurance estimates.
					List<long> listLabProcNums=procList.FindAll(x => x.ProcNumLab==ClaimProcsForClaim[i].ProcNum).Select(x => x.ProcNum).ToList();
					insPayEst+=ClaimProcsAll.FindAll(x => listLabProcNums.Contains(x.ProcNum) 
							&& x.InsSubNum==ClaimProcsForClaim[i].InsSubNum && x.PlanNum==ClaimProcsForClaim[i].PlanNum)
						.Sum(x => ClaimProcs.GetInsEstTotal(x));
				}
				ClaimProcsForClaim[i].ProcDate=ProcCur.ProcDate.Date;//this solves a rare bug. Keeps dates synched.
					//It's rare enough that I'm not goint to add it to the db maint tool.
				ClaimProcs.Update(ClaimProcsForClaim[i]);
				//but notice that the ClaimProcs lists are not refreshed until the loop is finished.
			}//for claimprocs.forclaim
			claimCur.ClaimFee=claimFee;
			claimCur.DedApplied=dedApplied;
			claimCur.InsPayEst=insPayEst;
			claimCur.InsPayAmt=insPayAmt;
			claimCur.WriteOff=writeoff;
			//Cur=ClaimCur;
			Claims.Update(claimCur);
		}

		///<summary>Creates a claim for a newly created repeat charge procedure.</summary>
		public static Claim CreateClaimForRepeatCharge(string claimType,List<PatPlan> patPlanList,List<InsPlan> planList,List<ClaimProc> claimProcList,
			Procedure proc,List<InsSub> subList) {
			//No remoting role check; no call to db
			long claimFormNum=0;
			InsPlan planCur=new InsPlan();
			InsSub subCur=new InsSub();
			Relat relatOther=Relat.Self;
			switch(claimType) {
				case "P":
					subCur=InsSubs.GetSub(PatPlans.GetInsSubNum(patPlanList,PatPlans.GetOrdinal(PriSecMed.Primary,patPlanList,planList,subList)),subList);
					planCur=InsPlans.GetPlan(subCur.PlanNum,planList);
					break;
				case "S":
					subCur=InsSubs.GetSub(PatPlans.GetInsSubNum(patPlanList,PatPlans.GetOrdinal(PriSecMed.Secondary,patPlanList,planList,subList)),subList);
					planCur=InsPlans.GetPlan(subCur.PlanNum,planList);
					break;
				case "Med":
					//It's already been verified that a med plan exists
					subCur=InsSubs.GetSub(PatPlans.GetInsSubNum(patPlanList,PatPlans.GetOrdinal(PriSecMed.Medical,patPlanList,planList,subList)),subList);
					planCur=InsPlans.GetPlan(subCur.PlanNum,planList);
					break;
			}
			ClaimProc claimProcCur=Procedures.GetClaimProcEstimate(proc.ProcNum,claimProcList,planCur,subCur.InsSubNum);
			if(claimProcCur==null) {
				claimProcCur=new ClaimProc();
				ClaimProcs.CreateEst(claimProcCur,proc,planCur,subCur);
			}
			Claim claimCur=new Claim();
			claimCur.PatNum=proc.PatNum;
			claimCur.DateService=proc.ProcDate;
			claimCur.ClinicNum=proc.ClinicNum;
			claimCur.PlaceService=proc.PlaceService;
			claimCur.ClaimStatus="W";
			claimCur.DateSent=DateTime.Today;
			claimCur.DateSentOrig=DateTime.MinValue;
			claimCur.PlanNum=planCur.PlanNum;
			claimCur.InsSubNum=subCur.InsSubNum;
			InsSub sub;
			switch(claimType) {
				case "P":
					claimCur.PatRelat=PatPlans.GetRelat(patPlanList,PatPlans.GetOrdinal(PriSecMed.Primary,patPlanList,planList,subList));
					claimCur.ClaimType="P";
					claimCur.InsSubNum2=PatPlans.GetInsSubNum(patPlanList,PatPlans.GetOrdinal(PriSecMed.Secondary,patPlanList,planList,subList));
					sub=InsSubs.GetSub(claimCur.InsSubNum2,subList);
					if(sub.PlanNum>0 && InsPlans.RefreshOne(sub.PlanNum).IsMedical) {
						claimCur.PlanNum2=0;//no sec ins
						claimCur.PatRelat2=Relat.Self;
					}
					else {
						claimCur.PlanNum2=sub.PlanNum;//might be 0 if no sec ins
						claimCur.PatRelat2=PatPlans.GetRelat(patPlanList,PatPlans.GetOrdinal(PriSecMed.Secondary,patPlanList,planList,subList));
					}
					break;
				case "S":
					claimCur.PatRelat=PatPlans.GetRelat(patPlanList,PatPlans.GetOrdinal(PriSecMed.Secondary,patPlanList,planList,subList));
					claimCur.ClaimType="S";
					claimCur.InsSubNum2=PatPlans.GetInsSubNum(patPlanList,PatPlans.GetOrdinal(PriSecMed.Primary,patPlanList,planList,subList));
					sub=InsSubs.GetSub(claimCur.InsSubNum2,subList);
					claimCur.PlanNum2=sub.PlanNum;
					claimCur.PatRelat2=PatPlans.GetRelat(patPlanList,PatPlans.GetOrdinal(PriSecMed.Primary,patPlanList,planList,subList));
					break;
				case "Med":
					claimCur.PatRelat=PatPlans.GetFromList(patPlanList,subCur.InsSubNum).Relationship;
					claimCur.ClaimType="Other";
					if(PrefC.GetBool(PrefName.ClaimMedTypeIsInstWhenInsPlanIsMedical)) {
						claimCur.MedType=EnumClaimMedType.Institutional;
					}
					else {
						claimCur.MedType=EnumClaimMedType.Medical;
					}
					break;
				case "Other":
					claimCur.PatRelat=relatOther;
					claimCur.ClaimType="Other";
					//plannum2 is not automatically filled in.
					claimCur.ClaimForm=claimFormNum;
					if(planCur.IsMedical) {
						if(PrefC.GetBool(PrefName.ClaimMedTypeIsInstWhenInsPlanIsMedical)) {
							claimCur.MedType=EnumClaimMedType.Institutional;
						}
						else {
							claimCur.MedType=EnumClaimMedType.Medical;
						}
					}
					break;
			}
			if(planCur.PlanType=="c") {//if capitation
				claimCur.ClaimType="Cap";
			}
			claimCur.ProvTreat=proc.ProvNum;
			if(Providers.GetIsSec(proc.ProvNum)) {
				claimCur.ProvTreat=Patients.GetPat(proc.PatNum).PriProv;
				//OK if zero, because auto select first in list when open claim
			}
			claimCur.IsProsthesis="N";
			claimCur.ProvBill=Providers.GetBillingProvNum(claimCur.ProvTreat,claimCur.ClinicNum);//OK if zero, because it will get fixed in claim
			claimCur.EmployRelated=YN.No;
			claimCur.ClaimForm=planCur.ClaimFormNum;
			claimCur.AttachedFlags="Mail";
			Claims.Insert(claimCur);
			//attach procedure
			claimProcCur.ClaimNum=claimCur.ClaimNum;
			if(planCur.PlanType=="c") {//if capitation
				claimProcCur.Status=ClaimProcStatus.CapClaim;
			}
			else {
				claimProcCur.Status=ClaimProcStatus.NotReceived;
			}
			if(planCur.UseAltCode && (ProcedureCodes.GetProcCode(proc.CodeNum).AlternateCode1!="")) {
				claimProcCur.CodeSent=ProcedureCodes.GetProcCode(proc.CodeNum).AlternateCode1;
			}
			else if(planCur.IsMedical && proc.MedicalCode!="") {
				claimProcCur.CodeSent=proc.MedicalCode;
			}
			else {
				claimProcCur.CodeSent=ProcedureCodes.GetProcCode(proc.CodeNum).ProcCode;
				if(claimProcCur.CodeSent.Length>5 && claimProcCur.CodeSent.Substring(0,1)=="D") {
					claimProcCur.CodeSent=claimProcCur.CodeSent.Substring(0,5);
				}
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
					if(claimProcCur.CodeSent.Length>5) {//In Canadian e-claims, codes can contain letters or numbers and cannot be longer than 5 characters.
						claimProcCur.CodeSent=claimProcCur.CodeSent.Substring(0,5);
					}
				}
			}
			claimProcCur.LineNumber=1;
			ClaimProcs.Update(claimProcCur);
			return claimCur;
		}
		
		///<summary>Create claim for the automatic ortho procedure.</summary>
		public static Claim CreateClaimForOrthoProc(string claimType,PatPlan patPlanCur,InsPlan insPlanCur,InsSub inssubCur,
			ClaimProc claimProc,Procedure proc, double feeBilled, DateTime dateBanding, int totalMonths, int monthsRem) {
			//No remoting role check; no call to db
			ClaimProc claimProcCur=Procedures.GetClaimProcEstimate(proc.ProcNum,new List<ClaimProc> { claimProc },insPlanCur,inssubCur.InsSubNum);
			List<PatPlan> listPatPlansForPat = PatPlans.GetPatPlansForPat(patPlanCur.PatNum);
			List<InsPlan> listInsPlansForPat = InsPlans.GetByInsSubs(listPatPlansForPat.Select(x => x.InsSubNum).ToList());
			List<InsSub> listInsSubsForPat = InsSubs.GetMany(listPatPlansForPat.Select(x => x.InsSubNum).ToList());
			if(claimProcCur==null) {
				claimProcCur=new ClaimProc();
				ClaimProcs.CreateEst(claimProcCur,proc,insPlanCur,inssubCur);
			}
			Claim claimCur=new Claim();
			claimCur.PatNum=proc.PatNum;
			claimCur.DateService=proc.ProcDate;
			claimCur.ClinicNum=proc.ClinicNum;
			claimCur.PlaceService=proc.PlaceService;
			claimCur.ClaimStatus="W";
			claimCur.DateSent=DateTime.Today;
			claimCur.DateSentOrig=DateTime.MinValue;
			claimCur.PlanNum=insPlanCur.PlanNum;
			claimCur.InsSubNum=inssubCur.InsSubNum;
			claimCur.ClaimFee=feeBilled;
			if(PrefC.GetBool(PrefName.OrthoClaimMarkAsOrtho)) {
				claimCur.IsOrtho=true;
			}
			if(PrefC.GetBool(PrefName.OrthoClaimUseDatePlacement)) {
				claimCur.OrthoDate=dateBanding;
				claimCur.OrthoTotalM=PIn.Byte(totalMonths.ToString(),false);
				claimCur.OrthoRemainM=PIn.Byte(monthsRem.ToString(),false);
			}
			InsSub sub;
			PatPlan patPlanOther;
			switch(claimType) {
				case "P":
					claimCur.PatRelat=patPlanCur.Relationship;
					claimCur.ClaimType="P";
					patPlanOther=PatPlans.GetPatPlan(patPlanCur.PatNum,PatPlans.GetOrdinal(PriSecMed.Secondary,listPatPlansForPat,listInsPlansForPat,listInsSubsForPat));
					if(patPlanOther==null) {
						claimCur.InsSubNum2=0;
						claimCur.PlanNum2=0;//no sec ins
						claimCur.PatRelat2=Relat.Self;
					}
					else {
						sub=InsSubs.GetOne(patPlanOther.InsSubNum);
						if(sub.PlanNum>0 && !InsPlans.RefreshOne(sub.PlanNum).IsMedical) {
							claimCur.PlanNum2=sub.PlanNum;//might be 0 if no sec ins
							claimCur.PatRelat2=patPlanOther.Relationship;
							claimCur.InsSubNum2=sub.InsSubNum;
						}
					}
					break;
				case "S":
					claimCur.PatRelat=patPlanCur.Relationship;
					claimCur.ClaimType="S";
					patPlanOther=PatPlans.GetPatPlan(patPlanCur.PatNum,PatPlans.GetOrdinal(PriSecMed.Primary,listPatPlansForPat,listInsPlansForPat,listInsSubsForPat));
					if(patPlanOther==null) { //should never happen
						claimCur.InsSubNum2=0;
						claimCur.PlanNum2=0;
						claimCur.PatRelat2=Relat.Self;
					}
					else {
						sub=InsSubs.GetOne(patPlanOther.InsSubNum);
						if(sub.PlanNum>0 && !InsPlans.RefreshOne(sub.PlanNum).IsMedical) {
							claimCur.PlanNum2=sub.PlanNum;
							claimCur.PatRelat2=patPlanOther.Relationship;
							claimCur.InsSubNum2=sub.InsSubNum;
						}
					}
					break;
			}
			if(insPlanCur.PlanType=="c") {//if capitation
				claimCur.ClaimType="Cap";
			}
			claimCur.ProvTreat=proc.ProvNum;
			if(Providers.GetIsSec(proc.ProvNum)) {
				claimCur.ProvTreat=Patients.GetPat(proc.PatNum).PriProv;
				//OK if zero, because auto select first in list when open claim
			}
			claimCur.IsProsthesis="N";
			claimCur.ProvBill=Providers.GetBillingProvNum(claimCur.ProvTreat,claimCur.ClinicNum);//OK if zero, because it will get fixed in claim
			claimCur.EmployRelated=YN.No;
			claimCur.ClaimForm=insPlanCur.ClaimFormNum;
			claimCur.AttachedFlags="Mail";
			Claims.Insert(claimCur);
			claimCur.ClaimIdentifier=Claims.ConvertClaimId(claimCur);
			Claims.Update(claimCur);
			//attach procedure
			claimProcCur.ClaimNum=claimCur.ClaimNum;
			if(insPlanCur.PlanType=="c") {//if capitation
				claimProcCur.Status=ClaimProcStatus.CapClaim;
			}
			else {
				claimProcCur.Status=ClaimProcStatus.NotReceived;
			}
			if(insPlanCur.UseAltCode && (ProcedureCodes.GetProcCode(proc.CodeNum).AlternateCode1!="")) {
				claimProcCur.CodeSent=ProcedureCodes.GetProcCode(proc.CodeNum).AlternateCode1;
			}
			else if(insPlanCur.IsMedical && proc.MedicalCode!="") {
				claimProcCur.CodeSent=proc.MedicalCode;
			}
			else {
				claimProcCur.CodeSent=ProcedureCodes.GetProcCode(proc.CodeNum).ProcCode;
				if(claimProcCur.CodeSent.Length>5 && claimProcCur.CodeSent.Substring(0,1)=="D") {
					claimProcCur.CodeSent=claimProcCur.CodeSent.Substring(0,5);
				}
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
					if(claimProcCur.CodeSent.Length>5) {//In Canadian e-claims, codes can contain letters or numbers and cannot be longer than 5 characters.
						claimProcCur.CodeSent=claimProcCur.CodeSent.Substring(0,5);
					}
				}
			}
			claimProcCur.LineNumber=1;
			claimProcCur.FeeBilled=feeBilled;
			ClaimProcs.Update(claimProcCur);
			return claimCur;
		}

		///<summary>Zeros securitylog FKey column for rows that are using the matching claimNum as FKey and are related to Claim.
		///Permtypes are generated from the AuditPerms property of the CrudTableAttribute within the Claim table type.</summary>
		public static void ClearFkey(long claimNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),claimNum);
				return;
			}
			Crud.ClaimCrud.ClearFkey(claimNum);
		}

		///<summary>Zeros securitylog FKey column for rows that are using the matching claimNums as FKey and are related to Claim.
		///Permtypes are generated from the AuditPerms property of the CrudTableAttribute within the Claim table type.</summary>
		public static void ClearFkey(List<long> listClaimNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listClaimNums);
				return;
			}
			Crud.ClaimCrud.ClearFkey(listClaimNums);
		}

		public static DateTime GetDateLastOrthoClaim(PatPlan patPlanCur,OrthoClaimType claimType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<DateTime>(MethodBase.GetCurrentMethod(),patPlanCur,claimType);
			}
			long orthoDefaultAutoCodeNum=PrefC.GetLong(PrefName.OrthoAutoProcCodeNum);
			string command="";
			if(claimType == OrthoClaimType.InitialPlusPeriodic) {
				command = @"	
				SELECT MAX(claim.DateSent) LastSent
				FROM claim
				INNER JOIN claimproc ON claimproc.ClaimNum = claim.ClaimNum
				INNER JOIN insplan ON claim.PlanNum = insplan.PlanNum
				INNER JOIN procedurelog ON procedurelog.ProcNum = claimproc.ProcNum
					AND procedurelog.CodeNum LIKE 
						IF(insplan.OrthoAutoProcCodeNumOverride = 0, 
						"+orthoDefaultAutoCodeNum+@",
						insplan.OrthoAutoProcCodeNumOverride)
				WHERE claim.ClaimStatus IN ('S','R')
				AND claim.PatNum = "+patPlanCur.PatNum+@"
				AND claim.InsSubNum = "+patPlanCur.InsSubNum;
			}
			else {
				command = @"	
				SELECT MAX(claim.DateSent) LastSent
				FROM claim
				INNER JOIN claimproc ON claimproc.ClaimNum = claim.ClaimNum
				INNER JOIN insplan ON claim.PlanNum = insplan.PlanNum
				INNER JOIN procedurelog ON procedurelog.ProcNum = claimproc.ProcNum
				INNER JOIN procedurecode ON procedurecode.CodeNum = procedurelog.CodeNum
				INNER JOIN covspan ON covspan.FromCode <= procedurecode.ProcCode AND covspan.ToCode >= procedurecode.ProcCode
				INNER JOIN covcat ON covcat.CovCatNum = covspan.CovCatNum
					AND covcat.EbenefitCat = "+POut.Int((int)EbenefitCategory.Orthodontics)+@"
				WHERE claim.ClaimStatus IN ('S','R')
				AND claim.PatNum = "+patPlanCur.PatNum+@"
				AND claim.InsSubNum = "+patPlanCur.InsSubNum;
			}
			return PIn.Date(Db.GetScalar(command));
		}

		public static List<Claim> GetForPat(long patNum) {
			return Crud.ClaimCrud.SelectMany("SELECT * FROM claim WHERE PatNum = "+patNum);
		}

		///<summary>Gets the most recent ortho claim with a banding code attached.
		///Returns null if no ortho banding code nums found or no corresponding claim found.</summary>
		public static Claim GetOrthoBandingClaim(long patNum,long planNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Claim>(MethodBase.GetCurrentMethod(),patNum,planNum);
			}
			List<long> listProcCodeNums=ProcedureCodes.GetOrthoBandingCodeNums();
			if(listProcCodeNums==null || listProcCodeNums.Count < 1) {
				return null;
			}
			string command = @"
				SELECT claim.* 
				FROM claim
				WHERE claim.PatNum = "+POut.Long(patNum)+@"
				AND claim.PlanNum = "+POut.Long(planNum)+@"
				AND claim.IsOrtho = 1
				AND claim.ClaimStatus = 'R'
				AND EXISTS(
					SELECT * FROM claimproc
					INNER JOIN procedurelog ON claimproc.ProcNum = procedurelog.ProcNum
					INNER JOIN procedurecode ON procedurecode.CodeNum = procedurelog.CodeNum
						AND procedurecode.CodeNum IN ("+String.Join(",",listProcCodeNums)+@")
					WHERE claimproc.ClaimNum = claim.ClaimNum
				)
				ORDER BY claim.DateSent DESC";
			return Crud.ClaimCrud.SelectOne(command);
		}

		///<summary>Returns the defalt/calculated claim ID based on the ClaimIdPrefix preference.</summary>
		public static string ConvertClaimId(Claim claim,Patient pat=null) {
			if(pat==null) {
				pat=Patients.GetPat(claim.PatNum);
			}
			return Patients.ReplacePatient(PrefC.GetString(PrefName.ClaimIdPrefix),pat)+claim.ClaimNum;
		}

	}//end class Claims

	///<summary>This is an odd class.  It holds data for the X12 (4010 only) generation process.  It replaces an older multi-dimensional array, so the names are funny, but helpful to prevent bugs.  Not an actual database table.</summary>
	public class X12TransactionItem{
		public string PayorId0;
		public long ProvBill1;
		public long Subscriber2;
		public long PatNum3;
		public long ClaimNum4;
	}

	///<summary>Holds a list of claims to show in the claims 'queue' waiting to be sent.  Not an actual database table.</summary>
	public class ClaimSendQueueItem{
		///<summary></summary>
		public long ClaimNum;
		///<summary></summary>
		public bool NoSendElect;
		///<summary></summary>
		public string PatName;
		///<summary>Single char: U,H,W,P,S,or R.</summary>
		///<remarks>U=Unsent, H=Hold until pri received, W=Waiting in queue, P=Probably sent, S=Sent, R=Received.  A(adj) is no longer used.</remarks>
		public string ClaimStatus;
		///<summary></summary>
		public string Carrier;
		///<summary></summary>
		public long PatNum;
		///<summary>ClearinghouseNum of HQ.</summary>
		public long ClearinghouseNum;
		///<summary></summary>
		public long ClinicNum;
		///<summary>Enum:EnumClaimMedType 0=Dental, 1=Medical, 2=Institutional</summary>
		public EnumClaimMedType MedType;
		///<summary></summary>
		public string MissingData;
		///<summary></summary>
		public string Warnings;
		///<summary></summary>
		public DateTime DateService;
		///<summary>False by default.  For speed purposes, claims should only be validated once, which is just before they are sent.</summary>
		public bool IsValid;
		/// <summary>Used to save what tracking is used for filtering.</summary>
		public long CustomTracking;
		///<summary>Claim has procedures with IcdVersion=9 and at least one Diagnostic.</summary>
		public bool HasIcd9;

		public ClaimSendQueueItem Copy(){
			return (ClaimSendQueueItem)MemberwiseClone();
		}
	}

	///<summary>Holds a list of claims to show in the Claim Pay Edit window.  Not an actual database table.</summary>
	public class ClaimPaySplit{
		///<summary></summary>
		public long ClaimNum;
		///<summary></summary>
		public string PatName;
		///<summary></summary>
		public long PatNum;
		///<summary></summary>
		public string Carrier;
		///<summary></summary>
		public DateTime DateClaim;
		///<summary></summary>
		public string ProvAbbr;
		///<summary></summary>
		public double FeeBilled;
		///<summary></summary>
		public double InsPayAmt;
		///<summary></summary>
		public long ClaimPaymentNum;
		///<summary>1-based</summary>
		public int PaymentRow;
		///<summary></summary>
		public string ClinicDesc;
		///<summary></summary>
		public string ClaimStatus;
	}

	///<summary>Different types of filters for the Claims Not Sent report.</summary>
	public enum ClaimNotSentStatuses {
		///<summary>0</summary>
		All,
		///<summary>1</summary>
		Primary,
		///<summary>2</summary>
		Secondary,
		///<summary>3</summary>
		Holding
	}

}