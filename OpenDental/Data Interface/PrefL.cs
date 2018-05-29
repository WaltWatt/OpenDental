using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using CodeBase;
using Ionic.Zip;
using MySql.Data.MySqlClient;
using OpenDentBusiness;

namespace OpenDental {
	public class PrefL{

		///<summary>This ONLY runs when first opening the program.  It returns true if either no conversion is necessary, or if conversion was successful.  False for other situations like corrupt db, trying to convert to older version, etc.  Silent mode is mostly used from internal tools.  It is currently used in the Main Program if the silent command line argument is set.</summary>
		public static bool ConvertDB(bool silent,string toVersion,Form currentForm) {
			ClassConvertDatabase ClassConvertDatabase2=new ClassConvertDatabase();
			string pref=PrefC.GetString(PrefName.DataBaseVersion);
			if(ClassConvertDatabase2.Convert(pref,toVersion,silent,currentForm)) {
				return true;
			}
			else {
				if(FormOpenDental.ExitCode==0) {
					FormOpenDental.ExitCode=200;//Convert Database has failed during execution (Unknown Error)
				}
				Environment.Exit(FormOpenDental.ExitCode);
				return false;
			}
		}

		///<summary>This ONLY runs when first opening the program.  It returns true if either no conversion is necessary, or if conversion was successful.  False for other situations like corrupt db, trying to convert to older version, etc.</summary>
		public static bool ConvertDB(Form currentForm) {
			return ConvertDB(false,Application.ProductVersion,currentForm);
		}

