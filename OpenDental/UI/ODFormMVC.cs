using CodeBase.MVC;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenDental {
	///<summary>All new-style OD forms extend this class. It fulfills an explicit MVC relationship for the extending class.
	///This class is a sibling to ODFormMVC. It does help and signal processing in addition to MVC.
	///Forms cannot directly extend this class but need to extend an intermediate class so that the Visual Studio designer does not have an error.
	///See https://stackoverflow.com/a/33112824 for details regarding why the designer cannot show forms that extend classes with generics.</summary>
	public class ODFormMVC<M,V,C> : ODViewAbs<M,V,C,Signalod>, ISignalProcessor where M : ODModelAbs<M> where C : ODControllerAbs<V> {

		public ODFormMVC() {
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
