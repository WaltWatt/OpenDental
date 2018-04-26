using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;
using System.Linq;

namespace OpenDental {
	public partial class FormPhoneComps:ODForm {
		///<summary>A local, deep copy of the phonecomp that is filled on load.  This list should never be manipulated.
		///Used to sync to the database on closing.</summary>
		private readonly List<PhoneComp> _listPhoneCompsOrig;
		///<summary>A local, deep copy of the phonecomp that is filled on load and manipulated throughout the life cycle of this window.
		///Used to sync to the database on closing.</summary>
		private List<PhoneComp> _listPhoneComps;

		public FormPhoneComps() {
			InitializeComponent();
			Lan.F(this);
			_listPhoneCompsOrig=PhoneComps.GetDeepCopy();
			_listPhoneComps=_listPhoneCompsOrig.Select(x => x.Copy())
				.OrderBy(x => x.PhoneExt)
				.ThenBy(x => x.ComputerName)
				.ToList();
		}

		private void FormPhoneComps_Load(object sender,EventArgs e) {
			FillMain();
			FillListComps();
		}

		private void FillMain() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			//It is important that the Ext column (editable) is the first column in the grid.  See gridMain_CellLeave().
			gridMain.Columns.Add(new ODGridColumn(Lan.g(gridMain.TranslationName,"Ext"),-1,GridSortingStrategy.AmountParse,true));
			gridMain.Columns.Add(new ODGridColumn(Lan.g(gridMain.TranslationName,"CompName"),-2,GridSortingStrategy.StringCompare));
			gridMain.Rows.Clear();
			ODGridRow row;
			foreach(PhoneComp phoneComp in _listPhoneComps) {
				row=new ODGridRow();
				row.Cells.Add((phoneComp.PhoneExt > 0) ? phoneComp.PhoneExt.ToString() : "");
				row.Cells.Add(phoneComp.ComputerName);
				row.Tag=phoneComp;
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void FillListComps() {
			listBoxComputers.Items.Clear();
			foreach(Computer computer in Computers.GetDeepCopy()) {
				listBoxComputers.Items.Add(new ODBoxItem<Computer>(computer.CompName,computer));
			}
		}

		private void AddComputers(List<string> listComputerNames) {
			if(listComputerNames==null) {
				return;
			}
			//Remove any computer names we already know about.
			listComputerNames.RemoveAll(x => _listPhoneComps.Any(y => y.ComputerName.ToUpper()==x.ToUpper()));
			if(listComputerNames.Count < 1) {
				return;
			}
			//Add all new computer names to our in-memory list of PhoneComps, sort the list and then refresh the grid.
			foreach(string computerName in listComputerNames) {
				_listPhoneComps.Add(new PhoneComp() {
					ComputerName=computerName,
					PhoneExt=0,
				});
			}
			_listPhoneComps=_listPhoneComps.OrderBy(x => x.PhoneExt)
				.ThenBy(x => x.ComputerName)
				.ToList();
			FillMain();
		}

		private void gridMain_CellLeave(object sender,ODGridClickEventArgs e) {
			if(e.Col!=0) {//Ext is the only editable column.
				return;
			}
			//Sync the extension column with the current row's tag because the user could have manually changed it.
			int extension=PIn.Int(gridMain.Rows[e.Row].Cells[e.Col].Text,false);
			gridMain.Rows[e.Row].Cells[e.Col].Text=extension.ToString();//Update the cell value in case PIn changed the value.  E.g. 'abc' turns into '0'
			((PhoneComp)gridMain.Rows[e.Row].Tag).PhoneExt=extension;//Update the tag so that synching on OK click works as expected.
		}

		private void butAdd_Click(object sender,EventArgs e) {
			AddComputers(listBoxComputers.SelectedTags<Computer>().Select(x => x.CompName).ToList());
			listBoxComputers.ClearSelected();
		}

		private void butAddCustom_Click(object sender,EventArgs e) {
			string computerName=PIn.String(textCustomCompName.Text.Trim());
			textCustomCompName.Clear();
			if(string.IsNullOrEmpty(computerName)) {
				return;
			}
			AddComputers(new List<string>() { computerName });
		}

		private void butOK_Click(object sender,EventArgs e) {
			//Sync the in memory list with the deep copy that was made from our cache when this window loaded.
			if(PhoneComps.Sync(_listPhoneComps,_listPhoneCompsOrig)) {
				//Changes were made when the sync was called.  Let other workstations know about the changes.
				DataValid.SetInvalid(InvalidType.PhoneComps);
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}