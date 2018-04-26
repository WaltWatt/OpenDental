using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using MySql.Data.MySqlClient;
using System.Linq;

namespace DatabaseIntegrityCheck {
	public partial class FormDatabaseCheck:Form {
		private List<string> _corruptTables;
		private bool _patientRowsLost;
		MySqlConnection _con;

		public FormDatabaseCheck() {
			InitializeComponent();
		}

		private void FormDatabaseCheck_Load(object sender,EventArgs e) {
			labelProgress.Text="";
			XmlDocument document=new XmlDocument();
			if(!File.Exists("FreeDentalConfig.xml")) {
				textComputerName.Text="localhost";
				#if(TRIALONLY)
					textDatabase.Text="demo";
				#else
					textDatabase.Text="opendental";
				#endif
				textUser.Text="root";
				return;
			}
			string passHash="";
			try {
				document.Load("FreeDentalConfig.xml");
				XmlNodeReader reader=new XmlNodeReader(document);
				string currentElement="";
				while(reader.Read()) {
					if(reader.NodeType==XmlNodeType.Element) {
						currentElement=reader.Name;
					}
					else if(reader.NodeType==XmlNodeType.Text) {
						switch(currentElement) {
							case "ComputerName":
								textComputerName.Text=reader.Value;
								break;
							case "Database":
								textDatabase.Text=reader.Value;
								break;
							case "User":
								textUser.Text=reader.Value;
								break;
							case "Password":
								textPassword.Text=reader.Value;
								break;
							case "MySQLPassHash":
								passHash=reader.Value;
								break;
						}
					}
				}
				reader.Close();
			}
			catch(Exception) {
				//MessageBox.Show(e.Message);
				textComputerName.Text="localhost";
				textDatabase.Text="opendental";
				textUser.Text="root";
			}
			string decryptedPwd;
			if(textPassword.Text=="" && passHash!="" && CDT.Class1.Decrypt(passHash,out decryptedPwd)) {
				textPassword.Text=decryptedPwd;
			}
			textPassword.PasswordChar=textPassword.Text==""?default(char):'*';//mask password on leave
		}

		private void textPassword_TextChanged(object sender,EventArgs e) {
			if(textPassword.Text=="") {
				textPassword.PasswordChar=default(char);//if text is cleared, turn off password char mask
			}
		}

		private void textPassword_Leave(object sender,EventArgs e) {
			textPassword.PasswordChar=textPassword.Text==""?default(char):'*';//mask password on leave
		}		
				
		private DataTable GetTable(string command) {
			MySqlCommand cmd=new MySqlCommand();
			cmd.CommandTimeout=1200;//Timeout for all check table and repair table commands.  Twenty minutes per command.  Needed for large databases (NADG)
			cmd.CommandText=command; 
			cmd.Connection=_con;
			DataTable table=new DataTable();
			MySqlDataAdapter da=new MySqlDataAdapter(cmd);
			da.Fill(table);
			return table;
		}

		private void ShowProgress(string message) {
			labelProgress.Text=message;
			Application.DoEvents();
		}

		private void SaveToLog(string results,string repairTime) {
			try {
				string logDirectory = "DatabaseIntegrityCheckLog";
				if(!Directory.Exists(logDirectory)) {
					Directory.CreateDirectory(logDirectory);
				}
				FileStream fs = new FileStream(CombinePaths(logDirectory,"RepairLog_"+repairTime+".txt"),FileMode.Append,FileAccess.Write,FileShare.Read);
				StreamWriter sw = new StreamWriter(fs);
				sw.Write(results);
				sw.Close();
				sw=null;
				fs.Close();
				fs=null;
			}
			catch {
				textResults.AppendText("Unable to save results file.\r\n\r\n");
			}
		}

