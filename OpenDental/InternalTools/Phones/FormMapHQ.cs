using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Drawing;
using System.Linq;
using System.Text;
using CodeBase;
using System.ComponentModel;

namespace OpenDental {
	public partial class FormMapHQ:ODForm {
		#region Events
		public event EventHandler RoomControlClicked;
		public event EventHandler ExtraMapClicked;
		[Category("Property Changed"),Description("Event raised when user wants to go to a patient or related object.")]
		public event EventHandler GoToChanged=null;
		#endregion

		#region Private Members
		///<summary>Keep track of full screen state</summary>
		private bool _isFullScreen;
		///<summary>This is the difference between server time and local computer time.  Used to ensure that times displayed are accurate to the second.  This value is usally just a few seconds, but possibly a few minutes.</summary>
		private TimeSpan _timeDelta;
		private List<MapAreaContainer> _listMaps;
		private MapAreaContainer _mapCur;
		///<summary>The site that is associated to the first three octets of the computer that has launched this map.</summary>
		private Site _siteCur;
		//preferences for setting triage alert colors
		private int _triageRedCalls,_triageRedTime,_voicemailCalls,_voicemailTime,_triageCalls,_triageTime,_triageTimeWarning;

		private PhoneEmpSubGroupType _curSubGroupType {
			get {
					if(!tabMain.TabPages.ContainsKey(PhoneEmpSubGroupType.Escal.ToString())) {//Control has not been initialized.
						return PhoneEmpSubGroupType.Escal;
					}
					return (PhoneEmpSubGroupType)(tabMain.SelectedTab.Tag??PhoneEmpSubGroupType.Escal);
			}
		}
		#endregion Private Members

		#region Initialize

		public FormMapHQ() {
			InitializeComponent();
			//Do not do anything to do with database or control init here. We will be using this ctor later in order to create a temporary object so we can figure out what size the form should be when the user comes back from full screen mode. Wait until FormMapHQ_Load to do anything meaningful.
			_isFullScreen=false;
			_timeDelta=MiscData.GetNowDateTime()-DateTime.Now;
			//Add the mousewheel event to allow mousewheel scrolling to repaint the grid as well.
			mapAreaPanelHQ.MouseWheel+=new MouseEventHandler(mapAreaPanelHQ_Scroll);
			mapAreaPanelHQ.RoomControlClicked+=MapAreaPanelHQ_RoomControlClicked;
			mapAreaPanelHQ.GoToChanged+=new System.EventHandler(this.MapAreaRoomControl_GoToChanged);
		}

		public void MapAreaPanelHQ_RoomControlClicked(object sender,EventArgs e) {
			RoomControlClicked?.Invoke(sender,e);
		}

		private void FormMapHQ_Load(object sender,EventArgs e) {
			_siteCur=SiteLinks.GetSiteByIP();
			if(_siteCur==null) {
				MessageBox.Show("Error.  No sites found in the cache.");
				DialogResult=DialogResult.Abort;
				Close();
				return;
			}
			FormOpenDental.AddMapToList(this);
			FillMaps();
			FillTabs();
			FillCombo();
			FillMapAreaPanel();
			FillTriagePreferences();
			FillTriageLabelColors();
		}

		private void FillMaps() {
			//Get the list of maps from our JSON preference.
			_listMaps=PhoneMapJSON.GetFromDb();
			//Add a custom order to this map list which will prefer maps that are associated to the local computer's site.
			_listMaps=_listMaps.OrderBy(x => x.SiteNum!=_siteCur.SiteNum)
				.ThenBy(x => x.MapAreaContainerNum).ToList();
			//Select the first map in our list that matches the site associated to the current computer.
			_mapCur=_listMaps[0];
		}

		private void FillTriageLabelColors() {
			labelTriageOpsCountLocal.SetTriageColors(_mapCur.SiteNum);
			labelTriageOpsCountTotal.SetTriageColors();
		}

		private void FillTabs() {
			tabMain.TabPages.Clear();
			tabMain.TabPages.Add(PhoneEmpSubGroupType.Avail.ToString(),PhoneEmpSubGroupType.Avail.ToString());//Both key and value are the same.
			tabMain.TabPages[0].Tag=PhoneEmpSubGroupType.Avail;
			List<PhoneEmpSubGroupType> eVals=Enum.GetValues(typeof(PhoneEmpSubGroupType)).Cast<PhoneEmpSubGroupType>().ToList();
			foreach(PhoneEmpSubGroupType e in eVals) {
				if(e==PhoneEmpSubGroupType.Avail) {
					continue;//Already added above
				}
				tabMain.TabPages.Add(e.ToString(),e.ToString());//Both key and value are the same.
				tabMain.TabPages[e.ToString()].Tag=e;
			}
		}

