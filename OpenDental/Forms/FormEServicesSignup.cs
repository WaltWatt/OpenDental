using CodeBase;
using Microsoft.Win32;
using OpenDental.UI;
using OpenDentBusiness;
using OpenDentBusiness.Mobile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Globalization;
using System.Data;
using System.Linq;
using System.IO;
using WebServiceSerializer;
using OpenDentBusiness.WebServiceMainHQ;
using OpenDentBusiness.WebTypes.WebSched.TimeSlot;

namespace OpenDental {
	public partial class FormEServicesSetup {
		private bool IsTabValidSignup() {
			return true;
		}

		private void FillTabSignup() {
			ODException.SwallowAnyException(() => {
#if DEBUG
				_signupOut.SignupPortalUrl=_signupOut.SignupPortalUrl.Replace("https://www.patientviewer.com/SignupPortal/GWT/SignupPortal/SignupPortal.html","http://127.0.0.1:8888/SignupPortal.html");
#endif
				webBrowserSignup.Navigate(_signupOut.SignupPortalUrl);
			});
		}

		private void SaveTabSignup() {

		}

		private void AuthorizeTabSignup(bool allowEdit) {

		}
	}
}
