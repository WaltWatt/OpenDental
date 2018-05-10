using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDentBusiness.HL7;
using OpenDentBusiness.UI;
using CodeBase;
using System.Linq;

namespace OpenDental{
	public class AppointmentL {
		///<summary>The date currently selected in the appointment module.</summary>
		public static DateTime DateSelected;

		///<summary></summary>
		public static List<DateTime> GetSearchResults(long aptNum,DateTime afterDate,List<long> providerNums,int resultCount,TimeSpan beforeTime,TimeSpan afterTime) {
			if(beforeTime==TimeSpan.FromSeconds(0)) {//if they didn't set a before time, set it to a large timespan so that we can use the same logic for checking appointment times.
				beforeTime=TimeSpan.FromHours(25);//bigger than any time of day.
			}
			SearchBehaviorCriteria SearchType = (SearchBehaviorCriteria)PrefC.GetInt(PrefName.AppointmentSearchBehavior);
			List<DateTime> retVal= new List<DateTime>();
			DateTime dayEvaluating=afterDate.AddDays(1);
			Appointment appointmentToAdd=Appointments.GetOneApt(aptNum);
			List<DateTime> potentialProvAppointmentTime;
			List<DateTime> potentialOpAppointmentTime; 
			List<Operatory> opsListAll = Operatories.GetDeepCopy();//all operatory Numbers
			List<Schedule> scheduleListAll = Schedules.GetTwoYearPeriod(dayEvaluating);// Schedules for the given day.
			List<Appointment> appointmentListAll = Appointments.GetForPeriodList(dayEvaluating,dayEvaluating.AddYears(2));
			List<ScheduleOp> schedOpListAll = ScheduleOps.GetForSchedList(scheduleListAll);
			List<ApptSearchProviderSchedule> provScheds = new List<ApptSearchProviderSchedule>();//Provider Bar, ProviderSched Bar, Date and Provider
			List<ApptSearchOperatorySchedule> operatrorySchedules = new List<ApptSearchOperatorySchedule>();//filtered based on SearchType
			List<long> operatoryNums = new List<long>();//more usefull than a list of operatories.
			for(int i=0;i<opsListAll.Count;i++) {
				operatoryNums.Add(opsListAll[i].OperatoryNum);
			}
			while(retVal.Count < resultCount && dayEvaluating < afterDate.AddYears(2)) {
				potentialOpAppointmentTime = new List<DateTime>();//clear or create
				//Providers-------------------------------------------------------------------------------------------------------------------------------------
				potentialProvAppointmentTime = new List<DateTime>();//clear or create
				provScheds = Appointments.GetApptSearchProviderScheduleForProvidersAndDate(providerNums,dayEvaluating,scheduleListAll,appointmentListAll);
				for(int i=0;i<provScheds.Count;i++) {
					for(int j=0;j<288;j++) {//search every 5 minute increment per day
						if(j+appointmentToAdd.Pattern.Length>288) {
							break;
						}
						if(potentialProvAppointmentTime.Contains(dayEvaluating.AddMinutes(j*5))) {
							continue;
						}
						bool addDateTime=true;
						for(int k=0;k<appointmentToAdd.Pattern.Length;k++) {
							if((provScheds[i].ProvBar[j+k]==false && appointmentToAdd.Pattern[k]=='X') || provScheds[i].ProvSchedule[j+k]==false) {
								addDateTime=false;
								break;
							}
						}
						if(addDateTime) {
							potentialProvAppointmentTime.Add(dayEvaluating.AddMinutes(j*5));
						}
					}
				}
				if(SearchType==SearchBehaviorCriteria.ProviderTimeOperatory) {//Handle Operatories here----------------------------------------------------------------------------
					operatrorySchedules = GetAllForDate(dayEvaluating,scheduleListAll,appointmentListAll,schedOpListAll,operatoryNums,providerNums);
					potentialOpAppointmentTime = new List<DateTime>();//create or clear
					//for(int j=0;j<operatrorySchedules.Count;j++) {//for each operatory 
					for(int i=0;i<288;i++) {//search every 5 minute increment per day
						if(i+appointmentToAdd.Pattern.Length>288) {//skip if appointment would span across midnight
							break;
						}
						for(int j=0;j<operatrorySchedules.Count;j++) {//for each operatory 
							//if(potentialOpAppointmentTime.Contains(dayEvaluating.AddMinutes(i*5))) {//skip if we already have this dateTime
							//  break;
							//}
							bool addDateTime=true;
							for(int k=0;k<appointmentToAdd.Pattern.Length;k++) {//check appointment against operatories
								if(operatrorySchedules[j].OperatorySched[i+k]==false) {
									addDateTime=false;
									break;
								}
							}
							if(!addDateTime){
								continue;
							}
							if(addDateTime){// && SearchType==SearchBehaviorCriteria.ProviderTimeOperatory) {//check appointment against providers available for the given operatory
								bool provAvail=false;
								for(int k=0;k<providerNums.Count;k++) {
									if(!operatrorySchedules[j].ProviderNums.Contains(providerNums[k])) {
										continue;
									}
									provAvail=true;
									for(int m=0;m<appointmentToAdd.Pattern.Length;m++) {
										if((provScheds[k].ProvBar[i+m]==false && appointmentToAdd.Pattern[m]=='X') || provScheds[k].ProvSchedule[i+m]==false) {//if provider bar time slot
											provAvail=false;
											break;
										}
									}
									if(provAvail) {//found a provider with an available operatory
										break;
									}
								}
								if(provAvail && addDateTime) {//operatory and provider are available
									potentialOpAppointmentTime.Add(dayEvaluating.AddMinutes(i*5));
								}
							}
							else {//not using SearchBehaviorCriteria.ProviderTimeOperatory
								if(addDateTime) {
									potentialOpAppointmentTime.Add(dayEvaluating.AddMinutes(i*5));
								}
							}
						}
					}
				}
				//At this point the potentialOpAppointmentTime is already filtered and only contains appointment times that match both provider time and operatory time. 
				switch(SearchType) {
					case SearchBehaviorCriteria.ProviderTime:
						//Add based on provider bars
						for(int i=0;i<potentialProvAppointmentTime.Count;i++) {
							if(potentialProvAppointmentTime[i].TimeOfDay>beforeTime || potentialProvAppointmentTime[i].TimeOfDay<afterTime) {
								continue;
							}
							retVal.Add(potentialProvAppointmentTime[i]);//add one for this day
							break;//stop looking through potential times for today.
						}
						break;
					case SearchBehaviorCriteria.ProviderTimeOperatory:
						//add based on provider bar and operatory bar
						for(int i=0;i<potentialOpAppointmentTime.Count;i++) {
							if(potentialOpAppointmentTime[i].TimeOfDay>beforeTime || potentialOpAppointmentTime[i].TimeOfDay<afterTime) {
								continue;
							}
							retVal.Add(potentialOpAppointmentTime[i]);//add one for this day
							break;//stop looking through potential times for today.
						}
						break;
				}
				dayEvaluating=dayEvaluating.AddDays(1);
			}
			return retVal;
		}

