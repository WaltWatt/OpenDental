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

namespace OpenDental {
	public partial class FormEtrans277Edit:ODForm {

		public Etrans EtransCur;
		private string MessageText;
		private X277 x277;

		public FormEtrans277Edit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormEtrans277Edit_Load(object sender,EventArgs e) {
			MessageText=EtransMessageTexts.GetMessageText(EtransCur.EtransMessageTextNum);
			x277=new X277(MessageText);
			FillHeader();
			FillGrid();
		}

		private void FormEtrans277Edit_Resize(object sender,EventArgs e) {
			//This funciton is called before FormEtrans277Edit_Load() when using ShowDialog(). Therefore, x277 is null the first time FormEtrans277Edit_Resize() is called.
			if(x277==null) {
				return;
			}
			FillGrid();
		}

		private void FillHeader() {
			//Set the title of the window to include he reponding entity type and name (i.e. payor delta, clearinghouse emdeon, etc...)
			Text+=x277.GetInformationSourceType()+" "+x277.GetInformationSourceName();
			//Fill the textboxes in the upper portion of the window.
			textReceiptDate.Text=x277.GetInformationSourceReceiptDate().ToShortDateString();
			textProcessDate.Text=x277.GetInformationSourceProcessDate().ToShortDateString();
			//textQuantityAccepted.Text=x277.GetQuantityAccepted().ToString();
			//textQuantityRejected.Text=x277.GetQuantityRejected().ToString();
			//textAmountAccepted.Text=x277.GetAmountAccepted().ToString("F");
			//textAmountRejected.Text=x277.GetAmountRejected().ToString("F");
		}

		private void FillGrid() {
			int numAccepted=0;
			int numRejected=0;
			decimal amountAccepted=0;
			decimal amountRejected=0;
			List<string> claimTrackingNumbers=x277.GetClaimTrackingNumbers();
			//bool showInstBillType=false;
			bool showServiceDateRange=false;
			for(int i=0;i<claimTrackingNumbers.Count;i++) {
				string[] claimInfo=x277.GetClaimInfo(claimTrackingNumbers[i]);
				//if(claimInfo[5]!="") { //institutional type of bill
				//  showInstBillType=true;
				//}
				if(claimInfo[7]!="") {//service date end
					showServiceDateRange=true;
				}
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			int variableWidth=this.Width-2*gridMain.Left-10;
			if(showServiceDateRange) {
				const int serviceDateFromWidth=86;
				col=new ODGridColumn(Lan.g(this,"ServDateFrom"),serviceDateFromWidth,HorizontalAlignment.Center);
				gridMain.Columns.Add(col);
				variableWidth-=serviceDateFromWidth;
				const int serviceDateToWidth=80;
				col=new ODGridColumn(Lan.g(this,"ServDateTo"),serviceDateToWidth,HorizontalAlignment.Center);
				gridMain.Columns.Add(col);
				variableWidth-=serviceDateToWidth;
			}
			else {
				const int serviceDateWidth=80;
				col=new ODGridColumn(Lan.g(this,"ServiceDate"),serviceDateWidth,HorizontalAlignment.Center);
				gridMain.Columns.Add(col);
				variableWidth-=serviceDateWidth;
			}
			const int amountWidth=80;
			col=new ODGridColumn(Lan.g(this,"Amount"),amountWidth,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			variableWidth-=amountWidth;
			const int statusWidth=54;
			col=new ODGridColumn(Lan.g(this,"Status"),statusWidth,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			variableWidth-=statusWidth;
			const int lnameWidth=150;
			const int fnameWidth=100;
			const int claimIdWidth=100;
			const int payorControlNumWidth=126;
			variableWidth+=-lnameWidth-fnameWidth-claimIdWidth-payorControlNumWidth;
			col=new ODGridColumn(Lan.g(this,"Reason"),variableWidth);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"LName"),lnameWidth);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"FName"),fnameWidth);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"ClaimIdentifier"),claimIdWidth);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"PayorControlNum"),payorControlNumWidth);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			for(int i=0;i<claimTrackingNumbers.Count;i++) {
				string[] claimInfo=x277.GetClaimInfo(claimTrackingNumbers[i]);
			  ODGridRow row=new ODGridRow();
				row.Cells.Add(new ODGridCell(claimInfo[6]));//service date start
				if(showServiceDateRange) {					
					row.Cells.Add(new ODGridCell(claimInfo[7]));//service date end
				}
				string claimStatus="";
				decimal claimAmount=PIn.Decimal(claimInfo[9]);
				if(claimInfo[3]=="A") {
					claimStatus="Accepted";
					numAccepted++;
					amountAccepted+=claimAmount;
				}
				else if(claimInfo[3]=="R") {
					claimStatus="Rejected";
					numRejected++;
					amountRejected+=claimAmount;
				}
				row.Cells.Add(new ODGridCell(claimAmount.ToString("F")));//amount
				row.Cells.Add(new ODGridCell(claimStatus));//status
				row.Cells.Add(new ODGridCell(claimInfo[8]));//reason
				row.Cells.Add(new ODGridCell(claimInfo[0]));//lname
				row.Cells.Add(new ODGridCell(claimInfo[1]));//fname
				row.Cells.Add(new ODGridCell(claimTrackingNumbers[i]));//claim identifier
				row.Cells.Add(new ODGridCell(claimInfo[4]));//payor control number
			  gridMain.Rows.Add(row);
			}			
			gridMain.EndUpdate();
			textQuantityAccepted.Text=numAccepted.ToString();
			textQuantityRejected.Text=numRejected.ToString();
			textAmountAccepted.Text=amountAccepted.ToString("F");
			textAmountRejected.Text=amountRejected.ToString("F");
		}

		private void butRawMessage_Click(object sender,EventArgs e) {
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(MessageText);
			msgbox.ShowDialog();
		}

		private void butOK_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}
		
	}
}