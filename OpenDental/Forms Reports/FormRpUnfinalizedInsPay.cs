using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;
using OpenDental.UI;
using System.Drawing.Printing;
using System.IO;
using CodeBase;

namespace OpenDental {
	public partial class FormRpUnfinalizedInsPay:ODForm {
		private bool _headingPrinted;
		private int _pagesPrinted;
		private int _headingPrintH;

		private List<RpUnfinalizedInsPay.UnfinalizedInsPay> _listUnfinalizedInsPay;
		private List<Clinic> _listClinics;
		private List<string> _listTypes;

		public FormRpUnfinalizedInsPay() {
			InitializeComponent();
			Lan.F(this);
    }

    private void FormRpInsPayPlansPastDue_Load(object sender,EventArgs e) {
      FillType();
      if(PrefC.HasClinicsEnabled) {
        comboBoxMultiClinics.Visible=true;
        labelClinic.Visible=true;
        FillClinics();
      }
      if(!LoadData()) {
        DialogResult=DialogResult.Cancel;
        return;
			}
			FillGrid();
    }

    private void FillType() {
			_listTypes=new List<string>();
			comboBoxMultiType.Items.Add(Lan.g(this,"All"));
			_listTypes.Add("All");
			comboBoxMultiType.Items.Add(Lan.g(this,"Partial Payment"));
			_listTypes.Add("PartialPayment");
			comboBoxMultiType.Items.Add(Lan.g(this,"Unfinalized Claim"));
			_listTypes.Add("UnfinalizedClaim");
			comboBoxMultiType.SetSelected(0,true);
		}

		private void FillClinics() {
			_listClinics=Clinics.GetForUserod(Security.CurUser);
			comboBoxMultiClinics.Items.Add(Lan.g(this,"All"));
			comboBoxMultiClinics.SetSelected(0,true);
			if(!Security.CurUser.ClinicIsRestricted) {
				comboBoxMultiClinics.Items.Add(Lan.g(this,"Unassigned"));
			}
			for(int i=0;i<_listClinics.Count;i++) {
				comboBoxMultiClinics.Items.Add(_listClinics[i].Abbr);				
			}
		}

		///<summary>Retrieves data and uses them to create new UnfinalizedInsPay objects.
		///Heavy lifting done here once upon load.  This also gets called if the user clicks "Refresh Data".</summary>
		private bool LoadData() {
			_listUnfinalizedInsPay=RpUnfinalizedInsPay.GetUnfinalizedInsPay(textCarrier.Text,PrefC.GetDate(PrefName.ClaimPaymentNoShowZeroDate));		
      return true;
		}

