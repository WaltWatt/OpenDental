using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.WebTypes.Shared.XWeb;

namespace OpenDental {
	public partial class FormXWebTransactions:ODForm {
		///<summary>The XWeb transactions for the selected date range and clinics.</summary>
		private DataTable _tableTrans;
		///<summary>The list of clinics available to the current user.</summary>
		private List<Clinic> _listClinics;

		public FormXWebTransactions() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormXWebTransactions_Load(object sender,EventArgs e) {
			if(PrefC.HasClinicsEnabled) {
				FillClinics();
			}
			else {
				comboClinic.Visible=false;
				labelClinic.Visible=false;
			}
			textDateFrom.Text=DateTime.Today.ToShortDateString();
			textDateTo.Text=DateTime.Today.ToShortDateString();
			FillGrid();
		}

		///<summary>Fills the clinics combo box with the clincs available to this user.</summary>
		private void FillClinics() {
			_listClinics=Clinics.GetForUserod(Security.CurUser);
			comboClinic.Items.Add(Lan.g(this,"All"));
			comboClinic.SelectedIndex=0;
			int offset=1;
			if(!Security.CurUser.ClinicIsRestricted) {
				comboClinic.Items.Add(Lan.g(this,"Unassigned"));
				offset++;
			}
			_listClinics.ForEach(x => comboClinic.Items.Add(x.Abbr));
			comboClinic.SelectedIndex=_listClinics.FindIndex(x => x.ClinicNum==Clinics.ClinicNum)+offset;
			if(comboClinic.SelectedIndex-offset<0) {
				comboClinic.SelectedIndex=0;
			}
		}

