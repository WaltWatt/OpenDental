using System;
using System.Collections;

namespace OpenDentBusiness{

	///<summary>One of the dated charges attached to a payment plan.  This has nothing to do with payments, but rather just causes the amount due to increase on the date of the charge.  The amount of the charge is the sum of the principal and the interest.</summary>
	[Serializable]
	[CrudTable(IsSynchable=true)]
	public class PayPlanCharge:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long PayPlanChargeNum;
		///<summary>FK to payplan.PayPlanNum.</summary>
		public long PayPlanNum;
		///<summary>FK to patient.PatNum.  The guarantor account that each charge will affect.  Does not have to match the guarantor of the payment plan.
		///This column doesn't even have to point to a guarantor at all.  
		///E.g. Credits and Closeout debits will be linked to the patient, not guarantor.</summary>
		public long Guarantor;
		///<summary>FK to patient.PatNum.  The patient account that the principal gets removed from.</summary>
		public long PatNum;
		///<summary>The date that the charge will show on the patient account.  Any charge with a future date will not show on the account yet and will not affect the balance.</summary>
		public DateTime ChargeDate;
		///<summary>For Debits, this is the principal charge amount.  For Credits (version 2 only), then this is the credit amount.</summary>
		public double Principal;
		///<summary>For Debits, this is the interest portion of this payment.  Always 0 for Credits.</summary>
		public double Interest;
		///<summary>Any note about this particular payment plan charge</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string Note;
		///<summary>FK to provider.ProvNum.  Since there is no ProvNum field at the payplan level, the provider must be the same for all payplancharges.  
		///It's initially assigned as the patient priProv.  Payments applied should be to this provnum, 
		///although the current user interface does not help with this.</summary>
		public long ProvNum;
		///<summary>FK to clinic.ClinicNum.  Since there is no ClincNum field at the payplan level, the clinic must be the same for all payplancharges.  It's initially assigned using the patient clinic.  Payments applied should be to this clinic, although the current user interface does not help with this.</summary>
		public long ClinicNum;
		///<summary>Enum: The charge type of the payment plan. 0 - Debit, 1 - Credit.  Only relevant for those on Payment Plan Version 2.</summary>
		public PayPlanChargeType ChargeType;
		///<summary>FK to procedurelog.ProcNum.  The procedure that this payplancharge is attached to.  Only applies to credits.
		///Always 0 for debits.  Can be 0 for credits not attached to a procedure.</summary>
		public long ProcNum;
		///<summary>DateTime payplancharge was added to the payplan. Not editable by user.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateTEntry)]
		public DateTime SecDateTEntry;
		///<summary>DateTime payplancharge was edited. Not editable by user.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TimeStamp)]
		public DateTime SecDateTEdit;
		///<summary>FK to statement.StatementNum.  Only used when the statement in an invoice.</summary>
		public long StatementNum;

		///<summary></summary>
		public PayPlanCharge Copy(){
			return (PayPlanCharge)this.MemberwiseClone();
		}


	}
	public enum PayPlanChargeType {
		///<summary>0 - Debit</summary>
		Debit,
		///<summary>1 - Credit</summary>
		Credit
	}
}





















