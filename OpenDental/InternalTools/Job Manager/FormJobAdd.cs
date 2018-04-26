using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;
using CodeBase;

namespace OpenDental {
	public partial class FormJobAdd:ODForm {
		private Job _jobNew;
		private List<JobLink> _listJobLinks;
		private List<JobQuote> _listJobQuotes;
		private List<string> _listCategoryNames;
		private List<string> _listCategoryNamesFiltered;
		private List<Def> _listPriorities;
		private const string KEY="kS3o1OTpghLLPg49+Q7WKg==";
		private const string MSG="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";


		public FormJobAdd(Job jobNew) {
			_jobNew=jobNew;
			_listJobLinks=new List<JobLink>();
			_listJobQuotes=new List<JobQuote>();
			InitializeComponent();
			Lan.F(this);
		}

		private void FormJobAdd_Load(object sender,EventArgs e) {
			_listCategoryNames=Enum.GetNames(typeof(JobCategory)).ToList();
			_listCategoryNamesFiltered=_listCategoryNames.Where(x => x!=JobCategory.Query.ToString()).ToList();
			_listCategoryNamesFiltered.ForEach(x=>comboCategory.Items.Add(x));
			comboCategory.SelectedIndex=_listCategoryNamesFiltered.IndexOf(_jobNew.Category.ToString());
			_listPriorities=Defs.GetDefsForCategory(DefCat.JobPriorities,true).OrderBy(x => x.ItemOrder).ToList();
			_listPriorities.ForEach(x=>comboPriority.Items.Add(new ODBoxItem<Def>(x.ItemName,x)));
			comboPriority.SelectedIndex=_listPriorities.Select(x => x.DefNum).ToList().IndexOf(_jobNew.Priority);
		}

		#region FillGrids
		private void FillGridCustomers() {
			gridCustomers.BeginUpdate();
			gridCustomers.Columns.Clear();
			gridCustomers.Columns.Add(new ODGridColumn("PatNum",-1));
			gridCustomers.Columns.Add(new ODGridColumn("Name",-1));
			gridCustomers.Columns.Add(new ODGridColumn("BillType",-1));
			gridCustomers.Columns.Add(new ODGridColumn("Unlink",40,HorizontalAlignment.Center));
			gridCustomers.Rows.Clear();
			List<Patient> listPatients= Patients.GetMultPats(_listJobLinks.FindAll(x => x.LinkType==JobLinkType.Customer)
				.Select(x => x.FKey).ToList()).ToList();
			foreach(Patient pat in listPatients){
				ODGridRow row=new ODGridRow() { Tag=pat };
				row.Cells.Add(pat.PatNum.ToString());
				row.Cells.Add(pat.GetNameFL());
				row.Cells.Add(Defs.GetDef(DefCat.BillingTypes,pat.BillingType).ItemName);
				row.Cells.Add("X");
				gridCustomers.Rows.Add(row);
			}
			gridCustomers.EndUpdate();
		}

		private void FillGridSubscribers() {
			gridSubscribers.BeginUpdate();
			gridSubscribers.Columns.Clear();
			gridSubscribers.Columns.Add(new ODGridColumn("User Name",-1));
			gridSubscribers.Columns.Add(new ODGridColumn("Unlink",40,HorizontalAlignment.Center));
			gridSubscribers.Rows.Clear();
			List<Userod> listSubscribers=_listJobLinks.FindAll(x => x.LinkType==JobLinkType.Subscriber)
				.Select(x => Userods.GetFirstOrDefault(y => y.UserNum==x.FKey)).ToList();
			foreach(Userod user in listSubscribers.FindAll(x => x!=null)){
				ODGridRow row=new ODGridRow() { Tag =user };
				row.Cells.Add(user.UserName);
				row.Cells.Add("X");
				gridSubscribers.Rows.Add(row);
			}
			gridSubscribers.EndUpdate();
		}

