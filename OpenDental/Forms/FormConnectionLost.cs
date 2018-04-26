using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental {
	public partial class FormConnectionLost:ODForm {

		///<summary>Optionally set errorMessage to override the label text that is displayed to the user.</summary>
		public FormConnectionLost(string errorMessage="") {
			InitializeComponent();
			labelErrMsg.Text=errorMessage;
			DataConnectionEvent.Fired+=DataConnection_ConnectionRestored;//Listen for events that might signify that the connection is restored
		}

		private void TestConnection() {
			Cursor=Cursors.WaitCursor;
			DataConnection dconn=new DataConnection();
			bool isConnected=true;
			try {
				dconn.SetDb(DataConnection.GetCurrentConnectionString(),"",DataConnection.DBtype);
				DataConnectionEvent.Fire(new DataConnectionEventArgs("DataConnection","Connection restored",true,
					DataConnection.GetCurrentConnectionString()));
			}
			catch { 
				isConnected=false;
			}
			Cursor=Cursors.Default;
			if(isConnected) {
				DialogResult=DialogResult.OK;
			}
		}
		
		///<summary>DataConnection will be repeatedly trying to reestablish a database connection, and will throw this event when it does.</summary>
		private void DataConnection_ConnectionRestored(DataConnectionEventArgs e) {
			if(e.IsConnectionRestored) {
				DialogResult=DialogResult.OK;
			}
		}

		private void butRetry_Click(object sender,EventArgs e) {
			TestConnection();
		}

		private void butExit_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormConnectionLost_FormClosing(object sender,FormClosingEventArgs e) {
			DataConnectionEvent.Fired-=DataConnection_ConnectionRestored;
		}
	}
}