		///<summary>Copies the installation directory files into the database.</summary>
		///<param name="versionCurrent">The versioning information that will go into the Manifest.txt</param>
		///<param name="isSilent">Set to true when upgrading silently.  No message boxes will show but errors will log and exit codes will be set.</param>
		///<param name="hasAtoZ">Set to true when a copy of the update files needs to be made in the AtoZ share (for backwards compatibility).</param>
		///<param name="hasConcatFiles">Set to true to also make one large concatenated row in the database (for backwards compatibility).
		///This method will not return false if this particular option has problems executing.
		///Making this singular row often times violates MySQL limitations which cause errors that cannot be easily avoided.
		///Therefore, this method has the potential to log an error of the concat files failing yet the method can still return true.</param>
		///<returns>Returns true if the update files were successfully copied into the database.</returns>
		public static bool CopyFromHereToUpdateFiles(Version versionCurrent,bool isSilent,bool hasAtoZ,bool hasConcatFiles,Form currentForm=null) {
			#region Get Valid AtoZ path
			if(hasAtoZ && PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ) {
				string prefImagePath=ImageStore.GetPreferredAtoZpath();
				if(prefImagePath==null || !Directory.Exists(prefImagePath)) {//AtoZ folder not found
					if(isSilent) {
						FormOpenDental.ExitCode=300;//AtoZ folder not found (Warning)
						return false;
					}
					FormPath FormP=new FormPath();
					FormP.IsStartingUp=true;
					FormP.ShowDialog();
					if(FormP.DialogResult!=DialogResult.OK) {
						MsgBox.Show("Prefs","Invalid A to Z path.  Closing program.");
						FormOpenDental.ExitCode=300;//AtoZ folder not found (Warning)
						return false;
					}
				}
			}
			#endregion
			#region Start Notification Thread
			Action actionCloseRecopyProgress=null;
			if(!isSilent) {
				actionCloseRecopyProgress=ODProgressOld.ShowProgressStatus("RecopyProgress",currentForm:currentForm);
			}
			#endregion
			#region Delete Old UpdateFiles Folders
			string folderTempUpdateFiles=ODFileUtils.CombinePaths(PrefC.GetTempFolderPath(),"UpdateFiles");
			string folderAtoZUpdateFiles="";
			if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ && hasAtoZ) {
				folderAtoZUpdateFiles=ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),"UpdateFiles");
			}
			ODEvent.Fire(new ODEventArgs("RecopyProgress",Lan.g("Prefs","Removing old update files...")));
			//Try to delete the UpdateFiles folder from both the AtoZ share and the local TEMP dir.
			if(!DeleteFolder(folderAtoZUpdateFiles) | !DeleteFolder(folderTempUpdateFiles)) {//Logical OR to prevent short circuit.
				actionCloseRecopyProgress?.Invoke();
				FormOpenDental.ExitCode=301;//UpdateFiles folder cannot be deleted (Warning)
				if(!isSilent) {
					MsgBox.Show("Prefs","Unable to delete old UpdateFiles folder.  Go manually delete the UpdateFiles folder then retry.");
				}
				return false;
			}
			#endregion
			#region Copy Current Installation Directory To UpdateFiles Folders
			ODEvent.Fire(new ODEventArgs("RecopyProgress",Lan.g("Prefs","Backing up new update files...")));
			//Copy the installation directory files to the UpdateFiles share and a TEMP dir that we just created which we will zip up and insert into the db.
			//When PrefC.AtoZfolderUsed is true and we're upgrading from a version prior to 15.3.10, this copy that we are about to make allows backwards 
			//compatibility for versions of OD that do not look at the database for their UpdateFiles.
			if(!CopyInstallFilesToPath(folderAtoZUpdateFiles,versionCurrent) | !CopyInstallFilesToPath(folderTempUpdateFiles,versionCurrent)) {
				actionCloseRecopyProgress?.Invoke();
				FormOpenDental.ExitCode=302;//Installation files could not be copied.
				if(!isSilent) {
					MsgBox.Show("Prefs","Failed to copy the current installation files on this computer.\r\n"
						+"This could be due to a lack of permissions, or file(s) in the installation directory are still in use.");
				}
				return false;
			}
			#endregion
			#region Get Current MySQL max_allowed_packet Setting
			//Starting in v15.3, we always insert the UpdateFiles into the database.
			int maxAllowedPacket=0;
			int defaultMaxAllowedPacketSize=41943040;//40MB
			if(DataConnection.DBtype==DatabaseType.MySql) {
				ODEvent.Fire(new ODEventArgs("RecopyProgress",Lan.g("Prefs","Getting MySQL max allowed packet setting...")));
				maxAllowedPacket=MiscData.GetMaxAllowedPacket();
				//If trying to get the max_allowed_packet value for MySQL failed, assume they can handle 40MB of data.
				//Our installations of MySQL defaults the global property 'max_allowed_packet' to 40MB.
				//Nathan suggested forcing the global and local max_allowed_packet to 40MB if it was set to anything less.
				if(maxAllowedPacket < defaultMaxAllowedPacketSize) {
					try {
						maxAllowedPacket=MiscData.SetMaxAllowedPacket(defaultMaxAllowedPacketSize);
					}
					catch(Exception ex) {
						//Do nothing.  Either maxAllowedPacket is set to something small (e.g. 10MB) and we failed to update it to 40MB (should be fine)
						//             OR we failed to get and set the global variable due to MySQL permissions and a UE was thrown.
						//             Regardless, if maxAllowedPacket is 0 (the only thing that we can't have happen) it will get updated to 40MB later down.
						EventLog.WriteEntry("OpenDental","Error updating max_allowed_packet from "+maxAllowedPacket
							+" to "+defaultMaxAllowedPacketSize+":\r\n"+ex.Message,EventLogEntryType.Error);
					}
				}
			}
			//Only change maxAllowedPacket if we couldn't successfully get the current value from the database or using Oracle.
			//This will let the program attempt to insert the UpdateFiles into the db with the assumption that they are using our default setting (40MB).
			//Worst case scenario, the user will hit the max_packet_allowed error below which will simply notify them to update their my.ini manually.
			if(maxAllowedPacket==0) {
				maxAllowedPacket=defaultMaxAllowedPacketSize;
			}
			//Now we need to break up the memory stream into a Base64 string but each payload needs to be small enough to send to MySQL.
			//Each character in Base64 represents 6 bits.  Therefore, 4 chars are used to represent 3 bytes
			//Therefore we have to read an amout of bytes per loop that must be divisible by 3. 
			//Also, we want to 'buffer' a few KB for MySQL because the query itself and the parameter information will take up some bytes (unknown).
			int charsPerPayload=maxAllowedPacket-8192;//Arbitrarily subtracted 8KB from max allowed bytes for MySQL "header" information.
			charsPerPayload-=(charsPerPayload % 3);//Use the closest amount of bytes divisible by 3.
			#endregion
			#region Zip Update Files Into Memory
			MemoryStream memStream=new MemoryStream();
			ZipFile zipFile=new ZipFile();
			//Take the entire directory in the temp dir that we just created and zip it up.
			ODEvent.Fire(new ODEventArgs("RecopyProgress",Lan.g("Prefs","Compressing new update files...")));
			try {
				zipFile.AddDirectory(folderTempUpdateFiles);
				zipFile.Save(memStream);
			}
			catch(Exception ex) {
				memStream.Dispose();
				zipFile.Dispose();
				actionCloseRecopyProgress?.Invoke();
				FormOpenDental.ExitCode=304;//Error compressing UpdateFiles
				if(!isSilent) {
					MessageBox.Show(Lan.g("Prefs","Error compressing UpdateFiles:")+"\r\n"+ex.Message);
				}
				return false;
			}
			#endregion
			#region Insert Update Files Into One Row
			if(hasConcatFiles) {
				//For backwards compatibility we have to try and store the entire UpdateFiles content into one row
				//Everything within this section will be in a try catch because we found out that it can fail due to the amount of data being sent.
				//The MySQL CONCAT command gives up on life after so much data and sets the column to 0 bytes but does not throw an exception.
				//This is simply here to help reduce the number of offices that might have problems updating from older versions.
				//E.g. buying a new workstation and using an old trial installer could require this single large Update Files column.
				try {
					//Converting the file to Base64String bloats the size by approximately 30% so we need to make sure that the chunk size is well below 40MB
					//Old code used 15MB and that seemed to work very well for the majority of users.
					charsPerPayload=Math.Min(charsPerPayload,15728640);//15728640 is divisible by 3 which is important for Base64 "appending" logic.
					ODEvent.Fire(new ODEventArgs("RecopyProgress",Lan.g("Prefs","Deleting old update files row...")));
					DocumentMiscs.DeleteAllForType(DocumentMiscType.UpdateFiles);
					byte[] zipFileBytes=new byte[charsPerPayload];
					memStream.Position=0;//Start at the beginning of the stream.
					ODEvent.Fire(new ODEventArgs("RecopyProgress",Lan.g("Prefs","Inserting new update files into database row...")));
					DocumentMisc docUpdateFiles=new DocumentMisc();
					docUpdateFiles.DateCreated=DateTime.Today;
					docUpdateFiles.DocMiscType=DocumentMiscType.UpdateFiles;
					docUpdateFiles.FileName="UpdateFiles.zip";
					DocumentMiscs.Insert(docUpdateFiles);
					while((memStream.Read(zipFileBytes,0,zipFileBytes.Length))>0) {
						DocumentMiscs.AppendRawBase64ForUpdateFiles(Convert.ToBase64String(zipFileBytes));
					}
				}
				catch(Exception ex) {
					//Only log the error, do not stop the update process.  The above code is known to fail for various reasons and we abandoned it.
					ODEvent.Fire(new ODEventArgs("RecopyProgress",Lan.g("Prefs","Error inserting new update files into database row...")));
					EventLog.WriteEntry("OpenDental","Error inserting new update files into database row:\r\n"+ex.Message,EventLogEntryType.Error);
				}
			}
			#endregion
			#region Insert Update Files Segments Into Many Rows
			//When we try and send over ~40MB of data, MySQL can drop our connection randomly giving a "MySQL server has gone away" error.
			//Use a maximum of ~1MB payloads so that the likelyhood of this error is less.
			charsPerPayload=Math.Min(charsPerPayload,1048575);//1048575 is divisible by 3 which is important for Base64 "appending" logic.
			try {
				//Clear and prep the current UpdateFiles row in the documentmisc table for the updated binaries.
				ODEvent.Fire(new ODEventArgs("RecopyProgress",Lan.g("Prefs","Deleting old update files segments...")));
				DocumentMiscs.DeleteAllForType(DocumentMiscType.UpdateFilesSegment);
				byte[] zipFileBytes=new byte[charsPerPayload];
				memStream.Position=0;//Start at the beginning of the stream.
				//Convert the zipped up bytes into Base64 and instantly insert it into the database little by little.
				ODEvent.Fire(new ODEventArgs("RecopyProgress",Lan.g("Prefs","Inserting new update files segments into database...")));
				try {
					int count=1;
					DocumentMisc docUpdateFilesSegment=new DocumentMisc();
					docUpdateFilesSegment.DateCreated=DateTime.Today;
					docUpdateFilesSegment.DocMiscType=DocumentMiscType.UpdateFilesSegment;
					while((memStream.Read(zipFileBytes,0,zipFileBytes.Length))>0) {
						docUpdateFilesSegment.FileName=count.ToString().PadLeft(4,'0');
						docUpdateFilesSegment.RawBase64=Convert.ToBase64String(zipFileBytes);
						DocumentMiscs.Insert(docUpdateFilesSegment);
						count++;
					}
					ODEvent.Fire(new ODEventArgs("RecopyProgress",Lan.g("Prefs","Done...")));
				}
				catch(MySqlException myEx) {
					EventLog.WriteEntry("OpenDental","Error inserting UpdateFiles into database:"
						+"\r\nMySqlException: "+myEx.Message
						+"\r\n  maxAllowedPacket: "+maxAllowedPacket
						+"\r\n  charsPerPayload: "+charsPerPayload
						+"\r\n  memStream.Length: "+memStream.Length,EventLogEntryType.Error);
					throw myEx;
				}
				catch(Exception ex) {
					EventLog.WriteEntry("OpenDental","Error inserting UpdateFiles into database:"
						+"\r\n"+ex.Message
						+"\r\n  maxAllowedPacket: "+maxAllowedPacket
						+"\r\n  charsPerPayload: "+charsPerPayload
						+"\r\n  memStream.Length: "+memStream.Length,EventLogEntryType.Error);
					throw ex;
				}
			}
			catch(MySqlException myEx) {
				actionCloseRecopyProgress?.Invoke();
				FormOpenDental.ExitCode=303;//Failed inserting update files into the database.
				if(isSilent) {
					return false;
				}
				string errorStr=Lan.g("Prefs","Failed inserting update files into the database.");
				if(myEx.Number.In((int)MySqlErrorCode.TooLongString,(int)MySqlErrorCode.WarningAllowedPacketOverflowed,(int)MySqlErrorCode.PacketTooLarge)) {
					errorStr+="\r\n"+Lan.g("Prefs","Please call us or have your IT admin increase the max_allowed_packet to 40MB in the my.ini file.");
					try {
						string innoDBTableNames=InnoDb.GetInnodbTableNames();
						if(innoDBTableNames!="") {
							//Starting in MySQL 5.6 you can specify innodb_log_file_size in the my.ini and it typicaly needs to be set higher than 48 MB (default).
							//There is danger in manipulating this value so we should not do it for the customer but have their IT do it.
							//Since the innodb_log_size variable only exists in MySQL 5.6 and greater, if the user adds this setting to their my.ini file for an
							//older version, then MySQL will fail to start.  
							//An alternative solution would be to convert their tables over to MyISAM instead of letting them continue with InnoDB (if possible).
							//The following message to the user is vague on purpose to avoid listing version numbers.
							errorStr+="\r\n"+Lan.g("Prefs","InnoDB tables have been detected, you may need to increase the innodb_log_file_size variable.");
						}
					}
					catch(Exception) {
						//Do not add the additional InnoDB warning because it will often times just confuse our typical users (odds are they are using MyISAM).
					}
				}
				else {
					errorStr+="\r\n"+Lan.g("Prefs","MySqlException")+": "+myEx.Message;
				}
				MessageBox.Show(errorStr);
				return false;
			}
			catch(Exception ex) {//non-MySqlException
				actionCloseRecopyProgress?.Invoke();
				FormOpenDental.ExitCode=303;//Failed inserting update files into the database.
				if(isSilent) {
					return false;
				}
				MessageBox.Show(Lan.g("Prefs","Failed inserting update files into the database.")+"\r\n"+ex.Message);
				return false;
			}
			finally {
				if(memStream!=null) {
					memStream.Dispose();
				}
				if(zipFile!=null) {
					zipFile.Dispose();
				}
			}
			#endregion
			actionCloseRecopyProgress?.Invoke();
			return true;//Inserting a compressed version of the files in the current installation directory into the database was successful.
		}

		///<summary>Creates the directory passed in and copies the files in the current installation directory to the specified folder path.
		///Creates a Manifest.txt file with the version information passed in.
		///Returns false if anything goes wrong.  Returns true if copy was successful or if the folder path passed in is null or blank.</summary>
		private static bool CopyInstallFilesToPath(string folderPath,Version versionCurrent) {
			if(string.IsNullOrEmpty(folderPath)) {
				return true;//Some customers will not be using the AtoZ folder which will pass in blank.
			}
			try {
				Directory.CreateDirectory(folderPath);
				DirectoryInfo dirInfo=new DirectoryInfo(Application.StartupPath);
				FileInfo[] appfiles=dirInfo.GetFiles();
				for(int i = 0;i<appfiles.Length;i++) {
					if(appfiles[i].Name=="FreeDentalConfig.xml") {
						continue;//skip this one.
					}
					if(appfiles[i].Name=="OpenDentalServerConfig.xml") {
						continue;//skip also
					}
					if(appfiles[i].Name=="ProximitySettings.txt") {
						continue;//skip as well
					}
					if(appfiles[i].Name=="DpsPos.dll.config") {
						continue;//yep, skip it
					}
					if(appfiles[i].Name.StartsWith("openlog")) {
						continue;//these can be big and are irrelevant
					}
					if(appfiles[i].Name.Contains("__")) {//double underscore
						continue;//So that plugin dlls can purposely skip the file copy.
					}
					//include UpdateFileCopier
					File.Copy(appfiles[i].FullName,ODFileUtils.CombinePaths(folderPath,appfiles[i].Name));
				}
				//Create a simple manifest file so that we know what version the files are for.
				File.WriteAllText(ODFileUtils.CombinePaths(folderPath,"Manifest.txt"),versionCurrent.ToString(3));
			}
			catch(Exception) {
				return false;
			}
			return true;
		}

		///<summary>Recursively deletes all folders and files in the provided folder path.
		///Waits up to 10 seconds for the delete command to finish.  Returns false if anything goes wrong.</summary>
		private static bool DeleteFolder(string folderPath) {
			if(string.IsNullOrEmpty(folderPath)) {
				return true;//Nothing to delete.
			}
			if(!Directory.Exists(folderPath)) {
				return true;//Already deleted
			}
			//The directory we want to delete is present so try and recursively delete it and all its content.
			try {
				Directory.Delete(folderPath,true);
			}
			catch {
				return false;//Something went wrong, typically a permission issue or files are still in use.
			}
			//The delete seems to have been successful, wait up to 10 seconds so that CreateDirectory won't malfunction.
			DateTime dateTimeWait=DateTime.Now.AddSeconds(10);
			while(Directory.Exists(folderPath) && DateTime.Now < dateTimeWait) {
				Application.DoEvents();
			}
			if(Directory.Exists(folderPath)) {//Dir is still present after 10 seconds of waiting for the delete to complete.
				return false;
			}
			return true;//Dir deleted successfully.
		}

		///<summary>Called from FormOpenDental.PrefsStartup.</summary>
		public static bool CheckProgramVersion(bool isSilent=false) {
#if DEBUG
			return true;//Development mode never needs to check versions or copy files to other directories.  Simply return true at this point.
#else
			if(PrefC.GetBool(PrefName.UpdateWindowShowsClassicView)) {
				if(isSilent) {
					FormOpenDental.ExitCode=399;//Classic View is not supported with Silent Update
					return false;
				}
				return CheckProgramVersionClassic();
			}
			string updateServerName = PrefC.GetString(PrefName.WebServiceServerName);
			Version storedVersion=new Version(PrefC.GetString(PrefName.ProgramVersion));
			Version currentVersion=new Version(Application.ProductVersion);
			//Regardless of the WebServiceServerName preference (update server) if the stored version if higher than the current version then this
			//computer needs to be upgraded to the version of the program that is stored within the database (or UpdateFiles folder in AtoZ).
			if(storedVersion>currentVersion) {
				if(isSilent) {//This should never happen after a silent update.
					FormOpenDental.ExitCode=312;//Stored version is higher that client version after an update was successful.
					return false;
				}
				//At this point we know with absolute certainty that we cannot allow this version to connect to the database.
				//Attempt to update the client version to the correct version from the server.
				ODException.SwallowAnyException(() => {
					UpdateClientFromServerUpdateFiles(storedVersion,currentVersion);
				});
				return false;//Regardless if the client update was successful or not we HAVE to restart Open Dental so always return false.
			}
			//Give option to downgrade to server if client version > server version and both the WebServiceServerName isn't blank and the current computer ID is not the same as the WebServiceServerName
			if(storedVersion<currentVersion 
				&& updateServerName!="" 
				&& !ODEnvironment.IdIsThisComputer(updateServerName.ToLower()))
			{
				if(isSilent) {//This should have already been thrown but we do it here anyway.
					FormOpenDental.ExitCode=141;//Updates are only allowed from a designated web server
					return false;
				}
				//Offer to downgrade
				string message=Lan.g("Prefs","Your version is more recent than the server version.");
				message+="\r\n"+Lan.g("Prefs","Updates are only allowed from the web server")+": "+updateServerName;
				message+="\r\n\r\n"+Lan.g("Prefs","Do you want to downgrade to the server version?");
				if(MessageBox.Show(message,"",MessageBoxButtons.OKCancel)!=DialogResult.OK) {
					return false;//If user clicks cancel, then exit program
				}
				//At this point we know with absolute certainty that we cannot allow this version to connect to the database.
				//Attempt to downgrade the client version to the correct version from the server.
				ODException.SwallowAnyException(() => {
					UpdateClientFromServerUpdateFiles(storedVersion,currentVersion);
				});
				return false;//Regardless if the downgrade was successful or not we HAVE to restart Open Dental so always return false.
			}
			//Push update to server if client version > server version and either the WebServiceServerName is blank or the current computer ID is the same as the WebServiceServerName
			//At this point we know 100% it's going to be an upgrade
			else if(storedVersion<currentVersion 
				&& (updateServerName=="" || ODEnvironment.IdIsThisComputer(updateServerName.ToLower()))) {
#if TRIALONLY
				if(PrefC.GetString(PrefName.RegistrationKey)!="") {//Allow databases with no reg key to continue.  Needed by our conversion department.
					//Trial users should never be able to update a database, not even the ProgramVersion preference.
					MsgBox.Show("PrefL","Trial versions cannot connect to live databases.  Please run the Setup.exe in the AtoZ folder to reinstall your original version.");
					FormOpenDental.ExitCode=398;//Trial versions does not support updating databases.
					return false;//Should not get to this line.  Just in case.
				}
#endif
				//This has been commented out because it was deemed unnecessary: 10/10/14 per Jason and Derek
				//There are two different situations where this might happen.
				//if(PrefC.GetString(PrefName.UpdateInProgressOnComputerName)==""){//1. Just performed an update from this workstation on another database.
				//	//This is very common for admins when viewing slighly older databases.
				//	//There should be no annoying behavior here.  So do nothing.
				//	#if !DEBUG
				//		//Excluding this in debug allows us to view slightly older databases without accidentally altering them.
				//		Prefs.UpdateString(PrefName.ProgramVersion,currentVersion.ToString());
				//		Cache.Refresh(InvalidType.Prefs);
				//	#endif
				//	return true;
				//}
				//and 2a. Just performed an update from this workstation on this database.  
				//or 2b. Just performed an update from this workstation for multiple databases.
				//In both 2a and 2b, we already downloaded Setup file to correct location for this db, so skip 1 above.
				//This computer just performed an update, but none of the other computers has updated yet.
				//So attempt to stash all files that are in the Application directory.
				//At this point we know that we are going to perform an update.
				bool hasAtoZ=false;
				bool hasConcatFiles=false;
				//Check to see if the version we are coming from is prior to v15.3.
				//If we are coming from an older version, we need to put a copy of the Update Files into the AtoZ share for backwards compatibility.
				if(storedVersion<new Version("15.3.10")) {
					//In 15.3.10 we started to explicitly use the database for storing the Update Files folder.
					//Any clients updating from a previous version still need the Update Files in the AtoZ because they look there instead of the db.
					hasAtoZ=true;
				}
				//Check to see if the version we are coming from is prior to v15.4.50 or between v16.1.0 and v16.1.20.
				//If these scenarios are met, we need to insert the new Update Files as one big row using the old CONCAT methodology. 
				if(storedVersion < new Version("15.4.50")
					|| (storedVersion > new Version("16.1.0") && storedVersion < new Version("16.1.20"))) 
				{
					//Attempts to copy UpdateFiles into a single row in the database for backwards compatibility.
					//If copying the entire UpdateFiles zip into one row fails, the update will go on because it has been proven to be unreliable. 
					hasConcatFiles=true;
				}
				if(!CopyFromHereToUpdateFiles(currentVersion,isSilent,hasAtoZ,hasConcatFiles)) {
					Environment.Exit(FormOpenDental.ExitCode);
					return false;
				}
				Prefs.UpdateString(PrefName.ProgramVersion,currentVersion.ToString());
				UpdateHistory updateHistory=new UpdateHistory(currentVersion.ToString());
				UpdateHistories.Insert(updateHistory);
				Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName,"");//now, other workstations will be allowed to update.
				Cache.Refresh(InvalidType.Prefs);
				//Try to install Open Dental Service, since this is the computer that matches PrefName.WebServiceServerName.
				//If PrefName.WebServiceServerName is blank, this will install on the computer running the update if it isn't already installed.
				//In addition to the service installing, there will also be a new OpenDentalServiceConfig.xml file that the service will use.  
				//The config data is defaulted to the current connection settings in DataConnection. 
				TryInstallOpenDentalService(isSilent);
				bool needsEConnectorUpgrade=false;
				//Check to see if the eConnector has ever been installed.  Warn them about potential complications due to converting to eConnector.
				//This only needs to happen once due to transitioning users over to the new eConnector service.
				if(!PrefC.GetBool(PrefName.EConnectorEnabled)) {
					//This upgrade might require converting the CustListener service over to the eConnector.
					if(updateServerName=="") {
						//There isn't an "Update Server Name" set so we don't know if this is the correct computer that should be running the eConnector.
						//Check to see if it currently has the listener installed on it and if it does, upgrade it to the eConnector.
						//Otherwise, warn them that their eServices might not work.
						int countCustListeners=0;
						try {
							countCustListeners=ServicesHelper.GetServicesByExe("OpenDentalCustListener.exe").Count;
						}
						catch(Exception) {
							//Do nothing and assume no CustListeners are installed.
						}
						if(countCustListeners > 0) {
							needsEConnectorUpgrade=true;//This computer is not set as the upgrade server but is upgrading and DOES have a listener present.
						}
						else {//No listener services found on the computer doing the upgrade.
							//Warn the user that their eServices will go down if there are any entries in the eservicesignal table within the last month.
							List<EServiceSignal> listSignals=EServiceSignals.GetServiceHistory(eServiceCode.ListenerService,DateTime.Now.AddMonths(-1),DateTime.Now);
							if(listSignals.Count > 0 && !isSilent) {
								MsgBox.Show("PrefL","eServices will not work until the eConnector service is installed on the computer that is running the Listener Service.  Please contact us for help installing the eConnector service or see our online manual for more information.");
							}
						}
					}
					//The "Update Server Name" preference is set to something so check to see if this is the Update Server computer.
					if(ODEnvironment.IdIsThisComputer(updateServerName.ToLower()) || needsEConnectorUpgrade) {
						//This is the computer that the eConnector should be installed on.  Try to upgrade or install it.
						bool isListening;
						//if isSilent=false, a messagebox will be displayed if anything goes wrong.
						if(UpgradeOrInstallEConnector(isSilent,out isListening)) {
							Prefs.UpdateBool(PrefName.EConnectorEnabled,true);//No need to send an invalidate signal to other workstations.  They were kicked out.
							try {
								ListenerServiceType listenerType=WebServiceMainHQProxy.SetEConnectorOn();
								string logText=Lan.g("PrefL","eConnector status automatically set to")+" "+listenerType.ToString()+" "
									+Lan.g("PrefL","via the upgrade process.");
								SecurityLogs.MakeLogEntry(Permissions.EServicesSetup,0,logText);
							}
							catch(Exception) {
								if(!isSilent) {
									//Notify the user that HQ was not updated regarding the status of the eConnector (important).
									MsgBox.Show("PrefL","Could not update the eConnector communication status.  Please contact us to enable eServices.");
								}
							}
						}
						else {//Upgrading to eConnector failed.
							//Purposefully do not fail the upgrade if automatically upgrading to the eConnector failed.
							//The user will call us up when their eServices are no longer working and we will be able to assist them in installing the new service.
							//NEVER update the EConnectorEnabled preference to false.  There is no such thing.  It is used as a one time flag.
						}
					}
				}
			}
			return true;
