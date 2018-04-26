using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using OpenDental.UI;

namespace OpenDental {
	public partial class FormDomainUserPick:ODForm {

		public string SelectedDomainName;
		private const int _ACCOUNTDISABLE=0x0002; //The userAccount Control is a collection of bit flags. ACCOUNTDISABLE mask is 0x0002
		private string[] _fields= {
			"userPrincipalName",	//Preferred login name. May be blank if the user was created pre-Windows 2000.
			"sAMAccountName",			//login name to support pre-Windows 2000. Included in case the userPrincipalName is blank
			"displayName",				//Display Name
			"primaryGroupID",			//If the user belongs to a group
			"userAccountControl"	//Account status. 512=enabled, 514=disabled, 66048=enabled, password never expires, 66050=disabled, password never expires
		};

		public FormDomainUserPick() {
			InitializeComponent();
			Lan.F(this);
		}
				
		private void FormDomainUserPick_Load(object sender,EventArgs e) {
			FillGrid();
		}

		private void FillGrid() {
			SearchResultCollection listResults=GetUsers();
			if(listResults==null) {
				return;
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			gridMain.Rows.Clear();
			ODGridColumn col;
			for(int i=0;i<_fields.Length;i++) {
				col=new ODGridColumn(Lan.g(this,_fields[i]),175);
				gridMain.Columns.Add(col);
			}
			ODGridRow row;
			foreach(SearchResult result in listResults) {
				row=new ODGridRow();
				row.Tag="";
				for(int i=0;i<_fields.Length;i++) {
					if(result.Properties.Contains(_fields[i])) { //some accounts may not have userPrincipalName if they were created before Windows 2000
						switch(_fields[i]) {
							case "sAMAccountName":
								row.Tag=result.Properties[_fields[i]][0].ToString(); //use the pre-Windows2000 username because all users have one
								break;
							case "userAccountControl":
								row.Cells.Add(GetAccountEnabledStatus((int)result.Properties[_fields[i]][0]));
								continue;
						}
						row.Cells.Add(result.Properties[_fields[i]][0].ToString());
					}
					else {
						row.Cells.Add("");
					}
				}				
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		public string GetAccountEnabledStatus(int flag) {
			if(Convert.ToBoolean(flag & _ACCOUNTDISABLE)) {
				return "Disabled";
			}
			return "Enabled";
		}

		private SearchResultCollection GetUsers() {
			try {
				DirectoryEntry myLdapConnection=new DirectoryEntry(PrefC.GetString(PrefName.DomainLoginPath));
				DirectorySearcher search=new DirectorySearcher(myLdapConnection);
				search.Filter="(&(objectClass=user)(objectCategory=person))";
				search.Sort.PropertyName=_fields[0];//by default sort results by the first field
				for(int i=0;i<_fields.Length;i++) {
					search.PropertiesToLoad.Add(_fields[i]);
				}
				return search.FindAll();
			}
			catch(Exception ex) {
				MessageBox.Show(Lan.g(this,"An error occurred fetching domain users: ")+" "+ex.Message);
				return null;
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			SelectedDomainName=PIn.String(gridMain.Rows[gridMain.GetSelectedIndex()].Tag.ToString());
			DialogResult=DialogResult.OK;
		}
		
		private void butNone_Click(object sender,EventArgs e) {
			SelectedDomainName="";//Empty string indicates no domain user
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()!=-1) {
				SelectedDomainName=PIn.String(gridMain.Rows[gridMain.GetSelectedIndex()].Tag.ToString());
			}
			else {
				SelectedDomainName="";
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}