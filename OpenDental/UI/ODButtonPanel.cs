using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace OpenDental.UI {
	///<summary></summary>
	public delegate void ODButtonPanelEventHandler(object sender,ODButtonPanelEventArgs e);

	///<summary>Allows for a button panel that is customizable and generated from data in the DB.</summary>
	public partial class ODButtonPanel:UserControl {
		private int _rowHeight=18;
		private float _fontSize=8.5f;
		private bool _useBlueTheme;
		private Point _pointMouseClick;
		public List<ODPanelItem> Items;
		public bool IsUpdating;
		///<summary>The last item that was clicked on. Can be null.</summary>
		public ODPanelItem SelectedItem;
		///<summary>The last row that was clicked on. -1 if no row selected.</summary>
		public int SelectedRow; 

		#region EventHandlers
		///<summary></summary>
		[Category("Action"),Description("Occurs when an item is single clicked.")]
		public event ODButtonPanelEventHandler ItemClick=null;
		///<summary></summary>
		[Category("Action"),Description("Occurs when a button item is single clicked.")]
		public event ODButtonPanelEventHandler ItemClickBut=null;
		///<summary></summary>
		[Category("Action"),Description("Occurs when a label item is single clicked.")]
		public event ODButtonPanelEventHandler ItemClickLabel=null;
		///<summary></summary>
		[Category("Action"),Description("Occurs when a row is single clicked.")]
		public event ODButtonPanelEventHandler RowClick=null;
		///<summary></summary>
		[Category("Action"),Description("Occurs when an item is double clicked.")]
		public event ODButtonPanelEventHandler ItemDoubleClick=null;
		///<summary></summary>
		[Category("Action"),Description("Occurs when a button item is double clicked.")]
		public event ODButtonPanelEventHandler ItemDoubleClickBut=null;
		///<summary></summary>
		[Category("Action"),Description("Occurs when a label item is double clicked.")]
		public event ODButtonPanelEventHandler ItemDoubleClickLabel=null;
		///<summary></summary>
		[Category("Action"),Description("Occurs when a row is double clicked.")]
		public event ODButtonPanelEventHandler RowDoubleClick=null;
		#endregion

		#region Properties

		[Category("Appearance"),Description("For future use."),
		NotifyParentProperty(true), RefreshProperties(RefreshProperties.Repaint)]
		public bool UseBlueTheme {
			get {
				return _useBlueTheme;
			}
			set {
				_useBlueTheme=value;
			}
		}
		#endregion

		public ODButtonPanel() {
			InitializeComponent();
			Items=new List<ODPanelItem>();
			SelectedRow=-1;
			//might need parent form to doubel buffer.
			DoubleBuffered=true;
		}

		///<summary></summary>
		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);
		}

		///<summary></summary>
		protected override void OnResize(EventArgs e) {
			base.OnResize(e);
			Invalidate();
		}

		#region Computations
		///<summary>Computes the position of each column and the overall width.  Called from endUpdate and also from OnPaint.</summary>
		private void ComputeWidthsAndLocations(Graphics g) {
			Items.Sort(ODPanelItem.SortYX);
			for(int i=0;i<Items.Count;i++) {
				Items[i].CalculateWidth(g,_fontSize);
				Items[i].Location.Y=Items[i].YPos*_rowHeight;
				//Then set xPos using previous cell's position and width:
				if(i>0 && Items[i].YPos==Items[i-1].YPos) {//Previous item was on the same row. First item on each row is 0
					Items[i].Location.X=Items[i-1].Location.X+Items[i-1].ItemWidth;
				}
			}
		}

		///<summary>Returns the panel item clicked on. Returns null if no item found.</summary>
		public ODPanelItem PointToItem(Point loc) {
			ODPanelItem retVal=null;
			retVal=Items.Find(item =>
				item.YPos==(loc.Y/_rowHeight) //item is on the clicked row
				&& item.Location.X<loc.X //point is to to the right of the left side
				&& item.Location.X+item.ItemWidth>loc.X); //point is the the left of the right side
			return retVal;
		}
		#endregion Painting

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
			ComputeWidthsAndLocations(e.Graphics);
			DrawBackG(e.Graphics);
			DrawItems(e.Graphics);
			DrawOutline(e.Graphics);
		}

		///<summary>Draws a solid gray background.</summary>
		private void DrawBackG(Graphics g) {
			Color cBackG=Color.FromArgb(224,223,227);
			if(_useBlueTheme) {
				cBackG=Color.FromArgb(202,212,222);//174,196,217);//151,180,196);
			}
			g.FillRectangle(new SolidBrush(cBackG),
				0,0,
				Width,Height);//Creates a shadow on top and left of control.
			g.FillRectangle(new SolidBrush(Color.White),
				1,1,
				Width-1,Height-1);
		}

		private void DrawItems(Graphics g) {
			for(int i=0;i<Items.Count;i++) {
				switch(Items[i].ItemType) {
					case ODPanelItemType.Button:
						DrawItemBut(g,Items[i]);
						break;
					case ODPanelItemType.Label:
						DrawItemLabel(g,Items[i]);
						break;
				}
			}
		}

		private void DrawItemLabel(Graphics g,ODPanelItem item) {
			Color cBack=Color.White;//FromArgb(224,223,227);
			Color cText=Color.Black;
			if(_useBlueTheme) {
				//TODO:
			}
			RectangleF itemRect=new RectangleF(
				item.Location.X,item.Location.Y,
				item.ItemWidth,_rowHeight);
			g.FillRectangle(new SolidBrush(cBack),itemRect);
			g.DrawString(item.Text,Font,new SolidBrush(cText),itemRect,new StringFormat { Alignment=StringAlignment.Center,LineAlignment=StringAlignment.Center });//TODO, probably tweek this to draw text int he right spot.
		}

		private void DrawItemBut(Graphics g,ODPanelItem item) {
			//Copied and pasted from OpenDental.UI.Button
			this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
				ControlStyles.DoubleBuffer,true);
			Color colorBorder      =Color.FromArgb(28,81,128);//150,190,210);
			Color colorDisabledFore=Color.FromArgb(161,161,146);
			Color colorShadow      =Color.FromArgb(180,180,180);
			Color colorDarkest     =Color.FromArgb(157,164,196);
			Color colorLightest    =Color.FromArgb(255,255,255);
			Color colorMain        =Color.FromArgb(223,224,235);//218,220,235);//200,202,220);
			Color colorDarkDefault =Color.FromArgb(50,70,230);
			Color colorHoverDark   =Color.FromArgb(255,190,100);//(255,165,0) is pure orange
			Color colorHoverLight  =Color.FromArgb(255,210,130);//(255,223,163) is a fairly light orange
			//this.OnPaintBackground(p);
			//Graphics g=p.Graphics;
			Rectangle recOutline=new Rectangle(item.Location.X,item.Location.Y,item.ItemWidth-1,_rowHeight-1);
			float radius=4;
			DrawItemButBackground(g,recOutline,radius,colorDarkest/*colorDarkDefault*/,colorMain,colorLightest);
			float diagonalLength=(float)Math.Sqrt(recOutline.Height*recOutline.Height+recOutline.Width*recOutline.Width);
			////unit vector representing direction of diagonal
			//float unitvectorx=recOutline.Width/diagonalLength;
			//float unitvectory=-recOutline.Height/diagonalLength;
			////unit vector rotated 90 degrees:
			//float unitvector90x=-unitvectory;
			//float unitvector90y=unitvectorx;
			//float length90=unitvectorx*recOutline.Height/(unitvectorx*unitvector90y-unitvector90x*unitvectory);
			DrawRoundedRectangle(g,new Pen(colorBorder),recOutline,radius);
			DrawItemButTextAndImage(g,item);
			DrawItemButReflection(g,recOutline,radius);
		}

			#region Button Drawing Helper Functions
			///<summary>Draws a rectangle with rounded edges.</summary>
			public static void DrawRoundedRectangle(Graphics grfx,Pen pen,RectangleF rect,float round) {
				SmoothingMode oldSmoothingMode = grfx.SmoothingMode;
				grfx.SmoothingMode = SmoothingMode.AntiAlias;
				//top
				grfx.DrawLine(pen,rect.Left+round,rect.Top,rect.Right-round,rect.Top);
				grfx.DrawArc(pen,rect.Right-round*2,rect.Top,round*2,round*2,270,90);
				//
				grfx.DrawLine(pen,rect.Right,rect.Top+round,rect.Right,rect.Bottom-round);
				grfx.DrawArc(pen,rect.Right-round*2,rect.Bottom-round*2,round*2,round*2,0,90);
				//
				grfx.DrawLine(pen,rect.Right-round,rect.Bottom,rect.Left+round,rect.Bottom);
				grfx.DrawArc(pen,rect.Left,rect.Bottom-round*2,round*2,round*2,90,90);
				//
				grfx.DrawLine(pen,rect.Left,rect.Bottom-round,rect.Left,rect.Top+round);
				grfx.DrawArc(pen,rect.Left,rect.Top,round*2,round*2,180,90);
				//
				grfx.SmoothingMode = oldSmoothingMode;
			}

			private void DrawItemButReflection(Graphics g,RectangleF rect,float radius) {
				//lower--------------------------------------------------------------------
				Color clrDarkOverlay=Color.FromArgb(50,125,125,125);
				LinearGradientBrush brush=new LinearGradientBrush(new PointF(rect.Left,rect.Bottom),
					new PointF(rect.Left,rect.Top+rect.Height/2-radius*2f),Color.FromArgb(0,0,0,0),
					Color.FromArgb(50,0,0,0));
				GraphicsPath path=new GraphicsPath();
				path.AddLine(rect.Left+radius,rect.Top+rect.Height/2f,rect.Right-radius*2f,rect.Top+rect.Height/2f);
				path.AddArc(new RectangleF(rect.Right-(radius*4f),rect.Top+rect.Height/2f-radius*4f,radius*4f,radius*4f),90,-90);
				path.AddLine(rect.Right,rect.Top+rect.Height/2f-radius,rect.Right,rect.Bottom);
				path.AddLine(rect.Right,rect.Bottom,rect.Left,rect.Bottom);
				path.AddLine(rect.Left,rect.Bottom,rect.Left,rect.Top+rect.Height/2f-radius/2f);
				path.AddArc(new RectangleF(rect.Left,rect.Top+rect.Height/2f-radius,radius*2f,radius),180,-90);
				//g.DrawPath(Pens.Red,path);
				g.FillPath(brush,path);
			}

			private void DrawItemButBackground(Graphics g,RectangleF rect,float radius,Color clrDark,Color clrMain,Color clrLight) {
				if(radius<0) {
					radius=0;
				}
				LinearGradientBrush brush;
				SolidBrush brushS=new SolidBrush(clrMain);
				g.SmoothingMode = SmoothingMode.HighQuality;
				//sin(45)=.85. But experimentally, .7 works much better.
				//1/.85=1.18 But experimentally, 1.37 works better. What gives?
				//top
				brush=new LinearGradientBrush(new PointF(rect.Left+radius,rect.Top+radius),
					new PointF(rect.Left+radius,rect.Top),
					clrMain,clrLight);
				g.FillRectangle(brushS,rect.Left+radius,rect.Top,rect.Width-(radius*2),radius);
				//UR
				//2 pies of 45 each.
				brush=new LinearGradientBrush(new PointF(rect.Right-radius,rect.Top),
					new PointF(rect.Right-(radius/2f),rect.Top+(radius/2f)),
					clrLight,clrMain);
				g.FillPie(brushS,rect.Right-(radius*2),rect.Top,radius*2,radius*2,270,45);
				brush=new LinearGradientBrush(new PointF(rect.Right-(radius/2f)-.5f,rect.Top+(radius/2f)-.5f),
					new PointF(rect.Right,rect.Top+radius),
					clrMain,clrDark);
				g.FillPie(brush,rect.Right-(radius*2),rect.Top,radius*2,radius*2,315,45);
				//right
				brush=new LinearGradientBrush(new PointF(rect.Right-radius,rect.Top+radius),
					new PointF(rect.Right,rect.Top+radius),
					clrMain,clrDark);
				g.FillRectangle(brush,rect.Right-radius,rect.Top+radius-.5f,radius,rect.Height-(radius*2)+1f);
				//LR
				g.FillPie(new SolidBrush(clrDark),rect.Right-(radius*2),rect.Bottom-(radius*2),radius*2,radius*2,0,90);
				brush=new LinearGradientBrush(new PointF(rect.Right-radius,rect.Bottom-radius),
					new PointF(rect.Right-(radius*.5f)+.5f,rect.Bottom-(radius*.5f)+.5f),
					clrMain,clrDark);
				g.FillPolygon(brush,new PointF[] {
					new PointF(rect.Right-radius,rect.Bottom-radius),
					new PointF(rect.Right,rect.Bottom-radius),
					new PointF(rect.Right-radius,rect.Bottom)});
				//bottom
				brush=new LinearGradientBrush(new PointF(rect.Left+radius,rect.Bottom-radius),
					new PointF(rect.Left+radius,rect.Bottom),
					clrMain,clrDark);
				g.FillRectangle(brush,rect.Left+radius-.5f,rect.Bottom-radius,rect.Width-(radius*2)+1f,radius);
				//LL
				//2 pies of 45 each.
				brush=new LinearGradientBrush(new PointF(rect.Left+(radius/2f),rect.Bottom-(radius/2f)),
					new PointF(rect.Left+radius,rect.Bottom),
					clrMain,clrDark);
				g.FillPie(brush,rect.Left,rect.Bottom-(radius*2),radius*2,radius*2,90,45);
				brush=new LinearGradientBrush(new PointF(rect.Left+(radius/2f),rect.Bottom-(radius/2f)),
					new PointF(rect.Left,rect.Bottom-radius),
					clrMain,clrLight);
				g.FillPie(brushS,rect.Left,rect.Bottom-(radius*2),radius*2,radius*2,135,45);
				//left
				brush=new LinearGradientBrush(new PointF(rect.Left+radius,rect.Top),
					new PointF(rect.Left,rect.Top),
					clrMain,clrLight);
				g.FillRectangle(brushS,rect.Left,rect.Top+radius,radius,rect.Height-(radius*2));
				//UL
				g.FillPie(//new SolidBrush(clrLight)
					brushS,rect.Left,rect.Top,radius*2,radius*2,180,90);
				brush=new LinearGradientBrush(new PointF(rect.Left+radius,rect.Top+radius),
					new PointF(rect.Left+(radius/2f),rect.Top+(radius/2f)),
					clrMain,clrLight);
				//center
				GraphicsPath path=new GraphicsPath();
				path.AddEllipse(rect.Left-rect.Width/8f,rect.Top-rect.Height/2f,rect.Width,rect.Height*3f/2f);
				PathGradientBrush pathBrush=new PathGradientBrush(path);
				pathBrush.CenterColor=Color.FromArgb(255,255,255,255);
				pathBrush.SurroundColors=new Color[] { Color.FromArgb(0,255,255,255) };
				g.FillRectangle(new SolidBrush(clrMain),
					rect.Left+radius-.5f,rect.Top+radius-.5f,
					rect.Width-(radius*2)+1f,rect.Height-(radius*2)+1f);
				g.FillRectangle(
					pathBrush,
					rect.Left+radius-.5f,rect.Top+radius-.5f,
					rect.Width-(radius*2)+1f,rect.Height-(radius*2)+1f);
				//highlights
				brush=new LinearGradientBrush(new PointF(rect.Left+radius,rect.Top),
					new PointF(rect.Left+radius+rect.Width*2f/3f,rect.Top),
					clrLight,clrMain);
				g.FillRectangle(brush,rect.Left+radius,rect.Y+radius*3f/8f,rect.Width/2f,radius/4f);
				path=new GraphicsPath();
				path.AddLine(rect.Left+radius,rect.Top+radius*3/8,rect.Left+radius,rect.Top+radius*5/8);
				path.AddArc(new RectangleF(rect.Left+radius*5/8,rect.Top+radius*5/8,radius*3/4,radius*3/4),270,-90);
				path.AddArc(new RectangleF(rect.Left+radius*3/8,rect.Top+radius*7/8,radius*1/4,radius*1/4),0,180);
				path.AddArc(new RectangleF(rect.Left+radius*3/8,rect.Top+radius*3/8,radius*5/4,radius*5/4),180,90);
				//g.DrawPath(Pens.Red,path);
				g.FillPath(new SolidBrush(clrLight),path);
			}

			///<summary>Draws the text and image</summary>
			private void DrawItemButTextAndImage(Graphics g,ODPanelItem item) {
				g.SmoothingMode = SmoothingMode.HighQuality;
				SolidBrush brushText=new SolidBrush(ForeColor);
				SolidBrush brushGlow=new SolidBrush(Color.White);
				StringFormat sf=DrawItemButGetStringFormat(ContentAlignment.MiddleCenter);
				RectangleF recGlow1;
				RectangleF recText;
				recGlow1=new RectangleF(item.Location.X+.5f,item.Location.Y+.5f,item.ItemWidth,_rowHeight);
				recText=new RectangleF(item.Location.X,item.Location.Y,item.ItemWidth,_rowHeight);
				g.DrawString(item.Text,this.Font,brushGlow,recGlow1,sf);
				g.DrawString(item.Text,this.Font,brushText,recText,sf);
				brushText.Dispose();
				sf.Dispose();
			}

			private StringFormat DrawItemButGetStringFormat(ContentAlignment contentAlignment) {
				if(!Enum.IsDefined(typeof(ContentAlignment),(int)contentAlignment))
					throw new System.ComponentModel.InvalidEnumArgumentException(
						"contentAlignment",(int)contentAlignment,typeof(ContentAlignment));
				StringFormat stringFormat = new StringFormat();
				switch(contentAlignment) {
					case ContentAlignment.MiddleCenter:
						stringFormat.LineAlignment = StringAlignment.Center;
						stringFormat.Alignment = StringAlignment.Center;
						break;
					case ContentAlignment.MiddleLeft:
						stringFormat.LineAlignment = StringAlignment.Center;
						stringFormat.Alignment = StringAlignment.Near;
						break;
					case ContentAlignment.MiddleRight:
						stringFormat.LineAlignment = StringAlignment.Center;
						stringFormat.Alignment = StringAlignment.Far;
						break;
					case ContentAlignment.TopCenter:
						stringFormat.LineAlignment = StringAlignment.Near;
						stringFormat.Alignment = StringAlignment.Center;
						break;
					case ContentAlignment.TopLeft:
						stringFormat.LineAlignment = StringAlignment.Near;
						stringFormat.Alignment = StringAlignment.Near;
						break;
					case ContentAlignment.TopRight:
						stringFormat.LineAlignment = StringAlignment.Near;
						stringFormat.Alignment = StringAlignment.Far;
						break;
					case ContentAlignment.BottomCenter:
						stringFormat.LineAlignment = StringAlignment.Far;
						stringFormat.Alignment = StringAlignment.Center;
						break;
					case ContentAlignment.BottomLeft:
						stringFormat.LineAlignment = StringAlignment.Far;
						stringFormat.Alignment = StringAlignment.Near;
						break;
					case ContentAlignment.BottomRight:
						stringFormat.LineAlignment = StringAlignment.Far;
						stringFormat.Alignment = StringAlignment.Far;
						break;
				}
				return stringFormat;
			}
			#endregion Button Drawing Helper Functions

		///<summary>Draws outline around entire control.</summary>
		private void DrawOutline(Graphics g) {
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

		#region Single Click
		protected override void OnClick(EventArgs e) {
			base.OnClick(e);
			SelectedItem=PointToItem(_pointMouseClick);
			SelectedRow=_pointMouseClick.Y/_rowHeight;
			if(SelectedItem!=null) {
				//OnClickItem(SelectedItem);
				switch(SelectedItem.ItemType) {
					case ODPanelItemType.Button:
						OnClickButton(SelectedItem);
						break;
					//case ODPanelItemType.Label:
					//	OnClickLabel(SelectedItem);
					//	break;
				}
			}
			//OnRowClick(SelectedRow);
		}

		private void OnClickButton(ODPanelItem itemClick) {
			ODButtonPanelEventArgs pArgs=new ODButtonPanelEventArgs(SelectedItem,SelectedRow,MouseButtons.Left);
			if(ItemClickBut!=null) {
				ItemClickBut(this,pArgs);
			}
		}
		#endregion Single Click

		#region Double Click
		///<summary></summary>
		protected override void OnDoubleClick(EventArgs e) {
			base.OnDoubleClick(e);
			SelectedItem=PointToItem(_pointMouseClick);
			SelectedRow=_pointMouseClick.Y/_rowHeight;
			//if(SelectedItem!=null) {
			//	OnDoubleClickItem(SelectedItem);
			//	switch(SelectedItem.ItemType) {
			//		case ODPanelItemType.Button:
			//			OnDoubleClickButton(SelectedItem);
			//			break;
			//		case ODPanelItemType.Label:
			//			OnDoubleClickLabel(SelectedItem);
			//			break;
			//	}
			//}
			OnRowDoubleClick(_pointMouseClick.Y/_rowHeight);
		}

		private void OnRowDoubleClick(int p) {
			ODButtonPanelEventArgs pArgs=new ODButtonPanelEventArgs(SelectedItem,SelectedRow,MouseButtons.Left);
			if(RowDoubleClick!=null) {
				RowDoubleClick(this,pArgs);
			}
		}
		#endregion Double Click

		#endregion Clicking

		#region MouseEvents

		///<summary></summary>
		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			_pointMouseClick=new Point(e.X,e.Y);
			//ODPanelItem item=PointToItem(_pointMouseClick);
			//int row=e.Y/_rowHeight;
		}

		#endregion MouseEvents

		#region BeginEndUpdate
		///<summary>Call this before adding any rows.  You would typically call Rows.Clear after this.</summary>
		public void BeginUpdate() {
			IsUpdating=true;
		}

		///<summary>Must be called after adding rows.  This computes the columns, computes the rows, lays out the scrollbars, clears SelectedIndices, and invalidates.</summary>
		public void EndUpdate() {
			using(Graphics g=this.CreateGraphics()) {
				ComputeWidthsAndLocations(g);
			}
			IsUpdating=false;
			Invalidate();
		}
		#endregion BeginEndUpdate

	}

	///<summary></summary>
	public class ODButtonPanelEventArgs {
		private ODPanelItem item;
		private int row;
		private MouseButtons button;

		///<summary></summary>
		public ODButtonPanelEventArgs(ODPanelItem item,int row,MouseButtons button) {
			this.item=item;
			this.row=row;
			this.button=button;
		}

		///<summary>Can be null.</summary>
		public ODPanelItem Item {
			get {
				return item;
			}
		}

		///<summary></summary>
		public int Row {
			get {
				return row;
			}
		}

		///<summary>Gets which mouse button was pressed.</summary>
		public MouseButtons Button {
			get {
				return button;
			}
		}

	}

}
