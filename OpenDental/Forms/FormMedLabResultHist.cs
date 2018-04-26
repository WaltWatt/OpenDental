using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormMedLabResultHist:ODForm {
		public Patient PatCur;
		public MedLabResult ResultCur;

		public FormMedLabResultHist() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormMedLabResultHist_Load(object sender, EventArgs e) {
			FillGridResultHist();
		}

		private void FillGridResultHist() {
			MedLab medLabCur=MedLabs.GetOne(ResultCur.MedLabNum);
			if(medLabCur==null) {//should never happen, but we must have a MedLab object to fill the grid
				return;
			}
			gridResultHist.BeginUpdate();
			gridResultHist.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn("Result Description / Value",425);
			gridResultHist.Columns.Add(col);
			col=new ODGridColumn("Flag",110);
			gridResultHist.Columns.Add(col);
			col=new ODGridColumn("Units",65);
			gridResultHist.Columns.Add(col);
			col=new ODGridColumn("Date/Time Reported",130);//OBR-22, Date/Time Observations Reported
			col.SortingStrategy=GridSortingStrategy.DateParse;
			gridResultHist.Columns.Add(col);
			col=new ODGridColumn("Date/Time Observed",130);//OBX-11, Date/Time of Observation
			col.SortingStrategy=GridSortingStrategy.DateParse;
			gridResultHist.Columns.Add(col);
			col=new ODGridColumn("Status",70);
			gridResultHist.Columns.Add(col);
			gridResultHist.Rows.Clear();
			ODGridRow row;
			long patNum=0;
			if(PatCur!=null) {
				patNum=PatCur.PatNum;
			}
			List<MedLabResult> listResults=MedLabResults.GetResultHist(ResultCur,patNum,medLabCur.SpecimenID,medLabCur.SpecimenIDFiller);
			for(int i=0;i<listResults.Count;i++) {
				row=new ODGridRow();
				string obsVal=listResults[i].ObsText;
				if(listResults[i].ObsValue!="Test Not Performed") {
					obsVal+="\r\n    "+listResults[i].ObsValue.Replace("\r\n","\r\n    ");
				}
				if(listResults[i].Note!="") {
					obsVal+="\r\n    "+listResults[i].Note.Replace("\r\n","\r\n    ");
				}
				row.Cells.Add(obsVal);
				row.Cells.Add(MedLabResults.GetAbnormalFlagDescript(listResults[i].AbnormalFlag));
				row.Cells.Add(listResults[i].ObsUnits);
				medLabCur=MedLabs.GetOne(listResults[i].MedLabNum);
				string dateReported="";
				if(medLabCur!=null) {
					dateReported=medLabCur.DateTimeReported.ToString("MM/dd/yyyy hh:mm tt");//DT format matches LabCorp examples (US only company)
				}
				row.Cells.Add(dateReported);
				row.Cells.Add(listResults[i].DateTimeObs.ToString("MM/dd/yyyy hh:mm tt"));//DT format matches LabCorp examples (US only company)
				row.Cells.Add(MedLabs.GetStatusDescript(listResults[i].ResultStatus));
				gridResultHist.Rows.Add(row);
			}
			gridResultHist.EndUpdate();
		}

		private void butClose_Click(object sender,EventArgs e) {
			this.Close();
		}
		
	}
}
