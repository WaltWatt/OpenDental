using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	public partial class UserControlPhoneSmall:UserControl {
		private List<Phone> _listPhones;
		private List<PhoneEmpDefault> _listPhoneEmpDefaults;
		///<summary>When the GoToChanged event fires, this tells us which patnum.</summary>
		public long GotoPatNum;
		///<summary></summary>
		[Category("Property Changed"),Description("Event raised when user wants to go to a patient or related object.")]
		public event EventHandler GoToChanged=null;
		public int Extension;
		///<summary>A list of rooms for the phone map.</summary>
		private List<MapAreaContainer> _listRooms;
		///<summary>A list of all map areas.  Each map area represents a "tile" which is associated to a phone extension at HQ.</summary>
		private List<MapArea> _listMapAreas;
		private long _mapAreaContainerNum=0;
		private PhoneConf _phoneConf;
		private DateTime _dateTimeConfRoom;
		private DateTime _dateTimeConfRoomEnd;

		public PhoneTile PhoneTile {
			get {
				return phoneTile;
			}
		}
		
		public UserControlPhoneSmall() {
			InitializeComponent();
			phoneTile.GoToChanged += new System.EventHandler(this.phoneTile_GoToChanged);
			phoneTile.NeedsHelpClicked+= new System.EventHandler(this.toggleHelp_Click);
			phoneTile.MenuNumbers=menuNumbers;
			phoneTile.MenuStatus=menuStatus;
			//_listMapAreas=MapAreas.Refresh();//PROBLEM: This gets called before databases are selected
		}

		///<summary>Set list of phones to display. Get/Set accessor won't work here because we require 2 seperate fields in order to update the control properly.</summary>
		public void SetPhoneList(List<PhoneEmpDefault> peds,List<Phone> phones) {
			//create a new list so our sorting doesn't affect this list elsewhere
			_listPhones=new List<Phone>();
			_listMapAreas=MapAreas.Refresh();
			if(_listRooms==null) {
				UpdateComboRooms();//Only call this once from here otherwise it gets called too often and causes strange flickering.
			}
			if(_mapAreaContainerNum < 1) {
				_listPhones.AddRange(phones);
			}
			else {//A specific room was selected so we only want to show those extensions.
				//Find all the map areas that correspond to the selected map area container.
				List<MapArea> listMapAreas=_listMapAreas.FindAll(x => x.MapAreaContainerNum==_mapAreaContainerNum);
				//Get all phones that correspond to the map areas found.
				_listPhones=phones.FindAll(x => listMapAreas.Exists(y => y.Extension==x.Extension));
			}
			//We always want to sort the list of phones so that they display in a predictable fashion.
			_listPhones.Sort(new Phones.PhoneComparer(Phones.PhoneComparer.SortBy.ext));
			_listPhoneEmpDefaults=peds;
			Invalidate();
		}

		private void UpdateComboRooms() {
			int selectedIndex=comboRoom.SelectedIndex;
			_listRooms=PhoneMapJSON.GetFromDb();
			comboRoom.Items.Clear();
			comboRoom.Items.Add("All");
			for(int i=0;i<_listRooms.Count;i++) {
				comboRoom.Items.Add(_listRooms[i].Description);
				if(_listRooms[i].MapAreaContainerNum==_mapAreaContainerNum) {
					selectedIndex=i+1;
				}
			}
			comboRoom.SelectedIndex=selectedIndex==-1 ? 0 : selectedIndex;
		}

		private void comboRoom_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboRoom.SelectedIndex < 1) {
				_mapAreaContainerNum=0;
			}
			else {
				_mapAreaContainerNum=_listRooms[comboRoom.SelectedIndex-1].MapAreaContainerNum;
			}
		}

		///<summary>Set the phone which is linked to the extension at this desk. If phone==null then no phone info shown.</summary>
		public void SetPhone(Phone phone,PhoneEmpDefault phoneEmpDefault,ChatUser chat,bool isTriageOperator) {
			phoneTile.SetPhone(phone,phoneEmpDefault,chat,isTriageOperator);
		}

		///<summary>Sets the Enabled property on all controls that need to be toggled based on the phone tracking server heartbeat.</summary>
		public void SetEnabledStateForControls(bool enabled) {
			butGetConfRoom.Enabled=enabled;
		}

		private void FillTile() {
			//UpdateComboRooms();//We can't do this in the constructor and all the other methods fire too often.  FillTile is a good place for this.
												 //Get the new phone list from the database and redraw control.
			SetPhoneList(PhoneEmpDefaults.Refresh(),Phones.GetPhoneList());
			//Set the currently selected phone accordingly.
			if(_listPhones==null) {//No phone list. Shouldn't get here.
				phoneTile.SetPhone(null,null,null,false);
				return;
			}
			Phone phone=Phones.GetPhoneForExtension(_listPhones,Extension);
			PhoneEmpDefault phoneEmpDefault=null;
			ChatUser chatUser=null;
			if(phone!=null) {
				phoneEmpDefault=PhoneEmpDefaults.GetEmpDefaultFromList(phone.EmployeeNum,_listPhoneEmpDefaults);
				chatUser=ChatUsers.GetFromExt(phone.Extension);
			}
			phoneTile.SetPhone(phone,phoneEmpDefault,chatUser,PhoneEmpDefaults.IsTriageOperatorForExtension(Extension,_listPhoneEmpDefaults));	
		}

		private void UserControlPhoneSmall_Paint(object sender,PaintEventArgs e) {
			Graphics g=e.Graphics;
			g.FillRectangle(SystemBrushes.Control,this.Bounds);
			if(_listPhones==null) {
				return;
			}
			int columns=9;
			int rows=1;
			//Dynamically figure out how many rows are needed if there are any phone tiles present.
			if(_listPhones.Count > 0 && columns > 0) {
				rows=(_listPhones.Count + columns - 1) / columns;//Rounds up the result of tile count / columns.
			}
			float boxWidth=((float)this.Width)/columns; //21.4f;
			float boxHeight=17f;
			float comboBoxHeight=11f;
			float hTot=boxHeight*rows;
			//float x=0f;
			//float y=0f;
			int xTile=0;//Just in case this needs to be dynamic in the future as well.
			int yTile=(int)comboBoxHeight + 5;//The height of the lights plus a little padding.
			phoneTile.Location=new Point(xTile,yTile);
			////Create a white "background" rectangle so that any empty squares (no employees) will show as white boxes instead of no color.
			//g.FillRectangle(new SolidBrush(Color.White),x,y+comboBoxHeight,boxWidth*columns,boxHeight*rows);
			////Dynamically move the phone tile control down.
			////Dynamically resize the entire UserControlPhoneSmall.  If the width changes, update PhoneTile.LayoutHorizontal (property setter).
			//this.Size=new System.Drawing.Size(213,yTile+phoneTile.Height);
			//for(int i=0;i<_listPhones.Count;i++) {				
			//	//Draw the extension number if a person is available at that extension.
			//	if(_listPhones[i].ClockStatus!=ClockStatusEnum.Home
			//		&& _listPhones[i].ClockStatus!=ClockStatusEnum.None) {
			//		//Colors the box a color based on the corresponding phone's status.
			//		Color outerColor;
			//		Color innerColor;
			//		Color fontColor;
			//		bool isTriageOperatorOnTheClock=false;					
			//		//get the cubicle color and triage status
			//		PhoneEmpDefault ped=PhoneEmpDefaults.GetEmpDefaultFromList(_listPhones[i].EmployeeNum,_listPhoneEmpDefaults);
			//		Phones.GetPhoneColor(_listPhones[i],ped,false,out outerColor,out innerColor,out fontColor,out isTriageOperatorOnTheClock);
			//		using(Brush brush=new SolidBrush(outerColor)) {
			//			g.FillRectangle(brush,x*boxWidth,(y*boxHeight)+comboBoxHeight,boxWidth,boxHeight);
			//		}
			//		Font baseFont=new Font("Arial",7);
			//		SizeF extSize=g.MeasureString(_listPhones[i].Extension.ToString(),baseFont);
			//		float padX=(boxWidth-extSize.Width)/2;
			//		float padY=(boxHeight-extSize.Height)/2;
			//		using(Brush brush=new SolidBrush(Color.Black)) {
			//			g.DrawString(_listPhones[i].Extension.ToString(),baseFont,brush,(float)Math.Ceiling((x*boxWidth)+(padX)),(y*boxHeight)+(padY + comboBoxHeight));
			//		}
			//	}
			//	x++;
			//	if(x>=columns) {
			//		x=0f;
			//		y++;
			//	}
			//}
			////horiz lines
			//for(int i=0;i<rows+1;i++) {
			//	g.DrawLine(Pens.Black,0,(i*boxHeight)+comboBoxHeight,Width,(i*boxHeight)+comboBoxHeight);
			//}
			////Very bottom
			//g.DrawLine(Pens.Black,0,Height-1+comboBoxHeight,Width,Height-1+comboBoxHeight);
			////vert
			//for(int i=0;i<columns;i++) {
			//	g.DrawLine(Pens.Black,i*boxWidth,comboBoxHeight,i*boxWidth,hTot+comboBoxHeight);
			//}
			//g.DrawLine(Pens.Black,Width-1,comboBoxHeight,Width-1,hTot+comboBoxHeight);
		}

		private void phoneTile_GoToChanged(object sender,EventArgs e) {
			if(phoneTile.PhoneCur==null) {
				return;
			}
			if(phoneTile.PhoneCur.PatNum==0) {
				return;
			}
			GotoPatNum=phoneTile.PhoneCur.PatNum;
			OnGoToChanged();
		}

		protected void OnGoToChanged() {
			if(GoToChanged!=null) {
				GoToChanged(this,new EventArgs());
			}
		}

		private void menuItemManage_Click(object sender,EventArgs e) {
			PhoneUI.Manage(phoneTile);
		}

		private void menuItemAdd_Click(object sender,EventArgs e) {
			PhoneUI.Add(phoneTile);
		}

		//Timecards-------------------------------------------------------------------------------------

		private void menuItemAvailable_Click(object sender,EventArgs e) {
			SetAvailable();
		}

		private void menuItemTraining_Click(object sender,EventArgs e) {
			PhoneUI.Training(phoneTile);
			FillTile();
		}

		private void menuItemTeamAssist_Click(object sender,EventArgs e) {
			SetTeamAssist();
		}

		private void menuItemTCResponder_Click(object sender,EventArgs e) {
			SetTCResponder();
		}

		private void menuItemNeedsHelp_Click(object sender,EventArgs e) {
			PhoneUI.NeedsHelp(phoneTile);
			FillTile();
		}

		private void toggleHelp_Click(object sender,EventArgs e) {
			if(phoneTile.PhoneCur.ClockStatus == ClockStatusEnum.NeedsHelp
				|| phoneTile.PhoneCur.ClockStatus == ClockStatusEnum.HelpOnTheWay) {
				PhoneUI.Available(phoneTile);
			}
			else {
				PhoneUI.NeedsHelp(phoneTile);
			}
			FillTile();
		}

		private void menuItemWrapUp_Click(object sender,EventArgs e) {
			PhoneUI.WrapUp(phoneTile);
			FillTile();
		}

		private void menuItemOfflineAssist_Click(object sender,EventArgs e) {
			PhoneUI.OfflineAssist(phoneTile);
			FillTile();
		}

		private void menuItemUnavailable_Click(object sender,EventArgs e) {
			PhoneUI.Unavailable(phoneTile);
			FillTile();
		}

		private void menuItemBackup_Click(object sender,EventArgs e) {
			PhoneUI.Backup(phoneTile);
			FillTile();
		}

		public void SetAvailable() {
			PhoneUI.Available(phoneTile);
			FillTile();
		}

		public void SetTeamAssist() {
			PhoneUI.TeamAssist(phoneTile);
			FillTile();
		}
		
		public void SetTCResponder() {
			PhoneUI.TCResponder(phoneTile);
			FillTile();
		}

		//RingGroups---------------------------------------------------

		private void menuItemQueueTech_Click(object sender,EventArgs e) {
			PhoneUI.QueueTech(phoneTile);
		}

		private void menuItemQueueNone_Click(object sender,EventArgs e) {
			PhoneUI.QueueNone(phoneTile);
		}

		private void menuItemQueueDefault_Click(object sender,EventArgs e) {
			PhoneUI.QueueDefault(phoneTile);
		}

		private void menuItemQueueBackup_Click(object sender,EventArgs e) {
			PhoneUI.QueueBackup(phoneTile);
		}

		//Timecard---------------------------------------------------

		private void menuItemLunch_Click(object sender,EventArgs e) {
			PhoneUI.Lunch(phoneTile);
			FillTile();
		}

		private void menuItemHome_Click(object sender,EventArgs e) {
			PhoneUI.Home(phoneTile);
			FillTile();
		}

		private void menuItemBreak_Click(object sender,EventArgs e) {
			PhoneUI.Break(phoneTile);
			FillTile();
		}

		//Conference Room--------------------------------------------
		private void butGetConfRoom_Click(object sender,EventArgs e) {
			if(_listPhones==null || _listPhoneEmpDefaults==null) {
				FillTile();
			}
			PhoneEmpDefault ped=null;
			try {
				//phoneTile.PhoneCur could be null which the following line would fail so we will surround this with a try / catch.
				ped=_listPhoneEmpDefaults.FirstOrDefault(x => x.PhoneExt==phoneTile.PhoneCur.Extension);
				if(ped==null) {
					throw new ApplicationException("Invalid PhoneEmpDefault");
				}
			}
			catch(Exception ex) {
				ex.DoNothing();
				MsgBox.Show(this,"This extension is not currently associated to a valid PhoneEmpDefault row.");
				return;
			}
			if(ped.SiteNum < 1) {
				MsgBox.Show(this,"This extension is not currently associated to a site.\r\n"
					+"A site must first be set within the Edit Employee Setting window.");
				return;
			}
			if(ped.IsTriageOperator) {
				SetTeamAssist();
			}
			timerConfRoom.Stop();
			labelConfRoom.Text="Reserving Conf Room...";
			labelConfRoom.Visible=true;
			_phoneConf=PhoneConfs.GetAndReserveConfRoom(ped.SiteNum);
			_dateTimeConfRoom=DateTime.Now;
			_dateTimeConfRoomEnd=_dateTimeConfRoom.AddMinutes(5);
			timerConfRoom.Start();
		}

		private void butConfRooms_Click(object sender,EventArgs e) {
			FormPhoneConfs FormPC=new FormPhoneConfs();
			FormPC.ShowDialog();//ShowDialog because we do not this window to be floating open for long periods of time.
		}

		///<summary>After five minutes has passed, the conference room label will be hidden until the user reserves another conf room.</summary>
		private void timerConfRoom_Tick(object sender,EventArgs e) {
			_dateTimeConfRoom=_dateTimeConfRoom.AddSeconds(1);
			if(_phoneConf==null) {
				labelConfRoom.Text="No Conf Room Available\r\n"
					+"Please Try Again";
			}
			else {
				//A valid conference room was found and reserved.
				labelConfRoom.Text="Conf Room Reserved:\r\n"
					+_phoneConf.Extension.ToString()+"\r\n"
					+(_dateTimeConfRoomEnd - _dateTimeConfRoom).ToStringmmss();
			}
			labelConfRoom.Visible=true;
			if((_dateTimeConfRoom >= _dateTimeConfRoomEnd)) {
				_dateTimeConfRoom=DateTime.MinValue;
				labelConfRoom.Visible=false;
				timerConfRoom.Stop();
			}
		}
	}
}