		/// <summary>Uses Inputs to construct a List&lt;ApptSearchOperatorySchedule&gt;. It is written to reduce the number of queries to the database.</summary>
		private static List<ApptSearchOperatorySchedule> GetAllForDate(DateTime ScheduleDate,List<Schedule> ScheduleList,List<Appointment> AppointmentList,List<ScheduleOp> ScheduleOpList,List<long> OperatoryNums,List<long> ProviderNums) {
			List<ApptSearchOperatorySchedule> retVal = new List<ApptSearchOperatorySchedule>();
			List<ApptSearchOperatorySchedule> opSchedListAll = new List<ApptSearchOperatorySchedule>();
			List<Operatory> opsListAll = Operatories.GetDeepCopy();
			opsListAll.Sort(compareOpsByOpNum);//sort by Operatory Num Ascending
			OperatoryNums.Sort();//Sort by operatory Num Ascending to match
			List<List<long>> opsProvPerSchedules = new List<List<long>>();//opsProvPerSchedules[<opIndex>][ProviderNums] based solely on schedules, lists of providers 'allowed' to work in the given operatory
			List<List<long>> opsProvPerOperatories = new List<List<long>>();//opsProvPerSchedules[<opIndex>][ProviderNums] based solely on operatories, lists of providers 'allowed' to work in the given operatory
			List<List<long>> opsProvIntersect = new List<List<long>>();////opsProvPerSchedules[<opIndex>][ProviderNums] based on the intersection of the two data sets above.
			ScheduleDate=ScheduleDate.Date;//remove time component
			for(int i=0;i<OperatoryNums.Count;i++) {
				opSchedListAll.Add(new ApptSearchOperatorySchedule());
				opSchedListAll[i].SchedDate=ScheduleDate;
				opSchedListAll[i].ProviderNums=new List<long>();
				opSchedListAll[i].OperatoryNum=OperatoryNums[i];
				opSchedListAll[i].OperatorySched=new bool[288];
				for(int j=0;j<288;j++) {
					opSchedListAll[i].OperatorySched[j]=true;//Set entire operatory schedule to true. True=available.
				}
				opsProvPerSchedules.Add(new List<long>());
				opsProvPerOperatories.Add(new List<long>());
				opsProvIntersect.Add(new List<long>());
			}
			#region fillOpSchedListAll.ProviderNums
			for(int i=0;i<ScheduleList.Count;i++) {//use this loop to fill opsProvPerSchedules
				if(ScheduleList[i].SchedDate.Date!=ScheduleDate) {//only schedules for the applicable day.
					continue;
				}
				int schedopsforschedule=0;
				for(int j=0;j<ScheduleOpList.Count;j++) {
					if(ScheduleOpList[j].ScheduleNum!=ScheduleList[i].ScheduleNum) {//ScheduleOp does not apply to this schedule
						continue;
					}
					schedopsforschedule++;
					int indexofop = OperatoryNums.IndexOf(ScheduleOpList[j].OperatoryNum);//cache to increase speed
					if(opsProvPerSchedules[indexofop].Contains(ScheduleList[i].ProvNum)) {//only add ones that have not been added.
						continue;
					}
					opsProvPerSchedules[indexofop].Add(ScheduleList[i].ProvNum);
				}
				if(schedopsforschedule==0) {//Provider is scheduled to work, but not limited to any specific operatory so add provider num to all operatories in opsProvPerSchedules
					for(int k=0;k<opsProvPerSchedules.Count;k++) {
						if(opsProvPerSchedules[k].Contains(ScheduleList[i].ProvNum)) {
							continue;
						}
						opsProvPerSchedules[k].Add(ScheduleList[i].ProvNum);
					}
				}
			}
			for(int i=0;i<opsListAll.Count;i++) {//use this loop to fill opsProvPerOperatories
				opsProvPerOperatories[i].Add(opsListAll[i].ProvDentist);
				opsProvPerOperatories[i].Add(opsListAll[i].ProvHygienist);
			}
			for(int i=0;i<opsProvPerSchedules.Count;i++) {//Use this loop to fill opsProvIntersect by finding matching pairs in opsProvPerSchedules and opsProvPerOperatories
				for(int j=0;j<opsProvPerSchedules[i].Count;j++) {
					if(opsProvPerOperatories[i][0]==0 && opsProvPerOperatories[i][1]==0) {//There are no providers set for this operatory, use all the provider nums from the schedules.
						opsProvIntersect[i].Add(opsProvPerSchedules[i][j]);
						opSchedListAll[i].ProviderNums.Add(opsProvPerSchedules[i][j]);
						continue;
					}
					if(opsProvPerSchedules[i][j]==0) {
						continue;//just in case a non valid prov num got through.
					}
					if(opsProvPerOperatories[i].Contains(opsProvPerSchedules[i][j])) {//if a provider was assigned and matches
						opsProvIntersect[i].Add(opsProvPerSchedules[i][j]);
						opSchedListAll[i].ProviderNums.Add(opsProvPerSchedules[i][j]);
					}
				}
			}
			#endregion fillOpSchedListAll.ProviderNums
			for(int i=0;i<AppointmentList.Count;i++) {//use this loop to set all operatory schedules.
				if(AppointmentList[i].AptDateTime.Date!=ScheduleDate) {//skip appointments that do not apply to this date
					continue;
				}
				if(AppointmentList[i].Op==0) {//If the appointment isn't associated to an Op, it isn't on the schedule and won't interfere with available timeslots.
					continue;
				}
				int indexofop = OperatoryNums.IndexOf(AppointmentList[i].Op);
				int aptstartindex= (int)AppointmentList[i].AptDateTime.TimeOfDay.TotalMinutes/5;
				for(int j=0;j<AppointmentList[i].Pattern.Length;j++) {//make unavailable all blocks of time during this appointment.
					opSchedListAll[indexofop].OperatorySched[aptstartindex+j]=false;//Set time block to false, meaning something is scheduled here.
				}
			}
			for(int i=0;i<opSchedListAll.Count;i++) {//Filter out operatory schedules for ops that our selected providers don't work in.
				if(retVal.Contains(opSchedListAll[i])) {
					continue;
				}
				for(int j=0;j<opSchedListAll[i].ProviderNums.Count;j++) {
					if(ProviderNums.Contains(opSchedListAll[i].ProviderNums[j])) {
						retVal.Add(opSchedListAll[i]);
						break;
					}
				}
			}
			//For Future Use When adding third search behavior:
			//if((SearchBehaviorCriteria)PrefC.GetInt(PrefName.AppointmentSearchBehavior)==SearchBehaviorCriteria.OperatoryOnly) {
			//  return opSchedListAll;
			//}
			return retVal;
		}

