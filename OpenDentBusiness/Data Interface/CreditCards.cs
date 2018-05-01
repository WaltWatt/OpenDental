using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class CreditCards{
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


		///<summary>If patNum==0 then does not filter on PatNum; otherwise filters on PatNum. Only includes credit cards whose source is Open Dental
		///proper.</summary>
		public static List<CreditCard> Refresh(long patNum){
			//No need to check RemotingRole; no call to db.
			return RefreshBySource(patNum,new List<CreditCardSource>() { CreditCardSource.None,CreditCardSource.PayConnect,CreditCardSource.XServer,
				CreditCardSource.XServerPayConnect,CreditCardSource.XWeb,CreditCardSource.XWebPortalLogin,CreditCardSource.PaySimple });
		}

		///<summary>Get all credit cards by a given list of CreditCardSource(s). Optionally filter by a given patNum.</summary>
		public static List<CreditCard> RefreshBySource(long patNum,List<CreditCardSource> listSources) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<CreditCard>>(MethodBase.GetCurrentMethod(),patNum,listSources);
			}
			if(listSources.Count==0) {
				return new List<CreditCard>();
			}
			string command="SELECT * FROM creditcard WHERE CCSource IN ("+string.Join(",",listSources.Select(x=>POut.Int((int)x)))+") ";
			if(patNum!=0) { //Add the PatNum criteria.
				command+="AND PatNum="+POut.Long(patNum)+" ";
			}
			command+="ORDER BY ItemOrder DESC";
			return Crud.CreditCardCrud.SelectMany(command);
		}

		///<summary>Gets one CreditCard from the db.</summary>
		public static CreditCard GetOne(long creditCardNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<CreditCard>(MethodBase.GetCurrentMethod(),creditCardNum);
			}
			return Crud.CreditCardCrud.SelectOne(creditCardNum);
		}

		///<summary></summary>
		public static long Insert(CreditCard creditCard){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				creditCard.CreditCardNum=Meth.GetLong(MethodBase.GetCurrentMethod(),creditCard);
				return creditCard.CreditCardNum;
			}
			return Crud.CreditCardCrud.Insert(creditCard);
		}

		///<summary>Validate xWebResponse and create a new credit card from the XWebResponse.</summary>
		public static long InsertFromXWeb(XWebResponse xWebResponse) {
			//No need to check RemotingRole;no call to db.
			if(GetTokenCount(xWebResponse.Alias,
				new List<CreditCardSource> { CreditCardSource.XWeb,CreditCardSource.XWebPortalLogin })>0) 
			{ //Prevent duplicates.
				throw new Exception("XCharge token already exists: "+xWebResponse.Alias);
			}
			return Insert(new CreditCard() {
				PatNum=xWebResponse.PatNum,
				XChargeToken=xWebResponse.Alias,
				CCNumberMasked=xWebResponse.MaskedAcctNum,
				CCExpiration=xWebResponse.AccountExpirationDate,
				CCSource=xWebResponse.CCSource,
				ClinicNum=xWebResponse.ClinicNum,
				ItemOrder=Refresh(xWebResponse.PatNum).Count,
				Address="",
				Zip="",
				ChargeAmt=0,
				DateStart=DateTime.MinValue,
				DateStop=DateTime.MinValue,
				Note="",
				PayPlanNum=0,
				PayConnectToken="",
				PayConnectTokenExp=DateTime.MinValue,
				PaySimpleToken="",
				Procedures="",
			});
		}

		///<summary></summary>
		public static void Update(CreditCard creditCard){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),creditCard);
				return;
			}
			Crud.CreditCardCrud.Update(creditCard);
		}

		///<summary></summary>
		public static void Delete(long creditCardNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),creditCardNum);
				return;
			}
			string command= "DELETE FROM creditcard WHERE CreditCardNum = "+POut.Long(creditCardNum);
			Db.NonQ(command);
		}

		///<summary>Gets the masked CC# and exp date for all cards setup for monthly charges for the specified patient.  Only used for filling [CreditCardsOnFile] variable when emailing statements.</summary>
		public static string GetMonthlyCardsOnFile(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetString(MethodBase.GetCurrentMethod(),patNum);
			}
			string result="";
			string command="SELECT * FROM creditcard WHERE PatNum="+POut.Long(patNum)
				+" AND ("+DbHelper.Year("DateStop")+"<1880 OR DateStop>"+DbHelper.Now()+") "//Recurring card is active.
				+" AND ChargeAmt>0"
				+" AND CCSource NOT IN ("+(int)CreditCardSource.XWeb+","+(int)CreditCardSource.XWebPortalLogin+") ";//Not created from the Patient Portal
			List<CreditCard> monthlyCards=Crud.CreditCardCrud.SelectMany(command);
			for(int i=0;i<monthlyCards.Count;i++) {
				if(i>0) {
					result+=", ";
				}
				result+=monthlyCards[i].CCNumberMasked+" exp:"+monthlyCards[i].CCExpiration.ToString("MM/yy");
			}
			return result;
		}

		///<summary>Returns list of active credit cards.</summary>
		public static List<CreditCard> GetActiveCards(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<CreditCard>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM creditcard WHERE PatNum="+POut.Long(patNum)
				+" AND ("+DbHelper.Year("DateStop")+"<1880 OR DateStop>="+DbHelper.Curdate()+") "
				+" AND ("+DbHelper.Year("DateStart")+">1880 AND DateStart<="+DbHelper.Curdate()+") "//Recurring card is active.
				+" AND CCSource NOT IN ("+(int)CreditCardSource.XWeb+","+(int)CreditCardSource.XWebPortalLogin+") ";//Not created from the Patient Portal
			return Crud.CreditCardCrud.SelectMany(command);
		}

		///<summary>Updates the Procedures column on all cards that have not stopped that are not marked to exclude from sync.  Only used at HQ.</summary>
		public static void SyncDefaultProcs(List<string> listProcCodes) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listProcCodes);
				return;
			}
			string command="UPDATE creditcard SET Procedures='"+string.Join(",",listProcCodes.Select(x => POut.String(x)))+"'"
				+" WHERE ("+DbHelper.Year("DateStop")+"<1880 OR DateStop>="+DbHelper.Curdate()+") "//Stop date has not past
				+" AND ExcludeProcSync=0"
				+" AND CCSource NOT IN ("+(int)CreditCardSource.XWeb+","+(int)CreditCardSource.XWebPortalLogin+") ";//Not created from the Patient Portal
			Db.NonQ(command);
		}

		///<summary>Returns list of credit cards that are ready for a recurring charge.  Filters by ClinicNums in list if provided.  List of ClinicNums
		///should contain all clinics the current user is authorized to access.  Further filtering by selected clinics is done at the UI level to save
		///DB calls.</summary>
		public static DataTable GetRecurringChargeList(List<long> listClinicNums) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),listClinicNums);
			}
			DataTable table=new DataTable();
			//This query will return patient information and the latest recurring payment whom:
			//	-have recurring charges setup and today's date falls within the start and stop range.
			//NOTE: Query will return patients with or without payments regardless of when that payment occurred, filtering is done below.
			string command="SELECT CreditCardNum,PatNum,PatName,FamBalTotal,PayPlanDue,"+POut.Date(DateTime.MinValue)+" AS LatestPayment,DateStart,Address,"
				+"AddressPat,Zip,ZipPat,XChargeToken,CCNumberMasked,CCExpiration,ChargeAmt,PayPlanNum,ProvNum,PayPlanPatnum,ClinicNum,Procedures,BillingCycleDay,Guarantor,"
				+"PayConnectToken,PayConnectTokenExp,PaySimpleToken "
				+"FROM (";
			#region Payments
			//The PayOrder is used to differentiate rows attached to payment plans
			command+="(SELECT 1 AS PayOrder,cc.CreditCardNum,cc.PatNum,"+DbHelper.Concat("pat.LName","', '","pat.FName")+" PatName,"
				+"guar.LName GuarLName,guar.FName GuarFName,guar.BalTotal-guar.InsEst FamBalTotal,0 AS PayPlanDue,"
				+"cc.DateStart,cc.Address,pat.Address AddressPat,cc.Zip,pat.Zip ZipPat,cc.XChargeToken,cc.CCNumberMasked,cc.CCExpiration,cc.ChargeAmt,"
				+"cc.PayPlanNum,cc.DateStop,0 ProvNum,0 PayPlanPatNum,pat.ClinicNum,cc.Procedures,pat.BillingCycleDay,pat.Guarantor,cc.PayConnectToken,cc.PayConnectTokenExp,cc.PaySimpleToken "
				+"FROM creditcard cc "
				+"INNER JOIN patient pat ON pat.PatNum=cc.PatNum "
				+"INNER JOIN patient guar ON guar.PatNum=pat.Guarantor "
				+"WHERE cc.PayPlanNum=0 "//Keeps card from showing up in case they have a balance AND is setup for payment plan. 
				+"AND CCSource NOT IN ("+(int)CreditCardSource.XWeb+","+(int)CreditCardSource.XWebPortalLogin+") ";//Not created from the Patient Portal
			if(listClinicNums!=null && listClinicNums.Count>0) {
				command+="AND pat.ClinicNum IN ("+string.Join(",",listClinicNums.Select(x => POut.Long(x)))+") ";
			}
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command+="GROUP BY cc.CreditCardNum) ";
			}
			else {//Oracle
				command+="GROUP BY cc.CreditCardNum,cc.PatNum,"+DbHelper.Concat("pat.LName","', '","pat.FName")+",PatName,guar.BalTotal-guar.InsEst,"
					+"cc.Address,pat.Address,cc.Zip,pat.Zip,cc.XChargeToken,cc.CCNumberMasked,cc.CCExpiration,cc.ChargeAmt,cc.PayPlanNum,cc.DateStop,PayPlanPatNum,"
					+"pat.ClinicNum,cc.Procedures,pat.BillingCycleDay,pat.Guarantor,cc.PayConnectToken,cc.PayConnectTokenExp,cc.PaySimpleToken) ";
			}
			#endregion
			command+="UNION ALL ";
			#region Payment Plans
			command+="(SELECT 2 AS PayOrder,cc.CreditCardNum,cc.PatNum,"+DbHelper.Concat("pat.LName","', '","pat.FName")+" PatName,"
				+"guar.LName GuarLName,guar.FName GuarFName,guar.BalTotal-guar.InsEst FamBalTotal,"
				+"ROUND(COALESCE(ppc.pastCharges,0)-COALESCE(SUM(ps.SplitAmt),0),2) PayPlanDueCalc,"//payplancharges-paysplits attached to pp is PayPlanDueCalc
				+"cc.DateStart,cc.Address,pat.Address AddressPat,cc.Zip,pat.Zip ZipPat,cc.XChargeToken,cc.CCNumberMasked,cc.CCExpiration,cc.ChargeAmt,"
				+"cc.PayPlanNum,cc.DateStop,COALESCE(ppc.maxProvNum,0) ProvNum,COALESCE(ppc.maxPatNum,0) PayPlanPatNum,COALESCE(ppc.maxClinicNum,0) ClinicNum,cc.Procedures,"
				+"pat.BillingCycleDay,pat.Guarantor,cc.PayConnectToken,cc.PayConnectTokenExp,cc.PaySimpleToken "
				+"FROM creditcard cc "
				+"INNER JOIN patient pat ON pat.PatNum=cc.PatNum "
				+"INNER JOIN patient guar ON guar.PatNum=pat.Guarantor "
				+"LEFT JOIN paysplit ps ON ps.PayPlanNum=cc.PayPlanNum AND ps.PayPlanNum<>0 "
				+"LEFT JOIN ("
					+"SELECT PayPlanNum,MAX(ProvNum) maxProvNum,MAX(PatNum) maxPatNum,MAX(ClinicNum) maxClinicNum,"
					+"SUM(CASE WHEN ChargeType="+POut.Int((int)PayPlanChargeType.Debit)+" "
						+"AND ChargeDate <= "+DbHelper.Curdate()+" THEN Principal+Interest ELSE 0 END) pastCharges "
					+"FROM payplancharge "
					+"GROUP BY PayPlanNum"
				+") ppc ON ppc.PayPlanNum=cc.PayPlanNum "
				+"WHERE cc.PayPlanNum>0 "
				+"AND CCSource NOT IN ("+(int)CreditCardSource.XWeb+","+(int)CreditCardSource.XWebPortalLogin+") ";//Not created from the Patient Portal
			if(listClinicNums!=null && listClinicNums.Count>0) {
				command+="AND ppc.maxClinicNum IN ("+string.Join(",",listClinicNums.Select(x => POut.Long(x)))+") ";
			}
			if(DataConnection.DBtype==DatabaseType.MySql) {
				command+="GROUP BY cc.CreditCardNum ";
			}
			else {//Oracle
				command+="GROUP BY cc.CreditCardNum,cc.PatNum,"+DbHelper.Concat("pat.LName","', '","pat.FName")+",PatName,guar.BalTotal-guar.InsEst,"
					+"cc.Address,pat.Address,cc.Zip,pat.Zip,cc.XChargeToken,cc.CCNumberMasked,cc.CCExpiration,cc.ChargeAmt,cc.PayPlanNum,cc.DateStop,PayPlanPatNum,"
					+"ClinicNum,cc.Procedues,pat.BillingCycleDay,pat.Guarantor,cc.PayConnectToken,cc.PayConnectTokenExp,cc.PaySimpleToken ";
			}
			command+="HAVING PayPlanDueCalc>0)";//don't show cc's attached to payplans when the payplan has nothing due
			#endregion
			//Now we have all the results for payments and payment plans, so do an obvious filter. A more thorough filter happens later.
			command+=") due "
				+"WHERE DateStart<="+DbHelper.Curdate()+" AND "+DbHelper.Year("DateStart")+">1880 "
				+"AND (DateStop>="+DbHelper.Curdate()+" OR "+DbHelper.Year("DateStop")+"<1880) "
				+"ORDER BY GuarLName,GuarFName,PatName,PayOrder DESC";
			table=Db.GetTable(command);
			//Query for latest payments seperately because this takes a very long time when run as a sub select
			if(table.Rows.Count<1) {
				return table;
			}
			command="SELECT PatNum,MAX(CASE WHEN "+DbHelper.Year("RecurringChargeDate")+" > 1880 "
				+"THEN RecurringChargeDate ELSE PayDate END) RecurringChargeDate "
				+"FROM payment "
				+"WHERE IsRecurringCC=1 AND PayAmt > 0 "
				+"AND PatNum IN ("+string.Join(",",table.Select().Select(x => POut.String(x["PatNum"].ToString())))+") "//table has at least 1 row
				+"GROUP BY PatNum";
			//dictionary is key=PatNum, value=LatestPayment which will be the lastest date a recurring charge payment was made
			Dictionary<long,DateTime> dictPatNumDate=Db.GetTable(command).Select()
				.ToDictionary(x => PIn.Long(x["PatNum"].ToString()),x =>	PIn.Date(x["RecurringChargeDate"].ToString()));
			table.Select().Where(x => dictPatNumDate.ContainsKey(PIn.Long(x["PatNum"].ToString()))).ToList()
				.ForEach(x => x["LatestPayment"]=dictPatNumDate[PIn.Long(x["PatNum"].ToString())]);
			FilterRecurringChargeList(table);
			return table;
		}
		
		/// <summary>Adds up the total fees for the procedures passed in that have been completed since the last billing day.</summary>
		public static double TotalRecurringCharges(long patNum,string procedures,int billingDay) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetDouble(MethodBase.GetCurrentMethod(),patNum,procedures,billingDay);
			}
			//Find the beginning of the current billing cycle, use that date to total charges between now and then for this cycle only.
			//Include that date only when we are not on the first day of the current billing cycle.
			DateTime startBillingCycle;
			if(DateTime.Today.Day>billingDay) {//if today is 7/13/2015 and billingDay is 26, startBillingCycle will be 6/26/2015
				startBillingCycle=new DateTime(DateTime.Today.Year,DateTime.Today.Month,billingDay);
			}
			else {
				//DateTime.Today.AddMonths handles the number of days in the month and leap years
				//Examples: if today was 12/31/2015, AddMonths(-1) would yield 11/30/2015; if today was 3/31/2016, AddMonths(-1) would yield 2/29/2016
				startBillingCycle=DateTime.Today.AddMonths(-1);
				if(billingDay<=DateTime.DaysInMonth(startBillingCycle.Year,startBillingCycle.Month)) {
					//This corrects the issue of a billing cycle day after today but this month doesn't have enough days when last month does
					//Example: if today was 11/30/2015 and the pat's billing cycle day was the 31st, startBillingCycle=Today.AddMonths(-1) would be 10/30/2015.
					//But this pat's billing cycle day is the 31st and the December has 31 days.  This adjusts the start of the billing cycle to 10/31/2015.
					//Example 2: if today was 2/29/2016 (leap year) and the pat's billing cycle day was the 30th, startBillingCycle should be 1/30/2016.
					//Today.AddMonths(-1) would be 1/29/2016, so this adjusts startBillingCycle to 1/30/2016.
					startBillingCycle=new DateTime(startBillingCycle.Year,startBillingCycle.Month,billingDay);
				}
			}
			string procStr="'"+POut.String(procedures).Replace(",","','")+"'";
			string command="SELECT SUM(pl.ProcFee) "
				+"FROM procedurelog pl "
				+"INNER JOIN procedurecode pc ON pl.CodeNum=pc.CodeNum "
				+"WHERE pl.ProcStatus=2 "
				+"AND pc.ProcCode IN ("+procStr+") "
				+"AND pl.PatNum="+POut.Long(patNum)+" "
				+"AND pl.ProcDate<="+DbHelper.Curdate()+" ";
			//If today is the billingDay or today is the last day of the current month and the billingDay is greater than today
			//i.e. billingDay=31 and today is the 30th which is the last day of the current month, only count procs with date after the 31st of last month
			if(billingDay==DateTime.Today.Day
				|| (billingDay>DateTime.Today.Day
				&& DateTime.Today.Day==DateTime.DaysInMonth(DateTime.Today.Year,DateTime.Today.Month)))
			{
				command+="AND pl.ProcDate>"+POut.Date(startBillingCycle);
			}
			else {
				command+="AND pl.ProcDate>="+POut.Date(startBillingCycle);
			}
			return PIn.Double(Db.GetScalar(command));
		}

		/// <summary>Returns true if the procedure passed in is linked to any other active card on the patient's account.</summary>
		public static bool ProcLinkedToCard(long patNum,string procCode,long cardNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),patNum,procCode,cardNum);
			}
			string command="SELECT CreditCardNum,Procedures "
				+"FROM creditcard "
				+"WHERE PatNum="+POut.Long(patNum)+" "
				+"AND DateStart<="+DbHelper.Curdate()+" AND "+DbHelper.Year("DateStart")+">1880 "
				+"AND (DateStop>="+DbHelper.Curdate()+" OR "+DbHelper.Year("DateStop")+"<1880) "
				+"AND CreditCardNum!="+POut.Long(cardNum);
			DataTable table=Db.GetTable(command);
			return table.Rows.OfType<DataRow>().SelectMany(x => x["Procedures"].ToString().Split(',')).Any(x => x==procCode);
		}

		///<summary>Table must include columns labeled LatestPayment and DateStart.</summary>
		public static void FilterRecurringChargeList(DataTable table) {
			DateTime curDate=MiscData.GetNowDateTime();
			//Loop through table and remove patients that do not need to be charged yet.
			for(int i=0;i<table.Rows.Count;i++) {
				DateTime latestPayment=PIn.Date(table.Rows[i]["LatestPayment"].ToString());
				DateTime dateStart=PIn.Date(table.Rows[i]["DateStart"].ToString());
				if(curDate>latestPayment.AddDays(31)) {//if it's been more than a month since they made any sort of payment
					//if we reduce the days below 31, then slighly more people will be charged, especially from Feb to March.  31 eliminates those false positives.
					continue;//charge them
				}
				//Not enough days in the current month so show on the last day of the month
				//Example: DateStart=8/31/2010 and the current month is February 2011 which does not have 31 days.
				//So the patient needs to show in list if current day is the 28th (or last day of the month).
				int daysInMonth=DateTime.DaysInMonth(curDate.Year,curDate.Month);
				if(daysInMonth<=dateStart.Day && daysInMonth==curDate.Day && curDate.Date!=latestPayment.Date) {//if their recurring charge would fall on an invalid day of the month, and this is that last day of the month
					continue;//we want them to show because the charge should go in on this date.
				}
				if(curDate.Day>=dateStart.Day) {//If the recurring charge date was earlier in this month, then the recurring charge will go in for this month.
					if(curDate.Month>latestPayment.Month || curDate.Year>latestPayment.Year) {//if the latest payment was last month (or earlier).  The year check catches December
						continue;//No payments were made this month, so charge.
					}
				}
				else {//Else, current date is before the recurring date in the current month, so the recurring charge will be going in for last month
					//Check if payment didn't happen last month.
					if(curDate.AddMonths(-1).Date>latestPayment.Date//the latest recurring charge payment for this card was before one month ago
						&& curDate.AddMonths(-1).Month!=latestPayment.Month)//the latest recurring charge payment for this card did not happen during last month
						//&& curDate.Date!=latestPayment.Date)//no longer necessary since latest payment was before one month ago
					{
						//Charge did not happen last month so the patient needs to show up in list.
						//Example: Last month had a recurring charge set at the end of the month that fell on a weekend.
						//Today is the next month and still before the recurring charge date. 
						//This will allow the charge for the previous month to happen if the 30 day check didn't catch it above.
						continue;
					}
				}
				//Patient doesn't need to be charged yet so remove from the table.
				table.Rows.RemoveAt(i);
				i--;
			}
		}

		///<summary>Returns number of times token is in use.  Token was duplicated once and caused the wrong card to be charged.</summary>
		public static int GetTokenCount(string token,List<CreditCardSource> listSources) {
			//No need to check RemotingRole; no call to db.
			return GetCardsByToken(token,listSources).Count;
		}

		public static List<CreditCard> GetCardsByToken(string token,List<CreditCardSource> listSources) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<CreditCard>>(MethodBase.GetCurrentMethod(),token,listSources);
			}
			if(listSources.Count==0) {
				return new List<CreditCard>();
			}
			string command="SELECT * FROM creditcard WHERE CCSource IN ("+string.Join(",",listSources.Select(x=>POut.Int((int)x)))+") AND (0 ";
			if(listSources.Contains(CreditCardSource.PayConnect) || listSources.Contains(CreditCardSource.XServerPayConnect)) {
				command+="OR PayConnectToken='"+POut.String(token)+"' ";
			}
			if(listSources.Contains(CreditCardSource.XServer)
				|| listSources.Contains(CreditCardSource.XWeb)
				|| listSources.Contains(CreditCardSource.XServerPayConnect)
				|| listSources.Contains(CreditCardSource.XWebPortalLogin))
			{
				command+="OR XChargeToken='"+POut.String(token)+"' ";
			}
			if(listSources.Contains(CreditCardSource.PaySimple)) {
				command+="OR PaySimpleToken='"+POut.String(token)+"' ";
			}
			command+=")";
			return Crud.CreditCardCrud.SelectMany(command);
		}

		///<summary>Returns number of times token is in use.  Token was duplicated once and caused the wrong card to be charged.</summary>
		public static int GetTokenCount(string token,CreditCardSource ccSource) {
			//No need to check RemotingRole; no call to db.
			return GetTokenCount(token,new List<CreditCardSource>() { ccSource });
		}

		///<summary>Checks if token already exists in db.</summary>
		public static bool TokenExists(string token,CreditCardSource ccSource) {
			//No need to check RemotingRole; no call to db.
			return GetTokenCount(token,ccSource) >= 1;
		}		

		public static List<CreditCard> GetCardsWithTokenBySource(List<CreditCardSource> listSources) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<CreditCard>>(MethodBase.GetCurrentMethod(),listSources);
			}
			if(listSources.Count==0) {
				return new List<CreditCard>();
			}
			string command="SELECT * FROM creditcard WHERE CCSource IN ("+string.Join(",",listSources.Select(x=>POut.Int((int)x)))+") AND (0 ";
			if(listSources.Contains(CreditCardSource.PayConnect) || listSources.Contains(CreditCardSource.XServerPayConnect)) {
				command+="OR PayConnectToken!='' ";
			}
			if(listSources.Contains(CreditCardSource.XServer)
				|| listSources.Contains(CreditCardSource.XWeb)
				|| listSources.Contains(CreditCardSource.XServerPayConnect)
				|| listSources.Contains(CreditCardSource.XWebPortalLogin))
			{
				command+="OR XChargeToken!='' ";
			}
			if(listSources.Contains(CreditCardSource.PaySimple)) {
				command+="OR PaySimpleToken!='' ";
			}
			command+=")";
			return Crud.CreditCardCrud.SelectMany(command);
		}

		///<summary>Gets every credit card in the db with an X-Charge token that was created from the specified source.</summary>
		public static List<CreditCard> GetCardsWithXChargeTokens(CreditCardSource ccSource=CreditCardSource.XServer) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<CreditCard>>(MethodBase.GetCurrentMethod(),ccSource);
			}
			string command="SELECT * FROM creditcard WHERE XChargeToken!=\"\" "
				+"AND CCSource="+POut.Int((int)ccSource);
			return Crud.CreditCardCrud.SelectMany(command);
		}

		///<summary>Gets every credit card in the db with a PayConnect token.</summary>
		public static List<CreditCard> GetCardsWithPayConnectTokens() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<CreditCard>>(MethodBase.GetCurrentMethod());
			}
			string command = "SELECT * FROM creditcard WHERE PayConnectToken!=\"\"";
			return Crud.CreditCardCrud.SelectMany(command);
		}
	}
}