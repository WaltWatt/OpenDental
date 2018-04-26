using CodeBase;
using Newtonsoft.Json;
using OpenDentBusiness.WebTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebServiceSerializer;

namespace OpenDentBusiness {
	public class PaySimple:WebBase {

		public class PropertyDescs {
			public static string PaySimpleApiUserName="PaySimple API User Name";
			public static string PaySimpleApiKey="PaySimple API Key";
			public static string PaySimplePayType="PaySimple Payment Type";
		}

		///<summary>Throws exceptions.  Will purposefully throw ODExceptions that are already translated and in a pretty format.</summary>
		public static string GetCheckoutToken() {
			ValidateProgram();
			return PaySimpleApi.GetCheckoutToken(GetAuthHeader());
		}

		///<summary>Throws exceptions.  Will purposefully throw ODExceptions that are already translated and in a pretty format.</summary>
		public static string GetCustomerToken() {
			ValidateProgram();
			return PaySimpleApi.GetCustomerToken(GetAuthHeader(),"1234");
		}

		///<summary>Throws exceptions.  Will purposefully throw ODExceptions that are already translated and in a pretty format.</summary>
		public static long AddCustomer(string fname,string lname,string idInDb="") {
			ValidateProgram();
			return PaySimpleApi.PostCustomer(GetAuthHeader(),PaySimpleApi.MakeNewCustomerData(fname,lname,idInDb));
		}

		///<summary>Throws exceptions.  Will purposefully throw ODExceptions that are already translated and in a pretty format.</summary>
		public static long AddCreditCard(long customerId,string ccNum,DateTime ccExpDate,bool isDefault,string billingZipCode="") {
			ValidateProgram();
			if(customerId==0) {
				throw new ODException(Lans.g("PaySimple","Invalid PaySimple Customer ID provided: ")+customerId.ToString());
			}
			return PaySimpleApi.PostAccountCreditCard(GetAuthHeader(),PaySimpleApi.MakeNewAccountCreditCardData(customerId,ccNum,ccExpDate,PaySimpleApi.GetCardType(ccNum),isDefault,billingZipCode));
		}
		
		public static long AddCreditCardTest() {
			ValidateProgram();
			return AddCreditCard(453400,"4111111111111111",new DateTime(2021,12,1),false);
		}
		
		public static string MakePaymentTest() {
			ValidateProgram();
			return MakePayment(1,new CreditCard() {
				PatNum=104,
				PaySimpleToken="639958",
			},50,"4111111111111111",new DateTime(2021,12,1),false,true);
		}

		///<summary>Throws exceptions.  Will purposefully throw ODExceptions that are already translated and in a pretty format.
		///Returns the PaymentId given by PaySimple.</summary>
		public static string MakePayment(long patNum,CreditCard cc,decimal payAmt,string ccNum,DateTime ccExpDate,bool isDefault,bool saveToken,string billingZipCode="",string cvv="") {
			ValidateProgram();
			if((cc==null || string.IsNullOrWhiteSpace(cc.PaySimpleToken)) && (string.IsNullOrWhiteSpace(ccNum) || ccExpDate.Year<DateTime.Today.Year)) {
				throw new ODException(Lans.g("PaySimple","Error making payment"));
			}
			if(cc==null) {
				cc=new CreditCard() {
					PatNum=patNum,
					PaySimpleToken="",
				};
			}
			if(string.IsNullOrWhiteSpace(cc.PaySimpleToken)) {
				Patient patCur=Patients.GetPat(cc.PatNum);
				if(patCur==null) {
					patCur=new Patient() {
						PatNum=patNum,
						FName="",
						LName="",
					};
				}
				long psCustomerId=GetCustomerIdForPat(patCur.PatNum,patCur.FName,patCur.LName,true);
				cc.PaySimpleToken=POut.Long(AddCreditCard(psCustomerId,ccNum,ccExpDate,isDefault,billingZipCode));
				if(saveToken && cc.CreditCardNum>0) {//If the user doesn't want Open Dental to store their account id, we will let them continue entering their CC info.
					CreditCards.Update(cc);
				}
			}
			else {//cc is null, use ccNum/ccExpDate
				//TODO!
			}
			return PaySimpleApi.PostPayment(GetAuthHeader(),PaySimpleApi.MakeNewPaymentData(PIn.Long(cc.PaySimpleToken),payAmt,cvv));
		}

