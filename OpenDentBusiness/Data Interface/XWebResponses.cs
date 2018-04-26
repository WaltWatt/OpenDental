using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class XWebResponses{
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

		///<summary>Gets one XWebResponse from the db.</summary>
		public static XWebResponse GetOne(long xWebResponseNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<XWebResponse>(MethodBase.GetCurrentMethod(),xWebResponseNum);
			}
			return Crud.XWebResponseCrud.SelectOne(xWebResponseNum);
		}

		///<summary>Gets the XWeb transactions for approved transactions. To get for all clinics, pass in a list of empty clinicNums.</summary>
		public static DataTable GetApprovedTransactions(List<long> listClinicNums,DateTime dateFrom,DateTime dateTo) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetTable(MethodBase.GetCurrentMethod(),listClinicNums,dateFrom,dateTo);
			}
			string command="SELECT "+DbHelper.Concat("patient.LName","', '","patient.FName")+" Patient,xwebresponse.DateTUpdate,xwebresponse.TransactionID,"
				+"xwebresponse.MaskedAcctNum,xwebresponse.ExpDate,xwebresponse.Amount,xwebresponse.PaymentNum,xwebresponse.TransactionStatus,"
				+"(CASE WHEN payment.PayNum IS NULL THEN 0 ELSE 1 END) doesPaymentExist,COALESCE(clinic.Abbr,'Unassigned') Clinic,xwebresponse.PatNum, "
				+"xwebresponse.XWebResponseNum,xwebresponse.Alias "
				+"FROM xwebresponse "
				+"INNER JOIN patient ON patient.PatNum=xwebresponse.PatNum "
				+"LEFT JOIN payment ON payment.PayNum=xwebresponse.PaymentNum "
				+"LEFT JOIN clinic ON clinic.ClinicNum=xwebresponse.ClinicNum "
				+"WHERE xwebresponse.TransactionStatus IN("
				+POut.Int((int)XWebTransactionStatus.DtgPaymentApproved)+","
				+POut.Int((int)XWebTransactionStatus.HpfCompletePaymentApproved)+","
				+POut.Int((int)XWebTransactionStatus.HpfCompletePaymentApprovedPartial)+","
				+POut.Int((int)XWebTransactionStatus.DtgPaymentReturned)+","
				+POut.Int((int)XWebTransactionStatus.DtgPaymentVoided)+") "
				+"AND xwebresponse.ResponseCode IN("
				+POut.Int((int)XWebResponseCodes.Approval)+","
				+POut.Int((int)XWebResponseCodes.PartialApproval)+") "
				+"AND xwebresponse.DateTUpdate BETWEEN "+POut.DateT(dateFrom)+" AND "+POut.DateT(dateTo.AddDays(1))+" ";
			if(listClinicNums.Count>0) {
				command+="AND xwebresponse.ClinicNum IN ("+string.Join(",",listClinicNums.Select(x => POut.Long(x)))+") ";
			}
			command+="ORDER BY xwebresponse.DateTUpdate,patient.LName,patient.FName ";
			return Db.GetTable(command);
		}

		///<summary>Gets the XWebResponse that is associated with this payNum. Returns null if the XWebResponse does not exist.</summary>
		public static XWebResponse GetOneByPaymentNum(long payNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<XWebResponse>(MethodBase.GetCurrentMethod(),payNum);
			}
			string command="SELECT * FROM xwebresponse WHERE PaymentNum="+POut.Long(payNum);
			return Crud.XWebResponseCrud.SelectOne(command);
		}

		///<summary>Gets all XWebResponses where TransactionStatus==XWebTransactionStatus.HpfPending from the db.</summary>
		public static List<XWebResponse> GetPendingHPFs() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<XWebResponse>>(MethodBase.GetCurrentMethod());
			}
			return Crud.XWebResponseCrud.SelectMany("SELECT * FROM xwebresponse "
				+"WHERE TransactionStatus = "+POut.Int((int)XWebTransactionStatus.HpfPending)+" "
				+"AND (TransactionType = '"+POut.String(XWebTransactionType.AliasCreateTransaction.ToString())+"' OR TransactionType = '"+POut.String(XWebTransactionType.CreditSaleTransaction.ToString())+"')");
		}
		
		///<summary></summary>
		public static long Insert(XWebResponse xWebResponse) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				xWebResponse.XWebResponseNum=Meth.GetLong(MethodBase.GetCurrentMethod(),xWebResponse);
				return xWebResponse.XWebResponseNum;
			}
			return Crud.XWebResponseCrud.Insert(xWebResponse);
		}

		///<summary></summary>
		public static void Update(XWebResponse xWebResponse) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),xWebResponse);
				return;
			}
			Crud.XWebResponseCrud.Update(xWebResponse);
		}

		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern

		private class XWebResponseCache : CacheListAbs<XWebResponse> {
			protected override List<XWebResponse> GetCacheFromDb() {
				string command="SELECT * FROM XWebResponse ORDER BY ItemOrder";
				return Crud.XWebResponseCrud.SelectMany(command);
			}
			protected override List<XWebResponse> TableToList(DataTable table) {
				return Crud.XWebResponseCrud.TableToList(table);
			}
			protected override XWebResponse Copy(XWebResponse XWebResponse) {
				return XWebResponse.Clone();
			}
			protected override DataTable ListToTable(List<XWebResponse> listXWebResponses) {
				return Crud.XWebResponseCrud.ListToTable(listXWebResponses,"XWebResponse");
			}
			protected override void FillCacheIfNeeded() {
				XWebResponses.GetTableFromCache(false);
			}
			protected override bool IsInListShort(XWebResponse XWebResponse) {
				return !XWebResponse.IsHidden;
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static XWebResponseCache _XWebResponseCache=new XWebResponseCache();

		///<summary>A list of all XWebResponses. Returns a deep copy.</summary>
		public static List<XWebResponse> ListDeep {
			get {
				return _XWebResponseCache.ListDeep;
			}
		}

		///<summary>A list of all visible XWebResponses. Returns a deep copy.</summary>
		public static List<XWebResponse> ListShortDeep {
			get {
				return _XWebResponseCache.ListShortDeep;
			}
		}

		///<summary>A list of all XWebResponses. Returns a shallow copy.</summary>
		public static List<XWebResponse> ListShallow {
			get {
				return _XWebResponseCache.ListShallow;
			}
		}

		///<summary>A list of all visible XWebResponses. Returns a shallow copy.</summary>
		public static List<XWebResponse> ListShort {
			get {
				return _XWebResponseCache.ListShallowShort;
			}
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_XWebResponseCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_XWebResponseCache.FillCacheFromTable(table);
				return table;
			}
			return _XWebResponseCache.GetTableFromCache(doRefreshCache);
		}

		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<XWebResponse> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<XWebResponse>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM xwebresponse WHERE PatNum = "+POut.Long(patNum);
			return Crud.XWebResponseCrud.SelectMany(command);
		}

		///<summary></summary>
		public static void Delete(long xWebResponseNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),xWebResponseNum);
				return;
			}
			Crud.XWebResponseCrud.Delete(xWebResponseNum);
		}

		

		
		*/



	}
}