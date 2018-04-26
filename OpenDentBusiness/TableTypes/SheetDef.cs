using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Xml.Serialization;

namespace OpenDentBusiness{

	///<summary>A definition (template) for a sheet.  Can be pulled from the database, or it can be internally defined.</summary>
	[Serializable()]
	public class SheetDef:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long SheetDefNum;
		///<summary>The description of this sheetdef.</summary>
		public string Description;
		///<summary>Enum:SheetTypeEnum</summary>
		public SheetTypeEnum SheetType;
		///<summary>The default fontSize for the sheet.  The actual font must still be saved with each sheetField.</summary>
		public float FontSize;
		///<summary>The default fontName for the sheet.  The actual font must still be saved with each sheetField.</summary>
		public string FontName;
		///<summary>Width of each page in the sheet in pixels, 100 pixels per inch.</summary>
		public int Width;
		///<summary>Height of each page in the sheet in pixels, 100 pixels per inch.</summary>
		public int Height;
		///<summary>Set to true to print landscape.</summary>
		public bool IsLandscape;
		///<summary>Amount of editable space. Actual size when filling sheet may be different.</summary>
		public int PageCount;
		///<summary>If false, forces old single page behavior which ignores page breaks.</summary>
		public bool IsMultiPage;
		///<summary>Enum:BypassLockStatus Specifies whether a sheet can be created before the global lock date.</summary>
		public BypassLockStatus BypassGlobalLock;

		///<Summary>A collection of all parameters for this sheetdef.  There's usually only one parameter.  The first parameter will be a List long if it's a batch.  If a sheet has already been filled, saved to the database, and printed, then there is no longer any need for the parameters in order to fill the data.  So a retrieved sheet will have no parameters, signalling a skip in the fill phase.  There will still be parameters tucked away in the Field data in the database, but they won't become part of the sheet.</Summary>
		[CrudColumn(IsNotDbColumn=true)]
		[XmlIgnore]
		public List<SheetParameter> Parameters;
		///<Summary></Summary>
		[CrudColumn(IsNotDbColumn=true)]
		[XmlIgnore]
		public List<SheetFieldDef> SheetFieldDefs;

		///<summary>Vertical height per page taking into account the IsLandscape flag.</summary>
		public int HeightPage {
			get {
				if(IsLandscape) {
					return Width;
				}
				else {
					return Height;
				}
			}
		}

		///<summary>Horizontal width per page taking into account the IsLandscape flag.</summary>
		public int WidthPage {
			get {
				if(IsLandscape) {
					return Height;
				}
				else {
					return Width;
				}
			}
		}

		///<summary>Takes into account IsLandscape and PageCount. Sets the size of the editable area, This does not represent printed height, as some controls may grow.</summary>
		public int HeightTotal {
			get {
				//Math.Max in case PageCount=0;
				if(IsLandscape) {
					return Math.Max(Width,Width*PageCount);
				}
				else {
					return Math.Max(Height,Height*PageCount);
				}
			}
		}

		///<Summary></Summary>
		public Font GetFont(){
			return new Font(FontName,FontSize);
		}

		public SheetDef(){//required for use as a generic.
			PageCount=1;
		}

		public SheetDef(SheetTypeEnum sheetType){
			SheetType=sheetType;
			PageCount=1;
			Parameters=SheetParameter.GetForType(sheetType);
			SheetFieldDefs=new List<SheetFieldDef>();
		}

		public SheetDef Copy(){
			SheetDef sheetdef=(SheetDef)this.MemberwiseClone();
			//do I need to copy the lists?
			return sheetdef;
		}

		///<summary>Used only for serialization purposes</summary>
		[XmlElement("SheetFieldDefs",typeof(SheetFieldDef[]))]
		public SheetFieldDef[] SheetFieldDefsXml {
			get {
				if(SheetFieldDefs==null) {
					return new SheetFieldDef[0];
				}
				return SheetFieldDefs.ToArray();
			}
			set {
				SheetFieldDefs=new List<SheetFieldDef>();
				for(int i=0;i<value.Length;i++) {
					SheetFieldDefs.Add(value[i]);
				}
			}
		}

		
		
		

	}

	

	

}
