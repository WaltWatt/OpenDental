using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;

namespace OpenDental{
	/// <summary>For a given subscriber, this list all their plans.  User then selects one plan from the list or creates a blank plan.</summary>
	public class FormInsSelectSubscr : ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.ListBox listPlans;
		private OpenDental.UI.Button butNew;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private long Subscriber;
		private List <InsSub> SubList;
		private long _patNum;
		///<summary>When dialogResult=OK, this will contain the InsSubNum of the selected plan.  If this is 0, then user has selected the 'New' option.</summary>
		public long SelectedInsSubNum;

		///<summary></summary>
		public FormInsSelectSubscr(long subscriber, long patNum)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			Subscriber=subscriber;
			_patNum=patNum;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInsSelectSubscr));
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.listPlans = new System.Windows.Forms.ListBox();
			this.butNew = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(317,211);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75,26);
			this.butCancel.TabIndex = 0;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(226,211);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75,26);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// listPlans
			// 
			this.listPlans.Location = new System.Drawing.Point(24,21);
			this.listPlans.Name = "listPlans";
			this.listPlans.Size = new System.Drawing.Size(368,160);
			this.listPlans.TabIndex = 2;
			this.listPlans.DoubleClick += new System.EventHandler(this.listPlans_DoubleClick);
			// 
			// butNew
			// 
			this.butNew.AdjustImageLocation = new System.Drawing.Point(0,0);
			this.butNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butNew.Autosize = true;
			this.butNew.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNew.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNew.CornerRadius = 4F;
			this.butNew.Location = new System.Drawing.Point(26,211);
			this.butNew.Name = "butNew";
			this.butNew.Size = new System.Drawing.Size(85,26);
			this.butNew.TabIndex = 3;
			this.butNew.Text = "New Plan";
			this.butNew.Click += new System.EventHandler(this.butNew_Click);
			// 
			// FormInsSelectSubscr
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5,13);
			this.ClientSize = new System.Drawing.Size(420,263);
			this.Controls.Add(this.butNew);
			this.Controls.Add(this.listPlans);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormInsSelectSubscr";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Select Insurance Plan";
			this.Load += new System.EventHandler(this.FormInsSelectSubscr_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormInsSelectSubscr_Load(object sender, System.EventArgs e) {
			SubList=InsSubs.GetListForSubscriber(Subscriber);
			List<InsPlan> planList=InsPlans.RefreshForSubList(SubList);
			//PatPlan[] patPlanArray;
			string str;
			InsPlan plan;
			if(!InsSubs.ValidatePlanNumForList(SubList.Select(x => x.InsSubNum).ToList())) {//If !isValid, any links should have been fixed and we now need to update our list.
				SubList=InsSubs.GetListForSubscriber(Subscriber);
			}
			for(int i=0;i<SubList.Count;i++) {
				plan=InsPlans.GetPlan(SubList[i].PlanNum,planList);
				str=InsPlans.GetCarrierName(SubList[i].PlanNum,planList);
				if(plan.GroupNum!="") {
					str+=Lan.g(this," group:")+plan.GroupNum;
				}
				int countPatPlans=PatPlans.GetCountBySubNum(SubList[i].InsSubNum);
				if(countPatPlans==0) {
					str+=" "+Lan.g(this,"(not in use)");
				}
				listPlans.Items.Add(str);
			}
		}

		private void listPlans_DoubleClick(object sender, System.EventArgs e) {
			if(listPlans.SelectedIndex==-1){
				return;
			}
			if(PatPlans.GetCountForPatAndInsSub(SubList[listPlans.SelectedIndex].InsSubNum,_patNum)!=0) {
				MsgBox.Show(this,"This patient already has this plan attached.  If you would like to add a new plan for the same subscriber and insurance carrier, click new plan.");
				return;
			}
			SelectedInsSubNum=SubList[listPlans.SelectedIndex].InsSubNum;
			DialogResult=DialogResult.OK;
		}

		private void butNew_Click(object sender, System.EventArgs e) {
			SelectedInsSubNum=0;
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(listPlans.SelectedIndex==-1){
				MsgBox.Show(this,"Please select a plan first.");
				return;
			}
			if(PatPlans.GetCountForPatAndInsSub(SubList[listPlans.SelectedIndex].InsSubNum,_patNum)!=0) {
				MsgBox.Show(this,"This patient already has this plan attached.  If you would like to add a new plan for the same subscriber and insurance carrier, click new plan.");
				return;
			}
			SelectedInsSubNum=SubList[listPlans.SelectedIndex].InsSubNum;
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		

		

		


	}
}





















