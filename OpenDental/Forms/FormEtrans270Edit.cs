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
	public partial class FormEtrans270Edit:ODForm {
		public Etrans EtransCur;
		private Etrans EtransAck271;
		private string MessageText;
		private string MessageTextAck;
		//public bool IsNew;//this makes no sense.  A 270 will never be new.  Always created, saved, and sent ahead of time.
		///<summary>True if the 270 and response have just been created and are being viewed for the first time.</summary>
		public bool IsInitialResponse;
		///<summary>The list of EB objects parsed from the 270.</summary>
		private List<EB271> listEB;
		private List<DTP271> listDTP;
		private X271 x271;
		public List<Benefit> benList;
		private long PatPlanNum;
		private long PlanNum;
		private bool headingPrinted;
		private int pagesPrinted;
		private int headingPrintH=0;
		private long SubNum;
		private bool _isDependent;
		private long _subPatNum;
		private bool _isConinsuranceInverted;

		public List<DTP271> ListDTP{
			get { return listDTP; }
		}

		public FormEtrans270Edit(long patPlanNum,long planNum,long subNum,bool isDependent,long subPatNum,bool isCoinsuranceInverted) {
			InitializeComponent();
			Lan.F(this);
			PatPlanNum=patPlanNum;
			PlanNum=planNum;
			SubNum=subNum;
			_isDependent=isDependent;
			_subPatNum=subPatNum;
			_isConinsuranceInverted=isCoinsuranceInverted;
		}

		private void FormEtrans270Edit_Load(object sender,EventArgs e) {
			MessageText=EtransMessageTexts.GetMessageText(EtransCur.EtransMessageTextNum);
			MessageTextAck="";
			//textMessageText.Text=MessageText;
			textNote.Text=EtransCur.Note;
			EtransAck271=Etranss.GetEtrans(EtransCur.AckEtransNum);
			x271=null;
			if(EtransAck271!=null) {
				MessageTextAck=EtransMessageTexts.GetMessageText(EtransAck271.EtransMessageTextNum);//.Replace("~","~\r\n");
				if(EtransAck271.Etype==EtransType.BenefitResponse271) {
					x271=new X271(MessageTextAck);
				}
			}
			listDTP=new List<DTP271>();
			if(x271 != null) {
				listDTP=x271.GetListDtpSubscriber();
			}
			radioBenefitSendsPat.Checked=(!_isConinsuranceInverted);
			radioBenefitSendsIns.Checked=(_isConinsuranceInverted);
			FillGridDates();
			CreateListOfBenefits();
			FillGrid();
			FillGridBen();
			if(IsInitialResponse) {
				SelectForImport();
			}
			long patNum=(EtransCur.PatNum==0?_subPatNum:EtransCur.PatNum);//Older 270/217s were always for the subscriber and have etrans.PatNum of 0.
			this.Text+=": "+Patients.GetNameLF(patNum);
		}

		private void FormEtrans270Edit_Shown(object sender,EventArgs e) {
			//The 997, 999, 277, or 835 would only exist for a failure.  A success would be a 271.
			if(EtransAck271!=null && (EtransAck271.Etype==EtransType.Acknowledge_997 || EtransAck271.Etype==EtransType.Acknowledge_999 || EtransAck271.Etype==EtransType.StatusNotify_277 || EtransAck271.Etype==EtransType.ERA_835)) {
				if(IsInitialResponse) {
					MessageBox.Show(EtransCur.Note);
				}
			}
		}

		private void radioModeElect_Click(object sender,EventArgs e) {
			butPrint.Visible=false;
			groupImport.Visible=true;
			butImport.Visible=true;
			labelImport.Visible=true;
			gridBen.Visible=true;
			groupImportBenefit.Visible=true;
			gridMain.Height=gridBen.Top-gridMain.Top-3;
			gridMain.Width=gridBen.Right-gridMain.Left;
			FillGrid();
		}

		private void radioModeMessage_Click(object sender,EventArgs e) {
			butPrint.Visible=true;
			groupImport.Visible=false;
			butImport.Visible=false;
			labelImport.Visible=false;
			gridBen.Visible=false;
			groupImportBenefit.Visible=false;
			gridMain.Height=labelNote.Top-gridMain.Top;
			gridMain.Width=760;//to fit on a piece of paper.
			butPrint.Location=new Point(gridMain.Right-butPrint.Width,butPrint.Location.Y);
			FillGrid();
		}

		private void SelectForImport() {
			for(int i=0;i<listEB.Count;i++) {
				if(listEB[i].Benefitt !=null) {
					gridMain.SetSelected(i,true);
				}
			}
		}

		private void CreateListOfBenefits() {
			listEB=new List<EB271>();
			if(x271 != null) {
				listEB=x271.GetListEB(radioInNetwork.Checked,radioBenefitSendsIns.Checked);
			}
		}

		private void FillGridDates() {
			gridDates.BeginUpdate();
			gridDates.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Date"),150);
			gridDates.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Qualifier"),230);
			gridDates.Columns.Add(col);
			gridDates.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<listDTP.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(DTP271.GetDateStr(listDTP[i].Segment.Get(2),listDTP[i].Segment.Get(3)));
				row.Cells.Add(DTP271.GetQualifierDescript(listDTP[i].Segment.Get(1)));
				gridDates.Rows.Add(row);
			}
			gridDates.EndUpdate();
		}

		private void FillGrid(){
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Response"),360);
			gridMain.Columns.Add(col);
			if(radioModeElect.Checked) {
				col=new ODGridColumn(Lan.g(this,"Note"),212);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"Import As Benefit"),360);
				gridMain.Columns.Add(col);
			}
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<listEB.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(listEB[i].GetDescription(radioModeMessage.Checked,radioBenefitSendsPat.Checked));
				if(radioModeElect.Checked) {
					row.Cells.Add(listEB[i].Segment.Get(5));
					if(listEB[i].Benefitt==null) {
						row.Cells.Add("");
					}
					else {
						row.Cells.Add(listEB[i].Benefitt.ToString(true));
					}
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(e.Col<2) {//raw benefit
				FormEtrans270EBraw FormE=new FormEtrans270EBraw();
				FormE.EB271val=listEB[e.Row];
				FormE.ShowDialog();
				//user can't make changes, so no need to refresh grid.
			}
			else {//generated benefit
				if(listEB[e.Row].Benefitt==null) {//create new benefit
					listEB[e.Row].Benefitt=new Benefit();
					FormBenefitEdit FormB=new FormBenefitEdit(0,PlanNum);
					FormB.IsNew=true;
					FormB.BenCur=listEB[e.Row].Benefitt;
					FormB.ShowDialog();
					if(FormB.BenCur==null) {//user deleted or cancelled
						listEB[e.Row].Benefitt=null;
					}
				}
				else {//edit existing benefit
					FormBenefitEdit FormB=new FormBenefitEdit(0,PlanNum);
					FormB.BenCur=listEB[e.Row].Benefitt;
					FormB.ShowDialog();
					if(FormB.BenCur==null) {//user deleted
						listEB[e.Row].Benefitt=null;
					}
				}
				FillGrid();
			}
		}

		private void FillGridBen() {
			gridBen.BeginUpdate();
			gridBen.Columns.Clear();
			ODGridColumn col=new ODGridColumn("",420);
			gridBen.Columns.Add(col);
			gridBen.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<benList.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(benList[i].ToString());
				gridBen.Rows.Add(row);
			}
			gridBen.EndUpdate();
		}

		private void gridBen_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			int benefitListI=benList.IndexOf(benList[e.Row]);
			FormBenefitEdit FormB=new FormBenefitEdit(0,PlanNum);
			FormB.BenCur=benList[e.Row];
			FormB.ShowDialog();
			if(FormB.BenCur==null) {//user deleted
				benList.RemoveAt(benefitListI);
			}
			FillGridBen();
		}
		
		private void radioBenefitPerct_CheckedChanged(object sender,EventArgs e) {
			_isConinsuranceInverted=radioBenefitSendsIns.Checked;
			CreateListOfBenefits();
			FillGrid();
			SelectForImport();
		}

		private void radioInNetwork_Click(object sender,EventArgs e) {
			CreateListOfBenefits();
			FillGrid();
			SelectForImport();
		}

		private void radioOutNetwork_Click(object sender,EventArgs e) {
			CreateListOfBenefits();
			FillGrid();
			SelectForImport();
		}

		private void butImport_Click(object sender,EventArgs e) {
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				if(listEB[gridMain.SelectedIndices[i]].Benefitt==null){
					MsgBox.Show(this,"All selected rows must contain benefits to import.");
					return;
				}
			}
			Benefit ben;
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				ben=listEB[gridMain.SelectedIndices[i]].Benefitt;
				if(_isDependent && ben.CoverageLevel!=BenefitCoverageLevel.Family) {//Dependent level benefit, set all benefits as patient overrides.
					ben.PlanNum=0;//Must be 0 when setting PatPlanNum.
					ben.PatPlanNum=PatPlanNum;
				}
				else {
					ben.PlanNum=PlanNum;
				}
				benList.Add(ben);
			}
			FillGridBen();
		}

		private void butShowRequest_Click(object sender,EventArgs e) {
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(MessageText);
			msgbox.ShowDialog();
		}

		private void butShowResponse_Click(object sender,EventArgs e) {
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(MessageTextAck);
			msgbox.ShowDialog();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			//only visible in Message mode.
			pagesPrinted=0;
			PrintDocument pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,80);
			//pd.OriginAtMargins=true;
			if(pd.DefaultPageSettings.PrintableArea.Height==0) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			headingPrinted=false;
			try {
				#if DEBUG
					FormRpPrintPreview pView = new FormRpPrintPreview();
					pView.printPreviewControl2.Document=pd;
					pView.ShowDialog();
				#else
					if(PrinterL.SetPrinter(pd,PrintSituation.Default,EtransCur.PatNum,"Electronic benefit request from "+EtransCur.DateTimeTrans.ToShortDateString()+" printed")) {
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
			Font headingFont=new Font("Arial",12,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			if(!headingPrinted) {
				text=Lan.g(this,"Electronic Benefits Response");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				InsSub sub=InsSubs.GetSub(this.SubNum,new List<InsSub>());
				InsPlan plan=InsPlans.GetPlan(this.PlanNum,new List<InsPlan>());
				Patient subsc=Patients.GetPat(sub.Subscriber);
				text=Lan.g(this,"Subscriber: ")+subsc.GetNameFL();
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
				Carrier carrier=Carriers.GetCarrier(plan.CarrierNum);
				if(carrier.CarrierNum!=0) {//not corrupted
					text=carrier.CarrierName;
					g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				}
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
			}
			g.Dispose();
		}

		/*
		private void butShowResponseDeciph_Click(object sender,EventArgs e) {
			if(!X12object.IsX12(MessageTextAck)) {
				MessageBox.Show("Only works with 997's");
				return;
			}
			X12object x12obj=new X12object(MessageTextAck);
			if(!x12obj.Is997()) {
				MessageBox.Show("Only works with 997's");
				return;
			}
			X997 x997=new X997(MessageTextAck);
			string display=x997.GetHumanReadable();
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(display);
			msgbox.ShowDialog();
		}*/

		private void butDelete_Click(object sender,EventArgs e) {
			//This button is not visible if IsNew
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete entire request and response?")) {
				return;
			}
			if(EtransAck271!=null) {
				EtransMessageTexts.Delete(EtransAck271.EtransMessageTextNum);
				Etranss.Delete(EtransAck271.EtransNum);
			}
			EtransMessageTexts.Delete(EtransCur.EtransMessageTextNum);
			Etranss.Delete(EtransCur.EtransNum);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			EtransCur.Note=textNote.Text;
			Etranss.Update(EtransCur);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			//if(IsNew) {
			//	EtransMessageTexts.Delete(EtransCur.EtransMessageTextNum);
			//	Etranss.Delete(EtransCur.EtransNum);
			//}
			DialogResult=DialogResult.Cancel;
		}

	

		

		

		

		

	

		
	}
}