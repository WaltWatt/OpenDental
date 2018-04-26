using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDentBusiness {
	public class CreditCardUtils {

		public static string GetCardType(string ccNum) {
			if(ccNum==null || ccNum=="") {
				return "";
			}
			ccNum=StripNonDigits(ccNum);
			if(ccNum.StartsWith("4")) {
				return "VISA";
			}
			if(ccNum.StartsWith("5")) {
				return "MASTERCARD";
			}
			if(ccNum.StartsWith("34") || ccNum.StartsWith("37")) {
				return "AMEX";
			}
			if(ccNum.StartsWith("30") || ccNum.StartsWith("36") || ccNum.StartsWith("38")) {
				return "DINERS";
			}
			if(ccNum.StartsWith("6011")) {
				return "DISCOVER";
			}
			return "";
		}

		///<summary>Strips non-digit characters from a string. Returns the modified string, or null if 's' is null.</summary>
		public static string StripNonDigits(string s) {
			return StripNonDigits(s,new char[] { });
		}

		///<summary>Strips non-digit characters from a string. The variable s is the string to strip. The allowed array must contain characters that should not be stripped. Returns the modified string, or null if 's' is null.</summary>
		public static string StripNonDigits(string s,char[] allowed) {
			if(s==null) {
				return null;
			}
			StringBuilder buff=new StringBuilder(s);
			StripNonDigits(buff,allowed);
			return buff.ToString();
		}

		///<summary>Strips non-digit characters from a string. The variable s is the string to strip.</summary>
		public static void StripNonDigits(StringBuilder s) {
			StripNonDigits(s,new char[] { });
		}

		///<summary>Strips non-digit characters from a string. The variable s is the string to strip. The allowed array must contain the characters that should not be stripped.</summary>
		public static void StripNonDigits(StringBuilder s,char[] allowed) {
			for(int i = 0;i<s.Length;i++) {
				if(!Char.IsDigit(s[i]) && !ContainsCharacter(s[i],allowed)) {
					s.Remove(i,1);
					i--;
				}
			}
		}

		///<summary>Searches a character array for the presence of the given character. Variable c is the character to search for. The search array is the array to search in. Returns true if the character is present in the array.  false otherwise.</summary>
		public static bool ContainsCharacter(char c,char[] search) {
			foreach(char x in search) {
				if(c==x) {
					return true;
				}
			}
			return false;
		}
	}
}
