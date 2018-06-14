using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;
using System.Drawing;
using System.Drawing.Printing;

namespace OpenDental {
	///<summary>This form will not be available to those who are still using PayPlans Version 1.</summary>
	public partial class FormPayPlanCredits:ODForm {
		///<summary>Set to the patient of the payment plan in the constructor.</summary>
		private Patient _patCur;
		private PayPlan _payPlanCur;
		public bool IsNew;
		public bool IsDeleted;
		///<summary>The current patNum is the only patient in the list.</summary>
		private List<long> _listPatNums;
		private List<Adjustment> _listAdjustments;
		private List<Procedure> _listProcs;
		private List<PayPlanCharge> _listPayPlanCharges;
		private List<ClaimProc> _listInsPayAsTotal;
		private List<Payment> _listPayments;
		private List<PaySplit> _listPaySplits;
		private List<AccountEntry>_listAccountCharges;
		private decimal _accountCredits;
		///<summary>Payment plan credits for the current payment plan.  Must be passed-in.</summary>
		public List<PayPlanCharge> ListPayPlanCreditsCur;
		///<summary>For getting insurace estimates for TP procedures and completed procedures whose claims haven't been received.
    ///Also used for getting insurance payments on procedures.</summary>
		private List<ClaimProc> _listClaimProcs;
		private List<PayPlanEntry> _listPayPlanEntries;
		private bool headingPrinted;
		private int pagesPrinted;
		private int headingPrintH;

		public FormPayPlanCredits(PayPlan payPlanCur) {
			_payPlanCur=payPlanCur;
			_patCur=Patients.GetPat(payPlanCur.PatNum);
			InitializeComponent();
			Lan.F(this);
		}

		private void FormPayPlanCredits_Load(object sender,EventArgs e) {
			_listPatNums=new List<long>();
			_listPatNums.Add(_patCur.PatNum);
			_listAdjustments=Adjustments.GetAdjustForPats(_listPatNums);
			_listProcs=Procedures.GetCompAndTpForPats(_listPatNums);
			List<PayPlan> listPayPlans=PayPlans.GetForPats(_listPatNums,_patCur.PatNum);//Used to figure out how much we need to pay off procs with, also contains insurance payplans.
			_listPayPlanCharges=new List<PayPlanCharge>();
			if(listPayPlans.Count>0) {
				//get all current payplan charges for plans already on the patient, excluding the current one.
				_listPayPlanCharges=PayPlanCharges.GetDueForPayPlans(listPayPlans,_patCur.PatNum)//Does not get charges for the future.
					.Where(x => !(x.PayPlanNum==_payPlanCur.PayPlanNum && x.ChargeType==PayPlanChargeType.Credit)).ToList(); //do not get credits for current payplan
			}
			List<PaySplit> tempListPaySplits=PaySplits.GetForPats(_listPatNums).Where(x => x.UnearnedType == 0).ToList();//Might contain payplan payments. Do not include unearned.
			_listPaySplits=tempListPaySplits.FindAll(x => x.PayPlanNum==0 || listPayPlans.Exists(y => y.PayPlanNum==x.PayPlanNum));
			_listPayments=Payments.GetNonSplitForPats(_listPatNums);
			_listInsPayAsTotal=ClaimProcs.GetByTotForPats(_listPatNums);//Claimprocs paid as total, might contain ins payplan payments.
			_listClaimProcs=ClaimProcs.GetForProcs(_listProcs.Select(x => x.ProcNum).ToList());
			textCode.Text=Lan.g(this,"None");
			FillGrid();
			if(!Security.IsAuthorized(Permissions.PayPlanEdit,true)) {
				this.DisableForm(butCancel,checkHideUnattached,checkShowImplicit,butPrint,gridMain);
			}
		}

