using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OpenDental {
	public partial class FormWikiListHeaders : ODForm {
		private string _wikiListCurName;
		///<summary>Widths must always be specified.  Not optional.</summary>
		private List<WikiListHeaderWidth> _listTableHeaders;
		///<summary>All possible "options" for the currently selected Wiki List Header.</summary>
		private List<string> _listComboOptions=new List<string>();
		private int _gridOldIndex=-1;
		private int _gridCurIndex=-1;
		private int _pickListIndex=-1;

		public FormWikiListHeaders(string wikiListCurName) {
			InitializeComponent();
			Lan.F(this);
			_wikiListCurName=wikiListCurName;
		}

		private void FormWikiListHeaders_Load(object sender,EventArgs e) {
			List<WikiListHeaderWidth> listTableHeadersShallow=WikiListHeaderWidths.GetForList(_wikiListCurName);
			_listTableHeaders=listTableHeadersShallow.Select(x => x.Copy()).ToList();
			FillGrid();
		}

		///<summary>Each row of data becomes a column in the grid. This pattern is only used in a few locations.</summary>
		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableWikiListHeaders","Column Name"),100,true);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableWikiListHeaders","Width"),0,true);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			for(int i = 0;i<_listTableHeaders.Count;i++) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(_listTableHeaders[i].ColName);
				row.Cells.Add(_listTableHeaders[i].ColWidth.ToString());
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void FillGridPickList() {
			gridPickList.BeginUpdate();
			gridPickList.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(gridPickList.TranslationName,"Input Text"),100,true);
			col.IsEditable=true;
			gridPickList.Columns.Add(col);
			gridPickList.Rows.Clear();
			if(gridMain.SelectedCell.Y!=-1) {
				ODGridRow row;
				for(int i = 0;i<_listComboOptions.Count;i++) {
					row=new ODGridRow();
					row.Cells.Add(_listComboOptions[i]);
					gridPickList.Rows.Add(row);
				}
			}
			gridPickList.EndUpdate();
			if(_pickListIndex > -1 && _listComboOptions.Count > _pickListIndex) {
				gridPickList.SetSelected(_pickListIndex,true);
			}
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			_gridOldIndex=_gridCurIndex;
			_gridCurIndex=e.Row;
			//convert list back a \r\n delimited string
			if(_gridOldIndex>0) {//0 is the primary key which we want to ignore saving PickList for.
				_listTableHeaders[_gridOldIndex].PickList=string.Join("\r\n",_listComboOptions);
			}
			_listComboOptions=_listTableHeaders[e.Row].PickList.Split(new string[] { "\r\n" },StringSplitOptions.RemoveEmptyEntries).ToList();
			FillGridPickList();
		}

		///<summary>Used to store the currently selected cell in the secondary grid.  
		///SelectedCell and Selected don't behave correctly when you click away or the cell is editable.</summary>
		private void gridPickList_CellEnter(object sender,ODGridClickEventArgs e) {
			_pickListIndex=e.Row;
		}

		private void gridPickList_CellLeave(object sender,ODGridClickEventArgs e) {
			string addedListItem=gridPickList.Rows[e.Row].Cells[0].Text;
			if(e.Row < _listComboOptions.Count) {
				_listComboOptions[e.Row]=addedListItem;
			}
		}

		private void butAdd_Click(object sender,EventArgs e) {
			//Get input from user
			InputBox newOption=new InputBox(Lan.g(this,"New Pick List Option"));
			if(newOption.ShowDialog()==DialogResult.OK) {
				_listComboOptions.Add(newOption.textResult.Text);
				FillGridPickList();
			}
		}

		private void butRemove_Click(object sender,EventArgs e) {
			//Check to make sure a cell is selected to delete
			if(_pickListIndex>-1) {
				_listComboOptions.RemoveAt(_pickListIndex);
				_pickListIndex--;
				FillGridPickList();
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(_gridCurIndex>0) {
				_listTableHeaders[_gridCurIndex].PickList=string.Join("\r\n",_listComboOptions);
			}
			//Set primary key to correct name-----------------------------------------------------------------------
			gridMain.Rows[0].Cells[0].Text=_wikiListCurName+"Num";//prevents exceptions from occuring when user tries to rename PK.
			//Validate column names---------------------------------------------------------------------------------
			for(int i = 0;i<gridMain.Rows.Count;i++) {//ODGridCell colNameCell in gridMain.Rows[0].Cells){
				if(Regex.IsMatch(gridMain.Rows[i].Cells[0].Text,@"^\d")) {
					MsgBox.Show(this,"Column cannot start with numbers.");
					return;
				}
				if(Regex.IsMatch(gridMain.Rows[i].Cells[0].Text,@"\s")) {
					MsgBox.Show(this,"Column names cannot contain spaces.");
					return;
				}
				if(Regex.IsMatch(gridMain.Rows[i].Cells[0].Text,@"\W")) {//W=non-word chars
					MsgBox.Show(this,"Column names cannot contain special characters.");
					return;
				}
			}
			//Check for reserved words--------------------------------------------------------------------------------
			for(int i = 0;i<gridMain.Rows.Count;i++) {//ODGridCell colNameCell in gridMain.Rows[0].Cells){
				if(DbHelper.isMySQLReservedWord(gridMain.Rows[i].Cells[0].Text)) {
					MessageBox.Show(Lan.g(this,"Column name is a reserved word in MySQL")+":"+gridMain.Rows[i].Cells[0].Text);
					return;
				}
				//primary key is caught by duplicate column name logic.
			}
			//Check for duplicates-----------------------------------------------------------------------------------
			List<string> listColNamesCheck=new List<string>();
			for(int i = 0;i<gridMain.Rows.Count;i++) {//ODGridCell colNameCell in gridMain.Rows[0].Cells){
				if(listColNamesCheck.Contains(gridMain.Rows[i].Cells[0].Text)) {
					MessageBox.Show(Lan.g(this,"Duplicate column name detected")+":"+gridMain.Rows[0].Cells[i].Text);
					return;
				}
				listColNamesCheck.Add(gridMain.Rows[i].Cells[0].Text);
			}
			//Validate column widths---------------------------------------------------------------------------------
			for(int i = 0;i<gridMain.Rows.Count;i++) {//ODGridCell colNameCell in gridMain.Rows[0].Cells){
				if(Regex.IsMatch(gridMain.Rows[i].Cells[1].Text,@"\D")) {// "\D" matches any non-decimal character
					MsgBox.Show(this,"Column widths must only contain positive integers.");
					return;
				}
				if(gridMain.Rows[i].Cells[1].Text.Contains("-")
					|| gridMain.Rows[i].Cells[1].Text.Contains(".")
					|| gridMain.Rows[i].Cells[1].Text.Contains(",")) //inlcude the comma for international support. For instance Pi = 3.1415 or 3,1415 depending on your region
				{
					MsgBox.Show(this,"Column widths must only contain positive integers.");
					return;
				}
			}
			//save values to List<WikiListHeaderWidth> TableHeaders
			for(int i = 0;i<_listTableHeaders.Count;i++) {
				_listTableHeaders[i].ColName=PIn.String(gridMain.Rows[i].Cells[0].Text);
				_listTableHeaders[i].ColWidth=PIn.Int(gridMain.Rows[i].Cells[1].Text);
			}
			//Save data to database-----------------------------------------------------------------------------------
			try {
				WikiListHeaderWidths.UpdateNamesAndWidths(_wikiListCurName,_listTableHeaders);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);//will throw exception if table schema has changed since the window was opened.
				DialogResult=DialogResult.Cancel;
			}
			DataValid.SetInvalid(InvalidType.Wiki);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}