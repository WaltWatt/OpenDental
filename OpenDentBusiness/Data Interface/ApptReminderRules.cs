using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Globalization;
using CodeBase;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace OpenDentBusiness{

	///<summary></summary>
	public class ApptReminderRules{
		#region Get Methods

		///<summary>Gets all, sorts by TSPrior Desc, Should never be more than 3 (per clinic if this is implemented for clinics.)</summary>
		public static List<ApptReminderRule> GetForTypes(params ApptReminderType[] arrTypes) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ApptReminderRule>>(MethodBase.GetCurrentMethod(),arrTypes);
			}
			if(arrTypes.Length==0) {
				return new List<ApptReminderRule>();
			}
			string command="SELECT * FROM apptreminderrule WHERE TypeCur IN("+string.Join(",",arrTypes.Select(x => POut.Int((int)x)))+")";
			return Crud.ApptReminderRuleCrud.SelectMany(command);
		}

		///<summary>Gets all from the DB for the given clinic and types.</summary>
		public static List<ApptReminderRule> GetForClinicAndTypes(long clinicNum,params ApptReminderType[] arrTypes) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ApptReminderRule>>(MethodBase.GetCurrentMethod(),clinicNum,arrTypes);
			}
			if(arrTypes.Length==0) {
				return new List<ApptReminderRule>();
			}
			string command = "SELECT * FROM apptreminderrule WHERE ClinicNum="+POut.Long(clinicNum)
				+" AND TypeCur IN("+string.Join(",",arrTypes.Select(x => POut.Int((int)x)))+")";
			return Crud.ApptReminderRuleCrud.SelectMany(command).ToList();
		}
		#endregion

		#region Modification Methods

		#region Insert
		#endregion

		#region Update
		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods
		#endregion

		///<summary>If true, then </summary>
		public static bool IsReminders {
			get {
				return PrefC.GetBool(PrefName.ApptRemindAutoEnabled) && Db.GetLong("SELECT count(*) FROM ApptReminderRule WHERE TSPrior>0")>0;
			}
		}

		///<summary>Gets all, sorts by TSPrior Desc, Should never be more than 3 (per clinic if this is implemented for clinics.)</summary>
		public static List<ApptReminderRule> GetAll(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ApptReminderRule>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM apptreminderrule ";
			return Crud.ApptReminderRuleCrud.SelectMany(command).OrderByDescending(x => new[] { 1,2,0 }.ToList().IndexOf((int)x.TypeCur)).ToList();
		}

		///<summary>16.3.29 is more strict about reminder rule setup. Prompt the user and allow them to exit the update if desired. Get all currently enabled reminder rules.
		///Returns 2 element list of bool. 
		///[0] indicates if any single clinic/practice has more than 1 same day reminder. 
		///[1] indicates if any single clinic/practice has more than 1 future day reminder.</summary>
		public static List<bool> Get_16_3_29_ConversionFlags() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<bool>>(System.Reflection.MethodBase.GetCurrentMethod());
			}
			//We can't use CRUD here as we may be in between versions so use DataTable directly.
			string command="SELECT ApptReminderRuleNum, TypeCur, TSPrior, ClinicNum FROM apptreminderrule WHERE TypeCur=0";
			var groups=Db.GetTable(command).Select().Select(x => new {
				ApptReminderRuleNum = PIn.Long(x[0].ToString()),
				TypeCur = PIn.Int(x[1].ToString()),
				TSPrior = TimeSpan.FromTicks(PIn.Long(x[2].ToString())),
				ClinicNum = PIn.Long(x[3].ToString())
			})
			//All rules grouped by clinic and whether they are same day or future day.
			.GroupBy(x => new { ClincNum=x.ClinicNum, IsSameDay=x.TSPrior.TotalDays<1 });
			return new List<bool>() {
				//Any 1 single clinic has more than 1 same day reminder.
				groups.Any(x => x.Key.IsSameDay && x.Count()>1),
				//Any 1 single clinic has more than 1 future day reminder.
				groups.Any(x => !x.Key.IsSameDay && x.Count()>1)
			};
		}

		public static List<ApptReminderRule> GetDefaults() {
			return GetForClinic(0);
		}

		///<summary>Gets all from the DB, sorts by TSPrior Desc.</summary>
		public static List<ApptReminderRule> GetForClinic(long clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ApptReminderRule>>(MethodBase.GetCurrentMethod(),clinicNum);
			}
			string command = "SELECT * FROM apptreminderrule WHERE ClinicNum="+POut.Long(clinicNum);
			return Crud.ApptReminderRuleCrud.SelectMany(command).OrderByDescending(x => x.TSPrior.TotalMinutes).ToList();
		}
		
		public static void SyncByClinicAndTypes(List<ApptReminderRule> listNew,long clinicNum,params ApptReminderType[] arrTypes) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listNew,clinicNum,arrTypes);
				return;
			}
			if(arrTypes.Length==0) {
				return;
			}
			List<ApptReminderRule> listOld=ApptReminderRules.GetForClinicAndTypes(clinicNum,arrTypes);//ClinicNum can be 0
			if(Crud.ApptReminderRuleCrud.Sync(listNew,listOld)) {
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,string.Join(", ",arrTypes.Select(x => x.GetDescription()))
					+" rules changed for ClinicNum: "+clinicNum.ToString()+".");
			}
		}

		///<summary>Gets one ApptReminderRule from the db.</summary>
		public static ApptReminderRule GetOne(long apptReminderRuleNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<ApptReminderRule>(MethodBase.GetCurrentMethod(),apptReminderRuleNum);
			}
			return Crud.ApptReminderRuleCrud.SelectOne(apptReminderRuleNum);
		}

		///<summary></summary>
		public static long Insert(ApptReminderRule apptReminderRule){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				apptReminderRule.ApptReminderRuleNum=Meth.GetLong(MethodBase.GetCurrentMethod(),apptReminderRule);
				return apptReminderRule.ApptReminderRuleNum;
			}
			return Crud.ApptReminderRuleCrud.Insert(apptReminderRule);
		}

		///<summary></summary>
		public static void Update(ApptReminderRule apptReminderRule){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),apptReminderRule);
				return;
			}
			Crud.ApptReminderRuleCrud.Update(apptReminderRule);
		}

		///<summary></summary>
		public static void Delete(long apptReminderRuleNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),apptReminderRuleNum);
				return;
			}
			Crud.ApptReminderRuleCrud.Delete(apptReminderRuleNum);
		}

		///<summary>Update Appointment.Confirmed. Returns true if update was allowed. Returns false if it was prevented.</summary>
		public static bool UpdateAppointmentConfirmationStatus(long aptNum,long confirmDefNum,string commaListOfExcludedDefNums) {
			Appointment aptCur=Appointments.GetOneApt(aptNum);
			if(aptCur==null) {
				return false;
			}
			List<long> preventChangeFrom=commaListOfExcludedDefNums.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
			if(preventChangeFrom.Contains(aptCur.Confirmed)) { //This appointment is in a confirmation state that can no longer be updated.
				return false;
			}
			//Keep the update small.
			Appointment aptOld=aptCur.Copy();
			aptCur.Confirmed=confirmDefNum;
			Appointments.Update(aptCur,aptOld);//Appointments S-Class handles Signalods
			SecurityLogs.MakeLogEntry(Permissions.ApptConfirmStatusEdit,aptCur.PatNum,"Appointment confirmation status changed from "
				+Defs.GetName(DefCat.ApptConfirmed,aptOld.Confirmed)+" to "+Defs.GetName(DefCat.ApptConfirmed,aptCur.Confirmed)
				+" due to an eConfirmation.",aptCur.AptNum,LogSources.AutoConfirmations,aptOld.DateTStamp);
			return true;			
		}

		public static ApptReminderRule CreateDefaultReminderRule(ApptReminderType ruleType,long clinicNum = 0,bool isBeforeAppointment = true) {
			switch(ruleType) {
				case ApptReminderType.Reminder:
					return new ApptReminderRule() {
						ClinicNum=clinicNum,//works with practice too because _listClinics[0] is a spoofed "Practice/Defaults" clinic with ClinicNum=0
						TypeCur=ApptReminderType.Reminder,
						TSPrior=TimeSpan.FromHours(3),
						TemplateSMS="Appointment Reminder: [NameF] is scheduled for [ApptTime] on [ApptDate] at [ClinicName]. If you have questions call [ClinicPhone].",//default message
						TemplateEmail="[NameF] is scheduled for [ApptTime] on [ApptDate] at [OfficeName]. If you have questions, call [OfficePhone].",//default message
						TemplateEmailSubject="Appointment Reminder",//default subject
						TemplateSMSAggShared="Appointment Reminder:\n[Appts]\nIf you have questions call [ClinicPhone].",
						TemplateSMSAggPerAppt="[NameF] is scheduled for [ApptTime] on [ApptDate] at [ClinicName].",
						TemplateEmailSubjAggShared="Appointment Reminder",
						TemplateEmailAggShared="[Appts]\nIf you have questions, call [OfficePhone].",
						TemplateEmailAggPerAppt="[NameF] is scheduled for [ApptTime] on [ApptDate] at [ClinicName].",
						//SendOrder="0,1,2" //part of ctor
					};
				case ApptReminderType.ConfirmationFutureDay:
					return new ApptReminderRule() {
						ClinicNum=clinicNum,//works with practice too because _listClinics[0] is a spoofed "Practice/Defaults" clinic with ClinicNum=0
						TypeCur=ApptReminderType.ConfirmationFutureDay,
						TSPrior=TimeSpan.FromDays(7),
						TemplateSMS="[NameF] is scheduled for [ApptTime] on [ApptDate] at [OfficeName]. Reply [ConfirmCode] to confirm or call [OfficePhone].",//default message
						TemplateEmail="[NameF] is scheduled for [ApptTime] on [ApptDate] at [OfficeName]. Click [ConfirmURL] to confirm or call [OfficePhone].",//default message
						TemplateEmailSubject="Appointment Confirmation",//default subject
						TemplateSMSAggShared="[Appts]\nReply [ConfirmCode] to confirm or call [OfficePhone].",
						TemplateSMSAggPerAppt="[NameF] is scheduled for [ApptTime] on [ApptDate] at [ClinicName].",
						TemplateEmailSubjAggShared="Appointment Confirmation",
						TemplateEmailAggShared="[Appts]\nClick [ConfirmURL] to confirm or call [OfficePhone].",
						TemplateEmailAggPerAppt="[NameF] is scheduled for [ApptTime] on [ApptDate] at [ClinicName].",
						//SendOrder="0,1,2" //part of ctor
						DoNotSendWithin=TimeSpan.FromDays(1).Add(TimeSpan.FromHours(10)),
					};
				case ApptReminderType.PatientPortalInvite:
					if(isBeforeAppointment) {
						return new ApptReminderRule() {
							ClinicNum=clinicNum,
							TypeCur=ApptReminderType.PatientPortalInvite,
							TSPrior=TimeSpan.FromDays(7),
							TemplateEmail=@"[NameF],
			
In preparation for your upcoming dental appointment at [OfficeName], we invite you to log in to our Patient Portal. "+@"
There you can view your scheduled appointments, view your treatment plan, send a message to your provider, and view your account balance. "+@"
Visit [PatientPortalURL] and use this temporary user name and password to log in:

User name: [UserName]
Password: [Password]

If you have any questions, please give us a call at [OfficePhone], and we would be happy to answer any of your questions.",
							TemplateEmailSubject="Patient Portal Invitation",
							TemplateEmailSubjAggShared="Patient Portal Invitation",
							TemplateEmailAggShared=@"[NameF],
			
In preparation for your upcoming dental appointments at [OfficeName], we invite you to log in to our Patient Portal. "+@"
There you can view your scheduled appointments, view your treatment plan, send a message to your provider, and view your account balance. "+@"
Visit [PatientPortalURL] and use these temporary user names and passwords to log in:

[Credentials]
If you have any questions, please give us a call at [OfficePhone], and we would be happy to answer any of your questions.",
							TemplateEmailAggPerAppt=@"[NameF]
User name: [UserName]
Password: [Password]
",
							SendOrder="2" //Email only
						};
					}
					else {//Same day
						return new ApptReminderRule() {
							ClinicNum=clinicNum,
							TypeCur=ApptReminderType.PatientPortalInvite,
							TSPrior=new TimeSpan(-1,0,0),//Send 1 hour after the appointment
							TemplateEmail=@"[NameF],
			
Thank you for coming in to visit [OfficeName] today. As a follow up to your appointment, we invite you to log in to our Patient Portal. "+@"
There you can view your scheduled appointments, view your treatment plan, send a message to your provider, and view your account balance. "+@"
Visit [PatientPortalURL] and use this temporary user name and password to log in:

User name: [UserName]
Password: [Password]

If you have any questions, please give us a call at [OfficePhone], and we would be happy to answer any of your questions.",
							TemplateEmailSubject="Patient Portal Invitation",
							TemplateEmailSubjAggShared="Patient Portal Invitation",
							TemplateEmailAggShared=@"[NameF],
			
Thank you for coming in to visit [OfficeName] today. As a follow up to your appointment, we invite you to log in to our Patient Portal. "+@"
There you can view your scheduled appointments, view your treatment plan, send a message to your provider, and view your account balance. "+@"
Visit [PatientPortalURL] and use these temporary user names and passwords to log in:

[Credentials]
If you have any questions, please give us a call at [OfficePhone], and we would be happy to answer any of your questions.",
							TemplateEmailAggPerAppt=@"[NameF]
User name: [UserName]
Password: [Password]
",
							SendOrder="2" //Email only
						};
					}
			}
			return null;
		}

		/// <summary>Returns the list of replacement tags available for the passed in ApptReminderRuleType.</summary>
		public static List<string> GetAvailableTags(ApptReminderType type) {
			List<string> retVal = new List<string>() {
				"[NameF]",
				"[ApptTime]",
				"[ApptTimeAskedArrive]",
				"[ApptDate]",
				"[ClinicName]",
				"[ClinicPhone]",
				"[ProvName]",
				"[ProvAbbr]",
				"[PracticeName]",
				"[PracticePhone]"
			};
			if(type==ApptReminderType.ConfirmationFutureDay) {
				retVal.AddRange(new[] {
					"[ConfirmCode]",
					"[ConfirmURL]"
				});
			}
			else if(type==ApptReminderType.PatientPortalInvite) {
				retVal.AddRange(new[] {
					"[UserName]",
					"[Password]",
					"[PatientPortalURL]"
				});
			}
			retVal.Sort();//alphabetical
			return retVal;
		}	

		/// <summary>Returns the list of replacement tags available for the Aggregate Templates for the passed in ApptReminderRuleType.</summary>
		public static List<string> GetAvailableAggTags(ApptReminderType type) {
			List<string> retVal=GetAvailableTags(type);
			retVal.Add("[Appts]");//[Appts] is used for child nodes
			retVal.Sort();
			return retVal;
		}
	}
}