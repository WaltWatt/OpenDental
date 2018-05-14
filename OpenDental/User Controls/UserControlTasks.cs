using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Linq;
using CodeBase;

namespace OpenDental {
	public partial class UserControlTasks:UserControl {
		[Category("Action"),Description("Fires towards the end of the FillGrid method.")]
		public event FillGridEventHandler FillGridEvent;
		///<summary>List of all TastLists that are to be displayed in the main window. Combine with TasksList.</summary>
		private List<TaskList> _listTaskLists;
		///<summary>List of all Tasks that are to be displayed in the main window.  Combine with TaskListsList.</summary>
		private List<Task> _listTasks;
		//<Summary>Only used if viewing user tab.  This is a list of all task lists in the general tab.  It is used to generate full path heirarchy info for each task list in the user tab.</Summary>
		//private List<TaskList> TaskListsAllGeneral;
		///<summary>An arraylist of TaskLists beginning from the trunk and adding on branches.  If the count is 0, then we are in the trunk of one of the five categories.  The last TaskList in the TreeHistory is the one that is open in the main window.</summary>
		private List<TaskList> _listTaskListTreeHistory;
		///<summary>A TaskList that is on the 'clipboard' waiting to be pasted.  Will be null if nothing has been copied yet.</summary>
		private TaskList _clipTaskList;
		///<summary>A Task that is on the 'clipboard' waiting to be pasted.  Will be null if nothing has been copied yet.</summary>
		private Task _clipTask;
		///<summary>If there is an item on our 'clipboard', this tracks whether it was cut.</summary>
		private bool _wasCut;
		///<summary>The index of the last clicked item in the main list.</summary>
		private int _clickedI;
		///<summary>After closing, if this is not zero, then it will jump to the object specified in GotoKeyNum.</summary>
		public TaskObjectType GotoType;
		///<summary>After closing, if this is not zero, then it will jump to the specified patient.</summary>
		public long GotoKeyNum;
		///<summary>All notes for the showing tasks, ordered by date time.</summary>
		private List<TaskNote> _listTaskNotes;
		///<summary>A friendly string that could be used as the title of any window that has this control on it.
		///It will contain the description of the currently selected task list and a count of all new tasks within that list.</summary>
		public string ControlParentTitle;
		private const int _TriageListNum=1697;
		private bool _isTaskSortApptDateTime;//Use task AptDateTime sort setup in FormTaskOptions.
		private bool _isShowFinishedTasks=false;//Show finished task setup in FormTaskOptions.
		private DateTime _dateTimeStartShowFinished=DateTimeOD.Today.AddDays(-7);//Show finished task date setup in FormTaskOptions.
		///<summary>Keeps track of which tasks are expanded.  Persists between TaskList list updates.</summary>
		private List<long> _listExpandedTaskNums=new List<long>();
		private bool _isCollapsedByDefault;
		private bool _hasListSwitched;
		///<summary>This can be three states: 0 for all tasks expanded, 1 for all tasks collapsed, and -1 for mixed.</summary>
		private int _taskCollapsedState;
		///<summary>When a task is selected via right click, we make a shallow copy of the task so menu options are performed on the correct task.</summary>
		private Task _clickedTask;
		///<summary>The states of patients from previously seen tasks.</summary>
		private Dictionary<long,string> _dictPatStates=new Dictionary<long, string>();

		public UserControlTasks() {
			InitializeComponent();
			//this.listMain.ContextMenu = this.menuEdit;
			//Lan.F(this);
			Lan.C(this,menuEdit);
			gridMain.ContextMenu=menuEdit;
		}

		///<summary>The parent might call this if it gets a signal that a relevant task was added from another workstation.  The parent should only call this if it has been verified that there is a change to tasks.</summary>
		public void RefreshTasks(){
			Logger.LogAction("UserControlTasks.RefreshTasks",LogPath.Signals,() => FillGrid());
		}

		///<summary>And resets the tabs if the user changes.</summary>
		public void InitializeOnStartup(){
			if(Security.CurUser==null) {
				return;
			}
			tabUser.Text=Lan.g(this,"for ")+Security.CurUser.UserName;
			tabNew.Text=Lan.g(this,"New for ")+Security.CurUser.UserName;
			if(PrefC.GetBool(PrefName.TasksShowOpenTickets)) {
				if(!tabContr.TabPages.Contains(tabOpenTickets)) {
					tabContr.TabPages.Insert(2,tabOpenTickets);
				}
			}
			else{
				if(tabContr.TabPages.Contains(tabOpenTickets)) {
					tabContr.TabPages.Remove(tabOpenTickets);
				}
			}
			LayoutToolBar();
			if(PrefC.GetBool(PrefName.TasksUseRepeating)) {
				if(!tabContr.TabPages.Contains(tabRepeating)) {
					tabContr.TabPages.Add(tabRepeating);
					tabContr.TabPages.Add(tabDate);
					tabContr.TabPages.Add(tabWeek);
					tabContr.TabPages.Add(tabMonth);
				}
			}
			else {//Repeating tasks disabled.
				if(tabContr.TabPages.Contains(tabRepeating)) {
					tabContr.TabPages.Remove(tabRepeating);
					tabContr.TabPages.Remove(tabDate);
					tabContr.TabPages.Remove(tabWeek);
					tabContr.TabPages.Remove(tabMonth);
				}
			}
			if(Tasks.LastOpenList==null) {//first time openning
				_listTaskListTreeHistory=new List<TaskList>();
				cal.SelectionStart=DateTimeOD.Today;
			}
			else {//reopening
				if(Tasks.LastOpenGroup >= tabContr.TabPages.Count) {
					//This happens if the user changes the TasksUseRepeating from true to false, then refreshes the tasks.
					Tasks.LastOpenGroup=0;
				}
				tabContr.SelectedIndex=Tasks.LastOpenGroup;
				_listTaskListTreeHistory=new List<TaskList>();
				//for(int i=0;i<Tasks.LastOpenList.Count;i++) {
				//	TreeHistory.Add(((TaskList)Tasks.LastOpenList[i]).Copy());
				//}
				cal.SelectionStart=Tasks.LastOpenDate;
			}
			if(PrefC.IsODHQ) {
				menuNavJob.Visible=true;
				menuNavJob.Enabled=false;
				if(Security.IsAuthorized(Permissions.TaskEdit,true)) {
					menuDeleteTaken.Visible=true;
				}
			}
			_isTaskSortApptDateTime=PrefC.GetBool(PrefName.TaskSortApptDateTime);//This sets it for use and also for the task options default value.
			List<UserOdPref> listPrefsForCollapsing=UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum,UserOdFkeyType.TaskCollapse);
			_isCollapsedByDefault=listPrefsForCollapsing.Count==0 ? false : PIn.Bool(listPrefsForCollapsing[0].ValueString);
			_hasListSwitched=true;
			_taskCollapsedState=_isCollapsedByDefault ? 1 : 0;
			FillTree();
			FillGrid();
			if(tabContr.SelectedTab!=tabOpenTickets) {//because it will have alread been set
				SetOpenTicketTab(-1);
			}
			SetPatientTicketTab(-1);
			SetMenusEnabled();
		}

		public UserControlTasksTab TaskTab {
			get {
				if(tabContr.SelectedTab==tabUser) {
					return UserControlTasksTab.ForUser;
				}
				else if(tabContr.SelectedTab==tabNew) {
					return UserControlTasksTab.UserNew;
				}
				else if(tabContr.SelectedTab==tabOpenTickets) {
					return UserControlTasksTab.OpenTickets;
				}
				else if(tabContr.SelectedTab==tabPatientTickets) {
					return UserControlTasksTab.PatientTickets;
				}
				else if(tabContr.SelectedTab==tabMain) {
					return UserControlTasksTab.Main;
				}
				else if(tabContr.SelectedTab==tabReminders) {
					return UserControlTasksTab.Reminders;
				}
				else if(tabContr.SelectedTab==tabRepeating) {
					return UserControlTasksTab.RepeatingSetup;
				}
				else if(tabContr.SelectedTab==tabDate) {
					return UserControlTasksTab.RepeatingByDate;
				}
				else if(tabContr.SelectedTab==tabWeek) {
					return UserControlTasksTab.RepeatingByWeek;
				}
				else if(tabContr.SelectedTab==tabMonth) {
					return UserControlTasksTab.RepeatingByMonth;
				}
				return UserControlTasksTab.Invalid;//Default.  Should not happen.
			}
			set {
				TabPage tabOld=tabContr.SelectedTab;
				if(value==UserControlTasksTab.ForUser) {
					tabContr.SelectedTab=tabUser;
				}
				else if(value==UserControlTasksTab.UserNew) {
					tabContr.SelectedTab=tabNew;
				}
				else if(value==UserControlTasksTab.OpenTickets && PrefC.GetBool(PrefName.TasksShowOpenTickets)) {
					tabContr.SelectedTab=tabOpenTickets;
				}
				else if(value==UserControlTasksTab.Main) {
					tabContr.SelectedTab=tabMain;
				}
				else if(value==UserControlTasksTab.Reminders) {
					tabContr.SelectedTab=tabReminders;
				}
				else if(value==UserControlTasksTab.RepeatingSetup && PrefC.GetBool(PrefName.TasksUseRepeating)) {
					tabContr.SelectedTab=tabRepeating;
				}
				else if(value==UserControlTasksTab.RepeatingByDate && PrefC.GetBool(PrefName.TasksUseRepeating)) {
					tabContr.SelectedTab=tabDate;
				}
				else if(value==UserControlTasksTab.RepeatingByWeek && PrefC.GetBool(PrefName.TasksUseRepeating)) {
					tabContr.SelectedTab=tabWeek;
				}
				else if(value==UserControlTasksTab.RepeatingByMonth && PrefC.GetBool(PrefName.TasksUseRepeating)) {
					tabContr.SelectedTab=tabMonth;
				}
				else if(value==UserControlTasksTab.PatientTickets) {
					tabContr.SelectedTab=tabPatientTickets;
				}
				else if(value==UserControlTasksTab.Invalid) {
					//Do nothing.
				}
				if(tabContr.SelectedTab!=tabOld) {//Tab changed, refresh the tree.
					_listTaskListTreeHistory=new List<TaskList>();//clear the tree no matter which tab selected.
					FillTree();
					FillGrid();
				}
			}
		}

		///<summary>Called whenever OpenTicket tab is refreshed to set the count at the top.  Also called from InitializeOnStartup.  In that case, we don't know what the count should be, so we pass in a -1.</summary>
		private void SetOpenTicketTab(int countSet) {
			if(!tabContr.TabPages.Contains(tabOpenTickets)) {
				return;
			}
			if(countSet==-1) {
				countSet=Tasks.GetCountOpenTickets(Security.CurUser.UserNum);
			}
			tabOpenTickets.Text=Lan.g(this,"Open Tasks")+" ("+countSet.ToString()+")";
		}

		///<summary>Called whenever PatientTickets tab is refreshed to set the count at the top.  Also called from InitializeOnStartup.  In that case, we don't know what the count should be, so we pass in a -1.</summary>
		private void SetPatientTicketTab(int countSet) {
			if(!tabContr.TabPages.Contains(tabPatientTickets)) {
				return;
			}
			if(countSet==-1 && FormOpenDental.CurPatNum!=0) {
				countSet=Tasks.GetCountPatientTickets(FormOpenDental.CurPatNum);
			}
			tabPatientTickets.Text=Lan.g(this,"Patient Tasks")+" ("+(countSet==-1?"0":countSet.ToString())+")";
		}

		public void ClearLogOff() {
			tabUser.Text="for";
			tabNew.Text="New for";
			_listTaskListTreeHistory=new List<TaskList>();
			FillTree();
			gridMain.Rows.Clear();
			gridMain.Invalidate();
		}

