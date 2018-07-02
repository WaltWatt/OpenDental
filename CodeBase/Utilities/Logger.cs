using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Collections;
using System.Linq;

namespace CodeBase {
	///<summary>Used to log messages to our internal log file, or to other resources, such as message boxes.</summary>
	public class Logger{
		///<summary>Levels of logging severity to indicate importance.</summary> 
		public enum Severity{
			NONE=0,//Must be first.
			DEBUG=1,
			INFO=2,
			WARNING=3,
			ERROR=4,
			FATAL_ERROR=5,
		};

		///<summary>The number of bytes it takes to move the current log to the backup/old log, to prevent the log files from growing infinately.</summary>
		private const int logRollByteCount=1048576;
		public static Logger openlog=new Logger(ODFileUtils.GetProgramDirectory()+"openlog.txt");
		
		private string logFile="";
		///<summary>Specifies the current logging level. Any severity less than the given level is not logged.</summary> 
#if(DEBUG)
		public Severity level=Severity.DEBUG;
#else
		public Severity level=Severity.NONE;
#endif
		#region WebCore Logger Copy
		public static int MAX_FILE_SIZE_KB=1000;
		private static Dictionary<string /*sub-directory, can be empty string (not null though)*/,object[]/*{StreamWriter, Create DateTime} the file currently linked to this sub-directory*/> _files=new Dictionary<string,object[]>();
		private static object _lock=new object();
		private const string CLEANUP_DIR="CleanupLogger";
		private static ODThread _threadLoggerCleanup=null;
		#endregion
		///<summary>Boolean used to determine what directory the logger will write to.</summary>
		private static bool _canUseMyDocsDir=false;

		public Logger(string pLogFile){
				logFile=pLogFile;
		}

		///<summary>Convert a severity code into a string.</summary>
		public static string SeverityToString(Severity sev){
			switch(sev){
				case Severity.NONE:
					return "NONE";
				case Severity.DEBUG:
					return "DEBUG";
				case Severity.INFO:
					return "INFO";
				case Severity.WARNING:
					return "WARNING";
				case Severity.ERROR:
					return "ERROR";
				case Severity.FATAL_ERROR:
					return "FATAL ERROR";
				default:
					break;
			}
			return "UNKNOWN SEVERITY";
		}

		public static int MaxSeverityStringLength(){
			int maxlen=0;
			for(Severity sev=(Severity)1;sev<(Severity)7;sev++){
				maxlen=Math.Max(maxlen,SeverityToString(sev).Length);
			}
			return maxlen;
		}

		///<summary>Used to write the logger file to the MyDocuments directory to avoid having to run OD as admin. Use sparingly, should only be called once per application instance.</summary>
		public static void UseMyDocsDirectory() {
			lock (_lock) {
				if(_canUseMyDocsDir) {
					return;
				}
				CloseLogger();
				_canUseMyDocsDir=true;
			}
		}

		///<summary>Log a message from an unknown source.</summary>
		public bool Log(string message,Severity severity) {
			return Log(null,"",message,false,severity);
		}

		public bool LogMB(string message,Severity severity) {
			return Log(null,"",message,true,severity);
		}

		public bool Log(Object sender,string sendingFunctionName,string message,Severity severity) {
			return Log(sender,sendingFunctionName,message,false,severity);
		}

		public bool LogMB(Object sender,string sendingFunctionName,string message,Severity severity) {
			return Log(sender,sendingFunctionName,message,true,severity);
		}