		private void FillCombo() {
			comboRoom.Items.Clear();
			foreach(MapAreaContainer mapCur in _listMaps) {
				comboRoom.Items.Add(mapCur.Description);
			}
			int selectedIndex=0;
			if(_mapCur!=null) {
				_listMaps.FindIndex(x => x.MapAreaContainerNum==_mapCur.MapAreaContainerNum);
			}
			comboRoom.SelectedIndex=(selectedIndex==-1 ? 0 : selectedIndex);
		}

		///<summary>Setup the map panel with the cubicles and labels before filling with real-time data. Call this on load or anytime the cubicle layout has changed.</summary>
		private void FillMapAreaPanel() {
			mapAreaPanelHQ.Controls.Clear();
			mapAreaPanelHQ.FloorHeightFeet=_mapCur.FloorHeightFeet;
			mapAreaPanelHQ.FloorWidthFeet=_mapCur.FloorWidthFeet;
			//fill the panel
			List<MapArea> clinicMapItems=MapAreas.Refresh();
			for(int i=0;i<clinicMapItems.Count;i++) {
				if(clinicMapItems[i].MapAreaContainerNum!=_listMaps[comboRoom.SelectedIndex].MapAreaContainerNum) {
					continue;
				}
				if(clinicMapItems[i].ItemType==MapItemType.Room) {
					mapAreaPanelHQ.AddCubicle(clinicMapItems[i]);
				}
				else if(clinicMapItems[i].ItemType==MapItemType.DisplayLabel) {
					mapAreaPanelHQ.AddDisplayLabel(clinicMapItems[i]);
				}
			}
		}

		/// <summary>Gets the preferences from the database that determine when the map alert colors change.</summary>
		private void FillTriagePreferences() {
			_triageRedCalls=PrefC.GetInt(PrefName.TriageRedCalls);
			_triageCalls=PrefC.GetInt(PrefName.TriageCalls);
			_triageRedTime=PrefC.GetInt(PrefName.TriageRedTime);
			_triageTime=PrefC.GetInt(PrefName.TriageTime);
			_triageTimeWarning=PrefC.GetInt(PrefName.TriageTimeWarning);
			_voicemailCalls=PrefC.GetInt(PrefName.VoicemailCalls);
			_voicemailTime=PrefC.GetInt(PrefName.VoicemailTime);
		}

		#endregion

		#region Set label text and colors

		public void SetEServiceMetrics(EServiceMetrics metricsToday) {
			eServiceMetricsControl.AccountBalance=metricsToday.AccountBalanceEuro;
			if(metricsToday.Severity==eServiceSignalSeverity.Critical) {
				eServiceMetricsControl.StartFlashing(metricsToday.CriticalStatus);
			}
			else {
				eServiceMetricsControl.StopFlashing();
			}
			switch(metricsToday.Severity) {
				case eServiceSignalSeverity.Working:
					eServiceMetricsControl.AlertColor=Color.LimeGreen;
					break;
				case eServiceSignalSeverity.Warning:
					eServiceMetricsControl.AlertColor=Color.Yellow;
					break;
				case eServiceSignalSeverity.Error:
					eServiceMetricsControl.AlertColor=Color.Orange;
					break;
				case eServiceSignalSeverity.Critical:
					eServiceMetricsControl.AlertColor=Color.Red;
					break;
			}
		}

