using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;
using CodeBase;

namespace OpenDental
{
	/// <summary>
	/// Summary description for FormClaimPayTotal.
	/// </summary>
	public class FormClaimPayTotal : ODForm
	{
		private OpenDental.ValidDouble textWriteOff;
		private System.Windows.Forms.TextBox textInsPayAllowed;
		private OpenDental.ValidDouble textInsPayAmt;
		private System.Windows.Forms.TextBox textClaimFee;
		private OpenDental.ValidDouble textDedApplied;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butDeductible;
		private OpenDental.UI.Button butWriteOff;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		///<summary></summary>
		public ClaimProc[] ClaimProcsToEdit;
		private List<Procedure> ProcList;
		private Patient PatCur;
		private Family FamCur;
		private OpenDental.UI.ODGrid gridMain;
		private List<InsPlan> PlanList;
		private List<PatPlan> PatPlanList;
		private ValidDouble textLabFees;
		private List<InsSub> SubList;
		private List<Def> _listClaimPaymentTrackingDefs;
		private List<ClaimProc> _listClaimProcsOld;
		///<summary>True if the user has permission to edit WriteOffs based on the minimum proc.DateEntryC of the procedures to which the claimprocs
		///in the ClaimProcsToEdit array are attached.</summary>
		private bool _isWriteOffEditable;

