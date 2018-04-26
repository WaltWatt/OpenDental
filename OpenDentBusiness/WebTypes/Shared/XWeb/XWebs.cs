using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using CodeBase;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection;

namespace OpenDentBusiness.WebTypes.Shared.XWeb {
	public class XWebs {
		///<summary>Only used for testing. Should always be false otherwise.</summary>
		public static bool UseTestAccountOD=false;
		///<summary>Only used for testing. Should always be false otherwise.</summary>
		public static bool UseTestAccountLoopback=false;
		///<summary>USED BY DEBUGGING ONLY!!! Points the eConnector at XWeb test URLs.</summary>
		public static bool UseXWebTestGateway=false;

		#region Public Interface
		///<summary>Creates and returns the HPF URL and validation OTK which can be used to make a payment for an unspecified credit card.  Throws exceptions.</summary>
		public static XWebResponse GetHpfUrlForPayment(long patNum,string payNote,bool isMobile,double amount,bool createAlias,
			CreditCardSource ccSource) {
			//No need to check RemotingRole;no call to db.
			return new XWebInputHpfPayment(patNum,isMobile,amount,payNote,createAlias,ccSource).GenerateOutput();
		}

		///<summary>Creates and returns the HPF URL and validation OTK which can be used to create a credit card alias.  Throws exceptions.</summary>
		public static XWebResponse GetHpfUrlForCreditCardAlias(long patNum,bool isMobile) {
			//No need to check RemotingRole;no call to db.
			return new XWebInputHpfCreditCardAlias(patNum,isMobile).GenerateOutput();
		}

		///<summary>Make a payment using HPF directly.  Throws exceptions.</summary>
		public static XWebResponse MakePaymentWithAlias(long patNum,string payNote,double amount,long creditCardNum) {
			//No need to check RemotingRole;no call to db.
			return new XWebInputDTGPaymentSale(patNum,payNote,amount,creditCardNum).GenerateOutput();
		}

		///<summary>Void a payment using DTG directly. This is only valid for payments on the same day on which they originated.  Throws exceptions.</summary>
		public static XWebResponse VoidPayment(long patNum,string payNote,long xWebResponseNum) {
			//No need to check RemotingRole;no call to db.
			return new XWebInputDTGPaymentVoid(patNum,payNote,xWebResponseNum).GenerateOutput();
		}

		///<summary>Return a payment amount to a credit card using DTG directly.  Use this when void is not an option or you want to credit directly to the credit card without first having a transaction to void.  Throws exceptions.</summary>
		public static XWebResponse ReturnPayment(long patNum,string payNote,double amount,long creditCardNum,bool createPayment) {
			//No need to check RemotingRole;no call to db.
			return new XWebInputDTGPaymentReturn(patNum,payNote,amount,creditCardNum,createPayment).GenerateOutput();
		}

		///<summary>Delete a credit card from the database and delete it from XWeb repository using DTG directly.  Throws exceptions.</summary>
		public static XWebResponse DeleteCreditCard(long patNum,long creditCardNum) {
			//No need to check RemotingRole;no call to db.
			return new XWebInputDTGDeleteAlias(patNum,creditCardNum).GenerateOutput();
		}

		///<summary>Makes a web request to X-Charge to get the status for the OTK passed in.  Throws exceptions.</summary>
		public static XWebResponse GetOtkStatus(long patNum,string otk,bool blockUntilResponse) {
			//No need to check RemotingRole;no call to db.
			return new XWebInputOtkStatus(patNum,otk,blockUntilResponse).GenerateOutput();
		}
		#endregion

		#region Events
		public static EventHandler<Logger.LoggerEventArgs> InputEvent;
		public static EventHandler<Logger.LoggerEventArgs> OutputEvent;
		public static EventHandler WakeupMonitor;
		///<summary>XWeb Gateway input event. Always sent as Verbose.</summary>
		protected static void OnInputEvent(object sender,string s) {
			if(InputEvent!=null) {
				InputEvent(sender,new Logger.LoggerEventArgs(s,LogLevel.Verbose));
			}
		}
		///<summary>XWeb Gateway output event. Always sent as Verbose.</summary>
		protected static void OnOutputEvent(object sender,string s) {
			if(OutputEvent!=null) {
				OutputEvent(sender,new Logger.LoggerEventArgs(s,LogLevel.Verbose));
			}
		}
		///<summary>New OTK is available. Send wakeup event to EConnector so it will start processing immediately.</summary>
		protected static void OnWakeupMonitor(object sender,EventArgs e) {
			if(WakeupMonitor!=null) {
				WakeupMonitor(sender,new EventArgs());
			}
		}
		#endregion

