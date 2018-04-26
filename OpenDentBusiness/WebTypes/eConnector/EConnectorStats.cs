using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeBase;
using Newtonsoft.Json;

namespace OpenDentBusiness.WebTypes {
	///<summary>This class is used to hold interesting pieces of information regarding a customer's eConnector.</summary>
	public class EConnectorStats : WebBase {
		///<summary>The computer where the eConnector is running.</summary>
		public string EConnectorComputerName;
		///<summary>The user under which the eConnector is running.</summary>
		public string EConnectorDomainUserName;
		///<summary>An IP address of the computer where the eConnector is running.</summary>
		public string EConnectorIP;
		///<summary>The preference in the database.</summary>
		public bool HasClinicsEnabled;
		///<summary>The number of clinics that are currently in use.</summary>
		public int CountActiveClinics;
		///<summary>The number of clinics that are not currently in use.</summary>
		public int CountInactiveClinics;
		///<summary>The number of patients who have a completed procedure in the last two years.</summary>
		public int CountActivePatients;
		///<summary>The number of patients who have not had a completed procedure in the last two years.</summary>
		public int CountNonactivePatients;
		///<summary>The signals of type ListenerService from the last 30 days. Equivalent to what displays in the History grid in the EConnector tab
		///in the eServices Setup window.</summary>
		public List<EServiceSignal> ListEServiceSignals;
		///<summary>The time that these statistics were generated. Will be in the time zone of eConnector.</summary>
		public DateTime DateTimeNow;
		///<summary>A few choice prefs.</summary>
		public List<Pref> ListEServicePrefs;

