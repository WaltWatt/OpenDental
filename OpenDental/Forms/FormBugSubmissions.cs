using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Linq;
using CodeBase;
using System.Text.RegularExpressions;
using System.Text;

namespace OpenDental {
	public partial class FormBugSubmissions:ODForm {
	
		///<summary>List of all bugSubmissons from the bugs DB.</summary>
		private List<BugSubmission> _listAllSubs;
		///<summary>When FormBugSumissionMode.AddBug, form will close after adding a bug.
		///When FormBugSumissionMode.ViewOnly, the "Add Bug" button is not visable.
		///When FormBugSumissionMode.SelectionMode, the "Add Bug" is changed to "Ok".</summary>
		private FormBugSubmissionMode _viewMode;
		///<summary>Used to determine if a new bug should show (Enhancement) in the description.</summary>
		private Job _jobCur;
		///<summary>Null unless a bug is added when _viewMode is FormBugSumissionMode.AddBug.</summary>
		public Bug BugCur;
		///<summary>List of selected bugSubmissions when _viewMode is FormBugSumissionMode.SelectionMode.</summary>
		public List<BugSubmission> ListSelectedSubs=new List<BugSubmission>();
		///<summary>List of bugSubmissions to view when _viewMode is FormBugSumissionMode.ViewMode.</summary>
		public List<BugSubmission> ListViewedSubs=new List<BugSubmission>();
		///<summary>Dictionary of patients that will lazy load as users click on entries.  The key is the Registration Key.</summary>
		private Dictionary<string,Patient> _dictPatients=new Dictionary<string, Patient>();
		///<summary>The current patient associated to the selected bug submission row. Null if no row selected or if multiple rows selected.</summary>
		private Patient _patCur;
		///<summary>BugSubmission from the currently selected submission in either gridSubs or gridCustomerSubs if any, otherwise null.</summary>
		private BugSubmission _subCur;
		
		///<summary>Set job if you would like to create a bug with (Enhancement) in the bug text.
		///When isViewOnlyMode is true, you will not be able to create a bug.
		///When isSelectedMode is true, the form will close after a double click selection or a group selection.</summary>
		public FormBugSubmissions(Job job=null,FormBugSubmissionMode viewMode=FormBugSubmissionMode.AddBug) {
			InitializeComponent();
			Lan.F(this);
			_jobCur=job;
			_viewMode=viewMode;
		}

