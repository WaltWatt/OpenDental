﻿using CodeBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness.WebTypes.WebSched.TimeSlot {
	public class TimeSlots {

		///<summary>Gets up to 30 days of open time slots based on the recall passed in.
		///Open time slots are found by looping through operatories flagged for Web Sched and finding openings that can hold the recall.
		///The amount of time required to be considered "available" is dictated by the RecallType associated to the recall passed in.
		///Throws exceptions.</summary>
		public static List<TimeSlot> GetAvailableWebSchedTimeSlots(long recallNum,DateTime dateStart,DateTime dateEnd,long provNum=0,
			bool allowOtherProv=true) 
		{
			//No need to check RemotingRole; no call to db.
			Clinic clinic=Clinics.GetClinicForRecall(recallNum);
			Recall recall=Recalls.GetRecall(recallNum);
			if(recall==null) {
				throw new ODException(Lans.g("WebSched","The recall appointment you are trying to schedule is no longer available.")+"\r\n"
					+Lans.g("WebSched","Please call us to schedule your appointment."));
			}
			List<Provider> listProviders=Providers.GetProvidersForWebSched(recall.PatNum);
			if(provNum > 0 && !allowOtherProv) {
				listProviders=listProviders.FindAll(x => x.ProvNum==provNum);
			}
			RecallType recallType=RecallTypes.GetFirstOrDefault(x => x.RecallTypeNum==recall.RecallTypeNum);
			return GetAvailableWebSchedTimeSlots(recallType,listProviders,clinic,dateStart,dateEnd,recall);
		}

		///<summary>Gets up to 30 days of open time slots based on the RecallType passed in.
		///Open time slots are found by looping through operatories flagged for Web Sched and finding openings that can hold the RecallType.
		///The RecallType passed in must be a valid recall type.
		///Providers passed in will be the only providers considered when looking for available time slots.
		///Passing in a null clinic will only consider operatories with clinics set to 0 (unassigned).
		///The timeslots on and between the Start and End dates passed in will be considered and potentially returned as available.
		///Optionally pass in a recall object in order to consider all other recalls due for the patient.  This will potentially affect the time pattern.
		///Throws exceptions.</summary>
		public static List<TimeSlot> GetAvailableWebSchedTimeSlots(RecallType recallType,List<Provider> listProviders,Clinic clinic
			,DateTime dateStart,DateTime dateEnd,Recall recallCur=null) 
		{
			//No need to check RemotingRole; no call to db.
			if(recallType==null) {//Validate that recallType is not null.
				throw new ODException(Lans.g("WebSched","The recall appointment you are trying to schedule is no longer available.")+"\r\n"
					+Lans.g("WebSched","Please call us to schedule your appointment."));
			}
			//Get all the Operatories that are flagged for Web Sched.
			List<Operatory> listOperatories=Operatories.GetOpsForWebSched();
			if(listOperatories.Count < 1) {//This is very possible for offices that aren't set up the way that we expect them to be.
				throw new ODException(Lans.g("WebSched","There are no operatories set up for Web Sched.")+"\r\n"
					+Lans.g("WebSched","Please call us to schedule your appointment."),ODException.ErrorCodes.NoOperatoriesSetup);
			}
			List<long> listProvNums=listProviders.Select(x => x.ProvNum).Distinct().ToList();
			List<Schedule> listSchedules=Schedules.GetSchedulesAndBlockoutsForWebSched(listProvNums,dateStart,dateEnd,true
				,(clinic==null) ? 0 : clinic.ClinicNum);
			string timePatternRecall=recallType.TimePattern;
			//Apparently scheduling this one recall can potentially schedule a bunch of other recalls at the same time.
			//We need to potentially bloat our time pattern based on the other recalls that are due for this specific patient.
			if(recallCur!=null) {
				Patient patCur=Patients.GetLim(recallCur.PatNum);
				List<Recall> listRecalls=Recalls.GetList(recallCur.PatNum);
				timePatternRecall=Recalls.GetRecallTimePattern(recallCur,listRecalls,patCur,new List<string>());
			}
			string timePatternAppointment=RecallTypes.ConvertTimePattern(timePatternRecall);
			return GetTimeSlotsForRange(dateStart,dateEnd,timePatternAppointment,listProvNums,listOperatories,listSchedules,clinic);
		}

		///<summary>Gets up to 30 days of open time slots for New Patient Appointments based on the timePattern passed in.
		///Open time slots are found by looping through the passed in operatories and finding openings that can hold the entire appointment.
		///Passing in a clinicNum of 0 will only consider unassigned operatories.
		///The timeslots on and between the Start and End dates passed in will be considered and potentially returned as available.
		///Optionally pass in an appt type def num which will only consider operatories with the corresponding appointment type.
		///defNumApptType is required and will ONLY consider operatories that are associated to the def's corresponding appointment type.
		///The time pattern and procedures on the appointment will be determined via the appointment type as well.
		///Throws exceptions.</summary>
		public static List<TimeSlot> GetAvailableNewPatApptTimeSlots(DateTime dateStart,DateTime dateEnd,long clinicNum,long defNumApptType) {
			//No need to check RemotingRole; no call to db.
			//Get the appointment type that is associated to the def passed in.  This is required for New Pat Appts.
			AppointmentType appointmentType=AppointmentTypes.GetWebSchedNewPatApptTypeByDef(defNumApptType);
			if(appointmentType==null) {
				//This message will typically show to a patient and we want them to call in OR to refresh the web app which should no longer show the reason.
				throw new ODException(Lans.g("WebSched","The reason for your appointment is no longer available.")+"\r\n"
					+Lans.g("WebSched","Please call us to schedule your appointment."));
			}
			//Now we need to find all operatories that are associated to the aforementioned appointment type.
			List<Operatory> listOperatories=Operatories.GetOpsForWebSchedNewPatApptDef(defNumApptType);
			if(listOperatories.Count < 1) {//This is very possible for offices that aren't set up the way that we expect them to be.
				return new List<TimeSlot>();//Don't throw an exception here to the patient, they can just select another reason.
			}
			//Set the timePattern from the appointment type passed in.
			string timePattern=AppointmentTypes.GetTimePatternForAppointmentType(appointmentType);
			List<Provider> listProviders=Providers.GetProvidersForWebSchedNewPatAppt();
			Clinic clinic=Clinics.GetClinic(clinicNum);
			List<long> listProvNums=listProviders.Select(x => x.ProvNum).Distinct().ToList();
			List<Schedule> listSchedules=Schedules.GetSchedulesAndBlockoutsForWebSched(listProvNums,dateStart,dateEnd,false,clinicNum);
			return GetTimeSlotsForRange(dateStart,dateEnd,timePattern,listProvNums,listOperatories,listSchedules,clinic,defNumApptType);
		}

		///<summary>Gets open time slots based on the parameters passed in.
		///Open time slots are found by looping through the passed in operatories and finding openings that can hold the entire appointment.
		///Make sure that timePattern is always passed in utilizing 5 minute increments (no conversion will be applied to the pattern passed in).
		///Providers passed in will be the only providers considered when looking for available time slots.
		///Passing in a null clinic will only consider operatories with clinics set to 0 (unassigned).
		///The timeslots on and between the Start and End dates passed in will be considered and potentially returned as available.
		///Optionally set defNumApptType if looking for time slots for New Pat Appt which will apply the DefNum to all time slots found.
		///Throws exceptions.</summary>
		public static List<TimeSlot> GetTimeSlotsForRange(DateTime dateStart,DateTime dateEnd,string timePattern,List<long> listProvNums
			,List<Operatory> listOperatories,List<Schedule> listSchedules,Clinic clinic,long defNumApptType=0)
		{
			//No need to check RemotingRole; no call to db.
			//Order the operatories passed in by their ItemOrder just in case they were passed in all jumbled up.
			List<long> listOpNums=listOperatories.OrderBy(x => x.ItemOrder).Select(x => x.OperatoryNum).Distinct().ToList();
			//Remove all schedules that fall outside of the date range passed in.  Only consider the date right now, the time portion is handled later.
			listSchedules.RemoveAll(x => !x.SchedDate.Date.Between(dateStart.Date,dateEnd.Date));
			List<Schedule> listProviderSchedules=listSchedules.FindAll(x => x.BlockoutType==0);
			List<Schedule> listBlockoutSchedules=listSchedules.FindAll(x => x.BlockoutType > 0);
			//Get every single appointment for all operatories within our start and end dates for double booking and overlapping consideration.
			List<Appointment> listApptsForOps=Appointments.GetAppointmentsForOpsByPeriod(Operatories.GetDeepCopy(true).Select(x => x.OperatoryNum).ToList()
				,dateStart,dateEnd);
			//We need to be conscious of double booking possibilities.  Go get provider schedule information for the date range passed in.
			Dictionary<DateTime,List<ApptSearchProviderSchedule>> dictProvSchedules=Appointments.GetApptSearchProviderScheduleForProvidersAndDate(
				listProvNums,dateStart,dateEnd,listProviderSchedules,listApptsForOps);
			//Split up the operatory specific provider schedules from the dynamic ones because each will have different operatory logic.
			List<Schedule> listProviderSchedulesWithOp=listProviderSchedules.FindAll(x => x.Ops.Intersect(listOpNums).ToList().Count > 0);
			List<ScheduleOp> listScheduleOps=ScheduleOps.GetForSchedList(listProviderSchedules);
			//Now we need to get the dynamic schedules (not assigned to a specific operatory).
			List<Schedule> listProviderDynamicSchedules=listProviderSchedules.FindAll(x => !listScheduleOps.Exists(y => y.ScheduleNum==x.ScheduleNum));
			//Now that we have found all possible valid schedules, find all the unique time slots from them.
			List<Schedule> listProviderSchedulesAll=new List<Schedule>(listProviderSchedulesWithOp);
			listProviderSchedulesAll.AddRange(listProviderDynamicSchedules);
			listProviderSchedulesAll=listProviderSchedulesAll.OrderBy(x => x.SchedDate).ToList();
			List<TimeSlot> listAvailableTimeSlots=new List<TimeSlot>();
			List<DateTime> listUniqueDays=new List<DateTime>();
			int timeIncrement=PrefC.GetInt(PrefName.AppointmentTimeIncrement);
			//Loop through all schedules five minutes at a time to find time slots large enough that have no appointments and no blockouts within them.
			foreach(Schedule schedule in listProviderSchedulesAll) {
				DateTime dateSched=schedule.SchedDate;
				//Straight up ignore schedules in the past.  This should not be possible but this is just in case.
				if(dateSched.Date < DateTime.Today) {
					continue;
				}
				if(!listUniqueDays.Contains(dateSched)) {
					listUniqueDays.Add(dateSched);
				}
				TimeSpan timeSchedStart=schedule.StartTime;
				TimeSpan timeSchedStop=schedule.StopTime;
				//Now, make sure that the start time is set to a starting time that makes sense with the appointment time increment preference.
				int minsOver=(timeSchedStart.Minutes)%timeIncrement;
				if(minsOver>0) {
					int minsToAdd=timeIncrement-minsOver;
					timeSchedStart=timeSchedStart.Add(new TimeSpan(0,minsToAdd,0));
				}
				//Double check that we haven't pushed the start time past the stop time.
				if(timeSchedStart>=timeSchedStop) {
					continue;
				}
				//Figure out all possible operatories for this particular schedule.
				List<Operatory> listOpsForSchedule=new List<Operatory>();
				if(schedule.Ops.Count > 0) {
					listOpsForSchedule=listOperatories.FindAll(x => schedule.Ops.Exists(y => y==x.OperatoryNum));
				}
				else {//Dynamic schedule.  Figure out what operatories this provider is part of that are associated to the corresponding eService.
					//Get all of the valid operatories that this provider is associated with.
					listOpsForSchedule=listOperatories.FindAll(x => x.ProvDentist==schedule.ProvNum || x.ProvHygienist==schedule.ProvNum);
				}
				if(PrefC.HasClinicsEnabled) {
					//Skip this schedule entry if the operatory's clinic does not match the patient's clinic.
					if(clinic==null) {
						//If a clinic was not passed in, ONLY consider unassigned operatories
						listOpsForSchedule=listOpsForSchedule.FindAll(x => x.ClinicNum==0);
					}
					else {
						//If a valid clinic was passed in, make sure the operatory has a matching clinic.
						listOpsForSchedule=listOpsForSchedule.FindAll(x => x.ClinicNum==clinic.ClinicNum);
					}
				}
				if(listOpsForSchedule.Count==0) {
					continue;//No valid operatories for this schedule.
				}
				//The list of operatories has been filtered above so we need to find ALL available time slots for this schedule in all operatories.
				foreach(Operatory op in listOpsForSchedule) {
					AddTimeSlotsFromSchedule(listAvailableTimeSlots,schedule,op.OperatoryNum,timeSchedStart,timeSchedStop
						,listBlockoutSchedules,dictProvSchedules,listApptsForOps,timePattern,defNumApptType);
				}
			}
			//Remove any time slots that start before right now (just in case the consuming method is looking for slots for today).
			listAvailableTimeSlots.RemoveAll(x => x.DateTimeStart.Date==DateTime.Now.Date && x.DateTimeStart.TimeOfDay < DateTime.Now.TimeOfDay);
			//Order the entire list of available time slots so that they are displayed to the user in sequential order.
			//We need to do this because we loop through each provider's schedule one at a time and add openings as we find them.
			//Then order by operatory.ItemOrder in order to preserve old behavior (filling up the schedule via operatories from the left to the right).
			return listAvailableTimeSlots.OrderBy(x => x.DateTimeStart)
				//listOpNums was ordered by ItemOrder at the top of this method so we can trust that it is in the correct order.
				.ThenBy(x => listOpNums.IndexOf(x.OperatoryNum))
				.ToList();
		}

		///<summary>Adds valid time slots to listAvailableTimeSlots if the time slot found does NOT already exist within the list.
		///This is a helper method to better break up the complexity of GetAvailableWebSchedTimeSlots() so that it is easier to follow.
		///Make sure that timePattern is always passed in utilizing 5 minute increments (no conversion will be applied to the pattern passed in).
		///Optionally set defNumApptType if looking for time slots for New Pat Appt which will apply the DefNum to all time slots found.</summary>
		private static void AddTimeSlotsFromSchedule(List<TimeSlot> listAvailableTimeSlots,Schedule schedule,long operatoryNum
			,TimeSpan timeSchedStart,TimeSpan timeSchedStop,List<Schedule> listBlockouts
			,Dictionary<DateTime,List<ApptSearchProviderSchedule>> dictProvSchedules,List<Appointment> listApptsForOps,string timePattern
			,long defNumApptType=0)
		{
			//No need to check RemotingRole; no call to db and this is a private method.
			//Figure out how large of a time slot we need to find in order to consider this time slot "available".
			int apptLengthMins=timePattern.Length * 5;
			int timeIncrement=PrefC.GetInt(PrefName.AppointmentTimeIncrement);
			DateTime dateSched=schedule.SchedDate;
			//Start going through this operatories schedule according to the time increment, looking for a gap that can handle apptLengthMins.
			TimeSpan timeSlotStart=new TimeSpan(timeSchedStart.Ticks);
			//Start looking for collisions AFTER the start time.
			//Stop as soon as the slots stop time meets or passes the sched stop time.
			//Iterate through the schedule via the time increment preference.
			for(TimeSpan timeSlotStop=timeSchedStart.Add(new TimeSpan(0,timeIncrement,0))
				;timeSlotStop<=timeSchedStop
				;timeSlotStop=timeSlotStop.Add(new TimeSpan(0,timeIncrement,0))) 
			{
				//Check to see if we've found an opening.
				TimeSpan timeSpanCur=timeSlotStop-timeSlotStart;
				//Check to see if there is an appointment or a blockout that collides with this blockout.
				bool isOverlapping=false;
				//First we'll look at blockouts because it should be quicker than looking at the appointments
				foreach(Schedule blockout in listBlockouts) {
					if(dateSched.Date!=blockout.SchedDate) {
						continue;//Blockout is not on the same day that we are looking at.
					}
					if(!blockout.Ops.Exists(x => x==operatoryNum)) {
						continue;//Blockout is not present in this operatory.
					}
					//Same operatory and day, check if the times overlap.
					//Create new TimeSpans in order to remove the date portion from the blockouts.
					TimeSpan timeBlockoutStart=new TimeSpan(blockout.StartTime.Hours,blockout.StartTime.Minutes,0);
					TimeSpan timeBlockoutStop=new TimeSpan(blockout.StopTime.Hours,blockout.StopTime.Minutes,0);
					if(IsTimeOverlapping(timeSlotStart,timeSlotStop,timeBlockoutStart,timeBlockoutStop)) {
						isOverlapping=true;
						break;
					}
				}
				if(isOverlapping) {//This check is here so that we don't waste time looping through appointments if we don't need to.
					//There was a collision, set the time slot start time to the stop time and continue from there.
					timeSlotStart=timeSlotStop;
					continue;
				}
				//Next we'll look for overlapping appointments
				foreach(Appointment appointment in listApptsForOps) {
					if(appointment.Op!=operatoryNum) {
						continue;
					}
					if(dateSched.Date!=appointment.AptDateTime.Date) {
						continue;//Appt is not on the same day that we are looking at.
					}
					//Same operatory and day, check if the times overlap.
					TimeSpan timeApptStart=appointment.AptDateTime.TimeOfDay;
					TimeSpan timeApptStop=appointment.AptDateTime.AddMinutes(appointment.Pattern.Length*5).TimeOfDay;
					if(IsTimeOverlapping(timeSlotStart,timeSlotStop,timeApptStart,timeApptStop)) {
						isOverlapping=true;
						break;
					}
				}
				if(isOverlapping) {
					//There was a collision, set the time slot start time to the stop time and continue from there.
					timeSlotStart=timeSlotStop;
					continue;
				}
				if(timeSpanCur.TotalMinutes>=apptLengthMins) {
					//We just found an opening.  Make sure we don't already have this time slot available.
					DateTime dateTimeSlotStart=new DateTime(dateSched.Year,dateSched.Month,dateSched.Day,timeSlotStart.Hours,timeSlotStart.Minutes,0);
					DateTime dateTimeSlotStop=new DateTime(dateSched.Year,dateSched.Month,dateSched.Day,timeSlotStop.Hours,timeSlotStop.Minutes,0);
					TimeSlot timeSlot=new TimeSlot(dateTimeSlotStart,dateTimeSlotStop,operatoryNum,schedule.ProvNum,defNumApptType);
					if(!listAvailableTimeSlots.Any(x => (x.DateTimeStart==dateTimeSlotStart && x.DateTimeStop==dateTimeSlotStop 
						&& x.ProvNum==schedule.ProvNum))) //We will return multiple time slots for the same time for different providers.
					{
						//This time slot is not already in our list of available time slots, check for double booking.
						if(dictProvSchedules.ContainsKey(dateSched.Date)) {
							long recallProvNum=schedule.ProvNum;
							if(IsApptPatternDoubleBooked(dictProvSchedules[dateSched.Date],recallProvNum,timePattern,dateTimeSlotStart)) {
								//There is a double booking conflict.  Do not add this time slot as a possibility.
								//However, at this point we know that there are no appointment conflicts for the current time slot, only a double booking conflict.
								//The appointment needs to scoot within the operatory to hopefully find the first available opening (unit test 86).
								timeSlotStart=timeSlotStart.Add(new TimeSpan(0,timeIncrement,0));
								continue;
							}
						}
						//There are no collisions with this provider's schedule, add it to our list of available time slots.
						listAvailableTimeSlots.Add(timeSlot);
					}
					else {
						//We have found a time slot in another operatory that matches the necessary criteria.
						//Check to see if this operatory should be considered before the previously found operatory.
						TimeSlot timeSlotCur=listAvailableTimeSlots.First(x => (x.DateTimeStart==dateTimeSlotStart && x.DateTimeStop==dateTimeSlotStop));
						Operatory operatoryIn=Operatories.GetOperatory(operatoryNum);
						Operatory operatoryCur=Operatories.GetOperatory(timeSlotCur.OperatoryNum);
						if(operatoryIn.ItemOrder < operatoryCur.ItemOrder) {
							timeSlotCur.OperatoryNum=operatoryIn.OperatoryNum;
						}
					}
					//Continue looking for more open slots starting at the end of this time slot.
					//E.g. we just found 9:30 AM to 10:00 AM.  We need to continue from 10:00 AM.
					timeSlotStart=timeSlotStop;
					continue;
				}
			}
		}

		///<summary>Checks to see if the provider has any double booking issues with the appointment time pattern passed in.
		///Logic in this method ignores HYG conflicts purposefully for Web Sched. timePattern must be a time pattern in 5 minute increments.</summary>
		private static bool IsApptPatternDoubleBooked(List<ApptSearchProviderSchedule> listProviderSchedules,long provNum,string timePattern
			,DateTime dateTimeAppointmentStart) 
		{
			//No need to check RemotingRole; no call to db and this is a private method.
			List<ApptSearchProviderSchedule> listProviderSchedulesForProv=listProviderSchedules.FindAll(x => x.ProviderNum==provNum);
			//Figure out what 5 min increment the dateTimeAppointmentStart passed in starts on.
			int startingIncrement=(int)dateTimeAppointmentStart.TimeOfDay.TotalMinutes/5;
			for(int i=0;i<listProviderSchedulesForProv.Count;i++) {//There should only be one.
				//Check to make sure the ProvBar does not have any conflicts with the timePattern passed in.
				List<bool> listHasDouble=listProviderSchedulesForProv[i].ProvBar.ToList().FindAll(x => !x);
				for(int j=0;j<timePattern.Length;j++) {
					if(timePattern[j]=='/') {//Don't worry about HYG conflicts.
						continue;
					}
					if(!listProviderSchedulesForProv[i].ProvBar[startingIncrement+j]) {//False means there is a collision in the providers schedule.
						return true;
					}
				}
			}
			return false;//No double booking collision.
		}

		///<summary>Checks if the two times passed in overlap.</summary>
		private static bool IsTimeOverlapping(TimeSpan timeStartBegin,TimeSpan timeStartEnd,TimeSpan timeStopBegin,TimeSpan timeStopEnd) {
			//No need to check RemotingRole; no call to db and this is a private method.
			//Test start times
			if(timeStartBegin >= timeStopBegin && timeStartBegin < timeStopEnd) {
				return true;
			}
			//Test end times
			if(timeStartEnd > timeStopBegin && timeStartEnd <= timeStopEnd) {
				return true;
			}
			//Test engulf
			if(timeStartBegin <= timeStopBegin && timeStartEnd >= timeStopEnd) {
				return true;
			}
			return false;
		}
	}
}
