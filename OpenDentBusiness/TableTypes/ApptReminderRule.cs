using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace OpenDentBusiness {
	///<summary> Appointment Reminder Rules are used to track the automated generation and sending of appointment reminders and confirmations. 
	/// Users are allowed to define up to two reminders and one confirmation (per clinic.) These can be sent out any number of Days, Hours, and/or 
	/// Minutes before a scheduled appointment.
	/// <para>PRACTICE - Appointment Reminder Rules will be saved and edited with clinicNum=0. This denotes the "Defaults" when using clinics, 
	/// but for a practice the defaults become the practice rules.</para>
	///<para>CLINICS - When using clinics, each clinic has a bool "IsConfirmEnabled" that determines if a particular clinic has automated reminders/confirmations
	///enabled. If not, no reminders will be sent out for the clinic. If enabled, and no rules are defined for the clinic, then the clinic will attempt to use the
	///defaults that have been defined with clinicNum==0. If a clinic is enabled and has at least one AppointmentReminderRule defined, then NO defaults will be
	///used for that clinic.</para>
	///<para>REMINDERS - reminders are sent out using the ApptComm system implemented by DG. These used to be stored as preferences for the practice only,
	///now users are allowed to define them on a per clinic basis. Reminders should be considered one way communications and should not be desingned with a
	///customer response in mind.</para>
	///<para>CONFIRMATIONS - confirmations are sent using the new automated-confirmation system implemented by RM (proper) and SO (web backend). Confirmations
	///are intended to allow end patients to respond to OpenDental via text or email and automatically confirm, or set to a desired status, the appointments
	///on the schedule.</para>
	/// </summary>
	[Serializable()]
	[CrudTable(IsSynchable =true)]
	public class ApptReminderRule : TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey = true)]
		public long ApptReminderRuleNum;
		///<summary>Enum:ApptReminderType </summary>
		public ApptReminderType TypeCur;
		///<summary>Time before appointment that this confirmation should be sent.</summary>
		[XmlIgnore]
		[CrudColumn(SpecialType = CrudSpecialColType.TimeSpanLong)]
		public TimeSpan TSPrior;
		///<summary>Comma Delimited List of comm types. Enum values of ApptComm.CommType. 0=pref,1=sms,2=email; Like the deprecated pref "ApptReminderSendOrder"</summary>
		public string SendOrder;
		///<summary>Set to True if both an email AND a text should be sent.</summary>
		public bool IsSendAll;
		///<summary>If using SMS, this template will be used to generate the body of the text message.</summary>
		[CrudColumn(SpecialType = CrudSpecialColType.TextIsClob)]
		public string TemplateSMS;
		///<summary>If using email, this template will be used to generate the subject of the email.</summary>
		[CrudColumn(SpecialType = CrudSpecialColType.TextIsClob)]
		public string TemplateEmailSubject;
		///<summary>If using email, this template will be used to generate the body of the email.</summary>
		[CrudColumn(SpecialType = CrudSpecialColType.TextIsClob)]
		public string TemplateEmail;
		///<summary>FK to clinic.ClinicNum.  Allows reminder rules to be configured on a per clinic basis. If ClinicNum==0 then it is the practice/HQ/default settings.</summary>
		public long ClinicNum;
		///<summary>Used when aggregating multiple appointments together into a single message.</summary>
		[CrudColumn(SpecialType = CrudSpecialColType.TextIsClob)]
		public string TemplateSMSAggShared;
		///<summary>Used when aggregating multiple appointments together into a single message.</summary>
		[CrudColumn(SpecialType = CrudSpecialColType.TextIsClob)]
		public string TemplateSMSAggPerAppt;
		///<summary>Used when aggregating multiple appointments together into a single message.</summary>
		[CrudColumn(SpecialType = CrudSpecialColType.TextIsClob)]
		public string TemplateEmailSubjAggShared;
		///<summary>Used when aggregating multiple appointments together into a single message.</summary>
		[CrudColumn(SpecialType = CrudSpecialColType.TextIsClob)]
		public string TemplateEmailAggShared;
		///<summary>Used when aggregating multiple appointments together into a single message.</summary>
		[CrudColumn(SpecialType = CrudSpecialColType.TextIsClob)]
		public string TemplateEmailAggPerAppt;
		///<summary>The time before the appointment in which this reminder should NOT be sent. E.g., if this value is 2 days, and an appt is created one
		///day in the future, a reminder will not be sent.</summary>
		[XmlIgnore]
		[CrudColumn(SpecialType = CrudSpecialColType.TimeSpanLong)]
		public TimeSpan DoNotSendWithin;

		///<summary>Used only for serialization purposes</summary>
		[XmlElement("TSPrior",typeof(long))]
		public long TSPriorXml {
			get {
				return TSPrior.Ticks;
			}
			set {
				TSPrior=TimeSpan.FromTicks(value);
			}
		}

		///<summary>Used only for serialization purposes</summary>
		[XmlElement("DoNotSendWithin",typeof(long))]
		public long DoNotSendWithinXml {
			get {
				return DoNotSendWithin.Ticks;
			}
			set {
				DoNotSendWithin=TimeSpan.FromTicks(value);
			}
		}

		public ApptReminderRule() {
			SendOrder="0,1,2";//default send order
			//Singleton templates
			TemplateSMS="";//default message set from FormApptReminderRuleEdit
			TemplateEmail="";//default message set from FormApptReminderRuleEdit
			TemplateEmailSubject="";
			//Aggregate Templates
			TemplateSMSAggShared="";
			TemplateSMSAggPerAppt="";
			TemplateEmailSubjAggShared="";
			TemplateEmailAggShared="";
			TemplateEmailAggPerAppt="";
		}

		public bool IsValidDuration {
			get {
				if(!IsEnabled) {
					return false;
				}
				if(TypeCur==ApptReminderType.ConfirmationFutureDay) {
					return TSPrior.Days>=1;
				}
				return true;
			}
		}
		public bool IsSameDay {
			get {
				return IsValidDuration && TSPrior.Days==0;
			}
		}

		public bool IsFutureDay {
			get {
				return IsValidDuration && TSPrior.TotalDays>=1;
			}
		}

		public bool IsPastDay {
			get {
				return IsValidDuration && TSPrior.TotalDays<=-1;
			}
		}

		///<summary>Only valid for IsFutureDay rule.</summary>
		public int NumDaysInFuture {
			get {
				if(!IsFutureDay) {
					return 0;
				}
				//Rounds 1.1 to 1. So anything less than exactly n days will be n days.
				return TSPrior.Days;
			}
		}

		///<summary>Only valid for IsSameDay rule.</summary>
		public int NumMinutesInFuture {
			get {
				if(!IsSameDay) {
					return 0;
				}
				return (int)TSPrior.TotalMinutes;
			}
		}

		///<summary>Only valid for IsPastDay rule.</summary>
		public int NumDaysInPast {
			get {
				if(!IsPastDay) {
					return 0;
				}
				//Rounds 1.1 to 1. So anything less than exactly n days will be n days.
				return Math.Abs(TSPrior.Days);
			}
		}

		public bool IsEnabled {
			get {
				return TSPrior!=TimeSpan.Zero;
			}
		}

		public ApptReminderRule Copy() {
			ApptReminderRule retVal = (ApptReminderRule)this.MemberwiseClone();
			return retVal;
		}

		public ApptReminderRule CopyWithClinicNum(long clinicNum) {
			ApptReminderRule retVal = Copy();
			retVal.ClinicNum=clinicNum;
			return retVal;
		}
	}

	public enum ApptReminderType {
		///<summary>0 - Used to define the rules for when reminders should be sent out.</summary>
		Reminder,
		///<summary>1 - Defines rules for when confirmations should be sent out.</summary>
		[Description("Confirmation")]
		ConfirmationFutureDay,
		///<summary>2 - DEPRECATED. As of 17.4, all reminders have a status of Reminder.</summary>
		[Description("Reminder (Future)")]
		ReminderFutureDay,
		///<summary>3 - Send emails to patients with their credentials to the Patient Portal.</summary>
		[Description("Patient Portal Invites")]
		PatientPortalInvite,
	}

	///<summary></summary>
	public enum CommType {
		///<summary>0 - Use text OR email based on patient preference.</summary>
		Preferred = 0,
		///<summary>1 - Attempt to send text message, if successful do not send via email. (Unless, a SendAll bool is used, which usually negates the need for this enumeration.)</summary>
		Text = 1,
		///<summary>2 - Attempt to send email message, if successful do not send via text. (Unless, a SendAll bool is used, which usually negates the need for this enumeration.)</summary>
		Email = 2
	}

	///<summary></summary>
	public enum IntervalType {
		///<summary></summary>
		Daily,
		///<summary></summary>
		Hourly

	}
}
