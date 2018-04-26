/* Receivables Breakdown Report:
 * Author: Bill MacWilliams
 * Company: Kapricorn Systems Inc.
 * 
 * This report will give you the currect calculated receivables upto the date specified.
 * This report does NOT show anything past the date specified.
 * This will allow you to do a breakdown for any month / year with totals for the month
 * at the bottom of the report and a running receivables at the far right. It also gives
 * your a breakdown by day of your receivables.  Currently this report is for the entire
 * practice.
*/
using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;
using OpenDental.ReportingComplex;

namespace OpenDental {

	///<summary></summary>
	public class FormRpReceivablesBreakdown:ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.ComponentModel.Container components = null;
		private MonthCalendar date1;
		private Label label1;
		private System.Windows.Forms.ListBox listProv;
		private Label labelProvider;
		private ListBox listClinic;
		private Label labClinic;
		private GroupBox groupInsBox;
		private RadioButton radioWriteoffPay;
		private RadioButton radioWriteoffProc;
		private Label label2;
		private List<Provider> _listProvs;

		///<summary></summary>
		public FormRpReceivablesBreakdown() {
			InitializeComponent();
			Lan.F(this);
		}

		///<summary></summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpReceivablesBreakdown));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.date1 = new System.Windows.Forms.MonthCalendar();
			this.label1 = new System.Windows.Forms.Label();
			this.listProv = new System.Windows.Forms.ListBox();
			this.labelProvider = new System.Windows.Forms.Label();
			this.listClinic = new System.Windows.Forms.ListBox();
			this.labClinic = new System.Windows.Forms.Label();
			this.groupInsBox = new System.Windows.Forms.GroupBox();
			this.radioWriteoffProc = new System.Windows.Forms.RadioButton();
			this.radioWriteoffPay = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.groupInsBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(468,286);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75,26);
			this.butCancel.TabIndex = 3;
			this.butCancel.Text = "&Cancel";
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(378,286);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75,26);
			this.butOK.TabIndex = 2;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// date1
			// 
			this.date1.Location = new System.Drawing.Point(317,56);
			this.date1.Name = "date1";
			this.date1.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(314,37);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(230,16);
			this.label1.TabIndex = 5;
			this.label1.Text = "Up to the following date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listProv
			// 
			this.listProv.Location = new System.Drawing.Point(27,58);
			this.listProv.Name = "listProv";
			this.listProv.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listProv.Size = new System.Drawing.Size(126,147);
			this.listProv.TabIndex = 30;
			// 
			// labelProvider
			// 
			this.labelProvider.Location = new System.Drawing.Point(24,39);
			this.labelProvider.Name = "labelProvider";
			this.labelProvider.Size = new System.Drawing.Size(103,16);
			this.labelProvider.TabIndex = 7;
			this.labelProvider.Text = "Providers";
			this.labelProvider.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listClinic
			// 
			this.listClinic.FormattingEnabled = true;
			this.listClinic.Location = new System.Drawing.Point(171,58);
			this.listClinic.Name = "listClinic";
			this.listClinic.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listClinic.Size = new System.Drawing.Size(126,147);
			this.listClinic.TabIndex = 31;
			// 
			// labClinic
			// 
			this.labClinic.Location = new System.Drawing.Point(171,39);
			this.labClinic.Name = "labClinic";
			this.labClinic.Size = new System.Drawing.Size(103,16);
			this.labClinic.TabIndex = 32;
			this.labClinic.Text = "Clinic";
			this.labClinic.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// groupInsBox
			// 
			this.groupInsBox.Controls.Add(this.radioWriteoffProc);
			this.groupInsBox.Controls.Add(this.radioWriteoffPay);
			this.groupInsBox.Location = new System.Drawing.Point(27,217);
			this.groupInsBox.Name = "groupInsBox";
			this.groupInsBox.Size = new System.Drawing.Size(270,66);
			this.groupInsBox.TabIndex = 33;
			this.groupInsBox.TabStop = false;
			this.groupInsBox.Text = "Show Insurance Writeoffs";
			// 
			// radioWriteoffProc
			// 
			this.radioWriteoffProc.Location = new System.Drawing.Point(7,38);
			this.radioWriteoffProc.Name = "radioWriteoffProc";
			this.radioWriteoffProc.Size = new System.Drawing.Size(199,17);
			this.radioWriteoffProc.TabIndex = 1;
			this.radioWriteoffProc.TabStop = true;
			this.radioWriteoffProc.Text = "Using procedure date.";
			this.radioWriteoffProc.UseVisualStyleBackColor = true;
			// 
			// radioWriteoffPay
			// 
			this.radioWriteoffPay.Location = new System.Drawing.Point(7,20);
			this.radioWriteoffPay.Name = "radioWriteoffPay";
			this.radioWriteoffPay.Size = new System.Drawing.Size(240,17);
			this.radioWriteoffPay.TabIndex = 0;
			this.radioWriteoffPay.TabStop = true;
			this.radioWriteoffPay.Text = "Using insurance payment date.";
			this.radioWriteoffPay.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24,9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(519,20);
			this.label2.TabIndex = 34;
			this.label2.Text = "This report does not take payment plans into account if you are not using Line-It" +
		"em Payment Plans.";
			// 
			// FormRpReceivablesBreakdown
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(567,324);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.groupInsBox);
			this.Controls.Add(this.labClinic);
			this.Controls.Add(this.listClinic);
			this.Controls.Add(this.labelProvider);
			this.Controls.Add(this.listProv);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.date1);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormRpReceivablesBreakdown";
			this.ShowInTaskbar = false;
			this.Text = "Receivables Breakdown";
			this.Load += new System.EventHandler(this.FormRpReceivablesBreakdown_Load);
			this.groupInsBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormRpReceivablesBreakdown_Load(object sender,EventArgs e) {
			_listProvs=Providers.GetListReports();
			radioWriteoffPay.Checked = true;
			listProv.Items.Add(Lan.g(this,"Practice"));
			for(int i = 0;i < _listProvs.Count;i++) {
				listProv.Items.Add(_listProvs[i].GetLongDesc());
			}
			listProv.SetSelected(0,true);
			//if(PrefC.GetBool(PrefName.EasyNoClinics")){
			listClinic.Visible = false;
			labClinic.Visible = false;
			/*}
			else{
					listClinic.Items.Add(Lan.g(this,"Unassigned"));
					listClinic.SetSelected(0,true);
					for(int i=0;i<Clinics.List.Length;i++) {
							listClinic.Items.Add(Clinics.List[i].Description);
							listClinic.SetSelected(i+1,true);
					}
			}*/
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			int payPlanVersion = PrefC.GetInt(PrefName.PayPlansVersion);
			string bDate;
			string eDate;
			decimal rcvStart = 0;
			decimal rcvProd = 0;
			decimal rcvPayPlanCharges = 0;
			decimal rcvAdj = 0;
			decimal rcvWriteoff = 0;
			decimal rcvPayment = 0;
			decimal rcvInsPayment = 0;
			decimal runningRcv = 0;
			decimal rcvDaily = 0;
			decimal[] colTotals = new decimal[9];
			string wMonth;
			string wYear;
			string wDay = "01";
			string wDate;
			// Get the year / month and instert the 1st of the month for stop point for calculated running balance
			wYear = date1.SelectionStart.Year.ToString();
			wMonth = date1.SelectionStart.Month.ToString();
			if(wMonth.Length<2) {
				wMonth = "0" + wMonth;
			}
			wDate = wYear +"-"+ wMonth +"-"+ wDay;
			List<long> listProvNums = new List<long>();
			bool hasAllProvs = (listProv.SelectedIndices[0]==0);
			if(!hasAllProvs) {
				for(int i = 0;i<listProv.SelectedIndices.Count;i++) {
					//Minus 1 due to the 'Practice' option.
					listProvNums.Add(_listProvs[listProv.SelectedIndices[i]-1].ProvNum);
				}
			}
			bool isPayPlan2;
			if(payPlanVersion==2) {
				isPayPlan2=true;
			}
			else {
				isPayPlan2=false;
			}
			ReportComplex report = new ReportComplex(true,false);
			DataTable TableCharge = new DataTable();  //charges
			DataTable TablePayPlanCharge = new DataTable();  //PayPlanCharges
			DataTable TableCapWriteoff = new DataTable();  //capComplete writeoffs
			DataTable TableInsWriteoff = new DataTable();  //ins writeoffs
			DataTable TablePay = new DataTable();  //payments - Patient
			DataTable TableIns = new DataTable();  //payments - Ins, added SPK 
			DataTable TableAdj = new DataTable();  //adjustments
			
			//
			// Main Loop:  This will loop twice 1st loop gets running balance to start of month selected
			//             2nd will break the numbers down by day and calculate the running balances
			//
			for(int j = 0;j <= 1;j++) {
				if(j == 0) {
					bDate = "0001-01-01";
					eDate = wDate;
				}
				else {
					bDate = wDate;
					eDate = POut.Date(date1.SelectionStart.AddDays(1)).Substring(1,10);// Needed because all Queries are < end date to get correct Starting AR
				}
				TableCharge=RpReceivablesBreakdown.GetRecvBreakdownTable(date1.SelectionStart,listProvNums,radioWriteoffPay.Checked,isPayPlan2,wDate,eDate,bDate,"TableCharge");
				if(isPayPlan2) {
					TablePayPlanCharge=RpReceivablesBreakdown.GetRecvBreakdownTable(date1.SelectionStart,listProvNums,radioWriteoffPay.Checked,isPayPlan2,wDate,eDate,bDate,"TablePayPlanCharge");
				}
				TableCapWriteoff=RpReceivablesBreakdown.GetRecvBreakdownTable(date1.SelectionStart,listProvNums,radioWriteoffPay.Checked,isPayPlan2,wDate,eDate,bDate,"TableCapWriteoff");
				TableInsWriteoff=RpReceivablesBreakdown.GetRecvBreakdownTable(date1.SelectionStart,listProvNums,radioWriteoffPay.Checked,isPayPlan2,wDate,eDate,bDate,"TableInsWriteoff");
				TablePay=RpReceivablesBreakdown.GetRecvBreakdownTable(date1.SelectionStart,listProvNums,radioWriteoffPay.Checked,isPayPlan2,wDate,eDate,bDate,"TablePay");
				TableIns=RpReceivablesBreakdown.GetRecvBreakdownTable(date1.SelectionStart,listProvNums,radioWriteoffPay.Checked,isPayPlan2,wDate,eDate,bDate,"TableIns");
				TableAdj=RpReceivablesBreakdown.GetRecvBreakdownTable(date1.SelectionStart,listProvNums,radioWriteoffPay.Checked,isPayPlan2,wDate,eDate,bDate,"TableAdj");
				//Sum up all the transactions grouped by date.
				Dictionary<DateTime,decimal> dictPayPlanCharges = TablePayPlanCharge.Select().GroupBy(x => PIn.Date(x["ChargeDate"].ToString()))
					.ToDictionary(y => y.Key,y => y.ToList().Sum(z => PIn.Decimal(z["Amt"].ToString())));
				//1st Loop Calculate running Accounts Receivable upto the 1st of the Month Selected
				//2nd Loop Calculate the Daily Accounts Receivable upto the Date Selected
				//Finaly Generate Report showing the breakdown upto the date specified with totals for what is on the report
				if(j == 0) {
					for(int k = 0;k < TableCharge.Rows.Count;k++) {
						rcvProd += PIn.Decimal(TableCharge.Rows[k][1].ToString());
					}
					rcvPayPlanCharges+=dictPayPlanCharges.Sum(x => x.Value);
					for(int k = 0;k < TableCapWriteoff.Rows.Count;k++) {
						rcvWriteoff += PIn.Decimal(TableCapWriteoff.Rows[k][1].ToString());
					}
					for(int k = 0;k < TableInsWriteoff.Rows.Count;k++) {
						rcvWriteoff += PIn.Decimal(TableInsWriteoff.Rows[k][1].ToString());
					}
					for(int k = 0;k < TablePay.Rows.Count;k++) {
						rcvPayment += PIn.Decimal(TablePay.Rows[k][1].ToString());
					}
					for(int k = 0;k < TableIns.Rows.Count;k++) {
						rcvInsPayment += PIn.Decimal(TableIns.Rows[k][1].ToString());
					}
					for(int k = 0;k < TableAdj.Rows.Count;k++) {
						rcvAdj += PIn.Decimal(TableAdj.Rows[k][1].ToString());
					}
					TableCharge.Clear();
					TablePayPlanCharge.Clear();
					TableCapWriteoff.Clear();
					TableInsWriteoff.Clear();
					TablePay.Clear();
					TableIns.Clear();
					TableAdj.Clear();
					rcvStart = (rcvProd + rcvPayPlanCharges + rcvAdj - rcvWriteoff) - (rcvPayment + rcvInsPayment);
				}
				else {
					rcvAdj = 0;
					rcvInsPayment = 0;
					rcvPayment = 0;
					rcvProd = 0;
					rcvPayPlanCharges = 0;
					rcvWriteoff = 0;
					rcvDaily = 0;
					runningRcv = rcvStart;
					DataTable TableQ = new DataTable();
					TableQ.Columns.Add("Date");
					TableQ.Columns.Add("Charges");
					TableQ.Columns.Add("PayPlanCharges");
					TableQ.Columns.Add("Adjustments");
					TableQ.Columns.Add("Writeoffs");
					TableQ.Columns.Add("Payment");
					TableQ.Columns.Add("InsPayment");
					TableQ.Columns.Add("Daily");
					TableQ.Columns.Add("Running");
					eDate = POut.Date(date1.SelectionStart).Substring(1,10);// Reset EndDate to Selected Date
					DateTime[] dates = new DateTime[(PIn.Date(eDate) - PIn.Date(bDate)).Days + 1];
					for(int i = 0;i < dates.Length;i++) {//usually 31 days in loop
						dates[i] = PIn.Date(bDate).AddDays(i);
						//create new row called 'row' based on structure of TableQ
						DataRow row = TableQ.NewRow();
						row[0] = dates[i].ToShortDateString();
						for(int k = 0;k < TableCharge.Rows.Count;k++) {
							if(dates[i] == (PIn.Date(TableCharge.Rows[k][0].ToString()))) {
								rcvProd += PIn.Decimal(TableCharge.Rows[k][1].ToString());
							}
						}
						decimal rcvPayPlanChargesForDay = 0;
						if(dictPayPlanCharges.TryGetValue(dates[i],out rcvPayPlanChargesForDay)) {
							rcvPayPlanCharges += rcvPayPlanChargesForDay;
						}
						for(int k = 0;k < TableCapWriteoff.Rows.Count;k++) {
							if(dates[i] == (PIn.Date(TableCapWriteoff.Rows[k][0].ToString()))) {
								rcvWriteoff += PIn.Decimal(TableCapWriteoff.Rows[k][1].ToString());
							}
						}
						for(int k = 0;k < TableAdj.Rows.Count;k++) {
							if(dates[i] == (PIn.Date(TableAdj.Rows[k][0].ToString()))) {
								rcvAdj += PIn.Decimal(TableAdj.Rows[k][1].ToString());
							}
						}
						for(int k = 0;k < TableInsWriteoff.Rows.Count;k++) {
							if(dates[i] == (PIn.Date(TableInsWriteoff.Rows[k][0].ToString()))) {
								rcvWriteoff += PIn.Decimal(TableInsWriteoff.Rows[k][1].ToString());
							}
						}
						for(int k = 0;k < TablePay.Rows.Count;k++) {
							if(dates[i] == (PIn.Date(TablePay.Rows[k][0].ToString()))) {
								rcvPayment += PIn.Decimal(TablePay.Rows[k][1].ToString());
							}
						}
						for(int k = 0;k < TableIns.Rows.Count;k++) {
							if(dates[i] == (PIn.Date(TableIns.Rows[k][0].ToString()))) {
								rcvInsPayment += PIn.Decimal(TableIns.Rows[k][1].ToString());
							}
						}
						//rcvPayPlanCharges will be 0 if not on version 2.
						rcvDaily = (rcvProd + rcvPayPlanCharges + rcvAdj - rcvWriteoff) - (rcvPayment + rcvInsPayment);
						runningRcv += (rcvProd + rcvPayPlanCharges + rcvAdj - rcvWriteoff) - (rcvPayment + rcvInsPayment);
						row["Charges"] = rcvProd.ToString("n");
						row["PayPlanCharges"] = rcvPayPlanCharges.ToString("n");
						row["Adjustments"] = rcvAdj.ToString("n");
						row["Writeoffs"] = rcvWriteoff.ToString("n");
						row["Payment"] = rcvPayment.ToString("n");
						row["InsPayment"] = rcvInsPayment.ToString("n");
						row["Daily"] = rcvDaily.ToString("n");
						row["Running"] = runningRcv.ToString("n");
						colTotals[1] += rcvProd;
						colTotals[2] += rcvPayPlanCharges;
						colTotals[3] += rcvAdj;
						colTotals[4] += rcvWriteoff;
						colTotals[5] += rcvPayment;
						colTotals[6] += rcvInsPayment;
						colTotals[7] += rcvDaily;
						colTotals[8] = runningRcv;
						TableQ.Rows.Add(row);  //adds row to table Q
						rcvAdj = 0;
						rcvInsPayment = 0;
						rcvPayment = 0;
						rcvProd = 0;
						rcvPayPlanCharges = 0;
						rcvWriteoff = 0;
					}
					//Drop the PayPlanCharges column if not version 2
					if(payPlanVersion!=2) {
						TableQ.Columns.RemoveAt(2);
					}
					////columnCount will get incremented on the second ColTotal call so we can use it in this way
					//if(payPlanVersion==2) {
					//	report.ColTotal[1]=PIn.Decimal(colTotals[1].ToString("n")); //prod
					//	report.ColTotal[2]=PIn.Decimal(colTotals[2].ToString("n")); //payplancharges
					//	report.ColTotal[3]=PIn.Decimal(colTotals[3].ToString("n")); //adjustment
					//	report.ColTotal[4]=PIn.Decimal(colTotals[4].ToString("n")); //writeoffs
					//	report.ColTotal[5]=PIn.Decimal(colTotals[5].ToString("n")); //payment
					//	report.ColTotal[6]=PIn.Decimal(colTotals[6].ToString("n")); //inspayment
					//	report.ColTotal[7]=PIn.Decimal(colTotals[7].ToString("n")); //daily
					//	report.ColTotal[8]=PIn.Decimal(colTotals[8].ToString("n")); //running
					//}
					//else {
					//	report.ColTotal[1]=PIn.Decimal(colTotals[1].ToString("n")); //prod
					//	report.ColTotal[2]=PIn.Decimal(colTotals[3].ToString("n")); //adjustment
					//	report.ColTotal[3]=PIn.Decimal(colTotals[4].ToString("n")); //writeoffs
					//	report.ColTotal[4]=PIn.Decimal(colTotals[5].ToString("n")); //payment
					//	report.ColTotal[5]=PIn.Decimal(colTotals[6].ToString("n")); //inspayment
					//	report.ColTotal[6]=PIn.Decimal(colTotals[7].ToString("n")); //daily
					//	report.ColTotal[7]=PIn.Decimal(colTotals[8].ToString("n")); //running
					//}
					Font font = new Font("Tahoma",9);
					Font boldFont = new Font("Tahoma",9,FontStyle.Bold);
					Font fontTitle = new Font("Tahoma",17,FontStyle.Bold);
					Font fontSubTitle = new Font("Tahoma",10,FontStyle.Bold);
					report.ReportName=Lan.g(this,"Receivables Breakdown");
					report.AddTitle("Title",Lan.g(this,"Receivables Breakdown"),fontTitle);
					report.AddSubTitle("PracticeTitle",PrefC.GetString(PrefName.PracticeTitle),fontSubTitle);
					report.AddSubTitle("Date SubTitle",date1.SelectionStart.ToString("d"),fontSubTitle);
					string provNames = "";
					for(int i=0;i<listProv.SelectedIndices.Count;i++) {
						if(hasAllProvs) {
							provNames = "All Providers";
							break;
						}
						if(i>0) {
							provNames+=", ";
						}
						provNames+=_listProvs[listProv.SelectedIndices[i]-1].Abbr;
					}
					report.AddSubTitle("Provider SubTitle",provNames);
					int[] summaryGroups1 = {1};
					QueryObject query=report.AddQuery(TableQ,Lan.g(this,"Date")+": "+DateTimeOD.Today.ToString("d"));
					query.AddColumn("Date",72,FieldValueType.Date);
					query.AddColumn("Production",80,FieldValueType.Number);
					query.GetColumnDetail("Production").ContentAlignment=ContentAlignment.MiddleRight;
					if(payPlanVersion==2) {
						query.AddColumn("PayPlan",80,FieldValueType.Number);
						query.GetColumnDetail("PayPlan").ContentAlignment=ContentAlignment.MiddleRight;
					}
					query.AddColumn("Adjustment",80,FieldValueType.Number);
					query.GetColumnDetail("Adjustment").ContentAlignment=ContentAlignment.MiddleRight;
					query.AddColumn("Writeoff",80,FieldValueType.Number);
					query.GetColumnDetail("Writeoff").ContentAlignment=ContentAlignment.MiddleRight;
					query.AddColumn("Payment",80,FieldValueType.Number);
					query.GetColumnDetail("Payment").ContentAlignment=ContentAlignment.MiddleRight;
					query.AddColumn("InsPayment",80,FieldValueType.Number);
					query.GetColumnDetail("InsPayment").ContentAlignment=ContentAlignment.MiddleRight;
					query.AddColumn("Daily A/R",100,FieldValueType.Number);
					query.GetColumnDetail("Daily A/R").ContentAlignment=ContentAlignment.MiddleRight;
					query.AddColumn("Ending A/R",100);
					query.GetColumnDetail("Ending A/R").ContentAlignment=ContentAlignment.MiddleRight;
					query.GetColumnDetail("Ending A/R").Font=boldFont;
					report.AddFooterText("Desc","Receivables Calculation: (Production + PayPlanCharges + Adjustments - Writeoffs) - (Payments + Insurance Payments)",font,0,ContentAlignment.MiddleRight);
					//report.AddText("EndingAR","Final Ending A/R: "+runningRcv.ToString(),boldFont,0,ContentAlignment.MiddleLeft);
					report.AddPageNum(font);
					if(!report.SubmitQueries()) {
						return;
					}
					FormReportComplex FormR=new FormReportComplex(report);
					FormR.ShowDialog();
					DialogResult = DialogResult.OK;
				}//END If 
			}// END For Loop
		}//END OK button Clicked
	}
}