		///<summary></summary>
		public FormClaimPayTotal(Patient patCur,Family famCur,List <InsPlan> planList,List<PatPlan> patPlanList,List<InsSub> subList){
			InitializeComponent();// Required for Windows Form Designer support
			FamCur=famCur;
			PatCur=patCur;
			PlanList=planList;
			SubList=subList;
			PatPlanList=patPlanList;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClaimPayTotal));
			this.textInsPayAllowed = new System.Windows.Forms.TextBox();
			this.textClaimFee = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.butWriteOff = new OpenDental.UI.Button();
			this.butDeductible = new OpenDental.UI.Button();
			this.textWriteOff = new OpenDental.ValidDouble();
			this.textInsPayAmt = new OpenDental.ValidDouble();
			this.textDedApplied = new OpenDental.ValidDouble();
			this.butCancel = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.textLabFees = new OpenDental.ValidDouble();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.SuspendLayout();
			// 
			// textInsPayAllowed
			// 
			this.textInsPayAllowed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textInsPayAllowed.Location = new System.Drawing.Point(578, 275);
			this.textInsPayAllowed.Name = "textInsPayAllowed";
			this.textInsPayAllowed.ReadOnly = true;
			this.textInsPayAllowed.Size = new System.Drawing.Size(62, 20);
			this.textInsPayAllowed.TabIndex = 116;
			this.textInsPayAllowed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textClaimFee
			// 
			this.textClaimFee.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textClaimFee.Location = new System.Drawing.Point(392, 275);
			this.textClaimFee.Name = "textClaimFee";
			this.textClaimFee.ReadOnly = true;
			this.textClaimFee.Size = new System.Drawing.Size(62, 20);
			this.textClaimFee.TabIndex = 118;
			this.textClaimFee.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(304, 278);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(84, 16);
			this.label1.TabIndex = 117;
			this.label1.Text = "Totals";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.Location = new System.Drawing.Point(407, 325);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(348, 39);
			this.label2.TabIndex = 122;
			this.label2.Text = "Before you click OK, the Deductible and the Ins Pay amounts should exactly match " +
    "the insurance EOB.";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.Location = new System.Drawing.Point(11, 283);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(116, 34);
			this.label3.TabIndex = 123;
			this.label3.Text = "Assign to selected procedure:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.Location = new System.Drawing.Point(155, 289);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(108, 29);
			this.label4.TabIndex = 124;
			this.label4.Text = "On all unpaid amounts:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// butWriteOff
			// 
			this.butWriteOff.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butWriteOff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butWriteOff.Autosize = true;
			this.butWriteOff.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butWriteOff.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butWriteOff.CornerRadius = 4F;
			this.butWriteOff.Location = new System.Drawing.Point(154, 324);
			this.butWriteOff.Name = "butWriteOff";
			this.butWriteOff.Size = new System.Drawing.Size(90, 25);
			this.butWriteOff.TabIndex = 121;
			this.butWriteOff.Text = "&Write Off";
			this.butWriteOff.Click += new System.EventHandler(this.butWriteOff_Click);
			// 
			// butDeductible
			// 
			this.butDeductible.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDeductible.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDeductible.Autosize = true;
			this.butDeductible.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDeductible.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDeductible.CornerRadius = 4F;
			this.butDeductible.Location = new System.Drawing.Point(14, 324);
			this.butDeductible.Name = "butDeductible";
			this.butDeductible.Size = new System.Drawing.Size(92, 25);
			this.butDeductible.TabIndex = 120;
			this.butDeductible.Text = "&Deductible";
			this.butDeductible.Click += new System.EventHandler(this.butDeductible_Click);
			// 
			// textWriteOff
			// 
			this.textWriteOff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textWriteOff.Location = new System.Drawing.Point(702, 275);
			this.textWriteOff.MaxVal = 100000000D;
			this.textWriteOff.MinVal = -100000000D;
			this.textWriteOff.Name = "textWriteOff";
			this.textWriteOff.ReadOnly = true;
			this.textWriteOff.Size = new System.Drawing.Size(62, 20);
			this.textWriteOff.TabIndex = 119;
			this.textWriteOff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textInsPayAmt
			// 
			this.textInsPayAmt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textInsPayAmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textInsPayAmt.Location = new System.Drawing.Point(640, 275);
			this.textInsPayAmt.MaxVal = 100000000D;
			this.textInsPayAmt.MinVal = -100000000D;
			this.textInsPayAmt.Name = "textInsPayAmt";
			this.textInsPayAmt.ReadOnly = true;
			this.textInsPayAmt.Size = new System.Drawing.Size(62, 20);
			this.textInsPayAmt.TabIndex = 115;
			this.textInsPayAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textDedApplied
			// 
			this.textDedApplied.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textDedApplied.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textDedApplied.Location = new System.Drawing.Point(516, 275);
			this.textDedApplied.MaxVal = 100000000D;
			this.textDedApplied.MinVal = -100000000D;
			this.textDedApplied.Name = "textDedApplied";
			this.textDedApplied.ReadOnly = true;
			this.textDedApplied.Size = new System.Drawing.Size(62, 20);
			this.textDedApplied.TabIndex = 114;
			this.textDedApplied.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
			this.butCancel.Location = new System.Drawing.Point(1038, 324);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 25);
			this.butCancel.TabIndex = 2;
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
			this.butOK.Location = new System.Drawing.Point(949, 324);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 25);
			this.butOK.TabIndex = 1;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// textLabFees
			// 
			this.textLabFees.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textLabFees.Location = new System.Drawing.Point(454, 275);
			this.textLabFees.MaxVal = 100000000D;
			this.textLabFees.MinVal = -100000000D;
			this.textLabFees.Name = "textLabFees";
			this.textLabFees.ReadOnly = true;
			this.textLabFees.Size = new System.Drawing.Size(62, 20);
			this.textLabFees.TabIndex = 139;
			this.textLabFees.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(8, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.OneCell;
			this.gridMain.Size = new System.Drawing.Size(1146, 257);
			this.gridMain.TabIndex = 125;
			this.gridMain.Title = "Procedures";
			this.gridMain.TranslationName = "TableClaimProc";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			this.gridMain.CellTextChanged += new System.EventHandler(this.gridMain_CellTextChanged);
			// 
			// FormClaimPayTotal
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(1166, 363);
			this.Controls.Add(this.textLabFees);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.butWriteOff);
			this.Controls.Add(this.butDeductible);
			this.Controls.Add(this.textWriteOff);
			this.Controls.Add(this.textInsPayAllowed);
			this.Controls.Add(this.textInsPayAmt);
			this.Controls.Add(this.textClaimFee);
			this.Controls.Add(this.textDedApplied);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butCancel);
			this.Controls.Add(this.butOK);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(1000, 200);
			this.Name = "FormClaimPayTotal";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Enter Payment";
			this.Activated += new System.EventHandler(this.FormClaimPayTotal_Activated);
			this.Load += new System.EventHandler(this.FormClaimPayTotal_Load);
			this.Shown += new System.EventHandler(this.FormClaimPayTotal_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormClaimPayTotal_Load(object sender, System.EventArgs e) {
			_listClaimProcsOld=new List<ClaimProc>();
			foreach(ClaimProc cp in ClaimProcsToEdit) {
				_listClaimProcsOld.Add(cp.Copy());
			}
			ProcList=Procedures.Refresh(PatCur.PatNum);
			_isWriteOffEditable=Security.IsAuthorized(Permissions.InsWriteOffEdit,
				ProcList.FindAll(x => ClaimProcsToEdit.Any(y => y.ProcNum==x.ProcNum)).Select(x => x.DateEntryC).Min());
			butWriteOff.Enabled=_isWriteOffEditable;
			if(!CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				textLabFees.Visible=false;
				textDedApplied.Location=textLabFees.Location;
				textInsPayAllowed.Location=new Point(textDedApplied.Right-1,textInsPayAllowed.Location.Y);
				textInsPayAmt.Location=new Point(textInsPayAllowed.Right-1,textInsPayAllowed.Location.Y);
				textWriteOff.Location=new Point(textInsPayAmt.Right-1,textInsPayAllowed.Location.Y);
			}
			_listClaimPaymentTrackingDefs=Defs.GetDefsForCategory(DefCat.ClaimPaymentTracking,true);
			FillGrid();
		}

		private void FormClaimPayTotal_Shown(object sender,EventArgs e) {
			InsPlan plan=InsPlans.GetPlan(ClaimProcsToEdit[0].PlanNum,PlanList);
			if(plan.AllowedFeeSched!=0){//allowed fee sched
				gridMain.SetSelected(new Point(gridMain.Columns.GetIndex(Lan.g("TableClaimProc","Allowed")),0));//Allowed, first row.
			}
			else{
				gridMain.SetSelected(new Point(gridMain.Columns.GetIndex(Lan.g("TableClaimProc","Ins Pay")),0));//InsPay, first row.
			}
		}

		private void FillGrid(){
			//Changes made in this window do not get saved until after this window closes.
			//But if you double click on a row, then you will end up saving.  That shouldn't hurt anything, but could be improved.
			//also calculates totals for this "payment"
			//the payment itself is imaginary and is simply the sum of the claimprocs on this form
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			List<string> listDefDescripts=new List<string>();
			listDefDescripts.Add("None");
			for(int i=0;i<_listClaimPaymentTrackingDefs.Count;i++){
				listDefDescripts.Add(_listClaimPaymentTrackingDefs[i].ItemName);
			}
			ODGridColumn col=new ODGridColumn(Lan.g("TableClaimProc","Date"),66);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Prov"),50);
			gridMain.Columns.Add(col);
			if(Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
				col=new ODGridColumn(Lan.g("TableClaimProc","Code"),75);
				gridMain.Columns.Add(col);
			}
			else {
				col=new ODGridColumn(Lan.g("TableClaimProc","Code"),50);
				gridMain.Columns.Add(col);
				col=new ODGridColumn(Lan.g("TableClaimProc","Tth"),25);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g("TableClaimProc","Description"),130);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Fee"),62,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Ins Fee"),62,HorizontalAlignment.Right);
			gridMain.Columns.Add(col);
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				col=new ODGridColumn(Lan.g("TableClaimProc","Labs"),62,HorizontalAlignment.Right);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g("TableClaimProc","Deduct"),62,HorizontalAlignment.Right,true);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Allowed"),62,HorizontalAlignment.Right,true);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Ins Pay"),62,HorizontalAlignment.Right,true);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Writeoff"),62,HorizontalAlignment.Right,_isWriteOffEditable);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Status"),50,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Pmt"),62,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Pay Tracking"),130,listDefDescripts,160);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableClaimProc","Remarks"),130,true);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			Procedure ProcCur;
			for(int i=0;i<ClaimProcsToEdit.Length;i++){
				row=new ODGridRow();
				if(ClaimProcsToEdit[i].ProcNum==0) {//Total payment
					//We want to always show the "Payment Date" instead of the procedure date for total payments because they are not associated to procedures.
					row.Cells.Add(ClaimProcsToEdit[i].DateCP.ToShortDateString());
				}
				else {
					row.Cells.Add(ClaimProcsToEdit[i].ProcDate.ToShortDateString());
				}
				row.Cells.Add(Providers.GetAbbr(ClaimProcsToEdit[i].ProvNum));
				string procFee="";
				if(ClaimProcsToEdit[i].ProcNum==0) {
					row.Cells.Add("");
					if(!Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
						row.Cells.Add("");
					}
					row.Cells.Add(Lan.g(this,"Total Payment"));
				}
				else {
					ProcCur=Procedures.GetProcFromList(ProcList,ClaimProcsToEdit[i].ProcNum);//will return a new procedure if none found.
					procFee=(ProcCur.ProcFee*(ProcCur.BaseUnits+ProcCur.UnitQty)).ToString("F");
					ProcedureCode procCode=ProcedureCodes.GetProcCode(ProcCur.CodeNum);
					row.Cells.Add(procCode.ProcCode);
					if(!Clinics.IsMedicalPracticeOrClinic(Clinics.ClinicNum)) {
						row.Cells.Add(Tooth.ToInternat(ProcCur.ToothNum));
					}
					string descript=procCode.Descript;
					if(procCode.IsCanadianLab) {
						descript="^ ^ "+descript;
					}
					row.Cells.Add(descript);
				}
				row.Cells.Add(procFee);
				row.Cells.Add(ClaimProcsToEdit[i].FeeBilled.ToString("F"));
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
					decimal labFeesForProc=0;
					List<Procedure> labFeeProcs=Procedures.GetCanadianLabFees(ClaimProcsToEdit[i].ProcNum,ProcList);
					for(int j=0;j<labFeeProcs.Count;j++) {
						labFeesForProc+=(decimal)labFeeProcs[j].ProcFee;
					}
					row.Cells.Add(labFeesForProc.ToString("F"));
				}
				row.Cells.Add(ClaimProcsToEdit[i].DedApplied.ToString("F"));
				if(ClaimProcsToEdit[i].AllowedOverride==-1){
					row.Cells.Add("");
				}
				else{
					row.Cells.Add(ClaimProcsToEdit[i].AllowedOverride.ToString("F"));
				}
				row.Cells.Add(ClaimProcsToEdit[i].InsPayAmt.ToString("F"));
				row.Cells.Add(ClaimProcsToEdit[i].WriteOff.ToString("F"));
				switch(ClaimProcsToEdit[i].Status){
					case ClaimProcStatus.Received:
						row.Cells.Add(Lan.g("TableClaimProc","Recd"));
						break;
					case ClaimProcStatus.NotReceived:
						row.Cells.Add("");
						break;
					//adjustment would never show here
					case ClaimProcStatus.Preauth:
						row.Cells.Add(Lan.g("TableClaimProc","PreA"));
						break;
					case ClaimProcStatus.Supplemental:
						row.Cells.Add(Lan.g("TableClaimProc","Supp"));
						break;
					case ClaimProcStatus.CapClaim:
						row.Cells.Add(Lan.g("TableClaimProc","Cap"));
						break;
					//Estimate would never show here
					//Cap would never show here
				}
				if(ClaimProcsToEdit[i].ClaimPaymentNum>0){
					row.Cells.Add("X");
				}
				else{
					row.Cells.Add("");
				}
				bool isDefPresent=false;
				for(int j=0;j<_listClaimPaymentTrackingDefs.Count;j++) {
					if(ClaimProcsToEdit[i].ClaimPaymentTracking==_listClaimPaymentTrackingDefs[j].DefNum) {
						row.Cells.Add(_listClaimPaymentTrackingDefs[j].ItemName);
						row.Cells[row.Cells.Count-1].SelectedIndex=j+1;
						isDefPresent=true;
						break;
					}
				}
				if(!isDefPresent) { //The ClaimPaymentTracking definition has been hidden or ClaimPaymentTracking==0					
					row.Cells.Add("");
					row.Cells[row.Cells.Count-1].SelectedIndex=0;
				}
				row.Cells.Add(ClaimProcsToEdit[i].Remarks);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			FillTotals();
		}

		private void gridMain_CellDoubleClick(object sender,OpenDental.UI.ODGridClickEventArgs e) {
			if(!SaveGridChanges()) {
				return;
			}
			List<ClaimProcHist> histList=null;
			List<ClaimProcHist> loopList=null;
			FormClaimProc FormCP=new FormClaimProc(ClaimProcsToEdit[e.Row],null,FamCur,PatCur,PlanList,histList,ref loopList,PatPlanList,false,SubList);
			FormCP.IsInClaim=true;
			//no need to worry about permissions here
			FormCP.ShowDialog();
			if(FormCP.DialogResult!=DialogResult.OK){
				return;
			}
			FillGrid();
			FillTotals();
		}

		private void gridMain_CellTextChanged(object sender,EventArgs e) {
			FillTotals();
		}

		///<Summary>Fails silently if text is in invalid format.</Summary>
		private void FillTotals(){
			double claimFee=0;
			double labFees=0;
			double dedApplied=0;
			double insPayAmtAllowed=0;
			double insPayAmt=0;
			double writeOff=0;
			//double amt;
			for(int i=0;i<gridMain.Rows.Count;i++){
				claimFee+=ClaimProcsToEdit[i].FeeBilled;//5
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
					labFees+=PIn.Double(gridMain.Rows[i].Cells[gridMain.Columns.GetIndex(Lan.g("TableClaimProc","Labs"))].Text);
				}
				dedApplied+=PIn.Double(gridMain.Rows[i].Cells[gridMain.Columns.GetIndex(Lan.g("TableClaimProc","Deduct"))].Text);
				insPayAmtAllowed+=PIn.Double(gridMain.Rows[i].Cells[gridMain.Columns.GetIndex(Lan.g("TableClaimProc","Allowed"))].Text);
				insPayAmt+=PIn.Double(gridMain.Rows[i].Cells[gridMain.Columns.GetIndex(Lan.g("TableClaimProc","Ins Pay"))].Text);
				writeOff+=PIn.Double(gridMain.Rows[i].Cells[gridMain.Columns.GetIndex(Lan.g("TableClaimProc","Writeoff"))].Text);
			}
			textClaimFee.Text=claimFee.ToString("F");
			textLabFees.Text=labFees.ToString("F");
			textDedApplied.Text=dedApplied.ToString("F");
			textInsPayAllowed.Text=insPayAmtAllowed.ToString("F");
			textInsPayAmt.Text=insPayAmt.ToString("F");
			textWriteOff.Text=writeOff.ToString("F");
		}

		private bool SaveGridChanges(){
			//validate all grid cells
			double dbl=0;
			int deductIdx=gridMain.Columns.GetIndex(Lan.g("TableClaimProc","Deduct"));
			int allowedIdx=gridMain.Columns.GetIndex(Lan.g("TableClaimProc","Allowed"));
			int insPayIdx=gridMain.Columns.GetIndex(Lan.g("TableClaimProc","Ins Pay"));
			int writeoffIdx=gridMain.Columns.GetIndex(Lan.g("TableClaimProc","Writeoff"));
			int statusIdx=gridMain.Columns.GetIndex(Lan.g("TableClaimProc","Status"));
			for(int i=0;i<gridMain.Rows.Count;i++){
				try{
					//Check for invalid numbers being entered.
					if(gridMain.Rows[i].Cells[deductIdx].Text != "") {
						dbl=Convert.ToDouble(gridMain.Rows[i].Cells[deductIdx].Text);
					}
					if(gridMain.Rows[i].Cells[allowedIdx].Text != "") {
						dbl=Convert.ToDouble(gridMain.Rows[i].Cells[allowedIdx].Text);
					}
					if(gridMain.Rows[i].Cells[insPayIdx].Text != "") {
						dbl=Convert.ToDouble(gridMain.Rows[i].Cells[insPayIdx].Text);
					}
					if(gridMain.Rows[i].Cells[writeoffIdx].Text != "") {
						dbl=Convert.ToDouble(gridMain.Rows[i].Cells[writeoffIdx].Text);
						if(dbl<0 && gridMain.Rows[i].Cells[statusIdx].Text!="Supp") {
							MsgBox.Show(this,"Only supplemental payments can have a negative writeoff.");
							return false;
						}
						double claimWriteOffTotal=ClaimProcs.GetClaimWriteOffTotal(ClaimProcsToEdit[0].ClaimNum,ClaimProcsToEdit[i].ProcNum,ClaimProcsToEdit.ToList());
						if(claimWriteOffTotal+dbl<0) {
							MsgBox.Show(this,"The current writeoff value for supplemental payment "+(i+1).ToString()+" will cause the procedure's total writeoff to be negative.  Please change it to be at least "+(dbl-(claimWriteOffTotal+dbl)).ToString()+" to continue.");
							return false;
						}
					}
				}
				catch{
					MsgBox.Show(this,"Invalid number.  It needs to be in 0.00 form.");
					return false;
				}
			}
			if(!isClaimProcGreaterThanProcFee()) {
				return false;
			}
			for(int i=0;i<ClaimProcsToEdit.Length;i++) {
				ClaimProcsToEdit[i].DedApplied=PIn.Double(gridMain.Rows[i].Cells[deductIdx].Text);
				if(gridMain.Rows[i].Cells[allowedIdx].Text=="") {
					ClaimProcsToEdit[i].AllowedOverride=-1;
				}
				else {
					ClaimProcsToEdit[i].AllowedOverride=PIn.Double(gridMain.Rows[i].Cells[allowedIdx].Text);
				}
				ClaimProcsToEdit[i].InsPayAmt=PIn.Double(gridMain.Rows[i].Cells[insPayIdx].Text);
				ClaimProcsToEdit[i].WriteOff=PIn.Double(gridMain.Rows[i].Cells[writeoffIdx].Text);
				int idx=gridMain.Rows[i].Cells[gridMain.Columns.GetIndex(Lan.g("TableClaimProc","Pay Tracking"))].SelectedIndex;
				ClaimProcsToEdit[i].ClaimPaymentTracking=idx==0 ? 0 : _listClaimPaymentTrackingDefs[idx-1].DefNum;
				ClaimProcsToEdit[i].Remarks=gridMain.Rows[i].Cells[gridMain.Columns.GetIndex(Lan.g("TableClaimProc","Remarks"))].Text;
			}
			return true;
		}

		/// <summary>Returns true if ClaimProcAllowCreditsGreaterThanProcFee preference allows the user to add credits greater than the proc fee. Otherwise returns false </summary>
		private bool isClaimProcGreaterThanProcFee() {
			ClaimProcCreditsGreaterThanProcFee creditsGreaterPref=(ClaimProcCreditsGreaterThanProcFee)PrefC.GetInt(PrefName.ClaimProcAllowCreditsGreaterThanProcFee);
			if(creditsGreaterPref==ClaimProcCreditsGreaterThanProcFee.Allow) {
				return true;
			}
			List<Procedure> listProcs=Procedures.GetManyProc(ClaimProcsToEdit.Select(x=>x.ProcNum).ToList(),false);
			List<ClaimProc> listClaimProcsForPat=ClaimProcs.Refresh(PatCur.PatNum);
			List<PaySplit> listPaySplitForSelectedCP= PaySplits.GetPaySplitsFromProcs(ClaimProcsToEdit.Select(x=>x.ProcNum).ToList());
			List<Adjustment> listAdjForSelectedCP=Adjustments.GetForProcs(ClaimProcsToEdit.Select(x=>x.ProcNum).ToList());
			bool isCreditGreater=false;
			List<string> listProcDescripts=new List<string>();
			for(int i=0;i<ClaimProcsToEdit.Length;i++) {
				ClaimProc claimProcCur=ClaimProcsToEdit[i];
				int insPayIdx=gridMain.Columns.GetIndex(Lan.g("TableClaimProc","Ins Pay"));
				int writeoffIdx=gridMain.Columns.GetIndex(Lan.g("TableClaimProc","Writeoff"));
				int feeAcctIdx=gridMain.Columns.GetIndex(Lan.g("TableClaimProc","Fee"));
				decimal insPayAmt=(decimal)ClaimProcs.ProcInsPay(listClaimProcsForPat.FindAll(x => x.ClaimProcNum!=claimProcCur.ClaimProcNum),claimProcCur.ProcNum)
					+PIn.Decimal(gridMain.Rows[i].Cells[insPayIdx].Text);
				decimal writeOff=(decimal)ClaimProcs.ProcWriteoff(listClaimProcsForPat.FindAll(x => x.ClaimProcNum!=claimProcCur.ClaimProcNum),claimProcCur.ProcNum)
					+PIn.Decimal(gridMain.Rows[i].Cells[writeoffIdx].Text);
				decimal feeAcct=PIn.Decimal(gridMain.Rows[i].Cells[feeAcctIdx].Text);
				decimal adj=listAdjForSelectedCP.Where(x=>x.ProcNum==claimProcCur.ProcNum).Select(x=>(decimal)x.AdjAmt).Sum();
				decimal patPayAmt=listPaySplitForSelectedCP.Where(x=>x.ProcNum==claimProcCur.ProcNum).Select(x=>(decimal)x.SplitAmt).Sum();
				//Any changes to this calculation should also consider FormClaimProc.IsClaimProcGreaterThanProcFee().
				decimal creditRem=feeAcct-patPayAmt-insPayAmt-writeOff+adj;
				isCreditGreater|=(creditRem.IsLessThanZero());
				if(creditRem.IsLessThanZero()) {
					Procedure proc=listProcs.FirstOrDefault(x=>x.ProcNum==claimProcCur.ProcNum);
					listProcDescripts.Add((proc==null ? "" : ProcedureCodes.GetProcCode(proc.CodeNum).ProcCode)
						+"\t"+Lan.g(this,"Fee")+": "+feeAcct.ToString("F")
						+"\t"+Lan.g(this,"Credits")+": "+(Math.Abs(-patPayAmt-insPayAmt-writeOff+adj)).ToString("F")
						+"\t"+Lan.g(this,"Remaining")+": ("+Math.Abs(creditRem).ToString("F")+")");
				}
			}
			if(!isCreditGreater) {
				return true;
			}
			if(creditsGreaterPref==ClaimProcCreditsGreaterThanProcFee.Block) {
				MsgBoxCopyPaste msgBox=new MsgBoxCopyPaste(Lan.g(this,"Remaining amount is negative for the following procedures")+":\r\n"
					+string.Join("\r\n",listProcDescripts)+"\r\n"+Lan.g(this,"Not allowed to continue."));
				msgBox.Text=Lan.g(this,"Overpaid Procedure Warning");
				msgBox.ShowDialog();
				return false;
			}
			if(creditsGreaterPref==ClaimProcCreditsGreaterThanProcFee.Warn) {
				return MessageBox.Show(Lan.g(this,"Remaining amount is negative for the following procedures")+":\r\n"
					+string.Join("\r\n",listProcDescripts.Take(10))+"\r\n"+(listProcDescripts.Count>10?"...\r\n":"")+Lan.g(this,"Continue?")
					,Lan.g(this,"Overpaid Procedure Warning"),MessageBoxButtons.OKCancel)==DialogResult.OK;
			} 
			return true;//should never get to this line, only possible if another enum value is added to allow, warn, and block
		}

		private void butDeductible_Click(object sender, System.EventArgs e) {
			if(gridMain.SelectedCell.X==-1){
				MsgBox.Show(this,"Please select one procedure.  Then click this button to assign the deductible to that procedure.");
				return;
			}
			if(!SaveGridChanges()) {
				return;
			}
			Double dedAmt=0;
			//remove the existing deductible from each procedure and move it to dedAmt.
			for(int i=0;i<ClaimProcsToEdit.Length;i++){
				if(ClaimProcsToEdit[i].DedApplied > 0){
					dedAmt+=ClaimProcsToEdit[i].DedApplied;
					ClaimProcsToEdit[i].InsPayEst+=ClaimProcsToEdit[i].DedApplied;//dedAmt might be more
					ClaimProcsToEdit[i].InsPayAmt+=ClaimProcsToEdit[i].DedApplied;
					ClaimProcsToEdit[i].DedApplied=0;
				}
			}
			if(dedAmt==0){
				MsgBox.Show(this,"There does not seem to be a deductible to apply.  You can still apply a deductible manually by double clicking on a procedure.");
				return;
			}
			//then move dedAmt to the selected proc
			ClaimProcsToEdit[gridMain.SelectedCell.Y].DedApplied=dedAmt;
			ClaimProcsToEdit[gridMain.SelectedCell.Y].InsPayEst-=dedAmt;
			ClaimProcsToEdit[gridMain.SelectedCell.Y].InsPayAmt-=dedAmt;
			FillGrid();
		}

		public void butWriteOff_Click(object sender, System.EventArgs e) {
			DialogResult dresWriteoff=DialogResult.Cancel;
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				dresWriteoff=MessageBox.Show(
					 Lan.g(this,"Write off unpaid amounts on labs and procedures?")+"\r\n"
					+Lan.g(this,"Choose Yes to write off unpaid amounts on both labs and procedures.")+"\r\n"
					+Lan.g(this,"Choose No to write off unpaid amounts on procedures only."),"",MessageBoxButtons.YesNoCancel);
				if(dresWriteoff!=DialogResult.Yes && dresWriteoff!=DialogResult.No) {//Cancel
					return;
				}
			}
			else {//United States
				if(MessageBox.Show(Lan.g(this,"Write off unpaid amount on each procedure?"),"",MessageBoxButtons.OKCancel)!=DialogResult.OK) {
					return;
				}
			}
			if(!SaveGridChanges()) {
				return;
			}
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA") && dresWriteoff==DialogResult.Yes) {//Canadian. en-CA or fr-CA
				Claim claim=Claims.GetClaim(ClaimProcsToEdit[0].ClaimNum);//There should be at least one, since a claim can only be created with one or more procedures.
				ClaimProc cpTotalLabs=new ClaimProc();
				cpTotalLabs.ClaimNum=claim.ClaimNum;
				cpTotalLabs.PatNum=claim.PatNum;
				cpTotalLabs.ProvNum=claim.ProvTreat;
				cpTotalLabs.Status=ClaimProcStatus.Received;
				cpTotalLabs.PlanNum=claim.PlanNum;
				cpTotalLabs.InsSubNum=claim.InsSubNum;
				cpTotalLabs.DateCP=DateTimeOD.Today;
				cpTotalLabs.ProcDate=claim.DateService;
				cpTotalLabs.DateEntry=DateTime.Now;
				cpTotalLabs.ClinicNum=claim.ClinicNum;
				cpTotalLabs.WriteOff=0;
				cpTotalLabs.InsPayAmt=0;
				for(int i=0;i<ClaimProcsToEdit.Length;i++) {
					ClaimProc claimProc=ClaimProcsToEdit[i];
					double procLabInsPaid=0;
					if(claimProc.InsPayAmt>claimProc.FeeBilled) {
						procLabInsPaid=claimProc.InsPayAmt-claimProc.FeeBilled;//The amount of exceess greater than the fee billed.
						claimProc.InsPayAmt=claimProc.FeeBilled;
					}
					List<Procedure> listProcLabs=Procedures.GetCanadianLabFees(claimProc.ProcNum);//0, 1 or 2 lab fees per procedure
					double procLabTotal=0;
					for(int j=0;j<listProcLabs.Count;j++) {
						procLabTotal+=listProcLabs[j].ProcFee;
					}
					if(procLabInsPaid>procLabTotal) {//Could happen if the user enters a payment amount greater than the fee billed and lab fees added together.
						procLabInsPaid=procLabTotal;
					}
					cpTotalLabs.InsPayAmt+=procLabInsPaid;
					cpTotalLabs.WriteOff+=procLabTotal-procLabInsPaid;
				}
				if(cpTotalLabs.InsPayAmt>0 || cpTotalLabs.WriteOff>0) {//These amounts will both be zero if there are no lab fees on any of the procedures.  These amounts should never be negative.
					ClaimProcs.Insert(cpTotalLabs);
				}
			}
			//fix later: does not take into account other payments.
			double unpaidAmt=0;
			List<Procedure> ProcList=Procedures.Refresh(PatCur.PatNum);
			for(int i=0;i<ClaimProcsToEdit.Length;i++){
				//ClaimProcsToEdit guaranteed to only contain claimprocs for procedures before this form loads, payments are not in the list
				unpaidAmt=Procedures.GetProcFromList(ProcList,ClaimProcsToEdit[i].ProcNum).ProcFee
					//((Procedure)Procedures.HList[ClaimProcsToEdit[i].ProcNum]).ProcFee
					-ClaimProcsToEdit[i].DedApplied
					-ClaimProcsToEdit[i].InsPayAmt;
				if(unpaidAmt > 0){
					ClaimProcsToEdit[i].WriteOff=unpaidAmt;
				}
			}
			FillGrid();
		}

		private void SaveAllowedFees(){
			//if no allowed fees entered, then nothing to do 
			bool allowedFeesEntered=false;
			for(int i=0;i<gridMain.Rows.Count;i++){
				if(gridMain.Rows[i].Cells[gridMain.Columns.GetIndex(Lan.g("TableClaimProc","Allowed"))].Text!=""){
					allowedFeesEntered=true;
					break;
				}
			}
			if(!allowedFeesEntered){
				return;
			}
			//if no allowed fee schedule, then nothing to do
			InsPlan plan=InsPlans.GetPlan(ClaimProcsToEdit[0].PlanNum,PlanList);
			if(plan.AllowedFeeSched==0){//no allowed fee sched
				//plan.PlanType!="p" && //not ppo, and 
				return;
			}
			//ask user if they want to save the fees
			if(!MsgBox.Show(this,true,"Save the allowed amounts to the allowed fee schedule?")){
				return;
			}
			//select the feeSchedule
			long feeSched=-1;
			//if(plan.PlanType=="p"){//ppo
			//	feeSched=plan.FeeSched;
			//}
			//else if(plan.AllowedFeeSched!=0){//an allowed fee schedule exists
			feeSched=plan.AllowedFeeSched;
			//}
			if(FeeScheds.GetIsHidden(feeSched)){
				MsgBox.Show(this,"Allowed fee schedule is hidden, so no changes can be made.");
				return;
			}
			Fee FeeCur=null;
			long codeNum;
			List<Procedure> ProcList=Procedures.Refresh(PatCur.PatNum);
			Procedure proc;
			List<long> invalidFeeSchedNums = new List<long>();
			for(int i=0;i<ClaimProcsToEdit.Length;i++){
				proc=Procedures.GetProcFromList(ProcList,ClaimProcsToEdit[i].ProcNum);
				codeNum=proc.CodeNum;
				//ProcNum not found or 0 for payments
				if(codeNum==0){
					continue;
				}
				if(gridMain.Rows[i].Cells[gridMain.Columns.GetIndex(Lan.g("TableClaimProc","Allowed"))].Text.Trim()==""//Nothing is entered in allowed 
					&& _listClaimProcsOld[i].AllowedOverride==-1) //And there was not originally a value in the allowed column
				{
					continue;
				}
				DateTime datePrevious=DateTime.MinValue;
				FeeCur=Fees.GetFee(codeNum,feeSched,proc.ClinicNum,proc.ProvNum);
				if(FeeCur==null) {
					FeeSched feeSchedObj=FeeScheds.GetFirst(x => x.FeeSchedNum==feeSched);
					FeeCur=new Fee();
					FeeCur.FeeSched=feeSched;
					FeeCur.CodeNum=codeNum;
					FeeCur.ClinicNum=(feeSchedObj.IsGlobal) ? 0 : proc.ClinicNum;
					FeeCur.ProvNum=(feeSchedObj.IsGlobal) ? 0 : proc.ProvNum;
					FeeCur.Amount=PIn.Double(gridMain.Rows[i].Cells[gridMain.Columns.GetIndex(Lan.g("TableClaimProc","Allowed"))].Text);
					Fees.Insert(FeeCur);
				}
				else{
					datePrevious=FeeCur.SecDateTEdit;
					FeeCur.Amount=PIn.Double(gridMain.Rows[i].Cells[gridMain.Columns.GetIndex(Lan.g("TableClaimProc","Allowed"))].Text);
					Fees.Update(FeeCur);
				}
				SecurityLogs.MakeLogEntry(Permissions.ProcFeeEdit,0,Lan.g(this,"Procedure")+": "+ProcedureCodes.GetStringProcCode(FeeCur.CodeNum)
					+", "+Lan.g(this,"Fee")+": "+FeeCur.Amount.ToString("c")+", "+Lan.g(this,"Fee Schedule")+" "+FeeScheds.GetDescription(FeeCur.FeeSched)
					+". "+Lan.g(this,"Automatic change to allowed fee in Enter Payment window.  Confirmed by user."),FeeCur.CodeNum,DateTime.MinValue);
				SecurityLogs.MakeLogEntry(Permissions.LogFeeEdit,0,Lan.g(this,"Fee Updated"),FeeCur.FeeNum,datePrevious);
				invalidFeeSchedNums.Add(FeeCur.FeeSched);
			}
			foreach(long feeSchedNum in invalidFeeSchedNums.Distinct()) {
				Signalods.SetInvalid(InvalidType.Fees,KeyType.FeeSched,feeSchedNum);
			}
		}

		private void butOK_Click(object sender,System.EventArgs e) {
			if(!SaveGridChanges()) {
				return;
			}
			SaveAllowedFees();
			if(PrefC.GetBool(PrefName.ClaimSnapshotEnabled)) {
				Claim claimCur=Claims.GetClaim(_listClaimProcsOld[0].ClaimNum);
				if(claimCur.ClaimType!="PreAuth") {
					ClaimSnapshots.CreateClaimSnapshot(_listClaimProcsOld,ClaimSnapshotTrigger.InsPayment,claimCur.ClaimType);
				}
			}
			ClaimTrackings.InsertClaimProcReceived(ClaimProcsToEdit[0].ClaimNum,Security.CurUser.UserNum);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormClaimPayTotal_Activated(object sender,EventArgs e) {

		}

	
	}
}







