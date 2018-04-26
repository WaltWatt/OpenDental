using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormCreditRecurringDateChoose:ODForm {
		private CreditCard _creditCardCur;
		private DateTime _lastMonth;
		private DateTime _thisMonth;
		public DateTime PayDate;
		private Patient _pat;

		public FormCreditRecurringDateChoose(CreditCard creditCard,Patient pat) {
			InitializeComponent();
			_creditCardCur=creditCard;
			_pat=pat;
			_lastMonth=GetValidPayDate(DateTime.Now.AddMonths(-1));
			_thisMonth=GetValidPayDate(DateTime.Now);
			Lan.F(this);
		}

		private void FormCreditRecurringDateChoose_Load(object sender,EventArgs e) {
			if(PrefC.GetBool(PrefName.RecurringChargesUseTransDate)) {
				labelLastMonth.Text=Lan.g(this,"Recurring charge date will be:")+" "+_lastMonth.ToShortDateString();
				labelThisMonth.Text=Lan.g(this,"Recurring charge date will be:")+" "+_thisMonth.ToShortDateString();
			}
			else {
				labelLastMonth.Text+=" "+_lastMonth.ToShortDateString();
				labelThisMonth.Text+=" "+_thisMonth.ToShortDateString();
			}
			//If the recurring pay date is in the future do not let them choose that option.
			if(_thisMonth>DateTime.Now) {
				butThisMonth.Enabled=false;
				labelThisMonth.Text=Lan.g(this,"Cannot make payment for future date:")+" "+_thisMonth.ToShortDateString();
			}
		}

		///<summary>Returns a valid date based on the Month and Year taken from the date passed in and the Day that is set for the recurring charges.</summary>
		private DateTime GetValidPayDate(DateTime date) {
			int dayOfMonth;
			if(PrefC.IsODHQ && PrefC.GetBool(PrefName.BillingUseBillingCycleDay)) {
				dayOfMonth=_pat.BillingCycleDay;
			}
			else {
				dayOfMonth=_creditCardCur.DateStart.Day;
			}
			DateTime newDate;
			try {
				newDate=new DateTime(date.Year,date.Month,dayOfMonth);
			}
			catch {//Not a valid date, so use the max day in that month.
				newDate=new DateTime(date.Year,date.Month,DateTime.DaysInMonth(date.Year,date.Month));
			}
			return newDate;
		}

		private void butLastMonth_Click(object sender,EventArgs e) {
			PayDate=_lastMonth;
			DialogResult=DialogResult.OK;
		}

		private void butThisMonth_Click(object sender,EventArgs e) {
			PayDate=_thisMonth;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}