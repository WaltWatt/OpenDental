using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CentralManager {
	public partial class FormCentralConnectionGroupEdit:Form {
		///<summary>Global list of all connections.  Never changes, used for filling filtered connection list and limiting database calls.</summary>
		private List<CentralConnection> _listConns;
		///<summary>List of all connections that are already associated with the current connection group.</summary>
		private List<CentralConnection> _listConnsCur;
		///<summary>List of all conn group attaches so that we can sync them to the database upon closing.</summary>
		private List<ConnGroupAttach> _listConnGroupAttaches;
		///<summary>ConnectionGroupCur must be passed in from outside.</summary>
		public ConnectionGroup ConnectionGroupCur;
		public bool IsNew;

		public FormCentralConnectionGroupEdit() {
			InitializeComponent();
		}

		private void FormCentralConnectionGroupEdit_Load(object sender,EventArgs e) {
			_listConns=CentralConnections.GetConnections();
			_listConnsCur=new List<CentralConnection>();
			if(IsNew) {
				_listConnGroupAttaches=new List<ConnGroupAttach>();
			}
			else {//Take full list and filter out
				_listConnGroupAttaches=ConnGroupAttaches.GetForGroup(ConnectionGroupCur.ConnectionGroupNum);
				//Grab all the connections associated to the corresponding connection group from the list of all connections.
				_listConnsCur=_listConns.FindAll(x => _listConnGroupAttaches.Exists(y => y.CentralConnectionNum==x.CentralConnectionNum));
			}
			textDescription.Text=ConnectionGroupCur.Description;
			FillGrid();
		}

		private void FillGrid() {//Only shows connections in the grid of the currently selected connection group.
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn("Database",320);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Note",300);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			foreach(CentralConnection conn in _listConnsCur) {
				row=new ODGridRow();
				if(conn.DatabaseName=="") {//uri
					row.Cells.Add(conn.ServiceURI);
				}
				else {
					row.Cells.Add(conn.ServerName+", "+conn.DatabaseName);
				}
				row.Cells.Add(conn.Note);
				row.Tag=conn;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormCentralConnections FormCC=new FormCentralConnections(_listConnsCur.Select(x => x.CentralConnectionNum).ToList());
			foreach(CentralConnection conn in _listConns) {
				FormCC.ListConns.Add(conn.Copy());//Add a copy of each CentralConnection to the FormCC's ListConns for display purposes.
			}
			FormCC.LabelText.Text=Lans.g(this,"Select connections then click OK to add them to the currently edited group.");
			FormCC.Text=Lans.g(this,"Group Connections");
			if(FormCC.ShowDialog()==DialogResult.OK) {
				//Find all connections that do not already exist in _listConnsCur
				List<CentralConnection> listNewConns=FormCC.ListConns.FindAll(x => !_listConnsCur.Exists(y => y.CentralConnectionNum==x.CentralConnectionNum));
				//Add them to _listConnsCur
				_listConnsCur.AddRange(listNewConns);
			}
			FillGrid();
		}

		private void butRemove_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MessageBox.Show(Lans.g(this,"Please select a connection first to remove it."));
				return;
			}
			//Remove highlighted connections
			//The following code is slightly harder to read but much faster than looping backwards through the list and using RemovingAt().
			List<long> listConnNums=new List<long>();
			foreach(int i in gridMain.SelectedIndices) {
				listConnNums.Add(((CentralConnection)gridMain.Rows[i].Tag).CentralConnectionNum);
			}
			//Set our current list to a list that does not have any selected connections.
			_listConnsCur=_listConnsCur.FindAll(x => !listConnNums.Exists(y => y==x.CentralConnectionNum));
			FillGrid();
		}

		private void butOK_Click(object sender,EventArgs e) {
			//Sync groupattaches for this group prior to leaving this window.  Updating the ConnectionGroup list in parent window is taken care of there.
			if(textDescription.Text=="") {
				MessageBox.Show(Lans.g(this,"Please enter something for the description."));
				return;
			}
			ConnectionGroupCur.Description=textDescription.Text;
			//Find all connections in our current list that do not have a corresponding conn group attach entry.
			List<CentralConnection> listConnsToAdd=_listConnsCur.FindAll(x => !_listConnGroupAttaches.Exists(y => y.CentralConnectionNum==x.CentralConnectionNum));
			//Add a conn group attach for each connection found.
			foreach(CentralConnection conn in listConnsToAdd) {
				ConnGroupAttach connGA=new ConnGroupAttach();
				connGA.CentralConnectionNum=conn.CentralConnectionNum;
				connGA.ConnectionGroupNum=ConnectionGroupCur.ConnectionGroupNum;
				_listConnGroupAttaches.Add(connGA);
			}
			//Make sure that we only keep all conn group attaches that have a valid match.
			//Removing any orphaned conn group attaches from our in-memory list will cause the sync to remove the entries from the db correctly.
			_listConnGroupAttaches=_listConnGroupAttaches.FindAll(x => _listConnsCur.Exists(y => y.CentralConnectionNum==x.CentralConnectionNum));
			//_listConnGroupAttaches now directly reflects what is shown in the UI, without creating duplicates.
			ConnGroupAttaches.Sync(_listConnGroupAttaches,ConnectionGroupCur.ConnectionGroupNum);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;//Do nothing, parent form forgets all changes.
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(IsNew) {//Do nothing
				DialogResult=DialogResult.Cancel;
			}
			//Deletion is permanent.  Remove all groupattaches for this group then Set ConnectionGroupCur to null so parent form knows to remove it.
			if(MessageBox.Show(this,Lans.g(this,"Delete this entire connection group?"),"",MessageBoxButtons.YesNo)==DialogResult.No) {
				return;
			}
			for(int i=0;i<_listConnGroupAttaches.Count;i++) {
				ConnGroupAttaches.Delete(_listConnGroupAttaches[i].ConnGroupAttachNum);
			}
			ConnectionGroupCur=null;
			DialogResult=DialogResult.OK;
		}

	}
}