		private static int compareOpsByOpNum(Operatory op1,Operatory op2) {
			return (int)op1.OperatoryNum-(int)op2.OperatoryNum;
		}

		/*
		///<summary>Only used in GetSearchResults.  All times between start and stop get set to true in provBarSched.</summary>
		private static void SetProvBarSched(ref bool[] provBarSched,TimeSpan timeStart,TimeSpan timeStop){
			int startI=GetProvBarIndex(timeStart);
			int stopI=GetProvBarIndex(timeStop);
			for(int i=startI;i<=stopI;i++){
				provBarSched[i]=true;
			}
		}

		private static int GetProvBarIndex(TimeSpan time) {
			return (int)(((double)time.Hours*(double)60/(double)PrefC.GetLong(PrefName.AppointmentTimeIncrement)//aptTimeIncr=minutesPerIncr
				+(double)time.Minutes/(double)PrefC.GetLong(PrefName.AppointmentTimeIncrement))
				*(double)ApptDrawing.LineH*ApptDrawing.RowsPerIncr)
				/ApptDrawing.LineH;//rounds down
		}*/

		///<summary>Used by UI when it needs a recall appointment placed on the pinboard ready to schedule.  This method creates the appointment and attaches all appropriate procedures.  It's up to the calling class to then place the appointment on the pinboard.  If the appointment doesn't get scheduled, it's important to delete it.  If a recallNum is not 0 or -1, then it will create an appt of that recalltype.</summary>
		public static Appointment CreateRecallApt(Patient patCur,List<InsPlan> planList,long recallNum,List<InsSub> subList
			,DateTime aptDateTime=default(DateTime))
		{
			List<Recall> recallList=Recalls.GetList(patCur.PatNum);
			Recall recallCur=null;
			if(recallNum>0) {
				recallCur=Recalls.GetRecall(recallNum);
			}
			else{
				for(int i=0;i<recallList.Count;i++){
					if(recallList[i].RecallTypeNum==RecallTypes.PerioType || recallList[i].RecallTypeNum==RecallTypes.ProphyType){
						if(!recallList[i].IsDisabled){
							recallCur=recallList[i];
						}
						break;
					}
				}
			}
			if(recallCur==null){
				//Typically never happens because everyone has a recall.  However, it can happen when patients have custom recalls due
				throw new ApplicationException(Lan.g("AppointmentL","No special type recall is due."));
			}
			if(recallCur.DateScheduled.Date>DateTime.Today) {
				throw new ApplicationException(Lan.g("AppointmentL","Recall has already been scheduled for ")+recallCur.DateScheduled.ToShortDateString());
			}
			Appointment aptCur=new Appointment();
			aptCur.AptDateTime=aptDateTime;		
			List<string> procs=RecallTypes.GetProcs(recallCur.RecallTypeNum);
			List<Procedure> listProcs=Appointments.FillAppointmentForRecall(aptCur,recallCur,recallList,patCur,procs,planList,subList);
			for(int i=0;i<listProcs.Count;i++) {
				if(Programs.UsingOrion) {
					FormProcEdit FormP=new FormProcEdit(listProcs[i],patCur.Copy(),Patients.GetFamily(patCur.PatNum));
					FormP.IsNew=true;
					FormP.ShowDialog();
					if(FormP.DialogResult==DialogResult.Cancel) {
						//any created claimprocs are automatically deleted from within procEdit window.
						try {
							Procedures.Delete(listProcs[i].ProcNum);//also deletes the claimprocs
						}
						catch(Exception ex) {
							MessageBox.Show(ex.Message);
						}
					}
					else {
						//Do not synch. Recalls based on ScheduleByDate reports in Orion mode.
						//Recalls.Synch(PatCur.PatNum);
					}
				}
			}
			return aptCur;
		}

