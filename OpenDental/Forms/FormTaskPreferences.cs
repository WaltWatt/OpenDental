using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormTaskPreferences:ODForm {

		public FormTaskPreferences() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormTaskPreferences_Load(object sender,EventArgs e) {
			checkShowLegacyRepeatingTasks.Checked=PrefC.GetBool(PrefName.TasksUseRepeating);
			checkTaskListAlwaysShow.Checked=PrefC.GetBool(PrefName.TaskListAlwaysShowsAtBottom);
			if(checkTaskListAlwaysShow.Checked) {
				groupBoxComputerDefaults.Enabled=true;
			}
			else {
				groupBoxComputerDefaults.Enabled=false;
			}
			checkTasksNewTrackedByUser.Checked=PrefC.GetBool(PrefName.TasksNewTrackedByUser);
			checkShowOpenTickets.Checked=PrefC.GetBool(PrefName.TasksShowOpenTickets);
			checkBoxTaskKeepListHidden.Checked=ComputerPrefs.LocalComputer.TaskKeepListHidden;
			if(ComputerPrefs.LocalComputer.TaskDock==0) {
				radioBottom.Checked=true;
			}
			else {
				radioRight.Checked=true;
			}
			validNumX.Text=ComputerPrefs.LocalComputer.TaskX.ToString();
			validNumY.Text=ComputerPrefs.LocalComputer.TaskY.ToString();
			checkTaskSortApptDateTime.Checked=PrefC.GetBool(PrefName.TaskSortApptDateTime);
		}

		private void butTaskInboxSetup_Click(object sender,EventArgs e) {
			//If we ever allow users to enter this window without Setup permissions add Setup permission check here.
			FormTaskInboxSetup FormT=new FormTaskInboxSetup();
			FormT.ShowDialog();
		}

		private void checkTaskListAlwaysShow_CheckedChanged(object sender,EventArgs e) {
			if(checkTaskListAlwaysShow.Checked) {
				groupBoxComputerDefaults.Enabled=true;
			}
			else {
				groupBoxComputerDefaults.Enabled=false;
			}
		}

		private void checkBoxTaskKeepListHidden_CheckedChanged(object sender,EventArgs e) {
			if(checkBoxTaskKeepListHidden.Checked) {
				radioBottom.Enabled=false;
				radioRight.Enabled=false;
				labelX.Enabled=false;
				labelY.Enabled=false;
				validNumX.Enabled=false;
				validNumY.Enabled=false;
			}
			else {
				radioBottom.Enabled=true;
				radioRight.Enabled=true;
				labelX.Enabled=true;
				labelY.Enabled=true;
				validNumX.Enabled=true;
				validNumY.Enabled=true;
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(validNumX.errorProvider1.GetError(validNumX)!="" | validNumY.errorProvider1.GetError(validNumY)!="") {
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return;
			}
			bool changed = false;
			if(Prefs.UpdateBool(PrefName.TaskListAlwaysShowsAtBottom,checkTaskListAlwaysShow.Checked)
				| Prefs.UpdateBool(PrefName.TasksUseRepeating,checkShowLegacyRepeatingTasks.Checked)
				| Prefs.UpdateBool(PrefName.TasksNewTrackedByUser,checkTasksNewTrackedByUser.Checked)
				| Prefs.UpdateBool(PrefName.TasksShowOpenTickets,checkShowOpenTickets.Checked)
				| Prefs.UpdateBool(PrefName.TaskSortApptDateTime,checkTaskSortApptDateTime.Checked)
				) {
				changed=true;
			}
			if(ComputerPrefs.LocalComputer.TaskKeepListHidden!=checkBoxTaskKeepListHidden.Checked) {
				ComputerPrefs.LocalComputer.TaskKeepListHidden=checkBoxTaskKeepListHidden.Checked;
				changed=true;//needed to trigger screen refresh
			}
			if(radioBottom.Checked && ComputerPrefs.LocalComputer.TaskDock!=0) {
				ComputerPrefs.LocalComputer.TaskDock=0;
				changed=true;
			}
			else if(!radioBottom.Checked && ComputerPrefs.LocalComputer.TaskDock!=1) {
				ComputerPrefs.LocalComputer.TaskDock=1;
				changed=true;
			}
			if(ComputerPrefs.LocalComputer.TaskX!=PIn.Int(validNumX.Text)) {
				ComputerPrefs.LocalComputer.TaskX=PIn.Int(validNumX.Text);
				changed=true;
			}
			if(ComputerPrefs.LocalComputer.TaskY!=PIn.Int(validNumY.Text)) {
				ComputerPrefs.LocalComputer.TaskY=PIn.Int(validNumY.Text);
				changed=true;
			}
			if(changed) {
				DataValid.SetInvalid(InvalidType.Prefs,InvalidType.Computers);
				ComputerPrefs.Update(ComputerPrefs.LocalComputer);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}