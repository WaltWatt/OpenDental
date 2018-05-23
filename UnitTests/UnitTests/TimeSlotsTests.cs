using CodeBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDentBusiness;
using OpenDentBusiness.WebTypes.WebSched.TimeSlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnitTestsCore;

namespace UnitTests.UnitTests {
	[TestClass]
	public class TimeSlotsTests:TestBase {

		///<summary>Web Sched - Clinic priority.  Time slots should be found only for operatories for the patients clinic.
		///This unit test used to be called "Legacy_TestSixtyEight" for historical purposes.</summary>
		[TestMethod]
		public void TimeSlots_GetAvailableWebSchedTimeSlots_ClinicPriority() {
			//if(specificTest!=0 && specificTest!=68) {
			//	return "";
			//}
			string suffix="68";
			//Start with fresh tables so that our time slots are extremely predictable.
			RecallTypeT.ClearRecallTypeTable();
			RecallT.ClearRecallTable();
			OperatoryT.ClearOperatoryTable();
			ScheduleT.ClearScheduleTable();
			ScheduleOpT.ClearScheduleOpTable();
			//Turn clinics ON!
			Prefs.UpdateBool(PrefName.EasyNoClinics,false);//Not no clinics.
			long clinicNum1=ClinicT.CreateClinic("1 - "+suffix).ClinicNum;
			long clinicNum2=ClinicT.CreateClinic("2 - "+suffix).ClinicNum;
			//Create a date that will always be in the future.  This date will be used for schedules and recalls.
			DateTime dateTimeSchedule=DateTime.Now.AddYears(1);
			long provNumDoc=ProviderT.CreateProvider("Doc-"+suffix);
			long provNumHyg=ProviderT.CreateProvider("Hyg-"+suffix,isSecondary:true);
			//Create the patient and have them associated to the second clinic.
			Patient pat=PatientT.CreatePatient(suffix,provNumDoc,clinicNum2);
			//Make sure the that Appointment View time increment is set to 10 min.
			Prefs.UpdateInt(PrefName.AppointmentTimeIncrement,10);
			Def defLunchBlockout=DefinitionT.CreateDefinition(DefCat.BlockoutTypes,"Lunch-"+suffix,itemColor:System.Drawing.Color.Azure);
			//Create a psudo prophy recall type that lasts 40 mins and has an interval of every 6 months and 1 day.
			RecallType recallType=RecallTypeT.CreateRecallType("Prophy-"+suffix,"D1110,D1330","//X/",new Interval(1,0,6,0));
			//Create a recall for our patient.
			Recall recall=RecallT.CreateRecall(pat.PatNum,recallType.RecallTypeNum,dateTimeSchedule,new Interval(1,0,6,0));
			//Create operatories for the providers but make the each op assigned to a different clinic.  Hyg will be assigned to clinicNum2.
			Operatory opDoc=OperatoryT.CreateOperatory("1-"+suffix,"Doc Op - "+suffix,provNumDoc,provNumHyg,clinicNum1,isWebSched:true,itemOrder:0);
			Operatory opHyg=OperatoryT.CreateOperatory("2-"+suffix,"Hyg Op - "+suffix,provNumDoc,provNumHyg,clinicNum2,isWebSched:true,itemOrder:1);
			//Create a schedule for the doctor from 09:00 - 11:30 with a 30 min break and then back to work from 12:00 - 17:00
			Schedule schedDocMorning=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,9,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,11,30,0).Ticks),schedType:ScheduleType.Provider,provNum:provNumDoc);
			//Create a blockout for lunch because why not.
			Schedule schedDocLunch=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,11,30,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,12,0,0).Ticks),schedType:ScheduleType.Blockout,blockoutType:defLunchBlockout.DefNum);
			//Schedule for closing from 12:00 - 17:00
			Schedule schedDocEvening=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,12,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,17,0,0).Ticks),schedType:ScheduleType.Provider,provNum: provNumDoc);
			//Now link up the schedule entries to the Web Sched operatory
			ScheduleOpT.CreateScheduleOp(opDoc.OperatoryNum,schedDocMorning.ScheduleNum);
			ScheduleOpT.CreateScheduleOp(opDoc.OperatoryNum,schedDocLunch.ScheduleNum);
			ScheduleOpT.CreateScheduleOp(opDoc.OperatoryNum,schedDocEvening.ScheduleNum);
			//Create a crazy schedule for the clinicNum2 operatory which should be the time sltos that get returned.
			//02:00 - 04:00 with a 15 hour break and then back to work from 19:00 - 23:20
			Schedule schedDocMorning2=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,2,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,4,0,0).Ticks),schedType:ScheduleType.Provider,provNum:provNumDoc);
			//Create a European length lunch.
			Schedule schedDocLunch2=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,4,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,19,0,0).Ticks),schedType:ScheduleType.Blockout,blockoutType:defLunchBlockout.DefNum);
			//Schedule for closing from 19:00 - 23:20
			Schedule schedDocEvening2=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,19,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,23,20,0).Ticks),schedType:ScheduleType.Provider,provNum:provNumDoc);
			//Link the crazy schedule up to the non-Web Sched operatory.
			ScheduleOpT.CreateScheduleOp(opHyg.OperatoryNum,schedDocMorning2.ScheduleNum);
			ScheduleOpT.CreateScheduleOp(opHyg.OperatoryNum,schedDocLunch2.ScheduleNum);
			ScheduleOpT.CreateScheduleOp(opHyg.OperatoryNum,schedDocEvening2.ScheduleNum);
			//The open time slots returned should all return for the Web Sched operatory and not the other one.
			List<TimeSlot> listTimeSlots=TimeSlots.GetAvailableWebSchedTimeSlots(recall.RecallNum,dateTimeSchedule,dateTimeSchedule.AddDays(30));
			//There should be 10 time slots returned that span from 09:00 - 16:40.
			//The 11:00 - 12:00 hour should be open (can't fit 40 min appt over lunch break).
			if(listTimeSlots.Count!=9) {
				throw new Exception("Incorrect number of time slots returned.  Expected 9, received "+listTimeSlots.Count+".");
			}
			if(listTimeSlots.Any(x => x.OperatoryNum!=opHyg.OperatoryNum)) {
				throw new Exception("Invalid operatory time slot returned.  Expected all time slots to be in operatory #"+opHyg.OperatoryNum);
			}
			//Just check 4 specific time slots.  Don't worry about checking all of them cause that is a little overkill.
			//First slot
			//if(listTimeSlots[0].DateTimeStart.Date!=dateTimeSchedule.Date
			//	|| listTimeSlots[0].DateTimeStart!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,2,0,0)
			//	|| listTimeSlots[0].DateTimeStop!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,2,40,0)
			//	|| listTimeSlots[0].OperatoryNum!=opHyg.OperatoryNum) 
			//{
			//	throw new Exception("Invalid first time slot returned.  See test for expected values.\r\n");
			//}
			Assert.IsFalse(listTimeSlots[0].DateTimeStart.Date!=dateTimeSchedule.Date
				|| listTimeSlots[0].DateTimeStart!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,2,0,0)
				|| listTimeSlots[0].DateTimeStop!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,2,40,0)
				|| listTimeSlots[0].OperatoryNum!=opHyg.OperatoryNum);
			//Slot @ 03:20 - 04:00
			//if(listTimeSlots[2].DateTimeStart.Date!=dateTimeSchedule.Date
			//	|| listTimeSlots[2].DateTimeStart!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,3,20,0)
			//	|| listTimeSlots[2].DateTimeStop!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,4,0,0)
			//	|| listTimeSlots[2].OperatoryNum!=opHyg.OperatoryNum) 
			//{
			//	throw new Exception("Invalid pre-lunch time slot returned.  See test for expected values.\r\n");
			//}
			Assert.IsFalse(listTimeSlots[2].DateTimeStart.Date!=dateTimeSchedule.Date
				|| listTimeSlots[2].DateTimeStart!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,3,20,0)
				|| listTimeSlots[2].DateTimeStop!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,4,0,0)
				|| listTimeSlots[2].OperatoryNum!=opHyg.OperatoryNum);
			//Slot @ 19:00 - 19:40
			//if(listTimeSlots[3].DateTimeStart.Date!=dateTimeSchedule.Date
			//	|| listTimeSlots[3].DateTimeStart!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,19,0,0)
			//	|| listTimeSlots[3].DateTimeStop!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,19,40,0)
			//	|| listTimeSlots[3].OperatoryNum!=opHyg.OperatoryNum) 
			//{
			//	throw new Exception("Invalid post-lunch time slot returned.  See test for expected values.\r\n");
			//}
			Assert.IsFalse(listTimeSlots[3].DateTimeStart.Date!=dateTimeSchedule.Date
				|| listTimeSlots[3].DateTimeStart!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,19,0,0)
				|| listTimeSlots[3].DateTimeStop!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,19,40,0)
				|| listTimeSlots[3].OperatoryNum!=opHyg.OperatoryNum);
			//Last slot.
			//if(listTimeSlots[8].DateTimeStart.Date!=dateTimeSchedule.Date
			//	|| listTimeSlots[8].DateTimeStart!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,22,20,0)
			//	|| listTimeSlots[8].DateTimeStop!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,23,0,0)
			//	|| listTimeSlots[8].OperatoryNum!=opHyg.OperatoryNum) 
			//{
			//	throw new Exception("Invalid last time slot returned.  See test for expected values.\r\n");
			//}
			Assert.IsFalse(listTimeSlots[8].DateTimeStart.Date!=dateTimeSchedule.Date
				|| listTimeSlots[8].DateTimeStart!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,22,20,0)
				|| listTimeSlots[8].DateTimeStop!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,23,0,0)
				|| listTimeSlots[8].OperatoryNum!=opHyg.OperatoryNum);
			//return "68: Passed.  Web Sched - Clinic priority worked as expected.\r\n";
		}

		///<summary>Web Sched - Overflow. Multiple Web Sched operatories should flow into eachother nicely meaning that if the 9 - 10 slot is taken
		///in the first operatory, then the next operatory in line should show the 9 - 10 slot as open.
		///This unit test used to be called "Legacy_TestSixtyNine" for historical purposes.</summary>
		[TestMethod]
		public void TimeSlots_GetAvailableWebSchedTimeSlots_Overflow() {
			//if(specificTest!=0 && specificTest!=69) {
			//	return "";
			//}
			string suffix="69";
			//Start with fresh tables so that our time slots are extremely predictable.
			AppointmentT.ClearAppointmentTable();
			RecallTypeT.ClearRecallTypeTable();
			RecallT.ClearRecallTable();
			OperatoryT.ClearOperatoryTable();
			ScheduleT.ClearScheduleTable();
			ScheduleOpT.ClearScheduleOpTable();
			//Create a date that will always be in the future.  This date will be used for schedules and recalls.
			DateTime dateTimeSchedule=DateTime.Now.AddYears(1);
			DateTime dateTimeScheduleNextDay=dateTimeSchedule.AddDays(1);
			long provNumDoc=ProviderT.CreateProvider("Doc-"+suffix);
			long provNumHyg=ProviderT.CreateProvider("Hyg-"+suffix,isSecondary:true);
			Patient pat=PatientT.CreatePatient(suffix,provNumHyg);
			//Make sure the that Appointment View time increment is set to 10 min.
			Prefs.UpdateInt(PrefName.AppointmentTimeIncrement,10);
			Def defLunchBlockout=DefinitionT.CreateDefinition(DefCat.BlockoutTypes,"Lunch-"+suffix,itemColor:System.Drawing.Color.Azure);
			//Create a psudo prophy recall type that lasts 40 mins and has an interval of every 6 months and 1 day.
			RecallType recallType=RecallTypeT.CreateRecallType("Prophy-"+suffix,"D1110,D1330","//X/",new Interval(1,0,6,0));
			//Create a recall for our patient.
			Recall recall=RecallT.CreateRecall(pat.PatNum,recallType.RecallTypeNum,dateTimeSchedule,new Interval(1,0,6,0));
			//Create operatories for the providers.
			Operatory opDoc=OperatoryT.CreateOperatory("1-"+suffix,"Doc Op - "+suffix,provNumDoc,provNumHyg,isWebSched:true,itemOrder:0);
			Operatory opHyg=OperatoryT.CreateOperatory("2-"+suffix,"Hyg Op - "+suffix,provNumDoc,provNumHyg,isWebSched:true,itemOrder:1,isHygiene:true);
			//Create two schedules for the doctor from 09:00 - 10:00 on two different days.
			Schedule schedDoc=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,9,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,10,0,0).Ticks),schedType:ScheduleType.Provider,provNum:provNumDoc);
			Schedule schedDocNextDay=ScheduleT.CreateSchedule(dateTimeScheduleNextDay,new TimeSpan(new DateTime(1,1,1,9,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,10,0,0).Ticks),schedType:ScheduleType.Provider,provNum:provNumDoc);
			//Now link up the schedule entries to the Web Sched operatory
			ScheduleOpT.CreateScheduleOp(opDoc.OperatoryNum,schedDoc.ScheduleNum);
			ScheduleOpT.CreateScheduleOp(opDoc.OperatoryNum,schedDocNextDay.ScheduleNum);
			//Create the same schedule for the other provider but only for the first day.
			Schedule schedHyg=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,9,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,10,0,0).Ticks),schedType:ScheduleType.Provider,provNum:provNumHyg);
			//Link the crazy schedule up to the non-Web Sched operatory.
			ScheduleOpT.CreateScheduleOp(opHyg.OperatoryNum,schedHyg.ScheduleNum);
			//Create two appointments for both of the operatories during the scheduled times on the first day.
			AppointmentT.CreateAppointment(pat.PatNum,new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,9,0,0)
				,opDoc.OperatoryNum,provNumDoc,pattern: "//XXXX//");
			AppointmentT.CreateAppointment(pat.PatNum,new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,9,0,0)
				,opHyg.OperatoryNum,provNumDoc,provNumHyg,pattern: "//XXXX//",isHygiene: true);
			//The open time slot returned should be for the Web Sched operatory on the NEXT DAY due to appointments blocking the current day.
			List<TimeSlot> listTimeSlots=TimeSlots.GetAvailableWebSchedTimeSlots(recall.RecallNum,dateTimeSchedule,dateTimeSchedule.AddDays(30));
			//There should be 1 time slot returned that spans from 09:00 - 09:40.
			//if(listTimeSlots.Count!=1) {
			//	throw new Exception("Incorrect number of time slots returned.  Expected 1, received "+listTimeSlots.Count+".\r\n");
			//}
			Assert.AreEqual(1,listTimeSlots.Count);
			//if(listTimeSlots.Any(x => x.OperatoryNum!=opDoc.OperatoryNum)) {
			//	throw new Exception("Invalid operatory time slot returned.  Expected all time slots to be in operatory #"+opDoc.OperatoryNum+"\r\n");
			//}
			Assert.IsFalse(listTimeSlots.Any(x => x.OperatoryNum!=opDoc.OperatoryNum));
			//if(listTimeSlots[0].DateTimeStart.Date!=dateTimeScheduleNextDay.Date
			//	|| listTimeSlots[0].DateTimeStart!=new DateTime(dateTimeScheduleNextDay.Year,dateTimeScheduleNextDay.Month,dateTimeScheduleNextDay.Day,9,0,0)
			//	|| listTimeSlots[0].DateTimeStop!=new DateTime(dateTimeScheduleNextDay.Year,dateTimeScheduleNextDay.Month,dateTimeScheduleNextDay.Day,9,40,0)
			//	|| listTimeSlots[0].OperatoryNum!=opDoc.OperatoryNum) 
			//{
			//	throw new Exception("Invalid first time slot returned.  See test for expected values.\r\n");
			//}
			Assert.IsFalse(listTimeSlots[0].DateTimeStart.Date!=dateTimeScheduleNextDay.Date
				|| listTimeSlots[0].DateTimeStart!=new DateTime(dateTimeScheduleNextDay.Year,dateTimeScheduleNextDay.Month,dateTimeScheduleNextDay.Day,9,0,0)
				|| listTimeSlots[0].DateTimeStop!=new DateTime(dateTimeScheduleNextDay.Year,dateTimeScheduleNextDay.Month,dateTimeScheduleNextDay.Day,9,40,0)
				|| listTimeSlots[0].OperatoryNum!=opDoc.OperatoryNum);
			//return "69: Passed.  Web Sched - Operatory overflow worked as expected.\r\n";
		}

		///<summary>Web Sched - Double Booking.  Providers should not be overwhelmed.  An open time slot does not mean that the slot should always be 
		///returned to the patient as available.  If the provider is double booked, the time slot should not be offered as a choice.
		///This unit test used to be called "Legacy_TestSeventy" for historical purposes.</summary>
		[TestMethod]
		public void TimeSlots_GetAvailableWebSchedTimeSlots_DoubleBooking() {
			//if(specificTest!=0 && specificTest!=70) {
			//	return "";
			//}
			string suffix="70";
			//Start with fresh tables so that our time slots are extremely predictable.
			AppointmentT.ClearAppointmentTable();
			RecallTypeT.ClearRecallTypeTable();
			RecallT.ClearRecallTable();
			OperatoryT.ClearOperatoryTable();
			ScheduleT.ClearScheduleTable();
			ScheduleOpT.ClearScheduleOpTable();
			//Create a date that will always be in the future.  This date will be used for schedules and recalls.
			DateTime dateTimeSchedule=DateTime.Now.AddYears(1);
			DateTime dateTimeScheduleNextDay=dateTimeSchedule.AddDays(1);
			long provNumDoc=ProviderT.CreateProvider("Doc-"+suffix);
			long provNumHyg=ProviderT.CreateProvider("Hyg-"+suffix,isSecondary:true);
			Patient pat=PatientT.CreatePatient(suffix,provNumHyg);
			//Make sure the that Appointment View time increment is set to 10 min.
			Prefs.UpdateInt(PrefName.AppointmentTimeIncrement,10);
			Def defLunchBlockout=DefinitionT.CreateDefinition(DefCat.BlockoutTypes,"Lunch-"+suffix,itemColor:System.Drawing.Color.Azure);
			//Create a psudo prophy recall type that lasts 40 mins and has an interval of every 6 months and 1 day.
			RecallType recallType=RecallTypeT.CreateRecallType("Prophy-"+suffix,"D1110,D1330","//X/",new Interval(1,0,6,0));
			//Create a recall for our patient.
			Recall recall=RecallT.CreateRecall(pat.PatNum,recallType.RecallTypeNum,dateTimeSchedule,new Interval(1,0,6,0));
			//Create operatories for the providers.
			//Firt op will ONLY have provNumDoc associated, NOT the hygienist because we want to keep it simple with one provider to consider.
			Operatory opDoc=OperatoryT.CreateOperatory("1-"+suffix,"Doc Op - "+suffix,provNumHyg,isWebSched:true,itemOrder:2);
			//Now for two non-Web Sched ops that the provNumDoc will be double booked for.  Hyg provs on these ops doesn't affect anything.
			Operatory opHyg=OperatoryT.CreateOperatory("2-"+suffix,"Hyg Op - "+suffix,provNumDoc,provNumHyg,itemOrder:1,isHygiene:true);
			Operatory opExtra=OperatoryT.CreateOperatory("3-"+suffix,"Extra Op - "+suffix,provNumDoc,provNumHyg,itemOrder:0);
			//Create two schedules for the doctor from 09:00 - 10:00 on two different days.
			Schedule schedDoc=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,9,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,10,0,0).Ticks),schedType:ScheduleType.Provider,provNum:provNumDoc);
			Schedule schedDocNextDay=ScheduleT.CreateSchedule(dateTimeScheduleNextDay,new TimeSpan(new DateTime(1,1,1,9,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,10,0,0).Ticks),schedType:ScheduleType.Provider,provNum:provNumDoc);
			//Now link up the schedule entries to all of the operatories
			ScheduleOpT.CreateScheduleOp(opDoc.OperatoryNum,schedDoc.ScheduleNum);
			ScheduleOpT.CreateScheduleOp(opDoc.OperatoryNum,schedDocNextDay.ScheduleNum);
			ScheduleOpT.CreateScheduleOp(opHyg.OperatoryNum,schedDoc.ScheduleNum);
			ScheduleOpT.CreateScheduleOp(opHyg.OperatoryNum,schedDocNextDay.ScheduleNum);
			ScheduleOpT.CreateScheduleOp(opExtra.OperatoryNum,schedDoc.ScheduleNum);
			ScheduleOpT.CreateScheduleOp(opExtra.OperatoryNum,schedDocNextDay.ScheduleNum);
			//Create two appointments for the two non-Web Sched operatories during the scheduled times on the first day.
			AppointmentT.CreateAppointment(pat.PatNum,new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,9,0,0)
				,opHyg.OperatoryNum,provNumDoc,pattern: "//XXXXXXXX");
			AppointmentT.CreateAppointment(pat.PatNum,new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,9,0,0)
				,opExtra.OperatoryNum,provNumDoc,pattern: "//XXXXXXXX");
			//The open time slot returned should be for the Web Sched operatory on the NEXT DAY due to double booking appointments on the current day.
			List<TimeSlot> listTimeSlots=TimeSlots.GetAvailableWebSchedTimeSlots(recall.RecallNum,dateTimeSchedule,dateTimeSchedule.AddDays(30));
			//There should be 1 time slot returned that spans from 09:00 - 09:40.
			//if(listTimeSlots.Count!=1) {
			//	throw new Exception("Incorrect number of time slots returned.  Expected 1, received "+listTimeSlots.Count+".\r\n");
			//}
			Assert.AreEqual(1,listTimeSlots.Count);
			//if(listTimeSlots.Any(x => x.OperatoryNum!=opDoc.OperatoryNum)) {
			//	throw new Exception("Invalid operatory time slot returned.  Expected all time slots to be in operatory #"+opDoc.OperatoryNum+"\r\n");
			//}
			Assert.IsFalse(listTimeSlots.Any(x => x.OperatoryNum!=opDoc.OperatoryNum));
			//First slot.
			//if(listTimeSlots[0].DateTimeStart.Date!=dateTimeScheduleNextDay.Date
			//	|| listTimeSlots[0].DateTimeStart!=new DateTime(dateTimeScheduleNextDay.Year,dateTimeScheduleNextDay.Month,dateTimeScheduleNextDay.Day,9,0,0)
			//	|| listTimeSlots[0].DateTimeStop!=new DateTime(dateTimeScheduleNextDay.Year,dateTimeScheduleNextDay.Month,dateTimeScheduleNextDay.Day,9,40,0)
			//	|| listTimeSlots[0].OperatoryNum!=opDoc.OperatoryNum) 
			//{
			//	throw new Exception("Invalid first time slot returned.  See test for expected values.\r\n");
			//}
			Assert.IsFalse(listTimeSlots[0].DateTimeStart.Date!=dateTimeScheduleNextDay.Date
				|| listTimeSlots[0].DateTimeStart!=new DateTime(dateTimeScheduleNextDay.Year,dateTimeScheduleNextDay.Month,dateTimeScheduleNextDay.Day,9,0,0)
				|| listTimeSlots[0].DateTimeStop!=new DateTime(dateTimeScheduleNextDay.Year,dateTimeScheduleNextDay.Month,dateTimeScheduleNextDay.Day,9,40,0)
				|| listTimeSlots[0].OperatoryNum!=opDoc.OperatoryNum);
			//return "70: Passed.  Web Sched - Double booking worked as expected.\r\n";
		}

		///<summary>Web Sched - Basic time slot finding.  No complex scenarios, just making sure small offices find open slots.
		///This unit test used to be called "Legacy_TestSeventyOne" for historical purposes.</summary>
		[TestMethod]
		public void TimeSlots_GetAvailableWebSchedTimeSlots_Basic() {
			//if(specificTest!=0 && specificTest!=71) {
			//	return "";
			//}
			string suffix="71";
			//Start with fresh tables so that our time slots are extremely predictable.
			RecallTypeT.ClearRecallTypeTable();
			RecallT.ClearRecallTable();
			OperatoryT.ClearOperatoryTable();
			ScheduleT.ClearScheduleTable();
			ScheduleOpT.ClearScheduleOpTable();
			//Create a date that will always be in the future.  This date will be used for schedules and recalls.
			DateTime dateTimeSchedule=DateTime.Now.AddYears(1);
			long provNumDoc=ProviderT.CreateProvider("Doc-"+suffix);
			long provNumHyg=ProviderT.CreateProvider("Hyg-"+suffix,isSecondary:true);
			Patient pat=PatientT.CreatePatient(suffix,provNumDoc);
			//Make sure the that Appointment View time increment is set to 10 min.
			Prefs.UpdateInt(PrefName.AppointmentTimeIncrement,10);
			Def defLunchBlockout=DefinitionT.CreateDefinition(DefCat.BlockoutTypes,"Lunch-"+suffix,itemColor:System.Drawing.Color.Azure);
			//Create a psudo prophy recall type that lasts 40 mins and has an interval of every 6 months and 1 day.
			RecallType recallType=RecallTypeT.CreateRecallType("Prophy-"+suffix,"D1110,D1330","//X/",new Interval(1,0,6,0));
			//Create a recall for our patient.
			Recall recall=RecallT.CreateRecall(pat.PatNum,recallType.RecallTypeNum,dateTimeSchedule,new Interval(1,0,6,0));
			//Create operatories for the providers but make the Hygiene op NON-WEB SCHED.
			Operatory opDoc=OperatoryT.CreateOperatory("1-"+suffix,"Doc Op - "+suffix,provNumDoc,provNumHyg,isWebSched:true,itemOrder:0);
			Operatory opHyg=OperatoryT.CreateOperatory("2-"+suffix,"Hyg Op - "+suffix,provNumDoc,provNumHyg,itemOrder:1,isHygiene:true);
			//Create a schedule for the doctor from 09:00 - 11:30 with a 30 min break and then back to work from 12:00 - 17:00
			Schedule schedDocMorning=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,9,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,11,30,0).Ticks),schedType:ScheduleType.Provider,provNum:provNumDoc);
			//Create a blockout for lunch because why not.
			Schedule schedDocLunch=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,11,30,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,12,0,0).Ticks),schedType:ScheduleType.Blockout,blockoutType:defLunchBlockout.DefNum);
			//Schedule for closing from 12:00 - 17:00
			Schedule schedDocEvening=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,12,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,17,0,0).Ticks),schedType:ScheduleType.Provider,provNum: provNumDoc);
			//Now link up the schedule entries to the Web Sched operatory
			ScheduleOpT.CreateScheduleOp(opDoc.OperatoryNum,schedDocMorning.ScheduleNum);
			ScheduleOpT.CreateScheduleOp(opDoc.OperatoryNum,schedDocLunch.ScheduleNum);
			ScheduleOpT.CreateScheduleOp(opDoc.OperatoryNum,schedDocEvening.ScheduleNum);
			//Create a crazy schedule for the non-Web Sched operatory which should not return ANY of the available time slots for that op.
			//02:00 - 04:00 with a 15 hour break and then back to work from 19:00 - 23:20
			Schedule schedDocMorning2=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,2,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,4,0,0).Ticks),schedType:ScheduleType.Provider,provNum:provNumDoc);
			//Create a European length lunch.
			Schedule schedDocLunch2=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,4,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,19,0,0).Ticks),schedType:ScheduleType.Blockout,blockoutType:defLunchBlockout.DefNum);
			//Schedule for closing from 19:00 - 23:20
			Schedule schedDocEvening2=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,19,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,23,20,0).Ticks),schedType:ScheduleType.Provider,provNum:provNumDoc);
			//Link the crazy schedule up to the non-Web Sched operatory.
			ScheduleOpT.CreateScheduleOp(opHyg.OperatoryNum,schedDocMorning2.ScheduleNum);
			ScheduleOpT.CreateScheduleOp(opHyg.OperatoryNum,schedDocLunch2.ScheduleNum);
			ScheduleOpT.CreateScheduleOp(opHyg.OperatoryNum,schedDocEvening2.ScheduleNum);
			//The open time slots returned should all return for the Web Sched operatory and not the other one.
			List<TimeSlot> listTimeSlots=TimeSlots.GetAvailableWebSchedTimeSlots(recall.RecallNum,dateTimeSchedule,dateTimeSchedule.AddDays(30));
			//There should be 10 time slots returned that span from 09:00 - 16:40.
			//The 11:00 - 12:00 hour should be open (can't fit 40 min appt over lunch break).
			if(listTimeSlots.Count!=10) {
				throw new Exception("Incorrect number of time slots returned.  Expected 10, received "+listTimeSlots.Count+".");
			}
			if(listTimeSlots.Any(x => x.OperatoryNum!=opDoc.OperatoryNum)) {
				throw new Exception("Invalid operatory time slot returned.  Expected all time slots to be in operatory #"+opDoc.OperatoryNum);
			}
			//Just check 4 specific time slots.  Don't worry about checking all of them cause that is a little overkill.
			//First slot.
			//if(listTimeSlots[0].DateTimeStart.Date!=dateTimeSchedule.Date
			//	|| listTimeSlots[0].DateTimeStart!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,9,0,0)
			//	|| listTimeSlots[0].DateTimeStop!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,9,40,0)
			//	|| listTimeSlots[0].OperatoryNum!=opDoc.OperatoryNum) 
			//{
			//	throw new Exception("Invalid first time slot returned.  See test for expected values.\r\n");
			//}
			Assert.IsFalse(listTimeSlots[0].DateTimeStart.Date!=dateTimeSchedule.Date
				|| listTimeSlots[0].DateTimeStart!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,9,0,0)
				|| listTimeSlots[0].DateTimeStop!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,9,40,0)
				|| listTimeSlots[0].OperatoryNum!=opDoc.OperatoryNum);
			//Slot @ 10:20 - 11:00
			//if(listTimeSlots[2].DateTimeStart.Date!=dateTimeSchedule.Date
			//	|| listTimeSlots[2].DateTimeStart!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,10,20,0)
			//	|| listTimeSlots[2].DateTimeStop!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,11,0,0)
			//	|| listTimeSlots[2].OperatoryNum!=opDoc.OperatoryNum) 
			//{
			//	throw new Exception("Invalid pre-lunch time slot returned.  See test for expected values.\r\n");
			//}
			Assert.IsFalse(listTimeSlots[2].DateTimeStart.Date!=dateTimeSchedule.Date
				|| listTimeSlots[2].DateTimeStart!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,10,20,0)
				|| listTimeSlots[2].DateTimeStop!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,11,0,0)
				|| listTimeSlots[2].OperatoryNum!=opDoc.OperatoryNum);
			//Slot @ 12:00 - 12:40
			//if(listTimeSlots[3].DateTimeStart.Date!=dateTimeSchedule.Date
			//	|| listTimeSlots[3].DateTimeStart!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,12,0,0)
			//	|| listTimeSlots[3].DateTimeStop!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,12,40,0)
			//	|| listTimeSlots[3].OperatoryNum!=opDoc.OperatoryNum) 
			//{
			//	throw new Exception("Invalid post-lunch time slot returned.  See test for expected values.\r\n");
			//}
			Assert.IsFalse(listTimeSlots[3].DateTimeStart.Date!=dateTimeSchedule.Date
				|| listTimeSlots[3].DateTimeStart!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,12,0,0)
				|| listTimeSlots[3].DateTimeStop!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,12,40,0)
				|| listTimeSlots[3].OperatoryNum!=opDoc.OperatoryNum);
			//Last slot.
			//if(listTimeSlots[9].DateTimeStart.Date!=dateTimeSchedule.Date
			//	|| listTimeSlots[9].DateTimeStart!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,16,0,0)
			//	|| listTimeSlots[9].DateTimeStop!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,16,40,0)
			//	|| listTimeSlots[9].OperatoryNum!=opDoc.OperatoryNum) 
			//{
			//	throw new Exception("Invalid last time slot returned.  See test for expected values.\r\n");
			//}
			Assert.IsFalse(listTimeSlots[9].DateTimeStart.Date!=dateTimeSchedule.Date
				|| listTimeSlots[9].DateTimeStart!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,16,0,0)
				|| listTimeSlots[9].DateTimeStop!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,16,40,0)
				|| listTimeSlots[9].OperatoryNum!=opDoc.OperatoryNum);
			//return "71: Passed.  Web Sched - Basic time slot finding returned the correct time slots.\r\n";
		}

		///<summary>Web Sched - Operatory priority.  Time slots should be scheduled based on operatory item order (left to right on sched).
		///This unit test used to be called "Legacy_TestSeventyTwo" for historical purposes.</summary>
		[TestMethod]
		public void TimeSlots_GetAvailableWebSchedTimeSlots_OperatoryPriority() {
			//if(specificTest!=0 && specificTest!=72) {
			//	return "";
			//}
			string suffix="72";
			//Start with fresh tables so that our time slots are extremely predictable.
			RecallTypeT.ClearRecallTypeTable();
			RecallT.ClearRecallTable();
			OperatoryT.ClearOperatoryTable();
			ScheduleT.ClearScheduleTable();
			ScheduleOpT.ClearScheduleOpTable();
			//Create a date that will always be in the future.  This date will be used for schedules and recalls.
			DateTime dateTimeSchedule=DateTime.Now.AddYears(1);
			long provNumDoc=ProviderT.CreateProvider("Doc-"+suffix);
			long provNumHyg=ProviderT.CreateProvider("Hyg-"+suffix,isSecondary:true);
			Patient pat=PatientT.CreatePatient(suffix,provNumHyg);
			//Make sure the that Appointment View time increment is set to 10 min.
			Prefs.UpdateInt(PrefName.AppointmentTimeIncrement,10);
			Def defLunchBlockout=DefinitionT.CreateDefinition(DefCat.BlockoutTypes,"Lunch-"+suffix,itemColor:System.Drawing.Color.Azure);
			//Create a psudo prophy recall type that lasts 40 mins and has an interval of every 6 months and 1 day.
			RecallType recallType=RecallTypeT.CreateRecallType("Prophy-"+suffix,"D1110,D1330","//X/",new Interval(1,0,6,0));
			//Create a recall for our patient.
			Recall recall=RecallT.CreateRecall(pat.PatNum,recallType.RecallTypeNum,dateTimeSchedule,new Interval(1,0,6,0));
			//Create operatories for the providers but make the Hygiene operatory show up FIRST (item order = 0) before the doc's op.
			Operatory opDoc=OperatoryT.CreateOperatory("1-"+suffix,"Doc Op - "+suffix,provNumDoc,provNumHyg,isWebSched:true,itemOrder:1);
			Operatory opHyg=OperatoryT.CreateOperatory("2-"+suffix,"Hyg Op - "+suffix,provNumDoc,provNumHyg,isWebSched:true,itemOrder:0,isHygiene:true);
			//Create a schedule for the doctor from 09:00 - 10:00
			Schedule schedDoc=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,9,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,10,0,0).Ticks),schedType:ScheduleType.Provider,provNum:provNumDoc);
			//Now link up the schedule entries to the Web Sched operatory
			ScheduleOpT.CreateScheduleOp(opDoc.OperatoryNum,schedDoc.ScheduleNum);
			//Create the exact same schedule for the other provider.
			Schedule schedHyg=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,9,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,10,0,0).Ticks),schedType:ScheduleType.Provider,provNum:provNumHyg);
			//Link the crazy schedule up to the non-Web Sched operatory.
			ScheduleOpT.CreateScheduleOp(opHyg.OperatoryNum,schedHyg.ScheduleNum);
			//The first open time slot returned should be for the first Web Sched operatory and not the second one due to item order.
			List<TimeSlot> listTimeSlots=TimeSlots.GetAvailableWebSchedTimeSlots(recall.RecallNum,dateTimeSchedule,dateTimeSchedule.AddDays(30));
			//Any time slot returned should span from 09:00 - 09:40.
			Assert.AreEqual(1,listTimeSlots.DistinctBy(x => x.DateTimeStart).Count());
			//First slot.
			Assert.AreEqual(dateTimeSchedule.Date,listTimeSlots[0].DateTimeStart.Date);
			Assert.AreEqual(new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,9,0,0),listTimeSlots[0].DateTimeStart);
			Assert.AreEqual(new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,9,40,0),listTimeSlots[0].DateTimeStop);
			Assert.AreEqual(opHyg.OperatoryNum,listTimeSlots[0].OperatoryNum);
		}

		///<summary>Web Sched - Appointments should be able to start at their earliest available times.
		///The first phase of Web Sched only allowed users to schedule appointments on top of the hour, one per hour.
		///Phase two logic would go through the schedule one time increment at a time until it found a slot big enough to fit the appointment.
		///Once a slot big enough was found it would check for double booking.  If there was double booking then it would continue from the end of the apt.
		///This unit test is for the logic that is applied in phase three where the opening logic should not continue from the end of the apt but instead
		///should scoot the entire appointment along the schedule one time increment at a time until a suitable slot is found.
		///This unit test used to be called "Legacy_TestEightySix" for historical purposes.</summary>
		[TestMethod]
		public void TimeSlots_GetAvailableWebSchedTimeSlots_EarliestTime() {
			//E.g. 10 min increments, provider scheduled 8 - 9, provider scheduled in another operatory from 8:00 - 8:20 and then from 8:30 - 8:50.
			//A Web Sched appointment that is 30 minutes long (//XX//) should be able to schedule an appointment @ 8:10 - 8:40
			//Old logic would not find this appointment slot because it would find 8:00 - 8:30 open but then would find a double booking collision.
			//It would then continue looking for available openings from 8:30 onward.  We need it to instead scoot one time increment (8:10 - 8:40).
			//if(specificTest!=0 && specificTest!=86) {
			//	return "";
			//}
			string suffix="86";
			//Start with fresh tables so that our time slots are extremely predictable.
			AppointmentT.ClearAppointmentTable();
			RecallTypeT.ClearRecallTypeTable();
			RecallT.ClearRecallTable();
			OperatoryT.ClearOperatoryTable();
			ScheduleT.ClearScheduleTable();
			ScheduleOpT.ClearScheduleOpTable();
			//Create a date that will always be in the future.  This date will be used for schedules and recalls.
			DateTime dateTimeSchedule=DateTime.Now.AddYears(1);
			long provNumDoc=ProviderT.CreateProvider("Doc-"+suffix);
			long provNumHyg=ProviderT.CreateProvider("Hyg-"+suffix,isSecondary:true);
			Patient pat=PatientT.CreatePatient(suffix,provNumHyg);
			//Make sure the that Appointment View time increment is set to 10 min.
			Prefs.UpdateInt(PrefName.AppointmentTimeIncrement,10);
			//Create a psudo prophy recall type that lasts 30 mins with prov time in the middle and has an interval of every 6 months and 1 day.
			RecallType recallType=RecallTypeT.CreateRecallType("Prophy-"+suffix,"D1110,D1330","/X/",new Interval(1,0,6,0));
			//Create a recall for our patient.
			Recall recall=RecallT.CreateRecall(pat.PatNum,recallType.RecallTypeNum,dateTimeSchedule,new Interval(1,0,6,0));
			//Create operatories for the providers.
			//Firt op will ONLY have provNumDoc associated, NOT the hygienist because we want to keep it simple with one provider to consider.
			Operatory opDoc=OperatoryT.CreateOperatory("1-"+suffix,"Doc Op - "+suffix,provNumDoc,isWebSched:true,itemOrder:1);
			//Now for an extra non-Web Sched op that the provNumDoc will be double booked for.  Hyg provs on this op doesn't affect anything.
			Operatory opHyg=OperatoryT.CreateOperatory("2-"+suffix,"Hyg Op - "+suffix,provNumDoc,provNumHyg,itemOrder:2,isHygiene:true);
			//Create one schedule for the doctor from 08:00 - 09:00.
			Schedule schedOpDoc=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,8,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,9,0,0).Ticks),schedType:ScheduleType.Provider,provNum:provNumDoc);
			//Now link up the schedule entries to all of the operatories
			ScheduleOpT.CreateScheduleOp(opDoc.OperatoryNum,schedOpDoc.ScheduleNum);
			ScheduleOpT.CreateScheduleOp(opHyg.OperatoryNum,schedOpDoc.ScheduleNum);
			//Create an appointment that will leave an opening for the doctor @8:30 (middle of the appointment) in our Web Sched operatory.
			//The appointment that we are about to create needs to be in the extra operatory so that Web Sched is able to find the valid opening.
			AppointmentT.CreateAppointment(pat.PatNum,new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,8,0,0)
				,opHyg.OperatoryNum,provNumDoc,pattern: "XXXX//XXXX//");
			List<TimeSlot> listTimeSlots=TimeSlots.GetAvailableWebSchedTimeSlots(recall.RecallNum,dateTimeSchedule,dateTimeSchedule.AddDays(30));
			//There should be exactly one time slot returned that spans from 08:10 - 08:30.
			//if(listTimeSlots.Count!=1) {
			//	throw new Exception("Incorrect number of time slots returned.  Expected 1, received "+listTimeSlots.Count+".\r\n");
			//}
			Assert.AreEqual(1,listTimeSlots.Count);
			//if(listTimeSlots.Any(x => x.OperatoryNum!=opDoc.OperatoryNum)) {
			//	throw new Exception("Invalid operatory time slot returned.  Expected all time slots to be in operatory #"+opDoc.OperatoryNum+"\r\n");
			//}
			Assert.IsFalse(listTimeSlots.Any(x => x.OperatoryNum!=opDoc.OperatoryNum));
			//First slot.
			//if(listTimeSlots[0].DateTimeStart.Date!=dateTimeSchedule.Date
			//	|| listTimeSlots[0].DateTimeStart!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,8,10,0)
			//	|| listTimeSlots[0].DateTimeStop!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,8,40,0)
			//	|| listTimeSlots[0].OperatoryNum!=opDoc.OperatoryNum) 
			//{
			//	throw new Exception("Invalid first time slot returned.  See test for expected values.\r\n");
			//}
			Assert.IsFalse(listTimeSlots[0].DateTimeStart.Date!=dateTimeSchedule.Date
				|| listTimeSlots[0].DateTimeStart!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,8,10,0)
				|| listTimeSlots[0].DateTimeStop!=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,8,40,0)
				|| listTimeSlots[0].OperatoryNum!=opDoc.OperatoryNum);
			//return "86: Passed.  Web Sched - Available time slot scooting is working as expected.\r\n";
		}

		///<summary>All of the Web Sched applications have gone through lots of phases in regards to how many days / months are considered.
		///This unit test is strictly here to make sure that time slots outside of the date range provided are NOT returned.</summary>
		[TestMethod]
		public void TimeSlots_GetTimeSlotsForRange_DateRange() {
			string suffix="TimeSlots_GetTimeSlotsForRange_DateRange";
			//Start with fresh tables so that our time slots are extremely predictable.
			RecallTypeT.ClearRecallTypeTable();
			RecallT.ClearRecallTable();
			OperatoryT.ClearOperatoryTable();
			ScheduleT.ClearScheduleTable();
			ScheduleOpT.ClearScheduleOpTable();
			//Create a date that will always be in the future.  This date will be used for schedules and recalls.
			DateTime dateTimeSchedule=DateTime.Now.AddYears(1);
			long provNumDoc=ProviderT.CreateProvider("Doc-"+suffix);
			long provNumHyg=ProviderT.CreateProvider("Hyg-"+suffix,isSecondary:true);
			Patient pat=PatientT.CreatePatient(suffix,provNumDoc);
			//Make sure the that Appointment View time increment is set to 10 min.
			Prefs.UpdateInt(PrefName.AppointmentTimeIncrement,10);
			Def defLunchBlockout=DefinitionT.CreateDefinition(DefCat.BlockoutTypes,"Lunch-"+suffix,itemColor:System.Drawing.Color.Azure);
			//Create operatories for the providers but make the Hygiene op NON-WEB SCHED.
			Operatory opDoc=OperatoryT.CreateOperatory("1-"+suffix,"Doc Op - "+suffix,provNumDoc,provNumHyg,isWebSched:true,itemOrder:0);
			Operatory opHyg=OperatoryT.CreateOperatory("2-"+suffix,"Hyg Op - "+suffix,provNumDoc,provNumHyg,itemOrder:1,isHygiene:true);
			//Create a schedule for the doctor from 09:00 - 11:30 with a 30 min break and then back to work from 12:00 - 17:00
			Schedule schedDocMorning=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,9,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,11,30,0).Ticks),schedType:ScheduleType.Provider,provNum:provNumDoc);
			//Create a blockout for lunch because why not.
			Schedule schedDocLunch=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,11,30,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,12,0,0).Ticks),schedType:ScheduleType.Blockout,blockoutType:defLunchBlockout.DefNum);
			//Schedule for closing from 12:00 - 17:00
			Schedule schedDocEvening=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,12,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,17,0,0).Ticks),schedType:ScheduleType.Provider,provNum: provNumDoc);
			//Now link up the schedule entries to the Web Sched operatory
			ScheduleOpT.CreateScheduleOp(opDoc.OperatoryNum,schedDocMorning.ScheduleNum);
			ScheduleOpT.CreateScheduleOp(opDoc.OperatoryNum,schedDocLunch.ScheduleNum);
			ScheduleOpT.CreateScheduleOp(opDoc.OperatoryNum,schedDocEvening.ScheduleNum);
			//Create a crazy schedule for the non-Web Sched operatory which should not return ANY of the available time slots for that op.
			//02:00 - 04:00 with a 15 hour break and then back to work from 19:00 - 23:20
			Schedule schedDocMorning2=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,2,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,4,0,0).Ticks),schedType:ScheduleType.Provider,provNum:provNumDoc);
			//Create a European length lunch.
			Schedule schedDocLunch2=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,4,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,19,0,0).Ticks),schedType:ScheduleType.Blockout,blockoutType:defLunchBlockout.DefNum);
			//Schedule for closing from 19:00 - 23:20
			Schedule schedDocEvening2=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,19,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,23,20,0).Ticks),schedType:ScheduleType.Provider,provNum:provNumDoc);
			//Link the crazy schedule up to the non-Web Sched operatory.
			ScheduleOpT.CreateScheduleOp(opHyg.OperatoryNum,schedDocMorning2.ScheduleNum);
			ScheduleOpT.CreateScheduleOp(opHyg.OperatoryNum,schedDocLunch2.ScheduleNum);
			ScheduleOpT.CreateScheduleOp(opHyg.OperatoryNum,schedDocEvening2.ScheduleNum);
			//Create some simple schedules that will fall outside of our date range (before and after).
			DateTime dateTimeScheduleBefore=dateTimeSchedule.AddMonths(-1);
			Schedule schedDocMonthBefore=ScheduleT.CreateSchedule(dateTimeScheduleBefore,new TimeSpan(new DateTime(1,1,1,9,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,17,0,0).Ticks),schedType:ScheduleType.Provider,provNum:provNumDoc);
			ScheduleOpT.CreateScheduleOp(opDoc.OperatoryNum,schedDocMonthBefore.ScheduleNum);
			//Now for the schedule that falls after.
			DateTime dateTimeScheduleAfter=dateTimeSchedule.AddMonths(1);
			Schedule schedDocMonthAfter=ScheduleT.CreateSchedule(dateTimeScheduleAfter,new TimeSpan(new DateTime(1,1,1,9,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,17,0,0).Ticks),schedType:ScheduleType.Provider,provNum:provNumDoc);
			ScheduleOpT.CreateScheduleOp(opDoc.OperatoryNum,schedDocMonthAfter.ScheduleNum);
			//Make the date range for the entire month that we landed on.
			DateTime dateStart=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,1);
			DateTime dateEnd=dateStart.AddMonths(1).AddDays(-1);//Will always return the last day of the month.
			//Refresh all of the schedules that we just created above so that their non db columns are filled correctly.
			List<Schedule> listSchedules=Schedules.GetByScheduleNum(new List<long>() {
				schedDocMorning.ScheduleNum,
				schedDocMorning2.ScheduleNum,
				schedDocLunch.ScheduleNum,
				schedDocLunch2.ScheduleNum,
				schedDocEvening.ScheduleNum,
				schedDocEvening2.ScheduleNum,
				schedDocMonthBefore.ScheduleNum,
				schedDocMonthAfter.ScheduleNum,
			});
			List<TimeSlot> listTimeSlots=TimeSlots.GetTimeSlotsForRange(dateStart,dateEnd
				,"/X/" //The time pattern is not very important here (other than being short enough to actually return at least one time slot).
				,new List<long>() { provNumDoc,provNumHyg }
				,new List<Operatory>() { opDoc,opHyg }
				,listSchedules
				,null);//Null clinic will only consider ops with ClinicNum set to 0.
			//There should not be ANY time slots returned that fall outside of our start and end date ranges.
			Assert.IsTrue(listTimeSlots.All(x => x.DateTimeStart.Date.Between(dateStart,dateEnd)));
		}

		///<summary>The last available time slot for today is not always getting returned in a very specific setup.
		///This unit test is for helping make sure that our time slot finding logic returns expected results for same day time slots.</summary>
		[TestMethod]
		public void TimeSlots_GetTimeSlotsForRange_LastSlotSameDay() {
			string suffix=MethodBase.GetCurrentMethod().Name;
			//Have the date we prefer be today.  This date will be used for schedules time slot finding.
			DateTime dateTimeSchedule=DateTime.Now;
			long provNumHyg=ProviderT.CreateProvider("Hyg-"+suffix);
			Patient pat=PatientT.CreatePatient(suffix,provNumHyg);
			//Make sure the that Appointment View time increment is set to 15 min.
			Prefs.UpdateInt(PrefName.AppointmentTimeIncrement,15);
			//Create an operatory for the provider.
			Operatory opHyg=OperatoryT.CreateOperatory("1-"+suffix,"Hyg Op - "+suffix,provHygienist:provNumHyg,isWebSched:true,itemOrder:0);
			//Create a schedule for the provider from 08:00 - 19:00
			Schedule schedDoc=ScheduleT.CreateSchedule(dateTimeSchedule,new TimeSpan(new DateTime(1,1,1,8,0,0).Ticks)
				,new TimeSpan(new DateTime(1,1,1,19,0,0).Ticks),schedType:ScheduleType.Provider,provNum:provNumHyg);
			//Now link up the schedule entry to the Web Sched operatory
			ScheduleOpT.CreateScheduleOp(opHyg.OperatoryNum,schedDoc.ScheduleNum);
			//Make the date range for the entire month that we landed on.
			DateTime dateStart=new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,1);
			DateTime dateEnd=dateStart.AddMonths(1).AddDays(-1);//Will always return the last day of the month.
			//Refresh the schedule that we created above so that the non db columns are filled correctly.
			List<Schedule> listSchedules=Schedules.GetByScheduleNum(new List<long>() {
				schedDoc.ScheduleNum,
			});
			//Create two appointments that take up the majority of the operatory except the last two hour blocks.
			//This is the specific scenario that the eServices team setup in order to duplicate the issue.
			//390 min long appointment from 08:00 - 14:30
			AppointmentT.CreateAppointment(pat.PatNum,new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,8,0,0)
				,opHyg.OperatoryNum,provNumHyg,pattern: "//////////////////////////////////////////////////////////////////////////////");
			//150 min long appointment from 14:30 - 17:00
			AppointmentT.CreateAppointment(pat.PatNum,new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,14,30,0)
				,opHyg.OperatoryNum,provNumHyg,pattern: "//////////////////////////////");
			//Search for hour long time slot openings.
			List<TimeSlot> listTimeSlots=TimeSlots.GetTimeSlotsForRange(dateStart,dateEnd
				,"/XXXXXXXXXX/"//60 min appt
				,new List<long>() { provNumHyg }
				,new List<Operatory>() { opHyg }
				,listSchedules
				,null);//Null clinic will only consider ops with ClinicNum set to 0.
			//The time slots returned will depend on what time of day this unit test is actually ran.
			if(dateTimeSchedule.Hour < 17) {
				Assert.AreEqual(2,listTimeSlots.Count);
				//There should only be two time slots available and it is the last two hours of the schedule; 17:00 - 18:00 and 18:00 - 19:00.
				Assert.IsTrue(listTimeSlots[0].DateTimeStart==new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,17,0,0)
					&& listTimeSlots[0].DateTimeStop==new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,18,0,0));
				Assert.IsTrue(listTimeSlots[1].DateTimeStart==new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,18,0,0)
					&& listTimeSlots[1].DateTimeStop==new DateTime(dateTimeSchedule.Year,dateTimeSchedule.Month,dateTimeSchedule.Day,19,0,0));
			}
			else if(dateTimeSchedule.Hour < 18) {
				//Only one time slot will be available because the engineer is working too late and needs to go home to their family.
				Assert.AreEqual(1,listTimeSlots.Count);
				//It doesn't matter what time this slot is available, just the fact that only one is available is good enough.
			}
			else {
				//There won't be any time slots for today available.  This unit test is kind of pointless to run so late in the day.
				Assert.AreEqual(0,listTimeSlots.Count);
			}
		}

	}
}
