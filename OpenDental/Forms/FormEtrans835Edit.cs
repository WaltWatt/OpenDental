using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;
using System.Linq;

namespace OpenDental {
	public partial class FormEtrans835Edit:ODForm {

		public string TranSetId835;
		///<summary>Must be set before the form is shown.</summary>
		public Etrans EtransCur;
		///<summary>Must be set before the form is shown.  The message text for EtransCur.</summary>
		public string MessageText835;
		private X835 _x835;
		private decimal _claimInsPaidSum;
		private decimal _provAdjAmtSum;
		private static FormEtrans835Edit _form835=null;
		private ContextMenu gridClaimDetailsMenu=new ContextMenu();
		///<summary>List of all claimProcs associated to the attached claims.</summary>
		private List<ClaimProc> _listClaimProcs;
		///<summary>List of all attaches associated to the attached claims, even if associated to another etrans.</summary>
		private List<Etrans835Attach> _listAttaches; 

		public FormEtrans835Edit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormEtrans835Edit_Load(object sender,EventArgs e) {
			if(_form835!=null && !_form835.IsDisposed) {
				if(!MsgBox.Show(this,true,"Opening another ERA will close the current ERA you already have open.  Continue?")) {
					//Form already exists and user wants to keep current instance.
					TranSetId835=_form835.TranSetId835;
					EtransCur=_form835.EtransCur;
					MessageText835=_form835.MessageText835;
				}
				_form835.Close();//Always close old form and open new form, so the new copy will come to front, since BringToFront() does not always work.
			}
			_form835=this;//Set the static variable to this form because we're always going to show this form even if they're viewing old information.
			List <Etrans835Attach> listAttached=Etrans835Attaches.GetForEtrans(EtransCur.EtransNum);
			_x835=new X835(EtransCur,MessageText835,TranSetId835,listAttached);
			RefreshFromDb();
			FillAll();
			Menu.MenuItemCollection menuItemCollection=new Menu.MenuItemCollection(gridClaimDetailsMenu);
			List<MenuItem> listMenuItems=new List<MenuItem>();
			listMenuItems.Add(new MenuItem(Lan.g(this,"Go to Account"),new EventHandler(gridClaimDetails_RightClickHelper)));
			listMenuItems.Add(new MenuItem(Lan.g(this,"Add Tracking Status"),new EventHandler(gridClaimDetails_RightClickHelper)));//Enabled by default
			menuItemCollection.AddRange(listMenuItems.ToArray());
			gridClaimDetailsMenu.Popup+=new EventHandler((o,ea) => {
				int index=gridClaimDetails.GetSelectedIndex();
				bool isGoToAccountEnabled=false;
				if(index!=-1 && gridClaimDetails.SelectedIndices.Count()==1) {
					Hx835_Claim claimPaid=(Hx835_Claim)gridClaimDetails.Rows[index].Tag;
					if(claimPaid.IsAttachedToClaim && claimPaid.ClaimNum!=0) {
						isGoToAccountEnabled=true;
					}
				}
				gridClaimDetailsMenu.MenuItems[0].Enabled=isGoToAccountEnabled;
			});
			gridClaimDetails.ContextMenu=gridClaimDetailsMenu;
			gridClaimDetails.AllowSortingByColumn=true;
		}

		private void RefreshFromDb() {
			List<long> listClaimNums=_x835.ListClaimsPaid.Select(x => x.ClaimNum).Where(x => x!=0).ToList();
			_listClaimProcs=ClaimProcs.RefreshForClaims(listClaimNums);
			_listAttaches=Etrans835Attaches.GetForEtransNumOrClaimNums(false,EtransCur.EtransNum,listClaimNums.ToArray());
		}

		private void FormEtrans835Edit_Resize(object sender,EventArgs e) {
			//This funciton is called before FormEtrans835Edit_Load() when using ShowDialog(). Therefore, x835 is null the first time FormEtrans835Edit_Resize() is called.
			if(_x835==null) {
				return;
			}
			gridProviderAdjustments.Width=butOK.Right-gridProviderAdjustments.Left;
			FillProviderAdjustmentDetails();//Because the grid columns change size depending on the form size.
			gridClaimDetails.Width=gridProviderAdjustments.Width;
			gridClaimDetails.Height=labelPaymentAmount.Top-5-gridClaimDetails.Top;
			FillClaimDetails();//Because the grid columns change size depending on the form size.
		}

		///<summary>Reads the X12 835 text in the MessageText variable and displays the information in this form.</summary>
		private void FillAll() {
			//*835 has 3 parts: Table 1 (header), Table 2 (claim level details, one CLP segment for each claim), and Table 3 (PLB: provider/check level details).
			FillHeader();//Table 1
			FillClaimDetails();//Table 2
			FillProviderAdjustmentDetails();//Table 3
			FillFooter();
			//The following concepts should each be addressed as development progresses.
			//*837 CLM01 -> 835 CLP01 (even for split claims)
			//*Advance payments (pg. 23): in PLB segment with adjustment reason code PI.  Can be yearly or monthly.  We need to find a way to pull provider level adjustments into a deposit.
			//*Bundled procs (pg. 27): have the original proc listed in SV06. Use Line Item Control Number to identify the original proc line.
			//*Predetermination (pg. 28): Identified by claim status code 25 in CLP02. Claim adjustment reason code is 101.
			//*Claim reversals (pg. 30): Identified by code 22 in CLP02. The original claim adjustment codes can be found in CAS01 to negate the original claim.
			//Use CLP07 to identify the original claim, or if different, get the original ref num from REF02 of 2040REF*F8.
			//*Interest and Prompt Payment Discounts (pg. 31): Located in AMT segments with qualifiers I (interest) and D8 (discount). Found at claim and provider/check level.
			//Not part of AR, but part of deposit. Handle this situation by using claimprocs with 2 new status, one for interest and one for discount? Would allow reports, deposits, and claim checks to work as is.
			//*Capitation and related payments or adjustments (pg. 34 & 52): Not many of our customers use capitation, so this will probably be our last concern.
			//*Claim splits (pg. 36): MIA or MOA segments will exist to indicate the claim was split.
			//*Service Line Splits (pg. 42): LQ segment with LQ01=HE and LQ02=N123 indicate the procedure was split.
		}

		///<summary>Reads the X12 835 text in the MessageText variable and displays the information from Table 1 (Header).</summary>
		private void FillHeader() {
			//Payer information
			textPayerName.Text=_x835.PayerName;
			textPayerID.Text=_x835.PayerId;
			textPayerAddress1.Text=_x835.PayerAddress;
			textPayerCity.Text=_x835.PayerCity;
			textPayerState.Text=_x835.PayerState;
			textPayerZip.Text=_x835.PayerZip;
			textPayerContactInfo.Text=_x835.PayerContactInfo;
			//Payee information
			textPayeeName.Text=_x835.PayeeName;
			labelPayeeIdType.Text=Lan.g(this,"Payee")+" "+_x835.PayeeIdType;
			textPayeeID.Text=_x835.PayeeId;
			//Payment information
			textTransHandlingDesc.Text=_x835.TransactionHandlingDescript;
			textPaymentMethod.Text=_x835.PayMethodDescript;
			if(_x835.IsCredit) {
				textPaymentAmount.Text=_x835.InsPaid.ToString("f2");
			}
			else {
				textPaymentAmount.Text="-"+_x835.InsPaid.ToString("f2");
			}
			textAcctNumEndingIn.Text=_x835.AccountNumReceiving;
			if(_x835.DateEffective.Year>1880) {
				textDateEffective.Text=_x835.DateEffective.ToShortDateString();
			}
			textCheckNumOrRefNum.Text=_x835.TransRefNum;
			textNote.Text=EtransCur.Note;
		}

