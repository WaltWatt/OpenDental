using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using OpenDental.UI;
using System.Drawing.Printing;
using System.IO;

namespace OpenDental {
	public partial class FormRpInsPayPlansPastDue:ODForm {
		private bool _headingPrinted;
		private int _pagesPrinted;
		private int _headingPrintH;

		private List<PayPlanExtended> _listPayPlanExtended;
		private List<Provider> _listProviders;
		private List<Clinic> _listClinics;

		public FormRpInsPayPlansPastDue() {
			InitializeComponent();
			Lan.F(this);
    }

    private void FormRpInsPayPlansPastDue_Load(object sender,EventArgs e) {
      FillProvs();
      if(PrefC.HasClinicsEnabled) {
        comboBoxMultiClinics.Visible=true;
        labelClinic.Visible=true;
        FillClinics();
      }
      if(!LoadData()) {
        DialogResult=DialogResult.Cancel;
        return;
      }
      FillGrid();
    }

    private void FillProvs() {
			_listProviders=Providers.GetListReports();
			comboBoxMultiProv.Items.Add(Lan.g(this,"All"));
			for(int i = 0;i<_listProviders.Count;i++) {
				comboBoxMultiProv.Items.Add(_listProviders[i].GetLongDesc());
			}
			comboBoxMultiProv.SetSelected(0,true);
		}

		private void FillClinics() {
			List <int> listSelectedItems=new List<int>();
			_listClinics=Clinics.GetForUserod(Security.CurUser);
			comboBoxMultiClinics.Items.Add(Lan.g(this,"All"));
			if(!Security.CurUser.ClinicIsRestricted) {
				comboBoxMultiClinics.Items.Add(Lan.g(this,"Unassigned"));
				listSelectedItems.Add(1);
			}
			for(int i = 0;i<_listClinics.Count;i++) {
				int curIndex=comboBoxMultiClinics.Items.Add(_listClinics[i].Abbr);
				if(Clinics.ClinicNum==0) {
					listSelectedItems.Add(curIndex);
				}
				if(_listClinics[i].ClinicNum==Clinics.ClinicNum) {
					listSelectedItems.Clear();
					listSelectedItems.Add(curIndex);
				}
			}
			foreach(int index in listSelectedItems) {
				comboBoxMultiClinics.SetSelected(index,true);
			}
		}

		///<summary>Retrieves data and uses them to create new PayPlanExtended objects.
		///Heavy lifting (db calls and double loops) done here once upon load.  This also gets called if the user clicks "Refresh Data".</summary>
		private bool LoadData() {
			List<PayPlan> listPayPlans;
			List<PayPlanCharge> listPayPlanCharges;
			List<ClaimProc> listPayPlanClaimProcs;
			List<Patient> listPatients;
			List<InsPlan> listInsPlans;
			listPayPlans=PayPlans.GetAllOpenInsPayPlans();
      if(listPayPlans.Count==0) {
        MsgBox.Show(this,"There are no insurance payment plans past due.");
        return false;
      }
			listPayPlanCharges=PayPlanCharges.GetForPayPlans(listPayPlans.Select(x => x.PayPlanNum).ToList()).Where(x => x.ChargeType == PayPlanChargeType.Debit).ToList();
			listPayPlanClaimProcs=ClaimProcs.GetForPayPlans(listPayPlans.Select(x => x.PayPlanNum).ToList()
				,new List<ClaimProcStatus>() {ClaimProcStatus.Received,ClaimProcStatus.Supplemental });
			listPatients=Patients.GetLimForPats(listPayPlans.Select(x => x.PatNum).ToList());
			listInsPlans=InsPlans.GetPlans(listPayPlans.Select(x => x.PlanNum).ToList());
			_listPayPlanExtended=new List<PayPlanExtended>();
			foreach(PayPlan plan in listPayPlans) {
				//for each payplan, create a PayPlanExtended object which contains all of the payment plan's information and it's charges.
				//pass in the plan, the list of associated charges, and the list of associated claimprocs (payments).
				_listPayPlanExtended.Add(new PayPlanExtended(plan,
					listPatients.FirstOrDefault(x => x.PatNum == plan.PatNum),
					listPayPlanCharges.Where(x => x.PayPlanNum == plan.PayPlanNum).ToList(),
					listPayPlanClaimProcs.Where(x => x.PayPlanNum == plan.PayPlanNum).ToList(),
					listInsPlans.FirstOrDefault(x => x.PlanNum == plan.PlanNum)));
			}
      return true;
		}

