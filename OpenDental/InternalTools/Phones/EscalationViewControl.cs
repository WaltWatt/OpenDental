using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenDental {
	public partial class EscalationViewControl:UserControl, INotifyPropertyChanged {
		
		public event PropertyChangedEventHandler PropertyChanged;
		///<summary>Key: Employee name, Value: True if the employee is at their desk.</summary>
		public Dictionary<string,bool> DictProximity=new Dictionary<string, bool>();
		///<summary>Key: Employee name, Value: True if the row should show the employee extension instead of the proximity figure.</summary>
		public Dictionary<string,bool> DictShowExtension=new Dictionary<string,bool>();
		///<summary>Key: Employee name, Value: The extension.</summary>
		public Dictionary<string,int> DictExtensions=new Dictionary<string,int>();
		///<summary>Key: Employee name, Value: The color that the row should be highlighted in.</summary>
		public Dictionary<string,Color> DictColors=new Dictionary<string,Color>();

		private bool _isUpdating=false;
		///<summary>When the mouse wheel scrolls we adjust the starting index for the strings we draw from _items.</summary>
		private int indexItemStart=0;
		///<summary>Flag used to indicate when we are passing in new items to draw, when true we do not reset the scroll value.</summary>
		public bool IsNewItems;
		///<summary>The size it takes to measure the letter 'a' using the controls current Font.</summary>
		private Size _sizeFont;
		///<summary>The size it takes to measure the extension '9999' using the controls current Font.</summary>
		private SizeF _sizeExtension;

		private int _rowTextHeight {
			get {
				if(_sizeFont.Width==0 && _sizeFont.Height==0) {
					_sizeFont=TextRenderer.MeasureText("a",Font);
				}
				return _sizeFont.Height;
			}
		}

		private float _rowTotalHeight {
			get {
				return _rowTextHeight+(LinePadding*2);
			}
		}

		private BindingList<String> _items=new BindingList<string>();
		[Category("Appearance")]
		[Description("Strings to be printed")]
		public BindingList<String> Items {
			get {
				return _items;
			}
			set {
				_items=value;
				PropertyChanged(this,new PropertyChangedEventArgs("Items"));
			}
		}

		private int _borderThickness=6;
		[Category("Appearance")]
		[Description("Thickness of the border drawn around the control")]
		public int BorderThickness {
			get {
				return _borderThickness;
			}
			set {
				_borderThickness=value;
				PropertyChanged(this,new PropertyChangedEventArgs("BorderThickness"));
			}
		}

		private Color _outerColor=Color.Black;
		[Category("Appearance")]
		[Description("Exterior Border Color")]
		public Color OuterColor {
			get {
				return _outerColor;
			}
			set {
				_outerColor=value;
				PropertyChanged(this,new PropertyChangedEventArgs("OuterColor"));
			}
		}

		private int _linePadding=-6;
		[Category("Appearance")]
		[Description("Padding of each line. Suggest -6 for 0 padding between lines. Must be an even number.")]
		public int LinePadding {
			get {
				return _linePadding;
			}
			set {
				_linePadding=value;
				PropertyChanged(this,new PropertyChangedEventArgs("LinePadding"));
			}
		}

		private int _startFadeIndex=4;
		[Category("Appearance")]
		[Description("Lines will start to fade at this 0-based index.")]
		public int StartFadeIndex {
			get {
				return _startFadeIndex;
			}
			set {
				_startFadeIndex=value;
				PropertyChanged(this,new PropertyChangedEventArgs("StartFadeIndex"));
			}
		}

		private int _fadeAlphaIncrement=40;
		[Category("Appearance")]
		[Description("Alpha increment to be subtracted from each faded line. Will be subtracted from each line after StartFadeIndex and eventually bottom out at 0 (fully transparent).")]
		public int FadeAlphaIncrement {
			get {
				return _fadeAlphaIncrement;
			}
			set {
				_fadeAlphaIncrement=value;
				PropertyChanged(this,new PropertyChangedEventArgs("FadeAlphaIncrement"));
			}
		}

		private int _minAlpha=60;
		[Category("Appearance")]
		[Description("Minimum alpha transparency value. Set to 0 if full transparency is desired. Otherwise a number between 0-255. 0 is full transparent, 255 is full opaque.")]
		public int MinAlpha {
			get {
				return _minAlpha;
			}
			set {
				_minAlpha=value;
				PropertyChanged(this,new PropertyChangedEventArgs("MinAlpha"));
			}
		}
	
		public EscalationViewControl() {
			InitializeComponent();
			PropertyChanged+=EscalationViewControl_PropertyChanged;

		}

		private void EscalationViewControl_PropertyChanged(object sender,PropertyChangedEventArgs e) {
			Invalidate();
		}

		private void EscalationViewControl_Paint(object sender,PaintEventArgs e) {
			if(_isUpdating) {
				return;
			}
			Pen penOuter=new Pen(OuterColor,BorderThickness);
			try {
				RectangleF rcOuter=this.ClientRectangle;
				if(_sizeExtension.Width==0 && _sizeExtension.Height==0) {
					_sizeExtension=e.Graphics.MeasureString("9999",Font);
				}
				//clear control canvas
				e.Graphics.Clear(this.BackColor);
				float halfPenThickness=BorderThickness/(float)2;
				//deflate for border
				rcOuter.Inflate(-halfPenThickness,-halfPenThickness);
				//draw border
				e.Graphics.DrawRectangle(penOuter,rcOuter.X,rcOuter.Y,rcOuter.Width,rcOuter.Height);
				//deflate to drawable region
				rcOuter.Inflate(-halfPenThickness,-halfPenThickness);
				int alpha=255;
				int countDrawn=0;//Count of items drawn to the control, used to calculate y position for drawing.
				for(int i=0;i<_items.Count;i++) {
					string item=_items[i];
					if(i>StartFadeIndex) { //Only start fading after the user defined fade index.
						//Move toward transparency.
						alpha=Math.Max(MinAlpha,alpha-FadeAlphaIncrement);
					}
					if(_items.IndexOf(item)<indexItemStart) {
						continue;//We have scrolled past the current item, do not draw.
					}
					//Set the bounds of the drawing rectangle.
					float y=rcOuter.Y+(countDrawn*_rowTextHeight)+(countDrawn*(2*LinePadding));
					RectangleF rcItem=new RectangleF(rcOuter.X,y,rcOuter.Width,_rowTotalHeight);
					//We always want to draw the color for employees that have a color set.
					if(DictColors.ContainsKey(item) && DictColors[item]!=this.BackColor) {
						using(Brush brushText=new SolidBrush(Color.FromArgb(alpha,DictColors[item]))) {
							e.Graphics.FillRectangle(brushText,rcItem);
						}
					}
					StringFormat sf=new StringFormat(StringFormatFlags.NoWrap);
					sf.LineAlignment=StringAlignment.Center;
					using(Brush brushText=new SolidBrush(Color.FromArgb(alpha,ForeColor))) {
						e.Graphics.DrawString(item,Font,brushText,rcItem,sf);
					}
					countDrawn++;
					//We always want to draw the extension for employees within escalation views.
					if(DictShowExtension.ContainsKey(item) && DictShowExtension[item]==true && DictExtensions.ContainsKey(item)) {
						//Display the extension in the right hand side of the view.
						string ext=DictExtensions[item].ToString();
						float x=rcOuter.X+rcOuter.Width-_sizeExtension.Width;
						rcItem=new RectangleF(x,y,_sizeExtension.Width,_rowTotalHeight);
						using(Brush brushText=new SolidBrush(Color.FromArgb(alpha,ForeColor))) {
							e.Graphics.DrawString(ext,Font,brushText,rcItem,sf);
						}
					}
					//The little proximity guy should only show for employees that are proximal AND at the same location that the currently selected room is at.
					if(DictProximity.ContainsKey(item) && DictProximity[item]==true) {
						//If we're not displaying the extension, we'll show the little proximity figure.
						Image ProxImage=Properties.Resources.Figure;
						using(Bitmap bitmap=new Bitmap(ProxImage)) {
							float proxImgX=rcItem.X+rcItem.Width-_sizeExtension.Width-ProxImage.Width-10;
							float proxImgY=y+(ProxImage.Height/2);
							RectangleF rectImage=new RectangleF(proxImgX,proxImgY,ProxImage.Width,ProxImage.Height);
							e.Graphics.DrawImage(
								ProxImage,
								rectImage,
								new RectangleF(0,0,bitmap.Width,bitmap.Height),
								GraphicsUnit.Pixel);
						}
					}
				}
			}
			catch {
			}
			finally {
				penOuter.Dispose();
			}
		}

		public void BeginUpdate() {
			_isUpdating=true;
		}

		public void EndUpdate() {
			_isUpdating=false;
			if(IsNewItems) {
				indexItemStart=0;//Resets scroll value after updates.
				IsNewItems=false;
			}
			Invalidate();
		}

		private void EscalationViewControl_MouseWheel(object sender,MouseEventArgs e) {
			if(e.Delta>0) {//MouseWheel up
				indexItemStart=Math.Max(indexItemStart-1,0);//Ensures we do not scroll above first item.
			}
			else {//MouseWheel down
				indexItemStart=Math.Min(indexItemStart+1,_items.Count-1);//-1 so that there will always be 1 item drawn.
				if(_items.Count-indexItemStart<9) {//Difference represents the number of items we are goin to draw.
					indexItemStart=_items.Count-9;//Always keep the escalation view full, last item will be at bottom of the view.
				}
			}
			PropertyChanged(this,new PropertyChangedEventArgs("MouseWheel"));
		}
	}
}
