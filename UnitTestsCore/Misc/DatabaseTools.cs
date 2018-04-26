using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using OpenDentBusiness;
using CodeBase;

namespace UnitTestsCore {
	///<summary>Contains the queries, scripts, and tools to clear the database of data from previous unitTest runs.</summary>
	public class DatabaseTools {

		///<summary>This is analogous to FormChooseDatabase.TryToConnect.  Empty string is allowed.</summary>
		public static bool SetDbConnection(string dbName,bool isOracle){
			return SetDbConnection(dbName,"localhost","","root","",isOracle);
		}

		//<summary>This function allows connecting to a specific server.</summary>
		public static bool SetDbConnection(string dbName,string serverAddr,string port,string userName,string password,bool isOracle) {
			OpenDentBusiness.DataConnection dcon;
			//Try to connect to the database directly
			try {
				if(!isOracle) {
					DataConnection.DBtype=DatabaseType.MySql;
					dcon=new OpenDentBusiness.DataConnection(DataConnection.DBtype);
					string connectStr="Server="+serverAddr
						+";Port="+port
						+";Database="+dbName
						+";User ID="+userName
						+";Password="+password
						+";CharSet=utf8"
						+";Treat Tiny As Boolean=false"
						+";Allow User Variables=true"
						+";Default Command Timeout=3600";
					dcon.SetDb(connectStr,"",DataConnection.DBtype,true);
					RemotingClient.RemotingRole=RemotingRole.ClientDirect;
					return true;
				}
				else {
					DataConnection.DBtype=DatabaseType.Oracle;
					dcon=new OpenDentBusiness.DataConnection(DataConnection.DBtype);
					dcon.SetDb("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST="+serverAddr+")(PORT="+port+"))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=XE)));User Id="+userName+";Password="+password+";","",DataConnection.DBtype,true);
					RemotingClient.RemotingRole=RemotingRole.ClientDirect;
					return true;
				}
			}
			catch(Exception ex) {
				ex.DoNothing();
				if(isOracle) {
					throw new Exception("May need to create a Fresh Db for Oracle.");
				}
				return false;
			}
		}
		
		private static void ExecuteCommand(string Command){
			try {
				System.Diagnostics.ProcessStartInfo ProcessInfo;
				System.Diagnostics.Process Process;
				ProcessInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe","/C " + Command);
				ProcessInfo.CreateNoWindow = false;
				ProcessInfo.UseShellExecute = false;
				Process = System.Diagnostics.Process.Start(ProcessInfo);
				Process.Close();
			}
			catch {
				throw new Exception("Running cmd failed.");
			}
		}


		public static string ClearDb() {
			string command=@"
				DELETE FROM alertitem;
					DELETE FROM appointment;
					DELETE FROM apptreminderrule;
					DELETE FROM asapcomm;
					DELETE FROM carrier;
					DELETE FROM claim;
					DELETE FROM claimproc;
					DELETE FROM clinic;
					DELETE FROM clockevent;
					DELETE FROM confirmationrequest;
					DELETE FROM creditcard;
					DELETE FROM employee;
					DELETE FROM fee;
					DELETE FROM feesched WHERE FeeSchedNum !=53; /*because this is the default fee schedule for providers*/
					DELETE FROM hl7def;
					DELETE FROM hl7msg;
					DELETE FROM insplan;
					DELETE FROM operatory;
					DELETE FROM patient;
					DELETE FROM patientportalinvite;
					DELETE FROM patientrace;
					DELETE FROM patplan;
					DELETE FROM payment;
					DELETE FROM paysplit;
					DELETE FROM payperiod;
					DELETE FROM payplan;
					DELETE FROM payplancharge;
					DELETE FROM procedurelog;
					DELETE FROM provider WHERE ProvNum>2;
					DELETE FROM recall;
					DELETE FROM schedule;
					DELETE FROM smsphone;
					DELETE FROM smstomobile;
					DELETE FROM timeadjust;
					DELETE FROM timecardrule;
				";
			DataCore.NonQ(command);
			Providers.RefreshCache();
			FeeScheds.RefreshCache();
			return "Database cleared of old data.\r\n";
		}

		
	}
}
