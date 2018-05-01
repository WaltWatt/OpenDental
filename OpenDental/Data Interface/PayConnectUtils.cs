using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DentalXChange.Dps.Pos;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	static class PayConnectUtils {

		///<summary>Return bool if value passed in is numeric only</summary>
		public static bool IsNumeric(string str) {
			if(str==null) {
				return false;
			}
			Regex objNotWholePattern=new Regex("[^0-9]");
			return !objNotWholePattern.IsMatch(str);
		}

		///<summary>Builds a receipt string for a web service transaction.</summary>
		public static string BuildReceiptString(PayConnectService.creditCardRequest request,PayConnectService.transResponse response,
			PayConnectService.signatureResponse sigResponse,long clinicNum) {
			string result="";
			int xmin=0;
			int xleft=xmin;
			int xright=15;
			int xmax=37;
			result+=Environment.NewLine;
			result+=CreditCardUtils.AddClinicToReceipt(clinicNum);
			//Print body
			result+="Date".PadRight(xright-xleft,'.')+DateTime.Now.ToString()+Environment.NewLine;
			result+=Environment.NewLine;
			result+="Trans Type".PadRight(xright-xleft,'.')+request.TransType.ToString()+Environment.NewLine;
			result+=Environment.NewLine;
			result+="Transaction #".PadRight(xright-xleft,'.')+response.RefNumber+Environment.NewLine;
			result+="Name".PadRight(xright-xleft,'.')+request.NameOnCard+Environment.NewLine;
			result+="Account".PadRight(xright-xleft,'.');
			for(int i = 0;i<request.CardNumber.Length-4;i++) {
				result+="*";
			}
			result+=request.CardNumber.Substring(request.CardNumber.Length-4)+Environment.NewLine;//last 4 digits of card number only.
			result+="Exp Date".PadRight(xright-xleft,'.')+request.Expiration.month.ToString().PadLeft(2,'0')+(request.Expiration.year%100)+Environment.NewLine;
			result+="Card Type".PadRight(xright-xleft,'.')+CreditCardUtils.GetCardType(request.CardNumber)+Environment.NewLine;
			result+="Entry".PadRight(xright-xleft,'.')+(String.IsNullOrEmpty(request.MagData) ? "Manual" : "Swiped")+Environment.NewLine;
			result+="Auth Code".PadRight(xright-xleft,'.')+response.AuthCode+Environment.NewLine;
			result+="Result".PadRight(xright-xleft,'.')+response.Status.description+Environment.NewLine;
			if(response.Messages!=null) {
				string label="Message";
				foreach(string m in response.Messages) {
					result+=label.PadRight(xright-xleft,'.')+m+Environment.NewLine;
					label="";
				}
			}
			result+=Environment.NewLine+Environment.NewLine+Environment.NewLine;
			if(request.TransType.In(PayConnectService.transType.RETURN,PayConnectService.transType.VOID)) {
				result+="Total Amt".PadRight(xright-xleft,'.')+(request.Amount*-1)+Environment.NewLine;
			}
			else {
				result+="Total Amt".PadRight(xright-xleft,'.')+request.Amount+Environment.NewLine;
			}
			result+=Environment.NewLine+Environment.NewLine+Environment.NewLine;
			result+="I agree to pay the above total amount according to my card issuer/bank agreement."+Environment.NewLine;
			result+=Environment.NewLine+Environment.NewLine+Environment.NewLine+Environment.NewLine+Environment.NewLine;
			if(sigResponse==null || sigResponse.Status==null || sigResponse.Status.code!=0) {
				result+="Signature X".PadRight(xmax-xleft,'_');
			}
			else {
				result+="Electronically signed";
			}
			return result;
		}

		///<summary>Builds a receipt string for a terminal transaction.</summary>
		public static string BuildReceiptString(PosRequest posRequest,PosResponse posResponse,PayConnectService.signatureResponse sigResponse,
			long clinicNum) {
			string result="";
			int xleft=0;
			int xright=15;
			int xmax=37;
			result+=Environment.NewLine;
			result+=CreditCardUtils.AddClinicToReceipt(clinicNum);
			//Print body
			result+="Date".PadRight(xright-xleft,'.')+DateTime.Now.ToString()+Environment.NewLine;
			result+=Environment.NewLine;
			result+=AddReceiptField("Trans Type",posResponse.TransactionType.ToString());
			result+=Environment.NewLine;
			result+=AddReceiptField("Transaction #",posResponse.ReferenceNumber.ToString());
			result+=AddReceiptField("Account",posResponse.CardNumber);
			result+=AddReceiptField("Card Type",posResponse.CardBrand);
			result+=AddReceiptField("Entry",posResponse.EntryMode);
			result+=AddReceiptField("Auth Code",posResponse.AuthCode);
			result+=AddReceiptField("Result",posResponse.ResponseDescription);
			result+=AddReceiptField("MerchantId",posResponse.MerchantId);
			result+=AddReceiptField("TerminalId",posResponse.TerminalId);
			result+=AddReceiptField("Mode",posResponse.Mode);
			result+=AddReceiptField("CardVerifyMthd",posResponse.CardVerificationMethod);
			if(posResponse.EMV!=null && !string.IsNullOrEmpty(posResponse.EMV.AppId)) {
				result+=AddReceiptField("EMV AppId",posResponse.EMV.AppId);
			}
			if(posResponse.EMV!=null && !string.IsNullOrEmpty(posResponse.EMV.TermVerifResults)) {
				result+=AddReceiptField("EMV TermResult",posResponse.EMV.TermVerifResults);
			}
			if(posResponse.EMV!=null && !string.IsNullOrEmpty(posResponse.EMV.IssuerAppData)) {
				result+=AddReceiptField("EMV IssuerData",posResponse.EMV.IssuerAppData);
			}
			if(posResponse.EMV!=null && !string.IsNullOrEmpty(posResponse.EMV.TransStatusInfo)) {
				result+=AddReceiptField("EMV TransInfo",posResponse.EMV.TransStatusInfo);
			}
			if(posResponse.EMV!=null && !string.IsNullOrEmpty(posResponse.EMV.AuthResponseCode)) {
				result+=AddReceiptField("EMV AuthResp",posResponse.EMV.AuthResponseCode);
			}
			result+=Environment.NewLine+Environment.NewLine+Environment.NewLine;
			if(posResponse.TransactionType.In(TransactionType.Refund,TransactionType.Void)) {
				result+="Total Amt".PadRight(xright-xleft,'.')+(posResponse.Amount*-1)+Environment.NewLine;
			}
			else {
				result+="Total Amt".PadRight(xright-xleft,'.')+posResponse.Amount+Environment.NewLine;
			}
			result+=Environment.NewLine+Environment.NewLine+Environment.NewLine;
			result+="I agree to pay the above total amount according to my card issuer/bank agreement."+Environment.NewLine;
			result+=Environment.NewLine+Environment.NewLine+Environment.NewLine+Environment.NewLine+Environment.NewLine;
			if(sigResponse==null || sigResponse.Status==null || sigResponse.Status.code!=0) {
				result+="Signature X".PadRight(xmax-xleft,'_');
			}
			else {
				result+="Electronically signed";
			}
			return result;
		}

		///<summary>Returns the field name and value formatted to be added to a receipt string. The fieldName should be less than 15 characters.</summary>
		private static string AddReceiptField(string fieldName,string fieldValue) {
			int xleft=0;
			int xright=15;
			int xmax=37;
			string retStr="";
			fieldValue=fieldValue??"";
			retStr+=fieldName.PadRight(xright-xleft,'.');
			if(fieldValue.Length<xmax-xright) {//Short enough to fit on one line
				retStr+=fieldValue+Environment.NewLine;
			}
			else {//Put the field value on two lines
				retStr+=fieldValue.Substring(0,xmax-xright-1)+Environment.NewLine;
				retStr+="".PadRight(xright,'.')+fieldValue.Substring(xmax-xright-1)+Environment.NewLine;
			}
			return retStr;
		}

	}
}