		///<summary>Refresh the phone panel every X seconds after it has already been setup.  Make sure to call FillMapAreaPanel before calling this the first time.</summary>
		public void SetPhoneList(List<PhoneEmpDefault> peds,List<Phone> phones,List<PhoneEmpSubGroup> listSubGroups,List<ChatUser> listChatUsers) {
			try {
				string title="Call Center Map - Triage Coord. - ";
				try { //get the triage coord label but don't fail just because we can't find it
					SiteLink siteLink=SiteLinks.GetFirstOrDefault(x => x.SiteNum==_mapCur.SiteNum);
					title+=Employees.GetNameFL(Employees.GetEmp(siteLink.EmployeeNum));
				}
				catch {
					title+="Not Set";
				}
				labelTriageCoordinator.Text=title;
				labelCurrentTime.Text=DateTime.Now.ToShortTimeString();
				#region Triage Counts
				//The triage count used to only count up the triage operators within the currently selected room.
				//Now we want to count all operators at the selected site (local) and then all operators across all sites (total).
				int triageStaffCountLocal=0;
				int triageStaffCountTotal=0;
				foreach(PhoneEmpDefault phoneEmpDefault in peds.FindAll(x => x.IsTriageOperator && x.HasColor)) {
					Phone phone=phones.FirstOrDefault(x => x.Extension==phoneEmpDefault.PhoneExt);
					if(phone==null) {
						continue;
					}
					if(phone.ClockStatus.In(ClockStatusEnum.None,ClockStatusEnum.Home,ClockStatusEnum.Lunch,ClockStatusEnum.Break,ClockStatusEnum.Off
						,ClockStatusEnum.Unavailable,ClockStatusEnum.NeedsHelp,ClockStatusEnum.HelpOnTheWay))
					{
						continue;
					}
					//This is a triage operator who is currently here and on the clock.
					if(phoneEmpDefault.SiteNum==_mapCur.SiteNum) {
						triageStaffCountLocal++;
					}
					triageStaffCountTotal++;
				}
				labelTriageOpsCountLocal.Text=triageStaffCountLocal.ToString();
				labelTriageOpsCountTotal.Text=triageStaffCountTotal.ToString();
				#endregion
				for(int i=0;i<this.mapAreaPanelHQ.Controls.Count;i++) { //loop through all of our cubicles and labels and find the matches
					try {
						if(!(this.mapAreaPanelHQ.Controls[i] is MapAreaRoomControl)) {
							continue;
						}
						MapAreaRoomControl room=(MapAreaRoomControl)this.mapAreaPanelHQ.Controls[i];
						if(room.MapAreaItem.Extension==0) { //This cubicle has not been given an extension yet.
							room.Empty=true;
							continue;
						}
						Phone phone=Phones.GetPhoneForExtension(phones,room.MapAreaItem.Extension);
						if(phone==null) {//We have a cubicle with no corresponding phone entry.
							room.Empty=true;
							continue;
						}
						room.PhoneCur=phone;//Refresh PhoneCur so that it has up to date customer information.
						ChatUser chatuser=listChatUsers.Where(x => x.Extension == phone.Extension).FirstOrDefault();
						PhoneEmpDefault phoneEmpDefault=PhoneEmpDefaults.GetEmpDefaultFromList(phone.EmployeeNum,peds);
						if(phoneEmpDefault==null) {//We have a cubicle with no corresponding phone emp default entry.
							room.Empty=true;
							continue;
						}
						//we got this far so we found a corresponding cubicle for this phone entry
						room.EmployeeNum=phone.EmployeeNum;
						room.EmployeeName=phone.EmployeeName;
						if(phone.DateTimeNeedsHelpStart.Date==DateTime.Today) { //if they need help, use that time.
							TimeSpan span=DateTime.Now-phone.DateTimeNeedsHelpStart+_timeDelta;
							DateTime timeOfDay=DateTime.Today+span;
							room.Elapsed=timeOfDay.ToString("H:mm:ss");
						}
						else if(phone.DateTimeStart.Date==DateTime.Today && phone.Description != "") { //else if in a call, use call time.
							TimeSpan span=DateTime.Now-phone.DateTimeStart+_timeDelta;
							DateTime timeOfDay=DateTime.Today+span;
							room.Elapsed=timeOfDay.ToString("H:mm:ss");
						}
						else if(phone.Description=="" && chatuser!=null && chatuser.CurrentSessions>0) { //else if in a chat, use chat time.
						  TimeSpan span=TimeSpan.FromMilliseconds(chatuser.SessionTime)+_timeDelta;
						  room.Elapsed=span.ToStringHmmss();
						}
						else if(phone.DateTimeStart.Date==DateTime.Today) { //else available, use that time.
						  TimeSpan span = DateTime.Now-phone.DateTimeStart+_timeDelta;
						  DateTime timeOfDay = DateTime.Today+span;
						  room.Elapsed=timeOfDay.ToString("H:mm:ss");
						}
						else { //else, whatever.
							room.Elapsed="";
						}
						if(phone.IsProxVisible) {
							room.ProxImage=Properties.Resources.Figure;
						}
						else if(phone.DateTProximal.AddHours(8)>DateTime.Now) {
							room.ProxImage=Properties.Resources.NoFigure;//TODO: replace image with one from Nathan
						}
						else {
							room.ProxImage=null;
						}
						room.IsAtDesk=phone.IsProxVisible;
						string status=Phones.ConvertClockStatusToString(phone.ClockStatus);
						//Check if the user is logged in.
						if(phone.ClockStatus==ClockStatusEnum.None
							||phone.ClockStatus==ClockStatusEnum.Home) {
							status="Home";
						}
						room.Status=status;
						if(phone.Description=="") {
							room.PhoneImage=null;
							if(chatuser!=null&&chatuser.CurrentSessions!=0) {
								room.ChatImage=Properties.Resources.gtaicon3;
							}
							else {
								room.ChatImage=null;
							}
						}
						else {
							room.PhoneImage=Properties.Resources.phoneInUse;
						}
						Color outerColor;
						Color innerColor;
						Color fontColor;
						bool isTriageOperatorOnTheClock;
						//get the cubicle color and triage status
						Phones.GetPhoneColor(phone,phoneEmpDefault,true,out outerColor,out innerColor,out fontColor,out isTriageOperatorOnTheClock);
						if(!room.IsFlashing) { //if the control is already flashing then don't overwrite the colors. this would cause a "spastic" flash effect.
							room.OuterColor=outerColor;
							room.InnerColor=innerColor;
						}
						room.ForeColor=fontColor;
						if(phone.ClockStatus==ClockStatusEnum.NeedsHelp) { //turn on flashing
							room.StartFlashing();
						}
						else { //turn off flashing
							room.StopFlashing();
						}
						room.Invalidate(true);
					}
					catch(Exception e) {
						e.DoNothing();
					}
				}
				refreshCurrentTabHelper(peds,phones,listSubGroups);
			}
			catch {
				//something failed unexpectedly
			}
		}

