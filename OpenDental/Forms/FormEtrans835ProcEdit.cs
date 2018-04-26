using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;

namespace OpenDental {
	public partial class FormEtrans835ProcEdit:ODForm {

		private Hx835_Proc _proc;
		private decimal _patRespSum;
		private decimal _contractualObligationSum;
		private decimal _payorInitiatedReductionSum;
		private decimal _otherAdjustmentSum;

		public FormEtrans835ProcEdit(Hx835_Proc proc) {
			InitializeComponent();
			Lan.F(this);
			_proc=proc;
		}

		private void FormEtrans835ClaimEdit_Load(object sender,EventArgs e) {
			FillAll();
		}

		private void FormEtrans835ClaimEdit_Resize(object sender,EventArgs e) {
			FillProcedureAdjustments();//Because the grid columns change size depending on the form size.
			FillRemarks();//Because the grid columns change size depending on the form size.
			FillSupplementalInfo();//Because the grid columns change size depending on the form size.
		}

		private void FillAll() {
			FillProcedureAdjustments();
			FillHeader();//Must be after FillProcedureAdjustments().
			FillRemarks();
			FillSupplementalInfo();
		}

		private void FillHeader() {
			Text="Procedure Paid - Patient: "+_proc.ClaimPaid.PatientName;
			textProcAdjudicated.Text=_proc.ProcCodeAdjudicated;
			if(ProcedureCodes.IsValidCode(_proc.ProcCodeAdjudicated)) {
				textProcAdjudicated.Text=_proc.ProcCodeAdjudicated+" - "+ProcedureCodes.GetProcCode(_proc.ProcCodeAdjudicated).AbbrDesc;
			}
			textProcSubmitted.Text=_proc.ProcCodeBilled;
			if(ProcedureCodes.IsValidCode(_proc.ProcCodeBilled)) {
				textProcSubmitted.Text=_proc.ProcCodeBilled+" - "+ProcedureCodes.GetProcCode(_proc.ProcCodeBilled).AbbrDesc;
			}
			textDateService.Text=_proc.DateServiceStart.ToShortDateString();
			if(_proc.DateServiceEnd>_proc.DateServiceStart) {
				textDateService.Text+=" to "+_proc.DateServiceEnd.ToShortDateString();
				textDateService.Width=160;//Increase width to accout for extra text.
			}
			textInsPaid.Text=_proc.InsPaid.ToString("f2");
			if(_proc.ProcNum==0) {
				textProcNum.Text="";
			}
			else {
				textProcNum.Text=_proc.ProcNum.ToString();
			}
			textProcFee.Text=_proc.ProcFee.ToString("f2");			
			textInsPaidCalc.Text=(_proc.ProcFee-_patRespSum-_contractualObligationSum-_payorInitiatedReductionSum-_otherAdjustmentSum).ToString("f2");
		}

		private void FillProcedureAdjustments() {
			if(_proc.ListProcAdjustments.Count==0) {
				gridProcedureAdjustments.Title="Procedure Adjustments (None Reported)";
			}
			else {
				gridProcedureAdjustments.Title="Procedure Adjustments";
			}
			gridProcedureAdjustments.BeginUpdate();
			gridProcedureAdjustments.Columns.Clear();
			const int colWidthDescription=200;
			const int colWidthAdjAmt=80;
			int colWidthVariable=gridProcedureAdjustments.Width-10-colWidthDescription-colWidthAdjAmt;
			gridProcedureAdjustments.Columns.Add(new UI.ODGridColumn("Description",colWidthDescription,HorizontalAlignment.Left));
			gridProcedureAdjustments.Columns.Add(new UI.ODGridColumn("Reason",colWidthVariable,HorizontalAlignment.Left));
			gridProcedureAdjustments.Columns.Add(new UI.ODGridColumn("AdjAmt",colWidthAdjAmt,HorizontalAlignment.Right));
			gridProcedureAdjustments.Rows.Clear();
			_patRespSum=0;
			_contractualObligationSum=0;
			_payorInitiatedReductionSum=0;
			_otherAdjustmentSum=0;
			for(int i=0;i<_proc.ListProcAdjustments.Count;i++) {
				Hx835_Adj adj=_proc.ListProcAdjustments[i];
				ODGridRow row=new ODGridRow();
				row.Tag=adj;
				row.Cells.Add(new ODGridCell(adj.AdjustRemarks));//Remarks
				row.Cells.Add(new ODGridCell(adj.ReasonDescript));//Reason
				row.Cells.Add(new ODGridCell(adj.AdjAmt.ToString("f2")));//AdjAmt
				if(adj.AdjCode=="PR") {//Patient Responsibility
					_patRespSum+=adj.AdjAmt;
				}
				else if(adj.AdjCode=="CO") {//Contractual Obligations
					_contractualObligationSum+=adj.AdjAmt;
				}
				else if(adj.AdjCode=="PI") {//Payor Initiated Reductions
					_payorInitiatedReductionSum+=adj.AdjAmt;
				}
				else {//Other Adjustments
					_otherAdjustmentSum+=adj.AdjAmt;
				}
				gridProcedureAdjustments.Rows.Add(row);
			}
			gridProcedureAdjustments.EndUpdate();
			textPatRespSum.Text=_patRespSum.ToString("f2");
			textContractualObligSum.Text=_contractualObligationSum.ToString("f2");
			textPayorReductionSum.Text=_payorInitiatedReductionSum.ToString("f2");
			textOtherAdjustmentSum.Text=_otherAdjustmentSum.ToString("f2");
		}

