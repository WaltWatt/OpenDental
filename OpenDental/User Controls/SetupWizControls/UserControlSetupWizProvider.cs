using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using OpenDental.UI;
using OpenDental;
using OpenDentBusiness;


namespace OpenDental.User_Controls.SetupWizard {
	public partial class UserControlSetupWizProvider:SetupWizControl {
		private int _blink;
		public UserControlSetupWizProvider() {
			InitializeComponent();
		}

		private void UserControlSetupWizProvider_Load(object sender,EventArgs e) {
			FillGrid();
			if(Providers.GetWhere(x => x.FName.ToLower() != "default",true).ToList().Count==0) {
				MsgBox.Show("FormSetupWizard","You have no valid providers. Please click the add button to Add a provider or Add information to the default providers.");
				timer1.Start();
			}
		}

		private void FillGrid() {
			List<Provider> listProvs=Providers.GetDeepCopy(true);
			Color needsAttnCol = OpenDental.SetupWizard.GetColor(ODSetupStatus.NeedsAttention);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col = new ODGridColumn(Lan.g("FormSetupWizard","First Name"),90);
			gridMain.Columns.Add(col);
			col = new ODGridColumn(Lan.g("FormSetupWizard","Last Name"),90);
			gridMain.Columns.Add(col);
			col = new ODGridColumn(Lan.g("FormSetupWizard","Abbrev"),70);
			gridMain.Columns.Add(col);
			col = new ODGridColumn(Lan.g("FormSetupWizard","Suffix"),60);
			gridMain.Columns.Add(col);
			col = new ODGridColumn(Lan.g("FormSetupWizard","SSN/TIN"),130);
			gridMain.Columns.Add(col);
			col = new ODGridColumn(Lan.g("FormSetupWizard","NPI"),130);
			gridMain.Columns.Add(col);
			col = new ODGridColumn(Lan.g("FormSetupWizard","AptColor"),60);
			gridMain.Columns.Add(col);
			col = new ODGridColumn(Lan.g("FormSetupWizard","LineColor"),60);
			gridMain.Columns.Add(col);
			col = new ODGridColumn(Lan.g("FormSetupWizard","IsHyg"),60,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			bool isAllComplete = true;
			if(listProvs.Where(x => x.FName.ToLower() != "default").ToList().Count==0) {
				isAllComplete=false;
			}
			foreach(Provider prov in listProvs) {
				row = new ODGridRow();
				bool isDentist = OpenDental.SetupWizard.ProvSetup.IsPrimary(prov);
				bool isHyg = prov.IsSecondary;
				row.Cells.Add(prov.FName);
				if((isDentist || isHyg) && (string.IsNullOrEmpty(prov.FName) || prov.FName.ToLower() == "default")) {
					row.Cells[row.Cells.Count-1].CellColor=needsAttnCol;
					isAllComplete=false;
				}
				row.Cells.Add(prov.LName);
				if((isDentist || isHyg) && string.IsNullOrEmpty(prov.LName)) {
					row.Cells[row.Cells.Count-1].CellColor=needsAttnCol;
					isAllComplete=false;
				}
				row.Cells.Add(prov.Abbr);
				if((isDentist || isHyg) && string.IsNullOrEmpty(prov.Abbr)) {
					row.Cells[row.Cells.Count-1].CellColor=needsAttnCol;
					isAllComplete=false;
				}
				row.Cells.Add(prov.Suffix);
				if((isDentist) && string.IsNullOrEmpty(prov.Suffix)) {
					row.Cells[row.Cells.Count-1].CellColor=needsAttnCol;
					isAllComplete=false;
				}
				row.Cells.Add(prov.SSN);
				if((isDentist) && string.IsNullOrEmpty(prov.SSN)) {
					row.Cells[row.Cells.Count-1].CellColor=needsAttnCol;
					isAllComplete=false;
				}
				row.Cells.Add(prov.NationalProvID);
				if((isDentist) && string.IsNullOrEmpty(prov.NationalProvID)) {
					row.Cells[row.Cells.Count-1].CellColor=needsAttnCol;
					isAllComplete=false;
				}
				row.Cells.Add("");
				row.Cells[row.Cells.Count-1].CellColor = prov.ProvColor;
				//not required
				row.Cells.Add("");
				row.Cells[row.Cells.Count-1].CellColor = prov.OutlineColor;
				//not required
				row.Cells.Add(prov.IsSecondary?"X":"");
				//not required
				row.Tag=prov;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			if(isAllComplete) {
				IsDone=true;
			}
			else {
				IsDone=false;
			}
		}

		private void timer1_Tick(object sender,EventArgs e) {
			if(_blink > 5) {
				pictureAdd.Visible=true;
				foreach(ODGridRow rowCur in gridMain.Rows) {
					rowCur.ColorBackG=OpenDental.SetupWizard.GetColor(ODSetupStatus.NeedsAttention);
				}
				gridMain.Invalidate();
				timer1.Stop();
				return;
			}
			pictureAdd.Visible=!pictureAdd.Visible;
			foreach(ODGridRow rowCur in gridMain.Rows) {
				rowCur.ColorBackG=rowCur.ColorBackG==Color.White?OpenDental.SetupWizard.GetColor(ODSetupStatus.NeedsAttention):Color.White;
			}
			gridMain.Invalidate();
			_blink++;
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			Provider selectedProv = (Provider)gridMain.Rows[e.Row].Tag;
			FormProvEdit FormPE = new FormProvEdit();
			FormPE.ProvCur = selectedProv;
			FormPE.ShowDialog();
			if(FormPE.DialogResult == DialogResult.OK) {
				FillGrid();
			}
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormProvEdit FormPE = new FormProvEdit();
			FormPE.ProvCur=new Provider();
			FormPE.ProvCur.IsNew=true;
			Provider provCur = new Provider();
			if(gridMain.SelectedIndices.Length>0) {//place new provider after the first selected index. No changes are made to DB until after provider is actually inserted.
				FormPE.ProvCur.ItemOrder=((Provider)gridMain.Rows[gridMain.SelectedIndices[0]].Tag).ItemOrder;//now two with this itemorder
			}
			else if(gridMain.Rows.Count>0) {
				FormPE.ProvCur.ItemOrder=((Provider)gridMain.Rows[gridMain.Rows.Count-1].Tag).ItemOrder+1;
			}
			else {
				FormPE.ProvCur.ItemOrder=0;
			}
			FormPE.IsNew=true;
			FormPE.ShowDialog();
			if(FormPE.DialogResult!=DialogResult.OK) {
				return;
			}
			provCur=FormPE.ProvCur;
			//new provider has already been inserted into DB from above
			Providers.MoveDownBelow(provCur);//safe to run even if none selected.
			Cache.Refresh(InvalidType.Providers);
			FillGrid();
		}

		private void butAdvanced_Click(object sender,EventArgs e) {
			new FormProviderSetup().ShowDialog();
			FillGrid();
		}
	}
}
