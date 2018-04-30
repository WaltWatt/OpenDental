using System;
using System.ComponentModel;

namespace OpenDentBusiness {
	///<summary>Messages are only inserted into this table after they are accepted by ODHQ.</summary>
	[Serializable]
	[CrudTable(HasBatchWriteMethods=true)]
	public class SmsToMobile:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long SmsToMobileNum;
		///<summary>FK to patient.PatNum</summary>
		public long PatNum;
		///<summary>GUID. Uniquely identifies this message and is used for tracking message status.</summary>
		public string GuidMessage;
		///<summary>GUID. When sending batch messages, all messages will have the same batch GUID that should be the GUID of the first message within the batch.</summary>
		public string GuidBatch;
		///<summary>This is the sending phone number in international format. Each office may have several different numbers that they use.</summary>
		public string SmsPhoneNumber;
		///<summary>The phone number that this message was sent to. Must be kept in addition to the PatNum.</summary>
		public string MobilePhoneNumber;
		///<summary>Set to true if this message should "jump the queue" and be sent asap.</summary>
		public bool IsTimeSensitive;
		///<summary>Enum:SmsMessageSource  This is used to identify where in the program this message originated from.</summary>
		public SmsMessageSource MsgType;
		///<summary>The contents of the message.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob | CrudSpecialColType.CleanText)]
		public string MsgText;
		///<summary>Enum:SmsDeliveryStatus  Set by the Listener, tracks status of SMS.</summary>
		public SmsDeliveryStatus SmsStatus;
		///<summary>The count of parts that this message will be broken into when sent.
		///A single long message will be broken into several smaller 153 utf8 or 70 unicode character messages.</summary>
		public int MsgParts;
		///<summary>The amount charged to the customer. Total cost for this message always stored in US Dollars.</summary>
		public float MsgChargeUSD;
		///<summary>FK to clinic.ClinicNum.  0 when not using clinics.</summary>
		public long ClinicNum;
		///<summary>Only used when SmsDeliveryStatus==Failed.</summary>
		public string CustErrorText;
		///<summary>Time message was accepted at ODHQ.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateTimeSent;
		///<summary>Date time that the message was either successfully delivered or failed.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateTimeTerminated;
		///<summary>Messages are hidden, not deleted.</summary>
		public bool IsHidden;

		public SmsToMobile Copy() {
			return (SmsToMobile)this.MemberwiseClone();
		}
	}

	///<summary>This helps us determine how to handle messages.</summary>
	public enum SmsMessageSource{
		///<summary>0. Should not be used.</summary>
		Undefined,
		///<summary>1. This should be used for one-off messages that might be sent as direct communication with patient.</summary>
		[Description("Manual")]
		DirectSms,
		///<summary>2. Used when sending single or batch recall SMS from the Open Dental program.</summary>
		[Description("Recall Manual")]
		Recall,
		///<summary>3. Used when sending single or batch reminder SMS.</summary>
		[Description("eReminder")]
		Reminder,
		///<summary>4. Used when sending a test message from HQ. Customer will not be charged for this message.</summary>
		[Description("Test")]
		TestNoCharge,
		///<summary>5. Used when sending confirmations.</summary>
		Confirmation,
		///<summary>6. Used when sending confirmation requests. Will be the subject of automated response processing.</summary>
		[Description("eConfirmation")]
		ConfirmationRequest,
		///<summary>7. Used when sending batch recall SMS from the eConnector.</summary>
		[Description("Recall Auto")]
		RecallAuto,
		///<summary>8. Used when sending single or batch SMS from the clicking the Text button on the ASAP window.</summary>
		[Description("ASAP Manual")]
		AsapManual,
		///<summary>9. Sending an SMS for the Web Sched ASAP feature.</summary>
		[Description("ASAP Auto")]
		WebSchedASAP,
		///<summary>10. Sending an SMS for the Web Sched verify feature.</summary>
		[Description("Verification")]
		Verify,
		///<summary>11. Sending an SMS to let the patient know that a statement is available.</summary>
		[Description("Statements")]
		Statements,
		///<summary>11. Sending an SMS to let the patient know that a statement is available.</summary>
		[Description("Verify WSNP")]
		VerifyWSNP,
	}

	///<summary>None should never be used, the code should be re-written to not use it.</summary>
	public enum SmsDeliveryStatus {
		///<summary>0. Should not be used.</summary>
		None,
		///<summary>1. After a message has been accepted at ODHQ. Before any feedback.</summary>
		Pending,
		///<summary>2. Delivered to customer, carrier replied with confirmation.</summary>
		DeliveryConf,
		///<summary>3. Delivered to customer, no confirmation of failure or delivery sent back from carrier.</summary>
		DeliveryUnconf,
		///<summary>4. Attempted delivery, failure message return after arriving at handset.</summary>
		FailWithCharge,
		///<summary>5. Attempted delivery, immediate failure confirmation received from carrier.</summary>
		FailNoCharge
	}
}