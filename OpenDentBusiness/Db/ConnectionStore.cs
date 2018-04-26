using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Hosting;
using System.Xml.Serialization;
using CodeBase;
using System.Xml;

namespace OpenDentBusiness {
	///<summary>Thread-safe access to a list of connection store object which is retreived from a given file. If Init is not called then looks for ConnectionStore.xml in working directory.</summary>
	public class ConnectionStore {
		private static object _lock=new object();
		///<summary>Only used by _dictCentConnSafe. Do not use elsewhere in this class.</summary>
		private static Dictionary<ConnectionNames,OpenDentBusiness.CentralConnection> _dictCentConnUnsafe;
		
		///<summary>Uses for thread-safe internal acces.</summary>
		private static Dictionary<ConnectionNames,OpenDentBusiness.CentralConnection> _dictCentConnSafe {
			get {
				//This action is just a glorified private method. It does a bunch of dirty work that would need to be copy/pasted multiple times otherwise.
				//Any callers of this action should guard against re-entry by only returning non-null one time (or whenever absolutely necessary).
				Action<Func<Dictionary<ConnectionNames,OpenDentBusiness.CentralConnection>>> aTryInit=new Action<Func<Dictionary<ConnectionNames, CentralConnection>>>((f) => {
					//Try to load the file.
					Dictionary<ConnectionNames,OpenDentBusiness.CentralConnection> dictNew=null;
					ODException.SwallowAnyException(new Action(() => { dictNew=f(); }));
					if(dictNew==null) { //Load failed or we already found valid connection previously so don't continue.
						return;
					}
					lock(_lock) { //We got this far so set/merge the global dict.
						if(_dictCentConnUnsafe==null) { //First time, set the global dict.
							_dictCentConnUnsafe=dictNew;
						}
						//Merge the new dict with the global dict.
						foreach(KeyValuePair<ConnectionNames,OpenDentBusiness.CentralConnection> kvp in dictNew) {
							if(!_dictCentConnUnsafe.ContainsKey(kvp.Key)) { //First in wins, so only add if we don't already have this entry.
								_dictCentConnUnsafe[kvp.Key]=kvp.Value;								
							}
						}
					}
				});
				//Try to init each connection file. The first one that loads successfully will be given priority. All other attempts will fail silently and the first file's contents will persist.				
				aTryInit(new Func<Dictionary<ConnectionNames,CentralConnection>>(() => { //Windows forms and service applications.
					return InitConnectionStoreXml(CodeBase.ODFileUtils.CombinePaths(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location),"ConnectionStore.xml"));
				}));
				aTryInit(new Func<Dictionary<ConnectionNames,CentralConnection>>(() => { //Web applications.
					return InitConnectionStoreXml(CodeBase.ODFileUtils.CombinePaths(HostingEnvironment.ApplicationPhysicalPath,"ConnectionStore.xml"));
				}));
				aTryInit(new Func<Dictionary<ConnectionNames,CentralConnection>>(() => { //Windows forms and service applications.
					return InitOpenDentalWebConfigXml(CodeBase.ODFileUtils.CombinePaths(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location),"OpenDentalWebConfig.xml"));
				}));
				aTryInit(new Func<Dictionary<ConnectionNames,CentralConnection>>(() => { //Web applications.
					return InitOpenDentalWebConfigXml(CodeBase.ODFileUtils.CombinePaths(HostingEnvironment.ApplicationPhysicalPath,"OpenDentalWebConfig.xml"));
				}));
				aTryInit(new Func<Dictionary<ConnectionNames,CentralConnection>>(() => { //Dental Office Report Server (via prefs).
					if(HasSingleEntry(ConnectionNames.DentalOfficeReportServer)) { //Only try to add this value if it doesn't already exist.
						return null;
					}
					//Be aware that if PrefC cache is not already filled and/or DataConnection.SetDb() has not already been called, this will fail.
					CentralConnection cn=null;
					ODException.SwallowAnyException(() => {
						cn=new CentralConnection() { //give regular server credentials if the report server is not set up.
							ServerName=PrefC.ReportingServer.Server=="" ? DataConnection.GetServerName() : PrefC.ReportingServer.Server,
							DatabaseName=PrefC.ReportingServer.Server=="" ? DataConnection.GetDatabaseName() : PrefC.ReportingServer.Database,
							MySqlUser=PrefC.ReportingServer.Server=="" ? DataConnection.GetMysqlUser() : PrefC.ReportingServer.MySqlUser,
							MySqlPassword=PrefC.ReportingServer.Server=="" ? DataConnection.GetMysqlPass() : PrefC.ReportingServer.MySqlPass,
							//no ternary operator because URI will be blank if they're not using a middle tier reporting server.
							ServiceURI=PrefC.ReportingServer.URI,
						};
					});					
					//Not already there so add it once.
					return new Dictionary<ConnectionNames,CentralConnection>() { { ConnectionNames.DentalOfficeReportServer,cn??new CentralConnection() } };
				}));
				aTryInit(new Func<Dictionary<ConnectionNames,CentralConnection>>(() => { //CustomersHQ (via prefs).
					if(HasSingleEntry(ConnectionNames.CustomersHQ)) { //Only try to add this value if it doesn't already exist.
						return null;
					}
					//If PrefC cache is not already filled and/or DataConnection.SetDb() has not already been called, this will fail.
					CentralConnection cn=null;
					ODException.SwallowAnyException(() => {
						if(!PrefC.IsODHQ) {
							return;
						}
						cn=new CentralConnection {
#if DEBUG
							ServerName="localhost",
#else
							ServerName=PrefC.GetString(PrefName.CustomersHQServer),
#endif
							DatabaseName=PrefC.GetString(PrefName.CustomersHQDatabase),
							MySqlUser=PrefC.GetString(PrefName.CustomersHQMySqlUser),
						};
						CDT.Class1.Decrypt(PrefC.GetString(PrefName.CustomersHQMySqlPassHash),out cn.MySqlPassword);
					});
					//Not already there so add it once.
					return new Dictionary<ConnectionNames,CentralConnection>() { { ConnectionNames.CustomersHQ,cn??new CentralConnection() } };
				}));
				Dictionary<ConnectionNames,OpenDentBusiness.CentralConnection> dictRet=null;
				lock (_lock) {
					dictRet=_dictCentConnUnsafe;
				}
				return dictRet;
			}
		}

		///<summary>Private ctor prevents this class from being instansiated. We will just use the class for static init.</summary>
		private ConnectionStore() {
		}

		///<summary>Sets the current dictionary of connections to null so that it reinitializes all connections the next time it is accessed.
		///This is mainly used for connections that utilize preferences so that the dictionary can be up to date.</summary>
		public static void ClearConnectionDictionary() {
			lock(_lock) {
				_dictCentConnUnsafe=null;
			}
		}

		///<summary>Returns true if any ConnectionName entries have been loaded; otherwise returns false.</summary>
		public static bool HasAnyEntries {
			get {
				int entryCount=0;
				lock (_lock) {
					entryCount=_dictCentConnUnsafe==null?0:_dictCentConnUnsafe.Count;
				}
				return entryCount>0;
			}
		}

		///<summary>Returns true if the connectionName entry has been loaded; otherwise returns false.</summary>
		public static bool HasSingleEntry(ConnectionNames connectionName) {
			bool ret=false;
			lock (_lock) {
				ret=_dictCentConnUnsafe!=null&&_dictCentConnUnsafe.ContainsKey(connectionName);
			}
			return ret;
		}

		///<summary>Initializes central connection store from a given ConnectionStore formatted file. Throws exceptions if file not found or init fails for any other reason.</summary>
		private static Dictionary<ConnectionNames,OpenDentBusiness.CentralConnection> InitConnectionStoreXml(string fullPath) {			
			return  InitConnectionsFromXmlFile<ListCentralConnections,CentralConnection>(fullPath,new Func<CentralConnection, CentralConnection>((conn) => {
				return conn;
			}));			
		}

		///<summary>Initializes central connection store from a given OpenDentalWebConfig file. Throws exceptions if file not found or init fails for any other reason.</summary>
		private static Dictionary<ConnectionNames,OpenDentBusiness.CentralConnection> InitOpenDentalWebConfigXml(string fullPath) {
			return InitConnectionsFromXmlFile<ConnectionSettings,DatabaseConnection>(fullPath,new Func<DatabaseConnection, CentralConnection>((conn) => {
				return new CentralConnection() {
					CentralConnectionNum=0,
					ConnectionStatus="",
					DatabaseName=conn.Database,
					ItemOrder=0,
					MySqlPassword=conn.Password,
					MySqlUser=conn.User,
					Note=conn.Note,
					OdPassword="",
					OdUser="",
					ServerName=conn.ComputerName,
					ServiceURI="",
					WebServiceIsEcw=false
				};
			}));			
		}

		///<summary>Initializes central connection store from a given xml file. Throws exceptions if file not found or init fails for any other reason.</summary>
		/// <typeparam name="FILETYPE">Must extend IConnectionFile. Instance will be created by deserializing the given xml file.</typeparam>
		/// <typeparam name="ITEMTYPE">The type of item defined by the given IConnectionFile type.</typeparam>
		/// <param name="fullPath">Full file path to the xml file.</param>
		/// <param name="fGetConnectionFromItem">Function that will take in an instance of ITEMTYPE and return an instance of CentralConnection.</param>
		private static Dictionary<ConnectionNames,OpenDentBusiness.CentralConnection> InitConnectionsFromXmlFile<FILETYPE,ITEMTYPE>(string fullPath,Func<ITEMTYPE,CentralConnection> fGetConnectionFromItem) where FILETYPE : IConnectionFile<ITEMTYPE> {
			if(HasAnyEntries) { //Prevent re-entry. Only want to run this once.
				return null;
			}
			if(!File.Exists(fullPath)) {
				throw new Exception("ConnectionStore file not found: "+fullPath);
			}
			//Deserialize the file.
			FILETYPE fromFile;
			using(XmlReader reader=XmlReader.Create(fullPath)) {
				fromFile=(FILETYPE)new XmlSerializer(typeof(FILETYPE)).Deserialize(reader);				
			}
			Dictionary<ConnectionNames,CentralConnection> ret=new Dictionary<ConnectionNames,CentralConnection>();
			//Loop through each item in the file and convert it to a CentralConnection.
			foreach(ITEMTYPE item in fromFile.Items) {
				CentralConnection centConnCur=fGetConnectionFromItem(item);
				ConnectionNames connName;
				//Note field must deserialize to a ConnectionNames enum value.
				if(Enum.TryParse<ConnectionNames>(centConnCur.Note,out connName)) {
					ret[connName]=centConnCur;
				}
			}
			return ret;
		}

		///<summary>Get a central connection by name.</summary>
		public static OpenDentBusiness.CentralConnection GetConnection(ConnectionNames name) {
			Dictionary<ConnectionNames,OpenDentBusiness.CentralConnection> dict=_dictCentConnSafe;
			if(dict==null || !dict.ContainsKey(name)) {
				throw new Exception("Connection name not found: "+name);
			}
			return dict[name];
		}

		///<summary>Sets the connection of the current thread to the ConnectionName indicated. Connection details will be retrieved from ConnectionStore.xml.</summary>
		public static OpenDentBusiness.CentralConnection SetDb(ConnectionNames dbName) {
			CentralConnection conn=GetConnection(dbName);
			if(!string.IsNullOrEmpty(conn.ServiceURI)) {
				RemotingClient.ServerURI=conn.ServiceURI;
			}
			else {
				new DataConnection().SetDb(conn.ServerName,conn.DatabaseName,conn.MySqlUser,conn.MySqlPassword,"","",DatabaseType.MySql);
			}
			return conn;
		}

		///<summary>Sets the connection of the current thread to the ConnectionName indicated. Connection details will be retrieved from ConnectionStore.xml.</summary>
		public static OpenDentBusiness.CentralConnection SetDbT(ConnectionNames dbName) {
			OpenDentBusiness.CentralConnection conn=GetConnection(dbName);
			if(!string.IsNullOrEmpty(conn.ServiceURI)) {
				RemotingClient.SetRemotingT(conn.ServiceURI,RemotingRole.ClientWeb,(dbName==ConnectionNames.DentalOfficeReportServer));
			}
			else {
				new OpenDentBusiness.DataConnection().SetDbT(conn.ServerName,conn.DatabaseName,conn.MySqlUser,conn.MySqlPassword,"","",OpenDentBusiness.DatabaseType.MySql,true);
			}
			return conn;
		}

		///<summary>Instance will be created from a deserialized xml string.</summary>
		/// <typeparam name="ITEMTYPE">Type type of list items included in this file.</typeparam>
		public interface IConnectionFile<ITEMTYPE> {
			List<ITEMTYPE> Items { get; set; }
		}

		[XmlRoot("ListCentralConnections")]
		public class ListCentralConnections:IConnectionFile<CentralConnection> {
			//Example File Format.
			/*
			<ListCentralConnections>
				<CentralConnection>
					<CentralConnectionNum>0</CentralConnectionNum>
					<ServerName>localhost</ServerName>
					<DatabaseName>serviceshq</DatabaseName>
					<MySqlUser>root</MySqlUser>
					<MySqlPassword></MySqlPassword>
					<ServiceURI></ServiceURI>
					<OdUser></OdUser>
					<OdPassword></OdPassword>
					<Note>ServicesHQ</Note>
					<ItemOrder>0</ItemOrder>
					<WebServiceIsEcw>0</WebServiceIsEcw>
				</CentralConnection>
			</ListCentralConnections>
			*/

			///<summary>Interface property. Overriding here allows us to define the XML element name which to look for in the file.</summary>
			[XmlElement("CentralConnection")]
			public List<CentralConnection> Items { get; set; }
			public ListCentralConnections() {
				Items=new List<CentralConnection>();
			}
		}

		[XmlRoot("ConnectionSettings")]
		public class ConnectionSettings:IConnectionFile<DatabaseConnection> {
			//Example File Format.
			/*
			<?xml version="1.0"?>
			<ConnectionSettings>
				<DatabaseConnection>
					<ComputerName>localhost</ComputerName>
					<Database>serviceshq</Database>
					<User>root</User>
					<Password></Password>
					<UserLow>root</UserLow>
					<PasswordLow></PasswordLow>
					<DatabaseType>MySql</DatabaseType>
					<Note>ServicesHQ</Note>
				</DatabaseConnection>
			</ConnectionSettings>
			*/

			///<summary>Interface property. Overriding here allows us to define the XML element name which to look for in the file.</summary>
			[XmlElement("DatabaseConnection")]
			public List<DatabaseConnection> Items { get; set; }
			public ConnectionSettings() {
				Items=new List<DatabaseConnection>();
			}
		}

		public class DatabaseConnection {
			///<summary>If direct db connection.  Can be ip address.</summary>
			public string ComputerName;
			///<summary>If direct db connection.</summary>
			public string Database;
			///<summary>High permissions user name.</summary>
			public string User;
			///<summary>High permissions password.</summary>
			public string Password;
			///<summary>Low permissions user name.</summary>
			public string UserLow;
			///<summary>Low permissions password.</summary>
			public string PasswordLow;
			///<summary>String representation of database type.</summary>
			public string DatabaseType;
			///<summary>Must deserialize to a ConnectionNames enum value.</summary>
			public string Note;

			[XmlIgnore]
			public DatabaseType DbType {
				get {
					DatabaseType dbType;
					if(!Enum.TryParse<DatabaseType>(this.DatabaseType,out dbType)) {
						dbType=OpenDentBusiness.DatabaseType.MySql;
					}
					return dbType;
				}
			}
		}
	}
}