		///<summary>Throws exceptions.  Will purposefully throw ODExceptions that are already translated and in a pretty format.</summary>
		public static string VoidPayment(string paySimplePaymentId) {
			if(string.IsNullOrWhiteSpace(paySimplePaymentId)) {
				throw new Exception(Lans.g("PaySimple","Invalid PaySimple Payment ID to void."));
			}
			return PaySimpleApi.PutPaymentVoided(GetAuthHeader(),paySimplePaymentId);
		}

		///<summary>Throws exceptions.  Will purposefully throw ODExceptions that are already translated and in a pretty format.</summary>
		public static string ReversePayment(string paySimplePaymentId) {
			if(string.IsNullOrWhiteSpace(paySimplePaymentId)) {
				throw new Exception(Lans.g("PaySimple","Invalid PaySimple Payment ID to reverse."));
			}
			return PaySimpleApi.PutPaymentReversed(GetAuthHeader(),paySimplePaymentId);
		}

		private static string GetAuthHeader() {
#if DEBUG
			//string apiUserName="APIUser155356";
			//string apiKey="QkQRj8i0QDPOtUBhbTWx7irBrqospeY8RDC4HxW2LD3IDIfo1bcumTMomp7IJbYONjIna84QPwMwfFLMTtZcMJ2Bm4meQIfojgsDrZr5HxAnQkylHJgF7t2XUDoVy6I0";
			string apiUserName=ProgramProperties.GetPropValForClinicOrDefault(Programs.GetCur(ProgramName.PaySimple).ProgramNum
				,PropertyDescs.PaySimpleApiUserName
				,Clinics.ClinicNum);
			string apiKey=ProgramProperties.GetPropValForClinicOrDefault(Programs.GetCur(ProgramName.PaySimple).ProgramNum
				,PropertyDescs.PaySimpleApiKey
				,Clinics.ClinicNum);
#else
			string apiUserName=ProgramProperties.GetPropValForClinicOrDefault(Programs.GetCur(ProgramName.PaySimple).ProgramNum
				,PropertyDescs.PaySimpleApiUserName
				,Clinics.ClinicNum);
			string apiKey=ProgramProperties.GetPropValForClinicOrDefault(Programs.GetCur(ProgramName.PaySimple).ProgramNum
				,PropertyDescs.PaySimpleApiKey
				,Clinics.ClinicNum);
#endif
			return PaySimpleApi.GetAuthHeader(apiUserName,apiKey);
		}

		///<summary>Throws exceptions if the PaySimple program or program properties are not valid.
		///If this method doesn't throw an exception, everything is assumed to be valid.</summary>
		private static void ValidateProgram() {
			Program progPaySimple=Programs.GetCur(ProgramName.PaySimple);
			if(progPaySimple==null) {
				throw new ODException(Lans.g("PaySimple","PaySimple program does not exist in the database.  Please call support."));
			}
			if(!progPaySimple.Enabled) {
				throw new ODException(Lans.g("PaySimple","PaySimple is not enabled."));
			}
			string apiUserName=ProgramProperties.GetPropValForClinicOrDefault(Programs.GetCur(ProgramName.PaySimple).ProgramNum
				,PropertyDescs.PaySimpleApiUserName
				,Clinics.ClinicNum);
			string apiKey=ProgramProperties.GetPropValForClinicOrDefault(Programs.GetCur(ProgramName.PaySimple).ProgramNum
				,PropertyDescs.PaySimpleApiKey
				,Clinics.ClinicNum);
			if(string.IsNullOrWhiteSpace(apiUserName) || string.IsNullOrWhiteSpace(apiKey)) {
				throw new ODException(Lans.g("PaySimple","PaySimple Username or Key is empty."));
			}
		}

