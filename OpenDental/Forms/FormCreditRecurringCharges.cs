using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CodeBase;
using OpenDental.Bridges;
using OpenDental.PayConnectService;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormCreditRecurringCharges:ODForm {
		///<summary>Holds all recurring charges for the clinics available to be selected by the user (those in _listUserClinics).  A filtered selection of
		///recurring charges will be selected from this table when filling the grid if clinics are enabled and the user filters by clinic(s).</summary>
		private DataTable _tableAllRCs;
		private PrintDocument pd;
		private int pagesPrinted;
		private int headingPrintH;
		private bool headingPrinted;
		private Program _progCur;
		private DateTime _nowDateTime;
		private string _xPath;
		private int _success;
		private int _failed;
		private int _updated;
		///<summary>List of clinics for which the current user is authorized.  If user is not restricted, list will also contain a dummy clinic with
		///ClinicNum=0 for HQ.  List will be empty if the current user is not allowed to access any clinics or clinics are not enabled.</summary>
		private List<Clinic> _listUserClinics;
		///<summary>Subset of _table rows for the currently selected clinics.  _table holds the rows for all clinics for which the user has access.
		///This list is the _table rows that apply to the currently selected clinic and is used to fill the grid.</summary>
		private List<DataRow> _listRowsCur;
		///<summary>True if we are programmatically selecting indexes in listClinics.  This allows us to use the SelectedIndexChanged event handler and
		///disable the event when we are setting the selected indexes programmatically.</summary>
		private bool _isSelecting;

		///<summary>Only works for XCharge,PayConnect, and PaySimple so far.</summary>
		public FormCreditRecurringCharges() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormRecurringCharges_Load(object sender,EventArgs e) {
			if(!PrefC.IsODHQ) {
				checkHideBold.Checked=true;
				checkHideBold.Visible=false;
			}
			if(Programs.IsEnabled(ProgramName.PayConnect)) {
				_progCur=Programs.GetCur(ProgramName.PayConnect);
				labelUpdated.Visible=false;
				checkForceDuplicates.Checked=PIn.Bool(ProgramProperties.GetPropValForClinicOrDefault(_progCur.ProgramNum,
					PayConnect.ProgramProperties.PayConnectForceRecurringCharge,Clinics.ClinicNum));
			}
			else if(Programs.IsEnabled(ProgramName.PaySimple)) {
				_progCur=Programs.GetCur(ProgramName.PaySimple);
				labelUpdated.Visible=false;
				checkForceDuplicates.Checked=false;
				checkForceDuplicates.Visible=false;//PaySimple always rejects identical transactions made within 5 minutes of eachother.
			}
			else if(Programs.IsEnabled(ProgramName.Xcharge)) {
				_progCur=Programs.GetCur(ProgramName.Xcharge);
				_xPath=Programs.GetProgramPath(_progCur);
				checkForceDuplicates.Checked=PIn.Bool(ProgramProperties.GetPropValForClinicOrDefault(_progCur.ProgramNum,
					XCharge.ProgramProperties.XChargeForceRecurringCharge,Clinics.ClinicNum));
				if(!File.Exists(_xPath)) {//program path is invalid
					//if user has setup permission and they want to edit the program path, show the X-Charge setup window
					if(Security.IsAuthorized(Permissions.Setup)
						&& MsgBox.Show(this,MsgBoxButtons.YesNo,"The X-Charge path is not valid.  Would you like to edit the path?"))
					{
						FormXchargeSetup FormX=new FormXchargeSetup();
						FormX.ShowDialog();
						if(FormX.DialogResult==DialogResult.OK) {
							//The user could have correctly enabled the X-Charge bridge, we need to update our local _programCur and _xPath variable2
							_progCur=Programs.GetCur(ProgramName.Xcharge);
							_xPath=Programs.GetProgramPath(_progCur);
						}
					}
					//if the program path still does not exist, whether or not they attempted to edit the program link, tell them to edit and close the form
					if(!File.Exists(_xPath)) {
						MsgBox.Show(this,"The X-Charge program path is not valid.  Edit the program link in order to use the CC Recurring Charges feature.");
						Close();
						return;
					}
				}
			}
			if(_progCur==null) {
				MsgBox.Show(this,"The PayConnect, PaySimple, or X-Charge program link must be enabled in order to use the CC Recurring Charges feature.");
				Close();
				return;
			}
			_isSelecting=true;
			_listUserClinics=new List<Clinic>();
			if(PrefC.HasClinicsEnabled) {
				if(!Security.CurUser.ClinicIsRestricted) {
					_listUserClinics.Add(new Clinic() { Description=Lan.g(this,"Unassigned") });
				}
				Clinics.GetForUserod(Security.CurUser).ForEach(x => _listUserClinics.Add(x));
				for(int i=0;i<_listUserClinics.Count;i++) {
					listClinics.Items.Add(_listUserClinics[i].Description);
					listClinics.SetSelected(i,true);
				}
				//checkAllClin.Checked=true;//checked true by default in designer so we don't trigger the event to select all and fill grid
			}
			else {
				groupClinics.Visible=false;
			}
			_isSelecting=false;
			//X-Charge or PayConnect is enabled and if X-Charge is enabled the path to the X-Charge executable is valid
			_nowDateTime=MiscData.GetNowDateTime();
			labelCharged.Text=Lan.g(this,"Charged=")+"0";
			labelFailed.Text=Lan.g(this,"Failed=")+"0";
			FillGrid(true);
			gridMain.SetSelected(true);
			labelSelected.Text=Lan.g(this,"Selected=")+gridMain.SelectedIndices.Length.ToString();
		}

		///<summary>The DataTable used to fill the grid will only be refreshed from the db if isFromDb is true.  Otherwise the grid will be refilled using
		///the existing table.  Only get from the db on load or if the Refresh button is pressed, not when the user is selecting the clinic(s).</summary>
		private void FillGrid(bool isFromDb=false) {
			Cursor=Cursors.WaitCursor;
			if(isFromDb) {
				FillDataTableHelper();
			}
			List<long> listSelectedClinicNums=listClinics.SelectedIndices.OfType<int>().Select(x => _listUserClinics[x].ClinicNum).ToList();
			if(PrefC.HasClinicsEnabled) {
				_listRowsCur=_tableAllRCs.Select().Where(x => listSelectedClinicNums.Contains(PIn.Long(x["ClinicNum"].ToString()))).ToList();
			}
			else {
				_listRowsCur=_tableAllRCs.Select().ToList();
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableRecurring","PatNum"),55));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableRecurring","Name"),PrefC.HasClinicsEnabled?190:220));
			if(PrefC.HasClinicsEnabled) {
				gridMain.Columns.Add(new ODGridColumn(Lan.g("TableRecurring","Clinic"),65));
			}
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableRecurring","Date"),PrefC.HasClinicsEnabled?80:80,HorizontalAlignment.Right));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableRecurring","Family Bal"),PrefC.HasClinicsEnabled?70:85,HorizontalAlignment.Right));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableRecurring","PayPlan Due"),PrefC.HasClinicsEnabled?75:85,HorizontalAlignment.Right));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableRecurring","Total Due"),PrefC.HasClinicsEnabled?65:80,HorizontalAlignment.Right));
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableRecurring","Repeat Amt"),PrefC.HasClinicsEnabled?75:90,HorizontalAlignment.Right));//RptChrgAmt
			gridMain.Columns.Add(new ODGridColumn(Lan.g("TableRecurring","Charge Amt"),PrefC.HasClinicsEnabled?85:100,HorizontalAlignment.Right));
			gridMain.Rows.Clear();
			ODGridRow row;
			foreach(DataRow rawRow in _listRowsCur) {
				row=new ODGridRow();
				double famBalTotal=PIn.Double(rawRow["FamBalTotal"].ToString());//pat bal+payplan due, but if pat bal<0 and payplan due>0 then just payplan due
				double payPlanDue=PIn.Double(rawRow["PayPlanDue"].ToString());
				double chargeAmt=PIn.Double(rawRow["ChargeAmt"].ToString());
				double rptChargeAmt=PIn.Double(rawRow["RepeatChargeAmt"].ToString());//includes repeat charge (from procs if ODHQ) and attached payplan
				row.Cells.Add(rawRow["PatNum"].ToString());
				row.Cells.Add(rawRow["PatName"].ToString());
				if(PrefC.HasClinicsEnabled) {
					Clinic clinicCur=_listUserClinics.FirstOrDefault(x => x.ClinicNum==PIn.Long(rawRow["ClinicNum"].ToString()));
					row.Cells.Add(clinicCur!=null?clinicCur.Description:"");//get description from cache if clinics are enabled
				}
				int billingDay=0;
				if(PrefC.GetBool(PrefName.BillingUseBillingCycleDay)) {
					billingDay=PIn.Int(rawRow["BillingCycleDay"].ToString());
				}
				else {
					billingDay=PIn.Date(rawRow["DateStart"].ToString()).Day;
				}
				DateTime startBillingCycle=DateTimeOD.GetMostRecentValidDate(DateTime.Today.Year,DateTime.Today.Month,billingDay);
				if(startBillingCycle>DateTime.Today) {
					startBillingCycle=startBillingCycle.AddMonths(-1);//Won't give a date with incorrect day.  AddMonths will give the end of the month if needed.
				}
				DateTime dateExcludeIfBefore=PIn.Date(textDate.Text);//If entry is invalid, all charges will be included because this will be MinDate.
				if(startBillingCycle < dateExcludeIfBefore) {
					continue;//Don't show row in grid
				}
				row.Cells.Add(startBillingCycle.ToShortDateString());
				row.Cells.Add(famBalTotal.ToString("c"));
				if(payPlanDue!=0) {
					row.Cells.Add(payPlanDue.ToString("c"));
					//negative family balance does not subtract from payplan amount due and negative payplan amount due does not subtract from family balance due
					double totalBal=(Math.Max(famBalTotal,0));
					if(PrefC.GetInt(PrefName.PayPlansVersion) == 1) {//in PP v2, the PP amt due is included in the pat balance
						totalBal+=Math.Max(payPlanDue,0);
					}
					else if(PrefC.GetInt(PrefName.PayPlansVersion)==2) {
						totalBal=Math.Max(totalBal,payPlanDue);//At minimum, the Total Due should be the Pay Plan Due amount.
					}
					row.Cells.Add(totalBal.ToString("c"));
				}
				else {
					row.Cells.Add("");
					row.Cells.Add(famBalTotal.ToString("c"));
				}
				row.Cells.Add(rptChargeAmt.ToString("c"));
				row.Cells.Add(chargeAmt.ToString("c"));
				if(!checkHideBold.Checked) {
					double diff=(Math.Max(famBalTotal,0)+Math.Max(payPlanDue,0))-rptChargeAmt;
					if(diff.IsZero()) {
						//don't bold anything
					}
					else if(diff>0) {
						row.Cells[6].Bold=YN.Yes;//"Repeating Amt"
						row.Cells[7].Bold=YN.Yes;//"Charge Amt"
					}
					else if(diff<0) {
						row.Cells[5].Bold=YN.Yes;//"Total Due"
						row.Cells[7].Bold=YN.Yes;//"Charge Amt"
					}
				}
				row.Tag=PIn.Long(rawRow["CreditCardNum"].ToString());
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			labelTotal.Text=Lan.g(this,"Total=")+gridMain.Rows.Count.ToString();
			labelSelected.Text=Lan.g(this,"Selected=")+gridMain.SelectedIndices.Length.ToString();
			Cursor=Cursors.Default;
		}

		///<summary>Fills the class-wide _table with recurring charges from the db.  Gets recurring charges for all clinics for which the user has
		///permission to access.  If clinics are not enabled the table will not be filtered by clinic.
		///Only called from FillGrid and only when refreshing from the db.</summary>
		private void FillDataTableHelper() {
			List<long> listClinicNums=new List<long>();
			if(PrefC.HasClinicsEnabled && Security.CurUser.ClinicIsRestricted) {
				listClinicNums=_listUserClinics.Select(x => x.ClinicNum).ToList();
			}
			//if no clinics are selected but clinics are enabled and the user is restricted, the results will be empty so no need to run the report
			//if clinics are enabled and the user is not restricted and selects no clinics, there will not be a clinic filter in the query, so all clinics
			if(PrefC.HasClinicsEnabled && Security.CurUser.ClinicIsRestricted && listClinicNums.Count==0) {
				_tableAllRCs=new DataTable();
			}
			else {
				_tableAllRCs=CreditCards.GetRecurringChargeList(listClinicNums);
				_tableAllRCs.Columns.Add("RepeatChargeAmt");
			}
			Dictionary<long,decimal> dictFamBals=new Dictionary<long,decimal>();//Keeps track of the family balance for each patient
			//Calculate the repeat charge amount and the amount to be charged for each credit card
			DataRow rowCur;
			for(int i=_tableAllRCs.Rows.Count-1;i>-1;i--) {//loop through backwards since we may remove rows if the charge amount is <=0 or patCur==null
				rowCur=_tableAllRCs.Rows[i];
				decimal famBalTotal=PIn.Decimal(rowCur["FamBalTotal"].ToString());
				decimal rptChargeAmt;
				//will be 0 if this is not a payplan row, if negative don't subtract from the FamBalTotal
				decimal payPlanDue=Math.Max(PIn.Decimal(rowCur["PayPlanDue"].ToString()),0);
				long patNum=PIn.Long(rowCur["PatNum"].ToString());
				string procedures=rowCur["Procedures"].ToString();
				//This is a very ineffecient way to get the total of the recurring charges for a card.  Example:  For the customers db, to generate a list of
				//161 cards with recurring charges due, TotalRecurringCharges is called ~2500 times.  We could modify this to get the ProcFee sum for each
				//card in the list with a single query and return the DataTable.  But since this is for HQ only, we will leave it for now.
				if(PrefC.IsODHQ) {//HQ calculates repeating charges based on the presence of procedures on the patient's account that are linked to the CC
					if(PrefC.GetBool(PrefName.BillingUseBillingCycleDay)) {
						rptChargeAmt=(decimal)CreditCards.TotalRecurringCharges(patNum,procedures,PIn.Int(rowCur["BillingCycleDay"].ToString()));
					}
					else {
						rptChargeAmt=(decimal)CreditCards.TotalRecurringCharges(patNum,procedures,PIn.Date(rowCur["DateStart"].ToString()).Day);
					}
					rptChargeAmt+=payPlanDue;//payPlanDue will be 0 if this is not a payplan row.  If negative amount due on payplan, payPlanDue is set to 0 above
				}
				else {//non-HQ calculates repeating charges by the ChargeAmt on the credit card which is the sum of repeat charge and payplan payment amount
					rptChargeAmt=PIn.Decimal(rowCur["ChargeAmt"].ToString());
				}
				//the Total Bal column should display the famBalTotal plus payPlanDue on the attached payplan if there is one with a positive amount due
				//if the payplan has a negative amount due, it is set to 0 above and does not subtract from famBalTotal
				//if the account balance is negative, the Total Bal column should still display the entire amount due on the payplan (if >0)
				//if the account balance is negative and there is no payplan, the Total Bal column will be the negative account balance
				if(payPlanDue>0) {//if there is a payplan attached to this repeatcharge and a positive amount due
					//negative family balance does not subtract from payplan amount due and negative payplan amount due does not subtract from family balance due
					famBalTotal=Math.Max(famBalTotal,0);
					if(PrefC.GetInt(PrefName.PayPlansVersion)==1) {//in PP v2, the PP amt due is included in the pat balance
						famBalTotal+=payPlanDue;
					}
				}
				long guarNum=PIn.Long(rowCur["Guarantor"].ToString());
				//if guarantor is already in the dict and this is a payplan charge row, add the payPlanDue to fambal so the patient is charged
				if(dictFamBals.ContainsKey(guarNum) && payPlanDue>0 
					&& PrefC.GetInt(PrefName.PayPlansVersion)==1) //in PP v2, the PP amt due is included in the pat balance
				{
					dictFamBals[guarNum]=Math.Max(dictFamBals[guarNum],0)+payPlanDue;//this way the payplan charge will be charged even if the fam bal is < 0
				}
				if(!dictFamBals.ContainsKey(guarNum)) {
					dictFamBals.Add(guarNum,famBalTotal);
				}
				//05/22/2017 Nathan and Chris discussed making it so that a credit card attached to a pay plan would charge no more than the Due Now on the
				//pay plan, but this messed up our internal accounting department because they had cards attached to payment plans that also were paying
				//down the account balance. In the future if our customers desire it, we can come up with a way so that a credit card attached to a pay plan
				//only charges the amount on the pay plan.
				decimal chargeAmt=Math.Max(dictFamBals[guarNum],payPlanDue);
				//Make sure the charge amount is not more than the repeat charge amount
				chargeAmt=Math.Min(chargeAmt,rptChargeAmt);
				if(chargeAmt<=0) {
					_tableAllRCs.Rows.RemoveAt(i);
					continue;
				}
				rowCur["ChargeAmt"]=chargeAmt;
				rowCur["RepeatChargeAmt"]=rptChargeAmt;
				dictFamBals[guarNum]-=chargeAmt;//Decrease so the sum of repeating charges on all cards is not greater than the family balance
			}
		}
		
		private void listClinics_SelectedIndexChanged(object sender,EventArgs e) {
			//if the selected indices have not changed, don't refill the grid
			if(_isSelecting) {
				return;
			}
			RefreshRecurringCharges(true,false);
			checkAllClin.Checked=(listClinics.SelectedIndices.Count==listClinics.Items.Count);
		}
		
		private void checkAllClin_Click(object sender,EventArgs e) {
			if(!checkAllClin.Checked) {
				return;
			}
			_isSelecting=true;
			for(int i=0;i<listClinics.Items.Count;i++) {
				listClinics.SetSelected(i,true);
			}
			_isSelecting=false;
			RefreshRecurringCharges(true,false);
		}

		///<summary>Returns a valid DateTime for the payment's PayDate.  Contains logic if payment should be for the previous or the current month.</summary>
		private DateTime GetPayDate(DateTime latestPayment,DateTime dateStart) {
			if(PrefC.GetBool(PrefName.RecurringChargesUseTransDate)) {
				return _nowDateTime;
			}
			return GetRecurringChargeDate(dateStart);
		}

		///<summary>Returns a date for the recurring charge that this payment applies to.</summary>
		private DateTime GetRecurringChargeDate(DateTime dateStart) {
			//Most common, current day >= dateStart so we use current month and year with the dateStart day.  Will always be a legal DateTime.
			if(_nowDateTime.Day>=dateStart.Day) {
				return new DateTime(_nowDateTime.Year,_nowDateTime.Month,dateStart.Day);
			}
			//If not enough days in current month to match the dateStart see if on the last day in the month.
			//Example: dateStart=08/31/2009 and month is February 28th so we need the PayDate to be today not for last day on the last month, which would happen below.
			int daysInMonth=DateTime.DaysInMonth(_nowDateTime.Year,_nowDateTime.Month);
			if(daysInMonth<=dateStart.Day && daysInMonth==_nowDateTime.Day) {
				return _nowDateTime;//Today is last day of the month so return today as the PayDate.
			}
			//PayDate needs to be for the previous month so we need to determine if using the dateStart day would be a legal DateTime.
			DateTime nowMinusOneMonth=_nowDateTime.AddMonths(-1);
			daysInMonth=DateTime.DaysInMonth(nowMinusOneMonth.Year,nowMinusOneMonth.Month);
			if(daysInMonth<=dateStart.Day) {
				return new DateTime(nowMinusOneMonth.Year,nowMinusOneMonth.Month,daysInMonth);//Returns the last day of the previous month.
			}
			return new DateTime(nowMinusOneMonth.Year,nowMinusOneMonth.Month,dateStart.Day);//Previous month contains a legal date using dateStart's day.
		}

		///<summary>Tests the selected indicies with newly calculated pay dates.  If there's a date violation, a warning shows and false is returned.</summary>
		private bool PaymentsWithinLockDate() {
			//Check if user has the payment create permission in the first place to save time.
			if(!Security.IsAuthorized(Permissions.PaymentCreate,_nowDateTime.Date)) {
				return false;
			}
			List<string> warnings=new List<string>();
			foreach(int i in gridMain.SelectedIndices) {
				DataRow rowCur=_listRowsCur[i];
				//Calculate what the new pay date will be.
				DateTime newPayDate=GetPayDate(PIn.Date(rowCur["LatestPayment"].ToString()),PIn.Date(rowCur["DateStart"].ToString()));
				//Test if the user can create a payment with the new pay date.
				if(!Security.IsAuthorized(Permissions.PaymentCreate,newPayDate,true)) {
					if(warnings.Count==0) {
						warnings.Add(Lan.g(this,"Lock date limitation is preventing the recurring charges from running")+":");
					}
					warnings.Add(newPayDate.ToShortDateString()+" - "+rowCur["PatNum"].ToString()+": "+rowCur["PatName"].ToString()+" - "
						+PIn.Double(rowCur["FamBalTotal"].ToString()).ToString("c")+" - "+PIn.Double(rowCur["ChargeAmt"].ToString()).ToString("c"));
				}
			}
			if(warnings.Count>0) {
				//Show the warning message.  This allows the user the ability to unhighlight rows or go change the date limitation.
				MessageBox.Show(string.Join("\r\n",warnings));
				return false;
			}
			return true;
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			labelSelected.Text=Lan.g(this,"Selected=")+gridMain.SelectedIndices.Length.ToString();
		}
		
		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!Security.IsAuthorized(Permissions.AccountModule)) {
				return;
			}
			if(e.Row<0) {
				MsgBox.Show(this,"Must select at least one recurring charge.");
				return;
			}
			long patNum=PIn.Long(_listRowsCur[e.Row]["PatNum"].ToString());
			GotoModule.GotoAccount(patNum);
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			RefreshRecurringCharges(false,true);
		}

		private void RefreshRecurringCharges(bool isSelectAll,bool isFromDb) {
			Cursor=Cursors.WaitCursor;
			List<long> listSelectedCCNums=gridMain.SelectedIndices.OfType<int>().Select(x => (long)gridMain.Rows[x].Tag).ToList();
			FillGrid(isFromDb);
			labelCharged.Text=Lan.g(this,"Charged=")+"0";
			labelFailed.Text=Lan.g(this,"Failed=")+"0";
			if(isSelectAll) {
				gridMain.SetSelected(true);
			}
			else {
				for(int i=0;i<gridMain.Rows.Count;i++) {
					if(listSelectedCCNums.Contains((long)gridMain.Rows[i].Tag)) {
						gridMain.SetSelected(i,true);
					}
				}
			}
			labelSelected.Text=Lan.g(this,"Selected=")+gridMain.SelectedIndices.Length.ToString();
			Cursor=Cursors.Default;
		}

		private void textDate_TextChanged(object sender,EventArgs e) {
			RefreshRecurringCharges(false,false);
		}

		private void butToday_Click(object sender,EventArgs e) {
			textDate.Text=DateTime.Today.ToShortDateString();
		}

		private void checkHideBold_Click(object sender,EventArgs e) {
			RefreshRecurringCharges(false,false);
		}

		private void butPrintList_Click(object sender,EventArgs e) {
			pagesPrinted=0;
			pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			pd.DefaultPageSettings.Landscape=true;
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
					if(PrinterL.SetPrinter(pd,PrintSituation.Default,0,"CreditCard recurring charges list printed")) {
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
				text=Lan.g(this,"Recurring Charges");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
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

		private void butAll_Click(object sender,EventArgs e) {
			gridMain.SetSelected(true);
			labelSelected.Text=Lan.g(this,"Selected=")+gridMain.SelectedIndices.Length.ToString();
		}

		private void butNone_Click(object sender,EventArgs e) {
			gridMain.SetSelected(false);
			labelSelected.Text=Lan.g(this,"Selected=")+gridMain.SelectedIndices.Length.ToString();
		}

		private void SendXCharge() {
			StringBuilder strBuilderResultFile=new StringBuilder();
			strBuilderResultFile.AppendLine("Recurring charge results for "+DateTime.Now.ToShortDateString()+" ran at "+DateTime.Now.ToShortTimeString());
			strBuilderResultFile.AppendLine();
			string resultfile=PrefC.GetRandomTempFile("txt");
			List<long> listClinicNumsBadCredentials=new List<long>();
			//Making a copy now because the user can change the selected index as we are looping through
			List<int> listSelectedIndices=gridMain.SelectedIndices.ToList();
			foreach(int selectedIndex in listSelectedIndices) {
				DataRow rowCur=_listRowsCur[selectedIndex];
				int tokenCount=CreditCards.GetTokenCount(rowCur["XChargeToken"].ToString(),
					new List<CreditCardSource> { CreditCardSource.XServer,CreditCardSource.XServerPayConnect });
				if(rowCur["XChargeToken"].ToString()!="" && tokenCount!=1) {
					_failed++;
					labelFailed.Text=Lan.g(this,"Failed=")+_failed;
					string msg=(tokenCount>1)?"A duplicate token was found":"A token no longer exists";
					MessageBox.Show(Lan.g(this,msg+", the card cannot be charged for customer")+": "+rowCur["PatName"].ToString());
					continue;
				}
				long clinicNumCur=0;
				if(PrefC.HasClinicsEnabled) {
					//this is patient.ClinicNum or if it's a payplan row it's the ClinicNum from one of the payplancharges on the payplan
					clinicNumCur=PIn.Long(rowCur["ClinicNum"].ToString());//If clinics were enabled but no longer are, use credentials for headquarters.
				}
				if(listClinicNumsBadCredentials.Contains(clinicNumCur)) {//username or password is blank, don't try to process
					_failed++;
					labelFailed.Text=Lan.g(this,"Failed=")+_failed;
					continue;
				}
				string username=ProgramProperties.GetPropVal(_progCur.ProgramNum,"Username",clinicNumCur);
				string password=ProgramProperties.GetPropVal(_progCur.ProgramNum,"Password",clinicNumCur);
				if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) {//clinicNumCur is not in listClinicNumsBadCredentials yet
					_failed++;
					labelFailed.Text=Lan.g(this,"Failed=")+_failed;
					string clinicAbbr="Headquarters";
					if(clinicNumCur>0) {
						clinicAbbr=Clinics.GetAbbr(clinicNumCur);
					}
					MessageBox.Show(this,Lan.g(this,"The X-Charge Username or Password for the following clinic has not been set")+":\r\n"+clinicAbbr+"\r\n"
						+Lan.g(this,"All charges for that clinic will be skipped."));
					listClinicNumsBadCredentials.Add(clinicNumCur);
					continue;
				}
				password=MiscUtils.Decrypt(password);
				ProcessStartInfo info=new ProcessStartInfo(_xPath);
				long patNum=PIn.Long(rowCur["PatNum"].ToString());
				Patient patCur=Patients.GetPat(patNum);
				if(patCur==null) {
					continue;
				}
				try {
					File.Delete(resultfile);//delete the old result file.
				}
				catch {
					//Probably did not have permissions to delete the file.  Don't do anything, because a message will show telling them that the cards left in the grid failed.
					//They will then go try and run the cards in the Account module and will then get a detailed message telling them what is wrong.
					continue;
				}
				info.Arguments="";
				double amt=PIn.Double(rowCur["ChargeAmt"].ToString());
				DateTime exp=PIn.Date(rowCur["CCExpiration"].ToString());
				string address=PIn.String(rowCur["Address"].ToString());
				string addressPat=PIn.String(rowCur["AddressPat"].ToString());
				string zip=PIn.String(rowCur["Zip"].ToString());
				string zipPat=PIn.String(rowCur["ZipPat"].ToString());
				long creditCardNum=PIn.Long(rowCur["CreditCardNum"].ToString());
				info.Arguments+="/AMOUNT:"+amt.ToString("F2")+" /LOCKAMOUNT ";
				info.Arguments+="/TRANSACTIONTYPE:PURCHASE /LOCKTRANTYPE ";
				if(rowCur["XChargeToken"].ToString()!="") {
					info.Arguments+="/XCACCOUNTID:"+rowCur["XChargeToken"].ToString()+" ";
					info.Arguments+="/RECURRING ";
					info.Arguments+="/GETXCACCOUNTIDSTATUS ";
				}
				else {
					info.Arguments+="/ACCOUNT:"+rowCur["CCNumberMasked"].ToString()+" ";
				}
				if(exp.Year>1880) {
					info.Arguments+="/EXP:"+exp.ToString("MMyy")+" ";
				}
				if(address!="") {
					info.Arguments+="\"/ADDRESS:"+address+"\" ";
				}
				else if(addressPat!="") {
					info.Arguments+="\"/ADDRESS:"+addressPat+"\" ";
				}
				//If ODHQ, do not add the zip code if the customer has an active foreign registration key
				bool hasForeignKey=false;
				if(PrefC.IsODHQ) {
					hasForeignKey=RegistrationKeys.GetForPatient(patNum)
						.Where(x => x.IsForeign)
						.Where(x => x.DateStarted<=DateTimeOD.Today)
						.Where(x => x.DateEnded.Year<1880 || x.DateEnded>=DateTimeOD.Today)
						.Where(x => x.DateDisabled.Year<1880 || x.DateDisabled>=DateTimeOD.Today)
						.Count()>0;
				}
				if(zip!="" && !hasForeignKey) {
					info.Arguments+="\"/ZIP:"+zip+"\" ";
				}
				else if(zipPat!="" && !hasForeignKey) {
					info.Arguments+="\"/ZIP:"+zipPat+"\" ";
				}
				info.Arguments+="/RECEIPT:Pat"+patNum+" ";//aka invoice#
				info.Arguments+="\"/CLERK:"+Security.CurUser.UserName+" R\" /LOCKCLERK ";
				info.Arguments+="/RESULTFILE:\""+resultfile+"\" ";
				info.Arguments+="/USERID:"+username+" ";
				info.Arguments+="/PASSWORD:"+password+" ";
				info.Arguments+="/HIDEMAINWINDOW ";
				info.Arguments+="/AUTOPROCESS ";
				info.Arguments+="/SMALLWINDOW ";
				info.Arguments+="/AUTOCLOSE ";
				info.Arguments+="/NORESULTDIALOG ";
				if(checkForceDuplicates.Checked) {
					info.Arguments+="/ALLOWDUPLICATES ";
				}
				info.Arguments+="/RECEIPTINRESULT ";
				Cursor=Cursors.WaitCursor;
				Process process=new Process();
				process.StartInfo=info;
				process.EnableRaisingEvents=true;
				process.Start();
				while(!process.HasExited) {
					Application.DoEvents();
				}
				Thread.Sleep(200);//Wait 2/10 second to give time for file to be created.
				Cursor=Cursors.Default;
				string line="";
				bool updateCard=false;
				string newAccount="";
				double amount=0;
				StringBuilder receipt=new StringBuilder();
				DateTime newExpiration=new DateTime();
				StringBuilder strBuilderResultText=new StringBuilder();
				strBuilderResultFile.AppendLine("PatNum: "+patNum+" Name: "+rowCur["PatName"].ToString());
				try {
					using(TextReader reader=new StreamReader(resultfile)) {
						line=reader.ReadLine();
						while(line!=null) {
							if(!line.StartsWith("RECEIPT=")) {//Don't include the receipt string in the PayNote
								strBuilderResultText.AppendLine(line);
							}
							if(line.StartsWith("RESULT=")) {
								if(line=="RESULT=SUCCESS") {
									_success++;
									labelCharged.Text=Lan.g(this,"Charged=")+_success;
								}
								else {
									_failed++;
									labelFailed.Text=Lan.g(this,"Failed=")+_failed;
								}
							}
							else if(line=="XCACCOUNTIDUPDATED=T") {//Decline minimizer updated the account information since the last time this card was charged
								updateCard=true;
								_updated++;
								labelUpdated.Text=Lan.g(this,"Updated=")+_updated;
							}
							else if(line.StartsWith("ACCOUNT=")) {
								newAccount=line.Substring("ACCOUNT=".Length);
							}
							else if(line.StartsWith("EXPIRATION=")) {
								string expStr=line.Substring("EXPIRATION=".Length);//Expiration should be MMYY
								newExpiration=new DateTime(PIn.Int("20"+expStr.Substring(2)),PIn.Int(expStr.Substring(0,2)),1);//First day of the month
							}
							else if(line.StartsWith("APPROVEDAMOUNT=")) {
								amount=PIn.Double(line.Substring("APPROVEDAMOUNT=".Length));
							}
							else if(line.StartsWith("RECEIPT=")) {
								receipt.Append(line.Substring("RECEIPT=".Length));
								receipt.Replace("\\n","\n");//The receipts from X-Charge escape newline characters.
								receipt.Replace("\r","");//remove any existing \r's before replacing \n's with \r\n's
								receipt.Replace("\n","\r\n");
							}
							line=reader.ReadLine();
						}
						strBuilderResultFile.AppendLine(strBuilderResultText.ToString());
						strBuilderResultFile.AppendLine();
					}
				}
				catch {
					continue;//Cards will still be in the list if something went wrong.
				}
				//If the decline minimizer updated the card, returned a value in the ACCOUNT field, and returned a valid exp date.  Update our record.
				if(updateCard && newAccount!="" && newExpiration.Year>1880) {
					CreditCard creditCardCur=CreditCards.GetOne(creditCardNum);
					//Update the payment note with the changes.
					if(creditCardCur.CCNumberMasked != newAccount) {
						strBuilderResultText.AppendLine(Lan.g(this,"Account number changed from")+" "+creditCardCur.CCNumberMasked+" "
							+Lan.g(this,"to")+" "+newAccount);
					}
					if(creditCardCur.CCExpiration != newExpiration) {
						strBuilderResultText.AppendLine(Lan.g(this,"Expiration changed from")+" "+creditCardCur.CCExpiration.ToString("MMyy")+" "
							+Lan.g(this,"to")+" "+newExpiration.ToString("MMyy"));
					}
					creditCardCur.CCNumberMasked=newAccount;
					creditCardCur.CCExpiration=newExpiration;
					CreditCards.Update(creditCardCur);
				}
				CreatePayment(patCur,selectedIndex,strBuilderResultText.ToString(),amount,receipt.ToString());				
			}
			try {
				File.WriteAllText(ODFileUtils.CombinePaths(Path.GetDirectoryName(_xPath),"RecurringChargeResult.txt"),strBuilderResultFile.ToString());
			}
			catch { } //Do nothing cause this is just for internal use.
		}

		private void SendPayConnect() {
			Dictionary<long,string> dictClinicNumDesc=new Dictionary<long,string>();
			if(PrefC.HasClinicsEnabled) {
				dictClinicNumDesc=Clinics.GetClinicsNoCache().ToDictionary(x => x.ClinicNum,x => x.Description);
			}
			dictClinicNumDesc[0]=PrefC.GetString(PrefName.PracticeTitle);
			StringBuilder strBuilderResultFile=new StringBuilder();
			strBuilderResultFile.AppendLine("Recurring charge results for "+DateTime.Now.ToShortDateString()+" ran at "+DateTime.Now.ToShortTimeString());
			strBuilderResultFile.AppendLine();
			#region Card Charge Loop
			//Making a copy now because the user can change the selected index as we are looping through
			List<int> listSelectedIndices=gridMain.SelectedIndices.ToList();
			foreach(int selectedIndex in listSelectedIndices) {
				DataRow rowCur=_listRowsCur[selectedIndex];
				bool isPayConnectToken=true;
				string tokenOrCCMasked=rowCur["PayConnectToken"].ToString();
				int tokenCount=CreditCards.GetTokenCount(tokenOrCCMasked,
					new List<CreditCardSource> { CreditCardSource.PayConnect,CreditCardSource.XServerPayConnect });
				if(tokenOrCCMasked!="" && tokenCount!=1) {
					_failed++;
					labelFailed.Text=Lan.g(this,"Failed=")+_failed;
					string msg=(tokenCount>1)?"A duplicate token was found":"A token no longer exists";
					MessageBox.Show(Lan.g(this,msg+", the card cannot be charged for customer")+": "+rowCur["PatName"].ToString());
					continue;
				}
				long patNum=PIn.Long(rowCur["PatNum"].ToString());
				Patient patCur=Patients.GetPat(patNum);
				if(patCur==null) {
					continue;
				}
				DateTime exp=PIn.Date(rowCur["PayConnectTokenExp"].ToString());
				if(tokenOrCCMasked=="") {
					isPayConnectToken=false;
					tokenOrCCMasked=rowCur["CCNumberMasked"].ToString();
					exp=PIn.Date(rowCur["CCExpiration"].ToString());
				}
				decimal amt=PIn.Decimal(rowCur["ChargeAmt"].ToString());
				string zip=PIn.String(rowCur["Zip"].ToString());
				long clinicNumCur=PIn.Long(rowCur["ClinicNum"].ToString());
				double amount=0;
				//request a PayConnect token, if a token was already saved PayConnect will return the same token,
				//otherwise replace CCNumberMasked with the returned token if the sale successful
				Cursor=Cursors.WaitCursor;
				bool forceDuplicates=checkForceDuplicates.Checked;
				PayConnectService.creditCardRequest payConnectRequest=PayConnect.BuildSaleRequest(
					amt,tokenOrCCMasked,exp.Year,exp.Month,
					patCur.GetNameFLnoPref(),"",zip,null,
					PayConnectService.transType.SALE,"",true, 
					isForced:forceDuplicates);
				//clinicNumCur could be 0, and the practice level or 'Headquarters' PayConnect credentials would be used for this charge
				PayConnectService.transResponse payConnectResponse=PayConnect.ProcessCreditCard(payConnectRequest,clinicNumCur);
				Cursor=Cursors.Default;
				StringBuilder strBuilderResultText=new StringBuilder();//this payment's result text, used in payment note and then appended to file string builder
				strBuilderResultFile.AppendLine("PatNum: "+patNum+" Name: "+patCur.GetNameFLnoPref());
				if(payConnectResponse==null || payConnectResponse.Status==null) {
					_failed++;
					labelFailed.Text=Lan.g(this,"Failed=")+_failed;
					if(PrefC.HasClinicsEnabled && dictClinicNumDesc.ContainsKey(clinicNumCur)) {
						strBuilderResultText.AppendLine("CLINIC="+dictClinicNumDesc[clinicNumCur]);
					}
					strBuilderResultText.AppendLine(Lan.g(this,"Transaction Failed, unknown error"));
					strBuilderResultFile.AppendLine(strBuilderResultText.ToString());//add to the file string builder
				}
				else if(payConnectResponse.Status.code!=0) {//error in transaction
					_failed++;
					labelFailed.Text=Lan.g(this,"Failed=")+_failed;
					if(PrefC.HasClinicsEnabled && dictClinicNumDesc.ContainsKey(clinicNumCur)) {
						strBuilderResultText.AppendLine("CLINIC="+dictClinicNumDesc[clinicNumCur]);
					}
					strBuilderResultText.AppendLine(Lan.g(this,"Transaction Type")+": "+PayConnectService.transType.SALE.ToString());
					strBuilderResultText.AppendLine(Lan.g(this,"Status")+": "+payConnectResponse.Status.description);
					strBuilderResultFile.AppendLine(strBuilderResultText.ToString());//add to the file string builder
				}
				else {//approved sale, update CC, add result to file string builder			
					_success++;
					labelCharged.Text=Lan.g(this,"Charged=")+_success;
					CreditCard ccCur=CreditCards.GetOne(PIn.Long(rowCur["CreditCardNum"].ToString()));
					UpdateCreditCardPayConnect(ccCur,payConnectResponse);
					//add to strbuilder that will be written to txt file and to the payment note
					if(PrefC.HasClinicsEnabled && dictClinicNumDesc.ContainsKey(clinicNumCur)) {
						strBuilderResultText.AppendLine("CLINIC="+dictClinicNumDesc[clinicNumCur]);
					}
					strBuilderResultText.AppendLine("RESULT="+payConnectResponse.Status.description);
					strBuilderResultText.AppendLine("TRANS TYPE="+PayConnectService.transType.SALE.ToString());
					strBuilderResultText.AppendLine("AUTH CODE="+payConnectResponse.AuthCode);
					strBuilderResultText.AppendLine("ENTRY=MANUAL");
					strBuilderResultText.AppendLine("CLERK="+Security.CurUser.UserName);
					strBuilderResultText.AppendLine("TRANSACTION NUMBER="+payConnectResponse.RefNumber);
					if(ccCur!=null) {
						strBuilderResultText.AppendLine("ACCOUNT="+ccCur.CCNumberMasked);//XXXXXXXXXXXX1234, all but last four numbers replaced with X's
					}
					if(payConnectResponse.PaymentToken!=null && payConnectResponse.PaymentToken.Expiration!=null) {
						strBuilderResultText.AppendLine("EXPIRATION="+payConnectResponse.PaymentToken.Expiration.month.ToString().PadLeft(2,'0')
							+(payConnectResponse.PaymentToken.Expiration.year%100));
					}
					if(isPayConnectToken) {
						strBuilderResultText.AppendLine("CARD TYPE=PayConnect Token");
					}
					else {
						strBuilderResultText.AppendLine("CARD TYPE="+CreditCardUtils.GetCardType(tokenOrCCMasked));
					}
					strBuilderResultText.AppendLine("AMOUNT="+payConnectRequest.Amount.ToString("F2"));
					amount=(double)payConnectRequest.Amount;
				}
				string receipt=PayConnectUtils.BuildReceiptString(payConnectRequest,payConnectResponse,null,clinicNumCur);
				CreatePayment(patCur,selectedIndex,strBuilderResultText.ToString(),amount,receipt);
				strBuilderResultFile.AppendLine(strBuilderResultText.ToString());
			}
			#endregion Card Charge Loop
			if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ) {
				try {
					Cursor=Cursors.WaitCursor;
					string payConnectResultDir=ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),"PayConnect");
					if(!Directory.Exists(payConnectResultDir)) {
						Directory.CreateDirectory(payConnectResultDir);
					}
					File.WriteAllText(ODFileUtils.CombinePaths(payConnectResultDir,"RecurringChargeResult.txt"),strBuilderResultFile.ToString());
					Cursor=Cursors.Default;
				}
				catch { } //Do nothing cause this is just for internal use.
			}
			else if(CloudStorage.IsCloudStorage) {
				Cursor=Cursors.WaitCursor;
				FormProgress FormP=new FormProgress();
				FormP.DisplayText="Uploading...";
				FormP.NumberFormat="F";
				FormP.NumberMultiplication=1;
				FormP.MaxVal=100;//Doesn't matter what this value is as long as it is greater than 0
				FormP.TickMS=1000;
				OpenDentalCloud.Core.TaskStateUpload state=CloudStorage.UploadAsync(
					CloudStorage.AtoZPath+"/PayConnect"
					,"RecurringChargeResult.txt"
					,Encoding.ASCII.GetBytes(strBuilderResultFile.ToString())
					,new OpenDentalCloud.ProgressHandler(FormP.OnProgress));
				if(FormP.ShowDialog()==DialogResult.Cancel) {
					state.DoCancel=true;
				}
				else {
					//Upload was successful
				}
			}
		}

		private void SendPaySimple() {
			Dictionary<long,string> dictClinicNumDesc=new Dictionary<long,string>();
			if(PrefC.HasClinicsEnabled) {
				dictClinicNumDesc=Clinics.GetClinicsNoCache().ToDictionary(x => x.ClinicNum,x => x.Description);
			}
			dictClinicNumDesc[0]=PrefC.GetString(PrefName.PracticeTitle);
			StringBuilder strBuilderResultFile=new StringBuilder();
			strBuilderResultFile.AppendLine("Recurring charge results for "+DateTime.Now.ToShortDateString()+" ran at "+DateTime.Now.ToShortTimeString());
			strBuilderResultFile.AppendLine();
			#region Card Charge Loop
			//Making a copy now because the user can change the selected index as we are looping through
			List<int> listSelectedIndices=gridMain.SelectedIndices.ToList();
			foreach(int selectedIndex in listSelectedIndices) {
				DataRow rowCur=_listRowsCur[selectedIndex];
				string paySimpleAccountId=rowCur["PaySimpleToken"].ToString();
				int tokenCount=CreditCards.GetTokenCount(paySimpleAccountId,new List<CreditCardSource> { CreditCardSource.PaySimple });
				if(string.IsNullOrWhiteSpace(paySimpleAccountId) || tokenCount!=1) {
					_failed++;
					labelFailed.Text=Lan.g(this,"Failed=")+_failed;
					string msg=(tokenCount>1)?"A duplicate token was found":"A token was not found";
					MessageBox.Show(Lan.g(this,msg+", the card cannot be charged for customer")+": "+rowCur["PatName"].ToString());
					continue;
				}
				long patNum=PIn.Long(rowCur["PatNum"].ToString());
				Patient patCur=Patients.GetPat(patNum);
				if(patCur==null) {
					continue;
				}
				DateTime exp=PIn.Date(rowCur["CCExpiration"].ToString());//We don't have a PaySimpleTokenExpiration, so use the CC's stored one.
				decimal amt=PIn.Decimal(rowCur["ChargeAmt"].ToString());
				string zip=PIn.String(rowCur["Zip"].ToString());
				long clinicNumCur=PIn.Long(rowCur["ClinicNum"].ToString());
				double resultAmt=0;
				Cursor=Cursors.WaitCursor;
				StringBuilder strBuilderResultText=new StringBuilder();//this payment's result text, used in payment note and then appended to file string builder
				strBuilderResultFile.AppendLine("PatNum: "+patNum+" Name: "+patCur.GetNameFLnoPref());
				try {
					PaySimple.ApiResponse response=PaySimple.MakePaymentByToken(new CreditCard() {
						CreditCardNum=PIn.Long(rowCur["CreditCardNum"].ToString()),
						PaySimpleToken=paySimpleAccountId,
						PatNum=patCur.PatNum,
					},amt,clinicNumCur);
					if(response==null) {
						//If this happens, the API method returned successfully and somehow we didn't create a response.
						//The intent of the PaySimple API integration is that we always get a response or throw exceptions.
						throw new ODException(Lan.g(this,"Unknown error making payment.  Please contact support."));
					}
					//approved sale, update CC, add result to file string builder			
					_success++;
					labelCharged.Text=Lan.g(this,"Charged=")+_success;
					CreditCard ccCur=CreditCards.GetOne(PIn.Long(rowCur["CreditCardNum"].ToString()));
					if(ccCur!=null && ccCur.PaySimpleToken!=response.PaySimpleToken) {
						ccCur.PaySimpleToken=response.PaySimpleToken;
						CreditCards.Update(ccCur);
					}
					//add to strbuilder that will be written to txt file and to the payment note
					string clinicDesc="";
					if(PrefC.HasClinicsEnabled && dictClinicNumDesc.ContainsKey(clinicNumCur)) {
						clinicDesc=dictClinicNumDesc[clinicNumCur];
					}
					strBuilderResultText.AppendLine(response.ToNoteString(clinicDesc,"Manual",Security.CurUser.UserName,
						ccCur.CCExpiration.Month.ToString().PadLeft(2,'0')+(ccCur.CCExpiration.Year%100),"PaySimple Token"));
					resultAmt=(double)response.Amount;
					response.BuildReceiptString(ccCur.CCNumberMasked,ccCur.CCExpiration.Month,ccCur.CCExpiration.Year,"",clinicNumCur);
					string receipt=response.TransactionReceipt;
					CreatePayment(patCur,selectedIndex,strBuilderResultText.ToString(),resultAmt,receipt);
				}
				catch(Exception ex) {
					_failed++;
					labelFailed.Text=Lan.g(this,"Failed=")+_failed;
					string clinicDesc="";
					if(PrefC.HasClinicsEnabled && dictClinicNumDesc.ContainsKey(clinicNumCur)) {
						clinicDesc=dictClinicNumDesc[clinicNumCur];
					}
					AddErrorToStrb(strBuilderResultText,ex.Message,clinicDesc);
				}
				finally {
					strBuilderResultFile.AppendLine(strBuilderResultText.ToString());//add to the file string builder
					Cursor=Cursors.Default;
				}
			}
			#endregion Card Charge Loop
			if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ) {
				try {
					Cursor=Cursors.WaitCursor;
					string paySimpleResultDir=ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),"PaySimple");
					if(!Directory.Exists(paySimpleResultDir)) {
						Directory.CreateDirectory(paySimpleResultDir);
					}
					File.WriteAllText(ODFileUtils.CombinePaths(paySimpleResultDir,"RecurringChargeResult.txt"),strBuilderResultFile.ToString());
					Cursor=Cursors.Default;
				}
				catch(Exception ex) {
					ex.DoNothing();//Do nothing cause this is just for internal use.
				}
			}
			else if(CloudStorage.IsCloudStorage) {
				Cursor=Cursors.WaitCursor;
				FormProgress FormP=new FormProgress();
				FormP.DisplayText="Uploading...";
				FormP.NumberFormat="F";
				FormP.NumberMultiplication=1;
				FormP.MaxVal=100;//Doesn't matter what this value is as long as it is greater than 0
				FormP.TickMS=1000;
				OpenDentalCloud.Core.TaskStateUpload state=CloudStorage.UploadAsync(
					CloudStorage.AtoZPath+"/PaySimple"//I feel like this should be ODUtil.CombinePaths but I don't want to have to test it.
					,"RecurringChargeResult.txt"
					,Encoding.ASCII.GetBytes(strBuilderResultFile.ToString())
					,new OpenDentalCloud.ProgressHandler(FormP.OnProgress));
				if(FormP.ShowDialog()==DialogResult.Cancel) {
					state.DoCancel=true;
				}
				else {
					//Upload was successful
				}
			}
		}

		private void AddErrorToStrb(StringBuilder strb,string errorMsg,string clinicDesc) {
			strb.AppendLine(Lan.g(this,"Transaction Type")+": "+PaySimple.TransType.SALE.ToString());
			if(string.IsNullOrWhiteSpace(clinicDesc)) {
				strb.AppendLine("CLINIC="+clinicDesc);
			}
			strb.AppendLine(Lan.g(this,"Error")+": "+errorMsg);
		}

		///<summary>Updates the credit card's masked number and expiration.</summary>
		private void UpdateCreditCardPayConnect(CreditCard ccCur,transResponse payConnectResponse) {
			if(ccCur==null || payConnectResponse==null || payConnectResponse.PaymentToken==null || payConnectResponse.PaymentToken.Expiration==null) {
				return;
			}
			expiration payConnectExp=payConnectResponse.PaymentToken.Expiration;
			//if stored CC token or token expiration are different than those returned by PayConnect, update the stored CC
			if(ccCur.PayConnectToken!=payConnectResponse.PaymentToken.TokenId
				|| ccCur.PayConnectTokenExp.Year!=payConnectExp.year
				|| ccCur.PayConnectTokenExp.Month!=payConnectExp.month) 
			{
				ccCur.PayConnectToken=payConnectResponse.PaymentToken.TokenId;
				ccCur.PayConnectTokenExp=new DateTime(payConnectExp.year,payConnectExp.month,
					DateTime.DaysInMonth(payConnectExp.year,payConnectExp.month));
				ccCur.CCNumberMasked=ccCur.PayConnectToken.Substring(ccCur.PayConnectToken.Length-4).PadLeft(ccCur.PayConnectToken.Length,'X');
				ccCur.CCExpiration=ccCur.PayConnectTokenExp;
				CreditCards.Update(ccCur);
			}
		}

		///<summary>Inserts a payment and paysplit, called after processing a payment through either X-Charge or PayConnect or PaySimple.
		///selectedIndex is the current selected index of the gridMain row this payment is for.</summary>
		private void CreatePayment(Patient patCur,int selectedIndex,string note,double amount,string receipt) {
			DataRow rowCur=_listRowsCur[selectedIndex];
			Payment paymentCur=new Payment();
			paymentCur.DateEntry=_nowDateTime.Date;
			DateTime dateStart=PIn.Date(rowCur["DateStart"].ToString());
			if(PrefC.IsODHQ && PrefC.GetBool(PrefName.BillingUseBillingCycleDay)) {
				int dayOfMonth=Math.Min(DateTime.DaysInMonth(dateStart.Year,dateStart.Month),
					PIn.Int(rowCur["BillingCycleDay"].ToString()));
				dateStart=new DateTime(dateStart.Year,dateStart.Month,dayOfMonth);
			}
			paymentCur.PayDate=GetPayDate(PIn.Date(rowCur["LatestPayment"].ToString()),dateStart);
			paymentCur.RecurringChargeDate=GetRecurringChargeDate(dateStart);
			paymentCur.PatNum=patCur.PatNum;
			//Explicitly set ClinicNum=0, since a pat's ClinicNum will remain set if the user enabled clinics, assigned patients to clinics, and then
			//disabled clinics because we use the ClinicNum to determine which PayConnect or XCharge/XWeb credentials to use for payments.
			paymentCur.ClinicNum=0;
			if(PrefC.HasClinicsEnabled) {
				paymentCur.ClinicNum=PIn.Long(rowCur["ClinicNum"].ToString());
			}
			//ClinicNum can be 0 for 'Headquarters' or clinics not enabled, PayType will be the 0 clinic or headquarters PayType if using PayConnect or PaySimple
			string ppPayTypeDesc="PaymentType";
			if(_progCur.ProgName==ProgramName.PaySimple.ToString()) {
				ppPayTypeDesc=PaySimple.PropertyDescs.PaySimplePayType;
			}
			paymentCur.PayType=PIn.Int(ProgramProperties.GetPropVal(_progCur.ProgramNum,ppPayTypeDesc,paymentCur.ClinicNum));
			paymentCur.PayAmt=amount;
			double payPlanDue=PIn.Double(rowCur["PayPlanDue"].ToString());
			paymentCur.PayNote=note;
			paymentCur.IsRecurringCC=true;
			if(_progCur.ProgName==ProgramName.Xcharge.ToString()) {
				paymentCur.PaymentSource=CreditCardSource.XServer;
			}
			else if(_progCur.ProgName==ProgramName.PayConnect.ToString()) {
				paymentCur.PaymentSource=CreditCardSource.PayConnect;
			}
			else if(_progCur.ProgName==ProgramName.PaySimple.ToString()) {
				paymentCur.PaymentSource=CreditCardSource.PaySimple;
			}
			else {
				paymentCur.PaymentSource=CreditCardSource.None;
			}
			paymentCur.Receipt=receipt;
			Payments.Insert(paymentCur);
			SecurityLogs.MakeLogEntry(Permissions.PaymentCreate,paymentCur.PatNum,Patients.GetLim(paymentCur.PatNum).GetNameLF()+", "
				+paymentCur.PayAmt.ToString("c")+", "+Lans.g(this,"created from the Recurring Charges List"));
			long provNumPayPlan=PIn.Long(rowCur["ProvNum"].ToString());//for payment plans only
			//Regular payments need to apply to the provider that the family owes the most money to.
			//Also get provNum for provider owed the most if the card is for a payplan and for other repeating charges and they will be charged for both
			//the payplan and regular repeating charges
			long provNumRegPmts=0;
			if(provNumPayPlan==0 || paymentCur.PayAmt-payPlanDue>0) {//provNum==0 for cards not attached to a payplan.
				DataTable dt=Patients.GetPaymentStartingBalances(patCur.Guarantor,paymentCur.PayNum);
				double highestAmt=0;
				for(int j=0;j<dt.Rows.Count;j++) {
					double afterIns=PIn.Double(dt.Rows[j]["AfterIns"].ToString());
					if(highestAmt>=afterIns) {
						continue;
					}
					highestAmt=afterIns;
					if(PrefC.GetBool(PrefName.RecurringChargesUsePriProv)) {
						provNumRegPmts=patCur.PriProv;
					}
					else { 
						provNumRegPmts=PIn.Long(dt.Rows[j]["ProvNum"].ToString());
					}
				}
			}
			long splitPatNum=paymentCur.PatNum;
			long patNumPayPlan=PIn.Long(rowCur["PayPlanPatNum"].ToString());//for payment plans only
			if(patNumPayPlan!=0) {//Add the payplan's patnum to the paysplit. 
				splitPatNum=patNumPayPlan;
			}
			PaySplit split=new PaySplit();
			split.PatNum=splitPatNum;
			split.ClinicNum=paymentCur.ClinicNum;
			split.PayNum=paymentCur.PayNum;
			split.DatePay=paymentCur.PayDate;
			split.PayPlanNum=PIn.Long(rowCur["PayPlanNum"].ToString());
			if(split.PayPlanNum==0 || payPlanDue<=0) {//this row is not for a payplan or there is no payplandue
				split.PayPlanNum=0;//if the payplan does not have any amount due, don't attach split to payplan
				split.SplitAmt=paymentCur.PayAmt;
				paymentCur.PayAmt-=split.SplitAmt;
				split.ProvNum=provNumRegPmts;
				split.ClinicNum=patCur.ClinicNum;
			}
			else {//row includes a payplan amount due, could also include a regular repeating pay amount as part of the total charge amount
				split.SplitAmt=Math.Min(payPlanDue,paymentCur.PayAmt);//ensures a split is not more than the actual payment amount
				paymentCur.PayAmt-=split.SplitAmt;//subtract the payplan pay amount from the total payment amount and create another split not attached to payplan
				split.ProvNum=provNumPayPlan;
			}
			PaySplits.Insert(split);
			//if the above split was for a payment plan and there is still some PayAmt left, insert another split not attached to the payplan
			if(paymentCur.PayAmt>0) {
				split=new PaySplit();
				split.PatNum=paymentCur.PatNum;
				split.ClinicNum=patCur.ClinicNum;
				split.PayNum=paymentCur.PayNum;
				split.DatePay=paymentCur.PayDate;
				split.ProvNum=provNumRegPmts;
				split.SplitAmt=paymentCur.PayAmt;
				split.PayPlanNum=0;
				PaySplits.Insert(split);
			}
			//consider moving the aging calls up in the Send methods and building a list of actions to feed into RunParallel to thread them.
			if(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)) {
				Ledgers.ComputeAging(patCur.Guarantor,PrefC.GetDate(PrefName.DateLastAging));
			}
			else {
				Ledgers.ComputeAging(patCur.Guarantor,DateTimeOD.Today);
				if(PrefC.GetDate(PrefName.DateLastAging)!=DateTime.Today) {
					Prefs.UpdateString(PrefName.DateLastAging,POut.Date(DateTime.Today,false));
					//Since this is always called from UI, the above line works fine to keep the prefs cache current.
				}
			}
		}

		///<summary>Will process payments for all authorized charges for each CC stored and marked for recurring charges.
		///X-Charge or PayConnect or PaySimple must be enabled.
		///Program validation done on load and if properties are not valid the form will close and exit.</summary>
		private void butSend_Click(object sender,EventArgs e) {
			RefreshRecurringCharges(false,true);
			if(gridMain.SelectedIndices.Length<1) {
				MsgBox.Show(this,"Must select at least one recurring charge.");
				return;
			}
			if(!PaymentsWithinLockDate()) {
				return;
			}
			//Not checking DatePay for FutureTransactionDate pref violation because these should always have a date <= today
			_success=0;
			_failed=0;
			if(_progCur.ProgName==ProgramName.Xcharge.ToString()) {
				SendXCharge();
			}
			else if(_progCur.ProgName==ProgramName.PayConnect.ToString()) {
				SendPayConnect();
			}
			else if(_progCur.ProgName==ProgramName.PaySimple.ToString()) {
				SendPaySimple();
			}
			try {
				FillGrid(true);
			}
			catch(ObjectDisposedException) {
				//This likely occurred if the user clicked Close before the charges were done processing. Since we don't need to display the grid, we can
				//do nothing.
			}
			labelCharged.Text=Lan.g(this,"Charged=")+_success;
			labelFailed.Text=Lan.g(this,"Failed=")+_failed;
			MsgBox.Show(this,"Done charging cards.\r\nIf there are any patients remaining in list, print the list and handle each one manually.");
		}

		private void butCancel_Click(object sender,EventArgs e) {
			Close();
		}

	}
}