		///<summary>Tests to see if this appointment will create a double booking. Returns arrayList with no items in it if no double bookings for this appt.  But if double booking, then it returns an arrayList of codes which would be double booked.  You must supply the appointment being scheduled as well as a list of all appointments for that day.  The list can include the appointment being tested if user is moving it to a different time on the same day.  The ProcsForOne list of procedures needs to contain the procedures for the apt becauese procsMultApts won't necessarily, especially if it's a planned appt on the pinboard.</summary>
		public static ArrayList GetDoubleBookedCodes(Appointment apt,DataTable dayTable,List<Procedure> procsMultApts,Procedure[] procsForOne) {
			ArrayList retVal=new ArrayList();//codes
			//figure out which provider we are testing for
			long provNum;
			if(apt.IsHygiene){
				provNum=apt.ProvHyg;
			}
			else{
				provNum=apt.ProvNum;
			}
			//compute the starting row of this appt
			int convertToY=(int)(((double)apt.AptDateTime.Hour*(double)60
				/(double)PrefC.GetLong(PrefName.AppointmentTimeIncrement)
				+(double)apt.AptDateTime.Minute
				/(double)PrefC.GetLong(PrefName.AppointmentTimeIncrement)
				)*(double)ApptDrawing.LineH*ApptDrawing.RowsPerIncr);
			int startIndex=convertToY/ApptDrawing.LineH;//rounds down
			string pattern=ApptSingleDrawing.GetPatternShowing(apt.Pattern);
			//keep track of which rows in the entire day would be occupied by provider time for this appt
			ArrayList aptProvTime=new ArrayList();
			for(int k=0;k<pattern.Length;k++){
				if(pattern.Substring(k,1)=="X"){
					aptProvTime.Add(startIndex+k);//even if it extends past midnight, we don't care
				}
			}
			//Now, loop through all the other appointments for the day, and see if any would overlap this one
			bool overlaps;
			Procedure[] procs;
			bool doubleBooked=false;//applies to all appts, not just one at a time.
			DateTime aptDateTime;
			for(int i=0;i<dayTable.Rows.Count;i++){
				if(dayTable.Rows[i]["AptNum"].ToString()==apt.AptNum.ToString()){//ignore current apt in its old location
					continue;
				}
				//ignore other providers
				if(dayTable.Rows[i]["IsHygiene"].ToString()=="1" && dayTable.Rows[i]["ProvHyg"].ToString()!=provNum.ToString()){
					continue;
				}
				if(dayTable.Rows[i]["IsHygiene"].ToString()=="0" && dayTable.Rows[i]["ProvNum"].ToString()!=provNum.ToString()){
					continue;
				}
				if(dayTable.Rows[i]["AptStatus"].ToString()==((int)ApptStatus.Broken).ToString()){//ignore broken appts
					continue;
				}
				aptDateTime=PIn.DateT(dayTable.Rows[i]["AptDateTime"].ToString());
				if(ApptDrawing.IsWeeklyView && aptDateTime.Date!=apt.AptDateTime.Date){//These appointments are on different days.
					continue;
				}
				//calculate starting row
				//this math is copied from another section of the program, so it's sloppy. Safer than trying to rewrite it:
				convertToY=(int)(((double)aptDateTime.Hour*(double)60
					/(double)PrefC.GetLong(PrefName.AppointmentTimeIncrement)
					+(double)aptDateTime.Minute
					/(double)PrefC.GetLong(PrefName.AppointmentTimeIncrement)
					)*(double)ApptDrawing.LineH*ApptDrawing.RowsPerIncr);
				startIndex=convertToY/ApptDrawing.LineH;//rounds down
				pattern=ApptSingleDrawing.GetPatternShowing(dayTable.Rows[i]["Pattern"].ToString());
				//now compare it to apt
				overlaps=false;
				for(int k=0;k<pattern.Length;k++){
					if(pattern.Substring(k,1)=="X"){
						if(aptProvTime.Contains(startIndex+k)){
							overlaps=true;
							doubleBooked=true;
						}
					}
				}
				if(overlaps){
					//we need to add all codes for this appt to retVal
					procs=Procedures.GetProcsOneApt(PIn.Long(dayTable.Rows[i]["AptNum"].ToString()),procsMultApts);
					for(int j=0;j<procs.Length;j++){
						retVal.Add(ProcedureCodes.GetStringProcCode(procs[j].CodeNum));
					}
				}
			}
			//now, retVal contains all double booked procs except for this appt
			//need to all procs for this appt.
			if(doubleBooked){
				for(int j=0;j<procsForOne.Length;j++) {
					retVal.Add(ProcedureCodes.GetStringProcCode(procsForOne[j].CodeNum));
				}
			}
			return retVal;
		}