		///<summary>Returns the CustomerId that PaySimple gave us for the given patNum, or 0 if there wasn't a CustomerId found.</summary>
		public static long GetCustomerIdForPat(long patNum) {
			return GetCustomerIdForPat(patNum,"","",false);
		}

		///<summary>Returns the CustomerId that PaySimple gave us for the given patNum.
		///If addToPaySimpleIfMissing, the PaySimple API will add the given patNum if a link isn't in our database and return the new CustomerId.
		///Otherwise, it will return 0.</summary>
		public static long GetCustomerIdForPat(long patNum,string fname,string lname,bool addToPaySimpleIfMissing) {
			long psCustomerId=PatientLinks.GetPatNumsLinkedFrom(patNum,PatientLinkType.PaySimple).FirstOrDefault();
			if(psCustomerId==0 && addToPaySimpleIfMissing) {//Patient doesn't have a PaySimpleCustomerId
				psCustomerId=AddCustomer(fname,lname,(patNum>0 ? "PatNum: "+patNum.ToString() : ""));
				PatientLinks.Insert(new PatientLink() {
					PatNumFrom=patNum,
					PatNumTo=psCustomerId,
					LinkType=PatientLinkType.PaySimple,
				});
			}
			return psCustomerId;
		}

		private class PaySimpleApi {

			///<summary>Throws exceptions for http codes of 300 or more from API call.</summary>
			public static string GetCheckoutToken(string authHeader) {
				var response=Request(ApiRoute.Token,HttpMethod.Post,authHeader,"{}",
					#region ResponseType Object
					new {
						Meta=new {
							Errors=new {
								ErrorCode="InvalidInput",
								ErrorMessages=new [] { new {
										Field="",
										Message="",
									}
								}
							},
							HttpStatus="",
							HttpStatusCode="",
							PagingDetails="",
						},
						Response=new {
							JwtToken="",
							Expiration="",
						}
					}
					#endregion
				);
				return response.Response.JwtToken;
			}

			///<summary>Throws exceptions for http codes of 300 or more from API call.</summary>
			public static string GetCustomerToken(string authHeader,string paySimpleCustID) {
				var response=Request(ApiRoute.CustomerToken,HttpMethod.Get,authHeader,"",
					#region ResponseType Object
					new {
						Meta=new {
							Errors=new {
								ErrorCode="InvalidInput",
								ErrorMessages=new [] { new {
										Field="",
										Message="",
									}
								}
							},
							HttpStatus="",
							HttpStatusCode="",
							PagingDetails="",
						},
						Response=new {
							JwtToken="",
						}
					}
					#endregion
					,paySimpleCustID
				);
				return response.Response.JwtToken;
			}

			///<summary>Throws exceptions for http codes of 300 or more from API call.</summary>
			public static long PostCustomer(string authHeader,string postData) {
				var response=Request(ApiRoute.Customer,HttpMethod.Post,authHeader,postData,
					#region ResponseType Object
					new {
						Meta=new {
							Errors=new {
								ErrorCode="InvalidInput",
								ErrorMessages=new [] { new {
										Field="",
										Message="",
									}
								}
							},
							HttpStatus="",
							HttpStatusCode="",
							PagingDetails="",
						},
						Response=new {
							MiddleName="",
							AltEmail="",
							AltPhone="",
							MobilePhone="",
							Fax="",
							Website="",
							BillingAddress="",
							ShippingSameAsBilling=true,
							ShippingAddress="",
							Company="",
							Notes="",
							CustomerAccount="",
							FirstName="",
							LastName="",
							Email="",
							Phone="",
							Id=(long)0,
							LastModified="",
							CreatedOn="",
						}
					}
					#endregion
				);
				return response.Response.Id;
			}

