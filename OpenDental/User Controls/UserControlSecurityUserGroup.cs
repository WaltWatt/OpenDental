using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.ComponentModel;
using CodeBase;

namespace OpenDental {
	///<summary>This control contains the User and User Group edit tabs for use in FormSecurity 
	///and FormCentralSecurity so that any changes need not be made in multiple places.
	///The implementing class should handle all the "Security Events" listed in the designer.</summary>
	public partial class UserControlSecurityUserGroup:UserControl {
		#region Private Variables
		///<summary>When true, various selection and click methods will not be executed. 
		///Set to true when loading/filling lists/grids.
		/// If this is set to true, ALWAYS set it back to false when you are done.</summary>
		private bool _isFillingList;
		///<summary>Used to filter the list of users shown in the "Users" tab.</summary>
		private Dictionary<long,Provider> _dictProvNumProvs;
		#endregion
		#region Public Variables/Events
		///<summary>The form that implements this control should use their own Add and Edit User/UserGroup forms.</summary>
		public delegate void SecurityTabsEventHandler(object sender,SecurityEventArgs e);
		///<summary>An eventhandler that returns a DialogResult, so that the form that implements this security tree 
		///can use their own Report Permission and Group Permission forms. The result of this event is passed into the Security Tree.</summary>
		public delegate DialogResult SecurityTreeEventHandler(object sender,SecurityEventArgs e);
		[Category("Security Events"), Description("Occurs when the Add User button is clicked.")]
		public event SecurityTabsEventHandler AddUserClick = null;
		[Category("Security Events"), Description("Occurs when the Edit User button is clicked.")]
		public event SecurityTabsEventHandler EditUserClick = null;
		[Category("Security Events"), Description("Occurs when the Add User Group button is clicked.")]
		public event SecurityTabsEventHandler AddUserGroupClick = null;
		[Category("Security Events"), Description("Occurs when the Edit User Group button is clicked.")]
		public event SecurityTabsEventHandler EditUserGroupClick = null;
		[Category("Security Events"), Description("Occurs when the Report Permission is checked.")]
		public event SecurityTreeEventHandler ReportPermissionChecked = null;
		[Category("Security Events"), Description("Occurs when a date-editable Group Permission is checked.")]
		public event SecurityTreeEventHandler GroupPermissionChecked = null;
		#endregion
		#region Properties
		#region Public
		[Category("Security Properties"), Description("Set to true when this user control is used in the CEMT tool. "+
			"When true, includes CEMT users and user groups and hides the User Filters.")]
		public bool IsForCEMT {
			get;
			set;
		}
		#endregion
		#region Private
		///<summary>The user selected in the "Users" tab. Setting the SelectedUser to null or a user that does not exist in the listbox does nothing.</summary>
		public Userod SelectedUser {
			get {
				return (gridUsers.SelectedTag<Userod>());//Returns null if no selections
			}
			set {
				if(value == null) {
					return;
				}
				for(int i=0;i<gridUsers.Rows.Count;i++){
					gridUsers.SetSelected(i,false);
					if(((Userod)(gridUsers.Rows[i].Tag)).UserNum==value.UserNum) {
						gridUsers.SetSelected(i,true);
						break;
					}
				}
			}
		}

		///<summary>The usergroup selected in the "User Groups" tab. 
		///Setting the SelectedUserGroup to null or a usergroup that does not exist in the listbox does nothing.</summary>
		public UserGroup SelectedUserGroup {
			get { return ((ODBoxItem<UserGroup>)listUserGroupTabUserGroups.SelectedItem)?.Tag; }
			set {
				if(value == null) {
					return;
				}
				foreach(ODBoxItem<UserGroup> boxItemUserGroupCur in listUserGroupTabUserGroups.Items) {
					if(boxItemUserGroupCur.Tag.UserGroupNum == value.UserGroupNum) {
						listUserGroupTabUserGroups.SelectedItem=boxItemUserGroupCur;
						break;
					}
				}
			}
		}
		#endregion
		#endregion

