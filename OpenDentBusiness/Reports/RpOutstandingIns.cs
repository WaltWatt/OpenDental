using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness {
	public class RpOutstandingIns {

		///<summary>Called from FormRpOutstandingIns. Gets outstanding insurance claims. Requires all fields. provNumList may be empty (but will return null if isAllProv is false).  listClinicNums may be empty.  dateMin and dateMax will not be used if they are set to DateTime.MinValue() (01/01/0001). If isPreauth is true only claims of type preauth will be returned.</summary>
		public static List<OutstandingInsClaim> GetOutInsClaims(List<long> listProvNums,DateTime dateFrom,
			DateTime dateTo,bool isPreauth,List<long> listClinicNums,string carrierName,List<long> listUserNums, DateFilterBy dateFilterBy)
		{ 
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) { 
				return Meth.GetObject<List<OutstandingInsClaim>>(MethodBase.GetCurrentMethod(),listProvNums,dateFrom,
					dateTo,isPreauth,listClinicNums,carrierName,listUserNums,dateFilterBy);
			} 
			string command = @"
				SELECT carrier.CarrierName, 
				carrier.Phone carrierPhone, 
				claim.ClaimType, 
				claim.DateService, 
				claim.DateSent, 
				claim.DateSentOrig DateOrigSent, 
				claim.ClaimFee, 
				claim.ClaimNum, 
				claim.ClinicNum,
				insplan.GroupNum, 
				insplan.GroupName,
				inssub.SubscriberID SubID,"
				+DbHelper.Concat("sub.LName","', '","sub.FName")+@" SubName, 
				sub.Birthdate SubDOB,				
				patient.FName PatFName, 
				patient.LName PatLName, 
				patient.MiddleI PatMiddleI, 
				patient.PatNum, 
				patient.Birthdate PatDOB,
				definition.ItemValue DaysSuppressed,"
				+DbHelper.DtimeToDate("statusHistory.DateTimeEntry")+@" DateLog,
				definition.DefNum CustomTrackingDefNum, 
				statusHistory.TrackingErrorDefNum ErrorCodeDefNum, 
				COALESCE(claimtracking.UserNum,0) UserNum 
				FROM carrier 
				INNER JOIN insplan ON insplan.CarrierNum=carrier.CarrierNum 
				INNER JOIN claim ON claim.PlanNum=insplan.PlanNum 
				AND claim.ClaimStatus='S' ";
			if(dateFrom!=DateTime.MinValue) {
				if(dateFilterBy==DateFilterBy.DateSentOrig) {
					command+="AND claim.DateSentOrig >= "+POut.Date(dateFrom)+" ";
				}
				else if(dateFilterBy==DateFilterBy.DateSent) {
					command+="AND claim.DateSent >= "+POut.Date(dateFrom)+" ";
				}
				else {
					command+="AND claim.DateService >= "+POut.Date(dateFrom)+" ";
				}
			}
			if(dateTo!=DateTime.MinValue) {
				if(dateFilterBy==DateFilterBy.DateSentOrig) {
					command+="AND claim.DateSentOrig <= "+POut.Date(dateTo)+" ";
				}
				else if(dateFilterBy==DateFilterBy.DateSent) {
					command+="AND claim.DateSent <= "+POut.Date(dateTo)+" ";
				}
				else {
					command+="AND claim.DateService <= "+POut.Date(dateTo)+" ";
				}
			}
			if(listProvNums.Count>0) {
				command+="AND claim.ProvTreat IN ("+String.Join(",",listProvNums)+") ";
			}
			if(listClinicNums.Count>0) {
				command+="AND claim.ClinicNum IN ("+String.Join(",",listClinicNums)+") ";
			}
			if(!isPreauth) {
				command+="AND claim.ClaimType!='Preauth' ";
			}
			command+="LEFT JOIN claimtracking ON claimtracking.ClaimNum=claim.ClaimNum AND TrackingType='"+POut.String(ClaimTrackingType.ClaimUser.ToString())+"' ";
			command+="LEFT JOIN definition ON definition.DefNum=claim.CustomTracking "
				+"LEFT JOIN claimtracking statusHistory ON statusHistory.ClaimNum=claim.ClaimNum "
					+"AND statusHistory.TrackingDefNum=definition.DefNum "
					+"AND statusHistory.DateTimeEntry=(SELECT MAX(ct.DateTimeEntry) FROM claimtracking ct WHERE ct.ClaimNum=claim.ClaimNum AND ct.TrackingDefNum!=0) "
					+"AND statusHistory.TrackingType='"+POut.String(ClaimTrackingType.StatusHistory.ToString())+"' "
				+"INNER JOIN patient ON patient.PatNum=claim.PatNum "
				+"LEFT JOIN inssub ON claim.InsSubNum = inssub.InsSubNum "
				+"LEFT JOIN patient sub ON inssub.Subscriber = sub.PatNum "
				+"WHERE carrier.CarrierName LIKE '%"+POut.String(carrierName.Trim())+"%' ";
			if(listUserNums.Count>0) {
				command+="AND (claimtracking.UserNum IN ("+String.Join(",",listUserNums)+") ";
				if(listUserNums.Contains(0)) {
					//Selected users includes 'Unassigned' so we want to allow claims without associated claimTracking rows to show.
					command+=" OR claimtracking.UserNum IS NULL";
				}
				command+=") ";
			}
			command+="ORDER BY carrier.CarrierName,claim.DateService,patient.LName,patient.FName,claim.ClaimType";
			object[] parameters={command};
			Plugins.HookAddCode(null,"Claims.GetOutInsClaims_beforequeryrun",parameters);//Moved entire method from Claims.cs
			command=(string)parameters[0];
			DataTable table=Db.GetTable(command);
			List<OutstandingInsClaim> listOutstandingInsClaims = table.Rows.OfType<DataRow>().Select(x => new OutstandingInsClaim(x)).ToList();
			return listOutstandingInsClaims;
		}

		[Serializable]
		public class OutstandingInsClaim {
			public string CarrierName;
			public string CarrierPhone;
			public string ClaimType;
			public string PatFName;
			public string PatLName;
			public string PatMiddleI;
			public long PatNum;
			public DateTime DateService;
			public DateTime DateSent;
			public DateTime DateOrigSent;
			public decimal ClaimFee;
			public long ClaimNum;
			public long ClinicNum;
			public int DaysSuppressed;
			public DateTime DateLog;
			public long ErrorCodeDefNum;
			public long UserNum;
			public string GroupNum;
			public string GroupName;
			public string SubName;
			public DateTime SubDOB;
			public string SubID;
			public DateTime PatDOB; 
			public long CustomTrackingDefNum;
			public long CustomTrackingAssignedUser;

			public OutstandingInsClaim() {//Required for serialization
			}

			public OutstandingInsClaim(DataRow rowCur) {
				CarrierName=rowCur["CarrierName"].ToString();
				CarrierPhone=rowCur["CarrierPhone"].ToString();
				ClaimType=rowCur["ClaimType"].ToString();
				PatFName=rowCur["PatFName"].ToString();
				PatLName=rowCur["PatLName"].ToString();
				PatMiddleI=rowCur["PatMiddleI"].ToString();
				PatNum=PIn.Long(rowCur["PatNum"].ToString());
				PatDOB=PIn.DateT(rowCur["PatDOB"].ToString());
				DateService=PIn.DateT(rowCur["DateService"].ToString());
				DateSent=PIn.DateT(rowCur["DateSent"].ToString());
				DateOrigSent=PIn.DateT(rowCur["DateOrigSent"].ToString());
				ClaimFee=PIn.Decimal(rowCur["ClaimFee"].ToString());
				ClaimNum=PIn.Long(rowCur["ClaimNum"].ToString());
				ClinicNum=PIn.Long(rowCur["ClinicNum"].ToString());
				DaysSuppressed=PIn.Int(rowCur["DaysSuppressed"].ToString());
				DateLog=PIn.DateT(rowCur["DateLog"].ToString());
				ErrorCodeDefNum=PIn.Long(rowCur["ErrorCodeDefNum"].ToString());
				GroupNum=PIn.String(rowCur["GroupNum"].ToString());
				GroupName=PIn.String(rowCur["GroupName"].ToString());
				SubName=PIn.String(rowCur["SubName"].ToString());
				SubDOB=PIn.DateT(rowCur["SubDOB"].ToString());
				SubID=PIn.String(rowCur["SubID"].ToString());
				CustomTrackingDefNum=PIn.Long(rowCur["CustomTrackingDefNum"].ToString());
				UserNum=PIn.Long(rowCur["UserNum"].ToString());
			}
		}

		public enum DateFilterBy {
			[Description("Date Sent")]
			DateSent,
			[Description("Date Sent Orig")]
			DateSentOrig,
			[Description("Date of Service")]
			DateService
		}

		public enum DateFilterTab {
			[Description("Days Old")]
			DaysOld,
			[Description("Date Range")]
			DateRange
		}

	}
}