		private void FormBugSubmissions_Load(object sender,EventArgs e) {
			switch(_viewMode) {
				case FormBugSubmissionMode.AddBug:
					dateRangePicker.SetDateTimeFrom(DateTime.Today.AddDays(-60));
					dateRangePicker.SetDateTimeTo(DateTime.Today);
					break;
				case FormBugSubmissionMode.ViewOnly:
					dateRangePicker.SetDateTimeFrom(DateTime.MinValue);
					dateRangePicker.SetDateTimeTo(DateTime.MaxValue.AddDays(-1));//Subtract a day for DbHelper.DateTConditionColumn(...)
					butAddJob.Visible=false;
					checkShowAttached.Checked=true;
					break;
				case FormBugSubmissionMode.SelectionMode:
					dateRangePicker.SetDateTimeFrom(DateTime.MinValue);
					dateRangePicker.SetDateTimeTo(DateTime.MaxValue.AddDays(-1));//Subtract a day for DbHelper.DateTConditionColumn(...)
					butAddJob.Text="OK";//On click the selected rows are saved and this form will close.
					break;
				case FormBugSubmissionMode.ValidationMode:
					dateRangePicker.SetDateTimeFrom(DateTime.MinValue);
					dateRangePicker.SetDateTimeTo(DateTime.MaxValue.AddDays(-1));//Subtract a day for DbHelper.DateTConditionColumn(...)
					butAddJob.Text="OK";
					checkShowAttached.Checked=true;
					groupFilters.Enabled=false;
					break;
			}
			bugSubmissionControl.TextDevNoteLeave+=textDevNote_PostLeave;
			#region comboGrouping
			comboGrouping.Items.Add("None");
			comboGrouping.Items.Add("RegKey/Ver/Stack");
			comboGrouping.Items.Add("StackTrace");
			comboGrouping.Items.Add("95%");
			switch(_viewMode) {
				case FormBugSubmissionMode.AddBug:
					comboGrouping.SelectedIndex=2;//Default to StackTrace.
					break;
				case FormBugSubmissionMode.SelectionMode:
				case FormBugSubmissionMode.ValidationMode:
				case FormBugSubmissionMode.ViewOnly:
					comboGrouping.SelectedIndex=0;//Default to None.
					break;
			}
			#endregion
			#region comboSortBy
			comboSortBy.Items.Add("Vers./Count");
			comboSortBy.SelectedIndex=0;//Default to Vers./Count
			#endregion
			#region Right Click Menu
			ContextMenu gridSubMenu=new ContextMenu();
			Menu.MenuItemCollection menuItemCollection=new Menu.MenuItemCollection(gridSubMenu);
			List<MenuItem> listMenuItems=new List<MenuItem>();
			listMenuItems.Add(new MenuItem(Lan.g(this,"Open Submission"),new EventHandler(gridClaimDetails_RightClickHelper)));
			listMenuItems.Add(new MenuItem(Lan.g(this,"Open Bug"),new EventHandler(gridClaimDetails_RightClickHelper)));//Enabled by default
			menuItemCollection.AddRange(listMenuItems.ToArray());
			gridSubMenu.Popup+=new EventHandler((o,ea) => {
				int index=gridSubs.GetSelectedIndex();
				bool isOpenSubEnabled=false;
				bool isOpenBugEnabled=false;
				if(index!=-1 && gridSubs.SelectedIndices.Count()==1) {
					BugSubmission bugSub=((List<BugSubmission>)gridSubs.Rows[index].Tag).First();
					isOpenSubEnabled=true;
					isOpenBugEnabled=(bugSub.BugId!=0);
				}
				gridSubMenu.MenuItems[0].Enabled=isOpenSubEnabled;//Open Submission
				gridSubMenu.MenuItems[1].Enabled=isOpenBugEnabled;//Open Bug
			});
			gridSubs.ContextMenu=gridSubMenu;
			#endregion
			FillSubGrid(true);
		}
		
		private void findPreviouslyFixedSubmisisonsToolStripMenuItem_Click(object sender,EventArgs e) {
			if(!BugSubmissionL.TryAssociateSimilarBugSubmissions(this.Location)) {
				return;
			}
			FillSubGrid(true);
		}

		///<summary>Click method used by all gridClaimDetails right click options.</summary>
		private void gridClaimDetails_RightClickHelper(object sender,EventArgs e) {
			int index=gridSubs.GetSelectedIndex();
			if(index==-1) {//Should not happen, menu item is only enabled when exactly 1 row is selected.
				return;
			}
			List<BugSubmission> listSubs=(List<BugSubmission>)gridSubs.Rows[index].Tag;
			switch(((MenuItem)sender).Index) {
				case 0://Open Submission
					FormBugSubmission formBugSub=new FormBugSubmission(listSubs[0],_jobCur);
					formBugSub.Show();
				break;
				case 1://Open Bug
					OpenBug(listSubs[0]);
				break;
			}
		}

