using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Globalization;
using Newtonsoft.Json;
using CodeBase;

namespace OpenDentBusiness{
	///<summary></summary>
	public class SmsFromMobiles{
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

		/*
		Only pull out the methods below as you need them.  Otherwise, leave them commented out.

		///<summary></summary>
		public static List<SmsFromMobile> Refresh(long patNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SmsFromMobile>>(MethodBase.GetCurrentMethod(),patNum);
			}
			string command="SELECT * FROM smsfrommobile WHERE PatNum = "+POut.Long(patNum);
			return Crud.SmsFromMobileCrud.SelectMany(command);
		}

		///<summary>Gets one SmsFromMobile from the db.</summary>
		public static SmsFromMobile GetOne(long smsFromMobileNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<SmsFromMobile>(MethodBase.GetCurrentMethod(),smsFromMobileNum);
			}
			return Crud.SmsFromMobileCrud.SelectOne(smsFromMobileNum);
		}
		


		///<summary></summary>
		public static void Update(SmsFromMobile smsFromMobile){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsFromMobile);
				return;
			}
			Crud.SmsFromMobileCrud.Update(smsFromMobile);
		}

		///<summary></summary>
		public static void Delete(long smsFromMobileNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),smsFromMobileNum);
				return;
			}
			string command= "DELETE FROM smsfrommobile WHERE SmsFromMobileNum = "+POut.Long(smsFromMobileNum);
			Db.NonQ(command);
		}
		*/

		///<summary>Structured data to be stored as json List in Signalod.MsgValue for InvalidType.SmsTextMsgReceivedUnreadCount.</summary>
		public class SmsNotification {
			[JsonProperty(PropertyName = "A")]
			public long ClinicNum { get; set; }
			[JsonProperty(PropertyName="B")]
			public int Count { get; set; }

			public static string GetJsonFromList(List<SmsNotification> listNotifications) {
				return JsonConvert.SerializeObject(listNotifications);
			}

			public static List<SmsNotification> GetListFromJson(string json) {
				List<SmsNotification> ret=null;
				ODException.SwallowAnyException(() => ret=JsonConvert.DeserializeObject<List<SmsNotification>>(json));
				return ret;
			}
		}

		///<summary>A list item per each clinic indicating the number of messages which have not yet been read for that clinic (practice). 
		///Inserts a new InvalidType.SmsTextMsgReceivedUnreadCount signal.</summary>
		public static List<SmsNotification> UpdateSmsNotification() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SmsNotification>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT ClinicNum,COUNT(*) AS CountUnread FROM smsfrommobile WHERE SmsStatus=0 GROUP BY ClinicNum";
			List<SmsNotification> ret=Db.GetTable(command).AsEnumerable()
				.Select(x => new SmsNotification() {
					ClinicNum=PIn.Long(x["ClinicNum"].ToString()),
					Count=PIn.Int(x["CountUnread"].ToString()),
				}).ToList();
			//Insert as structured data signal so all workstations won't have to query the db to get the counts. They will get it directly from Signalod.MsgValue.
			Signalods.InsertSmsNotification(SmsNotification.GetJsonFromList(ret));
			return ret;
		}

