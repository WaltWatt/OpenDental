using CodeBase;
using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using OpenDental;
using System.Linq;

namespace CentralManager {
	public partial class FormCentralPatientSearch:Form {
		///<summary>List of connections used to connect to databases and fill patient data dictionary.</summary>
		public List<CentralConnection> ListConns;
		/// <summary>Dataset containing tables of patients for each connection.</summary>
		private DataSet _dataConnPats;
		private object _lockObj=new object();
		private int _complConnAmt;
		private string _invalidConnsLog;
		private bool _hasWarningShown=false;
		public bool AutoLogIn;

		public FormCentralPatientSearch() {
			ListConns=new List<CentralConnection>();
			InitializeComponent();
		}

		private void FormCentralPatientSearch_Load(object sender,System.EventArgs e) {
			DisplayFields.RefreshCache();
			_complConnAmt=0;
			_dataConnPats=new DataSet();
			_invalidConnsLog="";
			StartThreadsForConns();
		}

		///<summary>Loops through all connections passed in and spawns a thread for each to go fetch patient data from each db using the given filters.</summary>
		private void StartThreadsForConns() {
			_dataConnPats.Tables.Clear();
			bool hasConnsSkipped=false;
			for(int i=0;i<ListConns.Count;i++) {
				//Filter the threads by their connection name
				string connName="";
				if(ListConns[i].DatabaseName=="") {//uri
					connName=ListConns[i].ServiceURI;
				}
				else {
					connName=ListConns[i].ServerName+", "+ListConns[i].DatabaseName;
				}
				if(!connName.Contains(textConn.Text)) {
					//Do NOT spawn a thread to go fetch data for this connection because the user has filtered it out.
					//Increment the completed thread count and continue.
					hasConnsSkipped=true;
					lock(_lockObj) {
						_complConnAmt++;
					}
					continue;
				}
				//At this point we know the connection has not been filtered out, so fire up a thread to go get the patient data table for the search.
				ODThread odThread=new ODThread(GetPtDataTableForConn,new object[]{ListConns[i]});
				odThread.GroupName="FetchPats";
				odThread.Start();
			}
			if(hasConnsSkipped) {
				//There is a chance that some threads finished (by failing, etc) before the end of the loop where we spawned the threads
				//so we want to guarantee that the failure message shows if any connection was skipped.
				//This is required because FillGrid contains code that only shows a warning message when all connections have finished.
				FillGrid();
			}
		}

		private void GetPtDataTableForConn(ODThread odThread) {
			CentralConnection connection=(CentralConnection)odThread.Parameters[0];
			//Filter the threads by their connection name
			string connName="";
			if(connection.DatabaseName=="") {//uri
				connName=connection.ServiceURI;
			}
			else {
				connName=connection.ServerName+", "+connection.DatabaseName;
			}
			if(!CentralConnectionHelper.UpdateCentralConnection(connection,false)) {
				lock(_lockObj) {
					_invalidConnsLog+="\r\n"+connName;
					_complConnAmt++;
				}
				connection.ConnectionStatus="OFFLINE";
				BeginInvoke(new FillGridDelegate(FillGrid));
				return;
			}
			DataTable table=new DataTable();
			try {
				table=Patients.GetPtDataTable(checkLimit.Checked,textLName.Text,textFName.Text,textPhone.Text,
						textAddress.Text,checkHideInactive.Checked,textCity.Text,textState.Text,
						textSSN.Text,textPatNum.Text,textChartNumber.Text,0,
						checkGuarantors.Checked,!checkHideArchived.Checked,//checkHideArchived is opposite label for what this function expects, but hideArchived makes more sense
						PIn.DateT(textBirthdate.Text),0,textSubscriberID.Text,textEmail.Text,textCountry.Text,"","","");
			}
			catch(ThreadAbortException tae) {
				throw tae;//ODThread needs to clean up after an abort exception is thrown.
			}
			catch(Exception) {
				//This can happen if the connection to the server was severed somehow during the execution of the query.
				lock(_lockObj) {
					_invalidConnsLog+="\r\n"+connName+"  -GetPtDataTable";
					_complConnAmt++;
				}
				BeginInvoke(new FillGridDelegate(FillGrid));//Pops up a message box if this was the last thread to finish.
				return;
			}
			table.TableName=connName;
			odThread.Tag=table;
			lock(_lockObj) {
				_complConnAmt++;
				_dataConnPats.Tables.Add((DataTable)odThread.Tag);
			}
			BeginInvoke(new FillGridDelegate(FillGrid));
		}
				
		public delegate void FillGridDelegate();

