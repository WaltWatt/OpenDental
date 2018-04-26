﻿using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OpenDentBusiness{
	///<summary>One field on a sheet.</summary>
	[Serializable()]
	[CrudTable(IsSynchable=true,HasBatchWriteMethods=true)]
	public class SheetField:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long SheetFieldNum;
		///<summary>FK to sheet.SheetNum.</summary>
		public long SheetNum;
		///<summary>Enum:SheetFieldType  OutputText, InputField, StaticText,Parameter(only used for SheetField, not SheetFieldDef),Image,Drawing,Line,Rectangle,CheckBox,SigBox,PatImage.</summary>
		public SheetFieldType FieldType;
		///<summary><para>Mostly for OutputText and InputField types.  Each sheet typically has a main datatable type.</para>
		///<para>OutputText: FieldName is usually the string representation of the database column for the main table.  For other tables, it can be of the form table.Column.  There may also be extra fields available that are not strictly pulled from the database.  Extra fields will start with lowercase to indicate that they are not pure database fields.  The list of available fields for each type in SheetFieldsAvailable.  Users can pick from that list.  </para>
		///<para>InputField: are internally tied to actions to persist the data.  So they are also hard coded and are available in SheetFieldsAvailable.  </para>
		///<para>Static images: this is the full file name including extension, but without path.  Static images paths are reconstructed by looking in the AtoZ folder, SheetImages folder.  </para>
		///<para>PatImage: This is the DefNum for the document category. This is used in SheetFiller to find a document num, this is translated in FormSheetFillEdit to a human readable name. Example: 113 (if 113 is the defnum for photos). In FormSheetFillEdit,  this will display as PatImg:photos.</para>
		///<para>Parameter: the FieldName stores the name of the parameter.</para></summary>
		public string FieldName;
		///<summary><para>For OutputText, this value is set before printing.  This is the data obtained from the database and ready to print.  For StaticText, this is copied from the sheetFieldDef, but in-line fields like [this] will have been filled.  For an archived sheet retrieved from the database (all SheetField rows), this value will have been saved and will not be filled again automatically.</para>
		///<para>Parameter fieldtype: this will store the value of the parameter.</para>
		///<para>Drawing fieldtype: this will be the point data for the lines.  The format would look similar to this: 45,68;48,70;49,72;0,0;55,88;etc.  It's simply a sequence of points, separated by semicolons.</para>
		///<para>CheckBox: it will either be an X or empty.</para>
		///<para>SigBox: the first char will be 0 or 1 to indicate SigIsTopaz, and all subsequent chars will be the Signature itself.</para>
		///<para>PatImage: Docnum or blank, FK to document.DocNum.</para>
		///<para>ComboBox: The chosen option, semicolon, then a pipe delimited list of options such as: March;January|February|March|April</para>
		///<para>ScreenChart: Contains a semicolon delimited list of a single number followed by groups of comma separated surfaces.
		///The first digit represents what type of ScreenChart it is.  0 = Permanent, 1 = Primary
		///It may look like 0;S,P,N;S,S,S;... etc.</para></summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string FieldValue;
		///<summary>The fontSize for this field regardless of the default for the sheet.  The actual font must be saved with each sheetField.</summary>
		public float FontSize;
		///<summary>The fontName for this field regardless of the default for the sheet.  The actual font must be saved with each sheetField.</summary>
		public string FontName;
		///<summary>.</summary>
		public bool FontIsBold;
		///<summary>In pixels.</summary>
		public int XPos;
		///<summary>In pixels.</summary>
		public int YPos;
		///<summary>The field will be constrained horizontally to this size.  Not allowed to be zero.</summary>
		public int Width;
		///<summary>The field will be constrained vertically to this size.  Not allowed to be stored as 0.  It's not allowed to be zero so that it will be visible on the designer. Set to 0 in memory by SheetUtil.CalculateHeights if image is innacessible for printing.</summary>
		public int Height;
		///<summary>Enum:GrowthBehaviorEnum</summary>
		public GrowthBehaviorEnum GrowthBehavior;
		///<summary>This is only used for checkboxes that you want to behave like radiobuttons.  Set the FieldName the same for each Checkbox in the group.  The FieldValue will likely be X for one of them and empty string for the others.  Each of them will have a different RadioButtonValue.  Whichever box has X, the RadioButtonValue for that box will be used when importing.  This field is not used for "misc" radiobutton groups.</summary>
		public string RadioButtonValue;
		///<summary>Name which identifies the group within which the radio button belongs. FieldName must be set to "misc" in order for the group to take effect.</summary>
		public string RadioButtonGroup;
		///<summary>Set to true if this field is required to have a value before the sheet is closed.</summary>
		public bool IsRequired;
		///<summary>Tab stop order for all fields. Only checkboxes and input fields can have values other than 0.</summary>
		public int TabOrder;
		///<summary>Allows reporting on misc fields.</summary>
		public string ReportableName;
		///<summary>Text Alignment for text fields.</summary>
		public HorizontalAlignment TextAlign;
		/// <summary>If a sheet field is locked, it stops a user from editing the text when presented in a sheet.</summary>
		public bool IsLocked;
		///<summary>Text color, line color, rectangle color.</summary>
		[XmlIgnore]
		public Color ItemColor;
		///<summary>Used to store the key to display signature box when printing.  Not stored in DB.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public string SigKey;
		///<summary>DateTime that a sheet was signed.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateTimeSig;

		///<summary>Used only for serialization purposes</summary>
		[XmlElement("ItemColor",typeof(int))]
		public int ItemColorXml {
			get {
				return ItemColor.ToArgb();
			}
			set {
				ItemColor=Color.FromArgb(value);
			}
		}
				
		public SheetField Copy(){
			return (SheetField)this.MemberwiseClone();
		}

		public override string ToString() {
			return FieldName+" "+FieldValue;
		}

		///<Summary>Should only be called after FieldValue has been set, due to GrowthBehavior.</Summary>
		public Rectangle Bounds {
			get {
				return new Rectangle(XPos,YPos,Width,Height);
			}
		}

		///<Summary>Should only be called after FieldValue has been set, due to GrowthBehavior.</Summary>
		public RectangleF BoundsF {
			get {
				return new RectangleF(XPos,YPos,Width,Height);
			}
		}


	}
}