		private void FillGridQuotes() {
			gridQuotes.BeginUpdate();
			gridQuotes.Columns.Clear();
			gridQuotes.Columns.Add(new ODGridColumn("PatNum",-1));
			gridQuotes.Columns.Add(new ODGridColumn("Hours",-1,HorizontalAlignment.Center));
			gridQuotes.Columns.Add(new ODGridColumn("Amt",-1,HorizontalAlignment.Right));
			gridQuotes.Columns.Add(new ODGridColumn("Appr?",40,HorizontalAlignment.Center));
			gridQuotes.Columns.Add(new ODGridColumn("Unlink",40,HorizontalAlignment.Center));
			gridQuotes.NoteSpanStart=0;
			gridQuotes.NoteSpanStop=4;
			gridQuotes.Rows.Clear();
			foreach(JobQuote jobQuote in _listJobQuotes){
				ODGridRow row=new ODGridRow() { Tag=jobQuote };
				row.Cells.Add(jobQuote.PatNum.ToString());
				row.Cells.Add(jobQuote.Hours);
				row.Cells.Add(jobQuote.ApprovedAmount=="0.00"?jobQuote.Amount:jobQuote.ApprovedAmount);
				row.Cells.Add(jobQuote.IsCustomerApproved?"X":"");
				row.Cells.Add("X");
				row.Note=jobQuote.Note;
				gridQuotes.Rows.Add(row);
			}
			gridQuotes.EndUpdate();
		}

		private void FillGridTasks() {
			gridTasks.BeginUpdate();
			gridTasks.Columns.Clear();
			gridTasks.Columns.Add(new ODGridColumn("Date",-1));
			gridTasks.Columns.Add(new ODGridColumn("TaskList",-1));
			gridTasks.Columns.Add(new ODGridColumn("Done",40) { TextAlign=HorizontalAlignment.Center });
			gridTasks.Columns.Add(new ODGridColumn("Unlink",40,HorizontalAlignment.Center));
			gridTasks.NoteSpanStart=0;
			gridTasks.NoteSpanStop=3;
			gridTasks.Rows.Clear();
			List<Task> listTasks=_listJobLinks.FindAll(x => x.LinkType==JobLinkType.Task)
				.Select(x => Tasks.GetOne(x.FKey))
				.Where(x => x!=null)
				.OrderBy(x =>x.DateTimeEntry).ToList();
			foreach(Task task in listTasks){
				ODGridRow row=new ODGridRow() { Tag=task.TaskNum };//taskNum
				row.Cells.Add(task.DateTimeEntry.ToShortDateString());
				row.Cells.Add(TaskLists.GetOne(task.TaskListNum)?.Descript??"<TaskListNum:"+task.TaskListNum+">");
				row.Cells.Add(task.TaskStatus==TaskStatusEnum.Done?"X":"");
				row.Cells.Add("X");
				row.Note=task.Descript.Left(100,true).Trim();
				gridTasks.Rows.Add(row);
			}
			gridTasks.EndUpdate();
		}

		private void FillGridFeatureReq() {
			gridFeatureReq.BeginUpdate();
			gridFeatureReq.Columns.Clear();
			gridFeatureReq.Columns.Add(new ODGridColumn("Feat Req Num",-1));
			gridFeatureReq.Columns.Add(new ODGridColumn("Unlink",40,HorizontalAlignment.Center));
			//todo: add status of FR. Difficult because FR dataset comes from webservice.
			//gridFeatureReq.Columns.Add(new ODGridColumn("Status",50){TextAlign=HorizontalAlignment.Center});
			gridFeatureReq.Rows.Clear();
			List<long> listReqNums=_listJobLinks.FindAll(x => x.LinkType==JobLinkType.Request).Select(x => x.FKey).ToList();
			foreach(long reqNum in listReqNums){
				ODGridRow row=new ODGridRow() { Tag=reqNum };//FR Num
				row.Cells.Add(reqNum.ToString());
				row.Cells.Add("X");
				//todo: add status of FR. Difficult because FR dataset comes from webservice.
				gridFeatureReq.Rows.Add(row);
			}
			gridFeatureReq.EndUpdate();
		}

