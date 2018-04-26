﻿using System;
using System.Collections;
using System.Drawing;

namespace OpenDentBusiness {
	///<summary>For the orthochart feature, each row in this table is one cell in that grid.  An empty cell often corresponds to a missing db table row.</summary>
	[Serializable]
	public class OrthoChart:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long OrthoChartNum;
		///<summary>FK to patient.PatNum.</summary>
		public long PatNum;
		///<summary>Date of service.</summary>
		public DateTime DateService;
		///<summary>.</summary>
		public string FieldName;
		///<summary>.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string FieldValue;
		///<summary>FK to userod.UserNum.  The user that created or last edited an ortho chart field.</summary>
		public long UserNum;

		///<summary></summary>
		public OrthoChart Copy() {
			return (OrthoChart)this.MemberwiseClone();
		}
	}
}