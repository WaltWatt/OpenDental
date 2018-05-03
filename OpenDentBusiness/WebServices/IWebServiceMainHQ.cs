﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness {
	///<summary>This interface needs to have any method from WebServiceMainHQ that you want to use. This allows for testing code without having to
	///have an instance of WebServiceMainHQ setup.</summary>
	public interface IWebServiceMainHQ {
		///<summary>RealWebServiceMainHQ must be able to change the URL at runtime. URL is typically a property of each web reference in c#. 
		///Placing this here allows the mock instance to see this property also.</summary>
		string Url { get; set; }
		string HandleConfirmationsApptChanged(string officeData);
		List<long> GetEServiceClinicsAllowed(List<long> listClinicNums,eServiceCode eService);
		string WebSchedAsapSend(string officeData);
		string TestConnection(string officeData);
		string TestConnectionDb(string officeData);
		string SetEConnectorType(string officeData,bool isListening);
		string BuildWebSchedNewPatApptURLs(string registrationKey,string clinicNums);
		string EServiceSetup(string officeData);
		string EnableAdditionalFeatures(string officeData);
		void SetEConnectorStatsAsync(string payload);
		string SmsSend(string officeData);
		string BuildOAuthUrl(string registrationKey,string appName);
		string GetDropboxAccessToken(string accessCode);
		string GetFHIRAPIKeysForOffice(string officeData);
		string GenerateWebAppUrl(string officeData);
		string GenerateFHIRAPIKey(string officeData);
		string UpdateFHIRAPIKeys(string officeData);
		string ConfirmationRequestSend(string officeData);
		string WebSchedRecallNotificationsSend(string officeData);
		string GetEConnectorType(string officeData);
		string RequestListenerProxyPrefs(string officeData);
		string ValidateWebAppUrl(string officeData);
		string GetFeaturesForCustomer(string registrationKey);
		string GetDropboxAuthorizationUrl(long registrationKeyNum);
		void ProcessEConnectorFailoverAsync(string officeData);
		string CheckFHIRAPIKey(string officeData);
		string ConfirmationRequestSignAgreement(string officeData);
		string ConfirmationRequestCancelService(string officeData);
		string PerformRefreshCache(string officeData);
		string ValidateVersion(string officeData);
		string BuildConfirmationRequestUrl(string shortGuid);
		string BuildWebSchedUrl(string shortGuid);
		string BuildWebSchedASAPUrl(string shortGuid);
		string SubmitUnhandledException(string officeData);
		string ValidateEService(string registrationKey,string eService);
		string GenerateShortGUIDs(string officeData);
		string BuildPatientPortalStatementUrl(string shortGuid);
		string BuildConfirmationRequestUrlQuick(string shortGuid);
	}
}
