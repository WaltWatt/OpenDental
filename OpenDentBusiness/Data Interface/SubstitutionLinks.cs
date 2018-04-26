using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SubstitutionLinks{
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary>Gets one SubstitutionLink from the db.</summary>
		public static SubstitutionLink GetOne(long substitutionLinkNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<SubstitutionLink>(MethodBase.GetCurrentMethod(),substitutionLinkNum);
			}
			return Crud.SubstitutionLinkCrud.SelectOne(substitutionLinkNum);
		}

		///<summary></summary>
		public static long Insert(SubstitutionLink substitutionLink){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				substitutionLink.SubstitutionLinkNum=Meth.GetLong(MethodBase.GetCurrentMethod(),substitutionLink);
				return substitutionLink.SubstitutionLinkNum;
			}
			return Crud.SubstitutionLinkCrud.Insert(substitutionLink);
		}

		///<summary></summary>
		public static void Update(SubstitutionLink substitutionLink){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),substitutionLink);
				return;
			}
			Crud.SubstitutionLinkCrud.Update(substitutionLink);
		}

		///<summary></summary>
		public static void Delete(long substitutionLinkNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),substitutionLinkNum);
				return;
			}
			Crud.SubstitutionLinkCrud.Delete(substitutionLinkNum);
		}

		

		
		*/

		///<summary></summary>
		public static List<SubstitutionLink> GetAllForPlans(List<InsPlan> listInsPlans) {
			//No need to check RemotingRole; no call to db.
			return GetAllForPlans(listInsPlans.Select(x => x.PlanNum).ToArray());
		}

		///<summary></summary>
		public static List<SubstitutionLink> GetAllForPlans(params long[] arrayPlanNums){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SubstitutionLink>>(MethodBase.GetCurrentMethod(),arrayPlanNums);
			}
			if(arrayPlanNums.Length==0) {
				return new List<SubstitutionLink>();
			}
			List <long> listPlanNums=new List<long>(arrayPlanNums);
			string command="SELECT * FROM substitutionlink WHERE PlanNum IN("+String.Join(",",listPlanNums.Select(x => POut.Long(x)))+")";
			return Crud.SubstitutionLinkCrud.SelectMany(command);
		}

		///<summary>Inserts, updates, or deletes the passed in list against the stale list listOld.  Returns true if db changes were made.</summary>
		public static bool Sync(List<SubstitutionLink> listNew,List<SubstitutionLink> listOld) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),listNew,listOld);
			}
			return Crud.SubstitutionLinkCrud.Sync(listNew,listOld);
		}

		public static bool HasSubstCodeForPlan(InsPlan insPlan,long codeNum,List<SubstitutionLink> listSubLinks) {
			//No need to check RemotingRole; no call to db.
			if(insPlan.CodeSubstNone) {
				return false;
			}
			return !listSubLinks.Exists(x => x.PlanNum==insPlan.PlanNum && x.CodeNum==codeNum);
		}

	}
}