			///<summary>Throws exceptions for http codes of 300 or more from API call.</summary>
			public static long PostAccountCreditCard(string authHeader,string postData) {
				var response=Request(ApiRoute.AccountCreditCard,HttpMethod.Post,authHeader,postData,
					#region ResponseType Object
					new {
						Meta=new {
							Errors=new {
								ErrorCode="InvalidInput",
								ErrorMessages=new [] { new {
										Field="",
										Message="",
									}
								}
							},
							HttpStatus="",
							HttpStatusCode="",
							PagingDetails="",
						},
						Response=new {
							CreditCardNumber="",
							ExpirationDate="",
							Issuer="",
							BillingZipCode="",
							CustomerId=(long)0,
							IsDefault=true,
							Id=395560,
							LastModified="",
							CreatedOn=""
						}
					}
					#endregion
				);
				return response.Response.Id;
			}

			///<summary>Throws exceptions for http codes of 300 or more from API call.</summary>
			public static string PostPayment(string authHeader,string postData) {
				var response=Request(ApiRoute.Payment,HttpMethod.Post,authHeader,postData,
					#region ResponseType Object
					new {
						Meta=new {
							Errors=new {
								ErrorCode="InvalidInput",
								ErrorMessages=new [] { new {
										Field="",
										Message="",
									}
								}
							},
							HttpStatus="",
							HttpStatusCode="",
							PagingDetails="",
						},
						Response=new {
							CustomerId=(long)0,
							CustomerFirstName="",
							CustomerLastName="",
							CustomerCompany="",
							ReferenceId=(long)0,
							Status="Authorized",
							RecurringScheduleId=(long)0,
							PaymentType="CC",
							PaymentSubType="MOTO",
							ProviderAuthCode="Approved",
							TraceNumber="",
							PaymentDate="",
							ReturnDate="",
							EstimatedSettleDate="",
							ActualSettledDate="",
							CanVoidUntil="",
							FailureData=new {
								Code="",
								Description="",
								MerchantActionText=""
							},
							AccountId=(long)0,
							InvoiceId="",
							Amount=0.0,
							IsDebit=false,
							InvoiceNumber="123AB",//Can have alpha characters
							PurchaseOrderNumber="",
							OrderId="",
							Description="",
							Latitude="",
							Longitude="",
							SuccessReceiptOptions="",
							FailureReceiptOptions="",
							Id=(long)0,
							LastModified="",
							CreatedOn="",
						}
					}
					#endregion
				);
				return response.Response.Id.ToString();
			}

			///<summary>Throws exceptions for http codes of 300 or more from API call.</summary>
			public static string PutPaymentVoided(string authHeader,string paymentId) {
				var response=Request(ApiRoute.PaymentVoided,HttpMethod.Put,authHeader,"",
					#region ResponseType Object
					new {
						Meta=new {
							Errors=new {
								ErrorCode="InvalidInput",
								ErrorMessages=new [] { new {
										Field="",
										Message="",
									}
								}
							},
							HttpStatus="",
							HttpStatusCode="",
							PagingDetails="",
						},
						Response=new {
							CustomerId=(long)0,
							CustomerFirstName="",
							CustomerLastName="",
							CustomerCompany="",
							ReferenceId=(long)0,
							Status="Authorized",
							RecurringScheduleId=(long)0,
							PaymentType="CC",
							PaymentSubType="MOTO",
							ProviderAuthCode="Approved",
							TraceNumber="",
							PaymentDate="",
							ReturnDate="",
							EstimatedSettleDate="",
							ActualSettledDate="",
							CanVoidUntil="",
							FailureData=new {
								Code="",
								Description="",
								MerchantActionText=""
							},
							AccountId=(long)0,
							InvoiceId="",
							Amount=0.0,
							IsDebit=false,
							InvoiceNumber="123AB",//Can have alpha characters
							PurchaseOrderNumber="",
							OrderId="",
							Description="",
							Latitude="",
							Longitude="",
							SuccessReceiptOptions="",
							FailureReceiptOptions="",
							Id=(long)0,
							LastModified="",
							CreatedOn="",
						}
					}
					#endregion
					,paymentId
				);
				if(response.Response.Status!="Voided") {
					throw new ODException(Lans.g("PaySimple","Payment could not be voided.  Please try again."));
				}
				return response.Response.Id.ToString();
			}