		///<summary>Used by OD HQ.</summary>
		public void FillGridWithTriageList() {
			menuItemAddCommlog.Visible=true;
			TaskList tlOne=TaskLists.GetOne(_TriageListNum);
			tabContr.SelectedTab=tabMain;
			if(_listTaskListTreeHistory==null) {
				_listTaskListTreeHistory=new List<TaskList>();
			}
			_listTaskListTreeHistory.Clear();
			_listTaskListTreeHistory.Add(tlOne);
			if(_listTaskLists==null) {
				_listTaskLists=new List<TaskList>();
			}
			_listTaskLists.Clear();
			if(_listTasks==null) {
				_listTasks=new List<Task>();
			}
			_listTasks.Clear();
			_listTasks=Tasks.RefreshChildren(_TriageListNum,false,cal.SelectionStart,Security.CurUser.UserNum,0,TaskType.All);
			FillTree();
			FillGrid();
			gridMain.Focus();//Allow immediate mouse wheel scroll when loading triage list, no click required
		}

		private void UserControlTasks_Load(object sender,System.EventArgs e) {
			if(this.DesignMode){
				return;
			}
			if(!PrefC.GetBool(PrefName.TaskAncestorsAllSetInVersion55)) {
				if(!MsgBox.Show(this,true,"A one-time routine needs to be run.  It will take a few minutes.  Do you have time right now?")){
					return;
				}
				Cursor=Cursors.WaitCursor;
				TaskAncestors.SynchAll();
				Prefs.UpdateBool(PrefName.TaskAncestorsAllSetInVersion55,true);
				DataValid.SetInvalid(InvalidType.Prefs);
				Cursor=Cursors.Default;
			}
		}

		///<summary></summary>
		public void LayoutToolBar() {
			ToolBarMain.Buttons.Clear();
			ODToolBarButton butOptions=new ODToolBarButton();
			butOptions.Text=Lan.g(this,"Options");
			butOptions.ToolTipText=Lan.g(this,"Set session specific task options.");
			butOptions.Tag="Options";
			ToolBarMain.Buttons.Add(butOptions);
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Add Task List"),0,"","AddList"));
			ODToolBarButton butTask=new ODToolBarButton(Lan.g(this,"Add Task"),1,"","AddTask");
			butTask.Style=ODToolBarButtonStyle.DropDownButton;
			butTask.DropDownMenu=menuTask;
			ToolBarMain.Buttons.Add(butTask);
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Search"),-1,"","Search"));
			ODToolBarButton button=new ODToolBarButton();
			button.Text=Lan.g(this,"Manage Blocks");
			button.ToolTipText=Lan.g(this,"Manage which task lists will have popups blocked even when subscribed.");
			button.Tag="BlockSubsc";
			button.Pushed=Security.CurUser.DefaultHidePopups;
			ToolBarMain.Buttons.Add(button);
			ToolBarMain.Invalidate();
		}

		private void FillTree() {
			tree.Nodes.Clear();
			TreeNode node;
			//TreeNode lastNode=null;
			string nodedesc;
			for(int i=0;i<_listTaskListTreeHistory.Count;i++) {
				nodedesc=_listTaskListTreeHistory[i].Descript;
				if(tabContr.SelectedTab==tabUser) {
					nodedesc=_listTaskListTreeHistory[i].ParentDesc+nodedesc;
				}
				node=new TreeNode(nodedesc);
				node.Tag=_listTaskListTreeHistory[i].TaskListNum;
				if(tree.SelectedNode==null) {
					tree.Nodes.Add(node);
				}
				else {
					tree.SelectedNode.Nodes.Add(node);
				}
				tree.SelectedNode=node;
			}
			//remember this position for the next time we open tasks
			Tasks.LastOpenList=new ArrayList();
			for(int i=0;i<_listTaskListTreeHistory.Count;i++) {
				Tasks.LastOpenList.Add(_listTaskListTreeHistory[i].Copy());
			}
			Tasks.LastOpenGroup=tabContr.SelectedIndex;
			Tasks.LastOpenDate=cal.SelectionStart;
			//layout
			if(tabContr.SelectedTab==tabDate || tabContr.SelectedTab==tabWeek || tabContr.SelectedTab==tabMonth) {
				tree.Top=cal.Bottom+1;//Show the calendar.
			}
			else {
				tree.Top=tabContr.Bottom;//Hide the calendar.
			}
			tree.Height=_listTaskListTreeHistory.Count*tree.ItemHeight+8;
			tree.Refresh();
			gridMain.Top=tree.Bottom;
		}

		public void RefreshPatTicketsIfNeeded() {
			if(TaskTab==UserControlTasksTab.PatientTickets) {
				FillGrid();
			}
			else {
				SetPatientTicketTab(-1);
			}
		}

