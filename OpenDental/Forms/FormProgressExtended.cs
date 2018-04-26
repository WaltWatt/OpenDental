using CodeBase;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;

namespace OpenDental {
	///<summary>Do not instatiate this class.  It is not meant for public use.  Use ODProgress.ShowProgressExtended() instead.
	///Launch this window in a separate thread so that the progress bar can smoothly spin without waiting on the main thread.
	///Send the phrase "DEFCON 1" in order to have the window gracefully close (as to not rely on thread abort).
	///FormProgressExtended is extremely tailored to monitoring the progress of FormBilling.  Much will need to change about this to make it generic.</summary>
	public partial class FormProgressExtended:Form {
		private string _odEventName;
		private Type _eventType;
		///<summary>Progress bars that have already been added to the form.</summary>
		private List<ODProgressBar> _listProgressBars=new List<ODProgressBar>();
		///<summary>Event is fired when the user clicks the pause/resume button.</summary>
		public event ProgressPausedHandler ProgressPaused=null;
		///<summary>Event is fired when the user clicks the cancel button.</summary>
		public event ProgressCanceledHandler ProgressCanceled=null;
		///<summary>True if the user clicked Pause, set back to false when the user clicks Resume.</summary>
		private bool _isPaused;
		/// <summary>True if progress has been completed and the Done event has been called. </summary>
		private bool _isDone;

		///<summary>Do not instatiate this class.  It is not meant for public use.  Use ODProgress.ShowProgressExtended() instead.</summary>
		public FormProgressExtended(string odEventName) : this(odEventName,typeof(ODEvent),false) {

		}

		///<summary>Do not instatiate this class.  It is not meant for public use.  Use ODProgress.ShowProgressExtended() instead.
		///Launches a progress window that will constantly spin and display status updates for global ODEvents with corresponding name.
		///eventType must be a Type that contains an event called Fired.</summary>
		public FormProgressExtended(string odEventName,Type eventType,bool hasHistory=false) {
			InitializeComponent();
			Lan.F(this);
			_odEventName=odEventName;
			_eventType=eventType;
			//Registers this form for any progress status updates that happen throughout the entire program.
			EventInfo fireEvent;
			try {
				fireEvent=eventType.GetEvent("Fired");
			}
			catch(Exception) {
				fireEvent=typeof(ODEvent).GetEvent("Fired");
			}
			Type fireEventType = fireEvent.EventHandlerType;
			Delegate fireDelegate = Delegate.CreateDelegate(fireEventType, this, "ODEvent_Fired");
			MethodInfo addHandler = fireEvent.GetAddMethod();
			Object[] addHandlerArgs = { fireDelegate };
			addHandler.Invoke(this, addHandlerArgs);
		}

		///<summary>Creates a new progress bar and adds it to the form.</summary>
		private ODProgressBar AddNewProgressBar(string leftLabel,string topLabel,string rightLabel,int blockValue,int blockMax,string tagString, 
			ProgBarStyle progStyle,int marqSpeed,bool isLeftHidden,bool isTopHidden,bool isRightHidden) 
		{
			if(_listProgressBars.Count>10) {
				MsgBox.Show(this,"Cannot have more than 10 progress bars on window.");
				return null;
			}
			ODProgressBar pbar=new ODProgressBar(leftLabel,topLabel,rightLabel,blockValue,blockMax,tagString,progStyle,marqSpeed,isLeftHidden,isTopHidden,
				isRightHidden);
			int rowLocation=tableLayoutPanel1.RowCount;
			tableLayoutPanel1.Controls.Add(pbar,1,rowLocation);
			pbar.Name="pbar"+rowLocation;
			return pbar;
		}

		private void UpdateProgressBar(ODProgressBar progBar, ProgressBarHelper progHelper) {
			progBar.ODProgUpdate(progHelper.LabelValue, progHelper.LabelTop, progHelper.PercentValue, progHelper.BlockValue,progHelper.BlockMax, 
				progHelper.TagString,progHelper.ProgressStyle,progHelper.MarqueeSpeed,progHelper.IsValHidden,progHelper.IsTopHidden,progHelper.IsPercentHidden);
		}