			///<summary>Throws exceptions for http codes of 300 or more from API call.</summary>
			public static string PutPaymentReversed(string authHeader,string paymentId) {
				var response=Request(ApiRoute.PaymentReversed,HttpMethod.Put,authHeader,"",
					#region ResponseType Object
					new {
						Meta=new {
							Errors=new {
								ErrorCode="InvalidInput",
								ErrorMessages=new [] { new {
										Field="",
										Message="",
									}
								}
							},
							HttpStatus="",
							HttpStatusCode="",
							PagingDetails="",
						},
						Response=new {
							CustomerId=(long)0,
							CustomerFirstName="",
							CustomerLastName="",
							CustomerCompany="",
							ReferenceId=(long)0,
							Status="Authorized",
							RecurringScheduleId=(long)0,
							PaymentType="CC",
							PaymentSubType="MOTO",
							ProviderAuthCode="Approved",
							TraceNumber="",
							PaymentDate="",
							ReturnDate="",
							EstimatedSettleDate="",
							ActualSettledDate="",
							CanVoidUntil="",
							FailureData=new {
								Code="",
								Description="",
								MerchantActionText=""
							},
							AccountId=(long)0,
							InvoiceId="",
							Amount=0.0,
							IsDebit=false,
							InvoiceNumber="123AB",//Can have alpha characters
							PurchaseOrderNumber="",
							OrderId="",
							Description="",
							Latitude="",
							Longitude="",
							SuccessReceiptOptions="",
							FailureReceiptOptions="",
							Id=(long)0,
							LastModified="",
							CreatedOn="",
						}
					}
					#endregion
					,paymentId
				);
				if(response.Response.Status!="ReversePosted") {
					throw new ODException(Lans.g("PaySimple","Payment could not be reversed.  Please try again."));
				}
				return response.Response.Id.ToString();
			}

			///<summary>Throws exception if the response from the server returned an http code of 300 or greater.</summary>
			private static T Request<T>(ApiRoute route,HttpMethod method,string authHeader,string body,T responseType,string routeId="") {
				using(WebClient client=new WebClient()) {
					client.Headers[HttpRequestHeader.Accept]="application/json";
					client.Headers[HttpRequestHeader.ContentType]="application/json";
					client.Headers[HttpRequestHeader.Authorization]=authHeader;
					client.Encoding=UnicodeEncoding.UTF8;
					//Post with Authorization headers and a body comprised of a JSON serialized anonymous type.
					try {
						string res="";
						//Only GET and POST are supported currently.
						if(method==HttpMethod.Get) {
							res=client.DownloadString(GetApiUrl(route,routeId));
						}
						else if(method==HttpMethod.Post) {
							res=client.UploadString(GetApiUrl(route,routeId),HttpMethod.Post.Method,body);
						}
						else {//PUT
							res=client.UploadString(GetApiUrl(route,routeId),HttpMethod.Put.Method,body);
						}
						if((typeof(T)==typeof(string))) {//If user wants the entire json response as a string
							return (T)Convert.ChangeType(res,typeof(T));
						}
						else {
							return JsonConvert.DeserializeAnonymousType(res,responseType);
						}
					}
					catch(WebException wex) {
						string res=new StreamReader(((HttpWebResponse)wex.Response).GetResponseStream()).ReadToEnd();
						if(string.IsNullOrWhiteSpace(res)) {
							//The response didn't contain a body.  Through my limited testing, it only happens for 401 (Unauthorized) requests.
							if(wex.Response.GetType()==typeof(HttpWebResponse)) {
								HttpStatusCode statusCode=((HttpWebResponse)wex.Response).StatusCode;
								if(statusCode==HttpStatusCode.Unauthorized) {
									throw new ODException(Lans.g("PaySimple","Invalid PaySimple credentials.  Check your Username and Key and try again."));
								}
							}
						}
						else {
							HandleWebException(res);
						}
						string errorMsg=wex.Message+(string.IsNullOrWhiteSpace(res) ? "" : "\r\nRaw response:\r\n"+res);
						throw new Exception(errorMsg,wex);//If it got this far and haven't rethrown, simply throw the entire exception.
					}
					catch(Exception ex) {
						//WebClient returned an http status code >= 300
						ex.DoNothing();
						//For now, rethrow error and let whoever is expecting errors to handle them.
						//We may enhance this to care about codes at some point.
						throw;
					}
				}
			}