		private void FillGrid(){
			if(Security.CurUser==null 
				|| (RemotingClient.RemotingRole==RemotingRole.ClientWeb && !Security.IsUserLoggedIn)) 
			{
				gridMain.BeginUpdate();
				gridMain.Rows.Clear();
				gridMain.EndUpdate();
				return;
			}
			long parent;
			DateTime date;
			if(_listTaskListTreeHistory==null){
				return;
			}
			if(_listTaskListTreeHistory.Count>0) {//not on main trunk
				parent=_listTaskListTreeHistory[_listTaskListTreeHistory.Count-1].TaskListNum;
				date=DateTime.MinValue;
			}
			else {//one of the main trunks
				parent=0;
				date=cal.SelectionStart;
			}
			gridMain.Height=this.ClientSize.Height-gridMain.Top-3;
			RefreshMainLists(parent,date);
			#region dated trunk automation
			//dated trunk automation-----------------------------------------------------------------------------
			if(_listTaskListTreeHistory.Count==0//main trunk
				&& (tabContr.SelectedTab==tabDate || tabContr.SelectedTab==tabWeek || tabContr.SelectedTab==tabMonth))
			{
				//clear any lists which are derived from a repeating list and which do not have any itmes checked off
				bool changeMade=false;
				for(int i=0;i<_listTaskLists.Count;i++) {
					if(_listTaskLists[i].FromNum==0) {//ignore because not derived from a repeating list
						continue;
					}
					if(!AnyAreMarkedComplete(_listTaskLists[i])) {
						DeleteEntireList(_listTaskLists[i]);
						changeMade=true;
					}
				}
				//clear any tasks which are derived from a repeating task 
				//and which are still new (not marked viewed or done)
				for(int i=0;i<_listTasks.Count;i++) {
					if(_listTasks[i].FromNum==0) {
						continue;
					}
					if(_listTasks[i].TaskStatus==TaskStatusEnum.New) {
						Tasks.Delete(_listTasks[i].TaskNum);
						SecurityLogs.MakeLogEntry(Permissions.TaskEdit,0,"Task "+POut.Long(_listTasks[i].TaskNum)+" deleted",0);
						changeMade=true;
					}
				}
				if(changeMade) {
					RefreshMainLists(parent,date);
				}
				//now add back any repeating lists and tasks that meet the criteria
				//Get lists of all repeating lists and tasks of one type.  We will pick items from these two lists.
				List<TaskList> repeatingLists=new List<TaskList>();
				List<Task> repeatingTasks=new List<Task>();
				if(tabContr.SelectedTab==tabDate){
					repeatingLists=TaskLists.RefreshRepeating(TaskDateType.Day);
					repeatingTasks=Tasks.RefreshRepeating(TaskDateType.Day,Security.CurUser.UserNum);
				}
				if(tabContr.SelectedTab==tabWeek){
					repeatingLists=TaskLists.RefreshRepeating(TaskDateType.Week);
					repeatingTasks=Tasks.RefreshRepeating(TaskDateType.Week,Security.CurUser.UserNum);
				}
				if(tabContr.SelectedTab==tabMonth) {
					repeatingLists=TaskLists.RefreshRepeating(TaskDateType.Month);
					repeatingTasks=Tasks.RefreshRepeating(TaskDateType.Month,Security.CurUser.UserNum);
				}
				//loop through list and add back any that meet criteria.
				changeMade=false;
				bool alreadyExists;
				for(int i=0;i<repeatingLists.Count;i++) {
					//if already exists, skip
					alreadyExists=false;
					for(int j=0;j<_listTaskLists.Count;j++) {//loop through Main list
						if(_listTaskLists[j].FromNum==repeatingLists[i].TaskListNum) {
							alreadyExists=true;
							break;
						}
					}
					if(alreadyExists) {
						continue;
					}
					//otherwise, duplicate the list
					repeatingLists[i].DateTL=date;
					repeatingLists[i].FromNum=repeatingLists[i].TaskListNum;
					repeatingLists[i].IsRepeating=false;
					repeatingLists[i].Parent=0;
					repeatingLists[i].ObjectType=0;//user will have to set explicitly
					DuplicateExistingList(repeatingLists[i],true);//repeating lists cannot be subscribed to, so send null in as old list, will not attempt to move subscriptions
					changeMade=true;
				}
				for(int i=0;i<repeatingTasks.Count;i++) {
					//if already exists, skip
					alreadyExists=false;
					for(int j=0;j<_listTasks.Count;j++) {//loop through Main list
						if(_listTasks[j].FromNum==repeatingTasks[i].TaskNum) {
							alreadyExists=true;
							break;
						}
					}
					if(alreadyExists) {
						continue;
					}
					//otherwise, duplicate the task
					repeatingTasks[i].DateTask=date;
					repeatingTasks[i].FromNum=repeatingTasks[i].TaskNum;
					repeatingTasks[i].IsRepeating=false;
					repeatingTasks[i].TaskListNum=0;
					//repeatingTasks[i].UserNum//repeating tasks shouldn't get a usernum
					Tasks.Insert(repeatingTasks[i]);
					changeMade=true;
				}
				if(changeMade) {
					RefreshMainLists(parent,date);
				}
			}//End of dated trunk automation--------------------------------------------------------------------------
			#endregion dated trunk automation
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn("",17);
			col.ImageList=imageListTree;
			gridMain.Columns.Add(col);//Checkbox column
			if(tabContr.SelectedTab==tabNew && !PrefC.GetBool(PrefName.TasksNewTrackedByUser)) {//The old way
				col=new ODGridColumn(Lan.g("TableTasks","Read"),35,HorizontalAlignment.Center);
				//col.ImageList=imageListTree;
				gridMain.Columns.Add(col);
			}
			if(tabContr.SelectedTab==tabNew || tabContr.SelectedTab==tabOpenTickets || tabContr.SelectedTab==tabPatientTickets) {
				col=new ODGridColumn(Lan.g("TableTasks","Task List"),90);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g(this,"+/-"),17,HorizontalAlignment.Center);
			col.CustomClickEvent+=GridHeaderClickEvent;
			gridMain.Columns.Add(col);
			if(PrefC.GetBool(PrefName.DockPhonePanelShow)){//HQ
				col=new ODGridColumn(Lan.g("TableTasks","ST"),30,HorizontalAlignment.Center);//ST
				gridMain.Columns.Add(col);
				List<long> listPatsNotInDict=_listTasks.Where(x => x.ObjectType==TaskObjectType.Patient && x.KeyNum!=0 && !_dictPatStates.ContainsKey(x.KeyNum))
					.Select(x => x.KeyNum).ToList();
				Dictionary<long,string> dictPatNewStates=Patients.GetStatesForPats(listPatsNotInDict);
				foreach(long patNum in dictPatNewStates.Keys) {
					_dictPatStates.Add(patNum,dictPatNewStates[patNum]);
				}
				if(parent!=_TriageListNum) {//Everything that's not triage
					col=new ODGridColumn(Lan.g("TableTasks","Job"),30,HorizontalAlignment.Center);//Job
					gridMain.Columns.Add(col);
				}
			}
			col=new ODGridColumn(Lan.g("TableTasks","Description"),200);//any width
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			List<JobLink> _listJobLinks;
			List<Job> _listJobs;
			string dateStr="";
			string objDesc="";
			string tasklistdescript="";
			string notes="";
			//These strings are always inserted into cells, so they are always set to "" if there no job
			string jobNumString="";
			int imageindex;
			for(int i=0;i<_listTaskLists.Count;i++) {
				dateStr="";
				if(_listTaskLists[i].DateTL.Year>1880
					&& (tabContr.SelectedTab==tabUser || tabContr.SelectedTab==tabMain || tabContr.SelectedTab==tabReminders))
				{
					if(_listTaskLists[i].DateType==TaskDateType.Day) {
						dateStr=_listTaskLists[i].DateTL.ToShortDateString()+" - ";
					}
					else if(_listTaskLists[i].DateType==TaskDateType.Week) {
						dateStr=Lan.g(this,"Week of")+" "+_listTaskLists[i].DateTL.ToShortDateString()+" - ";
					}
					else if(_listTaskLists[i].DateType==TaskDateType.Month) {
						dateStr=_listTaskLists[i].DateTL.ToString("MMMM")+" - ";
					}
				}
				objDesc="";
				if(tabContr.SelectedTab==tabUser){
					objDesc=_listTaskLists[i].ParentDesc;
				}
				tasklistdescript=_listTaskLists[i].Descript;
				imageindex=0;
				if(_listTaskLists[i].NewTaskCount>0){
					imageindex=3;//orange
					tasklistdescript=tasklistdescript+"("+_listTaskLists[i].NewTaskCount.ToString()+")";
				}
				row=new ODGridRow();
				row.Cells.Add(imageindex.ToString());
				row.Cells.Add("");
				if(PrefC.GetBool(PrefName.DockPhonePanelShow)) {//HQ.  Add if job manager is available
					row.Cells.Add("");//ST
					if(parent!=_TriageListNum) {//Everything that's not triage
						row.Cells.Add("");//Job
					}
				}
				row.Cells.Add(dateStr+objDesc+tasklistdescript);
				gridMain.Rows.Add(row);
			}
			List<long> listAptNums=_listTasks.Where(x => x.ObjectType==TaskObjectType.Appointment).Select(y => y.KeyNum).ToList();
			SerializableDictionary<long,string> dictApptObjDescripts=Tasks.GetApptObjDescripts(listAptNums);
			for(int i=0;i<_listTasks.Count;i++) {
				dateStr="";
				jobNumString="";
				string stateString="";
				if(PrefC.GetBool(PrefName.DockPhonePanelShow)) {//HQ
					stateString=HQStateColumn(_listTasks[i]);//ST
					if(parent!=_TriageListNum) {//Everything that's not triage
						//get list of jobs attached to task then insert info about those jobs.
						_listJobLinks=JobLinks.GetForTask(_listTasks[i].TaskNum);
						_listJobs=Jobs.GetMany(_listJobLinks.Select(x => x.JobNum).ToList());
						if(_listJobs.Count > 0) {
							jobNumString="X";//Is job
						}
					}
				}
				if(tabContr.SelectedTab==tabUser || tabContr.SelectedTab==tabNew
					|| tabContr.SelectedTab==tabOpenTickets || tabContr.SelectedTab==tabMain 
					|| tabContr.SelectedTab==tabReminders	|| tabContr.SelectedTab==tabPatientTickets) 
				{
					if(_listTasks[i].DateTask.Year>1880) {
						if(_listTasks[i].DateType==TaskDateType.Day) {
							dateStr+=_listTasks[i].DateTask.ToShortDateString()+" - ";
						}
						else if(_listTasks[i].DateType==TaskDateType.Week) {
							dateStr+=Lan.g(this,"Week of")+" "+_listTasks[i].DateTask.ToShortDateString()+" - ";
						}
						else if(_listTasks[i].DateType==TaskDateType.Month) {
							dateStr+=_listTasks[i].DateTask.ToString("MMMM")+" - ";
						}
					}
					else if(_listTasks[i].DateTimeEntry.Year>1880) {
						dateStr+=_listTasks[i].DateTimeEntry.ToShortDateString()+" "+_listTasks[i].DateTimeEntry.ToShortTimeString()+" - ";
					}
				}
				objDesc="";
				if(_listTasks[i].TaskStatus==TaskStatusEnum.Done){
					objDesc=Lan.g(this,"Done:")+_listTasks[i].DateTimeFinished.ToShortDateString()+" - ";
				}
				if(_listTasks[i].ObjectType==TaskObjectType.Patient) {
					if(_listTasks[i].KeyNum!=0) {
						objDesc+=_listTasks[i].PatientName+" - ";
					}
				}
				else if(_listTasks[i].ObjectType==TaskObjectType.Appointment) {
					if(_listTasks[i].KeyNum!=0) {
						dictApptObjDescripts.TryGetValue(_listTasks[i].KeyNum,out objDesc);
					}
				}
				if(!_listTasks[i].Descript.StartsWith("==") && _listTasks[i].UserNum!=0) {
					objDesc+=Userods.GetName(_listTasks[i].UserNum)+" - ";
				}
				notes="";
				List<TaskNote> listNotesForTask=_listTaskNotes.FindAll(x => x.TaskNum==_listTasks[i].TaskNum);
				if(!_listExpandedTaskNums.Contains(_listTasks[i].TaskNum) && listNotesForTask.Count>1) {
					TaskNote lastNote=listNotesForTask[listNotesForTask.Count-1];
					notes+="\r\n\u22EE\r\n" //Vertical ellipse followed by last note. \u22EE - vertical ellipses
							+"=="+Userods.GetName(lastNote.UserNum)+" - "
							+lastNote.DateTimeNote.ToShortDateString()+" "
							+lastNote.DateTimeNote.ToShortTimeString()
							+" - "+lastNote.Note;
				}
				else { 
					foreach(TaskNote note in listNotesForTask) {
						notes+="\r\n"//even on the first loop
							+"=="+Userods.GetName(note.UserNum)+" - "
							+note.DateTimeNote.ToShortDateString()+" "
							+note.DateTimeNote.ToShortTimeString()
							+" - "+note.Note;
					}
				}
				row=new ODGridRow();
				if(PrefC.GetBool(PrefName.TasksNewTrackedByUser)) {//The new way
					if(_listTasks[i].TaskStatus==TaskStatusEnum.Done) {
						row.Cells.Add("1");
					}
					else {
						if(_listTasks[i].IsUnread) {
							row.Cells.Add("4");
						}
						else{
							row.Cells.Add("2");
						}
					}
				}
				else {
					switch(_listTasks[i].TaskStatus) {
						case TaskStatusEnum.New:
							row.Cells.Add("4");
							break;
						case TaskStatusEnum.Viewed:
							row.Cells.Add("2");
							break;
						case TaskStatusEnum.Done:
							row.Cells.Add("1");
							break;
					}
					if(tabContr.SelectedTab==tabNew) {//In this mode, there's a extra column in this tab
						row.Cells.Add("read");
					}
				}
				if(tabContr.SelectedTab==tabNew || tabContr.SelectedTab==tabOpenTickets || tabContr.SelectedTab==tabPatientTickets) {
					row.Cells.Add(_listTasks[i].ParentDesc);
				}
				if(_listExpandedTaskNums.Contains(_listTasks[i].TaskNum)) {
					if(_listTasks[i].Descript.Length>250 || listNotesForTask.Count>1 || (listNotesForTask.Count==1 && notes.Length>250)) {
						row.Cells.Add("-");
					}
					else {
						row.Cells.Add("");
					}
					if(PrefC.GetBool(PrefName.DockPhonePanelShow)) {//HQ
						row.Cells.Add(stateString);//ST
						if(parent!=_TriageListNum) {//Everything that's not triage
							row.Cells.Add(jobNumString);//Job
						}
					}
					row.Cells.Add(dateStr+objDesc+_listTasks[i].Descript+notes);
				}
				else {
					//Conditions for giving collapse option: Descript is long, there is more than one note, or there is one note and it's long.
					if(_listTasks[i].Descript.Length>250 || listNotesForTask.Count>1 || (listNotesForTask.Count==1 && notes.Length>250)) {
						row.Cells.Add("+");
						if(PrefC.GetBool(PrefName.DockPhonePanelShow)) {//HQ
							row.Cells.Add(stateString);//ST
							if(parent!=_TriageListNum) {//Everything that's not triage
								row.Cells.Add(jobNumString);//Job
							}
						}
						string rowString=dateStr+objDesc;
						if(_listTasks[i].Descript.Length>250) {
							rowString+=_listTasks[i].Descript.Substring(0,250)+"(...)";//546,300 tasks have average Descript length of 142.1 characters.
						}
						else {
							rowString+=_listTasks[i].Descript;
						}
						if(notes.Length>250) {
							rowString+=notes.Substring(0,250)+"(...)";
						}
						else {
							rowString+=notes;
						}
						row.Cells.Add(rowString);
					}
					else {//Descript length <= 250 and notes <=1 and note length is <= 250.  No collapse option.
						row.Cells.Add("");
						if(PrefC.GetBool(PrefName.DockPhonePanelShow)) {//HQ
							row.Cells.Add(stateString);//ST
							if(parent!=_TriageListNum) {//Everything that's not triage
								row.Cells.Add(jobNumString);//Job
							}
						}
						row.Cells.Add(dateStr+objDesc+_listTasks[i].Descript+notes);
					}
				}
				row.ColorBackG=Defs.GetColor(DefCat.TaskPriorities,_listTasks[i].PriorityDefNum);//No need to do any text detection for triage priorities, we'll just use the task priority colors.
				row.Tag=_listTasks[i];
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			gridMain.ScrollValue=gridMain.ScrollValue;//this forces scroll value to reset if it's > allowed max.
			if(tabContr.SelectedTab==tabOpenTickets) {
				SetOpenTicketTab(gridMain.Rows.Count);
			}
			if(tabContr.SelectedTab==tabPatientTickets) {
				SetPatientTicketTab(gridMain.Rows.Count);
			}
			else {
				SetPatientTicketTab(-1);
			}
			SetControlTitleHelper();
		}

		///<summary>Helper used to fill ST column for HQ.
		///Only call after determining if HQ.</summary>
		private string HQStateColumn(Task task) {
			long patNum=(task.ObjectType==TaskObjectType.Patient?task.KeyNum:0);
			if(_dictPatStates.ContainsKey(patNum)) {
				return _dictPatStates[patNum];
			}
			else {
				return "";
			}
		}

		///<summary>Click event for GridMain's collapse/expand column header.</summary>
		private void GridHeaderClickEvent(object sender,EventArgs e) {
			if(_taskCollapsedState==-1) {//Mixed mode
				_taskCollapsedState=_isCollapsedByDefault ? 1 : 0;
				FillGrid();//Re-do the grid with whatever their default mode is.
				return; 
			}
			if(_taskCollapsedState==0) {//All are NOT collapsed. Make them all collapsed.
				_taskCollapsedState=1;
				FillGrid();
				return;
			}
			if(_taskCollapsedState==1) {//All ARE collapsed.  Make them all NOT collapsed.
				_taskCollapsedState=0;
				FillGrid();
				return;
			}
		}

		///<summary>Updates ControlParentTitle to give more information about the currently selected task list.  Currently only called in FillGrid()</summary>
		private void SetControlTitleHelper() {
			if(FillGridEvent==null){//Delegate has not been assigned, so we do not care.
				return;
			}
			string taskListDescript="";
			if(tabContr.SelectedTab==tabNew) {//Special case tab. All grid rows are guaranteed to be task so we manually set values.
				taskListDescript=Lan.g(this,"New for")+" "+Security.CurUser.UserName;
			}
			else if(_listTaskListTreeHistory.Count>0){//Not in main trunk
				taskListDescript=_listTaskListTreeHistory[_listTaskListTreeHistory.Count-1].Descript;
			}
			if(taskListDescript=="") {//Should only happen when at main trunk.
				ControlParentTitle=Lan.g(this,"Tasks");
			}
			else {
				int tasksNewCount=_listTaskLists.Sum(x => x.NewTaskCount);
				tasksNewCount+=_listTasks.Sum(x => x.TaskStatus==TaskStatusEnum.New?1:0);
				ControlParentTitle=Lan.g(this,"Tasks")+" - "+taskListDescript+" ("+tasksNewCount.ToString()+")";
			}
			FillGridEvent.Invoke(this,new EventArgs());
		}

		///<summary>A recursive function that checks every child in a list IsFromRepeating.  If any are marked complete, then it returns true, signifying that this list should be immune from being deleted since it's already in use.</summary>
		private bool AnyAreMarkedComplete(TaskList list) {
			//get all children:
			List<TaskList> childLists=TaskLists.RefreshChildren(list.TaskListNum,Security.CurUser.UserNum,0,TaskType.Normal);
			List<Task> childTasks=Tasks.RefreshChildren(list.TaskListNum,true,DateTime.MinValue,Security.CurUser.UserNum,0,TaskType.Normal);
			for(int i=0;i<childLists.Count;i++) {
				if(AnyAreMarkedComplete(childLists[i])) {
					return true;
				}
			}
			for(int i=0;i<childTasks.Count;i++) {
				if(childTasks[i].TaskStatus==TaskStatusEnum.Done) {
					return true;
				}
			}
			return false;
		}

		///<summary>If parent=0, then this is a trunk.</summary>
		private void RefreshMainLists(long parent,DateTime date) {
			if(this.DesignMode){
				_listTaskLists=new List<TaskList>();
				_listTasks=new List<Task>();
				_listTaskNotes=new List<TaskNote>();
				return;
			}
			TaskType taskType=TaskType.Normal;
			if(tabContr.SelectedTab==tabReminders) {
				taskType=TaskType.Reminder;
			}
			if(parent!=0){//not a trunk
				//if(TreeHistory.Count>0//we already know this is true
				long userNumInbox=TaskLists.GetMailboxUserNum(_listTaskListTreeHistory[0].TaskListNum);
				_listTaskLists=TaskLists.RefreshChildren(parent,Security.CurUser.UserNum,userNumInbox,taskType);
				_listTasks=Tasks.RefreshChildren(parent,_isShowFinishedTasks,_dateTimeStartShowFinished,Security.CurUser.UserNum,userNumInbox,taskType,
					_isTaskSortApptDateTime);
			}
			else if(tabContr.SelectedTab==tabUser) {
				_listTaskLists=TaskLists.RefreshUserTrunk(Security.CurUser.UserNum);
				_listTasks=new List<Task>();//no tasks in the user trunk
			}
			else if(tabContr.SelectedTab==tabNew) {
				_listTaskLists=new List<TaskList>();//no task lists in new tab
				_listTasks=Tasks.RefreshUserNew(Security.CurUser.UserNum);
			}
			else if(tabContr.SelectedTab==tabOpenTickets) {
				_listTaskLists=new List<TaskList>();//no task lists in new tab
				_listTasks=Tasks.RefreshOpenTickets(Security.CurUser.UserNum);
			}
			else if(tabContr.SelectedTab==tabPatientTickets) {
				_listTaskLists=new List<TaskList>();
				_listTasks=new List<Task>();
				if(FormOpenDental.CurPatNum!=0) {
					_listTasks=Tasks.RefreshPatientTickets(FormOpenDental.CurPatNum);
				}
			}
			else if(tabContr.SelectedTab==tabMain) {
				_listTaskLists=TaskLists.RefreshMainTrunk(Security.CurUser.UserNum,TaskType.Normal);
				_listTasks=Tasks.RefreshMainTrunk(_isShowFinishedTasks,_dateTimeStartShowFinished,Security.CurUser.UserNum,TaskType.Normal);
			}
			else if(tabContr.SelectedTab==tabReminders) {
				_listTaskLists=TaskLists.RefreshMainTrunk(Security.CurUser.UserNum,TaskType.Reminder);
				_listTasks=Tasks.RefreshMainTrunk(_isShowFinishedTasks,_dateTimeStartShowFinished,Security.CurUser.UserNum,TaskType.Reminder);
			}
			else if(tabContr.SelectedTab==tabRepeating) {
				_listTaskLists=TaskLists.RefreshRepeatingTrunk();
				_listTasks=Tasks.RefreshRepeatingTrunk();
			}
			else if(tabContr.SelectedTab==tabDate) {
				_listTaskLists=TaskLists.RefreshDatedTrunk(date,TaskDateType.Day);
				_listTasks=Tasks.RefreshDatedTrunk(date,TaskDateType.Day,_isShowFinishedTasks,_dateTimeStartShowFinished,Security.CurUser.UserNum);
			}
			else if(tabContr.SelectedTab==tabWeek) {
				_listTaskLists=TaskLists.RefreshDatedTrunk(date,TaskDateType.Week);
				_listTasks=Tasks.RefreshDatedTrunk(date,TaskDateType.Week,_isShowFinishedTasks,_dateTimeStartShowFinished,Security.CurUser.UserNum);
			}
			else if(tabContr.SelectedTab==tabMonth) {
				_listTaskLists=TaskLists.RefreshDatedTrunk(date,TaskDateType.Month);
				_listTasks=Tasks.RefreshDatedTrunk(date,TaskDateType.Month,_isShowFinishedTasks,_dateTimeStartShowFinished,Security.CurUser.UserNum);
			}
			//notes
			List<long> taskNums=new List<long>();
			for(int i=0;i<_listTasks.Count;i++) {
				taskNums.Add(_listTasks[i].TaskNum);
			}
			if(_hasListSwitched) {
				if(_isCollapsedByDefault) {
					_listExpandedTaskNums.Clear();
				}
				else {
					_listExpandedTaskNums.AddRange(taskNums);
				}
				_hasListSwitched=false;
			}
			else {
				if(_taskCollapsedState==1) {//Header was clicked, make all collapsed
					_listExpandedTaskNums.Clear();				
				}
				else if(_taskCollapsedState==0) {//Header was clicked, make all expanded
					_listExpandedTaskNums.AddRange(taskNums);
				}
				else { 
					for(int i=_listExpandedTaskNums.Count-1;i>=0;i--) {
						if(!taskNums.Contains(_listExpandedTaskNums[i])) {
							_listExpandedTaskNums.Remove(_listExpandedTaskNums[i]);//The Task was removed from the visual list, don't keep it around in the expanded list.
						}
					}
				}
			}
			_listTaskNotes=TaskNotes.RefreshForTasks(taskNums);
		}
		
		private void tabContr_MouseDown(object sender,MouseEventArgs e) {
			_listTaskListTreeHistory=new List<TaskList>();//clear the tree no matter which tab clicked.
			_hasListSwitched=true;
			FillTree();
			FillGrid();
			//Allows mouse wheel scroll without having to click in grid.  Helpful on 'Main' as it is populated with task lists, which drill down on single click.
			gridMain.Focus();
		}

		private void cal_DateSelected(object sender,System.Windows.Forms.DateRangeEventArgs e) {
			_listTaskListTreeHistory=new List<TaskList>();//clear the tree
			FillTree();
			FillGrid();
		}

		private void ToolBarMain_ButtonClick(object sender,OpenDental.UI.ODToolBarButtonClickEventArgs e) {
			//if(e.Button.Tag.GetType()==typeof(string)){
			//standard predefined button
			switch(e.Button.Tag.ToString()) {
				case "Options":
					Options_Clicked();
					break;
				case "AddList":
					AddList_Clicked();
					break;
				case "AddTask":
					AddTask_Clicked();
					break;
				case "Search":
					Search_Clicked();
					break;
				case "BlockSubsc":
					BlockSubsc_Clicked();
					break;
			}
		}
	
		private void Options_Clicked() {
			FormTaskOptions FormTO = new FormTaskOptions(_isShowFinishedTasks,_dateTimeStartShowFinished,_isTaskSortApptDateTime);
			FormTO.StartPosition=FormStartPosition.Manual;//Allows us to set starting form starting Location.
			Point pointFormLocation=this.PointToScreen(ToolBarMain.Location);//Since we cant get ToolBarMain.Buttons["Options"] location directly.
			pointFormLocation.X+=ToolBarMain.Buttons["Options"].Bounds.Width;//Add Options button width so by default form opens along side button.
			Rectangle screenDim=SystemInformation.VirtualScreen;//Dimensions of users screen. Includes if user has more then 1 screen.
			if(pointFormLocation.X+FormTO.Width > screenDim.Width) {//Not all of form will be on screen, so adjust.
				pointFormLocation.X=screenDim.Width-FormTO.Width-5;//5 for some padding.
			}
			if(pointFormLocation.Y+FormTO.Height > screenDim.Height) {//Not all of form will be on screen, so adjust.
				pointFormLocation.Y=screenDim.Height-FormTO.Height-5;//5 for some padding.
			}
			FormTO.Location=pointFormLocation;
			FormTO.ShowDialog();
			_isShowFinishedTasks=FormTO.IsShowFinishedTasks;
			_dateTimeStartShowFinished=FormTO.DateTimeStartShowFinished;
			_isTaskSortApptDateTime=FormTO.IsSortApptDateTime;
			_isCollapsedByDefault=PIn.Bool(UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum,UserOdFkeyType.TaskCollapse)[0].ValueString);
			_hasListSwitched=true;//To display tasks in correctly collapsed/expanded state
			FillGrid();
		}

