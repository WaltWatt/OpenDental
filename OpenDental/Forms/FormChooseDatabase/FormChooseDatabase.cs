using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental {
	public partial class FormChooseDatabase : BaseFormChooseDatabase {

		public FormChooseDatabase() {
			InitializeComponent();
			Lan.F(this);
		}

		private void ChooseDatabaseView_Load(object sender,EventArgs e) {
			if(_model.IsAccessedFromMainMenu) {
				comboComputerName.Enabled=false;
				_model.CentralConnectionCur.ServerName=DataConnection.GetServerName();
				comboDatabase.Enabled=false;
				_model.CentralConnectionCur.DatabaseName=DataConnection.GetDatabaseName();
			}
			checkConnectServer.Checked=false;
			groupDirect.Enabled=true;
			groupServer.Enabled=false;
			comboComputerName.Text=_model.CentralConnectionCur.ServerName;
			comboDatabase.Text=_model.CentralConnectionCur.DatabaseName;
			textUser.Text=_model.CentralConnectionCur.MySqlUser;
			textPassword.Text=_model.CentralConnectionCur.MySqlPassword;
			textPassword.PasswordChar=(textPassword.Text=="" ? default(char) : '*');
			textUser2.Text=_model.CentralConnectionCur.OdUser;
			//textPassword2.Text not allowed to be preset.
			if(listType.Items.Count > 0 && listType.Items.Count >= 2) {
				listType.SelectedIndex=(int)_model.DbType;
			}
			textConnectionString.Text=_model.ConnectionString;
			checkNoShow.Checked=(_model.NoShow==YN.Yes);
			checkBoxAutomaticLogin.Checked=_model.CentralConnectionCur.IsAutomaticLogin;
			if(!string.IsNullOrEmpty(_model.CentralConnectionCur.ServiceURI)) {
				checkConnectServer.Checked=true;
				groupDirect.Enabled=false;
				groupServer.Enabled=true;
				textURI.Text=_model.CentralConnectionCur.ServiceURI;
				checkUsingEcw.Checked=_model.CentralConnectionCur.WebServiceIsEcw;
				textUser2.Select();
				return;
			}
			if(textUser2.Text!="") {
				textPassword2.Select();
			}
		}

		public override bool TryGetModelFromView(out ChooseDatabaseModel model) {
			model=null;
			try {
				_model.CentralConnectionCur.ServerName=comboComputerName.Text;
				_model.CentralConnectionCur.DatabaseName=comboDatabase.Text;
				_model.CentralConnectionCur.MySqlUser=textUser.Text;
				_model.CentralConnectionCur.MySqlPassword=textPassword.Text;
				_model.NoShow=(checkNoShow.Checked ? YN.Yes : YN.No);
				_model.CentralConnectionCur.ServiceURI=(checkConnectServer.Checked ? textURI.Text : "");
				_model.CentralConnectionCur.OdUser=textUser2.Text;
				_model.CentralConnectionCur.OdPassword=textPassword2.Text;
				_model.CentralConnectionCur.WebServiceIsEcw=checkUsingEcw.Checked;
				_model.DbType=(listType.SelectedIndex==1 ? DatabaseType.Oracle : DatabaseType.MySql);
				_model.ConnectionString=textConnectionString.Text;
				_model.CentralConnectionCur.IsAutomaticLogin=checkBoxAutomaticLogin.Checked;
			}
			catch(Exception) {
				return false;
			}
			model=_model.Copy();
			return true;
		}

		public void SetController(ChooseDatabaseController controller) {
			_controller=controller;
		}

		public void FillComboComputerNames(string[] arrayComputerNames) {
			comboComputerName.Items.Clear();
			comboComputerName.Items.AddRange(arrayComputerNames);
		}

		public void FillComboDatabases(string[] arrayDatabases) {
			comboDatabase.Items.Clear();
			comboDatabase.Items.AddRange(arrayDatabases);
		}

		public void AddDatabaseType(string dbType) {
			listType.Items.Add(dbType);
		}

		public void SetDatabaseType(int index) {
			if(index > -1 && index < listType.Items.Count) {
				listType.SelectedIndex=index;
			}
		}

		private void comboDatabase_DropDown(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;
			_controller.DatabaseDropDown(sender,e);
			Cursor=Cursors.Default;
		}

		private void checkConnectServer_Click(object sender,EventArgs e) {
			if(checkConnectServer.Checked) {
				groupServer.Enabled=true;
				groupDirect.Enabled=false;
			}
			else {
				groupServer.Enabled=false;
				groupDirect.Enabled=true;
			}
		}

		private void textPassword_TextChanged(object sender,EventArgs e) {
			if(textPassword.Text=="") {
				textPassword.PasswordChar=default(char);//if text is cleared, turn off password char mask
			}
		}

		private void textPassword_Leave(object sender,EventArgs e) {
			textPassword.PasswordChar=(textPassword.Text=="" ? default(char) : '*');//mask password if loaded from the config file
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(!_controller.butOK_Click(sender,e)) {
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}

	///<summary>Required so that Visual Studio can design this form.  The designer does not allow directly extending classes with generics.</summary>
	public class BaseFormChooseDatabase : ODFormMVC<ChooseDatabaseModel,FormChooseDatabase,ChooseDatabaseController> { }
}