using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary>Not a true Cache pattern.  It only loads the cache once on startup and then never again.  No entry in the Cache file.  No InvalidType for PhoneEmpDefault.  Data is almost always pulled from db in realtime, and this cache is only used for default ringgroups.</summary>
	public class PhoneEmpDefaults{
		#region CachePattern

		///<summary>A list of all PhoneEmpDefaults.</summary>
		private static List<PhoneEmpDefault> listt;

		///<summary>A list of all PhoneEmpDefaults.</summary>
		public static List<PhoneEmpDefault> Listt{
			get {
				if(listt==null) {
					RefreshCache();
				}
				return listt;
			}
			set {
				listt=value;
			}
		}

		///<summary>Not part of the true Cache pattern.  See notes above.</summary>
		public static DataTable RefreshCache(){
			//No need to check RemotingRole; Calls GetTableRemotelyIfNeeded().
			string command="SELECT * FROM phoneempdefault";
			DataTable table=Cache.GetTableRemotelyIfNeeded(MethodBase.GetCurrentMethod(),command);
			table.TableName="PhoneEmpDefault";
			FillCache(table);
			return table;
		}

		///<summary></summary>
		public static void FillCache(DataTable table){
			//No need to check RemotingRole; no call to db.
			listt=Crud.PhoneEmpDefaultCrud.TableToList(table);
		}
		#endregion
		
		///<summary></summary>
		public static List<PhoneEmpDefault> Refresh(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<PhoneEmpDefault>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM phoneempdefault ORDER BY PhoneExt";//because that's the order we are used to in the phone panel.
			return Crud.PhoneEmpDefaultCrud.SelectMany(command);
		}

		/// <summary>use sparingly as this makes a db call every time. only used for validating user is not modifying "dirty" data</summary>
		public static bool GetGraphedStatusForEmployeeDate(long employeeNum,DateTime dateEntry) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),employeeNum,dateEntry);
			}
			PhoneEmpDefault phoneEmpDefault=Crud.PhoneEmpDefaultCrud.SelectOne(employeeNum);
			if(phoneEmpDefault==null) {
				return false;
			}
			bool isGraphed=phoneEmpDefault.IsGraphed; //get employee default
			PhoneGraph phoneGraph=PhoneGraphs.GetForEmployeeNumAndDate(employeeNum,dateEntry); //check for exception
			if(phoneGraph!=null) {//exception found so use that
				isGraphed=phoneGraph.IsGraphed;
			}
			return isGraphed;
		}

		///<summary>Gets one PhoneEmpDefault from the db.  Can return null.</summary>
		public static PhoneEmpDefault GetOne(long employeeNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<PhoneEmpDefault>(MethodBase.GetCurrentMethod(),employeeNum);
			}
			return Crud.PhoneEmpDefaultCrud.SelectOne(employeeNum);
		}

		///<summary>From local list. Can return null.</summary>
		public static PhoneEmpDefault GetEmpDefaultFromList(long employeeNum,List<PhoneEmpDefault> listPED) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<listPED.Count;i++) {
				if(listPED[i].EmployeeNum==employeeNum) {
					return listPED[i];
				}
			}
			return null;
		}

		///<summary>Can return null.</summary>
		public static PhoneEmpDefault GetByExtAndEmp(int extension,long employeeNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<PhoneEmpDefault>(MethodBase.GetCurrentMethod(),extension,employeeNum);
			}
			string command="SELECT * FROM phoneempdefault WHERE PhoneExt="+POut.Int(extension)+" "
				+"AND EmployeeNum="+POut.Long(employeeNum);
			return Crud.PhoneEmpDefaultCrud.SelectOne(command);
		}
		
		public static AsteriskQueues GetRingGroup(long employeeNum) {
			//No need to check RemotingRole; no call to db.
			for(int i=0;i<Listt.Count;i++) {
				if(Listt[i].EmployeeNum==employeeNum) {
					return Listt[i].RingGroups;
				}
			}
			return AsteriskQueues.Tech;
		}

		///<summary>Find first employee with this extension and return their IsTriageOperator flag.</summary>
		public static bool IsTriageOperatorForExtension(int extension,List<PhoneEmpDefault> listPED) {
			//No need to check RemotingRole; no call to db.
			if(extension==0) {
				return false;
			}
			for(int i=0;i<listPED.Count;i++) {
				if(listPED[i].PhoneExt==extension) {
					return listPED[i].IsTriageOperator;
				}
			}
			return false; //couldn't find extension
		}

		///<summary>The employee passed in will take over the extension passed in.  
		///Moves any other employee who currently has this extension set (in phoneempdefault) to extension zero.  
		///This prevents duplicate extensions in phoneempdefault.</summary>
		public static void SetAvailable(int extension,long empNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),extension,empNum);
				return;
			}
			Employee emp=Employees.GetEmp(empNum);
			if(emp==null) {//Should never happen. This means the employee that's changing their status doesn't exist in the employee table.
				return;
			}
			string command="UPDATE phoneempdefault "
				+"SET StatusOverride="+POut.Int((int)PhoneEmpStatusOverride.None)
					+",PhoneExt="+POut.Int(extension)
					+",EmpName='"+POut.String(emp.FName)+"' "
				+"WHERE EmployeeNum="+POut.Long(empNum);
			Db.NonQ(command);
			//Set the extension to 0 for any other employee that is using this extension to prevent duplicate rows using the same extentions.
			//This would cause confusion for the ring groups.  This is possible if a user logged off and another employee logs into their computer.
			command="UPDATE phoneempdefault SET PhoneExt=0 "
				+"WHERE PhoneExt="+POut.Int(extension)+" "
				+"AND EmployeeNum!="+POut.Long(empNum);
			Db.NonQ(command);
		}
	
		///<summary></summary>
		public static long Insert(PhoneEmpDefault phoneEmpDefault){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				phoneEmpDefault.EmployeeNum=Meth.GetLong(MethodBase.GetCurrentMethod(),phoneEmpDefault);
				return phoneEmpDefault.EmployeeNum;
			}
			return Crud.PhoneEmpDefaultCrud.Insert(phoneEmpDefault,true);//user specifies the PK
		}

		///<summary></summary>
		public static void Update(PhoneEmpDefault phoneEmpDefault){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),phoneEmpDefault);
				return;
			}
			Crud.PhoneEmpDefaultCrud.Update(phoneEmpDefault);
		}

		///<summary>Invalidates all rows' EscalationOrder and updates to the escalation ordering as given in the listPED input argument.</summary>
		/// <param name="listPED">The new list. EscalationOrder should be 1-based and ordered appropriately. Any employees that should not be included in escalation should have EscalationOrder==-1.</param>
		public static void UpdateEscalationOrder(List<PhoneEmpDefault> listPED) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listPED);
				return;
			}
			//Invalidate all rows.
			string command= "UPDATE PhoneEmpDefault SET EscalationOrder=-1";
			Db.NonQ(command);
			//Update all rows.
			for(int i=0;i<listPED.Count;i++) {
				Crud.PhoneEmpDefaultCrud.Update(listPED[i]);
			}
		}

		///<summary></summary>
		public static void Delete(long employeeNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),employeeNum);
				return;
			}
			string command= "DELETE FROM phoneempdefault WHERE EmployeeNum = "+POut.Long(employeeNum);
			Db.NonQ(command);
		}

		/// <summary>sorting class used to sort PhoneEmpDefault in various ways</summary>
		public class PhoneEmpDefaultComparer:IComparer<PhoneEmpDefault> {
			
			private SortBy SortOn = SortBy.name;
			
			public PhoneEmpDefaultComparer(SortBy sortBy) {
				SortOn=sortBy;
			}
			
			public int Compare(PhoneEmpDefault x,PhoneEmpDefault y) {
				int retVal=0;
				switch(SortOn) {
					case SortBy.empNum:
						retVal=x.EmployeeNum.CompareTo(y.EmployeeNum); 
						break;
					case SortBy.ext:
						retVal=x.PhoneExt.CompareTo(y.PhoneExt); 
						break;
					case SortBy.escalation:
						retVal=x.EscalationOrder.CompareTo(y.EscalationOrder);
						break;
					case SortBy.name:
					default:
						retVal=x.EmpName.CompareTo(y.EmpName);
						break;
				}
				if(retVal==0) {//last name is tie breaker
					return x.EmpName.CompareTo(y.EmpName);
				}
				//we got here so our sort was successful
				return retVal;
			}
			
			public enum SortBy {
				///<summary>0 - By Extension.</summary>
				ext,
				///<summary>1 - By EmployeeNum.</summary>
				empNum,
				///<summary>2 - By Name.</summary>
				name,
				///<summary>3 - By Escalation Order.</summary>
				escalation
			};
		}
	}
}