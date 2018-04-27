using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using CodeBase;
using OpenDentBusiness;

namespace OpenDentBusiness {
	public class RemotingClient {
		///<summary>This dll will be in one of these three roles.  There can be a dll on the client and a dll on the server, both involved in the logic.  This keeps track of which one is which.</summary>
		private static RemotingRole _remotingRole;
		///<summary>If ClientWeb, then this is the URL to the server.</summary>
		private static string _serverUri;
		///<summary>If ClientWeb (middle tier user), proxy settings can be picked up from MiddleTierProxyConfig.xml.</summary>
		public static string MidTierProxyAddress;
		///<summary>If ClientWeb (middle tier user), proxy settings can be picked up from MiddleTierProxyConfig.xml.</summary>
		public static string MidTierProxyUserName;
		///<summary>If ClientWeb (middle tier user), proxy settings can be picked up from MiddleTierProxyConfig.xml.</summary>
		public static string MidTierProxyPassword;
		///<summary>Thread static version of RemotingRole</summary>
		[ThreadStatic]
		public static RemotingRole _remotingRoleT;
		///<summary>Thread static string version of RemotingRole because enums cannot be null thus we never know what value to trust.</summary>
		[ThreadStatic]
		public static string _remotingRoleTStr;
		///<summary>Thread static version of ServerURI</summary>
		[ThreadStatic]
		public static string _serverUriT;
		private static bool _hasLoginFailed;
		private static ReaderWriterLockSlim _lockLoginFailed=new ReaderWriterLockSlim();
		
		///<summary>Set to true when a middle tier client has failed to validate credentials AFTER having logged in successfully.</summary>
		public static bool HasLoginFailed {
			get {
				if(RemotingRole!=RemotingRole.ClientWeb) {
					return false;//There is no such thing as the middle tier failing to "log in".  It must return a failed to log in payload to the client.
				}
				_lockLoginFailed.EnterReadLock();
				try {
					return _hasLoginFailed;
				}
				finally {
					_lockLoginFailed.ExitReadLock();
				}
			}
			set {
				_lockLoginFailed.EnterWriteLock();
				try {
					_hasLoginFailed=value;
				}
				finally {
					_lockLoginFailed.ExitWriteLock();
				}
			}
		}

		[ThreadStatic]
		private static bool _isReportServer;
		///<summary>True if the RemotingClient connection is connecting to a report middle tier server.</summary>
		public static bool IsReportServer {
			get {
				return _isReportServer;
			}
			set {
				if(RemotingRole!=RemotingRole.ServerWeb) {
					_isReportServer=false;//We only allow the passing of queries over middle tier when the client is itself a middle tier server.
				}
				else {
					_isReportServer=value;
				}
			}
		}

		///<summary>Returns either the thread specific RemotingRole or the globally set RemotingRole.</summary>
		public static RemotingRole RemotingRole {
			get {
				if(String.IsNullOrEmpty(_remotingRoleTStr)) {
					return _remotingRole;
				}
				return _remotingRoleT;
			}
			set {
				_remotingRole=value;
				_remotingRoleT=value;
				_remotingRoleTStr=value.ToString();//Simply used as an indicator that _remotingRoleT has been set.
			}
		}

		///<summary>Returns either the thread specific Server URI or the globally set Server URI.</summary>
		public static string ServerURI {
			get {
				if(String.IsNullOrEmpty(_serverUriT)) {
					return _serverUri;
				}
				return _serverUriT;
			}
			set {
				_serverUri=value;
				_serverUriT=value;
			}
		}

		public static void SetRemotingT(string serverURI,RemotingRole remotingRole,bool isReportServer) {
			IsReportServer=isReportServer;
			_remotingRoleT=remotingRole;
			_remotingRoleTStr=_remotingRoleT.ToString();//Simply used as an indicator that _remotingRoleT has been set.
			_serverUriT=serverURI;
		}

		public static void SetRemotingRoleT(RemotingRole remotingRole) {
			_remotingRoleT=remotingRole;
			_remotingRoleTStr=_remotingRoleT.ToString();//Simply used as an indicator that _remotingRoleT has been set.
		}

		public static void SetServerURIT(string serverURI) {
			_serverUriT=serverURI;
		}

		public static DataTable ProcessGetTable(DtoGetTable dto) {
			string result=SendAndReceive(dto);
			try {
				return XmlConverter.XmlToTable(result);
			}
			catch {
				DtoException exception=(DtoException)DataTransferObject.Deserialize(result);
				throw ThrowExceptionForDto(exception);
			}
		}

		public static DataTable ProcessGetTableLow(DtoGetTableLow dto) {
			string result=SendAndReceive(dto);
			try {
				return XmlConverter.XmlToTable(result);
			}
			catch {
				DtoException exception=(DtoException)DataTransferObject.Deserialize(result);
				throw ThrowExceptionForDto(exception);
			}
		}

		///<summary></summary>
		public static DataSet ProcessGetDS(DtoGetDS dto) {
			string result=SendAndReceive(dto);
			if(Regex.IsMatch(result,"<DtoException xmlns:xsi=")) {
				DtoException exception=(DtoException)DataTransferObject.Deserialize(result);
				throw new Exception(exception.Message);
			}
			try {
				return XmlConverter.XmlToDs(result);
			}
			catch {
				DtoException exception=(DtoException)DataTransferObject.Deserialize(result);
				throw ThrowExceptionForDto(exception);
			}
		}

