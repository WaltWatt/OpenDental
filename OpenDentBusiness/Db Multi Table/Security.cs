using CodeBase;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

namespace OpenDentBusiness{
	///<summary></summary>
	public class Security{
		///<summary>The current user.  Might be null when first starting the program.  Otherwise, must contain valid user.</summary>
		private static Userod curUser;
		///<summary>The current user.  Might be null when first starting the program.  Otherwise, must contain valid user.</summary>
		[ThreadStatic]
		private static Userod _curUserT;
		///<summary>Remember the password that the user typed in. This is always saved on Log On. 
		///Do not store it in the database.  We will need it when connecting to the web service.
		///Needed for CEMT and reporting servers. If eCW, then this is already encrypted.</summary>
		private static string _passwordTyped;
		///<summary>The password that the user typed in.</summary>
		[ThreadStatic]
		private static string _passwordTypedT;
		///<summary>Tracks whether or not the user is logged in.  Security.CurUser==null usually is used for this purpose, 
		///but in Middle Tier we do not null out CurUser so that queries can continue to be run on the web service.</summary>
		public static bool IsUserLoggedIn;
		///<summary>The last local datetime that there was any mouse or keyboard activity.  Used for auto logoff comparison and for disabling signal 
		///processing due to inactivity.  Must be public so that it can be accessed from multiple application level classes.</summary>
		public static DateTime DateTimeLastActivity;

		public static Userod CurUser {
			get {
				if(_curUserT!=null) {
					return _curUserT;
				}
				if(RemotingClient.RemotingRole==RemotingRole.ServerWeb) {
					throw new ApplicationException("Security.Userod not accessible from RemotingRole.ServerWeb.");
				}
				return curUser;
			}
			set {
				_curUserT=value;
				curUser=value;
			}
		}

		///<summary>Remember the password that the user typed in. This is always saved on Log On. 
		///Do not store it in the database.  We will need it when connecting to the web service.
		///Needed for CEMT and reporting servers. If eCW, then this is already encrypted.</summary>
		public static string PasswordTyped {
			get {
				if(_passwordTypedT!=null) {
					return _passwordTypedT;
				}
				if(RemotingClient.RemotingRole==RemotingRole.ServerWeb) {
					throw new ApplicationException("Security.PasswordTyped not accessible from RemotingRole.ServerWeb.");
				}
				return _passwordTyped;
			}
			set {
				_passwordTypedT=value;
				_passwordTyped=value;
			}
		}

		///<summary></summary>
		public Security(){
			//No need to check RemotingRole; no call to db.
		}

		///<summary>Checks to see if current user is authorized.  It also checks any date restrictions.  If not authorized, it gives a Message box saying so and returns false.</summary>
		public static bool IsAuthorized(Permissions perm){
			//No need to check RemotingRole; no call to db.
			return IsAuthorized(perm,DateTime.MinValue,false);
		}

		public static bool IsAuthorized(Permissions perm,long FKey) {
			//No need to check RemotingRole; no call to db.
			return IsAuthorized(perm,DateTime.MinValue,false,true,0,-1,0,FKey);
		}

		///<summary>Checks to see if current user is authorized.  It also checks any date restrictions.  If not authorized, it gives a Message box saying so and returns false.</summary>
		public static bool IsAuthorized(Permissions perm,DateTime date){
			//No need to check RemotingRole; no call to db.
			return IsAuthorized(perm,date,false);
		}

		///<summary>Checks to see if current user is authorized.  It also checks any date restrictions.  If not authorized, it gives a Message box saying so and returns false.</summary>
		public static bool IsAuthorized(Permissions perm,bool suppressMessage){
			//No need to check RemotingRole; no call to db.
			return IsAuthorized(perm,DateTime.MinValue,suppressMessage);
		}

		///<summary>Checks to see if current user is authorized.  It also checks any date restrictions.  If not authorized, it gives a Message box saying so and returns false.</summary>
		public static bool IsAuthorized(Permissions perm,DateTime date,bool suppressMessage){
			//No need to check RemotingRole; no call to db.
			return IsAuthorized(perm,date,suppressMessage,false);
		}

		///<summary>Checks to see if current user is authorized.  It also checks any date restrictions.  If not authorized, it gives a Message box saying so and returns false.</summary>
		public static bool IsAuthorized(Permissions perm,DateTime date,bool suppressMessage,bool suppressLockDateMessage) {
			return IsAuthorized(perm,date,suppressMessage,suppressLockDateMessage,0,-1,0,0);
		}

		public static bool IsAuthorized(Permissions perm,DateTime date,long procCodeNum,double procCodeFee) {
			return IsAuthorized(perm,date,false,false,procCodeNum,procCodeFee,0,0);
		}
		