		#region Helper Classes
		///<summary>Extend this class to interface the XWeb API with a specific web call.</summary>
		private abstract class XWebInputAbs {
			///<summary>Required for all web requests. 12 digit ID given to the merchant once they have been enrolled in the system.</summary>
			private string _xWebID;
			///<summary>Required for all web requests. 32 alphanumeric string assigned by XWeb.  Merchant must present the key to authenticate itself to the Gateway.  Required unless UserID and Password are used instead.</summary>
			private string _authKey;
			///<summary>Required for all web requests. 8 digit ID given to the merchant once they have been enrolled in the system.</summary>
			private string _terminalID;
			///<summary>PatNum which this transaction is linked to.</summary>
			protected long _patNum;
			///<summary>ProvNum which this transaction is linked to. PriProv of _patNum.</summary>
			protected long _provNum;
			///<summary>ClinicNum which this transaction is linked to. 0 if not using clinics.</summary>
			protected long _clinicNum;
			///<summary>Defines the type of transaction which is being created.</summary>
			protected XWebTransactionType _transactionType=XWebTransactionType.CreditSaleTransaction;
			///<summary>Serialize the input XWeb transmission to bytes.</summary>
			protected string GatewayInput {
				get {
					Dictionary<string,string> dictGatewayParams=GatewayParams;
					StringBuilder strBldXml=new StringBuilder();
					using(XmlWriter xmlWriter = XmlWriter.Create(strBldXml)) {
						xmlWriter.WriteStartElement("GatewayRequest");
						foreach(KeyValuePair<string,string> param in dictGatewayParams) {
							xmlWriter.WriteStartElement(param.Key);
							xmlWriter.WriteString(param.Value);
							xmlWriter.WriteEndElement();
						}
						xmlWriter.WriteEndElement();
						xmlWriter.Flush();
					}
					return strBldXml.ToString();
				}
			}
			///<summary>Returns the test or production URL for the XWeb gateway based on if the solution is configured for debugging.</summary>
			private string _gatewayUrl {
				get {
					if(UseXWebTestGateway) {
						//X-Charge Test Gateway URL 
						return "https://test.t3secure.net/x-chargeweb.dll";
					}
					else {
						//X-Charge Production Gateway URL : 
						return "https://gw.t3secure.net/x-chargeweb.dll";
					}
				}
			}
			///<summary>Provide key/value pairs for the XWeb transmission. X-Web program properties should be validated before calling this method.</summary>
			private Dictionary<string,string> GatewayParams {
				get {
					//Always add shared params at start. These parameters are shared for both OTK creation and Hpf status monitoring.
					Dictionary<string,string> ret=new Dictionary<string,string>();
					ret.Add("SpecVersion",XWebSpecVersion);
					if(UseTestAccountOD) { //OD test account.
						ret.Add("XWebID","800000000894");
						ret.Add("AuthKey","8RLVqcxRCTpH7bu9bjMkY1uLEiyWGsan");
						ret.Add("TerminalID","80023128");
					}
					else if(UseTestAccountLoopback) { //XWeb loopback test account. For testing different DTG responses (see appendix D of DTG doc).
						ret.Add("XWebID","800000000894");
						ret.Add("AuthKey","t4mkUvKWQ12N2DNGSoKBMvOVNLuE1LFm");
						ret.Add("TerminalID","80023450");
					}
					else { //Production.
						ret.Add("XWebID",_xWebID.ToString());
						ret.Add("AuthKey",_authKey.ToString());
						ret.Add("TerminalID",_terminalID.ToString());
					}
					//OTK status polling does not require a specific transaction type. All other transaction types must be specified in the params.
					if(_transactionType!=XWebTransactionType.PollOtkUnspecified) {
						ret.Add("TransactionType",_transactionType.ToString());
					}
					//Add transaction specific params.
					ExtraGatewayParams.ToList().ForEach(x => ret[x.Key]=x.Value);
					return ret;
				}
			}
			///<summary>Extra key/value pairs specific to a given transmission.</summary>
			protected abstract Dictionary<string,string> ExtraGatewayParams { get; }
			///<summary>Must either be XWeb3.6 (DTG) or XWebSecure3.6 (HPF).</summary>
			protected virtual string XWebSpecVersion { get { return "XWebSecure3.6"; } }
			///<summary>Inidicates that calling GenerateOutput should in-turn cause WakeupMonitor event to be raised.</summary>
			public virtual bool WakeupMonitorThread { get { return true; } }
			///<summary>Inidicates that calling GenerateOutput should in-turn cause new row to be added to XWebResponse.</summary>
			public virtual bool InsertResponseIntoDb { get { return true; } }
			///<summary>Interface the XWeb Gateway and return an instance of XWebResponse. Goes to db and/or cache to get patient info and ProgramProperties for XWeb.</summary>
			public XWebResponse GenerateOutput() {
				Patient pat=OpenDentBusiness.Patients.GetPat(_patNum);
				if(pat==null) {
					throw new ODException("Patient not found for PatNum: "+_patNum.ToString(),ODException.ErrorCodes.XWebProgramProperties);
				}
				_patNum=pat.PatNum;
				_provNum=pat.PriProv;
				//Explicitly set ClinicNum=0, since a pat's ClinicNum will remain set if the user enabled clinics, assigned patients to clinics, and then
				//disabled clinics because we use the ClinicNum to determine which PayConnect or XCharge/XWeb credentials to use for payments.
				_clinicNum=0;
				if(PrefC.HasClinicsEnabled) {
					_clinicNum=pat.ClinicNum;
				}
				if(!OpenDentBusiness.PrefC.HasClinicsEnabled) { //Patient.ClinicNum is unreliable if clinics have been turned off.
					_clinicNum=0;
				}
				bool paymentsAllowed; long paymentTypeDefNum;
				ProgramProperties.GetXWebCreds(_clinicNum,out paymentsAllowed,out _xWebID,out _authKey,out _terminalID,out paymentTypeDefNum);
				if(!paymentsAllowed) {
					throw new ODException("Clinic or Practice has online payments disabled",ODException.ErrorCodes.XWebProgramProperties);
				}
				XWebResponse response=CreateGatewayResponse(UploadData(GatewayInput));
				response.PatNum=_patNum;
				response.ProvNum=_provNum;
				response.ClinicNum=_clinicNum;
				response.DateTUpdate=DateTime.Now;
				response.TransactionType=_transactionType.ToString();
				response.TransactionStatus=XWebTransactionStatus.HpfPending;
				PostProcessOutput(response);
				if(InsertResponseIntoDb) {
					XWebResponses.Insert(response);
				}
				if(WakeupMonitorThread) {
					OnWakeupMonitor(response,new EventArgs());
				}
				return response;
			}
			///<summary>Once XWeb Gateway response had been generated, the inheritor of this class may process it and add it to by overriding this method.</summary>
			protected abstract void PostProcessOutput(XWebResponse response);
			///<summary>Convert output (in xml) from the XWeb gateway to GatewayResponse.</summary>
			private static XWebResponse CreateGatewayResponse(string xml) {
				using(StringReader sr = new StringReader(xml)) {
					//XWeb's xml references this class as GatewayResponse but OD wants to deserialize to a class called XWebResponse. 
					//We must explicitly specific the XmlRoot node name here to make that conversion.	
					XWebResponse ret=(XWebResponse)(new XmlSerializer(typeof(XWebResponse),new XmlRootAttribute("GatewayResponse")).Deserialize(sr));
					//Convert int to XWebStatus.
					ret.XWebResponseCode=XWebResponse.ConvertResponseCode(ret.ResponseCode);
					ret.AccountExpirationDate=XWebResponse.ConvertExpDate(ret.ExpDate);
					return ret;
				}
			}

