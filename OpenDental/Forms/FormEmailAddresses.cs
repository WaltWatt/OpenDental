using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormEmailAddresses:ODForm {
		public bool IsSelectionMode;
		public long EmailAddressNum;
		///<summary>If true, a signal for invalid Email cache will be sent out upon closing.</summary>
		public bool IsChanged;
		private List<EmailAddress> _listEmailAddresses;

		public FormEmailAddresses() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormEmailAddresses_Load(object sender,EventArgs e) {
			if(IsSelectionMode) {
				labelInboxComputerName.Visible=false;
				textInboxComputerName.Visible=false;
				butThisComputer.Visible=false;
				labelInboxCheckInterval.Visible=false;
				textInboxCheckInterval.Visible=false;
				labelInboxCheckUnits.Visible=false;
			}
			else {
				textInboxComputerName.Text=PrefC.GetString(PrefName.EmailInboxComputerName);
				textInboxCheckInterval.Text=PrefC.GetInt(PrefName.EmailInboxCheckInterval).ToString();//Calls PIn() internally.
			}
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(IsSelectionMode) {
				EmailAddressNum=_listEmailAddresses[gridMain.GetSelectedIndex()].EmailAddressNum;
				DialogResult=DialogResult.OK;
			}
			else {
				FormEmailAddressEdit FormEAE=new FormEmailAddressEdit();
				FormEAE.EmailAddressCur=_listEmailAddresses[e.Row];
				FormEAE.ShowDialog();
				if(FormEAE.DialogResult==DialogResult.OK) {
					IsChanged=true;
					FillGrid();
				}
			}
		}

		private void FillGrid() {
			EmailAddresses.RefreshCache();
			_listEmailAddresses=EmailAddresses.GetDeepCopy();
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g(this,"User Name"),240);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Sender Address"),270);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Default"),50,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Notify"),0,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listEmailAddresses.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listEmailAddresses[i].EmailUsername);
				row.Cells.Add(_listEmailAddresses[i].SenderAddress);
				row.Cells.Add((_listEmailAddresses[i].EmailAddressNum==PrefC.GetLong(PrefName.EmailDefaultAddressNum))?"X":"");
				row.Cells.Add((_listEmailAddresses[i].EmailAddressNum==PrefC.GetLong(PrefName.EmailNotifyAddressNum))?"X":"");
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butSetDefault_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select a row first.");
				return;
			}
			if(Prefs.UpdateLong(PrefName.EmailDefaultAddressNum,_listEmailAddresses[gridMain.GetSelectedIndex()].EmailAddressNum)) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			FillGrid();
		}

		private void butWebMailNotify_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select a row first.");
				return;
			}
			if(Prefs.UpdateLong(PrefName.EmailNotifyAddressNum,_listEmailAddresses[gridMain.GetSelectedIndex()].EmailAddressNum)) {
				DataValid.SetInvalid(InvalidType.Prefs);
			}
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormEmailAddressEdit FormEAE=new FormEmailAddressEdit();
			FormEAE.EmailAddressCur=new EmailAddress();
			FormEAE.IsNew=true;
			FormEAE.ShowDialog();
			if(FormEAE.DialogResult==DialogResult.OK) {
				FillGrid();
				IsChanged=true;
			}
		}

		private void butThisComputer_Click(object sender,EventArgs e) {
			textInboxComputerName.Text=Dns.GetHostName();
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(IsSelectionMode) {
				if(gridMain.GetSelectedIndex()==-1) {
					MsgBox.Show(this,"Please select an email address.");
					return;
				}
				EmailAddressNum=_listEmailAddresses[gridMain.GetSelectedIndex()].EmailAddressNum;
			}
			else {//The following fields are only visible when not in selection mode.
				if(textInboxComputerName.Text.Trim().ToLower()=="localhost" || textInboxComputerName.Text.Trim()=="127.0.0.1") {
					//If we allowed localhost, then there would potentially be email duplication in the db when emails are pulled in.
					MsgBox.Show(this,"Computer name to fetch new email from cannot be localhost or 127.0.0.1 or any other loopback address.");
					return;
				}
				int inboxCheckIntervalMinuteCount=0;
				try {
					inboxCheckIntervalMinuteCount=int.Parse(textInboxCheckInterval.Text);
					if(inboxCheckIntervalMinuteCount<1 || inboxCheckIntervalMinuteCount>60) {
						throw new ApplicationException("Invalid value.");//User never sees this message.
					}
				}
				catch {
					MsgBox.Show(this,"Inbox check interval must be between 1 and 60 inclusive.");
					return;
				}
				if(Prefs.UpdateString(PrefName.EmailInboxComputerName,textInboxComputerName.Text)
					| Prefs.UpdateInt(PrefName.EmailInboxCheckInterval,inboxCheckIntervalMinuteCount)) {
					DataValid.SetInvalid(InvalidType.Prefs);
				}
			}
			DialogResult=DialogResult.OK;
		}

		private void FormEmailAddresses_FormClosing(object sender,FormClosingEventArgs e) {
			if(IsChanged) {
				DataValid.SetInvalid(InvalidType.Email);
			}
		}

	}
}