		public UserControlSecurityUserGroup() {
			InitializeComponent();
		}

		private void UserControlUserGroupSecurity_Load(object sender,EventArgs e) {
			if(IsForCEMT) {
				groupBox2.Visible=false;
				gridUsers.Bounds = new Rectangle(gridUsers.Bounds.X,securityTreeUser.Bounds.Y,gridUsers.Bounds.Width,securityTreeUser.Bounds.Height);
				listUserTabUserGroups.Bounds = new Rectangle(listUserTabUserGroups.Bounds.X,securityTreeUser.Bounds.Y,listUserTabUserGroups.Bounds.Width,securityTreeUser.Bounds.Height);
				labelUserTabUserGroups.Location = new Point(labelUserTabUserGroups.Location.X,labelPerm.Location.Y);
			}
			if(!this.DesignMode) {
				securityTreeUser.FillTreePermissionsInitial();
				#region Load Users Tab
				FillFilters();
				FillGridUsers();
				FillUserTabGroups();
				gridUsers.SetSelected(0,true);//Default to top user in grid.
				#endregion
				#region Load UserGroups Tab
				securityTreeUserGroup.FillTreePermissionsInitial();
				FillListUserGroupTabUserGroups();
				securityTreeUserGroup.FillForUserGroup(SelectedUserGroup.UserGroupNum);
				FillAssociatedUsers();
				#endregion
			}
		}

		#region User Tab Methods
		///<summary>Fills the filter comboboxes on the "Users" tab.</summary>
		private void FillFilters() {
			foreach(UserFilters filterCur in Enum.GetValues(typeof(UserFilters))) {
				if(PrefC.GetBool(PrefName.EasyHideDentalSchools) && (filterCur == UserFilters.Students || filterCur == UserFilters.Instructors)) {
					continue;
				}
				comboShowOnly.Items.Add(new ODBoxItem<UserFilters>(Lan.g(this,filterCur.GetDescription()),filterCur));
			}
			comboShowOnly.SelectedIndex=0;
			comboSchoolClass.Items.Add(new ODBoxItem<SchoolClass>(Lan.g(this,"All")));
			comboSchoolClass.SelectedIndex=0;
			foreach(SchoolClass schoolClassCur in SchoolClasses.GetDeepCopy()) {
				comboSchoolClass.Items.Add(new ODBoxItem<SchoolClass>(SchoolClasses.GetDescript(schoolClassCur),schoolClassCur));
			}
			if(PrefC.HasClinicsEnabled) {
				comboClinic.Visible=true;
				labelClinic.Visible=true;
				comboClinic.Items.Clear();
				comboClinic.Items.Add(new ODBoxItem<Clinic>(Lan.g(this,"All Clinics")));
				comboClinic.SelectedIndex=0;
				foreach(Clinic clinicCur in Clinics.GetDeepCopy(true)) {
					comboClinic.Items.Add(new ODBoxItem<Clinic>(clinicCur.Abbr,clinicCur));
				}
			}
			comboGroups.Items.Clear();
			comboGroups.Items.Add(new ODBoxItem<UserGroup>(Lan.g(this,"All Groups")));
			comboGroups.SelectedIndex=0;
			foreach(UserGroup groupCur in UserGroups.GetList(IsForCEMT)) {
				comboGroups.Items.Add(new ODBoxItem<UserGroup>(groupCur.Description,groupCur));
			}
		}

		///<summary>Fills listUserTabUserGroups.</summary>
		private void FillUserTabGroups() {
			_isFillingList=true;
			listUserTabUserGroups.Items.Clear();
			foreach(UserGroup groupCur in UserGroups.GetList(IsForCEMT)) {
				listUserTabUserGroups.Items.Add(new ODBoxItem<UserGroup>(groupCur.Description,groupCur));
			}
			_isFillingList=false;
			RefreshUserTabGroups();
		}