		private void FillGrid() {
			List<long> listClinicNums=new List<long>();
			if(PrefC.HasClinicsEnabled && comboClinic.SelectedIndex!=0) {//Not 'All' selected
				if(Security.CurUser.ClinicIsRestricted) {
					listClinicNums.Add(_listClinics[comboClinic.SelectedIndex-1].ClinicNum);//Minus 1 for 'All'
				}
				else {
					if(comboClinic.SelectedIndex==1) {//'Unassigned' selected
						listClinicNums.Add(0);
					}
					else if(comboClinic.SelectedIndex>1) {
						listClinicNums.Add(_listClinics[comboClinic.SelectedIndex-2].ClinicNum);//Minus 2 for 'All' and 'Unassigned'
					}
				}
			}
			else {
				//Send an empty list of clinics to get all transactions
			}
			DateTime dateFrom=PIn.Date(textDateFrom.Text);
			DateTime dateTo=PIn.Date(textDateTo.Text);
			_tableTrans=XWebResponses.GetApprovedTransactions(listClinicNums,dateFrom,dateTo);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"Patient"),120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Amount"),60,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Date"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Tran Type"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Card Number"),140);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Expiration"),70);
			gridMain.Columns.Add(col);
			if(PrefC.HasClinicsEnabled) {
				col=new ODGridColumn(Lan.g(this,"Clinic"),100);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g(this,"Transaction ID"),110);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_tableTrans.Rows.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_tableTrans.Rows[i]["Patient"].ToString());
				row.Cells.Add(PIn.Double(_tableTrans.Rows[i]["Amount"].ToString()).ToString("f"));
				row.Cells.Add(PIn.Date(_tableTrans.Rows[i]["DateTUpdate"].ToString()).ToShortDateString());
				XWebTransactionStatus tranStatus=(XWebTransactionStatus)PIn.Int(_tableTrans.Rows[i]["TransactionStatus"].ToString());
				string tranStatusStr;
				switch(tranStatus) {
					case XWebTransactionStatus.DtgPaymentApproved:
					case XWebTransactionStatus.HpfCompletePaymentApproved:
					case XWebTransactionStatus.HpfCompletePaymentApprovedPartial:
						tranStatusStr="Sale";
						break;
					case XWebTransactionStatus.DtgPaymentReturned:
						tranStatusStr="Return";
						break;
					case XWebTransactionStatus.DtgPaymentVoided:
						tranStatusStr="Void";
						break;
					default://These other values should not be returned from the query.
						tranStatusStr=tranStatus.ToString();
						break;
				}
				row.Cells.Add(tranStatusStr);
				row.Cells.Add(_tableTrans.Rows[i]["MaskedAcctNum"].ToString());
				string expiration=_tableTrans.Rows[i]["ExpDate"].ToString();
				if(expiration.Length>2) {
					expiration=expiration.Substring(0,2)+"/"+expiration.Substring(2);
				}
				row.Cells.Add(expiration);
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(_tableTrans.Rows[i]["Clinic"].ToString());
				}
				row.Cells.Add(_tableTrans.Rows[i]["TransactionID"].ToString());
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			if(textDateFrom.Text==""
				|| textDateTo.Text==""
				|| textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!="") 
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			FillGrid();
		}

		private void gridMain_MouseDown(object sender,MouseEventArgs e) {
			if(e.Button==MouseButtons.Right) {
				gridMain.SetSelected(false);
			}
		}

		private void gridMain_MouseClick(object sender,MouseEventArgs e) {
			if(e.Button==MouseButtons.Right && gridMain.SelectedIndices.Length>0) {
				if(_tableTrans.Rows[gridMain.SelectedIndices[0]]["doesPaymentExist"].ToString()=="1") {
					openPaymentToolStripMenuItem.Visible=true;
				}
				else {
					openPaymentToolStripMenuItem.Visible=false;
				}
				switch((XWebTransactionStatus)PIn.Int(_tableTrans.Rows[gridMain.SelectedIndices[0]]["TransactionStatus"].ToString())) {
					case XWebTransactionStatus.DtgPaymentApproved:
					case XWebTransactionStatus.HpfCompletePaymentApproved:
					case XWebTransactionStatus.HpfCompletePaymentApprovedPartial:
					case XWebTransactionStatus.DtgPaymentReturned:
						voidPaymentToolStripMenuItem.Visible=true;
						processReturnToolStripMenuItem.Visible=true;
						break;
					case XWebTransactionStatus.DtgPaymentVoided:
					default:
						voidPaymentToolStripMenuItem.Visible=false;
						processReturnToolStripMenuItem.Visible=false;
						break;
				}
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(e.Row<0 || !Security.IsAuthorized(Permissions.AccountModule)) {
				return;
			}
			long patNum=PIn.Long(_tableTrans.Rows[e.Row]["PatNum"].ToString());
			GotoModule.GotoAccount(patNum);
		}

		private void menuItemGoTo_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length<1 || !Security.IsAuthorized(Permissions.AccountModule)) {
				return;
			}
			long patNum=PIn.Long(_tableTrans.Rows[gridMain.SelectedIndices[0]]["PatNum"].ToString());
			GotoModule.GotoAccount(patNum);
		}

		private void openPaymentToolStripMenuItem_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length<1) {
				return;
			}
			Payment pay=Payments.GetPayment(PIn.Long(_tableTrans.Rows[gridMain.SelectedIndices[0]]["PaymentNum"].ToString()));
			if(pay==null) {//The payment has been deleted
				MsgBox.Show(this,"This payment no longer exists.");
				return;
			}
			Patient pat=Patients.GetPat(pay.PatNum);
			Family fam=Patients.GetFamily(pat.PatNum);
			FormPayment FormP=new FormPayment(pat,fam,pay,false);
			FormP.ShowDialog();
			FillGrid();
		}

		private void voidPaymentToolStripMenuItem_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.PaymentCreate)) {
				return;
			}
			if(gridMain.SelectedIndices.Length<1
				|| !MsgBox.Show(this,MsgBoxButtons.YesNo,"Void this payment?"))
			{
				return;
			}
			long patNum=PIn.Long(_tableTrans.Rows[gridMain.SelectedIndices[0]]["PatNum"].ToString());
			long xWebResponseNum=PIn.Long(_tableTrans.Rows[gridMain.SelectedIndices[0]]["XWebResponseNum"].ToString());
			string payNote=Lan.g(this,"Void XWeb payment made from within Open Dental")+"\r\n"
				+Lan.g(this,"Amount:")+" "+PIn.Double(_tableTrans.Rows[gridMain.SelectedIndices[0]]["Amount"].ToString()).ToString("f")+"\r\n"
				+Lan.g(this,"Transaction ID:")+" "+_tableTrans.Rows[gridMain.SelectedIndices[0]]["TransactionID"].ToString()+"\r\n"
				+Lan.g(this,"Card Number:")+" "+_tableTrans.Rows[gridMain.SelectedIndices[0]]["MaskedAcctNum"].ToString()+"\r\n"
				+Lan.g(this,"Processed:")+" "+DateTime.Now.ToShortDateString()+" "+DateTime.Now.ToShortTimeString();
			try {
				Cursor=Cursors.WaitCursor;
				XWebs.VoidPayment(patNum,payNote,xWebResponseNum);
				Cursor=Cursors.Default;
				MsgBox.Show(this,"Void successful");
				FillGrid();
			}
			catch(ODException ex) {
				Cursor=Cursors.Default;
				MessageBox.Show(ex.Message);
			}
		}

		private void processReturnToolStripMenuItem_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length<1) {
				return;
			}
			long patNum=PIn.Long(_tableTrans.Rows[gridMain.SelectedIndices[0]]["PatNum"].ToString());
			string alias=_tableTrans.Rows[gridMain.SelectedIndices[0]]["Alias"].ToString();
			List<CreditCard> listCards=CreditCards.GetCardsByToken(alias,
				new List<CreditCardSource> { CreditCardSource.XWeb, CreditCardSource.XWebPortalLogin });
			if(listCards.Count==0) {
				MsgBox.Show(this,"This credit card is no longer stored in the database. Return cannot be processed.");
				return;
			}
			if(listCards.Count>1) {
				MsgBox.Show(this,"There is more than one card in the database with this token. Return cannot be processed due to the risk of charging the "+
					"incorrect card.");
				return;
			}
			FormXWeb FormXW=new FormXWeb(patNum,listCards.FirstOrDefault(),XWebTransactionType.CreditReturnTransaction,createPayment:true);
			FormXW.LockCardInfo=true;
			if(FormXW.ShowDialog()==DialogResult.OK) {
				FillGrid();
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

	}
}