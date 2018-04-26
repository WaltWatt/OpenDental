using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace OpenDentBusiness{
	///<summary></summary>
	public class UserWebs {
		#region Get Methods
		#endregion

		#region Modification Methods

		#region Insert

		///<summary></summary>
		public static void InsertMany(List<UserWeb> listUserWebs) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),listUserWebs);
				return;
			}
			Crud.UserWebCrud.InsertMany(listUserWebs);
		}

		#endregion

		#region Update
		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods		

		///<summary>Creates a username that is not yet in use.</summary>
		public static string CreateUserNameFromPat(Patient pat,UserWebFKeyType fkeyType) {
			//No need to check RemotingRole; no call to db.
			string retVal="";
			bool userNameFound=false;
			Random rand=new Random();
			int i=0;
			while(!userNameFound) {
				retVal=pat.FName+rand.Next(100,100000);
				if(!UserWebs.UserNameExists(retVal,fkeyType)) {
					userNameFound=true;
				}
				if(i>1000) {
					throw new CodeBase.ODException(Lans.g("UserWebs","Unable to create username for patient."));
				}
				i++;
			}
			return retVal;
		}

		///<summary>Generates a random password 8 char long containing at least one uppercase, one lowercase, and one number.</summary>
		public static string GenerateRandomPassword(int length) {
			//No need to check RemotingRole; no call to db.
			//Chracters like o(letter O), 0 (Zero), l (letter l), 1 (one) etc are avoided because they can be ambigious.
			string PASSWORD_CHARS_LCASE="abcdefgijkmnopqrstwxyz";
			string PASSWORD_CHARS_UCASE="ABCDEFGHJKLMNPQRSTWXYZ";
			string PASSWORD_CHARS_NUMERIC="23456789";
			//Create a local array containing supported password characters grouped by types.
			char[][] charGroups=new char[][]{
						PASSWORD_CHARS_LCASE.ToCharArray(),
						PASSWORD_CHARS_UCASE.ToCharArray(),
						PASSWORD_CHARS_NUMERIC.ToCharArray(),};
			//Use this array to track the number of unused characters in each character group.
			int[] charsLeftInGroup=new int[charGroups.Length];
			//Initially, all characters in each group are not used.
			for(int i = 0;i<charsLeftInGroup.Length;i++) {
				charsLeftInGroup[i]=charGroups[i].Length;
			}
			//Use this array to track (iterate through) unused character groups.
			int[] leftGroupsOrder=new int[charGroups.Length];
			//Initially, all character groups are not used.
			for(int i = 0;i<leftGroupsOrder.Length;i++) {
				leftGroupsOrder[i]=i;
			}
			Random random=new Random();
			//This array will hold password characters.
			char[] password=new char[length];
			//Index of the next character to be added to password.
			int nextCharIdx;
			//Index of the next character group to be processed.
			int nextGroupIdx;
			//Index which will be used to track not processed character groups.
			int nextLeftGroupsOrderIdx;
			//Index of the last non-processed character in a group.
			int lastCharIdx;
			//Index of the last non-processed group.
			int lastLeftGroupsOrderIdx=leftGroupsOrder.Length - 1;
			//Generate password characters one at a time.
			for(int i = 0;i<password.Length;i++) {
				//If only one character group remained unprocessed, process it;
				//otherwise, pick a random character group from the unprocessed
				//group list. To allow a special character to appear in the
				//first position, increment the second parameter of the Next
				//function call by one, i.e. lastLeftGroupsOrderIdx + 1.
				if(lastLeftGroupsOrderIdx==0) {
					nextLeftGroupsOrderIdx=0;
				}
				else {
					nextLeftGroupsOrderIdx=random.Next(0,lastLeftGroupsOrderIdx);
				}
				//Get the actual index of the character group, from which we will
				//pick the next character.
				nextGroupIdx=leftGroupsOrder[nextLeftGroupsOrderIdx];
				//Get the index of the last unprocessed characters in this group.
				lastCharIdx=charsLeftInGroup[nextGroupIdx] - 1;
				//If only one unprocessed character is left, pick it; otherwise,
				//get a random character from the unused character list.
				if(lastCharIdx==0) {
					nextCharIdx=0;
				}
				else {
					nextCharIdx=random.Next(0,lastCharIdx+1);
				}
				//Add this character to the password.
				password[i]=charGroups[nextGroupIdx][nextCharIdx];
				//If we processed the last character in this group, start over.
				if(lastCharIdx==0) {
					charsLeftInGroup[nextGroupIdx]=charGroups[nextGroupIdx].Length;
					//There are more unprocessed characters left.
				}
				else {
					//Swap processed character with the last unprocessed character
					//so that we don't pick it until we process all characters in
					//this group.
					if(lastCharIdx !=nextCharIdx) {
						char temp=charGroups[nextGroupIdx][lastCharIdx];
						charGroups[nextGroupIdx][lastCharIdx]=charGroups[nextGroupIdx][nextCharIdx];
						charGroups[nextGroupIdx][nextCharIdx]=temp;
					}
					//Decrement the number of unprocessed characters in
					//this group.
					charsLeftInGroup[nextGroupIdx]--;
				}
				//If we processed the last group, start all over.
				if(lastLeftGroupsOrderIdx==0) {
					lastLeftGroupsOrderIdx=leftGroupsOrder.Length - 1;
					//There are more unprocessed groups left.
				}
				else {
					//Swap processed group with the last unprocessed group
					//so that we don't pick it until we process all groups.
					if(lastLeftGroupsOrderIdx !=nextLeftGroupsOrderIdx) {
						int temp=leftGroupsOrder[lastLeftGroupsOrderIdx];
						leftGroupsOrder[lastLeftGroupsOrderIdx]=
																leftGroupsOrder[nextLeftGroupsOrderIdx];
						leftGroupsOrder[nextLeftGroupsOrderIdx]=temp;
					}
					//Decrement the number of unprocessed groups.
					lastLeftGroupsOrderIdx--;
				}
			}
			//Convert password characters into a string and return the result.
			return new string(password);
		}

		public static bool ValidatePatientAccess(Patient pat,out string strErrors) {
			//No need to check RemotingRole; no call to db.
			StringBuilder strbErrors=new StringBuilder();
			if(pat.FName.Trim()=="") {
				strbErrors.AppendLine(Lans.g("PatientPortal","Missing patient first name."));
			}
			if(pat.LName.Trim()=="") {
				strbErrors.AppendLine(Lans.g("PatientPortal","Missing patient last name."));
			}
			if(pat.Address.Trim()=="") {
				strbErrors.AppendLine(Lans.g("PatientPortal","Missing patient address line 1."));
			}
			if(pat.City.Trim()=="") {
				strbErrors.AppendLine(Lans.g("PatientPortal","Missing patient city."));
			}
			if(CultureInfo.CurrentCulture.Name.EndsWith("US") && pat.State.Trim().Length!=2) {
				strbErrors.AppendLine(Lans.g("PatientPortal","Invalid patient state.  Must be two letters."));
			}
			if(pat.Birthdate.Year<1880) {
				strbErrors.AppendLine(Lans.g("PatientPortal","Missing patient birth date."));
			}
			if(pat.HmPhone.Trim()=="" && pat.WirelessPhone.Trim()=="" && pat.WkPhone.Trim()=="") {
				strbErrors.AppendLine(Lans.g("PatientPortal","Missing patient phone;  Must have home, wireless, or work phone."));
			}
			strErrors=strbErrors.ToString();
			return strErrors=="";
		}

		///<summary>Generates a username and password if necessary for this patient. If the patient already has access to the Patient Portal or if they
		///are not eligible to be given access, this will return null.</summary>
		public static UserWeb GetNewPatientPortalCredentials(Patient pat,bool doUpdateDatabase,out string passwordGenerated) {
			//No need to check RemotingRole; no call to db.
			passwordGenerated="";
			if(string.IsNullOrEmpty(PrefC.GetString(PrefName.PatientPortalURL))) {
				return null;//Haven't set up patient portal yet.
			}
			string errors;
			if(!UserWebs.ValidatePatientAccess(pat,out errors)) {
				return null;//Patient is missing necessary fields.
			}
			UserWeb userWeb=UserWebs.GetByFKeyAndType(pat.PatNum,UserWebFKeyType.PatientPortal);
			if(userWeb==null) {
				userWeb=new UserWeb();
				userWeb.UserName=UserWebs.CreateUserNameFromPat(pat,UserWebFKeyType.PatientPortal);
				userWeb.FKey=pat.PatNum;
				userWeb.FKeyType=UserWebFKeyType.PatientPortal;
				userWeb.RequireUserNameChange=true;
				userWeb.Password="";
				userWeb.IsNew=true;
				if(doUpdateDatabase) {
					UserWebs.Insert(userWeb);
				}
			}
			if(!string.IsNullOrEmpty(userWeb.Password) //If they already have access to the Patient Portal, return.
				&& !userWeb.RequirePasswordChange) //If they need to change their password, we are going to generate another password for them.
			{ 
				return null;
			}
			if(string.IsNullOrEmpty(userWeb.Password)//Only insert an EHR event if their password is blank (meaning they don't currently have access).
				&& doUpdateDatabase) 
			{ 
				EhrMeasureEvent newMeasureEvent=new EhrMeasureEvent();
				newMeasureEvent.DateTEvent=DateTime.Now;
				newMeasureEvent.EventType=EhrMeasureEventType.OnlineAccessProvided;
				newMeasureEvent.PatNum=pat.PatNum;
				newMeasureEvent.MoreInfo="";
				EhrMeasureEvents.Insert(newMeasureEvent);
			}
			passwordGenerated=UserWebs.GenerateRandomPassword(8);
			userWeb.Password=Userods.HashPassword(passwordGenerated,false);
			userWeb.RequirePasswordChange=true;
			if(doUpdateDatabase) {
				UserWebs.Update(userWeb);
			}
			return userWeb;
		}

		#endregion


		///<summary>Gets one UserWeb from the db.</summary>
		public static UserWeb GetOne(long userWebNum){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<UserWeb>(MethodBase.GetCurrentMethod(),userWebNum);
			}
			return Crud.UserWebCrud.SelectOne(userWebNum);
		}

		///<summary></summary>
		public static long Insert(UserWeb userWeb){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				userWeb.UserWebNum=Meth.GetLong(MethodBase.GetCurrentMethod(),userWeb);
				return userWeb.UserWebNum;
			}
			return Crud.UserWebCrud.Insert(userWeb);
		}

		///<summary></summary>
		public static void Update(UserWeb userWeb){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userWeb);
				return;
			}
			Crud.UserWebCrud.Update(userWeb);
		}

		///<summary></summary>
		public static void Update(UserWeb userWeb,UserWeb userWebOld){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userWeb);
				return;
			}
			Crud.UserWebCrud.Update(userWeb,userWebOld);
		}

		///<summary></summary>
		public static void Delete(long userWebNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),userWebNum);
				return;
			}
			Crud.UserWebCrud.Delete(userWebNum);
		}

		///<summary>Gets the UserWeb associated to the passed in username and hashed password.  Must provide the FKeyType.  Returns null if not found.</summary>
		public static UserWeb GetByUserNameAndPassword(string userName,string passwordHashed,UserWebFKeyType fkeyType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<UserWeb>(MethodBase.GetCurrentMethod(),userName,passwordHashed,fkeyType);
			}
			string command="SELECT * "
				+"FROM userweb "
				+"WHERE Password='"+passwordHashed+"' "
				+"AND UserName='"+OpenDentBusiness.POut.String(userName)+"' "
				+"AND FKeyType="+OpenDentBusiness.POut.Int((int)fkeyType)+"";
			return Crud.UserWebCrud.SelectOne(command);
		}

		///<summary>Gets the UserWeb associated to the passed in username.  Must provide the FKeyType.  Returns null if not found.</summary>
		public static UserWeb GetByUserName(string userName,UserWebFKeyType fkeyType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<UserWeb>(MethodBase.GetCurrentMethod(),userName,fkeyType);
			}
			string command="SELECT * "
				+"FROM userweb "
				+"WHERE UserName='"+OpenDentBusiness.POut.String(userName)+"' "
				+"AND FKeyType="+OpenDentBusiness.POut.Int((int)fkeyType)+"";
			return Crud.UserWebCrud.SelectOne(command);
		}

		///<summary>Gets the UserWeb associated to the passed in username and reset code.  Must provide the FKeyType.  Returns null if not found.</summary>
		public static UserWeb GetByUserNameAndResetCode(string userName,string resetCode,UserWebFKeyType fkeyType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<UserWeb>(MethodBase.GetCurrentMethod(),userName,resetCode,fkeyType);
			}
			string command="SELECT * "
				+"FROM userweb "
				+"WHERE userweb.FKeyType="+POut.Int((int)UserWebFKeyType.PatientPortal)+" "
				+"AND userweb.UserName='"+POut.String(userName)+"' "
				+"AND userweb.PasswordResetCode='"+POut.String(resetCode)+"' ";
			return Crud.UserWebCrud.SelectOne(command);
		}

		public static UserWeb GetByFKeyAndType(long fkey,UserWebFKeyType fkeyType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetObject<UserWeb>(MethodBase.GetCurrentMethod(),fkey,fkeyType);
			}
			string command="SELECT * FROM userweb WHERE FKey="+POut.Long(fkey)+" AND FKeyType="+POut.Int((int)fkeyType)+" ";
			return Crud.UserWebCrud.SelectOne(command);
		}

		public static bool UserNameExists(string userName,UserWebFKeyType fkeyType) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb){
				return Meth.GetBool(MethodBase.GetCurrentMethod(),userName,fkeyType);
			}
			string command="SELECT COUNT(*) FROM userweb WHERE UserName='"+POut.String(userName)+"' AND FKeyType="+POut.Int((int)fkeyType)+" ";
			string count=Db.GetCount(command);
			if(count!="0") {
				return true;
			}
			return false;
		}

	}
}