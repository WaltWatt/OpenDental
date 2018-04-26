using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using OpenDental.UI;

namespace OpenDental{
	/// <summary>Lets the user choose which payment plan to attach a payment to if there are more than one available.</summary>
	public class FormPayPlanSelect : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private UI.Button butNone;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		/// <summary>A list of plans passed to this form which are to be displayed.</summary>
		private List<PayPlan> _listValidPayPlans;
		private List<PayPlanCharge> _listPayPlanCharges;
		private UI.ODGrid gridMain;
		///<summary>Have the option to not select a payment plan.</summary>
		private bool _includeNone;
		private Label labelExpl;

		/// <summary>The pk of the plan selected.</summary>
		public long SelectedPayPlanNum;

		///<summary>Optionally pass in the ability to not select a payment plan (include a None button)</summary>
		public FormPayPlanSelect(List<PayPlan> validPlans,bool includeNone=false)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			_includeNone=includeNone;
			_listValidPayPlans=validPlans;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPayPlanSelect));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butNone = new OpenDental.UI.Button();
			this.labelExpl = new System.Windows.Forms.Label();
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
			this.butCancel.Location = new System.Drawing.Point(422, 208);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 26);
			this.butCancel.TabIndex = 1;
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
			this.butOK.Location = new System.Drawing.Point(341, 208);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 26);
			this.butOK.TabIndex = 0;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
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
			this.gridMain.Location = new System.Drawing.Point(12, 22);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(490, 180);
			this.gridMain.TabIndex = 3;
			this.gridMain.Title = "Payment Plans";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TablePaymentPlans";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridMain_KeyDown);
			// 
			// butNone
			// 
			this.butNone.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butNone.Autosize = true;
			this.butNone.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNone.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNone.CornerRadius = 4F;
			this.butNone.Location = new System.Drawing.Point(12, 208);
			this.butNone.Name = "butNone";
			this.butNone.Size = new System.Drawing.Size(75, 26);
			this.butNone.TabIndex = 4;
			this.butNone.Text = "None";
			this.butNone.Visible = false;
			this.butNone.Click += new System.EventHandler(this.butNone_Click);
			// 
			// labelExpl
			// 
			this.labelExpl.Location = new System.Drawing.Point(12, 4);
			this.labelExpl.Name = "labelExpl";
			this.labelExpl.Size = new System.Drawing.Size(410, 16);
			this.labelExpl.TabIndex = 5;
			this.labelExpl.Text = "Select a Payment Plan to attach to, or click \'None\'.";
			this.labelExpl.Visible = false;
			// 
			// FormPayPlanSelect
			// 
			this.CancelButton = this.butCancel;
			this.ClientSize = new System.Drawing.Size(509, 246);
			this.Controls.Add(this.labelExpl);
			this.Controls.Add(this.butNone);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(525, 585);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(525, 285);
			this.Name = "FormPayPlanSelect";
			this.ShowInTaskbar = false;
			this.Text = "Select Payment Plan";
			this.Load += new System.EventHandler(this.FormPayPlanSelect_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormPayPlanSelect_Load(object sender, System.EventArgs e) {
			if(_includeNone) {
				this.Text=Lan.g(this,"Attach to payment plan?");
				labelExpl.Visible=true;
				butNone.Visible=true;
			}
			_listPayPlanCharges=PayPlanCharges.GetForPayPlans(_listValidPayPlans.Select(x => x.PayPlanNum).ToList());
			FillGrid();
			gridMain.SetSelected(0,true); 
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			ODGridColumn col = new ODGridColumn("Date",70);
			gridMain.Columns.Add(col);
			col = new ODGridColumn("Patient",100);
			gridMain.Columns.Add(col);
			col = new ODGridColumn("Category",80);
			gridMain.Columns.Add(col);
			col = new ODGridColumn("Total Cost",80);
			gridMain.Columns.Add(col);
			col = new ODGridColumn("Balance",80);
			gridMain.Columns.Add(col);
			col = new ODGridColumn("Due Now",80);
			gridMain.Columns.Add(col);
			List<long> listPayPlanNums=_listValidPayPlans.Select(x => x.PayPlanNum).ToList();
			List<PaySplit> listPaySplits=PaySplits.GetForPayPlans(listPayPlanNums);
			List<Patient> listPats=Patients.GetLimForPats(_listValidPayPlans.Select(x=>x.PatNum).ToList());
			for(int i = 0;i<_listValidPayPlans.Count;i++) {
				//no db calls are made in this loop because we have all the necessary information already.
				PayPlan planCur=_listValidPayPlans[i];
				Patient patCur=listPats.Where(x => x.PatNum == planCur.PatNum).FirstOrDefault();
				ODGridRow row=new ODGridRow();
				row.Cells.Add(planCur.PayPlanDate.ToShortDateString());//date
				row.Cells.Add(patCur.LName+", "+patCur.FName);//patient			
				if(planCur.PlanCategory==0) {
					row.Cells.Add(Lan.g(this,"None"));
				}
				else {
					row.Cells.Add(Defs.GetDef(DefCat.PayPlanCategories,planCur.PlanCategory).ItemName);
				}
				row.Cells.Add(PayPlans.GetTotalCost(planCur.PayPlanNum,_listPayPlanCharges).ToString("F"));//total cost
				row.Cells.Add(PayPlans.GetBalance(planCur.PayPlanNum,_listPayPlanCharges,listPaySplits).ToString("F"));//balance
				row.Cells.Add(PayPlans.GetDueNow(planCur.PayPlanNum,_listPayPlanCharges,listPaySplits).ToString("F"));//due now
				row.Tag=planCur.PayPlanNum;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				return;
			}
			SelectedPayPlanNum=(long)gridMain.Rows[gridMain.GetSelectedIndex()].Tag;
			DialogResult=DialogResult.OK;
		}

		private void butNone_Click(object sender,EventArgs e) {
			SelectedPayPlanNum=0;
			DialogResult=DialogResult.OK;
		}

		private void gridMain_KeyDown(object sender,KeyEventArgs e) {
			if(gridMain.GetSelectedIndex()==-1 || (e.KeyCode!=Keys.Enter)) {
				return;
			}
			SelectedPayPlanNum=(long)gridMain.Rows[gridMain.GetSelectedIndex()].Tag;
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				MessageBox.Show(Lan.g(this,"Please select a payment plan first."));
				return;
			}
			SelectedPayPlanNum=(long)gridMain.Rows[gridMain.GetSelectedIndex()].Tag;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}




































