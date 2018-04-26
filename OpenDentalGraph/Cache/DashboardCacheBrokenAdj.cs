using System;
using OpenDentBusiness;
using System.Data;

namespace OpenDentalGraph.Cache {
	public class DashboardCacheBrokenAdj:DashboardCacheWithQuery<BrokenAdj> {
		protected override string GetCommand(DashboardFilter filter) {
			string where="";
			if(filter.UseDateFilter) {
				where="WHERE DATE(AdjDate) BETWEEN "+POut.Date(filter.DateFrom)+" AND "+POut.Date(filter.DateTo)+" ";
			}
			return
				"SELECT AdjDate,ProvNum,COUNT(AdjNum) AdjCount,ClinicNum,AdjType, SUM(AdjAmt) AdjAmt "
				+"FROM adjustment "
				+"INNER JOIN definition ON definition.DefNum=adjustment.AdjType "
				+"AND definition.ItemValue = '+' "
				+where
				+"GROUP BY AdjDate,ProvNum,ClinicNum,AdjType "
				+"ORDER BY AdjDate,ProvNum,ClinicNum ";
		}

		protected override BrokenAdj GetInstanceFromDataRow(DataRow x) {
			return new BrokenAdj() {
				ProvNum=PIn.Long(x["ProvNum"].ToString()),
				ClinicNum=PIn.Long(x["ClinicNum"].ToString()),
				DateStamp=PIn.Date(x["AdjDate"].ToString()),
				Val=PIn.Double(x["AdjAmt"].ToString()),
				AdjType=PIn.Long(x["AdjType"].ToString()),
				Count=PIn.Long(x["AdjCount"].ToString()),
			};
		}
	}

	public class BrokenAdj:GraphQuantityOverTime.GraphDataPointClinic {
		public long AdjType;

	}
}