using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	///<summary>This form allows users to add adjustments to multiple procedures and does line item accounting for those adjustments. 
	///If you are modifying this form make sure you have a strong understanding of the private class it utilizes. 
	///MultiAdjEntry is crucial to almost all logical operations in this form.</summary>
	public partial class FormAdjMulti:ODForm {
		#region Private Fields
		//Only set if _isSelectionMode is true. This is used to sync _listGridEntries with the procedures selected in the account module.
		private List<Procedure> _listSelectedProcs;
		private List<MultiAdjEntry> _listGridEntries=new List<MultiAdjEntry>();
		private Patient _patCur;
		private List<Provider> _listProviders;
		private List<Clinic> _listClinics;
		private bool _isSelectionMode;
		private List<Def> _listAdjPosCats;
		private List<Def> _listAdjNegCats;
		private int _rigorousAdjustments;
		#endregion
		
		///<summary>Pass in a list to only display certain procedures.  If none are provided, we will dynamically get unpaid procedures</summary>
		public FormAdjMulti(Patient patCur, List<Procedure> selectedProcs=null) {
			InitializeComponent();
			Lan.F(this);
			_patCur=patCur;
			_listSelectedProcs=selectedProcs;
			if(selectedProcs!=null && selectedProcs.Count>0) {//The user specified procedures
				_isSelectionMode=true;
			}
			else {
				_isSelectionMode=false;
			}
		}
		
		private void FormMultiAdj_Load(object sender,EventArgs e) {
			dateAdjustment.Text=DateTime.Today.ToShortDateString();
			_rigorousAdjustments=PrefC.GetInt(PrefName.RigorousAdjustments);
			FillListBoxAdjTypes();
			FillComboProv();
			FillComboClinics();
			//Must happen after comboboxes are filled
			if(_rigorousAdjustments==0) {
				comboProv.Enabled=false;
				comboClinic.Enabled=false;
				butPickProv.Enabled=false;
				butPickClinic.Enabled=false;
			}
			if(_rigorousAdjustments==(int)RigorousAdjustments.DontEnforce) {
				radioIncludeAll.Checked=true;
				creditFilterRefresh();
			}
			else {
				radioAllocatedOnly.Checked=true;
				creditFilterRefresh();
			}
			FillGrid();
			if(_isSelectionMode) {
				foreach(Procedure selectedProc in _listSelectedProcs) {
					gridMain.SetSelected(_listGridEntries.FindIndex(x => x.Proc.ProcNum==selectedProc.ProcNum),true);
				}
			}
			textAmt.Select();//start cursor here
		}

		#region Fill Methods
		///<summary>This method converts the AccountEntry objects we get back from AccountModules.GetListUnpaidAccountCharges() to MultiAdjEntry objects.
		///These are used to fill the grid and do all relevant logic in this form. This method will return a fresh list of procedures and will not show
		///any existing adjustments that may have been made in this form already. Called on load and when checkShowImplicit is clicked.
		///When called from checkShowImplicit any existing adjustments made will not be shown.(Ask Andrew about this functionality)</summary>
		private List<MultiAdjEntry> FillListGridEntries(CreditCalcType calc) {
			List<MultiAdjEntry> retVal=new List<MultiAdjEntry>();
			#region Get required data
			List<Procedure> listProcedures=Procedures.GetCompleteForPats(new List<long> { _patCur.PatNum });
			//Does not get charges for the future.
			List<PaySplit> listPaySplits=PaySplits.GetForPats(new List<long> { _patCur.PatNum }).Where(x => x.UnearnedType==0).ToList();
			List<Adjustment> listAdjustments=Adjustments.GetAdjustForPats(new List<long> { _patCur.PatNum });
			//Might contain payplan payments. Do not include unearned.
			List<PayPlanCharge> listPayPlanCharges=PayPlanCharges.GetDueForPayPlans(PayPlans.GetForPats(null,_patCur.PatNum),_patCur.PatNum).ToList();
			List<ClaimProc> listInsPayAsTotal=ClaimProcs.GetByTotForPats(new List<long> { _patCur.PatNum });
			List<ClaimProc> listClaimProcs=ClaimProcs.GetForProcs(listProcedures.Select(x => x.ProcNum).ToList());
			#endregion
			List<AccountEntry> listAccountCharges=AccountModules.GetListUnpaidAccountCharges(listProcedures, listAdjustments
				,listPaySplits, listClaimProcs, listPayPlanCharges, listInsPayAsTotal, calc, new List<PaySplit>());
			List<AccountEntry> listAccountChargesIncludeAll=null;
			if(calc==CreditCalcType.ExcludeAll) {
				//We need to get all credits so that our AmtRemBefore can reflect what has truly been allocated to the procedure.
				listAccountChargesIncludeAll=AccountModules.GetListUnpaidAccountCharges(listProcedures,listAdjustments
				,listPaySplits,listClaimProcs,listPayPlanCharges,listInsPayAsTotal,CreditCalcType.IncludeAll,new List<PaySplit>());
			}
			MultiAdjEntry multiEntry=null;
			foreach(AccountEntry entry in listAccountCharges) {
				//We only want AccountEntries that are completed procedures.
				if(entry.GetType()!=typeof(ProcExtended)) {
					continue;
				}
				ProcExtended procEntry=(ProcExtended)entry.Tag;
				if((_listSelectedProcs!=null && _listSelectedProcs.Any(x => x.ProcNum==procEntry.Proc.ProcNum))//Allow selected procs to show if paid off
					|| entry.AmountEnd!=0)//All unpaid procedures should always show up per Nathan
				{
					double amtRemBefore=(double)(listAccountChargesIncludeAll?.FirstOrDefault(x =>x.Tag.GetType()==typeof(ProcExtended)
						&& ((ProcExtended)x.Tag).Proc.ProcNum==procEntry.Proc.ProcNum)??entry).AmountStart;
					multiEntry=new MultiAdjEntry(procEntry.Proc,(double)entry.AmountStart,(double)entry.AmountEnd,amtRemBefore);
					retVal.Add(multiEntry);
				}
			}
			OrderListGridEntries();
			return retVal;
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g("TableMultiAdjs","Date"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableMultiAdjs","Provider"),90);
			gridMain.Columns.Add(col);
			if(PrefC.HasClinicsEnabled) {
				col=new ODGridColumn(Lan.g("TableMultiAdjs","Clinic"),90);
				gridMain.Columns.Add(col);		
			}		 	 
			col=new ODGridColumn(Lan.g("TableMultiAdjs","Type"),120);
			gridMain.Columns.Add(col);			 
			col=new ODGridColumn(Lan.g("TableMultiAdjs","Fee"),70,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);			 
			col=new ODGridColumn(Lan.g("TableMultiAdjs","Rem Before"),70,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);			 		 
			col=new ODGridColumn(Lan.g("TableMultiAdjs","Adj Amt"),70,HorizontalAlignment.Left);
			gridMain.Columns.Add(col);			 
			col=new ODGridColumn(Lan.g("TableMultiAdjs","Rem After"),70,HorizontalAlignment.Left);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			RecalculateGridEntries();
			foreach(MultiAdjEntry entryCur in _listGridEntries) {
				ODGridRow row=entryCur.ToGridRow();
				row.Tag=entryCur;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			butAdd.Text=Lan.g(this,"Add Adjustments");
		}

		private void FillListBoxAdjTypes() {
			//Remove hidden adjustment types
			List<Def> adjCat=Defs.GetCatList((int)DefCat.AdjTypes).ToList().FindAll(x => !x.IsHidden);
			//Positive adjustment types
			_listAdjPosCats=adjCat.FindAll(x => x.ItemValue=="+");
			_listAdjPosCats.ForEach(x => listTypePos.Items.Add(x.ItemName));
			//Negativate adjustment types
			_listAdjNegCats=adjCat.FindAll(x => x.ItemValue=="-");
			_listAdjNegCats.ForEach(x => listTypeNeg.Items.Add(x.ItemName));
		}

		private void FillComboProv() {
			comboProv.Items.Clear();
			comboProv.Items.Add(new ODBoxItem<Provider>("Inherit"));//Inherit was carefully approved by Nathan (and reluctantly Allen)
			_listProviders=Providers.GetDeepCopy(true);
			foreach(Provider prov in _listProviders) {
				comboProv.Items.Add(new ODBoxItem<Provider>(prov.Abbr,prov));//Don't need to lan.g prov names
			}
			comboProv.SelectedIndex=0;
		}

		private void FillComboClinics() {
			if(PrefC.HasClinicsEnabled) {
				comboClinic.Items.Clear();
				comboClinic.Items.Add(new ODBoxItem<Clinic>("Inherit"));//Inherit was carefully approved by Nathan (and reluctantly Allen)
				_listClinics=Clinics.GetDeepCopy(true);
				foreach(Clinic clin in _listClinics) {
					comboClinic.Items.Add(new ODBoxItem<Clinic>(Lan.g(this,clin.Abbr),clin));
				}
				comboClinic.SelectedIndex=0;
			}
			else {
				comboClinic.Visible=false;
				butPickClinic.Visible=false;
				labelClinic.Visible=false;
			}
		}
		#endregion

		#region Helper Methods
		private void OrderListGridEntries() {
			//This ordering by assumes that there should only be exactly one model with a null adjustment per procedure. (the head procedure row)
			//And a null procedure means we are dealing with an unattached adjustment
			_listGridEntries=_listGridEntries.OrderBy(x => x.Proc==null)
				.ThenBy(x => x.Proc!=null ? x.Proc.ProcNum : 0)//If no Proc (unattached adj) then ordering is taken care of later.
				.ThenBy(x => x.Adj!=null ? x.Adj.AdjDate : DateTime.MinValue)
				.ThenBy(x => x.Adj!=null ? x.AdjustmentEntryNum : 0).ToList();//Unattached adjustments will get ordered here.
		}

		private void InsertUnattachedRow() {
			//Only insert the Unattached row if we do not already have unattached adjustments in the grid
			if(_listGridEntries.Any(x => x.Proc==null)) {
				return;
			}
			//We are just adding a dummy row header to help the UI look a little cleaner.
			_listGridEntries.Add(new MultiAdjEntry(null,null,0,0));
		}

		private AdjAmtType SetAdjAmtType() {
			if(radioFixedAmt.Checked) {
				return AdjAmtType.FixedAmt;
			}
			else if(radioPercentRemBal.Checked) {
				return AdjAmtType.PercentOfRemBal;
			}
			else {
				return AdjAmtType.PercentOfFee;
			}
		}

		///<summary>This method will recalculate the RemAfter variable for each MultiAdjEntry in the grid. This is required for line item accounting.
		///If there are no unattached adjustments this method will remove the "Unattached" row, as well.</summary>
		private void RecalculateGridEntries() {
			//Remove the "Unattached" row if there are no unattached adjustments in the grid
			if(_listGridEntries.Count(x => x.Proc==null && x.Adj!=null)==0) {//There are no unattached adjustments
				_listGridEntries.RemoveAll(x => x.IsUnattachedRowHeader());//Remove the header row for unattached adjustments
			}
			//Rename the grid when there are no adjustments
			if(_listGridEntries.Count(x => x.Adj!=null)==0) {
				gridMain.Title="Available Procedures";
			}
			#region Recalculate RemAfter for each grid entry
			_listGridEntries.FindAll(x => x.Proc==null && x.Adj!=null).ForEach(x => x.RecalculateAdjAmt(0));//Manually recalculate all unattached adjs.  They should all be FixedAmt
			foreach(MultiAdjEntry procEntry in _listGridEntries) {
				if(procEntry.Proc==null || procEntry.Adj!=null) {
					continue;
				}
				//At this point we have a valid proc with no Adj (header procedure)
				List<MultiAdjEntry> listAdjs=_listGridEntries.FindAll(x => x.Proc!=null && x.Proc.ProcNum==procEntry.Proc.ProcNum && x.Adj!=null);
				listAdjs.Sort((x,y) => {
					if(x.Adj.AdjDate.CompareTo(y.Adj.AdjDate)==0) {
						return x.AdjustmentEntryNum.CompareTo(y.AdjustmentEntryNum);//If the dates are the same, sort by AdjustmentEntryNum
					}
					return x.Adj.AdjDate.CompareTo(y.Adj.AdjDate);
				});
				double remAfter=procEntry.RemBefore;
				foreach(MultiAdjEntry adjEntry in listAdjs) {
					//We must recalculate the AdjAmt for each adjustment here because this is the only place that has context of each line item.
					//For instance, if we update multiple procedures at a time, we would have to re-loop through each adjustment like we do here
					//to ensure that the percents are correct.
					adjEntry.RecalculateAdjAmt(remAfter);
					remAfter+=adjEntry.Adj.AdjAmt;
					adjEntry.RemAfter=remAfter;
				}
				procEntry.RemAfter=remAfter;
			}
			#endregion
			OrderListGridEntries();
		}

		///<summary>Updates a selected row with the user selected values. Returns the new RemAfter value for the passed in procedure.</summary>
		private MultiAdjEntry UpdateAdjValues(MultiAdjEntry row) {
			Def selectedAdjType;
			if(listTypePos.SelectedIndex!=-1) {
				selectedAdjType=_listAdjPosCats[listTypePos.SelectedIndex];
			}
			else{//Nothing was selected in listTypePos so there has to be a selection for negative.
				selectedAdjType=_listAdjNegCats[listTypeNeg.SelectedIndex];
			}
			//set prov
			if(((ODBoxItem<Provider>)comboProv.SelectedItem).Text=="Inherit") {//Inherit was carefully approved by Nathan (and reluctantly Allen)
				if(row.Proc!=null) {
						row.Adj.ProvNum=row.Proc.ProvNum;
				}
			}
			else {
				row.Adj.ProvNum=((ODBoxItem<Provider>)comboProv.SelectedItem).Tag.ProvNum;
			}
			//set clinic
			long selectedClinicNum=0;
			if(PrefC.HasClinicsEnabled) {
				if(((ODBoxItem<Clinic>)comboClinic.SelectedItem).Text=="Inherit") {
					if(row.Proc!=null) {
						selectedClinicNum=row.Proc.ClinicNum; 
					}
				}
				else {
					selectedClinicNum=((ODBoxItem<Clinic>)comboClinic.SelectedItem).Tag.ClinicNum;
				}
			}
			row.Adj.AdjType=selectedAdjType.DefNum;
			row.Adj.ClinicNum=selectedClinicNum;
			row.Adj.AdjDate=PIn.Date(dateAdjustment.Text);
			row.Adj.AdjNote=PIn.String(textNote.Text);
			row.Adj.PatNum=_patCur.PatNum;
			if(row.Proc==null) {//Unassigned adjustments have to be fixed amounts, or else they will be 0.
				row.AdjAmtType=AdjAmtType.FixedAmt;
			}
			else {
				row.AdjAmtType=SetAdjAmtType();
			}
			row.AdjAmtOrPerc=PIn.Double(textAmt.Text);
			return row;
		}

		///<summary>Checks to verify that the user does not try to add an adjustment and update an adjustment at the same time.
		///True if the user has an adjustment and procedure row selected, false if not.</summary>
		private bool HasProcAndAdjSelected(List<MultiAdjEntry> listRows) {
			bool hasProcRowSelected=listRows.Count(x => x.Proc!=null && x.Adj==null)>0;
			bool hasAdjRowSelected=listRows.Count(x => x.Adj!=null)>0;
			if(hasProcRowSelected && hasAdjRowSelected) {
				return true;
			}
			return false;
		}
		#endregion

		#region Events

		private void butPickProv_Click(object sender,EventArgs e) {
			FormProviderPick FormProvPick=new FormProviderPick(_listProviders);
			if(FormProvPick.ShowDialog()==DialogResult.OK) {
				comboProv.SelectedIndex=-1;
				foreach(ODBoxItem<Provider> boxItemProvCur in comboProv.Items.OfType<ODBoxItem<Provider>>()) {
					if(boxItemProvCur.Tag.ProvNum == FormProvPick.SelectedProvNum) {
						comboProv.SelectedItem = boxItemProvCur;
						break;
					}
				}
			}
		}

		private void butPickClinic_Click(object sender,EventArgs e) {
			FormClinics form=new FormClinics();
			form.ListClinics=_listClinics;
			form.IsSelectionMode=true;
			form.IsMultiSelect=false;
			if(form.ShowDialog()==DialogResult.OK) {
				comboClinic.SelectedIndex=-1;
				for(int i=0;i<comboClinic.Items.Count;i++) {
					if(((ODBoxItem<Clinic>)(comboClinic.Items[i])).Tag.ClinicNum==form.SelectedClinicNum) {
						comboClinic.SelectedIndex=i;
						break;
					}
				}
			}
		}

		private void listTypePos_SelectedIndexChanged(object sender,System.EventArgs e) {
			if(listTypePos.SelectedIndex>-1) {
				listTypeNeg.SelectedIndex=-1;
			}
		}

		private void listTypeNeg_SelectedIndexChanged(object sender,System.EventArgs e) {
			if(listTypeNeg.SelectedIndex>-1) {
				listTypePos.SelectedIndex=-1;
			}
		}

		private void radioCredits_Click(object sender,EventArgs e) {
			creditFilterRefresh();
		}

		private void creditFilterRefresh() {
			CreditCalcType selectedType;
			if(radioAllocatedOnly.Checked) {
				selectedType=CreditCalcType.AllocatedOnly;
			}
			else if(radioIncludeAll.Checked) {
				selectedType=CreditCalcType.IncludeAll;
			}
			else {
				selectedType=CreditCalcType.ExcludeAll;
			}
			List<MultiAdjEntry> listProcs=FillListGridEntries(selectedType);			
			if(_listGridEntries.Where(x => x.Adj!=null)//They have made an adjustment row for this procedure
				.Any(x => x.Proc!=null && !x.Proc.ProcNum.In(listProcs.Where(y => y.Proc!=null).Select(y => y.Proc.ProcNum)))//The procedure is no longer in the list
				&& !MsgBox.Show(this,true,"This will clear out any adjustments you have added. Are you sure you want to continue?")) 
			{
				//When going from ExplicitOnly to FIFO we may lose procedures that currently exist in _listGridEntries.
				//This becomes problematic when the user has entered adjustments that have not been entered in the DB yet.
				//We will warn the user here that by unchecking the checkbox they will lose all adjustments they may have entered.
				//The reason for this is that it keeps our logic simple so that we can simply reset
				//the _listGridEntries to the entries returned by FillListGridEntries().
				radioExcludeAll.Checked=true;//This shows all procedures on the account
				return;
			}
			foreach(MultiAdjEntry entryCur in _listGridEntries) {
				//We only keep adjustments where the procedure is in the list
				if(entryCur.Adj==null || entryCur.Adj.ProcNum==0) {
					continue;
				}
				if(entryCur.Proc.ProcNum.In(listProcs.Where(x => x.Proc!=null).Select(x => x.Proc.ProcNum))) {
					listProcs.Add(entryCur);
				}
			}
			_listGridEntries=listProcs;
			OrderListGridEntries();
			FillGrid();
		}

		private void radioFixedAmt_CheckedChanged(object sender,EventArgs e) {
			if(radioFixedAmt.Checked) {
				labelAmount.Text="Amount";
			}
			else {
				labelAmount.Text="Percent";
			}
		}

		private void butAdd_Click(object sender,EventArgs e) {
			List<MultiAdjEntry> listSelectedEntries=new List<MultiAdjEntry>();			
			for(int i=0;i<gridMain.SelectedIndices.Count();i++) {
				listSelectedEntries.Add((MultiAdjEntry)(gridMain.Rows[gridMain.SelectedIndices[i]].Tag));
			}
			#region User Input Validation
			if(listSelectedEntries.Count()==0 && _rigorousAdjustments==0) {
				MsgBox.Show(this,"You must select a procedure to add the adjustment to.");
				return;
			}
			if(listTypePos.SelectedIndex==-1 && listTypeNeg.SelectedIndex==-1){
				MsgBox.Show(this,"Please pick an adjustment type.");
				return;
			}
			if(comboProv.SelectedIndex==-1){
				MsgBox.Show(this,"Please pick a provider.");
				return;
			}
			if(PrefC.HasClinicsEnabled && comboClinic.SelectedIndex==-1){
				MsgBox.Show(this,"Please pick a clinic.");
				return;
			}
			if(string.IsNullOrWhiteSpace(dateAdjustment.Text) || dateAdjustment.errorProvider1.GetError(dateAdjustment)!="") {
				MsgBox.Show(this,"Please enter a valid date.");
				return;
			}
			if(PIn.Date(dateAdjustment.Text) > DateTime.Today && !PrefC.GetBool(PrefName.FutureTransDatesAllowed)) {
				MsgBox.Show(this,"Adjustments cannot be made for future dates");
				return;
			}
			if(string.IsNullOrWhiteSpace(textAmt.Text) || textAmt.errorProvider1.GetError(textAmt)!=""){
				MsgBox.Show(this,"Please enter a valid amount.");
				return;
			}
			#endregion
			//loop through and update all adjustment rows
			foreach(MultiAdjEntry adjRow in listSelectedEntries.FindAll(x => x.Adj!=null)) {
				UpdateAdjValues(adjRow);
			}
			Def selectedAdjType;
			if(listTypePos.SelectedIndex!=-1) {
				selectedAdjType=_listAdjPosCats[listTypePos.SelectedIndex];
			}
			else{
				selectedAdjType=_listAdjNegCats[listTypeNeg.SelectedIndex];
			}
			//Create new adjustment
			Adjustment adjCur=new Adjustment();
			adjCur.AdjType=selectedAdjType.DefNum;
			adjCur.AdjDate=PIn.Date(dateAdjustment.Text);
			adjCur.AdjNote=PIn.String(textNote.Text);
			adjCur.PatNum=_patCur.PatNum;
			if(listSelectedEntries.Count==0) {//User did not select anything and hit "add", so they must want to add an unattached adjustment.
				InsertUnattachedRow();
				adjCur.ProvNum=_patCur.PriProv;
				adjCur.ClinicNum=_patCur.ClinicNum;
				adjCur.ProcDate=adjCur.AdjDate;
				adjCur.ProcNum=0;
				MultiAdjEntry adjRow=new MultiAdjEntry(adjCur,adjCur.AdjAmt);
				adjRow.AdjAmtType=AdjAmtType.FixedAmt;
				adjRow.AdjAmtOrPerc=PIn.Double(textAmt.Text);
				adjRow.RecalculateAdjAmt(0);
				_listGridEntries.Add(adjRow);
			}
			else {//One or more procedure rows selected, create adjustments
				foreach(MultiAdjEntry procRow in listSelectedEntries.FindAll(x => x.IsProcedureRow())) {//Loop through all selected procedures
					Adjustment adj=adjCur.Clone();
					adj.ProcDate=procRow.Proc.ProcDate;
					adj.ProcNum=procRow.Proc.ProcNum;
					MultiAdjEntry adjRow=new MultiAdjEntry(procRow.Proc,adj,procRow.RemAfter,procRow.AmtRemBefore);
					adjRow=UpdateAdjValues(adjRow);//This will set all of the important values from UI selections
					adjRow.AdjAmtType=SetAdjAmtType();
					adjRow.AdjAmtOrPerc=PIn.Double(textAmt.Text);
					_listGridEntries.Add(adjRow);
				}
			}
			gridMain.Title="Available Procedures with Adjustments";
			FillGrid();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			for(int i=0;i<gridMain.SelectedIndices.Count();i++) {
				MultiAdjEntry selectedRow=(MultiAdjEntry)gridMain.Rows[gridMain.SelectedIndices[i]].Tag;
				if(selectedRow.Adj!=null) {//user selected an adjustment row
					if(selectedRow.Proc==null) {//unattached adjustment
						_listGridEntries.Remove(selectedRow);
						continue;
					}
					_listGridEntries.RemoveAll(x => x.AdjustmentEntryNum==selectedRow.AdjustmentEntryNum);//Should only be one, but will clean up accidental duplicates if a bug occurs
				}
			}
			butAdd.Enabled=true;
			FillGrid();
		}
		
		private void gridMain_MouseUp(object sender,MouseEventArgs e) {
			List<MultiAdjEntry> listSelectedEntries=new List<MultiAdjEntry>();
			for(int i=0;i < gridMain.SelectedIndices.Count();i++) { //fill the list with all the selected items in the grid.
				listSelectedEntries.Add((MultiAdjEntry)(gridMain.Rows[gridMain.SelectedIndices[i]].Tag));
			}
			butAdd.Enabled=true;
			if(listSelectedEntries.Count==0) { //if there are no entries selected
				butAdd.Text=Lan.g(this,"Add Adjustments");
			}
			else if(listSelectedEntries.Count==1) {
				if(listSelectedEntries[0].Adj!=null) {//user selected an adjustment row
					butAdd.Text=Lan.g(this,"Update");
					textNote.Text=listSelectedEntries[0].Adj.AdjNote;
				}
				else {//Selected a procedure row
					butAdd.Text=Lan.g(this,"Add Adjustments");
				}
			}
			else {//Multiple entries selected
				if(HasProcAndAdjSelected(listSelectedEntries)) {
					butAdd.Enabled=false;
				}
				else if(listSelectedEntries.All(x => x.Adj!=null)) {//User has selected multiple adjustments
					butAdd.Text=Lan.g(this,"Update");
				}
				else if(listSelectedEntries.All(x => x.IsProcedureRow())) {//User has selected multiple proc rows
					butAdd.Text=Lan.g(this,"Add Adjustments");
				}
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			bool hasNegAmt=false;
			foreach(MultiAdjEntry mae in _listGridEntries) {
				if(mae.Adj==null) {
					continue;//Procedures
				}
				decimal remAfter=(decimal)mae.AmtRemBefore+(decimal)mae.Adj.AdjAmt;
				if(remAfter.IsLessThanZero()) {
					hasNegAmt=true;
					break;
				}
			}
			if(hasNegAmt && !MsgBox.Show(this,MsgBoxButtons.OKCancel,"Remaining amount on a procedure is negative.  Continue?","Overpaid Procedure Warning")){
				return;
			}
			foreach(MultiAdjEntry row in _listGridEntries) {
				if(row.Adj==null) {//skip over Procedure rows
					continue;
				}
				Adjustments.Insert(row.Adj);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
		#endregion
	
		///<summary>This is a private class used to simplify grid logic in this form. 
		///This class represents either a procedure row, an adjustment row, or an unattached adjustment row.</summary>
		private class MultiAdjEntry {
			///<summary>The amount remaining before any adjustments in the form are made. This amount does not change.</summary>
			public readonly double AmtRemBefore;
			//Should always have a procedure unless the user creates an unattached adjustment. In that case the proc will be null.
			#region Procedure Fields
			//The associated procedure. Null if the adjustment is unattached.
			public Procedure Proc;
			//These fields are necessary to do line item accounting in this form. As we add adjustments we need to show the new procedure fee value.
			public double RemBefore;
			public double RemAfter;
			#endregion
			#region Adjustment Fields
			private static long _adjustmentEntryAutoIncrementValue=1;
			//No matter which constructor is used, the AdjustmentEntryNum will be unique and automatically assigned. 
			//This is only used for adjustment rows, and is necessary for line item accounting.
			public long AdjustmentEntryNum=(_adjustmentEntryAutoIncrementValue++);
			private List<Def> _listAdjDefs=Defs.GetCatList((int)DefCat.AdjTypes).ToList();
			///<summary>Stores an adjustment (not in the db yet). If this field is null then this entry is a procedure row, 
			///otherwise this entry is an adjustment row.</summary>
			public Adjustment Adj;
			public double AdjAmtOrPerc=0;
			public AdjAmtType AdjAmtType=AdjAmtType.FixedAmt;
			#endregion

			///<summary>This constructor should be called for procedure rows.</summary>
			public MultiAdjEntry(Procedure proc, double remBefore, double remAfter,double amtRemBefore) {
				Proc=proc;
				Adj=null;
				RemBefore=remBefore;
				RemAfter=remAfter;
				AmtRemBefore=amtRemBefore;
			}

			///<summary>This constructor should be called for attached adjustment rows.</summary>
			public MultiAdjEntry(Procedure proc, Adjustment adj, double procFeeAfter,double amtRemBefore) {
				Proc=proc;
				Adj=adj;
				RemBefore=-1;
				RemAfter=procFeeAfter;
				AmtRemBefore=amtRemBefore;
			}

			///<summary>This constructor should only be used for unattached adjustments.</summary>
			public MultiAdjEntry(Adjustment adj, double adjAmt) {
				//We purposely set the associated procedure to null so that we can differentiate between an unattached adjustment and a regular adjustment
				Proc=null;
				Adj=adj;
				RemBefore=-1;
				RemAfter=adjAmt;
			}

			///<summary>Determines whether the current object is an UnattachedRow header</summary>
			public bool IsUnattachedRowHeader() {
				return (Proc==null && Adj==null);
			}

			///<summary>Determines whether the current object is an Procedure header</summary>
			public bool IsProcedureRow() {
				return (Proc!=null && Adj==null);
			}

			public void RecalculateAdjAmt(double remBefore) {
				if(AdjAmtType==AdjAmtType.FixedAmt) {
					Adj.AdjAmt=AdjAmtOrPerc;
				}
				else if(AdjAmtType==AdjAmtType.PercentOfRemBal) {
					Adj.AdjAmt=Math.Round((AdjAmtOrPerc/100)*remBefore,2);
				}
				else if(AdjAmtType==AdjAmtType.PercentOfFee) {
					Adj.AdjAmt=Math.Round((AdjAmtOrPerc/100)*Proc.ProcFee,2);
				}
				if((Defs.GetDef(DefCat.AdjTypes,Adj.AdjType)).ItemValue=="-") {//uses the cache
					Adj.AdjAmt=Adj.AdjAmt*-1;
				}
			}

			public string GetAdjustmentType(Adjustment adj) {
				return _listAdjDefs.Find(x => x.DefNum==adj.AdjType)?.ItemName??"";
			}

			public ODGridRow ToGridRow() {
				ODGridRow row=new ODGridRow();
				ODGridCell cell;
				if(Proc!=null && Adj==null) {//A procedure row
					//Date
					cell=new ODGridCell(Proc.ProcDate.ToShortDateString());
					row.Cells.Add(cell);
					//Provider
					cell=new ODGridCell(Providers.GetAbbr(Proc.ProvNum));
					row.Cells.Add(cell);
					//Clinic
					if(PrefC.HasClinicsEnabled) {
						cell=new ODGridCell(Clinics.GetAbbr(Proc.ClinicNum));
						row.Cells.Add(cell);
					}
					//Code
					cell=new ODGridCell(ProcedureCodes.GetStringProcCode(Proc.CodeNum));
					row.Cells.Add(cell);
					//Fee;
					cell=new ODGridCell(Procedures.CalculateProcCharge(Proc).ToString("c"));
					cell.CellColor=Color.LightYellow;
					row.Cells.Add(cell);
					//Rem Before
					cell=new ODGridCell(RemBefore.ToString("c"));
					cell.CellColor=Color.LightYellow;
					row.Cells.Add(cell);
					//Adj Amt
					cell=new ODGridCell("");
					cell.CellColor=Color.White;
					row.Cells.Add(cell);
					//Rem After
					cell=new ODGridCell("");
					cell.CellColor=Color.White;
					row.Cells.Add(cell);
				}
				else if(IsUnattachedRowHeader()) {//The row header for unattached adjustments
					row.ColorBackG=Color.LightYellow;
					//Date
					cell=new ODGridCell("Unassigned");
					row.Cells.Add(cell);
					//Provider
					cell=new ODGridCell("");
					row.Cells.Add(cell);
					//Clinic
					if(PrefC.HasClinicsEnabled) {
						cell=new ODGridCell("");
						row.Cells.Add(cell);
					}
					//Code
					cell=new ODGridCell("");
					row.Cells.Add(cell);
					//Fee;
					cell=new ODGridCell("");
					row.Cells.Add(cell);
					//Rem Before
					cell=new ODGridCell("");
					row.Cells.Add(cell);
					//Adj Amt
					cell=new ODGridCell("");
					row.Cells.Add(cell);
					//Rem After
					cell=new ODGridCell("");
					row.Cells.Add(cell);
				}
				else {//An adjustment row
					//Adj Date
					cell=new ODGridCell(Adj.AdjDate.ToShortDateString());
					row.Cells.Add(cell);
					//Provider
					cell=new ODGridCell(Providers.GetAbbr(Adj.ProvNum));
					row.Cells.Add(cell);
					//Clinic
					if(PrefC.HasClinicsEnabled) {
						cell=new ODGridCell(Clinics.GetAbbr(Adj.ClinicNum));
						row.Cells.Add(cell);
					}
					//Adj Type
					cell=new ODGridCell(GetAdjustmentType(Adj));
					row.Cells.Add(cell);
					//Fee
					cell=new ODGridCell();
					row.Cells.Add(cell);
					//Rem Before
					cell=new ODGridCell();
					row.Cells.Add(cell);
					//Adj Amt
					cell=new ODGridCell(Math.Round(Adj.AdjAmt,2).ToString());
					cell.CellColor=Color.LightCyan;
					row.Cells.Add(cell);
					//Rem After
					cell=new ODGridCell(Math.Round(RemAfter,2).ToString());
					if(Proc==null) {//Unassigned adj
						cell=new ODGridCell();
					}
					cell.CellColor=Color.LightCyan;
					row.Cells.Add(cell);
				}
				return row;
			}
		}

		enum AdjAmtType {
			FixedAmt,
			PercentOfRemBal,
			PercentOfFee,
		}
	}
}