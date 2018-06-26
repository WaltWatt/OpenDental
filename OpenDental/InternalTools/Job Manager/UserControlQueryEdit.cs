using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using OpenDental.UI;
using OpenDentBusiness;
using CodeBase;
using System.Diagnostics;

namespace OpenDental.InternalTools.Job_Manager {
	
	public partial class UserControlQueryEdit:UserControl {
		//FIELDS
		public bool IsNew;
		private Job _jobOld;
		private Job _jobCur;
		///<summary>Private member for IsChanged Property. Private setter, public getter.</summary>
		private bool _isChanged;
		private bool _isOverride;
		private bool _isLoading;
		///<summary>Passed in to LoadJob from job manager, used to display job family.</summary>
		private TreeNode _treeNode;
		/////<summary>The list of all job priorities that are ordered the way they need to display in the UI.</summary>
		//public List<JobPriority> _listJobPrioritiesSorted=new List<JobPriority> {
		//	JobPriority.Urgent,
		//	JobPriority.High,
		//	JobPriority.MediumHigh,
		//	JobPriority.Normal,
		//	JobPriority.Low,
		//	JobPriority.OnHold
		//};

		///<summary>Occurs whenever this control saves changes to DB, after the control has redrawn itself. 
		/// Usually connected to either a form close or refresh.</summary>
		[Category("Action"),Description("Whenever this control saves changes to DB, after the control has redrawn itself. Usually connected to either a form close or refresh.")]
		public event EventHandler SaveClick=null;

		public delegate void RequestJobEvent(object sender,long jobNum);
		public event RequestJobEvent RequestJob=null;

		public delegate void UpdateTitleEvent(object sender,string title);
		public event UpdateTitleEvent TitleChanged=null;

		public delegate void JobOverrideEvent(object sender,bool isOverride);
		public event JobOverrideEvent JobOverride=null;

		//PROPERTIES
		public bool IsChanged {
			get { return _isChanged; }
			private set {
				if(_isLoading) {
					_isChanged=false;
					return;
				}
				_isChanged=value;
			}
		}

		public bool IsOverride {
			get {return _isOverride;}
			set {
				_isOverride=value;
				CheckPermissions();
			}
		}

		//FUNCTIONS
		public UserControlQueryEdit() {
			InitializeComponent();
			if(gridFiles.ContextMenu==null) {
				gridFiles.ContextMenu=new ContextMenu();
			}
			gridFiles.ContextMenu.Popup+=FilePopupHelper;
		}

		///<summary>Not a property so that this is compatible with the VS designer.</summary>
		public Job GetJob() {
			if(_jobCur==null) {
				return null;
			}
			Job job = _jobCur.Copy();
			job.Implementation=textEditorMain.MainRtf;
			return job;
		}

		///<summary>Should only be called once when new job should be loaded into control. If called again, changes will be lost.</summary>
		public void LoadJob(Job job,TreeNode treeNode) {
			_isLoading=true;
			this.Enabled=false;//disable control while it is filled.
			_isOverride=false;
			IsChanged=false;
			_treeNode=treeNode;
			if(job==null) {
				_jobCur=new Job();
			}
			else {
				_jobCur=job.Copy();
				IsNew=job.IsNew;
			}
			_jobOld=_jobCur.Copy();//cannot be null
			try {
				textEditorMain.MainRtf=_jobCur.Implementation;//This is here to convert our old job descriptions to the new RTF descriptions.
			}
			catch {
				textEditorMain.MainText=_jobCur.Implementation;
			}
			//Query Jobs must have a quote attached on creation.
			textJobNum.Text=_jobCur.JobNum>0?_jobCur.JobNum.ToString():Lan.g("Jobs","New Job");
			textDateEntry.Text=_jobCur.DateTimeEntry.Year>1880?_jobCur.DateTimeEntry.ToShortDateString():"";
			textTitle.Text=_jobCur.Title.ToString();
			textCustomer.Text=_jobCur.ListJobQuotes.FirstOrDefault().PatNum.ToString()+" - "+Patients.GetPat(_jobCur.ListJobQuotes.FirstOrDefault().PatNum).GetNameLF();
			comboPriority.Items.Clear();
			foreach(Def def in Defs.GetDefsForCategory(DefCat.JobPriorities,true).ToList()) {
				comboPriority.Items.Add(new ODBoxItem<Def>(def.ItemName,def));
				if(def.DefNum==job.Priority) {
					comboPriority.SelectedIndex=comboPriority.Items.Count-1;
				}
			}
			comboPhase.Items.Clear();
			foreach(JobPhase phase in Enum.GetValues(typeof(JobPhase)).Cast<JobPhase>()) {
				comboPhase.Items.Add(new ODBoxItem<JobPhase>(phase.GetDescription(),phase));
				if(phase==job.PhaseCur) {
					comboPhase.SelectedIndex=comboPhase.Items.Count-1;
				}
			}
			textQuoteHours.Text=_jobCur.ListJobQuotes.FirstOrDefault().Hours.ToString();
			textQuoteAmount.Text=_jobCur.ListJobQuotes.FirstOrDefault().Amount.ToString();
			checkApproved.Checked=_jobCur.ListJobQuotes.FirstOrDefault().IsCustomerApproved;
			textQuoteDate.Text=_jobCur.DateTimeCustContact.Year<1880?"":_jobCur.DateTimeCustContact.ToShortDateString();
			textSchedDate.Text=_jobCur.AckDateTime.Year<1880?"":_jobCur.AckDateTime.ToString();
			FillAllGrids();
			IsChanged=false;
			CheckPermissions();
			if(job!=null) {//re-enable control after we have loaded the job.
				this.Enabled=true;
			}
			_isLoading=false;
		}

		private void FillAllGrids() {
			FillGridRoles();
			FillGridSubscribers();
			FillGridTasks();
			FillGridAppointments();
			FillGridFiles();
			FillGridNote();
			FillGridHistory();
		}

		#region FillGrids

