using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using CodeBase;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormJobDashboard:ODForm {
		private List<Job> _listJobsAll=new List<Job>();
		private Font _baseFont;
		private Size _baseSize;

		public FormJobDashboard() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormJobDashboard_Load(object sender,EventArgs e) {
			this.WindowState=FormWindowState.Maximized;
			Color color0 = Color.DarkSlateGray;
			Color color1 = Color.LightSlateGray;
			bool useColor0 = true;
			foreach(Userod user in Userods.GetUsersByJobRole(JobPerm.Engineer,false).OrderByDescending(x => x.UserName)) {
				JobManagerUserOverview jobManage;
				if(useColor0) {
					jobManage=new JobManagerUserOverview(color0);
					useColor0=false;
				}
				else {
					jobManage=new JobManagerUserOverview(color1);
					useColor0=true;
				}
				jobManage.User=user;
				jobManage.Dock=DockStyle.Top;
				this.Controls.Add(jobManage);
			}
			//Fills all in memory data from the DB on a seperate thread and then refills controls.
			ODThread thread = new ODThread((o) => {
				_listJobsAll=Jobs.GetAll();
				Jobs.FillInMemoryLists(_listJobsAll);
				FillDashboard();
			});
			thread.AddExceptionHandler((ex) => {
				MessageBox.Show(ex.Message);
			});
			thread.Start(true);
			_baseFont=Font;
			_baseSize=Size;
		}

		public override void OnProcessSignals(List<Signalod> listSignals) {
			if(!listSignals.Exists(x => x.IType==InvalidType.Jobs)) {
				return;//no job signals;
			}
			//Get the latest jobs that have been updated by the signal.
			//Initialized to <jobNum,null>
			Dictionary<long,Job> dictNewJobs = listSignals.FindAll(x => x.IType==InvalidType.Jobs && x.FKeyType==KeyType.Job)
				.Select(x => x.FKey)
				.Distinct()
				.ToDictionary(x => x,x => (Job)null);
			List<Job> newJobs = Jobs.GetMany(dictNewJobs.Keys.ToList());
			Jobs.FillInMemoryLists(newJobs);
			newJobs.ForEach(x => dictNewJobs[x.JobNum]=x);
			//Update in memory lists.
			foreach(KeyValuePair<long,Job> kvp in dictNewJobs) {
				if(kvp.Value==null) {//deleted job
					_listJobsAll.RemoveAll(x => x.JobNum==kvp.Key);
					continue;
				}
				//Master Job List
				Job jobOld = _listJobsAll.FirstOrDefault(x => x.JobNum==kvp.Key);
				if(jobOld==null) {//new job entirely, no need to update anything in memory, just add to jobs list.
					_listJobsAll.Add(kvp.Value);
					continue;
				}
				_listJobsAll[_listJobsAll.IndexOf(jobOld)]=kvp.Value;
			}
			FillDashboard();
		}

		///<summary>Only called after a thread has finished getting all data from the database.</summary>
		private void FillDashboard() {
			if(this.InvokeRequired) {
				this.Invoke((Action)delegate () {
					FillDashboard();
				});
				return;
			}
			foreach(JobManagerUserOverview jobManage in Controls.OfType<JobManagerUserOverview>()) {
				List<Job> listJobs = _listJobsAll.Where(x => x.OwnerNum==jobManage.User.UserNum).ToList();
				jobManage.FillControls(listJobs);
			}
		}

		protected override void OnMouseWheel(MouseEventArgs e) {
			Control contr = GetChildAtPoint(e.Location);
			//if(contr!=null && contr.GetType()==typeof(JobManagerUserOverview) && ((JobManagerUserOverview)contr).MouseIsOver) {
			//	return;
			//}
			base.OnMouseWheel(e);
		}

		private void FormJobDashboard_ResizeEnd(object sender,EventArgs e) {
			float scaleFactorHeight = Size.Height/_baseSize.Height;
			float scaleFactorWidth = Size.Width/_baseSize.Width;
			this.Font=new Font(_baseFont.FontFamily,_baseFont.SizeInPoints*scaleFactorHeight*scaleFactorWidth);
		}
	}
}