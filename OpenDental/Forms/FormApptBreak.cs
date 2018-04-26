using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using System.Linq;

namespace OpenDental {
	public partial class FormApptBreak:ODForm {
		
		public ApptBreakSelection FormApptBreakSelection;
		public ProcedureCode SelectedProcCode;
		private readonly Appointment _appt;

		public FormApptBreak(Appointment appt) {
			InitializeComponent();
			Lan.F(this);
			_appt=appt;
		}

		private void FormApptBreak_Load(object sender,EventArgs e) {
			BrokenApptProcedure brokenApptProcs=(BrokenApptProcedure)PrefC.GetInt(PrefName.BrokenApptProcedure);
			radioMissed.Enabled=brokenApptProcs.In(BrokenApptProcedure.Missed,BrokenApptProcedure.Both);
			radioCancelled.Enabled=brokenApptProcs.In(BrokenApptProcedure.Cancelled,BrokenApptProcedure.Both);
			if(radioMissed.Enabled && !radioCancelled.Enabled) {
				radioMissed.Checked=true;
			}
			else if(!radioMissed.Enabled && radioCancelled.Enabled) {
				radioMissed.Checked=true;
			}
		}

		private bool ValidateSelection() {
			if(!radioMissed.Checked && !radioCancelled.Checked) {
				MsgBox.Show(this,"Please select a broken procedure type.");
				return false;
			}
			return true;
		}

		private void PromptTextASAPList() {
			if(!PrefC.GetBool(PrefName.WebSchedAsapEnabled) || Appointments.RefreshASAP(0,0,_appt.ClinicNum,new List<ApptStatus>()).Count==0
				|| !MsgBox.Show("Appointment",MsgBoxButtons.YesNo,"Text patients on the ASAP List and offer them this opening?")) 
			{
				return;
			}
			DateTime slotStart=AppointmentL.DateSelected.Date;//Midnight
			DateTime slotEnd=AppointmentL.DateSelected.Date.AddDays(1);//Midnight tomorrow
			//Loop through all other appts in the op to find a slot that will not overlap.
			List<Appointment> listApptsInOp=Appointments.GetAppointmentsForOpsByPeriod(new List<long> { _appt.Op },_appt.AptDateTime);
			foreach(Appointment otherAppt in listApptsInOp.Where(x => x.AptNum!=_appt.AptNum)) {
				DateTime dateEndApt=otherAppt.AptDateTime.AddMinutes(otherAppt.Pattern.Length*5);
				if(dateEndApt.Between(slotStart,_appt.AptDateTime)) {
					slotStart=dateEndApt;
				}
				if(otherAppt.AptDateTime.Between(_appt.AptDateTime,slotEnd)) {
					slotEnd=otherAppt.AptDateTime;
				}
			}
			slotStart=ODMathLib.Max(slotStart,_appt.AptDateTime.AddHours(-1));
			slotEnd=ODMathLib.Min(slotEnd,_appt.AptDateTime.AddHours(3));
			FormASAP formASAP=new FormASAP(_appt.AptDateTime,slotStart,slotEnd,_appt.Op);
			formASAP.ShowDialog();
		}

		private void butUnsched_Click(object sender,EventArgs e) {;
			if(!ValidateSelection()) {
				return;
			}
			PromptTextASAPList();
			FormApptBreakSelection=ApptBreakSelection.Unsched;
			DialogResult=DialogResult.OK;
		}

		private void butPinboard_Click(object sender,EventArgs e) {
			if(!ValidateSelection()) {
				return;
			}
			PromptTextASAPList();
			FormApptBreakSelection=ApptBreakSelection.Pinboard;
			DialogResult=DialogResult.OK;
		}

		private void butApptBook_Click(object sender,EventArgs e) {
			if(!ValidateSelection()) {
				return;
			}
			FormApptBreakSelection=ApptBreakSelection.ApptBook;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			FormApptBreakSelection=ApptBreakSelection.None;
			DialogResult=DialogResult.Cancel;
		}

		private void FormApptBreak_FormClosing(object sender,FormClosingEventArgs e) {
			if(this.DialogResult!=DialogResult.OK) {
				return;
			}
			SelectedProcCode=radioMissed.Checked?ProcedureCodes.GetProcCode("D9986"):ProcedureCodes.GetProcCode("D9987");
		}
	}

	public enum ApptBreakSelection {
		///<summary>0 - Default.</summary>
		None,
		///<summary>1</summary>
		Unsched,
		///<summary>2</summary>
		Pinboard,
		///<summary>3</summary>
		ApptBook
	}

}