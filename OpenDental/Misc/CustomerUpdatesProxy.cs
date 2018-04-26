using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using OpenDental.customerUpdates;
using OpenDentBusiness;

namespace OpenDental {
	public class CustomerUpdatesProxy {

		/// <summary>Get an instance of OpenDental.customerUpdates.Service1 (referred to as 'Customer Updates Web Service'. Also sets IWebProxy and ICredentials if specified for this customer. Service1 is ready to use on return.</summary>
		public static Service1 GetWebServiceInstance() {
			Service1 ws=new Service1();
			ws.Url=PrefC.GetString(PrefName.UpdateServerAddress);
			//Uncomment this block if you want to test new web service funcionality on localhost. 
			//Use .\Development\Shared Projects Subversion\WebServiceCustomerUpdates solution to attach debugger to process 'ASP .NET Development Server - Port 3824'.
//#if DEBUG
//			ws.Url=@"http://localhost:3824/Service1.asmx";
//			ws.Timeout=(int)TimeSpan.FromMinutes(20).TotalMilliseconds;
//#endif
			if(PrefC.GetString(PrefName.UpdateWebProxyAddress) !="") {
				IWebProxy proxy = new WebProxy(PrefC.GetString(PrefName.UpdateWebProxyAddress));
				ICredentials cred=new NetworkCredential(PrefC.GetString(PrefName.UpdateWebProxyUserName),PrefC.GetString(PrefName.UpdateWebProxyPassword));
				proxy.Credentials=cred;
				ws.Proxy=proxy;
			}
			return ws;
		}		
	}
}