		public void ODEvent_Fired(ODEventArgs e) {
			//We don't know what thread will cause a progress status change, so invoke this method as a delegate if necessary.
			if(this.InvokeRequired) {
				this.Invoke((Action)delegate() { ODEvent_Fired(e); });
				return;
			}
			//Make sure that this ODEvent is for FormProgressExtended and that the Tag is not null and is a string.
			if(e.Name!=_odEventName || e.Tag==null) {
				return;
			}
			ProgressBarHelper progHelper=new ProgressBarHelper("");
			bool hasProgHelper=false;
			string status="";
			if(e.Tag.GetType()==typeof(string)) {
				status=((string)e.Tag);
			}
			else if(e.Tag.GetType()==typeof(ProgressBarHelper)) {
				progHelper=(ProgressBarHelper)e.Tag;
				status=progHelper.LabelValue;
				hasProgHelper=true;
			}
			else {//Unsupported type passed in.
				return;
			}
			//When the developer wants to close the window, they will send an ODEvent with "DEFCON 1" to signal this form to shut everything down.
			if(status.ToUpper()=="DEFCON 1") {
				DialogResult=DialogResult.OK;
				Close();
				return;
			}
			if(hasProgHelper) {
				switch(progHelper.ProgressBarEventType) {
					case ProgBarEventType.BringToFront:
						this.TopMost=true;
						this.TopMost=false;
						break;
					case ProgBarEventType.Header:
						this.Text=status;
						break;
					case ProgBarEventType.ProgressLog:
						label4.Text=status;
						break;
					case ProgBarEventType.TextMsg:
						status=status.Trim();//Get rid of leading and trailing new lines.
						textMsg.AppendText((string.IsNullOrWhiteSpace(textMsg.Text) ? "" : "\r\n")+status.PadRight(60));
						break;
					case ProgBarEventType.WarningOff:
					case ProgBarEventType.AllowResume:
						labelWarning.Visible=false;
						butPause.Enabled=true;
						butPause.Text=Lan.g(this,"Resume");
						break;
					case ProgBarEventType.Done:
						butCancel.Visible=true; //can sometimes be set to invisible with HideButtons. Show if previously invisible.
						butCancel.Text=Lan.g(this,"Close");
						butPause.Enabled=false;
						_isDone=true;
						break;
					case ProgBarEventType.HideButtons://hide pause and cancel. Call Done when "close button" should appear. 
						butPause.Visible=false;
						butCancel.Visible=false;
						break;
					case ProgBarEventType.ProgressBar:
					default:
						if(!_listProgressBars.Exists(x => x.TagString.ToLower()==progHelper.TagString.ToLower())) {//if not already added
							ODProgressBar progBar=AddNewProgressBar(progHelper.LabelValue,progHelper.LabelTop,progHelper.PercentValue,progHelper.BlockValue,
								progHelper.BlockMax,progHelper.TagString,progHelper.ProgressStyle,progHelper.MarqueeSpeed,progHelper.IsValHidden,progHelper.IsTopHidden,
								progHelper.IsPercentHidden);
							if(progBar==null) {
								break;
							}				
							_listProgressBars.Add(progBar);
						}
						else {
							ODProgressBar odBar=_listProgressBars.Find(x => x.TagString.ToLower()==progHelper.TagString.ToLower());
							UpdateProgressBar(odBar,progHelper);
						}
						break;
				}
			}
			Application.DoEvents();//So that the label updates with the new status.
		}

		private void butPause_Click(object sender,EventArgs e) {
			_isPaused=(!_isPaused);
			ProgressPaused?.Invoke(this,new ProgressPausedArgs(_isPaused));
			if(_isPaused) {
				butPause.Text=Lan.g(this,"Resume");
				//We are not going to allow the user to immediately unpause because we want to allow the instantiator of this form to get itself into a state
				//where it can be unpaused. When it is ready to be unpaused, it will fire an event with the tag "AllowResume".
				butPause.Enabled=false;
				labelWarning.Visible=true;
			}
			else {
				butPause.Text=Lan.g(this,"Pause");
			}
		}

		private void butCancel_Click(object sender,EventArgs e) {//also can change to 'close'
			ProgressCanceled?.Invoke(this,new EventArgs());
			//disable button until window is ready to cancel and close
			if(_isDone) {
				DialogResult=DialogResult.OK;
			}
			//process has not been completed. Wait to finish current process and then have instantiator close.
			else {
				butCancel.Enabled=false;
				labelWarning.Visible=true; 
			}
		}

		private void FormProgressExtended_FormClosing(object sender,FormClosingEventArgs e) {
			EventInfo fireEvent;
			try {
				fireEvent=_eventType.GetEvent("Fired");
			}
			catch(Exception) {
				fireEvent=typeof(ODEvent).GetEvent("Fired");
			}
			Type fireEventType = fireEvent.EventHandlerType;
			Delegate fireDelegate = Delegate.CreateDelegate(fireEventType, this, "ODEvent_Fired");
			MethodInfo removeHandler = fireEvent.GetRemoveMethod();
			Object[] removeHandlerArgs = { fireDelegate };
			removeHandler.Invoke(this, removeHandlerArgs);
		}	


	}//End FormProgressExtended

	public delegate void ProgressCanceledHandler(object sender,EventArgs e);
	public delegate void ProgressPausedHandler(object sender,ProgressPausedArgs e);

	public class ProgressPausedArgs {
		public bool IsPaused;
		public ProgressPausedArgs(bool isPaused) {
			IsPaused=isPaused;
		}
	}
}