		///<summary>Log a message to the log text file and add a description of the sender (for debugging purposes). If sender is null then a description of the sender is not printed. Always returns false so that a calling boolean function can return at the same time that it logs an error message.</summary>
		public bool Log(Object sender,string sendingFunctionName,string message,bool msgBox,Severity severity){
			if(severity>level){//Only log messages with a severity matches the current level. This will even skip message boxes.
				return false;
			}
			try{
				if(sender!=null){
					if(sendingFunctionName!=null && sendingFunctionName.Length>0){
						message=sender.ToString()+"."+sendingFunctionName+": "+message;
					}else{
						message=sender.ToString()+": "+message;
					}
				}else if(sendingFunctionName!=null && sendingFunctionName.Length>0){
					message=sendingFunctionName+": "+message;
				}
				int procId=System.Diagnostics.Process.GetCurrentProcess().Id;
				message=DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")+" "+procId.ToString().PadLeft(6,'0')+" "+
					SeverityToString(severity)+" "+message;
				if(msgBox) {
					MsgBoxCopyPaste mbox=new MsgBoxCopyPaste(message);
					mbox.ShowDialog();
				}
			}catch(Exception e){
				MessageBox.Show(e.ToString());
			}
			//File access is always exclusive, so if we cannot access the file, we can try again for a little while
			//and hope that the other process will release the file.
			bool tryagain=true;
			int numtries=0;
			while(tryagain && numtries<5){
				tryagain=false;
				numtries++;
				try{
					if(logFile!=null){
						//Ensure that the log file always exists before trying to read it.
						if(!File.Exists(logFile)){
							try{
								FileStream fs=File.Create(logFile);
								fs.Dispose();
							}catch{
							}
						}else{
							//Make the log file roll into the old log file when it reaches the roll byte size.
							System.IO.StreamReader sr=new System.IO.StreamReader(logFile);
							if(sr!=null){
								Stream st=sr.BaseStream;
								long fileLength=st.Length;
								if(fileLength>=logRollByteCount) {
									try {
										File.Copy(logFile,logFile+".old.txt");
									}catch{
									}
									try{
										File.Delete(logFile);
									}catch{
									}
									fileLength=0;
								}
								st.Dispose();
								sr.Dispose();
								if(fileLength<1){
									try{
										FileStream fs=File.Create(logFile);
										fs.Dispose();
									}catch{
									}
								}
							}
						}
						//Re-open the log file 
						System.IO.StreamWriter sw=new System.IO.StreamWriter(logFile,true);//Open the file exclusively.
						if(sw!=null) {
							sw.WriteLine(message);
							sw.Flush();
							sw.Dispose();//Close the file to allow exclusive access by other instances of OpenDental.
						}
					}
				}catch{
					tryagain=true;
				}
			}
			return false;
		}

		#region WebCore Logger Copy
		public const string DATETIME_FORMAT="MM/dd/yy HH:mm:ss:fff";
		private static DateTime _lastLoggerCleanup=DateTime.MinValue;

		#region Parseable Logging
		///<summary>This method takes a string that should be some kind of an identifier (usually method name) for the method that is being logged. 
		///The optional string is for any additional information the implementer finds useful to be in the log string. 
		///LogPath determines the directory to log to and LogPhase determines whether the logger is a "Start" line or a "Stop" line.</summary>
		public static void LogToPath(string log,LogPath path,LogPhase logPhase,string optionalDesc="") {
			string logWrite=GetCallingMethod()+" "+log;
			switch(logPhase) {
				case LogPhase.Unspecified:
					break;
				case LogPhase.Start:
					logWrite+=" start";
					break;
				case LogPhase.End:
					logWrite+=" end";
					break;
			}
			if(optionalDesc!="") {
				logWrite+=" ... "+optionalDesc;
			}
			LogVerbose(logWrite,path.ToString()+"\\"+Process.GetCurrentProcess().Id.ToString());
		}

		///<summary>Accomplishes the same function as LogSignals, but by using an action we can save on messy looking code for sections that need heavy logging.
		///Should generally be used on one line statements.</summary>
		public static void LogAction(string log,LogPath path,Action act,string optionalDesc="") {
			LogToPath(log,path,LogPhase.Start,optionalDesc);
			act();
			LogToPath(log,path,LogPhase.End);
		}

		///<summary>This method finds the method that called the logger. It loops through the first 5 stack frames and returns the full name of the 
		///method that called it. This method carefully excludes its own calling methods from the stack trace.</summary>
		private static string GetCallingMethod() {
			try {
				for(int i=2;i<4;i++) {//Start at stackframe(2) because 0,1, and possibly 2 are the parents of this method.		
						StackFrame frame=new StackFrame(i);
						System.Reflection.MethodBase method=frame.GetMethod();
						if(!method.Name.ToLower().Contains("logtopath") && !method.Name.ToLower().Contains("logaction")) {
							return method.ReflectedType.FullName+"."+method.Name;
						}
				}
			}
			catch(Exception e) {
					e.DoNothing();
			}
			//Return blank if we couldn't find a method name that wasn't ourself in the first 5 frames
			return "";
		}

		public delegate bool DoVerboseLoggingArgs();
		public static DoVerboseLoggingArgs DoVerboseLogging;