		private void FillRemarks() {
			if(_proc.ListRemarks.Count==0) {
				gridRemarks.Title="Remarks (None Reported)";
			}
			else {
				gridRemarks.Title="Remarks";
			}
			gridRemarks.BeginUpdate();
			gridRemarks.Columns.Clear();
			gridRemarks.Columns.Add(new UI.ODGridColumn("",gridRemarks.Width,HorizontalAlignment.Left));
			gridRemarks.Rows.Clear();
			for(int i=0;i<_proc.ListRemarks.Count;i++) {
				ODGridRow row=new ODGridRow();
				row.Tag=_proc.ListRemarks[i].Value;
				row.Cells.Add(new UI.ODGridCell(_proc.ListRemarks[i].Value));
				gridRemarks.Rows.Add(row);
			}
			gridRemarks.EndUpdate();
		}

		private void FillSupplementalInfo() {
			if(_proc.ListSupplementalInfo.Count==0) {
				gridSupplementalInfo.Title="Supplemental Info (None Reported)";
			}
			else {
				gridSupplementalInfo.Title="Supplemental Info";
			}
			gridSupplementalInfo.BeginUpdate();
			gridSupplementalInfo.Columns.Clear();
			const int colWidthAmt=80;
			int colWidthVariable=gridSupplementalInfo.Width-10-colWidthAmt;
			gridSupplementalInfo.Columns.Add(new ODGridColumn("Description",colWidthVariable,HorizontalAlignment.Left));
			gridSupplementalInfo.Columns.Add(new ODGridColumn("Amt",colWidthAmt,HorizontalAlignment.Right));
			gridSupplementalInfo.Rows.Clear();
			for(int i=0;i<_proc.ListSupplementalInfo.Count;i++) {
				Hx835_Info info=_proc.ListSupplementalInfo[i];
				ODGridRow row=new ODGridRow();
				row.Tag=info;
				row.Cells.Add(info.FieldName);//Description
				row.Cells.Add(info.FieldValue);//Amount
				gridSupplementalInfo.Rows.Add(row);
			}
			gridSupplementalInfo.EndUpdate();
		}

		private void gridProcedureAdjustments_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Hx835_Adj adj=(Hx835_Adj)gridProcedureAdjustments.Rows[e.Row].Tag;
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(adj.AdjCode+" "+adj.AdjustRemarks+"\r\r"+adj.ReasonDescript+"\r\n"+adj.AdjAmt.ToString("f2"));
			msgbox.Show(this);//This window is just used to display information.
		}

		private void gridRemarks_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			string remark=(string)gridRemarks.Rows[e.Row].Tag;
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(remark);
			msgbox.Show(this);//This window is just used to display information.
		}

		private void gridSupplementalInfo_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Hx835_Info info=(Hx835_Info)gridSupplementalInfo.Rows[e.Row].Tag;
			MsgBoxCopyPaste msgbox=new MsgBoxCopyPaste(info.FieldName+"\r\n"+info.FieldValue);
			msgbox.Show(this);//This window is just used to display information.
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
			Close();
		}
		
	}
}