using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace OpenDentBusiness {
	public class XmlConverter {
		///<summary>Chars from (char)0 through (char)0x19 (31) are the first 32 characters and are reserved control chars.
		///The exceptions are (char)0x9, a tab, (char)0xA (10), a line feed, and (char)0xD (13), a carriage return.
		///We will manually escape line feeds and carriage returns so they aren't converted to \r\n's for us, but we will leave tab chars.</summary>
		private const char _charTab=(char)0x9;
		///<summary>_charSpace (0x20=32=Space Character), which is the first non-control char that is a valid xml char.
		///The range from _charSpace (0x20=32) through _charSurr (0xD7FF=55295) inclusive are all allowed for xml.</summary>
		private const char _charSpace=(char)0x20;
		///<summary>The pipe or vertical line character (0x7C=124=Pipe ('|') Character) is a valid xml char, but we are using it to deliniate table cells
		///in order to make our tables smaller xml packages.  We will consider it an invalid char and manually escape it in strings.  That way any |'s in
		///the xml are guaranteed to be delimiters for a table that were manually placed after escaping.  We split by |'s and then unescape the cell data
		///on the receiving end when processing a table as xml.  Characters from _charSpace through _charLCurly inclusive are valid xml chars.</summary>
		private const char _charLCurly=(char)0x7B;
		///<summary>Characters from _charRCurly through _charSurr inclusive are valid xml chars.</summary>
		private const char _charRCurly=(char)0x7D;
		///<summary>Chars greater than _charSurr (0xD7FF=55295) and less than _charPrivFirst (0xE000=57344) are invalid for xml and must be either removed
		///or manually escaped/handled before xml serialization.
		///They are 0xD800-0xDB7F - High Surrogates, 0xDB80-0xDBFF - High Private Use Surrogates, and 0xDC00-0xDFFF - Low Surrogates.</summary>
		private const char _charSurr=(char)0xD7FF;
		///<summary>Chars from _charPrivFirst (0xE000=57344=Private Use, First) through _charRepl (0xFFFD=65533=Replacement Character) inclusive are
		///legal xml characters.</summary>
		private const char _charPrivFirst=(char)0xE000;
		///<summary>Chars greater than _charRepl (0xFFFD=65533=Replacement Character), explicitly 0xFFFE=65534 and 0xFFFF=65535, are invalid.
		///(Chars max out at 0xFFFF)</summary>
		private const char _charRepl=(char)0xFFFD;
		///<summary>This array is filled with 65,536 chars as strings.  The int index in the array is the int value of the char (as a string) in the array.
		///Example: _xmlEscapeStrings[97]=((char)97).ToString()="a".</summary>
		private static string[] _xmlEscapeStrings;

		public static string[] XmlEscapeStrings {
			get {
				if(_xmlEscapeStrings==null) {
					_xmlEscapeStrings=new string[0x10000];
					for(int i=0;i<0x10000;i++) {
						//Add valid characters as they are.
						if(i==_charTab || (i>=_charSpace && i<=_charLCurly) || (i>=_charRCurly && i<=_charSurr) || (i>=_charPrivFirst && i<=_charRepl)) {
							_xmlEscapeStrings[i]=((char)i).ToString();
						}
						else {//Escape invalid characters.
							_xmlEscapeStrings[i]="&#"+i+";";
						}
					}
				}
				return _xmlEscapeStrings;
			}
		}

		///<summary>Should accept any type, including simple types, OD types, Arrays, Lists, and arrays of DtoObject.  But not DataTable or DataSet.  If we find a type that isn't supported, then we need to add it.</summary>
		public static string Serialize<T>(T obj) {
			StringBuilder strBuild=new StringBuilder();
			#if DEBUG
				XmlWriterSettings settings=new XmlWriterSettings();
				settings.Indent=true;
				settings.IndentChars="   ";
				//using the constructor decreases performance and leads to memory leaks.
				//But it makes the xml much more readable
				XmlWriter writer=XmlWriter.Create(strBuild,settings);
			#else
				XmlWriter writer=XmlWriter.Create(strBuild);
			#endif
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			serializer.Serialize(writer,obj);
			writer.Close();
			return strBuild.ToString();
		}

		///<summary>For late binding of class type.</summary>
		public static string Serialize(Type classType,object obj) {
			StringBuilder strBuild=new StringBuilder();
			#if DEBUG
				XmlWriterSettings settings=new XmlWriterSettings();
				settings.Indent=true;
				settings.IndentChars="   ";
				//settings.NewLineHandling=NewLineHandling.None;//an attempt to not remove \r in strings.  Failed.
				//using the constructor decreases performance and leads to memory leaks.
				//But it makes the xml much more readable
				XmlWriter writer=XmlWriter.Create(strBuild,settings);
			#else
				XmlWriter writer=XmlWriter.Create(strBuild);
			#endif
			XmlSerializer serializer;
			if(classType==typeof(Color)) {
				serializer = new XmlSerializer(typeof(int));
				serializer.Serialize(writer,((Color)obj).ToArgb());
			}
			else if(classType==typeof(TimeSpan)) {
				serializer = new XmlSerializer(typeof(long));
				serializer.Serialize(writer,((TimeSpan)obj).Ticks);
			}
			else {
				if(obj!=null) {
					obj=XmlEscapeRecursion(classType,obj);//search object for string fields to escape
				}
				serializer = new XmlSerializer(classType);
				serializer.Serialize(writer,obj);
			}
			writer.Close();
			return strBuild.ToString();
			//the result will be fully qualified xml, including declaration.  Example:
			/*
			{<?xml version="1.0" encoding="utf-16"?>
<Userod xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
   <IsNew>false</IsNew>
   <UserNum>1</UserNum>
   <UserName>Admin</UserName>
   <Password />
   <UserGroupNum>1</UserGroupNum>
   <EmployeeNum>0</EmployeeNum>
   <ClinicNum>0</ClinicNum>
   <ProvNum>0</ProvNum>
   <IsHidden>false</IsHidden>
   <TaskListInBox>0</TaskListInBox>
   <AnesthProvType>3</AnesthProvType>
   <DefaultHidePopups>false</DefaultHidePopups>
   <PasswordIsStrong>false</PasswordIsStrong>
</Userod>}*/
		}

		///<summary>Escapes any chars for xml within the string fields and properties of an object.  If the object is a list or an array of objects,
		///recursively searches the items looking for string fields and properties to escape for xml.</summary>
		public static object XmlEscapeRecursion(Type type,object obj) {
			if(type==typeof(string)) {
				if(string.IsNullOrEmpty(obj.ToString())) {
					return obj;
				}
				obj=XmlEscape(obj.ToString());
			}
			else if(type==typeof(DataTable)) {//DataTable escaping is taken care of in TableToXml
				return obj;
			}
			else if(typeof(IEnumerable).IsAssignableFrom(type)) {//object is either a list or an array, need to escape all fields and properties of all items
				//Make a list of objects so that it is easier to iterate through the IEnumerable object.
				//We include nulls so that the counts of both IEnumerables are the same due to requirement of boxing and unboxing of strings.
				List<object> listObj=((IEnumerable)obj).Cast<object>().ToList();//.Where(x => x!=null).ToList();
				for(int i=0;i<listObj.Count;i++) {
					object objCur=listObj[i];
					if(objCur==null) {
						continue;
					}
					Type typeCur=objCur.GetType();
					if(typeCur==typeof(string)) {
						if(string.IsNullOrEmpty(objCur.ToString())) {
							continue;
						}
						//objCur=XmlEscape(objCur.ToString());
						//We cannot use objCur when dealing with strings.  Simply using the '=' operator causes a new string (different memory) to be returned.
						//If ojbCur was to be used, the IEnumerable 'obj' variable could NOT be affected (due to the memory of obj[i] being different).
						//Therefore, we have to directly manipulate 'obj' by boxing and unboxing it which will affect the same memory location of the string.
						if(type.Name=="List`1") {
							((List<string>)obj)[i]=XmlEscape(objCur.ToString());
						}
						else if(type.Name=="String[]") {
							((string[])obj)[i]=XmlEscape(objCur.ToString());
						}
						else {
							throw new Exception("Unsupported IEnumerable type passed through XmlConverter.XmlEscapeRecursion().  Passed in type was: "+type.Name);
						}
					}
					else if(typeof(IEnumerable).IsAssignableFrom(typeCur) || !typeCur.IsValueType) {
						//this list has an object or a list or an array as a member, recursively search for string fields and properties
						objCur=XmlEscapeRecursion(typeCur,objCur);
					}
				}
			}
			else if(!type.IsValueType) {
				//if the object is not a value type and is not in the System namespace (besides strings, lists, and arrays) then it must be a class object,
				//i.e. a Patient or an Appointment object, escape all string fields and properties
				FieldInfo[] fiArray=type.GetFields().Where(x => x!=null)
					.Where(x => x.GetCustomAttributes<XmlIgnoreAttribute>().Count()==0).ToArray();
				foreach(FieldInfo fi in fiArray) {
					try {
						object objCur=fi.GetValue(obj);
						if(objCur==null) {
							continue;
						}
						if(fi.FieldType==typeof(string)) {//if this is a string field, escape for xml
							if(string.IsNullOrEmpty(objCur.ToString())) {
								continue;
							}
							objCur=XmlEscape(objCur.ToString());
							fi.SetValue(obj,objCur);
						}
						else if(typeof(IEnumerable).IsAssignableFrom(fi.FieldType) || !fi.FieldType.IsValueType) {
							//if this field is an object or a list or an array of objects, recursively search for fields and properties to escape
							objCur=XmlEscapeRecursion(fi.FieldType,objCur);
							fi.SetValue(obj,objCur);
						}
					}
					catch(Exception) {
						//Something went wrong, odds are there are no invalid chars to escape.  Move on.
					}
				}
				PropertyInfo[] piArray=type.GetProperties().Where(x => x!=null)
					.Where(x => x.GetCustomAttributes<XmlIgnoreAttribute>().Count()==0).ToArray();
				foreach(PropertyInfo pi in piArray) {
					try {
						object objCur=pi.GetValue(obj,null);
						if(objCur==null) {
							continue;
						}
						if(pi.PropertyType==typeof(string)) {//if this is a string property, escape for xml
							if(string.IsNullOrEmpty(objCur.ToString())) {
								continue;
							}
							if(IsPropertyCustomSerializedXmlElement(type,pi)) {
								continue;
							}
							objCur=XmlEscape(objCur.ToString());
							pi.SetValue(obj,objCur,null);
						}
						else if(typeof(IEnumerable).IsAssignableFrom(pi.PropertyType) || !pi.PropertyType.IsValueType) {
							//if this property is an object or a list or an array of objects, recursively search for properties and fields to escape
							objCur=XmlEscapeRecursion(pi.PropertyType,objCur);
							pi.SetValue(obj,objCur,null);
						}
					}
					catch(Exception) {
						//Something went wrong, odds are there are no invalid chars to escape.  Move on.
					}
				}
			}
			return obj;
		}

		///<summary>Un-escapes any chars from xml within the string fields and properties of an object.  If the object is a list or an array of objects,
		///recursively searches the items looking for string fields and properties to un-escape from xml.</summary>
		public static object XmlUnescapeRecursion(Type type,object obj) {
			if(type==typeof(string)) {
				if(string.IsNullOrEmpty(obj.ToString())) {
					return obj;
				}
				obj=XmlUnescape(obj.ToString());
			}
			else if(type==typeof(DataTable)) {//DataTable escaping is taken care of in TableToXml
				return obj;
			}
			else if(typeof(IEnumerable).IsAssignableFrom(type)) {//object is either a list or an array, need to escape all fields and properties of all items
				//Make a list of objects so that it is easier to iterate through the IEnumerable object.
				//We include nulls so that the counts of both IEnumerables are the same due to requirement of boxing and unboxing of strings.
				List<object> listObj=((IEnumerable)obj).Cast<object>().ToList();//.Where(x => x!=null).ToList();
				for(int i=0;i<listObj.Count;i++) {
					object objCur=listObj[i];
					if(objCur==null) {
						continue;
					}
					Type typeCur=objCur.GetType();
					if(typeCur==typeof(string)) {
						if(string.IsNullOrEmpty(objCur.ToString())) {
							continue;
						}
						//objCur=XmlUnescape(objCur.ToString());
						//We cannot use objCur when dealing with strings.  Simply using the '=' operator causes a new string (different memory) to be returned.
						//If ojbCur was to be used, the IEnumerable 'obj' variable could NOT be affected (due to the memory of obj[i] being different).
						//Therefore, we have to directly manipulate 'obj' by boxing and unboxing it which will affect the same memory location of the string.
						if(type.Name=="List`1") {
							((List<string>)obj)[i]=XmlUnescape(objCur.ToString());
						}
						else if(type.Name=="String[]") {
							((string[])obj)[i]=XmlUnescape(objCur.ToString());
						}
						else {
							throw new Exception("Unsupported IEnumerable type passed through XmlConverter.XmlUnescapeRecursion().  Passed in type was: "+type.Name);
						}
					}
					else if(typeof(IEnumerable).IsAssignableFrom(typeCur) || !typeCur.IsValueType) {
						//this list has an object or a list or an array as a member, recursively search for string fields and properties
						objCur=XmlUnescapeRecursion(typeCur,objCur);
					}
				}
			}
			else if(!type.IsValueType) {
				//if the object is not a value type and is not in the System namespace (besides strings, lists, and arrays) then it must be a class object,
				//i.e. a Patient or an Appointment object, escape all string fields and properties
				FieldInfo[] fiArray=type.GetFields().Where(x => x!=null)
					.Where(x => x.GetCustomAttributes<XmlIgnoreAttribute>().Count()==0).ToArray();
				foreach(FieldInfo fi in fiArray) {
					try {
						object objCur=fi.GetValue(obj);
						if(objCur==null) {
							continue;
						}
						if(fi.FieldType==typeof(string)) {//if this is a string field, escape for xml
							if(string.IsNullOrEmpty(objCur.ToString())) {
								continue;
							}
							objCur=XmlUnescape(objCur.ToString());
							fi.SetValue(obj,objCur);
						}
						else if(typeof(IEnumerable).IsAssignableFrom(fi.FieldType) || !fi.FieldType.IsValueType) {
							//if this field is an object or a list or an array of objects, recursively search for fields and properties to escape
							objCur=XmlUnescapeRecursion(fi.FieldType,objCur);
							fi.SetValue(obj,objCur);
						}
					}
					catch(Exception) {
						//Something went wrong, odds are there are no invalid chars to escape.  Move on.
					}
				}
				PropertyInfo[] piArray=type.GetProperties().Where(x => x!=null)
					.Where(x => x.GetCustomAttributes<XmlIgnoreAttribute>().Count()==0).ToArray();
				foreach(PropertyInfo pi in piArray) {
					try {
						object objCur=pi.GetValue(obj,null);
						if(objCur==null) {
							continue;
						}
						if(pi.PropertyType==typeof(string)) {//if this is a string property, escape for xml
							if(string.IsNullOrEmpty(objCur.ToString())) {
								continue;
							}
							if(IsPropertyCustomSerializedXmlElement(type,pi)) {
								continue;
							}
							objCur=XmlUnescape(objCur.ToString());
							pi.SetValue(obj,objCur,null);
						}
						else if(typeof(IEnumerable).IsAssignableFrom(pi.PropertyType) || !pi.PropertyType.IsValueType) {
							//if this property is an object or a list or an array of objects, recursively search for properties and fields to escape
							objCur=XmlUnescapeRecursion(pi.PropertyType,objCur);
							pi.SetValue(obj,objCur,null);
						}
					}
					catch(Exception) {
						//Something went wrong, odds are there are no invalid chars to escape.  Move on.
					}
				}
			}
			return obj;
		}

		///<summary>Should accept any type.  Tested types include System types, OD types, Arrays, Lists, arrays of DtoObject, null DataObjectBase, null arrays, null Lists.  But not DataTable or DataSet.  If we find a type that isn't supported, then we need to add it.  Types that are currently unsupported include Arrays of DataObjectBase that contain a null.  Lists that contain nulls are untested and may be an issue for DataObjectBase.</summary>
		public static T Deserialize<T>(string xmlData) {
			Type type = typeof(T);
			/*later.  I don't think arrays will null objects will be an issue.
			if(type.IsArray) {
				Type arrayType=type.GetElementType();
				if(arrayType.BaseType==typeof(DataObjectBase)) {
					//split into items
				}
			}*/
			if(type.IsGenericType) {//List<>
				//because the built-in deserializer does not handle null list<>, but instead returns an empty list.
				//<ArrayOfDocument xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xsi:nil="true" />
				if(Regex.IsMatch(xmlData,"<ArrayOf[^>]*xsi:nil=\"true\"")) {
					return default(T);//null
				}
			}
			StringReader strReader=new StringReader(xmlData);
			//XmlReader reader=XmlReader.Create(strReader);
			XmlTextReader reader=new XmlTextReader(strReader);
			XmlSerializer serializer;
			T retVal;
			if(type==typeof(Color)) {
				serializer = new XmlSerializer(typeof(int));
				retVal=(T)((object)Color.FromArgb((int)serializer.Deserialize(reader)));
			}
			else if(type==typeof(TimeSpan)) {
				serializer = new XmlSerializer(typeof(long));
				retVal=(T)((object)TimeSpan.FromTicks((long)serializer.Deserialize(reader)));
			}
			else if(type.IsInterface) {
				//For methods that return an interface, we serialize the return object as a DtoObject.
				serializer=new XmlSerializer(typeof(DtoObject));
				retVal=(T)((DtoObject)serializer.Deserialize(reader)).Obj;
			}
			else {
				serializer = new XmlSerializer(type);
				retVal=(T)serializer.Deserialize(reader);
				if(retVal!=null) {
					retVal=(T)XmlUnescapeRecursion(type,retVal);
				}
			}
			strReader.Close();
			reader.Close();
			return retVal;
		}

		///<summary>Serializes a DataTable by looping through the rows and columns.</summary>
		public static string TableToXml(DataTable table) {
			StringBuilder result=new StringBuilder();
			result.Append("<DataTable>");
			//Table name.
			result.Append("<Name>").Append(XmlEscape(table.TableName)).Append("</Name>");
			//Column names.
			result.Append("<Cols>");
			foreach(DataColumn col in table.Columns) {
				//result.Append("<Col>").Append(EscapeForXml(table.Columns[i].ColumnName)).Append("</Col>");
				//We escape column names just in case.
				result.Append("<Col");
				if(col.DataType==typeof(decimal)) {
					result.Append(" DataType=\"decimal\"");
				}
				else if(col.DataType==typeof(DateTime)) {
					result.Append(" DataType=\"DateTime\"");
				}
				result.Append(">");
				result.Append(XmlEscape(col.ColumnName));
				result.Append("</Col>");
			}
			result.Append("</Cols>");
			result.Append("<Cells>");
			List<string> listCells;
			//old way: <x>cell0</x><x>cell1</x><x>cellwith|pipe</x>  new way: cell0|cell1|cellwith&#124;pipe
			//Strategy: When serializing, convert | to &#124;.  When deserializing, split by |, then convert &#124; back to |.
			//Set each cell by looping through each column row by row.
			foreach(DataRow row in table.Rows) {
				listCells=new List<string>();
				foreach(DataColumn col in table.Columns) {
					listCells.Add(XmlEscape(row[col].ToString()));
				}
				result.Append("<y>"+string.Join("|",listCells)+"</y>");
			}
			result.Append("</Cells>");
			result.Append("</DataTable>");
			return result.ToString();
		}

		public static string DsToXml(DataSet ds) {
			StringBuilder strb=new StringBuilder();
			strb.Append("<DataSet>");
			strb.Append("<DataTables>");
			for(int i=0;i<ds.Tables.Count;i++) {
				strb.Append(TableToXml(ds.Tables[i]));
			}
			strb.Append("</DataTables>");
			strb.Append("</DataSet>");
			return strb.ToString();
		}

		/// <summary></summary>
		public static DataTable XmlToTable(string xmlData) {
			DataTable table=new DataTable();
			XmlDocument doc=new XmlDocument();
			doc.LoadXml(xmlData);
			//<DataTable><Name></Name><Cols><Col>cell00</Col></Cols><Cells><y><x>cell00</x></y></Cells></DataTable>
			XmlNode nodeName=doc.SelectSingleNode("//Name");
			table.TableName=XmlStringUnescape(nodeName.InnerText);
			XmlNode nodeCols=doc.SelectSingleNode("//Cols");
			foreach(XmlNode childNode in nodeCols.ChildNodes) {
				DataColumn col=new DataColumn(XmlStringUnescape(childNode.InnerText));
				if(childNode.Attributes.Count>0) {//if attribute is set for column
					string dataType=XmlStringUnescape(childNode.Attributes["DataType"].InnerText);//this is safe because we created the xml
					if(dataType=="decimal") {
						col.DataType=typeof(decimal);
					}
					else if(dataType=="DateTime") {
						col.DataType=typeof(DateTime);
					}
				}
				table.Columns.Add(col);
			}
			XmlNodeList nodeListY=doc.SelectSingleNode("//Cells").ChildNodes;
			foreach(XmlNode node in nodeListY) {//loop y rows
				DataRow row=table.NewRow();
				List<string> listCols=node.ChildNodes[0].InnerText.Split('|').ToList();//Break up the XML node which is columns delimited by pipes.
				for(int i=0;i<listCols.Count;i++) {
					string colUnescaped=XmlStringUnescape(listCols[i]);
					//Check the type of the current column to make sure we do not try and set a DateTime column to empty string (throws exception).
					if(table.Columns[i].DataType==typeof(DateTime) && string.IsNullOrEmpty(colUnescaped)) {
						colUnescaped=PIn.DateT(colUnescaped).ToString();//PIn.DateT handles empty strings and turns them into DateTime.MinValue
					}
					row[i]=colUnescaped;
				}
				table.Rows.Add(row);
			}
			return table;
		}
		
		public static DataSet XmlToDs(string xmlData) {
			DataSet ds=new DataSet();
			XmlDocument doc=new XmlDocument();
			doc.LoadXml(xmlData);
			//<DataSet><DataTables><DataTable><Name>table0</Name><Cols><Col>cell00</Col></Cols><Cells><y><x>cell00</x></y></Cells></DataTable></DataTables></DataSet>
			XmlNode nodeTables=doc.SelectSingleNode("//DataTables");
			for(int t=0;t<nodeTables.ChildNodes.Count;t++) {
				ds.Tables.Add(XmlToTable(nodeTables.ChildNodes[t].OuterXml));//<DataTable>....</DataTable>
			}
			return ds;
		}

		///<summary>Pass in the object type of the class that contains an element that we handle not using the typical method.(DataTable or DataSet)
		///Also, provide the property that will be checked to see if it is of type string and if it is meant to replace the element type.</summary>
		private static bool IsPropertyCustomSerializedXmlElement(Type type,PropertyInfo propertyInfo) {
			if(propertyInfo.PropertyType!=typeof(string)) {
				return false;
			}
			XmlElementAttribute attribute = propertyInfo.GetCustomAttribute<XmlElementAttribute>();
			//We have already escaped the string inside of the TableToXML or DSToXML method for property that are helping serialize a datatable.
			//If this property has an XML element name the same as a datatable or dataset field, we can skip it.
			if(attribute!=null && type.GetFields().Where(x => x!=null)
				.Where(x => x.FieldType==typeof(DataTable) || x.FieldType==typeof(DataSet))
				.Where(x => x.GetCustomAttributes<XmlIgnoreAttribute>().Count()>0)
				.Any(x => x.Name==attribute.ElementName)) 
			{
				return true;
			}
			return false;
		}

		///<summary>Escapes common characters used in XML from myString.  Also escapes any characters that are invalid for use in XML with an escape sequence of the pattern "&amp;#"+(int)char+";" (ampersand+hash+the unicode int (not hex) value of the char+semicolon)</summary>
		public static string XmlEscape(string myString) {
			if(string.IsNullOrEmpty(myString)) {
				return myString;
			}
			StringBuilder sbEscaped=new StringBuilder();
			foreach(char c in myString) {
				//XmlEscapeStrings contains replacement sequences for any invalid characters including |'s, which we use to delineate table cells,
				//and \r's and \n's so they are not converted to \r\n's thereby invalidating signatures
				sbEscaped.Append(XmlEscapeStrings[c]);
			}
			XmlDocument doc=new XmlDocument();
			XmlNode node=doc.CreateElement("root");
			node.InnerText=sbEscaped.ToString();
			myString=node.InnerXml;
			return myString;
		}

		public static string XmlUnescape(string myString) {
			if(string.IsNullOrEmpty(myString)) {
				return myString;
			}
			XmlDocument doc=new XmlDocument();
			XmlNode node=doc.CreateElement("root");
			node.InnerXml=myString;
			myString=node.InnerText;
			myString=XmlStringUnescape(myString);
			return myString;
		}

		public static string XmlStringUnescape(string myString) {
			if(string.IsNullOrEmpty(myString)) {
				return myString;
			}
			//Loop through every character possible.  
			for(int i=0;i<XmlEscapeStrings.Length;i++) {
				//Check once every 255 characters to see if we've completely cleaned myString.
				if(i%255==0 && !Regex.IsMatch(myString,@"&#[0-9]+;")) {
					break;//There is nothing else to clean so stop looking.
				}
				if(i==(int)_charTab
					|| (i>=(int)_charSpace && i<=(int)_charLCurly)
					|| (i>=(int)_charRCurly && i<=(int)_charSurr)
					|| (i>=(int)_charPrivFirst && i<=(int)_charRepl))
				{
					continue;
				}
				myString=myString.Replace(XmlEscapeStrings[i],((char)i).ToString());
			}
			return myString;
		}


	}
}
