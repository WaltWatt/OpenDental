using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using OpenDentBusiness.Crud;
using CodeBase;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Operatories {
		#region Get Methods

		public static string GetAbbrev(long operatoryNum) {
			//No need to check RemotingRole; no call to db.
			Operatory operatory=GetFirstOrDefault(x => x.OperatoryNum==operatoryNum);
			return (operatory==null ? "" : operatory.Abbrev);
		}

		public static string GetOpName(long operatoryNum) {
			//No need to check RemotingRole; no call to db.
			Operatory operatory=GetFirstOrDefault(x => x.OperatoryNum==operatoryNum);
			return (operatory==null ? "" : operatory.OpName);
		}

		///<summary>Gets the order of the op within ListShort or -1 if not found.</summary>
		public static int GetOrder(long opNum) {
			//No need to check RemotingRole; no call to db.
			return _operatoryCache.GetFindIndex(x => x.OperatoryNum==opNum,true);
		}

		///<summary>Gets operatory from the cache.</summary>
		public static Operatory GetOperatory(long operatoryNum) {
			//No need to check RemotingRole; no call to db.
			return GetFirstOrDefault(x => x.OperatoryNum==operatoryNum);
		}

		///<summary>Get all non-hidden operatories for the clinic passed in.</summary>
		public static List<Operatory> GetOpsForClinic(long clinicNum) {
			//No need to check RemotingRole; no call to db.
			return GetWhere(x => x.ClinicNum==clinicNum,true);
		}

		public static List<Operatory> GetOpsForWebSched() {
			//No need to check RemotingRole; no call to db.
			//Only return the ops flagged as IsWebSched.
			return GetWhere(x => x.IsWebSched,true);
		}

		///<summary>Returns operatories that are associated to a WebSchedNewPatApptTypes definition.</summary>
		public static List<Operatory> GetOpsForWebSchedNewPatAppts(bool isShort=true) {
			//No need to check RemotingRole; no call to db.
			//Get all of the deflinks that are of type Operatory in order to get the operatory specific FKeys.
			List<long> listOperatoryNums=DefLinks.GetDefLinksForWebSchedNewPatApptOperatories()
				.Select(x => x.FKey)
				.Distinct()
				.ToList();
			return GetWhere(x => listOperatoryNums.Contains(x.OperatoryNum),isShort);
		}

		///<summary>Returns operatories that are associated to the WebSchedNewPatApptTypes definition passed in.</summary>
		public static List<Operatory> GetOpsForWebSchedNewPatApptDef(long defNum,bool isShort=true) {
			//No need to check RemotingRole; no call to db.
			List<long> listOperatoryNums=DefLinks.GetDefLinksForWebSchedNewPatApptOperatories()
				.Where(x => x.DefNum==defNum)
				.Select(x => x.FKey)
				.Distinct()
				.ToList();
			return GetWhere(x => listOperatoryNums.Contains(x.OperatoryNum),isShort);
		}
		#endregion

		#region Modification Methods
		
		#region Insert
		#endregion

		#region Update
		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods
		#endregion

		#region CachePattern

		private class OperatoryCache : CacheListAbs<Operatory> {
			protected override List<Operatory> GetCacheFromDb() {
				string command= @"
				SELECT operatory.*, CASE WHEN apptviewop.OpNum IS NULL THEN 0 ELSE 1 END IsInHQView
				FROM operatory
				LEFT JOIN (
					SELECT apptviewitem.OpNum
					FROM apptviewitem
					INNER JOIN apptview ON apptview.ApptViewNum = apptviewitem.ApptViewNum
						AND apptview.ClinicNum = 0
					GROUP BY apptviewitem.OpNum
				)apptviewop ON operatory.OperatoryNum = apptviewop.OpNum
				ORDER BY ItemOrder";
				return TableToList(Db.GetTable(command));
			}
			protected override List<Operatory> TableToList(DataTable table) {
				List<Operatory> listOps=Crud.OperatoryCrud.TableToList(table);
				//The IsInHQView flag is not important enough to cause filling the cache to fail.
				ODException.SwallowAnyException(() => {
					for(int i=0;i<table.Rows.Count;i++) {
						listOps[i].IsInHQView=PIn.Bool(table.Rows[i]["IsInHQView"].ToString());
					}
				});
				//WSNPA operatory defs are important enough that we want this portion to fail if it has problems.
				//Create a dictionary comprised of Key: OperatoryNum and value: List of definition DefNums.
				Dictionary<long,List<long>> dictWSNPAOperatoryDefNums=DefLinks.GetDefLinksByType(DefLinkType.Operatory)
					.GroupBy(x => x.FKey)//FKey for DefLinkType.Operatory is OperatoryNum
					.ToDictionary(x => x.Key,x => x.Select(y => y.DefNum).ToList());
				foreach(long operatoryNum in dictWSNPAOperatoryDefNums.Keys) {
					Operatory op=listOps.FirstOrDefault(x => x.OperatoryNum==operatoryNum);
					if(op!=null) {
						op.ListWSNPAOperatoryDefNums=dictWSNPAOperatoryDefNums[operatoryNum];
					}
				}
				return listOps;
			}
			protected override Operatory Copy(Operatory operatory) {
				return operatory.Copy();
			}
			protected override DataTable ListToTable(List<Operatory> listOperatories) {
				DataTable table=Crud.OperatoryCrud.ListToTable(listOperatories,"Operatory");
				//The IsInHQView flag is not important enough to cause filling the cache to fail.
				try {
					table.Columns.Add("IsInHQView");
					for(int i=0;i<table.Rows.Count;i++) {
						table.Rows[i]["IsInHQView"]=POut.Bool(listOperatories[i].IsInHQView);
					}
				}
				catch(Exception e) {
					e.DoNothing();
				}
				return table;
			}
			protected override void FillCacheIfNeeded() {
				Operatories.GetTableFromCache(false);
			}
			protected override bool IsInListShort(Operatory operatory) {
				return !operatory.IsHidden;
			}
		}

		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static OperatoryCache _operatoryCache=new OperatoryCache();

		///<summary>Gets a deep copy of all matching items from the cache via ListLong.  Set isShort true to search through ListShort instead.</summary>
		public static List<Operatory> GetWhere(Predicate<Operatory> match,bool isShort=false) {
			return _operatoryCache.GetWhere(match,isShort);
		}

		public static int GetCount(bool isShort=false) {
			return _operatoryCache.GetCount(isShort);
		}

		public static List<Operatory> GetDeepCopy(bool isShort=false) {
			return _operatoryCache.GetDeepCopy(isShort);
		}

		public static Operatory GetFirstOrDefault(Func<Operatory,bool> match,bool isShort=false) {
			return _operatoryCache.GetFirstOrDefault(match,isShort);
		}

		public static Operatory GetFirst(Func<Operatory,bool> match,bool isShort=false) {
			return _operatoryCache.GetFirst(match,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_operatoryCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_operatoryCache.FillCacheFromTable(table);
				return table;
			}
			return _operatoryCache.GetTableFromCache(doRefreshCache);
		}

		#endregion

		#region Sync Pattern

		///<summary>Inserts, updates, or deletes database rows to match supplied list.
		///Also syncs each operatory's deflink entries if the operatory.ListWSNPAOperatoryDefNums is not null.</summary>
		public static void Sync(List<Operatory> listNew,List<Operatory> listOld) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listNew,listOld);//never pass DB list through the web service
				return;
			}
			Crud.OperatoryCrud.Sync(listNew,listOld);
			//Regardless if changes were made during the sync, we need to make sure to sync the DefLinks for WSNPA appointment types.
			//This needs to happen after the sync call so that the PKs have been correctly set for listNew.
			List<DefLink> listDefLinksAll=DefLinks.GetDefLinksForWebSchedNewPatApptOperatories();
			foreach(Operatory operatory in listNew) {
				DefLinks.SyncWebSchedNewPatApptOpLinks(operatory,listDefLinksAll);
			}
			//Delete any deflinks for operatories that are present within listOld but are not present within listNew.
			List<long> listDeleteOpNums=listOld.Where(x => !listNew.Any(y => y.OperatoryNum==x.OperatoryNum))
				.Select(x => x.OperatoryNum)
				.Distinct()
				.ToList();
			DefLinks.DeleteAllForFKeys(listDeleteOpNums,DefLinkType.Operatory);
		}

		#endregion

		///<summary></summary>
		public static long Insert(Operatory operatory) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				operatory.OperatoryNum=Meth.GetLong(MethodBase.GetCurrentMethod(),operatory);
				return operatory.OperatoryNum;
			}
			return Crud.OperatoryCrud.Insert(operatory);
		}

		///<summary></summary>
		public static void Update(Operatory operatory) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),operatory);
				return;
			}
			Crud.OperatoryCrud.Update(operatory);
		}

		//<summary>Checks dependencies first.  Throws exception if can't delete.</summary>
		//public void Delete(){//no such thing as delete.  Hide instead
		//}

		public static List<Operatory> GetChangedSince(DateTime changedSince) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Operatory>>(MethodBase.GetCurrentMethod(),changedSince);
			}
			string command="SELECT * FROM operatory WHERE DateTStamp > "+POut.DateT(changedSince);
			return Crud.OperatoryCrud.SelectMany(command);
		}

		///<summary>Gets a list of all future appointments for a given Operatory.  Ordered by dateTime</summary>
		public static bool HasFutureApts(long operatoryNum,params ApptStatus[] arrayIgnoreStatuses) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),operatoryNum,arrayIgnoreStatuses);
			}
			string command="SELECT COUNT(*) FROM appointment "
				+"WHERE Op = "+POut.Long(operatoryNum)+" ";
			if(arrayIgnoreStatuses.Length > 0) {
				command+="AND AptStatus NOT IN (";
				for(int i=0;i<arrayIgnoreStatuses.Length;i++) {
					if(i > 0) {
						command+=",";
					}
					command+=POut.Int((int)arrayIgnoreStatuses[i]);
				}
				command+=") ";
			}
			command+="AND AptDateTime > "+DbHelper.Now();
			return PIn.Int(Db.GetScalar(command))>0;
		}

		///<summary>Returns a list of all appointments and whether that appointment has a conflict for the given listChildOpNums.
		///Used to determine if there are any overlapping appointments for ALL time between a 'master' op appointments and the 'child' ops appointments.
		///If an appointment from one of the give child ops has a confilict with the master op, then the appointment.Tag will be true.
		///Throws exceptions.</summary>
		public static List<ODTuple<Appointment,bool>> MergeApptCheck(long masterOpNum,List<long> listChildOpNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ODTuple<Appointment,bool>>>(MethodBase.GetCurrentMethod(),masterOpNum,listChildOpNums);
			}
			if(listChildOpNums==null || listChildOpNums.Count==0) {
				return new List<ODTuple<Appointment,bool>>();
			}
			if(listChildOpNums.Contains(masterOpNum)) {
				throw new ApplicationException(Lans.g("Operatories","The operatory to keep cannot be within the selected list of operatories to combine."));
			}
			List<int> listApptStatus=new List<int>();
			listApptStatus.Add((int)ApptStatus.Scheduled);
			listApptStatus.Add((int)ApptStatus.Complete);
			listApptStatus.Add((int)ApptStatus.Broken);
			listApptStatus.Add((int)ApptStatus.PtNote);
			//12/09/2016 - Query below originally created by Allen and moddified for job by Joe.
			string command=
			"SELECT childAppointments.*, "+
				"MAX(CASE WHEN "+
				"(childAppointments.AptDateTime <= MasterAppointments.AptDateTime AND childAppointments.AptDateTime + INTERVAL (LENGTH(childAppointments.Pattern) * 5) MINUTE > MasterAppointments.AptDateTime) "+
				"OR "+
				"(MasterAppointments.AptDateTime <= childAppointments.AptDateTime AND MasterAppointments.AptDateTime + INTERVAL (LENGTH(MasterAppointments.Pattern) * 5) MINUTE > childAppointments.AptDateTime) "+
				"THEN 1 ELSE 0 END) AS HasConflict "+
				"FROM ( "+
					"SELECT * "+
					"FROM appointment "+
					"WHERE Op ="+POut.Long(masterOpNum)+" "+
					"AND aptstatus IN ("+String.Join(",",listApptStatus)+") "+
				") MasterAppointments "+
				"CROSS JOIN ( "+
					"SELECT * "+
					"FROM appointment "+
					"WHERE Op IN ("+String.Join(",",listChildOpNums)+") "+
					"AND aptstatus IN ("+String.Join(",",listApptStatus)+") "+
				") childAppointments "+
				"GROUP BY childAppointments.AptNum";
			DataTable table=Db.GetTable(command);
			List<Appointment> list=AppointmentCrud.TableToList(table);
			List<ODTuple<Appointment,bool>> listTuplesAptConflicts=new List<ODTuple<Appointment,bool>>();
			for(int i=0; i<table.Rows.Count; i++) {
				Appointment appt=list.First(x => x.AptNum==PIn.Long(table.Rows[i]["AptNum"].ToString()));//Safe
				bool hasConflict=PIn.Bool(table.Rows[i]["HasConflict"].ToString());
				listTuplesAptConflicts.Add(new ODTuple<Appointment,bool>(appt,hasConflict));
			}
			return listTuplesAptConflicts;
		}

		///<summary>Hides all operatories that are not the master op and moves any appointments passed in into the master op.
		///Throws exceptions</summary>
		public static void MergeOperatoriesIntoMaster(long masterOpNum,List<long> listOpNumsToMerge,List<Appointment> listApptsToMerge) {
			//No need to check RemotingRole; No db call.
			List<Operatory> listOps=Operatories.GetDeepCopy();
			Operatory masterOp=listOps.FirstOrDefault(x => x.OperatoryNum==masterOpNum);
			if(masterOp==null) {
				throw new ApplicationException(Lans.g("Operatories","Operatory to merge into no longer exists."));
			}
			if(listApptsToMerge.Count>0) {
				//All appts in listAppts are appts that we are going to move to new op.
				List<Appointment> listApptsNew=listApptsToMerge.Select(x => x.Copy()).ToList();//Copy object so that we do not change original object in memory.
				listApptsNew.ForEach(x => x.Op=masterOpNum);//Associate to new op selection
				Appointments.Sync(listApptsNew,listApptsToMerge,0);
			}
			List<Operatory> listOpsToMerge=listOps.Select(x=> x.Copy()).ToList();//Copy object so that we do not change original object in memory.
			listOpsToMerge.FindAll(x => x.OperatoryNum!=masterOpNum && listOpNumsToMerge.Contains(x.OperatoryNum))
				.ForEach(x => x.IsHidden=true);
			Operatories.Sync(listOpsToMerge,listOps);
			SecurityLogs.MakeLogEntry(Permissions.Setup,0
				,Lans.g("Operatories","The following operatories and all of their appointments were merged into the")
					+" "+masterOp.Abbrev+" "+Lans.g("Operatories","operatory;")+" "
					+string.Join(", ",listOpsToMerge.FindAll(x => x.OperatoryNum!=masterOpNum && listOpNumsToMerge.Contains(x.OperatoryNum)).Select(x => x.Abbrev)));
		}
	}
	


}