		private void FillGrid() {
			Cursor=Cursors.WaitCursor;
			gridMain.BeginUpdate();
			List<DisplayField> fields=DisplayFields.GetForCategory(DisplayFieldCategory.CEMTSearchPatients);
			if(gridMain.Columns.Count==0) {//Init only once.
				foreach(DisplayField field in fields) {
					string heading=field.InternalName;
					if(string.IsNullOrEmpty(heading)) {
						heading=field.Description;
					}
					gridMain.Columns.Add(new ODGridColumn(heading,field.ColumnWidth));
				}
			}
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_dataConnPats.Tables.Count;i++) {
				for(int j=0;j<_dataConnPats.Tables[i].Rows.Count;j++) {
					row=new ODGridRow();
					foreach(DisplayField field in fields) {
						switch(field.InternalName) {
							#region Row Cell Filling
							case "Conn":
								row.Cells.Add(_dataConnPats.Tables[i].TableName);
								break;
							case "PatNum":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["PatNum"].ToString());
								break;
							case "LName":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["LName"].ToString());
								break;
							case "FName":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["FName"].ToString());
								break;
							case "SSN":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["SSN"].ToString());
								break;
							case "PatStatus":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["PatStatus"].ToString());
								break;
							case "Age":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["age"].ToString());
								break;
							case "City":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["City"].ToString());
								break;
							case "State":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["State"].ToString());
								break;
							case "Address":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["Address"].ToString());
								break;
							case "Wk Phone":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["WkPhone"].ToString());
								break;
							case "Email":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["Email"].ToString());
								break;
							case "ChartNum":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["ChartNumber"].ToString());
								break;
							case "MI":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["MiddleI"].ToString());
								break;
							case "Pref Name":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["Preferred"].ToString());
								break;
							case "Hm Phone":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["HmPhone"].ToString());
								break;
							case "Bill Type":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["BillingType"].ToString());
								break;
							case "Pri Prov":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["PriProv"].ToString());
								break;
							case "Birthdate":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["Birthdate"].ToString());
								break;
							case "Site":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["site"].ToString());
								break;
							case "Clinic":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["clinic"].ToString());
								break;
							case "Wireless Ph":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["WirelessPhone"].ToString());
								break;
							case "Sec Prov":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["SecProv"].ToString());
								break;
							case "LastVisit":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["lastVisit"].ToString());
								break;
							case "NextVisit":
								row.Cells.Add(_dataConnPats.Tables[i].Rows[j]["nextVisit"].ToString());
								break;
							#endregion
						}
					}
					row.Tag=ListConns.Find(x => ((x.ServerName+", "+x.DatabaseName)==_dataConnPats.Tables[i].TableName || x.ServiceURI==_dataConnPats.Tables[i].TableName));
					gridMain.Rows.Add(row);
				}
			}
			gridMain.EndUpdate();
			Cursor=Cursors.Default;
			if(_complConnAmt==ListConns.Count) {
				ODThread.QuitSyncThreadsByGroupName(1,"FetchPats");//Clean up finished threads.
				butRefresh.Text=Lans.g(this,"Refresh");
				labelFetch.Visible=false;
				if(!_hasWarningShown && _invalidConnsLog!="") {
					_hasWarningShown=true;//Keeps the message box from showing up for subsequent threads.
					MessageBox.Show(this,Lan.g(this,"Could not connect to the following servers")+":"+_invalidConnsLog);
				}
			}
			else {
				butRefresh.Text=Lans.g(this,"Stop Refresh");
				labelFetch.Visible=true;
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			CentralConnection conn=(CentralConnection)gridMain.Rows[e.Row].Tag;
			string args=CentralConnectionHelper.GetArgsFromConnection(conn);
			if(AutoLogIn) {
			  args+="UserName=\""+Security.CurUser.UserName+"\" ";
			  args+="OdPassword=\""+Security.PasswordTyped+"\" ";
      }
			args+="PatNum="+gridMain.Rows[e.Row].Cells[1].Text;//PatNum
			#if DEBUG
				Process.Start("C:\\Development\\OPEN DENTAL SUBVERSION\\head\\OpenDental\\bin\\Debug\\OpenDental.exe",args);
			#else
				Process.Start("OpenDental.exe",args);
			#endif
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			ODThread.JoinThreadsByGroupName(1,"FetchPats");//Stop fetching immediately
			_hasWarningShown=false;
			lock(_lockObj) {
				_invalidConnsLog="";
			}
			if(butRefresh.Text==Lans.g(this,"Refresh")) {
				_dataConnPats.Clear();
				butRefresh.Text=Lans.g(this,"Stop Refresh");
				labelFetch.Visible=true;
				_complConnAmt=0;
				StartThreadsForConns();
			}
			else {
				butRefresh.Text=Lans.g(this,"Refresh");
				labelFetch.Visible=false;
				_complConnAmt=ListConns.Count;
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void FormCentralPatientSearch_FormClosing(object sender,FormClosingEventArgs e) {
			//User could have closed the window before all threads finished.  Make sure to abort all threads instantly.
			ODThread.QuitSyncThreadsByGroupName(1,"FetchPats");
		}

	}
}
