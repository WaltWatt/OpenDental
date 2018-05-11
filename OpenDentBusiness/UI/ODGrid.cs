using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Windows.Forms;
using OpenDentBusiness;
using PdfSharp.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CodeBase;
using System.IO;
using System.Net.Mail;

namespace OpenDental.UI {
	///<summary></summary>
	public delegate void ODGridClickEventHandler(object sender,ODGridClickEventArgs e);

	///<summary>A new and improved grid control to replace the inherited ContrTable that is used so extensively in the program.</summary>
	[DefaultEvent("CellDoubleClick")]
	public class ODGrid:System.Windows.Forms.UserControl {
		///<summary>Required designer variable.</summary>
		private System.ComponentModel.Container components = null;
		private ODGridColumnCollection columns;
		///<summary></summary>
		[Category("Action"),Description("Occurs when a cell is double clicked.")]
		public event ODGridClickEventHandler CellDoubleClick=null;
		///<summary></summary>
		[Category("Action"),Description("Occurs when a combo box item is selected.")]
		public event ODGridClickEventHandler CellSelectionCommitted=null;
		///<summary></summary>
		[Category("Action"),Description("Occurs when a cell is single clicked.")]
		public event ODGridClickEventHandler CellClick=null;
		///<summary></summary>
		[Category("Property Changed"),Description("Event used when cells are editable.  The TextChanged event is passed up from the textbox where the editing is taking place.")]
		public event EventHandler CellTextChanged=null;
		///<summary></summary>
		[Category("Focus"),Description("Event used when cells are editable.  LostFocus event is passed up from the textbox where the editing is taking place.")]
		public event ODGridClickEventHandler CellLeave=null;
		[Category("Focus"),Description("Event used when cells are editable.  GotFocus event is passed up from the textbox where the editing is taking place.")]
		public event ODGridClickEventHandler CellEnter=null;
		///<summary></summary>
		[Category("Action"),Description("Occurs when rows are selected or unselected by the user for any reason, including mouse and keyboard clicks.  Only works for GridSelectionModes.One for now (enhance later).  Excludes programmatic selection.")]
		public event EventHandler OnSelectionCommitted=null;
		///<summary></summary>
		[Category("Action"),Description("If HasAddButton is true, this event will fire when the add button is clicked.")]
		public event EventHandler TitleAddClick=null;
		///<summary></summary>
		[Category("Action"),Description("If AllowSortingByColumn is true, this event will fire when a column header is clicked and the grid is sorted.  "
			+"Used to reselect rows after sorting.")]
		public event EventHandler OnSortByColumn=null;
		private string title;
		private Font _titleFont=new Font(FontFamily.GenericSansSerif,10,FontStyle.Bold);
		private Font _headerFont=new Font(FontFamily.GenericSansSerif,8.5f,FontStyle.Bold);
		private Font _cellFont=new Font(FontFamily.GenericSansSerif,8.5f);
		public Font FontForSheets;
		protected int titleHeight=18;
		private int headerHeight=15;
		private Color cGridLine=Color.FromArgb(180,180,180);
		private System.Windows.Forms.VScrollBar vScroll;
		private System.Windows.Forms.HScrollBar hScroll;
    ///<summary>Always contains all of the rows in the grid, even if they are hidden.</summary>
    private ODGridRowCollection rows;
		///<summary>If HasDropDowns is true, this contains the rows that are currently visible in the grid. Otherwise it's empty.
		///ComputeFilterRows is the ONLY method that should ever add/modify rows in _rowsFiltered.</summary>
		private ODGridRowCollection _rowsFiltered;
    private bool IsUpdating;
		///<summary>The total height of the grid.</summary>
		private int GridH;
		///<summary>The total width of the grid.</summary>
		private int GridW;
		private bool hScrollVisible;
		///<summary>Set at the very beginning of OnPaint.  Uses the column width of each column to set up this array with one element for each column.  Contains the columns Pos for that column.</summary>
		private int[] ColPos;
		private ArrayList selectedIndices;
		private Point selectedCell;
		private int MouseDownRow;
		private int MouseDownCol;
		public bool ControlIsDown;//Used in ComboBoxMulti.cs
		private bool ShiftIsDown;
		//private bool UpDownKey;
		private GridSelectionMode selectionMode;
		private bool MouseIsDown;
		private bool _mouseIsOver;
		public bool MouseIsOver {
			get {
				return _mouseIsOver;
			}
		}
		private string translationName;
		private Color selectedRowColor;
		private bool allowSelection;
		private bool wrapText;
		private int noteSpanStart;
		private int noteSpanStop;
		private TextBoxBase editBox;
		private ComboBox comboBox;
		private MouseButtons lastButtonPressed;
		private ArrayList selectedIndicesWhenMouseDown;
		private bool allowSortingByColumn;
		private bool mouseIsDownInHeader;
		///<summary>Typically -1 to show no triangle.  Or, specify a column to show a triangle.  The actual sorting happens at mouse down.</summary>
		private int sortedByColumnIdx;
		///<summary>True to show a triangle pointing up.  False to show a triangle pointing down.  Only works if sortedByColumnIdx is not -1.</summary>
		private bool sortedIsAscending;
		//private List<List<int>> multiPageNoteHeights;//If a row's NoteHeight won't fit on one page, get various page heights here (printing).
		//private List<List<string>> multiPageNoteSection;//Section of the note that is split up across multiple pages.
		private int RowsPrinted;
		///<summary>If we are part way through drawing a note when we reach the end of a page, this will contain the remainder of the note still to be printed.  If it is empty string, then we are not in the middle of a note.</summary>
		private string NoteRemaining;
		private Point SelectedCellOld;
		///<summary>Is set when ComputeRows is called, then used . If any columns are editable HasEditableColumn is true.</summary>
		private bool HasEditableColumn;
		///<summary></summary>
		private const int EDITABLE_ROW_HEIGHT=19;
		private bool editableAcceptsCR;
		private static bool _useBlueTheme;
		private StringFormat _format;
		public bool HideScrollBars=false;//used when printing grids to sheets
		///<summary>Currently only used for printing on sheets.</summary>
		public List<ODPrintRow> PrintRows;
		///<summary>Used when calculating printed row positions.  Set to 0 when using in FormSheetFillEdit.</summary>
		public int TopMargin;
		///<summary>Used when calculating printed row positions.  Set to 0 when using in FormSheetFillEdit.</summary>
		public int BottomMargin;
		public int PageHeight;
		///<summary>(Printing Only) The position on the page that this grid will print. If this is halfway down the second page, 1100px tall, this value should be 1650, not 550.</summary>
		public int YPosField;
		///<summary>Height of field when printing.  Set using CalculateHeights() from EndUpdate()</summary>
		private int _printHeight;
		private bool _hasMultilineHeaders;
		private bool _hasAddButton;
		///<summary>Used to "gray out" AddButton when functionality should be disabled.  Only affects UI; disabling functionality must be implemented in event handler.</summary>
		private bool _isAddButtonEnabled=true;
		/// <summary>Truncates the note to this many characters.
		///Allen approved 3/16/2018 reducing from 200,000 to 30,000 it was found that UEs could occur at lengths greater than 32,000</summary>
		private int _noteLengthLimit=30000;
		private List<MenuItem> _listMenuItemLinks;
		private Point _currentClickLocation;
		private bool _hasDistinctClickEvents;
		private bool _hasLinkDetect;
		private bool _hasEditableRTF;
		///<summary>Thread to read inputs on grid. Currently only used for mouse click logic.</summary>
		private ODThread _threadDistinctClickEvents=null;
		///<summary>Set to -1 if no mouse down event has happened recently.  Otherwise set to the row the single click event occurred at.
		///Only used for click logic.  See MouseDownRow also.</summary>
		private int _mouseClickIdx=-1;
		///<summary>Only used for mouse click logic.</summary>
    private int _mouseClickCount=0;
		///<summary>Only used for mouse click logic.  The time which the first mouse click occurred so we can determine if double clicking.</summary>
		private DateTime _dateTimeMouseClick=DateTime.MinValue;
		///<summary>Regular expresion used to help identify URLs in the grid.
		///This is not all encompassing, there will be URLs that do not match this but this should work for 99%.</summary>
		private const string URL_REGEX_STRING=@"(http://|https://)?[-a-zA-Z0-9@:%._\\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=,()]*)";
		private bool _hasDropDowns = false;
		///<summary>HasMultiLineHeaders must be turned on for this to work.</summary>
		private bool _hasAutoWrappedHeaders= false;
		///<summary>These widths will only vary from the columns[i].ColWidth when ColWidth is 0 or negative (dynamic).</summary>
		private List <int> _listCurColumnWidths=new List<int>();

		///<summary></summary>
		public ODGrid() {
			//InitializeComponent();// Required for Windows.Forms Class Composition Designer support
			//Add any constructor code after InitializeComponent call
			_mouseIsOver=false;
			columns=new ODGridColumnCollection();
      rows=new ODGridRowCollection();
      _rowsFiltered=new ODGridRowCollection();
      vScroll=new VScrollBar();
			vScroll.Scroll+=new ScrollEventHandler(vScroll_Scroll);
			vScroll.MouseEnter+=new EventHandler(vScroll_MouseEnter);
			vScroll.MouseLeave+=new EventHandler(vScroll_MouseLeave);
			vScroll.MouseMove+=new MouseEventHandler(vScroll_MouseMove);
			hScroll=new HScrollBar();
			hScroll.Scroll+=new ScrollEventHandler(hScroll_Scroll);
			hScroll.MouseEnter+=new EventHandler(hScroll_MouseEnter);
			hScroll.MouseLeave+=new EventHandler(hScroll_MouseLeave);
			hScroll.MouseMove+=new MouseEventHandler(hScroll_MouseMove);
			this.Controls.Add(vScroll);
			this.Controls.Add(hScroll);
			selectedIndices=new ArrayList();
			selectedCell=new Point(-1,-1);
			selectionMode=GridSelectionMode.One;
			selectedRowColor=Color.Silver;
			allowSelection=true;
			_hasLinkDetect=true;
			wrapText=true;
			noteSpanStart=0;
			noteSpanStop=0;
			sortedByColumnIdx=-1;
			float[] arrayTabStops={50.0f};
			_format=new StringFormat();
			_format.SetTabStops(0.0f,arrayTabStops);
		}

