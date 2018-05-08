using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Linq;
using System.Globalization;
using CodeBase;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SmsToMobiles{
		///<summary>The amount that is charged per outgoing text. The actual charge may be higher if the message contains multiple pages.</summary>
		public const double CHARGE_PER_MSG=0.04;

		#region Get Methods
		#endregion

		#region Modification Methods

		#region Insert

		private static void InsertMany(List<SmsToMobile> listSmsToMobiles) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listSmsToMobiles);
				return;
			}
			Crud.SmsToMobileCrud.InsertMany(listSmsToMobiles);
		}

		#endregion

		#region Update
		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods
		#endregion

		//If this table type will exist as cached data, uncomment the CachePattern region below and edit.
		/*
		#region CachePattern

		private class SmsToMobileCache : CacheListAbs<SmsToMobile> {
			protected override List<SmsToMobile> GetCacheFromDb() {
				string command="SELECT * FROM SmsToMobile ORDER BY ItemOrder";
				return Crud.SmsToMobileCrud.SelectMany(command);
			}
			protected override List<SmsToMobile> TableToList(DataTable table) {
				return Crud.SmsToMobileCrud.TableToList(table);
			}
			protected override SmsToMobile Copy(SmsToMobile SmsToMobile) {
				return SmsToMobile.Clone();
			}
			protected override DataTable ListToTable(List<SmsToMobile> listSmsToMobiles) {
				return Crud.SmsToMobileCrud.ListToTable(listSmsToMobiles,"SmsToMobile");
			}
			protected override void FillCacheIfNeeded() {
				SmsToMobiles.GetTableFromCache(false);
			}
			protected override bool IsInListShort(SmsToMobile SmsToMobile) {
				return !SmsToMobile.IsHidden;
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static SmsToMobileCache _SmsToMobileCache=new SmsToMobileCache();

		///<summary>A list of all SmsToMobiles. Returns a deep copy.</summary>
		public static List<SmsToMobile> ListDeep {
			get {
				return _SmsToMobileCache.ListDeep;
			}
		}

		///<summary>A list of all visible SmsToMobiles. Returns a deep copy.</summary>
		public static List<SmsToMobile> ListShortDeep {
			get {
				return _SmsToMobileCache.ListShortDeep;
			}
		}

		///<summary>A list of all SmsToMobiles. Returns a shallow copy.</summary>
		public static List<SmsToMobile> ListShallow {
			get {
				return _SmsToMobileCache.ListShallow;
			}
		}

		///<summary>A list of all visible SmsToMobiles. Returns a shallow copy.</summary>
		public static List<SmsToMobile> ListShort {
			get {
				return _SmsToMobileCache.ListShallowShort;
			}
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_SmsToMobileCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_SmsToMobileCache.FillCacheFromTable(table);
				return table;
			}
			return _SmsToMobileCache.GetTableFromCache(doRefreshCache);
		}

		#endregion
		*/
		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<SmsToMobile> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SmsToMobile>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM smstomobile WHERE PatNum = "+POut.Long(patNum);
			return Crud.SmsToMobileCrud.SelectMany(command);
		}

		///<summary>Gets one SmsToMobile from the db.</summary>
		public static SmsToMobile GetOne(long smsToMobileNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<SmsToMobile>(MethodBase.GetCurrentMethod(),smsToMobileNum);
			}
			return Crud.SmsToMobileCrud.SelectOne(smsToMobileNum);
		}

		///<summary></summary>
		public static void Update(SmsToMobile smsToMobile){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsToMobile);
				return;
			}
			Crud.SmsToMobileCrud.Update(smsToMobile);
		}

		///<summary></summary>
		public static void Delete(long smsToMobileNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsToMobileNum);
				return;
			}
			string command= "DELETE FROM smstomobile WHERE smsToMobileNum = "+POut.Long(smsToMobileNum);
			Db.NonQ(command);
		}
		*/

		///<summary></summary>
		public static void Update(SmsToMobile smsToMobile) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsToMobile);
				return;
			}
			Crud.SmsToMobileCrud.Update(smsToMobile);
		}

		///<summary>Gets one SmsToMobile from the db.</summary>
		public static SmsToMobile GetMessageByGuid(string guid) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<SmsToMobile>(MethodBase.GetCurrentMethod(),guid);
			}
			string command="SELECT * FROM smstomobile WHERE GuidMessage='"+guid+"'";
			return Crud.SmsToMobileCrud.SelectOne(command);
		}

		///<summary></summary>
		public static long Insert(SmsToMobile smsToMobile) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				smsToMobile.SmsToMobileNum=Meth.GetLong(MethodBase.GetCurrentMethod(),smsToMobile);
				return smsToMobile.SmsToMobileNum;
			}
			return Crud.SmsToMobileCrud.Insert(smsToMobile);
		}

		///<summary>Gets all SMS incoming messages for the specified filters.  If dateStart is 01/01/0001 then no start date will be used.  If dateEnd is 01/01/0001 then no end date will be used.  If listClinicNums is empty then will return messages for all clinics.  If patNum is non-zero, then only the messages for the specified patient will be returned, otherwise messages for all patients will be returned.</summary>
		public static List<SmsToMobile> GetMessages(DateTime dateStart,DateTime dateEnd,List<long> listClinicNums,long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SmsToMobile>>(MethodBase.GetCurrentMethod(),dateStart,dateEnd,listClinicNums,patNum);
			}
			List<string> listCommandFilters=new List<string>();
			if(dateStart>DateTime.MinValue) {
				listCommandFilters.Add(DbHelper.DtimeToDate("DateTimeSent")+">="+POut.Date(dateStart));
			}
			if(dateEnd>DateTime.MinValue) {
				listCommandFilters.Add(DbHelper.DtimeToDate("DateTimeSent")+"<="+POut.Date(dateEnd));
			}
			if(patNum==0) {
				//Only limit clinic if not searching for a particular PatNum.
				if(listClinicNums.Count>0) {
					listCommandFilters.Add("ClinicNum IN ("+String.Join(",",listClinicNums.Select(x => POut.Long(x)))+")");
				}
			}
			else {
				listCommandFilters.Add("PatNum="+POut.Long(patNum));
			}
			
			string command="SELECT * FROM smstomobile";
			if(listCommandFilters.Count>0) {
				command+=" WHERE "+String.Join(" AND ",listCommandFilters);
			}
			return Crud.SmsToMobileCrud.SelectMany(command);
		}

		///<summary>Convert a phone number to internation format and remove all punctuation. Validates input number format. Throws exceptions.</summary>
		public static string ConvertPhoneToInternational(string phoneRaw,string countryCodeLocalMachine,string countryCodeSmsPhone) {
			//No need to check RemotingRole; no call to db.
			if(string.IsNullOrWhiteSpace(phoneRaw)) {
				throw new Exception("Input phone number must be set");
			}
			bool isUSorCanada=(countryCodeLocalMachine.ToUpper().In("US","CA") || countryCodeSmsPhone.ToUpper().In("US","CA"));
			//Remove non-numeric.
			string ret=new string(phoneRaw.Where(x => char.IsDigit(x)).ToArray());
			if(isUSorCanada) {
				if(!ret.StartsWith("1")) { //Add a "1" if US or Canada
					ret="1"+ret;
				}
				if(ret.Length!=11) {
					throw new Exception("Input phone number cannot be properly formatted for country code: "+countryCodeLocalMachine);
				}
			}
			return ret;
		}

		public static bool SendSmsSingle(SmsToMobile sms) {
			return SendSms(new List<SmsToMobile> { sms });
		}
		
		///<summary>Surround with Try/Catch.  Sent as time sensitive message.</summary>
		public static bool SendSmsSingle(long patNum,string wirelessPhone,string message,long clinicNum,SmsMessageSource smsMessageSource,bool makeCommLog=true,Userod user=null,bool canCheckBal=true) {
			//No need to check RemotingRole; no call to db.
			if(Plugins.HookMethod(null,"SmsToMobiles.SendSmsSingle_start",patNum,wirelessPhone,message,clinicNum)) {
				return true;
			}
			double balance=SmsPhones.GetClinicBalance(clinicNum);
			if(balance-CHARGE_PER_MSG<0 && canCheckBal) { //ODException.ErrorCode 1 will be processed specially by caller.
				throw new ODException("To send this message first increase spending limit for integrated texting from eServices Setup.",1);
			}
			string countryCodeLocal=CultureInfo.CurrentCulture.Name.Substring(CultureInfo.CurrentCulture.Name.Length-2);//Example "en-US"="US"
			string countryCodePhone=SmsPhones.GetForClinics(new List<long> { clinicNum }).FirstOrDefault()?.CountryCode??"";
			SmsToMobile smsToMobile=new SmsToMobile();
			smsToMobile.ClinicNum=clinicNum;
			smsToMobile.GuidMessage=Guid.NewGuid().ToString();
			smsToMobile.GuidBatch=smsToMobile.GuidMessage;
			smsToMobile.IsTimeSensitive=true;
			smsToMobile.MobilePhoneNumber=ConvertPhoneToInternational(wirelessPhone,countryCodeLocal,countryCodePhone);
			smsToMobile.PatNum=patNum;
			smsToMobile.MsgText=message;
			smsToMobile.MsgType=smsMessageSource;
			SmsToMobiles.SendSms(new List<SmsToMobile>() { smsToMobile });//Will throw if failed.
			HandleSentSms(new List<SmsToMobile>() { smsToMobile },makeCommLog,user);
			return true;
		}

		///<summary>Surround with try/catch. Returns true if all messages succeded, throws exception if it failed.</summary>
		public static bool SendSmsMany(List<SmsToMobile> listMessages,bool makeCommLog=true,Userod user=null,bool canCheckBal=true) {
			//No need to check RemotingRole; no call to db.
			if(listMessages==null || listMessages.Count==0) {
				return true;
			}
			if(canCheckBal) {
				foreach(long clinicNum in listMessages.Select(x => x.ClinicNum)) {
					double balance=SmsPhones.GetClinicBalance(clinicNum);
					if(balance-(CHARGE_PER_MSG*listMessages.Count(x => x.ClinicNum==clinicNum)) < 0) { 
						//ODException.ErrorCode 1 will be processed specially by caller.
						throw new ODException("To send these messages first increase spending limit for integrated texting from eServices Setup.",1);
					}
				}
			}
			SendSms(listMessages);
			HandleSentSms(listMessages,makeCommLog,user);
			return true;
		}

		///<summary>Inserts the SmsToMobile to the database and creates a commlog if necessary.</summary>
		private static void HandleSentSms(List<SmsToMobile> listSmsToMobiles,bool makeCommLog,Userod user) {
			//No need to check RemotingRole; no call to db.
			foreach(SmsToMobile smsToMobile in listSmsToMobiles) {
				smsToMobile.SmsStatus=SmsDeliveryStatus.Pending;
				smsToMobile.DateTimeSent=DateTime.Now;
				if(smsToMobile.PatNum!=0 && makeCommLog) {  //Patient specified and calling code won't make commlog, make it here.
					long userNum=0;
					if(user!=null) {
						userNum=user.UserNum;
					}
					Commlogs.Insert(new Commlog() {
						CommDateTime=smsToMobile.DateTimeSent,
						Mode_=CommItemMode.Text,
						Note="Text message sent: "+smsToMobile.MsgText,
						PatNum=smsToMobile.PatNum,
						CommType=Commlogs.GetTypeAuto(CommItemTypeAuto.TEXT),
						SentOrReceived=CommSentOrReceived.Sent,
						UserNum=userNum
					});
				}
			}
			InsertMany(listSmsToMobiles);
		}

		///<summary></summary>
		public static void Update(SmsToMobile smsToMobile,SmsToMobile oldSmsToMobile) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsToMobile);
				return;
			}
			Crud.SmsToMobileCrud.Update(smsToMobile,oldSmsToMobile);
		}

		///<summary>Surround with try/catch. Returns true if all messages succeded, throws exception if it failed. 
		///All Integrated Texting should use this method, CallFire texting does not use this method.</summary>
		public static bool SendSms(List<SmsToMobile> listMessages) {
			//No need to check RemotingRole; no call to db.
			if(Plugins.HookMethod(null,"SmsToMobiles.SendSms_start",listMessages)) {
				return true;
			}
			if(listMessages==null || listMessages.Count==0) {
				throw new Exception("No messages to send.");
			}
			StringBuilder strbuild=new StringBuilder();
			using(XmlWriter writer=XmlWriter.Create(strbuild,WebServiceMainHQProxy.CreateXmlWriterSettings(true))){
				writer.WriteStartElement("Payload");
				writer.WriteStartElement("ListSmsToMobile");
				System.Xml.Serialization.XmlSerializer xmlListSmsToMobileSerializer=new System.Xml.Serialization.XmlSerializer(typeof(List<SmsToMobile>));
				xmlListSmsToMobileSerializer.Serialize(writer,listMessages);
				writer.WriteEndElement(); //ListSmsToMobile	
				writer.WriteEndElement(); //Payload	
			}	
			string result = "";
			try {
				result=WebServiceMainHQProxy.GetWebServiceMainHQInstance()
					.SmsSend(WebServiceMainHQProxy.CreateWebServiceHQPayload(strbuild.ToString(),eServiceCode.IntegratedTexting));
			}
			catch(Exception ex) {
				ex.DoNothing();
				throw new Exception("Unable to send using web service.");
			}
			XmlDocument doc=new XmlDocument();
			doc.LoadXml(result);
			XmlNode node=doc.SelectSingleNode("//Error");
			if(node!=null) {
				throw new Exception(node.InnerText);
			}
			node=doc.SelectSingleNode("//Success");
			if(node!=null) {
				return true;
			}
			//Should never happen, we didn't get an explicit fail or success
			throw new Exception("Unknown error has occured.");
		}

	}
}