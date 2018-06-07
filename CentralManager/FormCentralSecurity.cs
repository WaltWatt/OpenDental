using CodeBase;
using OpenDental;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CentralManager {
	public partial class FormCentralSecurity:Form {
		public List<CentralConnection> ListConns;

		public FormCentralSecurity() {
			ListConns=new List<CentralConnection>();
			InitializeComponent();
		}

		private void FormCentralSecurity_Load(object sender,EventArgs e) {
			#region Load Global Settings
			textSyncCode.Text=PrefC.GetString(PrefName.CentralManagerSyncCode);
			checkEnable.Checked=PrefC.GetBool(PrefName.CentralManagerSecurityLock);
			checkAdmin.Checked=PrefC.GetBool(PrefName.SecurityLockIncludesAdmin);
			if(PrefC.GetDate(PrefName.SecurityLockDate).Year>1880) {
				textDate.Text=PrefC.GetDate(PrefName.SecurityLockDate).ToShortDateString();
			}
			if(PrefC.GetInt(PrefName.SecurityLockDays)>0) {
				textDays.Text=PrefC.GetInt(PrefName.SecurityLockDays).ToString();
			}
			#endregion
		}

		#region Global Variable Methods
		private void textDate_KeyDown(object sender,KeyEventArgs e) {
			textDays.Text="";
		}

		private void textDays_KeyDown(object sender,KeyEventArgs e) {
			textDate.Text="";
			textDate.errorProvider1.SetError(textDate,"");
		}
		#endregion

		#region Sync Methods
		private void butSync_Click(object sender,EventArgs e) {
			if(textDate.errorProvider1.GetError(textDate)!="") {
				MsgBox.Show(this,"Please fix error first.");
				return;
			}
			//Enter info into local DB before pushing out to others so we save it.
			int days=PIn.Int(textDays.Text);
			DateTime date=PIn.Date(textDate.Text);
			Prefs.UpdateString(PrefName.SecurityLockDate,POut.Date(date,false));
			Prefs.UpdateInt(PrefName.SecurityLockDays,days);
			Prefs.UpdateBool(PrefName.SecurityLockIncludesAdmin,checkAdmin.Checked) ;
			Prefs.UpdateBool(PrefName.CentralManagerSecurityLock,checkEnable.Checked);
			FormCentralConnections FormCC=new FormCentralConnections();
			FormCC.LabelText.Text=Lans.g("CentralSecurity","Sync will create or update the Central Management users, passwords, and user groups to all selected databases.");
			FormCC.Text=Lans.g("CentralSecurity","Sync Security");
			foreach(CentralConnection conn in ListConns) { 
				FormCC.ListConns.Add(conn.Copy());
			}
			List<CentralConnection> listSelectedConns=new List<CentralConnection>();
			if(FormCC.ShowDialog()==DialogResult.OK) {
				listSelectedConns=FormCC.ListConns;
			}
			else {
				return;
			}
			MsgBoxCopyPaste MsgBoxCopyPaste=new MsgBoxCopyPaste(CentralSyncHelper.SyncAll(listSelectedConns));
			MsgBoxCopyPaste.ShowDialog();
		}

		private void butSyncUsers_Click(object sender,EventArgs e) {
			FormCentralConnections FormCC=new FormCentralConnections();
			FormCC.LabelText.Text=Lans.g("CentralSecurity","Sync will create or update the Central Management users, passwords, and user groups to all selected databases.");
			FormCC.Text=Lans.g("CentralSecurity","Sync Security");
			foreach(CentralConnection conn in ListConns) { 
				FormCC.ListConns.Add(conn.Copy());
			}
			List<CentralConnection> listSelectedConns=new List<CentralConnection>();
			if(FormCC.ShowDialog()==DialogResult.OK) {
				listSelectedConns=FormCC.ListConns;
			}
			else {
				return;
			}
			MsgBoxCopyPaste MsgBoxCopyPaste=new MsgBoxCopyPaste(CentralSyncHelper.SyncUsers(listSelectedConns));
			MsgBoxCopyPaste.ShowDialog();			
		}

		private void butSyncLocks_Click(object sender,EventArgs e) {
			FormCentralConnections FormCC=new FormCentralConnections();
			FormCC.LabelText.Text=Lans.g("CentralSecurity","Sync will create or update the Central Management users, passwords, and user groups to all selected databases.");
			FormCC.Text=Lans.g("CentralSecurity","Sync Security");
			foreach(CentralConnection conn in ListConns) { 
				FormCC.ListConns.Add(conn.Copy());
			}
			List<CentralConnection> listSelectedConns=new List<CentralConnection>();
			if(FormCC.ShowDialog()==DialogResult.OK) {
				listSelectedConns=FormCC.ListConns;
			}
			else {
				return;
			}
			MsgBoxCopyPaste MsgBoxCopyPaste=new MsgBoxCopyPaste(CentralSyncHelper.SyncLocks(listSelectedConns));
			MsgBoxCopyPaste.ShowDialog();
		}
		#endregion

		///<summary>Add user button.</summary>
		private void userControlSecurityTabs_AddUserClick(object sender,SecurityEventArgs e) {
			Userod user = new Userod();
			user.IsNew=true;
			FormCentralUserEdit FormCU = new FormCentralUserEdit(user);
			FormCU.ShowDialog();
			if(FormCU.DialogResult == DialogResult.OK) {//update to reflect changes that were made in FormUserEdit.
				userControlSecurityTabs.FillGridUsers();//New user is not in grid yet, add them.
				userControlSecurityTabs.SelectedUser=FormCU.UserCur;//Selects the user that was just added in the grid.
				userControlSecurityTabs.RefreshUserTabGroups();//Previously selected users User Groups are still selected, refresh for UserCur.
			}
		}

		///<summary>Edit user button.</summary>
		private void userControlSecurityTabs_EditUserClick(object sender,SecurityEventArgs e) {
			FormCentralUserEdit FormCUE = new FormCentralUserEdit(e.User);
			FormCUE.ShowDialog();
			if(FormCUE.DialogResult == DialogResult.OK) {
				userControlSecurityTabs.FillGridUsers();
				userControlSecurityTabs.RefreshUserTabGroups();
			}
		}

		///<summary>Add user group button.</summary>
		private void userControlSecurityTabs_AddUserGroupClick(object sender,SecurityEventArgs e) {
			UserGroup group = new UserGroup();
			group.IsNew=true;
			FormCentralUserGroupEdit FormU = new FormCentralUserGroupEdit(group);
			FormU.ShowDialog();
			if(FormU.DialogResult == DialogResult.OK) {
				userControlSecurityTabs.FillListUserGroupTabUserGroups();
				userControlSecurityTabs.SelectedUserGroup=group;
			}
		}

		///<summary>Edit user group button.</summary>
		private void userControlSecurityTabs_EditUserGroupClick(object sender,SecurityEventArgs e) {
			FormCentralUserGroupEdit FormU = new FormCentralUserGroupEdit(e.Group);
			FormU.ShowDialog();
			if(FormU.DialogResult==DialogResult.OK) {
				userControlSecurityTabs.FillListUserGroupTabUserGroups();
			}
		}

		private DialogResult userControlSecurityTabs_ReportPermissionChecked(object sender,SecurityEventArgs e) {
			GroupPermission perm = e.Perm;
			FormCentralReportSetup FormCRS = new FormCentralReportSetup(perm.UserGroupNum,true);
			FormCRS.ShowDialog();
			if(FormCRS.DialogResult==DialogResult.Cancel) {
				return FormCRS.DialogResult;
			}
			if(!FormCRS.HasReportPerms) {//Only insert base Reports permission if the user actually has any reports allowed
				return FormCRS.DialogResult;
			}
			try {
				GroupPermissions.Insert(perm);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return DialogResult.Cancel;
			}
			return FormCRS.DialogResult;
		}

		private DialogResult userControlSecurityTabs_GroupPermissionChecked(object sender,SecurityEventArgs e) {
			FormCentralGroupPermEdit FormCG = new FormCentralGroupPermEdit(e.Perm);
			FormCG.ShowDialog();
			return FormCG.DialogResult;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textDate.errorProvider1.GetError(textDate)!="") {
				MsgBox.Show(this,"Please fix error first.");
				return;
			}
			//Enter info into local DB before pushing out to others so we save it.
			int days=PIn.Int(textDays.Text);
			DateTime date=PIn.Date(textDate.Text);
			Prefs.UpdateString(PrefName.SecurityLockDate,POut.Date(date,false));
			Prefs.UpdateInt(PrefName.SecurityLockDays,days);
			Prefs.UpdateBool(PrefName.SecurityLockIncludesAdmin,checkAdmin.Checked) ;
			Prefs.UpdateBool(PrefName.CentralManagerSecurityLock,checkEnable.Checked);
			DialogResult=DialogResult.OK;
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}
