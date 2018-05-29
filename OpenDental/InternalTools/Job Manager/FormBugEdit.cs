using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormBugEdit:Form {
		public Bug BugCur;
		public bool IsNew;
		private List<BugSubmission> _listBugSubs=new List<BugSubmission>();

		///<summary>When listBugSubs is set, on OK click all BugSubmissions.BugId will be updated to the current bugId.
		///Seting listBugSubs will add to any existing BugSubmissions they may or may not exist  that areassociated to BugCur.</summary>
		public FormBugEdit(List<BugSubmission> listBugSubs=null) {
			InitializeComponent();
			if(listBugSubs!=null) {
				_listBugSubs.AddRange(listBugSubs);
			}
		}

		private void FormBugEdit_Load(object sender,EventArgs e) {
			if(BugCur==null) {
				MsgBox.Show(this,"An invalid bug was attempted to be loaded.");
				DialogResult=DialogResult.Abort;
				Close();
				return;
			}
			textBugId.Text=BugCur.BugId.ToString();
			textCreationDate.Text=BugCur.CreationDate.ToString();
			comboStatus.Text=BugCur.Status_.ToString();
			comboPriority.Text=BugCur.PriorityLevel.ToString();
			textVersionsFound.Text=BugCur.VersionsFound;
			textVersionsFixed.Text=BugCur.VersionsFixed;
			textDescription.Text=BugCur.Description;
			textLongDesc.Text=BugCur.LongDesc;
			textPrivateDesc.Text=BugCur.PrivateDesc;
			textDiscussion.Text=BugCur.Discussion;
			textSubmitter.Text=Bugs.GetSubmitterName(BugCur.Submitter);
			if(!IsNew) {
				_listBugSubs.AddRange(BugSubmissions.GetForBugId(BugCur.BugId));
			}
			FillGrid();
		}
		
		private void FillGrid() {
			gridSubs.BeginUpdate();
			if(gridSubs.Columns.Count==0) {
				gridSubs.Columns.Add(new ODGridColumn("Reg Key",125));
				gridSubs.Columns.Add(new ODGridColumn("Version",60,GridSortingStrategy.VersionNumber));
				gridSubs.Columns.Add(new ODGridColumn("DateTime",75,GridSortingStrategy.DateParse));
				gridSubs.AllowSortingByColumn=true;
			}
			gridSubs.Rows.Clear();
			foreach(BugSubmission sub in _listBugSubs){
				ODGridRow row=new ODGridRow();
				row.Cells.Add(sub.RegKey);
				row.Cells.Add(sub.DbVersion);
				row.Cells.Add(sub.SubmissionDateTime.ToString());
				row.Tag=sub;
				gridSubs.Rows.Add(row);
			}
			gridSubs.EndUpdate();
			gridSubs.SortForced(1,true);/*Sort by Version column*/
		}

		private void gridSubs_CellClick(object sender,ODGridClickEventArgs e) {
			if(e.Button!=MouseButtons.Right) {
				return;
			}
			gridSubs.ContextMenu=new ContextMenu();
			ContextMenu menu=gridSubs.ContextMenu;
			BugSubmission sub=(BugSubmission)gridSubs.Rows[e.Row].Tag;
			menu.MenuItems.Add("Unlink Submission",(o,arg) => {
				_listBugSubs.Remove(sub);
				BugSubmissions.UpdateBugIds(0,new List<long>() { sub.BugSubmissionNum });
				FillGrid();
			});
			menu.Show(gridSubs,gridSubs.PointToClient(Cursor.Position));
		}

		private void gridSubs_TitleAddClick(object sender,EventArgs e) {
			FormBugSubmissions FormBS=new FormBugSubmissions(viewMode:FormBugSubmissionMode.SelectionMode);
			if(FormBS.ShowDialog()!=DialogResult.OK) {
				return;
			}
			_listBugSubs.AddRange(FormBS.ListSelectedSubs);
			FillGrid();
		}
		
		private void gridSubs_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(e.Row==-1) {
				return;
			}
			FormBugSubmission formBugSub=new FormBugSubmission(_listBugSubs[e.Row]);
			formBugSub.Show();
		}

		private void butCopyDown_Click(object sender,EventArgs e) {
			textVersionsFixed.Text=textVersionsFound.Text;
		}

		private void butLast1found_Click(object sender,EventArgs e) {
			textVersionsFound.Text=VersionReleases.GetLastReleases(1);
		}

		private void butLast2found_Click(object sender,EventArgs e) {
			textVersionsFound.Text=VersionReleases.GetLastReleases(2);
		}

		private void butLast3found_Click(object sender,EventArgs e) {
			textVersionsFound.Text=VersionReleases.GetLastReleases(3);
		}

		private void butLast1_Click(object sender,EventArgs e) {
			textVersionsFixed.Text=VersionReleases.GetLastReleases(1);
		}

		private void butLast2_Click(object sender,EventArgs e) {
			textVersionsFixed.Text=VersionReleases.GetLastReleases(2);
		}

		private void butLast3_Click(object sender,EventArgs e) {
			textVersionsFixed.Text=VersionReleases.GetLastReleases(3);
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(IsNew){
				DialogResult=DialogResult.Cancel;
				return;
			}
			Bugs.Delete(BugCur.BugId);
			BugSubmissions.UpdateBugIds(0,_listBugSubs.Select(x => x.BugSubmissionNum).ToList());
			BugCur=null;
			DialogResult=DialogResult.OK;
		}

		private void butLeaveStatus_Click(object sender,EventArgs e) {
			BugCur.Status_=(BugStatus)Enum.Parse(typeof(BugStatus),comboStatus.Text);
			SaveToDb();
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(BugCur.Submitter==0) {
				MessageBox.Show("A valid submitter wasn't picked.  Make sure the computer being used is associated to a buguser.");
				return;
			}
			if(textVersionsFixed.Text!=""){
				BugCur.Status_=BugStatus.Fixed;
			}
			else if(comboStatus.SelectedIndex==0) {//none
				BugCur.Status_=BugStatus.Accepted;
			}
			else{
				BugCur.Status_=(BugStatus)Enum.Parse(typeof(BugStatus),comboStatus.Text);
			}
			SaveToDb();
			BugSubmissions.UpdateBugIds(BugCur.BugId,_listBugSubs.Select(x => x.BugSubmissionNum).ToList());
			DialogResult=DialogResult.OK;
		}

		private void SaveToDb(){
			//BugId
			//CreationDate
			BugCur.Type_=BugType.Bug;//user can't change
			BugCur.PriorityLevel=PIn.Int(comboPriority.Text);
			BugCur.VersionsFound=textVersionsFound.Text;
			BugCur.VersionsFixed=textVersionsFixed.Text;
			BugCur.Description=textDescription.Text;
			BugCur.LongDesc=textLongDesc.Text;
			BugCur.PrivateDesc=textPrivateDesc.Text;
			BugCur.Discussion=textDiscussion.Text;
			//Submitter
			if(IsNew){
				Bugs.Insert(BugCur);
			}
			else{
				Bugs.Update(BugCur);
			}
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}