		private void FillSubGrid(bool isRefreshNeeded=false,string grouping95="") {
			Action loadingProgress=null;
			Cursor=Cursors.WaitCursor;
			bugSubmissionControl.ClearCustomerInfo();
			bugSubmissionControl.SetTextDevNoteEnabled(false);
			if(isRefreshNeeded) {
				loadingProgress=ODProgressOld.ShowProgressStatus("FormBugSubmissions",this,Lan.g(this,"Refreshing Data")+"...",false);
				#region Refresh Logic
				if(_viewMode.In(FormBugSubmissionMode.ViewOnly,FormBugSubmissionMode.ValidationMode)) {
					_listAllSubs=ListViewedSubs;
				}
				else {
					_listAllSubs=BugSubmissions.GetAllInRange(dateRangePicker.GetDateTimeFrom(),dateRangePicker.GetDateTimeTo());
				}
				try {
					_dictPatients=RegistrationKeys.GetPatientsByKeys(_listAllSubs.Select(x => x.RegKey).ToList());
				}
				catch(Exception e) {
					e.DoNothing();
					_dictPatients=new Dictionary<string, Patient>();
				}
				#endregion
			}
			gridSubs.BeginUpdate();
			#region gridSubs columns
			gridSubs.Columns.Clear();
			gridSubs.Columns.Add(new ODGridColumn("Submitter",140));
			gridSubs.Columns.Add(new ODGridColumn("Vers.",55,GridSortingStrategy.VersionNumber));
			if(comboGrouping.SelectedIndex==0) {//Group by 'None'
				gridSubs.Columns.Add(new ODGridColumn("DateTime",75,GridSortingStrategy.DateParse));
			}
			else {
				gridSubs.Columns.Add(new ODGridColumn("Count",75,GridSortingStrategy.AmountParse));
			}
			gridSubs.Columns.Add(new ODGridColumn("HasBug",50,HorizontalAlignment.Center));
			gridSubs.Columns.Add(new ODGridColumn("Msg Text",300));
			gridSubs.AllowSortingByColumn=true;
			#endregion
			#region Filter Logic
			ODEvent.Fire(new ODEventArgs("FormBugSubmissions","Filtering Data"));
			List<string> listSelectedVersions=comboVersions.ListSelectedItems.Select(x => (string)x).ToList();
			if(listSelectedVersions.Contains("All")) {
				listSelectedVersions.Clear();
			}
			List<string> listSelectedRegKeys=comboRegKeys.ListSelectedItems.Select(x => (string)x).ToList();
			if(listSelectedRegKeys.Contains("All")) {
				listSelectedRegKeys.Clear();
			}
			List<string> listStackFilters=textStackFilter.Text.Split(',')
				.Where(x => !string.IsNullOrWhiteSpace(x))
				.Select(x => x.ToLower()).ToList();
			List<string> listPatNumFilters=textPatNums.Text.Split(',')
				.Where(x => !string.IsNullOrWhiteSpace(x))
				.Select(x => x.ToLower()).ToList();
			_listAllSubs.ForEach(x => x.TagOD=null);
			List<BugSubmission> listFilteredSubs=_listAllSubs.Where(x => 
				PassesFilterValidation(x,listSelectedRegKeys,listStackFilters,listPatNumFilters,listSelectedVersions,grouping95)
			).ToList();
			if(isRefreshNeeded) {
				FillVersionsFilter(listFilteredSubs);
				FillRegKeyFilter(listFilteredSubs);
			}
			#endregion
			#region Grouping Logic
			List<BugSubmission> listGroupedSubs;
			int index=0;
			List<BugSubmission> listGridSubmissions=new List<BugSubmission>();
			foreach(BugSubmission sub in listFilteredSubs) {
				ODEvent.Fire(new ODEventArgs("FormBugSubmissions","Grouping Data: "+POut.Double(((double)index++/(double)listFilteredSubs.Count)*100)+"%"));
				if(sub.TagOD!=null) {
					continue;
				}
				switch(comboGrouping.SelectedIndex) {
					case 0:
						#region None
						sub.TagOD=new List<BugSubmission>() { sub };//Tag is a specific bugSubmission
						listGridSubmissions.Add(sub.Copy());
						#endregion
						break;
					case 1:
						#region RegKey/Ver/Stack
						listGroupedSubs=listFilteredSubs.FindAll(x => x.TagOD==null && x.RegKey==sub.RegKey
							&& x.Info.DictPrefValues[PrefName.ProgramVersion]==sub.Info.DictPrefValues[PrefName.ProgramVersion]
							&& x.ExceptionStackTrace==sub.ExceptionStackTrace
							&& x.BugId==sub.BugId);
						if(listGroupedSubs.Count==0) {
							continue;
						}
						listGroupedSubs=listGroupedSubs.OrderByDescending(x => new Version(x.Info.DictPrefValues[PrefName.ProgramVersion]))
							.ThenByDescending(x => x.SubmissionDateTime).ToList();
						listGroupedSubs.ForEach(x => x.TagOD=true);//So we don't considered previously handled submissions.
						listGroupedSubs.First().TagOD=listGroupedSubs;//First element is what is shown in grid, still wont be considered again.
						listGridSubmissions.Add(listGroupedSubs.First().Copy());
						#endregion
						break;
					case 2:
						#region StackTrace
						listGroupedSubs=listFilteredSubs.FindAll(x => x.TagOD==null && x.ExceptionStackTrace==sub.ExceptionStackTrace && x.BugId==sub.BugId);
						if(listGroupedSubs.Count==0) {
							continue;
						}
						listGroupedSubs=listGroupedSubs.OrderByDescending(x => new Version(x.Info.DictPrefValues[PrefName.ProgramVersion]))
							.ThenByDescending(x => x.SubmissionDateTime).ToList();
						listGroupedSubs.ForEach(x => x.TagOD=true);//So we don't considered previously handled submissions.
						listGroupedSubs.First().TagOD=listGroupedSubs;//First element is what is shown in grid, still wont be considered again.
						listGridSubmissions.Add(listGroupedSubs.First().Copy());
						#endregion
						break;
					case 3:
						#region 95%
						//At this point all bugSubmissions in listFilteredSubs is at least a 95% match. Group them all together in a single row.
						listGroupedSubs=listFilteredSubs;
						listGroupedSubs=listGroupedSubs.OrderByDescending(x => new Version(x.Info.DictPrefValues[PrefName.ProgramVersion]))
							.ThenByDescending(x => x.SubmissionDateTime).ToList();
						listGroupedSubs.ForEach(x => x.TagOD=true);//So we don't considered previously handled submissions.
						listGroupedSubs.First().TagOD=listGroupedSubs;//First element is what is shown in grid, still wont be considered again.
						listGridSubmissions.Add(listGroupedSubs.First().Copy());
						#endregion
						break;
				}
			}
			#endregion
			#region Sorting Logic
			ODEvent.Fire(new ODEventArgs("FormBugSubmissions","Sorting Data"));
			switch(comboSortBy.SelectedIndex) {
				case 0:
					listGridSubmissions=listGridSubmissions.OrderByDescending(x => new Version(x.Info.DictPrefValues[PrefName.ProgramVersion]))
						.ThenByDescending(x => GetGroupCount(x))
						.ThenByDescending(x => x.SubmissionDateTime).ToList();
					break;
			}
			#endregion
			#region Fill gridSubs
			gridSubs.Rows.Clear();
			index=0;
			foreach(BugSubmission sub in listGridSubmissions){
				ODEvent.Fire(new ODEventArgs("FormBugSubmissions","Filling Grid: "+POut.Double(((double)index++/(double)listFilteredSubs.Count)*100)+"%"));
				gridSubs.Rows.Add(GetODGridRowForSub(sub));
			}
			gridSubs.EndUpdate();
			#endregion
			try {
				loadingProgress?.Invoke();//When this function executes quickly this can fail rarely, fail silently because of WaitCursor.
			}
			catch(Exception ex) {
				ex.DoNothing();
			}
			Cursor=Cursors.Default;
		}