		private void CreatePayPlanEntries(bool showImplicitlyPaidOffProcs=false) {
			_listAccountCharges=GetAccountCharges();
			if(showImplicitlyPaidOffProcs) {
				_accountCredits=0;
			}
			else {
				_accountCredits=GetAccountCredits();
			}
			LinkChargesToCredits();
			_listPayPlanEntries=new List<PayPlanEntry>();
			foreach(AccountEntry entryCharge in _listAccountCharges) {
				//for all account charges
				if(entryCharge.AmountEnd==0
					|| entryCharge.GetType()==typeof(Adjustment)
					|| entryCharge.GetType()==typeof(PayPlanCharge)
					|| entryCharge.PatNum!=_patCur.PatNum) {
					continue;
				}
				//if it's a procedure..
				Procedure procCur=(Procedure)entryCharge.Tag;
				_listPayPlanEntries.Add(new PayPlanEntry() {
					ProcNumOrd=procCur.ProcNum,
					AmtOrd=(double)entryCharge.AmountOriginal,
					DateOrd=procCur.ProcDate,
					ProcStatOrd=procCur.ProcStatus,
					IsChargeOrd=false,
					DateStr=procCur.ProcDate.ToShortDateString(),
					PatStr=_patCur.FName,
					StatStr=procCur.ProcStatus.ToString(),
					ProcStr=ProcedureCodes.GetStringProcCode(procCur.CodeNum),
					FeeStr=(Procedures.CalculateProcCharge(procCur)).ToString("f"),
					RemBefStr=entryCharge.AmountStart.ToString("f"),
					Proc=procCur,
					//everything else blank
				});
			}
			ListPayPlanCreditsCur=ListPayPlanCreditsCur.OrderBy(x => x.ChargeDate).ToList();
			List<PayPlanCharge> listCreditsApplied= new List<PayPlanCharge>();
			//if it's a payplancharge..
			foreach(PayPlanCharge credCur in ListPayPlanCreditsCur) {
				AccountEntry entryCur=_listAccountCharges.Where(x => x.PriKey==credCur.ProcNum && x.GetType() == typeof(Procedure)).FirstOrDefault();
				Procedure procCur=null;
				if(entryCur!=null) {
					procCur=(Procedure)entryCur.Tag;
				}
				double fee=0;
				if(entryCur!=null) {
					fee=(double)entryCur.AmountStart;
				}
				listCreditsApplied.Add(credCur);
				double remAfter=fee-listCreditsApplied.Where(x => x.ProcNum==credCur.ProcNum).Sum(x => x.Principal);
				PayPlanEntry addEntry=new PayPlanEntry();
				addEntry.ProcNumOrd=credCur.ProcNum;
				addEntry.AmtOrd=credCur.Principal;
				addEntry.DateOrd=credCur.ChargeDate;
				addEntry.ProcStatOrd=ProcStat.TP;
				if(procCur!=null) {
					addEntry.ProcStatOrd=procCur.ProcStatus;
				}
				addEntry.IsChargeOrd=true;
				if(procCur!=null && procCur.ProcStatus==ProcStat.TP) {
					addEntry.CredDateStr=Lan.g(this,"None");
				}
				else {
					addEntry.CredDateStr=credCur.ChargeDate.ToShortDateString();
				}
				addEntry.AmtStr=credCur.Principal.ToString("f");
				addEntry.RemAftStr="";
				if(entryCur!=null) {
					addEntry.RemAftStr=remAfter.ToString("f");
				}
				addEntry.NoteStr=credCur.Note;
				addEntry.Proc=procCur;
				addEntry.Charge=credCur;
					//everything else blank
				_listPayPlanEntries.Add(addEntry);
			}
			//add another procedure row showing all unattached credits if at least one charge with a procnum of 0 exists.
			if(ListPayPlanCreditsCur.Exists(x => x.ProcNum==0)){
				_listPayPlanEntries.Add(new PayPlanEntry {
					AmtOrd=0,
					DateOrd=DateTime.MinValue,
					ProcStatOrd=ProcStat.TP, //for ordering purposes, since we want unattached to always show up last.
					IsChargeOrd=false,
					ProcNumOrd=0,
					DateStr=Lan.g(this,"Unattached"),
					PatStr=_patCur.FName,
					StatStr="",
					ProcStr="",
					FeeStr="",
					RemBefStr="",
					//everything else blank
				});
			}
			_listPayPlanEntries=_listPayPlanEntries
				.OrderByDescending(x => x.ProcStatOrd)
				.ThenByDescending(x => x.ProcNumOrd)
				.ThenBy(x => x.IsChargeOrd)
				.ThenBy(x => x.DateOrd).ToList();
		}

