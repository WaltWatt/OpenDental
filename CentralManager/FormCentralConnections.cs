using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CentralManager {
	public partial class FormCentralConnections:Form {
		///<summary>Initially blank or should be filled with connection copies.
		///When OK is clicked, this list will contain all selected connections.</summary>
		public List<CentralConnection> ListConns;
		///<summary>A filtered list of connections.  Gets refilled every time FillGrid is called.</summary>
		private List<CentralConnection> _listConnsDisplay;
		private List<ConnectionGroup> _listConnectionGroups;
		///<summary>A list of the currently selected CentralConnectionNums.</summary>
		private List<long> _listSelectedConnNums;

		public FormCentralConnections() : this(new List<long>()) {
		}

		///<summary>Launches the central connections window with the list of corresponding connections selected.</summary>
		public FormCentralConnections(List<long> listSelectedConnNums) {
			InitializeComponent();
			ListConns=new List<CentralConnection>();
			_listSelectedConnNums=listSelectedConnNums;
		}

		private void FormCentralConnections_Load(object sender,EventArgs e) {
			_listConnectionGroups=ConnectionGroups.GetDeepCopy();
			comboConnectionGroups.Items.Add("All");
			comboConnectionGroups.Items.AddRange(_listConnectionGroups.Select(x => x.Description).ToArray());
			comboConnectionGroups.SelectedIndex=0;//Default to all.
			FillGrid();
		}

		private void FillGrid() {
			if(_listSelectedConnNums.Count==0) {//Could have been set on load.
				foreach(int i in gridMain.SelectedIndices) {
					_listSelectedConnNums.Add(((CentralConnection)gridMain.Rows[i].Tag).CentralConnectionNum);
				}
			}
			_listConnsDisplay=null;
			if(comboConnectionGroups.SelectedIndex>0) {
				_listConnsDisplay=CentralConnections.FilterConnections(ListConns,textSearch.Text,_listConnectionGroups[comboConnectionGroups.SelectedIndex-1]);
			}
			else {
				_listConnsDisplay=CentralConnections.FilterConnections(ListConns,textSearch.Text,null);
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn("#",40);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Database",300);
			gridMain.Columns.Add(col);
			col=new ODGridColumn("Note",260);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			List<int> listSelectedIndices=new List<int>();
			foreach(CentralConnection conn in _listConnsDisplay) {
				row=new ODGridRow();
				row.Cells.Add(conn.ItemOrder.ToString());
				if(conn.DatabaseName=="") {//uri
					row.Cells.Add(conn.ServiceURI);
				}
				else {
					row.Cells.Add(conn.ServerName+", "+conn.DatabaseName);
				}
				row.Cells.Add(conn.Note);
				//Add the selected index if needed before adding the row to the collection of rows.
				if(_listSelectedConnNums.Exists(x => x==(conn.CentralConnectionNum))) {
					listSelectedIndices.Add(gridMain.Rows.Count);
				}
				row.Tag=conn;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			//Reselect any connections that were selected before re-filling the grid.
			gridMain.SetSelected(listSelectedIndices.ToArray(),true);
			_listSelectedConnNums.Clear();//Always clear the list out for subsequent Fills will remember selected indicies.
		}

		private void comboConnectionGroups_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void textSearch_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			FormCentralConnectionEdit FormCCE=new FormCentralConnectionEdit();
			FormCCE.CentralConnectionCur=(CentralConnection)gridMain.Rows[e.Row].Tag;
			FormCCE.ShowDialog();
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			CentralConnection conn=new CentralConnection();
			conn.IsNew=true;
			FormCentralConnectionEdit FormCCS=new FormCentralConnectionEdit();
			FormCCS.CentralConnectionCur=conn;
			if(FormCCS.ShowDialog()==DialogResult.OK) {//Will insert conn on OK.
				ListConns.Add(FormCCS.CentralConnectionCur);//IsNew will be false if inserted
			}
			FillGrid();//Refreshing the grid will show any new connections added.
		}

		private void butOK_Click(object sender,EventArgs e) {
			//Add any selected connections to ListConns so that forms outside know which connections to add.
			ListConns.Clear();
			foreach(int i in gridMain.SelectedIndices) {
				CentralConnection conn=(CentralConnection)gridMain.Rows[i].Tag;
				ListConns.Add(conn);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
		
	}
}