		///<summary>Send a summary of eConnector statistics to OD HQ. This should only be called from the eConnector.</summary>
		public static void UpdateEConnectorStats() {
			OpenDentBusiness.WebTypes.EConnectorStats eConnStats=new OpenDentBusiness.WebTypes.EConnectorStats();
			eConnStats.EConnectorComputerName=Environment.MachineName;
			eConnStats.EConnectorDomainUserName=Environment.UserName;
			eConnStats.EConnectorIP=ODEnvironment.GetLocalIPAddress();
			eConnStats.HasClinicsEnabled=PrefC.HasClinicsEnabled;
			if(PrefC.HasClinicsEnabled) {
				eConnStats.CountActiveClinics=OpenDentBusiness.Clinics.GetCount();
				eConnStats.CountInactiveClinics=OpenDentBusiness.Clinics.GetCount()-eConnStats.CountActiveClinics;
			}
			else {
				eConnStats.CountActiveClinics=0;
				eConnStats.CountInactiveClinics=OpenDentBusiness.Clinics.GetCount();
			}
			eConnStats.CountActivePatients=OpenDentBusiness.Procedures.GetCountPatsComplete(DateTime.Today.AddYears(-2),DateTime.Today);
			eConnStats.CountNonactivePatients=OpenDentBusiness.Patients.GetPatCountAll()-eConnStats.CountActivePatients;
			eConnStats.ListEServiceSignals=OpenDentBusiness.EServiceSignals.GetServiceHistory(eServiceCode.ListenerService,DateTime.Today.AddDays(-30),
				DateTime.Today,30);
			eConnStats.DateTimeNow=DateTime.Now;
			eConnStats.ListEServicePrefs=new List<Pref>();
			foreach(PrefName prefName in Enum.GetValues(typeof(PrefName))) {
				if(prefName.In(
					PrefName.RegistrationKey,
					PrefName.ProgramVersion,
					PrefName.DataBaseVersion,
					PrefName.TextingDefaultClinicNum,
					PrefName.WebServiceServerName,
					PrefName.AutomaticCommunicationTimeStart,
					PrefName.AutomaticCommunicationTimeEnd)
					|| prefName.ToString().StartsWith("WebSched")
					|| prefName.ToString().StartsWith("ApptConfirm")
					|| prefName.ToString().StartsWith("ApptRemind")
					|| prefName.ToString().StartsWith("ApptEConfirm")
					|| prefName.ToString().StartsWith("Recall")
					|| prefName.ToString().StartsWith("PatientPortal")
					|| prefName.ToString().StartsWith("Sms")) 
				{
					try {
						eConnStats.ListEServicePrefs.Add(Prefs.GetPref(prefName.ToString()));
					}
					catch(Exception ex) {
						ex.DoNothing();
					}
				}
			}
			List<EConnectorStats> listStatsToSend=new List<EConnectorStats> { eConnStats };
			string dbStats=PrefC.GetString(PrefName.EConnectorStatistics);
			List<EConnectorStats> listDbStats=DeserializeListFromJson(dbStats)??new List<EConnectorStats>();
			bool doCreateAlert=false;
			foreach(EConnectorStats stats in listDbStats) {
				//If a different eConnector is saving stats, add that one to the list to be sent to HQ.
				if(eConnStats.EConnectorComputerName!=stats.EConnectorComputerName && (eConnStats.DateTimeNow-stats.DateTimeNow).TotalHours < 23) {
					stats.ListEServicePrefs=new List<Pref>();//To save on bandwidth
					stats.ListEServiceSignals=new List<EServiceSignal>();
					listStatsToSend.Add(stats);
					if((eConnStats.DateTimeNow-stats.DateTimeNow).TotalHours < 3) {
						doCreateAlert=true;
					}
				}
			}
			if(doCreateAlert && AlertItems.RefreshForType(AlertType.MultipleEConnectors).Count==0) {
				AlertItem alert=new AlertItem {
					Actions=ActionType.MarkAsRead | ActionType.Delete,
					Description=Lans.g("EConnectorStats","eConnector services are being run on these computers:")+" "
						+string.Join(", ",listStatsToSend.Select(x => x.EConnectorComputerName)),
					Severity=SeverityType.High,
					Type=AlertType.MultipleEConnectors,
				};
				AlertItems.Insert(alert);
			}
			string statsStr=SerializeToJson(listStatsToSend);
			OpenDentBusiness.Prefs.UpdateString(PrefName.EConnectorStatistics,statsStr);
			string payload=WebServiceMainHQProxy.CreateWebServiceHQPayload(WebServiceMainHQProxy.CreatePayloadContent(statsStr,"EConnectorStatsStr"),
				eServiceCode.ListenerService);
			WebServiceMainHQProxy.GetWebServiceMainHQInstance().SetEConnectorStatsAsync(payload);
		}
		
		///<summary>Returns an empty list if deserialization fails.</summary>
		public static List<EConnectorStats> DeserializeListFromJson(string jsonString) {
			try {
				return JsonConvert.DeserializeObject<List<EConnectorStats>>(jsonString)??new List<EConnectorStats>();
			}
			catch(Exception ex) {
				ex.DoNothing();
				try {
					//Previous to 17.3.21ish, eConnectors sent over a single EConnectorStats instead of a list.
					return new List<EConnectorStats> { JsonConvert.DeserializeObject<EConnectorStats>(jsonString)??new EConnectorStats() };
				}
				catch(Exception e) {
					e.DoNothing();
				}
				return new List<EConnectorStats>();
			}
		}

		public static string SerializeToJson(List<EConnectorStats> listStats) {
			return JsonConvert.SerializeObject(listStats);
		}

		///<summary>Get the most recent entry for each computer.</summary>
		public static List<EConnectorStats> GroupByComputerName(List<EConnectorStats> listIn) {
			try {
				List<EConnectorStats> listOut=listIn.OrderByDescending(x => x.DateTimeNow)
					.GroupBy(x => x.EConnectorComputerName)
					.Select(x => x.First()).ToList();
				return listOut;
			}
			catch(Exception e) {
				e.DoNothing();
			}
			return listIn;
		}
	}
}
