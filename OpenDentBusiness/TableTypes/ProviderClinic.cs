using System;
using System.Collections;
using System.Drawing;

namespace OpenDentBusiness {
	///<summary>Only used if clinics enabled.  Allows the user to specify DEA number override for the provider at the specified clinic.</summary>
	[Serializable]
	[CrudTable(IsSynchable = true)]
	public class ProviderClinic:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long ProviderClinicNum;
		///<summary>FK to provider.ProvNum.</summary>
		public long ProvNum;
		///<summary>FK to clinic.ClinicNum.</summary>
		public long ClinicNum;
		///<summary>The DEA number for this provider and clinic.  The DEA number used to be stored in provider.DEANum.</summary>
		public string DEANum;

		///<summary></summary>
		public ProviderClinic Copy() {
			return (ProviderClinic)this.MemberwiseClone();
		}
	}
}