			///<summary>Create a temporary WebClient connected to the XWeb gateway and perform UplodateData.</summary>
			/// <returns>The response from the url given the input.</returns>
			protected string UploadData(string input) {
				byte[] inputBytes=Encoding.ASCII.GetBytes(input);
				if(inputBytes.Length>=2048) {
					throw new ODException("X-Web gateway request is too long.",ODException.ErrorCodes.MaxRequestDataExceeded);
				}
				OnInputEvent(this,PrettyPrintXml(input));
				//Create HTTPS connection to X-Web gateway and send GET request.
				using(WebClient webClient = new WebClient()) {
					//Upload the XML request to the X-Web gateway and retrieve the response string.
					byte[] response=webClient.UploadData(_gatewayUrl,inputBytes);
					//Convert the byte array to a string for parsing.
					string ret=webClient.Encoding.GetString(response);
					OnOutputEvent(this,PrettyPrintXml(ret));
					return ret;
				}
			}

			///<summary>Return formatted xml string.</summary>
			private static string PrettyPrintXml(string unformatted) {
				try {
					return XDocument.Parse(unformatted).ToString();
				}
				catch {
					return unformatted;
				}
			}

			public XWebInputAbs(XWebTransactionType transactionType,long patNum) {
				_transactionType=transactionType;
				_patNum=patNum;
			}
		}

