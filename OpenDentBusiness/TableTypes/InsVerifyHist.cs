using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDentBusiness {
	///<summary>Inherits from insverify. A historical copy of an insurance verification record.</summary>
	[Serializable]
	public class InsVerifyHist:InsVerify {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long InsVerifyHistNum;
		///<summary>FK to userod.UserNum.  User that was logged on when row was inserted.</summary>
		public long VerifyUserNum;
		
		public InsVerifyHist() {
		}

		public InsVerifyHist(InsVerify insVerify) {
			this.InsVerifyNum=insVerify.InsVerifyNum;
			this.DateLastVerified=insVerify.DateLastVerified;
			this.UserNum=insVerify.UserNum;
			this.VerifyType=insVerify.VerifyType;
			this.FKey=insVerify.FKey;
			this.DefNum=insVerify.DefNum;
			this.Note=insVerify.Note;
			this.DateLastAssigned=insVerify.DateLastAssigned;
			this.DateTimeEntry=insVerify.DateTimeEntry;
			this.HoursAvailableForVerification=insVerify.HoursAvailableForVerification;
			this.VerifyUserNum=Security.CurUser.UserNum;
		}

		///<summary></summary>
		public InsVerifyHist Copy() {
			return (InsVerifyHist)this.MemberwiseClone();
		}
	}
}