		///<summary>Reads the X12 835 text in the MessageText variable and displays the information from Table 2 (Detail).</summary>
		private void FillClaimDetails() {
			gridClaimDetails.BeginUpdate();
			gridClaimDetails.Columns.Clear();
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"Recd"),32,HorizontalAlignment.Center));
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"Patient"),0,HorizontalAlignment.Left));
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"DateService"),80,HorizontalAlignment.Center));
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"Claim\r\nIdentifier"),50,HorizontalAlignment.Left));
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"Payor\r\nControl#"),56,HorizontalAlignment.Center));//Payer Claim Control Number (CLP07)
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"Status"),0,HorizontalAlignment.Left));//Claim Status Code Description (CLP02)
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"ClaimFee"),70,HorizontalAlignment.Right));//Total Claim Charge Amount (CLP03)
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"InsPaid"),70,HorizontalAlignment.Right));//Claim Payment Amount (CLP04)
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"PatPort"),70,HorizontalAlignment.Right));//Patient Portion
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"Deduct"),70,HorizontalAlignment.Right));//Deductible
			gridClaimDetails.Columns.Add(new ODGridColumn(Lan.g(this,"Writeoff"),70,HorizontalAlignment.Right));//Writeoff
			gridClaimDetails.Rows.Clear();
			_claimInsPaidSum=0;
			List<Hx835_Claim> listClaimsPaid=_x835.ListClaimsPaid;
			for(int i=0;i<listClaimsPaid.Count;i++) {
				Hx835_Claim claimPaid=listClaimsPaid[i];
				ODGridRow row=new ODGridRow();
				SetClaimDetailRows(new List<ODGridRow>() { row },new List<Hx835_Claim>() { claimPaid });
				_claimInsPaidSum+=claimPaid.InsPaid;
				gridClaimDetails.Rows.Add(row);
			}
			gridClaimDetails.EndUpdate();
		}
		
		///<summary>ListRows and ListClaimsPaid must be 1:1 and in the same order.</summary>
		private void SetClaimDetailRows(List<ODGridRow> listRows,List<Hx835_Claim> listClaimsPaid,bool isRefreshNeeded=false) {
			if(isRefreshNeeded) {
				RefreshFromDb();
			}
			for(int i=0;i<listRows.Count;i++) {
				UI.ODGridRow row=listRows[i];
				Hx835_Claim claimPaid=listClaimsPaid[i];
				row.Tag=claimPaid;
				row.Cells.Clear();
				string claimStatus="";
				if(claimPaid.IsProcessed(_listClaimProcs,_listAttaches)) {
					claimStatus="X";
				}
				else if(claimPaid.IsAttachedToClaim && claimPaid.ClaimNum==0) {
					claimStatus="N/A";//User detached claim manually.
				}
				row.Cells.Add(claimStatus);
				row.Cells.Add(new UI.ODGridCell(claimPaid.PatientName.ToString()));//Patient
				string strDateService=claimPaid.DateServiceStart.ToShortDateString();
				if(claimPaid.DateServiceEnd>claimPaid.DateServiceStart) {
					strDateService+=" to "+claimPaid.DateServiceEnd.ToShortDateString();
				}
				row.Cells.Add(new UI.ODGridCell(strDateService));//DateService
				row.Cells.Add(new UI.ODGridCell(claimPaid.ClaimTrackingNumber));//Claim Identfier
				row.Cells.Add(new UI.ODGridCell(claimPaid.PayerControlNumber));//PayorControlNum
				row.Cells.Add(new UI.ODGridCell(claimPaid.StatusCodeDescript));//Status
				row.Cells.Add(new UI.ODGridCell(claimPaid.ClaimFee.ToString("f2")));//ClaimFee
				row.Cells.Add(new UI.ODGridCell(claimPaid.InsPaid.ToString("f2")));//InsPaid
				row.Cells.Add(new UI.ODGridCell(claimPaid.PatientPortionAmt.ToString("f2")));//PatPort
				row.Cells.Add(new UI.ODGridCell(claimPaid.PatientDeductAmt.ToString("f2")));//Deduct
				row.Cells.Add(new UI.ODGridCell(claimPaid.WriteoffAmt.ToString("f2")));//Writeoff
			}
		}

		///<summary>Reads the X12 835 text in the MessageText variable and displays the information from Table 3 (Summary).</summary>
		private void FillProviderAdjustmentDetails() {
			if(_x835.ListProvAdjustments.Count==0) {
				gridProviderAdjustments.Title="Provider Adjustments (None Reported)";
			}
			else {
				gridProviderAdjustments.Title="Provider Adjustments";
			}
			const int colWidthNPI=88;
			const int colWidthFiscalPeriod=80;
			const int colWidthReasonCode=90;
			const int colWidthRefIdent=80;
			const int colWidthAmount=80;
			int colWidthVariable=gridProviderAdjustments.Width-colWidthNPI-colWidthFiscalPeriod-colWidthReasonCode-colWidthRefIdent-colWidthAmount;
			gridProviderAdjustments.BeginUpdate();
			gridProviderAdjustments.Columns.Clear();
			gridProviderAdjustments.Columns.Add(new ODGridColumn("NPI",colWidthNPI,HorizontalAlignment.Center));
			gridProviderAdjustments.Columns.Add(new ODGridColumn("FiscalPeriod",colWidthFiscalPeriod,HorizontalAlignment.Center));
			gridProviderAdjustments.Columns.Add(new ODGridColumn("Reason",colWidthVariable,HorizontalAlignment.Left));
			gridProviderAdjustments.Columns.Add(new ODGridColumn("ReasonCode",colWidthReasonCode,HorizontalAlignment.Center));
			gridProviderAdjustments.Columns.Add(new ODGridColumn("RefIdent",colWidthRefIdent,HorizontalAlignment.Center));			
			gridProviderAdjustments.Columns.Add(new ODGridColumn("AdjAmt",colWidthAmount,HorizontalAlignment.Right));
			gridProviderAdjustments.EndUpdate();
			gridProviderAdjustments.BeginUpdate();
			gridProviderAdjustments.Rows.Clear();
			_provAdjAmtSum=0;
			for(int i=0;i<_x835.ListProvAdjustments.Count;i++) {
				Hx835_ProvAdj provAdj=_x835.ListProvAdjustments[i];
				ODGridRow row=new ODGridRow();
				row.Tag=provAdj;
				row.Cells.Add(new ODGridCell(provAdj.Npi));//NPI
				row.Cells.Add(new ODGridCell(provAdj.DateFiscalPeriod.ToShortDateString()));//FiscalPeriod
				row.Cells.Add(new ODGridCell(provAdj.ReasonCodeDescript));//Reason
				row.Cells.Add(new ODGridCell(provAdj.ReasonCode));//ReasonCode
				row.Cells.Add(new ODGridCell(provAdj.RefIdentification));//RefIdent
				row.Cells.Add(new ODGridCell(provAdj.AdjAmt.ToString("f2")));//AdjAmt
				_provAdjAmtSum+=provAdj.AdjAmt;
				gridProviderAdjustments.Rows.Add(row);
			}
			gridProviderAdjustments.EndUpdate();
		}

		private void FillFooter() {
			textClaimInsPaidSum.Text=_claimInsPaidSum.ToString("f2");
			textProjAdjAmtSum.Text=_provAdjAmtSum.ToString("f2");
			textPayAmountCalc.Text=(_claimInsPaidSum-_provAdjAmtSum).ToString("f2");
		}

		private void butRawMessage_Click(object sender,EventArgs e) {
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(MessageText835);
			msgbox.Show(this);//This window is just used to display information.
		}

		private void gridProviderAdjustments_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Hx835_ProvAdj provAdj=(Hx835_ProvAdj)gridProviderAdjustments.Rows[e.Row].Tag;
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(
				provAdj.Npi+"\r\n"
				+provAdj.DateFiscalPeriod.ToShortDateString()+"\r\n"
				+provAdj.ReasonCode+" "+provAdj.ReasonCodeDescript+"\r\n"
				+provAdj.RefIdentification+"\r\n"
				+provAdj.AdjAmt.ToString("f2"));
			msgbox.Show(this);//This window is just used to display information.
		}

		private void gridClaimDetails_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Hx835_Claim claimPaid=(Hx835_Claim)gridClaimDetails.Rows[e.Row].Tag;
			Claim claim=claimPaid.GetClaimFromDb();
			bool isAttachNeeded=(!claimPaid.IsAttachedToClaim);
			if(claim==null) {//Not found in db.
				claim=ClaimSelectionHelper(claimPaid);
				if(claim!=null) {//A claim was selected.
					isAttachNeeded=true;//Hard attach selected claim to db claim, since user manually picked matching claim.
				}
			}
			if(claim==null) {//Claim not found and user did not select one.
				return;
			}
			//From this point on claim is not null.
			bool isReadOnly=true;
			CreateAttachForClaim(claimPaid,claim.ClaimNum,isAttachNeeded);
			if(claimPaid.IsSplitClaim) {
				//Sync ClaimNum for all split claims in the same group, based on user selection.
				claimPaid.GetOtherNotDetachedSplitClaims()
					.ForEach(x => CreateAttachForClaim(x,claim.ClaimNum,isAttachNeeded));
			}
			if(claimPaid.IsProcessed(_listClaimProcs,_listAttaches)) {
				//If the claim is already received, then we do not allow the user to enter payments.
				//The user can edit the claim to change the status from received if they wish to enter the payments again.
				if(Security.IsAuthorized(Permissions.ClaimView)) {
					Patient pat=Patients.GetPat(claim.PatNum);
					Family fam=Patients.GetFamily(claim.PatNum);
					FormClaimEdit formCE=new FormClaimEdit(claim,pat,fam);
					formCE.ShowDialog();//Modal, because the user could edit information in this window.
				}
				isReadOnly=false;
			}
			else if(Security.IsAuthorized(Permissions.InsPayCreate)) {//Claim found and is not received.  Date not checked here, but it will be checked when actually creating the check.
				EnterPayment(_x835,claimPaid,claim,false);
				isReadOnly=false;
			}
			if(isReadOnly) {
				FormEtrans835ClaimEdit formC=new FormEtrans835ClaimEdit(_x835,claimPaid);
				formC.Show(this);//This window is just used to display information.
			}
			if(!gridClaimDetails.IsDisposed) {//Not sure how the grid is sometimes disposed, perhaps because of it being non-modal.
				//Refresh the claim detail row in case something changed above.
				gridClaimDetails.BeginUpdate();
				List<ODGridRow> listRows=new List<ODGridRow>() { gridClaimDetails.Rows[e.Row] };//Row associated to claimPaid
				if(claimPaid.IsSplitClaim) {//Need to update multiple rows.
					List<Hx835_Claim> listOtherSplitClaims=claimPaid.GetOtherNotDetachedSplitClaims();
					List<ODGridRow> listAdditionalRows=gridClaimDetails.Rows
						.Where(x =>
							x!=gridClaimDetails.Rows[e.Row]
							&& listOtherSplitClaims.Contains((Hx835_Claim)x.Tag)
						).ToList();
					listRows.AddRange(listAdditionalRows);
				}
				SetClaimDetailRows(listRows,listRows.Select(x => (Hx835_Claim)x.Tag ).ToList(),true);
				gridClaimDetails.EndUpdate();
			}
		}

		///<summary>If given claim!=null, attempts to open the patient select and claim select windows.
		///Sets isAttachNeeded to true if user went through full patient and claim selection logic, claim was not null when provided.
		///Returns false if user does not select a claim.</summary>
		private Claim ClaimSelectionHelper(Hx835_Claim claimPaid) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Original claim not found.  You can now attempt to locate the claim manually.")) {
				return null;
			}
			//Create partial patient object to pre-fill search text boxes in FormPatientSelect
			Patient patCur=new Patient();
			patCur.LName=claimPaid.PatientName.Lname;
			patCur.FName=claimPaid.PatientName.Fname;
			FormPatientSelect formP=new FormPatientSelect(patCur);
			formP.PreFillSearchBoxes(patCur);
			formP.ShowDialog();
			if(formP.DialogResult!=DialogResult.OK) {
				return null;
			}
			FormEtrans835ClaimSelect eTransClaimSelect=new FormEtrans835ClaimSelect(formP.SelectedPatNum,claimPaid);
			eTransClaimSelect.ShowDialog();
			if(eTransClaimSelect.DialogResult!=DialogResult.OK) {
				return null;
			}
			Claim claim=eTransClaimSelect.ClaimSelected; //Set claim so below we can act if a claim was already linked.
			if(!String.IsNullOrEmpty(claimPaid.ClaimTrackingNumber) && claimPaid.ClaimTrackingNumber!="0") {//Claim was not printed, it is an eclaim.
				claim.ClaimIdentifier=claimPaid.ClaimTrackingNumber;//Already checked DOS and ClaimFee, update claim identifier to link claims.
				Claims.UpdateClaimIdentifier(claim.ClaimNum,claim.ClaimIdentifier);//Update DB
			}
			return claim;
		}
		
		///<summary>Inserts new Etrans835Attach for given claimPaid and claim.
		///Deletes any existing Etrans835Attach prior to inserting new one.
		///Sets claimPaid.ClaimNum and claimPaid.IsAttachedToClaim.</summary>
		private void CreateAttachForClaim(Hx835_Claim claimPaid,long claimNum,bool isNewAttachNeeded) {
			if(!isNewAttachNeeded 
				&& _listAttaches.Exists(
					x => x.ClaimNum==claimNum 
					&& x.EtransNum==_x835.EtransSource.EtransNum 
					&& x.ClpSegmentIndex==claimPaid.ClpSegmentIndex)
				)
			{
				//Not forcing a new attach and one already exists.
				return;
			}
			//Create a hard link between the selected claim and the claim info on the 835.
			Etrans835Attaches.Delete(EtransCur.EtransNum,claimPaid.ClpSegmentIndex);//Detach existing if any.
			Etrans835Attach attach=new Etrans835Attach();
			attach.EtransNum=EtransCur.EtransNum;
			attach.ClaimNum=claimNum;
			attach.ClpSegmentIndex=claimPaid.ClpSegmentIndex;
			Etrans835Attaches.Insert(attach);
			claimPaid.ClaimNum=claimNum;
			claimPaid.IsAttachedToClaim=true;
		}

		///<summary>Click method used by all gridClaimDetails right click options.</summary>
		private void gridClaimDetails_RightClickHelper(object sender,EventArgs e) {
			int index=gridClaimDetails.GetSelectedIndex();
			if(index==-1) {//Should not happen, menu item is only enabled when exactly 1 row is selected.
				return;
			}
			int menuItemIndex=((MenuItem)sender).Index;//sender is the selected menuItem
			if(menuItemIndex==0) {//Go To Account
				Hx835_Claim claimPaid=(Hx835_Claim)gridClaimDetails.Rows[index].Tag;
				GotoModule.GotoAccount(Claims.GetClaim(claimPaid.ClaimNum).PatNum);
			}
			else if(menuItemIndex==1) {//Add Tracking Status
				Hx835_Claim claimPaid=(Hx835_Claim)gridClaimDetails.Rows[index].Tag;
				Claim claim=claimPaid.GetClaimFromDb();
				bool isAttachNeeded=(!claimPaid.IsAttachedToClaim);
				if(claim==null) {//Not found in db.
					claim=ClaimSelectionHelper(claimPaid);
					if(claim!=null) {//A claim was selected.
						isAttachNeeded=true;//Hard attach selected claim to db claim, since user manually picked matching claim.
					}
				}
				if(claim==null) {//Claim not found and user did not select one.
					return;
				}
				CreateAttachForClaim(claimPaid,claim.ClaimNum,isAttachNeeded);
				if(FormClaimEdit.AddClaimCustomTracking(claim,claimPaid.GetRemarks())) {//A tracking status was chosen and the tracking status contains an error code.
					DetachClaimHelper(new List<int> { index });//Detach claim from ERA for convenience.
				}
				FillClaimDetails();
			}
		}

		///<summary>Enter either by total and/or by procedure, depending on whether or not procedure detail was provided in the 835 for this claim.
		///This function creates the payment claimprocs and displays the payment entry window.</summary>
		public static void EnterPayment(X835 x835,Hx835_Claim claimPaid,Claim claim,bool isAutomatic) {
			bool isSupplementalPay=(claim.ClaimStatus=="R");
			//Warn user if they selected a claim which is already received, so they do not accidentally enter a supplemental payment when they meant to enter a new payment.
			if(isSupplementalPay && !MsgBox.Show("FormEtrans835Edit",MsgBoxButtons.YesNo,"You are about to post supplemental payments to this claim.\r\nContinue?")) {
				return;
			}
			Patient pat=Patients.GetPat(claim.PatNum);
			Family fam=Patients.GetFamily(claim.PatNum);
			List<InsSub> listInsSubs=InsSubs.RefreshForFam(fam);
			List<InsPlan> listInsPlans=InsPlans.RefreshForSubList(listInsSubs);
			List<PatPlan> listPatPlans=PatPlans.Refresh(claim.PatNum);
			List<ClaimProc> listClaimProcsForClaim=ClaimProcs.RefreshForClaim(claim.ClaimNum);
			List<Procedure> listProcs=Procedures.GetProcsFromClaimProcs(listClaimProcsForClaim);
			List<Hx835_Claim> listNotDetachedPaidClaims=new List<Hx835_Claim>();
			listNotDetachedPaidClaims.Add(claimPaid);
			if(claimPaid.IsSplitClaim) {
				listNotDetachedPaidClaims.AddRange(claimPaid.GetOtherNotDetachedSplitClaims());
			}
			ClaimProc cpByTotal=new ClaimProc();
			cpByTotal.FeeBilled=0;//All attached claimprocs will show in the grid and be used for the total sum.
			cpByTotal.DedApplied=(double)(listNotDetachedPaidClaims.Sum(x => x.PatientDeductAmt));
			cpByTotal.AllowedOverride=(double)(listNotDetachedPaidClaims.Sum(x => x.AllowedAmt));
			cpByTotal.InsPayAmt=(double)(listNotDetachedPaidClaims.Sum(x => x.InsPaid));
			cpByTotal.WriteOff=(double)(listNotDetachedPaidClaims.Sum(x => x.WriteoffAmt));
			List<ClaimProc> listClaimProcsToEdit=new List<ClaimProc>();
			//Automatically set PayPlanNum if there is a payplan with matching PatNum, PlanNum, and InsSubNum that has not been paid in full.
			long insPayPlanNum=0;
			if(claim.ClaimType!="PreAuth" && claim.ClaimType!="Cap") {//By definition, capitation insurance pays in one lump-sum, not over an extended period of time.
				//By sending in ClaimNum, we ensure that we only get the payplan a claimproc from this claim was already attached to or payplans with no claimprocs attached.
				List<PayPlan> listPayPlans=PayPlans.GetValidInsPayPlans(claim.PatNum,claim.PlanNum,claim.InsSubNum,claim.ClaimNum);
				if(listPayPlans.Count==1) {
					insPayPlanNum=listPayPlans[0].PayPlanNum;
				}
				else if(listPayPlans.Count>1 && !isAutomatic) {
					//More than one valid PayPlan.  Cannot show this prompt when entering automatically, because it would disrupt workflow.
					FormPayPlanSelect FormPPS=new FormPayPlanSelect(listPayPlans);
					FormPPS.ShowDialog();//Modal because this form allows editing of information.
					if(FormPPS.DialogResult==DialogResult.OK) {
						insPayPlanNum=FormPPS.SelectedPayPlanNum;
					}
				}
			}
			if(isSupplementalPay) {
				List<ClaimProc> listClaimCopyProcs=listClaimProcsForClaim.Select(x => x.Copy()).ToList();
				if(claimPaid.IsSplitClaim) {
					//Split supplemental payment, only CreateSuppClaimProcs for the sub set of split claim procs.
					foreach(Hx835_Claim splitClaim in listNotDetachedPaidClaims) {
						foreach(Hx835_Proc proc in splitClaim.ListProcs) {
							ClaimProc claimProcForClaim=listClaimCopyProcs.FirstOrDefault(x => 
								x.ProcNum!=0 && ((x.ProcNum==proc.ProcNum)
								|| x.CodeSent==proc.ProcCodeBilled
								&& (decimal)x.FeeBilled==proc.ProcFee
								&& x.Status==ClaimProcStatus.Received
								&& x.TagOD==null)
							);
							if(claimProcForClaim==null) {
								continue;
							}
							claimProcForClaim.TagOD=true;
						}
					}
					//Remove all claimProcs that were not matched, to avoid entering payment on unmatched claimprocs.
					listClaimCopyProcs.RemoveAll(x => x.TagOD==null);
				}
				//Selection logic inside ClaimProcs.CreateSuppClaimProcs() mimics FormClaimEdit "Supplemental" button click.
				listClaimProcsToEdit=ClaimProcs.CreateSuppClaimProcs(listClaimCopyProcs,claimPaid.IsReversal,false);
				listClaimProcsForClaim.AddRange(listClaimProcsToEdit);//listClaimProcsToEdit is a subsSet of listClaimProcsForClaim, like above
			}
			else if(claimPaid.IsSplitClaim) {//Not supplemental, simply a split.
				//For split claims we only want to edit the sub-set of procs that exist on the internal claim.
				foreach(Hx835_Proc proc in listNotDetachedPaidClaims.SelectMany(x => x.ListProcs).ToList()) {
					ClaimProc claimProcFromClaim=listClaimProcsForClaim.FirstOrDefault(x => 
						//Mimics proc matching in claimPaid.GetPaymentsForClaimProcs(...)
						x.ProcNum!=0 && ((x.ProcNum==proc.ProcNum)
						|| (x.CodeSent==proc.ProcCodeBilled 
						&& (decimal)x.FeeBilled==proc.ProcFee
						&& x.Status==ClaimProcStatus.NotReceived
						&& x.TagOD==null))
					);
					if(claimProcFromClaim==null) {//Not found, By Total payment row will be inserted.
						continue;
					}
					claimProcFromClaim.TagOD=true;
					listClaimProcsToEdit.Add(claimProcFromClaim);
				}
			}
			else {//Original payment
				//Selection logic mimics FormClaimEdit "By Procedure" button selection logic.
				//Choose the claimprocs which are not received.
				for(int i=0;i<listClaimProcsForClaim.Count;i++) {
					if(listClaimProcsForClaim[i].ProcNum==0) {//Exclude any "by total" claimprocs.  Choose claimprocs for procedures only.
						continue;
					}
					if(listClaimProcsForClaim[i].Status!=ClaimProcStatus.NotReceived) {//Ignore procedures already received.
						continue;
					}
					listClaimProcsToEdit.Add(listClaimProcsForClaim[i]);//Procedures not yet received.
				}
				//If all claimprocs are received, then choose claimprocs if not paid on.
				if(listClaimProcsToEdit.Count==0) {
					for(int i=0;i<listClaimProcsForClaim.Count;i++) {
						if(listClaimProcsForClaim[i].ProcNum==0) {//Exclude any "by total" claimprocs.  Choose claimprocs for procedures only.
							continue;
						}
						if(listClaimProcsForClaim[i].ClaimPaymentNum!=0) {//Exclude claimprocs already paid.
							continue;
						}
						listClaimProcsToEdit.Add(listClaimProcsForClaim[i]);//Procedures not paid yet.
					}
				}
			}
			//For each NotReceived/unpaid procedure on the claim where the procedure information can be successfully located on the EOB, enter the payment information.
			List <List <Hx835_Proc>> listProcsForClaimProcs=Hx835_Claim.GetPaymentsForClaimProcs(listClaimProcsToEdit,listNotDetachedPaidClaims.SelectMany(x => x.ListProcs).ToList());
			for(int i=0;i<listClaimProcsToEdit.Count;i++) {
				ClaimProc claimProc=listClaimProcsToEdit[i];
				List<Hx835_Proc> listProcsForProcNum=listProcsForClaimProcs[i];
				//If listProcsForProcNum.Count==0, then procedure payment details were not not found for this one specific procedure.
				//This can happen with procedures from older 837s, when we did not send out the procedure identifiers, in which case ProcNum would be 0.
				//Since we cannot place detail on the service line, we will leave the amounts for the procedure on the total payment line.
				//If listProcsForPorcNum.Count==1, then we know that the procedure was adjudicated as is or it might have been bundled, but we treat both situations the same way.
				//The 835 is required to include one line for each bundled procedure, which gives is a direct manner in which to associate each line to its original procedure.
				//If listProcForProcNum.Count > 1, then the procedure was either split or unbundled when it was adjudicated by the payer.
				//We will not bother to modify the procedure codes on the claim, because the user can see how the procedure was split or unbunbled by viewing the 835 details.
				//Instead, we will simply add up all of the partial payment lines for the procedure, and report the full payment amount on the original procedure.
				claimProc.DedApplied=0;
				claimProc.AllowedOverride=0;
				claimProc.InsPayAmt=0;
				claimProc.WriteOff=0;
				if(isSupplementalPay) {
					//This mimics how a normal supplemental payment is created in FormClaim edit "Supplemental" button click.
					//We do not do this in ClaimProcs.CreateSuppClaimProcs(...) for matching reasons.
					//Stops the claim totals from being incorrect.
					claimProc.FeeBilled=0;
				}
				StringBuilder sb=new StringBuilder();
				//TODO: Will the negative writeoff be cleared back to zero anywhere else (ex ClaimProc edit window)?
				for(int j=0;j<listProcsForProcNum.Count;j++) {
					Hx835_Proc procPaidPartial=listProcsForProcNum[j];
					claimProc.DedApplied+=(double)procPaidPartial.DeductibleAmt;
					claimProc.AllowedOverride+=(double)procPaidPartial.AllowedAmt;
					claimProc.InsPayAmt+=(double)procPaidPartial.InsPaid;
					claimProc.WriteOff+=(double)procPaidPartial.WriteoffAmt;
					if(sb.Length>0) {
						sb.Append("\r\n");
					}
					sb.Append(procPaidPartial.GetRemarks());
				}
				claimProc.Remarks=sb.ToString();
				if(claim.ClaimType=="PreAuth") {
					claimProc.Status=ClaimProcStatus.Preauth;
				}
				else if(claim.ClaimType=="Cap") {
					//Do nothing.  The claimprocstatus will remain Capitation.
				}
				else {//Received or Supplemental
					if(isSupplementalPay) {
						//This is already set in ClaimProcs.CreateSuppClaimProcs() above, but lets make it clear for others.
						claimProc.Status=ClaimProcStatus.Supplemental;
						claimProc.IsNew=true;//Used in FormEtrans835ClaimPay.FillGridProcedures().
					}
					else {//Received.  Original payment
						claimProc.Status=ClaimProcStatus.Received;
						claimProc.PayPlanNum=insPayPlanNum;//Payment plans do not exist for PreAuths or Capitation claims, by definition.
						if(claimPaid.IsSplitClaim) {
							claimProc.IsNew=true;//Used in FormEtrans835ClaimPay.FillGridProcedures() to highlight the procs on this split claim
						}
					}
					claimProc.DateEntry=DateTime.Now;//Date is was set rec'd or supplemental.
				}
				claimProc.DateCP=DateTimeOD.Today;
			}
			//Limit the scope of the "By Total" payment to the new claimprocs only.
			//This "By Total" payment will account for any procedures that could not be matched to the
			//procedures reported on the ERA due to any changes that occured after the claim was originally sent.
			for(int i=0;i<listClaimProcsToEdit.Count;i++) {
				ClaimProc claimProc=listClaimProcsToEdit[i];
				cpByTotal.DedApplied-=claimProc.DedApplied;
				cpByTotal.AllowedOverride-=claimProc.AllowedOverride;
				cpByTotal.InsPayAmt-=claimProc.InsPayAmt;
				cpByTotal.WriteOff-=claimProc.WriteOff;//May cause cpByTotal.Writeoff to go negative if the user typed in the value for claimProc.Writeoff.
			}
			//The writeoff may be negative if the user manually entered some payment amounts before loading this window, if UCR fee schedule incorrect, and is always negative for reversals.
			if(!claimPaid.IsReversal && cpByTotal.WriteOff<0) {
				cpByTotal.WriteOff=0;
			}
			bool isByTotalIncluded=true;
			//Do not create a total payment if the payment contains all zero amounts, because it would not be useful.  Written to account for potential rounding errors in the amounts.
			if(Math.Round(cpByTotal.DedApplied,2,MidpointRounding.AwayFromZero)==0
				&& Math.Round(cpByTotal.AllowedOverride,2,MidpointRounding.AwayFromZero)==0
				&& Math.Round(cpByTotal.InsPayAmt,2,MidpointRounding.AwayFromZero)==0
				&& Math.Round(cpByTotal.WriteOff,2,MidpointRounding.AwayFromZero)==0)
			{
				isByTotalIncluded=false;
			}
			if(claim.ClaimType=="PreAuth") {
				//In the claim edit window we currently block users from entering PreAuth payments by total, presumably because total payments affect the patient balance.
				isByTotalIncluded=false;
			}
			else if(claim.ClaimType=="Cap") {
				//In the edit claim window, we currently warn and discourage users from entering Capitation payments by total, because total payments affect the patient balance.
				isByTotalIncluded=false;
			}
			if(isByTotalIncluded) {
				cpByTotal.Status=ClaimProcStatus.Received;
				cpByTotal.ClaimNum=claim.ClaimNum;
				cpByTotal.PatNum=claim.PatNum;
				cpByTotal.ProvNum=claim.ProvTreat;
				cpByTotal.PlanNum=claim.PlanNum;
				cpByTotal.InsSubNum=claim.InsSubNum;
				cpByTotal.DateCP=DateTimeOD.Today;
				cpByTotal.ProcDate=claim.DateService;
				cpByTotal.DateEntry=DateTime.Now;
				cpByTotal.ClinicNum=claim.ClinicNum;
				cpByTotal.Remarks=string.Join("\r\n",listNotDetachedPaidClaims.SelectMany(x => x.GetRemarks()));
				cpByTotal.PayPlanNum=insPayPlanNum;
				cpByTotal.IsNew=true;//Used in FormEtrans835ClaimPay.FillGridProcedures().
				//Add the total payment to the beginning of the list, so that the ins paid amount for the total payment will be highlighted when FormEtrans835ClaimPay loads.
				listClaimProcsForClaim.Insert(0,cpByTotal);
			}
			FormEtrans835ClaimPay FormP=new FormEtrans835ClaimPay(x835,claimPaid,claim,pat,fam,listInsPlans,listPatPlans,listInsSubs);
			FormP.ListClaimProcsForClaim=listClaimProcsForClaim;
			if(isAutomatic) {
				FormP.ReceivePayment();
			}
			else if(FormP.ShowDialog()!=DialogResult.OK) {//Modal because this window can edit information
				if(cpByTotal.ClaimProcNum!=0) {
					ClaimProcs.Delete(cpByTotal);
				}
				if(isSupplementalPay) {	
					ClaimProcs.DeleteMany(listClaimProcsToEdit);//Supplemental claimProcs are pre inserted, delete if we do not post payment information.
				}
			}
		}

		///<summary>etrans must be the entire object due to butOK_Click(...) calling Etranss.Update(...).
		///Anywhere we pull etrans from Etranss.RefreshHistory(...) will need to pull full object using an additional query.
		///Eventually we should enhance Etranss.RefreshHistory(...) to return full objects.</summary>
		public static void ShowEra(Etrans etrans){
			string messageText835=EtransMessageTexts.GetMessageText(etrans.EtransMessageTextNum,false);
			X12object x835=new X12object(messageText835);
			List<string> listTranSetIds=x835.GetTranSetIds();
			if(listTranSetIds.Count>=2 && etrans.TranSetId835=="") {//More than one EOB in the 835 and we do not know which one to pick.
				FormEtrans835PickEob formPickEob=new FormEtrans835PickEob(listTranSetIds,messageText835,etrans);
				formPickEob.ShowDialog();
			}
			else {
				FormEtrans835Edit Form835=new FormEtrans835Edit();
				Form835.EtransCur=etrans;
				Form835.MessageText835=messageText835;
				Form835.TranSetId835=etrans.TranSetId835;//Empty or null string will cause the first EOB in the 835 to display.
				Form835.Show();//Non-modal
			}
		}

		private void butDetachClaim_Click(object sender,EventArgs e) {
			if(gridClaimDetails.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select a claim from the claims paid grid and try again.");
				return;
			}
			if(gridClaimDetails.SelectedIndices.Length > 1) {
				if(!MsgBox.Show(this,true,
					"All selected claims will be immediately detached from this ERA even if you click Cancel when you leave the ERA window.  "
					+"Click OK to continue, or click Cancel to leave claims attached."))
				{
					return;
				}
			}
			DetachClaimHelper(gridClaimDetails.SelectedIndices.ToList());
		}

		private void DetachClaimHelper(List<int> listGridIndices) {
			gridClaimDetails.BeginUpdate();
			for(int i=0;i<listGridIndices.Count;i++) {
				ODGridRow row=gridClaimDetails.Rows[listGridIndices[i]];
				Hx835_Claim claimPaid=(Hx835_Claim)row.Tag;
				Etrans835Attaches.Delete(EtransCur.EtransNum,claimPaid.ClpSegmentIndex);
				Etrans835Attach attach=new Etrans835Attach();
				attach.EtransNum=EtransCur.EtransNum;
				attach.ClaimNum=0;
				attach.ClpSegmentIndex=claimPaid.ClpSegmentIndex;
				Etrans835Attaches.Insert(attach);
				claimPaid.IsAttachedToClaim=true;
				claimPaid.ClaimNum=0;
				SetClaimDetailRows(new List<ODGridRow>() { row },new List<Hx835_Claim>() { claimPaid },true);
			}
			gridClaimDetails.EndUpdate();
		}

		private void buttonFindClaimMatch_Click(object sender,EventArgs e) {
			List<Hx835_Claim> listDetachedPaidClaims=gridClaimDetails
				.SelectedIndices
				.Select(x => (Hx835_Claim)gridClaimDetails.Rows[x].Tag)
				.ToList();
			if(listDetachedPaidClaims.Any(x => x.IsAttachedToClaim && x.ClaimNum!=0)) {
				MsgBox.Show(this,"Only manually detached rows can be selected.");
				return;
			}
			List<X12ClaimMatch> listMatches=_x835.GetClaimMatches(listDetachedPaidClaims);
			List<long> listClaimNums=Claims.GetClaimFromX12(listMatches);//Can return null.
			if(listClaimNums!=null) {
				_x835.SetClaimNumsForUnattached(listClaimNums,listDetachedPaidClaims);//List are 1:1 and in same order
				for(int i=0;i<listDetachedPaidClaims.Count;i++) {
					CreateAttachForClaim(listDetachedPaidClaims[i],listClaimNums[i],true);
				}
				List<ODGridRow> listGridRows=gridClaimDetails.Rows
					.Where(x =>
						listDetachedPaidClaims.Contains((Hx835_Claim)x.Tag)
					).ToList();
				gridClaimDetails.BeginUpdate();
				SetClaimDetailRows(listGridRows,listGridRows.Select(x => ((Hx835_Claim)x.Tag)).ToList(),true);
				gridClaimDetails.EndUpdate();
			}
			MsgBox.Show(this,"Done");
		}

		private void butClaimDetails_Click(object sender,EventArgs e) {
			if(gridClaimDetails.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Choose a claim paid before viewing details.");
				return;
			}
			Hx835_Claim claimPaid=(Hx835_Claim)gridClaimDetails.Rows[gridClaimDetails.SelectedIndices[0]].Tag;
			FormEtrans835ClaimEdit formE=new FormEtrans835ClaimEdit(_x835,claimPaid);
			formE.Show(this);//This window is just used to display information.
		}

		private void butPrint_Click(object sender,EventArgs e) {
			PrintClickHelper(false);
		}
		
		private void PrintClickHelper(bool isPreviewMode) {
			Sheet sheet=SheetUtil.CreateSheet(SheetDefs.GetInternalOrCustom(SheetInternalType.ERA));
			SheetParameter.GetParamByName(sheet.Parameters,"ERA").ParamValue=_x835;//Required param
			SheetFiller.FillFields(sheet);
			SheetPrinting.Print(sheet,isPreviewMode:isPreviewMode);
		}

		private void butPreview_Click(object sender,EventArgs e) {
			PrintClickHelper(true);
		}
		
		///<summary>Since ERAs are only used in the United States, we do not need to translate any text shown to the user.</summary>
		private void butBatch_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.InsPayCreate)) {//date not checked here, but it will be checked when saving the check to prevent backdating
				return;
			}
			if(gridClaimDetails.Rows.Count==0) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			//List of claims from _x835.ListClaimsPaid[X].GetClaimFromDB(), can be null.
			List<Claim> listClaimsFor835=Claims.GetClaimsFromClaimNums(_x835.ListClaimsPaid.Where(x=> x.ClaimNum!=0).Select(x => x.ClaimNum).ToList());
			List<Claim> listClaims=new List<Claim>();
			#region Populate listClaims
			List<Hx835_Claim> listSkippedPreauths=_x835.ListClaimsPaid.FindAll(x => x.IsPreauth && !x.IsAttachedToClaim);
			if(listSkippedPreauths.Count>0
				&& !MsgBox.Show(this,MsgBoxButtons.YesNo,
				"There are preauths that have not been attached to an internal claim or have not been detached from the ERA.  "
				+"Would you like to automatically detach and ignore these preauths?") )
			{
				Cursor=Cursors.Default;
				return;
			}
			List<int> listGridIndices=listSkippedPreauths.Select(x => _x835.ListClaimsPaid.FindIndex(y => y==x)).ToList();
			DetachClaimHelper(listGridIndices);
			foreach(Hx835_Claim claim in _x835.ListClaimsPaid) {
				if((claim.IsAttachedToClaim && claim.ClaimNum==0) //User manually detached claim.
					|| claim.IsPreauth) 
				{
					continue;
				}
				listClaims.Add(listClaimsFor835.FirstOrDefault(x => x.ClaimNum==claim.ClaimNum));//Can add nulls
				int index=listClaims.Count-1;
				Claim claimCur=listClaims[index];
				if(claimCur==null) {//Claim wasn't found in DB.
					listClaims[index]=new Claim();//ClaimNum will be 0, indicating that this is not a real claim.
					continue;
				}
			}
			#endregion
			if(listClaims.Count==0) {
				Cursor=Cursors.Default;
				MsgBox.Show(this,"All claims have been detached from this ERA or are preauths (there is no payment).  Click OK to close the ERA instead.");
				return;
			}
			if(listClaims.Exists(x => x.ClaimNum==0 || x.ClaimStatus!="R")) {
				#region Column width: PatNum
				int patNumColumnLength=6;//Minimum of 6 because that is the width of the column name "PatNum"
				if(listClaims.Exists(x => x.ClaimNum!=0)) {//There are claims that were found in DB, need to consider claim.PatNum.ToString() lengths.
					patNumColumnLength=Math.Min(8,Math.Max(patNumColumnLength,listClaims.Max(x => x.PatNum.ToString().Length)));
				}
				#endregion
				#region Column width: Patient
				Dictionary<long,string> dictPatNames=new Dictionary<long,string>();
				foreach(Claim claim in listClaims) {
					if(dictPatNames.ContainsKey(claim.PatNum)) {//Can be 0.  We want to add 0 to dictionary so blank name will show below.
						continue;
					}
					Patient patCur=Patients.GetPat(claim.PatNum);
					if(patCur==null) {
						dictPatNames.Add(0,"");
					}
					else {
						dictPatNames.Add(claim.PatNum,Patients.GetNameLF(patCur));
					}
				}
				int maxNamesLength=Math.Max(7,dictPatNames.Values.Max(x => x.Length));//Minimum of 7 to account for column title width "Patient".
				int maxX835NameLength=0;
				if(listClaims.Exists(x => x.ClaimNum==0)) {//There is a claim that could not be found in the DB. Must consider Hx835_Claims.PatientName lengths.
					maxX835NameLength=_x835.ListClaimsPaid
						//Only consider claims that could not be found in DB.  Both list are 1:1 and ClaimNum==0 represents that the claim could not be found.
						.FindAll(x => listClaims[_x835.ListClaimsPaid.IndexOf(x)].ClaimNum==0)
						.Max(x => x.PatientName.ToString().Length);
				}
				int maxColumnLength=Math.Max(maxNamesLength,maxX835NameLength);
				#endregion
				#region Construct msg
				string msg="One or more claims are not recieved.\r\n"
					+"You must receive all of the following claims before finializing payment:\r\n";
				msg+="-------------------------------------------------------------------\r\n";
				msg+="PatNum".PadRight(patNumColumnLength)+"\t"+"Patient".PadRight(maxColumnLength)+"\tDOS       \tTotal Fee\r\n";
				msg+="-------------------------------------------------------------------\r\n";
				for(int i=0;i<listClaims.Count;i++) {
					if(listClaims[i].ClaimNum==0) {//Current claim was not found in DB, so we will use the Hx835_Claim object.
						Hx835_Claim xClaimCur=_x835.ListClaimsPaid[i];
						msg+="".PadRight(patNumColumnLength)+"\t"//Blank PatNum because unknown.
							+xClaimCur.PatientName.ToString().PadRight(maxColumnLength)+"\t"
							+xClaimCur.DateServiceStart.ToShortDateString()+"\t"
							+POut.Decimal(xClaimCur.ClaimFee)+"\r\n";
						continue;
					}
					//Current claim was found in DB, so we will use Claim object
					Claim claim=listClaims[i];
					if(claim.ClaimStatus=="R") {
						continue;
					}
					msg+=claim.PatNum.ToString().PadRight(patNumColumnLength).Substring(0,patNumColumnLength)+"\t"
						+dictPatNames[claim.PatNum].PadRight(maxColumnLength).Substring(0,maxColumnLength)+"\t"
						+claim.DateService.ToShortDateString()+"\t"
						+POut.Double(claim.ClaimFee)+"\r\n";
				}
				#endregion
				new MsgBoxCopyPaste(msg).ShowDialog();
				Cursor=Cursors.Default;
				return;
			}
			List<ClaimProc> listClaimProcsAll=ClaimProcs.RefreshForClaims(listClaims.Where(x => x.ClaimNum!=0).Select(x=>x.ClaimNum).ToList());
			//Dictionary such that the key is a claimNum and the value is a list of associated claimProcs.
			Dictionary<long,List<ClaimProc>> dictClaimProcs=listClaimProcsAll.GroupBy(x => x.ClaimNum)
				.ToDictionary(
					x => x.Key,//ClaimNum
					x=>listClaimProcsAll.FindAll(y => y.ClaimNum==x.Key)//List of claimprocs associated to current claimNum
			);
			if(listClaimProcsAll.Exists(x => !x.Status.In(ClaimProcStatus.Received,ClaimProcStatus.Supplemental,ClaimProcStatus.CapClaim))) {
				int patNumColumnLength=Math.Max(6,listClaimProcsAll.Max(x => x.PatNum.ToString().Length));//PatNum column length
				Dictionary<long,string> dictPatNames=GetAllUniquePatNamesForClaims(listClaims);
				int maxNamesLength=dictPatNames.Values.Max(x => x.Length);
				#region Construct msg
				string msg="One or more claim procedures are set to the wrong status and are not ready to be finalized.\r\n"
					+"The acceptable claim procedure statuses are Received, Supplemental and CapClaim.\r\n"
					+"The following claims have claim procedures which need to be modified before finalizing:\r\n";
				msg+="-------------------------------------------------------------------\r\n";
				msg+="PatNum".PadRight(patNumColumnLength)+"\t"+"Patient".PadRight(maxNamesLength)+"\tDOS       \tTotal Fee\r\n";
				msg+="-------------------------------------------------------------------\r\n";
				foreach(Claim claim in listClaims) {
					List <ClaimProc> listClaimProcs=dictClaimProcs[claim.ClaimNum];
					if(listClaimProcs.All(x => x.Status.In(ClaimProcStatus.Received,ClaimProcStatus.Supplemental,ClaimProcStatus.CapClaim))) {
						continue;
					}
					msg+=claim.PatNum.ToString().PadRight(patNumColumnLength).Substring(0,patNumColumnLength)+"\t"
						+dictPatNames[claim.PatNum].PadRight(maxNamesLength).Substring(0,maxNamesLength)+"\t"
						+claim.DateService.ToShortDateString()+"\t"
						+POut.Double(claim.ClaimFee)+"\r\n";
				}
				#endregion
				new MsgBoxCopyPaste(msg).ShowDialog();
				Cursor=Cursors.Default;
				return;
			}
			#region ClaimPayment creation
			ClaimPayment claimPay=new ClaimPayment();
			//Mimics FormClaimEdit.butBatch_Click(...)
			claimPay.CheckDate=MiscData.GetNowDateTime().Date;//Today's date for easier tracking by the office and to avoid backdating before accounting lock dates.
			claimPay.IsPartial=true;//This flag is changed to "false" when the payment is finalized from inside FormClaimPayBatch.
			Patient pat=Patients.GetPat(listClaims[0].PatNum);
			claimPay.ClinicNum=pat.ClinicNum;
			claimPay.CarrierName=_x835.PayerName;
			claimPay.CheckAmt=listClaimProcsAll.Where(x => x.ClaimPaymentNum==0).Sum(x => x.InsPayAmt);//Ignore claimprocs associated to previously finalized payments.
			claimPay.CheckNum=_x835.TransRefNum;
			long defNum=0;
			if(_x835._paymentMethodCode=="CHK") {//Physical check
				defNum=Defs.GetByExactName(DefCat.InsurancePaymentType,"Check");
			}
			else if(_x835._paymentMethodCode=="ACH") {//Electronic check
				defNum=Defs.GetByExactName(DefCat.InsurancePaymentType,"EFT");
			}
			else if(_x835._paymentMethodCode=="FWT") {//Wire transfer
				defNum=Defs.GetByExactName(DefCat.InsurancePaymentType,"Wired");
			}
			claimPay.PayType=defNum;		
			ClaimPayments.Insert(claimPay);
			#endregion
			#region Update Claim and ClaimProcs
			foreach(List<ClaimProc> listClaimProcs in dictClaimProcs.Values) {
				listClaimProcs.ForEach(x => {
					if(x.ClaimPaymentNum==0) {
						//Only update claimprocs that are not already associated to another claim payment.
						//This will happen when this ERA contains claim reversals or corrections, both are entered as supplemental payments.
						x.ClaimPaymentNum=claimPay.ClaimPaymentNum;
						ClaimProcs.Update(x); 
					} 
				});
			}
			#endregion
			FormClaimEdit.FormFinalizePaymentHelper(claimPay,listClaims[0],pat,Patients.GetFamily(pat.PatNum));
			Cursor=Cursors.Default;
			DialogResult=DialogResult.OK;
			Close();
		}

		private Dictionary<long,string> GetAllUniquePatNamesForClaims(List<Claim> listClaims) {
			Dictionary<long,string> dictPatNames=new Dictionary<long, string>();
			foreach(Claim claim in listClaims) {
				if(dictPatNames.ContainsKey(claim.PatNum)) {//Can be 0.  We want to add 0 to dictionary so blank name will show below.
					continue;
				}
				Patient patCur=Patients.GetPat(claim.PatNum);
				if(patCur==null) {
					dictPatNames.Add(0,"");
				}
				else {
					dictPatNames.Add(claim.PatNum,Patients.GetNameLF(patCur));
				}
			}
			return dictPatNames;
		}

		private void butOK_Click(object sender,EventArgs e) {
			EtransCur.Note=textNote.Text;
			bool isReceived=true;
			for(int i=0;i<gridClaimDetails.Rows.Count;i++) {
				if(gridClaimDetails.Rows[i].Cells[0].Text=="") {
					isReceived=false;
					break;
				}
			}
			if(isReceived) {
				EtransCur.AckCode="Recd";
			}
			else {
				EtransCur.AckCode="";
			}
			Etranss.Update(EtransCur);
			DialogResult=DialogResult.OK;
			Close();
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
			Close();
		}

		private void FormEtrans835Edit_FormClosing(object sender,FormClosingEventArgs e) {
			_form835=null;
		}
		
	}
}