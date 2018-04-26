using System;
using OpenDentBusiness;
using System.Windows.Forms;
using System.Reflection;
using CodeBase;
using System.Globalization;
using System.IO;
using System.Drawing;
using System.Threading;

namespace OpenDental {
	///<summary></summary>
	public partial class FormSplash:Form
  {
		private string _odEventName;
    ///<summary></summary>
    public FormSplash() {
			InitializeComponent();
			progressBar.Hide();
			labelProgress.Hide();
			this.ClientSize=new Size(500,300);
    }
				
		///<summary>Do not instatiate this class.  It is not meant for public use.  Use ODProgressWindow.ShowSplash() instead.
		///Launches a splash screen with a progress bar that will display status updates for global ODEvents with the corresponding name.</summary>
		public FormSplash(string eventName){
			InitializeComponent();
			Lan.F(this);
			_odEventName=eventName;
			//Registers this form for any progress status updates that happen throughout the entire program.
			SplashProgressEvent.Fired+=ODEvent_Fired;
		}

    private void FormSplash_Load(object sender, EventArgs e) {
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				BackgroundImage=Properties.Resources.splashCanada;
			}
			if(File.Exists(Directory.GetCurrentDirectory()+@"\Splash.jpg")) {
				BackgroundImage=new Bitmap(Directory.GetCurrentDirectory()+@"\Splash.jpg");
			}
			if(Plugins.PluginsAreLoaded) {
				Plugins.HookAddCode(this,"FormSplash.FormSplash_Load_end");
			}
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
			labelProgress.Text=status+"... ("+progHelper.PercentValue+")";
			if(hasProgHelper) {
				if(progHelper.BlockMax!=0) {
					progressBar.Maximum=progHelper.BlockMax;
				}
				if(progHelper.BlockValue!=0) {
					//When the progress bar draws itself it gradually fills in the bar. This causes the progress bar to be much further behind the percent
					//label when the program loads quickly. A trick to get around this is to set the value and then set the value to a lower value.
					progressBar.Value=progHelper.BlockValue;
					progressBar.Value=Math.Max(progHelper.BlockValue-1,0);
					progressBar.Value=progHelper.BlockValue;
				}
			}
			Application.DoEvents();//So that the label updates with the new status.
		}

		private void FormSplash_FormClosing(object sender,FormClosingEventArgs e) {
			SplashProgressEvent.Fired-=ODEvent_Fired;
		}
  }
}