		private void AddList_Clicked() {
			if(!Security.IsAuthorized(Permissions.TaskListCreate,false)) {
				return;
			}
			if(tabContr.SelectedTab==tabUser && _listTaskListTreeHistory.Count==0) {//trunk of user tab
				MsgBox.Show(this,"Not allowed to add a task list to the trunk of the user tab.  Either use the subscription feature, or add it to a child list.");
				return;
			}
			if(tabContr.SelectedTab==tabNew) {//new tab
				MsgBox.Show(this,"Not allowed to add items to the 'New' tab.");
				return;
			}
			if(tabContr.SelectedTab==tabPatientTickets) {
				MsgBox.Show(this,"Not allowed to add a task list to the 'Patient Tasks' tab.");
				return;
			}
			TaskList cur=new TaskList();
			//if this is a child of any other taskList
			if(_listTaskListTreeHistory.Count>0) {
				cur.Parent=_listTaskListTreeHistory[_listTaskListTreeHistory.Count-1].TaskListNum;
			}
			else {
				cur.Parent=0;
				if(tabContr.SelectedTab==tabDate) {
					cur.DateTL=cal.SelectionStart;
					cur.DateType=TaskDateType.Day;
				}
				else if(tabContr.SelectedTab==tabWeek) {
					cur.DateTL=cal.SelectionStart;
					cur.DateType=TaskDateType.Week;
				}
				else if(tabContr.SelectedTab==tabMonth) {
					cur.DateTL=cal.SelectionStart;
					cur.DateType=TaskDateType.Month;
				}
			}
			if(tabContr.SelectedTab==tabRepeating) {
				cur.IsRepeating=true;
			}
			FormTaskListEdit FormT=new FormTaskListEdit(cur);
			FormT.IsNew=true;
			FormT.ShowDialog();
			FillGrid();
		}

		private void AddTask(bool isReminder) {
			if(Plugins.HookMethod(this,"UserControlTasks.AddTask_Clicked")) {
				return;
			}
			//if(tabContr.SelectedTab==tabUser && TreeHistory.Count==0) {//trunk of user tab
			//	MsgBox.Show(this,"Not allowed to add a task to the trunk of the user tab.  Add it to a child list instead.");
			//	return;
			//}
			//if(tabContr.SelectedTab==tabNew) {//new tab
			//	MsgBox.Show(this,"Not allowed to add items to the 'New' tab.");
			//	return;
			//}
			Task task=new Task();
			task.TaskListNum=-1;//don't show it in any list yet.
			Tasks.Insert(task);
			Task taskOld=task.Copy();
			//if this is a child of any taskList
			if(_listTaskListTreeHistory.Count>0) {
				task.TaskListNum=_listTaskListTreeHistory[_listTaskListTreeHistory.Count-1].TaskListNum;
			}
			else if(tabContr.SelectedTab==tabNew) {//new tab
				task.TaskListNum=-1;//Force FormTaskEdit to ask user to pick a task list.
			}
			else if(tabContr.SelectedTab==tabUser && _listTaskListTreeHistory.Count==0) {//trunk of user tab
				task.TaskListNum=-1;//Force FormTaskEdit to ask user to pick a task list.
			}
			else {
				task.TaskListNum=0;
				if(tabContr.SelectedTab==tabDate) {
					task.DateTask=cal.SelectionStart;
					task.DateType=TaskDateType.Day;
				}
				else if(tabContr.SelectedTab==tabWeek) {
					task.DateTask=cal.SelectionStart;
					task.DateType=TaskDateType.Week;
				}
				else if(tabContr.SelectedTab==tabMonth) {
					task.DateTask=cal.SelectionStart;
					task.DateType=TaskDateType.Month;
				}
			}
			if(tabContr.SelectedTab==tabRepeating) {
				task.IsRepeating=true;
			}
			task.UserNum=Security.CurUser.UserNum;
			if(isReminder) {
				task.ReminderType=TaskReminderType.Once;
			}
			FormTaskEdit FormT=new FormTaskEdit(task,taskOld);
			FormT.IsNew=true;
			FormT.Closing+=new CancelEventHandler(TaskGoToEvent);
			FormT.Show();//non-modal
		}

