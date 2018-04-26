using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness{
	///<summary></summary>
	public class EmailAddresses{
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

		#region CachePattern

		private class EmailAddressCache : CacheListAbs<EmailAddress> {
			protected override List<EmailAddress> GetCacheFromDb() {
				string command="SELECT * FROM emailaddress WHERE UserNum = 0 ORDER BY EmailUsername";
				return Crud.EmailAddressCrud.SelectMany(command);
			}
			protected override List<EmailAddress> TableToList(DataTable table) {
				return Crud.EmailAddressCrud.TableToList(table);
			}
			protected override EmailAddress Copy(EmailAddress emailAddress) {
				return emailAddress.Clone();
			}
			protected override DataTable ListToTable(List<EmailAddress> listEmailAddresses) {
				return Crud.EmailAddressCrud.ListToTable(listEmailAddresses,"EmailAddress");
			}
			protected override void FillCacheIfNeeded() {
				EmailAddresses.GetTableFromCache(false);
			}
		}
		
		///<summary>The object that accesses the cache in a thread-safe manner.</summary>
		private static EmailAddressCache _emailAddressCache=new EmailAddressCache();

		public static List<EmailAddress> GetDeepCopy(bool isShort=false) {
			return _emailAddressCache.GetDeepCopy(isShort);
		}

		public static EmailAddress GetFirstOrDefault(Func<EmailAddress,bool> match,bool isShort=false) {
			return _emailAddressCache.GetFirstOrDefault(match,isShort);
		}

		public static List<EmailAddress> GetWhere(Predicate<EmailAddress> match,bool isShort=false) {
			return _emailAddressCache.GetWhere(match,isShort);
		}

		///<summary>Refreshes the cache and returns it as a DataTable. This will refresh the ClientWeb's cache and the ServerWeb's cache.</summary>
		public static DataTable RefreshCache() {
			return GetTableFromCache(true);
		}

		///<summary>Fills the local cache with the passed in DataTable.</summary>
		public static void FillCacheFromTable(DataTable table) {
			_emailAddressCache.FillCacheFromTable(table);
		}

		///<summary>Always refreshes the ClientWeb's cache.</summary>
		public static DataTable GetTableFromCache(bool doRefreshCache) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				DataTable table=Meth.GetTable(MethodBase.GetCurrentMethod(),doRefreshCache);
				_emailAddressCache.FillCacheFromTable(table);
				return table;
			}
			return _emailAddressCache.GetTableFromCache(doRefreshCache);
		}

		#endregion

		///<summary>Gets the default email address for the clinic/practice. Takes a clinic num. 
		///If clinic num is 0 or there is no default for that clinic, it will get practice default. 
		///May return a new blank object.</summary>
		public static EmailAddress GetByClinic(long clinicNum,bool doAllowNullReturn=false) {
			//No need to check RemotingRole; no call to db.
			EmailAddress emailAddress=null;
			Clinic clinic=Clinics.GetClinic(clinicNum);
			if(PrefC.GetBool(PrefName.EasyNoClinics) || clinic==null) {//No clinic, get practice default
				emailAddress=GetOne(PrefC.GetLong(PrefName.EmailDefaultAddressNum));
			}
			else {
				emailAddress=GetOne(clinic.EmailAddressNum);
				if(emailAddress==null) {//clinic.EmailAddressNum 0. Use default.
					emailAddress=GetOne(PrefC.GetLong(PrefName.EmailDefaultAddressNum));
				}
			}
			if(emailAddress==null) {
				emailAddress=GetFirstOrDefault(x => true);//user didn't set a default, so just pick the first email in the list.
				if(emailAddress==null && !doAllowNullReturn) {//If there are no email addresses AT ALL!!
					emailAddress=new EmailAddress();//To avoid null checks.
					emailAddress.EmailPassword="";
					emailAddress.EmailUsername="";
					emailAddress.Pop3ServerIncoming="";
					emailAddress.SenderAddress="";
					emailAddress.SMTPserver="";
				}
			}
			return emailAddress;
		}

		///<summary>Executes a query to the database to get the email address associated to the passed-in user.  
		///Does not use the cache.  Returns null if no email address in the database matches the passed-in user.</summary>
		public static EmailAddress GetForUser(long userNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<EmailAddress>(MethodBase.GetCurrentMethod(),userNum);
			}
			string command="SELECT * FROM emailaddress WHERE emailaddress.UserNum = "+userNum;
			return Crud.EmailAddressCrud.SelectOne(command);
		}

		///<summary>Gets the default email address for new outgoing emails.
		///Will attempt to get the current user's email address first. 
		///If it can't find one, will return the clinic/practice default.
		///Can return a new blank email address if no email addresses are defined for the clinic/practice.</summary>
		public static EmailAddress GetNewEmailDefault(long userNum,long clinicNum) {
			//No need to check RemotingRole; no call to db.
			return GetForUser(userNum)??GetByClinic(clinicNum);
		}

		///<summary>Gets one EmailAddress from the cached listt.  Might be null.</summary>
		public static EmailAddress GetOne(long emailAddressNum) {
			//No need to check RemotingRole; no call to db.
			return GetFirstOrDefault(x => x.EmailAddressNum==emailAddressNum);
		}

		///<summary>Returns true if the passed-in email username already exists in the cached list of non-user email addresses.</summary>
		public static bool AddressExists(string emailUserName,long skipEmailAddressNum=0) {
			//No need to check RemotingRole; no call to db.
			List<EmailAddress> listEmailAddresses=GetWhere(x => x.EmailAddressNum!=skipEmailAddressNum
				&& x.EmailUsername.Trim().ToLower()==emailUserName.Trim().ToLower());
			return (listEmailAddresses.Count > 0);
		}

		///<summary>Gets all email addresses, including those email addresses which are not in the cache.</summary>
		public static List<EmailAddress> GetAll() {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<EmailAddress>>(MethodBase.GetCurrentMethod());
			}
			string command="SELECT * FROM emailaddress";
			return Crud.EmailAddressCrud.SelectMany(command);
		}

		///<summary>Checks to make sure at least one non-user email address has a valid (not blank) SMTP server.</summary>
		public static bool ExistsValidEmail() {
			//No need to check RemotingRole; no call to db.
			EmailAddress emailAddress=GetFirstOrDefault(x => x.SMTPserver!="");
			return (emailAddress!=null);
		}

		///<summary></summary>
		public static long Insert(EmailAddress emailAddress) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				emailAddress.EmailAddressNum=Meth.GetLong(MethodBase.GetCurrentMethod(),emailAddress);
				return emailAddress.EmailAddressNum;
			}
			return Crud.EmailAddressCrud.Insert(emailAddress);
		}

		///<summary></summary>
		public static void Update(EmailAddress emailAddress){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),emailAddress);
				return;
			}
			Crud.EmailAddressCrud.Update(emailAddress);
		}

		///<summary></summary>
		public static void Delete(long emailAddressNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),emailAddressNum);
				return;
			}
			string command= "DELETE FROM emailaddress WHERE EmailAddressNum = "+POut.Long(emailAddressNum);
			Db.NonQ(command);
		}
	}
}