		///<summary>Returns true if PrefName.BrokenApptMissedProc or PrefName.BrokenApptCancelledProc are true.</summary>
		public static bool HasBrokenApptProcs() {
			return PrefC.GetLong(PrefName.BrokenApptProcedure)>0;
		}

		///<summary>Sets given appt.AptStatus to broken.
		///Provide procCode that should be charted, can be null but will not chart a broken procedure.
		///Also considers various broken procedure based prefs.
		///Makes its own securitylog entries.</summary>
		public static void BreakApptHelper(Appointment appt,Patient pat,ProcedureCode procCode) {
			//suppressHistory is true due to below logic creating a log with a specific HistAppointmentAction instead of the generic changed.
			DateTime datePrevious=appt.DateTStamp;
			bool suppressHistory=false;
			if(procCode!=null) {
				suppressHistory=(procCode.ProcCode.In("D9986","D9987"));
			}
			Appointments.SetAptStatus(appt,ApptStatus.Broken,suppressHistory); //Appointments S-Class handles Signalods
			if(appt.AptStatus!=ApptStatus.Complete) { //seperate log entry for completed appointments.
				SecurityLogs.MakeLogEntry(Permissions.AppointmentEdit,pat.PatNum,
					appt.ProcDescript+", "+appt.AptDateTime.ToString()
					+", Broken from the Appts module.",appt.AptNum,datePrevious);
			}
			else {
				SecurityLogs.MakeLogEntry(Permissions.AppointmentCompleteEdit,pat.PatNum,
					appt.ProcDescript+", "+appt.AptDateTime.ToString()
					+", Broken from the Appts module.",appt.AptNum,datePrevious);
			}
			#region HL7
			//If there is an existing HL7 def enabled, send a SIU message if there is an outbound SIU message defined
			if(HL7Defs.IsExistingHL7Enabled()) {
				//S15 - Appt Cancellation event
				MessageHL7 messageHL7=MessageConstructor.GenerateSIU(pat,Patients.GetPat(pat.Guarantor),EventTypeHL7.S15,appt);
				//Will be null if there is no outbound SIU message defined, so do nothing
				if(messageHL7!=null) {
					HL7Msg hl7Msg=new HL7Msg();
					hl7Msg.AptNum=appt.AptNum;
					hl7Msg.HL7Status=HL7MessageStatus.OutPending;//it will be marked outSent by the HL7 service.
					hl7Msg.MsgText=messageHL7.ToString();
					hl7Msg.PatNum=pat.PatNum;
					HL7Msgs.Insert(hl7Msg);
#if DEBUG
					MessageBox.Show("Appointments",messageHL7.ToString());
#endif
				}
			}
			#endregion
			#region Charting the proc
			if(procCode!=null) {
				switch(procCode.ProcCode) { 
					case "D9986"://Missed
						HistAppointments.CreateHistoryEntry(appt.AptNum,HistAppointmentAction.Missed);
					break;
					case "D9987"://Cancelled
						HistAppointments.CreateHistoryEntry(appt.AptNum,HistAppointmentAction.Cancelled);
					break;
				}
				Procedure procedureCur=new Procedure();
				procedureCur.PatNum=pat.PatNum;
				procedureCur.ProvNum=(procCode.ProvNumDefault>0 ? procCode.ProvNumDefault : appt.ProvNum);
				procedureCur.CodeNum=procCode.CodeNum;
				procedureCur.ProcDate=DateTime.Today;
				procedureCur.DateEntryC=DateTime.Now;
				procedureCur.ProcStatus=ProcStat.C;
				procedureCur.ClinicNum=appt.ClinicNum;
				procedureCur.UserNum=Security.CurUser.UserNum;
				procedureCur.Note=Lans.g("AppointmentEdit","Appt BROKEN for")+" "+appt.ProcDescript+"  "+appt.AptDateTime.ToString();
				procedureCur.PlaceService=(PlaceOfService)PrefC.GetInt(PrefName.DefaultProcedurePlaceService);//Default proc place of service for the Practice is used. 
				List<InsSub> listInsSubs=InsSubs.RefreshForFam(Patients.GetFamily(pat.PatNum));
				List<InsPlan> listInsPlans=InsPlans.RefreshForSubList(listInsSubs);
				List<PatPlan> listPatPlans=PatPlans.Refresh(pat.PatNum);
				InsPlan insPlanPrimary=null;
				InsSub insSubPrimary=null;
				if(listPatPlans.Count>0) {
					insSubPrimary=InsSubs.GetSub(listPatPlans[0].InsSubNum,listInsSubs);
					insPlanPrimary=InsPlans.GetPlan(insSubPrimary.PlanNum,listInsPlans);
				}
				double procFee;
				long feeSch;
				if(insPlanPrimary==null||procCode.NoBillIns) {
					feeSch=FeeScheds.GetFeeSched(0,pat.FeeSched,procedureCur.ProvNum);
				}
				else {//Only take into account the patient's insurance fee schedule if the D9986 procedure is not marked as NoBillIns
					feeSch=FeeScheds.GetFeeSched(insPlanPrimary.FeeSched,pat.FeeSched,procedureCur.ProvNum);
				}
				procFee=Fees.GetAmount0(procedureCur.CodeNum,feeSch,procedureCur.ClinicNum,procedureCur.ProvNum);
				if(insPlanPrimary!=null&&insPlanPrimary.PlanType=="p"&&!insPlanPrimary.IsMedical) {//PPO
					double provFee=Fees.GetAmount0(procedureCur.CodeNum,Providers.GetProv(procedureCur.ProvNum).FeeSched,procedureCur.ClinicNum,
					procedureCur.ProvNum);
					procedureCur.ProcFee=Math.Max(provFee,procFee);
				}
				else {
					procedureCur.ProcFee=procFee;
				}
				if(!PrefC.GetBool(PrefName.EasyHidePublicHealth)) {
					procedureCur.SiteNum=pat.SiteNum;
				}
				Procedures.Insert(procedureCur);
				//Now make a claimproc if the patient has insurance.  We do this now for consistency because a claimproc could get created in the future.
				List<Benefit> listBenefits=Benefits.Refresh(listPatPlans,listInsSubs);
				List<ClaimProc> listClaimProcsForProc=ClaimProcs.RefreshForProc(procedureCur.ProcNum);
				Procedures.ComputeEstimates(procedureCur,pat.PatNum,listClaimProcsForProc,false,listInsPlans,listPatPlans,listBenefits,pat.Age,listInsSubs);
				FormProcBroken FormPB=new FormProcBroken(procedureCur);
				FormPB.IsNew=true;
				FormPB.ShowDialog();
			}
			#endregion
			#region BrokenApptAdjustment
			if(PrefC.GetBool(PrefName.BrokenApptAdjustment)) {
				Adjustment AdjustmentCur=new Adjustment();
				AdjustmentCur.DateEntry=DateTime.Today;
				AdjustmentCur.AdjDate=DateTime.Today;
				AdjustmentCur.ProcDate=DateTime.Today;
				AdjustmentCur.ProvNum=appt.ProvNum;
				AdjustmentCur.PatNum=pat.PatNum;
				AdjustmentCur.AdjType=PrefC.GetLong(PrefName.BrokenAppointmentAdjustmentType);
				AdjustmentCur.ClinicNum=appt.ClinicNum;
				FormAdjust FormA=new FormAdjust(pat,AdjustmentCur);
				FormA.IsNew=true;
				FormA.ShowDialog();
			}
			#endregion
			#region BrokenApptCommLog
			if(PrefC.GetBool(PrefName.BrokenApptCommLog)) {
				Commlog CommlogCur=new Commlog();
				CommlogCur.PatNum=pat.PatNum;
				CommlogCur.CommDateTime=DateTime.Now;
				CommlogCur.CommType=Commlogs.GetTypeAuto(CommItemTypeAuto.APPT);
				CommlogCur.Note=Lan.g("Appointment","Appt BROKEN for")+" "+appt.ProcDescript+"  "+appt.AptDateTime.ToString();
				CommlogCur.Mode_=CommItemMode.None;
				CommlogCur.UserNum=Security.CurUser.UserNum;
				FormCommItem FormCI=new FormCommItem();
				FormCI.ShowDialog(new CommItemModel() { CommlogCur=CommlogCur },new CommItemController(FormCI) { IsNew=true });
			}
			#endregion
			AutomationL.Trigger(AutomationTrigger.BreakAppointment,null,pat.PatNum);
			Recalls.SynchScheduledApptFull(appt.PatNum);
		}

