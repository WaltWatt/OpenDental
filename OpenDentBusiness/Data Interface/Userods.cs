using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Services;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using CodeBase;

namespace OpenDentBusiness {
	///<summary>(Users OD)</summary>
	public class Userods {
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
		#endregion

		//private static bool webServerConfigHasLoadedd=false;

		#region CachePattern

		private class UserodCache : CacheListAbs<Userod> {
			protected override List<Userod> GetCacheFromDb() {
				string command="SELECT * FROM userod ORDER BY UserName";
				return Crud.UserodCrud.SelectMany(command);
			}
			protected override List<Userod> TableToList(DataTable table) {
				return Crud.UserodCrud.TableToList(table);
			}
			protected override Userod Copy(Userod userod) {
				return userod.Copy();
			}
			protected override DataTable ListToTable(List<Userod> listUserods) {
				return Crud.UserodCrud.ListToTable(listUserods,"Userod");
			}
			protected override void FillCacheIfNeeded() {
				Userods.GetTableFromCache(false);
			}
			protected override bool IsInListShort(Userod userod) {
				return !userod.IsHidden && userod.UserNumCEMT==0;
			}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static UserodCache _userodCache=new UserodCache();

		public static Userod GetFirstOrDefault(Func<Userod,bool> match,bool isShort=false) {
			return _userodCache.GetFirstOrDefault(match,isShort);
		}

		///<summary>Gets a deep copy of all matching items from the cache via ListLong.  Set isShort true to search through ListShort instead.</summary>
		public static List<Userod> GetWhere(Predicate<Userod> match,bool isShort=false) {
			return _userodCache.GetWhere(match,isShort);
		}

		public static List<Userod> GetDeepCopy(bool isShort=false) {
			return _userodCache.GetDeepCopy(isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_userodCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_userodCache.FillCacheFromTable(table);
				return table;
			}
			return _userodCache.GetTableFromCache(doRefreshCache);
		}

		#endregion

		///<summary></summary>
		public static List<Userod> GetAll() {
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command = "SELECT * FROM userod ORDER BY UserName";
			return Crud.UserodCrud.TableToList(Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command));
		}

		///<summary></summary>
		public static Userod GetUser(long userNum) {
			//No need to check RemotingRole; no call to db.
			return GetFirstOrDefault(x => x.UserNum==userNum);
		}

		///<summary>Returns a list of users from the list of usernums.</summary>
		public static List<Userod> GetUsers(List<long> listUserNums) {
			//No need to check RemotingRole; no call to db.
			return GetWhere(x => listUserNums.Contains(x.UserNum));
		}

		///<summary>Returns a list of all non-hidden users.  Does not include CEMT users.</summary>
		public static List<Userod> GetUsers() {
			//No need to check RemotingRole; no call to db.
			return GetUsers(false);
		}

		///<summary>Returns a list of all non-hidden users.  Set includeCEMT to true if you want CEMT users included.</summary>
		public static List<Userod> GetUsers(bool includeCEMT) {
			//No need to check RemotingRole; no call to db.
			List<Userod> retVal=new List<Userod>();
			List<Userod> listUsersLong=Userods.GetDeepCopy();
			for(int i=0;i<listUsersLong.Count;i++) {
				if(listUsersLong[i].IsHidden) {
					continue;
				}
				if(!includeCEMT && listUsersLong[i].UserNumCEMT!=0) {
					continue;
				}
				retVal.Add(listUsersLong[i]);
			}
			return retVal;
		}

		///<summary>Returns a list of all non-hidden users.  Does not include CEMT users.</summary>
		public static List<Userod> GetUsersByClinic(long clinicNum) {
			//No need to check RemotingRole; no call to db.
			return Userods.GetWhere(x => !x.IsHidden)//all non-hidden users
				.FindAll(x => !x.ClinicIsRestricted || x.ClinicNum==clinicNum); //for the given clinic or unassigned to clinic
				//CEMT user filter not required. CEMT users SHOULD be unrestricted to a clinic.
		}
		
