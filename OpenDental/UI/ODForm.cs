using CodeBase.MVC;
using OpenDentBusiness;
using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace OpenDental {
	///<summary>All old-style OD forms extend this class. There is no explicit or implicit MVC relationship required to use this class.
	///This class is a sibling to ODFormMVC. It does help and signal processing without MVC.</summary>
	public class ODForm : ODFormAbs<Signalod>, ISignalProcessor {

		public ODForm() {
			this.Shown+=new EventHandler((o,e) => {
				Signalods.SubscribeSignalProcessor(this);
			});
		}

		public override void ShowHelp(string name) {
			try {
				OpenDentalHelp.ODHelp.ShowHelp(name);
			}
			catch(Exception ex) {
				MessageBox.Show(this,ex.Message);
			}
		}

		///<summary>Override this if your form cares about signal processing.</summary>
		public virtual void OnProcessSignals(List<Signalod> listObjs) {

		}

		///<summary>Seal OnProcessObjects because it is too vague and our engineers are already used to overriding OnProcessSignals.</summary>
		public sealed override void OnProcessObjects(List<Signalod> listObjs) {
			OnProcessSignals(listObjs);
		}

	}
}