		private void AddTask_Clicked() {
			bool isReminder=false;
			if(tabContr.SelectedTab==tabReminders) {
				isReminder=true;
			}
			AddTask(isReminder);
		}

		private void menuItemTaskReminder_Click(object sender,EventArgs e) {
			AddTask(true);
		}

		public void Search_Clicked() {
			FormTaskSearch FormTS=new FormTaskSearch();
			FormTS.Show();
		}

		public void TaskGoToEvent(object sender,CancelEventArgs e) {
			FormTaskEdit FormT=(FormTaskEdit)sender;
			if(FormT.GotoType!=TaskObjectType.None) {
				GotoType=FormT.GotoType;
				GotoKeyNum=FormT.GotoKeyNum;
				FormOpenDental.S_TaskGoTo(GotoType,GotoKeyNum);
			}
			if(!this.IsDisposed) {
				FillGrid();
			}
		}

		private void BlockSubsc_Clicked() {
			FormTaskListBlocks FormTLB = new FormTaskListBlocks();
			FormTLB.ShowDialog();
			if(FormTLB.DialogResult==DialogResult.OK) {
				DataValid.SetInvalid(InvalidType.Security);
			}
		}

		private void Done_Clicked() {
			//already blocked if list
			Task task=_clickedTask;
			Task oldTask=task.Copy();
			task.TaskStatus=TaskStatusEnum.Done;
			if(task.DateTimeFinished.Year<1880) {
				task.DateTimeFinished=DateTime.Now;
			}
			try {
				Tasks.Update(task,oldTask);
			}
			catch(Exception ex) {
				//We manipulated the TaskStatus and need to set it back to what it was because something went wrong.
				int idx=_listTasks.FindIndex(x => x.TaskNum==oldTask.TaskNum);
				if(idx>-1) {
					_listTasks[idx]=oldTask;
				}
				MessageBox.Show(ex.Message);
				return;
			}
			TaskUnreads.DeleteForTask(task.TaskNum);
			TaskHist taskHist=new TaskHist(oldTask);
			taskHist.UserNumHist=Security.CurUser.UserNum;
			TaskHists.Insert(taskHist);
			DataValid.SetInvalidTask(task.TaskNum,false);//this causes an immediate local refresh of the grid
		}

		private void Edit_Clicked() {
			if(_clickedI < _listTaskLists.Count) {//is list
				FormTaskListEdit FormT=new FormTaskListEdit(_listTaskLists[_clickedI]);
				FormT.ShowDialog();
				FillGrid();
			}
			else {//task
				FormTaskEdit FormT=new FormTaskEdit(_clickedTask,_clickedTask.Copy());
				FormT.Show();//non-modal
			}
		}

		private void Cut_Clicked() {
			if(_clickedI < _listTaskLists.Count) {//is list
				_clipTaskList=_listTaskLists[_clickedI].Copy();
				_clipTask=null;
			}
			else {//task
				_clipTaskList=null;
				_clipTask=_clickedTask.Copy();
			}
			_wasCut=true;
		}

		private void Copy_Clicked() {
			if(_clickedI < _listTaskLists.Count) {//is list
				_clipTaskList=_listTaskLists[_clickedI].Copy();
				_clipTask=null;
			}
			else {//task
				_clipTaskList=null;
				_clipTask=_clickedTask.Copy();
				if(!String.IsNullOrEmpty(_clipTask.ReminderGroupId)) {
					//Any reminder tasks duplicated must have a brand new ReminderGroupId
					//so that they do not affect the original reminder task chain.
					Tasks.SetReminderGroupId(_clipTask);
				}
			}
			_wasCut=false;
		}

		///<summary>When copying and pasting, Task hist will be lost because the pasted task has a new TaskNum.</summary>
		private void Paste_Clicked() {
			if(_clipTaskList!=null) {//a taskList is on the clipboard
				if(!_wasCut) {
					return;//Tasklists are no longer allowed to be copied, only cut.  Code should never make it this far.
				}
				TaskList newTL=_clipTaskList.Copy();
				if(_listTaskListTreeHistory.Count>0) {//not on main trunk
					newTL.Parent=_listTaskListTreeHistory[_listTaskListTreeHistory.Count-1].TaskListNum;
					if(tabContr.SelectedTab==tabUser){
						//treat pasting just like it's the main tab, because not on the trunk.
					}
					else if(tabContr.SelectedTab==tabMain){
						//even though usually only trunks are dated, we will leave the date alone in main
						//category since user may wish to preserve it. All other children get date cleared.
					}
					else if(tabContr.SelectedTab==tabReminders) {
						//treat pasting just like it's the main tab.
					}
					else if(tabContr.SelectedTab==tabRepeating){
						newTL.DateTL=DateTime.MinValue;//never a date
						//leave dateType alone, since that affects how it repeats
					}
					else if(tabContr.SelectedTab==tabDate
						|| tabContr.SelectedTab==tabWeek
						|| tabContr.SelectedTab==tabMonth) 
					{
						newTL.DateTL=DateTime.MinValue;//children do not get dated
						newTL.DateType=TaskDateType.None;//this doesn't matter either for children
					}
				}
				else {//one of the main trunks
					newTL.Parent=0;
					if(tabContr.SelectedTab==tabUser) {
						//maybe we should treat this like a subscription rather than a paste.  Implement later.  For now:
						MsgBox.Show(this,"Not allowed to paste directly to the trunk of this tab.  Try using the subscription feature instead.");
						return;
					}
					else if(tabContr.SelectedTab==tabMain) {
						newTL.DateTL=DateTime.MinValue;
						newTL.DateType=TaskDateType.None;
					}
					else if(tabContr.SelectedTab==tabReminders) {
						newTL.DateTL=DateTime.MinValue;
						newTL.DateType=TaskDateType.None;
					}
					else if(tabContr.SelectedTab==tabRepeating) {
						newTL.DateTL=DateTime.MinValue;//never a date
						//newTL.DateType=TaskDateType.None;//leave alone
					}
					else if(tabContr.SelectedTab==tabDate){
						newTL.DateTL=cal.SelectionStart;
						newTL.DateType=TaskDateType.Day;
					}
					else if(tabContr.SelectedTab==tabWeek) {
						newTL.DateTL=cal.SelectionStart;
						newTL.DateType=TaskDateType.Week;
					}
					else if(tabContr.SelectedTab==tabMonth) {
						newTL.DateTL=cal.SelectionStart;
						newTL.DateType=TaskDateType.Month;
					}
				}
				if(tabContr.SelectedTab==tabRepeating) {
					newTL.IsRepeating=true;
				}
				else {
					newTL.IsRepeating=false;
				}
				newTL.FromNum=0;//always
				if(_clipTaskList.TaskListNum==newTL.Parent && _wasCut) {
					MsgBox.Show(this,"Cannot cut and paste a task list into itself.  Please move it into a different task list.");
					return;
				}
				if(TaskLists.IsAncestor(_clipTaskList.TaskListNum,newTL.Parent)) {
					//The user is attempting to cut or copy a TaskList into one of its ancestors.  We don't want to do normal movement logic for this case.
					//We move the TaskList desired to have its parent to the list they desire.  
					//We change the TaskList's direct children to have the parent of the TaskList being moved.
					MoveListIntoAncestor(newTL,_clipTaskList.Parent);
				}
				else {
					if(tabContr.SelectedTab==tabUser || tabContr.SelectedTab==tabMain || tabContr.SelectedTab==tabReminders) {
						MoveTaskList(newTL,true);
					}
					else {
						MoveTaskList(newTL,false);
					}
				}
				DataValid.SetInvalid(InvalidType.Task);
			}
			if(_clipTask!=null) {//a task is on the clipboard
				Task newT=_clipTask.Copy();
				if(_listTaskListTreeHistory.Count>0) {//not on main trunk
					newT.TaskListNum=_listTaskListTreeHistory[_listTaskListTreeHistory.Count-1].TaskListNum;
					if(tabContr.SelectedTab==tabUser) {
						//treat pasting just like it's the main tab, because not on the trunk.
					}
					else if(tabContr.SelectedTab==tabMain) {
						//even though usually only trunks are dated, we will leave the date alone in main
						//category since user may wish to preserve it. All other children get date cleared.
					}
					else if(tabContr.SelectedTab==tabReminders) {
						//treat pasting just like it's the main tab.
					}
					else if(tabContr.SelectedTab==tabRepeating) {
						newT.DateTask=DateTime.MinValue;//never a date
						//leave dateType alone, since that affects how it repeats
					}
					else if(tabContr.SelectedTab==tabDate
						|| tabContr.SelectedTab==tabWeek
						|| tabContr.SelectedTab==tabMonth) 
					{
						newT.DateTask=DateTime.MinValue;//children do not get dated
						newT.DateType=TaskDateType.None;//this doesn't matter either for children
					}
				}
				else {//one of the main trunks
					newT.TaskListNum=0;
					if(tabContr.SelectedTab==tabUser) {
						//never allowed to have a task on the user trunk.
						MsgBox.Show(this,"Tasks may not be pasted directly to the trunk of this tab.  Try pasting within a list instead.");
						return;
					}
					else if(tabContr.SelectedTab==tabMain) {
						newT.DateTask=DateTime.MinValue;
						newT.DateType=TaskDateType.None;
					}
					else if(tabContr.SelectedTab==tabReminders) {
						newT.DateTask=DateTime.MinValue;
						newT.DateType=TaskDateType.None;
					}
					else if(tabContr.SelectedTab==tabRepeating) {
						newT.DateTask=DateTime.MinValue;//never a date
						//newTL.DateType=TaskDateType.None;//leave alone
					}
					else if(tabContr.SelectedTab==tabDate) {
						newT.DateTask=cal.SelectionStart;
						newT.DateType=TaskDateType.Day;
					}
					else if(tabContr.SelectedTab==tabWeek) {
						newT.DateTask=cal.SelectionStart;
						newT.DateType=TaskDateType.Week;
					}
					else if(tabContr.SelectedTab==tabMonth) {
						newT.DateTask=cal.SelectionStart;
						newT.DateType=TaskDateType.Month;
					}
				}
				if(tabContr.SelectedTab==tabRepeating) {
					newT.IsRepeating=true;
				}
				else {
					newT.IsRepeating=false;
				}
				newT.FromNum=0;//always
				if(!String.IsNullOrEmpty(newT.ReminderGroupId)) {
					//Any reminder tasks duplicated to another task list must have a brand new ReminderGroupId
					//so that they do not affect the original reminder task chain.
					Tasks.SetReminderGroupId(newT);
				}
				if(_wasCut && Tasks.WasTaskAltered(_clipTask)){
					MsgBox.Show("Tasks","Not allowed to move because the task has been altered by someone else.");
					FillGrid();
					return;
				}
				string histDescript="";
				if(_wasCut) { //cut
					histDescript="This task was cut from task list "+TaskLists.GetFullPath(_clipTask.TaskListNum)+" and pasted into "+TaskLists.GetFullPath(newT.TaskListNum);
					Tasks.Update(newT,_clipTask);
				}
				else { //copied
					List<TaskNote> noteList=TaskNotes.GetForTask(newT.TaskNum);
					Tasks.Insert(newT);//Creates a new PK for newT
					histDescript="This task was copied from task "+_clipTask.TaskNum+" in task list "+TaskLists.GetFullPath(_clipTask.TaskListNum);
					for(int t=0;t<noteList.Count;t++) {
						noteList[t].TaskNum=newT.TaskNum;
						TaskNotes.Insert(noteList[t]);//Creates the new note with the current datetime stamp.
						TaskNotes.Update(noteList[t]);//Restores the historical datetime for the note.
					}
				}
				TaskHist hist=new TaskHist(newT);
				hist.Descript=histDescript;
				hist.UserNum=Security.CurUser.UserNum;
				TaskHists.Insert(hist);
				DataValid.SetInvalidTask(newT.TaskNum,true);
			}
			if(_wasCut && _clipTask!=null) {
				DataValid.SetInvalidTask(_clipTask.TaskNum,false);//this causes an immediate local refresh of the grid
			}
			//Turn the cut into a copy once the users has pasted at least once.
			_wasCut=false;
		}