		public static bool ValidateApptToPinboard(Appointment appt) {
			if(!Security.IsAuthorized(Permissions.AppointmentMove)) {
				return false;
			}
			if(appt.AptStatus==ApptStatus.Complete) {
				MsgBox.Show("Appointments","Not allowed to move completed appointments.");
				return false;
			}
			if(PatRestrictionL.IsRestricted(appt.PatNum,PatRestrict.ApptSchedule)) {
				return false;
			}
			return true;
		}

		/// <summary>Helper method to send given appt to pinboard.
		/// Refreshes Appointment module.
		/// Also does some appointment and security validation.</summary>
		public static void CopyAptToPinboardHelper(Appointment appt) {
			GotoModule.PinToAppt(new List<long>() { appt.AptNum },appt.PatNum);
		}

		public static bool ValidateApptUnsched(Appointment appt) {
			if((appt.AptStatus!=ApptStatus.Complete && !Security.IsAuthorized(Permissions.AppointmentMove)) //seperate permissions for complete appts.
				||(appt.AptStatus==ApptStatus.Complete && !Security.IsAuthorized(Permissions.AppointmentCompleteEdit))) {
				return false;
			}
			if(PatRestrictionL.IsRestricted(appt.PatNum,PatRestrict.ApptSchedule)) {
				return false;
			}
			if(appt.AptStatus==ApptStatus.PtNote | appt.AptStatus==ApptStatus.PtNoteCompleted) {
				return false;
			}
			return true;
		}

