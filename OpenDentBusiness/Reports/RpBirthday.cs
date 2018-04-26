using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace OpenDentBusiness {
	public class RpBirthday {

		public static DataTable GetBirthdayTable(DateTime dateFrom,DateTime dateTo) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),dateFrom,dateTo);
			}
			string dateWhereClause;
			string orderByClause;
			if(dateFrom.Year==dateTo.Year) {
				dateWhereClause="SUBSTRING(Birthdate,6,5) >= '"+dateFrom.ToString("MM-dd")+"' "
				+"AND SUBSTRING(Birthdate,6,5) <= '"+dateTo.ToString("MM-dd")+"' ";
				orderByClause="MONTH(Birthdate),DAY(Birthdate)";
			}
			else {//The date range spans more than 1 calendar year
				dateWhereClause="(SUBSTRING(Birthdate,6,5) >= '"+dateFrom.ToString("MM-dd")+"' "
				+"OR SUBSTRING(Birthdate,6,5) <= '"+dateTo.ToString("MM-dd")+"') ";
				orderByClause="SUBSTRING(Birthdate,6,5) < '"+dateFrom.ToString("MM-dd")+"',MONTH(Birthdate),DAY(Birthdate)";
			}
			string command="SELECT LName,FName,Preferred,Address,Address2,City,State,Zip,Birthdate "
				+"FROM patient " 
				+"WHERE "+dateWhereClause+" "
				+"AND Birthdate > '1880-01-01' "
				+"AND PatStatus=0	"
				+"ORDER BY "+orderByClause;
			DataTable table=ReportsComplex.RunFuncOnReportServer(() => ReportsComplex.GetTable(command));
			table.Columns.Add("Age");
			for(int i=0;i<table.Rows.Count;i++) {
				table.Rows[i]["Age"]=Patients.DateToAge(PIn.Date(table.Rows[i]["Birthdate"].ToString()),dateTo).ToString();
			}
			return table;
		}
	}
}

