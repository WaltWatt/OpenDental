using CodeBase;
using System;

namespace OpenDentBusiness {
	///<summary></summary>
	public class SplashProgressEvent {
		///<summary></summary>
		public static event ODEventHandler Fired;

		///<summary></summary>
		public static void Fire(ODEventArgs e) {
			if(Fired!=null) {
				Fired(e);
			}
		}
	}
}
