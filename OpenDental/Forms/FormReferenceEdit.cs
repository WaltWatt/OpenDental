using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormReferenceEdit:ODForm {
		private CustReference RefCur;
		private List<CustRefEntry> RefEntryList;

		public FormReferenceEdit(CustReference refCur) {
			InitializeComponent();
			Lan.F(this);
			RefCur=refCur;
		}

		private void FormReferenceEdit_Load(object sender,EventArgs e) {
			textName.Text=CustReferences.GetCustNameFL(RefCur.PatNum);
			textNote.Text=RefCur.Note;
			checkBadRef.Checked=RefCur.IsBadRef;
			if(RefCur.DateMostRecent.Year>1880) {
				textRecentDate.Text=RefCur.DateMostRecent.ToShortDateString();
			}
			FillMain();
		}

		private void FillMain() {
			RefEntryList=CustRefEntries.GetEntryListForReference(RefCur.PatNum);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn("PatNum",65);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Last Name",120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("First Name",120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Date Entry",0);
			col.TextAlign=HorizontalAlignment.Center;
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<RefEntryList.Count;i++) {
				row=new ODGridRow();
				Patient pat=Patients.GetLim(RefEntryList[i].PatNumCust);
				row.Cells.Add(pat.PatNum.ToString());
				row.Cells.Add(pat.LName);
				row.Cells.Add(pat.FName);
				row.Cells.Add(RefEntryList[i].DateEntry.ToShortDateString());
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,UI.ODGridClickEventArgs e) {
			FormReferenceEntryEdit FormREE=new FormReferenceEntryEdit(RefEntryList[e.Row]);
			FormREE.ShowDialog();
			FillMain();
		}

		private void butToday_Click(object sender,EventArgs e) {
			textRecentDate.Text=DateTime.Now.ToShortDateString();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(textRecentDate.errorProvider1.GetError(textRecentDate)!="") {
				MsgBox.Show(this,"Please enter a valid date.");
			}
			RefCur.DateMostRecent=PIn.Date(textRecentDate.Text);
			RefCur.IsBadRef=checkBadRef.Checked;
			RefCur.Note=textNote.Text;
			CustReferences.Update(RefCur);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}