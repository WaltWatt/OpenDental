using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;
using System.Drawing.Text;
using System.Linq;
using System.Xml;

namespace OpenDental {
	public partial class FormJobSearch:ODForm {
		public string InitialSearchString="";
		private List<Job> _listJobsAll;
		private List<Job> _listJobsFiltered;
		private List<Task> _listTasksAll;
		private List<TaskNote> _listTaskNotesAll;
		private List<Patient> _listPatientAll;
		private List<FeatureRequest> _listFeatureRequestsAll;
		private List<Bug> _listBugsAll;
		private List<JobPhase> _listJobStatuses;
		private List<JobCategory> _listJobCategory;
		private Job _selectedJob;
		private bool _IsSearchReady = false;
		private List<Userod> _listUsers;

		public FormJobSearch(List<Job> listJobsAll=null) {
			_listJobsAll=listJobsAll;
			InitializeComponent();
		}

		public Job SelectedJob{
			get {return _selectedJob;}
		}

		public List<Job> GetSearchResults() {
			return _listJobsFiltered??new List<Job>();
		}

		private void FormJobNew_Load(object sender,EventArgs e) {
			textSearch.Text=InitialSearchString;
			listBoxUsers.Items.Add("Any");
			_listUsers=Userods.GetDeepCopy(true);
			_listUsers.ForEach(x => listBoxUsers.Items.Add(x.UserName));
			//Statuses
			listBoxStatus.Items.Add("Any");
			_listJobStatuses =Enum.GetValues(typeof(JobPhase)).Cast<JobPhase>().ToList();
			_listJobStatuses.ForEach(x=>listBoxStatus.Items.Add(x.ToString()));
			//Categories
			listBoxCategory.Items.Add("Any");
			_listJobCategory=Enum.GetValues(typeof(JobCategory)).Cast<JobCategory>().ToList();
			_listJobCategory.ForEach(x => listBoxCategory.Items.Add(x.ToString()));
			ODThread thread=new ODThread((o) => {
				//We can reduce these calls to DB by passing in more data from calling class. if available.
				if(_listJobsAll==null) {
					_listJobsAll=Jobs.GetAll();
					Jobs.FillInMemoryLists(_listJobsAll);
				}
				try {
					_listFeatureRequestsAll=FeatureRequests.GetAll();
				}
				catch (Exception ex){
					ex.DoNothing();
					_listFeatureRequestsAll=new List<FeatureRequest>();
				}
				_listBugsAll=Bugs.GetAll();
				this.Invoke((Action)ReadyForSearch);
			});
			thread.AddExceptionHandler((ex) => {/*todo*/});
			thread.Start();
		}

		private void ReadyForSearch() {
			_IsSearchReady=true;
			try {
				FillGridMain();
			}
			catch (Exception e){
				e.DoNothing();
			}
		}

		private void listBoxUsers_SelectedIndexChanged(object sender,EventArgs e) {
			FillGridMain();
		}

		private void listBoxStatus_SelectedIndexChanged(object sender,EventArgs e) {
			FillGridMain();
		}

		private void listBoxCategory_SelectedIndexChanged(object sender,EventArgs e) {
			FillGridMain();
		}