		private void tabMain_SelectedIndexChanged(object sender,EventArgs e) {
			refreshCurrentTabHelper(PhoneEmpDefaults.Refresh(),Phones.GetPhoneList(),PhoneEmpSubGroups.GetAll());
		}

		private void refreshCurrentTabHelper(List<PhoneEmpDefault> phoneEmpDefaultsIn,List<Phone> phonesIn,List<PhoneEmpSubGroup> subGroupsIn) {
			List<PhoneEmpSubGroup> subGroups=subGroupsIn.FindAll(x => x.SubGroupType==_curSubGroupType);
			//List of EmployeeNums to show for current tab.
			List<long> empNums=subGroups.Select(y => y.EmployeeNum).ToList();
			List<PhoneEmpDefault> phoneEmpDefaults=phoneEmpDefaultsIn.FindAll(x => empNums.Contains(x.EmployeeNum));//Employees who belong to this sub group.
			foreach(PhoneEmpDefault phoneEmp in phoneEmpDefaults) {
				phoneEmp.EscalationOrder=subGroups.First(x => x.EmployeeNum==phoneEmp.EmployeeNum).EscalationOrder;
			}
			SetEscalationList(phoneEmpDefaults,phonesIn);
		}

		private void SetEscalationList(List<PhoneEmpDefault> peds,List<Phone> phones) {
			try {
				escalationView.BeginUpdate();
				escalationView.Items.Clear();
				escalationView.DictProximity.Clear();
				escalationView.DictShowExtension.Clear();
				escalationView.DictExtensions.Clear();
				escalationView.DictColors.Clear();
				if(escalationView.Tag==null || (((int)escalationView.Tag)!=tabMain.SelectedIndex)) {
					escalationView.IsNewItems=true;
					escalationView.Tag=tabMain.SelectedIndex;
				}
				List<PhoneEmpDefault> listFiltered=peds.FindAll(x => DoAddToEscalationView(x,phones));
				List<PhoneEmpDefault> listSorted=SortForEscalationView(listFiltered,phones);
				for(int i=0;i<listSorted.Count;i++) {
					PhoneEmpDefault ped=listSorted[i];
					Phone phone=ODMethodsT.Coalesce(Phones.GetPhoneForEmployeeNum(phones,ped.EmployeeNum));
					escalationView.Items.Add(ped.EmpName);
					//Only show the proximity icon if the phone.IsProxVisible AND the employee is at the same site as our currently selected room.
					escalationView.DictProximity.Add(ped.EmpName,(_mapCur.SiteNum==ped.SiteNum && phone.IsProxVisible));
					//Extensions will always show for both locations unless the employee is not proximal.
					escalationView.DictShowExtension.Add(ped.EmpName,phone.IsProxVisible);
					escalationView.DictExtensions.Add(ped.EmpName,ped.PhoneExt);
					if(_mapCur.SiteNum!=ped.SiteNum) {
						escalationView.DictColors.Add(ped.EmpName,SiteLinks.GetSiteColorBySiteNum(ped.SiteNum,escalationView.BackColor));
					}
				}
			}
			catch {
			}
			finally {
				escalationView.EndUpdate();
			}
		}

