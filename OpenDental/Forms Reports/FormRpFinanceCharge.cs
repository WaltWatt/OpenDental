using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenDental.ReportingComplex;
using OpenDentBusiness;

namespace OpenDental {
	///<summary></summary>
	public class FormRpFinanceCharge : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.ComponentModel.Container components = null;
		private GroupBox groupBox2;
		private Label label2;
		private ValidDate textDateFrom;
		private ValidDate textDateTo;
		private Label label3;
		private ListBox listProv;
		private ListBox listBillingType;
		private CheckBox checkAllProv;
		private CheckBox checkAllBilling;
		private Label label1;
		private Label label4;
		private FormQuery FormQuery2;
		private List<Provider> _listProviders=new List<Provider>();
		private List<Def> _listBillingTypeDefs=new List<Def>();

		///<summary></summary>
		public FormRpFinanceCharge(){
			InitializeComponent();
			Lan.F(this);
		}


		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRpFinanceCharge));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textDateFrom = new OpenDental.ValidDate();
			this.textDateTo = new OpenDental.ValidDate();
			this.label3 = new System.Windows.Forms.Label();
			this.listProv = new System.Windows.Forms.ListBox();
			this.listBillingType = new System.Windows.Forms.ListBox();
			this.checkAllProv = new System.Windows.Forms.CheckBox();
			this.checkAllBilling = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox2.SuspendLayout();
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
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(361, 203);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 4;
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
			this.butOK.Location = new System.Drawing.Point(280, 203);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.textDateFrom);
			this.groupBox2.Controls.Add(this.textDateTo);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(263, 50);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(173, 86);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Date Range";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(7, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 18);
			this.label2.TabIndex = 37;
			this.label2.Text = "From";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textDateFrom
			// 
			this.textDateFrom.Location = new System.Drawing.Point(59, 19);
			this.textDateFrom.Name = "textDateFrom";
			this.textDateFrom.Size = new System.Drawing.Size(91, 20);
			this.textDateFrom.TabIndex = 1;
			// 
			// textDateTo
			// 
			this.textDateTo.Location = new System.Drawing.Point(60, 52);
			this.textDateTo.Name = "textDateTo";
			this.textDateTo.Size = new System.Drawing.Size(91, 20);
			this.textDateTo.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(7, 55);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 18);
			this.label3.TabIndex = 39;
			this.label3.Text = "To";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// listProv
			// 
			this.listProv.Location = new System.Drawing.Point(10, 50);
			this.listProv.Name = "listProv";
			this.listProv.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listProv.Size = new System.Drawing.Size(120, 147);
			this.listProv.TabIndex = 37;
			// 
			// listBillingType
			// 
			this.listBillingType.Location = new System.Drawing.Point(137, 50);
			this.listBillingType.Name = "listBillingType";
			this.listBillingType.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listBillingType.Size = new System.Drawing.Size(120, 147);
			this.listBillingType.TabIndex = 38;
			// 
			// checkAllProv
			// 
			this.checkAllProv.Checked = true;
			this.checkAllProv.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkAllProv.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllProv.Location = new System.Drawing.Point(10, 28);
			this.checkAllProv.Name = "checkAllProv";
			this.checkAllProv.Size = new System.Drawing.Size(95, 16);
			this.checkAllProv.TabIndex = 49;
			this.checkAllProv.Text = "All";
			this.checkAllProv.Click += new System.EventHandler(this.checkAllProv_Click);
			// 
			// checkAllBilling
			// 
			this.checkAllBilling.Checked = true;
			this.checkAllBilling.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkAllBilling.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkAllBilling.Location = new System.Drawing.Point(137, 28);
			this.checkAllBilling.Name = "checkAllBilling";
			this.checkAllBilling.Size = new System.Drawing.Size(95, 16);
			this.checkAllBilling.TabIndex = 50;
			this.checkAllBilling.Text = "All";
			this.checkAllBilling.Click += new System.EventHandler(this.checkAllBilling_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(7, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 16);
			this.label1.TabIndex = 51;
			this.label1.Text = "Providers";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(134, 9);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(104, 16);
			this.label4.TabIndex = 52;
			this.label4.Text = "Billing Types";
			// 
			// FormRpFinanceCharge
			// 
			this.AcceptButton = this.butOK;
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(448, 241);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkAllBilling);
			this.Controls.Add(this.checkAllProv);
			this.Controls.Add(this.listBillingType);
			this.Controls.Add(this.listProv);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(447, 280);
			this.Name = "FormRpFinanceCharge";
			this.ShowInTaskbar = false;
			this.Text = "Finance Charge Report";
			this.Load += new System.EventHandler(this.FormRpFinanceCharge_Load);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormRpFinanceCharge_Load(object sender, System.EventArgs e) {
			textDateFrom.Text=PrefC.GetDate(PrefName.FinanceChargeLastRun).ToShortDateString();
			textDateTo.Text=PrefC.GetDate(PrefName.FinanceChargeLastRun).ToShortDateString();
			_listProviders=Providers.GetListReports();
			_listBillingTypeDefs=Defs.GetDefsForCategory(DefCat.BillingTypes,true);
			foreach(Def billingType in _listBillingTypeDefs) {
				listBillingType.Items.Add(billingType.ItemName);
			}
			if(listBillingType.Items.Count>0) {
				listBillingType.SelectedIndex=0;
			}
			foreach(Provider prov in _listProviders) {
				listProv.Items.Add(prov.GetLongDesc());
			}
			if(listProv.Items.Count>0) {
				listProv.SelectedIndex=0; 
			}
			checkAllProv.Checked=true;
			checkAllBilling.Checked=true;
			listProv.Visible=false;
			listBillingType.Visible=false;
		}

		private void checkAllProv_Click(object sender,EventArgs e) {
			listProv.Visible=!checkAllProv.Checked;
		}

		private void checkAllBilling_Click(object sender,EventArgs e) {
			listBillingType.Visible=!checkAllBilling.Checked;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textDateFrom.errorProvider1.GetError(textDateFrom)!=""
				|| textDateTo.errorProvider1.GetError(textDateTo)!="") 
			{
				MsgBox.Show(this,"Please fix data entry errors first.");
				return;
			}
			DateTime dateFrom=PIn.Date(textDateFrom.Text);
			DateTime dateTo=PIn.Date(textDateTo.Text);
			if(dateTo<dateFrom) {
				MsgBox.Show(this,"To date cannot be before From date.");
				return;
			}
			ReportComplex report=new ReportComplex(true,false);
			List<long> listProvNums = new List<long>();
			if(!checkAllProv.Checked) {
				listProvNums.AddRange(listProv.SelectedIndices.OfType<int>().Select(x => _listProviders[x].ProvNum).ToList());
			}
			List<long> listBillingDefNums= new List<long>();
			if(!checkAllBilling.Checked) {
				listBillingDefNums.AddRange(listBillingType.SelectedIndices.OfType<int>().Select(x => _listBillingTypeDefs[x].DefNum).ToList());
			}
			DataTable table=RpFinanceCharge.GetFinanceChargeTable(dateFrom,dateTo,PrefC.GetLong(PrefName.FinanceChargeAdjustmentType),listProvNums,listBillingDefNums);
			Font font=new Font("Tahoma",9);
			Font fontTitle=new Font("Tahoma",17,FontStyle.Bold);
			Font fontSubTitle=new Font("Tahoma",10,FontStyle.Bold);
			report.ReportName=Lan.g(this,"Finance Charge Report");
			report.AddTitle("Title",Lan.g(this,"Finance Charge Report"),fontTitle);
			report.AddSubTitle("PracticeTitle",PrefC.GetString(PrefName.PracticeTitle),fontSubTitle);
			report.AddSubTitle("Date SubTitle",dateFrom.ToString("d")+" - "+dateTo.ToString("d"),fontSubTitle);
			string subtitleProvs="";
			if(listProvNums.Count>0) {
				subtitleProvs+=string.Join(", ",listProv.SelectedIndices.OfType<int>().ToList().Select(x =>_listProviders[x].Abbr));
			}
			else {
				subtitleProvs=Lan.g(this,"All Providers");
			}
			report.AddSubTitle("Provider Subtitle",subtitleProvs);
			string subtBillingTypes="";
			if(listBillingDefNums.Count>0) {
				subtBillingTypes+=string.Join(", ",listBillingType.SelectedIndices.OfType<int>().Select(x => _listBillingTypeDefs[x].ItemName));
			}
			else {
				subtBillingTypes=Lan.g(this,"All Billing Types");
			}
			report.AddSubTitle("Billing Subtitle",subtBillingTypes);
			QueryObject query=report.AddQuery(table,Lan.g(this,"Date")+": "+DateTimeOD.Today.ToString("d"));
			query.AddColumn("PatNum",75);
			query.AddColumn("Patient Name",180);
			query.AddColumn("Preferred Name",130);
			query.AddColumn("Amount",100,FieldValueType.Number);
			query.GetColumnDetail("Amount").ContentAlignment=ContentAlignment.MiddleRight;
			report.AddPageNum(font);
			if(!report.SubmitQueries()) {
				return;
			}
			FormReportComplex FormR=new FormReportComplex(report);
			FormR.ShowDialog();		
			DialogResult=DialogResult.OK;
		}

	}
}
