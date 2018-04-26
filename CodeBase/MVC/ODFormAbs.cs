using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CodeBase.MVC {
	///<summary>Base class that all forms should extend.  Provides accessibility to features for all forms like Help and object processing.</summary>
	///<typeparam name="P">Processor Type - Typically set to Signalod but can be set to any object type that gets processed by the form.</typeparam>
	public class ODFormAbs<P> : Form, IODProcessor<P> {
		///<summary>Override and set to false if you want your form not to respond to F1 key for help.</summary>
		protected virtual bool HasHelpKey {
			get {
				return true;
			}
		}

		public ODFormAbs() {
			#region Designer Properties
			this.AutoScaleMode=AutoScaleMode.None;
			this.ClientSize=new System.Drawing.Size(974,696);
			this.KeyPreview=true;
			this.MinimumSize=new System.Drawing.Size(100,100);
			this.Name="ODFormAbs";
			this.StartPosition=FormStartPosition.CenterScreen;
			this.Text="ODFormAbs";
			this.KeyUp+=new KeyEventHandler(this.ODFormAbs_KeyUp);
			#endregion
		}

		///<summary>Sets the entire form into "read only" mode by disabling all controls on the form.
		///Pass in any controls that should say enabled (e.g. Cancel button). 
		///This can be used to stop users from clicking items they do not have permission for.</summary>
		public void DisableForm(params Control[] enabledControls) {
			foreach(Control ctrl in this.Controls) {
				if(enabledControls.Contains(ctrl)) {
					continue;
				}
				//Attempt to disable the control.
				try {
					ctrl.Enabled=false;
				}
				catch(Exception ex) {
					//Some controls do not support being disabled.  E.g. the WebBrowser control will throw an exception here.
					ex.DoNothing();
				}
			}
		}

		private void ODFormAbs_KeyUp(object sender,KeyEventArgs e) {
			if(e.KeyCode!=Keys.F1) {
				return;
			}
			if(!HasHelpKey) { //Not all forms want to offer help as an option. Some may be using F1 for other purposes.
				return;
			}
			try {
				ShowHelp(((Form)sender).Name);
			}
			catch(Exception ex) {
				MessageBox.Show(this,ex.Message);
			}
		}

		///<summary>Override this if your form cares about showing help when the user presses the F1 key.</summary>
		public virtual void ShowHelp(string name) { }

		///<summary>Base handler for the IODProcessor interface. Wrap it with logging and callback to OnProcess().</summary>
		public void ProcessObjects(List<P> listObjs) {
			Logger.LogAction("ODFormAbs.ProcessObjects",LogPath.Signals,() => OnProcessObjects(listObjs),this.GetType().Name);
		}

		///<summary>Override this if your form cares about object processing.</summary>
		public virtual void OnProcessObjects(List<P> listObjs) {
		}
	}
}
