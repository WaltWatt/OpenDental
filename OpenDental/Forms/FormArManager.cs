using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CodeBase;
using MySql.Data.MySqlClient;
using OpenDental.UI;
using OpenDentalCloud;
using OpenDentalCloud.Core;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormArManager:ODForm {
		private List<Provider> _listProviders;
		private List<Clinic> _listClinics;
		private List<Def> _listBillTypesNoColl;
		private Def _collectionBillType;
		private List<PatAging> _listPatAgingUnsentAll;
		private List<PatAging> _listPatAgingSentAll;
		private List<TsiTransType> _listNewStatuses;
		private List<TsiTransType> _listSentTabTransTypes;
		private bool _isResizing;
		private bool _hasResizeBegan;
		///<summary>Used to reselect rows after sorting the grid by column.  Filled on grid MouseDown event and used in grid OnSortByColumn event to reselect.</summary>
		private List<long> _listSelectedPatNums;
		private Program _tsiProg;
		private Dictionary<long,List<ProgramProperty>> _dictClinicProgProps;
		private ToolTip _toolTipUnsentErrors;
		private Point _lastCursorPos;

		public FormArManager() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormArManager_Load(object sender,EventArgs e) {
			#region Get Variables for Both Tabs
			List<Def> billTypeDefs=Defs.GetDefsForCategory(DefCat.BillingTypes,true);
			_collectionBillType=billTypeDefs.FirstOrDefault(x => x.ItemValue.ToLower()=="c")?.Copy();
			_listBillTypesNoColl=billTypeDefs.Where(x => x.ItemValue.ToLower()!="c").Select(x => x.Copy()).ToList();
			_listClinics=new List<Clinic>();
			if(PrefC.HasClinicsEnabled) {
				_listClinics.AddRange(Clinics.GetForUserod(Security.CurUser,true,Lan.g(this,"Unassigned")).OrderBy(x => x.ClinicNum!=0).ThenBy(x => x.ItemOrder));
			}
			else {//clinics disabled
				_listClinics.Add(Clinics.GetPracticeAsClinicZero(Lan.g(this,"Unassigned")));
			}
			_listProviders=Providers.GetDeepCopy(true);
			_tsiProg=Programs.GetCur(ProgramName.Transworld);
			_dictClinicProgProps=new Dictionary<long,List<ProgramProperty>>();
			if(_tsiProg!=null && _tsiProg.Enabled) {
				_dictClinicProgProps=ProgramProperties.GetForProgram(_tsiProg.ProgramNum)
					.FindAll(x => _listClinics.Any(y => y.ClinicNum==x.ClinicNum))//will contain the HQ "clinic" if clinics are disabled or user unrestricted
					.GroupBy(x => x.ClinicNum).ToDictionary(x => x.Key,x => x.ToList());
			}
			_toolTipUnsentErrors=new ToolTip() { InitialDelay=1000,ReshowDelay=1000,ShowAlways=true };
			_lastCursorPos=gridUnsent.PointToClient(Cursor.Position);
			#endregion Get Variables for Both Tabs
			#region Fill Unsent Tab Filter ComboBoxes, CheckBoxes, and Fields
			#region Unsent Tab Clinic Combo
			if(PrefC.HasClinicsEnabled) {
				comboBoxMultiUnsentClinics.Visible=true;
				labelUnsentClinics.Visible=true;
				comboBoxMultiSentClinics.Visible=true;
				labelSentClinics.Visible=true;
				comboBoxMultiUnsentClinics.Items.Add(Lan.g(this,"All"));
				comboBoxMultiSentClinics.Items.Add(Lan.g(this,"All"));
				if(Clinics.ClinicNum==0) {
					comboBoxMultiUnsentClinics.SetSelected(0,true);
					comboBoxMultiSentClinics.SetSelected(0,true);
				}
				foreach(Clinic clinCur in _listClinics) {
					comboBoxMultiUnsentClinics.Items.Add(clinCur.Abbr);
					comboBoxMultiSentClinics.Items.Add(clinCur.Abbr);
					if(Clinics.ClinicNum>0 && clinCur.ClinicNum==Clinics.ClinicNum) {
						comboBoxMultiUnsentClinics.SetSelected(comboBoxMultiUnsentClinics.Items.Count-1,true);
						comboBoxMultiSentClinics.SetSelected(comboBoxMultiSentClinics.Items.Count-1,true);
					}
				}
				if(comboBoxMultiUnsentClinics.SelectedIndices.Count==0) {
					comboBoxMultiUnsentClinics.SetSelected(0,true);
					comboBoxMultiSentClinics.SetSelected(0,true);//if unsent clinic combo has 0 selected, so will sent clinic combo
				}
			}
			#endregion Unsent Tab Clinic Combo
			#region Unsent Tab Prov Combo
			comboBoxMultiUnsentProvs.Items.Add("All");
			comboBoxMultiUnsentProvs.SetSelected(0,true);
			_listProviders.ForEach(x => comboBoxMultiUnsentProvs.Items.Add(x.GetLongDesc()));
			#endregion Unsent Tab Prov Combo
			#region Unsent Tab Bill Type Combo
			List<long> listDefaultBillTypes=PrefC.GetString(PrefName.ArManagerBillingTypes)
				.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries)
				.Select(x => PIn.Long(x)).ToList();
			comboBoxMultiBillTypes.Items.Add("All");
			comboBoxMultiBillTypes.SetSelected(0,_listBillTypesNoColl.All(x => !listDefaultBillTypes.Contains(x.DefNum)));//select All if no valid defaults are set
			for(int i=0;i<_listBillTypesNoColl.Count;i++) {
				comboBoxMultiBillTypes.Items.Add(_listBillTypesNoColl[i].ItemName);
				if(listDefaultBillTypes.Contains(_listBillTypesNoColl[i].DefNum)) {
					comboBoxMultiBillTypes.SetSelected(i+1,true);//+1 for All
				}
			}
			#endregion Unsent Tab Bill Type Combo
			#region Unsent Tab Account Age Combo
			comboUnsentAccountAge.Items.Add(Lan.g(this,"Any Balance"));
			comboUnsentAccountAge.Items.Add(Lan.g(this,"Over 30 Days"));
			comboUnsentAccountAge.Items.Add(Lan.g(this,"Over 60 Days"));
			comboUnsentAccountAge.Items.Add(Lan.g(this,"Over 90 Days"));
			comboUnsentAccountAge.SelectedIndexChanged-=comboUnsentAccountAge_SelectedIndexChanged;
			comboUnsentAccountAge.SelectedIndex=new List<string>() { "30","60","90" }.IndexOf(PrefC.GetString(PrefName.ArManagerUnsentAgeOfAccount))+1;//+1 for any bal
			comboUnsentAccountAge.SelectedIndexChanged+=comboUnsentAccountAge_SelectedIndexChanged;
			#endregion Unsent Tab Account Age Combo
			#region Unsent Tab Textbox Filters
			//text min bal
			textUnsentMinBal.TextChanged-=textUnsentMinBal_TextChanged;
			textUnsentMinBal.Text=PrefC.GetDouble(PrefName.ArManagerUnsentMinBal).ToString();
			textUnsentMinBal.TextChanged+=textUnsentMinBal_TextChanged;
			//text days since last payment
			textUnsentDaysLastPay.TextChanged-=textUnsentDaysLastPay_TextChanged;
			textUnsentDaysLastPay.Text=PrefC.GetInt(PrefName.ArManagerUnsentDaysSinceLastPay).ToString();
			textUnsentDaysLastPay.TextChanged+=textUnsentDaysLastPay_TextChanged;
			#endregion Unsent Tab Textbox Filters
			#region Unsent Tab Checkbox Filters
			//exclude if ins pending
			checkExcludeInsPending.CheckedChanged-=checkExcludeInsPending_CheckedChanged;
			checkExcludeInsPending.Checked=PrefC.GetBool(PrefName.ArManagerExcludeInsPending);
			checkExcludeInsPending.CheckedChanged+=checkExcludeInsPending_CheckedChanged;
			//exclude if unsent procs
			checkExcludeIfProcs.CheckedChanged-=checkExcludeIfProcs_CheckedChanged;
			checkExcludeIfProcs.Checked=PrefC.GetBool(PrefName.ArManagerExcludeIfUnsentProcs);
			checkExcludeIfProcs.CheckedChanged+=checkExcludeIfProcs_CheckedChanged;
			//exclude if bad address (no zipcode)
			checkExcludeBadAddress.CheckedChanged-=checkExcludeBadAddress_CheckedChanged;
			checkExcludeBadAddress.Checked=PrefC.GetBool(PrefName.ArManagerExcludeBadAddresses);
			checkExcludeBadAddress.CheckedChanged+=checkExcludeBadAddress_CheckedChanged;
			#endregion Unsent Tab Checkbox Filters
			#region Unsent Tab Demand Type Combo
			FillDemandTypes();
			#endregion Unsent Tab Demand Type Combo
			#endregion Fill Unsent Tab Filter ComboBoxes, CheckBoxes, and Fields
			#region Fill Sent Tab Filter ComboBoxes, CheckBoxes, and Fields
			#region Sent Tab Provs Combo
			comboBoxMultiSentProvs.Items.Add("All");
			comboBoxMultiSentProvs.SetSelected(0,true);
			_listProviders.ForEach(x => comboBoxMultiSentProvs.Items.Add(x.GetLongDesc()));
			#endregion Sent Tab Provs Combo
			#region Sent Tab Trans Type Combo
			_listSentTabTransTypes=Enum.GetValues(typeof(TsiTransType)).OfType<TsiTransType>()
				.Where(x => !x.In(TsiTransType.PF,TsiTransType.PT,TsiTransType.SS,TsiTransType.CN)).ToList();
			List<TsiTransType> listDefaultLastTransTypes=PrefC.GetString(PrefName.ArManagerLastTransTypes)
				.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries)
				.Select(x => PIn.Enum<TsiTransType>(x,true))
				.Where(x => !x.In(TsiTransType.PF,TsiTransType.PT,TsiTransType.SS,TsiTransType.CN)).ToList();
			comboBoxMultiLastTransType.Items.Add(Lan.g(this,"All"));
			comboBoxMultiLastTransType.SetSelected(0,_listSentTabTransTypes.All(x => !listDefaultLastTransTypes.Contains(x)));//select All if no valid defaults are set
			for(int i=0;i<_listSentTabTransTypes.Count;i++) {
				comboBoxMultiLastTransType.Items.Add(_listSentTabTransTypes[i].GetDescription());
				if(listDefaultLastTransTypes.Contains(_listSentTabTransTypes[i])) {
					comboBoxMultiLastTransType.SetSelected(i+1,true);//+1 for All
				}
			}
			#endregion Sent Tab Trans Type Combo
			#region Sent Tab Account Age Combo
			comboSentAccountAge.Items.Add(Lan.g(this,"Any Balance"));
			comboSentAccountAge.Items.Add(Lan.g(this,"Over 30 Days"));
			comboSentAccountAge.Items.Add(Lan.g(this,"Over 60 Days"));
			comboSentAccountAge.Items.Add(Lan.g(this,"Over 90 Days"));
			comboSentAccountAge.SelectedIndexChanged-=comboSentAccountAge_SelectedIndexChanged;
			comboSentAccountAge.SelectedIndex=new List<string>() { "30","60","90" }.IndexOf(PrefC.GetString(PrefName.ArManagerSentAgeOfAccount))+1;//+1 for any bal
			comboSentAccountAge.SelectedIndexChanged+=comboSentAccountAge_SelectedIndexChanged;
			#endregion Sent Tab Account Age Combo
			#region Sent Tab Textbox Filters
			//text min bal
			textSentMinBal.TextChanged-=textSentMinBal_TextChanged;
			textSentMinBal.Text=PrefC.GetDouble(PrefName.ArManagerSentMinBal).ToString();
			textSentMinBal.TextChanged+=textSentMinBal_TextChanged;
			//text days since last payment
			textSentDaysLastPay.TextChanged-=textSentDaysLastPay_TextChanged;
			textSentDaysLastPay.Text=PrefC.GetInt(PrefName.ArManagerSentDaysSinceLastPay).ToString();
			textSentDaysLastPay.TextChanged+=textSentDaysLastPay_TextChanged;
			#endregion Sent Tab Textbox Filters
			#region Sent Tab New Statuses Combo
			_listNewStatuses=new List<TsiTransType>() { TsiTransType.SS,TsiTransType.CN };
			_listNewStatuses.ForEach(x => comboNewStatus.Items.Add(x.GetDescription()));
			#endregion Sent Tab New Statuses Combo
			#region Sent Tab New Bill Types Combo
			_listBillTypesNoColl.ForEach(x => comboNewBillType.Items.Add(x.ItemName));
			errorProvider1.SetError(comboNewBillType,"");
			#endregion Sent Tab New Bill Types Combo
			#endregion Fill Sent Tab Filter ComboBoxes, CheckBoxes, and Fields
			#region Run Aging if Necessary
			string msgText="";
			try {
				msgText="There was a problem running aging.  Would you like to load the accounts grid with currently existing account information?";
				while(!RunAgingIfNecessary()) {
					if(!MsgBox.Show(this,MsgBoxButtons.YesNo,msgText)) {
						Close();
						return;
					}
				}
			}
			catch(Exception ex) {
				ex.DoNothing();
				msgText="There was a problem running aging.  Would you like to load the accounts grid with currently existing account information?";
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,msgText)) {
					Close();
					return;
				}
			}
			#endregion Run Aging if Necessary
			#region Get Aging List and Fill Grids
			RefreshAll();
			FillGrids();
			#endregion Get Aging List and Fill Grids
		}

		#region Methods For Both Tabs

		private bool RunAgingIfNecessary() {
			string msgText="";
			if(PrefC.GetBool(PrefName.AgingIsEnterprise)) {
				return RunAgingEnterprise();
			}
			else if(!PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)) {
				Cursor=Cursors.WaitCursor;
				msgText=Lan.g(this,"Calculating aging for all patients as of")+" "+DateTime.Today.ToShortDateString()+"...";
				Action actionCloseAgingProgress=ODProgressOld.ShowProgressStatus("ArManager",this,msgText);
				try {
					Ledgers.RunAging();
				}
				catch(MySqlException ex) {
					if(ex==null || ex.Number!=1213) {//not a deadlock error, just throw
						throw;
					}
					actionCloseAgingProgress?.Invoke();//terminates progress bar
					Cursor=Cursors.Default;
					MsgBox.Show(this,"Deadlock error detected in aging transaction and rolled back. Try again later.");
					return false;
				}
				finally {
					actionCloseAgingProgress?.Invoke();//terminates progress bar
					Cursor=Cursors.Default;
				}
			}
			msgText="Last aging date seems old.  Would you like to run aging now?  The account list will load whether or not aging gets updated.";
			//All places in the program where aging can be run for all patients, the Setup permission is required because it can take a long time.
			if(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily) 
				&& Security.IsAuthorized(Permissions.Setup,true)
				&& PrefC.GetDate(PrefName.DateLastAging)<DateTime.Today.AddDays(-15)
				&& MsgBox.Show(this,MsgBoxButtons.YesNo,msgText))
			{
				FormAging FormA=new FormAging();
				FormA.BringToFront();
				FormA.ShowDialog();
			}
			return true;
		}

		private bool RunAgingEnterprise() {
			DateTime dtNow=MiscData.GetNowDateTime();
			DateTime dtToday=dtNow.Date;
			DateTime dateLastAging=PrefC.GetDate(PrefName.DateLastAging);
			string msgText=Lan.g(this,"Aging has already been calculated for")+" "+dtToday.ToShortDateString()+" "
				+Lan.g(this,"and does not normally need to run more than once per day.")+"\r\n\r\n"+Lan.g(this,"Run anway?");
			if(dateLastAging.Date==dtToday.Date && MessageBox.Show(this,msgText,"",MessageBoxButtons.YesNo)!=DialogResult.Yes) {
				return true;
			}
			Prefs.RefreshCache();
			DateTime dateTAgingBeganPref=PrefC.GetDateT(PrefName.AgingBeginDateTime);
			if(dateTAgingBeganPref>DateTime.MinValue) {
				msgText=Lan.g(this,"In order to manage accounts receivable, aging must be calculated, but you cannot run aging until it has finished the current "
					+"calculations which began on")+" "+dateTAgingBeganPref.ToString()+".\r\n"+Lans.g(this,"If you believe the current aging process has finished, "
					+"a user with SecurityAdmin permission can manually clear the date and time by going to Setup | Miscellaneous and pressing the 'Clear' button.");
				MessageBox.Show(this,msgText);
				return false;
			}
			Prefs.UpdateString(PrefName.AgingBeginDateTime,POut.DateT(dtNow,false));//get lock on pref to block others
			Signalods.SetInvalid(InvalidType.Prefs);//signal a cache refresh so other computers will have the updated pref as quickly as possible
			Cursor=Cursors.WaitCursor;
			msgText=Lan.g(this,"Calculating enterprise aging for all patients as of")+" "+dtToday.ToShortDateString()+"...";
			Action actionCloseAgingProgress=ODProgressOld.ShowProgressStatus("ArManager",this,msgText);
			try {
				Ledgers.ComputeAging(0,dtToday);
				Prefs.UpdateString(PrefName.DateLastAging,POut.Date(dtToday,false));
			}
			catch(MySqlException ex) {
				if(ex==null || ex.Number!=1213) {//not a deadlock error, just throw
					throw;
				}
				actionCloseAgingProgress?.Invoke();//terminates progress bar
				Cursor=Cursors.Default;
				MsgBox.Show(this,"Deadlock error detected in aging transaction and rolled back. Try again later.");
				return false;
			}
			finally {
				Prefs.UpdateString(PrefName.AgingBeginDateTime,"");//clear lock on pref whether aging was successful or not
				Signalods.SetInvalid(InvalidType.Prefs);
				actionCloseAgingProgress?.Invoke();//terminates progress bar
				Cursor=Cursors.Default;
			}
			return true;
		}

		private void RefreshAll() {
			Cursor=Cursors.WaitCursor;
			string msgText=Lan.g(this,"Retrieving aging list as of")+" "+MiscData.GetNowDateTime().ToShortDateString()+"...";
			Action actionCloseAgingProgress=ODProgressOld.ShowProgressStatus("ArManager",this,msgText);
			List<PatAging> listPatAgingAll=Patients.GetAgingList();
			_listPatAgingSentAll=new List<PatAging>();
			_listPatAgingUnsentAll=new List<PatAging>();
			foreach(PatAging ptAgeCur in listPatAgingAll) {
				if(_collectionBillType!=null && ptAgeCur.BillingType==_collectionBillType.DefNum) {
					_listPatAgingSentAll.Add(ptAgeCur);
				}
				else {
					_listPatAgingUnsentAll.Add(ptAgeCur);
				}
			}
			actionCloseAgingProgress?.Invoke();
			Cursor=Cursors.Default;
		}

		private void FillGrids() {
			FillGridUnsent();
			FillGridSent();
		}

		private void menuItemGoTo_Click(object sender,EventArgs e) {
			PatAging patAgeCur;
			ODGrid gridCur;
			if(tabControlMain.SelectedTab==tabUnsent) {
				gridCur=gridUnsent;
			}
			else {
				gridCur=gridSent;
			}
			if(gridCur.SelectedIndices.Length!=1) {
				MsgBox.Show(this,"Please select one patient first.");
				return;
			}
			patAgeCur=gridCur.Rows[gridCur.SelectedIndices[0]].Tag as PatAging;
			if(patAgeCur==null) {
				return;
			}
			FormOpenDental.S_Contr_PatientSelected(Patients.GetPat(patAgeCur.PatNum),false);
			GotoModule.GotoAccount(0);
			SendToBack();
		}

		private void timerFillGrid_Tick(object sender,EventArgs e) {
			timerFillGrid.Enabled=false;
			ValidateChildren(ValidationConstraints.Enabled|ValidationConstraints.Visible|ValidationConstraints.Selectable);
			if(tabControlMain.SelectedTab==tabUnsent) {
				FillGridUnsent();
			}
			else {
				FillGridSent();
			}
		}

		private void tabControlMain_SelectedIndexChanged(object sender,EventArgs e) {
			timerFillGrid.Enabled=false;
		}

		///<summary>Fills the grid, but only if not preceded by a ResizeBegin event, i.e. the form is being manually resized or moved around the screen.</summary>
		private void FormArManager_Resize(object sender,EventArgs e) {
			_isResizing=true;
			if(_hasResizeBegan || WindowState==FormWindowState.Minimized) {
				return;//handle fill grid in ResizeEnd
			}
			//Don't attempt to fill the grid if either of these lists are null.  They're set on load and should never be null once load has finished, but
			//this was firing before load and caused an error on one customer PC.
			if(_listPatAgingUnsentAll==null || _listPatAgingSentAll==null) {
				_isResizing=false;
				return;
			}
			FillGrids();
			_isResizing=false;
		}

		///<summary>Fires when manual resizing begins.  Sets _hasResizeBegan so the grid is only filled once manual resizing is finished.</summary>
		private void FormArManager_ResizeBegin(object sender,EventArgs e) {
			_hasResizeBegan=true;
		}

		///<summary>Fires when manual resizing is complete, NOT when changing window state. i.e. this is not fired when the window is maximized, minimized
		///or restored.  But this also fires when just moving the form around, so we will use both bools to determine if we have a ResizeBegin and actual 
		///resize events and only then refill the grid.</summary>
		private void FormArManager_ResizeEnd(object sender,EventArgs e) {
			if(_isResizing && _hasResizeBegan) {
				FillGrids();
			}
			_isResizing=false;
			_hasResizeBegan=false;
		}

		private void SaveDefaults() {
			if(textUnsentMinBal.errorProvider1.GetError(textUnsentMinBal)!="" || textUnsentDaysLastPay.errorProvider1.GetError(textUnsentDaysLastPay)!=""
				|| textSentMinBal.errorProvider1.GetError(textSentMinBal)!="" || textSentDaysLastPay.errorProvider1.GetError(textSentDaysLastPay)!="")
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			#region Sent Defaults
			string selectedTransTypes="";//indicates all.
			if(comboBoxMultiLastTransType.SelectedIndices.Count>0 && !comboBoxMultiLastTransType.SelectedIndices.Contains(0)) {
				selectedTransTypes=string.Join(",",comboBoxMultiLastTransType.ListSelectedIndices.Select(x => _listSentTabTransTypes[x-1]));//-1 for All
			}
			string sentAgeOfAccount="";//indicates any age
			if(comboSentAccountAge.SelectedIndex.In(1,2,3)) {
				sentAgeOfAccount=(30*comboSentAccountAge.SelectedIndex).ToString();//ageOfAccount is 30, 60, or 90
			}
			int sentDaysSinceLastPay=0;
			if(!string.IsNullOrWhiteSpace(textSentDaysLastPay.Text)) {
				sentDaysSinceLastPay=PIn.Int(textSentDaysLastPay.Text);
			}
			double sentMinBal=0.00;
			if(!string.IsNullOrWhiteSpace(textSentMinBal.Text)) {
				sentMinBal=PIn.Double(textSentMinBal.Text);
			}
			#endregion Sent Defaults
			#region Unsent Defaults
			string selectedBillTypes="";//indicates all.
			if(comboBoxMultiBillTypes.SelectedIndices.Count>0 && !comboBoxMultiBillTypes.SelectedIndices.Contains(0)) {
				selectedBillTypes=string.Join(",",comboBoxMultiBillTypes.ListSelectedIndices.Select(x => _listBillTypesNoColl[x-1].DefNum));//-1 for All
			}
			string unsentAgeOfAccount="";//indicates any age
			if(comboUnsentAccountAge.SelectedIndex.In(1,2,3)) {
				unsentAgeOfAccount=(30*comboUnsentAccountAge.SelectedIndex).ToString();//ageOfAccount is 30, 60, or 90
			}
			int unsentDaysSinceLastPay=0;
			if(!string.IsNullOrWhiteSpace(textUnsentDaysLastPay.Text)) {
				unsentDaysSinceLastPay=PIn.Int(textUnsentDaysLastPay.Text);
			}
			double unsentMinBal=0.00;
			if(!string.IsNullOrWhiteSpace(textUnsentMinBal.Text)) {
				unsentMinBal=PIn.Double(textUnsentMinBal.Text);
			}
			#endregion Unsent Defaults
			if( Prefs.UpdateString(PrefName.ArManagerBillingTypes,selectedBillTypes)
				| Prefs.UpdateBool(PrefName.ArManagerExcludeBadAddresses,checkExcludeBadAddress.Checked)
				| Prefs.UpdateBool(PrefName.ArManagerExcludeIfUnsentProcs,checkExcludeIfProcs.Checked)
				| Prefs.UpdateBool(PrefName.ArManagerExcludeInsPending,checkExcludeInsPending.Checked)
				| Prefs.UpdateString(PrefName.ArManagerLastTransTypes,selectedTransTypes)
				| Prefs.UpdateString(PrefName.ArManagerSentAgeOfAccount,sentAgeOfAccount)
				| Prefs.UpdateInt(PrefName.ArManagerSentDaysSinceLastPay,sentDaysSinceLastPay)
				| Prefs.UpdateString(PrefName.ArManagerSentMinBal,POut.Double(sentMinBal))
				| Prefs.UpdateString(PrefName.ArManagerUnsentAgeOfAccount,unsentAgeOfAccount)
				| Prefs.UpdateInt(PrefName.ArManagerUnsentDaysSinceLastPay,unsentDaysSinceLastPay)
				| Prefs.UpdateString(PrefName.ArManagerUnsentMinBal,POut.Double(unsentMinBal)))
			{
				DataValid.SetInvalid(InvalidType.Prefs);
			}
		}

		private void butTsiOcp_Click(object sender,EventArgs e) {
			Process.Start("https://service.transworldsystems.com/FormsLogin.asp?/rep/repview.asp");
		}

		#endregion Methods For Both Tabs
		#region Unsent Tab Methods

		///<summary>Returns a dictionary of key=column heading, value=tuple of Item1=xPos,Item2=width for the given column heading.  Used to position
		///totals texboxes below the corresponding column and to resize/reposition as the form is resized.</summary>
		private Dictionary<string,Tuple<int,int>> GetXPosAndWidths(ODGrid grid) {
			Dictionary<string,Tuple<int,int>> retval=new Dictionary<string,Tuple<int,int>>();
			int xPos=grid.Location.X;
			foreach(ODGridColumn column in grid.Columns) {
				retval[column.Heading]=Tuple.Create(xPos+1,column.ColWidth+1);//+1 because the textbox lines seem to be slightly thinner than the grid column lines
				xPos+=column.ColWidth;
			}
			return retval;
		}

		private void FillGridUnsent() {
			Cursor=Cursors.WaitCursor;
			List<PatAging> listPatAgeFiltered=GetListPatAgingUnsentFiltered();
			List<long> listSelectedPatNums=gridUnsent.SelectedGridRows.Select(x => (x.Tag as PatAging).PatNum).ToList();
			#region Set Grid Title and Columns
			gridUnsent.BeginUpdate();
			gridUnsent.Columns.Clear();
			gridUnsent.Columns.Add(new ODGridColumn("Guarantor",PrefC.HasClinicsEnabled?140:240));
			if(PrefC.HasClinicsEnabled) {
				gridUnsent.Columns.Add(new ODGridColumn("Clinic",100));
			}
			gridUnsent.Columns.Add(new ODGridColumn("Prov",70));
			gridUnsent.Columns.Add(new ODGridColumn("Billing Type",83));
			gridUnsent.Columns.Add(new ODGridColumn("0-30 Days",73,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridUnsent.Columns.Add(new ODGridColumn("31-60 Days",75,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridUnsent.Columns.Add(new ODGridColumn("61-90 Days",75,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridUnsent.Columns.Add(new ODGridColumn("> 90 Days",73,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridUnsent.Columns.Add(new ODGridColumn("Total",60,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridUnsent.Columns.Add(new ODGridColumn("-Ins Est",65,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridUnsent.Columns.Add(new ODGridColumn("=Patient",65,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridUnsent.Columns.Add(new ODGridColumn("PayPlan Due",80,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridUnsent.Columns.Add(new ODGridColumn("Date Last Pay",85,HorizontalAlignment.Center,GridSortingStrategy.DateParse));
			gridUnsent.Columns.Add(new ODGridColumn("DateTime Suspended",135));
			//this form initially set to the max allowed (by OD) form size 1246, which is also the minimum size for this form.  If the user resizes the form
			//to be larger, increase each column width by the same ratio to spread out the additional real estate
			gridUnsent.Columns.OfType<ODGridColumn>().ToList().ForEach(x => x.ColWidth=(int)((float)x.ColWidth*Width/MinimumSize.Width));
			#endregion Set Grid Title and Columns
			#region Fill Grid Rows
			gridUnsent.Rows.Clear();
			bool isPatNumIncluded=PrefC.GetBool(PrefName.ReportsShowPatNum);
			Dictionary<long,string> dictClinicAbbrs=_listClinics.ToDictionary(x => x.ClinicNum,x => x.Abbr);
			Dictionary<long,string> dictProvAbbrs=_listProviders.ToDictionary(x => x.ProvNum,x => x.Abbr);
			Dictionary<long,string> dictBillTypeNames=Defs.GetDefsForCategory(DefCat.BillingTypes).ToDictionary(x => x.DefNum,x => x.ItemName);
			Dictionary<long,DateTime> dictSuspendDateTimes=new Dictionary<long,DateTime>();
			foreach(PatAging pAgeCur in listPatAgeFiltered) {
				TsiTransLog tsiLogMostRecentStatusChange=pAgeCur.ListTsiLogs
					.Find(x => x.TransType.In(TsiTransType.CN,TsiTransType.PF,TsiTransType.PL,TsiTransType.PT,TsiTransType.RI,TsiTransType.SS));
				if(tsiLogMostRecentStatusChange!=null && tsiLogMostRecentStatusChange.TransType==TsiTransType.SS) {
					dictSuspendDateTimes.Add(pAgeCur.PatNum,tsiLogMostRecentStatusChange.TransDateTime);
				}
			}
			ODGridRow row;
			List<int> listIndicesToReselect=new List<int>();
			foreach(PatAging patAgeCur in listPatAgeFiltered) {
				row=new ODGridRow();
				row.Cells.Add((isPatNumIncluded?(patAgeCur.PatNum.ToString()+" - "):"")+patAgeCur.PatName);
				if(PrefC.HasClinicsEnabled) {
					string clinicAbbr;
					if(!dictClinicAbbrs.TryGetValue(patAgeCur.ClinicNum,out clinicAbbr)) {
						clinicAbbr="";
					}
					row.Cells.Add(clinicAbbr);
				}
				string provAbbr;
				if(!dictProvAbbrs.TryGetValue(patAgeCur.PriProv,out provAbbr)) {
					provAbbr="";
				}
				row.Cells.Add(provAbbr);
				string billTypeName;
				if(!dictBillTypeNames.TryGetValue(patAgeCur.BillingType,out billTypeName)) {
					billTypeName="";
				}
				row.Cells.Add(billTypeName);
				row.Cells.Add(patAgeCur.Bal_0_30.ToString("n"));
				row.Cells.Add(patAgeCur.Bal_31_60.ToString("n"));
				row.Cells.Add(patAgeCur.Bal_61_90.ToString("n"));
				row.Cells.Add(patAgeCur.BalOver90.ToString("n"));
				row.Cells.Add(patAgeCur.BalTotal.ToString("n"));
				row.Cells.Add(patAgeCur.InsEst.ToString("n"));
				row.Cells.Add(patAgeCur.AmountDue.ToString("n"));
				row.Cells.Add(patAgeCur.PayPlanDue.ToString("n"));
				row.Cells.Add(patAgeCur.DateLastPay.Year>1880?patAgeCur.DateLastPay.ToString("d"):"");
				TsiTransLog suspendLog=patAgeCur.ListTsiLogs.Find(x => x.TransType==TsiTransType.SS);
				row.Cells.Add(suspendLog!=null?suspendLog.TransDateTime.ToString():"");
				if(patAgeCur.Birthdate.Year<1880//invalid bday
					|| patAgeCur.Birthdate>DateTime.Today.AddYears(-18)//under 18 years old
					|| new[] { patAgeCur.Address,patAgeCur.City,patAgeCur.State,patAgeCur.Zip }.Any(x => string.IsNullOrWhiteSpace(x)))//missing address information
				{
					//color row light red/pink, using cell color so selecting row still shows color
					row.Cells.OfType<ODGridCell>().ToList().ForEach(x => x.CellColor=Color.FromArgb(255,255,230,234));
				}
				row.Tag=patAgeCur;
				gridUnsent.Rows.Add(row);
				if(listSelectedPatNums.Contains(patAgeCur.PatNum)) {
					listIndicesToReselect.Add(gridUnsent.Rows.Count-1);
				}
			}
			gridUnsent.EndUpdate();
			#endregion Fill Grid Rows
			groupPlaceAccounts.Enabled=(gridUnsent.Rows.Count>0);
			if(listIndicesToReselect.Count>0) {
				gridUnsent.SetSelected(listIndicesToReselect.ToArray(),true);
			}
			//We want to line up the totals textbox with their corresponding grid column. Since the columns resize dynamically, we need to move the
			//location of the textboxes.
			Dictionary<string,Tuple<int,int>> dictColPosAndWidth=GetXPosAndWidths(gridUnsent);
			labelUnsentTotals.Location=new Point(dictColPosAndWidth["0-30 Days"].Item1-labelUnsentTotals.Width-1,labelUnsentTotals.Location.Y);
			textUnsent0to30.Location=new Point(dictColPosAndWidth["0-30 Days"].Item1,textUnsent0to30.Location.Y);
			textUnsent0to30.Width=dictColPosAndWidth["0-30 Days"].Item2;
			textUnsent31to60.Location=new Point(dictColPosAndWidth["31-60 Days"].Item1,textUnsent31to60.Location.Y);
			textUnsent31to60.Width=dictColPosAndWidth["31-60 Days"].Item2;
			textUnsent61to90.Location=new Point(dictColPosAndWidth["61-90 Days"].Item1,textUnsent61to90.Location.Y);
			textUnsent61to90.Width=dictColPosAndWidth["61-90 Days"].Item2;
			textUnsentOver90.Location=new Point(dictColPosAndWidth["> 90 Days"].Item1,textUnsentOver90.Location.Y);
			textUnsentOver90.Width=dictColPosAndWidth["> 90 Days"].Item2;
			textUnsentTotal.Location=new Point(dictColPosAndWidth["Total"].Item1,textUnsentTotal.Location.Y);
			textUnsentTotal.Width=dictColPosAndWidth["Total"].Item2;
			textUnsentInsEst.Location=new Point(dictColPosAndWidth["-Ins Est"].Item1,textUnsentInsEst.Location.Y);
			textUnsentInsEst.Width=dictColPosAndWidth["-Ins Est"].Item2;
			textUnsentPatient.Location=new Point(dictColPosAndWidth["=Patient"].Item1,textUnsentPatient.Location.Y);
			textUnsentPatient.Width=dictColPosAndWidth["=Patient"].Item2;
			textUnsentPayPlanDue.Location=new Point(dictColPosAndWidth["PayPlan Due"].Item1,textUnsentPayPlanDue.Location.Y);
			textUnsentPayPlanDue.Width=dictColPosAndWidth["PayPlan Due"].Item2;
			textUnsentTotalNumAccts.Text=listPatAgeFiltered.Count.ToString();
			textUnsent0to30.Text=listPatAgeFiltered.Sum(x => x.Bal_0_30).ToString("n");
			textUnsent31to60.Text=listPatAgeFiltered.Sum(x => x.Bal_31_60).ToString("n");
			textUnsent61to90.Text=listPatAgeFiltered.Sum(x => x.Bal_61_90).ToString("n");
			textUnsentOver90.Text=listPatAgeFiltered.Sum(x => x.BalOver90).ToString("n");
			textUnsentTotal.Text=listPatAgeFiltered.Sum(x => x.BalTotal).ToString("n");
			textUnsentInsEst.Text=listPatAgeFiltered.Sum(x => x.InsEst).ToString("n");
			textUnsentPatient.Text=listPatAgeFiltered.Sum(x => x.AmountDue).ToString("n");
			textUnsentPayPlanDue.Text=listPatAgeFiltered.Sum(x => x.PayPlanDue).ToString("n");
			Cursor=Cursors.Default;
		}

		private List<PatAging> GetListPatAgingUnsentFiltered() {
			List<PatAging> retval=new List<PatAging>();
			#region Validate Inputs
			if(textUnsentMinBal.errorProvider1.GetError(textUnsentMinBal)!="" || textUnsentDaysLastPay.errorProvider1.GetError(textUnsentDaysLastPay)!="") {
				MsgBox.Show(this,"Please fix data entry errors in Unsent tab first.");
				return retval;//return empty list, filter inputs cannot be applied since there are errors
			}
			#endregion Validate Inputs
			#region Get Filter Data
			double minBalance=Math.Round(PIn.Double(textUnsentMinBal.Text),3);
			DateTime dtLastPay=DateTime.Today.AddDays(-PIn.Int(textUnsentDaysLastPay.Text));
			AgeOfAccount accountAge=new[] { AgeOfAccount.Any,AgeOfAccount.Over30,AgeOfAccount.Over60,AgeOfAccount.Over90 }[comboUnsentAccountAge.SelectedIndex];
			List<long> listBillTypes=new List<long>();
			if(!comboBoxMultiBillTypes.ListSelectedIndices.Contains(0)) {
				listBillTypes=comboBoxMultiBillTypes.ListSelectedIndices.Select(x => _listBillTypesNoColl[x-1].DefNum).ToList();
			}
			List<long> listProvNums=new List<long>();
			if(!comboBoxMultiUnsentProvs.ListSelectedIndices.Contains(0)) {
				listProvNums=comboBoxMultiUnsentProvs.ListSelectedIndices.Select(x => _listProviders[x-1].ProvNum).ToList();
			}
			List<long> listClinicNums=new List<long>();
			if(PrefC.HasClinicsEnabled) {
				if(comboBoxMultiUnsentClinics.ListSelectedIndices.Contains(0)) {
					listClinicNums=_listClinics.Select(x => x.ClinicNum).ToList();
				}
				else {
					//x-1 works because we know index 0 isn't selected(from above contains(0)) and we -1 for All clinics
					listClinicNums=comboBoxMultiUnsentClinics.ListSelectedIndices.Select(x => _listClinics[x-1].ClinicNum).ToList();
				}
			}
			#endregion Get Filter Data
			#region Apply Filter Data to PatAging List
			retval=_listPatAgingUnsentAll.FindAll(x =>
				Math.Round(x.AmountDue,3) >= minBalance
				&& (dtLastPay.Date>=DateTime.Today.Date || x.DateLastPay.Date<dtLastPay.Date)
				&& (listBillTypes.Count==0 || listBillTypes.Contains(x.BillingType))
				&& (listProvNums.Count==0 || listProvNums.Contains(x.PriProv))
				&& (listClinicNums.Count==0 || listClinicNums.Contains(x.ClinicNum))
				&& (!checkExcludeBadAddress.Checked || new[] { x.Address,x.City,x.State,x.Zip }.All(y => !string.IsNullOrWhiteSpace(y)))
				&& (!checkExcludeIfProcs.Checked || !x.HasUnsentProcs)
				&& (!checkExcludeInsPending.Checked || !x.HasInsPending)
				&& ( ((int)accountAge < 4 && x.BalOver90 > 0.005)//if Any, Over30, Over60 or Over90 are selected, check BalOver90
					|| ((int)accountAge < 3 && x.Bal_61_90 > 0.005)//if Any, Over30 or Over60 are selected, check Bal_61_90
					|| ((int)accountAge < 2 && x.Bal_31_60 > 0.005)//if Any or Over30 are selected, check Bal_31_60
					|| (int)accountAge < 1 ));//or if Any bal is selected
			#endregion Apply Filter Data to PatAging List
			return retval;
		}

		private void gridUnsentMain_MouseDown(object sender,MouseEventArgs e) {
			if(e.Button==MouseButtons.Right){
				gridUnsent.SetSelected(false);
			}
			else {
				_listSelectedPatNums=gridUnsent.SelectedGridRows.Select(x => ((PatAging)x.Tag).PatNum).ToList();
			}
		}

		private void gridUnsentMain_OnSortByColumn(object sender,EventArgs e) {
			if(_listSelectedPatNums!=null && _listSelectedPatNums.Count>0) {
				gridUnsent.SetSelected(_listSelectedPatNums.Select(
						x => gridUnsent.Rows.ToList().FindIndex(y => x==((PatAging)y.Tag).PatNum))//Find all possible indexes within the grid.
						.Where(x => x > -1)//Ignore any entries within _listSelectedPatNums that were not found in our grid.
						.ToArray()
					,true);
			}
		}

		private void gridUnsent_MouseMove(object sender,MouseEventArgs e) {
			try {
				if(_lastCursorPos==e.Location) {
					return;
				}
				if(e.Button!=MouseButtons.None) {
					_toolTipUnsentErrors.RemoveAll();
					return;
				}
				int colIndex=gridUnsent.PointToCol(e.X);
				if(colIndex<0) {
					return;
				}
				int rowIndex=gridUnsent.PointToRow(e.Y);
				if(rowIndex<0) {
					return;
				}
				int lastRowIndex=gridUnsent.PointToRow(_lastCursorPos.Y);
				if(rowIndex==lastRowIndex) {
					return;
				}
				PatAging pAgeCur=(PatAging)gridUnsent.Rows[rowIndex].Tag;
				if(pAgeCur==null) {
					_toolTipUnsentErrors.RemoveAll();
					return;
				}
				List<string> listErrors=new List<string>();
				if(pAgeCur.Birthdate.Year<1880 || pAgeCur.Birthdate>DateTime.Today.AddYears(-18)) {
					listErrors.Add(Lan.g(this,"Birthdate"));
				}
				if(new[] { pAgeCur.Address,pAgeCur.City,pAgeCur.State,pAgeCur.Zip }.Any(x => string.IsNullOrWhiteSpace(x))) {
					listErrors.Add(Lan.g(this,"Address"));
				}
				if(listErrors.Count==0) {
					_toolTipUnsentErrors.RemoveAll();
					return;
				}
				_toolTipUnsentErrors.SetToolTip(gridUnsent,Lan.g(this,"Invalid")+" "+string.Join(" "+Lan.g(this,"and")+" ",listErrors));
			}
			catch(Exception ex) {
				_toolTipUnsentErrors.RemoveAll();
				ex.DoNothing();
			}
			finally {
				_lastCursorPos=e.Location;
			}
		}

		private void comboBoxMultiUnsentClinics_Leave(object sender,EventArgs e) {
			if(comboBoxMultiUnsentClinics.SelectedIndices.Contains(0)) {
				comboBoxMultiUnsentClinics.SelectedIndicesClear();
			}
			if(comboBoxMultiUnsentClinics.SelectedIndices.Count==0) {
				comboBoxMultiUnsentClinics.SetSelected(0,true);
			}
		}

		private void comboBoxMultiUnsentProvs_Leave(object sender,EventArgs e) {
			if(comboBoxMultiUnsentProvs.SelectedIndices.Contains(0)) {
				comboBoxMultiUnsentProvs.SelectedIndicesClear();
			}
			if(comboBoxMultiUnsentProvs.SelectedIndices.Count==0) {
				comboBoxMultiUnsentProvs.SetSelected(0,true);
			}
		}

		private void comboBoxMultiBillTypes_Leave(object sender,EventArgs e) {
			if(comboBoxMultiBillTypes.SelectedIndices.Contains(0)) {
				comboBoxMultiBillTypes.SelectedIndicesClear();
			}
			if(comboBoxMultiBillTypes.SelectedIndices.Count==0) {
				comboBoxMultiBillTypes.SetSelected(0,true);
			}
		}

		private void FillDemandTypes() {
			TsiDemandType selectedType=comboDemandType.SelectedTag<TsiDemandType>();//If they have nothing selected, this will default to 'Accelerator'.
			List<long> listClinicNums=new List<long>();
			if(!PrefC.HasClinicsEnabled || comboBoxMultiUnsentClinics.ListSelectedIndices.Contains(0)) {
				listClinicNums=_listClinics.Select(x => x.ClinicNum).ToList();//if clinics are disabled, this will only contain the HQ "clinic"
			}
			else {
				//x-1 works because we know index 0 isn't selected(from above contains(0)) and we -1 due to the "All"" clinic
				listClinicNums=comboBoxMultiUnsentClinics.ListSelectedIndices.Select(x => _listClinics[x-1].ClinicNum).ToList();
			}
			comboDemandType.Items.Clear();
			Dictionary<long,string[]> dictClinicSelectedServices=_dictClinicProgProps
				.Where(x => x.Value.Any(y => y.PropertyDesc=="SelectedServices" && !string.IsNullOrEmpty(y.PropertyValue)))
				.ToDictionary(x => x.Key,x => x.Value.Find(y => y.PropertyDesc=="SelectedServices").PropertyValue.Split(','));
			if(listClinicNums.Any(x => dictClinicSelectedServices.ContainsKey(x) 
				&& dictClinicSelectedServices[x].Contains(((int)TsiDemandType.Accelerator).ToString()))) 
			{
				comboDemandType.Items.Add(new ODBoxItem<TsiDemandType>(TsiDemandType.Accelerator.GetDescription(),TsiDemandType.Accelerator));
			}
			if(listClinicNums.Any(x => dictClinicSelectedServices.ContainsKey(x) 
				&& dictClinicSelectedServices[x].Contains(((int)TsiDemandType.ProfitRecovery).ToString()))) 
			{
				comboDemandType.Items.Add(new ODBoxItem<TsiDemandType>(TsiDemandType.ProfitRecovery.GetDescription(),TsiDemandType.ProfitRecovery));
			}
			if(listClinicNums.Any(x => dictClinicSelectedServices.ContainsKey(x) 
				&& dictClinicSelectedServices[x].Contains(((int)TsiDemandType.Collection).ToString()))) 
			{
				comboDemandType.Items.Add(new ODBoxItem<TsiDemandType>(TsiDemandType.Collection.GetDescription(),TsiDemandType.Collection));
			}
			if(comboDemandType.Items.Count>0) {
				comboDemandType.SetSelectedItem<TsiDemandType>(x => x==selectedType,"");
			}
		}

		private void comboBoxMultiUnsentClinics_SelectionChangeCommitted(object sender,EventArgs e) {
			FillDemandTypes();
			FillGridUnsent();
		}

		private void comboBoxMultiUnsentProvs_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGridUnsent();
		}

		private void comboBoxMultiUnsentBillTypes_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGridUnsent();
		}

		private void comboUnsentAccountAge_SelectedIndexChanged(object sender,EventArgs e) {
			FillGridUnsent();
		}

		private void textUnsentMinBal_TextChanged(object sender,EventArgs e) {
			timerFillGrid.Enabled=false;
			timerFillGrid.Enabled=true;
		}

		private void textUnsentDaysLastPay_TextChanged(object sender,EventArgs e) {
			timerFillGrid.Enabled=false;
			timerFillGrid.Enabled=true;
		}

		private void checkExcludeInsPending_CheckedChanged(object sender,EventArgs e) {
			FillGridUnsent();
		}

		private void checkExcludeIfProcs_CheckedChanged(object sender,EventArgs e) {
			FillGridUnsent();
		}

		private void checkExcludeBadAddress_CheckedChanged(object sender,EventArgs e) {
			FillGridUnsent();
		}

		private void butUnsentSaveDefault_Click(object sender,EventArgs e) {
			SaveDefaults();
		}

		private void butUnsentAll_Click(object sender,EventArgs e) {
			gridUnsent.SetSelected(true);
		}

		private void butUnsentNone_Click(object sender,EventArgs e) {
			gridUnsent.SetSelected(false);
		}

		private void butUnsentPrint_Click(object sender,EventArgs e) {
//TODO
		}

		private void butRunAging_Click(object sender,EventArgs e) {
			RunAgingIfNecessary();
			RefreshAll();
			FillGridUnsent();
		}

		private void butSend_Click(object sender,EventArgs e) {
			#region Get and Validate Data
			if(!Security.IsAuthorized(Permissions.Billing)) {
				return;
			}
			if(gridUnsent.SelectedIndices.Length<1) {
				MsgBox.Show(this,"Please select accounts to send to TSI first.");
				return;
			}
			if(_collectionBillType==null) {
				if(Security.IsAuthorized(Permissions.Setup)
					&& MsgBox.Show(this,MsgBoxButtons.YesNo,"There must be a collections billing type defined in order to send accounts to TSI.  Would you like "
						+"to open the definitions window now to create a collections billing type?"))
				{
					FormDefinitions FormDefs=new FormDefinitions(DefCat.BillingTypes);
					FormDefs.ShowDialog();//no OK button, only Close which returns DialogResult.Cancel, just get the billing type again in case they created it
					_collectionBillType=Defs.GetDefsForCategory(DefCat.BillingTypes,true).FirstOrDefault(x => x.ItemValue.ToLower()=="c");
				}
				FormDefinitions FormD=new FormDefinitions(DefCat.BillingTypes);
				FormD.ShowDialog();//no OK button, only Close which returns DialogResult.Cancel, just get the billing type again in case they created it
				_collectionBillType=Defs.GetDefsForCategory(DefCat.BillingTypes,true).FirstOrDefault(x => x.ItemValue.ToLower()=="c");
				if(_collectionBillType==null) {//still no collections billing type
					MsgBox.Show(this,"Please create a collections billing type and try again later.");
					return;
				}
			}
			if(_tsiProg==null) {
				MsgBox.Show(this,"The Transworld program link does not exist.  Please contact support.");
				return;
			}
			if(!_tsiProg.Enabled) {
				MsgBox.Show(this,"The Transworld program link is not enabled.");
				return;
			}
			if(_dictClinicProgProps.Count==0) {
				MsgBox.Show(this,"The Transworld program link is not setup.  Try again after entering the program link properties.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			List<PatAging> listPatAging=gridUnsent.SelectedGridRows.Select(x => x.Tag as PatAging).ToList();
			List<long> listClinicsSkipped=new List<long>();
			foreach(long clinicNum in listPatAging.Select(x => x.ClinicNum).Distinct()) {
				if(!PrefC.HasClinicsEnabled && clinicNum>0) {
					continue;//Only test the HQ clinic (ClinicNum=0) if clinics are not enabled
				}
				List<ProgramProperty> listProgProps;
				if(!_dictClinicProgProps.TryGetValue(clinicNum,out listProgProps) || !TsiTransLogs.ValidateClinicSftpDetails(listProgProps)) {
					listClinicsSkipped.Add(clinicNum);
				}
			}
			if(PrefC.HasClinicsEnabled) {
				if(listClinicsSkipped.Contains(0)) {
					listClinicsSkipped.AddRange(_listClinics.FindAll(x => !_dictClinicProgProps.ContainsKey(x.ClinicNum)).Select(x => x.ClinicNum));
				}
				else if(!Security.CurUser.ClinicIsRestricted && _dictClinicProgProps.ContainsKey(0)) {
					//if clinics are enabled and the user is not restricted, any clinic without prog props will use the HQ prog props for the sftp connection
					_listClinics.FindAll(x => !_dictClinicProgProps.ContainsKey(x.ClinicNum)).ForEach(x => _dictClinicProgProps[x.ClinicNum]=_dictClinicProgProps[0]);
				}
			}
			if(_dictClinicProgProps.All(x => listClinicsSkipped.Contains(x.Key))) {
				Cursor=Cursors.Default;
				MsgBox.Show(this,"An SFTP connection could not be made using the connection details "+(PrefC.HasClinicsEnabled ? "for any clinic " : "")
					+"in the enabled Transworld (TSI) program link.  Accounts cannot be sent to collection until the program link is setup.");
				return;
			}
			//TSI connection details validated, at least one clinic the user has access to is setup with valid connection details
			#region Get Age of Accounts Dictionary
			DateTime dateAsOf=DateTime.Today;//used to determine when the balance on this date began
			if(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)) {//if aging calculated monthly, use the last aging date instead of today
				dateAsOf=PrefC.GetDate(PrefName.DateLastAging);
			}
			//make lookup dict of key=PatNum, value=DateBalBegan
			Dictionary<long,DateTime> dictPatNumDateBalBegan=Ledgers.GetDateBalanceBegan(listPatAging,dateAsOf,false).Select()
				.ToDictionary(x => PIn.Long(x["PatNum"].ToString()),x => PIn.Date(x["DateAccountAge"].ToString()));
			#endregion Get PatAgings and Age of Accounts Dictionary
			#region Validate Selected Pats and Demand Type
			Dictionary<long,string[]> dictClinicSelectedServices=_dictClinicProgProps
				.Where(x => x.Value.Any(y => y.PropertyDesc=="SelectedServices" && !string.IsNullOrEmpty(y.PropertyValue)))
				.ToDictionary(x => x.Key,x => x.Value.Find(y => y.PropertyDesc=="SelectedServices").PropertyValue.Split(','));
			TsiDemandType demandType=comboDemandType.SelectedTag<TsiDemandType>();
			List<long> listPatNumsToReselect=listPatAging.FindAll(x => !dictClinicSelectedServices.ContainsKey(PrefC.HasClinicsEnabled?x.ClinicNum:0)
						|| !dictClinicSelectedServices[PrefC.HasClinicsEnabled?x.ClinicNum:0].Contains(((int)demandType).ToString())).Select(x => x.PatNum).ToList();
			string msgTxt="";
			if(listPatNumsToReselect.Count > 0)	{
				Cursor=Cursors.Default;
				msgTxt=Lan.g(this,"At least one of the selected guarantors is assigned to a clinic that does not have the")+" "+demandType.GetDescription()
					+" "+Lan.g(this,"service enabled.  Those account(s) will not be sent to TSI and will remain in the unsent grid.");
				MessageBox.Show(msgTxt);
			}
			List<long> listPatNumsWrongService=new List<long>();
			if(demandType==TsiDemandType.Accelerator) {
				listPatNumsWrongService=dictPatNumDateBalBegan.Where(x => !listPatNumsToReselect.Contains(x.Key) && x.Value.Date<dateAsOf.AddDays(-120).Date)
					.Select(x => x.Key).ToList();
				msgTxt=Lan.g(this,"The accelerator service is recommended for accounts 31-120 days old and one or more of the selected account balances "
					+"is over 120 days old.  You may wish to consider placing those accounts directly to the Profit Recovery or Collection service.")+"\r\n"
					+Lan.g(this,"Would you like to send the accounts to Accelerator?")+"\r\n\r\n"+Lan.g(this,"Press Yes to send all accounts to Accelerator.")
					+"\r\n\r\n"+Lan.g(this,"Press No to send only the accounts 120 days old or less to Accelerator and leave the older accounts in the unsent "
					+"grid to send to Profit Recovery or Collection later.")+"\r\n\r\n"+Lan.g(this,"Press Cancel to cancel sending all accounts.");
			}
			else if(demandType==TsiDemandType.ProfitRecovery) {
				listPatNumsWrongService=dictPatNumDateBalBegan.Where(x => !listPatNumsToReselect.Contains(x.Key) && x.Value.Date<dateAsOf.AddDays(-180).Date)
					.Select(x => x.Key).ToList();
				msgTxt=Lan.g(this,"The Profit Recovery service is recommended for accounts 121-180 days old and one or more of the selected account "
					+"balances is over 180 days old.  You may wish to consider placing those accounts directly to the Collection service.")+"\r\n"
					+Lan.g(this,"Would you like to send the accounts to Profit Recovery?")+"\r\n\r\n"+Lan.g(this,"Press Yes to send all accounts to Profit "
					+"Recovery.")+"\r\n\r\n"+Lan.g(this,"Press No to send only the accounts 180 days old or less to Profit Recovery and leave the older "
					+"accounts in the unsent grid to send to Collections later.")+"\r\n\r\n"+Lan.g(this,"Press Cancel to cancel sending all accounts.");
			}
			if(listPatNumsWrongService.Count > 0) {
				Cursor=Cursors.Default;
				switch(MessageBox.Show(msgTxt,"",MessageBoxButtons.YesNoCancel)) {
					case DialogResult.No:
						listPatNumsToReselect.AddRange(listPatNumsWrongService);
						break;
					case DialogResult.Cancel:
						return;
					default:
						break;
				}
			}
			#endregion Validate Selected Pats and Demand Type
			#region Validate Birthdate and Address
			List<string> listErrorMsgs=new List<string>();
			List<long> listPatNumsBadBday=listPatAging
				.FindAll(x => !listPatNumsToReselect.Contains(x.PatNum) && (x.Birthdate.Year<1880 || x.Birthdate>DateTime.Today.AddYears(-18)))
				.Select(x => x.PatNum).ToList();
			if(listPatNumsBadBday.Count>0) {
				listErrorMsgs.Add(Lan.g(this,"Invalid birthdate or under the age of 18"));
				listPatNumsToReselect.AddRange(listPatNumsBadBday);
			}
			List<long> listPatNumsBadAddress=listPatAging
				.FindAll(x => !listPatNumsToReselect.Contains(x.PatNum)
					&& new[] { x.Address,x.City,x.State,x.Zip }.Any(y => string.IsNullOrEmpty(y)))
				.Select(x => x.PatNum).ToList();
			if(listPatNumsBadAddress.Count>0) {
				listErrorMsgs.Add(Lan.g(this,"Bad address"));
				listPatNumsToReselect.AddRange(listPatNumsBadAddress);
			}
			if(listErrorMsgs.Count>0) {
				Cursor=Cursors.Default;
				msgTxt=Lan.g(this,"One or more of the selected guarantors has the following error(s) and will not be sent to TSI")+":\r\n\r\n"
					+string.Join("\r\n",listErrorMsgs);
				MessageBox.Show(msgTxt);
			}
			#endregion Validate Birthdate and Address
			#region Validate Balances
			List<long> listPatNumsNegBal=listPatAging
				.FindAll(x => !listPatNumsToReselect.Contains(x.PatNum) && Math.Round(x.AmountDue,3) < 0.005)
				.Select(x => x.PatNum).ToList();
			if(listPatNumsNegBal.Count>0) {
				Cursor=Cursors.Default;
				msgTxt=listPatNumsNegBal.Count+" "+Lan.g(this,"of the selected guarantor(s) have a balance less than or equal to 0.  Are you sure you want "
					+"to send the account(s) to TSI?")+"\r\n\r\n"
					+Lan.g(this,"Press Yes to send the account(s) with a balance less than or equal to 0 anyway.")+"\r\n\r\n"
					+Lan.g(this,"Press No to skip the account(s) with a balance less than or equal to 0 and send the remaining account(s) to TSI.")+"\r\n\r\n"
					+Lan.g(this,"Press Cancel to cancel sending all accounts.");
				switch(MessageBox.Show(msgTxt,"",MessageBoxButtons.YesNoCancel)) {
					case DialogResult.No:
						listPatNumsToReselect.AddRange(listPatNumsNegBal);
						break;
					case DialogResult.Cancel:
						return;
					default:
						break;
				}
			}
			#endregion Validate Balances
			Cursor=Cursors.WaitCursor;
			listPatAging.RemoveAll(x => listPatNumsToReselect.Contains(x.PatNum));
			listPatNumsToReselect.ForEach(x => dictPatNumDateBalBegan.Remove(x));
			if(listPatAging.Count==0) {
				int[] selectedRows=listPatNumsToReselect.Select(x => gridUnsent.Rows.ToList().FindIndex(y => x==((PatAging)y.Tag).PatNum)).Where(x => x>-1).ToArray();
				gridUnsent.SetSelected(selectedRows,true);
				Cursor=Cursors.Default;
				return;
			}
			//dictionary key=PatNum, value=dictionary key=Tuple<TsiFKeyType,long>, value=TsiTrans with that type and key for that pat.  Used for placement
			//msgs to keep all trans for a fam associated with the placement msg to determine later if the past account details were changed after being
			//placed for collection with Transworld.
			Dictionary<long,Dictionary<Tuple<TsiFKeyType,long>,TsiTrans>> dictPatTrans=Ledgers.GetDictTransForGuars(listPatAging.Select(x => x.PatNum).ToList());
			#endregion Get and Validate Data
			#region Create Messages and TsiTransLogs
			Dictionary<long,Dictionary<long,string>> dictClinicUpdateMsgs=new Dictionary<long,Dictionary<long,string>>();
			Dictionary<long,Dictionary<long,string>> dictClinicPlacementMsgs=new Dictionary<long,Dictionary<long,string>>();
			Dictionary<long,List<TsiTransLog>> dictClinicNumListTransLogs=new Dictionary<long,List<TsiTransLog>>();
			List<long> listFailedPatNums=new List<long>();
			foreach(PatAging pAgingCur in listPatAging) {
				long clinicNum=PrefC.HasClinicsEnabled?pAgingCur.ClinicNum:0;
				if(listClinicsSkipped.Contains(clinicNum)) {
					listFailedPatNums.Add(pAgingCur.PatNum);
					continue;
				}
				DateTime dateBalBegan=DateTime.MinValue;
				dictPatNumDateBalBegan.TryGetValue(pAgingCur.PatNum,out dateBalBegan);
				string clientID="";
				List<ProgramProperty> listProgProps;
				if(!_dictClinicProgProps.TryGetValue(clinicNum,out listProgProps) && !_dictClinicProgProps.TryGetValue(0,out listProgProps)) {
					listClinicsSkipped.Add(clinicNum);
					listFailedPatNums.Add(pAgingCur.PatNum);
					continue;
				}
				if(demandType==TsiDemandType.Accelerator) {
					clientID=listProgProps.Find(x => x.PropertyDesc=="ClientIdAccelerator")?.PropertyValue??"";
				}
				else {
					clientID=listProgProps.Find(x => x.PropertyDesc=="ClientIdCollection")?.PropertyValue??"";
				}
				try {
					//find most recent account change log less than 50 days ago and if it was a suspend trans send reinstate update msg instead of placement msg
					TsiTransLog logMostRecentAcctChange=pAgingCur.ListTsiLogs
						.Find(x => x.TransType.In(TsiTransType.CN,TsiTransType.PF,TsiTransType.PL,TsiTransType.PT,TsiTransType.RI,TsiTransType.SS)
							&& x.TransDateTime>=DateTime.Today.AddDays(-50));
					if(logMostRecentAcctChange!=null && logMostRecentAcctChange.TransType==TsiTransType.SS) {
						//most recent account change trans was less than 50 days ago and was to suspend the account, so generate and send update message to reinstate
						string updateStatusMsg=TsiMsgConstructor.GenerateUpdate(pAgingCur.PatNum,clientID,TsiTransType.RI,0.00,pAgingCur.AmountDue);
						if(!dictClinicUpdateMsgs.ContainsKey(clinicNum)) {
							dictClinicUpdateMsgs[clinicNum]=new Dictionary<long,string>();
						}
						dictClinicUpdateMsgs[clinicNum].Add(pAgingCur.PatNum,updateStatusMsg);
						if(!dictClinicNumListTransLogs.ContainsKey(clinicNum)) {
							dictClinicNumListTransLogs[clinicNum]=new List<TsiTransLog>();
						}
						dictClinicNumListTransLogs[clinicNum].Add(new TsiTransLog() {
							PatNum=pAgingCur.PatNum,
							UserNum=Security.CurUser.UserNum,
							TransType=TsiTransType.RI,
							//TransDateTime=DateTime.Now,//set on insert, not editable by user
							//DemandType=TsiDemandType.Accelerator,//only used for placement messages
							//ServiceCode=TsiServiceCode.Diplomatic,//only used for placement messages
							ClientId=clientID,
							TransAmt=0.00,
							AccountBalance=pAgingCur.AmountDue,
							FKeyType=TsiFKeyType.None,//only used for account trans updates
							FKey=0,//only used for account trans updates
							RawMsgText=updateStatusMsg,
							DictTransByType=new Dictionary<Tuple<TsiFKeyType,long>,TsiTrans>()//sets string field TransJson to empty string
						});
					}
					else {
						string msg=TsiMsgConstructor.GeneratePlacement(pAgingCur,clientID,dateBalBegan,demandType);
						if(!dictClinicPlacementMsgs.ContainsKey(clinicNum)) {
							dictClinicPlacementMsgs[clinicNum]=new Dictionary<long,string>();
						}
						dictClinicPlacementMsgs[clinicNum].Add(pAgingCur.PatNum,msg);
						TsiTransLog logCur=new TsiTransLog() {
							PatNum=pAgingCur.PatNum,
							UserNum=Security.CurUser.UserNum,
							TransType=TsiTransType.PL,
							//TransDateTime=DateTime.Now,//set on insert, not editable by user
							DemandType=demandType,
							ServiceCode=TsiServiceCode.Diplomatic,
							ClientId=clientID,
							TransAmt=0.00,
							AccountBalance=pAgingCur.AmountDue,
							FKeyType=TsiFKeyType.None,//not used for placement messages
							FKey=0,//not used for placement messages
							RawMsgText=msg,
							DictTransByType=new Dictionary<Tuple<TsiFKeyType,long>,TsiTrans>()//sets string field TransJson to empty string
						};
						Dictionary<Tuple<TsiFKeyType,long>,TsiTrans> dictPatCurTrans;
						if(dictPatTrans.TryGetValue(logCur.PatNum,out dictPatCurTrans)) {
							logCur.DictTransByType=new Dictionary<Tuple<TsiFKeyType,long>,TsiTrans>(dictPatCurTrans);//sets TransJson to Json serialized string
						}
						if(!dictClinicNumListTransLogs.ContainsKey(clinicNum)) {
							dictClinicNumListTransLogs[clinicNum]=new List<TsiTransLog>();
						}
						dictClinicNumListTransLogs[clinicNum].Add(logCur);
						if(!logCur.AccountBalance.IsEqual(logCur.DictTransByType.Sum(x => x.Value.TranAmt))) {
							throw new ApplicationException("The guarantor's amount due does not match the sum of ledger transactions.  The following guarantor was "
								+"not sent to Transworld.  Try running aging and/or Database Maintenance and then try sending this guarantor again.\r\n"
								+"Patient: "+pAgingCur.PatNum+" - "+pAgingCur.PatName);
						}
					}
				}
				catch(ApplicationException ex) {
					listFailedPatNums.Add(pAgingCur.PatNum);
					if(dictClinicUpdateMsgs.ContainsKey(clinicNum)) {
						dictClinicUpdateMsgs[clinicNum].Remove(pAgingCur.PatNum);
					}
					if(dictClinicPlacementMsgs.ContainsKey(clinicNum)) {
						dictClinicPlacementMsgs[clinicNum].Remove(pAgingCur.PatNum);
					}
					if(dictClinicNumListTransLogs.ContainsKey(clinicNum)) {
						dictClinicNumListTransLogs[clinicNum].RemoveAll(x => x.PatNum==pAgingCur.PatNum);
					}
					Cursor=Cursors.Default;
					if(MsgBox.Show(this,MsgBoxButtons.YesNo,ex.Message+"\r\nDo you want to continue attempting to send the remaining accounts?")) {
						Cursor=Cursors.WaitCursor;
						continue;
					}
					else {
						break;
					}
				}
			}
			#endregion Create Messages and TsiTransLogs
			#region Send Clinic Batch Placement Files, Insert TsiTransLogs, and Update Patient Billing Types
			foreach(KeyValuePair<long,Dictionary<long,string>> kvp in dictClinicPlacementMsgs) {
				if(kvp.Value.Count<1) {
					continue;
				}
				List<ProgramProperty> listProps=new List<ProgramProperty>();
				if(!_dictClinicProgProps.TryGetValue(kvp.Key,out listProps) && !_dictClinicProgProps.TryGetValue(0,out listProps)) {
					//should never happen, dictClinicProps should contain all clinicNums the user has access to, including clinicnum 0
					listFailedPatNums.AddRange(kvp.Value.Keys);
					continue;
				}
				string sftpAddress=listProps.Find(x => x.PropertyDesc=="SftpServerAddress")?.PropertyValue??"";
				int sftpPort;
				if(!int.TryParse(listProps.Find(x => x.PropertyDesc=="SftpServerPort")?.PropertyValue??"",out sftpPort)) {
					sftpPort=22;//default to port 22
				}
				string userName=listProps.Find(x => x.PropertyDesc=="SftpUsername")?.PropertyValue??"";
				string userPassword=listProps.Find(x => x.PropertyDesc=="SftpPassword")?.PropertyValue??"";
				byte[] fileContents=Encoding.ASCII.GetBytes(TsiMsgConstructor.GetPlacementFileHeader()+"\r\n"+string.Join("\r\n",kvp.Value.Values));
				try {
					TaskStateUpload state=new Sftp.Upload(sftpAddress,userName,userPassword,sftpPort) {
						Folder="/xfer/incoming",
						FileName="TsiPlacements_"+DateTime.Now.ToString("yyyyMMddhhmmss")+".txt",
						FileContent=fileContents,
						HasExceptions=true
					};
					state.Execute(false);
				}
				catch(Exception ex) {
					ex.DoNothing();
					listFailedPatNums.AddRange(kvp.Value.Keys);
					continue;
				}
				//Upload was successful
				List<TsiTransLog> listLogsForInsert=new List<TsiTransLog>();
				//dictClinicNumListTransLogs should always contain the same clinicNums as dictClinicMsgs, so this should always insert the messages,
				//i.e. TryGetValue never returns false
				if(dictClinicNumListTransLogs.TryGetValue(kvp.Key,out listLogsForInsert)) {
					TsiTransLogs.InsertMany(listLogsForInsert);
				}
				Patients.UpdateAllFamilyBillingTypes(_collectionBillType.DefNum,kvp.Value.Keys.ToList());//mark all family members as sent to collection
			}
			#endregion Send Clinic Batch Placement Files, Insert TsiTransLogs, and Update Patient Billing Types
			#region Send Clinic Batch Update Files, Insert TsiTransLogs, and Update Patient Billing Types
			foreach(KeyValuePair<long,Dictionary<long,string>> kvp in dictClinicUpdateMsgs) {
				if(kvp.Value.Count<1) {
					continue;
				}
				List<ProgramProperty> listProps=new List<ProgramProperty>();
				if(!_dictClinicProgProps.TryGetValue(kvp.Key,out listProps) && !_dictClinicProgProps.TryGetValue(0,out listProps)) {
					//should never happen, dictClinicProps should contain all clinicNums the user has access to, including clinicnum 0
					listFailedPatNums.AddRange(kvp.Value.Keys);
					continue;
				}
				string sftpAddress=listProps.Find(x => x.PropertyDesc=="SftpServerAddress")?.PropertyValue??"";
				int sftpPort;
				if(!int.TryParse(listProps.Find(x => x.PropertyDesc=="SftpServerPort")?.PropertyValue??"",out sftpPort)) {
					sftpPort=22;//default to port 22
				}
				string userName=listProps.Find(x => x.PropertyDesc=="SftpUsername")?.PropertyValue??"";
				string userPassword=listProps.Find(x => x.PropertyDesc=="SftpPassword")?.PropertyValue??"";
				byte[] fileContents=Encoding.ASCII.GetBytes(TsiMsgConstructor.GetUpdateFileHeader()+"\r\n"+string.Join("\r\n",kvp.Value.Values));
				try {
					TaskStateUpload state=new Sftp.Upload(sftpAddress,userName,userPassword,sftpPort) {
						Folder="/xfer/incoming",
						FileName="TsiUpdates_"+DateTime.Now.ToString("yyyyMMddhhmmss")+".txt",
						FileContent=fileContents,
						HasExceptions=true
					};
					state.Execute(false);
				}
				catch(Exception ex) {
					ex.DoNothing();
					listFailedPatNums.AddRange(kvp.Value.Keys);
					continue;
				}
				//Upload was successful
				List<TsiTransLog> listLogsForInsert=new List<TsiTransLog>();
				//dictClinicNumListTransLogs should always contain the same clinicNums as dictClinicMsgs, so this should always insert the messages,
				//i.e. TryGetValue never returns false
				if(dictClinicNumListTransLogs.TryGetValue(kvp.Key,out listLogsForInsert)) {
					TsiTransLogs.InsertMany(listLogsForInsert);
				}
				//update all family billing types to the collection bill type
				Patients.UpdateAllFamilyBillingTypes(_collectionBillType.DefNum,kvp.Value.Keys.ToList());
			}
			#endregion Send Clinic Batch Update Files, Insert TsiTransLogs, and Update Patient Billing Types
			#region FillGrids With Updated Info
			RefreshAll();
			FillGrids();
			#endregion FillGrids With Updated Info
			if(listPatNumsToReselect.Count+listFailedPatNums.Count>0) {
				int[] selectedRows=listPatNumsToReselect.Union(listFailedPatNums)
					.Select(x => gridUnsent.Rows.ToList().FindIndex(y => x==((PatAging)y.Tag).PatNum)).Where(x => x>-1).ToArray();
				gridUnsent.SetSelected(selectedRows,true);
			}
			Cursor=Cursors.Default;
			if(listFailedPatNums.Count>0) {
				MessageBox.Show(listFailedPatNums.Count+" "+Lan.g(this,"accounts did not upload successfully.  They have not been marked as sent to "
					+"TSI and will have to be resent."));
			}
		}

		#endregion Unsent Tab Methods
		#region Sent Tab Methods

		private void FillGridSent() {
			Cursor=Cursors.WaitCursor;
			List<PatAging> listPatAgeFiltered=GetListPatAgingSentFiltered();
			List<long> listSelectedPatNums=gridSent.SelectedGridRows.Select(x => (x.Tag as PatAging).PatNum).ToList();
			#region Set Grid Title and Columns
			gridSent.BeginUpdate();
			gridSent.Columns.Clear();
			gridSent.Columns.Add(new ODGridColumn("Guarantor",PrefC.HasClinicsEnabled?150:250));
			if(PrefC.HasClinicsEnabled) {
				gridSent.Columns.Add(new ODGridColumn("Clinic",100));
			}
			gridSent.Columns.Add(new ODGridColumn("Prov",75));
			gridSent.Columns.Add(new ODGridColumn("0-30 Days",65,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridSent.Columns.Add(new ODGridColumn("31-60 Days",70,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridSent.Columns.Add(new ODGridColumn("61-90 Days",70,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridSent.Columns.Add(new ODGridColumn("> 90 Days",65,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridSent.Columns.Add(new ODGridColumn("Total",55,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridSent.Columns.Add(new ODGridColumn("-Ins Est",55,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridSent.Columns.Add(new ODGridColumn("=Patient",55,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridSent.Columns.Add(new ODGridColumn("PayPlan Due",80,HorizontalAlignment.Right,GridSortingStrategy.AmountParse));
			gridSent.Columns.Add(new ODGridColumn("Last Paid",65,HorizontalAlignment.Center,GridSortingStrategy.DateParse));
			gridSent.Columns.Add(new ODGridColumn("Demand Type",90));
			gridSent.Columns.Add(new ODGridColumn("Last Transaction",184));
			//gridSent.Columns.Add(new ODGridColumn("Date Last Txn",119,HorizontalAlignment.Center,GridSortingStrategy.DateParse));
			//this form initially set to the max allowed (by OD) form size 1246, which is also the minimum size for this form.  If the user resizes the form
			//to be larger, increase each column width by the same ratio to spread out the additional real estate
			gridSent.Columns.OfType<ODGridColumn>().ToList().ForEach(x => x.ColWidth=(int)((float)x.ColWidth*Width/MinimumSize.Width));
			#endregion Set Grid Title and Columns
			#region Fill Grid Rows
			gridSent.Rows.Clear();
			bool isPatNumIncluded=PrefC.GetBool(PrefName.ReportsShowPatNum);
			Dictionary<long,string> dictClinicAbbrs=_listClinics.ToDictionary(x => x.ClinicNum,x => x.Abbr);
			Dictionary<long,string> dictProvAbbrs=_listProviders.ToDictionary(x => x.ProvNum,x => x.Abbr);
			ODGridRow row;
			List<int> listIndicesToReselect=new List<int>();
			foreach(PatAging patAgeCur in listPatAgeFiltered) {
				row=new ODGridRow();
				row.Cells.Add((isPatNumIncluded?(patAgeCur.PatNum.ToString()+" - "):"")+patAgeCur.PatName);
				if(PrefC.HasClinicsEnabled) {
					string clinicAbbr;
					if(!dictClinicAbbrs.TryGetValue(patAgeCur.ClinicNum,out clinicAbbr)) {
						clinicAbbr="";
					}
					row.Cells.Add(clinicAbbr);
				}
				string provAbbr;
				if(!dictProvAbbrs.TryGetValue(patAgeCur.PriProv,out provAbbr)) {
					provAbbr="";
				}
				row.Cells.Add(provAbbr);
				row.Cells.Add(patAgeCur.Bal_0_30.ToString("n"));
				row.Cells.Add(patAgeCur.Bal_31_60.ToString("n"));
				row.Cells.Add(patAgeCur.Bal_61_90.ToString("n"));
				row.Cells.Add(patAgeCur.BalOver90.ToString("n"));
				row.Cells.Add(patAgeCur.BalTotal.ToString("n"));
				row.Cells.Add(patAgeCur.InsEst.ToString("n"));
				row.Cells.Add(patAgeCur.AmountDue.ToString("n"));
				row.Cells.Add(patAgeCur.PayPlanDue.ToString("n"));
				row.Cells.Add(patAgeCur.DateLastPay.Year>1880?patAgeCur.DateLastPay.ToString("d"):"");
				TsiTransLog placementlog=patAgeCur.ListTsiLogs.FirstOrDefault(x => x.TransType==TsiTransType.PL);
				row.Cells.Add(placementlog!=null?placementlog.DemandType.GetDescription():"");
				string lastTransTypeDate="";
				if(patAgeCur.ListTsiLogs.Count>0) {
					lastTransTypeDate=patAgeCur.ListTsiLogs[0].TransType.GetDescription()+" - "+patAgeCur.ListTsiLogs[0].TransDateTime.ToString("g");
				}
				row.Cells.Add(lastTransTypeDate);
				row.Tag=patAgeCur;
				gridSent.Rows.Add(row);
				if(listSelectedPatNums.Contains(patAgeCur.PatNum)) {
					listIndicesToReselect.Add(gridSent.Rows.Count-1);
				}
			}
			gridSent.EndUpdate();
			#endregion Fill Grid Rows
			groupUpdateAccounts.Enabled=(gridSent.Rows.Count>0);
			if(listIndicesToReselect.Count>0) {
				gridSent.SetSelected(listIndicesToReselect.ToArray(),true);
			}
			Dictionary<string,Tuple<int,int>> dictColPosAndWidth=GetXPosAndWidths(gridSent);
			labelSentTotals.Location=new Point(dictColPosAndWidth["0-30 Days"].Item1-labelSentTotals.Width-1,labelSentTotals.Location.Y);
			textSent0to30.Location=new Point(dictColPosAndWidth["0-30 Days"].Item1,textSent0to30.Location.Y);
			textSent0to30.Width=dictColPosAndWidth["0-30 Days"].Item2;
			textSent31to60.Location=new Point(dictColPosAndWidth["31-60 Days"].Item1,textSent31to60.Location.Y);
			textSent31to60.Width=dictColPosAndWidth["31-60 Days"].Item2;
			textSent61to90.Location=new Point(dictColPosAndWidth["61-90 Days"].Item1,textSent61to90.Location.Y);
			textSent61to90.Width=dictColPosAndWidth["61-90 Days"].Item2;
			textSentOver90.Location=new Point(dictColPosAndWidth["> 90 Days"].Item1,textSentOver90.Location.Y);
			textSentOver90.Width=dictColPosAndWidth["> 90 Days"].Item2;
			textSentTotal.Location=new Point(dictColPosAndWidth["Total"].Item1,textSentTotal.Location.Y);
			textSentTotal.Width=dictColPosAndWidth["Total"].Item2;
			textSentInsEst.Location=new Point(dictColPosAndWidth["-Ins Est"].Item1,textSentInsEst.Location.Y);
			textSentInsEst.Width=dictColPosAndWidth["-Ins Est"].Item2;
			textSentPatient.Location=new Point(dictColPosAndWidth["=Patient"].Item1,textSentPatient.Location.Y);
			textSentPatient.Width=dictColPosAndWidth["=Patient"].Item2;
			textSentPayPlanDue.Location=new Point(dictColPosAndWidth["PayPlan Due"].Item1,textSentPayPlanDue.Location.Y);
			textSentPayPlanDue.Width=dictColPosAndWidth["PayPlan Due"].Item2;
			textSentTotalNumAccts.Text=listPatAgeFiltered.Count.ToString();
			textSent0to30.Text=listPatAgeFiltered.Sum(x => x.Bal_0_30).ToString("n");
			textSent31to60.Text=listPatAgeFiltered.Sum(x => x.Bal_31_60).ToString("n");
			textSent61to90.Text=listPatAgeFiltered.Sum(x => x.Bal_61_90).ToString("n");
			textSentOver90.Text=listPatAgeFiltered.Sum(x => x.BalOver90).ToString("n");
			textSentTotal.Text=listPatAgeFiltered.Sum(x => x.BalTotal).ToString("n");
			textSentInsEst.Text=listPatAgeFiltered.Sum(x => x.InsEst).ToString("n");
			textSentPatient.Text=listPatAgeFiltered.Sum(x => x.AmountDue).ToString("n");
			textSentPayPlanDue.Text=listPatAgeFiltered.Sum(x => x.PayPlanDue).ToString("n");
			Cursor=Cursors.Default;
		}

		private List<PatAging> GetListPatAgingSentFiltered() {
			List<PatAging> retval=new List<PatAging>();
			#region Validate Inputs
			if(textSentMinBal.errorProvider1.GetError(textSentMinBal)!="" || textSentDaysLastPay.errorProvider1.GetError(textSentDaysLastPay)!="") {
				MsgBox.Show(this,"Please fix data entry errors in Sent tab first.");//return empty list, filter inputs cannot be applied since there are errors
				return retval;
			}
			#endregion Validate Inputs
			#region Get Filter Data
			double minBalance=Math.Round(PIn.Double(textSentMinBal.Text),3);
			DateTime dtLastPay=DateTime.Today.AddDays(-PIn.Int(textSentDaysLastPay.Text));
			AgeOfAccount accountAge=new[] { AgeOfAccount.Any,AgeOfAccount.Over30,AgeOfAccount.Over60,AgeOfAccount.Over90 }[comboSentAccountAge.SelectedIndex];
			List<TsiTransType> listTranTypes=new List<TsiTransType>();
			if(!comboBoxMultiLastTransType.ListSelectedIndices.Contains(0)) {
				listTranTypes=comboBoxMultiLastTransType.ListSelectedIndices.Select(x => _listSentTabTransTypes[x-1]).ToList();
			}
			List<long> listProvNums=new List<long>();
			if(!comboBoxMultiSentProvs.ListSelectedIndices.Contains(0)) {
				listProvNums=comboBoxMultiSentProvs.ListSelectedIndices.Select(x => _listProviders[x-1].ProvNum).ToList();
			}
			List<long> listClinicNums=new List<long>();
			if(PrefC.HasClinicsEnabled) {
				if(comboBoxMultiSentClinics.ListSelectedIndices.Contains(0)) {
					listClinicNums=_listClinics.Select(x => x.ClinicNum).ToList();
				}
				else {
					//x-1 works because we know index 0 isn't selected(from above contains(0)) and we -1 for All clinics
					listClinicNums=comboBoxMultiSentClinics.ListSelectedIndices.Select(x => _listClinics[x-1].ClinicNum).ToList();
				}
			}
			#endregion Get Filter Data
			#region Apply Filter Data to PatAging List
			retval=_listPatAgingSentAll.FindAll(x =>
				Math.Round(x.AmountDue,3) >= minBalance
				&& (dtLastPay.Date>=DateTime.Today.Date || x.DateLastPay.Date<dtLastPay.Date)
				&& (listTranTypes.Count==0 || x.ListTsiLogs.Count<1 || listTranTypes.Contains(x.ListTsiLogs[0].TransType))
				&& (listProvNums.Count==0 || listProvNums.Contains(x.PriProv))
				&& (listClinicNums.Count==0 || listClinicNums.Contains(x.ClinicNum))
				&& ( ((int)accountAge < 4 && x.BalOver90 > 0.005)//if Any, Over30, Over60 or Over90 are selected, check BalOver90
					|| ((int)accountAge < 3 && x.Bal_61_90 > 0.005)//if Any, Over30 or Over60 are selected, check Bal_61_90
					|| ((int)accountAge < 2 && x.Bal_31_60 > 0.005)//if Any or Over30 are selected, check Bal_31_60
					|| (int)accountAge < 1 ));//or if Any bal is selected
			#endregion Apply Filter Data to PatAging List
			return retval;
		}

		private void gridSentMain_MouseDown(object sender,MouseEventArgs e) {
			if(e.Button==MouseButtons.Right){
				gridSent.SetSelected(false);
			}
			else {
				_listSelectedPatNums=gridSent.SelectedGridRows.Select(x => ((PatAging)x.Tag).PatNum).ToList();
			}
		}

		private void gridSentMain_OnSortByColumn(object sender,EventArgs e) {
			if(_listSelectedPatNums!=null && _listSelectedPatNums.Count>0) {
				gridSent.SetSelected(_listSelectedPatNums.Select(x => gridSent.Rows.ToList().FindIndex(y => x==((PatAging)y.Tag).PatNum)).Where(x => x>-1).ToArray(),true);
			}
		}

		private void comboBoxMultiSentClinics_Leave(object sender,EventArgs e) {
			if(comboBoxMultiSentClinics.SelectedIndices.Contains(0)) {
				comboBoxMultiSentClinics.SelectedIndicesClear();
			}
			if(comboBoxMultiSentClinics.SelectedIndices.Count==0) {
				comboBoxMultiSentClinics.SetSelected(0,true);
			}
		}

		private void comboBoxMultiSentProvs_Leave(object sender,EventArgs e) {
			if(comboBoxMultiSentProvs.SelectedIndices.Contains(0)) {
				comboBoxMultiSentProvs.SelectedIndicesClear();
			}
			if(comboBoxMultiSentProvs.SelectedIndices.Count==0) {
				comboBoxMultiSentProvs.SetSelected(0,true);
			}
		}

		private void comboBoxMultiLastTransType_Leave(object sender,EventArgs e) {
			if(comboBoxMultiLastTransType.SelectedIndices.Contains(0)) {
				comboBoxMultiLastTransType.SelectedIndicesClear();
			}
			if(comboBoxMultiLastTransType.SelectedIndices.Count==0) {
				comboBoxMultiLastTransType.SetSelected(0,true);
			}
		}

		private void comboBoxMultiSentClinics_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGridSent();
		}

		private void comboBoxMultiSentProvs_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGridSent();
		}

		private void comboBoxMultiLastTransType_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGridSent();
		}

		private void comboSentAccountAge_SelectedIndexChanged(object sender,EventArgs e) {
			FillGridSent();
		}

		private void textSentMinBal_TextChanged(object sender,EventArgs e) {
			timerFillGrid.Enabled=false;
			timerFillGrid.Enabled=true;
		}

		private void textSentDaysLastPay_TextChanged(object sender,EventArgs e) {
			timerFillGrid.Enabled=false;
			timerFillGrid.Enabled=true;
		}

		private void comboNewStatus_SelectedIndexChanged(object sender,EventArgs e) {
			if(comboNewStatus.SelectedIndex<0 || comboNewStatus.SelectedIndex>_listNewStatuses.Count-1) {
				return;
			}
			if(comboNewBillType.SelectedIndex<0 || comboNewBillType.SelectedIndex>_listBillTypesNoColl.Count-1) {
				errorProvider1.SetError(comboNewBillType,Lan.g(this,"Select billing type for accounts that will no longer be managed by Transworld."));
			}
		}

		private void comboNewBillType_SelectionChangeCommitted(object sender,EventArgs e) {
			if(comboNewBillType.SelectedIndex<0 || comboNewBillType.SelectedIndex>_listBillTypesNoColl.Count-1) {
				errorProvider1.SetError(comboNewBillType,Lan.g(this,"Select billing type for accounts that will no longer be managed by Transworld."));
				return;
			}
			errorProvider1.SetError(comboNewBillType,"");
		}

		private void butSentSaveDefaults_Click(object sender,EventArgs e) {
			SaveDefaults();
		}

		private void butSentAll_Click(object sender,EventArgs e) {
			gridSent.SetSelected(true);
		}

		private void butSentNone_Click(object sender,EventArgs e) {
			gridSent.SetSelected(false);
		}

		private void butSentPrint_Click(object sender,EventArgs e) {
//TODO
		}

		private void butUpdateStatus_Click(object sender,EventArgs e) {
			#region Get and Validate Data
			if(!Security.IsAuthorized(Permissions.Billing)) {
				return;
			}
			if(comboNewStatus.SelectedIndex<0 || comboNewStatus.SelectedIndex>=_listNewStatuses.Count) {
				MsgBox.Show(this,"Please select a new status first.");
				return;
			}
			if(gridSent.SelectedIndices.Length<1) {
				MsgBox.Show(this,"Please select accounts to update first.");
				return;
			}
			if(_tsiProg==null) {
				MsgBox.Show(this,"The Transworld program link does not exist.  Please contact support.");
				return;
			}
			if(!_tsiProg.Enabled) {
				MsgBox.Show(this,"The Transworld program link is not enabled.");
				return;
			}
			if(_dictClinicProgProps.Count==0) {
				MsgBox.Show(this,"The Transworld program link is not setup.  Try again after entering the program link properties.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			List<PatAging> listPatAging=gridSent.SelectedGridRows.Select(x => x.Tag as PatAging).ToList();
			List<long> listClinicsSkipped=new List<long>();
			foreach(long clinicNum in listPatAging.Select(x => x.ClinicNum).Distinct()) {
				if(!PrefC.HasClinicsEnabled && clinicNum>0) {
					continue;//Only test the HQ clinic (ClinicNum=0) if clinics are not enabled
				}
				List<ProgramProperty> listPropsCur;
				if(!_dictClinicProgProps.TryGetValue(clinicNum,out listPropsCur) || !TsiTransLogs.ValidateClinicSftpDetails(listPropsCur)) {
					listClinicsSkipped.Add(clinicNum);
				}
			}
			if(PrefC.HasClinicsEnabled) {
				if(listClinicsSkipped.Contains(0)) {
					listClinicsSkipped.AddRange(_listClinics.FindAll(x => !_dictClinicProgProps.ContainsKey(x.ClinicNum)).Select(x => x.ClinicNum));
				}
				else if(!Security.CurUser.ClinicIsRestricted && _dictClinicProgProps.ContainsKey(0)) {
					//if clinics are enabled and the user is not restricted, any clinic without prog props will use the HQ prog props for the sftp connection
					_listClinics.FindAll(x => !_dictClinicProgProps.ContainsKey(x.ClinicNum)).ForEach(x => _dictClinicProgProps[x.ClinicNum]=_dictClinicProgProps[0]);
				}
			}
			if(_dictClinicProgProps.All(x => listClinicsSkipped.Contains(x.Key))) {
				Cursor=Cursors.Default;
				MsgBox.Show(this,"An SFTP connection could not be made using the connection details "+(PrefC.HasClinicsEnabled ? "for any clinic " : "")
					+"in the enabled Transworld (TSI) program link.  Account statuses cannot be updated with TSI until the program link is setup.");
				return;
			}
			TsiTransType transType=_listNewStatuses[comboNewStatus.SelectedIndex];
			if(transType==TsiTransType.SS) {
				Cursor=Cursors.Default;
				string msgTxt=Lan.g(this,"The account(s) will be suspended for a maximum of 50 days but only if they are in the Accelerator or Profit Recovery "
					+"stage.  Accounts in the Transworld Systems Collection stage will NOT be suspended and will have to be reinstated from the unsent grid.  "
					+"During the 50 day suspension you may reinstate the account(s) at any time.  However, after 50 days has passed, the account(s) will expire "
					+"and will no longer be available to reinstate.")+"\r\n\r\n"+Lan.g(this,"Do you want to suspend the service for the selected account(s)?");
				if(MessageBox.Show(msgTxt,"",MessageBoxButtons.OKCancel)==DialogResult.Cancel) {
					return;
				}
			}
			if(comboNewBillType.SelectedIndex<0 || comboNewBillType.SelectedIndex>=_listBillTypesNoColl.Count) {
				Cursor=Cursors.Default;
				MsgBox.Show(this,"Please select a new billing type to assign to the guarantors that are no longer going to be managed by Transworld.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			long newBillType=_listBillTypesNoColl[comboNewBillType.SelectedIndex].DefNum;
			#endregion Get and Validate Data
			#region Create Messages and TsiTransLogs
			//TSI connection details validated, at least one clinic the user has access to is setup with valid connection details
			Dictionary<long,Dictionary<long,string>> dictClinicMsgs=new Dictionary<long,Dictionary<long,string>>();
			Dictionary<long,List<TsiTransLog>> dictClinicNumListTransLogs=new Dictionary<long,List<TsiTransLog>>();
			List<long> listFailedPatNums=new List<long>();
			foreach(PatAging pAgingCur in listPatAging) {
				long clinicNum=PrefC.HasClinicsEnabled?pAgingCur.ClinicNum:0;
				if(listClinicsSkipped.Contains(clinicNum)) {
					listFailedPatNums.Add(pAgingCur.PatNum);
					continue;
				}
				List<ProgramProperty> listProgProps;
				if(!_dictClinicProgProps.TryGetValue(clinicNum,out listProgProps) && !_dictClinicProgProps.TryGetValue(0,out listProgProps)) {
					listFailedPatNums.Add(pAgingCur.PatNum);
					listClinicsSkipped.Add(clinicNum);
					continue;
				}
				string clientId="";
				if(pAgingCur.ListTsiLogs.Count>0) {
					clientId=pAgingCur.ListTsiLogs[0].ClientId;
				}
				if(string.IsNullOrEmpty(clientId)) {
					clientId=listProgProps.Find(x => x.PropertyDesc=="ClientIdAccelerator")?.PropertyValue;
				}
				if(string.IsNullOrEmpty(clientId)) {
					clientId=listProgProps.Find(x => x.PropertyDesc=="ClientIdCollection")?.PropertyValue;
				}
				if(string.IsNullOrEmpty(clientId)) {
					listFailedPatNums.Add(pAgingCur.PatNum);
					listClinicsSkipped.Add(clinicNum);
					continue;
				}
				try {
					string msg=TsiMsgConstructor.GenerateUpdate(pAgingCur.PatNum,clientId,transType,0.00,pAgingCur.AmountDue);
					if(!dictClinicMsgs.ContainsKey(clinicNum)) {
						dictClinicMsgs[clinicNum]=new Dictionary<long,string>();
					}
					dictClinicMsgs[clinicNum].Add(pAgingCur.PatNum,msg);
					if(!dictClinicNumListTransLogs.ContainsKey(clinicNum)) {
						dictClinicNumListTransLogs[clinicNum]=new List<TsiTransLog>();
					}
					dictClinicNumListTransLogs[clinicNum].Add(new TsiTransLog() {
						PatNum=pAgingCur.PatNum,
						UserNum=Security.CurUser.UserNum,
						TransType=transType,
						//TransDateTime=DateTime.Now,//set on insert, not editable by user
						//DemandType=TsiDemandType.Accelerator,//only valid for placement msgs
						//ServiceCode=TsiServiceCode.Diplomatic,//only valid for placement msgs
						ClientId=clientId,
						TransAmt=0.00,
						AccountBalance=pAgingCur.AmountDue,
						FKeyType=TsiFKeyType.None,//only used for account trans updates
						FKey=0,//only used for account trans updates
						RawMsgText=msg
						//,TransJson=""//only valid for placement msgs
					});
				}
				catch(ApplicationException ex) {
					listFailedPatNums.Add(pAgingCur.PatNum);
					if(dictClinicMsgs.ContainsKey(clinicNum)) {
						dictClinicMsgs[clinicNum].Remove(pAgingCur.PatNum);
					}
					if(dictClinicNumListTransLogs.ContainsKey(clinicNum)) {
						dictClinicNumListTransLogs[clinicNum].RemoveAll(x => x.PatNum==pAgingCur.PatNum);
					}
					Cursor=Cursors.Default;
					if(MsgBox.Show(this,MsgBoxButtons.YesNo,ex.Message+"\r\nDo you want to continue attempting to send any remaining accounts?")) {
						Cursor=Cursors.WaitCursor;
						continue;
					}
					else {
						break;
					}
				}
			}
			#endregion Create Messages and TsiTransLogs
			#region Send Clinic Batch Files, Insert TsiTransLogs, and Update Patient Billing Types
			foreach(KeyValuePair<long,Dictionary<long,string>> kvp in dictClinicMsgs) {
				if(kvp.Value.Count<1) {
					continue;
				}
				List<ProgramProperty> listProps=new List<ProgramProperty>();
				if(!_dictClinicProgProps.TryGetValue(kvp.Key,out listProps) && !_dictClinicProgProps.TryGetValue(0,out listProps)) {
					//should never happen, dictClinicProps should contain all clinicNums the user has access to, including clinicnum 0
					listFailedPatNums.AddRange(kvp.Value.Keys);
					continue;
				}
				string sftpAddress=listProps.Find(x => x.PropertyDesc=="SftpServerAddress")?.PropertyValue??"";
				int sftpPort;
				if(!int.TryParse(listProps.Find(x => x.PropertyDesc=="SftpServerPort")?.PropertyValue??"",out sftpPort)) {
					sftpPort=22;//default to port 22
				}
				string userName=listProps.Find(x => x.PropertyDesc=="SftpUsername")?.PropertyValue??"";
				string userPassword=listProps.Find(x => x.PropertyDesc=="SftpPassword")?.PropertyValue??"";
				byte[] fileContents=Encoding.ASCII.GetBytes(TsiMsgConstructor.GetUpdateFileHeader()+"\r\n"+string.Join("\r\n",kvp.Value.Values));
				try {
					TaskStateUpload state=new Sftp.Upload(sftpAddress,userName,userPassword,sftpPort) {
						Folder="/xfer/incoming",
						FileName="TsiUpdates_"+DateTime.Now.ToString("yyyyMMddhhmmss")+".txt",
						FileContent=fileContents,
						HasExceptions=true
					};
					state.Execute(false);
				}
				catch(Exception ex) {
					ex.DoNothing();
					listFailedPatNums.AddRange(kvp.Value.Keys);
					continue;
				}
				//Upload was successful
				List<TsiTransLog> listLogsForInsert=new List<TsiTransLog>();
				//dictClinicNumListTransLogs should always contain the same clinicNums as dictClinicMsgs, so this should always insert the messages,
				//i.e. TryGetValue never returns false
				if(dictClinicNumListTransLogs.TryGetValue(kvp.Key,out listLogsForInsert)) {
					TsiTransLogs.InsertMany(listLogsForInsert);
				}
				//update all family billing types to the collection bill type if transtype is reinstated, otherwise to the selected new billing type
				Patients.UpdateAllFamilyBillingTypes((transType==TsiTransType.RI?_collectionBillType.DefNum:newBillType),kvp.Value.Keys.ToList());
			}
			#endregion Send Clinic Batch Files, Insert TsiTransLogs, and Update Patient Billing Types
			RefreshAll();
			FillGrids();
			Cursor=Cursors.Default;
			if(listFailedPatNums.Count>0) {
				MessageBox.Show(listFailedPatNums.Count+" "+Lan.g(this,"accounts did not upload successfully.  They have not been marked as sent to "
					+"collection and will have to be resent."));
				int[] selectedRows=listFailedPatNums.Select(x => gridSent.Rows.ToList().FindIndex(y => x==((PatAging)y.Tag).PatNum)).Where(x => x>-1).ToArray();
				gridSent.SetSelected(selectedRows,true);
			}
		}

		#endregion Sent Tab Methods

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

		private void FormArManager_FormClosing(object sender,FormClosingEventArgs e) {
			timerFillGrid.Enabled=false;
		}

	}
}