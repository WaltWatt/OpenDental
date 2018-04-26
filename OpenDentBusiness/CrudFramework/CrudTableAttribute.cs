using System;
using System.Collections.Generic;
using System.Text;

namespace OpenDentBusiness {
	///<summary>Crud table attributes cannot be used by inherited classes because some properties would not work if they were inherited.
	///Simply add the desired attributes to the "inheriting" class which will effectively override the attribute.</summary>
	[AttributeUsage(AttributeTargets.Class,AllowMultiple=false,Inherited=false)]
	public class CrudTableAttribute : Attribute {
		public CrudTableAttribute() {
			this.tableName="";
			this.isDeleteForbidden=false;
			this.isMissingInGeneral=false;
			this.isMobile=false;
			this.isSynchable=false;
			this._auditPerms=CrudAuditPerm.None;
			this._isSecurityStamped=false;
			this._hasBatchWriteMethods=false;
		}

		private string tableName;
		///<summary>If tablename is different than the lowercase class name.</summary>
		public string TableName {
			get { return tableName; }
			set { tableName=value; }
		}

		private bool isDeleteForbidden;
		///<summary>Set to true for tables where rows are not deleted.</summary>
		public bool IsDeleteForbidden {
			get { return isDeleteForbidden; }
			set { isDeleteForbidden=value; }
		}

		private bool isMissingInGeneral;
		///<summary>Set to true for tables that are part of internal tools and not found in the general release.  The Crud generator will gracefully skip these tables if missing from the database that it's running against.  It also won't try to generate a dataInterface s class.</summary>
		public bool IsMissingInGeneral {
			get { return isMissingInGeneral; }
			set { isMissingInGeneral=value; }
		}

		private bool isMobile;
		///<summary>Set to true for tables that are used on server for mobile services.  These are 'lite' versions of the main tables, and end with m.  A composite primary key will be expected.  The Crud generator will generate these crud files in a different place than the other crud files.  It will also generate the dataInterface 'ms' class to a different location.  It also won't validate that the table exists in the test database.</summary>
		public bool IsMobile {
			get { return isMobile; }
			set { isMobile=value; }
		}

		private bool isSynchable;
		public bool IsSynchable {
			get { return isSynchable; }
			set { isSynchable=value; }
		}

		private CrudAuditPerm _auditPerms;
		///<summary>Enum containing all permissions used as an FKey entry for the Securitylog table.
		///The Crud generator uses these to add an additional function call to Delete(), and a new function ClearFkey() to ensure that securitylog FKeys 
		///  are not orphaned.</summary>
		public CrudAuditPerm AuditPerms {
			get { return _auditPerms; }
			set { _auditPerms=value; }
		}

		private bool _isSecurityStamped;
		///<summary>If IsSecurityStamped is true, the table must include the field SecUserNumEntry.
		///<para>If IsSynchable and IsSecurityStamped are BOTH true, the Crud generator will create a Sync function that takes userNum and sets the
		///SecUserNumEntry field before inserting.  Security.CurUser isn't accessible from the Crud due to remoting role, must be passed in.</para>
		///<para>IsSecurityStamped is ignored if IsSynchable is false.</para></summary>
		public bool IsSecurityStamped {
			get { return _isSecurityStamped; }
			set { _isSecurityStamped=value; }
		}

		private bool _hasBatchWriteMethods;
		public bool HasBatchWriteMethods {
			get { return _hasBatchWriteMethods; }
			set { _hasBatchWriteMethods=value; }
		}

	}

	///<summary>Hard coded list of all permission names that are used for securitylog.FKey.  Uses 2^n values for use in bitwise operations.
	///This enum can only hold 31 permissions (and none) before we will need to create a new one.  Instead of creating a new enum, we could instead
	///create a new table to hold a composite key between the permission type the table name and foreign key.</summary>
	[Flags]
	public enum CrudAuditPerm {
		///<summary>Perm#:0 - Value:0</summary>
		None=0,
		///<summary>Perm#:1 - Value:1</summary>
		AppointmentCompleteEdit=1,
		///<summary>Perm#:2 - Value:2</summary>
		AppointmentCreate=2,
		///<summary>Perm#:3 - Value:4</summary>
		AppointmentEdit=4,
		///<summary>Perm#:4 - Value:8</summary>
		AppointmentMove=8,
		///<summary>Perm#:5 - Value:16</summary>
		ClaimHistoryEdit=16,
		///<summary>Perm#:6 - Value:32</summary>
		ImageDelete=32,
		///<summary>Perm#:7 - Value:64</summary>
		ImageEdit=64,
		///<summary>Perm#:8 - Value:128</summary>
		InsPlanChangeCarrierName=128,
		///<summary>Perm#:9 - Value:256</summary>
		RxCreate=256,
		///<summary>Perm#:10 - Value:512</summary>
		RxEdit=512,
		///<summary>Perm#:11 - Value:1024</summary>
		TaskNoteEdit=1024,
		///<summary>Perm#:12 - Value:2048</summary>
		PatientPortal=2048,
		///<summary>Perm#:13 - Value:4096</summary>
		ProcFeeEdit=4096,
		///<summary>Perm#:14 - Value:8192</summary>
		LogFeeEdit=8192,
		///<summary>Perm#15 - Value:16384</summary>
		LogSubscriberEdit=16384
	}

}
