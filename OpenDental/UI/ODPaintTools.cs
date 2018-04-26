using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace OpenDental.UI {

	///<summary>Contains all of the brushes, pens, and colors needed to draw an ODToolBar</summary>
	public class ODPaintTools {
		private LinearGradientBrush _brushPushed;
		private LinearGradientBrush _brushHover;
		private LinearGradientBrush _brushMedium;
		private LinearGradientBrush _brushDark;
		///<summary>The same orange color as the unread task notification color.</summary>
		public static Color ColorNotify=Color.FromArgb(252,178,129);
		///<summary>The darker version of ColorNotify.  This color is useful for coloring grid cell text, because it contrasts well with the normal white background, as well as the slate gray background of selected cells, as well as the normal black text.  Calculated by using website paletton.com.  This darker color is not calculated by using a simple division or subtraction, but is instead calculated using a more advanced formula.</summary>
		public static Color ColorNotifyDark=Color.FromArgb(182,98,44);
		///<summary>The same orange color as the unread task notification color.</summary>
		private static LinearGradientBrush _brushNotify=new LinearGradientBrush(new Point(0,0),new Point(0,25),Color.FromArgb(255,231,167),ColorNotify);
		private Brush _brushTextFore;
		private Brush _brushText;
		private Brush _brushTextDisabled;
		private Pen _penOutline;
		private Pen _penDivider;

		public LinearGradientBrush BrushPushed {
			get {
				return _brushPushed;
			}
		}

		public LinearGradientBrush BrushHover {
			get {
				return _brushHover;
			}
		}

		public LinearGradientBrush BrushMedium {
			get {
				return _brushMedium;
			}
		}

		public LinearGradientBrush BrushDark {
			get {
				return _brushDark;
			}
		}

		public LinearGradientBrush BrushNotify {
			get {
				return _brushNotify;
			}
		}

		public Brush BrushTextFore {
			get {
				return _brushTextFore;
			}
		}

		public Brush BrushText {
			get {
				return _brushText;
			}
		}

		public Brush BrushTextDisabled {
			get {
				return _brushTextDisabled;
			}
		}

		public Pen PenOutline {
			get {
				return _penOutline;
			}
		}

		public Pen PenDivider {
			get {
				return _penDivider;
			}
		}

		///<summary>Creates brushes and pens as the default OD color scheme.  Must pass in the Control.DefaultForColor.</summary>
		public ODPaintTools(Color defaultForeColor) {
			//Old grey theme.
			Color cTopPushed=Color.FromArgb(248,248,248);
			Color cBotPushed=Color.FromArgb(248,248,248);
			Color cTopHover=Color.FromArgb(240,240,240);
			Color cBotHover=Color.FromArgb(240,240,240);
			Color cTopMedium=SystemColors.Control;
			Color cBotMedium=SystemColors.Control;
			Color cTopDark=Color.FromArgb(210,210,210);
			Color cBotDark=Color.FromArgb(210,210,210);
			Color cTextFore=defaultForeColor;
			Color cText=Color.DarkSlateGray;
			Color cTextDisabled=SystemColors.GrayText;
			Color cPenOutline=Color.SlateGray;
			Color cPenDivider=Color.FromArgb(180,180,180);
			//Brushes
			_brushPushed				=new LinearGradientBrush(new Point(0,0),new Point(0,25),cTopPushed,cBotPushed);
			_brushHover					=new LinearGradientBrush(new Point(0,0),new Point(0,25),cTopHover,cBotHover);
			_brushMedium				=new LinearGradientBrush(new Point(0,0),new Point(0,25),cTopMedium,cBotMedium);
			_brushDark					=new LinearGradientBrush(new Point(0,0),new Point(0,25),cTopDark,cBotDark);
			_brushTextFore			=new SolidBrush(cTextFore);
			_brushText					=new SolidBrush(cText);
			_brushTextDisabled	=new SolidBrush(cTextDisabled);
			//Pens
			_penOutline	=new Pen(cPenOutline,1);
			_penDivider	=new Pen(cPenDivider,1);
		}

		///<summary>Creates brushes and pens with custom color gradiant colors used to draw OD controls.  Must pass in the Control.DefaultForColor and the two colors you want to gradiant between.
		///<para>Used to derive the blue theme and red theme used for NewCrop button.</para></summary>
		public ODPaintTools(Color defaultForeColor,Color cTopMedium,Color cBotMedium) {
			//Static Colors
			Color cTextFore=defaultForeColor;
			Color cText=Color.Black;
			Color cTextDisabled=SystemColors.GrayText;
			Color cPenOutline=Color.SlateGray;
			Color cPenDivider=Color.FromArgb(180,180,180);
			int l=10;//L for light. added to light values of shaded buttons
			int d=-30;//D for dark. added to dark values of shadded buttons.
			//Derived colors
			Color cTopPushed=Color.FromArgb((byte)Math.Min(255,cTopMedium.R+2*l),(byte)Math.Min(255,cTopMedium.G+2*l),(byte)Math.Min(255,cTopMedium.B+2*l));
			Color cBotPushed=Color.FromArgb((byte)Math.Min(255,cBotMedium.R+2*l),(byte)Math.Min(255,cBotMedium.G+2*l),(byte)Math.Min(255,cBotMedium.B+2*l));
			Color cTopHover=cTopMedium;//Color.FromArgb((byte)Math.Min(255,cTopMedium.R+l),(byte)Math.Min(255,cTopMedium.G+l),(byte)Math.Min(255,cTopMedium.B+l));
			Color cBotHover=cBotMedium;//Color.FromArgb((byte)Math.Min(255,cBotMedium.R+l),(byte)Math.Min(255,cBotMedium.G+l),(byte)Math.Min(255,cBotMedium.B+l));
			Color cTopDark=Color.FromArgb((byte)Math.Min(255,cTopMedium.R+d),(byte)Math.Min(255,cTopMedium.G+d),(byte)Math.Min(255,cTopMedium.B+d));
			Color cBotDark=Color.FromArgb((byte)Math.Min(255,cBotMedium.R+d),(byte)Math.Min(255,cBotMedium.G+d),(byte)Math.Min(255,cBotMedium.B+d));
			//Brushes
			_brushPushed				=new LinearGradientBrush(new Point(0,0),new Point(0,25),cTopPushed,cBotPushed);
			_brushHover					=new LinearGradientBrush(new Point(0,0),new Point(0,25),cTopHover,cBotHover);
			_brushMedium				=new LinearGradientBrush(new Point(0,0),new Point(0,25),cTopMedium,cBotMedium);
			_brushDark					=new LinearGradientBrush(new Point(0,0),new Point(0,25),cTopDark,cBotDark);
			_brushTextFore			=new SolidBrush(cTextFore);
			_brushText					=new SolidBrush(cText);
			_brushTextDisabled	=new SolidBrush(cTextDisabled);
			//Pens
			_penOutline	=new Pen(cPenOutline,1);
			_penDivider	=new Pen(cPenDivider,1);
		}

	}

}


