using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeBase;

namespace OpenDental {
	///<summary>An easy way to udpate a progress bar when performing long-running computations. When using this class, the long-running computation
	///should be on a thread that is not the main UI thread.  The thread which is not the main thread should not read/write to any UI elements,
	///or else an Invoke would be required and defeat the purpose.</summary>
	public class ODProgress:IODProgress {
		private FormProgressStatus _formProgress;
		private bool _doClose;

		[DefaultValue("ODProgress")]
		public string LanThis { get; set; }

		public ODProgress(string startingMessage="Please Wait...",ProgBarStyle progStyle=ProgBarStyle.Blocks) {
			_formProgress=new FormProgressStatus("",typeof(ODEvent),false,true);
			_formProgress.UpdateProgressDetailed(startingMessage,progStyle:progStyle);
		}

		///<summary>Runs the workerDelegate on a separate thread and displays a progress window while the thread is running.</summary>
		public static void ShowProgressForThread(ODThread.WorkerDelegate workerDelegate,Control parent,string startingMessage="Please Wait...",
			ProgBarStyle progStyle=ProgBarStyle.Blocks,string threadName="ODProgressThread",ODThread.ExceptionDelegate exceptionHandler=null) 
		{
			ODProgress prog=new ODProgress(Lan.g(parent,startingMessage),progStyle);
			ODThread thread=new ODThread(workerDelegate);
			if(exceptionHandler!=null) {
				thread.AddExceptionHandler(exceptionHandler);
			}
			thread.Name=threadName;
			thread.ProgressLog=prog;
			thread.Start(true);
			prog.ShowDialog(parent);
		}

		///<summary>Updates the status label.</summary>
		public void UpdateProgress(string message) {
			if(_formProgress.InvokeRequired) {
				_formProgress.BeginInvoke(() => UpdateProgress(message));
				return;
			}
			_formProgress.UpdateProgress(message);
		}

		///<summary>Updates the progress bar with these details.</summary>
		public void UpdateProgressDetailed(string labelValue,string percentVal="",string tagString="",int barVal=0,int barMax=100,int marqSpeed=0,
			string labelTop="",bool isLeftHidden=false,bool isTopHidden=false,bool isPercentHidden=false,
			ProgBarStyle progStyle=ProgBarStyle.Blocks,ProgBarEventType progEvent=ProgBarEventType.ProgressBar) 
		{
			if(_formProgress.InvokeRequired) {
				_formProgress.BeginInvoke(() => UpdateProgressDetailed(labelValue,percentVal,tagString,barVal,barMax,marqSpeed,labelTop,isLeftHidden,isTopHidden,
					isPercentHidden,progStyle,progEvent));
				return;
			}
			_formProgress.UpdateProgressDetailed(labelValue,percentVal,tagString,barVal,barMax,marqSpeed,labelTop,isLeftHidden,isTopHidden,isPercentHidden,
				progStyle,progEvent);
		}

		///<summary>Shows the form until someone else calls Close() on this object.</summary>
		public void ShowDialog(Control parent) {
			if(_doClose) {//The worker thread finished extremely fast and already called Close().
				return;
			}
			_formProgress.ShowDialog(parent);
		}

		///<summary>Closes out the form.</summary>
		public void Close() {
			if(_formProgress.InvokeRequired) {
				_formProgress.Invoke(() => Close());//Not using BeginInvoke() because we want the form to be fully closed before proceeding
				return;
			}
			if(!_formProgress.IsDisposed) {
				if(_formProgress.IsHandleCreated) {
					_formProgress.Close();
				}
				else {
					//ShowDialog() has not been called yet.
					_doClose=true;
				}
			}
		}
		
	}
}