		/// <summary>Helper method to send given appt to the unscheduled list.
		/// Creates SecurityLogs and considers HL7.</summary>
		public static void SetApptUnschedHelper(Appointment appt,Patient pat=null) {
			DateTime datePrevious=appt.DateTStamp;
			Appointments.SetAptStatus(appt,ApptStatus.UnschedList); //Appointments S-Class handles Signalods
			#region SecurityLogs
			if(appt.AptStatus!=ApptStatus.Complete) { //seperate log entry for editing completed appts.
				SecurityLogs.MakeLogEntry(Permissions.AppointmentMove,appt.PatNum,
					appt.ProcDescript+", "+appt.AptDateTime.ToString()+", Sent to Unscheduled List",
					appt.AptNum,datePrevious);
			}
			else {
				SecurityLogs.MakeLogEntry(Permissions.AppointmentCompleteEdit,appt.PatNum,
					appt.ProcDescript+", "+appt.AptDateTime.ToString()+", Sent to Unscheduled List",
					appt.AptNum,datePrevious);
			}
			#endregion
			#region HL7
			//If there is an existing HL7 def enabled, send a SIU message if there is an outbound SIU message defined
			if(HL7Defs.IsExistingHL7Enabled()) {
				if(pat==null) {
					pat=Patients.GetPat(appt.PatNum);
				}
				//S15 - Appt Cancellation event
				MessageHL7 messageHL7=MessageConstructor.GenerateSIU(pat,Patients.GetPat(pat.Guarantor),EventTypeHL7.S15,appt);
				//Will be null if there is no outbound SIU message defined, so do nothing
				if(messageHL7!=null) {
					HL7Msg hl7Msg=new HL7Msg();
					hl7Msg.AptNum=appt.AptNum;
					hl7Msg.HL7Status=HL7MessageStatus.OutPending;//it will be marked outSent by the HL7 service.
					hl7Msg.MsgText=messageHL7.ToString();
					hl7Msg.PatNum=pat.PatNum;
					HL7Msgs.Insert(hl7Msg);
#if DEBUG
					MessageBox.Show("Appointments",messageHL7.ToString());
#endif
				}
			}
			#endregion
			Recalls.SynchScheduledApptFull(appt.PatNum);
		}

		///<summary>Creats a new appointment for the given patient.  A valid patient must be passed in.
		///Set useApptDrawingSettings to true if the user double clicked on the appointment schedule in order to make a new appointment.
		///It will utilize the global static properties to help set required fields for "Scheduled" appointments.
		///Otherwise, simply sets the corresponding PatNum and then the status to "Unscheduled".</summary>
		public static Appointment MakeNewAppointment(Patient PatCur,bool useApptDrawingSettings) {
			DateTime d;
			//Appointments.MakeNewAppointment may or may not use apptDateTime depending on useApptDrawingSettings,
			//however it's safer to just pass in the appropriate datetime verses DateTime.MinVal.
			DateTime apptDateTime;
			if(ApptDrawing.IsWeeklyView) {
				d=ContrAppt.WeekStartDate.AddDays(ContrAppt.SheetClickedonDay);
			}
			else {
				d=AppointmentL.DateSelected;
			}
			int minutes=(int)(ContrAppt.SheetClickedonMin/ApptDrawing.MinPerIncr)*ApptDrawing.MinPerIncr;
			apptDateTime=new DateTime(d.Year,d.Month,d.Day
				,ContrAppt.SheetClickedonHour,minutes,0);
			//Make the appointment in memory
			Appointment apptCur=Appointments.MakeNewAppointment(PatCur,apptDateTime,ContrAppt.SheetClickedonOp,useApptDrawingSettings);
			if(PatCur.AskToArriveEarly>0 && useApptDrawingSettings) {
				MessageBox.Show(Lan.g("FormApptsOther","Ask patient to arrive")+" "+PatCur.AskToArriveEarly
					+" "+Lan.g("FormApptsOther","minutes early at")+" "+apptCur.DateTimeAskedToArrive.ToShortTimeString()+".");
			}
			return apptCur;
		}