			private static void HandleWebException(string paySimpleErrorJson) {
				var errorObj=JsonConvert.DeserializeAnonymousType(paySimpleErrorJson,new {
					Meta=new {
						Errors=new {
							ErrorCode="InvalidInput",
							ErrorMessages=new[] {new {
									Field="",
									Message="",
								}
							}
						},
						HttpStatus="",
						HttpStatusCode=HttpStatusCode.NotFound,
						PagingDetails="",
					}
				});
				try {
					//Make assumptions that every response from PaySimple matches their API documentation schema.
					//If something fails we should default to the exception that was originally thrown in the calling method.
					var metaObj=errorObj.Meta;
					var errors=metaObj.Errors;
					StringBuilder strbError=new StringBuilder();
					strbError.AppendLine("PaySimple ErrorCode:  "+errors.ErrorCode);
					if(errors.ErrorMessages.Length>0) {
						strbError.AppendLine("PaySimple Error Message(s):");
						errors.ErrorMessages.ToList().ForEach(x => strbError.AppendLine(x.Message));
					}
					//Purposefully not Lans.g and throwing ODException.  I don't want this to look like a generic exception being caught.
					throw new ODException(strbError.ToString());
				}
				catch(ODException ex) {
					ex.DoNothing();
					throw;//Re-throw the stringbuilder above.
				}
				catch(Exception e) {
					e.DoNothing();//The calling method should throw if this doesn't.
				}
			}

			public static string MakeNewPaymentData(long accountId,decimal amt,string cvv="") {
				return JsonConvert.SerializeObject(new {
					//Required fields:
					AccountId=accountId,
					Amount=amt,
					//Optional fields:
					//IsDebit=true,
					CVV=cvv,
					//PaymentSubType="MOTO",
					//InvoiceId="",
					//InvoiceNumber="",
					//PurchaseOrderNumber = "",
					//OrderId = "",
					//Description = payment.PayNote,
					//Latitude="",
					//Longitude="",
					//SuccessReceiptOptions="",
					//SendToCustomer=false,//Dictates if a receipt gets emailed to the customer after the payment is made
					//SendToOtherAddresses="",//A specific email (or emails, surrounded in brackets separated by commas) to send the receipt to.
					//FailureReceiptOptions="",
					//SendToCustomer=false,
					//SendToOtherAddresses="",
				});
			}

			public static string MakeNewCustomerData(string fname,string lname,string idInDb="") {
				return JsonConvert.SerializeObject(new {
					//Required fields:
					FirstName=fname,
					LastName=lname,
					ShippingSameAsBilling=true,//Hardcoded because we don't support this
					//Optional fields:
					//BillingAddress=new {
					//	StreetAddress1="",
					//	StreetAddress2="",
					//	City="",
					//	StateCode="",
					//	ZipCode="",
					//	Country="",
					//},
					//ShippingAddress="",
					//Company="",
					//Notes="",
					CustomerAccount=idInDb,
					//Email="",
					//Phone="",
				});
			}