		private ODGridRow GetODGridRowForSub(BugSubmission sub) {
			ODGridRow row=new ODGridRow();
			row.Cells.Add(_dictPatients.ContainsKey(sub.RegKey)?_dictPatients[sub.RegKey].GetNameLF():sub.RegKey);
			List<BugSubmission> listGroupedSubs=(sub.TagOD as List<BugSubmission>);
			row.Cells.Add(listGroupedSubs.First().Info.DictPrefValues[PrefName.ProgramVersion]);
			switch(comboGrouping.SelectedIndex) {
				case 0://None
					row.Cells.Add(sub.SubmissionDateTime.ToString().Replace('\r',' '));
					break;
				case 1://Customer
				case 2://StackTrace
				case 3://95%
					row.Cells.Add(listGroupedSubs.Count.ToString());
					break;
			}
			row.Cells.Add(sub.BugId==0 ? "" : "X");
			row.Cells.Add(sub.ExceptionMessageText+(string.IsNullOrEmpty(sub.DevNote)?"":"\r\n\r\nDevNote: "+sub.DevNote));
			row.Tag=listGroupedSubs;//Tag is a list of bugSubmissions, even if no grouping.
			return row;
		}

		private bool PassesFilterValidation(BugSubmission sub,List<string> listSelectedRegKeys,
			List<string> listStackFilters,List<string> listPatNumFilters,List<string> listSelectedVersions,string grouping95)
		{
			if(!_viewMode.In(FormBugSubmissionMode.ValidationMode,FormBugSubmissionMode.ViewOnly) 
					&& ((!string.IsNullOrWhiteSpace(textMsgText.Text)&&!sub.ExceptionMessageText.ToLower().Contains(textMsgText.Text.ToLower()))
					||(listSelectedRegKeys.Count!=0 && !listSelectedRegKeys.Contains(sub.RegKey))
					||(listStackFilters.Count!=0 && !listStackFilters.Exists(x => sub.ExceptionStackTrace.ToLower().Contains(x)))
					||(listPatNumFilters.Count!=0 && !listPatNumFilters.Exists(x => x==_dictPatients[sub.RegKey].PatNum.ToString()))
					||(sub.BugId!=0 && !checkShowAttached.Checked)
					||(checkExcludeHQ.Checked && (_dictPatients[sub.RegKey].BillingType==436||_dictPatients[sub.RegKey].PatNum==1486))//436 is "Internal Use" def, 1486 is HQ patNum.
					||(listSelectedVersions.Count!=0 && !listSelectedVersions.Contains(sub.Info.DictPrefValues[PrefName.ProgramVersion]))
					||(!sub.SubmissionDateTime.Between(dateRangePicker.GetDateTimeFrom(),dateRangePicker.GetDateTimeTo()))
					||(!string.IsNullOrWhiteSpace(textDevNoteFilter.Text) && !sub.DevNote.ToLower().Contains(textDevNoteFilter.Text.ToLower()))
					||(!string.IsNullOrEmpty(grouping95) && BugSubmissionL.CalculateSimilarity(grouping95,sub.ExceptionStackTrace)<95)))
			{
				return false;
			}
			return true;
		}

