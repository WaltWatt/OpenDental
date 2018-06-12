using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace CodeBase {
	public static class MiscUtils {

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd,int wMsg,int wParam,int lParam);

		private static Random rand=new Random();

		public static string CreateRandomAlphaNumericString(int length){
			string result="";
			string randChrs="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			for(int i=0;i<length;i++){
				result+=randChrs[rand.Next(0,randChrs.Length-1)];
			}
			return result;
		}

		public static string CreateRandomNumericString(int length) {
			string result="";
			string randChrs="0123456789";
			for(int i = 0;i<length;i++) {
				result+=randChrs[rand.Next(0,randChrs.Length-1)];
			}
			return result;
		}

		///<summary>Encrypts signature text and returns a base 64 string so that it can go directly into the database.</summary>
		public static string Encrypt(string encrypt) {
			UTF8Encoding enc=new UTF8Encoding();
			byte[] arrayEncryptBytes=Encoding.UTF8.GetBytes(encrypt);
			MemoryStream ms=new MemoryStream();
			CryptoStream cs=null;
			Aes aes=new AesCryptoServiceProvider();
			aes.Key=enc.GetBytes("AKQjlLUjlcABVbqp");
			aes.IV=new byte[16];
			ICryptoTransform encryptor=aes.CreateEncryptor(aes.Key,aes.IV);
			cs=new CryptoStream(ms,encryptor,CryptoStreamMode.Write);
			cs.Write(arrayEncryptBytes,0,arrayEncryptBytes.Length);
			cs.FlushFinalBlock();
			byte[] retval=new byte[ms.Length];
			ms.Position=0;
			ms.Read(retval,0,(int)ms.Length);
			cs.Dispose();
			ms.Dispose();
			if(aes!=null) {
				aes.Clear();
			}
			return Convert.ToBase64String(retval);
		}

		public static string Decrypt(string encString,bool doThrow=false) {
			try {
				byte[] encrypted=Convert.FromBase64String(encString);
				MemoryStream ms=null;
				CryptoStream cs=null;
				StreamReader sr=null;
				Aes aes=new AesCryptoServiceProvider();
				UTF8Encoding enc=new UTF8Encoding();
				aes.Key=enc.GetBytes("AKQjlLUjlcABVbqp");
				aes.IV=new byte[16];
				ICryptoTransform decryptor=aes.CreateDecryptor(aes.Key,aes.IV);
				ms=new MemoryStream(encrypted);
				cs=new CryptoStream(ms,decryptor,CryptoStreamMode.Read);
				sr=new StreamReader(cs);
				string decrypted=sr.ReadToEnd();
				ms.Dispose();
				cs.Dispose();
				sr.Dispose();
				if(aes!=null) {
					aes.Clear();
				}
				return decrypted;
			}
			catch(Exception e) {
				if(doThrow) {
					throw e;
				}
				MessageBox.Show("Text entered was not valid encrypted text.");
				return "";
			}
		}

		///<summary>Accepts a 3 character string which represents a neutral culture (for example, "eng" for English) in the ISO639-2 format.  Returns null if the three letter ISO639-2 name is not standard (useful for determining custom languages).</summary>
		public static CultureInfo GetCultureFromThreeLetter(string strThreeLetterISOname) {
			if(strThreeLetterISOname==null || strThreeLetterISOname.Length!=3) {//Length check helps quickly identify custom languages.
				return null;
			}
			CultureInfo[] arrayCulturesNeutral=CultureInfo.GetCultures(CultureTypes.NeutralCultures);
			for(int i=0;i<arrayCulturesNeutral.Length;i++) {
				if(arrayCulturesNeutral[i].ThreeLetterISOLanguageName==strThreeLetterISOname) {
					return arrayCulturesNeutral[i];
				}
			}
			return null;
		}

		///<summary>Universal extension for IN statement.  Use like this: if(!x.In(2,3,61,71))</summary>
		public static bool In<T>(this T item,params T[] list) {
			return list.Contains(item);
		}

		///<summary>Universal extension for IN statement.  Use like this: if(!x.In(list))</summary>
		public static bool In<T>(this T item,IEnumerable<T> list) {
			return list.Contains(item);
		}

		///<summary>Extension for BETWEEN statement.  Use like this: if(x.Between(0,9))</summary>
		public static bool Between<T>(this T item,T lowerBound,T upperBound,bool isLowerBoundInclusive=true,bool isUpperBoundInclusive=true) 
			where T:IComparable 
		{
			if(isLowerBoundInclusive && isUpperBoundInclusive) {
				return (item.CompareTo(lowerBound)>=0 && item.CompareTo(upperBound)<=0);
			}
			if(isLowerBoundInclusive && !isUpperBoundInclusive) {
				return (item.CompareTo(lowerBound)>=0 && item.CompareTo(upperBound) < 0);
			}
			if(!isLowerBoundInclusive && isUpperBoundInclusive) {
				return (item.CompareTo(lowerBound) > 0 && item.CompareTo(upperBound)<=0);
			}
			if(!isLowerBoundInclusive && !isUpperBoundInclusive) {
				return (item.CompareTo(lowerBound) > 0 && item.CompareTo(upperBound) < 0);
			}
			return false;//This code is unreachable but the compiler doesn't realize it.
		}

		///<summary>Filters the current IEnumerable of objects based on the func provided.
		///C# does not provide a way to do listObj.Distinct(x => x.Field).  This extension allows us to do listObj.DistinctBy(x => x.Field)</summary>
		public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> listSource,Func<T,TKey> keySelector) {
			HashSet<TKey> hashKeys=new HashSet<TKey>();
			foreach(T source in listSource) {
				if(hashKeys.Add(keySelector(source))) {
					yield return source;//Manipulates the current sourceList instead of having to return an entire list.
				}
			}
		}

		///<summary>Finds installed IE version for this workstation and attempts to modify registry to force browser emualtion to this version.
		///Typically used in conjunction with WebBrowser control to ensure that the WebBrowser is running in the latest available emulation mode.
		///Returns true if the emulation version was previously wrong but was successfully updated. Otherwise returns false.
		///If true is returned than this application will need to be restarted in order for the changes to take effect.</summary>
		public static bool TryUpdateIeEmulation() {
			bool ret=false;
			try {
				int browserVersion;
				//Get the installed IE version.
				using(WebBrowser wb = new WebBrowser()) {
					browserVersion=wb.Version.Major;
				}
				int regVal;
				//Set the appropriate IE version
				if(browserVersion>=11) {
					regVal=11001;
				}
				else if(browserVersion==10) {
					regVal=10001;
				}
				else if(browserVersion==9) {
					regVal=9999;
				}
				else if(browserVersion==8) {
					regVal=8888;
				}
				else if(browserVersion==7) {
					regVal=7000;
				}
				else {//Unknown version.  This will happen when version 12 and beyond are released.
					regVal=browserVersion*1000+1;//Guess the regVal code needed based on the historic pattern.
				}
				//Set the actual key.  This key can be set without admin rights, because it is within the current user's registry store.
				string applicationName=Process.GetCurrentProcess().ProcessName+".exe";//This is OpenDental.vhost.exe when debugging, different for distributors.
				string keyPath=@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
				Microsoft.Win32.RegistryKey key=Microsoft.Win32.Registry.CurrentUser.OpenSubKey(keyPath,true);
				if(key==null) {
					key=Microsoft.Win32.Registry.CurrentUser.CreateSubKey(keyPath);
				}
				object keyValueCur=key.GetValue(applicationName);
				if(keyValueCur==null||keyValueCur.ToString()!=regVal.ToString()) {
					key.SetValue(applicationName,regVal,Microsoft.Win32.RegistryValueKind.DWord);
					ret=true;
				}
				key.Close();
			}
			catch (Exception e){
				e.DoNothing();
			}
			return ret;
		}

		///<summary>Returns true if the two time slots overlap in time. Slot1 and Slot2 are interchangeable.</summary>
		public static bool DoSlotsOverlap(DateTime slot1Start,DateTime slot1End,DateTime slot2Start,DateTime slot2End) {
			return (slot1End > slot2Start && slot1Start < slot2End);
		}

		///<summary>Returns exception string that includes the threadName if provided and exception type and up to 5 inner exceptions.
		///Used for both bugSubmissions and the MsgBoxCopyPaste shown to customers when a UE occurs.</summary>
		public static string GetExceptionText(Exception e,string threadName=null) {
			return "Unhandled exception"+(string.IsNullOrEmpty(threadName) ?"":" from "+threadName)+":  "
					+(string.IsNullOrEmpty(e.Message)?"No Exception Message":e.Message+"\r\n")
					+(string.IsNullOrEmpty(e.GetType().ToString())?"No Exception Type":e.GetType().ToString())+"\r\n"
					+(string.IsNullOrEmpty(e.StackTrace)?"No StackTrace":e.StackTrace)
					+InnerExceptionToString(e);//New lines handled in method.
		}

		///<summary>Formats the inner exception (and all its inner exceptions) as a readable string. Okay to pass in an exception with no inner 
		///exception.</summary>
		///<param name="depth">The recursive depth of the current method call.</param>
		public static string InnerExceptionToString(Exception ex,int depth = 0) {
			if(ex.InnerException==null
				|| depth>=5)//Limit to 5 inner exceptions to prevent infinite recursion
			{
				return "";
			}
			return "\r\n-------------------------------------------\r\n"
				+"Inner exception:  "+ex.InnerException.Message+"\r\n"+ex.InnerException.GetType().ToString()+"\r\n"
				+ex.InnerException.StackTrace
				+InnerExceptionToString(ex.InnerException,++depth);
		}
	}
}
