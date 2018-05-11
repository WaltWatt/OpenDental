using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using CodeBase;

namespace OpenDental.UI{
	///<summary></summary>
	public delegate void ODToolBarButtonClickEventHandler(object sender,ODToolBarButtonClickEventArgs e);
	///<summary></summary>
	public delegate void ODToolBarButtonPageNavEventHandler(object sender,ODToolBarButtonPageNavEventArgs e);

	///<summary>Open Dental Toolbar.  This is very custom.</summary>
	[DefaultEvent("ButtonClick")]
	public class ODToolBar : System.Windows.Forms.UserControl{
		///<summary>Required designer variable.</summary>
		private System.ComponentModel.Container components = null;
		private ODToolBarButtonCollection buttons=new ODToolBarButtonCollection();
		private ImageList imageList;
		private bool mouseIsDown;
		///<summary>A hot button is either: 1.The button that the mouseDown happened on, regardless of the current position of the mouse, or 2.If the mouse is not down, the button in State.Hover. Keeping track of which one is hot allows faster painting during mouse events.</summary>
		private ODToolBarButton hotButton;
		private ToolTip toolTip1;
		///<summary></summary>
		[Category("Action"),Description("Occurs when a button is clicked.")]
		public event ODToolBarButtonClickEventHandler ButtonClick=null;
		[Category("Action"),Description("Occurs when page navigation has occurred.")]
		public event ODToolBarButtonPageNavEventHandler PageNav=null;
		private static ODPaintTools _paintToolboxDefault; //Default toolbar colors. (Blue or Grey)
		private static ODPaintTools _paintToolboxError; //Color only used when button.IsRed=true. (Red)
		///<summary>This can be set from anywhere to affect all toolbars simultaneously.</summary>
		private static bool _useBlueTheme;
		private ValidNum textPageNav;

		///<summary></summary>
		public ODToolBar(){
			InitializeComponent();// This call is required by the Windows.Forms Form Designer.
			ReloadBrushes();
			toolTip1 = new ToolTip();
			toolTip1.InitialDelay=1100;
		}

		public static bool UseBlueTheme {
			get {
				return _useBlueTheme;
			}
			set {
				_useBlueTheme=value;
				ReloadBrushes();
			}
		}

		///<summary>Reloads brushes based on current theme.</summary>
		private static void ReloadBrushes() {
			_paintToolboxDefault=new ODPaintTools(DefaultForeColor);//(Usually Grey)
			if(UseBlueTheme) {
				_paintToolboxDefault=new ODPaintTools(DefaultForeColor,Color.FromArgb(255,255,255),Color.FromArgb(171,181,209));//(White and Blue)
			}
			_paintToolboxError=new ODPaintTools(DefaultForeColor,Color.FromArgb(255,192,192),Color.FromArgb(255,98,98));//Color.FromArgb(255,255,255),Color.FromArgb(255,98,98));//Color.FromArgb(255,192,192));//(Red)
		}

		///<summary>Clean up any resources being used.</summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// ODToolBar
			// 
			this.DoubleBuffered = true;
			this.Name = "ODToolBar";
			this.Load += new System.EventHandler(this.ODToolBar_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void ODToolBar_Load(object sender, System.EventArgs e) {
			if(imageList!=null){
				Height=imageList.ImageSize.Height+3;
			}
		}

		///<summary>Gets the collection of ODToolBarButton controls assigned to the toolbar control.</summary>
		public ODToolBarButtonCollection Buttons{
			get{
				return buttons;
			}
			//set{
			//}	
		}
		
		///<summary>Gets or sets the collection of images available to the toolbar buttons.</summary>
		public ImageList ImageList{
			get{
				return imageList;
			}
			set{
				imageList=value;
			}
		}
		
		///<summary>This should only happen when mouse enters. Only causes a repaint if needed.</summary>
		protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e){
      base.OnMouseMove(e);
			if(mouseIsDown){
				//regardless of whether a button is hot, nothing changes until the mouse is released.
				//a hot(pressed) button remains so, and no buttons are hot when hover
				//,so do nothing
			}
			else{//mouse is not down
				ODToolBarButton button=HitTest(e.X,e.Y);//this is the button the mouse is over at the moment.
				//first handle the old hotbutton
				if(hotButton!=null){//if there is a previous hotbutton
					if(hotButton!=button){//if we have moved to hover over a new button, or to hover over nothing
						hotButton.State=ToolBarButtonState.Normal;
						Invalidate(hotButton.Bounds);
					}
				}
				//then, the new button
				if(button!=null){
					if(hotButton!=button){//if we have moved to hover over a new button
						toolTip1.SetToolTip(this,button.ToolTipText);
						button.State=ToolBarButtonState.Hover;
						Invalidate(button.Bounds);
					}
					else{//Still hovering over the same button as before
						//do nothing.
					}
				}
				else{
					toolTip1.SetToolTip(this,"");
				}
				hotButton=button;//this might be null if hovering over nothing.
				//if there was no previous hotbutton, and there is not current hotbutton, then do nothing.
			}
		}

