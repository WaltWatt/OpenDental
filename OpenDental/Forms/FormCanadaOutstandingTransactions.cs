using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDentBusiness.Eclaims;

namespace OpenDental {
	public partial class FormCanadaOutstandingTransactions:ODForm {

		List<Carrier> carriers=new List<Carrier>();
		private List<Provider> _listProviders;

		public FormCanadaOutstandingTransactions() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormCanadaOutstandingTransactions_Load(object sender,EventArgs e) {
			carriers=Carriers.GetWhere(x => 
				(x.CanadianSupportedTypes & CanSupTransTypes.RequestForOutstandingTrans_04)==CanSupTransTypes.RequestForOutstandingTrans_04);
			foreach(Carrier carrier in carriers) {
				listCarriers.Items.Add(carrier.CarrierName);
			}
			_listProviders=Providers.GetDeepCopy(true);
			for(int i=0;i<_listProviders.Count;i++) {
				if(!_listProviders[i].IsCDAnet || _listProviders[i].NationalProvID=="" || _listProviders[i].CanadianOfficeNum=="") {
					continue;
				}
				if(!listOfficeNumbers.Items.Contains(_listProviders[i].CanadianOfficeNum)) {
					listOfficeNumbers.Items.Add(_listProviders[i].CanadianOfficeNum);
				}
			}
			if(listOfficeNumbers.Items.Count<1) {
				MsgBox.Show(this,"At least one unhidden provider must have a CDA Number and an Office Number set before running a Request for Outstanding Transactions.");
				Close();
			}
		}

		private void radioVersion2_Click(object sender,EventArgs e) {
			radioVersion2.Checked=true;
			radioVersion4Itrans.Checked=false;
			radioVersion4ToCarrier.Checked=false;
			groupCarrier.Enabled=false;
		}

		private void radioVersion4Itrans_Click(object sender,EventArgs e) {
			radioVersion2.Checked=false;
			radioVersion4Itrans.Checked=true;
			radioVersion4ToCarrier.Checked=false;
			groupCarrier.Enabled=false;
		}

		private void radioVersion4ToCarrier_Click(object sender,EventArgs e) {
			radioVersion2.Checked=false;
			radioVersion4Itrans.Checked=false;
			radioVersion4ToCarrier.Checked=true;
			groupCarrier.Enabled=true;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(radioVersion4ToCarrier.Checked) {
				if(listCarriers.SelectedIndex<0) {
					MsgBox.Show(this,"You must first select a carrier to use.");
					return;
				}
			}
			if(listOfficeNumbers.SelectedIndex<0) {
				MsgBox.Show(this,"You must first select an Office Number to use.");
				return;
			}
			Cursor=Cursors.WaitCursor;
			Provider prov=null;
			for(int i=0;i<_listProviders.Count;i++) {
				if(_listProviders[i].CanadianOfficeNum==listOfficeNumbers.Items[listOfficeNumbers.SelectedIndex].ToString() 
					&& _listProviders[i].NationalProvID!="" && _listProviders[i].IsCDAnet) {
					prov=_listProviders[i];
					break;
				}
			}
			try {
				if(radioVersion2.Checked) {
					Clearinghouse clearinghouseHq=Canadian.GetCanadianClearinghouseHq(null);
					Clearinghouse clearinghouseClin=Clearinghouses.OverrideFields(clearinghouseHq,Clinics.ClinicNum);
					CanadianOutput.GetOutstandingTransactions(clearinghouseClin,true,false,null,prov,false,FormClaimPrint.PrintCdaClaimForm,
						FormCCDPrint.PrintCCD);
				}
				else if(radioVersion4Itrans.Checked) {
					Clearinghouse clearinghouseHq=Canadian.GetCanadianClearinghouseHq(null);
					Clearinghouse clearinghouseClin=Clearinghouses.OverrideFields(clearinghouseHq,Clinics.ClinicNum);
					CanadianOutput.GetOutstandingTransactions(clearinghouseClin,false,true,null,prov,false,FormClaimPrint.PrintCdaClaimForm,
						FormCCDPrint.PrintCCD);
				}
				else if(radioVersion4ToCarrier.Checked) {
					Carrier carrier=carriers[listCarriers.SelectedIndex];
					Clearinghouse clearinghouseHq=Canadian.GetCanadianClearinghouseHq(carrier);
					Clearinghouse clearinghouseClin=Clearinghouses.OverrideFields(clearinghouseHq,Clinics.ClinicNum);
					CanadianOutput.GetOutstandingTransactions(clearinghouseClin,false,false,carrier,prov,false,FormClaimPrint.PrintCdaClaimForm,
						FormCCDPrint.PrintCCD);
				}
				Cursor=Cursors.Default;
				MsgBox.Show(this,"Done.");
			}
			catch(ApplicationException aex) {
				Cursor=Cursors.Default;
				MessageBox.Show(Lan.g(this,"Request failed: ")+aex.Message);
			}
			catch(Exception ex) {
				Cursor=Cursors.Default;
				MessageBox.Show(Lan.g(this,"Request failed: ")+ex.ToString());
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}