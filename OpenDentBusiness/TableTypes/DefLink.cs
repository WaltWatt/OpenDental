﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness {
	///<summary></summary>
	[Serializable]
	[CrudTable(IsSynchable = true)]
	public class DefLink:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long DefLinkNum;
		///<summary>FK to definition.DefNum. The definition that is linked to </summary>
		public long DefNum;
		///<summary>A foreign key to a table associated with the DefLinkType. 
		///Uses include:  ClinicNum with DefLinkType Clinic, PatNum with DefLinkType Patient.</summary>
		public long FKey;
		///<summary>Enum:DefLinkType The type of link.</summary>
		public DefLinkType LinkType;

		///<summary></summary>
		public DefLink Copy() {
			return (DefLink)this.MemberwiseClone();
		}
	}

	///<summary>The manner in which a definition is linked.</summary>
	public enum DefLinkType {
		///<summary>0. The definition is linked to a clinic.</summary>
		Clinic,
		///<summary>1. The definition is linked to a patient.</summary>
		Patient,
		///<summary>2. The definition is linked to an appointment type.</summary>
		AppointmentType,
	}

	///<summary>Helper class for the DefLink table.  This class resides within DefLink.cs</summary>
	public class DefLinkClinic {
		public Clinic Clinic;
		public List<DefLink > ListDefLink;
		public DefLinkClinic(Clinic clinic,List<DefLink> listDefLink) {
			Clinic=clinic;
			ListDefLink=listDefLink;
		}
	}	
}
