using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	public partial class FormJobReviewEdit:ODForm {
		private JobReview _jobReviewCur;
		private List<Userod> _listReviewers;
		private List<string> _listReviewStatusNames;
		private Userod secUser;

		///<summary>Used for existing Reviews. Pass in the jobNum and the jobReviewNum.</summary>
		public FormJobReviewEdit(JobReview jobReview) {
			secUser=Security.CurUser;
			_jobReviewCur=jobReview.Copy();
			InitializeComponent();
			Lan.F(this);
		}

		///<summary>Can be null if deleted from this form.</summary>
		public JobReview JobReviewCur {
			get {
				return _jobReviewCur;
			}
		}

		private void FormJobReviewEdit_Load(object sender,EventArgs e) {
			_listReviewers=Userods.GetUsersByJobRole(JobPerm.Writeup,false);
			_listReviewers.ForEach(x => comboReviewer.Items.Add(x.UserName));
			comboReviewer.SelectedIndex=_listReviewers.FindIndex(x => x.UserNum==_jobReviewCur.ReviewerNum);
			//TimeLogs are used for storing job time rather than reviews so we remove it as an option here.
			_listReviewStatusNames=Enum.GetNames(typeof(JobReviewStatus)).Where(x => x!="TimeLog").ToList();
			_listReviewStatusNames.ForEach(x=>comboStatus.Items.Add(x));
			comboStatus.SelectedIndex=(int)_jobReviewCur.ReviewStatus;
			CheckPermissions();
			if(!_jobReviewCur.IsNew) {
				textDateLastEdited.Text=_jobReviewCur.DateTStamp.ToShortDateString();
			} 
			textDescription.Text=_jobReviewCur.Description;
			textReviewTime.Text=_jobReviewCur.TimeReview.TotalMinutes.ToString();
		}

		private void CheckPermissions() {
			//Disable all controls
			comboReviewer.Enabled=false;
			textDescription.ReadOnly=true;
			comboStatus.Enabled=false;
			butDelete.Enabled=false;
			textReviewTime.Enabled=false;
			if(_jobReviewCur.ReviewerNum==0 && JobPermissions.IsAuthorized(JobPerm.Writeup,true,secUser.UserNum)) {//allow any expert to change the expert if there is no expert.
				comboReviewer.Enabled=true;
			}
			if(_jobReviewCur.ReviewerNum==secUser.UserNum || (_jobReviewCur.ReviewerNum==0 && JobPermissions.IsAuthorized(JobPerm.Writeup,true,secUser.UserNum))) { //allow current expert to edit things.
				textDescription.ReadOnly=false;
				comboStatus.Enabled=true;
				butDelete.Enabled=true;
				textReviewTime.Enabled=true;
			}
			if(_jobReviewCur.Description=="" && _jobReviewCur.ReviewStatus!=JobReviewStatus.Done && JobPermissions.IsAuthorized(JobPerm.Writeup,true,secUser.UserNum)) {
				butDelete.Enabled=true;
			}
			if(new[] { JobReviewStatus.Done,JobReviewStatus.NeedsAdditionalWork }.Contains(_jobReviewCur.ReviewStatus)) {
				butDelete.Enabled=false;
			}
		}

		private void butLogin_Click(object sender,EventArgs e) {
			//Logout
			if(secUser.UserNum!=Security.CurUser.UserNum) {
				butLogin.Text=Lan.g(this,"Login as...");
				this.Text=Lan.g(this,"Job Review Edit");
				secUser=Security.CurUser;
				CheckPermissions();
				return;
			}
			//Otherwise login
			FormLogOn FormLO = new FormLogOn() { IsSimpleSwitch=true };
			if(JobReviewCur!=null) {
				FormLO.UserNumPrompt=JobReviewCur.ReviewerNum;
			}
			FormLO.ShowDialog();
			if(FormLO.DialogResult!=DialogResult.OK) {
				return;
			}
			secUser=FormLO.CurUserSimpleSwitch;
			CheckPermissions();
			if(secUser.UserNum!=Security.CurUser.UserNum) {
				butLogin.Text=Lan.g(this,"Logout");
				this.Text=Lan.g(this,"Job Review Edit")+" - Logged in as "+secUser.UserName;
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(_jobReviewCur.IsNew) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Delete the current job review?")) {
				_jobReviewCur=null;
				DialogResult=DialogResult.OK;
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textReviewTime.errorProvider1.GetError(textReviewTime)!="" || String.IsNullOrEmpty(textReviewTime.Text)) {
				MsgBox.Show(this,"Please provide a valid number for your review time");
				return;
			}
			if(comboReviewer.SelectedIndex>-1) {
				_jobReviewCur.ReviewerNum=_listReviewers[comboReviewer.SelectedIndex].UserNum;
			}
			_jobReviewCur.TimeReview=new TimeSpan(0,int.Parse(textReviewTime.Text),0);
			//Get from the JobReviewStatus enum since the list in this is missing the TimeLog enum value
			_jobReviewCur.ReviewStatus=(JobReviewStatus)_listReviewStatusNames.IndexOf(comboStatus.SelectedItem.ToString());
			_jobReviewCur.Description=textDescription.Text;
			if(_jobReviewCur.IsNew) {
				_jobReviewCur.DateTStamp=DateTime.Now;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel; //removing new jobs from the DB is taken care of in FormClosing
		}

	}
}