using CodeBase.MVC;
using OpenDentBusiness;
using System;
using System.Windows.Forms;

namespace OpenDental {
	public class ChooseDatabaseController:ODControllerAbs<FormChooseDatabase> {

		public ChooseDatabaseController(FormChooseDatabase view) : base(view) {
			_view.Load+=FormChooseDatabase_Load;
		}

		private void FormChooseDatabase_Load(object sender,EventArgs e) {
			//The model can be requested from the form once the form has officially loaded.
			ChooseDatabaseModel chooseDatabaseModel;
			_view.TryGetModelFromView(out chooseDatabaseModel);
			_view.FillComboDatabases(CentralConnections.GetDatabases(chooseDatabaseModel.CentralConnectionCur,chooseDatabaseModel.DbType));
		}

		public override void OnPostInit() {
			_view.AddDatabaseType("MySQL");
			_view.AddDatabaseType("Oracle");
			_view.SetDatabaseType(0);
			_view.FillComboComputerNames(CentralConnections.GetComputerNames());
		}

		internal void DatabaseDropDown(object sender,EventArgs e) {
			ChooseDatabaseModel chooseDatabaseModel;
			_view.TryGetModelFromView(out chooseDatabaseModel);
			_view.FillComboDatabases(CentralConnections.GetDatabases(chooseDatabaseModel.CentralConnectionCur
				,chooseDatabaseModel.DbType));
		}

		internal bool butOK_Click(object sender,EventArgs e) {
			ChooseDatabaseModel chooseDatabaseModel;
			_view.TryGetModelFromView(out chooseDatabaseModel);
			try {
				CentralConnections.TryToConnect(chooseDatabaseModel.CentralConnectionCur,chooseDatabaseModel.DbType,chooseDatabaseModel.ConnectionString
					,(chooseDatabaseModel.NoShow==YN.Yes),chooseDatabaseModel.ListAdminCompNames);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return false;
			}
			//A successful connection was made using the settings within the current choose database model.
			return true;
		}
	}
}
