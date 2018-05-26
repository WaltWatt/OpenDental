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

		///<summary>Throws exceptions.  Will purposefully throw ODExceptions that are already translated and formatted.</summary>
		public static long AddCustomer(string fname,string lname,string idInDb="",long clinicNum=-1) {
			ValidateProgram(clinicNum);
			return PaySimpleApi.PostCustomer(GetAuthHeader(clinicNum),PaySimpleApi.MakeNewCustomerData(fname,lname,idInDb));
		}

		///<summary>Throws exceptions.  Will purposefully throw ODExceptions that are already translated and and formatted.</summary>
		public static ApiResponse AddCreditCard(long customerId,string ccNum,DateTime ccExpDate,string billingZipCode="",long clinicNum=-1) {
			ValidateProgram(clinicNum);
			if(customerId==0) {
				throw new ODException(Lans.g("PaySimple","Invalid PaySimple Customer ID provided: ")+customerId.ToString());
			}
			return PaySimpleApi.PostAccountCreditCard(GetAuthHeader(clinicNum),PaySimpleApi.MakeNewAccountCreditCardData(customerId,ccNum,ccExpDate,PaySimpleApi.GetCardType(ccNum),billingZipCode));
		}

		///<summary>Throws exceptions.  Will purposefully throw ODExceptions that are already translated and and formatted.
		///If PatNum is 0, we will make a one time payment for an UNKNOWN patient.  This is currently only intended for prepaid insurance cards.
		///Returns the PaymentId given by PaySimple.</summary>
		public static ApiResponse MakePayment(long patNum,CreditCard cc,decimal payAmt,string ccNum,DateTime ccExpDate,bool isOneTimePayment,string billingZipCode="",string cvv="",long clinicNum=-1) {
			ValidateProgram(clinicNum);
			if(patNum==0) {
				//MakePaymentNoPat will validate its credentials.
				return MakePaymentNoPat(payAmt,ccNum,ccExpDate,billingZipCode,cvv,clinicNum);
			}
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
				long psCustomerId=GetCustomerIdForPat(patCur.PatNum,patCur.FName,patCur.LName,clinicNum);
				ApiResponse apiResponse=AddCreditCard(psCustomerId,ccNum,ccExpDate,billingZipCode,clinicNum);
				cc.PaySimpleToken=apiResponse.PaySimpleToken;
				if(!isOneTimePayment && cc.CreditCardNum>0) {//If the user doesn't want Open Dental to store their account id, we will let them continue entering their CC info.
					CreditCards.Update(cc);
				}
			}
			return PaySimpleApi.PostPayment(GetAuthHeader(clinicNum),PaySimpleApi.MakeNewPaymentData(PIn.Long(cc.PaySimpleToken),payAmt,cvv));
		}

		///<summary>Throws exceptions.  Will purposefully throw ODExceptions that are already translated and and formatted.
		///Returns the PaymentId given by PaySimple.</summary>
		public static ApiResponse MakePaymentByToken(CreditCard cc,decimal payAmt,long clinicNum=-1) {
			ValidateProgram(clinicNum);
			if(cc==null || string.IsNullOrWhiteSpace(cc.PaySimpleToken)) {
				throw new ODException(Lans.g("PaySimple","Error making payment by token"));
			}
			return MakePayment(cc.PatNum,cc,payAmt,"",DateTime.MinValue,false,"","",clinicNum);
		}

		///<summary>Throws exceptions.  Will purposefully throw ODExceptions that are already translated and and formatted.</summary>
		public static ApiResponse VoidPayment(string paySimplePaymentId,long clinicNum=-1) {
			ValidateProgram(clinicNum);
			if(string.IsNullOrWhiteSpace(paySimplePaymentId)) {
				throw new Exception(Lans.g("PaySimple","Invalid PaySimple Payment ID to void."));
			}
			return PaySimpleApi.PutPaymentVoided(GetAuthHeader(clinicNum),paySimplePaymentId);
		}

		///<summary>Throws exceptions.  Will purposefully throw ODExceptions that are already translated and and formatted.</summary>
		public static ApiResponse ReversePayment(string paySimplePaymentId,long clinicNum=-1) {
			ValidateProgram(clinicNum);
			if(string.IsNullOrWhiteSpace(paySimplePaymentId)) {
				throw new Exception(Lans.g("PaySimple","Invalid PaySimple Payment ID to reverse."));
			}
			return PaySimpleApi.PutPaymentReversed(GetAuthHeader(clinicNum),paySimplePaymentId);
		}

		///<summary>Throws exceptions.  Will purposefully throw ODExceptions that are already translated and and formatted.
		///Returns the PaymentId given by PaySimple.</summary>
		private static ApiResponse MakePaymentNoPat(decimal payAmt,string ccNum,DateTime ccExpDate,string billingZipCode="",string cvv="",long clinicNum=-1) {
			ValidateProgram(clinicNum);
			if(string.IsNullOrWhiteSpace(ccNum) || ccExpDate.Year<DateTime.Today.Year) {
				throw new ODException(Lans.g("PaySimple","Error making payment"));
			}
			long psCustomerId=AddCustomer("UNKNOWN","UNKNOWN","",clinicNum);
			ApiResponse apiResponse=AddCreditCard(psCustomerId,ccNum,ccExpDate,billingZipCode);
			string accountId=apiResponse.PaySimpleToken;
			return PaySimpleApi.PostPayment(GetAuthHeader(clinicNum),PaySimpleApi.MakeNewPaymentData(PIn.Long(accountId),payAmt,cvv));
		}

		///<summary>Returns the Authorization header for the api call, using the passed in clinicNum if provided, otherwise uses the currently selected clinic.</summary>
		private static string GetAuthHeader(long clinicNum=-1) {
			if(clinicNum==-1) {
				clinicNum=Clinics.ClinicNum;
			}
			string apiUserName=ProgramProperties.GetPropValForClinicOrDefault(Programs.GetCur(ProgramName.PaySimple).ProgramNum
				,PropertyDescs.PaySimpleApiUserName
				,clinicNum);
			string apiKey=ProgramProperties.GetPropValForClinicOrDefault(Programs.GetCur(ProgramName.PaySimple).ProgramNum
				,PropertyDescs.PaySimpleApiKey
				,clinicNum);
#if DEBUG
			//string apiUserName="APIUser155356";
			//string apiKey="QkQRj8i0QDPOtUBhbTWx7irBrqospeY8RDC4HxW2LD3IDIfo1bcumTMomp7IJbYONjIna84QPwMwfFLMTtZcMJ2Bm4meQIfojgsDrZr5HxAnQkylHJgF7t2XUDoVy6I0";
#endif
			return PaySimpleApi.GetAuthHeader(apiUserName,apiKey);
		}

		///<summary>Throws exceptions if the PaySimple program or program properties are not valid.
		///If this method doesn't throw an exception, everything is assumed to be valid.</summary>
		private static void ValidateProgram(long clinicNum=-1) {
			if(clinicNum==-1) {
				clinicNum=Clinics.ClinicNum;
			}
			Program progPaySimple=Programs.GetCur(ProgramName.PaySimple);
			if(progPaySimple==null) {
				throw new ODException(Lans.g("PaySimple","PaySimple program does not exist in the database.  Please call support."));
			}
			if(!progPaySimple.Enabled) {
				throw new ODException(Lans.g("PaySimple","PaySimple is not enabled."));
			}
			string apiUserName=ProgramProperties.GetPropValForClinicOrDefault(progPaySimple.ProgramNum,PropertyDescs.PaySimpleApiUserName,clinicNum);
			string apiKey=ProgramProperties.GetPropValForClinicOrDefault(progPaySimple.ProgramNum,PropertyDescs.PaySimpleApiKey,clinicNum);
			string payType=ProgramProperties.GetPropValForClinicOrDefault(progPaySimple.ProgramNum,PropertyDescs.PaySimplePayType,clinicNum);
			if(string.IsNullOrWhiteSpace(apiUserName) || string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(payType)) {
				throw new ODException(Lans.g("PaySimple","PaySimple Username, Key, or PayType is empty."));
			}
		}
		
		///<summary>Returns the CustomerId that PaySimple gave us for the given patNum.
		///If addToPaySimpleIfMissing, the PaySimple API will add the given patNum if a link isn't in our database and return the new CustomerId.
		///Otherwise, it will return 0.</summary>
		public static long GetCustomerIdForPat(long patNum,string fname,string lname,long clinicNum=-1) {
			long psCustomerId=PatientLinks.GetPatNumsLinkedFrom(patNum,PatientLinkType.PaySimple).FirstOrDefault();
			if(psCustomerId==0) {//Patient doesn't have a PaySimpleCustomerId
				psCustomerId=AddCustomer(fname,lname,(patNum>0 ? "PatNum: "+patNum.ToString() : ""),clinicNum);
				PatientLinks.Insert(new PatientLink() {
					PatNumFrom=patNum,
					PatNumTo=psCustomerId,
					LinkType=PatientLinkType.PaySimple,
				});
			}
			return psCustomerId;
		}

		///<summary>OD response object for PaySimple API method responses.</summary>
		public class ApiResponse {
			///<summary>The transaction that was just processed.</summary>
			public TransType TransType;
			///<summary>The status of the PaySimple response.</summary>
			public string Status;
			///<summary>The returned ProviderAuthCode from PaySimple.</summary>
			public string AuthCode;
			///<summary>The PaySimple Payment ID of the payment that was handled.</summary>
			public string RefNumber;
			///<summary>The PaySimple Account ID of the credit card used to make the payment.</summary>
			public string PaySimpleToken;
			//Commented out CardType.  There isn't a consistent way to get the card type without making extra API calls to PaySimple.
			/////<summary>The issuer of the credit card (E.g. VISA or MASTERCARD).</summary>
			//public string CardType;
			///<summary>Not given from the API.  This is filled after returning back to Open Dental 
			///and uses Open Dental specific things to generate this (ie. clinic info)</summary>
			public string TransactionReceipt;
			public decimal Amount;

			///<summary>Builds the receipt string for a web service transaction.
			///This method assumes ccExpYear is a 4 digit integer.</summary>
			public void BuildReceiptString(string ccNum,int ccExpMonth,int ccExpYear,string nameOnCard,long clinicNum,bool wasSwiped=false) {
				string result="";
				int xleft=0;
				int xright=15;
				result+=Environment.NewLine;
				result+=CreditCardUtils.AddClinicToReceipt(clinicNum);
				//Print body
				result+="Date".PadRight(xright-xleft,'.')+DateTime.Now.ToString()+Environment.NewLine;
				result+=Environment.NewLine;
				result+="Trans Type".PadRight(xright-xleft,'.')+this.TransType.ToString()+Environment.NewLine;
				result+=Environment.NewLine;
				result+="Transaction #".PadRight(xright-xleft,'.')+this.RefNumber+Environment.NewLine;
				if(!string.IsNullOrWhiteSpace(nameOnCard)) {
					result+="Name".PadRight(xright-xleft,'.')+nameOnCard+Environment.NewLine;
				}
				result+="Account".PadRight(xright-xleft,'.');
				for(int i = 0;i<ccNum.Length-4;i++) {
					result+="*";
				}
				if(!string.IsNullOrWhiteSpace(ccNum)) {
					result+=ccNum.Substring(ccNum.Length-4)+Environment.NewLine;//last 4 digits of card number only.
				}
				if(ccExpMonth>=0 && ccExpYear>=0) {
					result+="Exp Date".PadRight(xright-xleft,'.')+ccExpMonth.ToString().PadLeft(2,'0')+(ccExpYear%100)+Environment.NewLine;
				}
				result+="Entry".PadRight(xright-xleft,'.')+(wasSwiped ? "Swiped" : "Manual")+Environment.NewLine;
				result+="Auth Code".PadRight(xright-xleft,'.')+this.AuthCode+Environment.NewLine;
				result+="Result".PadRight(xright-xleft,'.')+this.Status+Environment.NewLine;
				result+=Environment.NewLine+Environment.NewLine+Environment.NewLine;
				if(this.TransType.In(PaySimple.TransType.RETURN,PaySimple.TransType.VOID)) {
					result+="Total Amt".PadRight(xright-xleft,'.')+(this.Amount*-1)+Environment.NewLine;
				}
				else {
					result+="Total Amt".PadRight(xright-xleft,'.')+this.Amount+Environment.NewLine;
				}
				if(this.TransType==TransType.SALE) {
					result+=Environment.NewLine+Environment.NewLine+Environment.NewLine;
					result+="I agree to pay the above total amount according to my card issuer/bank agreement.";
				}
				this.TransactionReceipt=result;
			}

			///<summary>Returns the translated and note-formatted string that represents the result of the API call.</summary>
			public string ToNoteString(string clinicDesc="",string entry="",string curUserName="",string expDateStr="",string cardType="") {
				string retVal="";
				if(!string.IsNullOrWhiteSpace(clinicDesc)) {
					retVal+=Lans.g("PaySimple","Clinic")+": "+clinicDesc+Environment.NewLine;
				}
				retVal+=Lans.g("PaySimple","Transaction Type")+": "+Enum.GetName(typeof(TransType),this.TransType)+Environment.NewLine+
					Lans.g("PaySimple","Status")+": "+this.Status+Environment.NewLine+
					Lans.g("PaySimple","Auth Code")+": "+this.AuthCode+Environment.NewLine+
					Lans.g("PaySimple","Amount")+": "+this.Amount+Environment.NewLine+
					Lans.g("PaySimple","PaySimple Account ID")+": "+this.PaySimpleToken+Environment.NewLine+
					Lans.g("PaySimple","PaySimple Transaction Number")+": "+this.RefNumber+Environment.NewLine;
				if(!string.IsNullOrWhiteSpace(entry)) {
					retVal+=Lans.g("PaySimple","Entry")+": "+entry+Environment.NewLine;
				}
				if(!string.IsNullOrWhiteSpace(curUserName)) {
					retVal+=Lans.g("PaySimple","Clerk")+": "+curUserName+Environment.NewLine;
				}
				if(!string.IsNullOrWhiteSpace(expDateStr)) {
					retVal+=Lans.g("PaySimple","Expiration")+": "+expDateStr+Environment.NewLine;
				}
				if(!string.IsNullOrWhiteSpace(cardType)) {
					retVal+=Lans.g("PaySimple","Card Type")+": "+cardType+Environment.NewLine;
				}
				return retVal;
			}
		}

		private class PaySimpleApi {

			#region SDK Calls
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
			public static string GetCustomerToken(string authHeader,long paySimpleCustID) {
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
					,paySimpleCustID.ToString()
				);
				return response.Response.JwtToken;
			}
			#endregion

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
			public static ApiResponse PostAccountCreditCard(string authHeader,string postData) {
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
				return new ApiResponse() {
					Status="",
					AuthCode="",
					PaySimpleToken=response.Response.Id.ToString(),
					RefNumber="",
					TransType=TransType.AUTH,
				};
			}

			///<summary>Throws exceptions for http codes of 300 or more from API call.</summary>
			public static ApiResponse PostPayment(string authHeader,string postData) {
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
							Amount=(decimal)0.0,
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
				return new ApiResponse() {
					Status=response.Response.Status,
					AuthCode=response.Response.ProviderAuthCode,
					PaySimpleToken=response.Response.AccountId.ToString(),
					RefNumber=response.Response.Id.ToString(),
					TransType=TransType.SALE,
					Amount=response.Response.Amount,
				};
			}

			///<summary>Throws exceptions for http codes of 300 or more from API call.</summary>
			public static ApiResponse PutPaymentVoided(string authHeader,string paymentId) {
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
							Amount=(decimal)0.0,
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
				return new ApiResponse() {
					Status=response.Response.Status,
					AuthCode=response.Response.ProviderAuthCode,
					PaySimpleToken=response.Response.AccountId.ToString(),
					RefNumber=response.Response.Id.ToString(),
					TransType=TransType.VOID,
					Amount=response.Response.Amount,
				};
			}

			///<summary>Throws exceptions for http codes of 300 or more from API call.</summary>
			public static ApiResponse PutPaymentReversed(string authHeader,string paymentId) {
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
							Amount=(decimal)0.0,
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
				return new ApiResponse() {
					Status=response.Response.Status,
					AuthCode=response.Response.ProviderAuthCode,
					PaySimpleToken=response.Response.AccountId.ToString(),
					RefNumber=response.Response.ReferenceId.ToString(),//TODO:  Check that this ID is actually the same as paymentID.  I would prefer to get it from PaySimple instead of my parameter,
					TransType=TransType.RETURN,
					Amount=response.Response.Amount,
				};
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
						else if(method==HttpMethod.Put) {
							res=client.UploadString(GetApiUrl(route,routeId),HttpMethod.Put.Method,body);
						}
						else {
							throw new Exception("Unsupported HttpMethod type: "+method.Method);
						}
#if DEBUG
						if((typeof(T)==typeof(string))) {//If user wants the entire json response as a string
							return (T)Convert.ChangeType(res,typeof(T));
						}
#endif
						return JsonConvert.DeserializeAnonymousType(res,responseType);
					}
					catch(WebException wex) {
						string res="";
						using(var sr=new StreamReader(((HttpWebResponse)wex.Response).GetResponseStream())) {
							res=sr.ReadToEnd();
						}
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

#region MakePostData

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

			public static string MakeNewAccountCreditCardData(long customerId,string ccNum,DateTime ccExpDate,int issuer,string billingZipCode="") {
				return JsonConvert.SerializeObject(new {
					//Required fields:
					CustomerId=customerId,
					CreditCardNumber=ccNum,
					ExpirationDate=ccExpDate.ToString("MM/yyyy"),
					Issuer=issuer,
					IsDefault=false,
					//Optional fields:
					BillingZipCode=billingZipCode,
				});
			}

#endregion

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
				string apiUrl="https://api.paysimple.com/v4";
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