#endif
		}

		///<summary>If AtoZ.manifest was wrong, or if user is not using AtoZ, then just download again.  Will use dir selected by user.  If an appropriate download is not available, it will fail and inform user.</summary>
		private static void DownloadAndRunSetup(Version storedVersion,Version currentVersion) {
			string patchName="Setup.exe";
			string updateUri=PrefC.GetString(PrefName.UpdateWebsitePath);
			string updateCode=PrefC.GetString(PrefName.UpdateCode);
			string updateInfoMajor="";
			string updateInfoMinor="";
			if(!FormUpdate.ShouldDownloadUpdate(updateUri,updateCode,out updateInfoMajor,out updateInfoMinor)){
				return;
			}
			if(MessageBox.Show(
				Lan.g("Prefs","Setup file will now be downloaded.")+"\r\n"
				+Lan.g("Prefs","Workstation version will be updated from ")+currentVersion.ToString(3)
				+Lan.g("Prefs"," to ")+storedVersion.ToString(3),
				"",MessageBoxButtons.OKCancel)
				!=DialogResult.OK)//they don't want to update for some reason.
			{
				return;
			}
			FolderBrowserDialog dlg=new FolderBrowserDialog();
			dlg.SelectedPath=ImageStore.GetPreferredAtoZpath();
			dlg.Description=Lan.g("Prefs","Setup.exe will be downloaded to the folder you select below");
			if(dlg.ShowDialog()!=DialogResult.OK) {
				return;//app will exit
			}
			string tempFile=ODFileUtils.CombinePaths(dlg.SelectedPath,patchName);
			//ODFileUtils.CombinePaths(GetTempFolderPath(),patchName);
			FormUpdate.DownloadInstallPatchFromURI(updateUri+updateCode+"/"+patchName,//Source URI
				tempFile,true,false,null);//Local destination file.
			if(File.Exists(tempFile)) {//If user canceld in DownloadInstallPatchFromURI file will not exist.
				File.Delete(tempFile);//Cleanup install file.
			}
		}

				///<summary>This ONLY runs when first opening the program.  Gets run early in the sequence. Returns false if the program should exit.</summary>
		public static bool CheckMySqlVersion() {
			return CheckMySqlVersion(false);
		}

		///<summary>This ONLY runs when first opening the program.  Gets run early in the sequence. Returns false if the program should exit.</summary>
		public static bool CheckMySqlVersion(bool isSilent) {
			if(DataConnection.DBtype!=DatabaseType.MySql) {
				return true;
			}
			bool hasBackup=false;
			string thisVersion=MiscData.GetMySqlVersion();
			Version versionMySQL=new Version(thisVersion);
			if(versionMySQL < new Version(5,0)) {
				FormOpenDental.ExitCode=110;//MySQL version lower than 5.0
				if(!isSilent) {
					//We will force users to upgrade to 5.0, but not yet to 5.5
					MessageBox.Show(Lan.g("Prefs","Your version of MySQL won't work with this program")+": "+thisVersion
						+".  "+Lan.g("Prefs","You should upgrade to MySQL 5.0 using the installer on our website."));
				}
				return false;
			}
			if(!PrefC.ContainsKey("MySqlVersion")) {//db has not yet been updated to store this pref
				//We're going to skip this.  We will recommend that people first upgrade OD, then MySQL, so this won't be an issue.
			}
			else {//Using a version that stores the MySQL version as a preference.
				//There was an old bug where the MySQLVersion preference could be stored as 5,5 instead of 5.5 due to converting the version into a float.
				//Replace any commas with periods before checking if the preference is going to change.
				//This is simply an attempt to avoid making unnecessary backups for users with a corrupt version (e.g. 5,5).
				if(PrefC.GetString(PrefName.MySqlVersion).Contains(",")) {
					Prefs.UpdateString(PrefName.MySqlVersion,PrefC.GetString(PrefName.MySqlVersion).Replace(",","."));
				}
				//Now check to see if the MySQL version has been updated.  If it has, make an automatic backup, repair, and optimize all tables.
				if(Prefs.UpdateString(PrefName.MySqlVersion,(thisVersion))) {
					if(!isSilent) {
						if(!MsgBox.Show("Prefs",MsgBoxButtons.OKCancel,"Tables will now be backed up, optimized, and repaired.  This will take a minute or two.  Continue?")) {
							FormOpenDental.ExitCode=0;
							return false;
						}
					}
					if(!Shared.BackupRepairAndOptimize(isSilent,BackupLocation.ConvertScript,false)) {
						FormOpenDental.ExitCode=101;//Database Backup failed
						return false;
					}
					hasBackup=true;
				}
			}
			if(PrefC.ContainsKey("DatabaseConvertedForMySql41")) {
				return true;//already converted
			}
			if(!isSilent) {
				if(!MsgBox.Show("Prefs",true,"Your database will now be converted for use with MySQL 4.1.")) {
					FormOpenDental.ExitCode=0;
					return false;
				}
			}
			//ClassConvertDatabase CCD=new ClassConvertDatabase();
			if(!hasBackup) {//A backup could have been made if the tables were optimized and repaired above.
				if(!Shared.MakeABackup(isSilent,BackupLocation.ConvertScript,false)) {
					FormOpenDental.ExitCode=101;//Database Backup failed
					return false;//but this should never happen
				}
			}
			if(!isSilent) {
				MsgBox.Show("Prefs","Backup performed");
			}
			Prefs.ConvertToMySqlVersion41();
			if(!isSilent) {
				MsgBox.Show("Prefs","Converted");
			}
			return true;
		}

		///<summary>This runs when first opening the program.  If MySql is not at 5.5 or higher, it reminds the user, but does not force them to upgrade.</summary>
		public static void MySqlVersion55Remind(){
			if(DataConnection.DBtype!=DatabaseType.MySql) {
				return;
			}
			string thisVersion=MiscData.GetMySqlVersion();
			Version versionMySQL=new Version(thisVersion);
			if(versionMySQL < new Version(5,5) && !Programs.IsEnabled(ProgramName.eClinicalWorks)) {//Do not show msg if MySQL version is 5.5 or greater or eCW is enabled
				MsgBox.Show("Prefs","You should upgrade to MySQL 5.5 using the installer posted on our website.  It's not urgent, but until you upgrade, you are likely to get a few errors each day which will require restarting the MySQL service.");
			}
		}

		///<summary></summary>
		private static bool CheckProgramVersionClassic() {
			Version storedVersion=new Version(PrefC.GetString(PrefName.ProgramVersion));
			Version currentVersion=new Version(Application.ProductVersion);
			string database=MiscData.GetCurrentDatabase();
			if(storedVersion<currentVersion) {
				Prefs.UpdateString(PrefName.ProgramVersion,currentVersion.ToString());
				UpdateHistory updateHistory=new UpdateHistory(currentVersion.ToString());
				UpdateHistories.Insert(updateHistory);
				Cache.Refresh(InvalidType.Prefs);
			}
			if(storedVersion>currentVersion) {
				if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ) {
					string setupBinPath=ODFileUtils.CombinePaths(ImageStore.GetPreferredAtoZpath(),"Setup.exe");
					if(File.Exists(setupBinPath)) {
						if(MessageBox.Show("You are attempting to run version "+currentVersion.ToString(3)+",\r\n"
							+"But the database "+database+"\r\n"
							+"is already using version "+storedVersion.ToString(3)+".\r\n"
							+"A newer version must have already been installed on at least one computer.\r\n"  
							+"The setup program stored in your A to Z folder will now be launched.\r\n"
							+"Or, if you hit Cancel, then you will have the option to download again."
							,"",MessageBoxButtons.OKCancel)==DialogResult.Cancel) {
							if(MessageBox.Show("Download again?","",MessageBoxButtons.OKCancel)
								==DialogResult.OK) {
								FormUpdate FormU=new FormUpdate();
								FormU.ShowDialog();
							}
							Application.Exit();
							return false;
						}
						try {
							Process.Start(setupBinPath);
						}
						catch {
							MessageBox.Show("Could not launch Setup.exe");
						}
					}
					else if(MessageBox.Show("A newer version has been installed on at least one computer,"+
							"but Setup.exe could not be found in any of the following paths: "+
							ImageStore.GetPreferredAtoZpath()+".  Download again?","",MessageBoxButtons.OKCancel)==DialogResult.OK) {
						FormUpdate FormU=new FormUpdate();
						FormU.ShowDialog();
					}
				}
				else {//Not using image path.
					//perform program update automatically.
					string patchName="Setup.exe";
					string updateUri=PrefC.GetString(PrefName.UpdateWebsitePath);
					string updateCode=PrefC.GetString(PrefName.UpdateCode);
					string updateInfoMajor="";
					string updateInfoMinor="";
					if(FormUpdate.ShouldDownloadUpdate(updateUri,updateCode,out updateInfoMajor,out updateInfoMinor)) {
						if(MessageBox.Show(updateInfoMajor+Lan.g("Prefs","Perform program update now?"),"",
							MessageBoxButtons.YesNo)==DialogResult.Yes) {
							string tempFile=ODFileUtils.CombinePaths(PrefC.GetTempFolderPath(),patchName);//Resort to a more common temp file name.
							FormUpdate.DownloadInstallPatchFromURI(updateUri+updateCode+"/"+patchName,//Source URI
								tempFile,true,true,null);//Local destination file.
							if(File.Exists(tempFile)) {//If user canceld in DownloadInstallPatchFromURI file will not exist.
								File.Delete(tempFile);//Cleanup install file.
							}
						}
					}
				}
				Application.Exit();//always exits, whether launch of setup worked or not
				return false;
			}
			return true;
		}

		///<summary>Checks to see any OpenDentalCustListener services are currently installed.
		///If present, each CustListener service will be uninstalled.
		///After successfully removing all CustListener services, one eConnector service will be installed.
		///Returns true if the CustListener service was successfully upgraded to the eConnector service.</summary>
		///<param name="isSilent">Set to false to show meaningful error messages, otherwise fails silently.</param>
		///<param name="isListening">Will get set to true if the customer was previously using the CustListener service.</param>
		///<returns>True if only one CustListener services present and was successfully uninstalled along with the eConnector service getting installed.
		///False if more than one CustListener service is present or the eConnector service could not install.</returns>
		public static bool UpgradeOrInstallEConnector(bool isSilent,out bool isListening) {
			isListening=false;
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				if(!isSilent) {
					MsgBox.Show("ServicesHelper","Not allowed to install services when using the middle tier.");
				}
				return false;
			}
			try {
				//Check to see if CustListener service is installed and needs to be uninstalled.
				List<ServiceController> listCustListenerServices=ServicesHelper.GetServicesByExe("OpenDentalCustListener.exe");
				if(listCustListenerServices.Count>0) {
					isListening=true;
				}
				if(listCustListenerServices.Count==1) {//Only uninstall the listener service if there is exactly one found.  This is just a nicety.
					ServicesHelper.Uninstall(listCustListenerServices[0]);
				}
				List<ServiceController> listEConnectorServices=ServicesHelper.GetServicesByExe("OpenDentalEConnector.exe");
				if(listEConnectorServices.Count>0) {
					return true;//An eConnector service is already installed.
				}
				string eConnectorExePath=ODFileUtils.CombinePaths(Directory.GetCurrentDirectory(),"OpenDentalEConnector","OpenDentalEConnector.exe");
				FileInfo eConnectorExeFI=new FileInfo(eConnectorExePath);
				if(!ServicesHelper.Install("OpenDentalEConnector",eConnectorExeFI)) {
					if(!isSilent) {
						throw new ApplicationException(Lans.g("ServicesHelper","Unable to install the OpenDentalEConnector service."));
					}
					return false;
				}
				//Create a new OpenDentalWebConfig.xml file for the eConnector if one is not already present.
				if(!CreateConfigForEConnector(isListening)) {
					if(!isSilent) {
						throw new ApplicationException(Lans.g("ServicesHelper","The config file for the OpenDentalEConnector service could not be created."));
					}
					return false;
				}
				//Now that the service has finally installed we need to try and start it.
				listEConnectorServices=ServicesHelper.GetServicesByExe("OpenDentalEConnector.exe");
				if(listEConnectorServices.Count<1) {
					if(!isSilent) {
						throw new ApplicationException(Lans.g("ServicesHelper","OpenDentalEConnector service could not be found in order to automatically start it."));
					}
					return false;
				}
				string eConnectorStartingErrors=ServicesHelper.StartServices(listEConnectorServices);
				if(!string.IsNullOrEmpty(eConnectorStartingErrors)) {
					if(!isSilent) {
						throw new ApplicationException(Lans.g("ServicesHelper","Unable to start the following eConnector services:")+"\r\n"+eConnectorStartingErrors);
					}
					return false;
				}
				return true;
			}
			catch(Exception ex) {
				if(!isSilent) {
					MessageBox.Show(Lans.g("ServicesHelper","Failed upgrading to the eConnector service:")+"\r\n"+ex.Message
						+"\r\n\r\n"+Lans.g("ServicesHelper","Try running as Administrator."));
				}
				return false;
			}
		}

		///<summary>Tries to install the OpenDentalService if needed.  Returns false if failed.
		///Set isSilent to false to show meaningful error messages, otherwise fails silently.</summary>
		public static bool TryInstallOpenDentalService(bool isSilent) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				if(!isSilent) {
					MsgBox.Show("ServicesHelper","Not allowed to install services when using the middle tier.");
				}
				return false;
			}
			try {
				List<ServiceController> listOpenDentalServices=ServicesHelper.GetServicesByExe("OpenDentalService.exe");
				if(listOpenDentalServices.Count>0) {
					return true;//An Open Dental Service is already installed.
				}
				string odServiceFilePath=ODFileUtils.CombinePaths(Directory.GetCurrentDirectory(),"OpenDentalService","OpenDentalService.exe");
				FileInfo odServiceExeFI=new FileInfo(odServiceFilePath);
				if(!ServicesHelper.Install("OpenDentalService",odServiceExeFI)) {
					AlertItems.CreateGenericAlert(Lans.g("ServicesHelper","Open Dental Service Error"),Lans.g("ServicesHelper","Failed to install OpenDentalService, try running as admin."));
					return false;
				}
				//Create a new OpenDentalServiceConfig.xml file for Open Dental Service if one is not already present.
				if(!CreateConfigForOpenDentalService()) {
					AlertItems.CreateGenericAlert(Lans.g("ServicesHelper","Open Dental Service Error"),Lans.g("ServicesHelper","Failed to create OpenDentalServiceConfig.xml file."));
					return false;
				}
				//Now that the service has finally installed we need to try and start it.
				listOpenDentalServices=ServicesHelper.GetServicesByExe("OpenDentalService.exe");
				if(listOpenDentalServices.Count<1) {
					AlertItems.CreateGenericAlert(Lans.g("ServicesHelper","Open Dental Service Error"),Lans.g("ServicesHelper","OpenDental Service could not be found."));
					return false;
				}
				string openDentalServiceStartingErrors=ServicesHelper.StartServices(listOpenDentalServices);
				if(!string.IsNullOrEmpty(openDentalServiceStartingErrors)) {
					AlertItems.CreateGenericAlert(Lans.g("ServicesHelper","Open Dental Service Error"),Lans.g("ServicesHelper","The following service(s) could not start:")+" "+openDentalServiceStartingErrors);
					return false;
				}
				return true;
			}
			catch(Exception e) {
				AlertItems.CreateGenericAlert(Lans.g("ServicesHelper","Open Dental Service Error"),Lans.g("ServicesHelper","Unknown exception:")+" "+e.Message);
				return false;
			}
		}

		///<summary>Creates a default OpenDentalWebConfig.xml file for the eConnector if one is not already present.
		///Uses the current connection settings in DataConnection.  This method does NOT work if called via middle tier.
		///Users should not be installing the eConnector via the middle tier.</summary>
		private static bool CreateConfigForEConnector(bool isListening) {
			string eConnectorConfigPath=ODFileUtils.CombinePaths(Directory.GetCurrentDirectory(),"OpenDentalEConnector","OpenDentalWebConfig.xml");
			string custListenerConfigPath=ODFileUtils.CombinePaths(Directory.GetCurrentDirectory(),"OpenDentalCustListener","OpenDentalWebConfig.xml");
			//Check to see if there is already a config file present.
			if(File.Exists(eConnectorConfigPath)) {
				return true;//Nothing to do.
			}
			//At this point we know that the eConnector does not have a config file present.
			//Check to see if the user is currently using the CustListener service.
			if(isListening) {
				//Try and grab a copy of the CustListener service config file first.
				if(File.Exists(custListenerConfigPath)) {
					try {
						File.Copy(custListenerConfigPath,"",false);
						//If we got to this point the copy was successful and now the eConnector has a valid config file.
						return true;
					}
					catch(Exception) {
						//The copy didn't work for some reason.  Simply try to create a new file in the eConnector directory.
					}
				}
			}
			string mySqlPassHash;
			CDT.Class1.Encrypt(DataConnection.GetMysqlPass(),out mySqlPassHash);
			return ServicesHelper.CreateServiceConfigFile(eConnectorConfigPath
				,DataConnection.GetServerName()
				,DataConnection.GetDatabaseName()
				,DataConnection.GetMysqlUser()
				,DataConnection.GetMysqlPass()
				,mySqlPassHash
				,DataConnection.GetMysqlUserLow()
				,DataConnection.GetMysqlPassLow());
		}

		///<summary>Creates a default OpenDentalServiceConfig.xml file for Open Dental Service if one is not already present.
		///Uses the current connection settings in DataConnection.  This method does NOT work if called via middle tier.
		///Users should not be installing Open Dental Service via the middle tier.</summary>
		public static bool CreateConfigForOpenDentalService() {
			string odServiceConfigPath=ODFileUtils.CombinePaths(Directory.GetCurrentDirectory(),"OpenDentalService","OpenDentalServiceConfig.xml");
			//Check to see if there is already a config file present.
			if(File.Exists(odServiceConfigPath)) {
				return true;//Nothing to do.
			}
			//At this point we know that Open Dental Service does not have a config file present.
			string mySqlPassHash;
			CDT.Class1.Encrypt(DataConnection.GetMysqlPass(),out mySqlPassHash);
			return ServicesHelper.CreateServiceConfigFile(odServiceConfigPath
				,DataConnection.GetServerName()
				,DataConnection.GetDatabaseName()
				,DataConnection.GetMysqlUser()
				,DataConnection.GetMysqlPass()
				,mySqlPassHash
				,DataConnection.GetMysqlUserLow()
				,DataConnection.GetMysqlPassLow());
		}

		///<summary>Performs both upgrades and downgrades by recopying update files from DB to temp folder, then from temp folder to local program path.
		///This is the update sequence for both a direct workstation, and for a ClientWeb workstation.</summary>
		private static bool UpdateClientFromServerUpdateFiles(Version storedVersion, Version currentVersion) {
			string folderUpdate=ODFileUtils.CombinePaths(PrefC.GetTempFolderPath(),"UpdateFiles");
			if(Directory.Exists(folderUpdate)) {
				try {
					Directory.Delete(folderUpdate,true);
				}
				catch(Exception ex) {
					FriendlyException.Show(Lan.g("Prefs","Unable to delete update files from local temp folder. Try closing and reopening the program."),ex);
					FormOpenDental.ExitCode=301;//UpdateFiles folder cannot be deleted
					Environment.Exit(FormOpenDental.ExitCode);
					return false;
				}
			}
			StringBuilder strBuilder=new StringBuilder();
			DocumentMisc docUpdateFilesPart=null;
			int count=1;
			string fileName=count.ToString().PadLeft(4,'0');
			while((docUpdateFilesPart=DocumentMiscs.GetByTypeAndFileName(fileName,DocumentMiscType.UpdateFilesSegment))!=null) {
				strBuilder.Append(docUpdateFilesPart.RawBase64);
				count++;
				fileName=count.ToString().PadLeft(4,'0');
			}
			try {
				//strBuilder.ToString() has a tendency to fail when the string contains roughly 170MB of data.
				//If that becomes a typical size for our Update Files folder we should consider not storing the data as Base64.
				byte[] rawBytes=Convert.FromBase64String(strBuilder.ToString());
				using(ZipFile unzipped=ZipFile.Read(rawBytes)) {
					unzipped.ExtractAll(folderUpdate);
				}
			}
			catch(Exception) {
				//fail silently
			}
			//look at the manifest to see if it's the version we need
			string manifestVersion="";
			try {
				manifestVersion=File.ReadAllText(ODFileUtils.CombinePaths(folderUpdate,"Manifest.txt"));
			}
			catch {
				//fail silently
			}
			if(manifestVersion!=storedVersion.ToString(3)) {//manifest version is wrong
				//No point trying the Setup.exe because that's probably wrong too.
				//Just go straight to downloading and running the Setup.exe.
				string manpath=ODFileUtils.CombinePaths(folderUpdate,"Manifest.txt");
				if(MessageBox.Show(Lan.g("Prefs","The expected version information was not found in this file: ")+manpath+".  "
					+Lan.g("Prefs","There is probably a permission issue on that folder which should be fixed. ")
					+"\r\n\r\n"+Lan.g("Prefs","The suggested solution is to return to the computer where the update was just run.  Go to Help | Update | Setup, and click the Recopy button.")
					+"\r\n\r\n"+Lan.g("Prefs","If, instead, you click OK in this window, then a fresh Setup file will be downloaded and run."),						
					"",MessageBoxButtons.OKCancel)!=DialogResult.OK)//they don't want to download again.
				{
					FormOpenDental.ExitCode=312;//Stored version is higher that client version after an update was successful.
					Environment.Exit(FormOpenDental.ExitCode);
					return false;
				}
				DownloadAndRunSetup(storedVersion,currentVersion);
				Environment.Exit(0);
				return false;
			}
			//manifest version matches
			if(MessageBox.Show(Lan.g("Prefs","Files will now be copied.")+"\r\n"
				+Lan.g("Prefs","Workstation version will be updated from ")+currentVersion.ToString(3)
				+Lan.g("Prefs"," to ")+storedVersion.ToString(3),
				"",MessageBoxButtons.OKCancel)
				!=DialogResult.OK)//they don't want to update for some reason.
			{
				Environment.Exit(0);
				return false;
			}
			string tempDir=PrefC.GetTempFolderPath();
			//copy UpdateFileCopier.exe to the temp directory
			File.Copy(ODFileUtils.CombinePaths(folderUpdate,"UpdateFileCopier.exe"),//source
				ODFileUtils.CombinePaths(tempDir,"UpdateFileCopier.exe"),//dest
				true);//overwrite
			//wait a moment to make sure the file was copied
			Thread.Sleep(500);
			//launch UpdateFileCopier to copy all files to here.
			int processId=Process.GetCurrentProcess().Id;
			string appDir=Application.StartupPath;
			string startFileName=ODFileUtils.CombinePaths(tempDir,"UpdateFileCopier.exe");
			string arguments="\""+folderUpdate+"\""//pass the source directory to the file copier.
				+" "+processId.ToString()//and the processId of Open Dental.
				+" \""+appDir+"\"";//and the directory where OD is running
			try {
				Process.Start(startFileName,arguments);
			}
			catch(Exception ex) {
				FriendlyException.Show(Lan.g("Prefs","Unable to start the update file copier. Try closing and reopening the program."),ex);
				FormOpenDental.ExitCode=305;//Unable to start the UpdateFileCopier.exe process.
				Environment.Exit(FormOpenDental.ExitCode);
				return false;
			}	
			Environment.Exit(0);//always exits, whether launch of setup worked or not
			return false;
		}

		/// <summary>Check for a developer only license</summary>
		public static bool IsRegKeyForTesting() {
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = ("    ");
			StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer=XmlWriter.Create(strbuild,settings)){
				writer.WriteStartElement("RegistrationKey");
				writer.WriteString(PrefC.GetString(PrefName.RegistrationKey));			
				writer.WriteEndElement();
			}
			try {
				string response=CustomerUpdatesProxy.GetWebServiceInstance().RequestIsDevKey(strbuild.ToString());
				XmlDocument doc=new XmlDocument();
				doc.LoadXml(response);
				XmlNode node=doc.SelectSingleNode("//IsDevKey");
				return PIn.Bool(node.InnerText);
			}
			catch(Exception ex) {
				//They don't have an external internet connection.
				ex.DoNothing();
				return false;
			}
		}
	}
}
