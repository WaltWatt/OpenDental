using CodeBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness.WebServices {
	public class OpenDentalServerProxy {
		public static OpenDentalServerMockIIS MockOpenDentalServerCur {
			private get;//Use GetOpenDentalServerInstance() instead.
			set;
		}

		public static IOpenDentalServer GetOpenDentalServerInstance() {
			if(MockOpenDentalServerCur!=null) {
				return MockOpenDentalServerCur;
			}
			OpenDentalServerReal service=new OpenDentalServerReal();
			service.Url=RemotingClient.ServerURI;
			if(RemotingClient.MidTierProxyAddress!=null && RemotingClient.MidTierProxyAddress!="") {
				IWebProxy proxy=new WebProxy(RemotingClient.MidTierProxyAddress);
				ICredentials cred=new NetworkCredential(RemotingClient.MidTierProxyUserName,RemotingClient.MidTierProxyPassword);
				proxy.Credentials=cred;
				service.Proxy=proxy;
			}
			//The default useragent is
			//Mozilla/4.0 (compatible; MSIE 6.0; MS Web Services Client Protocol 4.0.30319.296)
			//But DHS firewall doesn't allow that.  MSIE 6.0 is probably too old, and their firewall also looks for IE8Mercury.
			service.UserAgent="Mozilla/4.0 (compatible; MSIE 7.0; MS Web Services Client Protocol 4.0.30319.296; IE8Mercury)";
			return service;
		}

	}
}
