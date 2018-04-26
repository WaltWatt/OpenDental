using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;
using System.Linq;

namespace OpenDental {
	public partial class FormJobPermissions:ODForm {
		private long _userNum;
		private List<JobPermission> _jobPermissions;

		///<summary>Pass in the jobNum for existing jobs.</summary>
		public FormJobPermissions(long userNum) {
			_userNum=userNum;
			InitializeComponent();
			Lan.F(this);
		}

		private void FormJobRoles_Load(object sender,EventArgs e) {
			_jobPermissions=JobPermissions.GetForUser(_userNum);
			Enum.GetNames(typeof(JobPerm)).ToList().ForEach(x => listAvailable.Items.Add(x.ToString()));
			_jobPermissions.Select(x=>(int)x.JobPermType).Distinct().ToList().ForEach(x=>listAvailable.SelectedIndices.Add(x));
		}


		private void butEngineer_Click(object sender,EventArgs e) {
			listAvailable.ClearSelected();
			listAvailable.SelectedIndices.Add((int)JobPerm.Engineer);
			listAvailable.SelectedIndices.Add((int)JobPerm.Concept);
		}

		private void butPreExpert_Click(object sender,EventArgs e) {
			listAvailable.ClearSelected();
			listAvailable.SelectedIndices.Add((int)JobPerm.Writeup);
			listAvailable.SelectedIndices.Add((int)JobPerm.Assignment);
			listAvailable.SelectedIndices.Add((int)JobPerm.Review);
			listAvailable.SelectedIndices.Add((int)JobPerm.Engineer);
			listAvailable.SelectedIndices.Add((int)JobPerm.Concept);
		}

		private void butExpert_Click(object sender,EventArgs e) {
			listAvailable.ClearSelected();
			listAvailable.SelectedIndices.Add((int)JobPerm.Writeup);
			listAvailable.SelectedIndices.Add((int)JobPerm.Assignment);
			listAvailable.SelectedIndices.Add((int)JobPerm.Review);
			listAvailable.SelectedIndices.Add((int)JobPerm.Engineer);
			listAvailable.SelectedIndices.Add((int)JobPerm.Concept);
			listAvailable.SelectedIndices.Add((int)JobPerm.Quote);
			listAvailable.SelectedIndices.Add((int)JobPerm.Override);
		}

		private void butTechWriter_Click(object sender,EventArgs e) {
			listAvailable.ClearSelected();
			listAvailable.SelectedIndices.Add((int)JobPerm.Documentation);
		}

		private void butJobManager_Click(object sender,EventArgs e) {
			listAvailable.ClearSelected();
			listAvailable.SelectedIndices.Add((int)JobPerm.Assignment);
			listAvailable.SelectedIndices.Add((int)JobPerm.Approval);
			listAvailable.SelectedIndices.Add((int)JobPerm.Concept);
			listAvailable.SelectedIndices.Add((int)JobPerm.Quote);
			listAvailable.SelectedIndices.Add((int)JobPerm.Override);
		}

		private void butFeatureManager_Click(object sender,EventArgs e) {
			listAvailable.ClearSelected();
			listAvailable.SelectedIndices.Add((int)JobPerm.Concept);
			listAvailable.SelectedIndices.Add((int)JobPerm.FeatureManager);
		}

		private void butQueryManager_Click(object sender,EventArgs e) {
			listAvailable.ClearSelected();
			listAvailable.SelectedIndices.Add((int)JobPerm.SeniorQueryCoordinator);
			listAvailable.SelectedIndices.Add((int)JobPerm.QueryCoordinator);
			listAvailable.SelectedIndices.Add((int)JobPerm.QueryTech);
		}

		private void butCustomerManager_Click(object sender,EventArgs e) {
			listAvailable.ClearSelected();
			listAvailable.SelectedIndices.Add((int)JobPerm.Concept);
			listAvailable.SelectedIndices.Add((int)JobPerm.NotifyCustomer);
		}

		private void butQuoteManager_Click(object sender,EventArgs e) {
			listAvailable.ClearSelected();
			listAvailable.SelectedIndices.Add((int)JobPerm.Concept);
			listAvailable.SelectedIndices.Add((int)JobPerm.Quote);
		}

		private void butOK_Click(object sender,EventArgs e) {
			_jobPermissions.Clear();
			JobPermission jobPermission;
			for(int i=0;i<listAvailable.SelectedIndices.Count;i++) {
				jobPermission=new JobPermission();
				jobPermission.UserNum=_userNum;
				jobPermission.JobPermType=(JobPerm)listAvailable.SelectedIndices[i];
				_jobPermissions.Add(jobPermission);
			}
			JobPermissions.Sync(_jobPermissions,_userNum);
			DataValid.SetInvalid(InvalidType.JobPermission);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}