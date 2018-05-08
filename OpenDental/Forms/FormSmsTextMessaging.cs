using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormSmsTextMessaging:ODForm {

		#region Private data; properties/fields
		///<summary>List of Clinics associated with ComboClinic.  May contain an entry for clinicnum=0.  ComboClinic indexes match 1to1.</summary>
		private List<Clinic> _listClinics=null;
		///<summary>Set from FormOpenDental.  This can be null if the calling code does not wish to get dynamic unread message counts.
		///Allows FormSmsTextMessaging to update the unread SMS text message count in FormOpenDental as the user reads their messages.</summary>
		private Action _smsNotifier=null;
		///<summary>The selected patNum.  Can be 0 to include all.</summary>
		private long _patNumCur=0;
		///<summary>The column index of the Status column within the Messages grid.
		///This is a class-wide variable to prevent bugs if we decide to change the column order of the Messages grid.</summary>
		private int _columnStatusIdx=0;
		private UserOdPref _groupByPref=null;
		private List<SmsFromMobile> _listSmsFromMobile=new List<SmsFromMobile>();
		private List<SmsToMobile> _listSmsToMobile=new List<SmsToMobile>();
		private Color _colorSelect=Color.FromArgb(224,243,255);
		private Dictionary<long,string> _dictPatNames=new Dictionary<long, string>();
		private DateTime _dateFrom {
			get {
				return PIn.Date(textDateFrom.Text);
			}
		}
		private DateTime _dateTo {
			get {
				return PIn.Date(textDateTo.Text);
			}
		}
		private bool _isGroupByPatient {
			get {
				return radioGroupByPatient.Checked;
			}
		}
		///<summary>Null if gridMessage current selected row tag is not TextMessageGrouped.</summary>
		private TextMessageGrouped _selectedSmsGroup {
			get {
				if(gridMessages.GetSelectedIndex()==-1) {
					return null;
				}
				object selectedTag=gridMessages.Rows[gridMessages.GetSelectedIndex()].Tag;
				if(!(selectedTag is TextMessageGrouped)) {
					return null;
				}
				return (TextMessageGrouped)selectedTag;
			}
		}
		///<summary>Null if gridMessage current selected row tag is not SmsFromMobile.</summary>
		private SmsFromMobile _selectedSmsFromMobile {
			get {
				if(gridMessages.GetSelectedIndex()==-1) {
					return null;
				}
				object selectedTag=gridMessages.Rows[gridMessages.GetSelectedIndex()].Tag;
				if(!(selectedTag is SmsFromMobile)) {
					return null;
				}
				return (SmsFromMobile)selectedTag;
			}
		}
		///<summary>Null if gridMessage current selected row tag is not SmsToMobile.</summary>
		private SmsToMobile _selectedSmsToMobile {
			get {
				if(gridMessages.GetSelectedIndex()==-1) {
					return null;
				}
				object selectedTag=gridMessages.Rows[gridMessages.GetSelectedIndex()].Tag;
				if(!(selectedTag is SmsToMobile)) {
					return null;
				}
				return (SmsToMobile)selectedTag;
			}
		}
		private bool _hasSelectedMessage {
			get {
				return _selectedSmsGroup!=null || _selectedSmsFromMobile!=null || _selectedSmsToMobile!=null;
			}
		}
		///<summary>0 if gridMessage current selected row tag is not SmsToMobile or SmsFromMobile or TextMessageGrouped.</summary>
		private long _selectedPatNum {
			get {
				if(_selectedSmsGroup!=null) {
					return _selectedSmsGroup.PatNum;
				}
				if(_selectedSmsFromMobile!=null) {
					return _selectedSmsFromMobile.PatNum;
				}
				if(_selectedSmsToMobile!=null) {
					return _selectedSmsToMobile.PatNum;
				}
				return 0;
			}
		}
		///<summary>Empty if gridMessage current selected row tag is not SmsToMobile or SmsFromMobile or TextMessageGrouped.</summary>
		private string _selectedMobileNumber {
			get {
				if(_selectedSmsGroup!=null) {
					return _selectedSmsGroup.PatPhone;
				}
				if(_selectedSmsFromMobile!=null) {
					return _selectedSmsFromMobile.MobilePhoneNumber;
				}
				if(_selectedSmsToMobile!=null) {
					return _selectedSmsToMobile.MobilePhoneNumber;
				}
				return "";
			}
		}
		//Leaving this blank will cause the clinic filter to be ignored in SmsFromMobiles.GetMessages().
		private List<long> _listClinicNumsSelected {
			get {
				if(!PrefC.HasClinicsEnabled) {
					return new List<long>();
				}
				List<long> ret=new List<long>();
				for(int i=0;i<comboClinic.SelectedIndices.Count;i++) {
					int index=(int)comboClinic.SelectedIndices[i];
					ret.Add(_listClinics[index].ClinicNum);
				}
				if(ret.Contains(0)) { //If 0-clinic is select, just select all.
					ret=new List<long>(_listClinics.Select(x => x.ClinicNum));
				}
				return ret;
			}
		}
		///<summary>Always includes ReceivedUnread. May include ReceivedRead.</summary>
		private List<SmsFromStatus> _listSmsFromStatusesSelected {
			get {
				List<SmsFromStatus> ret=new List<SmsFromStatus>();
				ret.Add(SmsFromStatus.ReceivedUnread);
				if(checkRead.Checked) {
					ret.Add(SmsFromStatus.ReceivedRead);
				}
				return ret;
			}
		}
		///<summary>Passively load _dictPatNames from db if entry is not already found.</summary>
		private string GetPatientName(long patNum) {
			AddPatientNames(new List<long>() { patNum });
			return _dictPatNames.ContainsKey(patNum) ? _dictPatNames[patNum] : Lan.g(this,"Not found");
		}
		///<summary>Safe to call this for any number of PatNums. Will only go to db if given PatNum(s) not already found in _dictPatNames.</summary>
		private void AddPatientNames(List<long> listPatNums) {
			var newPatNums=listPatNums.Where(x => x!=0 && !_dictPatNames.ContainsKey(x)).ToList();
			if(newPatNums.Count()>0) {
				var newNames=Patients.GetPatientNames(newPatNums);
				foreach(var x in newNames) {
					_dictPatNames[x.Key]=x.Value;
				}
			}
		}
		#endregion

		#region Init
		public FormSmsTextMessaging(bool isSent,bool isReceived,Action smsNotifier) {
			InitializeComponent();
			Lan.F(this);
			checkSent.Checked=isSent;
			checkRead.Checked=isReceived;
			_smsNotifier=smsNotifier;
			_groupByPref=UserOdPrefs.GetFirstOrNewByUserAndFkeyType(Security.CurUser.UserNum,UserOdFkeyType.SmsGroupBy);
			if(_groupByPref.ValueString=="1") {
				radioGroupByPatient.Checked=true;
			}
			else {
				radioGroupByNone.Checked=true;
			}
		}

		private void FormSmsTextMessaging_Load(object sender,EventArgs e) {
			gridMessages.ContextMenu=contextMenuMessages;
			if(PrefC.HasClinicsEnabled) {
				labelClinic.Visible=true;
				comboClinic.Visible=true;
				comboClinic.Items.Clear();
				_listClinics=Clinics.GetForUserod(Security.CurUser);
				if(!Security.CurUser.ClinicIsRestricted || Security.CurUser.ClinicNum==0) {
					Clinic hqClinic=Clinics.GetPracticeAsClinicZero();
					hqClinic.Abbr=(PrefC.GetString(PrefName.PracticeTitle)+" ("+Lan.g(this,"Practice")+")");
					_listClinics.Insert(0,hqClinic);//Add HQ
				}
				for(int i=0;i<_listClinics.Count;i++) {
					comboClinic.Items.Add(_listClinics[i].Abbr);
					if(_listClinics[i].ClinicNum==Clinics.ClinicNum) {
						//Selected clinlic by default.
						comboClinic.SetSelected(i,true);
					}
				}
				if(comboClinic.SelectedIndices.Count==0&&comboClinic.Items.Count>0) {
					comboClinic.SetSelected(0,true);
				}
				if(Clinics.ClinicNum==0) { //HQ clinic is selected so select all clinics in the filter.
					comboClinic.SetSelected(true);
				}
			}
			textDateFrom.Text=DateTimeOD.Today.AddDays(-7).ToShortDateString();
			textDateTo.Text=DateTimeOD.Today.ToShortDateString();			
			FillGridTextMessages();
			//It is important that these events get registered after we have filled the form. Don't want them to fire twice when setting the initial values.
			radioGroupByNone.CheckedChanged+=new EventHandler(RadioGroupBy_CheckedChanged);
			radioGroupByPatient.CheckedChanged+=new EventHandler(RadioGroupBy_CheckedChanged);
			this.FormClosing+=new FormClosingEventHandler((o,e1) => {
				_groupByPref.ValueString=radioGroupByPatient.Checked ? "1" : "0";
				UserOdPrefs.Upsert(_groupByPref);
			});
			Plugins.HookAddCode(this,"FormSmsTextMessaging.Load_end");
		}

		private void RadioGroupBy_CheckedChanged(object sender,EventArgs e) {
			if(sender==null || !(sender is RadioButton) || !((RadioButton)sender).Checked) {
				return;
			}
			FillGridTextMessages(true,false);
		}
		#endregion

		#region Fill Grids
		private void FillGridTextMessages(bool isRedrawOnly=false,bool retainSort=true) {
			//Show/hide context menu items according to the grouping mode.
			new List<MenuItem>() {
				menuItemChangePat,
				menuItemMarkUnread,
				menuItemMarkRead,
				menuItemHide,
				menuItemUnhide,
				menuItemBlockNumber
			}
			.ForEach(x => x.Visible=!_isGroupByPatient);
			if(PrefC.HasClinicsEnabled && comboClinic.SelectedIndices.Count==0) {
				gridMessages.BeginUpdate();
				gridMessages.Rows.Clear();
				gridMessages.EndUpdate();
				return;
			}
			//Hold these. We will be clearing them below and they will need to be restored.
			int sortByColIdx=gridMessages.SortedByColumnIdx;
			bool isSortAsc=gridMessages.SortedIsAscending;
			if(sortByColIdx==-1 || !retainSort) {
				sortByColIdx=0;
				isSortAsc=false;
			}
			if(!isRedrawOnly) {
				_listSmsFromMobile=SmsFromMobiles.GetMessages(_dateFrom,_dateTo,_listClinicNumsSelected,_patNumCur,false,_listSmsFromStatusesSelected.ToArray());
				if(checkSent.Checked) {
					_listSmsToMobile=SmsToMobiles.GetMessages(_dateFrom,_dateTo,_listClinicNumsSelected,_patNumCur);
				}
				AddPatientNames(_listSmsFromMobile.GroupBy(x => x.PatNum).Select(x => x.Key)
					.Union(_listSmsToMobile.GroupBy(x => x.PatNum).Select(x => x.Key)).ToList());
			}
			if(_isGroupByPatient) {
				FillGridTextMessagesGroupedYes(sortByColIdx,isSortAsc,_selectedPatNum,_selectedSmsGroup);
			}
			else {
				FillGridTextMessagesGroupedNo(sortByColIdx,isSortAsc,_selectedPatNum,_selectedSmsToMobile,_selectedSmsFromMobile);
			}
		}

		private void FillGridTextMessagesGroupedNo(int sortByColIdx,bool isSortAsc,long selectedPatNum,SmsToMobile selectedSmsToMobile,SmsFromMobile selectedSmsFromMobile) {
			gridMessages.Title=Lan.g(this,"Text Messages - Right click for options - Unread messages always shown");
			gridMessages.BeginUpdate();
			gridMessages.Rows.Clear();
			gridMessages.Columns.Clear();
			gridMessages.Columns.Add(new UI.ODGridColumn("DateTime",140,HorizontalAlignment.Left) { SortingStrategy=UI.GridSortingStrategy.DateParse });
			gridMessages.Columns.Add(new UI.ODGridColumn("Sent /\r\nReceived",80,HorizontalAlignment.Center){SortingStrategy=UI.GridSortingStrategy.StringCompare} );
			gridMessages.Columns.Add(new UI.ODGridColumn("Status",70,HorizontalAlignment.Center) { SortingStrategy=UI.GridSortingStrategy.StringCompare });
			_columnStatusIdx=gridMessages.Columns.Count-1;
			gridMessages.Columns.Add(new UI.ODGridColumn("#Phone\r\nMatches",60,HorizontalAlignment.Center) { SortingStrategy=UI.GridSortingStrategy.AmountParse });
			gridMessages.Columns.Add(new UI.ODGridColumn("Patient\r\nPhone",100,HorizontalAlignment.Center) { SortingStrategy=UI.GridSortingStrategy.StringCompare });
			gridMessages.Columns.Add(new UI.ODGridColumn("Patient",150,HorizontalAlignment.Left) { SortingStrategy=UI.GridSortingStrategy.StringCompare });
			gridMessages.Columns.Add(new UI.ODGridColumn("Cost",32,HorizontalAlignment.Right) { SortingStrategy=UI.GridSortingStrategy.AmountParse });
			if(PrefC.HasClinicsEnabled) {
				gridMessages.Columns.Add(new UI.ODGridColumn("Clinic",130,HorizontalAlignment.Left) { SortingStrategy=UI.GridSortingStrategy.StringCompare });
			}
			if(checkHidden.Checked) {
				gridMessages.Columns.Add(new UI.ODGridColumn("Hidden",46,HorizontalAlignment.Center){SortingStrategy=UI.GridSortingStrategy.StringCompare});
			}
			foreach(SmsFromMobile smsFromMobile in _listSmsFromMobile) {
				if(!checkHidden.Checked && smsFromMobile.IsHidden) {
					continue;
				}
				UI.ODGridRow row=new UI.ODGridRow();
				row.Tag=smsFromMobile;
				if(smsFromMobile.SmsStatus==SmsFromStatus.ReceivedUnread) {
					row.Bold=true;
				}
				row.Cells.Add(smsFromMobile.DateTimeReceived.ToString());//DateTime
				row.Cells.Add(Lan.g(this,"Received"));//Type
				row.Cells.Add(SmsFromMobiles.GetSmsFromStatusDescript(smsFromMobile.SmsStatus));//Status
				row.Cells.Add(smsFromMobile.MatchCount.ToString());//#Phone Matches
				row.Cells.Add(smsFromMobile.MobilePhoneNumber);//Patient Phone
				row.Cells.Add(smsFromMobile.PatNum==0 ? Lan.g(this,"Unassigned") : GetPatientName(smsFromMobile.PatNum));//Patient
				row.Cells.Add("0.00");//Cost
				if(PrefC.HasClinicsEnabled) {
					if(smsFromMobile.ClinicNum==0) {
						row.Cells.Add(PrefC.GetString(PrefName.PracticeTitle)+" ("+Lan.g(this,"Practice")+")");
					}
					else { 
						Clinic clinic=Clinics.GetClinic(smsFromMobile.ClinicNum);
						row.Cells.Add(clinic.Abbr);//Clinic
					}
				}
				if(checkHidden.Checked) {
					row.Cells.Add(smsFromMobile.IsHidden?"X":"");//Hidden
				}
				if(selectedPatNum!=0 && smsFromMobile.PatNum==selectedPatNum) {
					row.ColorBackG=_colorSelect;
				}
				gridMessages.Rows.Add(row);
			}
			if(checkSent.Checked) {
				foreach(SmsToMobile smsToMobile in _listSmsToMobile) {
					if(!checkHidden.Checked && smsToMobile.IsHidden) {
						continue;
					}
					UI.ODGridRow row=new UI.ODGridRow();
					row.Tag=smsToMobile;
					row.Cells.Add(smsToMobile.DateTimeSent.ToString());//DateTime
					row.Cells.Add(Lan.g(this,"Sent"));//Type
					string smsStatus=smsToMobile.SmsStatus.ToString(); //Default to the actual status.
					switch(smsToMobile.SmsStatus) {
						case SmsDeliveryStatus.DeliveryConf:
						case SmsDeliveryStatus.DeliveryUnconf:
							//Treated the same as far as the user is concerned.
							smsStatus=Lan.g(this,"Delivered");
							break;
						case SmsDeliveryStatus.FailWithCharge:
						case SmsDeliveryStatus.FailNoCharge:
							//Treated the same as far as the user is concerned.
							smsStatus=Lan.g(this,"Failed");
							break;
					}
					row.Cells.Add(smsStatus);//Status
					row.Cells.Add("-");//#Phone Matches, not applicable to outbound messages.
					row.Cells.Add(smsToMobile.MobilePhoneNumber);//Patient Phone
					row.Cells.Add(smsToMobile.PatNum==0 ? "" : GetPatientName(smsToMobile.PatNum));//Patient
					row.Cells.Add(smsToMobile.MsgChargeUSD.ToString("f"));//Cost
					if(PrefC.HasClinicsEnabled) {
						if(smsToMobile.ClinicNum==0) {
							row.Cells.Add(PrefC.GetString(PrefName.PracticeTitle)+" ("+Lan.g(this,"Practice")+")");
						}
						else { 
							Clinic clinic=Clinics.GetClinic(smsToMobile.ClinicNum);
							row.Cells.Add(clinic.Abbr);//Clinic
						}
					}
					if(checkHidden.Checked) {
						row.Cells.Add(smsToMobile.IsHidden?"X":"");//Hidden
					}
					if(selectedPatNum!=0 && smsToMobile.PatNum==selectedPatNum) {
						row.ColorBackG=_colorSelect;
					}
					gridMessages.Rows.Add(row);
				}
			}
			gridMessages.EndUpdate();
			gridMessages.SortForced(sortByColIdx,isSortAsc);
			//Check new grid rows against previous selection and re-select.			
			for(int i=0;i<gridMessages.Rows.Count;i++) { 
				if(gridMessages.Rows[i].Tag is SmsFromMobile && selectedSmsFromMobile!=null
					&& ((SmsFromMobile)gridMessages.Rows[i].Tag).SmsFromMobileNum==selectedSmsFromMobile.SmsFromMobileNum) 
				{
					gridMessages.SetSelected(i,true);
					break;
				}
				if(gridMessages.Rows[i].Tag is SmsToMobile && selectedSmsToMobile!=null
					&& ((SmsToMobile)gridMessages.Rows[i].Tag).SmsToMobileNum==selectedSmsToMobile.SmsToMobileNum) 
				{
					gridMessages.SetSelected(i,true);
					break;
				}
			}
			FillGridMessageThread();
		}

		string GetDeliverStatus(SmsDeliveryStatus smsStatus) {
			switch(smsStatus) {
				case SmsDeliveryStatus.DeliveryConf:
				case SmsDeliveryStatus.DeliveryUnconf:
					//Treated the same as far as the user is concerned.
					return Lan.g(this,"Delivered");
				case SmsDeliveryStatus.FailWithCharge:
				case SmsDeliveryStatus.FailNoCharge:
					//Treated the same as far as the user is concerned.
					return Lan.g(this,"Failed");
			}
			//Default to the actual status.
			return smsStatus.ToString();
		}

		private void FillGridTextMessagesGroupedYes(int sortByColIdx,bool isSortAsc,long selectedPatNum,TextMessageGrouped selectedSmsGroup) {
			gridMessages.Title=Lan.g(this,"Text Messages - Grouped by patient");
			gridMessages.BeginUpdate();
			gridMessages.Rows.Clear();
			gridMessages.Columns.Clear();
			gridMessages.Columns.Add(new UI.ODGridColumn("DateTime",140,HorizontalAlignment.Left) { SortingStrategy=UI.GridSortingStrategy.DateParse });
			gridMessages.Columns.Add(new UI.ODGridColumn("Status",100,HorizontalAlignment.Left) { SortingStrategy=UI.GridSortingStrategy.StringCompare });
			_columnStatusIdx=gridMessages.Columns.Count-1;
			gridMessages.Columns.Add(new UI.ODGridColumn("Patient\r\nPhone",100,HorizontalAlignment.Center) { SortingStrategy=UI.GridSortingStrategy.StringCompare });
			gridMessages.Columns.Add(new UI.ODGridColumn("Patient",150,HorizontalAlignment.Left) { SortingStrategy=UI.GridSortingStrategy.StringCompare });
			if(PrefC.HasClinicsEnabled) {
				gridMessages.Columns.Add(new UI.ODGridColumn("Clinic",130,HorizontalAlignment.Left) { SortingStrategy=UI.GridSortingStrategy.StringCompare });
			}
			gridMessages.Columns.Add(new UI.ODGridColumn("Latest Message",150,HorizontalAlignment.Left) { SortingStrategy=UI.GridSortingStrategy.StringCompare });
			List<TextMessageGrouped> groupToMobile=new List<TextMessageGrouped>();
			List<TextMessageGrouped> groupFromMobile=_listSmsFromMobile
				.Where(x => (x.IsHidden && checkHidden.Checked) || !x.IsHidden)
				.OrderByDescending(x => x.DateTimeReceived)
				.GroupBy(x => new { x.PatNum, x.MobilePhoneNumber })
				.Select(x => new TextMessageGrouped() {
					DateTimeMostRecent=x.First().DateTimeReceived,
					PatPhone=x.First().MobilePhoneNumber,
					PatNum=x.First().PatNum,
					ClinicNum=x.First().ClinicNum,
					ClinicAbbr=PrefC.HasClinicsEnabled ? (x.First().ClinicNum==0 ? PrefC.GetString(PrefName.PracticeTitle)+" ("+Lan.g(this,"Practice")+")" : Clinics.GetClinic(x.First().ClinicNum).Abbr) : "",
					PatName=x.First().PatNum==0 ? Lan.g(this,"Unassigned") : GetPatientName(x.First().PatNum),
					TextMsg=x.First().MsgText,
					HasUnread=x.Any(y => y.SmsStatus==SmsFromStatus.ReceivedUnread),
					Status=Lan.g(this,"Rcv")+" - "+SmsFromMobiles.GetSmsFromStatusDescript(x.First().SmsStatus),
					ListToMobile=new List<SmsToMobile>(),
					ListFromMobile=x.ToList(),
				}).ToList();
			if(checkSent.Checked) {
				groupToMobile=_listSmsToMobile
					.Where(x => (x.IsHidden && checkHidden.Checked) || !x.IsHidden)
					.OrderByDescending(x => x.DateTimeSent)
					.GroupBy(x => new { x.PatNum,x.MobilePhoneNumber })
					.Select(x => new TextMessageGrouped() {
						DateTimeMostRecent=x.First().DateTimeSent,
						PatPhone=x.First().MobilePhoneNumber,
						PatNum=x.First().PatNum,
						ClinicNum=x.First().ClinicNum,
						ClinicAbbr=PrefC.HasClinicsEnabled ? (x.First().ClinicNum==0 ? PrefC.GetString(PrefName.PracticeTitle)+" ("+Lan.g(this,"Practice")+")" : Clinics.GetClinic(x.First().ClinicNum).Abbr) : "",
						PatName=x.First().PatNum==0 ? Lan.g(this,"Unassigned") : GetPatientName(x.First().PatNum),
						TextMsg=x.First().MsgText,
						HasUnread=false,
						Status=Lan.g(this,"Sent")+" - "+GetDeliverStatus(x.First().SmsStatus),
						ListFromMobile=new List<SmsFromMobile>(),
						ListToMobile=x.ToList(),
					}).ToList();
			}			
			var groupAll=groupFromMobile.Union(groupToMobile)
				.OrderByDescending(x => x.DateTimeMostRecent)
				.GroupBy(x => new { x.PatNum, x.PatPhone })
				.Select(x => new {
					//First entry from the union.
					Group=x.First(),
					HasUnread=x.Any(y => y.HasUnread),
					//All messages from the entire group make up the lists.
					ListToMobile=x.SelectMany(y => y.ListToMobile).ToList(),
					ListFromMobile=x.SelectMany(y => y.ListFromMobile).ToList()
				}).ToList();
			foreach(var group in groupAll) {				
				UI.ODGridRow row=new UI.ODGridRow();
				//Patch the list back together before saving it to the tag. This tag will be used later to change the SmsStatus of selected grid row's messages.
				TextMessageGrouped sms=group.Group;
				sms.ListFromMobile=group.ListFromMobile;
				sms.ListToMobile=group.ListToMobile;
				sms.HasUnread=group.HasUnread;
				row.Tag=sms;
				if(sms.HasUnread) {
					row.Bold=true;
				}
				row.Cells.Add(sms.DateTimeMostRecent.ToString());
				row.Cells.Add(sms.Status);
				row.Cells.Add(sms.PatPhone);//Patient Phone
				row.Cells.Add(sms.PatName);
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(sms.ClinicAbbr);
				}
				row.Cells.Add(sms.TextMsg);
				gridMessages.Rows.Add(row);
			}
			gridMessages.EndUpdate();
			gridMessages.SortForced(sortByColIdx,isSortAsc);
			//Check new grid rows against previous selection and re-select.			
			for(int i = 0;i<gridMessages.Rows.Count;i++) {
				if(
					gridMessages.Rows[i].Tag is TextMessageGrouped && 
					selectedSmsGroup!=null &&
					((TextMessageGrouped)gridMessages.Rows[i].Tag).PatPhone==selectedSmsGroup.PatPhone &&
					((TextMessageGrouped)gridMessages.Rows[i].Tag).PatNum==selectedSmsGroup.PatNum)
				{
					gridMessages.SetSelected(i,true);
					break;
				}
			}
			FillGridMessageThread();
		}

		private void FillGridMessageThread() {
			if(_selectedPatNum==0) {
				Text=Lan.g(this,"Text Messaging");
			}
			else {
				Text=Lan.g(this,"Text Messaging - Patient:")+" "+GetPatientName(_selectedPatNum);
			}
			if(_selectedPatNum==0) { //A message with no patNum was selected or nothing is selected at all.
				if(_selectedSmsToMobile!=null) { //SmsToMobile is selected.
					smsThreadView.ListSmsThreadMessages=new List<SmsThreadMessage>() { new SmsThreadMessage(_selectedSmsToMobile.DateTimeSent,_selectedSmsToMobile.MsgText,true,true,true) };
				}
				else if(_selectedSmsFromMobile!=null) { //SmsFromMobile is selected.
					smsThreadView.ListSmsThreadMessages=new List<SmsThreadMessage>() { new SmsThreadMessage(_selectedSmsFromMobile.DateTimeReceived,_selectedSmsFromMobile.MsgText,true,true,true) };
				}
				else if(_selectedSmsGroup!=null) { //SmsFromMobile is selected.
					smsThreadView.ListSmsThreadMessages=new List<SmsThreadMessage>() { new SmsThreadMessage(_selectedSmsGroup.DateTimeMostRecent,_selectedSmsGroup.TextMsg,true,true,true) };
				}
				else { //Nothing selected at all, clear the message thread.
					smsThreadView.ListSmsThreadMessages=null;
				}
				return;
			}
			List<SmsThreadMessage> listSmsThreadMessages=new List<SmsThreadMessage>();
			List<SmsFromMobile> listSmsFromMobile=SmsFromMobiles.GetMessages(DateTime.MinValue,DateTime.MinValue,_listClinicNumsSelected,_selectedPatNum,true);
			foreach(SmsFromMobile smsFromMobile in listSmsFromMobile) {
				bool isHighlighted=false;
				if(_selectedSmsFromMobile!=null && _selectedSmsFromMobile.SmsFromMobileNum==smsFromMobile.SmsFromMobileNum) {
					isHighlighted=true;
				}
				listSmsThreadMessages.Add(new SmsThreadMessage(smsFromMobile.DateTimeReceived,smsFromMobile.MsgText,true,false,isHighlighted));
			}
			List<SmsToMobile> listSmsToMobile=SmsToMobiles.GetMessages(DateTime.MinValue,DateTime.MinValue,_listClinicNumsSelected,_selectedPatNum);
			foreach(SmsToMobile smsToMobile in listSmsToMobile) {
				bool isHighlighted=false;
				if(_selectedSmsToMobile!=null && _selectedSmsToMobile.SmsToMobileNum==smsToMobile.SmsToMobileNum) {
					isHighlighted=true;
				}
				bool isImportant=false;
				if(smsToMobile.SmsStatus==SmsDeliveryStatus.FailNoCharge ||smsToMobile.SmsStatus==SmsDeliveryStatus.FailWithCharge) {
					isImportant=true;
				}
				listSmsThreadMessages.Add(new SmsThreadMessage(smsToMobile.DateTimeSent,smsToMobile.MsgText,false,isImportant,isHighlighted));
			}
			listSmsThreadMessages.Sort(SmsThreadMessage.CompareMessages);
			smsThreadView.ListSmsThreadMessages=listSmsThreadMessages;
			SetReplyHelper();
		}
		#endregion

		private void SetReplyHelper() {
			if(!_hasSelectedMessage) {
				butSend.Enabled=false;
				textReply.Enabled=false;
				textReply.Text="";
				return;
			}
			butSend.Enabled=true;
			textReply.Enabled=true;
		}
		
		#region Control Event Handlers
		private void butPatCurrent_Click(object sender,EventArgs e) {
			_patNumCur=FormOpenDental.CurPatNum;
			if(_patNumCur==0) {
				textPatient.Text="";
			}
			else {
				textPatient.Text=Patients.GetLim(_patNumCur).GetNameLF();
			}
		}

		private void butPatFind_Click(object sender,EventArgs e) {
			FormPatientSelect FormP=new FormPatientSelect();
			FormP.ShowDialog();
			if(FormP.DialogResult!=DialogResult.OK) {
				return;
			}
			_patNumCur=FormP.SelectedPatNum;
			textPatient.Text=Patients.GetLim(_patNumCur).GetNameLF();
		}

		private void butPatAll_Click(object sender,EventArgs e) {
			_patNumCur=0;
			textPatient.Text="";
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			FillGridTextMessages();
			Cursor=Cursors.Default;
		}

		///<summary>Since the grid is set to SelectionMode of "One", there will always be exactly 1 selection when this event occurs.
		///Initially the grid does not have any selections, but this event will not be called the first time until the first row is selected.
		///Additionally, there is no way for the user to deselect a row when SelectionMode is set to "One", the user can only change the selection.</summary>
		private void gridMessages_OnSelectionCommitted(object sender,EventArgs e) {
			List<SmsFromMobile> listMarkReceivedRead=new List<SmsFromMobile>();
			//If the clicked/selected message was a ReceivedUnread message, then mark the message ReceivedRead in the db as well as the grid.
			if(_selectedSmsFromMobile!=null && _selectedSmsFromMobile.SmsStatus==SmsFromStatus.ReceivedUnread) {
				listMarkReceivedRead.Add(_selectedSmsFromMobile);
			}
			if(_selectedSmsGroup!=null) {
				listMarkReceivedRead.AddRange(_selectedSmsGroup.ListFromMobile.FindAll(x => x.SmsStatus==SmsFromStatus.ReceivedUnread));
			}
			if(listMarkReceivedRead.Count>0) {
				//Messages were marked as 'Read' so update the db and update the grid in place.
				foreach(SmsFromMobile fromMobile in listMarkReceivedRead) {
					SmsFromMobile oldSmsFromMobile=fromMobile.Copy();
					fromMobile.SmsStatus=SmsFromStatus.ReceivedRead;
					SmsFromMobiles.Update(fromMobile,oldSmsFromMobile);
				}
				_smsNotifier?.Invoke();
				//Fix the rows in place. Forcing an entire refresh would cause sorting of grid not to persist.
				gridMessages.Rows[gridMessages.SelectedIndices[0]].Bold=false;
				var tag=gridMessages.Rows[gridMessages.SelectedIndices[0]].Tag;
				if(tag is TextMessageGrouped) {
					TextMessageGrouped smsGroup=(TextMessageGrouped)tag;
					smsGroup.HasUnread=false;
					//Latest entry wins for this column value.
					var latestFromMobile=smsGroup.ListFromMobile.OrderByDescending(x => x.DateTimeReceived).FirstOrDefault()??new SmsFromMobile() { DateTimeReceived=DateTime.MinValue };
					var latestToMobile=smsGroup.ListToMobile.OrderByDescending(x => x.DateTimeSent).FirstOrDefault()??new SmsToMobile() {DateTimeSent=DateTime.MinValue };
					gridMessages.Rows[gridMessages.SelectedIndices[0]].Cells[_columnStatusIdx].Text=
						latestFromMobile.DateTimeReceived>latestToMobile.DateTimeSent ? 
							Lan.g(this,"Rcv")+" - "+SmsFromMobiles.GetSmsFromStatusDescript(SmsFromStatus.ReceivedRead) : 
							Lan.g(this,"Sent")+" - "+GetDeliverStatus(latestToMobile.SmsStatus);
				}
				else if(tag is SmsFromMobile) {
					SmsFromMobile smsFrom=(SmsFromMobile)tag;
					smsFrom.SmsStatus=SmsFromStatus.ReceivedRead;
					gridMessages.Rows[gridMessages.SelectedIndices[0]].Cells[_columnStatusIdx].Text=SmsFromMobiles.GetSmsFromStatusDescript(SmsFromStatus.ReceivedRead);
				}			
			}
			//Update highlighted rows.
			long selectedPatNum=_selectedPatNum;
			gridMessages.Rows.ToList().ForEach(x => {
				long patNum=0;
				if(x.Tag is TextMessageGrouped) {
					patNum=((TextMessageGrouped)x.Tag).PatNum;
				}
				else if(x.Tag is SmsFromMobile) {
					patNum=((SmsFromMobile)x.Tag).PatNum;					
				}
				else if(x.Tag is SmsToMobile) {
					patNum=((SmsToMobile)x.Tag).PatNum;
				}
				x.ColorBackG=patNum!=0 && selectedPatNum==patNum ? _colorSelect : Color.White;
			});
			gridMessages.Invalidate();
			FillGridMessageThread();
		}
		
		///<summary>Sets the given status for the selected receieved message.</summary>
		private void SetReceivedSelectedStatus(SmsFromStatus smsFromStatus) {
			if(_selectedSmsGroup!=null) {
				MsgBox.Show(this,"Please turn off Group By Patient.");
				return;
			}
			if(_selectedSmsFromMobile==null) {
				MsgBox.Show(this,"Please select a received message.");
				return;
			}
			Cursor=Cursors.WaitCursor;			
			UI.ODGridRow row=gridMessages.Rows[gridMessages.GetSelectedIndex()];
			SmsFromMobile smsFromMobile=_selectedSmsFromMobile;
			SmsFromMobile oldSmsFromMobile=smsFromMobile.Copy();
			smsFromMobile.SmsStatus=smsFromStatus;
			SmsFromMobiles.Update(smsFromMobile,oldSmsFromMobile);
			row.Cells[_columnStatusIdx].Text=SmsFromMobiles.GetSmsFromStatusDescript(smsFromStatus);
			row.Bold=false;
			if(smsFromStatus==SmsFromStatus.ReceivedUnread) {
				row.Bold=true;
			}			
			gridMessages.Invalidate();//To show the status changes in the grid.
			_smsNotifier?.Invoke();
			Cursor=Cursors.Default;
		}

		private void menuItemChangePat_Click(object sender,EventArgs e) {
			if(_selectedSmsGroup!=null) {
				MsgBox.Show(this,"Please turn off Group By Patient.");
				return;
			}
			if(_selectedSmsFromMobile==null) {
				MsgBox.Show(this,"Please select a received message.");
				return;
			}
			FormPatientSelect form=new FormPatientSelect();
			form.ExplicitPatNums=SmsFromMobiles.FindPatNums(
				_selectedSmsFromMobile.MobilePhoneNumber,
				//Country code of current environment.
				CultureInfo.CurrentCulture.Name.Substring(CultureInfo.CurrentCulture.Name.Length-2),
				Clinics.ClinicNum==0?null : _listClinicNumsSelected.Union(new List<long>() { _selectedSmsFromMobile.ClinicNum }).ToList())
				//Convert to a list of patnums.
				.Select(x=>x[0]).ToList();
			if(form.ShowDialog()!=DialogResult.OK) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			SmsFromMobile smsFromMobile=_selectedSmsFromMobile;
			SmsFromMobile oldSmsFromMobile=smsFromMobile.Copy();
			smsFromMobile.PatNum=form.SelectedPatNum;
			SmsFromMobiles.Update(smsFromMobile,oldSmsFromMobile);
			//Upsert the Commlog.
			if(smsFromMobile.CommlogNum==0) {
				//When a new sms comes in on the server, a corresponding commlog is inserted, unless the sms could not be matched to a patient by phone #.
				//We need to insert a new commlog when the patient is selected, for the case when the message has not been asssigned to a patient yet.
				//This way we can ensure that all sms with a patient attached have a corresponding commlog.
				Commlog commlog=new Commlog();
				commlog.CommDateTime=smsFromMobile.DateTimeReceived;
				commlog.CommType=Commlogs.GetTypeAuto(CommItemTypeAuto.TEXT);
				commlog.Mode_=CommItemMode.Text;
				commlog.Note=smsFromMobile.MsgText;
				commlog.PatNum=smsFromMobile.PatNum;
				commlog.SentOrReceived=CommSentOrReceived.Received;
				Commlogs.Insert(commlog);
			}
			else {
				Commlog commlog=Commlogs.GetOne(smsFromMobile.CommlogNum);
				Commlog oldCommlog=commlog.Copy();
				commlog.PatNum=form.SelectedPatNum;
				Commlogs.Update(commlog,oldCommlog);
			}			
			FillGridTextMessages();//Refresh grid to show changed patient.
			Cursor=Cursors.Default;
			MsgBox.Show(this,"Text message was moved successfully");
		}

		private void menuItemMarkUnread_Click(object sender,EventArgs e) {
			SetReceivedSelectedStatus(SmsFromStatus.ReceivedUnread);
		}

		private void menuItemMarkRead_Click(object sender,EventArgs e) {
			SetReceivedSelectedStatus(SmsFromStatus.ReceivedRead);
		}

		///<summary>Set isHide to true to hide the selected message.  Set IsHide to false to unhide the selected message.</summary>
		private void HideOrUnhideMessages(bool isHide) {
			if(!_hasSelectedMessage) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			if(_selectedSmsFromMobile!=null) {
				SmsFromMobile oldSmsFromMobile=_selectedSmsFromMobile.Copy();
				_selectedSmsFromMobile.IsHidden=isHide;
				SmsFromMobiles.Update(_selectedSmsFromMobile,oldSmsFromMobile);
			}
			else if(_selectedSmsToMobile!=null) {
				SmsToMobile oldSmsToMobile=_selectedSmsToMobile.Copy();
				_selectedSmsToMobile.IsHidden=isHide;
				SmsToMobiles.Update(_selectedSmsToMobile,oldSmsToMobile);
			}
			Cursor=Cursors.Default;
			FillGridTextMessages();
		}

		private void menuItemHide_Click(object sender,EventArgs e) {
			if(!_hasSelectedMessage) {
				MsgBox.Show(this,"Please select a message before attempting to hide.");
				return;
			}
			if(!MsgBox.Show(this,true,"Hide selected message?")) {
				return;
			}
			HideOrUnhideMessages(true);
		}

		private void menuItemUnhide_Click(object sender,EventArgs e) {
			if(!_hasSelectedMessage) {
				MsgBox.Show(this,"Please select a message before attempting to unhide.");
				return;
			}
			if(!MsgBox.Show(this,true,"Unhide selected message?")) {
				return;
			}
			HideOrUnhideMessages(false);
		}

		private void menuItemBlockNumber_Click(object sender,EventArgs e) {
			if(!_hasSelectedMessage) {
				MsgBox.Show(this,"Please select a received or sent message to block.");
				return;
			}
			string numberToBlock=_selectedMobileNumber;
			string question=Lan.g(this,"Block incoming texts from")+" "+numberToBlock+"? "+Lan.g(this,"This cannot be undone.");
			if(_selectedPatNum!=0) {
				Patient pat=Patients.GetPat(_selectedPatNum);
				if(pat!=null) {
					question+="\r\n"+Lan.g(this,"This phone number is attached to patient")+" "+pat.GetNameFLnoPref()+".";
				}
			}
			if(MessageBox.Show(question,"",MessageBoxButtons.YesNo)==DialogResult.No) {
				return;
			}
			SmsBlockPhones.Insert(new SmsBlockPhone { BlockWirelessNumber=numberToBlock });
			DataValid.SetInvalid(InvalidType.SmsBlockPhones);
		}
				
		private void menuItemSelectPatient_Click(object sender,EventArgs e) {
			if(!_hasSelectedMessage) {
				MsgBox.Show(this,"Please select a message first.");
				return;
			}			
			if(_selectedPatNum==0) {
				MsgBox.Show(this,"Please select a message with a valid patient attached.");
				return;
			}
			FormOpenDental.S_Contr_PatientSelected(Patients.GetPat(_selectedPatNum),true);
		}

		private void butSend_Click(object sender,EventArgs e) {
			if(!_hasSelectedMessage) {
				MsgBox.Show(this,"Please select a message thread to reply to.");
				return;
			}
			if(textReply.Text=="") {
				MsgBox.Show(this,"Please input reply text first.");
				return;
			}
			long clinicNum=0;
			if(_selectedSmsGroup!=null) {
				clinicNum=_selectedSmsGroup.ClinicNum;//can be 0
			}
			else if(_selectedSmsFromMobile!=null) {
				clinicNum=_selectedSmsFromMobile.ClinicNum;//can be 0
			}
			else if(_selectedSmsToMobile!=null) {
				clinicNum=_selectedSmsToMobile.ClinicNum;//can be 0
			}
			if(string.IsNullOrEmpty(_selectedMobileNumber)) {
				//should never happen.
				MsgBox.Show(this,"Selected message does not have a valid phone number to send to.");
				return;
			}
			if(PrefC.HasClinicsEnabled && clinicNum==0) {
				clinicNum=PrefC.GetLong(PrefName.TextingDefaultClinicNum);
				if(clinicNum==0) {
					MsgBox.Show(this,"No default clinic setup for texting.");
					return;
				}
			}
			//Verify that the highlighted blue rows and grey selected row match before allowing the user to send the message.
			//This was happening for the user due to the way ODGrid.CellClick functions.  This issue should now be fixed, but this is a catch-all
			//to ensure that it is impossible for the user to send to the wrong patient due to invalid selections.
			bool isInvalidSelection=false;
			for(int i=0;i<gridMessages.Rows.Count;i++) {
				UI.ODGridRow row=gridMessages.Rows[i];
				if(row.ColorBackG!=_colorSelect) { //Verify that the row the user appears to have selected is actually the selected row.
					continue;
				}
				if(row.Tag is TextMessageGrouped) {
					TextMessageGrouped smsGroup=(TextMessageGrouped)row.Tag;
					if(smsGroup.PatNum!=_selectedPatNum) {
						gridMessages.SetSelected(i,true);//Change the selection to the first highlighted row.
						isInvalidSelection=true;
						break;
					}
				}
				else if(row.Tag is SmsFromMobile) {
					SmsFromMobile smsFrom=(SmsFromMobile)row.Tag;
					if(smsFrom.PatNum!=_selectedPatNum) {
						gridMessages.SetSelected(i,true);//Change the selection to the first highlighted row.
						isInvalidSelection=true;
						break;
					}
				}
				else if(row.Tag is SmsToMobile) {
					SmsToMobile smsTo=(SmsToMobile)row.Tag;
					if(smsTo.PatNum!=_selectedPatNum) {
						gridMessages.SetSelected(i,true);//Change the selection to the first highlighted row.
						isInvalidSelection=true;
						break;
					}
				}
			}
			if(isInvalidSelection) {
				FillGridTextMessages(true);//This ensures that the selected row and highlighted rows are for the same patient as displayed in the grid.
				MsgBox.Show(this,"Message selection was not showing correctly in the grid.  Selections have been refreshed.  "
					+"Please review selected message thread before sending.");
				return;
			}
			try {
				if(Plugins.HookMethod(this,"FormSmsTextMessaging.butReply_Click_sendSmsSingle",_selectedPatNum,_selectedMobileNumber,textReply.Text,YN.Yes)) {
					goto HookSkipSmsCall;
				}
				SmsToMobiles.SendSmsSingle(_selectedPatNum,_selectedMobileNumber,textReply.Text,clinicNum,SmsMessageSource.DirectSms,user: Security.CurUser);
			}
			catch(Exception ex) {
				if(!FormEServicesSetup.ProcessSendSmsException(ex)) {
					MsgBox.Show(this,ex.Message);
				}
				return;
			}
			HookSkipSmsCall: { }
			textReply.Text="";
			FillGridMessageThread();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}
		#endregion

		#region Helper class
		private class TextMessageGrouped {
			public DateTime DateTimeMostRecent;
			public string PatPhone;
			public string PatName;
			public long PatNum;
			public string ClinicAbbr;
			public long ClinicNum;
			public string TextMsg;
			public bool HasUnread=false;
			public string Status;
			public List<SmsFromMobile> ListFromMobile=new List<SmsFromMobile>();
			public List<SmsToMobile> ListToMobile=new List<SmsToMobile>();
		}
		#endregion
		
