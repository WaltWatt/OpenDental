using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class RxPats {
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


		///<summary>Returns a list of RxPats containing the passed in PatNum.</summary>
		public static List<RxPat> GetAllForPat(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<RxPat>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM rxpat WHERE PatNum="+POut.Long(patNum)+" ";
			return Crud.RxPatCrud.SelectMany(command);
		}

		///<summary>Used in Ehr.  Excludes controlled substances.</summary>
		public static List<RxPat> GetPermissableForDateRange(long patNum,DateTime dateStart,DateTime dateStop) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<RxPat>>(MethodBase.GetCurrentMethod(),patNum,dateStart,dateStop);
			}
			string command="SELECT * FROM rxpat WHERE PatNum="+POut.Long(patNum)+" "
				+"AND RxDate >= "+POut.Date(dateStart)+" "
				+"AND RxDate <= "+POut.Date(dateStop)+" "
				+"AND IsControlled = 0";
			return Crud.RxPatCrud.SelectMany(command);
		}

		///<summary></summary>
		public static RxPat GetRx(long rxNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<RxPat>(MethodBase.GetCurrentMethod(),rxNum);
			}
			return Crud.RxPatCrud.SelectOne(rxNum);
		}

		///<summary></summary>
		public static void Update(RxPat rx) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),rx);
				return;
			}
			Crud.RxPatCrud.Update(rx);
		}

		public static bool Update(RxPat rx,RxPat oldRx) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),rx,oldRx);
			}
			return Crud.RxPatCrud.Update(rx,oldRx);
		}

		///<summary></summary>
		public static long Insert(RxPat rx) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				rx.RxNum=Meth.GetLong(MethodBase.GetCurrentMethod(),rx);
				return rx.RxNum;
			}
			return Crud.RxPatCrud.Insert(rx);
		}

		///<summary></summary>
		public static void Delete(long rxNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),rxNum);
				return;
			}
			Crud.RxPatCrud.Delete(rxNum);
		}

		public static List<long> GetChangedSinceRxNums(DateTime changedSince) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long>>(MethodBase.GetCurrentMethod(),changedSince);
			}
			string command="SELECT RxNum FROM rxpat WHERE DateTStamp > "+POut.DateT(changedSince);
			DataTable dt=Db.GetTable(command);
			List<long> rxnums = new List<long>(dt.Rows.Count);
			for(int i=0;i<dt.Rows.Count;i++) {
				rxnums.Add(PIn.Long(dt.Rows[i]["RxNum"].ToString()));
			}
			return rxnums;
		}

		///<summary>Used along with GetChangedSinceRxNums</summary>
		public static List<RxPat> GetMultRxPats(List<long> rxNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<RxPat>>(MethodBase.GetCurrentMethod(),rxNums);
			}
			string strRxNums="";
			DataTable table;
			if(rxNums.Count>0) {
				for(int i=0;i<rxNums.Count;i++) {
					if(i>0) {
						strRxNums+="OR ";
					}
					strRxNums+="RxNum='"+rxNums[i].ToString()+"' ";
				}
				string command="SELECT * FROM rxpat WHERE "+strRxNums;
				table=Db.GetTable(command);
			}
			else {
				table=new DataTable();
			}
			RxPat[] multRxs=Crud.RxPatCrud.TableToList(table).ToArray();
			List<RxPat> rxList=new List<RxPat>(multRxs);
			return rxList;
		}

		///<summary>Used in FormRxSend to fill electronic queue.</summary>
		public static List<RxPat> GetQueue() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<RxPat>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM rxpat WHERE SendStatus=1";
			return Crud.RxPatCrud.SelectMany(command);
		}

		///<summary></summary>
		public static RxPat GetErxByIdForPat(string erxGuid,long patNum=0) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<RxPat>(MethodBase.GetCurrentMethod(),erxGuid,patNum);
			}
			string command="SELECT * FROM rxpat WHERE ErxGuid='"+POut.String(erxGuid)+"'";
			if(patNum!=0) {
				command+=" AND PatNum="+POut.Long(patNum);
			}
			List<RxPat> rxNewCrop=Crud.RxPatCrud.SelectMany(command);
			if(rxNewCrop.Count==0) {
				return null;
			}
			return rxNewCrop[0];
		}

		///<summary>Zeros securitylog FKey column for rows that are using the matching rxNum as FKey and are related to RxPat.
		///Permtypes are generated from the AuditPerms property of the CrudTableAttribute within the RxPat table type.</summary>
		public static void ClearFkey(long rxNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),rxNum);
				return;
			}
			Crud.RxPatCrud.ClearFkey(rxNum);
		}

		///<summary>Zeros securitylog FKey column for rows that are using the matching rxNums as FKey and are related to RxPat.
		///Permtypes are generated from the AuditPerms property of the CrudTableAttribute within the RxPat table type.</summary>
		public static void ClearFkey(List<long> listRxNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listRxNums);
				return;
			}
			Crud.RxPatCrud.ClearFkey(listRxNums);
		}
	}

	


}













