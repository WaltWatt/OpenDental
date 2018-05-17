using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	public partial class FormEtrans835s:ODForm {
	
		///<summary>Start date used to populate _listEtranss.</summary>
		private DateTime _reportDateFrom=DateTime.MaxValue;
		///<summary>End date used to populate _listEtranss.</summary>
		private DateTime _reportDateTo=DateTime.MaxValue;
		///<summary>List of clinics user has access to.</summary>
		private List<Clinic> _listUserClinics;
		///<summary>List of Etrans in date range for etype of EtransType.ERA_835.</summary>
		private List<Etrans> _listEtranss=new List<Etrans>();
		///<summary>Dictionary such that they key is an etrans.EtransNum and value is a list of paid claims associated to it from the database.
		///We allow NULL in our List, this way we know that there was a claim object that can not be found and we use this in determining the status.</summary>
		private Dictionary<long,List<Claim>> _dictEtransClaims=new Dictionary<long, List<Claim>>();
		///<summary>Dictionary such that they key is an etrans.EtransNum and value is a list of ClaimPayment.CheckNums.</summary>
		private Dictionary<long,bool> _dictClaimPayExists=new Dictionary<long,bool>();
		///<summary>Dictionary such that they key is an etrans.EtransNum and value is the 835 object.</summary>
		private Dictionary<long,X835> _dictEtrans835s=new Dictionary<long, X835>();
		/// <summary>List of all Clinics, used to populate the Clinic column in FillGrid().  Prevents getting deep copies of the clinic cache when filling the grid.</summary>
		private List<Clinic> _listClinics;

		public FormEtrans835s() {
			InitializeComponent();
			Lan.F(this);
		}
		
		private void FormEtrans835s_Load(object sender,EventArgs e) {
			textDateFrom.Text=DateTimeOD.Today.AddDays(-7).ToShortDateString();
			textDateTo.Text=DateTimeOD.Today.ToShortDateString();
			#region User Clinics
			if(PrefC.HasClinicsEnabled) {
				comboClinics.Visible=true;
				labelClinic.Visible=true;
				comboClinics.Items.Clear();
				comboClinics.Items.Add(Lan.g(this,"All"));
				comboClinics.Items.Add(Lan.g(this,"Unassigned"));
				_listUserClinics=Clinics.GetForUserod(Security.CurUser);
				_listUserClinics.ForEach(x => comboClinics.Items.Add(x.Abbr));
				comboClinics.SetSelected(0,true);//Defaults to 'All' so that 835s with missing clinic will show.
			}
			#endregion
			#region Statuses
			listStatus.Items.Add(Lans.g(this,"Unprocessed"));//Claims are either not found or not recieved or a combination.
			listStatus.SetSelected(listStatus.Items.Count-1,true);
			listStatus.Items.Add(Lans.g(this,"Partial"));//Some claims found and marked recieved. Others are either not found or not recieved.
			listStatus.SetSelected(listStatus.Items.Count-1,true);
			listStatus.Items.Add(Lans.g(this,"NotFinalized"));//All claims found and marked recieved but no ins payment.
			listStatus.SetSelected(listStatus.Items.Count-1,true);
			listStatus.Items.Add(Lans.g(this,"Finalized"));//All claims found, recieved and all have existing ins payment.
			listStatus.SetSelected(listStatus.Items.Count-1,false);
			#endregion
			_listClinics=Clinics.GetDeepCopy(true);
			RefreshAndFillGrid();//Will not run query, simply initilizes the grid.
			gridMain.AllowSortingByColumn=true;
		}

		///<summary>Called when we want to refresh form list and data. Also calls FillGrid().
		///Set hasFilters to true when we want to refresh and apply current filters.</summary>
		private void RefreshAndFillGrid() {
			_listEtranss=new List<Etrans>();
			if(ValidateFields()) {
				DataTable table=Etranss.RefreshHistory(_reportDateFrom,_reportDateTo,new List<EtransType>() { EtransType.ERA_835 });
				foreach(DataRow row in table.Rows) {
					Etrans etrans=new Etrans();
					etrans.EtransNum=PIn.Long(row["EtransNum"].ToString());
					etrans.ClaimNum=PIn.Long(row["ClaimNum"].ToString());
					etrans.Note=row["Note"].ToString();
					etrans.EtransMessageTextNum=PIn.Long(row["EtransMessageTextNum"].ToString());
					etrans.TranSetId835=row["TranSetId835"].ToString();
					etrans.UserNum=Security.CurUser.UserNum;
					etrans.DateTimeTrans=PIn.DateT(row["dateTimeTrans"].ToString());
					_listEtranss.Add(etrans);
				}
			}
			FilterAndFillGrid(true);
		}

		///<summary>Returns false when either _reportDateFrom or _reportDateTo are invalid.</summary>
		private bool ValidateFields() {
			_reportDateFrom=GetDateFrom();
			_reportDateTo=GetDateTo();
			if(PrefC.HasClinicsEnabled) {
				bool isAllClinics=comboClinics.ListSelectedIndices.Contains(0);
				if(!isAllClinics && comboClinics.SelectedIndices.Count==0) {
					comboClinics.SetSelected(0,true);//All clinics.
				}
			}
			if(_reportDateFrom==DateTime.MinValue || _reportDateTo==DateTime.MinValue) {
				return false;
			}
			return true;
		}
		
		private DateTime GetDateFrom() {
			try {
				return DateTime.Parse(textDateFrom.Text);
			}
			catch{
			}
			return DateTime.MinValue;
		}

		private DateTime GetDateTo() {
			try {
				return DateTime.Parse(textDateTo.Text);
			}
			catch{
			}
			return DateTime.MinValue;
		}

		///<summary>Fills grid based on values in _listEtrans.
		///Set isRefreshNeeded to true when we need to reinitialize local dictionarys after in memory list is also updated. Required true for first time running.
		///Also allows you to passed in predetermined filter options.</summary>
		private void FillGrid(bool isRefreshNeeded,List<string> listSelectedStatuses,List<long> listSelectedClinicNums,
			string carrierName,string checkTraceNum,string amountMin,string amountMax)
		{
			Cursor=Cursors.WaitCursor;
			Action actionCloseProgress=null;
			if(isRefreshNeeded) {
				actionCloseProgress=ODProgressOld.ShowProgressStatus("Etrans835",this,Lan.g(this,"Gathering data")+"...",false);
				_dictEtrans835s.Clear();
				_dictEtransClaims.Clear();
				_dictClaimPayExists.Clear();
				List <Etrans835Attach> listAttached=Etrans835Attaches.GetForEtrans(_listEtranss.Select(x => x.EtransNum).ToArray());
				Dictionary<long,string> dictEtransMessages=new Dictionary<long, string>();
				List<X12ClaimMatch> list835ClaimMatches=new List<X12ClaimMatch>();
				Dictionary<long,int> dictClaimMatchCount=new Dictionary<long,int>();//1:1 with _listEtranss. Stores how many claim matches each 835 has.
				int batchQueryInterval=500;//Every 500 rows we get the next 500 message texts to save memory.
				int rowCur=0;
				foreach(Etrans etrans in _listEtranss) {
					if(rowCur%batchQueryInterval==0) {
						int range=Math.Min(batchQueryInterval,_listEtranss.Count-rowCur);//Either the full batchQueryInterval amount or the remaining amount of etrans.
						dictEtransMessages=EtransMessageTexts.GetMessageTexts(_listEtranss.GetRange(rowCur,range).Select(x => x.EtransMessageTextNum).ToList(),false);
					}
					rowCur++;
					ODEvent.Fire(new ODEventArgs("Etrans835",Lan.g(this,"Processing 835: ")+": "+rowCur+" out of "+_listEtranss.Count));
					List <Etrans835Attach> listAttachedTo835=listAttached.FindAll(x => x.EtransNum==etrans.EtransNum);
					X835 x835=new X835(etrans,dictEtransMessages[etrans.EtransMessageTextNum],etrans.TranSetId835,listAttachedTo835,true);
					_dictEtrans835s.Add(etrans.EtransNum,x835);
					List<X12ClaimMatch> listClaimMatches=x835.GetClaimMatches();
					dictClaimMatchCount.Add(etrans.EtransNum,listClaimMatches.Count);
					list835ClaimMatches.AddRange(listClaimMatches);
				}
				#region Set 835 unattached in batch and build _dictEtransClaims and _dictClaimPayCheckNums.
				ODEvent.Fire(new ODEventArgs("Etrans835",Lan.g(this,"Gathering internal claim matches.")));
				List<long> listClaimNums=Claims.GetClaimFromX12(list835ClaimMatches);//Can return null.
				ODEvent.Fire(new ODEventArgs("Etrans835",Lan.g(this,"Building data sets.")));
				int claimIndexCur=0;
				List<long> listMatchedClaimNums=new List<long>();
				foreach(Etrans etrans in _listEtranss) {
						X835 x835=_dictEtrans835s[etrans.EtransNum];
						if(listClaimNums!=null) {
							x835.SetClaimNumsForUnattached(listClaimNums.GetRange(claimIndexCur,dictClaimMatchCount[etrans.EtransNum]));
						}
						claimIndexCur+=dictClaimMatchCount[etrans.EtransNum];
						listMatchedClaimNums.AddRange(x835.ListClaimsPaid.FindAll(x => x.ClaimNum!=0).Select(x => x.ClaimNum).ToList());
				}
				List<Claim> listClaims=Claims.GetClaimsFromClaimNums(listMatchedClaimNums.Distinct().ToList());
				_dictClaimPayExists=ClaimPayments.HasClaimPayment(listMatchedClaimNums);//Every claim num is associated to a bool. True when there is an existing claimPayment.
				foreach(Etrans etrans in _listEtranss) {
					X835 x835=_dictEtrans835s[etrans.EtransNum];
					#region _dictEtransClaims, _dictClaimPayCheckNums
					_dictEtransClaims.Add(etrans.EtransNum,new List<Claim>());
					List <long> listSubClaimNums=x835.ListClaimsPaid.FindAll(x => x.ClaimNum!=0).Select(y => y.ClaimNum).ToList();
					List <Claim> listClaimsFor835=listClaims.FindAll(x => listSubClaimNums.Contains(x.ClaimNum));
					foreach(Hx835_Claim claim in x835.ListClaimsPaid) {
						Claim claimCur=listClaimsFor835.FirstOrDefault(x => x.ClaimNum==claim.ClaimNum);//Can be null.
						if(claimCur==null && claim.IsAttachedToClaim && claim.ClaimNum==0) {
							claimCur=new Claim();//Create empty claim since user detached claim manually, will not be considered in GetStringStatus(...).
						}
						if(claimCur!=null && claim.IsPreauth) {//User attached preauth to internal claim, no payment needed to be considered 'Finalized' in GetStringStatus(...).
							_dictClaimPayExists[claim.ClaimNum]=true;
						}
						_dictEtransClaims[etrans.EtransNum].Add(claimCur);
					}
					#endregion
				}
				ODEvent.Fire(new ODEventArgs("Etrans835",Lan.g(this,"Filling Grid.")));
				#endregion
			}
			gridMain.BeginUpdate();
			#region Initilize columns only once
			if(gridMain.Columns.Count==0) {
				ODGridColumn col;
				col=new ODGridColumn(Lan.g("TableEtrans835s","Patient Name"),250);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableEtrans835s","Carrier Name"),190);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableEtrans835s","Status"),80);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableEtrans835s","Date"),80);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableEtrans835s","Amount"),80);
				gridMain.Columns.Add(col);
				if(PrefC.HasClinicsEnabled) {
					col=new ODGridColumn(Lan.g("TableEtrans835s","Clinic"),70);
					gridMain.Columns.Add(col);
				}
				col=new ODGridColumn(Lan.g("TableEtrans835s","Code"),37,HorizontalAlignment.Center);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableEtrans835s","Note"),0);
				gridMain.Columns.Add(col);
			}
			#endregion
			gridMain.Rows.Clear();
			foreach(Etrans etrans in _listEtranss) {
				X835 x835=_dictEtrans835s[etrans.EtransNum];
				#region Filter: Carrier Name
				if(carrierName!="" && !x835.PayerName.ToLower().Contains(carrierName.ToLower())) {
					continue;
				}
				#endregion
				string status=GetStringStatus(etrans.EtransNum);
				#region Filter: Status
				if(!listSelectedStatuses.Contains(status.Replace("*",""))) {//The filter will ignore finalized with detached claims.
					continue;
				}
				#endregion
				//List of ClinicNums for the current etrans.ListClaimsPaid from the DB.
				List<long> listClinicNums=_dictEtransClaims[etrans.EtransNum].Select(x => x==null? 0 :x.ClinicNum).Distinct().ToList();
				#region Filter: Clinics
				if(PrefC.HasClinicsEnabled && !listClinicNums.Exists(x => listSelectedClinicNums.Contains(x))) {
					continue;//The ClinicNums associated to the 835 do not match any of the selected ClinicNums, so nothing to show in this 835.
				}
				#endregion
				#region Filter: Check and Trace Value
				if(checkTraceNum!="" && !x835.TransRefNum.Contains(checkTraceNum)) {//Trace Number does not match
					continue;
				}
				#endregion
				#region Filter: Insurance Check Range Min and Max
				if(amountMin!="" && x835.InsPaid < PIn.Decimal(amountMin) || amountMax!="" && x835.InsPaid > PIn.Decimal(amountMax)) {
					continue;//Either the InsPaid is below or above our range.
				}
				#endregion
				ODGridRow row=new ODGridRow();
				#region Column: Patient Name
				List<string> listPatNames=x835.ListClaimsPaid.Select(x => x.PatientName.ToString()).Distinct().ToList();
				string patName=(listPatNames.Count>0 ? listPatNames[0] : "");
				if(listPatNames.Count>1) {
					patName="("+POut.Long(listPatNames.Count)+")";
				}
				row.Cells.Add(patName);
				#endregion
				row.Cells.Add(x835.PayerName);
				row.Cells.Add(status);//See GetStringStatus(...) for possible values.
				row.Cells.Add(POut.Date(etrans.DateTimeTrans));
				row.Cells.Add(POut.Decimal(x835.InsPaid));
				#region Column: Clinic
				if(PrefC.HasClinicsEnabled) {	
					string clinicAbbr="";
					if(listClinicNums.Count==1) {
						if(listClinicNums[0]==0) {
							clinicAbbr=Lan.g(this,"Unassigned");
						}
						else {
							clinicAbbr=Clinics.GetAbbr(listClinicNums[0]);
						}
					}
					else if(listClinicNums.Count>1) {
						clinicAbbr="("+Lan.g(this,"Multiple")+")";
					}
					row.Cells.Add(clinicAbbr);
				}
				#endregion
				row.Cells.Add(x835._paymentMethodCode);
				row.Cells.Add(etrans.Note);
				row.Tag=etrans;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			try {
				actionCloseProgress?.Invoke();//When this function executes quickly this can fail rarely, fail silently because of WaitCursor.
			}
			catch(Exception ex) {
				ex.DoNothing();
			}
			Cursor=Cursors.Default;
		}

		private string GetStringStatus(long etransNum) {
			int countRecieved=0;
			int countRevievedWithPay=0;
			int countOther=0;
			int countDetached=0;//Count of manually detached claims. These claims are not considered in status.
			foreach(Claim claim in _dictEtransClaims[etransNum]) {
				if(claim!=null && claim.ClaimNum==0) {//Dummy claim created for claim that was unatached by user.
					countDetached++;
					continue;
				}
				else if(claim!=null && claim.ClaimStatus=="R") {
					countRecieved++;
					if(_dictClaimPayExists[claim.ClaimNum]) {//Has at least 1 claimpayment associated to ClaimNum
						countRevievedWithPay++;//Could also be a manually detached claim.
					}
					continue;
				}
				countOther++;
			}
			if(countRecieved==_dictEtransClaims[etransNum].Count-countDetached) {//All claims found and marked recieved.
				if(countRevievedWithPay==_dictEtransClaims[etransNum].Count-countDetached) {
					if(countDetached > 0) {
						return Lans.g(this,"Finalized*");//All recieved and have existing ins check, but some detached.
					}
					return Lans.g(this,"Finalized");//All recieved and have existing ins check.
				}
				return Lans.g(this,"NotFinalized");//All recieved, but at least 1 missing an ins check.
			}
			else if(countOther==_dictEtransClaims[etransNum].Count-countDetached) {//Not recieved.
				return Lans.g(this,"Unprocessed");
			}
			else if(countRecieved>0) {//Some recieved but not all.
				return Lans.g(this,"Partial");
			}
			return "";//Just in case
		}

		#region Calendar Logic
		private void butDropFrom_Click(object sender,EventArgs e) {
			ToggleCalendars();
		}

		private void butDropTo_Click(object sender,EventArgs e) {
			ToggleCalendars();
		}

		private void ToggleCalendars() {
			if(calendarFrom.Visible) {
				butDropFrom.ImageIndex=0;//Set to arrow down image.
				butDropTo.ImageIndex=0;//Set to arrow down image.
				//hide the calendars
				calendarFrom.Visible=false;
				calendarTo.Visible=false;
				//Issue: What if they want to run for today? The will have to click away from the date then back on  today...
				//if(textDateFrom.Text=="") {
				//	textDateFrom.Text=calendarFrom.
				//}
				//if(textDateTo.Text=="") {
				//	textDateFrom.Text=calendarFrom.
				//}
			}
			else {
				butDropFrom.ImageIndex=1;//Set to arrow up image.
				butDropTo.ImageIndex=1;//Set to arrow up image.
				//set the date on the calendars to match what's showing in the boxes
				if(textDateFrom.errorProvider1.GetError(textDateFrom)==""
					&& textDateTo.errorProvider1.GetError(textDateTo)=="") {//if no date errors
					if(textDateFrom.Text=="") {
						calendarFrom.SetDate(DateTime.Today);
					}
					else {
						calendarFrom.SetDate(PIn.Date(textDateFrom.Text));
					}
					if(textDateTo.Text=="") {
						calendarTo.SetDate(DateTime.Today);
					}
					else {
						calendarTo.SetDate(PIn.Date(textDateTo.Text));
					}
				}
				//show the calendars
				calendarFrom.Visible=true;
				calendarTo.Visible=true;
			}
		}

		private void calendarFrom_DateSelected(object sender,DateRangeEventArgs e) {
			_reportDateFrom=calendarFrom.SelectionStart;
			textDateFrom.Text=_reportDateFrom.ToShortDateString();
		}

		private void calendarTo_DateSelected(object sender,DateRangeEventArgs e) {
			_reportDateTo=calendarTo.SelectionStart;
			textDateTo.Text=_reportDateTo.ToShortDateString();
		}

		private void butWeekPrevious_Click(object sender,EventArgs e) {
			DateTime dateFrom=PIn.Date(textDateFrom.Text);
			DateTime dateTo=PIn.Date(textDateTo.Text);
			if(dateFrom!=DateTime.MinValue) {
				dateTo=dateFrom.AddDays(-1);
				textDateFrom.Text=dateTo.AddDays(-7).ToShortDateString();
				textDateTo.Text=dateTo.ToShortDateString();
			}
			else if(dateTo!=DateTime.MinValue) {//Invalid dateFrom but valid dateTo
				dateTo=dateTo.AddDays(-8);
				textDateFrom.Text=dateTo.AddDays(-7).ToShortDateString();
				textDateTo.Text=dateTo.ToShortDateString();
			}
			else {//Both dates invalid
				textDateFrom.Text=DateTime.Today.AddDays(-7).ToShortDateString();
				textDateTo.Text=DateTime.Today.ToShortDateString();
			}
			if(calendarFrom.Visible) { //textTo and textFrom are set above, so no check is necessary.
				calendarFrom.SetDate(PIn.Date(textDateFrom.Text));
				calendarTo.SetDate(PIn.Date(textDateTo.Text));
			}
		}

		private void butWeekNext_Click(object sender,EventArgs e) {
			DateTime dateFrom=PIn.Date(textDateFrom.Text);
			DateTime dateTo=PIn.Date(textDateTo.Text);
			if(dateTo!=DateTime.MinValue) {
				dateFrom=dateTo.AddDays(1);
				textDateFrom.Text=dateFrom.ToShortDateString();
				textDateTo.Text=dateFrom.AddDays(7).ToShortDateString();
			}
			else if(dateFrom!=DateTime.MinValue) {//Invalid dateTo but valid dateFrom
				 dateFrom=dateFrom.AddDays(8);
				 textDateFrom.Text=dateFrom.ToShortDateString();
				 textDateTo.Text=dateFrom.AddDays(7).ToShortDateString();
			}
			else {//Both dates invalid
				textDateFrom.Text=DateTime.Today.ToShortDateString();
				textDateTo.Text=DateTime.Today.AddDays(7).ToShortDateString();
			}
			if(calendarFrom.Visible) { //textTo and textFrom are set above, so no check is necessary.
				calendarFrom.SetDate(PIn.Date(textDateFrom.Text));
				calendarTo.SetDate(PIn.Date(textDateTo.Text));
			}
		}
		#endregion

		#region Filter Logic
		private void comboStatus_SelectionChangeCommitted(object sender,EventArgs e) {
			FilterAndFillGrid();
		}

		private void comboClinics_SelectionChangeCommitted(object sender,EventArgs e) {
			FilterAndFillGrid();
		}

		private void listStatus_MouseClick(object sender,MouseEventArgs e) {
			FilterAndFillGrid();
		}
		
		private void textCarrier_KeyUp(object sender,KeyEventArgs e) {
			FilterAndFillGrid();
		}

		private void textCheckTrace_KeyUp(object sender,KeyEventArgs e) {
			FilterAndFillGrid();
		}

		private void textRangeMin_KeyUp(object sender,KeyEventArgs e) {
			FilterAndFillGrid();
		}

		private void textRangeMax_KeyUp(object sender,KeyEventArgs e) {
			FilterAndFillGrid();
		}

		///<summary>Called when we need to filter the current in memory contents in _listEtrans. Calls FillGrid()</summary>
		private void FilterAndFillGrid(bool isRefreshNeeded=false) {
			List<string> listSelectedStatuses=new List<string>();
			foreach(int index in listStatus.SelectedIndices) {
				listSelectedStatuses.Add(listStatus.Items[index].ToString());
			}
			List<long> listClinicNums=null;//A null signifies that clinics are disabled.
			if(PrefC.HasClinicsEnabled) {
				if(comboClinics.SelectedIndices.Contains(0)) {//'All' is selected
					listClinicNums=_listUserClinics.Select(x => x.ClinicNum).ToList();
					listClinicNums.Add(0);//'Unassigned'
				}
				else {//'All' is not selected
					int offset=2;//Skip the 'All' option.  Skip the 'Unassigned' option.
					listClinicNums=comboClinics.ListSelectedIndices.FindAll(x => x > offset-1)//Ignore 'All' and 'Unassigned'.
						.Select(x => _listUserClinics[x-offset])
						.Select(x => x.ClinicNum).ToList();
					if(comboClinics.SelectedIndices.Contains(1)) {//Unassigned
						listClinicNums.Add(0);
					}
				}
			}
			FillGrid(
				isRefreshNeeded:				isRefreshNeeded,
				listSelectedStatuses:		listSelectedStatuses,
				listSelectedClinicNums:	listClinicNums,
				carrierName:						textCarrier.Text,
				checkTraceNum:					textCheckTrace.Text,
				amountMin:							textRangeMin.Text,
				amountMax:							textRangeMax.Text
			);
		}
		#endregion

		private void butRefresh_Click(object sender,EventArgs e) {
			RefreshAndFillGrid();
		}

		private void gridMain_DoubleClick(object sender,EventArgs e) {
			int index=gridMain.GetSelectedIndex();
			if(index==-1) {//Clicked in empty space. 
				return;
			}
			//Mimics FormClaimsSend.gridHistory_CellDoubleClick(...)
			Cursor=Cursors.WaitCursor;
			Etrans et=(Etrans)gridMain.Rows[index].Tag;
			//Sadly this is needed due to FormEtrans835Edit calling Etranss.Update .
			//See Etranss.RefreshHistory(...), this query does not select all etrans columns.
			//Mimics FormClaimsSend.gridHistory_CellDoubleClick(...)
			et=Etranss.GetEtrans(et.EtransNum);
			FormEtrans835Edit.ShowEra(et);
			Cursor=Cursors.Default;
		}
		
		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
			Close();
		}

	}

}
