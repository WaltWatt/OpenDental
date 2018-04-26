using System;
using System.Collections;
using System.Drawing;
//using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Globalization;
using System.ComponentModel;
//using System.Windows.Forms;

namespace OpenDentBusiness{
	
	/*=========================================================================================
	=================================== class PIn ===========================================*/
	///<summary>"P" stands for Parameter.  Converts strings coming in from user input into the appropriate type.</summary>
	public class PIn{
		///<summary></summary>
		public static Bitmap Bitmap(string myString) {
			if(myString==null || myString.Length<0x32) {//Bitmaps require a minimum length for header info.
				return null;
			}
			try {
				byte[] rawData=Convert.FromBase64String(myString);
				MemoryStream stream=new MemoryStream(rawData);
				System.Drawing.Bitmap image=new System.Drawing.Bitmap(stream);
				return image;
			}
			catch {
				return null;
			}
		}

		///<summary></summary>
		public static bool Bool (string myString){
			if(myString=="" || myString=="0") {
				return false;
			}
			return true;//js enhanced this to allow larger numbers to indicate bool true.  Specifically TaskUnreads
			//return myString=="1";
		}

		///<summary>Set has exceptions to false to supress exceptions and return 0 if the input string is not an byte.</summary>
		public static byte Byte (string myString, bool hasExceptions = true){
			if(myString==""){
				return 0;
			}
			else{
				try {
					return System.Convert.ToByte(myString);
				}
				catch(Exception ex) {
					if(hasExceptions) {
						throw ex;
					}
					return 0;
				}
			}
		}

		///<summary>Some versions of MySQL return a GROUP_CONCAT as a string, and other versions return it as a byte array.  This method handles either way, making it work smoothly with different versions.</summary>
		public static string ByteArray(object obj) {
			if(obj.GetType()==typeof(Byte[])) {
				Byte[] bytes=(Byte[])obj;
				StringBuilder strbuild=new StringBuilder();
				for(int i=0;i<bytes.Length;i++) {
					strbuild.Append((char)bytes[i]);
				}
				return strbuild.ToString();
			}
			else {//string
				return obj.ToString();
			}
		}

		///<summary>Processes dates incoming from db that look like "4/29/2013", and dates from textboxes where users entered and which have usually been validated.</summary>
		public static DateTime Date(string myString){
			if(myString=="" || myString==null) {
				return DateTime.MinValue;
			}
			try{
				return (DateTime.Parse(myString));//DateTimeKind.Unspecified, which prevents -7:00, for example, from being tacked onto the end during serialization.
				//return DateTime.Parse(myString,CultureInfo.InvariantCulture);
			}
			catch{
				return DateTime.MinValue;
			}
		}

		///<summary></summary>
		public static DateTime DateT(string myString){
			if(myString=="")
				return DateTime.MinValue;
			//if(myString=="0000-00-00 00:00:00")//useless
			//	return DateTime.MinValue;
			try{
				return (DateTime.Parse(myString));
			}
			catch{
				return DateTime.MinValue;
			}
		}
		
		///<summary>If blank or invalid, returns 0. Otherwise, parses.</summary>
		public static decimal Decimal (string myString){
			if(myString==""){
				return 0;
			}
			else{
				try{
					return System.Convert.ToDecimal(myString);
				}
				catch{
					//MessageBox.Show("Error converting "+myString+" to decimal");
					return 0;
				}
			}
		}

		///<summary>If blank or invalid, returns 0. Otherwise, parses.</summary>
		///<param name="doUseEnUSFormat">If false, will use the computer's current settings to parse. If true, will use the en-US setting where a "."
		///separates the decimal portion and a "," separates groups.</param>
		public static double Double(string myString,bool doUseEnUSFormat=false) {
			if(myString=="") {
				return 0;
			}
			else {
				try {
					if(doUseEnUSFormat) {
						NumberFormatInfo format=new NumberFormatInfo();
						format.NumberDecimalSeparator=".";
						format.NumberGroupSeparator=",";
						return System.Convert.ToDouble(myString,format);
					}
					return System.Convert.ToDouble(myString);//In Europe, comes in as a comma, parsed according to culture.
				}
				catch {
					//MessageBox.Show("Error converting "+myString+" to double");
					return 0;
				}
			}
		}

		///<summary>Set has exceptions to false to supress exceptions and return 0 if the input string is not an int.</summary>
		public static int Int(string myString,bool hasExceptions=true) {
			if(myString=="") {
				return 0;
			}
			else {
				try {
					return System.Convert.ToInt32(myString);
				}
				catch(Exception ex) {
					if(hasExceptions) {
						throw ex;
					}
					return 0;
				}
			}
		}

		///<summary></summary>
		public static float Float(string myString) {
			if(myString=="") {
				return 0;
			}
			try{
				return System.Convert.ToSingle(myString);
			}
			catch {//because this will fail when getting the mysql version on startup, which always comes back with a period.
				return System.Convert.ToSingle(myString,CultureInfo.InvariantCulture);
			}
		}

