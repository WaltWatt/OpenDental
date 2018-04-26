using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using Health.Direct.Common.Certificates;
using CodeBase;
using OpenDentBusiness.FileIO;
using System.Data;

namespace OpenDentBusiness{
	///<summary>An email message is always attached to a patient.</summary>
	public class EmailMessages{		
		#region Get Methods
		#endregion

		#region Modification Methods
		
		#region Insert
		#endregion

		#region Update
		#endregion

		#region Delete
		#endregion

		#endregion

		#region Misc Methods
		#endregion

		//ThreadStatic Variables are thread specific and are thread safe thus do not require locking.
		[ThreadStatic]
		private static Health.Direct.Agent.DirectAgent _directAgent=null;
		private static List<EmailAddress> _listCurrentlyReceivingEmailAddressNums=new List<EmailAddress>();

    #region Database Calls
		///<summary>Gets one email message from the database.</summary>
		public static EmailMessage GetOne(long msgNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<EmailMessage>(MethodBase.GetCurrentMethod(),msgNum);
			}
			string command="SELECT * FROM emailmessage WHERE EmailMessageNum = "+POut.Long(msgNum);
			EmailMessage emailMessage=Crud.EmailMessageCrud.SelectOne(msgNum);
			if(emailMessage!=null) {
				command="SELECT * FROM emailattach WHERE EmailMessageNum = "+POut.Long(msgNum);
				emailMessage.Attachments=Crud.EmailAttachCrud.SelectMany(command);
			}
			return emailMessage;
		}

