using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class AppointmentT {

		///<summary></summary>
		public static Appointment CreateAppointment(long patNum,DateTime aptDateTime,long opNum,long provNum,long provHyg=0,string pattern="//XXXX//"
			,long clinicNum=0,bool isHygiene=false,ApptStatus aptStatus=ApptStatus.Scheduled,ApptPriority priority=ApptPriority.Normal)
		{
			Appointment appointment=new Appointment();
			appointment.AptDateTime=aptDateTime;
			appointment.AptStatus=aptStatus;
			appointment.ClinicNum=clinicNum;
			appointment.IsHygiene=isHygiene;
			appointment.Op=opNum;
			appointment.PatNum=patNum;
			appointment.Pattern=pattern;
			appointment.ProvNum=provNum;
			appointment.ProvHyg=provHyg;
			appointment.Priority=priority;
			Appointments.Insert(appointment);
			return appointment;
		}

		///<summary>Deletes everything from the appointment table.  Does not truncate the table so that PKs are not reused on accident.</summary>
		public static void ClearAppointmentTable() {
			string command="DELETE FROM appointment WHERE AptNum > 0";
			DataCore.NonQ(command);
		}
	}
}