		///<summary>If HasVerboseLogging(Environment.MachineName) then Logger.WriteLine(log). Otherwise do nothing.</summary>
		private static void LogVerbose(string log,string subDirectory = "") {
			try {
				if(DoVerboseLogging==null || !DoVerboseLogging()) {
					return;
				}
				if(DateTime.Now.Subtract(_lastLoggerCleanup)>TimeSpan.FromDays(1)) { //Once a day logger cleanup is due.
					Logger.CleanupLoggerDirectoryAsync(0);
					_lastLoggerCleanup=DateTime.Now;
				}
				Logger.WriteLine(log,subDirectory);
			}
			catch(Exception e) {
				e.DoNothing();
			}
		}

		#endregion
		public static string GetDirectory(string subDirectory) {
			subDirectory=ScrubSubDirPath(subDirectory);
			string ret = "";
			bool canUseMyDocsDir;
			lock (_lock) {
				canUseMyDocsDir=_canUseMyDocsDir;
			}
			//Could make this a ternary operator but it is incredibly long.
			if(canUseMyDocsDir) {
				//The logger file is sometimes blocked by Windows unless OD is ran as admin. This is a work around to avoid that file block by writing to MyDocuments.
				ret=System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"Logger");
			}
			else {
				ret=ODFileUtils.CombinePaths(AppDomain.CurrentDomain.BaseDirectory,"Logger");
			}
			if(!string.IsNullOrEmpty(subDirectory)) {
				ret=ODFileUtils.CombinePaths(ret,subDirectory);
			}
			return ret;
		}

		public static void WriteLine(string line,string subDirectory) {
			WriteLine(line,subDirectory,false,true);
		}

		public static void WriteLine(string line,string subDirectory,bool singleFileOnly,bool includeTimestamp) {
			lock (_lock) {
				subDirectory=ScrubSubDirPath(subDirectory);
				StreamWriter file = Open(subDirectory,singleFileOnly);
				if(file==null) {
					return;
				}
				string timeStamp=includeTimestamp?(DateTime.Now.ToString(DATETIME_FORMAT)+"\t"):"";
				file.WriteLine(timeStamp+line);
			}
		}

		public static void WriteError(string line,string subDirectory) {
			WriteLine("failed - "+line,subDirectory,false,true);
		}

		public static void WriteException(Exception e,string subDirectory) {
			WriteError(e.Message+"\r\n"+e.StackTrace,subDirectory);
		}

		public static void CloseLogger() {
			lock (_lock) {
				while(_files.Count>=1) {
					IEnumerator enumerator = _files.Keys.GetEnumerator();
					if(enumerator==null||!enumerator.MoveNext()) {
						break;
					}
					CloseFile((string)enumerator.Current);
				}
			}
		}

		///<summary>Starts a thread which cleans up the Logger directory. 
		///Thread will be interrupted and stopped if doInterrupt returns true.
		///Thread can also be interrupted from it's dorman state if the Action which is returned is invoked.
		///Delete all files and folders under the Logger directory which are older than 30 days. 
		///This method can take several minutes if there are are lot of files to process.
		///If run once and exit is desired then set frequencyMS=0. Otherwise set frequencyMS to desired frequency of full scan.
		///It is suggested that this scan run no more than once per day.</summary>
		public static Action CleanupLoggerDirectoryAsync(int frequencyMS,Func<bool> doInterrupt=null,Action<string> onScanningItem=null,Action onExit=null) {
			Action onCancel=new Action(() => {
				if(_threadLoggerCleanup!=null) {
					_threadLoggerCleanup.QuitAsync(true);
				}
			});
			if(_threadLoggerCleanup!=null) { //Still running from 1 day ago. That would be a problem.
				return onCancel;
			}
			//Start a thread so we do not interrupt the rest of the AccountMaintThread operations.
			_threadLoggerCleanup=new ODThread(frequencyMS,new ODThread.WorkerDelegate((th) => {
				CleanupLoggerDirectory(doInterrupt,onScanningItem);
			}));
			_threadLoggerCleanup.AddExitHandler(new ODThread.WorkerDelegate((th) => {
				_threadLoggerCleanup=null;
				if(onExit!=null) {
					onExit();
				}
			}));
			_threadLoggerCleanup.AddExceptionHandler(new ODThread.ExceptionDelegate((e) => { WriteException(e,CLEANUP_DIR); }));
			_threadLoggerCleanup.Start(true);
			return onCancel;
		}

