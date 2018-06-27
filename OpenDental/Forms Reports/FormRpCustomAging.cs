using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using System.Linq;
using OpenDental.UI;
using System.Drawing.Printing;
using OpenDental.ReportingComplex;

namespace OpenDental {
	public partial class FormRpCustomAging:ODForm {
		private AgingOptions _agingOptions;

		public FormRpCustomAging() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormRpCustomAging_Load(object sender,EventArgs e) {
			textDate.Text=DateTimeOD.Today.ToShortDateString();
			if(PrefC.GetBool(PrefName.ReportsPPOwriteoffDefaultToProcDate)) {
				radioWriteoffProcDate.Checked=true;
			}
			else {
				radioWriteoffInsPayDate.Checked=true;
			}
			if(PrefC.GetInt(PrefName.PayPlansVersion) == (int)PayPlanVersions.AgeCreditsAndDebits) {
				checkAgePayPlanCharges.Checked=true;
				checkAgePayPlanCredits.Checked=true;
			}
			else {//both Traditional and AgedCreditsOnly only age credits.
				checkAgePayPlanCharges.Checked=false;
				checkAgePayPlanCredits.Checked=true;
			}
			FillClinics();
			FillProvs();
			FillBillType();
		}

		private void FillBillType() {
			listBoxBillTypes.Items.Clear();
			List<Def> listBillTypes = Defs.GetDefsForCategory(DefCat.BillingTypes,true);
			for(int i = 0;i < listBillTypes.Count;i++) {
				listBoxBillTypes.Items.Add(new ODBoxItem<Def>(listBillTypes[i].ItemName,listBillTypes[i]));
				listBoxBillTypes.SetSelected(i,true);
			}
		}

		private void FillProvs() {
			listBoxProvs.Items.Clear();
			List<Provider> listProvs = Providers.GetListReports();
			for(int i = 0;i < listProvs.Count;i++) {
				listBoxProvs.Items.Add(new ODBoxItem<Provider>(listProvs[i].Abbr,listProvs[i]));
				listBoxProvs.SetSelected(i,true);
			}
		}

		private void FillClinics() {
			if(!PrefC.HasClinicsEnabled) {
				listBoxClins.Visible=false;
				labelClinic.Visible=false;
				checkAllClin.Visible=false;
				return;
			}
			listBoxClins.Items.Clear();
			List<Clinic> listClinics=Clinics.GetForUserod(Security.CurUser,true);
			for(int i=0;i<listClinics.Count;i++) {
				listBoxClins.Items.Add(new ODBoxItem<Clinic>(listClinics[i].Abbr,listClinics[i]));
				listBoxClins.SetSelected(i,true);
			}
		}

