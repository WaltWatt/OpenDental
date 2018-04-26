using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormPopupAudit:ODForm {
		public Popup PopupCur;
		public Patient PatCur;
		///<summary>All archived popups for the current popup</summary>
		private List<Popup> ListPopupAud;

		public FormPopupAudit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormPopupAudit_Load(object sender,System.EventArgs e) {
			FillGrid();
		}

		private void FillGrid() {//The grid cannot be changed to be sortable. This audit relies on the oldest popup changes being located at the top
			ListPopupAud=Popups.GetArchivesForPopup(PopupCur.PopupNum);
			gridPopupAudit.BeginUpdate();
			gridPopupAudit.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TablePopupsForFamily","Create Date"),140);
			gridPopupAudit.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePopupsForFamily","Edit Date"),140);
			gridPopupAudit.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePopupsForFamily","Level"),80);
			gridPopupAudit.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePopupsForFamily","Disabled"),60,HorizontalAlignment.Center);
			gridPopupAudit.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePopupsForFamily","Popup Message"),100);
			gridPopupAudit.Columns.Add(col);
			gridPopupAudit.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<ListPopupAud.Count;i++) {
				row=new ODGridRow();
				if(PopupCur.DateTimeEntry.Year<1880) {
					row.Cells.Add("");
				}
				else {
					row.Cells.Add(PopupCur.DateTimeEntry.ToString());
				}
				if(i==0) {
					row.Cells.Add("");//Very first pop up entry.  No edit date.
				}
				else {
					row.Cells.Add(ListPopupAud[i-1].DateTimeEntry.ToString());//Gets the previous DateTimeEntry to show as the last edit date.
				}
				row.Cells.Add(Lan.g("enumEnumPopupLevel",ListPopupAud[i].PopupLevel.ToString()));
				row.Cells.Add(ListPopupAud[i].IsDisabled?"X":"");
				row.Cells.Add(ListPopupAud[i].Description);
				gridPopupAudit.Rows.Add(row);
			}
			gridPopupAudit.EndUpdate();
		}

		private void gridPopupAudit_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormPopupEdit FormPE=new FormPopupEdit();
			FormPE.PopupAudit=PopupCur;
			DateTime dateLastEdit=DateTime.MinValue;
			if(e.Row > 0 && ListPopupAud[e.Row-1].DateTimeEntry.Year > 1880) {
				FormPE.DateLastEdit=ListPopupAud[e.Row-1].DateTimeEntry;
			}
			FormPE.PopupCur=ListPopupAud[e.Row];
			FormPE.ShowDialog();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}