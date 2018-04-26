using CodeBase;
using System;

namespace OpenDentBusiness {
	///<summary>Specific ODEvent for when communication to the database is unavailable.</summary>
	public class DataConnectionEvent {
		///<summary>This event will get fired whenever communication to the database is attempted and fails.</summary>
		public static event DataConnectionEventHandler Fired;

		///<summary>Call this method only when communication to the database is not possible.</summary>
		public static void Fire(DataConnectionEventArgs e) {
			if(Fired!=null) {
				Fired(e);
			}
		}
	}

	public class DataConnectionEventArgs:ODEventArgs {
		///<summary>This will be set to true once the connection to the database has been restored.</summary>
		public bool IsConnectionRestored;
		///<summary>The connection string of the database that this event is for.</summary>
		public string ConnectionString;

		public DataConnectionEventArgs(string name,string description,bool isConnectionRestored,string connectionString) : base(name,description) {
			IsConnectionRestored=isConnectionRestored;
			ConnectionString=connectionString;
		}
	}

	///<summary></summary>
	public delegate void DataConnectionEventHandler(DataConnectionEventArgs e);
}
