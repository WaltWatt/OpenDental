using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class MapAreas{
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
		public static List<MapArea> Refresh(){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<MapArea>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM maparea";
			return Crud.MapAreaCrud.SelectMany(command);
		}
		/*		
		///<summary>Gets one MapArea from the db.</summary>
		public static MapArea GetOne(long mapAreaNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<MapArea>(MethodBase.GetCurrentMethod(),mapAreaNum);
			}
			return Crud.MapAreaCrud.SelectOne(mapAreaNum);
		}
		*/
		///<summary></summary>
		public static long Insert(MapArea mapArea){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				mapArea.MapAreaNum=Meth.GetLong(MethodBase.GetCurrentMethod(),mapArea);
				return mapArea.MapAreaNum;
			}
			return Crud.MapAreaCrud.Insert(mapArea);
		}

		///<summary></summary>
		public static void Update(MapArea mapArea){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),mapArea);
				return;
			}
			Crud.MapAreaCrud.Update(mapArea);
		}

		///<summary></summary>
		public static void Delete(long mapAreaNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),mapAreaNum);
				return;
			}
			string command= "DELETE FROM maparea WHERE MapAreaNum = "+POut.Long(mapAreaNum);
			Db.NonQ(command);
		}



	}
}