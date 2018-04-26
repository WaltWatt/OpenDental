using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenDentBusiness {
	///<summary></summary>
	public class Resellers {
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


		///<summary>Gets one Reseller from the db.</summary>
		public static Reseller GetOne(long resellerNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Reseller>(MethodBase.GetCurrentMethod(),resellerNum);
			}
			return Crud.ResellerCrud.SelectOne(resellerNum);
		}

		///<summary>Gets one Reseller from the db with the given patNum FK.</summary>
		public static Reseller GetOneByPatNum(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<Reseller>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM reseller "
				+"WHERE PatNum = "+POut.Long(patNum);
			return Crud.ResellerCrud.SelectOne(command);
		}

		///<summary></summary>
		public static long Insert(Reseller reseller) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				reseller.ResellerNum=Meth.GetLong(MethodBase.GetCurrentMethod(),reseller);
				return reseller.ResellerNum;
			}
			return Crud.ResellerCrud.Insert(reseller);
		}

		///<summary></summary>
		public static void Update(Reseller reseller) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),reseller);
				return;
			}
			Crud.ResellerCrud.Update(reseller);
		}

		///<summary>Make sure to check that the reseller does not have any customers before deleting them.</summary>
		public static void Delete(long resellerNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),resellerNum);
				return;
			}
			string command= "DELETE FROM reseller WHERE ResellerNum = "+POut.Long(resellerNum);
			Db.NonQ(command);
		}

		///<summary>Gets a list of resellers and some of their information.  Only used from FormResellers to fill the grid.</summary>
		public static DataTable GetResellerList() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod());
			}
			string command="SELECT ResellerNum,patient.PatNum,LName,FName,Preferred,WkPhone,WirelessPhone,PhoneNumberVal,Address,City,State,Email,PatStatus "
				+"FROM reseller "
				+"INNER JOIN patient ON reseller.PatNum=patient.PatNum "
				+"LEFT JOIN phonenumber ON phonenumber.PatNum=patient.PatNum "
				+"GROUP BY patient.PatNum "
				+"ORDER BY LName ";
			return Db.GetTable(command);
		}

		///<summary>Gets all of the customers of the reseller (family members) that have active services.  Only used from FormResellerEdit to fill the grid.</summary>
		public static DataTable GetResellerCustomersList(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT patient.PatNum,RegKey,procedurecode.ProcCode,procedurecode.Descript,resellerservice.Fee,repeatcharge.DateStart,repeatcharge.DateStop,repeatcharge.Note "
				+"FROM patient "
				+"INNER JOIN registrationkey ON patient.PatNum=registrationkey.PatNum AND IsResellerCustomer=1 "
				+"LEFT JOIN repeatcharge ON patient.PatNum=repeatcharge.PatNum "
				+"LEFT JOIN procedurecode ON repeatcharge.ProcCode=procedurecode.ProcCode "
				+"LEFT JOIN reseller ON patient.Guarantor=reseller.PatNum "
				+"LEFT JOIN resellerservice ON reseller.ResellerNum=resellerservice.resellerNum AND resellerservice.CodeNum=procedurecode.CodeNum "
				+"WHERE patient.PatNum!='"+POut.Long(patNum)+"' "
				+"AND patient.Guarantor='"+POut.Long(patNum)+"' "
				+"ORDER BY patient.LName ";
			return Db.GetTable(command);
		}

		///<summary>Checks the database to see if the user name is already in use.</summary>
		public static bool IsUserNameInUse(long patNum,string userName) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),patNum,userName);
			}
			string command="SELECT COUNT(*) FROM reseller WHERE PatNum!="+POut.Long(patNum)+" AND UserName='"+POut.String(userName)+"'";
			if(PIn.Int(Db.GetScalar(command))>0) {
				return true;//User name in use.
			}
			return false;
		}

		///<summary>Checks the database to see if the patient is a reseller.</summary>
		public static bool IsResellerFamily(long guarantor) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),guarantor);
			}
			string command="SELECT COUNT(*) FROM reseller WHERE PatNum="+POut.Long(guarantor);
			if(PIn.Int(Db.GetScalar(command))>0) {
				return true;
			}
			return false;
		}

		///<summary>Checks the database to see if the reseller has customers with active repeating charges.</summary>
		public static bool HasActiveResellerCustomers(Reseller reseller) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),reseller);
			}
			string command=@"SELECT COUNT(*) FROM patient
				INNER JOIN registrationkey ON patient.PatNum=registrationkey.PatNum AND IsResellerCustomer=1
				INNER JOIN repeatcharge ON patient.PatNum=repeatcharge.PatNum
				INNER JOIN procedurecode ON repeatcharge.ProcCode=procedurecode.ProcCode
				INNER JOIN resellerservice ON procedurecode.CodeNum=resellerservice.CodeNum 
				WHERE ((DATE(repeatcharge.DateStart)<=DATE(NOW()) AND ((YEAR(repeatcharge.DateStop)<1880) OR (DATE(NOW()<DATE(repeatcharge.DateStop))))))
				AND patient.Guarantor="+POut.Long(reseller.PatNum)+" "
				+"AND resellerservice.ResellerNum="+POut.Long(reseller.ResellerNum)+" "
				+"GROUP BY patient.PatNum";
			if(PIn.Int(Db.GetScalar(command))>0) {
				return true;
			}
			return false;
		}



	}
}