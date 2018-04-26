using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebServiceSerializer;

namespace OpenDentBusiness {
	public class WebServiceMainHQMockDemo : OpenDentBusiness.WebServiceMainHQ.WebServiceMainHQ, IWebServiceMainHQ {
		public List<long> GetEServiceClinicsAllowed(List<long> listClinicNums,eServiceCode eService) {
			throw new NotImplementedException();
		}

		public new string EServiceSetup(string officeData) {
			try {
				WebServiceMainHQProxy.EServiceSetup.SignupOut signupOut=new WebServiceMainHQProxy.EServiceSetup.SignupOut() {
					EServices=GetEServicesForAll(),
					HasClinics=PrefC.HasClinicsEnabled,
					ListenerTypeInt=(int)ListenerServiceType.ListenerServiceProxy,
					MethodNameInt=(int)WebServiceMainHQProxy.EServiceSetup.SetupMethod.GetSignupOutFull,
					Phones=GetPhonesForAll(),
					Prompts=new List<string>(),
					SignupPortalPermissionInt=(int)SignupPortalPermission.FullPermission,
					SignupPortalUrl=GetHostedUrlForCode(eServiceCode.SignupPortal),
				};
				//Write the response out as a plain string. We will deserialize it on the other side.
				return WebSerializer.SerializePrimitive<string>(WebServiceMainHQProxy.WriteXml(signupOut));
			}
			catch(Exception ex) {
				StringBuilder strbuild=new StringBuilder();
				using(XmlWriter writer=XmlWriter.Create(strbuild,WebServiceMainHQProxy.CreateXmlWriterSettings(true))) {
					writer.WriteStartElement("Response");
					writer.WriteStartElement("Error");
					writer.WriteString(ex.Message);
					writer.WriteEndElement();
					writer.WriteEndElement();
				}
				return strbuild.ToString();
			}
		}

		///<summary>Returns all possible eServices for every clinic in the database.</summary>
		private List<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService> GetEServicesForAll() {
			if(PrefC.HasClinicsEnabled) {
				List<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService> listEServices
					=new List<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService>();
				foreach(Clinic clinic in Clinics.GetDeepCopy(true)) {
					listEServices.AddRange(GetEServicesForClinic(clinic.ClinicNum));
				}
				return listEServices;
			}
			else {
				return GetEServicesForClinic(0);
			}
		}

		///<summary>Returns all possible eServices for the clinic passed in.</summary>
		private List<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService> GetEServicesForClinic(long clinicNum=0) {
			return new List<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService>() {
				//GetEServiceForCode(eServiceCode.BugSubmission,clinicNum),
				GetEServiceForCode(eServiceCode.Bundle,clinicNum),
				GetEServiceForCode(eServiceCode.ConfirmationRequest,clinicNum),
				//GetEServiceForCode(eServiceCode.FeaturePortal,clinicNum),
				//GetEServiceForCode(eServiceCode.FHIR,clinicNum),
				//GetEServiceForCode(eServiceCode.HQManager,clinicNum),
				//GetEServiceForCode(eServiceCode.HQProxyService,clinicNum),

				//If you need to test texting signup, uncomment the following lines. Note: this will modify your db (clinics and texting prefs).
				//GetEServiceForCode(eServiceCode.IntegratedTexting,clinicNum),
				//GetEServiceForCode(eServiceCode.IntegratedTextingUsage,clinicNum),

				//GetEServiceForCode(eServiceCode.ListenerService,clinicNum),
				GetEServiceForCode(eServiceCode.MobileWeb,clinicNum),
				//GetEServiceForCode(eServiceCode.OAuth,clinicNum),
				GetEServiceForCode(eServiceCode.PatientPortal,clinicNum),
				GetEServiceForCode(eServiceCode.PatientPortalMakePayment,clinicNum),
				GetEServiceForCode(eServiceCode.PatientPortalViewStatement,clinicNum),
				//GetEServiceForCode(eServiceCode.ResellerPortal,clinicNum),
				//GetEServiceForCode(eServiceCode.ResellerSoftwareOnly,clinicNum),
				GetEServiceForCode(eServiceCode.SignupPortal,clinicNum),
				GetEServiceForCode(eServiceCode.SoftwareUpdate,clinicNum),
				//GetEServiceForCode(eServiceCode.Undefined,clinicNum),
				GetEServiceForCode(eServiceCode.WebForms,clinicNum),
				GetEServiceForCode(eServiceCode.WebSched,clinicNum),
				GetEServiceForCode(eServiceCode.WebSchedASAP,clinicNum),
				GetEServiceForCode(eServiceCode.WebSchedNewPatAppt,clinicNum),
			};
		}

		private WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService GetEServiceForCode(eServiceCode code,long clinicNum=0) {
			if(code==eServiceCode.IntegratedTexting) {
				return new WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutSms() {
					ClinicNum=clinicNum,
					EServiceCodeInt=(int)code,
					HostedUrl=GetHostedUrlForCode(code),
					HostedUrlPayment="http://debug.hosted.url.payment",//TODO: no idea what to do here.
					IsEnabled=true,
					CountryCode="US",
					MonthlySmsLimit=20,
					SmsContractDate=DateTime.Today.AddYears(-1),
				};
			}
			return new WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService() {
				ClinicNum=clinicNum,
				EServiceCodeInt=(int)code,
				HostedUrl=GetHostedUrlForCode(code),
				HostedUrlPayment="http://debug.hosted.url.payment",//TODO: no idea what to do here.
				IsEnabled=true,
			};
		}

		private string GetHostedUrlForCode(eServiceCode code) {
			switch(code) {
				case(eServiceCode.MobileWeb):
					return "http://127.0.0.1:5000/MobileWeb.html";
				case (eServiceCode.PatientPortal):
				case (eServiceCode.PatientPortalMakePayment):
				case (eServiceCode.PatientPortalViewStatement):
					return "http://127.0.0.1:4000/PatientPortal.html";
				case (eServiceCode.SignupPortal):
					return "http://127.0.0.1:8888/SignupPortal";
				case (eServiceCode.WebForms):
					return "http://127.0.0.1:3000/WebForms.html";
				case (eServiceCode.WebSched):
				case (eServiceCode.WebSchedASAP):
				case (eServiceCode.WebSchedNewPatAppt):
					return "http://127.0.0.1:8000/WebSched.html";
				default:
					return "";
			}
		}

		private List<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutPhone> GetPhonesForAll() {
			//This will cause the local debug db not to sync the SmsPhone table. This just means that SmsPhone table can't be trusted in debug mode.
			//If you need to test "real" VLNs, just make a list here that includes known VLNs that are actually owned by HQ for debugging.
			return new List<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutPhone>();
		}

	}
}