		///<summary>Returns a filtered list of userods that should be displayed. Returns all users when IsCEMT is true.</summary>
		private List<Userod> GetFilteredUsersHelper() {
			List<Userod> retVal = Userods.GetDeepCopy();
			if(IsForCEMT) {
				return retVal;
			}
			if(_dictProvNumProvs == null) { //fill the dictionary if needed
				_dictProvNumProvs=Providers.GetMultProviders(Userods.GetDeepCopy().Select(x => x.ProvNum).ToList()).ToDictionary(x => x.ProvNum,x => x);
			}
			retVal.RemoveAll(x => x.UserNumCEMT>0);//NEVER show CEMT users when not in the CEMT tool.
			if(!checkShowHidden.Checked) {
				retVal.RemoveAll(x => x.IsHidden);
			}
			long classNum = 0;
			if(comboSchoolClass.Visible && comboSchoolClass.SelectedIndex>0) {
				classNum=((ODBoxItem<SchoolClass>)comboSchoolClass.SelectedItem).Tag.SchoolClassNum;
			}
			switch(((ODBoxItem<UserFilters>)comboShowOnly.SelectedItem).Tag) {
				case UserFilters.Employees:
					retVal.RemoveAll(x => x.EmployeeNum==0);
					break;
				case UserFilters.Providers:
					retVal.RemoveAll(x => x.ProvNum==0);
					break;
				case UserFilters.Students:
					//might not count user as student if attached to invalid providers.
					retVal.RemoveAll(x => !_dictProvNumProvs.ContainsKey(x.ProvNum) || _dictProvNumProvs[x.ProvNum].IsInstructor);
					if(classNum>0) {
						retVal.RemoveAll(x => _dictProvNumProvs[x.ProvNum].SchoolClassNum!=classNum);
					}
					break;
				case UserFilters.Instructors:
					retVal.RemoveAll(x => !_dictProvNumProvs.ContainsKey(x.ProvNum) || !_dictProvNumProvs[x.ProvNum].IsInstructor);
					if(classNum>0) {
						retVal.RemoveAll(x => _dictProvNumProvs[x.ProvNum].SchoolClassNum!=classNum);
					}
					break;
				case UserFilters.Other:
					retVal.RemoveAll(x => x.EmployeeNum!=0 || x.ProvNum!=0);
					break;
				case UserFilters.AllUsers:
				default:
					break;
			}
			if(comboClinic.SelectedIndex>0) {
				retVal.RemoveAll(x => x.ClinicNum!=((ODBoxItem<Clinic>)comboClinic.SelectedItem).Tag.ClinicNum);
			}
			if(comboGroups.SelectedIndex>0) {
				retVal.RemoveAll(x => !x.IsInUserGroup(((ODBoxItem<UserGroup>)comboGroups.SelectedItem).Tag.UserGroupNum));
			}
			if(!string.IsNullOrWhiteSpace(textPowerSearch.Text)) {
				switch(((ODBoxItem<UserFilters>)comboShowOnly.SelectedItem).Tag) {
					case UserFilters.Employees:
						retVal.RemoveAll(x => !Employees.GetNameFL(x.EmployeeNum).ToLower().Contains(textPowerSearch.Text.ToLower()));
						break;
					case UserFilters.Providers:
					case UserFilters.Students:
					case UserFilters.Instructors:
						retVal.RemoveAll(x => !_dictProvNumProvs[x.ProvNum].GetLongDesc().ToLower().Contains(textPowerSearch.Text.ToLower()));
						break;
					case UserFilters.AllUsers:
					case UserFilters.Other:
					default:
						retVal.RemoveAll(x => !x.UserName.ToLower().Contains(textPowerSearch.Text.ToLower()));
						break;
				}
			}
			return retVal;
		}

		///<summary>Refreshes the security tree in the "Users" tab.</summary>
		private void RefreshUserTree() {
			securityTreeUser.FillForUserGroup(listUserTabUserGroups.SelectedItems.OfType<ODBoxItem<UserGroup>>().Select(x => x.Tag.UserGroupNum).ToList());
		}

