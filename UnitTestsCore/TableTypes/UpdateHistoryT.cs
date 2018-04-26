using System;
using System.Collections.Generic;
using System.Linq;
using OpenDentBusiness;

namespace UnitTestsCore {
	public class UpdateHistoryT {
		///<summary>Inserts an UpdateHistory for the version passed in if there is no entry for that version. Returns the primary key of the row inserted
		///or 0 if an UpdateHistory of that version already exists.</summary>
		public static long CreateUpdateHistory(string version) {
			List<UpdateHistory> listUpdates=UpdateHistories.GetAll();
			if(listUpdates.Any(x=>x.ProgramVersion==version)) {
				return 0;
			}
			UpdateHistory update=new UpdateHistory();
			update.ProgramVersion="16.1.1.0";
			long updateHistoryNum=UpdateHistories.Insert(update);
			return updateHistoryNum;
		}
	}
}
