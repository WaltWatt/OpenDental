using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace CodeBase {
	///<summary>A wrapper for the c# Thread class.  The purpose of this class is to help implement a well defined pattern throughout our applications.  It also allows us to better document threading where C# lacks documentation.  Since there is no way to get the list of managed threads for an application, the only way we can maintain a list is to do it ourselves.  The advantage of maintaining a list of managed threads is that we can much more easily ensure that all threads are gracefully quit when the program exits.</summary>
	public class ODThread {
		///<summary>The C# thread that is used to run ODThread internally.</summary>
		private Thread _thread=null;
		///<summary>Sleep timer which can be interrupted elegantly.</summary>
		private AutoResetEvent _waitEvent=new AutoResetEvent(false);
		///<summary>The exact time when this thread was started.  Useful for determining thread run times.</summary>
		private DateTime _dateTimeStart=DateTime.MinValue;
		///<summary>The exact time when this thread was quit.  Useful for determining thread run times.</summary>
		private DateTime _dateTimeQuit=DateTime.MinValue;
		///<summary>Gets set to true when QuitSync() or QuitAsync() has been called or if this thread has finished and no timed interval was set.</summary>
		private bool _hasQuit=false;
		///<summary>Indicates if ODThread has been scheduled to quit. Check this from within a resource intensive thread periodically if you want to exit gracefully during the course of the WorkerDelegate function.</summary>
		public bool HasQuit {
			get {
				return _hasQuit;
			}
		}
		///<summary>The amount of time in milliseconds that this thread will sleep before calling the WorkerDelegate again.  Setting the interval to zero or a negative number will call the WorkerDelegate once and then quit itself.</summary>
		public int TimeIntervalMS=0;
		///<summary>Pointer to the function from the calling code which will perform the majority of this thread's work.</summary>
		private WorkerDelegate _worker=null;
		///<summary>Pointer to the function from the calling code which will be alerted when the run function has thrown an unhandled exception.</summary>
		private ExceptionDelegate _exceptionHandler=null;
		///<summary>Pointer to the function from the calling code which will be alerted when the run function has completed.  This will NOT fire if Join() times out.</summary>
		private WorkerDelegate _exitHandler=null;
		///<summary>Pointer to the function from the calling code which will be run before the main worker delegate starts.</summary>
		private WorkerDelegate _setupHandler=null;
		///<summary>Custom data which can be set before launching the thread and then safely accessed within the WorkerDelegate.  Helps prevent the need to lock objects due to multi-threading, most of the time.</summary>
		public object Tag=null;
		///<summary>Custom data which can be used within the WorkerDelegate.  Helps prevent the need to lock objects due to multi-threading, most of the time.</summary>
		public object[] Parameters=null;
		///<summary>Used to identify groups of ODThread objects.  Helpful when you need to wait for or quit an entire group of threads.  Initially set to "default".</summary>
		public string GroupName="default";
		///<summary>Global list of all ODThreads which have not been quit.  Used for thread group operations.</summary>
		private static List<ODThread> _listOdThreads=new List<ODThread>();
		///<summary>Thread safe lock object.  Any time a static variable is accessed, it MUST be wrapped with a lock.  Failing to lock will result in a potential for unsafe access by multiple threads at the same time.</summary>
		private static object _lockObj=new object();
		///<summary>Only set when calling Start().  Causes this thread to automatically remove itself from the global list of ODThreads once it has finished doing work.</summary>
		private bool _isAutoCleanup;
		///<summary>Used internally by RegisterForUnhandled().</summary>
		private static Action<Exception,Thread> OnUnhandledException;
		///<summary>An object used to update the UI of progress happening in the thread.</summary>
		private IODProgress _progressLog;

		///<summary>Gets or sets the name of the C# thread to make it easier to find specific threads while debugging.</summary>
		public string Name { 
			get {
				return _thread.Name;
			}
			set {
				_thread.Name=value;
			}
		}

		///<summary>An object used to update the UI of progress happening in the thread. This will be closed automatically when the thread finishes.
		///</summary>
		public IODProgress ProgressLog {
			get {
				if(_progressLog==null) {
					return ODProgressDoNothing.Instance;
				}
				return _progressLog;
			}
			set {
				_progressLog=value;
			}
		}

		///<summary>Creates a thread that will only run once after Start() is called.</summary>
		public ODThread(WorkerDelegate worker) : this(worker,null) {
		}

		///<summary>Creates a thread that will only run once after Start() is called.</summary>
		public ODThread(WorkerDelegate worker,params object[] parameters) : this(0,worker,parameters) {
		}

		///<summary>Creates a thread that will continue to run the WorkerDelegate after Start() is called and will stop running once one of the quit methods has been called or the application itself is closing.  timeIntervalMS (in milliseconds) determines how long the thread will wait before executing the WorkerDelegate again.  Set timeIntervalMS to zero or a negative number to have the WorkerDelegate only execute once and then quit itself.</summary>
		public ODThread(int timeIntervalMS,WorkerDelegate worker,params object[] parameters) {
			lock(_lockObj) {
				_listOdThreads.Add(this);
			}
			_thread=new Thread(new ThreadStart(this.Run));
			TimeIntervalMS=timeIntervalMS;
			_worker+=worker;
			Parameters=parameters;
		}

		public override string ToString() {
			return Name;
		}

		public void SetApartmentState(ApartmentState aptState) {
			_thread.SetApartmentState(aptState);
		}

		///<summary>Start all threads for a given group. If thread has already been started then take no action on that thread.</summary>
		public static void StartThreadsByGroupName(string groupName) {
			List<ODThread> listOdThreadsForGroup=GetThreadsByGroupName(groupName);
			for(int i=0;i<listOdThreadsForGroup.Count;i++) {
				listOdThreadsForGroup[i].Start(false);
			}			
		}

		///<summary>Starts the thread and returns immediately.  If the thread is already started or has already finished, then this function will have no 
		///effect.  Set isAutoCleanup to true to have this thread automatically remove itself from the global list of ODThreads once it has finished doing
		///work.</summary>
		public void Start(bool isAutoCleanup=true) {
			_isAutoCleanup=isAutoCleanup;
			if(_thread.IsAlive) {
				return;//The thread is already running.
			}
			if(_hasQuit) {
				return;//The thread has finished.
			}
			_dateTimeStart=DateTime.Now;
			_thread.Start();
		}

		///<summary>If the thread is currently waiting, this will interrupt the wait and force the thread to continue running instantly.</summary>
		public void Wakeup() {
			_waitEvent.Set();
		}

		///<summary>Main thread loop that executes the WorkerDelegate and sleeps for the designated timeIntervalMS (in milliseconds) if one was set.</summary>
		private void Run() {
			try {
				_setupHandler?.Invoke(this);
			}
			catch(Exception e) {
				if(!WorkerExceptionHandler(e)) {
					return;
				}
			}
			while(!_hasQuit) {
				try {
					_worker(this);
				}
				catch(Exception e) {
					if(!WorkerExceptionHandler(e)) {
						return;
					}
				}
				if(TimeIntervalMS>0) {
					if(!_hasQuit) {
						_waitEvent.WaitOne(TimeIntervalMS);//WaitOne is used instead of Sleep so that threads can be 'woken up' in the middle of waiting in order to process pertinent information.
					}
				}
				else if(TimeIntervalMS<=0) {//Interval was set to zero or a negative number, so do work once and then quits the thread.
					_hasQuit=true;
				}
			}
			_dateTimeQuit=DateTime.Now;
			try {
				ProgressLog.Close();
			}
			catch(Exception e) {
				e.DoNothing();
			}
			_exitHandler?.Invoke(this);
			if(_isAutoCleanup) {
				lock(_lockObj) {
					_listOdThreads.Remove(this);
				}
			}
		}

		///<summary>Calls the appropriate exception handler for this exception. Returns false if the thread needs to quit.</summary>
		private bool WorkerExceptionHandler(Exception e) {
			if(e is ThreadAbortException) {
				//We know that a join failed by exceeding the allotted timeout.
				_dateTimeQuit=DateTime.Now;
				_hasQuit=true;
				return false;
			}
			//An exception was unhandled by the worker delegate. Alert the caller if they have subscribed to this event.
			if(_exceptionHandler!=null) { //Do not set the quit flag, just call the handler and let the thread continue living.
				_exceptionHandler(e);
			}
			else {
				//Caller has not explicitly registered for this thread's exception handler so stop program execution and alert end user that something unforeseen has failed.
				//In this case it is safe to quit this thread because the application is probably about to either hard crash or exit gracefully.
				_hasQuit=true;
				if(OnUnhandledException!=null) {
					OnUnhandledException(e,_thread);
					return false;
				}
				//If we get here the entire program will shutdown without warning and only leave a vague reference to KERNELBASE.dll in the event viewer.
				throw e;
			}
			return true;
		}

		///<summary>Forces the calling thread to synchronously wait for the current thread to finish doing work.  Pass Timeout.Infinite into timeoutMS if you wish to wait as long as necessary for the thread to join.  The thread will be aborted if the timeout was reached and then will return false.</summary>
		public bool Join(int timeoutMS) {
			if(_thread.ThreadState==ThreadState.Unstarted) {
				return true;//Thread has not even started yet to we cannot join.
			}
			bool hasJoined=_thread.Join(timeoutMS);
			if(!hasJoined) {
				//The timeout expired and the thread is still alive so we want to abort it manually.
				//Abort exceptions will be swallowed within Run()
				_thread.Abort();
			}
			return hasJoined;
		}

		///<summary>Raises onExit when all thread's from the given groupName have exited. Returns immediately.</summary>
		public static void AddGroupNameExitHandler(string groupName,EventHandler onExit) {
			new ODThread(new WorkerDelegate((ODThread thread) => {
				JoinThreadsByGroupName(Timeout.Infinite,groupName,false);
				onExit(groupName,new EventArgs());
			})).Start(true);			
		}

		///<summary>Synchronously waits for all threads in the specified group to finish doing work.  Pass Timeout.Infinite into timeoutMS if you wish to wait as long as necessary for all threads to join.  Set doRemoveThreads to true to remove all threads from the global list of threads.</summary>
		public static void JoinThreadsByGroupName(int timeoutMS,string groupName,bool doRemoveThreads=false) {
			List<ODThread> listOdThreadsForGroup=GetThreadsByGroupName(groupName);
			for(int i=0;i<listOdThreadsForGroup.Count;i++) {
				listOdThreadsForGroup[i].Join(timeoutMS);
			}
			if(doRemoveThreads) {
				//Remove all threads from the global list of ODThreads.
				foreach(ODThread thread in listOdThreadsForGroup) {
					thread.QuitAsync();
				}
			}
		}

		///<summary>Immediately returns after flagging the thread to quit itself asynchronously.  The thread may execute a bit longer.  If the thread has been forgotten, it will be forcefully quit on closing of the main application.</summary>
		public void QuitAsync() {
			QuitAsync(true);
		}

		///<summary>Immediately returns after flagging the thread to quit itself asynchronously.  The thread may execute a bit longer.  If the thread has been forgotten, it will be forcefully quit on closing of the main application.  Set removeThread false if you want this thread to stay in the global list of ODThreads.</summary>
		public void QuitAsync(bool removeThread) {
			_hasQuit=true;
			//If thread is in idle due to wait event, then wake it immediately so we can more quickly quit.  Helps the thread quit within timeoutMS.
			Wakeup();
			if(removeThread) {
				lock(_lockObj) {
					_listOdThreads.Remove(this);
				}
			}
		}

		///<summary>Waits for this thread to quit itself before returning.  If the thread has been forgotten, it will be forcefully quit on closing of the main application.</summary>
		public void QuitSync(int timeoutMS) {
			_hasQuit=true;
			//If thread is in waiting on wait event, wake it can quit gracefully.
			Wakeup();
			try {
				Join(timeoutMS);//Wait for allotted time before throwing ThreadAbortException.
			}
			catch {
				//Guards against re-entrance into this function just in case the main thread called QuitSyncAllOdThreads() and this thread itself called QuitSync() at the same time.
				//This will be very rare and if we get to this point, we know that the thread has already been joined or aborted and thus has already finished doing work so it is fine to remove.
			}
			finally {
				lock(_lockObj) {
					_listOdThreads.Remove(this);
				}
			}
		}

		///<summary>Asynchronously quits all threads that have the passed in group name.
		///Optionally have this quit method remove the threads from the global list of threads.</summary>
		public static void QuitAsyncThreadsByGroupName(string groupName,bool doRemoveThreads=false) {
			List<ODThread> listThreadsForGroup=GetThreadsByGroupName(groupName);
			for(int i=0;i<listThreadsForGroup.Count;i++) { //Quit all threads in parallel so our wait times are not cummulative.
				listThreadsForGroup[i].QuitAsync(doRemoveThreads);//Do not remove threads from global list so that the Join can have access to them.
			}
		}

		///<summary>Waits for ALL threads in the group to finish doing work before returning.  Each thread will be given the timeoutMS to quit.  Try to keep in mind how many threads are going to be quitting when setting the milliseconds for the timeout.  If the thread has been forgotten, it will be forcefully quit on closing of the main application.  Removes all threads from the global list of ODThreads after the threads have quit.</summary>
		public static void QuitSyncThreadsByGroupName(int timeoutMS,string groupName) {
			QuitAsyncThreadsByGroupName(groupName);
			//Wait for all threads to end or timeout, whichever comes first.
			JoinThreadsByGroupName(timeoutMS,groupName,true);
		}

		///<summary>Should only be called when the main application is closing.  Loops through ALL ODThreads that are still running and aborts them instantly.  If you want to give each thread a chance to gracefully quit, call QuitSyncThreadsByGroupName instead.</summary>
		public static void QuitSyncAllOdThreads() {
			QuitSyncThreadsByGroupName(0,"");
		}

		///<summary>Returns the specified group of threads in the same order they were created.  If groupName is empty, then returns the list of all current ODThreads.</summary>
		public static List<ODThread> GetThreadsByGroupName(string groupName) {
			List<ODThread> listOdThreadsForGroup=new List<ODThread>();
			lock(_lockObj) {
				for(int i=0;i<_listOdThreads.Count;i++) {
					if(groupName=="" || _listOdThreads[i].GroupName==groupName) {
						listOdThreadsForGroup.Add(_listOdThreads[i]);
					}
				}
			}
			return listOdThreadsForGroup;
		}

		///<summary>Add an exception handler to be alerted of unhandled exceptions in the work delegate.</summary>
		public void AddExceptionHandler(ExceptionDelegate exceptionHandler) {
			_exceptionHandler+=exceptionHandler;
		}

		///<summary>Add an exit handler that will get fired once the thread loop has exited.
		///Fires in the context of this thread not the context of the calling / creating thread.
		///Make sure to use Invoke or BeginInvoke if you are going to be manipulating UI elements from this handler.</summary>
		public void AddExitHandler(WorkerDelegate exitHandler) {
			_exitHandler+=exitHandler;
		}

		///<summary>Add a delegate that will get called before the main worker delegate starts. If this is a thread that runs repeatedly at an interval,
		///this delegate will only run before the first time the thread is run. It is implied that this is invoked from within the thread context.
		///Make sure to use Invoke or BeginInvoke if you are going to be manipulating UI elements from this handler.</summary>
		public void AddSetupHandler(WorkerDelegate setupHandler) {
			_setupHandler+=setupHandler;
		}

		///<summary>If the thread has not started, then returns 0.  If the thread has started but has not quit yet, then returns the amount of time which has elapsed since the thread was started.  If the thread has quit, returns the time elapsed between when the thread was started and when the thread was quit.</summary>
		public TimeSpan GetTimeElapsed() {
			if(_hasQuit) {
				return (_dateTimeQuit-_dateTimeStart);
			}
			else if(_dateTimeStart>DateTime.MinValue) {
				return (DateTime.Now-_dateTimeStart);
			}
			return TimeSpan.Zero;
		}

		public static bool MakeThread(IODThread threadClass,bool isAutoStart=false,string groupName="") {
			if(threadClass.IsInit) {
				return false;
			}
			ODThread thread=new ODThread(threadClass.GetThreadRunIntervalMS(),threadClass.OnThreadRun);
			thread.Name=threadClass.GetThreadName();
			if(groupName!="") {
				thread.GroupName=groupName;
			}
			thread.AddExceptionHandler(new ExceptionDelegate(threadClass.OnThreadException));
			thread.AddExitHandler(new WorkerDelegate(threadClass.OnThreadExit));
			if(isAutoStart) {
				thread.Start();
			}
			threadClass.IsInit=true;
			threadClass.ODThread=thread;
			return true;
		}

		///<summary>Spread the given actions over the given numThreads. Blocks until threads have completed or timeout is reached.
		///If numThreads is not provided then numThreads will default to Environment.ProcessorCount. This is typically what you should let happen.
		///If onException is provided then one and only one onException event will be raised when any number of exceptions occur.
		///All actions will run to completion regardless if any/all throw unhandled exceptions.
		///Throws exception on main thread if any action throws and unhandled exception and no onException was provided.</summary>
		public static void RunParallel(List<Action> listActions,TimeSpan timeout,int numThreads = 0,ExceptionDelegate onException=null) {
			//Use as many threads as required by default.
			int threadCount=numThreads;
			if(threadCount<=0) {
				//No requirement on thread count was given so use the number of processor cores.
				threadCount=Environment.ProcessorCount;
				//Using at least 8 has neglibile negative impact so if the user didn't otherwise specify, use at least 8 threads.
				if(threadCount<8) {
					threadCount=8;
				}
			}
			//Make a group of threads to spread out the workload.
			List<Action> listActionsCur=new List<Action>();
			Exception exceptionFirst=null;
			int actionsPerThread=(int)Math.Ceiling((double)listActions.Count/threadCount);
			object locker=new object();
			//No one outside of this method cares about this group name. They have no authority over this group.
			int threadID=1;
			string threadGroupGUID=Guid.NewGuid().ToString();
			string threadGroupName="ODThread.ThreadPool()"+threadGroupGUID;
			for(int i = 0;i<listActions.Count;i++) {
				//Add to the current thread pool.
				listActionsCur.Add(listActions[i]);
				//If this thread pool is full then start it.
				if(listActionsCur.Count==actionsPerThread||i==(listActions.Count-1)) {
					ODThread odThread=new ODThread(new WorkerDelegate((ODThread o) => {
						((List<Action>)o.Tag).ForEach(x => x());
					}));
					odThread.Tag=new List<Action>(listActionsCur);
					odThread.Name=threadGroupName+"-"+threadID;
					odThread.GroupName=threadGroupName;
					odThread.AddExceptionHandler(new ExceptionDelegate((Exception e) => {
						lock (locker) {
							if(exceptionFirst==null) { //First in wins.
								exceptionFirst=e;
							}
						}
					}));
					//We just started a new thread pool so start a new one.
					listActionsCur.Clear();
					odThread.Start(true);
					threadID++;
				}
			}
			//Wait for all appointment drawing threads to finish.
			JoinThreadsByGroupName((int)timeout.TotalMilliseconds,threadGroupName,true);
			//We are back on the thread that called us now.
			if(exceptionFirst!=null) { //One of the actions threw an unhandled exception.
				if(onException==null) { //No handler was provided so throw. 
					throw exceptionFirst;
				}
				//Caller wants to know about this exception so tell them.
				onException(exceptionFirst);
			}
		}

		///<summary>Program entry of any application using ODThread call this method and provide the Application.Run's form/control. 
		///Any unhandled exception originating from an ODThread will be passed along through this handler.
		///The handler instance is responsible for joining back to the main thread, reporting the error, and exiting the program. 
		///Failing to register here in your application will result in unhandled exceptions in ODThread killing your program without any on-screen feedback and
		///a vague event blaming KERNELBASE.dll will be posted to the event viewer.</summary>
		public static void RegisterForUnhandledExceptions(Control ctrlMainThread,Action<Exception,String> onException=null) {
			OnUnhandledException=new Action<Exception,Thread>((e,th) => {
				//BeginInvoke allows the thread to continue. Simply using Invoke() would cause a thread dead-lock.
				ctrlMainThread.BeginInvoke(() => {
					//We are back on the main thread and the offending ODThread is now continuing and exiting so it is safe to join here.
					th.Join();
					if(onException!=null) { //Registered application would like to handle this on their own.
						onException(e,th.Name);
						return;
					}
					new MsgBoxCopyPaste("A critical error has occurred. The program will now close. Please call support and report the following...\r\n\r\n"
						+"Error:\r\n"+e.Message+"\r\n\r\nStack Trace:\r\n"+e.StackTrace).ShowDialog();
					//Guaranteed to kill any threads which are still running. 999 means 'Unknown Problem' per the OD manual.
					Application.Exit();
				});
			});
		}

		///<summary>Pointer delegate to the method that does the work for this thread.  The worker method has to take an ODThread as a parameter so that it has access to Tag and other variables when needed.</summary>
		public delegate void WorkerDelegate(ODThread odThread);

		///<summary>Pointer delegate to the method that gets called when the worker delegate throws an unhandled exception.</summary>
		public delegate void ExceptionDelegate(Exception e);
	}

	///<summary>This interface is needed for the MakeThread method so that we don't have to copy additional files to the UpdateFileCopier project.</summary>
	public interface IODThread {

		ODThread ODThread {
			get;
			set;
		}

		bool IsInit {
			get;
			set;
		}

		int GetThreadRunIntervalMS();
		void OnThreadRun(ODThread odThread);
		void OnThreadException(Exception e);
		void OnThreadExit(ODThread odThread);
		string GetLogDirectoryName();
		string GetThreadName();
	}

}