		private void SendToMe_Clicked() {
			if(Security.CurUser.TaskListInBox==0) {
				MsgBox.Show(this,"You do not have an inbox.");
				return;
			}
			Task task=_clickedTask;
			Task oldTask=task.Copy();
			task.TaskListNum=Security.CurUser.TaskListInBox;
			Cursor=Cursors.WaitCursor;
			try {
				Tasks.Update(task,oldTask);
				//At HQ the refresh interval wasn't quick enough for the task to pop up.
				//We will immediately show the task instead of waiting for the refresh interval.
				TaskHist taskHist=new TaskHist(oldTask);
				taskHist.UserNumHist=Security.CurUser.UserNum;
				TaskHists.Insert(taskHist);
				DataValid.SetInvalidTask(task.TaskNum,false);
				Cursor=Cursors.Default;
				FormTaskEdit FormT=new FormTaskEdit(task,task.Copy());
				FormT.IsPopup=true;
				FormT.Show();//non-modal
			}
			catch(Exception ex) {
				Cursor=Cursors.Default;
				MessageBox.Show(ex.Message);
				task.TaskListNum=oldTask.TaskListNum;
				return;
			}
		}

		private void SendToMeCommlog_Clicked() {
			//This option is only selectable if the task clicked is a patient task.
			if(Security.CurUser.TaskListInBox==0) {
				MsgBox.Show(this,"You do not have an inbox.");
				return;
			}
			Task task=_clickedTask;
			Task oldTask=task.Copy();
			task.TaskListNum=Security.CurUser.TaskListInBox;
			Cursor=Cursors.WaitCursor;
			try {
				Tasks.Update(task,oldTask);
				TaskHist taskHist=new TaskHist(oldTask);
				taskHist.UserNumHist=Security.CurUser.UserNum;
				TaskHists.Insert(taskHist);
				DataValid.SetInvalidTask(task.TaskNum, false);
			}
			catch(Exception e) {
				Cursor=Cursors.Default;
				MsgBox.Show(this, e.Message);
				task.TaskListNum=oldTask.TaskListNum;
				return;
			}
			Cursor=Cursors.Default;
			//Add new note to task
			TaskNote TaskNoteCur=new TaskNote();
			TaskNoteCur.TaskNum=task.TaskNum;
			TaskNoteCur.DateTimeNote=DateTime.Now;
			TaskNoteCur.UserNum=Security.CurUser.UserNum;
			TaskNoteCur.IsNew=true;
			TaskNoteCur.Note="Returned Call";
			TaskNotes.Insert(TaskNoteCur);
			Tasks.TaskEditCreateLog(Permissions.TaskNoteEdit,Lan.g(this,"Automatically added task note")+": Returned Call",Tasks.GetOne(TaskNoteCur.TaskNum));
			//Load patient into chart module
			Goto_Clicked();
			//Create new commlog from task
			Commlog commlog=new Commlog();
			commlog.PatNum=task.KeyNum;	//tied to either patient.PatNum or appointment.AptNum
			commlog.CommDateTime=DateTime.Now;
			commlog.CommType=Commlogs.GetTypeAuto(CommItemTypeAuto.MISC);
			if(PrefC.GetBool(PrefName.DistributorKey)) {
				commlog.Mode_=CommItemMode.None;
				commlog.SentOrReceived=CommSentOrReceived.Neither;
			}
			else {
				commlog.Mode_=CommItemMode.Phone;
				commlog.SentOrReceived=CommSentOrReceived.Received;
			}
			commlog.UserNum=Security.CurUser.UserNum;
			commlog.Note="Returned call RE: "+task.Descript;
			//Push new commlog to screen
			FormCommItem FormCI=new FormCommItem();
			FormCI.ShowDialog(new CommItemModel() { CommlogCur=commlog },new CommItemController(FormCI) { IsNew=true });
		}

		private void Goto_Clicked() {
			//not even allowed to get to this point unless a valid task
			Task task=_clickedTask;
			GotoType=task.ObjectType;
			GotoKeyNum=task.KeyNum;
			FormOpenDental.S_TaskGoTo(GotoType,GotoKeyNum);
		}

		private void MoveListIntoAncestor(TaskList newList,long oldListParent) {
			if(_wasCut) {//If the TaskList was cut, move direct children of the list "up" one in the hierarchy and then update
				List<TaskList> childLists=TaskLists.RefreshChildren(newList.TaskListNum,Security.CurUser.UserNum,0,TaskType.All);
				for(int i=0;i<childLists.Count;i++) {
					childLists[i].Parent=oldListParent;
					TaskLists.Update(childLists[i]);
				}
				TaskLists.Update(newList);
			}
			else {//Just insert a new TaskList if it was copied.
				TaskLists.Insert(newList);
			}
		}

		///<summary>Assign new parent FKey for existing tasklist, and update TaskAncestors.  Used when cutting and pasting a tasklist.
		///Does not create new task or tasklist entries.</summary>
		private void MoveTaskList(TaskList newList,bool isInMainOrUser) {
			List<TaskList> childLists=TaskLists.RefreshChildren(newList.TaskListNum,Security.CurUser.UserNum,0,TaskType.All);
			List<Task> childTasks=Tasks.RefreshChildren(newList.TaskListNum,true,DateTime.MinValue,Security.CurUser.UserNum,0,TaskType.All);
			TaskLists.Update(newList);//Not making a new TaskList, just moving an old one
			for(int i=0;i<childLists.Count;i++) { //updates all the child tasklists and recursively calls this method for each of their children lists.
				childLists[i].Parent=newList.TaskListNum;
				if(newList.IsRepeating) {
					childLists[i].IsRepeating=true;
					childLists[i].DateTL=DateTime.MinValue;//never a date
				}
				else {
					childLists[i].IsRepeating=false;
				}
				childLists[i].FromNum=0;
				if(!isInMainOrUser) {
					childLists[i].DateTL=DateTime.MinValue;
					childLists[i].DateType=TaskDateType.None;
				}
				MoveTaskList(childLists[i],isInMainOrUser);//delete any existing subscriptions
			}
			TaskAncestors.SynchManyForSameTasklist(childTasks,newList.TaskListNum,newList.Parent);
		}

		///<summary>Only used for dated task lists. Should NOT be used for regular task lists, puts too much strain on DB with large amount of tasks.
		///A recursive function that duplicates an entire existing TaskList. 
		///For the initial loop, make changes to the original taskList before passing it in.  
		///That way, Date and type are only set in initial loop.  All children preserve original dates and types. 
		///The isRepeating value will be applied in all loops.  Also, make sure to change the parent num to the new one before calling this function.
		///The taskListNum will always change, because we are inserting new record into database. </summary>
		private void DuplicateExistingList(TaskList newList,bool isInMainOrUser) {
			//get all children:
			List<TaskList> childLists=TaskLists.RefreshChildren(newList.TaskListNum,Security.CurUser.UserNum,0,TaskType.All);
			List<Task> childTasks=Tasks.RefreshChildren(newList.TaskListNum,true,DateTime.MinValue,Security.CurUser.UserNum,0,TaskType.All);
			if(_wasCut) { //Not making a new TaskList, just moving an old one
				TaskLists.Update(newList);
			}
			else {//copied -- We are making a new TaskList, we're keeping the old one as well
				TaskLists.Insert(newList);
			}
			//now we have a new taskListNum to work with
			for(int i=0;i<childLists.Count;i++) { //updates all the child tasklists and recursively calls this method for each of their children lists.
				childLists[i].Parent=newList.TaskListNum;
				if(newList.IsRepeating) {
					childLists[i].IsRepeating=true;
					childLists[i].DateTL=DateTime.MinValue;//never a date
				}
				else {
					childLists[i].IsRepeating=false;
				}
				childLists[i].FromNum=0;
				if(!isInMainOrUser) {
					childLists[i].DateTL=DateTime.MinValue;
					childLists[i].DateType=TaskDateType.None;
				}
				DuplicateExistingList(childLists[i],isInMainOrUser);//delete any existing subscriptions
			}
			for(int i = 0;i<childTasks.Count;i++) { //updates all the child tasks. If the task list was cut, then just update the child tasks' ancestors.
				if(_wasCut) {
					TaskAncestors.Synch(childTasks[i]);
				}
				else {//copied
					childTasks[i].TaskListNum=newList.TaskListNum;
					if(newList.IsRepeating) {
						childTasks[i].IsRepeating=true;
						childTasks[i].DateTask=DateTime.MinValue;//never a date
					}
					else {
						childTasks[i].IsRepeating=false;
					}
					childTasks[i].FromNum=0;
					if(!isInMainOrUser) {
						childTasks[i].DateTask=DateTime.MinValue;
						childTasks[i].DateType=TaskDateType.None;
					}
					if(!String.IsNullOrEmpty(childTasks[i].ReminderGroupId)) {
						//Any reminder tasks duplicated to another task list must have a brand new ReminderGroupId
						//so that they do not affect the original reminder task chain.
						Tasks.SetReminderGroupId(childTasks[i]);
					}
					List<TaskNote> noteList=TaskNotes.GetForTask(childTasks[i].TaskNum);
					long newTaskNum=Tasks.Insert(childTasks[i]);
					for(int t=0;t<noteList.Count;t++) {
						noteList[t].TaskNum=newTaskNum;
						TaskNotes.Insert(noteList[t]);//Creates the new note with the current datetime stamp.
						TaskNotes.Update(noteList[t]);//Restores the historical datetime for the note.
					}
				}
			}
		}

