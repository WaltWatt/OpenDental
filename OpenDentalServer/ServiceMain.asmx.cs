using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Services;
using CodeBase;
using OpenDentBusiness;

namespace OpenDentalServer {
	/// <summary></summary>
	[WebService(Namespace="http://www.open-dent.com/OpenDentalServer")]
	[WebServiceBinding(ConformsTo=WsiProfiles.BasicProfile1_1)]
	[ToolboxItem(false)]
	public class ServiceMain:System.Web.Services.WebService {

		/// <summary>Pass in a serialized dto.  It returns a dto which must be deserialized by the client.</summary>
		[WebMethod]
		public string ProcessRequest(string dtoString) {
			#if DEBUG
				//System.Threading.Thread.Sleep(100);//to test slowness issues with web service.
			#endif
			DataTransferObject dto=DataTransferObject.Deserialize(dtoString);
			try {
				string[] fullNameComponents=GetComponentsFromDtoMeth(dto.MethodName);
				if(fullNameComponents.Length==3 && fullNameComponents[2].ToLower()=="hashpassword") {
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
						configFilePath=ODFileUtils.CombinePaths(Server.MapPath("."),HostingEnvironment.ApplicationVirtualPath.Trim('/')+"Config.xml");
					}
					if(string.IsNullOrWhiteSpace(configFilePath)
						|| !File.Exists(configFilePath))//returns false if the file doesn't exist, user doesn't have permission for file, path is blank or null
					{
						//either configFilePath not set or file doesn't exist, default to OpenDentalServerConfig.xml
						configFilePath=ODFileUtils.CombinePaths(Server.MapPath("."),"OpenDentalServerConfig.xml");
					}
					Userods.LoadDatabaseInfoFromFile(configFilePath);
				}
				//only check user and password if not calling Security.LogInWeb or Userods.CheckUserAndPassword because they already call
				//Userods.CheckUserAndPassword and may include an additional parameter.
				if(fullNameComponents.Length<2 || !(fullNameComponents[1]+"."+fullNameComponents[2]).In("Security.LogInWeb","Userods.CheckUserAndPassword")) {
					//Set Security.CurUser so that queries can be run against the db as if it were this user.
					Security.CurUser=Userods.CheckUserAndPassword(dto.Credentials.Username
						,dto.Credentials.Password
						,Programs.IsEnabled(ProgramName.eClinicalWorks)
						,true);
					Security.PasswordTyped=dto.Credentials.Password;
				}
				//If the MySQL service has gone away, we will retry up to 10 seconds to establish a connection. We don't want to wait too much longer 
				//because the client will timeout if we don't return from this method quickly.
				DataConnection.ConnectionRetrySeconds=10;
				Type typeDto=dto.GetType();
				string assemblyName=fullNameComponents[0];//OpenDentBusiness or else a plugin name
				string className=fullNameComponents[1];
				string methodName=fullNameComponents[2];
				Type typeClass=null;
				Assembly ass=Plugins.GetAssembly(assemblyName);
				if(ass==null) {
					typeClass=Type.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
						+"."+className+","+assemblyName);
				}
				else {//plugin was found
					typeClass=ass.GetType(assemblyName//actually, the namespace which we require to be same as assembly by convention
						+"."+className);
				}
				DtoObject[] parameters=dto.Params;
				Type[] paramTypes=DtoObject.GenerateTypes(parameters,assemblyName);
				MethodInfo methodInfo=typeClass.GetMethod(methodName,paramTypes);
				if(methodInfo==null) {
					throw new ApplicationException("Method not found with "+parameters.Length.ToString()+" parameters: "+dto.MethodName);
				}
				object[] paramObjs=DtoObject.GenerateObjects(parameters);
				#region DtoGetTable
				if(typeDto==typeof(DtoGetTable)) {
					DataTable dt=(DataTable)methodInfo.Invoke(null,paramObjs);
					return XmlConverter.TableToXml(dt);
				}
				#endregion
				#region DtoGetTableLow
				else if(typeDto==typeof(DtoGetTableLow)) {
					DataTable dt=Reports.GetTable((string)paramObjs[0]);
					return XmlConverter.TableToXml(dt);
				}
				#endregion
				#region DtoGetDS
				else if(typeDto==typeof(DtoGetDS)) {
					DataSet ds=(DataSet)methodInfo.Invoke(null,paramObjs);
					return XmlConverter.DsToXml(ds);
				}
				#endregion
				#region DtoGetSerializableDictionary
				else if(typeDto==typeof(DtoGetSerializableDictionary)) {
					object objResult=methodInfo.Invoke(null,paramObjs);
					Type returnType=methodInfo.ReturnType;
					return XmlConverter.Serialize(returnType,objResult);
				}
				#endregion
				#region DtoGetLong
				else if(typeDto==typeof(DtoGetLong)) {
					long longResult=(long)methodInfo.Invoke(null,paramObjs);
					return longResult.ToString();
				}
				#endregion
				#region DtoGetInt
				else if(typeDto==typeof(DtoGetInt)) {
					int intResult=(int)methodInfo.Invoke(null,paramObjs);
					return intResult.ToString();
				}
				#endregion
				#region DtoGetDouble
				else if(typeDto==typeof(DtoGetDouble)) {
					double doubleResult=(double)methodInfo.Invoke(null,paramObjs);
					return doubleResult.ToString();
				}
				#endregion
				#region DtoGetVoid
				else if(typeDto==typeof(DtoGetVoid)) {
					methodInfo.Invoke(null,paramObjs);
					return "0";
				}
				#endregion
				#region DtoGetObject
				else if(typeDto==typeof(DtoGetObject)) {
					if(className==nameof(Security) && methodName==nameof(Security.LogInWeb)) {
						string mappedPath=Server.MapPath(".");
						parameters[2]=new DtoObject(mappedPath,typeof(string));//because we can't access this variable from within OpenDentBusiness.
						RemotingClient.RemotingRole=RemotingRole.ServerWeb;
						paramObjs=DtoObject.GenerateObjects(parameters);
					}
					object objResult=methodInfo.Invoke(null,paramObjs);
					Type returnType=methodInfo.ReturnType;
					return XmlConverter.Serialize(returnType,objResult);
				}
				#endregion
				#region DtoGetString
				else if(typeDto==typeof(DtoGetString)) {
					string strResult=(string)methodInfo.Invoke(null,paramObjs);
					return XmlConverter.XmlEscape(strResult);
				}
				#endregion
				#region DtoGetBool
				else if(typeDto==typeof(DtoGetBool)) {
					bool boolResult=(bool)methodInfo.Invoke(null,paramObjs);
					return boolResult.ToString();
				}
				#endregion
				else {
					throw new NotSupportedException("Dto type not supported: "+typeDto.FullName);
				}
			}
			catch(ODException ode) {
				DtoException exception=new DtoException();
				if(ode.InnerException==null) {
					exception.ExceptionType=ode.GetType().Name;
					exception.Message=ode.Message;
				}
				else {
					exception.ExceptionType=ode.InnerException.GetType().Name;
					exception.Message=ode.InnerException.Message;
				}
				exception.ErrorCode=ode.ErrorCode;
				return exception.Serialize();
			}
			catch(Exception e) {
				DtoException exception=new DtoException();
				if(e.InnerException==null) {
					exception.ExceptionType=e.GetType().Name;
					exception.Message=e.Message;
				}
				else {
					exception.ExceptionType=e.InnerException.GetType().Name;
					exception.Message=e.InnerException.Message;
				}
				return exception.Serialize();
			}
		}

		///<summary>Only used if the dto passed into ProcessRequest is trying to call "Userods.HashPassword".
		///This is so that passwords will be hashed on the server to utilize the server's MD5 hash algorithm instead of the workstation's algorithm.  
		///This is due to the group policy security option "System cryptography: Use FIPS compliant algorithms for encryption,
		///hashing and signing" that is enabled on workstations for some users but not on the server.  This allows those users to utilize the server's
		///algorithm without requiring the workstations to have the algorithm at all.</summary>
		private string GetHashPassword(DataTransferObject dto) {
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

		///<summary>Helper function to handle full method name and turn it into 3 components.  The 3 components returned are:
		///1. Assembly name
		///2. Class name (however, this may contain the portion of the namespace after the assembly, 
		///				e.g. if "OpenDentBusiness.Eclaims.Eclaims.GetMissingData" is passed in, this component will contain "Eclaims.Eclaims".)
		///3. Method name</summary>
		private string[] GetComponentsFromDtoMeth(string methodName) {
			if(methodName.Split('.').Length==2) {
				//Versions prior to 14.3 will send 2 components. 14.3 and above will send the assembly name OpenDentBusiness or plugin assembly name.  
				//If only 2 components are received, we will prepend OpenDentBusiness so this will be backward compatible with versions prior to 14.3.
				methodName="OpenDentBusiness."+methodName;
			}
			if(methodName.Split('.').Length<=3) {
				return methodName.Split('.');
			}
			//The method is in a namespace that contains multiple parts.
			int firstIdx=methodName.IndexOf('.');
			int lastIdx=methodName.LastIndexOf('.');
			return new string[] {
				//First part of namespace which should also be the assembly name
				methodName.Substring(0,firstIdx),
				//The rest of the namespace plus the class name
				methodName.Substring(firstIdx+1,lastIdx-firstIdx-1),
				//The method name
				methodName.Substring(lastIdx+1)
			};
		}

		

	}
}
