using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using WebServiceSerializer;
using CodeBase;
using System.IO;

namespace OpenDentBusiness {
	public static class WebServiceMainHQProxy {

		public static IWebServiceMainHQ MockWebServiceMainHQ {
			private get;//Use GetWebServiceMainHQInstance()
			set;
		}
		
		///<summary>Get an instance of the WebServicesHQ web service which includes the URL (pulled from PrefC). 
		///Optionally, you can provide the URL. This option should only be used by web apps which don't want to cause a call to PrefC.
		///Currently, OpenDentalREST and WebHostSynch hard-code the URL.</summary>
		public static IWebServiceMainHQ GetWebServiceMainHQInstance(string webServiceHqUrl="") {
			if(MockWebServiceMainHQ!=null) {
				return MockWebServiceMainHQ;
			}
			WebServiceMainHQReal service=new WebServiceMainHQReal();
#if ALPHA
			//Most beta applications should still set PrefName.WebServiceHQServerURL to the alpha address. This just makes it more fool proof for Alpha compiled applications.
			service.Url="http://10.10.1.184:49999/alpha/opendentalwebservicehq/webservicemainhq.asmx";
			return service;
#endif
			if(string.IsNullOrEmpty(webServiceHqUrl)) { //Default to the production URL.				
				service.Url=PrefC.GetString(PrefName.WebServiceHQServerURL);
			}
			else { //URL was provided so use that.
				service.Url=webServiceHqUrl;
			}
#if DEBUG
			//Change arguments for debug only.
			//service.Url="http://localhost/OpenDentalWebServiceHQ/WebServiceMainHQ.asmx";//localhost
			service.Url="http://chrism/OpenDentalWebServiceHQ/WebServiceMainHQ.asmx"; //Chris has most graciously volunteered to host the test serviceshq web service and db.
			//service.Url="http://10.10.2.18:55018/OpenDentalWebServiceHQ/WebServiceMainHQ.asmx";//Sam's Computer
			//service.Url="http://server184:49999/OpenDentalWebServiceHQ/WebServiceMainHQ.asmx";//The actual server hosting WebServiceMainHQ.
			//service.Url="http://sam/OpenDentalWebServiceHQ/WebServiceMainHQ.asmx"; //Sams computer
			service.Timeout=(int)TimeSpan.FromMinutes(60).TotalMilliseconds;
#endif
			return service;
		}
		
		///<summary>Any calls to WebServiceMainHQ must go through this method. The payload created here will be digested and extracted to OpenDentalWebServiceHQ.PayloadArgs.
		///If any args are null or not provided then they will be retrieved from Prefs.</summary>
		///<param name="registrationKey">An Open Dental distributed registration key that HQ has on file.  Do not include hyphens.</param>
		///<param name="practiceTitle">Any string is acceptable.</param>
		///<param name="practicePhone">Any string is acceptable.</param>
		///<param name="programVersion">Typically major.minor.build.revision.  E.g. 12.4.58.0</param>
		///<param name="payloadContentxAsXml">Use CreateXmlWriterSettings(true) to create your payload xml. Outer-most xml element MUST be labeled 'Payload'.</param>
		///<param name="serviceCode">Used on case by case basis to validate that customer is registered for the given service.</param>
		///<returns>An XML string that can be passed into a WebServiceHQ web method.</returns>
		public static string CreateWebServiceHQPayload(
			string payloadContentxAsXml,eServiceCode serviceCode,string registrationKey=null,string practiceTitle=null,string practicePhone=null,string programVersion=null) 
		{
			StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer=XmlWriter.Create(strbuild,CreateXmlWriterSettings(false))) {
				writer.WriteStartElement("Request");
				writer.WriteStartElement("Credentials");
				writer.WriteStartElement("RegistrationKey");
				writer.WriteString(registrationKey??PrefC.GetString(PrefName.RegistrationKey));
				writer.WriteEndElement();
				writer.WriteStartElement("PracticeTitle");
				writer.WriteString(practiceTitle??PrefC.GetString(PrefName.PracticeTitle));
				writer.WriteEndElement();
				writer.WriteStartElement("PracticePhone");
				writer.WriteString(practicePhone??PrefC.GetString(PrefName.PracticePhone));
				writer.WriteEndElement();
				writer.WriteStartElement("ProgramVersion");
				writer.WriteString(programVersion??PrefC.GetString(PrefName.ProgramVersion));
				writer.WriteEndElement();
				writer.WriteStartElement("ServiceCode");
				writer.WriteString(serviceCode.ToString());
				writer.WriteEndElement();
				writer.WriteEndElement(); //Credentials
				writer.WriteRaw(payloadContentxAsXml);
				writer.WriteEndElement(); //Request
			}
			return strbuild.ToString();
		}

