using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormWebSchedASAPHistory:ODForm {
		private List<AsapComms.AsapCommHist> _listAsapCommHists;
		private DateTime _prevDateFrom;
		private DateTime _prevDateTo;
		private List<long> _prevSelectedClinicNums;

		public FormWebSchedASAPHistory() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormWebSchedASAPHistory_Load(object sender,EventArgs e) {
			datePicker.SetDateTimeFrom(DateTime.Today.AddDays(-7));
			datePicker.SetDateTimeTo(DateTime.Today);
			if(!PrefC.HasClinicsEnabled) {
				labelClinic.Visible=false;
			}
			FillGrid();
		}

		private void GetData() {
			Cursor=Cursors.WaitCursor;
			List<long> listClinicNums=null;
			if(PrefC.HasClinicsEnabled) {
				listClinicNums=comboClinic.ListSelectedClinicNums;
			}
			_listAsapCommHists=AsapComms.GetHist(datePicker.GetDateTimeFrom(),datePicker.GetDateTimeTo(),listClinicNums:listClinicNums);
			_prevDateFrom=datePicker.GetDateTimeFrom();
			_prevDateTo=datePicker.GetDateTimeTo();
			_prevSelectedClinicNums=new List<long>(comboClinic.ListSelectedClinicNums);
			Cursor=Cursors.Default;
		}

		private void FillGrid() {
			if(_listAsapCommHists==null || datePicker.GetDateTimeFrom() < _prevDateFrom || datePicker.GetDateTimeTo() > _prevDateTo
				|| comboClinic.ListSelectedClinicNums.Any(x => !x.In(_prevSelectedClinicNums))) 
			{
				//The user is asking for data that we have not fetched yet.
				GetData();
			}
			bool isClinicsEnabled=PrefC.HasClinicsEnabled;
			List<AsapComms.AsapCommHist> listHist=_listAsapCommHists.Where(x => x.AsapComm.DateTimeEntry
				.Between(datePicker.GetDateTimeFrom(),datePicker.GetDateTimeTo()))
				.Where(x => !isClinicsEnabled || x.AsapComm.ClinicNum.In(comboClinic.ListSelectedClinicNums)).ToList();
			gridHistory.BeginUpdate();
			gridHistory.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"Patient"),120);
			gridHistory.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Status"),120);
			gridHistory.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"SMS Send Time"),140);
			gridHistory.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Email Send Time"),140);
			gridHistory.Columns.Add(col);
			if(PrefC.HasClinicsEnabled) {
				col=new ODGridColumn(Lan.g(this,"Clinic"),120);
				gridHistory.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g(this,"Original Appt Time"),140);
			gridHistory.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Slot Start"),140);
			gridHistory.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Slot Stop"),140);
			gridHistory.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Date Entry"),140);
			gridHistory.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"SMS Message Text"),250);
			gridHistory.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Email Message Text"),250);
			gridHistory.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Note"),250);
			gridHistory.Columns.Add(col);
			gridHistory.Rows.Clear();
			foreach(AsapComms.AsapCommHist asapHist in listHist) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(asapHist.PatientName);
				row.Cells.Add(Lan.g(this,asapHist.AsapComm.ResponseStatus.GetDescription()));
				string smsSent;
				if(asapHist.AsapComm.SmsSendStatus==AutoCommStatus.SendSuccessful) {
					smsSent=asapHist.AsapComm.DateTimeSmsSent.ToString();
				}
				else if(asapHist.AsapComm.SmsSendStatus==AutoCommStatus.SendNotAttempted) {
					smsSent=asapHist.AsapComm.DateTimeSmsScheduled.ToString();
				}
				else {
					smsSent="";
				}
				row.Cells.Add(smsSent);
				string emailSent;
				if(asapHist.AsapComm.EmailSendStatus==AutoCommStatus.SendSuccessful) {
					emailSent=asapHist.AsapComm.DateTimeEmailSent.ToString();
				}
				else {
					emailSent="";
				}
				row.Cells.Add(emailSent);
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(ODMethodsT.Coalesce(Clinics.GetClinic(asapHist.AsapComm.ClinicNum)).Abbr);
				}
				row.Cells.Add(asapHist.AsapComm.DateTimeOrig.Year > 1880 ? asapHist.AsapComm.DateTimeOrig.ToString() : "");
				row.Cells.Add(asapHist.DateTimeSlotStart.ToString());
				row.Cells.Add(asapHist.DateTimeSlotEnd.ToString());
				row.Cells.Add(asapHist.AsapComm.DateTimeEntry.ToString());
				row.Cells.Add(asapHist.SMSMessageText);
				row.Cells.Add(asapHist.EmailMessageText);
				row.Cells.Add(asapHist.AsapComm.Note);
				row.Tag=asapHist;
				gridHistory.Rows.Add(row);
			}
			gridHistory.EndUpdate();
			textFilled.Text=listHist.Select(x => x.AsapComm)
				.Where(x => x.ResponseStatus==AsapRSVPStatus.AcceptedAndMoved)
				.DistinctBy(x => x.FKey).Count().ToString();
			textTextsSent.Text=listHist.Select(x => x.AsapComm)
				.Where(x => x.SmsSendStatus==AutoCommStatus.SendSuccessful)
				.DistinctBy(x => x.GuidMessageToMobile).Count().ToString();
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void datePicker_Leave(object sender,EventArgs e) {
			FillGrid();
		}

		private void datePicker_CalendarClosed(object sender,EventArgs e) {
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

	}
}