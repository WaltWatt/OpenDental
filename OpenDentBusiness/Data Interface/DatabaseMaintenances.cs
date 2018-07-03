using CodeBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenDentBusiness {
	public class DatabaseMaintenances {

		private const string _lanThis="FormDatabaseMaintenance";

		#region Get Methods
		public static List<DatabaseMaintenance> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<DatabaseMaintenance>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM databasemaintenance";
			return Crud.DatabaseMaintenanceCrud.SelectMany(command);
		}
		#endregion
		#region Modification Methods
		#region Insert
		///<summary>Compares all DBM methods in the database to the entire list of methods passed in.</summary>
		public static void InsertMissingDBMs(List<string> listDbmMethods) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listDbmMethods);
				return;
			}
			List<string> listDbmMethodNames=GetAll().Select(x => x.MethodName).ToList();
			foreach(string methodName in listDbmMethods) {
				if(listDbmMethodNames.Contains(methodName)) {
					continue;
				}
				DatabaseMaintenance dbm=new DatabaseMaintenance();
				dbm.MethodName=methodName;
				Crud.DatabaseMaintenanceCrud.Insert(dbm);
			}
		}
		#endregion
		#region Update
		public static void Update(DatabaseMaintenance dbm) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),dbm);
				return;
			}
			Crud.DatabaseMaintenanceCrud.Update(dbm);
		}

		///<summary>Updates the DateLastRun column to NOW for any DBM method that matches the method name passed in.</summary>
		public static void UpdateDateLastRun(string methodName) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),methodName);
				return;
			}
			string command="UPDATE databasemaintenance SET DateLastRun="+DbHelper.Now()+" "
				+"WHERE MethodName='"+POut.String(methodName)+"'";
			Db.NonQ(command);
		}
		#endregion
		#region Delete
		#endregion
		#endregion
		#region Misc Methods
		///<summary>Throws exceptions to be displayed to the user.</summary>
		public static void SaveLogToFile(string logText) {
			//No need to check RemotingRole; no call to db.
			StringBuilder strB=new StringBuilder();
			strB.Append(DateTime.Now.ToString());
			strB.Append('-',65);
			strB.AppendLine();//New line.
			strB.Append(logText);
			strB.AppendLine(Lans.g("FormDatabaseMaintenance","Done"));
			string path=CodeBase.ODFileUtils.CombinePaths(System.Windows.Forms.Application.StartupPath,"RepairLog.txt");
			try {
				File.AppendAllText(path,strB.ToString());
			}
			catch(SecurityException) {
				throw new SecurityException(Lans.g("FormDatabaseMaintenance","Log not saved to Repairlog.txt because user does not have permission to access that file."));
			}
			catch(UnauthorizedAccessException) {
				throw new UnauthorizedAccessException(Lans.g("FormDatabaseMaintenance","Log not saved to Repairlog.txt because user does not have permission to access that file."));
			}
			//Throw all other types of exceptions like usual.
		}
		#endregion

		#region List of Tables and Columns for null check---------------------------------------------------------------------------------------------------
		///<summary>List of tables and columns to remove null characters from.
		///Loop through this list two items at a time because it is designed to have a table first which is then followed by a relative column.</summary>
		private static List<string> _listTableAndColumns=new List<string>() {
				//Table					//Column
				"adjustment",   "AdjNote",
				"appointment",  "Note",
				"commlog",      "Note",
				"definition",   "ItemName",
				"diseasedef",   "DiseaseName",
				"patient",      "Address",
				"patient",      "Address2",
				"patient",      "AddrNote",
				"patient",      "MedUrgNote",
				"patientnote",  "FamFinancial",
				"patientnote",  "Medical",
				"patientnote",  "MedicalComp",
				"patientnote",  "Service",
				"patientnote",  "Treatment",
				"payment",      "PayNote",
				"procnote",     "Note",
			};
		#endregion List of Tables and Columns for null check------------------------------------------------------------------------------------------------

		#region Methods That Affect All or Many Tables------------------------------------------------------------------------------------------------------

		///<summary></summary>
		[DbmMethodAttr]
		public static string MySQLServerOptionsValidate(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			string command="SHOW GLOBAL VARIABLES LIKE 'sql_mode'";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count<1||table.Columns.Count<2) {//user may not have permission to access global variables?
				return Lans.g("FormDatabaseMaintenance","Unable to access the MySQL server variable 'sql_mode', probably due to permissions")+".  "
					+Lans.g("FormDatabaseMaintenance","The sql_mode must be blank or NO_AUTO_CREATE_USER")+".\r\n";
			}
			string sqlmode=table.Rows[0][1].ToString();
			string sqlmodeDisplay=(string.IsNullOrWhiteSpace(sqlmode) ? Lans.g("FormDatabaseMaintenance","blank") : sqlmode);//translated 'blank' for display
			if(string.IsNullOrWhiteSpace(sqlmode)||sqlmode.ToUpper()=="NO_AUTO_CREATE_USER") {
				if(!verbose) {
					//Nothing broken, not verbose, show ""
					return "";
				}
				else {
					//Nothing is broken, verbose on, show current sql_mode.
					return Lans.g("FormDatabaseMaintenance","The MySQL server variable 'sql_mode' is currently set to")+" "+sqlmodeDisplay+".\r\n";
				}
			}
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					log+=Lans.g("FormDatabaseMaintenance","The MySQL server variable 'sql_mode' must be blank or NO_AUTO_CREATE_USER and is currently set to")
						+" "+sqlmodeDisplay+".\r\n";
					break;
				case DbmMode.Fix:
					try {
						command="SET GLOBAL sql_mode=''";
						Db.NonQ(command);
						command="SET SESSION sql_mode=''";
						Db.NonQ(command);
						log+=Lans.g("FormDatabaseMaintenance","The MySQL server variable 'sql_mode' has been changed from")+" "+sqlmodeDisplay+" "
							+Lans.g("FormDatabaseMaintenance","to blank")+".\r\n";
					}
					catch(Exception ex) {
						ex.DoNothing();//prevent vs warning
						log+=Lans.g("FormDatabaseMaintenance","Unable to set the MySQL server variable 'sql_mode', probably due to permissions")+".  "
							+Lans.g("FormDatabaseMaintenance","The sql_mode must be blank or NO_AUTO_CREATE_USER and is currently set to")
							+" "+sqlmodeDisplay+".\r\n";
					}
					break;
			}//end switch
			return log;
		}

		///<summary>Returns a Tuple with Item1=log string and Item2=whether the table checks were successful.</summary>
		public static ODTuple<string,bool> MySQLTables(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<ODTuple<string,bool>>(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			bool success=true;
			if(DataConnection.DBtype!=DatabaseType.MySql) {
				return new ODTuple<string, bool>("",success);
			}
			if(PrefC.GetBool(PrefName.DatabaseMaintenanceSkipCheckTable)) {
				return new ODTuple<string,bool>("",success);
			}
			string command="DROP TABLE IF EXISTS `signal`";//Signal is keyword for MySQL 5.5.  Was renamed to signalod so drop if exists.
			Db.NonQ(command);
			command="SHOW FULL TABLES WHERE Table_type='BASE TABLE'";//Tables, not views.  Does not work in MySQL 4.1, however we test for MySQL version >= 5.0 in PrefL.
			DataTable table=Db.GetTable(command);
			string[] tableNames=new string[table.Rows.Count];
			int lastRow;
			bool existsCorruptFiles=false;
			bool existsUnvalidatedTables=false;
			for(int i = 0;i<table.Rows.Count;i++) {
				tableNames[i]=table.Rows[i][0].ToString();
			}
			for(int i = 0;i<tableNames.Length;i++) {
				//Alert anyone that cares that we are checking this table.
				ODEvent.Fire(new ODEventArgs("CheckTableProgress",Lans.g("MiscData","Checking table")+": "+tableNames[i]));
				command="CHECK TABLE `"+tableNames[i]+"`";
				try {
					table=Db.GetTable(command);
					lastRow=table.Rows.Count-1;
					string status=PIn.ByteArray(table.Rows[lastRow][3]);
					if(status!="OK") {
						log+=Lans.g("FormDatabaseMaintenance","Corrupt file found for table")+" "+tableNames[i]+"\r\n";
						existsCorruptFiles=true;
					}
				}
				catch(Exception ex) {
					log+=Lans.g("FormDatabaseMaintenance","Unable to validate table")+" "+tableNames[i]+"\r\n"+ex.Message+"\r\n";
					existsUnvalidatedTables=true;
				}
			}
			if(existsUnvalidatedTables) {
				success=false;//no other checks should be done until we can successfully get past this.
				log+=Lans.g("FormDatabaseMaintenance","Tables found that could not be validated.")+"\r\n"
					+Lans.g("FormDatabaseMaintenance","Done.");
			}
			if(existsCorruptFiles) {
				success=false;//no other checks should be done until we can successfully get past this.
				log+=Lans.g("FormDatabaseMaintenance","Corrupted database files found, please call support immediately or see manual for more details.")+"\r\n"
					+Lans.g("FormDatabaseMaintenance","Done.");
			}
			if(!existsUnvalidatedTables && !existsCorruptFiles) {
				if(verbose) {
					log+=Lans.g("FormDatabaseMaintenance","Tables validated successfully.  No corrupted tables.")+"\r\n";
				}
			}
			return new ODTuple<string,bool>(log,success);
		}

		///<summary>If using MySQL, tries to repair and then optimize each table.
		///Developers must make a backup prior to calling this method because repairs have a tendency to delete data.
		///Currently called whenever MySQL is upgraded and when users click Optimize in database maintenance.</summary>
		public static string RepairAndOptimize(bool isLogged = false) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),isLogged);
			}
			StringBuilder retVal=new StringBuilder();
			DataTable tableLog=null;
			if(DataConnection.DBtype!=DatabaseType.MySql) {
				return "";
			}
			string command="SHOW FULL TABLES WHERE Table_type='BASE TABLE';";//Tables, not views.  Does not work in MySQL 4.1, however we test for MySQL version >= 5.0 in PrefL.
			DataTable table=Db.GetTable(command);
			string[] tableNames=new string[table.Rows.Count];
			for(int i = 0;i<table.Rows.Count;i++) {
				tableNames[i]=table.Rows[i][0].ToString();
			}
			for(int i = 0;i<tableNames.Length;i++) {
				//Alert anyone that cares that we are optimizing this table.
				ODEvent.Fire(new ODEventArgs("RepairAndOptimizeProgress",Lans.g("MiscData","Optimizing table")+": "+tableNames[i]));
				string optimizeResult=OptimizeTable(tableNames[i],isLogged);
				if(isLogged) {
					retVal.AppendLine(optimizeResult);
				}
			}
			for(int i = 0;i<tableNames.Length;i++) {
				//Alert anyone that cares that we are repairing this table.
				ODEvent.Fire(new ODEventArgs("RepairAndOptimizeProgress",Lans.g("MiscData","Repairing table")+": "+tableNames[i]));
				command="REPAIR TABLE `"+tableNames[i]+"`";
				if(!isLogged) {
					Db.NonQ(command);
				}
				else {
					tableLog=Db.GetTable(command);
					for(int r = 0;r<tableLog.Rows.Count;r++) {//usually only 1 row, unless something abnormal is found.
						retVal.AppendLine(tableLog.Rows[r]["Table"].ToString().PadRight(50,' ')
							+" | "+tableLog.Rows[r]["Op"].ToString()
							+" | "+tableLog.Rows[r]["Msg_type"].ToString()
							+" | "+tableLog.Rows[r]["Msg_text"].ToString());
					}
				}
			}
			return retVal.ToString();
		}

		///<summary>Optimizes the table passed in.  Set hasResult to true to return a string representation of the query results.
		///Does not attempt the optimize if random PKs is turned on or if the table is of storage engine InnoDB.
		///See wiki page [[Database Storage Engine Comparison: InnoDB vs MyISAM]] for reasons why.</summary>
		public static string OptimizeTable(string tableName,bool hasResult = false) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),tableName,hasResult);
			}
			if(DataConnection.DBtype!=DatabaseType.MySql) {
				return "";
			}
			string retVal="";
			if(PrefC.GetBool(PrefName.RandomPrimaryKeys)) {
				retVal=tableName+" "+Lans.g("MiscData","skipped due to using random primary keys.");
			}
			//Check to see if the table has its storage engine set to InnoDB.
			string command="SELECT ENGINE FROM information_schema.TABLES "
				+"WHERE TABLE_SCHEMA='"+POut.String(DataConnection.GetDatabaseName())+"' "
				+"AND TABLE_NAME='"+POut.String(tableName)+"' ";
			string storageEngine=Db.GetScalar(command);
			if(storageEngine.ToLower()=="innodb") {
				retVal=tableName+" "+Lans.g("MiscData","skipped due to using the InnoDB storage engine.");
			}
			//Only run OPTIMIZE if random PKs are not used and the table is not using the InnoDB storage engine.
			if(retVal=="") {
				command="OPTIMIZE TABLE `"+tableName+"`";//Ticks used in case user has added custom tables with unusual characters.
				DataTable tableLog=Db.GetTable(command);
				if(hasResult) {
					//Loop through any rows returned and return the results.  Often times will only be one row unless there was a problem with optimizing.
					foreach(DataRow row in tableLog.Rows) {
						retVal+=(row["Table"].ToString().PadRight(50,' ')
							+" | "+row["Op"].ToString()
							+" | "+row["Msg_type"].ToString()
							+" | "+row["Msg_text"].ToString());
					}
				}
			}
			return retVal;
		}

		[DbmMethodAttr]
		public static string DatesNoZeros(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				//This check is not valid for Oracle, because each of the following fields are defined as non-null, and 0000-00-00 is not a valid Oracle date.
				return "";
			}
			//dynamically get every single date, datetime, and timestamp column from all tables in the db.
			string command = @"
				SELECT cols.TABLE_NAME, cols.COLUMN_NAME, cols.COLUMN_DEFAULT
				FROM information_schema.COLUMNS cols
				WHERE cols.DATA_TYPE IN ('datetime','date','timestamp')
				AND TABLE_SCHEMA = '"+DataConnection.GetDatabaseName()+"'";
			DataTable table = Db.GetTable(command);
			int countTotal=0;
			List<string> listInvalidColNames = new List<string>();
			List<string> listErrors=new List<string>();
			//for each of those columns
			List<string> listDbCommands = new List<string>();
			foreach(DataRow row in table.Rows) {
				string tableName = PIn.String(row["TABLE_NAME"].ToString());
				string columnName = PIn.String(row["COLUMN_NAME"].ToString());
				try {
					//get the primary key of that column's table
					command = "SHOW KEYS FROM "+tableName+" WHERE Key_name='PRIMARY'";
					DataTable tablePKs=Db.GetTable(command);
					if(tablePKs.Rows.Count==0) {
						continue;//Should never happen but there might be tables without a primary key.
					}
					string priKeyCol = tablePKs.Rows[0]["Column_name"].ToString();
					//check to see if there are any invalid dates
					command = "SELECT "+priKeyCol+" FROM "+tableName+" WHERE "+columnName+" = '0000-00-00'";//works for invalid dates, datetimes, and timestamps.
					DataTable tableInvalid = Db.GetTable(command);
					//and count them up.
					countTotal+=tableInvalid.Rows.Count;
					//if there are some that are invalid, then fix them by setting them to the default value.
					//default value is usually 0001-01-01 for most dates, but can be the current timestamp for DateTStamp columns.
					if(tableInvalid.Rows.Count > 0) {
						string priKeys = String.Join(",",tableInvalid.Rows.Cast<DataRow>().Select(x => x[0]).ToList());
						listInvalidColNames.Add(tableName+"."+columnName+": Keys ("+priKeys+")");
						listDbCommands.Add("UPDATE "+tableName+" SET "+columnName+" = DEFAULT WHERE "+priKeyCol+" IN ("+priKeys+")");
					}
				}
				catch(Exception ex) {
					//This could happen if we're trying to inspect a temp table that has since been deleted.
					listErrors.Add(tableName+"."+columnName+": "+ex.Message);
				}
			}
			string log="";
			if(listErrors.Count > 0) {
				log+=Lans.g("FormDatabaseMaintenance","Unable to check the following columns:")+"\r\n";
				log+=String.Join("\r\n",listErrors)+"\r\n";
			}
			switch(modeCur) {
				case DbmMode.Check:
					if(countTotal > 0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Number of dates invalid:")+" "+countTotal+"\r\n";
						log+=Lans.g("FormDatabaseMaintenance","The following rows have invalid dates:")+"\r\n";
						log+=String.Join("\r\n",listInvalidColNames);
					}
					break;
				case DbmMode.Fix:
					if(countTotal > 0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Number of dates fixed:")+" "+countTotal+"\r\n";
						log+=Lans.g("FormDatabaseMaintenance","The following rows had invalid dates:")+"\r\n";
						log+=String.Join("\r\n",listInvalidColNames);
						listDbCommands.ForEach(x => Db.NonQ(x));
					}
					break;
			}
			return log;
		}

		///<summary>Deprecated.</summary>
		public static string DecimalValues(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			//This specific fix is no longer needed, since we are using ROUND(EstBalance,2) in the aging calculation now.
			//However, it is still a problem in many columns that we will eventually need to deal with.
			//Maybe add this back when users can control which fixes they make.
			//One problem is the foreign users do not necessarily use 2 decimal places (Kuwait uses 3).
			////Holds columns to be checked. Strings are in pairs in the following order: table-name,column-name
			//string[] decimalCols=new string[] {
			//  "patient","EstBalance"
			//};
			//int decimalPlacessToRoundTo=8;
			//long numberFixed=0;
			//for(int i=0;i<decimalCols.Length;i+=2) {
			//  string tablename=decimalCols[i];
			//  string colname=decimalCols[i+1];
			//  string command="UPDATE "+tablename+" SET "+colname+"=ROUND("+colname+","+decimalPlacessToRoundTo
			//    +") WHERE "+colname+"!=ROUND("+colname+","+decimalPlacessToRoundTo+")";
			//  numberFixed+=Db.NonQ(command);
			//}
			//if(numberFixed>0 || verbose) {
			//  log+=Lans.g("FormDatabaseMaintenance","Decimal values fixed: ")+numberFixed.ToString()+"\r\n";
			//}
			return log;
		}

		///<summary>also checks patient.AddrNote</summary>
		[DbmMethodAttr]
		public static string SpecialCharactersInNotes(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			string log="";
			//this will run for fix or check, but will only fix if the special char button is used 
			//Fix code is in a dedicated button "Spec Char"
			string command="SELECT * FROM appointment WHERE (ProcDescript REGEXP '[^[:alnum:]^[:space:]^[:punct:]]+') OR (Note REGEXP '[^[:alnum:]^[:space:]^[:punct:]]+')";
			List<Appointment> apts=Crud.AppointmentCrud.SelectMany(command);
			List<char> specialCharsFound=new List<char>();
			int specialCharCount=0;
			int intC=0;
			foreach(Appointment apt in apts) {
				foreach(char c in apt.Note) {
					intC=(int)c;
					if((intC<126 && intC>31)//31 - 126 are all safe.
						|| intC==9     //"Horizontal Tabulation"
						|| intC==10    //Line Feed
						|| intC==13) { //carriage return
						continue;
					}
					specialCharCount++;
					if(specialCharsFound.Contains(c)) {
						continue;
					}
					specialCharsFound.Add(c);
				}
				foreach(char c in apt.ProcDescript) {//search every character in ProcDescript
					intC=(int)c;
					if((intC<126 && intC>31)//31 - 126 are all safe.
						|| intC==9     //"Horizontal Tabulation"
						|| intC==10    //Line Feed
						|| intC==13) { //carriage return
						continue;
					}
					specialCharCount++;
					if(specialCharsFound.Contains(c)) {
						continue;
					}
					specialCharsFound.Add(c);
				}
			}
			command="SELECT * FROM patient WHERE AddrNote REGEXP '[^[:alnum:]^[:space:]]+'";
			List<Patient> pats=OpenDentBusiness.Crud.PatientCrud.SelectMany(command);
			intC=0;
			foreach(Patient pat in pats) {
				foreach(char c in pat.AddrNote) {
					intC=(int)c;
					if((intC<126 && intC>31)//31 - 126 are all safe.
						|| intC==9      //"Horizontal Tabulation"
						|| intC==10     //Line Feed
						|| intC==13) {  //carriage return
						continue;
					}
					specialCharCount++;
					if(specialCharsFound.Contains(c)) {
						continue;
					}
					specialCharsFound.Add(c);
				}
			}
			foreach(char c in specialCharsFound) {
				log+=c.ToString()+" doesn't work.\r\n";
			}
			for(int i = 0;i<_listTableAndColumns.Count;i+=2) {
				string tableName=_listTableAndColumns[i];
				string columnName=_listTableAndColumns[i+1];
				command="SELECT COUNT(*) FROM "+tableName+" WHERE "+columnName+" LIKE '%"+POut.String("\0")+"%'";
				specialCharCount+=PIn.Int(Db.GetCount(command));
			}
			if(specialCharCount!=0 || verbose) {
				log+=specialCharCount.ToString()+" "+Lans.g("FormDatabaseMaintenance","total special characters found.  The Spec Char tool will remove these characters.")+"\r\n";
			}
			return log;
		}

		[DbmMethodAttr]
		public static string NotesWithTooMuchWhiteSpace(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			var listTablesAndColumns=new[]{
				new {tableName="appointment",columnName="Note",key="AptNum"},
				new {tableName="commlog",columnName="Note",key="CommlogNum"},
				new {tableName="procnote",columnName="Note",key="ProcNoteNum"},
				new {tableName="patient",columnName="FamFinUrgNote",key="PatNum"}
			};
			for(int i = 0;i<listTablesAndColumns.Length;i++) {
				string tableName=listTablesAndColumns[i].tableName;
				string colName=listTablesAndColumns[i].columnName;
				string priKey=listTablesAndColumns[i].key;
				#region Tabs
				string tooManyT=string.Join("",Enumerable.Repeat("\t",30));//Can't think of any good reason to have more than 30 tabs in a row
				switch(modeCur) {
					case DbmMode.Check:
						command="SELECT COUNT(*) FROM "+POut.String(tableName)+" WHERE "+POut.String(colName)+" LIKE '%"+POut.String(tooManyT)+"%'";
						int numFound=PIn.Int(Db.GetCount(command));
						if(numFound>0 || verbose) {
							log+=POut.String(tableName)+"."+POut.String(colName)+" "+Lans.g("FormDatabaseMaintenance","rows with too many tabs found:")
								+" "+numFound.ToString()+"\r\n";
						}
						break;
					case DbmMode.Fix:
						command="SELECT "+priKey+","+colName+" FROM "+POut.String(tableName)+" WHERE "+POut.String(colName)+" LIKE '%"+POut.String(tooManyT)+"%'";
						DataTable resultTable=Db.GetTable(command);
						long numFixed=0;
						if(resultTable.Rows.Count>0 || verbose) {
							for(int j = 0;j<resultTable.Rows.Count;j++) {
								long id=PIn.Long(resultTable.Rows[j][priKey].ToString());
								string oldNote=PIn.String(resultTable.Rows[j][colName].ToString());
								string newNote=Regex.Replace(oldNote,POut.String(tooManyT)+"[\t]*","");
								command="UPDATE "+POut.String(tableName)+" SET "+POut.String(colName)+"='"+POut.String(newNote)+"' WHERE "+POut.String(priKey)+"="+POut.Long(id);
								Db.NonQ(command);
								numFixed++;
							}
							log+=POut.String(tableName)+"."+POut.String(colName)+" "+Lans.g("FormDatabaseMaintenance","rows with too many tabs fixed:")
								+" "+numFixed.ToString()+"\r\n";
						}
						break;
				}
				#endregion
				#region Newlines
				string tooManyRN=string.Join("",Enumerable.Repeat("\r\n",30));// \r\n, \r\n, \r\n... as fast as you can!
				string tooManyN=string.Join("",Enumerable.Repeat("\n",30));// Sometimes we have had newlines encoded as \n
				switch(modeCur) {
					case DbmMode.Check:
						command="SELECT COUNT(*) FROM "+POut.String(tableName)+" "
							+"WHERE "+POut.String(colName)+" LIKE '%"+POut.String(tooManyRN)+"%' "
							+"OR "+POut.String(colName)+" LIKE '%"+POut.String(tooManyN)+"%'";
						int numFound=PIn.Int(Db.GetCount(command));
						if(numFound>0 || verbose) {
							log+=POut.String(tableName)+"."+POut.String(colName)+" "+Lans.g("FormDatabaseMaintenance","rows with too many newlines found:")
								+" "+numFound.ToString()+"\r\n";
						}
						break;
					case DbmMode.Fix:
						command="SELECT * FROM "+POut.String(tableName)+" "
							+"WHERE "+POut.String(colName)+" LIKE '%"+POut.String(tooManyRN)+"%' "
							+"OR "+POut.String(colName)+" LIKE '%"+POut.String(tooManyN)+"%'";
						DataTable resultTable=Db.GetTable(command);
						long numFixed=0;
						if(resultTable.Rows.Count>0 || verbose) {
							for(int j = 0;j<resultTable.Rows.Count;j++) {
								long id=PIn.Long(resultTable.Rows[j][priKey].ToString());
								string oldNote=PIn.String(resultTable.Rows[j][colName].ToString());
								string newNote=Regex.Replace(oldNote,POut.String(tooManyRN)+"[\r\n]*","\r\n");
								newNote=Regex.Replace(newNote,POut.String(tooManyN)+"[\n]*","\r\n");
								command="UPDATE "+POut.String(tableName)+" SET "+POut.String(colName)+"='"+POut.String(newNote)+"' WHERE "+POut.String(priKey)+"="+POut.Long(id);
								Db.NonQ(command);
								numFixed++;
							}
							log+=POut.String(tableName)+"."+POut.String(colName)+" "+Lans.g("FormDatabaseMaintenance","rows with too many newlines fixed:")
								+" "+numFixed.ToString()+"\r\n";
						}
						break;
				}
				#endregion
				#region Trailing Spaces
				string tooManySP=string.Join("",Enumerable.Repeat(@" ",300));//Spaces are very easy to draw so only remove ridiculous amounts of them.
				switch(modeCur) {
					case DbmMode.Check:
						command=@"SELECT COUNT(*) FROM "+POut.String(tableName)+" "
							+@"WHERE "+POut.String(colName)+" LIKE '%"+POut.String(tooManySP)+"%' ";//This is Sparta!
						int numFound=PIn.Int(Db.GetCount(command));
						if(numFound>0 || verbose) {
							log+=POut.String(tableName)+"."+POut.String(colName)+" "+Lans.g("FormDatabaseMaintenance","rows with too many trailing white spaces found:")
								+" "+numFound.ToString()+"\r\n";
						}
						break;
					case DbmMode.Fix:
						command="SELECT "+priKey+","+colName+" FROM "+POut.String(tableName)+" "
							+"WHERE "+POut.String(colName)+" LIKE '%"+POut.String(tooManySP)+"%' ";
						DataTable resultTable=Db.GetTable(command);
						long numFixed=0;
						if(resultTable.Rows.Count>0 || verbose) {
							for(int j = 0;j<resultTable.Rows.Count;j++) {
								long id=PIn.Long(resultTable.Rows[j][priKey].ToString());
								string oldNote=PIn.String(resultTable.Rows[j][colName].ToString());
								string newNote=Regex.Replace(oldNote,POut.String(tooManySP)+"[ ]*","");
								command="UPDATE "+POut.String(tableName)+" SET "+POut.String(colName)+"='"+POut.String(newNote)+"' WHERE "+POut.String(priKey)+"="+POut.Long(id);
								Db.NonQ(command);
								numFixed++;
							}
							log+=POut.String(tableName)+"."+POut.String(colName)+" "+Lans.g("FormDatabaseMaintenance","rows with too many trailing white spaces fixed:")
								+" "+numFixed.ToString()+"\r\n";
						}
						break;
				}
				#endregion
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown=true)]
		public static string TransactionsWithFutureDates(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			bool isFutureTransAllowed=PrefC.GetBool(PrefName.FutureTransDatesAllowed);
			bool isFuturePaymentsAllowed=PrefC.GetBool(PrefName.AccountAllowFutureDebits);
			if(isFutureTransAllowed) {//future dates are allowed so this DBM doesn't apply.
				return log;
			}
			string command=Ledgers.GetTransQueryString(DateTime.Today,"");
			DataTable table=Db.GetTable(command);
			DataTable flaggedTransactions=table.Clone();
			foreach(DataRow row in table.Rows) {
				if(PIn.String(row["TranType"].ToString())!="PPCharge" && PIn.String(row["TranType"].ToString()) !="PPCComplete"
					&& PIn.Date(row["TranDate"].ToString()) > DateTime.Today.Date) 
				{//transaction is date for the future
					//if either future dated payments or future transactions are allowed, don't count transactions dealing with payments.
					if(PIn.String(row["TranType"].ToString())=="PatPay" && isFuturePaymentsAllowed) {
						continue; //the are allowing future payments so skip this row. 
					}
					flaggedTransactions.Rows.Add(row.ItemArray);
				}
			}
			log+=Lans.g("FormDatabaseMaintenance","Future dated transactions found:")+" "+flaggedTransactions.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(flaggedTransactions.Rows.Count > 0 || verbose) {
						log+="\r\n"+Lans.g("FormDatabaseMaintenance","Manual fix needed. Double click to see a break down.");
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(flaggedTransactions.Rows.Count>0) {
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						foreach(DataRow transaction in flaggedTransactions.Rows) {
							string tranType="";
							switch(PIn.String(transaction["TranType"].ToString())) {
								case "Adj":
									tranType=Lans.g("FormDatabaseMaintenance","Adjustment");
									break;
								case "Claimproc":
									tranType=Lans.g("FormDatabaseMaintenance","Claim procedure");
									break;
								case "PatPay":
									tranType=Lans.g("FormDatabaseMaintenance","Patient payment");//paysplit?
									break;
								case "Proc":
									tranType=Lans.g("FormDatabaseMaintenance","Procedure");
									break;
							}
							log+="\r\n   "+tranType+" "+Lans.g("FormDatabaseMaintenance","found for patient #")+PIn.Long(transaction["PatNum"].ToString())+" "
								+Lans.g("FormDatabaseMaintenance", "dated")+" "+PIn.Date(transaction["TranDate"].ToString()).ToShortDateString()+" "
								+Lans.g("FormDatabaseMaintenance","amounting to")+" "+PIn.Double(transaction["TranAmount"].ToString()).ToString("c");
						}
						log+="\r\n"+Lans.g("FormDatabaseMaintenance","Go to patient accounts to find and manually correct future dates.");
					}
					break;
			}
			return log;
		}

		#endregion Methods That Affect All or Many Tables---------------------------------------------------------------------------------------------------
		#region Methods That Apply to Specific Tables-------------------------------------------------------------------------------------------------------

		#region Appointment-----------------------------------------------------------------------------------------------------------------------------

		[DbmMethodAttr(HasBreakDown = true)]
		public static string AppointmentCompleteWithTpAttached(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			string command="SELECT DISTINCT appointment.PatNum, "+DbHelper.Concat("LName","\", \"","FName")+" AS PatName, AptDateTime "
				+"FROM appointment "
				+"INNER JOIN patient ON patient.PatNum=appointment.PatNum "
				+"INNER JOIN procedurelog ON procedurelog.AptNum=appointment.AptNum "
				+"WHERE AptStatus="+POut.Int((int)ApptStatus.Complete)+" "
				+"AND procedurelog.ProcStatus="+POut.Int((int)ProcStat.TP)+" "
				+"ORDER BY PatName";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			//There is something to report OR the user has verbose mode on.
			string log=Lans.g("FormDatabaseMaintenance","Completed appointments with treatment planned procedures attached")+": "+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {//Only the fix should show the entire list of items.
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {//Running the fix and there are items to show.
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							log+="   "+table.Rows[i]["PatNum"].ToString()
								+"-"+table.Rows[i]["PatName"].ToString()
								+"  Appt Date:"+PIn.DateT(table.Rows[i]["AptDateTime"].ToString()).ToShortDateString();
							log+="\r\n";
						}
						log+=Lans.g("FormDatabaseMaintenance","   They need to be fixed manually.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string AppointmentsNoPattern(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command=@"SELECT AptNum FROM appointment WHERE Pattern=''";
			DataTable table=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Appointments found with zero length: ")+table.Rows.Count.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					if(table.Rows.Count>0) {
						//detach all procedures
						if(DataConnection.DBtype==DatabaseType.Oracle) {
							command="UPDATE procedurelog P SET P.AptNum=0 WHERE (SELECT A.Pattern FROM appointment A WHERE A.AptNum=P.AptNum AND ROWNUM<=1)=''";
						}
						else {
							command="UPDATE procedurelog P, appointment A SET P.AptNum=0 WHERE P.AptNum=A.AptNum AND A.Pattern=''";
						}
						Db.NonQ(command);
						if(DataConnection.DBtype==DatabaseType.Oracle) {
							command="UPDATE procedurelog P SET P.PlannedAptNum=0 WHERE (SELECT A.Pattern FROM appointment A WHERE A.AptNum=P.PlannedAptNum AND ROWNUM<=1)=''";
						}
						else {
							command="UPDATE procedurelog P, appointment A SET P.PlannedAptNum=0 WHERE P.PlannedAptNum=A.AptNum AND A.Pattern=''";
						}
						Db.NonQ(command);
						command="SELECT appointment.AptNum FROM appointment WHERE Pattern=''";
						DataTable tableAptNums=Db.GetTable(command);
						List<long> listAptNums=new List<long>();
						for(int i = 0;i<tableAptNums.Rows.Count;i++) {
							listAptNums.Add(PIn.Long(tableAptNums.Rows[i]["AptNum"].ToString()));
						}
						if(listAptNums.Count>0) {
							Appointments.ClearFkey(listAptNums);//Zero securitylog FKey column for rows to be deleted.
							command="SELECT * FROM appointment WHERE AptNum IN("+String.Join(",",listAptNums)+")";
							List<Appointment> listApts=Crud.AppointmentCrud.SelectMany(command);
							foreach(Appointment apt in listApts) {
								HistAppointments.CreateHistoryEntry(apt,HistAppointmentAction.Deleted);
							}
						}
						command="DELETE FROM appointment WHERE Pattern=''";
						Db.NonQ(command);
					}
					int numberFixed=table.Rows.Count;
					if(numberFixed!=0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Appointments deleted with zero length")+": "+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string AppointmentsNoDateOrProcs(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM appointment "
						+"WHERE AptStatus=1 "//scheduled 
						+"AND "+DbHelper.Year("AptDateTime")+"<1880 "//scheduled but no date 
						+"AND NOT EXISTS(SELECT * FROM procedurelog WHERE procedurelog.AptNum=appointment.AptNum)";//and no procs
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound!=0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Appointments found with no date and no procs")+": "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="SELECT appointment.AptNum FROM appointment "
						+"WHERE AptStatus="+POut.Int((int)ApptStatus.Scheduled)+" "
						+"AND "+DbHelper.Year("AptDateTime")+"<1880 "//scheduled but no date 
						+"AND NOT EXISTS(SELECT * FROM procedurelog WHERE procedurelog.AptNum=appointment.AptNum)";//and no procs
					DataTable tableAptNums=Db.GetTable(command);
					List<long> listAptNums=new List<long>();
					for(int i = 0;i<tableAptNums.Rows.Count;i++) {
						listAptNums.Add(PIn.Long(tableAptNums.Rows[i]["AptNum"].ToString()));
					}
					if(listAptNums.Count>0) {
						Appointments.ClearFkey(listAptNums);//Zero securitylog FKey column for rows to be deleted.
						command="SELECT * FROM appointment WHERE AptNum IN("+String.Join(",",listAptNums)+")";
						List<Appointment> listApts=Crud.AppointmentCrud.SelectMany(command);
						foreach(Appointment apt in listApts) {
							HistAppointments.CreateHistoryEntry(apt,HistAppointmentAction.Deleted);
						}
					}
					command="DELETE FROM appointment "
						+"WHERE AptStatus="+POut.Int((int)ApptStatus.Scheduled)+" "
						+"AND "+DbHelper.Year("AptDateTime")+"<1880 "//scheduled but no date 
						+"AND NOT EXISTS(SELECT * FROM procedurelog WHERE procedurelog.AptNum=appointment.AptNum)";//and no procs
					long numberFixed=Db.NonQ(command);
					if(numberFixed!=0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Appointments deleted due to no date and no procs")+": "+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>userNum can be 0, it's set before remoting role check and passed to the server if necessary.</summary>
		[DbmMethodAttr(HasUserNum = true)]
		public static string AppointmentsNoPatients(bool verbose,DbmMode modeCur,long userNum = 0) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				userNum=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur,userNum);
			}
			string command="SELECT Count(*) FROM appointment WHERE PatNum NOT IN (SELECT PatNum FROM patient)";
			int count=PIn.Int(Db.GetCount(command));
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Appointments found with invalid patients: ")+count.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					//Fix is safe because we are not deleting data, we are just attaching abandoned appointments to a dummy patient.
					long patientsAdded=0;
					if(count!=0) {
						command="SELECT PatNum FROM appointment WHERE PatNum NOT IN (SELECT PatNum FROM patient)";
						List<long> patNums=Db.GetListLong(command).Distinct().ToList();
						if(patNums.Contains(0)) {//appointments with no patient.
							Patient tempPat=new Patient() {
								FName="MISSING",
								LName="PATIENT",
								AddrNote="DBM created this patient and assigned patientless appointments to it on "+DateTime.Now.ToShortDateString(),
								Birthdate=DateTime.MinValue,
								BillingType=PrefC.GetLong(PrefName.PracticeDefaultBillType),
								PatStatus=PatientStatus.Inactive,
								PriProv=PrefC.GetLong(PrefName.PracticeDefaultProv)
							};
							tempPat.SecUserNumEntry=userNum;
							Patients.Insert(tempPat,false);
							SecurityLogs.MakeLogEntry(Permissions.PatientCreate,tempPat.PatNum,"Recreated from DBM fix for AppointmentsNoPatients.",LogSources.DBM);
							Patient oldPat=tempPat.Copy();
							tempPat.Guarantor=tempPat.PatNum;
							Patients.Update(tempPat,oldPat);//update guarantor
							command="UPDATE appointment SET PatNum="+POut.Long(tempPat.PatNum)+" WHERE PatNum=0";
							Db.NonQ(command);
							patientsAdded++;
							patNums.Remove(0);
						}
						foreach(long patnum in patNums) {//appointments with missing patient
							Patients.Insert(new Patient() {
								PatNum=patnum,
								Guarantor=patnum,
								FName="MISSING",
								LName="PATIENT",
								AddrNote="DBM re-created this patient because an appointment existed for the patient on "+DateTime.Now.ToShortDateString(),
								Birthdate=DateTime.MinValue,
								BillingType=PrefC.GetLong(PrefName.PracticeDefaultBillType),
								PatStatus=PatientStatus.Inactive,
								PriProv=PrefC.GetLong(PrefName.PracticeDefaultProv),
								SecUserNumEntry=userNum
							},true);
							SecurityLogs.MakeLogEntry(Permissions.PatientCreate,patnum,"Recreated from DBM fix for AppointmentsNoPatients.",LogSources.DBM);
							patientsAdded++;
						}
					}
					if(count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Appointments fixed with invalid patients: ")+count.ToString()+"\r\n";
						log+=Lans.g("FormDatabaseMaintenance","Missing patients added: ")+patientsAdded.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string AppointmentPlannedNoPlannedApt(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM appointment WHERE AptStatus=6 AND AptNum NOT IN (SELECT AptNum FROM plannedappt)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound!=0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Appointments with status set to planned without Planned Appointment: ")+numFound.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="SELECT * FROM appointment WHERE AptStatus=6 AND AptNum NOT IN (SELECT AptNum FROM plannedappt)";
					DataTable appts=Db.GetTable(command);
					if(appts.Rows.Count > 0 || verbose) {
						PlannedAppt plannedAppt;
						for(int i = 0;i<appts.Rows.Count;i++) {
							plannedAppt=new PlannedAppt();
							plannedAppt.PatNum=PIn.Long(appts.Rows[i]["PatNum"].ToString());
							plannedAppt.AptNum=PIn.Long(appts.Rows[i]["AptNum"].ToString());
							plannedAppt.ItemOrder=1;
							PlannedAppts.Insert(plannedAppt);
						}
						log+=Lans.g("FormDatabaseMaintenance","Planned Appointments created for Appointments with status set to planned and no Planned Appointment: ")+appts.Rows.Count+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string AppointmentScheduledWithCompletedProcs(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT DISTINCT appointment.PatNum, "+DbHelper.Concat("LName","\', \'","FName")+" AS PatName, appointment.AptDateTime "
				+"FROM appointment "
				+"INNER JOIN patient ON patient.PatNum=appointment.PatNum "
				+"INNER JOIN procedurelog ON procedurelog.AptNum=appointment.AptNum "
				+"WHERE AptStatus = "+POut.Int((int)ApptStatus.Scheduled)+" "
				+"AND procedurelog.ProcStatus="+POut.Int((int)ProcStat.C)+" "
				+"ORDER BY PatName";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			//There is something to report OR the user has verbose mode on.
			string log=Lans.g("FormDatabaseMaintenance","Scheduled appointments with completed procedures attached")+": "+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {//Only the fix should show the entire list of items.
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {//Running the fix and there are items to show.
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							log+="   "+table.Rows[i]["PatNum"].ToString()
								+"-"+table.Rows[i]["PatName"].ToString()
								+"  Appt Date:"+PIn.DateT(table.Rows[i]["AptDateTime"].ToString()).ToShortDateString();
							log+="\r\n";
						}
						log+=Lans.g("FormDatabaseMaintenance","   They need to be fixed manually.")+"\r\n";
					}
					break;
			}
			return log;
		}

		#endregion Appointment--------------------------------------------------------------------------------------------------------------------------
		#region AuditTrail, AutoCode, Automation--------------------------------------------------------------------------------------------------------

		///<summary>For appointments that have more than one AppointmentCreate audit entry, deletes all but the newest.</summary>
		[DbmMethodAttr]
		public static string AuditTrailDeleteDuplicateApptCreate(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			string command="SELECT securitylog.SecurityLogNum "
				+"FROM securitylog "
				+"INNER JOIN ("
					+"SELECT PatNum,FKey,MAX(LogDateTime) LogDateTime "
					+"FROM securitylog "
					+"WHERE PermType="+POut.Int((int)Permissions.AppointmentCreate)+" "
					+"AND FKey>0 "
					+"GROUP BY PatNum,FKey "
					+"HAVING COUNT(*)>1"
				+") sl ON sl.PatNum=securitylog.PatNum "
				+"AND sl.FKey=securitylog.FKey "
				+"AND sl.LogDateTime!=securitylog.LogDateTime "
				+"AND securitylog.PermType="+POut.Int((int)Permissions.AppointmentCreate)+" "
				+"GROUP BY securitylog.PatNum,securitylog.FKey";
			List<long> listDupApptCreates=Db.GetListLong(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					int numFound=listDupApptCreates.Count;
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Appointments found with duplicate Appt Create audit trail entries:")+" "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					if(listDupApptCreates.Count>0) {
						command="DELETE FROM securitylog WHERE SecurityLogNum IN("+string.Join(",",listDupApptCreates)+")";
						long numberFixed=Db.NonQ(command);
						if(numberFixed>0 || verbose) {
							log+=Lans.g("FormDatabaseMaintenance","Audit trail entries deleted due to duplicate Appt Create entries:")+" "
								+numberFixed.ToString()+"\r\n";
						}
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string AutoCodeItemsWithNoAutoCode(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			DataTable table;
			switch(modeCur) {
				case DbmMode.Check:
					command=@"SELECT DISTINCT AutoCodeNum FROM autocodeitem WHERE NOT EXISTS(
						SELECT * FROM autocode WHERE autocodeitem.AutoCodeNum=autocode.AutoCodeNum)";
					table=Db.GetTable(command);
					int numFound=table.Rows.Count;
					if(numFound!=0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Auto codes missing due to invalid auto code items")+": "+numFound.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command=@"SELECT DISTINCT AutoCodeNum FROM autocodeitem WHERE NOT EXISTS(
						SELECT * FROM autocode WHERE autocodeitem.AutoCodeNum=autocode.AutoCodeNum)";
					table=Db.GetTable(command);
					int numFixed=table.Rows.Count;
					for(int i = 0;i<table.Rows.Count;i++) {
						AutoCode autoCode=new AutoCode();
						autoCode.AutoCodeNum=PIn.Long(table.Rows[i]["AutoCodeNum"].ToString());
						autoCode.Description="UNKNOWN";
						Crud.AutoCodeCrud.Insert(autoCode,true);
					}
					if(numFixed>0) {
						Signalods.SetInvalid(InvalidType.AutoCodes);
					}
					if(numFixed!=0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Auto codes created due to invalid auto code items")+": "+numFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string AutoCodesDeleteWithNoItems(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command=@"SELECT COUNT(*) FROM autocode WHERE NOT EXISTS(
						SELECT * FROM autocodeitem WHERE autocodeitem.AutoCodeNum=autocode.AutoCodeNum)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound!=0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Autocodes found with no items: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command=@"DELETE FROM autocode WHERE NOT EXISTS(
						SELECT * FROM autocodeitem WHERE autocodeitem.AutoCodeNum=autocode.AutoCodeNum)";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0) {
						Signalods.SetInvalid(InvalidType.AutoCodes);
					}
					if(numberFixed!=0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Autocodes deleted due to no items: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string AutomationTriggersWithNoSheetDefs(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command=@"SELECT COUNT(*) FROM automation WHERE automation.SheetDefNum!=0 AND NOT EXISTS(
					SELECT SheetDefNum FROM sheetdef WHERE automation.SheetDefNum=sheetdef.SheetDefNum)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound!=0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Automation triggers found with no sheet defs: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command=@"DELETE FROM automation WHERE automation.SheetDefNum!=0 AND NOT EXISTS(
					SELECT SheetDefNum FROM sheetdef WHERE automation.SheetDefNum=sheetdef.SheetDefNum)";
					long numberFixed=Db.NonQ(command);
					if(numberFixed!=0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Automation triggers deleted due to no sheet defs: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		#endregion AuditTrail, AutoCode, Automation-----------------------------------------------------------------------------------------------------
		#region Benefit, BillingType--------------------------------------------------------------------------------------------------------------------

		///<summary>Remove duplicates where all benefit columns match except for BenefitNum.</summary>
		[DbmMethodAttr]
		public static string BenefitsWithExactDuplicatesForInsPlan(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			DataTable table;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(DISTINCT ben2.BenefitNum) DuplicateCount "
						+"FROM benefit ben "
						+"INNER JOIN benefit ben2 ON ben.PlanNum=ben2.PlanNum "
						+"AND ben.PatPlanNum=ben2.PatPlanNum "
						+"AND ben.CovCatNum=ben2.CovCatNum "
						+"AND ben.BenefitType=ben2.BenefitType "
						+"AND ben.Percent=ben2.Percent "
						+"AND ben.MonetaryAmt=ben2.MonetaryAmt "
						+"AND ben.TimePeriod=ben2.TimePeriod "
						+"AND ben.QuantityQualifier=ben2.QuantityQualifier "
						+"AND ben.Quantity=ben2.Quantity "
						+"AND ben.CodeNum=ben2.CodeNum "
						+"AND ben.CoverageLevel=ben2.CoverageLevel "
						+"AND ben.BenefitNum<ben2.BenefitNum";  //This ensures that the benefit with the lowest primary key in the match will not be counted.
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Duplicate benefits found")+": "+numFound.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="SELECT DISTINCT ben2.BenefitNum "
						+"FROM benefit ben "
						+"INNER JOIN benefit ben2 ON ben.PlanNum=ben2.PlanNum "
						+"AND ben.PatPlanNum=ben2.PatPlanNum "
						+"AND ben.CovCatNum=ben2.CovCatNum "
						+"AND ben.BenefitType=ben2.BenefitType "
						+"AND ben.Percent=ben2.Percent "
						+"AND ben.MonetaryAmt=ben2.MonetaryAmt "
						+"AND ben.TimePeriod=ben2.TimePeriod "
						+"AND ben.QuantityQualifier=ben2.QuantityQualifier "
						+"AND ben.Quantity=ben2.Quantity "
						+"AND ben.CodeNum=ben2.CodeNum "
						+"AND ben.CoverageLevel=ben2.CoverageLevel "
						+"AND ben.BenefitNum<ben2.BenefitNum";  //This ensures that the benefit with the lowest primary key in the match will not be deleted.
					table=Db.GetTable(command);
					List<long> listBenefitNums=new List<long>();
					if(table.Rows.Count>0 || verbose) {
						for(int i = 0;i<table.Rows.Count;i++) {
							listBenefitNums.Add(PIn.Long(table.Rows[i]["BenefitNum"].ToString()));
						}
						long numFixed=0;
						if(listBenefitNums.Count>0) {
							command="DELETE FROM benefit WHERE BenefitNum IN ("+string.Join(",",listBenefitNums)+")";
							numFixed=PIn.Long(Db.NonQ(command).ToString());
						}
						log+=Lans.g("FormDatabaseMaintenance","Duplicate benefits deleted")+": "+numFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>Identify duplicates where all benefit columns match except for BenefitNum, Percent, and MonetaryAmt.</summary>
		[DbmMethodAttr(HasBreakDown = true)]
		public static string BenefitsWithPartialDuplicatesForInsPlan(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT DISTINCT employer.EmpName,carrier.CarrierName,carrier.Phone,carrier.Address,carrier.City,carrier.State,carrier.Zip, "
				+"insplan.GroupNum,insplan.GroupName, carrier.NoSendElect,carrier.ElectID, "
				+"(SELECT COUNT(DISTINCT Subscriber) FROM inssub WHERE insplan.PlanNum=inssub.PlanNum) subscribers, insplan.PlanNum "
				+"FROM benefit ben "
				+"INNER JOIN benefit ben2 ON ben.PlanNum=ben2.PlanNum "
					+"AND ben.PatPlanNum=ben2.PatPlanNum "
					+"AND ben.CovCatNum=ben2.CovCatNum "
					+"AND ben.BenefitType=ben2.BenefitType "
					+"AND (ben.Percent!=ben2.Percent OR ben.MonetaryAmt!=ben2.MonetaryAmt) "  //Only benefits with Percent or MonetaryAmts that don't match.
					+"AND ben.TimePeriod=ben2.TimePeriod "
					+"AND ben.QuantityQualifier=ben2.QuantityQualifier "
					+"AND ben.Quantity=ben2.Quantity "
					+"AND ben.CodeNum=ben2.CodeNum "
					+"AND ben.CoverageLevel=ben2.CoverageLevel "
					+"AND ben.BenefitNum<ben2.BenefitNum "
				+"INNER JOIN insplan ON insplan.PlanNum=ben.PlanNum "
				+"LEFT JOIN carrier ON carrier.CarrierNum=insplan.CarrierNum "
				+"LEFT JOIN employer ON employer.EmployerNum=insplan.EmployerNum";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			//There is something to report OR the user has verbose mode on.
			string log=Lans.g("FormDatabaseMaintenance","Insurance plans with partial duplicate benefits found")+": "+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {//Only the fix should show the entire list of items.
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {//Running the fix and there are items to show.
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							//Show the same columns as the Insurance Plans list.  We don't have an easy identifier for insurance plans, and we do not want to 
							//  give a patient example since there is a good chance that in fixing the benefits the user will just split that plan off and will 
							//  not solve the issue.
							log+="   Employer: "+table.Rows[i]["EmpName"].ToString();
							log+=",  Carrier: "+table.Rows[i]["CarrierName"].ToString();
							log+=",  Phone: "+table.Rows[i]["Phone"].ToString();
							log+=",  Address: "+table.Rows[i]["Address"].ToString();
							log+=",  City: "+table.Rows[i]["City"].ToString();
							log+=",  ST: "+table.Rows[i]["State"].ToString();
							log+=",  Zip: "+table.Rows[i]["Zip"].ToString();
							log+=",  Group#: "+table.Rows[i]["GroupNum"].ToString();
							log+=",  GroupName: "+table.Rows[i]["GroupName"].ToString();
							log+=",  NoE: ";
							if(table.Rows[i]["NoSendElect"].ToString()=="1") {
								log+="X";
							}
							else {
								log+=" ";
							}
							log+=",  ElectID: "+table.Rows[i]["ElectID"].ToString();
							log+=",  Subs: "+table.Rows[i]["subscribers"].ToString();
							log+="\r\n";
						}
						log+=Lans.g("FormDatabaseMaintenance","   They need to be fixed manually.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string BillingTypesInvalid(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT ValueString FROM preference WHERE PrefName='PracticeDefaultBillType'";
			long billingType=PIn.Long(Db.GetScalar(command));
			command="SELECT COUNT(*) FROM definition WHERE Category=4 AND definition.DefNum="+billingType;
			int prefExists=PIn.Int(Db.GetCount(command));
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(prefExists!=1) {
						log+=Lans.g("FormDatabaseMaintenance","No default billing type set.")+"\r\n";
					}
					else if(verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Default practice billing type verified.")+"\r\n";
					}
					//Check for any patients with invalid billingtype.
					command="SELECT COUNT(*) FROM patient WHERE NOT EXISTS(SELECT * FROM definition WHERE Category=4 AND patient.BillingType=definition.DefNum)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound!=0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Patients with invalid billing type: ")+numFound.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					//Fix for default billingtype not being set.
					if(prefExists!=1) {//invalid billing type
						command="SELECT DefNum FROM definition WHERE Category=4 AND IsHidden=0 ORDER BY ItemOrder";
						DataTable table=Db.GetTable(command);
						if(table.Rows.Count==0) {//if all billing types are hidden
							command="SELECT DefNum FROM definition WHERE Category=4 ORDER BY ItemOrder";
							table=Db.GetTable(command);
						}
						command="UPDATE preference SET ValueString='"+table.Rows[0][0].ToString()+"' WHERE PrefName='PracticeDefaultBillType'";
						Db.NonQ(command);
						log+=Lans.g("FormDatabaseMaintenance","Default billing type preference was set due to being invalid.")+"\r\n";
						Prefs.RefreshCache();//for the next line.
					}
					//Fix for patients with invalid billingtype.
					command="UPDATE patient SET patient.BillingType="+POut.Long(PrefC.GetLong(PrefName.PracticeDefaultBillType));
					command+=" WHERE NOT EXISTS(SELECT * FROM definition WHERE Category=4 AND patient.BillingType=definition.DefNum)";
					long numberFixed=Db.NonQ(command);
					if(numberFixed!=0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Patients billing type set to default due to being invalid: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		#endregion Benefit, BillingType-----------------------------------------------------------------------------------------------------------------
		#region Carrier, Claim--------------------------------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string CanadaCarriersCdaMissingInfo(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(!CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				return Lans.g("FormDatabaseMaintenance","Skipped. Local computer region must be set to Canada to run.");
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			string command="SELECT CanadianNetworkNum FROM canadiannetwork WHERE Abbrev='TELUS B' LIMIT 1";
			long canadianNetworkNumTelusB=PIn.Long(Db.GetScalar(command));
			command="SELECT CanadianNetworkNum FROM canadiannetwork WHERE Abbrev='CSI' LIMIT 1";
			//CSI is now known as "instream"
			long canadianNetworkNumCSI=PIn.Long(Db.GetScalar(command));
			command="SELECT CanadianNetworkNum FROM canadiannetwork WHERE Abbrev='CDCS' LIMIT 1";
			long canadianNetworkNumCDCS=PIn.Long(Db.GetScalar(command));
			command="SELECT CanadianNetworkNum FROM canadiannetwork WHERE Abbrev='TELUS A' LIMIT 1";
			long canadianNetworkNumTelusA=PIn.Long(Db.GetScalar(command));
			command="SELECT CanadianNetworkNum FROM canadiannetwork WHERE Abbrev='MBC' LIMIT 1";
			long canadianNetworkNumMBC=PIn.Long(Db.GetScalar(command));
			command="SELECT CanadianNetworkNum FROM canadiannetwork WHERE Abbrev='ABC' LIMIT 1";
			long canadianNetworkNumABC=PIn.Long(Db.GetScalar(command));
			CanSupTransTypes claimTypes=CanSupTransTypes.ClaimAckEmbedded_11e|CanSupTransTypes.ClaimEobEmbedded_21e;//Claim 01, claim ack 11, and claim eob 21 are implied.
			CanSupTransTypes reversalTypes=CanSupTransTypes.ClaimReversal_02|CanSupTransTypes.ClaimReversalResponse_12;
			CanSupTransTypes predeterminationTypes=CanSupTransTypes.PredeterminationAck_13|CanSupTransTypes.PredeterminationAckEmbedded_13e|CanSupTransTypes.PredeterminationMultiPage_03|CanSupTransTypes.PredeterminationSinglePage_03;
			CanSupTransTypes rotTypes=CanSupTransTypes.RequestForOutstandingTrans_04;
			CanSupTransTypes cobTypes=CanSupTransTypes.CobClaimTransaction_07;
			CanSupTransTypes eligibilityTypes=CanSupTransTypes.EligibilityTransaction_08|CanSupTransTypes.EligibilityResponse_18;
			CanSupTransTypes rprTypes=CanSupTransTypes.RequestForPaymentReconciliation_06;
			//Column order: ElectID,CanadianEncryptionMethod,CDAnetVersion,CanadianSupportedTypes,CanadianNetworkNum
			object[] carrierInfo=new object[] {
				//accerta
				"311140",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumCSI,
				//adsc
				"000105",1,"04",claimTypes|reversalTypes|predeterminationTypes|rotTypes|cobTypes|eligibilityTypes,canadianNetworkNumCSI,
				//aga
				"610226",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//appq
				"628112",1,"02",claimTypes|reversalTypes|predeterminationTypes|cobTypes,canadianNetworkNumTelusB,
				//alberta blue cross. Usually sent through ClaimStream instead of ITRANS.
				"000090",1,"04",claimTypes|reversalTypes|predeterminationTypes|rotTypes|cobTypes,canadianNetworkNumABC,
				//assumption life
				"610191",1,"04",claimTypes,canadianNetworkNumTelusB,
				//autoben
				"628151",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//benecaid health benefit solutions
				"610708",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//benefits trust
				"610146",1,"02",claimTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//beneplan - Old carrier that is no longer listed one iTrans supported list.
				"400008",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//boilermakers' national benefit plan - Old carrier that is no longer listed one iTrans supported list.
				"000116",1,"04",claimTypes|predeterminationTypes,canadianNetworkNumCSI,
				//canadian benefit providers
				"610202",1,"04",claimTypes|reversalTypes|predeterminationTypes|cobTypes,canadianNetworkNumTelusB,
				//capitale
				"600502",1,"04",claimTypes|reversalTypes|predeterminationTypes|rotTypes|cobTypes,canadianNetworkNumTelusB,
				//carpenters and allied workers local
				"000117",1,"04",claimTypes|predeterminationTypes,canadianNetworkNumCSI,
				//cdcs
				"610129",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumCDCS,
				//claimsecure
				"610099",1,"04",claimTypes|eligibilityTypes,canadianNetworkNumTelusB,
				//co-operators
				"606258",1,"04",claimTypes|reversalTypes|predeterminationTypes|cobTypes,canadianNetworkNumTelusB,
				//Commision de la construction du Quebec
				"000036",1,"02",claimTypes|reversalTypes,canadianNetworkNumTelusB,
				//coughlin & associates
				"610105",1,"04",claimTypes|reversalTypes|predeterminationTypes|rotTypes,canadianNetworkNumTelusB,
				//cowan wright beauchamps
				"610153",1,"04",claimTypes|reversalTypes,canadianNetworkNumTelusB,
				//desjardins financial security
				"000051",1,"04",claimTypes|reversalTypes|rotTypes,canadianNetworkNumTelusB,
				//empire life insurance company
				"000033",1,"04",claimTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//equitable life
				"000029",1,"02",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusA,
				//esorse corporation
				"610650",1,"04",claimTypes|reversalTypes|predeterminationTypes|rprTypes|cobTypes,canadianNetworkNumTelusB,
				//fas administrators
				"610614",1,"04",claimTypes|reversalTypes,canadianNetworkNumTelusB,
				//GMS Insurance Inc. (GMS) (ESC)
				"610218",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumCSI,
				//great west life assurance company
				"000011",1,"02",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusA,
				//green sheild canada
				"000102",1,"04",claimTypes|reversalTypes|predeterminationTypes|cobTypes,canadianNetworkNumTelusB,
				//group medical services
				"610217",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//groupe premier medical
				"610266",1,"04",claimTypes|reversalTypes,canadianNetworkNumTelusB,
				//grouphealth benefit solutions
				"000125",1,"04",claimTypes|reversalTypes|predeterminationTypes|cobTypes,canadianNetworkNumTelusB,
				//groupsource - Old carrier that is no longer listed one iTrans supported list.
				"605064",1,"04",claimTypes|reversalTypes|eligibilityTypes,canadianNetworkNumCSI,
				//Humania Assurance Inc (formerly La Survivance) (ESC)
				"000080",1,"04",claimTypes,canadianNetworkNumTelusB,
				//industrial alliance
				"000060",1,"02",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusA,
				//industrial alliance pacific insurance and financial
				"000024",1,"02",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusA,
				//johnson inc.
				"627265",1,"04",claimTypes,canadianNetworkNumTelusB,
				//johnston group
				"627223",1,"02",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumCSI,
				//lee-power & associates
				"627585",1,"02",claimTypes,canadianNetworkNumTelusA,
				//local 1030 health benefity plan
				"000118",1,"04",claimTypes|predeterminationTypes,canadianNetworkNumCSI,
				//manion wilkins
				"610158",1,"04",claimTypes|reversalTypes,canadianNetworkNumTelusB,
				//manitoba blue cross
				"000094",1,"04",claimTypes|reversalTypes|predeterminationTypes|rotTypes,canadianNetworkNumMBC,
				//manitoba cleft palate program
				"000114",1,"04",claimTypes|predeterminationTypes|rotTypes,canadianNetworkNumCSI,
				//manitoba health
				"000113",1,"04",claimTypes|rotTypes,canadianNetworkNumCSI,
				//manufacturers life insurance company
				"000034",1,"02",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//manulife financial
				"610059",1,"02",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//maritime life assurance company
				"311113",1,"02",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//maritime pro
				"610070",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//mdm
				"601052",1,"02",claimTypes|reversalTypes|predeterminationTypes|eligibilityTypes,canadianNetworkNumTelusB,
				//medavie blue cross
				"610047",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//nexgenrx
				"610634",1,"04",claimTypes|reversalTypes|predeterminationTypes|cobTypes,canadianNetworkNumTelusB,
				//Non-Insured Health Benefits (NIHB) Program (ESC)
				"610124",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//nova scotia community services
				"000109",1,"04",claimTypes|reversalTypes|predeterminationTypes|rotTypes|cobTypes|eligibilityTypes,canadianNetworkNumCSI,
				//nova scotia medical services insurance
				"000108",1,"04",claimTypes|reversalTypes|predeterminationTypes|rotTypes|cobTypes|eligibilityTypes,canadianNetworkNumCSI,
				//nunatsiavut government department of health
				"610172",1,"04",claimTypes|reversalTypes,canadianNetworkNumCSI,
				//ontario ironworkers
				"000123",1,"04",claimTypes|predeterminationTypes|cobTypes,canadianNetworkNumCSI,
				//pacific blue cross
				"000064",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumCSI,
				//pbas
				"610256",1,"04",claimTypes|predeterminationTypes|rotTypes,canadianNetworkNumCSI,
				//quickcard
				"000103",1,"04",claimTypes|reversalTypes|predeterminationTypes|rotTypes|cobTypes|eligibilityTypes,canadianNetworkNumCSI,
				//rbc insurance
				"000124",1,"04",claimTypes|reversalTypes|predeterminationTypes|rotTypes,canadianNetworkNumTelusB,
				//rwam insurance
				"610616",1,"04",claimTypes|reversalTypes,canadianNetworkNumTelusB,
				//saskatchewan blue cross
				"000096",1,"04",claimTypes,canadianNetworkNumTelusB,
				//Segic (BATCH) benefits
				"610360",1,"04",claimTypes|reversalTypes|predeterminationTypes|cobTypes,canadianNetworkNumTelusB,
				//ses benefits
				"610196",1,"04",claimTypes|reversalTypes,canadianNetworkNumTelusB,
				//sheet metal workers local 30 benefit plan - Old carrier that is no longer listed one iTrans supported list.
				"000119",1,"04",claimTypes|predeterminationTypes,canadianNetworkNumCSI,
				//ssq societe d'assurance-vie inc.
				"000079",1,"04",claimTypes,canadianNetworkNumTelusB,
				//standard life assurance company
				"000020",1,"04",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
				//sun life of canada
				"000016",1,"04",claimTypes|reversalTypes|predeterminationTypes|rotTypes|cobTypes,canadianNetworkNumTelusB,
				//syndicat des fonctionnaires municipaux mtl
				"610677",1,"04",claimTypes|reversalTypes,canadianNetworkNumTelusB,
				//the building union of canada health beneift plan
				"000120",1,"04",claimTypes|predeterminationTypes,canadianNetworkNumCSI,
				//U-L Mutual (ESC)
				"610643",1,"04",claimTypes|reversalTypes,canadianNetworkNumTelusB,
				//u.a. local 46 dental plan
				"000115",1,"04",claimTypes|predeterminationTypes,canadianNetworkNumCSI,
				//u.a. local 787 health trust fund dental plan - Old carrier that is no longer listed one iTrans supported list.
				"000110",1,"04",claimTypes|predeterminationTypes,canadianNetworkNumCSI,
				//wawanesa
				"311109",1,"02",claimTypes|reversalTypes|predeterminationTypes,canadianNetworkNumTelusB,
			};
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					int numFound=0;
					for(int i = 0;i<carrierInfo.Length;i+=5) {
						command="SELECT COUNT(*) "+
							"FROM carrier "+
							"WHERE IsCDA<>0 AND ElectID='"+POut.String((string)carrierInfo[i])+"' AND "+
							"(CanadianEncryptionMethod<>"+POut.Int((int)carrierInfo[i+1])+" OR "+
							"CDAnetVersion<>'"+POut.String((string)carrierInfo[i+2])+"' OR "+
							"CanadianSupportedTypes<>"+POut.Int((int)carrierInfo[i+3])+" OR "+
							"CanadianNetworkNum<>"+POut.Long((long)carrierInfo[i+4])+")";
						numFound+=PIn.Int(Db.GetCount(command));
					}
					if(numFound!=0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","CDANet carriers with incorrect network, encryption method or version, based on carrier identification number: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					long numberFixed=0;
					for(int i = 0;i<carrierInfo.Length;i+=5) {
						command="UPDATE carrier SET "+
							"CanadianEncryptionMethod="+POut.Int((int)carrierInfo[i+1])+","+
							"CDAnetVersion='"+POut.String((string)carrierInfo[i+2])+"',"+
							"CanadianSupportedTypes="+POut.Int((int)carrierInfo[i+3])+","+
							"CanadianNetworkNum="+POut.Long((long)carrierInfo[i+4])+" "+
							"WHERE IsCDA<>0 AND ElectID='"+POut.String((string)carrierInfo[i])+"' AND "+
							"(CanadianEncryptionMethod<>"+POut.Int((int)carrierInfo[i+1])+" OR "+
							"CDAnetVersion<>'"+POut.String((string)carrierInfo[i+2])+"' OR "+
							"CanadianSupportedTypes<>"+POut.Int((int)carrierInfo[i+3])+" OR "+
							"CanadianNetworkNum<>"+POut.Long((long)carrierInfo[i+4])+")";
						numberFixed+=Db.NonQ(command);
					}
					if(numberFixed!=0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","CDANet carriers fixed based on carrier identification number: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimDeleteWithNoClaimProcs(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command=@"SELECT COUNT(*) FROM claim WHERE NOT EXISTS(
						SELECT * FROM claimproc WHERE claim.ClaimNum=claimproc.ClaimNum)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound!=0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claims found with no procedures")+": "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="SELECT claim.ClaimNum FROM claim WHERE NOT EXISTS( "
						+"SELECT * FROM claimproc WHERE claim.ClaimNum=claimproc.ClaimNum)";
					DataTable tableClaimNums=Db.GetTable(command);
					List<long> listClaimNums=new List<long>();
					for(int i = 0;i<tableClaimNums.Rows.Count;i++) {
						listClaimNums.Add(PIn.Long(tableClaimNums.Rows[i]["ClaimNum"].ToString()));
					}
					if(listClaimNums.Count>0) {
						Claims.ClearFkey(listClaimNums);//Zero securitylog FKey column for rows to be deleted.
					}
					//Orphaned claims do not show in the account module (tested) so we need to delete them because no other way.
					command=@"DELETE FROM claim WHERE NOT EXISTS(
						SELECT * FROM claimproc WHERE claim.ClaimNum=claimproc.ClaimNum)";
					long numberFixed=Db.NonQ(command);
					if(numberFixed!=0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claims deleted due to no procedures")+": "+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>No need to pass in userNum, it's set before remoting role check and passed to the server if necessary.</summary>
		[DbmMethodAttr(HasBreakDown = true,HasUserNum = true)]
		public static string ClaimWithInvalidInsSubNum(bool verbose,DbmMode modeCur,long userNum = 0) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				userNum=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur,userNum);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					//Claim.PlanNum is 0 and inssub.plannum is 0 or does not exist.
					command="SELECT count(*) FROM claim LEFT JOIN inssub ON claim.InsSubNum=inssub.InsSubNum "
						+"WHERE claim.PlanNum=0 AND ( inssub.PlanNum=0 OR inssub.PlanNum IS NULL )";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound!=0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claims with invalid InsSubNum: ")+numFound+"\r\n";
					}
					//situation where PlanNum and InsSubNum are both invalid and not zero is handled in InsSubNumMismatchPlanNum
					break;
				case DbmMode.Breakdown:
					command="SELECT claim.ClaimNum, claim.PatNum, claim.DateService FROM claim LEFT JOIN inssub ON claim.InsSubNum=inssub.InsSubNum "
						+"WHERE claim.PlanNum=0 AND ( inssub.PlanNum=0 OR inssub.PlanNum IS NULL )";
					var dictPatClaims=Db.GetTable(command).Select().Select(x=> new {
						ClaimNum=PIn.Long(x["ClaimNum"].ToString()),
						PatNum=PIn.Long(x["PatNum"].ToString()),
						DateService=PIn.DateT(x["DateService"].ToString())
					}).GroupBy(x=>x.PatNum).ToDictionary(x=>x.Key,x=>x.ToList());
					List<Patient> listPatLims=Patients.GetLimForPats(dictPatClaims.Keys.ToList());
					if(dictPatClaims.Count>0||verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claims with invalid InsSubNum: ")+dictPatClaims.Values.Sum(x => x.Count)+"\r\n";
						log+=Lans.g("FormDatabaseMaintenance","Patients Affected: ")+dictPatClaims.Count+"\r\n\r\n";
						foreach(Patient patLim in listPatLims) {
							string lineItemDBM="   "+Lans.g("FormDatabaseMaintenance","Patient with affected claim(s)")+": "+patLim.GetNameLF()+" (PatNum:"+patLim.PatNum+")"+"\r\n";
							//No additional translation needed. All "words" exactly match Schema column names.
							dictPatClaims[patLim.PatNum].ForEach(x => lineItemDBM+=string.Format("    ClaimNum:{0} DateService:{1}\r\n",x.ClaimNum,x.DateService.ToShortDateString()));
							lineItemDBM+="\r\n";
							log+=lineItemDBM;
						}
					}
					break;
				case DbmMode.Fix:
					command="SELECT claim.ClaimNum, claim.PatNum FROM claim LEFT JOIN inssub ON claim.InsSubNum=inssub.InsSubNum "
						+"WHERE claim.PlanNum=0 AND ( inssub.PlanNum=0 OR inssub.PlanNum IS NULL )";
					DataTable table=Db.GetTable(command);
					long numberFixed=table.Rows.Count;
					InsPlan plan=null;
					InsSub sub=null;
					if(numberFixed>0) {
						log+=Lans.g("FormDatabaseMaintenance","List of patients who will need insurance information reentered:")+"\r\n";
					}
					for(int i = 0;i<numberFixed;i++) {
						plan=new InsPlan();//Create a dummy plan and carrier to attach claims and claim procs to.
						plan.IsHidden=true;
						plan.CarrierNum=Carriers.GetByNameAndPhone("UNKNOWN CARRIER","",true).CarrierNum;
						plan.SecUserNumEntry=userNum;
						InsPlans.Insert(plan);
						long claimNum=PIn.Long(table.Rows[i]["ClaimNum"].ToString());
						long patNum=PIn.Long(table.Rows[i]["PatNum"].ToString());
						sub=new InsSub();//Create inssubs and attach claim and procs to both plan and inssub.
						sub.PlanNum=plan.PlanNum;
						sub.Subscriber=PIn.Long(table.Rows[i]["PatNum"].ToString());
						sub.SubscriberID="unknown";
						sub.SecUserNumEntry=userNum;
						InsSubs.Insert(sub);
						command="UPDATE claim SET PlanNum="+plan.PlanNum+",InsSubNum="+sub.InsSubNum+" WHERE ClaimNum="+claimNum;
						Db.NonQ(command);
						command="UPDATE claimproc SET PlanNum="+plan.PlanNum+",InsSubNum="+sub.InsSubNum+" WHERE ClaimNum="+claimNum;
						Db.NonQ(command);
						Patient pat=Patients.GetLim(patNum);
						log+="PatNum: "+pat.PatNum+" - "+Patients.GetNameFL(pat.LName,pat.FName,pat.Preferred,pat.MiddleI)+"\r\n";
					}
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claims with invalid InsSubNum fixed: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>Also fixes situations where PatNum=0</summary>
		[DbmMethodAttr]
		public static string ClaimWithInvalidPatNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command=@"SELECT claim.ClaimNum, procedurelog.PatNum patNumCorrect
				FROM claim, claimproc, procedurelog
				WHERE claim.PatNum NOT IN (SELECT PatNum FROM patient)
				AND claim.ClaimNum=claimproc.ClaimNum
				AND claimproc.ProcNum=procedurelog.ProcNum
				AND procedurelog.PatNum!=0
				GROUP BY claim.ClaimNum, procedurelog.PatNum";
			DataTable table=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claims found with invalid patients attached: ")+table.Rows.Count.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					for(int i = 0;i<table.Rows.Count;i++) {
						command="UPDATE claim SET PatNum='"+POut.Long(PIn.Long(table.Rows[i]["patNumCorrect"].ToString()))+"' "
							+"WHERE ClaimNum="+POut.Long(PIn.Long(table.Rows[i]["ClaimNum"].ToString()));
						Db.NonQ(command);
					}
					int numberFixed=table.Rows.Count;
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claim with invalid PatNums fixed: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimWithInvalidProvTreat(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM claim WHERE ProvTreat > 0 AND ProvTreat NOT IN (SELECT ProvNum FROM provider);";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claims with invalid ProvTreat found")+": "+numFound.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="UPDATE claim SET ProvTreat="+POut.Long(PrefC.GetLong(PrefName.PracticeDefaultProv))+
							" WHERE ProvTreat > 0 AND ProvTreat NOT IN (SELECT ProvNum FROM provider);";
					long numFixed=Db.NonQ(command);
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claims with invalid ProvTreat fixed")+": "+numFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimWriteoffSum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			//Sums for each claim---------------------------------------------------------------------
			string command=@"SELECT claim.ClaimNum,SUM(claimproc.WriteOff) sumwo,claim.WriteOff
				FROM claim,claimproc
				WHERE claim.ClaimNum=claimproc.ClaimNum
				GROUP BY claim.ClaimNum,claim.WriteOff
				HAVING sumwo-claim.WriteOff > .01
				OR sumwo-claim.WriteOff < -.01";
			DataTable table=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claim writeoff sums found incorrect: ")+table.Rows.Count.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					for(int i = 0;i<table.Rows.Count;i++) {
						command="UPDATE claim SET WriteOff='"+POut.Double(PIn.Double(table.Rows[i]["sumwo"].ToString()))+"' "
							+"WHERE ClaimNum="+table.Rows[i]["ClaimNum"].ToString();
						Db.NonQ(command);
					}
					int numberFixed=table.Rows.Count;
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claim writeoff sums fixed: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		#endregion Carrier, Claim-----------------------------------------------------------------------------------------------------------------------
		#region ClaimPayment----------------------------------------------------------------------------------------------------------------------------

		///<summary>Finds claimpayments where the CheckAmt!=SUM(InsPayAmt) for the claimprocs attached to the claimpayment.  Manual fix.
		///No need to pass in userNum, it's set before remoting role check and passed to the server if necessary.</summary>
		[DbmMethodAttr(HasBreakDown = true,HasUserNum = true)]
		public static string ClaimPaymentCheckAmt(bool verbose,DbmMode modeCur,long userNum = 0) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				userNum=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur,userNum);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			//because of the way this is grouped, it will just get one of many patients for each
			string command=@"SELECT claimproc.ClaimPaymentNum,ROUND(SUM(InsPayAmt),2) _sumpay,ROUND(CheckAmt,2) _checkamt,claimproc.PatNum
					FROM claimpayment,claimproc
					WHERE claimpayment.ClaimPaymentNum=claimproc.ClaimPaymentNum
					GROUP BY claimproc.ClaimPaymentNum,CheckAmt
					HAVING _sumpay!=_checkamt";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			//There is something to report OR the user has verbose mode on.
			string log=Lans.g("FormDatabaseMaintenance","Claim payment sums found incorrect: ")+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {//Only the fix should show the entire list of items.
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {//Running the fix and there are items to show.
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						//Changing the claim payment sums automatically is dangerous so give the user enough information to investigate themselves.
						for(int i = 0;i<table.Rows.Count;i++) {
							Patient pat=Patients.GetPat(PIn.Long(table.Rows[i]["PatNum"].ToString()));
							command="SELECT CheckDate,CheckAmt,IsPartial FROM claimpayment WHERE ClaimPaymentNum="+table.Rows[i]["ClaimPaymentNum"].ToString();
							DataTable claimPayTable=Db.GetTable(command);
							if(pat==null) {
								//insert pat
								Patient dummyPatient=new Patient();
								dummyPatient.PatNum=PIn.Long(table.Rows[i]["PatNum"].ToString());
								dummyPatient.Guarantor=dummyPatient.PatNum;
								dummyPatient.FName="MISSING";
								dummyPatient.LName="PATIENT";
								dummyPatient.AddrNote="This patient was inserted due to claimprocs with invalid PatNum on "+DateTime.Now.ToShortDateString()+" while doing database maintenance.";
								dummyPatient.BillingType=PrefC.GetLong(PrefName.PracticeDefaultBillType);
								dummyPatient.PatStatus=PatientStatus.Archived;
								dummyPatient.PriProv=PrefC.GetLong(PrefName.PracticeDefaultProv);
								dummyPatient.SecUserNumEntry=userNum;
								long dummyPatNum=Patients.Insert(dummyPatient,true);
								SecurityLogs.MakeLogEntry(Permissions.PatientCreate,dummyPatNum,"Recreated from DBM fix for ClaimPaymentCheckAmt.",LogSources.DBM);
								pat=Patients.GetPat(dummyPatient.PatNum);
							}
							log+="   Patient: #"+table.Rows[i]["PatNum"].ToString()+":"+pat.GetNameFirstOrPrefL()
									+" Date: "+PIn.Date(claimPayTable.Rows[0]["CheckDate"].ToString()).ToShortDateString()
									+" Amount: "+PIn.Double(claimPayTable.Rows[0]["CheckAmt"].ToString()).ToString("F");
							if(!PIn.Bool(claimPayTable.Rows[0]["IsPartial"].ToString())) {
								command="UPDATE claimpayment SET IsPartial=1 WHERE ClaimPaymentNum="+PIn.Long(table.Rows[i]["ClaimPaymentNum"].ToString()).ToString();
								Db.NonQ(command);
								log+=" (row has been unlocked and marked as partial)";
							}
							log+="\r\n";
						}
						log+=Lans.g("FormDatabaseMaintenance","   They need to be fixed manually.")+"\r\n";
						#region Potential Fix
						/*
						for(int i=0;i<table.Rows.Count;i++) {
							command="UPDATE claimpayment SET CheckAmt='"+POut.Double(PIn.Double(table.Rows[i]["_sumpay"].ToString()))+"' "
								+"WHERE ClaimPaymentNum="+table.Rows[i]["ClaimPaymentNum"].ToString();
							Db.NonQ(command);
						}
						int numberFixed=table.Rows.Count;
						if(numberFixed>0 || verbose) {
							log+=Lans.g("FormDatabaseMaintenance","Claim payment sums fixed: ")+numberFixed.ToString()+"\r\n";
						}*/
						#endregion Potential Fix
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimPaymentDetachMissingDeposit(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM claimpayment "
						+"WHERE DepositNum != 0 "
						+"AND NOT EXISTS(SELECT * FROM deposit WHERE deposit.DepositNum=claimpayment.DepositNum)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claim payments attached to deposits that no longer exist: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="UPDATE claimpayment SET DepositNum=0 "
						+"WHERE DepositNum != 0 "
						+"AND NOT EXISTS(SELECT * FROM deposit WHERE deposit.DepositNum=claimpayment.DepositNum)";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claim payments detached from deposits that no longer exist: ")
						+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string ClaimPaymentsNotPartialWithNoClaimProcs(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT claimpayment.CheckDate, definition.ItemName, claimpayment.CheckAmt, "+
				"claimpayment.CarrierName, clinic.Description, claimpayment.Note "+
				"FROM claimpayment "+
				"INNER JOIN definition ON definition.DefNum=claimpayment.PayType "+
				"LEFT JOIN clinic ON clinic.ClinicNum=claimpayment.ClinicNum "+
				"WHERE claimpayment.IsPartial=0 "+
				"AND NOT EXISTS( "+
					"SELECT ClaimProcNum "+
					"FROM claimproc "+
					"WHERE claimproc.ClaimPaymentNum=claimpayment.ClaimPaymentNum "+
				") ";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			//There is something to report OR the user has verbose mode on.   
			string log=Lans.g("FormDatabaseMaintenance","Insurance payments with no claims attached that are not marked as partial")+": "+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {//Only the fix should show the entire list of items.
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {//Running the fix and there are items to show.
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							log+="   "+Lans.g("FormDatabaseMaintenance","Date")+": "+PIn.Date(table.Rows[i]["CheckDate"].ToString()).ToShortDateString();
							log+=", "+Lans.g("FormDatabaseMaintenance","Type")+": "+PIn.String(table.Rows[i]["ItemName"].ToString());
							log+=", "+Lans.g("FormDatabaseMaintenance","Amount")+": "+PIn.Double(table.Rows[i]["CheckAmt"].ToString()).ToString("c");
							//Partial will always be blank
							log+=", "+Lans.g("FormDatabaseMaintenance","Carrier")+": "+PIn.String(table.Rows[i]["CarrierName"].ToString());
							log+=", "+Lans.g("FormDatabaseMaintenance","Clinic")+": "+PIn.String(table.Rows[i]["Description"].ToString());
							log+=", "+Lans.g("FormDatabaseMaintenance","Note")+": ";
							if(PIn.String(table.Rows[i]["Note"].ToString()).Length>15) {
								log+=PIn.String(table.Rows[i]["Note"].ToString()).Substring(0,15)+"...";
							}
							else {
								log+=PIn.String(table.Rows[i]["Note"].ToString());
							}
							log+="\r\n";
						}
						log+="   "+Lans.g("FormDatabaseMaintenance","They need to be fixed manually.")+"\r\n";
					}
					break;
			}
			return log;
		}

		#endregion ClaimPayment-------------------------------------------------------------------------------------------------------------------------
		#region ClaimProc-------------------------------------------------------------------------------------------------------------------------------
		/// <summary>Shows patients that have claim payments attached to patient payment plans.</summary>
		[DbmMethodAttr(HasBreakDown=true)]
		public static string ClaimProcAttachedToPatientPaymentPlans(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			DataTable table=GetClaimProcsAttachedToPatientPaymentPlans();
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			string log=Lans.g("FormDatabaseMaintenance","ClaimProcs attached to insurance payment plans")+": "+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {
						log+="\r\n"+Lans.g("FormDatabaseMaintenance","Manual fix needed. Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i=0;i<table.Rows.Count;i++) {
							log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Patient #")+table.Rows[i]["PatNum"].ToString()+" "
								+Lans.g("FormDatabaseMaintenance","has a payment amount for")+" "+PIn.Double(table.Rows[i]["InsPayAmt"].ToString()).ToString("c")+" "
								+Lans.g("FormDatabaseMaintenance","on date")+" "+PIn.Date(table.Rows[i]["DateCP"].ToString()).ToShortDateString()+" "
								+Lans.g("FormDatabaseMaintenance","attached to patient payment plan #")+table.Rows[i]["PayPlanNum"];
						}
						log+="\r\n"+Lans.g("FormDatabaseMaintenance","Run 'Pay Plan Payments' in the Tools tab to fix these payments.");
					}
					break;
			}
			return log;
		}

		///<summary>Deletes claimprocs that are attached to group notes.</summary>
		[DbmMethodAttr]
		public static string ClaimProcEstimateAttachedToGroupNote(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			//It is impossible to attach a group note to a claim, because group notes always have status EC, but status C is required to attach to a claim, or status TP for a preauth.
			//Since the group note cannot be attached to a claim, it also cannot be attached to a claim payment.
			//Claimproc estimates attached to group notes cannot be viewed anywhere in the UI.
			string command="SELECT claimproc.ClaimProcNum "
				+"FROM claimproc "
				+"INNER JOIN procedurelog ON claimproc.ProcNum=procedurelog.ProcNum "
				+"INNER JOIN procedurecode ON procedurecode.CodeNum=procedurelog.CodeNum AND procedurecode.ProcCode='~GRP~' "
				+"WHERE claimproc.Status="+POut.Int((int)ClaimProcStatus.Estimate)+" AND claimproc.ClaimNum=0 AND claimproc.ClaimPaymentNum=0";//Ensures that the claimproc has no relevant information attached to it.
			DataTable table=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Estimates attached to group notes")+": "+table.Rows.Count+"\r\n";
					}
					break;
				case DbmMode.Fix:
					if(table.Rows.Count>0) {
						command="DELETE FROM claimproc WHERE claimproc.ClaimProcNum IN (";
						for(int i = 0;i<table.Rows.Count;i++) {
							if(i>0) {
								command+=",";
							}
							command+=PIn.Long(table.Rows[i]["ClaimProcNum"].ToString());
						}
						command+=")";
						Db.NonQ(command);
					}
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Estimates attached to group notes deleted")+": "+table.Rows.Count+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcDateNotMatchCapComplete(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM claimproc WHERE Status=7 AND DateCP != ProcDate";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Capitation procs with mismatched dates found: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					//js ok
					command="UPDATE claimproc SET DateCP=ProcDate WHERE Status=7 AND DateCP != ProcDate";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Capitation procs with mismatched dates fixed: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcDateNotMatchPayment(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			DataTable table;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT claimproc.ClaimProcNum,claimpayment.CheckDate FROM claimproc,claimpayment "
					+"WHERE claimproc.ClaimPaymentNum=claimpayment.ClaimPaymentNum "
					+"AND claimproc.DateCP!=claimpayment.CheckDate";
					table=Db.GetTable(command);
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claim payments with mismatched dates found: ")+table.Rows.Count.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					//This is a very strict relationship that has been enforced rigorously for many years.
					//If there is an error, it is a fairly new error.  All errors must be repaired.
					//It won't change amounts of history, just dates.  The changes will typically be only a few days or weeks.
					//Various reports assume this enforcement and the reports will malfunction if this is not fixed.
					//Let's list out each change.  Patient name, procedure desc, date of service, old dateCP, new dateCP (check date).
					command="SELECT patient.LName,patient.FName,patient.MiddleI,claimproc.CodeSent,claim.DateService,claimproc.DateCP,claimpayment.CheckDate,claimproc.ClaimProcNum "
					+"FROM claimproc,patient,claim,claimpayment "
					+"WHERE claimproc.PatNum=patient.PatNum "
					+"AND claimproc.ClaimNum=claim.ClaimNum "
					+"AND claimproc.ClaimPaymentNum=claimpayment.ClaimPaymentNum "
					+"AND claimproc.DateCP!=claimpayment.CheckDate";
					table=Db.GetTable(command);
					string patientName;
					string codeSent;
					DateTime dateService;
					DateTime oldDateCP;
					DateTime newDateCP;
					if(table.Rows.Count>0 || verbose) {
						log+="Claim payments with mismatched dates (Patient Name, Code Sent, Date of Service, Old Date, New Date):\r\n";
					}
					for(int i = 0;i<table.Rows.Count;i++) {
						patientName=table.Rows[i]["LName"].ToString() + ", " + table.Rows[i]["FName"].ToString() + " " + table.Rows[i]["MiddleI"].ToString();
						patientName=patientName.Trim();//Looks better when middle initial is not present.//Doesn't work though
						codeSent=table.Rows[i]["CodeSent"].ToString();
						dateService=PIn.Date(table.Rows[i]["DateService"].ToString());
						oldDateCP=PIn.Date(table.Rows[i]["DateCP"].ToString());
						newDateCP=PIn.Date(table.Rows[i]["CheckDate"].ToString());
						command="UPDATE claimproc SET DateCP="+POut.Date(newDateCP)
							+" WHERE ClaimProcNum="+table.Rows[i]["ClaimProcNum"].ToString();
						Db.NonQ(command);
						log+=patientName + ", " + codeSent + ", " + dateService.ToShortDateString() + ", " + oldDateCP.ToShortDateString() + ", " + newDateCP.ToShortDateString() + "\r\n";
					}
					int numberFixed=table.Rows.Count;
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claim payments with mismatched dates fixed: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcDeleteDuplicateEstimateForSameInsPlan(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command;
			if(DataConnection.DBtype==DatabaseType.MySql) {
				//Get all the claimproc estimates that already have a claimproc marked as received from the same InsPlan.
				command="SELECT cp.ClaimProcNum FROM claimproc cp USE KEY(PRIMARY)"
				+" INNER JOIN claimproc cp2 ON cp2.PatNum=cp.PatNum"
				+" AND cp2.PlanNum=cp.PlanNum"    //The same insurance plan
				+" AND cp2.InsSubNum=cp.InsSubNum"//for the same subscriber
				+" AND cp2.ProcNum=cp.ProcNum"    //for the same procedure.
				+" AND cp2.Status="+POut.Int((int)ClaimProcStatus.Received)
				+" WHERE cp.Status="+POut.Int((int)ClaimProcStatus.Estimate)
				+" AND cp.ClaimNum=0";//Make sure the estimate is not already attached to a claim somehow.
			}
			else {//oracle
						//Get all the claimproc estimates that already have a claimproc marked as received from the same InsPlan.
				command="SELECT cp.ClaimProcNum FROM claimproc cp"
				+" INNER JOIN claimproc cp2 ON cp2.PatNum=cp.PatNum"
				+" AND cp2.PlanNum=cp.PlanNum"    //The same insurance plan
				+" AND cp2.InsSubNum=cp.InsSubNum"//for the same subscriber
				+" AND cp2.ProcNum=cp.ProcNum"    //for the same procedure.
				+" AND cp2.Status="+POut.Int((int)ClaimProcStatus.Received)
				+" WHERE cp.Status="+POut.Int((int)ClaimProcStatus.Estimate)
				+" AND cp.ClaimNum=0";//Make sure the estimate is not already attached to a claim somehow.
			}
			DataTable table=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Duplicate ClaimProc estimates for the same InsPlan found: ")+table.Rows.Count+"\r\n";
					}
					break;
				case DbmMode.Fix:
					if(table.Rows.Count>0) {
						command="DELETE FROM claimproc WHERE ClaimProcNum IN (";
						for(int i = 0;i<table.Rows.Count;i++) {
							if(i>0) {
								command+=",";
							}
							command+=table.Rows[i]["ClaimProcNum"].ToString();
						}
						command+=")";
						Db.NonQ(command);
					}
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Duplicate ClaimProc estimates for the same InsPlan deleted: ")+table.Rows.Count+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcDeleteInvalidAdjustments(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM claimproc WHERE claimproc.ClaimNum=0 "
						+"AND NOT EXISTS(SELECT PlanNum FROM insplan WHERE insplan.PlanNum=claimproc.PlanNum) "
						+"AND claimproc.Status="+POut.Int((int)ClaimProcStatus.Adjustment);
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claimproc adjustments found with invalid PlanNum:")+" "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="DELETE FROM claimproc WHERE claimproc.ClaimNum=0 "
						+"AND NOT EXISTS(SELECT PlanNum FROM insplan WHERE insplan.PlanNum=claimproc.PlanNum) "
						+"AND claimproc.Status="+POut.Int((int)ClaimProcStatus.Adjustment);
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claimproc adjustments deleted due to invalid PlanNum:")+" "+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcDeleteWithInvalidClaimNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM claimproc WHERE claimproc.ClaimNum!=0 "
						+"AND NOT EXISTS(SELECT * FROM claim WHERE claim.ClaimNum=claimproc.ClaimNum) "
						+"AND claimproc.InsPayAmt=0 AND claimproc.WriteOff=0";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claimprocs found with invalid ClaimNum: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="DELETE FROM claimproc WHERE claimproc.ClaimNum!=0 "
						+"AND NOT EXISTS(SELECT * FROM claim WHERE claim.ClaimNum=claimproc.ClaimNum) "
						+"AND claimproc.InsPayAmt=0 AND claimproc.WriteOff=0";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claimprocs deleted due to invalid ClaimNum: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcDeleteMismatchPatNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT ClaimProcNum FROM claimproc "
				+"INNER JOIN procedurelog ON procedurelog.ProcNum=claimproc.ProcNum "
				+"WHERE claimproc.ProcNum>0 "
				+"AND claimproc.PatNum!=procedurelog.PatNum "
				+"AND claimproc.InsPayAmt=0 "
				+"AND(claimproc.WriteOff=0 "
				+"OR(claimproc.Status="+(int)ClaimProcStatus.CapEstimate+" "
				+"AND claimproc.WriteOff=procedurelog.ProcFee AND procedurelog.ProcStatus IN("+(int)ProcStat.TP+","+(int)ProcStat.TPi+")))";
			List<long> listClaimProcNums=Db.GetListLong(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					int numFound=listClaimProcNums.Count;
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claimprocs found with PatNum that doesn't match the procedure PatNum:")+" "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					if(listClaimProcNums.Count>0) {
						command="DELETE FROM claimproc WHERE ClaimProcNum IN("+string.Join(",",listClaimProcNums)+")";
						long numberFixed=Db.NonQ(command);
						if(numberFixed>0 || verbose) {
							log+=Lans.g("FormDatabaseMaintenance","Claimprocs deleted due to PatNum not matching the procedure PatNum:")+" "
								+numberFixed.ToString()+"\r\n";
						}
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcDeleteEstimateWithInvalidProcNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM claimproc WHERE ProcNum>0 "
						+"AND Status="+POut.Int((int)ClaimProcStatus.Estimate)+" "
						+"AND NOT EXISTS(SELECT * FROM procedurelog "
						+"WHERE claimproc.ProcNum=procedurelog.ProcNum)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Estimates found for procedures that no longer exist: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					//These seem to pop up quite regularly due to the program forgetting to delete them
					command="DELETE FROM claimproc WHERE ProcNum>0 "
						+"AND Status="+POut.Int((int)ClaimProcStatus.Estimate)+" "
						+"AND NOT EXISTS(SELECT * FROM procedurelog "
						+"WHERE claimproc.ProcNum=procedurelog.ProcNum)";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Estimates deleted for procedures that no longer exist: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcDeleteCapEstimateWithProcComplete(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM claimproc WHERE ProcNum>0 "
						+"AND Status="+POut.Int((int)ClaimProcStatus.CapEstimate)+" "
						+"AND EXISTS("
							+"SELECT * FROM procedurelog "
							+"WHERE claimproc.ProcNum=procedurelog.ProcNum "
							+"AND procedurelog.ProcStatus="+POut.Int((int)ProcStat.C)
						+")";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Capitation estimates found for completed procedures: ")+numFound.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="DELETE FROM claimproc WHERE ProcNum>0 "
						+"AND Status="+POut.Int((int)ClaimProcStatus.CapEstimate)+" "
						+"AND EXISTS("
							+"SELECT * FROM procedurelog "
							+"WHERE claimproc.ProcNum=procedurelog.ProcNum "
							+"AND procedurelog.ProcStatus="+POut.Int((int)ProcStat.C)
						+")";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Capitation estimates deleted for completed procedures: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcEstNoBillIns(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM claimproc WHERE NoBillIns=1 AND InsPayEst !=0";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claimprocs found with non-zero estimates marked NoBillIns: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					//This is just estimate info, regardless of the claimproc status, so totally safe.
					command="UPDATE claimproc SET InsPayEst=0 WHERE NoBillIns=1 AND InsPayEst !=0";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claimproc estimates set to zero because marked NoBillIns: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcEstWithInsPaidAmt(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command=@"SELECT COUNT(*) FROM claimproc WHERE InsPayAmt > 0 AND ClaimNum=0 AND Status=6";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","ClaimProc estimates with InsPaidAmt > 0 found: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					//The InsPayAmt is already being ignored due to the status of the claimproc.  So changing its value is harmless.
					command=@"UPDATE claimproc SET InsPayAmt=0 WHERE InsPayAmt > 0 AND ClaimNum=0 AND Status=6";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","ClaimProc estimates with InsPaidAmt > 0 fixed: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcPatNumMissing(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM claimproc WHERE PatNum=0";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","ClaimProcs with missing patnums found: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="DELETE FROM claimproc WHERE PatNum=0 AND InsPayAmt=0 AND WriteOff=0";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","ClaimProcs with missing patnums fixed: ")+numberFixed+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcProvNumMissing(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM claimproc WHERE ProvNum=0 AND Status!=3";//Status 3 is adjustment which does not require a provider.
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","ClaimProcs with missing provnums found: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					//If estimate, set to default prov (doesn't affect finances)
					command="UPDATE claimproc SET ProvNum="+PrefC.GetString(PrefName.PracticeDefaultProv)+" WHERE ProvNum=0 AND Status="+POut.Int((int)ClaimProcStatus.Estimate);
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","ClaimProcs with missing provnums fixed: ")+numberFixed.ToString()+"\r\n";
					}
					//create a dummy provider (using helper function in Providers.cs)
					//change provnum to the dummy prov (something like Providers.GetDummy())
					//Provider dummy=new Provider();
					//dummy.Abbr="Dummy";
					//dummy.FName="Dummy";
					//dummy.LName="Provider";
					//Will get to this soon.
					//01-17-2011 No fix yet. This has not caused issues except for notifying users.
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string ClaimProcPreauthNotMatchClaim(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command=@"SELECT claim.PatNum,claim.DateService,claimproc.ProcDate,claimproc.CodeSent,claimproc.FeeBilled 
				FROM claimproc,claim 
				WHERE claimproc.ClaimNum=claim.ClaimNum
				AND claim.ClaimType='PreAuth'
				AND claimproc.Status!=2";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			string log=Lans.g("FormDatabaseMaintenance","ClaimProcs for preauths with status not preauth")+": "+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {//Only the fix should show the entire list of items.
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {//Running the fix and there are items to show.
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							Patient pat=Patients.GetPat(PIn.Long(table.Rows[i]["PatNum"].ToString()));
							log+="   Patient: #"+pat.PatNum.ToString()+":"+pat.GetNameFirstOrPrefL()
								+" ClaimDate: "+PIn.Date(table.Rows[i]["DateService"].ToString()).ToShortDateString()
								+" ProcDate: "+PIn.Date(table.Rows[i]["ProcDate"].ToString()).ToShortDateString()
								+" Code: "+table.Rows[i]["CodeSent"].ToString()+"\r\n";
						}
						log+=Lans.g("FormDatabaseMaintenance","   They need to be fixed manually.")+"\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>We are only checking mismatched statuses if claim is marked as received.</summary>
		[DbmMethodAttr(HasBreakDown = true)]
		public static string ClaimProcStatusNotMatchClaim(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command=@"SELECT claim.PatNum,claim.DateService,claimproc.ProcDate,claimproc.CodeSent,claimproc.FeeBilled
					FROM claimproc,claim
					WHERE claimproc.ClaimNum=claim.ClaimNum
					AND claim.ClaimStatus='R'
					AND claimproc.Status="+POut.Int((int)ClaimProcStatus.NotReceived);
			//If a claim is re-sent after being received, the claimprocs Status will be Received but the claim will be Sent, which is to be expected, so we
			//no longer want to flag them as being a DBM issue.  They will show on the unreceived claims report and the user can go manually change the
			//claim status to received if it really is a mistake caused by a user manually changing the claim or claimproc statuses.
			//+"OR (claim.ClaimStatus!='R' AND claimproc.Status="+POut.Int((int)ClaimProcStatus.Received)+"))";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			string log=Lans.g("FormDatabaseMaintenance","ClaimProcs with status not matching claim found: ")+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {//Only the fix should show the entire list of items.
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {//Running the fix and there are items to show.
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							Patient pat=Patients.GetPat(PIn.Long(table.Rows[i]["PatNum"].ToString()));
							log+="   Patient: #"+pat.PatNum.ToString()+":"+pat.GetNameFirstOrPrefL()
								+" ClaimDate: "+PIn.Date(table.Rows[i]["DateService"].ToString()).ToShortDateString()
								+" ProcDate: "+PIn.Date(table.Rows[i]["ProcDate"].ToString()).ToShortDateString()
								+" Code: "+table.Rows[i]["CodeSent"].ToString()
								+" FeeBilled: "+PIn.Double(table.Rows[i]["FeeBilled"].ToString()).ToString("F")+"\r\n";
						}
						log+=Lans.g("FormDatabaseMaintenance","   They need to be fixed manually.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcTotalPaymentWithInvalidDate(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT ClaimProcNum FROM claimproc,claim"
				+" WHERE claimproc.ProcNum=0"//Total payments
				+" AND claimproc.ProcDate < "+POut.Date(new DateTime(1880,1,1))//which have invalid dates
				+" AND claimproc.ClaimNum=claim.ClaimNum"
				+" AND claim.DateService > "+POut.Date(new DateTime(1880,1,1));//but have valid date of service on the claim
			DataTable table=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Total claim payments with invalid date found")+": "+table.Rows.Count+"\r\n";
					}
					break;
				case DbmMode.Fix:
					if(table.Rows.Count>0) {
						command="UPDATE claimproc,claim SET claimproc.ProcDate=claim.DateService"//Resets date for total payments to DateService
							+" WHERE claimproc.ProcNum=0"//Total payments
							+" AND claimproc.ProcDate < "+POut.Date(new DateTime(1880,1,1))//which have invalid dates
							+" AND claimproc.ClaimNum=claim.ClaimNum"
							+" AND claim.DateService > "+POut.Date(new DateTime(1880,1,1));//but have valid date of service on the claim
						Db.NonQ(command);
					}
					int numberFixed=table.Rows.Count;
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Total claim payments with invalid date fixed")+": "+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>No need to pass in userNum, it's set before remoting role check and passed to the server if necessary.</summary>
		[DbmMethodAttr(HasUserNum = true)]
		public static string ClaimProcWithInvalidClaimNum(bool verbose,DbmMode modeCur,long userNum = 0) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				userNum=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur,userNum);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM claimproc WHERE claimproc.ClaimNum!=0 "
						+"AND NOT EXISTS(SELECT * FROM claim WHERE claim.ClaimNum=claimproc.ClaimNum) "
						+"AND (claimproc.InsPayAmt!=0 OR claimproc.WriteOff!=0)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claimprocs found with invalid ClaimNum: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					//We can't touch those claimprocs because it would mess up the accounting.
					//We will create dummy claims for all claimprocs with invalid ClaimNums if those claimprocs have amounts entered in the InsPayAmt or Writeoff columns, otherwise you could not delete the procedure or create a new claim
					command="SELECT * FROM claimproc WHERE claimproc.ClaimNum!=0 "
						+"AND NOT EXISTS(SELECT * FROM claim WHERE claim.ClaimNum=claimproc.ClaimNum) "
						+"AND (claimproc.InsPayAmt!=0 OR claimproc.WriteOff!=0) "
						+"GROUP BY claimproc.ClaimNum";
					DataTable table=Db.GetTable(command);
					List<ClaimProc> cpList=Crud.ClaimProcCrud.TableToList(table);
					Claim claim;
					for(int i = 0;i<cpList.Count;i++) {
						claim=new Claim();
						claim.ClaimNum=cpList[i].ClaimNum;
						claim.PatNum=cpList[i].PatNum;
						claim.ClinicNum=cpList[i].ClinicNum;
						if(cpList[i].Status==ClaimProcStatus.Received) {
							claim.ClaimStatus="R";//Status received because we know it's been paid on and the claimproc status is received
						}
						else {
							claim.ClaimStatus="W";
						}
						claim.PlanNum=cpList[i].PlanNum;
						claim.InsSubNum=cpList[i].InsSubNum;
						claim.ProvTreat=cpList[i].ProvNum;
						claim.SecUserNumEntry=userNum;
						Crud.ClaimCrud.Insert(claim,true);//Allows us to use a primary key that was "used".
						Patient pat=Patients.GetLim(claim.PatNum);
						log+=Lans.g("FormDatabaseMaintenance","Claim created due to claimprocs with invalid ClaimNums for patient: ")
							+pat.PatNum+" - "+Patients.GetNameFL(pat.LName,pat.FName,pat.Preferred,pat.MiddleI)+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcWithInvalidClaimPaymentNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command=@"SELECT COUNT(*) FROM claimproc WHERE claimpaymentnum !=0 AND NOT EXISTS(
						SELECT * FROM claimpayment WHERE claimpayment.ClaimPaymentNum=claimproc.ClaimPaymentNum)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","ClaimProcs with with invalid ClaimPaymentNumber found: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					//slightly dangerous.  User will have to create ins check again.  But does not alter financials.
					command=@"UPDATE claimproc SET ClaimPaymentNum=0 WHERE claimpaymentnum !=0 AND NOT EXISTS(
						SELECT * FROM claimpayment WHERE claimpayment.ClaimPaymentNum=claimproc.ClaimPaymentNum)";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","ClaimProcs with with invalid ClaimPaymentNumber fixed: ")+numberFixed.ToString()+"\r\n";
						//Tell user what items to create ins checks for?
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcWithInvalidPayPlanNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command=@"SELECT COUNT(*) FROM claimproc WHERE PayPlanNum>0 AND PayPlanNum NOT IN(SELECT PayPlanNum FROM payplan)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","ClaimProcs with with invalid PayPlanNum found")+": "+numFound.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					//safe, if the user wants to attach the claimprocs to a payplan for tracking the ins payments they would just need to attach to a valid payplan
					command=@"UPDATE claimproc SET PayPlanNum=0 WHERE PayPlanNum>0 AND PayPlanNum NOT IN(SELECT PayPlanNum FROM payplan)";
					long numFixed=Db.NonQ(command);
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","ClaimProcs with with invalid PayPlanNum fixed")+": "+numFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcWithInvalidProvNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM claimproc WHERE ProvNum > 0 AND ProvNum NOT IN (SELECT ProvNum FROM provider)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claimprocs with invalid ProvNum found")+": "+numFound.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="UPDATE claimproc SET ProvNum="+POut.Long(PrefC.GetLong(PrefName.PracticeDefaultProv))+
							" WHERE ProvNum > 0 AND ProvNum NOT IN (SELECT ProvNum FROM provider)";
					long numFixed=Db.NonQ(command);
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claimprocs with invalid ProvNum fixed")+": "+numFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ClaimProcWithInvalidSubNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM claimproc WHERE claimproc.InsSubNum > 0 AND claimproc.Status="+POut.Int((int)ClaimProcStatus.Estimate)
						+" AND claimproc.InsSubNum NOT IN (SELECT inssub.InsSubNum FROM inssub)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claimprocs with invalid InsSubNum found")+": "+numFound.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="DELETE FROM claimproc WHERE claimproc.InsSubNum > 0 AND claimproc.Status="+POut.Int((int)ClaimProcStatus.Estimate)
						+" AND claimproc.InsSubNum NOT IN (SELECT inssub.InsSubNum FROM inssub)";
					long numFixed=Db.NonQ(command);
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claimprocs with invalid InsSubNum fixed")+": "+numFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string ClaimProcWriteOffNegative(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command=@"SELECT patient.LName,patient.FName,patient.MiddleI,claimproc.CodeSent,procedurelog.ProcFee,procedurelog.ProcDate,claimproc.WriteOff
					FROM claimproc 
					LEFT JOIN patient ON claimproc.PatNum=patient.PatNum
					LEFT JOIN procedurelog ON claimproc.ProcNum=procedurelog.ProcNum 
					WHERE WriteOff<0";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			string log=Lans.g("FormDatabaseMaintenance","Negative writeoffs found: ")+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {//Only the fix should show the entire list of items.
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {//Running the fix and there are items to show.
						string patientName;
						string codeSent;
						decimal writeOff;
						decimal procFee;
						DateTime procDate;
						log+=Lans.g("FormDatabaseMaintenance","List of patients with procedures that have negative writeoffs:\r\n");
						for(int i = 0;i<table.Rows.Count;i++) {
							patientName=table.Rows[i]["LName"].ToString() + ", " + table.Rows[i]["FName"].ToString() + " " + table.Rows[i]["MiddleI"].ToString();
							codeSent=table.Rows[i]["CodeSent"].ToString();
							procDate=PIn.Date(table.Rows[i]["ProcDate"].ToString());
							writeOff=PIn.Decimal(table.Rows[i]["WriteOff"].ToString());
							procFee=PIn.Decimal(table.Rows[i]["ProcFee"].ToString());
							log+=patientName+" "+codeSent+" fee:"+procFee.ToString("c")+" date:"+procDate.ToShortDateString()+" writeoff:"+writeOff.ToString("c")+"\r\n";
						}
						log+=Lans.g("FormDatabaseMaintenance","Go to the patients listed above and manually correct the writeoffs.\r\n");
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string CanadaClaimProcForWrongPatient(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(!CultureInfo.CurrentCulture.Name.EndsWith("CA")) {
				return Lans.g("FormDatabaseMaintenance","Skipped. Local computer region must be set to Canada to run.");
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			//Look at the comments for claimProc.DateEntry, if not set then the claimProc has no financial meaning yet.
			string command=@"SELECT claimproc.*
				FROM claimproc 
				INNER JOIN claim ON claim.ClaimNum=claimproc.ClaimNum
				WHERE (claimproc.PatNum!=claim.PatNum)";
			List<ClaimProc> listClaimProcs=Crud.ClaimProcCrud.SelectMany(command);
			if(listClaimProcs.Count==0 && !verbose) {
				return "";
			}
			string log=Lans.g("FormDatabaseMaintenance","Claimprocs associated to wrong patient")+":"+listClaimProcs.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(listClaimProcs.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claimprocs associated to wrong patient")+": "+listClaimProcs.Count.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					foreach(ClaimProc claimProc in listClaimProcs) {
						ClaimProcs.Delete(claimProc);
					}
					break;
			}
			return log;
		}

		#endregion ClaimProc----------------------------------------------------------------------------------------------------------------------------
		#region Clearinghouse---------------------------------------------------------------------------------------------------------------------------
		[DbmMethodAttr(HasBreakDown = true)]
		public static string ClearinghouseInvalidFormat(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			int formatEnumCount=Enum.GetNames(typeof(ElectronicClaimFormat)).Length-1;
			string command="SELECT clearinghouse.Description,COALESCE(clinic.Abbr,'"+POut.String(Lans.g("FormDatabaseMaintenance","Unassigned"))+"') Abbr "
				+"FROM clearinghouse "
				+"LEFT JOIN clinic on clinic.ClinicNum=clearinghouse.ClinicNum "
				+"WHERE EFormat>"+formatEnumCount;
			DataTable table=Db.GetTable(command);
			int numFound=table.Rows.Count;
			if(numFound==0 && !verbose) {
				return "";
			}
			string log=Lans.g("FormDatabaseMaintenance","Clearinghouses with invalid Format found:")+" "+numFound;
			switch(modeCur) {
				case DbmMode.Check:
					if(numFound>0) {
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.");
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(numFound>0) {
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							log+="	"+table.Rows[i]["Description"].ToString();
							if(PrefC.HasClinicsEnabled) {
								log+=" "+Lans.g("FormDatabaseMaintenance","for Clinic")+": "+table.Rows[i]["Abbr"].ToString();
							}
							log+="\r\n";
						}
						log+=Lans.g("FormDatabaseMaintenance","They need to be fixed manually.");
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string ClearinghouseInvalidCommBridge(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			int commBridgeEnumCount=Enum.GetNames(typeof(EclaimsCommBridge)).Length - 1;
			string command="SELECT clearinghouse.Description,COALESCE(clinic.Abbr,'"+POut.String(Lans.g("FormDatabaseMaintenance","Unassigned"))+"') Abbr "
				+"FROM clearinghouse "
				+"LEFT JOIN clinic on clinic.ClinicNum=clearinghouse.ClinicNum "
				+"WHERE CommBridge>"+commBridgeEnumCount;
			DataTable table=Db.GetTable(command);
			int numFound=table.Rows.Count;
			if(numFound==0 && !verbose) {
				return "";
			}
			string log=Lans.g("FormDatabaseMaintenance","Clearinghouses with invalid CommBridge found:")+" "+numFound;
			switch(modeCur) {
				case DbmMode.Check:
					if(numFound>0) {
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.");
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(numFound>0) {
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							log+="	"+table.Rows[i]["Description"].ToString();
							if(PrefC.HasClinicsEnabled) {
								log+=" "+Lans.g("FormDatabaseMaintenance","for Clinic")+": "+table.Rows[i]["Abbr"].ToString();
							}
							log+="\r\n";
						}
						log+=Lans.g("FormDatabaseMaintenance","They need to be fixed manually.");
					}
					break;
			}
			return log;
		}
		#endregion
		#region Clinic

		///<summary>Inserts missing/invalid clinics.</summary>
		[DbmMethodAttr]
		public static string ClinicNumMissingInvalid(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			//look at the procedurelog and patient table because they will most likely have all possible clinics.
			string command=@"
				SELECT procedurelog.ClinicNum 
				FROM procedurelog 
				WHERE procedurelog.ClinicNum > 0 
				AND procedurelog.ClinicNum NOT IN (SELECT ClinicNum FROM clinic) 
				UNION 
				SELECT patient.ClinicNum 
				FROM patient
				WHERE patient.ClinicNum > 0 
				AND patient.ClinicNum NOT IN (SELECT ClinicNum FROM clinic) ";
			List<long> listInvalidClinicNums = Db.GetListLong(command);
			string log = "";
			switch(modeCur) {
				case DbmMode.Check:
					if(listInvalidClinicNums.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Clinics missing")+": "+listInvalidClinicNums.Count+"\r\n";
					}
					break;
				case DbmMode.Fix:
					if(listInvalidClinicNums.Count>0) {
						foreach(long clinicNumInvalid in listInvalidClinicNums) {
							command ="SELECT MAX(ItemOrder) FROM clinic";
							int itemOrd = Db.GetInt(command) + 1;
							Clinic missingClinic = new Clinic() {
								ClinicNum = clinicNumInvalid,
								Description = "INVALID CLINIC #"+clinicNumInvalid,
								Abbr = "INVALID #"+clinicNumInvalid,
								ItemOrder = itemOrd
							};
							Clinics.Insert(missingClinic,true);
						}
						if(listInvalidClinicNums.Count>0 || verbose) {
							log+=Lans.g("FormDatabaseMaintenance","Missing clinics added")+": "+listInvalidClinicNums.Count+"\r\n";
						}
					}
					break;
			}
			return log;
		}

		#endregion
		#region ClockEvent, Deposit, Disease, Document--------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string ClockEventInFuture(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command=@"SELECT COUNT(*) FROM clockevent WHERE TimeDisplayed1 > "+DbHelper.Now()+"+INTERVAL 15 MINUTE";
					int numFound=PIn.Int(Db.GetCount(command));
					command=@"SELECT COUNT(*) FROM clockevent WHERE TimeDisplayed2 > "+DbHelper.Now()+"+INTERVAL 15 MINUTE";
					numFound+=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Timecard entries invalid")+": "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command=@"UPDATE clockevent SET TimeDisplayed1="+DbHelper.Now()+" WHERE TimeDisplayed1 > "+DbHelper.Now()+"+INTERVAL 15 MINUTE";
					long numberFixed=Db.NonQ(command);
					command=@"UPDATE clockevent SET TimeDisplayed2="+DbHelper.Now()+" WHERE TimeDisplayed2 > "+DbHelper.Now()+"+INTERVAL 15 MINUTE";
					numberFixed+=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Future timecard entry times fixed")+": "+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		/*Deprecated per Nathan 4/18/2017.
		 * These do not show in the UI, so there is no way to manually fix them (aside from running a query to manually delete them).  Since they have no
		 * payments attached, they should not affect any reports or any other area of the program.  And since there is no fix Nathan decided we just
		 * wouldn't report them.  The only possible downside is if the table filled up with thousands or hundreds of thousands of these "bad" deposits,
		 * but we don't believe this will happen.  We believe we have fixed the bug that was creating this as of 4/18/2017.  FormDepositEdit was allowing
		 * empty deposits to be created as well as an issue with the Print/CreatePDF/Email buttons and the Refresh button causing transactions to be saved
		 * attached to the deposit, then when refreshed they were removed from the list and when OK was pressed the transactions were removed from the
		 * deposit but the deposit was left in the db with the amount but nothing attached.
		[DbmMethodAttr(HasBreakDown=true)]
		public static string DepositsWithNoPayments(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			//Gets all deposits that have a positive amount with no payments attached. This was caused by a bug in 16.2.1-16.2.8.
			command=@"SELECT deposit.DepositNum,deposit.DateDeposit,deposit.Amount
				FROM deposit 
				LEFT JOIN payment ON payment.DepositNum=deposit.DepositNum
				LEFT JOIN claimpayment ON claimpayment.DepositNum=deposit.DepositNum			
				WHERE payment.PayNum IS NULL AND claimpayment.ClaimPaymentNum IS NULL
				AND deposit.Amount>0.005
				ORDER BY deposit.DateDeposit,deposit.DepositNum";
			table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			int numFound=table.Rows.Count;
			string log=Lans.g("FormDatabaseMaintenance","Deposits found with no payments attached: ")+numFound;
			switch(modeCur) {
				case DbmMode.Check:
					if(numFound>0) {//Only the fix should show the entire list of items.
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(numFound>0) {//Running the fix and there are items to show.
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						foreach(DataRow row in table.Rows) {
							log+="   "+PIn.Date(row["DateDeposit"].ToString()).ToShortDateString()
								+"    "+PIn.Double(row["Amount"].ToString()).ToString("f")+"\r\n";
						}
						log+="   "+Lans.g("FormDatabaseMaintenance","They need to be deleted manually.")+"\r\n";
					}
					break;
			}
			return log;
		}*/

		///<summary>Finds deposits where there are attached payments and the deposit amount does not equal the sum of the attached payment amounts.
		///No need to pass in userNum, it's set before remoting role check and passed to the server if necessary.</summary>
		[DbmMethodAttr(HasBreakDown = true,HasUserNum = true)]
		public static string DepositsWithIncorrectAmount(bool verbose,DbmMode modeCur,long userNum = 0) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				userNum=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur,userNum);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			//only deposits with payments attached (INNER JOIN) where sum of payment amounts don't match the deposit amount
			//deposits with positive amount but no payments attached (LEFT JOIN...) is handled in a separate DBM above
			string command=@"SELECT deposit.DepositNum,deposit.Amount,deposit.DateDeposit,SUM(payments.amt) _sum
				FROM deposit
				INNER JOIN (
					SELECT payment.DepositNum,payment.PayAmt amt
					FROM payment
					UNION ALL
					SELECT claimpayment.DepositNum,claimpayment.CheckAmt amt
					FROM claimpayment
				) payments ON payments.DepositNum=deposit.DepositNum
				GROUP BY deposit.DepositNum
				HAVING ROUND(_sum,2) != ROUND(deposit.Amount,2)
				ORDER BY deposit.DateDeposit,deposit.DepositNum";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			//There is something to report OR the user has verbose mode on.
			string log=Lans.g("FormDatabaseMaintenance","Deposit sums found incorrect: ")+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {//Running the fix and there are items to show.
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							DateTime date=PIn.Date(table.Rows[i]["DateDeposit"].ToString());
							Double oldval=PIn.Double(table.Rows[i]["Amount"].ToString());
							Double newval=PIn.Double(table.Rows[i]["_sum"].ToString());
							log+="   "+Lans.g("FormDatabaseMaintenance","Deposit Date: ")+date.ToShortDateString()
								+", "+Lans.g("FormDatabaseMaintenance","Current Sum: ")+oldval.ToString("c")
								+", "+Lans.g("FormDatabaseMaintenance","Expected Sum:")+newval.ToString("c")+"\r\n";
						}
						log+=Lans.g("FormDatabaseMaintenance","   They need to be fixed manually.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string DiseaseWithInvalidDiseaseDef(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			string log="";
			string command=@"SELECT DiseaseNum,DiseaseDefNum FROM disease WHERE DiseaseDefNum NOT IN(SELECT DiseaseDefNum FROM diseasedef)";
			DataTable table=Db.GetTable(command);
			int numFound=table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Problems with invalid references found")+": "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					if(numFound > 0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Problems with invalid references found")+": "+numFound+"\r\n";
					}
					if(numFound > 0) {
						//Check to see if there is already a diseasedef called UNKNOWN PROBLEM.
						command="SELECT DiseaseDefNum FROM diseasedef WHERE DiseaseName='UNKNOWN PROBLEM'";
						long diseaseDefNum=PIn.Long(Db.GetScalar(command));
						if(diseaseDefNum==0) {
							//Create a new DiseaseDef called UNKNOWN PROBLEM.
							DiseaseDef diseaseDef=new DiseaseDef();
							diseaseDef.DiseaseName="UNKNOWN PROBLEM";
							diseaseDef.IsHidden=false;
							diseaseDefNum=DiseaseDefs.Insert(diseaseDef);
						}
						//Update the disease table.
						command="UPDATE disease SET DiseaseDefNum="+POut.Long(diseaseDefNum)+" WHERE DiseaseNum IN("
							+string.Join(",",table.Select().Select(x => PIn.Long(x["DiseaseNum"].ToString())))+")";
						Db.NonQ(command);
						log+=Lans.g("FormDatabaseMaintenance","All invalid references have been attached to the problem called")+" UNKNOWN PROBLEM.\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string DocumentWithInvalidDate(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			//Gets a list of documents with dates that are invalid (0001-01-01). The list should be blank. If not, then the document's date will be set to 001-01-02 which will allow deletion.
			string command="SELECT COUNT(*) FROM document WHERE DateCreated="+POut.Date(new DateTime(1,1,1));
			int numFound=PIn.Int(Db.GetCount(command));
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Documents with invalid dates found")+": "+numFound.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					if(numFound>0) {
						command="UPDATE document SET DateCreated="+POut.Date(new DateTime(1,1,2))+" WHERE DateCreated="+POut.Date(new DateTime(1,1,1));
						Db.NonQ(command);
					}
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Documents with invalid dates fixed")+": "+numFound.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string DocumentWithNoCategory(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT DocNum FROM document WHERE DocCategory=0";
			DataTable table=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Images with no category found: ")+table.Rows.Count+"\r\n";
					}
					break;
				case DbmMode.Fix:
					List<Def> listDefs=Defs.GetDefsForCategory(DefCat.ImageCats,true);
					for(int i = 0;i<table.Rows.Count;i++) {
						command="UPDATE document SET DocCategory="+POut.Long(listDefs[0].DefNum)
							+" WHERE DocNum="+table.Rows[i][0].ToString();
						Db.NonQ(command);
					}
					int numberFixed=table.Rows.Count;
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Images with no category fixed: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		#endregion ClockEvent, Deposit, Disease, Document-----------------------------------------------------------------------------------------------
		#region Ebill, EduResource, EmailAttach---------------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string EbillDuplicates(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			//Min may not be the oldest when using random primary keys, but we have to pick one.  In most cases they're identical anyway.
			string command="SELECT MIN(EbillNum) EbillNum,COUNT(*) Count "
				+"FROM ebill "
				+"GROUP BY ClinicNum ";
			DataTable tableEbills=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					int numFound=tableEbills.Select().Select(x => PIn.Int(x["Count"].ToString())-1).Sum();//count duplicates=Sum(# per group-1)
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Ebill duplicate clinic entries found: ")
							+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					long numberFixed=0;
					if(tableEbills.Rows.Count>0) {
						command="DELETE FROM ebill WHERE EbillNum NOT IN ("
							+string.Join(",",tableEbills.Select().Select(x => PIn.Long(x["EbillNum"].ToString())))+")";
						numberFixed=Db.NonQ(command);
					}
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Ebill duplicate clinic entries deleted: ")
							+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>Inserts an ebill for ClinicNum 0 if it does not exist.</summary>
		[DbmMethodAttr]
		public static string EbillMissingDefaultEntry(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command="SELECT COUNT(*) FROM ebill WHERE ClinicNum=0";
			int numFound=PIn.Int(Db.GetCount(command));
			switch(modeCur) {
				case DbmMode.Check:
					if(numFound==0) {
						log+=Lans.g("FormDatabaseMaintenance","Missing default ebill entry.");
					}
					break;
				case DbmMode.Fix:
					if(numFound==0) {
						long ebillNum=OpenDentBusiness.Crud.EbillCrud.Insert(new Ebill() {
							ClinicNum=0,
							ClientAcctNumber="",
							ElectUserName="",
							ElectPassword="",
							PracticeAddress=EbillAddress.PracticePhysical,
							RemitAddress=EbillAddress.PracticeBilling
						});
						if(ebillNum>0) {
							log+=Lans.g("FormDatabaseMaintenance","Default ebill entry inserted.");
						}
					}
					break;
			}
			return log;
		}

		///<summary>This could be enhanced to validate all foreign keys on the eduresource.</summary>
		[DbmMethodAttr]
		public static string EduResourceInvalidDiseaseDefNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT EduResourceNum FROM eduresource WHERE DiseaseDefNum != 0 AND DiseaseDefNum NOT IN (SELECT DiseaseDefNum FROM diseasedef)";
			DataTable table=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","EHR Educational Resources with invalid problem found: ")+table.Rows.Count+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="SELECT DiseaseDefNum FROM diseasedef WHERE ItemOrder=(SELECT MIN(ItemOrder) FROM diseasedef WHERE IsHidden=0)";
					long defNum=PIn.Long(Db.GetScalar(command));
					for(int i = 0;i<table.Rows.Count;i++) {
						command="UPDATE eduresource SET DiseaseDefNum='"+defNum+"' WHERE EduResourceNum='"+table.Rows[i][0].ToString()+"'";
						Db.NonQ(command);
					}
					int numberFixed=table.Rows.Count;
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","EHR Educational Resources with invalid problem fixed: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string EmailAttachWithTemplateNumAndMessageNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM emailattach WHERE emailattach.EmailTemplateNum!=0 AND emailattach.EmailMessageNum!=0";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Email attachments attached to both an email and a template found")+": "+numFound.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="UPDATE emailattach SET EmailTemplateNum=0 WHERE emailattach.EmailTemplateNum!=0 AND emailattach.EmailMessageNum!=0";
					long numFixed=Db.NonQ(command);
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Email attachments attached to both an email and a template fixed")+": "+numFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		#endregion Ebill, EduResource, EmailAttach------------------------------------------------------------------------------------------------------
		#region Fee, FeeSchedule, GroupNote-------------------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string FeeDeleteDuplicates(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			string command="SELECT FeeNum,FeeSched,CodeNum,Amount,ClinicNum,ProvNum FROM fee GROUP BY FeeSched,CodeNum,ClinicNum,ProvNum HAVING COUNT(CodeNum)>1";
			DataTable table=Db.GetTable(command);
			int count=table.Rows.Count;
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedure codes with duplicate fee entries: ")+count+"\r\n";
					}
					break;
				case DbmMode.Fix:
					long numberFixed=0;
					for(int i = 0;i<count;i++) {
						if(i==0) {
							log+=Lans.g("FormDatabaseMaintenance","The following procedure codes had duplicate fee entries.  Verify that the following amounts are correct:")+"\r\n";
						}
						//Make an entry in the log so that the user knows to verify the amount for this fee.
						log+="Fee Schedule: "+FeeScheds.GetDescription(PIn.Long(table.Rows[i]["FeeSched"].ToString()))//No call to db.
							+" - Code: "+ProcedureCodes.GetStringProcCode(PIn.Long(table.Rows[i]["CodeNum"].ToString()))//No call to db.
							+" - Amount: "+PIn.Double(table.Rows[i]["Amount"].ToString()).ToString("n")+"\r\n";
						//At least one fee needs to stay.  Each row in table is a random fee, so we'll just keep that one and delete the rest.
						command="SELECT FeeNum FROM fee WHERE FeeSched="+table.Rows[i]["FeeSched"].ToString()
							+" AND CodeNum="+table.Rows[i]["CodeNum"].ToString()
							+" AND ClinicNum="+table.Rows[i]["ClinicNum"].ToString()
							+" AND ProvNum="+table.Rows[i]["ProvNum"].ToString()
							+" AND FeeNum!="+table.Rows[i]["FeeNum"].ToString();//This is the random fee we will keep.
						List<long> listFeeNums=Db.GetListLong(command);
						Fees.DeleteMany(listFeeNums);
						numberFixed+=listFeeNums.Count;
					}
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Duplicate fees deleted")+": "+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string FeeDeleteForInvalidProc(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			string command=@"SELECT FeeNum,FeeSched,fee.CodeNum AS 'CodeNum' FROM fee 
									LEFT JOIN procedurecode ON fee.CodeNum=procedurecode.CodeNum 
									WHERE procedurecode.CodeNum IS NULL";
			DataTable table=Db.GetTable(command);
			int count=table.Rows.Count;
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedure codes with invalid procedure codes: ")+count+"\r\n";
					}
					break;
				case DbmMode.Fix:
					long numberFixed=0;
					for(int i = 0;i<count;i++) {
						if(i==0) {
							log+=Lans.g("FormDatabaseMaintenance","The following fees had invalid procedure codes:")+"\r\n";
						}
						//Make an entry in the log so that the user knows to verify the amount for this fee.
						log+="Fee Schedule: "+FeeScheds.GetDescription(PIn.Long(table.Rows[i]["FeeSched"].ToString()))+"\r\n";//No call to db.
						command="SELECT FeeNum FROM fee WHERE FeeNum="+table.Rows[i]["FeeNum"].ToString();
						List<long> listFeeNums=Db.GetListLong(command);
						Fees.DeleteMany(listFeeNums);
						numberFixed+=listFeeNums.Count;
					}
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Fees with invalid procedure codes deleted")+": "+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string GroupNoteWithInvalidAptNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM procedurelog "
						+"INNER JOIN procedurecode ON procedurelog.CodeNum=procedurecode.CodeNum "
						+"WHERE procedurelog.AptNum!=0 AND procedurecode.ProcCode='~GRP~'";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Group notes attached to appointments: ")+numFound.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="UPDATE procedurelog SET AptNum=0 "
						+"WHERE AptNum!=0 AND CodeNum IN(SELECT CodeNum FROM procedurecode WHERE procedurecode.ProcCode='~GRP~')";
					long numfixed=Db.NonQ(command);
					if(numfixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Group notes attached to appointments fixed: ")+numfixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string GroupNoteWithInvalidProcStatus(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM procedurelog "
						+"INNER JOIN procedurecode ON procedurelog.CodeNum=procedurecode.CodeNum "
						+"WHERE procedurelog.ProcStatus NOT IN("+POut.Int((int)ProcStat.D)+","+POut.Int((int)ProcStat.EC)+") "
						+"AND procedurecode.ProcCode='~GRP~'";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Group notes with invalid status: ")+numFound.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="UPDATE procedurelog SET ProcStatus="+POut.Long((int)ProcStat.EC)+" "
						+"WHERE ProcStatus NOT IN("+POut.Int((int)ProcStat.D)+","+POut.Int((int)ProcStat.EC)+") "
						+"AND CodeNum IN(SELECT CodeNum FROM procedurecode WHERE procedurecode.ProcCode='~GRP~')";
					long numfixed=Db.NonQ(command);
					if(numfixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Group notes statuses fixed: ")+numfixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string FeeScheduleHiddenWithPatient(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(DISTINCT(FeeSchedNum)) FROM patient "
						+"INNER JOIN feesched ON patient.FeeSched=feesched.FeeSchedNum "
						+"WHERE feesched.IsHidden=1";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Hidden Fee Schedules associated to patients: ")+numFound.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="UPDATE feesched SET IsHidden=0 "
						+"WHERE IsHidden=1 AND FeeSchedNum IN (SELECT FeeSched FROM patient)";
					long numfixed=Db.NonQ(command);
					if(numfixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Hidden Fee Schedules associated to patients unhidden: ")+numfixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		#endregion Fee, FeeSchedule, GroupNote----------------------------------------------------------------------------------------------------------
		#region Icd9
		[DbmMethodAttr]
		public static string Icd9InvalidCodes(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			List<long> listIcd9Nums=Db.GetListLong("SELECT ICD9Num FROM icd9 WHERE ICD9Code=''");
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(listIcd9Nums.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Invalid ICD9 codes found")+": "+listIcd9Nums.Count+"\r\n";
					}
				break;
				case DbmMode.Fix:
					if(listIcd9Nums.Count>0) {
						Db.NonQ("DELETE FROM icd9 WHERE ICD9Num IN("+String.Join(",",listIcd9Nums)+")");
					}
					int numberFixed=listIcd9Nums.Count;
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Invalid ICD9 codes removed")+": "+numberFixed.ToString()+"\r\n";
					}
				break;
			}
			return log;
		}
		#endregion
		#region InsPayPlan, InsPlan, InsSub-------------------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string InsPayPlanWithPatientPayments(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			//Gets a list of payplans of type insurance that have patient payments attached to them and no insurance payments attached
			string command=@"SELECT payplan.PayPlanNum 
								FROM payplan
								INNER JOIN paysplit ON paysplit.PayPlanNum=payplan.PayPlanNum
								LEFT JOIN claimproc ON claimproc.PayPlanNum=payplan.PayPlanNum
									AND claimproc.Status IN("
										+POut.Int((int)ClaimProcStatus.Received)+","
										+POut.Int((int)ClaimProcStatus.Supplemental)+","
										+POut.Int((int)ClaimProcStatus.CapClaim)+") "+
							@"WHERE payplan.PlanNum!=0
								AND claimproc.ClaimProcNum IS NULL
								GROUP BY payplan.PayPlanNum";
			List<long> listPayPlanNums=Db.GetListLong(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(listPayPlanNums.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Ins payment plans with patient payments attached")+": "+listPayPlanNums.Count+"\r\n";
					}
					break;
				case DbmMode.Fix:
					if(listPayPlanNums.Count>0) {
						//Change the insurance payment plan to a patient payment plan so that the payments will be visible within the payment plan
						command="UPDATE payplan SET PlanNum=0 WHERE PayPlanNum IN("+String.Join(",",listPayPlanNums)+")";
						Db.NonQ(command);
					}
					int numberFixed=listPayPlanNums.Count;
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Ins payment plans with patient payments attached fixed")+": "+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string InsPlanInvalidCarrier(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			//Gets a list of insurance plans that do not have a carrier attached. The list should be blank. If not, then you need to go to the plan listed and add a carrier. Missing carriers will cause the send claims function to give an error.
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT PlanNum FROM insplan WHERE CarrierNum NOT IN (SELECT CarrierNum FROM carrier)";
					DataTable table=Db.GetTable(command);
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Ins plans with carrier missing found: ")+table.Rows.Count+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
					command="SELECT PlanNum, CarrierNum FROM insplan WHERE CarrierNum NOT IN (SELECT CarrierNum FROM carrier)";
					var dictCarrierPlans=Db.GetTable(command).Select().Select(x=> new {
						CarrierNum=PIn.Long(x["CarrierNum"].ToString()),
						PlanNum=PIn.Long(x["PlanNum"].ToString())
					}).GroupBy(x=>x.CarrierNum).ToDictionary(x=>x.Key,x=>x.ToList());
					if(dictCarrierPlans.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Invalid Carriers Referenced: ")+dictCarrierPlans.Count+"\r\n";
						log+=Lans.g("FormDatabaseMaintenance","Ins Plans Affected: ")+dictCarrierPlans.Values.Sum(x => x.Count)+"\r\n\r\n";
						foreach(var kvp in dictCarrierPlans) {
							log+="   "+Lans.g("FormDatabaseMaintenance","CarrierNum")+": "+kvp.Key+"\r\n";
							kvp.Value.ForEach(x => log+=string.Format("    PlanNum:{0}\r\n",x.PlanNum));
							log+="\r\n";
						}
					}
					break;
				case DbmMode.Fix:
					command="SELECT PlanNum FROM insplan WHERE CarrierNum NOT IN (SELECT CarrierNum FROM carrier)";
					table=Db.GetTable(command);
					if(table.Rows.Count>0) {
						Carrier carrier=Carriers.GetByNameAndPhone("UNKNOWN CARRIER","",true);
						command="UPDATE insplan SET CarrierNum="+POut.Long(carrier.CarrierNum)//set this new carrier for all insplans
							+" WHERE CarrierNum NOT IN (SELECT CarrierNum FROM carrier)";//which have invalid carriernums
						Db.NonQ(command);
					}
					int numberFixed=table.Rows.Count;
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Ins plans with carrier missing fixed: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>No need to pass in userNum, it's set before remoting role check and passed to the server if necessary.</summary>
		[DbmMethodAttr(HasBreakDown = true,HasUserNum = true,HasPatNum = true)]
		public static string InsPlanInvalidNum(bool verbose,DbmMode modeCur,long userNum = 0,long patNum = 0) {
			//Many sections removed because they are now fixed in InsSubNumMismatchPlanNum.
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				userNum=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur,userNum,patNum);
			}
			string log="";
			bool isPatSpecific=false;
			List<long> listPatPlanNums=new List<long>();
			List<long> listInsSubNums=new List<long>();
			List<long> listPlanNums=new List<long>();
			if(patNum > 0) {
				isPatSpecific=true;
				//Benefits need to check against PlanNums and PatPlanNums
				List<PatPlan> listPatPlans=PatPlans.Refresh(patNum);
				List<InsSub> listInsSubs=InsSubs.GetListForSubscriber(patNum);
				if(listPatPlans.Count < 1) {
					if(listInsSubs.Count < 1) {
						return Lans.g("FormDatabaseMaintenance","No insurance plans on file for patient.  Run the full DBM in order to fix any problems.");
					}
					listInsSubNums=listInsSubs.Select(x => x.InsSubNum).ToList();
					listPlanNums=listInsSubs.Select(x => x.PlanNum).ToList();
				}
				else {//PatPlans in the database
					listPatPlanNums=listPatPlans.Select(x => x.PatPlanNum).ToList();
					//Patients could have orphaned ins subs in the database, make sure to include those as well.
					listInsSubNums=listPatPlans.Select(x => x.InsSubNum)
						.Union(listInsSubs.Select(x => x.InsSubNum))
						.Distinct().ToList();
					listPlanNums=InsSubs.GetMany(listInsSubNums).Select(x => x.PlanNum).ToList();
				}
				if(listInsSubNums.Count < 1) {
					return Lans.g("FormDatabaseMaintenance","This patient has insurance plans that cannot be fixed on a patient specific level.\r\n"
						+"PatPlans: "+listPatPlanNums.Count+"  InsSubs: "+listInsSubNums.Count+"  PlanNums: "+listPlanNums.Count
						+"Run the full DBM in order to fix these problems.");
				}
			}
			switch(modeCur) {
				case DbmMode.Check:
					#region CHECK
					string command="SELECT COUNT(*) FROM appointment "
						+"WHERE appointment.InsPlan1 != 0 "
						+(isPatSpecific ? "AND appointment.PatNum="+POut.Long(patNum)+" " : "")
						+"AND NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=appointment.InsPlan1)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Invalid appointment InsPlan1 values: ")+numFound+"\r\n";
					}
					command="SELECT COUNT(*) FROM appointment "
						+"WHERE appointment.InsPlan2 != 0 "
						+(isPatSpecific ? "AND appointment.PatNum="+POut.Long(patNum)+" " : "")
						+"AND NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=appointment.InsPlan2)";
					numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Invalid appointment InsPlan2 values: ")+numFound+"\r\n";
					}
					command="SELECT COUNT(*) FROM benefit "
						+"WHERE PlanNum !=0 ";
					if(isPatSpecific) {
						if(listPlanNums.Count > 0) {
							command+="AND PlanNum IN ("+string.Join(",",listPlanNums)+") ";
						}
						else {
							command+="AND FALSE ";
						}
					}
					command+="AND NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=benefit.PlanNum)";
					numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Invalid benefit PlanNums: ")+numFound+"\r\n";
					}
					command="SELECT COUNT(*) FROM inssub "
						+"WHERE "+(isPatSpecific ? "InsSubNum IN("+string.Join(",",listInsSubNums)+") AND " : "")
						+"NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=inssub.PlanNum)";
					numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Invalid inssub PlanNums: ")+numFound+"\r\n";
					}
					#endregion CHECK
					break;
				case DbmMode.Breakdown:
					#region BREAKDOWN
					command="SELECT PatNum, AptNum, InsPlan1 FROM appointment "
						+"WHERE appointment.InsPlan1 != 0 "
						+(isPatSpecific ? "AND appointment.PatNum="+POut.Long(patNum)+" " : "")
						+"AND NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=appointment.InsPlan1)";
					var dictBadAppts1=Db.GetTable(command).Select().Select(x=>new {
						PatNum=PIn.Long(x["PatNum"].ToString()),
						AptNum=PIn.Long(x["AptNum"].ToString()),
						InsPlan1=PIn.Long(x["InsPlan1"].ToString())
					})
					.GroupBy(x=>x.PatNum).ToDictionary(x=>x.Key,x=>x.ToList());
					command="SELECT PatNum, AptNum, InsPlan2 FROM appointment "
						+"WHERE appointment.InsPlan2 != 0 "
						+(isPatSpecific ? "AND appointment.PatNum="+POut.Long(patNum)+" " : "")
						+"AND NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=appointment.InsPlan2)";
					var dictBadAppts2=Db.GetTable(command).Select().Select(x=>new {
						PatNum=PIn.Long(x["PatNum"].ToString()),
						AptNum=PIn.Long(x["AptNum"].ToString()),
						InsPlan2=PIn.Long(x["InsPlan2"].ToString())
					})
					.GroupBy(x=>x.PatNum).ToDictionary(x=>x.Key,x=>x.ToList());
					command="SELECT BenefitNum, PlanNum FROM benefit "
						+"WHERE PlanNum !=0 ";
					if(isPatSpecific) {
						if(listPlanNums.Count > 0) {
							command+="AND PlanNum IN ("+string.Join(",",listPlanNums)+") ";
						}
						else {
							command+="AND FALSE ";
						}
					}
					command+="AND NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=benefit.PlanNum)";
					var dictBadBenefitsByPlan=Db.GetTable(command).Select().Select(x=>new {
						//PatNum=PIn.Long(x["PatNum"].ToString()), //PatNum not available.
						PlanNum=PIn.Long(x["PlanNum"].ToString()),
						BenefitNum=PIn.Long(x["BenefitNum"].ToString())
					})
					.GroupBy(x=>x.PlanNum).ToDictionary(x=>x.Key,x=>x.ToList());
					command="SELECT Subscriber, InsSubNum, PlanNum FROM inssub "
						+"WHERE "+(isPatSpecific ? "InsSubNum IN("+string.Join(",",listInsSubNums)+") AND " : "")
						+"NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=inssub.PlanNum)";
					var dictBadInsSubs=Db.GetTable(command).Select().Select(x=>new {
						Subscriber=PIn.Long(x["Subscriber"].ToString()),
						InsSubNum=PIn.Long(x["InsSubNum"].ToString()),
						PlanNum=PIn.Long(x["PlanNum"].ToString())
					})
					.GroupBy(x=>x.Subscriber).ToDictionary(x=>x.Key,x=>x.ToList());
					List<long> distinctPatNums=dictBadAppts1.Keys
						.Union(dictBadAppts2.Keys)
						.Union(dictBadInsSubs.Keys)
						.ToList();
					List<Patient> listPatLims=Patients.GetLimForPats(distinctPatNums);
					numFound=dictBadAppts1.Values.Sum(x => x.Count)+
						dictBadAppts2.Values.Sum(x => x.Count)+
						dictBadInsSubs.Values.Sum(x => x.Count)+
						dictBadBenefitsByPlan.Values.Sum(x => x.Count);
					if(numFound>0||verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Invalid PlanNum references found : ")+numFound+"\r\n";
						log+=Lans.g("FormDatabaseMaintenance","Patients Affected : ")+distinctPatNums.Count+(dictBadBenefitsByPlan.Count>0 ? "+" : "")+"\r\n";
						foreach(Patient patLim in listPatLims) {
							string lineItemDBM="   "+Lans.g("FormDatabaseMaintenance","Patient with invalid PlanNums")+":"+patLim.GetNameLF()+" (PatNum:"+patLim.PatNum+")"+"\r\n";
							//No additional translation needed. All "words" exactly match Schema column names.
							if(dictBadAppts1.ContainsKey(patLim.PatNum)) {
								dictBadAppts1[patLim.PatNum].ForEach(x => lineItemDBM+=string.Format("    AptNum:{0} InsPlan1:{1}\r\n",x.AptNum,x.InsPlan1));
							}
							if(dictBadAppts2.ContainsKey(patLim.PatNum)) {
								dictBadAppts2[patLim.PatNum].ForEach(x => lineItemDBM+=string.Format("    AptNum:{0} InsPlan2:{1}\r\n",x.AptNum,x.InsPlan2));
							}
							if(dictBadInsSubs.ContainsKey(patLim.PatNum)) {
								dictBadInsSubs[patLim.PatNum].ForEach(x => lineItemDBM+=string.Format("    InsSubNum:{0} PlanNum:{1}\r\n",x.InsSubNum,x.PlanNum));
							}
							lineItemDBM+="\r\n";
							log+=lineItemDBM;
						}
						foreach(var kvp in dictBadBenefitsByPlan) {
							string lineItemDBM="   "+Lans.g("FormDatabaseMaintenance","Invalid plan with attached benefits")+": PlanNum:"+kvp.Key+"\r\n";
							//No additional translation needed. All "words" exactly match Schema column names.
							lineItemDBM+="    BenefitNums:"+string.Join(", ",kvp.Value.Select(x => x.BenefitNum))+"\r\n\r\n";
							log+=lineItemDBM;
						}
					}
					#endregion BREAKDOWN
					break;
				case DbmMode.Fix:
					#region FIX
					//One option will sometimes be to create a dummy plan to attach these things to, be we have not had to implement that yet.  
					//We need databases with actual problems to test these fixes against.
					//appointment.InsPlan1-----------------------------------------------------------------------------------------------
					command="UPDATE appointment SET InsPlan1=0 "
						+"WHERE InsPlan1 != 0 "
						+(isPatSpecific ? "AND appointment.PatNum="+POut.Long(patNum)+" " : "")
						+"AND NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=appointment.InsPlan1)";
					long numFixed=Db.NonQ(command);
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Invalid appointment InsPlan1 values fixed: ")+numFixed+"\r\n";
					}
					//appointment.InsPlan2-----------------------------------------------------------------------------------------------
					command="UPDATE appointment SET InsPlan2=0 "
						+"WHERE InsPlan2 != 0 "
						+(isPatSpecific ? "AND appointment.PatNum="+POut.Long(patNum)+" " : "")
						+"AND NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=appointment.InsPlan2)";
					numFixed=Db.NonQ(command);
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Invalid appointment InsPlan2 values fixed: ")+numFixed+"\r\n";
					}
					//benefit.PlanNum----------------------------------------------------------------------------------------------------
					command="DELETE FROM benefit "
						+"WHERE PlanNum !=0 ";
					if(isPatSpecific) {
						if(listPlanNums.Count > 0) {
							command+="AND PlanNum IN ("+string.Join(",",listPlanNums)+") ";
						}
						else {
							command+="AND FALSE ";
						}
					}
					command+="AND NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=benefit.PlanNum)";
					numFixed=Db.NonQ(command);
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Invalid benefit PlanNums fixed: ")+numFixed+"\r\n";
					}
					//inssub.PlanNum------------------------------------------------------------------------------------------------------
					numFixed=0;
					//1: PlanNum=0
					command="SELECT InsSubNum FROM inssub WHERE PlanNum=0 "+(isPatSpecific ? "AND InsSubNum IN("+string.Join(",",listInsSubNums)+")" : "");
					DataTable table=Db.GetTable(command);
					for(int i = 0;i<table.Rows.Count;i++) {
						long insSubNum=PIn.Long(table.Rows[i]["InsSubNum"].ToString());
						command="SELECT COUNT(*) FROM claim WHERE InsSubNum="+POut.Long(insSubNum);
						int countUsed=PIn.Int(Db.GetCount(command));
						command="SELECT COUNT(*) FROM claimproc WHERE InsSubNum="+POut.Long(insSubNum)+" "
							+"AND (ClaimNum<>0 OR (Status<>6 AND Status<>3))";//attached to a claim or (not an estimate or adjustment)
						countUsed+=PIn.Int(Db.GetCount(command));
						command="SELECT COUNT(*) FROM etrans WHERE InsSubNum="+POut.Long(insSubNum);
						countUsed+=PIn.Int(Db.GetCount(command));
						//command="SELECT COUNT(*) FROM patplan WHERE InsSubNum="+POut.Long(insSubNum);
						//countUsed+=PIn.Int(Db.GetCount(command));
						command="SELECT COUNT(*) FROM payplan WHERE InsSubNum="+POut.Long(insSubNum);
						countUsed+=PIn.Int(Db.GetCount(command));
						if(countUsed==0) {
							command="DELETE FROM claimproc WHERE InsSubNum="+POut.Long(insSubNum)+" "
								+"AND ClaimNum=0 AND (Status=6 OR Status=3)";//ok to delete because no claim and just an estimate or adjustment
							Db.NonQ(command);
							InsSubs.ClearFkey(insSubNum);
							command="DELETE FROM inssub WHERE InsSubNum="+POut.Long(insSubNum);
							Db.NonQ(command);
							command="DELETE FROM patplan WHERE InsSubNum="+POut.Long(insSubNum);//It's very safe to "drop coverage" for a patient.
							Db.NonQ(command);
							numFixed++;
							continue;
						}
					}
					//2: PlanNum invalid
					command="SELECT InsSubNum,PlanNum FROM inssub "
						+"WHERE "+(isPatSpecific ? "InsSubNum IN("+string.Join(",",listInsSubNums)+") AND " : "")
						+"NOT EXISTS(SELECT * FROM insplan WHERE insplan.PlanNum=inssub.PlanNum)";
					table=Db.GetTable(command);
					for(int i = 0;i<table.Rows.Count;i++) {
						long planNum=PIn.Long(table.Rows[i]["PlanNum"].ToString());
						long insSubNum=PIn.Long(table.Rows[i]["InsSubNum"].ToString());
						command="SELECT COUNT(*) FROM claim WHERE InsSubNum="+POut.Long(insSubNum);
						int countUsed=PIn.Int(Db.GetCount(command));
						command="SELECT COUNT(*) FROM claimproc WHERE InsSubNum="+POut.Long(insSubNum);
						countUsed+=PIn.Int(Db.GetCount(command));
						command="SELECT COUNT(*) FROM etrans WHERE InsSubNum="+POut.Long(insSubNum);
						countUsed+=PIn.Int(Db.GetCount(command));
						command="SELECT COUNT(*) FROM patplan WHERE InsSubNum="+POut.Long(insSubNum);
						countUsed+=PIn.Int(Db.GetCount(command));
						command="SELECT COUNT(*) FROM payplan WHERE InsSubNum="+POut.Long(insSubNum);
						countUsed+=PIn.Int(Db.GetCount(command));
						//planNum
						command="SELECT COUNT(*) FROM benefit WHERE PlanNum="+POut.Long(planNum);
						countUsed+=PIn.Int(Db.GetCount(command));
						command="SELECT COUNT(*) FROM claim WHERE PlanNum="+POut.Long(planNum);
						countUsed+=PIn.Int(Db.GetCount(command));
						command="SELECT COUNT(*) FROM claimproc WHERE PlanNum="+POut.Long(planNum);
						countUsed+=PIn.Int(Db.GetCount(command));
						if(countUsed==0) {//There are no other pointers to this invalid plannum or this inssub, delete this inssub
							InsSubs.ClearFkey(insSubNum);
							command="DELETE FROM inssub WHERE InsSubNum="+POut.Long(insSubNum);
							Db.NonQ(command);
							numFixed++;
							continue;
						}
						else {//There are objects referencing this inssub or this insplan.  Insert a dummy plan linked to a dummy carrier with CarrierName=Unknown
							InsPlan insplan=new InsPlan();
							insplan.IsHidden=true;
							insplan.CarrierNum=Carriers.GetByNameAndPhone("UNKNOWN CARRIER","",true).CarrierNum;
							insplan.SecUserNumEntry=userNum;
							long insPlanNum=InsPlans.Insert(insplan);
							command="UPDATE inssub SET PlanNum="+POut.Long(insPlanNum)+" WHERE InsSubNum="+POut.Long(insSubNum);
							Db.NonQ(command);
							numFixed++;
							continue;
						}
					}
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Invalid inssub PlanNums fixed: ")+numFixed+"\r\n";
					}
					#endregion FIX
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string InsPlanNoClaimForm(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM insplan WHERE ClaimFormNum=0";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Insplan claimforms missing: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="UPDATE insplan SET ClaimFormNum="+POut.Long(PrefC.GetLong(PrefName.DefaultClaimForm))
						+" WHERE ClaimFormNum=0";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Insplan claimforms set if missing: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>No need to pass in userNum, it's set before remoting role check and passed to the server if necessary.</summary>
		[DbmMethodAttr(HasUserNum = true)]
		public static string InsSubInvalidSubscriber(bool verbose,DbmMode modeCur,long userNum = 0) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				userNum=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur,userNum);
			}
			string command=@"SELECT inssub.Subscriber,COALESCE(patient.PatStatus,-1) PatStatus FROM inssub 
				LEFT JOIN patient ON patient.PatNum=inssub.Subscriber
				WHERE (patient.PatNum IS NULL OR patient.PatStatus="+POut.Int((int)PatientStatus.Deleted)+@")
				AND inssub.Subscriber != 0 
				GROUP BY inssub.Subscriber";
			DataTable table=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					int numFound=table.Rows.Count;
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","InsSub subscribers missing: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					long priProv=PrefC.GetLong(PrefName.PracticeDefaultProv);
					long billType=PrefC.GetLong(PrefName.PracticeDefaultBillType);
					for(int i = 0;i<table.Rows.Count;i++) {
						if(PIn.Int(table.Rows[i]["PatStatus"].ToString())==(int)PatientStatus.Deleted) {//The patient exists in the db but is deleted.
																																														//Change the patient to Archived.
							Patient pat=Patients.GetPat(PIn.Long(table.Rows[i]["Subscriber"].ToString()));
							Patient patOld=pat.Copy();
							pat.PatStatus=PatientStatus.Archived;
							Patients.Update(pat,patOld);
							SecurityLogs.MakeLogEntry(Permissions.PatientEdit,pat.PatNum,
								"Patient status changed from 'Deleted' to 'Archived' from DBM fix for InsSubInvalidSubscriber.",LogSources.DBM);
						}
						else {//The patient does not exist in the db at all.
									//Create dummy patients using the FKs that the Subscriber column is expecting.
							Patient pat=new Patient();
							pat.PatNum=PIn.Long(table.Rows[i]["Subscriber"].ToString());
							pat.LName="UNKNOWN";
							pat.FName="Unknown";
							pat.Guarantor=pat.PatNum;
							pat.PriProv=priProv;
							pat.BillingType=billType;
							pat.SecUserNumEntry=userNum;
							Patients.Insert(pat,true);
							SecurityLogs.MakeLogEntry(Permissions.PatientCreate,pat.PatNum,"Recreated from DBM fix for InsSubInvalidSubscriber.",LogSources.DBM);
						}
					}
					int numberFixed=table.Rows.Count;
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","InsSub subscribers fixed: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>Checks for situations where there are valid InsSubNums, but mismatched PlanNums.
		///No need to pass in userNum, it's set before remoting role check and passed to the server if necessary.</summary>
		[DbmMethodAttr(HasUserNum = true,HasBreakDown = true)]
		public static string InsSubNumMismatchPlanNum(bool verbose,DbmMode modeCur,long userNum = 0) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				userNum=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur,userNum);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			string log="";
			//Not going to validate the following tables because they do not have an InsSubNum column: appointmentx2, benefit.
			//This DBM assumes that the inssub table is correct because that's what we're comparing against.
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					#region CHECK
					int numFound=0;
					bool hasBreakDown=false;
					//Can't do the following because no inssubnum: appointmentx2, benefit.
					//Can't do inssub because that's what we're comparing against.  That's the one that's assumed to be correct.
					//claim.PlanNum -----------------------------------------------------------------------------------------------------
					command="SELECT COUNT(*) FROM claim "
						+"WHERE PlanNum NOT IN (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claim.InsSubNum) ";
					numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Mismatched claim InsSubNum/PlanNum values")+": "+numFound+"\r\n";
						if(numFound>0) {
							hasBreakDown=true;
						}
					}
					//claim.PlanNum2---------------------------------------------------------------------------------------------------
					command="SELECT COUNT(*) FROM claim "
						+"WHERE PlanNum2 != 0 "
						+"AND PlanNum2 NOT IN (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claim.InsSubNum2)";
					numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Mismatched claim InsSubNum2/PlanNum2 values")+": "+numFound+"\r\n";
						if(numFound>0) {
							hasBreakDown=true;
						}
					}
					//claimproc---------------------------------------------------------------------------------------------------
					command="SELECT COUNT(*) FROM claimproc "
						+"WHERE PlanNum NOT IN (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claimproc.InsSubNum)";
					numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Mismatched claimproc InsSubNum/PlanNum values")+": "+numFound+"\r\n";
						if(numFound>0) {
							hasBreakDown=true;
						}
					}
					//etrans---------------------------------------------------------------------------------------------------
					command="SELECT COUNT(*) FROM etrans "
						+"WHERE PlanNum!=0 AND PlanNum NOT IN (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=etrans.InsSubNum)";
					numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Mismatched etrans InsSubNum/PlanNum values")+": "+numFound+"\r\n";
						if(numFound>0) {
							hasBreakDown=true;
						}
					}
					//payplan---------------------------------------------------------------------------------------------------
					command="SELECT COUNT(*) FROM payplan "
						+"WHERE EXISTS (SELECT PlanNum FROM inssub WHERE inssub.InsSubNum=payplan.InsSubNum AND inssub.PlanNum!=payplan.PlanNum)";
					numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Mismatched payplan InsSubNum/PlanNum values")+": "+numFound+"\r\n";
						if(numFound>0) {
							hasBreakDown=true;
						}
					}
					if(hasBreakDown) {
						log+="   "+Lans.g("FormDatabaseMaintenance","Run Fix or double click to see a break down.");
					}
					break;
				#endregion CHECK
				case DbmMode.Breakdown:
					#region BREAKDOWN
					//In this BREAKDOWN, when user double clicks on this DBM query and show what needs to be fixed/will attempted to be fixed when running Fix.
					//claim.PlanNum -----------------------------------------------------------------------------------------------------
					command="SELECT claim.PatNum,claim.PlanNum,claim.ClaimNum,claim.DateService FROM claim "
						+"WHERE PlanNum NOT IN (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claim.InsSubNum) ";
					var dictBadClaims1=Db.GetTable(command).Select().Select(x=>new {
						PatNum=PIn.Long(x["PatNum"].ToString()),
						PlanNum=PIn.Long(x["PlanNum"].ToString()),
						ClaimNum=PIn.Long(x["ClaimNum"].ToString()),
						DateService=PIn.DateT(x["DateService"].ToString())
					}).GroupBy(x=>x.PatNum)
					.ToDictionary(x=>x.Key,x=>x.ToList());
					//claim.PlanNum2---------------------------------------------------------------------------------------------------
					command="SELECT claim.PatNum,claim.PlanNum,claim.ClaimNum,claim.DateService FROM claim "
						+"WHERE PlanNum2 != 0 "
						+"AND PlanNum2 NOT IN (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claim.InsSubNum2)";
					var dictBadClaims2=Db.GetTable(command).Select().Select(x=>new {
						PatNum=PIn.Long(x["PatNum"].ToString()),
						PlanNum2=PIn.Long(x["PlanNum2"].ToString()),
						ClaimNum=PIn.Long(x["ClaimNum"].ToString()),
						DateService=PIn.DateT(x["DateService"].ToString())
					}).GroupBy(x=>x.PatNum)
					.ToDictionary(x=>x.Key,x=>x.ToList());
					//claimproc---------------------------------------------------------------------------------------------------
					command="SELECT claimproc.PatNum,claimproc.ClaimProcNum,claimproc.InsSubNum,claimproc.ProcNum,claimproc.ClaimNum FROM claimproc "
						+"WHERE PlanNum NOT IN (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claimproc.InsSubNum)";
					var dictBadClaimProcs=Db.GetTable(command).Select().Select(x=>new {
						PatNum=PIn.Long(x["PatNum"].ToString()),
						ClaimProcNum=PIn.Long(x["ClaimProcNum"].ToString()),
						InsSubNum=PIn.Long(x["InsSubNum"].ToString()),
						ProcNum=PIn.Long(x["ProcNum"].ToString()),
						ClaimNum=PIn.Long(x["ClaimNum"].ToString())
					}).GroupBy(x=>x.PatNum)
					.ToDictionary(x=>x.Key,x=>x.ToList());
					//etrans---------------------------------------------------------------------------------------------------
					command="SELECT etrans.PatNum,etrans.PlanNum,etrans.EtransNum,etrans.DateTimeTrans FROM etrans "
						+"WHERE PlanNum!=0 AND PlanNum NOT IN (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=etrans.InsSubNum)";
					var dictBadEtrans=Db.GetTable(command).Select().Select(x=>new {
						PatNum=PIn.Long(x["PatNum"].ToString()),
						PlanNum=PIn.Long(x["PlanNum"].ToString()),
						EtransNum=PIn.Long(x["EtransNum"].ToString()),
						DateTimeTrans=PIn.DateT(x["DateTimeTrans"].ToString())
					}).GroupBy(x=>x.PatNum)
					.ToDictionary(x=>x.Key,x=>x.ToList());
					//payplan---------------------------------------------------------------------------------------------------
					command="SELECT payplan.PatNum,payplan.PlanNum,payplan.PayPlanNum FROM payplan "
						+"WHERE EXISTS (SELECT PlanNum FROM inssub WHERE inssub.InsSubNum=payplan.InsSubNum AND inssub.PlanNum!=payplan.PlanNum)";
					var dictBadPayPlans=Db.GetTable(command).Select().Select(x=>new {
						PatNum=PIn.Long(x["PatNum"].ToString()),
						PlanNum=PIn.Long(x["PlanNum"].ToString()),
						PayPlanNum=PIn.Long(x["PayPlanNum"].ToString())
					}).GroupBy(x=>x.PatNum)
					.ToDictionary(x=>x.Key,x=>x.ToList());
					List<long> distinctPatNums=dictBadClaims1.Keys
						.Union(dictBadClaims2.Keys)
						.Union(dictBadClaimProcs.Keys)
						.Union(dictBadEtrans.Keys)
						.Union(dictBadPayPlans.Keys)
						.ToList();
					List<Patient> listPatLims=Patients.GetLimForPats(distinctPatNums);
					numFound=dictBadClaims1.Values.Sum(x => x.Count)+
						dictBadClaims2.Values.Sum(x => x.Count)+
						dictBadClaimProcs.Values.Sum(x => x.Count)+
						dictBadEtrans.Values.Sum(x => x.Count)+
						dictBadPayPlans.Values.Sum(x => x.Count);
					if(numFound>0||verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Mismatched InsSubNum/PlanNum values")+": "+numFound+"\r\n";
						log+=Lans.g("FormDatabaseMaintenance","Patients affected")+": "+distinctPatNums.Count+"\r\n";
						foreach(Patient patLim in listPatLims) {
							string lineItemDBM="   "+Lans.g("FormDatabaseMaintenance","Patient with associated invalid PlanNums")+":"+patLim.GetNameLF()+" (PatNum:"+patLim.PatNum+")"+"\r\n";
							//No additional translation needed. All "words" exactly match Schema column names.
							if(dictBadClaims1.ContainsKey(patLim.PatNum)) {
								dictBadClaims1[patLim.PatNum].ForEach(x => lineItemDBM+=string.Format("    ClaimNum:{0} PlanNum:{1} DateService:{2}\r\n",x.ClaimNum,x.PlanNum,x.DateService.ToShortDateString()));
							}
							if(dictBadClaims2.ContainsKey(patLim.PatNum)) {
								dictBadClaims2[patLim.PatNum].ForEach(x => lineItemDBM+=string.Format("    ClaimNum:{0} PlanNum2:{1} DateService:{2}\r\n",x.ClaimNum,x.PlanNum2,x.DateService.ToShortDateString()));
							}
							if(dictBadClaimProcs.ContainsKey(patLim.PatNum)) {
								dictBadClaimProcs[patLim.PatNum].ForEach(x => lineItemDBM+=string.Format("    ClaimProcNum:{0} InsSubNum:{1} ClaimNum:{2} ProcNum:{3}\r\n",x.ClaimProcNum,x.InsSubNum,x.ClaimNum,x.ProcNum));
							}
							if(dictBadEtrans.ContainsKey(patLim.PatNum)) {
								dictBadEtrans[patLim.PatNum].ForEach(x => lineItemDBM+=string.Format("    ETransNum:{0} PlanNum:{1} DateTimeTrans:{2}\r\n",x.EtransNum,x.PlanNum,x.DateTimeTrans.ToString()));
							}
							if(dictBadPayPlans.ContainsKey(patLim.PatNum)) {
								dictBadPayPlans[patLim.PatNum].ForEach(x => lineItemDBM+=string.Format("    PayPlanNum:{0} PlanNum:{1}\r\n",x.PayPlanNum,x.PlanNum));
							}
							lineItemDBM+="\r\n";
							log+=lineItemDBM;
						}
					}
					break;
				#endregion BREAKDOWN
				case DbmMode.Fix:
					#region FIX
					long numFixed=0;
					#region Claim PlanNum
					#region claim.PlanNum (1/4) Mismatch
					command="UPDATE claim SET PlanNum=(SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claim.InsSubNum) "
						+"WHERE PlanNum != (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claim.InsSubNum)";
					numFixed=Db.NonQ(command);
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Mismatched claim InsSubNum/PlanNum fixed")+": "+numFixed.ToString()+"\r\n";
					}
					#endregion
					numFixed=0;
					#region claim.PlanNum (2/4) PlanNum zero, invalid InsSubNum
					//Will leave orphaned claimprocs. No finanicals to check.
					command="SELECT claim.ClaimNum FROM claim WHERE PlanNum=0 AND ClaimStatus IN ('PreAuth','W','U') "
						+"AND NOT EXISTS(SELECT * FROM inssub WHERE inssub.InsSubNum=claim.InsSubNum)";
					DataTable tableClaimNums=Db.GetTable(command);
					List<long> listClaimNums=new List<long>();
					for(int i = 0;i<tableClaimNums.Rows.Count;i++) {
						listClaimNums.Add(PIn.Long(tableClaimNums.Rows[i]["ClaimNum"].ToString()));
					}
					if(listClaimNums.Count>0) {
						Claims.ClearFkey(listClaimNums);//Zero securitylog FKey column for rows to be deleted.
					}
					command="DELETE FROM claim WHERE PlanNum=0 AND ClaimStatus IN ('PreAuth','W','U') AND NOT EXISTS(SELECT * FROM inssub WHERE inssub.InsSubNum=claim.InsSubNum)";
					numFixed=Db.NonQ(command);
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claims deleted with invalid InsSubNum and PlanNum=0")+": "+numFixed.ToString()+"\r\n";
					}
					#endregion
					numFixed=0;
					#region claim.PlanNum (3/4) PlanNum invalid, and claim.InsSubNum invalid
					command="SELECT claim.PatNum,claim.PlanNum,claim.InsSubNum FROM claim "
						+"WHERE PlanNum NOT IN (SELECT insplan.PlanNum FROM insplan) "
						+"AND InsSubNum NOT IN (SELECT inssub.InsSubNum FROM inssub) ";
					DataTable table=Db.GetTable(command);
					if(table.Rows.Count>0) {
						log+=Lans.g("FormDatabaseMaintenance","List of patients who will need insurance information reentered:")+"\r\n";
					}
					for(int i = 0;i<table.Rows.Count;i++) {//Create simple InsPlans and InsSubs for each claim to replace the missing ones.
																								 //make sure a plan does not exist from a previous insert in this loop
						command="SELECT COUNT(*) FROM insplan WHERE PlanNum=" + table.Rows[i]["PlanNum"].ToString();
						if(Db.GetCount(command)=="0") {
							InsPlan plan=new InsPlan();
							plan.PlanNum=PIn.Long(table.Rows[i]["PlanNum"].ToString());//reuse the existing FK
							plan.IsHidden=true;
							plan.CarrierNum=Carriers.GetByNameAndPhone("UNKNOWN CARRIER","",true).CarrierNum;
							plan.SecUserNumEntry=userNum;
							InsPlans.Insert(plan,true);
						}
						long patNum=PIn.Long(table.Rows[i]["PatNum"].ToString());
						//make sure an inssub does not exist from a previous insert in this loop
						command="SELECT COUNT(*) FROM inssub WHERE InsSubNum=" + table.Rows[i]["InsSubNum"].ToString();
						if(Db.GetCount(command)=="0") {
							InsSub sub=new InsSub();
							sub.InsSubNum=PIn.Long(table.Rows[i]["InsSubNum"].ToString());//reuse the existing FK
							sub.PlanNum=PIn.Long(table.Rows[i]["PlanNum"].ToString());
							sub.Subscriber=patNum;//if this sub was created on a previous loop, this may be some other patient.
							sub.SubscriberID="unknown";
							sub.SecUserNumEntry=userNum;
							InsSubs.Insert(sub,true);
						}
						Patient pat=Patients.GetLim(patNum);
						log+="PatNum: "+pat.PatNum+" - "+Patients.GetNameFL(pat.LName,pat.FName,pat.Preferred,pat.MiddleI)+"\r\n";
					}
					numFixed=table.Rows.Count;
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claims with invalid PlanNums and invalid InsSubNums fixed: ")+numFixed.ToString()+"\r\n";
					}
					#endregion
					numFixed=0;
					#region claim.PlanNum (4/4) PlanNum valid, but claim.InsSubNum invalid
					command="SELECT PatNum,PlanNum,InsSubNum FROM claim "
						+"WHERE PlanNum IN (SELECT insplan.PlanNum FROM insplan) "
						+"AND InsSubNum NOT IN (SELECT inssub.InsSubNum FROM inssub) GROUP BY InsSubNum";
					table=Db.GetTable(command);
					//Create a dummy inssub and link it to the valid plan.
					for(int i = 0;i<table.Rows.Count;i++) {
						InsSub sub=new InsSub();
						sub.InsSubNum=PIn.Long(table.Rows[i]["InsSubNum"].ToString());
						sub.PlanNum=PIn.Long(table.Rows[i]["PlanNum"].ToString());
						sub.Subscriber=PIn.Long(table.Rows[i]["PatNum"].ToString());
						sub.SubscriberID="unknown";
						sub.SecUserNumEntry=userNum;
						InsSubs.Insert(sub,true);
					}
					numFixed=table.Rows.Count;
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claims with invalid InsSubNums and invalid PlanNums fixed: ")+numFixed.ToString()+"\r\n";
					}
					#endregion
					#endregion
					numFixed=0;
					#region Claim PlanNum2
					//claim.PlanNum2---------------------------------------------------------------------------------------------------
					command="UPDATE claim SET PlanNum2=(SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claim.InsSubNum2) "
						+"WHERE PlanNum2 != 0 "
						+"AND PlanNum2 NOT IN (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claim.InsSubNum2)";
					//if InsSubNum2 was completely invalid, then PlanNum2 gets set to zero here.
					numFixed=Db.NonQ(command);
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Mismatched claim InsSubNum2/PlanNum2 fixed: ")+numFixed.ToString()+"\r\n";
					}
					#endregion
					numFixed=0;
					#region ClaimProc
					//claimproc (1/2) If planNum is valid but InsSubNum does not exist, then add a dummy inssub----------------------------------------
					command="SELECT PatNum,PlanNum,InsSubNum FROM claimproc "
						+"WHERE PlanNum IN (SELECT insplan.PlanNum FROM insplan) "
						+"AND InsSubNum NOT IN (SELECT inssub.InsSubNum FROM inssub) GROUP BY InsSubNum";
					table=Db.GetTable(command);
					//Create a dummy inssub and link it to the valid plan.
					for(int i = 0;i<table.Rows.Count;i++) {
						InsSub sub=new InsSub();
						sub.InsSubNum=PIn.Long(table.Rows[i]["InsSubNum"].ToString());
						sub.PlanNum=PIn.Long(table.Rows[i]["PlanNum"].ToString());
						sub.Subscriber=PIn.Long(table.Rows[i]["PatNum"].ToString());
						sub.SubscriberID="unknown";
						sub.SecUserNumEntry=userNum;
						InsSubs.Insert(sub,true);
					}
					numFixed=table.Rows.Count;
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claims with valid PlanNums and invalid InsSubNums fixed: ")+numFixed.ToString()+"\r\n";
					}
					numFixed=0;
					//claimproc (2/2) Mismatch, but InsSubNum is valid
					command="UPDATE claimproc SET PlanNum=(SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claimproc.InsSubNum) "
						+"WHERE PlanNum != (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=claimproc.InsSubNum)";
					numFixed=Db.NonQ(command);
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Mismatched claimproc InsSubNum/PlanNum fixed: ")+numFixed.ToString()+"\r\n";
					}
					numFixed=0;
					//claimproc.PlanNum zero, invalid InsSubNum--------------------------------------------------------------------------------
					command="DELETE FROM claimproc WHERE PlanNum=0 AND NOT EXISTS(SELECT * FROM inssub WHERE inssub.InsSubNum=claimproc.InsSubNum)"
						+" AND InsPayAmt=0 AND WriteOff=0"//Make sure this deletion will not affect financials.
						+" AND Status IN (6,2)";//OK to delete because no claim and just an estimate (6) or preauth (2) claimproc
					numFixed=Db.NonQ(command);
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Claimprocs deleted with invalid InsSubNum and PlanNum=0: ")+numFixed.ToString()+"\r\n";
					}
					#endregion
					numFixed=0;
					#region Etrans
					//etrans---------------------------------------------------------------------------------------------------
					command="UPDATE etrans SET PlanNum=(SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=etrans.InsSubNum) "
						+"WHERE PlanNum!=0 AND PlanNum != (SELECT inssub.PlanNum FROM inssub WHERE inssub.InsSubNum=etrans.InsSubNum)";
					numFixed=Db.NonQ(command);
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Mismatched etrans InsSubNum/PlanNum fixed: ")+numFixed.ToString()+"\r\n";
					}
					#endregion
					numFixed=0;
					#region PayPlan
					//payplan--------------------------------------------------------------------------------------------------
					command="UPDATE payplan SET PlanNum=(SELECT PlanNum FROM inssub WHERE inssub.InsSubNum=payplan.InsSubNum) "
						+"WHERE EXISTS (SELECT PlanNum FROM inssub WHERE inssub.InsSubNum=payplan.InsSubNum AND inssub.PlanNum!=payplan.PlanNum)";
					numFixed=Db.NonQ(command);
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Mismatched payplan InsSubNum/PlanNum fixed: ")+numFixed.ToString()+"\r\n";
					}
					#endregion
					numFixed=0;
					#endregion FIX
					break;
			}
			return log;
		}

		#endregion InsPayPlan, InsPlan, InsSub----------------------------------------------------------------------------------------------------------
		#region JournalEntry, LabCase, Laboratory-------------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string JournalEntryInvalidAccountNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT COUNT(*) FROM journalentry WHERE AccountNum NOT IN(SELECT AccountNum FROM account)";
			int numFound=PIn.Int(Db.GetCount(command));
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(numFound > 0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Transactions found attached to an invalid account")+": "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					if(numFound > 0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Transactions found attached to an invalid account")+": "+numFound+"\r\n";
					}
					if(numFound > 0) {
						//Check to see if there is already an active account called UNKNOWN.
						command="SELECT AccountNum FROM account WHERE Description='UNKNOWN' AND Inactive=0";
						long accountNum=PIn.Long(Db.GetScalar(command));
						if(accountNum==0) {
							//Create a new Account called UNKNOWN.
							Account account=new Account();
							account.Description="UNKNOWN";
							account.Inactive=false;//Just in case.
							account.AcctType=AccountType.Asset;//Default account type.  This DBM check was added to fix orphaned automatic payment journal entries, which should have been associated to an income account.
							accountNum=Accounts.Insert(account);
						}
						//Update the journalentry table.
						command="UPDATE journalentry SET AccountNum="+POut.Long(accountNum)+" WHERE AccountNum NOT IN(SELECT AccountNum FROM account)";
						Db.NonQ(command);
						log+=Lans.g("FormDatabaseMaintenance","   All invalid transactions have been attached to the account called UNKNOWN.")+"\r\n";
						log+=Lans.g("FormDatabaseMaintenance","   They need to be fixed manually.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string LabCaseWithInvalidLaboratory(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM labcase WHERE laboratoryNum NOT IN(SELECT laboratoryNum FROM laboratory)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Lab cases found with invalid laboratories")+": "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="SELECT COUNT(*) FROM labcase WHERE laboratoryNum NOT IN(SELECT laboratoryNum FROM laboratory)";
					long numberFixed=long.Parse(Db.GetCount(command));
					command="SELECT * FROM labcase WHERE laboratoryNum NOT IN(SELECT laboratoryNum FROM laboratory) GROUP BY LaboratoryNum";
					DataTable table=Db.GetTable(command);
					long labnum;
					for(int i = 0;i<table.Rows.Count;i++) {
						Laboratory lab=new Laboratory();
						lab.LaboratoryNum=PIn.Long(table.Rows[i]["LaboratoryNum"].ToString());
						lab.Description="Laboratory "+table.Rows[i]["LaboratoryNum"].ToString();
						//laboratoryNum is not allowed to be zero
						labnum=Crud.LaboratoryCrud.Insert(lab);
						command="UPDATE labcase SET LaboratoryNum="+POut.Long(labnum)+" WHERE LaboratoryNum="+table.Rows[i]["LaboratoryNum"].ToString();
						Db.NonQ(command);
					}
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Lab cases fixed with invalid laboratories")+": "+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string LaboratoryWithInvalidSlip(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM laboratory WHERE Slip NOT IN(SELECT SheetDefNum FROM sheetdef) AND Slip != 0";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Laboratories found with invalid lab slips")+": "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="UPDATE laboratory SET Slip=0 WHERE Slip NOT IN(SELECT SheetDefNum FROM sheetdef) AND Slip != 0";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Laboratories fixed with invalid lab slips")+": "+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		#endregion JournalEntry, LabCase, Laboratory----------------------------------------------------------------------------------------------------
		#region MedicationPat, Medication, MessageButton------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string MedicationPatDeleteWithInvalidMedNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM medicationpat WHERE "//We no longer delete medicationpats where MedicationNum is 0 because we allow MedicationNums to be 0 for use in MU2.
						+"(medicationpat.MedicationNum<>0 AND NOT EXISTS(SELECT * FROM medication WHERE medication.MedicationNum=medicationpat.MedicationNum))";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Medications found where no defition exists for them: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="DELETE FROM medicationpat WHERE "//We no longer delete medicationpats where MedicationNum is 0 because we allow MedicationNums to be 0 for use in MU2.
						+"(medicationpat.MedicationNum<>0 AND NOT EXISTS(SELECT * FROM medication WHERE medication.MedicationNum=medicationpat.MedicationNum))";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Medications deleted because no definition exists for them: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string MedicationWithInvalidGenericNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command=@"SELECT COUNT(*) FROM medication WHERE GenericNum NOT IN (SELECT MedicationNum FROM medication)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Medications with missing generic brand found: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					List<Medication> listMeds;
					//Select into list because the following query is not valid in MySQL
					//UPDATE medication SET GenericNum=MedicationNum WHERE GenericNum NOT IN (SELECT MedicationNum FROM medication)
					command="SELECT * FROM medication WHERE GenericNum NOT IN (SELECT MedicationNum FROM medication)";
					listMeds=Crud.MedicationCrud.SelectMany(command);
					for(int i = 0;i<listMeds.Count;i++) {
						listMeds[i].GenericNum=listMeds[i].MedicationNum;
						Medications.Update(listMeds[i]);
					}
					if(listMeds.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Medications with missing generic brand fixed: ")+listMeds.Count.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string MessageButtonDuplicateButtonIndex(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			string queryStr="SELECT COUNT(*) NumFound,SigButDefNum,ButtonIndex,ComputerName FROM sigbutdef GROUP BY ComputerName,ButtonIndex HAVING COUNT(*) > 1";
			DataTable table=Db.GetTable(queryStr);
			int numFound=table.Select().Sum(x => PIn.Int(x["NumFound"].ToString())-1);//sum the duplicates; one in each group is valid.
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Messaging buttons found with invalid button orders")+": "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					do {
						//Loop through the messaging buttons and increment the duplicate button index by the max plus one.
						for(int i = 0;i<table.Rows.Count;i++) {
							command="SELECT MAX(ButtonIndex) FROM sigbutdef WHERE ComputerName='"+table.Rows[i]["ComputerName"].ToString()+"'";
							int newIndex=PIn.Int(Db.GetScalar(command))+1;
							command="UPDATE sigbutdef SET ButtonIndex="+newIndex.ToString()+" WHERE SigButDefNum="+table.Rows[i]["SigButDefNum"].ToString();
							Db.NonQ(command);
						}
						//It's possible we need to loop through several more times depending on how many items shared the same button index. 
						table=Db.GetTable(queryStr);
					} while(table.Rows.Count > 0);
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Messaging buttons with invalid button orders fixed")+": "+numFound.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		#endregion MedicationPat, Medication, MessageButton---------------------------------------------------------------------------------------------
		#region Operatory, OrthoChart, PatField---------------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string OperatoryInvalidReference(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			//Get distinct operatory nums that have been orphaned from appointment, scheduleop, and apptviewitems.  
			//We use a UNION instead of UNION ALL because we want MySQL or Oracle to group duplicate OpNums together.
			string command=@"SELECT appointment.Op AS OpNum FROM appointment WHERE appointment.Op!=0 AND NOT EXISTS(SELECT * FROM operatory WHERE operatory.OperatoryNum=appointment.Op)
									UNION 
									SELECT scheduleop.OperatoryNum AS OpNum FROM scheduleop WHERE scheduleop.OperatoryNum!=0 AND NOT EXISTS(SELECT * FROM operatory WHERE operatory.OperatoryNum=scheduleop.OperatoryNum) 
									UNION 
									SELECT apptviewitem.OpNum AS OpNum FROM apptviewitem WHERE apptviewitem.OpNum!=0 AND NOT EXISTS(SELECT * FROM operatory WHERE operatory.OperatoryNum=apptviewitem.OpNum)";
			DataTable table=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("OperatoryInvalidReference","Operatory references that are invalid")+": "+table.Rows.Count+"\r\n";
					}
					break;
				case DbmMode.Fix:
					for(int i = 0;i<table.Rows.Count;i++) {
						long opNum=PIn.Long(table.Rows[i]["OpNum"].ToString());
						if(opNum!=0) {
							Operatory op=new Operatory();
							op.OperatoryNum=opNum;
							op.OpName="UNKNOWN-"+opNum;
							op.Abbrev="UNKN";
							Crud.OperatoryCrud.Insert(op,true);
						}
					}
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("OperatoryInvalidReference","Operatories created from an invalid operatory reference")+": "+table.Rows.Count+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string OrthoChartDeleteDuplicates(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command=@"SELECT SUM(duplicates.CountDup) 
				FROM (
					SELECT COUNT(*)-1 CountDup 
					FROM orthochart 
					GROUP BY PatNum,DateService,FieldName,FieldValue 
					HAVING COUNT(*) > 1
			) duplicates";
			long numFound=PIn.Long(Db.GetCount(command));
			switch(modeCur) {
				case DbmMode.Check:
					if(numFound > 0 || verbose) {
						log+=numFound.ToString()+Lans.g("FormDatabaseMaintenance"," duplicate cell entries found.")+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command=@"SELECT MAX(OrthoChartNum)OrthoChartNum,PatNum,DateService,FieldName,FieldValue 
						FROM orthochart 
						GROUP BY PatNum,DateService,FieldName,FieldValue 
						HAVING COUNT(*) > 1";
					DataTable table=Db.GetTable(command);
					long numFixed=numFound;
					if(table.Rows.Count>0 || verbose) {
						for(int i = 0;i<table.Rows.Count;i++) {
							//get row information, and then delete everything from orthochart where that info matches, excluding the duplicate with the highest 
							//OrthoChartNum.
							long orthochartnum=PIn.Long(table.Rows[i]["OrthoChartNum"].ToString());
							long patNum=PIn.Long(table.Rows[i]["PatNum"].ToString());
							DateTime dateService=PIn.Date(table.Rows[i]["DateService"].ToString());
							string fieldName=table.Rows[i]["FieldName"].ToString();
							string fieldValue=table.Rows[i]["FieldValue"].ToString();
							command="DELETE FROM orthochart WHERE PatNum="+POut.Long(patNum)+" AND DateService="+POut.Date(dateService)
								+" AND FieldName='"+POut.String(fieldName)+"' AND FieldValue='"+POut.String(fieldValue)
								+"' AND OrthoChartNum !="+POut.Long(orthochartnum);
							Db.NonQ(command);
						}
						log+=Lans.g("FormDatabaseMaintenance","Duplicates processed")+":"+numFixed.ToString()+"\r\n";
						//check to see if there are duplicate date entries,fieldnames which aren't supposed to occur. This means there is conflict one needs to be chosen.
						command="SELECT * FROM orthochart GROUP BY PatNum,DateService,FieldName HAVING COUNT(*) > 1";
						table=Db.GetTable(command);
						if(table.Rows.Count>0) {
							log+=Lans.g("FormDatabaseMaintenance","Duplicate entries could not be deleted for the following OrthoChartNums. Please call support "
								+"and escalate to an engineer to remove duplicates.");
						}
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string OrthoChartFieldsWithoutValues(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string queryStr="SELECT COUNT(*) FROM orthochart WHERE FieldValue=''";
			int numFound=PIn.Int(Db.GetCount(queryStr));
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Ortho chart fields without values found")+": "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					string command="DELETE FROM orthochart WHERE FieldValue=''";
					Db.NonQ(command);
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Ortho chart fields without values fixed")+": "+numFound.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PatFieldsDeleteDuplicates(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			//This code is only needed for older db's. New DB's created after 12.2.30 and 12.3.2 shouldn't need this.
			string command=@"DROP TABLE IF EXISTS tempduplicatepatfields";
			Db.NonQ(command);
			string tableName="tempduplicatepatfields"+MiscUtils.CreateRandomAlphaNumericString(8);//max size for a table name in oracle is 30 chars.
																																														//This query run very fast on a db with no corruption.
			command=@"CREATE TABLE "+tableName+@"
								SELECT *
								FROM patfield
								GROUP BY PatNum,FieldName
								HAVING COUNT(*)>1";
			Db.NonQ(command);
			command=@"SELECT patient.PatNum,LName,FName
								FROM patient 
								INNER JOIN "+tableName+@" t ON t.PatNum=patient.PatNum
								GROUP BY patient.PatNum";
			DataTable table=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Patients with duplicate field entries found: ")+table.Rows.Count+".\r\n";
					}
					break;
				case DbmMode.Fix:
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","The following patients had corrupt Patient Fields. Please verify the Patient Fields of these patients:")+"\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							log+="#"+table.Rows[i]["PatNum"].ToString()+" "+table.Rows[i]["LName"]+", "+table.Rows[i]["FName"]+".\r\n";
						}
						//Without this index the delete process takes too long.
						command="ALTER TABLE "+tableName+" ADD INDEX(PatNum)";
						Db.NonQ(command);
						command="ALTER TABLE "+tableName+" ADD INDEX(FieldName)";
						Db.NonQ(command);
						command="DELETE FROM patfield WHERE ((PatNum, FieldName) IN (SELECT PatNum, FieldName FROM "+tableName+"));";
						Db.NonQ(command);
						command="INSERT INTO patfield SELECT * FROM "+tableName+";";
						Db.NonQ(command);
						log+=Lans.g("FormDatabaseMaintenance","Patients with duplicate field entries removed: ")+table.Rows.Count+".\r\n";
					}
					break;
			}
			command=@"DROP TABLE IF EXISTS "+tableName;
			Db.NonQ(command);
			return log;
		}

		#endregion Operatory, OrthoChart, PatField------------------------------------------------------------------------------------------------------
		#region Patient---------------------------------------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string PatientBadGuarantor(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT p.PatNum FROM patient p LEFT JOIN patient p2 ON p.Guarantor=p2.PatNum WHERE p2.PatNum IS NULL";
			DataTable table=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Patients with invalid Guarantors found: ")+table.Rows.Count.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					for(int i = 0;i<table.Rows.Count;i++) {
						command="UPDATE patient SET Guarantor=PatNum WHERE PatNum="+table.Rows[i][0].ToString();
						Db.NonQ(command);
					}
					int numberFixed=table.Rows.Count;
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Patients with invalid Guarantors fixed: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PatientBadGuarantorWithAnotherGuarantor(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT p.PatNum,p2.Guarantor FROM patient p LEFT JOIN patient p2 ON p.Guarantor=p2.PatNum WHERE p2.PatNum!=p2.Guarantor";
			DataTable table=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Patients with a guarantor who has another guarantor found: ")+table.Rows.Count.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					for(int i = 0;i<table.Rows.Count;i++) {
						command="UPDATE patient SET Guarantor="+table.Rows[i]["Guarantor"].ToString()+" WHERE PatNum="+table.Rows[i]["PatNum"].ToString();
						Db.NonQ(command);
					}
					int numberFixed=table.Rows.Count;
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Patients with a guarantor who has another guarantor fixed: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PatientDeletedWithClinicNumSet(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM patient WHERE ClinicNum!=0 AND PatStatus="+POut.Int((int)PatientStatus.Deleted);
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Deleted patients with a clinic still set: ")+numFound.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="UPDATE patient SET ClinicNum=0 WHERE ClinicNum!=0 AND PatStatus="+POut.Int((int)PatientStatus.Deleted);
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Deleted patients with clinics cleared: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>Finds any patients that have an invalid BillingType (Billing Type that does not exist as a definition) and sets them to the first
		///billing type in definitions table.</summary>
		[DbmMethodAttr]
		public static string PatientInvalidBillingType(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM patient "
						+"LEFT JOIN definition ON BillingType=definition.DefNum "
							+"AND Category="+(int)DefCat.BillingTypes+" "
						+"WHERE DefNum IS NULL ";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Patients found with invalid billing type")+": "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="SELECT PatNum FROM patient "
						+"LEFT JOIN definition ON BillingType=definition.DefNum "
							+"AND Category="+(int)DefCat.BillingTypes+" "
						+"WHERE DefNum IS NULL";
					List<long> listPatNums=Db.GetListLong(command);
					long numberFixed=0;
					if(listPatNums.Count>0) {
						command="UPDATE patient SET BillingType="
							+"("+DbHelper.LimitOrderBy("SELECT DefNum FROM definition WHERE Category="+(int)DefCat.BillingTypes+" ORDER BY ItemOrder",1)+") "
							+"WHERE PatNum IN ("+string.Join(",",listPatNums)+")";
						numberFixed=Db.NonQ(command);
					}
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Patients fixed with invalid billing type")+": "+numberFixed+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PatientInvalidGradeLevel(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM patient WHERE GradeLevel < 0";//Any negative number is considered invalid.
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound > 0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Patients with invalid GradeLevel set")+": "+numFound.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					//Set all invalid Grade Levels to Unknown.
					command="UPDATE patient SET GradeLevel="+POut.Int((int)PatientGrade.Unknown)+" WHERE GradeLevel < 0";
					long numFixed=Db.NonQ(command);
					if(numFixed > 0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Patients with invalid GradeLevel fixed")+": "+numFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		//NOTE: 03/24/2016 Issue with DateTimeDeceased for a patient where the the DB value was "·,'*-0--,) 14:25:37" for customer 3202. 
		//Per Nathan if this occurs again we need to make a DBM to fix this.
		//[DbmMethod]
		//public static string PatientInvalidDateTimeDeceased(bool verbose,DBMMode modeCur) {
		//	if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
		//		return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
		//	}
		//	string log="";
		//	if(isCheck) {
		//		command="";
		//		int numFound=PIn.Int(Db.GetCount(command));
		//		if(numFound>0 || verbose) {
		//			log+=Lans.g("FormDatabaseMaintenance",": ")+numFound.ToString()+"\r\n";
		//		}
		//	}
		//	else {//fix
		//		command="";
		//		long numberFixed=Db.NonQ(command);
		//		if(numberFixed>0 || verbose) {
		//			log+=Lans.g("FormDatabaseMaintenance",": ")+numberFixed.ToString()+"\r\n";
		//		}
		//	}
		//	return log;
		//}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string PatientsNoClinicSet(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				return "";
			}
			//Get patients not assigned to a clinic:
			string command=@"SELECT PatNum,LName,FName FROM patient WHERE ClinicNum=0 AND PatStatus!="+POut.Int((int)PatientStatus.Deleted);
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			string log=Lans.g("FormDatabaseMaintenance","Patients with no Clinic assigned: ")+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {//Only the fix should show the entire list of items.
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {//Running the fix and there are items to show.
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							//Start a new line and indent every three patients for printing purposes.
							if(i%3==0) {
								log+="\r\n   ";
							}
							log+=table.Rows[i]["PatNum"].ToString()+"-"
							+table.Rows[i]["LName"].ToString()+", "
							+table.Rows[i]["FName"].ToString()+"; ";
						}
						log+="\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string PatientPriProvHidden(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			string command=@"
				SELECT provider.ProvNum,provider.Abbr
				FROM provider
				INNER JOIN patient ON patient.PriProv=provider.ProvNum
				WHERE provider.IsHidden=1
				GROUP BY provider.ProvNum";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			string log=Lans.g("FormDatabaseMaintenance","Hidden providers with patients: ")+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {//Only the fix should show the entire list of items.
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {//Running the fix and there are items to show.
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						if(table.Rows.Count>0 || verbose) {
							DataTable patTable;
							for(int i = 0;i<table.Rows.Count;i++) {
								log+="     "+table.Rows[i]["Abbr"].ToString()+": ";
								command=@"SELECT PatNum,LName,FName FROM patient WHERE PriProv=(SELECT ProvNum FROM provider WHERE ProvNum="
									+table.Rows[i]["ProvNum"].ToString()+" AND IsHidden=1) LIMIT 10";
								patTable=Db.GetTable(command);
								for(int j = 0;j<patTable.Rows.Count;j++) {
									if(j>0) {
										log+=", ";
									}
									log+=patTable.Rows[j]["PatNum"].ToString()+"-"+patTable.Rows[j]["FName"].ToString()+" "+patTable.Rows[j]["LName"].ToString();
								}
								log+="\r\n";
							}
							log+=Lans.g("FormDatabaseMaintenance","   Go to Lists | Providers to move all patients from the hidden provider to another provider.")+"\r\n";
						}
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PatientPriProvMissing(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command=@"SELECT COUNT(*) FROM patient WHERE PriProv=0";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Patient pri provs not set: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					//previous versions of the program just dealt gracefully with missing provnum.
					//From now on, we can assum priprov is not missing, making coding easier.
					command=@"UPDATE patient SET PriProv="+PrefC.GetString(PrefName.PracticeDefaultProv)+" WHERE PriProv=0";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Patient pri provs fixed: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PatientUnDeleteWithBalance(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT PatNum FROM patient	WHERE PatStatus=4 "
				+"AND (Bal_0_30 !=0	OR Bal_31_60 !=0 OR Bal_61_90 !=0	OR BalOver90 !=0 OR InsEst !=0 OR BalTotal !=0)";
			DataTable table=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Patients found who are marked deleted with non-zero balances: ")+table.Rows.Count+"\r\n";
					}
					break;
				case DbmMode.Fix:
					if(table.Rows.Count==0 && verbose) {
						log+=Lans.g("FormDatabaseMaintenance","No balances found for deleted patients.")+"\r\n";
						return log;
					}
					Patient pat;
					Patient old;
					for(int i = 0;i<table.Rows.Count;i++) {
						pat=Patients.GetPat(PIn.Long(table.Rows[i][0].ToString()));
						old=pat.Copy();
						pat.LName=pat.LName+Lans.g("FormDatabaseMaintenance","DELETED");
						pat.PatStatus=PatientStatus.Archived;
						Patients.Update(pat,old);
						log+=Lans.g("FormDatabaseMaintenance","Warning!  Patient:")+" "+old.GetNameFL()+" "
							+Lans.g("FormDatabaseMaintenance","was previously marked as deleted, but was found to have a balance. Patient has been changed to Archive status.  The account will need to be cleared, and the patient deleted again.")+"\r\n";
					}
					break;
			}
			return log;
		}

		#endregion Patient------------------------------------------------------------------------------------------------------------------------------
		#region PatPlan, Payment------------------------------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string PatPlanDeleteWithInvalidInsSubNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM patplan WHERE InsSubNum NOT IN (SELECT InsSubNum FROM inssub)";
					string countStr=Db.GetCount(command);
					if(countStr!="0" || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Pat plans found with invalid InsSubNums: ")+countStr+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="DELETE FROM patplan WHERE InsSubNum NOT IN (SELECT InsSubNum FROM inssub)";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Pat plans with invalid InsSubNums deleted: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PatPlanDeleteWithInvalidPatNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM patplan WHERE PatNum NOT IN (SELECT PatNum FROM patient)";
					string countStr=Db.GetCount(command);
					if(countStr!="0" || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Pat plans found with invalid PatNums: ")+countStr+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="DELETE FROM patplan WHERE PatNum NOT IN (SELECT PatNum FROM patient)";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Pat plans with invalid PatNums deleted: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string PatPlanOrdinalDuplicates(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			string command="SELECT patient.PatNum,patient.LName,patient.FName,COUNT(*) "
				+"FROM patplan "
				+"INNER JOIN patient ON patient.PatNum=patplan.PatNum "
				+"GROUP BY patplan.PatNum,patplan.Ordinal "
				+"HAVING COUNT(*)>1";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			string log=Lans.g("FormDatabaseMaintenance","PatPlan duplicate ordinals: ")+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {//Only the fix should show the entire list of items.
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {//Running the fix and there are items to show.
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							log+="   #"+table.Rows[i]["PatNum"].ToString()+" - "+PIn.String(table.Rows[i]["FName"].ToString())+" "+PIn.String(table.Rows[i]["LName"].ToString())+"\r\n";
						}
						log+=Lans.g("FormDatabaseMaintenance","   They need to be fixed manually.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PatPlanOrdinalZeroToOne(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT PatPlanNum,PatNum FROM patplan WHERE Ordinal=0";
			DataTable table=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","PatPlan ordinals currently zero: ")+table.Rows.Count+"\r\n";
					}
					break;
				case DbmMode.Fix:
					int numberFixed=0;
					for(int i = 0;i<table.Rows.Count;i++) {
						PatPlan patPlan=PatPlans.GetPatPlan(PIn.Long(table.Rows[i][1].ToString()),0);
						if(patPlan!=null) {//Unlikely but possible if plan gets deleted by a user during this check.
							PatPlans.SetOrdinal(patPlan.PatPlanNum,1);
							numberFixed++;
						}
					}
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","PatPlan ordinals changed from 0 to 1: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PatPlanOrdinalTwoToOne(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT PatPlanNum,PatNum FROM patplan patplan1 WHERE Ordinal=2 AND NOT EXISTS("
				+"SELECT * FROM patplan patplan2 WHERE patplan1.PatNum=patplan2.PatNum AND patplan2.Ordinal=1)";
			DataTable table=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","PatPlans for secondary found where no primary ins: ")+table.Rows.Count+"\r\n";
					}
					break;
				case DbmMode.Fix:
					int numberFixed=0;
					for(int i = 0;i<table.Rows.Count;i++) {
						PatPlan patPlan=PatPlans.GetPatPlan(PIn.Long(table.Rows[i][1].ToString()),2);
						if(patPlan!=null) {//Unlikely but possible if plan gets deleted by a user during this check.
							PatPlans.SetOrdinal(patPlan.PatPlanNum,1);
							numberFixed++;
						}
					}
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","PatPlan ordinals changed from 2 to 1 if no primary ins: ")+numberFixed+"\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>Shows payments that have a PaymentAmt that doesn't match the sum of all associated PaySplits.  
		///Payments with no PaySplits are dealt with in PaymentMissingPaySplit()</summary>
		[DbmMethodAttr(HasBreakDown = true)]
		public static string PaymentAmtNotMatchPaySplitTotal(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			//Note that this just returns info for a (seemingly) random patient that has a paysplit for the payment.
			//This is because the payment only shows in the ledger for the patient with the paysplit, not the patient on the payment.
			string command="SELECT patient.PatNum, patient.LName, patient.FName, payment.PayDate "
				+"FROM payment "
				+"INNER JOIN ( "
					+"SELECT paysplit.PayNum, SUM(paysplit.SplitAmt) totSplitAmt, MIN(paysplit.PatNum) PatNum "
					+"FROM paysplit "
					+"GROUP BY paysplit.PayNum "
				+") pstotals ON pstotals.PayNum=payment.PayNum "
				+"INNER JOIN patient ON patient.PatNum=pstotals.PatNum "
				+"WHERE payment.PayAmt!=0 "
				+"AND ROUND(payment.PayAmt,2)!=ROUND(pstotals.totSplitAmt,2) "
				+"ORDER BY patient.LName, patient.FName, payment.PayDate";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			string log=Lans.g("FormDatabaseMaintenance","Payments with amounts that do not match the total split(s) amounts")+": "+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {//Only the fix should show the entire list of items.
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {//Running the fix and there are items to show.
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							log+="   "+table.Rows[i]["PatNum"].ToString();
							log+="  "+Patients.GetNameLF(table.Rows[i]["LName"].ToString(),table.Rows[i]["FName"].ToString(),"","");
							log+="  "+PIn.DateT(table.Rows[i]["PayDate"].ToString()).ToShortDateString();
							log+="\r\n";
						}
						log+="   "+Lans.g("FormDatabaseMaintenance","They need to be fixed manually.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PaymentDetachMissingDeposit(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM payment "
						+"WHERE DepositNum != 0 "
						+"AND NOT EXISTS(SELECT * FROM deposit WHERE deposit.DepositNum=payment.DepositNum)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Payments attached to deposits that no longer exist: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="UPDATE payment SET DepositNum=0 "
						+"WHERE DepositNum != 0 "
						+"AND NOT EXISTS(SELECT * FROM deposit WHERE deposit.DepositNum=payment.DepositNum)";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Payments detached from deposits that no longer exist: ")
						+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PaymentMissingPaySplit(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM payment "
						+"WHERE PayNum NOT IN (SELECT PayNum FROM paysplit) "//Payments with no split that are
						+"AND ((DepositNum=0) "                              //not attached to a deposit
						+"OR (DepositNum!=0 AND PayAmt=0))";                 //or attached to a deposit with no amount.
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Payments with no attached paysplit: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="DELETE FROM payment "
						+"WHERE PayNum NOT IN (SELECT PayNum FROM paysplit) "//Payments with no split that are
						+"AND ((DepositNum=0) "                              //not attached to a deposit
						+"OR (DepositNum!=0 AND PayAmt=0))";                 //or attached to a deposit with no amount.
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Payments with no attached paysplit fixed: ")+numberFixed+"\r\n";
					}
					break;
			}
			return log;
		}

		#endregion PatPlan, Payment---------------------------------------------------------------------------------------------------------------------
		#region PayPlanCharge, PayPlan, PaySplit--------------------------------------------------------------------------------------------------------

		//DEPRECATED. This no longer holds true with Payment Plans as of Version 16.2.
		//[DbmMethod]
		//public static string PayPlanChargeGuarantorMatch(bool verbose,DBMMode modeCur) {
		//	if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
		//		return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
		//	}
		//	if(DataConnection.DBtype==DatabaseType.Oracle) {
		//		return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
		//	}
		//	string log="";
		//	if(isCheck) {
		//		int numFound=0;
		//		command="SELECT COUNT(*) FROM payplancharge,payplan "
		//			+"WHERE payplan.PayPlanNum=payplancharge.PayPlanNum "
		//			+"AND payplancharge.Guarantor != payplan.Guarantor";
		//		numFound+=PIn.Int(Db.GetCount(command));
		//		command="SELECT COUNT(*) FROM payplancharge,payplan "
		//			+"WHERE payplan.PayPlanNum=payplancharge.PayPlanNum "
		//			+"AND payplancharge.PatNum != payplan.PatNum";
		//		numFound+=PIn.Int(Db.GetCount(command));
		//		if(numFound>0 || verbose) {
		//			log+=Lans.g("FormDatabaseMaintenance","PayPlanCharge guarantors and pats not matching payplan guarantors and pats: ")+numFound+"\r\n";
		//		}
		//	}
		//	else {
		//		//Fix the cases where payplan.Guarantor and payplan.PatNum are not zero. 
		//		command="UPDATE payplan,payplancharge "
		//			+"SET payplancharge.Guarantor=payplan.Guarantor "
		//			+"WHERE payplan.PayPlanNum=payplancharge.PayPlanNum "
		//			+"AND payplancharge.Guarantor != payplan.Guarantor "
		//		+"AND payplan.Guarantor != 0";
		//		long numFixed=Db.NonQ(command);
		//		command="UPDATE payplan,payplancharge "
		//			+"SET payplancharge.PatNum=payplan.PatNum "
		//			+"WHERE payplan.PayPlanNum=payplancharge.PayPlanNum "
		//			+"AND payplancharge.PatNum != payplan.PatNum "
		//		+"AND payplan.PatNum != 0";
		//		numFixed+=Db.NonQ(command);
		//		if(numFixed>0 || verbose) {
		//			log+=Lans.g("FormDatabaseMaintenance","PayPlanCharge guarantors and pats fixed to match payplan: ")+numFixed+"\r\n";
		//		}
		//		//No fix yet if payplan.Guarantor or payplan.PatNum are zero but there are good values in PayPlanCharge.
		//	}
		//	return log;
		//}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string PayPlanChargeProvNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			string command="SELECT pat.PatNum AS 'PatNum',pat.LName AS 'PatLName',pat.FName AS 'PatFName',guar.PatNum AS 'GuarNum',guar.LName AS 'GuarLName',guar.FName AS 'GuarFName',payplan.PayPlanDate "
				+"FROM payplancharge "
				+"LEFT JOIN payplan ON payplancharge.PayPlanNum=payplan.PayPlanNum "
				+"LEFT JOIN patient pat ON payplan.PatNum=pat.PatNum "
				+"LEFT JOIN patient guar ON payplan.Guarantor=guar.PatNum "
				+"WHERE payplancharge.ProvNum=0 "
				+"GROUP BY payplancharge.PayPlanNum";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			string log=Lans.g("FormDatabaseMaintenance","Pay plans with charges that have providers missing: ")+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {//Only the fix should show the entire list of items.
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {//Running the fix and there are items to show.
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							log+="   "+Lans.g("FormDatabaseMaintenance","Pay Plan Date")+": "+PIn.DateT(table.Rows[i]["PayPlanDate"].ToString()).ToShortDateString()+"\r\n"
							+"      "+Lans.g("FormDatabaseMaintenance","Guarantor")+": #"+table.Rows[i]["PatNum"]+" - "+table.Rows[i]["PatFName"]+" "+table.Rows[i]["PatLName"]+"\r\n"
							+"      "+Lans.g("FormDatabaseMaintenance","For Patient")+": #"+table.Rows[i]["GuarNum"]+" - "+table.Rows[i]["GuarFName"]+" "+table.Rows[i]["GuarLName"]+"\r\n";
						}
						log+=Lans.g("FormDatabaseMaintenance","   They need to be fixed manually.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PayPlanChargeWithInvalidPayPlanNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT COUNT(DISTINCT PayPlanNum) FROM (SELECT PayPlanNum FROM payplancharge WHERE PayPlanNum NOT IN(SELECT PayPlanNum FROM payplan) "
				+"UNION SELECT PayPlanNum FROM creditcard WHERE PayPlanNum>0 AND PayPlanNum NOT IN(SELECT PayPlanNum FROM payplan)) A";
			string count=Db.GetCount(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(count!="0" || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","PayPlan charges or credit cards with an invalid PayPlanNum found")+": "+count;
					}
					break;
				case DbmMode.Fix:
					if(count!="0" || verbose) {
						//Delete the payment plan charges and update credit cards that point to an invalid payment plan. Claimprocs and paysplits with an invalid
						//PayPlanNum are taken care of in other DBM methods.
						command="DELETE FROM payplancharge WHERE PayPlanNum NOT IN(SELECT PayPlanNum FROM payplan)";
						Db.NonQ(command);
						command="UPDATE creditcard SET PayPlanNum=0 WHERE PayPlanNum NOT IN(SELECT PayPlanNum FROM payplan)";
						Db.NonQ(command);
						log+=Lans.g("FormDatabaseMaintenance","PayPlan charges or credit cards with an invalid PayPlanNum fixed")+": "+count;
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PayPlanSetGuarantorToPatForIns(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT COUNT(*) FROM payplan WHERE PlanNum>0 AND Guarantor != PatNum";
			int numFound=PIn.Int(Db.GetCount(command));
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","PayPlan Guarantors not equal to PatNum where used for insurance tracking: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					//Too dangerous to do anything at all.  Just have a very descriptive explanation in the check.
					//For now, tell the user that a fix is under development.
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","PayPlan Guarantors not equal to PatNum where used for insurance tracking: ")+numFound+"\r\n";
						log+=Lans.g("FormDatabaseMaintenance","   A safe fix is under development.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string PaySplitAttachedToDeletedProc(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			string command="SELECT CONCAT(pat.LName,' ',pat.FName) AS 'PatientName', payment.PatNum, payment.PayDate, payment.PayAmt, "
				+"paysplit.DatePay, procedurecode.AbbrDesc, CONCAT(splitpat.FName,' ',splitpat.LName) AS 'SplitPatientName', paysplit.SplitAmt "
				+"FROM paysplit "
				+"INNER JOIN payment ON payment.PayNum=paysplit.PayNum "
				+"INNER JOIN procedurelog ON paysplit.ProcNum=procedurelog.ProcNum AND procedurelog.ProcStatus="+POut.Int((int)ProcStat.D)+" "
				+"INNER JOIN procedurecode ON procedurelog.CodeNum=procedurecode.CodeNum "
				+"INNER JOIN patient pat ON pat.PatNum=payment.PatNum "
				+"INNER JOIN patient splitpat ON splitpat.PatNum=paysplit.PatNum "
				+"ORDER BY pat.LName, pat.FName, payment.PayDate, paysplit.DatePay, procedurecode.AbbrDesc";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			string log=Lans.g("FormDatabaseMaintenance","Paysplits attached to deleted procedures")+": "+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {//Only the fix should show the entire list of items.
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {//Running the fix and there are items to show.
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							log+="   "+Lans.g("FormDatabaseMaintenance","Payment")+": #"+table.Rows[i]["PatNum"].ToString();
							log+=" "+table.Rows[i]["PatientName"].ToString();
							log+=" "+PIn.DateT(table.Rows[i]["PayDate"].ToString()).ToShortDateString();
							log+=" "+PIn.Double(table.Rows[i]["PayAmt"].ToString()).ToString("c");
							log+="\r\n      "+Lans.g("FormDatabaseMaintenance","Split")+": "+PIn.DateT(table.Rows[i]["DatePay"].ToString()).ToShortDateString();
							log+=" "+table.Rows[i]["SplitPatientName"].ToString();
							log+=" "+table.Rows[i]["AbbrDesc"].ToString();
							log+=" "+PIn.Double(table.Rows[i]["SplitAmt"].ToString()).ToString("c");
							log+="\r\n";
						}
						log+="   "+Lans.g("FormDatabaseMaintenance","They need to be fixed manually.")+"\r\n";
					}
					break;
			}
			return log;
		}

		/// <summary>Shows patients that have paysplits attached to insurance payment plans.</summary>
		[DbmMethodAttr(HasBreakDown=true)]
		public static string PaySplitAttachedToInsurancePaymentPlan(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			DataTable table=GetPaySplitsAttachedToInsurancePaymentPlan();
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			string log=Lans.g("FormDatabaseMaintenance","Paysplits attached to insurance payment plans")+": "+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {
						log+="\r\n"+Lans.g("FormDatabaseMaintenance","Manual fix needed. Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i=0;i<table.Rows.Count;i++) {
							log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Patient #")+" "+table.Rows[i]["PatNum"].ToString()+" "
								+Lans.g("FormDatabaseMaintenance","Amount")+": "+PIn.Double(table.Rows[i]["SplitAmt"].ToString()).ToString("c")+" "
								+Lans.g("FormDatabaseMaintenance","Date")+": "+PIn.Date(table.Rows[i]["DatePay"].ToString()).ToShortDateString()+" "
								+Lans.g("FormDatabaseMaintenance","Insurance payment plan #")+table.Rows[i]["PayPlanNum"];
						}
						log+="\r\n"+Lans.g("FormDatabaseMaintenance","Run 'Pay Plan Payments' in the Tools tab to fix these payments.");
					}
					break;
			}
			return log;
		}

		//DEPRECATED. This no longer holds true for payplans as of version 16.2. Plus, this never did anything anyway.
		//[DbmMethod]
		//public static string PaySplitAttachedToPayPlan(bool verbose,DBMMode modeCur) {
		//	if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
		//		return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
		//	}
		//	string log="";
		//	command="SELECT SplitNum,payplan.Guarantor FROM paysplit,payplan "
		//		+"WHERE paysplit.PayPlanNum=payplan.PayPlanNum "
		//		+"AND paysplit.PatNum!=payplan.Guarantor";
		//	DataTable table=Db.GetTable(command);
		//	if(isCheck) {
		//		if(table.Rows.Count>0 || verbose) {
		//			log+=Lans.g("FormDatabaseMaintenance","Paysplits found with patnum not matching payplan guarantor: ")+table.Rows.Count+"\r\n";
		//		}
		//	}
		//	else {
		//		//Too dangerous to do anything at all.  Just have a very descriptive explanation in the check.
		//		//For now, tell the user that a fix is under development.
		//		if(table.Rows.Count>0 || verbose) {
		//			log+=Lans.g("FormDatabaseMaintenance","Paysplits found with patnum not matching payplan guarantor: ")+table.Rows.Count+"\r\n";
		//			log+=Lans.g("FormDatabaseMaintenance","   A safe fix is under development.")+"\r\n";
		//		}
		//	}
		//	return log;
		//}

		///<summary>No need to pass in userNum, it's set before remoting role check and passed to the server if necessary.</summary>
		[DbmMethodAttr(HasUserNum = true)]
		public static string PaySplitWithInvalidPayNum(bool verbose,DbmMode modeCur,long userNum = 0) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				userNum=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur,userNum);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM paysplit WHERE NOT EXISTS(SELECT * FROM payment WHERE paysplit.PayNum=payment.PayNum)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Paysplits found with invalid PayNum: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					if(DataConnection.DBtype==DatabaseType.Oracle) {
						return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
					}
					command=@"SELECT *,SUM(SplitAmt) SplitAmt_ 
						FROM paysplit 
						WHERE NOT EXISTS(SELECT * FROM payment WHERE paysplit.PayNum=payment.PayNum) 
						AND PayNum!=0
						GROUP BY PayNum";
					DataTable table=Db.GetTable(command);
					int rowsFixed=0;
					if(table.Rows.Count>0) {
						List<Def> listDefs=Defs.GetDefsForCategory(DefCat.PaymentTypes,true);
						for(int i = 0;i<table.Rows.Count;i++) {
							//There's only one place in the program where this is called from.  Date is today, so no need to validate the date.
							Payment payment=new Payment();
							payment.PayType=listDefs[0].DefNum;
							payment.DateEntry=PIn.Date(table.Rows[i]["DateEntry"].ToString());
							payment.PatNum=PIn.Long(table.Rows[i]["PatNum"].ToString());
							payment.PayDate=PIn.Date(table.Rows[i]["DatePay"].ToString());
							payment.PayAmt=PIn.Double(table.Rows[i]["SplitAmt_"].ToString());
							payment.PayNote="Dummy payment. Original payment entry missing from the database.";
							payment.PayNum=PIn.Long(table.Rows[i]["PayNum"].ToString());
							payment.SecUserNumEntry=userNum;
							payment.PaymentSource=CreditCardSource.None;
							payment.ProcessStatus=ProcessStat.OfficeProcessed;
							Payments.Insert(payment,true);
						}
						rowsFixed+=table.Rows.Count;
					}
					//Handling paysplits that have a pay num of 0 separately because we want to create one payment per patient per day
					command=@"SELECT *,SUM(SplitAmt) SplitAmt_ 
						FROM paysplit 
						WHERE NOT EXISTS(SELECT * FROM payment WHERE paysplit.PayNum=payment.PayNum) 
						AND PayNum=0
						GROUP BY PatNum,DatePay";
					table=Db.GetTable(command);
					if(table.Rows.Count>0) {
						List<Def> listDefs=Defs.GetDefsForCategory(DefCat.PaymentTypes,true);
						for(int i=0;i<table.Rows.Count;i++) {
							Payment payment=new Payment();
							payment.PayType=listDefs[0].DefNum;
							payment.DateEntry=PIn.Date(table.Rows[i]["DateEntry"].ToString());
							payment.PatNum=PIn.Long(table.Rows[i]["PatNum"].ToString());
							payment.PayDate=PIn.Date(table.Rows[i]["DatePay"].ToString());
							payment.PayAmt=PIn.Double(table.Rows[i]["SplitAmt_"].ToString());
							payment.PayNote="Dummy payment. Original payment entry number was 0.";
							payment.PayNum=PIn.Long(table.Rows[i]["PayNum"].ToString());
							payment.SecUserNumEntry=userNum;
							payment.PaymentSource=CreditCardSource.None;
							payment.ProcessStatus=ProcessStat.OfficeProcessed;
							Payments.Insert(payment);
							command="UPDATE paysplit SET PayNum="+POut.Long(payment.PayNum)+
								" WHERE PayNum=0 AND PatNum="+POut.Long(payment.PatNum)+" AND DatePay="+POut.Date(payment.PayDate);
							Db.NonQ(command);
						}
						rowsFixed+=table.Rows.Count;
					}
					if(rowsFixed > 0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Paysplits found with invalid PayNum fixed: ")+rowsFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PaySplitWithInvalidPayPlanNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM paysplit WHERE paysplit.PayPlanNum!=0 AND paysplit.PayPlanNum NOT IN(SELECT payplan.PayPlanNum FROM payplan)";
					int numFound=PIn.Int(Db.GetScalar(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Paysplits found with invalid PayPlanNum: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="UPDATE paysplit SET paysplit.PayPlanNum=0 WHERE paysplit.PayPlanNum!=0 AND paysplit.PayPlanNum NOT IN(SELECT payplan.PayPlanNum FROM payplan)";
					long numFixed=Db.NonQ(command);
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Paysplits with invalid PayPlanNums fixed: ")+numFixed+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string PaySplitWithInvalidPrePayNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT ps1.* FROM paysplit ps1 LEFT JOIN paysplit ps2 ON ps1.FSplitNum=ps2.SplitNum WHERE ps1.FSplitNum!=0 AND ps2.SplitNum IS NULL";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			string log=Lans.g("FormDatabaseMaintenance","Paysplits attached to deleted prepayments")+": "+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {//Only the fix should show the entire list of items.
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {//Running the fix and there are items to show.
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							log+="   "+Lans.g("FormDatabaseMaintenance","PatNum")+": #"+table.Rows[i]["PatNum"].ToString();
							log+=" "+PIn.DateT(table.Rows[i]["DatePay"].ToString()).ToShortDateString();
							log+=" "+PIn.Double(table.Rows[i]["SplitAmt"].ToString()).ToString("c");
							log+="\r\n";
						}
						log+="   "+Lans.g("FormDatabaseMaintenance","They need to be fixed manually.")+"\r\n";
					}
					break;
			}
			return log;
		}

		#endregion PayPlanCharge, PayPlan, PaySplit-----------------------------------------------------------------------------------------------------
		#region PerioMeasure, PlannedAppt, Preference---------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string PerioMeasureWithInvalidIntTooth(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command=@"SELECT COUNT(*) FROM periomeasure WHERE IntTooth > 32 OR IntTooth < 1";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","PerioMeasures found with invalid tooth number: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command=@"DELETE FROM periomeasure WHERE IntTooth > 32 OR IntTooth < 1";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","PerioMeasures deleted due to invalid tooth number: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PlannedApptsWithInvalidAptNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command=@"SELECT COUNT(*) FROM plannedappt WHERE AptNum=0";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Planned appointments found with invalid AptNum")+": "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command=@"DELETE FROM plannedappt WHERE AptNum=0";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Planned appointments deleted due to invalid AptNum")+": "+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PreferenceAllergiesIndicateNone(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM allergydef where AllergyDefNum="+POut.Long(PrefC.GetLong(PrefName.AllergiesIndicateNone));
					if(PIn.Int(Db.GetCount(command))==0 && PrefC.GetString(PrefName.AllergiesIndicateNone)!="") {
						log+=Lans.g("FormDatabaseMaintenance","Preference \"AllergyIndicatesNone\" is an invalid value.")+"\r\n";
					}
					else if(verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Preference \"AllergyIndicatesNone\" checked.")+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="SELECT COUNT(*) FROM allergydef where AllergyDefNum="+POut.Long(PrefC.GetLong(PrefName.AllergiesIndicateNone));
					if(PIn.Int(Db.GetCount(command))==0 && PrefC.GetString(PrefName.AllergiesIndicateNone)!="") {
						Prefs.UpdateString(PrefName.AllergiesIndicateNone,"");
						Signalods.SetInvalid(InvalidType.Prefs);
						log+=Lans.g("FormDatabaseMaintenance","Preference \"AllergyIndicatesNone\" set to blank due to an invalid value.")+"\r\n";
					}
					else if(verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Preference \"AllergyIndicatesNone\" checked.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PreferenceDateDepositsStarted(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			DateTime date=PrefC.GetDate(PrefName.DateDepositsStarted);
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					if(date<DateTime.Now.AddMonths(-1)) {
						log+=Lans.g("FormDatabaseMaintenance","Deposit start date needs to be reset.")+"\r\n";
					}
					else if(verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Deposit start date checked.")+"\r\n";
					}
					break;
				case DbmMode.Fix:
					//If the program locks up when trying to create a deposit slip, it's because someone removed the start date from the deposit edit window. Run this query to get back in.
					if(date<DateTime.Now.AddMonths(-1)) {
						command="UPDATE preference SET ValueString="+POut.Date(DateTime.Today.AddDays(-21))
						+" WHERE PrefName='DateDepositsStarted'";
						Db.NonQ(command);
						Signalods.SetInvalid(InvalidType.Prefs);
						log+=Lans.g("FormDatabaseMaintenance","Deposit start date reset.")+"\r\n";
					}
					else if(verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Deposit start date checked.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PreferenceMedicationsIndicateNone(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM medication where MedicationNum="+POut.Long(PrefC.GetLong(PrefName.MedicationsIndicateNone));
					if(PIn.Int(Db.GetCount(command))==0 && PrefC.GetString(PrefName.MedicationsIndicateNone)!="") {
						log+=Lans.g("FormDatabaseMaintenance","Preference \"MedicationsIndicateNone\" is an invalid value.")+"\r\n";
					}
					else if(verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Preference \"MedicationsIndicateNone\" checked.")+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="SELECT COUNT(*) FROM medication where MedicationNum="+POut.Long(PrefC.GetLong(PrefName.MedicationsIndicateNone));
					if(PIn.Int(Db.GetCount(command))==0 && PrefC.GetString(PrefName.MedicationsIndicateNone)!="") {
						Prefs.UpdateString(PrefName.MedicationsIndicateNone,"");
						Signalods.SetInvalid(InvalidType.Prefs);
						log+=Lans.g("FormDatabaseMaintenance","Preference \"MedicationsIndicateNone\" set to blank due to an invalid value.")+"\r\n";
					}
					else if(verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Preference \"MedicationsIndicateNone\" checked.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PreferenceProblemsIndicateNone(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM diseasedef where DiseaseDefNum="+POut.Long(PrefC.GetLong(PrefName.ProblemsIndicateNone));
					if(PIn.Int(Db.GetCount(command))==0 && PrefC.GetString(PrefName.ProblemsIndicateNone)!="") {
						log+=Lans.g("FormDatabaseMaintenance","Preference \"ProblemsIndicateNone\" is an invalid value.")+"\r\n";
					}
					else if(verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Preference \"ProblemsIndicateNone\" checked.")+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="SELECT COUNT(*) FROM diseasedef where DiseaseDefNum="+POut.Long(PrefC.GetLong(PrefName.ProblemsIndicateNone));
					if(PIn.Int(Db.GetCount(command))==0 && PrefC.GetString(PrefName.ProblemsIndicateNone)!="") {
						Prefs.UpdateString(PrefName.ProblemsIndicateNone,"");
						Signalods.SetInvalid(InvalidType.Prefs);
						log+=Lans.g("FormDatabaseMaintenance","Preference \"ProblemsIndicateNone\" set to blank due to an invalid value.")+"\r\n";
					}
					else if(verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Preference \"ProblemsIndicateNone\" checked.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PreferenceTimeCardOvertimeFirstDayOfWeek(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(PrefC.GetInt(PrefName.TimeCardOvertimeFirstDayOfWeek)<0 || PrefC.GetInt(PrefName.TimeCardOvertimeFirstDayOfWeek)>6) {
						log+=Lans.g("FormDatabaseMaintenance","Preference \"TimeCardOvertimeFirstDayOfWeek\" is an invalid value.")+"\r\n";
					}
					else if(verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Preference \"TimeCardOvertimeFirstDayOfWeek\" checked.")+"\r\n";
					}
					break;
				case DbmMode.Fix:
					if(PrefC.GetInt(PrefName.TimeCardOvertimeFirstDayOfWeek)<0 || PrefC.GetInt(PrefName.TimeCardOvertimeFirstDayOfWeek)>6) {
						Prefs.UpdateInt(PrefName.TimeCardOvertimeFirstDayOfWeek,0);//0==Sunday
						Signalods.SetInvalid(InvalidType.Prefs);
						log+=Lans.g("FormDatabaseMaintenance","Preference \"TimeCardOvertimeFirstDayOfWeek\" set to Sunday due to an invalid value.")+"\r\n";
					}
					else if(verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Preference \"TimeCardOvertimeFirstDayOfWeek\" checked.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string PreferencePracticeProv(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(modeCur==DbmMode.Breakdown) {
				return "";
			}
			string log="";
			string command="SELECT valuestring FROM preference WHERE prefname='PracticeDefaultProv'";
			DataTable table=Db.GetTable(command);
			if(table.Rows[0][0].ToString()!="") {
				if(verbose) {
					log+=Lans.g("FormDatabaseMaintenance","Default practice provider verified.")+"\r\n";
				}
			}
			else {
				log+=Lans.g("FormDatabaseMaintenance","No default provider set.")+"\r\n";
				if(modeCur!=DbmMode.Check) {
					if(DataConnection.DBtype==DatabaseType.Oracle) {
						command="SELECT ProvNum FROM provider WHERE IsHidden=0 ORDER BY itemorder";
					}
					else {//MySQL
						command="SELECT provnum FROM provider WHERE IsHidden=0 ORDER BY itemorder LIMIT 1";
					}
					table=Db.GetTable(command);
					command="UPDATE preference SET valuestring='"+table.Rows[0][0].ToString()+"' WHERE prefname='PracticeDefaultProv'";
					Db.NonQ(command);
					log+="  "+Lans.g("FormDatabaseMaintenance","Fixed.")+"\r\n";
				}
			}
			return log;
		}

		#endregion PerioMeasure, PlannedAppt, Preference------------------------------------------------------------------------------------------------
		#region ProcButton, ProcedureCode---------------------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string ProcButtonItemsDeleteWithInvalidAutoCode(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command=@"SELECT COUNT(*) FROM procbuttonitem WHERE CodeNum=0 AND NOT EXISTS(
						SELECT * FROM autocode WHERE autocode.AutoCodeNum=procbuttonitem.AutoCodeNum)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","ProcButtonItems found with invalid autocode: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command=@"DELETE FROM procbuttonitem WHERE CodeNum=0 AND NOT EXISTS(
						SELECT * FROM autocode WHERE autocode.AutoCodeNum=procbuttonitem.AutoCodeNum)";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0) {
						Signalods.SetInvalid(InvalidType.ProcButtons);
					}
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","ProcButtonItems deleted due to invalid autocode: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurecodeCategoryNotSet(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			List<Def> listProcCodeCats=Defs.GetDefsForCategory(DefCat.ProcCodeCats,true);
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM procedurecode WHERE procedurecode.ProcCat=0";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						if(listProcCodeCats.Count==0) {
							log+=Lans.g("FormDatabaseMaintenance","Procedure codes with no categories found but cannot be fixed because there are no visible proc code categories.")+"\r\n";
							return log;
						}
						log+=Lans.g("FormDatabaseMaintenance","Procedure codes with no category found")+": "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					if(listProcCodeCats.Count==0) {
						log+=Lans.g("FormDatabaseMaintenance","Procedure codes with no categories cannot be fixed because there are no visible proc code categories.")+"\r\n";
						return log;
					}
					command="UPDATE procedurecode SET procedurecode.ProcCat="+POut.Long(listProcCodeCats[0].DefNum)+" WHERE procedurecode.ProcCat=0";
					long numberfixed=Db.NonQ(command);
					if(numberfixed>0) {
						Signalods.SetInvalid(InvalidType.ProcCodes);
					}
					if(numberfixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedure codes with no category fixed")+": "+numberfixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurecodeInvalidProvNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command=@"SELECT procedurecode.CodeNum FROM procedurecode 
				LEFT JOIN provider ON procedurecode.ProvNumDefault=provider.ProvNum 
				WHERE provider.ProvNum IS NULL 
				AND procedurecode.ProvNumDefault!=0";
			List<long> listProcNums=Db.GetListLong(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(listProcNums.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedure codes with invalid Default Provider found")+": "+listProcNums.Count+"\r\n";
					}
					break;
				case DbmMode.Fix:
					if(listProcNums.Count>0) {
						command="UPDATE procedurecode SET procedurecode.ProvNumDefault=0 WHERE procedurecode.CodeNum IN ("+String.Join(",",listProcNums)+")";
						Db.NonQ(command);
					}
					if(listProcNums.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedure codes with invalid Default Provider fixed")+": "+listProcNums.Count+"\r\n";
					}
					break;
			}
			return log;
		}

		#endregion ProcButton, ProcedureCode------------------------------------------------------------------------------------------------------------
		#region ProcedureLog----------------------------------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string ProcedurelogAttachedToApptWithProcStatusDeleted(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM procedurelog "
						+"WHERE ProcStatus=6 AND (AptNum!=0 OR PlannedAptNum!=0)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Deleted procedures still attached to appointments: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="UPDATE procedurelog SET AptNum=0,PlannedAptNum=0 "
						+"WHERE ProcStatus=6 "
						+"AND (AptNum!=0 OR PlannedAptNum!=0)";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Deleted procedures detached from appointments: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurelogAttachedToWrongAppts(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					if(DataConnection.DBtype==DatabaseType.Oracle) {
						command="SELECT COUNT(*) FROM procedurelog p "
							+"WHERE (SELECT COUNT(*) FROM appointment a WHERE p.AptNum=a.AptNum AND p.PatNum!=a.PatNum AND ROWNUM<=1)>0";
					}
					else {
						command="SELECT COUNT(*) FROM appointment,procedurelog "
							+"WHERE procedurelog.AptNum=appointment.AptNum AND procedurelog.PatNum != appointment.PatNum";
					}
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedures attached to appointments with incorrect patient: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					if(DataConnection.DBtype==DatabaseType.Oracle) {
						command="UPDATE procedurelog p SET p.AptNum=0 "
							+"WHERE (SELECT COUNT(*) FROM appointment a WHERE p.AptNum=a.AptNum AND p.PatNum!=a.PatNum AND ROWNUM<=1)>0";
					}
					else {
						command="UPDATE appointment,procedurelog SET procedurelog.AptNum=0 "
							+"WHERE procedurelog.AptNum=appointment.AptNum AND procedurelog.PatNum != appointment.PatNum";
					}
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedures detached from appointments: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurelogAttachedToWrongApptDate(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					if(DataConnection.DBtype==DatabaseType.Oracle) {
						command=@"SELECT COUNT(*) FROM procedurelog p
							WHERE p.ProcStatus=2 AND 
							(SELECT COUNT(*) FROM appointment a WHERE a.AptNum=p.AptNum AND TO_DATE(p.ProcDate)!=TO_DATE(a.AptDateTime) AND ROWNUM<=1)>0";
					}
					else {
						command=@"SELECT COUNT(*) FROM procedurelog,appointment
							WHERE procedurelog.AptNum=appointment.AptNum
							AND DATE(procedurelog.ProcDate) != DATE(appointment.AptDateTime)
							AND procedurelog.ProcStatus=2";
					}
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedures which are attached to appointments with mismatched dates: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					if(DataConnection.DBtype==DatabaseType.Oracle) {
						command=@"UPDATE procedurelog p
							SET p.AptNum=0
							WHERE p.ProcStatus=2 AND 
							(SELECT COUNT(*) FROM appointment a WHERE a.AptNum=p.AptNum AND TO_DATE(p.ProcDate)!=TO_DATE(a.AptDateTime) AND ROWNUM<=1)>0";
					}
					else {
						command=@"UPDATE procedurelog,appointment
							SET procedurelog.AptNum=0
							WHERE procedurelog.AptNum=appointment.AptNum
							AND DATE(procedurelog.ProcDate) != DATE(appointment.AptDateTime)
							AND procedurelog.ProcStatus=2";//only detach completed procs 
					}
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedures detached from appointments due to mismatched dates: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurelogBaseUnitsZero(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					//zero--------------------------------------------------------------------------------------
					command=@"SELECT COUNT(*) FROM procedurelog 
						WHERE baseunits != (SELECT procedurecode.BaseUnits FROM procedurecode WHERE procedurecode.CodeNum=procedurelog.CodeNum)
						AND baseunits=0";
					//we do not want to change this automatically.  Do not fix these!
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedure BaseUnits are zero and are not matching procedurecode BaseUnits: ")+numFound+"\r\n";
					}
					//not zero----------------------------------------------------------------------------------
					command=@"SELECT COUNT(*)
						FROM procedurelog
						WHERE BaseUnits!=0
						AND (SELECT procedurecode.BaseUnits FROM procedurecode WHERE procedurecode.CodeNum=procedurelog.CodeNum)=0";
					//very safe to change them back to zero.
					numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedure BaseUnits not zero, but procedurecode BaseUnits are zero: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					//first situation: don't fix.
					//second situation:
					//Writing the query this way allows it to work with Oracle.
					command=@"UPDATE procedurelog
						SET BaseUnits=0
						WHERE BaseUnits!=0 
						AND (SELECT procedurecode.BaseUnits FROM procedurecode WHERE procedurecode.CodeNum=procedurelog.CodeNum)=0";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedure BaseUnits set to zero because procedurecode BaseUnits are zero: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurelogCodeNumInvalid(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM procedurelog WHERE NOT EXISTS (SELECT * FROM procedurecode WHERE procedurecode.CodeNum=procedurelog.CodeNum)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedures found with invalid CodeNum")+": "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					long badCodeNum=0;
					if(!ProcedureCodes.IsValidCode("~BAD~")) {
						ProcedureCode badCode=new ProcedureCode();
						badCode.ProcCode="~BAD~";
						badCode.Descript="Invalid procedure";
						badCode.AbbrDesc="Invalid procedure";
						badCode.ProcCat=Defs.GetByExactNameNeverZero(DefCat.ProcCodeCats,"Never Used");
						ProcedureCodes.Insert(badCode);
						badCodeNum=badCode.CodeNum;
					}
					else {
						badCodeNum=ProcedureCodes.GetCodeNum("~BAD~");
					}
					command="UPDATE procedurelog SET CodeNum=" + POut.Long(badCodeNum) + " WHERE NOT EXISTS (SELECT * FROM procedurecode WHERE procedurecode.CodeNum=procedurelog.CodeNum)";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedures fixed with invalid CodeNum")+": "+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string ProcedurelogLabAttachedToDeletedProc(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(!CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				return Lans.g("FormDatabaseMaintenance","Skipped. Local computer region must be set to Canada to run.");
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM procedurelog "
						+"WHERE ProcStatus=2 AND ProcNumLab IN(SELECT ProcNum FROM procedurelog WHERE ProcStatus=6)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Completed procedure labs attached to deleted procedures: ")+numFound;
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Double click to run the fix and see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					command="SELECT patient.PatNum,patient.FName,patient.LName FROM procedurelog "
						+"LEFT JOIN patient ON procedurelog.PatNum=patient.PatNum "
						+"WHERE ProcStatus="+POut.Int((int)ProcStat.C)+" AND ProcNumLab IN(SELECT ProcNum FROM procedurelog WHERE ProcStatus="+POut.Int((int)ProcStat.D)+") ";
					if(DataConnection.DBtype==DatabaseType.MySql) {
						command+="GROUP BY patient.PatNum";
					}
					else {//Oracle
						command+="GROUP BY patient.PatNum,patient.FName,patient.LName";
					}
					DataTable table=Db.GetTable(command);
					if(DataConnection.DBtype==DatabaseType.MySql) {
						command="UPDATE procedurelog plab,procedurelog p "
						+"SET plab.ProcNumLab=0 "
						+"WHERE plab.ProcStatus="+POut.Int((int)ProcStat.C)+" AND plab.ProcNumLab=p.ProcNum AND p.ProcStatus="+POut.Int((int)ProcStat.D);
					}
					else {//Oracle
						command="UPDATE procedurelog plab SET plab.ProcNumLab=0 "
							+"WHERE plab.ProcStatus="+POut.Int((int)ProcStat.C)+" "
							+"AND plab.ProcNumLab IN (SELECT p.ProcNum FROM procedurelog p WHERE p.ProcStatus="+POut.Int((int)ProcStat.D)+") ";
					}
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Patients with completed lab procedures detached from deleted procedures: ")+numberFixed;
						if(table.Rows.Count>0) {
							log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
							log+=string.Join("\r\n",table.Select().Select(x => "#"+x["PatNum"].ToString()+":"+x["FName"].ToString()+" "+x["LName"].ToString()))+"\r\n";
						}
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string ProcedurelogMultipleClaimProcForInsSub(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT patient.PatNum,patient.LName,patient.FName,procedurelog.ProcDate,procedurecode.ProcCode "
				+"FROM claimproc "
				+"INNER JOIN procedurelog ON procedurelog.ProcNum=claimproc.ProcNum "
				+"INNER JOIN procedurecode ON procedurecode.CodeNum=procedurelog.CodeNum "
				+"INNER JOIN patient ON patient.PatNum=claimproc.PatNum "
				+"WHERE (claimproc.Status="+POut.Int((int)ClaimProcStatus.NotReceived)+" "
				+"OR claimproc.Status="+POut.Int((int)ClaimProcStatus.Received)+" "
				+"OR claimproc.Status="+POut.Int((int)ClaimProcStatus.Estimate)+") "
				+"AND procedurelog.ProcStatus!="+POut.Int((int)ProcStat.D)+" " //exclude deleted procedures
				+"GROUP BY claimproc.ProcNum, claimproc.InsSubNum, claimproc.PlanNum "
					+", patient.PatNum, patient.LName, patient.FName, procedurelog.ProcDate, procedurecode.ProcCode "//For Oracle.
				+"HAVING COUNT(*)>1 "
				+"ORDER BY patient.LName, patient.FName, procedurelog.ProcDate, procedurecode.ProcCode";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			string log=Lans.g("FormDatabaseMaintenance","Procedures with multiple claimprocs for the same insurance plan")+": "+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {//Only the fix should show the entire list of items.
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {//Running the fix and there are items to show.
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							log+="   "+table.Rows[i]["PatNum"].ToString()+"-"+table.Rows[i]["LName"].ToString()+", "+table.Rows[i]["FName"].ToString()
								+"  Procedure Date: "+PIn.Date(table.Rows[i]["ProcDate"].ToString()).ToShortDateString()+"  "+table.Rows[i]["ProcCode"];
							log+="\r\n";
						}
						log+=Lans.g("FormDatabaseMaintenance","   They need to be fixed manually.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurelogProvNumMissing(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT COUNT(*) FROM procedurelog WHERE ProvNum=0";
			int numFound=PIn.Int(Db.GetCount(command));
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
				case DbmMode.Fix:
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedures with missing provnums found: ")+numFound+"\r\n";
						log+=Lans.g("FormDatabaseMaintenance","   A safe fix is under development.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurelogToothNums(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			if(modeCur==DbmMode.Breakdown) {
				return "";
			}
			string log="";
			//The logic for checking whether a tooth is invalid was obtained from Tooth.IsValidDB().
			string command="SELECT ProcNum,ToothNum,PatNum FROM procedurelog "
				+"WHERE ToothNum!='' "
				+"AND ToothNum NOT REGEXP '^[A-T]S?$' "//supernumerary
				+"AND (ToothNum NOT REGEXP '^[1-9][0-9]?$' "//matches 1 or 2 digits, leading 0 not allowed
				+"OR (CAST(ToothNum AS UNSIGNED)>32 AND CAST(ToothNum AS UNSIGNED)<51) "
				+"OR CAST(ToothNum AS UNSIGNED)>82) ";
			DataTable table=Db.GetTable(command);
			Patient Lim=null;
			string toothNum;
			int numberFixed=0;
			for(int i = 0;i<table.Rows.Count;i++) {
				toothNum=table.Rows[i][1].ToString();
				if(verbose) {
					Lim=Patients.GetLim(Convert.ToInt32(table.Rows[i][2].ToString()));
				}
				if(string.CompareOrdinal(toothNum,"a")>=0 && string.CompareOrdinal(toothNum,"t")<=0) {
					if(modeCur!=DbmMode.Check) {
						command="UPDATE procedurelog SET ToothNum='"+toothNum.ToUpper()+"' WHERE ProcNum="+table.Rows[i][0].ToString();
						Db.NonQ(command);
					}
					if(verbose) {
						log+=Lim.GetNameLF()+" "+toothNum+" - "+toothNum.ToUpper()+"\r\n";
					}
					numberFixed++;
				}
				else {
					if(modeCur!=DbmMode.Check) {
						command="UPDATE procedurelog SET ToothNum='1' WHERE ProcNum="+table.Rows[i][0].ToString();
						Db.NonQ(command);
					}
					if(verbose) {
						log+=Lim.GetNameLF()+" "+toothNum+" - 1\r\n";
					}
					numberFixed++;
				}
			}
			if(numberFixed!=0 || verbose) {
				log+=Lans.g("FormDatabaseMaintenance","Check for invalid tooth numbers complete.  Records checked: ")
					+Db.GetCount("SELECT COUNT(*) FROM procedurelog")+". "+Lans.g("FormDatabaseMaintenance","Records invalid: ")+numberFixed.ToString()+"\r\n";
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string ProcedurelogTpAttachedToClaim(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT procedurelog.ProcNum,claim.ClaimNum,claim.DateService,patient.PatNum,patient.LName,patient.FName,procedurecode.ProcCode "
				+"FROM procedurelog,claim,claimproc,patient,procedurecode "
				+"WHERE procedurelog.ProcNum=claimproc.ProcNum "
				+"AND claim.ClaimNum=claimproc.ClaimNum "
				+"AND claim.PatNum=patient.PatNum "
				+"AND procedurelog.CodeNum=procedurecode.CodeNum "
				+"AND procedurelog.ProcStatus!="+POut.Long((int)ProcStat.C)+" "//procedure not complete
				+"AND (claim.ClaimStatus='W' OR claim.ClaimStatus='S' OR claim.ClaimStatus='R') "//waiting, sent, or received
				+"AND (claim.ClaimType='P' OR claim.ClaimType='S' OR claim.ClaimType='Other')";//pri, sec, or other.  Eliminates preauths.
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			string log=Lans.g("FormDatabaseMaintenance","Procedures attached to claims with status of TP: ")+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {//Only the fix should show the entire list of items.
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {//Running the fix and there are items to show.
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							log+=Lans.g("FormDatabaseMaintenance","Patient")
									+" "+table.Rows[i]["FName"].ToString()
									+" "+table.Rows[i]["LName"].ToString()
									+" #"+table.Rows[i]["PatNum"].ToString()
									+", for claim service date "+PIn.Date(table.Rows[i]["DateService"].ToString()).ToShortDateString()
									+", procedure code "+table.Rows[i]["ProcCode"].ToString()+"\r\n";
						}
						log+=Lans.g("FormDatabaseMaintenance","   They need to be fixed manually.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string ProcedurelogNotComplAttachedToComplLabCanada(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(!CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				return Lans.g("FormDatabaseMaintenance","Skipped. Local computer region must be set to Canada to run.");
			}
			string command="SELECT pc.ProcCode ProcCode,pclab.ProcCode ProcCodeLab,proc.PatNum,proc.ProcDate "
				+"FROM procedurelog proc "
				+"INNER JOIN procedurecode pc ON pc.CodeNum=proc.CodeNum "
				+"INNER JOIN procedurelog lab ON proc.ProcNum=lab.ProcNumLab AND lab.ProcStatus="+POut.Long((int)ProcStat.C)+" "
				+"INNER JOIN procedurecode pclab ON pclab.CodeNum=lab.CodeNum "
				+"WHERE proc.ProcStatus!="+POut.Long((int)ProcStat.C);
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			string log=Lans.g("FormDatabaseMaintenance","Completed lab fees with treatment planned procedures attached")+": "+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {//Only the fix should show the entire list of items.
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {//Running the fix and there are items to show.
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							log+=Lans.g("FormDatabaseMaintenance","Completed lab fee")+" "+table.Rows[i]["ProcCodeLab"].ToString()+" "
									+Lans.g("FormDatabaseMaintenance","is attached to non-complete procedure")+" "+table.Rows[i]["ProcCode"].ToString()+" "
									+Lans.g("FormDatabaseMaintenance","on date")+" "+PIn.Date(table.Rows[i]["ProcDate"].ToString()).ToShortDateString()+". "
									+Lans.g("FormDatabaseMaintenance","PatNum: ")+table.Rows[i]["PatNum"].ToString()+"\r\n";
						}
						log+=Lans.g("FormDatabaseMaintenance","   Fix manually from within the Chart module.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurelogUnitQtyZero(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM procedurelog WHERE UnitQty=0";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedures with UnitQty=0 found: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command=@"UPDATE procedurelog        
						SET UnitQty=1
						WHERE UnitQty=0";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedures changed from UnitQty=0 to UnitQty=1: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurelogWithInvalidProvNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command;
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM procedurelog WHERE ProvNum > 0 AND ProvNum NOT IN (SELECT ProvNum FROM provider)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedures with invalid ProvNum found")+": "+numFound.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="UPDATE procedurelog SET ProvNum="+POut.Long(PrefC.GetLong(PrefName.PracticeDefaultProv))+
							" WHERE ProvNum > 0 AND ProvNum NOT IN (SELECT ProvNum FROM provider)";
					long numFixed=Db.NonQ(command);
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedures with invalid ProvNum fixed")+": "+numFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurelogWithInvalidAptNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) "
						+"FROM procedurelog "
						+"WHERE (AptNum NOT IN(SELECT AptNum FROM appointment) AND AptNum!=0) "
						+"OR (PlannedAptNum NOT IN(SELECT AptNum FROM appointment) AND PlannedAptNum!=0)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedures attached to invalid appointments")+": "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="UPDATE procedurelog SET AptNum=0 "
						+"WHERE AptNum NOT IN(SELECT AptNum FROM appointment) AND AptNum!=0";
					long numberFixed=Db.NonQ(command);
					command="UPDATE procedurelog SET PlannedAptNum=0 "
						+"WHERE PlannedAptNum NOT IN(SELECT AptNum FROM appointment) AND PlannedAptNum!=0";
					numberFixed+=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedures with invalid appointments fixed")+": "+numberFixed.ToString()+"\r\n";//Do we care enough that this number could be inflated if a procedure had both an invalid AptNum AND PlannedNum?
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProcedurelogWithInvalidClinicNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) "
						+"FROM procedurelog "
						+"WHERE ClinicNum NOT IN(SELECT ClinicNum FROM clinic) AND ClinicNum!=0 ";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedures attached to invalid clinics")+": "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="UPDATE procedurelog SET ClinicNum=0 "
						+"WHERE ClinicNum NOT IN(SELECT ClinicNum FROM clinic) and ClinicNum!=0 ";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Procedures with invalid clinics fixed")+": "+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}
		#endregion ProcedureLog-------------------------------------------------------------------------------------------------------------------------
		#region ProgramProperty, Provider, QuickPasteNote-----------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string ProgramPropertiesDuplicatesForHQ(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string progNumStr=POut.Long(Programs.GetProgramNum(ProgramName.Xcharge))+","+POut.Long(Programs.GetProgramNum(ProgramName.PayConnect));
			//Min may not be the oldest when using random primary keys, but we have to pick one.  In most all cases theyre identical anyway.
			string command="SELECT MIN(ProgramPropertyNum) ProgramPropertyNum,COUNT(*) Count "
					+"FROM programproperty "
					+"WHERE ClinicNum=0 "
					+"AND ProgramNum IN ("+progNumStr+") "
					+"GROUP BY ProgramNum,PropertyDesc";
			DataTable tableProgProps=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					int numFound=tableProgProps.Select().Select(x => PIn.Int(x["Count"].ToString())-1).Sum();
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","X-Charge and/or PayConnect duplicate program property entries found: ")
							+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="DELETE FROM programproperty WHERE ClinicNum=0 AND ProgramNum IN ("+progNumStr+") "
						+"AND ProgramPropertyNum NOT IN ("+string.Join(",",tableProgProps.Select().Select(x => PIn.Long(x["ProgramPropertyNum"].ToString())))+")";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","X-Charge and/or PayConnect duplicate program property entries deleted: ")
							+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProgramPropertiesMissingForClinic(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			//X-Charge and PayConnect are currently the only program links that use ClinicNum.
			string progNumStr=POut.Long(Programs.GetProgramNum(ProgramName.Xcharge))+","+POut.Long(Programs.GetProgramNum(ProgramName.PayConnect));
			string command="SELECT DISTINCT pphq.*,clinic.ClinicNum missingClinicNum "//Distinct in case there are duplicate prog props with a ClinicNum 0.
				+"FROM programproperty pphq "
				+"INNER JOIN clinic ON TRUE "
				+"LEFT JOIN programproperty ppcl ON ppcl.ProgramNum=pphq.ProgramNum "
				+"AND ppcl.PropertyDesc=pphq.PropertyDesc "
					+"AND ppcl.ClinicNum=clinic.ClinicNum "
				+"WHERE pphq.ProgramNum IN ("+progNumStr+") "
				+"AND pphq.ClinicNum=0 "
				+"AND pphq.PropertyDesc!='' "
				+"AND ppcl.ClinicNum IS NULL ";
			DataTable tableProgProps=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					int numFound=tableProgProps.Rows.Count;
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","X-Charge and/or PayConnect missing program property entries found: ")
							+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					List<ProgramProperty> listProgProps=Crud.ProgramPropertyCrud.TableToList(tableProgProps);
					for(int i = 0;i<listProgProps.Count;i++) {
						listProgProps[i].ClinicNum=PIn.Long(tableProgProps.Rows[i]["missingClinicNum"].ToString());
						ProgramProperties.Insert(listProgProps[i]);
					}
					long numberFixed=tableProgProps.Rows.Count;
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","X-Charge and/or PayConnect missing program property entries inserted: ")
							+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr(HasBreakDown = true)]
		public static string ProviderHiddenWithClaimPayments(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command=@"SELECT MAX(claimproc.ProcDate) ProcDate,provider.ProvNum
				FROM claimproc,provider
				WHERE claimproc.ProvNum=provider.ProvNum
				AND provider.IsHidden=1
				AND claimproc.InsPayAmt>0
				GROUP BY provider.ProvNum";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			string log=Lans.g("FormDatabaseMaintenance","Hidden providers with claim payments")+": "+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count > 0) {
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count > 0) {
						Provider prov;
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							prov=Providers.GetProv(PIn.Long(table.Rows[i]["ProvNum"].ToString()));
							log+=prov.Abbr+" "+Lans.g("FormDatabaseMaintenance","has claim payments entered as recently as")+" "
								+PIn.Date(table.Rows[i]["ProcDate"].ToString()).ToShortDateString()+"\r\n";
						}
						log+="   "+Lans.g("FormDatabaseMaintenance","This data will be missing on income reports.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string ProviderWithInvalidFeeSched(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command=@"SELECT COUNT(*) FROM provider WHERE FeeSched NOT IN (SELECT FeeSchedNum FROM feesched)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Providers found with invalid FeeSched: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command=@"UPDATE provider SET FeeSched="+POut.Long(FeeScheds.GetFirst(true).FeeSchedNum)+" "
						+"WHERE FeeSched NOT IN (SELECT FeeSchedNum FROM feesched)";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Providers whose FeeSched has been changed: ")
						+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string QuickPasteNoteWithInvalidCatNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command=@"SELECT COUNT(*) FROM quickpastenote WHERE QuickPasteCatNum=0";
			int numFound=PIn.Int(Db.GetCount(command));
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Quick Paste Notes with an invalid category num")+": "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					long numberFixed=0;
					if(numFound>0) {
						QuickPasteCat quickPasteCatNew=new QuickPasteCat();
						quickPasteCatNew.Description="DBM GENERATED";
						QuickPasteCats.Insert(quickPasteCatNew);
						command=@"UPDATE quickpastenote SET QuickPasteCatNum="+POut.Long(quickPasteCatNew.QuickPasteCatNum)+" WHERE QuickPasteCatNum=0";
						numberFixed=Db.NonQ(command);
					}
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Quick Paste Notes with an invalid category num fixed")+": "+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		#endregion ProgramProperty, Provider, QuickPasteNote--------------------------------------------------------------------------------------------
		#region Recall, RecallTrigger, RefAttach, RxAlert---------------------------------------------------------------------------------------

		[DbmMethodAttr(HasBreakDown = true)]
		public static string RecallDuplicatesWarn(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			if(RecallTypes.PerioType<1 || RecallTypes.ProphyType<1) {
				return Lans.g("FormDatabaseMaintenance","Warning!  Recall types not set up properly.  There must be at least one of each type: perio and prophy.")+"\r\n";
			}
			string command="SELECT FName,LName,COUNT(*) countDups FROM patient LEFT JOIN recall ON recall.PatNum=patient.PatNum "
				+"AND (recall.RecallTypeNum="+POut.Long(RecallTypes.PerioType)+" "
				+"OR recall.RecallTypeNum="+POut.Long(RecallTypes.ProphyType)+") "
				+"GROUP BY FName,LName,patient.PatNum HAVING countDups>1";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			string log=Lans.g("FormDatabaseMaintenance","Number of patients with duplicate recalls: ")+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {//Only the fix should show the entire list of items.
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Breakdown:
				case DbmMode.Fix:
					if(table.Rows.Count>0) {//Running the fix and there are items to show.
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							if(i%3==0) {
								log+="\r\n   ";
							}
							log+=table.Rows[i]["FName"].ToString()+" "+table.Rows[i]["LName"].ToString()+"; ";
						}
						log+=Lans.g("FormDatabaseMaintenance","   They need to be fixed manually.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string RecallsWithInvalidRecallType(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command=@"SELECT recall.RecallTypeNum 
				FROM recall 
				LEFT JOIN recalltype ON recalltype.RecallTypeNum=recall.RecallTypeNum 
				WHERE recalltype.RecallTypeNum IS NULL";
			List<long> listRecallTypeNums=Db.GetListLong(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					int numFound=listRecallTypeNums.Count;
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Recalls found with invalid recall types:")+" "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					//Inserting temporary recall types so that the recalls are no longer orphaned
					long numberFixed=listRecallTypeNums.Count;
					listRecallTypeNums=listRecallTypeNums.Distinct().ToList();
					for(int i = 0;i<listRecallTypeNums.Count;i++) {
						command="INSERT INTO recalltype (RecallTypeNum,Description,DefaultInterval,TimePattern,Procedures) VALUES ("
							+POut.Long(listRecallTypeNums[i])+",'Temporary Recall "+POut.Int(i+1)+"',0,'','')";
						Db.NonQ(command);
					}
					long numberFixedTypes=listRecallTypeNums.Count;
					if(numberFixedTypes > 0) {
						Signalods.SetInvalid(InvalidType.RecallTypes);
					}
					if(numberFixed > 0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Recalls fixed with invalid recall types:")+" "+numberFixed+". "
							+Lans.g("FormDatabaseMaintenance","Temporary recall types added:")+" "+numberFixedTypes+". "
							+Lans.g("FormDatabaseMaintenance","Go to Setup | Appointments | Recall Types "
								+"to either rename them or remove all recalls from these recall types.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string RecallTriggerDeleteBadCodeNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM recalltrigger WHERE NOT EXISTS (SELECT * FROM procedurecode WHERE procedurecode.CodeNum=recalltrigger.CodeNum)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Recall triggers found with bad codenum: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command=@"DELETE FROM recalltrigger
						WHERE NOT EXISTS (SELECT * FROM procedurecode WHERE procedurecode.CodeNum=recalltrigger.CodeNum)";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0) {
						Signalods.SetInvalid(InvalidType.RecallTypes);
					}
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Recall triggers deleted due to bad codenum: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string RefAttachDeleteWithInvalidReferral(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM refattach WHERE ReferralNum NOT IN (SELECT ReferralNum FROM referral)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Ref attachments found with invalid referrals: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="DELETE FROM refattach WHERE ReferralNum NOT IN (SELECT ReferralNum FROM referral)";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Ref attachments with invalid referrals deleted: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>Finds patients that have a more than 1 from referral with the same order.</summary>
		[DbmMethodAttr]
		public static string RefAttachesWithDuplicateOrder(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command=@"SELECT DISTINCT PatNum 
						FROM refattach 
						GROUP BY PatNum,ItemOrder
						HAVING COUNT(*) > 1";
					int numFound=Db.GetListLong(command).Count;
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Patients found with multiple referral attachments of the same order:")+" "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command=@"SELECT refattach.*
						FROM (
							SELECT DISTINCT PatNum
							FROM refattach
							GROUP BY PatNum,ItemOrder
							HAVING COUNT(*) > 1
						) multattach
						INNER JOIN refattach ON refattach.PatNum=multattach.PatNum";
					List<RefAttach> listAttaches=Crud.RefAttachCrud.SelectMany(command);
					foreach(List<RefAttach> listAttachesForPat in listAttaches.GroupBy(x => x.PatNum).Select(x => x.ToList())) {
						//Change the order of all ref attaches on the patient so that none have the same ItemOrder.
						int itemOrder=1;
						foreach(RefAttach attach in listAttachesForPat.OrderBy(x => x.ItemOrder).ThenBy(x => x.RefDate).ThenBy(x => x.RefAttachNum)) {
							RefAttach attachOld=attach.Copy();
							attach.ItemOrder=itemOrder++;
							RefAttaches.Update(attach,attachOld);
						}
					}
					long numberFixed=listAttaches.Select(x => x.PatNum).Distinct().Count();
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Patients fixed with multiple referral attachments of the same order:")+" "+numberFixed+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string RxAlertBadAllergyDefNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM rxalert WHERE rxalert.AllergyDefNum!=0 AND NOT EXISTS (SELECT * FROM allergydef WHERE allergydef.AllergyDefNum=rxalert.AllergyDefNum)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Rx alerts with bad allergy definitions: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					//command=@"SELECT * FROM rxalert WHERE NOT EXISTS (SELECT * FROM allergydef WHERE allergydef.AllergyDefNum=rxalert.AllergyDefNum)";
					//table=Db.GetTable(command);
					command="UPDATE rxalert SET AllergyDefNum=0 WHERE rxalert.AllergyDefNum!=0 AND NOT EXISTS (SELECT * FROM allergydef WHERE allergydef.AllergyDefNum=rxalert.AllergyDefNum)";
					long rowsChanged=Db.NonQ(command);
					if(rowsChanged>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Rx alerts with bad allergy definitions cleared: ")+rowsChanged.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		#endregion Recall, RecallTrigger, RefAttach, RxAlert------------------------------------------------------------------------------------
		#region ScheduleOp, Schedule, SecurityLog, Sheet------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string ScheduleOpsInvalidScheduleNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM scheduleop WHERE NOT EXISTS(SELECT * FROM schedule WHERE scheduleop.ScheduleNum=schedule.ScheduleNum)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Scheduleops with invalid ScheduleNums found")+": "+numFound.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="DELETE FROM scheduleop WHERE NOT EXISTS(SELECT * FROM schedule WHERE scheduleop.ScheduleNum=schedule.ScheduleNum)";
					long numFixed=Db.NonQ(command);
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Scheduleops with invalid ScheduleNums deleted")+": "+numFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string SchedulesBlockoutStopBeforeStart(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM schedule WHERE StopTime<StartTime";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Schedules and blockouts having stop time before start time: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="DELETE FROM schedule WHERE StopTime<StartTime";
					long numFixed=Db.NonQ(command);
					if(numFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Schedules and blockouts having stop time before start time fixed: ")+numFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string SchedulesDeleteHiddenProviders(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM provider WHERE IsHidden=1 AND ProvNum IN (SELECT ProvNum FROM schedule WHERE SchedDate > "+DbHelper.Now()+" GROUP BY ProvNum)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Hidden providers found on future schedules: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="SELECT ProvNum FROM provider WHERE IsHidden=1 AND ProvNum IN (SELECT ProvNum FROM schedule WHERE SchedDate > "+DbHelper.Now()+" GROUP BY ProvNum)";
					DataTable table=Db.GetTable(command);
					List<long> provNums=new List<long>();
					for(int i = 0;i<table.Rows.Count;i++) {
						provNums.Add(PIn.Long(table.Rows[i]["ProvNum"].ToString()));
					}
					Providers.RemoveProvsFromFutureSchedule(provNums);//Deletes future schedules for providers.
					if(provNums.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Hidden providers found on future schedules fixed: ")+provNums.Count.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string SchedulesDeleteShort(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command=@"SELECT schedule.ScheduleNum,schedule.StartTime,schedule.StopTime 
				FROM schedule
				WHERE schedule.Status="+POut.Int((int)SchedStatus.Open)/*closed and holiday statuses do not use starttime and stoptime*/+@"
				AND (schedule.Note=''"/*we don't want to remove provider notes, employee notes, or pratice notes.*/+@"
				OR schedule.SchedType="+POut.Int((int)ScheduleType.WebSchedASAP)+")";
			DataTable table=Db.GetTable(command);
			List<long> listSchedulesToDelete=new List<long>();
			foreach(DataRow row in table.Rows) {
				TimeSpan startTime=PIn.Time(row["StartTime"].ToString());
				TimeSpan stopTime=PIn.Time(row["StopTime"].ToString());
				if(stopTime-startTime < new TimeSpan(0,5,0)) {//Schedule items less than five minutes won't show up on the schedule.
					listSchedulesToDelete.Add(PIn.Long(row["ScheduleNum"].ToString()));
				}
			}
			switch(modeCur) {
				case DbmMode.Check:
					int numFound=listSchedulesToDelete.Count;
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Schedule blocks invalid:")+" "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					int numberFixed=listSchedulesToDelete.Count;
					if(listSchedulesToDelete.Count > 0) {
						command="DELETE FROM schedule WHERE ScheduleNum IN("+string.Join(",",listSchedulesToDelete)+")";
						Db.NonQ(command);
					}
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Schedule blocks fixed:")+" "+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string SchedulesDeleteProvClosed(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM schedule WHERE SchedType=1 AND Status=1";//type=prov,status=closed
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Schedules found which are causing printing issues: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="DELETE FROM schedule WHERE SchedType=1 AND Status=1";//type=prov,status=closed
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Schedules deleted that were causing printing issues: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>This function will fix any FKey entries in securitylog that point to entries in other tables that have been deleted (orphaned FKeys).
		///It uses reflection to find all tables using audit trail Fkey columns and their respective permissions.
		///This method does not need to change even if more permissions are added.</summary>
		//No longer needed, removed per Nathan.
		//public static string SecurityLogInvalidFKey(bool verbose,DbmMode modeCur) {
		//	if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
		//		return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
		//	}
		//	if(modeCur==DbmMode.Breakdown) {
		//		return "";
		//	}
		//	string log="";
		//	long numFoundOrFixed=0;
		//	Assembly assembly=Assembly.GetExecutingAssembly();
		//	Type[] arrTypes;
		//	try {
		//		arrTypes=assembly.GetTypes();
		//	}
		//	catch(ReflectionTypeLoadException ex) {//Some dependencies were probably not able to be loaded
		//		log+="Error: Unable to load types from OpenDentBusiness. Try running the method on a different computer.\r\n";
		//		if(verbose) {
		//			log+="Types successfully loaded:\r\n"+string.Join("\r\n",ex.Types.Where(x => x!=null))+"\r\n";
		//		}
		//		return log;
		//	}
		//	foreach(Type type in arrTypes) {
		//		if(type.Namespace!="OpenDentBusiness" || !type.IsClass) {
		//			continue;
		//		}
		//		string tableName=type.Name.ToLower();
		//		List<CrudTableAttribute> listCrudTableAttributes=type.GetCustomAttributes(typeof(CrudTableAttribute),true)
		//			.Select(x => (CrudTableAttribute)x).ToList();
		//		//Get audit trail permissions
		//		if(listCrudTableAttributes.Count!=1 || listCrudTableAttributes[0].AuditPerms==CrudAuditPerm.None) {
		//			continue;
		//		}
		//		List<Permissions> listPermissions=GroupPermissions.GetPermsFromCrudAuditPerm(listCrudTableAttributes[0].AuditPerms);
		//		if(listPermissions.Count==0) {
		//			//This error log is explicitly for Open Dental engineers and is purposefully not translated.
		//			log+="Error: Permission not found in GetPermsFromCrudAuditPerm() for "+tableName+" \r\n";
		//			continue;
		//		}
		//		//Make a comma delimited string of the int values of each permission for this class.
		//		string permsCommaDelimStr=String.Join(",",listPermissions.Select(x => (int)x).ToList());
		//		//Get the table type name from the type.
		//		if(listCrudTableAttributes[0].TableName!="") {
		//			tableName=listCrudTableAttributes[0].TableName;
		//		}
		//		//Get primary key column name
		//		string priKeyColumnName="";
		//		foreach(FieldInfo field in type.GetFields()) {
		//			List<CrudColumnAttribute> listCrudColumnAttributes=field.GetCustomAttributes(typeof(CrudColumnAttribute),true)
		//				.Select(x => (CrudColumnAttribute)x).ToList();
		//			if(listCrudColumnAttributes.Count!=1 || !listCrudColumnAttributes[0].IsPriKey) {
		//				continue;
		//			}
		//			priKeyColumnName=field.Name;
		//			break;
		//		}
		//		if(priKeyColumnName=="") {
		//			//This error log is explicitly for Open Dental engineers and is purposefully not translated.
		//			log+="Error: Primary key attribute not found for table "+tableName+"\r\n";
		//			continue;
		//		}
		//		if(modeCur==DbmMode.Check) {
		//			numFoundOrFixed+=GetCountForSecuritylogInvalidFKeys(permsCommaDelimStr,tableName,priKeyColumnName);
		//		}
		//		else {
		//			numFoundOrFixed+=UpdateOrphanedSecuritylogInvalidKeys(permsCommaDelimStr,tableName,priKeyColumnName);
		//		}
		//	}
		//	if(modeCur==DbmMode.Check) {
		//		if(numFoundOrFixed>0 || verbose) {
		//			log+=Lans.g("FormDatabaseMaintenance","Audit trail entries with invalid FKeys found")+": "+numFoundOrFixed.ToString()+"\r\n";
		//		}
		//	}
		//	else {
		//		if(numFoundOrFixed>0 || verbose) {
		//			log+=Lans.g("FormDatabaseMaintenance","Audit trail entries with invalid FKeys fixed")+": "+numFoundOrFixed.ToString()+"\r\n";
		//		}
		//	}
		//	return log;
		//}

		[DbmMethodAttr]
		public static string SheetDepositSlips(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT SheetNum FROM sheet WHERE SheetType="+POut.Int((int)SheetTypeEnum.DepositSlip);
			DataTable table=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Deposit slip sheets")+": "+table.Rows.Count+"\r\n";
					}
					break;
				case DbmMode.Fix:
					if(table.Rows.Count>0) {
						for(int i = 0;i<table.Rows.Count;i++) {
							long sheetNum=PIn.Long(table.Rows[i]["SheetNum"].ToString());
							command="DELETE FROM sheetfield WHERE SheetNum="+POut.Long(sheetNum);
							Db.NonQ(command);
							command="DELETE FROM sheet WHERE SheetNum="+POut.Long(sheetNum);
							Db.NonQ(command);
						}
					}
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Deposit slip sheets deleted")+": "+table.Rows.Count+"\r\n";
					}
					break;
			}
			return log;
		}

		#endregion ScheduleOp, Schedule, SecurityLog, Sheet---------------------------------------------------------------------------------------------
		#region Signal, SigMessage, Statement, SummaryOfCare--------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string SignalInFuture(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command=@"SELECT COUNT(*) FROM signalod WHERE SigDateTime > "+DbHelper.Now();
					long numFound=PIn.Long(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Signalod entries with future time:")+" "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command=@"DELETE FROM signalod WHERE SigDateTime > "+DbHelper.Now();
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Signalod entries with future times deleted:")+" "+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string SigMessageInFuture(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command=@"SELECT COUNT(*) FROM sigmessage WHERE MessageDateTime > "+DbHelper.Now()+" OR AckDateTime > "+DbHelper.Now();
					long numFound=PIn.Long(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Sigmessage entries with future time:")+" "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command=@"DELETE FROM sigmessage WHERE MessageDateTime > "+DbHelper.Now()+" OR AckDateTime > "+DbHelper.Now();
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Sigmessage entries with future times deleted:")+" "+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string StatementDateRangeMax(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM statement WHERE DateRangeTo='9999-12-31'";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Statement DateRangeTo max found: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="UPDATE statement SET DateRangeTo='2200-01-01' WHERE DateRangeTo='9999-12-31'";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Statement DateRangeTo max fixed: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string SummaryOfCaresWithoutReferralsAttached(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(modeCur==DbmMode.Breakdown) {
				return "";
			}
			string log="";
			string command="SELECT * FROM refattach WHERE RefAttachNum NOT IN ("
					+"SELECT FKey FROM ehrmeasureevent WHERE EventType="+POut.Int((int)EhrMeasureEventType.SummaryOfCareProvidedToDr)+" "
					+"OR EventType="+POut.Int((int)EhrMeasureEventType.SummaryOfCareProvidedToDrElectronic)+") "
				+"AND RefType="+POut.Int((int)ReferralType.RefTo)+" "
				+"AND IsTransitionOfCare=1 ";
			//We want to fix as many measure events as we can even if they aren't good enough to count towards the actual measure. 
			//+"AND ProvNum!=0";//E.g. we will link measure events to refattaches even if the ref attach has no provider.  This way, they only have to fix the ref attach in order for their measures to show.
			List<RefAttach> refAttaches=Crud.RefAttachCrud.SelectMany(command);
			command="SELECT * FROM ehrmeasureevent "
				+"WHERE FKey NOT IN (SELECT RefAttachNum FROM refattach WHERE RefType="+POut.Int((int)ReferralType.RefTo)+" AND IsTransitionOfCare=1) "
				+"AND EventType="+POut.Int((int)EhrMeasureEventType.SummaryOfCareProvidedToDr)+" "
				+"OR EventType="+POut.Int((int)EhrMeasureEventType.SummaryOfCareProvidedToDrElectronic)+" "
				+"ORDER BY DateTEvent";
			List<EhrMeasureEvent> measureEvents=Crud.EhrMeasureEventCrud.SelectMany(command);
			int numberFixed=0;
			for(int i = 0;i<refAttaches.Count;i++) {
				for(int j = 0;j<measureEvents.Count;j++) {
					if(refAttaches[i].PatNum==measureEvents[j].PatNum
							&& measureEvents[j].FKey==0
							&& measureEvents[j].DateTEvent>=refAttaches[i].RefDate.AddDays(-3)
							&& measureEvents[j].DateTEvent<=refAttaches[i].RefDate.AddDays(1)) {
						if(modeCur!=DbmMode.Check) {
							measureEvents[j].FKey=refAttaches[i].RefAttachNum;
							EhrMeasureEvents.Update(measureEvents[j]);
						}
						measureEvents.RemoveAt(j);
						numberFixed++;
						break;
					}
				}
			}
			if(modeCur==DbmMode.Check && (numberFixed>0 || verbose)) {
				log+=Lans.g("FormDatabaseMaintenance","Summary of cares with no referrals attached")+": "+numberFixed.ToString()+"\r\n";
			}
			else if(modeCur!=DbmMode.Check && (numberFixed>0 || verbose)) {
				log+=Lans.g("FormDatabaseMaintenance","Summary of cares that had a referral attached")+": "+numberFixed.ToString()+"\r\n";
			}
			return log;
		}

		#endregion Signal, SigMessage, Statement, SummaryOfCare-----------------------------------------------------------------------------------------
		#region Task, TaskList, TimeCardRule, TreatPlan-------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string TaskListsWithCircularParentChild(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			//In order to figure out a cyclical chain of task lists we need to:
			//1. Get all TaskLists
			//2. For each TaskList whose Parent is 0...
			//a. Find all TaskLists in that "family".  We know that in order to have a cyclical relationship NONE of the TaskLists in the cycle can have Parent of 0
			//b. Remove those TaskLists from the "bad list"
			//c. When we run out of TaskLists with Parent of 0, the TaskLists left in the list are those that are part of a TaskList cycle
			//Get a list of all TaskLists
			string command="SELECT * FROM tasklist";
			List<TaskList> listAllTaskLists=Crud.TaskListCrud.SelectMany(command);
			List<TaskList> listTrunkTaskLists=listAllTaskLists.FindAll(x => x.Parent==0);//Find first TaskList with Parent of 0
			listAllTaskLists.RemoveAll(x => x.Parent==0);
			Action<long> RemoveAncestors=null;
			//Delegate method to recursively traverse the tree of a TaskList and remove all child TaskLists.
			RemoveAncestors = new Action<long>(taskListNum => {
				List<TaskList> listChildren=listAllTaskLists.FindAll(x => x.Parent==taskListNum);
				foreach(TaskList childList in listChildren) {
					RemoveAncestors.Invoke(childList.TaskListNum);
					listAllTaskLists.Remove(childList);
				}
			});
			foreach(TaskList taskListParent in listTrunkTaskLists) {
				RemoveAncestors.Invoke(taskListParent.TaskListNum);
			}
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(listAllTaskLists.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Task Lists with circular parent-child relationship")+": "+listAllTaskLists.Count.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					long taskListNum=0;
					if(listAllTaskLists.Count>0) {
						command="INSERT INTO tasklist (Descript,Parent,DateTL,IsRepeating,DateType,FromNum,ObjectType,DateTimeEntry) VALUES('FIX TASKLISTS',0,'0001-01-01',0,0,0,0,CURDATE())";
						taskListNum=Db.NonQ(command,true);
					}
					foreach(TaskList taskList in listAllTaskLists) {
						//We will set each TaskList's parent to be 0 so the user can again access them via the Main tab and put them wherever they want.
						command="UPDATE tasklist SET Parent="+POut.Long(taskListNum)+" WHERE TaskListNum="+taskList.TaskListNum.ToString();
						Db.NonQ(command);
					}
					if(listAllTaskLists.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Task Lists with circular parent-child relationship corrected")+": "+listAllTaskLists.Count.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string TasksCompletedWithInvalidFinishDateTime(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			string command="SELECT task.TaskNum,IFNULL(MAX(tasknote.DateTimeNote),task.DateTimeEntry) AS DateTimeNoteMax "
				+"FROM task "
				+"LEFT JOIN tasknote ON task.TaskNum=tasknote.TaskNum "
				+"WHERE task.TaskStatus="+POut.Int((int)TaskStatusEnum.Done)+" "
				+"AND task.DateTimeFinished="+POut.DateT(new DateTime(1,1,1))+" "
				+"GROUP BY task.TaskNum";
			DataTable table=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Tasks completed with invalid Finished Date/Time")+": "+table.Rows.Count.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					foreach(DataRow row in table.Rows) {
						//Update the DateTimeFinished with either the max note DateTime or the time of the tasks DateTimeEntry.
						//We cannot use the raw string in the DataTable because C# has auto-formatted the row into a DateTime row.
						//Therefore we have to convert the string into a DateTime object and then send it back out in the format that MySQL expects.
						DateTime dateTimeNoteMax=PIn.DateT(row["DateTimeNoteMax"].ToString());
						command="UPDATE task SET DateTimeFinished="+POut.DateT(dateTimeNoteMax)+" "
							+"WHERE TaskNum="+row["TaskNum"].ToString();
						Db.NonQ(command);
					}
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Tasks completed with invalid Finished Date/Times corrected")+": "+table.Rows.Count.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string TaskSubscriptionsInvalid(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM tasksubscription "
						+"WHERE NOT EXISTS(SELECT * FROM tasklist WHERE tasksubscription.TaskListNum=tasklist.TaskListNum)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Task subscriptions invalid: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="DELETE FROM tasksubscription "
						+"WHERE NOT EXISTS(SELECT * FROM tasklist WHERE tasksubscription.TaskListNum=tasklist.TaskListNum)";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Task subscriptions deleted: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string TaskUnreadsWithoutTasksAttached(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM taskunread "
						+"WHERE taskunread.TaskNum NOT IN(SELECT TaskNum FROM task)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Unread task notifications for deleted tasks")+": "+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="DELETE FROM taskunread "
						+"WHERE taskunread.TaskNum NOT IN(SELECT TaskNum FROM task)";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Unread task notifications for deleted tasks removed")+": "+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string TimeCardRuleEmployeeNumInvalid(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM timecardrule "
						+"WHERE timecardrule.EmployeeNum!=0 " //0 is all employees, so it is a 'valid' employee number
						+"AND timecardrule.EmployeeNum NOT IN(SELECT employee.EmployeeNum FROM employee)";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Timecard rules found with invalid employee number: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="UPDATE timecardrule "
						+"SET timecardrule.EmployeeNum=0 "
						+"WHERE timecardrule.EmployeeNum!=0 " //don't set to 0 if already 0
						+"AND timecardrule.EmployeeNum NOT IN(SELECT employee.EmployeeNum FROM employee)";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Timecard rules applied to All Employees due to invalid employee number: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>No need to pass in userNum, it's set before remoting role check and passed to the server if necessary.</summary>
		[DbmMethodAttr(HasUserNum = true)]
		public static string TreatPlansInvalid(bool verbose,DbmMode modeCur,long userNum = 0) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				userNum=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur,userNum);
			}
			string command="SELECT treatplan.PatNum FROM procedurelog	"//procs for 1 pat attached to a treatplan for another
					+"INNER JOIN treatplanattach ON treatplanattach.ProcNum=procedurelog.ProcNum "
					+"INNER JOIN treatplan ON treatplan.TreatPlanNum=treatplanattach.TreatPlanNum AND procedurelog.PatNum!=treatplan.PatNum "
					+"UNION "//more than 1 active treatment plan
					+"SELECT PatNum FROM treatplan WHERE TPStatus=1 GROUP BY PatNum HAVING COUNT(DISTINCT TreatPlanNum)>1";
			List<long> listPatNumsForAudit=Db.GetListLong(command).Distinct().ToList();
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(listPatNumsForAudit.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Patients found with one or more invalid treatment plans")+": "+listPatNumsForAudit.Count+"\r\n";
					}
					break;
				case DbmMode.Fix:
					listPatNumsForAudit.ForEach(x => TreatPlans.AuditPlans(x,(Patients.GetPat(x).DiscountPlanNum==0 ? TreatPlanType.Insurance : TreatPlanType.Discount),userNum));
					TreatPlanAttaches.DeleteOrphaned();
					if(listPatNumsForAudit.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Patients with one or more invalid treatment plans fixed")+": "+listPatNumsForAudit.Count+"\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>Finds treatplanattaches with the same treatplannum and procnum.</summary>
		[DbmMethodAttr]
		public static string TreatPlanAttachDuplicateProc(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT treatplanattach.TreatPlanNum, MIN(treatplanattach.TreatPlanAttachNum) AS OriginalTPANum, "
			+ " ProcNum, "
			+ " COUNT(ProcNum) NumDupes "
			+ " FROM treatplanattach "
			+ " GROUP BY treatplanattach.treatplannum, treatplanattach.ProcNum "
			+ " HAVING NumDupes > 1 ";
			DataTable table=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","TreatPlanAttaches with duplicate ProcNums and TreatPlanNums found")+": "+table.Rows.Count.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					for(int i = 0;i < table.Rows.Count;i++) {
						command="DELETE FROM treatplanattach WHERE treatplanattach.TreatPlanNum="+table.Rows[i]["TreatPlanNum"]
							+" AND treatplanattach.ProcNum="+table.Rows[i]["ProcNum"]
							+" AND treatplanattach.TreatPlanAttachNum != "+table.Rows[i]["OriginalTPANum"];
						Db.NonQ(command);
					}
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","TreatPlanAttaches with duplicate ProcNums and TreatPlanNums deleted")+": "+table.Rows.Count.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		///<summary>Finds proctps that have been orphaned and creates dummy treatment plans for DateTime.MinValue so that the orphaned proctps can be viewed.</summary>
		[DbmMethodAttr]
		public static string TreatPlanOrphanedProcTps(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command=@"SELECT proctp.PatNum,proctp.TreatPlanNum 
				FROM proctp
				LEFT JOIN treatplan ON treatplan.TreatPlanNum = proctp.TreatPlanNum 
				WHERE treatplan.TreatPlanNum IS NULL 
				GROUP BY proctp.TreatPlanNum";
			DataTable table=Db.GetTable(command);
			string log = "";
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Treatment Plans with orphaned proctps")+": "+table.Rows.Count.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					for(int i = 0;i < table.Rows.Count;i++) {
						TreatPlan tp = new TreatPlan() {
							DateTP = DateTime.MinValue,
							Heading = "MISSING TREATMENT PLAN",
							Note = "This treatment plan was created by Database Maintenence because of orphaned proctps.",
							PatNum = PIn.Long(table.Rows[i]["PatNum"].ToString()),
							SecUserNumEntry = Security.CurUser.UserNum,
							TreatPlanNum = PIn.Long(table.Rows[i]["TreatPlanNum"].ToString()),
							TPStatus = TreatPlanStatus.Saved
						};
						Crud.TreatPlanCrud.Insert(tp,true);
					}
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Treatment Plans with orphaned proctps fixed")+": "+table.Rows.Count.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		#endregion Task, TaskList, TimeCardRule, TreatPlan----------------------------------------------------------------------------------------------
		#region UnscheduledAppt, Userod-----------------------------------------------------------------------------------------------------------------

		[DbmMethodAttr]
		public static string UnscheduledApptsWithInvalidOpNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string command="SELECT aptNum FROM appointment WHERE Op != 0 AND AptStatus=3";//UnschedList
			DataTable table=Db.GetTable(command);
			string log="";
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Unscheduled appointments with invalid Op nums")+": "+table.Rows.Count.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="UPDATE appointment SET Op=0 WHERE AptStatus=3";//UnschedList
					Db.NonQ(command);
					if(table.Rows.Count>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Unscheduled appointments with invalid Op nums corrected")+": "+table.Rows.Count.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		/// <summary>Only one user of a given UserName may be unhidden at a time. Warn the user and instruct them to hide extras.</summary>
		[DbmMethodAttr(HasBreakDown = true)]
		public static string UserodDuplicateUser(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return Lans.g("FormDatabaseMaintenance","Currently not Oracle compatible.  Please call support.");
			}
			string command="SELECT UserName FROM userod WHERE IsHidden=0 GROUP BY UserName HAVING Count(*)>1;";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0 && !verbose) {
				return "";
			}
			string log=Lans.g("FormDatabaseMaintenance","Users with duplicates")+": "+table.Rows.Count;
			switch(modeCur) {
				case DbmMode.Check:
					if(table.Rows.Count!=0) {//Only the fix should show the entire list of items.
						log+="\r\n   "+Lans.g("FormDatabaseMaintenance","Manual fix needed.  Double click to see a break down.")+"\r\n";
					}
					break;
				case DbmMode.Fix:
				case DbmMode.Breakdown:
					if(table.Rows.Count>0) {//Running the fix and there are items to show.
						log+=", "+Lans.g("FormDatabaseMaintenance","including")+":\r\n";
						for(int i = 0;i<table.Rows.Count;i++) {
							log+=Lans.g("FormDatabaseMaintenance","User")+" - "+table.Rows[i]["UserName"].ToString()+"\r\n";
						}
						log+=Lans.g("FormDatabaseMaintenance","   They need to be fixed manually.  Please go to Setup | Security and hide all but one of each unique user.")+"\r\n";
					}
					break;
			}
			return log;
		}

		[DbmMethodAttr]
		public static string UserodInvalidClinicNum(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT Count(*) FROM userod WHERE ClinicNum<>0 AND ClinicNum NOT IN (SELECT ClinicNum FROM clinic)";
					long numFound=PIn.Long(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Users found with invalid ClinicNum: ")+numFound+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="UPDATE userod SET ClinicNum=0 WHERE ClinicNum<>0 AND ClinicNum NOT IN (SELECT ClinicNum FROM clinic)";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Users fixed with invalid ClinicNum: ")+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		///// <summary>Deprecated as of 17.4 since we began using the usergroupattach table. userod has an invalid FK to usergroup</summary>
		//[DbmMethodAttr]
		//public static string UserodInvalidUserGroupNum(bool verbose,DbmMode modeCur) {
		//	if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
		//		return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
		//	}
		//	string log="";
		//	switch(modeCur) {
		//		case DbmMode.Check:
		//			command="SELECT Count(*) FROM userod WHERE UserGroupNum NOT IN (SELECT UserGroupNum FROM usergroup) ";
		//			long numFound=PIn.Long(Db.GetCount(command));
		//			if(numFound>0 || verbose) {
		//				log+=Lans.g("FormDatabaseMaintenance","Users found with invalid UserGroupNum: ")+numFound+"\r\n";
		//			}
		//			break;
		//		case DbmMode.Fix:
		//			command="SELECT * FROM userod WHERE UserGroupNum NOT IN (SELECT UserGroupNum FROM usergroup) ";
		//			table=Db.GetTable(command);
		//			long userNum;
		//			string userName;
		//			long userGroupNum;
		//			long numberFixed=0;
		//			for(int i=0;i<table.Rows.Count;i++) {//Create a usergroup with the same name as the userod+"Group"
		//				userNum=PIn.Long(table.Rows[i]["UserNum"].ToString());
		//				userName=PIn.String(table.Rows[i]["UserName"].ToString());
		//				command="INSERT INTO usergroup (Description) VALUES('"+POut.String(userName+" Group")+"')";
		//				userGroupNum=Db.NonQ(command,true);
		//				command="UPDATE userod SET UserGroupNum="+POut.Long(userGroupNum)+" WHERE UserNum="+POut.Long(userNum);
		//				Db.NonQ(command);
		//				numberFixed++;
		//			}
		//			if(numberFixed>0 || verbose) {
		//				log+=Lans.g("FormDatabaseMaintenance","Users fixed with invalid UserGroupNum: ")+numberFixed.ToString()+"\r\n";
		//			}
		//			break;
		//	}
		//	return log;
		//}

		/// <summary>userod is restricted to ClinicNum 0 - All.  Restricted to All clinics doesn't make sense.  This will set the ClinicIsRestricted bool to false if ClinicNum=0.</summary>
		[DbmMethodAttr]
		public static string UserodInvalidRestrictedClinic(bool verbose,DbmMode modeCur) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),verbose,modeCur);
			}
			string log="";
			string command;
			switch(modeCur) {
				case DbmMode.Check:
					command="SELECT COUNT(*) FROM userod WHERE ClinicNum=0 AND ClinicIsRestricted=1";
					int numFound=PIn.Int(Db.GetCount(command));
					if(numFound>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Users found restricted to an invalid clinic")+": "+numFound.ToString()+"\r\n";
					}
					break;
				case DbmMode.Fix:
					command="UPDATE userod SET ClinicIsRestricted=0 WHERE ClinicNum=0 AND ClinicIsRestricted=1";
					long numberFixed=Db.NonQ(command);
					if(numberFixed>0 || verbose) {
						log+=Lans.g("FormDatabaseMaintenance","Users fixed with restriction to an invalid clinic")+": "+numberFixed.ToString()+"\r\n";
					}
					break;
			}
			return log;
		}

		#endregion UnscheduledAppt, Userod--------------------------------------------------------------------------------------------------------------

		#endregion Methods That Apply to Specific Tables----------------------------------------------------------------------------------------------------
		#region Tool Button and Helper Methods--------------------------------------------------------------------------------------------------------------

		public static List<string> GetDatabaseNames() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<string>>(MethodBase.GetCurrentMethod());
			}
			List<string> retVal=new List<string>();
			string command="SHOW DATABASES";
			//if this next step fails, table will simply have 0 rows
			DataTable table=Db.GetTable(command);
			for(int i = 0;i<table.Rows.Count;i++) {
				retVal.Add(table.Rows[i][0].ToString());
			}
			return retVal;
		}

		///<summary>Will return empty string if no problems.</summary>
		public static string GetDuplicateClaimProcs() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod());
			}
			string retVal="";
			string command=@"SELECT LName,FName,patient.PatNum,ClaimNum,FeeBilled,Status,ProcNum,ProcDate,ClaimProcNum,InsPayAmt,LineNumber, COUNT(*) cnt
FROM claimproc
LEFT JOIN patient ON patient.PatNum=claimproc.PatNum
WHERE ClaimNum > 0
AND ProcNum>0
AND Status!=4/*exclude supplemental*/
GROUP BY LName,FName,patient.PatNum,ClaimNum,FeeBilled,Status,ProcNum,ProcDate,ClaimProcNum,InsPayAmt,LineNumber 
HAVING cnt>1";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0) {
				return "";
			}
			retVal+="Duplicate claim payments found:\r\n";
			DateTime date;
			for(int i = 0;i<table.Rows.Count;i++) {
				if(i>0) {//check for duplicate rows.  We only want to report each claim once.
					if(table.Rows[i]["ClaimNum"].ToString()==table.Rows[i-1]["ClaimNum"].ToString()) {
						continue;
					}
				}
				date=PIn.Date(table.Rows[i]["ProcDate"].ToString());
				retVal+=table.Rows[i]["LName"].ToString()+", "
					+table.Rows[i]["FName"].ToString()+" "
					+"("+table.Rows[i]["PatNum"].ToString()+"), "
					+date.ToShortDateString()+"\r\n";
			}
			retVal+="\r\n";
			return retVal;
		}

		///<summary>Will return empty string if no problems.</summary>
		public static string GetDuplicateSupplementalPayments() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod());
			}
			string retVal="";
			string command=@"SELECT LName,FName,patient.PatNum,ClaimNum,FeeBilled,Status,ProcNum,ProcDate,ClaimProcNum,InsPayAmt,LineNumber, COUNT(*) cnt
FROM claimproc
LEFT JOIN patient ON patient.PatNum=claimproc.PatNum
WHERE ClaimNum > 0
AND ProcNum>0
AND Status=4/*only supplemental*/
GROUP BY LName,FName,patient.PatNum,ClaimNum,FeeBilled,Status,ProcNum,ProcDate,ClaimProcNum,InsPayAmt,LineNumber
HAVING cnt>1";
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0) {
				return "";
			}
			retVal+="Duplicate supplemental payments found (may be false positives):\r\n";
			DateTime date;
			for(int i = 0;i<table.Rows.Count;i++) {
				if(i>0) {
					if(table.Rows[i]["ClaimNum"].ToString()==table.Rows[i-1]["ClaimNum"].ToString()) {
						continue;
					}
				}
				date=PIn.Date(table.Rows[i]["ProcDate"].ToString());
				retVal+=table.Rows[i]["LName"].ToString()+", "
					+table.Rows[i]["FName"].ToString()+" "
					+"("+table.Rows[i]["PatNum"].ToString()+"), "
					+date.ToShortDateString()+"\r\n";
			}
			retVal+="\r\n";
			return retVal;
		}

		///<summary></summary>
		public static string GetMissingClaimProcs(string olddb) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),olddb);
			}
			string retVal="";
			string command="SELECT LName,FName,patient.PatNum,ClaimNum,FeeBilled,Status,ProcNum,ProcDate,ClaimProcNum,InsPayAmt,LineNumber "
				+"FROM "+olddb+".claimproc "
				+"LEFT JOIN "+olddb+".patient ON "+olddb+".patient.PatNum="+olddb+".claimproc.PatNum "
				+"WHERE NOT EXISTS(SELECT * FROM claimproc WHERE claimproc.ClaimProcNum="+olddb+".claimproc.ClaimProcNum) "
				+"AND ClaimNum > 0 AND ProcNum>0";
			DataTable table=Db.GetTable(command);
			double insPayAmt;
			double feeBilled;
			int count=0;
			for(int i = 0;i<table.Rows.Count;i++) {
				insPayAmt=PIn.Double(table.Rows[i]["InsPayAmt"].ToString());
				feeBilled=PIn.Double(table.Rows[i]["FeeBilled"].ToString());
				command="SELECT COUNT(*) FROM "+olddb+".claimproc "
					+"WHERE ClaimNum= "+table.Rows[i]["ClaimNum"].ToString()+" "
					+"AND ProcNum= "+table.Rows[i]["ProcNum"].ToString()+" "
					+"AND Status= "+table.Rows[i]["Status"].ToString()+" "
					+"AND InsPayAmt= '"+POut.Double(insPayAmt)+"' "
					+"AND FeeBilled= '"+POut.Double(feeBilled)+"' "
					+"AND LineNumber= "+table.Rows[i]["LineNumber"].ToString();
				string result=Db.GetCount(command);
				if(result!="1") {//only include in result if there are duplicates in old db.
					count++;
				}
			}
			command="SELECT ClaimPaymentNum "
				+"FROM "+olddb+".claimpayment "
				+"WHERE NOT EXISTS(SELECT * FROM claimpayment WHERE claimpayment.ClaimPaymentNum="+olddb+".claimpayment.ClaimPaymentNum) ";
			DataTable table2=Db.GetTable(command);
			if(count==0 && table2.Rows.Count==0) {
				return "";
			}
			retVal+="Missing claim payments found: "+count.ToString()+"\r\n";
			retVal+="Missing claim checks found (probably false positives): "+table2.Rows.Count.ToString()+"\r\n";
			return retVal;
		}

		//public static bool DatabaseIsOlderThanMarchSeventeenth(string olddb) {
		//  if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
		//    return Meth.GetBool(MethodBase.GetCurrentMethod(),olddb);
		//  }
		//  command="SELECT COUNT(*) FROM "+olddb+".claimproc WHERE DateEntry > '2010-03-16'";
		//  if(Db.GetCount(command)=="0") {
		//    return true;
		//  }
		//  return false;
		//}

		/// <summary></summary>
		public static string FixClaimProcDeleteDuplicates() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod());
			}
			string log="";
			//command=@"SELECT LName,FName,patient.PatNum,ClaimNum,FeeBilled,Status,ProcNum,ProcDate,ClaimProcNum,InsPayAmt,LineNumber, COUNT(*) cnt
			//	FROM claimproc
			//	LEFT JOIN patient ON patient.PatNum=claimproc.PatNum
			//	WHERE ClaimNum > 0
			//	AND ProcNum>0
			//	AND Status!=4/*exclude supplemental*/
			//	GROUP BY ClaimNum,ProcNum,Status,InsPayAmt,FeeBilled,LineNumber
			//	HAVING cnt>1";
			//table=Db.GetTable(command);
			//long numberFixed=0;
			//double insPayAmt;
			//double feeBilled;
			//for(int i=0;i<table.Rows.Count;i++) {
			//  insPayAmt=PIn.Double(table.Rows[i]["InsPayAmt"].ToString());
			//  feeBilled=PIn.Double(table.Rows[i]["FeeBilled"].ToString());
			//  command="DELETE FROM claimproc "
			//    +"WHERE ClaimNum= "+table.Rows[i]["ClaimNum"].ToString()+" "
			//    +"AND ProcNum= "+table.Rows[i]["ProcNum"].ToString()+" "
			//    +"AND Status= "+table.Rows[i]["Status"].ToString()+" "
			//    +"AND InsPayAmt= '"+POut.Double(insPayAmt)+"' "
			//    +"AND FeeBilled= '"+POut.Double(feeBilled)+"' "
			//    +"AND LineNumber= "+table.Rows[i]["LineNumber"].ToString()+" "
			//    +"AND ClaimProcNum != "+table.Rows[i]["ClaimProcNum"].ToString();
			//  numberFixed+=Db.NonQ(command);
			//}
			//log+="Claimprocs deleted due duplicate entries: "+numberFixed.ToString()+".\r\n";
			return log;
		}

		/// <summary></summary>
		public static string FixMissingClaimProcs(string olddb) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),olddb);
			}
			string log="";
			//command="SELECT LName,FName,patient.PatNum,ClaimNum,FeeBilled,Status,ProcNum,ProcDate,ClaimProcNum,InsPayAmt,LineNumber "
			//  +"FROM "+olddb+".claimproc "
			//  +"LEFT JOIN "+olddb+".patient ON "+olddb+".patient.PatNum="+olddb+".claimproc.PatNum "
			//  +"WHERE NOT EXISTS(SELECT * FROM claimproc WHERE claimproc.ClaimProcNum="+olddb+".claimproc.ClaimProcNum) "
			//  +"AND ClaimNum > 0 AND ProcNum>0";
			//table=Db.GetTable(command);
			//long numberFixed=0;
			//command="SELECT ValueString FROM "+olddb+".preference WHERE PrefName='DataBaseVersion'";
			//string oldVersString=Db.GetScalar(command);
			//Version oldVersion=new Version(oldVersString);
			//if(oldVersion < new Version("6.7.1.0")) {
			//  return "Version of old database is too old to use with the automated tool: "+oldVersString;
			//}
			//double insPayAmt;
			//double feeBilled;
			//for(int i=0;i<table.Rows.Count;i++) {
			//  insPayAmt=PIn.Double(table.Rows[i]["InsPayAmt"].ToString());
			//  feeBilled=PIn.Double(table.Rows[i]["FeeBilled"].ToString());
			//  command="SELECT COUNT(*) FROM "+olddb+".claimproc "
			//    +"WHERE ClaimNum= "+table.Rows[i]["ClaimNum"].ToString()+" "
			//    +"AND ProcNum= "+table.Rows[i]["ProcNum"].ToString()+" "
			//    +"AND Status= "+table.Rows[i]["Status"].ToString()+" "
			//    +"AND InsPayAmt= '"+POut.Double(insPayAmt)+"' "
			//    +"AND FeeBilled= '"+POut.Double(feeBilled)+"' "
			//    +"AND LineNumber= "+table.Rows[i]["LineNumber"].ToString();
			//  string result=Db.GetCount(command);
			//  if(result=="1") {//only include in result if there are duplicates in old db.
			//    continue;
			//  }
			//  command="INSERT INTO claimproc SELECT *";
			//  if(oldVersion < new Version("6.8.1.0")) {
			//    command+=",-1,-1,0";
			//  }
			//  else if(oldVersion < new Version("6.9.1.0")) {
			//    command+=",0";
			//  }
			//  command+=" FROM "+olddb+".claimproc "
			//    +"WHERE "+olddb+".claimproc.ClaimProcNum="+table.Rows[i]["ClaimProcNum"].ToString();
			//  numberFixed+=Db.NonQ(command);
			//}
			//command="SELECT ClaimPaymentNum "
			//  +"FROM "+olddb+".claimpayment "
			//  +"WHERE NOT EXISTS(SELECT * FROM claimpayment WHERE claimpayment.ClaimPaymentNum="+olddb+".claimpayment.ClaimPaymentNum) ";
			//table=Db.GetTable(command);
			//long numberFixed2=0;
			//for(int i=0;i<table.Rows.Count;i++) {
			//  command="INSERT INTO claimpayment SELECT * FROM "+olddb+".claimpayment "
			//    +"WHERE "+olddb+".claimpayment.ClaimPaymentNum="+table.Rows[i]["ClaimPaymentNum"].ToString();
			//  numberFixed2+=Db.NonQ(command);
			//}
			//log+="Missing claimprocs added back: "+numberFixed.ToString()+".\r\n";
			//log+="Missing claimpayments added back: "+numberFixed2.ToString()+".\r\n";
			return log;
		}

		///<summary>Removes unsupported unicode characters from appointment.ProcDescript, appointment.Note, and patient.AddrNote.
		///Also removes mysql null character ("\0" or CHAR(0)) from several columns from several tables.
		///These null characters were causing the middle tier deserialization to fail as they are not UTF-16 supported characters.
		///They are, however, allowed in UTF-8.</summary>
		public static void FixSpecialCharacters() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod());
				return;
			}
			string command="SELECT * FROM appointment WHERE (ProcDescript REGEXP '[^[:alnum:]^[:space:]^[:punct:]]+') OR (Note REGEXP '[^[:alnum:]^[:space:]^[:punct:]]+')";
			List<Appointment> apts=OpenDentBusiness.Crud.AppointmentCrud.SelectMany(command);
			List<char> specialCharsFound=new List<char>();
			int specialCharCount=0;
			int intC=0;
			if(apts.Count!=0) {
				foreach(Appointment apt in apts) {
					foreach(char c in apt.Note) {
						intC=(int)c;
						if((intC<126 && intC>31)//31 - 126 are all safe.
							|| intC==9     //"Horizontal Tabulation"
							|| intC==10    //Line Feed
							|| intC==13) { //carriage return
							continue;
						}
						specialCharCount++;
						if(specialCharsFound.Contains(c)) {
							continue;
						}
						specialCharsFound.Add(c);
					}
					foreach(char c in apt.ProcDescript) {//search every character in ProcDescript
						intC=(int)c;
						if((intC<126 && intC>31)//31 - 126 are all safe.
							|| intC==9     //"Horizontal Tabulation"
							|| intC==10    //Line Feed
							|| intC==13) { //carriage return
							continue;
						}
						specialCharCount++;
						if(specialCharsFound.Contains(c)) {
							continue;
						}
						specialCharsFound.Add(c);
					}
				}
				foreach(char c in specialCharsFound) {
					command="UPDATE appointment SET Note=REPLACE(Note,'"+POut.String(c.ToString())+"',''), ProcDescript=REPLACE(ProcDescript,'"+POut.String(c.ToString())+"','')";
					Db.NonQ(command);
				}
			}
			command="SELECT * FROM patient WHERE AddrNote REGEXP '[^[:alnum:]^[:space:]]+'";
			List<Patient> pats=OpenDentBusiness.Crud.PatientCrud.SelectMany(command);
			specialCharsFound=new List<char>();
			specialCharCount=0;
			intC=0;
			if(pats.Count>0) {
				foreach(Patient pat in pats) {
					foreach(char c in pat.AddrNote) {
						intC=(int)c;
						if((intC<126 && intC>31)//31 - 126 are all safe.
							|| intC==9      //"Horizontal Tabulation"
							|| intC==10     //Line Feed
							|| intC==13) {  //carriage return
							continue;
						}
						specialCharCount++;
						if(specialCharsFound.Contains(c)) {
							continue;
						}
						specialCharsFound.Add(c);
					}
				}
				foreach(char c in specialCharsFound) {
					command="UPDATE patient SET AddrNote=REPLACE(AddrNote,'"+POut.String(c.ToString())+"','')";
					Db.NonQ(command);
				}
			}
			for(int i = 0;i<_listTableAndColumns.Count;i+=2) {
				string tableName=_listTableAndColumns[i];
				string columnName=_listTableAndColumns[i+1];
				command="UPDATE "+tableName+" "
					+"SET "+columnName+"=REPLACE("+columnName+",CHAR(0),'') "
					+"WHERE "+columnName+" LIKE '%"+POut.String("\0")+"%'";
				Db.NonQ(command);
			}
			return;
		}

		///<summary>Replaces null strings with empty strings and returns the number of rows changed.</summary>
		public static long MySqlRemoveNullStrings() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetLong(MethodBase.GetCurrentMethod());
			}
			string command=@"SELECT table_name,column_name 
				FROM information_schema.columns 
				WHERE table_schema=(SELECT DATABASE()) 
				AND (data_type='char' 
					OR data_type='longtext' 
					OR data_type='mediumtext' 
					OR data_type='text' 
					OR data_type='varchar') 
				AND is_nullable='YES'";
			DataTable table=Db.GetTable(command);
			long changeCount=0;
			for(int i = 0;i<table.Rows.Count;i++) {
				command="UPDATE `"+table.Rows[i]["table_name"].ToString()+"` "
					+"SET `"+table.Rows[i]["column_name"].ToString()
					+"`='' WHERE `"+table.Rows[i]["column_name"].ToString()+"` IS NULL";
				changeCount+=Db.NonQ(command);
			}
			return changeCount;
		}

		///<summary>Makes a backup of the database, clears out etransmessagetext entries over a year old, and then runs optimize on just the etransmessagetext table.  Customers were calling in with the complaint that their etransmessagetext table is too big so we added this tool.</summary>
		public static void ClearOldEtransMessageText() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod());
				return;
			}
			//Make a backup of DB before we change anything, especially because we will be running optimize at the end.
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				return; //Several issues need to be addressed before supporting Oracle.  E.g. backing up, creating temporary tables with globally unique identifiers, etc.
			}
			//Unlink etrans records from their etransmessagetext records if older than 1 year.
			//We want to keep the 835's around, because they are financial documents which the user may want to reference from the claim edit window later.
			string command="UPDATE etrans "
				+"SET EtransMessageTextNum=0 "
				+"WHERE DATE(DateTimeTrans)<ADDDATE(CURDATE(),INTERVAL -1 YEAR) AND Etype!="+POut.Long((int)EtransType.ERA_835);
			Db.NonQ(command);
			//Create a temporary table to hold all of the EtransMessageTextNum foreign keys which are sill in use within etrans.  The temporary table speeds up the next query.
			string tableName="tempetransnomessage"+MiscUtils.CreateRandomAlphaNumericString(8);//max size for a table name in oracle is 30 chars.
			command="DROP TABLE IF EXISTS "+tableName+"; "
				+"CREATE TABLE "+tableName+" "
				+"SELECT DISTINCT EtransMessageTextNum FROM etrans WHERE EtransMessageTextNum!=0; "
				+"ALTER TABLE "+tableName+" ADD INDEX (EtransMessageTextNum);";
			Db.NonQ(command);
			//Delete unlinked etransmessagetext entries.  Remember, multiple etrans records might point to a single etransmessagetext record.  Therefore, we must keep a particular etransmessagetext record if at least one etrans record needs it.
			command="DELETE FROM etransmessagetext "
				+"WHERE EtransMessageTextNum NOT IN (SELECT EtransMessageTextNum FROM "+tableName+");";
			Db.NonQ(command);
			//Remove the temporary table which is no longer needed.
			command="DROP TABLE "+tableName+";";
			Db.NonQ(command);
			//To reclaim that space on the disk you have to do an Optimize.
			OptimizeTable("etransmessagetext");
		}

		///<summary>Return values look like 'MyISAM' or 'InnoDB'. Will return empty string on error.</summary>
		public static string GetStorageEngineDefaultName() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod());
			}
			string retVal="";
			try {
				retVal=Db.GetScalar("SELECT @@default_storage_engine");//Mysql 5.5.3+
			}
			catch {
				//using SHOW GLOBAL VARIABLES will return an empty string if not supported.
				DataTable dtEngine;
				dtEngine=Db.GetTable("SHOW GLOBAL VARIABLES LIKE 'storage_engine'");//MySQL 5.5.2-
				if(dtEngine.Rows.Count>0) {
					retVal=PIn.String(dtEngine.Rows[0]["Value"].ToString());
				}
			}
			return retVal;
		}

		///<summary>Gets the number of tables in MyISAM format.</summary>
		public static int GetMyisamTableCount() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetInt(MethodBase.GetCurrentMethod());
			}
			string command="SELECT TABLE_NAME FROM INFORMATION_SCHEMA.tables "
				+"WHERE TABLE_SCHEMA='"+POut.String(DataConnection.GetDatabaseName())+"' "
				+"AND ENGINE LIKE 'MyISAM'";
			return Db.GetTable(command).Rows.Count;
		}

		///<summary>Returns true if the conversion was successfull or no conversion was necessary. The goal is to convert InnoDB tables (excluding the 'phone' table) to MyISAM format when there are a mixture of InnoDB and MyISAM tables but no conversion will be performed when all of the tables are already in the same format.</summary>
		public static bool ConvertTablesToMyisam() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod());
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) { //Does not apply to Oracle.
				return true;
			}
			string command="SELECT TABLE_NAME,ENGINE FROM INFORMATION_SCHEMA.tables "
				+"WHERE TABLE_SCHEMA='"+POut.String(DataConnection.GetDatabaseName())+"' "
				+"AND TABLE_NAME!='phone'";//this table is used internally at OD HQ, and is always innodb.
			DataTable dtTableTypes=Db.GetTable(command);
			int numInnodb=0;//Or possibly some other format.
			int numMyisam=0;
			for(int i = 0;i<dtTableTypes.Rows.Count;i++) {
				if(PIn.String(dtTableTypes.Rows[i]["ENGINE"].ToString()).ToUpper()=="MYISAM") {
					numMyisam++;
				}
				else {
					numInnodb++;
				}
			}
			if(numInnodb>0 && numMyisam>0) {//Fix tables by converting them to MyISAM when there is a mixture of different table types.
				for(int i = 0;i<dtTableTypes.Rows.Count;i++) {
					if(PIn.String(dtTableTypes.Rows[i]["ENGINE"].ToString()).ToUpper()=="MYISAM") {
						continue;
					}
					string tableName=PIn.String(dtTableTypes.Rows[i]["TABLE_NAME"].ToString());
					command="ALTER TABLE `"+tableName+"` ENGINE='MyISAM'";
					try {
						Db.NonQ(command);
					}
					catch {
						return false;
					}
				}
				command="SELECT TABLE_NAME FROM INFORMATION_SCHEMA.tables "
					+"WHERE TABLE_SCHEMA='"+POut.String(DataConnection.GetDatabaseName())+"' "
					+"AND TABLE_NAME!='phone' "
					+"AND ENGINE NOT LIKE 'MyISAM'";
				if(Db.GetTable(command).Rows.Count!=0) { //If any tables are still InnoDB.
					return false;
				}
			}
			return true;
		}

		///<summary>Returns the number of invalid FKey entries for specified tableName, permissions, and primary key column.
		///You MUST check remoting role before calling this method.  It is purposefully private and must remain so.</summary>
		private static long GetCountForSecuritylogInvalidFKeys(string permsCommaDelimStr,string tableName,string priKeyColumnName) {
			//No remoting role check; This is a private static method and those cannot be called from the middle tier.
			string command="SELECT COUNT(securitylog.SecurityLogNum) "
					+"FROM securitylog "
					+"WHERE securitylog.PermType IN ("+POut.String(permsCommaDelimStr)+") "
					+"AND securitylog.FKey!=0 "
					+"AND NOT EXISTS ( "
						+"SELECT "+tableName+"."+priKeyColumnName+" "
						+"FROM "+tableName+" "
						+"WHERE "+tableName+"."+priKeyColumnName+"=securitylog.FKey "
					+")";
			return PIn.Long(Db.GetCount(command));
		}

		///<summary>Fixes orphaned FKey entries for specific tableName, permissions, and primary key column.
		///Returns number of rows fixed.
		///You MUST check remoting role before calling this method.  It is purposefully private and must remain so.</summary>
		private static long UpdateOrphanedSecuritylogInvalidKeys(string permsCommaDelimStr,string tableName,string priKeyColumnName) {
			string command="UPDATE securitylog SET FKey=0 "
					+"WHERE securitylog.PermType IN ("+POut.String(permsCommaDelimStr)+") "
					+"AND securitylog.FKey!=0 "
					+"AND NOT EXISTS ( "
						+"SELECT "+tableName+"."+priKeyColumnName+" "
						+"FROM "+tableName+" "
						+"WHERE "+tableName+"."+priKeyColumnName+"=securitylog.FKey "
					+")";
			return Db.NonQ(command);
		}

		///<summary>Used to estimate the time that CreateMissingActiveTPs will take to run.</summary>
		public static List<Procedure> GetProcsNoActiveTp() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Procedure>>(MethodBase.GetCurrentMethod());
			}
			//pats with TP'd procs and no active treatplan OR pats with TPi'd procs that are attached to a sched or planned appt and no active treatplan
			string command="SELECT * FROM procedurelog WHERE (ProcStatus="+(int)ProcStat.TP+" "//TP proc exists
				+"OR (ProcStatus="+(int)ProcStat.TPi+" AND (AptNum>0 OR PlannedAptNum>0))) "//TPi proc exists that is attached to a sched or planned appt
				+"AND PatNum NOT IN(SELECT PatNum FROM treatplan WHERE TPStatus="+(int)TreatPlanStatus.Active+")";//no active treatplan
			return Crud.ProcedureCrud.SelectMany(command);
		}

		///<summary>No need to pass in userNum, it's set before remoting role check and passed to the server if necessary.</summary>
		public static string CreateMissingActiveTPs(List<Procedure> listTpTpiProcs,long userNum = 0) {
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb) {
				userNum=Security.CurUser.UserNum;//must be before normal remoting role check to get user at workstation
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod(),listTpTpiProcs,userNum);
			}
			if(listTpTpiProcs.Count==0) {//should never happen, won't get called if the list is empty, but just in case
				return "";
			}
			listTpTpiProcs=listTpTpiProcs.OrderBy(x => x.PatNum).ToList();//code below relies of patients being grouped.
																																		//listTpTpiProcs.Sort((x,y) => { return x.PatNum.CompareTo(y.PatNum); });//possibly more efficient
			TreatPlan activePlan=null;
			long patNumCur=0;
			//listProcsNoTp is ordered by PatNum, so each time we find a new PatNum we will create a new active plan and attach procs to it
			//until we find the next PatNum
			foreach(Procedure procCur in listTpTpiProcs) {
				if(procCur.PatNum!=patNumCur) {//new patient, create active plan
					activePlan=new TreatPlan {//create active plan, all patients in listPatNumsNoTp do not have an active plan
						Heading=Lans.g("TreatPlans","Active Treatment Plan"),
						Note=PrefC.GetString(PrefName.TreatmentPlanNote),
						TPStatus=TreatPlanStatus.Active,
						PatNum=procCur.PatNum,
						//UserNumPresenter=userNum,
						SecUserNumEntry=userNum,
						TPType=(Patients.GetPat(procCur.PatNum).DiscountPlanNum==0 ? TreatPlanType.Insurance : TreatPlanType.Discount)
					};
					activePlan.TreatPlanNum=TreatPlans.Insert(activePlan);
					patNumCur=procCur.PatNum;
				}
				TreatPlanAttaches.Insert(new TreatPlanAttach { ProcNum=procCur.ProcNum,TreatPlanNum=activePlan.TreatPlanNum,Priority=procCur.Priority });
			}
			return "Patients with active treatment plans created: "+listTpTpiProcs.Select(x => x.PatNum).Distinct().ToList().Count;
		}

		///<summary>This method is designed to help save hard drive space due to the RawEmailIn column containing Base64 attachments.</summary>
		public static string CleanUpRawEmails() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod());
			}
			//Get all clear text emailmessages that can have their RawEmailIn columns safely manipulated from the inbox.
			//These emails are safe to remove attachments from the RawEmailIn because they have already been digested and attchments extracted.
			string command="SELECT EmailMessageNum FROM emailmessage "
				+"WHERE RawEmailIn!='' "
				+"AND SentOrReceived IN ("+POut.Int((int)EmailSentOrReceived.Received)+","+POut.Int((int)EmailSentOrReceived.Read)+")";
			//POut.Int((int)EmailSentOrReceived.ReceivedDirect)+","+POut.Int((int)EmailSentOrReceived.ReadDirect)
			//We might need to include encrypted emails in the future if the email table is large due to encrypted emails.
			//Currently not including encrypted emails because the computer running this tool would need the private key to decrypt the message and
			//we would need to take an extra step at the end (after cleaning up attachments) to re-encrypt the modified email message. 
			//The current customers complaining only have bloat with clear text emails so that is where we are going to start with the clean up tool.
			ODEvent.Fire(new ODEventArgs("RawEmailCleanUp",Lans.g("DatabaseMaintenance","Getting email messages from the database...")));
			DataTable tableEmailMessageNums=Db.GetTable(command);
			if(tableEmailMessageNums.Rows.Count==0) {
				return Lans.g("DatabaseMaintenance","There are no email messages that need to be cleaned up.");
			}
			List<EmailAddress> listEmailAddresses=EmailAddresses.GetAll();//Do not use the cache because the cache doesn't contain all email addresses.
			int noChangeCount=0;
			int errorCount=0;
			int cleanedCount=0;
			int index=1;
			//Call the processing email logic for each email which will clear out the RawEmailIn column if the email is successfully digested.
			foreach(DataRow row in tableEmailMessageNums.Rows) {
				ODEvent.Fire(new ODEventArgs("RawEmailCleanUp",Lans.g("DatabaseMaintenance","Processing email message")
					+"  "+index.ToString()+" / "+tableEmailMessageNums.Rows.Count.ToString()));
				index++;
				EmailMessage emailMessage=EmailMessages.GetOne(PIn.Long(row["EmailMessageNum"].ToString()));
				EmailMessage oldEmailMessage=emailMessage.Copy();
				//Try and find the corresponding email address for this email.
				EmailAddress emailAddress=listEmailAddresses.FirstOrDefault(x => x.EmailUsername.ToLower()==emailMessage.RecipientAddress.ToLower());
				if(emailAddress==null) {
					errorCount++;
					continue;
				}
				try {
					EmailMessage emailMessageNew=EmailMessages.ProcessRawEmailMessageIn(emailMessage.RawEmailIn,emailMessage.EmailMessageNum
						,emailAddress,false,oldEmailMessage.SentOrReceived);
					if(Crud.EmailMessageCrud.UpdateComparison(emailMessageNew,oldEmailMessage)) {
						cleanedCount++;
					}
					else {//No changes.
						noChangeCount++;
					}
				}
				catch(Exception) {
					//Nothing to do, don't worry about it.
					errorCount++;
				}
			}
			if(DataConnection.DBtype==DatabaseType.MySql && tableEmailMessageNums.Rows.Count!=noChangeCount) {//Using MySQL and something actually changed.
																																																				//Optimize the emailmessage table so that the user can see the space savings within a File Explorer right away.
				ODEvent.Fire(new ODEventArgs("RawEmailCleanUp",Lans.g("DatabaseMaintenance","Optimizing the email message table...")));
				OptimizeTable("emailmessage");
			}
			string strResults=Lans.g("DatabaseMaintenance","Done.  No clean up required.");
			if(cleanedCount > 0 || errorCount > 0) {
				strResults=Lans.g("DatabaseMaintenance","Total email messages considered")+": "+tableEmailMessageNums.Rows.Count.ToString()+"\r\n"
					+Lans.g("DatabaseMaintenance","Email messages successfully cleaned up")+": "+cleanedCount.ToString()+"\r\n"
					+Lans.g("DatabaseMaintenance","Email messages that did not nead to be cleaned up")+": "+noChangeCount.ToString()+"\r\n"
					+Lans.g("DatabaseMaintenance","Email messages that failed to be cleaned up")+": "+errorCount.ToString();
			}
			return strResults;
		}

		///<summary>Similar to InsPlans.ComputeEstimatesForPatNums(...)</summary>
		public static void RecalcEstimates(List<Procedure> listProcs) {
			List<long> listPatNums=listProcs.Select(x => x.PatNum).Distinct().ToList();
			//No need to check RemotingRole; no call to db.
			long patNum=0;
			for(int i = 0;i<listPatNums.Count;i++) {
				patNum=listPatNums[i];
				Family fam=Patients.GetFamily(patNum);
				Patient pat=fam.GetPatient(patNum);
				//Only grab the procedures that have not been completed yet.
				List<Procedure> listNonCompletedProcs=listProcs.FindAll(x => x.PatNum==patNum);
				List<ClaimProc> listClaimProcs=ClaimProcs.GetForProcs(listNonCompletedProcs.Select(x => x.ProcNum).ToList());
				//Only use the claim procs associated to the non-completed procedures.
				List<ClaimProc> listNonCompletedClaimProcs=listClaimProcs.FindAll(x => listNonCompletedProcs.Exists(y => y.ProcNum==x.ProcNum));
				List<InsSub> listSubs=InsSubs.RefreshForFam(fam);
				List<InsPlan> listPlans=InsPlans.RefreshForSubList(listSubs);
				List<PatPlan> listPatPlans=PatPlans.Refresh(patNum);
				List<Benefit> listBenefits=Benefits.Refresh(listPatPlans,listSubs);
				Procedures.ComputeEstimatesForAll(patNum,listNonCompletedClaimProcs,listNonCompletedProcs,listPlans,listPatPlans,listBenefits,pat.Age,listSubs,null,true);
				Patients.SetHasIns(patNum);
			}
		}

		///<summary>Detaches all patient payments attached to insurance payment plans and all insurance payments attached to patient payment plans.
		///Returns a description of the changes that were made so that the user can go make manual changes if necessary.</summary>
		public static string DetachInvalidPaymentPlanPayments() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetString(MethodBase.GetCurrentMethod());
			}
			string resultsMsg="";
			DataTable table=GetPaySplitsAttachedToInsurancePaymentPlan();
			if(table.Rows.Count > 0) {
				string command="UPDATE paysplit SET PayPlanNum=0 WHERE SplitNum IN("
					+string.Join(",",table.Select().Select(x => x["SplitNum"].ToString()))+")";
				Db.NonQ(command);
				resultsMsg+=Lans.g(_lanThis,"The following patient payments were detached from insurance payment plans.  It is recommended you verify "
					+"these accounts are correct.");
				for(int i=0;i<table.Rows.Count;i++) {
					resultsMsg+="\r\n   "+Lans.g(_lanThis,"Patient #")+" "+table.Rows[i]["PatNum"].ToString()+" "
						+Lans.g(_lanThis,"had a payment amount for")+" "+PIn.Double(table.Rows[i]["SplitAmt"].ToString()).ToString("c")+" "
						+Lans.g(_lanThis,"on date")+" "+PIn.Date(table.Rows[i]["DatePay"].ToString()).ToShortDateString()+" "
						+Lans.g(_lanThis,"attached to insurance payment plan #")+table.Rows[i]["PayPlanNum"];
				}
			}
			table=GetClaimProcsAttachedToPatientPaymentPlans();
			if(table.Rows.Count > 0) {
				string command="UPDATE claimproc SET PayPlanNum=0 WHERE ClaimProcNum IN("
					+string.Join(",",table.Select().Select(x => x["ClaimProcNum"].ToString()))+")";
				Db.NonQ(command);
				if(resultsMsg!="") {
					resultsMsg+="\r\n\r\n";
				}
				resultsMsg+=Lans.g(_lanThis,"The following insurance payments were detached from patient payment plans.  It is recommended you verify "
					+"these accounts are correct.");
				for(int i = 0;i<table.Rows.Count;i++) {
					resultsMsg+="\r\n   "+Lans.g(_lanThis,"Patient #")+table.Rows[i]["PatNum"].ToString()+" "
						+Lans.g(_lanThis,"had a payment amount for")+" "+PIn.Double(table.Rows[i]["InsPayAmt"].ToString()).ToString("c")+" "
						+Lans.g(_lanThis,"on date")+" "+PIn.Date(table.Rows[i]["DateCP"].ToString()).ToShortDateString()+" "
						+Lans.g(_lanThis,"attached to patient payment plan #")+table.Rows[i]["PayPlanNum"];
				}
			}
			if(resultsMsg=="") {
				resultsMsg+=Lans.g(_lanThis,"No payments found that needed to be detached from payment plans.");
			}
			return resultsMsg;
		}

		///<summary>Gets the DataTable that contains paysplits attached to insurance payment plans.
		///Table will contain the following columns; SplitNum, PatNum, SplitAmt, DatePay, PayPlanNum</summary>
		private static DataTable GetPaySplitsAttachedToInsurancePaymentPlan() {
			//Need to check remoting role before calling; private method
			string command="SELECT paysplit.SplitNum,paysplit.PatNum,paysplit.SplitAmt,paysplit.DatePay,paysplit.PayPlanNum FROM paysplit "
				+"INNER JOIN payplan ON payplan.PayPlanNum=paysplit.PayPlanNum "
				+"WHERE paysplit.PayPlanNum!=0 "
				+"AND payplan.PlanNum!=0 ";//insurance payment plan
			return Db.GetTable(command);
		}

		///<summary>Gets claim procs that are attached to patient payment plans.
		///Table will contain the following columns; ClaimProcNum, PatNum, InsPayAmt, DateCP, PayPlanNum</summary>
		private static DataTable GetClaimProcsAttachedToPatientPaymentPlans() {
			//Need to check remoting role before calling; private method
			string command="SELECT claimproc.ClaimProcNum,claimproc.PatNum,claimproc.InsPayAmt,claimproc.DateCP,claimproc.PayPlanNum FROM claimproc "
				+"INNER JOIN payplan ON payplan.PayPlanNum=claimproc.PayPlanNum "
				+"WHERE claimproc.PayPlanNum!=0 "
				+"AND payplan.PlanNum=0 ";//standard payment plan
			return Db.GetTable(command);
		}

		#endregion Tool Button and Helper Methods-----------------------------------------------------------------------------------------------------------

		///<summary>Uses reflection to get all database maintenance methods that are specifically flagged for DBM.
		///When clinicNum is set to a medical clinic, all methods that match "tooth" will not be returned.</summary>
		public static List<MethodInfo> GetMethodsForDisplay(long clinicNum=0,bool hasOnlyPatNumMethods=false) {
			//No need to check RemotingRole; no call to db.
			List<MethodInfo> listDbmMethodsGrid=new List<MethodInfo>();
			//Grab all methods from the DatabaseMaintenance class to dynamically fill the grid.
			MethodInfo[] arrayDbmMethodsAll=(typeof(DatabaseMaintenances)).GetMethods();
			//Sort the methods by name so that they are easier for users to find desired methods to run.
			Array.Sort(arrayDbmMethodsAll,new MethodInfoComparer());
			bool isMedicalClinic=Clinics.IsMedicalPracticeOrClinic(clinicNum);
			foreach(MethodInfo meth in arrayDbmMethodsAll) {
				DbmMethodAttr dbmAttribute=(DbmMethodAttr)Attribute.GetCustomAttribute(meth,typeof(DbmMethodAttr));
				if(dbmAttribute==null) {
					continue;//This is not a valid DBM method.
				}
				if(isMedicalClinic && Regex.IsMatch(meth.Name,"tooth",RegexOptions.IgnoreCase)) {
					continue;//This is not a DBM for medical users.
				}
				if(hasOnlyPatNumMethods && !dbmAttribute.HasPatNum) {
					continue;//This is not a patient specific DBM method.
				}
				//This is a valid DBM method and should be added to the list of methods to display to the user.
				listDbmMethodsGrid.Add(meth);
			}
			return listDbmMethodsGrid;
		}

		///<summary>Returns true if the method passed in supports break down.</summary>
		public static bool MethodHasBreakDown(MethodInfo method) {
			//No need to check RemotingRole; no call to db.
			return method.GetCustomAttributes(typeof(DbmMethodAttr),true).OfType<DbmMethodAttr>().All(x => x.HasBreakDown);
		}
	}
}