		private bool DirectoryCopy(string databaseDirectory,string dbName) {
			//from FormBackup.InstanceMethodBackup.
			try {
				string backupDateTime=DateTime.Now.ToString("ddMMyyyy_HHmmss");
				string toPath=CombinePaths(databaseDirectory,dbName+"backup_"+backupDateTime);
				string fromPath=CombinePaths(databaseDirectory,dbName);
				DirectoryInfo dirInfo=new DirectoryInfo(fromPath);//does not check to see if dir exists
				Directory.CreateDirectory(toPath);
				FileInfo[] files=dirInfo.GetFiles();
				for(int i=0;i<files.Length;i++){
					string fromFile=files[i].FullName;
					string toFile=CombinePaths(toPath,files[i].Name);
					if(File.Exists(toFile)) {
						if(files[i].LastWriteTime!=File.GetLastWriteTime(toFile)) {//if modification dates don't match
							FileAttributes fa=File.GetAttributes(toFile);
							bool isReadOnly=((fa&FileAttributes.ReadOnly)==FileAttributes.ReadOnly);
							if(isReadOnly) {
								//If the destination file exists and is marked as read only, then we must mark it as a
								//normal read/write file before it may be overwritten.
								File.SetAttributes(toFile,FileAttributes.Normal);//Remove read only from the destination file.
							}
							File.Copy(fromFile,toFile,true);
						}
					} else {//file doesn't exist, so just copy
						File.Copy(fromFile,toFile);
					}
				}				
				return true;//successful
			}
			catch (Exception ex){
				MessageBox.Show("Unable to make backup, aborting repair. Please make sure you have sufficient privileges and try again."
					+"\r\n"+ex.StackTrace);
				return false;//error occured
			}
		}

		private int CheckInnoDb() {
			string command="SELECT TABLE_NAME FROM INFORMATION_SCHEMA.tables WHERE TABLE_SCHEMA='"+textDatabase.Text+"' AND ENGINE NOT LIKE 'MyISAM'";
			return GetTable(command).Rows.Count;
		}

		///<summary>Copied from ODFileUtils.CombinePaths.</summary>
		public static string CombinePaths(params string[] paths){
			string finalPath="";
			for(int i=0;i<paths.Length;i++){
				string path=RemoveTrailingSeparators(paths[i]);
				//Add an appropriate slash to divide the path peices, but do not use a trailing slash on the last piece.
				if(i<paths.Length-1){
					if(path!=null && path.Length>0){
						path=path+Path.DirectorySeparatorChar;
					}
				}
				finalPath=finalPath+path;
			}
			return finalPath;
		}

		///<summary>Removes a trailing path separator from the given string if one exists.</summary>
		public static string RemoveTrailingSeparators(string path){
			while(path!=null && path.Length>0 && (path[path.Length-1]=='\\' || path[path.Length-1]=='/')) {
				path=path.Substring(0,path.Length-1);
			}
			return path;
		}

		private bool OpenConnection() {
			_con=new MySqlConnection("Server="+textComputerName.Text
				+";Database="+textDatabase.Text
				+";User ID="+textUser.Text
				+";Password="+textPassword.Text
				+";CharSet=utf8");
			try {
				_con.Open();
			}
			catch {
				MessageBox.Show("Unable to connect to the database on the specified server.");
				return false;
			}
			return true;
		}

		private void butCheck_Click(object sender,EventArgs e) {
			if(!OpenConnection()) {
				return;
			}
			string command="SHOW FULL TABLES WHERE Table_type='BASE TABLE'";//Tables, not views.  Does not work in MySQL 4.1, however we test for MySQL version >= 5.0 in PrefL.
			try {
				Cursor=Cursors.WaitCursor;
				DataTable tableAllTables=GetTable(command);
				List<string> listTableNames = tableAllTables.Rows.OfType<DataRow>().Select(x => x[0].ToString()).ToList();
				_corruptTables=new List<string>();
				List<string> listCheckFailed = new List<string>();
				foreach(string tableName in listTableNames) {
					ShowProgress("Checking table "+tableName);
					try {
						command="CHECK TABLE "+tableName;
						DataTable tableOneTable=GetTable(command);
						int lastRow=tableOneTable.Rows.Count-1;
						int lastCol=tableOneTable.Columns.Count-1;
						string lastcell = tableOneTable.Rows[lastRow][lastCol].ToString();
						if(lastcell!="OK") {
							_corruptTables.Add(tableName);
						}
					}
					catch {
						listCheckFailed.Add(tableName);
					}
				}
				ShowProgress("");
				Cursor=Cursors.Default;
				if(listCheckFailed.Count > 0) {
					MessageBox.Show("Table check failed for some tables.\r\n"
						+"For these tables, we could not determine if repair is needed:\r\n"
						+string.Join(",",listCheckFailed));
				}
				if(_corruptTables.Count==0) {
					textResults.Text="You have no corrupt tables.";
				}
				else {
					MessageBox.Show("You have the following corrupt tables:\r\n"
						+string.Join(",",_corruptTables)+"\r\n"
						+"Select 'Repair' while on the server to repair corrupt tables. A backup will be made for you.");
				}
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
			}
			finally {
				Cursor=Cursors.Default;
				_con.Close();
			}
		}

