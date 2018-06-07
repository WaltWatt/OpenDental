using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness {
	///<summary></summary>
	public class RepeatCharges {
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

		///<summary>Gets a list of all RepeatCharges for a given patient.  Supply 0 to get a list for all patients.</summary>
		public static RepeatCharge[] Refresh(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<RepeatCharge[]>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM repeatcharge";
			if(patNum!=0) {
				command+=" WHERE PatNum = "+POut.Long(patNum);
			}
			command+=" ORDER BY DateStart";
			return Crud.RepeatChargeCrud.SelectMany(command).ToArray();
		}	

		///<summary></summary>
		public static void Update(RepeatCharge charge){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),charge);
				return;
			}
			Crud.RepeatChargeCrud.Update(charge);
		}

		public static void UpdateChargeAmt(long repeatChargeNum,double chargeAmt) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),repeatChargeNum,chargeAmt);
				return;
			}
			string command="UPDATE repeatcharge SET ChargeAmt="+POut.Double(chargeAmt)+" "
				+"WHERE RepeatChargeNum="+POut.Long(repeatChargeNum);
			Db.NonQ(command);
		}

		///<summary>Inserts a new RepeatCharge if none found for patNum/procCode. Otherwise updates existing.
		///If dateStart is not explicitly provided, then it will default to DateTime.Today.
		///If RepeatCharge already exists then chargeAmt, dateStart, createsClaim, copyNoteToProc inputs will be ignored and RepeatCharge will simply be re-enabled.
		///Use RepeatCharges.Update() if you want to specify new input values.</summary>
		public static void EnableRepeatCharge(long patNum,float chargeAmt,string procCode,DateTime dateStart=new DateTime(),bool createsClaim=false,bool copyNoteToProc=false) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patNum,chargeAmt,procCode,dateStart,createsClaim,copyNoteToProc);
				return;
			}
			if(dateStart.Year<1880) { //Default to today if none provided.
				dateStart=DateTime.Today;
			}
			RepeatCharge[] repeateCharges=RepeatCharges.Refresh(patNum);
			RepeatCharge repeatCharge=null;
			if(repeateCharges!=null) {
				repeatCharge=repeateCharges.FirstOrDefault(x => x.ProcCode==procCode);
			}
			if(repeatCharge==null) {
				repeatCharge=new RepeatCharge();
				repeatCharge.ProcCode=procCode;
				repeatCharge.PatNum=patNum;
				repeatCharge.IsEnabled=true;
				repeatCharge.DateStart=dateStart; 
				repeatCharge.CreatesClaim=createsClaim;
				repeatCharge.CopyNoteToProc=copyNoteToProc;
				repeatCharge.ChargeAmt=chargeAmt;
				repeatCharge.RepeatChargeNum=RepeatCharges.Insert(repeatCharge);
			}
			repeatCharge.IsEnabled=true;
			repeatCharge.DateStop=System.DateTime.MinValue;
			RepeatCharges.Update(repeatCharge);
		}

		///<summary></summary>
		public static long Insert(RepeatCharge charge) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				charge.RepeatChargeNum=Meth.GetLong(MethodBase.GetCurrentMethod(),charge);
				return charge.RepeatChargeNum;
			}
			return Crud.RepeatChargeCrud.Insert(charge);
		}

		///<summary>Called from FormRepeatCharge.</summary>
		public static void Delete(RepeatCharge charge){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),charge);
				return;
			}
			string command="DELETE FROM repeatcharge WHERE RepeatChargeNum ="+POut.Long(charge.RepeatChargeNum);
			Db.NonQ(command);
		}

		///<summary>For internal use only.  Returns all eRx repeating charges for all customers.</summary>
		public static List<RepeatCharge> GetForErx() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<RepeatCharge>>(MethodBase.GetCurrentMethod());
			}
			//Does not need to be Oracle compatible because this is an internal tool only.
			string command="SELECT * FROM repeatcharge WHERE ProcCode REGEXP '^Z[0-9]{3,}$'";
			return Crud.RepeatChargeCrud.SelectMany(command);
		}

		///<summary>Get the list of all RepeatCharge rows. DO NOT REMOVE! Used by OD WebApps solution.</summary>
		// ReSharper disable once UnusedMember.Global
		public static List<RepeatCharge> GetAll() {
			//No need to check RemotingRole; no call to db.
			return Refresh(0).ToList();			
		}

		///<summary>Get all repeat charges for guarantor. This is used by the Reseller Portal to get all repeat linked to the given reseller guarantor.</summary>
		public static List<RepeatCharge> GetFamilyGuarantor(long guarantor) {
			string command="SELECT rc.* FROM repeatcharge rc INNER JOIN patient p ON p.PatNum=rc.PatNum WHERE p.Guarantor="+POut.Long(guarantor);
			return Crud.RepeatChargeCrud.SelectMany(command);
		}

		///<summary>Returns true if there are any active repeating charges on the patient's account, false if there are not.</summary>
		public static bool ActiveRepeatChargeExists(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),patNum);
			}
			//Counts the number of repeat charges that a patient has with a valid start date in the past and no stop date or a stop date in the future
			string command="SELECT COUNT(*) FROM repeatcharge "
				+"WHERE PatNum="+POut.Long(patNum)+" AND DateStart BETWEEN '1880-01-01' AND "+DbHelper.Curdate()+" "
				+"AND (DateStop='0001-01-01' OR DateStop>="+DbHelper.Curdate()+")";
			if(Db.GetCount(command)=="0") {
				return false;
			}
			return true;
		}

		///<summary>Returns true if the dates passed in from the corresponding repeat charge are active as of DateTime.Today.
		///Mimics the logic within ActiveRepeatChargeExists() in the sense that dateStart is a valid date and is before or on DateTime.Today 
		/// and that dateStop is either an invalid date (has yet to be set) OR is a valid date that is in the future or on DateTime.Today.</summary>
		public static bool IsRepeatChargeActive(DateTime dateStart,DateTime dateStop) {
			if((dateStart.Year > 1880 && dateStart<=DateTime.Today)
				&& (dateStop.Year < 1880 || dateStop>=DateTime.Today)) 
			{
				return true;
			}
			return false;
		}
		
		/// <summary>Runs repeating charges for the date passed in, usually today. Can't use 'out' variables because this runs over Middle Tier.</summary>
		public static RepeatChargeResult RunRepeatingCharges(DateTime dateRun) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<RepeatChargeResult>(MethodBase.GetCurrentMethod(),dateRun);
			}
			Prefs.UpdateDateT(PrefName.RepeatingChargesBeginDateTime,dateRun);
			try {
				RepeatChargeResult result=new RepeatChargeResult();
				List<RepeatCharge> listRepeatingCharges=RepeatCharges.Refresh(0).ToList();
				if(PrefC.IsODHQ) {
					//Remove all eService repeating charges.
					//EService charges have already been calculated and stored in EServiceBilling table. Add those here.
					List<string> listEServiceCodes=EServiceCodeLink.GetProcCodesForAll();
					listRepeatingCharges.RemoveAll(x => listEServiceCodes.Contains(x.ProcCode));
					result.ProceduresAddedCount+=EServiceBillings.AddEServiceRepeatingChargesHelper(dateRun).Count;
				}
				//Must contain all procedures that affect the date range, safe to contain too many, bad to contain too few.
				List<Procedure> listExistingProcs=Procedures.GetCompletedForDateRange(dateRun.AddMonths(-3),dateRun.AddDays(1),
					listRepeatingCharges.Select(x => x.ProcCode).Distinct().Select(x => ProcedureCodes.GetProcCode(x).CodeNum).ToList());
				DateTime startedUsingFKs=UpdateHistories.GetDateForVersion(new Version("16.1.0.0"));//We started using FKs from procs to repeat charges in 16.1.
				foreach(RepeatCharge repeatCharge in listRepeatingCharges) {
					if(!repeatCharge.IsEnabled || (repeatCharge.DateStop.Year > 1880 && repeatCharge.DateStop.AddMonths(3) < dateRun)) {
						continue;//This repeating charge is too old to possibly create a new charge. Not precise but greatly reduces calls to DB.
										 //We will filter by more stringently on the DateStop later on.
					}
					Patient pat=null;
					List<DateTime> listBillingDates;//This list will have 1 or 2 dates where a repeating charge might be added
					if(PrefC.GetBool(PrefName.BillingUseBillingCycleDay)) {
						pat=Patients.GetPat(repeatCharge.PatNum);
						listBillingDates=GetBillingDatesHelper(repeatCharge.DateStart,repeatCharge.DateStop,dateRun,pat.BillingCycleDay);
					}
					else {
						listBillingDates=GetBillingDatesHelper(repeatCharge.DateStart,repeatCharge.DateStop,dateRun);
					}
					long codeNum=ProcedureCodes.GetCodeNum(repeatCharge.ProcCode);
					//Remove billing dates if there is a procedure from this repeat charge in that month and year
					for(int i=listBillingDates.Count-1;i>=0;i--) {//iterate backwards to remove elements
						DateTime billingDate=listBillingDates[i];
						for(int j=listExistingProcs.Count-1;j>=0;j--) {//iterate backwards to remove elements
							Procedure proc=listExistingProcs[j];
							if((proc.RepeatChargeNum==repeatCharge.RepeatChargeNum //Check the procedure's FK first
								&& IsRepeatDateHelper(repeatCharge,billingDate,proc.ProcDate,pat))
								//Use the old logic without matching FKs only if the procedure was added before updating to 16.1
								//Match patnum, codenum, fee, year, and month (IsRepeatDateHelper uses special logic to determine correct month)
								|| ((proc.ProcDate<startedUsingFKs || startedUsingFKs.Year<1880)
								&& proc.PatNum==repeatCharge.PatNum
								&& proc.CodeNum==codeNum
								&& IsRepeatDateHelper(repeatCharge,billingDate,proc.ProcDate,pat)
								&& proc.ProcFee.IsEqual(repeatCharge.ChargeAmt))) 
							{
								//This is a match to an existing procedure.
								listBillingDates.RemoveAt(i);//Removing so that a procedure will not get added on this date.
								listExistingProcs.RemoveAt(j);//Removing so that another repeat charge of the same code, date, and amount will be added.
								break;//Go to the next billing date
							}
						}
					}
					//If any billing dates have not been filtered out, add a repeating charge on those dates
					foreach(DateTime billingDate in listBillingDates) {
						Procedure procAdded=AddRepeatingChargeHelper(repeatCharge,billingDate,dateRun);
						List<Claim> listClaimsAdded=new List<Claim>();
						if(repeatCharge.CreatesClaim && !ProcedureCodes.GetProcCode(repeatCharge.ProcCode).NoBillIns) {
							listClaimsAdded=AddClaimsHelper(repeatCharge,procAdded);
						}
						result.ProceduresAddedCount++;
						result.ClaimsAddedCount+=listClaimsAdded.Count;
					}
				}
				return result;
			}
			finally {
				Prefs.UpdateString(PrefName.RepeatingChargesBeginDateTime,"");
			}
		}

		///<summary>Do not call this until after determining if the repeate charge might generate a claim.  This function checks current insurance and 
		///may not add claims if no insurance is found.</summary>
		private static List<Claim> AddClaimsHelper(RepeatCharge repeateCharge,Procedure proc) {
			//No remoting role check; no call to db
			List<PatPlan> patPlanList=PatPlans.Refresh(repeateCharge.PatNum);
			List<InsSub> subList=InsSubs.RefreshForFam(Patients.GetFamily(repeateCharge.PatNum));
			List<InsPlan> insPlanList=InsPlans.RefreshForSubList(subList);
			List<Benefit> benefitList=Benefits.Refresh(patPlanList,subList);
			List<Claim> retVal=new List<Claim>();
			Claim claimCur;
			if(patPlanList.Count==0) {//no current insurance, do not create a claim
				return retVal;
			}
			//create the claimprocs
			Procedures.ComputeEstimates(proc,proc.PatNum,new List<ClaimProc>(),true,insPlanList,patPlanList,benefitList,
				Patients.GetPat(proc.PatNum).Age,subList);
			//get claimprocs for this proc, may be more than one
			List<ClaimProc> claimProcList=ClaimProcs.GetForProc(ClaimProcs.Refresh(proc.PatNum),proc.ProcNum);
			string claimType="P";
			if(patPlanList.Count==1 && PatPlans.GetOrdinal(PriSecMed.Medical,patPlanList,insPlanList,subList)>0) {//if there's exactly one medical plan
				claimType="Med";
			}
			claimCur=Claims.CreateClaimForRepeatCharge(claimType,patPlanList,insPlanList,claimProcList,proc,subList);
			claimProcList=ClaimProcs.Refresh(proc.PatNum);
			if(claimCur.ClaimNum==0) {
				return retVal;
			}
			retVal.Add(claimCur);
			Claims.CalculateAndUpdate(new List<Procedure> { proc },insPlanList,claimCur,patPlanList,benefitList,Patients.GetPat(proc.PatNum).Age,subList);
			if(PatPlans.GetOrdinal(PriSecMed.Secondary,patPlanList,insPlanList,subList)>0 //if there exists a secondary plan
				 && !CultureInfo.CurrentCulture.Name.EndsWith("CA")) //and not canada (don't create secondary claim for canada)
			{
				claimCur=Claims.CreateClaimForRepeatCharge("S",patPlanList,insPlanList,claimProcList,proc,subList);
				if(claimCur.ClaimNum==0) {
					return retVal;
				}
				retVal.Add(claimCur);
				ClaimProcs.Refresh(proc.PatNum);
				claimCur.ClaimStatus="H";
				Claims.CalculateAndUpdate(new List<Procedure> { proc },insPlanList,claimCur,patPlanList,benefitList,Patients.GetPat(proc.PatNum).Age,subList);
			}
			return retVal;
		}

		///<summary>Returns 1 or 2 dates to be billed given the date range. Only filtering based on date range has been performed.</summary>
		private static List<DateTime> GetBillingDatesHelper(DateTime dateStart,DateTime dateStop,DateTime dateRun,int billingCycleDay=0) {
			//No remoting role check; no call to db
			List<DateTime> retVal=new List<DateTime>();
			if(!PrefC.GetBool(PrefName.BillingUseBillingCycleDay)) {
				billingCycleDay=dateStart.Day;
			}
			//Add dates on the first of each of the last three months
			retVal.Add(new DateTime(dateRun.AddMonths(-0).Year,dateRun.AddMonths(-0).Month,1));//current month -0
			retVal.Add(new DateTime(dateRun.AddMonths(-1).Year,dateRun.AddMonths(-1).Month,1));//current month -1
			retVal.Add(new DateTime(dateRun.AddMonths(-2).Year,dateRun.AddMonths(-2).Month,1));//current month -2
			//This loop fixes day of month, taking into account billing day past the end of the month.
			for(int i=0;i<retVal.Count;i++) {
				int billingDay=Math.Min(retVal[i].AddMonths(1).AddDays(-1).Day, billingCycleDay);
				retVal[i]=new DateTime(retVal[i].Year,retVal[i].Month,billingDay);//This re-adds the billing date with the proper day of month.
			}
			//Remove billing dates that are calulated before repeat charge started.
			retVal.RemoveAll(x => x < dateStart);
			//Remove billing dates older than one month and 20 days ago.
			retVal.RemoveAll(x => x < dateRun.AddMonths(-1).AddDays(-20));
			//Remove any dates after today
			retVal.RemoveAll(x => x > dateRun);
			//Remove billing dates past the end of the dateStop
			int monthAdd=0;
			//To account for a partial month, add a charge after the repeat charge stop date in certain circumstances (for each of these scenarios, the 
			//billingCycleDay will be 11):
			//--Scenario #1: The start day is before the stop day which is before the billing day. Ex: Start: 12/08, Stop 12/09
			//--Scenario #2: The start day is after the billing day which is after the stop day. Ex: Start: 11/25 Stop 12/01
			//--Scenario #3: The start day is before the stop day but before the billing day. Ex: Start: 11/25, Stop 11/27
			//--Scenario #4: The start day is the same as the stop day but after the billing day. Ex: Start: 10/13, Stop 11/13
			//--Scenario #5: The start day is the same as the stop day but before the billing day. Ex: Start: 11/10, Stop 12/10
			//Each of these repeat charges will post a charge on 12/11 even though it is after the stop date.
			if(PrefC.GetBool(PrefName.BillingUseBillingCycleDay)) {
				if(dateStart.Day<billingCycleDay) {
					if((dateStop.Day < billingCycleDay && dateStart.Day < dateStop.Day)//Scenario #1
						|| dateStart.Day==dateStop.Day)//Scenario #5
					{
						monthAdd=1;
					}
				}
				else if(dateStart.Day>billingCycleDay) {
					if(dateStart.Day <= dateStop.Day//Scenario #3 and #4
						|| dateStop.Day < billingCycleDay)//Scenario #2
					{
						monthAdd=1;
					}
				}
			}
			if(dateStop.Year>1880) {
				retVal.RemoveAll(x => x > dateStop.AddMonths(monthAdd));
			}
			retVal.Sort();//Order by oldest first
			return retVal;
		}

		private static Procedure AddRepeatingChargeHelper(RepeatCharge repeatCharge,DateTime billingDate,DateTime dateNow) {
			//No remoting role check; no call to db
			Procedure procedure=new Procedure();
			ProcedureCode procCode=ProcedureCodes.GetProcCode(repeatCharge.ProcCode);
			Patient pat=Patients.GetPat(repeatCharge.PatNum);
			procedure.CodeNum=procCode.CodeNum;
			procedure.ClinicNum=pat.ClinicNum;
			procedure.DateEntryC=dateNow;
			procedure.PatNum=repeatCharge.PatNum;
			procedure.ProcDate=billingDate;
			procedure.DateTP=billingDate;
			procedure.ProcFee=repeatCharge.ChargeAmt;
			procedure.ProcStatus=ProcStat.C;
			if(procCode.ProvNumDefault==0) {
				procedure.ProvNum=pat.PriProv;
			}
			else {
				procedure.ProvNum=procCode.ProvNumDefault;
			}
			procedure.MedicalCode=ProcedureCodes.GetProcCode(procedure.CodeNum).MedicalCode;
			procedure.BaseUnits=ProcedureCodes.GetProcCode(procedure.CodeNum).BaseUnits;
			procedure.DiagnosticCode=PrefC.GetString(PrefName.ICD9DefaultForNewProcs);
			procedure.RepeatChargeNum=repeatCharge.RepeatChargeNum;
			procedure.PlaceService=(PlaceOfService)PrefC.GetInt(PrefName.DefaultProcedurePlaceService);//Default Proc Place of Service for the Practice is used.  
			//Check if the repeating charge has been flagged to copy it's note into the billing note of the procedure.
			if(repeatCharge.CopyNoteToProc) {
				procedure.BillingNote=repeatCharge.Note;
				if(repeatCharge.ErxAccountId!="") {
					procedure.BillingNote=
						"NPI="+repeatCharge.Npi+"  "+"ErxAccountId="+repeatCharge.ErxAccountId;
					if(!string.IsNullOrEmpty(repeatCharge.ProviderName)) {//Provider name would be empty if older and no longer updated from eRx.
						procedure.BillingNote+="\r\nProviderName="+repeatCharge.ProviderName;
					}
					if(!string.IsNullOrEmpty(repeatCharge.Note)) {
						procedure.BillingNote+="\r\n"+repeatCharge.Note;
					}
				}
			}
			if(!PrefC.GetBool(PrefName.EasyHidePublicHealth)) {
				procedure.SiteNum=pat.SiteNum;
			}
			Procedures.Insert(procedure); //no recall synch needed because dental offices don't use this feature
			//Using Prepayments for this Procedure
			if(repeatCharge.UsePrepay) {
				//NOTE: ProvNum=0 on these splits, so I'm pretty sure they aren't allocated to anything.
				List<PaySplit> prePaySplits=PaySplits.GetPrepayForFam(Patients.GetFamily(repeatCharge.PatNum));
				List<PaySplit> paySplitsForPrePaySplits=PaySplits.GetSplitsForPrepay(prePaySplits);
				Payment payCur=new Payment();
				payCur.ClinicNum=procedure.ClinicNum;
				payCur.DateEntry=billingDate;
				payCur.IsSplit=true;
				payCur.PatNum=repeatCharge.PatNum;
				payCur.PayDate=billingDate;
				payCur.PayType=0;//Income transfer (will always be income transfer)
				payCur.PayAmt=0;//Income transfer payment
				payCur.PayNum=Payments.Insert(payCur);
				decimal payAmt=0;
				string noteText="";
				foreach(PaySplit prePaySplit in prePaySplits) {
					prePaySplit.SplitAmt+=paySplitsForPrePaySplits.Where(x => x.FSplitNum==prePaySplit.SplitNum).Sum(y => y.SplitAmt);//Reduce prepay split amount.
					PaySplit split=new PaySplit();
					PaySplit split2=new PaySplit();
					if(prePaySplit.SplitAmt>procedure.ProcFee-(double)payAmt) {
						//Split amount is more than the remainder of the procfee requires, use partial from split
						split.SplitAmt=procedure.ProcFee-(double)payAmt;
						split2.SplitAmt=0-(procedure.ProcFee-(double)payAmt);
						payAmt=(decimal)procedure.ProcFee;
					}
					else {
						//Split amount is less than or equal to the remainder of the procfee
						split.SplitAmt=prePaySplit.SplitAmt;
						split2.SplitAmt=0-prePaySplit.SplitAmt;
						payAmt+=(decimal)prePaySplit.SplitAmt;
					}
					if(split.SplitAmt==0) {
						continue;//Don't make splits for 0 amount.
					}
					//Positive split, attached to proc and for proc's prov and clinic
					split.DateEntry=billingDate;
					split.DatePay=billingDate;
					split.PatNum=procedure.PatNum;
					split.PayNum=payCur.PayNum;
					split.ProcNum=procedure.ProcNum;
					split.ProvNum=procedure.ProvNum;
					split.ClinicNum=procedure.ClinicNum;
					if(noteText!="") {
						noteText+=", ";
					}
					noteText+=split.SplitAmt.ToString("c");
					PaySplits.Insert(split);
					//Negative split, attached to prepay's prov and clinic, but not proc
					split2.DateEntry=billingDate;
					split2.DatePay=billingDate;
					split2.PatNum=procedure.PatNum;
					split2.PayNum=payCur.PayNum;
					split2.FSplitNum=prePaySplit.SplitNum;
					split2.ProvNum=prePaySplit.ProvNum;
					split2.ClinicNum=prePaySplit.ClinicNum;
					PaySplits.Insert(split2);
					if(payAmt>=(decimal)procedure.ProcFee) {
						//Break out of loop
						break;
					}
				}
				payCur.PayNote="Allocated "+noteText+" prepayments to repeating charge.";
				Payments.Update(payCur,false);
			}
			return procedure;
		}

		///<summary>Should only be called if ODHQ.</summary>
		private static List<Procedure> AddSmsRepeatingChargesHelper(DateTime dateRun) {
			//No remoting role check; no call to db
			DateTime dateStart=new DateTime(dateRun.AddMonths(-1).AddDays(-20).Year,dateRun.AddMonths(-1).AddDays(-20).Month,1);
			DateTime dateStop=dateRun.AddDays(1);
			List<SmsBilling> listSmsBilling=SmsBillings.GetByDateRange(dateStart,dateStop);
			List<Patient> listPatients=Patients.GetMultPats(listSmsBilling.Select(x => x.CustPatNum).Distinct().ToList()).ToList(); //local cache
			ProcedureCode procCodeAccess=ProcedureCodes.GetProcCode("038");
			ProcedureCode procCodeUsage=ProcedureCodes.GetProcCode("039");
			ProcedureCode procCodeConfirm=ProcedureCodes.GetProcCode("040");
			List<Procedure> listProcsAccess=Procedures.GetCompletedForDateRange(dateStart,dateStop,new List<long> {procCodeAccess.CodeNum});
			List<Procedure> listProcsUsage=Procedures.GetCompletedForDateRange(dateStart,dateStop,new List<long> {procCodeUsage.CodeNum});
			List<Procedure> listProcsConfirm=Procedures.GetCompletedForDateRange(dateStart,dateStop,new List<long> {procCodeConfirm.CodeNum});
			List<Procedure> retVal=new List<Procedure>();
			foreach(SmsBilling smsBilling in listSmsBilling) {
				Patient pat=listPatients.FirstOrDefault(x => x.PatNum==smsBilling.CustPatNum);
				if(pat==null) {
					EServiceSignal eSignal=new EServiceSignal {
						ServiceCode=(int)eServiceCode.IntegratedTexting,
						SigDateTime=MiscData.GetNowDateTime(),
						Severity=eServiceSignalSeverity.Error,
						Description="Sms billing row found for non existent patient PatNum:"+smsBilling.CustPatNum
					};
					EServiceSignals.Insert(eSignal);
					continue;
				}
				//Find the billing date based on the date usage.
				DateTime billingDate=smsBilling.DateUsage.AddMonths(1);//we always bill the month after usage posts. Example: all January usage = 01/01/2015
				billingDate=new DateTime(
					billingDate.Year,
					billingDate.Month,
					Math.Min(pat.BillingCycleDay,DateTime.DaysInMonth(billingDate.Year,billingDate.Month)));
				//example: dateUsage=08/01/2015, billing cycle date=8/14/2012, billing date should be 9/14/2015.
				if(billingDate>dateRun || billingDate<dateRun.AddMonths(-1).AddDays(-20)) {
					//One month and 20 day window. Bill regardless of presence of "038" repeat charge.
					continue;
				}
				if(smsBilling.AccessChargeTotalUSD==0 && smsBilling.MsgChargeTotalUSD==0 &&smsBilling.ConfirmationChargeTotalUSD==0) {
					//No charges so skip this customer.
					continue;
				}
				//Only post confirmation charge if valid.
				if(smsBilling.ConfirmationChargeTotalUSD>0
					&& (billingDate <= DateTime.Today || PrefC.GetBool(PrefName.FutureTransDatesAllowed)
					&& !listProcsConfirm.Exists(x => x.PatNum==pat.PatNum && x.ProcDate.Year==billingDate.Year && x.ProcDate.Month==billingDate.Month)))
				{
					//The calculated access charge was greater than 0 and there is not an existing "038" procedure on the account for that month.
					Procedure procConfirm=new Procedure();
					procConfirm.CodeNum=procCodeConfirm.CodeNum;
					procConfirm.DateEntryC=DateTime.Today;
					procConfirm.PatNum=pat.PatNum;
					procConfirm.ProcDate=billingDate;
					procConfirm.DateTP=billingDate;
					procConfirm.ProcFee=smsBilling.ConfirmationChargeTotalUSD;
					procConfirm.ProcStatus=ProcStat.C;
					procConfirm.ProvNum=PrefC.GetLong(PrefName.PracticeDefaultProv);
					procConfirm.MedicalCode=procCodeConfirm.MedicalCode;
					procConfirm.BaseUnits=procCodeConfirm.BaseUnits;
					procConfirm.DiagnosticCode=PrefC.GetString(PrefName.ICD9DefaultForNewProcs);
					procConfirm.BillingNote=smsBilling.BillingDescConfirmation;
					procConfirm.PlaceService=(PlaceOfService)PrefC.GetInt(PrefName.DefaultProcedurePlaceService);//Default Proc Place of Service for the Practice is used.
					Procedures.Insert(procConfirm);
					listProcsConfirm.Add(procConfirm);
					retVal.Add(procConfirm);
				}
				//Confirmation charges may wipe out access charges. We still want to see the $0 charge in this case so post this charge if either of the 2 are valid.
				if((smsBilling.AccessChargeTotalUSD>0 || smsBilling.ConfirmationChargeTotalUSD>0)
					&& (billingDate <= DateTime.Today || PrefC.GetBool(PrefName.FutureTransDatesAllowed)) 
					&& !listProcsAccess.Exists(x => x.PatNum==pat.PatNum && x.ProcDate.Year==billingDate.Year && x.ProcDate.Month==billingDate.Month))
				{
					//The calculated access charge was greater than 0 and there is not an existing "038" procedure on the account for that month.
					Procedure procAccess=new Procedure();
					procAccess.CodeNum=procCodeAccess.CodeNum;
					procAccess.DateEntryC=DateTime.Today;
					procAccess.PatNum=pat.PatNum;
					procAccess.ProcDate=billingDate;
					procAccess.DateTP=billingDate;
					procAccess.ProcFee=smsBilling.AccessChargeTotalUSD;
					procAccess.ProcStatus=ProcStat.C;
					procAccess.ProvNum=PrefC.GetLong(PrefName.PracticeDefaultProv);
					procAccess.MedicalCode=procCodeAccess.MedicalCode;
					procAccess.BaseUnits=procCodeAccess.BaseUnits;
					procAccess.DiagnosticCode=PrefC.GetString(PrefName.ICD9DefaultForNewProcs);
					procAccess.BillingNote=smsBilling.BillingDescSms;
					procAccess.PlaceService=(PlaceOfService)PrefC.GetInt(PrefName.DefaultProcedurePlaceService);//Default Proc Place of Service for the Practice is used. 
					Procedures.Insert(procAccess);
					listProcsAccess.Add(procAccess);
					retVal.Add(procAccess);
				}
				//Only post usage charge if valid.
				if(smsBilling.MsgChargeTotalUSD>0
					&& (billingDate <= DateTime.Today || PrefC.GetBool(PrefName.FutureTransDatesAllowed))
					&& !listProcsUsage.Exists(x => x.PatNum==pat.PatNum && x.ProcDate.Year==billingDate.Year && x.ProcDate.Month==billingDate.Month))
				{
					//Calculated Usage charge > 0 and not already billed, may exist without access charge
					Procedure procUsage=new Procedure();
					procUsage.CodeNum=procCodeUsage.CodeNum;
					procUsage.DateEntryC=DateTime.Today;
					procUsage.PatNum=pat.PatNum;
					procUsage.ProcDate=billingDate;
					procUsage.DateTP=billingDate;
					procUsage.ProcFee=smsBilling.MsgChargeTotalUSD;
					procUsage.ProcStatus=ProcStat.C;
					procUsage.ProvNum=PrefC.GetLong(PrefName.PracticeDefaultProv);
					procUsage.MedicalCode=procCodeUsage.MedicalCode;
					procUsage.BaseUnits=procCodeUsage.BaseUnits;
					procUsage.DiagnosticCode=PrefC.GetString(PrefName.ICD9DefaultForNewProcs);
					procUsage.PlaceService=(PlaceOfService)PrefC.GetInt(PrefName.DefaultProcedurePlaceService);//Default Proc Place of Service for the Practice is used. 
					procUsage.BillingNote="Texting Usage charge for "+smsBilling.DateUsage.ToString("MMMM yyyy")+".";
					Procedures.Insert(procUsage);
					listProcsUsage.Add(procUsage);
					retVal.Add(procUsage);
				}
			}
			return retVal;
		}

		///<summary>Returns true if the existing procedure was for the possibleBillingDate.</summary>
		private static bool IsRepeatDateHelper(RepeatCharge repeatCharge,DateTime possibleBillingDate,DateTime existingProcedureDate,Patient pat) {
			//No remoting role check; no call to db
			if(PrefC.GetBool(PrefName.BillingUseBillingCycleDay)) {
				pat=pat??Patients.GetPat(repeatCharge.PatNum);
				if(pat.BillingCycleDay!=existingProcedureDate.Day
					&& possibleBillingDate.AddMonths(-1).Month==existingProcedureDate.Month
					&& possibleBillingDate.AddMonths(-1).Year==existingProcedureDate.Year
					&& existingProcedureDate>=repeatCharge.DateStart) 
				{
					//This is needed in case the patient's billing day changed after procedures had been added for a repeat charge.
					return true;
				}
				//Only match month and year to be equal
				return (possibleBillingDate.Month==existingProcedureDate.Month && possibleBillingDate.Year==existingProcedureDate.Year);
			}
			if(possibleBillingDate.Month!=existingProcedureDate.Month || possibleBillingDate.Year!=existingProcedureDate.Year) {
				return false;
			}
			//Iterate through dates using new logic that takes repeatCharge.DateStart.AddMonths(n) to calculate dates
			DateTime possibleDateNew=repeatCharge.DateStart;
			int dateNewMonths=0;
			//Iterate through dates using old logic that starts with repeatCharge.DateStart and adds one month at a time to calculate dates
			DateTime possibleDateOld=repeatCharge.DateStart;
			do {
				if(existingProcedureDate==possibleDateNew || existingProcedureDate==possibleDateOld) {
					return true;
				}
				dateNewMonths++;
				possibleDateNew=repeatCharge.DateStart.AddMonths(dateNewMonths);
				possibleDateOld=possibleDateOld.AddMonths(1);
			}
			while(possibleDateNew<=existingProcedureDate);
			return false;
		}
	}

	public class RepeatChargeResult {
		public int ProceduresAddedCount=0;
		public int ClaimsAddedCount=0;
	}
}










