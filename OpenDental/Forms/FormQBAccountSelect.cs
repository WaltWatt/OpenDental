using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormQBAccountSelect:ODForm {
		///<summary>List of deposit accounts from pref.</summary>
		private List<string> _listDepositAccountsQB;
		///<summary>List of income accounts from pref.</summary>
		private List<string> _listIncomeAccountsQB;
		///<summary>The selected account when clicking OK.  Used in FormDepositEdit to pass to quickbooks.</summary>
		public string DepositAccountSelected="";
		///<summary>The selected account when clicking OK.  Used in FormDepositEdit to pass to quickbooks.</summary>
		public string IncomeAccountSelected="";

		public FormQBAccountSelect() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormQBAccountSelect_Load(object sender,EventArgs e) {
			_listDepositAccountsQB=Accounts.GetDepositAccountsQB();
			for(int i=0;i<_listDepositAccountsQB.Count;i++) {
				comboDepositAccount.Items.Add(_listDepositAccountsQB[i]);
			}
			comboDepositAccount.SelectedIndex=0;
			_listIncomeAccountsQB=Accounts.GetIncomeAccountsQB();
			for(int i=0;i<_listIncomeAccountsQB.Count;i++) {
				comboIncomeAccountQB.Items.Add(_listIncomeAccountsQB[i]);
			}
			comboIncomeAccountQB.SelectedIndex=0;
		}

		private void butOK_Click(object sender,EventArgs e) {
			DepositAccountSelected=_listDepositAccountsQB[comboDepositAccount.SelectedIndex];
			IncomeAccountSelected=_listIncomeAccountsQB[comboIncomeAccountQB.SelectedIndex];
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}


	}
}