using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;

namespace OpenDental{
	/// <summary>The Next appoinment tracking tool.</summary>
	public class FormTrackNext : ODForm {
		private OpenDental.UI.Button butClose;
		private List<Appointment> _listPlannedAppts;
		private OpenDental.UI.ODGrid gridMain;
		private ComboBox comboProv;
		private Label label4;
		private OpenDental.UI.Button butRefresh;
		private ComboBox comboOrder;
		private Label label1;
		private OpenDental.UI.Button butPrint;
		private int pagesPrinted;
		private bool headingPrinted;
		private PrintDocument pd;
		private ComboBox comboSite;
		private Label labelSite;
		private int headingPrintH;
		private ComboBox comboClinic;
		private Label labelClinic;
		private List<Clinic> _listUserClinics;
		private IContainer components;
		private ToolStripMenuItem toolStripMenuItemSelectPatient;
		private ToolStripMenuItem toolStripMenuItemSeeChart;
		private ToolStripMenuItem toolStripMenuItemSendToPinboard;
		private ODDateRangePicker dateRangePicker;
		private Label label5;
		private TextBox textCodeRange;
		private Label label6;
		private MenuStrip menuMain;
		private ToolStripMenuItem setupToolStripMenuItem;
		private ContextMenuStrip menuRightClick;
		private List<Provider> _listProviders;
		private List<Site> _listSites;

