using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenDentBusiness {
	///<summary>Separate class for keeping track of accounting transactions (procedures, payplancharges, and adjustments).
	///This should be used when you need to get a list of all transactions for a patient and sort them, eg in FormProcSelect.</summary>
	[Serializable]
	public class AccountEntry {
		private static long AccountEntryAutoIncrementValue=1;
		///<summary>No matter which constructor is used, the AccountEntryNum will be unique and automatically assigned.</summary>
		public long AccountEntryNum = (AccountEntryAutoIncrementValue++);
		//Read only data.  Do not modify, or else the historic information will be changed.
		[XmlIgnore]
		public object Tag;
		public DateTime Date;
		public long PriKey;
		public long ProvNum;
		public long ClinicNum;
		public long PatNum;
		public decimal AmountOriginal;
		//Variables below will be changed as needed.
		public decimal AmountStart;
		public decimal AmountEnd;
		public SplitCollection SplitCollection=new SplitCollection();

		[XmlElement(nameof(Tag))]
		public DtoObject TagXml {
			get {
				if(Tag==null) {
					return null;
				}
				return new DtoObject(Tag,Tag.GetType());
			}
			set {
				if(value==null) {
					Tag=null;
					return;
				}
				Tag=value.Obj;
			}
		}
		
		public new Type GetType() {
			return Tag.GetType();
		}

		///<summary>For serialization only.</summary>
		public AccountEntry() {
		}

		public AccountEntry(ClaimProc claimProc) {
				Tag=claimProc;
				Date=claimProc.DateCP;
				PriKey=claimProc.ClaimProcNum;
				AmountOriginal=0-(decimal)(claimProc.InsPayAmt+claimProc.WriteOff);
				AmountStart=AmountOriginal;
				AmountEnd=AmountOriginal;
				ProvNum=claimProc.ProvNum;
				ClinicNum=claimProc.ClinicNum;
				PatNum=claimProc.PatNum;
			}

		public AccountEntry(PayPlanCharge payPlanCharge) {
			Tag=payPlanCharge;
			Date=payPlanCharge.ChargeDate;
			PriKey=payPlanCharge.PayPlanChargeNum;
			AmountOriginal=(decimal)payPlanCharge.Principal+(decimal)payPlanCharge.Interest;
			AmountStart=AmountOriginal;
			AmountEnd=AmountOriginal;
			ProvNum=payPlanCharge.ProvNum;
			ClinicNum=payPlanCharge.ClinicNum;
			PatNum=payPlanCharge.PatNum;
		}

		///<summary>Turns negative adjustments positive.</summary>
		public AccountEntry(Adjustment adjustment) {
			Tag=adjustment;
			Date=adjustment.AdjDate;
			PriKey=adjustment.AdjNum;
			AmountOriginal=(decimal)adjustment.AdjAmt;
			AmountStart=AmountOriginal;
			AmountEnd=AmountOriginal;
			ProvNum=adjustment.ProvNum;
			ClinicNum=adjustment.ClinicNum;
			PatNum=adjustment.PatNum;
		}

		public AccountEntry(Procedure proc) {
			Tag=proc;
			Date=proc.ProcDate;
			PriKey=proc.ProcNum;
			AmountOriginal=(decimal)proc.ProcFee;
			AmountStart=AmountOriginal;
			//we might need to change it to this later, but we don't want to accidentally break current functionality for now.
			//AmountStart=(decimal)proc.ProcFee*Math.Max(1,proc.BaseUnits+proc.UnitQty); 
			AmountEnd=AmountOriginal;
			ProvNum=proc.ProvNum;
			ClinicNum=proc.ClinicNum;
			PatNum=proc.PatNum;
		}

		public AccountEntry(ProcExtended procE) {
			Tag=procE;
			Date=procE.Proc.ProcDate;
			PriKey=procE.Proc.ProcNum;
			AmountOriginal=(decimal)procE.AmountOriginal;
			AmountStart=(decimal)procE.AmountStart;
			AmountEnd=(decimal)procE.AmountEnd;
			ProvNum=procE.Proc.ProvNum;
			ClinicNum=procE.Proc.ClinicNum;
			PatNum=procE.Proc.PatNum;
		}

		public AccountEntry(PaySplit paySplit) {
			Tag=paySplit;
			Date=paySplit.DatePay;
			PriKey=paySplit.SplitNum;
			AmountOriginal=0-(decimal)paySplit.SplitAmt;
			AmountStart=AmountOriginal;
			AmountEnd=AmountOriginal;
			ProvNum=paySplit.ProvNum;
			SplitCollection.Add(paySplit);
			ClinicNum=paySplit.ClinicNum;
			PatNum=paySplit.PatNum;
		}

		///<summary></summary>
		public AccountEntry Copy(){
			return (AccountEntry)this.MemberwiseClone();
		}
	}
}