		///<summary>Interface XWeb API to poll for an existing OTK's status.</summary>
		private class XWebInputOtkStatus:XWebInputAbs {
			///<summary>Only used for hpf status polling. The previously generated one time key belonging to the transaction being polled.</summary>
			private string _otk;
			///<summary>Only used for hpf status polling. Indicates if the call should block until a status is available. True=block, False=poll.</summary>
			private bool _blockUntilResponse=false;
			///<summary>Helper method that returns all of our customized parameters for the HPF status transaction request.  OTKs will be valid up to 24 hours for polling.  This method assumes IsXwebReady() was called prior to getting called.  Throws exceptions.</summary>
			protected override Dictionary<string,string> ExtraGatewayParams {
				get {
					Dictionary<string,string> ret=new Dictionary<string, string>();
					ret.Add("OTK",_otk);
					if(_blockUntilResponse) { //Make blocking call.
											  //Status call will wait to send a response until an Approval, HPF timeout, or another termination event has occurred. 
											  //Once the transaction is complete, a response code will be sent back to the calling application.
						ret.Add("ResponseMode","PERSIST_UNTIL_COMPLETE");
					}
					else { //Make non-blocking call.
						   //POLL events should have an interval of at least 5 seconds between them. 
						   //A final response (000, 101) can then be retrieved and is available for up to 24 hours using the OTK.
						ret.Add("ResponseMode","POLL");
					}
					return ret;
				}
			}
			///<summary>Calling GenerateOutput should NOT cause WakeupMonitor event to be raised.</summary>
			public override bool WakeupMonitorThread { get { return false; } }

			///<summary>Calling GenerateOutput should NOT cause new row to be added to XWebResponse.</summary>
			public override bool InsertResponseIntoDb { get { return false; } }

			///<summary>No special processing for XWebInputOtkStatus.</summary>
			protected override void PostProcessOutput(XWebResponse response) { }

			public XWebInputOtkStatus(long patNum,string otk,bool blockUntilResponse) : base(XWebTransactionType.PollOtkUnspecified,patNum) {
				_otk=otk;
				_blockUntilResponse=blockUntilResponse;
			}
		}

		///<summary>Interface XWeb API to generate an OTK and an HPF to be used to create a credit card alias which can be stored and used later.</summary>
		private class XWebInputHpfCreditCardAlias:XWebInputHpf {
			public XWebInputHpfCreditCardAlias(long patNum,bool isMobile) : base(XWebTransactionType.AliasCreateTransaction,patNum,isMobile) {
			}

			///<summary>No extra params needed for CC alias.</summary>
			protected override Dictionary<string,string> ExtraHPFParams { get { return new Dictionary<string, string>(); } }
		}

		///<summary>Interface XWeb API to generate an OTK and an HPF to be used to make a payment.</summary>
		private class XWebInputHpfPayment:XWebInputHpf {
			///<summary>Only used for hpf generating. Amount length can be 1-7  digits.  E.g. 0.01 - 99999.99.  Pass in 0 to let the user decide what the amount should be.</summary>
			private double _amount;
			///<summary>Will be entered as Payment.PayNote once payment transaction has completed.</summary>
			private string _payNote;
			///<summary>Indicates if the CC transaction should create and return a reusable credit card alias.</summary>
			private bool _createAlias;
			///<summary>Indicates the place where this transaction came from.</summary>
			private CreditCardSource _ccSource;
			///<summary>Returns dictionary of all parameters needed for an OTK request.  Throws exceptions.</summary>
			protected override Dictionary<string,string> ExtraHPFParams {
				get {
					Dictionary<string,string> ret=new Dictionary<string, string>();
					//Allows duplicate transactions within 15 minutes of eachother.
					ret.Add("DuplicateMode","CHECKING_OFF");
					//Return a new CC alias.
					ret.Add("CreateAlias",_createAlias ? "TRUE" : "FALSE");
					//dictGatewayParams.Add("Industry","RETAIL");
					if(_amount>0) {//If a valid amount was passed in, set it and lock it down.  Otherwise, leave it up to the user to decide how much they want to pay.									   
								   //Amount length can be 1-7  digits.  E.g. 0.01 - 99999.99 (decimal point not required for even dollar amounts).
						ret.Add("Amount",POut.Double(_amount));
						//Locks the amount field so that the patient cannot change it.
						ret.Add("AmountLocked","TRUE");
						//No partial payments. Payment will either be full approved or declined.
						ret.Add("EnablePartialApprovals","FALSE");
					}
					return ret;
				}
			}

			///<summary>Adds amount to response.</summary>
			protected override void PostProcessOutput(XWebResponse response) {
				base.PostProcessOutput(response);
				response.Amount=_amount;
				response.PayNote=_payNote;
				response.CCSource=_ccSource;
			}