		private void butRepair_Click(object sender,EventArgs e) {
			_patientRowsLost=false;
			if(!OpenConnection()) {
				return;
			}
			if(CheckInnoDb()>0) {
				MessageBox.Show(this,"This tool cannot be run with InnoDb tables.");
				return;
			}
			//If tool is not currently being run on the server (i.e, textComputer.Text is not the current computer or localhost), they cannot use the Repair function.
			if(textComputerName.Text.ToLower()!=Environment.MachineName.ToLower() && textComputerName.Text.ToLower()!="localhost") {
				MessageBox.Show("Repair must be run on the server.");
				return;
			}
			string command="SELECT @@datadir";//this finds the path to the data directory that the databases are stored in.
			DataTable pathTable=GetTable(command);
			string dataPath=pathTable.Rows[0][0].ToString();
			string backupDateTime=DateTime.Now.ToString("ddMMyyyy_HHmmss");
			if(!DirectoryCopy(dataPath,textDatabase.Text)) {
				return;
			}
			//this tool would only be used with MySQL, so the current code is just fine.
			try {
				Cursor=Cursors.WaitCursor;
				string results="";
				if(_corruptTables==null || _corruptTables.Count<=0) {
					//run on all tables
					command="SHOW FULL TABLES WHERE Table_type='BASE TABLE'";
					DataTable table = GetTable(command);
					List<string> listTables = table.Rows.OfType<DataRow>().Select(x => x[0].ToString()).ToList();
					results=RepairTables(listTables);
				}
				else {
					results=RepairTables(_corruptTables);
				}
				textResults.Clear();
				SaveToLog(results,backupDateTime);
				ShowProgress("");
				if(_corruptTables!=null) {
					_corruptTables.Clear();
				}
				if(_patientRowsLost) {
					MessageBox.Show("Rows have been lost in the patient table. Please call support and escalate to a conversions engineer or a senior engineer.");
				}
				textResults.AppendText("Finished table repair. Check information in 'RepairLog_"+backupDateTime+".txt' to verify that tables are not corrupt."
					+"\r\n\r\nResults:\r\n"+results);
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);
			}
			finally {
				Cursor=Cursors.Default;
				_con.Close();
			}
		}

		private string RepairTables(List<string> listTablesToRepair) {
			string results="";
			for(int i = 0;i<listTablesToRepair.Count;i++) {
				ShowProgress("Repairing table "+listTablesToRepair[i]);
				DataTable table=GetTable("REPAIR TABLE "+listTablesToRepair[i]);
				for(int j = 0;j<table.Rows.Count;j++) {
					string line = "";
					for(int k = 0;k < table.Columns.Count;k++) {
						line+=table.Rows[j][k].ToString()+", ";
						if(listTablesToRepair[i]=="patient") {
							if(line.Contains("Number of rows changed")) {
								string data = line.Substring(line.IndexOf("from")+5).TrimEnd();
								string num2 = data.Substring(data.IndexOf("to")+3).TrimEnd(',');
								string num1 = data.Remove(data.IndexOf("to")).TrimEnd();
								if((Convert.ToInt32(num1))>(Convert.ToInt32(num2))) {
									_patientRowsLost=true;
								}
							}
						}
					}
					results+=line+"\r\n";
				}
			}
			return results;
		}

	}
}