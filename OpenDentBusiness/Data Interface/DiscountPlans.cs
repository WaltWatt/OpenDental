using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class DiscountPlans{
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
		public static List<DiscountPlan> GetAll(bool getHidden){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<DiscountPlan>>(MethodBase.GetCurrentMethod(),getHidden);
			}
			string command="SELECT * FROM discountplan";
			if(!getHidden) {
				command+=" WHERE IsHidden=0";
			}
			return Crud.DiscountPlanCrud.SelectMany(command);
		}

		///<summary>Gets one DiscountPlan from the db.</summary>
		public static DiscountPlan GetPlan(long discountPlanNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<DiscountPlan>(MethodBase.GetCurrentMethod(),discountPlanNum);
			}
			return Crud.DiscountPlanCrud.SelectOne(discountPlanNum);
		}

		///<summary></summary>
		public static long Insert(DiscountPlan discountPlan){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				discountPlan.DiscountPlanNum=Meth.GetLong(MethodBase.GetCurrentMethod(),discountPlan);
				return discountPlan.DiscountPlanNum;
			}
			return Crud.DiscountPlanCrud.Insert(discountPlan);
		}

		///<summary></summary>
		public static void Update(DiscountPlan discountPlan){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),discountPlan);
				return;
			}
			Crud.DiscountPlanCrud.Update(discountPlan);
		}

		///<summary>Sets DiscountPlanNum to 0 for specified PatNum.</summary>
		public static void DropForPatient(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),patNum);
				return;
			}
			string command="UPDATE patient SET DiscountPlanNum=0 WHERE PatNum="+POut.Long(patNum);
			Db.NonQ(command);
		}

		///<summary>Changes the DiscountPlanNum of all patients that have _planFrom.DiscountPlanNum to _planInto.DiscountPlanNum</summary>
		public static void MergeTwoPlans(DiscountPlan planInto,DiscountPlan planFrom) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),planInto,planFrom);
				return;
			}
			string command="UPDATE patient SET DiscountPlanNum="+POut.Long(planInto.DiscountPlanNum)
				+" WHERE DiscountPlanNum="+POut.Long(planFrom.DiscountPlanNum);
			Db.NonQ(command);
			//Delete the discount plan from the database.
			Crud.DiscountPlanCrud.Delete(planFrom.DiscountPlanNum);
		}

		///<summary>Returns an empty list if planNum is 0.</summary>
		public static List<Patient> GetPatsForPlan(long planNum) {
			if(planNum==0) {
				return new List<Patient>();
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),planNum);
			}
			string command="SELECT * FROM patient WHERE DiscountPlanNum="+POut.Long(planNum);
			return Crud.PatientCrud.SelectMany(command);
		}

		///<summary>Returns an empty list if the list of plan nums is empty.</summary>
		public static List<Patient> GetPatsForPlans(List<long> listPlanNums) {
			if(listPlanNums.Count==0) {
				return new List<Patient>();
			}
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<Patient>>(MethodBase.GetCurrentMethod(),listPlanNums);
			}
			string command="SELECT * FROM patient WHERE DiscountPlanNum IN ("+string.Join(",",listPlanNums)+")";
			return Crud.PatientCrud.SelectMany(command);
		}
	}
}