			public XWebInputHpfPayment(long patNum,bool isMobile,double amount,string payNote,bool createAlias,CreditCardSource ccSource) 
				: base(XWebTransactionType.CreditSaleTransaction,patNum,isMobile) {
				_amount=amount;
				_payNote=payNote;
				_createAlias=createAlias;
				_ccSource=ccSource;
				if(_ccSource!=CreditCardSource.XWeb && _ccSource!=CreditCardSource.XWebPortalLogin) {
					throw new ODException("Invalid CreditCardSource: "+_ccSource.ToString(),ODException.ErrorCodes.OtkArgsInvalid);
				}
				//Validate the amount.
				if(_amount<0.00||_amount>99999.99) {
					throw new ODException("Invalid Amount",ODException.ErrorCodes.OtkArgsInvalid);
				}
				if(string.IsNullOrEmpty(_payNote)) {
					throw new ODException("Invalid PayNote",ODException.ErrorCodes.OtkArgsInvalid);
				}
			}
		}

		///<summary>Interface XWeb API to generate an OTK and an HPF.</summary>
		private abstract class XWebInputHpf:XWebInputAbs {
			///<summary>Determines which Hpf form url will be used.</summary>
			private bool _isMobile=false;
			///<summary>Amount of time allowed between OTK creation and timeout being returned by HPF status polling.</summary>
			private TimeSpan _formTimeout=TimeSpan.FromMinutes(10);
			///<summary>Returns the URL for the XWeb form based on debug and mobile prefs.</summary>
			private string _hpfUrlBase {
				get {
					if(_isMobile) {
						if(UseXWebTestGateway) {
							return "https://integrator.t3secure.net/hpf/hpfmobile.aspx";//X-Charge Test Mobile Form URL 
						}
						else {
							return "https://online.t3secure.net/hpf/hpfmobile.aspx";//X-Charge Production Mobile Form URL 
						}
					}
					else {
						if(UseXWebTestGateway) {
							return "https://integrator.t3secure.net/hpf/hpf.aspx";//X-Charge Test Form URL
						}
						else {
							return "https://online.t3secure.net/hpf/hpf.aspx";//X-Charge Production Form URL
						}
					}
				}
			}
			///<summary>Helper method that returns all of our customized parameters for the HPF URL creation.  These parameters will determine how the HPF will display to the end user.</summary>
			private static Dictionary<string,string> QueryStringArgs {
				get {
					Dictionary<string,string> dictHpfParams=new Dictionary<string,string>();
					dictHpfParams.Add("Background-Color","RGB(241,241,241)");
					dictHpfParams.Add("Errors-Font-Color","Red");
					dictHpfParams.Add("Font-Color-Color","Black");
					dictHpfParams.Add("Font-Size","16px");
					dictHpfParams.Add("Font-Family","Arial, Courier, Tahoma");
					dictHpfParams.Add("StyleButtons-Color","ON");
					dictHpfParams.Add("AutoSubmit","ON");
					dictHpfParams.Add("SubmitKeyCode","119");
					dictHpfParams.Add("ClearKeyCode","120");
					dictHpfParams.Add("VisualIndicator","ON");
					dictHpfParams.Add("XWebSecureLogo","ON");
					dictHpfParams.Add("Divider","ON");
					return dictHpfParams;
				}
			}
			///<summary>Extra key/value pairs specific to a given XWebInputHpf transmission.</summary>
			protected abstract Dictionary<string,string> ExtraHPFParams { get; }
			///<summary>Returns dictionary of all parameters needed for an OTK request.</summary>
			protected override Dictionary<string,string> ExtraGatewayParams {
				get {
					Dictionary<string,string> ret=new Dictionary<string, string>();
					//This field is used to specify the time, in minutes, that the HPF will be available for processing transactions. 
					//After this time elapses the HPF will return a Transaction Time Exceeded error. 
					//The minimum (and default) is 5 minutes, and the maximum is 30 minutes.
					//If requesting HPF status using PERSIST_UNTIL_COMPLETE, then when this timeout is reached, the status request will return 101 (Expired without Approval).
					ret.Add("FormTimeout",((int)_formTimeout.TotalMinutes).ToString());
					//Redirect to a static page.
					ret.Add("SuccessURL","https://www.patientviewer.com/payok.html");	
					//So that XWeb doesn't try to look for a card reader. 
					ret.Add("DeviceType","NONE");//According to XCharge Integration Specialist emailed to CM, this is required otherwise defaults to "EMV"
					ExtraHPFParams.ToList().ForEach(x => ret[x.Key]=x.Value);
					return ret;
				}
			}

			public XWebInputHpf(XWebTransactionType transactionType,long patNum,bool isMobile) : base(transactionType,patNum) {
				_isMobile=isMobile;
			}