		private void FillGridBugs() {
			gridBugs.BeginUpdate();
			gridBugs.Columns.Clear();
			gridBugs.Columns.Add(new ODGridColumn("Bug Num (From JRMT)",-1));
			gridBugs.Columns.Add(new ODGridColumn("Unlink",40,HorizontalAlignment.Center));
			gridBugs.Rows.Clear();
			List<JobLink> listBugLinks=_listJobLinks.FindAll(x => x.LinkType==JobLinkType.Bug).ToList();
			for(int i=0;i<listBugLinks.Count;i++) {
				Bug bug=Bugs.GetOne(listBugLinks[i].FKey);
				ODGridRow row=new ODGridRow() { Tag=listBugLinks[i].FKey };//bugNum
				row.Cells.Add(bug==null ? "Invalid Bug" : bug.Description);
				row.Cells.Add("X");
				gridBugs.Rows.Add(row);
			}
			gridBugs.EndUpdate();
		}
		#endregion

		#region GridActions
		private void gridCustomers_TitleAddClick(object sender,EventArgs e) {
			FormPatientSelect FormPS=new FormPatientSelect();
			FormPS.ShowDialog();
			if(FormPS.DialogResult!=DialogResult.OK) {
				return;
			}
			JobLink jobLink = new JobLink() {
				FKey=FormPS.SelectedPatNum,
				LinkType=JobLinkType.Customer
			};
			_listJobLinks.Add(jobLink);
			FillGridCustomers();
		}

		private void gridSubscribers_TitleAddClick(object sender,EventArgs e) {
			FormUserPick FormUP = new FormUserPick();
			//Suggest current user if not already watching.
			if(_listJobLinks.FindAll(x => x.LinkType==JobLinkType.Subscriber).All(x => x.FKey!=Security.CurUser.UserNum)) {
				FormUP.SuggestedUserNum=Security.CurUser.UserNum;
			}
			FormUP.IsSelectionmode=true;
			FormUP.ShowDialog();
			if(FormUP.DialogResult!=DialogResult.OK) {
				return;
			}
			JobLink jobLink = new JobLink() {
				FKey=FormUP.SelectedUserNum,
				LinkType=JobLinkType.Subscriber
			};
			_listJobLinks.Add(jobLink);
			FillGridSubscribers();
		}

		private void gridCustomerQuotes_TitleAddClick(object sender,EventArgs e) {
			FormJobQuoteEdit FormJQE=new FormJobQuoteEdit(new JobQuote() {IsNew=true});
			FormJQE.ShowDialog();
			if(FormJQE.DialogResult!=DialogResult.OK || FormJQE.JobQuoteCur==null) {
				return;
			}
			_listJobQuotes.Add(FormJQE.JobQuoteCur);
			FillGridQuotes();
		}

		private void gridTasks_TitleAddClick(object sender,EventArgs e) {
			FormTaskSearch FormTS=new FormTaskSearch() {IsSelectionMode=true};
			FormTS.ShowDialog();
			if(FormTS.DialogResult!=DialogResult.OK) {
				return;
			}
			JobLink jobLink=new JobLink();
			jobLink.FKey=FormTS.SelectedTaskNum;
			jobLink.LinkType=JobLinkType.Task;
			_listJobLinks.Add(jobLink);
			FillGridTasks();
		}

		private void gridFeatureReq_TitleAddClick(object sender,EventArgs e) {
			AddFeatureRequest();
		}

		private bool AddFeatureRequest() {
			FormFeatureRequest FormFR=new FormFeatureRequest() {IsSelectionMode=true};
			FormFR.ShowDialog();
			if(FormFR.DialogResult!=DialogResult.OK) {
				return false;
			}
			JobLink jobLink=new JobLink();
			jobLink.FKey=FormFR.SelectedFeatureNum;
			jobLink.LinkType=JobLinkType.Request;
			_listJobLinks.Add(jobLink);
			FillGridFeatureReq();
			return true;
		}

