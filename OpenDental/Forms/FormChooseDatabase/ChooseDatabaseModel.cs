using CodeBase;
using CodeBase.MVC;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace OpenDental {
	///<summary></summary>
	public class ChooseDatabaseModel:ODModelAbs<ChooseDatabaseModel> {
		///<summmary></summmary>
		public CentralConnection CentralConnectionCur=new CentralConnection();
		///<summary></summary>
		public string ConnectionString="";
		///<summary>This is used when selecting File>Choose Database.  It will behave slightly differently.</summary>
		public bool IsAccessedFromMainMenu;
		///<summary>When silently running GetConfig() without showing UI, this gets set to true if either NoShowOnStartup or UsingEcw is found in config file.</summary>
		public YN NoShow;
		///<summary></summary>
		public DatabaseType DbType;
		///<summary>Stored so that they don't get deleted when re-writing the FreeDentalConfig file.</summary>
		public List<string> ListAdminCompNames=new List<string>();

		public ChooseDatabaseModel() {
		}

		public override ChooseDatabaseModel Copy() {
			ChooseDatabaseModel retVal=new ChooseDatabaseModel();
			retVal.CentralConnectionCur=CentralConnectionCur.Copy();
			retVal.ConnectionString=this.ConnectionString;
			retVal.IsAccessedFromMainMenu=this.IsAccessedFromMainMenu;
			retVal.NoShow=this.NoShow;
			retVal.DbType=this.DbType;
			retVal.ListAdminCompNames=this.ListAdminCompNames.Select(x => x).ToList();
			return retVal;
		}

		///<summary>Every optional parameter provided should coincide with a commandline arguement.
		///The values passed in will typically override any settings loaded in from the config file.
		///Passing in a value for webServiceUri or databaseName will cause the config file to not even be considered.</summary>
		public static ChooseDatabaseModel GetChooseDatabaseModelFromConfig(string webServiceUri="",YN webServiceIsEcw=YN.Unknown,string odUser=""
			,string serverName="",string databaseName="",string mySqlUser="",string mySqlPassword="",string mySqlPassHash="",YN noShow=YN.Unknown) 
		{
			ChooseDatabaseModel chooseDatabaseModel=new ChooseDatabaseModel();
			//Even if we are passed a URI as a command line argument we still need to check the FreeDentalConfig file for middle tier automatic log in.
			//The only time we do not need to do that is if a direct DB has been passed in.
			if(string.IsNullOrEmpty(databaseName)) {
				#region Config File
				//command-line support for the upper portion of this window will be added later.
				//Improvement should be made here to avoid requiring admin priv.
				//Search path should be something like this:
				//1. /home/username/.opendental/config.xml (or corresponding user path in Windows)
				//2. /etc/opendental/config.xml (or corresponding machine path in Windows) (should be default for new installs) 
				//3. Application Directory/FreeDentalConfig.xml (read-only if user is not admin)
				string xmlPath=ODFileUtils.CombinePaths(Application.StartupPath,"FreeDentalConfig.xml");
				if(!File.Exists(xmlPath)) {
					FileStream fs;
					try {
						fs=File.Create(xmlPath);
					}
					catch(Exception) {
						//No translation right here because we typically do not have a database connection yet.
						throw new ODException("The very first time that the program is run, it must be run as an Admin.  "
							+"If using Vista, right click, run as Admin.");
					}
					fs.Close();
				}
				XmlDocument document=new XmlDocument();
				try {
					document.Load(xmlPath);
					XPathNavigator Navigator=document.CreateNavigator();
					XPathNavigator nav;
					//Database type
					nav=Navigator.SelectSingleNode("//DatabaseType");
					DataConnection.DBtype=DatabaseType.MySql;
					if(nav!=null && nav.Value=="Oracle") {
						DataConnection.DBtype=DatabaseType.Oracle;
					}
					nav=Navigator.SelectSingleNode("//AdminCompNames");
					if(nav != null) {
						chooseDatabaseModel.ListAdminCompNames.Clear(); //this method gets called more than once
						XPathNodeIterator navIterator=nav.SelectChildren(XPathNodeType.All);
						for(int i = 0;i<navIterator.Count;i++) {
							navIterator.MoveNext();
							chooseDatabaseModel.ListAdminCompNames.Add(navIterator.Current.Value);//Add this computer name to the list.
						}
					}
					//See if there's a ConnectionString
					nav=Navigator.SelectSingleNode("//ConnectionString");
					if(nav!=null) {
						//If there is a ConnectionString, then use it.
						chooseDatabaseModel.ConnectionString=nav.Value;
						return chooseDatabaseModel;
					}
					//See if there's a UseXWebTestGateway
					nav=Navigator.SelectSingleNode("//UseXWebTestGateway");
					if(nav!=null) {
						OpenDentBusiness.WebTypes.Shared.XWeb.XWebs.UseXWebTestGateway=nav.Value.ToLower()=="true";
					}
					//See if there's a DatabaseConnection
					nav=Navigator.SelectSingleNode("//DatabaseConnection");
					if(nav!=null) {
						//If there is a DatabaseConnection, then use it.
						chooseDatabaseModel.CentralConnectionCur.ServerName=nav.SelectSingleNode("ComputerName").Value;
						chooseDatabaseModel.CentralConnectionCur.DatabaseName=nav.SelectSingleNode("Database").Value;
						chooseDatabaseModel.CentralConnectionCur.MySqlUser=nav.SelectSingleNode("User").Value;
						chooseDatabaseModel.CentralConnectionCur.MySqlPassword=nav.SelectSingleNode("Password").Value;
						XPathNavigator encryptedPwdNode=nav.SelectSingleNode("MySQLPassHash");
						//If the Password node is empty, but there is a value in the MySQLPassHash node, decrypt the node value and use that instead
						string _decryptedPwd;
						if(chooseDatabaseModel.CentralConnectionCur.MySqlPassword==""
							&& encryptedPwdNode!=null
							&& encryptedPwdNode.Value!=""
							&& CDT.Class1.Decrypt(encryptedPwdNode.Value,out _decryptedPwd))
						{
							//decrypted value could be an empty string, which means they don't have a password set, so textPassword will be an empty string
							chooseDatabaseModel.CentralConnectionCur.MySqlPassword=_decryptedPwd;
						}
						XPathNavigator noshownav=nav.SelectSingleNode("NoShowOnStartup");
						if(noshownav!=null && noshownav.Value=="True"){
							chooseDatabaseModel.NoShow=YN.Yes;
						}
						return chooseDatabaseModel;
					}
					nav=Navigator.SelectSingleNode("//ServerConnection");
					/* example:
					<ServerConnection>
						<URI>http://server/OpenDentalServer/ServiceMain.asmx</URI>
						<UsingEcw>True</UsingEcw>
					</ServerConnection>
					*/
					if(nav!=null) {
						//If there is a ServerConnection, then use it.
						chooseDatabaseModel.CentralConnectionCur.ServiceURI=nav.SelectSingleNode("URI").Value;
						XPathNavigator ecwnav=nav.SelectSingleNode("UsingEcw");
						if(ecwnav!=null && ecwnav.Value=="True") {
							chooseDatabaseModel.NoShow=YN.Yes;
							chooseDatabaseModel.CentralConnectionCur.WebServiceIsEcw=true;
						}
						XPathNavigator autoLoginNav=nav.SelectSingleNode("UsingAutoLogin");
						//Auto login the user using their windows credentials
						if(autoLoginNav!=null && autoLoginNav.Value=="True") {
							chooseDatabaseModel.CentralConnectionCur.OdUser=nav.SelectSingleNode("User").Value;
							//Get the user's password from Windows Credential Manager
							try {
								chooseDatabaseModel.CentralConnectionCur.OdPassword=
									PasswordVaultWrapper.RetrievePassword(chooseDatabaseModel.CentralConnectionCur.ServiceURI,chooseDatabaseModel.CentralConnectionCur.OdUser);
								//Must set this so FormChooseDatabase() does not launch
								chooseDatabaseModel.NoShow=YN.Yes;
								chooseDatabaseModel.CentralConnectionCur.IsAutomaticLogin=true;
							}
							catch(Exception ex) {
								ex.DoNothing();//We still want to display the server URI and the user name if getting the password fails.
							}
						}
					}
				}
				catch(Exception) {
					//Common error: root element is missing
					chooseDatabaseModel.CentralConnectionCur.ServerName="localhost";
	#if(TRIALONLY)
					chooseDatabaseModel.CentralConnectionCur.ServerName="demo";
	#else
					chooseDatabaseModel.CentralConnectionCur.DatabaseName="opendental";
	#endif
					chooseDatabaseModel.CentralConnectionCur.MySqlUser="root";
				}
				#endregion
			}
			//Command line args should always trump settings from the config file.
			#region Command Line Arguements
			if(webServiceUri!="") {//if a URI was passed in
				chooseDatabaseModel.CentralConnectionCur.ServiceURI=webServiceUri;
			}
			if(webServiceIsEcw!=YN.Unknown) {
				chooseDatabaseModel.CentralConnectionCur.WebServiceIsEcw=(webServiceIsEcw==YN.Yes ? true : false);
			}
			if(odUser!="") {
				chooseDatabaseModel.CentralConnectionCur.OdUser=odUser;
			}
			//OdPassHash;//not allowed to be used here.  Instead, only used directly in TryToConnect
			//OdPassword//not allowed to be used here.  Instead, only used directly in TryToConnect
			if(serverName!="") {
				chooseDatabaseModel.CentralConnectionCur.ServerName=serverName;
			}
			if(databaseName!="") {
				chooseDatabaseModel.CentralConnectionCur.DatabaseName=databaseName;
			}
			if(mySqlUser!="") {
				chooseDatabaseModel.CentralConnectionCur.MySqlUser=mySqlUser;
			}
			if(mySqlPassword!="") {
				chooseDatabaseModel.CentralConnectionCur.MySqlPassword=mySqlPassword;
			}
			if(mySqlPassHash!="") {
				string decryptedPwd;
				CDT.Class1.Decrypt(mySqlPassHash,out decryptedPwd);
				chooseDatabaseModel.CentralConnectionCur.MySqlPassword=decryptedPwd;
			}
			if(noShow!=YN.Unknown) {
				chooseDatabaseModel.NoShow=noShow;
			}
			#endregion
			return chooseDatabaseModel;
		}
	}
}
