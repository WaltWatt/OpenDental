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
			dateRangePicker.SetDateTimeFrom(DateTime.Today.AddDays(-60));
			dateRangePicker.SetDateTimeTo(DateTime.Today);
			#region comboGrouping
			comboGrouping.Items.Add("None");
			comboGrouping.Items.Add("Customer");
			comboGrouping.Items.Add("StackTrace");
			//comboGrouping.Items.Add("95%");
			comboGrouping.SelectedIndex=0;//Default to None.
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
			if(_viewMode==FormBugSubmissionMode.ViewOnly) {
				butAddBug.Visible=false;
				checkShowAttached.Checked=true;
			}
			else if(_viewMode==FormBugSubmissionMode.SelectionMode) {
				butAddBug.Text="OK";//On click the selected rows are saved and this form will close.
			}
			else if(_viewMode==FormBugSubmissionMode.ValidationMode) {
				butAddBug.Text="OK";
				checkShowAttached.Checked=true;
				groupFilters.Enabled=false;
			}
		}
		
		private void findPreviouslyFixedSubmisisonsToolStripMenuItem_Click(object sender,EventArgs e) {
			if(!BugSubmissionL.TryAssociateSimilarBugSubmissions(_listAllSubs,this.Location)) {
				return;
			}
			FillSubGrid();
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

		private void FillSubGrid(bool isRefreshNeeded=false) {
			SetCustomerInfo();
			if(isRefreshNeeded) {
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
			}
			gridSubs.BeginUpdate();
			gridSubs.Columns.Clear();
			gridSubs.Columns.Add(new ODGridColumn("Reg Key",140));
			gridSubs.Columns.Add(new ODGridColumn("Vers.",55,GridSortingStrategy.VersionNumber));
			if(comboGrouping.SelectedIndex==0) {//None
				gridSubs.Columns.Add(new ODGridColumn("DateTime",75,GridSortingStrategy.DateParse));
			}
			else {
				gridSubs.Columns.Add(new ODGridColumn("Count",75,GridSortingStrategy.AmountParse));
			}
			gridSubs.Columns.Add(new ODGridColumn("HasBug",50,HorizontalAlignment.Center));
			gridSubs.Columns.Add(new ODGridColumn("Msg Text",300));
			gridSubs.AllowSortingByColumn=true;
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
			gridSubs.Rows.Clear();
			_listAllSubs.ForEach(x => x.TagOD=null);
			List<BugSubmission> listFilteredSubs=_listAllSubs.Where(x => 
				PassesFilterValidation(x,listSelectedRegKeys,listStackFilters,listPatNumFilters,listSelectedVersions)
			).ToList();
			foreach(BugSubmission sub in listFilteredSubs){
				ODGridRow row=new ODGridRow();
				row.Cells.Add(sub.RegKey);
				row.Cells.Add(sub.Info.DictPrefValues[PrefName.ProgramVersion]);
				List<BugSubmission> listPreviousRows;
				List<BugSubmission> listGroupedSubs;
				List<string> listProgVersions;
				switch(comboGrouping.SelectedIndex) {
					case 0:
						#region None
						row.Cells.Add(sub.SubmissionDateTime.ToString().Replace('\r',' '));
						row.Tag=new List<BugSubmission>() { sub };//Tag is a specific bugSubmission
						#endregion
						break;
					case 1:
						#region Customer
						//Take previously proccessed rows and see if we have already handled the grouping.
						listPreviousRows=listFilteredSubs.Take(listFilteredSubs.IndexOf(sub)).ToList();
						if(listPreviousRows.Any(x => x.RegKey==sub.RegKey
								&&x.Info.DictPrefValues[PrefName.ProgramVersion]==sub.Info.DictPrefValues[PrefName.ProgramVersion]
								&&x.ExceptionStackTrace==sub.ExceptionStackTrace
								&&x.BugId==sub.BugId)) {
							//Skip adding rows that have already been added when grouping
							continue;
						}
						listGroupedSubs=listFilteredSubs.FindAll(x => x.RegKey==sub.RegKey
							&&x.Info.DictPrefValues[PrefName.ProgramVersion]==sub.Info.DictPrefValues[PrefName.ProgramVersion]
							&&x.ExceptionStackTrace==sub.ExceptionStackTrace
							&&x.BugId==sub.BugId);
						row.Cells.Add(listGroupedSubs.Count.ToString());
						//row.Cells[1].Text=string.Join(",",listGroupedSubs.Select(x => x.Info.DictPrefValues[PrefName.ProgramVersion]).ToList());
						listProgVersions=listGroupedSubs.Select(x => x.Info.DictPrefValues[PrefName.ProgramVersion]).Distinct().ToList();
						if(listProgVersions.Count>1) {
							row.Cells[1].Text=listProgVersions.Select(x => new Version(x)).Max().ToString();
						}
						else {
							row.Cells[1].Text=listProgVersions.First();
						}
						row.Tag=listGroupedSubs;//Tag is a list of bugSubmissions
						#endregion
						break;
					case 2:
						#region StackTrace
						//Take previously proccessed rows and see if we have already handled the grouping.
						listPreviousRows=listFilteredSubs.Take(listFilteredSubs.IndexOf(sub)).ToList();
						if(listPreviousRows.Any(x => x.ExceptionStackTrace==sub.ExceptionStackTrace
								&&x.BugId==sub.BugId)) {
							//Skip adding rows that have already been added when grouping
							continue;
						}
						listGroupedSubs=listFilteredSubs.FindAll(x => x.ExceptionStackTrace==sub.ExceptionStackTrace
							&&x.BugId==sub.BugId);
						row.Cells.Add(listGroupedSubs.Count.ToString());
						listProgVersions=listGroupedSubs.Select(x => x.Info.DictPrefValues[PrefName.ProgramVersion]).Distinct().ToList();
						if(listProgVersions.Count>1) {
							row.Cells[1].Text=listProgVersions.Select(x => new Version(x)).Max().ToString();
						}
						else {
							row.Cells[1].Text=listProgVersions.First();
						}
						row.Tag=listGroupedSubs;//Tag is a list of bugSubmissions
						#endregion
						break;
					case 3:
						#region 95%
						//if(sub.TagOD!=null) {
						//	continue;
						//}
						//listGroupedSubs=listFilteredSubs.FindAll(x => x.BugId==sub.BugId
						//	&&x.TagOD==null
						//	&&CalculateSimilarity(x.ExceptionMessageText,sub.ExceptionMessageText)>95
						//	&&(x==sub||CalculateSimilarity(x.ExceptionStackTrace,sub.ExceptionStackTrace)>95));
						//listGroupedSubs.ForEach(x => x.TagOD=true);
						//row.Cells.Add(listGroupedSubs.Count.ToString());
						//listProgVersions=listGroupedSubs.Select(x => x.Info.DictPrefValues[PrefName.ProgramVersion]).Distinct().ToList();
						//if(listProgVersions.Count>1) {
						//	row.Cells[1].Text=listProgVersions.Select(x => new Version(x)).Max().ToString();
						//}
						//else {
						//	row.Cells[1].Text=listProgVersions.First();
						//}
						//row.Tag=listGroupedSubs;//Tag is a list of bugSubmissions
						#endregion
						break;
				}
				row.Cells.Add(sub.BugId==0 ? "" : "X");
				row.Cells.Add(sub.ExceptionMessageText);
				gridSubs.Rows.Add(row);
			}
			gridSubs.EndUpdate();
			gridSubs.SortForced(1,false);//Sort by Version column
			if(isRefreshNeeded) {
				FillVersionsFilter();
				FillRegKeyFilter();
			}
		}

		private bool PassesFilterValidation(BugSubmission sub,List<string> listSelectedRegKeys,
			List<string> listStackFilters,List<string> listPatNumFilters,List<string> listSelectedVersions)
		{
			if((!string.IsNullOrWhiteSpace(textMsgText.Text)&&!sub.ExceptionMessageText.ToLower().Contains(textMsgText.Text.ToLower()))
					||(listSelectedRegKeys.Count!=0 && !listSelectedRegKeys.Contains(sub.RegKey))
					||(listStackFilters.Count!=0 && !listStackFilters.Exists(x => sub.ExceptionStackTrace.ToLower().Contains(x)))
					||(listPatNumFilters.Count!=0 && !listPatNumFilters.Exists(x => x==_dictPatients[sub.RegKey].PatNum.ToString()))
					||(sub.BugId!=0 && !checkShowAttached.Checked)
					||(checkExcludeHQ.Checked && (_dictPatients[sub.RegKey].BillingType==436||_dictPatients[sub.RegKey].PatNum==1486))//436 is "Internal Use" def, 1486 is HQ patNum.
					||(listSelectedVersions.Count!=0 && !listSelectedVersions.Contains(sub.Info.DictPrefValues[PrefName.ProgramVersion]))
					||(!sub.SubmissionDateTime.Between(dateRangePicker.GetDateTimeFrom(),dateRangePicker.GetDateTimeTo()))) 
			{
				return false;
			}
			return true;
		}
		
		private void FillVersionsFilter() {
			comboVersions.Items.Clear();
			comboVersions.Items.Add("All");
			List<string> listVersions=_listAllSubs.Select(x => x.Info.DictPrefValues[PrefName.ProgramVersion])
				.Distinct()
				.ToList();
			listVersions.Sort((x,y) => new Version(y).CompareTo(new Version(x)));//descending
			listVersions.ForEach(x => comboVersions.Items.Add(x));
			comboVersions.SetSelected(0,true);//Select 'All' by default
		}
		
		private void FillRegKeyFilter() {
			comboRegKeys.Items.Clear();
			comboRegKeys.Items.Add("All");
			List<string> listVersions=_listAllSubs.Select(x => x.RegKey)
				.Distinct()
				.ToList();
			listVersions.Sort();
			listVersions.ForEach(x => comboRegKeys.Items.Add(x));
			comboRegKeys.SetSelected(0,true);//Select 'All' by default
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
			if(e.Row==-1 || gridSubs.SelectedIndices.Length>1) {
				SetCustomerInfo();//Clears customer info, stackTrace and info.
				return;
			}
			BugSubmission sub=((List<BugSubmission>)gridSubs.Rows[e.Row].Tag)[0];
			textStack.Text=sub.ExceptionMessageText+"\r\n"+sub.ExceptionStackTrace;
			FillOfficeInfoGrid(sub);
			SetCustomerInfo(sub);
		}
		
		///<summary>When sub is set, fills customer group box with various information.
		///When null, clears all fields.</summary>
		private void SetCustomerInfo(BugSubmission sub=null,bool refreshGrid=true) {
			if(sub==null) {
				textStack.Text="";//Also clear any submission specific fields.
				labelCustomerNum.Text="";
				labelCustomerName.Text="";
				labelCustomerState.Text="";
				labelCustomerPhone.Text="";
				labelSubNum.Text="";
				labelLastCall.Text="";
				FillOfficeInfoGrid(null);
				gridCustomerSubs.BeginUpdate();
				gridCustomerSubs.Rows.Clear();
				gridCustomerSubs.EndUpdate();
				butGoToAccount.Enabled=false;
				butBugTask.Enabled=false;
				return;
			}
			try {
				if(_dictPatients.ContainsKey(sub.RegKey)) {
					_patCur=_dictPatients[sub.RegKey];
				}
				else {
					RegistrationKey key=RegistrationKeys.GetByKey(sub.RegKey);
					_patCur=Patients.GetPat(key.PatNum);
					if(_patCur==null) {
						return;//Should not happen.
					}
					_dictPatients.Add(sub.RegKey,_patCur);
				}
				labelCustomerNum.Text=_patCur.PatNum.ToString();
				labelCustomerName.Text=_patCur.GetNameLF();
				labelCustomerState.Text=_patCur.State;
				labelCustomerPhone.Text=_patCur.WkPhone;
				labelSubNum.Text=POut.Long(sub.BugSubmissionNum);
				labelLastCall.Text=Commlogs.GetDateTimeOfLastEntryForPat(_patCur.PatNum).ToString();
			}
			catch(Exception ex) {
				ex.DoNothing();
			}
			if(!refreshGrid) {
				return;
			}
			switch(comboGrouping.SelectedIndex) {
				case 0:
					#region None
					gridCustomerSubs.Title="Customer Submissions";
					gridCustomerSubs.BeginUpdate();
					gridCustomerSubs.Columns.Clear();
					gridCustomerSubs.Columns.Add(new ODGridColumn("Version",100,HorizontalAlignment.Center));
					gridCustomerSubs.Columns.Add(new ODGridColumn("Count",50,HorizontalAlignment.Center));
					gridCustomerSubs.Rows.Clear();
					Dictionary<string,List<BugSubmission>> dictCustomerSubVersions=_listAllSubs
						.Where(x => x.RegKey==sub.RegKey)
						.GroupBy(x => x.Info.DictPrefValues[PrefName.ProgramVersion])
						.ToDictionary(x => x.Key,x => x.DistinctBy(y => y.ExceptionStackTrace).ToList());
					foreach(KeyValuePair<string,List<BugSubmission>> pair in dictCustomerSubVersions) {
						gridCustomerSubs.Rows.Add(new ODGridRow(pair.Key,pair.Value.Count.ToString()));
					}
					gridCustomerSubs.EndUpdate();
					#endregion
					break;
				case 1:
				case 2:
				case 3:
					#region Customer, Stacktrace, 95%
					List<BugSubmission> listSubGroup=((List<BugSubmission>)gridSubs.Rows[gridSubs.GetSelectedIndex()].Tag);
					gridCustomerSubs.Title="Grouped Submissions";
					gridCustomerSubs.BeginUpdate();
					gridCustomerSubs.Columns.Clear();
					gridCustomerSubs.Columns.Add(new ODGridColumn("Vers.",55,HorizontalAlignment.Center));
					gridCustomerSubs.Columns.Add(new ODGridColumn("RegKey",140,HorizontalAlignment.Center));
					gridCustomerSubs.Rows.Clear();
					listSubGroup.ForEach(x => { 
						ODGridRow row=new ODGridRow(x.Info.DictPrefValues[PrefName.ProgramVersion],x.RegKey);
						row.Tag=x;
						gridCustomerSubs.Rows.Add(row);
					});
					gridCustomerSubs.EndUpdate();
					#endregion
					break;
			}
			butGoToAccount.Enabled=true;
			butBugTask.Enabled=true;
		}
		
		private void gridCustomerSubs_CellClick(object sender,ODGridClickEventArgs e) {
			if(e.Row==-1 || comboGrouping.SelectedIndex==0) {//0=None
				return;
			}
			BugSubmission sub=(BugSubmission)gridCustomerSubs.Rows[e.Row].Tag;
			textStack.Text=sub.ExceptionMessageText+"\r\n"+sub.ExceptionStackTrace;
			FillOfficeInfoGrid(sub);
			SetCustomerInfo(sub,false);
		}

		private void gridCustomerSubs_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(e.Row==-1 || comboGrouping.SelectedIndex==0) {//0=None
				return;
			}
			List<BugSubmission> listSubGroup=((List<BugSubmission>)gridSubs.Rows[gridSubs.GetSelectedIndex()].Tag);
			FormBugSubmissions formGroupBugSubs=new FormBugSubmissions(viewMode:FormBugSubmissionMode.ViewOnly);
			formGroupBugSubs.ListViewedSubs=listSubGroup;
			formGroupBugSubs.ShowDialog();
		}

		private void butGoToAccount_Click(object sender,EventArgs e) {
			//Button is only enabled if _patCur is not null.
			GotoModule.GotoAccount(_patCur.PatNum);
		}

		private void butBugTask_Click(object sender,EventArgs e) {
			//Button is only enabled if _patCur is not null (user has 1 row selected).
			BugSubmission sub=gridSubs.SelectedIndices.SelectMany(x => (List<BugSubmission>)gridSubs.Rows[x].Tag).First();
			BugSubmissionL.CreateTask(_patCur,sub);
		}

		private void FillOfficeInfoGrid(BugSubmission sub){
			gridOfficeInfo.BeginUpdate();
			if(gridOfficeInfo.Columns.Count==0) {
				gridOfficeInfo.Columns.Add(new ODGridColumn("Field",130));
				gridOfficeInfo.Columns.Add(new ODGridColumn("Value",125));
			}
			gridOfficeInfo.Rows.Clear();
			if(sub!=null) {
				gridOfficeInfo.Rows.Add(new ODGridRow("Preferences","") { ColorBackG=gridOfficeInfo.HeaderColor,Bold=true,Tag=true });
				List<PrefName> listPrefNames=sub.Info.DictPrefValues.Keys.ToList();
				foreach(PrefName prefName in listPrefNames) {
					ODGridRow row=new ODGridRow();
					row.Cells.Add(prefName.ToString());
					row.Cells.Add(sub.Info.DictPrefValues[prefName]);
					gridOfficeInfo.Rows.Add(row);
				}
				gridOfficeInfo.Rows.Add(new ODGridRow("Other","") { ColorBackG=gridOfficeInfo.HeaderColor,Bold=true,Tag=true });
				gridOfficeInfo.Rows.Add(new ODGridRow("CountClinics",sub.Info.CountClinics.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("EnabledPlugins",string.Join(",",sub.Info.EnabledPlugins?.Select(x => x).ToList()??new List<string>())));
				gridOfficeInfo.Rows.Add(new ODGridRow("ClinicNumCur",sub.Info.ClinicNumCur.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("UserNumCur",sub.Info.UserNumCur.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("PatientNumCur",sub.Info.PatientNumCur.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("ModuleNameCur",sub.Info.ModuleNameCur?.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("IsOfficeOnReplication",sub.Info.IsOfficeOnReplication.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("IsOfficeUsingMiddleTier",sub.Info.IsOfficeUsingMiddleTier.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("WindowsVersion",sub.Info.WindowsVersion?.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("CompName",sub.Info.CompName?.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("PreviousUpdateVersion",sub.Info.PreviousUpdateVersion));
				gridOfficeInfo.Rows.Add(new ODGridRow("PreviousUpdateTime",sub.Info.PreviousUpdateTime.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("ThreadName",sub.Info.ThreadName?.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("DatabaseName",sub.Info.DatabaseName?.ToString()));
			}
			gridOfficeInfo.EndUpdate();
		}

		private void dateRangePicker_CalendarClosed(object sender,EventArgs e) {
			FillSubGrid(true);//Refresh _listAllSubs
		}

		private void comboVersions_SelectionChangeCommitted(object sender,EventArgs e) {
			FillSubGrid();
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
		///<summary>Used when we wish to simply view the bug submissions, does not allow users to add bugs.</summary>
		ViewOnly,
		///<summary>Used when attaching bug submissions to exiting bugs. Changed butAdd to show "OK" and return selected rows.</summary>
		SelectionMode,
		///<summary>Used when using the similiar bugs tool. Changed butAdd to show "OK" and returns all BugSubmissions in the grid on Ok click.</summary>
		ValidationMode,
	}
}