			///<summary>Verifies OTK creation and creates HpfUrl.</summary>
			protected override void PostProcessOutput(XWebResponse response) {
				//Create a One Time Key (OTK) for the desired amount.
				if(response.XWebResponseCode!=XWebResponseCodes.OtkSuccess) {
					throw new ODException("OTK creation failed: "+response.ResponseDescription);
				}
				//Build the HPF URL that will be used by the browser.
				response.HpfUrl=_hpfUrlBase+"?otk="+response.OTK;
				Dictionary<string,string> dictHpfParams=QueryStringArgs;
				foreach(KeyValuePair<string,string> param in dictHpfParams) {
					response.HpfUrl+="&"+param.Key+"="+param.Value;
				}
				//Get the status of the OTK we just created.
				XWebResponseCodes status=new XWebInputOtkStatus(_patNum,response.OTK,false).GenerateOutput().XWebResponseCode;
				if(status!=XWebResponseCodes.Pending) { //Should be pending. If not, there is a problem.
					throw new ODException("New OTK status invalid: "+status.ToString());
				}
				response.HpfExpiration=DateTime.Now.Add(_formTimeout);
			}
		}

		///<summary>Interface XWeb API to make a payment, void payment, or return payment.</summary>
		private abstract class XWebInputDTG:XWebInputAbs {
			///<summary>Will be entered as Payment.PayNote once payment transaction has completed.</summary>
			protected string _payNote;
			///<summary>DTG does not use the secure gateway.</summary>
			protected override string XWebSpecVersion { get { return "XWeb3.6"; } }
			///<summary>Calling GenerateOutput should NOT cause WakeupMonitor event to be raised.</summary>
			public override bool WakeupMonitorThread { get { return false; } }
			///<summary>Extra key/value pairs specific to a given XWebInputDTG transmission.</summary>
			protected abstract Dictionary<string,string> ExtraDTGParams { get; }
			///<summary>Provide key/value pairs for the XWeb DTG transmission.</summary>
			protected override Dictionary<string,string> ExtraGatewayParams {
				get {
					Dictionary<string,string> ret=new Dictionary<string, string>();
					//Required but is always NONE in this case.
					ret.Add("TrackCapabilities","NONE");
					//Required but is always false in this case.
					ret.Add("PinCapabilities","FALSE");
					//Payment is being initiated from a PC (as opposed to TERMINAL or ECR).
					ret.Add("POSType","PC");
					ExtraDTGParams.ToList().ForEach(x => ret[x.Key]=x.Value);
					return ret;
				}
			}
			protected abstract XWebTransactionStatus ApprovalStatus { get; }

			public XWebInputDTG(XWebTransactionType transactionType,long patNum,string payNote) : base(transactionType,patNum) {
				_payNote=payNote;
				if(string.IsNullOrEmpty(_payNote)) {
					throw new ODException("Invalid PayNote",ODException.ErrorCodes.OtkArgsInvalid);
				}
			}

			///<summary>Verifies approval and sets response.PatNote.</summary>
			protected override void PostProcessOutput(XWebResponse response) {
				if(response.XWebResponseCode!=XWebResponseCodes.Approval && response.XWebResponseCode!=XWebResponseCodes.AliasSuccess) {
					throw new ODException("DTG failed ("+response.ResponseCode.ToString()+"): "+response.ResponseDescription);
				}
				response.PayNote=_payNote;
				response.TransactionStatus=ApprovalStatus;
			}
		}

		///<summary>Interface XWeb API to make a payment for the given amount using the given pre-authorized credit card alias. 
		///Makes the call directly to the XWeb gateway (DTG) and does not involve HPF.</summary>
		private class XWebInputDTGPaymentSale:XWebInputDTG {
			///<summary>Amount to be charged for this transaction. Amount length can be 1-7  digits.  E.g. 0.01 - 99999.99.</summary>
			protected double _amount;
			///<summary>Pre-authorized credit card to be used for this transaction. Must have previously been created by the user via AliasCreateTransaction.</summary>
			protected CreditCard _cc=null;
			///<summary>Provide key/value pairs for the XWeb transmission.</summary>
			protected override Dictionary<string,string> ExtraDTGParams {
				get {
					Dictionary<string,string> ret=new Dictionary<string, string>();
					//In order to make a DTG payment you MUST provide one (and only one) of the following sets of information about the cc...
					//Track data, AcctNum & ExpDate, EncryptedData, Alias, OrderID, or DeviceEncryptedData.
					//We will use Alias so all other forms of credit card identification are NOT required.
					ret.Add("Alias",_cc.XChargeToken);
					//Amount is required for DTG alias payment.
					ret.Add("Amount",POut.Double(_amount));
					//No partial payments. Payment will either be full approved or declined.
					ret.Add("EnablePartialApprovals","FALSE");
					//Allows duplicate transactions within 15 minutes of eachother.
					ret.Add("DuplicateMode","CHECKING_OFF");
					//Required but is always false in this case.
					ret.Add("CustomerPresent","FALSE");
					//Required but is always false in this case.
					ret.Add("CardPresent","FALSE");
					return ret;
				}
			}
			protected override XWebTransactionStatus ApprovalStatus { get { return XWebTransactionStatus.DtgPaymentApproved; } }
			protected virtual bool InsertPaymentOnApproval {
				get {
					return _insertPaymentOnApproval;
				}
				set {
					_insertPaymentOnApproval=value;
				}
			}
			protected virtual bool InsertPositivePayment { get { return true; } }
			///<summary>Default to true.</summary>
			private bool _insertPaymentOnApproval=true;