		///<summary>Clean up any resources being used.</summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(components != null) {
					components.Dispose();
				}
				if(_format!=null) {
					_format.Dispose();
					_format=null;
				}
				QuitThread();
			}
			base.Dispose(disposing);
		}

		private void QuitThread() {
			if(_threadDistinctClickEvents!=null) {
				_threadDistinctClickEvents.QuitAsync();
				_threadDistinctClickEvents=null;
			}
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			components = new System.ComponentModel.Container();
		}
		#endregion

		///<summary></summary>
		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);
			if(this.Parent!=null) {
				this.Parent.MouseWheel+=new MouseEventHandler(Parent_MouseWheel);
				this.Parent.KeyDown+=new KeyEventHandler(Parent_KeyDown);
				this.Parent.KeyUp+=new KeyEventHandler(Parent_KeyUp);
			}
			if(this.ParentForm!=null) {
				this.ParentForm.FormClosing+=ODGrid_ParentFormClosing;
			}
		}

		private void ODGrid_ParentFormClosing(object sender,EventArgs e) {
			QuitThread();
		}

		///<summary></summary>
		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
			LayoutScrollBars();
			if(!hScrollVisible) {
				BeginUpdate();
				EndUpdate(false,true);//Force rows heights to be recalculated.
			}
			Invalidate();
		}

		public void AddRow(params string[] cells) {
			ODGridRow row = new ODGridRow();
			foreach(string cell in cells) {
				row.Cells.Add(cell);
			}
			Rows.Add(row);
		}

		//public void AddRow(params ODGridCell[] cells) {
		//	ODGridRow row = new ODGridRow();
		//	foreach(ODGridCell cell in cells) {
		//		row.Cells.Add(cell);
		//	}
		//	Rows.Add(row);
		//}

		#region Properties

		[Category("Appearance"),Description("Sets a font for the Title")]
		public Font TitleFont {
			get {
				return _titleFont;
			}
			set {
				_titleFont=value;
			}
		}
		
		[Category("Appearance"),Description("Sets a font for Headers")]
		public Font HeaderFont {
			get {
				return _headerFont;
			}
			set {
				_headerFont=value;
			}
		}

		[Category("Appearance"),Description("Sets a font for Cells")]
		public Font CellFont {
			get {
				return _cellFont;
			}
			set {
				_cellFont=value;
			}
		}

		public int TitleHeight{
			get {
				return titleHeight;
			}
			set {
				titleHeight=value;
			}
		}

		public int HeaderHeight {
			get {
				return headerHeight;
			}
			set {
				headerHeight=value;
			}
		}

		///<summary>Height of field when printing.  Set using CalculateHeights() from EndUpdate()</summary>
		public int PrintHeight {
			get {
				return _printHeight;
			}
		}

		///<summary>Allow Headers to be multiple lines tall.</summary>
		[Category("Appearance"),Description("Set true to allow new line characters in column headers.")]
		public bool HasMultilineHeaders {
			get {
				return _hasMultilineHeaders;
			}
			set {
				_hasMultilineHeaders=value;
				if(!_hasMultilineHeaders) {
					_hasAutoWrappedHeaders=false;
				}
			}
		}

		///<summary></summary>
		[Category("Appearance"),Description("Set to true to show an add button on the right side of the title bar.")]
		//[NotifyParentProperty(true),RefreshProperties(RefreshProperties.Repaint)]
		public bool HasAddButton {
			get {return _hasAddButton;}
			set {
				_hasAddButton=value;
				Refresh();
			}
		}

		public bool GetIsAddButtonEnabled(){
			return _isAddButtonEnabled;
		}

		public void SetAddButtonEnabled(bool enable){
			_isAddButtonEnabled=enable;
			Refresh();
		}

		///<summary>Gets the collection of ODGridColumns assigned to the ODGrid control.</summary>
		//[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		//[Editor(typeof(System.ComponentModel.Design.CollectionEditor),typeof(System.Drawing.Design.UITypeEditor))]
		//[Browsable(false)]//only because MS is buggy.
		public ODGridColumnCollection Columns {
			get {
				return columns;
			}
		}

    ///<summary>Gets the collection of ODGridRows assigned to the ODGrid control.</summary>
    [Browsable(false)]
    public ODGridRowCollection Rows {
      get {
        return rows;
      }
    }

    ///<summary>If HasDropDowns is true, this list will only contain the rows that are currently displayed; any hidden rows due to drop downs are excluded.
		///Otherwise, if HasDropDowns is false, returns all Rows.
    ///Do NOT call Add(), Clear(), or Insert() on this list - use Rows instead.</summary>
    [Browsable(false)]
    public ODGridRowCollection RowsFiltered {
      get {
				if(!_hasDropDowns) {
					return rows;
				}
        ODGridRowCollection retVal = new ODGridRowCollection();
        foreach(ODGridRow rowCur in _rowsFiltered) {
          retVal.Add(rowCur);
        }
        return retVal;
      }
		}

		///<summary>Returns the list of rows selected instead of a list of indices.</summary>
		[Browsable(false)]
		public List<ODGridRow> SelectedGridRows {
			get {
				if(SelectionMode==GridSelectionMode.OneCell) {
					return new List<ODGridRow>() { RowsFiltered[selectedCell.Y] };
				}
				else {
					return selectedIndices.ToArray().Select(x => RowsFiltered[(int)x]).ToList();
				}
			}
		}


		///<summary>Set to true if you want to allow rows to dropdown other rows.
		///If true, add all of your rows to Rows like normal, but use RowsFiltered to access your list of rows (when registering click events, selectedIndices, etc).
		///You should leave enough space in the row's first cell to display a drop down arrow.</summary>
		[Category("Behavior"), Description("Set true to enable row drop downs.  Rows that can drop down must have a parent row set.")]
		public bool HasDropDowns {
			get {
				return _hasDropDowns;
			}
			set {
				_hasDropDowns=value;
			}
		}

		///<summary>The title of the grid which shows across the top.</summary>
		[Category("Appearance"),Description("The title of the grid which shows across the top.")]
		public string Title {
			get {
				return title;
			}
			set {
				title=value;
				Invalidate();
			}
		}

		///<summary>Set true to show a horizontal scrollbar.  Vertical scrollbar always shows, but is disabled if not needed.  If hScroll is not visible, then grid will auto reset width to match width of columns.</summary>
		[Category("Appearance"),Description("Set true to show a horizontal scrollbar.")]
		public bool HScrollVisible {
			get {
				return hScrollVisible;
			}
			set {
				hScrollVisible=value;
				LayoutScrollBars();
				Invalidate();
			}
		}

		///<summary>The index of the row that is the first row displayed on the ODGrid. Also sets ScrollValue.</summary>
		public void ScrollToIndex(int index) {
			if(index>Rows.Count || index < 0) {
				return;
			}
			ScrollValue=rows[index].RowLoc;
		}

		///<summary>The index of the row that is the last row to be displayed on the ODGrid. Also sets ScrollValue.</summary>
		public void ScrollToIndexBottom(int index) {
			if(index>Rows.Count) {
				return;
			}
			ScrollValue=((rows[index].RowLoc+rows[index].RowHeight+rows[index].NoteHeight+titleHeight+headerHeight)-Height)+3;//+3 accounts for the grid lines.
		}

		///<summary>Gets or sets the position of the vertical scrollbar.  Does all error checking and invalidates.</summary>
		[Browsable(false)]
		public int ScrollValue {
			get {
				return vScroll.Value;
			}
			set {
				if(!vScroll.Enabled) {
					return;
				}
				int scrollValue=0;
				if(value>vScroll.Maximum-vScroll.LargeChange){
					scrollValue=vScroll.Maximum-vScroll.LargeChange;
				}
				else if(value<vScroll.Minimum) {
					scrollValue=vScroll.Minimum;
				}
				else {
					scrollValue=value;
				}
				try {
					vScroll.Value=scrollValue;
				}
				catch(Exception e) {//This should never ever happen.
					//Showing a messagebox is NOT our normal way of handling errors on mouse events, but the user would get a popup for the unhandled exception, anyway.
					MessageBox.Show("Error: Invalid Scroll Value. \r\n"
						+"Scroll value from: "+vScroll.Value+"\r\n"
						+"Scroll value to: "+scrollValue+"\r\n"
						+"Min scroll value: "+vScroll.Minimum+"\r\n"
						+"Max scroll value: "+vScroll.Maximum+"\r\n"
						+"Large change value: "+vScroll.LargeChange+"\r\n\r\n"
						+e.ToString());
					vScroll.Value=vScroll.Minimum;
				}
				if(editBox!=null) {
					editBox.Dispose();
				}
				Invalidate();
			}
		}

		protected override void OnSizeChanged(EventArgs e) {
			base.OnSizeChanged(e);
			ScrollValue=vScroll.Value;
		}

		///<summary>Holds the int values of the indices of the selected rows.  To set selected indices, use SetSelected().</summary>
		[Browsable(false)]
		public int[] SelectedIndices {
			get {
				if(SelectionMode==GridSelectionMode.OneCell) {
					return (selectedCell.Y==-1 ? new int[0] : new int[] { selectedCell.Y });
				}
				else {
					int[] retVal=new int[selectedIndices.Count];
					selectedIndices.CopyTo(retVal);
					Array.Sort(retVal);//they must be in numerical order
					return retVal;
				}
			}
		}

		///<summary>Holds the x,y values of the selected cell if in OneCell mode.  -1,-1 represents no cell selected.</summary>
		[Browsable(false)]
		public Point SelectedCell {
			get {
				return selectedCell;
			}
		}

		///<summary></summary>
		[Category("Behavior"),Description("Just like the listBox.SelectionMode, except no MultiSimple, and added OneCell.")]
		[DefaultValue(typeof(GridSelectionMode),"One")]
		public GridSelectionMode SelectionMode {
			get {
				return selectionMode;
			}
			set {
				//if((GridSelectionMode)value==SelectionMode.MultiSimple){
				//	MessageBox.Show("MultiSimple not supported.");
				//	return;
				//}
				if((GridSelectionMode)value==GridSelectionMode.OneCell) {
					selectedCell=new Point(-1,-1);//?
					selectedIndices=new ArrayList();
				}
				selectionMode=value;
			}
		}

		///<summary></summary>
		[Category("Behavior"),Description("Set false to disable row selection when user clicks.  Row selection should then be handled by the form using the cellClick event.")]
		[DefaultValue(true)]
		public bool AllowSelection {
			get {
				return allowSelection;
			}
			set {
				allowSelection=value;
			}
		}

		///<summary>Uniquely identifies the grid for translation to another language.</summary>
		[Category("Appearance"),Description("Uniquely identifies the grid for translation to another language.")]
		public string TranslationName {
			get {
				return translationName;
			}
			set {
				translationName=value;
			}
		}

		///<summary>The background color that is used for selected rows.</summary>
		[Category("Appearance"),Description("The background color that is used for selected rows.")]
		[DefaultValue(typeof(Color),"Silver")]
		public Color SelectedRowColor {
			get {
				return selectedRowColor;
			}
			set {
				selectedRowColor=value;
			}
		}

		///<summary>The background color that is used for selected rows.</summary>
		[Browsable(false)]
		public Color HeaderColor {
			get {
				if(_useBlueTheme) {
					return Color.FromArgb(223,234,245);//208,225,242);//166,185,204);
				}
				return Color.FromArgb(210,210,210);
			}
		}

		///<summary>Text within each cell will wrap, making some rows taller.</summary>
		[Category("Behavior"),Description("Text within each cell will wrap, making some rows taller.")]
		[DefaultValue(true)]
		public bool WrapText {
			get {
				return wrapText;
			}
			set {
				wrapText=value;
			}
		}

		///<summary>The starting column for notes on each row.  Notes are not part of the main row, but are displayed on subsequent lines.</summary>
		[Category("Appearance"),Description("The starting column for notes on each row.")]
		[DefaultValue(0)]//typeof(int),0)]
		public int NoteSpanStart {
			get {
				return noteSpanStart;
			}
			set {
				noteSpanStart=value;
			}
		}

		///<summary>The starting column for notes on each row.  Notes are not part of the main row, but are displayed on subsequent lines.  If this remains 0, then notes will be entirey skipped for this grid.</summary>
		[Category("Appearance"),Description("The ending column for notes on each row.")]
		[DefaultValue(0)]//typeof(int),0)]
		public int NoteSpanStop {
			get {
				return noteSpanStop;
			}
			set {
				noteSpanStop=value;
			}
		}

		///<summary>Deprecated, use dynamic columns instead.
		///Used when drawing to PDF. We need the width of all fixed width columns summed.
		///Ignores dynamic columns (non-positive width)</summary>
		public int WidthAllColumns {
			get {
				int retVal=0;
				for(int i=0;i<columns.Count;i++) {
					if(columns[i].ColWidth > 0) {
						retVal+=columns[i].ColWidth;
					}
				}
				return retVal;
			}
		}

		///<summary>Set true to allow user to click on column headers to sort rows, alternating between ascending and descending.</summary>
		[Category("Behavior"),Description("Set true to allow user to click on column headers to sort rows, alternating between ascending and descending.")]
		[DefaultValue(false)]
		public bool AllowSortingByColumn {
			get {
				return allowSortingByColumn;
			}
			set {
				allowSortingByColumn=value;
				if(!allowSortingByColumn) {
					sortedByColumnIdx=-1;
				}
			}
		}

		[Category("Behavior"),Description("Set true to use RichTextBoxes for editable cells")]
		[DefaultValue(false)]
		public bool EditableUsesRTF {
			get {
				return _hasEditableRTF;
			}
			set {
				_hasEditableRTF=value;
			}
		}

		///<summary>Only affects grids with editable columns. True allows carriage returns within cells. Falses causes carriage returns to go to the next editable cell.</summary>
		[Category("Behavior"),Description("Set true to allow editable cells to accept carriage returns.")]
		[DefaultValue(false)]
		public bool EditableAcceptsCR {
			get {
				return editableAcceptsCR;
			}
			set {
				editableAcceptsCR=value;
			}
		}

		///<summary>Container sets this for all grids simultaneously.</summary>
		[Browsable(false)]
		public static bool UseBlueTheme {
			get {
				return _useBlueTheme;
			}
			set {
				_useBlueTheme=value;
			}
		}

		///<summary>Returns current sort order.  Used to maintain current grid sorting when refreshing the grid.</summary>
		[Browsable(false)]
		public bool SortedIsAscending {
			get {
				return sortedIsAscending;
			}
		}

		///<summary>Returns current sort column index.  Used to maintain current grid sorting when refreshing the grid.</summary>
		[Browsable(false)]
		public int SortedByColumnIdx {
			get {
				return sortedByColumnIdx;
			}
		}

		[Category("Behavior"),Description("Set true to force mutual exclusion of OnClick and OnDoubleClick events.")]
		[DefaultValue(false)]
		public bool HasDistinctClickEvents {
			get {
				return _hasDistinctClickEvents;
			}
			set {
				_hasDistinctClickEvents=value;
			}
		}

		[Category("Behavior"), Description("Set false to disallow links from being automatically added to right click menus.")]
		[DefaultValue(true)]
		public bool HasLinkDetect {
			get {
				return _hasLinkDetect;
			}
			set {
				_hasLinkDetect=value;
			}
		}

		[Category("Appearance"), Description("Set to true to wrap headers automatically if the length of the header text is longer than the column."
			+" HasMultiLineHeaders must be set to true for this to work.")]
		[DefaultValue(false)]
		public bool HasAutoWrappedHeaders {
			get {
				return _hasAutoWrappedHeaders;
			}
			set {
				_hasAutoWrappedHeaders=value;
				if(value) {
					HasMultilineHeaders=true;
				}
			}
		}

		#endregion Properties

		#region Computations
		///<summary>Computes the position of each column and the overall width.  Called from endUpdate and also from OnPaint.</summary>
		private void ComputeColumns() {
			_listCurColumnWidths.Clear();
			foreach(ODGridColumn col in columns) {//Default to assuming fixed width columns.  If any are dynamic (0 or negative), then corrected below.
				_listCurColumnWidths.Add(col.ColWidth);
			}
			//Automatic sizing does not happen if horizontal scroll visible, because scrolling allows user to see grid of any width no matter how large.
			if(!hScrollVisible) {//This used to be in the resize logic.
				List <int> listDynamicColumnIndices=new List<int>();
				int dynamicColumnWeightTotal=0;//Will be a negative number if there is at least one dynamic column.
				for(int i=0;i<columns.Count;i++) {//Figure out which columns are dynamic.
					if(columns[i].ColWidth<=0) {//Column is dynamic.
						listDynamicColumnIndices.Add(i);
						dynamicColumnWeightTotal+=Math.Min(columns[i].ColWidth,-1);//0 weight counts as a -1.
					}
				}
				if(listDynamicColumnIndices.Count==0) {//No dynamic columns.  Force last column to automatically resize.
					//For backwards compatibility.  We always used to resize the last column automatically no matter what the ColWidth of the column was.
					//For this reason, most of the time the last colum had a ColWidth of 0, which now will be treated as a dynamic column anyway.
					//For those cases where all ColWidths are positive, this rule will kick in and automatically size the last column.
					listDynamicColumnIndices.Add(columns.Count-1);
					dynamicColumnWeightTotal=-1;
				}
				int minGridW=0;//Sum of all columns widths, excluding dynamic columns.
				for(int i=0;i<columns.Count;i++) {
					if(!listDynamicColumnIndices.Contains(i)) {
						minGridW+=columns[i].ColWidth;
					}
				}
				int minimumWidth=minGridW+2+vScroll.Width+5;
				bool isTooNarrow=(this.Width < minimumWidth);
				if(isTooNarrow) {
					vScroll.Visible=false;//When the grid is very narrow, we would rather show more of the actual columns instead of the scrollbar.
				}
				else {
					vScroll.Visible=true;
				}
				if(!isTooNarrow && columns.Count > 0 && !HideScrollBars) {//Set the widths of the weighted dynamic columns automatically.
					decimal variableWidthTotal=Width-2-vScroll.Width-minGridW;
					decimal widthUsedTotal=0;
					for(int i=0;i<listDynamicColumnIndices.Count;i++) {
						int index=listDynamicColumnIndices[i];
						_listCurColumnWidths[index]=(int)Math.Floor((Math.Min(columns[index].ColWidth,-1)*variableWidthTotal)/dynamicColumnWeightTotal);
						widthUsedTotal+=_listCurColumnWidths[index];
					}
					//There may be a remainder of unsed pixes in the width due to the Floor operation above.  Should be less than the number of dynamic columns.
					decimal remainderWidth=variableWidthTotal-widthUsedTotal;
					//Loop through and distribute the remainin pixels to the varaible width columns to completely use the total variable width.
					int colIdx=0;
					while(remainderWidth > 0) {
						int index=listDynamicColumnIndices[colIdx];
						_listCurColumnWidths[index]++;//Add one pixel to the current column from the remainder.
						remainderWidth--;//Deduct from the remainder so we know how many we have left to distribute.
						colIdx=(colIdx+1)%listDynamicColumnIndices.Count;//Mod just in case we wrap around (should not happen).
					}
				}
			}
			ColPos=new int[columns.Count];
			for(int i=0;i<ColPos.Length;i++) {
				if(i==0)
					ColPos[i]=0;
				else
					ColPos[i]=ColPos[i-1]+_listCurColumnWidths[i-1];
			}
			if(columns.Count>0) {
				GridW=ColPos[ColPos.Length-1]+_listCurColumnWidths[columns.Count-1];
			}
		}

		///<summary>Called from PrintPage() and EndUpdate().  After adding rows to the grid, this calculates the height of each row because some rows may have text wrap and will take up more than one row.  Also, rows with notes, must be made much larger, because notes start on the second line.  If column images are used, rows will be enlarged to make space for the images.</summary>
		private void ComputeRows(Graphics g,bool IsForPrinting=false) {
			Font tempFont=new Font(CellFont,CellFont.Style);
			if(FontForSheets!=null) {
				tempFont=new Font(FontForSheets,FontStyle.Regular);
			}
			using(Font cellFontBold=new Font(tempFont,FontStyle.Bold))
			using(Font cellFontNormal=new Font(tempFont,FontStyle.Regular)) {
				//multiPageNoteHeights=new List<List<int>>();
				//multiPageNoteSection=new List<List<string>>();
				//for(int i=0;i<rows.Count;i++) {
					//List<int> intList=new List<int>();
					//multiPageNoteHeights.Add(intList);
					//List<string> stringList=new List<string>();
					//multiPageNoteSection.Add(stringList);
				//}
				GridH=0;
				int cellH;
				int noteW=0;
				if(NoteSpanStop>0 && NoteSpanStart<columns.Count) {
					for(int i=NoteSpanStart;i<=NoteSpanStop;i++) {
						noteW+=_listCurColumnWidths[i];
					}
				}
				int imageH=0;
				HasEditableColumn=false;
				for(int i=0;i<columns.Count;i++) {
					if(columns[i].IsEditable || columns[i].ListDisplayStrings != null) {
						HasEditableColumn=true;
					}
					if(columns[i].ImageList!=null) {
						if(columns[i].ImageList.ImageSize.Height>imageH) {
							imageH=columns[i].ImageList.ImageSize.Height+1;
						}
					}
				}
				for(int i=0;i<rows.Count;i++) {
					rows[i].RowHeight=0;
					Font cellFont=cellFontNormal;
					if(rows[i].Bold==true) {
						cellFont=cellFontBold;
					}
					else {
						//Determine if any cells in this row are bold.  If at least one cell is bold, then we need to calculate the row height using bold font.
						for(int j=0;j<rows[i].Cells.Count;j++) {
							if(rows[i].Cells[j].Bold==YN.Yes) {//We don't care if a cell is underlined because it does not affect the size of the row
								cellFont=cellFontBold;
								break;
							}
						}
					}
					if(wrapText) {
						//find the tallest col
						for(int j=0;j<rows[i].Cells.Count;j++) {
							if(HasEditableColumn) {
								//doesn't seem to calculate right when it ends in a "\r\n". It doesn't make room for the new line. Make it, by adding another one for calculations.
								cellH=(int)Math.Ceiling(((1.03)*(float)(g.MeasureString(rows[i].Cells[j].Text+"\r\n",cellFont,_listCurColumnWidths[j],_format).Height))+4);//because textbox will be bigger
								if(cellH < EDITABLE_ROW_HEIGHT) {
									cellH=EDITABLE_ROW_HEIGHT;//only used for single line text
								}
							}
							else {
								float hTemp=g.MeasureString(rows[i].Cells[j].Text,cellFont,_listCurColumnWidths[j],_format).Height;
								if(IsForPrinting) {
									hTemp=LineSpacingForFont(cellFont.Name)*hTemp;
								}
								cellH=(int)Math.Ceiling(hTemp)+1;
							}
							//if(rows[i].Height==0) {//not set
							//  cellH=(int)Math.Ceiling(g.MeasureString(rows[i].Cells[j].Text,cellFont,_listCurColumnWidths[j]).Height+1);
							//}
							//else {
							//  cellH=rows[i].Height;
							//}
							if(cellH>rows[i].RowHeight) {
								rows[i].RowHeight=cellH;
							}
						}
						//Cameron 10/23/2013: Rows used to look like thick lines when the row height was 1.  When the height is less than 4, the row is not visible enough to select or edit.
						//We will use the height of the string "Any" to determine a better row height so the user can see that it is an empty row.
						//If, for whatever reason, their font really does return a row height less than 4, the following code will return that value anyway thus this change should be harmless.
						if(rows[i].RowHeight<4) {
							rows[i].RowHeight=(int)Math.Ceiling(g.MeasureString("Any",cellFont,100,_format).Height+1);
						}
					}
					else {//text not wrapping
						if(HasEditableColumn) {
							rows[i].RowHeight=EDITABLE_ROW_HEIGHT;
						}
						else {
							rows[i].RowHeight=(int)Math.Ceiling(g.MeasureString("Any",cellFont,100,_format).Height+1);
						}
						//if(rows[i].Height==0) {//not set
						//	rows[i].RowHeight=(int)Math.Ceiling(g.MeasureString("Any",cellFont,100).Height+1);
						//}
						//else {
						//	rows[i].RowHeight=rows[i].Height;
						//}
					}
					if(imageH>rows[i].RowHeight) {
						rows[i].RowHeight=imageH;
					}
					if(noteW>0 && rows[i].Note!="") {
						if(rows[i].Note.Length<=_noteLengthLimit) {//Limiting the characters shown because large emails can cause OD to hang.
							rows[i].NoteHeight=(int)Math.Ceiling(g.MeasureString(rows[i].Note,cellFontNormal,noteW,_format).Height);//Notes cannot be bold.  Always normal font.
						}
						else {
							rows[i].NoteHeight=(int)Math.Ceiling(g.MeasureString(rows[i].Note.Substring(0,_noteLengthLimit),cellFontNormal,noteW,_format).Height);
						}
					}
					if(i==0) {
						rows[i].RowLoc=0;
					}
					else {
						rows[i].RowLoc=rows[i-1].RowLoc+rows[i-1].RowHeight+rows[i-1].NoteHeight;
					}
					if(_hasDropDowns) {
						//If the user has created a grid with "drop down parents" and didn't specify a drop down state for the parent, default to collapsed.
						if(rows[i].DropDownParent!=null && rows[i].DropDownParent.DropDownState==ODGridDropDownState.None) {
							rows[i].DropDownParent.DropDownState=ODGridDropDownState.Up;
						}
					}
				}
				GridH=RowsFiltered.OfType<ODGridRow>().Sum(x => x.RowHeight+x.NoteHeight);
			}//end using
			tempFont.Dispose();
			if(IsForPrinting) {
				ComputePrintRows();
			}
		}

		///<summary>Fills PrintRows with row information.</summary>
		private void ComputePrintRows() {
			PrintRows=new List<ODPrintRow>();
			bool drawTitle=false;
			bool drawHeader=true;
			bool drawFooter=false;
			int yPosCur=YPosField;
			int bottomCurPage=PageHeight-BottomMargin;
			while(yPosCur>bottomCurPage) {//advance pages until we are using correct y values. Example: grid starts on page three, yPosCur would be something like 2500
				bottomCurPage+=PageHeight-(TopMargin+BottomMargin);
			}
			for(int i=0;i<rows.Count;i++) {
				#region Split patient accounts on Statement grids.
				if(i==0
					&& (title.StartsWith("TreatPlanBenefitsFamily") 
					|| title.StartsWith("TreatPlanBenefitsIndividual")
					|| title.StartsWith("StatementPayPlan")
					|| title.StartsWith("StatementInvoicePayment")
					|| title.StartsWith("StatementMain.NotIntermingled")))
				{
					drawTitle=true;
				}
				else if(title.StartsWith("StatementMain.NotIntermingled") 
					&& i>0 
					&& rows[i].Tag.ToString()!=rows[i-1].Tag.ToString()) //Tag should be PatNum
				{
					yPosCur+=20; //space out grids.
					PrintRows[i-1].IsBottomRow=true;
					drawTitle=true;
					drawHeader=true;
				}
				#endregion
				#region Page break logic
				if(i==rows.Count-1 && (title.StartsWith("StatementPayPlan") || title.StartsWith("StatementInvoicePayment"))) {
					drawFooter=true;
				}
				if(yPosCur //start position of row
					+rows[i].RowHeight //+row height
					+(drawTitle?TitleHeight:0) //+title height if needed
					+(drawHeader?HeaderHeight:0) //+header height if needed
					+(drawFooter?TitleHeight:0) //+footer height if needed.
					>=bottomCurPage) 
				{
					if(i>0) {
						PrintRows[i-1].IsBottomRow=true;//this row causes a page break. Previous row should be a bottom row.
					}
					yPosCur=bottomCurPage+1;
					bottomCurPage+=PageHeight-(TopMargin+BottomMargin);
					drawHeader=true;
				}
				#endregion
				PrintRows.Add(new ODPrintRow(yPosCur,drawTitle,drawHeader,false,drawFooter));
				yPosCur+=(drawTitle?TitleHeight:0);
				yPosCur+=(drawHeader?HeaderHeight:0);
				yPosCur+=rows[i].RowHeight;
				yPosCur+=(drawFooter?TitleHeight:0);
				drawTitle=drawHeader=drawFooter=false;//reset all flags for next row.
				if(i==rows.Count-1){//set print height equal to the bottom of the last row.
					PrintRows[i].IsBottomRow=true;
					_printHeight=yPosCur-YPosField;
				}
			}
		}

		///<summary>ComputeRows must have been called at least once in the lifespan of this grid before calling this method.  
		///It requires row heights to have been computed first.
		///This is the ONLY method that should ever add/modify rows in _rowsFiltered.</summary>
    private void ComputeFilterRows() {
			if(!_hasDropDowns) {
				return;
			}
			//contains only the rows that we will actually be printing. (any rows hidden because their dropdown parent is closed will not be in this list.)
			ODGridRowCollection rowsFiltered = new ODGridRowCollection();
      int locDiff = 0; //keep track of how much we have to subract from the row location to account for hidden rows.
      for(int i = 0;i<rows.Count;i++) {
        //Keeps track of which rows have already been listed as a parent of this row. The same parent should not be added to this list more than one time.
        List<ODGridRow> listParentRows = new List<ODGridRow>();
        if(rows[i].DropDownParent==null) { //the row doesn't have a drop down parent, so it must be showing.
          ODGridRow addRow = rows[i].Copy();
          addRow.RowLoc-=locDiff;
          rowsFiltered.Add(addRow);
        }
        else { //the row has a dropdown parent.
          ODGridRow rowCur = rows[i];
          bool skip = false;
          //this will throw a stack overflow exception if two gridrows list each other as a parent.
          //to prevent this, we throw a manual exception explaining what happened.
          while(rowCur.DropDownParent!=null) { //Keep going until we get to the topmost parent row in the tree. If DropDownParent==-1, that means this row has no parent.
            if(listParentRows.Contains(rowCur)) {
              throw new Exception("You cannot have a parent row and a child row pointing to each other.");
            }
            listParentRows.Add(rowCur);
            if(rowCur.DropDownParent.DropDownState!=ODGridDropDownState.Down) { //if this row's parent is not dropped down, then this row is not displaying, so skip it.
              //if any of a row's parents are closed, don't show the row.
              skip=true;
              //break; //we choose not to break here so that we traverse the full tree of rows to make sure there is not a circular relationship between parent & child rows.
            }
            //traverse the tree; go up one parent, then run the same loop.
            rowCur=rowCur.DropDownParent;
          }
          if(!skip) { //none of this row's parents are closed.
            //Add the row to the filtered list to display.
            ODGridRow addRow = rows[i].Copy(); //since we're editing the row's RowLoc, take a copy of the row.
            addRow.RowLoc-=locDiff;
            rowsFiltered.Add(addRow);
          }
          else {
            //otherwise, one of the row's parents were closed
            //don't add the row to the list of filtered rows and account for the difference in rowLocations for all rows below this one.
            locDiff+=rows[i].TotalHeight;
          }
        }
      }
      _rowsFiltered=rowsFiltered;
    }

    ///<summary>Returns row. -1 if no valid row.  Supply the y position in pixels.
    ///Always returns the value in terms of the currently displaying rows.</summary>
    public int PointToRow(int y) {
			if(y<1+titleHeight+headerHeight) {
				return -1;
			}
			for(int i=0;i<RowsFiltered.Count;i++) {
				if(y>-vScroll.Value+1+titleHeight+headerHeight+RowsFiltered[i].RowLoc+RowsFiltered[i].RowHeight+RowsFiltered[i].NoteHeight) {
					continue;//clicked below this row.
				}
				return i;
			}
			return -1;
		}

		///<summary>Returns col.  Supply the x position in pixels. -1 if no valid column.</summary>
		public int PointToCol(int x) {
			int colRight;//the right edge of each column
			for(int i=0;i<columns.Count;i++) {
				colRight=0;
				for(int c=0;c<i+1;c++) {
					colRight+=_listCurColumnWidths[c];
				}
				if(x>-hScroll.Value+colRight) {
					continue;//clicked to the right of this col
				}
				return i;
			}
			return -1;
		}
		#endregion Computations

		#region Painting
		///<summary></summary>
		protected override void OnPaintBackground(PaintEventArgs pea) {
			//base.OnPaintBackground (pea);
			//don't paint background.  This reduces flickering.
		}

		///<summary>Runs any time the control is invalidated.</summary>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
			if(IsUpdating) {
				return;
			}
			if(Width<1 || Height<1) {
				return;
			}
			ComputeColumns();//it's only here because I can't figure out how to do it when columns are added. It will be removed.
			Bitmap doubleBuffer=new Bitmap(Width,Height);
			doubleBuffer.SetResolution(e.Graphics.DpiX,e.Graphics.DpiY);
			using(Graphics g=Graphics.FromImage(doubleBuffer)) {
				g.SmoothingMode=SmoothingMode.HighQuality;//for the up/down triangles
				//g.TextRenderingHint=TextRenderingHint.AntiAlias;//for accurate string measurements. Didn't work
				//g.TextRenderingHint=TextRenderingHint.SingleBitPerPixelGridFit;
				//float pagescale=g.PageScale;
				DrawBackG(g);
				DrawRows(g);
				DrawTitleAndHeaders(g);//this will draw on top of any grid stuff
				DrawOutline(g);
				//Draw a reminder for the programmer using this grid to set the TranslationName property.
				if(translationName==null) {
					string stringException=new ArgumentNullException("TranslationName").ToString();
					Font exceptionFont=new Font(FontFamily.GenericSansSerif,16,FontStyle.Bold);
					RectangleF rectangle=new RectangleF(0,titleHeight+headerHeight,Width,Height-titleHeight-headerHeight);
					SolidBrush exceptionBrush=new SolidBrush(Color.Red);
					g.DrawString(stringException,exceptionFont,exceptionBrush,rectangle,_format);	
				}
			}
			e.Graphics.DrawImageUnscaled(doubleBuffer,0,0);
			doubleBuffer.Dispose();
			doubleBuffer=null;
		}

		private void setHeaderHeightHelper() {
			if(!_hasMultilineHeaders || Width==0 || Height==0) {
				return;
			}
			Bitmap doubleBuffer=new Bitmap(Width,Height);
			using(Graphics g = Graphics.FromImage(doubleBuffer)) {
				for(int i=0;i<columns.Count;i++) {
					headerHeight=Math.Max(headerHeight,Convert.ToInt32(g.MeasureString(columns[i].Heading,_headerFont,_listCurColumnWidths[i]).Height+2));
				}
			}
			doubleBuffer.Dispose();
			doubleBuffer=null;
		}

		///<summary>Draws a solid gray background.</summary>
		private void DrawBackG(Graphics g) {
			//if(vScroll.Enabled){//all backg white, since no gray will show
			//	g.FillRectangle(new SolidBrush(Color.White),
			//		0,titleHeight+headerHeight+1,
			//		Width,this.Height-titleHeight-headerHeight-1);
			//}
			//else{
			Color cBackG=Color.FromArgb(224,223,227);
			if(_useBlueTheme) {
				cBackG=Color.FromArgb(202,212,222);//174,196,217);//151,180,196);
			}
			g.FillRectangle(new SolidBrush(cBackG),
				0,titleHeight+headerHeight,
				Width,Height-titleHeight-headerHeight);
			//}
		}

		///<summary>Draws the background, lines, and text for all rows that are visible.</summary>
		private void DrawRows(Graphics g) {
			if(CultureInfo.CurrentCulture.Name.EndsWith("IN") && _cellFont.Name!="Arial") {
				_cellFont=new Font("Arial",_cellFont.Size);
			}
      //Draw each row that should be displayed.
      for(int i=0;i<RowsFiltered.Count;i++) {
        if(-vScroll.Value+RowsFiltered[i].RowLoc+RowsFiltered[i].RowHeight+RowsFiltered[i].NoteHeight<0) {
          continue;//lower edge of row above top of grid area
        }
        if(-vScroll.Value+1+titleHeight+headerHeight+RowsFiltered[i].RowLoc>Height) {
          return;//row below lower edge of control
        }
        DrawRow(i,g,_cellFont,RowsFiltered);
      }
		}

		///<summary>Draws background, lines, image, and text for a single row.</summary>
		private void DrawRow(int rowI,Graphics g,Font cellFont, ODGridRowCollection rowsFiltered) {
			RectangleF textRect;
			Pen gridPen=new Pen(this.cGridLine);
			Pen lowerPen=new Pen(this.cGridLine);
			if(rowI==rowsFiltered.Count-1) {//last row
				lowerPen=new Pen(Color.FromArgb(120,120,120));
			}
			else {
				if(rowsFiltered[rowI].ColorLborder!=Color.Empty) {
					lowerPen=new Pen(rowsFiltered[rowI].ColorLborder);
				}
			}
			SolidBrush textBrush;
			//selected row color
			if(selectedIndices.Contains(rowI)) {
				g.FillRectangle(new SolidBrush(selectedRowColor),
					1,
					-vScroll.Value+1+titleHeight+headerHeight+rowsFiltered[rowI].RowLoc+1,
					GridW,
					rowsFiltered[rowI].RowHeight+rowsFiltered[rowI].NoteHeight-1);
			}
			//colored row background
			else if(rowsFiltered[rowI].ColorBackG!=Color.White) {
				g.FillRectangle(new SolidBrush(rowsFiltered[rowI].ColorBackG),
					1,
					-vScroll.Value+1+titleHeight+headerHeight+rowsFiltered[rowI].RowLoc+1,
					GridW,
					rowsFiltered[rowI].RowHeight+rowsFiltered[rowI].NoteHeight-1);
			}
			//normal row color
			else {//need to draw over the gray background
				g.FillRectangle(new SolidBrush(rowsFiltered[rowI].ColorBackG),
					1,
					-vScroll.Value+1+titleHeight+headerHeight+rowsFiltered[rowI].RowLoc+1,
					GridW,//this is a really simple width value that always works well
					rowsFiltered[rowI].RowHeight+rowsFiltered[rowI].NoteHeight-1);
			}
			//Color Individual Cells.
			for(int i=0;i<rowsFiltered[rowI].Cells.Count;i++) {
				if(i>Columns.Count) {
					break;
				}
				ODGridCell cell = rowsFiltered[rowI].Cells[i];
				if(cell.CellColor==default(Color)) {//same as Color.Empty
					continue;
				}
				List<ODGridColumn> listColumns=Columns.Cast<ODGridColumn>().ToList();
				//ACCOUNT FOR ROW BACK GROUND COLORS. Cell color= Avg(CellColor+BackGColor)
				Color ColorCell;
				if(selectedIndices.Contains(rowI)) {
					ColorCell = Color.FromArgb(
						(selectedRowColor.R+cell.CellColor.R)/2,
						(selectedRowColor.G+cell.CellColor.G)/2,
						(selectedRowColor.B+cell.CellColor.B)/2);
				}
				//colored row background
				else if(rowsFiltered[rowI].ColorBackG!=Color.White) {
					ColorCell = Color.FromArgb(
						(rowsFiltered[rowI].ColorBackG.R+cell.CellColor.R)/2,
						(rowsFiltered[rowI].ColorBackG.G+cell.CellColor.G)/2,
						(rowsFiltered[rowI].ColorBackG.B+cell.CellColor.B)/2);
				}
				//normal row color
				else {//need to draw over the gray background
					ColorCell = cell.CellColor;
				}
				using(SolidBrush backBrush = new SolidBrush(ColorCell)) {
					g.FillRectangle(backBrush,
						-hScroll.Value+_listCurColumnWidths.Take(i).Sum(x =>x)+1,
						-vScroll.Value+1+titleHeight+headerHeight+rowsFiltered[rowI].RowLoc+1,
						_listCurColumnWidths[i],//this is a really simple width value that always works well
						rowsFiltered[rowI].RowHeight+rowsFiltered[rowI].NoteHeight-1);
				}
			}
			if(selectionMode==GridSelectionMode.OneCell && selectedCell.X!=-1 && selectedCell.Y!=-1
			&& selectedCell.Y==rowI) {
				g.FillRectangle(new SolidBrush(selectedRowColor),
					-hScroll.Value+1+ColPos[selectedCell.X],
					-vScroll.Value+1+titleHeight+headerHeight+rowsFiltered[rowI].RowLoc+1,
					_listCurColumnWidths[selectedCell.X],
					rowsFiltered[rowI].RowHeight+rowsFiltered[rowI].NoteHeight-1);
			}
			//lines for note section
			if(rowsFiltered[rowI].NoteHeight>0) {
				//left vertical gridline
				if(NoteSpanStart!=0) {
					g.DrawLine(gridPen,
						-hScroll.Value+1+ColPos[NoteSpanStart],
						-vScroll.Value+1+titleHeight+headerHeight+rowsFiltered[rowI].RowLoc+rowsFiltered[rowI].RowHeight,
						-hScroll.Value+1+ColPos[NoteSpanStart],
						-vScroll.Value+1+titleHeight+headerHeight+rowsFiltered[rowI].RowLoc+rowsFiltered[rowI].RowHeight+rowsFiltered[rowI].NoteHeight);
				}
				//Horizontal line which divides the main part of the row from the notes section of the row
				g.DrawLine(gridPen,
					-hScroll.Value+1+ColPos[0]+1,
					-vScroll.Value+1+titleHeight+headerHeight+rowsFiltered[rowI].RowLoc+rowsFiltered[rowI].RowHeight,
					-hScroll.Value+1+ColPos[columns.Count-1]+_listCurColumnWidths[columns.Count-1],
					-vScroll.Value+1+titleHeight+headerHeight+rowsFiltered[rowI].RowLoc+rowsFiltered[rowI].RowHeight);

			}
			for(int i=0;i<columns.Count;i++) {
				//right vertical gridline
				if(rowI==0) {
					g.DrawLine(gridPen,
						-hScroll.Value+1+ColPos[i]+_listCurColumnWidths[i],
						-vScroll.Value+1+titleHeight+headerHeight+rowsFiltered[rowI].RowLoc,
						-hScroll.Value+1+ColPos[i]+_listCurColumnWidths[i],
						-vScroll.Value+1+titleHeight+headerHeight+rowsFiltered[rowI].RowLoc+rowsFiltered[rowI].RowHeight);
				}
				else {
					g.DrawLine(gridPen,
						-hScroll.Value+1+ColPos[i]+_listCurColumnWidths[i],
						-vScroll.Value+1+titleHeight+headerHeight+rowsFiltered[rowI].RowLoc+1,
						-hScroll.Value+1+ColPos[i]+_listCurColumnWidths[i],
						-vScroll.Value+1+titleHeight+headerHeight+rowsFiltered[rowI].RowLoc+rowsFiltered[rowI].RowHeight);
				}
				//lower horizontal gridline
				if(i==0) {
					g.DrawLine(lowerPen,
						-hScroll.Value+1+ColPos[i],
						-vScroll.Value+1+titleHeight+headerHeight+rowsFiltered[rowI].RowLoc+rowsFiltered[rowI].RowHeight+rowsFiltered[rowI].NoteHeight,
						-hScroll.Value+1+ColPos[i]+_listCurColumnWidths[i],
						-vScroll.Value+1+titleHeight+headerHeight+rowsFiltered[rowI].RowLoc+rowsFiltered[rowI].RowHeight+rowsFiltered[rowI].NoteHeight);
				}
				else {
					g.DrawLine(lowerPen,
						-hScroll.Value+1+ColPos[i]+1,
						-vScroll.Value+1+titleHeight+headerHeight+rowsFiltered[rowI].RowLoc+rowsFiltered[rowI].RowHeight+rowsFiltered[rowI].NoteHeight,
						-hScroll.Value+1+ColPos[i]+_listCurColumnWidths[i],
						-vScroll.Value+1+titleHeight+headerHeight+rowsFiltered[rowI].RowLoc+rowsFiltered[rowI].RowHeight+rowsFiltered[rowI].NoteHeight);
				}
				//text
				if(rowsFiltered[rowI].Cells.Count-1<i) {
					continue;
				}
				switch(columns[i].TextAlign) {
					case HorizontalAlignment.Left:
						_format.Alignment=StringAlignment.Near;
						break;
					case HorizontalAlignment.Center:
						_format.Alignment=StringAlignment.Center;
						break;
					case HorizontalAlignment.Right:
						_format.Alignment=StringAlignment.Far;
						break;
				}
				int vertical=-vScroll.Value+1+titleHeight+headerHeight+rowsFiltered[rowI].RowLoc+1;
				int horizontal=-hScroll.Value+1+ColPos[i]+1;
				int cellW=_listCurColumnWidths[i];
				int cellH=rowsFiltered[rowI].RowHeight;
				if(HasEditableColumn) {//These cells are taller
					vertical+=2;//so this is to push text down to center it in the cell
					cellH-=3;//to keep it from spilling into the next cell
				}
				if(columns[i].TextAlign==HorizontalAlignment.Right) {
					if(HasEditableColumn) {
						horizontal-=4;
						cellW+=2;
					}
					else {
						horizontal-=2;
						cellW+=2;
					}
				}
				textRect=new RectangleF(horizontal,vertical,cellW,cellH);
				if(rowsFiltered[rowI].Cells[i].ColorText==Color.Empty) {
					textBrush=new SolidBrush(rowsFiltered[rowI].ColorText);
				}
				else {
					textBrush=new SolidBrush(rowsFiltered[rowI].Cells[i].ColorText);
				}
				if(rowsFiltered[rowI].Cells[i].Bold==YN.Yes) {
					cellFont=new Font(cellFont,FontStyle.Bold);
				}
				else if(rowsFiltered[rowI].Cells[i].Bold==YN.No) {
					cellFont=new Font(cellFont,FontStyle.Regular);
				}
				else {//unknown.  Use row bold
					if(rowsFiltered[rowI].Bold) {
						cellFont=new Font(cellFont,FontStyle.Bold);
					}
					else {
						cellFont=new Font(cellFont,FontStyle.Regular);
					}
				}
				if(rowsFiltered[rowI].Cells[i].Underline==YN.Yes) {//Underline the current cell.  If it is already bold, make the cell bold and underlined.
					cellFont=new Font(cellFont,(cellFont.Bold)?(FontStyle.Bold | FontStyle.Underline):FontStyle.Underline);
				}
				if(columns[i].ImageList==null) {
					if(_hasDropDowns) {
						if(i==0) { //only draw the dropdown arrow in the first column of the row.
							using(Pen trianglePen = new Pen(Color.Black)) {
								int textShift=0;
								if(rowsFiltered[rowI].DropDownState==ODGridDropDownState.Up) {
									PointF topPoint = new PointF(textRect.X,(((textRect.Top+textRect.Bottom)/2)-5));
									PointF botPoint = new PointF(topPoint.X,topPoint.Y+10);
									PointF rightPoint = new PointF(topPoint.X+5,topPoint.Y+5);
									g.DrawPolygon(trianglePen,new PointF[] { topPoint,botPoint,rightPoint });
									textShift=10; //keep it a consistent 10 so the text doesn't shift left-right when the dropdown is toggled.
								}
								else if(rowsFiltered[rowI].DropDownState==ODGridDropDownState.Down) {
									PointF leftPoint = new PointF(textRect.X,(((textRect.Top+textRect.Bottom)/2)-5));
									PointF rightPoint = new PointF(leftPoint.X+10,leftPoint.Y);
									PointF botPoint = new PointF(leftPoint.X+5,leftPoint.Y+5);
									g.DrawPolygon(trianglePen,new PointF[] { leftPoint,botPoint,rightPoint });
									textShift=10; //keep it a consistent 10 so the text doesn't shift left-right when the dropdown is toggled.
								}
								//a small indicator that shows that a row is a drop down child.
								if(rowsFiltered[rowI].DropDownParent!=null) {
									PointF topPoint = new PointF(textRect.X+5,textRect.Top);
									PointF midPoint = new PointF(textRect.X+5,textRect.Top+5);
									PointF rightPoint = new PointF(textRect.X+10,textRect.Top+5);
									g.DrawLines(trianglePen,new PointF[] { topPoint,midPoint,rightPoint });
									textShift=10; //keep it a consistent 10 so the text doesn't shift left-right when the dropdown is toggled.
								}
								textRect.X+=textShift;
							}
						}
					}
					//draw the text in the cell.
					g.DrawString(rowsFiltered[rowI].Cells[i].Text,cellFont,textBrush,textRect,_format);
				}
				else {
					int imageIndex=-1;
					if(rowsFiltered[rowI].Cells[i].Text!="") {
						imageIndex=PIn.Int(rowsFiltered[rowI].Cells[i].Text);
					}
					if(imageIndex!=-1) {
						Image img=columns[i].ImageList.Images[imageIndex];
						g.DrawImage(img,horizontal,vertical-1);
					}
				}
			}
			//note text
			if(rowsFiltered[rowI].NoteHeight>0 && NoteSpanStop>0 && NoteSpanStart<columns.Count) {
				int noteW=0;
				for(int i=NoteSpanStart;i<=NoteSpanStop;i++) {
					noteW+=_listCurColumnWidths[i];
				}
				if(rowsFiltered[rowI].Bold) {
					cellFont=new Font(cellFont,FontStyle.Bold);
				}
				else {
					cellFont=new Font(cellFont,FontStyle.Regular);
				}
				textBrush=new SolidBrush(rowsFiltered[rowI].ColorText);
				textRect=new RectangleF(
					-hScroll.Value+1+ColPos[NoteSpanStart]+1,
					-vScroll.Value+1+titleHeight+headerHeight+rowsFiltered[rowI].RowLoc+rowsFiltered[rowI].RowHeight+1,
					ColPos[NoteSpanStop]+_listCurColumnWidths[NoteSpanStop]-ColPos[NoteSpanStart],
					rowsFiltered[rowI].NoteHeight);
				_format.Alignment=StringAlignment.Near;
				if(rowsFiltered[rowI].Note.Length<=_noteLengthLimit) {
					g.DrawString(rowsFiltered[rowI].Note,cellFont,textBrush,textRect,_format);
				}
				else {
					g.DrawString(rowsFiltered[rowI].Note.Substring(0,_noteLengthLimit),cellFont,textBrush,textRect,_format);
				}
			}
		}

		private void DrawTitleAndHeaders(Graphics g) {
			//Title----------------------------------------------------------------------------------------------------
			if(TitleHeight!=0) {
				Color cTitleTop=Color.White;
				Color cTitleBottom=Color.FromArgb(213,213,223);
				Color cTitleText=Color.Black;
				if(_useBlueTheme) {
					cTitleTop=Color.FromArgb(156,175,230);//191,205,245);//139,160,224);//114,136,201);//106,132,210);//109,129,191);//104,136,232);
					cTitleBottom=Color.FromArgb(60,90,150);//35,55,115);//49,63,105);//(20,47,126);
					cTitleText=Color.White;
				}
				LinearGradientBrush brushTitleBackground=new LinearGradientBrush(new Rectangle(0,0,Width,titleHeight),cTitleTop,cTitleBottom,LinearGradientMode.Vertical);
				SolidBrush brushTitleText=new SolidBrush(cTitleText);
				g.FillRectangle(brushTitleBackground,0,0,Width,titleHeight);
				g.DrawString(title,_titleFont,brushTitleText,Width/2-g.MeasureString(title,_titleFont).Width/2,2);
				if(HasAddButton) {
					int addW=titleHeight;
					int dividerX=Width-addW-3;
					const int dividerLineWidth=1;
					const int plusSignWidth=4;
					Brush brushPlusSign=brushTitleText;
					if(!GetIsAddButtonEnabled()) {
						//"gray out" darkest background color for plus sign
						const double fadeFactor=0.8;
						brushPlusSign=new LinearGradientBrush(new Rectangle(0,0,Width,titleHeight),
							Color.FromArgb((int)(cTitleTop.R*fadeFactor),(int)(cTitleTop.G*fadeFactor),(int)(cTitleTop.B*fadeFactor)),
							Color.FromArgb((int)(cTitleBottom.R*fadeFactor),(int)(cTitleBottom.G*fadeFactor),(int)(cTitleBottom.B*fadeFactor)),
							LinearGradientMode.Vertical);//"gray out" AddButton
					}
					using(Pen pDark=new Pen(Color.FromArgb(102,102,122)))
					using(Pen pText=new Pen(cTitleText)){
						g.DrawLine(Pens.LightGray,new Point(dividerX,0),new Point(dividerX,this.TitleHeight));//divider line(right side)
						g.DrawLine(pDark,new Point(dividerX-dividerLineWidth,0),new Point(dividerX-dividerLineWidth,this.TitleHeight));//divider line(left side)
						g.FillRectangle(brushPlusSign,//vertical bar in "+" sign
							Width-addW/2-plusSignWidth,2,
							plusSignWidth,addW-plusSignWidth);
						//Width-addW/2+2,addW-2);
						g.FillRectangle(brushPlusSign,//horizontal bar in "+" sign
							Width-addW,(addW-plusSignWidth)/2,
							addW-plusSignWidth,plusSignWidth);
						//Width-2,addW/2+2);
						//g.DrawString("+",titleFont,brushTitleText,Width-addW+4,2);
					}
					if(brushPlusSign!=null) {
						brushPlusSign.Dispose();
						brushPlusSign=null;
					}
				}
				if(brushTitleBackground!=null) {
					brushTitleBackground.Dispose();
					brushTitleBackground=null;
				}
				if(brushTitleText!=null) {
					brushTitleText.Dispose();
					brushTitleText=null;
				}
			}
			//Column Headers-----------------------------------------------------------------------------------------
			if(HeaderHeight!=0) {
				Color cTitleBackG=Color.FromArgb(210,210,210);
				if(_useBlueTheme) {
					cTitleBackG=Color.FromArgb(223,234,245);//208,225,242);//166,185,204);
				}
				g.FillRectangle(new SolidBrush(cTitleBackG),0,titleHeight,Width,headerHeight);//background
				g.DrawLine(new Pen(Color.FromArgb(102,102,122)),0,titleHeight,Width,titleHeight);//line between title and headers
				using(StringFormat format=new StringFormat())
				{
					for(int i=0;i<columns.Count;i++) {
						if(i!=0) {
							//vertical lines separating column headers
							g.DrawLine(new Pen(Color.FromArgb(120,120,120)),-hScroll.Value+1+ColPos[i],titleHeight+3,
								-hScroll.Value+1+ColPos[i],titleHeight+headerHeight-2);
							g.DrawLine(new Pen(Color.White),-hScroll.Value+1+ColPos[i]+1,titleHeight+3,
								-hScroll.Value+1+ColPos[i]+1,titleHeight+headerHeight-2);
						}
						format.Alignment=StringAlignment.Center;
						float verticalAdjust=0;
						if(_hasMultilineHeaders) {
							verticalAdjust=(headerHeight-g.MeasureString(columns[i].Heading,_headerFont).Height)/2;//usually 0
						}
						if(verticalAdjust>2) {
							verticalAdjust-=2;//text looked too low. This shifts the text up a little for any of the non-multiline headers.
						}
						if(_hasAutoWrappedHeaders) {
							RectangleF layoutRec=new RectangleF(ColPos[i]-hScroll.Value,titleHeight+2,_listCurColumnWidths[i],headerHeight);
							g.DrawString(columns[i].Heading,_headerFont,Brushes.Black,layoutRec,format);
						}
						else {
							g.DrawString(columns[i].Heading,_headerFont,Brushes.Black,
								-hScroll.Value+ColPos[i]+_listCurColumnWidths[i]/2,
								titleHeight+2+verticalAdjust,
								format);
						}
						if(sortedByColumnIdx==i) {
							PointF p=new PointF(-hScroll.Value+1+ColPos[i]+6,titleHeight+(float)headerHeight/2f);
							if(sortedIsAscending) {//pointing up
								g.FillPolygon(Brushes.White,new PointF[] {
								new PointF(p.X-4.9f,p.Y+2f),//LLstub
								new PointF(p.X-4.9f,p.Y+2.5f),//LLbase
								new PointF(p.X+4.9f,p.Y+2.5f),//LRbase
								new PointF(p.X+4.9f,p.Y+2f),//LRstub
								new PointF(p.X,p.Y-2.8f)});//Top
								g.FillPolygon(Brushes.Black,new PointF[] {
								new PointF(p.X-4,p.Y+2),//LL
								new PointF(p.X+4,p.Y+2),//LR
								new PointF(p.X,p.Y-2)});//Top
							}
							else {//pointing down
								g.FillPolygon(Brushes.White,new PointF[] {//shaped like home plate
								new PointF(p.X-4.9f,p.Y-2f),//ULstub
								new PointF(p.X-4.9f,p.Y-2.7f),//ULtop
								new PointF(p.X+4.9f,p.Y-2.7f),//URtop
								new PointF(p.X+4.9f,p.Y-2f),//URstub
								new PointF(p.X,p.Y+2.8f)});//Bottom
								g.FillPolygon(Brushes.Black,new PointF[] {
								new PointF(p.X-4,p.Y-2),//UL
								new PointF(p.X+4,p.Y-2),//UR
								new PointF(p.X,p.Y+2)});//Bottom
							}
						}
					}
				}
			}
			else{
				return;
			}
			//line below headers
			g.DrawLine(new Pen(Color.FromArgb(120,120,120)),0,titleHeight+headerHeight,Width,titleHeight+headerHeight);
		}

		///<summary>Draws outline around entire control.</summary>
		private void DrawOutline(Graphics g) {
			if(hScroll.Visible) {//for the little square at the lower right between the two scrollbars
				g.FillRectangle(new SolidBrush(Color.FromKnownColor(KnownColor.Control)),Width-vScroll.Width-1,
					Height-hScroll.Height-1,vScroll.Width,hScroll.Height);
			}
			Color cOutline=Color.FromArgb(119,119,146);
			if(_useBlueTheme) {
				cOutline=Color.FromArgb(47,70,117);
			}
			using(Pen pen=new Pen(cOutline)) {
				g.DrawRectangle(pen,0,0,Width-1,Height-1);
			}
		}
		#endregion

		#region Clicking
		///<summary></summary>
		protected void OnCellDoubleClick(int col,int row) {
			ODGridClickEventArgs gArgs=new ODGridClickEventArgs(col,row,MouseButtons.Left);
			if(CellDoubleClick!=null) {
				CellDoubleClick(this,gArgs);
			}
		}

		protected void OnCellSelectionChangeCommitted(int col,int row) {
			ODGridClickEventArgs gArgs=new ODGridClickEventArgs(col,row,MouseButtons.Left);
			if(CellSelectionCommitted!=null) {
				CellSelectionCommitted(this,gArgs);
			}
		}

		///<summary></summary>
		protected override void OnDoubleClick(EventArgs e) {
			base.OnDoubleClick(e);
			if(HasDistinctClickEvents) {//ODThread _threadDistinctClickEvents handles OnClick(...) and OnDoubleClick(...) calls.
				return;
			}
			OnDoubleClickHelper(e);
		}

		private void OnDoubleClickHelper(EventArgs e) {
			if(MouseDownRow==-1) {
				return;//double click was in the title or header section
			}
			if(MouseDownCol==-1) {
				return;//click was to the right of the columns
			}
			OnCellDoubleClick(MouseDownCol,MouseDownRow);
		}

		///<summary></summary>
		protected void OnCellClick(int col,int row,MouseButtons button) {
			ODGridClickEventArgs gArgs=new ODGridClickEventArgs(col,row,button);
			if(CellClick!=null) {
				CellClick(this,gArgs);
			}
		}

    private void DropDownRowClicked(int mouseDownRow,int mouseDownY) {
      int curRow = PointToRow(mouseDownY);
      if(curRow!=MouseDownRow) {
        //if they click + dragged, don't expand/collapse the row they initially clicked on.
        return;
      }
      if(RowsFiltered[mouseDownRow].DropDownState==ODGridDropDownState.Up) {
        foreach(ODGridRow rowCur in rows) {
          if(rowCur.RowNum==RowsFiltered[mouseDownRow].RowNum) {
            //find the row in the full list of rows that matches the row they clicked on.
            rowCur.DropDownState=ODGridDropDownState.Down;
          }
        }
      }
      else if(RowsFiltered[mouseDownRow].DropDownState==ODGridDropDownState.Down) {
        foreach(ODGridRow rowCur in rows) {
          if(rowCur.RowNum==RowsFiltered[mouseDownRow].RowNum) {
            //find the row in the full list of rows that matches the row they clicked on.
            rowCur.DropDownState=ODGridDropDownState.Up;
          }
        }   
      }
			ComputeFilterRows();
			GridH=RowsFiltered.OfType<ODGridRow>().Sum(x => x.RowHeight+x.NoteHeight);
			LayoutScrollBars();
			this.Invalidate(); //causes a repaint.
    }

    ///<summary></summary>
    protected override void OnClick(EventArgs e) {
			base.OnClick(e);
			if(HasDistinctClickEvents) {//ODThread _threadDistinctClickEvents handles OnClick(...) and OnDoubleClick(...) calls.
				return;
			}
			OnClickHelper(e);
		}

		private void OnClickHelper(EventArgs e) {
			if(HasAddButton //only check this if we are showing the "add button"
				&& TitleAddClick!=null //there is an event handler
				&& ((MouseEventArgs)e).X>=Width-TitleHeight-5 && ((MouseEventArgs)e).Y<=TitleHeight)
			{
				TitleAddClick(this,e);
			}
      if(_hasDropDowns
				&& MouseDownRow>-1 && MouseDownCol==0 && RowsFiltered[MouseDownRow].DropDownState!=ODGridDropDownState.None
        /*&& ((MouseEventArgs)(e)).X < 10*///uncomment if the user should have to click the DropDown Triangle to for the row to drop down.
        && !ControlIsDown //if the user is trying to select the row, don't drop it down.
				&& !ShiftIsDown
        ) 
			{ 
        DropDownRowClicked(MouseDownRow,((MouseEventArgs)(e)).Y);
        return;
      }
      if(MouseDownRow>-1 && MouseDownCol>-1) {//on a row and not to the side of the columns.
				OnCellClick(MouseDownCol,MouseDownRow,lastButtonPressed);
			}
		}

    private static void OpenWikiPage(string pageTitle) {
			if(WikiPages.NavPageDelegate!=null) {
				WikiPages.NavPageDelegate(pageTitle);
			}
		}

		private static void OpenWebPage(string url) {
			try {
				if(!url.ToLower().StartsWith("http")) {
					url=@"http://"+url;
				}
				Process.Start(url);
			}
			catch {
				MessageBox.Show(Lans.g("OdGrid","Failed to open web browser.  Please make sure you have a default browser set and are connected to the internet then try again."),Lans.g("OdGrid","Attention"));
			}
		}
		#endregion Clicking

		#region BeginEndUpdate
		///<summary>Call this before adding any rows.  You would typically call Rows.Clear after this.</summary>
		public void BeginUpdate() {
			IsUpdating=true;
		}

		///<summary>Must be called after adding rows.  This computes the columns, computes the rows, lays out the scrollbars, clears SelectedIndices, and invalidates.  Does not zero out scrollVal.  Sometimes, it seems like scrollVal needs to be reset somehow because it's an inappropriate number, and when you first grab the scrollbar, it jumps.  No time to investigate.</summary>
		public void EndUpdate(bool isForPrinting=false,bool isRecompute=false) {
			if(this.IsDisposed) {
				//Occassionally our customers report to us that they get a 'Cannot access a disposed object' from inside this method. We don't have
				//a good idea why this is happening since these errors usually occur on a form that doesn't have any threading. We're putting this here to
				//hopefully prevent some of these errors.
				return;
			}
			ComputeColumns();
			using(Graphics g=this.CreateGraphics()) {
				ComputeRows(g,isForPrinting);
			}
			LayoutScrollBars();
      ComputeFilterRows();
      //ScrollValue=0;
			if(!isRecompute) {
				selectedIndices=new ArrayList();
				selectedCell=new Point(-1,-1);
				if(editBox!=null) {
					editBox.Dispose();
				}
				sortedByColumnIdx=-1;
			}
			IsUpdating=false;
			Invalidate();
		}
		#endregion BeginEndUpdate

		#region Printing

		///<summary></summary>
		public void PrintRow(int rowI,Graphics g,int x=0,int y=0,bool isBottom=false,bool isSheetGrid=false,bool isPrintingSheet=false) {
			Font tempFont=new Font(_cellFont,_cellFont.Style);
			if(FontForSheets!=null) {
				tempFont=new Font(FontForSheets,FontStyle.Regular);
			}
			Color cOutline=Color.FromArgb(119,119,146);
			if(_useBlueTheme) {
				cOutline=Color.FromArgb(47,70,117);
			}
			if(isSheetGrid) {
				cOutline=Color.Black;
			}
			RectangleF textRect;
			Pen gridPen=new Pen(this.cGridLine);
			Pen lowerPen=new Pen(this.cGridLine);
			if(rowI==rows.Count-1) {//last row
				lowerPen=new Pen(Color.FromArgb(120,120,120));
			}
			else {
				if(rows[rowI].ColorLborder!=Color.Empty) {
					lowerPen=new Pen(rows[rowI].ColorLborder);
				}
			}
			SolidBrush textBrush;
			//selected row color
			if(selectedIndices.Contains(rowI)) {
				g.FillRectangle(new SolidBrush(selectedRowColor),
					x+1,
					y-vScroll.Value+1,//+titleHeight+headerHeight+rows[rowI].RowLoc+1,
					GridW,
					rows[rowI].RowHeight+rows[rowI].NoteHeight-1);
			}
			//colored row background
			else if(rows[rowI].ColorBackG!=Color.White) {
				g.FillRectangle(new SolidBrush(rows[rowI].ColorBackG),
					x+1,
					y-vScroll.Value+1,//+titleHeight+headerHeight+rows[rowI].RowLoc+1,
					GridW,
					rows[rowI].RowHeight+rows[rowI].NoteHeight-1);
			}
			//normal row color
			else {//need to draw over the gray background
				g.FillRectangle(new SolidBrush(rows[rowI].ColorBackG),
					x+1,
					y-vScroll.Value+1,//+titleHeight+headerHeight+rows[rowI].RowLoc+1,
					GridW,//this is a really simple width value that always works well
					rows[rowI].RowHeight+rows[rowI].NoteHeight-1);
			}
			if(selectionMode==GridSelectionMode.OneCell && selectedCell.X!=-1 && selectedCell.Y!=-1
			&& selectedCell.Y==rowI) {
				g.FillRectangle(new SolidBrush(selectedRowColor),
					x-hScroll.Value+1+ColPos[selectedCell.X],
					y-vScroll.Value+1,//+titleHeight+headerHeight+rows[rowI].RowLoc+1,
					_listCurColumnWidths[selectedCell.X],
					rows[rowI].RowHeight+rows[rowI].NoteHeight-1);
			}
			//lines for note section
			if(rows[rowI].NoteHeight>0) {
				//left vertical gridline
				if(NoteSpanStart!=0) {
					g.DrawLine(gridPen,
						x-hScroll.Value+1+ColPos[NoteSpanStart],
						y-vScroll.Value+1+rows[rowI].RowHeight,//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI],
						x-hScroll.Value+1+ColPos[NoteSpanStart],
						y-vScroll.Value+1+rows[rowI].RowHeight+rows[rowI].NoteHeight);//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI]+rows[rowI].NoteHeight);
				}
				//Horizontal line which divides the main part of the row from the notes section of the row
				g.DrawLine(gridPen,
					x-hScroll.Value+1+ColPos[0]+1,
					y-vScroll.Value+1+rows[rowI].RowHeight,//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI],
					x-hScroll.Value+1+ColPos[columns.Count-1]+_listCurColumnWidths[columns.Count-1],
					y-vScroll.Value+1+rows[rowI].RowHeight);//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI]);

			}
			for(int i=0;i<columns.Count;i++) {
				//right vertical gridline
				if(rowI==0) {
					g.DrawLine(gridPen,
						x-hScroll.Value+ColPos[i]+_listCurColumnWidths[i],
						y-vScroll.Value+1,//+rows[rowI].RowLoc,//+titleHeight+headerHeight+rows[rowI].RowLoc,
						x-hScroll.Value+ColPos[i]+_listCurColumnWidths[i],
						y-vScroll.Value+1+rows[rowI].RowHeight);//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI]);
				}
				else {
					g.DrawLine(gridPen,
						x-hScroll.Value+ColPos[i]+_listCurColumnWidths[i],
						y-vScroll.Value+1,//+rows[rowI].RowLoc,//+titleHeight+headerHeight+rows[rowI].RowLoc+1,
						x-hScroll.Value+ColPos[i]+_listCurColumnWidths[i],
						y-vScroll.Value+1+rows[rowI].RowHeight);//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI]);
				}
				//lower horizontal gridline
				if(i==0) {
					g.DrawLine(lowerPen,
						x-hScroll.Value+ColPos[i],
						y-vScroll.Value+1+rows[rowI].RowHeight+rows[rowI].NoteHeight,//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI]+rows[rowI].NoteHeight,
						x-hScroll.Value+ColPos[i]+_listCurColumnWidths[i],
						y-vScroll.Value+1+rows[rowI].RowHeight+rows[rowI].NoteHeight);//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI]+rows[rowI].NoteHeight);
				}
				else {
					g.DrawLine(lowerPen,
						x-hScroll.Value+ColPos[i]+1,
						y-vScroll.Value+1+rows[rowI].RowHeight+rows[rowI].NoteHeight,//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI]+rows[rowI].NoteHeight,
						x-hScroll.Value+ColPos[i]+_listCurColumnWidths[i],
						y-vScroll.Value+1+rows[rowI].RowHeight+rows[rowI].NoteHeight);//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI]+rows[rowI].NoteHeight);
				}
				//text
				if(rows[rowI].Cells.Count-1<i) {
					continue;
				}
				switch(columns[i].TextAlign) {
					case HorizontalAlignment.Left:
						_format.Alignment=StringAlignment.Near;
						break;
					case HorizontalAlignment.Center:
						_format.Alignment=StringAlignment.Center;
						break;
					case HorizontalAlignment.Right:
						_format.Alignment=StringAlignment.Far;
						break;
				}
				int vertical=y-vScroll.Value+1+1;// +titleHeight+headerHeight+rows[rowI].RowLoc+1;
				int horizontal=x-hScroll.Value+1+ColPos[i];
				int cellW=_listCurColumnWidths[i];
				int cellH=rows[rowI].RowHeight;
				if(HasEditableColumn) {//These cells are taller
					vertical+=2;//so this is to push text down to center it in the cell
					cellH-=3;//to keep it from spilling into the next cell
				}
				if(columns[i].TextAlign==HorizontalAlignment.Right) {
					if(HasEditableColumn) {
						horizontal-=4;
						cellW+=2;
					}
					else {
						horizontal-=2;
						cellW+=2;
					}
				}
				textRect=new RectangleF(horizontal,vertical,cellW,cellH);
				if(rows[rowI].Cells[i].ColorText==Color.Empty) {
					textBrush=new SolidBrush(rows[rowI].ColorText);
				}
				else {
					textBrush=new SolidBrush(rows[rowI].Cells[i].ColorText);
				}
				if(rows[rowI].Cells[i].Bold==YN.Yes) {
					tempFont=new Font(tempFont,FontStyle.Bold);
				}
				else if(rows[rowI].Cells[i].Bold==YN.No) {
					tempFont=new Font(tempFont,FontStyle.Regular);
				}
				else {//unknown.  Use row bold
					if(rows[rowI].Bold) {
						tempFont=new Font(tempFont,FontStyle.Bold);
					}
					else {
						tempFont=new Font(tempFont,FontStyle.Regular);
					}
				}
				if(rows[rowI].Cells[i].Underline==YN.Yes) {//Underline the current cell.  If it is already bold, make the cell bold and underlined.
					tempFont=new Font(tempFont,(tempFont.Bold)?(FontStyle.Bold | FontStyle.Underline):FontStyle.Underline);
				}
				if(columns[i].ImageList==null) {
					if(isPrintingSheet) {
						//Using a slightly smaller font because g.DrawString draws text slightly larger when using the printer's graphics
						Font smallerFont=new Font(tempFont.FontFamily,(float)(tempFont.Size*0.96),tempFont.Style);
						g.DrawString(rows[rowI].Cells[i].Text,smallerFont,textBrush,textRect,_format);
						smallerFont.Dispose();
					}
					else {//Viewing the grid normally
						g.DrawString(rows[rowI].Cells[i].Text,tempFont,textBrush,textRect,_format);
					}
				}
				else {
					int imageIndex=-1;
					if(rows[rowI].Cells[i].Text!="") {
						imageIndex=PIn.Int(rows[rowI].Cells[i].Text);
					}
					if(imageIndex!=-1) {
						Image img=columns[i].ImageList.Images[imageIndex];
						g.DrawImage(img,horizontal,vertical-1);
					}
				}
			}
			//note text
			if(rows[rowI].NoteHeight>0 && NoteSpanStop>0 && NoteSpanStart<columns.Count) {
				int noteW=0;
				for(int i=NoteSpanStart;i<=NoteSpanStop;i++) {
					noteW+=_listCurColumnWidths[i];
				}
				if(rows[rowI].Bold) {
					tempFont=new Font(tempFont,FontStyle.Bold);
				}
				else {
					tempFont=new Font(tempFont,FontStyle.Regular);
				}
				textBrush=new SolidBrush(rows[rowI].ColorText);
				textRect=new RectangleF(
					x-hScroll.Value+1+ColPos[NoteSpanStart]+1,
					y-vScroll.Value+1+rows[rowI].RowHeight+1,//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI]+1,
					ColPos[NoteSpanStop]+_listCurColumnWidths[NoteSpanStop]-ColPos[NoteSpanStart],
					rows[rowI].NoteHeight);
				_format.Alignment=StringAlignment.Near;
				g.DrawString(rows[rowI].Note,tempFont,textBrush,textRect,_format);
			}
			//Left right and bottom lines of grid.  This creates the outline of the entire grid when not using outline control
			//Outline the Title
			using(Pen pen=new Pen(cOutline)) {
				//Draw line from LL to UL to UR to LR. top three sides of a rectangle.
				g.DrawLine(pen,x,y,x,y+rows[rowI].RowHeight+rows[rowI].NoteHeight+1);//left side line
				g.DrawLine(pen,x+Width,y,x+Width,y+rows[rowI].RowHeight+rows[rowI].NoteHeight+1);//right side line
				if(isBottom) {
					g.DrawLine(pen,x,y+rows[rowI].RowHeight+rows[rowI].NoteHeight+1,x+Width,y+rows[rowI].RowHeight+rows[rowI].NoteHeight+1);//bottom line.
				}
			}
			tempFont.Dispose();
		}

		///<summary></summary>
		public void PrintRowX(int rowI,XGraphics g,int x=0,int y=0,bool isBottom=false,bool isSheetGrid=false) {
			Font tempFont=new Font(_cellFont,_cellFont.Style);
			if(FontForSheets!=null) {
				tempFont=new Font(FontForSheets,FontStyle.Regular);
			}
			Color cOutline=Color.FromArgb(119,119,146);
			if(_useBlueTheme) {
				cOutline=Color.FromArgb(47,70,117);
			}
			if(isSheetGrid) {
				cOutline=Color.Black;
			}
			XFont cellFont=new XFont(tempFont.Name,tempFont.Size);
			XRect textRect;
			XStringAlignment _xAlign=XStringAlignment.Near;
			XPen gridPen=new XPen(this.cGridLine);
			XPen lowerPen=new XPen(this.cGridLine);
			if(rowI==rows.Count-1) {//last row
				lowerPen=new XPen(XColor.FromArgb(120,120,120));
			}
			else {
				if(rows[rowI].ColorLborder!=Color.Empty) {
					lowerPen=new XPen(rows[rowI].ColorLborder);
				}
			}
			XSolidBrush textBrush;
			//selected row color
			if(selectedIndices.Contains(rowI)) {
				g.DrawRectangle(new XSolidBrush(selectedRowColor),
					p(x+1),
					p(y-vScroll.Value+1),//+titleHeight+headerHeight+rows[rowI].RowLoc+1,
					p(GridW),
					p(rows[rowI].RowHeight+rows[rowI].NoteHeight-1));
			}
			//colored row background
			else if(rows[rowI].ColorBackG!=Color.White) {
				g.DrawRectangle(new XSolidBrush(rows[rowI].ColorBackG),
					p(x+1),
					p(y-vScroll.Value+1),//+titleHeight+headerHeight+rows[rowI].RowLoc+1,
					p(GridW),//this is a really simple width value that always works well
					p(rows[rowI].RowHeight+rows[rowI].NoteHeight-1));
			}
			//normal row color
			else {//need to draw over the gray background
				g.DrawRectangle(new XSolidBrush(rows[rowI].ColorBackG),
					p(x+1),
					p(y-vScroll.Value+1),//+titleHeight+headerHeight+rows[rowI].RowLoc+1,
					p(GridW),//this is a really simple width value that always works well
					p(rows[rowI].RowHeight+rows[rowI].NoteHeight-1));
			}
			if(selectionMode==GridSelectionMode.OneCell && selectedCell.X!=-1 && selectedCell.Y!=-1
			&& selectedCell.Y==rowI) {
				g.DrawRectangle(new XSolidBrush(selectedRowColor),
					p(x-hScroll.Value+1+ColPos[selectedCell.X]),
					p(y-vScroll.Value+1),//+titleHeight+headerHeight+rows[rowI].RowLoc+1,
					p(_listCurColumnWidths[selectedCell.X]),
					p(rows[rowI].RowHeight+rows[rowI].NoteHeight-1));
			}
			//lines for note section
			if(rows[rowI].NoteHeight>0) {
				//left vertical gridline
				if(NoteSpanStart!=0) {
					g.DrawLine(gridPen,
						p(x-hScroll.Value+1+ColPos[NoteSpanStart]),
						p(y-vScroll.Value+1+rows[rowI].RowHeight),//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI],
						p(x-hScroll.Value+1+ColPos[NoteSpanStart]),
						p(y-vScroll.Value+1+rows[rowI].RowHeight+rows[rowI].NoteHeight));//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI]+rows[rowI].NoteHeight);
				}
				//Horizontal line which divides the main part of the row from the notes section of the row
				g.DrawLine(gridPen,
					p(x-hScroll.Value+1+ColPos[0]+1),
					p(y-vScroll.Value+1+rows[rowI].RowHeight),//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI],
					p(x-hScroll.Value+1+ColPos[columns.Count-1]+_listCurColumnWidths[columns.Count-1]),
					p(y-vScroll.Value+1+rows[rowI].RowHeight));//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI]);
			}
			for(int i=0;i<columns.Count;i++) {
				//right vertical gridline
				if(rowI==0) {
					g.DrawLine(gridPen,
						p(x-hScroll.Value+ColPos[i]+_listCurColumnWidths[i]),
						p(y-vScroll.Value+1),//+rows[rowI].RowLoc,//+titleHeight+headerHeight+rows[rowI].RowLoc,
						p(x-hScroll.Value+ColPos[i]+_listCurColumnWidths[i]),
						p(y-vScroll.Value+1+rows[rowI].RowHeight));//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI]);
				}
				else {
					g.DrawLine(gridPen,
						p(x-hScroll.Value+ColPos[i]+_listCurColumnWidths[i]),
						p(y-vScroll.Value+1),//+rows[rowI].RowLoc,//+titleHeight+headerHeight+rows[rowI].RowLoc+1,
						p(x-hScroll.Value+ColPos[i]+_listCurColumnWidths[i]),
						p(y-vScroll.Value+1+rows[rowI].RowHeight));//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI]);
				}
				//lower horizontal gridline
				if(i==0) {
					g.DrawLine(lowerPen,
						p(x-hScroll.Value+ColPos[i]),
						p(y-vScroll.Value+1+rows[rowI].RowHeight+rows[rowI].NoteHeight),//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI]+rows[rowI].NoteHeight,
						p(x-hScroll.Value+ColPos[i]+_listCurColumnWidths[i]),
						p(y-vScroll.Value+1+rows[rowI].RowHeight+rows[rowI].NoteHeight));//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI]+rows[rowI].NoteHeight);
				}
				else {
					g.DrawLine(lowerPen,
						p(x-hScroll.Value+ColPos[i]+1),
						p(y-vScroll.Value+1+rows[rowI].RowHeight+rows[rowI].NoteHeight),//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI]+rows[rowI].NoteHeight,
						p(x-hScroll.Value+ColPos[i]+_listCurColumnWidths[i]),
						p(y-vScroll.Value+1+rows[rowI].RowHeight+rows[rowI].NoteHeight));//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI]+rows[rowI].NoteHeight);
				}
				//text
				if(rows[rowI].Cells.Count-1<i) {
					continue;
				}
				float adjH=0;
				switch(columns[i].TextAlign) {
					case HorizontalAlignment.Left:
						_xAlign=XStringAlignment.Near;
						adjH=1;
						break;
					case HorizontalAlignment.Center:
						_xAlign=XStringAlignment.Center;
						adjH=_listCurColumnWidths[i]/2f;
						break;
					case HorizontalAlignment.Right:
						_xAlign=XStringAlignment.Far;
						adjH=_listCurColumnWidths[i]-2;
						break;
				}
				int vertical=y-vScroll.Value-2;// +1+1;// +titleHeight+headerHeight+rows[rowI].RowLoc+1;
				int horizontal=x-hScroll.Value+1+ColPos[i];
				int cellW=_listCurColumnWidths[i];
				int cellH=rows[rowI].RowHeight;
				if(HasEditableColumn) {//These cells are taller
					vertical+=2;//so this is to push text down to center it in the cell
					cellH-=3;//to keep it from spilling into the next cell
				}
				if(columns[i].TextAlign==HorizontalAlignment.Right) {
					if(HasEditableColumn) {
						horizontal-=4;
						cellW+=2;
					}
					else {
						horizontal-=2;
						cellW+=2;
					}
				}
				textRect=new XRect(p(horizontal+adjH),p(vertical),p(cellW),p(cellH));
				if(rows[rowI].Cells[i].ColorText==Color.Empty) {
					textBrush=new XSolidBrush(rows[rowI].ColorText);
				}
				else {
					textBrush=new XSolidBrush(rows[rowI].Cells[i].ColorText);
				}
				if(rows[rowI].Cells[i].Bold==YN.Yes) {
					cellFont=new XFont(tempFont.Name,tempFont.Size,XFontStyle.Bold);
				}
				else if(rows[rowI].Cells[i].Bold==YN.No) {
					cellFont=new XFont(tempFont.Name,tempFont.Size,XFontStyle.Regular);
				}
				else {//unknown.  Use row bold
					if(rows[rowI].Bold) {
						cellFont=new XFont(tempFont.Name,tempFont.Size,XFontStyle.Bold);
					}
					else {
						cellFont=new XFont(tempFont.Name,tempFont.Size,XFontStyle.Regular);
					}
				}
				//do not underline row if we are printing to PDF
				//if(rows[rowI].Cells[i].Underline==YN.Yes) {//Underline the current cell.  If it is already bold, make the cell bold and underlined.
				//	cellFont=new XFont(cellFont,(cellFont.Bold)?(XFontStyle.Bold | XFontStyle.Underline):XFontStyle.Underline);
				//}
				if(columns[i].ImageList==null) {
					DrawStringX(g,rows[rowI].Cells[i].Text,cellFont,textBrush,textRect,_xAlign);
				}
				else {
					int imageIndex=-1;
					if(rows[rowI].Cells[i].Text!="") {
						imageIndex=PIn.Int(rows[rowI].Cells[i].Text);
					}
					if(imageIndex!=-1) {
						XImage img=columns[i].ImageList.Images[imageIndex];
						g.DrawImage(img,horizontal,vertical-1);
					}
				}
			}
			//note text
			if(rows[rowI].NoteHeight>0 && NoteSpanStop>0 && NoteSpanStart<columns.Count) {
				int noteW=0;
				for(int i=NoteSpanStart;i<=NoteSpanStop;i++) {
					noteW+=_listCurColumnWidths[i];
				}
				if(rows[rowI].Bold) {
					cellFont=new XFont(tempFont.Name,tempFont.Size,XFontStyle.Bold);
				}
				else {
					cellFont=new XFont(tempFont.Name,tempFont.Size,XFontStyle.Regular);
				}
				textBrush=new XSolidBrush(rows[rowI].ColorText);
				textRect=new XRect(
					p(x-hScroll.Value+1+ColPos[NoteSpanStart]+1),
					p(y-vScroll.Value+1+rows[rowI].RowHeight+1),//+titleHeight+headerHeight+rows[rowI].RowLoc+rowHeights[rowI]+1,
					p(ColPos[NoteSpanStop]+_listCurColumnWidths[NoteSpanStop]-ColPos[NoteSpanStart]),
					p(rows[rowI].NoteHeight));
				_xAlign=XStringAlignment.Near;
				DrawStringX(g,rows[rowI].Note,cellFont,textBrush,textRect,_xAlign);
			}
			//Left right and bottom lines of grid.  This creates the outline of the entire grid when not using outline control
			//Outline the Title
			XPen pen=new XPen(cOutline);
			//Draw line from LL to UL to UR to LR. top three sides of a rectangle.
			g.DrawLine(pen,p(x),p(y),p(x),p(y+rows[rowI].RowHeight+rows[rowI].NoteHeight+1));//left side line
			g.DrawLine(pen,p(x+Width),p(y),p(x+Width),p(y+rows[rowI].RowHeight+rows[rowI].NoteHeight+1));//right side line
			if(isBottom) {
				g.DrawLine(pen,p(x),p(y+rows[rowI].RowHeight+rows[rowI].NoteHeight)+1,p(x+Width),p(y+rows[rowI].RowHeight+rows[rowI].NoteHeight+1));//bottom line.
			}
			tempFont.Dispose();
		}

		public void PrintTitle(Graphics g,int x,int y) {
			Color cTitleTop=Color.White;
			Color cTitleBottom=Color.FromArgb(213,213,223);
			Color cTitleText=Color.Black;
			if(_useBlueTheme) {
				cTitleTop=Color.FromArgb(156,175,230);//191,205,245);//139,160,224);//114,136,201);//106,132,210);//109,129,191);//104,136,232);
				cTitleBottom=Color.FromArgb(60,90,150);//35,55,115);//49,63,105);//(20,47,126);
				cTitleText=Color.White;
			}
			LinearGradientBrush brushTitleBackground=new LinearGradientBrush(new Rectangle(x,y,Width,titleHeight),cTitleTop,cTitleBottom,LinearGradientMode.Vertical);
			SolidBrush brushTitleText=new SolidBrush(cTitleText);
			g.FillRectangle(brushTitleBackground,x,y,Width,titleHeight);
			g.DrawString(title,_titleFont,brushTitleText,x+(Width/2-g.MeasureString(title,_titleFont).Width/2),y+2);
			//Outline the Title
			Color cOutline=Color.FromArgb(119,119,146);
			if(_useBlueTheme) {
				cOutline=Color.FromArgb(47,70,117);
			}
			using(Pen pen=new Pen(cOutline)) {
				//Draw line from LL to UL to UR to LR. top three sides of a rectangle.
				g.DrawLines(pen,new Point[] { 
					new Point(x,y+titleHeight),
					new Point(x,y),
					new Point(x+Width,y),
					new Point(x+Width,y+titleHeight) });
				//g.DrawRectangle(pen,0,0,Width-1,Height-1);
			}
			if(brushTitleBackground!=null) {
				brushTitleBackground.Dispose();
				brushTitleBackground=null;
			}
			if(brushTitleText!=null) {
				brushTitleText.Dispose();
				brushTitleText=null;
			}
		}

		public void PrintTitleX(XGraphics g,int x,int y) {
			Color cTitleTop=Color.White;
			Color cTitleBottom=Color.FromArgb(213,213,223);
			Color cTitleText=Color.Black;
			if(_useBlueTheme) {
				cTitleTop=Color.FromArgb(156,175,230);//191,205,245);//139,160,224);//114,136,201);//106,132,210);//109,129,191);//104,136,232);
				cTitleBottom=Color.FromArgb(60,90,150);//35,55,115);//49,63,105);//(20,47,126);
				cTitleText=Color.White;
			}
			LinearGradientBrush brushTitleBackground=new LinearGradientBrush(new Rectangle(x,y,Width,titleHeight),cTitleTop,cTitleBottom,LinearGradientMode.Vertical);
			XSolidBrush brushTitleText=new XSolidBrush(cTitleText);
			g.DrawRectangle(brushTitleBackground,p(x),p(y),p(Width),p(titleHeight));
			XFont xTitleFont=new XFont(_titleFont.FontFamily.ToString(),_titleFont.Size,XFontStyle.Bold);
			//g.DrawString(title,titleFont,brushTitleText,p((float)x+(float)(Width/2-g.MeasureString(title,titleFont).Width/2)),p(y+2));
			DrawStringX(g,title,xTitleFont,brushTitleText,new XRect((float)x+(float)(Width/2),y,100,100),XStringAlignment.Center);
			//Outline the Title
			Color cOutline=Color.FromArgb(119,119,146);
			if(_useBlueTheme) {
				cOutline=Color.FromArgb(47,70,117);
			}
			using(Pen pen=new Pen(cOutline)) {
				//Draw line from LL to UL to UR to LR. top three sides of a rectangle.
				g.DrawLines(pen,new Point[] { 
					new Point(x,y+titleHeight),
					new Point(x,y),
					new Point(x+Width,y),
					new Point(x+Width,y+titleHeight) });
				//g.DrawRectangle(pen,0,0,Width-1,Height-1);
			}
			if(brushTitleBackground!=null) {
				brushTitleBackground.Dispose();
				brushTitleBackground=null;
			}
		}

		public void PrintHeader(Graphics g,int x,int y) {
			Color cOutline=cOutline=Color.Black;
			Color cTitleTop=Color.White;
			Color cTitleBottom=Color.FromArgb(213,213,223);
			Color cTitleText=Color.Black;
			Color cTitleBackG=Color.LightGray;
			g.FillRectangle(new SolidBrush(cTitleBackG),x,y,Width,headerHeight);//background
			g.DrawLine(new Pen(Color.FromArgb(102,102,122)),x,y,x+Width,y);//line between title and headers
			for(int i=0;i<columns.Count;i++) {
				if(i!=0) {
					//vertical lines separating column headers
					g.DrawLine(new Pen(cOutline),x+(-hScroll.Value+ColPos[i]),y,
						x+(-hScroll.Value+ColPos[i]),y+headerHeight);
				}
				g.DrawString(columns[i].Heading,_headerFont,Brushes.Black,
					(float)x+(-hScroll.Value+ColPos[i]+_listCurColumnWidths[i]/2-g.MeasureString(columns[i].Heading,_headerFont).Width/2),
					(float)y+1);
				if(sortedByColumnIdx==i) {
					PointF p=new PointF(x+(-hScroll.Value+1+ColPos[i]+6),y+(float)headerHeight/2f);
					if(sortedIsAscending) { //pointing up
						g.FillPolygon(Brushes.White,new PointF[] {
							new PointF(p.X-4.9f,p.Y+2f), //LLstub
							new PointF(p.X-4.9f,p.Y+2.5f), //LLbase
							new PointF(p.X+4.9f,p.Y+2.5f), //LRbase
							new PointF(p.X+4.9f,p.Y+2f), //LRstub
							new PointF(p.X,p.Y-2.8f)
						}); //Top
						g.FillPolygon(Brushes.Black,new PointF[] {
							new PointF(p.X-4,p.Y+2), //LL
							new PointF(p.X+4,p.Y+2), //LR
							new PointF(p.X,p.Y-2)
						}); //Top
					}
					else { //pointing down
						g.FillPolygon(Brushes.White,new PointF[] { //shaped like home plate
							new PointF(p.X-4.9f,p.Y-2f), //ULstub
							new PointF(p.X-4.9f,p.Y-2.7f), //ULtop
							new PointF(p.X+4.9f,p.Y-2.7f), //URtop
							new PointF(p.X+4.9f,p.Y-2f), //URstub
							new PointF(p.X,p.Y+2.8f)
						}); //Bottom
						g.FillPolygon(Brushes.Black,new PointF[] {
							new PointF(p.X-4,p.Y-2), //UL
							new PointF(p.X+4,p.Y-2), //UR
							new PointF(p.X,p.Y+2)
						}); //Bottom
					}
				}
			} //end for columns.Count
			//Outline the Title
			using(Pen pen=new Pen(cOutline)) {
				g.DrawRectangle(pen,x,y,Width,HeaderHeight);
			}
				g.DrawLine(new Pen(cOutline),x,y+headerHeight,x+Width,y+headerHeight);
		}

		public void PrintHeaderX(XGraphics g,int x,int y) {
			Color cOutline=cOutline=Color.Black;
			Color cTitleTop=Color.White;
			Color cTitleBottom=Color.FromArgb(213,213,223);
			Color cTitleText=Color.Black;
			Color cTitleBackG=Color.LightGray;
			g.DrawRectangle(new XSolidBrush(cTitleBackG),p(x),p(y),p(Width),p(headerHeight));//background
			g.DrawLine(new XPen(Color.FromArgb(102,102,122)),p(x),p(y),p(x+Width),p(y));//line between title and headers
			XFont xHeaderFont=new XFont(_headerFont.FontFamily.Name.ToString(),_headerFont.Size,XFontStyle.Bold);
			for(int i=0;i<columns.Count;i++) {
				if(i!=0) {
					g.DrawLine(new XPen(cOutline),p(x+(-hScroll.Value+ColPos[i])),p(y),
						p(x+(-hScroll.Value+ColPos[i])),p(y+headerHeight));
				}
				float xFloat=(float)x+(float)(-hScroll.Value+ColPos[i]+_listCurColumnWidths[i]/2);//for some reason visual studio would not allow this statement within the DrawString Below.
				DrawStringX(g,columns[i].Heading,xHeaderFont,XBrushes.Black,new XRect(p(xFloat),p(y-3),100,100),XStringAlignment.Center);
			}//end for columns.Count
			//Outline the Title
			XPen pen=new XPen(cOutline);
			g.DrawRectangle(pen,p(x),p(y),p(Width),p(HeaderHeight));
			g.DrawLine(new XPen(cOutline),p(x),p(y+headerHeight),p(x+Width),p(y+headerHeight));
		}

		///<summary>(Not used for sheet printing) If there are more pages to print, it returns -1.  If this is the last page, it returns the yPos of where the printing stopped.  Graphics will be paper, pageNumber resets some class level variables at page 0, bounds are used to contain the grid drawing, and marginTopFirstPage leaves room so as to not overwrite the title and subtitle.</summary>
		public int PrintPage(Graphics g,int pageNumber,Rectangle bounds,int marginTopFirstPage,bool HasHeaderSpaceOnEveryPage=false) {
			//Printers ignore TextRenderingHint.AntiAlias.  
			//And they ignore SmoothingMode.HighQuality.
			//They seem to do font themselves instead of letting us have control.
			//g.TextRenderingHint=TextRenderingHint.AntiAlias;//an attempt to fix the printing measurements.
			//g.SmoothingMode=SmoothingMode.HighQuality;
			//g.PageUnit=GraphicsUnit.Display;
			//float pagescale=g.PageScale;
			//g.PixelOffsetMode=PixelOffsetMode.HighQuality;
			//g.
			if(RowsPrinted==0) {
				//set row heights 4% larger when printing:
				ComputeRows(g);
			}
			int xPos=bounds.Left;
			//now, try to center in bounds
			if((float)GridW<bounds.Width) {
				xPos=(int)(bounds.Left+bounds.Width/2-(float)GridW/2);
			}
			Pen gridPen;
			Pen lowerPen;
			SolidBrush textBrush;
			RectangleF textRect;
			Font cellFont=new Font(_cellFont,_cellFont.Style);
			//Initialize our pens for drawing.
			gridPen=new Pen(this.cGridLine);
			lowerPen=new Pen(this.cGridLine);
			int yPos=bounds.Top;
			if(HasHeaderSpaceOnEveryPage) {
				yPos=marginTopFirstPage;//Margin is lower because title and subtitle are printed externally.
			}
			if(pageNumber==0) {
				yPos=marginTopFirstPage;//Margin is lower because title and subtitle are printed externally.
				RowsPrinted=0;
				NoteRemaining="";
			}
			bool isFirstRowOnPage=true;//helps with handling a very tall first row
			if(RowsPrinted==rows.Count-1) {//last row
				lowerPen=new Pen(Color.FromArgb(120,120,120));
			}
			else {
				if(rows.Count>0 && rows[RowsPrinted].ColorLborder!=Color.Empty) {
					lowerPen=new Pen(rows[RowsPrinted].ColorLborder);
				}
			}
			try {
				#region ColumnHeaders
				//Print column headers on every page.
				g.FillRectangle(Brushes.LightGray,xPos+ColPos[0],yPos,(float)GridW,headerHeight);
				g.DrawRectangle(Pens.Black,xPos+ColPos[0],yPos,(float)GridW,headerHeight);
				for(int i=1;i<ColPos.Length;i++) {
					g.DrawLine(Pens.Black,xPos+(float)ColPos[i],yPos,xPos+(float)ColPos[i],yPos+headerHeight);
				}
				for(int i=0;i<columns.Count;i++) {
					g.DrawString(columns[i].Heading,_headerFont,Brushes.Black,
						xPos+(float)ColPos[i]+_listCurColumnWidths[i]/2-g.MeasureString(columns[i].Heading,_headerFont).Width/2,
						yPos);
				}
				yPos+=headerHeight;
				#endregion ColumnHeaders
				#region Rows
				while(RowsPrinted<rows.Count) {
					#region RowMainPart
					if(NoteRemaining=="") {//We are not in the middle of a note from a previous page. If we are in the middle of a note that will get printed next, as it is the next region of code (RowNotePart).
						//Go to next page if it doesn't fit.
						if(yPos+(float)rows[RowsPrinted].RowHeight > bounds.Bottom) {//The row is too tall to fit
							if(isFirstRowOnPage) {
								//todo some day: handle very tall first rows.  For now, print what we can.
							}
							else {
								break;//Go to next page.
							}
						}
						//There is enough room to print this row.
						//Draw the left vertical gridline
						g.DrawLine(gridPen,
							xPos+ColPos[0],
							yPos,
							xPos+ColPos[0],
							yPos+(float)rows[RowsPrinted].RowHeight);
						for(int i=0;i<columns.Count;i++) {
							//Draw the other vertical gridlines
							g.DrawLine(gridPen,
								xPos+(float)ColPos[i]+(float)_listCurColumnWidths[i],
								yPos,
								xPos+(float)ColPos[i]+(float)_listCurColumnWidths[i],
								yPos+(float)rows[RowsPrinted].RowHeight);
							if(rows[RowsPrinted].Note=="") {//End of row. Mark with a dark line (lowerPen).
								//Horizontal line which divides the main part of the row from the notes section of the row one column at a time.
								g.DrawLine(lowerPen,
									xPos+ColPos[0],
									yPos+(float)rows[RowsPrinted].RowHeight,
									xPos+(float)ColPos[columns.Count-1]+(float)_listCurColumnWidths[columns.Count-1],
									yPos+(float)rows[RowsPrinted].RowHeight);
							}
							else {//Middle of row. Still need to print the note part of the row. Mark with a medium line (gridPen).
								//Horizontal line which divides the main part of the row from the notes section of the row one column at a time.
								g.DrawLine(gridPen,
									xPos+ColPos[0],
									yPos+(float)rows[RowsPrinted].RowHeight,
									xPos+(float)ColPos[columns.Count-1]+(float)_listCurColumnWidths[columns.Count-1],
									yPos+(float)rows[RowsPrinted].RowHeight);
							}
							//text
							if(rows[RowsPrinted].Cells.Count-1<i) {
								continue;
							}
							switch(columns[i].TextAlign) {
								case HorizontalAlignment.Left:
									_format.Alignment=StringAlignment.Near;
									break;
								case HorizontalAlignment.Center:
									_format.Alignment=StringAlignment.Center;
									break;
								case HorizontalAlignment.Right:
									_format.Alignment=StringAlignment.Far;
									break;
							}
							if(rows[RowsPrinted].Cells[i].ColorText==Color.Empty) {
								textBrush=new SolidBrush(rows[RowsPrinted].ColorText);
							}
							else {
								textBrush=new SolidBrush(rows[RowsPrinted].Cells[i].ColorText);
							}
							if(rows[RowsPrinted].Cells[i].Bold==YN.Yes) {
								cellFont=new Font(cellFont,FontStyle.Bold);
							}
							else if(rows[RowsPrinted].Cells[i].Bold==YN.No) {
								cellFont=new Font(cellFont,FontStyle.Regular);
							}
							else {//unknown.  Use row bold
								if(rows[RowsPrinted].Bold) {
									cellFont=new Font(cellFont,FontStyle.Bold);
								}
								else {
									cellFont=new Font(cellFont,FontStyle.Regular);
								}
							}
							//Do not underline if printing grid
							//if(rows[RowsPrinted].Cells[i].Underline==YN.Yes) {//Underline the current cell.  If it is already bold, make the cell bold and underlined.
							//	cellFont=new Font(cellFont,(cellFont.Bold)?(FontStyle.Bold | FontStyle.Underline):FontStyle.Underline);
							//}
							//Some printers will malfunction (BSOD) if print bold colored fonts.  This prevents the error.
							if(textBrush.Color!=Color.Black && cellFont.Bold) {
								cellFont=new Font(cellFont,FontStyle.Regular);
							}
							if(columns[i].TextAlign==HorizontalAlignment.Right) {
								textRect=new RectangleF(
									xPos+(float)ColPos[i]-2,
									yPos,
									(float)_listCurColumnWidths[i]+2,
									(float)rows[RowsPrinted].RowHeight);
								//shift the rect to account for MS issue with strings of different lengths
								//js- 5/2/11,I don't understand this.  I would like to research it.
								textRect.Location=new PointF
									(textRect.X+g.MeasureString(rows[RowsPrinted].Cells[i].Text,cellFont).Width/textRect.Width,
									textRect.Y);
								//g.DrawString(rows[RowsPrinted].Cells[i].Text,cellFont,textBrush,textRect,_format);

							}
							else {
								textRect=new RectangleF(
									xPos+(float)ColPos[i],
									yPos,
									(float)_listCurColumnWidths[i],
									(float)rows[RowsPrinted].RowHeight);
								//g.DrawString(rows[RowsPrinted].Cells[i].Text,cellFont,textBrush,textRect,_format);
							}
							g.DrawString(rows[RowsPrinted].Cells[i].Text,cellFont,textBrush,textRect,_format);
						}
						yPos+=(int)((float)rows[RowsPrinted].RowHeight);//Move yPos down the length of the row (not the note).
					}
					#endregion RowMainPart
					#region NotePart
					if(rows[RowsPrinted].Note=="") {
						RowsPrinted++;//There is no note. Go to next row.
						isFirstRowOnPage=false;
						continue; 
					}
					//Figure out how much vertical distance the rest of the note will take up.
					int noteHeight;
					int noteW=0;
					_format.Alignment=StringAlignment.Near;
					for(int i=NoteSpanStart;i<=NoteSpanStop;i++) {
						noteW+=(int)((float)_listCurColumnWidths[i]);
					}
					if(NoteRemaining=="") {//We are not in the middle of a note.
						if(rows[RowsPrinted].Note.Length<=_noteLengthLimit) {
							NoteRemaining=rows[RowsPrinted].Note;//The note remaining is the whole note.
						}
						else {
							NoteRemaining=rows[RowsPrinted].Note.Substring(0,_noteLengthLimit);//The note remaining is the whole note up to the note length limit.
						}
					}
					noteHeight=(int)g.MeasureString(NoteRemaining,cellFont,noteW,_format).Height; //This is how much height the rest of the note will take.
					bool roomForRestOfNote=false;
					//Test to see if there's enough room on the page for the rest of the note.
					if(yPos+noteHeight<bounds.Bottom) {
						roomForRestOfNote=true;
					}
					#region PrintRestOfNote
					if(roomForRestOfNote) { //There is enough room
						//print it
						//draw lines for the rest of the note
						if(noteHeight>0) {
							//left vertical gridline
							if(NoteSpanStart!=0) {
								g.DrawLine(gridPen,
									xPos+(float)ColPos[NoteSpanStart],
									yPos,
									xPos+(float)ColPos[NoteSpanStart],
									yPos+noteHeight);
							}
							//right vertical gridline
							g.DrawLine(gridPen,
								xPos+(float)ColPos[columns.Count-1]+(float)_listCurColumnWidths[columns.Count-1],
								yPos,
								xPos+(float)ColPos[columns.Count-1]+(float)_listCurColumnWidths[columns.Count-1],
								yPos+noteHeight);
							//left vertical gridline
							g.DrawLine(gridPen,
								xPos+ColPos[0],
								yPos,
								xPos+ColPos[0],
								yPos+noteHeight);
						}
						//lower horizontal gridline gets marked with the dark lowerPen since this is the end of a row
						g.DrawLine(lowerPen,
							xPos+ColPos[0],
							yPos+noteHeight,
							xPos+(float)ColPos[columns.Count-1]+(float)_listCurColumnWidths[columns.Count-1],
							yPos+noteHeight);
						//note text
						if(noteHeight>0 && NoteSpanStop>0 && NoteSpanStart<columns.Count) {
							if(rows[RowsPrinted].Bold) {
								cellFont=new Font(cellFont,FontStyle.Bold);
							}
							else {
								cellFont=new Font(cellFont,FontStyle.Regular);
							}
							textBrush=new SolidBrush(rows[RowsPrinted].ColorText);
							textRect=new RectangleF(
								xPos+(float)ColPos[NoteSpanStart]+1,
								yPos,
								(float)ColPos[NoteSpanStop]+(float)_listCurColumnWidths[NoteSpanStop]-(float)ColPos[NoteSpanStart],
								noteHeight);
							g.DrawString(NoteRemaining,cellFont,textBrush,textRect,_format);
						}
						NoteRemaining="";
						RowsPrinted++;
						isFirstRowOnPage=false;
						yPos+=noteHeight;
					}
					#endregion PrintRestOfNote
					#region PrintPartOfNote
					else {//The rest of the note will not fit on this page.
						//Print as much as you can.
						noteHeight=bounds.Bottom-yPos;//This is the amount of space remaining.
						if(noteHeight<15) {
							return -1; //If noteHeight is less than this we will get a negative value for the rectangle of space remaining because we subtract 15 from this for the rectangle size when using measureString. This is because one line takes 15, and if there is 1 pixel of height available, measureString will fill it with text, which will then get partly cut off. So when we use measureString we will subtract 15 from the noteHeight.
						}							
						SizeF sizeF;
						int charactersFitted;
						int linesFilled;
						string noteFitted;//This is the part of the note we will print.
						//js- I'd like to incorporate ,StringFormat.GenericTypographic into the MeasureString, but can't find the overload.
						sizeF=g.MeasureString(NoteRemaining,cellFont,new SizeF(noteW,noteHeight-15),_format,out charactersFitted,out linesFilled);//Text that fits will be NoteRemaining.Substring(0,charactersFitted).
						noteFitted=NoteRemaining.Substring(0,charactersFitted);
						//draw lines for the part of the note that fits on this page
						if(noteHeight>0) {
							//left vertical gridline
							if(NoteSpanStart!=0) {
								g.DrawLine(gridPen,
									xPos+(float)ColPos[NoteSpanStart],
									yPos,
									xPos+(float)ColPos[NoteSpanStart],
									yPos+noteHeight);
							}
							//right vertical gridline
							g.DrawLine(gridPen,
								xPos+(float)ColPos[columns.Count-1]+(float)_listCurColumnWidths[columns.Count-1],
								yPos,
								xPos+(float)ColPos[columns.Count-1]+(float)_listCurColumnWidths[columns.Count-1],
								yPos+noteHeight);
							//left vertical gridline
							g.DrawLine(gridPen,
								xPos+ColPos[0],
								yPos,
								xPos+ColPos[0],
								yPos+noteHeight);
						}
						//lower horizontal gridline gets marked with gridPen since its still the middle of a row (still more note to print)
						g.DrawLine(gridPen,
							xPos+ColPos[0],
							yPos+noteHeight,
							xPos+(float)ColPos[columns.Count-1]+(float)_listCurColumnWidths[columns.Count-1],
							yPos+noteHeight);
						//note text
						if(noteHeight>0 && NoteSpanStop>0 && NoteSpanStart<columns.Count) {
							if(rows[RowsPrinted].Bold) {
								cellFont=new Font(cellFont,FontStyle.Bold);
							}
							else {
								cellFont=new Font(cellFont,FontStyle.Regular);
							}
							textBrush=new SolidBrush(rows[RowsPrinted].ColorText);
							textRect=new RectangleF(
								xPos+(float)ColPos[NoteSpanStart]+1,
								yPos,
								(float)ColPos[NoteSpanStop]+(float)_listCurColumnWidths[NoteSpanStop]-(float)ColPos[NoteSpanStart],
								noteHeight);
							g.DrawString(noteFitted,cellFont,textBrush,textRect,_format);
						}
						NoteRemaining=NoteRemaining.Substring(charactersFitted);
						break;
					}
					#endregion PrintPartOfNote
					#endregion Rows
				}
				#endregion Rows
			}
			finally {
				if(cellFont!=null) {
					cellFont.Dispose();
				}
			}
			if(RowsPrinted==rows.Count) {//done printing
				//set row heights back to screen heights.
				using(Graphics gfx=this.CreateGraphics()) {
					ComputeRows(gfx);
				}
				return yPos;
				
			}
			else{//more pages to print
				
				return -1;
			}
		}

		///<summary>Exports the grid to a text or Excel file. The user will have the opportunity to choose the location of the export file.</summary>
		public void Export(string fileName) {
			SaveFileDialog saveFileDialog=new SaveFileDialog();
			saveFileDialog.AddExtension=true;
			saveFileDialog.FileName=fileName;
			if(!Directory.Exists(PrefC.GetString(PrefName.ExportPath))) {
				try {
					Directory.CreateDirectory(PrefC.GetString(PrefName.ExportPath));
					saveFileDialog.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
				}
				catch {
					//initialDirectory will be blank
				}
			}
			else {
				saveFileDialog.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
			}
			saveFileDialog.Filter="Text files(*.txt)|*.txt|Excel Files(*.xls)|*.xls|All files(*.*)|*.*";
			saveFileDialog.FilterIndex=0;
			if(saveFileDialog.ShowDialog()!=DialogResult.OK) {
				return;
			}
			try {
				using(StreamWriter sw = new StreamWriter(saveFileDialog.FileName,false)) {
					String line="";
					for(int i = 0;i<Columns.Count;i++) {
						line+=Columns[i].Heading+"\t";
					}
					sw.WriteLine(line);
					for(int i = 0;i<Rows.Count;i++) {
						line="";
						for(int j = 0;j<Columns.Count;j++) {
							line+=Rows[i].Cells[j].Text;
							if(j<Columns.Count-1) {
								line+="\t";
							}
						}
						sw.WriteLine(line);
					}
				}
			}
			catch {
				MessageBox.Show(Lans.g(this,"File in use by another program.  Close and try again."));
				return;
			}
			MessageBox.Show(Lans.g(this,"File created successfully"));
		}

		#endregion Printing

		#region Selections
		///<summary>Throws exceptions.  Use to set a row selected or not.  Can handle values outside the acceptable range.</summary>
		public void SetSelected(int index,bool setValue) {
			if(setValue) {//select specified index
				if(selectionMode==GridSelectionMode.None) {
					throw new Exception("Selection mode is none.");
				}
				if(index<0 || index>RowsFiltered.Count-1) {//check to see if index is within the valid range of values
					return;//if not, then ignore.
				}
				if(selectionMode==GridSelectionMode.One) {
					selectedIndices.Clear();//clear existing selection before assigning the new one.
				}
				if(!selectedIndices.Contains(index)) {
					selectedIndices.Add(index);
				}
			}
			else {//unselect specified index
				if(selectedIndices.Contains(index)) {
					selectedIndices.Remove(index);
				}
			}
			Invalidate();
		}

		///<summary>Throws exceptions.  Similar to SetSelected, except will also deselect any items not specified in iArray.</summary>
		public void SetSelectedStrict(int[] iArray) {
			selectedIndices.Clear();//clear existing selection before assigning the new one.
			SetSelected(iArray,true);
		}

		///<summary>Throws exceptions.  Allows setting multiple values all at once</summary>
		public void SetSelected(int[] iArray,bool setValue) {
			if(selectionMode==GridSelectionMode.None) {
				throw new Exception("Selection mode is none.");
			}
			if(selectionMode==GridSelectionMode.One) {
				throw new Exception("Selection mode is one.");
			}
			for(int i=0;i<iArray.Length;i++) {
				if(setValue) {//select specified index
					if(iArray[i]<0 || iArray[i]>RowsFiltered.Count-1) {//check to see if index is within the valid range of values
						return;//if not, then ignore.
					}
					if(!selectedIndices.Contains(iArray[i])) {
						selectedIndices.Add(iArray[i]);
					}
				}
				else {//unselect specified index
					if(selectedIndices.Contains(iArray[i])) {
						selectedIndices.Remove(iArray[i]);
					}
				}
			}
			Invalidate();
		}

		///<summary>Throws exceptions.  Sets all rows to specified value.</summary>
		public void SetSelected(bool setValue) {
			if(selectionMode==GridSelectionMode.None) {
				throw new Exception("Selection mode is none.");
			}
			if(selectionMode==GridSelectionMode.One && setValue==true) {
				throw new Exception("Selection mode is one.");
			}
			if(selectionMode==GridSelectionMode.OneCell) {
				throw new Exception("Selection mode is OneCell.");
			}
			selectedIndices.Clear();
			if(setValue) {//select all
				for(int i=0;i<RowsFiltered.Count;i++) {
					selectedIndices.Add(i);
				}
			}
			Invalidate();
		}

		///<summary>Throws exceptions.</summary>
		public void SetSelected(Point setCell) {
			if(selectionMode!=GridSelectionMode.OneCell) {
				throw new Exception("Selection mode must be OneCell.");
			}
			selectedCell=setCell;
			if(editBox!=null) {
				editBox.Dispose();
			}
			if(Columns[selectedCell.X].IsEditable) {
				CreateEditBox();
			}
			Invalidate();
		}

		///<summary>If one row is selected, it returns the index to that row.  If more than one row are selected, it returns the first selected row.  Really only useful for SelectionMode.One.  If no rows selected, returns -1.</summary>
		public int GetSelectedIndex() {
			if(SelectionMode==GridSelectionMode.OneCell) {
				return selectedCell.Y;
			}
			if(selectedIndices.Count>0) {
				return (int)selectedIndices[0];
			}
			return -1;
		}

		private void SelectionAddedByUser(int index) {
			selectedIndices.Add(index);
			if(OnSelectionCommitted!=null) {
				EventArgs args=new EventArgs();
				OnSelectionCommitted(this,args);
			}
		}

		///<summary>Returns the tag of the selected item. If T does not match the actual tag, it will return the default of T (usually null).</summary>
		public T SelectedTag<T>() {
			if(SelectedIndices.Length > 0 && Rows[GetSelectedIndex()].Tag is T) {
				return (T)Rows[GetSelectedIndex()].Tag;
			}
			return default(T);
		}

		///<summary>Returns the tags of the selected items. If T does not match the actual tag, it will return the default of T (usually null).</summary>
		public List<T> SelectedTags<T>() {
			List<T> listTags=new List<T>();
			foreach(int selectedIndex in SelectedIndices) {
				if(Rows[selectedIndex].Tag is T) {
					listTags.Add((T)Rows[selectedIndex].Tag);
				}
			}
			return listTags;
		}

		#endregion Selections

		#region Scrolling
		private void LayoutScrollBars() {
			setHeaderHeightHelper();
			vScroll.Location=new Point(this.Width-vScroll.Width-1,titleHeight+headerHeight+1);
			if(this.hScrollVisible) {
				hScroll.Visible=true;
				vScroll.Height=this.Height-titleHeight-headerHeight-hScroll.Height-2;
				hScroll.Location=new Point(1,this.Height-hScroll.Height-1);
				hScroll.Width=this.Width-vScroll.Width-2;
				if(GridW<hScroll.Width) {
					hScroll.Value=0;
					hScroll.Enabled=false;
				}
				else {
					hScroll.Enabled=true;
					hScroll.Minimum = 0;
					hScroll.Maximum=GridW;
					hScroll.LargeChange=(hScroll.Width<0?0:hScroll.Width);//Don't allow negative width (will throw exception).
					hScroll.SmallChange=(int)(50);
				}

			}
			else {
				hScroll.Visible=false;
				vScroll.Height=this.Height-titleHeight-headerHeight-2;
			}
			if(vScroll.Height<=0) {
				return;
			}
			//hScroll support incomplete
			if(GridH<vScroll.Height) {
				vScroll.Value=0;
				vScroll.Enabled=false;
			}
			else {
				vScroll.Enabled=true;
				vScroll.Minimum = 0;
				vScroll.Maximum=GridH;
				vScroll.LargeChange=vScroll.Height;//it used to crash right here as it tried to assign a negative number.
				vScroll.SmallChange=(int)(14*3.4);//it's not an even number so that it is obvious to user that rows moved
			}
			//vScroll.Value=0;
		}

		private void vScroll_Scroll(object sender,System.Windows.Forms.ScrollEventArgs e) {
			if(editBox!=null) {
				editBox.Dispose();
			}
			Invalidate();
			this.Focus();
		}

		private void hScroll_Scroll(object sender,System.Windows.Forms.ScrollEventArgs e) {
			//if(UpDownKey) return;
			Invalidate();
			this.Focus();
		}

		///<summary>Usually called after entering a new list to automatically scroll to the end.</summary>
		public void ScrollToEnd() {
			ScrollValue=vScroll.Maximum;//this does all error checking and invalidates
		}
		#endregion Scrolling

		#region Sorting
		///<summary>Set sortedByColIdx to -1 to clear sorting. Copied from SortByColumn. No need to call fill grid after calling this.  Also used in PatientPortalManager.</summary>
		public void SortForced(int sortedByColIdx,bool isAsc) {
			sortedIsAscending=isAsc;
			sortedByColumnIdx=sortedByColIdx;
			if(sortedByColIdx==-1) {
				return;
			}
			List<ODGridRow> rowsSorted=new List<ODGridRow>();
			for(int i=0;i<rows.Count;i++) {
				rowsSorted.Add(rows[i]);
			}
			if(columns[sortedByColumnIdx].SortingStrategy==GridSortingStrategy.StringCompare) {
				rowsSorted.Sort(SortStringCompare);
			}
			else if(columns[sortedByColumnIdx].SortingStrategy==GridSortingStrategy.DateParse) {
				rowsSorted.Sort(SortDateParse);
			}
			else if(columns[sortedByColumnIdx].SortingStrategy==GridSortingStrategy.ToothNumberParse) {
				rowsSorted.Sort(SortToothNumberParse);
			}
			else if(columns[sortedByColumnIdx].SortingStrategy==GridSortingStrategy.AmountParse) {
				rowsSorted.Sort(SortAmountParse);
			}
			else if(columns[sortedByColumnIdx].SortingStrategy==GridSortingStrategy.TimeParse) {
				rowsSorted.Sort(SortTimeParse);
			}
			else if(columns[sortedByColumnIdx].SortingStrategy==GridSortingStrategy.VersionNumber) {
				rowsSorted.Sort(SortVersionParse);
			}
			BeginUpdate();
			rows.Clear();
			for(int i=0;i<rowsSorted.Count;i++) {
				rows.Add(rowsSorted[i]);
			}
			EndUpdate();
			sortedByColumnIdx=sortedByColIdx;//Must be set again since set to -1 in EndUpdate();
		}

		///<summary>Gets run on mouse down on a column header.</summary>
		private void SortByColumn(int mouseDownCol) {
			if(mouseDownCol==-1) {
				return;
			}
			if(sortedByColumnIdx==mouseDownCol) {//already sorting by this column
				sortedIsAscending=!sortedIsAscending;//switch ascending/descending.
			}
			else {
				sortedIsAscending=true;//start out ascending
				sortedByColumnIdx=mouseDownCol;
			}
			List<ODGridRow> rowsSorted=new List<ODGridRow>();
			for(int i=0;i<rows.Count;i++) {
				rowsSorted.Add(rows[i]);
			}
			if(columns[sortedByColumnIdx].SortingStrategy==GridSortingStrategy.StringCompare) {
				rowsSorted.Sort(SortStringCompare);
			}
			else if(columns[sortedByColumnIdx].SortingStrategy==GridSortingStrategy.DateParse) {
				rowsSorted.Sort(SortDateParse);
			}
			else if(columns[sortedByColumnIdx].SortingStrategy==GridSortingStrategy.ToothNumberParse) {
				rowsSorted.Sort(SortToothNumberParse);
			}
			else if(columns[sortedByColumnIdx].SortingStrategy==GridSortingStrategy.AmountParse) {
				rowsSorted.Sort(SortAmountParse);
			}
			else if(columns[sortedByColumnIdx].SortingStrategy==GridSortingStrategy.TimeParse) {
				rowsSorted.Sort(SortTimeParse);
			}
			else if(columns[sortedByColumnIdx].SortingStrategy==GridSortingStrategy.VersionNumber) {
				rowsSorted.Sort(SortVersionParse);
			}
			BeginUpdate();
			rows.Clear();
			for(int i=0;i<rowsSorted.Count;i++) {
				rows.Add(rowsSorted[i]);
			}
			EndUpdate();
			sortedByColumnIdx=mouseDownCol;//Must be set again since set to -1 in EndUpdate();
			if(AllowSortingByColumn) { //only check this if sorting by column is enabled for the grid
				OnSortByColumn?.Invoke(this,new EventArgs());
			}
		}

		private int SortStringCompare(ODGridRow row1,ODGridRow row2) {
			return (sortedIsAscending?1:-1)*row1.Cells[sortedByColumnIdx].Text.CompareTo(row2.Cells[sortedByColumnIdx].Text);
		}

		private int SortDateParse(ODGridRow row1,ODGridRow row2) {
			string raw1=row1.Cells[sortedByColumnIdx].Text;
			string raw2=row2.Cells[sortedByColumnIdx].Text;
			DateTime date1=DateTime.MinValue;
			DateTime date2=DateTime.MinValue;
			//TryParse is a much faster operation than Parse in the event that the input won't parse to a date.
			if(DateTime.TryParse(raw1,out date1) &&
				DateTime.TryParse(raw2,out date2)) {
				return (sortedIsAscending?1:-1)*date1.CompareTo(date2);
			}
			else { //One of the inputs is not a date so default string compare.
				return SortStringCompare(row1,row2);
			}
		}

		private int SortTimeParse(ODGridRow row1,ODGridRow row2) {
			string raw1=row1.Cells[sortedByColumnIdx].Text;
			string raw2=row2.Cells[sortedByColumnIdx].Text;
			TimeSpan time1;
			TimeSpan time2;
			//TryParse is a much faster operation than Parse in the event that the input won't parse to a date.
			if(TimeSpan.TryParse(raw1,out time1) &&
				TimeSpan.TryParse(raw2,out time2)) {
				return (sortedIsAscending?1:-1)*time1.CompareTo(time2);
			}
			else { //One of the inputs is not a date so default string compare.
				return SortStringCompare(row1,row2);
			}
		}

		private int SortToothNumberParse(ODGridRow row1,ODGridRow row2) {
			//remember that teeth could be in international format.
			//fail gracefully
			string raw1=row1.Cells[sortedByColumnIdx].Text;
			string raw2=row2.Cells[sortedByColumnIdx].Text;
			if(!Tooth.IsValidEntry(raw1) && !Tooth.IsValidEntry(raw2)) {//both invalid
				return 0;
			}
			int retVal=0;
			if(!Tooth.IsValidEntry(raw1)) {//only first invalid
				retVal=-1; ;
			}
			else if(!Tooth.IsValidEntry(raw2)) {//only second invalid
				retVal=1; ;
			}
			else {//both valid
				string tooth1=Tooth.FromInternat(raw1);
				string tooth2=Tooth.FromInternat(raw2);
				int toothInt1=Tooth.ToInt(tooth1);
				int toothInt2=Tooth.ToInt(tooth2);
				retVal=toothInt1.CompareTo(toothInt2);
			}
			return (sortedIsAscending?1:-1)*retVal;
		}

		private int SortVersionParse(ODGridRow row1,ODGridRow row2) {
			Version v1=new Version(row1.Cells[sortedByColumnIdx].Text);
			Version v2=new Version(row2.Cells[sortedByColumnIdx].Text);
			return (sortedIsAscending?1:-1)*v1.CompareTo(v2);
		}

		private int SortAmountParse(ODGridRow row1,ODGridRow row2) {
			//This is here because AmountParse does not sort correctly when the amount contains non-numeric characters
			//We could improve this later with some kind of grid text cleaner that is called before running this sort.
			string raw1=row1.Cells[sortedByColumnIdx].Text;			
			raw1=raw1.Replace("$","");
			string raw2=row2.Cells[sortedByColumnIdx].Text;
			raw2=raw2.Replace("$","");
			Decimal amt1=0;
			Decimal amt2=0;
			if(raw1!="") {
				try {
					amt1=Decimal.Parse(raw1);
				}
				catch {
					return 0;//shouldn't happen
				}
			}
			if(raw2!="") {
				try {
					amt2=Decimal.Parse(raw2);
				}
				catch {
					return 0;//shouldn't happen
				}
			}
			return (sortedIsAscending?1:-1)*amt1.CompareTo(amt2);
		}

		///<summary>Swaps two rows in the grid. Returns false if either of the indexes is greater than the number of rows in the grid.</summary>
		public bool SwapRows(int indxMoveFrom,int indxMoveTo) {
			if(Rows.Count<=Math.Max(indxMoveFrom,indxMoveTo)
				|| Math.Min(indxMoveFrom,indxMoveTo)<0) 
			{
				return false;
			}
			BeginUpdate();
			ODGridRow dataRowFrom=Rows[indxMoveFrom];
			Rows[indxMoveFrom]=Rows[indxMoveTo];
			Rows[indxMoveTo]=dataRowFrom;
			EndUpdate();
			return true;
		}

		#endregion Sorting

		#region MouseEvents

		///<summary>Several location throughout the program the context menu changes. This subscribes each menu to use the popup helper below.</summary>
		protected override void OnContextMenuChanged(EventArgs e) {
			base.OnContextMenuChanged(e);
			if(HasLinkDetect && this.ContextMenu!=null) {
				this.ContextMenu.Popup+=PopupHelper;
			}
			if(this.ContextMenu==null) {
				this.ContextMenu=new ContextMenu();
			}
			this.ContextMenu.Popup+=CopyHelper;
		}

		///<summary>Just prior to displaying the context menu, add wiki links if neccesary.</summary>
		private void PopupHelper(object sender, EventArgs e) {
			//If multiple grids add the same instance of ConextMenu then all of them will raise this event any time any of them raise the event.
			//Only allow the event to operate if this is the grid that actually fired the event.
			try {
				if(((ContextMenu)sender).SourceControl.Name != this.Name) {
					return;
				}
			}
			catch(Exception ex) {
				ex.DoNothing();
				return;
			}
			//ODGrids are not only used within the Open Dental project.  Often times they are used in projects that do not have a DB connection.
			bool doWikiLogic=false;//Default the Wiki logic to false for all ODGrids.
			try {
				//NOTE: if this preference is changed while the program is open there MAY be some lingering wiki links in the context menu. 
				//It is not worth it to force users to log off and back on again, or to run the link removal code below EVERY time, even if the pref is disabled.
				doWikiLogic=PrefC.GetBool(PrefName.WikiDetectLinks);//if this fails then we do not have a pref table or a wiki, so don't bother going with this part.
			}
			catch(Exception ex) {
				ex.DoNothing();
			}
			removeContextMenuLinks();
			int rowClick = PointToRow(_currentClickLocation.Y);
			int colClick = PointToCol(_currentClickLocation.X);
			if(rowClick<0 || colClick<0) {//don't diplay links, not on grid row.
				return;
			}
			if(lastButtonPressed==MouseButtons.Right && rowClick<=this.Rows.Count) {
				ODGridRow row = this.Rows[rowClick];
				if(this.ContextMenu==null) {
					this.ContextMenu=new ContextMenu();
					return;
				}
				_listMenuItemLinks=new List<MenuItem>();
				if(this.ContextMenu.MenuItems.Count>0) {
					_listMenuItemLinks.Add(new MenuItem("-"));
				}
				StringBuilder sb = new StringBuilder();
				row.Cells.OfType<ODGridCell>().ToList().ForEach(x => sb.AppendLine(x.Text));
				sb.AppendLine(row.Note);
				List<string> listStringMatches;
				if(doWikiLogic) {
					listStringMatches=Regex.Matches(sb.ToString(),@"\[\[.+?]]")
						.OfType<Match>()
						.Select(m => m.Groups[0].Value.Trim('[').Trim(']'))
						.Distinct()
						.ToList();
					foreach(string match in listStringMatches) {
						_listMenuItemLinks.Add(new MenuItem("Wiki - "+match,(s,eArg) => { OpenWikiPage(match); }));
					}
				}
				listStringMatches=Regex.Matches(sb.ToString(),URL_REGEX_STRING).OfType<Match>().Select(m => m.Groups[0].Value).Distinct().ToList();
				foreach(string match in listStringMatches) {
					Regex rgx = new Regex(@"[\\]{1}");
					if(rgx.IsMatch(match)) {
						continue;
					}
					try {
						MailAddress emailAddress=new MailAddress(match);
						continue;//'match' is a valid email address, which at this time we don't want to create a ContextMenu Web link for.
					}
					catch(FormatException fe) {
						fe.DoNothing();//Not a valid email address format, so it should be a web link.  Carry on to creating the item in the ContextMenu.
					}
					string title=match;
					if(title.Length>=25) {
						title=title.Substring(0,25)+(match.Count()>=25?"...":"");
					}
					_listMenuItemLinks.Add(new MenuItem("Web - "+title,(s,eArg) => { OpenWebPage(match); }));
				}
				_listMenuItemLinks=_listMenuItemLinks.OrderByDescending(x => x.Text=="-").ThenBy(x => x.Text).ToList();//alphabetize the link items.
				if(_listMenuItemLinks.Any(x=>x.Text!="-")) {//at least one REAL menu item that is not the divider.
					_listMenuItemLinks.ForEach(x => this.ContextMenu.MenuItems.Add(x));
				}
			}
		}

		///<summary>Removes wiki and web links from context menu.</summary>
		private void removeContextMenuLinks() {
			if(this.ContextMenu==null || _listMenuItemLinks==null) {
				return;
			}
			foreach(MenuItem mi in _listMenuItemLinks) {
				this.ContextMenu.MenuItems.Remove(mi);
			}
		}

		///<summary>Just prior to displaying the context menu, add wiki links if neccesary.</summary>
		protected virtual void CopyHelper(object sender,EventArgs e) {
			//If multiple grids add the same instance of ConextMenu then all of them will raise this event any time any of them raise the event.
			//Only allow the event to operate if this is the grid that actually fired the event.
			try {
				if(((ContextMenu)sender).SourceControl.Name != this.Name) {
					return;
				}
			}
			catch(Exception ex) {
				ex.DoNothing();
				return;
			}
			if(this.ContextMenu==null) {
				return;
			}
			MenuItem copyMenuItem = this.ContextMenu.MenuItems.OfType<MenuItem>().FirstOrDefault(x => x.Text == "Copy Cell Text");
			List<MenuItem> listMenuItems = new List<MenuItem>();
			if(copyMenuItem==null) {
				copyMenuItem = new MenuItem("Copy Cell Text",OnCopyCellClick);
				if(this.ContextMenu.MenuItems.Count > 0) {
					listMenuItems.Add(new MenuItem("-"));
				}
				listMenuItems.Add(copyMenuItem);
				listMenuItems.ForEach(x => this.ContextMenu.MenuItems.Add(x));
			}
			string rowFilteredText="";
			try {
				rowFilteredText=RowsFiltered[MouseDownRow].Cells[MouseDownCol].Text;
			}
			catch(Exception ex) {
				ex.DoNothing();//We have had reports of the above line causing a UE. We don't know how it happened, so we're just going to ignore it.
			}
			if(MouseDownRow == -1 || MouseDownCol == -1 || string.IsNullOrEmpty(rowFilteredText)) {
				copyMenuItem.Enabled = false;
			}
			else {
				copyMenuItem.Enabled = true;
			}
		}

		private void OnCopyCellClick(object sender,EventArgs e) {
			try {
				string copyText = RowsFiltered[MouseDownRow].Cells[MouseDownCol].Text;
				Clipboard.SetText(copyText);
			}
			catch {
				//show a message box?
			}
		}

		///<summary></summary>
		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			lastButtonPressed=e.Button;//used in the click event.
			_currentClickLocation=e.Location;//stored for later use during context menu display
			MouseDownRow=PointToRow(e.Y);
			MouseDownCol=PointToCol(e.X);
			if(e.Button==MouseButtons.Right) {
				if(selectedIndices.Contains(MouseDownRow)) {//If a currently selected row is clicked, then ignore right click.
					return;
				}
				//otherwise, row will be selected. Useful when using context menu.
			}
			MouseIsDown=true;
			if(e.Y < 1+titleHeight) {//mouse down was in the title section
				return;
			}
			if(e.Y < 1+titleHeight+headerHeight) {//mouse down was on a column header
				mouseIsDownInHeader=true;
				if(MouseDownCol!=-1 && columns[MouseDownCol].CustomClickEvent!=null) {
					columns[MouseDownCol].CustomClickEvent(null,null);
					return;
				}
				else if(allowSortingByColumn) {
					if(MouseDownCol==-1) {
						return;
					}
					SortByColumn(MouseDownCol);
					Invalidate();
					return;
				}
				else {
					return;
				}
			}
			if(MouseDownRow==-1) {//mouse down was below the grid rows
				return;
			}
			if(MouseDownCol==-1) {//mouse down was to the right of columns
				return;
			}
			if(!allowSelection) {
				return;//clicks do not trigger selection of rows, but cell click event still gets fired
			}
			switch(selectionMode) {
				case GridSelectionMode.None:
					return;
				case GridSelectionMode.One:
					selectedIndices.Clear();
					SelectionAddedByUser(MouseDownRow);
					break;
				case GridSelectionMode.OneCell:
					selectedIndices.Clear();
					//Point oldSelectedCell=selectedCell;
					//if(oldSelectedCell.X!=selectedCell.X || oldSelectedCell.Y!=selectedCell.Y){
					if(editBox!=null) {
						editBox.Dispose();//a lot happens right here, including a FillGrid() which sets selectedCell to -1,-1
					}
					if(comboBox!=null) {
						//We can only show one combo box at a time because the grid does not limit the number of rows that it shows.
						//If we were to always show every combo box in a column, we would most likely get a 'too many handles' exception.
						comboBox.Dispose();
					}
					selectedCell=new Point(MouseDownCol,MouseDownRow);
					if(Columns[selectedCell.X].IsEditable) {
						CreateEditBox();
						//When the edit text box was created, added to the control, and given focus, the chain of events stops and the OnClick event never gets fired.
						//We can guarantee that the user did in fact click on a cell at this point in the mouse down event.
						OnClick(e);
					}
					else if(Columns[selectedCell.X].ListDisplayStrings!=null) {
						CreateComboBox();
					}
					break;
				case GridSelectionMode.MultiExtended:
					if(ControlIsDown) {
						//we need to remember exactly which rows were selected the moment the mouse down started.
						//Then, if the mouse gets dragged up or down, the rows between mouse start and mouse end
						//will be set to the opposite of these remembered values.
						selectedIndicesWhenMouseDown=new ArrayList(selectedIndices);
						if(selectedIndices.Contains(MouseDownRow)) {
							selectedIndices.Remove(MouseDownRow);
						}
						else {
							selectedIndices.Add(MouseDownRow);
						}
					}
					else if(ShiftIsDown) {
						if(selectedIndices.Count==0) {
							selectedIndices.Add(MouseDownRow);
						}
						else {
							int fromRow=(int)selectedIndices[0];
							selectedIndices.Clear();
							if(MouseDownRow<fromRow) {//dragging down
								for(int i=MouseDownRow;i<=fromRow;i++) {
									selectedIndices.Add(i);
								}
							}
							else {
								for(int i=fromRow;i<=MouseDownRow;i++) {
									selectedIndices.Add(i);
								}
							}
						}
					}
					else {//ctrl or shift not down
						selectedIndices.Clear();
						selectedIndices.Add(MouseDownRow);
					}
					break;
			}
			Invalidate();
		}

		///<summary></summary>
		protected override void OnMouseUp(MouseEventArgs e) {
			base.OnMouseUp(e);
			if(_hasDistinctClickEvents) {
				if(_threadDistinctClickEvents==null) {
					_threadDistinctClickEvents=new ODThread(10,WorkerThread_ClickListener);
					_threadDistinctClickEvents.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception ex) => { }));
					_threadDistinctClickEvents.Name="ODGridClickListenerThread";
					_threadDistinctClickEvents.Start(true);
				}
				if(_mouseClickIdx==-1) {// This is the first mouse click.
					_mouseClickIdx=MouseDownRow;
					_dateTimeMouseClick=DateTime.Now;
				}
				if(_mouseClickIdx==MouseDownRow) {//Clicked on the same row.
					_mouseClickCount++;
				}
				else {//Clicked on a new row.
					//_mouseClickIdx=MouseDownRow;
					//_dateTimeMouseClick=DateTime.Now;
					//OnClickHelper(new EventArgs());
				}
			}
			//if(e.Button==MouseButtons.Right){
			//	return;
			//}		
			if(this.ContextMenu==null && this.ContextMenuStrip==null) {
				if(e.Button==MouseButtons.Right) {
					this.ContextMenu=new ContextMenu();
					this.ContextMenu.Show(this,_currentClickLocation);//triggers autofill via the popup helper.
				}
			}
			MouseIsDown=false;
			mouseIsDownInHeader=false;
		}
		
		private void WorkerThread_ClickListener(ODThread odThread) {
			try {
				Invoke(new DelegateClickListener(UpdateInputStates));
			}
			catch(ObjectDisposedException ex) {
				QuitThread();//Recreate the thread the next time the user clicks.
				if(this.IsDisposed) {
					//Joe - Mimics issue we ran into in ComboBoxMulti.cs
					//On rare occasion, the control is disposed near the same instant that it is Ivoked above.
					//In this situation, we are quiting the thread, thus we expect the control to be disposed and ignore the error.
					return;
				}
				throw ex;
			}
		}

		private void UpdateInputStates() {
			if(this.Focused==false) {
				//Allow the MouseUp event handler to process clicks again.
				_mouseClickIdx=-1;
				_mouseClickCount=0;
				_dateTimeMouseClick=DateTime.MinValue;
				return;
			}
			if(Control.MouseButtons.HasFlag(MouseButtons.Left)) {//We only want to fire click events once a click has finished.
				return;//Mouse is still down, and we want to wait for mouse up, so do nothing.
			}
			if(_mouseClickCount==0) {
				return;
			}
			//As long as the double click has not happened and the double click time has not elapsed, continue to give the user a chance to double click.
			if(_mouseClickCount==1 && (DateTime.Now-_dateTimeMouseClick).TotalMilliseconds < ODEnvironment.DoubleClickTime) {
				return;
			}
      if(_mouseClickCount==1) {
				OnClickHelper(new EventArgs());				
      }
      else {
        OnDoubleClickHelper(new EventArgs());
      }
      //Allow the MouseUp event handler to process clicks again.
      _mouseClickIdx=-1;
      _mouseClickCount=0;
			_dateTimeMouseClick=DateTime.MinValue;
		}

		///<summary>Creates combo boxes in the appropriate location of the grid so users can select and change them.</summary>
		private void CreateComboBox() {
			ODGridCell odGridCell=rows[selectedCell.Y].Cells[selectedCell.X];
			ODGridColumn odGridColumn=Columns[selectedCell.X];
			comboBox=new ComboBox();
			comboBox.FlatStyle=FlatStyle.Popup;
			comboBox.DropDownStyle=ComboBoxStyle.DropDownList;//Makes it non-editable
			int colWidth=(odGridColumn.DropDownWidth > 0) ? odGridColumn.DropDownWidth+1 : _listCurColumnWidths[selectedCell.X]+1;
			comboBox.Size=new Size(colWidth,rows[selectedCell.Y].RowHeight+1);
			comboBox.Location=new Point(-hScroll.Value+1+ColPos[selectedCell.X],
				-vScroll.Value+1+titleHeight+headerHeight+rows[selectedCell.Y].RowLoc+((rows[selectedCell.Y].RowHeight-comboBox.Size.Height)/2));//Centers the combo box vertically.
			comboBox.Items.Clear();
			for(int i=0;i<odGridColumn.ListDisplayStrings.Count;i++) {
				comboBox.Items.Add(odGridColumn.ListDisplayStrings[i]);
			}
			comboBox.SelectedIndex=odGridCell.SelectedIndex;
			comboBox.SelectionChangeCommitted+=new EventHandler(dropDownBox_SelectionChangeCommitted);
			comboBox.GotFocus+=new EventHandler(dropDownBox_GotFocus);
			comboBox.LostFocus+=new EventHandler(dropDownBox_LostFocus);
			this.Controls.Add(comboBox); 
			comboBox.Focus();
			SelectedCellOld=new Point(selectedCell.X,selectedCell.Y);
		}

		void dropDownBox_GotFocus(object sender,EventArgs e) {
			OnCellEnter(selectedCell.X,selectedCell.Y);
		}

		void dropDownBox_LostFocus(object sender,EventArgs e) {
			ComboBox comboBox=(ComboBox)sender;
			OnCellLeave(SelectedCellOld.X,SelectedCellOld.Y);
			if(!comboBox.Disposing || !comboBox.IsDisposed) {
				comboBox.Dispose();
				comboBox=null;
			}
		}

		void dropDownBox_SelectionChangeCommitted(object sender,EventArgs e) {
			rows[SelectedCell.Y].Cells[selectedCell.X].Text=comboBox.Items[comboBox.SelectedIndex].ToString();
			rows[SelectedCell.Y].Cells[selectedCell.X].SelectedIndex=comboBox.SelectedIndex;
			OnCellSelectionChangeCommitted(SelectedCell.X,selectedCell.Y);
		}

		///<summary>When selection mode is OneCell, and user clicks in a column that isEditable, then this edit box will appear.</summary>
		private void CreateEditBox() {
			int hScrollBarHeight=0;
			if(HScrollVisible) {
				hScrollBarHeight=SystemInformation.HorizontalScrollBarHeight;
			}
			//Check if new edit box location is below the display screen
			int editBoxLocationTop=-vScroll.Value+1+titleHeight+headerHeight+rows[selectedCell.Y].RowLoc+rows[selectedCell.Y].RowHeight+hScrollBarHeight;
			if(editBoxLocationTop > this.DisplayRectangle.Bottom) {
				int onScreenPixels=vScroll.Value+DisplayRectangle.Height-titleHeight-headerHeight-(rows[selectedCell.Y].RowLoc)-hScrollBarHeight;
				int offScreenPixels=rows[selectedCell.Y].RowHeight-onScreenPixels;
				if(offScreenPixels>0) {
					ScrollValue+=offScreenPixels;//Scrolling down
				}
			}
			else if(-vScroll.Value+1+titleHeight+headerHeight+rows[selectedCell.Y].RowLoc<this.DisplayRectangle.Top+titleHeight+headerHeight) {
				//If new edit box location is above the display screen
				ScrollToIndex(selectedCell.Y);//Scrolling up
			}
			if(_hasEditableRTF) {
				RichTextBox editRichBox=new RichTextBox();
				editRichBox.Multiline=true;
				editRichBox.BorderStyle=BorderStyle.FixedSingle;
				if(Columns[selectedCell.X].TextAlign==HorizontalAlignment.Right) {
					editRichBox.SelectionAlignment=HorizontalAlignment.Right;
				}
				//Rich text boxes have strange borders (3D looking) and so we have to manipulate the size and location differently.
				editRichBox.Size=new Size(_listCurColumnWidths[selectedCell.X]-1,rows[selectedCell.Y].RowHeight-1);
				editRichBox.Location=new Point(-hScroll.Value+2+ColPos[selectedCell.X],
					-vScroll.Value+2+titleHeight+headerHeight+rows[selectedCell.Y].RowLoc);
				editBox=editRichBox;
			}
			else {
				TextBox editTextBox=new TextBox();
				editTextBox.Multiline=true;
				if(Columns[selectedCell.X].TextAlign==HorizontalAlignment.Right) {
					editTextBox.TextAlign=HorizontalAlignment.Right;
				}
				//Rich text boxes have strange borders (3D looking) and so we have to manipulate the size and location differently.
				editTextBox.Size=new Size(_listCurColumnWidths[selectedCell.X]+1,rows[selectedCell.Y].RowHeight+1);
				editTextBox.Location=new Point(-hScroll.Value+1+ColPos[selectedCell.X],
					-vScroll.Value+1+titleHeight+headerHeight+rows[selectedCell.Y].RowLoc);
				editBox=editTextBox;
			}
			//If the cell's color is set manually, that color will also show up for this EditBox.
			editBox.BackColor=rows[selectedCell.Y].ColorBackG;
			editBox.Font=_cellFont;
			editBox.Text=Rows[selectedCell.Y].Cells[selectedCell.X].Text;
			editBox.TextChanged+=new EventHandler(editBox_TextChanged);
			editBox.GotFocus+=new EventHandler(editBox_GotFocus);
			editBox.LostFocus+=new EventHandler(editBox_LostFocus);
			editBox.KeyDown+=new KeyEventHandler(editBox_KeyDown);
			editBox.KeyUp+=new KeyEventHandler(editBox_KeyUp);
			editBox.AcceptsTab=true;
			this.Controls.Add(editBox);
			if(_hasEditableRTF) {
				//RichTextBox always allows return
				if(!editableAcceptsCR) {
					editBox.SelectAll();//Only select all when not multiline (editableAcceptsCR) i.e. proc list for editing fees selects all for easy overwriting.
				}
			}
			else {
				if(editableAcceptsCR) {//Allow the edit box to handle carriage returns/multiline text.
					((TextBox)editBox).AcceptsReturn=true;
				}
				else {
					editBox.SelectAll();//Only select all when not multiline (editableAcceptsCR) i.e. proc list for editing fees selects all for easy overwriting.
				}
			}
			//Set the cell of the current editBox so that the value of that cell is saved when it looses focus (used for mouse click).
			SelectedCellOld=new Point(selectedCell.X,selectedCell.Y);
			editBox.Focus();
		}

		void editBox_LostFocus(object sender,EventArgs e) {
			//editBox_Leave wouldn't catch all scenarios
			OnCellLeave(SelectedCellOld.X,SelectedCellOld.Y);
			if(editBox!=null && (!editBox.Disposing || !editBox.IsDisposed)) {
				editBox.Dispose();
				editBox=null;
			}
		}

		void editBox_GotFocus(object sender,EventArgs e) {
			OnCellEnter(SelectedCellOld.X,SelectedCellOld.Y);
		}

		void editBox_KeyDown(object sender,KeyEventArgs e) {
			if(e.Shift && e.KeyCode == Keys.Enter) {
				Rows[selectedCell.Y].Cells[selectedCell.X].Text+="\r\n";
				return;
			}
			if(e.KeyCode==Keys.Enter) {//usually move to the next cell
				if(editableAcceptsCR) {//When multiline it inserts a carriage return instead of moving to the next cell.
					return;
				}
				editBox_NextCell();
			}
			if(e.KeyCode==Keys.Down) {
				if(editableAcceptsCR) {//When multiline it moves down inside the text instead of down to the next cell.
					return;
				}
				if(SelectedCellOld.Y<rows.Count-1) {
					editBox.Dispose();
					editBox=null;
					selectedCell=new Point(SelectedCellOld.X,SelectedCellOld.Y+1);
					CreateEditBox();
				}
			}
			if(e.KeyCode==Keys.Up) {
				if(editableAcceptsCR) {//When multiline it moves up inside the text instead of up to the next cell.
					return;
				}
				if(SelectedCellOld.Y>0) {
					editBox.Dispose();
					editBox=null;
					selectedCell=new Point(SelectedCellOld.X,SelectedCellOld.Y-1);
					CreateEditBox();
				}
			}
			if(e.KeyCode==Keys.Tab) {
				editBox_NextCell();
			}
		}
		
		private void editBox_NextCell() {
			editBox.Dispose();//This fires editBox_LostFocus, which is where we call OnCellLeave.
			editBox=null;
			//find the next editable cell to the right.
			int nextCellToRight=-1;
			for(int i=SelectedCellOld.X+1;i<columns.Count;i++) {
				if(columns[i].IsEditable) {
					nextCellToRight=i;
					break;
				}
			}
			if(nextCellToRight!=-1) {
				selectedCell=new Point(nextCellToRight,SelectedCellOld.Y);
				CreateEditBox();
				return;
			}
			//can't move to the right, so attempt to move down.
			if(SelectedCellOld.Y==rows.Count-1) {
				return;//can't move down
			}
			nextCellToRight=-1;
			for(int i=0;i<columns.Count;i++) {
				if(columns[i].IsEditable) {
					nextCellToRight=i;
					break;
				}
			}
			//guaranteed to have a value
			selectedCell=new Point(nextCellToRight,SelectedCellOld.Y+1);
			CreateEditBox();
		}
		
		void editBox_KeyUp(object sender,KeyEventArgs e) {
			if(editBox==null) {
				return;
			}
			if(editBox.Text=="") {
				return;
			}
			Graphics g=CreateGraphics();
			int cellH=(int)((1.03)*(float)(g.MeasureString(editBox.Text+"\r\n",_cellFont,editBox.Width).Height))+4;
			if(cellH < EDITABLE_ROW_HEIGHT) {//if it's less than one line
			  cellH=EDITABLE_ROW_HEIGHT;//set it to one line
			}
			if(cellH>editBox.Height) {//it needs to grow so redraw it. Only measures the text of this one cell so checking here for shrinking would cause unnecessary redraws and other bugs.
				rows[selectedCell.Y].Cells[selectedCell.X].Text=editBox.Text;
				Point cellSelected=new Point(selectedCell.X,selectedCell.Y);
				int selectionStart=editBox.SelectionStart;
				List<ODGridColumn> listCols=new List<ODGridColumn>();
				for(int i=0;i<columns.Count;i++) {
					listCols.Add(columns[i].Copy());
				}
				List<ODGridRow> listRows=new List<ODGridRow>();
				ODGridRow row;
				for(int i=0;i<rows.Count;i++) {
					row=new ODGridRow();
					for(int j=0;j<rows[i].Cells.Count;j++) {
						row.Cells.Add(new ODGridCell(rows[i].Cells[j].Text,rows[i].Cells[j].SelectedIndex));
					}
					row.Tag=rows[i].Tag;
					listRows.Add(row);
				}
				BeginUpdate();
				columns.Clear();
				for(int i=0;i<listCols.Count;i++) {
					columns.Add(listCols[i].Copy());
				}
				rows.Clear();
				for(int i=0;i<listRows.Count;i++) {
					row=new ODGridRow();
					for(int j=0;j<listRows[i].Cells.Count;j++) {
						row.Cells.Add(listRows[i].Cells[j].Text);
						row.Cells[j].SelectedIndex=listRows[i].Cells[j].SelectedIndex;
					}
					row.Tag=listRows[i].Tag;
					rows.Add(row);
				}
				EndUpdate();
				if(editBox!=null) {
					editBox.Dispose();
				}
				selectedCell=cellSelected;
				CreateEditBox();
				if(editBox!=null) {
					editBox.SelectionStart=selectionStart;
					editBox.SelectionLength=0;
				}
			}
			g.Dispose();
		}

		void editBox_TextChanged(object sender,EventArgs e) {
			if(editBox!=null) {
				Rows[selectedCell.Y].Cells[selectedCell.X].Text=editBox.Text;
			}
			OnCellTextChanged();
		}

		///<summary>The purpose of this is to allow dragging to select multiple rows.  Only makes sense if selectionMode==MultiExtended.  Doesn't matter whether ctrl is down, because that only affects the mouse down event.</summary>
		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);
			_mouseIsOver=true;
			if(!MouseIsDown) {
				return;
			}
			if(selectionMode!=GridSelectionMode.MultiExtended) {
				return;
			}
			if(!allowSelection) {
				return;//dragging does not change selection of rows
			}
			if(mouseIsDownInHeader) {
				return;//started drag in header, so not allowed to select anything.
			}
			int curRow=PointToRow(e.Y);
			if(curRow==-1 || curRow==MouseDownRow) {
				return;
			}
			//because mouse might have moved faster than computer could keep up, we have to loop through all rows between
			if(ControlIsDown) {
				if(selectedIndicesWhenMouseDown==null) {
					selectedIndices=new ArrayList();
				}
				else {
					selectedIndices=new ArrayList(selectedIndicesWhenMouseDown);
				}
			}
			else {
				selectedIndices=new ArrayList();
			}
			if(MouseDownRow<curRow) {//dragging down
				for(int i=MouseDownRow;i<=curRow;i++) {
					if(i==-1) {
						continue;
					}
					if(selectedIndices.Contains(i)) {
						selectedIndices.Remove(i);
					}
					else {
						selectedIndices.Add(i);
					}
				}
			}
			else {//dragging up
				for(int i=curRow;i<=MouseDownRow;i++) {
					if(selectedIndices.Contains(i)) {
						selectedIndices.Remove(i);
					}
					else {
						selectedIndices.Add(i);
					}
				}
			}
			Invalidate();
		}

		///<summary></summary>
		protected override void OnMouseEnter(EventArgs e) {
			base.OnMouseEnter(e);
			_mouseIsOver=true;
		}

		///<summary></summary>
		protected override void OnMouseLeave(EventArgs e) {
			base.OnMouseLeave(e);
			_mouseIsOver=false;
		}

		private void vScroll_MouseEnter(Object sender,EventArgs e) {
			_mouseIsOver=true;
		}

		private void vScroll_MouseLeave(Object sender,EventArgs e) {
			_mouseIsOver=false;
		}

		private void vScroll_MouseMove(Object sender,MouseEventArgs e) {
			_mouseIsOver=true;
		}

		private void hScroll_MouseEnter(Object sender,EventArgs e) {
			_mouseIsOver=true;
		}

		private void hScroll_MouseLeave(Object sender,EventArgs e) {
			_mouseIsOver=false;
		}

		private void hScroll_MouseMove(Object sender,MouseEventArgs e) {
			_mouseIsOver=true;
		}

		private void Parent_MouseWheel(Object sender,MouseEventArgs e) {
			if(_mouseIsOver) {
				//this.ac
				this.Select();//?
				//this.Focus();
			}
		}

		///<summary></summary>
		protected override void OnMouseWheel(MouseEventArgs e) {
			base.OnMouseWheel(e);
			ScrollValue-=e.Delta/3;
		}

		#endregion MouseEvents

		#region KeyEvents
		///<summary></summary>
		protected override void OnKeyDown(KeyEventArgs e) {
			base.OnKeyDown(e);
			if(e.KeyCode==Keys.ControlKey) {
				ControlIsDown=true;
			}
			if(e.KeyCode==Keys.ShiftKey) {
				ShiftIsDown=true;
			}
		}

		///<summary></summary>
		protected override void OnKeyUp(KeyEventArgs e) {
			base.OnKeyUp(e);
			if(e.KeyCode==Keys.ControlKey) {
				ControlIsDown=false;
			}
			if(e.KeyCode==Keys.ShiftKey) {
				ShiftIsDown=false;
			}
		}

		/// <summary>If the Ctrl key down is not being captured by the grid because it doesn't have focus, then this automatically handles it.  The only thing you have to do to make it work is to turn on KeyPreview for the parent form.</summary>
		private void Parent_KeyDown(Object sender,KeyEventArgs e) {
			if(e.KeyCode==Keys.ControlKey) {
				ControlIsDown=true;
			}
			if(e.KeyCode==Keys.ShiftKey) {
				ShiftIsDown=true;
			}
			if(selectionMode==GridSelectionMode.One) {
				if(e.KeyCode==Keys.Down) {
					if(selectedIndices.Count>0 && (int)selectedIndices[0] <RowsFiltered.Count-1) {
						int prevRow=(int)selectedIndices[0];
						selectedIndices.Clear();
						SelectionAddedByUser(prevRow+1);
						hScroll.Value=hScroll.Minimum;
					}
				}
				else if(e.KeyCode==Keys.Up) {
					if(selectedIndices.Count>0 && (int)selectedIndices[0] > 0) {
						int prevRow=(int)selectedIndices[0];
						selectedIndices.Clear();
						SelectionAddedByUser(prevRow-1);
					}
				}
			}
		}

		/// <summary>If the Ctrl key down is not being captured by the grid because it doesn't have focus, then this automatically handles it.  The only thing you have to do to make it work is to turn on KeyPreview for the parent form.</summary>
		private void Parent_KeyUp(Object sender,KeyEventArgs e) {
			if(e.KeyCode==Keys.ControlKey) {
				ControlIsDown=false;
			}
			if(e.KeyCode==Keys.ShiftKey) {
				ShiftIsDown=false;
			}
			//if(e.KeyCode==Keys.Down | e.KeyCode==Keys.Up){
			//	UpDownKey=false;
			//	hScroll.Value=hScroll.Minimum;
			//}
		}

		protected void OnCellTextChanged() {
			if(CellTextChanged!=null) {
				CellTextChanged(this,new EventArgs());
			}
		}

		protected void OnCellLeave(int col,int row) {
			if(CellLeave!=null) {
				CellLeave(this,new ODGridClickEventArgs(col,row,MouseButtons.None));
			}
		}

		protected void OnCellEnter(int col,int row) {
			if(CellEnter!=null) {
				CellEnter(this,new ODGridClickEventArgs(col,row,MouseButtons.None));
			}
		}
		#endregion KeyEvents

		#region Helper Methods
		///<summary>Returns the text in the cell for the given row and column. Will throw if either index is invalid.</summary>
		public string GetText(int cellRow,int cellColumn) {
			return Rows[cellRow].Cells[cellColumn].Text;
		}
		#endregion Helper Methods

		///<summary>The pdfSharp version of drawstring.  g is used for measurement.  scaleToPix scales xObjects to pixels.</summary>
		private static void DrawStringX(XGraphics xg,string str,XFont xfont,XBrush xbrush,XRect xbounds,XStringAlignment sa) {
			Graphics g=Graphics.FromImage(new Bitmap(100,100));//only used for measurements.
			int topPad=0;// 2;
			int rightPad=5;//helps get measurements better.
			double scaleToPix=1d/p(1);
			//There are two coordinate systems here: pixels (used by us) and points (used by PdfSharp).
			//MeasureString and ALL related measurement functions must use pixels.
			//DrawString is the ONLY function that uses points.
			//pixels:
			Rectangle bounds=new Rectangle((int)(scaleToPix*xbounds.Left),
				(int)(scaleToPix*xbounds.Top),
				(int)(scaleToPix*xbounds.Width),
				(int)(scaleToPix*xbounds.Height));
			FontStyle fontstyle=FontStyle.Regular;
			if(xfont.Style==XFontStyle.Bold) {
				fontstyle=FontStyle.Bold;
			}
			//pixels: (except Size is em-size)
			Font font=new Font(xfont.Name,(float)xfont.Size,fontstyle);
			xfont=new XFont(xfont.Name,xfont.Size,xfont.Style);
			//pixels:
			SizeF fit=new SizeF((float)(bounds.Width-rightPad),(float)(font.Height));
			StringFormat format=StringFormat.GenericTypographic;
			//pixels:
			float pixelsPerLine=(float)font.Height-0.5f;//LineSpacingForFont(font.Name) * (float)font.Height;
			float lineIdx=0;
			int chars;
			int lines;
			//points:
			RectangleF layoutRectangle;
			for(int ix=0;ix<str.Length;ix+=chars) {
				if(bounds.Y+topPad+pixelsPerLine*lineIdx>bounds.Bottom) {
					break;
				}
				//pixels:
				g.MeasureString(str.Substring(ix),font,fit,format,out chars,out lines);
				//PdfSharp isn't smart enough to cut off the lower half of a line.
				//if(bounds.Y+topPad+pixelsPerLine*lineIdx+font.Height > bounds.Bottom) {
				//	layoutH=bounds.Bottom-(bounds.Y+topPad+pixelsPerLine*lineIdx);
				//}
				//else {
				//	layoutH=font.Height+2;
				//}
				//use points here:
				float adjustTextDown=10f;//this value was arrived at by trial and error.
				layoutRectangle=new RectangleF(
					(float)xbounds.X,
					//(float)(xbounds.Y+(float)topPad/scaleToPix+(pixelsPerLine/scaleToPix)*lineIdx),
					(float)(xbounds.Y+adjustTextDown+(pixelsPerLine/scaleToPix)*lineIdx),
					(float)xbounds.Width+50,//any amount of extra padding here will not cause malfunction
					0);//layoutH);
				XStringFormat sf=XStringFormats.Default;
				sf.Alignment=sa;
				xg.DrawString(str.Substring(ix,chars),xfont,xbrush,(double)layoutRectangle.Left,(double)layoutRectangle.Top,sf);
				lineIdx+=1;
			}
			g.Dispose();
		}

		///<summary>This line spacing is specifically picked to match the RichTextBox.</summary>
		private static float LineSpacingForFont(string fontName) {
			if(fontName.ToLower()=="arial") {
				return 1.055f;
			}
			else if(fontName.ToLower()=="courier new") {
				return 1.055f;
			}
			//else if(fontName.ToLower()=="microsoft sans serif"){
			//	return 1.00f;
			//}
			return 1.05f;
		}
		
		///<summary>Converts pixels used by us to points used by PdfSharp.</summary>
		private static double p(int pixels){
			XUnit xunit=XUnit.FromInch((double)pixels/100d);//100 ppi
			return xunit.Point;
				//XUnit.FromInch((double)pixels/100);
		}

		///<summary>Converts pixels used by us to points used by PdfSharp.</summary>
		private static double p(float pixels){
			XUnit xunit=XUnit.FromInch((double)pixels/100d);//100 ppi
			return xunit.Point;
		}

		public delegate void DelegateClickListener();




















	}

	public class ODPrintRow {
		///<summary>YPos relative to top of entire grid.  When printing this includes adjustments for page breaks.  If row has title/header the title/header should be drawn at this position.</summary>
		public int YPos;
		///<summary>Usually only true for some grids, and only for the first row.</summary>
		public bool IsTitleRow;
		///<summary>Usually true if row is at the top of a new page, or when changing patients in a statement grid.</summary>
		public bool IsHeaderRow;
		///<summary>True for rows that require a bold bottom line, at end of entire grid, at page breaks, or at a separation in the grid.</summary>
		public bool IsBottomRow;
		///<summary>Rarely true, usually only for last row in particular grids.</summary>
		public bool IsFooterRow;

		public ODPrintRow(int yPos,bool isTitleRow,bool isHeaderRow,bool isBottomRow,bool isFooterRow) {
			YPos=yPos;
			IsTitleRow=isTitleRow;
			IsHeaderRow=isHeaderRow;
			IsBottomRow=isBottomRow;
			IsFooterRow=isFooterRow;
		}
	}


	///<summary></summary>
	public class ODGridClickEventArgs {
		private int col;
		private int row;
		private MouseButtons button;

		///<summary></summary>
		public ODGridClickEventArgs(int col,int row,MouseButtons button) {
			this.col=col;
			this.row=row;
			this.button=button;
		}

		///<summary></summary>
		public int Row {
			get {
				return row;
			}
		}

		///<summary></summary>
		public int Col {
			get {
				return col;
			}
		}

		///<summary>Gets which mouse button was pressed.</summary>
		public MouseButtons Button {
			get {
				return button;
			}
		}

  }

  ///<summary>Specifies the selection behavior of an ODGrid.</summary>   
  //[ComVisible(true)]
  public enum GridSelectionMode {
    ///<summary>0-No items can be selected.</summary>  
    None,
    ///<summary>1-Only one row can be selected.</summary>  
    One,
    ///<summary>2-Only one cell can be selected.</summary>
    OneCell,
    ///<summary>3-Multiple items can be selected, and the user can use the SHIFT, CTRL, and arrow keys to make selections</summary>   
    MultiExtended,
  }

}






/*This is a template of typical grid code which can be quickly pasted into any form.
 
		using OpenDental.UI;

		FillGrid(){
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("Table",""),);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("Table",""),);
			gridMain.Columns.Add(col);
			 
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<List.Length;i++){
				row=new ODGridRow();
				row.Cells.Add("");
				row.Cells.Add("");
			  
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

*/