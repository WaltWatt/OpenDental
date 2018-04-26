using OpenDental.ReportingComplex;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data;

namespace OpenDental
{
	/// <summary>
	/// Summary description for FormRpApptWithPhones.
	/// </summary>
	public class FormRpAppointments : ODForm {
		private System.Windows.Forms.ListBox listProvs;
		private System.Windows.Forms.Label label1;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private CheckBox checkWebSchedRecall;
		private Label label2;
		private Label label3;
		private ValidDate textDateTo;
		private ValidDate textDateFrom;
		private UI.Button butTomorrow;
		private UI.Button butToday;
		private GroupBox groupBox1;
		private ListBox listClinics;
		private Label labelClinics;
		private List<Clinic> _listClinics;
		private CheckBox checkAllClinics;
		private CheckBox checkAllProvs;
		private List<Provider> _listProviders;
		private GroupBox groupBox2;
		private CheckBox checkWebSchedNewPat;
		private bool _hasClinicsEnabled;
		private RadioButton radioDateAptCreated;
		private RadioButton radioAptDate;
		private CheckBox checkShowNoteAppts;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		///<summary></summary>
		public FormRpAppointments()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpAppointments));
			this.listProvs = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.checkWebSchedRecall = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textDateTo = new OpenDental.ValidDate();
			this.textDateFrom = new OpenDental.ValidDate();
			this.butTomorrow = new OpenDental.UI.Button();
			this.butToday = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioDateAptCreated = new System.Windows.Forms.RadioButton();
			this.radioAptDate = new System.Windows.Forms.RadioButton();
			this.listClinics = new System.Windows.Forms.ListBox();
			this.labelClinics = new System.Windows.Forms.Label();
			this.checkAllClinics = new System.Windows.Forms.CheckBox();
			this.checkAllProvs = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkWebSchedNewPat = new System.Windows.Forms.CheckBox();
			this.checkShowNoteAppts = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// listProvs
			// 
			this.listProvs.Location = new System.Drawing.Point(12, 57);
			this.listProvs.Name = "listProvs";
			this.listProvs.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listProvs.Size = new System.Drawing.Size(120, 212);
			this.listProvs.TabIndex = 33;
			this.listProvs.Click += new System.EventHandler(this.listProvs_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 16);
			this.label1.TabIndex = 32;
			this.label1.Text = "Providers";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(518, 345);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 44;
			this.butCancel.Text = "&Cancel";
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(437, 345);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 43;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// checkWebSchedRecall
			// 
			this.checkWebSchedRecall.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkWebSchedRecall.Location = new System.Drawing.Point(60, 21);
			this.checkWebSchedRecall.Name = "checkWebSchedRecall";
			this.checkWebSchedRecall.Size = new System.Drawing.Size(224, 18);
			this.checkWebSchedRecall.TabIndex = 46;
			this.checkWebSchedRecall.Text = "Show Recall Appointments";
			this.checkWebSchedRecall.CheckedChanged += new System.EventHandler(this.checkWebSchedRecall_CheckedChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(9, 18);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(51, 18);
			this.label2.TabIndex = 37;
			this.label2.Text = "From";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 44);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(51, 18);
			this.label3.TabIndex = 39;
			this.label3.Text = "To";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDateTo
			// 
			this.textDateTo.Location = new System.Drawing.Point(61, 43);
			this.textDateTo.Name = "textDateTo";
			this.textDateTo.Size = new System.Drawing.Size(100, 20);
			this.textDateTo.TabIndex = 44;
			// 
			// textDateFrom
			// 
			this.textDateFrom.Location = new System.Drawing.Point(61, 16);
			this.textDateFrom.Name = "textDateFrom";
			this.textDateFrom.Size = new System.Drawing.Size(100, 20);
			this.textDateFrom.TabIndex = 43;
			// 
			// butTomorrow
			// 
			this.butTomorrow.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butTomorrow.Autosize = true;
			this.butTomorrow.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butTomorrow.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butTomorrow.CornerRadius = 4F;
			this.butTomorrow.Location = new System.Drawing.Point(164, 41);
			this.butTomorrow.Name = "butTomorrow";
			this.butTomorrow.Size = new System.Drawing.Size(96, 23);
			this.butTomorrow.TabIndex = 45;
			this.butTomorrow.Text = "Tomorrow";
			this.butTomorrow.Click += new System.EventHandler(this.butTomorrow_Click);
			// 
			// butToday
			// 
			this.butToday.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butToday.Autosize = true;
			this.butToday.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butToday.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butToday.CornerRadius = 4F;
			this.butToday.Location = new System.Drawing.Point(164, 15);
			this.butToday.Name = "butToday";
			this.butToday.Size = new System.Drawing.Size(96, 23);
			this.butToday.TabIndex = 46;
			this.butToday.Text = "Today";
			this.butToday.Click += new System.EventHandler(this.butToday_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.checkShowNoteAppts);
			this.groupBox1.Controls.Add(this.radioDateAptCreated);
			this.groupBox1.Controls.Add(this.radioAptDate);
			this.groupBox1.Controls.Add(this.butToday);
			this.groupBox1.Controls.Add(this.butTomorrow);
			this.groupBox1.Controls.Add(this.textDateFrom);
			this.groupBox1.Controls.Add(this.textDateTo);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(264, 51);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(322, 143);
			this.groupBox1.TabIndex = 45;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Date Range";
			// 
			// radioDateAptCreated
			// 
			this.radioDateAptCreated.Location = new System.Drawing.Point(60, 92);
			this.radioDateAptCreated.Name = "radioDateAptCreated";
			this.radioDateAptCreated.Size = new System.Drawing.Size(224, 17);
			this.radioDateAptCreated.TabIndex = 48;
			this.radioDateAptCreated.Text = "Appointment Date Created";
			this.radioDateAptCreated.UseVisualStyleBackColor = true;
			// 
			// radioAptDate
			// 
			this.radioAptDate.Checked = true;
			this.radioAptDate.Location = new System.Drawing.Point(60, 72);
			this.radioAptDate.Name = "radioAptDate";
			this.radioAptDate.Size = new System.Drawing.Size(224, 17);
			this.radioAptDate.TabIndex = 47;
			this.radioAptDate.TabStop = true;
			this.radioAptDate.Text = "Appointment Date";
			this.radioAptDate.UseVisualStyleBackColor = true;
			// 
			// listClinics
			// 
			this.listClinics.Location = new System.Drawing.Point(138, 57);
			this.listClinics.Name = "listClinics";
			this.listClinics.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listClinics.Size = new System.Drawing.Size(120, 212);
			this.listClinics.TabIndex = 48;
			this.listClinics.Click += new System.EventHandler(this.listClinics_Click);
			// 
			// labelClinics
			// 
			this.labelClinics.Location = new System.Drawing.Point(138, 19);
			this.labelClinics.Name = "labelClinics";
			this.labelClinics.Size = new System.Drawing.Size(104, 16);
			this.labelClinics.TabIndex = 47;
			this.labelClinics.Text = "Clinics";
			this.labelClinics.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// checkAllClinics
			// 
			this.checkAllClinics.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllClinics.Location = new System.Drawing.Point(138, 38);
			this.checkAllClinics.Name = "checkAllClinics";
			this.checkAllClinics.Size = new System.Drawing.Size(95, 16);
			this.checkAllClinics.TabIndex = 50;
			this.checkAllClinics.Text = "All";
			this.checkAllClinics.Click += new System.EventHandler(this.checkAllClinics_Click);
			// 
			// checkAllProvs
			// 
			this.checkAllProvs.Checked = true;
			this.checkAllProvs.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkAllProvs.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllProvs.Location = new System.Drawing.Point(12, 38);
			this.checkAllProvs.Name = "checkAllProvs";
			this.checkAllProvs.Size = new System.Drawing.Size(95, 16);
			this.checkAllProvs.TabIndex = 51;
			this.checkAllProvs.Text = "All";
			this.checkAllProvs.Click += new System.EventHandler(this.checkAllProvs_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkWebSchedNewPat);
			this.groupBox2.Controls.Add(this.checkWebSchedRecall);
			this.groupBox2.Location = new System.Drawing.Point(264, 200);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(322, 69);
			this.groupBox2.TabIndex = 47;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Web Sched Appointments Only";
			// 
			// checkWebSchedNewPat
			// 
			this.checkWebSchedNewPat.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkWebSchedNewPat.Location = new System.Drawing.Point(60, 45);
			this.checkWebSchedNewPat.Name = "checkWebSchedNewPat";
			this.checkWebSchedNewPat.Size = new System.Drawing.Size(224, 18);
			this.checkWebSchedNewPat.TabIndex = 48;
			this.checkWebSchedNewPat.Text = "Show New Patient Appointments";
			this.checkWebSchedNewPat.CheckedChanged += new System.EventHandler(this.checkWebSchedRecall_CheckedChanged);
			// 
			// checkShowNoteAppts
			// 
			this.checkShowNoteAppts.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkShowNoteAppts.Location = new System.Drawing.Point(60, 115);
			this.checkShowNoteAppts.Name = "checkShowNoteAppts";
			this.checkShowNoteAppts.Size = new System.Drawing.Size(202, 18);
			this.checkShowNoteAppts.TabIndex = 49;
			this.checkShowNoteAppts.Text = "Show \"Note\" Appointments";
			// 
			// FormRpAppointments
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(617, 383);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.checkAllProvs);
			this.Controls.Add(this.checkAllClinics);
			this.Controls.Add(this.listClinics);
			this.Controls.Add(this.labelClinics);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.listProvs);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(606, 310);
			this.Name = "FormRpAppointments";
			this.Text = "Appointments Report";
			this.Load += new System.EventHandler(this.FormRpApptWithPhones_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormRpApptWithPhones_Load(object sender,System.EventArgs e) {
			_listProviders=Providers.GetListReports();
			for(int i=0;i<_listProviders.Count;i++) {
				listProvs.Items.Add(_listProviders[i].GetLongDesc());
			}
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				labelClinics.Visible=false;
				checkAllClinics.Visible=false;
				listClinics.Visible=false;
				_hasClinicsEnabled=false;
			}
			else {//Clinics enabled.
				_hasClinicsEnabled=true;
				_listClinics=Clinics.GetForUserod(Security.CurUser);
				listClinics.Items.Clear();
				if(!Security.CurUser.ClinicIsRestricted) {
					listClinics.Items.Add(Lan.g(this,"Unassigned"));
					listClinics.SetSelected(0,true);
				}
				for(int i=0;i<_listClinics.Count;i++) {
					int curIndex=listClinics.Items.Add(_listClinics[i].Abbr);
					if(_listClinics[i].ClinicNum==Clinics.ClinicNum) {
						listClinics.SelectedIndices.Clear();
						listClinics.SetSelected(curIndex,true);
					}
				}
			}
			SetTomorrow();
		}

		private void SetTomorrow() {
			textDateFrom.Text=DateTime.Today.AddDays(1).ToShortDateString();
			textDateTo.Text=DateTime.Today.AddDays(1).ToShortDateString();
		}

		///<summary>Validates the fields on the form.  Returns false is something is not filled out correctly.</summary>
		private bool IsValid() {
			//validate user input
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!="") 
			{
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return false;
			}
			if(textDateFrom.Text.Length==0
				|| textDateTo.Text.Length==0) 
			{
				MessageBox.Show(Lan.g(this,"From and To dates are required."));
				return false;
			}
			DateTime dateFrom=PIn.Date(textDateFrom.Text);
			DateTime dateTo=PIn.Date(textDateTo.Text);
			if(dateTo < dateFrom) {
				MessageBox.Show(Lan.g(this,"To date cannot be before From date."));
				return false;
			}
			if(!checkAllProvs.Checked && listProvs.SelectedIndices.Count==0) {
				MessageBox.Show(Lan.g(this,"You must select at least one provider."));
				return false;
			}
			if(_hasClinicsEnabled) {//Not no clinics.
				if(!checkAllClinics.Checked && listClinics.SelectedIndices.Count==0) {
					MsgBox.Show(this,"You must select at least one clinic.");
					return false;
				}
			}
			return true;
		}

		private void checkAllProvs_Click(object sender,EventArgs e) {
			if(checkAllProvs.Checked) {
				listProvs.SelectedIndices.Clear();
			}
		}

		private void checkAllClinics_Click(object sender,EventArgs e) {
			if(checkAllClinics.Checked) {
				for(int i=0;i<listClinics.Items.Count;i++) {
					listClinics.SetSelected(i,true);
				}
			}
			else {
				listClinics.SelectedIndices.Clear();
			}
		}

		private void listProvs_Click(object sender,EventArgs e) {
			if(listProvs.SelectedIndices.Count>0) {
				checkAllProvs.Checked=false;
			}
		}

		private void listClinics_Click(object sender,EventArgs e) {
			if(listClinics.SelectedIndices.Count>0) {
				checkAllClinics.Checked=false;
			}
		}

		private void butToday_Click(object sender, System.EventArgs e) {
			textDateFrom.Text=DateTime.Today.ToShortDateString();
			textDateTo.Text=DateTime.Today.ToShortDateString();
		}

		private void butTomorrow_Click(object sender,System.EventArgs e) {
			SetTomorrow();
		}

		private void checkWebSchedRecall_CheckedChanged(object sender,EventArgs e) {
			if(((CheckBox)sender).Checked) {
				radioDateAptCreated.Checked=true;
			}
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(!IsValid()) {
				return;
			}
			List<long> listClinicNums=new List<long>();
			for(int i=0;i<listClinics.SelectedIndices.Count;i++) {
				if(Security.CurUser.ClinicIsRestricted) {
						listClinicNums.Add(_listClinics[listClinics.SelectedIndices[i]].ClinicNum);//we know that the list is a 1:1 to _listClinics
					}
				else {
					if(listClinics.SelectedIndices[i]==0) {
						listClinicNums.Add(0);
					}
					else {
						listClinicNums.Add(_listClinics[listClinics.SelectedIndices[i]-1].ClinicNum);//Minus 1 from the selected index
					}
				}
			}
			List<long> listProvNums=new List<long>();
			if(checkAllProvs.Checked) {
				for(int i = 0;i<_listProviders.Count;i++) {
					listProvNums.Add(_listProviders[i].ProvNum);
				}
			}
			else {
				for(int i=0;i<listProvs.SelectedIndices.Count;i++) {
					listProvNums.Add(_listProviders[listProvs.SelectedIndices[i]].ProvNum);
				}
			}
			ReportComplex report=new ReportComplex(true,true);
			DateTime dateFrom=PIn.Date(textDateFrom.Text);
			DateTime dateTo=PIn.Date(textDateTo.Text);
			DataTable table = new DataTable();
			List<ApptStatus> listStatuses=new List<ApptStatus> { ApptStatus.Planned,ApptStatus.UnschedList };
			if(!checkShowNoteAppts.Checked) {
				listStatuses.Add(ApptStatus.PtNote);
				listStatuses.Add(ApptStatus.PtNoteCompleted);
			}
			RpAppointments.SortAndFilterBy sortBy=radioDateAptCreated.Checked ? RpAppointments.SortAndFilterBy.SecDateEntry : RpAppointments.SortAndFilterBy.AptDateTime;
			table=RpAppointments.GetAppointmentTable(dateFrom,dateTo,listProvNums,listClinicNums,_hasClinicsEnabled,checkWebSchedRecall.Checked,checkWebSchedNewPat.Checked,false,
				sortBy,listStatuses,new List<long>(),nameof(FormRpAppointments));
			//create the report
			Font font=new Font("Tahoma",9);
			Font fontTitle=new Font("Tahoma",17,FontStyle.Bold);
			Font fontSubTitle=new Font("Tahoma",10,FontStyle.Bold);
			report.ReportName=Lan.g(this,"Appointments");
			report.AddTitle("Title",Lan.g(this,"Appointments"),fontTitle);
			report.AddSubTitle("PracName",PrefC.GetString(PrefName.PracticeTitle),fontSubTitle);
			report.AddSubTitle("Date",dateFrom.ToShortDateString()+" - "+dateTo.ToShortDateString(),fontSubTitle);
			if(checkAllProvs.Checked) {
				report.AddSubTitle("Providers",Lan.g(this,"All Providers"));
			}
			else {
				string str="";
				for(int i=0;i<listProvs.SelectedIndices.Count;i++) {
					if(i>0) {
						str+=", ";
					}
					str+=_listProviders[listProvs.SelectedIndices[i]].Abbr;
				}
				report.AddSubTitle("Providers",str);
			}
			QueryObject query;
			//setup query
			if(!_hasClinicsEnabled) {
				query=report.AddQuery(table,"","",SplitByKind.None,1,true);
			}
			else {
				query=report.AddQuery(table,"","ClinicDesc",SplitByKind.Value,1,true);
			}
			// add columns to report
			if(radioAptDate.Checked) {
				query.AddColumn("Date",80,FieldValueType.Date,font);
				query.GetColumnDetail("Date").SuppressIfDuplicate = true;
				query.GetColumnDetail("Date").StringFormat="d";
			}
			else {
				query.AddColumn("DateCreated",80,FieldValueType.Date,font);
				query.GetColumnDetail("DateCreated").SuppressIfDuplicate = true;
				query.GetColumnDetail("DateCreated").StringFormat="d";
				query.AddColumn("AptDate",80,FieldValueType.Date,font);
				query.GetColumnDetail("AptDate").StringFormat="d";
			}
			query.AddColumn("PatNum",55,FieldValueType.String,font);
			query.AddColumn("Patient",150,FieldValueType.String,font);
			query.AddColumn("Age",45,FieldValueType.Age,font);
			query.AddColumn("Time",65,FieldValueType.Date,font);
			query.GetColumnDetail("Time").StringFormat="t";
			query.GetColumnDetail("Time").ContentAlignment = ContentAlignment.MiddleRight;
			query.GetColumnHeader("Time").ContentAlignment = ContentAlignment.MiddleRight;
			query.AddColumn("Length",45,FieldValueType.Integer,font);
			query.GetColumnHeader("Length").Location=new Point(
				query.GetColumnHeader("Length").Location.X,
				query.GetColumnHeader("Length").Location.Y);
			query.GetColumnHeader("Length").ContentAlignment = ContentAlignment.MiddleCenter;
			query.GetColumnDetail("Length").ContentAlignment = ContentAlignment.MiddleCenter;
			query.GetColumnDetail("Length").Location=new Point(
				query.GetColumnDetail("Length").Location.X,
				query.GetColumnDetail("Length").Location.Y);
			query.AddColumn("Description",170,FieldValueType.String,font);
			query.AddColumn("Home Ph.",120,FieldValueType.String,font);
			query.AddColumn("Work Ph.",120,FieldValueType.String,font);
			query.AddColumn("Cell Ph.",120,FieldValueType.String,font);
			report.AddPageNum(font);
			report.AddGridLines();
			// execute query
			if(!report.SubmitQueries()) {
				return;
			}
			// display report
			FormReportComplex FormR=new FormReportComplex(report);
			//FormR.MyReport=report;
			FormR.ShowDialog();
			DialogResult=DialogResult.OK;
		}
	}
}
