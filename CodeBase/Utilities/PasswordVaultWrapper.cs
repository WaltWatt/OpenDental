using Windows.Security.Credentials;

namespace CodeBase {
	///<summary>This wrapper class protects Windows 7 users from a runtime error that is caused by the Windows.wnd reference. This reference only works on windows 8 and up. Any calling of 
	///the member's methods MUST be try-caught to avoid a runtime error.</summary>
	public class PasswordVaultWrapper {
		
		///<summary>This method will throw an exception if you pass it a blank password. Windows cannot encrypt a blank password. Callers of this method should consider this scenario.
		///Throws exceptions.</summary>
		public static void WritePassword(string uri,string username,string password) {
			new PasswordVault().Add(new PasswordCredential("OpenDental Middle Tier:"+uri,username,password));//WCM encrypts the password
		}

		///<summary>An exception will be throw if the password cannot be found. Callers of this method should consider this scenario.</summary>
		public static string RetrievePassword(string uri,string username) {
			//This will only return the password if it has been saved under the current Windows user.
			PasswordCredential cred=new PasswordVault().Retrieve("OpenDental Middle Tier:"+uri,username);
			cred.RetrievePassword();
			return cred.Password;
		}
	}
}