		///<summary>Checks to see if current user is authorized.  It also checks any date restrictions.  If not authorized, it gives a Message box saying so and returns false.</summary>
		public static bool IsAuthorized(Permissions perm,DateTime date,bool suppressMessage,bool suppressLockDateMessage,long procCodeNum,
			double procCodeFee,long sheetDefNum,long fKey) 
		{
			//No need to check RemotingRole; no call to db.
			if(Security.CurUser==null) {
				if(!suppressMessage) {
					MessageBox.Show(Lans.g("Security","Not authorized for")+"\r\n"+GroupPermissions.GetDesc(perm));
				}
				return false;
			}
			try {
				return IsAuthorized(perm,date,suppressMessage,suppressLockDateMessage,curUser,procCodeNum,procCodeFee,sheetDefNum,fKey);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return false;
			}
		}

		///<summary>Will throw an error if not authorized and message not suppressed.</summary>
		public static bool IsAuthorized(Permissions perm,DateTime date,bool suppressMessage,bool suppressLockDateMessage,Userod curUser,
			long procCodeNum,double procFee,long sheetDefNum,long fKey) 
		{
			//No need to check RemotingRole; no call to db.
			date=date.Date; //Remove the time portion of date so we can compare strictly as a date later.
			//Check eConnector permission first.
			if(IsValidEServicePermission(perm)) {
				return true;
			}
			if(!GroupPermissions.HasPermission(curUser,perm,fKey)){
				if(!suppressMessage){
					throw new Exception(Lans.g("Security","Not authorized.")+"\r\n"
						+Lans.g("Security","A user with the SecurityAdmin permission must grant you access for")+":\r\n"+GroupPermissions.GetDesc(perm));
				}
				return false;
			}
			if(perm==Permissions.AccountingCreate || perm==Permissions.AccountingEdit){
				if(date <= PrefC.GetDate(PrefName.AccountingLockDate)){
					if(!suppressMessage && !suppressLockDateMessage) {
						throw new Exception(Lans.g("Security","Locked by Administrator."));
					}
					return false;	
				}
			}
			//Check the global security lock------------------------------------------------------------------------------------
			if(IsGlobalDateLock(perm,date,suppressMessage||suppressLockDateMessage,procCodeNum,procFee,sheetDefNum)) {
				return false;
			}
			//Check date/days limits on individual permission----------------------------------------------------------------
			if(!GroupPermissions.PermTakesDates(perm)){
				return true;
			}
			//Include CEMT users, as a CEMT user could be logged in when this is checked.
			DateTime dateLimit =GetDateLimit(perm,curUser.GetGroups(true).Select(x => x.UserGroupNum).ToList()); 
			if(date>dateLimit){//authorized
				return true;
			}
			//Prevents certain bugs when 1/1/1 dates are passed in and compared----------------------------------------------
			//Handling of min dates.  There might be others, but we have to handle them individually to avoid introduction of bugs.
			if(perm==Permissions.ClaimDelete//older versions did not have SecDateEntry
				|| perm==Permissions.ClaimSentEdit//no date sent was entered before setting claim received
				|| perm==Permissions.ProcComplEdit//a completed procedure with a min date.
				|| perm==Permissions.ProcComplEditLimited//because ProcComplEdit was in this list
				|| perm==Permissions.ProcExistingEdit//a completed EO or EC procedure with a min date.
				|| perm==Permissions.InsPayEdit//a claim payment with no date.
				|| perm==Permissions.InsWriteOffEdit//older versions did not have SecDateEntry or DateEntryC
				|| perm==Permissions.TreatPlanEdit
				|| perm==Permissions.AdjustmentEdit
				|| perm==Permissions.CommlogEdit//usually from a conversion
				|| perm==Permissions.ProcDelete//because older versions did not set the DateEntryC.
				|| perm==Permissions.ImageDelete//In case an image has a creation date of DateTime.MinVal.
				|| perm==Permissions.PerioEdit//In case perio chart exam has a creation date of DateTime.MinValue.
				|| perm==Permissions.PreAuthSentEdit)//older versions did not have SecDateEntry
			{
				if(date.Year<1880	&& dateLimit.Year<1880) {
					return true;
				}
			}
			if(!suppressMessage){
				throw new Exception(Lans.g("Security","Not authorized for")+"\r\n"
					+GroupPermissions.GetDesc(perm)+"\r\n"+Lans.g("Security","Date limitation"));
			}
			return false;		
		}

