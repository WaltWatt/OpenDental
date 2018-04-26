using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SupplyOrders {
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


		///<summary>Gets all SupplyOrders for one supplier, ordered by date.</summary>
		public static List<SupplyOrder> CreateObjects(long supplierNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SupplyOrder>>(MethodBase.GetCurrentMethod(),supplierNum);
			}
			string command="SELECT * FROM supplyorder "
				+"WHERE SupplierNum="+POut.Long(supplierNum)
				+" ORDER BY DatePlaced";
			return Crud.SupplyOrderCrud.SelectMany(command);
		}

		///<summary>Gets all SupplyOrders, ordered by date.</summary>
		public static List<SupplyOrder> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SupplyOrder>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM supplyorder ORDER BY DatePlaced";
			return Crud.SupplyOrderCrud.SelectMany(command);
		}

		///<summary></summary>
		public static long Insert(SupplyOrder order) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				order.SupplyOrderNum=Meth.GetLong(MethodBase.GetCurrentMethod(),order);
				return order.SupplyOrderNum;
			}
			return Crud.SupplyOrderCrud.Insert(order);
		}

		///<summary></summary>
		public static void Update(SupplyOrder order) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),order);
				return;
			}
			Crud.SupplyOrderCrud.Update(order);
		}

		///<summary>No need to surround with try-catch.</summary>
		public static void DeleteObject(SupplyOrder order){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),order);
				return;
			}
			//validate that not already in use-no
			//delete associated orderItems
			string command="DELETE FROM supplyorderitem WHERE SupplyOrderNum="+POut.Long(order.SupplyOrderNum);
			Db.NonQ(command);
			Crud.SupplyOrderCrud.Delete(order.SupplyOrderNum);
		}

		//Retotals all items attached to order and updates AmountTotal.
		public static void UpdateOrderPrice(long orderNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),orderNum);
				return;
			}
			string command="SELECT SUM(Qty*Price) FROM supplyorderitem WHERE SupplyOrderNum="+orderNum;
			double amountTotal=PIn.Double(Db.GetScalar(command));
			command="SELECT * FROM supplyorder WHERE SupplyOrderNum="+orderNum;
			SupplyOrder so=Crud.SupplyOrderCrud.SelectOne(command);
			so.AmountTotal=amountTotal;
			SupplyOrders.Update(so);
		}




	}

	


	


}