		private void gridBugs_TitleAddClick(object sender,EventArgs e) {
			AddBug();
		}

		private bool AddBug() {
			FormBugSearch FormBS=new FormBugSearch(_jobNew);
			FormBS.ShowDialog();
			if(FormBS.DialogResult!=DialogResult.OK || FormBS.BugCur==null) {
				return false;
			}
			JobLink jobLink=new JobLink();
			jobLink.FKey=FormBS.BugCur.BugId;
			jobLink.LinkType=JobLinkType.Bug;
			_listJobLinks.Add(jobLink);
			FillGridBugs();
			return true;
		}

		private void gridBugs_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormBugEdit FormBE=new FormBugEdit();
			FormBE.BugCur=Bugs.GetOne((long)gridBugs.Rows[e.Row].Tag);
			FormBE.ShowDialog();
			if(FormBE.BugCur==null) {
				_listJobLinks.RemoveAll(x => x.LinkType==JobLinkType.Bug && x.FKey==(long)gridBugs.Rows[e.Row].Tag);
			}
			FillGridBugs();
		}

		private void gridQuotes_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(!(gridQuotes.Rows[e.Row].Tag is JobQuote)) {
				return;//should never happen
			}
			JobQuote jq = (JobQuote)gridQuotes.Rows[e.Row].Tag;
			FormJobQuoteEdit FormJQE = new FormJobQuoteEdit(jq);
			FormJQE.ShowDialog();
			if(FormJQE.DialogResult!=DialogResult.OK) {
				return;
			}
			_listJobQuotes.RemoveAll(x=>x.JobQuoteNum==jq.JobQuoteNum);//should remove only one
			if(FormJQE.JobQuoteCur!=null) {//re-add altered version, iff the jobquote was modified.
				_listJobQuotes.Add(FormJQE.JobQuoteCur);
			}
			FillGridQuotes();
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

		private void gridCustomers_CellClick(object sender,ODGridClickEventArgs e) {
			if(e.Col==3) {//Remove
				long FKey = ((Patient)gridCustomers.Rows[e.Row].Tag).PatNum;
				_listJobLinks.RemoveAll(x => x.LinkType==JobLinkType.Customer&&x.FKey==FKey);
				FillGridCustomers();
			}
		}

		private void gridSubscribers_CellClick(object sender,ODGridClickEventArgs e) {
			if(e.Col==1) {//Remove
				long FKey = ((Userod)gridSubscribers.Rows[e.Row].Tag).UserNum;
				_listJobLinks.RemoveAll(x => x.LinkType==JobLinkType.Subscriber&&x.FKey==FKey);
				FillGridSubscribers();
			}
		}

		private void gridQuotes_CellClick(object sender,ODGridClickEventArgs e) {
			if(e.Col==4) {//Remove
				long FKey = ((JobQuote)gridQuotes.Rows[e.Row].Tag).JobQuoteNum;
				_listJobQuotes.RemoveAll(x => x.JobQuoteNum==FKey);
				FillGridQuotes();
			}
		}

		private void gridTasks_CellClick(object sender,ODGridClickEventArgs e) {
			if(e.Col==3) {//Remove
				long FKey = (long)gridTasks.Rows[e.Row].Tag;
				_listJobLinks.RemoveAll(x => x.LinkType==JobLinkType.Request&&x.FKey==FKey);
				FillGridFeatureReq();
			}
		}

		private void gridFeatureReq_CellClick(object sender,ODGridClickEventArgs e) {
			if(e.Col==1) {//Remove
				long FKey = (long)gridFeatureReq.Rows[e.Row].Tag;
				_listJobLinks.RemoveAll(x => x.LinkType==JobLinkType.Task&&x.FKey==FKey);
				FillGridTasks();
			}
		}

		private void gridBugs_CellClick(object sender,ODGridClickEventArgs e) {
			if(e.Col==1) {//Remove
				long FKey = (long)gridBugs.Rows[e.Row].Tag;
				_listJobLinks.RemoveAll(x => x.LinkType==JobLinkType.Bug&&x.FKey==FKey);
				FillGridBugs();
			}
		}
		#endregion
		