		///<summary>Actually fill the grid with the data. Filtering based on the user-defined criteria gets done here.</summary>
		private void FillGrid() {
			//get the user-entered filter values.
			int daysPassedFilter=PIn.Int(textDaysPastDue.Text,false); //returns 0 if exceptions are thrown.
			List<long> listProvNums=new List<long>();
			if(comboBoxMultiProv.SelectedIndices[0].ToString()=="0") {
				listProvNums.AddRange(_listProviders.Select(x => x.ProvNum).ToList());
			}
			else {
				for(int i = 0;i<comboBoxMultiProv.SelectedIndices.Count;i++) {
					listProvNums.Add((long)_listProviders[(int)comboBoxMultiProv.SelectedIndices[i]-1].ProvNum);
				}
			}
			List<long> listClinicNums=new List<long>();
			if(PrefC.HasClinicsEnabled) {
				if(comboBoxMultiClinics.ListSelectedIndices.Contains(0)) {
					for(int j = 0;j<_listClinics.Count;j++) {
						listClinicNums.Add(_listClinics[j].ClinicNum);//Add all clinics this person has access to.
					}
					if(!Security.CurUser.ClinicIsRestricted) {
						listClinicNums.Add(0);
					}
				}
				else {
					for(int i = 0;i<comboBoxMultiClinics.ListSelectedIndices.Count;i++) {
						if(Security.CurUser.ClinicIsRestricted) {
							listClinicNums.Add(_listClinics[comboBoxMultiClinics.ListSelectedIndices[i]-1].ClinicNum);
						}
						else if(comboBoxMultiClinics.ListSelectedIndices[i]==1) {
							listClinicNums.Add(0);
						}
						else {
							listClinicNums.Add(_listClinics[comboBoxMultiClinics.ListSelectedIndices[i]-2].ClinicNum);
						}
					}
				}
			}
			//fill the grid
			gridMain.BeginUpdate();
			//columns
			gridMain.Columns.Clear();
			ODGridColumn col = new ODGridColumn(Lan.g("TableInsPayPlanPastDue","Patient"),180);
			gridMain.Columns.Add(col);
			col = new ODGridColumn(Lan.g("TableInsPayPlanPastDue","DateLastPmt"),90);
			gridMain.Columns.Add(col);
			col = new ODGridColumn(Lan.g("TableInsPayPlanPastDue","#Overdue"),75);
			gridMain.Columns.Add(col);
			col = new ODGridColumn(Lan.g("TableInsPayPlanPastDue","AmtOverdue"),90);
			gridMain.Columns.Add(col);
			col = new ODGridColumn(Lan.g("TableInsPayPlanPastDue","DaysOverdue"),90);
			gridMain.Columns.Add(col);
			col = new ODGridColumn(Lan.g("TableInsPayPlanPastDue","CarrierName/Phone"),0);
			gridMain.Columns.Add(col);
			//rows
			gridMain.Rows.Clear();
			ODGridRow row;
			foreach(PayPlanExtended payPlanCur in _listPayPlanExtended) {
				if(daysPassedFilter > payPlanCur.DaysOverdue || payPlanCur.DaysOverdue < 1) {
					continue;
				}
				if(!listProvNums.Contains(payPlanCur.ListPayPlanCharges[0].ProvNum)) {
					continue;
				}
				if(PrefC.HasClinicsEnabled && (!listClinicNums.Contains(payPlanCur.ListPayPlanCharges[0].ClinicNum))) {
					continue;
				}
				row = new ODGridRow();
				string patName =payPlanCur.PatientCur.LName + ", " + payPlanCur.PatientCur.FName;
				string carrierNamePhone = payPlanCur.CarrierCur.CarrierName+"\r\n"+Lan.g("TableInsPayPlanPastDue","Ph:")+" "+payPlanCur.CarrierCur.Phone;
				row.Cells.Add(patName);
				row.Cells.Add(payPlanCur.DateLastPayment.ToShortDateString());
				row.Cells.Add(payPlanCur.NumChargesOverdue.ToString());
				row.Cells.Add(payPlanCur.AmtOverdue.ToString("f"));
				row.Cells.Add(payPlanCur.DaysOverdue.ToString());
				row.Cells.Add(carrierNamePhone);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void comboBoxMulti_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void textDaysPastDue_TextChanged(object sender,EventArgs e) {
			timerTypeDays.Stop();
			timerTypeDays.Start();
		}

		private void timerTypeDays_Tick(object sender,EventArgs e) {
			timerTypeDays.Stop();
			FillGrid();
		}

		//Copied from FormRpOutstandingIns.cs
		private void butPrint_Click(object sender,EventArgs e) {
			_pagesPrinted=0;
			PrintDocument pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			//pd.DefaultPageSettings.Landscape=!checkIgnoreCustom.Checked;//If we are including custom tracking, print in landscape mode.
			if(pd.DefaultPageSettings.PrintableArea.Height==0) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			_headingPrinted=false;
			try {
#if DEBUG
				FormRpPrintPreview pView = new FormRpPrintPreview();
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

		//Copied from FormRpOutstandingIns.cs
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
			if(!_headingPrinted) {
				text=Lan.g(this,"Outstanding Insurance Payment Plans");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				if(comboBoxMultiProv.SelectedIndices[0].ToString()=="0") {
					text="For All Providers";
				}
				else {
					text="For Providers: ";
					for(int i = 0;i<comboBoxMultiProv.SelectedIndices.Count;i++) {
						if(i!=0) {
							text+=", ";
						}
						text+=_listProviders[(int)comboBoxMultiProv.SelectedIndices[i]-1].Abbr;
					}
				}
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				if(comboBoxMultiClinics.SelectedIndices[0].ToString()=="0") {
					text="For All Clinics";
				}
				else {
					text="For Clinics: ";
					for(int i = 0;i<comboBoxMultiClinics.SelectedIndices.Count;i++) {
						if(i!=0) {
							text+=", ";
						}
						if(comboBoxMultiClinics.SelectedIndices[i].ToString()=="1") {
							text+="Unassigned";
						}
						else {
							text+=_listClinics[(int)comboBoxMultiClinics.SelectedIndices[i]-2].Abbr;
						}
					}
				}
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=20;
				_headingPrinted=true;
				_headingPrintH=yPos;
			}
			#endregion
			yPos=gridMain.PrintPage(g,_pagesPrinted,bounds,_headingPrintH);
			_pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
				//text="Total: $"+total.ToString("F");
				//g.DrawString(text,subHeadingFont,Brushes.Black,center+gridMain.Width/2-g.MeasureString(text,subHeadingFont).Width-10,yPos);
			}
			g.Dispose();
		}

		//Copied from FormRpOutstandingIns.cs
		private void butExport_Click(object sender,System.EventArgs e) {
			SaveFileDialog saveFileDialog=new SaveFileDialog();
			saveFileDialog.AddExtension=true;
			saveFileDialog.FileName="Outstanding Insurance Payment Plans";
			if(!Directory.Exists(PrefC.GetString(PrefName.ExportPath))) {
				try {
					Directory.CreateDirectory(PrefC.GetString(PrefName.ExportPath));
					saveFileDialog.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
				}
				catch {
					//initialDirectory will be blank
				}
			}
			else {
				saveFileDialog.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
			}
			saveFileDialog.Filter="Text files(*.txt)|*.txt|Excel Files(*.xls)|*.xls|All files(*.*)|*.*";
			saveFileDialog.FilterIndex=0;
			if(saveFileDialog.ShowDialog()!=DialogResult.OK) {
				return;
			}
			try {
				using(StreamWriter sw = new StreamWriter(saveFileDialog.FileName,false))
				//new FileStream(,FileMode.Create,FileAccess.Write,FileShare.Read)))
				{
					String line="";
					for(int i = 0;i<gridMain.Columns.Count;i++) {
						line+=gridMain.Columns[i].Heading+"\t";
					}
					sw.WriteLine(line);
					for(int i = 0;i<gridMain.Rows.Count;i++) {
						line="";
						for(int j = 0;j<gridMain.Columns.Count;j++) {
							line+=gridMain.Rows[i].Cells[j].Text;
							if(j<gridMain.Columns.Count-1) {
								line+="\t";
							}
						}
						sw.WriteLine(line);
					}
				}
			}
			catch {
				MessageBox.Show(Lan.g(this,"File in use by another program.  Close and try again."));
				return;
			}
			MessageBox.Show(Lan.g(this,"File created successfully"));
		}


		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		///<summary>Class that contains a singular payment plan and all relevant information to be displayed in the grid.
		///Pass in the payplan, patient, list of payplancharges for that payplan, list of claimprocs attached to that payplan, and insplan when callingt he constructor.</summary>
		public class PayPlanExtended {
			//passed in
			public PayPlan PayPlan;
			public List<PayPlanCharge> ListPayPlanCharges;
			public List<ClaimProc> ListClaimProcs;
			public Patient PatientCur;
			public InsPlan InsPlanCur;
			//retrieved
			public Carrier CarrierCur;
			//calculated
			public DateTime DateOldestUnpaid;
			public double AmtOverdue;
			public int NumChargesOverdue;
			public int DaysOverdue;
			public DateTime DateLastPayment;

			public PayPlanExtended(PayPlan payPlan,Patient patCur,List<PayPlanCharge> listPayPlanCharges,List<ClaimProc> listClaimProcs,InsPlan insPlan) {
				//assign passed-in values
				PayPlan=payPlan;
				if(patCur == null) {
					PatientCur=new Patient();
				}
				else {
					PatientCur=patCur;
				}
				ListPayPlanCharges=listPayPlanCharges;
				ListClaimProcs=listClaimProcs;
				if(insPlan == null) {
					InsPlanCur = new InsPlan();
				}
				else {
					InsPlanCur=insPlan;
				}
				//find carrierCur. GetCarrier uses the H List if possible.
				CarrierCur = Carriers.GetCarrier(InsPlanCur.CarrierNum);
				CalculateOverdues();
			}

			private void CalculateOverdues() {
				//calculate AmtOverdue, NumChargesOverdue, and DaysOverdue
				DateLastPayment=DateTime.MinValue;
				foreach(ClaimProc claimProcCur in ListClaimProcs) {
					if(claimProcCur.DateCP > DateLastPayment) {
						DateLastPayment = claimProcCur.DateCP;
					}
					double payAmt = claimProcCur.InsPayAmt;
					foreach(PayPlanCharge payPlanChargeCur in ListPayPlanCharges) {
						if(payAmt<=0) {
							break;
						}
						if(payAmt>=payPlanChargeCur.Interest) {
							payAmt-=payPlanChargeCur.Interest;
							payPlanChargeCur.Interest = 0;
						}
						else {
							payPlanChargeCur.Interest -= payAmt;
							payAmt=0;
						}
						if(payAmt>=payPlanChargeCur.Principal) {
							payAmt-=payPlanChargeCur.Principal;
							payPlanChargeCur.Principal = 0;
						}
						else {
							payPlanChargeCur.Principal -= payAmt;
							payAmt=0;
						}
					}
				}
				List<PayPlanCharge> listChargesOverdue=ListPayPlanCharges.Where(x => (x.ChargeDate < DateTimeOD.Today) && (x.Principal + x.Interest > 0)).ToList();
				if(listChargesOverdue.Count>0) {
					AmtOverdue=listChargesOverdue.Sum(x => x.Principal + x.Interest);
					NumChargesOverdue=listChargesOverdue.Count;
					DateOldestUnpaid=listChargesOverdue.Min(x => x.ChargeDate);
					DaysOverdue=(DateTimeOD.Today - DateOldestUnpaid).Days;
				}
				else {
					AmtOverdue=0;
					NumChargesOverdue=0;
					DaysOverdue=0;
				}
			}
		}
  }


}