		///<summary>Cleans up the Logger directory. 
		///Can be interrupted and stopped if doInterrupt returns true.
		///Delete all files and folders under the Logger directory which are older than 30 days. 
		///This method can take several minutes if there are are lot of files to process.</summary>
		public static void CleanupLoggerDirectory(Func<bool> doInterrupt=null,Action<string> onScanningItem=null) {			
			try {
				Func<bool> quitPremature=new Func<bool>(() => {
					if(doInterrupt==null) {
						return false;
					}
					return doInterrupt();
				});
				Action<string> status=new Action<string>((s) => {
					WriteLine(s,CLEANUP_DIR);
				});
				Action<Exception> statusE=new Action<Exception>((e) => {
					WriteException(e,CLEANUP_DIR);
				});
				Action<int,int,bool,string> logIfNecessary=new Action<int, int, bool, string>((numerator,denominator,forceLog,itemName) => {
					if(!string.IsNullOrEmpty(itemName) && onScanningItem!=null) {
						onScanningItem(itemName);
					}
					//Log a max of 100 times.
					int mod=denominator/100;
					if(mod<=0) { //Guard against divide by 0.
						mod=1;
					}
					if(!forceLog && (numerator % mod != 0)) { //Not time to log yet.
						return;
					}
					int percentDone;
					if(denominator<=0) {
						percentDone=100;
					}
					else {
						percentDone=(int)(numerator/(double)denominator*100);
					}
					status("Scanned "+numerator.ToString()+" of "+denominator.ToString()+". "+percentDone.ToString()+"% done.");
					if(!string.IsNullOrEmpty(itemName)) {
						status("Current scan item: "+itemName);
					}
				});
				TimeSpan tsOldThan=TimeSpan.FromDays(30);
				DirectoryInfo di=new DirectoryInfo(GetDirectory(""));
				#region Delete files
				//Get all files recursively under this directory.
				status("Scanning files.");
				status("Querying files in directory: "+di.FullName);
				logIfNecessary(1,10000,false,di.FullName);
				List<FileInfo> files=new List<FileInfo>(di.GetFiles("*.*",SearchOption.AllDirectories))
					//Filter down to those files that are old enough to be deleted.
					.FindAll(x => x.CreationTime<DateTime.Today.Add(-tsOldThan));
				status("Deleting "+files.Count.ToString()+" files... "+(files.Sum(x => x.Length)/1024/(double)1024).ToString("N2")+" MB");
				FileInfo file;
				//Delete each of the files.
				for(int iCurFile=0;iCurFile<files.Count;iCurFile++) {
					try {
						file=files[iCurFile];
						if(quitPremature()) {
							logIfNecessary(iCurFile,files.Count,true,"");
							status("Quit file delete premature");
							return;
						}
						logIfNecessary(iCurFile,files.Count,false,file.FullName);
						file.Delete();
					}
					catch(Exception e) {
						statusE(e);
					}
				}
				status("File scan complete.");
				#endregion
				#region Delete directories
				//The files are now gone so it is safe to recursively delete all directories that may now be empty.
				//Count all directories and sub-directories so we can keep track of progress.
				int totalDirectories=Directory.GetDirectories(di.FullName,"*.*",SearchOption.AllDirectories).Length;
				int iCurFolder=0;
				//Delete the specified directory if it is empty. Optionally recursively delete any subdirectories which are empty.
				//Declared null at first so it can be called recursively from within it's own definition.
				Action<string> deleteEmptyDir=null;
				deleteEmptyDir=new Action<string>((diCur) => {
					#region deleteEmptyDir
					try {
						if(quitPremature()) {
							logIfNecessary(iCurFolder,totalDirectories,true,"");
							status("Quit directory delete premature");
							return;
						}
						foreach(string d in Directory.GetDirectories(diCur,"*.*",SearchOption.TopDirectoryOnly)) { //All directories that should delete must delete in order to return true for this directory.
							deleteEmptyDir(d);
						}
						logIfNecessary(iCurFolder,totalDirectories,false,diCur);
						iCurFolder++;
						//Throws exception if directory is not empty. That's what we want.
						Directory.Delete(diCur,false);
					}
					catch(IOException e) {
						//Directory is most likely not empty. 
						//It is much faster to try to delete the directory and have it fail than to query the diretory for existing files beforehand.
						e.DoNothing();
					}
					catch(Exception e) {
						//If we get here then a directory that should have been deleted was not deleted.
						statusE(e);
					}
					#endregion
				});
				status("Scanning "+totalDirectories.ToString()+" directories.");
				deleteEmptyDir(di.FullName);
				status("Directory scan complete.");
				#endregion
			}
			catch(Exception e) {
				WriteException(e,CLEANUP_DIR);
			}
		}
		