		///<summary>Returns the button that contains these coordinates, or null if no hit.</summary>
		private ODToolBarButton HitTest(int x,int y){
			foreach(ODToolBarButton button in buttons){
				if(button.Bounds.Contains(x,y))
					return button;
			}
			return null;
		}

		private bool HitTestDrop(ODToolBarButton button,int x,int y){
			Rectangle dropRect=new Rectangle(button.Bounds.X+button.Bounds.Width-15,button.Bounds.Y
				,15,button.Bounds.Height);
			if(dropRect.Contains(x,y)){
				return true;
			}
			return false;
		}
	  
		///<summary>Resets button appearance. This will also deactivate the button if it has been pressed but not released. A pressed button will still be hot, however, so that if the mouse enters again, it will behave properly.  Repaints only if necessary.</summary>
		protected override void OnMouseLeave(System.EventArgs e){
			base.OnMouseLeave(e);
			if(mouseIsDown){//mouse is down
				//if a button is hot, it will remain so, even if leave.  As long as mouse is down.
				//,so do nothing.
				//Also, if a button is not hot, nothing will happen when leave
				//,so do nothing.
			}
			else{//mouse is not down
				if(hotButton!=null){//if there was a previous hotButton
					hotButton.State=ToolBarButtonState.Normal;
					Invalidate(hotButton.Bounds);
					hotButton=null;
				}
			}
		}
	  
		///<summary>Change the button to a pressed state.</summary>
		protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e){
			base.OnMouseDown(e);
			if((e.Button & MouseButtons.Left)!=MouseButtons.Left){
				return;
			}
			mouseIsDown=true;
			ODToolBarButton button=HitTest(e.X,e.Y);
			if(button==null){//if there is no current hover button
				return;//don't set a hotButton
			}
			//if(!button.Enabled){
			//	return;//disabled buttons don't respond
			//}
			hotButton=button;
			if(button.Style==ODToolBarButtonStyle.DropDownButton
				&& HitTestDrop(button,e.X,e.Y))
			{
				button.State=ToolBarButtonState.DropPressed;
			}
			else{
				button.State=ToolBarButtonState.Pressed;
				
			}
			Invalidate(button.Bounds);
		}
	  