		///<summary>Surrond with Try/Catch. Error messages will be thrown to caller.</summary>
		public static bool IsGlobalDateLock(Permissions perm,DateTime date,bool isSilent=false,long codeNum=0,double procFee=-1,long sheetDefNum=0) {
			if(!(new[] {
				 Permissions.AdjustmentCreate
				,Permissions.AdjustmentEdit
				,Permissions.PaymentCreate
				,Permissions.PaymentEdit
				,Permissions.ProcComplCreate
				,Permissions.ProcComplEdit
				,Permissions.ProcExistingEdit
			//,Permissions.ProcComplEditLimited
			//,Permissions.ImageDelete
				,Permissions.InsPayCreate
				,Permissions.InsPayEdit
			//,Permissions.InsWriteOffEdit//per Nathan 7/5/2016 this should not be affected by the global date lock
				,Permissions.SheetEdit
				,Permissions.SheetDelete
				,Permissions.CommlogEdit
			//,Permissions.ClaimDelete //per Nathan 01/18/2018 this should not be affected by the global date lock
				,Permissions.PayPlanEdit
			//,Permissions.ClaimHistoryEdit //per Nathan & Mark 03/01/2018 this should not be affected by the global lock date, not financial data.
			}).Contains(perm)) {
				return false;//permission being checked is not affected by global lock date.
			}
			if(date.Year==1) {
				return false;//Invalid or MinDate passed in.
			}
			if(!PrefC.GetBool(PrefName.SecurityLockIncludesAdmin) && GroupPermissions.HasPermission(Security.CurUser,Permissions.SecurityAdmin,0)) {
				return false;//admins are never affected by global date limitation when preference is false.
			}
			if(perm.In(Permissions.ProcComplCreate,Permissions.ProcComplEdit,Permissions.ProcExistingEdit) 
				&& ProcedureCodes.CanBypassLockDate(codeNum,procFee)) 
			{
				return false;
			}
			if(perm.In(Permissions.SheetEdit,Permissions.SheetDelete) && sheetDefNum > 0	&& SheetDefs.CanBypassLockDate(sheetDefNum)) {
				return false;
			}
			//If global lock is Date based.
			if(date <= PrefC.GetDate(PrefName.SecurityLockDate)) {
				if(!isSilent) {
					MessageBox.Show(Lans.g("Security","Locked by Administrator before ")+PrefC.GetDate(PrefName.SecurityLockDate).ToShortDateString());
				}
				return true;
			}
			//If global lock is days based.
			int lockDays = PrefC.GetInt(PrefName.SecurityLockDays);
			if(lockDays>0 && date<=DateTime.Today.AddDays(-lockDays)) {
				if(!isSilent) {
					MessageBox.Show(Lans.g("Security","Locked by Administrator before ")+lockDays.ToString()+" days.");
				}
				return true;
			}
			return false;
		}
		
		///<summary>Returns the Date that the user is restricted to for the passed-in PermType. 
		///Returns MinVal if the user is not restricted or does not have the permission.</summary>
		private static DateTime GetDateLimit(Permissions permType,List<long> listUserGroupNums){
			//No need to check RemotingRole; no call to db.
			DateTime nowDate=DateTime.MinValue;
			Func<DateTime> getNowDate=new Func<DateTime>(() => {
				if(nowDate.Year < 1880) {
					nowDate=MiscData.GetNowDateTime().Date;
				}
				return nowDate;
			});
			DateTime retVal=DateTime.MinValue;
			List<GroupPermission> listGroupPerms = GroupPermissions.GetForUserGroups(listUserGroupNums,permType);
			//get the permission that applies
			GroupPermission perm = listGroupPerms.OrderBy((GroupPermission y) => {
				if(y.NewerDays==0 && y.NewerDate == DateTime.MinValue) {
					return DateTime.MinValue;
				}
				if(y.NewerDays==0) {
					return y.NewerDate;
				}
				return getNowDate().AddDays(-y.NewerDays);
			}).FirstOrDefault();
			if(perm==null) {
				//do not change retVal. The user does not have the permission.
			}
			else if(perm.NewerDate.Year < 1880 && perm.NewerDays == 0) {
				//do not change retVal. The user is not restricted by date.
			}
			else if(perm.NewerDate.Year>1880) {
				retVal=perm.NewerDate;
			}
			else if(getNowDate().AddDays(-perm.NewerDays)>retVal) {
				retVal=getNowDate().AddDays(-perm.NewerDays);
			}
			return retVal;
		}

		///<summary>Gets a module that the user has permission to use.  Tries the suggestedI first.  If a -1 is supplied, it tries to find any authorized module.  If no authorization for any module, it returns a -1, causing no module to be selected.</summary>
		public static int GetModule(int suggestI){
			//No need to check RemotingRole; no call to db.
			if(suggestI!=-1 && IsAuthorized(PermofModule(suggestI),DateTime.MinValue,true)){
				return suggestI;
			}
			for(int i=0;i<7;i++){
				if(IsAuthorized(PermofModule(i),DateTime.MinValue,true)){
					return i;
				}
			}
			return -1;
		}

