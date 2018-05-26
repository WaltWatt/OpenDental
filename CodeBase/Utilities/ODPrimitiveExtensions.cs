using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using CodeBase;

namespace OpenDentBusiness {
	public static class ODPrimitiveExtensions {
		///<summary>Used to check if a floating point number is "equal" to zero based on some epsilon. 
		/// Epsilon is 0.0000001f and will return true if the absolute value of the double is less than that.</summary>
		public static bool IsZero(this double val) {
			return Math.Abs(val)<=0.0000001f;
		}

		///<summary>Used to check if a floating point number is "equal" to zero based on some epsilon. 
		/// Epsilon is 0.0000001f and will return true if the absolute value of the double is less than that.</summary>
		public static bool IsZero(this float val) {
			return Math.Abs(val)<=0.0000001f;
		}

		public static bool IsEqual(this float val,float val2) {
			return IsZero(val-val2);
		}

		public static bool IsEqual(this double val,double val2) {
			return IsZero(val-val2);
		}

		///<summary>Used to check if a double is "less than" another double based on some epsilon.</summary>
		public static bool IsLessThan(this double val,double val2) {
			return val2-val > 0.0000001f;
		}

		///<summary>Used to check if a double is "greater than" another double based on some epsilon.</summary>
		public static bool IsGreaterThan(this double val,double val2) {
			return val-val2 > 0.0000001f;
		}

		///<summary>Provide custom comparison for a given type (T). Returns true if 2 items are equal, otherwise returns false.</summary>
		public static bool IsEqual<T>(this T val,T val2,Func<T,T,bool> funcCompare) {
			return funcCompare(val,val2);
		}
		
		///<summary>Used to check if a decimal number is "less than" zero based on some epsilon.</summary>
		public static bool IsLessThanZero(this decimal val) {
			return val < -0.0000001M;
		}

		///<summary>Used to check if a decimal number is "greater than" zero based on some epsilon.</summary>
		public static bool IsGreaterThanZero(this decimal val) {
			return val > 0.0000001M;
		}

		public static string Left(this string s,int maxCharacters,bool hasElipsis=false) {
			if(s==null || string.IsNullOrEmpty(s) || maxCharacters<1) {
				return "";
			}
			if(s.Length>maxCharacters) {
				if(hasElipsis && maxCharacters>4) {
					return s.Substring(0,maxCharacters-3)+"...";
				}
				return s.Substring(0,maxCharacters);
			}
			return s;
		}

		public static string Right(this string s,int maxCharacters) {
			if(s==null || string.IsNullOrEmpty(s) || maxCharacters<1) {
				return "";
			}
			if(s.Length>maxCharacters) {
				return s.Substring(s.Length-maxCharacters,maxCharacters);
			}
			return s;
		}

		///<summary>Returns the Description attribute if available. If not, returns enum.ToString().</summary>
		public static string GetDescription(this Enum value,bool useShortVersionIfAvailable = false) {
			Type type = value.GetType();
			string name = Enum.GetName(type,value);
			if(name==null) {
				return value.ToString();
			}
			FieldInfo field = type.GetField(name);
			if(field==null) {
				return value.ToString();
			}
			if(useShortVersionIfAvailable) {
				ShortDescriptionAttribute attrShort=(ShortDescriptionAttribute)Attribute.GetCustomAttribute(field,typeof(ShortDescriptionAttribute));
				if(attrShort!=null) {
					return attrShort.ShortDesc;
				}
			}
			DescriptionAttribute attr=(DescriptionAttribute)Attribute.GetCustomAttribute(field,typeof(DescriptionAttribute));
			if(attr==null) {
				return value.ToString();
			}
			return attr.Description;
		}

		///<summary>Returns the attribute for the enum value if available. If not, returns the default value for the attribute.</summary>
		public static T GetAttributeOrDefault<T>(this Enum value) where T:Attribute,new() {
			Type type=value.GetType();
			string name=Enum.GetName(type,value);
			if(name==null) {
				return new T();
			}
			FieldInfo field=type.GetField(name);
			if(field==null) {
				return new T();
			}
			T attr=Attribute.GetCustomAttribute(field,typeof(T)) as T;
			if(attr==null) {
				return new T();
			}
			return attr;
		}

		///<summary>Returns true if the enum value matches any of the flags passed in.</summary>
		public static bool HasAnyFlag(this Enum value,params Enum[] flags) {
			long valLong=Convert.ToInt64(value);
			if(valLong==0) {
				return flags.Contains(value);
			}
			return flags.Any(x => (valLong & Convert.ToInt64(x)) > 0);
		}

