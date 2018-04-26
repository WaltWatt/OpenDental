using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using CodeBase;

namespace OpenDental {
	public class ODcodeBox:RichTextBox {
		private int _lastLineWidth=0;
		///<summary>Set to -1 when the line number width needs to be recalculated.</summary>
		private int _lineNumberWidth=-1;
		///<summary></summary>
		private SizeF _lineNumberSize=new SizeF();
		///<summary>Holds the indexes of each newline char within _stringTextMain.  Set to null when needed to recalculate.</summary>
		private List<int> _listIndicesOfNewLines;
		///<summary>The string format that keeps the numbers on the left centered and aligned with the adjacent text.</summary>
		private StringFormat _stringFormatForDrawingNumbers;
		///<summary>The actual format that goes into the parenthesis of the ToString() to format the string. E.g. {0:D3}</summary>
		private string _stringFormatForNumbers;
		private Font _lineNumberFont=new Font("Courier New",9F,FontStyle.Regular);
		private Brush _brushOlive=new SolidBrush(Color.Olive);

		public ODcodeBox() {
			this.
			_stringFormatForDrawingNumbers=new StringFormat();
			//Center the text horizontally
			_stringFormatForDrawingNumbers.Alignment=StringAlignment.Center;
			//Shove the number to the top (so that long sentences that span multiple "lines" but have no newline chars only show as one line number.
			//This makes it so that a large blank space will show underneath any additional space needed between line numbers.
			//This looks really good so don't mess with this setting.
			_stringFormatForDrawingNumbers.LineAlignment=StringAlignment.Near;
			_stringFormatForDrawingNumbers.Trimming=StringTrimming.None;
		}

		private List<int> ListIndicesOfNewLines {
			get {
				if(_listIndicesOfNewLines==null) {
					_listIndicesOfNewLines=new List<int>();
					int index=0;
					foreach(char character in this.Text) {
						if(character=='\n') {
							_listIndicesOfNewLines.Add(index);
						}
						index++;
					}
					//Always add a new line for the very last line.
					_listIndicesOfNewLines.Add(this.Text.Length-1);
				}
				return _listIndicesOfNewLines;
			}
		}
		
		private int LineNumberWidth {
			get {
				if(_lineNumberWidth > 0) {
					return _lineNumberWidth;
				}
				int numberOfDigits=1;
				if(ListIndicesOfNewLines.Count>0) {
					numberOfDigits=(int)(1+Math.Log((double)ListIndicesOfNewLines.Count,10));
				}
				string widthForDigits=new string('Z',numberOfDigits);
				using(Bitmap bmp=new Bitmap(1,1))
				using(Graphics g=Graphics.FromImage(bmp)) 
				{
					_lineNumberSize=g.MeasureString(widthForDigits,_lineNumberFont);
					_lineNumberWidth=(int)Math.Ceiling(_lineNumberSize.Width+10);
					_stringFormatForNumbers="{0:D"+numberOfDigits+"}";
					return _lineNumberWidth;
				}
			}
		}

		///<summary>Recalculates the line numbers on every text change.</summary>
		protected override void OnTextChanged(EventArgs e) {
			LineNumberRecalculate();
			base.OnTextChanged(e);
		}
		
		///<summary>Nulls out variables so that the text boxes recalculate their contents on paint.</summary>
		private void LineNumberRecalculate() {
			_listIndicesOfNewLines=null;
			_lineNumberWidth=-1;
			WindowsApiWrapper.SendMessage(this.Handle,(int)WindowsApiWrapper.WinMessagesOther.WM_PAINT,0,0);
		}

		protected override void WndProc(ref Message m) {
			bool isHandled=false;
			switch(m.Msg) {
				case (int)WindowsApiWrapper.WinMessagesOther.WM_CHAR:
					LineNumberRecalculate();
					break;
				case (int)WindowsApiWrapper.WinMessagesOther.WM_PAINT:
					base.WndProc(ref m);
					PaintLineNumbers(Graphics.FromHwnd(Handle));
					isHandled=true;
					break;
			}
			if(!isHandled) {
				base.WndProc(ref m);
			}
		}

