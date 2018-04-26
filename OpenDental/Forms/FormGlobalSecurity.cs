using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.DirectoryServices;
using System.Linq;
using CodeBase;

namespace OpenDental {
	public partial class FormGlobalSecurity:ODForm {

		public FormGlobalSecurity() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormGlobalSecurity_Load(object sender,EventArgs e) {
			textLogOffAfterMinutes.Text=PrefC.GetInt(PrefName.SecurityLogOffAfterMinutes).ToString();
			checkPasswordsMustBeStrong.Checked=PrefC.GetBool(PrefName.PasswordsMustBeStrong);
			checkPasswordsStrongIncludeSpecial.Checked=PrefC.GetBool(PrefName.PasswordsStrongIncludeSpecial);
			checkPasswordForceWeakToStrong.Checked=PrefC.GetBool(PrefName.PasswordsWeakChangeToStrong);
			checkTimecardSecurityEnabled.Checked=PrefC.GetBool(PrefName.TimecardSecurityEnabled);
			checkCannotEditOwn.Checked=PrefC.GetBool(PrefName.TimecardUsersDontEditOwnCard);
			checkCannotEditOwn.Enabled=checkTimecardSecurityEnabled.Checked;
			checkDomainLoginEnabled.Checked=PrefC.GetBool(PrefName.DomainLoginEnabled);
			textDomainLoginPath.ReadOnly=!checkDomainLoginEnabled.Checked;
			textDomainLoginPath.Text=PrefC.GetString(PrefName.DomainLoginPath);
			checkLogOffWindows.Checked=PrefC.GetBool(PrefName.SecurityLogOffWithWindows);
			checkUserNameManualEntry.Checked=PrefC.GetBool(PrefName.UserNameManualEntry);
			if(PrefC.GetDate(PrefName.BackupReminderLastDateRun).ToShortDateString()==DateTime.MaxValue.AddMonths(-1).ToShortDateString()) {
				checkDisableBackupReminder.Checked=true;
			}
			if(PrefC.GetInt(PrefName.SecurityLockDays)>0) {
				textDaysLock.Text=PrefC.GetInt(PrefName.SecurityLockDays).ToString();
			}
			if(PrefC.GetDate(PrefName.SecurityLockDate).Year>1880) {
				textDateLock.Text=PrefC.GetDate(PrefName.SecurityLockDate).ToShortDateString();
			}
			if(PrefC.GetBool(PrefName.CentralManagerSecurityLock)) {
				butChange.Enabled=false;
				labelGlobalDateLockDisabled.Visible=true;			}
			List<UserGroup> listGroupsNotAdmin=UserGroups.GetList().FindAll(x => !GroupPermissions.HasPermission(x.UserGroupNum,Permissions.SecurityAdmin,0));
			foreach(UserGroup group in listGroupsNotAdmin) {
				int idx=comboGroups.Items.Add(new ODBoxItem<UserGroup>(group.Description,group));
				if(PrefC.GetLong(PrefName.DefaultUserGroup)==group.UserGroupNum) {
					comboGroups.SelectedIndex=idx;
				}
			}
		}

		private void checkTimecardSecurityEnabled_Click(object sender,EventArgs e) {
			checkCannotEditOwn.Enabled=checkTimecardSecurityEnabled.Checked;
		}

		private void checkDomainLoginEnabled_CheckedChanged(object sender,EventArgs e) {
			textDomainLoginPath.ReadOnly=!checkDomainLoginEnabled.Checked;
		}

		private void checkDomainLoginEnabled_MouseUp(object sender,MouseEventArgs e) {
			if(checkDomainLoginEnabled.Checked && string.IsNullOrWhiteSpace(textDomainLoginPath.Text)) {
				if(MsgBox.Show(this,MsgBoxButtons.YesNo,"Would you like to use your current domain as the domain login path?")) {
					try {
						DirectoryEntry rootDSE = new DirectoryEntry("LDAP://RootDSE");
						string defaultNamingContext = rootDSE.Properties["defaultNamingContext"].Value.ToString();
						textDomainLoginPath.Text="LDAP://"+defaultNamingContext;
					}
					catch(Exception ex) {
						FriendlyException.Show(Lan.g(this,"Unable to bind to the current domain."),ex);
					}
				}
			}
		}

		///<summary>Validation for the domain login path provided. 
		///Accepted formats are those listed here: https://msdn.microsoft.com/en-us/library/aa746384(v=vs.85).aspx, excluding plain "LDAP:"
		///Does not check if there are users on the domain object, only that the domain object exists and can be searched.</summary>
		private void textDomainLoginPath_Leave(object sender,EventArgs e) {
			if(checkDomainLoginEnabled.Checked) {
				if(string.IsNullOrWhiteSpace(textDomainLoginPath.Text)) {
					MsgBox.Show(this,"Warning. Domain Login is enabled, but no path has been entered. If you do not provide a domain path,"
						+"you will not be able to assign domain logins to users.");
				}
				else {
					try {
						DirectoryEntry testEntry = new DirectoryEntry(textDomainLoginPath.Text);
						DirectorySearcher search = new DirectorySearcher(testEntry);
						SearchResultCollection testResults = search.FindAll(); //Just do a generic search to verify the object might have users on it
					}
					catch(Exception ex) {
						FriendlyException.Show(Lan.g(this,"An error occurred while attempting to access the provided Domain Login Path."),ex);
					}
				}
			}
		}

