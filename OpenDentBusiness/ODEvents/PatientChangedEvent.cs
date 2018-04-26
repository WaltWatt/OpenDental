using CodeBase;
using System;

namespace OpenDentBusiness {
	///<summary>The patient changed event will fire when FormOpenDental notices that the currently selected patient has changed.</summary>
	public class PatientChangedEvent {
		///<summary></summary>
		public static event ODEventHandler Fired;

		///<summary>This event class was designed for the ODEventArgs to have a name of "FormOpenDental" because that is who is supposed to be firing.
		///The Tag should be a long that represents the PatNum of the patient that has just been changed to.</summary>
		public static void Fire(ODEventArgs e) {
			if(Fired!=null) {
				Fired(e);
			}
		}
	}
}
