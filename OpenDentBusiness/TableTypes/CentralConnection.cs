using System;
using System.Collections;
using System.Xml.Serialization;

namespace OpenDentBusiness{

	///<summary>Used by the Central Manager.  Stores the information needed to establish a connection to a remote database.</summary>
	[Serializable()]
	[CrudTable(IsSynchable=true)]
	public class CentralConnection:TableBase {
		///<summary>Primary key.</summary>
		[CrudColumn(IsPriKey=true)]
		public long CentralConnectionNum;
		///<summary>If direct db connection.  Can be ip address.</summary>
		public string ServerName;
		///<summary>If direct db connection.</summary>
		public string DatabaseName;
		///<summary>If direct db connection.</summary>
		public string MySqlUser;
		///<summary>If direct db connection.  Symmetrically encrypted.</summary>
		public string MySqlPassword;
		///<summary>If connecting to the web service. Can be on VPN, or can be over https.</summary>
		public string ServiceURI;
		///<summary>Deprecated.  If connecting to the web service.</summary>
		public string OdUser;
		///<summary>Deprecated.  If connecting to the web service.  Symmetrically encrypted.</summary>
		public string OdPassword;
		///<summary>When being used by ConnectionStore xml file, must deserialize to a ConnectionNames enum value. Otherwise just used as a generic notes field.</summary>
		[CrudColumn(SpecialType=CrudSpecialColType.TextIsClob)]
		public string Note;
		///<summary>0-based.</summary>
		public int ItemOrder;
		///<summary>If set to true, the password hash is calculated differently.</summary>
		public bool WebServiceIsEcw;
		///<summary>Contains the most recent information about this connection.  OK if no problems, version information if version mismatch, 
		///nothing for not checked, and OFFLINE if previously couldn't connect.</summary>
		public string ConnectionStatus;
		///<summary>Set when reading from the config file. Not an actual DB column.</summary>
		[CrudColumn(IsNotDbColumn=true)]
		public bool IsAutomaticLogin;

		///<summary>Returns a copy.</summary>
		public CentralConnection Copy() {
			return (CentralConnection)this.MemberwiseClone();
		}

	}
	


}













