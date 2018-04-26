using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormSheetTools:ODForm {
		public bool HasSheetsChanged=false;//Whether or not sheets have been edited/added/deleted from the DB from within this form.
		public long ImportedSheetDefNum=0;//The primary key of the last sheet that was imported.

		public FormSheetTools() {
			InitializeComponent();
			Lan.F(this);
		}

		private void butImport_Click(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			OpenFileDialog openDlg=new OpenFileDialog();
			string initDir=PrefC.GetString(PrefName.ExportPath);
			if(Directory.Exists(initDir)) {
				openDlg.InitialDirectory=initDir;
			}
			if(openDlg.ShowDialog()!=DialogResult.OK) {
				Cursor=Cursors.Default;
				return;
			}
			try {
				//ImportCustomSheetDef(openDlg.FileName);
				SheetDef sheetdef=new SheetDef();
				XmlSerializer serializer=new XmlSerializer(typeof(SheetDef));
				if(openDlg.FileName!="") {
					if(!File.Exists(openDlg.FileName)) {
						throw new ApplicationException(Lan.g("FormSheetDefs","File does not exist."));
					}
					try {
						using(TextReader reader=new StreamReader(openDlg.FileName)) {
							sheetdef=(SheetDef)serializer.Deserialize(reader);
						}
					}
					catch {
						throw new ApplicationException(Lan.g("FormSheetDefs","Invalid file format"));
					}
				}
				sheetdef.IsNew=true;
				//Users might be importing a sheet that was developed in an older version that does not support ItemColor.  Default them to black if necessary.
				for(int i=0;i<sheetdef.SheetFieldDefs.Count;i++) {
					//Static text, lines, and rectangles are the only field types that support ItemColor.
					if(sheetdef.SheetFieldDefs[i].FieldType!=SheetFieldType.StaticText
						&& sheetdef.SheetFieldDefs[i].FieldType!=SheetFieldType.Line
						&& sheetdef.SheetFieldDefs[i].FieldType!=SheetFieldType.Rectangle) {
						continue;
					}
					//ItemColor will be set to "Empty" if this is a sheet that was exported from a previous version that didn't support ItemColor.
					//Color.Empty will actually draw but will be 'invisible' to the user.  For this reason, we considered this a bug and defaulted the color to black.
					if(sheetdef.SheetFieldDefs[i].ItemColor==Color.Empty) {
						sheetdef.SheetFieldDefs[i].ItemColor=Color.Black;//Old sheet behavior was to always draw these field types in black.
					}
				}
				SheetDefs.InsertOrUpdate(sheetdef);
				HasSheetsChanged=true;//Flag as true so we know to refresh the grid in FormSheetDefs.cs
				ImportedSheetDefNum=sheetdef.SheetDefNum;//Set this so when we return to FormSheetDefs.cs we can select that row.
			}
			catch(ApplicationException ex) {
				Cursor=Cursors.Default;
				MessageBox.Show(ex.Message);
				return;
			}
			Cursor=Cursors.Default;
			MsgBox.Show(this,"Imported.");
		}

		private void butExport_Click(object sender,EventArgs e) {
			FormSheetExport formSE=new FormSheetExport();
			formSE.ShowDialog();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}

	}
}