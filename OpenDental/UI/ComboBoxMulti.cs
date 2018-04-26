using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CodeBase;

namespace OpenDental.UI {
	public class ComboBoxMulti : UserControl {
		private ArrayList _listItems;
		private TextBox textMain;
		private ComboMultiDelimiter _comboDelimiter;
		private ImageList _imageListButDrop;
		private IContainer components;
		private ODThread _threadCheckKeyboard=null;
		private Button butDrop;
		private static bool _isLeftMouseDown;
		private static bool _isShiftDown;
		private static bool _isCtrlDown;
		private Panel panel1;
		private ODGrid gridMain;
		private PopupWindow _popup=null;
		//Eventually we may want to set this.
		private const int _maxDropDownHeight=500;	

		public ComboBoxMulti() {
			//This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			_listItems=new ArrayList();
			ComboDelimiter=ComboMultiDelimiter.Comma;//Required because we specified a default value of true for the designer.
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComboBoxMulti));
			this.textMain = new System.Windows.Forms.TextBox();
			this._imageListButDrop = new System.Windows.Forms.ImageList(this.components);
			this.butDrop = new OpenDental.UI.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// textMain
			// 
			this.textMain.BackColor = System.Drawing.Color.White;
			this.textMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textMain.Location = new System.Drawing.Point(2, 4);
			this.textMain.Name = "textMain";
			this.textMain.ReadOnly = true;
			this.textMain.Size = new System.Drawing.Size(97, 13);
			this.textMain.TabIndex = 2;
			// 
			// imageList1
			// 
			this._imageListButDrop.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this._imageListButDrop.TransparentColor = System.Drawing.Color.Transparent;
			this._imageListButDrop.Images.SetKeyName(0, "arrowDownTriangle.gif");
			this._imageListButDrop.Images.SetKeyName(1, "arrowUpTriangle.gif");
			// 
			// butDrop
			// 
			this.butDrop.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDrop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butDrop.Autosize = true;
			this.butDrop.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDrop.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDrop.CornerRadius = 4F;
			this.butDrop.ImageIndex = 0;
			this.butDrop.ImageList = this._imageListButDrop;
			this.butDrop.Location = new System.Drawing.Point(102, 1);
			this.butDrop.Name = "butDrop";
			this.butDrop.Size = new System.Drawing.Size(17, 19);
			this.butDrop.TabIndex = 4;
			this.butDrop.UseVisualStyleBackColor = true;
			this.butDrop.Click += new System.EventHandler(this.dropButton_Click);
			// 
			// panel1
			// 
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(120, 21);
			this.panel1.TabIndex = 5;
			this.panel1.BorderStyle = BorderStyle.FixedSingle;
			// 
			// gridMain
			// 
			this.gridMain.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.gridMain.AutoSize = false;
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(0, 18);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectedRowColor = System.Drawing.SystemColors.MenuHighlight;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(120, 70);
			this.gridMain.TabIndex = 3;
			this.gridMain.Title = null;
			this.gridMain.TranslationName = "Misc";
			this.gridMain.HeaderHeight=0;
			this.gridMain.TitleHeight=0;
			this.gridMain.Visible = false;
			this.gridMain.Margin=new Padding(0);
			this.gridMain.Padding=new Padding(0);
			// 
			// ComboBoxMulti
			// 
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Controls.Add(this.butDrop);
			this.Controls.Add(this.textMain);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.gridMain);
			this.Name = "ComboBoxMulti";
			this.Size = new System.Drawing.Size(120, 21);
			this.Load += new System.EventHandler(this.ComboBoxMulti_Load);
			this.Layout += new System.Windows.Forms.LayoutEventHandler(this.ComboBoxMulti_Layout);
			this.Leave += new System.EventHandler(this.ComboBoxMulti_Leave);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion Component Designer generated code

		#region Properties

		/// <summary>The items to display in the combo box.</summary>
		[Category("Data"),Description("The text of the items to display in the dropdown section.")]
		public virtual ArrayList Items{
			get{
				return _listItems;
			}
			set{
				_listItems=value;
			}
		}

		///<summary>Gets a value indicating whether the combo box is displaying its drop-down portion.</summary>
		private bool DroppedDown{
			get{
				if(_popup==null) {
					return false;
				}
				return _popup.IsDroppedDown;
			}
		}

		public List<object> ListSelectedItems {
			get {
				return ListSelectedIndices.Select(x => Items[x]).ToList();
			}
		}

		///<summary>The indices of selected items.  Mimics the ListSelectedIndices property.</summary>
		public virtual ArrayList SelectedIndices{
			get{
				return new ArrayList(ArraySelectedIndices);
			}
			set{
				ArraySelectedIndices=value.Cast<int>().ToArray();
			}
		}

		///<summary>The indices of selected items.  Mimics the SelectedIndices property.</summary>
		public List<int> ListSelectedIndices {
			get{
				return ArraySelectedIndices.ToList();
			}
		}

		public virtual int[] ArraySelectedIndices {
			get {
				if(gridMain.Rows.Count==Items.Count) {
					for(int i=0;i<gridMain.Rows.Count;i++) {
						if(gridMain.Rows[i].Tag!=_listItems[i]) {
							FillGrid();
							break;
						}
					}
				}
				else {
					FillGrid();
				}
				return gridMain.SelectedIndices;
			}
			set {
				gridMain.SetSelectedStrict(value);
			}
		}

		///<summary>Use commas instead of OR in the display when muliple selected.</summary>
		[Category("Appearance"),Description("Use commas instead of OR in the display when muliple selected.")]
		[DefaultValue(ComboMultiDelimiter.Comma)]
		public ComboMultiDelimiter ComboDelimiter{
			get{
				return _comboDelimiter;
			}
			set{
				_comboDelimiter=value;
			}
		}

		#endregion Properties

		public void SetSelected(bool setToValue) {
			List<int> listIndexs=new List<int>();
			int index=0;
			foreach(object item in Items) {
				listIndexs.Add(index);
				index++;
			}
			SetSelectedHelper(setToValue,listIndexs.ToArray());
		}

		///<summary>This function compensates for how this control used to be populated and used.
		///We are doing this in order to minimize changes in forms that use this control.
		///Eventually we can enhance the forms to use this control better.</summary>
		public void SetSelected(int index,bool setToValue){
			SetSelectedHelper(setToValue,index);
		}

		private void SetSelectedHelper(bool setToValue,params int[] listIndexs) {
			foreach(int row in listIndexs) {
				if(setToValue && !ArraySelectedIndices.Contains(row)) {
					gridMain.SetSelected(row,true);
					continue;
				}
				else if(!setToValue && ArraySelectedIndices.Contains(row)) {
					gridMain.SetSelected(row,false);
					continue;
				}
			}
			FillText();
		}

		public void SelectedIndicesClear() {
			gridMain.SetSelected(false);
		}
		
		private void ComboBoxMulti_Load(object sender, System.EventArgs e) {
			FillText();
			if(ParentForm!=null) {
				ParentForm.FormClosing+=ComboBoxMulti_ParentFormClosing;
				//If the user drags the parent window to a new location, collapse the combobox if dropped down.
				//Dragging the parent window via the title bar does not call ComboBoxMulti_Leave without this extra line of code.
				ParentForm.Move+=ComboBoxMulti_Leave;
			}
		}

		private void ComboBoxMulti_Layout(object sender, System.Windows.Forms.LayoutEventArgs e) {
			textMain.Width=Width-21;
			panel1.Width=Width;
			gridMain.Width=Width;
		}

		private void ComboBoxMulti_Leave(object sender, System.EventArgs e) {
			if(DroppedDown) {
				DropDownToggle();
			}
		}

		private void dropButton_Click(object sender,EventArgs e) {
			DropDownToggle();
		}

		private void DropDownToggle() {
			if(DroppedDown){//DroppedDown, so we need to collapse.
				butDrop.ImageIndex=0;//Set to arrow down image.
				_popup.Close();//This collapses the popup.  The popup is NOT disposed.
				FillText();
				if(SelectionChangeCommitted!=null) {
					SelectionChangeCommitted(this,new EventArgs());
				}
			}
			else{//Not DroppedDown, so we need to DropDown.
				butDrop.ImageIndex=1;//Set to arrow up image.
				#region Popup setup
				_popup=new PopupWindow(gridMain);
				_popup.AutoClose=false;//This prevents the Alt key from collapsing the combobox and prevents other events from collapsing as well.
				_popup.Closed+=PopupWindow_Closed;
				_popup.Opened+=PopupWindow_Opened;
				Control parent=Parent;
				int x=this.Location.X;
				int y=this.Location.Y;
				while(parent!=null && !typeof(Form).IsAssignableFrom(parent.GetType())) {
					x+=parent.Location.X;
					y+=parent.Location.Y;
					parent=parent.Parent;
				}
				#endregion Popup setup
				_popup.Show(ParentForm,new Point(x,y+this.Height));
			}
			//The keyboard listener lifetime begins on first dropdown and ends when the control is disposed or parent form is closed.
			if(_threadCheckKeyboard==null) {
				//When DroppedDown, will update input flags for auto collapsing.
				_threadCheckKeyboard=new ODThread(100,WorkerThread_KeyboardListener);
				_threadCheckKeyboard.Start();
			}
		}

		#region Popup Events

		public void PopupWindow_Closed(object sender,ToolStripDropDownClosedEventArgs e) {
			butDrop.ImageIndex=0;//Set to arrow down image.
			_popup.IsDroppedDown=false;
		}

		public void PopupWindow_Opened(object sender,EventArgs e) {
			butDrop.ImageIndex=1;//Set to arrow up image.
			_popup.IsDroppedDown=true;
		}

		#endregion Popup Events

		#region Input Listening Thread

		private void WorkerThread_KeyboardListener(ODThread odThread) {
			if(!DroppedDown) {
				return;
			}
			try {
				Invoke(new DelegateKeyboardListener(UpdateInputStates));
			}
			catch(ObjectDisposedException ex) {
				if(this.IsDisposed) {
					//On rare occasion, the control is disposed near the same instant that it is Ivoked above.
					//In this situation, we are quiting the thread, thus we expect the control to be disposed and ignore the error.
					return;
				}
				throw ex;
			}
		}

		private void UpdateInputStates() {
			if(!_isLeftMouseDown //No pertinent inputs are being held down.
				&& !_isShiftDown
				&& !_isCtrlDown
				&& MouseOverGrid() //Not over gridMain.vScroll
				&& Control.MouseButtons.HasFlag(MouseButtons.Left))
			{
				_isLeftMouseDown=true;
			}
			else if(_isLeftMouseDown && !Control.MouseButtons.HasFlag(MouseButtons.Left)) {//User released left mouse key.
				_isLeftMouseDown=false;
				if(DroppedDown) {
					DropDownToggle();
				}
			}
			else if(!_isCtrlDown && Control.ModifierKeys.HasFlag(Keys.Control)) {//User just started holding the Ctrl key.
				_isCtrlDown=true;
				//Set flag so that gridMain maintains same behavior.
				//Typically you would set KeyPreview to true in a Form but we do not have that ability with UserControl.
				//See summary above ODGrid.Parent_KeyDown(...);
				gridMain.ControlIsDown=true;
			}
			else if(_isCtrlDown && !Control.ModifierKeys.HasFlag(Keys.Control)) {//User released Ctrl key.
				_isCtrlDown=false;
				gridMain.ControlIsDown=false;
				DropDownToggle();
			}
			else if(!_isShiftDown && Control.ModifierKeys.HasFlag(Keys.Shift)) {//User just started holding the Shift key.
				_isShiftDown=true;
			}
			else if(_isShiftDown && !Control.ModifierKeys.HasFlag(Keys.Shift) //User released the Shift key,
				&& !Control.ModifierKeys.HasFlag(Keys.Control))//and not holding Ctrl.
			{//User released the Shift key, and not holding Ctrl.
				_isShiftDown=false;
				DropDownToggle();
			}
		}

		///<summary>Returns true if mouse is over gridMain but no if it is over the vScrollBar.</summary>
		private bool MouseOverGrid() {
			//Possition of the mouse relative to gridMain.
			Point mouseGridCoordinates=gridMain.PointToClient(Cursor.Position);
			if(mouseGridCoordinates.X>gridMain.Width-SystemInformation.VerticalScrollBarWidth) {//Cursor is over gridMain.vScroll.
				return false;
			}
			return true;
		}

		#endregion Input Listening Thread
		
		private void FillGrid() {
			List <object> listSelectedTags=new List<object>();//This list is used to maintain previous selections.
			foreach(int index in gridMain.SelectedIndices) {
				listSelectedTags.Add(gridMain.Rows[index].Tag);
			}
			gridMain.BeginUpdate();
			gridMain.Rows.Clear();//Clears the selections also.
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn();
			gridMain.Columns.Add(col);
			ODGridRow row;
			for(int i=0;i<_listItems.Count;i++) {
				row=new ODGridRow();
				row.Tag=_listItems[i];
				row.Cells.Add(_listItems[i].ToString());
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			for(int i=0;i<gridMain.Rows.Count;i++) {
				if(listSelectedTags.Contains(gridMain.Rows[i].Tag)) {
					gridMain.SetSelected(i,true);
				}
			}
			int heightTotal = 0;
			for(int i = 0;i<gridMain.Rows.Count;i++) {
				heightTotal+=gridMain.Rows[i].RowHeight;
			}
			gridMain.Height=heightTotal+3;//1 (for upper grid border) +1 (for lower grid border) +1 (for toolbar magic space at top) = 3			
			if(gridMain.Height>_maxDropDownHeight) {
				gridMain.Height=_maxDropDownHeight;
			}
		}

		private void FillText() {
			textMain.Text="";
			string delimiter="";
			switch(_comboDelimiter) {
				case ComboMultiDelimiter.Comma:
					delimiter=", ";
				break;
				case ComboMultiDelimiter.OR:
					delimiter=" OR ";
				break;
				case ComboMultiDelimiter.None:
					if(ArraySelectedIndices.Length>1) {
						textMain.Text=Lan.g(this,"Multiple Selected");
						return;
					}//Otherwise prints either a blank or a single selection.
				break;
			}
			for(int i=0;i<ArraySelectedIndices.Length;i++) {
				if(i>0) {
					textMain.Text+=delimiter;
				}
				textMain.Text+=_listItems[ArraySelectedIndices[i]];
			}
		}

		#region Thread Quiting

		private void QuitThread() {
			if(_threadCheckKeyboard!=null) {
				_threadCheckKeyboard.QuitAsync();
				_threadCheckKeyboard=null;
			}
		}

		private void ComboBoxMulti_ParentFormClosing(object sender,EventArgs e) {
			QuitThread();
		}

		protected override void Dispose(bool disposing) {
			QuitThread();
			base.Dispose(disposing);
		}

		#endregion Thread Quiting

		#region Delegates

		public delegate void DelegateKeyboardListener();

		public delegate void SelectionChangeCommittedHandler(object sender,EventArgs e);

		///<summary>Occurs when one of the menu items is selected.  This line causes the event to show in the designer.</summary>
		public event SelectionChangeCommittedHandler SelectionChangeCommitted;

		#endregion Delegates

	}

	public enum ComboMultiDelimiter {
		///<summary>When used combo box will simply show 'Multiple Selected'.</summary>
		None,
		OR,
		Comma,
	}

	///<summary>We need this special class so that we have access to ToolStripDropDown.content.</summary>
	internal class PopupWindow:ToolStripDropDown {
		private Control _content=null;
		private ToolStripControlHost _host=null;
		public bool IsDroppedDown=false;

		public PopupWindow(Control content) {
			this.AutoSize=false;
			this.DoubleBuffered=true;
			this.ResizeRedraw=false;
			this._content=content;
			this._host=new System.Windows.Forms.ToolStripControlHost(content);
			this._host.AutoSize=false;
			this._host.Anchor=AnchorStyles.None;
			this.Size=new Size(content.Width,content.Height+1);
			this.MinimumSize=this.Size;
			this.MaximumSize=this.Size;
			this._content.Location=Point.Empty;
			this.Padding=new Padding(0);
			this.Margin=new Padding(0);
			this.Items.Add(this._host);
		}

	}
}
