		public static string CreateWebServiceHQWebHostSynchPayload(string payloadContentxAsXml,string registrationKey){
			return CreateWebServiceHQPayload(payloadContentxAsXml,eServiceCode.WebHostSynch,registrationKey,"","","");
		}

		///<summary>Creates an XML string for the payload of the provided content. Currently only useful if you have one thing to include in the payload.
		///</summary>
		public static string CreatePayloadContent<T>(T content,string tagName) {
			System.Xml.Serialization.XmlSerializer xmlListConfirmationRequestSerializer=new System.Xml.Serialization.XmlSerializer(typeof(T));
			StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer=XmlWriter.Create(strbuild,WebServiceMainHQProxy.CreateXmlWriterSettings(true))) {
				writer.WriteStartElement("Payload");
				writer.WriteStartElement(tagName);
				xmlListConfirmationRequestSerializer.Serialize(writer,content);
				writer.WriteEndElement();	
				writer.WriteEndElement(); //Payload	
			}
			return strbuild.ToString();
		}

		///<summary>Creates an XML string for the payload of the provided content. The list passed in is a tuple where the first item is the content to
		///be serialized and the second item is the tag name for the content.</summary>
		public static string CreatePayloadContent(List<PayloadItem> listPayloadItems) {
			StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer=XmlWriter.Create(strbuild,WebServiceMainHQProxy.CreateXmlWriterSettings(true))) {
				writer.WriteStartElement("Payload");
				foreach(PayloadItem payLoadItem in listPayloadItems) {
					XmlSerializer xmlListConfirmationRequestSerializer=new XmlSerializer(payLoadItem.Content.GetType());
					writer.WriteStartElement(payLoadItem.TagName);
					xmlListConfirmationRequestSerializer.Serialize(writer,payLoadItem.Content);
					writer.WriteEndElement();
				}
				writer.WriteEndElement(); //Payload	
			}
			return strbuild.ToString();
		}
		
		public static T DeserializeOutput<T>(string resultXml,string tagName) {
			XmlDocument doc=new XmlDocument();
			doc.LoadXml(resultXml);
			//Validate output.
			XmlNode node=doc.SelectSingleNode("//Error");
			if(node!=null) {
				throw new Exception(node.InnerText);
			}
			node=doc.SelectSingleNode("//"+tagName);
			if(node==null) {
				throw new Exception("Output node not found: "+tagName);
			}
			T retVal;
			using(XmlReader reader=XmlReader.Create(new StringReader(node.InnerXml))) {
				XmlSerializer serializer=new XmlSerializer(typeof(T));
				retVal=(T)serializer.Deserialize(reader);
			}
			if(retVal==null) {
				throw new Exception("Output node invalid: "+tagName);
			}
			return retVal;
		}

		public static XmlWriterSettings CreateXmlWriterSettings(bool omitXmlDeclaration) {
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = ("    ");
			settings.OmitXmlDeclaration=omitXmlDeclaration;
			return settings;
		}

		///<summary>Called from OpenDentalWebApps. Returns XML string with a Response element that contains the given content with nodeName.</summary>
		public static string CreateSuccessResponse<T>(T content,string nodeName) {
			StringBuilder strbuild=new StringBuilder();
			XmlSerializer xmlContentSerializer=new XmlSerializer(typeof(T));
			using(XmlWriter writer=XmlWriter.Create(strbuild,CreateXmlWriterSettings(false))) {
				writer.WriteStartElement("Response");
				writer.WriteStartElement(nodeName);
				xmlContentSerializer.Serialize(writer,content);
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
			return strbuild.ToString();
		}

		///<summary>Called from OpenDentalWebApps. Returns Response-Error payload with given error text. Application exceptions are thrown by various places in the validation code above. 
		///The message will be human ledgible and relevant to the error. All other exceptions were unexpected and the error message would not provide any value to the end user.</summary>
		public static string CreateErrorResponse(Exception e,string defaultError) {
			if(e==null || !(e is ApplicationException)) {
				return CreateErrorResponse(defaultError);
			}
			return CreateErrorResponse(e.Message);
		}

		///<summary>Called from OpenDentalWebApps. Returns Response-Error payload with given error text.</summary>
		public static string CreateErrorResponse(string error) {
			StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer=XmlWriter.Create(strbuild,CreateXmlWriterSettings(false))) {
				writer.WriteStartElement("Response");
				writer.WriteStartElement("Error");
				writer.WriteString(error);
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
			return strbuild.ToString();
		}

		///<summary>Prior to 17.4 the user had the choice to install the eConnector but leave it disabled. 
		///As of 17.4 installing gives implied conset to also enable the servcie for communicating.</summary>
		public static ListenerServiceType SetEConnectorOn() {
			return WebSerializer.DeserializePrimitiveOrThrow<ListenerServiceType>(
					GetWebServiceMainHQInstance().SetEConnectorType(WebSerializer.SerializePrimitive<string>(PrefC.GetString(PrefName.RegistrationKey)),true));
		}

		///<summary>Throws exceptions.</summary>
		public static void BuildWebSchedNewPatApptURLs(List<long> listClinicNums,Action<XmlNode> aNodeParsed) {
			//Make a web call to HQ to get the URLs for all the clinics.
			string response=GetWebServiceMainHQInstance()
					.BuildWebSchedNewPatApptURLs(PrefC.GetString(PrefName.RegistrationKey),String.Join("|",listClinicNums));
			//Parse Response
			XmlDocument doc=new XmlDocument();
			XmlNode nodeError=null;
			XmlNode nodeResponse=null;
			XmlNodeList nodeURLs=null;
			//Invalid web service response passed in.  Node will be null and will throw correctly.
			ODException.SwallowAnyException(() => {
				doc.LoadXml(response);
				nodeError=doc.SelectSingleNode("//Error");
				nodeResponse=doc.SelectSingleNode("//GetWebSchedURLsResponse");
			});
			#region Error Handling
			if(nodeError!=null||nodeResponse==null) {
				string error=Lans.g("WebSched","There was an error with the web request.  Please try again or give us a call.");
				//Either something went wrong or someone tried to get cute and use our Web Sched service when they weren't supposed to.
				if(nodeError!=null) {
					error+="\r\n"+Lans.g("WebSched","Error Details")+":\r\n"+nodeError.InnerText;
				}
				throw new Exception(error);
			}
			nodeURLs=doc.GetElementsByTagName("URL");
			if(nodeURLs==null) {
				throw new Exception("Invalid response from server recieved.");
			}
			#endregion
			//At this point we know we got a valid response from our web service.
			//Loop through all the URL nodes that were returned.
			foreach(XmlNode node in nodeURLs) {
				aNodeParsed(node);
			}
		}
		
		///<summary>Test the connection to WebServiceMainHQ</summary>
		public static bool CanReachWebService() {
			try {
				GetWebServiceMainHQInstance().TestConnection("");
				//Communication succeeded so that is considered a pass.
				return true;
			}
			catch (Exception e) {
				e.DoNothing();
				return false;
			}
		}

		#region EService setup
		///<summary>Called by local practice db to query HQ for EService setup info. Must remain very lite and versionless. Will be used by signup portal.
		///If HasClinics==true then any SignupOut.EServices entries where ClinicNum==0 are invalid and should be ignored.
		///If HasClinics==false then SignupOut.EServices should only pay attention items where ClinicNum==0.
		///This list is kept completely unfiltered by ClinicNum for forward compatibility reasons. 
		///The ClinicNum 0 items are always used by the Signup portal to determine default signup preferences.
		///However, these items are only used for validation and billing in the case where HasClinics==true.</summary>
		public static EServiceSetup.SignupOut GetEServiceSetupFull(SignupPortalPermission permission,bool isSwitchClinicPref=false) {
			//Clinics will be stored in this order at HQ to allow signup portal to display them in proper order.
			List<Clinic> clinics=Clinics.GetDeepCopy().OrderBy(x => x.ItemOrder).ToList();
			if(PrefC.GetBool(PrefName.ClinicListIsAlphabetical)) {
				clinics=clinics.OrderBy(x => x.Abbr).ToList();
			}
#if DEBUG
			bool isMockChanged=false;
			if(WebServiceMainHQProxy.MockWebServiceMainHQ==null) {
				WebServiceMainHQProxy.MockWebServiceMainHQ=new WebServiceMainHQMockDemo();
				isMockChanged=true;
			}
#endif
			EServiceSetup.SignupOut signupOut=ReadXml<EServiceSetup.SignupOut>
				(
					WebSerializer.DeserializePrimitiveOrThrow<string>
					(
						GetWebServiceMainHQInstance().EServiceSetup
						(
							CreateWebServiceHQPayload
							(
								WriteXml(new EServiceSetup.SignupIn() {
									MethodNameInt=(int)EServiceSetup.SetupMethod.GetSignupOutFull,
									HasClinics=PrefC.HasClinicsEnabled,
									//ClinicNum is not currently used as input.
									ClinicNum=0,
									ProgramVersionStr=PrefC.GetString(PrefName.ProgramVersion),
									SignupPortalPermissionInt=(int)permission,
									Clinics=clinics
										.Select(x => new EServiceSetup.SignupIn.ClinicLiteIn() {
											ClinicNum=x.ClinicNum,
											ClinicTitle=x.Abbr,
											IsHidden=x.IsHidden,
										}).ToList(),
									IsSwitchClinicPref=isSwitchClinicPref,
								}),eServiceCode.Undefined
							)
						)
					)
				);
#if DEBUG
			if(isMockChanged) {
				WebServiceMainHQProxy.MockWebServiceMainHQ=null;
			}
#endif
			//We just got the latest sync info from HQ so update the local db to reflect what HQ says is true.
			#region Reconcile Phones
			List<SmsPhone> listPhonesHQ=signupOut.Phones.Select(x => new SmsPhone() {
				ClinicNum=x.ClinicNum,
				CountryCode=x.CountryCode,
				DateTimeActive=x.DateTimeActive,
				DateTimeInactive=x.DateTimeInactive,
				InactiveCode=x.InactiveCode,
				PhoneNumber=x.PhoneNumber,
			}).ToList();
			SmsPhones.UpdateOrInsertFromList(listPhonesHQ);
			#endregion
			#region Reconcile practice and clinics
			List<EServiceSetup.SignupOut.SignupOutSms> smsSignups=GetSignups<EServiceSetup.SignupOut.SignupOutSms>(signupOut,eServiceCode.IntegratedTexting);
			bool isCacheInvalid=false;
			bool isSmsEnabled=false;
			if(PrefC.HasClinicsEnabled) { //Clinics are ON so loop through all clinics and reconcile with HQ.
				List<Clinic> listClinicsAll=Clinics.GetDeepCopy();
				foreach(Clinic clinicDb in listClinicsAll) {
					WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutSms clinicSignup=
						smsSignups.FirstOrDefault(x => x.ClinicNum==clinicDb.ClinicNum)??new WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutSms() {
							//Not found so turn it off.
							SmsContractDate=DateTime.MinValue,
							MonthlySmsLimit=0,
							IsEnabled=false,
						};
					Clinic clinicNew=clinicDb.Copy();
					clinicNew.SmsContractDate=clinicSignup.SmsContractDate;
					clinicNew.SmsMonthlyLimit=clinicSignup.MonthlySmsLimit;
					isCacheInvalid|=Clinics.Update(clinicNew,clinicDb);
					isSmsEnabled|=clinicSignup.IsEnabled;
				}				
			}
			else { //Clinics are off so ClinicNum 0 is the practice clinic.
				WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutSms practiceSignup=
					smsSignups.FirstOrDefault(x => x.ClinicNum==0)??new WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutSms() {
						//Not found so turn it off.
						SmsContractDate=DateTime.MinValue,
						MonthlySmsLimit=0,
						IsEnabled=false,
					};
				isCacheInvalid
					|=Prefs.UpdateDateT(PrefName.SmsContractDate,practiceSignup.SmsContractDate)
					|Prefs.UpdateLong(PrefName.TextingDefaultClinicNum,0)
					|Prefs.UpdateDouble(PrefName.SmsMonthlyLimit,practiceSignup.MonthlySmsLimit);
				isSmsEnabled|=practiceSignup.IsEnabled;
			}
			#endregion
			#region Reconcile CallFire
			//Turn off CallFire if SMS has been activated.
			//This only happens the first time SMS is turned on and CallFire is still activated.
			if(isSmsEnabled && Programs.IsEnabled(ProgramName.CallFire)) {
				Program callfire=Programs.GetCur(ProgramName.CallFire);
				if(callfire!=null) {
					callfire.Enabled=false;
					Programs.Update(callfire);
					Signalods.Insert(new Signalod() { IType=InvalidType.Providers });
					signupOut.Prompts.Add("Call Fire has been disabled. Cancel Integrated Texting and access program properties to retain Call Fire.");
				}
			}
			#endregion
			#region eConfirmations
			if(Prefs.UpdateBool(PrefName.ApptConfirmAutoSignedUp,IsEServiceActive(signupOut,eServiceCode.ConfirmationRequest))) {
				//HQ does not match the local pref. Make it match with HQ.
				isCacheInvalid=true;
				SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Automated appointment eConfirmations automatically changed by HQ.  Local pref set to "
					+IsEServiceActive(signupOut,eServiceCode.ConfirmationRequest).ToString()+".");
			}
			#endregion
			if(isCacheInvalid) { //Something changed in the db. Alert other workstations and change this workstation immediately.
				Signalods.Insert(new Signalod() { IType=InvalidType.Prefs });
				Prefs.RefreshCache();
				Signalods.Insert(new Signalod() { IType=InvalidType.Providers });
				Providers.RefreshCache();
				Clinics.RefreshCache();
			}
			return signupOut;
		}

		///<summary>Called by local practice db to query HQ for EService setup info. Must remain very lite and versionless. Will be used by signup portal.
		///If any args are null or not provided then they will be retrieved from Prefs.</summary>
		public static EServiceSetup.SignupOut GetEServiceSetupLite(
			SignupPortalPermission permission,string registrationKey=null,string practiceTitle=null,string practicePhone=null,string programVersion=null) 
		{
			return ReadXml<EServiceSetup.SignupOut>
				(
					WebSerializer.DeserializePrimitiveOrThrow<string>
					(
						GetWebServiceMainHQInstance().EServiceSetup
						(
							CreateWebServiceHQPayload
							(
								WriteXml(new EServiceSetup.SignupIn() {
									MethodNameInt=(int)EServiceSetup.SetupMethod.GetEServiceSetupLite,
									SignupPortalPermissionInt=(int)permission,
								}),
								eServiceCode.Undefined,
								//null is allowed for the rest. Will get converted to RegKey Pref.
								registrationKey, 
								practiceTitle,
								practicePhone,
								programVersion
							)
						)
					)
				);
		}

		///<summary>Called by local practice db to query HQ for EService setup info. Must remain very lite and versionless. 
		///This method will determine if clinics are enabled and HQ will include ClinicNum 0 in the output without it first being included in the input.
		///If clinics not enabled and an emptly list is passed in, then this method will automatically validate against the practice clinic (ClinicNum 0).</summary>
		public static List<long> GetEServiceClinicsAllowed(List<long> listClinicNums,eServiceCode eService) {
			if(MockWebServiceMainHQ!=null) {
				//Using the mock implementation because calling the real guts of this method would be difficult in testing because you would have to have a
				//database setup with signups for eServices.
				return MockWebServiceMainHQ.GetEServiceClinicsAllowed(listClinicNums,eService);
			}
			EServiceSetup.SignupOut signupOut=ReadXml<EServiceSetup.SignupOut>
			(
				WebSerializer.DeserializePrimitiveOrThrow<string>
				(
					GetWebServiceMainHQInstance().EServiceSetup
					(
						CreateWebServiceHQPayload
						(
							WriteXml(new EServiceSetup.SignupIn() {
								MethodNameInt=(int)EServiceSetup.SetupMethod.ValidateClinics,
								HasClinics=PrefC.HasClinicsEnabled,
								SignupPortalPermissionInt=(int)SignupPortalPermission.ReadOnly,
								Clinics=listClinicNums.Select(x => new EServiceSetup.SignupIn.ClinicLiteIn() { ClinicNum=x }).ToList(),
							}),
							eService
						)
					)
				)
			);
			//Include the "Practice" clinic if the calling method didn't specifically provide ClinicNum 0.
			if(!signupOut.HasClinics && !listClinicNums.Contains(0)) {
				listClinicNums.Add(0);
			}
			return signupOut.EServices
				//We only care about the input eService.
				.FindAll(x => x.EService==eService)
				//Must be included in the HQ output list in order to be considered valid.
				.FindAll(x => listClinicNums.Any(y => y == x.ClinicNum))
				.Select(x => x.ClinicNum).ToList();
		}

		///<summary>Makes web service call to HQ and validates if the given eService is allowed for the given clinic.
		///Set clinicNum==0 to check for a non-clinics practice. Returns true if allowed; otherwise returns false.
		///This is just a wrapper for GetEServiceClinicsAllowed().</summary>
		public static bool IsEServiceClinicAllowed(long clinicNum,eServiceCode eService) {
			return GetEServiceClinicsAllowed(new List<long> { clinicNum },eService).Any(x => x==clinicNum);
		}

		///<summary>Makes web service call to HQ and validates if the given eService is allowed for the practice.
		///True indicates that at least 1 clinic and/or the practice iteself is signed up.
		///Use IsEServiceClinicAllowed() or GetEServiceClinicsAllowed() if you care about specific clinics.
		///This is just a wrapper for GetEServiceClinicsAllowed().</summary>
		public static bool IsEServiceAllowed(eServiceCode eService) {
			return GetEServiceClinicsAllowed(Clinics.GetDeepCopy().Select(x => x.ClinicNum).ToList(),eService).Count>0;
		}

		///<summary>Helper to interpret output from GetEServiceSetupFull().
		///Gets all signups found for the given eService.</summary>
		public static List<T> GetSignups<T>(EServiceSetup.SignupOut signupOut,eServiceCode eService) where T : WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService {
			return signupOut.EServices
				.FindAll(x => x.EService==eService)
				.Cast<T>().ToList();
		}

		///<summary>Helper to interpret output from GetEServiceSetupFull().
		///Indicates if HQ says this account is registered for the given eService.</summary>
		public static bool IsEServiceActive(EServiceSetup.SignupOut signupOut,eServiceCode eService) {
			return GetSignups<WebServiceMainHQProxy.EServiceSetup.SignupOut.SignupOutEService>(signupOut,eService)
				.Any(x => x.IsEnabled);
		}

		///<summary>Used to send EServiceClinic(s) to and from HQ. Must remain very lite and versionless. Will be used by signup portal.</summary>
		public class EServiceSetup {
			///<summary>Input of WebServiceHQ.EServiceSetup() web method.</summary>
			public class SignupIn {
				///<summary>All clinics belonging to local db. Required for lite version? NO.</summary>
				public List<ClinicLiteIn> Clinics;
				///<summary>Try to convert this to SignupPortalPermission enum in OD. If not found then omit this list item. Required for all versions? YES.</summary>
				public int SignupPortalPermissionInt;
				///<summary>Local DB ClinicNum. Can be 0. Required for lite version? NO.</summary>
				public long ClinicNum;
				///<summary>Should be convertible to Version. Required for lite version? NO.</summary>
				public string ProgramVersionStr;
				///<summary>Indicates if this practice has the clinics feature turned on.</summary>
				public bool HasClinics=false;
				///<summary>Try to convert this to SetupMethod enum in OD. This flag will determine which method is called by WebServiceHQ. Required for all versions? YES.</summary>
				public int MethodNameInt;
				///<summary>Request is coming from customer in FormShowFeatures. Customer has elected to turn 'Clinics' feature on or off. 
				///Back-end will detect this flag and change foreign keys to match new setup for this customer.</summary>
				public bool IsSwitchClinicPref=false;

				[XmlIgnore]
				public SetupMethod MethodName {
					get {
						if(Enum.IsDefined(typeof(SetupMethod),MethodNameInt)) {
							return (SetupMethod)MethodNameInt;
						}
						return SetupMethod.Undefined;
					}
				}

				[XmlIgnore]
				public SignupPortalPermission PortalPermission {
					get {
						if(Enum.IsDefined(typeof(SignupPortalPermission),SignupPortalPermissionInt)) {
							return (SignupPortalPermission)SignupPortalPermissionInt;
						}
						return SignupPortalPermission.Denied;
					}
				}
				
				[XmlIgnore]
				public Version ProgramVersion {
					get {
						Version ret;
						if(!Version.TryParse(ProgramVersionStr,out ret)) {
							ret=new Version(0,0);
						}
						return ret;
					}
				}

				///<summary>Lite version of HQ EServiceClinic table. Versionless so keep simple and do not remove or rename fields.</summary>
				public class ClinicLiteIn {
					public long ClinicNum;
					public string ClinicTitle;
					public bool IsHidden;
				}
			}

			///<summary>Output of WebServiceHQ.EServiceSetup() web method. Set SignupIn.IsLiteVersion=true for lite version.</summary>
			public class SignupOut {
				///<summary>What type of listener is being used by this office. Included in lite version? YES.</summary>
				public int ListenerTypeInt;
				///<summary>Clinic based description of EService signup status. 
				///Full version will included SignupOutSms for IntegratedTexting entries. 
				///Lite version includes only SignupOutEService objects, even for IntegratedTexting. Included in lite version? YES.</summary>
				public List<SignupOutEService> EServices;
				///<summary>Any phones owned by this reg key. Included in lite version? NO.</summary>
				public List<SignupOutPhone> Phones;
				///<summary>Navigates to signup portal using given SignupIn inputs. Included in lite version? NO.</summary>
				public string SignupPortalUrl;
				///<summary>Indicates if this practice has the clinics feature turned on.</summary>
				public bool HasClinics=false;
				///<summary>Try to convert this to SignupPortalPermission enum in OD. If not found then omit this list item. Required for lite version? NO.</summary>
				public int SignupPortalPermissionInt;
				///<summary>Try to convert this to SetupMethod enum in OD. This flag will determine which method is called by WebServiceHQ. Required for all versions? YES.</summary>
				public int MethodNameInt;
				///<summary>This list will be filled by GetEServiceSetupFull(). 
				///If any Prompts are added then they should typically be shown to the user. They will usually indicate that a local pref of some sort has been changed.</summary>
				[XmlIgnore]
				public List<string> Prompts=new List<string>();
				
				[XmlIgnore]
				public SetupMethod MethodName {
					get {
						if(Enum.IsDefined(typeof(SetupMethod),MethodNameInt)) {
							return (SetupMethod)MethodNameInt;
						}
						return SetupMethod.Undefined;
					}
				}

				[XmlIgnore]
				public SignupPortalPermission PortalPermission {
					get {
						if(Enum.IsDefined(typeof(SignupPortalPermission),SignupPortalPermissionInt)) {
							return (SignupPortalPermission)SignupPortalPermissionInt;
						}
						return SignupPortalPermission.Denied;
					}
				}

				[XmlIgnore]
				public ListenerServiceType ListenerType {
					get {
						if(Enum.IsDefined(typeof(ListenerServiceType),ListenerTypeInt)) {
							return (ListenerServiceType)ListenerTypeInt;
						}
						return ListenerServiceType.DisabledByHQ;
					}
				}

				public class SignupOutSms:SignupOutEService {
					///<summary>Monthly amount spent on texting which this clinic does not want to exceed.</summary>
					public double MonthlySmsLimit;
					///<summary>The start date of the texting service.</summary>
					public DateTime SmsContractDate;
					///<summary>Country code linked to this clinic for the purpose of Integrated Texting. String version of ISO31661.</summary>
					public string CountryCode;
				}

				///<summary>Lite version of HQ EServiceSignup table. Versionless so keep simple and do not remove or rename fields.</summary>
				[XmlInclude(typeof(SignupOutSms))] //Allows sub-class to be serialized without custom serializer.
				public class SignupOutEService {
					public long ClinicNum;
					///<summary>Try to convert this to eServiceCode enum in OD. If not found then omit this list item.</summary>
					public int EServiceCodeInt;
					///<summary>URL used for this EService for this clinic. Only applies in some scenarios, otherwise empty.</summary>
					public string HostedUrl;
					///<summary>URL used for the Patient Portal EService for this clinic to make payments quickly. Only applies in some scenarios, otherwise empty.</summary>
					public string HostedUrlPayment;
					///<summary>From HQ RepeatCharge and HQ EServiceSignup.</summary>
					public bool IsEnabled;

					[XmlIgnore]
					public eServiceCode EService {
						get {
							if(Enum.IsDefined(typeof(eServiceCode),EServiceCodeInt)) {
								return (eServiceCode)EServiceCodeInt;
							}
							return eServiceCode.Undefined;
						}
					}
				}

				///<summary>Lite version of SmsPhone table. Versionless so keep simple and do not remove or rename fields.</summary>
				public class SignupOutPhone {
					public long ClinicNum;
					public string PhoneNumber;
					public string CountryCode;
					public DateTime DateTimeActive;
					public DateTime DateTimeInactive;
					public string InactiveCode;
					public bool IsActivated;
				}				
			}

			///<summary>Order matters for serialization. Do not change order.</summary>
			public enum SetupMethod {
				Undefined,
				GetSignupOutFull,
				GetEServiceSetupLite,
				ValidateClinics,
			}
		}
		#endregion

		///<summary>Gets the specified number of short GUIDs</summary>
		///<returns>First item in the Tuple is the short GUID. Second item is the URL for the short GUID if there is one for this eService.</returns>
		public static List<ShortGuidResult> GetShortGUIDs(int numberToGet,long clinicNum,eServiceCode eService) {
			List<PayloadItem> listPayloadItems=new List<PayloadItem> {
				new PayloadItem(clinicNum,"ClinicNum"),
				new PayloadItem(numberToGet,"NumberShortGUIDsToGet")
			};
			string officeData=WebServiceMainHQProxy.CreateWebServiceHQPayload(WebServiceMainHQProxy.CreatePayloadContent(listPayloadItems),eService);
			string result=WebServiceMainHQProxy.GetWebServiceMainHQInstance().GenerateShortGUIDs(officeData);
			return WebServiceMainHQProxy.DeserializeOutput<List<ShortGuidResult>>(result,"ListShortGuidResults");
		}

		///<summary>WebServiceMainHQ.GenerateShortGUIDs returns a list of these.</summary>
		[Serializable]
		public class ShortGuidResult {
			public string ShortGuid;
			public string ShortURL;
			public string MediumURL;
		}

		private static string GetNodeNameFromType(Type t) {
			return t.Name+"Xml";
		}

		public static T ReadXml<T>(string xml) where T : new() {
			XmlDocument doc=new XmlDocument();
			doc.LoadXml(xml);
			XmlNode node=doc.SelectSingleNode("//"+GetNodeNameFromType(typeof(T)));
			if(node==null) {
				throw new ApplicationException(GetNodeNameFromType(typeof(T))+" node not present.");
			}
			T ret=default(T); XmlSerializer serializer=new XmlSerializer(typeof(T));
			using(XmlReader reader = XmlReader.Create(new System.IO.StringReader(node.InnerXml))) {
				ret=(T)serializer.Deserialize(reader);
			}
			if(ret==null) {
				ret=new T();
			}
			return ret;
		}

		public static string WriteXml<T>(T input) {
			XmlSerializer serializer=new XmlSerializer(typeof(T)); StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer = XmlWriter.Create(strbuild,WebServiceMainHQProxy.CreateXmlWriterSettings(true))) {
				writer.WriteStartElement(GetNodeNameFromType(typeof(T)));
				serializer.Serialize(writer,input);
				writer.WriteEndElement();
			}
			return strbuild.ToString();
		}
	}


	public class PayloadItem:Tuple<object,string> {
		public object Content { get { return Item1; } }
		public string TagName { get { return Item2; } }
		public PayloadItem(object content,string tagName) : base(content,tagName) {
		}
	}


}