		private static void CloseFile(string subDirectory) {
			try {
				lock (_lock) {
					StreamWriter file = null;
					DateTime created = DateTime.Now;
					if(!TryGetFile(subDirectory,out file,out created)) {
						return;
					}
					file.Dispose();
					_files.Remove(subDirectory);
				}
			}
			catch { }
		}

		public static void ParseLogs(string directory) {
			try {
				DirectoryInfo di = new DirectoryInfo(directory);
				if(!di.Exists) {
					return;
				}
				using(StreamWriter sw = new StreamWriter(ODFileUtils.CombinePaths(AppDomain.CurrentDomain.BaseDirectory,"Errors - "+DateTime.Now.ToString("MM-dd-yy HH-mm-ss")+".txt"))) {
					ParseDirectory(di,sw);
					foreach(DirectoryInfo diSub in di.GetDirectories()) {
						ParseDirectory(diSub,sw);
					}
				}
			}
			catch(Exception e) {
				throw e;
			}
		}

		private static void ParseDirectory(DirectoryInfo di,StreamWriter sw) {
			FileInfo[] files = di.GetFiles("*.txt");
			foreach(FileInfo fi in files) {
				using(StreamReader sr = new StreamReader(fi.FullName)) {
					string line = "";
					string lower = "";
					while(sr.Peek()>0) {
						line=sr.ReadLine();
						lower=line.ToLower();
						if(!lower.Contains("failed")) {
							continue;
						}
						sw.WriteLine(line);
					}
				}
			}
		}

		public static bool SingleFileLoggerExists(string subDirectory) {
			subDirectory=ScrubSubDirPath(subDirectory);
			FileInfo fi = new FileInfo(ODFileUtils.CombinePaths(GetDirectory(subDirectory),subDirectory+".txt"));
			return fi.Exists;
		}

		private static bool TryGetFile(string subDirectory,out StreamWriter file,out DateTime created) {
			file=null;
			created=DateTime.MinValue;
			lock (_lock) {
				object[] obj = null;
				if(!_files.TryGetValue(subDirectory,out obj)) {
					return false;
				}
				file=(StreamWriter)obj[0];
				created=(DateTime)obj[1];
				return true;
			}
		}

		private static StreamWriter Open(string subDirectory,bool singleFileOnly) {
			try {
				lock (_lock) {
					StreamWriter file = null;
					DateTime created = DateTime.MinValue;
					if(TryGetFile(subDirectory,out file,out created)) { //file has been created
						if(singleFileOnly) {
							return file;
						}
						if(DateTime.Today==created.Date) { //it was created today
							if((file.BaseStream.Length/1024)<=MAX_FILE_SIZE_KB) { //it is within the acceptable size limit
								return file;
							}
						}
						CloseFile(subDirectory);
					}
					file=new StreamWriter(GetFileName(subDirectory,DateTime.Today,singleFileOnly),true);
					file.AutoFlush=true;
					_files[subDirectory]=new object[] { file,DateTime.Now };
					return file;
				}
			}
			catch {
				return null;
			}
		}

		///<summary>The full path and file name will be kept safe by Logger itself. Logger will not create a path name that won't comply with Windows file system.
		///The one exception is where we let the implementer set the subDirectory. This subDirectory could be any string so we will scrub that string here and make it comply.</summary>
		private static string ScrubSubDirPath(string dirIn) {
			//Remove any extra directory delimiters from the subDirectory.
			string dirOut=dirIn.TrimEnd('\\').TrimStart('\\');
			//This exhaustive list was found here. https://stackoverflow.com/a/33608950
			var replaceThese=Path.GetInvalidPathChars().Union(new char[] { ':', '?', '/', '!', '*', '%', '.', ',', }).ToList().FindAll(x => dirOut.Contains(x));
			foreach(char c in replaceThese) {
				dirOut=dirOut.Replace(c,'-');
			}
			return dirOut;
		}

		private static string GetFileNameSingleFileOnly(string subDirectory) {
			DirectoryInfo di = new DirectoryInfo(GetDirectory(subDirectory));
			FileInfo fi = new FileInfo(ODFileUtils.CombinePaths(di.FullName,subDirectory+".txt"));
			if(!di.Exists) {
				di.Create();
			}
			return fi.FullName;
		}