		private void FillGrid() {
			if(checkShowImplicit.Checked) {
				CreatePayPlanEntries(true);
			}
			else {
				CreatePayPlanEntries();
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g("TablePaymentPlanProcsAndCreds","Date"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePaymentPlanProcsAndCreds","Stat"),30);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePaymentPlanProcsAndCreds","Code"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePaymentPlanProcsAndCreds","Fee"),55,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePaymentPlanProcsAndCreds","Rem Before"),70,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePaymentPlanProcsAndCreds","Credit Date"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePaymentPlanProcsAndCreds","Amount"),55,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePaymentPlanProcsAndCreds","Rem After"),60,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePaymentPlanProcsAndCreds","Note"),0);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			double totalAttached=0;
			foreach(PayPlanEntry entryCur in _listPayPlanEntries) { //for all account charges
				if(checkHideUnattached.Checked && !ListPayPlanCreditsCur.Exists(x => x.ProcNum==entryCur.ProcNumOrd)) {
					continue;
				}
				row=new ODGridRow();
				//we color the relevant cells to make the table easier to read.
				//the colors have been looked-at and approved by colourblind Josh.
				//In the future, we will probably make these customizable definitions.
				ODGridCell cell=new ODGridCell(entryCur.DateStr);
				//for procedure rows, cell color should be LightYellow for all relevant fields.
				cell.CellColor=entryCur.IsChargeOrd ? Color.White : Color.LightYellow;
				row.Cells.Add(cell);
				cell=new ODGridCell(entryCur.StatStr);
				cell.CellColor=entryCur.IsChargeOrd ? Color.White : Color.LightYellow;
				row.Cells.Add(cell);
				cell=new ODGridCell(entryCur.ProcStr);
				cell.CellColor=entryCur.IsChargeOrd ? Color.White : Color.LightYellow;
				row.Cells.Add(cell);
				cell=new ODGridCell(entryCur.FeeStr);
				cell.CellColor=entryCur.IsChargeOrd ? Color.White : Color.LightYellow;
				row.Cells.Add(cell);
				cell=new ODGridCell(entryCur.RemBefStr);
				cell.CellColor=entryCur.IsChargeOrd ? Color.White : Color.LightYellow;
				row.Cells.Add(cell);
				cell=new ODGridCell(entryCur.CredDateStr);
				//for charge rows, cell color should be LightCyan for all relevant fields.
				cell.CellColor=entryCur.IsChargeOrd ? Color.LightCyan : Color.White;
				row.Cells.Add(cell);
				cell=new ODGridCell(entryCur.AmtStr);
				cell.CellColor=entryCur.IsChargeOrd ? Color.LightCyan : Color.White;
				row.Cells.Add(cell);
				totalAttached+=PIn.Double(entryCur.AmtStr);
				cell=new ODGridCell(entryCur.RemAftStr);
				cell.CellColor=entryCur.IsChargeOrd ? Color.LightCyan : Color.White;
				row.Cells.Add(cell);
				cell=new ODGridCell(entryCur.NoteStr);
				cell.CellColor=entryCur.IsChargeOrd ? Color.LightCyan : Color.White;
				row.Cells.Add(cell);
				row.Tag=entryCur;
				if(!entryCur.IsChargeOrd) {
					row.ColorLborder=Color.Black;
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			textTotal.Text=totalAttached.ToString("f");
		}

		///<summary>Gets all charges for the current patient. Returns a list of AccountEntries.</summary>
		private List<AccountEntry> GetAccountCharges() {
			List<AccountEntry> listCharges=new List<AccountEntry>();
			for(int i=0;i<_listPayPlanCharges.Count;i++) {
					if(_listPayPlanCharges[i].ChargeType==PayPlanChargeType.Debit) {
						listCharges.Add(new AccountEntry(_listPayPlanCharges[i]));
					}
				}
			for(int i=0;i<_listAdjustments.Count;i++) {
				if(_listAdjustments[i].AdjAmt>0 && _listAdjustments[i].ProcNum==0) {
					listCharges.Add(new AccountEntry(_listAdjustments[i]));
				}
			}
			for(int i=0;i<_listProcs.Count;i++) {
				listCharges.Add(new AccountEntry(_listProcs[i]));
			}
			listCharges.Sort(AccountEntrySort);
			return listCharges;
		}

		///<summary>Gets all unattached credits for the current patient.  Returns the summed credits as a decimal.</summary>
		private decimal GetAccountCredits() {
			//Getting a date-sorted list of all credits that haven't been attributed to anything.
			decimal creditTotal=0;
			for(int i=0;i<_listAdjustments.Count;i++) {
				if(_listAdjustments[i].AdjAmt<0) {
					creditTotal-=(decimal)_listAdjustments[i].AdjAmt;
				}
			}
			for(int i=0;i<_listPaySplits.Count;i++) {
				creditTotal+=(decimal)_listPaySplits[i].SplitAmt;
			}
			for(int i=0;i<_listPayments.Count;i++) {
				creditTotal+=(decimal)_listPayments[i].PayAmt;
			}
			for(int i=0;i<_listInsPayAsTotal.Count;i++) {
				creditTotal+=(decimal)_listInsPayAsTotal[i].InsPayAmt;
			}
			for(int i=0;i<_listPayPlanCharges.Count;i++) {
				if(_listPayPlanCharges[i].ChargeType==PayPlanChargeType.Credit) {
					creditTotal+=(decimal)_listPayPlanCharges[i].Principal;
				}
			}
			//Credits for the current payment plan shouldn't get linked here, they get linked above.
			//for(int i = 0;i<ListPayPlanCreditsCur.Count;i++) {
			//	if(ListPayPlanCreditsCur[i].ChargeType==PayPlanChargeType.Credit) {
			//		creditTotal+=(decimal)ListPayPlanCreditsCur[i].Principal;
			//	}
			//}
			//don't need to to take insurance estimates and estimated writeoffs into account as they are always explicitly linked to procedures.
			return creditTotal;
		}

		///<summary>Links charges to credits explicitly based on FKs first, then implicitly based on Date.</summary>
		private void LinkChargesToCredits() {
			#region Explicit
			for(int i=0;i<_listAccountCharges.Count;i++) {
				AccountEntry charge=_listAccountCharges[i];
				for(int j=0;j<_listPaySplits.Count;j++) {
					PaySplit paySplit=_listPaySplits[j];
					decimal paySplitAmt=(decimal)paySplit.SplitAmt;
					//Procedures that were being paid on through this payment plan should not get removed from this grid, even if they are fully paid off.
					if(charge.GetType()==typeof(Procedure)
						&& paySplit.ProcNum==charge.PriKey
						&& (paySplit.PayPlanNum == 0 || paySplit.PayPlanNum != _payPlanCur.PayPlanNum)) 
					{
						charge.ListPaySplits.Add(paySplit);
						charge.AmountEnd-=paySplitAmt;
						_accountCredits-=paySplitAmt;
						charge.AmountStart-=paySplitAmt;
					}
					else if(charge.GetType()==typeof(PayPlanCharge) && ((PayPlanCharge)charge.Tag).PayPlanNum==paySplit.PayPlanNum && charge.AmountEnd>0 && paySplit.SplitAmt>0) {
						charge.AmountEnd-=paySplitAmt;
						_accountCredits-=paySplitAmt;
					}
				}
				for(int j = 0;j<_listAdjustments.Count;j++) {
					Adjustment adjustment=_listAdjustments[j];
					decimal adjustmentAmt=(decimal)adjustment.AdjAmt;
					if(charge.GetType()==typeof(Procedure) && adjustment.ProcNum==charge.PriKey) {
						charge.AmountEnd+=adjustmentAmt;
						if(adjustment.AdjAmt<0) {
							_accountCredits+=adjustmentAmt;
						}
						charge.AmountStart+=adjustmentAmt;
						//If the adjustment is attached to a procedure decrease the procedure's amountoriginal so we know what it was just prior to autosplitting.
					}
				}
				for(int j = 0;j < _listPayPlanCharges.Count;j++) {
					PayPlanCharge payPlanCharge=_listPayPlanCharges[j];
					if(charge.GetType()==typeof(Procedure) && payPlanCharge.ProcNum == charge.PriKey) //payPlanCharge.ProcNum will only be set for credits.
						{
						charge.AmountEnd-=(decimal)payPlanCharge.Principal;
						charge.AmountStart-=(decimal)payPlanCharge.Principal;
						_accountCredits-=(decimal)payPlanCharge.Principal;
					}
				}
				//Credits for the current payment plan don't get linked here, they get linked on the grid (Above)
				//for(int j = 0;j < ListPayPlanCreditsCur.Count;j++) {
				//	PayPlanCharge payPlanCharge=ListPayPlanCreditsCur[j];
				//	if(charge.GetType()==typeof(Procedure) && payPlanCharge.ProcNum == charge.PriKey) {
				//		charge.AmountEnd-=(decimal)payPlanCharge.Principal;
				//		charge.AmountStart-=(decimal)payPlanCharge.Principal;
				//		_accountCredits-=(decimal)payPlanCharge.Principal;
				//	}
				//}
				//
				//claimprocs explicitly linked to the procedures for this patient.
				for(int j=0;j < _listClaimProcs.Count;j++) {
					ClaimProc claimProcCur=_listClaimProcs[j];
					if(charge.GetType()!=typeof(Procedure) || claimProcCur.ProcNum!=charge.PriKey) {
						continue;
					}
					decimal amt=0;
					if((claimProcCur.Status==ClaimProcStatus.Estimate || claimProcCur.Status==ClaimProcStatus.NotReceived)) {
						//Estimated Payment
						amt=(decimal)claimProcCur.InsEstTotal;
						if(claimProcCur.InsEstTotalOverride!=-1) {
							amt=(decimal)claimProcCur.InsEstTotalOverride;
						}
						charge.AmountEnd-=amt;
						charge.AmountStart-=amt;
						//Estimated Writeoff
						amt=0;
						if(claimProcCur.WriteOffEstOverride!=-1) {
							amt=(decimal)claimProcCur.WriteOffEstOverride;
						}
						else if(claimProcCur.WriteOffEst!=-1) {
							amt=(decimal)claimProcCur.WriteOffEst;
						}
						charge.AmountEnd-=amt;
						charge.AmountStart-=amt;
					}
					else if(claimProcCur.Status==ClaimProcStatus.Received || claimProcCur.Status==ClaimProcStatus.Supplemental
						|| claimProcCur.Status==ClaimProcStatus.CapClaim || claimProcCur.Status==ClaimProcStatus.CapComplete) 
					{
						//actual payment and actual writeoff.
						amt=(decimal)claimProcCur.InsPayAmt+(decimal)claimProcCur.WriteOff;
						charge.AmountEnd-=amt;
						charge.AmountStart-=amt;
					}
				}
			}
			#endregion Explicit
			//Apply negative charges as if they're credits.
			for(int i=0;i < _listAccountCharges.Count;i++) {
				AccountEntry entryCharge=_listAccountCharges[i];
				if(entryCharge.AmountEnd<0) {
					_accountCredits-=entryCharge.AmountEnd;
					entryCharge.AmountEnd=0;
				}
			}
			#region Implicit
			//Now we have a date-sorted list of all the unpaid charges as well as all non-attributed credits.  
			//We need to go through each and pay them off in order until all we have left is the most recent unpaid charges.
			for(int i=0;i<_listAccountCharges.Count && _accountCredits>0;i++) {
				AccountEntry charge=_listAccountCharges[i];
				decimal amt=Math.Min(charge.AmountEnd,_accountCredits);
				charge.AmountEnd-=amt;
				_accountCredits-=amt;
				charge.AmountStart-=amt;//Decrease amount original for the charge so we know what it was just prior to when the autosplits were made.
			}
			#endregion Implicit
		}

		///<summary>Simple sort that sorts based on date.</summary>
		private int AccountEntrySort(AccountEntry x,AccountEntry y) {
			return x.Date.CompareTo(y.Date);
		}

		private void SetTextBoxes() {
			List<PayPlanEntry> listSelectedEntries=new List<PayPlanEntry>();
			for(int i=0;i < gridMain.SelectedIndices.Count();i++) { //fill the list with all the selected items in the grid.
				listSelectedEntries.Add((PayPlanEntry)(gridMain.Rows[gridMain.SelectedIndices[i]].Tag));
			}
			bool isUpdateButton=false;//keep track of the state of the button, if it is add or update. 
			if(listSelectedEntries.Count==0) { //if there are no entries selected
				//button should say Add, textboxes should be editable. No attached procedure.
				butAddOrUpdate.Text=Lan.g(this,"Add");
				textAmt.Text="";
				textDate.Text="";
				textCode.Text=Lan.g(this,"None");
				textNote.Text="";
				textAmt.ReadOnly=false;
				textDate.ReadOnly=false;
				textNote.ReadOnly=false;
			}
			else if(listSelectedEntries.Count==1) { //if there is one entry selected
				PayPlanEntry selectedEntry=listSelectedEntries[0]; //all textboxes should be editable
				textAmt.ReadOnly=false;
				textDate.ReadOnly=false;
				textNote.ReadOnly=false;
				if(selectedEntry.IsChargeOrd) { //if it's a PayPlanCharge
					//button should say Update, text boxes should fill with info from that charge.
					butAddOrUpdate.Text=Lan.g(this,"Update");
					isUpdateButton=true;
					textAmt.Text=selectedEntry.AmtStr;
					textNote.Text=selectedEntry.NoteStr;
					if(selectedEntry.ProcStatOrd==ProcStat.TP && selectedEntry.ProcNumOrd!=0) {//if tp, grey out the date textbox. it should always be maxvalue.
						//tp and procnum==0 means that it's the unattached row, in which case we don't want to make the text boxes ready-only.
						textDate.ReadOnly=true;
						textDate.Text="";
					}
					else {
						textDate.Text=selectedEntry.CredDateStr;
					}
					if(selectedEntry.Proc==null) { //selected charge could be unattached.
						textCode.Text=Lan.g(this,"Unattached");
					}
					else {
						textCode.Text=ProcedureCodes.GetStringProcCode(selectedEntry.Proc.CodeNum);
					}
				}
				else {// selected line item is a procedure (or the "Unattached" entry)
					//button should say "Add", text boxes should fill with info from that procedure (or "unattached").
					butAddOrUpdate.Text=Lan.g(this,"Add");
					if(selectedEntry.Proc==null) {
						textCode.Text=Lan.g(this,"Unattached");
						textAmt.Text="0.00";
						textNote.Text="";
						textDate.Text=DateTimeOD.Today.ToShortDateString();
					}
					else { //if it is a procedure (and not the "unattached" row)
						List<PayPlanEntry> listEntriesForProc=_listPayPlanEntries
						.Where(x => x.ProcNumOrd==selectedEntry.ProcNumOrd)
						.Where(x => x.IsChargeOrd==true).ToList();
						if(listEntriesForProc.Count==0) { //if there are no other charges attached to the procedure
							textAmt.Text=selectedEntry.RemBefStr; //set textAmt to the value in RemBefore
						}
						else {//if there are other charges attached, fill the amount textbox with the minimum value in the RemAftr column.
							textAmt.Text=listEntriesForProc.Min(x => PIn.Double(x.RemAftStr)).ToString("f");
						}
						textDate.Text=DateTimeOD.Today.ToShortDateString();
						textNote.Text=ProcedureCodes.GetStringProcCode(selectedEntry.Proc.CodeNum)+": "+Procedures.GetDescription(selectedEntry.Proc);
						textCode.Text=ProcedureCodes.GetStringProcCode(selectedEntry.Proc.CodeNum);
					}
				}
			}
			else if(listSelectedEntries.Count>1) { //if they selected multiple line items
				//change the button to say "add"
				//blank out and make read-only all text boxes.
				butAddOrUpdate.Text=Lan.g(this,"Add");
				textAmt.Text="";
				textDate.Text="";
				textNote.Text="";
				textCode.Text=Lan.g(this,"Multiple");
				textAmt.ReadOnly=true;
				textDate.ReadOnly=true;
				textNote.ReadOnly=true;
			}
			if(listSelectedEntries.Any(x => Security.IsGlobalDateLock(Permissions.PayPlanEdit,x.DateOrd,true))) {
				if(isUpdateButton) {
					butAddOrUpdate.Enabled=false;//only disallow them from updating a tx credit, adding a new one is okay. 
				}
				else {
					butAddOrUpdate.Enabled=true;
				}
				butDelete.Enabled=false;
			}
			else {
				butAddOrUpdate.Enabled=true;
				butDelete.Enabled=true;
			}
		}

		private void gridMain_MouseUp(object sender,MouseEventArgs e) {
			SetTextBoxes();
		}

		private void butClear_Click(object sender,EventArgs e) {
			gridMain.SetSelected(false);
			SetTextBoxes();
		}

		private void butAddOrUpdate_Click(object sender,EventArgs e) {
			List<PayPlanEntry> listSelectedEntries=new List<PayPlanEntry>();
			for(int i=0;i < gridMain.SelectedIndices.Count();i++) { //add all of the currently selected entries to this list.
				listSelectedEntries.Add((PayPlanEntry)(gridMain.Rows[gridMain.SelectedIndices[i]].Tag));
			}
			if(listSelectedEntries.Count<=1) { //validation (doesn't matter if multiple are selected)
				if(String.IsNullOrEmpty(textAmt.Text) || textAmt.errorProvider1.GetError(textAmt)!="" || PIn.Double(textAmt.Text)==0) {
					MsgBox.Show(this,"Please enter a valid amount.");
					return;
				}
				if(textDate.Text!="" && textDate.errorProvider1.GetError(textDate)!="") {
					MsgBox.Show(this,"Please enter a valid date.");
					return;
				}
			}
			if(textDate.Text=="") {
				textDate.Text=DateTime.Today.ToShortDateString();
			}
			if(Security.IsGlobalDateLock(Permissions.PayPlanEdit,PIn.Date(textDate.Text))) {
				return;
			}
			if(listSelectedEntries.Count==0) { //if they have none selected
				//add an unattached charge.
				PayPlanCharge addCharge=new PayPlanCharge() {
					ChargeDate=PIn.Date(textDate.Text),
					ChargeType=PayPlanChargeType.Credit,
					Guarantor=_patCur.PatNum,//credits should always appear on the patient of the payment plan.
					Note=PIn.String(textNote.Text),
					PatNum=_patCur.PatNum,
					PayPlanNum=_payPlanCur.PayPlanNum,
					Principal=PIn.Double(textAmt.Text),
					ProcNum=0,
					//provider/clinic will be set when the amortization schedule is saved. FormPayPlan.SaveData()
					//ClinicNum=0,
					//ProvNum=0,
				};
				ListPayPlanCreditsCur.Add(addCharge);
			}
			else if(listSelectedEntries.Count==1) { //if they have one selected
				PayPlanEntry selectedEntry=listSelectedEntries[0];
				if(selectedEntry.IsChargeOrd) { //if it's a charge
					//update the charge selected. get info from text boxes.
					//DO NOT use PayPlanChargeNum. They are not pre-inserted so they will all be 0 if new.
					PayPlanCharge selectedCharge=((PayPlanEntry)(gridMain.Rows[gridMain.SelectedIndices[0]].Tag)).Charge;
					selectedCharge.Principal=PIn.Double(textAmt.Text);
					selectedCharge.Note=PIn.String(textNote.Text);
					if(selectedEntry.ProcStatOrd==ProcStat.TP && selectedEntry.ProcNumOrd!=0) { //if it's treatment planned, save the date as maxvalue so it will not show up in the ledger.
						//if it doesn't have a procnum, then we are editing an unattached row.
						selectedCharge.ChargeDate=DateTime.MaxValue;
					}
					else {
						selectedCharge.ChargeDate=PIn.Date(textDate.Text);
					}
				}
				else { //if it's a procedure
					//add a charge for the selected procedure. get info from text boxes.
					PayPlanCharge addCharge=new PayPlanCharge();
					if(selectedEntry.ProcStatOrd==ProcStat.TP && selectedEntry.ProcNumOrd!=0) {//If tp, maxvalue.
						//if procnum == 0, it's unattached.
						addCharge.ChargeDate=DateTime.MaxValue;
					}
					else {
						addCharge.ChargeDate=PIn.Date(textDate.Text);
					}
					addCharge.ChargeType=PayPlanChargeType.Credit;
					addCharge.Guarantor=_patCur.PatNum;//credits should always appear on the patient of the payment plan.
					addCharge.Note=PIn.String(textNote.Text);
					addCharge.PatNum=_patCur.PatNum;
					addCharge.PayPlanNum=_payPlanCur.PayPlanNum;
					addCharge.Principal=PIn.Double(textAmt.Text);
					addCharge.ProcNum=selectedEntry.ProcNumOrd;
					//provider/clinic will be set when the amortization schedule is saved. FormPayPlan.SaveData()
					//ClinicNum=0,
					//ProvNum=0,
					ListPayPlanCreditsCur.Add(addCharge);
				}
			}
			else if(listSelectedEntries.Count>1) { //if they have more than one entry selected
				//remove everythig that doesn't have a procnum from the list
				List<PayPlanEntry> listSelectedProcs=listSelectedEntries.Where(x => !x.IsChargeOrd).Where(x => x.Proc != null).ToList();
				if(listSelectedEntries.Count==0) { //if the list is then empty, there's nothing to do.
					MsgBox.Show(this,"You must have at least one procedure selected.");
					return;
				}
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel, 
					"Add a payment plan credit for each of the selected procedure's remaining amount?  Selected credits will be ignored.")) {
					return;
				}
				//add a charge for every selected procedure for the amount remaining.
				//don't allow adding $0.00 credits.
				foreach(PayPlanEntry entryProcCur in listSelectedProcs) {
					List<PayPlanEntry> listEntriesForProc=_listPayPlanEntries
						.Where(x => x.ProcNumOrd==entryProcCur.ProcNumOrd)
						.Where(x => x.IsChargeOrd==true).ToList();
					PayPlanCharge addCharge=new PayPlanCharge();
					if(entryProcCur.ProcStatOrd==ProcStat.TP) {//If tp, maxvalue.
						addCharge.ChargeDate=DateTime.MaxValue;
					}
					else {
						addCharge.ChargeDate=DateTimeOD.Today;
					}
					addCharge.ChargeType=PayPlanChargeType.Credit;
					addCharge.Guarantor=_patCur.PatNum;//credits should always appear on the patient of the payment plan.
					addCharge.Note=ProcedureCodes.GetStringProcCode(entryProcCur.Proc.CodeNum)+": "+Procedures.GetDescription(entryProcCur.Proc);
					addCharge.PatNum=_patCur.PatNum;
					addCharge.PayPlanNum=_payPlanCur.PayPlanNum;
					addCharge.Principal=PIn.Double(entryProcCur.RemBefStr);
					if(listEntriesForProc.Count!=0) {
						addCharge.Principal=listEntriesForProc.Min(x => PIn.Double(x.RemAftStr));
					}
					addCharge.ProcNum=entryProcCur.ProcNumOrd;
					//provider/clinic will be set when the amortization schedule is saved. FormPayPlan.SaveData()
					//ClinicNum=0,
					//ProvNum=0,
					if(addCharge.Principal>0) {
						ListPayPlanCreditsCur.Add(addCharge);
					}
				}
			}
			textAmt.Text="";
			textDate.Text="";
			textNote.Text="";
			FillGrid();
			SetTextBoxes();
		}

		private void checkHideUnattached_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
			SetTextBoxes();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			pagesPrinted=0;
			PrintDocument pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			pd.DefaultPageSettings.Landscape=true;//If we are including custom tracking, print in landscape mode.
			if(pd.DefaultPageSettings.PrintableArea.Height==0) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			headingPrinted=false;
			try {
#if DEBUG
				FormRpPrintPreview pView=new FormRpPrintPreview();
				pView.printPreviewControl2.Document=pd;
				pView.ShowDialog();
#else
					if(PrinterL.SetPrinter(pd,PrintSituation.Default,0,"Outstanding insurance report printed")) {
						pd.Print();
					}
#endif
			}
			catch {
				MessageBox.Show(Lan.g(this,"Printer not available"));
			}
		}

		private void pd_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
			//new Rectangle(50,40,800,1035);//Some printers can handle up to 1042
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			if(!headingPrinted) {
				text=Lan.g(this,"Payment Plan Credits");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				text=DateTime.Today.ToShortDateString();
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				text=_patCur.LName+", "+_patCur.FName;
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=20;
				headingPrinted=true;
				headingPrintH=yPos;
			}
			#endregion
			yPos=gridMain.PrintPage(g,pagesPrinted,bounds,headingPrintH);
			pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
				text=Lan.g(this,"Total")+": "+PIn.Double(textTotal.Text).ToString("c");
				g.DrawString(text,subHeadingFont,Brushes.Black,center+gridMain.Width/2-g.MeasureString(text,subHeadingFont).Width-10,yPos);
			}
			g.Dispose();
		}

		private void checkShowImplicit_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			List<PayPlanEntry> listSelectedEntries=new List<PayPlanEntry>();
			for(int i=0;i < gridMain.SelectedIndices.Count();i++) {
				listSelectedEntries.Add((PayPlanEntry)(gridMain.Rows[gridMain.SelectedIndices[i]].Tag));
			}
			List<PayPlanCharge> listSelectedCharges=listSelectedEntries.Where(x => x.Charge != null).Select(x => x.Charge).ToList();
			//remove all procedures from the list. you cannot delete procedures from here.
			if(listSelectedCharges.Count<1) {
				MsgBox.Show(this,"You must have at least one payment plan charge selected.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Delete selected payment plan charges?")) {
				return;
			}
			foreach(PayPlanCharge chargeCur in listSelectedCharges) {
				ListPayPlanCreditsCur.Remove(chargeCur);
			}
			FillGrid();
			SetTextBoxes();
		}

		private void butOK_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


		///<summary>Same as private class in PaySplitManager.</summary>
		private class AccountEntry {
			private static long AccountEntryAutoIncrementValue=1;
			///<summary>No matter which constructor is used, the AccountEntryNum will be unique and automatically assigned.</summary>
			public long AccountEntryNum=(AccountEntryAutoIncrementValue++);
			//Read only data.  Do not modify, or else the historic information will be changed.
			public object Tag;
			public DateTime Date;
			public long PriKey;
			public long ProvNum;
			public long ClinicNum;
			public long PatNum;
			public decimal AmountOriginal;
			//Variables below will be changed as needed.
			public decimal AmountStart;
			public decimal AmountEnd;
			public List<PaySplit> ListPaySplits=new List<PaySplit>();//List of paysplits for this charge.

			public new Type GetType() {
				return Tag.GetType();
			}

			public AccountEntry(PayPlanCharge payPlanCharge) {
				Tag=payPlanCharge;
				Date=payPlanCharge.ChargeDate;
				PriKey=payPlanCharge.PayPlanChargeNum;
				AmountOriginal=(decimal)payPlanCharge.Principal+(decimal)payPlanCharge.Interest;
				AmountStart=AmountOriginal;
				AmountEnd=AmountOriginal;
				ProvNum=payPlanCharge.ProvNum;
				ClinicNum=payPlanCharge.ClinicNum;
				PatNum=payPlanCharge.PatNum;
			}

			///<summary>Turns negative adjustments positive.</summary>
			public AccountEntry(Adjustment adjustment) {
				Tag=adjustment;
				Date=adjustment.AdjDate;
				PriKey=adjustment.AdjNum;
				AmountOriginal=(decimal)adjustment.AdjAmt;
				AmountStart=AmountOriginal;
				AmountEnd=AmountOriginal;
				ProvNum=adjustment.ProvNum;
				ClinicNum=adjustment.ClinicNum;
				PatNum=adjustment.PatNum;
			}

			public AccountEntry(Procedure proc) {
				Tag=proc;
				Date=proc.ProcDate;
				PriKey=proc.ProcNum;
				AmountOriginal=(decimal)proc.ProcFee;
				AmountStart=(decimal)proc.ProcFee*Math.Max(1,proc.BaseUnits+proc.UnitQty);
				AmountEnd=AmountOriginal;
				if(proc.ProcStatus==ProcStat.TP) {
					AmountStart-=(decimal)proc.Discount;
					AmountEnd-=(decimal)proc.Discount;
				}
				ProvNum=proc.ProvNum;
				ClinicNum=proc.ClinicNum;
				PatNum=proc.PatNum;
			}
		}

		///<summary>Private class for ordering and displaying line items in FormPayPlanCredits.</summary>
		private class PayPlanEntry {
			//ordering fields
			public long ProcNumOrd;
			public DateTime DateOrd;
			public double AmtOrd;
			public bool IsChargeOrd;
			public ProcStat ProcStatOrd;
			//visible fields
			public string DateStr="";
			public string PatStr="";
			public string StatStr="";
			public string ProcStr="";
			public string FeeStr="";
			public string RemBefStr="";
			public string CredDateStr="";
			public string AmtStr="";
			public string RemAftStr="";
			public string NoteStr="";
			//other fields
			///<summary>Stores the procedure associated to the payplanentry. Null if none.</summary>
			public Procedure Proc;
			///<summary>If a charge, stores the payplancharge associated. Null if a procedure.</summary>
			public PayPlanCharge Charge;
		}
	}
}