		///<summary>Sorts the list of PhoneEmpDefaults in the appropriate way for the selected escalation view.</summary>
		private List<PhoneEmpDefault> SortForEscalationView(List<PhoneEmpDefault> peds,List<Phone> phones) {
			if(_curSubGroupType==PhoneEmpSubGroupType.Avail) {
				Func<PhoneEmpDefault,Phone> getPhone=new Func<PhoneEmpDefault,Phone>((phoneEmpDef) => {
					return ODMethodsT.Coalesce(Phones.GetPhoneForEmployeeNum(phones,phoneEmpDef.EmployeeNum));
				});
				return peds.OrderBy(x => getPhone(x).ClockStatus!=ClockStatusEnum.Available)//Show Available first
					.ThenBy(x => getPhone(x).ClockStatus!=ClockStatusEnum.Training)//Training next
					.ThenBy(x => getPhone(x).ClockStatus!=ClockStatusEnum.Backup)//Backup next
					.ThenBy(x => getPhone(x).DateTimeStart.Year < 1880)//Show people who have an actual DateTimeStart first
					.ThenBy(x => getPhone(x).DateTimeStart)//Show those first who have been in this status longest
					.ToList();
			}
			//All other escalation views beside Avail
			return peds.OrderBy(x => !IsAtCurrentLocation(x))//Show people at the selected location first
				.ThenBy(x => x.EscalationOrder)
				.ThenBy(x => x.EmpName).ToList();
		}

		///<summary>Returns true if the employee for the PhoneEmpDefault should be added to the selected escalation view.</summary>
		private bool DoAddToEscalationView(PhoneEmpDefault ped,List<Phone> phones) {
			if(ped.EscalationOrder<=0) { //Filter out employees that do not have an escalation order set.
				return false;
			}
			Phone phone=Phones.GetPhoneForEmployeeNum(phones,ped.EmployeeNum);
			if(phone==null || phone.Description!="") { //Filter out invalid employees or employees that are already on the phone.
				return false;
			}
			if(_curSubGroupType==PhoneEmpSubGroupType.Avail) {//Special rules for the Avail escalation view
				if(!phone.IsProxVisible) {
					return false;
				}
				if(!IsAtCurrentLocation(ped)) {
					return false;
				}
				if(!phone.ClockStatus.In(ClockStatusEnum.Available,ClockStatusEnum.Training,ClockStatusEnum.Backup)) {
					return false;
				}
				return true;
			}
			//All other escalation views besides Avail
			if(!phone.ClockStatus.In(ClockStatusEnum.Available,ClockStatusEnum.OfflineAssist,ClockStatusEnum.Backup))	{
				return false;
			}
			return true;
		}

		///<summary>Returns true if the employee for the PhoneEmpDefault is at the current location. An employee is considered to be at the same location
		///as a room if the employee's site is the same for the current room.</summary>
		private bool IsAtCurrentLocation(PhoneEmpDefault ped) {
			return ped.SiteNum==_mapCur.SiteNum;
		}

