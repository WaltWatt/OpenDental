using System;

namespace OpenDentBusiness{

	///<summary>One Rx for one patient. Copied from rxdef rather than linked to it.</summary>
	[Serializable]
	[CrudTable(AuditPerms=CrudAuditPerm.RxCreate|CrudAuditPerm.RxEdit)]
	public class RxPat:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long RxNum;
		///<summary>FK to patient.PatNum.</summary>
		public long PatNum;
		///<summary>Date of Rx.</summary>
		public DateTime RxDate;
		///<summary>Drug name. Example: PenVK 500 mg capsules. Example: Percocet 5/500 tablets.</summary>
		public string Drug;
		///<summary>Directions. Example: Take 2 tablets qid. (qid means 4 times a day)</summary>
		public string Sig;
		///<summary>Amount to dispense. Example: 12 (twelve)</summary>
		public string Disp;
		///<summary>Number of refills. Example: 3.  Example: 1 per month.</summary>
		public string Refills;
		///<summary>FK to provider.ProvNum.</summary>
		public long ProvNum;
		///<summary>Notes specific to this Rx.  Will not show on the printout.  For staff use only.</summary>
		public string Notes;
		///<summary>FK to pharmacy.PharmacyNum.</summary>
		public long PharmacyNum;
		///<summary>Is a controlled substance.  This will affect the way it prints.</summary>
		public bool IsControlled;
		///<summary>The last date and time this row was altered.  Not user editable.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TimeStamp)]
		public DateTime DateTStamp;
		///<summary>Enum:RxSendStatus </summary>
		public RxSendStatus SendStatus;
		///<summary>Deprecated.  RxNorm Code identifier.  Was used in FormRxSend for EHR 2011, but FormRxSend has been deleted.  No longer in use anywhere.  Still exists in db for now.</summary>
		public long RxCui;
		///<summary>NCI Pharmaceutical Dosage Form code.  Only used with ehr.  For example, C48542 is the code for “Tablet dosing unit”.  User enters code manually, and it's only used for Rx Send, which will be deprecated with 2014 cert.  Guaranteed that nobody actually uses or cares about this field.</summary>
		public string DosageCode;
		///<summary>eRx returns this unique identifier to use for electronic Rx.  Also set for Open Dental created medications using a different format.</summary>
		public string ErxGuid;
		///<summary>True for historic prescriptions which existed prior to version 15.4.  The purpose of this column is to keep historic reports accurate.</summary>
		public bool IsErxOld;
		///<summary>The pharmacyinfo field contains the pharmacy name as well as other information about the pharmacy, but the information is inconsistent.
		///The purpose of this field is to give the user means to visually verify they have the correct pharmacy selected.</summary>
		public string ErxPharmacyInfo;
		///<summary>If true will require procedure be attached to this prescription when printed.  Usually true if IsControlled is true.</summary>
		public bool IsProcRequired;
		///<summary>The procedure attached to this prescription when IsProcRequired is true.</summary>
		public long ProcNum;
		///<summary>The number of days this prescription is intended to last.  Only used when IsProcRequired is true.</summary>
		public int DaysOfSupply;

		///<summary></summary>
		public RxPat Copy() {
			return (RxPat)this.MemberwiseClone();
		}

	}

	///<summary></summary>
	public enum RxSendStatus {
		///<summary>0</summary>
		Unsent,
		///<summary>1- This will never be used in production.  It was only used for proof of concept when building EHR.</summary>
		InElectQueue,
		///<summary>2</summary>
		SentElect,
		///<summary>3</summary>
		Printed,
		///<summary>4</summary>
		Faxed,
		///<summary>5</summary>
		CalledIn,
		///<summary>6</summary>
		GaveScript,
		///<summary>7</summary>
		Pending
	}


}













