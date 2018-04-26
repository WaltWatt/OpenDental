using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace OpenDentBusiness{
		///<summary>(User OD since "user" is a reserved word) Users are a completely separate entity from Providers and Employees even though they can be linked.  A usernumber can never be changed, ensuring a permanent way to record database entries and leave an audit trail.  A user can be a provider, employee, or neither.</summary>
	[Serializable()]
	public class Userod:TableBase{
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long UserNum;
		///<summary>.</summary>
		public string UserName;
		///<summary>The password hash, not the actual password.  If no password has been entered, then this will be blank.</summary>
		public string Password;
		///<summary>Deprecated. Use UserGroupAttaches to link Userods to UserGroups.</summary>
		public long UserGroupNum;
		///<summary>FK to employee.EmployeeNum. Cannot be used if provnum is used. Used for timecards to block access by other users.</summary>
		public long EmployeeNum;
		///<summary>FK to clinic.ClinicNum.  Default clinic for this user.  It causes new patients to default to this clinic when entered by this user.  
		///If 0, then user has no default clinic or default clinic is HQ if clinics are enabled.</summary> 		
		public long ClinicNum;
		///<summary>FK to provider.ProvNum.  Cannot be used if EmployeeNum is used.  It is possible to have multiple userods attached to a single provider.</summary>
		public long ProvNum;
		///<summary>Set true to hide user from login list.</summary>
		public bool IsHidden;
		///<summary>FK to tasklist.TaskListNum.  0 if no inbox setup yet.  It is assumed that the TaskList is in the main trunk, but this is not strictly enforced.  User can't delete an attached TaskList, but they could move it.</summary>
		public long TaskListInBox;
		/// <summary> Defaults to 3 (regular user) unless specified. Helps populates the Anesthetist, Surgeon, Assistant and Circulator dropdowns properly on FormAnestheticRecord/// </summary>
		public int AnesthProvType;
		///<summary>If set to true, the BlockSubsc button will start out pressed for this user.</summary>
		public bool DefaultHidePopups;
		///<summary>Gets set to true if strong passwords are turned on, and this user changes their password to a strong password.  We don't store actual passwords, so this flag is the only way to tell.</summary>
		public bool PasswordIsStrong;
		///<summary>Only used when userod.ClinicNum is set to not be zero.  Prevents user from having access to other clinics.</summary>
		public bool ClinicIsRestricted;
		///<summary>If set to true, the BlockInbox button will start out pressed for this user.</summary>
		public bool InboxHidePopups;
		///<summary>FK to userod.UserNum.  The user num within the Central Manager database.  Only editable via CEMT.  Can change when CEMT syncs.</summary>
		public long UserNumCEMT;
		///<summary>The date and time of the most recent log in failure for this user.  Set to MinValue after user logs in successfully.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateTFail;
		///<summary>The number of times this user has failed to log into their account.  Set to 0 after user logs in successfully.</summary>
		public byte FailedAttempts;
		/// <summary>The username for the ActiveDirectory user to link the account to.</summary>
		public string DomainUser;
		///<summary>Boolean.  If true, the user's password needs to be reset on next login.</summary>
		public bool IsPasswordResetRequired;

		///<summary>Enum representing what eService this user is a "phantom" user for.
		///This variable is to be ignored for serialization and was made private to emphasize the fact that it should not be a db column.
		///Mainly used to not have Security.IsAuthorized() checks throw exceptions due to a null Userod when the eServices are calling S class methods.
		///Currently only used to grant eServices access to certain methods that would otherwise reject it due to permissions.</summary>
		private EServiceTypes _eServiceType;
		///<summary>All valid users should NOT set this value to anything other than None otherwise permission checking will act unexpectedly.
		///Programmatically set this value from the init method of the corresponding eService.  Helps prevent unhandled exceptions.
		///Custom property only meant to be used via eServices.  Not a column in db.  Not to be used in middle tier environment.</summary>
		[XmlIgnore]
		public EServiceTypes EServiceType {
			get {
				return _eServiceType;
			}
			set {
				_eServiceType=value;
			}
		}

		public Userod(){

		}
		
		///<summary></summary>
		public Userod Copy(){
			return (Userod)this.MemberwiseClone();
		}

		public override string ToString(){
			return UserName;
		}

		public bool IsInUserGroup(long userGroupNum) {
			return Userods.IsInUserGroup(this.UserNum,userGroupNum);
		}

		///<summary>Gets all of the usergroups attached to this user.</summary>
		public List<UserGroup> GetGroups(bool includeCEMT = false) {
			return UserGroups.GetForUser(UserNum, includeCEMT);
		}

	}

	///<summary></summary>
	public enum EServiceTypes {
		///<summmary>Not an eService user.  All valid users should be this type otherwise permission checking will act differently.</summmary>
		None,
		///<summary></summary>
		EConnector,
		///<summary></summary>
		Broadcaster,
		///<summary></summary>
		BroadcastMonitor,
		///<summary></summary>
		ServiceMainHQ,
	}

	//public class DtoUserodRefresh:DtoQueryBase {
	//}

}
