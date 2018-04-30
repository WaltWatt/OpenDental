using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;
using OpenDentBusiness.Crud;

namespace OpenDental {
	public partial class FormClaimPayList:ODForm {
		List<ClaimPayment> ListClaimPay;
		///<summary>If this is not zero upon closing, then we will jump to the account module of that patient and highlight the claim.</summary>
		public long GotoClaimNum;
		///<summary>If this is not zero upon closing, then we will jump to the account module of that patient and highlight the claim.</summary>
		public long GotoPatNum;
		//<summary>Set to true if the batch list was accessed originally by going through a claim.  This disables the GotoAccount feature.</summary>
		//public bool IsFromClaim;
		///<summary>List of defs of type ClaimPaymentGroup</summary>
		private List<Def> _listCPGroups;
		List<Clinic> _listClinics;

		public FormClaimPayList() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormClaimPayList_Load(object sender,EventArgs e) {
			textDateFrom.Text=DateTime.Now.AddDays(-10).ToShortDateString();
			textDateTo.Text=DateTime.Now.ToShortDateString();
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				comboClinic.Visible=false;
				labelClinic.Visible=false;
			}
			else {
				comboClinic.Items.Add(Lan.g(this,"All"));
				comboClinic.SelectedIndex=0;
				_listClinics=Clinics.GetDeepCopy(true);
				foreach(Clinic clin in _listClinics) {
					comboClinic.Items.Add(clin.Abbr);
				}
			}
			_listCPGroups=Defs.GetDefsForCategory(DefCat.ClaimPaymentGroups,true);
			FillComboPaymentGroup();
			FillGrid();
		}

		private void FillGrid(){
			DateTime dateFrom=PIn.Date(textDateFrom.Text);
			DateTime dateTo=PIn.Date(textDateTo.Text);
			long clinicNum=0;
			if(comboClinic.SelectedIndex>0) {
				clinicNum=_listClinics[comboClinic.SelectedIndex-1].ClinicNum;
			}
			long selectedGroupNum=0;
			if(comboPayGroup.SelectedIndex!=0) {
				selectedGroupNum=_listCPGroups[comboPayGroup.SelectedIndex-1].DefNum;
			}
			DataTable tableClaimPayments=ClaimPayments.GetForDateRange(dateFrom,dateTo,clinicNum,selectedGroupNum);
			ListClaimPay=ClaimPaymentCrud.TableToList(tableClaimPayments);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Date"),65);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Type"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Amount"),75,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Partial"),40,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Carrier"),180);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"PayGroup"),80);
			gridMain.Columns.Add(col);
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				col=new ODGridColumn(Lan.g(this,"Clinic"),80);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g(this,"Note"),180);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Scanned"),40,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);			
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<ListClaimPay.Count;i++){
				row=new ODGridRow();
				if(ListClaimPay[i].CheckDate.Year<1800) {
					row.Cells.Add("");
				}
				else{
					row.Cells.Add(ListClaimPay[i].CheckDate.ToShortDateString());
				}
				row.Cells.Add(Defs.GetName(DefCat.InsurancePaymentType,ListClaimPay[i].PayType));
				row.Cells.Add(ListClaimPay[i].CheckAmt.ToString("c"));
				row.Cells.Add(ListClaimPay[i].IsPartial?"X":"");
				row.Cells.Add(ListClaimPay[i].CarrierName);
				row.Cells.Add(Defs.GetName(DefCat.ClaimPaymentGroups,ListClaimPay[i].PayGroup));
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
					row.Cells.Add(Clinics.GetAbbr(ListClaimPay[i].ClinicNum));
				}
				row.Cells.Add(ListClaimPay[i].Note);
				row.Cells.Add((tableClaimPayments.Rows[i]["hasEobAttach"].ToString()=="1")?"X":"");
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			gridMain.ScrollToEnd();
		}
		
		private void butAdd_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.InsPayCreate)) {//date not checked here, but it will be checked when saving the check to prevent backdating
				return;
			}
			ClaimPayment claimPayment=new ClaimPayment();
			claimPayment.CheckDate=DateTime.Now;
			claimPayment.IsPartial=true;
			FormClaimPayEdit FormCPE=new FormClaimPayEdit(claimPayment);
			FormCPE.IsNew=true;
			FormCPE.ShowDialog();
			if(FormCPE.DialogResult!=DialogResult.OK) {
				return;
			}
			FormClaimPayBatch FormCPB=new FormClaimPayBatch(claimPayment);
			FormCPB.Show();
			FormCPB.FormClosed+=FormCPB_FormClosed;
		}

		private void FormCPB_FormClosed(object sender,FormClosedEventArgs e) {
			if(IsDisposed) {//Auto-Logoff was causing an unhandled exception below.  Can't use dialogue result check here because we want to referesh the grid below even if user clicked cancel.
				return; //Don't refresh the grid, as the form is already disposed.
			}
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!Security.IsAuthorized(Permissions.InsPayCreate)) {
				return;
			}
			FormClaimPayBatch FormCPBEdit=new FormClaimPayBatch(ListClaimPay[gridMain.GetSelectedIndex()]);
			FormCPBEdit.Show();
			FormCPBEdit.FormClosed+=FormCPBEdit_FormClosed;
		}

		private void FormCPBEdit_FormClosed(object sender,FormClosedEventArgs e) {
			if(IsDisposed) {//Auto-Logoff was causing an unhandled exception below.  Can't use dialogue result check here because we want to referesh the grid below even if user clicked cancel.
				return; //Don't refresh the grid, as the form is already disposed.
			}
			FormClaimPayBatch FormCPBEdit=(FormClaimPayBatch)sender;
			if(FormCPBEdit.GotoClaimNum!=0){
				GotoClaimNum=FormCPBEdit.GotoClaimNum;
				GotoPatNum=FormCPBEdit.GotoPatNum;
				Close();
			}
			else{
				FillGrid();
			}
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

		private void FillComboPaymentGroup(long selectedDefNum=0) {
			comboPayGroup.Items.Clear();
			comboPayGroup.Items.Add("All");
			comboPayGroup.SelectedIndex=0;
			for(int i=0; i<_listCPGroups.Count; i++) {
				Def defCur=_listCPGroups[i];
				comboPayGroup.Items.Add(defCur.ItemName);
				if(selectedDefNum!=0 && selectedDefNum==defCur.DefNum) {
					comboPayGroup.SelectedIndex=i+1; //+1 to account for the "All" option already added to the combobox
				}
			}
		}

		private void butPickPaymentGroup_Click(object sender,EventArgs e) {
			FormDefinitionPicker FormDP= new FormDefinitionPicker(DefCat.ClaimPaymentGroups);
			FormDP.ShowDialog();
			if(FormDP.DialogResult==DialogResult.OK) {
				if(FormDP.ListSelectedDefs.Count<1) {
					FillComboPaymentGroup();
				}
				else { 
					FillComboPaymentGroup(FormDP.ListSelectedDefs[0].DefNum);
				}
			}
		}
	}
}