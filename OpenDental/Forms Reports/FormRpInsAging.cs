using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenDental.ReportingComplex;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental{
	///<summary></summary>
	public class FormRpInsAging : ODForm {
		#region Designer Variables
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private OpenDental.ValidDate textDate;
		private System.Windows.Forms.ListBox listBillType;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.RadioButton radio30;
		private System.Windows.Forms.RadioButton radio90;
		private System.Windows.Forms.RadioButton radio60;
		private ListBox listProv;
		private Label label3;
		private CheckBox checkProvAll;
		private CheckBox checkBillTypesAll;
		private CheckBox checkAllClin;
		private ListBox listClin;
		private Label labelClin;
		private System.Windows.Forms.RadioButton radioAny;
		private GroupBox groupGroupBy;
		private RadioButton radioGroupByPat;
		private RadioButton radioGroupByFam;
		#endregion
		private List<Clinic> _listClinics;
		private List<Provider> _listProviders;
		private List<Def> _listBillingTypeDefs;

		///<summary></summary>
		public FormRpInsAging(){
			InitializeComponent();
			Lan.F(this);
		}

		///<summary></summary>
		protected override void Dispose(bool disposing){
			if(disposing){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpInsAging));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radio30 = new System.Windows.Forms.RadioButton();
			this.radio90 = new System.Windows.Forms.RadioButton();
			this.radio60 = new System.Windows.Forms.RadioButton();
			this.radioAny = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.textDate = new OpenDental.ValidDate();
			this.listBillType = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.listProv = new System.Windows.Forms.ListBox();
			this.label3 = new System.Windows.Forms.Label();
			this.checkProvAll = new System.Windows.Forms.CheckBox();
			this.checkBillTypesAll = new System.Windows.Forms.CheckBox();
			this.checkAllClin = new System.Windows.Forms.CheckBox();
			this.listClin = new System.Windows.Forms.ListBox();
			this.labelClin = new System.Windows.Forms.Label();
			this.groupGroupBy = new System.Windows.Forms.GroupBox();
			this.radioGroupByPat = new System.Windows.Forms.RadioButton();
			this.radioGroupByFam = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.groupGroupBy.SuspendLayout();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(669, 252);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 17;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(583, 252);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 16;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radio30);
			this.groupBox1.Controls.Add(this.radio90);
			this.groupBox1.Controls.Add(this.radio60);
			this.groupBox1.Controls.Add(this.radioAny);
			this.groupBox1.Location = new System.Drawing.Point(24, 122);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(196, 114);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Age of Account";
			// 
			// radio30
			// 
			this.radio30.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radio30.Location = new System.Drawing.Point(12, 42);
			this.radio30.Name = "radio30";
			this.radio30.Size = new System.Drawing.Size(174, 18);
			this.radio30.TabIndex = 1;
			this.radio30.Text = "Over 30 Days";
			// 
			// radio90
			// 
			this.radio90.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radio90.Location = new System.Drawing.Point(12, 86);
			this.radio90.Name = "radio90";
			this.radio90.Size = new System.Drawing.Size(174, 18);
			this.radio90.TabIndex = 3;
			this.radio90.Text = "Over 90 Days";
			// 
			// radio60
			// 
			this.radio60.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radio60.Location = new System.Drawing.Point(12, 64);
			this.radio60.Name = "radio60";
			this.radio60.Size = new System.Drawing.Size(174, 18);
			this.radio60.TabIndex = 2;
			this.radio60.Text = "Over 60 Days";
			// 
			// radioAny
			// 
			this.radioAny.Checked = true;
			this.radioAny.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioAny.Location = new System.Drawing.Point(12, 20);
			this.radioAny.Name = "radioAny";
			this.radioAny.Size = new System.Drawing.Size(174, 18);
			this.radioAny.TabIndex = 0;
			this.radioAny.TabStop = true;
			this.radioAny.Text = "Any Balance";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(131, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "As Of Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textDate
			// 
			this.textDate.Location = new System.Drawing.Point(24, 20);
			this.textDate.Name = "textDate";
			this.textDate.Size = new System.Drawing.Size(106, 20);
			this.textDate.TabIndex = 1;
			// 
			// listBillType
			// 
			this.listBillType.Location = new System.Drawing.Point(244, 50);
			this.listBillType.Name = "listBillType";
			this.listBillType.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBillType.Size = new System.Drawing.Size(163, 186);
			this.listBillType.TabIndex = 11;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(242, 5);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(163, 17);
			this.label2.TabIndex = 0;
			this.label2.Text = "Billing Types";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listProv
			// 
			this.listProv.Location = new System.Drawing.Point(413, 50);
			this.listProv.Name = "listProv";
			this.listProv.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listProv.Size = new System.Drawing.Size(163, 186);
			this.listProv.TabIndex = 13;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(411, 5);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(163, 17);
			this.label3.TabIndex = 0;
			this.label3.Text = "Providers";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// checkProvAll
			// 
			this.checkProvAll.Checked = true;
			this.checkProvAll.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkProvAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkProvAll.Location = new System.Drawing.Point(413, 27);
			this.checkProvAll.Name = "checkProvAll";
			this.checkProvAll.Size = new System.Drawing.Size(163, 18);
			this.checkProvAll.TabIndex = 12;
			this.checkProvAll.Text = "All";
			this.checkProvAll.Click += new System.EventHandler(this.checkProvAll_Click);
			// 
			// checkBillTypesAll
			// 
			this.checkBillTypesAll.Checked = true;
			this.checkBillTypesAll.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBillTypesAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBillTypesAll.Location = new System.Drawing.Point(244, 27);
			this.checkBillTypesAll.Name = "checkBillTypesAll";
			this.checkBillTypesAll.Size = new System.Drawing.Size(163, 18);
			this.checkBillTypesAll.TabIndex = 10;
			this.checkBillTypesAll.Text = "All";
			this.checkBillTypesAll.Click += new System.EventHandler(this.checkBillTypesAll_Click);
			// 
			// checkAllClin
			// 
			this.checkAllClin.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllClin.Location = new System.Drawing.Point(582, 27);
			this.checkAllClin.Name = "checkAllClin";
			this.checkAllClin.Size = new System.Drawing.Size(163, 18);
			this.checkAllClin.TabIndex = 14;
			this.checkAllClin.Text = "All";
			this.checkAllClin.Click += new System.EventHandler(this.checkAllClin_Click);
			// 
			// listClin
			// 
			this.listClin.Location = new System.Drawing.Point(582, 50);
			this.listClin.Name = "listClin";
			this.listClin.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listClin.Size = new System.Drawing.Size(163, 186);
			this.listClin.TabIndex = 15;
			this.listClin.Click += new System.EventHandler(this.listClin_Click);
			// 
			// labelClin
			// 
			this.labelClin.Location = new System.Drawing.Point(580, 5);
			this.labelClin.Name = "labelClin";
			this.labelClin.Size = new System.Drawing.Size(163, 17);
			this.labelClin.TabIndex = 0;
			this.labelClin.Text = "Clinics";
			this.labelClin.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// groupGroupBy
			// 
			this.groupGroupBy.Controls.Add(this.radioGroupByPat);
			this.groupGroupBy.Controls.Add(this.radioGroupByFam);
			this.groupGroupBy.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupGroupBy.Location = new System.Drawing.Point(24, 46);
			this.groupGroupBy.Name = "groupGroupBy";
			this.groupGroupBy.Size = new System.Drawing.Size(196, 70);
			this.groupGroupBy.TabIndex = 2;
			this.groupGroupBy.TabStop = false;
			this.groupGroupBy.Text = "Group By";
			// 
			// radioGroupByPat
			// 
			this.radioGroupByPat.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioGroupByPat.Location = new System.Drawing.Point(12, 42);
			this.radioGroupByPat.Name = "radioGroupByPat";
			this.radioGroupByPat.Size = new System.Drawing.Size(174, 18);
			this.radioGroupByPat.TabIndex = 1;
			this.radioGroupByPat.Text = "Individual";
			this.radioGroupByPat.UseVisualStyleBackColor = true;
			// 
			// radioGroupByFam
			// 
			this.radioGroupByFam.Checked = true;
			this.radioGroupByFam.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioGroupByFam.Location = new System.Drawing.Point(12, 20);
			this.radioGroupByFam.Name = "radioGroupByFam";
			this.radioGroupByFam.Size = new System.Drawing.Size(174, 18);
			this.radioGroupByFam.TabIndex = 0;
			this.radioGroupByFam.TabStop = true;
			this.radioGroupByFam.Text = "Family";
			this.radioGroupByFam.UseVisualStyleBackColor = true;
			// 
			// FormRpInsAging
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(778, 290);
			this.Controls.Add(this.groupGroupBy);
			this.Controls.Add(this.checkAllClin);
			this.Controls.Add(this.listClin);
			this.Controls.Add(this.labelClin);
			this.Controls.Add(this.checkBillTypesAll);
			this.Controls.Add(this.checkProvAll);
			this.Controls.Add(this.listProv);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textDate);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.listBillType);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormRpInsAging";
			this.ShowInTaskbar = false;
			this.Text = "Insurance Aging Report";
			this.Load += new System.EventHandler(this.FormRpInsAging_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupGroupBy.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormRpInsAging_Load(object sender, System.EventArgs e) {
			_listProviders=Providers.GetListReports();
			DateTime lastAgingDate=PrefC.GetDate(PrefName.DateLastAging);
			if(lastAgingDate.Year<1880) {
				textDate.Text="";
			}
			else if(PrefC.GetBool(PrefName.AgingCalculatedMonthlyInsteadOfDaily)){
				textDate.Text=lastAgingDate.ToShortDateString();
			}
			else{
				textDate.Text=DateTime.Today.ToShortDateString();
			}
			_listBillingTypeDefs=Defs.GetDefsForCategory(DefCat.BillingTypes,true);
			for(int i=0;i<_listBillingTypeDefs.Count;i++){
				listBillType.Items.Add(_listBillingTypeDefs[i].ItemName);
			}
			if(listBillType.Items.Count>0){
				listBillType.SelectedIndex=0;
			}
			listBillType.Visible=false;
			checkBillTypesAll.Checked=true; //all billing types by default
			for(int i=0;i<_listProviders.Count;i++){
				listProv.Items.Add(_listProviders[i].GetLongDesc());
			}
			if(listProv.Items.Count>0) {
				listProv.SelectedIndex=0;
			}
			checkProvAll.Checked=true; //all provs by default
			listProv.Visible=false; 
			if(!PrefC.HasClinicsEnabled) {
				listClin.Visible=false;
				labelClin.Visible=false;
				checkAllClin.Visible=false;
			}
			else {
				_listClinics=new List<Clinic>();
				if(!Security.CurUser.ClinicIsRestricted) {
					listClin.Items.Add(new ODBoxItem<Clinic>(Lan.g(this,"Unassigned"),new Clinic() {ClinicNum = 0, Description = "Unassigned", Abbr = "Unassigned"}));
				}
				_listClinics=Clinics.GetForUserod(Security.CurUser);
				foreach(Clinic clinic in _listClinics) {
					ODBoxItem<Clinic> boxItemCur=new ODBoxItem<Clinic>(clinic.Abbr,clinic);
					listClin.Items.Add(boxItemCur);
					if(clinic.ClinicNum==Clinics.ClinicNum) {
						listClin.SelectedItem=boxItemCur;
					}
				}
				if(Clinics.ClinicNum==0) {
					checkAllClin.Checked=true;//event handler will select all indices
					listClin.Visible=false;
				}
			}
		}

		private void checkBillTypesAll_Click(object sender,EventArgs e) {
			if(checkBillTypesAll.Checked){
				listBillType.Visible=false;
			}
			else{
				listBillType.Visible=true;
			}
		}

		private void checkProvAll_Click(object sender,EventArgs e) {
			if(checkProvAll.Checked) {
				listProv.Visible=false;
			}
			else {
				listProv.Visible=true;
			}
		}

		private void checkAllClin_Click(object sender,EventArgs e) {
			if(checkAllClin.Checked) {
				listClin.Visible=false;
			}
			else {
				listClin.Visible=true;
			}
		}

		private void listClin_Click(object sender,EventArgs e) {
			checkAllClin.Checked=false;//will not clear all selected indices
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(!checkBillTypesAll.Checked && listBillType.SelectedIndices.Count==0){
				MsgBox.Show(this,"At least one billing type must be selected.");
				return;
			}
			if(!checkProvAll.Checked && listProv.SelectedIndices.Count==0) {
				MsgBox.Show(this,"At least one provider must be selected.");
				return;
			}
			if(PrefC.HasClinicsEnabled && !checkAllClin.Checked && listClin.SelectedIndices.Count==0) {
				MsgBox.Show(this,"At least one clinic must be selected.");
				return;
			}
			if(textDate.errorProvider1.GetError(textDate)!="") {
				MsgBox.Show(this,"Invalid date.");
				return;
			}
			DateTime asOfDate=PIn.Date(textDate.Text);
			List<long> listProvNums=new List<long>();
			List<long> listClinicNums=new List<long>();
			List<long> listBillingTypeDefNums=new List<long>();
			if(!checkProvAll.Checked) {
				listProvNums=listProv.SelectedIndices.OfType<int>().Select(x => _listProviders[x].ProvNum).ToList();
			}
			if(PrefC.HasClinicsEnabled) {
				if(checkAllClin.Checked) {
					listClinicNums=listClin.Items.OfType<ODBoxItem<Clinic>>().Select(x => x.Tag.ClinicNum).ToList();
				}
				else {
					listClinicNums=listClin.SelectedItems.OfType<ODBoxItem<Clinic>>().Select(x => x.Tag.ClinicNum).ToList();
				}
			}
			if(!checkBillTypesAll.Checked) {
				for(int i=0;i<listBillType.SelectedIndices.Count;i++) {
					listBillingTypeDefNums.Add(_listBillingTypeDefs[listBillType.SelectedIndices[i]].DefNum);
				}
			}
			AgeOfAccount accountAge=AgeOfAccount.Any;
			if(radioAny.Checked) {
				accountAge=AgeOfAccount.Any;
			}
			else if(radio30.Checked) {
				accountAge=AgeOfAccount.Over30;
			}
			else if(radio60.Checked) {
				accountAge=AgeOfAccount.Over60;
			}
			else if(radio90.Checked) {
				accountAge=AgeOfAccount.Over90;
			}
			ReportComplex report=new ReportComplex(true,true); 
			DataTable tableAging=new DataTable();
			tableAging=RpInsAging.GetInsAgingTable 
				(asOfDate,radioGroupByFam.Checked,accountAge,checkBillTypesAll.Checked,checkProvAll.Checked,listProvNums,listClinicNums,listBillingTypeDefNums);
			report.ReportName=Lan.g(this,"Insurance Aging Report");
			report.AddTitle("InsAging",Lan.g(this, "Insurance Aging Report"));
			report.AddSubTitle("PracTitle",PrefC.GetString(PrefName.PracticeTitle));
			report.AddSubTitle("AsOf",Lan.g(this,"As of ")+textDate.Text);
			if(radioAny.Checked){
				report.AddSubTitle("Balance",Lan.g(this,"Any Balance"));
			}
			else if(radio30.Checked){
				report.AddSubTitle("Over30",Lan.g(this,"Over 30 Days"));
			}
			else if(radio60.Checked){
				report.AddSubTitle("Over60",Lan.g(this,"Over 60 Days"));
			}
			else if(radio90.Checked){
				report.AddSubTitle("Over90",Lan.g(this,"Over 90 Days"));
			}
			if(checkBillTypesAll.Checked){
				report.AddSubTitle("AllBillingTypes",Lan.g(this,"All Billing Types"));
			}
			else{
				string subt=_listBillingTypeDefs[listBillType.SelectedIndices[0]].ItemName;
				for(int i=1;i<listBillType.SelectedIndices.Count;i++){ //there must be at least one selected
					subt+=", "+_listBillingTypeDefs[listBillType.SelectedIndices[i]].ItemName;
				}
				report.AddSubTitle("",subt);
			}
			string subtitleProvs="";
			if(checkProvAll.Checked) {
				subtitleProvs=Lan.g(this,"All Providers");
			}
			else {
				subtitleProvs+=string.Join(", ",listProv.SelectedIndices.OfType<int>().ToList().Select(x => _listProviders[x].Abbr));
			}
			report.AddSubTitle("Providers",subtitleProvs);
			if(checkAllClin.Checked) {
				report.AddSubTitle("Clinics",Lan.g(this,"All Clinics"));
			}
			else {
				string subt = "";
				for(int i = 0;i<listClin.SelectedIndices.Count;i++) {
					if(i!=0) {
						subt+=", ";
					}
					if(Security.CurUser.ClinicIsRestricted) {
						subt+=_listClinics[listClin.SelectedIndices[i]].Abbr;
					}
					else {
						if(listClin.SelectedIndices[i]==0) {
							subt+="Unassigned";
							continue;
						}
						subt+=_listClinics[listClin.SelectedIndices[i]-1].Abbr;
					}
				}
				report.AddSubTitle("Clinics",subt);
			}
			//Patient Account Aging Query-----------------------------------------------
			QueryObject query=report.AddQuery(tableAging,"Date "+DateTime.Today.ToShortDateString());
			query.AddColumn((radioGroupByFam.Checked?"GUARANTOR":"PATIENT"),160,FieldValueType.String);
			query.AddColumn("INS PAY EST 0-30",75,FieldValueType.Number);
			query.AddColumn("INS PAY EST 31-60",75,FieldValueType.Number);
			query.AddColumn("INS PAY EST 61-90",75,FieldValueType.Number);
			query.AddColumn("INS PAY EST >90",75,FieldValueType.Number);
			query.AddColumn("INS EST TOTAL", 80, FieldValueType.Number);
			query.AddColumn("PAT EST  BAL 0-30",75,FieldValueType.Number);
			query.AddColumn("PAT EST BAL 31-60",75,FieldValueType.Number);
			query.AddColumn("PAT EST BAL 61-90",75,FieldValueType.Number);
			query.AddColumn("PAT EST BAL >90",75,FieldValueType.Number);
			query.AddColumn("PAT TOTAL",80,FieldValueType.Number);
			report.AddPageNum();
			if(!report.SubmitQueries()) {
				return;
			}
			FormReportComplex FormR=new FormReportComplex(report);
			FormR.ShowDialog();
			DialogResult=DialogResult.OK;		
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult = DialogResult.Cancel;
		}

	}

}
