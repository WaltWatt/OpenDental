using System;
using System.Collections;
using System.Drawing;

namespace OpenDentBusiness {
	///<summary>One credit card along with any recurring charge information.</summary>
	[Serializable]
	public class CreditCard:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long CreditCardNum;
		///<summary>FK to patient.PatNum.</summary>
		public long PatNum;
		///<summary>.</summary>
		public string Address;
		///<summary>Postal code.</summary>
		public string Zip;
		///<summary>Token for X-Charge. Alphanumeric, upper and lower case, about 15 char long.  Passed into Xcharge instead of the actual card number.</summary>
		public string XChargeToken;
		///<summary>Credit Card Number.  Will be stored masked: XXXXXXXXXXXX1234.</summary>
		public string CCNumberMasked;
		///<summary>Only month and year are used, the day will usually be 1.</summary>
		public DateTime CCExpiration;
		///<summary>The order that multiple cards will show.  Zero-based.  First one will be default.</summary>
		public int ItemOrder;
		///<summary>Amount set for recurring charges.</summary>
		public Double ChargeAmt;
		///<summary>Start date for recurring charges.</summary>
		public DateTime DateStart;
		///<summary>Stop date for recurring charges.</summary>
		public DateTime DateStop;
		///<summary>Any notes about the credit card or account goes here.</summary>
		public string Note;
		///<summary>FK to payplan.PayPlanNum.</summary>
		public long PayPlanNum;
		///<summary>Token for PayConnect.  PayConnect returns a token and token expiration, when requested by the merchant's system, to be used instead
		///of actual credit card number in subsequent transactions.</summary>
		public string PayConnectToken;
		///<summary>Expiration for the PayConnect token.  Used with the PayConnect token instead of the actual credit card number and expiration.</summary>
		public DateTime PayConnectTokenExp;
		///<summary>What procedures will go on this card as a recurring charge.  Comma delimited list of ProcCodes.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string Procedures;
		///<summary>Enum:CreditCardSource Indicates which application made this credit card and token.</summary>
		public CreditCardSource CCSource;
		///<summary>FK to clinic.ClinicNum. The clinic where this card was added. Each clinic could have a different AuthKey and different
		///AuthKeys could generate overlapping tokens.</summary>
		public long ClinicNum;
		///<summary>Only used at OD HQ.  Excludes credit card from syncing default procedures.  False by default.</summary>
		public bool ExcludeProcSync;
		///<summary>Token for PaySimple.  PaySimple returns a token, when requested by the merchant's system, to be used instead
		///of actual credit card number in subsequent transactions.</summary>
		public string PaySimpleToken;

		public bool IsXWeb() {
			return CCSource==CreditCardSource.XWeb || CCSource==CreditCardSource.XWebPortalLogin;
		}

		///<summary></summary>
		public CreditCard Clone() {
			return (CreditCard)this.MemberwiseClone();
		}
	}

	public enum CreditCardSource {
		///<summary>0 - Storing the actual credit card number. Not recommended.</summary>
		None,
		///<summary>1 - Local installation of X-Charge</summary>
		XServer,
		///<summary>2 - Credit card created via X-Web (an eService)</summary>
		XWeb,
		///<summary>3 - PayConnect web service (from within OD).</summary>
		PayConnect,
		///<summary>4 - Credit card has been added through the local installation of X-Charge and the PayConnect web service.</summary>
		XServerPayConnect,
		///<summary>5 - Made from the login screen of the Patient Portal.</summary>
		XWebPortalLogin,
		///<summary>6 - PaySimple web service (from within OD).</summary>
		PaySimple,
	}
}