		///<summary>Gets all inbox email messages where EmailMessage.RecipientAddress==emailAddressInbox, or returns webmail messages instead.  
		///Pass in 0 for provNum to get email messages, pass in the current user's provNum to get webmail messages.</summary>
		public static List<EmailMessage> GetMailboxForAddress(EmailAddress emailAddress,DateTime dateFrom,DateTime dateTo,params MailboxType[] arrayMailBoxTypes) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<EmailMessage>>(MethodBase.GetCurrentMethod(),emailAddress,dateFrom,dateTo,arrayMailBoxTypes);
			}
			//We only pull the first 50 characters of the bodytext for preview purposes. We also do not pull the RawEmailIn, because it is not necessary for the inbox.
			//After double-clicking an email in the inbox to view it, then the entire email contents are read from the database.
			string command="SELECT EmailMessageNum,PatNum,ToAddress,FromAddress,Subject,SUBSTR(BodyText,1,50) BodyText,MsgDateTime,SentOrReceived,"
				+"RecipientAddress,'' RawEmailIn,ProvNumWebMail,PatNumSubj,CcAddress,BccAddress,HideIn,AptNum,UserNum "
				+"FROM emailmessage "
				+"WHERE MsgDateTime BETWEEN	"+POut.Date(dateFrom)+" AND "+POut.Date(dateTo.AddDays(1))+" AND ";//Cannot use DATE(MsgDateTime), because the index on MsgDateTime would not work.
			string strSentReceived="";
			if(emailAddress.WebmailProvNum==0) {//emailmessages
																					//must match one of these EmailSentOrReceived statuses
				if(arrayMailBoxTypes.Contains(MailboxType.Inbox)) {
					strSentReceived+=" (SentOrReceived IN ("
						+POut.Int((int)EmailSentOrReceived.Read)+","
						+POut.Int((int)EmailSentOrReceived.Received)+","
						+POut.Int((int)EmailSentOrReceived.ReceivedEncrypted)+","
						+POut.Int((int)EmailSentOrReceived.ReceivedDirect)+","
						+POut.Int((int)EmailSentOrReceived.ReadDirect)
						+") AND RecipientAddress = '"+POut.String(emailAddress.EmailUsername.Trim())+"') ";
				}
				if(arrayMailBoxTypes.Contains(MailboxType.Sent)) {
					if(strSentReceived!="") {
						strSentReceived+=" OR ";
					}
					strSentReceived+=" (SentOrReceived IN(";
					strSentReceived+=(int)EmailSentOrReceived.Sent+","+(int)EmailSentOrReceived.SentDirect+","
						+(int)EmailSentOrReceived.WebMailSent+","+(int)EmailSentOrReceived.WebMailSentRead+") "
						+" AND FromAddress = '"+POut.String(EmailMessages.GetAddressSimple(emailAddress.EmailUsername).Trim())+"') ";
				}
			}
			else {//webmail messages for matching provnum
				//must match one of these EmailSentOrReceived statuses
				strSentReceived+=" (SentOrReceived IN ("
					+POut.Int((int)EmailSentOrReceived.WebMailRecdRead)+","
					+POut.Int((int)EmailSentOrReceived.WebMailReceived)
				+") AND ProvNumWebMail="+POut.Long(emailAddress.WebmailProvNum)+") ";
			}
      command+=strSentReceived;
			command+="ORDER BY MsgDateTime";
			List<EmailMessage> retVal=Crud.EmailMessageCrud.SelectMany(command);
			for(int i=0;i<retVal.Count;i++) {
				command="SELECT * FROM emailattach WHERE EmailMessageNum = "+POut.Long(retVal[i].EmailMessageNum);
				retVal[i].Attachments=Crud.EmailAttachCrud.SelectMany(command);
			}
			return retVal;
		}

		///<summary>Returns the list of historically used email addresses.</summary>
		public static List<string> GetHistoricalEmailAddresses(EmailAddress emailAddress) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<string>>(MethodBase.GetCurrentMethod(),emailAddress);
			}
			string command=@"SELECT 
				emailmessage.ToAddress,
				emailmessage.FromAddress,
				emailmessage.RecipientAddress,
				emailmessage.CcAddress,
				emailmessage.BccAddress
				FROM emailmessage
				WHERE (SentOrReceived IN ("
					+POut.Int((int)EmailSentOrReceived.Read)+","
					+POut.Int((int)EmailSentOrReceived.Received)+","
					+POut.Int((int)EmailSentOrReceived.ReceivedEncrypted)+","
					+POut.Int((int)EmailSentOrReceived.ReceivedDirect)+","
					+POut.Int((int)EmailSentOrReceived.ReadDirect)
					+") AND RecipientAddress = '"+POut.String(emailAddress.EmailUsername.Trim())
					+"') OR (SentOrReceived IN("
				  +(int)EmailSentOrReceived.Sent+","+(int)EmailSentOrReceived.SentDirect+","
				  +(int)EmailSentOrReceived.WebMailSent+","+(int)EmailSentOrReceived.WebMailSentRead+") "
					+" AND FromAddress = '"+POut.String(EmailMessages.GetAddressSimple(emailAddress.EmailUsername).Trim())+"') ";
			DataTable table=Db.GetTable(command);
			List<EmailMessage> listMessages=new List<EmailMessage>();
			foreach(DataRow row in table.Rows) {
				EmailMessage emailMessage=new EmailMessage();
				emailMessage.ToAddress       = PIn.String(row["ToAddress"].ToString());
				emailMessage.FromAddress     = PIn.String(row["FromAddress"].ToString());
				emailMessage.RecipientAddress= PIn.String(row["RecipientAddress"].ToString());
				emailMessage.CcAddress       = PIn.String(row["CcAddress"].ToString());
				emailMessage.BccAddress      = PIn.String(row["BccAddress"].ToString());
				listMessages.Add(emailMessage);
			}
			return GetAddressesFromMessages(listMessages);
		}
		
		///<summary>Takes a list of email messages and returns the addresses that are used in any of them.</summary>
		public static List<string> GetAddressesFromMessages(List<EmailMessage> listEmailMessages) {
			return listEmailMessages.Where(x => !string.IsNullOrWhiteSpace(x.ToAddress)).Select(x => x.ToAddress.Trim())
				.Union(listEmailMessages.Where(x => !string.IsNullOrWhiteSpace(x.FromAddress)).Select(x => x.FromAddress.Trim()))
				.Union(listEmailMessages.Where(x => !string.IsNullOrWhiteSpace(x.BccAddress)).Select(x => x.BccAddress.Trim()))
				.Union(listEmailMessages.Where(x => !string.IsNullOrWhiteSpace(x.CcAddress)).Select(x => x.CcAddress.Trim()))
				.Union(listEmailMessages.Where(x => !string.IsNullOrWhiteSpace(x.RecipientAddress)).Select(x => x.RecipientAddress.Trim())).ToList();
		}

    ///<summary>Goes to the db and returns messages that match the passed-in params.
    ///Does not search on fields that are passed-in blank, 0, or DateTime.MinVal (depending on type).</summary>
    public static List<EmailMessage> GetBySearch(long searchPatNum,string searchEmail,DateTime dateFrom,DateTime dateTo,string searchBody,bool hasAttach) {
      if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
        return Meth.GetObject<List<EmailMessage>>(MethodBase.GetCurrentMethod(),searchPatNum,searchEmail,dateFrom,dateTo,searchBody);
      }
      string command="SELECT * FROM emailmessage "
        +"WHERE TRUE ";
      if(searchPatNum!=0) {
        command+="AND PatNum="+POut.Long(searchPatNum)+" ";
      }
      if(searchEmail!="") {
        command+="AND (FromAddress LIKE '%"+POut.String(searchEmail)+"%' "
          +"OR ToAddress LIKE '%"+POut.String(searchEmail)+"%' "
          +"OR RecipientAddress LIKE '%"+POut.String(searchEmail)+"%' "
          +"OR CcAddress LIKE '%"+POut.String(searchEmail)+"%' "
          +"OR BccAddress LIKE '%"+POut.String(searchEmail)+"%') ";
      }
      if(dateFrom!=DateTime.MinValue) {
        command+="AND DATE(MsgDateTime)>="+POut.Date(dateFrom)+" ";
      }
      if(dateTo!=DateTime.MinValue) {
        command+="AND DATE(MsgDateTime)<="+POut.Date(dateTo)+" ";
      }
      if(searchBody!="") { //this should never be blank
        command+="AND (Subject LIKE '%"+POut.String(searchBody)+"%' OR BodyText LIKE '%"+POut.String(searchBody)+"%')";
      }
      List<EmailMessage> retVal=Crud.EmailMessageCrud.SelectMany(command);
      for(int i=0;i<retVal.Count;i++) {
        command="SELECT * FROM emailattach WHERE EmailMessageNum="+POut.Long(retVal[i].EmailMessageNum);
        retVal[i].Attachments=Crud.EmailAttachCrud.SelectMany(command);
      }
      if(hasAttach) {
        retVal=retVal.Where(x => x.Attachments.Count>0).ToList();
      }
      return retVal;
    }

		public static List<EmailMessage> GetWebMailForPat(long patNum) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				return Meth.GetObject<List<EmailMessage>>(MethodBase.GetCurrentMethod(),patNum);
			}
			List<long> listPatNums=Patients.GetPatNumsForPhi(patNum);//Guaranteed to have at least one value (the patNum passed in).
			string command="SELECT * FROM emailmessage "
				+"WHERE PatNumSubj IN("+string.Join(",",listPatNums)+") "
				+"AND SentOrReceived IN ("
					+POut.Int((int)EmailSentOrReceived.WebMailReceived)+", "
					+POut.Int((int)EmailSentOrReceived.WebMailRecdRead)+", "
					+POut.Int((int)EmailSentOrReceived.WebMailSent)+", "
					+POut.Int((int)EmailSentOrReceived.WebMailSentRead)+") "
				+"ORDER BY MsgDateTime DESC";
			return Crud.EmailMessageCrud.SelectMany(command);
		}

		///<summary>If isAttachmentSyncNeeded is true, then it will automatically sync attachments.  Otherwise, attachments will not be modified.
		///The Patient Portal will pass in an object with an empty attachment list, but that does not mean that the attachments should be deleted.</summary>
		public static void Update(EmailMessage message,EmailMessage messageOld=null,bool isAttachmentSyncNeeded=true) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),message,messageOld,isAttachmentSyncNeeded);
				return;
			}
			if(messageOld==null) {
				Crud.EmailMessageCrud.Update(message);
			}
			else {
				Crud.EmailMessageCrud.Update(message,messageOld);
			}
			if(isAttachmentSyncNeeded) {
				foreach(EmailAttach attach in message.Attachments) {
					attach.EmailMessageNum=message.EmailMessageNum;//update all of the emailmessagenums for the attachments.
				};
				EmailAttaches.Sync(message.EmailMessageNum,message.Attachments);
			}
		}

		///<summary>Updates SentOrReceived and saves changes to db.  Better than using Update(), because does not delete and add attachments back into db.
    ///Returns the new EmailSentOrReceived Status.</summary>
		public static EmailSentOrReceived UpdateSentOrReceivedRead(EmailMessage emailMessage) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
        return Meth.GetObject<EmailSentOrReceived>(MethodBase.GetCurrentMethod(),emailMessage);
			}
			EmailSentOrReceived sentOrReceived=emailMessage.SentOrReceived;
			if(emailMessage.SentOrReceived==EmailSentOrReceived.Received) {
				sentOrReceived=EmailSentOrReceived.Read;
			}
			else if(emailMessage.SentOrReceived==EmailSentOrReceived.WebMailReceived) {
				sentOrReceived=EmailSentOrReceived.WebMailRecdRead;
			}
			else if(emailMessage.SentOrReceived==EmailSentOrReceived.ReceivedDirect) {
				sentOrReceived=EmailSentOrReceived.ReadDirect;
			}
			if(sentOrReceived==emailMessage.SentOrReceived) {
				return sentOrReceived;//Nothing to do.
			}
			string command="UPDATE emailmessage SET SentOrReceived="+POut.Int((int)sentOrReceived)+" WHERE EmailMessageNum="+POut.Long(emailMessage.EmailMessageNum);
			Db.NonQ(command);
      return sentOrReceived;
		}

		///<summary>Updates SentOrReceived and saves changes to db.  Better than using Update(), because does not delete and add attachments back into db.
    ///Returns the new status.</summary>
		public static EmailSentOrReceived UpdateSentOrReceivedUnread(EmailMessage emailMessage) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
        return Meth.GetObject<EmailSentOrReceived>(MethodBase.GetCurrentMethod(),emailMessage);
			}
			EmailSentOrReceived sentOrReceived=emailMessage.SentOrReceived;
			if(emailMessage.SentOrReceived==EmailSentOrReceived.Read) {
				sentOrReceived=EmailSentOrReceived.Received;
			}
			else if(emailMessage.SentOrReceived==EmailSentOrReceived.WebMailRecdRead) {
				sentOrReceived=EmailSentOrReceived.WebMailReceived;
			}
			else if(emailMessage.SentOrReceived==EmailSentOrReceived.ReadDirect) {
				sentOrReceived=EmailSentOrReceived.ReceivedDirect;
			}
			if(sentOrReceived==emailMessage.SentOrReceived) {
				return sentOrReceived;//Nothing to do.
			}
			string command="UPDATE emailmessage SET SentOrReceived="+POut.Int((int)sentOrReceived)+" WHERE EmailMessageNum="+POut.Long(emailMessage.EmailMessageNum);
			Db.NonQ(command);
      return sentOrReceived;
		}

		///<summary>Updates SentOrReceived and saves changes to db.  Better than using Update(), because does not delete and add attachments back into db.</summary>
		public static void UpdatePatNum(EmailMessage emailMessage) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),emailMessage);
				return;
			}
			string command="UPDATE emailmessage SET PatNum="+POut.Long(emailMessage.PatNum)+" WHERE EmailMessageNum="+POut.Long(emailMessage.EmailMessageNum);
			Db.NonQ(command);
			if(emailMessage.Attachments==null) {
				return;
			}
			for(int i=0;i<emailMessage.Attachments.Count;i++) {
				EhrSummaryCcd ehrSummaryCcd=EhrSummaryCcds.GetOneForEmailAttach(emailMessage.Attachments[i].EmailAttachNum);
				if(ehrSummaryCcd!=null) {
					ehrSummaryCcd.PatNum=emailMessage.PatNum;
					EhrSummaryCcds.Update(ehrSummaryCcd);
				}
			}
		}

		///<summary></summary>
		public static long Insert(EmailMessage message) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				message.EmailMessageNum=Meth.GetLong(MethodBase.GetCurrentMethod(),message);
				return message.EmailMessageNum;
			}
			Crud.EmailMessageCrud.Insert(message);
			//now, insert all the attaches.
			for(int i=0;i<message.Attachments.Count;i++) {
				message.Attachments[i].EmailMessageNum=message.EmailMessageNum;
				EmailAttaches.Insert(message.Attachments[i]);
			}
			return message.EmailMessageNum;
		}

		///<summary></summary>
		public static void Delete(EmailMessage message){
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),message);
				return;
			}
			if(message.EmailMessageNum==0){
				return;//this prevents deletion of all commlog entries of something goes wrong.
			}
			string command="DELETE FROM emailmessage WHERE EmailMessageNum="+POut.Long(message.EmailMessageNum);
			Db.NonQ(command);
		}
    #endregion

		#region Sending

		///<summary>Encrypts the message, verifies trust, locates the public encryption key for the To address (if already stored locally), etc.
		///Use this polymorphism when the attachments have already been saved to the email attachments folder in file form.  patNum can be 0.
		///Returns an empty string upon success, or an error string if there were errors.
		///It is possible that the email was sent to some trusted recipients and not sent to untrusted recipients (in which case there would be errors but some recipients would receive successfully).
		///Trust cannot be automatically added for the recipient addresses inside this function, because the patient portal uses this function and as soon as an address is trusted
		///all patients can then forward their personal information to the recipient address.
		///Surround with a try catch.</summary>
		public static string SendEmailDirect(EmailMessage emailMessage,EmailAddress emailAddressFrom) {
			//No need to check RemotingRole; no call to db.
			emailMessage.FromAddress=emailAddressFrom.EmailUsername.Trim();//Cannot be emailAddressFrom.SenderAddress, or else will not find the correct encryption certificate.  Used in ConvertEmailMessageToMessage().
			//Start by converting the emailMessage to an unencrypted message using the Direct libraries. The email must be in this form to carry out encryption.
			Health.Direct.Common.Mail.Message msgUnencrypted=ConvertEmailMessageToMessage(emailMessage,true);
			Health.Direct.Agent.MessageEnvelope msgEnvelopeUnencrypted=new Health.Direct.Agent.MessageEnvelope(msgUnencrypted);
			Health.Direct.Agent.OutgoingMessage outMsgUnencrypted=new Health.Direct.Agent.OutgoingMessage(msgEnvelopeUnencrypted);
			string strErrors=SendEmailDirect(outMsgUnencrypted,emailAddressFrom);
			return strErrors;
		}

		///<summary>Refreshes our cached copy of the public key certificate store and the anchor certificate store from the Windows certificate store.</summary>
		public static void RefreshCertStoreExternal(EmailAddress emailAddressLocal) {
			string strSenderAddress=emailAddressLocal.EmailUsername.Trim();//Cannot be emailAddressFrom.SenderAddress, or else will not find the right encryption certificate.
			try {
				GetDirectAgentForEmailAddress(strSenderAddress);//This line is where the refresh occurs.
			}
			catch(Exception ex) {//Likely a permission issue
				ex.DoNothing();
			}
		}

		///<summary>Throws exceptions.  outMsgDirect must be unencrypted, because this function will encrypt.  Encrypts the message, verifies trust, locates the public encryption key for the To address (if already stored locally), etc.
		///Returns an empty string upon success, or an error string if there were errors.  It is possible that the email was sent to some trusted recipients and not sent to untrusted recipients (in which case there would be errors but some recipients would receive successfully).</summary>
		private static string SendEmailDirect(Health.Direct.Agent.OutgoingMessage outMsgUnencrypted,EmailAddress emailAddressFrom) {
			//No need to check RemotingRole; no call to db.
			string strErrors="";
			string strSenderAddress=emailAddressFrom.EmailUsername.Trim();//Cannot be emailAddressFrom.SenderAddress, or else will not find the right encryption certificate.
			//Locate or discover public certificates for each receiver for encryption purposes.
			for(int i=0;i<outMsgUnencrypted.Recipients.Count;i++) {
				string receiveAddress=outMsgUnencrypted.Recipients[i].Address.Trim();
				List <X509Certificate2> listDirectValidCerts=new List<X509Certificate2>();
				List <X509Certificate2> listDirectInvalidCerts=new List<X509Certificate2>();
				try {
					TryAddTrustDirect(receiveAddress,listDirectValidCerts,listDirectInvalidCerts);
				}
				catch(Exception ex) {
					if(strErrors!="") {
						strErrors+="\r\n";
					}
					strErrors+=ex.Message;
				}
				//For Direct addresses, only use the current listValidCerts for sending.  Otherwise, use all certs in pub cert store.
				int untrustedCount=-1;
				if(listDirectValidCerts.Count > 0 || listDirectInvalidCerts.Count > 0) {//The receiveAddress has a hosted cert, therefore is Direct address.
					if(listDirectValidCerts.Count==0) {
						untrustedCount=listDirectInvalidCerts.Count;
					}
				}
				else {//Standard encrypted email.
					untrustedCount=GetReceiverUntrustedCount(receiveAddress);
				}
				if(untrustedCount >= 0) {
					if(strErrors!="") {
						strErrors+="\r\n";
					}
					strErrors+=Lans.g("EmailMessages","No active certificates discovered for recipient: ")+" "+receiveAddress;
					if(untrustedCount > 0) {
						strErrors+="\r\n"+Lans.g("EmailMessages","Inactive certificates discovered")+": "+untrustedCount;
					}
				}
			}
			if(strErrors!="") {
				return strErrors;//Most likely could not find the public certificate for the receiver.  In any case, cannot continue.
			}
			List <string> listAddresses=new List<string>();
			listAddresses.Add(strSenderAddress);
			for(int i=0;i<outMsgUnencrypted.Recipients.Count;i++) {
				listAddresses.Add(outMsgUnencrypted.Recipients[i].Address.Trim());
			}			
			Health.Direct.Agent.OutgoingMessage outMsgEncrypted=null;
			try {
				Health.Direct.Agent.DirectAgent directAgent=GetDirectAgentForEmailAddress(listAddresses.ToArray());	
				outMsgEncrypted=directAgent.ProcessOutgoing(outMsgUnencrypted);//This is where encryption, signing, and trust verification occurs.
			}
			catch(Exception ex) {
				if(strErrors!="") {
					strErrors+="\r\n";
				}
				strErrors+=ex.Message;
				return strErrors;//Cannot recover from an encryption error.
			}
			outMsgEncrypted.Message.SubjectValue="Encrypted Message";//Prevents a warning in the transport testing tool (TTT). http://tools.ietf.org/html/rfc5322#section-3.6.5
			EmailMessage emailMessageEncrypted=ConvertMessageToEmailMessage(outMsgEncrypted.Message,false,true);//No point in saving the encrypted attachment, because nobody can read it and it will bloat the OpenDentImages folder.
			NameValueCollection nameValueCollectionHeaders=new NameValueCollection();
			for(int i=0;i<outMsgEncrypted.Message.Headers.Count;i++) {
				nameValueCollectionHeaders.Add(outMsgEncrypted.Message.Headers[i].Name,outMsgEncrypted.Message.Headers[i].ValueRaw);
			}
			byte[] arrayEncryptedBody=Encoding.UTF8.GetBytes(outMsgEncrypted.Message.Body.Text);//The bytes of the encrypted and base 64 encoded body string.  No need to call Tidy() here because this body text will be in base64.
			MemoryStream ms=new MemoryStream(arrayEncryptedBody);
			ms.Position=0;
			//The memory stream for the alternate view must be mime (not an entire email), based on AlternateView use example http://msdn.microsoft.com/en-us/library/system.net.mail.mailmessage.alternateviews.aspx
			AlternateView alternateView=new AlternateView(ms,outMsgEncrypted.Message.ContentType);//Causes the receiver to recognize this email as an encrypted email.
			alternateView.TransferEncoding=TransferEncoding.SevenBit;
			if(emailAddressFrom.ServerPort==465) {//Implicit SSL
				//See comments inside SendEmailUnsecure() regarding why this does not work.
				if(strErrors!="") {
					strErrors+="\r\n";
				}
				strErrors+=Lans.g("EmailMessages","Direct messages cannot be sent over implicit SSL.");
			}
			else {
				WireEmailUnsecure(emailMessageEncrypted,emailAddressFrom,nameValueCollectionHeaders,alternateView);//Not really unsecure in this spot, because the message is already encrypted.
			}
			ms.Dispose();
			return strErrors;
		}

		///<summary>Used for creating encrypted Message Disposition Notification (MDN) ack messages for Direct.
		///An ack must be sent when a message is received/processed, and other acks are supposed be sent when other events occur (but are not required).
		///For example, when the user reads a decrypted message we must send an ack with notification type of Displayed (not required).</summary>
		private static string SendAckDirect(Health.Direct.Agent.IncomingMessage inMsg,EmailAddress emailAddressFrom,long patNum) {
			//No need to check RemotingRole; no call to db.
			//The CreateAcks() function handles the case where the incoming message is an MDN, in which case we do not reply with anything.
			//The CreateAcks() function also takes care of figuring out where to send the MDN, because the rules are complicated.
			//According to http://wiki.directproject.org/Applicability+Statement+for+Secure+Health+Transport+Working+Version#x3.0%20Message%20Disposition%20Notification,
			//The MDN must be sent to the first available of: Disposition-Notification-To header, MAIL FROM SMTP command, Sender header, From header.
			Health.Direct.Common.Mail.Notifications.MDNStandard.NotificationType notificationType=Health.Direct.Common.Mail.Notifications.MDNStandard.NotificationType.Failed;
			notificationType=Health.Direct.Common.Mail.Notifications.MDNStandard.NotificationType.Processed;
			IEnumerable<Health.Direct.Common.Mail.Notifications.NotificationMessage> notificationMsgs=inMsg.CreateAcks("OpenDental "+Assembly.GetExecutingAssembly().GetName().Version,"",notificationType);
			if(notificationMsgs==null) {
				return "";
			}
			string strErrorsAll="";
			foreach(Health.Direct.Common.Mail.Notifications.NotificationMessage notificationMsg in notificationMsgs) {
				string strErrors="";
				try {
					//According to RFC3798, section 3 - Format of a Message Disposition Notification http://tools.ietf.org/html/rfc3798#page-3
					//A message disposition notification is a MIME message with a top-level
					//content-type of multipart/report (defined in [RFC-REPORT]).  When
					//multipart/report content is used to transmit an MDN:
					//(a)  The report-type parameter of the multipart/report content is "disposition-notification".
					//(b)  The first component of the multipart/report contains a human-readable explanation of the MDN, as described in [RFC-REPORT].
					//(c)  The second component of the multipart/report is of content-type message/disposition-notification, described in section 3.1 of this document.
					//(d)  If the original message or a portion of the message is to be returned to the sender, it appears as the third component of the multipart/report.
					//     The decision of whether or not to return the message or part of the message is up to the MUA generating the MDN.  However, in the case of 
					//     encrypted messages requesting MDNs, encrypted message text MUST be returned, if it is returned at all, only in its original encrypted form.
					Health.Direct.Agent.OutgoingMessage outMsgDirect=new Health.Direct.Agent.OutgoingMessage(notificationMsg);
					if(notificationMsg.ToValue.Trim().ToLower()==notificationMsg.FromValue.Trim().ToLower()) {
						continue;//Do not send an ack to self.
					}
					EmailMessage emailMessage=ConvertMessageToEmailMessage(outMsgDirect.Message,false,true);
					emailMessage.PatNum=patNum;
					//First save the ack message to the database in case their is a failure sending the email. This way we can remember to try and send it again later, based on SentOrReceived.
					emailMessage.SentOrReceived=EmailSentOrReceived.AckDirectNotSent;
					MemoryStream ms=new MemoryStream();
					notificationMsg.Save(ms);
					byte[] arrayMdnMessageBytes=ms.ToArray();
					emailMessage.BodyText=Encoding.UTF8.GetString(arrayMdnMessageBytes);
					ms.Dispose();
					Insert(emailMessage);				
				}
				catch(Exception ex) {
					strErrors=ex.Message;
				}
				if(strErrorsAll!="") {
					strErrorsAll+="\r\n";
				}
				strErrorsAll+=strErrors;
			}
			try {
				SendOldestUnsentAck(emailAddressFrom);//Send the ack(s) we created above.
			}
			catch {
				//Not critical to send the acks here, because they will be sent later if they failed now.
			}
			return strErrorsAll;
		}

		///<summary>Gets the oldest Direct Ack (MDN) from the db which has not been sent yet and attempts to send it.
		///If the Ack fails to send, then it remains in the database with status AckDirectNotSent, so that another attempt will be made when this function is called again.
		///This function throttles the Ack responses to prevent the email host from flagging the emailAddressFrom as a spam account.  The throttle speed is one Ack per 60 seconds (to mimic human behavior).
		///Throws exceptions.</summary>
		public static void SendOldestUnsentAck(EmailAddress emailAddressFrom) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				Meth.GetVoid(MethodBase.GetCurrentMethod(),emailAddressFrom);
				return;
			}
			string command;
			//Get the time that the last Direct Ack was sent for the From address.
			command=DbHelper.LimitOrderBy(
				"SELECT MsgDateTime FROM emailmessage "
					+"WHERE FromAddress='"+POut.String(emailAddressFrom.EmailUsername.Trim())+"' AND SentOrReceived="+POut.Long((int)EmailSentOrReceived.AckDirectProcessed)+" "
					+"ORDER BY MsgDateTime DESC",
				1);
			DateTime dateTimeLastAck=PIn.DateT(Db.GetScalar(command));//dateTimeLastAck will be 0001-01-01 if there is not yet any sent Acks.
			if((DateTime.Now-dateTimeLastAck).TotalSeconds<60) {
				//Our last Ack sent was less than 15 seconds ago.  Abort sending Acks right now.
				return;
			}
			//Get the oldest Ack for the From address which has not been sent yet.
			command=DbHelper.LimitOrderBy(
				"SELECT * FROM emailmessage "
					+"WHERE FromAddress='"+POut.String(emailAddressFrom.EmailUsername.Trim())+"' AND SentOrReceived="+POut.Long((int)EmailSentOrReceived.AckDirectNotSent)+" "
					+"ORDER BY EmailMessageNum",//The oldest Ack is the one that was recorded first.  EmailMessageNum is better than using MsgDateTime, because MsgDateTime is only accurate down to the second.
				1);
			List <EmailMessage> listEmailMessageUnsentAcks=Crud.EmailMessageCrud.SelectMany(command);
			if(listEmailMessageUnsentAcks.Count<1) {
				return;//No Acks to send.
			}
			EmailMessage emailMessageAck=listEmailMessageUnsentAcks[0];
			string strRawEmailAck=emailMessageAck.BodyText;//Not really body text.  The entire raw Ack is saved here, and we use it to reconstruct the Ack email completely.
			Health.Direct.Agent.MessageEnvelope messageEnvelopeMdn=new Health.Direct.Agent.MessageEnvelope(strRawEmailAck);
			Health.Direct.Agent.OutgoingMessage outMsgDirect=new Health.Direct.Agent.OutgoingMessage(messageEnvelopeMdn);
			try {
				string strErrors=SendEmailDirect(outMsgDirect,emailAddressFrom);//Encryption is performed in this step. Throws an exception if unable to send (i.e. when internet down).
				if(strErrors=="") {
					emailMessageAck.SentOrReceived=EmailSentOrReceived.AckDirectProcessed;
					emailMessageAck.MsgDateTime=DateTime.Now;//Update the time, otherwise the throttle will not work properly.
					Update(emailMessageAck);
				}
			}
			catch {
			}
		}

		///<summary>Call to cleanup newlines within a string before including in an email. The RFC 822 guide states that every single line in a raw email message must end with \r\n, also known as CRLF.
		///Certain email providers will reject outgoing email from us if we have any lines ending with \n or \r. Email providers that we know care: Prosites. Other email providers seem to handle
		///all different types of newlines, even though \r or \n by itself is not standard. This function replaces all \r and \n with \r\n.</summary>
		public static string BodyTidy(string str) {
			//This function assumes the worst case, which is a string that has all 3 types of newlines: \r, \n and \r\n
			//We will first convert \r\n and \r into \n so that all our line endings are the same. Then replace \n with \r\n to make the newlines proper.
			string retVal=str.Replace("\r\n","\n");//We must replace the two character newline first so that our following replacements do not create extra newlines.
			retVal=retVal.Replace("\r","\n");//After this step, all newlines are in the form \n.
			retVal=retVal.Replace("\n","\r\n");//After this step, all newlines will be in form \r\n.
			return retVal;
		}

		/// <summary>Replaces new lines with a space. Emails with new line characters in the subject won't send.</summary>
		public static string SubjectTidy(string str) {
			string retVal=str.Replace("\r\n"," ");
			retVal=retVal.Replace("\r"," ");
			retVal=retVal.Replace("\n"," ");
			return retVal;
		}

		///<summary>Throws exceptions.  Attempts to physically send the message over the network wire.
		///Perfect for signed or encrypted email, because the MIME Content-Type is strictly defined for these types of emails.
		///Does not work for implicit SSL, but works for all other email settings, including explicit SSL.
		///If a message must be encrypted, then encrypt it before calling this function.
		///The patNum can be 0, but should be included if known, for auditing purposes.</summary>
		private static void WireEmailUnsecure(Health.Direct.Agent.OutgoingMessage msgOut,EmailAddress emailAddress,long patNum) {
			//No need to check RemotingRole; no call to db.
			//When batch email operations are performed, we sometimes do this check further up in the UI.  This check is here to as a catch-all.
            //Security.CurUser will be null if this is called from a third party application (like Patient Portal).  We want to continue if that is the case.
			if(Security.CurUser!=null && !Security.IsAuthorized(Permissions.EmailSend,true)) {//we need to suppress the message
				return;
			}
			if(emailAddress.IsImplicitSsl) {
				//The poor Content-Type header treatment by the System.Web.Mail.MailMessage class is the reason why both encrypted messages (Direct) and also signed unencrypted messages do not work though implicit SSL.
				//The System.Web.Mail.MailMessage class only understands plain text and html messages.
				//For a signed unencrypted message, the Content-Type header in the msgOut is "Content-Type: multipart/signed; boundary=PartA; protocol="application/pkcs7-signature"; micalg=sha1"
				//If the Content-Type header is added to the System.Web.Mail.MailMessage.Headers,
				//the Content-Type is modified to the following by C# when sending: "Content-Type: text/plain; boundary=PartA; protocol="application/pkcs7-signature"; micalg=sha1"
				throw new Exception(Lans.g("EmailMessages","Cannot send this type of message over implicit SSL."));
			}
			SmtpClient client=new SmtpClient(emailAddress.SMTPserver,emailAddress.ServerPort);
			//The default credentials are not used by default, according to: 
			//http://msdn2.microsoft.com/en-us/library/system.net.mail.smtpclient.usedefaultcredentials.aspx
			client.Credentials=new NetworkCredential(emailAddress.EmailUsername.Trim(),MiscUtils.Decrypt(emailAddress.EmailPassword));
			client.DeliveryMethod=SmtpDeliveryMethod.Network;
			client.EnableSsl=emailAddress.UseSSL;
			client.Timeout=180000;//3 minutes
			MailMessage message=new MailMessage();
			string contentType="text/plain";//This is the default value that C# would use if we did not specify a Content-Type.  However we need to specify the Content-Type for the AlternateView.
			for(int i=0;i<msgOut.Message.Headers.Count;i++) {//This copies all headers, including but not limited to: From/To/Subject/Date/MessageID/etc...
				string name=msgOut.Message.Headers[i].Name;
				string val=msgOut.Message.Headers[i].ValueRaw;
				if(name.ToUpper()=="BCC") {
					message.Bcc.Add(val.Trim());
				}
				else if(name.ToUpper()=="CC") {
					message.CC.Add(val.Trim());
				}
				else if(name.ToUpper()=="CONTENT-TYPE") {
					contentType=val;
				}
				else if(name.ToUpper()=="FROM") {
					message.From=new MailAddress(val.Trim());
				}
				else if(name.ToUpper()=="PRIORITY") {
					message.Priority=MailPriority.Normal;
					if(val.ToLower()=="high") {
						message.Priority=MailPriority.High;
					}
					else if(val.ToLower()=="low") {
						message.Priority=MailPriority.Low;
					}
				}
				else if(name.ToUpper()=="REPLY-TO") {
					message.ReplyTo=new MailAddress(val.Trim());
				}
				else if(name.ToUpper()=="REPLY-TO-LIST") {
					string[] arrayReplyTo=val.Split(',');
					for(int j=0;j<arrayReplyTo.Length;j++) {
						message.ReplyToList.Add(arrayReplyTo[j].Trim());
					}
				}
				else if(name.ToUpper()=="SENDER") {
					message.Sender=new MailAddress(val.Trim());
				}
				else if(name.ToUpper()=="SUBJECT") {
					message.Subject=SubjectTidy(val);
				}
				else if(name.ToUpper()=="TO") {
					message.To.Add(val.Trim());
				}
				else {//Other headers, such as MessageID, which is needed for Direct messaging, but is not part of the standard MailMessage object.
					message.Headers.Add(name,val);//Add to header verbatim.
				}
			}
			//Using an AlternateView is the only way to specify a custom Content-Type.  Both encrypted email and signed email messages have special Content-Types.  Necessary for Direct messaging.
			byte[] arrayContentBytes=Encoding.UTF8.GetBytes(msgOut.Message.Body.Text);//This includes the body and all attachments.  Should have already been formatted properly by the Direct library.
			MemoryStream msEmailContent=new MemoryStream(arrayContentBytes);
			msEmailContent.Position=0;
			AlternateView alternateView=new AlternateView(msEmailContent,contentType);
			alternateView.TransferEncoding=TransferEncoding.SevenBit;//Default is base64, but 7bit is much easier to read/debug.
			message.AlternateViews.Add(alternateView);
			client.Send(message);
			msEmailContent.Dispose();
			SecurityLogs.MakeLogEntry(Permissions.EmailSend,patNum,"Email Sent");
		}

		///<summary>Throws exceptions.  Attempts to physically send the message over the network wire.
		///This is used from wherever email needs to be sent throughout the program.
		///If a message must be encrypted, then encrypt it before calling this function.
		///nameValueCollectionHeaders can be null.</summary>
		private static void WireEmailUnsecure(EmailMessage emailMessage,EmailAddress emailAddress,NameValueCollection nameValueCollectionHeaders,params AlternateView[] arrayAlternateViews) {
			//No need to check RemotingRole; no call to db.
			//When batch email operations are performed, we sometimes do this check further up in the UI.  This check is here to as a catch-all.
			if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb//server can send email without checking user
				&& !Security.IsAuthorized(Permissions.EmailSend,true)) //we need to suppress the message
			{
				return;
			}
			emailMessage.UserNum=Security.CurUser.UserNum;
			if(emailAddress.ServerPort==465) {//implicit
				//uses System.Web.Mail, which is marked as deprecated, but still supports implicit
				//http://msdn.microsoft.com/en-us/library/ms877952(v=exchg.65).aspx
				System.Web.Mail.MailMessage message=new System.Web.Mail.MailMessage();
				message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver",emailAddress.SMTPserver);
				message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport","465");
				message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing","2");//sendusing: 1=pickup, 2=port, 3=using microsoft exchange
				message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate","1");//0=anonymous,1=clear text auth,2=context (NTLM)
				message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername",emailAddress.EmailUsername.Trim());
				message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword",MiscUtils.Decrypt(emailAddress.EmailPassword));
				//if(PrefC.GetBool(PrefName.EmailUseSSL)) {
				message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl","true");//false was also tested and does not work
				message.From=emailMessage.FromAddress.Trim();
				if(!string.IsNullOrWhiteSpace(emailMessage.ToAddress)) {
					message.To=emailMessage.ToAddress.Trim();
				}
				if(!string.IsNullOrWhiteSpace(emailMessage.CcAddress)) {
					message.Cc=emailMessage.CcAddress.Trim();
				}
				if(!string.IsNullOrWhiteSpace(emailMessage.BccAddress)) {
					message.Bcc=emailMessage.BccAddress.Trim();
				}
				message.Subject=SubjectTidy(emailMessage.Subject);
				message.Body=BodyTidy(emailMessage.BodyText);
				//message.Cc=;
				//message.Bcc=;
				//message.UrlContentBase=;
				//message.UrlContentLocation=;
				message.BodyEncoding=System.Text.Encoding.UTF8;
				message.BodyFormat=System.Web.Mail.MailFormat.Text;//or .Html
				if(nameValueCollectionHeaders!=null) {
					string[] arrayHeaderKeys=nameValueCollectionHeaders.AllKeys;
					for(int i=0;i<arrayHeaderKeys.Length;i++) {//Needed for Direct Acks to work.
						message.Headers.Add(arrayHeaderKeys[i],nameValueCollectionHeaders[arrayHeaderKeys[i]]);
					}
				}
				//TODO: We need to add some kind of alternatve view or similar replacement for outgoing Direct messages to work with SSL. Write the body to a temporary file and attach with the correct mime type and name?
				string attachPath=EmailAttaches.GetAttachPath();
				System.Web.Mail.MailAttachment attach;
				string attachFullPath;
				//foreach (string sSubstr in sAttach.Split(delim)){
				for(int i=0;i<emailMessage.Attachments.Count;i++) {
					if(CloudStorage.IsCloudStorage) {
						OpenDentalCloud.Core.TaskStateDownload state=CloudStorage.Download(attachPath,emailMessage.Attachments[i].ActualFileName);						
						attachFullPath=PrefC.GetRandomTempFile(Path.GetExtension(emailMessage.Attachments[i].ActualFileName));
						File.WriteAllBytes(attachFullPath,state.FileContent);
					}
					else {
						attachFullPath=ODFileUtils.CombinePaths(attachPath,emailMessage.Attachments[i].ActualFileName);
					}
					attach=new System.Web.Mail.MailAttachment(attachFullPath);
					//No way to set displayed filename on the MailAttachment object itself.  TODO: Copy the file to the temp directory in order to rename, then the attachment will go out with the correct name.
					message.Attachments.Add(attach);
				}
				System.Web.Mail.SmtpMail.SmtpServer=emailAddress.SMTPserver+":465";//"smtp.gmail.com:465";
				System.Web.Mail.SmtpMail.Send(message);
			}
			else {//explicit default port 587 
				SmtpClient client=new SmtpClient(emailAddress.SMTPserver,emailAddress.ServerPort);
				//The default credentials are not used by default, according to: 
				//http://msdn2.microsoft.com/en-us/library/system.net.mail.smtpclient.usedefaultcredentials.aspx
				client.Credentials=new NetworkCredential(emailAddress.EmailUsername.Trim(),MiscUtils.Decrypt(emailAddress.EmailPassword));
				client.DeliveryMethod=SmtpDeliveryMethod.Network;
				client.EnableSsl=emailAddress.UseSSL;
				client.Timeout=180000;//3 minutes
				MailMessage message=new MailMessage();
				message.From=new MailAddress(emailMessage.FromAddress.Trim());
				if(!string.IsNullOrWhiteSpace(emailMessage.ToAddress)) {
					message.To.Add(emailMessage.ToAddress.Trim());
				}
				if(!string.IsNullOrWhiteSpace(emailMessage.CcAddress)) {
					message.CC.Add(emailMessage.CcAddress.Trim());
				}
				if(!string.IsNullOrWhiteSpace(emailMessage.BccAddress)) {
					message.Bcc.Add(emailMessage.BccAddress.Trim());
				}
				message.Subject=SubjectTidy(emailMessage.Subject);
				message.Body=BodyTidy(emailMessage.BodyText);
				message.IsBodyHtml=false;
				if(nameValueCollectionHeaders!=null) {
					message.Headers.Add(nameValueCollectionHeaders);//Needed for Direct Acks to work.
				}
				for(int i=0;i<arrayAlternateViews.Length;i++) {//Needed for Direct messages to be interpreted encrypted on the receiver's end.
					message.AlternateViews.Add(arrayAlternateViews[i]);
				}
				Attachment attach;
				string attachPath=EmailAttaches.GetAttachPath();
				string attachFullPath;
				for(int i=0;i<emailMessage.Attachments.Count;i++) {
					if(CloudStorage.IsCloudStorage) {
						OpenDentalCloud.Core.TaskStateDownload state=CloudStorage.Download(attachPath,emailMessage.Attachments[i].ActualFileName);
						attachFullPath=PrefC.GetRandomTempFile(Path.GetExtension(emailMessage.Attachments[i].ActualFileName));
						File.WriteAllBytes(attachFullPath,state.FileContent);
					}
					else {
						attachFullPath=ODFileUtils.CombinePaths(attachPath,emailMessage.Attachments[i].ActualFileName);
					}
					attach=new Attachment(attachFullPath);
					//@"C:\OpenDentalData\EmailAttachments\1");
					attach.Name=emailMessage.Attachments[i].DisplayedFileName;
					//"canadian.gif";
					message.Attachments.Add(attach);
				}
				client.Send(message);
				if(RemotingClient.RemotingRole!=RemotingRole.ServerWeb){//user not accessible if patient portal or OC web server is sending an email
					SecurityLogs.MakeLogEntry(Permissions.EmailSend,emailMessage.PatNum,"Email Sent");
				}
			}
		}

		///<summary>Throws exceptions.  Uses the Direct library to sign the message, so that our unencrypted/signed messages are built the same way as our encrypted/signed messages.
		///The provided certificate must contain a private key, or else the signing will fail (exception) when computing the signature digest.</summary>
		public static void SendEmailUnsecureWithSig(EmailMessage emailMessage,EmailAddress emailAddressFrom,X509Certificate2 certPrivate) {
			if(emailAddressFrom.IsImplicitSsl) {
				throw new Exception(Lans.g("EmailMessages","Digitally signed messages cannot be sent over implicit SSL."));//See detailed comments in the private version of SendEmailUnsecure().
			}
			//No need to check RemotingRole; no call to db.
			emailMessage.UserNum=Security.CurUser.UserNum;
			emailMessage.FromAddress=emailAddressFrom.EmailUsername.Trim();//Cannot be emailAddressFrom.SenderAddress, or else will not find the correct signing certificate.  Used in ConvertEmailMessageToMessage().
			Health.Direct.Common.Mail.Message msg=ConvertEmailMessageToMessage(emailMessage,true);
			Health.Direct.Agent.MessageEnvelope msgEnvelope=new Health.Direct.Agent.MessageEnvelope(msg);
			Health.Direct.Agent.OutgoingMessage msgOut=new Health.Direct.Agent.OutgoingMessage(msgEnvelope);
			Health.Direct.Agent.DirectAgent directAgent=GetDirectAgentForEmailAddress(emailMessage.FromAddress);
			try {
				Health.Direct.Common.Cryptography.SignedEntity signedEntity=directAgent.Cryptographer.Sign(msgOut.Message,certPrivate);//Compute the signature digest.  A hash of the certificate against the raw email content.
				msgOut.Message.UpdateBody(signedEntity);//Modify the relevant message headers as well as the entire message body to include the signature digest.
			}
			catch(Exception ex) {
				throw new ApplicationException(Lans.g("EmailMessages","Failed to sign outgoing email message, probably due to permissions: ")+ex.Message);
			}
			WireEmailUnsecure(msgOut,emailAddressFrom,emailMessage.PatNum);
		}

		/// <summary>This is used from wherever unencrypted email needs to be sent throughout the program.  If a message must be encrypted, then encrypt it before calling this function.
		///Surround with a try catch.</summary>
		public static void SendEmailUnsecure(EmailMessage emailMessage,EmailAddress emailAddress) {
			//No need to check RemotingRole; no call to db.
			WireEmailUnsecure(emailMessage,emailAddress,null);
		}

		#endregion Sending

		#region Receiving

		///<summary>Fetches up to fetchCount number of messages from a POP3 server.  Set fetchCount=0 for all messages.  Typically, fetchCount is 0 or 1.
		///Example host name, pop3.live.com. Port is Normally 110 for plain POP3, 995 for SSL POP3.</summary>
		public static List<EmailMessage> ReceiveFromInbox(int receiveCount,EmailAddress emailAddressInbox) {
			List<EmailMessage> retVal=new List<EmailMessage>();
			if(_listCurrentlyReceivingEmailAddressNums.Select(x => x.EmailAddressNum).Contains(emailAddressInbox.EmailAddressNum)) {
				return retVal;//Already in the process of receving email. This can happen if the user clicks the refresh button at the same time the main polling thread is receiving.
			}
			_listCurrentlyReceivingEmailAddressNums.Add(emailAddressInbox);
			try {
				lock(emailAddressInbox) {
					retVal=ReceiveFromInboxThreadSafe(receiveCount,emailAddressInbox);
				}
			}
			catch(Exception) {
				throw;
			}
			finally {
				_listCurrentlyReceivingEmailAddressNums.RemoveAll(x => x.EmailAddressNum==emailAddressInbox.EmailAddressNum);
			}
			return retVal;
		}

		///<summary>Fetches up to fetchCount number of messages from a POP3 server.  Set fetchCount=0 for all messages.  Typically, fetchCount is 0 or 1.
		///Example host name, pop3.live.com. Port is Normally 110 for plain POP3, 995 for SSL POP3.</summary>
		private static List<EmailMessage> ReceiveFromInboxThreadSafe(int receiveCount,EmailAddress emailAddressInbox) {
			//No need to check RemotingRole; no call to db.
			List<EmailMessage> retVal=new List<EmailMessage>();
			//This code is modified from the example at: http://hpop.sourceforge.net/exampleFetchAllMessages.php
			using(OpenPop.Pop3.Pop3Client client=new OpenPop.Pop3.Pop3Client()) {//The client disconnects from the server when being disposed.
				client.Connect(emailAddressInbox.Pop3ServerIncoming,emailAddressInbox.ServerPortIncoming,emailAddressInbox.UseSSL,180000,180000,null);//3 minute timeout, just as for sending emails.
				client.Authenticate(emailAddressInbox.EmailUsername.Trim(),MiscUtils.Decrypt(emailAddressInbox.EmailPassword),OpenPop.Pop3.AuthenticationMethod.UsernameAndPassword);
				List <string> listMsgUids=client.GetMessageUids();//Get all unique identifiers for each email in the inbox.
				List<EmailMessageUid> listDownloadedMsgUids=EmailMessageUids.GetForRecipientAddress(emailAddressInbox.EmailUsername.Trim());
				List<string> listDownloadedMsgUidStrs=new List<string>();
				for(int i=0;i<listDownloadedMsgUids.Count;i++) {
					listDownloadedMsgUidStrs.Add(listDownloadedMsgUids[i].MsgId);
				}
				int msgDownloadedCount=0;
				for(int i=0;i<listMsgUids.Count;i++) {
					int msgIndex=i+1;//The message indicies are 1-based.
					string strMsgUid=listMsgUids[i];//Example: 1420562540.886638.p3plgemini22-06.prod.phx.2602059520
					OpenPop.Mime.Header.MessageHeader messageHeader=null;
					if(strMsgUid.Length==0) {
						//Message Uids are commonly used, but are optional according to the RFC822 email standard.
						//Uids are assgined by the sending client application, so they could be anything, but are supposed to be unique.
						//Additionally, most email servers are probably smart enough to create a Uid for any message where the Uid is missing.
						//In the worst case scenario, we create a Uid for the message based off of the message header information, which takes a little extra time, 
						//but is better than downloading old messages again, especially if some of those messages contain large attachments.
						messageHeader=client.GetMessageHeaders(msgIndex);//Takes 1-2 seconds to get this information from the server.  The message, minus body and minus attachments.
						strMsgUid=messageHeader.DateSent.ToString("yyyyMMddHHmmss")+emailAddressInbox.EmailUsername.Trim()+messageHeader.From.Address+messageHeader.Subject;
					}
					if(strMsgUid.Length>4000) {//The EmailMessageUid.MsgId field is only 4000 characters in size.
						strMsgUid=strMsgUid.Substring(0,4000);
					}
					if(listDownloadedMsgUidStrs.Contains(strMsgUid)) {
						continue;//Skip emails which have already been downloaded.
					}
					//messageHeader will only be defined if we created our own unique ID manually above.  MessageId is optional, just as the message UIDs are.
					if(messageHeader!=null && messageHeader.MessageId!="") {
						//The MessageId is usually generated by the email server.
						//The message does not have a UID, and the ID that we made up has not been downloaded before.  As a last resort we check the MessageId in 
						//the message header.  MessageId is different than the UID.  We should have used the MessageId as the second option in the past, but now 
						//we are stuck using it as a third option, because using MessageId as a second option would cause old emails to download again.
						strMsgUid=messageHeader.MessageId;//Example: xtbzX6Pumwpcn9NjhAJn5A@mcmail1.mcr.colo.comodo.net
						if(strMsgUid.Length>4000) {//The EmailMessageUid.MsgId field is only 4000 characters in size.
							strMsgUid=strMsgUid.Substring(0,4000);
						}
						if(listDownloadedMsgUidStrs.Contains(strMsgUid)) {
							continue;//Skip emails which have already been downloaded.
						}
					}
					//At this point, we know that the email is one which we have not downloaded yet.
					try {
						OpenPop.Mime.Message openPopMsg=client.GetMessage(msgIndex);//This is where the entire raw email is downloaded.
						bool isEmailFromInbox=true;
						if(openPopMsg.Headers.From.ToString().ToLower().Contains(emailAddressInbox.EmailUsername.Trim().ToLower())) {//The email Recipient and email From addresses are the same.
							//The email Recipient and email To or CC or BCC addresses are the same.  We have verified that a user can send an email to themself using only CC or BCC.
							if(String.Join(",",openPopMsg.Headers.To).ToLower().Contains(emailAddressInbox.EmailUsername.Trim().ToLower()) ||
								String.Join(",",openPopMsg.Headers.Cc).ToLower().Contains(emailAddressInbox.EmailUsername.Trim().ToLower()) ||
								String.Join(",",openPopMsg.Headers.Bcc).ToLower().Contains(emailAddressInbox.EmailUsername.Trim().ToLower()))
							{
								//Download this message because it was clearly sent from the user to theirself.
							}
							else {
								//Gmail will report sent email as if it is part of the Inbox. These emails will have the From address as the Recipient address, but the To address will be a different address.
								isEmailFromInbox=false;
							}
						}
						if(isEmailFromInbox) {
							string strRawEmail=openPopMsg.MessagePart.BodyEncoding.GetString(openPopMsg.RawMessage);
							EmailMessage emailMessage=ProcessRawEmailMessageIn(strRawEmail,0,emailAddressInbox,true);//Inserts to db.
							retVal.Add(emailMessage);
							msgDownloadedCount++;
						}
						EmailMessageUid emailMessageUid=new EmailMessageUid();
						emailMessageUid.RecipientAddress=emailAddressInbox.EmailUsername.Trim();
						emailMessageUid.MsgId=strMsgUid;
						EmailMessageUids.Insert(emailMessageUid);//Remember Uid was downloaded, to avoid email duplication the next time the inbox is refreshed.
						listDownloadedMsgUidStrs.Add(strMsgUid);
					}
					catch(ThreadAbortException) {
						//This can happen if the application is exiting. We need to leave right away so the program does not lock up.
						//Otherwise, this loop could continue for a while if there are a lot of messages to download.
						throw;
					}
					catch {
						//If one particular email fails to download, then skip it for now and move on to the next email.
					}
					if(receiveCount>0 && msgDownloadedCount>=receiveCount) {
						break;
					}
				}
			}
			//Since this function is fired automatically based on the inbox check interval, we also try to send the oldest unsent Ack.
			//The goal is to keep trying to send the Acks at a reasonable interval until they are successfully delivered.
			SendOldestUnsentAck(emailAddressInbox);
			return retVal;
		}

		///<summary>Parses a raw email into a usable object.</summary>
		private static Health.Direct.Agent.IncomingMessage RawEmailToIncomingMessage(string strRawEmailIn,EmailAddress emailAddressInbox) {
			//No need to check RemotingRole; no call to db.
			Health.Direct.Agent.IncomingMessage inMsg=null;
			string lastErrorMsg="";
			for(int i=0;i<5;i++) {//We will exit if unknown error or if previous error was the same as current error.
				try {
					inMsg=new Health.Direct.Agent.IncomingMessage(strRawEmailIn);//Used to parse all email (encrypted or not).
				}
				catch(Exception ex) {
					if(ex.Message==lastErrorMsg) {//Our last attempt to fix the issue failed.
						throw new ApplicationException("Failed to parse raw email message.\r\n"+ex.Message);
					}
					if(ex.Message=="Error=MissingHeaderValue") {
						//The "Welcome to Email" message from GoDaddy has a blank CC field which causes the IncomingMessage() constructor to throw an exception.
						//The TO header can be blank because it is not required, since the user could put all destination addresses in either CC or BCC alone.  We tested this.
						strRawEmailIn=Regex.Replace(strRawEmailIn,@"TO:[ \t]*\r\n","",RegexOptions.IgnoreCase);//Remove the TO header if it is any number of spaces or tabs followed by exactly one newline.
						strRawEmailIn=Regex.Replace(strRawEmailIn,@"BCC:[ \t]*\r\n","",RegexOptions.IgnoreCase);//BCC before CC, since CC is partial match of BCC
						strRawEmailIn=Regex.Replace(strRawEmailIn,@"CC:[ \t]*\r\n","",RegexOptions.IgnoreCase);//Remove the CC header if it is any number of spaces or tabs followed by exactly one newline.
					}
					else if(ex.Message=="An invalid character was found in the mail header: ';'.") {
						//When all recipients are in the bcc field, some clients (gmail) inputs "undisclosed-recipients:;" into the TO field, which causes an error to be thrown.
						strRawEmailIn=Regex.Replace(strRawEmailIn,@"undisclosed-recipients:;","",RegexOptions.IgnoreCase);//Remove "undisclosed-recipients".
					}
					else if(ex.Message=="Error=NoRecipients") {
						//When all recipients are in the bcc field, some clients (Apple mail) remove all address fields (To, cc, bcc) from the header, which causes an error to be thrown.
						//the code below attempts to add a bcc field with the user's email into the header (seems to work for emails coming from Apple mail)
						strRawEmailIn=Regex.Replace(strRawEmailIn,@"Subject: ",
							"Bcc: "+emailAddressInbox.EmailUsername+"\r\nSubject: ",RegexOptions.IgnoreCase);
					}
					else {
						throw new ApplicationException("Failed to parse raw email message.\r\n"+ex.Message);
					}
					lastErrorMsg=ex.Message;
				}
			}
			return inMsg;
		}

		///<summary>Throws various exceptions if decryption fails.  Decryption will fail if the sender is not yet trusted by the recipient.  Decrypts and valudates trust.  If decrypted successfully, removes the sender signature from the decrypted attachments and moves them into inMsg.Signatures.</summary>
		private static Health.Direct.Agent.IncomingMessage DecryptIncomingMessage(Health.Direct.Agent.IncomingMessage inMsg) {
			//No need to check RemotingRole; no call to db.
			Health.Direct.Agent.DirectAgent directAgent=GetDirectAgentForEmailAddress(inMsg.Message.ToValue.Trim());
			//throw new ApplicationException("test decryption failure");
			return directAgent.ProcessIncoming(inMsg);//Decrypts and valudates trust.  Also removes the signature from the decrypted attachments and moves them into inMsg.Signatures.
		}

		///<summary>Converts any raw email message (encrypted or not) into an EmailMessage object, and saves any email attachments to the emailattach table in the db.
		///The emailMessageNum will be used to set EmailMessage.EmailMessageNum.  If emailMessageNum is 0, then the EmailMessage will be inserted into the db, otherwise the EmailMessage will be updated in the db.
		///If the raw message is encrypted, then will attempt to decrypt.  If decryption fails, then the EmailMessage SentOrReceived will be ReceivedEncrypted and the EmailMessage body will be set to the entire contents of the raw email.
		///If decryption succeeds, then EmailMessage SentOrReceived will be set to ReceivedDirect, the EmailMessage body will contain the decrypted body text, and a Direct Ack "processed" message will be sent back to the sender using the email settings from emailAddressReceiver.
		///Set isAck to true if decrypting a direct message, false otherwise.
		///Setting sentOrReceivedUnencrypted only works for unencrypted emails.  Currently used by DBM so that it doesn't force the status to received.</summary>
		public static EmailMessage ProcessRawEmailMessageIn(string strRawEmail,long emailMessageNum,EmailAddress emailAddressReceiver,bool isAck
			,EmailSentOrReceived sentOrReceivedUnencrypted=EmailSentOrReceived.Received) 
		{
			//No need to check RemotingRole; no call to db.
			Health.Direct.Agent.IncomingMessage inMsg=RawEmailToIncomingMessage(strRawEmail,emailAddressReceiver);
			bool isEncrypted=IsMimeEntityEncrypted(inMsg.Message);
			EmailMessage emailMessage=null;
			if(isEncrypted) {
				emailMessage=ConvertMessageToEmailMessage(inMsg.Message,false,false);//Exclude attachments until we decrypt.
				emailMessage.RawEmailIn=strRawEmail;//The raw encrypted email, including the message, the attachments, and the signature.  The body of the encrypted email is just a base64 string until decrypted.
				emailMessage.EmailMessageNum=emailMessageNum;
				emailMessage.SentOrReceived=EmailSentOrReceived.ReceivedEncrypted;
				emailMessage.RecipientAddress=emailAddressReceiver.EmailUsername.Trim();
				//The entire contents of the email are saved in the emailMessage.BodyText field, so that if decryption fails, the email will still be saved to the db for decryption later if possible.
				emailMessage.BodyText=strRawEmail;
				try {
					inMsg=DecryptIncomingMessage(inMsg);
					emailMessage=ConvertMessageToEmailMessage(inMsg.Message,true,false);//If the message was wrapped, then the To, From, Subject and Date can change after decyption. We also need to create the attachments for the decrypted message.
					emailMessage.RawEmailIn=strRawEmail;//The raw encrypted email, including the message, the attachments, and the signature.  The body of the encrypted email is just a base64 string until decrypted.
					emailMessage.EmailMessageNum=emailMessageNum;
					emailMessage.SentOrReceived=EmailSentOrReceived.ReceivedDirect;
					emailMessage.RecipientAddress=emailAddressReceiver.EmailUsername.Trim();
					if(inMsg.HasSenderSignatures) {
						for(int i=0;i<inMsg.SenderSignatures.Count;i++) {
							EmailAttach emailAttach=EmailAttaches.CreateAttach("smime.p7s","",inMsg.SenderSignatures[i].Certificate.GetRawCertData(),false);
							emailMessage.Attachments.Add(emailAttach);
						}
					}
				}
				catch(Exception ex) {
					//SentOrReceived will be ReceivedEncrypted, indicating to the calling code that decryption failed.
					//The decryption step may have failed due to an untrusted sender, in which case the decrypting actually took place and the signature was extracted.
					//We add the signature to the email message so it will show up next to the email message in the inbox and make it easier for the user to add trust for the sender.
					if(inMsg.HasSenderSignatures) {
						for(int i=0;i<inMsg.SenderSignatures.Count;i++) {
							EmailAttach emailAttach=EmailAttaches.CreateAttach("smime.p7s","",inMsg.SenderSignatures[i].Certificate.GetRawCertData(),false);
							emailMessage.Attachments.Add(emailAttach);
						}
					}
					if(emailMessageNum==0) {
						EmailMessages.Insert(emailMessage);
						return emailMessage;//If the message was just downloaded, then this function was called from the inbox, simply return the inserted email without an exception (it can be decypted later manually by the user).
					}
					//Do not update if emailMessageNum<>0, because nothing changed (was encrypted and still is).
					throw ex;//Throw an exception if trying to decrypt an email that was already in the database, so the user can see the error message in the UI.
				}
			}
			else {//Unencrypted
				//First check to see if attachments have already been digested for this email.
				List<EmailAttach> listEmailAttaches=EmailAttaches.GetForEmail(emailMessageNum); //will return an empty list if emailmessagenum == 0
				bool parseAttachments=true;//Always parse attachments from the strRawEmail unless we've already parsed them before.
				if(listEmailAttaches.Count > 0) {
					//Attachments have already been parsed so do not waste time re-parsing.
					//Re-parsing attachments would be very bad because there is a good chance that strRawEmail has cleared out the body portion of attachments.
					//The actual attachments will be affected (erased) if these attachments are re-parsed due to the body portions being blank.
					parseAttachments=false;
				}
				emailMessage=ConvertMessageToEmailMessage(inMsg.Message,parseAttachments,false);
				emailMessage.RawEmailIn=strRawEmail;
				//Set the Attachments on emailMessage if the attachments weren't parsed from strRawEmail.
				if(!parseAttachments) {
					//Calling EmailMessages.Update() will delete all email attachments and sync them with the current list of attachments even if no changes.
					//Therefore, we need to make sure to have the Attachments variable set to the "old" (current really) list of attachments.
					emailMessage.Attachments=listEmailAttaches;
				}
				//Only try and trim the fat from the RawEmailIn column if attachments are present.
				if(GetAttachmentMimeParts(inMsg.Message,1).Count==1) {
					//At this point we know that the attachments have been successfully extracted from the raw message (now or some time in the past).
					//Try to remove the attachment text from the raw email as to save space in the database.
					try {
						emailMessage.RawEmailIn=DissolveAttachmentsFromIncomingMessage(inMsg);
					}
					catch {
						//Something went wrong so keep the "bloat" in the database because it is the safest option.
					}
				}
				else {//No attachments present.
					//No need to try and annul attachment body text from strRawEmail because it doesn't have any attachments.
				}
				emailMessage.EmailMessageNum=emailMessageNum;
				emailMessage.SentOrReceived=sentOrReceivedUnencrypted;
				emailMessage.RecipientAddress=emailAddressReceiver.EmailUsername.Trim();
			}
			EhrSummaryCcd ehrSummaryCcd=null;
			if(isEncrypted) {
				for(int i=0;i<emailMessage.Attachments.Count;i++) {
					if(Path.GetExtension(emailMessage.Attachments[i].ActualFileName).ToLower()!=".xml") {
						continue;
					}
					string strAttachPath=EmailAttaches.GetAttachPath();
					string strAttachText=FileAtoZ.ReadAllText(FileAtoZ.CombinePaths(strAttachPath,emailMessage.Attachments[i].ActualFileName));
					if(EhrCCD.IsCCD(strAttachText)) {
						if(emailMessage.PatNum==0) {
							try {
								XmlDocument xmlDocCcd=new XmlDocument();
								xmlDocCcd.LoadXml(strAttachText);
								emailMessage.PatNum=EhrCCD.GetCCDpat(xmlDocCcd);// A match is not guaranteed, which is why we have a button to allow the user to change the patient.
							}
							catch {
								//Invalid XML.  Cannot match patient.
							}
						}
						ehrSummaryCcd=new EhrSummaryCcd();
						ehrSummaryCcd.ContentSummary=strAttachText;
						ehrSummaryCcd.DateSummary=DateTime.Today;
						ehrSummaryCcd.EmailAttachNum=i;//Temporary value, so we can locate the FK down below.
						ehrSummaryCcd.PatNum=emailMessage.PatNum;
						break;//We can only handle one CCD message per email, because we only have one patnum field per email record and the ehrsummaryccd record requires a patnum.
					}
				}
			}
			if(emailMessage.PatNum==0) {//If a patient match was not already found, try to locate patient based on the email address sent from.
				string emailFromAddress=GetAddressSimple(emailMessage.FromAddress); 
				List<Patient> listMatchedPats=Patients.GetPatsByEmailAddress(emailFromAddress);
				if(listMatchedPats.Count==1) {//If multiple matches, then we do not want to mislead the user by assigning a patient.
					emailMessage.PatNum=listMatchedPats[0].PatNum;
				}
			}
			if(emailMessageNum==0) {
				EmailMessages.Insert(emailMessage);//Also inserts all of the attachments in emailMessage.Attachments after setting each attachment EmailMessageNum properly.
			}
			else {
				EmailMessages.Update(emailMessage);
			}
			if(ehrSummaryCcd!=null) {
				ehrSummaryCcd.EmailAttachNum=emailMessage.Attachments[(int)ehrSummaryCcd.EmailAttachNum].EmailAttachNum;
				EhrSummaryCcds.Insert(ehrSummaryCcd);
			}
			if(isEncrypted && isAck) {
				//Send a Message Disposition Notification (MDN) message to the sender, as required by the Direct messaging specifications.
				//The MDN will be attached to the same patient as the incoming message.
				SendAckDirect(inMsg,emailAddressReceiver,emailMessage.PatNum);
			}
			return emailMessage;
		}

		///<summary>Email bodies can have multiple parts.  Usually, for HTML email, there will be one HTML mime part plus one mime part for each image (in base64) which is part of the email message.  HTML messages usually also have one mime part for the text version of the email message, in case the email client does not have html capabilities.  This function extracts the text for all mime body parts which fully or partially match the specified mime content types.  For example, you could specify a mime content of "image/" to find images of all types, or you could specify a mime content type of "image/jpeg" to find only jpeg images.  Always returns one valid list for each specified mime content types, where the individual lists are always present but may be zero length.</summary>
		public static List<List<Health.Direct.Common.Mime.MimeEntity>> GetMimePartsForMimeTypes(string strRawEmailIn,EmailAddress emailAddressInbox,
			params string[] arrayMimeContentTypes)
		{
			//No need to check RemotingRole; no call to db.
			Health.Direct.Agent.IncomingMessage inMsg=null;
			List<Health.Direct.Common.Mime.MimeEntity> listMimeLeafNodes=null;
			try {
				inMsg=RawEmailToIncomingMessage(strRawEmailIn,emailAddressInbox);
				if(IsMimeEntityEncrypted(inMsg.Message)) {
					inMsg=DecryptIncomingMessage(inMsg);
				}
				listMimeLeafNodes=GetMimeLeafNodes(inMsg.Message);
				//If we were unable to read the mime parts, we will treat it as none found.
				listMimeLeafNodes=listMimeLeafNodes??new List<Health.Direct.Common.Mime.MimeEntity>();
			}
			catch {
				//Since we could not read the message, we cannot read the mime parts.  Therefore, none found.
				listMimeLeafNodes=new List<Health.Direct.Common.Mime.MimeEntity>();
			}
			List<List<Health.Direct.Common.Mime.MimeEntity>> retVal=new List<List<Health.Direct.Common.Mime.MimeEntity>>();
			for(int i=0;i<arrayMimeContentTypes.Length;i++) {
				string mimeContentType=arrayMimeContentTypes[i];
				List<Health.Direct.Common.Mime.MimeEntity> listMimeParts=new List<Health.Direct.Common.Mime.MimeEntity>();
				for(int j=0;j<listMimeLeafNodes.Count;j++) {
					if(listMimeLeafNodes[j].ContentType.Contains(mimeContentType)) {
						listMimeParts.Add(listMimeLeafNodes[j]);
					}
				}
				retVal.Add(listMimeParts);
			}			
			return retVal;
		}

		public static string GetMimeImageFileName(Health.Direct.Common.Mime.MimeEntity mimeEntityForImage) {
			int nameIndexStart=mimeEntityForImage.ContentType.ToLower().IndexOf("name=");
			if(nameIndexStart>=0) {
				nameIndexStart+=5;
			}
			else {
				nameIndexStart=mimeEntityForImage.ContentType.ToLower().IndexOf("filename=");
				if(nameIndexStart>=0) {
					nameIndexStart+=9;
				}
			}
			if(nameIndexStart<0) {
				return null;
			}
			int nameIndexEnd=mimeEntityForImage.ContentType.IndexOf(';',nameIndexStart+1);
			string fileName="";
			if(nameIndexEnd>=0) {
				fileName=mimeEntityForImage.ContentType.Substring(nameIndexStart,nameIndexEnd-nameIndexStart+1);
			}
			else {
				fileName=mimeEntityForImage.ContentType.Substring(nameIndexStart);
			}
			return fileName.Replace("\"","");
		}

		public static string GetMimeImageContentId(Health.Direct.Common.Mime.MimeEntity mimeEntityForImage) {
			if(!mimeEntityForImage.Headers.Contains("Content-ID")) {
				return "";
			}
			return mimeEntityForImage.Headers["Content-ID"].Value.Replace("<","").Replace(">","");
		}

		///<summary>Generates the image and returns the path to where the file was saved.  Returns null if the image could not be created.
		///Used to save images for received html messages.</summary>
		public static string SaveMimeImageToFile(Health.Direct.Common.Mime.MimeEntity mimeEntityForImage,string directoryPath) {
			//No need to check RemotingRole; no call to db.
			if(!IsMimeEntityBase64(mimeEntityForImage)) {
				return null;
			}
			try {
				byte[] bytesForImage=Convert.FromBase64String(mimeEntityForImage.Body.Text);
				MemoryStream ms=new MemoryStream(bytesForImage);
				Bitmap bitmap=new Bitmap(ms);
				string fileName=GetMimeImageFileName(mimeEntityForImage);
				string fileExt=Path.GetExtension(fileName);
				string filePath=ODFileUtils.CombinePaths(directoryPath,fileName);
				System.Drawing.Imaging.ImageFormat imageFormat=System.Drawing.Imaging.ImageFormat.Jpeg;
				if(fileExt.ToLower()==".bmp") {
					imageFormat=System.Drawing.Imaging.ImageFormat.Bmp;
				}
				else if(fileExt.ToLower()==".emf") {
					imageFormat=System.Drawing.Imaging.ImageFormat.Emf;
				}
				else if(fileExt.ToLower()==".exif") {
					imageFormat=System.Drawing.Imaging.ImageFormat.Exif;
				}
				else if(fileExt.ToLower()==".gif") {
					imageFormat=System.Drawing.Imaging.ImageFormat.Gif;
				}
				else if(fileExt.ToLower()==".ico") {
					imageFormat=System.Drawing.Imaging.ImageFormat.Icon;
				}
				else if(fileExt.ToLower()==".jpg" || fileExt.ToLower()==".jpeg") {
					imageFormat=System.Drawing.Imaging.ImageFormat.Jpeg;
				}
				else if(fileExt.ToLower()==".png") {
					imageFormat=System.Drawing.Imaging.ImageFormat.Png;
				}
				else if(fileExt.ToLower()==".tif" || fileExt.ToLower()==".tiff") {
					imageFormat=System.Drawing.Imaging.ImageFormat.Tiff;
				}
				else if(fileExt.ToLower()==".wmf") {
					imageFormat=System.Drawing.Imaging.ImageFormat.Wmf;
				}
				bitmap.Save(filePath,imageFormat);
				bitmap.Dispose();
				ms.Dispose();
				return filePath;
			}
			catch {
			}
			return null;
		}

		#endregion Receiving

		#region Helpers

		///<summary>Throws an exception if there is a permission issue.  Creates all of the necessary certificate stores for email encryption (Direct and Standard) if they do not already exist.
		///There is no way for the user to create these stores manually through Microsoft Management Console (mmc.exe) and they are needed to import certificates.</summary>
		public static void CreateCertificateStoresIfNeeded() {
			//No need to check RemotingRole; no call to db.
			Health.Direct.Common.Certificates.SystemX509Store.OpenAnchorEdit().Dispose();//Create the NHINDAnchor certificate store if it does not already exist on the local machine.
			Health.Direct.Common.Certificates.SystemX509Store.OpenExternalEdit().Dispose();//Create the NHINDExternal certificate store if it does not already exist on the local machine.
			Health.Direct.Common.Certificates.SystemX509Store.OpenPrivateEdit().Dispose();//Create the NHINDPrivate certificate store if it does not already exist on the local machine.
		}

		///<summary>Traverses the mime tree of the given email message and returns all attachment mime parts,
		///including older attachments from the beginning of the email thread.
		///Set limitCount to a number greater than 0 if you wish to stop searching for attachments once this threshold is met.</summary>
		private static List <Health.Direct.Common.Mime.MimeEntity> GetAttachmentMimeParts(Health.Direct.Common.Mail.Message message,int limitCount=0) {
			//No need to check RemotingRole; no call to db.
			List <Health.Direct.Common.Mime.MimeEntity> listAttachments=new List<Health.Direct.Common.Mime.MimeEntity>();
			List <Health.Direct.Common.Mime.MimeEntity> listMimeParts=new List<Health.Direct.Common.Mime.MimeEntity>();
			if(message.IsMultiPart) {
				listMimeParts.AddRange(message.GetParts());
			}
			while(listMimeParts.Count > 0) {//Traverse all branches of the mime tree to locate all attachment mime parts.
				Health.Direct.Common.Mime.MimeEntity mimeEntity=listMimeParts[0];
				listMimeParts.RemoveAt(0);
				if(mimeEntity.ContentDisposition!=null && mimeEntity.ContentDisposition.ToLower().Contains("attachment")) {//An email attachment.  Leaf node.
					if(mimeEntity.HasBody) {
						//Clear out the body or content of the attachment as to reduce the amount of space we take up in the database.
						//This is safe to do at this point because we have already extracted the attachments and they are stored in the AtoZ folder or db already.
						//Since the Text of the body for the mimeEntity is protected, we need to replace the current mime body with a new mime body.
						listAttachments.Add(mimeEntity);
						if(limitCount > 0 && listAttachments.Count==limitCount) {
							return listAttachments;
						}
					}
				}
				else if(mimeEntity.IsMultiPart) {//Branch node.
					listMimeParts.AddRange(mimeEntity.GetParts());//Push children mime parts to stack to be examined in a later pass through the loop.
				}
			}
			return listAttachments;
		}

		///<summary>Annuls the attachment text in the body section of all attachment mime parts (the Base64 content).
		///Returns the entire incoming message as raw text which is meant to be stored in the RawEmailIn column as to save space in the database.
		///Throws exceptions.</summary>
		private static string DissolveAttachmentsFromIncomingMessage(Health.Direct.Agent.IncomingMessage incomingMessage) {
			//No need to check RemotingRole; no call to db.
			string rawEmail=incomingMessage.SerializeMessage();//The original raw message, unaltered.
			List <Health.Direct.Common.Mime.MimeEntity> listAttachments=GetAttachmentMimeParts(incomingMessage.Message);
			foreach(Health.Direct.Common.Mime.MimeEntity attachment in listAttachments) {//Clear the body text of each raw attachment body.
				if(attachment.Body.Text.Length==0) {
					continue;//Body is already empty.  Nothing to do.
				}
				string rawAttachment=attachment.ToString();//This includes the mime headers as well as the body text.
				int attachIndex=rawEmail.IndexOf(rawAttachment);//Uniquely locate the mime text in the raw email (will be unique because of header timestamps)
				if(attachIndex < 0) {//Mime part not found in raw email?  Should be impossible.
					continue;//We do not want to crash for any reason when running DBM tools.
				}
				//Now find the start index of the attachment body from where the attachment starts.
				int bodyIndex=rawEmail.IndexOf(attachment.Body.Text,attachIndex);
				if(bodyIndex > attachIndex+rawAttachment.Length-1) {//The body text match located was beyond the attachment boundary.  Should be impossible.
					continue;//We do not want to crash for any reason when running DBM tools.
				}
				rawEmail=rawEmail.Remove(bodyIndex,attachment.Body.Text.Length);
			}
			return rawEmail;
		}

		///<summary>Throws exceptions if there are permission issues.  Recreates the directagent in order to refresh the certificate stores.</summary>
		private static Health.Direct.Agent.DirectAgent GetDirectAgentForEmailAddress(params string[] arrayEmailAddresses) {
			//No need to check RemotingRole; no call to db.
			List <string> listDomains=new List<string>();
			for(int i=0;i<arrayEmailAddresses.Length;i++) {
				listDomains.Add(GetDomainForAddress(arrayEmailAddresses[i]));
			}
			Health.Direct.Common.Domains.StaticDomainResolver resDomain=new Health.Direct.Common.Domains.StaticDomainResolver(listDomains.ToArray());
			ICertificateResolver resPriv=new EmailPrivateResolver();
			ICertificateResolver resPub=new EmailPublicResolver();
			SystemX509Store storeAnchors=SystemX509Store.OpenAnchor();
			X509Certificate2Collection colAnchors=storeAnchors.GetAllCertificates();
			storeAnchors.Dispose();
			TrustAnchorResolver resTrust=new TrustAnchorResolver(colAnchors);
			_directAgent=new Health.Direct.Agent.DirectAgent(resDomain,resPriv,resPub,resTrust);
			_directAgent.EncryptMessages=true;
			//The Transport Testing Tool (TTT) complained when we sent a message that was not wrapped.
			//Specifically, the tool looks for the headers Orig-Date and Message-Id after the message is decrypted.
			//See http://tools.ietf.org/html/rfc5322#section-3.6.1 and http://tools.ietf.org/html/rfc5322#section-3.6.4 for details about these two header fields.
			_directAgent.WrapMessages=true;
			return _directAgent;
		}

		///<summary>Returns -1 if the given address has at least one known and trusted certificate.
		///Returns a non-negative count of the number of known untrusted certificates if there are no known trusted certificates.</summary>
		public static int GetReceiverUntrustedCount(string strAddressTest) {
			//No need to check RemotingRole; no call to db.
			EmailPublicResolver resPub=new EmailPublicResolver();
			List<X509Certificate2> listValidCerts=new List<X509Certificate2>();
			List<X509Certificate2> listInvalidCerts=new List<X509Certificate2>();
			resPub.GetCertificates(strAddressTest,listValidCerts,listInvalidCerts);
			if(listValidCerts.Count > 0) {
				return -1;
			}
			return listInvalidCerts.Count;
		}

		public static bool IsSenderTrusted(string strAddressTest) {
			//No need to check RemotingRole; no call to db.
			if(strAddressTest.Trim()=="") {
				return false;
			}
			if(_directAgent==null) {
				GetDirectAgentForEmailAddress(strAddressTest);
			}
			SystemX509Store anchorStore=SystemX509Store.OpenAnchor();
			//Look for domain level and address level trust certificates (anchors).
			X509Certificate2Collection privCerts=new EmailPrivateResolver().GetCertificates(new MailAddress(strAddressTest));
			X509Certificate2Collection anchorCerts=anchorStore.GetAllCertificates();
			bool isTrusted=false;
			foreach(X509Certificate2 cert in privCerts) {
				if(_directAgent.TrustModel.CertChainValidator.IsTrustedCertificate(cert,anchorCerts)) {
					isTrusted=true;
					break;
				}
			}
			anchorStore.Dispose();
			return isTrusted;
		}

		///<summary>Returns true if trust already exists or has just been established for the given email address.</summary>
		public static bool TryAddTrustDirect(string strAddressTest,
			List<X509Certificate2> listDirectValidCerts=null,List<X509Certificate2> listDirectInvalidCerts=null)
		{
			//No need to check RemotingRole; no call to db.
			if(strAddressTest.Trim()=="") {
				return false;
			}
			if(listDirectValidCerts==null) {
				listDirectValidCerts=new List<X509Certificate2>();
			}
			if(listDirectInvalidCerts==null) {
				listDirectInvalidCerts=new List<X509Certificate2>();
			}			
			try {
				FindPublicCertForAddress(strAddressTest,listDirectValidCerts,listDirectInvalidCerts);
				EmailPublicResolver resPub=new EmailPublicResolver();
				List<X509Certificate2> listValidCerts=new List<X509Certificate2>();
				List<X509Certificate2> listInvalidCerts=new List<X509Certificate2>();
				resPub.GetCertificates(strAddressTest,listValidCerts,listInvalidCerts);
				if(listValidCerts.Count > 0 || listInvalidCerts.Count > 0) {
					Health.Direct.Common.Certificates.SystemX509Store storeAnchors=Health.Direct.Common.Certificates.SystemX509Store.OpenAnchorEdit();//Open for read and write.  Corresponds to NHINDAnchors/Certificates.
					storeAnchors.Add(listValidCerts);
					storeAnchors.Add(listInvalidCerts);
					storeAnchors.Dispose();
				}
				GetDirectAgentForEmailAddress(strAddressTest);//Force the cert stores to be refreshed within our DirectAgent instance.
				return true;
			}
			catch(Exception ex) {//Likely a network issue (FindPublicCertForAddress) or a permissions issue opening the anchors store.
				ex.DoNothing();
				return false;
			}
		}

		///<summary>Throws exceptions.  The smimeP7sFilePath must point to a smime.p7s file.</summary>
		public static X509Certificate2 GetEmailSignatureFromSmimeP7sFile(string smimeP7sFilePath) {
			X509Certificate2 signedCert2=null;
			try {
				X509Certificate signedCert1=X509Certificate2.CreateFromSignedFile(smimeP7sFilePath);//This is a public encryption key.
				signedCert2=new X509Certificate2(signedCert1);
			}
			catch(Exception ex) {
				throw new Exception(Lans.g("EmailMessages","Failed to load signature file")+". "+ex.Message);
			}
			return signedCert2;
		}

		///<summary>Returns the encryption/decryption certificate for the specified emailAddress from the store of private certificates, or returns null if none found.
		///Used for creating a signing signature in email encryption, which requires the private key (the public key alone is not enough, we tried it and an exception is thrown by Dot NET).
		///IMPORTANT: Be careful what you do with the private certificate.  It must never be shared with another party.</summary>
		public static X509Certificate2 GetCertFromPrivateStore(string emailAddress) {
			//No need to check RemotingRole; no call to db.
			//Look for domain level and address level trust certificates.
			MailAddress address=null;
			try {
				address=new MailAddress(emailAddress);
			}
			catch(Exception ex) {//This can happen if emailAddress is not formatted according to the email standard.
				ex.DoNothing();
				return null;
			}
			X509Certificate2Collection privCerts=null;
			try {
				privCerts=new EmailPrivateResolver().GetCertificates(address);
			}
			catch(Exception ex) {
				ex.DoNothing();//The private certificate store either does not exist or the user does not have read permission.  Probably does not exist.
			}
			if(privCerts==null || privCerts.Count==0) {
				return null;
			}
			return privCerts[0];
		}

		///<summary>Throws exceptions.</summary>
		public static void TryAddTrustForSignature(X509Certificate2 signedCert) {
			try {
				Health.Direct.Common.Certificates.SystemX509Store storePublicCerts=Health.Direct.Common.Certificates.SystemX509Store.OpenExternalEdit();//Open for read and write.  Corresponds to NHINDExternal/Certificates.
				storePublicCerts.Add(signedCert);//Write the pubic encryption certificate to the Windows certificate store.
			}
			catch(Exception ex) {
				throw new Exception(Lans.g("EmailMessages","Failed to save signature to encryption certificate store")+". "+ex.Message);
			}
			try {
				Health.Direct.Common.Certificates.SystemX509Store storeAnchors=Health.Direct.Common.Certificates.SystemX509Store.OpenAnchorEdit();//Open for read and write.  Corresponds to NHINDAnchors/Certificates.
				storeAnchors.Add(signedCert);//Adds to NHINDAnchors/Certificates within the windows certificate store manager (mmc).
			}
			catch(Exception ex) {
				throw new Exception(Lans.g("EmailMessages","Failed to save signature to trust certificate store")+". "+ex.Message);
			}
		}

		///<summary>Sometimes an email From address will contain the person's name along with their email address.  This function strips out the person's name if present.</summary>
		public static string GetAddressSimple(string emailAddress) {
      if(string.IsNullOrEmpty(emailAddress)) {
        return "";
      }
			if(emailAddress.Contains("<") && emailAddress.Contains(">")) {
				int startIndex=emailAddress.IndexOf("<")+1;
				int endIndex=emailAddress.IndexOf(">")-1;
				return emailAddress.Substring(startIndex,endIndex-startIndex+1);
			}
			return emailAddress;
		}

		///<summary>The specified emailAddress must be a properly formatted email address or properly formatted domain name.</summary>
		private static string GetDomainForAddress(string emailAddress) {
			emailAddress=GetAddressSimple(emailAddress);
			if(emailAddress.Contains("@")) {
				return emailAddress.Substring(emailAddress.IndexOf("@")+1);//For example, if ToAddress is ehr@opendental.com, then this will be opendental.com
			}
			return emailAddress;
		}

		///<summary>Searches the internet (DNS and LDAP) for hosted public certificates.
		///If public certificates are discovered from the Internet, then existing certificates in the store for that address will be replaced with the
		///discovered certificates.  The trust for any certificate must be added separately.
		///Returns true if the strAddressTest given is a Direct address (certificates were located in DNS or LDAP).
		///Returns false if the strAddressTest is to be treated as a standard encrypted email address.
		///Throws exceptions when no certificates were found or if there was a network failure.</summary>
		private static bool FindPublicCertForAddress(string strAddressTest,List<X509Certificate2> listValidCerts,List<X509Certificate2> listInvalidCerts){
			//No need to check RemotingRole; no call to db.
			listValidCerts.Clear();
			listInvalidCerts.Clear();
			//It may be useful in the future to attempt communicating with a secondary DNS server if the primary DNS is not available.
			//const string strDnsServer = "184.73.237.102";//Amazon - This is the DNS server used within the Direct resolverPlugins test project. Appears to have worked the best for them, compared to the others listed below, but was not accessible.
			//const string strDnsServer = "10.110.22.16";//This address was tried in the Direct resolverPlugins test project and is commented out, implying that it might not be the best DNS server to use.
			//const string strDnsServer = "207.170.210.162";//This address was tried in the Direct resolverPlugins test project and is commented out, implying that it might not be the best DNS server to use.
			const string strGlobalDnsServer = "8.8.8.8";//Google - This address was tried in the Direct resolverPlugins test project and is commented out, implying that it might not be the best DNS server to use.
			IPAddress ipAddressGlobalDnsServer=IPAddress.Parse(strGlobalDnsServer);
			MailAddress mailAddressQuery=new MailAddress(strAddressTest);
			//Attempt to discover the certificate via DNS.
			DnsQueryForCert(ipAddressGlobalDnsServer,mailAddressQuery,listValidCerts,listInvalidCerts);
			//Always look in LDAP even if we found some certificates in DNS.  This is required for Direct Module H.1 to work for stage 3.
			Health.Direct.Common.Certificates.ICertificateResolver certResolverInternetLdap=new Health.Direct.ResolverPlugins.LdapCertResolver(ipAddressGlobalDnsServer,TimeSpan.FromMinutes(3));
			X509Certificate2Collection collectionCerts=certResolverInternetLdap.GetCertificates(mailAddressQuery);//Can return null.
			if(collectionCerts!=null) {
				for(int i=0;i<collectionCerts.Count;i++) {
					if(!EmailNameResolver.IsCertValid(collectionCerts[i])) {
						//If the certificate is not yet valid or is expired, then discard.
						listInvalidCerts.Add(collectionCerts[i]);
						continue;
					}
					listValidCerts.Add(collectionCerts[i]);
				}
			}
			//If any certificates were discovered via DNS or LDAP, save them locally for later reference.
			EmailPublicResolver resPub=null;
			if(listValidCerts.Count > 0 || listInvalidCerts.Count > 0) {
				resPub=new EmailPublicResolver(false);//Open for read/write.  Requires more permission.  Users do not usually have write permission.
				//At least one certificate was hosted in DNS or LDAP, which means that the emailAddress is a Direct address,
				//not a regular encrypted email address.				
				X509Certificate2Collection oldPubCerts=resPub.GetCertificatesForAddress(strAddressTest);//Address specific (excludes domain level certificates).
				if(oldPubCerts!=null && oldPubCerts.Count > 0) {
					//Remove other certs which are specifically for the address being queried, to make stores match DNS and LDAP results.
					resPub.Store.Remove(oldPubCerts);
					SystemX509Store storeAnchors=SystemX509Store.OpenAnchorEdit();
					storeAnchors.Remove(oldPubCerts);
					storeAnchors.Dispose();
				}
				resPub.Store.Add(listValidCerts);//Write the discovered certificates to the Windows certificate store for future reference.
				resPub.Store.Add(listInvalidCerts);//Write the discovered certificates to the Windows certificate store for future reference.
				return true;
			}
			if(resPub==null) {
				resPub=new EmailPublicResolver(true);//Open for read only.  Nearly all users have read-only permission.
			}
			//No certificates discovered in DNS or LDAP.  Either the address is not a Direct address or the servers are down.
			//Treat the address as a standard encrypted email address and get the existing certificates from the store.
			resPub.GetCertificates(strAddressTest,listValidCerts,listInvalidCerts);
			return false;
		}

		///<summary>Send certificate DNS query to DNS server IP address to look for an email encryption certificate for the given emailAddress.
		///Adds the discovered certificates (if any) to the two X509Certificate2 lists given.</summary>
		private static void DnsQueryForCert(IPAddress ipAddressDnsServer,MailAddress emailAddress,
			List<X509Certificate2> listCertsDiscoveredActive,List<X509Certificate2> listCertsDiscoveredInactive)
		{
			Health.Direct.Common.Certificates.ICertificateResolver certResolverInternetDns=
				new Health.Direct.Common.Certificates.DnsCertResolver(ipAddressDnsServer);
			X509Certificate2Collection collectionCerts=certResolverInternetDns.GetCertificates(emailAddress);//Can return null.
			if(collectionCerts!=null) {//Certificates found via DNS.  Remove any invalid or expired certificates.
				for(int i=0;i<collectionCerts.Count;i++) {
					if(DateTime.Now<collectionCerts[i].NotBefore || DateTime.Now>collectionCerts[i].NotAfter) {
						//If the certificate is not yet valid or is expired, then discard so we can possibly discover a better certificate below.
						listCertsDiscoveredInactive.Add(collectionCerts[i]);
						continue;
					}
					listCertsDiscoveredActive.Add(collectionCerts[i]);
				}
			}
		}

		///<summary>Called by Broadcaster service to determine if the OD DNS certificate service is up and running.  Checks for a specific email address
		///which we have registered.  May not be able to find the reference using the Find All Refrences tool.  Assume the head version of this function
		///is always the live version.</summary>
		public static bool ExistsActiveCertInDns(string ipAddressDnsServer,string emailAddress) {
			IPAddress ipAddressGlobalDnsServer=IPAddress.Parse(ipAddressDnsServer);
			MailAddress mailAddressQuery=new MailAddress(emailAddress);
			List<X509Certificate2> listCertsDiscoveredActive=new List<X509Certificate2>();
			List<X509Certificate2> listCertsDiscoveredInactive=new List<X509Certificate2>();
			DnsQueryForCert(ipAddressGlobalDnsServer,mailAddressQuery,listCertsDiscoveredActive,listCertsDiscoveredInactive);
			if(listCertsDiscoveredActive.Count >= 1) {
				return true;
			}
			return false;
		}

		///<summary>Gets all mime parts in the message which do not have child mime parts.  Returns null on error.</summary>
		private static List<Health.Direct.Common.Mime.MimeEntity> GetMimeLeafNodes(Health.Direct.Common.Mail.Message message) {
			//No need to check RemotingRole; no call to db.
			//Think of the mime structure as a tree.
			List<Health.Direct.Common.Mime.MimeEntity> listMimePartLeafNodes=new List<Health.Direct.Common.Mime.MimeEntity>();
			Health.Direct.Common.Mime.MimeEntity mimeEntity=null;
			try {
				mimeEntity=message.ExtractMimeEntity();
			}
			catch {
				return null;			
			}
			//If GetParts() is called when IsMultiPart is false, then an exception will be thrown by the Direct library.
			if(message.IsMultiPart) {
				List<Health.Direct.Common.Mime.MimeEntity> listMimeMultiPart=new List<Health.Direct.Common.Mime.MimeEntity>();
				listMimeMultiPart.Add(mimeEntity);
				while(listMimeMultiPart.Count>0) {
					foreach(Health.Direct.Common.Mime.MimeEntity mimePart in listMimeMultiPart[0].GetParts()) {
						if(mimePart.IsMultiPart) {
							listMimeMultiPart.Add(mimePart);
						}
						else {
							listMimePartLeafNodes.Add(mimePart);
						}
					}
					listMimeMultiPart.RemoveAt(0);
				}
			}
			else {//Single body part.
				listMimePartLeafNodes.Add(mimeEntity);
			}
			return listMimePartLeafNodes;
		}

		///<summary>Throws exceptions.  Converts the Health.Direct.Common.Mail.Message into an OD EmailMessage.  The Direct library is used for both encrypted and unencrypted email.  Set hasAttachments to false to exclude attachments.</summary>
		private static EmailMessage ConvertMessageToEmailMessage(Health.Direct.Common.Mail.Message message,bool hasAttachments,bool isOutbound) {
			//No need to check RemotingRole; no call to db.
			EmailMessage emailMessage=new EmailMessage();
			emailMessage.FromAddress=message.FromValue.Trim();
			if(message.DateValue!=null) {//Is null when sending, but should not be null when receiving.
				//The received email message date must be in a very specific format and must match the RFC822 standard.  Is a required field for RFC822.  http://tools.ietf.org/html/rfc822
				//We show the datetime that the email landed onto the email server instead of the datetime that the email was downloaded.
				//Examples: "3 Dec 2013 17:10:37 -0800", "10 Dec 2013 17:10:37 -0800", "Tue, 5 Nov 2013 17:10:37 +0000 (UTC)", "Tue, 12 Nov 2013 17:10:37 +0000 (UTC)"
				if(message.DateValue.EndsWith("GMT")) {//Examples: Tue, 09 Sep 2014 23:16:36 GMT
					emailMessage.MsgDateTime=DateTime.Parse(message.DateValue);
				}
				else if(message.DateValue.Contains(",")) {//The day-of-week, comma and following space are optional. Examples: "Tue, 3 Dec 2013 17:10:37 +0000", "Tue, 12 Nov 2013 17:10:37 +0000 (UTC)"
					try {
						emailMessage.MsgDateTime=DateTime.ParseExact(message.DateValue.Substring(0,31),"ddd, d MMM yyyy HH:mm:ss zzz",System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);
					}
					catch {
						emailMessage.MsgDateTime=DateTime.ParseExact(message.DateValue.Substring(0,30),"ddd, d MMM yyyy HH:mm:ss zzz",System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);
					}
				}
				else {//Examples: "3 Dec 2013 17:10:37 -0800", "12 Nov 2013 17:10:37 -0800 (UTC)"
					try {
						emailMessage.MsgDateTime=DateTime.ParseExact(message.DateValue.Substring(0,26),"d MMM yyyy HH:mm:ss zzz",System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);
					}
					catch {
						emailMessage.MsgDateTime=DateTime.ParseExact(message.DateValue.Substring(0,25),"d MMM yyyy HH:mm:ss zzz",System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat);
					}
				}
			}
			else {//Sending the email.
				emailMessage.MsgDateTime=DateTime.Now;
			}
			emailMessage.Subject=SubjectTidy(message.SubjectValue);
			emailMessage.ToAddress=POut.String(message.ToValue).Trim();//ToValue can be null if recipients were CC or BCC only.
			emailMessage.CcAddress=POut.String(message.CcValue).Trim();
			emailMessage.BccAddress=POut.String(message.BccValue).Trim();
			//Think of the mime structure as a tree.
			//We want to treat one part and multi-part emails the same way below, so we make our own list of leaf node mime parts (mime parts which have no children, also know as single part).
			List<Health.Direct.Common.Mime.MimeEntity> listMimePartLeafNodes=GetMimeLeafNodes(message);
			if(listMimePartLeafNodes==null) {
				emailMessage.BodyText=ProcessMimeTextPart(message);
				return emailMessage;
			}
			List<Health.Direct.Common.Mime.MimeEntity> listMimeBodyTextParts=new List<Health.Direct.Common.Mime.MimeEntity>();
			List<Health.Direct.Common.Mime.MimeEntity> listMimeAttachParts=new List<Health.Direct.Common.Mime.MimeEntity>();
			for(int i=0;i<listMimePartLeafNodes.Count;i++) {
				Health.Direct.Common.Mime.MimeEntity mimePart=listMimePartLeafNodes[i];
				if(mimePart.ContentDisposition==null || !mimePart.ContentDisposition.ToLower().Contains("attachment")) {//Not an email attachment.  Treat as body text.
					listMimeBodyTextParts.Add(mimePart);
				}
				else {
					listMimeAttachParts.Add(mimePart);
				}
			}
			string strTextPartBoundary="";
			if(listMimeBodyTextParts.Count>1) {
				strTextPartBoundary=message.ParsedContentType.Boundary;
			}
			StringBuilder sbBodyText=new StringBuilder("");
			if(isOutbound) {
				for(int i=0;i<listMimeBodyTextParts.Count;i++) {
					if(strTextPartBoundary!="") {//For outgoing Direct Ack messages.
						sbBodyText.Append("\r\n--"+strTextPartBoundary+"\r\n");
						sbBodyText.Append(listMimeBodyTextParts[i].ToString());//Includes not only the body text, but also content type and content disposition.
					}
					else {
						sbBodyText.Append(ProcessMimeTextPart(listMimeBodyTextParts[i]));
					}
				}
				if(strTextPartBoundary!="") {
					sbBodyText.Append("\r\n--"+strTextPartBoundary+"--\r\n");
				}
			}
			else {
				//All plain text body parts will show to the user in the chart module progress notes.
				for(int i=0;i<listMimeBodyTextParts.Count;i++) {
					if(IsMimeEntityTextPlain(listMimeBodyTextParts[i])) {
						sbBodyText.Append(ProcessMimeTextPart(listMimeBodyTextParts[i]));
					}
				}
			}
			emailMessage.BodyText=sbBodyText.ToString();
			emailMessage.Attachments=new List<EmailAttach>();
			if(!hasAttachments) {
				return emailMessage;
			}
			//If an encrypted attachment is present (smime.p7m), then ensure the message content type correctly indicates an encrypted message.
			for(int i=0;i<listMimeAttachParts.Count;i++) {
				Health.Direct.Common.Mime.MimeEntity mimePartAttach=listMimeAttachParts[i];
				if(mimePartAttach.ParsedContentType.Name.ToLower()=="smime.p7m") {//encrypted attachment
					message.ContentType="application/pkcs7-mime; name=smime.p7m; boundary="+strTextPartBoundary+";";
					break;
				}
			}
			try {
				for(int i=0;i<listMimeAttachParts.Count;i++) {
					Health.Direct.Common.Mime.MimeEntity mimePartAttach=listMimeAttachParts[i];
					byte[] arrayData=null;
					try {
						if(IsMimeEntityBase64(mimePartAttach)) {
							arrayData=Convert.FromBase64String(mimePartAttach.Body.Text);
						}
					}
					catch {
					}
					if(arrayData==null) {//Plain attachment.
						arrayData=Encoding.UTF8.GetBytes(mimePartAttach.Body.Text);
					}
					EmailAttach emailAttach=EmailAttaches.CreateAttach(mimePartAttach.ParsedContentType.Name,"",arrayData,isOutbound);
					emailMessage.Attachments.Add(emailAttach);//The attachment EmailMessageNum is set when the emailMessage is inserted/updated below.
				}
			}
			catch(Exception ex) {
				//Failed to extract all attachments from the email message.  Cleanup the attachments which were successfully extracted.
				for(int i=0;i<emailMessage.Attachments.Count;i++) {
					string attachFilePath=FileAtoZ.CombinePaths(EmailAttaches.GetAttachPath(),emailMessage.Attachments[i].ActualFileName);
					if(!FileAtoZ.Exists(attachFilePath)) { 
						continue;
					}
					try {
						FileAtoZ.Delete(attachFilePath);
					}
					catch {
						//Probably nothing else we can do.  At least continue to the remaining attachments to try deleting them as well.
					}
				}
				throw ex;
			}
			return emailMessage;
		}

		///<summary>Converts our internal EmailMessage object to a Direct message object.  Used for outgoing email.  Wraps the message.</summary>
		private static Health.Direct.Common.Mail.Message ConvertEmailMessageToMessage(EmailMessage emailMessage,bool hasAttachments) {
			//No need to check RemotingRole; no call to db.
			//We need to use emailAddressFrom.Username instead of emailAddressFrom.SenderAddress, because of how strict encryption is for matching the name to the certificate.
			Health.Direct.Common.Mail.Message message=new Health.Direct.Common.Mail.Message();
			if(!string.IsNullOrWhiteSpace(emailMessage.ToAddress)) {
				message.To=new Health.Direct.Common.Mime.Header("To",emailMessage.ToAddress.Trim());
			}
			message.From=new Health.Direct.Common.Mime.Header("From",emailMessage.FromAddress.Trim());
			//message.Body is set below.
			message.ContentType="text/plain";//Setting the default content type helps with signing.
			if(!string.IsNullOrWhiteSpace(emailMessage.CcAddress)) {
				message.CcValue=emailMessage.CcAddress.Trim();//constructor does not accept cc and bcc values
			}
			if(!string.IsNullOrWhiteSpace(emailMessage.BccAddress)) {
				message.BccValue=emailMessage.BccAddress.Trim();
			}
			string subject=SubjectTidy(emailMessage.Subject);
			if(subject!="") {
				Health.Direct.Common.Mime.Header headerSubject=new Health.Direct.Common.Mime.Header("Subject",subject);
				message.Headers.Add(headerSubject);
			}
			//The Transport Testing Tool (TTT) complained when we sent a message that was not wrapped.
			//It appears that wrapped messages are preferred when sending a message, although support for incoming wrapped messages is optional (unwrapped is required).  We support both unwrapped and wrapped.
			//Specifically, the tool looks for the headers Orig-Date and Message-Id after the message is decrypted, so we need to include these two headers before encrypting an outgoing email.
			//The message date must be in a very specific format and must match the RFC822 standard.  Is a required field for RFC822.  http://tools.ietf.org/html/rfc822
			string strOrigDate=DateTime.Now.ToString("ddd, dd MMM yyyy HH:mm:ss zzz");//Example: "Tue, 12 Nov 2013 17:10:37 +08:00", which has an extra colon in the Zulu offset.
			strOrigDate=strOrigDate.Remove(strOrigDate.LastIndexOf(':'),1);//Remove the colon from the Zulu offset, as required by the RFC 822 message format.
			message.Date=new Health.Direct.Common.Mime.Header("Date",strOrigDate);//http://tools.ietf.org/html/rfc5322#section-3.6.1
			message.AssignMessageID();//http://tools.ietf.org/html/rfc5322#section-3.6.4
			string strBoundry="";
			List<Health.Direct.Common.Mime.MimeEntity> listMimeParts=new List<Health.Direct.Common.Mime.MimeEntity>();
			string bodyText=BodyTidy(emailMessage.BodyText);
			if(bodyText.Trim().Length>4 && bodyText.Trim().StartsWith("--") && bodyText.Trim().EndsWith("--")) {//The body text is multi-part.
				strBoundry=bodyText.Trim().Split(new string[] { "\r\n","\r","\n" },StringSplitOptions.None)[0];
				string[] arrayBodyTextParts=bodyText.Trim().TrimEnd('-').Split(new string[] { strBoundry },StringSplitOptions.RemoveEmptyEntries);
				for(int i=0;i<arrayBodyTextParts.Length;i++) {
					Health.Direct.Common.Mime.MimeEntity mimeEntityBodyText=new Health.Direct.Common.Mime.MimeEntity(arrayBodyTextParts[i]);
					mimeEntityBodyText.ContentType="text/plain;";
					listMimeParts.Add(mimeEntityBodyText);
				}
			}
			else {
				Health.Direct.Common.Mime.MimeEntity mimeEntityBodyText=new Health.Direct.Common.Mime.MimeEntity(bodyText);
				mimeEntityBodyText.ContentType="text/plain;";
				listMimeParts.Add(mimeEntityBodyText);
			}
			if(hasAttachments && emailMessage.Attachments!=null && emailMessage.Attachments.Count>0) {
				string strAttachPath=EmailAttaches.GetAttachPath();
				for(int i=0;i<emailMessage.Attachments.Count;i++) {
					string strAttachFile=FileAtoZ.CombinePaths(strAttachPath,emailMessage.Attachments[i].ActualFileName);
					//We always attach with base64 encoding, so that we do not have to worry about violating the RFC822 email format with binary characters or invalid newlines.
					Health.Direct.Common.Mime.MimeEntity mimeEntityAttach=new Health.Direct.Common.Mime.MimeEntity(
						Convert.ToBase64String(FileAtoZ.ReadAllBytes(strAttachFile)));
					mimeEntityAttach.ContentTransferEncoding="base64";
					mimeEntityAttach.ContentDisposition="attachment; filename=\""+emailMessage.Attachments[i].DisplayedFileName+"\"";
					mimeEntityAttach.ContentType=Mime.GetMimeTypeForEmail(strAttachFile)+"; name=\""+emailMessage.Attachments[i].DisplayedFileName+"\"";
					listMimeParts.Add(mimeEntityAttach);
				}
			}
			if(strBoundry=="") {
				strBoundry=CodeBase.MiscUtils.CreateRandomAlphaNumericString(32);
			}
			if(listMimeParts.Count==1) {//Single body part
				message.Body=listMimeParts[0].Body;
			}
			else if(listMimeParts.Count>1) {//multiple body parts
				message.SetParts(listMimeParts,"multipart/mixed; boundary="+strBoundry+";");
			}
			return message;
		}

		public static string ProcessMimeTextPart(Health.Direct.Common.Mime.MimeEntity mimeEntity) {
			//No need to check RemotingRole; no call to db.
			string strBodyText=mimeEntity.Body.Text;
			//Convert clear text mime parts which are base64 encoded into utf8 to make the text readable (plain text, html, xml, etc...)
			//This includes messages which were received as encryped and which were successfully decrypted.
			if(!IsMimeEntityEncrypted(mimeEntity) && IsMimeEntityText(mimeEntity) && IsMimeEntityBase64(mimeEntity)) {
				Encoding enc=GetMimeEncoding(mimeEntity);
				byte[] arrayBodyBytes=Convert.FromBase64String(mimeEntity.Body.Text);
				strBodyText=enc.GetString(arrayBodyBytes);
			}
			//Official documentation regarding text wrapping.  http://www.ietf.org/rfc/rfc2646.txt
			//Both text and html bodies appear to be commonly wrapped at 75 characters with an extra '=' character added to the end of wrapped lines.
			//We have seen email wrapped at 75 characters from a number of sources, including GoDaddy and Comodo.
			//However, lines may be wrapped at any number of characters, so we cannot rely on the length of the line.
			//Instead we rely on the presence of a "soft line break" (a special character SP followed by CRLF).
			//I hard line break is a CRLF which is not preceded by the SP character.
			//The SP character can be any character, and from what we have seen, is usually the '=' character.
			string sp="=";//Soft line break indicator character.
			string[] arrayMimeBodyLines=strBodyText.Split(new string[] { "\r\n","\r","\n" },StringSplitOptions.None);
			StringBuilder sbBodyText=new StringBuilder();
			for(int i=0;i<arrayMimeBodyLines.Length;i++) {
				if(arrayMimeBodyLines[i].EndsWith(sp)) {//Soft line break.  The line ends with SP CRLF
					//The current line is wrapped.  Remove the trailing soft line break indicator character and also remove the new line.
					//The CRLF was already removed when splitting, so we only need to remove the soft line break indicator at the end.
					sbBodyText.Append(arrayMimeBodyLines[i].Substring(0,arrayMimeBodyLines[i].Length-1));
				}
				else {//Hard line break.
					//The current line is not wrapped.  Do not modify this line.  Also ensure that the CRLF is placed back into the output.
					sbBodyText.Append(arrayMimeBodyLines[i]);
					if(i<arrayMimeBodyLines.Length) {
						sbBodyText.AppendLine();
					}
				}
			}
			//Soft line breaks have now been removed from the message.
			//In the remaining message, the same special character is used to precede encoded characters.
			//For example, "=3D" needs to be converted to an '=' character, because 3D in hexadecimal is the '=' character.
			//Another example, "=20" would be converted to a ' ' character.
			string strBodyTextUnwrapped=sbBodyText.ToString();
			string[] arrayBodyEncoded=strBodyTextUnwrapped.Split(new string[] { sp },StringSplitOptions.None);
			StringBuilder retVal=new StringBuilder();
			if(arrayBodyEncoded.Length>0) {
				retVal.Append(arrayBodyEncoded[0]);
				for(int i=1;i<arrayBodyEncoded.Length;i++) {
					if(Regex.IsMatch(arrayBodyEncoded[i],"^[0-9A-F]{2}.*")) {//Starts with a 2 digit hexadecimal number.
						string hexStr=arrayBodyEncoded[i].Substring(0,2);
						char c=(char)Convert.ToInt32(hexStr,16);
						retVal.Append(c);
						retVal.Append(arrayBodyEncoded[i].Substring(2));
					}
					else {
						retVal.Append(arrayBodyEncoded[i]);
					}
				}
			}
			return retVal.ToString();
		}

		private static Encoding GetMimeEncoding(Health.Direct.Common.Mime.MimeEntity mimeEntity) {
			//We cannot use mimeEntity.ParsedContentType.CharSet because it fails if there are spaces around the equal sign of the charset statement.
			string contentType=mimeEntity.ContentType.ToLower();
			int charsetIndex=contentType.IndexOf("charset");
			if(charsetIndex < 0) {
				throw new ApplicationException("Mime encoding not specified.");
			}
			charsetIndex=contentType.IndexOf("=",charsetIndex+7);//Find the '=' after the "charset" string (ignoring spaces).
			if(charsetIndex < 0) {
				throw new ApplicationException("Mime encoding incorrectly specified.");
			}
			charsetIndex++;//Skip '='
			while(charsetIndex < contentType.Length && Char.IsWhiteSpace(contentType[charsetIndex])) {//Skip white space after '='
				charsetIndex++;
			}
			string encName="";
			while(charsetIndex < contentType.Length && contentType[charsetIndex]!=';' && !Char.IsWhiteSpace(contentType[charsetIndex])) {
				encName+=contentType[charsetIndex];
				charsetIndex++;
			}
			encName=encName.Replace("\"","");//Remove double-quotes
			Encoding enc=null;
			try {
				enc=Encoding.GetEncoding(encName);
			}
			catch {
				if(encName.ToLower().StartsWith("cp")) {
					string codePage=encName.Substring(2);
					enc=Encoding.GetEncoding(int.Parse(codePage));
				}
			}
			return enc;
		}

		private static bool IsMimeEntityEncrypted(Health.Direct.Common.Mime.MimeEntity mimeEntity) {
			//No need to check RemotingRole; no call to db.
			if(mimeEntity.ContentType!=null && mimeEntity.ContentType.ToLower().Contains("application/pkcs7-mime")) {
				return true;//The email MIME/body is encrypted (known as S/MIME).  Treated as an Encrypted/Direct message.
			}
			return false;
		}

		private static bool IsMimeEntityBase64(Health.Direct.Common.Mime.MimeEntity mimeEntity) {
			//No need to check RemotingRole; no call to db.
			if(mimeEntity.ContentTransferEncoding!=null && mimeEntity.ContentTransferEncoding.ToLower().Contains("base64")) {
				return true;
			}
			return false;
		}

		///<summary>Returns true if plain text, xml, html, etc...</summary>
		private static bool IsMimeEntityText(Health.Direct.Common.Mime.MimeEntity mimeEntity) {
			//No need to check RemotingRole; no call to db.
			if(mimeEntity.ContentType!=null && mimeEntity.ContentType.ToLower().Contains("text/")) {
				return true;
			}
			return false;
		}

		///<summary>Returns true if plain text, xml, html, etc...</summary>
		private static bool IsMimeEntityTextPlain(Health.Direct.Common.Mime.MimeEntity mimeEntity) {
			//No need to check RemotingRole; no call to db.
			if(mimeEntity.ContentType!=null && mimeEntity.ContentType.ToLower().Contains("text/plain")) {
				return true;
			}
			return false;
		}

		public static string GetEmailSentOrReceivedDescript(EmailSentOrReceived sentOrReceived) {
			//No need to check RemotingRole; no call to db.
			if(IsRegularEmail(sentOrReceived)) {
				return Lans.g("EmailMessages","Regular Email");
			}
			if(IsEncryptedEmail(sentOrReceived)) {
				return Lans.g("EmailMessages","Encrypted Email");
			}
			if(IsSecureWebMail(sentOrReceived)) {
				return Lans.g("EmailMessages","Secure Web Mail");
			}
			if(IsUnsent(sentOrReceived)) {
				return Lans.g("EmailMessages","Unsent");
			}			
			return "";
		}

		public static bool IsRegularEmail(EmailSentOrReceived sentOrReceived) {
			//No need to check RemotingRole; no call to db.
			return (sentOrReceived==EmailSentOrReceived.Read || sentOrReceived==EmailSentOrReceived.Received || sentOrReceived==EmailSentOrReceived.Sent);
		}

		public static bool IsEncryptedEmail(EmailSentOrReceived sentOrReceived) {
			//No need to check RemotingRole; no call to db.
			return (sentOrReceived==EmailSentOrReceived.ReadDirect || sentOrReceived==EmailSentOrReceived.ReceivedDirect || 
				sentOrReceived==EmailSentOrReceived.SentDirect || sentOrReceived==EmailSentOrReceived.ReceivedEncrypted || 
				sentOrReceived==EmailSentOrReceived.AckDirectNotSent || sentOrReceived==EmailSentOrReceived.AckDirectProcessed);
		}

		public static bool IsSecureWebMail(EmailSentOrReceived sentOrReceived) {
			//No need to check RemotingRole; no call to db.
			return (sentOrReceived==EmailSentOrReceived.WebMailRecdRead || sentOrReceived==EmailSentOrReceived.WebMailReceived ||
				sentOrReceived==EmailSentOrReceived.WebMailSent || sentOrReceived==EmailSentOrReceived.WebMailSentRead);
		}

		public static bool IsUnsent(EmailSentOrReceived sentOrReceived) {
			//No need to check RemotingRole; no call to db.
			return (sentOrReceived==EmailSentOrReceived.Neither);
		}

		public static bool IsReceived(EmailSentOrReceived sentOrReceived) {
			//No need to check RemotingRole; no call to db.
			return (sentOrReceived==EmailSentOrReceived.ReceivedEncrypted || sentOrReceived==EmailSentOrReceived.ReceivedDirect ||
				sentOrReceived==EmailSentOrReceived.ReadDirect || sentOrReceived==EmailSentOrReceived.Received ||
				sentOrReceived==EmailSentOrReceived.Read || sentOrReceived==EmailSentOrReceived.WebMailReceived ||
				sentOrReceived==EmailSentOrReceived.WebMailRecdRead);
		}

    public static EmailMessage CreateReply(EmailMessage emailReceived,EmailAddress emailAddress,bool isReplyAll=false) {
      //No need to check RemotingRole; no call to db.
      EmailMessage replyMessage=new EmailMessage();
      replyMessage.PatNum=emailReceived.PatNum;
      replyMessage.ToAddress=emailReceived.FromAddress;
			if(isReplyAll) {
				emailReceived.ToAddress.Split(',').ToList()//email@od.com,email2@od.com,...
					.FindAll(x => x!=emailAddress.EmailUsername && x!=emailAddress.SenderAddress)//Since we are replying, remove our current email from list
					.ForEach(x => replyMessage.ToAddress+=","+x);
			}
      replyMessage.FromAddress=emailReceived.RecipientAddress;
      if(emailReceived.Subject.Trim().Length >=3 && emailReceived.Subject.Trim().Substring(0,3).ToString().ToLower()=="re:") { //already contains a "re:"
        replyMessage.Subject=emailReceived.Subject;
      }
      else { //doesn't contain a "re:"
        replyMessage.Subject="RE: " + emailReceived.Subject;
      }
      string bodyTextString;
      bodyTextString="\r\n\r\n\r\n" + "On " + emailReceived.MsgDateTime.ToString() + " " + emailReceived.FromAddress + " sent:\r\n>";
      string receivedEmailBody;
      List<List<Health.Direct.Common.Mime.MimeEntity>> listMimeParts =
          EmailMessages.GetMimePartsForMimeTypes(emailReceived.RawEmailIn,emailAddress,"text/html","text/plain","image/");
      List<Health.Direct.Common.Mime.MimeEntity> listHtmlParts=listMimeParts[0];//If RawEmailIn is blank, then this list will also be blank (ex Secure Web Mail messages).
      List<Health.Direct.Common.Mime.MimeEntity> listTextParts=listMimeParts[1];//If RawEmailIn is blank, then this list will also be blank (ex Secure Web Mail messages).
      if(listHtmlParts.Count>0) {//Html body found.
        receivedEmailBody=Regex.Replace(EmailMessages.ProcessMimeTextPart(listHtmlParts[0]),@"<(.|\n)*?>",""); //remove most html tags.
      }
      else if(listTextParts.Count>0) {//No html body found, however one specific mime part is for viewing in text only.					
        receivedEmailBody=EmailMessages.ProcessMimeTextPart(listTextParts[0]);
      }
      else {//No html body found and no text body found.  Last resort.  Show all mime parts which are not attachments (ugly).
        receivedEmailBody=emailReceived.BodyText;//This version of the body text includes all non-attachment mime parts.
      }
      receivedEmailBody=receivedEmailBody.Trim().Replace("\r\n",".!(-&>"); //replace with a string that would very very seldom happen.
      receivedEmailBody=receivedEmailBody.Trim().Replace("\n",".!(-&>");
      receivedEmailBody=receivedEmailBody.Trim().Replace("\r",".!(-&>");
      bodyTextString+=receivedEmailBody.Trim().Replace(".!(-&>","\r\n>");
      replyMessage.BodyText=bodyTextString;
      return replyMessage;
    }

    public static EmailMessage CreateForward(EmailMessage emailReceived,EmailAddress emailAddress) {
      //No need to check RemotingRole; no call to db.
      EmailMessage forwardMessage=new EmailMessage();
      forwardMessage.PatNum=emailReceived.PatNum;
      forwardMessage.FromAddress=emailAddress.EmailUsername;//We cannot use emailAddress.SenderAddress here in case the user wants to send a Direct message.
      if(emailReceived.Subject.Trim().ToLower().StartsWith("fwd:")) { //already contains a "fwd:"
        forwardMessage.Subject=emailReceived.Subject;
      }
      else { //doesn't contain a "fwd:"
        forwardMessage.Subject="FWD: "+emailReceived.Subject;
      }
      string bodyTextString;
      bodyTextString="\r\n\r\n\r\nOn "+emailReceived.MsgDateTime.ToString()+" "+emailReceived.FromAddress+" sent:\r\n>";
      string receivedEmailBody;
      List<List<Health.Direct.Common.Mime.MimeEntity>> listMimeParts=
          EmailMessages.GetMimePartsForMimeTypes(emailReceived.RawEmailIn,emailAddress,"text/html","text/plain","image/");
      List<Health.Direct.Common.Mime.MimeEntity> listHtmlParts=listMimeParts[0];//If RawEmailIn is blank, then this list will also be blank (ex Secure Web Mail messages).
      List<Health.Direct.Common.Mime.MimeEntity> listTextParts=listMimeParts[1];//If RawEmailIn is blank, then this list will also be blank (ex Secure Web Mail messages).
      if(listHtmlParts.Count>0) {//Html body found.
        receivedEmailBody=Regex.Replace(EmailMessages.ProcessMimeTextPart(listHtmlParts[0]),@"<(.|\n)*?>",""); //remove most html tags.
      }
      else if(listTextParts.Count>0) {//No html body found, however one specific mime part is for viewing in text only.					
        receivedEmailBody=EmailMessages.ProcessMimeTextPart(listTextParts[0]);
      }
      else {//No html body found and no text body found.  Last resort.  Show all mime parts which are not attachments (ugly).
        receivedEmailBody=emailReceived.BodyText;//This version of the body text includes all non-attachment mime parts.
      }
      receivedEmailBody=receivedEmailBody.Trim().Replace("\r\n",".!(-&>"); //replace with a string that would very very seldom happen.
      receivedEmailBody=receivedEmailBody.Trim().Replace("\n",".!(-&>");
      receivedEmailBody=receivedEmailBody.Trim().Replace("\r",".!(-&>");
      bodyTextString+=receivedEmailBody.Trim().Replace(".!(-&>","\r\n>");
      forwardMessage.BodyText=bodyTextString;
      return forwardMessage;
    }

		#endregion Helpers

		#region Testing

		///<summary>This method is only for ehr testing purposes, and it always uses the hidden pref EHREmailToAddress to send to.  For privacy reasons, this cannot be used with production patient info.  AttachName should include extension.</summary>
		public static void SendTestUnsecure(string subjectAndBody,string attachName,string attachContents) {
			//No need to check RemotingRole; no call to db.
			SendTestUnsecure(subjectAndBody,attachName,attachContents,"","");
		}

		///<summary>This method is only for ehr testing purposes, and it always uses the hidden pref EHREmailToAddress to send to.  For privacy reasons, this cannot be used with production patient info.  AttachName should include extension.</summary>
		public static void SendTestUnsecure(string subjectAndBody,string attachName1,string attachContents1,string attachName2,string attachContents2) {
			//No need to check RemotingRole; no call to db.
			string strTo=PrefC.GetString(PrefName.EHREmailToAddress);
			if(strTo=="") {
				throw new ApplicationException("This feature cannot be used except in a test environment because email is not secure.");
			}
			EmailAddress emailAddressFrom=EmailAddresses.GetByClinic(0);
			EmailMessage emailMessage=new EmailMessage();
			emailMessage.FromAddress=emailAddressFrom.EmailUsername.Trim();
			emailMessage.ToAddress=strTo.Trim();
			emailMessage.Subject=subjectAndBody;
			emailMessage.BodyText=subjectAndBody;
			if(attachName1!="") {
				EmailAttach emailAttach=EmailAttaches.CreateAttach(attachName1,Encoding.UTF8.GetBytes(attachContents1));
				emailMessage.Attachments.Add(emailAttach);
			}
			if(attachName2!="") {
				EmailAttach emailAttach=EmailAttaches.CreateAttach(attachName2,Encoding.UTF8.GetBytes(attachContents2));
				emailMessage.Attachments.Add(emailAttach);
			}
			SendEmailUnsecure(emailMessage,emailAddressFrom);
			Insert(emailMessage);
		}

		///<summary>Receives one email from the inbox, and returns the contents of the attachment as a string.  Will throw an exception if anything goes wrong, so surround with a try-catch.</summary>
		public static string ReceiveOneForEhrTest() {
			//No need to check RemotingRole; no call to db.
			if(PrefC.GetString(PrefName.EHREmailToAddress)=="") {//this pref is hidden, so no practical way for user to turn this on.
				throw new ApplicationException("This feature cannot be used except in a test environment because email is not secure.");
			}
			if(PrefC.GetString(PrefName.EHREmailPOPserver)=="") {
				throw new ApplicationException("No POP server set up.");
			}
			EmailAddress emailAddress=new EmailAddress();
			emailAddress.Pop3ServerIncoming=PrefC.GetString(PrefName.EHREmailPOPserver);
			emailAddress.ServerPortIncoming=PrefC.GetInt(PrefName.EHREmailPort);
			emailAddress.EmailUsername=PrefC.GetString(PrefName.EHREmailFromAddress);
			emailAddress.EmailPassword=PrefC.GetString(PrefName.EHREmailPassword);
			List<EmailMessage> emailMessages=ReceiveFromInbox(1,emailAddress);
			if(emailMessages.Count==0) {
				throw new Exception("Inbox empty.");
			}
			EmailMessage emailMessage=emailMessages[0];
			if(emailMessage.Attachments==null || emailMessage.Attachments.Count==0) {
				throw new Exception("No attachments");
			}
			return FileAtoZ.ReadAllText(FileAtoZ.CombinePaths(EmailAttaches.GetAttachPath(),emailMessage.Attachments[0].ActualFileName));
		}

		private static string GetTestEmail1() {
			//No need to check RemotingRole; no call to db.
			return @"This is a multipart message in MIME format.

------=_NextPart_000_0074_01CC35A4.193BF450
Content-Type: multipart/alternative;
	boundary=""----=_NextPart_001_0075_01CC35A4.193BF450""


------=_NextPart_001_0075_01CC35A4.193BF450
Content-Type: text/plain;
	charset=""us-ascii""
Content-Transfer-Encoding: 7bit

test


------=_NextPart_001_0075_01CC35A4.193BF450
Content-Type: text/html;
	charset=""us-ascii""
Content-Transfer-Encoding: quoted-printable

<html xmlns:v=3D""urn:schemas-microsoft-com:vml"" =
xmlns:o=3D""urn:schemas-microsoft-com:office:office"" =
xmlns:w=3D""urn:schemas-microsoft-com:office:word"" =
xmlns:m=3D""http://schemas.microsoft.com/office/2004/12/omml"" =
xmlns=3D""http://www.w3.org/TR/REC-html40""><head><meta =
http-equiv=3DContent-Type content=3D""text/html; =
charset=3Dus-ascii""><meta name=3DGenerator content=3D""Microsoft Word 14 =
(filtered medium)""><style><!--
/* Font Definitions */
@font-face
	{font-family:Calibri;
	panose-1:2 15 5 2 2 2 4 3 2 4;}
/* Style Definitions */
p.MsoNormal, li.MsoNormal, div.MsoNormal
	{margin:0in;
	margin-bottom:.0001pt;
	font-size:11.0pt;
	font-family:""Calibri"",""sans-serif"";}
a:link, span.MsoHyperlink
	{mso-style-priority:99;
	color:blue;
	text-decoration:underline;}
a:visited, span.MsoHyperlinkFollowed
	{mso-style-priority:99;
	color:purple;
	text-decoration:underline;}
span.EmailStyle17
	{mso-style-type:personal-compose;
	font-family:""Calibri"",""sans-serif"";
	color:windowtext;}
..MsoChpDefault
	{mso-style-type:export-only;
	font-family:""Calibri"",""sans-serif"";}
@page WordSection1
	{size:8.5in 11.0in;
	margin:1.0in 1.0in 1.0in 1.0in;}
div.WordSection1
	{page:WordSection1;}
--></style><!--[if gte mso 9]><xml>
<o:shapedefaults v:ext=3D""edit"" spidmax=3D""1026"" />
</xml><![endif]--><!--[if gte mso 9]><xml>
<o:shapelayout v:ext=3D""edit"">
<o:idmap v:ext=3D""edit"" data=3D""1"" />
</o:shapelayout></xml><![endif]--></head><body lang=3DEN-US link=3Dblue =
vlink=3Dpurple><div class=3DWordSection1><p =
class=3DMsoNormal>test<o:p></o:p></p></div></body></html>
------=_NextPart_001_0075_01CC35A4.193BF450--

------=_NextPart_000_0074_01CC35A4.193BF450
Content-Type: text/plain;
	name=""SarahEbbert_v4.txt""
Content-Transfer-Encoding: quoted-printable
Content-Disposition: attachment;
	filename=""SarahEbbert_v4.txt""

<?xml version=3D""1.0"" encoding=3D""UTF-8""?>
<ClinicalDocument xmlns=3D""urn:hl7-org:v3"">
   <typeId extension=3D""POCD_HD0000040"" root=3D""2.16.840.1.113883.1.3"" =
/>
   <templateId root=3D""2.16.840.1.113883.10.20.1"" />
   <id />
   <code code=3D""34133-9"" codeSystemName=3D""LOINC"" =
codeSystem=3D""2.16.840.1.113883.6.1"" displayName=3D""Summary of episode =
note"" />
   <documentationOf>
      <serviceEvent classCode=3D""PCPR"">
         <effectiveTime>
            <high value=3D""20110628075321-0700"" />
            <low value=3D""19621008000000-0700"" />
         </effectiveTime>
      </serviceEvent>
   </documentationOf>
   <languageCode value=3D""en-US"" />
   <templateId root=3D""2.16.840.1.113883.10.20.1"" />
   <effectiveTime value=3D""20110628075321-0700"" />
   <recordTarget>
      <patientRole>
         <id value=3D""7"" />
         <addr use=3D""HP"">
            <streetAddressLine>856 Salt Street</streetAddressLine>
            <streetAddressLine></streetAddressLine>
            <city>Shawville</city>
            <state>PA</state>
            <country></country>
         </addr>
         <patient>
            <name use=3D""L"">
               <given>Sarah</given>
               <given></given>
               <family>Ebbert</family>
               <suffix qualifier=3D""TITLE""></suffix>
            </name>
         </patient>
      </patientRole>
      <text>
         <table width=3D""100%"" border=3D""1"">
            <thead>
               <tr>
                  <th>Name</th>
                  <th>Date of Birth</th>
                  <th>Gender</th>
                  <th>Identification Number</th>
                  <th>Identification Number Type</th>
                  <th>Address/Phone</th>
               </tr>
            </thead>
            <tbody>
               <tr>
                  <td>Ebbert, Sarah </td>
                  <td>10/08/1962</td>
                  <td>Female</td>
                  <td>7</td>
                  <td>Open Dental PatNum</td>
                  <td>856 Salt Street=20
Shawville, PA
16873
(814)645-6489</td>
               </tr>
            </tbody>
         </table>
      </text>
   </recordTarget>
   <author>
      <assignedAuthor>
         <assignedPerson>
            <name>Auto Generated</name>
         </assignedPerson>
      </assignedAuthor>
   </author>
   <component>
      <!--Problems-->
      <section>
         <templateId root=3D""2.16.840.1.113883.10.20.1.11"" =
assigningAuthorityName=3D""HL7 CCD"" />
         <!--Problems section template-->
         <code code=3D""11450-4"" codeSystemName=3D""LOINC"" =
codeSystem=3D""2.16.840.1.113883.6.1"" displayName=3D""Problem list"" />
         <title>Problems</title>
         <text>
            <table width=3D""100%"" border=3D""1"">
               <thead>
                  <tr>
                     <th>ICD-9 Code</th>
                     <th>Patient Problem</th>
                     <th>Date Diagnosed</th>
                     <th>Status</th>
                  </tr>
               </thead>
               <tbody>
                  <tr ID=3D""CondID-1"">
                     <td>272.4</td>
                     <td>OTHER AND UNSPECIFIED HYPERLIPIDEMIA</td>
                     <td>07/05/2006</td>
                     <td>Active</td>
                  </tr>
                  <tr ID=3D""CondID-1"">
                     <td>401.9</td>
                     <td>UNSPECIFIED ESSENTIAL HYPERTENSION</td>
                     <td>07/05/2006</td>
                     <td>Active</td>
                  </tr>
               </tbody>
            </table>
         </text>
      </section>
      <component>
         <!--Alerts-->
         <section>
            <templateId root=3D""2.16.840.1.113883.10.20.1.2"" =
assigningAuthorityName=3D""HL7 CCD"" />
            <!--Alerts section template-->
            <code code=3D""48765-2"" codeSystemName=3D""LOINC"" =
codeSystem=3D""2.16.840.1.113883.6.1"" displayName=3D""Allergies, adverse =
reactions, alerts"" />
            <title>Allergies and Adverse Reactions</title>
            <text>
               <table width=3D""100%"" border=3D""1"">
                  <thead>
                     <tr>
                        <th>SNOMED Allergy Type Code</th>
                        <th>Medication/Agent Allergy</th>
                        <th>Reaction</th>
                        <th>Adverse Event Date</th>
                     </tr>
                  </thead>
                  <tbody>
                     <tr>
                        <td>416098002 - Drug allergy (disorder)</td>
                        <td>617314 - Lipitor</td>
                        <td>Rash and anaphylaxis</td>
                        <td>05/22/1998</td>
                     </tr>
                  </tbody>
               </table>
            </text>
         </section>
         <component>
            <!--Medications-->
            <section>
               <templateId root=3D""2.16.840.1.113883.10.20.1.8"" =
assigningAuthorityName=3D""HL7 CCD"" />
               <!--Medications section template-->
               <code code=3D""10160-0"" codeSystemName=3D""LOINC"" =
codeSystem=3D""2.16.840.1.113883.6.1"" displayName=3D""History of =
medication use"" />
               <title>Medications</title>
               <text>
                  <table width=3D""100%"" border=3D""1"">
                     <thead>
                        <tr>
                           <th>RxNorm Code</th>
                           <th>Product</th>
                           <th>Generic Name</th>
                           <th>Brand Name</th>
                           <th>Instructions</th>
                           <th>Date Started</th>
                           <th>Status</th>
                        </tr>
                     </thead>
                     <tbody>
                        <tr>
                           <td>617314</td>
                           <td>Medication</td>
                           <td>atorvastatin calcium</td>
                           <td>Lipitor</td>
                           <td>10 mg, 1 Tablet, Q Day</td>
                           <td>07/05/2006</td>
                           <td>Active</td>
                        </tr>
                        <tr>
                           <td>200801</td>
                           <td>Medication</td>
                           <td>furosemide</td>
                           <td>Lasix</td>
                           <td>20 mg, 1 Tablet, BID</td>
                           <td>07/05/2006</td>
                           <td>Active</td>
                        </tr>
                        <tr>
                           <td>628958</td>
                           <td>Medication</td>
                           <td>potassium chloride</td>
                           <td>Klor-Con</td>
                           <td>10 mEq, 1 Tablet, BID</td>
                           <td>07/05/2006</td>
                           <td>Active</td>
                        </tr>
                     </tbody>
                  </table>
               </text>
            </section>
            <component>
               <!--Results-->
               <section>
                  <templateId root=3D""2.16.840.1.113883.10.20.1.14"" =
assigningAuthorityName=3D""HL7 CCD"" />
                  <!--Relevant diagnostic tests and/or labratory data-->
                  <code code=3D""30954-2"" codeSystemName=3D""LOINC"" =
codeSystem=3D""2.16.840.1.113883.6.1"" displayName=3D""Allergies, adverse =
reactions, alerts"" />
                  <title>Results</title>
                  <text>
                     <table width=3D""100%"" border=3D""1"">
                        <thead>
                           <tr>
                              <th>LOINC Code</th>
                              <th>Test</th>
                              <th>Result</th>
                              <th>Abnormal Flag</th>
                              <th>Date Performed</th>
                           </tr>
                        </thead>
                        <tbody>
                           <tr>
                              <td>2823-3</td>
                              <td>Potassium</td>
                              <td>Normal</td>
                              <td>02/15/2009</td>
                           </tr>
                           <tr>
                              <td>14647-2</td>
                              <td>Total cholesterol</td>
                              <td>Normal</td>
                              <td>07/15/2009</td>
                           </tr>
                           <tr>
                              <td>14646-4</td>
                              <td>HDL cholesterol</td>
                              <td>Normal</td>
                              <td>07/15/2009</td>
                           </tr>
                           <tr>
                              <td>2089-1</td>
                              <td>LDL cholesterol</td>
                              <td>Above</td>
                              <td>07/15/2009</td>
                           </tr>
                           <tr>
                              <td>14927-8</td>
                              <td>Triglycerides</td>
                              <td>Above</td>
                              <td>07/15/2009</td>
                           </tr>
                        </tbody>
                     </table>
                  </text>
               </section>
            </component>
         </component>
      </component>
   </component>
</ClinicalDocument>
------=_NextPart_000_0074_01CC35A4.193BF450--";
		}

		private static string GetTestEmail2() {
			//No need to check RemotingRole; no call to db.
			return @"This is a multi-part message in MIME format.
--------------070304090505090508040909
Content-Type: text/plain; charset=ISO-8859-1; format=flowed
Content-Transfer-Encoding: 7bit

Clinical Exchange Test

--------------070304090505090508040909
Content-Type: text/plain;
 name=""SarahEbbert_v4.txt""
Content-Transfer-Encoding: base64
Content-Disposition: attachment;
 filename=""SarahEbbert_v4.txt""

PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz4NCjxDbGluaWNhbERvY3Vt
ZW50IHhtbG5zPSJ1cm46aGw3LW9yZzp2MyI+DQogICA8dHlwZUlkIGV4dGVuc2lvbj0iUE9D
RF9IRDAwMDAwNDAiIHJvb3Q9IjIuMTYuODQwLjEuMTEzODgzLjEuMyIgLz4NCiAgIDx0ZW1w
bGF0ZUlkIHJvb3Q9IjIuMTYuODQwLjEuMTEzODgzLjEwLjIwLjEiIC8+DQogICA8aWQgLz4N
CiAgIDxjb2RlIGNvZGU9IjM0MTMzLTkiIGNvZGVTeXN0ZW1OYW1lPSJMT0lOQyIgY29kZVN5
c3RlbT0iMi4xNi44NDAuMS4xMTM4ODMuNi4xIiBkaXNwbGF5TmFtZT0iU3VtbWFyeSBvZiBl
cGlzb2RlIG5vdGUiIC8+DQogICA8ZG9jdW1lbnRhdGlvbk9mPg0KICAgICAgPHNlcnZpY2VF
dmVudCBjbGFzc0NvZGU9IlBDUFIiPg0KICAgICAgICAgPGVmZmVjdGl2ZVRpbWU+DQogICAg
ICAgICAgICA8aGlnaCB2YWx1ZT0iMjAxMTA2MjgwNzUzMjEtMDcwMCIgLz4NCiAgICAgICAg
ICAgIDxsb3cgdmFsdWU9IjE5NjIxMDA4MDAwMDAwLTA3MDAiIC8+DQogICAgICAgICA8L2Vm
ZmVjdGl2ZVRpbWU+DQogICAgICA8L3NlcnZpY2VFdmVudD4NCiAgIDwvZG9jdW1lbnRhdGlv
bk9mPg0KICAgPGxhbmd1YWdlQ29kZSB2YWx1ZT0iZW4tVVMiIC8+DQogICA8dGVtcGxhdGVJ
ZCByb290PSIyLjE2Ljg0MC4xLjExMzg4My4xMC4yMC4xIiAvPg0KICAgPGVmZmVjdGl2ZVRp
bWUgdmFsdWU9IjIwMTEwNjI4MDc1MzIxLTA3MDAiIC8+DQogICA8cmVjb3JkVGFyZ2V0Pg0K
ICAgICAgPHBhdGllbnRSb2xlPg0KICAgICAgICAgPGlkIHZhbHVlPSI3IiAvPg0KICAgICAg
ICAgPGFkZHIgdXNlPSJIUCI+DQogICAgICAgICAgICA8c3RyZWV0QWRkcmVzc0xpbmU+ODU2
IFNhbHQgU3RyZWV0PC9zdHJlZXRBZGRyZXNzTGluZT4NCiAgICAgICAgICAgIDxzdHJlZXRB
ZGRyZXNzTGluZT48L3N0cmVldEFkZHJlc3NMaW5lPg0KICAgICAgICAgICAgPGNpdHk+U2hh
d3ZpbGxlPC9jaXR5Pg0KICAgICAgICAgICAgPHN0YXRlPlBBPC9zdGF0ZT4NCiAgICAgICAg
ICAgIDxjb3VudHJ5PjwvY291bnRyeT4NCiAgICAgICAgIDwvYWRkcj4NCiAgICAgICAgIDxw
YXRpZW50Pg0KICAgICAgICAgICAgPG5hbWUgdXNlPSJMIj4NCiAgICAgICAgICAgICAgIDxn
aXZlbj5TYXJhaDwvZ2l2ZW4+DQogICAgICAgICAgICAgICA8Z2l2ZW4+PC9naXZlbj4NCiAg
ICAgICAgICAgICAgIDxmYW1pbHk+RWJiZXJ0PC9mYW1pbHk+DQogICAgICAgICAgICAgICA8
c3VmZml4IHF1YWxpZmllcj0iVElUTEUiPjwvc3VmZml4Pg0KICAgICAgICAgICAgPC9uYW1l
Pg0KICAgICAgICAgPC9wYXRpZW50Pg0KICAgICAgPC9wYXRpZW50Um9sZT4NCiAgICAgIDx0
ZXh0Pg0KICAgICAgICAgPHRhYmxlIHdpZHRoPSIxMDAlIiBib3JkZXI9IjEiPg0KICAgICAg
ICAgICAgPHRoZWFkPg0KICAgICAgICAgICAgICAgPHRyPg0KICAgICAgICAgICAgICAgICAg
PHRoPk5hbWU8L3RoPg0KICAgICAgICAgICAgICAgICAgPHRoPkRhdGUgb2YgQmlydGg8L3Ro
Pg0KICAgICAgICAgICAgICAgICAgPHRoPkdlbmRlcjwvdGg+DQogICAgICAgICAgICAgICAg
ICA8dGg+SWRlbnRpZmljYXRpb24gTnVtYmVyPC90aD4NCiAgICAgICAgICAgICAgICAgIDx0
aD5JZGVudGlmaWNhdGlvbiBOdW1iZXIgVHlwZTwvdGg+DQogICAgICAgICAgICAgICAgICA8
dGg+QWRkcmVzcy9QaG9uZTwvdGg+DQogICAgICAgICAgICAgICA8L3RyPg0KICAgICAgICAg
ICAgPC90aGVhZD4NCiAgICAgICAgICAgIDx0Ym9keT4NCiAgICAgICAgICAgICAgIDx0cj4N
CiAgICAgICAgICAgICAgICAgIDx0ZD5FYmJlcnQsIFNhcmFoIDwvdGQ+DQogICAgICAgICAg
ICAgICAgICA8dGQ+MTAvMDgvMTk2MjwvdGQ+DQogICAgICAgICAgICAgICAgICA8dGQ+RmVt
YWxlPC90ZD4NCiAgICAgICAgICAgICAgICAgIDx0ZD43PC90ZD4NCiAgICAgICAgICAgICAg
ICAgIDx0ZD5PcGVuIERlbnRhbCBQYXROdW08L3RkPg0KICAgICAgICAgICAgICAgICAgPHRk
Pjg1NiBTYWx0IFN0cmVldCANClNoYXd2aWxsZSwgUEENCjE2ODczDQooODE0KTY0NS02NDg5
PC90ZD4NCiAgICAgICAgICAgICAgIDwvdHI+DQogICAgICAgICAgICA8L3Rib2R5Pg0KICAg
ICAgICAgPC90YWJsZT4NCiAgICAgIDwvdGV4dD4NCiAgIDwvcmVjb3JkVGFyZ2V0Pg0KICAg
PGF1dGhvcj4NCiAgICAgIDxhc3NpZ25lZEF1dGhvcj4NCiAgICAgICAgIDxhc3NpZ25lZFBl
cnNvbj4NCiAgICAgICAgICAgIDxuYW1lPkF1dG8gR2VuZXJhdGVkPC9uYW1lPg0KICAgICAg
ICAgPC9hc3NpZ25lZFBlcnNvbj4NCiAgICAgIDwvYXNzaWduZWRBdXRob3I+DQogICA8L2F1
dGhvcj4NCiAgIDxjb21wb25lbnQ+DQogICAgICA8IS0tUHJvYmxlbXMtLT4NCiAgICAgIDxz
ZWN0aW9uPg0KICAgICAgICAgPHRlbXBsYXRlSWQgcm9vdD0iMi4xNi44NDAuMS4xMTM4ODMu
MTAuMjAuMS4xMSIgYXNzaWduaW5nQXV0aG9yaXR5TmFtZT0iSEw3IENDRCIgLz4NCiAgICAg
ICAgIDwhLS1Qcm9ibGVtcyBzZWN0aW9uIHRlbXBsYXRlLS0+DQogICAgICAgICA8Y29kZSBj
b2RlPSIxMTQ1MC00IiBjb2RlU3lzdGVtTmFtZT0iTE9JTkMiIGNvZGVTeXN0ZW09IjIuMTYu
ODQwLjEuMTEzODgzLjYuMSIgZGlzcGxheU5hbWU9IlByb2JsZW0gbGlzdCIgLz4NCiAgICAg
ICAgIDx0aXRsZT5Qcm9ibGVtczwvdGl0bGU+DQogICAgICAgICA8dGV4dD4NCiAgICAgICAg
ICAgIDx0YWJsZSB3aWR0aD0iMTAwJSIgYm9yZGVyPSIxIj4NCiAgICAgICAgICAgICAgIDx0
aGVhZD4NCiAgICAgICAgICAgICAgICAgIDx0cj4NCiAgICAgICAgICAgICAgICAgICAgIDx0
aD5JQ0QtOSBDb2RlPC90aD4NCiAgICAgICAgICAgICAgICAgICAgIDx0aD5QYXRpZW50IFBy
b2JsZW08L3RoPg0KICAgICAgICAgICAgICAgICAgICAgPHRoPkRhdGUgRGlhZ25vc2VkPC90
aD4NCiAgICAgICAgICAgICAgICAgICAgIDx0aD5TdGF0dXM8L3RoPg0KICAgICAgICAgICAg
ICAgICAgPC90cj4NCiAgICAgICAgICAgICAgIDwvdGhlYWQ+DQogICAgICAgICAgICAgICA8
dGJvZHk+DQogICAgICAgICAgICAgICAgICA8dHIgSUQ9IkNvbmRJRC0xIj4NCiAgICAgICAg
ICAgICAgICAgICAgIDx0ZD4yNzIuNDwvdGQ+DQogICAgICAgICAgICAgICAgICAgICA8dGQ+
T1RIRVIgQU5EIFVOU1BFQ0lGSUVEIEhZUEVSTElQSURFTUlBPC90ZD4NCiAgICAgICAgICAg
ICAgICAgICAgIDx0ZD4wNy8wNS8yMDA2PC90ZD4NCiAgICAgICAgICAgICAgICAgICAgIDx0
ZD5BY3RpdmU8L3RkPg0KICAgICAgICAgICAgICAgICAgPC90cj4NCiAgICAgICAgICAgICAg
ICAgIDx0ciBJRD0iQ29uZElELTEiPg0KICAgICAgICAgICAgICAgICAgICAgPHRkPjQwMS45
PC90ZD4NCiAgICAgICAgICAgICAgICAgICAgIDx0ZD5VTlNQRUNJRklFRCBFU1NFTlRJQUwg
SFlQRVJURU5TSU9OPC90ZD4NCiAgICAgICAgICAgICAgICAgICAgIDx0ZD4wNy8wNS8yMDA2
PC90ZD4NCiAgICAgICAgICAgICAgICAgICAgIDx0ZD5BY3RpdmU8L3RkPg0KICAgICAgICAg
ICAgICAgICAgPC90cj4NCiAgICAgICAgICAgICAgIDwvdGJvZHk+DQogICAgICAgICAgICA8
L3RhYmxlPg0KICAgICAgICAgPC90ZXh0Pg0KICAgICAgPC9zZWN0aW9uPg0KICAgICAgPGNv
bXBvbmVudD4NCiAgICAgICAgIDwhLS1BbGVydHMtLT4NCiAgICAgICAgIDxzZWN0aW9uPg0K
ICAgICAgICAgICAgPHRlbXBsYXRlSWQgcm9vdD0iMi4xNi44NDAuMS4xMTM4ODMuMTAuMjAu
MS4yIiBhc3NpZ25pbmdBdXRob3JpdHlOYW1lPSJITDcgQ0NEIiAvPg0KICAgICAgICAgICAg
PCEtLUFsZXJ0cyBzZWN0aW9uIHRlbXBsYXRlLS0+DQogICAgICAgICAgICA8Y29kZSBjb2Rl
PSI0ODc2NS0yIiBjb2RlU3lzdGVtTmFtZT0iTE9JTkMiIGNvZGVTeXN0ZW09IjIuMTYuODQw
LjEuMTEzODgzLjYuMSIgZGlzcGxheU5hbWU9IkFsbGVyZ2llcywgYWR2ZXJzZSByZWFjdGlv
bnMsIGFsZXJ0cyIgLz4NCiAgICAgICAgICAgIDx0aXRsZT5BbGxlcmdpZXMgYW5kIEFkdmVy
c2UgUmVhY3Rpb25zPC90aXRsZT4NCiAgICAgICAgICAgIDx0ZXh0Pg0KICAgICAgICAgICAg
ICAgPHRhYmxlIHdpZHRoPSIxMDAlIiBib3JkZXI9IjEiPg0KICAgICAgICAgICAgICAgICAg
PHRoZWFkPg0KICAgICAgICAgICAgICAgICAgICAgPHRyPg0KICAgICAgICAgICAgICAgICAg
ICAgICAgPHRoPlNOT01FRCBBbGxlcmd5IFR5cGUgQ29kZTwvdGg+DQogICAgICAgICAgICAg
ICAgICAgICAgICA8dGg+TWVkaWNhdGlvbi9BZ2VudCBBbGxlcmd5PC90aD4NCiAgICAgICAg
ICAgICAgICAgICAgICAgIDx0aD5SZWFjdGlvbjwvdGg+DQogICAgICAgICAgICAgICAgICAg
ICAgICA8dGg+QWR2ZXJzZSBFdmVudCBEYXRlPC90aD4NCiAgICAgICAgICAgICAgICAgICAg
IDwvdHI+DQogICAgICAgICAgICAgICAgICA8L3RoZWFkPg0KICAgICAgICAgICAgICAgICAg
PHRib2R5Pg0KICAgICAgICAgICAgICAgICAgICAgPHRyPg0KICAgICAgICAgICAgICAgICAg
ICAgICAgPHRkPjQxNjA5ODAwMiAtIERydWcgYWxsZXJneSAoZGlzb3JkZXIpPC90ZD4NCiAg
ICAgICAgICAgICAgICAgICAgICAgIDx0ZD42MTczMTQgLSBMaXBpdG9yPC90ZD4NCiAgICAg
ICAgICAgICAgICAgICAgICAgIDx0ZD5SYXNoIGFuZCBhbmFwaHlsYXhpczwvdGQ+DQogICAg
ICAgICAgICAgICAgICAgICAgICA8dGQ+MDUvMjIvMTk5ODwvdGQ+DQogICAgICAgICAgICAg
ICAgICAgICA8L3RyPg0KICAgICAgICAgICAgICAgICAgPC90Ym9keT4NCiAgICAgICAgICAg
ICAgIDwvdGFibGU+DQogICAgICAgICAgICA8L3RleHQ+DQogICAgICAgICA8L3NlY3Rpb24+
DQogICAgICAgICA8Y29tcG9uZW50Pg0KICAgICAgICAgICAgPCEtLU1lZGljYXRpb25zLS0+
DQogICAgICAgICAgICA8c2VjdGlvbj4NCiAgICAgICAgICAgICAgIDx0ZW1wbGF0ZUlkIHJv
b3Q9IjIuMTYuODQwLjEuMTEzODgzLjEwLjIwLjEuOCIgYXNzaWduaW5nQXV0aG9yaXR5TmFt
ZT0iSEw3IENDRCIgLz4NCiAgICAgICAgICAgICAgIDwhLS1NZWRpY2F0aW9ucyBzZWN0aW9u
IHRlbXBsYXRlLS0+DQogICAgICAgICAgICAgICA8Y29kZSBjb2RlPSIxMDE2MC0wIiBjb2Rl
U3lzdGVtTmFtZT0iTE9JTkMiIGNvZGVTeXN0ZW09IjIuMTYuODQwLjEuMTEzODgzLjYuMSIg
ZGlzcGxheU5hbWU9Ikhpc3Rvcnkgb2YgbWVkaWNhdGlvbiB1c2UiIC8+DQogICAgICAgICAg
ICAgICA8dGl0bGU+TWVkaWNhdGlvbnM8L3RpdGxlPg0KICAgICAgICAgICAgICAgPHRleHQ+
DQogICAgICAgICAgICAgICAgICA8dGFibGUgd2lkdGg9IjEwMCUiIGJvcmRlcj0iMSI+DQog
ICAgICAgICAgICAgICAgICAgICA8dGhlYWQ+DQogICAgICAgICAgICAgICAgICAgICAgICA8
dHI+DQogICAgICAgICAgICAgICAgICAgICAgICAgICA8dGg+UnhOb3JtIENvZGU8L3RoPg0K
ICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRoPlByb2R1Y3Q8L3RoPg0KICAgICAgICAg
ICAgICAgICAgICAgICAgICAgPHRoPkdlbmVyaWMgTmFtZTwvdGg+DQogICAgICAgICAgICAg
ICAgICAgICAgICAgICA8dGg+QnJhbmQgTmFtZTwvdGg+DQogICAgICAgICAgICAgICAgICAg
ICAgICAgICA8dGg+SW5zdHJ1Y3Rpb25zPC90aD4NCiAgICAgICAgICAgICAgICAgICAgICAg
ICAgIDx0aD5EYXRlIFN0YXJ0ZWQ8L3RoPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAg
PHRoPlN0YXR1czwvdGg+DQogICAgICAgICAgICAgICAgICAgICAgICA8L3RyPg0KICAgICAg
ICAgICAgICAgICAgICAgPC90aGVhZD4NCiAgICAgICAgICAgICAgICAgICAgIDx0Ym9keT4N
CiAgICAgICAgICAgICAgICAgICAgICAgIDx0cj4NCiAgICAgICAgICAgICAgICAgICAgICAg
ICAgIDx0ZD42MTczMTQ8L3RkPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRkPk1l
ZGljYXRpb248L3RkPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRkPmF0b3J2YXN0
YXRpbiBjYWxjaXVtPC90ZD4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0ZD5MaXBp
dG9yPC90ZD4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0ZD4xMCBtZywgMSBUYWJs
ZXQsIFEgRGF5PC90ZD4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0ZD4wNy8wNS8y
MDA2PC90ZD4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0ZD5BY3RpdmU8L3RkPg0K
ICAgICAgICAgICAgICAgICAgICAgICAgPC90cj4NCiAgICAgICAgICAgICAgICAgICAgICAg
IDx0cj4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0ZD4yMDA4MDE8L3RkPg0KICAg
ICAgICAgICAgICAgICAgICAgICAgICAgPHRkPk1lZGljYXRpb248L3RkPg0KICAgICAgICAg
ICAgICAgICAgICAgICAgICAgPHRkPmZ1cm9zZW1pZGU8L3RkPg0KICAgICAgICAgICAgICAg
ICAgICAgICAgICAgPHRkPkxhc2l4PC90ZD4NCiAgICAgICAgICAgICAgICAgICAgICAgICAg
IDx0ZD4yMCBtZywgMSBUYWJsZXQsIEJJRDwvdGQ+DQogICAgICAgICAgICAgICAgICAgICAg
ICAgICA8dGQ+MDcvMDUvMjAwNjwvdGQ+DQogICAgICAgICAgICAgICAgICAgICAgICAgICA8
dGQ+QWN0aXZlPC90ZD4NCiAgICAgICAgICAgICAgICAgICAgICAgIDwvdHI+DQogICAgICAg
ICAgICAgICAgICAgICAgICA8dHI+DQogICAgICAgICAgICAgICAgICAgICAgICAgICA8dGQ+
NjI4OTU4PC90ZD4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0ZD5NZWRpY2F0aW9u
PC90ZD4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0ZD5wb3Rhc3NpdW0gY2hsb3Jp
ZGU8L3RkPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRkPktsb3ItQ29uPC90ZD4N
CiAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0ZD4xMCBtRXEsIDEgVGFibGV0LCBCSUQ8
L3RkPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRkPjA3LzA1LzIwMDY8L3RkPg0K
ICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRkPkFjdGl2ZTwvdGQ+DQogICAgICAgICAg
ICAgICAgICAgICAgICA8L3RyPg0KICAgICAgICAgICAgICAgICAgICAgPC90Ym9keT4NCiAg
ICAgICAgICAgICAgICAgIDwvdGFibGU+DQogICAgICAgICAgICAgICA8L3RleHQ+DQogICAg
ICAgICAgICA8L3NlY3Rpb24+DQogICAgICAgICAgICA8Y29tcG9uZW50Pg0KICAgICAgICAg
ICAgICAgPCEtLVJlc3VsdHMtLT4NCiAgICAgICAgICAgICAgIDxzZWN0aW9uPg0KICAgICAg
ICAgICAgICAgICAgPHRlbXBsYXRlSWQgcm9vdD0iMi4xNi44NDAuMS4xMTM4ODMuMTAuMjAu
MS4xNCIgYXNzaWduaW5nQXV0aG9yaXR5TmFtZT0iSEw3IENDRCIgLz4NCiAgICAgICAgICAg
ICAgICAgIDwhLS1SZWxldmFudCBkaWFnbm9zdGljIHRlc3RzIGFuZC9vciBsYWJyYXRvcnkg
ZGF0YS0tPg0KICAgICAgICAgICAgICAgICAgPGNvZGUgY29kZT0iMzA5NTQtMiIgY29kZVN5
c3RlbU5hbWU9IkxPSU5DIiBjb2RlU3lzdGVtPSIyLjE2Ljg0MC4xLjExMzg4My42LjEiIGRp
c3BsYXlOYW1lPSJBbGxlcmdpZXMsIGFkdmVyc2UgcmVhY3Rpb25zLCBhbGVydHMiIC8+DQog
ICAgICAgICAgICAgICAgICA8dGl0bGU+UmVzdWx0czwvdGl0bGU+DQogICAgICAgICAgICAg
ICAgICA8dGV4dD4NCiAgICAgICAgICAgICAgICAgICAgIDx0YWJsZSB3aWR0aD0iMTAwJSIg
Ym9yZGVyPSIxIj4NCiAgICAgICAgICAgICAgICAgICAgICAgIDx0aGVhZD4NCiAgICAgICAg
ICAgICAgICAgICAgICAgICAgIDx0cj4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAg
IDx0aD5MT0lOQyBDb2RlPC90aD4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0
aD5UZXN0PC90aD4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0aD5SZXN1bHQ8
L3RoPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRoPkFibm9ybWFsIEZsYWc8
L3RoPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRoPkRhdGUgUGVyZm9ybWVk
PC90aD4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvdHI+DQogICAgICAgICAgICAg
ICAgICAgICAgICA8L3RoZWFkPg0KICAgICAgICAgICAgICAgICAgICAgICAgPHRib2R5Pg0K
ICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRyPg0KICAgICAgICAgICAgICAgICAgICAg
ICAgICAgICAgPHRkPjI4MjMtMzwvdGQ+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAg
ICA8dGQ+UG90YXNzaXVtPC90ZD4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0
ZD5Ob3JtYWw8L3RkPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRkPjAyLzE1
LzIwMDk8L3RkPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgPC90cj4NCiAgICAgICAg
ICAgICAgICAgICAgICAgICAgIDx0cj4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAg
IDx0ZD4xNDY0Ny0yPC90ZD4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0ZD5U
b3RhbCBjaG9sZXN0ZXJvbDwvdGQ+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8
dGQ+Tm9ybWFsPC90ZD4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0ZD4wNy8x
NS8yMDA5PC90ZD4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvdHI+DQogICAgICAg
ICAgICAgICAgICAgICAgICAgICA8dHI+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAg
ICA8dGQ+MTQ2NDYtNDwvdGQ+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8dGQ+
SERMIGNob2xlc3Rlcm9sPC90ZD4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0
ZD5Ob3JtYWw8L3RkPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRkPjA3LzE1
LzIwMDk8L3RkPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgPC90cj4NCiAgICAgICAg
ICAgICAgICAgICAgICAgICAgIDx0cj4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAg
IDx0ZD4yMDg5LTE8L3RkPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRkPkxE
TCBjaG9sZXN0ZXJvbDwvdGQ+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8dGQ+
QWJvdmU8L3RkPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHRkPjA3LzE1LzIw
MDk8L3RkPg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgPC90cj4NCiAgICAgICAgICAg
ICAgICAgICAgICAgICAgIDx0cj4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0
ZD4xNDkyNy04PC90ZD4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0ZD5Ucmln
bHljZXJpZGVzPC90ZD4NCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx0ZD5BYm92
ZTwvdGQ+DQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8dGQ+MDcvMTUvMjAwOTwv
dGQ+DQogICAgICAgICAgICAgICAgICAgICAgICAgICA8L3RyPg0KICAgICAgICAgICAgICAg
ICAgICAgICAgPC90Ym9keT4NCiAgICAgICAgICAgICAgICAgICAgIDwvdGFibGU+DQogICAg
ICAgICAgICAgICAgICA8L3RleHQ+DQogICAgICAgICAgICAgICA8L3NlY3Rpb24+DQogICAg
ICAgICAgICA8L2NvbXBvbmVudD4NCiAgICAgICAgIDwvY29tcG9uZW50Pg0KICAgICAgPC9j
b21wb25lbnQ+DQogICA8L2NvbXBvbmVudD4NCjwvQ2xpbmljYWxEb2N1bWVudD4=
--------------070304090505090508040909--

";
		}

		#endregion Testing

		
	}

	public class EmailPublicResolver : EmailNameResolver {
		public EmailPublicResolver(bool isReadOnly=true) : base(isReadOnly?SystemX509Store.OpenExternal():SystemX509Store.OpenExternalEdit()) {
		}
	}

	public class EmailPrivateResolver : EmailNameResolver {
		public EmailPrivateResolver(bool isReadOnly=true) : base(isReadOnly?SystemX509Store.OpenPrivate():SystemX509Store.OpenPrivateEdit()) {
		}
	}

	public class EmailNameResolver : ICertificateResolver {

		public CertificateStore Store=null;

		///<summary>If isPublic is true then will resolve against public certificates, otherwise will resolve against private certificates.</summary>
		public EmailNameResolver(CertificateStore store) {
			Store=store;
		}

		~EmailNameResolver() {
			if(Store!=null) {
				Store.Dispose();
				Store=null;
			}
		}

		///<summary>Gets all active and valid address-level certificates for the specified email address.
		///If none found, then gets active and valid domain-level certificates for the specified email address.
		///If neither type of certificate is found, returns null.</summary>
		public X509Certificate2Collection GetCertificates(MailAddress address) {
			List <X509Certificate2> listValidCerts=new List<X509Certificate2>();
			List <X509Certificate2> listInvalidCerts=new List<X509Certificate2>();
			GetCertificates(address.Address,listValidCerts,listInvalidCerts);
			if(listValidCerts.Count > 0 || listInvalidCerts.Count > 0) {
				return new X509Certificate2Collection(listValidCerts.ToArray());
			}
			return null;
		}

		///<summary>Gets all address-level certificates for the specified email address into the two specified lists, separated by validity.
		///If none found, then gets domain-level certificates for the specified email address into the two specified lists, separated by validity.</summary>
		public void GetCertificates(string addressOrDomain,List <X509Certificate2> listValidCerts,List <X509Certificate2> listInvalidCerts) {
			if(addressOrDomain.Contains("@")) {
				GetCertificatesForAddress(addressOrDomain,listValidCerts,listInvalidCerts);
				if(listValidCerts.Count > 0) {
					//If we found at least one address specific certificate, then we ignore domain level certificates.
					//This will be true even if we only find invalid certificates, because we assume the intent of the setup was
					//to have an address specific certificate and there is currently something wrong which the user might correct.
					//We do not want to temporarily send a domain level certificate if there is a minor issue that can be easily corrected
					//for any invalid address specific certificates found.
					return;
				}
			}
			string domain=addressOrDomain;
			if(addressOrDomain.Contains("@")) {
				MailAddress address=new MailAddress(addressOrDomain);
				domain=address.Host;
			}
			GetCertificatesForDomain(domain,listValidCerts,listInvalidCerts);
		}

		///<summary>Gets all active and valid address-level certificates for the specified email address.</summary>
		public X509Certificate2Collection GetCertificatesForAddress(string emailAddress) {
			List <X509Certificate2> listValidCerts=new List<X509Certificate2>();
			List <X509Certificate2> listInvalidCerts=new List<X509Certificate2>();
			GetCertificatesForAddress(emailAddress,listValidCerts,listInvalidCerts);
			return new X509Certificate2Collection(listValidCerts.ToArray());
		}

		///<summary>Gets all address-level certificates for the specified email address into the two specified lists, separated by validity.</summary>
		public void GetCertificatesForAddress(string emailAddress,List <X509Certificate2> listValidCerts,List <X509Certificate2> listInvalidCerts) {
			X509Certificate2Collection colCerts=Store.GetAllCertificates();
			foreach(X509Certificate2 cert in colCerts) {
				string subjectName=GetCertSubjectName(cert);
				if(subjectName!="" && subjectName.ToLower()!=emailAddress.ToLower()) {
					continue;
				}
				if(GetCertRfc822Name(cert).ToLower()!=emailAddress.ToLower()) {
					continue;
				}
				if(IsCertValid(cert)) {
					listValidCerts.Add(cert);
				}
				else {
					listInvalidCerts.Add(cert);
				}
			}
		}

		///<summary>Gets all active and valid domain-level certificates for the specified domain name.</summary>
		public X509Certificate2Collection GetCertificatesForDomain(string domain) {
			List <X509Certificate2> listValidCerts=new List<X509Certificate2>();
			List <X509Certificate2> listInvalidCerts=new List<X509Certificate2>();
			GetCertificatesForDomain(domain,listValidCerts,listInvalidCerts);
			return new X509Certificate2Collection(listValidCerts.ToArray());
		}

		///<summary>Gets all domain-level certificates for the specified domain name into the two specified lists, separated by validity.</summary>
		public void GetCertificatesForDomain(string domain,List <X509Certificate2> listValidCerts,List <X509Certificate2> listInvalidCerts) {
			X509Certificate2Collection colCerts=Store.GetAllCertificates();
			foreach(X509Certificate2 cert in colCerts) {
				string subjectName=GetCertSubjectName(cert);
				if(subjectName!="" && subjectName.ToLower()!=domain.ToLower()) {
					continue;
				}
				if(GetCertDnsName(cert).ToLower()!=domain.ToLower()) {
					continue;
				}
				if(IsCertValid(cert)) {
					listValidCerts.Add(cert);
				}
				else {
					listInvalidCerts.Add(cert);
				}
			}
		}

		///<summary>Returns the subject name intended for email security from the given signed certificate.
		///Returns empty string if a subject name was not found for email security, which would imply that the certificate is not for email encryption use.</summary>
		public static string GetCertSubjectName(X509Certificate2 cert) {
			string[] arraySubjectNames=cert.SubjectName.Name.Split(',');
			for(int i=0;i<arraySubjectNames.Length;i++) {
				string typeAndName=arraySubjectNames[i].Trim();
				if(typeAndName.ToUpper().StartsWith("E=")) {
					string name=typeAndName.Substring(2);
					return name;
				}
			}
			return "";
		}
	
		///<summary>The RFC822 name is the fully quilified email address.</summary>
		public static string GetCertRfc822Name(X509Certificate2 cert) {
			foreach(X509Extension extension in cert.Extensions) {
				if(extension.Oid.FriendlyName.ToLower()!="subject alternative name") {
					continue;
				}
				AsnEncodedData asndata=new AsnEncodedData(extension.Oid,extension.RawData);
				string san=asndata.Format(true);
				string[] arraySanFields=san.Replace("\r","").Split('\n');
				foreach(string field in arraySanFields) {
					string fieldName="rfc822 name=";
					if(field.ToLower().StartsWith(fieldName)) {
						return field.Substring(fieldName.Length);
					}
				}
			}
			return "";
		}

		///<summary>The DNS name is the domain name part of the email address.</summary>
		public static string GetCertDnsName(X509Certificate2 cert) {
			foreach(X509Extension extension in cert.Extensions) {
				if(extension.Oid.FriendlyName.ToLower()!="subject alternative name") {
					continue;
				}
				AsnEncodedData asndata=new AsnEncodedData(extension.Oid,extension.RawData);
				string san=asndata.Format(true);
				string[] arraySanFields=san.Replace("\r","").Split('\n');
				foreach(string field in arraySanFields) {
					string fieldName="dns name=";
					if(field.ToLower().StartsWith(fieldName)) {
						return field.Substring(fieldName.Length);
					}
				}
			}
			return "";
		}

		///<summary>Before returning the certificate, verify that it is valid using the default trust flags provided by Direct.
		///This is how we avoid returning expired or revoked certificates etc.
		///This will ignore whether or not the certificate is trusted as a result of a matching trust anchor (this step is done later).
		///</summary>
		public static bool IsCertValid(X509Certificate2 cert) {
			//This code mimics Health.Direct.Agent.TrustChainValidator.IsTrustedCertificate().
			X509Chain chainBuilder=new X509Chain();
			chainBuilder.ChainPolicy=new X509ChainPolicy();
			chainBuilder.Build(cert);
			foreach(X509ChainElement chainElement in chainBuilder.ChainElements) {
				if(chainElement.ChainElementStatus.Any(s => (s.Status & Health.Direct.Agent.TrustChainValidator.DefaultProblemFlags) != 0)) {
					return false;
				}
			}
			return true;
		}

		///<summary>This is required by the interface.</summary>
		public event Action<ICertificateResolver, Exception> Error;
	}

}













