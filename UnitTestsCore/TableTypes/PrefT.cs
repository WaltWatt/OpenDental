using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenDentBusiness;

namespace UnitTestsCore {
	///<summary>All update methods will save which changes have been made so that they can be undone by RevertPrefChanges().</summary>
	public class PrefT {

		private static Dictionary<PrefName,string> _dictPrefsOrig=new Dictionary<PrefName, string>();

		private static void AddToDict(PrefName prefName,string newValue) {
			string oldValue=Prefs.GetPref(prefName.ToString()).ValueString;
			if(oldValue==newValue || _dictPrefsOrig.ContainsKey(prefName)) {
				return;
			}
			_dictPrefsOrig[prefName]=oldValue;
		}

		///<summary>Resets the preferences to what they were before any update methods in this class were called.</summary>
		public static void RevertPrefChanges() {
			foreach(PrefName prefName in _dictPrefsOrig.Keys) {
				Prefs.UpdateString(prefName,_dictPrefsOrig[prefName]);
			}
			_dictPrefsOrig.Clear();
		}

		///<summary>Updates a pref of type int.  Returns true if a change was required, or false if no change needed.</summary>
		public static bool UpdateInt(PrefName prefName,int newValue) {
			return UpdateLong(prefName,newValue);
		}

		///<summary>Updates a pref of type byte.  Returns true if a change was required, or false if no change needed.</summary>
		public static bool UpdateByte(PrefName prefName,byte newValue) {
			return UpdateLong(prefName,newValue);
		}

		///<summary>Updates a pref of type long.  Returns true if a change was required, or false if no change needed.</summary>
		public static bool UpdateLong(PrefName prefName,long newValue) {
			AddToDict(prefName,POut.Long(newValue));
			return Prefs.UpdateLong(prefName,newValue);
		}

		///<summary>Updates a pref of type double.  Returns true if a change was required, or false if no change needed.
		///Set doRounding false when the double passed in needs to be Multiple Precision Floating-Point Reliable (MPFR).</summary>
		public static bool UpdateDouble(PrefName prefName,double newValue,bool doRounding=true) {
			AddToDict(prefName,POut.Double(newValue));
			return Prefs.UpdateDouble(prefName,newValue,doRounding);
		}

		///<summary>Returns true if a change was required, or false if no change needed.</summary>
		public static bool UpdateBool(PrefName prefName,bool newValue) {
			AddToDict(prefName,POut.Bool(newValue));
			return Prefs.UpdateBool(prefName,newValue);
		}

		///<summary>Returns true if a change was required, or false if no change needed.</summary>
		public static bool UpdateString(PrefName prefName,string newValue) {
			AddToDict(prefName,newValue);
			return Prefs.UpdateString(prefName,newValue);
		}

		///<summary>Returns true if a change was required, or false if no change needed.</summary>
		public static bool UpdateDateT(PrefName prefName,DateTime newValue) {
			AddToDict(prefName,POut.DateT(newValue));
			return Prefs.UpdateDateT(prefName,newValue);
		}
	}
}