		///<summary>Actually fill the grid with the data. Filtering based on the user-defined criteria gets done here.</summary>
		private void FillGrid() {
			//get the user-entered filter values.
			//carrier filter is already taken care of in LoadData()
			List<string> listType=new List<string>();
			if(comboBoxMultiType.ListSelectedIndices.Contains(0)) {
				listType.AddRange(_listTypes);
			}
			else {
				for(int i=0;i<comboBoxMultiType.SelectedIndices.Count;i++) {
					listType.Add(_listTypes[(int)comboBoxMultiType.SelectedIndices[i]].ToString());
				}
			}
			List<long> listClinicNums=new List<long>();
			if(PrefC.HasClinicsEnabled) {
				if(comboBoxMultiClinics.ListSelectedIndices.Contains(0)) {//'All' is selected
					listClinicNums.AddRange(_listClinics.Select(x => x.ClinicNum));//Add all clinics this person has access to.
					if(!Security.CurUser.ClinicIsRestricted) {
						listClinicNums.Add(0);
					}
				}
				else {
					for(int i=0;i<comboBoxMultiClinics.ListSelectedIndices.Count;i++) {
						if(Security.CurUser.ClinicIsRestricted) {
							listClinicNums.Add(_listClinics[comboBoxMultiClinics.ListSelectedIndices[i]-1].ClinicNum);//Minus 1 for 'All'
						}
						else if(comboBoxMultiClinics.ListSelectedIndices[i]==1) {//'Unassigned' selected
							listClinicNums.Add(0);
						}
						else {
							listClinicNums.Add(_listClinics[comboBoxMultiClinics.ListSelectedIndices[i]-2].ClinicNum);//Minus 2 for 'All' and 'Unassigned'
						}
					}
				}
			}
			//fill the grid
			gridMain.BeginUpdate();
			//columns
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableUnfinalizedInsPay","Type"),120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableUnfinalizedInsPay","Patient"),200);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableUnfinalizedInsPay","Carrier"),200);
			gridMain.Columns.Add(col);
			if(PrefC.HasClinicsEnabled) {
				col=new ODGridColumn(Lan.g("TableUnfinalizedInsPay","Clinic"),160);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g("TableUnfinalizedInsPay","Date"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableUnfinalizedInsPay","Date of Service"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableUnfinalizedInsPay","Amount"),90);
			gridMain.Columns.Add(col);
			//rows
			gridMain.Rows.Clear();
			ODGridRow row;
			foreach(RpUnfinalizedInsPay.UnfinalizedInsPay unfinalCur in _listUnfinalizedInsPay) {
				if(!listType.Contains("All") && !listType.Contains(unfinalCur.Type.ToString())) {
					continue;
				}
				else {
					//no filter.
				}
				if(PrefC.HasClinicsEnabled && (!listClinicNums.Contains(unfinalCur.ClinicCur.ClinicNum))) {
					continue;
				}
				row=new ODGridRow();
				string patName=unfinalCur.PatientCur.GetNameLFnoPref();
				if(unfinalCur.CountPats > 1) {
					patName+=" "+Lan.g(this,"(multiple)");
				}
				string carrierName=unfinalCur.CarrierCur.CarrierName;
				row.Cells.Add(Lan.g(this,unfinalCur.Type.GetDescription()));
				row.Cells.Add(patName);
				row.Cells.Add(carrierName);
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(unfinalCur.ClinicCur.Description);
				}
				row.Cells.Add(unfinalCur.Date.ToShortDateString());
				row.Cells.Add((unfinalCur.DateOfService.Year < 1880)?"": unfinalCur.DateOfService.ToShortDateString());
				row.Cells.Add(unfinalCur.Amount.ToString("f"));
				row.Tag=unfinalCur;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void comboBoxMulti_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void gridMain_MouseUp(object sender,MouseEventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				contextMenuStrip1.Hide();
				return;
			}
			if(e.Button==MouseButtons.Right) {
				RpUnfinalizedInsPay.UnfinalizedInsPay unfinalPay=(RpUnfinalizedInsPay.UnfinalizedInsPay)gridMain.Rows[gridMain.SelectedIndices[0]].Tag;
				switch(unfinalPay.Type) {
					case RpUnfinalizedInsPay.UnfinalizedInsPay.UnfinalizedPaymentType.PartialPayment:
						goToAccountToolStripMenuItem.Visible=unfinalPay.CountPats!=0;
						openClaimToolStripMenuItem.Visible=false;
						createCheckToolStripMenuItem.Visible=false;
						openEOBToolStripMenuItem.Visible=true;
						deleteEOBToolStripMenuItem.Visible=true;
						break;
					case RpUnfinalizedInsPay.UnfinalizedInsPay.UnfinalizedPaymentType.UnfinalizedPayment:
						openEOBToolStripMenuItem.Visible=false;
						deleteEOBToolStripMenuItem.Visible=false;
						goToAccountToolStripMenuItem.Visible=true;
						openClaimToolStripMenuItem.Visible=true;
						createCheckToolStripMenuItem.Visible=true;
						break;
					default://should never happen.
						goToAccountToolStripMenuItem.Visible=false;
						openClaimToolStripMenuItem.Visible=false;
						createCheckToolStripMenuItem.Visible=false;
						openEOBToolStripMenuItem.Visible=false;
						deleteEOBToolStripMenuItem.Visible=false;
						break;
				}
			}
		}

		/// <summary>Creates a check for the claim selected. Copied logic from FormClaimEdit.cs</summary>
		private void createCheckToolStripMenuItem_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.InsPayCreate)) {//date not checked here, but it will be checked when saving the check to prevent backdating
				return;
			}
			if(PrefC.GetBool(PrefName.ClaimPaymentBatchOnly)) {
				//Is there a permission in the manage module that would block this behavior? Are we sending the user into a TRAP?!
				MsgBox.Show(this,"Please use Batch Insurance in Manage Module to Finalize Payments.");
				return;
			}
			RpUnfinalizedInsPay.UnfinalizedInsPay unfinalPay=(RpUnfinalizedInsPay.UnfinalizedInsPay)gridMain.Rows[gridMain.SelectedIndices[0]].Tag;
			if(unfinalPay.ClaimCur==null) {
				MsgBox.Show(this,"Unable to find claim for this partial payment.");
				return;
			}
			List<ClaimProc> listClaimProcForClaim=ClaimProcs.RefreshForClaim(unfinalPay.ClaimCur.ClaimNum);
			if(!listClaimProcForClaim.Any(x => x.Status.In(ClaimProcStatus.Received,ClaimProcStatus.Supplemental))) {
				MessageBox.Show(Lan.g(this,"There are no valid received payments for this claim."));
				return;
			}
			ClaimPayment claimPayment=new ClaimPayment();
			claimPayment.CheckDate=MiscData.GetNowDateTime().Date;//Today's date for easier tracking by the office and to avoid backdating before accounting lock dates.
			claimPayment.IsPartial=true;
			claimPayment.ClinicNum=unfinalPay.ClinicCur.ClinicNum;
			Family famCur=Patients.GetFamily(unfinalPay.PatientCur.PatNum);
			List<InsSub> listInsSub=InsSubs.RefreshForFam(famCur);
			List<InsPlan> listInsPlan=InsPlans.RefreshForSubList(listInsSub);
			claimPayment.CarrierName=Carriers.GetName(InsPlans.GetPlan(unfinalPay.ClaimCur.PlanNum,listInsPlan).CarrierNum);
			ClaimPayments.Insert(claimPayment);
			double amt=ClaimProcs.AttachAllOutstandingToPayment(claimPayment.ClaimPaymentNum,PrefC.GetDate(PrefName.ClaimPaymentNoShowZeroDate));
			claimPayment.CheckAmt=amt;
			ClaimPayments.Update(claimPayment);
			FormClaimEdit.FormFinalizePaymentHelper(claimPayment,unfinalPay.ClaimCur,unfinalPay.PatientCur,famCur);
			LoadData();
			FillGrid();
		}

		/// <summary>Opens the current selected claim.</summary>
		private void openClaimToolStripMenuItem_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.ClaimView)) {
				return;
			}
			RpUnfinalizedInsPay.UnfinalizedInsPay unfinalPay=(RpUnfinalizedInsPay.UnfinalizedInsPay)gridMain.Rows[gridMain.SelectedIndices[0]].Tag;
			if(unfinalPay.ClaimCur==null) {
				MsgBox.Show(this,"This claim has been deleted.");
				return;
			}
			Family famCur=Patients.GetFamily(unfinalPay.PatientCur.PatNum);
			FormClaimEdit FormCE=new FormClaimEdit(unfinalPay.ClaimCur,unfinalPay.PatientCur,famCur);
			FormCE.ShowDialog();
			LoadData();
			FillGrid();
		}

		/// <summary>Go to the selected patient's account.</summary>
		private void goToAccountToolStripMenuItem_Click(object sender,EventArgs e) {
			RpUnfinalizedInsPay.UnfinalizedInsPay unfinalPay=(RpUnfinalizedInsPay.UnfinalizedInsPay)gridMain.Rows[gridMain.SelectedIndices[0]].Tag;
			GotoModule.GotoAccount(unfinalPay.PatientCur.PatNum);
		}

		/// <summary>Opens the selected insurance payment.</summary>
		private void openEOBToolStripMenuItem_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.InsPayCreate)) {
				return;
			}
			RpUnfinalizedInsPay.UnfinalizedInsPay unfinalPay=(RpUnfinalizedInsPay.UnfinalizedInsPay)gridMain.Rows[gridMain.SelectedIndices[0]].Tag;
			if(unfinalPay.ClaimPaymentCur==null) {
				MsgBox.Show(this,"This claim payment has been deleted.");
				return;
			}
			FormClaimPayBatch FormCPB=new FormClaimPayBatch(unfinalPay.ClaimPaymentCur);
			FormCPB.ShowDialog();
			LoadData();
			FillGrid();
		}

		/// <summary>Deletes the selected insurance payment selected.</summary>
		private void deleteEOBToolStripMenuItem_Click(object sender,EventArgs e) {
			RpUnfinalizedInsPay.UnfinalizedInsPay unfinalPay=(RpUnfinalizedInsPay.UnfinalizedInsPay)gridMain.Rows[gridMain.SelectedIndices[0]].Tag;
			if(unfinalPay.ClaimPaymentCur==null) {
				MsgBox.Show(this,"This claim payment has been deleted.");
				return;
			}
			//Most likely this claim payment is marked as partial. Everyone should have permission to delete a partial payment.
			//Added a check to make sure user has permission and claimpayment is not partial.
			if(!Security.IsAuthorized(Permissions.InsPayEdit,unfinalPay.ClaimPaymentCur.CheckDate) && !unfinalPay.ClaimPaymentCur.IsPartial) {
				return;
			}
			if(!MsgBox.Show(this,true,"Delete this insurance check?")) {
				return;
			}
			try {
				ClaimPayments.Delete(unfinalPay.ClaimPaymentCur);
			}
			catch(ApplicationException ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			LoadData();
			FillGrid();
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			LoadData();
			FillGrid();
		}

		//Copied from FormRpOutstandingIns.cs
		private void butPrint_Click(object sender,EventArgs e) {
			_pagesPrinted=0;
			PrintDocument pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			pd.DefaultPageSettings.Landscape=true;
			if(pd.DefaultPageSettings.PrintableArea.Height==0) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			_headingPrinted=false;
			try {
#if DEBUG
				FormRpPrintPreview pView=new FormRpPrintPreview();
				pView.printPreviewControl2.Document=pd;
				pView.ShowDialog();
#else
					if(PrinterL.SetPrinter(pd,PrintSituation.Default,0,"Unfinalized Payment report printed")) {
						pd.Print();
					}
#endif
			}
			catch {
				MessageBox.Show(Lan.g(this,"Printer not available"));
			}
		}

		//Copied from FormRpOutstandingIns.cs
		private void pd_PrintPage(object sender,PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			if(!_headingPrinted) {
				text=Lan.g(this,"Unfinalized Insurance Payment Report");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				if(comboBoxMultiType.SelectedIndices[0].ToString()=="0") {
					text="For All Types";
				}
				else {
					text="For Types: ";
					for(int i=0;i<comboBoxMultiType.SelectedIndices.Count;i++) {
						if(i!=0) {
							text+=", ";
						}
						text+=_listTypes[(int)comboBoxMultiType.SelectedIndices[i]].ToString();
					}
				}
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				if(comboBoxMultiClinics.SelectedIndices[0].ToString()=="0") {
					text="For All Clinics";
				}
				else {
					text="For Clinics: ";
					for(int i=0;i<comboBoxMultiClinics.SelectedIndices.Count;i++) {
						if(i!=0) {
							text+=", ";
						}
						if(comboBoxMultiClinics.SelectedIndices[i].ToString()=="1") {
							text+="Unassigned";
						}
						else {
							text+=_listClinics[(int)comboBoxMultiClinics.SelectedIndices[i]-2].Abbr;
						}
					}
				}
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=20;
				_headingPrinted=true;
				_headingPrintH=yPos;
			}
			#endregion
			yPos=gridMain.PrintPage(g,_pagesPrinted,bounds,_headingPrintH);
			_pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
			}
			g.Dispose();
		}

		//Copied from FormRpOutstandingIns.cs
		private void butExport_Click(object sender,System.EventArgs e) {
			SaveFileDialog saveFileDialog=new SaveFileDialog();
			saveFileDialog.AddExtension=true;
			saveFileDialog.FileName="Unfinalized Insurance Payments";
			if(!Directory.Exists(PrefC.GetString(PrefName.ExportPath))) {
				try {
					Directory.CreateDirectory(PrefC.GetString(PrefName.ExportPath));
					saveFileDialog.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
				}
				catch {
					//initialDirectory will be blank
				}
			}
			else {
				saveFileDialog.InitialDirectory=PrefC.GetString(PrefName.ExportPath);
			}
			saveFileDialog.Filter="Text files(*.txt)|*.txt|Excel Files(*.xls)|*.xls|All files(*.*)|*.*";
			saveFileDialog.FilterIndex=0;
			if(saveFileDialog.ShowDialog()!=DialogResult.OK) {
				return;
			}
			try {
				using(StreamWriter sw=new StreamWriter(saveFileDialog.FileName,false))
				{
					String line="";
					for(int i=0;i<gridMain.Columns.Count;i++) {
						line+=gridMain.Columns[i].Heading+"\t";
					}
					sw.WriteLine(line);
					for(int i=0;i<gridMain.Rows.Count;i++) {
						line="";
						for(int j=0;j<gridMain.Columns.Count;j++) {
							line+=gridMain.Rows[i].Cells[j].Text;
							if(j<gridMain.Columns.Count-1) {
								line+="\t";
							}
						}
						sw.WriteLine(line);
					}
				}
			}
			catch {
				MessageBox.Show(Lan.g(this,"File in use by another program.  Close and try again."));
				return;
			}
			MessageBox.Show(Lan.g(this,"File created successfully"));
		}
		
		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

	}


}