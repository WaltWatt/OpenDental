using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;

namespace OpenDentBusiness {
	///<summary></summary>
	[Serializable()]
	public class DatabaseMaintenance:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long DatabaseMaintenanceNum;
		///<summary>The name of the databasemaintenance name.</summary>
		public string MethodName;
		///<summary>Set to true to indicate that the method is hidden.</summary>
		public bool IsHidden;
		///<summary>Set to true to indicate that the method is old.</summary>
		public bool IsOld;
		///<summary>Updates the date and time they run the method.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.DateT)]
		public DateTime DateLastRun;

		///<summary></summary>
		public DatabaseMaintenance Copy() {
			return (DatabaseMaintenance)this.MemberwiseClone();
		}
	}

	///<summary></summary>
	public enum DbmMode {
		///<summary></summary>
		Check = 0,
		///<summary></summary>
		Breakdown = 1,
		///<summary></summary>
		Fix = 2
	}

	///<summary>An attribute that should get applied to any method that needs to show up in the main grid of FormDatabaseMaintenance.
	///Also, an attribute that identifies methods that require a userNum parameter for sending the current user through the middle tier to set the
	///SecUserNumEntry field.</summary>
	[System.AttributeUsage(System.AttributeTargets.Method,AllowMultiple = false)]
	public class DbmMethodAttr:System.Attribute {
		private bool _hasBreakDown;
		private bool _hasUserNum;
		private bool _hasPatNum;

		///<summary>Set to true if this dbm method needs to be able to show the user a list or break down of items that need manual attention.</summary>
		public bool HasBreakDown {
			get {
				return _hasBreakDown;
			}
			set {
				_hasBreakDown=value;
			}
		}

		///<summary>Set to true if this dbm method requires a userNum parameter.  The parameter can be 0, it will be set if not ServerWeb remoting role.</summary>
		public bool HasUserNum {
			get { return _hasUserNum; }
			set { _hasUserNum=value; }
		}

		///<summary>Set to true if this dbm method needs to be able to run for a specific patient.</summary>
		public bool HasPatNum {
			get {
				return _hasPatNum;
			}
			set {
				_hasPatNum=value;
			}
		}

		public DbmMethodAttr() {
			this._hasBreakDown=false;
			this._hasUserNum=false;
			this._hasPatNum=false;
		}

	}

	///<summary>Sorting class used to sort a MethodInfo list by Name.</summary>
	public class MethodInfoComparer:IComparer<MethodInfo> {

		public MethodInfoComparer() {
		}

		public int Compare(MethodInfo x,MethodInfo y) {
			return x.Name.CompareTo(y.Name);
		}
	}
}
