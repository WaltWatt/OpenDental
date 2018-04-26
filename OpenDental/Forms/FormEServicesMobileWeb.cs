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

		private bool IsTabValidMobileWeb() {
			return true;
		}
		
		private void FillTabMobileWeb() {			
			string urlFromHQ=(
				WebServiceMainHQProxy.GetSignups<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService>(_signupOut,eServiceCode.MobileWeb).FirstOrDefault()??
				new WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService() { HostedUrl="" }
			).HostedUrl;
			textHostedUrlMobileWeb.Text=urlFromHQ;
		}

		private void SaveTabMobileWeb() {

		}
		
		private void butSetupMobileWebUsers_Click(object sender,EventArgs e) {
			FormOpenDental.S_MenuItemSecurity_Click(sender,e);
		}
	}
}
