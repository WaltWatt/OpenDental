using CodeBase;
using System;

namespace OpenDentBusiness {
	///<summary>Specific ODEvent for when communication to the database is unavailable.</summary>
	public class CommItemSaveEvent {
		///<summary>This event will get fired whenever communication to the database is attempted and fails.</summary>
		public static event ODEventHandler Fired;

		///<summary>Call this method only when communication to the database is not possible.</summary>
		public static void Fire(ODEventArgs e) {
			if(Fired!=null) {
				Fired(e);
			}
		}
	}
}
