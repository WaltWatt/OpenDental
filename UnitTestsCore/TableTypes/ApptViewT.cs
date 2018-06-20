using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestsCore {
	public class ApptViewT {

		///<summary></summary>
		public static ApptView CreateApptView(string description,TimeSpan apptTimeScrollStart=new TimeSpan()) {
			ApptView apptView=new ApptView() {
				Description=description,
				ApptTimeScrollStart=apptTimeScrollStart,
			};
			ApptViews.Insert(apptView);
			ApptViews.RefreshCache();
			return apptView;
		}

		///<summary>Deletes everything from the apptview table.  Does not truncate the table so that PKs are not reused on accident.</summary>
		public static void ClearApptView() {
			string command="DELETE FROM apptview WHERE ApptViewNum > 0";
			DataCore.NonQ(command);
		}
	}
}