		///<summary>Returns a list of all users without using the local cache.  Useful for multithreaded connections.</summary>
		public static List<Userod> GetUsersNoCache() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Userod>>(MethodBase.GetCurrentMethod());
			}
			List<Userod> retVal=new List<Userod>();
			string command="SELECT * FROM userod";
			DataTable tableUsers=Db.GetTable(command);
			retVal=Crud.UserodCrud.TableToList(tableUsers);
			return retVal;
		}

		///<summary>Returns a list of all CEMT users.</summary>
		public static List<Userod> GetUsersForCEMT() {
			//No need to check RemotingRole; no call to db.
			return GetWhere(x => x.UserNumCEMT!=0);
		}

		///<summary>Returns null if not found.  Is not case sensitive.  isEcwTight isn't even used.</summary>
		public static Userod GetUserByName(string userName,bool isEcwTight) {
			//No need to check RemotingRole; no call to db.
			return GetFirstOrDefault(x => !x.IsHidden && x.UserName.ToLower()==userName.ToLower());
		}

		///<summary>Returns null if not found.  isEcwTight is not case sensitive.  isEcwTight isn't even used.
		///Does not use the cache to find a corresponding user with the passed in userName.  Every middle tier call passes through here.</summary>
		public static Userod GetUserByNameNoCache(string userName,bool isEcwTight) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Userod>(MethodBase.GetCurrentMethod(),userName,isEcwTight);
			}
			string command="SELECT * FROM userod WHERE UserName='"+POut.String(userName)+"'";
			List<Userod> listUserods=Crud.UserodCrud.TableToList(Db.GetTable(command));
			return listUserods.FirstOrDefault(x => !x.IsHidden && x.UserName.ToLower()==userName.ToLower());
		}

		///<summary>Returns null if not found.</summary>
		public static Userod GetUserByEmployeeNum(long employeeNum) {
			//No need to check RemotingRole; no call to db.
			return GetFirstOrDefault(x => x.EmployeeNum==employeeNum);
		}

		///<summary>Returns all users that are associated to the employee passed in.  Returns empty list if no matches found.</summary>
		public static List<Userod> GetUsersByEmployeeNum(long employeeNum) {
			//No need to check RemotingRole; no call to db.
			return GetWhere(x => x.EmployeeNum==employeeNum);
		}

		///<summary>Returns all users that are associated to the permission passed in.  Returns empty list if no matches found.</summary>
		public static List<Userod> GetUsersByPermission(Permissions permission,bool showHidden) {
			//No need to check RemotingRole; no call to db.
			List<Userod> listAllUsers=Userods.GetDeepCopy(!showHidden);
			List<Userod> listUserods=new List<Userod>();
			for(int i=0;i<listAllUsers.Count;i++) {
				if(GroupPermissions.HasPermission(listAllUsers[i],permission,0)) {
					listUserods.Add(listAllUsers[i]);
				}
			}
			return listUserods;
		}

		///<summary>Returns all users that are associated to the permission passed in.  Returns empty list if no matches found.</summary>
		public static List<Userod> GetUsersByJobRole(JobPerm jobPerm,bool showHidden) {
			//No need to check RemotingRole; no call to db.
			List<JobPermission> listJobRoles=JobPermissions.GetList().FindAll(x=>x.JobPermType==jobPerm);
			return Userods.GetWhere(x=>listJobRoles.Any(y=>x.UserNum==y.UserNum),!showHidden);
		}

		///<summary>Gets all non-hidden users that have an associated provider.</summary>
		public static List<Userod> GetUsersWithProviders() {
			//No need to check RemotingRole; no call to db.
			return Userods.GetWhere(x => x.ProvNum!=0,true);
		}

		///<summary>Returns all users associated to the provider passed in.  Returns empty list if no matches found.</summary>
		public static List<Userod> GetUsersByProvNum(long provNum) {
			//No need to check RemotingRole; no call to db.
			return Userods.GetWhere(x => x.ProvNum==provNum,true);
		}
		
		public static List<Userod> GetUsersByInbox(long taskListNum) {
			//No need to check RemotingRole; no call to db.
			return Userods.GetWhere(x => x.TaskListInBox==taskListNum,true);
		}

		///<summary>Returns all users selectable for the insurance verification list.  
		///Pass in an empty list to not filter by clinic.  
		///Set isAssigning to false to return only users who have an insurance already assigned.</summary>
		public static List<Userod> GetUsersForVerifyList(List<long> listClinicNums,bool isAssigning) {
			//No need to check RemotingRole; no explicit call to db.
			List<long> listUserNumsInInsVerify=InsVerifies.GetAllInsVerifyUserNums();
			List<long> listUserNumsInClinic=new List<long>();
			if(listClinicNums.Count>0) {
				List<UserClinic> listUserClinics=new List<UserClinic>();
				for(int i=0;i<listClinicNums.Count;i++) {
					listUserNumsInClinic.AddRange(UserClinics.GetForClinic(listClinicNums[i]).Select(y => y.UserNum).Distinct().ToList());
				}
				listUserNumsInClinic.AddRange(GetUsers().FindAll(x => !x.ClinicIsRestricted).Select(x => x.UserNum).Distinct().ToList());//Always add unrestricted users into the list.
				listUserNumsInClinic=listUserNumsInClinic.Distinct().ToList();//Remove duplicates that could possibly be in the list.
				if(listUserNumsInClinic.Count>0) {
					listUserNumsInInsVerify=listUserNumsInInsVerify.FindAll(x => listUserNumsInClinic.Contains(x));
				}
				listUserNumsInInsVerify.AddRange(GetUsers(listUserNumsInInsVerify).FindAll(x => !x.ClinicIsRestricted).Select(x => x.UserNum).Distinct().ToList());//Always add unrestricted users into the list.
				listUserNumsInInsVerify=listUserNumsInInsVerify.Distinct().ToList();
			}
			List<Userod> listUsersWithPerm=GetUsersByPermission(Permissions.InsPlanVerifyList,false);
			if(isAssigning) {
				if(listClinicNums.Count==0) {
					return listUsersWithPerm;//Return unfiltered list of users with permission
				}
				//Don't limit user list to already assigned insurance verifications.
				return listUsersWithPerm.FindAll(x => listUserNumsInClinic.Contains(x.UserNum));//Return users with permission, limited by their clinics
			}
			return listUsersWithPerm.FindAll(x => listUserNumsInInsVerify.Contains(x.UserNum));//Return users limited by permission, clinic, and having an insurance already assigned.
		}

		///<summary>Returns all non-hidden users associated with the domain user name passed in. Returns an empty list if no matches found.</summary>
		public static List<Userod> GetUsersByDomainUserName(string domainUser) {
			return Userods.GetWhere(x => x.DomainUser.Equals(domainUser,StringComparison.InvariantCultureIgnoreCase),true);
		}

		///<summary>This handles situations where we have a usernum, but not a user.  And it handles usernum of zero.</summary>
		public static string GetName(long userNum) {
			//No need to check RemotingRole; no call to db.
			Userod user=GetFirstOrDefault(x => x.UserNum==userNum);
			return (user==null ? "" : user.UserName);
		}

		///<summary>Returns true if the user passed in is associated with a provider that has (or had) an EHR prov key.</summary>
		public static bool IsUserCpoe(Userod user) {
			//No need to check RemotingRole; no call to db.
			if(user==null) {
				return false;
			}
			Provider prov=Providers.GetProv(user.ProvNum);
			if(prov==null) {
				return false;
			}
			//Check to see if this provider has had a valid key at any point in history.
			return EhrProvKeys.HasProvHadKey(prov.LName,prov.FName);
		}

		///<summary>Refreshes the Userod cache from the database (to get updated information regarding failed login attempts) and then finds the 
		///corresponding user based on the username passed in.  If one is not found (case sensitive unless usingEcw is true) then null is returned.
		///Once a user has been found, if isMiddleTierCheck is false then the failed login attempts is checked and if the number of failed log in attempts
		///exceeds the limit an exception is thrown with a message to display to the user.  Then the hash of the password (if usingEcw is true then the
		///password needs to be hashed before passing into this method) is checked against the password hash that is currently in the database.
		///Returns a user object if user and password are valid.  Throws an exception with a specific message to display to the user if anything goes
		///wrong and if isMiddleTierCheck is false, then manipulates the appropriate log in failure columns in the db.</summary>
		public static Userod CheckUserAndPassword(string username,string password,bool isEcw,bool isMiddleTierCheck=false) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Userod>(MethodBase.GetCurrentMethod(),username,password,isEcw,isMiddleTierCheck);
			}
			//Do not use the cache here because an administrator could have cleared the log in failure attempt columns for this user.
			//Also, middle tier calls this method every single time a process request comes to it.
			Userod userDb=GetUserByNameNoCache(username,isEcw);
			if(userDb==null) {
				throw new ODException(Lans.g("Userods","Invalid username or password."),ODException.ErrorCodes.CheckUserAndPasswordFailed);
			}
			DateTime dateTimeNowDb=MiscData.GetNowDateTime();
			//We found a user via matching just the username passed in.  Now we need to check to see if they have exceeded the log in failure attempts.
			//For now we are hardcoding a 5 minute delay when the user has failed to log in 5 times in a row.  
			//An admin user can reset the password or the failure attempt count for the user failing to log in via the Security window.
			//Only do this check if this is ClientDirect or if it's a middle tier login attempt.  Middle tier non-logins will skip this check, i.e. every
			//middle tier process request will call this method before processing the request.  Don't check for attempts exceeded if not logging in.
			//This is so a user who changes their password on another instance will cause this instance to kick out to a failed login screen instead of
			//incrementing the failed attempt count.
			if(!isMiddleTierCheck //skip this if this is a middle tier credentials check before processing a request, so not a user login attempt
				&& userDb.DateTFail.Year > 1880 //The user has failed to log in recently
				&& dateTimeNowDb.Subtract(userDb.DateTFail) < TimeSpan.FromMinutes(5) //The last failure has been within the last 5 minutes.
				&& userDb.FailedAttempts >= 5) //The user failed 5 or more times.
			{
				throw new ApplicationException(Lans.g("Userods","Account has been locked due to failed log in attempts."
					+"\r\nCall your security admin to unlock your account or wait at least 5 minutes."));
			}
			bool isPasswordValid=CheckTypedPassword(password,userDb.Password,isEcw);
			Userod userNew=userDb.Copy();
			if(!isMiddleTierCheck) {//don't update the user if this is a middle tier check prior to processing a request, just kick out to FormLoginFailed
				//If the last failed log in attempt was more than 5 minutes ago, reset the columns in the database so the user can try 5 more times.
				if(userDb.DateTFail.Year > 1880 && dateTimeNowDb.Subtract(userDb.DateTFail) > TimeSpan.FromMinutes(5)) {
					userNew.FailedAttempts=0;
				}
				//Assume the user is going to fail logging in.
				userNew.DateTFail=dateTimeNowDb;
				userNew.FailedAttempts+=1;
				if(isPasswordValid) {
					userNew.DateTFail=DateTime.MinValue;
					userNew.FailedAttempts=0;
				}
				//Synchronize the database with the results of the log in attempt above, but only if this is ClientDirect or a middle tier login attempt.
				Crud.UserodCrud.Update(userNew,userDb);
			}
			if(isPasswordValid) {
				return userNew;
			}
			else {//Password was not valid.
				throw new ODException(Lans.g("Userods","Invalid username or password."),ODException.ErrorCodes.CheckUserAndPasswordFailed);
			}
		}

		///<summary>Checks to see if the hashed version of inputPass matches hashedPass.  If hashedPass is blank, inputPass must be blank as well.
		///When isEcw is true then inputPass should NOT be hashed because it should have already been hashed outside of this method.</summary>
		public static bool CheckTypedPassword(string inputPass,string hashedPass,bool isEcw=false) {
			//No need to check RemotingRole; no call to db.
			if(hashedPass=="") {
				return inputPass=="";
			}
			if(isEcw) {
				return inputPass==hashedPass;
			}
			string hashedInput=HashPassword(inputPass);
			return hashedInput==hashedPass;
		}		

		///<summary>Will throw an exception if it fails for any reason.  This will directly access the config file on the disk, read the values, and set 
		///the DataConnection to the new database.  If the web service attmepts to access the config file, and the config file xml node 
		///'ApplicationName' is missing or blank, it will be appended to the xml file.  If the 'ApplicationName' node
		///is set and the Application Virtual Path for the web service is not the same as the node value, throws an exception, which keeps the IIS service
		///from accessing the wrong database.</summary>
		public static void LoadDatabaseInfoFromFile(string configFilePath){
			//No need to check RemotingRole; no call to db.
			if(!File.Exists(configFilePath)){
				throw new Exception("Could not find "+configFilePath+" on the web server.");
			}
			XmlDocument doc=new XmlDocument();
			try {
				doc.Load(configFilePath);
			}
			catch{
				throw new Exception("Web server "+configFilePath+" could not be opened or is in an invalid format.");
			}
			XPathNavigator Navigator=doc.CreateNavigator();
			//always picks the first database entry in the file:
			XPathNavigator navConn=Navigator.SelectSingleNode("//DatabaseConnection");//[Database='"+database+"']");
			if(navConn==null) {
				throw new Exception(configFilePath+" does not contain a valid database entry.");//database+" is not an allowed database.");
			}
			#region Verify ApplicationName Config File Value
			XPathNavigator configFileNode=navConn.SelectSingleNode("ApplicationName");//usually /OpenDentalServer
			if(configFileNode==null) {//when first updating, this node will not exist in the xml file, so just add it.
				try {
					//AppendChild does not affect the position of the XPathNavigator; adds <ApplicationName>/OpenDentalServer<ApplicationName/> to the xml
					using(XmlWriter writer=navConn.AppendChild()) {
						writer.WriteElementString("ApplicationName",HostingEnvironment.ApplicationVirtualPath);
					}
					doc.Save(configFilePath);
				}
				catch { }//do nothing, unable to write to the XML file, move on anyway
			}
			else if(string.IsNullOrWhiteSpace(configFileNode.Value)) {//empty node, add the Application Virtual Path
				try {
					configFileNode.SetValue(HostingEnvironment.ApplicationVirtualPath);//sets value to /OpenDentalServer or whatever they named their app
					doc.Save(configFilePath);
				}
				catch { }//do nothing, unable to write to the XML file, move on anyway
			}
			else if(configFileNode.Value.ToLower()!=HostingEnvironment.ApplicationVirtualPath.ToLower()) {
				//the xml node exists and this file already has an Application Virtual Path in it that does not match the name of the IIS attempting to access it
				string filePath=ODFileUtils.CombinePaths(Path.GetDirectoryName(configFilePath),HostingEnvironment.ApplicationVirtualPath.Trim('/')+"Config.xml");
				throw new Exception("Multiple middle tier servers are potentially trying to connect to the same database.\r\n"
					+"This middle tier server cannot connect to the database within the config file found.\r\n"
					+"This middle tier server should be using the following config file:\r\n\t"+filePath+"\r\n"
					+"The config file is expecting an ApplicationName of:\r\n\t"+HostingEnvironment.ApplicationVirtualPath);
			}
			#endregion Verify ApplicationName Config File Value
			string connString="",server="",database="",mysqlUser="",mysqlPassword="",mysqlUserLow="",mysqlPasswordLow="";
			XPathNavigator navConString=navConn.SelectSingleNode("ConnectionString");
			if(navConString!=null) {//If there is a connection string then use it.
				connString=navConString.Value;
			}
			else {
				//return navOne.SelectSingleNode("summary").Value;
				//now, get the values for this connection
				server=navConn.SelectSingleNode("ComputerName").Value;
				database=navConn.SelectSingleNode("Database").Value;
				mysqlUser=navConn.SelectSingleNode("User").Value;
				mysqlPassword=navConn.SelectSingleNode("Password").Value;
				XPathNavigator encryptedPwdNode=navConn.SelectSingleNode("MySQLPassHash");
				string decryptedPwd;
				if(mysqlPassword=="" && encryptedPwdNode!=null && encryptedPwdNode.Value!="" && CDT.Class1.Decrypt(encryptedPwdNode.Value,out decryptedPwd)) {
					mysqlPassword=decryptedPwd;
				}
				mysqlUserLow=navConn.SelectSingleNode("UserLow").Value;
				mysqlPasswordLow=navConn.SelectSingleNode("PasswordLow").Value;
			}
			XPathNavigator dbTypeNav=navConn.SelectSingleNode("DatabaseType");
			DatabaseType dbtype=DatabaseType.MySql;
			if(dbTypeNav!=null){
				if(dbTypeNav.Value=="Oracle"){
					dbtype=DatabaseType.Oracle;
				}
			}
			DataConnection dcon=new DataConnection();
			if(connString!="") {
				try {
					dcon.SetDb(connString,"",dbtype);
				}
				catch(Exception e) {
					throw new Exception(e.Message+"\r\n"+"Connection to database failed.  Check the values in the config file on the web server "+configFilePath);
				}
			}
			else {
				try {
					dcon.SetDb(server,database,mysqlUser,mysqlPassword,mysqlUserLow,mysqlPasswordLow,dbtype);
				}
				catch(Exception e) {
					throw new Exception(e.Message+"\r\n"+"Connection to database failed.  Check the values in the config file on the web server "+configFilePath);
				}
			}
			//todo?: make sure no users have blank passwords.
		}

		/*
		///<summary>Used by the SL logon window to validate credentials.  Send in the password unhashed.  If invalid, it will always throw an exception of some type.  If it is valid, then it will return the hashed password.  This is the only place where the config file is read, and it's only read on startup.  So the web service needs to be restarted if the config file changes.</summary>
		public static string CheckDbUserPassword(string configFilePath,string username,string password){
			//for some reason, this static variable was remaining true even if the webservice was restarted.
			//So we're not going to use it anymore.  Always load from file.
			//if(!webServerConfigHasLoadedd){
				LoadDatabaseInfoFromFile(configFilePath);
			//	webServerConfigHasLoadedd=true;
			//}
			DataConnection dcon=new DataConnection();
			//Then, check username and password
			string passhash="";
			string command="SELECT Password FROM userod WHERE UserName='"+POut.PString(username)+"'";
			DataTable table=dcon.GetTable(command);
			if(table.Rows.Count!=0){
				passhash=table.Rows[0][0].ToString();
			}
			if(passhash=="" || passhash!=EncryptPassword(password)){
				throw new Exception("Invalid username or password.");
			}
			return passhash;
		}*/

		///<summary></summary>
		public static string HashPassword(string inputPass) {
			//No need to check RemotingRole; no call to db.
			bool useEcwAlgorithm=Programs.IsEnabled(ProgramName.eClinicalWorks);
			return HashPassword(inputPass,useEcwAlgorithm);
		}

		///<summary>No remoting role check necessary as we now use our internal .dll to do the FIPS compliant hashing.</summary>
		public static string HashPassword(string inputPass,bool useEcwAlgorithm) {
			if(inputPass=="") {
				return "";
			}
			if(useEcwAlgorithm){// && Programs.IsEnabled("eClinicalWorks")) {
				byte[] asciiBytes=Encoding.ASCII.GetBytes(inputPass);
				byte[] hashbytes=ODCrypt.MD5.Hash(asciiBytes);
				return BitConverter.ToString(hashbytes).Replace("-","").ToLower();
			}
			else {//typical, only difference is encoding.
				byte[] unicodeBytes=Encoding.Unicode.GetBytes(inputPass);
				byte[] hashbytes2=ODCrypt.MD5.Hash(unicodeBytes);
				return Convert.ToBase64String(hashbytes2);
			}
		}

		/////<summary>usertype can be 'all', 'prov', 'emp', 'stu', 'inst', or 'other'.</summary>
		//public static List<Userod> RefreshSecurity(string usertype,long schoolClassNum) {
		//	if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
		//		return Meth.GetObject<List<Userod>>(MethodBase.GetCurrentMethod(),usertype,schoolClassNum);
		//	}
		//	string command;
		//	if(usertype=="stu" || usertype=="inst") {
		//		command="SELECT userod.* FROM userod,provider "
		//			+"WHERE userod.ProvNum=provider.ProvNum "
		//			+"AND provider.IsInstructor=";
		//		if(usertype=="inst") {
		//			command+="1 ";
		//		}
		//		else {
		//			command+="0 ";
		//		}
		//		if(usertype=="stu" && schoolClassNum>0) {
		//			command+="AND SchoolClassNum="+POut.Long(schoolClassNum)+" ";
		//		}
		//		command+="ORDER BY UserName";
		//		return Crud.UserodCrud.SelectMany(command);
		//	}
		//	command="SELECT * FROM userod WHERE UserNumCEMT=0  ";
		//	if(usertype=="emp"){
		//		command+="AND EmployeeNum!=0 ";
		//	}
		//	else if(usertype=="prov") {//and all schoolclassnums
		//		command+="AND ProvNum!=0 ";
		//	}
		//	else if(usertype=="all") {
		//		//command+="";
		//	}
		//	else if(usertype=="other") {
		//		command+="AND ProvNum=0 AND EmployeeNum=0 ";
		//	}
		//	command+="ORDER BY UserName";
		//	return Crud.UserodCrud.SelectMany(command);
		//}

		///<summary>Updates all students/instructors to the specified user group.  Surround with try/catch because it can throw exceptions.</summary>
		public static void UpdateUserGroupsForDentalSchools(UserGroup userGroup,bool isInstructor) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userGroup,isInstructor);
				return;
			}
			string command;
			//Check if the user group that the students or instructors are trying to go to has the SecurityAdmin permission.
			if(!GroupPermissions.HasPermission(userGroup.UserGroupNum,Permissions.SecurityAdmin,0)) {
				//We need to make sure that moving these users to the new user group does not eliminate all SecurityAdmin users in db.
				command="SELECT COUNT(*) FROM usergroupattach "
					+"INNER JOIN usergroup ON usergroupattach.UserGroupNum=usergroup.UserGroupNum "
					+"INNER JOIN grouppermission ON grouppermission.UserGroupNum=usergroup.UserGroupNum "
					+"WHERE usergroupattach.UserNum NOT IN "
					+"(SELECT userod.UserNum FROM userod,provider "
						+"WHERE userod.ProvNum=provider.ProvNum ";
				if(!isInstructor) {
					command+="AND provider.IsInstructor="+POut.Bool(isInstructor)+" ";
					command+="AND provider.SchoolClassNum!=0) ";
				}
				else {
					command+="AND provider.IsInstructor="+POut.Bool(isInstructor)+") ";
				}
					command+="AND grouppermission.PermType="+POut.Int((int)Permissions.SecurityAdmin)+" ";
				int lastAdmin=PIn.Int(Db.GetCount(command));
				if(lastAdmin==0) {
					throw new Exception("Cannot move students or instructors to the new user group because it would leave no users with the SecurityAdmin permission.");
				}
			}
			command="UPDATE userod INNER JOIN provider ON userod.ProvNum=provider.ProvNum "
					+"SET UserGroupNum="+POut.Long(userGroup.UserGroupNum)+" "
					+"WHERE provider.IsInstructor="+POut.Bool(isInstructor);
			if(!isInstructor) {
				command+=" AND provider.SchoolClassNum!=0";
			}
			Db.NonQ(command);
		}

		///<summary>Surround with try/catch because it can throw exceptions.</summary>
		public static void Update(Userod userod, List<long> listUserGroupNums=null) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userod,listUserGroupNums);
				return;
			}
			Validate(false,userod,false,listUserGroupNums);
			Crud.UserodCrud.Update(userod);
			if(listUserGroupNums == null) {
				return;
			}
			UserGroupAttaches.SyncForUser(userod,listUserGroupNums);
		}

		///<summary>Update for CEMT only.  Used when updating Remote databases with information from the CEMT.  Because of potentially different primary keys we have to update based on UserNumCEMT.</summary>
		public static void UpdateCEMT(Userod userod) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userod);
				return;
			}
			//This should never happen, but is a failsafe to prevent the overwriting of all non-CEMT users in the remote database.
			if(userod.UserNumCEMT == 0) {
				return;
			}
			//Validate(false,userod,false);//Can't use this validate. it's for normal updating only.
			string command="UPDATE userod SET "
				+"UserName          = '"+POut.String(userod.UserName)+"', "
				+"Password          = '"+POut.String(userod.Password)+"', "
				//+"UserGroupNum      =  "+POut.Long(userod.UserGroupNum)+", "//need to find primary key of remote user group
				+"EmployeeNum       =  "+POut.Long  (userod.EmployeeNum)+", "
				+"ClinicNum         =  "+POut.Long  (userod.ClinicNum)+", "
				+"ProvNum           =  "+POut.Long  (userod.ProvNum)+", "
				+"IsHidden          =  "+POut.Bool  (userod.IsHidden)+", "
				+"TaskListInBox     =  "+POut.Long  (userod.TaskListInBox)+", "
				+"AnesthProvType    =  "+POut.Int   (userod.AnesthProvType)+", "
				+"DefaultHidePopups =  "+POut.Bool  (userod.DefaultHidePopups)+", "
				+"PasswordIsStrong  =  "+POut.Bool  (userod.PasswordIsStrong)+", "
				+"ClinicIsRestricted=  "+POut.Bool  (userod.ClinicIsRestricted)+", "
				+"InboxHidePopups   =  "+POut.Bool  (userod.InboxHidePopups)+" "
				+"WHERE UserNumCEMT = "+POut.Long(userod.UserNumCEMT);
			Db.NonQ(command);
		}

		///<summary>Surround with try/catch because it can throw exceptions.  Only used from FormOpenDental.menuItemPassword_Click().  
		///Same as Update(), only the Validate call skips checking duplicate names for hidden users.</summary>
		public static void UpdatePassword(Userod userod,string newPassHashed,bool isPasswordStrong) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userod,newPassHashed,isPasswordStrong);
				return;
			}
			Userod userToUpdate=userod.Copy();
			userToUpdate.Password=newPassHashed;
			userToUpdate.PasswordIsStrong=isPasswordStrong;
			List<UserGroup> listUserGroups=userToUpdate.GetGroups(); //do not include CEMT users.
			if(listUserGroups.Count < 1) {
				throw new Exception(Lans.g("Userods","The current user must be in at least one user group."));
			}
			Validate(false,userToUpdate,true,listUserGroups.Select(x => x.UserGroupNum).ToList());
			Crud.UserodCrud.Update(userToUpdate);
		}

		///<summary>A user must always have at least one associated userGroupAttach. Pass in the usergroup(s) that should be attached.
		///Surround with try/catch because it can throw exceptions.</summary>
		public static long Insert(Userod userod,List<long> listUserGroupNums,bool isForCEMT = false){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				userod.UserNum=Meth.GetLong(MethodBase.GetCurrentMethod(),userod,listUserGroupNums,isForCEMT);
				return userod.UserNum;
			}
			if(userod.IsHidden && UserGroups.IsAdminGroup(listUserGroupNums)) {
				throw new Exception(Lans.g("Userods","Admins cannot be hidden."));
			}
			Validate(true,userod,false,listUserGroupNums);
			long userNum = Crud.UserodCrud.Insert(userod);
			UserGroupAttaches.SyncForUser(userod,listUserGroupNums);
			if(isForCEMT) {
				userod.UserNumCEMT=userNum;
				Crud.UserodCrud.Update(userod);
			}
			return userNum;
		}

		public static long InsertNoCache(Userod userod){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetLong(MethodBase.GetCurrentMethod(),userod);
			}
			return Crud.UserodCrud.InsertNoCache(userod);
		}

		///<summary>Surround with try/catch because it can throw exceptions.  
		///We don't really need to make this public, but it's required in order to follow the RemotingRole pattern.
		///listUserGroupNum can only be null when validating for an Update.</summary>
		public static void Validate(bool isNew,Userod user,bool excludeHiddenUsers,List<long> listUserGroupNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),isNew,user,excludeHiddenUsers,listUserGroupNum);
				return;
			}
			//should add a check that employeenum and provnum are not both set.
			//make sure username is not already taken
			string command;
			long excludeUserNum;
			if(isNew){
				excludeUserNum=0;
			}
			else{
				excludeUserNum=user.UserNum;//it's ok if the name matches the current username
			}
			//It doesn't matter if the UserName is already in use if the user being updated is going to be hidden.  This check will block them from unhiding duplicate users.
			if(!user.IsHidden) {//if the user is now not hidden
				//CEMT users will not be visible from within Open Dental.  Therefore, make a different check so that we can know if the name
				//the user typed in is a duplicate of a CEMT user.  In doing this, we are able to give a better message.
				if(!IsUserNameUnique(user.UserName,excludeUserNum,excludeHiddenUsers,true)) {
					throw new Exception(Lans.g("Userods","UserName already in use by CEMT member."));
				}
				if(!IsUserNameUnique(user.UserName,excludeUserNum,excludeHiddenUsers)) {
					//IsUserNameUnique doesn't care if it's a CEMT user or not.. It just gets a count based on username.
					throw new Exception(Lans.g("Userods","UserName already in use."));
				}
			}
			if(listUserGroupNum==null) {//Not validating UserGroup selections.
				return;
			}
			if(listUserGroupNum.Count<1) {
				throw new Exception(Lans.g("Userods","The current user must be in at least one user group."));
			}
			//an admin user can never be hidden
			command="SELECT COUNT(*) FROM grouppermission "
				+"WHERE PermType='"+POut.Long((int)Permissions.SecurityAdmin)+"' "
				+"AND UserGroupNum IN ("+string.Join(",",listUserGroupNum)+") ";
			if(!isNew//Updating.
				&& Db.GetCount(command)=="0"//if this user would not have admin
				&& !IsSomeoneElseSecurityAdmin(user))//make sure someone else has admin
			{
				throw new Exception(Lans.g("Users","At least one user must have Security Admin permission."));
			}
			if(user.IsHidden//hidden 
				&& user.UserNumCEMT==0//and non-CEMT
				&& Db.GetCount(command)!="0")//if this user is admin
			{
				throw new Exception(Lans.g("Userods","Admins cannot be hidden."));
			}
		}

		/// <summary>Returns true if there is at least one user part of the SecurityAdmin permission excluding the user passed in.</summary>
		public static bool IsSomeoneElseSecurityAdmin(Userod user) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),user);
			}
			string command="SELECT COUNT(*) FROM userod "
				+"INNER JOIN usergroupattach ON usergroupattach.UserNum=userod.UserNum "
				+"INNER JOIN grouppermission ON usergroupattach.UserGroupNum=grouppermission.UserGroupNum "
				+"WHERE grouppermission.PermType='"+POut.Long((int)Permissions.SecurityAdmin)+"'"
				+" AND userod.IsHidden =0"
				+" AND userod.UserNum != "+POut.Long(user.UserNum);
			if(Db.GetCount(command)=="0") {//there are no other users with this permission
				return false;
			}
			return true;
		}

		public static bool IsUserNameUnique(string username,long excludeUserNum,bool excludeHiddenUsers) {
			return IsUserNameUnique(username,excludeUserNum,excludeHiddenUsers,false);
		}

		///<summary>Supply 0 or -1 for the excludeUserNum to not exclude any.</summary>
		public static bool IsUserNameUnique(string username,long excludeUserNum,bool excludeHiddenUsers,bool searchCEMTUsers) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),username,excludeUserNum,excludeHiddenUsers);
			}
			if(username==""){
				return false;
			}
			string command="SELECT COUNT(*) FROM userod WHERE ";
			//if(Programs.UsingEcwTight()){
			//	command+="BINARY ";//allows different usernames based on capitalization.//we no longer allow this
				//Does not need to be tested under Oracle because eCW users do not use Oracle.
			//}
			command+="UserName='"+POut.String(username)+"' "
				+"AND UserNum !="+POut.Long(excludeUserNum)+" ";
			if(excludeHiddenUsers) {
				command+="AND IsHidden=0 ";//not hidden
			}
			if(searchCEMTUsers) {
				command+="AND UserNumCEMT!=0";
			}
			DataTable table=Db.GetTable(command);
			if(table.Rows[0][0].ToString()=="0") {
				return true;
			}
			return false;
		}

		///<summary>Used in FormSecurity.FillTreeUsers</summary>
		public static List<Userod> GetForGroup(long userGroupNum) {
			//No need to check RemotingRole; no call to db.
			return GetWhere(x => x.IsInUserGroup(userGroupNum));
		}

		///<summary>Gets a list of users for which the passed-in clinicNum is the only one they have access to.</summary>
		public static List<Userod> GetUsersOnlyThisClinic(long clinicNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Userod>>(MethodBase.GetCurrentMethod(),clinicNum);
			}
			string command="SELECT userod.* "
			+ "FROM( "
				+ "SELECT userclinic.UserNum,COUNT(userclinic.ClinicNum) Clinics FROM userclinic "
				+ "GROUP BY userNum "
				+ "HAVING Clinics = 1 "
			+ ") users "
			+ "INNER JOIN userclinic ON userclinic.UserNum = users.UserNum "
				+ "AND userclinic.ClinicNum = " + POut.Long(clinicNum) + " "
			+ "INNER JOIN userod ON userod.UserNum = userclinic.UserNum ";
			return Crud.UserodCrud.SelectMany(command);
		}

		///<summary>This always returns one admin user.  There must be one and there is rarely more than one.
		///Only used on startup to determine if security is being used.  Can return null. Only returns non-hidden users</summary>
		public static Userod GetAdminUser() {
			//No need to check RemotingRole; no call to db.
			//just find any permission for security admin.  There has to be one.
			List<GroupPermission> listGroupPermissions=GroupPermissions.GetWhere(x => x.PermType==Permissions.SecurityAdmin);
			//Return the first user that is in any of the admin user groups.
			return GetFirstOrDefault(x => listGroupPermissions.Any(y => x.IsInUserGroup(y.UserGroupNum)),true);
		}

		/// <summary>Will return 0 if no inbox found for user.</summary>
		public static long GetInbox(long userNum) {
			//No need to check RemotingRole; no call to db.
			Userod userod=GetFirstOrDefault(x => x.UserNum==userNum);
			return (userod==null ? 0 : userod.TaskListInBox);
		}

		///<summary>Returns 3, which is non-admin provider type, if no match found.</summary>
		public static long GetAnesthProvType(long anesthProvType) {
			//No need to check RemotingRole; no call to db.
			Userod userod=GetFirstOrDefault(x => x.AnesthProvType==anesthProvType);
			return (userod==null ? 3 : userod.AnesthProvType);
		}

		public static List<Userod> GetUsersForJobs() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Userod>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM userod "
				+"INNER JOIN jobpermission ON userod.UserNum=jobpermission.UserNum "
				+"WHERE IsHidden=0 GROUP BY userod.UserNum ORDER BY UserName";
			return Crud.UserodCrud.SelectMany(command);
		}

		///<summary>Returns empty string if password is strong enough.  Otherwise, returns explanation of why it's not strong enough.</summary>
		public static string IsPasswordStrong(string pass) {
			//No need to check RemotingRole; no call to db.
			if(pass=="") {
				return Lans.g("FormUserPassword","Password may not be blank when the strong password feature is turned on.");
			}
			if(pass.Length<8) {
				return Lans.g("FormUserPassword","Password must be at least eight characters long when the strong password feature is turned on.");
			}
			bool containsCap=false;
			for(int i=0;i<pass.Length;i++) {
				if(Char.IsUpper(pass[i])) {
					containsCap=true;
				}
			}
			if(!containsCap) {
				return Lans.g("FormUserPassword","Password must contain at least one capital letter when the strong password feature is turned on.");
			}
			bool containsLower=false;
			for(int i=0;i<pass.Length;i++) {
				if(Char.IsLower(pass[i])) {
					containsLower=true;
				}
			}
			if(!containsLower) {
				return Lans.g("FormUserPassword","Password must contain at least one lower case letter when the strong password feature is turned on.");
			}
			if(PrefC.GetBool(PrefName.PasswordsStrongIncludeSpecial)) {
				bool hasSpecial=false;
				for(int i=0;i<pass.Length;i++) {
					if(!Char.IsLetterOrDigit(pass[i])) {
						hasSpecial=true;
						break;
					}
				}
				if(!hasSpecial) {
					return Lans.g("FormUserPassword","Password must contain at least one special character when the 'strong passwords require a special character' feature is turned on.");
				}
			}
			bool containsNum=false;
			for(int i=0;i<pass.Length;i++) {
				if(Char.IsNumber(pass[i])) {
					containsNum=true;
				}
			}
			if(!containsNum) {
				return Lans.g("FormUserPassword","Password must contain at least one number when the strong password feature is turned on.");
			}
			return "";
		}

		///<summary>This resets the strong password flag on all users after an admin turns off pref PasswordsMustBeStrong.  If strong passwords are again turned on later, then each user will have to edit their password in order set the strong password flag again.</summary>
		public static void ResetStrongPasswordFlags() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod());
				return;
			}
			string command="UPDATE userod SET PasswordIsStrong=0";
			Db.NonQ(command);
		}

		///<summary>Returns true if the passed-in user is apart of the passed-in usergroup.</summary>
		public static bool IsInUserGroup(long userNum,long userGroupNum) {
			//No need to check RemotingRole; no call to db.
			List<UserGroupAttach> listAttaches = UserGroupAttaches.GetForUser(userNum);
			return listAttaches.Select(x => x.UserGroupNum).Contains(userGroupNum);
		}



	}
}
