using System;
using OpenDentBusiness;
using System.Data;

namespace OpenDentalGraph.Cache {
	public class DashboardCacheAdjustment:DashboardCacheWithQuery<Adjustment> {
		protected override string GetCommand(DashboardFilter filter) {
			string where="";
			if(filter.UseDateFilter) {
				where="WHERE AdjDate BETWEEN "+POut.Date(filter.DateFrom)+" AND "+POut.Date(filter.DateTo)+" ";
			}
			return
				"SELECT AdjDate,ProvNum,SUM(AdjAmt) AdjTotal, ClinicNum "
				+"FROM adjustment "
				+where
				+"GROUP BY AdjDate,ProvNum,ClinicNum "
				+"HAVING AdjTotal<>0 "
				+"ORDER BY AdjDate,ProvNum ";
		}

		protected override Adjustment GetInstanceFromDataRow(DataRow x) {
			//long provNum=x.Field<long>("ProvNum");
			//string seriesName=DashboardCache.Providers.GetProvName(provNum);
			return new Adjustment() {
				ProvNum=PIn.Long(x["ProvNum"].ToString()),
				DateStamp=PIn.DateT(x["AdjDate"].ToString()),
				Val=PIn.Double(x["AdjTotal"].ToString()),
				Count=0, //count procedures, not adjustments.			
								 //SeriesName=seriesName,
				ClinicNum=PIn.Long(x["ClinicNum"].ToString()),
			};
		}
	}

	public class Adjustment:GraphQuantityOverTime.GraphDataPointClinic {
	}
}