		///<summary>Call ProcessInboundSms instead.</summary>
		private static long Insert(SmsFromMobile smsFromMobile) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				smsFromMobile.SmsFromMobileNum=Meth.GetLong(MethodBase.GetCurrentMethod(),smsFromMobile);
				return smsFromMobile.SmsFromMobileNum;
			}
			return Crud.SmsFromMobileCrud.Insert(smsFromMobile);
		}

		///<summary><para>Gets all SMS incoming messages for the specified filters.</para>
		///<para>If dateStart is 01/01/0001 then no start date will be used.</para>
		///<para>If dateEnd is 01/01/0001 then no end date will be used.</para>
		///<para>If listClinicNums is empty then will not filter by clinic.</para>
		///<para>If patNum is non-zero, then only the messages for the specified patient will be returned, otherwise messages for all patients will be returned.</para>
		///<para>If arrayStatuses is empty then messages with all statuses will be returned.</para></summary>
		public static List<SmsFromMobile> GetMessages(DateTime dateStart,DateTime dateEnd,List <long> listClinicNums,long patNum,bool isMessageThread,params SmsFromStatus[] arrayStatuses) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<SmsFromMobile>>(MethodBase.GetCurrentMethod(),dateStart,dateEnd,listClinicNums,patNum,isMessageThread,arrayStatuses);
			}
			List<SmsFromStatus> statusFilters=new List<SmsFromStatus>(arrayStatuses);
			List <string> listCommandFilters=new List<string>();
			if(dateStart>DateTime.MinValue) {
				listCommandFilters.Add(DbHelper.DtimeToDate("DateTimeReceived")+">="+POut.Date(dateStart));
			}
			if(dateEnd>DateTime.MinValue) {
				listCommandFilters.Add(DbHelper.DtimeToDate("DateTimeReceived")+"<="+POut.Date(dateEnd));
			}
			if(patNum==0) {
				//Only limit clinic if not searching for a particular PatNum.
				if(listClinicNums.Count>0) {
					listCommandFilters.Add("ClinicNum IN ("+string.Join(",",listClinicNums.Select(x => POut.Long(x)))+")");
				}
			}
			else {
				listCommandFilters.Add("PatNum="+POut.Long(patNum));
			}
			if(!isMessageThread) { //Always show unread in the grid.
				statusFilters.Add(SmsFromStatus.ReceivedUnread);
			}
			if(statusFilters.Count>0) {
				listCommandFilters.Add("SmsStatus IN ("+string.Join(",",statusFilters.GroupBy(x => x).Select(x => POut.Int((int)x.Key)))+")");
			}
			string command="SELECT * FROM smsfrommobile";
			if(listCommandFilters.Count>0) {
				command+=" WHERE "+string.Join(" AND ",listCommandFilters);
			}
			return Crud.SmsFromMobileCrud.SelectMany(command);
		}

		///<summary>Attempts to find exact match for patient. If found, creates commlog, associates Patnum, and inserts into DB.
		///Otherwise, it simply inserts SmsFromMobiles into the DB. ClinicNum should have already been set before calling this function.</summary>
		public static void ProcessInboundSms(List<SmsFromMobile> listMessages) {
			if(listMessages==null || listMessages.Count==0) {
				return;
			}
			List<SmsBlockPhone> listBlockedPhones=SmsBlockPhones.GetDeepCopy();
			for(int i=0;i<listMessages.Count;i++) {
				SmsFromMobile sms=listMessages[i];
				if(listBlockedPhones.Any(x => TelephoneNumbers.AreNumbersEqual(x.BlockWirelessNumber,sms.MobilePhoneNumber))) {
					continue;//The office has blocked this number.
				}
				sms.DateTimeReceived=DateTime.Now;
				SmsPhone smsPhone=SmsPhones.GetByPhone(sms.SmsPhoneNumber);
				string countryCode=CultureInfo.CurrentCulture.Name.Substring(CultureInfo.CurrentCulture.Name.Length-2);
				if(smsPhone!=null) {
					sms.ClinicNum=smsPhone.ClinicNum;
					countryCode=smsPhone.CountryCode;
				}
				//First try the clinic that belongs to this phone.
				List<long> listClinicNums=new List<long>();
				if(sms.ClinicNum!=0) {
					listClinicNums.Add(sms.ClinicNum);
				}
				List<long[]> listPatNums=FindPatNums(sms.MobilePhoneNumber,countryCode,listClinicNums);
				if(listPatNums.Count==0 && listClinicNums.Count>0) { //Could not find that patient in this clinic so try again for all clinics.
					listPatNums=FindPatNums(sms.MobilePhoneNumber,countryCode);
				}
				sms.MatchCount=listPatNums.Count;
				//Item1=PatNum; Item2=Guarantor
				if(listPatNums.Count==0 || listPatNums.Select(x => x[1]).Distinct().ToList().Count!=1){
					//We could not find definitive match, either 0 matches found, or more than one match found with different garantors
					Insert(sms);
					continue;
				}
				if(listPatNums.Count==1) {
					sms.PatNum=listPatNums[0][0];//PatNum
				}
				else {
					sms.PatNum=listPatNums[0][1];//GuarantorNum;  more than one match, but all have the same garantor.
				}
				Commlog comm=new Commlog() {
					CommDateTime=sms.DateTimeReceived,
					Mode_= CommItemMode.Text,
					Note=sms.MsgText,
					PatNum=sms.PatNum,
					CommType=Commlogs.GetTypeAuto(CommItemTypeAuto.TEXT),
					SentOrReceived= CommSentOrReceived.Received
				};
				sms.CommlogNum=Commlogs.Insert(comm);
				Insert(sms);
			}
			UpdateSmsNotification();
		}

		public static string GetSmsFromStatusDescript(SmsFromStatus smsFromStatus) {
			//No need to check RemotingRole; no call to db.
			if(smsFromStatus==SmsFromStatus.ReceivedUnread) {
				return "Unread";
			}
			else if(smsFromStatus==SmsFromStatus.ReceivedRead) {
				return "Read";
			}
			return "";
		}

		///<summary>Updates only the changed fields of the SMS text message (if any).</summary>
		public static bool Update(SmsFromMobile smsFromMobile,SmsFromMobile oldSmsFromMobile) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetBool(MethodBase.GetCurrentMethod(),smsFromMobile,oldSmsFromMobile);
			}
			return Crud.SmsFromMobileCrud.Update(smsFromMobile,oldSmsFromMobile);
		}

		///<summary>Used to link SmsFromMobiles to the patients that they came from. Returns list of patnum,garantorNum combos.</summary>
		public static List<long[]> FindPatNums(string phonePat,string countryCode,List<long> listClinicNums=null) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<long[]>>(MethodBase.GetCurrentMethod(),phonePat,countryCode,listClinicNums);
			}
			List<long[]> retVal=new List<long[]>();
			try {
				string phoneRegexp=ConvertPhoneToRegexp(phonePat,countryCode);
				//DO NOT POut THESE PHONE NUMBERS. They have been cleaned for use in this function by dirtyPhoneHelper.
				string command="SELECT PatNum, Guarantor FROM patient WHERE " 
					+"PatStatus NOT IN ("+POut.Int((int)PatientStatus.Archived)+","+POut.Int((int)PatientStatus.Deleted)+") "
					+"AND ("+DbHelper.Regexp("HmPhone",phoneRegexp)+" "
						+"OR "+DbHelper.Regexp("WkPhone",phoneRegexp)+" "
						+"OR "+DbHelper.Regexp("WirelessPhone",phoneRegexp)+")";
				if(listClinicNums!=null&&listClinicNums.Count>0) {
					command+=" AND ClinicNum IN("+string.Join(",",listClinicNums)+")";
				}
				DataTable table=Db.GetTable(command);
				foreach(DataRow row in table.Rows) {
					retVal.Add(new long[] { PIn.Long(row["PatNum"].ToString()),PIn.Long(row["Guarantor"].ToString()) });
				}
			}
			catch {	
				//should only happen if phone number is blank, if so, return empty list below.
			}
			return retVal;
		}

		///<summary>Expands a phone number into a string that can be used to ignore punctuation in a phone number.
		///Any string that passes through this function does not need to, and should not, go through POut.String()
		///Expands </summary>
		public static string ConvertPhoneToRegexp(string phoneRaw,string countryCode) {
			//Strip all non-numeric characters just in case.
			string retVal=new string(phoneRaw.Where(x => char.IsDigit(x)).ToArray());
			string prefix="";
			switch(countryCode.ToUpper()) {
				case "US":
				case "CA":
					//Number prefixed with a country and not prefixed with a country code should both be prefixed with a country code.
					//EG: Both of the following should yield the same 11-digit string... 80012345678, 180012345678 == 180012345678.
					if(retVal.Length==11) { //We have an 11-digit number coming in.
						//Prefix with * in order to make country code optional.
						prefix=retVal[0]+"*";
						//Remove the first char, which we just included in the prefix above.
						retVal=retVal.Substring(1);
					}
					break;
			}		
			if(String.IsNullOrEmpty(retVal)) {
				throw new Exception("Phone number cannot be blank.");
			}
			string wildcard=".*";
			//Add back the optional prefix from above and converto a RegEx.
			retVal=wildcard+prefix+String.Join("",retVal.Select(x => x+wildcard).ToList());			
			return retVal;
		}
	}
}