		private int GetGroupCount(BugSubmission sub) {
			return (sub.TagOD as List<BugSubmission>).Count;
		}
		
		private void FillVersionsFilter(List<BugSubmission> listSubmissions) {
			comboVersions.Items.Clear();
			comboVersions.Items.Add("All");
			List<string> listVersions=listSubmissions.Select(x => x.Info.DictPrefValues[PrefName.ProgramVersion])
				.Distinct()
				.ToList();
			listVersions.Sort((x,y) => new Version(y).CompareTo(new Version(x)));//descending
			listVersions.ForEach(x => comboVersions.Items.Add(x));
			if(comboVersions.SelectedIndices.Count==0) {
				comboVersions.SetSelected(0,true);//Select 'All' by default
			}
		}
		
		private void FillRegKeyFilter(List<BugSubmission> listSubmissions) {
			comboRegKeys.Items.Clear();
			comboRegKeys.Items.Add("All");
			List<string> listVersions=listSubmissions.Select(x => x.RegKey)
				.Distinct()
				.ToList();
			listVersions.Sort();
			listVersions.ForEach(x => comboRegKeys.Items.Add(x));
			if(comboRegKeys.SelectedIndices.Count==0) {
				comboRegKeys.SetSelected(0,true);//Select 'All' by default
			}
		}

