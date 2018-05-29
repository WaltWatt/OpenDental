using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormPaySplitManage:ODForm {
		///<summary>List of current paysplits for this payment.</summary>
		public List<PaySplit> ListSplitsCur;
		///<summary>List of current account charges for the family.  Gets filled from AutoSplitForPayment</summary>
		private List<AccountEntry> _listAccountCharges;
		///<summary>The amount entered for the current payment.  Amount currently available for paying off charges.  May be changed in this window.</summary>
		public decimal PaymentAmt;
		///<summary>The amount entered for the current payment.  Amount currently available for paying off charges.
		///If this value is zero, it will be set to the summation of the split amounts when OK is clicked.</summary>
		public decimal AmtTotal;
		public Family FamCur;
		public Patient PatCur;
		///<summary>Payment from the Payment window, amount gets modified only in the case that the original paymentAmt is zero.  It gets increased to
		///whatever the paysplit total is when the window is closed.</summary>
		public Payment PaymentCur;
		public DateTime PayDate;
		public bool IsNew;
		private List<long> listPatNums;
		///<summary>A dictionary or patients that we may need to reference to fill the grids to eliminate unnecessary calls to the DB.
		///Should contain all patients in the current family along with any patients of payment plans of which a member of this family is the guarantor.</summary>
		private Dictionary<long,Patient> dictPatients;
		private bool _isIncomeTransfer;
		public List<PaySplits.PaySplitAssociated> ListSplitsAssociated;

		public FormPaySplitManage(bool isIncomeTransfer) {
			InitializeComponent();
			_isIncomeTransfer=isIncomeTransfer;
			Lan.F(this);
		}

		private void FormPaySplitManage_Load(object sender,EventArgs e) {
			Init(false);
		}

		///<summary>Performs all of the Load functionality.  Public so it can be called from unit tests.</summary>
		public void Init(bool isTest) {
			_listAccountCharges=new List<AccountEntry>();
			textPayAmt.Text=PaymentAmt.ToString("f");
			AmtTotal=PaymentAmt;
			listPatNums=new List<long>();
			for(int i=0;i<FamCur.ListPats.Length;i++) {
				listPatNums.Add(FamCur.ListPats[i].PatNum);
			}
			dictPatients=new Dictionary<long,Patient>();
			List<Patient> listPatients=Patients.GetAssociatedPatients(PatCur.PatNum);
			listPatients.AddRange(FamCur.ListPats);
			foreach(Patient pat in listPatients) {
				dictPatients[pat.PatNum]=pat;
			}
			//This logic will ensure that regardless of if it's a new, or old payment any created paysplits that haven't been saved, 
			//such as if splits were made in this window then the window was closed and then reopened, will persist.
			decimal splitTotal=0;
			for(int i=0;i<ListSplitsCur.Count;i++) {
				splitTotal+=(decimal)ListSplitsCur[i].SplitAmt;
			}
			textSplitTotal.Text=POut.Decimal(splitTotal);
			PaymentAmt=Math.Round(PaymentAmt-splitTotal,3);
			checkShowPaid.Checked=true;
			//We want to fill the charge table.
			//AutoSplitForPayment will return new auto-splits if _payAvailableCur allows for some to be made.  Add these new splits to ListSplitsCur for display.
			ListSplitsCur.AddRange(AutoSplitForPayment(PaymentCur.PayNum,PayDate,isTest));
			FillGridSplits();
			//Select all charges on the right side that the paysplits are associated with.  Helps the user see what charges are attached.
			gridSplits.SetSelected(true);
			HighlightChargesForSplits();
		}

		///<summary>Fills the paysplit grid.</summary>
		private void FillGridSplits() {
			//Fill left grid with paysplits created
			gridSplits.BeginUpdate();
			gridSplits.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"Date"),65,HorizontalAlignment.Center);
			gridSplits.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Prov"),40);
			gridSplits.Columns.Add(col);
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
				col=new ODGridColumn(Lan.g(this,"Clinic"),40);
				gridSplits.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g(this,"Patient"),100);
			gridSplits.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"ProcCode"),60);
			gridSplits.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Type"),100);
			gridSplits.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Amount"),55,HorizontalAlignment.Right);
			gridSplits.Columns.Add(col);
			gridSplits.Rows.Clear();
			ODGridRow row;
			decimal splitTotal=0;
			for(int i=0;i<ListSplitsCur.Count;i++) {
				splitTotal+=(decimal)ListSplitsCur[i].SplitAmt;
				row=new ODGridRow();
				row.Tag=ListSplitsCur[i];
				row.Cells.Add(ListSplitsCur[i].DatePay.ToShortDateString());//Date
				row.Cells.Add(Providers.GetAbbr(ListSplitsCur[i].ProvNum));//Prov
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
					if(ListSplitsCur[i].ClinicNum!=0) {
						row.Cells.Add(Clinics.GetClinic(ListSplitsCur[i].ClinicNum).Description);//Clinic
					}
					else {
						row.Cells.Add("");//Clinic
					}
				}
				row.Cells.Add(Patients.GetPat(ListSplitsCur[i].PatNum).GetNameFL());//Patient
				row.Cells.Add(ProcedureCodes.GetStringProcCode(Procedures.GetOneProc(ListSplitsCur[i].ProcNum,false).CodeNum));//ProcCode
				string type="";
				if(ListSplitsCur[i].PayPlanNum!=0) {
					type+="PayPlanCharge";//Type
					if(ListSplitsCur[i].IsInterestSplit && ListSplitsCur[i].ProcNum==0 && ListSplitsCur[i].ProvNum!=0) {
						type+=" (interest)";
					}
				}
				if(ListSplitsCur[i].ProcNum!=0) {//Procedure
					Procedure proc=Procedures.GetOneProc(ListSplitsCur[i].ProcNum,false);
					string procDesc=Procedures.GetDescription(proc);
					if(type!="") {
						type+="\r\n";
					}
					type+="Proc: "+procDesc;//Type
				}
				if(ListSplitsCur[i].ProvNum==0) {//Unattached split
					if(type!="") {
						type+="\r\n";
					}
					type+="Unallocated";//Type
				}
				row.Cells.Add(type);
				if(row.Cells[row.Cells.Count-1].Text=="Unallocated") {
					row.Cells[row.Cells.Count-1].ColorText=Color.Red;
				}
				row.Cells.Add(ListSplitsCur[i].SplitAmt.ToString("f"));//Amount
				gridSplits.Rows.Add(row);
			}
			textSplitTotal.Text=POut.Decimal(splitTotal);
			gridSplits.EndUpdate();
			FillGridCharges();
		}

		///<summary>Fills charge grid, and then split grid.</summary>
		private void FillGridCharges() {
			//Fill right-hand grid with all the charges, filtered based on checkbox.
			gridCharges.BeginUpdate();
			gridCharges.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"Date"),65);
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Prov"),40);
			gridCharges.Columns.Add(col);
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
				col=new ODGridColumn(Lan.g(this,"Clinic"),40);
				gridCharges.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g(this,"Patient"),100);
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"ProcCode"),60);
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Type"),100);
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Amt Orig"),55,HorizontalAlignment.Right);
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Amt Start"),55,HorizontalAlignment.Right);
			gridCharges.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Amt End"),55,HorizontalAlignment.Right);
			gridCharges.Columns.Add(col);
			gridCharges.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listAccountCharges.Count;i++) {
				AccountEntry entryCharge=_listAccountCharges[i];
				if(!checkShowPaid.Checked) {//Filter out those that are paid in full and from other payments if checkbox unchecked.
					bool isFound=false;
					if(entryCharge.AmountEnd!=0) {
						isFound=true;
					}
					for(int j=0;j<gridSplits.Rows.Count;j++) {
						PaySplit entryCredit=(PaySplit)gridSplits.Rows[j].Tag;
						if(entryCharge.SplitCollection.Contains(entryCredit)) 
							//Charge is paid for by a split in this payment, display it.
						{
							if(entryCharge.GetType()==typeof(Procedure) && entryCredit.PayPlanNum!=0) {
								//Don't show the charge if it's a proc being paid by a payplan split.
								//From the user's perspective they're paying the "debits" not the procs.
							}
							else { 
								isFound=true;
								break;
							}
						}
						else if(entryCharge.GetType()==typeof(PayPlanCharge) && entryCredit.PayPlanNum==((PayPlanCharge)entryCharge.Tag).PayPlanNum && entryCharge.AmountStart!=0) {
							isFound=true;
							break;
						}
					}
					if(!isFound) {//Hiding charges that aren't associated with the current payment or have been paid in full.
						continue;
					}
				}
				row=new ODGridRow();
				row.Tag=_listAccountCharges[i];
				row.Cells.Add(entryCharge.Date.ToShortDateString());//Date
				row.Cells.Add(Providers.GetAbbr(entryCharge.ProvNum));//Provider
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
					row.Cells.Add(Clinics.GetAbbr(entryCharge.ClinicNum));
				}
				string patName=dictPatients[entryCharge.PatNum].LName + ", " + dictPatients[entryCharge.PatNum].FName;
				if(entryCharge.Tag.GetType()==typeof(PayPlanChargeType)) {
					patName+="\r\n"+Lan.g(this,"Guar")+": "+dictPatients[((PayPlanCharge)entryCharge.Tag).Guarantor].LName +", "
						+ dictPatients[((PayPlanCharge)entryCharge.Tag).Guarantor].FName;
				}
				row.Cells.Add(patName);//Patient
				string procCode="";
				if(entryCharge.Tag.GetType()==typeof(Procedure)) {
					procCode=ProcedureCodes.GetStringProcCode(((Procedure)entryCharge.Tag).CodeNum);
				}
				row.Cells.Add(procCode);//ProcCode
				row.Cells.Add(entryCharge.GetType().Name);//Type
				if(entryCharge.GetType()==typeof(Procedure)) {
					//Get the proc and add its description if the row is a proc.
					Procedure proc=(Procedure)entryCharge.Tag;
					row.Cells[row.Cells.Count-1].Text=Lan.g(this,"Proc")+": "+Procedures.GetDescription(proc);
				}
				row.Cells.Add(entryCharge.AmountOriginal.ToString("f"));//Amount Original
				row.Cells.Add(entryCharge.AmountStart.ToString("f"));//Amount Start
				row.Cells.Add(entryCharge.AmountEnd.ToString("f"));//Amount End
				gridCharges.Rows.Add(row);
			}
			gridCharges.EndUpdate();
		}

		///<summary>Creates paysplits associated to the patient passed in for the current payment until the payAmt has been met.  
		///Returns the list of new paysplits that have been created.  PaymentAmt will attempt to move towards 0 as paysplits are created.</summary>
		private List<PaySplit> AutoSplitForPayment(long payNum,DateTime date,bool isTest) {
			//Get the lists of items we'll be using to calculate with.
			List<Procedure> listProcs=Procedures.GetCompleteForPats(listPatNums);
			//listPayments should be empty, there isn't currently a way to make payments without at least one split.
			//During research however we found there were sometimes payments with no splits, so erred on the side of caution.
			List<Payment> listPayments=Payments.GetNonSplitForPats(listPatNums);
			List<Adjustment> listAdjustments=Adjustments.GetAdjustForPats(listPatNums);
			List<PaySplit> listPaySplits=PaySplits.GetForPats(listPatNums);//Might contain payplan payments.
			//Fix the memory locations of the existing pay splits for this payment within the list of pay splits for the entire family.
			//This is necessary for associating the correct tag values to grid rows.
			for(int i=listPaySplits.Count-1;i>=0;i--) {
				bool isFound=false;
				for(int j=0;j<ListSplitsCur.Count;j++) {
					if(listPaySplits[i].SplitNum==ListSplitsCur[j].SplitNum) {
						listPaySplits[i]=ListSplitsCur[j];
						isFound=true;
					}
				}
				if(!isFound && listPaySplits[i].PayNum==PaymentCur.PayNum) {
					//If we have a split that's not found in the passed-in list of splits for the payment
					//and the split we got from the DB is on this payment, remove it because the user must have deleted the split from the payment window.
					//The payment window won't update the DB with the change until it's closed.
					listPaySplits.RemoveAt(i);
				}
			}
			List<ClaimProc> listInsPayAsTotal=ClaimProcs.GetByTotForPats(listPatNums);//Claimprocs paid as total, might contain ins payplan payments.
			List<PayPlan> listPayPlans=PayPlans.GetForPats(listPatNums,PatCur.PatNum);//Used to figure out how much we need to pay off procs with, also contains insurance payplans.
			List<PayPlanCharge> listPayPlanCharges=new List<PayPlanCharge>();
			if(listPayPlans.Count>0){
				listPayPlanCharges=PayPlanCharges.GetDueForPayPlans(listPayPlans,PatCur.PatNum);//Does not get charges for the future.
			}
			List<ClaimProc> listClaimProcs=new List<ClaimProc>();
			for(int i=0;i<listPatNums.Count;i++) {
				listClaimProcs.AddRange(ClaimProcs.Refresh(listPatNums[i]));//There is no ClaimProcs.Refresh() for a family.
			}
			//Calculated using writeoffs, inspayest, inspayamt.  Done the same way ContrAcct does it.				
			for(int i=0;i<listProcs.Count;i++) {
				listProcs[i].ProcFee=ClaimProcs.GetPatPortion(listProcs[i],listClaimProcs,listAdjustments);
			}
			//Over the next 5 regions, we will do the following:
			//Create a list of account charges
			//Create a list of account credits
			//Explicitly link any of the credits to their corresponding charges if a link can be made. (ie. PaySplit.ProcNum to a Procedure.ProcNum)
			//Implicitly link any of the remaining credits to the non-zero charges FIFO by date.
			//Create Auto-splits for the current payment to any remaining non-zero charges FIFO by date.
			#region Construct List of Charges
			_listAccountCharges=new List<AccountEntry>();
			List<PayPlanCharge> listChargesToRemove=new List<PayPlanCharge>();
			foreach(PayPlanCharge ppc in listPayPlanCharges) {
				PayPlan pp=listPayPlans.Find(x => x.PayPlanNum==ppc.PayPlanNum);
				if(pp.IsClosed && PrefC.GetInt(PrefName.PayPlansVersion)==(int)PayPlanVersions.AgeCreditsAndDebits) {
					listChargesToRemove.Add(ppc);
					continue;
				}
				if(ppc.ChargeType==PayPlanChargeType.Debit && pp.InsSubNum==0) {
					//We only want debits from normal payplans, not ins payplans.
					_listAccountCharges.Add(new AccountEntry(ppc));
				}
			}
			listPayPlanCharges=listPayPlanCharges.Except(listChargesToRemove).ToList();
			for(int i=0;i<listAdjustments.Count;i++) {
				if(listAdjustments[i].AdjAmt>0 && listAdjustments[i].ProcNum==0) {
					_listAccountCharges.Add(new AccountEntry(listAdjustments[i]));
				}
			}
			for(int i=0;i<listProcs.Count;i++) {
				_listAccountCharges.Add(new AccountEntry(listProcs[i]));
			}
			_listAccountCharges.Sort(AccountEntrySort);
			#endregion Construct List of Charges
			#region Construct List of Credits
			//Getting a date-sorted list of all credits that haven't been attributed to anything.
			decimal creditTotal=0;
			for(int i=0;i<listAdjustments.Count;i++) {
				if(listAdjustments[i].AdjAmt<0 && listAdjustments[i].ProcNum==0) {
					creditTotal-=(decimal)listAdjustments[i].AdjAmt;
				}
			}
			for(int i=0;i<listPaySplits.Count;i++) {
				creditTotal+=(decimal)listPaySplits[i].SplitAmt;
			}
			for(int i=0;i<ListSplitsCur.Count;i++) {
				if(ListSplitsCur[i].SplitNum==0) {
					//If they created new splits on an old payment we need to add those to the credits list since they won't be over-written unlike a new payment.
					creditTotal+=(decimal)ListSplitsCur[i].SplitAmt;//Adding splits that haven't been entered into DB yet (re-opened split manager)
				}
			}
			for(int i=0;i<listPayments.Count;i++) {
				creditTotal+=(decimal)listPayments[i].PayAmt;
			}
			for(int i=0;i<listInsPayAsTotal.Count;i++) {			
				creditTotal+=(decimal)listInsPayAsTotal[i].InsPayAmt;
			}
			//Credits not attached to a procedure need to be used to counteract general charges instead of explicitly linked procs.
			creditTotal+=(decimal)listPayPlanCharges.Where(x => x.ChargeType == PayPlanChargeType.Credit)
				.Sum(y => y.Principal); //credits never have any interest
			#endregion Construct List of Credits
			#region Explicitly Link Credits
			//Any payment plan credits for procedures should get applied to that procedure and removed from the credit total bucket.
			foreach(AccountEntry chargeCur in _listAccountCharges) {
				if(chargeCur.Tag.GetType() == typeof(Procedure)) {
					decimal sumCreditsForProc=(decimal)listPayPlanCharges
						.Where(x=>x.ChargeType==PayPlanChargeType.Credit && x.ProcNum == chargeCur.PriKey)
						.Sum(x => x.Principal);
					//attaching more credits than the procedure is worth will apply that credit to account charges below (after Explicitly Link Credits region end)
					chargeCur.AmountStart-=sumCreditsForProc;
					chargeCur.AmountEnd-=sumCreditsForProc;
					creditTotal-=sumCreditsForProc;
				}
			}
			//Procedures First
			//Existing Payments on the payment plan
			//for each paysplit..
			List<PaySplit> listSplitsCurrentAndHistoric = new List<PaySplit>();
			listSplitsCurrentAndHistoric.AddRange(listPaySplits);
			listSplitsCurrentAndHistoric.AddRange(ListSplitsCur.Where(x => x.SplitNum == 0).ToList());
			foreach(PaySplit splitCur in listSplitsCurrentAndHistoric) {
				PaySplit splitCurCopy=splitCur.Copy(); //in case a method in the future needs an intact list of paysplits.
																							 //for each account entry...
				foreach(AccountEntry chargeCur in _listAccountCharges) {
					//if the account entry is a procedure and the split is attached to it, then explicitly apply the payment to the procedure.
					if(chargeCur.Tag.GetType() == typeof(Procedure) && splitCurCopy.ProcNum == chargeCur.PriKey && splitCurCopy.PayPlanNum == 0) {
						if(splitCurCopy.PayNum!=PaymentCur.PayNum) {
							chargeCur.AmountStart-=(decimal)splitCurCopy.SplitAmt;
						}
						chargeCur.AmountEnd-=(decimal)splitCurCopy.SplitAmt;
						chargeCur.SplitCollection.Add(splitCur);
						creditTotal-=(decimal)splitCurCopy.SplitAmt;
						break;
					}
					//else if the account entry is a payplancharge, then explicitly apply it to that payment plan charge.
					else if(chargeCur.Tag.GetType() == typeof(PayPlanCharge)
						&& splitCurCopy.PayPlanNum == ((PayPlanCharge)chargeCur.Tag).PayPlanNum
						&& chargeCur.AmountEnd > 0) {
						if(splitCurCopy.SplitAmt <= (double)chargeCur.AmountEnd) {//partial payment of the charge
							if(splitCurCopy.PayNum!=PaymentCur.PayNum) {
								chargeCur.AmountStart-=(decimal)splitCurCopy.SplitAmt;
							}
							chargeCur.AmountEnd-=(decimal)splitCurCopy.SplitAmt;
							//add a copy of the paysplit to the charge so that we can keep track of principal and interest payments.
							//We need to add the actual split to the list of paysplits because we call Contains() and Remove() on it.
							chargeCur.SplitCollection.Add(splitCur);
							creditTotal-=(decimal)splitCurCopy.SplitAmt;
							break;
						}
						else {//full payment, paySplitAmt > charge.AmountEnd
							if(splitCurCopy.PayNum!=PaymentCur.PayNum) {
								chargeCur.AmountStart=0;
							}
							creditTotal-=chargeCur.AmountEnd;
							splitCurCopy.SplitAmt-=(double)chargeCur.AmountEnd;//reduce split amount so that only what's left gets applied to other charges.
							chargeCur.SplitCollection.Add(splitCur);
							chargeCur.AmountEnd=0;
						}
					}
				}
			}
			#endregion Explicitly Link Credits
			//Apply negative charges as if they're credits.
			//This accounts for any procedures where the (paysplit amounts + payplancredit amounts) > ProcFee.
			for(int i = 0;i<_listAccountCharges.Count;i++) {
				AccountEntry entryCharge=_listAccountCharges[i];
				if(entryCharge.AmountEnd<0) {
					creditTotal-=entryCharge.AmountEnd; //Increases the credit amount since AmountEnd < 0.
					entryCharge.AmountEnd=0;
				}
			}
			#region Implicitly Link Credits
			//Now we have a date-sorted list of all the unpaid charges as well as all non-attributed credits.  
			//We need to go through each and pay them off in order until all we have left is the most recent unpaid charges.
			for(int i=0;i<_listAccountCharges.Count && creditTotal>0;i++) {
				AccountEntry charge=_listAccountCharges[i];
				decimal amt=Math.Min(charge.AmountEnd,creditTotal);
				charge.AmountEnd-=amt;
				creditTotal-=amt;
				charge.AmountStart-=amt;//Decrease amount original for the charge so we know what it was just prior to when the autosplits were made.
			}
			#endregion Implicitly Link Credits
			#region Auto-Split Current Payment
			//At this point we have a list of procs, positive adjustments, and payplancharges that require payment if the Amount>0.   
			//Create and associate new paysplits to their respective charge items.
			List<PaySplit> listAutoSplits=new List<PaySplit>();
			PaySplit split;
			for(int i=0;i<_listAccountCharges.Count;i++) {
				if(PaymentAmt==0) {
					break;
				}
				AccountEntry charge=_listAccountCharges[i];
				if(charge.AmountEnd==0) {
					continue;//Skip charges which are already paid.
				}
				if(PaymentAmt<0 && charge.AmountEnd>0) {//If they're different signs, don't make any guesses.  
				//This can happen if the user has less available than there are current splits for.
				//Remaining credits will always be all of one sign.
					if(!isTest && AmtTotal != 0) {
						//Don't want the message box to appear if the user did not set a AmtTotal limit for themselves.
						MsgBox.Show(this,"Payment cannot be automatically allocated because there are no outstanding negative balances.");
					}
					return listAutoSplits;//Will be empty
				}
				split=new PaySplit();
				if(charge.GetType()==typeof(PayPlanCharge)) { //payments are allocated differently for payment plan charges
					//it's an autosplit, so pass in 0 for payAmt (the method uses the classwide PaymentAmt variable instead)
					PayPlanVersions payPlanVer=(PayPlanVersions)PrefC.GetInt(PrefName.PayPlansVersion);
					if(payPlanVer!=PayPlanVersions.AgeCreditsAndDebits 
						|| (payPlanVer==PayPlanVersions.AgeCreditsAndDebits && !PayPlans.GetOne(((PayPlanCharge)charge.Tag).PayPlanNum).IsClosed)) 
					{ 
						decimal payAmtCur;
						ListSplitsCur.AddRange(PaySplits.CreateSplitForPayPlan(PaymentCur.PayNum,(double)PaymentAmt,charge,
							listPayPlanCharges.Where(x => x.ChargeType==PayPlanChargeType.Credit).ToList(),_listAccountCharges,0,true,out payAmtCur));
						PaymentAmt=payAmtCur;
					}
					continue;
				}
				else {
					if(Math.Abs(charge.AmountEnd)<Math.Abs(PaymentAmt)) {//charge has "less" than the payment, use partial payment.
						split.SplitAmt=(double)charge.AmountEnd;
						PaymentAmt=Math.Round(PaymentAmt-charge.AmountEnd,3);
						charge.AmountEnd=0;
					}
					else {//Use full payment
						split.SplitAmt=(double)PaymentAmt;
						charge.AmountEnd-=PaymentAmt;
						PaymentAmt=0;
					}
				}
				split.DatePay=date;
				split.PatNum=charge.PatNum;
				split.ProvNum=charge.ProvNum;
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
					split.ClinicNum=charge.ClinicNum;
				}
				if(charge.GetType()==typeof(Procedure)) {
					split.ProcNum=charge.PriKey;
				}
				else if(charge.GetType()==typeof(PayPlanCharge)) {
					split.PayPlanNum=((PayPlanCharge)charge.Tag).PayPlanNum;
				}
				split.PayNum=payNum;
				charge.SplitCollection.Add(split);
				listAutoSplits.Add(split);
			}
			if(listAutoSplits.Count==0 && ListSplitsCur.Count==0 && PaymentAmt != 0) {//Ensure there is at least one auto split if they entered a payAmt.
				split=new PaySplit();
				split.SplitAmt=(double)PaymentAmt;
				PaymentAmt=0;
				split.DatePay=date;
				split.PatNum=PaymentCur.PatNum;
				split.ProvNum=0;
				split.UnearnedType=PrefC.GetLong(PrefName.PrepaymentUnearnedType);//Use default unallocated type
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
					split.ClinicNum=PaymentCur.ClinicNum;
				}
				split.PayNum=payNum;
				listAutoSplits.Add(split);
			}
			if(PaymentAmt != 0) {//Create an unallocated split if there is any remaining payment amount.
				split=new PaySplit();
				split.SplitAmt=(double)PaymentAmt;
				PaymentAmt=0;
				split.DatePay=date;
				split.PatNum=PaymentCur.PatNum;
				split.ProvNum=0;
				split.UnearnedType=PrefC.GetLong(PrefName.PrepaymentUnearnedType);//Use default unallocated type
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
					split.ClinicNum=PaymentCur.ClinicNum;
				}
				split.PayNum=payNum;
				listAutoSplits.Add(split);
			}
			#endregion Auto-Split Current Payment
			return listAutoSplits;
		}

		///<summary>Creates a split similar to how CreateSplitsForPayment does it, but with selected rows of the grid.
		///If payAmt==0, attempt to pay charge in full.</summary>
		private void CreateSplit(AccountEntry charge,decimal payAmt,bool isManual=false) {
			PaySplit split=new PaySplit();
			split.DatePay=DateTimeOD.Today;
			if(charge.GetType()==typeof(Procedure)) {//Row selected is a Procedure.
				Procedure proc=(Procedure)charge.Tag;
				split.ProcNum=charge.PriKey;
			}
			else if(charge.GetType()==typeof(PayPlanCharge)) {//Row selected is a PayPlanCharge.
				PayPlanCharge ppChargeCur=(PayPlanCharge)charge.Tag;
				//if there is nothing owed, just create a split for 0.00 attached to the payplan but not any procedure.
				//that way the user can edit the split if they need to. If they don't edit it, then it will disappear after they click OK out of this window.
				if(charge.AmountEnd==0) { 
					split.ProvNum=charge.ProvNum;
					split.ClinicNum=charge.ClinicNum;
					split.PatNum=ppChargeCur.Guarantor;
					split.PayPlanNum=ppChargeCur.PayPlanNum;
					split.PayNum=PaymentCur.PayNum;
					ListSplitsCur.Add(split);
					return;
				}
				decimal payAmtCur;
				ListSplitsCur.AddRange(PaySplits.CreateSplitForPayPlan(PaymentCur.PayNum,PaymentCur.PayAmt,charge,
					PayPlanCharges.GetChargesForPayPlanChargeType(ppChargeCur.PayPlanNum,PayPlanChargeType.Credit),
					_listAccountCharges,
					payAmt,
					false,
					out payAmtCur));
				PaymentAmt=payAmtCur;
				return;
			}
			else if(charge.Tag.GetType()==typeof(Adjustment)) {//Row selected is an Adjustment.
				//Do nothing, nothing to link.
			}
			else {//PaySplits and overpayment refunds.
				//Do nothing, nothing to link.
			}
			if(isManual) {
				split.SplitAmt=(double)payAmt;
				charge.AmountEnd-=payAmt;
			}
			else { 
				decimal chargeAmt=charge.AmountEnd;
				if(Math.Abs(chargeAmt)<Math.Abs(payAmt) || PIn.Decimal(textPayAmt.Text)==0) {//Full payment of charge
					split.SplitAmt=(double)chargeAmt;
					charge.AmountEnd=0;//Reflect payment in underlying datastructure
				}
				else {//Partial payment of charge
					charge.AmountEnd-=payAmt;
					split.SplitAmt=(double)payAmt;
				}
			}
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Not no clinics
				split.ClinicNum=charge.ClinicNum;
			}
			PaymentAmt=PaymentAmt-(decimal)split.SplitAmt;
			split.ProvNum=charge.ProvNum;
			split.PatNum=charge.PatNum;
			split.PayNum=PaymentCur.PayNum;
			charge.SplitCollection.Add(split);
			ListSplitsCur.Add(split);
		}

		///<summary>Deletes selected paysplits from the grid and attributes amounts back to where they originated from.
		///This will return a list of payment plan charges that were affected. This is so that splits can be correctly re-attributed to the payplancharge
		///when the user edits the paysplit. There should only ever be one payplancharge in that list, since the user can only edit one split at a time.</summary>
		private List<long> DeleteSelected() {
			bool suppressMessage=false;
			//we need to return the payplancharge that the paysplit was associated to so that this paysplit can be correctly re-attributed to that charge.
			List<long> listPayPlanChargeNum=new List<long>();
			for(int i=gridSplits.SelectedIndices.Length-1;i>=0;i--) {
				int idx=gridSplits.SelectedIndices[i];
				PaySplit paySplit=(PaySplit)gridSplits.Rows[idx].Tag;
				if(paySplit.DateEntry!=DateTime.MinValue && !Security.IsAuthorized(Permissions.PaymentEdit,paySplit.DatePay,suppressMessage)) {
					suppressMessage=true;
					continue;//Don't delete this paysplit
				}
				if(paySplit.PayPlanNum!=0) {
					foreach(AccountEntry charge in _listAccountCharges) {
						if(charge.GetType()!=typeof(PayPlanCharge)) {//If the charge is not a payplancharge, we don't care
							continue;
						}
						if(!charge.SplitCollection.Contains(paySplit)) {//Only care about the charge if this paysplit is for that charge.
							continue;
						}
						if(paySplit.SplitAmt<=0) {
							break;
						}
						//It is now a charge for the payplan
						//When a split is deleted, put the money back on the payplan charge
						listPayPlanChargeNum.Add(charge.PriKey);
						decimal chargeAmtNew=charge.AmountEnd+(decimal)paySplit.SplitAmt;//Take the current value of the charge and add the split amt to it
						if(Math.Abs(chargeAmtNew)>Math.Abs(charge.AmountStart)) {//The split has more in it than the debit can take, use only a part of it
							//Find out how much of the split goes into the debit
							decimal debitDifference=charge.AmountStart-charge.AmountEnd;
							paySplit.SplitAmt-=(double)debitDifference;
							charge.AmountEnd=charge.AmountStart;
							PaymentAmt+=debitDifference;
							if(paySplit.ProcNum!=0) { 
								Procedure proc=(Procedure)_listAccountCharges.Find(x => x.GetType()==typeof(Procedure) && x.PriKey==paySplit.ProcNum).Tag;
								proc.ProcFee+=(double)debitDifference;//Put money back on the procfee so when we add splits later it calculates correctly
							}
						}
						else {
							charge.AmountEnd+=(decimal)paySplit.SplitAmt;//Give the money back to the charge so it will display.  Uses full paysplit amount.
							PaymentAmt+=(decimal)paySplit.SplitAmt;
							if(paySplit.ProcNum!=0) {
								Procedure proc=(Procedure)_listAccountCharges.Find(x => x.GetType()==typeof(Procedure) && x.PriKey==paySplit.ProcNum).Tag;
								proc.ProcFee+=paySplit.SplitAmt;//Put money back on the procfee so when we add splits later it calculates correctly
							}
							paySplit.SplitAmt=0;
						}
						charge.SplitCollection.Remove(paySplit);
					}
				}
				else { 
					for(int j=0;j<_listAccountCharges.Count;j++) {
						AccountEntry charge=_listAccountCharges[j];
						if(!charge.SplitCollection.Contains(paySplit)) {
							continue;
						}
						decimal chargeAmtNew=charge.AmountEnd+(decimal)paySplit.SplitAmt;
						if(Math.Abs(chargeAmtNew)>Math.Abs(charge.AmountStart)) {//Trying to delete an overpayment, just increase charge's amount to the max.
							charge.AmountEnd=charge.AmountStart;
						}
						else {
							charge.AmountEnd+=(decimal)paySplit.SplitAmt;//Give the money back to the charge so it will display.
						}
						charge.SplitCollection.Remove(paySplit);
					}
				}
				ListSplitsCur.Remove(paySplit);
				PaymentAmt=PaymentAmt+(decimal)paySplit.SplitAmt;
			}
			FillGridSplits();
			return listPayPlanChargeNum;
		}
		
		///<summary>Allows editing of an individual double clicked paysplit entry.</summary>
		private void gridSplits_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			PaySplit paySplitOld=(PaySplit)gridSplits.Rows[e.Row].Tag;
			PaySplit paySplit=paySplitOld.Copy();
			if(paySplit.DateEntry!=DateTime.MinValue && !Security.IsAuthorized(Permissions.PaymentEdit,paySplit.DatePay,false)) {
				return;
			}
			FormPaySplitEdit FormPSE=new FormPaySplitEdit(FamCur,_isIncomeTransfer);
      FormPSE.ListSplitsCur=ListSplitsCur;
			FormPSE.PaySplitCur=paySplit;
			FormPSE.SplitAssociated=ListSplitsAssociated.Find(x => x.PaySplitLinked==paySplitOld)??new PaySplits.PaySplitAssociated(null,null);
			FormPSE.ListPaySplitAssociated=ListSplitsAssociated;
			if(paySplit.IsInterestSplit && !MsgBox.Show(this,MsgBoxButtons.OKCancel,"Editing or deleting interest splits for a payment plan charge can"
				+" cause future splits to be allocated to the wrong provider. Do you want to continue?")) {
				return;
			}
			else if(FormPSE.ShowDialog()==DialogResult.OK) {//paySplit contains all the info we want.  
				//Delete paysplit from paysplit grid, credit the charge it's associated to.  
				//Paysplit may be re-associated with a different charge and we wouldn't know, so we need to do this.
				//Paysplits associated to payplancharge cannot be associated to a different payplancharge from this window.
				List<long> listEditedCharge=DeleteSelected();
				long payPlanChargeNum=0;
				if(listEditedCharge.Count>0) {
					payPlanChargeNum=listEditedCharge[0];
				}
				if(FormPSE.PaySplitCur==null) {//Deleted the paysplit, just return here.
					FillGridSplits();
					return;
				}
				UpdateForManualSplit(paySplit,payPlanChargeNum);
				PaymentAmt-=(decimal)paySplit.SplitAmt;
			}
		}

		///<summary>When a paysplit is selected this method highlights all charges associated with it.</summary>
		private void gridSplits_CellClick(object sender,ODGridClickEventArgs e) {
			HighlightChargesForSplits();
		}

		private void HighlightChargesForSplits() {
			gridCharges.SetSelected(false);
			for(int i=0;i<gridSplits.SelectedIndices.Length;i++) {
				PaySplit paySplit=(PaySplit)gridSplits.Rows[gridSplits.SelectedIndices[i]].Tag;
				for(int j=0;j<gridCharges.Rows.Count;j++) {
					AccountEntry accountEntryCharge=(AccountEntry)gridCharges.Rows[j].Tag;
					if(accountEntryCharge.SplitCollection.Contains(paySplit)) {
						gridCharges.SetSelected(j,true);
					}
				}
			}
		}

		///<summary>When a charge is selected this method highlights all paysplits associated with it.</summary>
		private void gridCharges_CellClick(object sender,ODGridClickEventArgs e) {
			gridSplits.SetSelected(false);
			for(int i=0;i<gridCharges.SelectedIndices.Length;i++) {
				AccountEntry accountEntryCharge=(AccountEntry)gridCharges.Rows[gridCharges.SelectedIndices[i]].Tag;
				for(int j=0;j<gridSplits.Rows.Count;j++) {
					PaySplit paySplit=(PaySplit)gridSplits.Rows[j].Tag;
					if(accountEntryCharge.SplitCollection.Contains(paySplit)) {
						gridSplits.SetSelected(j,true);
					}
					if(accountEntryCharge.GetType()==typeof(PayPlanCharge)) {
						if(paySplit.PayPlanNum==((PayPlanCharge)accountEntryCharge.Tag).PayPlanNum) {
							gridSplits.SetSelected(j,true);
						}
					}
				}
			}
		}

		///<summary>Creates a manual paysplit.</summary>
		private void butAdd_Click(object sender,EventArgs e) {
			PaySplit paySplit=new PaySplit();
			paySplit.SplitAmt=0;
			paySplit.DatePay=PaymentCur.PayDate;
			paySplit.DateEntry=MiscData.GetNowDateTime();//just a nicity for the user.  Insert uses server time.
			paySplit.PayNum=PaymentCur.PayNum;
			paySplit.ProvNum=Patients.GetProvNum(PatCur);
			paySplit.ClinicNum=PaymentCur.ClinicNum;
			FormPaySplitEdit FormPSE=new FormPaySplitEdit(FamCur,_isIncomeTransfer);
      FormPSE.ListSplitsCur=ListSplitsCur;
			FormPSE.PaySplitCur=paySplit;
			FormPSE.IsNew=true;
			if(FormPSE.ShowDialog()==DialogResult.OK) {
				UpdateForManualSplit(paySplit);
			}
		}

		///<summary>Updates the underlying data structures when a manual split is created or edited.</summary>
		private void UpdateForManualSplit(PaySplit paySplit, long payPlanChargeNum=0) {
			//Find the charge row for this new split.
			ListSplitsCur.Add(paySplit);
			//Locate a charge to apply the credit to, if a reasonable match exists.
			for(int i=0;i<_listAccountCharges.Count;i++) {
				AccountEntry charge=_listAccountCharges[i];
				if(charge.AmountEnd==0) {
					continue;
				}
				bool isMatchFound=false;
				//New Split is for this proc (but not attached to a payplan)
				if(charge.GetType()==typeof(Procedure) && charge.PriKey==paySplit.ProcNum && paySplit.PayPlanNum == 0) {
					isMatchFound=true;
				}
				else if(charge.GetType()==typeof(Adjustment) //New split is for this adjust
						&& paySplit.ProcNum==0 && paySplit.PayPlanNum==0 //Both being 0 is the only way we can tell it's for an Adj
						&& paySplit.DatePay==charge.Date
						&& paySplit.ProvNum==charge.ProvNum)
				{
					isMatchFound=true;
				}
				else if(charge.GetType()==typeof(PayPlanCharge) && charge.PriKey == payPlanChargeNum) {//split is for this payplancharge
					isMatchFound=true;
				}				
				if(isMatchFound) {
					decimal amtOwed=charge.AmountEnd;
					if(Math.Abs(amtOwed)<Math.Abs((decimal)paySplit.SplitAmt)) {//Partial payment
						charge.AmountEnd=0;//Reflect payment in underlying datastructure
					}
					else {//Full payment
						charge.AmountEnd=amtOwed-(decimal)paySplit.SplitAmt;
					}
					charge.SplitCollection.Add(paySplit);
				}
				//If none of these, it's unattached to the best of our knowledge.				
			}			
			FillGridSplits();//Fills charge grid too.
		}

		private void checkShowPaid_CheckedChanged(object sender,EventArgs e) {
			FillGridSplits();
		}

		///<summary>Creates paysplits for selected charges if there is enough payment left.</summary>
		private void butAddSplit_Click(object sender,EventArgs e) {
			for(int i=0;i<gridCharges.SelectedIndices.Length;i++) {
				AccountEntry charge=(AccountEntry)gridCharges.Rows[gridCharges.SelectedIndices[i]].Tag;
				CreateSplit(charge,PaymentAmt);
			}
			FillGridSplits();//Fills charge grid too.
		}

		///<summary>Creates paysplits after allowing the user to enter in a custom amount to pay for each selected charge.</summary>
		private void butAddPartial_Click(object sender,EventArgs e) {
			for(int i=0;i<gridCharges.SelectedIndices.Length;i++) {
				string chargeDescript="";
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
					chargeDescript=gridCharges.Rows[gridCharges.SelectedIndices[i]].Cells[4].Text;
				}
				else {
					chargeDescript=gridCharges.Rows[gridCharges.SelectedIndices[i]].Cells[3].Text;
				}
				FormAmountEdit FormAE=new FormAmountEdit(chargeDescript);
				AccountEntry selectedEntry=(AccountEntry)gridCharges.Rows[gridCharges.SelectedIndices[i]].Tag;
				FormAE.Amount=selectedEntry.AmountEnd;
				FormAE.ShowDialog();
				if(FormAE.DialogResult==DialogResult.OK) {
					decimal amount=FormAE.Amount;
					if(amount!=0) {
						CreateSplit(selectedEntry,amount,true);
					}
				}
			}
			FillGridSplits();//Fills charge grid too.
		}

		///<summary>Deletes all paysplits.</summary>
		private void butClearAll_Click(object sender,EventArgs e) {
			gridSplits.SetSelected(true);
			DeleteSelected();
		}

		///<summary>Deletes selected paysplits.</summary>
		private void butDeleteSplit_Click(object sender,EventArgs e) {
			for(int i = 0;i<gridSplits.SelectedIndices.Length;i++) {
				if(((PaySplit)gridSplits.Rows[gridSplits.SelectedIndices[i]].Tag).IsInterestSplit) {
					if(MsgBox.Show(this,MsgBoxButtons.OKCancel,"Deleting interest splits for a payment plan charge can cause line item accounting"
						+" to appear to be off in the paysplit manager. Do you want to continue?")) {
						break;//they clicked continue.
					}
					else {
						return;//return out of this method -- they don't want to delete.
					}
				}
			}
			DeleteSelected();
		}

		private void butOK_Click(object sender,EventArgs e) {
			decimal payAmt=PIn.Decimal(textPayAmt.Text);
			decimal splitTotal=PIn.Decimal(textSplitTotal.Text);
			if(splitTotal != 0 && PaymentCur.PayType == 0) { //income transfer
				MsgBox.Show(this,"Income transfers must have a split total of 0.");
				return;
			}
			//Create an unallocated split if there is any remaining in the payment amount.
			//Only create the unallocated payment if the sum of the splits is less than the whole payment amount.
			if(Math.Abs(payAmt)>Math.Abs(splitTotal) && payAmt!=0) {
				PaySplit split=new PaySplit();
				split.SplitAmt=(double)(payAmt-splitTotal);
				PaymentAmt=0;
				split.DatePay=PaymentCur.DateEntry;
				split.PatNum=PaymentCur.PatNum;
				split.ProvNum=0; //unallocated. This will make the text appear red so that the user knows to check it. If saved, will default to being a prepayment.
				split.UnearnedType=PrefC.GetLong(PrefName.PrepaymentUnearnedType);//Use default unallocated type
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {//Clinics
					split.ClinicNum=PaymentCur.ClinicNum;
				}
				split.PayNum=PaymentCur.PayNum;
				ListSplitsCur.Add(split);
				MsgBox.Show(this,"Payment split total does not equal payment amount.  An unallocated payment split has been added, please check for correctness.");
				FillGridSplits();
				return;
			}
			else if(payAmt==0) {//If they have a payment amount of 0 set the payment's PayAmt to what the split total is.
				AmtTotal=splitTotal;
			}
			else if(payAmt==splitTotal) {
				//Do nothing.
			}
			else {
				MsgBox.Show(this,"Payment amount cannot be less than the total split value.");
				return;
			}
			List<AccountEntry> listAdjustmentEntriesPaid = _listAccountCharges.Where(x => x.Tag.GetType() == typeof(Adjustment) && x.AmountStart > x.AmountEnd).ToList();
			foreach(AccountEntry adjEntry in listAdjustmentEntriesPaid) {
				if(_listAccountCharges
					.Where(x => x.AmountEnd > 0
						&& (x.Tag.GetType()==typeof(Procedure) || x.Tag.GetType()==typeof(PayPlanCharge))
						&& x.Date < adjEntry.Date).Count()>0) {
					if(MsgBox.Show(this,MsgBoxButtons.OKCancel,"Splits to adjustments exist when there are older procedures or payment plan charges not paid "
						+"off. This can cause line item accounting to be off for future splits made in the Paysplit manager.  Do you want to continue?")) {
						break; //they want to continue. Only ever show this msgbox once.
					}
					else {
						return;
					}
				}
			}
			for(int i=ListSplitsCur.Count-1;i>=0;i--) {
				if(ListSplitsCur[i].SplitAmt==0) {
					ListSplitsCur.RemoveAt(i);//We don't want any zero splits.  They were there just for display purposes.
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			//No paysplits were inserted.  Just close the window.
			DialogResult=DialogResult.Cancel;
		}

		///<summary>Simple sort that sorts based on date.</summary>
		private int AccountEntrySort(AccountEntry x,AccountEntry y) {
			return x.Date.CompareTo(y.Date);
		}

	}
}