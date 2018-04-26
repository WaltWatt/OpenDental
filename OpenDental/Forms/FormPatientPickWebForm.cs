using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using OpenDental.WebSheets;

namespace OpenDental {
	public partial class FormPatientPickWebForm:ODForm {
		private List<Patient> listPats;
		///<summary>If OK.  Can be zero to indicate create new patient.  A result of Cancel indicates quit importing altogether.</summary>
		public long SelectedPatNum;
		public string LnameEntered;
		public string FnameEntered;
		public DateTime BdateEntered;
		///<summary>If true, more than one patient matches FName, LName, and Birthdate. If false, no matches have been found.</summary>
		public bool HasMoreThanOneMatch;
		private SheetAndSheetField _sheetAndSheetField;

		public FormPatientPickWebForm(SheetAndSheetField sheetAndSheetField) {
			InitializeComponent();
			Lan.F(this);
			_sheetAndSheetField=sheetAndSheetField;
		}

		private void FormPatientPickWebForm_Load(object sender,EventArgs e) {
			textLName.Text=LnameEntered;
			textFName.Text=FnameEntered;
			textBirthdate.Text=BdateEntered.ToShortDateString();
			if(HasMoreThanOneMatch) {
				labelExplanation.Text=Lan.g(this,"More than one matching patient was found for this submitted web form.");
			}
			FillGrid();
		}

		private void FillGrid(){
			gridMain.BeginUpdate();
			if(HasMoreThanOneMatch) {
				gridMain.Title=Lan.g(this,"Matches");
			}
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Last Name"),110);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"First Name"),110);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Birthdate"),110);
			gridMain.Columns.Add(col);
			if(PrefC.HasClinicsEnabled) {
				col=new ODGridColumn(Lan.g(this,"Clinic Name"),110);
				gridMain.Columns.Add(col);
			}
			listPats=Patients.GetSimilarList(LnameEntered,FnameEntered,BdateEntered);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<listPats.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(listPats[i].LName);
				row.Cells.Add(listPats[i].FName);
				row.Cells.Add(listPats[i].Birthdate.ToShortDateString());
				if(PrefC.HasClinicsEnabled) {
					string clinicName=Clinics.GetDesc(listPats[i].ClinicNum);
					row.Cells.Add(!string.IsNullOrWhiteSpace(clinicName) ? clinicName : "N/A");
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butView_Click(object sender,EventArgs e) {
			Sheet sheet=SheetUtil.CreateSheetFromWebSheet(0,_sheetAndSheetField);
			FormSheetFillEdit.ShowForm(sheet,isReadOnly:true);
		}

		private void gridMain_CellDoubleClick(object sender,UI.ODGridClickEventArgs e) {
			SelectedPatNum=listPats[e.Row].PatNum;
			//Security log for patient select.
			Patient pat=Patients.GetPat(SelectedPatNum);
			SecurityLogs.MakeLogEntry(Permissions.SheetEdit,SelectedPatNum,"In the 'Pick Patient for Web Form', this user double clicked a name in the suggested list.  "
				+"This caused the web form for this patient: "+LnameEntered+", "+FnameEntered+" "+BdateEntered.ToShortDateString()+"  "
				+"to be manually attached to this other patient: "+pat.LName+", "+pat.FName+" "+pat.Birthdate.ToShortDateString());
			DialogResult=DialogResult.OK;
		}

		private void butSelect_Click(object sender,EventArgs e) {
			FormPatientSelect FormPs=new FormPatientSelect();
			FormPs.SelectionModeOnly=true;
			FormPs.ShowDialog();
			if(FormPs.DialogResult!=DialogResult.OK) {
				return;
			}
			SelectedPatNum=FormPs.SelectedPatNum;
			//Security log for patient select.
			Patient pat=Patients.GetPat(SelectedPatNum);
			SecurityLogs.MakeLogEntry(Permissions.SheetEdit,SelectedPatNum,"In the 'Pick Patient for Web Form', this user clicked the 'Select' button.  "
				+"By clicking the 'Select' button, the web form for this patient: "+LnameEntered+", "+FnameEntered+" "+BdateEntered.ToShortDateString()+"  "
				+"was manually attached to this other patient: "+pat.LName+", "+pat.FName+" "+pat.Birthdate.ToShortDateString());
			DialogResult=DialogResult.OK;
		}

		private void butNew_Click(object sender,EventArgs e) {
			SelectedPatNum=0;
			DialogResult=DialogResult.OK;
		}

		private void butSkip_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Ignore;
		}		

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
				
	}
}