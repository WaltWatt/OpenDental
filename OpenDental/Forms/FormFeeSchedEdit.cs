using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Collections.Generic;
using System.Linq;
using CodeBase;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormFeeSchedEdit:ODForm {
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textDescription;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Label label2;
		private ListBox listType;
		private CheckBox checkIsHidden;
		private CheckBox checkIsGlobal;
		public FeeSched FeeSchedCur;
		public Clinic ClinicCur;
		public List<FeeSched> ListFeeScheds;
		private List<Provider> _listProviders;

		///<summary></summary>
		public FormFeeSchedEdit()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFeeSchedEdit));
			this.label1 = new System.Windows.Forms.Label();
			this.textDescription = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.listType = new System.Windows.Forms.ListBox();
			this.checkIsHidden = new System.Windows.Forms.CheckBox();
			this.checkIsGlobal = new System.Windows.Forms.CheckBox();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(9, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(148, 17);
			this.label1.TabIndex = 2;
			this.label1.Text = "Description";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDescription
			// 
			this.textDescription.Location = new System.Drawing.Point(160, 20);
			this.textDescription.Name = "textDescription";
			this.textDescription.Size = new System.Drawing.Size(291, 20);
			this.textDescription.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(9, 46);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(148, 17);
			this.label2.TabIndex = 11;
			this.label2.Text = "Type";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// listType
			// 
			this.listType.FormattingEnabled = true;
			this.listType.Location = new System.Drawing.Point(160, 46);
			this.listType.Name = "listType";
			this.listType.Size = new System.Drawing.Size(120, 56);
			this.listType.TabIndex = 12;
			// 
			// checkIsHidden
			// 
			this.checkIsHidden.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsHidden.Location = new System.Drawing.Point(176, 107);
			this.checkIsHidden.Name = "checkIsHidden";
			this.checkIsHidden.Size = new System.Drawing.Size(104, 19);
			this.checkIsHidden.TabIndex = 13;
			this.checkIsHidden.Text = "Hidden";
			this.checkIsHidden.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsHidden.UseVisualStyleBackColor = true;
			this.checkIsHidden.Click += new System.EventHandler(this.checkIsHidden_Click);
			// 
			// checkIsGlobal
			// 
			this.checkIsGlobal.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsGlobal.Enabled = false;
			this.checkIsGlobal.Location = new System.Drawing.Point(112, 127);
			this.checkIsGlobal.Name = "checkIsGlobal";
			this.checkIsGlobal.Size = new System.Drawing.Size(168, 19);
			this.checkIsGlobal.TabIndex = 14;
			this.checkIsGlobal.Text = "Use Global Fees";
			this.checkIsGlobal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkIsGlobal.UseVisualStyleBackColor = true;
			this.checkIsGlobal.Click += new System.EventHandler(this.checkIsGlobal_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(309, 170);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 9;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(400, 170);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 10;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// FormFeeSchedEdit
			// 
			this.ClientSize = new System.Drawing.Size(501, 214);
			this.Controls.Add(this.checkIsGlobal);
			this.Controls.Add(this.checkIsHidden);
			this.Controls.Add(this.listType);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textDescription);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormFeeSchedEdit";
			this.ShowInTaskbar = false;
			this.Text = "Edit Fee Schedule";
			this.Load += new System.EventHandler(this.FormFeeSchedEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormFeeSchedEdit_Load(object sender, System.EventArgs e) {
			textDescription.Text=FeeSchedCur.Description;
			if(!FeeSchedCur.IsNew){
				listType.Enabled=false;
			}
			Array arrayValues=Enum.GetValues(typeof(FeeScheduleType));
			for(int i=0;i<arrayValues.Length;i++) {
				FeeScheduleType feeSchedType=((FeeScheduleType)arrayValues.GetValue(i));
				if(feeSchedType==FeeScheduleType.OutNetwork) {
					listType.Items.Add("Out of Network");
				}
				else {
					listType.Items.Add(arrayValues.GetValue(i).ToString());
				}
				if(FeeSchedCur.FeeSchedType==feeSchedType) {
					listType.SetSelected(i,true);
				}
			}
			checkIsHidden.Checked=FeeSchedCur.IsHidden;
			if(Clinics.ClinicNum==0) {//HQ clinic, let them change if a fee sched can be localized or not.
				checkIsGlobal.Enabled=true;
			}
			if(FeeSchedCur.IsNew) {
				checkIsGlobal.Checked=true;
			}
			else {
				checkIsGlobal.Checked=FeeSchedCur.IsGlobal;
			}
			_listProviders=Providers.GetDeepCopy(true);
		}

		private void checkIsGlobal_Click(object sender,EventArgs e) {
			if(checkIsGlobal.Checked) {//Checking IsGlobal (They want to delete their local fees for the feeschedule and use the HQ ones)
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Checking this option will use the global HQ fees and hide any clinic or provider specific fees.  Are you sure?")) {
					checkIsGlobal.Checked=false;
				}
			}
			else {//Unchecking IsGlobal (They want to create local fees for the feeschedule and override the HQ ones)
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Unchecking this option will allow:"+Environment.NewLine
					+"Fees to be different for other clinics or other providers for this same fee schedule. Are you sure?"))
				{
					checkIsGlobal.Checked=true;
				}
			}
		}

		private void checkIsHidden_Click(object sender,EventArgs e) {
			//Don't allow fees to be hidden if they are in use by a provider.
			if(!checkIsHidden.Checked) {
				return;//Unhiding a fee. OK.
			}
			if(FeeSchedCur.FeeSchedType!=FeeScheduleType.Normal) {
				return;//Not Normal fee. Not in use by a provider.
			}
			List<InsPlan> listInsPlanForFeeSched = InsPlans.GetForFeeSchedNum(FeeSchedCur.FeeSchedNum);
			if(listInsPlanForFeeSched.Count > 0) {
				string insPlanMsg = Lan.g(this,"This fee schedule is tied to")+" "
					+listInsPlanForFeeSched.Count+" "+Lan.g(this,"insurance plans.")+" "+Lan.g(this,"Continue?");
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,insPlanMsg)) {
					checkIsHidden.Checked=false;
					return;
				}
			}
			string providersUsingFee="";
			for(int i=0;i<_listProviders.Count;i++) {
				if(FeeSchedCur.FeeSchedNum==_listProviders[i].FeeSched) {
					if(providersUsingFee!=""){//There is a name before this on the list
						providersUsingFee+=", ";
					}
					providersUsingFee+=_listProviders[i].Abbr;
				}
			}
			if(providersUsingFee!="") {
				MessageBox.Show(Lan.g(this,"Cannot hide. Fee schedule is currently in use by the following providers")+":\r\n"+providersUsingFee);
				checkIsHidden.Checked=false;
			}
			string patsUsingFee="";
			//Don't allow fee schedules to be hidden if they are in use by a non-deleted patient.
			List<Patient> listPats=Patients.GetForFeeSched(FeeSchedCur.FeeSchedNum).FindAll(x => x.PatStatus!=PatientStatus.Deleted);
			patsUsingFee=string.Join("\r\n",listPats.Select(x => x.LName+", "+x.FName));
			if(patsUsingFee!="") {
				MsgBoxCopyPaste msgBoxCP=new MsgBoxCopyPaste(Lan.g(this,"Cannot hide. Fee schedule currently in use by the following non-deleted patients")
					+":\r\n"+patsUsingFee);
				msgBoxCP.ShowDialog();
				checkIsHidden.Checked=false;
			}
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			if(textDescription.Text==""){
				MsgBox.Show(this,"Description cannot be blank.");
				return;
			}
			FeeSchedCur.Description=textDescription.Text;
			FeeSchedCur.FeeSchedType=(FeeScheduleType)listType.SelectedIndex;
			FeeSchedCur.IsHidden=checkIsHidden.Checked;
			bool isGlobalOld=FeeSchedCur.IsGlobal;
			FeeSchedCur.IsGlobal=checkIsGlobal.Checked;
			if(FeeSchedCur.IsNew) {
				FeeSchedCur.IsNew=false;
				ListFeeScheds.Add(FeeSchedCur);
			}
			if(isGlobalOld!=FeeSchedCur.IsGlobal) {
				string log="Edited Fee Schedule \""+textDescription.Text+"\": Changed \"Use Headquarter's Fees\" from ";
				if(isGlobalOld) {
					log+="Checked ";
				}
				else {
					log+="Unchecked ";
				}
				if(FeeSchedCur.IsGlobal) {
					log+="to Checked";
				}
				else {
					log+="to Unchecked";
				}
				SecurityLogs.MakeLogEntry(Permissions.FeeSchedEdit,0,log);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}










	}
}





















