using System;
using CodeBase;

namespace OpenDentBusiness {
	///<summary>Specific ODEvent for when a logged in user's credentials fail after they've been logged in successfully.</summary>
	public class CredentialsFailedAfterLoginEvent {
		///<summary>This event will get fired whenever a user is logged in and after successfully logging in their username or password fail running a
		///middle tier request.</summary>
		public static event ODEventHandler Fired;

		///<summary>Call this method only when a user's credentials are used to login successfully, but then fail on a subsequent middle tier request.</summary>
		public static void Fire(ODEventArgs e) {
			if(Fired!=null) {
				Fired(e);
			}
		}
	}
}
