using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormOrthoChart:ODForm {
		private PatField[] _arrayPatientFields;
		private List<DisplayField> _listOrthDisplayFields;
		///<summary>A stale copy of all ortho charts associated to the selected patient.  Filled on load. This will be the same as what's in the
		///database.</summary>
		private List<OrthoChart> _listOrthoChartsInitial;
		private Patient _patCur;
		private PatientNote _patNoteCur;
		private List<string> _listDisplayFieldNames;
		///<summary>Set to true if any data changed within the grid.</summary>
		private bool _hasChanged;
		///<summary>This dictionary contains one row per date, and then a list of OrthoChart objects, one for each column.</summary>
		private SortedDictionary<DateTime,List<OrthoChart>> _dictOrthoCharts;
		///<summary>True if there are any ortho display fields with the internal name of "Signature"</summary>
		private bool _showSigBox;
		///<summary>Keeps track of the column index of the Signature field if one is present.
		///This column index represents the the index of the grid that is displayed to the user, not the index within _tableOrtho.</summary>
		private int _sigColIdx=-1;
		///<summary>Keeps track of the column index within _tableOrtho of the Signature field if one is present.</summary>
		private int _sigTableOrthoColIdx=-1;
		///<summary>The index of the previously selected ortho chart row.  This is used to help save data.</summary>
		private int _prevRow;
		private bool _topazNeedsSaved;
		private int _indexInitialTab;
		///<summary>Dates that have been newly added. Also includes today's date.</summary>
		private List<DateTime> _listDatesAdded;
		///<summary>Keeps track of the dates that can be edited by the user currently logged in.  This is simply to save database calls.</summary>
		private Dictionary<DateTime,bool> _dictCanEditDay=new Dictionary<DateTime, bool>();

		public FormOrthoChart(Patient patCur) : this(patCur,0) {
		}

		///<summary>Opens this patient's ortho chart with a specific chart tab opened.  tabIndex correlates to the index of OrthoChartTabs.Listt.</summary>
		public FormOrthoChart(Patient patCur,int tabIndex) {
			_patCur=patCur;
			_patNoteCur=PatientNotes.Refresh(_patCur.PatNum,_patCur.Guarantor);
			_prevRow=-1;
			InitializeComponent();
			_indexInitialTab=tabIndex;
			Lan.F(this);
		}

		private void FormOrthoChart_Load(object sender,EventArgs e) {
			signatureBoxWrapper.SetAllowDigitalSig(true);
			_listOrthoChartsInitial=OrthoCharts.GetAllForPatient(_patCur.PatNum);
			_listDatesAdded=new List<DateTime>() {
				DateTime.Today
			};
			_listOrthoChartTabs=OrthoChartTabs.GetDeepCopy(true);
			FillTabs();
			_dictOrthoCharts=new SortedDictionary<DateTime, List<OrthoChart>>();
			FillDictionary();
			//A specific tab is desired to be pre-selected.  This has to happen after FillDataTable() because it causes FillGrid() to get called.
			if(_indexInitialTab!=tabControl.SelectedIndex) {
				tabControl.SelectedIndex=_indexInitialTab;
			}
			else {//Tab index hasn't changed, fill the grid.
				FillGrid();
			}
			FillGridPat();
			if(PrefC.GetBool(PrefName.OrthoCaseInfoInOrthoChart)) {
				gridOrtho.Visible=true;
				FillOrtho();
			}
		}

		///<summary>Sets the title of the form to the TabName of the first tab in the cache and then refreshes the current tabs.</summary>
		private void FillTabs() {
			//Set the title of this form to the first tab in the list.  The button to launch the Ortho Chart from the Chart module will be the same.
			Text=_listOrthoChartTabs[0].TabName;//It is considered database corruption if this fails.
			OrthoChartTab orthoChartTabSelected=null;
			if(tabControl.SelectedIndex >= 0) {
				orthoChartTabSelected=(OrthoChartTab)tabControl.TabPages[tabControl.SelectedIndex].Tag;
			}
			tabControl.TabPages.Clear();
			for(int i=0;i<_listOrthoChartTabs.Count;i++) {
				TabPage tabPage=new TabPage(_listOrthoChartTabs[i].TabName);
				tabPage.Tag=_listOrthoChartTabs[i];
				tabControl.TabPages.Add(tabPage);
				if(orthoChartTabSelected!=null && _listOrthoChartTabs[i].OrthoChartTabNum==orthoChartTabSelected.OrthoChartTabNum) {
					tabControl.SelectedIndex=i;
				}
			}
			if(tabControl.SelectedIndex==-1) {
				tabControl.SelectedIndex=0;
			}
		}

		///<summary>Refreshes _dictOrthoCharts with the ortho charts found in _listOrthoChartsInitial.</summary>
		private void FillDictionary() {
			//Fill the dictionary.  Key will be date, value will be list of ortho charts for that date.
			foreach(OrthoChart chart in _listOrthoChartsInitial) {
				if(!_dictOrthoCharts.ContainsKey(chart.DateService)) {
					_dictOrthoCharts.Add(chart.DateService,new List<OrthoChart>() {chart});
				}
				else {
					_dictOrthoCharts[chart.DateService].Add(chart);
				}
			}
			if(!_dictOrthoCharts.ContainsKey(DateTime.Today)) {
				_dictOrthoCharts.Add(DateTime.Today,new List<OrthoChart>());
			}
			_listOrthDisplayFields=DisplayFields.GetForCategory(DisplayFieldCategory.OrthoChart);//List of display fields and the order they should be displayed in.
			_listDisplayFieldNames=new List<string>();
			for(int i=0;i<_listOrthDisplayFields.Count;i++) {
				if(_listOrthDisplayFields[i].InternalName=="Signature") {
					_sigTableOrthoColIdx=i;
				}
				_listDisplayFieldNames.Add(_listOrthDisplayFields[i].Description);
			}
		}

		///<summary>Clears the current grid and fills from datatable.  Do not call unless you have saved changes to database first.</summary>
		private void FillGrid() {
			if(tabControl.SelectedIndex==-1) {
				return;//This happens when the tab pages are cleared (updating the selected index).
			}
			//else if(_indexInitialTab!=0) {
			//	tabControl.SelectedIndex=_indexInitialTab;
			//	_indexInitialTab=0;
			//}
			Cursor=Cursors.WaitCursor;
			//Get all the corresponding fields from the OrthoChartTabLink table that are associated to the currently selected ortho tab.
			OrthoChartTab orthoChartTab=_listOrthoChartTabs[tabControl.SelectedIndex];
			List<DisplayField> listSelectedTabDisplayFields=
				OrthoChartTabLinks.GetWhere(x => x.OrthoChartTabNum==orthoChartTab.OrthoChartTabNum)//Determines the number of items that will be returned
				.OrderBy(x => x.ItemOrder)//Each tab is ordered based on the ortho tab link entry
				.Select(x => _listOrthDisplayFields.FirstOrDefault(y => y.DisplayFieldNum==x.DisplayFieldNum))//Project all corresponding display fields in order
				.Where(x => x!=null)//Can happen when there is an OrthoChartTabLink in the database pointing to an invalid display field.
				.ToList();//Casts the projection to a list of display fields
			_sigColIdx=-1;//Clear out the signature column index cause it will most likely change or disappear (switching tabs)
			int gridMainScrollValue=gridMain.ScrollValue;
			gridMain.BeginUpdate();
			//Set the title of the grid to the title of the currently selected ortho chart tab.  This is so that medical users don't see Ortho Chart.
			gridMain.Title=orthoChartTab.TabName;
			gridMain.Columns.Clear();
			ODGridColumn col;
			//First column will always be the date.  gridMain_CellLeave() depends on this fact.
			col=new ODGridColumn(Lan.g(this,"Date"),70);
			gridMain.Columns.Add(col);
			foreach(DisplayField field in listSelectedTabDisplayFields) {
				string columnHeader=string.IsNullOrEmpty(field.DescriptionOverride) ? field.Description : field.DescriptionOverride;
				if(!string.IsNullOrEmpty(field.PickList)) {
					List<string> listComboOptions=field.PickList.Split(new string[] { "\r\n" },StringSplitOptions.None).ToList();
					col=new ODGridColumn(columnHeader,field.ColumnWidth,listComboOptions);
				}
				else {
					col=new ODGridColumn(columnHeader,field.ColumnWidth,true);
					if(field.InternalName=="Signature") {
						_sigColIdx=gridMain.Columns.Count;
						col.TextAlign=HorizontalAlignment.Center;
						col.IsEditable=false;
					}
				}
				col.Tag=field.Description;
				gridMain.Columns.Add(col);
			}
			gridMain.Rows.Clear();
			ODGridRow row;
			foreach(KeyValuePair<DateTime, List<OrthoChart>> kvPair in _dictOrthoCharts) {
				row=new ODGridRow();
				DateTime tempDate=kvPair.Key;
				row.Cells.Add(tempDate.ToShortDateString());
				row.Tag=tempDate;
				bool areAllColumnsBlank=true;
				foreach(DisplayField field in listSelectedTabDisplayFields) {
					string cellValue="";
					if(field.InternalName!="Signature") {
						//Find the index of the corresponding column in our table via the column title.
						OrthoChart chart=kvPair.Value.Find(x => x.FieldName==field.Description);
						if(chart!=null) {
							cellValue=chart.FieldValue;
						}
					}
					if(!string.IsNullOrEmpty(field.PickList)) {
						List<string> listComboOptions=field.PickList.Split(new string[] { "\r\n" },StringSplitOptions.None).ToList();
						int selectedIndex=listComboOptions.FindIndex(x => x==cellValue);
						row.Cells.Add(cellValue,selectedIndex);
					}
					else {
						row.Cells.Add(cellValue);
					}
					if(cellValue!="") {
						areAllColumnsBlank=false;
					}
				}
				if(!areAllColumnsBlank || _listDatesAdded.Contains(tempDate)) {
					gridMain.Rows.Add(row);
				}
				CanEditRow(tempDate);
			}
			gridMain.EndUpdate();
			if(gridMainScrollValue==0) {
				gridMain.ScrollToEnd();
			}
			else {
				gridMain.ScrollValue=gridMainScrollValue;
				gridMainScrollValue=0;
			}
			//Show the signature control if a signature display field is present on any tab.
			_showSigBox=_listOrthDisplayFields.Any(x => x.InternalName=="Signature");
			signatureBoxWrapper.Visible=_showSigBox;//Hide the signature box if this tab does not have the signature column present.
			if(_showSigBox) {
				for(int i=0;i<gridMain.Rows.Count;i++) {
					DisplaySignature(i,false);
				}
				signatureBoxWrapper.ClearSignature();
				signatureBoxWrapper.Enabled=false;//We don't want it to be enabled unless the user has clicked on a row.
				_prevRow=-1;
			}
			Cursor=Cursors.Default;
		}

		private void FillGridPat() {
			gridPat.BeginUpdate();
			gridPat.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn("Field",150);
			gridPat.Columns.Add(col);
			col=new ODGridColumn("Value",200);
			gridPat.Columns.Add(col);
			gridPat.Rows.Clear();
			_arrayPatientFields=PatFields.Refresh(_patCur.PatNum);
			PatFieldDefs.RefreshCache();
			PatFieldL.AddPatFieldsToGrid(gridPat,_arrayPatientFields.ToList(),FieldLocations.OrthoChart);
			gridPat.EndUpdate();
		}

		///<summary>Same as FillOrtho() in the ContrAccount.</summary>
		private void FillOrtho() {
			gridOrtho.BeginUpdate();
			gridOrtho.Columns.Clear();
			gridOrtho.Columns.Add(new ODGridColumn("",(gridOrtho.Width/2)-20,HorizontalAlignment.Right));
			gridOrtho.Columns.Add(new ODGridColumn("",(gridOrtho.Width/2)+20,HorizontalAlignment.Left));
			gridOrtho.Rows.Clear();
			ODGridRow row = new ODGridRow();
			DateTime firstOrthoProc = Procedures.GetFirstOrthoProcDate(_patNoteCur);
			if(firstOrthoProc!=DateTime.MinValue) {
				row.Cells.Add(Lan.g(this,"Total Tx Time")+": "); //Number of Years/Months/Days since the first ortho procedure on this account
				DateTimeOD dateTOD = new DateTimeOD(firstOrthoProc,DateTimeOD.Today);
				string strDateDiff="";
				if(dateTOD.YearsDiff!=0) {
					strDateDiff+=dateTOD.YearsDiff+" "+Lan.g(this,"year"+(dateTOD.YearsDiff==1 ? "" : "s"));
				}
				if(dateTOD.MonthsDiff!=0) {
					if(strDateDiff!="") {
						strDateDiff+=", ";
					}
					strDateDiff+=dateTOD.MonthsDiff+" "+Lan.g(this,"month"+(dateTOD.MonthsDiff==1 ? "" : "s"));
				}
				if(dateTOD.DaysDiff!=0 || strDateDiff=="") {
					if(strDateDiff!="") {
						strDateDiff+=", ";
					}
					strDateDiff+=dateTOD.DaysDiff+" "+Lan.g(this,"day"+(dateTOD.DaysDiff==1 ? "" : "s"));
				}
				row.Cells.Add(strDateDiff);
				gridOrtho.Rows.Add(row);
				row = new ODGridRow();
				row.Cells.Add(Lan.g(this,"Date Start")+": "); //Date of the first ortho procedure on this account
				row.Cells.Add(firstOrthoProc.ToShortDateString());
				gridOrtho.Rows.Add(row);

				row = new ODGridRow();
				row.Cells.Add(Lan.g(this,"Tx Months Total")+": "); //this patient's OrthoClaimMonthsTreatment, or the practice default if 0.
				int txMonthsTotal=(_patNoteCur.OrthoMonthsTreatOverride==-1?PrefC.GetByte(PrefName.OrthoDefaultMonthsTreat):_patNoteCur.OrthoMonthsTreatOverride);
				row.Cells.Add(txMonthsTotal.ToString());
				gridOrtho.Rows.Add(row);

				row = new ODGridRow();
				int txTimeInMonths=dateTOD.YearsDiff * 12 + dateTOD.MonthsDiff + (dateTOD.DaysDiff < 15? 0: 1);
				row.Cells.Add(Lan.g(this,"Months in Treatment")+": "); //idk what the difference between this and 'Total Tx Time' is.
				row.Cells.Add(txTimeInMonths.ToString());
				gridOrtho.Rows.Add(row);

				row = new ODGridRow();
				row.Cells.Add(Lan.g(this,"Months Rem")+": "); //Months Total - Total Tx Time
				row.Cells.Add(Math.Max(0,txMonthsTotal-txTimeInMonths).ToString());
				gridOrtho.Rows.Add(row);

			}
			else {
				row = new ODGridRow();
				row.Cells.Add(""); //idk what the difference between this and 'Total Tx Time' is.
				row.Cells.Add(Lan.g(this,"No ortho procedures charted")+".");
				gridOrtho.Rows.Add(row);
			}
			gridOrtho.EndUpdate();
		}

		private void menuItemSetup_Click(object sender,EventArgs e) {
			FormDisplayFieldsOrthoChart form=new FormDisplayFieldsOrthoChart();
			if(form.ShowDialog()==DialogResult.OK) {
				FillTabs();
				FillDictionary();
				FillGrid();
			}
		}

		private void tabControl_TabIndexChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void signatureBoxWrapper_ClearSignatureClicked(object sender,EventArgs e) {
			if(gridMain.SelectedCell.Y==-1) {
				return;
			}
			DateTime orthoDate=GetOrthoDate(gridMain.SelectedCell.Y);
			if(OrthoSignature.GetSigString(GetValueFromDict(orthoDate,_sigTableOrthoColIdx))!="") {
				_hasChanged=true;
			}
			SetValueInDict("",orthoDate,_sigTableOrthoColIdx);
			string sigColumnName=_listOrthDisplayFields.FirstOrDefault(x => x.InternalName=="Signature").Description;
			DisplaySignature(gridMain.SelectedCell.Y);
			_prevRow=gridMain.SelectedCell.Y;
		}

		private void signatureBoxWrapper_SignTopazClicked(object sender,EventArgs e) {
			if(gridMain.SelectedCell.Y==-1) {
				return;
			}
			DateTime orthoDate=GetOrthoDate(gridMain.SelectedCell.Y);
			if(OrthoSignature.GetSigString(GetValueFromDict(orthoDate,_sigTableOrthoColIdx))!="") {
				_hasChanged=true;
			}
			SetValueInDict("",orthoDate,_sigTableOrthoColIdx);
			string sigColumnName=_listOrthDisplayFields.FirstOrDefault(x => x.InternalName=="Signature").Description;
			_prevRow=gridMain.SelectedCell.Y;
			_topazNeedsSaved=true;
		}

		///<summary>Displays the signature for this row when clicking on the Date column or the Signature column. The gridMain_CellEnter event
		///does not fire when the column is not editable.</summary>
		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			if(e.Col!=0 && e.Col!=_sigColIdx) {//If not the date column or the signature column, return.
				return;
			}
			SaveAndSetSignatures(e.Row);
		}

		///<summary>This is necessary in addition to CellClick for when the user tabs or uses the arrow keys to enter a cell.</summary>
		private void gridMain_CellEnter(object sender,ODGridClickEventArgs e) {
			SaveAndSetSignatures(e.Row);
		}

		///<summary>Saves the signature to the data table if it hasn't been and displays the signature for this row.</summary>
		private void SaveAndSetSignatures(int currentRow) {
			if(!_showSigBox || currentRow<0) {
				return;
			}
			SetSignatureButtonVisibility(currentRow);
			//Try and save the previous row's signature if needed.
			SaveSignatureToDict(_prevRow);
			DisplaySignature(_prevRow);
			if(_topazNeedsSaved) {
				_topazNeedsSaved=false;
			}
			//Now that any previous sigs have been saved, display the current row's signature.
			DisplaySignature(currentRow);
			signatureBoxWrapper.Enabled=true;
			_prevRow=currentRow;
		}

		private void gridMain_CellLeave(object sender,ODGridClickEventArgs e) {
			//Get the date for the ortho chart that was just edited.
			DateTime orthoDate=GetOrthoDate(e.Row);
			string oldText=GetValueFromDict(orthoDate,(string)gridMain.Columns[e.Col].Tag);
			string newText=gridMain.Rows[e.Row].Cells[e.Col].Text;
			if(CanEditRow(orthoDate)) {
				if(newText != oldText) {
					SetValueInDict(newText,orthoDate,(string)gridMain.Columns[e.Col].Tag);
					//Cannot be placed in if statement below as we only want to clear the signature when the grid text has changed.
					//We cannot use a textchanged event to call the .dll as this causes massive slowness for certain customers.
					if(_showSigBox) {
						signatureBoxWrapper.ClearSignature();
					}
					_hasChanged=true;//They had permission and they made a change.
				}
				if(_showSigBox) {
					SaveSignatureToDict(e.Row);
					DisplaySignature(e.Row);
				}
			}
			else {
				//User is not authorized to edit this cell.  Check if they changed the old value and if they did, put it back to the way it was and warn them about security.
				if(newText!=oldText) {
					//The user actually changed the cell's value and we need to change it back and warn them that they don't have permission.
					gridMain.Rows[e.Row].Cells[e.Col].Text=oldText;
					gridMain.Invalidate();
					MsgBox.Show(this,"You need either Ortho Chart Edit (full) or Ortho Chart Edit (same user, signed) to edit this ortho chart.");
				}
			}
		}
		
		///<summary>Displays the signature that is saved in the dictionary in the signature box. Colors the grid row green if the signature is valid, 
		///red if invalid, or white if blank. Puts "Valid" or "Invalid" in the grid's signature column.</summary>
		private void DisplaySignature(int gridRow,bool hasRefresh=true) {
			if(!_showSigBox || gridRow<0) {
				return;
			}
			DateTime orthoDate=GetOrthoDate(gridRow);
			List<OrthoChart> listOrthoCharts=_dictOrthoCharts[orthoDate];
			//Get the "translated" name for the signature column.
			string sigColumnName=_listOrthDisplayFields.FirstOrDefault(x => x.InternalName=="Signature").Description;
			if(!listOrthoCharts.Exists(x => x.FieldName==sigColumnName)) {
				signatureBoxWrapper.ClearSignature();
				return;
			}
			OrthoSignature sig=new OrthoSignature(listOrthoCharts.Find(x => x.FieldName==sigColumnName).FieldValue);
			if(sig.SigString=="") {
				signatureBoxWrapper.ClearSignature();
				gridMain.Rows[gridRow].ColorBackG=SystemColors.Window;
				//Empty out the signature column displaying to the user.
				if(_sigColIdx > 0) {//User might be vieweing a tab that does not have the signature column.  Greater than 0 because index 0 is a Date column.
					gridMain.Rows[gridRow].Cells[_sigColIdx].Text="";
				}
				if(hasRefresh) {
					gridMain.Refresh();
				}
				return;
			}
			string keyData=OrthoCharts.GetKeyDataForSignatureHash(_patCur,listOrthoCharts
				.FindAll(x => x.DateService==orthoDate && x.FieldValue!="" && x.FieldName!=sigColumnName),orthoDate);
			signatureBoxWrapper.FillSignature(sig.IsTopaz,keyData,sig.SigString);
			if(!signatureBoxWrapper.IsValid) {
				//This ortho chart may have been signed when we were using the patient name in the hash. Try hashing the signature with the patient name.
				keyData=OrthoCharts.GetKeyDataForSignatureHash(_patCur,listOrthoCharts
					.FindAll(x => x.DateService==orthoDate && x.FieldValue!="" && x.FieldName!=sigColumnName),orthoDate,doUsePatName:true);
				signatureBoxWrapper.FillSignature(sig.IsTopaz,keyData,sig.SigString);
			}
			if(signatureBoxWrapper.IsValid) {
				gridMain.Rows[gridRow].ColorBackG=Color.FromArgb(0,245,165);//A lighter version of Color.MediumSpringGreen
				if(_sigColIdx > 0) {//User might be vieweing a tab that does not have the signature column.  Greater than 0 because index 0 is a Date column.
					gridMain.Rows[gridRow].Cells[_sigColIdx].Text=Lan.g(this,"Valid");
				}
			}
			else {
				gridMain.Rows[gridRow].ColorBackG=Color.FromArgb(255,140,143);//A darker version of Color.LightPink
				if(_sigColIdx > 0) {//User might be vieweing a tab that does not have the signature column.  Greater than 0 because index 0 is a Date column.
					gridMain.Rows[gridRow].Cells[_sigColIdx].Text=Lan.g(this,"Invalid");
				}
			}
			if(hasRefresh) {
				gridMain.Refresh();
			}
		}

		///<summary>Removes the Sign Topaz button and the Clear Signature button from the signature box if the user does not have OrthoChartEdit
		///permissions for that date.</summary>
		private void SetSignatureButtonVisibility(int gridRow) {
			DateTime orthoDate=GetOrthoDate(gridRow);
			signatureBoxWrapper.SetButtonVisibility(CanEditRow(orthoDate));
		}

		///<summary>Saves the signature to the dictionary. The signature is hashed using the patient name, the date of service, and all ortho chart fields
		///(even the ones not showing).</summary>
		private void SaveSignatureToDict(int gridRow) {
			if(!_showSigBox || gridRow<0) {
				return;
			}
			DateTime orthoDate=GetOrthoDate(gridRow);
			if(!CanEditRow(orthoDate)) {
				return;
			}			
			if(!signatureBoxWrapper.GetSigChanged() || !signatureBoxWrapper.IsValid) {
				return;
			}
			string keyData;
			//Get the "translated" name for the signature column.
			string sigColumnName=_listOrthDisplayFields.FirstOrDefault(x => x.InternalName=="Signature").Description;
			List<OrthoChart> listCharts=_dictOrthoCharts[orthoDate].FindAll(x => x.DateService==orthoDate && x.FieldValue!="" && x.FieldName!=sigColumnName);
			keyData=OrthoCharts.GetKeyDataForSignatureSaving(listCharts,orthoDate);
			OrthoSignature sig=new OrthoSignature();
			sig.IsTopaz=signatureBoxWrapper.GetSigIsTopaz();
			sig.SigString=signatureBoxWrapper.GetSignature(keyData);
			if(sig.IsTopaz && !_topazNeedsSaved) {
				return;
			}
			if(OrthoSignature.GetSigString(GetValueFromDict(orthoDate,sigColumnName))!=sig.SigString) {
				_hasChanged=true;
				SetValueInDict(sig.ToString(),orthoDate,sigColumnName);
			}
		}

		private bool CanEditRow(DateTime orthoDate) {
			if(_dictCanEditDay.ContainsKey(orthoDate)) {
				return _dictCanEditDay[orthoDate];
			}
			if(Security.IsAuthorized(Permissions.OrthoChartEditFull,orthoDate,true)) {
				_dictCanEditDay[orthoDate]=true;
				return true;
			}
			if(!Security.IsAuthorized(Permissions.OrthoChartEditUser,orthoDate,true)) {
				_dictCanEditDay[orthoDate]=false;
				return false;//User doesn't have any permission.
			}
			//User has permission to edit the ones that they have signed or ones that no one has signed.
			if(!_showSigBox) {
				_dictCanEditDay[orthoDate]=true;
				return true;
			}
			OrthoChart sigChart=_dictOrthoCharts[orthoDate].Find(x => x.FieldName==_listDisplayFieldNames[_sigTableOrthoColIdx]);
			if(sigChart==null || sigChart.UserNum==Security.CurUser.UserNum) {
				_dictCanEditDay[orthoDate]=true;
				return true;
			}
			bool canEditRow;
			OrthoSignature sig=new OrthoSignature(sigChart.FieldValue);
			canEditRow=string.IsNullOrEmpty(sig.SigString);//User has partial permission and somebody else signed it.
			_dictCanEditDay[orthoDate]=canEditRow;
			return canEditRow;
		}

		///<summary>Gets the date of the ortho chart for the passed in grid row.</summary>
		private DateTime GetOrthoDate(int gridRow) {
			return PIn.Date(gridMain.Rows[gridRow].Cells[0].Text);//First column will always be the date.
		}

		///<summary>Gets the value from _dictOrthoCharts for the specified date and column heading.  Returns empty string if not found.
		///Note that the column name showing within the ODGrid could be different than the column heading within _dictOrthoCharts.
		///Use gridMain.Columns[x].Tag to get the corresponding column header that _dictOrthoCharts uses (displayfield.Description).</summary>
		private string GetValueFromDict(DateTime orthoDate,string columnHeading) {			
			if(!_dictOrthoCharts.ContainsKey(orthoDate) || !_dictOrthoCharts[orthoDate].Exists(x => x.FieldName==columnHeading)) {
				return "";
			}
			return _dictOrthoCharts[orthoDate].Find(x => x.FieldName==columnHeading).FieldValue;
		}

		///<summary>Gets the value from _dictOrthoCharts for the specified date and index.  Returns empty string if not found.</summary>
		private string GetValueFromDict(DateTime orthoDate,int columnIdx) {
			return GetValueFromDict(orthoDate,_listDisplayFieldNames[columnIdx]);
		}

		///<summary>Returns true if the display field column has a pick list</summary>
		private bool HasPickList(string colName) {
			foreach (DisplayField field in _listOrthDisplayFields) {
				if(field.Description==colName && !string.IsNullOrEmpty(field.PickList)) {
					return true;
				}
			}
			return false;
		}

		///<summary>Sets the value in _dictOrthoCharts for the specified date and column heading.</summary>
		private void SetValueInDict(string newValue,DateTime orthoDate,string columnHeading) {
			if(!_dictOrthoCharts.ContainsKey(orthoDate)) {
				_dictOrthoCharts.Add(orthoDate,new List<OrthoChart>());
			}
			if(!_dictOrthoCharts[orthoDate].Exists(x => x.FieldName==columnHeading)) {
				OrthoChart chart=new OrthoChart();
				chart.DateService=orthoDate;
				chart.FieldName=columnHeading;
				chart.FieldValue=newValue;
				chart.PatNum=_patCur.PatNum;
				chart.UserNum=Security.CurUser.UserNum;
				_dictOrthoCharts[orthoDate].Add(chart);
				return;
			}
			_dictOrthoCharts[orthoDate].Find(x => x.FieldName==columnHeading).FieldValue=newValue;
			_dictOrthoCharts[orthoDate].Find(x => x.FieldName==columnHeading).UserNum=Security.CurUser.UserNum;
		}

		///<summary>Sets the value in _dictOrthoCharts for the specified date and index.</summary>
		private void SetValueInDict(string newValue,DateTime orthoDate,int columnIdx) {
			SetValueInDict(newValue,orthoDate,_listDisplayFieldNames[columnIdx]);
		}

		private void gridMain_CellSelectionCommitted(object sender,ODGridClickEventArgs e) {
			DateTime orthoDate=GetOrthoDate(gridMain.SelectedCell.Y);
			if(!CanEditRow(orthoDate)) {
				return;
			}
			_hasChanged=true;
			if(_showSigBox) {
				signatureBoxWrapper.ClearSignature();
			}
		}

		private void gridPat_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(gridPat.Rows[e.Row].Tag is PatFieldDef) {//patfield for an existing PatFieldDef
				PatFieldDef patFieldDef=(PatFieldDef)gridPat.Rows[e.Row].Tag;
				PatField field=PatFields.GetByName(patFieldDef.FieldName,_arrayPatientFields);
				PatFieldL.OpenPatField(field,patFieldDef,_patCur.PatNum,true);
			}
			else if(gridPat.Rows[e.Row].Tag is PatField) {//PatField for a PatFieldDef that no longer exists
				PatField field=(PatField)gridPat.Rows[e.Row].Tag;
				FormPatFieldEdit FormPF=new FormPatFieldEdit(field);
				FormPF.IsLaunchedFromOrtho=true;
				FormPF.ShowDialog();
			}
			FillGridPat();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormOrthoChartAddDate FormOCAD=new FormOrthoChartAddDate();
			FormOCAD.ShowDialog();
			if(FormOCAD.DialogResult!=DialogResult.OK) {
				return;
			}
			if(_listDatesAdded.Contains(FormOCAD.SelectedDate)) {
				return;//No need to refill the grid
			}
			_listDatesAdded.Add(FormOCAD.SelectedDate);
			if(_dictOrthoCharts.ContainsKey(FormOCAD.SelectedDate)) {
				FillGrid();
				return;
			}
			SaveAndSetSignatures(gridMain.SelectedCell.Y);
			_dictOrthoCharts.Add(FormOCAD.SelectedDate,new List<OrthoChart>());//COULD HAVE TO DO WITH EMPTY SIG ITEM HERE??
			_hasChanged=true;
			FillGrid();
		}

		private void butUseAutoNote_Click(object sender,EventArgs e) {
			if(gridMain.SelectedCell.X==-1 || gridMain.SelectedCell.X==0 || gridMain.SelectedCell.X==_sigColIdx) {
				MsgBox.Show(this,"Please select an editable Ortho Chart cell first.");
				return;
			}
			if(HasPickList(gridMain.Columns[gridMain.SelectedCell.X].Heading)) {
				MsgBox.Show(this,"Cannot add auto notes to a field with a pick list.");
				return;
			}
			FormAutoNoteCompose FormA=new FormAutoNoteCompose();
			FormA.ShowDialog();
			if(FormA.DialogResult==DialogResult.OK) {
				Point selectedCell=new Point(gridMain.SelectedCell.X,gridMain.SelectedCell.Y);
				//Add text to current focused cell				
				gridMain.Rows[gridMain.SelectedCell.Y].Cells[gridMain.SelectedCell.X].Text+=FormA.CompletedNote;
				//Since the redrawing of the row height is dependent on the edit text box built into the ODGrid, we have to manually tell the grid to redraw.
				//This will essentially "refresh" the grid.  We do not want to call FillGrid() because that will lose data in other cells that have not been saved to datatable.
				if(_showSigBox && FormA.CompletedNote != "") {
					signatureBoxWrapper.ClearSignature();
					SaveSignatureToDict(gridMain.SelectedCell.Y);
					DisplaySignature(gridMain.SelectedCell.Y);
				}
				gridMain.BeginUpdate();
				gridMain.EndUpdate();
				gridMain.SetSelected(selectedCell);
				_hasChanged=true;
			}
		}

		private void butAudit_Click(object sender,EventArgs e) {
			SecurityLog[] orthoChartLogs;
			SecurityLog[] patientFieldLogs;
			try {
				orthoChartLogs=SecurityLogs.Refresh(_patCur.PatNum,new List<Permissions> { Permissions.OrthoChartEditFull },null,checkIncludeArchived.Checked);
				patientFieldLogs=SecurityLogs.Refresh(new DateTime(1,1,1),DateTime.Today,Permissions.PatientFieldEdit,_patCur.PatNum,0,
					DateTime.MinValue,DateTime.Today,checkIncludeArchived.Checked);
			}
			catch(Exception ex) {
				FriendlyException.Show(Lan.g(this,"There was a problem loading the Audit Trail."),ex);
				return;
			}
			SortedDictionary<DateTime,List<SecurityLog>> dictDatesOfServiceLogEntries=new SortedDictionary<DateTime,List<SecurityLog>>();
			//Add all dates from grid first, some may not have audit trail entries, but should be selectable from FormAO
			for(int i=0;i<gridMain.Rows.Count;i++) {
				DateTime dtCur=GetOrthoDate(i);
				if(dictDatesOfServiceLogEntries.ContainsKey(dtCur)) {
					continue;
				}
				dictDatesOfServiceLogEntries.Add(dtCur,new List<SecurityLog>());
			}
			//Add Ortho Audit Trail Entries
			for(int i=0;i<orthoChartLogs.Length;i++) {
				DateTime dtCur=OrthoCharts.GetOrthoDateFromLog(orthoChartLogs[i]);
				if(!dictDatesOfServiceLogEntries.ContainsKey(dtCur)) {
					dictDatesOfServiceLogEntries.Add(dtCur,new List<SecurityLog>());
				}
				dictDatesOfServiceLogEntries[dtCur].Add(orthoChartLogs[i]);//add entry to existing list.
			}
			FormAuditOrtho FormAO=new FormAuditOrtho();
			FormAO.DictDateOrthoLogs=dictDatesOfServiceLogEntries;
			FormAO.PatientFieldLogs.AddRange(patientFieldLogs);
			FormAO.ShowDialog();
		}

		private int _pagesPrinted;
		private int _headingPrintH;
		private bool _headingPrinted;
		private List<OrthoChartTab> _listOrthoChartTabs;

		private void butPrint_Click(object sender,EventArgs e) {
			_pagesPrinted=0;
			PrintDocument pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			if(pd.DefaultPageSettings.PrintableArea.Height==0) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			if(gridMain.WidthAllColumns>800) {
				//a new feature will need to be implemented to handle when columns widths are greater than 1050
				pd.DefaultPageSettings.Landscape=true;
			}
			_headingPrinted=false;
			try {
#if DEBUG
				FormRpPrintPreview pView = new FormRpPrintPreview();
				pView.printPreviewControl2.Document=pd;
				pView.ShowDialog();
#else
				if(PrinterL.SetPrinter(pd,PrintSituation.Default,0,"Ortho chart printed")) {
					pd.Print();
				}
#endif
			}
			catch {
				MessageBox.Show(Lan.g(this,"Printer not available"));
			}
		}

		private void pd_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
			Graphics g=e.Graphics;
			string text;
			using(Font headingFont = new Font("Arial",13,FontStyle.Bold))
			using(Font subHeadingFont = new Font("Arial",10,FontStyle.Bold)) {
				int yPos=bounds.Top;
				#region printHeading
				//========== TITLE ==========
				string title=_listOrthoChartTabs[0].TabName;
				g.DrawString(title,headingFont,Brushes.Black,400-g.MeasureString(title,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(title,headingFont).Height;
				if(_listOrthoChartTabs.Count>1) {
					OrthoChartTab orthoChartTab=_listOrthoChartTabs[tabControl.SelectedIndex];
					//========== SUBHEADING ==========
					text=orthoChartTab.TabName;//The tab selected will be the subheading. 
					g.DrawString(text,subHeadingFont,Brushes.Black,400-g.MeasureString(text,subHeadingFont).Width/2,yPos);
					//========== DATE ==========
					text=DateTime.Today.ToShortDateString();
					g.DrawString(text,subHeadingFont,Brushes.Black,800-g.MeasureString(text,subHeadingFont).Width,yPos);
					//========== PATIENT NAME ==========
					yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
					text=_patCur.GetNameFL();//name
					if(g.MeasureString(text,subHeadingFont).Width>700) {
						//extremely long name
						text=_patCur.GetNameFirst()[0]+". "+_patCur.LName;//example: J. Sparks
					}
					string[] headerText={ text };
					text=headerText[0];
					g.DrawString(text,subHeadingFont,Brushes.Black,400-g.MeasureString(text,subHeadingFont).Width/2,yPos);
					//========== PAGE NUMBER ==========
					text="Page "+(_pagesPrinted+1);
					g.DrawString(text,subHeadingFont,Brushes.Black,800-g.MeasureString(text,subHeadingFont).Width,yPos);
				}
				else {
					//========== PATIENT NAME ==========
					text=_patCur.GetNameFL();//name
					if(g.MeasureString(text,subHeadingFont).Width>700) {
						//extremely long name
						text=_patCur.GetNameFirst()[0]+". "+_patCur.LName;//example: J. Sparks
					}
					string[] headerText={ text };
					text=headerText[0];
					g.DrawString(text,subHeadingFont,Brushes.Black,400-g.MeasureString(text,subHeadingFont).Width/2,yPos);
					//========== DATE ==========
					text=DateTime.Today.ToShortDateString();
					g.DrawString(text,subHeadingFont,Brushes.Black,800-g.MeasureString(text,subHeadingFont).Width,yPos);
					yPos+=(int)g.MeasureString(text,subHeadingFont).Height;
					//========== PAGE NUMBER ==========
					text="Page "+(_pagesPrinted+1);
					g.DrawString(text,subHeadingFont,Brushes.Black,800-g.MeasureString(text,subHeadingFont).Width,yPos);
				}
				yPos+=20;
				_headingPrinted=true;
				_headingPrintH=yPos;
				#endregion
				//========== MAIN GRID ==========
				yPos=gridMain.PrintPage(g,_pagesPrinted,bounds,_headingPrintH,true);
				_pagesPrinted++;
				if(yPos==-1) {
					e.HasMorePages=true;
				}
				else {
					e.HasMorePages=false;
				}
			}//end using
			g.Dispose();
		}

		private void butOK_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormOrthoChart_FormClosing(object sender,FormClosingEventArgs e) {
			if(!_hasChanged) {
				return;
			}
			else if(DialogResult!=DialogResult.OK 
				&& _hasChanged && !MsgBox.Show(this,MsgBoxButtons.YesNo,"Unsaved changes will be lost. Would you like to save changes instead?")) 
			{
				return;
			}
			if(_showSigBox && gridMain.SelectedCell.Y != -1) {
				SaveSignatureToDict(gridMain.SelectedCell.Y);
			}
			if(_showSigBox) {
				//Don't save the signature if the user tried to sign an empty ortho chart.
				for(int i = _dictOrthoCharts.Keys.Count-1;i>=0;i--) {
					if(_dictOrthoCharts[_dictOrthoCharts.Keys.ElementAt(i)].All(x => (_sigTableOrthoColIdx>-1 && x.FieldName==_listDisplayFieldNames[_sigTableOrthoColIdx]) || x.FieldValue=="")) {
						_dictOrthoCharts.Remove(_dictOrthoCharts.Keys.ElementAt(i));
					}
				}
			}
			List<OrthoChart> listNewOrthoCharts=new List<OrthoChart>();
			foreach(KeyValuePair<DateTime,List<OrthoChart>> kvPair in _dictOrthoCharts) {
				listNewOrthoCharts.AddRange(kvPair.Value);
			}
			OrthoCharts.Sync(_patCur,listNewOrthoCharts,_listOrthDisplayFields,_listOrthDisplayFields.Find(x => x.InternalName=="Signature"));
		}

		///<summary>Stores the signature string and whether the signature is Topaz. Use ToString() to store it in the database.
		///This class saves us from adding a IsSigTopaz column to the orthochart table (somehow).</summary>
		private class OrthoSignature {
			public bool IsTopaz;
			public string SigString;

			///<summary>Parses a string like "0:ritwq/wV8vlrgUYahhK+RH5UeBFA6W4jCkZdo0cDWd63aZb1S/W3Z4eW5LmchqfgniG23" into a Signature object. 
			///The 1st character is whether or not the signature is Topaz; the 2nd character is a separator; the rest of the string is the signature data.
			///</summary>
			public OrthoSignature(string dbString) {
				if(dbString.Length < 3) {
					IsTopaz=false;
					SigString="";
					return;
				}
				IsTopaz=dbString[0]=='1' ? true : false;
				SigString=dbString.Substring(2);
			}

			public OrthoSignature() {
				IsTopaz=false;
				SigString="";
			}

			///<summary>Gets the signature string from a string like "1:52222559445999975122111500485555". Passing in an empty string will return an empty
			///string.</summary>
			public static string GetSigString(string dbString) {
				if(dbString.Length < 3) {
					return "";
				}
				return dbString.Substring(2);
			}

			///<summary>Converts the object to a string like "0:ritwq/wV8vlrgUYahhK+RH5UeBFA6W4jCkZdo0cDWd63aZb1S/W3Z4eW5LmchqfgniG23". The 1st character
			///is whether or not the signature is Topaz; the 2nd character is a separator; the rest of the string is the signature data.</summary>
			public override string ToString() {
				return (IsTopaz ? "1" : "0")+":"+SigString;
			}
		}
	}
}