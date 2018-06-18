using System;
using System.Collections;

namespace OpenDentBusiness {

	///<summary>An actual signal that gets sent out as part of the messaging functionality.</summary>
	[Serializable]
	[CrudTable(HasBatchWriteMethods=true)]
	public class Signalod:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long SignalNum;
		///<summary>If IType=Date, then this is the affected date in the Appointments module.</summary>
		public DateTime DateViewing;
		///<summary>The exact server time when this signal was entered into db.
		///This does not need to be set by sender since it's handled automatically.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime SigDateTime;
		///<summary>Usually identifies the object that was edited to cause the signal to be created.
		///Can be used for special scenarios based on the FKeyType.  E.g. for SmsMsgUnreadCount, this represents a count, not an FK.</summary>
		public long FKey;
		///<summary>Describes the type of object referenced by the FKey.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.EnumAsString)]
		public KeyType FKeyType;
		///<summary>Enum:InvalidType Indicates what cache or entity has been changed.</summary>
		public InvalidType IType;
		///<summary>Enum:RemotingRole The RemotingRole of the instance that created this signal.</summary>
		public RemotingRole RemoteRole;
		///<summary>Message value of the signal.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string MsgValue;

		///<summary></summary>
		public Signalod Copy() {
			return (Signalod)this.MemberwiseClone();
		}

	
	}

	///<summary>Do not combine with SignalType, they must be seperate. Stored as string, safe to reorder enum values.</summary>
	public enum KeyType {
		///<summary>0</summary>
		Undefined = 0,
		///<summary>1</summary>
		FeeSched,
		///<summary>2</summary>
		Job,
		///<summary>3</summary>
		Operatory,
		///<summary>4 - HQ only.  FKey will be the extension of the corresponding phone that is invalid.
		///Specifically used to talk to the PhoneTrackingServer in order to let it know that an extension has changed (e.g. queue change).</summary>
		PhoneExtension,
		///<summary>5</summary>
		Provider,
		///<summary>6</summary>
		SigMessage,
		///<summary>7 - Special KeyType that does not use a FK but instead will set FKey to a count of unread messages.
		///Used along side the SmsTextMsgReceivedUnreadCount InvalidType.</summary>
		SmsMsgUnreadCount,
		///<summary>8</summary>
		Task,
		///<summary>9 - Used to identify which signals a form can ignore.  If the FKey==Process.GetCurrentProcess().Id then this process sent it so ignore
		///it.  Used in FormTerminal, FormTerminalManager, and FormSheetFillEdit (for forms being filled at a kiosk).</summary>
		ProcessId,
		///<summary>10 - Used to notify the phone tracking server to kick all users out of a conference room.</summary>
		ConfKick
	}






}



















