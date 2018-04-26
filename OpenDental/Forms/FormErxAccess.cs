using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	///<summary>This form is only visible at HQ, or possibly at a distributor HQ.  This form is used to control provider access to eRx.</summary>
	public partial class FormErxAccess:ODForm {

		private Patient _patCur;
		private List<ProviderErx> _listProvErxs;

		public FormErxAccess(Patient pat) {
			InitializeComponent();
			Lan.F(this);
			_patCur=pat;
		}

		private void FormErxAccess_Load(object sender,EventArgs e) {
			FillProviders();
		}

		private void FillProviders() {
			gridProviders.BeginUpdate();
			gridProviders.Rows.Clear();
			gridProviders.Columns.Clear();
			gridProviders.Columns.Add(new UI.ODGridColumn("Type",74,HorizontalAlignment.Left));//Column width determined in LayoutProviders().
			gridProviders.Columns.Add(new UI.ODGridColumn("IsEnabled",74,HorizontalAlignment.Center));//Column width determined in LayoutProviders().
			gridProviders.Columns.Add(new UI.ODGridColumn("IsIDPd",74,HorizontalAlignment.Center));//Column width determined in LayoutProviders().
			gridProviders.Columns.Add(new UI.ODGridColumn("IsEPCS",74,HorizontalAlignment.Center));
			gridProviders.Columns.Add(new UI.ODGridColumn("NPI",0,HorizontalAlignment.Left));//Column width determined in LayoutProviders().
			//Gets from db.  Better to call db than to use cache at HQ, since cache might be large.
			//Only get Legacy eRx items.  Other types will be in the BroadcasterMonitor.
			_listProvErxs=ProviderErxs.Refresh(_patCur.PatNum).FindAll(x => x.ErxType==ErxOption.Legacy);
			for(int i=0;i<_listProvErxs.Count;i++) {
				UI.ODGridRow row=new UI.ODGridRow();
				row.Tag=_listProvErxs[i];
				row.Cells.Add(new UI.ODGridCell(_listProvErxs[i].ErxType.ToString()));
				string status="";
				if(_listProvErxs[i].IsEnabled==ErxStatus.Enabled) {
					status="X";
				}
				else if(_listProvErxs[i].IsEnabled!=ErxStatus.Disabled) {
					status="P";
				}
				row.Cells.Add(new UI.ODGridCell(status));
				row.Cells.Add(new UI.ODGridCell(_listProvErxs[i].IsIdentifyProofed?"X":""));
				row.Cells.Add(new UI.ODGridCell(_listProvErxs[i].IsEpcs?"X":""));
				row.Cells.Add(new UI.ODGridCell(_listProvErxs[i].NationalProviderID));
				gridProviders.Rows.Add(row);
			}
			gridProviders.EndUpdate();
		}

		private void checkShowDoseSpot_Click(object sender,EventArgs e) {
			FillProviders();
		}

		private void checkShowLegacy_Click(object sender,EventArgs e) {
			FillProviders();
		}

		private void butEnable_Click(object sender,EventArgs e) {
			if(gridProviders.SelectedIndices.Length==0) {
				MsgBox.Show(this,"At least one provider must be selected.");
				return;
			}
			gridProviders.BeginUpdate();
			for(int i=0;i<gridProviders.SelectedIndices.Length;i++) {
				int index=gridProviders.SelectedIndices[i];
				UI.ODGridRow row=gridProviders.Rows[index];
				row.Cells[1].Text="X";
				ProviderErx provErx=(ProviderErx)row.Tag;
				provErx.IsEnabled=ErxStatus.Enabled;
			}
			gridProviders.EndUpdate();
		}

		private void butDisable_Click(object sender,EventArgs e) {
			if(gridProviders.SelectedIndices.Length==0) {
				MsgBox.Show(this,"At least one provider must be selected.");
				return;
			}
			if(!MsgBox.Show(this,true,"Only use this button if the selected providers were Enabled accidentally or if the provider has canceled eRx.  "
				+"Continue?"))
			{
				return;
			}
			gridProviders.BeginUpdate();
			for(int i=0;i<gridProviders.SelectedIndices.Length;i++) {
				int index=gridProviders.SelectedIndices[i];
				UI.ODGridRow row=gridProviders.Rows[index];
				row.Cells[1].Text="";
				ProviderErx provErx=(ProviderErx)row.Tag;
				provErx.IsEnabled=ErxStatus.Disabled;
			}
			gridProviders.EndUpdate();
		}

		private void butIdpd_Click(object sender,EventArgs e) {
			if(gridProviders.SelectedIndices.Length==0) {
				MsgBox.Show(this,"At least one provider must be selected.");
				return;
			}
			gridProviders.BeginUpdate();
			for(int i=0;i<gridProviders.SelectedIndices.Length;i++) {
				int index=gridProviders.SelectedIndices[i];
				UI.ODGridRow row=gridProviders.Rows[index];
				row.Cells[2].Text="X";
				ProviderErx provErx=(ProviderErx)row.Tag;
				provErx.IsIdentifyProofed=true;
			}
			gridProviders.EndUpdate();
		}

		private void butNotIdpd_Click(object sender,EventArgs e) {
			if(gridProviders.SelectedIndices.Length==0) {
				MsgBox.Show(this,"At least one provider must be selected.");
				return;
			}
			if(!MsgBox.Show(this,true,"Only use this button if the selected providers were set to IDP'd accidentally.  Continue?")) {
				return;
			}
			gridProviders.BeginUpdate();
			for(int i=0;i<gridProviders.SelectedIndices.Length;i++) {
				int index=gridProviders.SelectedIndices[i];
				UI.ODGridRow row=gridProviders.Rows[index];
				row.Cells[2].Text="";
				ProviderErx provErx=(ProviderErx)row.Tag;
				provErx.IsIdentifyProofed=false;
			}
			gridProviders.EndUpdate();
		}

		private void butEPCS_Click(object sender,EventArgs e) {
			if(gridProviders.SelectedIndices.Length==0) {
				MsgBox.Show(this,"At least one provider must be selected.");
				return;
			}
			gridProviders.BeginUpdate();
			for(int i=0;i<gridProviders.SelectedIndices.Length;i++) {
				int index=gridProviders.SelectedIndices[i];
				UI.ODGridRow row=gridProviders.Rows[index];
				row.Cells[3].Text="X";
				ProviderErx provErx=(ProviderErx)row.Tag;
				provErx.IsEpcs=true;
			}
			gridProviders.EndUpdate();
		}

		private void butNotEPCS_Click(object sender,EventArgs e) {
			if(gridProviders.SelectedIndices.Length==0) {
				MsgBox.Show(this,"At least one provider must be selected.");
				return;
			}
			gridProviders.BeginUpdate();
			for(int i=0;i<gridProviders.SelectedIndices.Length;i++) {
				int index=gridProviders.SelectedIndices[i];
				UI.ODGridRow row=gridProviders.Rows[index];
				row.Cells[3].Text="";
				ProviderErx provErx=(ProviderErx)row.Tag;
				provErx.IsEpcs=false;
			}
			gridProviders.EndUpdate();
		}

		private void butOK_Click(object sender,EventArgs e) {
			List<ProviderErx> listOldProvErxs=ProviderErxs.Refresh(_patCur.PatNum).FindAll(x => x.ErxType==ErxOption.Legacy);
			ProviderErxs.Sync(_listProvErxs,listOldProvErxs);//No cache refresh because this is an HQ only form.
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}