#if DEBUG
		private void InsertDemoData() {
			bool insertInboud=false;
			bool insertOutbound=false;
			if(!insertInboud&&!insertOutbound) {
				return;
			}
			Random rand=new Random();
			var clinics=Clinics.GetDeepCopy();
			clinics.Add(Clinics.GetPracticeAsClinicZero());
			var vlns=SmsPhones.GetAll();
			var patients=Patients.GetAllPatients()
				.FindAll(x => !string.IsNullOrEmpty(x.HmPhone))
				.GroupBy(x => x.ClinicNum)
				.ToDictionary(x => x.Key,x => x.ToList());
			Action addMissingVLNs=new Action(() => {
				var missingVlns=patients.Keys
					.Select(x => x)
					.Where(x => !vlns.Any(y => y.ClinicNum==x)).ToList();
				foreach(var missingVln in missingVlns) {
					SmsPhones.Insert(new SmsPhone() {
						ClinicNum=missingVln,
						CountryCode="US",
						DateTimeActive=DateTime.Now.Subtract(TimeSpan.FromDays(1)),
						PhoneNumber=rand.Next(0,999999999).ToString("D10"),
					});
				}
				vlns=SmsPhones.GetAll();
			});
			if(insertInboud) {
				List<SmsFromMobile> listInbound=new List<SmsFromMobile>();
				int count=rand.Next(100,200);
				addMissingVLNs();
				for(int i = 0;i<count;i++) {
					string unique=rand.Next(0,1000000).ToString("D7");
					SmsPhone vln=vlns[rand.Next(0,vlns.Count)];
					long clinicNum=vln.ClinicNum;
					if(!patients.ContainsKey(clinicNum)) {
						continue;
					}
					var patsForClinic=patients[clinicNum];
					Patient pat=patsForClinic[rand.Next(0,patsForClinic.Count)];
					string patPhone=pat.HmPhone;
					listInbound.Add(new SmsFromMobile() {
						ClinicNum=clinicNum,
						DateTimeReceived=DateTime.Now.Subtract(TimeSpan.FromMinutes(rand.Next(5,2000))),
						GuidMessage="TEST"+unique,
						MobilePhoneNumber=patPhone,
						MsgPart=1,
						MsgTotal=1,
						MsgRefID="x",
						MsgText="msg - "+unique,
						SmsPhoneNumber=vln.PhoneNumber,
						SmsStatus=SmsFromStatus.ReceivedUnread,
						PatNum=0, //Leave unassigned, we will find the match below
					});
				}
				SmsFromMobiles.ProcessInboundSms(listInbound);
			}
			if(insertOutbound) {
				List<SmsToMobile> listOutbound=new List<SmsToMobile>();
				int count=rand.Next(10,100);
				addMissingVLNs();
				for(int i = 0;i<count;i++) {
					string unique=rand.Next(0,1000000).ToString("D7");
					SmsPhone vln=vlns[rand.Next(0,vlns.Count)];
					long clinicNum=vln.ClinicNum;
					if(!patients.ContainsKey(clinicNum)) {
						continue;
					}
					var patsForClinic=patients[clinicNum];
					Patient pat=patsForClinic[rand.Next(0,patsForClinic.Count)];
					string patPhone=pat.HmPhone;
					DateTime dtSent=DateTime.Now.Subtract(TimeSpan.FromMinutes(rand.Next(5,2000)));
					listOutbound.Add(new SmsToMobile() {
						ClinicNum=clinicNum,
						DateTimeSent=dtSent,
						DateTimeTerminated=dtSent.Add(TimeSpan.FromSeconds(rand.Next(60,240))),
						GuidBatch="TEST"+unique,
						GuidMessage="TEST"+unique,
						IsTimeSensitive=true,
						MobilePhoneNumber=patPhone,
						MsgChargeUSD=.04f,
						MsgParts=1,
						MsgText="outbound - "+unique,
						MsgType=SmsMessageSource.DirectSms,
						PatNum=pat.PatNum,
						SmsPhoneNumber=vln.PhoneNumber,
						SmsStatus=SmsDeliveryStatus.DeliveryConf,
					});
				}
				listOutbound.ForEach(x => SmsToMobiles.Insert(x));
			}
		}
#endif
	}
}