		///<summary>Refreshes the UserGroups list box on the "User" tab. Also refreshes the security tree. 
		///Public so that it can be called from the Form that implements this control.</summary>
		public void RefreshUserTabGroups() {
			_isFillingList=true;
			listUserTabUserGroups.ClearSelected();
			if(SelectedUser==null) {//No selected user, disable because modifying selections woudn't make sense.
				listUserTabUserGroups.Enabled=false;
			}
			else {
				listUserTabUserGroups.Enabled=true;
				List<long> listUserGroupNums = SelectedUser.GetGroups(IsForCEMT).Select(x => x.UserGroupNum).ToList();
				for(int i = 0;i < listUserTabUserGroups.Items.Count;i++) {
					if(listUserGroupNums.Contains(((ODBoxItem<UserGroup>)listUserTabUserGroups.Items[i]).Tag.UserGroupNum)) {
						listUserTabUserGroups.SetSelected(i,true);
					}
				}
			}
			_isFillingList=false;
			//RefreshTree takes a while (it has to draw many images) so this is to show the usergroup selections before loading the tree.
			Application.DoEvents();
			RefreshUserTree();
		}

		private void listUserTabUserGroups_SelectedIndexChanged(object sender,EventArgs e) {
			if(_isFillingList || SelectedUser==null) {
				return;
			}
			if(listUserTabUserGroups.SelectedItems.OfType<ODBoxItem<UserGroup>>().Select(x => x.Tag.UserGroupNum).Count() == 0) {
				MsgBox.Show(this,"A user must have at least one User Group attached.");
				RefreshUserTabGroups(); //set the groups back to what they were before.
				return;
			}
			List<long> listSelectedUserUserGroupsOld=SelectedUser.GetGroups(IsForCEMT).Select(x => x.UserGroupNum).ToList();
			List<long> listSelectedUserUserGroups=listUserTabUserGroups.SelectedTags<UserGroup>().Select(x => x.UserGroupNum).ToList();
			if(//Current selected groups do not contain SecurityAdmin permission
				GroupPermissions.GetForUserGroups(listSelectedUserUserGroups,Permissions.SecurityAdmin).Count==0
				//Selected user had SecurityAdmin permission before new selections
				&& GroupPermissions.GetForUserGroups(listSelectedUserUserGroupsOld,Permissions.SecurityAdmin).Count>0) 
			{
				//The SelectedUser is no longer part of SecurityAdmin group. Check that at least one other user is part of a SecurityAdmin Group.
				if(!Userods.IsSomeoneElseSecurityAdmin(SelectedUser)) {
					MsgBox.Show(this,Lan.g(this,"At least one user must have Security Admin permission."));
					RefreshUserTabGroups(); //set the groups back to what they were before.
					return;
				}
			}
			if(UserGroupAttaches.SyncForUser(SelectedUser,listSelectedUserUserGroups)!=0) {
				UserGroupAttaches.RefreshCache();//only refreshes local cache. 
			}
			RefreshUserTree();
		}

		private void comboShowOnly_SelectionIndexChanged(object sender,EventArgs e) {
			string filterType;
			switch(((ODBoxItem<UserFilters>)comboShowOnly.SelectedItem).Tag) {
				case UserFilters.Employees:
					filterType="Employee Name";
					break;
				case UserFilters.Providers:
				case UserFilters.Students:
				case UserFilters.Instructors:
					filterType="Provider Name";
					break;
				case UserFilters.AllUsers:
				case UserFilters.Other:
				default:
					filterType="Username";
					break;
			}
			if(((ODBoxItem<UserFilters>)comboShowOnly.SelectedItem).Tag==UserFilters.Students) {
				labelSchoolClass.Visible=true;
				comboSchoolClass.Visible=true;
			}
			else {
				labelSchoolClass.Visible=false;
				comboSchoolClass.Visible=false;
			}
			labelFilterType.Text=Lan.g(this,filterType);
			textPowerSearch.Text=string.Empty;
			FillGridUsers();
		}

