using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;
using CodeBase;
using System.Threading;
using OpenDentBusiness;

namespace OpenDental {
	static class ProgramEntry {
		[STAThread]
		static void Main(string[] args) {
			//Application.EnableVisualStyles() uses version 6 of comctl32.dll instead of version 5. See https://support.microsoft.com/en-us/kb/2892345
			//See also http://stackoverflow.com/questions/8335983/accessviolationexception-on-tooltip-that-faults-comctl32-dll-net-4-0
			Application.EnableVisualStyles();//This line fixes rare AccessViolationExceptions for ToolTips on our ValidDate boxes, ValidDouble boxes, etc...
			//Register an EventHandler which handles unhandled exceptions.
			//AppDomain.CurrentDomain.UnhandledException+=new UnhandledExceptionEventHandler(OnUnhandeledExceptionPolicy);
			bool isSecondInstance=false ;//or more.
			Process[] processes=Process.GetProcesses();
			for(int i=0;i<processes.Length;i++) {
				if(processes[i].Id==Process.GetCurrentProcess().Id) {
					continue;
				}
				//we have to do it this way because during debugging, the name has vshost tacked onto the end.
				if(processes[i].ProcessName.StartsWith("OpenDental")) {
					isSecondInstance=true;
					break;
				}
			}
			/*
			if(args.Length>0) {//if any command line args, then we will attempt to reuse an existing OD window.
				if(isSecondInstance){
					FormCommandLinePassOff formCommandLine=new FormCommandLinePassOff();
					formCommandLine.CommandLineArgs=new string[args.Length];
					args.CopyTo(formCommandLine.CommandLineArgs,0);
					Application.Run(formCommandLine);
					return;
				}
			}*/
			Application.SetCompatibleTextRenderingDefault(false);//designer uses new text rendering.  This makes the exe use matching text rendering.  Before this was added, it was common for labels to be longer in the running program than they were in the designer.
			Application.EnableVisualStyles();//changes appearance to XP
			Application.DoEvents();
			string[] cla=new string[args.Length];
			args.CopyTo(cla,0);
			FormOpenDental formOD=new FormOpenDental(cla);
			Action<Exception,string> onUnhandled=new Action<Exception,string>((e,threadName) => {
				//Try to automatically submit a bug report to HQ.  Default to "None" just in case the submission fails.
				BugSubmissionResult subResult=BugSubmissionResult.None;
				try {
					subResult=BugSubmissions.SubmitException(e,threadName,FormOpenDental.CurPatNum,formOD.GetSelectedModuleName());
				}
				catch(Exception ex) {
					ex.DoNothing();
				}
				FriendlyException.Show("Critical Error: "+e.Message,e,"Quit");
				formOD.ProcessKillCommand();
			});
			CodeBase.ODThread.RegisterForUnhandledExceptions(formOD,onUnhandled);
			formOD.IsSecondInstance=isSecondInstance;
			Application.AddMessageFilter(new ODGlobalUserActiveHandler());
			Application.ThreadException+=new ThreadExceptionEventHandler((object s,ThreadExceptionEventArgs e) => {
				onUnhandled(e.Exception,"ProgramEntry");
			});
			Application.Run(formOD);
		}

	}
}
