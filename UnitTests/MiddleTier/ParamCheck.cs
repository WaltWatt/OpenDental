using CodeBase;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UnitTests {
	///<summary>This class will use reflection to indicate what methods do not have the correct parameters within their RemotingRole check.</summary>
	public class ParamCheck {

		public static string ParamCheckSClasses() {
			StringBuilder retVal=new StringBuilder();
			string[] arraySClassPaths=Directory.GetFiles(@"C:\Development\OPEN DENTAL SUBVERSION\head\OpenDentBusiness\Data Interface");
			//Get just the names of the classes without the path and extension information.
			List<string> listSClassNames=arraySClassPaths.Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
			Assembly ass=Assembly.LoadFrom(@"C:\Development\OPEN DENTAL SUBVERSION\head\UnitTests\bin\Debug\OpenDentBusiness.dll");
			List<Type> listSClassTypes=ass.GetTypes().Where(x => x.IsClass && x.IsPublic && listSClassNames.Contains(x.Name))
				.OrderBy(x => x.Name)
				.ToList();
			//Loop through all classes and call every public static method at least once.
			//There will be a log created for all methods that end up calling Meth.Dto... which are going to be the only methods we really care about.
			foreach(Type sClass in listSClassTypes) {
				MethodInfo[] methods=sClass.GetMethods(BindingFlags.Static | BindingFlags.Public);
				foreach(MethodInfo method in methods) {
					if(method.IsGenericMethod || method.ContainsGenericParameters || method.IsAbstract || method.IsConstructor || method.IsFinal 
						|| method.IsVirtual) 
					{
						continue;
					}
					if(method.Name.StartsWith("get_") || method.Name.StartsWith("set_")) {
						continue;
					}
					bool hasOutOrRefParam=false;
					ParameterInfo[] parameters=method.GetParameters();
					object[] arrayObjs=new object[parameters.Length];
					for(int i=0;i<parameters.Length;i++) {
						try {
							Type parameterType=parameters[i].ParameterType;
							if(parameterType.IsByRef) {
								hasOutOrRefParam=true;
								break;
							}
							if(parameterType==typeof(string)) {
								arrayObjs[i]="";
							}
							else if(parameterType.IsArray) {
								arrayObjs[i]=Array.CreateInstance(parameterType.GetElementType(),0);
							}
							else if(parameterType==typeof(DataRow)) {
								arrayObjs[i]=new DataTable().NewRow();
							}
							else if(parameterType==typeof(DateTime)) {
								arrayObjs[i]=DateTime.Now;
							}
							else if(parameterType==typeof(Version)) {
								arrayObjs[i]=new Version(1,1,1,1);//Versions cannot be 0.0 for JSON
							}
							else if(parameterType==typeof(CultureInfo)) {
								arrayObjs[i]=CultureInfo.CurrentCulture;
							}
							else if(parameterType==typeof(CodeSystems.ProgressArgs)) {
								arrayObjs[i]=new CodeSystems.ProgressArgs((Action<int,int>)delegate(int a,int b) { });
							}
							else if(parameterType==typeof(Logger.WriteLineDelegate)) {
								arrayObjs[i]=new Logger.WriteLineDelegate((Action<string,LogLevel>)delegate(string a,LogLevel b) { });
							}
							else {
								arrayObjs[i]=Activator.CreateInstance(parameterType);
							}
						}
						catch(Exception ex) {
							string error="!!!!!UNSUPPORTED PARAMETER TYPE!!!!\r\n"
								+"Method: "+sClass?.Name+"."+method?.Name+"\r\n"
								+"\tError: "+ex.Message;
							retVal.AppendLine(error);
						}
					}
					if(hasOutOrRefParam) {
						continue;//Middle tier does not support out or ref params.
					}
					try {
						method.Invoke(null,arrayObjs);
					}
					catch(Exception ex) {
						string error="Method: "+sClass?.Name+"."+method?.Name+"\r\n"
							+"\tError: "+ex.Message;
						Console.WriteLine(error);
						retVal.AppendLine(error);
					}
				}
			}
			//Now that we have our log of all S class methods that end up calling a Meth.Dto... method, we need to parse it out and call them again.
			//Any methods that fail within this loop need to be manually examined and considered.
			if(string.IsNullOrEmpty(retVal.ToString())) {
				retVal.Append("Passed.  No methods found with invalid parameters.");
			}
			return retVal.ToString();
		}

	}
}