		public static string ToStringDHM(this TimeSpan ts) {
			return string.Format("{0:%d} Days {0:%h} Hours {0:%m} Minutes",ts);
		}

		public static string ToStringDH(this TimeSpan ts) {
			return string.Format("{0:%d} Days {0:%h} Hours",ts);
		}

		public static DateTime ToBeginningOfMinute(this DateTime dateT) {
			return new DateTime(dateT.Year,dateT.Month,dateT.Day,dateT.Hour,dateT.Minute,0,dateT.Kind);
		}

		public static DateTime ToBeginningOfMonth(this DateTime dateT) {
			return new DateTime(dateT.Year,dateT.Month,1,0,0,0,dateT.Kind);
		}

		public static DateTime ToEndOfMinute(this DateTime dateT) {
			return new DateTime(dateT.Year,dateT.Month,dateT.Day,dateT.Hour,dateT.Minute,59,dateT.Kind).AddMilliseconds(999);
		}

		///<summary>Use regular expressions to do an in-situ string replacement. Default behavior is case insensitive.</summary>
		/// <param name="pattern">Must be a REGEX compatible pattern.</param>
		/// <param name="replacement">The string that should be used to replace each occurance of the pattern.</param>
		/// <param name="regexOptions">IgnoreCase by default, allows others.</param>
		public static void RegReplace(this StringBuilder value, string pattern, string replacement,RegexOptions regexOptions=RegexOptions.IgnoreCase) {
			string newVal=Regex.Replace(value.ToString(),pattern,replacement,regexOptions);
			value.Clear();
			value.Append(newVal);
		}

		///<summary>Convert the first char in the string to upper case. The rest of the string will be lower case.</summary>
		public static string ToUpperFirstOnly(this string value) {
			if(string.IsNullOrEmpty(value)) {
				return value;
			}
			if(value.Length==1) {
				return value.ToUpper();
			}
			return value.Substring(0,1).ToUpper()+value.Substring(1,value.Length-1).ToLower();
		}

		///<summary>Removes all characters from the string that are not digits.</summary>
		public static string StripNonDigits(this string value) {
			if(string.IsNullOrEmpty(value)) {
				return value;
			}
			return new string(Array.FindAll(value.ToCharArray(),y => char.IsDigit(y)));
		}

		///<summary>Adds a new line if the string is not empty and appends the addition.</summary>
		public static string AppendLine(this string value,string addition) {
			if(value!="") {
				value+="\r\n";
			}
			return value+addition;
		}

		///<summary>Returns everything in the string after the "beforeThis" string. Throws if "beforeThis" is not present in the string.</summary>
		///<exception cref="IndexOutOfRangeException" />
		public static string SubstringBefore(this string value,string beforeThis) {
			return value.Substring(0,value.IndexOf(beforeThis));
		}

		///<summary>Returns everything in the string after the "afterThis" string. Throws if "afterThis" is not present in the string.</summary>
		///<exception cref="IndexOutOfRangeException" />
		public static string SubstringAfter(this string value,string afterThis) {
			return value.Substring(value.IndexOf(afterThis)+afterThis.Length);
		}

		///<summary>Compares the current dictionary to the dictionary passed in. 
		///Requires the implementer to provide a func that will handle the comparison of individual "items" (within the TValue object).
		///Throws if all keys and values match, otherwise returns silently.</summary>
		public static void CompareDictionary<TKey, TValue>(this IDictionary<TKey,TValue> dictOrig,IDictionary<TKey,TValue> dictFinal,Func<TValue,TValue,bool> funcCompare) {
			if(dictOrig.Count!=dictFinal.Count) {
				throw new Exception("Row sizes to do not match!");
			}
			if(dictOrig.Keys.Any(x => !dictFinal.ContainsKey(x))) {
				throw new Exception("Original key not found in final keys!");
			}
			if(dictFinal.Keys.Any(x => !dictOrig.ContainsKey(x))) {
				throw new Exception("Final key not found in original keys!");
			}
			bool valueMatch=dictOrig.All(kvpOrig => {
				TValue valFinal;
				if(!dictFinal.TryGetValue(kvpOrig.Key,out valFinal)) { //Should not get here, we already checked keys above.
					return false; 
				}
				//Compare values.
				return funcCompare(kvpOrig.Value,valFinal);
			});
			if(!valueMatch) {
				throw new Exception("Value mismatch in original and final");
			}
		}

