using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental.UI {
	public partial class BugSubmissionControl:UserControl {
		///<summary>Currently selected bugsubmission, either passed in from the calling form or from the group seleciton made in the Customer/Grouping grid.</summary>
		private BugSubmission _subCur;
		///<summary>The patient associated to _subCur</summary>
		private Patient _patCur;
		///<summary>Controls if the Customer/Grouping grid is used.</summary>
		private BugSubmissionControlMode _controlMode;
		///<summary>Passed in when grouping logic needs to be considered.</summary>
		private int _groupSelection;
		///<summary>List of bug submissions to be used in teh Customer/Grouping grid. Logic is dependent on _groupSelection value.</summary>
		private List<BugSubmission> _listSubs;
		///<summary>Fires when the user leaves the TextDevNote textbox.</summary>
		public TextDevNote_PostLeave TextDevNoteLeave;
		///<summary>Dictionary of patients that will lazy load as users click on entries.  The key is the Registration Key.</summary>
		private Dictionary<string,Patient> _dictPatients;
		
		public BugSubmissionControlMode ControlMode {
			get {
				return _controlMode;
			}
			set {
				_controlMode=value;
			}
		}

		public BugSubmissionControl() {
			InitializeComponent();
		}
		
		private void BugSubmissionControl_Load(object sender,EventArgs e) {
			switch(_controlMode) {
				case BugSubmissionControlMode.Specific:
					gridOfficeInfo.Height+=((gridCustomerSubs.Location.Y-gridOfficeInfo.Bottom)+gridCustomerSubs.Height);
					gridCustomerSubs.Visible=false;
					break;
				case BugSubmissionControlMode.General:
					break;
			}
		}

		///<summary>Must be called before RefreshView(...) to set the internal data.</summary>
		public void RefreshData(Dictionary<string,Patient> dictPatients,int groupSelection,List<BugSubmission> listSubs) {
			_dictPatients=(dictPatients==null?new Dictionary<string, Patient>():dictPatients);
			_groupSelection=groupSelection;
			_listSubs=listSubs;
		}

		/// <summary>Used to refresh the view to show the given sub information. Make sure and call RefreshData(...) when data has refreshed.</summary>
		/// <param name="sub">The bugSubmission that will be used to fill the UI.</param>
		/// <param name="pat">The bugSubmissions assocated patient, used for linking task and UI.</param>
		/// <param name="groupSelection">When using the control to view many bugSubmissions this is the grouping selection.</param>
		/// <param name="listSubs">The list of bugSubmissions to be considered in teh Customer/Grouping grid based on the groupSelection value.</param>
		public void RefreshView(BugSubmission sub,bool isCustomerGridRefresh=true) {
			_subCur=sub;
			_patCur=(_dictPatients.ContainsKey(_subCur.RegKey)?_dictPatients[_subCur.RegKey]:new Patient());
			FillMainView(_subCur);
			FillOfficeInfoGrid(_subCur);
			SetCustomerInfo(_subCur,isCustomerGridRefresh);
		}

		private void FillMainView(BugSubmission sub) {
			textStack.Text=sub.ExceptionStackTrace;
			textDevNote.Text=sub.DevNote;
		}
		
		private void FillOfficeInfoGrid(BugSubmission sub){
			gridOfficeInfo.BeginUpdate();
			if(gridOfficeInfo.Columns.Count==0) {
				gridOfficeInfo.Columns.Add(new ODGridColumn("Field",130));
				gridOfficeInfo.Columns.Add(new ODGridColumn("Value",125));
			}
			gridOfficeInfo.Rows.Clear();
			if(sub!=null) {
				gridOfficeInfo.Rows.Add(new ODGridRow("Preferences","") { ColorBackG=gridOfficeInfo.HeaderColor,Bold=true,Tag=true });
				List<PrefName> listPrefNames=sub.Info.DictPrefValues.Keys.ToList();
				foreach(PrefName prefName in listPrefNames) {
					ODGridRow row=new ODGridRow();
					row.Cells.Add(prefName.ToString());
					row.Cells.Add(sub.Info.DictPrefValues[prefName]);
					gridOfficeInfo.Rows.Add(row);
				}
				gridOfficeInfo.Rows.Add(new ODGridRow("Other","") { ColorBackG=gridOfficeInfo.HeaderColor,Bold=true,Tag=true });
				gridOfficeInfo.Rows.Add(new ODGridRow("CountClinics",sub.Info.CountClinics.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("EnabledPlugins",string.Join(",",sub.Info.EnabledPlugins?.Select(x => x).ToList()??new List<string>())));
				gridOfficeInfo.Rows.Add(new ODGridRow("ClinicNumCur",sub.Info.ClinicNumCur.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("UserNumCur",sub.Info.UserNumCur.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("PatientNumCur",sub.Info.PatientNumCur.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("ModuleNameCur",sub.Info.ModuleNameCur?.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("IsOfficeOnReplication",sub.Info.IsOfficeOnReplication.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("IsOfficeUsingMiddleTier",sub.Info.IsOfficeUsingMiddleTier.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("WindowsVersion",sub.Info.WindowsVersion?.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("CompName",sub.Info.CompName?.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("PreviousUpdateVersion",sub.Info.PreviousUpdateVersion));
				gridOfficeInfo.Rows.Add(new ODGridRow("PreviousUpdateTime",sub.Info.PreviousUpdateTime.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("ThreadName",sub.Info.ThreadName?.ToString()));
				gridOfficeInfo.Rows.Add(new ODGridRow("DatabaseName",sub.Info.DatabaseName?.ToString()));
			}
			gridOfficeInfo.EndUpdate();
		}

		///<summary>When sub is set, fills customer group box with various information.
		///When null, clears all fields.</summary>
		private void SetCustomerInfo(BugSubmission sub,bool isCustomerGridRefresh=true) {
			try {
				labelCustomerNum.Text=_patCur?.PatNum.ToString()??"";
				labelRegKey.Text=sub.RegKey;
				labelCustomerState.Text=_patCur?.State??"";
				labelCustomerPhone.Text=_patCur?.WkPhone??"";
				labelSubNum.Text=POut.Long(sub.BugSubmissionNum);
				labelLastCall.Text=Commlogs.GetDateTimeOfLastEntryForPat(_patCur?.PatNum??0).ToString();
			}
			catch(Exception ex) {
				ex.DoNothing();
			}
			butGoToAccount.Enabled=true;
			butBugTask.Enabled=true;
			butCompare.Enabled=true;
			if(!isCustomerGridRefresh || _groupSelection==-1 || _listSubs==null || !gridCustomerSubs.Visible) {//Just in case checks.
				return;
			}
			switch(_groupSelection) {
				case 0:
					#region None
					gridCustomerSubs.Title="Customer Submissions";
					gridCustomerSubs.BeginUpdate();
					gridCustomerSubs.Columns.Clear();
					gridCustomerSubs.Columns.Add(new ODGridColumn("Version",100,HorizontalAlignment.Center));
					gridCustomerSubs.Columns.Add(new ODGridColumn("Count",50,HorizontalAlignment.Center));
					gridCustomerSubs.Rows.Clear();
					Dictionary<string,List<BugSubmission>> dictCustomerSubVersions=_listSubs
						.Where(x => x.RegKey==sub.RegKey)
						.GroupBy(x => x.Info.DictPrefValues[PrefName.ProgramVersion])
						.ToDictionary(x => x.Key,x => x.DistinctBy(y => y.ExceptionStackTrace).ToList());
					foreach(KeyValuePair<string,List<BugSubmission>> pair in dictCustomerSubVersions) {
						gridCustomerSubs.Rows.Add(new ODGridRow(pair.Key,pair.Value.Count.ToString()));
					}
					gridCustomerSubs.EndUpdate();
					#endregion
					break;
				case 1:
				case 2:
				case 3:
					#region RegKey/Ver/Stack, Stacktrace, 95%
					gridCustomerSubs.Title="Grouped Submissions ("+_listSubs.DistinctBy(x => x.RegKey).Count()+" DIST)";
					gridCustomerSubs.BeginUpdate();
					gridCustomerSubs.Columns.Clear();
					gridCustomerSubs.Columns.Add(new ODGridColumn("Vers.",55,HorizontalAlignment.Center));
					gridCustomerSubs.Columns.Add(new ODGridColumn("RegKey",140,HorizontalAlignment.Center));
					gridCustomerSubs.Rows.Clear();
					_listSubs.ForEach(x => { 
						ODGridRow row=new ODGridRow(x.Info.DictPrefValues[PrefName.ProgramVersion],x.RegKey);
						row.Tag=x;
						gridCustomerSubs.Rows.Add(row);
					});
					gridCustomerSubs.EndUpdate();
					#endregion
					break;
			}
		}

		public void ClearCustomerInfo() {
			textStack.Text="";//Also clear any submission specific fields.
			labelCustomerNum.Text="";
			labelRegKey.Text="";
			labelCustomerState.Text="";
			labelCustomerPhone.Text="";
			labelSubNum.Text="";
			labelLastCall.Text="";
			FillOfficeInfoGrid(null);
			gridCustomerSubs.BeginUpdate();
			gridCustomerSubs.Rows.Clear();
			gridCustomerSubs.EndUpdate();
			butGoToAccount.Enabled=false;
			butBugTask.Enabled=false;
			butCompare.Enabled=false;
			textDevNote.Text="";
		}

		public void SetTextDevNoteEnabled(bool value) {
			this.textDevNote.Enabled=value;
		}
		
		private void textDevNote_Leave(object sender,EventArgs e) {
			if(_subCur.DevNote==textDevNote.Text) {
				return;
			}
			_subCur.DevNote=textDevNote.Text;
			BugSubmissions.Update(_subCur);
			TextDevNoteLeave?.Invoke(sender,e);
		}
		
		private void gridCustomerSubs_CellClick(object sender,ODGridClickEventArgs e) {
			if(e.Row==-1 || _groupSelection==0) {//0=None
				_subCur=null;
				return;
			}
			_subCur=(BugSubmission)gridCustomerSubs.Rows[e.Row].Tag;
			_patCur=(_dictPatients.ContainsKey(_subCur.RegKey)?_dictPatients[_subCur.RegKey]:new Patient());
			RefreshView(_subCur,false);
		}
		
		private void gridCustomerSubs_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(e.Row==-1 || _groupSelection==0) {//0=None
				return;
			}
			FormBugSubmissions formGroupBugSubs=new FormBugSubmissions(viewMode:FormBugSubmissionMode.ViewOnly);
			formGroupBugSubs.ListViewedSubs=_listSubs;
			formGroupBugSubs.ShowDialog();
		}

		private void butGoToAccount_Click(object sender,EventArgs e) {
			if(_patCur==null) {
				return;
			}
			//Button is only enabled if _patCur is not null.
			GotoModule.GotoAccount(_patCur.PatNum);
		}

		private void butBugTask_Click(object sender,EventArgs e) {
			if(_patCur==null) {
				return;
			}
			BugSubmissionL.CreateTask(_patCur,_subCur);
		}
		
		private void butCompare_Click(object sender,EventArgs e) {
			InputBox input=new InputBox("Please copy/paste your stack trace to compare to this bug.",true);
			if(input.ShowDialog()!=DialogResult.OK) {
				return;
			}
			string perct=POut.Double(BugSubmissionL.CalculateSimilarity(textStack.Text,input.textResult.Text));
			MsgBox.Show(this,perct+"%");
		}
		
		public delegate void TextDevNote_PostLeave(object sender,EventArgs e);

	}

}

public enum BugSubmissionControlMode {
	Specific,
	General
}
