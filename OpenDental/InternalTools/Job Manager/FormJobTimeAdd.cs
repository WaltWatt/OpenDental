using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormJobTimeAdd:ODForm {
		public JobReview TimeLogCur;

		///<summary></summary>
		public FormJobTimeAdd(JobReview timeLog) {
			TimeLogCur=timeLog;
			InitializeComponent();
			Lan.F(this);
		}

		private void FormJobTimeAdd_Load(object sender,EventArgs e) {
			TimeLogCur.ReviewStatus=JobReviewStatus.TimeLog;
			TimeLogCur.ReviewerNum=Security.CurUser.UserNum;
			TimeLogCur.DateTStamp=DateTime.Now;
			textUser.Text=Security.CurUser.UserName;
			textDate.Text=TimeLogCur.DateTStamp.ToShortDateString();
			List<string> listAvailableIncrements=new List<string>();
			double i=10;
			while(i>-10.5) {
				listAvailableIncrements.Add(i.ToString());
				i=i-(.5);
			}
			listAvailableIncrements.ForEach(x => comboAddedHours.Items.Add(x));
			comboAddedHours.Text="0";
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(comboAddedHours.Text=="0") {
				MsgBox.Show(this,"Please choose an option besides 0 for your hours added.");
				return;
			}
			TimeLogCur.TimeReview=TimeSpan.FromHours(double.Parse(comboAddedHours.Text));
			TimeLogCur.Description=textDescription.Text;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel; //removing new jobs from the DB is taken care of in FormClosing
		}
	}
}