		private void FillGridMain() {
			if(!_IsSearchReady) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			//TODO: change columns
			gridMain.Columns.Add(new ODGridColumn("Job\r\nNum",50));
			gridMain.Columns.Add(new ODGridColumn("Phase",85));
			gridMain.Columns.Add(new ODGridColumn("Category",80));
			gridMain.Columns.Add(new ODGridColumn("Job Title",300));
			gridMain.Columns.Add(new ODGridColumn("Version",80));
			gridMain.Columns.Add(new ODGridColumn("Expert",75));
			gridMain.Columns.Add(new ODGridColumn("Engineer",75));
			gridMain.Columns.Add(new ODGridColumn("Job\r\nMatch",45) { TextAlign=HorizontalAlignment.Center });
			gridMain.Columns.Add(new ODGridColumn("Bug\r\nMatch",45) { TextAlign=HorizontalAlignment.Center });
			gridMain.Columns.Add(new ODGridColumn("Feature\r\nRequest\r\nMatch",45) { TextAlign=HorizontalAlignment.Center });
			gridMain.Rows.Clear();
			string[] searchTokens = textSearch.Text.ToLower().Split(new[] { " " },StringSplitOptions.RemoveEmptyEntries);
			_listJobsFiltered=new List<Job>();
			long[] userNums=new long[0];
			JobCategory[] jobCats=new JobCategory[0];
			JobPhase[] jobPhases=new JobPhase[0];
			if(listBoxUsers.SelectedIndices.Count>0 && !listBoxUsers.SelectedIndices.Contains(0)) {
				userNums = listBoxUsers.SelectedIndices.Cast<int>().Select(x => _listUsers[x-1].UserNum).ToArray();
			}
			if(listBoxCategory.SelectedIndices.Count>0 && !listBoxCategory.SelectedIndices.Contains(0)) {
				jobCats = listBoxCategory.SelectedIndices.Cast<int>().Select(x => (JobCategory)(x-1)).ToArray();
			}
			if(listBoxStatus.SelectedIndices.Count>0 && !listBoxStatus.SelectedIndices.Contains(0)) {
				jobPhases = listBoxStatus.SelectedIndices.Cast<int>().Select(x => (JobPhase)(x-1)).ToArray();
			}
			foreach(Job jobCur in _listJobsAll) {
				if(jobCats.Length>0 && !jobCats.Contains(jobCur.Category)) {
					continue;
				}
				if(jobPhases.Length>0 && !jobPhases.Contains(jobCur.PhaseCur)) {
					continue;
				}
				if(userNums.Length>0 && !userNums.All(x => Jobs.GetUserNums(jobCur).Contains(x))) {
					continue;
				}
				bool isJobMatch = false;
				bool isBugMatch = false;
				bool isFeatureReqMatch = false;
				if(searchTokens.Length>0) {
					bool addRow = false;
					List<Bug> listBugs = jobCur.ListJobLinks.FindAll(x => x.LinkType==JobLinkType.Bug).Select(x => _listBugsAll.FirstOrDefault(y => x.FKey==y.BugId)).Where(x => x!=null).ToList();
					List<FeatureRequest> listFeatures = jobCur.ListJobLinks.FindAll(x => x.LinkType==JobLinkType.Request).Select(x => _listFeatureRequestsAll.FirstOrDefault(y => x.FKey==y.FeatReqNum)).Where(x => x!=null).ToList();
					foreach(string token in searchTokens.Distinct()) {
						bool isFound = false;
						//JOB MATCHES
						if(jobCur.Title.ToLower().Contains(token) 
							|| jobCur.Implementation.ToLower().Contains(token)
							|| jobCur.Requirements.ToLower().Contains(token) 
							|| jobCur.Documentation.ToLower().Contains(token)
							|| jobCur.JobNum.ToString().Contains(token)) 
						{
							isFound=true;
							isJobMatch=true;
						}
						//BUG MATCHES
						if(!isFound || !isBugMatch) {
							if(listBugs.Any(x => x.Description.ToLower().Contains(token) || x.Discussion.ToLower().Contains(token))) {
								isFound=true;
								isBugMatch=true;
							}
						}
						//FEATURE REQUEST MATCHES
						if(!isFound || !isFeatureReqMatch) {
							if(listFeatures.Any(x => x.Description.Contains(token) || x.FeatReqNum.ToString().ToLower().Contains(token))) {
								isFound=true;
								isFeatureReqMatch=true;
							}
						}
						addRow=isFound;
						if(!isFound) {
							break;//stop looking for additional tokens, we didn't find this one.
						}
					}
					if(!addRow) {
						continue;//we did not find one of the search terms.
					}
				}
				_listJobsFiltered.Add(jobCur);
				ODGridRow row = new ODGridRow();
				row.Cells.Add(jobCur.JobNum.ToString());
				row.Cells.Add(jobCur.PhaseCur.ToString());
				row.Cells.Add(jobCur.Category.ToString());
				row.Cells.Add(jobCur.Title.Left(53,true));
				row.Cells.Add(jobCur.JobVersion.ToString());
				row.Cells.Add(Userods.GetName(jobCur.UserNumExpert));
				row.Cells.Add(Userods.GetName(jobCur.UserNumEngineer));
				row.Cells.Add(isJobMatch ? "X" : "");
				row.Cells.Add(isBugMatch ? "X" : "");
				row.Cells.Add(new ODGridCell(isFeatureReqMatch ? "X" : "") { CellColor=_listFeatureRequestsAll.Count==0 ? Control.DefaultBackColor : Color.Empty });
				row.Tag=jobCur;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			Cursor=Cursors.Default;
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			if(e.Row<0 || e.Row>gridMain.Rows.Count || !(gridMain.Rows[e.Row].Tag is Job)) {
				_selectedJob=null;
				return;
			}
			_selectedJob=(Job)gridMain.Rows[e.Row].Tag;
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(e.Row<0 || e.Row>gridMain.Rows.Count || !(gridMain.Rows[e.Row].Tag is Job)) {
				_selectedJob=null;
				return;
			}
			_selectedJob=(Job)gridMain.Rows[e.Row].Tag;
			DialogResult=DialogResult.OK;
		}

		private void textSearch_TextChanged(object sender,EventArgs e) {
			//FillGridMain();
			timerSearch.Stop();
			timerSearch.Start();
		}

		private void timerSearch_Tick(object sender,EventArgs e) {
			timerSearch.Stop();
			FillGridMain();
		}

		private void butOK_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}