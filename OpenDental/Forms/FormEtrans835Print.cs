using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEtrans835Print:ODForm {

		private X835 _x835;
		private Hx835_Claim _claimPaid;
		private int _pagesPrintedCount;
		private bool _isHeadingPrinted;
		private int _headingPrintY;


		///<summary>Set x835 to view all x835.ListClaims info. Otherwise set claimPaid to a specific Hx835_Claim to view.</summary>
		public FormEtrans835Print(X835 x835,Hx835_Claim claimPaid=null) {
			InitializeComponent();
			Lan.F(this);
			_x835=x835;
			_claimPaid=claimPaid;
		}

		private void FormEtrans835Print_Load(object sender,EventArgs e) {
			FillGridMain();
		}

		private void FillGridMain() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Columns.Add(new UI.ODGridColumn("EnterBy",50,HorizontalAlignment.Center));
			gridMain.Columns.Add(new UI.ODGridColumn("Claim",0,HorizontalAlignment.Left));//Dynamic width
			gridMain.Columns.Add(new UI.ODGridColumn("Date",66,HorizontalAlignment.Center));
			gridMain.Columns.Add(new UI.ODGridColumn("Code",40,HorizontalAlignment.Center));
			gridMain.Columns.Add(new UI.ODGridColumn("CodeBill",56,HorizontalAlignment.Center));
			gridMain.Columns.Add(new UI.ODGridColumn("Billed",56,HorizontalAlignment.Right));
			gridMain.Columns.Add(new UI.ODGridColumn("PatResp",48,HorizontalAlignment.Right));
			gridMain.Columns.Add(new UI.ODGridColumn("Allowed",56,HorizontalAlignment.Right));
			gridMain.Columns.Add(new UI.ODGridColumn("InsPay",56,HorizontalAlignment.Right));
			gridMain.Rows.Clear();
			List<Hx835_Claim> listClaims;
			if(_x835!=null) {
				listClaims=_x835.ListClaimsPaid;
			}
			else {
				listClaims=new List<Hx835_Claim>() { _claimPaid };
			}
			for(int i=0;i<listClaims.Count;i++) {
				Hx835_Claim claimPaid=listClaims[i];
				UI.ODGridRow rowClaim=new UI.ODGridRow();
				//If there is no procedure detail, then the user will need to enter by total, because they will not know the procedure amounts paid.
				if(claimPaid.ListProcs.Count==0) {
					rowClaim.Cells.Add(new UI.ODGridCell("Total"));//EnterBy
				}
				//If there is procedure detail, and there are also claim level adjustments, then the user will need to enter the procedure amounts by procedure and the claim adjustment by total.
				else if(claimPaid.ClaimAdjustmentTotal!=0) {
					rowClaim.Cells.Add(new UI.ODGridCell("Proc &\r\nTotal"));//EnterBy
				}
				//If there is procedure detail, and there are no claim level adjustments, the user will need to enter the payments by procedure only.
				else {
					rowClaim.Cells.Add(new UI.ODGridCell("Proc"));//EnterBy
				}
				string strClaim="Patient: "+claimPaid.PatientName;
				if(claimPaid.SubscriberName!=claimPaid.PatientName) {
					strClaim+="\r\nSubscriber: "+claimPaid.SubscriberName;
				}
				if(claimPaid.ClaimTrackingNumber!="") {
					strClaim+="\r\nClaim Identifier: "+claimPaid.ClaimTrackingNumber;
				}
				if(claimPaid.PayerControlNumber!="") {
					strClaim+="\r\nPayer Control Number: "+claimPaid.PayerControlNumber;
				}
				if(claimPaid.ListProcs.Count>0 && claimPaid.ClaimAdjustmentTotal!=0) {
					//If there is no procedure detail, then the user will need to enter the claim payment by total.  In this case, the user only cares about the InsPaid for the entire claim.  Showing the adjustments would cause user confusion.
					//If there is procedure detail, then we need to show the claim adjustment total, because the user will need to enter this amount by total in addition to any procedure amounts entered.
					strClaim+="\r\nClaim Adjustments: "+claimPaid.ClaimAdjustmentTotal.ToString("f2");
				}
				rowClaim.Cells.Add(new UI.ODGridCell(strClaim));//Claim
				string strDateClaim=claimPaid.DateServiceStart.ToShortDateString();
				if(claimPaid.DateServiceEnd.Year>1880) {
					strDateClaim+=" to \r\n"+claimPaid.DateServiceEnd.ToShortDateString();
				}
				rowClaim.Cells.Add(new UI.ODGridCell(strDateClaim));//Date
				rowClaim.Cells.Add(new UI.ODGridCell(""));//Code
				rowClaim.Cells.Add(new UI.ODGridCell(""));//CodeBilled
				rowClaim.Cells.Add(new UI.ODGridCell(claimPaid.ClaimFee.ToString("f2")));//Billed
				rowClaim.Cells.Add(new UI.ODGridCell(claimPaid.PatientRespAmt.ToString("f2")));//PatResp
				rowClaim.Cells.Add(new UI.ODGridCell(""));//Allowed
				rowClaim.Cells.Add(new UI.ODGridCell(claimPaid.InsPaid.ToString("f2")));//InsPay
				gridMain.Rows.Add(rowClaim);
				for(int j=0;j<claimPaid.ListProcs.Count;j++) {
					Hx835_Proc proc=claimPaid.ListProcs[j];
					UI.ODGridRow rowProc=new UI.ODGridRow();
					rowProc.Cells.Add(new UI.ODGridCell(""));//EnterBy
					rowProc.Cells.Add(new UI.ODGridCell(""));//Claim
					string strDateProc=proc.DateServiceStart.ToShortDateString();
					if(proc.DateServiceEnd.Year>1880) {
						strDateProc+=" to \r\n"+proc.DateServiceEnd.ToShortDateString();
					}
					rowProc.Cells.Add(new UI.ODGridCell(strDateProc));//Date
					rowProc.Cells.Add(new UI.ODGridCell(proc.ProcCodeAdjudicated));//Code
					rowProc.Cells.Add(new UI.ODGridCell(proc.ProcCodeBilled));//CodeBilled
					rowProc.Cells.Add(new UI.ODGridCell(proc.ProcFee.ToString("f2")));//Billed
					rowProc.Cells.Add(new UI.ODGridCell(proc.PatRespTotal.ToString("f2")));//PatResp
					rowProc.Cells.Add(new UI.ODGridCell(proc.AllowedAmt.ToString("f2")));//Allowed
					rowProc.Cells.Add(new UI.ODGridCell(proc.InsPaid.ToString("f2")));//InsPay
					gridMain.Rows.Add(rowProc);
				}
			}
			gridMain.EndUpdate();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			_pagesPrintedCount=0;
			PrintDocument pd=new PrintDocument();
			pd.PrintPage+=new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,50,50);
			//pd.OriginAtMargins=true;
			pd.DefaultPageSettings.Landscape=false;
			if(pd.DefaultPageSettings.PrintableArea.Height==0) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			_isHeadingPrinted=false;
			try {
#if DEBUG
				FormRpPrintPreview formPreview=new FormRpPrintPreview();
				formPreview.printPreviewControl2.Document=pd;
				formPreview.ShowDialog();
#else
					if(PrinterL.SetPrinter(pd,PrintSituation.Default,0,"Electronic remittance advice (ERA) printed")) {
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
			if(!_isHeadingPrinted) {
				text=Lan.g(this,"Electronic Remittance Advice (ERA)");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				yPos+=20;
				_isHeadingPrinted=true;
				_headingPrintY=yPos;
			}
			#endregion
			yPos=gridMain.PrintPage(g,_pagesPrintedCount,bounds,_headingPrintY);
			_pagesPrintedCount++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
			}
			g.Dispose();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
			Close();
		}

	}
}