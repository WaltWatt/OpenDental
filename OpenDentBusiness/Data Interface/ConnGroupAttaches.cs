using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ConnGroupAttaches{
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


		///<summary>Inserts, updates, or deletes database rows to match supplied list.  Must always pass in ConnectionGroupNum.</summary>
		public static void Sync(List<ConnGroupAttach> listNew,long connectionGroupNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listNew,connectionGroupNum);//never pass DB list through the web service
				return;
			}
			List<ConnGroupAttach> listDB=ConnGroupAttaches.GetForGroup(connectionGroupNum);
			Crud.ConnGroupAttachCrud.Sync(listNew,listDB);
		}

		///<summary>Gets one ConnGroupAttach from the db.</summary>
		public static ConnGroupAttach GetOne(long connGroupAttachNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<ConnGroupAttach>(MethodBase.GetCurrentMethod(),connGroupAttachNum);
			}
			return Crud.ConnGroupAttachCrud.SelectOne(connGroupAttachNum);
		}

		///<summary>Gets all conn group attaches from the database.</summary>
		public static List<ConnGroupAttach> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<ConnGroupAttach>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM conngroupattach";
			return Crud.ConnGroupAttachCrud.SelectMany(command);
		}

		///<summary>Gets all ConnGroupAttaches for a given ConnectionGroupNum.</summary>
		public static List<ConnGroupAttach> GetForGroup(long connectionGroupNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<List<ConnGroupAttach>>(MethodBase.GetCurrentMethod(),connectionGroupNum);
			}
			string command="SELECT * FROM conngroupattach WHERE ConnectionGroupNum="+POut.Long(connectionGroupNum);
			return Crud.ConnGroupAttachCrud.SelectMany(command);
		}

		///<summary>Gets all ConnGroupAttaches for a given CentralConnectionNum.</summary>
		public static List<ConnGroupAttach> GetForConnection(long connectionNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<List<ConnGroupAttach>>(MethodBase.GetCurrentMethod(),connectionNum);
			}
			string command="SELECT * FROM conngroupattach WHERE CentralConnectionNum="+POut.Long(connectionNum);
			return Crud.ConnGroupAttachCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(ConnGroupAttach connGroupAttach){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				connGroupAttach.ConnGroupAttachNum=Meth.GetLong(MethodBase.GetCurrentMethod(),connGroupAttach);
				return connGroupAttach.ConnGroupAttachNum;
			}
			return Crud.ConnGroupAttachCrud.Insert(connGroupAttach);
		}

		///<summary></summary>
		public static void Update(ConnGroupAttach connGroupAttach){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),connGroupAttach);
				return;
			}
			Crud.ConnGroupAttachCrud.Update(connGroupAttach);
		}

		///<summary></summary>
		public static void Delete(long connGroupAttachNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),connGroupAttachNum);
				return;
			}
			string command= "DELETE FROM conngroupattach WHERE ConnGroupAttachNum = "+POut.Long(connGroupAttachNum);
			Db.NonQ(command);
		}

	}
}