			public XWebInputDTGPaymentSale(long patNum,string payNote,double amount,long creditCardNum) : this(patNum,payNote,amount,creditCardNum,XWebTransactionType.CreditSaleTransaction) {
			}

			protected XWebInputDTGPaymentSale(long patNum,string payNote,double amount,long creditCardNum,XWebTransactionType transactionType) : base(transactionType,patNum,payNote) {
				_amount=amount;
				if(_amount<=0.00||_amount>99999.99) {
					throw new ODException("Invalid Amount",ODException.ErrorCodes.OtkArgsInvalid);
				}
				CreditCard cc=CreditCards.GetOne(creditCardNum);
				if(cc==null) {
					throw new ODException("CreditCardNum not found: "+creditCardNum.ToString(),ODException.ErrorCodes.OtkArgsInvalid);
				}
				if(cc.PatNum!=_patNum) {
					throw new ODException("Credit card token does not belong to this patient. CreditCardNum: "+creditCardNum.ToString()+" - PatNum: "+_patNum.ToString()+".",ODException.ErrorCodes.OtkArgsInvalid);
				}
				if(string.IsNullOrEmpty(cc.XChargeToken)) {
					throw new ODException("Invalid CC Alias",ODException.ErrorCodes.OtkArgsInvalid);
				}
				_cc=cc;
			}

			///<summary>Performs base XWebInputDTGForPayment behavior and creates PaymentWeb row.</summary>
			protected override void PostProcessOutput(XWebResponse response) {
				//Verify result and set response.PayNote.
				base.PostProcessOutput(response);
				response.Alias=_cc.XChargeToken;
				if(InsertPaymentOnApproval) { //Insert Payment, PaySplit, and set FK.					
					response.PaymentNum=Payments.InsertFromXWeb(
						_patNum,_provNum,_clinicNum,
						(InsertPositivePayment ? _amount : -_amount),
						response.GetFormattedNote(InsertPositivePayment),"",CreditCardSource.XWeb);//todo: create a formatted receipt to show the web user after the payment has been accepted
				}
				//XWeb's Decline Minimizer will pass us back updated card information. Update our copy when necessary.
				bool update=false;
				if(_cc.CCExpiration!=response.AccountExpirationDate) {
					_cc.CCExpiration=response.AccountExpirationDate;
					update=true;
				}
				if(_cc.CCNumberMasked!=response.MaskedAcctNum) {
					_cc.CCNumberMasked=response.MaskedAcctNum;
					update=true;
				}
				if(update) {
					CreditCards.Update(_cc);
				}
			}
		}

		///<summary>Interface XWeb API to void a payment. Makes the call directly to the XWeb gateway (DTG) and does not involve HPF.</summary>
		private class XWebInputDTGPaymentVoid:XWebInputDTG {
			///<summary>Pre-authorized credit card alias to be used for this transaction. Must have previously been created by the user via AliasCreateTransaction.</summary>
			protected string _transactionID="";
			///<summary>Provide key/value pairs for the XWeb transmission.</summary>
			protected override Dictionary<string,string> ExtraDTGParams {
				get {
					Dictionary<string,string> ret=new Dictionary<string, string>();
					//From the original CreditSaleTransaction row.
					ret.Add("TransactionID",_transactionID);
					return ret;
				}
			}
			protected override XWebTransactionStatus ApprovalStatus { get { return XWebTransactionStatus.DtgPaymentVoided; } }
			protected bool _insertPositivePayment=true;

