using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Data;
using OpenDental.ReportingComplex;
using OpenDental.UI;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using CodeBase;

namespace OpenDental{
	///<summary></summary>
	public class FormRpProcNotBilledIns : ODForm {
		private OpenDental.UI.Button butClose;
		private OpenDental.UI.Button butPrint;
		private IContainer components;
		private CheckBox checkMedical;
		private Label labelClinic;
		private UI.ComboBoxMulti comboBoxMultiClinics;
		private UI.ODGrid gridMain;
		private List<Clinic> _listClinics;
		private ReportComplex _myReport;
		private decimal _procTotalAmt;
		private DateTime _myReportDateFrom;
		private UI.Button butNewClaims;
		private DateTime _myReportDateTo;
		private const int _colWidthPatName=200;
		private const int _colWidthProcDate=110;
		private const int _colWidthAmount=90;
		private const int _colWidthClinic=75;
		private CheckBox checkAutoGroupProcs;
		private DateTime _dateFromPrev=DateTime.MaxValue;
		private UI.Button butRefresh;
		private ImageList imageListCalendar;
		private UI.Button butSelectAll;
		private CheckBox checkShowProcsNoIns;
		private GroupBox groupBox2;
		private ODDateRangePicker dateRangePicker;
		private DateTime _dateToPrev=DateTime.MaxValue;

		///<summary></summary>
		public FormRpProcNotBilledIns(){
			InitializeComponent();
 			Lan.F(this);
		}