		public void SetOfficesDownList(List<Task> listOfficesDown) {
			try {
				officesDownView.BeginUpdate();
				officesDownView.Items.Clear();
				//Sort list by oldest.
				listOfficesDown.Sort(delegate(Task t1,Task t2) {
					return Comparer<DateTime>.Default.Compare(t1.DateTimeEntry,t2.DateTimeEntry);
				});
				for(int i=0;i<listOfficesDown.Count;i++) {
					Task task=listOfficesDown[i];
					if(task.TaskStatus==TaskStatusEnum.Done) { //Filter out old tasks. Should not be any but just in case.
						continue;
					}
					TimeSpan timeActive=DateTime.Now.Subtract(task.DateTimeEntry);
					//We got this far so the office is down.
					officesDownView.Items.Add(timeActive.ToStringHmmss()+" - "+task.KeyNum.ToString());
				}
			}
			catch {
			}
			finally {
				officesDownView.EndUpdate();
			}		
		}

		public void SetTriageUrgent(int calls,TimeSpan timeBehind) {
			this.labelTriageRedCalls.Text=calls.ToString();
			if(timeBehind==TimeSpan.Zero) { //format the string special for this case
				this.labelTriageRedTimeSpan.Text="00:00";
			}
			else {
				this.labelTriageRedTimeSpan.Text=timeBehind.ToStringmmss();
			}
			if(calls>_triageRedCalls) { //we are behind
				labelTriageRedCalls.SetAlertColors();
			}
			else { //we are ok
				labelTriageRedCalls.SetNormalColors();
			}
			if(timeBehind>TimeSpan.FromMinutes(_triageRedTime)) { //we are behind
				labelTriageRedTimeSpan.SetAlertColors();
			}
			else { //we are ok
				labelTriageRedTimeSpan.SetNormalColors();
			}
		}

		public void SetVoicemailRed(int calls,TimeSpan timeBehind) {
			this.labelVoicemailCalls.Text=calls.ToString();
			if(timeBehind==TimeSpan.Zero) { //format the string special for this case
				this.labelVoicemailTimeSpan.Text="00:00";
			}
			else {
				this.labelVoicemailTimeSpan.Text=timeBehind.ToStringmmss();
			}
			if(calls>_voicemailCalls) { //we are behind
				labelVoicemailCalls.SetAlertColors();
			}
			else { //we are ok
				labelVoicemailCalls.SetNormalColors();
			}
			if(timeBehind>TimeSpan.FromMinutes(_voicemailTime)) { //we are behind
				labelVoicemailTimeSpan.SetAlertColors();
			}
			else { //we are ok
				labelVoicemailTimeSpan.SetNormalColors();
			}
		}
		
		///<summary>Sets the time for current triage tasks and colors it according to how far behind we are.</summary>
		public void SetTriageNormal(int callsWithNotes,int callsWithNoNotes,TimeSpan timeBehind,int triageRed) {
			if(timeBehind==TimeSpan.Zero) { //format the string special for this case
				this.labelTriageTimeSpan.Text="0";
			}
			else {
				this.labelTriageTimeSpan.Text=((int)timeBehind.TotalMinutes).ToString();			
			}
			if(callsWithNoNotes>0 || triageRed>0) { //we have calls which don't have notes or a red triage task so display that number
				this.labelTriageCalls.Text=(callsWithNoNotes+triageRed).ToString();
			}
			else { //we don't have any calls with no notes nor any red triage tasks so display count of total tasks
				this.labelTriageCalls.Text="("+callsWithNotes.ToString()+")";
			}
			if(callsWithNoNotes+triageRed>_triageCalls) { //we are behind
				labelTriageCalls.SetAlertColors();
			}
			else { //we are ok
				labelTriageCalls.SetNormalColors();
			}
			if(timeBehind>TimeSpan.FromMinutes(_triageTime)) { //we are behind
				labelTriageTimeSpan.SetAlertColors();
			}
			else if(timeBehind>TimeSpan.FromMinutes(_triageTimeWarning)) { //we are approaching being behind
				labelTriageTimeSpan.SetWarnColors();
			}
			else { //we are ok
				labelTriageTimeSpan.SetNormalColors();
			}
		}

		#endregion Set label text and colors

		private void comboRoom_SelectionChangeCommitted(object sender,EventArgs e) {
			_mapCur=_listMaps[comboRoom.SelectedIndex];
			FillMapAreaPanel();
			FillTriageLabelColors();
		}