		private static Permissions PermofModule(int i){
			//No need to check RemotingRole; no call to db.
			switch(i){
				case 0:
					return Permissions.AppointmentsModule;
				case 1:
					return Permissions.FamilyModule;
				case 2:
					return Permissions.AccountModule;
				case 3:
					return Permissions.TPModule;
				case 4:
					return Permissions.ChartModule;
				case 5:
					return Permissions.ImagesModule;
				case 6:
					return Permissions.ManageModule;
			}
			return Permissions.None;
		}

		///<summary>Will throw an exception if server cannot validate username and password.
		///configPath will be empty from a workstation and filled from the server.  If Ecw, odpass will actually be the hash.</summary>
		public static Userod LogInWeb(string oduser,string odpass,string configPath,string clientVersionStr,bool usingEcw) {
			//Unusual remoting role check designed for first time logging in via middle tier.
			if(RemotingClient.RemotingRole==RemotingRole.ServerWeb) {
				Userod user=Userods.CheckUserAndPassword(oduser,odpass,usingEcw);
				if(string.IsNullOrEmpty(odpass)) {//All middle tier connections must pass in a password.
					throw new Exception("Invalid username or password.");
				}
				string command="SELECT ValueString FROM preference WHERE PrefName='ProgramVersion'";
				string dbVersionStr=Db.GetScalar(command);
				string serverVersionStr=Assembly.GetAssembly(typeof(Db)).GetName().Version.ToString(4);
				#if DEBUG
					if(Assembly.GetAssembly(typeof(Db)).GetName().Version.Build==0) {
						command="SELECT ValueString FROM preference WHERE PrefName='DataBaseVersion'";//Using this during debug in the head makes it open fast with less fiddling.
						dbVersionStr=Db.GetScalar(command);
					}
				#endif
				if(dbVersionStr!=serverVersionStr) {
					throw new Exception("Version mismatch.  Server:"+serverVersionStr+"  Database:"+dbVersionStr);
				}
				Version clientVersion=new Version(clientVersionStr);
				Version serverVersion=new Version(serverVersionStr);
				if(clientVersion > serverVersion){
					throw new Exception("Version mismatch.  Client:"+clientVersionStr+"  Server:"+serverVersionStr);
				}
				//if clientVersion == serverVersion, than we need do nothing.
				//if clientVersion < serverVersion, than an update will later be triggered.
				//Security.CurUser=user;//we're on the server, so this is meaningless
				return user;
				//return 0;//meaningless
			}
			else {
				//Because RemotingRole has not been set, and because CurUser has not been set,
				//this particular method is more verbose than most and does not use Meth.
				//It's not a good example of the standard way of doing things.
				DtoGetObject dto=new DtoGetObject();
				dto.Credentials=new Credentials();
				dto.Credentials.Username=oduser;
				dto.Credentials.Password=odpass;//Userods.EncryptPassword(password);
				dto.MethodName="OpenDentBusiness.Security.LogInWeb";
				dto.ObjectType=typeof(Userod).FullName;
				object[] parameters=new object[] { oduser,odpass,configPath,clientVersionStr,usingEcw };
				dto.Params=DtoObject.ConstructArray(MethodBase.GetCurrentMethod(),parameters);
				return RemotingClient.ProcessGetObject<Userod>(dto);//can throw exception
			}
		}

		#region eServices

		///<summary>Returns false if the currently logged in user is not designated for the eConnector or if the user does not have permission.</summary>
		private static bool IsValidEServicePermission(Permissions perm) {
			//No need to check RemotingRole; no call to db.
			if(curUser==null) {
				return false;
			}
			//Run specific checks against certain types of eServices.
			switch(curUser.EServiceType) {
				case EServiceTypes.Broadcaster:
				case EServiceTypes.BroadcastMonitor:
				case EServiceTypes.ServiceMainHQ:
					return true;//These eServices are at HQ and we trust ourselves to have full permissions for any S class method.
				case EServiceTypes.EConnector:
					return IsPermAllowedEConnector(perm);
				case EServiceTypes.None:
				default:
					return false;//Not an eService, let IsAuthorized handle the permission checking.
			}
		}

		///<summary>Returns true if the eConnector should be allowed to run methods with the passed in permission.</summary>
		private static bool IsPermAllowedEConnector(Permissions perm) {
			//We are typically on the customers eConnector and need to be careful when giving access to certain permission types.
			//Engineers must EXCPLICITLY add permissions to this switch statement as they need them.
			//Be very cautious when adding permissions because the flood gates for that permission will be opened once added.
			//E.g. we should never add a permission like Setup or SecurityAdmin.  If there is a need for such a thing, we need to rethink this paradigm.
			switch(perm) {
				//Add additional permissions to this case as needed to grant access.
				case Permissions.EmailSend:
					return true;
				default:
					return false;
			}
		}

		#endregion


	}
}

























