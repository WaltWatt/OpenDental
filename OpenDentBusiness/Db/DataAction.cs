using CodeBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness {
	///<summary>Perform actions in different database contexts.</summary>
	public class DataAction {
		#region Helpers to run directly on a given db.
		///<summary>HQ only. Perform the given action in the context of the customers db.</summary>
		public static void RunCustomers(Action a) {
			Run(a,ConnectionNames.CustomersHQ);
		}

		///<summary>HQ only. Perform the given action in the context of the eServices db.</summary>
		public static void RunEServices(Action a) {
			Run(a,ConnectionNames.ServicesHQ);
		}

		///<summary>Perform the given action in the context of the dental office db.</summary>
		public static void RunPractice(Action a) {
			Run(a,ConnectionNames.DentalOffice);
		}

		///<summary>Perform the given action in the context of the old mobile web db.</summary>
		public static void RunMobileWebOld(Action a) {
			Run(a,ConnectionNames.MobileWebOld);
		}

		///<summary>Perform the given action in the context of the webforms db.</summary>
		public static void RunMobileWebForms(Action a) {
			Run(a,ConnectionNames.WebForms);
		}

		///<summary>Perform the given function in the context of the dental office db.</summary>
		public static T GetPractice<T>(Func<T> fn) {
			return GetT(fn,ConnectionNames.DentalOffice);
		}

		///<summary>Perform the given function in the context of the dental office db.</summary>
		public static T GetEServices<T>(Func<T> fn) {
			return GetT(fn,ConnectionNames.ServicesHQ);
		}


		///<summary>Perform the given action in the context of the bugs db.  Set useConnectionStore false to use the hard coded "server, bugs" connection string.</summary>
		public static void RunBugsHQ(Action a,bool useConnectionStore=true) {
			if(useConnectionStore) {
				Run(a,ConnectionNames.BugsHQ);
			}
			else {
			#if DEBUG
				Run(a,"localhost","bugs","root","","","");
			#else
				Run(a,"server","bugs","root","","","");
			#endif
			}
		}
		#endregion

		#region Helpers to return typed data.
		///<summary>Perform the given function in the context of the given connectionName db and return a DataTable. Typed extension of GetT.</summary>
		public static DataTable GetDataTable(Func<DataTable> fn,ConnectionNames connectionName) {
			return GetT<DataTable>(fn,connectionName);
		}

		///<summary>Perform the given function in the context of the given connectionName db and return an int. Typed extension of GetT.</summary>
		public static int GetInt(Func<int> fn,ConnectionNames connectionName) {
			return GetT<int>(fn,connectionName);
		}

		///<summary>Perform the given function in the context of the given connectionName db and return a long. Typed extension of GetT.</summary>
		public static long GetLong(Func<long> fn,ConnectionNames connectionName) {
			return GetT<long>(fn,connectionName);
		}

		///<summary>Perform the given function in the context of the given connectionName db and return a string. Typed extension of GetT.</summary>
		public static string GetString(Func<string> fn,ConnectionNames connectionName) {
			//String is a reference type and will be set to null if the method happens to fail. Correct that to empty string since OD does not typically expect string to be null.
			return GetT<string>(fn,connectionName)??"";
		}
		#endregion

		#region Run and Get
		///<summary>Perform the given action in the context of the given connectionName db.</summary>
		public static void Run(Action a,ConnectionNames connectionName) {
			GetT(new Func<object>(() => { a(); return null; }),connectionName);
		}

		///<summary>Perform the given action in the context of the given connectionString db.</summary>
		public static void Run(Action a,string connectionString,DatabaseType dbType=DatabaseType.MySql) {
			GetT(new Func<object>(() => { a(); return null; }),connectionString,dbType);
		}
		
		///<summary>Perform the given action in the context of the given connectionString db.</summary>
		public static void Run(Action a,string server,string db,string user,string password,string userLow,string passLow,DatabaseType dbType=DatabaseType.MySql) {
			GetT(new Func<object>(() => { a(); return null; }),server,db,user,password,userLow,passLow,dbType);
		}

		///<summary>Perform the given function in the context of the given connectionString db and return a T.</summary>
		public static T GetT<T>(Func<T> fn,string connectionString,DatabaseType dbType=DatabaseType.MySql) {
			return GetT(fn,new Action(() => { new DataConnection().SetDbT(connectionString,"",dbType); }));			
		}

		///<summary>Perform the given function in the context of the given connectionString db and return a T.</summary>
		public static T GetT<T>(Func<T> fn,string server,string db,string user,string password,string userLow,string passLow,DatabaseType dbType=DatabaseType.MySql) {
			return GetT(fn,new Action(() => { new DataConnection().SetDbT(server,db,user,password,userLow,passLow,dbType,true); }));
		}

		///<summary>Perform the given function in the context of the given connectionName db and return a T.</summary>
		public static T GetT<T>(Func<T> fn,ConnectionNames connectionName) {
			return GetT(fn,new Action(() => { ConnectionStore.SetDbT(connectionName); }));
		}
						
		///<summary>Perform the given function in the context of the given connectionName db and return a T.</summary>
		private static T GetT<T>(Func<T> fn,Action aSetConnection) {
			Exception eFinal=null;
			//Reference types will be set to null.
			T ret=default(T);
			ODThread th=new ODThread(new ODThread.WorkerDelegate((thread) => {
				aSetConnection();
				ret=fn();
			}));
			th.AddExceptionHandler(new ODThread.ExceptionDelegate((e) => {
				eFinal=e;
			}));
			th.Start(true);
			//This is intended to be a blocking call so give the action as long as it needs to complete.
			th.Join(System.Threading.Timeout.Infinite);
			if(eFinal!=null) { //We are back on the main thread so it is safe to throw.
				throw new Exception(eFinal.Message,eFinal);
			}
			return ret;
		}
		#endregion
	}
}
