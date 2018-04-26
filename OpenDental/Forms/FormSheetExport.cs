using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using OpenDental.UI;
using OpenDentBusiness;


namespace OpenDental {
	public partial class FormSheetExport:ODForm {
		private List<SheetDef> _listSheetDefs;

		public FormSheetExport() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormSheetExport_Load(object sender,EventArgs e) {
			FillGridCustomSheet();
		}

		private void FillGridCustomSheet() {
			SheetDefs.RefreshCache();
			SheetFieldDefs.RefreshCache();
			_listSheetDefs=SheetDefs.GetDeepCopy(false);
			gridCustomSheet.BeginUpdate();
			gridCustomSheet.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableSheetDef","Description"),170);
			gridCustomSheet.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableSheetDef","Type"),100);
			gridCustomSheet.Columns.Add(col);
			gridCustomSheet.Rows.Clear();
			ODGridRow row;
			foreach(SheetDef sheetDef in _listSheetDefs) {
				row=new ODGridRow();
				row.Cells.Add(sheetDef.Description);
				row.Cells.Add(sheetDef.SheetType.ToString());
				gridCustomSheet.Rows.Add(row);
			}
			gridCustomSheet.EndUpdate();
		}

		private void butExport_Click(object sender,EventArgs e) {
			if(gridCustomSheet.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select a sheet from the list first.");
				return;
			}
			SheetDef sheetdef=SheetDefs.GetSheetDef(_listSheetDefs[gridCustomSheet.GetSelectedIndex()].SheetDefNum);
			SaveFileDialog saveDlg=new SaveFileDialog();
			string filename="SheetDefCustom.xml";
			saveDlg.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
			saveDlg.FileName=filename;
			if(saveDlg.ShowDialog()!=DialogResult.OK) {
				return;
			}
			XmlSerializer serializer=new XmlSerializer(typeof(SheetDef));
			using(TextWriter writer=new StreamWriter(saveDlg.FileName)) {
				serializer.Serialize(writer,sheetdef);
			}
			MsgBox.Show(this,"Exported");
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

	}
}