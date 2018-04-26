using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;

namespace OpenDentBusiness{
	///<summary></summary>
	public class AlertItems{
		#region Get Methods
		#endregion

		#region Modification Methods
		
		#region Insert
		///<summary>Inserts a generic alert where description will show in the menu item and itemValue will be shown within a MsgBoxCopyPaste.
		///Set itemValue to more specific reason for the alert.  E.g. exception text details as to help the techs give better support.</summary>
		public static void CreateGenericAlert(string description,string itemValue) {
			AlertItem alert=new AlertItem();
			alert.Type=AlertType.Generic;
			alert.Actions=ActionType.MarkAsRead | ActionType.Delete | ActionType.ShowItemValue;
			alert.Description=description;
			alert.Severity=SeverityType.Low;
			alert.ItemValue=itemValue;
			AlertItems.Insert(alert);
		}
		#endregion

		#region Update
		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods
		#endregion

		///<summary>Returns a list of AlertItems for the given clinicNum.</summary>
		public static List<AlertItem> RefreshForClinicAndTypes(long clinicNum,List<AlertType> listAlertTypes=null){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<AlertItem>>(MethodBase.GetCurrentMethod(),clinicNum,listAlertTypes);
			}
			if(listAlertTypes==null || listAlertTypes.Count==0) {
				return new List<AlertItem>();
			}
			long provNum=0;
			if(Security.CurUser!=null && Userods.IsUserCpoe(Security.CurUser)) {
				provNum=Security.CurUser.ProvNum;
			}
			string command="";
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command="SELECT * FROM alertitem "
					+"WHERE Type IN ("+String.Join(",",listAlertTypes.Cast<int>().ToList())+") "
					//For AlertType.RadiologyProcedures we only care if the alert is associated to the current logged in provider.
					//When provNum is 0 the initial WHEN check below will not bring any rows by definition of the FKey column.
					+"AND (CASE TYPE WHEN "+POut.Int((int)AlertType.RadiologyProcedures)+" THEN FKey="+POut.Long(provNum)+" "
					+"ELSE ClinicNum = "+POut.Long(clinicNum)+" OR ClinicNum=-1 END)";
			}
			else {//oracle
				//Case statements cannot change column return results unless they are within the SELECT case.
				command="SELECT AlertItemNum,CASE Type WHEN 3 THEN ClinicNum ELSE 0 END ClinicNum,Description,Type,Severity,Actions,FormToOpen,CASE Type WHEN 3 THEN 0 ELSE FKey END FKey,ItemValue "
					+"FROM alertitem "
					+"WHERE Type IN ("+String.Join(",",listAlertTypes.Cast<int>().ToList())+") ";
			}
			return Crud.AlertItemCrud.SelectMany(command);
		}

		///<summary>Returns a list of AlertItems for the given cinicNum.</summary>
		public static List<AlertItem> RefreshForType(AlertType alertType){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<AlertItem>>(MethodBase.GetCurrentMethod(),alertType);
			}
			string command="SELECT * FROM alertitem WHERE Type = "+POut.Int((int)alertType)+" ";
			return Crud.AlertItemCrud.SelectMany(command);
		}

		///<summary>Gets one AlertItem from the db.</summary>
		public static AlertItem GetOne(long alertItemNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<AlertItem>(MethodBase.GetCurrentMethod(),alertItemNum);
			}
			return Crud.AlertItemCrud.SelectOne(alertItemNum);
		}

		///<summary></summary>
		public static long Insert(AlertItem alertItem){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				alertItem.AlertItemNum=Meth.GetLong(MethodBase.GetCurrentMethod(),alertItem);
				return alertItem.AlertItemNum;
			}
			return Crud.AlertItemCrud.Insert(alertItem);
		}

		///<summary></summary>
		public static void Update(AlertItem alertItem){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),alertItem);
				return;
			}
			Crud.AlertItemCrud.Update(alertItem);
		}
		
		///<summary>If null listFKeys is provided then all rows of the given alertType will be deleted. Otherwise only rows which match listFKeys entries.</summary>
		public static void DeleteFor(AlertType alertType,List<long> listFKeys=null) {
			//No need to check RemotingRole; no call to db.
			List<AlertItem> listAlerts=RefreshForType(alertType);
			if(listFKeys!=null) { //Narrow down to just the FKeys provided.
				listAlerts=listAlerts.FindAll(x => listFKeys.Contains(x.FKey));
			}
			foreach(AlertItem alert in listAlerts) {
				Delete(alert.AlertItemNum);
			}
		}

		///<summary>Also deletes any AlertRead objects for this AlertItem.</summary>
		public static void Delete(long alertItemNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),alertItemNum);
				return;
			}
			AlertReads.DeleteForAlertItem(alertItemNum);
			Crud.AlertItemCrud.Delete(alertItemNum);
		}

		///<summary>Inserts, updates, or deletes db rows to match listNew.  No need to pass in userNum, it's set before remoting role check and passed to
		///the server if necessary.  Doesn't create ApptComm items, but will delete them.  If you use Sync, you must create new Apptcomm items.</summary>
		public static void Sync(List<AlertItem> listNew,List<AlertItem> listOld) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listNew,listOld);
				return;
			}
			Crud.AlertItemCrud.Sync(listNew,listOld);
		}

	}
}