using CodeBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace OpenDentBusiness {
	public class DtoProcessor {

		///<summary>Pass in a serialized dto.  It returns a dto which must be deserialized by the client.
		///Set serverMapPath to the root directory of the OpenDentalServerConfig.xml.  Typically Server.MapPath(".") from a web service.
		///Optional parameter because it is not necessary for Unit Tests (mock server).</summary>
		public static string ProcessDto(string dtoString,string serverMapPath="") {
			#if DEBUG
				//System.Threading.Thread.Sleep(100);//to test slowness issues with web service.
			#endif
			DataTransferObject dto=DataTransferObject.Deserialize(dtoString);
			try {
				string[] methNameComps=GetComponentsFromDtoMeth(dto.MethodName);
				if(methNameComps.Length==3 && methNameComps[2].ToLower()=="hashpassword") {
					return GetHashPassword(dto);
				}
				//Always attempt to set the database connection settings from the config file if they haven't been set yet.
				//We use to ONLY load in database settings when Security.LogInWeb was called but that is not good enough now that we have more services.
				//E.g. We do not want to manually call "Security.LogInWeb" from the CEMT when all we want is a single preference value.
				if(string.IsNullOrEmpty(DataConnection.GetServerName()) && string.IsNullOrEmpty(DataConnection.GetConnectionString())) {
					RemotingClient.RemotingRole=RemotingRole.ServerWeb;
					//the application virtual path is usually /OpenDentalServer, but may be different if hosting multiple databases on one IIS server
					string configFilePath="";
					if(!string.IsNullOrWhiteSpace(HostingEnvironment.ApplicationVirtualPath) && HostingEnvironment.ApplicationVirtualPath.Length>1) {
						//There can be multiple config files within a physical path that is shared by multiple IIS ASP.NET applications.
						//In order for the same physical path to host multiple applications, they each need a unique config file for db connection settings.
						//Each application will have a unique ApplicationVirtualPath which we will use to identify the corresponding config.xml.
						configFilePath=ODFileUtils.CombinePaths(serverMapPath,HostingEnvironment.ApplicationVirtualPath.Trim('/')+"Config.xml");
					}
					if(string.IsNullOrWhiteSpace(configFilePath)
						|| !File.Exists(configFilePath))//returns false if the file doesn't exist, user doesn't have permission for file, path is blank or null
					{
						//either configFilePath not set or file doesn't exist, default to OpenDentalServerConfig.xml
						configFilePath=ODFileUtils.CombinePaths(serverMapPath,"OpenDentalServerConfig.xml");
					}
					Userods.LoadDatabaseInfoFromFile(configFilePath);
				}
				//Set Security.CurUser so that queries can be run against the db as if it were this user.
				Security.CurUser=Userods.CheckUserAndPassword(dto.Credentials.Username
					,dto.Credentials.Password
					,Programs.IsEnabled(ProgramName.eClinicalWorks));
				Security.PasswordTyped=dto.Credentials.Password;
				Type type = dto.GetType();
				#region DtoGetTable
				if(type == typeof(DtoGetTable)) {
					DtoGetTable dtoGetTable=(DtoGetTable)dto;
					string[] fullNameComponents=GetComponentsFromDtoMeth(dtoGetTable.MethodName);
					string assemblyName=fullNameComponents[0];//OpenDentBusiness or else a plugin name
					string className=fullNameComponents[1];
					string methodName=fullNameComponents[2];
					Type classType=null;
					Assembly ass=Plugins.GetAssembly(assemblyName);
					if(ass==null) {
						classType=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className+","+assemblyName);
					}
					else {//plugin was found
						classType=ass.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className);
					}
					DtoObject[] parameters=dtoGetTable.Params;
					Type[] paramTypes=DtoObject.GenerateTypes(parameters,assemblyName);
					MethodInfo methodInfo=classType.GetMethod(methodName,paramTypes);
					if(methodInfo==null) {
						throw new ApplicationException("Method not found with "+parameters.Length.ToString()+" parameters: "+dtoGetTable.MethodName);
					}
					object[] paramObjs=DtoObject.GenerateObjects(parameters);
					DataTable dt=(DataTable)methodInfo.Invoke(null,paramObjs);
					String response=XmlConverter.TableToXml(dt);
					return response;
				}
				#endregion
				#region DtoGetTableLow
				else if(type == typeof(DtoGetTableLow)) {
					DtoGetTableLow dtoGetTableLow=(DtoGetTableLow)dto;
					DtoObject[] parameters=dtoGetTableLow.Params;
					object[] paramObjs=DtoObject.GenerateObjects(parameters);
					DataTable dt=Reports.GetTable((string)paramObjs[0]);
					String response=XmlConverter.TableToXml(dt);
					return response;
				}
				#endregion
				#region DtoGetDS
				else if(type == typeof(DtoGetDS)) {
					DtoGetDS dtoGetDS=(DtoGetDS)dto;
					string[] fullNameComponents=GetComponentsFromDtoMeth(dtoGetDS.MethodName);
					string assemblyName=fullNameComponents[0];//OpenDentBusiness or else a plugin name
					string className=fullNameComponents[1];
					string methodName=fullNameComponents[2];
					Type classType=null;
					Assembly ass=Plugins.GetAssembly(assemblyName);
					if(ass==null) {
						classType=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className+","+assemblyName);
					}
					else {//plugin was found
						classType=ass.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className);
					}
					DtoObject[] parameters=dtoGetDS.Params;
					Type[] paramTypes=DtoObject.GenerateTypes(parameters,assemblyName);
					MethodInfo methodInfo=classType.GetMethod(methodName,paramTypes);
					if(methodInfo==null) {
						throw new ApplicationException("Method not found with "+parameters.Length.ToString()+" parameters: "+dtoGetDS.MethodName);
					}
					object[] paramObjs=DtoObject.GenerateObjects(parameters);
					DataSet ds=(DataSet)methodInfo.Invoke(null,paramObjs);
					String response=XmlConverter.DsToXml(ds);
					return response;
				}
				#endregion
				#region DtoGetSerializableDictionary
				else if(type == typeof(DtoGetSerializableDictionary)) {
					DtoGetSerializableDictionary dtoGetSD=(DtoGetSerializableDictionary)dto;
					string[] fullNameComponents=GetComponentsFromDtoMeth(dtoGetSD.MethodName);
					string assemblyName=fullNameComponents[0];//OpenDentBusiness or else a plugin name
					string className=fullNameComponents[1];
					string methodName=fullNameComponents[2];
					Type classType=null;
					Assembly ass=Plugins.GetAssembly(assemblyName);
					if(ass==null) {
						classType=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className+","+assemblyName);
					}
					else {//plugin was found
						classType=ass.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className);
					}
					DtoObject[] parameters=dtoGetSD.Params;
					Type[] paramTypes=DtoObject.GenerateTypes(parameters,assemblyName);
					MethodInfo methodInfo=classType.GetMethod(methodName,paramTypes);
					if(methodInfo==null) {
						throw new ApplicationException("Method not found with "+parameters.Length.ToString()+" parameters: "+dtoGetSD.MethodName);
					}
					object[] paramObjs=DtoObject.GenerateObjects(parameters);
					Object objResult=methodInfo.Invoke(null,paramObjs);
					Type returnType=methodInfo.ReturnType;
					return XmlConverter.Serialize(returnType,objResult);
				}
				#endregion
				#region DtoGetLong
				else if(type == typeof(DtoGetLong)) {
					DtoGetLong dtoGetLong=(DtoGetLong)dto;
					string[] fullNameComponents=GetComponentsFromDtoMeth(dtoGetLong.MethodName);
					string assemblyName=fullNameComponents[0];//OpenDentBusiness or else a plugin name
					string className=fullNameComponents[1];
					string methodName=fullNameComponents[2];
					Type classType=null;
					Assembly ass=Plugins.GetAssembly(assemblyName);
					if(ass==null) {
						classType=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className+","+assemblyName);
					}
					else {//plugin was found
						classType=ass.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className);
					}
					DtoObject[] parameters=dtoGetLong.Params;
					Type[] paramTypes=DtoObject.GenerateTypes(parameters,assemblyName);
					MethodInfo methodInfo=classType.GetMethod(methodName,paramTypes);
					if(methodInfo==null) {
						throw new ApplicationException("Method not found with "+parameters.Length.ToString()+" parameters: "+dtoGetLong.MethodName);
					}
					object[] paramObjs=DtoObject.GenerateObjects(parameters);
					long longResult=(long)methodInfo.Invoke(null,paramObjs);
					return longResult.ToString();
				}
				#endregion
				#region DtoGetInt
				else if(type == typeof(DtoGetInt)) {
					DtoGetInt dtoGetInt=(DtoGetInt)dto;
					string[] fullNameComponents=GetComponentsFromDtoMeth(dtoGetInt.MethodName);
					string assemblyName=fullNameComponents[0];//OpenDentBusiness or else a plugin name
					string className=fullNameComponents[1];
					string methodName=fullNameComponents[2];
					Type classType=null;
					Assembly ass=Plugins.GetAssembly(assemblyName);
					if(ass==null) {
						classType=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className+","+assemblyName);
					}
					else {//plugin was found
						classType=ass.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className);
					}
					DtoObject[] parameters=dtoGetInt.Params;
					Type[] paramTypes=DtoObject.GenerateTypes(parameters,assemblyName);
					MethodInfo methodInfo=classType.GetMethod(methodName,paramTypes);
					if(methodInfo==null) {
						throw new ApplicationException("Method not found with "+parameters.Length.ToString()+" parameters: "+dtoGetInt.MethodName);
					}
					object[] paramObjs=DtoObject.GenerateObjects(parameters);
					int intResult=(int)methodInfo.Invoke(null,paramObjs);
					return intResult.ToString();
				}
				#endregion
				#region DtoGetDouble
				else if(type == typeof(DtoGetDouble)) {
					DtoGetDouble dtoGetDouble=(DtoGetDouble)dto;
					string[] fullNameComponents=GetComponentsFromDtoMeth(dtoGetDouble.MethodName);
					string assemblyName=fullNameComponents[0];//OpenDentBusiness or else a plugin name
					string className=fullNameComponents[1];
					string methodName=fullNameComponents[2];
					Type classType=null;
					Assembly ass=Plugins.GetAssembly(assemblyName);
					if(ass==null) {
						classType=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className+","+assemblyName);
					}
					else {//plugin was found
						classType=ass.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className);
					}
					DtoObject[] parameters=dtoGetDouble.Params;
					Type[] paramTypes=DtoObject.GenerateTypes(parameters,assemblyName);
					MethodInfo methodInfo=classType.GetMethod(methodName,paramTypes);
					if(methodInfo==null) {
						throw new ApplicationException("Method not found with "+parameters.Length.ToString()+" parameters: "+dtoGetDouble.MethodName);
					}
					object[] paramObjs=DtoObject.GenerateObjects(parameters);
					double doubleResult=(double)methodInfo.Invoke(null,paramObjs);
					return doubleResult.ToString();
				}
				#endregion
				#region DtoGetVoid
				else if(type == typeof(DtoGetVoid)) {
					DtoGetVoid dtoGetVoid=(DtoGetVoid)dto;
					string[] fullNameComponents=GetComponentsFromDtoMeth(dtoGetVoid.MethodName);
					string assemblyName=fullNameComponents[0];//OpenDentBusiness or else a plugin name
					string className=fullNameComponents[1];
					string methodName=fullNameComponents[2];
					Type classType=null;
					Assembly ass=Plugins.GetAssembly(assemblyName);
					if(ass==null) {
						classType=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className+","+assemblyName);
					}
					else {//plugin was found
						classType=ass.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className);
					}
					DtoObject[] parameters=dtoGetVoid.Params;
					Type[] paramTypes=DtoObject.GenerateTypes(parameters,assemblyName);
					MethodInfo methodInfo=classType.GetMethod(methodName,paramTypes);
					if(methodInfo==null) {
						throw new ApplicationException("Method not found with "+parameters.Length.ToString()+" parameters: "+dtoGetVoid.MethodName);
					}
					object[] paramObjs=DtoObject.GenerateObjects(parameters);
					methodInfo.Invoke(null,paramObjs);
					return "0";
				}
				#endregion
				#region DtoGetObject
				else if(type == typeof(DtoGetObject)) {
					DtoGetObject dtoGetObject=(DtoGetObject)dto;
					string[] fullNameComponents=GetComponentsFromDtoMeth(dtoGetObject.MethodName);
					string assemblyName=fullNameComponents[0];//OpenDentBusiness or else a plugin name
					string className=fullNameComponents[1];
					string methodName=fullNameComponents[2];
					//if(className != "Security" || methodName != "LogInWeb") {//because credentials will be checked inside that method
					//	Userods.CheckCredentials(dtoGetObject.Credentials);//will throw exception if fails.
					//}
					Type classType=null;
					Assembly ass=Plugins.GetAssembly(assemblyName);
					//if(className!="Security" || methodName!="LogInWeb") {//Do this for everything except Security.LogInWeb, because Plugins.GetAssembly will fail in that case.
					//	ass=Plugins.GetAssembly(assemblyName);
					//}
					if(ass==null) {
						classType=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className+","+assemblyName);
					}
					else {//plugin was found
						classType=ass.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className);
					}
					DtoObject[] parameters=dtoGetObject.Params;
					Type[] paramTypes=DtoObject.GenerateTypes(parameters,assemblyName);
					MethodInfo methodInfo=classType.GetMethod(methodName,paramTypes);
					if(methodInfo==null) {
						throw new ApplicationException("Method not found with "+parameters.Length.ToString()+" parameters: "+dtoGetObject.MethodName);
					}
					if(className=="Security" && methodName=="LogInWeb") {
						parameters[2]=new DtoObject(serverMapPath,typeof(string));//because we can't access this variable from within OpenDentBusiness.
						RemotingClient.RemotingRole=RemotingRole.ServerWeb;
					}
					object[] paramObjs=DtoObject.GenerateObjects(parameters);
					Object objResult=methodInfo.Invoke(null,paramObjs);
					Type returnType=methodInfo.ReturnType;
					if(returnType.IsInterface) {
						objResult=new DtoObject(objResult,objResult?.GetType()??returnType);
						returnType=typeof(DtoObject);
					}
					return XmlConverter.Serialize(returnType,objResult);
				}
				#endregion
				#region DtoGetString
				else if(type == typeof(DtoGetString)) {
					DtoGetString dtoGetString=(DtoGetString)dto;
					string[] fullNameComponents=GetComponentsFromDtoMeth(dtoGetString.MethodName);
					string assemblyName=fullNameComponents[0];//OpenDentBusiness or else a plugin name
					string className=fullNameComponents[1];
					string methodName=fullNameComponents[2];
					Type classType=null;
					Assembly ass=Plugins.GetAssembly(assemblyName);
					if(ass==null) {
						classType=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className+","+assemblyName);
					}
					else {//plugin was found
						classType=ass.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className);
					}
					DtoObject[] parameters=dtoGetString.Params;
					Type[] paramTypes=DtoObject.GenerateTypes(parameters,assemblyName);
					MethodInfo methodInfo=classType.GetMethod(methodName,paramTypes);
					if(methodInfo==null) {
						throw new ApplicationException("Method not found with "+parameters.Length.ToString()+" parameters: "+dtoGetString.MethodName);
					}
					object[] paramObjs=DtoObject.GenerateObjects(parameters);
					string strResult=(string)methodInfo.Invoke(null,paramObjs);
					strResult=XmlConverter.XmlEscape(strResult);
					return strResult;
				}
				#endregion
				#region DtoGetBool
				else if(type == typeof(DtoGetBool)) {
					DtoGetBool dtoGetBool=(DtoGetBool)dto;
					string[] fullNameComponents=GetComponentsFromDtoMeth(dtoGetBool.MethodName);
					string assemblyName=fullNameComponents[0];//OpenDentBusiness or else a plugin name
					string className=fullNameComponents[1];
					string methodName=fullNameComponents[2];
					Type classType=null;
					Assembly ass=Plugins.GetAssembly(assemblyName);
					if(ass==null) {
						classType=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className+","+assemblyName);
					}
					else {//plugin was found
						classType=ass.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
							+"."+className);
					}
					DtoObject[] parameters=dtoGetBool.Params;
					Type[] paramTypes=DtoObject.GenerateTypes(parameters,assemblyName);
					MethodInfo methodInfo=classType.GetMethod(methodName,paramTypes);
					if(methodInfo==null) {
						throw new ApplicationException("Method not found with "+parameters.Length.ToString()+" parameters: "+dtoGetBool.MethodName);
					}
					object[] paramObjs=DtoObject.GenerateObjects(parameters);
					bool boolResult=(bool)methodInfo.Invoke(null,paramObjs);
					return boolResult.ToString();
				}
				#endregion
				else {
					throw new NotSupportedException("Dto type not supported: "+type.FullName);
				}
			}
			catch(Exception e) {
				DtoException exception = new DtoException();
				exception.ExceptionType=e.GetType().BaseType.Name;//Since the exception was down converted to a regular exception, we need the BaseType.
				if(e.InnerException==null) {
					exception.Message = e.Message;
				}
				else {
					exception.Message = e.InnerException.Message;
				}
				return exception.Serialize();
			}
		}

		///<summary>Only used if the dto passed into ProcessRequest is trying to call "Userods.HashPassword".
		///This is so that passwords will be hashed on the server to utilize the server's MD5 hash algorithm instead of the workstation's algorithm.  
		///This is due to the group policy security option "System cryptography: Use FIPS compliant algorithms for encryption,
		///hashing and signing" that is enabled on workstations for some users but not on the server.  This allows those users to utilize the server's
		///algorithm without requiring the workstations to have the algorithm at all.</summary>
		private static string GetHashPassword(DataTransferObject dto) {
			DtoGetString dtoGetString=(DtoGetString)dto;
			string[] fullNameComponents=GetComponentsFromDtoMeth(dtoGetString.MethodName);
			string assemblyName=fullNameComponents[0];//OpenDentBusiness or else a plugin name
			string className=fullNameComponents[1];
			string methodName=fullNameComponents[2];
			Type classType=null;
			Assembly ass=Plugins.GetAssembly(assemblyName);
			if(ass==null) {
				classType=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
					+"."+className+","+assemblyName);
			}
			else {//plugin was found
				classType=ass.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
					+"."+className);
			}
			DtoObject[] parameters=dtoGetString.Params;
			Type[] paramTypes=DtoObject.GenerateTypes(parameters,assemblyName);
			MethodInfo methodInfo=classType.GetMethod(methodName,paramTypes);
			if(methodInfo==null) {
				throw new ApplicationException("Method not found with "+parameters.Length.ToString()+" parameters: "+dtoGetString.MethodName);
			}
			object[] paramObjs=DtoObject.GenerateObjects(parameters);
			string strResult=(string)methodInfo.Invoke(null,paramObjs);
			strResult=XmlConverter.XmlEscape(strResult);
			return strResult;
		}

		///<summary>Helper function to handle full method name with 2 components or 3 components.  Versions prior to 14.3 will send 2 components.  14.3 and above will send the assembly name OpenDentBusiness or plugin assembly name.  If only 2 components are received, we will prepend OpenDentBusiness so this will be backward compatible with versions prior to 14.3.</summary>
		private static string[] GetComponentsFromDtoMeth(string methodName) {
			if(methodName.Split('.').Length==2) {
				methodName="OpenDentBusiness."+methodName;
			}
			return methodName.Split('.');
		}
	}
}