		///<summary>PatientGoTo must be set before calling Show() or ShowDialog().</summary>
		public FormTrackNext(){
			InitializeComponent();// Required for Windows Form Designer support
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTrackNext));
			this.butClose = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.menuRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItemSelectPatient = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemSeeChart = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemSendToPinboard = new System.Windows.Forms.ToolStripMenuItem();
			this.comboProv = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.butRefresh = new OpenDental.UI.Button();
			this.comboOrder = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.butPrint = new OpenDental.UI.Button();
			this.comboSite = new System.Windows.Forms.ComboBox();
			this.labelSite = new System.Windows.Forms.Label();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.dateRangePicker = new OpenDental.UI.ODDateRangePicker();
			this.label5 = new System.Windows.Forms.Label();
			this.textCodeRange = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.menuMain = new System.Windows.Forms.MenuStrip();
			this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuRightClick.SuspendLayout();
			this.menuMain.SuspendLayout();
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
			this.butClose.Location = new System.Drawing.Point(758, 624);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(87, 24);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.ContextMenuStrip = this.menuRightClick;
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(12, 107);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(737, 541);
			this.gridMain.TabIndex = 2;
			this.gridMain.Title = "Planned Appointments";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "FormTrackNext";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grid_MouseUp);
			// 
			// menuRightClick
			// 
			this.menuRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSelectPatient,
            this.toolStripMenuItemSeeChart,
            this.toolStripMenuItemSendToPinboard});
			this.menuRightClick.Name = "menuRightClick";
			this.menuRightClick.Size = new System.Drawing.Size(166, 70);
			// 
			// toolStripMenuItemSelectPatient
			// 
			this.toolStripMenuItemSelectPatient.Name = "toolStripMenuItemSelectPatient";
			this.toolStripMenuItemSelectPatient.Size = new System.Drawing.Size(165, 22);
			this.toolStripMenuItemSelectPatient.Text = "Select Patient";
			this.toolStripMenuItemSelectPatient.Click += new System.EventHandler(this.menuRight_click);
			// 
			// toolStripMenuItemSeeChart
			// 
			this.toolStripMenuItemSeeChart.Name = "toolStripMenuItemSeeChart";
			this.toolStripMenuItemSeeChart.Size = new System.Drawing.Size(165, 22);
			this.toolStripMenuItemSeeChart.Text = "See Chart";
			this.toolStripMenuItemSeeChart.Click += new System.EventHandler(this.menuRight_click);
			// 
			// toolStripMenuItemSendToPinboard
			// 
			this.toolStripMenuItemSendToPinboard.Name = "toolStripMenuItemSendToPinboard";
			this.toolStripMenuItemSendToPinboard.Size = new System.Drawing.Size(165, 22);
			this.toolStripMenuItemSendToPinboard.Text = "Send to Pinboard";
			this.toolStripMenuItemSendToPinboard.Click += new System.EventHandler(this.menuRight_click);
			// 
			// comboProv
			// 
			this.comboProv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProv.Location = new System.Drawing.Point(336, 29);
			this.comboProv.MaxDropDownItems = 40;
			this.comboProv.Name = "comboProv";
			this.comboProv.Size = new System.Drawing.Size(181, 21);
			this.comboProv.TabIndex = 26;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(244, 33);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(91, 14);
			this.label4.TabIndex = 25;
			this.label4.Text = "Provider";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(758, 29);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(87, 24);
			this.butRefresh.TabIndex = 24;
			this.butRefresh.Text = "&Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// comboOrder
			// 
			this.comboOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboOrder.Location = new System.Drawing.Point(104, 29);
			this.comboOrder.MaxDropDownItems = 40;
			this.comboOrder.Name = "comboOrder";
			this.comboOrder.Size = new System.Drawing.Size(133, 21);
			this.comboOrder.TabIndex = 30;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(11, 33);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(91, 14);
			this.label1.TabIndex = 29;
			this.label1.Text = "Order by";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(758, 576);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(87, 24);
			this.butPrint.TabIndex = 31;
			this.butPrint.Text = "Print List";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// comboSite
			// 
			this.comboSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboSite.Location = new System.Drawing.Point(336, 55);
			this.comboSite.MaxDropDownItems = 40;
			this.comboSite.Name = "comboSite";
			this.comboSite.Size = new System.Drawing.Size(181, 21);
			this.comboSite.TabIndex = 33;
			// 
			// labelSite
			// 
			this.labelSite.Location = new System.Drawing.Point(243, 57);
			this.labelSite.Name = "labelSite";
			this.labelSite.Size = new System.Drawing.Size(91, 14);
			this.labelSite.TabIndex = 32;
			this.labelSite.Text = "Site";
			this.labelSite.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(613, 29);
			this.comboClinic.MaxDropDownItems = 40;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(133, 21);
			this.comboClinic.TabIndex = 35;
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(520, 33);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(91, 14);
			this.labelClinic.TabIndex = 34;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// dateRangePicker
			// 
			this.dateRangePicker.BackColor = System.Drawing.SystemColors.Control;
			this.dateRangePicker.DefaultDateTimeFrom = new System.DateTime(2017, 1, 1, 0, 0, 0, 0);
			this.dateRangePicker.DefaultDateTimeTo = new System.DateTime(2017, 8, 11, 0, 0, 0, 0);
			this.dateRangePicker.EnableWeekButtons = false;
			this.dateRangePicker.Location = new System.Drawing.Point(64, 79);
			this.dateRangePicker.MaximumSize = new System.Drawing.Size(0, 185);
			this.dateRangePicker.MinimumSize = new System.Drawing.Size(453, 22);
			this.dateRangePicker.Name = "dateRangePicker";
			this.dateRangePicker.Size = new System.Drawing.Size(497, 22);
			this.dateRangePicker.TabIndex = 36;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(613, 77);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(108, 13);
			this.label5.TabIndex = 47;
			this.label5.Text = "Ex: D2140-D2394";
			// 
			// textCodeRange
			// 
			this.textCodeRange.Location = new System.Drawing.Point(613, 55);
			this.textCodeRange.Name = "textCodeRange";
			this.textCodeRange.Size = new System.Drawing.Size(133, 20);
			this.textCodeRange.TabIndex = 45;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(534, 57);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(77, 17);
			this.label6.TabIndex = 46;
			this.label6.Text = "Code Range";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// menuMain
			// 
			this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setupToolStripMenuItem});
			this.menuMain.Location = new System.Drawing.Point(0, 0);
			this.menuMain.Name = "menuMain";
			this.menuMain.Size = new System.Drawing.Size(855, 24);
			this.menuMain.TabIndex = 48;
			// 
			// setupToolStripMenuItem
			// 
			this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
			this.setupToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
			this.setupToolStripMenuItem.Text = "Setup";
			this.setupToolStripMenuItem.Click += new System.EventHandler(this.setupToolStripMenuItem_Click);
			// 
			// FormTrackNext
			// 
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(855, 664);
			this.Controls.Add(this.menuMain);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textCodeRange);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.dateRangePicker);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.comboSite);
			this.Controls.Add(this.labelSite);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.comboOrder);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboProv);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(871, 345);
			this.Name = "FormTrackNext";
			this.Text = "Track Planned Appointments";
			this.Load += new System.EventHandler(this.FormTrackNext_Load);
			this.menuRightClick.ResumeLayout(false);
			this.menuMain.ResumeLayout(false);
			this.menuMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormTrackNext_Load(object sender, System.EventArgs e) {
			Cursor=Cursors.WaitCursor;
			comboOrder.Items.Add(Lan.g(this,"Status"));
			comboOrder.Items.Add(Lan.g(this,"Alphabetical"));
			comboOrder.Items.Add(Lan.g(this,"Date"));
			comboOrder.SelectedIndex=0;
			comboProv.Items.Add(Lan.g(this,"All"));
			comboProv.SelectedIndex=0;
			_listProviders=Providers.GetDeepCopy(true);
			for(int i=0;i<_listProviders.Count;i++) {
				comboProv.Items.Add(_listProviders[i].GetLongDesc());
			}
			if(PrefC.GetBool(PrefName.EasyHidePublicHealth)){
				comboSite.Visible=false;
				labelSite.Visible=false;
			}
			else{
				comboSite.Items.Add(Lan.g(this,"All"));
				comboSite.SelectedIndex=0;
				_listSites=Sites.GetDeepCopy();
				for(int i=0;i<_listSites.Count;i++) {
					comboSite.Items.Add(_listSites[i].Description);
				}
			}
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				comboClinic.Visible=false;
				labelClinic.Visible=false;
			}
			else {
				if(!Security.CurUser.ClinicIsRestricted) {
					comboClinic.Items.Add(Lan.g(this,"All"));
					comboClinic.SelectedIndex=0;
				}
				_listUserClinics=Clinics.GetForUserod(Security.CurUser);
				for(int i=0;i<_listUserClinics.Count;i++) {
					comboClinic.Items.Add(_listUserClinics[i].Abbr);
					if(_listUserClinics[i].ClinicNum==Clinics.ClinicNum) {
						comboClinic.SelectedIndex=i;
						if(!Security.CurUser.ClinicIsRestricted) {
							comboClinic.SelectedIndex++;//add 1 for "All"
						}
					}
				}
			}
			InitDateRange();
			RefreshAptList();
			FillGrid();
			Cursor=Cursors.Default;
		}

		private void InitDateRange() {
			int dayCount=PrefC.GetInt(PrefName.PlannedApptDaysPast);
			dateRangePicker.SetDateTimeFrom(DateTime.Today.AddDays(-dayCount));
			dayCount=PrefC.GetInt(PrefName.PlannedApptDaysFuture);
			dateRangePicker.SetDateTimeTo(DateTime.Today.AddDays(dayCount));
		}
		
		private void setupToolStripMenuItem_Click(object sender,EventArgs e) {
			FormTrackNextSetup formTNS=new FormTrackNextSetup();
			if(formTNS.ShowDialog()==DialogResult.OK) {
				InitDateRange();
			}
		}

		private void menuRight_click(object sender,System.EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an appointment first.");
				return;
			}
			switch(menuRightClick.Items.IndexOf((ToolStripMenuItem)sender)) {
				case 0: 
					SelectPatient_Click();
					break;
				case 1:
					SeeChart_Click();
					break;
				case 2:
					SendPinboard_Click();
					break;
			}
		}

		private void FillGrid(){
			Cursor=Cursors.WaitCursor;
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Patient"),140);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Date"),65);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Status"),110);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Prov"),50);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Procedures"),150);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Notes"),200);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			Dictionary<long,string> dictPatNames=Patients.GetLimForPats(_listPlannedAppts.Select(x => x.PatNum).ToList())
				.ToDictionary(x => x.PatNum,x => x.GetNameLF());
			foreach(Appointment apt in _listPlannedAppts){
				row=new ODGridRow();
				string patName=Lan.g(this,"UNKNOWN");
				dictPatNames.TryGetValue(apt.PatNum,out patName);
				row.Cells.Add(patName);
				if(apt.AptDateTime.Year<1880){
					row.Cells.Add("");
				}
				else{
					row.Cells.Add(apt.AptDateTime.ToShortDateString());
				}
				row.Cells.Add(Defs.GetName(DefCat.RecallUnschedStatus,apt.UnschedStatus));
				if(apt.IsHygiene) {
					Provider provHyg=Providers.GetFirstOrDefault(x => x.ProvNum==apt.ProvHyg);
					row.Cells.Add(provHyg==null?Lan.g(this,"INVALID"):provHyg.Abbr);
				}
				else {
					Provider prov=Providers.GetFirstOrDefault(x => x.ProvNum==apt.ProvNum);
					row.Cells.Add(prov==null?Lan.g(this,"INVALID"):prov.Abbr);
				}
				row.Cells.Add(apt.ProcDescript);
				row.Cells.Add(apt.Note);
			  
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			Cursor=Cursors.Default;
		}

		private void RefreshAptList() {
			string order="";
			switch(comboOrder.SelectedIndex) {
				case 0:
					order="status";
					break;
				case 1:
					order="alph";
					break;
				case 2:
					order="date";
					break;
			}
			long provNum=0;
			if(comboProv.SelectedIndex!=0) {
				provNum=_listProviders[comboProv.SelectedIndex-1].ProvNum;
			}
			long siteNum=0;
			if(!PrefC.GetBool(PrefName.EasyHidePublicHealth) && comboSite.SelectedIndex!=0) {
				siteNum=_listSites[comboSite.SelectedIndex-1].SiteNum;
			}
			long clinicNum=0;
			//if clinics are not enabled, comboClinic.SelectedIndex will be -1, so clinicNum will be 0 and list will not be filtered by clinic
			if(Security.CurUser.ClinicIsRestricted && comboClinic.SelectedIndex>-1) {
				clinicNum=_listUserClinics[comboClinic.SelectedIndex].ClinicNum;
			}
			else if(comboClinic.SelectedIndex > 0) {//if user is not restricted, clinicNum will be 0 and the query will get all clinic data
				clinicNum=_listUserClinics[comboClinic.SelectedIndex-1].ClinicNum;//if user is not restricted, comboClinic will contain "All" so minus 1
			}
			string codeStart="";
			string codeEnd="";
			if(textCodeRange.Text.Trim()!="") {
				if(textCodeRange.Text.Contains("-")) {
					string[] codeSplit=textCodeRange.Text.Split('-');
					codeStart=codeSplit[0].Trim().Replace('d','D');
					codeEnd=codeSplit[1].Trim().Replace('d','D');
				}
				else {
					codeStart=textCodeRange.Text.Trim().Replace('d','D');
					codeEnd=textCodeRange.Text.Trim().Replace('d','D');
				}
			}
			_listPlannedAppts=Appointments.RefreshPlannedTracker(order,provNum,siteNum,clinicNum,codeStart,codeEnd,dateRangePicker.GetDateTimeFrom(),dateRangePicker.GetDateTimeTo());
		}

		private void grid_MouseUp(object sender,MouseEventArgs e) {
			if(e.Button==MouseButtons.Right && gridMain.SelectedIndices.Length>0) {
				Patient pat=Patients.GetLim(_listPlannedAppts[gridMain.SelectedIndices[gridMain.SelectedIndices.Length-1]].PatNum);
				toolStripMenuItemSelectPatient.Text=Lan.g(gridMain.TranslationName,"Select Patient")+" ("+pat.GetNameFL()+")";
			}
		}

		private void SelectPatient_Click() {
			//If multiple selected, just take the last one to remain consistent with SendPinboard_Click.
			long patNum=_listPlannedAppts[gridMain.SelectedIndices[gridMain.SelectedIndices.Length-1]].PatNum;
			Patient pat=Patients.GetPat(patNum);
			FormOpenDental.S_Contr_PatientSelected(pat,true);
		}

		private void SeeChart_Click() {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an appointment first.");
				return;
			}
			//Only one can be selected at a time in this grid, but just in case we change it in the future it will select the last one in the list to be consistent with other patient selections.
			Patient pat=Patients.GetPat(_listPlannedAppts[gridMain.SelectedIndices[gridMain.SelectedIndices.Length-1]].PatNum);
			FormOpenDental.S_Contr_PatientSelected(pat,false);
			GotoModule.GotoChart(pat.PatNum);
		}

		private void SendPinboard_Click() {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an appointment first.");
				return;
			}
			List<long> listAppts=new List<long>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				listAppts.Add(_listPlannedAppts[gridMain.SelectedIndices[i]].AptNum);//Will only be one unless multiselect is enabled in the future
				_listPlannedAppts.RemoveAt(gridMain.SelectedIndices[i]);
			}
			GotoModule.PinToAppt(listAppts,0);//This will send all appointments in _listAptSelected to the pinboard, and will select the patient attached to the last appointment in _listAptSelected.
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			int currentSelection=gridMain.GetSelectedIndex();
			int currentScroll=gridMain.ScrollValue;
			Patient pat=Patients.GetPat(_listPlannedAppts[e.Row].PatNum);//Only one can be selected at a time in this grid.
			FormOpenDental.S_Contr_PatientSelected(pat,true);
			FormApptEdit FormAE=new FormApptEdit(_listPlannedAppts[e.Row].AptNum);
			FormAE.PinIsVisible=true;
			FormAE.ShowDialog();
			if(FormAE.DialogResult!=DialogResult.OK) {
				return;
			}
			if(FormAE.PinClicked) {
				SendPinboard_Click();
				DialogResult=DialogResult.OK;
			}
			else {
				RefreshAptList();
				FillGrid();
				gridMain.SetSelected(currentSelection,true);
				gridMain.ScrollValue=currentScroll;
			}
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			RefreshAptList();
			FillGrid();
		}

		private void butPrint_Click(object sender,EventArgs e) {
			pagesPrinted=0;
			pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			//pd.OriginAtMargins=true;
			if(pd.DefaultPageSettings.PrintableArea.Height==0) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			headingPrinted=false;
			#if DEBUG
				FormRpPrintPreview pView = new FormRpPrintPreview();
				pView.printPreviewControl2.Document=pd;
				pView.ShowDialog();
			#else
				if(!PrinterL.SetPrinter(pd,PrintSituation.Default,0,"Planned appointment tracker list printed")) {
					return;
				}
				try{
					pd.Print();
				}
				catch {
					MsgBox.Show(this,"Printer not available");
				}
			#endif	
		}

		private void pd_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
			//new Rectangle(50,40,800,1035);//Some printers can handle up to 1042
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			if(!headingPrinted) {
				text=Lan.g(this,"Planned Appointment Tracker");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				//yPos+=(int)g.MeasureString(text,headingFont).Height;
				//text=textDateFrom.Text+" "+Lan.g(this,"to")+" "+textDateTo.Text;
				//g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=25;
				headingPrinted=true;
				headingPrintH=yPos;
			}
			#endregion
			yPos=gridMain.PrintPage(g,pagesPrinted,bounds,headingPrintH);
			pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
			}
			g.Dispose();
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

	}
}