		private void gridSubs_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(_viewMode==FormBugSubmissionMode.ViewOnly) {//Not allowed to create a bug.
				return;
			}
			List<BugSubmission> listSubs=(List<BugSubmission>)gridSubs.Rows[e.Row].Tag;//Because it is a double click, we know there will only be 1 item in list
			if(_viewMode==FormBugSubmissionMode.SelectionMode) {
				ListSelectedSubs=listSubs;
				DialogResult=DialogResult.OK;
				return;
			}
			//The only time listSubs will have more than 1 item in it is when grouping.
			//The grouping logic ensures that all grouped items have the same bugid
			if(listSubs[0].BugId!=0) {
				OpenBug(listSubs[0]);
			}
			else {
				FormBugSubmission formBugSub=new FormBugSubmission(listSubs[0],_jobCur);
				formBugSub.Show();
			}
		}

		private void OpenBug(BugSubmission sub) {
			if(!JobPermissions.IsAuthorized(JobPerm.Concept,true)
				&& !JobPermissions.IsAuthorized(JobPerm.NotifyCustomer,true)
				&& !JobPermissions.IsAuthorized(JobPerm.FeatureManager,true)
				&& !JobPermissions.IsAuthorized(JobPerm.Documentation,true)) 
			{
				return;
			}
			FormBugEdit FormBE=new FormBugEdit();
			FormBE.BugCur=Bugs.GetOne(sub.BugId);
			if(FormBE.ShowDialog()==DialogResult.OK && FormBE.BugCur==null) {//Bug was deleted.
				FillSubGrid(true);
			}
		}
		
		private void gridSubs_CellClick(object sender,UI.ODGridClickEventArgs e) {
			butAddJob.Text="Add Job";//Always reset
			if(e.Row==-1 || gridSubs.SelectedIndices.Length!=1) {
				bugSubmissionControl.ClearCustomerInfo();
				_subCur=null;
				bugSubmissionControl.SetTextDevNoteEnabled(false);
				return;
			}
			bugSubmissionControl.SetTextDevNoteEnabled(true);
			_subCur=((List<BugSubmission>)gridSubs.Rows[e.Row].Tag)[0];
			if(_dictPatients.ContainsKey(_subCur.RegKey)) {
				_patCur=_dictPatients[_subCur.RegKey];
			}
			else {
				try {
					RegistrationKey key=RegistrationKeys.GetByKey(_subCur.RegKey);
					_patCur=Patients.GetPat(key.PatNum);

				}
				catch(Exception ex) {
					ex.DoNothing();
					_patCur=new Patient();//Just in case, needed mostly for debug.
				}
				_dictPatients.Add(_subCur.RegKey,_patCur);
			}
			List<BugSubmission> listSubs=_listAllSubs;
			if(comboGrouping.SelectedIndex.In(1,2,3)) {
				listSubs=((List<BugSubmission>)gridSubs.Rows[gridSubs.GetSelectedIndex()].Tag);
			}
			butAddJob.Tag=null;
			bugSubmissionControl.RefreshData(_dictPatients,comboGrouping.SelectedIndex,listSubs);//New selelction, refresh control data.
			bugSubmissionControl.RefreshView(_subCur);
			if(_subCur.BugId!=0) {
				List<JobLink> _listLinks=JobLinks.GetForType(JobLinkType.Bug,_subCur.BugId);
				if(_listLinks.Count==1) {
					butAddJob.Text="View Job";
					butAddJob.Tag=_listLinks.First();
				}
			}
			if(_viewMode.In(FormBugSubmissionMode.SelectionMode,FormBugSubmissionMode.ValidationMode)) {
				butAddJob.Text="OK";
			}
		}
		