		private void FillGrid(AgingOptions agingOptions,List<AgingPat> listAgingPats) {
			gridMain.BeginUpdate();
			gridTotals.BeginUpdate();
			gridMain.Columns.Clear();
			gridTotals.Columns.Clear();
			ODGridColumn col = new ODGridColumn("Patient",200,GridSortingStrategy.StringCompare);
			gridMain.Columns.Add(col);
			gridTotals.Columns.Add(col);
			col = new ODGridColumn("0-30 Days",100,GridSortingStrategy.AmountParse);
			gridMain.Columns.Add(col);
			gridTotals.Columns.Add(col);
			col = new ODGridColumn("31-60 Days",100,GridSortingStrategy.AmountParse);
			gridMain.Columns.Add(col);
			gridTotals.Columns.Add(col);
			col = new ODGridColumn("61-90 Days",100,GridSortingStrategy.AmountParse);
			gridMain.Columns.Add(col);
			gridTotals.Columns.Add(col);
			col = new ODGridColumn("> 90 Days",100,GridSortingStrategy.AmountParse);
			gridMain.Columns.Add(col);
			gridTotals.Columns.Add(col);
			col = new ODGridColumn("Total",100,GridSortingStrategy.AmountParse);
			gridMain.Columns.Add(col);
			gridTotals.Columns.Add(col);
			double totalZeroThirty = 0;
			double totalThirtySixty = 0;
			double totalSixtyNinety = 0;
			double totalOverNinety = 0;
			double totalBalTotal = 0;
			gridMain.Rows.Clear();
			foreach(AgingPat agingPatCur in listAgingPats) {
				ODGridRow row = new ODGridRow();
				string patName = agingPatCur.Pat.LName + ", " + agingPatCur.Pat.FName;
				row.Cells.Add(patName);
				totalZeroThirty +=agingPatCur.BalZeroThirty;
				row.Cells.Add(agingPatCur.BalZeroThirty.ToString("f"));
				totalThirtySixty+=agingPatCur.BalThirtySixty;
				row.Cells.Add(agingPatCur.BalThirtySixty.ToString("f"));
				totalSixtyNinety+=agingPatCur.BalSixtyNinety;
				row.Cells.Add(agingPatCur.BalSixtyNinety.ToString("f"));
				totalOverNinety+=agingPatCur.BalOverNinety;
				row.Cells.Add(agingPatCur.BalOverNinety.ToString("f"));
				totalBalTotal+=agingPatCur.BalTotal;
				row.Cells.Add(agingPatCur.BalTotal.ToString("f"));
				row.Tag=agingPatCur;
				gridMain.Rows.Add(row);
			}
			gridTotals.Rows.Clear();
			gridTotals.Rows.Add(new ODGridRow("Totals:",totalZeroThirty.ToString("f"),totalThirtySixty.ToString("f")
				,totalSixtyNinety.ToString("f"),totalOverNinety.ToString("f"),totalBalTotal.ToString("f")));
			gridMain.EndUpdate();
			gridTotals.EndUpdate();
		}

		#region Helper Methods
		private AgingOptions.AgingInclude GetAgingIncludes() {
			AgingOptions.AgingInclude agingInclude = AgingOptions.AgingInclude.None;
			if(checkAgeProcedures.Checked) {
				agingInclude |= AgingOptions.AgingInclude.ProcedureFees;
			}
			if(checkAgeAdjustments.Checked) {
				agingInclude |= AgingOptions.AgingInclude.Adjustments;
			}
			if(checkAgePayPlanCharges.Checked) {
				agingInclude |= AgingOptions.AgingInclude.PayPlanCharges;
			}
			if(checkAgePayPlanCredits.Checked) {
				agingInclude |= AgingOptions.AgingInclude.PayPlanCredits;
			}
			if(checkAgePatPayments.Checked) {
				agingInclude |= AgingOptions.AgingInclude.PatPayments;
			}
			if(checkAgeInsPayments.Checked) {
				agingInclude |= AgingOptions.AgingInclude.InsPayments;
			}
			if(checkAgeInsEsts.Checked) {
				agingInclude |= AgingOptions.AgingInclude.InsEsts;
			}
			if(checkAgeWriteoffs.Checked) {
				agingInclude |= AgingOptions.AgingInclude.Writeoffs;
			}
			if(checkAgeWriteoffEsts.Checked) {
				agingInclude |= AgingOptions.AgingInclude.WriteoffEsts;
			}
			return agingInclude;
		}

		private AgingOptions.WriteoffAgingOptions GetWriteoffOptions() {
			if(radioWriteoffInsPayDate.Checked) {
				return AgingOptions.WriteoffAgingOptions.InsPayDate;
			}
			else {
				return AgingOptions.WriteoffAgingOptions.ProcDate;
			}
		}

		private AgingOptions.FamilyGrouping GetFamilyGrouping() {
			if(radioGroupByFam.Checked) {
				return AgingOptions.FamilyGrouping.Family;
			}
			else {
				return AgingOptions.FamilyGrouping.Individual;
			}
		}

		private AgeOfAccount GetAgeOfAccount() {
			if(radio30.Checked) {
				return AgeOfAccount.Over30;
			}
			else if(radio60.Checked) {
				return AgeOfAccount.Over60;
			}
			else if(radio90.Checked) {
				return AgeOfAccount.Over90;
			}
			else {
				return AgeOfAccount.Any;
			}
		}