		///<summary>Change button to hover state and repaint if needed.</summary>
		protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e){
			base.OnMouseUp(e);
      if((e.Button & MouseButtons.Left)!=MouseButtons.Left){
				return;
			}
			mouseIsDown=false;
			ODToolBarButton button=HitTest(e.X,e.Y);
			if(hotButton==null){//if there was not a previous hotButton
				//do nothing
			}	
			else{//there was a hotButton
				hotButton.State=ToolBarButtonState.Normal;
				//but can't set it null yet, because still need it for testing
				Invalidate(hotButton.Bounds);//invalidate the old button
				//CLICK: 
				if(hotButton==button){//if mouse was released over the same button as it was depressed
					if(!button.Enabled){
						//disabled buttons don't respond at all
					}
					else if(button.Style==ODToolBarButtonStyle.DropDownButton//if current button is dropdown
						&& HitTestDrop(button,e.X,e.Y)//and we are in the dropdown area on the right
						&& button.DropDownMenu!=null)//and there is a dropdown menu to display
					{
						hotButton=null;
						button.State=ToolBarButtonState.Normal;
						Invalidate(button.Bounds);
						button.DropDownMenu.GetContextMenu().Show(this
							,new Point(button.Bounds.X,button.Bounds.Y+button.Bounds.Height));
					}
					else if(button.Style==ODToolBarButtonStyle.ToggleButton){//if current button is a toggle button
						if(button.Pushed)
							button.Pushed=false;
						else
							button.Pushed=true;
						OnButtonClicked(button);
					}
					else if(button.Style==ODToolBarButtonStyle.Label){
						//lables do not respond with click
					}
					else{
						OnButtonClicked(button);
					}
					return;//the button will not revert back to hover
				}//end of click section
				else{//there was a hot button, but it did not turn into a click
					hotButton=null;
				}
			}
			if(button!=null){//no click, and now there is a hover button, not the same as original button.
				//this section could easily be deleted, since all the user has to do is move the mouse slightly.
				button.State=ToolBarButtonState.Hover;
				hotButton=button;//set the current hover button to be the new hotbutton
				Invalidate(button.Bounds);
			}
		}

		///<summary></summary>
		protected void OnButtonClicked(ODToolBarButton myButton){
			ODToolBarButtonClickEventArgs bArgs=new ODToolBarButtonClickEventArgs(myButton);
			if(ButtonClick!=null) {
				ButtonClick(this,bArgs);
			}
		}

		private void RecalculateButtonsizes(Graphics g){
			int xPos=0;
			int height;
			int width;
			if(imageList==null){
				height=(int)g.MeasureString("anystring",Font).Height+10;
			}
			else{
				height=imageList.ImageSize.Height+2;
			}
			foreach(ODToolBarButton button in buttons){
				if(button.Style==ODToolBarButtonStyle.Separator) {
					width=4;
				}
				else if(button.Style==ODToolBarButtonStyle.PageNav) {
					//Set the size of the button to the width measurement of "0000/0000" with some padding.
					//The page navigation is tailored to print previews and users shouldn't have reports much larger than 9999 pages.
					width=(int)g.MeasureString("0000/0000",Font).Width+12;
				}
				else if(imageList==null || button.ImageIndex==-1) {//Normal button
					width=(int)g.MeasureString(button.Text,Font).Width+10;
				}
				else {//Image button
					if(button.Text=="")
						width=imageList.ImageSize.Width+7;//slightly wider than high for better 'feel'
					else
						width=imageList.ImageSize.Width+(int)g.MeasureString(button.Text,Font).Width+12;
				}
				//Check to see if this button needs a drop down.
				if(button.Style==ODToolBarButtonStyle.DropDownButton){
					if(button.Text==""){
						width-=10;//this way the main button part can be zero width if we just want the dropdown and nothing else.
					}
					width+=15;
				}
				button.Bounds=new Rectangle(new Point(xPos,0),new Size(width,height));
				xPos+=button.Bounds.Width;
			}
		}
	  
		///<summary>Runs any time the control is invalidated.</summary>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e){
			ODPaintTools paintToolboxCur=_paintToolboxDefault;
			if(DesignMode){
				e.Graphics.DrawRectangle(paintToolboxCur.PenOutline,0,0,Width-1,Height-1);//draw toolbar
				StringFormat format=new StringFormat();
				format.Alignment=StringAlignment.Center;
				format.LineAlignment=StringAlignment.Center;
				e.Graphics.DrawString(this.Name,Font,paintToolboxCur.BrushText,new Rectangle(0,0,Width,Height),format);
				return;
			}
			//OnPaint gets called a lot and the button collection can change while it is in the middle of drawing which can crash the program.
			//It's easier to just swallow any exception random exception that occurs because OnPaint will most likely get fired again real soon.
			//A better solution might be to surround Buttons with a read / write lock but that would be much more complicated than simply repainting.
			ODException.SwallowAnyException(() => {
				RecalculateButtonsizes(e.Graphics);
				foreach(ODToolBarButton button in Buttons) {
					//check to see if bound are in the clipping rectangle
					DrawButton(e.Graphics,button);
				}
				e.Graphics.DrawLine(paintToolboxCur.PenOutline,0,Height-1,Width-1,Height-1);
			});
		}

		private void DrawButton(Graphics g,ODToolBarButton button) {
			const int dropDownWidth=15;//The width of the dropdown rectangle where the triangle shows.
			bool isNotify=(button.Style==ODToolBarButtonStyle.DropDownButton && !String.IsNullOrWhiteSpace(button.NotificationText));//Notifies even when disabled.
			ODPaintTools paintToolboxCur=_paintToolboxDefault;
			if(button.IsRed) {
				paintToolboxCur=_paintToolboxError;
			}
			#region Separator
			if(button.Style==ODToolBarButtonStyle.Separator){
				//was 112,128,144
				//medium stripe
				g.DrawLine(new Pen(Color.FromArgb(190,200,210)),button.Bounds.Left,button.Bounds.Top+1,
					button.Bounds.Left,button.Bounds.Bottom-2);
				//dark stripe
				g.DrawLine(new Pen(Color.FromArgb(130,140,160)),button.Bounds.Left+1,button.Bounds.Top,
					button.Bounds.Left+1,button.Bounds.Bottom-1);
				//white stripe
				g.DrawLine(new Pen(Color.FromArgb(255,255,255)),button.Bounds.Left+2,button.Bounds.Top+1,
					button.Bounds.Left+2,button.Bounds.Bottom-2);
				return;
			}
			#endregion
			//draw background
			if(!button.Enabled){
				g.FillRectangle(paintToolboxCur.BrushMedium,button.Bounds);
			}
			else if(button.Style==ODToolBarButtonStyle.ToggleButton && button.Pushed){
				g.FillRectangle(paintToolboxCur.BrushPushed,button.Bounds);
			}
			else if(button.Style==ODToolBarButtonStyle.Label || button.Style==ODToolBarButtonStyle.PageNav){
				g.FillRectangle(paintToolboxCur.BrushMedium,button.Bounds);
			}
			else switch(button.State){
				case ToolBarButtonState.Normal://Control is 224,223,227 (==Ryan. This is not always true. Control is 240,240,240 for me.)
					g.FillRectangle(paintToolboxCur.BrushMedium,button.Bounds);
					break;
				case ToolBarButtonState.Hover://this is lighter than control
					g.FillRectangle(paintToolboxCur.BrushHover,button.Bounds);
					break;
				case ToolBarButtonState.Pressed://slightly darker than control
					g.FillRectangle(paintToolboxCur.BrushDark,button.Bounds);
					break;
				case ToolBarButtonState.DropPressed:
					//left half looks like hover:
					g.FillRectangle(paintToolboxCur.BrushHover
						,new Rectangle(button.Bounds.X,button.Bounds.Y
						,button.Bounds.Width-15,button.Bounds.Height));
					//right section looks like Pressed:
					g.FillRectangle(paintToolboxCur.BrushDark
						,new Rectangle(button.Bounds.X+button.Bounds.Width-15,button.Bounds.Y
						,15,button.Bounds.Height));
					break;
			}
			if(isNotify) {//Override dropdown background to show notification color.
				Rectangle rectDropDown=new Rectangle(button.Bounds.X+button.Bounds.Width-dropDownWidth,button.Bounds.Y,dropDownWidth,button.Bounds.Height);
				g.FillRectangle(paintToolboxCur.BrushNotify,rectDropDown);//Fill the dropdown background area with the notification color.
			}
			//draw image and/or text
			//Color textColor=ForeColor;
			Rectangle textRect;
			int textWidth=button.Bounds.Width;
			if(button.Style==ODToolBarButtonStyle.DropDownButton){
				textWidth-=15;
			}
			//if(!button.Enabled){
			//	textColor=SystemColors.GrayText;
			//}
			if(imageList!=null && button.ImageIndex!=-1 && button.ImageIndex<imageList.Images.Count){//draw image and text
				if(!button.Enabled){
					System.Windows.Forms.ControlPaint.DrawImageDisabled(g,imageList.Images[button.ImageIndex]
						,button.Bounds.X+3,button.Bounds.Y+1,SystemColors.Control);
					textRect=new Rectangle(button.Bounds.X+imageList.ImageSize.Width+3
						,button.Bounds.Y,textWidth-imageList.ImageSize.Width-3,button.Bounds.Height);
				}
				else if(button.State==ToolBarButtonState.Pressed){//draw slightly down and right
					g.DrawImage(imageList.Images[button.ImageIndex],button.Bounds.X+4,button.Bounds.Y+2);
					textRect=new Rectangle(button.Bounds.X+1+imageList.ImageSize.Width+3
						,button.Bounds.Y+1,textWidth-imageList.ImageSize.Width-3,button.Bounds.Height);
				}
				else{
					g.DrawImage(imageList.Images[button.ImageIndex],button.Bounds.X+3,button.Bounds.Y+1);
					textRect=new Rectangle(button.Bounds.X+imageList.ImageSize.Width+3
						,button.Bounds.Y,textWidth-imageList.ImageSize.Width-3,button.Bounds.Height);
				}
			}
			else{//only draw text
				if(button.Style==ODToolBarButtonStyle.Label || button.Style==ODToolBarButtonStyle.PageNav){
					textRect=new Rectangle(button.Bounds.X,button.Bounds.Y
						,textWidth,button.Bounds.Height);
				}
				else if(button.State==ToolBarButtonState.Pressed){//draw slightly down and right
					textRect=new Rectangle(button.Bounds.X+1,button.Bounds.Y+1
						,textWidth,button.Bounds.Height);
				}
				else{
					textRect=new Rectangle(button.Bounds.X,button.Bounds.Y
						,textWidth,button.Bounds.Height);
				}
			}
			StringFormat format;
			if(imageList!=null && button.ImageIndex!=-1){//if there is an image
				//draw text very close to image
				format=new StringFormat();
				format.Alignment=StringAlignment.Near;
				format.LineAlignment=StringAlignment.Center;
				if(button.Enabled) {
					g.DrawString(button.Text,Font,paintToolboxCur.BrushTextFore,textRect,format);
				}
				else {
					g.DrawString(button.Text,Font,paintToolboxCur.BrushTextDisabled,textRect,format);
				}
			}
			else{
				format=new StringFormat();
				format.Alignment=StringAlignment.Center;
				format.LineAlignment=StringAlignment.Center;
				if(button.Enabled) {
					g.DrawString(button.Text,Font,paintToolboxCur.BrushTextFore,textRect,format);
					//For page navigation buttons we ALWAYS show the text box so that users know they can type in the box to change pages.
					if(button.Style==ODToolBarButtonStyle.PageNav) {
						//Because we do not display zeros to users (e.g. 001 / 135) and instead leave them off (e.g. 1 / 135) the width will be lopsided
						//We need to draw each side of the page navigation (and pad a little) individually.
						SizeF size = g.MeasureString("/",Font);
						g.DrawString(button.PageValue.ToString(),Font,paintToolboxCur.BrushTextFore
							,new Rectangle(textRect.Location,new Size((textRect.Width/2)-(int)(size.Width/2)-2,textRect.Height))
							,new StringFormat() { Alignment=StringAlignment.Far,LineAlignment=StringAlignment.Center });
						g.DrawString(button.PageMax.ToString(),Font,paintToolboxCur.BrushTextFore
							,new Rectangle(new Point(textRect.X+(textRect.Width/2)+(int)(size.Width/2)+2,textRect.Y)
								,new Size((textRect.Width/2)-(int)(size.Width/2),textRect.Height))
							,new StringFormat() { Alignment=StringAlignment.Near,LineAlignment=StringAlignment.Center });
						//Only add the text box for page navigation once.  
						//However, we need to add the text box here in the paint otherwise we don't know how large to make the text box.
						if(textPageNav==null) {
							textPageNav=new ValidNum();
							textPageNav.Size=new Size((button.Bounds.Width-10)/2,button.Bounds.Height);
							textPageNav.KeyDown+=TextPageNav_KeyDown;
							textPageNav.MinVal=1;//There is no such thing as 0 pages in a preview, always set min to 1.
							textPageNav.RightToLeft=RightToLeft.Yes;
							this.Controls.Add(textPageNav);
						}
						//Always recalculate the location of the button just in case someone changes the size of another button on the toolbar.
						textPageNav.Location=new Point(button.Bounds.X+1,button.Bounds.Y+2);
						//Nav text changes constantly so we need to update it when redrawing.
						textPageNav.Text=button.PageValue.ToString();
						if(button.PageMax!=0) {
							textPageNav.MaxVal=button.PageMax;
						}
					}
				}
				else {
					g.DrawString(button.Text,Font,paintToolboxCur.BrushTextDisabled,textRect,format);
				}
			}
			//draw outline
			//Pen penR=penMedium;//new Pen(Color.FromArgb(180,180,180));
			if(!button.Enabled){
				//no outline
				g.DrawLine(paintToolboxCur.PenDivider,button.Bounds.Right-1,button.Bounds.Top,button.Bounds.Right-1,button.Bounds.Bottom-1);//vertical line on the right side
			}
			else if(button.Style==ODToolBarButtonStyle.ToggleButton && button.Pushed){
				g.DrawRectangle(paintToolboxCur.PenOutline,new Rectangle(button.Bounds.X,button.Bounds.Y
					,button.Bounds.Width-1,button.Bounds.Height-1));
			}
			else if(button.Style==ODToolBarButtonStyle.Label || button.Style==ODToolBarButtonStyle.PageNav){
				//no outline
				g.DrawLine(paintToolboxCur.PenDivider,button.Bounds.Right-1,button.Bounds.Top,button.Bounds.Right-1,button.Bounds.Bottom-1);//vertical line on the right side
			}
			else switch(button.State){
				case ToolBarButtonState.Normal:
					//no outline
						g.DrawLine(paintToolboxCur.PenDivider,button.Bounds.Right-1,button.Bounds.Top,button.Bounds.Right-1,button.Bounds.Bottom-1);
					break;
				case ToolBarButtonState.Hover:
					g.DrawRectangle(paintToolboxCur.PenOutline,new Rectangle(button.Bounds.X,button.Bounds.Y
						,button.Bounds.Width-1,button.Bounds.Height-1));
					break;
				case ToolBarButtonState.Pressed:
					g.DrawRectangle(paintToolboxCur.PenOutline,new Rectangle(button.Bounds.X,button.Bounds.Y
						,button.Bounds.Width-1,button.Bounds.Height-1));
					break;
				case ToolBarButtonState.DropPressed:
					g.DrawRectangle(paintToolboxCur.PenOutline,new Rectangle(button.Bounds.X,button.Bounds.Y
						,button.Bounds.Width-1,button.Bounds.Height-1));
					break;
			}
			if(button.Style==ODToolBarButtonStyle.DropDownButton){
				int adjDown=0;//The distance to push the triangle down to show the notification text.
				if(isNotify) {
					adjDown=6;
					//Draw the notification text.
					Size sizeText=TextRenderer.MeasureText(button.NotificationText,Font);
					g.DrawString(button.NotificationText,Font,(button.Enabled?paintToolboxCur.BrushTextFore:paintToolboxCur.BrushTextDisabled),
						button.Bounds.X+button.Bounds.Width+1-(dropDownWidth+sizeText.Width)/2f,button.Bounds.Y+2+sizeText.Height/2f,format);
				}
				Point[] triangle=new Point[3];
				triangle[0]=new Point(button.Bounds.X+button.Bounds.Width-11
					,button.Bounds.Y+button.Bounds.Height/2-2+adjDown);
				triangle[1]=new Point(button.Bounds.X+button.Bounds.Width-4
					,button.Bounds.Y+button.Bounds.Height/2-2+adjDown);
				triangle[2]=new Point(button.Bounds.X+button.Bounds.Width-8
					,button.Bounds.Y+button.Bounds.Height/2+2+adjDown);
				if(button.Enabled) {
					g.FillPolygon(paintToolboxCur.BrushTextFore,triangle);
				}
				else {
					g.FillPolygon(paintToolboxCur.BrushTextDisabled,triangle);
				}
				if(button.State!=ToolBarButtonState.Normal && button.Enabled){
					g.DrawLine(paintToolboxCur.PenOutline,button.Bounds.X+button.Bounds.Width-15,button.Bounds.Y
						,button.Bounds.X+button.Bounds.Width-15,button.Bounds.Y+button.Bounds.Height);
				}
			}
		}

		private void TextPageNav_KeyDown(object sender,KeyEventArgs e) {
			base.OnKeyDown(e);
			if(e.KeyCode==Keys.Enter && textPageNav.errorProvider1.GetError(textPageNav)=="") {
				int pageNum=textPageNav.MaxVal;
				try {
					pageNum=Int32.Parse(textPageNav.Text);
				}
				catch(Exception) {
					return;
				}
				PageNav(this,new ODToolBarButtonPageNavEventArgs(pageNum));
			}
		}

		///<summary>Like DrawButton but only called when using blue theme.</summary>
		//private void DrawButtonBlue(Graphics g,ODToolBarButton button) {
		//	#region Separator
		//	if(button.Style==ODToolBarButtonStyle.Separator) {
		//		//was 112,128,144
		//		//medium stripe
		//		g.DrawLine(new Pen(Color.FromArgb(190,200,210)),button.Bounds.Left,button.Bounds.Top+1,
		//			button.Bounds.Left,button.Bounds.Bottom-2);
		//		//dark stripe
		//		g.DrawLine(new Pen(Color.FromArgb(130,140,160)),button.Bounds.Left+1,button.Bounds.Top,
		//			button.Bounds.Left+1,button.Bounds.Bottom-1);
		//		//white stripe
		//		g.DrawLine(new Pen(Color.FromArgb(255,255,255)),button.Bounds.Left+2,button.Bounds.Top+1,
		//			button.Bounds.Left+2,button.Bounds.Bottom-2);
		//		return;
		//	}
		//	#endregion
		//	#region Background
		//	//draw background
		//	if(!button.Enabled) {
		//		g.FillRectangle(new SolidBrush(BlueThemeBottom),button.Bounds);//SystemColors.Control  224,223,227
		//	}
		//	else if(button.Style==ODToolBarButtonStyle.ToggleButton && button.Pushed) {
		//		g.FillRectangle(LGBrush,button.Bounds);//Color.FromArgb(248,248,248)
		//	}
		//	else if(button.Style==ODToolBarButtonStyle.Label) {
		//		g.FillRectangle(new SolidBrush(BlueThemeBottom),button.Bounds);//SystemColors.Control  224,223,227
		//	}
		//	else switch(button.State) {
		//			case ToolBarButtonState.Normal://Control is 224,223,227
		//				g.FillRectangle(LGBrush,button.Bounds);
		//				break;
		//			case ToolBarButtonState.Hover://this is lighter than control
		//				g.FillRectangle(LGBrushLighter,button.Bounds);
		//				break;
		//			case ToolBarButtonState.Pressed://slightly darker than control
		//				g.FillRectangle(LGBrushDarker,button.Bounds);
		//				break;
		//			case ToolBarButtonState.DropPressed:
		//				//left half looks like hover:
		//				g.FillRectangle(LGBrushLighter
		//					,new Rectangle(button.Bounds.X,button.Bounds.Y
		//					,button.Bounds.Width-15,button.Bounds.Height));
		//				//right section looks like Pressed:
		//				g.FillRectangle(LGBrushDarker
		//					,new Rectangle(button.Bounds.X+button.Bounds.Width-15,button.Bounds.Y
		//					,15,button.Bounds.Height));
		//				break;
		//		}
		//	#endregion
		//	#region Image and/or Text
		//	//draw image and/or text
		//	Color textColor=ForeColor;
		//	Rectangle textRect;
		//	int textWidth=button.Bounds.Width;
		//	if(button.Style==ODToolBarButtonStyle.DropDownButton) {
		//		textWidth-=15;
		//	}
		//	if(!button.Enabled) {
		//		textColor=SystemColors.GrayText;
		//	}
		//	if(imageList!=null && button.ImageIndex!=-1 && button.ImageIndex<imageList.Images.Count) {//draw image and text
		//		if(!button.Enabled) {
		//			System.Windows.Forms.ControlPaint.DrawImageDisabled(g,imageList.Images[button.ImageIndex]
		//				,button.Bounds.X+3,button.Bounds.Y+1,SystemColors.Control);
		//			textRect=new Rectangle(button.Bounds.X+imageList.ImageSize.Width+3
		//				,button.Bounds.Y,textWidth-imageList.ImageSize.Width-3,button.Bounds.Height);
		//		}
		//		else if(button.State==ToolBarButtonState.Pressed) {//draw slightly down and right
		//			g.DrawImage(imageList.Images[button.ImageIndex],button.Bounds.X+4,button.Bounds.Y+2);
		//			textRect=new Rectangle(button.Bounds.X+1+imageList.ImageSize.Width+3
		//				,button.Bounds.Y+1,textWidth-imageList.ImageSize.Width-3,button.Bounds.Height);
		//		}
		//		else {
		//			g.DrawImage(imageList.Images[button.ImageIndex],button.Bounds.X+3,button.Bounds.Y+1);
		//			textRect=new Rectangle(button.Bounds.X+imageList.ImageSize.Width+3
		//				,button.Bounds.Y,textWidth-imageList.ImageSize.Width-3,button.Bounds.Height);
		//		}
		//	}
		//	else {//only draw text
		//		if(button.Style==ODToolBarButtonStyle.Label) {
		//			textRect=new Rectangle(button.Bounds.X,button.Bounds.Y
		//				,textWidth,button.Bounds.Height);
		//		}
		//		else if(button.State==ToolBarButtonState.Pressed) {//draw slightly down and right
		//			textRect=new Rectangle(button.Bounds.X+1,button.Bounds.Y+1
		//				,textWidth,button.Bounds.Height);
		//		}
		//		else {
		//			textRect=new Rectangle(button.Bounds.X,button.Bounds.Y
		//				,textWidth,button.Bounds.Height);
		//		}
		//	}
		//	StringFormat format;
		//	if(imageList!=null && button.ImageIndex!=-1) {//if there is an image
		//		//draw text very close to image
		//		format=new StringFormat();
		//		format.Alignment=StringAlignment.Near;
		//		format.LineAlignment=StringAlignment.Center;
		//		g.DrawString(button.Text,Font,new SolidBrush(textColor),textRect,format);
		//	}
		//	else {
		//		format=new StringFormat();
		//		format.Alignment=StringAlignment.Center;
		//		format.LineAlignment=StringAlignment.Center;
		//		g.DrawString(button.Text,Font,new SolidBrush(textColor),textRect,format);
		//	}
		//	#endregion
		//	#region Outline
		//	//draw outline
		//	Pen penR=new Pen(Color.FromArgb(180,180,180));
		//	if(!button.Enabled) {
		//		//no outline
		//		g.DrawLine(penR,button.Bounds.Right-1,button.Bounds.Top,button.Bounds.Right-1,button.Bounds.Bottom-1);
		//	}
		//	else if(button.Style==ODToolBarButtonStyle.ToggleButton && button.Pushed) {
		//		g.DrawRectangle(Pens.SlateGray,new Rectangle(button.Bounds.X,button.Bounds.Y
		//			,button.Bounds.Width-1,button.Bounds.Height-1));
		//	}
		//	else if(button.Style==ODToolBarButtonStyle.Label) {
		//		//no outline
		//		g.DrawLine(penR,button.Bounds.Right-1,button.Bounds.Top,button.Bounds.Right-1,button.Bounds.Bottom-1);
		//	}
		//	else switch(button.State) {
		//			case ToolBarButtonState.Normal:
		//				//no outline
		//				g.DrawLine(penR,button.Bounds.Right-1,button.Bounds.Top,button.Bounds.Right-1,button.Bounds.Bottom-1);
		//				break;
		//			case ToolBarButtonState.Hover:
		//				g.DrawRectangle(Pens.SlateGray,new Rectangle(button.Bounds.X,button.Bounds.Y
		//					,button.Bounds.Width-1,button.Bounds.Height-1));
		//				break;
		//			case ToolBarButtonState.Pressed:
		//				g.DrawRectangle(Pens.SlateGray,new Rectangle(button.Bounds.X,button.Bounds.Y
		//					,button.Bounds.Width-1,button.Bounds.Height-1));
		//				break;
		//			case ToolBarButtonState.DropPressed:
		//				g.DrawRectangle(Pens.SlateGray,new Rectangle(button.Bounds.X,button.Bounds.Y
		//					,button.Bounds.Width-1,button.Bounds.Height-1));
		//				break;
		//		}
		//	if(button.Style==ODToolBarButtonStyle.DropDownButton) {
		//		Point[] triangle=new Point[3];
		//		triangle[0]=new Point(button.Bounds.X+button.Bounds.Width-11
		//			,button.Bounds.Y+button.Bounds.Height/2-2);
		//		triangle[1]=new Point(button.Bounds.X+button.Bounds.Width-4
		//			,button.Bounds.Y+button.Bounds.Height/2-2);
		//		triangle[2]=new Point(button.Bounds.X+button.Bounds.Width-8
		//			,button.Bounds.Y+button.Bounds.Height/2+2);
		//		g.FillPolygon(new SolidBrush(textColor),triangle);
		//		if(button.State!=ToolBarButtonState.Normal && button.Enabled) {
		//			g.DrawLine(Pens.SlateGray,button.Bounds.X+button.Bounds.Width-15,button.Bounds.Y
		//				,button.Bounds.X+button.Bounds.Width-15,button.Bounds.Y+button.Bounds.Height);
		//		}
		//	}
		//	#endregion
		//}

		

		

		

		


	}

	///<summary></summary>
	public class ODToolBarButtonClickEventArgs
	{
		//private OutlookButton outlookButton = null;//this is how to do it if a class instead of struct
		private ODToolBarButton button;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="myButton"></param>
		public ODToolBarButtonClickEventArgs(ODToolBarButton myButton){
			button=myButton;
		}

		///<summary></summary>
		public ODToolBarButton Button{
			get{ 
				return button;
			}
		}

	}	
	
	///<summary></summary>
	public class ODToolBarButtonPageNavEventArgs
	{
		private int _navValue;
		public ODToolBarButtonPageNavEventArgs(int navValue){
			_navValue=navValue;
		}

		///<summary></summary>
		public int NavValue{
			get{ 
				return _navValue;
			}
		}

	}
}