		public void textDevNote_PostLeave(object sender,EventArgs e){
			if(_subCur.TagOD is List<BugSubmission> && gridSubs.SelectedIndices.Count()>0) {//If _subCur is set from gridCustomerSubs then do not update row because dev note is not shown.
				int index=gridSubs.SelectedIndices[0];
				gridSubs.BeginUpdate();
				gridSubs.Rows[gridSubs.SelectedIndices[0]]=GetODGridRowForSub(_subCur);
				gridSubs.EndUpdate();
				gridSubs.SetSelected(index,true);
			}	
		}
		
		private void dateRangePicker_CalendarClosed(object sender,EventArgs e) {
			FillSubGrid(true);//Refresh _listAllSubs
		}

		private void comboVersions_SelectionChangeCommitted(object sender,EventArgs e) {
			string group95Matching="";
			if(sender==comboGrouping && comboGrouping.SelectedIndex==3) {//95%
				InputBox input=new InputBox("Paste the stack trace you wish to match against.",true);
				if(input.ShowDialog()!=DialogResult.OK) {
					return;
				}
				group95Matching=input.textResult.Text;
			}
			FillSubGrid(grouping95:group95Matching);
		}
		
		private void Filters_TextChanged(object sender,EventArgs e) {
			FillSubGrid();
		}
		
		private void checkBox_CheckedChanged(object sender,EventArgs e) {
			FillSubGrid();
		}
		
		private void butRefresh_Click(object sender,EventArgs e) {
			FillSubGrid(true);//Refresh _listAllSubs
		}

		private void butAdd_Click(object sender,EventArgs e) {
			if(_viewMode==FormBugSubmissionMode.SelectionMode) {//Text is set to "Ok" when SelectionMode
				ListSelectedSubs=gridSubs.SelectedIndices.SelectMany(x => (List<BugSubmission>)gridSubs.Rows[x].Tag).ToList();
				DialogResult=DialogResult.OK;
				return;
			}
			if(_viewMode==FormBugSubmissionMode.ValidationMode) {//Text is set to "Ok" when SelectionMode
				ListSelectedSubs=_listAllSubs;
				DialogResult=DialogResult.OK;
				return;
			}
			if(butAddJob.Text=="View Job" && butAddJob.Tag is JobLink) {//Assocaited to job, see gridSubs_CellClick(...)	
				FormOpenDental.S_GoToJob((butAddJob.Tag as JobLink).JobNum);
				return;
			}
			List<BugSubmission> listSelectedSubs=gridSubs.SelectedIndices.SelectMany(x => (List<BugSubmission>)gridSubs.Rows[x].Tag).ToList();
			BugCur=BugSubmissionL.AddBugAndJob(this,listSelectedSubs,_patCur);
			if(BugCur==null) {
				return;
			}
			if(this.Modal) {
				this.DialogResult=DialogResult.OK;
			}
			else {
				FillSubGrid(true);
			}
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
			Close();
		}

	}

	///<summary>Enum controling the way the form displays and behaves.</summary>
	public enum FormBugSubmissionMode {
		///<summary>This is the default way for the form to load. Used by job manager to add bugs</summary>
		AddBug,
		///<summary>Used when we wish to simply view the bug submissions, does not allow users to add bugs. Filter validation is skipped.</summary>
		ViewOnly,
		///<summary>Used when attaching bug submissions to exiting bugs. Changed butAdd to show "OK" and return selected rows.</summary>
		SelectionMode,
		///<summary>Used when using the similiar bugs tool. Changed butAdd to show "OK" and returns all BugSubmissions in the grid on Ok click. Filter validation is skipped.</summary>
		ValidationMode,
	}
}