			public static string MakeNewAccountCreditCardData(long customerId,string ccNum,DateTime ccExpDate,int issuer,bool isDefault,string billingZipCode="") {
				return JsonConvert.SerializeObject(new {
					//Required fields:
					CustomerId=customerId,
					CreditCardNumber=ccNum,
					ExpirationDate=ccExpDate.ToString("MM/yyyy"),
					Issuer=issuer,
					IsDefault=isDefault,
					//Optional fields:
					BillingZipCode=billingZipCode,
				});
			}

			public static string GetAuthHeader(string apiUserName,string apiKey) {
				string nowAsString=DateTime.Now.ToString(@"yyyy-MM-ddTHH\:mm\:sszzz");//This matches the PaySimple documentation for C#.
				string hash="";
				Encoding encoding=Encoding.UTF8;
				using(System.Security.Cryptography.HMACSHA256 hmac = new System.Security.Cryptography.HMACSHA256(encoding.GetBytes(apiKey))) {
					byte[] hashBytes=hmac.ComputeHash(encoding.GetBytes(nowAsString));
					hash=Convert.ToBase64String(hashBytes);
				}
				return string.Format("PSSERVER AccessId = {0}; Timestamp = {1}; Signature = {2}",apiUserName,nowAsString,hash);
			}

			///<summary>Takes a credit card number and returns the issuer id from it to match PaySimple's expected values</summary>
			public static int GetCardType(string ccNum) {
				int retVal=0;
				string cardTypeName=CreditCardUtils.GetCardType(ccNum);
				if(cardTypeName=="VISA") {
					retVal=12;
				}
				else if(cardTypeName=="MASTERCARD") {
					retVal=13;
				}
				else if(cardTypeName=="AMEX") {
					retVal=14;
				}
				else if(cardTypeName=="DISCOVER") {
					retVal=15;
				}
				else {
					throw new Exception("Unsupported Card Type");
				}
				return retVal;
			}

			///<summary>Returns the full URL according to the route/route id given.</summary>
			private static string GetApiUrl(ApiRoute route,string routeId="") {
#if DEBUG
				string apiUrl="https://sandbox-api.paysimple.com/v4";
#else
				string apiUrl="https://sandbox-api.paysimple.com/v4";//TODO:  CHANGE THIS TO PRODUCTION
#endif
				switch(route) {
					case ApiRoute.Root:
						//Do nothing.  This is to allow someone to quickly grab the URL without having to make a copy+paste reference.
						break;
					case ApiRoute.Token:
						apiUrl+="/checkouttoken";
						break;
					case ApiRoute.Customer:
						apiUrl+="/customer";
						break;
					case ApiRoute.AccountCreditCard:
						apiUrl+="/account/creditcard";
						break;
					case ApiRoute.Payment:
						apiUrl+="/payment";
						break;
					case ApiRoute.PaymentReversed:
						apiUrl+="/payment/"+routeId+"/reverse";
						break;
					case ApiRoute.PaymentVoided:
						apiUrl+="/payment/"+routeId+"/void";
						break;
					case ApiRoute.CustomerToken:
						apiUrl+="/customer/"+routeId+"/token";
						break;
					default:
						break;
				}
				return apiUrl;
			}

			private enum ApiRoute {
				///<summary>Base URL, no route</summary>
				Root,
				///<summary>CheckoutToken</summary>
				Token,
				///<summary>Customer</summary>
				Customer,
				///<summary>Account/CreditCard</summary>
				AccountCreditCard,
				///<summary>Payment</summary>
				Payment,
				///<summary>Payment/PaymentId/Reverse</summary>
				PaymentReversed,
				///<summary>Payment/PaymentId/Void</summary>
				PaymentVoided,
				///<summary>Customer/Token</summary>
				CustomerToken,
			}
		}

		public enum TransType {
			///<summary>Used to make a payment.</summary>
			SALE,
			///<summary>Used to add a credit card.</summary>
			AUTH,
			///<summary>Used to reverse a payment.</summary>
			RETURN,
			///<summary>Used to cancel a payment.</summary>
			VOID,
		}
	}
}
