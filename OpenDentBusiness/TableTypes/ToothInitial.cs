using System;
using System.Collections;
using System.Drawing;
using System.Xml.Serialization;

namespace OpenDentBusiness{

	/// <summary>Used to track missing teeth, primary teeth, movements, and drawings.</summary>
	[Serializable]
	public class ToothInitial:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long ToothInitialNum;
		///<summary>FK to patient.PatNum</summary>
		public long PatNum;
		///<summary>1-32 or A-Z. Supernumeraries not supported here yet.</summary>
		public string ToothNum;
		///<summary>Enum:ToothInitialType</summary>
		public ToothInitialType InitialType;
		///<summary>Shift in mm, or rotation / tipping in degrees.</summary>
		public float Movement;
		///<summary>Point data for a drawing segment.  The format would look similar to this: 45,68;48,70;49,72;0,0;55,88;etc.  It's simply a sequence of points, separated by semicolons.  Only positive numbers are used.  0,0 is the upper left of the tooth chart at original size.  Stored in pixels as originally drawn.  If we ever change the tooth chart, we will have to also keep an old version as an alternate to display old drawings.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string DrawingSegment;
		///<summary>.</summary>
		[XmlIgnore]
		public Color ColorDraw;

		///<summary>Used only for serialization purposes</summary>
		[XmlElement("ColorDraw",typeof(int))]
		public int ColorDrawXml {
			get {
				return ColorDraw.ToArgb();
			}
			set {
				ColorDraw=Color.FromArgb(value);
			}
		}

		///<summary></summary>
		public ToothInitial Copy(){
			return (ToothInitial)this.MemberwiseClone();
		}


	}

	




}

















