using CodeBase;
using System;

namespace OpenDentBusiness {
	///<summary></summary>
	public class ClearinghouseEvent:IODEvent {
		///<summary></summary>
		public const string Name="ClearinghouseEvent";
		///<summary></summary>
		public static event ODEventHandler Fired;

		///<summary></summary>
		public static void Fire(ODEventArgs e) {
			if(Fired!=null) {
				Fired(e);
			}
		}

		public void FireEvent(ODEventArgs e) {
			Fire(e);
		}
	}
}