		private AgingOptions.NegativeBalAgingOptions GetNegativeBalOptions() {
			if(radioIncludeNeg.Checked) {
				return AgingOptions.NegativeBalAgingOptions.Include;
			}
			else if(radioShowOnlyNeg.Checked) {
				return AgingOptions.NegativeBalAgingOptions.ShowOnly;
			}
			else {
				return AgingOptions.NegativeBalAgingOptions.Exclude;
			}
		}
		#endregion

		private void butRun_Click(object sender,EventArgs e) {
			//refresh aging list
			//refill the grid
			_agingOptions = new AgingOptions {
				DateAsOf = PIn.Date(textDate.Text),
				AgingInc = GetAgingIncludes(),
				WriteoffOptions = GetWriteoffOptions(),
				FamGroup = GetFamilyGrouping(),
				AgeAccount = GetAgeOfAccount(),
				NegativeBalOptions = GetNegativeBalOptions(),
				ExcludeInactive = checkExcludeInactive.Checked,
				ExcludeArchive=checkExcludeArchive.Checked,
				ExcludeBadAddress = checkExcludeBadAddresses.Checked,
				//pass in null for lists to not limit by them.
				ListProvs = checkAllProv.Checked ? null : listBoxProvs.SelectedItems.OfType<ODBoxItem<Provider>>().Select(x => x.Tag).ToList(),
				ListClins = checkAllClin.Checked ? null : listBoxClins.SelectedItems.OfType<ODBoxItem<Clinic>>().Select(x => x.Tag).ToList(),
				ListBillTypes = checkAllBillType.Checked ? null : listBoxBillTypes.SelectedItems.OfType<ODBoxItem<Def>>().Select(x => x.Tag).ToList(),
				AgeCredits = checkAgeCredits.Checked,
			};
			if(_agingOptions.AgingInc == (AgingOptions.AgingInclude.None)) {
				MsgBox.Show(this,"You must select at least one transaction type to include.");
				return;
			}
			FillGrid(_agingOptions,RpCustomAging.GetAgingList(_agingOptions));
		}

		private void listBoxBillTypes_Click(object sender,EventArgs e) {
			checkAllBillType.Checked=false;
		}

		private void listBoxProvs_Click(object sender,EventArgs e) {
			checkAllProv.Checked=false;
		}

		private void listBoxClins_Click(object sender,EventArgs e) {
			checkAllClin.Checked=false;
		}