		///<summary></summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpProcNotBilledIns));
			this.butClose = new OpenDental.UI.Button();
			this.butPrint = new OpenDental.UI.Button();
			this.checkMedical = new System.Windows.Forms.CheckBox();
			this.imageListCalendar = new System.Windows.Forms.ImageList(this.components);
			this.labelClinic = new System.Windows.Forms.Label();
			this.comboBoxMultiClinics = new OpenDental.UI.ComboBoxMulti();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butNewClaims = new OpenDental.UI.Button();
			this.checkAutoGroupProcs = new System.Windows.Forms.CheckBox();
			this.butRefresh = new OpenDental.UI.Button();
			this.butSelectAll = new OpenDental.UI.Button();
			this.checkShowProcsNoIns = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.dateRangePicker = new OpenDental.UI.ODDateRangePicker();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(917, 662);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 4;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(25, 662);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(75, 26);
			this.butPrint.TabIndex = 3;
			this.butPrint.Text = "Print";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// checkMedical
			// 
			this.checkMedical.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkMedical.Location = new System.Drawing.Point(678, 12);
			this.checkMedical.Name = "checkMedical";
			this.checkMedical.Size = new System.Drawing.Size(227, 17);
			this.checkMedical.TabIndex = 11;
			this.checkMedical.Text = "Include Medical Procedures";
			this.checkMedical.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkMedical.UseVisualStyleBackColor = true;
			this.checkMedical.Visible = false;
			// 
			// imageListCalendar
			// 
			this.imageListCalendar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListCalendar.ImageStream")));
			this.imageListCalendar.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListCalendar.Images.SetKeyName(0, "arrowDownTriangle.gif");
			this.imageListCalendar.Images.SetKeyName(1, "arrowUpTriangle.gif");
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(461, 36);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(55, 16);
			this.labelClinic.TabIndex = 68;
			this.labelClinic.Text = "Clinics";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelClinic.Visible = false;
			// 
			// comboBoxMultiClinics
			// 
			this.comboBoxMultiClinics.ArraySelectedIndices = new int[0];
			this.comboBoxMultiClinics.BackColor = System.Drawing.SystemColors.Window;
			this.comboBoxMultiClinics.Items = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiClinics.Items")));
			this.comboBoxMultiClinics.Location = new System.Drawing.Point(517, 35);
			this.comboBoxMultiClinics.Name = "comboBoxMultiClinics";
			this.comboBoxMultiClinics.SelectedIndices = ((System.Collections.ArrayList)(resources.GetObject("comboBoxMultiClinics.SelectedIndices")));
			this.comboBoxMultiClinics.Size = new System.Drawing.Size(160, 21);
			this.comboBoxMultiClinics.TabIndex = 67;
			this.comboBoxMultiClinics.Visible = false;
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(25, 71);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(967, 588);
			this.gridMain.TabIndex = 69;
			this.gridMain.Title = "Procedures Not Billed";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableProcedures";
			// 
			// butNewClaims
			// 
			this.butNewClaims.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNewClaims.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.butNewClaims.Autosize = true;
			this.butNewClaims.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNewClaims.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNewClaims.CornerRadius = 4F;
			this.butNewClaims.Location = new System.Drawing.Point(513, 662);
			this.butNewClaims.Name = "butNewClaims";
			this.butNewClaims.Size = new System.Drawing.Size(100, 26);
			this.butNewClaims.TabIndex = 71;
			this.butNewClaims.Text = "New Claims";
			this.butNewClaims.UseVisualStyleBackColor = true;
			this.butNewClaims.Click += new System.EventHandler(this.butNewClaims_Click);
			// 
			// checkAutoGroupProcs
			// 
			this.checkAutoGroupProcs.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAutoGroupProcs.Location = new System.Drawing.Point(678, 33);
			this.checkAutoGroupProcs.Name = "checkAutoGroupProcs";
			this.checkAutoGroupProcs.Size = new System.Drawing.Size(227, 17);
			this.checkAutoGroupProcs.TabIndex = 72;
			this.checkAutoGroupProcs.Text = "Automatically Group Procedures";
			this.checkAutoGroupProcs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkAutoGroupProcs.UseVisualStyleBackColor = true;
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(942, 38);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(50, 26);
			this.butRefresh.TabIndex = 73;
			this.butRefresh.Text = "Refresh";
			this.butRefresh.UseVisualStyleBackColor = true;
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// butSelectAll
			// 
			this.butSelectAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSelectAll.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.butSelectAll.Autosize = true;
			this.butSelectAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSelectAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSelectAll.CornerRadius = 4F;
			this.butSelectAll.Location = new System.Drawing.Point(407, 662);
			this.butSelectAll.Name = "butSelectAll";
			this.butSelectAll.Size = new System.Drawing.Size(100, 26);
			this.butSelectAll.TabIndex = 74;
			this.butSelectAll.Text = "Select All";
			this.butSelectAll.UseVisualStyleBackColor = true;
			this.butSelectAll.Click += new System.EventHandler(this.butSelectAll_Click);
			// 
			// checkShowProcsNoIns
			// 
			this.checkShowProcsNoIns.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowProcsNoIns.Location = new System.Drawing.Point(306, 12);
			this.checkShowProcsNoIns.Name = "checkShowProcsNoIns";
			this.checkShowProcsNoIns.Size = new System.Drawing.Size(371, 17);
			this.checkShowProcsNoIns.TabIndex = 75;
			this.checkShowProcsNoIns.Text = "Show Procedures Completed Before Insurance Added";
			this.checkShowProcsNoIns.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowProcsNoIns.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkShowProcsNoIns);
			this.groupBox2.Controls.Add(this.dateRangePicker);
			this.groupBox2.Controls.Add(this.checkMedical);
			this.groupBox2.Controls.Add(this.comboBoxMultiClinics);
			this.groupBox2.Controls.Add(this.checkAutoGroupProcs);
			this.groupBox2.Controls.Add(this.labelClinic);
			this.groupBox2.Location = new System.Drawing.Point(25, 4);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(911, 61);
			this.groupBox2.TabIndex = 77;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Filters";
			// 
			// dateRangePicker
			// 
			this.dateRangePicker.BackColor = System.Drawing.SystemColors.Control;
			this.dateRangePicker.DefaultDateTimeFrom = new System.DateTime(2018, 1, 1, 0, 0, 0, 0);
			this.dateRangePicker.DefaultDateTimeTo = new System.DateTime(2018, 1, 9, 0, 0, 0, 0);
			this.dateRangePicker.EnableWeekButtons = false;
			this.dateRangePicker.Location = new System.Drawing.Point(1, 34);
			this.dateRangePicker.MaximumSize = new System.Drawing.Size(0, 185);
			this.dateRangePicker.MinimumSize = new System.Drawing.Size(453, 22);
			this.dateRangePicker.Name = "dateRangePicker";
			this.dateRangePicker.Size = new System.Drawing.Size(453, 22);
			this.dateRangePicker.TabIndex = 0;
			// 
			// FormRpProcNotBilledIns
			// 
			this.AcceptButton = this.butPrint;
			this.ClientSize = new System.Drawing.Size(1019, 696);
			this.Controls.Add(this.butSelectAll);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.butNewClaims);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.groupBox2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(1035, 734);
			this.Name = "FormRpProcNotBilledIns";
			this.ShowInTaskbar = false;
			this.Text = "Procedures Not Billed to Insurance";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRpProcNotBilledIns_FormClosing);
			this.Load += new System.EventHandler(this.FormProcNotAttach_Load);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormProcNotAttach_Load(object sender, System.EventArgs e) {
			dateRangePicker.SetDateTimeTo(DateTime.Today);
			dateRangePicker.SetDateTimeFrom(DateTime.Today);
			if(PrefC.GetBool(PrefName.ShowFeatureMedicalInsurance)) {
				checkMedical.Visible=true;
			}
			if(PrefC.HasClinicsEnabled) {
				comboBoxMultiClinics.Visible=true;
				labelClinic.Visible=true;
				FillClinics();
			}
			if(PrefC.GetBool(PrefName.ClaimProcsNotBilledToInsAutoGroup)) {
				checkAutoGroupProcs.Checked=true;
			}
			FillGrid();
		}
		
		private void FillGrid() {
			RefreshReport();
			gridMain.BeginUpdate();
			ODGridColumn col=null;
			if(gridMain.Columns.Count==0) {
				col=new ODGridColumn(Lan.g(this,"Patient Name"),_colWidthPatName);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"Procedure Date"),_colWidthProcDate);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g(this,"Procedure Descipion"),0);//Dynaimc width
				gridMain.Columns.Add(col);
				if(PrefC.HasClinicsEnabled) {
					col=new ODGridColumn(Lan.g(this,"Clinic"),_colWidthClinic);
					gridMain.Columns.Add(col);
				}
				col=new ODGridColumn(Lan.g(this,"Amount"),_colWidthAmount,HorizontalAlignment.Right);
				gridMain.Columns.Add(col);
			}
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_myReport.ReportObjects.Count;i++) {
				if(_myReport.ReportObjects[i].ObjectType!=ReportObjectType.QueryObject) {
					continue;
				}
				QueryObject queryObj=(QueryObject)_myReport.ReportObjects[i];
				for(int j=0;j<queryObj.ReportTable.Rows.Count;j++) {
					row=new ODGridRow();
					row.Cells.Add(queryObj.ReportTable.Rows[j][0].ToString());//Procedure Name
					row.Cells.Add(PIn.Date(queryObj.ReportTable.Rows[j][1].ToString()).ToShortDateString());//Procedure Date
					row.Cells.Add(queryObj.ReportTable.Rows[j][2].ToString());//Procedure Description
					if(PrefC.HasClinicsEnabled) {
						long clinicNum=PIn.Long(queryObj.ReportTable.Rows[j][5].ToString());
						if(clinicNum==0) {
							row.Cells.Add("Unassigned");
						}
						else {
							row.Cells.Add(Clinics.GetAbbr(clinicNum));
						}
					}
					row.Cells.Add(PIn.Double(queryObj.ReportTable.Rows[j][3].ToString()).ToString("c"));//Amount
					_procTotalAmt+=PIn.Decimal(queryObj.ReportTable.Rows[j][3].ToString());
					row.Tag=PIn.Long(queryObj.ReportTable.Rows[j][4].ToString());//Tag set to ProcNum.  Used in butNewClaims_Click().
					gridMain.Rows.Add(row);
				}
			}
			gridMain.EndUpdate();
		}

		private void FillClinics() {
			List <int> listSelectedItems=new List<int>();
			_listClinics=Clinics.GetForUserod(Security.CurUser);
			comboBoxMultiClinics.Items.Add(Lan.g(this,"All"));
			if(!Security.CurUser.ClinicIsRestricted) {
				comboBoxMultiClinics.Items.Add(Lan.g(this,"Unassigned"));
				listSelectedItems.Add(1);
			}
			for(int i=0;i<_listClinics.Count;i++) {
				int curIndex=comboBoxMultiClinics.Items.Add(_listClinics[i].Abbr);
				if(Clinics.ClinicNum==0) {
					listSelectedItems.Add(curIndex);
				}
				if(_listClinics[i].ClinicNum==Clinics.ClinicNum) {
					listSelectedItems.Clear();
					listSelectedItems.Add(curIndex);
				}
			}
			foreach(int index in listSelectedItems) {
				comboBoxMultiClinics.SetSelected(index,true);
			}
		}

		//Only called in FillGrid().
		private void RefreshReport() {
			bool hasValidationPassed=ValidateFields();
			List<long> listClinicNums=new List<long>();
			if(PrefC.HasClinicsEnabled) {
				if(comboBoxMultiClinics.ListSelectedIndices.Contains(0)) {//All option selected
					for(int j=0;j<_listClinics.Count;j++) {
						listClinicNums.Add(_listClinics[j].ClinicNum);//Add all clinics this person has access to.
					}
					if(!Security.CurUser.ClinicIsRestricted) {
						listClinicNums.Add(0);//Unassigned
					}
				}
				else {//All option not selected
					for(int i=0;i<comboBoxMultiClinics.ListSelectedIndices.Count;i++) {
						if(Security.CurUser.ClinicIsRestricted) {
							listClinicNums.Add(_listClinics[comboBoxMultiClinics.ListSelectedIndices[i]-1].ClinicNum);//Minus 1 to skip over the All.
						}
						else if(comboBoxMultiClinics.ListSelectedIndices[i]==1) {//Not restricted and user selected Unassigned.
							listClinicNums.Add(0);//Unassigned
						}
						else {//Not restricted and Unassigned option is not selected.
							listClinicNums.Add(_listClinics[comboBoxMultiClinics.ListSelectedIndices[i]-2].ClinicNum);//Minus 2 to skip over All and Unassigned.
						}
					}
				}
			}
			DataTable tableNotBilled=new DataTable();
			if(hasValidationPassed) {
				tableNotBilled=RpProcNotBilledIns.GetProcsNotBilled(listClinicNums,checkMedical.Checked,_myReportDateFrom,_myReportDateTo, checkShowProcsNoIns.Checked);
			}
			string subtitleClinics="";
			if(PrefC.HasClinicsEnabled) {
				if(comboBoxMultiClinics.ListSelectedIndices.Contains(0)) {//All option selected
					subtitleClinics=Lan.g(this,"All Clinics");
				}
				else {
					for(int i=0;i<comboBoxMultiClinics.ListSelectedIndices.Count;i++) {
						if(i>0) {
							subtitleClinics+=", ";
						}
						if(Security.CurUser.ClinicIsRestricted) {
							subtitleClinics+=_listClinics[comboBoxMultiClinics.ListSelectedIndices[i]-1].Abbr;//Minus 1 for All.
						}
						else {//Not restricted
							if(comboBoxMultiClinics.ListSelectedIndices[i]==1) {//Unassigned option selected.
								subtitleClinics+=Lan.g(this,"Unassigned");
							}
							else {
								subtitleClinics+=_listClinics[comboBoxMultiClinics.ListSelectedIndices[i]-2].Abbr;//Minus 2 for All and Unassigned.
							}
						}
					}
				}
			}
			//This report will never show progress for printing.  This is because the report is being rebuilt whenever the grid is refreshed.
			_myReport=new ReportComplex(true,false,false);
			_myReport.ReportName=Lan.g(this,"Procedures Not Billed to Insurance");
			_myReport.AddTitle("Title",Lan.g(this,"Procedures Not Billed to Insurance"));
			_myReport.AddSubTitle("Practice Name",PrefC.GetString(PrefName.PracticeTitle));
			if(_myReportDateFrom==_myReportDateTo) {
				_myReport.AddSubTitle("Report Dates",_myReportDateFrom.ToShortDateString());
			}
			else {
				_myReport.AddSubTitle("Report Dates",_myReportDateFrom.ToShortDateString()+" - "+_myReportDateTo.ToShortDateString());
			}
			if(PrefC.HasClinicsEnabled) {
				_myReport.AddSubTitle("Clinics",subtitleClinics);
			}
			QueryObject query=_myReport.AddQuery(tableNotBilled,DateTimeOD.Today.ToShortDateString());
			query.AddColumn("Patient Name",_colWidthPatName,FieldValueType.String);
			query.AddColumn("Procedure Date",_colWidthProcDate,FieldValueType.Date);
			query.GetColumnDetail("Procedure Date").StringFormat="d";
			query.AddColumn("Procedure Description",300,FieldValueType.String);
			query.AddColumn("Amount",_colWidthAmount,FieldValueType.Number);
			_myReport.AddPageNum();
			if(!_myReport.SubmitQueries(false)) {//If we are loading and there are no results for _myReport do not show msgbox found in SubmitQueryies(...).
				return;
			}
		}
		
		//Only called in RefreshReport().
		private bool ValidateFields() {
			_myReportDateFrom=dateRangePicker.GetDateTimeFrom();
			_myReportDateTo=dateRangePicker.GetDateTimeTo();
			if(_myReportDateFrom>_myReportDateTo) {
				_myReportDateFrom=DateTime.MinValue;
				_myReportDateTo=DateTime.MaxValue;
			}
			if(PrefC.HasClinicsEnabled) {
				bool isAllClinics=comboBoxMultiClinics.ListSelectedIndices.Contains(0);
				if(!isAllClinics && comboBoxMultiClinics.SelectedIndices.Count==0) {
					comboBoxMultiClinics.SetSelected(0,true);//All clinics.
				}
			}
			if(_myReportDateFrom==DateTime.MinValue || _myReportDateTo==DateTime.MinValue) {
				return false;
			}
			return true;
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			FillGrid();
		}
		
		private void butPrint_Click(object sender,EventArgs e) {
			FormReportComplex FormR=new FormReportComplex(_myReport);
			FormR.ShowDialog();
		}
		
		private void butSelectAll_Click(object sender,EventArgs e) {
			gridMain.SetSelected(true);
		}

		private void butNewClaims_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {//No selections made.
				MsgBox.Show(this,"Please select at least one procedure.");
				return;
			}
			if(!ContrAccount.CheckClearinghouseDefaults()) {
				return;
			}
			//Generate List and Table----------------------------------------------------------------------------------------------------------------------
			//List of all procedures being shown.
			//Pulls procedures based off of the PatNum, if the row was selected in gridMain and if it has been attached to a claim.
			List<ProcNotBilled> listNotBilledProcs=new List<ProcNotBilled>();
			List<long> listPatNums=new List<long>();
			Patient patOld=new Patient();
			List<Claim> listPatClaims=new List<Claim>();
			List<ClaimProc> listPatClaimProcs=new List<ClaimProc>();
			List<ClaimProc> listCurClaimProcs=new List<ClaimProc>();
			//Table rows need to be 1:1 with gridMain rows due to logic in ContrAccount.toolBarButIns_Click(...).
			DataTable table=new DataTable();
			//Required columns as mentioned by ContrAccount.toolBarButIns_Click().
			table.Columns.Add("ProcNum");
			table.Columns.Add("chargesDouble");
			table.Columns.Add("ProcNumLab");
			for(int i=0;i<gridMain.Rows.Count;i++) {//Loop through gridMain to construct listNotBilledProcs.
				long procNumCur=PIn.Long(gridMain.Rows[i].Tag.ToString());//Tag is set to procNum in fillGrid().
				Procedure procCur=Procedures.GetOneProc(procNumCur,false);
				long patNumCur=procCur.PatNum;
				if(patOld.PatNum!=patNumCur) {//Procedures in gridMain are ordered by patient, so when the patient changes, we know previous patient is complete.
					listPatClaims=Claims.Refresh(patNumCur);
					listPatClaimProcs=ClaimProcs.Refresh(patNumCur);
					patOld=Patients.GetPat(procCur.PatNum);
				}
				listCurClaimProcs=ClaimProcs.GetForProc(listPatClaimProcs,procNumCur);
				bool hasPriClaim=false;
				bool hasSecClaim=false;
				for(int j=0;j<listCurClaimProcs.Count;j++) {
					ClaimProc claimProcCur=listCurClaimProcs[j];
					if(claimProcCur.ClaimNum > 0 && claimProcCur.Status!=ClaimProcStatus.Preauth && claimProcCur.Status!=ClaimProcStatus.Estimate) {
						Claim claimCur=Claims.GetFromList(listPatClaims,claimProcCur.ClaimNum);
						switch(claimCur.ClaimType) {
							case "P":
								hasPriClaim=true;
								break;
							case "S":
								hasSecClaim=true;
								break;
						}
					}
				}
				bool isSelected=gridMain.SelectedIndices.Contains(i);
				listNotBilledProcs.Add(new ProcNotBilled(patOld,procNumCur,i,isSelected,hasPriClaim,hasSecClaim,procCur.ClinicNum,procCur.PlaceService));
				DataRow row=table.NewRow();
				row["ProcNum"]=procNumCur;
				#region Calculate chargesDouble
				//Logic copied from AccountModules.GetAccount(...) line 857.
				double qty=(procCur.UnitQty+procCur.BaseUnits);
				if(qty==0){
					qty=1;
				}
				double writeOffCapSum=listPatClaimProcs.Where(x => x.Status==ClaimProcStatus.CapComplete).Select(y => y.WriteOff).ToList().Sum();
				row["chargesDouble"]=(procCur.ProcFee*qty)-writeOffCapSum;
				row["ProcNumLab"]=procCur.ProcNumLab;
				#endregion Calculate chargesDouble
				table.Rows.Add(row);
				if(listPatNums.Contains(patNumCur)) {
					continue;
				}
				listPatNums.Add(patNumCur);
			}
			List<List<ProcNotBilled>> listGroupedProcs=new List<List<ProcNotBilled>>();
			Patient patCur=null;
			List<PatPlan> listPatPlans=null;
			List<InsSub> listInsSubs=null;
			List<InsPlan> listInsPlans=null;
			List<Procedure> listPatientProcs=null;
			ProcNotBilled procNotBilled=new ProcNotBilled();//When automatically grouping,  this is used as the procedure to group by.
			long patNumOld=0;
			int claimCreatedCount=0;
			int patIndex=0;
			//The procedures show in the grid ordered by patient.  Also listPatNums contains unique patnums which are in the same order as the grid.
			while(patIndex < listPatNums.Count) {
				List<ProcNotBilled> listProcs=listNotBilledProcs.Where(x => x.Patient.PatNum==listPatNums[patIndex] && x.IsRowSelected && !x.IsAttached).ToList();
				if(listProcs.Count==0) {
					patNumOld=listPatNums[patIndex];
					patIndex++;//No procedures were selected for this patient.
					continue;
				}
				else {
					//Maintain the same patient, in order to create one or more additional claims for the remaining procedures.
					//Currently will only happen for specific instances; 
					//--Canadian customers who are attempting to create a claim with over 7 procedures.
					//--When checkAutoGroupProcs is checked and when there are multiple procedure groupings by GroupKey status, ClinicNum, and placeService.
				}
				if(patNumOld!=listPatNums[patIndex]) {//The patient could repeat if we had to group the procedures for the patinet into multiple claims.
					patCur=Patients.GetPat(listPatNums[patIndex]);
					listPatPlans=PatPlans.Refresh(patCur.PatNum);
					listInsSubs=InsSubs.RefreshForFam(Patients.GetFamily(patCur.PatNum));
					listInsPlans=InsPlans.RefreshForSubList(listInsSubs);
					listPatientProcs=Procedures.Refresh(patCur.PatNum);
				}
				if(checkAutoGroupProcs.Checked) {//Automatically Group Procedures.
					procNotBilled=listProcs[0];
					//Update listProcs to reflect those that match the procNotBilled values.
					listProcs=listProcs.FindAll(x => x.HasPriClaim==procNotBilled.HasPriClaim && x.HasSecClaim==procNotBilled.HasSecClaim);
					if(PrefC.HasClinicsEnabled) {//Group by clinic only if clinics enabled.
						listProcs=listProcs.FindAll(x => x.ClinicNum==procNotBilled.ClinicNum);
					}
					else if(!PrefC.GetBool(PrefName.EasyHidePublicHealth)) {//Group by Place of Service only if Public Health feature is enabled.
						listProcs=listProcs.FindAll(x => x.PlaceService==procNotBilled.PlaceService);
					}
				}
				GetUniqueDiagnosticCodes(listProcs,listPatientProcs,listPatPlans,listInsSubs,listInsPlans);
				if(listProcs.Count>7 && CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
					listProcs=listProcs.Take(7).ToList();//Returns first 7 items of the list.
				}
				listProcs.ForEach(x => x.IsAttached=true);//This way we can not attach procedures to multiple claims thanks to the logic above.
				if(listProcs.Any(x => listProcs[0].PlaceService!=x.PlaceService) || listProcs.Any(x => listProcs[0].ClinicNum!=x.ClinicNum)) {
					//Regardless if we are automatically grouping or not,
					//if all procs in our list at this point do not share the same PlaceService or ClinicNum then claims will not be made.
				}
				else {//Basic validation passed.
					if(!listProcs[0].HasPriClaim //Medical claim.
						&& PatPlans.GetOrdinal(PriSecMed.Medical,listPatPlans,listInsPlans,listInsSubs)>0 //Has medical ins.
						&& PatPlans.GetOrdinal(PriSecMed.Primary,listPatPlans,listInsPlans,listInsSubs)==0 //Does not have primary dental ins.
						&& PatPlans.GetOrdinal(PriSecMed.Secondary,listPatPlans,listInsPlans,listInsSubs)==0) //Does not have secondary dental ins.
					{
						claimCreatedCount++;
					}
					else {//Not a medical claim.
						if(!listProcs[0].HasPriClaim&&PatPlans.GetOrdinal(PriSecMed.Primary,listPatPlans,listInsPlans,listInsSubs)>0) {//Primary claim.
							claimCreatedCount++;
						}
						if(!listProcs[0].HasSecClaim&&PatPlans.GetOrdinal(PriSecMed.Secondary,listPatPlans,listInsPlans,listInsSubs)>0) {//Secondary claim.
							claimCreatedCount++;
						}
					}
				}
				listGroupedProcs.Add(listProcs);
			}
			if(!MsgBox.Show(this,true,"Clicking continue will create up to "+POut.Int(claimCreatedCount)+" claims and cannot be undone, except by manually going to each account.  "
				+"Some claims may not be created if there are validation issues.  Would you like to proceed?"))
			{
				return;
			}
			//Create Claims--------------------------------------------------------------------------------------------------------------------------------
			claimCreatedCount=0;
			string claimErrors="";
			foreach(List<ProcNotBilled> listProcs in listGroupedProcs) { 
				patCur=listProcs[0].Patient;
				gridMain.SetSelected(false);//Need to deslect all rows each time so that ContrAccount.toolBarButIns_Click(...) only uses pertinent rows.
				for(int j=0;j<listProcs.Count;j++) {
					gridMain.SetSelected(listProcs[j].RowIndex,true);//Select the pertinent rows so that they will be attached to the claim below.
				}
				ContrAccount.toolBarButIns_Click(false,patCur,Patients.GetFamily(patCur.PatNum),gridMain,table,
					procNotBilled.HasPriClaim,procNotBilled.HasSecClaim);
				string errorTitle=patCur.PatNum+" "+patCur.GetNameLFnoPref()+" - ";
				if(patNumOld==patCur.PatNum && !string.IsNullOrEmpty(ContrAccount.ClaimErrorsCur)){ 
					claimErrors+="\t\t"+ContrAccount.ClaimErrorsCur+"\r\n";
				}
				else if(!string.IsNullOrEmpty(ContrAccount.ClaimErrorsCur)){
					claimErrors+=errorTitle+ContrAccount.ClaimErrorsCur+"\r\n";
				}
				claimCreatedCount+=ContrAccount.ClaimCreatedCount;
				patNumOld=patCur.PatNum;
			}
			FillGrid();
			if(!string.IsNullOrEmpty(claimErrors)) {
				MsgBoxCopyPaste form=new MsgBoxCopyPaste(claimErrors);
				form.ShowDialog();
			}
			MessageBox.Show(Lan.g(this,"Number of claims created")+": "+claimCreatedCount);
		}
		
		///<summary>Mimics ContrAccount.CreateClaim(...).  Removes items from listProcs until unique diagnosis code count is low enough.</summary>
		private void GetUniqueDiagnosticCodes(List<ProcNotBilled> listProcs,List<Procedure> listPatProcs,List <PatPlan> listPatPlans,
			List<InsSub> listInsSubs,List<InsPlan> listInsPlans)
		{
			List<Procedure> listProcedures=new List<Procedure>();
			for(int i=0;i<listProcs.Count;i++) {
				listProcedures.Add(Procedures.GetProcFromList(listPatProcs,listProcs[i].ProcNum));
			}
			//If they have medical insurance and no dental, make the claim type Medical.  This is to avoid the scenario of multiple med ins and no dental.
			bool isMedical=false;
			if(PatPlans.GetOrdinal(PriSecMed.Medical,listPatPlans,listInsPlans,listInsSubs)>0
				&& PatPlans.GetOrdinal(PriSecMed.Primary,listPatPlans,listInsPlans,listInsSubs)==0
				&& PatPlans.GetOrdinal(PriSecMed.Secondary,listPatPlans,listInsPlans,listInsSubs)==0)
			{
				isMedical=true;
			}
			while(!isMedical && Procedures.GetUniqueDiagnosticCodes(listProcedures,false).Count > 4) {//dental
				int index=listProcedures.Count-1;
				listProcedures.RemoveAt(index);
				listProcs.RemoveAt(index);
			}
			while(isMedical && Procedures.GetUniqueDiagnosticCodes(listProcedures,true).Count > 12) {//medical
				int index=listProcedures.Count-1;
				listProcedures.RemoveAt(index);
				listProcs.RemoveAt(index);
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormRpProcNotBilledIns_FormClosing(object sender,FormClosingEventArgs e) {
			Prefs.UpdateBool(PrefName.ClaimProcsNotBilledToInsAutoGroup,checkAutoGroupProcs.Checked);
		}

	}//end class FormRpProcNotBilledIns

	///<summary>Used so that we can easily select pertinent procedures for a specific patient when creating claims.</summary>
	internal class ProcNotBilled {
		public Patient Patient;
		public long ProcNum;
		public int RowIndex;
		public bool IsRowSelected;
		///<summary>Flag used to make sure we do not attach procedures to multiple claims.
		///Very important for Canadian customers when we need to make multiple claims.</summary>
		public bool IsAttached;
		public bool HasPriClaim;
		public bool HasSecClaim;
		public long ClinicNum;
		public PlaceOfService PlaceService;

		public ProcNotBilled() {
			HasPriClaim=false;
			HasSecClaim=false;
		}

		public ProcNotBilled(Patient pat,long procNum,int rowIndex,bool isRowSelected,
			bool hasPriClaim,bool hasSecClaim,long clinicNum,PlaceOfService placeService)
		{
			Patient=pat;
			ProcNum=procNum;
			RowIndex=rowIndex;
			IsRowSelected=isRowSelected;
			IsAttached=false;
			HasPriClaim=hasPriClaim;
			HasSecClaim=hasSecClaim;
			ClinicNum=clinicNum;
			PlaceService=placeService;
		}
	}

}