		///<summary>Set has exceptions to false to supress exceptions and return 0 if the input string is not a long.</summary>
		public static long Long (string myString,bool hasExceptions=true){
			if(myString==""){
				return 0;
			}
			else{
				try {
				return System.Convert.ToInt64(myString);
				}
				catch(Exception ex) {
					if(hasExceptions) {
						throw ex;
					}
					return 0;
				}
			}
		}

		///<summary></summary>
		public static short Short(string myString) {
			if(myString == "") {
				return 0;
			}
			else {
				return System.Convert.ToInt16(myString);
			}
		}

		///<summary>Strongly types the value provided to the enumeration value of declared enum type (T).
		///When isEnumAsString is false, value should be the integer value of the desired enum item.  E.g. T = ApptStatus, value = "5", retVal = ApptStatus.Broken
		///When isEnumAsString is true, value must be the enum item name.  E.g. T = ProgramName, value = programCur.ProgName, retVal = ProgramName.Podium
		///By default, defaultEnumOption will give you the Enum option containing a value of 0 (either the first in the set, or the first one explicitly set to 0)
		///To default a non-0 Enum option, set defaultEnumOption accordingly.
		///This will mainly get called to circumvent double casting.  E.g. ApptStatus stat=(ApptStatus)PIn(table["ApptStatus"].ToString());</summary>
		public static T Enum<T>(string value,bool isEnumAsString = false,T defaultEnumOption = default(T)) where T : struct, IConvertible {
			if(!typeof(T).IsEnum) {
				throw new Exception("T must be an enumeration.");
			}
			T retVal=defaultEnumOption;
			if(isEnumAsString) {
				System.Enum.TryParse(value,out retVal);
			}
			else {
				int valueAsInt=Int(value);//value should be an integer at this point.
				//retVal=(T)(object)valueAsInt;//Double casting in this manner causes an issue when trying to parse a value that doesn't exist in the given T enum.
				string optionName=System.Enum.GetName(typeof(T),valueAsInt)?.ToString();
				if(!string.IsNullOrWhiteSpace(optionName)) {
					System.Enum.TryParse(optionName,out retVal);
				}
			}
			return retVal;
		}


		///<summary>Strongly types the integer provided to the enumeration value of the declared enum type (T).</summary>
		public static T Enum<T>(int value) where T : struct, IConvertible {
			return Enum<T>(value.ToString());
		}

		///<summary>Saves the string representation of a sound into a .wav file.  The timing of this is different than with the other "P" functions, and is only used by the export button in FormSigElementDefEdit</summary>
		public static void Sound(string sound,string filename) {
			if(!filename.ToLower().EndsWith(".wav")) {
				throw new ApplicationException("Filename must end with .wav");
			}
			byte[] rawData=Convert.FromBase64String(sound);
			FileStream stream=new FileStream(filename,FileMode.Create,FileAccess.Write);
			stream.Write(rawData,0,rawData.Length);
			stream.Close();
		}
		
		///<summary>Currently does nothing.</summary>
		public static string String (string myString){
			return myString;
		}

		///<summary>Timespans that might be invalid time of day.  Can be + or - and can be up to 800+ hours.  Stored in Oracle as varchar2.</summary>
		public static TimeSpan TSpan(string myString) {
			if(string.IsNullOrEmpty(myString)) {
				return System.TimeSpan.Zero;
			}
			try {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					//return System.TimeSpan.Parse(myString); //Does not work. Confuses hours with days and an exception is thrown in our large timespan test.
					bool isNegative=false;
					if(myString.StartsWith("-")) {
						isNegative=true;
						myString=myString.Substring(1);//remove the '-'
					}
					string[] timeValues=myString.Split(new char[] { ':' });
					if(timeValues.Length!=3) {
						return System.TimeSpan.Zero;
					}
					TimeSpan retval=new TimeSpan(PIn.Int(timeValues[0]),PIn.Int(timeValues[1]),PIn.Int(timeValues[2]));
					if(isNegative) {
						return retval.Negate();
					}
					return retval;
				}
				else {//mysql
					return (System.TimeSpan.Parse(myString));
				}
			}
			catch {
				return System.TimeSpan.Zero;
			}
		}

		///<summary>Used for Timespans that are guaranteed to always be a valid time of day.  No negatives or hours over 24.  Stored in Oracle as datetime.</summary>
		public static TimeSpan Time(string myString) {
			if(string.IsNullOrEmpty(myString)) {
				return System.TimeSpan.Zero;
			}
			try {
				if(DataConnection.DBtype==DatabaseType.Oracle) {
					return DateTime.Parse(myString).TimeOfDay;
				}
				else {//mysql
					return (System.TimeSpan.Parse(myString));
				}
			}
			catch {
				return System.TimeSpan.Zero;
			}
		}
		
	
		

		


	}

	


}