		private void goToAccountToolStripMenuItem_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.AccountModule)) {
				return;
			}
			if(gridMain.SelectedGridRows.Count==0) {
				MsgBox.Show(this,"Please select a patient first.");
				return;
			}
			GotoModule.GotoAccount(((AgingPat)gridMain.SelectedGridRows[0].Tag).Pat.PatNum);
		}

		private void butPrint_Click(object sender,EventArgs e) {
			if(gridMain.Rows.Count==0) {
				MsgBox.Show(this,"The report has no results to show. Please click 'Run' to populate the grid first.");
				return;
			}
			DataTable tableReportCur=new DataTable();
			tableReportCur.Columns.Add("Patient");
			tableReportCur.Columns.Add("0_30");
			tableReportCur.Columns.Add("31_60");
			tableReportCur.Columns.Add("61_90");
			tableReportCur.Columns.Add("Over90");
			tableReportCur.Columns.Add("BatTotal");
			//Uses the grid's rows so that the user's row sorting selection is retained.
			foreach(AgingPat agingPatCur in gridMain.Rows.Select(x => (AgingPat)x.Tag)) {
				DataRow row = tableReportCur.NewRow();
				row["Patient"] = agingPatCur.Pat.GetNameLF();
				row["0_30"] = agingPatCur.BalZeroThirty.ToString("f");
				row["31_60"] = agingPatCur.BalThirtySixty.ToString("f");
				row["61_90"] = agingPatCur.BalSixtyNinety.ToString("f");
				row["Over90"] = agingPatCur.BalOverNinety.ToString("f");
				row["BatTotal"] = agingPatCur.BalTotal.ToString("f");
				tableReportCur.Rows.Add(row);
			}
			ReportComplex report=new ReportComplex(true,false);
			report.ReportName=Lan.g(this,"Custom Aging of Accounts Receivable");
			report.AddTitle("Custom Aging Report",Lan.g(this,"Custom Aging of Accounts Receivable"));
			report.AddSubTitle("PracTitle",PrefC.GetString(PrefName.PracticeTitle));
			report.AddSubTitle("AsOf",Lan.g(this,"As of ")+_agingOptions.DateAsOf.ToShortDateString());
			List<string> listAgingInc=new List<string>();
			//Go through every aging option and for every one that is selected, add the descriptions as a subtitle
			foreach(AgingOptions.AgingInclude agingInc in Enum.GetValues(typeof(AgingOptions.AgingInclude))) {
				if(agingInc==AgingOptions.AgingInclude.None) {
					continue;
				}
				if(_agingOptions.AgingInc.HasFlag(agingInc)) {
					listAgingInc.Add(Lan.g(this,agingInc.GetDescription()));
				}
			}
			//Add a newline to the list if it's too long.
			if(listAgingInc.Count>5) {
				listAgingInc[(listAgingInc.Count+1)/2]="\r\n"+listAgingInc[(listAgingInc.Count+1)/2];
			}
			report.AddSubTitle("AgeInc",Lan.g(this,"For")+": "+string.Join(", ",listAgingInc));
			if(_agingOptions.AgeAccount==AgeOfAccount.Any) {
				report.AddSubTitle("Balance",Lan.g(this,"Any Balance"));
			}
			else if(_agingOptions.AgeAccount==AgeOfAccount.Over30) {
				report.AddSubTitle("Over30",Lan.g(this,"Over 30 Days"));
			}
			else if(_agingOptions.AgeAccount==AgeOfAccount.Over60) {
				report.AddSubTitle("Over60",Lan.g(this,"Over 60 Days"));
			}
			else if(_agingOptions.AgeAccount==AgeOfAccount.Over90) {
				report.AddSubTitle("Over90",Lan.g(this,"Over 90 Days"));
			}
			if(_agingOptions.ListBillTypes==null) {
				report.AddSubTitle("BillingTypes",Lan.g(this,"All Billing Types"));
			}
			else {
				report.AddSubTitle("BillingTypes",string.Join(", ",_agingOptions.ListBillTypes.Select(x => x.ItemName)));
			}
			if(_agingOptions.ListProvs==null) {
				report.AddSubTitle("Providers",Lan.g(this,"All Providers"));
			}
			else {
				report.AddSubTitle("Providers",string.Join(", ",_agingOptions.ListProvs.Select(x => x.Abbr)));
			}
			if(_agingOptions.ListClins==null) {
				report.AddSubTitle("Clinics",Lan.g(this,"All Clinics"));
			}
			else {
				report.AddSubTitle("Clinics",string.Join(", ",_agingOptions.ListClins.Select(x => x.Abbr)));
			}
			QueryObject query=report.AddQuery(tableReportCur,"Date "+DateTimeOD.Today.ToShortDateString());
			query.AddColumn((_agingOptions.FamGroup == AgingOptions.FamilyGrouping.Family ? "GUARANTOR" : "PATIENT"),160,FieldValueType.String);
			query.AddColumn("0-30 DAYS",75,FieldValueType.Number);
			query.AddColumn("31-60 DAYS",75,FieldValueType.Number);
			query.AddColumn("61-90 DAYS",75,FieldValueType.Number);
			query.AddColumn("> 90 DAYS",75,FieldValueType.Number);
			query.AddColumn("TOTAL",80,FieldValueType.Number);
			report.AddPageNum();
			report.AddGridLines();
			if(!report.SubmitQueries()) {
				return;
			}
			FormReportComplex FormR=new FormReportComplex(report);
			FormR.ShowDialog();
		}
		
		private void butCancel_Click(object sender,EventArgs e) {
			Close();
		}
	}

}