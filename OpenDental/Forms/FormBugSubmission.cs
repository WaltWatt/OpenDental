using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Linq;
using CodeBase;

namespace OpenDental {
	public partial class FormBugSubmission:ODForm {
		///<summary></summary>
		private BugSubmission _sub;
		///<summary>Used to determine if a new bug should show (Enhancement) in the description.</summary>
		private Job _jobCur;
		///<summary>Null unless a bug is added  or alrady exists.</summary>
		private Bug _bug;
		///<summary>The current patient associated to the selected bug submission row. Null if no row selected or if multiple rows selected.</summary>
		private Patient _patCur;
		///<summary></summary>
		private List<JobLink> _listLinks=new List<JobLink>();
		public FormBugSubmission(BugSubmission bugSub,Job job=null) {
			InitializeComponent();
			Lan.F(this);
			_sub=bugSub;
			_jobCur=job;
		}

		private void FormBugSubmission_Load(object sender,EventArgs e) {
			textStack.Text=_sub.ExceptionMessageText+"\r\n"+_sub.ExceptionStackTrace;
			labelRegKey.Text=_sub.RegKey;
			labelDateTime.Text=POut.DateT(_sub.SubmissionDateTime);
			labelVersion.Text=_sub.Info.DictPrefValues[PrefName.ProgramVersion];
			if(_sub.BugId!=0) {//Already associated to a bug
				_bug=Bugs.GetOne(_sub.BugId);
				butAddViewBug.Text="View Bug";
			}
			if(_bug!=null) {
				_listLinks=JobLinks.GetForType(JobLinkType.Bug,_bug.BugId);
				if(_listLinks.Count==1) {
					butAddViewJob.Text="View Job";
				}
			}
			FillOfficeInfoGrid(_sub);
			SetCustomerInfo(_sub);
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

		///<summary>When sub is set, fills customer group box with various information.
		///When null, clears all fields.</summary>
		private void SetCustomerInfo(BugSubmission sub=null) {
			if(sub==null) {
				textStack.Text="";//Also clear any submission specific fields.
				labelCustomerNum.Text="";
				labelCustomerName.Text="";
				labelCustomerState.Text="";
				labelCustomerPhone.Text="";
				labelSubNum.Text="";
				labelLastCall.Text="";
				FillOfficeInfoGrid(null);
				butGoToAccount.Enabled=false;
				butBugTask.Enabled=false;
				return;
			}
			try {
				if(_patCur==null) {
					RegistrationKey key=RegistrationKeys.GetByKey(sub.RegKey);
					_patCur=Patients.GetPat(key.PatNum);
					if(_patCur==null) {
						return;//Should not happen.
					}
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
			butGoToAccount.Enabled=true;
			butBugTask.Enabled=true;
		}
		
		private void butGoToAccount_Click(object sender,EventArgs e) {
			//Button is only enabled if _patCur is not null.
			GotoModule.GotoAccount(_patCur.PatNum);
		}

		private void butBugTask_Click(object sender,EventArgs e) {
			BugSubmissionL.CreateTask(_patCur,_sub);
		}
		
		private void butAddViewBug_Click(object sender,EventArgs e) {
			if(butAddViewBug.Text=="View Bug") {
				OpenBug(_sub);
				return;
			}
			if(AddBug()) {//Bug added.
				butAddViewBug.Text="View Bug";
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
				_bug=null;
				butAddViewBug.Text="Add Bug";
			}
		}

		private bool AddBug() {
			if(butAddViewBug.Text=="View Bug") {
				FormBugEdit formBE=new FormBugEdit();
				formBE.BugCur=_bug;
				formBE.ShowDialog();
				return false;
			}
			_bug=BugSubmissionL.AddBug(_sub,_jobCur);
			return (_bug==null);
		}
		
		private void butAddViewJob_Click(object sender,EventArgs e) {
			if(_bug!=null && _bug.BugId!=0) {//View Job mode
				if(_listLinks.Count==1) {
					JobLink link=_listLinks.First();	
					FormOpenDental.S_GoToJob(link.JobNum);
				}
				else {
					MsgBox.Show(this,"Submission is associated to multiple jobs");
				}
				return;
			}
			_bug=BugSubmissionL.AddBugAndJob(this,new List<BugSubmission>() { _sub },_patCur);
			if(_bug==null) {
				return;
			}
			if(this.Modal) {
				DialogResult=DialogResult.OK;
			}
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
			Close();
		}

	}
}