			public XWebInputDTGPaymentVoid(long patNum,string payNote,long xWebResponseNum) : base(XWebTransactionType.CreditVoidTransaction,patNum,payNote) {
				XWebResponse xwr=XWebResponses.GetOne(xWebResponseNum);
				if(xwr==null) {
					throw new ODException("XWebResponseNum not found: "+xWebResponseNum.ToString(),ODException.ErrorCodes.OtkArgsInvalid);
				}
				if(xwr.PatNum!=_patNum) {
					throw new ODException("XWebResponse does not belong to this patient. XWebResponseNum: "+xWebResponseNum.ToString()+" - PatNum: "+_patNum.ToString()+".",ODException.ErrorCodes.OtkArgsInvalid);
				}
				//We currently only support CreditSaleTransaction and CreditReturnTransaction. 
				switch(xwr.XTransactionType) {
					case XWebTransactionType.CreditSaleTransaction:
						_insertPositivePayment=false;
						break;
					case XWebTransactionType.CreditReturnTransaction:
						_insertPositivePayment=true;
						break;
					default:
						throw new ODException("Voiding invalid transaction type: "+xwr.TransactionType,ODException.ErrorCodes.OtkArgsInvalid);
				}
				_transactionID=xwr.TransactionID;
				if(string.IsNullOrEmpty(_transactionID)) {
					throw new ODException("Invalid TransactionID",ODException.ErrorCodes.OtkArgsInvalid);
				}
			}

			protected override void PostProcessOutput(XWebResponse response) {
				//Verify result and set response.PayNote.
				base.PostProcessOutput(response);
				//Insert Payment, PaySplit, and set FK.
				response.PaymentNum=Payments.InsertFromXWeb(
					_patNum,_provNum,_clinicNum,
					(_insertPositivePayment ? response.Amount : -response.Amount),
					response.GetFormattedNote(_insertPositivePayment),"",CreditCardSource.XWeb);
			}
		}

		///<summary>Interface XWeb API to post a return to the given credit card. Makes the call directly to the XWeb gateway (DTG) and does not involve HPF.</summary>
		private class XWebInputDTGPaymentReturn:XWebInputDTGPaymentSale {
			protected override XWebTransactionStatus ApprovalStatus { get { return XWebTransactionStatus.DtgPaymentReturned; } }
			protected override bool InsertPositivePayment {	get {	return false; } }
			

			public XWebInputDTGPaymentReturn(long patNum,string payNote,double amount,long creditCardNum,bool createPayment) 
				: base(patNum,payNote,amount,creditCardNum,XWebTransactionType.CreditReturnTransaction) 
			{
				InsertPaymentOnApproval=createPayment;
			}
		}

		///<summary>Interface XWeb API to delete a credit card alias. Makes the call directly to the XWeb gateway (DTG) and does not involve HPF.</summary>
		private class XWebInputDTGDeleteAlias:XWebInputDTG {
			///<summary>Pre-authorized credit card to be deleted. Must have previously been created by the user via AliasCreateTransaction.</summary>
			protected CreditCard _cc=null;
			///<summary>Provide key/value pairs for the XWeb transmission.</summary>
			protected override Dictionary<string,string> ExtraDTGParams {
				get {
					Dictionary<string,string> ret=new Dictionary<string, string>();
					ret.Add("Alias",_cc.XChargeToken);
					return ret;
				}
			}
			protected override XWebTransactionStatus ApprovalStatus { get { return XWebTransactionStatus.DtgAliasDeleted; } }

			public XWebInputDTGDeleteAlias(long patNum,long creditCardNum) : base(XWebTransactionType.AliasDeleteTransaction,patNum,"Deleting CreditCard: "+creditCardNum.ToString()) {
				CreditCard cc=CreditCards.GetOne(creditCardNum);
				if(cc==null) {
					throw new ODException("CreditCardNum not found: "+creditCardNum.ToString(),ODException.ErrorCodes.OtkArgsInvalid);
				}
				if(cc.PatNum!=_patNum) {
					throw new ODException("Credit card token does not belong to this patient. CreditCardNum: "+creditCardNum.ToString()+" - PatNum: "+_patNum.ToString()+".",ODException.ErrorCodes.OtkArgsInvalid);
				}
				if(string.IsNullOrEmpty(cc.XChargeToken)) {
					throw new ODException("Invalid CC Alias",ODException.ErrorCodes.OtkArgsInvalid);
				}
				_cc=cc;
			}

			///<summary>Performs base XWebInputDTGForPayment behavior and deletes CreditCard row.</summary>
			protected override void PostProcessOutput(XWebResponse response) {
				//Verify result and set response.PayNote.
				base.PostProcessOutput(response);
				response.Alias=_cc.XChargeToken;
				try { response.PayNote="Deleted CreditCard: "+JsonConvert.SerializeObject(_cc); } catch { }				
				CreditCards.Delete(_cc.CreditCardNum);
				List<CreditCard> creditCards=CreditCards.Refresh(_patNum);
				for(int i=0;i<creditCards.Count;i++) {
					creditCards[i].ItemOrder=creditCards.Count-(i+1);
					CreditCards.Update(creditCards[i]);//Resets ItemOrder.
				}			
			}
		}
		#endregion
	}
}