		private void checkPasswordsMustBeStrong_Click(object sender,EventArgs e) {
			if(!checkPasswordsMustBeStrong.Checked) {//unchecking the box
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Warning.  If this box is unchecked, the strong password flag on all users will be reset.  "
					+"If strong passwords are again turned on later, then each user will have to edit their password in order to cause the strong password flag to be set again.")) 
				{
					checkPasswordsMustBeStrong.Checked=true;//recheck it.
					return;
				}
			}
		}

		private void checkDisableBackupReminder_Click(object sender,EventArgs e) {
			InputBox inputbox = new InputBox("Please enter password");
			inputbox.setTitle("Change Backup Reminder Settings");
			inputbox.ShowDialog();
			if(inputbox.DialogResult!=DialogResult.OK) {
				checkDisableBackupReminder.Checked=!checkDisableBackupReminder.Checked;
				return;
			}
			if(inputbox.textResult.Text!="abracadabra") {
				checkDisableBackupReminder.Checked=!checkDisableBackupReminder.Checked;
				MsgBox.Show(this,"Wrong password");
				return;
			}
		}

		private void butChange_Click(object sender,EventArgs e) {
			FormSecurityLock FormS = new FormSecurityLock();
			FormS.ShowDialog();//prefs are set invalid within that form if needed.
			if(PrefC.GetInt(PrefName.SecurityLockDays)>0) {
				textDaysLock.Text=PrefC.GetInt(PrefName.SecurityLockDays).ToString();
			}
			else {
				textDaysLock.Text="";
			}
			if(PrefC.GetDate(PrefName.SecurityLockDate).Year>1880) {
				textDateLock.Text=PrefC.GetDate(PrefName.SecurityLockDate).ToShortDateString();
			}
			else {
				textDateLock.Text="";
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textLogOffAfterMinutes.Text!="") {
				try {
					int logOffMinutes = Int32.Parse(textLogOffAfterMinutes.Text);
					if(logOffMinutes<0) {//Automatic log off must be a positive numerical value.
						throw new Exception();
					}
				}
				catch {
					MsgBox.Show(this,"Log off after minutes is invalid.");
					return;
				}
			}
			DataValid.SetInvalid(InvalidType.Security);
			bool invalidatePrefs=false;
			if( //Prefs.UpdateBool(PrefName.PasswordsMustBeStrong,checkPasswordsMustBeStrong.Checked) //handled when box clicked.
				Prefs.UpdateBool(PrefName.TimecardSecurityEnabled,checkTimecardSecurityEnabled.Checked)
				| Prefs.UpdateBool(PrefName.TimecardUsersDontEditOwnCard,checkCannotEditOwn.Checked)
				| Prefs.UpdateBool(PrefName.SecurityLogOffWithWindows,checkLogOffWindows.Checked)
				| Prefs.UpdateBool(PrefName.UserNameManualEntry,checkUserNameManualEntry.Checked)
				| Prefs.UpdateBool(PrefName.PasswordsStrongIncludeSpecial,checkPasswordsStrongIncludeSpecial.Checked)
				| Prefs.UpdateBool(PrefName.PasswordsWeakChangeToStrong,checkPasswordForceWeakToStrong.Checked)
				| Prefs.UpdateInt(PrefName.SecurityLogOffAfterMinutes,PIn.Int(textLogOffAfterMinutes.Text))
				| Prefs.UpdateString(PrefName.DomainLoginPath,PIn.String(textDomainLoginPath.Text))
				| Prefs.UpdateString(PrefName.DomainLoginPath,textDomainLoginPath.Text)
				| Prefs.UpdateString(PrefName.DomainLoginPath,textDomainLoginPath.Text)
				| Prefs.UpdateBool(PrefName.DomainLoginEnabled,checkDomainLoginEnabled.Checked)
				| Prefs.UpdateLong(PrefName.DefaultUserGroup,comboGroups.SelectedIndex==-1?0:comboGroups.SelectedTag<UserGroup>().UserGroupNum)
				) 
			{
				invalidatePrefs=true;
			}
			//if PasswordsMustBeStrong was unchecked, then reset the strong password flags.
			if(Prefs.UpdateBool(PrefName.PasswordsMustBeStrong,checkPasswordsMustBeStrong.Checked) && !checkPasswordsMustBeStrong.Checked) {
				invalidatePrefs=true;
				Userods.ResetStrongPasswordFlags();
			}
			if(checkDisableBackupReminder.Checked) {
				invalidatePrefs|=Prefs.UpdateDateT(PrefName.BackupReminderLastDateRun,DateTime.MaxValue.AddMonths(-1)); //if MaxValue, gives error on startup.
			}
			else {
				invalidatePrefs|=Prefs.UpdateDateT(PrefName.BackupReminderLastDateRun,DateTimeOD.Today);
			}
			if(invalidatePrefs) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
		
	}
}