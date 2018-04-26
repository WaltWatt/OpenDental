using CodeBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace OpenDentBusiness {
	///<summary>HQ only. This is NOT a table type. It is a class that is populated by the Broadcaster at a fairly frequenty interval (30 seconds or so).
	///It is then serialized and saved as a ESerivceSignal via upsert. Each HQ workstation will then select that EServiceSignal very frequently and display the results.</summary>
	[Serializable()]
	public class EServiceMetrics {
		///<summary>Time at which this data was generated. It past a certain threshold in the past then consider the data invalid.</summary>
		public DateTime Timestamp=DateTime.MinValue;
		///<summary>True if all Broadcaster heartbeats are current and not critical; otherwise false.</summary>
		public bool IsBroadcasterHeartbeatOk;
		///<summary>Retreived from NexmoAPI.GetAccountBalance().</summary>
		public float AccountBalanceEuro;
		///<summary>If true then this data is valid and came from the Broadcaster AccountMaintThread; otherwise this data is not accurate.
		///Will be set after deserialization to indicate that the data was found and deserialized correctly.</summary>
		public bool IsValid;
		///<summary>If this string is not empty then assume a critical server is unavailable. This is considered a critical error.</summary>
		public string WebsitesDown="";

		///<summary>This is derived property. Do not serialize.</summary>
		[XmlIgnore]
		public eServiceSignalSeverity Severity {
			get {
				if(!string.IsNullOrEmpty(CriticalStatus)) {
					return eServiceSignalSeverity.Critical;
				}
				return eServiceSignalSeverity.Working;
			}
		}

		///<summary>This is derived property. Do not serialize. If returns non-empty string then assume EServices is in a critical state.</summary>
		[XmlIgnore]
		public string CriticalStatus {
			get {
				try {
					if(!IsValid) {
						throw new Exception("EServiceMetrics.IsValid=false. Deserialization failed.");
					}
					if(DateTime.Now.Subtract(Timestamp)>TimeSpan.FromMinutes(5)) {
						throw new Exception("EServiceMetrics object is more than 5 minutes stale.");
					}
					if(!IsBroadcasterHeartbeatOk) {
						throw new Exception("Broadcaster or Proxy thread heartbeat is invalid.");
					}
					if(!string.IsNullOrEmpty(WebsitesDown)) {
						throw new Exception(WebsitesDown);
					}
					return "";
				}
				catch(Exception e) {
					return "EService Critical Error: "+e.Message;
				}
			}
		}
		
		public delegate void EServiceMetricsArgs(EServiceMetrics eServiceMetrics);

		///<summary>Gets one EServiceSignalHQ from the serviceshq db located on SERVER184. Returns null in case of failure.</summary>
		public static EServiceMetrics GetEServiceMetricsFromSignalHQ() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<EServiceMetrics>(MethodBase.GetCurrentMethod());
			}
			//Create an ODThread so that we can safely change the database connection settings without affecting the calling method's connection.
			ODThread odThread=new ODThread(new ODThread.WorkerDelegate((ODThread o) => {
				//Always set the thread static database connection variables to set the serviceshq db conn.
				new DataConnection().SetDbT("server184","serviceshq","root","","","",DatabaseType.MySql,true);
				//See EServiceSignalHQs.GetEServiceMetrics() for details.
				string command =@"SELECT 0 EServiceSignalNum, h.* FROM eservicesignalhq h 
					WHERE h.ReasonCode=1024
						AND h.ReasonCategory=1
						AND h.ServiceCode=2
						AND h.RegistrationKeyNum=-1
					ORDER BY h.SigDateTime DESC 
					LIMIT 1";
				EServiceSignal eServiceSignal=Crud.EServiceSignalCrud.SelectOne(command);
				EServiceMetrics eServiceMetric=new EServiceMetrics();
				if(eServiceSignal!=null) {
					using(XmlReader reader=XmlReader.Create(new System.IO.StringReader(eServiceSignal.Tag))) {
						eServiceMetric=(EServiceMetrics)new XmlSerializer(typeof(EServiceMetrics)).Deserialize(reader);
					}
					eServiceMetric.IsValid=true;
				}
				o.Tag=eServiceMetric;
			}));
			odThread.AddExceptionHandler(new ODThread.ExceptionDelegate((Exception e) => { }));//Do nothing
			odThread.Name="eServiceMetricsThread";
			odThread.Start(true);
			if(!odThread.Join(2000)) { //Give this thread up to 2 seconds to complete.
				return null;
			}
			EServiceMetrics retVal=(EServiceMetrics)odThread.Tag;
			return retVal;
		}

		#region Calculate metrics

		///<summary>Get metrics from serviceshq.</summary>
		public static EServiceMetrics CalculateMetrics(float accountBalanceEuro,string nexmoStatus) {
			//No remoting role check, No call to database.
			DateTime dateTimeStart=DateTime.Today;
			DateTime dateTimeEnd=dateTimeStart.AddDays(1);
			List<string> websitesDown=PrefC.GetRaw("BroadcasterWebsitesToMonitor").Split(',')
				.Select(x => WebSiteIsAvailable(x)).ToList();
			websitesDown.Add(nexmoStatus);
			websitesDown.RemoveAll(x => string.IsNullOrEmpty(x.Trim()));
			EServiceMetrics ret=new EServiceMetrics() {
				Timestamp=DateTime.Now,
				AccountBalanceEuro=accountBalanceEuro,
				IsBroadcasterHeartbeatOk=GetIsBroadcasterHeartbeatOk(),
				WebsitesDown=string.Join("; ",websitesDown),
				IsValid=true,
			};			
			return ret;
		}

		///<summary>Sent an HTTP GET request to the given URL. Returns a string if domain given does not respond. This is considered a fail.</summary>
		private static string WebSiteIsAvailable(string url) {
			string ret="";
			Func<int,bool> checkTimeout=new Func<int, bool>((timeoutMS) => {
				try {
					HttpWebRequest request=(HttpWebRequest)HttpWebRequest.Create(url);
					request.Timeout=timeoutMS;
					request.Method="GET";
					using(HttpWebResponse response=(HttpWebResponse)request.GetResponse()) {
						//Good enough. we are only testing to see if we can get the response.
						return true;
					}
				}
				catch(Exception ex) {
					ret="Website down!!! ("+url+"): "+ex.Message;
					return false;
				}
			});
			//Try 10 times with longer timeout each time.
			for(int i = 1;i<=10;i++) {
				if(checkTimeout(i*100)) {
					return "";
				}
			}
			return ret;
		}

		private static bool GetIsBroadcasterHeartbeatOk() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod());
			}
			//Reason categories are defined by enums: BroadcasterThreadDefs AND ProxyThreadDef.
			string command=@"
				SELECT * FROM (
				  SELECT 
					0 EServiceSignalNum,
					e.*
				  FROM eservicesignalhq e
					WHERE
					  e.RegistrationKeyNum=-1  -- HQ
					  AND (e.ServiceCode=2 OR e.ServiceCode=3) -- IntegratedTexting OR HQProxyService
					  AND 
					  (
						e.ReasonCode=1004 -- Heartbeat
						OR e.ReasonCode=1005 -- ThreadExit
					   ) 
					ORDER BY 
					  e.SigDateTime DESC
				) a
				GROUP BY a.ReasonCategory
				ORDER BY a.SigDateTime DESC;";
			List<EServiceSignal> signals=Crud.EServiceSignalCrud.SelectMany(command);			
			if(signals.Exists(x => x.Severity==eServiceSignalSeverity.Critical || DateTime.Now.Subtract(x.SigDateTime)>TimeSpan.FromMinutes(10))) {
				return false;
			}
			//We got this far so all good.
			return true;
		}

		private static DataTable GetSmsInbound(DateTime dateTimeStart,DateTime dateTimeEnd) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateTimeStart,dateTimeEnd);
			}
			//-- Returns Count of outbound messages and total customer charges accrued.
			string command=@"
				SELECT 
				  COUNT(*) NumMessages
				FROM 
				  smsnexmomoterminated t
				WHERE
				  t.DateTimeODRcv>="+POut.DateT(dateTimeStart,true)+@"
				  AND t.DateTimeODRcv <"+POut.DateT(dateTimeEnd,true)+";";
			return Db.GetTable(command);
		}

		private static DataTable GetSmsOutbound(DateTime dateTimeStart,DateTime dateTimeEnd) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateTimeStart,dateTimeEnd);
			}
			//-- Returns Count of outbound messages and total customer charges accrued.
			string command=@"
				SELECT 
				  COUNT(*) NumMessages,
				  SUM(t.MsgChargeUSD) MsgChargeUSDTotal
				FROM 
				  smsnexmomtterminated t
				WHERE
				  t.MsgStatusCust IN(1,2,3,4)
				  AND t.DateTimeTerminated>="+POut.DateT(dateTimeStart,true)+@"
				  AND t.DateTimeTerminated <"+POut.DateT(dateTimeEnd,true)+";";
			return Db.GetTable(command);
		}

		private static DataTable GetBroadcastersErrors() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod());
			}
			//-- Returns Count of all unprocessed rows which have severity of Warning or Error.
			string command=@"
				SELECT e.Severity,COUNT(*) CountOf
				  FROM eservicesignalhq e
				WHERE
				  e.RegistrationKeyNum=-1  -- HQ
				  AND e.IsProcessed=0 -- NOT processed
				  AND 
				  (
					e.ReasonCode<>1004 -- NOT Heartbeat
					OR e.ReasonCode<>1005 -- NOT ThreadExit
				  ) 
				  AND 
				  (
					e.Severity=3 -- Warning
					OR e.Severity=4 -- Error
				  )
				GROUP BY
				  e.Severity
				;";
			return Db.GetTable(command);
		}

		#endregion
	}	
}
