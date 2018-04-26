using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenDental {
	public partial class FormVersionPrompt:ODForm {
		public string VersionText="";
		public bool IsHeadOnly=false;
		public FormVersionPrompt(string titleTextOverride="",bool showHeadOnly=false) {
			InitializeComponent();
			butHead.Visible=showHeadOnly;
			if(!string.IsNullOrEmpty(titleTextOverride)) {
				Text=titleTextOverride;
			}
		}

		private void FormVersionPrompt_Load(object sender,EventArgs e) {
			textVersions.Text=VersionText;
		}

		private void butHead_Click(object sender,EventArgs e) {
			IsHeadOnly=true;
			textVersions.Text="Head Only ("+VersionReleases.GetPossibleHeadRelease()+")";
			textVersions.Focus();
			textVersions.SelectionStart=11;
			textVersions.SelectionLength=4;
		}

		private void butLast1_Click(object sender,EventArgs e) {
			IsHeadOnly=false;
			textVersions.Text=VersionReleases.GetLastReleases(1);
		}

		private void butLast2_Click(object sender,EventArgs e) {
			IsHeadOnly=false;
			textVersions.Text=VersionReleases.GetLastReleases(2);
		}

		private void butLast3_Click(object sender,EventArgs e) {
			IsHeadOnly=false;
			textVersions.Text=VersionReleases.GetLastReleases(3);
		}

		private void butOK_Click(object sender,EventArgs e) {
			VersionText=textVersions.Text;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}