		public static SerializableDictionary<K,V> ProcessGetSerializableDictionary<K,V>(DtoGetSerializableDictionary dto) {
			string result=SendAndReceive(dto);
			try {
				return XmlConverter.Deserialize<SerializableDictionary<K,V>>(result);
			}
			catch {
				DtoException exception=(DtoException)DataTransferObject.Deserialize(result);
				throw ThrowExceptionForDto(exception);
			}
		}

		///<summary></summary>
		public static long ProcessGetLong(DtoGetLong dto) {
			string result=SendAndReceive(dto);//this might throw an exception if server unavailable
			try {
				return PIn.Long(result);
			}
			catch {
				DtoException exception=(DtoException)DataTransferObject.Deserialize(result);
				throw ThrowExceptionForDto(exception);
			}
		}

		///<summary></summary>
		public static int ProcessGetInt(DtoGetInt dto) {
			string result=SendAndReceive(dto);//this might throw an exception if server unavailable
			try {
				return PIn.Int(result);
			}
			catch {
				DtoException exception=(DtoException)DataTransferObject.Deserialize(result);
				throw ThrowExceptionForDto(exception);
			}
		}

		///<summary></summary>
		public static double ProcessGetDouble(DtoGetDouble dto) {
			string result=SendAndReceive(dto);//this might throw an exception if server unavailable
			try {
				return PIn.Double(result);
			}
			catch {
				DtoException exception=(DtoException)DataTransferObject.Deserialize(result);
				throw ThrowExceptionForDto(exception);
			}
		}

		///<summary></summary>
		public static void ProcessGetVoid(DtoGetVoid dto) {
			string result=SendAndReceive(dto);//this might throw an exception if server unavailable
			if(result!="0"){
				DtoException exception=(DtoException)DataTransferObject.Deserialize(result);
				throw ThrowExceptionForDto(exception);
			}
		}

		///<summary></summary>
		public static T ProcessGetObject<T>(DtoGetObject dto) {
			string result=SendAndReceive(dto);//this might throw an exception if server unavailable
			try {
				return XmlConverter.Deserialize<T>(result);
				/*
				XmlSerializer serializer=new XmlSerializer(typeof(T));
					//Type.GetType("OpenDentBusiness."+dto.ObjectType));
				StringReader strReader=new StringReader(result);
				XmlReader xmlReader=XmlReader.Create(strReader);
				object obj=serializer.Deserialize(xmlReader);
				strReader.Close();
				xmlReader.Close();
				return (T)obj;*/
			}
			catch {
				DtoException exception=(DtoException)DataTransferObject.Deserialize(result);
				throw ThrowExceptionForDto(exception);
			}
		}

		///<summary></summary>
		public static string ProcessGetString(DtoGetString dto) {
			string result=SendAndReceive(dto);//this might throw an exception if server unavailable
			DtoException exception;
			try {
				exception=(DtoException)DataTransferObject.Deserialize(result);
			}
			catch {
				return XmlConverter.XmlUnescape(result);
			}
			throw ThrowExceptionForDto(exception);
		}

		///<summary></summary>
		public static bool ProcessGetBool(DtoGetBool dto) {
			string result=SendAndReceive(dto);
			if(result=="True") {
				return true;
			}
			if(result=="False") {
				return false;
			}
			DtoException exception=(DtoException)DataTransferObject.Deserialize(result);
			throw ThrowExceptionForDto(exception);
		}

		internal static string SendAndReceive(DataTransferObject dto){
			//Anyone trying to invoke a method other than CheckUserAndPassword must first check the current HasLoginFailed status as to not call the middle tier too often.
			bool isCheckUserAndPassword=(dto.MethodName==nameof(OpenDentBusiness)+"."+nameof(Userods)+"."+nameof(Userods.CheckUserAndPassword));
			if(!isCheckUserAndPassword && HasLoginFailed) {
				throw new ODException("Invalid username or password.",ODException.ErrorCodes.CheckUserAndPasswordFailed);
			}
			string dtoString=dto.Serialize();
			IOpenDentalServer service=OpenDentBusiness.WebServices.OpenDentalServerProxy.GetOpenDentalServerInstance();
			return service.ProcessRequest(dtoString);
		}

		///<summary>Open Dental can require specific exceptions to be thrown.  This is a helper method that throws the correct exception type.
		///Add this function directly into a throw statement, so that the calling code knows that the code path will not need to return a value.</summary>
		private static Exception ThrowExceptionForDto(DtoException exception) {
			switch(exception.ExceptionType) {
				case "ApplicationException":
					throw new ApplicationException(exception.Message);
				case "InvalidProgramException":
					throw new InvalidProgramException(exception.Message);
				case "NotSupportedException":
					throw new NotSupportedException(exception.Message);
				case "ODException":
					throw new ODException(exception.Message,exception.ErrorCode);
				default:
					//Throw a generic exception which follows the old functionality for any other Exception type that we weren't explicitly expecting.
					throw new Exception(exception.Message);
			}
		}

		
	}

}
