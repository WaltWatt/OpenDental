using OpenDental;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CodeBase;

namespace CentralManager {
	public partial class FormCentralUserEdit:Form {
		public Userod UserCur;
		private List<AlertSub> _listAlertSubsOld;

		public FormCentralUserEdit(Userod user) {
			InitializeComponent();
			UserCur=user.Copy();
		}		

		private void FormCentralUserEdit_Load(object sender,EventArgs e) {
			checkIsHidden.Checked=UserCur.IsHidden;
			textUserName.Text=UserCur.UserName;
			List<UserGroup> listUserGroups=UserGroups.GetDeepCopy();
			for(int i = 0;i < listUserGroups.Count;i++) {
				UserGroup groupCur=listUserGroups[i];
				ODBoxItem<UserGroup> boxItemGroup = new ODBoxItem<UserGroup>(groupCur.Description,groupCur);
				listUserGroup.Items.Add(boxItemGroup);
				if(UserCur.IsInUserGroup(groupCur.UserGroupNum)){
					listUserGroup.SetSelected(i,true);
				}
			}
			if(listUserGroup.SelectedIndex==-1){//never allowed to delete last group, so this won't fail
				listUserGroup.SelectedIndex=0;
			}
			if(UserCur.Password==""){
				butPassword.Text="Create Password";
			}
			_listAlertSubsOld=AlertSubs.GetAllForUser(Security.CurUser.UserNum);
			listAlertSubMulti.Items.Clear();
			string[] arrayAlertTypes=Enum.GetNames(typeof(AlertType));
			for(int i=0;i<arrayAlertTypes.Length;i++){
				listAlertSubMulti.Items.Add(arrayAlertTypes[i]);
				listAlertSubMulti.SetSelected(i,_listAlertSubsOld.Exists(x => x.Type==(AlertType)i));
			}
			if(UserCur.IsNew) {
				butUnlock.Visible=false;
			}
		}

		private void butPassword_Click(object sender,EventArgs e) {
			bool isCreate=false;
			if(string.IsNullOrEmpty(UserCur.Password)) {
				isCreate=true;
			}
			FormCentralUserPasswordEdit FormCPE=new FormCentralUserPasswordEdit(isCreate,UserCur.UserName);
			FormCPE.IsInSecurityWindow=true;
			FormCPE.ShowDialog();
			if(FormCPE.DialogResult==DialogResult.Cancel){
				return;
			}
			UserCur.Password=FormCPE.HashedResult;
			if(UserCur.Password==""){
				butPassword.Text="Create Password";
			}
			else{
				butPassword.Text="Change Password";
			}
		}

		private void butUnlock_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,true,"Users can become locked when invalid credentials have been entered several times in a row.\r\n"
				+"Unlock this user so that more log in attempts can be made?")) 
			{
				return;
			}
			UserCur.DateTFail=DateTime.MinValue;
			UserCur.FailedAttempts=0;
			try {
				Userods.Update(UserCur);//This will also commit other things about the user if they've changed.  Oh well.
				MsgBox.Show(this,"User has been unlocked.");
			}
			catch(Exception) {
				MsgBox.Show(this,"There was a problem unlocking this user.  Please call support or wait the allotted lock time.");
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textUserName.Text=="") {
				MessageBox.Show(this,"Please enter a username.");
				return;
			}
			if(listUserGroup.SelectedItems.Count == 0) {
				MessageBox.Show(this,"Every user must be associated to at least one User Group.");
				return;
			}
			List<AlertSub> listAlertSubsCur=new List<AlertSub>();
			foreach(int index in listAlertSubMulti.SelectedIndices) {
				AlertSub alertSub=new AlertSub();
				alertSub.ClinicNum=0;
				alertSub.UserNum=Security.CurUser.UserNum;
				alertSub.Type=(AlertType)index;
				listAlertSubsCur.Add(alertSub);
			}
			AlertSubs.Sync(listAlertSubsCur,_listAlertSubsOld);
			UserCur.IsHidden=checkIsHidden.Checked;
			UserCur.UserName=textUserName.Text;
			if(UserCur.UserNum==Security.CurUser.UserNum) {
				Security.CurUser.UserName=textUserName.Text;
				//They changed their logged in user's information.  Update for when they sync then attempt to connect to remote DB.
			}
			UserCur.EmployeeNum=0;
			UserCur.ProvNum=0;
			UserCur.ClinicNum=0;
			UserCur.ClinicIsRestricted=false;
			try{
				if(UserCur.IsNew){
					//also updates the user's UserNumCEMT to be the user's usernum.
					long userNum=Userods.Insert(UserCur,listUserGroup.SelectedItems.OfType<ODBoxItem<UserGroup>>().Select(x => x.Tag.UserGroupNum).ToList(),true);
				}
				else{
					Userods.Update(UserCur,listUserGroup.SelectedItems.OfType<ODBoxItem<UserGroup>>().Select(x => x.Tag.UserGroupNum).ToList());
				}
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
				return;
			}
			Cache.Refresh(InvalidType.Security);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}