		private void FillGridRoles() {
			gridRoles.BeginUpdate();
			gridRoles.Columns.Clear();
			gridRoles.Columns.Add(new ODGridColumn("",90));
			gridRoles.Columns.Add(new ODGridColumn("User",50));
			gridRoles.NoteSpanStart=0;
			gridRoles.NoteSpanStop=1;
			gridRoles.Rows.Clear();
			//These columns are ordered by convenience, If some other ordering would be more convenient then they should just be re-ordered.
			gridRoles.AddRow("Created By:",Userods.GetName(_jobCur.UserNumConcept));
			gridRoles.AddRow("Owner:",Userods.GetName(_jobCur.UserNumEngineer));
			gridRoles.AddRow("Reviewer:",Userods.GetName(_jobCur.UserNumExpert));
			gridRoles.EndUpdate();
		}

		private void FillGridSubscribers() {
			gridWatchers.BeginUpdate();
			gridWatchers.Columns.Clear();
			gridWatchers.Columns.Add(new ODGridColumn("",50));
			gridWatchers.Rows.Clear();
			List<Userod> listSubscribers=_jobCur.ListJobLinks.FindAll(x => x.LinkType==JobLinkType.Subscriber)
				.Select(x => Userods.GetFirstOrDefault(y => y.UserNum==x.FKey)).ToList();
			foreach(Userod user in listSubscribers.FindAll(x => x!=null)){
				ODGridRow row=new ODGridRow() { Tag =user };
				row.Cells.Add(user.UserName);
				gridWatchers.Rows.Add(row);
			}
			gridWatchers.EndUpdate();
		}

		private void FillGridTasks() {
			gridTasks.BeginUpdate();
			gridTasks.Columns.Clear();
			gridTasks.Columns.Add(new ODGridColumn("Date",70));
			gridTasks.Columns.Add(new ODGridColumn("TaskList",100));
			gridTasks.Columns.Add(new ODGridColumn("Done",40) { TextAlign=HorizontalAlignment.Center });
			gridTasks.NoteSpanStart=0;
			gridTasks.NoteSpanStop=2;
			gridTasks.Rows.Clear();
			List<Task> listTasks=_jobCur.ListJobLinks.FindAll(x => x.LinkType==JobLinkType.Task)
				.Select(x => Tasks.GetOne(x.FKey))
				.Where(x => x!=null)
				.OrderBy(x =>x.DateTimeEntry).ToList();
			foreach(Task task in listTasks){
				ODGridRow row=new ODGridRow() { Tag=task.TaskNum };//taskNum
				row.Cells.Add(task.DateTimeEntry.ToShortDateString());
				row.Cells.Add(TaskLists.GetOne(task.TaskListNum)?.Descript??"<TaskListNum:"+task.TaskListNum+">");
				row.Cells.Add(task.TaskStatus==TaskStatusEnum.Done?"X":"");
				row.Note=task.Descript.Left(100,true).Trim();
				gridTasks.Rows.Add(row);
			}
			gridTasks.EndUpdate();
		}

		private void FillGridAppointments() {
			gridAppointments.BeginUpdate();
			gridAppointments.Columns.Clear();
			gridAppointments.Columns.Add(new ODGridColumn("Appt Num",150));
			gridAppointments.Rows.Clear();
			List<long> listApptNums=_jobCur.ListJobLinks.FindAll(x => x.LinkType==JobLinkType.Appointment).Select(x => x.FKey).ToList();
			foreach(long aptNum in listApptNums){
				ODGridRow row=new ODGridRow() { Tag=aptNum };
				row.Cells.Add(aptNum.ToString());
				gridAppointments.Rows.Add(row);
			}
			gridAppointments.EndUpdate();
		}

		private void FillGridFiles() {
			gridFiles.BeginUpdate();
			gridFiles.Columns.Clear();
			gridFiles.Columns.Add(new ODGridColumn(Lan.g(this,""),120));
			gridFiles.Rows.Clear();
			ODGridRow row;
			foreach(JobLink link in _jobCur.ListJobLinks.FindAll(x => x.LinkType==JobLinkType.File)) {
				row=new ODGridRow();
				if(String.IsNullOrEmpty(link.DisplayOverride)) {
					row.Cells.Add(link.Tag.Split('\\').Last());
				}
				else {
					row.Cells.Add(link.DisplayOverride.ToString());
				}
				row.Tag=link;
				gridFiles.Rows.Add(row);
			}
			gridFiles.EndUpdate();
		}
		#endregion

