using CodeBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace OpenDentBusiness{
	///<summary></summary>
	public class CentralConnections {
		#region Get Methods
		#endregion

		#region Modification Methods
		
		#region Insert
		#endregion

		#region Update
		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods

		#region Choose Database
		///<summary>Gets a list of all computer names on the network (this is not easy)</summary>
		public static string[] GetComputerNames(){
			if(Environment.OSVersion.Platform==PlatformID.Unix) {
				return new string[0];
			}
			try {
				File.Delete(ODFileUtils.CombinePaths(Application.StartupPath,"tempCompNames.txt"));
				ArrayList retList=new ArrayList();
				//string myAdd=Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();//obsolete
				string myAdd=Dns.GetHostEntry(Dns.GetHostName()).AddressList[0].ToString();
				ProcessStartInfo psi=new ProcessStartInfo();
				psi.FileName=@"C:\WINDOWS\system32\cmd.exe";//Path for the cmd prompt
				psi.Arguments="/c net view > tempCompNames.txt";//Arguments for the command prompt
				//"/c" tells it to run the following command which is "net view > tempCompNames.txt"
				//"net view" lists all the computers on the network
				//" > tempCompNames.txt" tells dos to put the results in a file called tempCompNames.txt
				psi.WindowStyle=ProcessWindowStyle.Hidden;//Hide the window
				Process.Start(psi);
				StreamReader sr=null;
				string filename=ODFileUtils.CombinePaths(Application.StartupPath,"tempCompNames.txt");
				Thread.Sleep(200);//sleep for 1/5 second
				if(!File.Exists(filename)) {
					return new string[0];
				}
				try {
					sr=new StreamReader(filename);
				}
				catch(Exception) {
				}
				while(!sr.ReadLine().StartsWith("--")){
					//The line just before the data looks like: --------------------------
				}
				string line="";
				retList.Add("localhost");
				while(true) {
					line=sr.ReadLine();
					if(line.StartsWith("The"))//cycle until we reach,"The command completed successfully."
						break;
					line=line.Split(char.Parse(" "))[0];// Split the line after the first space
					// Normally, in the file it lists it like this
					// \\MyComputer                 My Computer's Description
					// Take off the slashes, "\\MyComputer" to "MyComputer"
					retList.Add(line.Substring(2,line.Length-2));
				}
				sr.Close();
				File.Delete(ODFileUtils.CombinePaths(Application.StartupPath,"tempCompNames.txt"));
				string[] retArray=new string[retList.Count];
				retList.CopyTo(retArray);
				return retArray;
			}
			catch(Exception) {//it will always fail if not WinXP
				return new string[0];
			}
		}

		///<summary></summary>
		public static string[] GetDatabases(CentralConnection centralConnection,DatabaseType dbType) {
			if(centralConnection.ServerName=="") {
				return new string[0];
			}
			if(dbType!=DatabaseType.MySql) {
				return new string[0];//because SHOW DATABASES won't work
			}
			try {
				DataConnection dcon;
				//use the one table that we know exists
				if(centralConnection.MySqlUser=="") {
					dcon=new DataConnection(centralConnection.ServerName,"mysql","root",centralConnection.MySqlPassword,dbType);
				}
				else {
					dcon=new DataConnection(centralConnection.ServerName,"mysql",centralConnection.MySqlUser,centralConnection.MySqlPassword
						,dbType);
				}
				string command="SHOW DATABASES";
				//if this next step fails, table will simply have 0 rows
				DataTable table=dcon.GetTable(command,false);
				string[] dbNames=new string[table.Rows.Count];
				for(int i=0;i<table.Rows.Count;i++){
					dbNames[i]=table.Rows[i][0].ToString();
				}
				return dbNames;
			}
			catch(Exception) {
				return new string[0];
			}
		}

		///<summary>Throws an exception to display to the user if anything goes wrong.</summary>
		public static void TryToConnect(CentralConnection centralConnection,DatabaseType dbType,string connectionString="",bool noShowOnStartup=false
			,List<string> listAdminCompNames=null,bool saveOnSuccess=false) 
		{
			if(!string.IsNullOrEmpty(centralConnection.ServiceURI)) {
				LoadMiddleTierProxySettings();
				string originalURI=RemotingClient.ServerURI;
				RemotingClient.ServerURI=centralConnection.ServiceURI;
				bool useEcwAlgorithm=centralConnection.WebServiceIsEcw;
				RemotingRole originalRole=RemotingClient.RemotingRole;
				RemotingClient.RemotingRole=RemotingRole.ClientWeb;
				try {
					string password=centralConnection.OdPassword;
					if(useEcwAlgorithm) {
						//Userods.HashPassword explicitly goes over to middle tier in order to use it's MD5 algorithm.
						//It doesn't matter what Security.CurUser is when it is null because we are technically trying to set it for the first time.
						//It cannot be null before invoking HashPassword because middle needs it to NOT be null when creating the credentials for DtoGetString.
						if(Security.CurUser==null) {
							Security.CurUser=new Userod();
						}
						password=Userods.HashPassword(password,true);
					}
					string username=centralConnection.OdUser;
#if DEBUG
					if(username=="") {
						username="Admin";
						password="pass";
					}
#endif
					//ecw requires hash, but non-ecw requires actual password
					Security.CurUser=Security.LogInWeb(username,password,"",Application.ProductVersion,useEcwAlgorithm);
					Security.PasswordTyped=password;//for ecw, this is already encrypted.
				}
				catch(Exception ex) {
					RemotingClient.ServerURI=originalURI;
					RemotingClient.RemotingRole=originalRole;
					throw ex;
				}
			}
			else {
				DataConnection.DBtype=dbType;
				DataConnection dcon=new DataConnection();
				if(connectionString.Length > 0) {
					dcon.SetDb(connectionString,"",DataConnection.DBtype);
				}
				else {
					//Password could be plain text password from the Password field of the config file, the decrypted password from the MySQLPassHash field
					//of the config file, or password entered by the user and can be blank (empty string) in all cases
					dcon.SetDb(centralConnection.ServerName,centralConnection.DatabaseName,centralConnection.MySqlUser
						,centralConnection.MySqlPassword,"","",DataConnection.DBtype);
				}
				//a direct connection does not utilize lower privileges.
				RemotingClient.RemotingRole=RemotingRole.ClientDirect;
			}
			if(saveOnSuccess) {
				TrySaveConnectionSettings(centralConnection,dbType,connectionString,noShowOnStartup,listAdminCompNames);
			}
		}

		///<summary>If MiddleTierProxyConfix.xml is present, this loads the three variables from that file into memory.
		///Throws exceptions if anything goes wrong which will typically be shown to the user.</summary>
		public static void LoadMiddleTierProxySettings() {
			string xmlPath=ODFileUtils.CombinePaths(Application.StartupPath,"MiddleTierProxyConfig.xml");
			if(!File.Exists(xmlPath)) {
				return;
			}
			XmlDocument doc=new XmlDocument();
			doc.Load(xmlPath);
			RemotingClient.MidTierProxyAddress=doc.SelectSingleNode("//Address").InnerText;
			RemotingClient.MidTierProxyUserName=doc.SelectSingleNode("//UserName").InnerText;
			RemotingClient.MidTierProxyPassword=doc.SelectSingleNode("//Password").InnerText;
		}

		///<summary>Returns true if the connection settings were successfully saved to the FreeDentalConfig file.  Otherwise, false.</summary>
		public static bool TrySaveConnectionSettings(CentralConnection centralConnection,DatabaseType dbType,string connectionString=""
			,bool noShowOnStartup=false,List<string> listAdminCompNames=null) 
		{
			try {
				XmlWriterSettings settings=new XmlWriterSettings();
				settings.Indent=true;
				settings.IndentChars=("    ");
				using(XmlWriter writer=XmlWriter.Create(ODFileUtils.CombinePaths(Application.StartupPath,"FreeDentalConfig.xml"),settings)) {
					writer.WriteStartElement("ConnectionSettings");
					if(connectionString!="") {
						writer.WriteStartElement("ConnectionString");
						writer.WriteString(connectionString);
						writer.WriteEndElement();
					}
					else if(RemotingClient.RemotingRole==RemotingRole.ClientDirect) {
						writer.WriteStartElement("DatabaseConnection");
						writer.WriteStartElement("ComputerName");
						writer.WriteString(centralConnection.ServerName);
						writer.WriteEndElement();
						writer.WriteStartElement("Database");
						writer.WriteString(centralConnection.DatabaseName);
						writer.WriteEndElement();
						writer.WriteStartElement("User");
						writer.WriteString(centralConnection.MySqlUser);
						writer.WriteEndElement();
						string encryptedPwd;
						CDT.Class1.Encrypt(centralConnection.MySqlPassword,out encryptedPwd);//sets encryptedPwd ot value or null
						writer.WriteStartElement("Password");
						//If encryption fails, write plain text password to xml file; maintains old behavior.
						writer.WriteString(string.IsNullOrEmpty(encryptedPwd) ? centralConnection.MySqlPassword : "");
						writer.WriteEndElement();
						writer.WriteStartElement("MySQLPassHash");
						writer.WriteString(encryptedPwd??"");
						writer.WriteEndElement();
						writer.WriteStartElement("NoShowOnStartup");
						if(noShowOnStartup) {
							writer.WriteString("True");
						}
						else {
							writer.WriteString("False");
						}
						writer.WriteEndElement();
						writer.WriteEndElement();
					}
					else if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
						writer.WriteStartElement("ServerConnection");
						writer.WriteStartElement("URI");
						writer.WriteString(centralConnection.ServiceURI);
						writer.WriteEndElement();//end URI
						if(centralConnection.IsAutomaticLogin) {
							writer.WriteStartElement("User");
							writer.WriteString(centralConnection.OdUser);
							writer.WriteEndElement();//end Username
							writer.WriteStartElement("UsingAutoLogin");
							writer.WriteString("True");
							writer.WriteEndElement();//end UsingAutoLogin
						}
						writer.WriteStartElement("UsingEcw");
						if(centralConnection.WebServiceIsEcw) {
							writer.WriteString("True");
						}
						else {
							writer.WriteString("False");
						}
						writer.WriteEndElement();//end UsingEcw
						writer.WriteEndElement();//end ServerConnection
					}
					writer.WriteStartElement("DatabaseType");
					if(dbType==DatabaseType.MySql) {
						writer.WriteString("MySql");
					}
					else {
						writer.WriteString("Oracle");
					}
					writer.WriteEndElement();
					if(OpenDentBusiness.WebTypes.Shared.XWeb.XWebs.UseXWebTestGateway) {
						writer.WriteStartElement("UseXWebTestGateway");
						writer.WriteString("True");
						writer.WriteEndElement();
					}
					if(listAdminCompNames!=null && listAdminCompNames.Count>0) {
						writer.WriteStartElement("AdminCompNames");
						foreach(string compName in listAdminCompNames) {
							writer.WriteStartElement("CompName");
							writer.WriteString(compName);
							writer.WriteEndElement();
						}
						writer.WriteEndElement();
					}
					writer.WriteEndElement();
					writer.Flush();
				}//using writer
				if(centralConnection.IsAutomaticLogin) {//Input the user's credentials to WCM (Windows Credential Manager)
					PasswordVaultWrapper.WritePassword(centralConnection.ServiceURI,centralConnection.OdUser,centralConnection.OdPassword);
				}
			}
			catch(Exception) {
				return false;
			}
			return true;
		}
		#endregion

		#endregion

		///<summary>Gets all the central connections from the database ordered by ItemOrder.</summary>
		public static List<CentralConnection> GetConnections(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<CentralConnection>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM centralconnection ORDER BY ItemOrder";
			return Crud.CentralConnectionCrud.SelectMany(command);
		}

		///<summary>Filters _listConns to only include connections that are associated to the selected connection group.</summary>
		public static List<CentralConnection> FilterConnections(List<CentralConnection> listConns,string filterText,ConnectionGroup connGroup) {
			//No need to check RemotingRole; no call to db.
			//Get all ConnGroupAttaches for selected group.
			List<CentralConnection> retVal=listConns;
			if(connGroup!=null) {
				//Find all central connections that are in the group list
				List<ConnGroupAttach> listCentralConnGroupAttaches=ConnGroupAttaches.GetForGroup(connGroup.ConnectionGroupNum);
				retVal=retVal.FindAll(x => listCentralConnGroupAttaches.Exists(y => y.CentralConnectionNum==x.CentralConnectionNum));
			}
			//Find all central connections that meet the filterText criteria
			retVal=retVal.FindAll(x => x.DatabaseName.ToLower().Contains(filterText.ToLower()) 
																 || x.ServerName.ToLower().Contains(filterText.ToLower()) 
																 || x.ServiceURI.ToLower().Contains(filterText.ToLower()));
			return retVal;
		}

		///<summary></summary>
		public static long Insert(CentralConnection centralConnection){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				centralConnection.CentralConnectionNum=Meth.GetLong(MethodBase.GetCurrentMethod(),centralConnection);
				return centralConnection.CentralConnectionNum;
			}
			return Crud.CentralConnectionCrud.Insert(centralConnection);
		}

		///<summary></summary>
		public static void Update(CentralConnection centralConnection){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),centralConnection);
				return;
			}
			Crud.CentralConnectionCrud.Update(centralConnection);
		}

		///<summary>Updates Status of the provided CentralConnection</summary>
		public static void UpdateStatus(CentralConnection centralConnection) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),centralConnection);
				return;
			}
			string command="UPDATE centralconnection SET ConnectionStatus='"+POut.String(centralConnection.ConnectionStatus)
				+"' WHERE CentralConnectionNum="+POut.Long(centralConnection.CentralConnectionNum);
			Db.NonQ(command);
		}

		///<summary></summary>
		public static void Delete(long centralConnectionNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),centralConnectionNum);
				return;
			}
			string command= "DELETE FROM centralconnection WHERE CentralConnectionNum = "+POut.Long(centralConnectionNum);
			Db.NonQ(command);
		}

		///<summary>Encrypts signature text and returns a base 64 string so that it can go directly into the database.</summary>
		public static string Encrypt(string str,byte[] key){
			//No need to check RemotingRole; no call to db.
			if(str==""){
				return "";
			}
			byte[] ecryptBytes=Encoding.UTF8.GetBytes(str);
			MemoryStream ms=new MemoryStream();
			CryptoStream cs=null;
			Aes aes=new AesManaged();
			aes.Key=key;
			aes.IV=new byte[16];
			ICryptoTransform encryptor=aes.CreateEncryptor(aes.Key,aes.IV);
			cs=new CryptoStream(ms,encryptor,CryptoStreamMode.Write);
			cs.Write(ecryptBytes,0,ecryptBytes.Length);
			cs.FlushFinalBlock();
			byte[] encryptedBytes=new byte[ms.Length];
			ms.Position=0;
			ms.Read(encryptedBytes,0,(int)ms.Length);
			cs.Dispose();
			ms.Dispose();
			if(aes!=null) {
				aes.Clear();
			}
			return Convert.ToBase64String(encryptedBytes);			
		}

		public static string Decrypt(string str,byte[] key) {
			//No need to check RemotingRole; no call to db.
			if(str==""){
				return "";
			}
			try {
				byte[] encrypted=Convert.FromBase64String(str);
				MemoryStream ms=null;
				CryptoStream cs=null;
				StreamReader sr=null;
				Aes aes=new AesManaged();
				aes.Key=key;
				aes.IV=new byte[16];
				ICryptoTransform decryptor=aes.CreateDecryptor(aes.Key,aes.IV);
				ms=new MemoryStream(encrypted);
				cs=new CryptoStream(ms,decryptor,CryptoStreamMode.Read);
				sr=new StreamReader(cs);
				string decrypted=sr.ReadToEnd();
				ms.Dispose();
				cs.Dispose();
				sr.Dispose();
				if(aes!=null) {
					aes.Clear();
				}
				return decrypted;
			}
			catch { 
				//MessageBox.Show("Text entered was not valid encrypted text.");
				return"";
			}
		}

		///<summary></summary>
		private static string GenerateHash(string message) {
			//No need to check RemotingRole; no call to db.
			byte[] data=Encoding.ASCII.GetBytes(message);
			HashAlgorithm algorithm=SHA1.Create();
			byte[] hashbytes=algorithm.ComputeHash(data);
			byte digit1;
			byte digit2;
			string char1;
			string char2;
			StringBuilder strHash=new StringBuilder();
			for(int i=0;i<hashbytes.Length;i++) {
				if(hashbytes[i]==0) {
					digit1=0;
					digit2=0;
				}
				else {
					digit1=(byte)Math.Floor((double)hashbytes[i]/16d);
					//double remainder=Math.IEEERemainder((double)hashbytes[i],16d);
					digit2=(byte)(hashbytes[i]-(byte)(16*digit1));
				}
				char1=ByteToStr(digit1);
				char2=ByteToStr(digit2);
				strHash.Append(char1);
				strHash.Append(char2);
			}
			return strHash.ToString();
		}

		///<summary>Supply a CentralConnection and this method will go through the logic to put together the connection string.</summary>
		public static string GetConnectionString(CentralConnection conn) {
			string connString="";
			if(conn.DatabaseName!="") {
				connString=conn.ServerName;
				connString+=", "+conn.DatabaseName;
			}
			else if(conn.ServiceURI!="") {
				connString=conn.ServiceURI;
			}
			return connString;
		}

		///<summary>The only valid input is a value between 0 and 15.  Text returned will be 1-9 or a-f.</summary>
		private static string ByteToStr(Byte byteVal) {
			//No need to check RemotingRole; no call to db.
			switch(byteVal) {
				case 10:
					return "a";
				case 11:
					return "b";
				case 12:
					return "c";
				case 13:
					return "d";
				case 14:
					return "e";
				case 15:
					return "f";
				default:
					return byteVal.ToString();
			}
		}


		/*
		
		///<summary>Gets one CentralConnection from the db.</summary>
		public static CentralConnection GetOne(long centralConnectionNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<CentralConnection>(MethodBase.GetCurrentMethod(),centralConnectionNum);
			}
			return Crud.CentralConnectionCrud.SelectOne(centralConnectionNum);
		}

		
		*/



	}
}