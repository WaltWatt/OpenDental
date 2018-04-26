using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormTaskSendUser:ODForm {
		private List<TaskList> FilteredList;
		///<summary>If OK, then this will contain the selected TaskListNum</summary>
		public List<long> ListSelectedLists=new List<long>();

		public FormTaskSendUser(bool IsTaskNew=false) {
			InitializeComponent();
			Lan.F(this);
			checkMulti.Visible=IsTaskNew;
		}

		private void FormTaskSendUser_Load(object sender,EventArgs e) {
			List<Userod> UserList=Userods.GetDeepCopy(true);
			List<TaskList> TrunkList=TaskLists.RefreshMainTrunk(Security.CurUser.UserNum,TaskType.All);
			FilteredList=new List<TaskList>();
			for(int i=0;i<UserList.Count;i++){
				if(UserList[i].TaskListInBox==0){
					continue;
				}
				for(int t=0;t<TrunkList.Count;t++){
					if(TrunkList[t].TaskListNum==UserList[i].TaskListInBox){
						FilteredList.Add(TrunkList[t]);
						listMain.Items.Add(TrunkList[t].Descript);
					}
				}
			}
		}

		private void listMain_DoubleClick(object sender,EventArgs e) {
			if(listMain.SelectedIndex==-1){
				return;
			}
			for(int i=0;i<listMain.SelectedIndices.Count;i++) {
				ListSelectedLists.Add(FilteredList[listMain.SelectedIndices[i]].TaskListNum);
			}
			DialogResult=DialogResult.OK;
		}

		private void checkMulti_CheckedChanged(object sender,EventArgs e) {
			if(checkMulti.Checked) {
				listMain.SelectionMode=SelectionMode.MultiSimple;
				label1.Text=Lan.g(this,"Pick users to send to.  If a user is not in the list, then their inbox has not been setup yet.  Click on users to toggle.");
			}
			else {
				listMain.SelectionMode=SelectionMode.One;
				label1.Text=Lan.g(this,"Pick user to send to.  If the user is not in the list, then their inbox has not been setup yet.");
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(listMain.SelectedIndex==-1){
				MsgBox.Show(this,"Please select an item first.");
				return;
			}
			for(int i=0;i<listMain.SelectedIndices.Count;i++) {
				ListSelectedLists.Add(FilteredList[listMain.SelectedIndices[i]].TaskListNum);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		

		
	}
}