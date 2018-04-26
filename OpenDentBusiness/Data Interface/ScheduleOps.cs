using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ScheduleOps {
		#region Get Methods
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


		///<summary></summary>
		public static long Insert(ScheduleOp op) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				op.ScheduleOpNum=Meth.GetLong(MethodBase.GetCurrentMethod(),op);
				return op.ScheduleOpNum;
			}
			return Crud.ScheduleOpCrud.Insert(op);
		}

		///<summary></summary>
		public static List<ScheduleOp> GetForSched(long scheduleNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ScheduleOp>>(MethodBase.GetCurrentMethod(), scheduleNum);
			}
			string command="SELECT * FROM scheduleop ";
			command+="WHERE schedulenum = "+scheduleNum;
			return Crud.ScheduleOpCrud.SelectMany(command);
		}

		///<summary>Gets all the ScheduleOps for the list of schedules.</summary>
		public static List<ScheduleOp> GetForSchedList(List<Schedule> schedules) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ScheduleOp>>(MethodBase.GetCurrentMethod(),schedules);
			}
			if(schedules==null || schedules.Count==0) {
				return new List<ScheduleOp>();
			}
			string command="SELECT * FROM scheduleop WHERE ScheduleNum IN ("+string.Join(",",schedules.Select(x=>x.ScheduleNum))+")";
			return Crud.ScheduleOpCrud.SelectMany(command);
		}

		public static void DeleteBatch(List<long> listSchedOpNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listSchedOpNums);
				return;
			}
			if(listSchedOpNums==null || listSchedOpNums.Count==0) {
				return;
			}
			string command = "DELETE FROM scheduleop WHERE ScheduleOpNum IN ("+string.Join(",",listSchedOpNums)+")";
			Db.NonQ(command);
		}

	}

	

	

}