		///<summary>Just prior to displaying the context menu, add wiki links if neccesary.</summary>
		private void FilePopupHelper(object sender,EventArgs e) {
			ContextMenu menu = gridFiles.ContextMenu;
			//Always clear the options in the context menu because it could contain options for a previous row.
			menu.MenuItems.Clear();
			if(gridFiles.SelectedIndices.Length==0) {
				return;
			}
			JobLink link = _jobCur.ListJobLinks.FirstOrDefault(x => x.JobLinkNum==((JobLink)gridFiles.Rows[gridFiles.SelectedIndices[0]].Tag).JobLinkNum);
			menu.MenuItems.Add("Override display name",(o,arg) => {
				InputBox inputBox = new InputBox("Give a name override for the file");
				inputBox.textResult.Text=link.DisplayOverride;
				inputBox.textResult.SelectAll();
				if(inputBox.ShowDialog()==DialogResult.Cancel) {
					return;
				}
				link.DisplayOverride=inputBox.textResult.Text;
				JobLinks.Update(link);
				Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,_jobCur.JobNum);
				FillGridFiles();
			});
			menu.MenuItems.Add("Open File",(o,arg) => {
				try {
					System.Diagnostics.Process.Start(link.Tag);
				}
				catch(Exception ex) {
					ex.DoNothing();
					MessageBox.Show("Unable to open file.");
					try {
						System.Diagnostics.Process.Start(link.Tag.Substring(0,link.Tag.LastIndexOf('\\')));
					}
					catch(Exception exc) { exc.DoNothing(); }
				}
			});
			if(link.Tag.Contains("\\")) {
				try {
					string folder = link.Tag.Substring(0,link.Tag.LastIndexOf('\\'));
					menu.MenuItems.Add("Open Folder",(o,arg) => {
						try {
							System.Diagnostics.Process.Start(folder);
						}
						catch(Exception ex) { ex.DoNothing(); }
					});
				}
				catch(Exception ex) { ex.DoNothing(); }
			}
			menu.MenuItems.Add("-");
			menu.MenuItems.Add("Unlink File",(o,arg) => {
				List<JobLink> listLinks = _jobCur.ListJobLinks.FindAll(x => x.LinkType==JobLinkType.File && x.Tag==link.Tag);
				_jobCur.ListJobLinks.RemoveAll(x => x.LinkType==JobLinkType.File && x.Tag==link.Tag);
				_jobOld.ListJobLinks.RemoveAll(x => x.LinkType==JobLinkType.File && x.Tag==link.Tag);
				listLinks.Select(x => x.JobLinkNum).ToList().ForEach(JobLinks.Delete);
				FillGridFiles();
				Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,_jobCur.JobNum);
			});
			menu.MenuItems.Add(new MenuItem(link.Tag) { Enabled=false });//show file path in gray
		}

		public void FillGridNote() {
			gridNotes.BeginUpdate();
			gridNotes.Columns.Clear();
			gridNotes.Columns.Add(new ODGridColumn(Lan.g(this,"Note"),400));
			gridNotes.Rows.Clear();
			ODGridRow row;
			List<JobNote> listJobNotes=_jobCur.ListJobNotes.ToList();
			listJobNotes=listJobNotes.OrderByDescending(x => x.DateTimeNote).ToList();
			foreach(JobNote jobNote in listJobNotes) {
				row=new ODGridRow();
				row.Cells.Add(jobNote.DateTimeNote.ToShortDateString()+" "+jobNote.DateTimeNote.ToShortTimeString()+" - "+Userods.GetName(jobNote.UserNum)+" - "+jobNote.Note);
				row.Tag=jobNote;
				gridNotes.Rows.Add(row);
			}
			gridNotes.EndUpdate();
		}

		private void FillGridHistory() {
			gridHistory.BeginUpdate();
			gridHistory.Columns.Clear();
			gridHistory.Columns.Add(new ODGridColumn("Date",140));
			gridHistory.Columns.Add(new ODGridColumn("User Change",90) { TextAlign=HorizontalAlignment.Center });
			gridHistory.Columns.Add(new ODGridColumn("Expert",90) { TextAlign=HorizontalAlignment.Center });
			gridHistory.Columns.Add(new ODGridColumn("Engineer",90) { TextAlign=HorizontalAlignment.Center });
			gridHistory.Columns.Add(new ODGridColumn("RTF",35) { TextAlign=HorizontalAlignment.Center });//shows X if this row has a copy of job description text.
			gridHistory.Columns.Add(new ODGridColumn("Description",300));
			gridHistory.Rows.Clear();
			gridHistory.NoteSpanStart=1;
			gridHistory.NoteSpanStop=4;
			ODGridRow row;
			RichTextBox rtb = new RichTextBox();
			foreach(JobLog jobLog in _jobCur.ListJobLogs.OrderBy(x=>x.DateTimeEntry)) {
				row=new ODGridRow();
				row.Cells.Add(jobLog.DateTimeEntry.ToShortDateString()+" "+jobLog.DateTimeEntry.ToShortTimeString());
				row.Cells.Add(Userods.GetName(jobLog.UserNumChanged));
				row.Cells.Add(Userods.GetName(jobLog.UserNumExpert));
				row.Cells.Add(Userods.GetName(jobLog.UserNumEngineer));
				rtb.Clear();
				try {
					rtb.Rtf=jobLog.MainRTF;
				}
				catch {
					//fail silently
				}
				if(checkShowHistoryText.Checked && !string.IsNullOrWhiteSpace(rtb.Text)) {
					row.Note=rtb.Text;
				}
				row.Cells.Add(string.IsNullOrWhiteSpace(rtb.Text) ? "" : "X");
				row.Cells.Add(jobLog.Description);
				if(checkShowHistoryText.Checked && gridHistory.Rows.Count%2==1) {
					row.ColorBackG=Color.FromArgb(245,251,255);//light blue every other row.
				}
				row.Tag=jobLog;
				gridHistory.Rows.Add(row);
			}
			rtb.Dispose();
			gridHistory.EndUpdate();
		}

		///<summary>Based on job status, category, and user role, this will enable or disable various controls.</summary>
		private void CheckPermissions() {
			//disable various controls and re-enable them below depending on permissions.
			textEditorMain.ReadOnly=true;
			if(_jobCur==null) {
				return;
			}
			switch(_jobCur.PhaseCur) {
				case JobPhase.Concept:
				case JobPhase.Quote:
				case JobPhase.Definition:
					if(!JobPermissions.IsAuthorized(JobPerm.QueryTech,true)) {
						break;
					}
					textEditorMain.ReadOnly=false;
					break;
				case JobPhase.Development:
					if(!JobPermissions.IsAuthorized(JobPerm.QueryCoordinator,true)) {
						break;
					}
					textEditorMain.ReadOnly=false;
					break;
				case JobPhase.Complete:
					//Can only edit concept job if you meet one of the following
					//1) You have concept permission.
					//2) Concept needs approval and you have approval permission
					//textTitle.ReadOnly=false;
					//comboPriority.Enabled=true;
					//comboStatus.Enabled=true;
					//comboCategory.Enabled=true;
					//textVersion.ReadOnly=false;
					//textEstHours.Enabled=true;
					//textActualHours.Enabled=true;
					//butParentPick.Visible=true;
					//butParentRemove.Visible=true;
					//gridCustomerQuotes.HasAddButton=true;//Quote permission only
					//textEditorMain.ReadOnly=false;
					break;
				case JobPhase.Cancelled:
					//if(!JobPermissions.IsAuthorized(JobPerm.Concept,true)) {
					//	break;
					//}
					//Can only edit concept job if you meet one of the following
					//1) You have concept permission.
					//2) Concept needs approval and you have approval permission
					//textTitle.ReadOnly=false;
					//comboPriority.Enabled=true;
					//comboStatus.Enabled=true;
					//comboCategory.Enabled=true;
					//textVersion.ReadOnly=false;
					//textEstHours.Enabled=true;
					//textActualHours.Enabled=true;
					//butParentPick.Visible=true;
					//butParentRemove.Visible=true;
					//gridCustomerQuotes.HasAddButton=true;//Quote permission only
					//textEditorMain.ReadOnly=false;
					break;
				default:
					MessageBox.Show("Unsupported job status. Add to UserControlJobEdit.CheckPermissions()");
					break;
			}
			//Disable description, documentation, and title if "Checked out"
			textEditorMain.Enabled=true;//might still be read only.
		}

		///<summary>Resizes Link Grids in group box.</summary>
		private void groupLinks_Resize(object sender,EventArgs e) {
			List<ODGrid> grids=groupLinks.Controls.OfType<ODGrid>().OrderBy(x => x.Top).ToList();
			int padding=4;
			int topMost=grids.Min(x=>x.Top);
			int sizeEach=(groupLinks.Height-topMost-(padding*grids.Count))/grids.Count;
			for(int i=0;i<grids.Count;i++) {
				grids[i].Top=topMost+(i*(sizeEach+padding));
				grids[i].Height=sizeEach;
			}
		}

		private void butActions_Click(object sender,EventArgs e) {
			bool perm=false;
			ContextMenu actionMenu=new System.Windows.Forms.ContextMenu();
			switch(_jobCur.PhaseCur) {
				case JobPhase.Concept:
				case JobPhase.Definition:
				case JobPhase.Quote:
				case JobPhase.Development:
					perm=JobPermissions.IsAuthorized(JobPerm.QueryTech,true);
					actionMenu.MenuItems.Add(new MenuItem((_jobCur.UserNumConcept==0 ? "A" : "Rea")+"ssign Creator",actionMenu_AssignSubmitterClick) { Enabled=perm });
					actionMenu.MenuItems.Add(new MenuItem("Send to Definition",actionMenu_SendDefinitionClick) { Enabled=perm });
					if(JobPermissions.IsAuthorized(JobPerm.QueryCoordinator,true)) {
						actionMenu.MenuItems.Add(new MenuItem((_jobCur.UserNumEngineer==0 ? "A" : "Rea")+"ssign Owner",actionMenu_AssignEngineerClick) { Enabled=true });//x
					}
					perm=JobPermissions.IsAuthorized(JobPerm.QueryCoordinator,true) && (_jobCur.UserNumEngineer==0 || _jobCur.UserNumEngineer==Security.CurUser.UserNum);
					actionMenu.MenuItems.Add(new MenuItem("Send to Quote",actionMenu_RequestQuoteClick) { Enabled=perm });//x
					if(JobPermissions.IsAuthorized(JobPerm.SeniorQueryCoordinator,true)) {
						actionMenu.MenuItems.Add(new MenuItem((_jobCur.UserNumExpert==0 ? "A" : "Rea")+"ssign Reviewer",actionMenu_AssignExpertClick) { Enabled=true });//x
					}
					perm=JobPermissions.IsAuthorized(JobPerm.SeniorQueryCoordinator,true) && (_jobCur.UserNumExpert==0 || _jobCur.UserNumExpert==Security.CurUser.UserNum);
					actionMenu.MenuItems.Add(new MenuItem("Send to Development",actionMenu_SendInDevelopmentClick) { Enabled=perm });//x
					bool isSQC =JobPermissions.IsAuthorized(JobPerm.SeniorQueryCoordinator,true) && (_jobCur.UserNumExpert==0 || _jobCur.UserNumExpert==Security.CurUser.UserNum);
					perm=isSQC;
					actionMenu.MenuItems.Add(new MenuItem("Mark as Complete",actionMenu_ImplementedClick) { Enabled=perm });
					break;
				case JobPhase.Complete:
					actionMenu.MenuItems.Add(new MenuItem("Completed Job") { Enabled=false });
					break;
				case JobPhase.Cancelled:
					perm=JobPermissions.IsAuthorized(JobPerm.SeniorQueryCoordinator,true);
					//actionMenu.MenuItems.Add(new MenuItem("Reopen as Concept",actionMenu_ApproveConceptClick) { Enabled=perm });//x
					break;
				default:
					actionMenu.MenuItems.Add(new MenuItem("Unhandled status "+_jobCur.PhaseCur.ToString(),(o,ea)=> { }) { Enabled=false });
					break;
			}
			if(_jobCur.UserNumCheckout>0 && _jobCur.UserNumCheckout!=Security.CurUser.UserNum && !_isOverride) {
				//disable all menu items if job is checked out by other user.
				actionMenu.MenuItems.OfType<MenuItem>().ToList().ForEach(x => x.Enabled=false);
			}
			if(JobPermissions.IsAuthorized(JobPerm.QueryCoordinator,true) && _jobCur.PhaseCur!=JobPhase.Complete) {
				actionMenu.MenuItems.Add("-");
				actionMenu.MenuItems.Add(new MenuItem("Cancel Query",actionMenu_CancelJobClick));
			}
			if(actionMenu.MenuItems.Count>0 && actionMenu.MenuItems[0].Text=="-") {
				actionMenu.MenuItems.RemoveAt(0);
			}
			if(actionMenu.MenuItems.Count==0) {
				actionMenu.MenuItems.Add(new MenuItem("No Actions Available"){Enabled=false});
			}
			butActions.ContextMenu=actionMenu;
			butActions.ContextMenu.Show(butActions,new Point(0,butActions.Height));
		}

		///<summary>This should not implement any permissions, this should only check that the fields of the job are valid.</summary>
		/// <param name="_jobCur"></param>
		/// <returns></returns>
		private bool ValidateJob(Job _jobCur) {
			if(string.IsNullOrWhiteSpace(_jobCur.Title)) {
				MessageBox.Show("Invalid Title.");
				return false;
			}
			if(_jobCur.Category==JobCategory.Bug && _jobCur.ListJobLinks.Count(x => x.LinkType==JobLinkType.Bug)==0 && (_jobCur.PhaseCur!=JobPhase.Concept || _jobCur.IsApprovalNeeded)) {
				MsgBox.Show(this,"Bug jobs must have an attached bug.");
				return false;
			}
			return true;
		}

		#region ACTION BUTTON MENU ITEMS //====================================================
		#region Bug Actions
		private void actionMenu_SendDefinitionClick(object sender,EventArgs e) {
			if(!ValidateJob(_jobCur)) {
				return;
			}
			long userNumEngineer = _jobCur.UserNumEngineer;
			if(!PickUserByJobPermission("Pick Owner",JobPerm.QueryCoordinator,out userNumEngineer,_jobCur.UserNumEngineer,JobPermissions.IsAuthorized(JobPerm.QueryCoordinator,true,Security.CurUser.UserNum),false)) {
				return;
			}
			_jobCur.UserNumEngineer=userNumEngineer;
			_jobCur.PhaseCur=JobPhase.Definition;
			SaveJob(_jobCur);
		}

		private void actionMenu_SendInDevelopmentClick(object sender,EventArgs e) {
			if(!ValidateJob(_jobCur)) {
				return;
			}
			long userNumEngineer = _jobCur.UserNumEngineer;
			if(_jobCur.UserNumEngineer==0 && !PickUserByJobPermission("Pick Owner",JobPerm.QueryCoordinator,out userNumEngineer,_jobCur.UserNumEngineer>0 ? _jobCur.UserNumEngineer : _jobCur.UserNumConcept,true,false)) {
				return;
			}
			_jobCur.UserNumEngineer=userNumEngineer;
			_jobCur.PhaseCur=JobPhase.Development;
			SaveJob(_jobCur);
		}
		#endregion
		#region Assign Users

		private void actionMenu_AssignSubmitterClick(object sender,EventArgs e) {
			if(!ValidateJob(_jobCur)) {
				return;
			}
			long userNumConcept;
			if(!PickUserByJobPermission("Pick Creator",JobPerm.QueryTech,out userNumConcept,_jobCur.UserNumConcept,false,false)) {
				return;
			}
			if(userNumConcept==_jobOld.UserNumConcept) {
				return;
			}
			IsChanged=true;
			_jobCur.UserNumConcept=userNumConcept;
			SaveJob(_jobCur);
		}

		private void actionMenu_AssignExpertClick(object sender,EventArgs e) {
			if(!ValidateJob(_jobCur)) {
				return;
			}
			long userNumExpert;
			if(!PickUserByJobPermission("Pick Reviewer",JobPerm.SeniorQueryCoordinator,out userNumExpert,_jobCur.UserNumExpert,true,false)) {
				return;
			}
			if(userNumExpert==_jobOld.UserNumExpert) {
				return;
			}
			IsChanged=true;
			_jobCur.UserNumExpert=userNumExpert;
			SaveJob(_jobCur);
		}

		private void actionMenu_AssignEngineerClick(object sender,EventArgs e) {
			if(!ValidateJob(_jobCur)) {
				return;
			}
			long userNumEngineer;
			if(!PickUserByJobPermission("Pick Owner",JobPerm.QueryCoordinator,out userNumEngineer,_jobCur.UserNumEngineer,true,false)) {
				return;
			}
			if(userNumEngineer==_jobOld.UserNumEngineer) {
				return;
			}
			IsChanged=true;
			_jobCur.UserNumEngineer=userNumEngineer;
			SaveJob(_jobCur);
		}
		#endregion
		#region Request Approval/Reviews
		
		private void actionMenu_RepealApprovalRequestClick(object sender,EventArgs e) {
			IsChanged=true;
			_jobCur.IsApprovalNeeded=false;
			SaveJob(_jobCur);
		}

		private void actionMenu_UnapproveJobClick(object sender,EventArgs e) {
			if(!ValidateJob(_jobCur)) {
				return;
			}
			IsChanged=true;
			_jobCur.PhaseCur=JobPhase.Concept;
			_jobCur.IsApprovalNeeded=false;
			_jobCur.UserNumApproverConcept=0;
			_jobCur.UserNumApproverJob=0;
			_jobCur.UserNumApproverChange=0;
			SaveJob(_jobCur);
		}
		#endregion
		#region Approval Options
		private void actionMenu_RequestQuoteClick(object sender,EventArgs e) {
			IsChanged=true;
			_jobCur.PhaseCur=JobPhase.Quote;
			SaveJob(_jobCur);
		}

		private void actionMenu_CancelJobClick(object sender,EventArgs e) {
			if(!ValidateJob(_jobCur)) {
				return;
			}
			IsChanged=true;
			_jobCur.UserNumInfo=0;
			_jobCur.IsApprovalNeeded=false;
			_jobCur.PhaseCur=JobPhase.Cancelled;
			SaveJob(_jobCur);
		}
		#endregion

		private void actionMenu_ImplementedClick(object sender,EventArgs e) {
			if(!ValidateJob(_jobCur)) {
				return;
			}
			//_jobCur.Priority=JobPriority.Normal;
			//comboPriority.SelectedIndex=_listJobPrioritiesSorted.IndexOf(_jobCur.Priority);
			IsChanged=true;
			_jobCur.PhaseCur=JobPhase.Complete;
			SaveJob(_jobCur);
		}

		#endregion ACTION BUTTON MENU ITEMS //=================================================

		///<summary>If returns false if selection is cancelled. DefaultUserNum is usually the currently set usernum for a given role.</summary>
		private bool PickUserByJobPermission(string prompt,JobPerm jobPerm,out long selectedNum, long suggestedUserNum = 0,bool AllowNone = true,bool AllowAll = true) {
			selectedNum=0;
			List<Userod> listUsersForPicker = Userods.GetUsersByJobRole(jobPerm,false);
			FormUserPick FormUP = new FormUserPick();
			FormUP.Text=prompt;
			FormUP.IsSelectionmode=true;
			FormUP.ListUserodsFiltered=listUsersForPicker;
			FormUP.SuggestedUserNum=suggestedUserNum;
			FormUP.IsPickNoneAllowed=AllowNone;
			FormUP.IsShowAllAllowed=AllowAll;
			if(FormUP.ShowDialog()!=DialogResult.OK) {
				return false;
			}
			selectedNum=FormUP.SelectedUserNum;
			return true;
		}

		///<summary>Job must have all in memory fields filled. Eg. Job.ListJobLinks, Job.ListJobNotes, etc. Also makes some of the JobLog entries.</summary>
		private void SaveJob(Job job) {
			_isLoading=true;
			//Validation must happen before this is called.
			job.Implementation=textEditorMain.MainRtf;
			//All other fields should have been maintained while editing the job in the form.
			job.UserNumCheckout=0;
			if(job.JobNum==0 || IsNew) {
				if(job.UserNumConcept==0 && JobPermissions.IsAuthorized(JobPerm.QueryTech,true)) {
					job.UserNumConcept=Security.CurUser.UserNum;
				}
				Jobs.Insert(job);
				job.ListJobLinks.ForEach(x=>x.JobNum=job.JobNum);
				job.ListJobNotes.ForEach(x=>x.JobNum=job.JobNum);
				job.ListJobReviews.ForEach(x=>x.JobNum=job.JobNum);
				job.ListJobQuotes.ForEach(x=>x.JobNum=job.JobNum);
				//job.ListJobEvents.ForEach(x=>x.JobNum=job.JobNum);//do not sync
			}
			else {
				Jobs.Update(job);
			}
			JobLinks.Sync(job.ListJobLinks,job.JobNum);
			JobNotes.Sync(job.ListJobNotes,job.JobNum);
			JobReviews.SyncReviews(job.ListJobReviews,job.JobNum);
			JobQuotes.Sync(job.ListJobQuotes,job.JobNum);
			MakeLogEntry(job,_jobOld);
			Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,job.JobNum);
			LoadJob(job,_treeNode);//Tree view may become out of date if viewing a job for an extended period of time.
			if(SaveClick!=null) {
				SaveClick(this,new EventArgs());
			}
			_isLoading=false;
		}
		
		private void textTitle_Leave(object sender,EventArgs e) {
			if(_isLoading || IsNew) {
				return;
			}
			_jobCur.Title=textTitle.Text;
			Jobs.Update(_jobCur);
			Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,_jobCur.JobNum);
		}

		private void textCustomer_Click(object sender,EventArgs e) {
			JobQuote jobQuote = _jobCur.ListJobQuotes.FirstOrDefault();
			FormPatientSelect FormPS=new FormPatientSelect();
			if(jobQuote.PatNum!=0) {
				FormPS.ExplicitPatNums=new List<long> {jobQuote.PatNum};
			}
			FormPS.ShowDialog();
			if(FormPS.DialogResult!=DialogResult.OK) {
				return;
			}
			Patient pat=Patients.GetPat(FormPS.SelectedPatNum);
			if(pat!=null) {
				jobQuote.PatNum=pat.PatNum;
			}
			else {
				jobQuote.PatNum=0;
			}
			textCustomer.Text=jobQuote.PatNum.ToString()+" - "+Patients.GetPat(jobQuote.PatNum).GetNameLF();
			JobQuotes.Update(jobQuote);
		}

		private void comboPriority_SelectionChangeCommitted(object sender,EventArgs e) {
			if(_isLoading || IsNew) {
				return;
			}
			_jobCur.Priority=((ODBoxItem<Def>)comboPriority.SelectedItem).Tag.DefNum;
			Jobs.Update(_jobCur);
			Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,_jobCur.JobNum);
		}

		private void comboPhase_SelectionChangeCommitted(object sender,EventArgs e) {
			if(_isLoading || IsNew) {
				return;
			}
			_jobCur.PhaseCur=((ODBoxItem<JobPhase>)comboPhase.SelectedItem).Tag;
			Jobs.Update(_jobCur);
			Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,_jobCur.JobNum);
		}

		private void textQuoteHours_Leave(object sender,EventArgs e) {
			if(_isLoading || IsNew) {
				return;
			}
			JobQuote jobQuote = _jobCur.ListJobQuotes.FirstOrDefault();
			jobQuote.Hours=textQuoteHours.Text;
			JobQuotes.Update(jobQuote);
			Jobs.Update(_jobCur);
			Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,_jobCur.JobNum);
		}

		private void textQuoteAmount_Leave(object sender,EventArgs e) {
			if(_isLoading || IsNew) {
				return;
			}
			JobQuote jobQuote = _jobCur.ListJobQuotes.FirstOrDefault();
			jobQuote.Amount=textQuoteAmount.Text;
			JobQuotes.Update(jobQuote);
			Jobs.Update(_jobCur);
			Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,_jobCur.JobNum);
		}

		private void checkApproved_CheckedChanged(object sender,EventArgs e) {
			if(_isLoading || IsNew) {
				return;
			}
			JobQuote jobQuote = _jobCur.ListJobQuotes.FirstOrDefault();
			jobQuote.IsCustomerApproved=checkApproved.Checked;
			if(checkApproved.Checked) {
				_jobCur.DateTimeCustContact=DateTime.Now;
			}
			else {
				_jobCur.DateTimeCustContact=DateTime.MinValue;
			}
			textQuoteDate.Text=_jobCur.DateTimeCustContact.Year<1880?"":_jobCur.DateTimeCustContact.ToShortDateString();
			JobQuotes.Update(jobQuote);
			Jobs.Update(_jobCur);
			Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,_jobCur.JobNum);
		}

		private void textSchedDate_Leave(object sender,EventArgs e) {
			if(_isLoading || IsNew) {
				return;
			}
			DateTime date;
			if(!DateTime.TryParse(textSchedDate.Text,out date)) {
				MsgBox.Show(this,"Not a valid date");
			}
			else {
				_jobCur.AckDateTime=date;
			}
			textSchedDate.Text=_jobCur.AckDateTime.Year<1880?"":_jobCur.AckDateTime.ToString();
			Jobs.Update(_jobCur);
			Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,_jobCur.JobNum);
		}

		private void gridWatchers_TitleAddClick(object sender,EventArgs e) {
			if(_isLoading) {
				return;
			}
			if(_jobCur==null) {
				return;//should never happen
			}
			FormUserPick FormUP = new FormUserPick();
			//Suggest current user if not already watching.
			if(_jobCur.ListJobLinks.FindAll(x => x.LinkType==JobLinkType.Subscriber).All(x => x.FKey!=Security.CurUser.UserNum)) {
				FormUP.SuggestedUserNum=Security.CurUser.UserNum;
			}
			FormUP.IsSelectionmode=true;
			FormUP.ShowDialog();
			if(FormUP.DialogResult!=DialogResult.OK) {
				return;
			}
			JobLink jobLink = new JobLink() {
				FKey=FormUP.SelectedUserNum,
				JobNum=_jobCur.JobNum,
				LinkType=JobLinkType.Subscriber
			};
			if(!IsNew) {
				JobLinks.Insert(jobLink);
				Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,_jobCur.JobNum);
			}
			else {
				IsChanged=true;
			}
			_jobCur.ListJobLinks.Add(jobLink);
			FillGridSubscribers();
		}

		private void gridTasks_TitleAddClick(object sender,EventArgs e) {
			if(_isLoading) {
				return;
			}
			if(_jobCur==null) {
				return;//should never happen
			}
			if(!IsNew && Control.ModifierKeys == Keys.Shift) {
				Task task = new Task() {
					TaskListNum=-1,//don't show it in any list yet.
					UserNum=Security.CurUser.UserNum
				};
				Tasks.Insert(task);
				FormTaskEdit FormTE = new FormTaskEdit(task,task.Copy());
				JobLink jl = new JobLink();
				jl.JobNum=_jobCur.JobNum;
				jl.FKey=task.TaskNum;
				jl.LinkType=JobLinkType.Task;
				FormTE.FormClosing+=(o,ea) => {
					if(FormTE.DialogResult!=DialogResult.OK) {
						return;
					}
					if(Tasks.GetOne(jl.FKey)==null) {
						return;
					}
					JobLinks.Insert(jl);
					Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,jl.JobNum);
					if(this==null || this.IsDisposed) {
						return;//might be called after job manager has closed.
					}
					this.Invoke((Action)FillGridTasks);
				};
				FormTE.Show();
				return;
			}//end +Shift
			FormTaskSearch FormTS=new FormTaskSearch() {IsSelectionMode=true};
			FormTS.ShowDialog();
			if(FormTS.DialogResult!=DialogResult.OK) {
				return;
			}
			JobLink jobLink=new JobLink();
			jobLink.JobNum=_jobCur.JobNum;
			jobLink.FKey=FormTS.SelectedTaskNum;
			jobLink.LinkType=JobLinkType.Task;
			if(!IsNew) {
				JobLinks.Insert(jobLink);
				Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,_jobCur.JobNum);
			}
			else {
				IsChanged=true;
			}
			_jobCur.ListJobLinks.Add(jobLink);
			FillGridTasks();
		}

		private void gridAppointments_TitleAddClick(object sender,EventArgs e) {
			if(_isLoading) {
				return;
			}
			if(_jobCur.ListJobQuotes.FirstOrDefault().PatNum==0) {
				MsgBox.Show(this,"Customer must be attached to this query before attaching an appointment");
				return;
			}
			AddAppointment(_jobCur.ListJobQuotes.FirstOrDefault().PatNum);
		}

		private bool AddAppointment(long patNum) {
			if(_jobCur==null) {
				return false;//should never happen
			}
			FormApptsOther FormAO=new FormApptsOther(patNum);
			FormAO.SelectOnly=true;
			if(FormAO.ShowDialog()!=DialogResult.OK) {
				return false;
			}
			foreach(long aptNum in FormAO.AptNumsSelected) {
				JobLink jobLink = new JobLink();
				jobLink.JobNum=_jobCur.JobNum;
				jobLink.FKey=aptNum;
				jobLink.LinkType=JobLinkType.Appointment;
				if(!IsNew) {
					JobLinks.Insert(jobLink);
					Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,_jobCur.JobNum);
				}
				else {
					IsChanged=true;
				}
				_jobCur.ListJobLinks.Add(jobLink);
			}
			FillGridAppointments();
			return true;
		}

		private void gridFiles_TitleAddClick(object sender,EventArgs e) {
			if(_isLoading) {
				return;
			}
			if(_jobCur==null) {
				return;//should never happen
			}
			//Form to find file.
			OpenFileDialog formF = new OpenFileDialog();
			formF.Multiselect=true;
			if(formF.ShowDialog()!=DialogResult.OK) {
				return;
			}
			foreach(string filename in formF.FileNames) {
				JobLink jobLink = new JobLink();
				jobLink.JobNum=_jobCur.JobNum;
				jobLink.LinkType=JobLinkType.File;
				jobLink.Tag=filename;
				_jobCur.ListJobLinks.Add(jobLink);
				if(!IsNew) {
					JobLinks.Insert(jobLink);
				}
			}
			if(!IsNew) {
				Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,_jobCur.JobNum);
			}
			else {
				IsChanged=true;
			}
			FillGridFiles();
		}

		private void gridNotes_TitleAddClick(object sender,EventArgs e) {
			if(_isLoading) {
				return;
			}
			if(_jobCur==null) {
				return;//should never happen
			}
			JobNote jobNote=new JobNote() {
				DateTimeNote=MiscData.GetNowDateTime(),
				IsNew=true,
				JobNum=_jobCur.JobNum,
				UserNum=Security.CurUser.UserNum
			};
			FormJobNoteEdit FormJNE=new FormJobNoteEdit(jobNote);
			FormJNE.ShowDialog();
			if(FormJNE.DialogResult!=DialogResult.OK || FormJNE.JobNoteCur==null) {
				return;
			}
			if(!IsNew) {
				JobNotes.Insert(FormJNE.JobNoteCur);
				Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,_jobCur.JobNum);
			}
			else {
				IsChanged=true;
			}
			_jobCur.ListJobNotes.Add(FormJNE.JobNoteCur);
			FillGridNote();
		}

		private void gridTasks_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!(gridTasks.Rows[e.Row].Tag is long)) {
				return;//should never happen
			}
			//GoTo patietn will not work from this form. It would require a delegate to be passed in all the way from FormOpenDental.
			Task task=Tasks.GetOne((long)gridTasks.Rows[e.Row].Tag);
			FormTaskEdit FormTE=new FormTaskEdit(task,task.Copy());
			FormTE.Show();
			FormTE.FormClosing+=(o,ea) => {
				if(FormTE.DialogResult!=DialogResult.OK) {
					return;
				}
				if(this==null || this.IsDisposed) {
					return;
				}
				try { this.Invoke((Action)FillGridTasks); } catch(Exception) { }//If form disposed, this will catch.
			};
		}

		///<summary>SaveClick for both the Descritpion and Documentation</summary>
		private void textEditor_SaveClick(object sender,EventArgs e) {
			if(_isLoading) {
				return;
			}
			if(!ValidateJob(_jobCur)) {
				return;
			}
			SaveJob(_jobCur);
		}

		private void gridAppointments_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!(gridAppointments.Rows[e.Row].Tag is long)) {
				return;//should never happen.
			}
			try {
				FormApptEdit FormAE = new FormApptEdit((long)gridAppointments.Rows[e.Row].Tag);
				FormAE.Show();
				FormAE.FormClosing+=(o,ea) => {
					if(FormAE.DialogResult!=DialogResult.OK) {
						return;
					}
					if(this==null || this.IsDisposed) {
						return;
					}
					try {
						this.Invoke((Action)FillGridAppointments);
					}
					catch(Exception) { }//If form disposed, this will catch.
				};
			}
			catch {
				MsgBox.Show(this,"Appointment is most likely deleted. Please unlink this appointment from the job.");
			}
		}

		private void gridHistory_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(string.IsNullOrWhiteSpace(gridHistory.Rows[e.Row].Cells[4].Text) //because JobLog.MainRTF is not an empty string when it is blank.
				|| !(gridHistory.Rows[e.Row].Tag is JobLog)) 
			{
				return;
			}
			JobLog jobLog = (JobLog)gridHistory.Rows[e.Row].Tag;
			FormSpellChecker FormSC = new FormSpellChecker();
			FormSC.SetText(jobLog.MainRTF);
			FormSC.ShowDialog();
		}

		private void gridNotes_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(_isLoading) {
				return;
			}
			if(!(gridNotes.Rows[e.Row].Tag is JobNote)) {
				return;//should never happen.
			}
			JobNote jobNote = (JobNote)gridNotes.Rows[e.Row].Tag;
			FormJobNoteEdit FormJNE = new FormJobNoteEdit(jobNote);
			FormJNE.ShowDialog();
			if(FormJNE.DialogResult!=DialogResult.OK) {
				return;
			}
			if(IsNew) {
				IsChanged=true;
			}
			else {
				if(FormJNE.JobNoteCur==null) {
					JobNotes.Delete(jobNote.JobNoteNum);
				}
				else {
					JobNotes.Update(FormJNE.JobNoteCur);
				}
			}
			_jobCur.ListJobNotes.RemoveAll(x => x.JobNoteNum==jobNote.JobNoteNum);//should remove only one
			if(FormJNE.JobNoteCur!=null) {
				_jobCur.ListJobNotes.Add(FormJNE.JobNoteCur);
			}
			FillGridNote();
		}

		private void gridWatchers_CellClick(object sender,ODGridClickEventArgs e) {
			if(_isLoading) {
				return;
			}
			if(e.Button==MouseButtons.Right && (gridWatchers.Rows[e.Row].Tag is Userod)) {
				ContextMenu menu=new ContextMenu();
				long FKey = ((Userod)gridWatchers.Rows[e.Row].Tag).UserNum;
				menu.MenuItems.Add("Remove "+((Userod)gridWatchers.Rows[e.Row].Tag).UserName,(o,arg) => {
					List<JobLink> listLinks=_jobCur.ListJobLinks.FindAll(x => x.LinkType==JobLinkType.Subscriber&&x.FKey==FKey);//almost always only 1
					_jobCur.ListJobLinks.RemoveAll(x => x.LinkType==JobLinkType.Subscriber&&x.FKey==FKey);
					_jobOld.ListJobLinks.RemoveAll(x => x.LinkType==JobLinkType.Subscriber&&x.FKey==FKey);
					listLinks.Select(x=>x.JobLinkNum).ToList().ForEach(JobLinks.Delete);
					FillGridSubscribers();
					Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,_jobCur.JobNum);
				});
				menu.Show(gridWatchers,gridWatchers.PointToClient(Cursor.Position));
			}
		}

		private void gridTasks_CellClick(object sender,ODGridClickEventArgs e) {
			if(_isLoading) {
				return;
			}
			if(e.Button==MouseButtons.Right && (gridTasks.Rows[e.Row].Tag is long)) {
				ContextMenu menu = new ContextMenu();
				long FKey = (long)gridTasks.Rows[e.Row].Tag;
				menu.MenuItems.Add("Unlink Task",(o,arg) => {
					List<JobLink> listLinks = _jobCur.ListJobLinks.FindAll(x => x.LinkType==JobLinkType.Task&&x.FKey==FKey);
					_jobCur.ListJobLinks.RemoveAll(x => x.LinkType==JobLinkType.Task&&x.FKey==FKey);
					_jobOld.ListJobLinks.RemoveAll(x => x.LinkType==JobLinkType.Task&&x.FKey==FKey);
					listLinks.Select(x => x.JobLinkNum).ToList().ForEach(JobLinks.Delete);
					FillGridTasks();
					Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,_jobCur.JobNum);
				});
				menu.Show(gridTasks,gridTasks.PointToClient(Cursor.Position));
			}
		}

		private void gridAppointments_CellClick(object sender,ODGridClickEventArgs e) {
			if(_isLoading) {
				return;
			}
			if(e.Button==MouseButtons.Right && (gridAppointments.Rows[e.Row].Tag is long)) {
				ContextMenu menu = new ContextMenu();
				long FKey = (long)gridAppointments.Rows[e.Row].Tag;
				menu.MenuItems.Add("Unlink Appointment : "+FKey.ToString(),(o,arg) => {
					List<JobLink> listLinks = _jobCur.ListJobLinks.FindAll(x => x.LinkType==JobLinkType.Appointment&&x.FKey==FKey);
					_jobCur.ListJobLinks.RemoveAll(x => x.LinkType==JobLinkType.Appointment&&x.FKey==FKey);
					_jobOld.ListJobLinks.RemoveAll(x => x.LinkType==JobLinkType.Appointment&&x.FKey==FKey);
					listLinks.Select(x => x.JobLinkNum).ToList().ForEach(JobLinks.Delete);
					FillGridAppointments();
					Signalods.SetInvalid(InvalidType.Jobs,KeyType.Job,_jobCur.JobNum);
				});
				menu.Show(gridAppointments,gridAppointments.PointToClient(Cursor.Position));
			}
		}

		private void gridFiles_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(_isLoading) {
				return;
			}
			if(!(gridFiles.Rows[e.Row].Tag is JobLink)) {
				return;
			}
			try {
				System.Diagnostics.Process.Start(((JobLink)gridFiles.Rows[e.Row].Tag).Tag);
			}
			catch(Exception ex) {
				ex.DoNothing();
				MessageBox.Show("Unable to open file.");
			}
		}

		private void checkShowHistoryText_CheckedChanged(object sender,EventArgs e) {
			FillGridHistory();
		}

		///<summary>Adds error checking to the input parameters for JobLogs.MakeLogEntry also inserts joblog into the UI and _jobCur.ListJobLog if returned.</summary>
		private void MakeLogEntry(Job jobNew,Job jobOld) {
			JobLog jobLog = JobLogs.MakeLogEntry(jobNew,jobOld);
			if(jobLog==null) {
				return;
			}
			_jobCur.ListJobLogs.Add(jobLog);
			FillGridHistory();
		}

		///<summary>Does nothing, just used to make those annoying compile warnings go away.</summary>
		private void DoNothing() {
			var x = new object[] { TitleChanged,JobOverride };//simplest way to get rid of the "variable assigned but not used" warning.
		}
	}//end class

}//end namespace
	