		///<summary>Compares the current dictionary to the dictionary passed in. Returns true if all keys and values match, otherwise returns false.
		///Requires the implementer to provide a func that will handle the comparison of individual "items" (within the TValue object).
		///Returns true if dictionaries are the same, otherwise returns false.</summary>
		public static bool TryCompareDictionary<TKey, TValue>(this IDictionary<TKey,TValue> dictOrig,IDictionary<TKey,TValue> dictFinal,Func<TValue,TValue,bool> funcCompare) {
			try {
				dictOrig.CompareDictionary(dictFinal,funcCompare);
				return true;
			}
			catch(Exception e) {
				e.DoNothing();
			}
			return false;
		}

		///<summary>Compares 2 lists. Returns true if size and items are exactly the same in each list. 
		///Uses .Equals() of the given TSource type to compare so beware if comparing anything but primitives.
		///Throws if all list items match, otherwise returns silently.
		///<see cref="http://stackoverflow.com/a/5620298"/></summary>
		///<typeparam name="TSource">The type being compared. Will use .Equals() to compare, of funcCompare if provided.</typeparam>
		public static void CompareList<TSource>(this IEnumerable<TSource> first,IEnumerable<TSource> second,Func<TSource,TSource,bool> funcCompare = null) {
			if(first.Count()!=second.Count()) { //In case there are duplicates in either list.
				throw new Exception("Item count mismatch");
			}
			//No duplicates so any non-intersecting items indicates not a match.
			if(funcCompare==null) { //Use the default comparison func (usually for primitives only).
				if(first.Except(second).Union(second.Except(first)).Count()==0) {
					return; //Match.
				}
			}
			else { //Use the custom comparison func.
				ODEqualityComparer<TSource> compare=new ODEqualityComparer<TSource>(funcCompare);
				if(first.Except(second,compare).Union(second.Except(first,compare)).Count()==0) {
					return; //Match.
				}
			}
			throw new Exception("Items do not match");
		}

		///<summary>Deep copy each list items of the source list to a new list instance of the target return type. Property values will be shallow-copied if specified.</summary>
		public static List<TTarget> DeepCopyList<TSource, TTarget>(this List<TSource> source,bool shallowCopyProperties=true) where TTarget : TSource {
			PropertyInfo[] properties=typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
			FieldInfo[] fields=typeof(TSource).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
			List<TTarget> ret=new List<TTarget>();
			foreach(TSource sourceObj in source) {
				TTarget targetObj=(TTarget)Activator.CreateInstance(typeof(TTarget));
				if(shallowCopyProperties) {
					foreach(PropertyInfo property in properties) {
						try {
							property.SetValue(sourceObj,property.GetValue(sourceObj,null),null);
						}
						catch(Exception e) { //For Get-only-properties.
							e.DoNothing();
						} 
					}
				}
				foreach(FieldInfo field in fields) {
					field.SetValue(targetObj,field.GetValue(sourceObj));
				}
				ret.Add(targetObj);
			}
			return ret;
		}

		///<summary>Compares 2 lists. Returns true if size and items are exactly the same in each list. 
		///Uses .Equals() of the given TSource type to compare so beware if comparing anything but primitives.
		///Returns true if lists are the same, otherwise returns false.
		///<see cref="http://stackoverflow.com/a/5620298"/></summary>
		///<typeparam name="TSource">The type being compared. Will use .Equals() to compare.</typeparam>
		public static bool TryCompareList<TSource>(this IEnumerable<TSource> first,IEnumerable<TSource> second,Func<TSource,TSource,bool> funcCompare=null) {
			try {
				first.CompareList(second,funcCompare);
				return true;
			}
			catch(Exception e) {
				e.DoNothing();
			}
			return false;
		}

		///<summary>Allows for custom comparison of TSource. Implements IEqualityComparer, which is required by LINQ for inline comparisons.</summary>
		public class ODEqualityComparer<TSource>:IEqualityComparer<TSource> {
			private Func<TSource,TSource,bool> _funcCompare;

			public ODEqualityComparer(Func<TSource,TSource,bool> funcCompare) {
				this._funcCompare=funcCompare;
			}

			public bool Equals(TSource x,TSource y) {
				return _funcCompare(x,y);
			}

			public int GetHashCode(TSource obj) {				
				//Do not use obj.GetHashCode(). This will return a non-determinant value and cause .Equals() to be skipped in most cases.
				//Always return the same value (0 is acceptable). This will defer to the Equals override as the tie-breaker, which is what we want in this case.
				return 0;
			}
		}
	}

	public class ShortDescriptionAttribute:Attribute {
		public ShortDescriptionAttribute() : this("") {

		}
		public ShortDescriptionAttribute(string shortDesc) {
			ShortDesc=shortDesc;
		}

		private string _shortDesc="";
		public string ShortDesc {
			get { return _shortDesc; }
			set { _shortDesc=value; }
		}
	}
}