		private void PaintLineNumbers(Graphics g) {
			int lineNumberWidth=LineNumberWidth;
			//If the line number width has changed, we need to grow or shrink the left margin and repaint.
			if(lineNumberWidth!=_lastLineWidth) {
				SetLeftMargin(lineNumberWidth+Margin.Left);
				_lastLineWidth=lineNumberWidth;
				WindowsApiWrapper.SendMessage(Handle,(int)WindowsApiWrapper.WinMessagesOther.WM_PAINT,0,0);
				return;
			}
			//Figure out how many total lines can be shown on the screen.
			int visibleLineCount=(int)Math.Ceiling((double)this.Bounds.Height / _lineNumberSize.ToSize().Height);
			int minLineCount=Math.Min(visibleLineCount,ListIndicesOfNewLines.Count);
			int firstLineNumber=ListIndicesOfNewLines.Count;
			int firstCharIndex=this.GetCharIndexFromPosition(new Point(1,1));
			int idx=0;
			foreach(int index in ListIndicesOfNewLines) {
				idx++;
				if(firstCharIndex<=index) {
					firstLineNumber=idx;
					break;
				}
			}
			using(Bitmap doubleBufferer=new Bitmap(lineNumberWidth,this.Bounds.Height))
			using(Graphics gDoubleBuffer=Graphics.FromImage(doubleBufferer))
			{
				Rectangle rect=new Rectangle(0,0,lineNumberWidth,this.Bounds.Height);
				gDoubleBuffer.FillRectangle(SystemBrushes.ControlLight,rect);
				int curNumberWidth=lineNumberWidth;
				for(int i=firstLineNumber;i<=ListIndicesOfNewLines.Count;i++) {
					int curPointY=0;
					if(firstLineNumber!=1) {
						//Get the Y position of the character after the new line character of the previous line.
						curPointY=GetPosFromCharIndex(ListIndicesOfNewLines[i-2]+1).Y;//2 because we want the previous line and the list is 0 based
					}
					if(this.Bounds.Height<=curPointY) {
						break;
					}
					Rectangle rectDraw=new Rectangle(0,curPointY,curNumberWidth,_lineNumberSize.ToSize().Height);
					gDoubleBuffer.DrawString(firstLineNumber.ToString(),_lineNumberFont,_brushOlive,rectDraw,_stringFormatForDrawingNumbers);
					firstLineNumber++;
				}
				g.DrawImage(doubleBufferer,new Point(0,0));
			}
		}

		///<summary>Fills a memory address to quickly get the point of the char index within the RichTextBox from the Windows API wrapper.
		///This is MUCH faster than using RichTextBox.GetPositionFromCharIndex().</summary>
		private Point GetPosFromCharIndex(int index) {
			int rawPointSize=Marshal.SizeOf(typeof(Point));
			IntPtr wParam=Marshal.AllocHGlobal(rawPointSize);
			WindowsApiWrapper.SendMessage(this.Handle,(int)WindowsApiWrapper.EM_Rich.EM_POSFROMCHAR,(int)wParam,index);
			Point position=(Point)Marshal.PtrToStructure(wParam,typeof(Point));
			Marshal.FreeHGlobal(wParam);
			return position;
		}

		private void SetLeftMargin(int widthInPixels) {
			WindowsApiWrapper.SendMessage(this.Handle,(int)WindowsApiWrapper.EM_Rich.EM_SETMARGINS,WindowsApiWrapper.EC_LEFTMARGIN,
				widthInPixels);
		}

		public void SetScroll(int delta) {
			WindowsApiWrapper.SendMessage(this.Handle,(int)WindowsApiWrapper.EM_Rich.EM_LINESCROLL,0,delta);
		}
	}
}