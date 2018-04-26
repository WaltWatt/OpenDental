using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Drawing.Printing;

namespace OpenDental {
	public partial class FormRxManage:ODForm {
		private UI.ODGrid gridMain;
		private OpenDental.UI.Button butPrintSelected;
		private OpenDental.UI.Button butClose;
		private UI.Button butNewRx;
		private Label labelECWerror;
		private Patient _patCur;
		private List<RxPat> _listRx;

		public FormRxManage(Patient patCur) {
			InitializeComponent();
			_patCur=patCur;
			Lan.F(this);
		}

		private void FormRxManage_Load(object sender,System.EventArgs e) {
			FillGrid();
		}

		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableRxManage","Date"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRxManage","Drug"),140);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRxManage","Sig"),0);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRxManage","Disp"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRxManage","Refills"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRxManage","Provider"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRxManage","Notes"),0);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableRxManage","Missing Info"),0);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			_listRx=RxPats.GetAllForPat(_patCur.PatNum);
			_listRx.Sort(SortByRxDate);
			ODGridRow row;
			for(int i = 0;i<_listRx.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listRx[i].RxDate.ToShortDateString());
				row.Cells.Add(_listRx[i].Drug);
				row.Cells.Add(_listRx[i].Sig);
				row.Cells.Add(_listRx[i].Disp);
				row.Cells.Add(_listRx[i].Refills);
				row.Cells.Add(Providers.GetAbbr(_listRx[i].ProvNum));
				row.Cells.Add(_listRx[i].Notes);
				row.Cells.Add(SheetPrinting.ValidateRxForSheet(_listRx[i]));
				row.Tag=_listRx[i].Copy();
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		///<summary>Sorts the passed in RxPats by RxDate and then RxNum.</summary>
		private int SortByRxDate(RxPat rx1,RxPat rx2) {
			if(rx1.RxDate!=rx2.RxDate) {
				return rx2.RxDate.CompareTo(rx1.RxDate);
			}
			return rx2.RxNum.CompareTo(rx1.RxNum);
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				//this should never happen
				return;
			}
			RxPat rx=_listRx[gridMain.GetSelectedIndex()];
			FormRxEdit FormRxE=new FormRxEdit(_patCur,rx);
			FormRxE.ShowDialog();
			if(FormRxE.DialogResult!=DialogResult.OK) {
				return;
			}
			FillGrid();
		}

		///<summary>Prints the selected rx's. If one rx is selected, uses single rx sheet. If more than one is selected, uses multirx sheet</summary>
		private void butPrintSelect_Click(object sender,EventArgs e) {
			List<RxPat> listSelectRx=new List<RxPat>();
			SheetDef sheetDef;
			Sheet sheet;
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				listSelectRx.Add(_listRx[gridMain.SelectedIndices[i]]);
			}
			if(listSelectRx.Count==0) {
				MsgBox.Show(this,"At least one prescription must be selected");
				return;
			}
			if(PrinterSettings.InstalledPrinters.Count==0) {
				MsgBox.Show(this,"Error: No Printers Installed\r\n"+
									"If you do have a printer installed, restarting the workstation may solve the problem."
				);
				return;
			}
			if(listSelectRx.Count==1) {//old way of printing one rx
				//This logic is an exact copy of FormRxEdit.butPrint_Click()'s logic.  If this is updated, that method needs to be updated as well.
				sheetDef=SheetDefs.GetSheetsDefault(SheetTypeEnum.Rx,Clinics.ClinicNum);
				sheet=SheetUtil.CreateSheet(sheetDef,_patCur.PatNum);
				SheetParameter.SetParameter(sheet,"RxNum",listSelectRx[0].RxNum);
				SheetFiller.FillFields(sheet);
				SheetUtil.CalculateHeights(sheet);
				SheetPrinting.PrintRx(sheet,listSelectRx[0]);
			}
			else { //multiple rx selected
				//Print batch list of rx
				SheetPrinting.PrintMultiRx(listSelectRx);
			}
		}

		private void butNewRx_Click(object sender,EventArgs e) {
			//This code is a copy of ContrChart.Tool_Rx_Click().  Any changes to this code need to be changed there too.
			if(!Security.IsAuthorized(Permissions.RxCreate)) {
				return;
			}
			if(Programs.UsingEcwTightOrFullMode() && Bridges.ECW.UserId!=0) {
				VBbridges.Ecw.LoadRxForm((int)Bridges.ECW.UserId,Bridges.ECW.EcwConfigPath,(int)Bridges.ECW.AptNum);
				//refresh the right panel:
				try {
					string strAppServer=VBbridges.Ecw.GetAppServer((int)Bridges.ECW.UserId,Bridges.ECW.EcwConfigPath);
					labelECWerror.Visible=false;
				}
				catch(Exception ex) {
					labelECWerror.Text="Error: "+ex.Message;
					labelECWerror.Visible=true;
				}
			}
			else {
				FormRxSelect FormRS=new FormRxSelect(_patCur);
				FormRS.ShowDialog();
				if(FormRS.DialogResult!=DialogResult.OK) {
					return;
				}
				SecurityLogs.MakeLogEntry(Permissions.RxCreate,_patCur.PatNum,"Created prescription.");
			}
			FillGrid();
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

	}
}