		private void fullScreenToolStripMenuItem_Click(object sender,EventArgs e) {
			_isFullScreen=!_isFullScreen;
			if(_isFullScreen) { //switch to full screen
				this.fullScreenToolStripMenuItem.Text="Restore";
				this.setupToolStripMenuItem.Visible=false;
				this.WindowState=FormWindowState.Normal;
				this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.None;
				this.Bounds=System.Windows.Forms.Screen.FromControl(this).Bounds;
				this.mapAreaPanelHQ.PixelsPerFoot=18;
			}
			else { //set back to defaults
				this.fullScreenToolStripMenuItem.Text="Full Screen";
				FormMapHQ FormCMS=new FormMapHQ();
				this.FormBorderStyle=FormCMS.FormBorderStyle;
				this.Size=FormCMS.Size;
				this.CenterToScreen();
				this.setupToolStripMenuItem.Visible=true;
				this.mapAreaPanelHQ.PixelsPerFoot=17;
			}
		}

		private void mapToolStripMenuItem_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormMapSetup FormMS=new FormMapSetup();
			FormMS.ShowDialog();
			if(FormMS.DialogResult!=DialogResult.OK) {
				return;
			}
			_listMaps=FormMS.ListMaps;
			_mapCur=_listMaps[comboRoom.SelectedIndex];
			FillCombo();
			FillMapAreaPanel();
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"MapHQ layout changed");
		}

		private void escalationToolStripMenuItem_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormPhoneEmpDefaultEscalationEdit FormE=new FormPhoneEmpDefaultEscalationEdit();
			FormE.ShowDialog();
			refreshCurrentTabHelper(PhoneEmpDefaults.Refresh(),Phones.GetPhoneList(),PhoneEmpSubGroups.GetAll());
			SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Escalation team changed");
		}
		
		///<summary>Starts the timer that will refresh the panel so that ugly lines don't stay on the panel when scrolling.
		///The timer prevents lag due to repainting every time the scroll bar moves.</summary>
		private void mapAreaPanelHQ_Scroll(object sender,EventArgs e) {
			timer1.Stop();
			timer1.Start();
		}

		private void timer1_Tick(object sender,EventArgs e) {
			mapAreaPanelHQ.Refresh();
			timer1.Stop();
		}
		
		private void tabMain_DrawItem(object sender,DrawItemEventArgs e) {
			Graphics g=e.Graphics;
			Brush _textBrush=new SolidBrush(Color.Black);
			TabPage _tabPage=tabMain.TabPages[e.Index];
			Rectangle _tabBounds=tabMain.GetTabRect(e.Index);
			if(e.State==DrawItemState.Selected) {
				g.FillRectangle(Brushes.White,e.Bounds);
			}
			else {
				g.FillRectangle(Brushes.LightGray,e.Bounds);
			}
			// Draw string. Center the text.
			StringFormat _stringFlags=new StringFormat();
			_stringFlags.Alignment=StringAlignment.Center;
			_stringFlags.LineAlignment=StringAlignment.Center;
			g.DrawString(_tabPage.Text,tabMain.Font,_textBrush,_tabBounds,new StringFormat(_stringFlags));
		}

		private void toggleTriageToolStripMenuItem_Click(object sender,EventArgs e) {
			splitContainer2.Panel1Collapsed=!splitContainer2.Panel1Collapsed;
		}

		private void openNewMapToolStripMenuItem_Click(object sender,EventArgs e) {
			//Open the map from FormOpenDental so that it can register for the click events.
			ExtraMapClicked?.Invoke(this,e);
			//Form gets added to the list of all HQMaps in Load method.
		}

		private void FormMapHQ_FormClosed(object sender,FormClosedEventArgs e) {
			FormOpenDental.RemoveMapFromList(this);
		}

		private void confRoomsToolStripMenuItem_Click(object sender,EventArgs e) {
			FormPhoneConfs FormPC=new FormPhoneConfs();
			FormPC.ShowDialog();//ShowDialog because we do not want this window to be floating open for long periods of time.
		}

		private void phonesToolStripMenuItem_Click(object sender,EventArgs e) {
			FormPhoneEmpDefaults formPED=new FormPhoneEmpDefaults();
			formPED.ShowDialog();
		}

		private void MapAreaRoomControl_GoToChanged(object sender,EventArgs e) {
			GoToChanged?.Invoke(sender,new EventArgs());
		}

		///<summary>If the phone map has been changed, update all phone maps.</summary>
		public override void OnProcessSignals(List<Signalod> listSignals) {
			if(listSignals.Exists(x => x.IType==InvalidType.PhoneMap)) {
				FillMapAreaPanel();
			}
		}
	}
}
