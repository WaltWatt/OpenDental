using CodeBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Reflection;
using Microsoft.VisualBasic.Devices;
using System.Management;
using System.Linq;

namespace OpenDentBusiness {

	///<summary>Miscellaneous database functions.</summary>
	public class MiscData {

		///<summary>Gets the current date/Time direcly from the server.  Mostly used to prevent uesr from altering the workstation date to bypass security.</summary>
		public static DateTime GetNowDateTime() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<DateTime>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT NOW()";
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				command="SELECT CURRENT_TIMESTAMP FROM DUAL";
			}
			DataTable table=Db.GetTable(command);
			return PIn.DateT(table.Rows[0][0].ToString());
		}

		///<summary>Gets the current date/Time with milliseconds directly from server.  In Mysql we must query the server until the second rolls over, which may take up to one second.  Used to confirm synchronization in time for EHR.</summary>
		public static DateTime GetNowDateTimeWithMilli() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<DateTime>(MethodBase.GetCurrentMethod());
			}
			string command;
			string dbtime;
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command="SELECT NOW()"; //Only up to 1 second precision pre-Mysql 5.6.4.  Does not round milliseconds.
				dbtime=Db.GetScalar(command);
				int secondInit=PIn.DateT(dbtime).Second;
				int secondCur;
				//Continue querying server for current time until second changes (milliseconds will be close to 0)
				do {
					dbtime=Db.GetScalar(command);
					secondCur=PIn.DateT(dbtime).Second;
				}
				while(secondInit==secondCur);
			}
			else {
				command="SELECT CURRENT_TIMESTAMP(3) FROM DUAL"; //Timestamp with milliseconds
				dbtime=Db.GetScalar(command);
			}
			return PIn.DateT(dbtime);
		}

		///<summary>Returns specific information regarding the current version of Windows that is running.</summary>
		public static string GetOSVersionInfo() {
			string versionInfo="";
			//Utilize the visual basic ComputerInfo class in order to get the most accurate OS version.
			//This is because Environment.OSVersion was always returning a version that represented Windows 8.
			ComputerInfo computerInfo=new ComputerInfo();
			versionInfo=computerInfo.OSFullName+(Environment.Is64BitOperatingSystem ? " 64-bit":" 32-bit");	//OSFullName will be blank on wine
			//This chunk is to get the correct version number.  If we use Environment.OSVersion, it will return an incorrect number.
			//This is because the application isn't manifested to windows 8.1 or 10.  As a result is will always return a version num of 6.2.
			//https://msdn.microsoft.com/en-us/library/windows/desktop/ms724832(v=vs.85).aspx
			try {
				ManagementObjectSearcher mangementQuery=new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
				ManagementObject systemInfo=mangementQuery.Get().Cast<ManagementObject>().FirstOrDefault();
				versionInfo+=" Build "+systemInfo.Properties["Version"].Value;
			}
			catch(Exception e) {
				e.DoNothing();
			}
			return versionInfo;
		}

		///<summary>Used in MakeABackup to ensure a unique backup database name.</summary>
		private static bool Contains(string[] arrayToSearch,string valueToTest) {
			//No need to check RemotingRole; no call to db.
			string compare;
			for(int i=0;i<arrayToSearch.Length;i++) {
				compare=arrayToSearch[i];
				if(arrayToSearch[i]==valueToTest) {
					return true;
				}
			}
			return false;
		}

		///<summary>Backs up the database to the same directory as the original just in case the user did not have sense enough to do a backup first.
		///Does not work for Oracle, due to some MySQL specific commands inside.</summary>
		public static long MakeABackup() {
			//This function should always make the backup on the server itself, and since no directories are
			//referred to (all handled with MySQL), this function will always be referred to the server from
			//client machines.
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod());
			}
			//UpdateStreamLinePassword is purposefully named poorly and used in an odd fashion to sort of obfuscate it from our users.
			//GetStringNoCache() will return blank if pref does not exist.
			if(PrefC.GetStringNoCache(PrefName.UpdateStreamLinePassword)=="abracadabra") {
				return 0;
			}
			//only used in two places: upgrading version, and upgrading mysql version.
			//Both places check first to make sure user is using mysql.
			//we have to be careful to throw an exception if the backup is failing.
			DataConnection dcon=new DataConnection();
			string command="SELECT database()";
			DataTable table=dcon.GetTable(command);
			string oldDb=PIn.String(table.Rows[0][0].ToString());
			string newDb=oldDb+"backup_"+DateTime.Today.ToString("MM_dd_yyyy");
			command="SHOW DATABASES";
			table=dcon.GetTable(command);
			string[] databases=new string[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++) {
				databases[i]=table.Rows[i][0].ToString();
			}
			if(Contains(databases,newDb)) {//if the new database name already exists
				//find a unique one
				int uniqueID=1;
				string originalNewDb=newDb;
				do {
					newDb=originalNewDb+"_"+uniqueID.ToString();
					uniqueID++;
				}
				while(Contains(databases,newDb));
			}
			command="CREATE DATABASE `"+newDb+"` CHARACTER SET utf8";
			dcon.NonQ(command);
			command="SHOW FULL TABLES WHERE Table_type='BASE TABLE'";//Tables, not views.  Does not work in MySQL 4.1, however we test for MySQL version >= 5.0 in PrefL.
			table=dcon.GetTable(command);
			string[] tableName=new string[table.Rows.Count];
			for(int i=0;i<table.Rows.Count;i++) {
				tableName[i]=table.Rows[i][0].ToString();
			}
			//switch to using the new database
			DataConnection newDcon=new DataConnection(newDb);
			for(int i=0;i<tableName.Length;i++) {
				//Alert anyone that cares that we are backing up this table.
				ODEvent.Fire(new ODEventArgs("BackupProgress",Lans.g("MiscData","Backing up table")+": "+tableName[i]));
				command="SHOW CREATE TABLE `"+oldDb+"`.`"+tableName[i]+"`";//also works with views. Added backticks around table name for unusual characters.
				table=newDcon.GetTable(command);
				command=PIn.ByteArray(table.Rows[0][1]);
				newDcon.NonQ(command);//this has to be run using connection with new database
				command="INSERT INTO `"+newDb+"`.`"+tableName[i]+"` "
					+"SELECT * FROM `"+oldDb+"`.`"+tableName[i]+"`";//Added backticks around table name for unusual characters.
				newDcon.NonQ(command);
			}
			return 0;
		}
		
		///<summary>SetDbT prior to calling.</summary>
		public static void MakeArchive() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod());
				return;
			}
			string command="CREATE DATABASE `opendentalarchive` CHARACTER SET utf8";
			Db.NonQ(command);//We should already be connected to the archive server.
		}

		public static string GenerateTableQuery(string table) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),table);
			}
			string command="SHOW CREATE TABLE "+POut.String(table);
			return Db.GetTable(command).Rows[0][1].ToString();
		}

		public static void MakeTables(List<string> listTableQueries) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listTableQueries);
				return;
			}
			foreach(string command in listTableQueries) {
				Db.NonQ(command);
			}
		}

		public static void InsertPreferences(DataTable preferences) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),preferences);
				return;
			}
			string command="";
			foreach(DataRow row in preferences.Rows) {
				if(DataConnection.DBtype==DatabaseType.MySql) {
					command="INSERT INTO preference (PrefName,ValueString) VALUES('"+POut.String(row[1].ToString())+"','"+POut.String(row[2].ToString())+"')";
					Db.NonQ(command);
				}
				else {//oracle
					command="INSERT INTO preference (PrefNum,PrefName,ValueString) "
						+"VALUES((SELECT COALESCE(MAX(PrefNum),0)+1 FROM preference),'"+POut.String(row[1].ToString())+"','"+POut.String(row[2].ToString())+"')";
					Db.NonQ(command);
				}
			}
		}

		public static string GetCurrentDatabase() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod());
			}
			string command="SELECT database()";
			DataTable table=Db.GetTable(command);
			return PIn.String(table.Rows[0][0].ToString());
		}

		///<summary>Returns the major and minor version of MySQL for the current connection.  Returns a version of 0.0 if the MySQL version cannot be determined.</summary>
		public static string GetMySqlVersion() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod());
			}
			string command="SELECT @@version";
			DataTable table=Db.GetTable(command);
			string version=PIn.String(table.Rows[0][0].ToString());
			string[] arrayVersion=version.Split('.');
			try {
				return int.Parse(arrayVersion[0])+"."+int.Parse(arrayVersion[1]);
			}
			catch {
			}
			return "0.0";
		}

		///<summary>Gets the human readable host name of the database server, even when using the middle-tier.  This will return an empty string if Dns lookup fails.</summary>
		public static string GetODServer() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod());
			}
			//string command="SELECT @@hostname";//This command fails in MySQL 5.0.22 (the version of MySQL 5.0 we used to use), because the hostname variable was added in MySQL 5.0.38.
			//string rawHostName=DataConnection.GetServerName();//This could be a human readable name, or it might be "localhost" or "127.0.0.1" or another IP address.
			//return Dns.GetHostEntry(rawHostName).HostName;//Return the human readable name (full domain name) corresponding to the rawHostName.
			//Had to strip off the port, caused Dns.GetHostEntry to fail and is not needed to get the hostname
			string rawHostName=DataConnection.GetServerName();
			if(rawHostName!=null) {//rawHostName will be null if the user used a custom ConnectionString when they chose their database.
				rawHostName=rawHostName.Split(':')[0];//This could be a human readable name, or it might be "localhost" or "127.0.0.1" or another IP address.
			}
			string retval="";
			try {
				retval=Dns.GetHostEntry(rawHostName).HostName;//Return the human readable name (full domain name) corresponding to the rawHostName.
			}
			catch(Exception ex) {
				ex.DoNothing();
			}
			return retval;
		}

		///<summary>Returns the current value in the GLOBAL max_allowed_packet variable.
		///max_allowed_packet is stored as an integer in multiples of 1,024 with a min value of 1,024 and a max value of 1,073,741,824.</summary>
		public static int GetMaxAllowedPacket() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod());
			}
			int maxAllowedPacket=0;
			//The SHOW command is used because it was able to run with a user that had no permissions whatsoever.
			string command="SHOW GLOBAL VARIABLES WHERE Variable_name='max_allowed_packet'";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count > 0) {
				maxAllowedPacket=PIn.Int(table.Rows[0]["Value"].ToString());
			}
			return maxAllowedPacket;
		}

		///<summary>Sets the global MySQL variable max_allowed_packet to the passed in size (in bytes).
		///Returns the results of GetMaxAllowedPacket() after running the SET GLOBAL command.</summary>
		public static int SetMaxAllowedPacket(int sizeBytes) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod(),sizeBytes);
			}
			//As of MySQL 5.0.84 the session level max_allowed_packet variable is read only so we only need to change the global.
			string command="SET GLOBAL max_allowed_packet="+POut.Int(sizeBytes);
			Db.NonQ(command);
			return GetMaxAllowedPacket();
		}

		///<summary>Returns a collection of unique AtoZ folders for the array of dbnames passed in.  It will not include the current AtoZ folder for this database, even if shared by another db.  This is used for the feature that updates multiple databases simultaneously.</summary>
		public static List<string> GetAtoZforDb(string[] dbNames) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("MiscData.GetAtoZforDb failed.  Updates not allowed from ClientWeb.");
			}
			List<string> retval=new List<string>();
			DataConnection dcon=null;
			string atozName;
			string atozThisDb=PrefC.GetString(PrefName.DocPath);
			for(int i=0;i<dbNames.Length;i++) {
				try {
					dcon=new DataConnection(dbNames[i]);
					string command="SELECT ValueString FROM preference WHERE PrefName='DocPath'";
					atozName=dcon.GetScalar(command);
					if(retval.Contains(atozName)) {
						continue;
					}
					if(atozName==atozThisDb) {
						continue;
					}
					retval.Add(atozName);
				}
				catch {
					//don't add it to the list
				}
			}
			return retval;
		}

		public static void LockWorkstationsForDbs(string[] dbNames) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("MiscData.LockWorkstationsForDbs failed.  Updates not allowed from ClientWeb.");
			}
			DataConnection dcon=null;
			for(int i=0;i<dbNames.Length;i++) {
				try {
					dcon=new DataConnection(dbNames[i]);
					string command="UPDATE preference SET ValueString ='"+POut.String(Environment.MachineName)
						+"' WHERE PrefName='UpdateInProgressOnComputerName'";
					dcon.NonQ(command);
				}
				catch { }
			}
		}

		public static void UnlockWorkstationsForDbs(string[] dbNames) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("MiscData.UnlockWorkstationsForDbs failed.  Updates not allowed from ClientWeb.");
			}
			DataConnection dcon=null;
			for(int i=0;i<dbNames.Length;i++) {
				try {
					dcon=new DataConnection(dbNames[i]);
					string command="UPDATE preference SET ValueString =''"
						+" WHERE PrefName='UpdateInProgressOnComputerName'";
					dcon.NonQ(command);
				}
				catch { }
			}
		}

		public static void SetSqlMode() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod());
				return;
			}
			//The SHOW command is used because it was able to run with a user that had no permissions whatsoever.
			string command="SHOW GLOBAL VARIABLES WHERE Variable_name='sql_mode'";
			DataTable table=Db.GetTable(command);
			//We want to run the SET GLOBAL command when no rows were returned (above query failed) or if the sql_mode is not blank or NO_AUTO_CREATE_USER
			//(set to something that could cause errors).
			if(table.Rows.Count<1 || (table.Rows[0]["Value"].ToString()!="" && table.Rows[0]["Value"].ToString().ToUpper()!="NO_AUTO_CREATE_USER")) {
				command="SET GLOBAL sql_mode=''";//in case user did not use our my.ini file.  http://www.opendental.com/manual/mysqlservervariables.html
				Db.NonQ(command);
			}
		}

	}

	
}