		private void textTitle_TextChanged(object sender,EventArgs e) {
			_jobNew.Title=textTitle.Text;
		}

		private void comboPriority_SelectionChangeCommitted(object sender,EventArgs e) {
			long priorityNum=((ODBoxItem<Def>)comboPriority.SelectedItem).Tag.DefNum;
			_jobNew.Priority=priorityNum;
		}

		private void comboCategory_SelectionChangeCommitted(object sender,EventArgs e) {
			JobCategory jobCategoryNew=(JobCategory)_listCategoryNames.IndexOf(comboCategory.SelectedItem.ToString());
			if(jobCategoryNew==JobCategory.Bug) {
				Def bugDef=_listPriorities.FirstOrDefault(x => x.ItemValue.Contains("BugDefault"));
				comboPriority.SelectedIndex=_listPriorities.IndexOf(bugDef);
				_jobNew.Priority=bugDef.DefNum;
			}
			_jobNew.Category=jobCategoryNew;
		}

		private void butVersionPrompt_Click(object sender,EventArgs e) {
			FormVersionPrompt FormVP=new FormVersionPrompt();
			FormVP.VersionText=textVersion.Text;
			FormVP.ShowDialog();
			if(FormVP.DialogResult!=DialogResult.OK || string.IsNullOrEmpty(FormVP.VersionText)) {
				return;
			}
			textVersion.Text=FormVP.VersionText;
		}

		private bool ValidateJob() {
			if(string.IsNullOrWhiteSpace(textTitle.Text)) {
				MessageBox.Show("Invalid Title.");
				return false;
			}
			if(_jobNew.Priority==0) {
				MsgBox.Show(this,"Please select a priority before saving the job.");
				return false;
			}
			return true;
		}

		private bool ValidateTitle() {
			//Key Hint: Nick Fury quote from Captain America Winter Soldier, 3 words, no spaces, CamelCased
			if(Userods.HashPassword(textTitle.Text)!=KEY) {
				return true;
			}
			butOK.Enabled=false;
			butCancel.Enabled=false;
			tabControlExtra.SelectedTab=tabConcept;
			tabConcept.BackColor=Color.Black;
			labelCategory.Text="Protect";
			labelPriority.Text="The";
			labelVersion.Text="Program";
			comboCategory.Visible=false;
			comboPriority.Visible=false;
			textVersion.Visible=false;
			butVersionPrompt.Visible=false;
			label1.Visible=false;
			textConcept.ReadOnly=true;
			textConcept.TextBox.BackColor=Color.Black;
			textConcept.TextBox.ForeColor=Color.White;
			tabControlExtra.TabPages.Remove(tabCustomers);
			tabControlExtra.TabPages.Remove(tabQuotes);
			tabControlExtra.TabPages.Remove(tabSubscribers);
			tabControlExtra.TabPages.Remove(tabRequests);
			tabControlExtra.TabPages.Remove(tabBugs);
			tabControlExtra.TabPages.Remove(tabTasks);
			BackColor=Color.Black;
			foreach(Control c in this.Controls) {
				c.BackColor=Color.Black;
				c.ForeColor=Color.White;
			}
			Text="AVENGINEERS MODE ACTIVE";
			string str="";
			CDT.Class1.Decrypt(MSG,out str);
			textTitle.Text="Welcome Director Fury";
			textConcept.MainText=str;
			return false;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!ValidateTitle()) {
				return;
			}
			if(!ValidateJob()) {
				return;
			}
			_jobNew.JobVersion=textVersion.Text;
			_jobNew.Requirements=textConcept.MainRtf;
			long jobNum=Jobs.Insert(_jobNew);
			foreach(JobLink link in _listJobLinks) {
				link.JobNum=jobNum;
				JobLinks.Insert(link);
			}
			foreach(JobQuote quote in _listJobQuotes) {
				quote.JobNum=jobNum;
				JobQuotes.Insert(quote);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}