		private void Filter_Changed(object sender,EventArgs e) {
			FillGridUsers();
		}

		///<summary>Fills gridUsers. Public so that it can be called from the Form that implements this control.</summary>
		public void FillGridUsers() {
			_isFillingList=true;
			Userod selectedUser=SelectedUser;//preserve user selection.
			gridUsers.BeginUpdate();
			gridUsers.Columns.Clear();
			string tableName="TableSecurity";
			gridUsers.Columns.Add(new ODGridColumn(Lan.g(tableName,"Username"),90));
			gridUsers.Columns.Add(new ODGridColumn(Lan.g(tableName,"Employee"),90));
			gridUsers.Columns.Add(new ODGridColumn(Lan.g(tableName,"Provider"),90));
			if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
				gridUsers.Columns.Add(new ODGridColumn(Lan.g(tableName,"Clinic"),80));
				gridUsers.Columns.Add(new ODGridColumn(Lan.g(tableName,"Clinic\r\nRestr"),38,HorizontalAlignment.Center));
			}
			gridUsers.Columns.Add(new ODGridColumn(Lan.g(tableName,"Strong\r\nPwd"),45,HorizontalAlignment.Center));
			gridUsers.Rows.Clear();
			List<Userod> listFilteredUsers=GetFilteredUsersHelper();
			foreach(Userod user in listFilteredUsers) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(user.UserName);
				row.Cells.Add(Employees.GetNameFL(user.EmployeeNum));
				row.Cells.Add(Providers.GetLongDesc(user.ProvNum));
				if(!PrefC.GetBool(PrefName.EasyNoClinics)) {
					row.Cells.Add(Clinics.GetAbbr(user.ClinicNum));
					row.Cells.Add(user.ClinicIsRestricted?"X":"");
				}
				row.Cells.Add(user.PasswordIsStrong?"X":"");
				row.Tag=user;
				gridUsers.Rows.Add(row);
			}
			gridUsers.EndUpdate();
			_isFillingList=false;//Done filling the grid.
			//Selection logic has to occur after ODGrid.EndUpdate().
			if(selectedUser==null || !listFilteredUsers.Any(x => x.UserNum==selectedUser.UserNum)) {
				//No previously selected user, or previous selection not in filtered list.
				gridUsers.SetSelected(0,true);//Default to first user.
			}
			else {//Previous selection still exists in grid, so select it again.
				SelectedUser=selectedUser;//Reselect previously selected user.
			}
			RefreshUserTabGroups();
		}

		private void gridUsers_TitleAddClick(object sender,EventArgs e) {
			if(_isFillingList) {
				return;
			}
			//Call an event that bubbles back up to the calling Form.
			AddUserClick?.Invoke(this,new SecurityEventArgs(new Userod()));
		}

		private void gridUsers_CellClick(object sender,ODGridClickEventArgs e) {
			if(_isFillingList) {
				return;
			}
			//Refresh the selected groups and the security tree
			RefreshUserTabGroups();
		}

		private void gridUsers_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(_isFillingList) {
				return;
			}
			//Call an event that bubbles back up to the calling Form.
			EditUserClick?.Invoke(this,new SecurityEventArgs(SelectedUser));
		}

		#endregion

		#region UserGroup Tab Methods
		///<summary>Fills listUserGroupTabUserGroups. Public so that it can be called from the Form that implements this control.</summary>
		public void FillListUserGroupTabUserGroups() {
			_isFillingList=true;
			UserGroup selectedGroup = SelectedUserGroup; //Preserve Usergroup selection.
			listUserGroupTabUserGroups.Items.Clear();
			foreach(UserGroup groupCur in UserGroups.GetList(IsForCEMT)) {
				ODBoxItem<UserGroup> boxItemCur = new ODBoxItem<UserGroup>(groupCur.Description,groupCur);
				listUserGroupTabUserGroups.Items.Add(boxItemCur);
				if(selectedGroup != null && groupCur.UserGroupNum == selectedGroup.UserGroupNum) {
					listUserGroupTabUserGroups.SelectedItem = boxItemCur;
				}
			}
			_isFillingList=false;
			if(listUserGroupTabUserGroups.SelectedItem == null) {
				listUserGroupTabUserGroups.SetSelected(0,true);
			}
		}

		///<summary>Fills listAssociatedUsers, which displays the users that are currently associated to the selected usergroup.
		///This also dynamically sets the height of the control.</summary>
		private void FillAssociatedUsers() {
			listAssociatedUsers.Items.Clear();
			List<Userod> listUsers = Userods.GetForGroup(SelectedUserGroup.UserGroupNum);
			foreach(Userod userCur in listUsers) {
				listAssociatedUsers.Items.Add(new ODBoxItem<Userod>(userCur.UserName,userCur));
			}
			if(listAssociatedUsers.Items.Count == 0) {
				listAssociatedUsers.Items.Add(new ODBoxItem<Userod>(Lan.g(this,"None")));
			}
		}

		private void listUserGroupTabUserGroups_SelectedIndexChanged(object sender,EventArgs e) {
			if(_isFillingList) {
				return;
			}
			securityTreeUserGroup.FillForUserGroup(((ODBoxItem<UserGroup>)listUserGroupTabUserGroups.SelectedItem).Tag.UserGroupNum);
			FillAssociatedUsers();
		}

		private void butAddGroup_Click(object sender,EventArgs e) {
			//Call an event that bubbles back up to the calling Form.
			AddUserGroupClick?.Invoke(this,new SecurityEventArgs(new UserGroup()));
		}

		private void butEditGroup_Click(object sender,EventArgs e) {
			if(listUserGroupTabUserGroups.SelectedIndex==-1) {
				MsgBox.Show(this,"Please select a User Group to edit.");
				return;
			}
			//Call an event that bubbles back up to the calling Form.
			EditUserGroupClick?.Invoke(this,new SecurityEventArgs(SelectedUserGroup));
		}

		private void listUserGroupTabUserGroups_DoubleClick(object sender,EventArgs e) {
			//Call an event that bubbles back up to the calling Form.
			EditUserGroupClick?.Invoke(this,new SecurityEventArgs(SelectedUserGroup));
		}

		private void butSetAll_Click(object sender,EventArgs e) {
			securityTreeUserGroup.SetAll();
		}
		#endregion

		///<summary>We need to refresh the selected tab to display updated information.</summary>
		private void tabControlMain_SelectedIndexChanged(object sender,EventArgs e) {
			if(tabControlMain.SelectedTab == tabPageUsers) {
				FillGridUsers();
				FillUserTabGroups(); //a usergroup could have been added, so refresh.
			}
			else if(tabControlMain.SelectedTab == tabPageUserGroups) {
				FillAssociatedUsers(); //the only thing that could have changed are the users associated to the user groups.
			}
		}

		private DialogResult securityTreeUserGroup_ReportPermissionChecked(object sender,SecurityEventArgs e) {
			return ReportPermissionChecked?.Invoke(sender,e)??DialogResult.Cancel;
		}

		private enum UserFilters{
			[Description("All Users")]
			AllUsers=0,
			Providers,
			Employees,
			Students,
			Instructors,
			Other,
		}

		private DialogResult securityTreeUserGroup_GroupPermissionChecked(object sender,SecurityEventArgs e) {
			return GroupPermissionChecked?.Invoke(sender,e)??DialogResult.Cancel;
		}
	}

	///<summary>A rather generic EventArgs class that can contain specific Security Object types (Userod, UserGroup, or GroupPermission).</summary>
	public class SecurityEventArgs {
		public Userod User {
			get;
		}
		public UserGroup Group {
			get;
		}
		public GroupPermission Perm {
			get;
		}

		public SecurityEventArgs(Userod user) {
			User=user;
		}

		public SecurityEventArgs(UserGroup userGroup) {
			Group=userGroup;
		}

		public SecurityEventArgs(GroupPermission perm) {
			Perm=perm;
		}

	}
}
