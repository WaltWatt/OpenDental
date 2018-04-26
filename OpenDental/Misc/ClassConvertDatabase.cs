using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Design;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OpenDental{

	///<summary></summary>
	public partial class ClassConvertDatabase{
		private System.Version FromVersion;
		private System.Version ToVersion;
		private static Version _latestVersion;
		private static List<ConvertDatabasesMethodInfo> _listConvertMethods;
		///<summary>This is the regular expression pattern used to match our convert databases method version pattern of "ToX_X_X".</summary>
		public const string PatternMethodInfo=@"^To([0-9]+)_([0-9]+)_([0-9]+)$";

		///<summary>Gets a list of convert databases method infos and their corresponding version information based on their method name.</summary>
		public static List<ConvertDatabasesMethodInfo> ListConvertMethods {
			get {
				if(_listConvertMethods==null) {
					_listConvertMethods=GetAllVersions();
				}
				return _listConvertMethods;
			}
		}
		
		///<summary>Returns a version object that correlates to the last convert databases method on file.</summary>
		public static Version LatestVersion {
			get {
				if(_latestVersion==null) {
					_latestVersion=ListConvertMethods[ListConvertMethods.Count-1].VersionCur;
				}
				return _latestVersion;
			}
		}

		///<summary>Return false to indicate exit app.  Only called when program first starts up at the beginning of FormOpenDental.PrefsStartup.</summary>
		public bool Convert(string fromVersion,string toVersion,bool isSilent,Form currentForm=null) {
			FromVersion=new Version(fromVersion);
			ToVersion=new Version(toVersion);//Application.ProductVersion);
			if(FromVersion>=new Version("3.4.0") && PrefC.GetBool(PrefName.CorruptedDatabase)) {
				FormOpenDental.ExitCode=201;//Database was corrupted due to an update failure
				if(!isSilent) {
					MsgBox.Show(this,"Your database is corrupted because an update failed.  Please contact us.  This database is unusable and you will need to restore from a backup.");
				}
				return false;//shuts program down.
			}
			if(FromVersion==ToVersion) {
				return true;//no conversion necessary
			}
			if(FromVersion.CompareTo(ToVersion)>0){//"Cannot convert database to an older version."
				//no longer necessary to catch it here.  It will be handled soon enough in CheckProgramVersion
				return true;
			}
			if(FromVersion < new Version("2.8.0")) {
				FormOpenDental.ExitCode=130;//Database must be upgraded to 2.8 to continue
				if(!isSilent) {
					MsgBox.Show(this,"This database is too old to easily convert in one step. Please upgrade to 2.1 if necessary, then to 2.8.  Then you will be able to upgrade to this version. We apologize for the inconvenience.");
				}
				return false;
			}
			if(FromVersion < new Version("6.6.2")) {
				FormOpenDental.ExitCode=131;//Database must be upgraded to 11.1 to continue
				if(!isSilent) {
					MsgBox.Show(this,"This database is too old to easily convert in one step. Please upgrade to 11.1 first.  Then you will be able to upgrade to this version. We apologize for the inconvenience.");
				}
				return false;
			}
			if(FromVersion < new Version("3.0.1")) {
				if(!isSilent) {
					MsgBox.Show(this,"This is an old database.  The conversion must be done using MySQL 4.1 (not MySQL 5.0) or it will fail.");
				}
			}
			if(FromVersion.ToString()=="2.9.0.0" || FromVersion.ToString()=="3.0.0.0" || FromVersion.ToString()=="4.7.0.0") {
				FormOpenDental.ExitCode=190;//Cannot convert this database version which was only for development purposes
				if(!isSilent) {
					MsgBox.Show(this,"Cannot convert this database version which was only for development purposes.");
				}
				return false;
			}
			if(FromVersion > new Version("4.7.0") && FromVersion.Build==0) {
				FormOpenDental.ExitCode=190;//Cannot convert this database version which was only for development purposes
				if(!isSilent) {
					MsgBox.Show(this,"Cannot convert this database version which was only for development purposes.");
				}
				return false;
			}
			if(FromVersion >= LatestVersion) {
				return true;//no conversion necessary
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				FormOpenDental.ExitCode=140;//Web client cannot convert database
				if(!isSilent) {
					MsgBox.Show(this,"Web client cannot convert database.  Must be using a direct connection.");
				}
				return false;
			}
			if(ReplicationServers.ServerIsBlocked()) {
				FormOpenDental.ExitCode=150;//Replication server is blocked from performing updates
				if(!isSilent) {
					MsgBox.Show(this,"This replication server is blocked from performing updates.");
				}
				return false;
			}
#if TRIALONLY
			//Trial users should never be able to update a database.
			if(PrefC.GetString(PrefName.RegistrationKey)!="") {//Allow databases with no reg key to update.  Needed by our conversion department.
				FormOpenDental.ExitCode=191;//Trial versions cannot connect to live databases
				if(!isSilent) {
					MsgBox.Show(this,"Trial versions cannot connect to live databases.  Please run the Setup.exe in the AtoZ folder to reinstall your original version.");
				}
				return false;
			}
#endif
			if(PrefC.GetString(PrefName.WebServiceServerName)!="" //using web service
				&& !ODEnvironment.IdIsThisComputer(PrefC.GetString(PrefName.WebServiceServerName).ToLower()))//and not on web server 
			{
				if(isSilent) {
					FormOpenDental.ExitCode=141;//Updates are only allowed from a designated web server
					return false;//if you are in debug mode and you really need to update the DB, you can manually clear the WebServiceServerName preference.
				}
				//This will be handled in CheckProgramVersion, giving the user option to downgrade or exit program.
				return true;
			}
			//If MyISAM and InnoDb mix, then try to fix
			if(DataConnection.DBtype==DatabaseType.MySql) {//not for Oracle
				string namesInnodb=InnoDb.GetInnodbTableNames();//Or possibly some other format.
				int numMyisam=DatabaseMaintenances.GetMyisamTableCount();
				if(namesInnodb!="" && numMyisam>0) {
					if(!isSilent) {
						MessageBox.Show(Lan.g(this,"A mixture of database tables in InnoDB and MyISAM format were found.  A database backup will now be made, and then the following InnoDB tables will be converted to MyISAM format: ")+namesInnodb);
					}
					if(!Shared.MakeABackup(isSilent,BackupLocation.ConvertScript,false)) {
						Cursor.Current=Cursors.Default;
						FormOpenDental.ExitCode=101;//Database Backup failed
						return false;
					}
					if(!DatabaseMaintenances.ConvertTablesToMyisam()) {
						FormOpenDental.ExitCode=102;//Failed to convert InnoDB tables to MyISAM format
						if(!isSilent) {
							MessageBox.Show(Lan.g(this,"Failed to convert InnoDB tables to MyISAM format. Please contact support."));
						}
						return false;
					}
					if(!isSilent) {
						MessageBox.Show(Lan.g(this,"All tables converted to MyISAM format successfully."));
					}
					namesInnodb="";
				}
				if(namesInnodb=="" && numMyisam>0) {//if all tables are myisam
					//but default storage engine is innodb, then kick them out.
					if(DatabaseMaintenances.GetStorageEngineDefaultName().ToUpper()!="MYISAM") { //Probably InnoDB but could be another format.
						FormOpenDental.ExitCode=103;//Default database .ini setting is innoDB
						if(!isSilent) {
							MessageBox.Show(Lan.g(this,"The database tables are in MyISAM format, but the default database engine format is InnoDB. You must change the default storage engine within the my.ini (or my.cnf) file on the database server and restart MySQL in order to fix this problem. Exiting."));
						}
						return false;
					}
				}
			}
#if DEBUG
			if(!isSilent && MessageBox.Show("You are in Debug mode.  Your database can now be converted"+"\r"
				+"from version"+" "+FromVersion.ToString()+"\r"
				+"to version"+" "+ToVersion.ToString()+"\r"
				+"You can click Cancel to skip conversion and attempt to run the newer code against the older database."
				,"",MessageBoxButtons.OKCancel)!=DialogResult.OK)
			{
				return true;//If user clicks cancel, then do nothing
			}
#else
			if(!isSilent && MessageBox.Show(Lan.g(this,"Your database will now be converted")+"\r"
				+Lan.g(this,"from version")+" "+FromVersion.ToString()+"\r"
				+Lan.g(this,"to version")+" "+ToVersion.ToString()+"\r"
				+Lan.g(this,"The conversion works best if you are on the server.  Depending on the speed of your computer, it can be as fast as a few seconds, or it can take as long as 10 minutes.")
				,"",MessageBoxButtons.OKCancel)!=DialogResult.OK)
			{
				return false;//If user clicks cancel, then close the program
			}
#endif
			Cursor.Current=Cursors.WaitCursor;
			Action actionCloseConvertProgress=null;
#if !DEBUG
			if(!isSilent) {
				if(DataConnection.DBtype!=DatabaseType.MySql
				&& !MsgBox.Show(this,true,"If you have not made a backup, please Cancel and backup before continuing.  Continue?")) {
					return false;
				}
			}
			if(DataConnection.DBtype==DatabaseType.MySql) {
				if(!Shared.MakeABackup(isSilent,BackupLocation.ConvertScript,false)) {
					Cursor.Current=Cursors.Default;
					FormOpenDental.ExitCode=101;//Database Backup failed
					return false;
				}
			}
			//We've been getting an increasing number of phone calls with databases that have duplicate preferences which is impossible
			//unless a user has gotten this far and another computer in the office is in the middle of an update as well.
			//The issue is most likely due to the blocking messageboxes above which wait indefinitely for user input right before upgrading the database.
			//This means that the cache for this computer could be stale and we need to manually refresh our cache to double check 
			//that the database isn't flagged as corrupt, an update isn't in progress, or that the database version hasn't changed (someone successfully updated already).
			Prefs.RefreshCache();
			//Now check the preferences that should stop this computer from executing an update.
			if(PrefC.GetBool(PrefName.CorruptedDatabase) 
				|| (PrefC.GetString(PrefName.UpdateInProgressOnComputerName)!="" && PrefC.GetString(PrefName.UpdateInProgressOnComputerName)!=Environment.MachineName))
			{
				//At this point, the pref "corrupted database" being true means that a computer is in the middle of running the upgrade script.
				//There will be another corrupted database check on start up which will take care of the scenario where this is truly a corrupted database.
				//Also, we need to make sure that the update in progress preference is set to this computer because we JUST set it to that value before entering this method.
				//If it has changed, we absolutely know without a doubt that another computer is trying to update at the same time.
				FormOpenDental.ExitCode=142;//Update is already in progress from another computer
				if(!isSilent) {
					MsgBox.Show(this,"An update is already in progress from another computer.");
				}
				return false;
			}
			//Double check that the database version has not changed.  This check is here just in case another computer has successfully updated the database already.
			Version versionDatabase=new Version(PrefC.GetString(PrefName.DataBaseVersion));
			if(FromVersion!=versionDatabase) {
				FormOpenDental.ExitCode=143;//Database has already been updated from another computer
				if(!isSilent) {
					MsgBox.Show(this,"The database has already been updated from another computer.");
				}
				return false;
			}
			try {
#endif
				if(FromVersion < new Version("7.5.17")) {//Insurance Plan schema conversion
					if(isSilent) {
						FormOpenDental.ExitCode=139;//Update must be done manually to fix Insurance Plan Schema
						Application.Exit();
						return false;
					}
					Cursor.Current=Cursors.Default;
					YN InsPlanConverstion_7_5_17_AutoMergeYN=YN.Unknown;
					if(FromVersion < new Version("7.5.1")) {
						FormInsPlanConvert_7_5_17 form=new FormInsPlanConvert_7_5_17();
						if(PrefC.GetBoolSilent(PrefName.InsurancePlansShared,true)) {
							form.InsPlanConverstion_7_5_17_AutoMergeYN=YN.Yes;
						}
						else {
							form.InsPlanConverstion_7_5_17_AutoMergeYN=YN.No;
						}
						form.ShowDialog();
						if(form.DialogResult==DialogResult.Cancel) {
							MessageBox.Show("Your database has not been altered.");
							return false;
						}
						InsPlanConverstion_7_5_17_AutoMergeYN=form.InsPlanConverstion_7_5_17_AutoMergeYN;
					}
					ConvertDatabases.Set_7_5_17_AutoMerge(InsPlanConverstion_7_5_17_AutoMergeYN);//does nothing if this pref is already present for some reason.
					Cursor.Current=Cursors.WaitCursor;
				}
				if(!isSilent && FromVersion>new Version("16.3.0") && FromVersion<new Version("16.3.29") && ApptReminderRules.IsReminders) {
					//16.3.29 is more strict about reminder rule setup. Prompt the user and allow them to exit the update if desired.
					//Get all currently enabled reminder rules.
					List<bool> listReminderFlags=ApptReminderRules.Get_16_3_29_ConversionFlags();
					if(listReminderFlags?[0]??false) { //2 reminders scheduled for same day of appointment. 1 will be converted to future day reminder.
						MsgBox.Show(this,"You have multiple appointment reminders set to send on the same day of the appointment. One of these will be converted to send 1 day prior to the appointment.  Please review automated reminder rule setup after update has finished.");
					}
					if(listReminderFlags?[1]??false) { //2 reminders scheduled for future day of appointment. 1 will be converted to same day reminder.
						MsgBox.Show(this,"You have multiple appointment reminders set to send 1 or more days prior to the day of the appointment. One of these will be converted to send 1 hour prior to the appointment.  Please review automated reminder rule setup after update has finished.");
					}
				}
				if(FromVersion>=new Version("17.3.1") && FromVersion<new Version("17.3.23") && DataConnection.DBtype==DatabaseType.MySql
					&& (Tasks.HasAnyLongDescripts() || TaskNotes.HasAnyLongNotes() || Commlogs.HasAnyLongNotes())) 
				{
					if(isSilent) {
						FormOpenDental.ExitCode=138;//Update must be done manually in order to get data loss notification(s).
						Application.Exit();
						return false;
					}
					if(!MsgBox.Show(this,true,"Data will be lost during this update."
						+"\r\nContact support in order to retrieve the data from a backup after the update."
						+"\r\n\r\nContinue?"))
					{
						MessageBox.Show("Your database has not been altered.");
						return false;
					}
				}
				if(FromVersion>=new Version("3.4.0")) {
					Prefs.UpdateBool(PrefName.CorruptedDatabase,true);
				}
				ConvertDatabases.FromVersion=FromVersion;
#if !DEBUG
				//Typically the UpdateInProgressOnComputerName preference will have already been set within FormUpdate.
				//However, the user could have cancelled out of FormUpdate after successfully downloading the Setup.exe
				//OR the Setup.exe could have been manually sent to our customer (during troubleshooting with HQ).
				//For those scenarios, the preference will be empty at this point and we need to let other computers know that an update going to start.
				//Updating the string (again) here will guarantee that all computers know an update is in fact in progress from this machine.
				Prefs.UpdateString(PrefName.UpdateInProgressOnComputerName,Environment.MachineName);
#endif
				//Show a progress window that will indecate to the user that there is an active update in progress.  Currently okay to show during isSilent.
				actionCloseConvertProgress=ODProgressOld.ShowProgressStatus("ConvertDatabases",hasMinimize:false,currentForm:currentForm);
				ConvertDatabases.To2_8_2();//begins going through the chain of conversion steps
				InvokeConvertMethods();//continues going through the chain of conversion steps starting at v17.1.1 via reflection.
				actionCloseConvertProgress();
				Cursor.Current=Cursors.Default;
				if(FromVersion>=new Version("3.4.0")) {
					//CacheL.Refresh(InvalidType.Prefs);//or it won't know it has to update in the next line.
					Prefs.UpdateBool(PrefName.CorruptedDatabase,false,true);//more forceful refresh in order to properly change flag
				}
				Cache.Refresh(InvalidType.Prefs);
				if(!isSilent) {
					MsgBox.Show(this,"Database update successful");
				}
				return true;
#if !DEBUG
			}
			catch(System.IO.FileNotFoundException e) {
				actionCloseConvertProgress?.Invoke();
				FormOpenDental.ExitCode=160;//File not found exception
				if(!isSilent) {
					MessageBox.Show(e.FileName+" "+Lan.g(this,"could not be found. Your database has not been altered and is still usable if you uninstall this version, then reinstall the previous version."));
				}
				if(FromVersion>=new Version("3.4.0")) {
					Prefs.UpdateBool(PrefName.CorruptedDatabase,false);
				}
				return false;
			}
			catch(System.IO.DirectoryNotFoundException) {
				actionCloseConvertProgress?.Invoke();
				FormOpenDental.ExitCode=160;//ConversionFiles folder could not be found
				if(!isSilent) {
					MessageBox.Show(Lan.g(this,"ConversionFiles folder could not be found. Your database has not been altered and is still usable if you uninstall this version, then reinstall the previous version."));
				}
				if(FromVersion>=new Version("3.4.0")) {
					Prefs.UpdateBool(PrefName.CorruptedDatabase,false);
				}
				return false;
			}
			catch(Exception ex) {
				actionCloseConvertProgress?.Invoke();
				FormOpenDental.ExitCode=201;//Database was corrupted due to an update failure
				if(!isSilent) {
					MessageBox.Show(ex.Message+"\r\n\r\n"
						+Lan.g(this,"Conversion unsuccessful. Your database is now corrupted and you cannot use it.  Please contact us."));
				}
				//Then, application will exit, and database will remain tagged as corrupted.
				return false;
			}
#endif
		}

		///<summary>Uses reflection to invoke private methods of the ConvertDatabase class in order from least to greatest if needed.
		///The old way of converting the database was to manually daisy chain methods together.
		///The new way is to just add a method that follows a strict naming pattern which this method will invoke when needed.</summary>
		private void InvokeConvertMethods() {
			DataConnection.CommandTimout=7200;//2 hours, because conversion commands may take longer to run.
			//Loop through the list of convert databases methods from front to back because it has already been sorted (least to greatest).
			foreach(ConvertDatabasesMethodInfo convertMethodInfo in ListConvertMethods) {
				//This pattern of using reflection to invoke our convert methods started in v17.1 so we will skip all methods prior to that version.
				if(convertMethodInfo.VersionCur < new Version(17,1)) {
					continue;
				}
				//Skip all methods that are below or equal to our "from" version.
				if(convertMethodInfo.VersionCur<=FromVersion) {
					continue;
				}
				//This convert method needs to be invoked.
				ODEvent.Fire(new ODEventArgs("ConvertDatabases","Upgrading database to version: " //No translations in convert script.
					+convertMethodInfo.VersionCur.ToString(3)));//Only show the major, minor, build (preserves old functionality).
				try {
					//Use reflection to invoke the private static method.
					convertMethodInfo.MethodInfoCur.Invoke(this,new object[] { });
				}
				catch(Exception ex) {
					string message=Lan.g(this,"Convert Database failed ");
					try { 
						string methodName=convertMethodInfo.MethodInfoCur.Name;
						if(!string.IsNullOrEmpty(methodName)) {
							message+=Lan.g(this,"during: ")+methodName+"() ";
						}
						string command=Db.LastCommand;
						if(!string.IsNullOrEmpty(command)) {
							message+=Lan.g(this,"while running: ")+command+";";
						}
					}
					catch(Exception e) {
						e.DoNothing();//If this fails for any reason then just continue.
					}
					throw new Exception(message+"  "+ex.Message+"  "+ex.InnerException.Message,ex.InnerException);
				}
				//Update the preference that keeps track of what version Open Dental has successfully upgraded to.
				//Always require major, minor, build, revision.  Will throw an exception if the revision was not explicitly set (which we always set).
				Prefs.UpdateStringNoCache(PrefName.DataBaseVersion,convertMethodInfo.VersionCur.ToString(4));
			}
			DataConnection.CommandTimout=3600;//Set back to default of 1 hour.
		}

		///<summary>Uses reflection to get all "version" methods from the ConvertDatabasesX classes that match the "ToX_X_X" pattern.
		///Also sorts the methods in the correct order of which they should be invoked.</summary>
		private static List<ConvertDatabasesMethodInfo> GetAllVersions() {
			//Get all the private methods from the ConvertDatabases class via reflection.
			MethodInfo[] arrayConvertDbMethods=(typeof(ConvertDatabases)).GetMethods(BindingFlags.Static | BindingFlags.NonPublic);
			//Sort the methods so that they are numerically in the order that we require they be invoked in.
			List<ConvertDatabasesMethodInfo> listConvertMethods=new List<ConvertDatabasesMethodInfo>();
			foreach(MethodInfo methodInfo in arrayConvertDbMethods) {
				//Make sure that the only methods we add to our list match our ToX_X_X pattern.
				if(!Regex.Match(methodInfo.Name,PatternMethodInfo,RegexOptions.IgnoreCase).Success) {
					continue;//This method does not follow our pattern and is most likely a helper method.
				}
				listConvertMethods.Add(new ConvertDatabasesMethodInfo(methodInfo));
			}
			//Make sure that the list of methods are sorted in ascending order (least to greatest).
			listConvertMethods.Sort((ConvertDatabasesMethodInfo x,ConvertDatabasesMethodInfo y) => { return x.VersionCur.CompareTo(y.VersionCur); });
			return listConvertMethods;
		}

	}

	///<summary>A helper class to quickly manage convert databases methods.  Provides access to the corresponding MethodInfo and Version.</summary>
	public class ConvertDatabasesMethodInfo {
		private MethodInfo _methodInfo;
		private Version _version;

		public MethodInfo MethodInfoCur {
			get {
				return _methodInfo;
			}
		}

		public Version VersionCur {
			get {
				return _version;
			}
		}

		///<summary>The method info passed in should have a name that follows the ToX_X_X pattern.
		///Throws an exception if pattern not followed.</summary>
		public ConvertDatabasesMethodInfo(MethodInfo methodInfo) {
			_methodInfo=methodInfo;
			_version=GetVersionFromConvertMethod(methodInfo);
		}

		///<summary>Uses a regular expression to extract a version from the name of the method passed in.
		///The method info passed in should have a name that follows the ToX_X_X pattern.
		///Throws an exception if the method name pattern was not followed.</summary>
		private Version GetVersionFromConvertMethod(MethodInfo methodInfo) {
			Match match=Regex.Match(methodInfo.Name,ClassConvertDatabase.PatternMethodInfo,RegexOptions.IgnoreCase);
			if(!match.Success) {
				throw new ApplicationException("Invalid convert databases method passed into GetVersionFromConvertMethod.");
			}
			int major=PIn.Int(match.Result("$1"));
			int minor=PIn.Int(match.Result("$2"));
			int build=PIn.Int(match.Result("$3"));
			return new Version(major,minor,build,0);
		}
	}

}