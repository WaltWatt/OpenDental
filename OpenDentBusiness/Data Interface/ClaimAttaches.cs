using System;
using System.Collections;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class ClaimAttaches{
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
		public static long Insert(ClaimAttach attach) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				attach.ClaimAttachNum=Meth.GetLong(MethodBase.GetCurrentMethod(),attach);
				return attach.ClaimAttachNum;
			}
			return Crud.ClaimAttachCrud.Insert(attach);
		}


	}

	


}









