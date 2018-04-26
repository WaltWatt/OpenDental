using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class PhoneTile:UserControl {
		private Phone _phoneCur;
		///<summary>Passed in.  The amount of time it has taken for all the code to execute.</summary>
		public TimeSpan TimeDelta;
		///<summary></summary>
		[Category("Property Changed"),Description("Event raised when user wants to go to a patient or related object.")]
		public event EventHandler GoToChanged=null;
		///<summary></summary>
		[Category("Property Changed"),Description("Event raised when certain controls are selected on this tile related to menu events.")]
		public event EventHandler SelectedTileChanged=null;
		[Category("Property Changed"),Description("Event raised when the 'Needs Help' button (not the menuItem) is clicked.")]
		public event EventHandler NeedsHelpClicked=null;
		///<summary></summary>
		[Category("Action"),Description("Event raised when user clicks on screenshot.")]
		public event EventHandler ScreenshotClick=null;
		///<summary>Object passed in from parent form.  Event will be fired from that form.</summary>
		public ContextMenuStrip MenuNumbers;
		///<summary>Object passed in from parent form.  Event will be fired from that form.</summary>
		public ContextMenuStrip MenuStatus;
		private bool _layoutHorizontal=true;
		public bool ShowImageForced;
		//private const string _cPhone  = "☎";//U+260E
		//private const string _cPerson = "👤";//U+1F464
		//private const  _charPhoneTouch = "🕿";//U+1F57F
		//private const  string _cPersHelp = "💁";//U+1F481

		public PhoneTile() {
			InitializeComponent();
		}
		
		///<summary>Set phone and triage flag to display. Get/Set accessor won't work here because we require 2 seperate fields in order to update the control properly.</summary>
		public void SetPhone(Phone phone,PhoneEmpDefault phoneEmpDefault,ChatUser chatUserCur, bool isTriageOperator) {
			_phoneCur=phone;
			if(_phoneCur==null) { //empty out everything and return
				pictureInUse.Visible=false;	//pictureInUse.Visible=false;
				labelStatusAndNote.Text="";
				pictureProx.Visible=false;
				labelExtensionName.Text="";
				labelTime.Text="";
				labelTime.BackColor=this.BackColor;
				pictureNeedsHelpButton.Visible=false;
				pictureGTA.Visible=false;
				labelCustomer.Text="Invalid Comp or Phone";
				return;
			}
			pictureInUse.Visible=_phoneCur.Description!="";
			//Check if the user is logged in.
			if(_phoneCur.ClockStatus==ClockStatusEnum.Home
				|| _phoneCur.ClockStatus==ClockStatusEnum.None
				|| _phoneCur.ClockStatus==ClockStatusEnum.Off) 
			{
				labelStatusAndNote.Text="Clock In";
			}
			else {
				labelStatusAndNote.Text=_phoneCur.ClockStatus.GetDescription();
			}
			//Always show ext and name, no matter if user is clocked in or not. This keeps phone tiles from appearing blank with no extension and name.
			if(_phoneCur.EmployeeName!="") {
				labelExtensionName.Text=_phoneCur.Extension.ToString()+" - "+_phoneCur.EmployeeName;
			}
			else {
				labelExtensionName.Text=_phoneCur.Extension.ToString()+" - Vacant";
			}
			if(_phoneCur.DateTimeNeedsHelpStart.Date==DateTime.Today) {
				labelTime.Text=(DateTime.Now-_phoneCur.DateTimeNeedsHelpStart+TimeDelta).ToStringHmmss();
			}
			else if(_phoneCur.DateTimeStart.Date==DateTime.Today) {
				labelTime.Text=(DateTime.Now-_phoneCur.DateTimeStart+TimeDelta).ToStringHmmss();
			}
			else {
				labelTime.Text="";
			}
			if(_phoneCur.ClockStatus==ClockStatusEnum.Home
				|| _phoneCur.ClockStatus==ClockStatusEnum.None
				|| _phoneCur.ClockStatus==ClockStatusEnum.Break) {
				labelTime.BackColor=this.BackColor;//No color if employee is not currently working.
				pictureNeedsHelpButton.BackColor=this.BackColor;//No color if employee is not currently working.
			}
			else {
				Color outerColor;
				Color innerColor;
				Color fontColor;
				bool isTriageOperatorOnTheClock=false;
				//get the cubicle color and triage status
				Phones.GetPhoneColor(phone,phoneEmpDefault,false,out outerColor,out innerColor,out fontColor,out isTriageOperatorOnTheClock);
				if(!timerFlash.Enabled) {
					//if the control is already flashing then don't overwrite the colors. this would cause a "spastic" flash effect.
					Phones.PhoneColorScheme colorScheme=new Phones.PhoneColorScheme(true);
					labelTime.BackColor=outerColor;
					if(phone.ClockStatus==ClockStatusEnum.HelpOnTheWay) {
						labelTime.BackColor=colorScheme.ColorOuterNeedsHelp;
					}					
					if(_phoneCur.ClockStatus==ClockStatusEnum.NeedsHelp) {
						//Only start the flash timer and color the control once. This prevents over-flashing effect.
						labelTime.Tag=new object[2] { false,colorScheme.ColorOuterNeedsHelp};// labelTime.BackColor };
						timerFlash.Start();
					}
				}
			}
			if(_phoneCur.ClockStatus!=ClockStatusEnum.NeedsHelp) { //Always assume the flash timer was previously turned on and turn it off here. No harm if it' already off.
				timerFlash.Stop();
			}
			if(_phoneCur.ClockStatus==ClockStatusEnum.Home
				|| _phoneCur.ClockStatus==ClockStatusEnum.None) 
			{
				labelTime.BorderStyle=System.Windows.Forms.BorderStyle.None;//Remove color box if employee is not currently working.
			}
			else {
				labelTime.BorderStyle=System.Windows.Forms.BorderStyle.FixedSingle;
			}
			labelCustomer.Text=_phoneCur.CustomerNumber;
			//Always show ext and name, no matter if user is clocked in or not. This keeps phone tiles from appearing blank with no extension and name.
			if(_phoneCur.EmployeeName!="") {
				labelExtensionName.Text=_phoneCur.Extension.ToString() +" - "+_phoneCur.EmployeeName;// +(IsProximal ? " "+_cPerson : "");
			}
			else {
				labelExtensionName.Text=_phoneCur.Extension.ToString()+" - Vacant";
			}
			pictureProx.Visible=true;
			if(phone.IsProxVisible) {
				pictureProx.Image=Properties.Resources.Figure;
			}
			else if(phone.DateTProximal.AddHours(8)>DateTime.Now) {
				pictureProx.Image=Properties.Resources.NoFigure;//TODO: replace image with one from Nathan
			}
			else {
				pictureProx.Visible=false;
			}
			if(LayoutHorizontal) {
				this.labelExtensionName.Location=new Point(3,9);
				pictureProx.Location=new Point(173,11);
			}
			else {
				int stringW = TextRenderer.MeasureText(labelExtensionName.Text,labelExtensionName.Font).Width;
				int locationX = labelExtensionName.Location.X
					+stringW
					+(labelExtensionName.Width-stringW)/2 //half of the unused space around the centered text
					+1;//padding
				pictureProx.Location=new Point(locationX,6);
				pictureNeedsHelpButton.Visible=true;
			}
			if(phone.ClockStatus == ClockStatusEnum.Home
				|| phone.ClockStatus == ClockStatusEnum.Lunch
				|| phone.ClockStatus == ClockStatusEnum.Break
				|| phone.ClockStatus == ClockStatusEnum.None) {
				pictureNeedsHelpButton.Visible=false;
			}
			else {
				pictureNeedsHelpButton.Enabled=true;
				pictureNeedsHelpButton.Image=Properties.Resources.raisehanddisabled;
			}
			if(chatUserCur==null || chatUserCur.CurrentSessions == 0 || pictureInUse.Visible) {
				pictureGTA.Visible=false;
				pictureGTA.SendToBack();
			}
			else {
				pictureGTA.Visible=true;
				pictureGTA.BringToFront();
				labelTime.Text=TimeSpan.FromMilliseconds(chatUserCur.SessionTime).ToStringHmmss();
			}
		}

		///<summary>use SetPhone function to set phone and triage flag</summary>
		public Phone PhoneCur {
			get {
				return _phoneCur;
			}
		}
		
		[Category("Layout"),Description("Set true for horizontal layout and false for vertical.")]
		public bool LayoutHorizontal{
			get{
				return _layoutHorizontal;
			}
			set{
				_layoutHorizontal=value;
				if(_layoutHorizontal){
					//173,7
					pictureInUse.Location=new Point(6,25);
					pictureGTA.Location=new Point(6,25);
					this.labelExtensionName.Location=new Point(3,9);
					labelExtensionName.Size = new System.Drawing.Size(105,16);
					labelExtensionName.TextAlign=ContentAlignment.MiddleLeft;
					this.labelStatusAndNote.Location=new Point(31,22);
					labelStatusAndNote.TextAlign=ContentAlignment.MiddleLeft;
					labelStatusAndNote.Size=new Size(77,16);
					this.labelTime.Location=new Point(111,11);
					labelTime.Size=new Size(56,16);
					this.labelCustomer.Location=new Point(111,27);
					labelCustomer.Size=new Size(147,16);
					labelCustomer.TextAlign=ContentAlignment.MiddleLeft;
					this.pictureProx.Location=new Point(173,11);
					//Z-ordering
					pictureProx.BringToFront();
					labelTime.BringToFront();
					pictureNeedsHelpButton.Visible=false;
				}
				else {//vertical
					pictureInUse.Location=new Point(46,3);
					pictureGTA.Location=new Point(46,3);
					this.labelExtensionName.Location=new Point(0,3);//69,3);
					labelExtensionName.Size = new System.Drawing.Size(213,16);
					labelExtensionName.TextAlign = ContentAlignment.MiddleCenter;
					this.labelStatusAndNote.Location=new Point(12,21);
					labelStatusAndNote.TextAlign=ContentAlignment.MiddleCenter;
					labelStatusAndNote.Size=new Size(190,16);
					this.labelTime.Location=new Point(0,41);
					labelTime.TextAlign=ContentAlignment.MiddleCenter;
					labelTime.Size=new Size(213,17);
					this.labelCustomer.Location=new Point(0,59);
					labelCustomer.Size=new Size(213,16);
					labelCustomer.TextAlign=ContentAlignment.MiddleCenter;
					int locationX = labelExtensionName.Location.X
						+labelExtensionName.Width
						-(labelExtensionName.Width-(int)(TextRenderer.MeasureText(labelExtensionName.Text,labelExtensionName.Font).Width))/2 //half of the unused space around the centered text
						+5;//padding
					this.pictureProx.Location=new Point(Math.Min(locationX,labelExtensionName.Location.X+labelExtensionName.Width-10),25);
					pictureProx.BringToFront();
					pictureNeedsHelpButton.Visible=true;
					pictureNeedsHelpButton.Location=new Point(labelTime.Width-pictureNeedsHelpButton.Width,labelTime.Bounds.Y-pictureNeedsHelpButton.Height);
				}
			}
		}

		protected override Size DefaultSize {
			get {
				if(_layoutHorizontal){
					return new Size(267,50);
				}
				else{//vertical
					return new Size(150,82);
				}
			}
		}

		private void labelExtensionName_DoubleClick(object sender,EventArgs e) {
			if(_phoneCur==null || _phoneCur.EmployeeNum < 1) {
				return;
			}
			PhoneEmpDefault phoneEmpDefault=PhoneEmpDefaults.GetOne(_phoneCur.EmployeeNum);
			if(phoneEmpDefault==null) {
				MessageBox.Show("No 'phoneempdefault' row found for EmployeeNum "+_phoneCur.EmployeeNum
					+".\r\nGo to Phone Settings window and add a row for this employee.");
				return;
			}
			FormPhoneEmpDefaultEdit FormPEDE=new FormPhoneEmpDefaultEdit();
			FormPEDE.PedCur=phoneEmpDefault;
			FormPEDE.ShowDialog();
		}

		private void labelCustomer_MouseClick(object sender,MouseEventArgs e) {
			if((e.Button & MouseButtons.Right)==MouseButtons.Right) {
				return;
			}
			OnGoToChanged();
		}

		protected void OnGoToChanged() {
			if(GoToChanged!=null) {
				GoToChanged(this,new EventArgs());
			}
		}

		private void labelCustomer_MouseUp(object sender,MouseEventArgs e) {
			if(e.Button!=MouseButtons.Right) {
				return;
			}
			if(_phoneCur==null) {
				return;
			}
			OnSelectedTileChanged();
			MenuNumbers.Show(labelCustomer,e.Location);	
		}

		private void labelStatusAndNote_MouseUp(object sender,MouseEventArgs e) {
			if(e.Button!=MouseButtons.Right) {
				return;
			}
			if(_phoneCur==null) {
				return;
			}
			//Jason - Allowed to be 0 here.  The Security.UserCur.EmpNum will be used when they go to clock in and that is where the 0 check needs to be.
			//if(phoneCur.EmployeeNum==0) {
			//  return;
			//}
			OnSelectedTileChanged();
			bool allowStatusEdit=ClockEvents.IsClockedIn(PhoneCur.EmployeeNum);
			if(PhoneCur.EmployeeNum==Security.CurUser.EmployeeNum) { //Always allow status edit for yourself
				allowStatusEdit=true;
			}
			if(PhoneCur.ClockStatus==ClockStatusEnum.NeedsHelp) { //Always allow any employee to change any other employee from NeedsAssistance to Available
				allowStatusEdit=true;
			}
			string statusOnBehalfOf=PhoneCur.EmployeeName;
			bool allowSetSelfAvailable=false;
			if(!ClockEvents.IsClockedIn(PhoneCur.EmployeeNum) //No one is clocked in at this extension.
				&& !ClockEvents.IsClockedIn(Security.CurUser.EmployeeNum)) //This user is not clocked in either.
			{ 
				//Vacant extension and this user is not clocked in so allow this user to clock in at this extension.
				statusOnBehalfOf=Security.CurUser.UserName;
				allowSetSelfAvailable=true;
			}
			AddToolstripGroup("menuItemStatusOnBehalf","Status for: "+statusOnBehalfOf);
			AddToolstripGroup("menuItemRingGroupOnBehalf","Queues for ext: "+PhoneCur.Extension.ToString());
			AddToolstripGroup("menuItemClockOnBehalf","Clock event for: "+PhoneCur.EmployeeName);
			SetToolstripItemText("menuItemAvailable",allowStatusEdit || allowSetSelfAvailable);
			SetToolstripItemText("menuItemTraining",allowStatusEdit);
			SetToolstripItemText("menuItemTeamAssist",allowStatusEdit);
			SetToolstripItemText("menuItemNeedsHelp",allowStatusEdit);
			SetToolstripItemText("menuItemWrapUp",allowStatusEdit);
			SetToolstripItemText("menuItemOfflineAssist",allowStatusEdit);
			SetToolstripItemText("menuItemUnavailable",allowStatusEdit);
			SetToolstripItemText("menuItemTCResponder",allowStatusEdit);
			SetToolstripItemText("menuItemBackup",allowStatusEdit);
			SetToolstripItemText("menuItemLunch",allowStatusEdit);
			SetToolstripItemText("menuItemHome",allowStatusEdit);
			SetToolstripItemText("menuItemBreak",allowStatusEdit);
			MenuStatus.Show(labelStatusAndNote,e.Location);		
		}

		private void AddToolstripGroup(string groupName,string itemText) {
			ToolStripItem[] tsiFound=MenuStatus.Items.Find(groupName,false);
			if(tsiFound==null || tsiFound.Length<=0) {
				return;
			}
			tsiFound[0].Text=itemText;
		}

		private void SetToolstripItemText(string toolStripItemName,bool isClockedIn) {
			ToolStripItem[] tsiFound=MenuStatus.Items.Find(toolStripItemName,false);
			if(tsiFound==null || tsiFound.Length<=0) {
				return;
			}
			//set back to default
			tsiFound[0].Text=tsiFound[0].Text.Replace(" (Not Clocked In)","");
			if(isClockedIn) {
				tsiFound[0].Enabled=true;				
			}
			else {
				tsiFound[0].Enabled=false;
				tsiFound[0].Text=tsiFound[0].Text+" (Not Clocked In)";
			}			
		}

		protected void OnSelectedTileChanged() {
			if(SelectedTileChanged!=null) {
				SelectedTileChanged(this,new EventArgs());
			}
		}

		private void phoneTile_Click(object sender,EventArgs e) {
			ScreenshotClick?.Invoke(this,new EventArgs());
		}

		private void timerFlash_Tick(object sender,EventArgs e) {
			bool isColored=true;
			Color flashColor=SystemColors.Control;
			if(labelTime.Tag!=null 
				&& labelTime.Tag is object[]
				&& ((object[])labelTime.Tag).Length>=2) 
			{
					if(((object[])labelTime.Tag)[0] is bool) {
						isColored=(bool)((object[])labelTime.Tag)[0];
					}
					if(((object[])labelTime.Tag)[1] is Color) {
						flashColor=(Color)((object[])labelTime.Tag)[1];
					}
			}
			labelTime.BackColor=isColored ? this.BackColor : flashColor;
			labelTime.Tag=new object[2] { !isColored,flashColor };//this causes the isColored bit to flash, causing the colors to flash.
		}

		private void labelNeedsHelpButton_Click(object sender,EventArgs e) {
			if(_phoneCur==null) {
				return;
			}
			NeedsHelpClicked?.Invoke(this,new EventArgs());
		}
	}
}
