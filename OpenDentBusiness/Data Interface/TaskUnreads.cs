using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class TaskUnreads{
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
		public static long Insert(TaskUnread taskUnread){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				taskUnread.TaskUnreadNum=Meth.GetLong(MethodBase.GetCurrentMethod(),taskUnread);
				return taskUnread.TaskUnreadNum;
			}
			return Crud.TaskUnreadCrud.Insert(taskUnread);
		}

		///<summary>Sets a task read by a user by deleting all the matching taskunreads.  Quick and efficient to run any time.</summary>
		public static void SetRead(long userNum,long taskNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userNum,taskNum);
				return;
			}
			string command="DELETE FROM taskunread WHERE UserNum = "+POut.Long(userNum)+" "
				+"AND TaskNum = "+POut.Long(taskNum);
			Db.NonQ(command);
		}

		public static void AddUnreads(long taskNum,long curUserNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),taskNum,curUserNum);
				return;
			}
			//if the task is done, don't add unreads
			string command = "SELECT TaskStatus,UserNum,ReminderGroupId,DateTimeEntry,"+DbHelper.Now()+" DbTime "
				+"FROM task WHERE TaskNum = "+POut.Long(taskNum);
			DataTable table=Db.GetTable(command);
			if(table.Rows.Count==0) {
				return;//only happens when a task was deleted by one user but left open on another user's computer.
			}
			TaskStatusEnum taskStatus=(TaskStatusEnum)PIn.Int(table.Rows[0]["TaskStatus"].ToString());
			long userNumOwner=PIn.Long(table.Rows[0]["UserNum"].ToString());
			if(taskStatus==TaskStatusEnum.Done) {//
				return;
			}
			//Set it unread for the original owner of the task.
			if(userNumOwner!=curUserNum) {//but only if it's some other user
				SetUnread(userNumOwner,taskNum);
			}
			//Set it for this user if a future repeating task, so it will be new when "due".  Doing this here so we don't check every row below.
			//Only for future dates because we don't want to mark as new if it was already "due" and you added a note or something.
			if((PIn.String(table.Rows[0]["ReminderGroupId"].ToString())!="")//Is a reminder
				&& (PIn.DateT(table.Rows[0]["DateTimeEntry"].ToString())>PIn.DateT(table.Rows[0]["DbTime"].ToString())))//Is "due" in the future by DbTime 
			{
				SetUnread(curUserNum,taskNum);//Set unread for current user only, other users dealt with below.
			}
			//Then, for anyone subscribed
			long userNum;
			bool isUnread;
			//task subscriptions are not cached yet, so we use a query.
			//Get a list of all subscribers to this task
			command=@"SELECT 
									tasksubscription.UserNum,
									(CASE WHEN taskunread.UserNum IS NULL THEN 0 ELSE 1 END) IsUnread
								FROM tasksubscription
								INNER JOIN tasklist ON tasksubscription.TaskListNum = tasklist.TaskListNum 
								INNER JOIN taskancestor ON taskancestor.TaskListNum = tasklist.TaskListNum 
									AND taskancestor.TaskNum = "+POut.Long(taskNum)+" ";
			command+="LEFT JOIN taskunread ON taskunread.UserNum = tasksubscription.UserNum AND taskunread.TaskNum=taskancestor.TaskNum";
			table=Db.GetTable(command);
			List<long> listUserNums=new List<long>();
			for(int i=0;i<table.Rows.Count;i++) {
				userNum=PIn.Long(table.Rows[i]["UserNum"].ToString());
				isUnread=PIn.Bool(table.Rows[i]["IsUnread"].ToString());
				if(userNum==userNumOwner//already set
					|| userNum==curUserNum//If the current user is subscribed to this task. User has obviously already read it.
					|| listUserNums.Contains(userNum)
					|| isUnread) //Unread currently exists
				{
					continue;
				}
				listUserNums.Add(userNum);
			}
			SetUnreadMany(listUserNums,taskNum);//This no longer results in duplicates like it used to
		}

		public static bool IsUnread(long userNum,long taskNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),userNum,taskNum);
			}
			string command="SELECT COUNT(*) FROM taskunread WHERE UserNum = "+POut.Long(userNum)+" "
				+"AND TaskNum = "+POut.Long(taskNum);
			if(Db.GetCount(command)=="0") {
				return false;
			}
			return true;
		}

		///<summary>Sets unread for a single user.  Works well without duplicates, whether it's already set to Unread(new) or not.</summary>
		public static void SetUnread(long userNum,long taskNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userNum,taskNum);
				return;
			}
			if(IsUnread(userNum,taskNum)) {
				return;//Already set to unread, so nothing else to do
			}
			TaskUnread taskUnread=new TaskUnread();
			taskUnread.TaskNum=taskNum;
			taskUnread.UserNum=userNum;
			Insert(taskUnread);
		}
		
		///<summary>Sets unread for a list of users.  This assumes that the list passed in has already checked for duplicate task unreads.</summary>
		public static void SetUnreadMany(List<long> listUserNums,long taskNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listUserNums,taskNum);
				return;
			}
			List<TaskUnread> listUnreadsToInsert=new List<TaskUnread>();
			foreach(long userNum in listUserNums) {
				TaskUnread taskUnread=new TaskUnread();
				taskUnread.TaskNum=taskNum;
				taskUnread.UserNum=userNum;
				listUnreadsToInsert.Add(taskUnread);
			}
			Crud.TaskUnreadCrud.InsertMany(listUnreadsToInsert);
		}

		public static void DeleteForTask(long taskNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),taskNum);
				return;
			}
			string command="DELETE FROM taskunread WHERE TaskNum = "+POut.Long(taskNum);
			Db.NonQ(command);
		}



	}
}