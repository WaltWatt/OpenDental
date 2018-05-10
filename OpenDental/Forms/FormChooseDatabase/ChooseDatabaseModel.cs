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
				CentralConnections.GetChooseDatabaseConnectionSettings(out chooseDatabaseModel.CentralConnectionCur,out chooseDatabaseModel.ConnectionString
					,out chooseDatabaseModel.NoShow,out chooseDatabaseModel.DbType,out chooseDatabaseModel.ListAdminCompNames);
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