		private static string GetFileName(string subDirectory,DateTime date,bool singleFileOnly) {
			if(singleFileOnly) {
				return GetFileNameSingleFileOnly(subDirectory);
			}
			string formattedDate = date.ToString("yy-MM-dd");
			DirectoryInfo di = new DirectoryInfo(ODFileUtils.CombinePaths(GetDirectory(subDirectory),formattedDate));
			if(!di.Exists) {
				di.Create();
			}
			int fileNum = 1;
			do {
				FileInfo fi = new FileInfo(ODFileUtils.CombinePaths(di.FullName,formattedDate+" ("+fileNum.ToString("D3")+").txt"));
				if(!fi.Exists) { //file doesn't exist yet
					return fi.FullName;
				}
				if((fi.Length/1024)<=MAX_FILE_SIZE_KB) { //file is small enough to use
					return fi.FullName;
				}
				if(++fileNum>=1000) { //only create 1000 files max
					List<FileInfo> fileInfos = new List<FileInfo>(di.GetFiles(formattedDate+"*"));
					fileInfos.Sort(SortFileByModifiedTimeDesc);
					fileInfos[0].Delete();
					return fileInfos[0].FullName;
				}
			} while(true);
		}

		private static int SortFileByModifiedTimeDesc(FileInfo x,FileInfo y) {
			return x.LastWriteTime.CompareTo(y.LastWriteTime);
		}

		public delegate void WriteLineDelegate(string data,LogLevel logLevel);

		public class LoggerEventArgs:EventArgs {
			public string Data;
			public LogLevel LogLevel;
			public LoggerEventArgs(string data,LogLevel logLevel) {
				Data=data;
				LogLevel=logLevel;
			}
		};
		#endregion

		public interface IWriteLine {
			void WriteLine(string data,LogLevel logLevel,string subDirectory = "");
		}
	}

	///<summary>Class that writes to the specified logger directory.</summary>
	public class LogWriter:Logger.IWriteLine {
		public LogLevel LogLevel;
		public string BaseDirectory;

		///<summary>For serialization.</summary>
		public LogWriter() {
		}

		public LogWriter(LogLevel logLevel,string baseDirectory) {
			LogLevel=logLevel;
			BaseDirectory=baseDirectory;
		}

		public virtual void WriteLine(string data,LogLevel logLevel,string subDirectory = "") {
			if(logLevel>LogLevel) {
				return;
			}
			Logger.WriteLine(data,ODFileUtils.CombinePaths(BaseDirectory,subDirectory));
		}
	}

	///<summary>Class that generates a unique ID and includes it in every line it logs.</summary>
	public class LogRequest:LogWriter {
		private string _requestId;

		///<summary>For serialization.</summary>
		public LogRequest() {
		}

		public LogRequest(LogLevel logLevel,string baseDirectory) : base(logLevel,baseDirectory) {
			_requestId=Guid.NewGuid().ToString();
		}

		public override void WriteLine(string data,LogLevel logLevel,string subDirectory = "") {
			base.WriteLine("Request ID: "+_requestId+"\tMethod: "+GetCallingMethod()+"\t"+data,logLevel,subDirectory);
		}

		///<summary>Gets the name of the method that is calling WriteLine().</summary>
		private string GetCallingMethod() {
			try {
				for(int i=1;i<4;i++) {//Start at stackframe(1) because 0 is the parent of this method.		
					StackFrame frame=new StackFrame(i);
					System.Reflection.MethodBase method=frame.GetMethod();
					if(!method.Name.ToLower().Contains("writeline")) {
						return method.ReflectedType.FullName+"."+method.Name;
					}
				}
			}
			catch(Exception e) {
				e.DoNothing();
			}
			//Return blank if we couldn't find a method name that wasn't ourself in the first 5 frames
			return "";
		}
	}

	///<summary>0=Error, 1=Information, 2=Verbose</summary>
	public enum LogLevel {
		///<summary>0 Logs only errors.</summary>
		Error = 0,
		///<summary>1 Logs information plus errors.</summary>
		Information = 1,
		///<summary>2 Most verbose form of logging (use sparingly for very specific troubleshooting). Logs all entries all the time.</summary>
		Verbose = 2
	}

	///<summary>Used by LogToPath to decide if it needs to make a start entry, end entry, or unspecified for empirical log entries.</summary>
	public enum LogPhase {
		Unspecified,
		Start,
		End,
	}

	///<summary>Used by LogToPath to decide which folder to log to.</summary>
	public enum LogPath {
		Signals,
		ChartModule,
		AccountModule,
		OrthoChart,
	}
}
