using CodeBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenDental {
	///<summary>There are many moving pieces to the lifespan of a job.  This is a centralized class that helps handle all the job events.</summary>
	public class JobHandler {
		///<summary>The event that all job windows and controls should register for.
		///This will be the event that fires when something related to the job system changes.</summary>
		public static event ODEventHandler JobFired;

		///<summary>Triggers the JobFired event to get called with the passed in ODEventArgs.</summary>
		public static void Fire(ODEventArgs e) {
			if(JobFired!=null) {
				JobFired(e);
			}
		}
	}
}
