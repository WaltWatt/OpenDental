using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;
using System.Linq;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormEmailInbox:ODForm {
	///<summary>Do not access directly.  Instead use AddressInbox.</summary>
	private EmailAddress _addressInbox=null;
    ///<summary>Should always match the combobox 1:1.</summary>
		private List<EmailAddress> _listEmailAddresses;
    ///<summary>Unfiltered list of emails showing in the Inbox.</summary>
		private List<EmailMessage> _listInboxEmails;
    ///<summary>Unfiltered list of emails showing in the Sent Messages box.</summary>
    private List<EmailMessage> _listSentEmails;
    ///<summary>Gets refreshed as needed.</summary>
    Dictionary<long,string> _dictPatNames=Patients.GetAllPatientNames();
    ///<summary>Indicates whether or not the Sent Messages box needs to be refreshed.</summary>
    private bool _isRefreshSent=true;
    ///<summary>Indicates whether or not the Inbox needs to be refreshed.</summary>
    private bool _isRefreshInbox=true;
    ///<summary>The patnum of the patient chosen by the user by clicking the Patient Pick button when searching.</summary>
    private long _searchPatNum;
    ///<summary>True when the user is search mode.</summary>
    private bool _isSearching=false;
    ///<summary>When _isSearching is true, the following list is used instead if _listInboxEmails.
    ///At the point that _isSearching is true, it should always be filled.</summary>
    private List<EmailMessage> _listInboxSearched;
    ///<summary>When _isSearching is true, the following list is used instead of _listSentEmails. 
    ///At the point that _isSearching is true, it should always be filled.</summary>
    private List<EmailMessage> _listSentMessagesSearched;
		///<summary>True when the form has been closed.</summary>
		private bool _hasClosed;
		///<summary>List of all HideInFlags, except None.</summary>
		private List<HideInFlags> _listHideInFlags;

    ///<summary>The currently showing mailbox.</summary>
    private MailboxType ActiveMailbox {
      get {
        switch(tabControl1.SelectedIndex) {
          case 0:
            return MailboxType.Inbox;
          case 1:
            return MailboxType.Sent;
          default:
            throw new Exception("Unknown Mailbox");
        }
      }
    }

    ///<summary>The currently showing grid. Based off ActiveMailbox above.</summary>
    private ODGrid ActiveGrid {
      get {
        switch(ActiveMailbox) {
          case MailboxType.Inbox:
            return gridInbox;
          case MailboxType.Sent:
            return gridSent;
          default:
            throw new Exception("Unknown Mailbox");
        }
      }
    }

    ///<summary>The currently selected email address.</summary>
    private EmailAddress AddressCur {
      get {
        if(comboEmailAddress.SelectedIndex==-1) {
          _addressInbox=null;
        }
        else {
          _addressInbox=_listEmailAddresses[comboEmailAddress.SelectedIndex];
        }
        return _addressInbox;
      }
    }

    public FormEmailInbox() {
			InitializeComponent();
			Lan.F(this);			
		}

		private void FormEmailInbox_Resize(object sender,EventArgs e) {
			if(_hasClosed) {
				return;
			}
			if(_listEmailAddresses!=null) {//Since Resize() is called once before Load(), we must ensure FillComboEmail() has been called at least once.
				FillInboxOrSent();
			}
		}

		private void FormEmailInbox_Load(object sender,EventArgs e) {
      Cursor=Cursors.WaitCursor;
			//default dates to the last month
			textDateFrom.Text=DateTime.Now.AddDays(-31).ToShortDateString();
			textDateTo.Text=DateTime.Now.ToShortDateString();
			InitEmailHideInFlags();
      FillComboEmail();
			SetButtonsEnabled();
      if(_listEmailAddresses.Count==0) {
				MsgBox.Show(this,"No email addresses available for current user.  Edit email address info in the File menu or Setup menu.");
				this.Close();
				return;
			}
			textComputerNameReceive.Text=PrefC.GetString(PrefName.EmailInboxComputerName);
			textComputerName.Text=Dns.GetHostName();
			Application.DoEvents();//Show the form contents before loading email into the grid.
			try {
				GetMessages();//If no new messages, then the user will know based on what shows in the inbox grid.
				FillGridSent();
			}
			catch (ODException ex) {
				if(ex.ErrorCodeAsEnum == ODException.ErrorCodes.FormClosed) {
					return;
				}
				throw ex;
			}
      Cursor=Cursors.Default;
    }

    ///<summary>This function is called on load and also when signals are received indicating a refresh is needed for email addresses.</summary>
		private void FillComboEmail() {
      //Emails to show:
      //Default Practice
      //Default Clinic
      //Me
      //All other email addresses not tied to a user 
      long previousSelectedEmailAddressNum=(AddressCur==null)?0:AddressCur.EmailAddressNum;
      _listEmailAddresses=new List<EmailAddress>();
			List<EmailAddress> listAddresses=EmailAddresses.GetDeepCopy();//Does not include user specific email addresses.
			//Exclude any email addresses which are associated to clinics.
			if(PrefC.HasClinicsEnabled) {
				List<Clinic> listClinicsAll=Clinics.GetDeepCopy();
				for(int i=0;i<listClinicsAll.Count;i++) {
          listAddresses.RemoveAll(x => x.EmailAddressNum==listClinicsAll[i].EmailAddressNum);
				}
			}
      //Exclude default practice email address, since it is added on another line below.
      listAddresses.RemoveAll(x => x.EmailAddressNum==PrefC.GetLong(PrefName.EmailDefaultAddressNum));
      //Exclude web mail notification email address.
      listAddresses.RemoveAll(x => x.EmailAddressNum==PrefC.GetLong(PrefName.EmailNotifyAddressNum));
			//Add clinic defaults that the user has access to.  Do not add duplicates.
			if(PrefC.HasClinicsEnabled) {
				List<Clinic> listClinicForUser=Clinics.GetForUserod(Security.CurUser);
				for(int i=0;i < listClinicForUser.Count;i++) {
					EmailAddress emailClinic=EmailAddresses.GetByClinic(listClinicForUser[i].ClinicNum);
					if(listAddresses.Any(x => x.EmailAddressNum == emailClinic.EmailAddressNum)) {
						continue;
					}
          listAddresses.Insert(0,emailClinic);
				}
			}
      EmailAddress emailAddressPractice=EmailAddresses.GetOne(PrefC.GetLong(PrefName.EmailDefaultAddressNum));
      EmailAddress emailAddressMe=EmailAddresses.GetForUser(Security.CurUser.UserNum);
      //Add addresses which are: not associated to anything, or not default, or unique per clinic.
      comboEmailAddress.Items.Clear();
      for(int i=0;i<listAddresses.Count();i++) {
        if ((emailAddressPractice!=null && listAddresses[i].EmailUsername.Trim().ToLower()==emailAddressPractice.EmailUsername.Trim().ToLower())
          || (emailAddressMe!=null && listAddresses[i].EmailUsername.Trim().ToLower()==emailAddressMe.EmailUsername.Trim().ToLower()))
        {
          continue;
        }
        comboEmailAddress.Items.Add(listAddresses[i].EmailUsername);
        _listEmailAddresses.Add(listAddresses[i]);
      }
			//Add the practice default.
			if(emailAddressPractice!=null) {
				comboEmailAddress.Items.Insert(0,"Default <"+emailAddressPractice.EmailUsername+">");
				_listEmailAddresses.Insert(0,emailAddressPractice);//Practice Default
			}
			//Add the personal email address for the current user.
			if(emailAddressMe!=null) {
				comboEmailAddress.Items.Insert(0,"Me <"+emailAddressMe.EmailUsername+">");
				_listEmailAddresses.Insert(0,emailAddressMe);//"Me"
			}
			if(comboEmailAddress.Items.Count > 0) {
				comboEmailAddress.SelectedIndex=0;//This could be the default practice address, or personal address, or another address.
			}
			if(Security.CurUser.ProvNum!=0) { //If the first item in the combobox is selected, make checks to see if the current user has a provnum.
				comboEmailAddress.Items.Insert(0,"WebMail");//Only providers have access to see Webmail messages.
        _listEmailAddresses.Insert(0,new EmailAddress {
          EmailUsername="WebMail",
          WebmailProvNum=Security.CurUser.ProvNum});
      }
      for(int i=0;i<_listEmailAddresses.Count;i++) {
        EmailAddress addressCur=_listEmailAddresses[i];
        if(addressCur.EmailAddressNum==previousSelectedEmailAddressNum) {
          comboEmailAddress.SelectedIndex=i;
        }
      }
      //The user might have updated the computer to receive emails from, so we need to update in UI.
      textComputerNameReceive.Text=PrefC.GetString(PrefName.EmailInboxComputerName);
		}

		private void menuItemSetup_Click(object sender,EventArgs e) {
      if(!Security.IsAuthorized(Permissions.Setup)) {
        return;
      }
      FormEmailAddresses FormEA=new FormEmailAddresses();
      FormEA.ShowDialog();
      FillComboEmail();
		}

		///<summary>Gets new messages from email inbox, as well as older messages from the db. Also fills the grid.</summary>
		private int GetMessages() {
     _isRefreshInbox=true;
      FillGridInbox();//Show what is in db.
      if(comboEmailAddress.SelectedIndex==0 && Security.CurUser.ProvNum!=0) { //WebMail is selected
        return 0;//Webmail messages are in the database, so we will not need to receive email from the email server.
      }
      if(AddressCur.EmailUsername=="" || AddressCur.Pop3ServerIncoming=="") {//Email address not setup.
        return 0;
      }
      Application.DoEvents();//So that something is showing while the page is loading.
      if(!CodeBase.ODEnvironment.IdIsThisComputer(PrefC.GetString(PrefName.EmailInboxComputerName))) {//This is not the computer to get new messages from.
        return 0;
      }
      if(PrefC.GetString(PrefName.EmailInboxComputerName)=="") {
        MsgBox.Show(this,"Computer name to receive new email from has not been setup.");
        return 0;
      }
      int emailMessagesTotalCount=0;
      Text="Email Inbox for "+AddressCur.EmailUsername+" - Receiving new email...";
			bool hasMoreEmail = true;
			while(hasMoreEmail) {
				if(_hasClosed) {
					throw new ODException("Email inbox has been closed.",ODException.ErrorCodes.FormClosed);
				}
				List<EmailMessage> emailMessages= new List<EmailMessage>();
				try {
					emailMessages=EmailMessages.ReceiveFromInbox(1,AddressCur);
				}
				catch(Exception ex) {
					MessageBox.Show(Lan.g(this,"Error receiving email messages")+": "+ex.Message);
				}
				emailMessagesTotalCount+=emailMessages.Count;
				if(emailMessages.Count==0) {
					hasMoreEmail=false;
				}
				else { //Show messages as they are downloaded, to indicate to the user that the program is still processing.
					RefreshInboxEmailList();
					FillGridInbox();
					Application.DoEvents();
				}
			}
      Text="Email Inbox for "+AddressCur.EmailUsername+" - Resending any acknowledgments which previously failed...";
      EmailMessages.SendOldestUnsentAck(AddressCur);
      Text="Email Inbox for "+AddressCur.EmailUsername;
      return emailMessagesTotalCount;
		}

		///<summary>Gets new emails and also shows older emails from the database.</summary>
		private void FillGridInbox() {
     //Remember current email selections and sorting column.
      List<long> listEmailMessageNumsSelected=new List<long>();
      for(int i=0;i<gridInbox.SelectedIndices.Length;i++) {
        EmailMessage emailMessage=(EmailMessage)gridInbox.Rows[gridInbox.SelectedIndices[i]].Tag;
        listEmailMessageNumsSelected.Add(emailMessage.EmailMessageNum);
      }
      int sortByColIdx=gridInbox.SortedByColumnIdx;
      bool isSortAsc=gridInbox.SortedIsAscending;
      if(sortByColIdx==-1) {
        //Default to sorting by Date Received descending.
        sortByColIdx=2;
        isSortAsc=false;
      }
      if(_listInboxEmails==null || _isRefreshInbox) {
        RefreshInboxEmailList();
      }
      gridInbox.BeginUpdate();
      gridInbox.Rows.Clear();
      gridInbox.Columns.Clear();
      int colReceivedDatePixCount=140;
      int colMessageTypePixCount=120;
      int colFromPixCount=200;
      int colSigPixCount=40;
      int colPatientPixCount=140;
      int variableWidth=gridInbox.Width-10-colFromPixCount-colReceivedDatePixCount-colMessageTypePixCount-colSigPixCount-colPatientPixCount;
			gridInbox.Columns.Add(new UI.ODGridColumn(Lan.g(this,"From"),colFromPixCount,HorizontalAlignment.Left));//0
      gridInbox.Columns[gridInbox.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;
			gridInbox.Columns.Add(new UI.ODGridColumn(Lan.g(this,"Subject"),variableWidth,HorizontalAlignment.Left));//1
			gridInbox.Columns[gridInbox.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;
			gridInbox.Columns.Add(new UI.ODGridColumn(Lan.g(this,"Date Received"),colReceivedDatePixCount,HorizontalAlignment.Left));//2
			gridInbox.Columns[gridInbox.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.DateParse;
			gridInbox.Columns.Add(new UI.ODGridColumn(Lan.g(this,"MessageType"),colMessageTypePixCount,HorizontalAlignment.Left));//3
			gridInbox.Columns[gridInbox.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;
			gridInbox.Columns.Add(new UI.ODGridColumn(Lan.g(this,"Sig"),colSigPixCount,HorizontalAlignment.Center));//4
			gridInbox.Columns[gridInbox.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;
			gridInbox.Columns.Add(new UI.ODGridColumn(Lan.g(this,"Patient"),colPatientPixCount,HorizontalAlignment.Left));//5
      gridInbox.Columns[gridInbox.Columns.Count-1].SortingStrategy=UI.GridSortingStrategy.StringCompare;
      List<EmailMessage> listEmailsFiltered;
      if(_isSearching) { //if searching, use the search list. Should already be filled.
        listEmailsFiltered=_listInboxSearched.Where(x => EmailMessages.GetAddressSimple(x.RecipientAddress).Contains(AddressCur.EmailUsername)).ToList();
      }
      else {
        if(AddressCur.EmailUsername=="WebMail") {
          listEmailsFiltered=_listInboxEmails.Where(x => x.ProvNumWebMail==Security.CurUser.ProvNum).ToList();
        }
        else {
          listEmailsFiltered=_listInboxEmails.Where(x => EmailMessages.GetAddressSimple(x.RecipientAddress).Contains(AddressCur.EmailUsername)).ToList();
        }
      }
      for(int i=0;i<listEmailsFiltered.Count;i++) {
        EmailMessage emailMessage=listEmailsFiltered[i];
        if(!checkShowHiddenEmails.Checked && emailMessage.HideIn.HasFlag(HideInFlags.EmailInbox)) {
          continue;
        }
        UI.ODGridRow row=new UI.ODGridRow();
        row.Tag=emailMessage;//Used to locate the correct email message if the user decides to sort the grid.
        if(emailMessage.SentOrReceived==EmailSentOrReceived.Received || emailMessage.SentOrReceived==EmailSentOrReceived.WebMailReceived
          || emailMessage.SentOrReceived==EmailSentOrReceived.ReceivedEncrypted || emailMessage.SentOrReceived==EmailSentOrReceived.ReceivedDirect) {
          row.Bold=true;//unread
          //row.ColorText=UI.ODPaintTools.ColorNotify;
        }
        row.Cells.Add(new UI.ODGridCell(emailMessage.FromAddress));//0 From
        row.Cells.Add(new UI.ODGridCell(emailMessage.Subject));//1 Subject
        row.Cells.Add(new UI.ODGridCell(emailMessage.MsgDateTime.ToString()));//2 ReceivedDate
        row.Cells.Add(new UI.ODGridCell(EmailMessages.GetEmailSentOrReceivedDescript(emailMessage.SentOrReceived)));//3 MessageType
        string sigTrust="";//Blank for no signature, N for untrusted signature, Y for trusted signature.
        for(int j=0;j<emailMessage.Attachments.Count;j++) {
          if(emailMessage.Attachments[j].DisplayedFileName.ToLower()!="smime.p7s") {
            continue;//Not a digital signature.
          }
          sigTrust="N";
          //A more accurate way to test for trust would be to read the subject name from the certificate, then check the trust for the subject name instead of the from address.
          //We use the more accurate way inside FormEmailDigitalSignature.  However, we cannot use the accurate way inside the inbox because it would cause the inbox to load very slowly.
          if(EmailMessages.GetReceiverUntrustedCount(emailMessage.FromAddress)==-1) {
            sigTrust="Y";
          }
          break;
        }
        row.Cells.Add(new UI.ODGridCell(sigTrust));//4 Sig
        long patNumRegardingPatient=emailMessage.PatNum;
        //Webmail messages should list the patient as the PatNumSubj, which means "the patient whom this message is regarding".
        if(emailMessage.SentOrReceived==EmailSentOrReceived.WebMailReceived || emailMessage.SentOrReceived==EmailSentOrReceived.WebMailRecdRead) {
          patNumRegardingPatient=emailMessage.PatNumSubj;
        }
        string patName="";
        if(patNumRegardingPatient!=0 && !_dictPatNames.TryGetValue(patNumRegardingPatient,out patName)) {
          _dictPatNames=Patients.GetAllPatientNames();
          _dictPatNames.TryGetValue(patNumRegardingPatient,out patName);
        }
        row.Cells.Add(patName);//5 Patient
        gridInbox.Rows.Add(row);
      }
      gridInbox.EndUpdate();
      //sorting/reselcting previously selected rows
      gridInbox.SortForced(sortByColIdx,isSortAsc);
      //Selection must occur after EndUpdate().
      for(int i=0;i<gridInbox.Rows.Count;i++) {
        EmailMessage emailMessage=(EmailMessage)gridInbox.Rows[i].Tag;
        if(listEmailMessageNumsSelected.Contains(emailMessage.EmailMessageNum)) {
          gridInbox.SetSelected(i,true);
        }
      }
      if(gridInbox.SelectedIndices.Length!=1) { //collapse the panel if they don't have exactly one email selected.
        splitContainerNoFlicker.Panel2Collapsed=true;
      }
			RefreshShowIn();
		}

    ///<summary>Fills the Sent grid with emails. 
    ///Set _refreshSent to true to refresh the local list of emails from the db before calling this method.</summary>
    private void FillGridSent() {
      //remember emails selected
      List<long> listEmailMessageNumsSelected=new List<long>();
      for(int i=0;i<gridSent.SelectedIndices.Length;i++) {
        EmailMessage emailMessage=(EmailMessage)gridSent.Rows[gridSent.SelectedIndices[i]].Tag;
        listEmailMessageNumsSelected.Add(emailMessage.EmailMessageNum);
      }
      //sorting
      int sortByColIdx=gridSent.SortedByColumnIdx;
      bool isSortAsc=gridSent.SortedIsAscending;
      if(sortByColIdx==-1) {
        //Default to sorting by Date Received descending.
        sortByColIdx=2;
        isSortAsc=false;
      }
      if(_listSentEmails==null || _isRefreshSent==true) {
        RefreshSentEmailList();
      }
      //calculate column widths
      int colSentToPixCount=200;
      int colSentDatePixCount=140;
      int colMessageTypePixCount=120;
      int colSigPixCount=40;
      int colPatientPixCount=140;
      int variableWidth=gridSent.Width-10-colSentToPixCount-colSentDatePixCount-colMessageTypePixCount-colSigPixCount-colPatientPixCount;
      gridSent.BeginUpdate();
      gridSent.Columns.Clear();
      //add columns
      ODGridColumn col=new ODGridColumn("Sent To",colSentToPixCount,HorizontalAlignment.Left);
      col.SortingStrategy=GridSortingStrategy.StringCompare;
      gridSent.Columns.Add(col);
      col=new ODGridColumn("Subject",variableWidth,HorizontalAlignment.Left);
      col.SortingStrategy=GridSortingStrategy.StringCompare;
      gridSent.Columns.Add(col);
      col=new ODGridColumn("Date Sent",colSentDatePixCount,HorizontalAlignment.Left);
      col.SortingStrategy=GridSortingStrategy.DateParse;
      gridSent.Columns.Add(col);
      col=new ODGridColumn("MsgType",colMessageTypePixCount,HorizontalAlignment.Left);
      col.SortingStrategy=GridSortingStrategy.StringCompare;
      gridSent.Columns.Add(col);
      col=new ODGridColumn("Sig",colSigPixCount,HorizontalAlignment.Center);
      col.SortingStrategy=GridSortingStrategy.StringCompare;
      gridSent.Columns.Add(col);
      col=new ODGridColumn("Patient",colPatientPixCount,HorizontalAlignment.Left);
      col.SortingStrategy=GridSortingStrategy.StringCompare;
      gridSent.Columns.Add(col);
      gridSent.Rows.Clear();
      List<EmailMessage> listEmailsFiltered;
      if(_isSearching) { //if searching, use the search list. Should be prefilled.
        listEmailsFiltered=_listSentMessagesSearched.Where(x => AddressCur.EmailUsername==EmailMessages.GetAddressSimple(x.FromAddress)).ToList();
      }
      else {
        if(AddressCur.EmailUsername=="WebMail") {
          listEmailsFiltered=_listInboxEmails.Where(x => x.ProvNumWebMail==Security.CurUser.ProvNum).ToList();
        }
        else {
          listEmailsFiltered=_listSentEmails.Where(x => AddressCur.EmailUsername==EmailMessages.GetAddressSimple(x.FromAddress)).ToList();
        }
      }
      //add rows
      foreach(EmailMessage emailMessage in listEmailsFiltered) {
				if(!checkShowHiddenEmails.Checked && emailMessage.HideIn.HasFlag(HideInFlags.EmailInbox)) {//We might need a separate HideInFlags.EmailSent option later.
          continue;
        }
        ODGridRow row=new ODGridRow();
        row.Cells.Add(emailMessage.ToAddress);
        row.Cells.Add(emailMessage.Subject);
        row.Cells.Add(emailMessage.MsgDateTime.ToString());
        row.Cells.Add(emailMessage.SentOrReceived.ToString());
        string sigTrust="";
        for(int j=0;j<emailMessage.Attachments.Count;j++) {
          if(emailMessage.Attachments[j].DisplayedFileName.ToLower()!="smime.p7s") {
            continue;//Not a digital signature.
          }
          sigTrust="N";
          //A more accurate way to test for trust would be to read the subject name from the certificate, then check the trust for the subject name instead of the from address.
          //We use the more accurate way inside FormEmailDigitalSignature.  However, we cannot use the accurate way inside the inbox because it would cause the inbox to load very slowly.
          if(EmailMessages.IsSenderTrusted(emailMessage.FromAddress)) {
            sigTrust="Y";
          }
          break;
        }
        row.Cells.Add(sigTrust);
        long patNumRegardingPatient=emailMessage.PatNum;
        //Webmail messages should list the patient as the PatNumSubj, which means "the patient whom this message is regarding".
        if(emailMessage.SentOrReceived==EmailSentOrReceived.WebMailReceived || emailMessage.SentOrReceived==EmailSentOrReceived.WebMailRecdRead) {
          patNumRegardingPatient=emailMessage.PatNumSubj;
        }
        string patName="";
        if(patNumRegardingPatient!=0&&!_dictPatNames.TryGetValue(patNumRegardingPatient,out patName)) {
          _dictPatNames=Patients.GetAllPatientNames();
          _dictPatNames.TryGetValue(patNumRegardingPatient,out patName);
        }
        row.Cells.Add(patName);//5 Patient
        row.Tag=emailMessage;
        gridSent.Rows.Add(row);
      }
      gridSent.EndUpdate();
      //sorting/reselcting previously selected rows
      gridSent.SortForced(sortByColIdx,isSortAsc);
      //Select the previously selected emails
      for(int i=0;i<gridSent.Rows.Count;i++) {
        EmailMessage emailMessage=(EmailMessage)gridSent.Rows[i].Tag;
        if(listEmailMessageNumsSelected.Contains(emailMessage.EmailMessageNum)) {
          gridSent.SetSelected(i,true);
        }
      }
			RefreshShowIn();
    }

		///<summary>Loads HideInFlags into listEmailHideInFlags and adds them listHideInFlags.</summary>
		private void InitEmailHideInFlags() {
			_listHideInFlags=Enum.GetValues(typeof(HideInFlags)).Cast<HideInFlags>().Where(x => x!=HideInFlags.None).ToList();
			for(int i=0;i<_listHideInFlags.Count;i++) {
				listShowIn.Items.Add(Lan.g("enumHideInFlags",((HideInFlags)(_listHideInFlags[i])).GetDescription()));
			}
		}

		///<summary>Updates listShowIn according to current selections in GridSent/GridInbox</summary>
		private void RefreshShowIn() {
			EmailMessage emailMessageCur=null;
			if(ActiveGrid.SelectedGridRows.Count!=0) { //at least one selected email
				emailMessageCur=(EmailMessage)(ActiveGrid.SelectedGridRows[0].Tag);
			}
			//set all listShowIn Items to match selected email, default to all items not selected if no email selected
			for(int i=0;i<_listHideInFlags.Count;i++) {
				listShowIn.Items[i]=Lan.g("enumHideInFlags",_listHideInFlags[i].GetDescription());
				if(emailMessageCur==null) {
					listShowIn.SetSelected(i,false);//no selected email
				}
				else {
					bool isShowing=!emailMessageCur.HideIn.HasFlag(_listHideInFlags[i]);//HideInFlags logic inverted for "showing"
					listShowIn.SetSelected(i,isShowing);
				}
			}
			if(emailMessageCur==null) {
				listShowIn.Enabled=false;
				return;
			}
			//compare current email HideInFlags to other selected emails and modify listShowIn accordingly
			foreach(ODGridRow row in ActiveGrid.SelectedGridRows) {
				EmailMessage emailMessage=(EmailMessage)row.Tag;
				if(emailMessage.HideIn==emailMessageCur.HideIn) {
					continue;
				}
				//mismatched flags across multiple selected emails should display as "showing"+"Settings Vary"
				for(int i=0;i<_listHideInFlags.Count;i++) {
					if(emailMessage.HideIn.HasFlag(_listHideInFlags[i])!=emailMessageCur.HideIn.HasFlag(_listHideInFlags[i])) {
						listShowIn.Items[i]=Lan.g("enumHideInFlags",_listHideInFlags[i].GetDescription()+" *Settings Vary");
						listShowIn.SetSelected(i,true);
					}
				}
			}
		}

    ///<summary>Fills the two classwide search lists and calls the FillGrid methods to filter them.
    ///By the time this is called, the _isSearching variable should already be set to true.</summary>
    private void FillSearch() {
      string searchBody=textSearchBody.Text;
      string searchEmail=textSearchEmail.Text;
      DateTime searchDateFrom=PIn.Date(textDateFrom.Text); //returns MinVal if empty or invalid.
      DateTime searchDateTo=PIn.Date(textDateTo.Text); //returns MinVal if empty or invalid.
      List<EmailMessage> listInboxSearched=new List<EmailMessage>();
      List<EmailMessage> listSentMessagesSearched=new List<EmailMessage>();
      if(searchBody!="") {
        //We have to run a query here to search the database, since our cache only includes the first 50 characters of the body text for preview.
        List<EmailMessage> listEmailsSearched=EmailMessages.GetBySearch(_searchPatNum,searchEmail,searchDateFrom,searchDateTo,searchBody,checkSearchAttach.Checked);
        //inbox emails
        listInboxSearched=listEmailsSearched
          .Where(x => x.SentOrReceived
          .In(EmailSentOrReceived.Read,EmailSentOrReceived.ReadDirect,EmailSentOrReceived.Received,
            EmailSentOrReceived.ReceivedDirect,EmailSentOrReceived.ReceivedEncrypted,
            EmailSentOrReceived.WebMailRecdRead,EmailSentOrReceived.WebMailReceived)).ToList();
        //sent messages
        listSentMessagesSearched=listEmailsSearched
          .Where(x => x.SentOrReceived
          .In(EmailSentOrReceived.Sent,EmailSentOrReceived.SentDirect,
            EmailSentOrReceived.WebMailSent,EmailSentOrReceived.WebMailSentRead)).ToList();
      }
      else {
        //if not filtering by subject/body, then don't look through the db.
        //Filter Inbox Emails
        foreach(EmailMessage messageCur in _listInboxEmails) {
          if(_searchPatNum!=0) {
            if(messageCur.PatNum!=_searchPatNum) {
              continue;
            }
          }
          if(!string.IsNullOrEmpty(searchEmail)) {
            if(!messageCur.FromAddress.Contains(searchEmail) && !messageCur.ToAddress.Contains(searchEmail)
              && !messageCur.RecipientAddress.Contains(searchEmail) && !messageCur.CcAddress.Contains(searchEmail)
              && !messageCur.BccAddress.Contains(searchEmail))
            {
              continue;
            }
          }
          if(checkSearchAttach.Checked) {
            if(messageCur.Attachments.Count<1) {
              continue;
            }
          }
          if(searchDateFrom!=DateTime.MinValue) {
            if(messageCur.MsgDateTime<searchDateFrom) {
              continue;
            }
          }
          if(searchDateTo!=DateTime.MinValue) {
            if(messageCur.MsgDateTime>searchDateTo) {
              continue;
            }
          }
          listInboxSearched.Add(messageCur); //only happens if all the criteria are filled.
        }
        //Filter Sent Emails
        foreach(EmailMessage messageCur in _listSentEmails) {
          if(_searchPatNum!=0) {
            if(messageCur.PatNum!=_searchPatNum) {
              continue;
            }
          }
          if(!string.IsNullOrEmpty(searchEmail)) {
            if(!messageCur.FromAddress.Contains(searchEmail) && !messageCur.ToAddress.Contains(searchEmail)
              && !messageCur.RecipientAddress.Contains(searchEmail) && !messageCur.CcAddress.Contains(searchEmail)
              && !messageCur.BccAddress.Contains(searchEmail))
            {
              continue;
            }
          }
          if(checkSearchAttach.Checked) {
            if(messageCur.Attachments.Count<1) {
              continue;
            }
          }
          if(searchDateFrom!=DateTime.MinValue) {
            if(messageCur.MsgDateTime.ToShortDateString()!=searchDateFrom.ToShortDateString()) {
              continue;
            }
          }
          listSentMessagesSearched.Add(messageCur); //only happens if all the criteria are filled.
        }
      }
      _listInboxSearched=listInboxSearched;
      _listSentMessagesSearched=listSentMessagesSearched;
      FillGridInbox();
      FillGridSent();
    }

    private void gridInboxSent_CellClick(object sender,UI.ODGridClickEventArgs e) {
			SplitContainerNoFlicker activeSplitContainer=(ActiveMailbox==MailboxType.Inbox)?splitContainerNoFlicker:splitContainerSent;
      EmailPreviewControl activePreview=(ActiveMailbox==MailboxType.Inbox)?emailPreview:emailPreviewControl1;
      SetButtonsEnabled();
			RefreshShowIn();
      if(ActiveGrid.SelectedIndices.Length>=2) {
        activeSplitContainer.Panel2Collapsed=true;//Do not show preview if there are more than one emails selected.
        return;
      }
      EmailMessage emailMessage=(EmailMessage)ActiveGrid.Rows[e.Row].Tag;
      if(EmailMessages.IsSecureWebMail(emailMessage.SentOrReceived)) {
        //We do not yet have a preview for secure web mail messages.
        activeSplitContainer.Panel2Collapsed=true;
        return;
      }
      Cursor=Cursors.WaitCursor;
      if(ActiveMailbox==MailboxType.Inbox) {
        emailMessage.SentOrReceived=EmailMessages.UpdateSentOrReceivedRead(emailMessage);
      }
      emailMessage=EmailMessages.GetOne(emailMessage.EmailMessageNum);//Refresh from the database to get the full body text.
      activeSplitContainer.Panel2Collapsed=false;
      activePreview.EmailAddressPreview=AddressCur;
      activePreview.LoadEmailMessage(emailMessage);
      FillInboxOrSent();
      Cursor=Cursors.Default;
      //Handle Sig column clicks.
      if(e.Col!=4) {
        return;//Not the Sig column.
      }
      for(int i=0;i<emailMessage.Attachments.Count;i++) {
        if(emailMessage.Attachments[i].DisplayedFileName.ToLower()!="smime.p7s") {
          continue;
        }
        string smimeP7sFilePath=FileAtoZ.CombinePaths(EmailAttaches.GetAttachPath(),emailMessage.Attachments[i].ActualFileName);
				string localFile=PrefC.GetRandomTempFile(".p7s");
				FileAtoZ.Copy(smimeP7sFilePath,localFile,FileAtoZSourceDestination.AtoZToLocal);
        X509Certificate2 certSig=EmailMessages.GetEmailSignatureFromSmimeP7sFile(localFile);
        FormEmailDigitalSignature form=new FormEmailDigitalSignature(certSig);
        if(form.ShowDialog()==DialogResult.OK) {
          Cursor=Cursors.WaitCursor;
          //If the user just added trust, then refresh to pull the newly added certificate into the memory cache.
          EmailMessages.RefreshCertStoreExternal(AddressCur);
					//Refresh the entire inbox, because there may be multiple email messages from the same address that the user just added trust for.
					//The Sig column may need to be updated on multiple rows.
					try {
						GetMessages();
					}
					catch(ODException ex) {
						if(ex.ErrorCodeAsEnum == ODException.ErrorCodes.FormClosed) {
							return;
						}
						throw ex;
					}
					Cursor=Cursors.Default;
        }
        break;
      }
		}

		private void gridInboxSent_CellDoubleClick(object sender,UI.ODGridClickEventArgs e) {
      if(e.Row==-1) {
        return;
      }
      EmailMessage emailMessage=(EmailMessage)ActiveGrid.Rows[e.Row].Tag;
      //When an email is read from the database for display in the inbox, the BodyText is limited to 50 characters and the RawEmailIn is blank.
      emailMessage=EmailMessages.GetOne(emailMessage.EmailMessageNum);//Refresh the email from the database to include the full BodyText and RawEmailIn.
      if(emailMessage.SentOrReceived==EmailSentOrReceived.WebMailReceived
          || emailMessage.SentOrReceived==EmailSentOrReceived.WebMailRecdRead
          || emailMessage.SentOrReceived==EmailSentOrReceived.WebMailSent
          || emailMessage.SentOrReceived==EmailSentOrReceived.WebMailSentRead) 
      {
        //web mail uses special secure messaging portal
        FormWebMailMessageEdit FormWMME=new FormWebMailMessageEdit(emailMessage.PatNum,emailMessage);
        //Will return Abort if validation fails on load or message was deleted, in which case do not set email as read.
        if(FormWMME.ShowDialog() != DialogResult.Abort) {
          emailMessage.SentOrReceived=EmailMessages.UpdateSentOrReceivedRead(emailMessage);//Mark the message read.
        }
      }
      else {
        emailMessage.SentOrReceived=EmailMessages.UpdateSentOrReceivedRead(emailMessage); //mark read
        FormEmailMessageEdit FormEME=new FormEmailMessageEdit(emailMessage,AddressCur,false);
				FormEME.FormClosed+=FormEmailMessage_Closed; //takes care of updating email message grid (Inbox/Sent) when edit window closes
        FormEME.Show(); //any changes are taken care of in the edit window by signals.
      }
		}

		private void butMarkUnread_Click(object sender,EventArgs e) {
      Cursor=Cursors.WaitCursor;
      for(int i=0;i<gridInbox.SelectedIndices.Length;i++) {
        EmailMessage emailMessage=(EmailMessage)gridInbox.Rows[gridInbox.SelectedIndices[i]].Tag;
        emailMessage.SentOrReceived=EmailMessages.UpdateSentOrReceivedUnread(emailMessage);
      }
      FillGridInbox();
      Cursor=Cursors.Default;
		}

		private void butMarkRead_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			for(int i=0;i<gridInbox.SelectedIndices.Length;i++) {
				EmailMessage emailMessage=(EmailMessage)gridInbox.Rows[gridInbox.SelectedIndices[i]].Tag;
        emailMessage.SentOrReceived=EmailMessages.UpdateSentOrReceivedRead(emailMessage);
			}
			FillGridInbox();
			Cursor=Cursors.Default;
		}

		private void butChangePat_Click(object sender,EventArgs e) {
      if(ActiveGrid.SelectedIndices.Length==0) {
        MsgBox.Show(this,"Please select an email message.");
        return;
      }
      FormPatientSelect form=new FormPatientSelect();
      if(form.ShowDialog()!=DialogResult.OK) {
        return;
      }
      Cursor=Cursors.WaitCursor;
      for(int i=0;i<ActiveGrid.SelectedIndices.Length;i++) {
        EmailMessage emailMessage=(EmailMessage)ActiveGrid.Rows[ActiveGrid.SelectedIndices[i]].Tag;
        emailMessage.PatNum=form.SelectedPatNum;
        EmailMessages.UpdatePatNum(emailMessage);
      }
      int messagesMovedCount=ActiveGrid.SelectedIndices.Length;
      FillInboxOrSent();
      Signalods.SetInvalid(InvalidType.EmailMessages); //will refresh for other users.
      Cursor=Cursors.Default;
      MessageBox.Show(Lan.g(this,"Email messages moved successfully")+": "+messagesMovedCount);
		}

    private void butRefresh_Click(object sender,EventArgs e) {
      Cursor=Cursors.WaitCursor;
      if(ActiveMailbox==MailboxType.Inbox) {
				try {
					GetMessages(); //takes care of refreshing from the db.
				}
				catch(ODException ex) {
					if(ex.ErrorCodeAsEnum == ODException.ErrorCodes.FormClosed) {
						return;
					}
					throw ex;
				}
			}
      else if(ActiveMailbox==MailboxType.Sent) {
        _isRefreshSent=true;
        FillGridSent();
      }
      Cursor=Cursors.Default;
    }

    ///<summary>Refreshes the local list of inbox emails.</summary>
    private void RefreshInboxEmailList() {
			DateTime searchDateFrom=PIn.Date(textDateFrom.Text); //returns MinVal if empty or invalid.
			DateTime searchDateTo=PIn.Date(textDateTo.Text); //returns MinVal if empty or invalid.
			_listInboxEmails=EmailMessages.GetMailboxForAddress(AddressCur,searchDateFrom,searchDateTo,MailboxType.Inbox);
      _isRefreshInbox=false;
    }

    ///<summary>Refreshes the local list of sent emails.</summary>
    private void RefreshSentEmailList() {
			DateTime searchDateFrom=PIn.Date(textDateFrom.Text); //returns MinVal if empty or invalid.
			DateTime searchDateTo=PIn.Date(textDateTo.Text); //returns MinVal if empty or invalid.
			_listSentEmails=EmailMessages.GetMailboxForAddress(AddressCur,searchDateFrom,searchDateTo,MailboxType.Sent);
      _isRefreshSent=false;
    }

    private void RefreshLists() {
			DateTime searchDateFrom=PIn.Date(textDateFrom.Text); //returns MinVal if empty or invalid.
			DateTime searchDateTo=PIn.Date(textDateTo.Text); //returns MinVal if empty or invalid.
			List<EmailMessage> listEmailsForSelection=EmailMessages.GetMailboxForAddress(AddressCur,searchDateFrom,searchDateTo,MailboxType.Inbox,MailboxType.Sent);
      _listInboxEmails=listEmailsForSelection.Where(x => EmailMessages.IsReceived(x.SentOrReceived)).ToList();
      _listSentEmails=listEmailsForSelection.Where(x => !EmailMessages.IsReceived(x.SentOrReceived)).ToList();
      _isRefreshSent=false;
      _isRefreshInbox=false;
    }

    ///<summary>Sets the sidebar buttons enabled or disabled based on the currently selected Mailbox.</summary>
    private void SetButtonsEnabled() {
			if(ActiveGrid.SelectedIndices.Length==0) {
				listShowIn.Enabled=false;
			}
			else {
				listShowIn.Enabled=true;
			}
      if(ActiveMailbox==MailboxType.Inbox) {
        if(gridInbox.SelectedIndices.Length>1) {
          butReply.Enabled=false;
					butReplyAll.Enabled=false;
					butForward.Enabled=false;
        }
        else {
          butReply.Enabled=true;
					butReplyAll.Enabled=true;
					butForward.Enabled=true;
        }
        butMarkRead.Enabled=true;
        butMarkUnread.Enabled=true;
      }
      else if(ActiveMailbox==MailboxType.Sent) {
        butReply.Enabled=false;
        butReplyAll.Enabled=false;
				butForward.Enabled=false;
        butMarkRead.Enabled=false;
        butMarkUnread.Enabled=false;
      }
    }

    ///<summary>For searching.</summary>
    private void butPickPat_Click(object sender,EventArgs e) {
      FormPatientSelect FormPS=new FormPatientSelect();
      FormPS.ShowDialog();
      if(FormPS.DialogResult==DialogResult.OK) {
        _searchPatNum=FormPS.SelectedPatNum;
        string patName;
        if(!_dictPatNames.TryGetValue(FormPS.SelectedPatNum,out patName)) {
          _dictPatNames=Patients.GetAllPatientNames();
          _dictPatNames.TryGetValue(FormPS.SelectedPatNum,out patName);
        }
        textSearchPat.Text=patName;
      }
    }

    private void butSearch_Click(object sender,EventArgs e) {
      if(textSearchBody.Text=="" && textDateFrom.Text=="" && textDateTo.Text==""&& textSearchEmail.Text=="" && textSearchPat.Text=="" && !checkSearchAttach.Checked) {
        MsgBox.Show(this,"Please specify some search criteria before searching.");
        return;
      }
      Cursor=Cursors.WaitCursor;
      _isSearching=true;
      gridInbox.Title=Lan.g(this,"Currently Searching - Inbox");
      gridSent.Title=Lan.g(this,"Currently Searching - Sent Messages");
      butClear.Enabled=true;
      groupSearch.BackColor=Color.FromArgb(255,255,192); //same as the color of the Appointment Scheduler
      //if the user typed something into the Subject/Body textbox, then we need to go to the database.
      FillSearch();
      FillInboxOrSent();
      Cursor=Cursors.Default;
    }

    ///<summary>Clears search fields.</summary>
    private void butClear_Click(object sender,EventArgs e) {
      textSearchBody.Text="";
      textDateFrom.Text="";
      textDateTo.Text="";
      textSearchEmail.Text="";
      textSearchPat.Text="";
      checkSearchAttach.Checked=false;
      _searchPatNum=0;
      _isSearching=false;
      butClear.Enabled=false;
      gridInbox.Title=Lan.g(this,"Inbox");
      gridSent.Title=Lan.g(this,"Sent Messages");
      groupSearch.BackColor=SystemColors.Control;
      FillInboxOrSent();
    }

    private void butCompose_Click(object sender,EventArgs e) {
      if(!Security.IsAuthorized(Permissions.EmailSend)){ 
        return;
      }
      EmailMessage message=new EmailMessage();
			message.FromAddress=AddressCur.GetFrom();
      FormEmailMessageEdit FormE=new FormEmailMessageEdit(message,AddressCur,true,_listInboxEmails,_listSentEmails);
      FormE.IsNew=true;
      FormE.FormClosed+=FormEmailMessage_Closed; //takes care of updating gridSent
      FormE.Show();
    }

    private void butReply_Click(object sender,EventArgs e) {
			ReplyClickHelper();
    }
		
		private void butReplyAll_Click(object sender,EventArgs e) {
			ReplyClickHelper(true);
		}

		private void ReplyClickHelper(bool isReplyAll=false) {
      if(!Security.IsAuthorized(Permissions.EmailSend)) {
        return;
      }
      if(gridInbox.GetSelectedIndex()==-1) {
        MsgBox.Show(this,"You must select an email before replying.");
        return;
      }
      EmailMessage selectedMessage=EmailMessages.GetOne(((EmailMessage)gridInbox
        .Rows[gridInbox.GetSelectedIndex()].Tag).EmailMessageNum);//Refresh from the database to get the full body text.
      FormEmailMessageEdit FormE=new FormEmailMessageEdit(EmailMessages.CreateReply(selectedMessage,AddressCur,isReplyAll),AddressCur,true,_listInboxEmails,_listSentEmails);
      FormE.IsNew=true;
      FormE.FormClosed+=FormEmailMessage_Closed;//takes care of updating the SentMessages grid.
      FormE.Show();
		}
		
		private void butForward_Click(object sender,EventArgs e) {
      if(!Security.IsAuthorized(Permissions.EmailSend)) {
        return;
      }
      if(gridInbox.GetSelectedIndex()==-1) {
        MsgBox.Show(this,"You must select an email to forward.");
        return;
      }
      EmailMessage selectedMessage=EmailMessages.GetOne(((EmailMessage)gridInbox
					.Rows[gridInbox.GetSelectedIndex()].Tag).EmailMessageNum);//Refresh from the database to get the full body text.
      FormEmailMessageEdit FormE=new FormEmailMessageEdit(EmailMessages.CreateForward(selectedMessage,AddressCur),AddressCur,true,_listInboxEmails,_listSentEmails);
      FormE.IsNew=true;
      FormE.FormClosed+=FormEmailMessage_Closed;//takes care of updating the SentMessages grid.
      FormE.Show();
		}
		
    private void tabControl1_SelectedIndexChanged(object sender,EventArgs e) {
      Cursor=Cursors.WaitCursor;
      SetButtonsEnabled();
      FillInboxOrSent();
      Cursor=Cursors.Default;
    }

    private void comboEmailAddress_SelectionChangeCommitted(object sender,EventArgs e) {
      Text="Email Inbox for "+AddressCur.EmailUsername;
      if(AddressCur.EmailUsername==""||AddressCur.Pop3ServerIncoming=="") {//Email address not setup.
        Text="Email Inbox - The currently selected email address is not setup to receive email.";
      }
      if(comboEmailAddress.SelectedIndex==0&&Security.CurUser.ProvNum!=0) { //WebMail is selected
        Text="Email Inbox for "+AddressCur.EmailUsername;
      }
      Cursor=Cursors.WaitCursor;
      RefreshLists();
      if(ActiveMailbox==MailboxType.Inbox) {
				try {
					GetMessages();
				}
				catch(ODException ex) {
					if(ex.ErrorCodeAsEnum == ODException.ErrorCodes.FormClosed) {
						return;
					}
					throw ex;
				}
			}
      else {
        FillGridSent();
      }
      Cursor=Cursors.Default;
    }

	///<summary>If someone else is sending emails on another workstation, this will update this form to reflect that.</summary>
	public override void OnProcessSignals(List<Signalod> listSignals) {
		if(listSignals.Exists(x => x.IType==InvalidType.Email)) {
			Cursor=Cursors.WaitCursor;
			//an address may have changed. refill the combobox
			FillComboEmail();
		}
		if(listSignals.Exists(x => x.IType==InvalidType.EmailMessages)) {
			Cursor=Cursors.WaitCursor;
			_isRefreshInbox=true;
			_isRefreshSent=true;
			FillGridInbox();
			FillGridSent();
		}
		Cursor=Cursors.Default;
	}

		///<summary>Refreshes Inbox or Sent depending on currently active mailbox</summary>
		private void FillInboxOrSent(bool isRefreshNeeded=false) {
			//Refresh mailbox with/without Hidden emails
			_isRefreshSent=isRefreshNeeded;
			if(ActiveMailbox==MailboxType.Inbox) {
				FillGridInbox();
			}
			else if(ActiveMailbox==MailboxType.Sent) {
				FillGridSent();
			}
		}

		///<summary>Builds a HideInFlags flag from listShowIn</summary>
		private HideInFlags GetHideInFlagsFromListBox() {
			HideInFlags flags=HideInFlags.None;
			foreach(int i in listShowIn.SelectedIndices) {
				flags|=_listHideInFlags[i];
			}
			return (HideInFlags)((HideInFlags)_listHideInFlags.Sum(x => (uint)x)-flags); //UI and backend logic are flipped (show vs hide).
		}

		///<summary>Causes ActiveMailbox to refill based on checkShowHiddenEmails status.</summary>
		private void checkShowHiddenEmails_CheckedChanged(object sender,EventArgs e) {
			FillInboxOrSent();
		}

		///<summary>Updates email objects,DB, and UI in response to HideInFlags setting selections.</summary>
		private void listHideInFlags_MouseClick(object sender,MouseEventArgs e) {
			bool isGridRefreshRequired=false;
			EmailMessage emailMessage;
			HideInFlags flags=GetHideInFlagsFromListBox();
			foreach(ODGridRow row in ActiveGrid.SelectedGridRows) {
				emailMessage=(EmailMessage)row.Tag;
				if(emailMessage.HideIn==flags) {
						continue;
				}
				if(flags.HasFlag(HideInFlags.EmailInbox)) {
					isGridRefreshRequired=true;
				}
				EmailMessage emailOld=emailMessage.Copy();
				emailMessage.HideIn=flags;
				EmailMessages.Update(emailMessage,emailOld,false);
			}
			Signalods.SetInvalid(InvalidType.EmailMessages);
			if(isGridRefreshRequired) {
				FillInboxOrSent();
			}
			else {
				RefreshShowIn();
			}
		}

    private void butDelete_Click(object sender,EventArgs e) {
      if(ActiveGrid.SelectedIndices.Length==0) {
        MsgBox.Show(this,"Please select email to delete or hide.");
        return;
      }
      if(!MsgBox.Show(this,true,"Permanently delete or hide selected email(s)?")) {
        return;
      }
      Cursor=Cursors.WaitCursor;
      for(int i=0;i<ActiveGrid.SelectedIndices.Length;i++) {
        EmailMessage emailMessage=(EmailMessage)ActiveGrid.Rows[ActiveGrid.SelectedIndices[i]].Tag;
        //If attached to a patient, simply hide the email message instead of deleting it so that it still shows in other parts of the program.
        if(emailMessage.PatNum!=0) {
          emailMessage.HideIn=(HideInFlags)_listHideInFlags.Sum(x => (int)x);
          EmailMessages.Update(emailMessage);
          continue;
        }
        if(EmailMessages.IsSecureWebMail(emailMessage.SentOrReceived)) {
          EmailMessages.Delete(emailMessage);
          string logText="";
          logText+="\r\n"+Lan.g(this,"From")+": "+emailMessage.FromAddress+". ";
          logText+="\r\n"+Lan.g(this,"To")+": "+emailMessage.ToAddress+". ";
          if(!String.IsNullOrEmpty(emailMessage.Subject)) {
            logText+="\r\n"+Lan.g(this,"Subject")+": "+emailMessage.Subject+". ";
          }
          if(!String.IsNullOrEmpty(emailMessage.BodyText)) {
            if(emailMessage.BodyText.Length > 50) {
              logText+="\r\n"+Lan.g(this,"Body Text")+": "+emailMessage.BodyText.Substring(0,49)+"... ";
            }
            else {
              logText+="\r\n"+Lan.g(this,"Body Text")+": "+emailMessage.BodyText;
            }
          }
          if(emailMessage.MsgDateTime != DateTime.MinValue) {
            logText+="\r\n"+Lan.g(this,"Date")+": "+emailMessage.MsgDateTime.ToShortDateString()+". ";
          }
          SecurityLogs.MakeLogEntry(Permissions.WebMailDelete,emailMessage.PatNum,Lan.g(this,"Webmail deleted.")+" "+logText);
        }
        else {//Not a web mail message.
          EmailMessages.Delete(emailMessage);
        }
      }
      if(ActiveMailbox==MailboxType.Inbox) {
        _isRefreshInbox=true;
        FillGridInbox();
      }
      else {
        _isRefreshSent=true;
        FillGridSent();
      }
      Signalods.SetInvalid(InvalidType.EmailMessages);
      Cursor=Cursors.Default;
		}

    ///<summary>Since FormEmailMessageEdit is now modeless, this registers for events that 
    ///would cause changes to what the current form needs to display so they can be shows immediately.</summary>
    private void FormEmailMessage_Closed(object sender,EventArgs e) {
      if(sender.GetType()!=typeof(FormEmailMessageEdit)) {
        return; //this should never happen
      }
      if(!((FormEmailMessageEdit)sender).HasEmailChanged) {
        return;
      }
      Cursor=Cursors.WaitCursor;
			FillInboxOrSent(true);
      Cursor=Cursors.Default;
    }

    private void butClose_Click(object sender,EventArgs e) {
      Close();
    }

		private void FormEmailInbox_FormClosing(object sender,FormClosingEventArgs e) {
			_hasClosed=true;
		}
	}
}