		///<summary>Returns true if the user switched to a different patient.</summary>
		public static bool PromptForMerge(Patient patCur,out Patient newPatCur) {
			newPatCur=patCur;
			if(patCur==null) {
				return false;
			}
			List<PatientLink> listMergedPats=PatientLinks.GetLinks(patCur.PatNum,PatientLinkType.Merge);
			if(!PatientLinks.WasPatientMerged(patCur.PatNum,listMergedPats)) {
				return false;
			}
			//This patient has been merged before.  Get a list of all patients that this patient has been merged into.
			List<Patient> listPats=Patients.GetMultPats(listMergedPats.Where(x => x.PatNumTo!=patCur.PatNum)
					.Select(x => x.PatNumTo).ToList()).ToList();
			//Notify the user that the currently selected patient has been merged before and then ask them if they want to switch to the correct patient.
			foreach(Patient pat in listPats) {
				if(pat.PatStatus.In(PatientStatus.Patient,PatientStatus.Inactive)
					&& (MessageBox.Show(Lan.g("ContrAppt","The currently selected patient has been merged into another patient.\r\n"
					+"Switch to patient")+" "+pat.GetNameLF()+" #"+pat.PatNum.ToString()+"?","",MessageBoxButtons.YesNo)==DialogResult.Yes)) 
				{
					newPatCur=pat;
					return true;
				}
			}
			//The user has declined every possible patient that the current patient was merged to.  Let them keep the merge from patient selected.
			return false;
		}

		///<summary></summary>
		public static PlannedApptStatus CreatePlannedAppt(Patient pat,int itemOrder,List<long> listPreSelectedProcNums=null) {
			if(!Security.IsAuthorized(Permissions.AppointmentCreate)) {
				return PlannedApptStatus.Failure;
			}
			if(PatRestrictionL.IsRestricted(pat.PatNum,PatRestrict.ApptSchedule)) {
				return PlannedApptStatus.Failure;
			}
			if(PromptForMerge(pat,out pat)) {
				FormOpenDental.S_Contr_PatientSelected(pat,true,false);
			}
			if(pat.PatStatus.In(PatientStatus.Archived,PatientStatus.Deceased)) {
				MsgBox.Show("Appointments","Appointments cannot be scheduled for "+pat.PatStatus.ToString().ToLower()+" patients.");
				return PlannedApptStatus.Failure;
			}
			Appointment AptCur=new Appointment();
			AptCur.PatNum=pat.PatNum;
			AptCur.ProvNum=pat.PriProv;
			AptCur.ClinicNum=pat.ClinicNum;
			AptCur.AptStatus=ApptStatus.Planned;
			AptCur.AptDateTime=DateTimeOD.Today;
			List<Procedure> listProcs=Procedures.GetManyProc(listPreSelectedProcNums,false);//Returns empty list if null.
			//If listProcs is empty then AptCur.Pattern defaults to PrefName.AppointmentWithoutProcsDefaultLength value.
			//See Appointments.GetApptTimePatternForNoProcs().
			AptCur.Pattern=Appointments.CalculatePattern(pat,AptCur.ProvNum,AptCur.ProvHyg,listProcs,isMake5Minute:true);
			AptCur.TimeLocked=PrefC.GetBool(PrefName.AppointmentTimeIsLocked);
			Appointments.Insert(AptCur);
			PlannedAppt plannedAppt=new PlannedAppt();
			plannedAppt.AptNum=AptCur.AptNum;
			plannedAppt.PatNum=pat.PatNum;
			plannedAppt.ItemOrder=itemOrder;
			PlannedAppts.Insert(plannedAppt);
			Procedures.UpdateAptNums(listPreSelectedProcNums,plannedAppt.AptNum,true);//Simply returns if listPreSelectedProcNums is null
			FormApptEdit FormApptEdit=new FormApptEdit(AptCur.AptNum);
			FormApptEdit.IsNew=true;
			FormApptEdit.ShowDialog();
			if(FormApptEdit.DialogResult!=DialogResult.OK){
				Procedures.UpdateAptNums(listPreSelectedProcNums,0,true);//Simply returns if listPreSelectedProcNums is null
				return PlannedApptStatus.FillGridNeeded;
			}
			//Only set the appointment hygienist to this patient's secondary provider if one was not manually set within the edit window.
			if(AptCur.ProvHyg < 1) {
				List<Procedure> myProcList=Procedures.GetProcsForSingle(AptCur.AptNum,true);
				bool allProcsHyg=(myProcList.Count>0 && myProcList.Select(x=>ProcedureCodes.GetProcCode(x.CodeNum)).ToList().All(x=>x.IsHygiene));
				//Automatically set the appointments hygienist to the secondary provider of the patient if one is set.
				if(allProcsHyg && pat.SecProv!=0) {
					Appointment aptOld=AptCur.Copy();
					AptCur.ProvNum=pat.SecProv;
					Appointments.Update(AptCur,aptOld);
				}
			}
			Patient patOld=pat.Copy();
			pat.PlannedIsDone=false;
			Patients.Update(pat,patOld);
			FormOpenDental.S_RefreshCurrentModule(isClinicRefresh:false);//if procs were added in appt, then this will display them
			return PlannedApptStatus.Success;
		}
	}

	public enum PlannedApptStatus {
    ///<summary>1 - Used when failed validation.</summary>
    Failure,
    ///<summary>2 - Used when planned appt was created.</summary>
    Success,
		///<summary>3 - Used when planned appt was not created but we might need to fill a grid.</summary>
    FillGridNeeded
  }
	
}
