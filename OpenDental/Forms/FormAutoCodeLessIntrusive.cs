using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using CodeBase;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormAutoCodeLessIntrusive:ODForm {
		private System.Windows.Forms.Label labelMain;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butNo;
		private OpenDental.UI.Button butYes;
		private Label labelPrompt;
		///<summary>The text to display in this dialog</summary>
		public string mainText;
		private Patient _patCur;
		private Procedure _procCur;
		private ProcedureCode _procCodeCur;
		private long _verifyCode;
		private List<PatPlan> _listPatPlans;
		private List<InsSub> _listInsSubs;
		private List<InsPlan> _listInsPlans;
		private List<Benefit> _listBenefits;
		private List<ClaimProc> _listClaimProcs;
		private string _teethText;

		public Procedure Proc {
			get {
				return _procCur;
			}
		}

		///<summary></summary>
		public FormAutoCodeLessIntrusive(Patient pat,Procedure proc,ProcedureCode procCode,long verifyCode,List<PatPlan> listPatPlans,
			List<InsSub> listInsSubs,List<InsPlan> listInsPlans,List<Benefit> listBenefits,List<ClaimProc> listClaimProcs,string teethText=null)
		{
			_patCur=pat;
			_procCur=proc;
			_procCodeCur=procCode;
			_verifyCode=verifyCode;
			_listPatPlans=listPatPlans;
			_listInsSubs=listInsSubs;
			_listInsPlans=listInsPlans;
			_listBenefits=listBenefits;
			_listClaimProcs=listClaimProcs;
			_teethText=teethText;
			InitializeComponent();
			Lan.F(this,new Control[] {labelMain});
			//labelMain is translated from calling Form (FormProcEdit)
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAutoCodeLessIntrusive));
			this.butNo = new OpenDental.UI.Button();
			this.butYes = new OpenDental.UI.Button();
			this.labelMain = new System.Windows.Forms.Label();
			this.labelPrompt = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// butNo
			// 
			this.butNo.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNo.Autosize = true;
			this.butNo.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNo.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNo.CornerRadius = 4F;
			this.butNo.Location = new System.Drawing.Point(406, 169);
			this.butNo.Name = "butNo";
			this.butNo.Size = new System.Drawing.Size(75, 26);
			this.butNo.TabIndex = 0;
			this.butNo.Text = "&No";
			this.butNo.Click += new System.EventHandler(this.butNo_Click);
			// 
			// butYes
			// 
			this.butYes.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butYes.Autosize = true;
			this.butYes.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butYes.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butYes.CornerRadius = 4F;
			this.butYes.Location = new System.Drawing.Point(406, 128);
			this.butYes.Name = "butYes";
			this.butYes.Size = new System.Drawing.Size(75, 26);
			this.butYes.TabIndex = 1;
			this.butYes.Text = "&Yes";
			this.butYes.Click += new System.EventHandler(this.butYes_Click);
			// 
			// labelMain
			// 
			this.labelMain.Location = new System.Drawing.Point(35, 32);
			this.labelMain.Name = "labelMain";
			this.labelMain.Size = new System.Drawing.Size(438, 73);
			this.labelMain.TabIndex = 3;
			this.labelMain.Text = "labelMain";
			this.labelMain.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// labelPrompt
			// 
			this.labelPrompt.Location = new System.Drawing.Point(12, 152);
			this.labelPrompt.Name = "labelPrompt";
			this.labelPrompt.Size = new System.Drawing.Size(388, 43);
			this.labelPrompt.TabIndex = 4;
			this.labelPrompt.Text = "If you don\'t want to be prompted to change this type of procedure in the future, " +
    "then edit this Auto Code and check the box for \"Do not check codes...\"";
			// 
			// FormAutoCodeLessIntrusive
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(511, 211);
			this.Controls.Add(this.labelPrompt);
			this.Controls.Add(this.labelMain);
			this.Controls.Add(this.butYes);
			this.Controls.Add(this.butNo);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAutoCodeLessIntrusive";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Change Code?";
			this.Load += new System.EventHandler(this.FormAutoCodeLessIntrusive_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormAutoCodeLessIntrusive_Load(object sender, System.EventArgs e) {
			//Moved from FormProcEdit.SaveAndClose() in version 16.3+
			labelMain.Text=ProcedureCodes.GetProcCode(_verifyCode).ProcCode
				+" ("+ProcedureCodes.GetProcCode(_verifyCode).Descript+") "
				+Lan.g("FormProcEdit","is the recommended procedure code for this procedure.  Change procedure code and fee?");
			if(PrefC.GetBool(PrefName.ProcEditRequireAutoCodes)) {
				butNo.Text=Lan.g(this,"Edit Proc");//Button will otherwise say 'No'.
			}
		}

		private void butYes_Click(object sender, System.EventArgs e) {
			//Customers have been complaining about procedurelog entries changing their CodeNum column to 0.
			//Based on a security log provided by a customer, we were able to determine that this is one of two potential violators.
			//The following code is here simply to try and get the user to call us so that we can have proof and hopefully find the core of the issue.
			try {
				if(_verifyCode < 1) {
					throw new ApplicationException("Invalid Verify Code");
				}
			}
			catch(ApplicationException ae) {
				string error="Please notify support with the following information.\r\n"
					+"Error: "+ae.Message+"\r\n"
					+"_verifyCode: "+_verifyCode.ToString()+"\r\n"
					+"_procCur.CodeNum: "+(_procCur==null ? "NULL" : _procCur.CodeNum.ToString())+"\r\n"
					+"_procCodeCur.CodeNum: "+(_procCodeCur==null ? "NULL" : _procCodeCur.CodeNum.ToString())+"\r\n"
					+"\r\n"
					+"StackTrace:\r\n"+ae.StackTrace;
				MsgBoxCopyPaste MsgBCP=new MsgBoxCopyPaste(error);
				MsgBCP.Text="Fatal Error!!!";
				MsgBCP.Show();//Use .Show() to make it easy for the user to keep this window open while they call in.
				return;
			}
			//Moved from FormProcEdit.SaveAndClose() in version 16.3+
			Procedure procOld=_procCur.Copy();
			_procCur.CodeNum=_verifyCode;
			if(new[] { ProcStat.TP,ProcStat.C,ProcStat.TPi }.Contains(_procCur.ProcStatus)) {//Only change the fee if Complete or TP
				InsSub prisub=null;
				InsPlan priplan=null;
				if(_listPatPlans.Count>0) {
					prisub=InsSubs.GetSub(_listPatPlans[0].InsSubNum,_listInsSubs);
					priplan=InsPlans.GetPlan(prisub.PlanNum,_listInsPlans);
				}
				_procCur.ProcFee=Fees.GetAmount0(_procCur.CodeNum,FeeScheds.GetFeeSched(_patCur,_listInsPlans,_listPatPlans,_listInsSubs,_procCur.ProvNum),
					_procCur.ClinicNum,_procCur.ProvNum);
				if(priplan!=null && priplan.PlanType=="p") {//PPO
					double standardfee=Fees.GetAmount0(_procCur.CodeNum,Providers.GetProv(Patients.GetProvNum(_patCur)).FeeSched,_procCur.ClinicNum,
						_procCur.ProvNum);
					_procCur.ProcFee=Math.Max(_procCur.ProcFee,standardfee);
				}
			}
			Procedures.Update(_procCur,procOld);
			//Compute estimates required, otherwise if adding through quick add, it could have incorrect WO or InsEst if code changed.
			Procedures.ComputeEstimates(_procCur,_patCur.PatNum,_listClaimProcs,false,_listInsPlans,_listPatPlans,_listBenefits,_patCur.Age,_listInsSubs);
			Recalls.Synch(_procCur.PatNum);
			if(_procCur.ProcStatus.In(ProcStat.C,ProcStat.EO,ProcStat.EC)) {
				string logText=_procCodeCur.ProcCode+" ("+_procCur.ProcStatus+"), ";
				if(_teethText!=null && _teethText.Trim()!="") {
					logText+=Lan.g(this,"Teeth")+": "+_teethText+", ";
				}
				logText+=Lan.g(this,"Fee")+": "+_procCur.ProcFee.ToString("F")+", "+_procCodeCur.Descript;
				Permissions perm=Permissions.ProcComplEdit;
				if(_procCur.ProcStatus.In(ProcStat.EO,ProcStat.EC)) {
					perm=Permissions.ProcExistingEdit;
				}
				SecurityLogs.MakeLogEntry(perm,_patCur.PatNum,logText);
			}
			DialogResult=DialogResult.OK;
		}

		private void butNo_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		


	}
}





















