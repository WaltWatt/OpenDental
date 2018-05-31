using System;

namespace CodeBase {
	public class ODException:ApplicationException {
		private int _errorCode=0;
		///<summary>Contains query text when an ErrorCode in the 700s was thrown. This is the query that was attempted prior to an exception.</summary>
		private string _query="";

		///<summary>Gets the error code associated to this exception.  Defaults to 0 if no error code was explicitly set.</summary>		
		public int ErrorCode {
			get {
				return _errorCode;
			}
		}

		///<summary>Contains query text when an ErrorCode in the 700s was thrown. This is the query that was attempted prior to an exception.</summary>
		public string Query {
			get {
				return _query??"";
			}
		}

		///<summary>Convert an int to an Enum typed ErrorCode. Returns NotDefined if the input errorCode is not defined in ErrorCodes.</summary>		
		public static ErrorCodes GetErrorCodeAsEnum(int errorCode) {
			if(!Enum.IsDefined(typeof(ErrorCodes),errorCode)) {
				return ErrorCodes.NotDefined;
			}
			return (ErrorCodes)errorCode;
		}

		///<summary>Gets the pre-defined error code associated to this exception.  
		///Defaults to NotDefined if the error code (int) specified is not defined in ErrorCodes enum.</summary>		
		public ErrorCodes ErrorCodeAsEnum {
			get {
				return GetErrorCodeAsEnum(_errorCode);
			}
		}

		public ODException() { }

		public ODException(int errorCode) : this("",errorCode) { }

		public ODException(string message) : this(message,0) { }

		public ODException(string message,ErrorCodes errorCodeAsEnum) : this(message,(int)errorCodeAsEnum) { }

		public ODException(string message,int errorCode)
			: base(message) {
			_errorCode=errorCode;
		}
		
		///<summary>Used for query based exceptions in Db.cs</summary>
		public ODException(string message,string query,Exception ex) : base(message,ex) {
			_query=query;
			_errorCode=(int)ErrorCodes.DbQueryError;
		}

		///<summary>Wrap the given action in a try/catch and swallow any exceptions that are thrown. 
		///This should be used sparingly as we typically want to handle the exception or let it bubble up to the UI but sometimes you just want to ignore it.</summary>
		public static void SwallowAnyException(Action a) {
			try {
				a();
			}
			catch(Exception ex) { 
				ex.DoNothing();
			}
		}

		///<summary>Predefined ODException.ErrorCode field values. ErrorCode field is not limited to these values but this is a convenient way defined known error types.
		///These values must be converted to/from int in order to be stored in ODException.ErrorCode.
		///Number ranges are arbitrary but should reserve plenty of padding for the future of a given range.
		///Each number range should share a similar prefix between all of it's elements.</summary>
		public enum ErrorCodes {
			///<summary>0 is the default. If the given (int) ErrorCode is not defined here, it will be returned at 0 - NotDefined.</summary>
			NotDefined=0,
			//100-199 range. Values used by ODSocket architecture.
			///<summary>No immortal socket connection found for this RegistrationKeyNum. 
			///The Proxy is trying to communicate with this eConnector but the eConnector does not have an active connection.</summary>		
			ODSocketNotFoundForRegKeyNum=100,
			///<summary>Immortal socket connection was found by Proxy but the remote eConnector socket is not responding. 
			///Most likely because the eConnector has been turned off but the Proxy has not performed an ACK to discover that it's off.</summary>		
			ODSocketEConnectorNotResponding=101,
			//200-299 range. Values used by XWeb/XCharge integration.
			///<summary>.</summary>		
			OtkArgsInvalid=200,
			///<summary>.</summary>		
			OtkCreationFailed=201,
			///<summary>.</summary>		
			MaxRequestDataExceeded=202,
			///<summary>.</summary>		
			XWebProgramProperties=203,
			//400-499 range. Values used by web apps
			///<summary>No patient found that matches the specified parameters.</summary>
			NoPatientFound=400,
			///<summary>More than one patient found that matches the specified parameters.</summary>
			MultiplePatientsFound=401,
			///<summary>No appointment found that matches the specified parameters.</summary>
			NoAppointmentFound=402,
			///<summary>The time slot provided was not found or invalid.</summary>
			TimeSlotInvalid=403,
			///<summary>The response status provided is not acceptable.</summary>
			ResponseStatusInvalid=404,
			///<summary>No asapcomm found that matches the specified parameters.</summary>
			NoAsapCommFound=405,
			///<summary>No operatories have been set up for Web Sched.</summary>
			NoOperatoriesSetup=406,
			//500-599 range. Values used by Open Dental UI.
			FormClosed = 500,
			//600-699 range. Values used by RemotingClient/MiddleTier
			///<summary>After successfully logging in to Open Dental, a middle tier call to Userods.CheckUserAndPassword returned an "Invalid user or password" error.</summary>
			CheckUserAndPasswordFailed=600,
			//700-799 range. Values used by failed query exceptions.
			///<summary>Generic database command failed to execute.</summary>
			DbQueryError=700,
		}
	}
}
