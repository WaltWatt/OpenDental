using CodeBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace UpdateFileCopier {
	public partial class FormMain:Form {
		private string _sourceDirectory;
		private string _destDirectory;
		///<summary>Set to true if any files in the installation directory are currently in use.</summary>
		private bool _hasFilesInUse=false;
		///<summary>Set to true if any files in the UpdateFiles folder failed to copy over.</summary>
		private bool _hasCopyFailed=false;
		///<summary>If anything goes wrong with the file copying, this string should be used to hold more details about the error to show later.</summary>
		private string _error="";
		///<summary>Temporary directory that will be used to copy any files present in the destination folder before attempting to copy.</summary>
		private string TempPathDest {
			get {
				return ODFileUtils.CombinePaths(Path.GetTempPath(),"opendental","updatefilecopier","local");
			}
		}
		///<summary>Temporary directory that will be used to copy all files in the destination folder.</summary>
		private string TempPathSource {
			get {
				return ODFileUtils.CombinePaths(Path.GetTempPath(),"opendental","updatefilecopier","source");
			}
		}

		public FormMain(string sourceDirectory,string openDentProcessId,string destDirectory) {
			InitializeComponent();
			_sourceDirectory=sourceDirectory;
			_destDirectory=destDirectory;
		}

		private void FormMain_Load(object sender,EventArgs e) {

		}

		private void FormMain_Shown(object sender,EventArgs e) {
			StartFileCopierThread();
		}

		///<summary>Kills the processes OpenDental and WebCamOD and then starts a file copier thread.</summary>
		private void StartFileCopierThread() {
			Cursor=Cursors.WaitCursor;
			//kill all processes named OpenDental.
			//If the software has been rebranded, the original exe will NOT be correctly closed.
			KillProcess("OpenDental");
			//kill all processes named WebCamOD.
			//web cam does not always close properly when updater kills OpenDental
			//web cam relies on shared library, OpenDentBusiness.dll (shared ref with OpenDental). 
			//if this lib can't be updated then the opendental update/install fails
			KillProcess("WebCamOD");
			KillProcess("ProximityOD");
			//Kill known applications that are present within the installation directory so that the resources are freed if currently in use.
			//A customer complained on the forums about CentralManager explicitly.  DatabaseIntegrityCheck and ServiceManager are just in case.
			KillProcess("CentralManager");
			KillProcess("DatabaseIntegrityCheck");
			KillProcess("ServiceManager");
			//Create a separate thread to copy over the files from the UpdateFiles folder share.
			//In putting this logic in a thread, the user can close the window and not disrupt the copying process.
			ODThread odThread=new ODThread(OnThreadStart);
			odThread.AddExitHandler(OnThreadComplete);
			odThread.Start(true);
		}

		///<summary></summary>
		private static void KillProcess(string name) {
			Process[] processes=Process.GetProcessesByName(name);
			for(int i=0;i<processes.Length;i++) {
				try {
					processes[i].Kill();//CloseMainWindow and Close were ineffective if a dialog was open.
				}
				catch {
					//Kill() could fail if the process is closed between the time that the process list is read and the time that the Kill() function is called.
					//Since each Kill() call could take a few seconds, this exception could easily be caused by user interaction. In this case, the instance
					//is already closed so we don't need to take any further action.
				}
			}
		}

		///<summary>Attempts to open files that will be updated in the destination dir to check if any are currently in use.
		///Attempts to create temporary directories that will house the installation files temporarily.
		///Attempts to copy all files in the destination directory to a temp directory so that files can be reverted in case of a catastrophic failure.
		///Attempts to copy all files in the source directory to a local temp directory.
		///Attempts to move all files from the source temp directory into the destination directory.</summary>
		private void OnThreadStart(ODThread odThread) {
			SetLabelText("Preparing to Copy Files...");
			//Delay the thread for 300 milliseconds to make sure the above processes have really exited.
			Thread.Sleep(300);
			//Any file exclusions will have happened when originally copying files into the AtoZ folder.  Ex: FreeDentalConfig.xml
			//And that happens in OpenDental.PrefL.CheckProgramVersion().
			DirectoryInfo dirInfoSource=new DirectoryInfo(_sourceDirectory);
			FileInfo[] arraySourceFiles=dirInfoSource.GetFiles();
			DirectoryInfo dirInfoDest=new DirectoryInfo(_destDirectory);
			FileInfo[] arrayDestFiles=dirInfoDest.GetFiles();
			_hasFilesInUse=false;
			_hasCopyFailed=false;
			//Recreate the temp directories that will be used in the copying process
			if(!CreateTempDirectories()) {
				_hasCopyFailed=true;
				return;
			}
			//Check if any of the corresponding files in the installation directory are in use.
			if(HasFilesInUse(arraySourceFiles,arrayDestFiles)) {
				_hasFilesInUse=true;
				return;
			}
			if(!CopyFilesFromDestToTemp(arrayDestFiles)
				|| !CopyFilesFromSourceToTemp(arraySourceFiles)) 
			{
				_hasCopyFailed=true;
				return;
			}
			if(!MoveFilesToDestDir(arraySourceFiles)) {
				RevertDestFiles(arrayDestFiles);//Revert the files in the destination directory back to the way they were before trying to update them.
				_hasCopyFailed=true;
				return;
			}
			CleanUp();
		}

		///<summary></summary>
		private void OnThreadComplete(ODThread odThread) {
			LaunchOpenDental();
		}

		///<summary>Tries to recreate the two temporary directories that will be used in the copying process.
		///Returns false if deleting or creating the directories fails.</summary>
		private bool CreateTempDirectories() {
			try {
				//Recreate the current temp dir (used to copy files currently present in the destination directory).
				if(Directory.Exists(TempPathDest)) {
					Directory.Delete(TempPathDest,true);
				}
				Directory.CreateDirectory(TempPathDest);
				//Recreate the source temp dir (files from the source directory).
				if(Directory.Exists(TempPathSource)) {
					Directory.Delete(TempPathSource,true);
				}
				Directory.CreateDirectory(TempPathSource);
			}
			catch {
				//SetLabelText("Temporary directory recreate failure.");
				return false;
			}
			return true;
		}

		///<summary>Clean up method for after copying has completed.  Deletes temporary directories.</summary>
		private void CleanUp() {
			SetLabelText("Cleaning up...");
			try {
				if(Directory.Exists(TempPathDest)) {
					Directory.Delete(TempPathDest,true);
				}
				if(Directory.Exists(TempPathSource)) {
					Directory.Delete(TempPathSource,true);
				}
			}
			catch {
				//Can't delete the temporary folders that we created.  Oh well.
			}
		}

		///<summary>Verifies that all files passed in are not currently in use in the destination directory.
		///Returns true if there was a file with the same name in the list of files that will be copied still in use.</summary>
		private bool HasFilesInUse(FileInfo[] arraySourceFiles,FileInfo[] arrayDestFiles) {
			//For every file passed in, make sure the corresponding file in the destination directory is not in use.
			for(int i=0;i<arrayDestFiles.Length;i++) {
				bool isFilePresentInDestDir=false;
				//Loop through all the source file names to make sure this is a file that will be getting updated.
				for(int j=0;j<arraySourceFiles.Length;j++) {
					if(arraySourceFiles[j].Name==arrayDestFiles[i].Name) {
						//This file is present in the current destination directory and will need to be updated.
						isFilePresentInDestDir=true;
						break;
					}
				}
				if(!isFilePresentInDestDir) {
					continue;//File not present in destination directory, thus it cannot be in use.  Move on.
				}
				//Try to open the file which will successfully tell us if the file is currently in use.
				try {
					using(FileStream fStream=arrayDestFiles[i].Open(FileMode.Open,FileAccess.ReadWrite)) {
						//The quickest, easiest way to tell if a file is already in use is to simply try to open it with Write permissions.
						//If the file is in use and we try to open it for writing, then an exception will occur.  We also need Read permissions to copy.
						//If successful, the stream will close right away and move on to the next file.
						//This does not combat thread race conditions.  That is an impossible issue to combat (no such thing as an accurate "file in use" check).
					}
				}
				catch(System.Security.SecurityException se) {
					_error="Security Error when accessing "+arrayDestFiles[i].Name+": "+se.Message;
					return true;
				}
				catch(ArgumentException ae) {
					_error="Argument Error when accessing "+arrayDestFiles[i].Name+": "+ae.Message;
					return true;
				}
				catch(FileNotFoundException fnfe) {
					_error="File Not Found Error when accessing "+arrayDestFiles[i].Name+": "+fnfe.Message;
					return true;
				}
				catch(UnauthorizedAccessException uae) {
					_error="Unauthorized Access Error when accessing "+arrayDestFiles[i].Name+": "+uae.Message;
					return true;
				}
				catch(DirectoryNotFoundException dnfe) {
					_error="Directory Not Found Error when accessing "+arrayDestFiles[i].Name+": "+dnfe.Message;
					return true;
				}
				catch(IOException ioe) {
					_error="IO Error when accessing "+arrayDestFiles[i].Name+": "+ioe.Message;
					return true;
				}
				//The file is unavailable because it is still be written to or used by another thread.
				catch(Exception ex) {
					_error="Generic Error when accessing "+arrayDestFiles[i].Name+": "+ex.Message;
					return true;
				}
			}
			return false;
		}

		///<summary>Copies all files from the destination directory just in case something goes wrong there is a backup to revert back to.</summary>
		private bool CopyFilesFromDestToTemp(FileInfo[] arrayDestFiles) {
			for(int i=0;i<arrayDestFiles.Length;i++) {
				SetLabelText("Backing up: "+arrayDestFiles[i].Name);
				string sourceFileName=arrayDestFiles[i].FullName;
				string destFileName=ODFileUtils.CombinePaths(TempPathDest,arrayDestFiles[i].Name);
				try {
					File.Copy(sourceFileName,destFileName,true);
				}
				catch {
					//SetLabelText("Copying files from destination to temp failed.");
					return false;
				}
			}
			return true;//Files backed up successfully.
		}

		///<summary>Copies all files in the UpdateFiles share to a local temp directory.  
		///Returns false if there was a problem copying the files from the source directory.</summary>
		private bool CopyFilesFromSourceToTemp(FileInfo[] arraySourceFiles) {
			for(int i=0;i<arraySourceFiles.Length;i++) {
				SetLabelText("Copying: "+arraySourceFiles[i].Name);
				string source=arraySourceFiles[i].FullName;
				string dest=ODFileUtils.CombinePaths(TempPathSource,arraySourceFiles[i].Name);
				try {
					File.Copy(source,dest,true);
				}
				catch {
					//SetLabelText("Copying files from source to temp failed.");
					return false;
				}
			}
			return true;//Files copied successfully.
		}

		///<summary>Moves all files in the temporary directory to the destination directory.
		///Returns false if there was a problem moving the files from the temporary directory to the destination directory.</summary>
		private bool MoveFilesToDestDir(FileInfo[] arraySourceFiles) {
			string sourceManifest="";
			for(int i=0;i<arraySourceFiles.Length;i++) {
				if(arraySourceFiles[i].Name=="Manifest.txt") {
					sourceManifest=ODFileUtils.CombinePaths(TempPathSource,arraySourceFiles[i].Name);
					continue;
				}
				SetLabelText("Verifying: "+arraySourceFiles[i].Name);
				string source=ODFileUtils.CombinePaths(TempPathSource,arraySourceFiles[i].Name);
				string dest=ODFileUtils.CombinePaths(_destDirectory,arraySourceFiles[i].Name);
				try {
					File.Copy(source,dest,true);
				}
				catch {
					//SetLabelText("Copying files from temp to destination failed.");
					return false;
				}
			}
			//Save the Manifest.txt file for last so that any subsequent attempts launching Open Dental will try to launch the FileUpdateCopier again.
			if(sourceManifest!="") {
				try {
					File.Copy(sourceManifest,Path.Combine(_destDirectory,"Manifest.txt"),true);
				}
				catch {
					//SetLabelText("Copying Manifest.txt from temp to destination failed.");
					return false;
				}
			}
			return true;//Files copied successfully.
		}

		private void RevertDestFiles(FileInfo[] arrayDestFiles) {
			for(int i=0;i<arrayDestFiles.Length;i++) {
				SetLabelText("Reverting: "+arrayDestFiles[i].Name);
				string source=ODFileUtils.CombinePaths(TempPathDest,arrayDestFiles[i].Name);
				string dest=ODFileUtils.CombinePaths(_destDirectory,arrayDestFiles[i].Name);
				try {
					File.Copy(source,dest,true);
				}
				catch {
					//Not sure what to do here.  Maybe someone started using a file in the middle of a copy?
				}
			}
		}

		///<summary></summary>
		private void SetLabelText(string text) {
			if(this.InvokeRequired) {
				this.Invoke((Action)delegate() { SetLabelText(text); });
				return;
			}
			labelFile.Text=text;
		}

		///<summary>Checks and displays messages for any errors that occurred during the file copying.
		///If no errors, attempts to launch a new instance of Open Dental with the freshly copied binaries.</summary>
		private void LaunchOpenDental() {
			if(this.InvokeRequired) {
				this.Invoke((Action)delegate() { LaunchOpenDental(); });
				return;
			}
			Cursor=Cursors.Default;
			if(_hasFilesInUse) {
				string error="There are files still in use.  Please make sure all instances of Open Dental are closed then click Retry.";
				if(!string.IsNullOrEmpty(_error)) {
					error=_error+"\r\n\r\nPlease address the error above, make sure all instances of Open Dental are closed, then click Retry.";
				}
				SetLabelText(error);
				butRetry.Visible=true;
			}
			else if(_hasCopyFailed) {
				SetLabelText("Some files failed to copy.  Verify network access and permissions are correct, then click Retry.");
				butRetry.Visible=true;
			}
			else {//Everything copied correctly, try and launch Open Dental.
				SetLabelText("Done");
				Thread.Sleep(300);//Give SetLabelText some time to finish to make sure Application.Exit doesn't get called too soon.
				//If Open Dental has been rebranded, then change this value:
				Process.Start(Path.Combine(_destDirectory,"OpenDental.exe"));
				Application.Exit();//To close the form.
			}
		}

		private void butRetry_Click(object sender,EventArgs e) {
			butRetry.Visible=false;
			StartFileCopierThread();
		}


	}	
}
