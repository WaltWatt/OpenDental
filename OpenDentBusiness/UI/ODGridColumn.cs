using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace OpenDental.UI{ 

	///<summary></summary>
	//[DesignTimeVisible(false)]
	//[TypeConverter(typeof(GridColumnTypeConverter))]
	public class ODGridColumn{		
		private string _heading;
		private int _colWidth;
		private HorizontalAlignment _textAlign;
		private bool _isEditable;
		//private System.ComponentModel.Container components = null;
		private ImageList _imageList;
		private GridSortingStrategy _sortingStrategy;
		///<summary>When set, all cells in this column will display a combo box with these strings as options which the user can pick from.</summary>
		public List<string> ListDisplayStrings;
		///<summary>Set this to an event method and it will be used when the column header is clicked.</summary>
		public EventHandler CustomClickEvent;
		private object _tag;
		private int _dropDownWidth;

		///<summary>Creates a new ODGridcolumn.</summary>
		public ODGridColumn(){
			_heading="";
			_colWidth=80;
			_textAlign=HorizontalAlignment.Left;
			_isEditable=false;
			_imageList=null;
			_sortingStrategy=GridSortingStrategy.StringCompare;
		}

		///<summary>Creates a new ODGridcolumn with the given heading and width. Alignment left</summary>
		public ODGridColumn(string heading,int colWidth,HorizontalAlignment textAlign,bool isEditable) {
			_heading=heading;
			_colWidth=colWidth;
			_textAlign=textAlign;
			_isEditable=isEditable;
			_imageList=null;
			_sortingStrategy=GridSortingStrategy.StringCompare;
		}

		///<summary>Creates a new ODGridcolumn with the given heading and width. Alignment left</summary>
		public ODGridColumn(string heading,int colWidth,bool isEditable) {
			_heading=heading;
			_colWidth=colWidth;
			_isEditable=isEditable;
			_imageList=null;
			_sortingStrategy=GridSortingStrategy.StringCompare;
		}

		///<summary>Creates a new ODGridcolumn with the given heading and width.</summary>
		public ODGridColumn(string heading,int colWidth,HorizontalAlignment textAlign){
			_heading=heading;
			_colWidth=colWidth;
			_textAlign=textAlign;
			_imageList=null;
			_sortingStrategy=GridSortingStrategy.StringCompare;
		}

		///<summary>Creates a new ODGridcolumn with the given heading, width, and sorting strategy.</summary>
		public ODGridColumn(string heading,int colWidth,GridSortingStrategy sortingStrategy) {
			_heading=heading;
			_colWidth=colWidth;
			_textAlign=HorizontalAlignment.Left;
			_imageList=null;
			_sortingStrategy=sortingStrategy;
		}

		///<summary></summary>
		public ODGridColumn(string heading,int colWidth,GridSortingStrategy sortingStrategy,bool isEditable) {
			_heading=heading;
			_colWidth=colWidth;
			_textAlign=HorizontalAlignment.Left;
			_imageList=null;
			_sortingStrategy=sortingStrategy;
			_isEditable=isEditable;
		}

		///<summary>Creates a new ODGridcolumn with the given heading, width, and sorting strategy.</summary>
		public ODGridColumn(string heading,int colWidth,HorizontalAlignment textAlign,GridSortingStrategy sortingStrategy) {
			_heading=heading;
			_colWidth=colWidth;
			_textAlign=textAlign;
			_imageList=null;
			_sortingStrategy=sortingStrategy;
		}

		///<summary></summary>
		public ODGridColumn(string heading,int colWidth,HorizontalAlignment textAlign,GridSortingStrategy sortingStrategy,bool isEditable) {
			_heading=heading;
			_colWidth=colWidth;
			_textAlign=textAlign;
			_imageList=null;
			_sortingStrategy=sortingStrategy;
			_isEditable=isEditable;
		}

		///<summary>Creates a new ODGridcolumn with the given heading and width. Alignment left</summary>
		public ODGridColumn(string heading,int colWidth){
			_heading=heading;
			_colWidth=colWidth;
			_textAlign=HorizontalAlignment.Left;
			_imageList=null;
			_sortingStrategy=GridSortingStrategy.StringCompare;
		}
		
		///<summary>Creates a new ODGridcolumn with the given heading, width, and as a combo box type column with the options in listDisplayStrings.
		///On the grid make sure to set the SelectionMode to OneCell.</summary>
		public ODGridColumn(string heading,int colWidth,List<string> listDisplayStrings) {
			_heading=heading;
			_colWidth=colWidth;
			_sortingStrategy=GridSortingStrategy.StringCompare;
			ListDisplayStrings=listDisplayStrings;
		}

		///<summary>Creates a new ODGridcolumn with the given heading, width, and as a combo box type column with the options in listDisplayStrings.
		///On the grid make sure to set the SelectionMode to OneCell. Option to specify the drop down menu width used to display the listDisplayStrings.</summary>
		public ODGridColumn(string heading,int colWidth,List<string> listDisplayStrings,int dropDownWidth) {
			_heading=heading;
			_colWidth=colWidth;
			_sortingStrategy=GridSortingStrategy.StringCompare;
			ListDisplayStrings=listDisplayStrings;
			_dropDownWidth=dropDownWidth;
		}

		///<summary></summary>
		public string Heading{
			get{
				return _heading;
			}
			set{
				_heading=value;
			}
		}

		///<summary></summary>
		public int ColWidth{
			get{
				return _colWidth;
			}
			set{
				_colWidth=value;
			}
		}

	  ///<summary></summary>
		public HorizontalAlignment TextAlign{
			get{
				return _textAlign;
			}
			set{
				_textAlign=value;
			}
		}   
		
		///<summary></summary>
		public bool IsEditable{
			get {
				return _isEditable;
			}
			set {
				_isEditable=value;
			}
		}
	    
    ///<summary></summary>
		public ImageList ImageList{
			get {
				return _imageList;
			}
			set {
				_imageList=value;
			}
		}

		///<summary></summary>
		public GridSortingStrategy SortingStrategy{
			get {
				return _sortingStrategy;
			}
			set {
				_sortingStrategy=value;
			}
		}

		///<summary></summary>
		public object Tag {
			get {
				return _tag;
			}
			set {
				_tag=value;
			}
		}

		///<summary>The width of the combo box when it is dropped down.</summary>
		public int DropDownWidth {
			get {
				return _dropDownWidth;
			}
		}

		public ODGridColumn Copy() {
			ODGridColumn retVal=(ODGridColumn)this.MemberwiseClone();
			if(this.ListDisplayStrings!=null) {
				retVal.ListDisplayStrings=this.ListDisplayStrings.Select(x => new string(x.ToArray())).ToList();
			}
			return retVal;
		}

	}








	public enum GridSortingStrategy {
		///<summary>0- Default</summary>
		StringCompare,
		DateParse,
		ToothNumberParse,
		AmountParse,
		TimeParse,
		VersionNumber,
	}
}