		private void Delete_Clicked() {
			if(_clickedI < _listTaskLists.Count) {//is list
				//check to make sure the list is empty.
				List<Task> tsks=Tasks.RefreshChildren(_listTaskLists[_clickedI].TaskListNum,true,DateTime.MinValue,Security.CurUser.UserNum,0,TaskType.All);
				List<TaskList> tsklsts=TaskLists.RefreshChildren(_listTaskLists[_clickedI].TaskListNum,Security.CurUser.UserNum,0,TaskType.All);
				if(tsks.Count>0 || tsklsts.Count>0){
					MessageBox.Show(Lan.g(this,"Not allowed to delete a list unless it's empty.  This task list contains:")+"\r\n"
						+tsks.FindAll(x => String.IsNullOrEmpty(x.ReminderGroupId)).Count+" "+Lan.g(this,"normal tasks")+"\r\n"
						+tsks.FindAll(x => !String.IsNullOrEmpty(x.ReminderGroupId)).Count+" "+Lan.g(this,"reminder tasks")+"\r\n"
						+tsklsts.Count+" "+Lan.g(this,"task lists"));
					return;
				}
				if(TaskLists.GetMailboxUserNum(_listTaskLists[_clickedI].TaskListNum)!=0) {
					MsgBox.Show(this,"Not allowed to delete task list because it is attached to a user inbox.");
					return;
				}
				if(!MsgBox.Show(this,true,"Delete this empty list?")) {
					return;
				}
				TaskSubscriptions.UpdateTaskListSubs(_listTaskLists[_clickedI].TaskListNum,0);
				TaskLists.Delete(_listTaskLists[_clickedI]);
				//DeleteEntireList(TaskListsList[clickedI]);
				DataValid.SetInvalid(InvalidType.Task);
			}
			else {//Is task
				//This security logic should match FormTaskEdit for when we enable the delete button.
				bool isTaskForCurUser = true;
				if(_clickedTask.UserNum!=Security.CurUser.UserNum) {//current user didn't write this task, so block them.
					isTaskForCurUser=false;//Delete will only be enabled if the user has the TaskEdit and TaskNoteEdit permissions.
				}
				if(_clickedTask.TaskListNum!=Security.CurUser.TaskListInBox) {//the task is not in the logged-in user's inbox
					isTaskForCurUser=false;//Delete will only be enabled if the user has the TaskEdit and TaskNoteEdit permissions.
				}
				if(isTaskForCurUser) {//this just allows getting the NoteList less often
					_listTaskNotes=TaskNotes.GetForTask(_clickedTask.TaskNum);//so we can check so see if other users have added notes
					for(int i = 0;i<_listTaskNotes.Count;i++) {
						if(Security.CurUser.UserNum!=_listTaskNotes[i].UserNum) {
							isTaskForCurUser=false;
							break;
						}
					}
				}
				//Purposefully show a popup if the user is not authorized to delete this task.
				if(!isTaskForCurUser && (!Security.IsAuthorized(Permissions.TaskEdit) || !Security.IsAuthorized(Permissions.TaskNoteEdit))) {
					return;
				}
				//This logic should match FormTaskEdit.butDelete_Click()
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete Task?")) {
					return;
				}
				if(Tasks.GetOne(_clickedTask.TaskNum)==null) {
					MsgBox.Show(this,"Task already deleted.");
					return;
				}
				if(_clickedTask.TaskListNum==0) {
					Tasks.TaskEditCreateLog(Lan.g(this,"Deleted task"),_clickedTask);
				}
				else {
					string logText=Lan.g(this,"Deleted task from tasklist");
					TaskList tList=TaskLists.GetOne(_clickedTask.TaskListNum);
					if(tList!=null) {
						logText+=" "+tList.Descript;
					}
					else {
						logText+=". Task list no longer exists";
					}
					logText+=".";
					Tasks.TaskEditCreateLog(logText,_clickedTask);
				}
				Tasks.Delete(_clickedTask.TaskNum);//always do it this way to clean up all four tables
				DataValid.SetInvalidTask(_clickedTask.TaskNum,false);//no popup and causes grid to refresh.
				TaskHist taskHistory = new TaskHist(_clickedTask);
				taskHistory.IsNoteChange=false;
				taskHistory.UserNum=Security.CurUser.UserNum;
				TaskHists.Insert(taskHistory);
				SecurityLogs.MakeLogEntry(Permissions.TaskEdit,0,"Task "+POut.Long(_clickedTask.TaskNum)+" deleted",0);
			}
			//FillGrid();
		}

		///<summary>A recursive function that deletes the specified list and all children.</summary>
		private void DeleteEntireList(TaskList list) {
			//get all children:
			List<TaskList> childLists=TaskLists.RefreshChildren(list.TaskListNum,Security.CurUser.UserNum,0,TaskType.All);
			List<Task> childTasks=Tasks.RefreshChildren(list.TaskListNum,true,DateTime.MinValue,Security.CurUser.UserNum,0,TaskType.All);
			for(int i=0;i<childLists.Count;i++) {
				DeleteEntireList(childLists[i]);
			}
			for(int i=0;i<childTasks.Count;i++) {
				Tasks.Delete(childTasks[i].TaskNum);
				SecurityLogs.MakeLogEntry(Permissions.TaskEdit,0,"Task "+POut.Long(childTasks[i].TaskNum)+" deleted",0);
			}
			try {
				TaskLists.Delete(list);
			}
			catch(Exception e) {
				MessageBox.Show(e.Message);
			}
		}

		///<summary>The indexing logic here could be improved to be easier to read, by modifying the fill grid to save
		///column indexes into class-wide private varaibles.  This way we will have access to the index without performing any logic.
		///Additionally, each variable could be set to -1 when the column is not present.</summary>
		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(e.Col==0) {//check box column
				//no longer allow double click on checkbox, because it's annoying.
				return;
			}
			if(tabContr.SelectedTab==tabNew && e.Col==2 && PrefC.GetBool(PrefName.TasksNewTrackedByUser)) {//+/- column (an index varaible would help)
				return;//Don't double click on expand column, because it already has a single click functionality.
			}
			else if(tabContr.SelectedTab==tabNew && e.Col==3 && !PrefC.GetBool(PrefName.TasksNewTrackedByUser)) {//ST column (an index varaible would help)
				return;//Don't double click on ST column.
			}
			else if(tabContr.SelectedTab==tabNew && e.Col==4 && !PrefC.GetBool(PrefName.TasksNewTrackedByUser)) {//Job column (an index varaible would help)
				return;//Don't double click on Job column.
			}
			else if(e.Col==1) {//Task List column (an index varaible would help)
				return;//Don't double click on expand column
			}
			if(e.Row >= _listTaskLists.Count) {//is task
				if(IsInvalidTaskRow(e.Row)) {
					return; //could happen if the task list refreshed while the double-click was happening.
				}
				//It's important to grab the task directly from the db because the status in this list is fake, being the "unread" status instead.
				Task task=Tasks.GetOne(_listTasks[e.Row-_listTaskLists.Count].TaskNum);
				FormTaskEdit FormT=new FormTaskEdit(task,task.Copy());
				FormT.Show();//non-modal
			}
		}

		private void gridMain_MouseDown(object sender,MouseEventArgs e) {
			_clickedI=gridMain.PointToRow(e.Y);//e.Row;
			int clickedCol=gridMain.PointToCol(e.X);
			if(_clickedI==-1){
				return;
			}
			gridMain.SetSelected(_clickedI,true);//if right click.
			_clickedTask=(Task)gridMain.Rows[_clickedI].Tag;//Task lists cause _clickedTask to be null
			if(e.Button!=MouseButtons.Left) {
				return;
			}
			if(_clickedI < _listTaskLists.Count) {//is list
				//If the list is someone else's inbox, block
				//long mailboxUserNum=TaskLists.GetMailboxUserNum(TaskListsList[clickedI].TaskListNum);
				//This is too restrictive.  Need to work into security permissions:
				//if(mailboxUserNum != 0 && mailboxUserNum != Security.CurUser.UserNum) {
				//	MsgBox.Show(this,"Inboxes are private.");
				//	return;
				//}
				_listTaskListTreeHistory.Add(_listTaskLists[_clickedI]);
				_hasListSwitched=true;
				FillTree();
				FillGrid();
				return;
			}
			_taskCollapsedState=-1;
			if(tabContr.SelectedTab==tabNew && !PrefC.GetBool(PrefName.TasksNewTrackedByUser)){//There's an extra column
				if(clickedCol==1) {
					TaskUnreads.SetRead(Security.CurUser.UserNum,_listTasks[_clickedI-_listTaskLists.Count].TaskNum);
					FillGrid();
				}
				if(clickedCol==3) {//Expand column
					if(_listExpandedTaskNums.Contains(_listTasks[_clickedI-_listTaskLists.Count].TaskNum)) {
						_listExpandedTaskNums.Remove(_listTasks[_clickedI-_listTaskLists.Count].TaskNum);
					}
					else { 
						_listExpandedTaskNums.Add(_listTasks[_clickedI-_listTaskLists.Count].TaskNum);
					}
					FillGrid();
				}
				return;//but ignore column 0 for now.  We would need to add that as a new feature.
			}
			if(clickedCol==0){//check tasks off
				if(PrefC.GetBool(PrefName.TasksNewTrackedByUser)) {
					if(tabContr.SelectedTab==tabNew){
						//these are never in someone else's inbox, so don't block.
					}
					else if(tabContr.SelectedTab==tabPatientTickets 
						&& TaskUnreads.IsUnread(Security.CurUser.UserNum,_listTasks[_clickedI-_listTaskLists.Count].TaskNum)) 
					{
						//Task clicked is new for the user, don't block.
					}
					else{
						long userNumInbox=0;
						if(tabContr.SelectedTab.In(tabOpenTickets,tabPatientTickets)) {
							userNumInbox=TaskLists.GetMailboxUserNumByAncestor(_listTasks[_clickedI-_listTaskLists.Count].TaskNum);
						}
						else {
							if(_listTaskListTreeHistory.Count!=0) {
								userNumInbox=TaskLists.GetMailboxUserNum(_listTaskListTreeHistory[0].TaskListNum);
							}
							else {
								MsgBox.Show(this,"Please setup task lists before marking tasks as read.");
								return;
							}
						}
						if(userNumInbox != 0 && userNumInbox != Security.CurUser.UserNum) {
							MsgBox.Show(this,"Not allowed to mark off tasks in someone else's inbox.");
							return;
						}
					}
					//might not need to go to db to get this info 
					//might be able to check this:
					//if(task.IsUnread) {
					//But seems safer to go to db.
					if(TaskUnreads.IsUnread(Security.CurUser.UserNum,_listTasks[_clickedI-_listTaskLists.Count].TaskNum)) {
						TaskUnreads.SetRead(Security.CurUser.UserNum,_listTasks[_clickedI-_listTaskLists.Count].TaskNum);
					}
					DataValid.SetInvalidTask(_listTasks[_clickedI-_listTaskLists.Count].TaskNum,false);
					//if already read, nothing else to do.  If done, nothing to do
				}
				else {
					if(_listTasks[_clickedI-_listTaskLists.Count].TaskStatus==TaskStatusEnum.New) {
						Task task=_listTasks[_clickedI-_listTaskLists.Count].Copy();
						Task taskOld=task.Copy();
						task.TaskStatus=TaskStatusEnum.Viewed;
						try {
							Tasks.Update(task,taskOld);
							DataValid.SetInvalidTask(task.TaskNum,false);
						}
						catch(Exception ex) {
							MessageBox.Show(ex.Message);
							return;
						}
					}
					//no longer allowed to mark done from here
				}
				//FillGrid();
			}
			if((tabContr.SelectedTab.In(tabNew,tabPatientTickets,tabOpenTickets) && clickedCol==2) 
				|| (tabContr.SelectedTab!=tabNew && clickedCol==1)) 
			{
				if(_listExpandedTaskNums.Contains(_listTasks[_clickedI-_listTaskLists.Count].TaskNum)) {
					_listExpandedTaskNums.Remove(_listTasks[_clickedI-_listTaskLists.Count].TaskNum);
				}
				else { 
					_listExpandedTaskNums.Add(_listTasks[_clickedI-_listTaskLists.Count].TaskNum);
				}
				FillGrid();
			}
		}

		private void menuEdit_Popup(object sender,System.EventArgs e) {
			SetMenusEnabled();
		}

		private void SetMenusEnabled() {
			//Done----------------------------------
			if(gridMain.SelectedIndices.Length==0 || _clickedI < _listTaskLists.Count) {//or a tasklist selected
				menuItemDone.Enabled=false;
			}
			else {
				menuItemDone.Enabled=true;
			}
			//Edit,Cut,Copy,Delete-------------------------
			if(gridMain.SelectedIndices.Length==0) {
				menuItemEdit.Enabled=false;
				menuItemCut.Enabled=false;
				menuItemCopy.Enabled=false;
				menuItemDelete.Enabled=false;
			}
			else {
				menuItemEdit.Enabled=true;
				menuItemCut.Enabled=true;
				if(_clickedI < _listTaskLists.Count) {//Is a tasklist
					menuItemCopy.Enabled=false;//We don't want users to copy tasklists, only move them by cut.
				}
				else {
					menuItemCopy.Enabled=true;
				}
				menuItemDelete.Enabled=true;
			}
			//Paste----------------------------------------
			if(tabContr.SelectedTab==tabUser && _listTaskListTreeHistory.Count==0) {//not allowed to paste into the trunk of a user tab
				menuItemPaste.Enabled=false;
			}
			else if(_clipTaskList==null && _clipTask==null) {
				menuItemPaste.Enabled=false;
			}
			else {//there is an item on our clipboard
				menuItemPaste.Enabled=true;
			}
			//(overrides)
			if(tabContr.SelectedTab==tabNew || tabContr.SelectedTab==tabOpenTickets || tabContr.SelectedTab==tabPatientTickets) {
				menuItemCut.Enabled=false;
				menuItemDelete.Enabled=false;
				menuItemPaste.Enabled=false;
			}
			//Subscriptions----------------------------------------------------------
			if(gridMain.SelectedIndices.Length==0) {
				menuItemSubscribe.Enabled=false;
				menuItemUnsubscribe.Enabled=false;
			}
			else if(tabContr.SelectedTab==tabUser && _clickedI<_listTaskLists.Count) {//user tab and is a list
				menuItemSubscribe.Enabled=false;
				menuItemUnsubscribe.Enabled=true;
			}
			else if(tabContr.SelectedTab==tabMain && _clickedI < _listTaskLists.Count) {//main and tasklist
				menuItemSubscribe.Enabled=true;
				menuItemUnsubscribe.Enabled=false;
			}
			else if(tabContr.SelectedTab==tabReminders && _clickedI < _listTaskLists.Count) {//reminders and tasklist
				menuItemSubscribe.Enabled=true;
				menuItemUnsubscribe.Enabled=false;
			}
			else{//either any other tab, or a task on the main list
				menuItemSubscribe.Enabled=false;
				menuItemUnsubscribe.Enabled=false;
			}
			menuItemPriority.MenuItems.Clear();
			//SendToMe/GoTo/Task Priority---------------------------------------------------------------
			if(gridMain.SelectedIndices.Length>0 && _clickedI >= _listTaskLists.Count){//is task
				//The clicked task was removed from _listTasks, could happen between FillGrid(), mouse click, and now
				if(IsInvalidTaskRow(_clickedI)) {
					IgnoreTaskClick();
					return;
				}
				Task task=_listTasks[_clickedI-_listTaskLists.Count];
				if(task.ObjectType==TaskObjectType.None) {
					menuItemGoto.Enabled=false;
				}
				else {
					menuItemGoto.Enabled=true;
				}
				menuItemSendToMe.Enabled=true;
				//Check if task has patient attached
				if(task.ObjectType==TaskObjectType.Patient) {
					menuItemAddCommlog.Enabled=true;
				}
				else {
					menuItemAddCommlog.Enabled=false;
				}
				if(Defs.GetDefsForCategory(DefCat.TaskPriorities,true).Count==0) {
					menuItemPriority.Enabled=false;
				}
				else {
					menuItemPriority.Enabled=true;
					Def[] defs=Defs.GetDefsForCategory(DefCat.TaskPriorities,true).ToArray();
					foreach(Def def in defs) {
						MenuItem item=menuItemPriority.MenuItems.Add(def.ItemName);
						item.Click+=(sender,e) => menuTaskPriority_Click(task,def);
					}
				}
			}
			else {
				menuItemGoto.Enabled=false;//not a task
				menuItemSendToMe.Enabled=false;
				menuItemAddCommlog.Enabled=false;
				menuItemPriority.Enabled=false;
			}
			//Navigate to Job-------------------------------------------------------------
			if(gridMain.SelectedIndices.Length>0 && _clickedI >= _listTaskLists.Count && PrefC.IsODHQ) {
				//The clicked task was removed from _listTasks, could happen between FillGrid(), mouse click, and now
				if(IsInvalidTaskRow(_clickedI)) {
					IgnoreTaskClick();
					return;
				}
				Task task=_listTasks[_clickedI-_listTaskLists.Count];
				//get list of jobs attached to task then insert info about those jobs.
				List<JobLink> _listJobLinks=JobLinks.GetForTask(task.TaskNum);
				List<Job> _listJobs=Jobs.GetMany(_listJobLinks.Select(x => x.JobNum).ToList());
				//If a job exists that is attached to the task
				if(_listJobs.Count>0) {
					menuNavJob.MenuItems.Clear();	//clear whatever items were in the menu before.
					MenuItem newItem;
					string title;
					//Get a jobnum that matches the column in task menu
					foreach(Job selectedJob in _listJobs) {
						title=selectedJob.JobNum.ToString()+" ";
						//Append the correct letter to the jobnum
						switch(selectedJob.Category) {
							case JobCategory.Feature:
								title="F"+title;
								break;
							case JobCategory.Bug:
								title="B"+title;
								break;
							case JobCategory.Enhancement:
								title="E"+title;
								break;
							case JobCategory.Query:
								title="Q"+title;
								break;
							case JobCategory.ProgramBridge:
								title="P"+title;
								break;
							case JobCategory.InternalRequest:
								title="I"+title;
								break;
							case JobCategory.HqRequest:
								title="H"+title;
								break;
							case JobCategory.Conversion:
								title="C"+title;
								break;
							case JobCategory.Research:
								title="R"+title;
								break;
						}
						//Title is: "%jobnum% %description%" with a character limit of 30
						title+=selectedJob.Title;
						if(title.Length>=30) {
							title=title.Substring(0,30);
						}
						title+="...";
						newItem=new MenuItem(title);
						newItem.Tag=selectedJob;
						newItem.Click+=(sender,e) => menuNavJob_Click(sender,e,(Job)newItem.Tag);	//set a custom click event
						menuNavJob.MenuItems.Add(newItem);
					}
					menuNavJob.Enabled=true;
				}
				else {
					menuNavJob.Enabled=false;	//if there are no jobs, then just disable the ability to click or expand the sub-menu
				}
			}
			if(_clickedI<0) {//Not clicked on any row
				menuItemDone.Enabled=false;
				menuItemEdit.Enabled=false;
				menuItemCut.Enabled=false;
				menuItemCopy.Enabled=false;
				//menuItemPaste.Enabled=false;//Don't disable paste because this one makes sense for user to do.
				menuItemDelete.Enabled=false;
				menuItemSubscribe.Enabled=false;
				menuItemUnsubscribe.Enabled=false;
				menuItemSendToMe.Enabled=false;
				menuItemGoto.Enabled=false;
				menuItemPriority.Enabled=false;
				return;
			}
		}

		private bool IsInvalidTaskRow(int row) {//Index out of range
			return (row-_listTaskLists.Count < 0 || row-_listTaskLists.Count >= _listTasks.Count);
		}

		private void IgnoreTaskClick() {
			gridMain.SetSelected(_clickedI,false);//unselect problem row
			_clickedI=-1;//since row is unselected		
			foreach(MenuItem menuItem in gridMain.ContextMenu.MenuItems) { //disable ContextMenu options
				menuItem.Enabled=false;
			}
			DataValid.SetInvalid(InvalidType.Task);//this causes an immediate local refresh of the grid
		}

		private void OnSubscribe_Click(){
			//won't even get to this point unless it is a list
			try{
				TaskSubscriptions.SubscList(_listTaskLists[_clickedI].TaskListNum,Security.CurUser.UserNum);
			}
			catch(ApplicationException ex){//for example, if already subscribed.
				MessageBox.Show(ex.Message);
				return;
			}
			MsgBox.Show(this,"Done");
		}

		private void OnUnsubscribe_Click() {
			TaskSubscriptions.UnsubscList(_listTaskLists[_clickedI].TaskListNum,Security.CurUser.UserNum);
			//FillMain();
			FillGrid();
		}

		private void menuItemDone_Click(object sender,EventArgs e) {
			Done_Clicked();
		}

		private void menuItemEdit_Click(object sender,System.EventArgs e) {
			Edit_Clicked();
		}

		private void menuItemCut_Click(object sender,System.EventArgs e) {
			Cut_Clicked();
		}

		private void menuItemCopy_Click(object sender,System.EventArgs e) {
			Copy_Clicked();
		}

		private void menuItemPaste_Click(object sender,System.EventArgs e) {
			Paste_Clicked();
		}

		private void menuItemDelete_Click(object sender,System.EventArgs e) {
			Delete_Clicked();
		}

		private void menuItemSubscribe_Click(object sender,EventArgs e) {
			OnSubscribe_Click();
		}

		private void menuItemUnsubscribe_Click(object sender,EventArgs e) {
			OnUnsubscribe_Click();
		}

		private void menuItemSendToMe_Click(object sender,EventArgs e) {
			SendToMe_Clicked();
		}

		private void menuItemAddCommlog_Click(object sender,EventArgs e) {
			SendToMeCommlog_Clicked();
		}

		private void menuItemGoto_Click(object sender,System.EventArgs e) {
			Goto_Clicked();
		}

		private void menuNavJob_Click(object sender,EventArgs e,Job selectedJob) {
			FormOpenDental.S_GoToJob(selectedJob.JobNum);
		}

		private void menuDeleteTaken_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			TaskTakens.DeleteForTask(_clickedTask.TaskNum);
			Cursor=Cursors.Default;
			MsgBox.Show(this,"Task taken deleted");
		}

		private void menuTaskPriority_Click(Task task,Def priorityDef) {
			Task taskNew=task.Copy();
			taskNew.PriorityDefNum=priorityDef.DefNum;
			try {
				Tasks.Update(taskNew,task);
				if(PrefC.IsODHQ && priorityDef.DefNum==502) {//They chose Blue as their priority
					TaskNote taskNote=new TaskNote();
					taskNote.UserNum=Security.CurUser.UserNum;
					taskNote.TaskNum=task.TaskNum;
					taskNote.Note="Setting priority to blue.";
					TaskNotes.Insert(taskNote);
				}
				TaskHist taskHist=new TaskHist(task);
				taskHist.UserNumHist=Security.CurUser.UserNum;
				TaskHists.Insert(taskHist);
				DataValid.SetInvalidTask(taskNew.TaskNum,false);
			}
			catch(Exception ex) {//Happens when two users edit the same task at the same time.
				MessageBox.Show(ex.Message);
			}
		}

		//private void listMain_SelectedIndexChanged(object sender,System.EventArgs e) {
		//	SetMenusEnabled();
		//}

		private void tree_MouseDown(object sender,System.Windows.Forms.MouseEventArgs e) {
			for(int i=_listTaskListTreeHistory.Count-1;i>0;i--) {
				try {
					if(_listTaskListTreeHistory[i].TaskListNum==(long)tree.GetNodeAt(e.X,e.Y).Tag) {
						break;//don't remove the node click on or any higher node
					}
					_listTaskListTreeHistory.RemoveAt(i);
				}
				catch {//Harmless to return here because the user could have clicked near the node
					return;
				}
			}
			FillTree();
			//FillMain();
			FillGrid();
		}

		private void timerDoneTaskListRefresh_Tick(object sender,EventArgs e) {
			//This timer was set by textStartDate_TextChanged in order to prevent refreshing too frequently.
			timerDoneTaskListRefresh.Stop();
			FillGrid();
		}
		
		///<summary>Currently only used so that we can set the title of FormTask.</summary>
		public delegate void FillGridEventHandler(object sender,EventArgs e);

	}

	///<summary>Each item in this enumeration identifies a specific tab within UserControlTasks.</summary>
	public enum UserControlTasksTab {
		///<summary>0</summary>
		Invalid,
		///<summary>1</summary>
		ForUser,
		///<summary>2</summary>
		UserNew,
		///<summary>3</summary>
		OpenTickets,
		///<summary>4</summary>
		Main,
		///<summary>5</summary>
		Reminders,
		///<summary>6</summary>
		RepeatingSetup,
		///<summary>7</summary>
		RepeatingByDate,
		///<summary>8</summary>
		RepeatingByWeek,
		///<summary>9</summary>
		RepeatingByMonth,
		///<summary>10</summary>
		PatientTickets
	}

}
