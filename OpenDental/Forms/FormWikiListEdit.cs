using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental {
	public partial class FormWikiListEdit : ODForm {
		///<summary>Name of the wiki list being manipulated. This does not include the "wikilist" prefix. i.e. "networkdevices" not "wikilistnetworkdevices"</summary>
		public string WikiListCurName;
		public bool IsNew;
		private DataTable _table;
		private WikiListHist _wikiListOld;
		private bool _isEdited;
		private int[] _arraySearchColIdxs;
		///<summary>A list of all possible column headers for the current wiki list.  Each header contains additional information (e.g. PickList) that can be useful.</summary>
		private List<WikiListHeaderWidth> _listColumnHeaders=new List<WikiListHeaderWidth>();


		public FormWikiListEdit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormWikiListEdit_Load(object sender,EventArgs e) {
			if(!WikiLists.CheckExists(WikiListCurName)) {
				IsNew=true;
				WikiLists.CreateNewWikiList(WikiListCurName);
			}
			_table=WikiLists.GetByName(WikiListCurName);
			_wikiListOld=WikiListHists.GenerateFromName(WikiListCurName,Security.CurUser.UserNum);
			if(_wikiListOld==null) {
				_wikiListOld=new WikiListHist();
			}
			//Fill _columnHeaders
			_listColumnHeaders=WikiListHeaderWidths.GetForList(WikiListCurName);
			radioButHighlight.Checked=true;
			radioButFilter.Checked=false;
			FillGrid();
			ActiveControl=textSearch;//start in search box.
		}

		///<summary>Fills the grid with the contents of the corresponding wiki list table in the database.
		///After filling the grid, FilterGrid() will get invoked to apply any advanced search options.</summary>
		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col;
			for(int c = 0;c<_listColumnHeaders.Count;c++) {
				col=new ODGridColumn(_listColumnHeaders[c].ColName,_listColumnHeaders[c].ColWidth+20,false);
				gridMain.Columns.Add(col);
			}
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i = 0;i<_table.Rows.Count;i++) {
				row=new ODGridRow();
				for(int c = 0;c<_table.Columns.Count;c++) {
					row.Cells.Add(_table.Rows[i][c].ToString());
				}
				gridMain.Rows.Add(row);
				gridMain.Rows[i].Tag=i;
			}
			gridMain.EndUpdate();
			gridMain.Title=WikiListCurName;
			FilterGrid();
		}

		///<summary>Visually filters gridMain.  Tag is preserved so that double clicking and editing can still work.</summary>
		private void FilterGrid() {
			radioButFilter.Enabled=true;
			radioButHighlight.Enabled=true;
			labelSearch.Text=Lan.g(this,"Search");
			labelSearch.ForeColor=Color.Black;
			List<string> searchTerms=textSearch.Text.Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToList();
			#region Advanced Search
			if(_arraySearchColIdxs!=null && _arraySearchColIdxs.Length>0) {//adv search has been used, search specific columns selected
				labelSearch.Text=Lan.g(this,"Advanced Search");
				labelSearch.ForeColor=Color.Red;
				radioButFilter.Checked=false;
				radioButFilter.Enabled=false;
				radioButHighlight.Checked=false;
				radioButHighlight.Enabled=false;
				if(textSearch.Text=="") {
					return;
				}
				gridMain.BeginUpdate();
				gridMain.Rows.Clear();
				bool hasSearchText=false;
				for(int i = 0;i<_table.Rows.Count;i++) {
					for(int j = 0;j<_arraySearchColIdxs.Length;j++) { //loop through the selected columns only (very short loop)
						//Search through the corresponding cells for any partial matches (split by spaces).
						foreach(string searchWord in searchTerms) {
							if(_table.Rows[i].ItemArray[_arraySearchColIdxs[j]].ToString().ToUpper().Contains(searchWord.ToUpper())) {//if the cell contains the searched text
								hasSearchText=true;
								break;
							}
						}
						if(hasSearchText) {
							ODGridRow row=new ODGridRow();
							for(int k = 0;k<_table.Columns.Count;k++) {
								row.Cells.Add(_table.Rows[i][k].ToString());
							}
							row.Tag=i;
							gridMain.Rows.Add(row);
							hasSearchText=false;
							break;
						}
					}//end j
				}//end i
			}
			#endregion
			#region Highlight
			else if(radioButHighlight.Checked) {//highlight radiobutton checked
				if(textSearch.Text=="") {
					return;
				}
				bool isScrollSet=false;
				for(int i = 0;i<_table.Rows.Count;i++) {
					gridMain.Rows[i].ColorBackG=Color.White;
					List<string> listCellVals=_table.Rows[i].ItemArray.Select(x => x.ToString().ToUpper()).ToList();
					foreach(string searchWord in searchTerms) {
						//If any of the cell values contains the current search word, color the row yellow and move on.
						if(listCellVals.Any(x => x.Contains(searchWord.ToUpper()))) {
							gridMain.Rows[i].ColorBackG=Color.Yellow;
							if(!isScrollSet) {//scroll to the first match in the list.
								gridMain.ScrollToIndex(i);
								isScrollSet=true;
							}
							break;
						}
					}
				}//end i
			}
			#endregion
			#region Filter
			else {//filter radiobutton checked
				if(textSearch.Text=="") {
					return;
				}
				gridMain.BeginUpdate();
				gridMain.Rows.Clear();
				ODGridRow row;
				for(int i=0;i<_table.Rows.Count;i++) {
					List<string> listCellVals=_table.Rows[i].ItemArray.Select(x => x.ToString().ToUpper()).ToList();
					foreach(string searchWord in searchTerms) {
						if(listCellVals.Any(x => x.Contains(searchWord.ToUpper()))) {
							row=new ODGridRow();
							for(int j = 0;j<_table.Columns.Count;j++) {
								row.Cells.Add(_table.Rows[i][j].ToString());
							}
							row.Tag=i;
							gridMain.Rows.Add(row);
							break;
						}
					}
				}//end i
				#endregion
			}
			gridMain.EndUpdate();
		}


		private void gridMain_CellDoubleClick(object sender,OpenDental.UI.ODGridClickEventArgs e) {
			FormWikiListItemEdit FormWLIE = new FormWikiListItemEdit();
			FormWLIE.WikiListCurName=WikiListCurName;
			FormWLIE.ItemNum=PIn.Long(_table.Rows[(int)gridMain.Rows[e.Row].Tag][0].ToString());
			FormWLIE.ListColumnHeaders=_listColumnHeaders;
			FormWLIE.ShowDialog();
			//saving occurs from within the form.
			if(FormWLIE.DialogResult!=DialogResult.OK) {
				return;
			}
			SetIsEdited();
			_table=WikiLists.GetByName(WikiListCurName);
			FillGrid();
		}

		private void gridMain_CellTextChanged(object sender,EventArgs e) {
		}

		private void gridMain_CellLeave(object sender,ODGridClickEventArgs e) {
			/*
			Table.Rows[e.Row][e.Col]=gridMain.Rows[e.Row].Cells[e.Col].Text;
			Point cellSelected=new Point(gridMain.SelectedCell.X,gridMain.SelectedCell.Y);
			FillGrid();//gridMain.SelectedCell gets cleared.
			gridMain.SetSelected(cellSelected);*/
		}

		/*No longer necessary because gridMain_CellLeave does this as text is changed.
		///<summary>This is done before generating markup, when adding or removing rows or columns, and when changing from "none" view to another view.  FillGrid can't be done until this is done.</summary>
		private void PumpGridIntoTable() {
			//table and grid will only have the same numbers of rows and columns if the view is none.
			//Otherwise, table may have more columns
			//So this is only allowed when switching from the none view to some other view.
			if(ViewShowing!=0) {
				return;
			}
			for(int i=0;i<Table.Rows.Count;i++) {
				for(int c=0;c<Table.Columns.Count;c++) {
					Table.Rows[i][c]=gridMain.Rows[i].Cells[c].Text;
				}
			}
		}*/

		private void butColumnLeft_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.WikiListSetup)) {//gives a message box if no permission
				return;
			}
			if(gridMain.SelectedCell.X==-1) {
				return;
			}
			SetIsEdited();
			Point pointNewSelectedCell=gridMain.SelectedCell;
			pointNewSelectedCell.X=Math.Max(1,pointNewSelectedCell.X-1);
			WikiLists.ShiftColumnLeft(WikiListCurName,_table.Columns[gridMain.SelectedCell.X].ColumnName);
			_table=WikiLists.GetByName(WikiListCurName);
			_listColumnHeaders=WikiListHeaderWidths.GetForList(WikiListCurName);
			FillGrid();
			gridMain.SetSelected(pointNewSelectedCell);
		}

		private void butColumnRight_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.WikiListSetup)) {//gives a message box if no permission
				return;
			}
			if(gridMain.SelectedCell.X==-1) {
				return;
			}
			SetIsEdited();
			Point pointNewSelectedCell=gridMain.SelectedCell;
			pointNewSelectedCell.X=Math.Min(gridMain.Columns.Count-1,pointNewSelectedCell.X+1);
			WikiLists.ShiftColumnRight(WikiListCurName,_table.Columns[gridMain.SelectedCell.X].ColumnName);
			_table=WikiLists.GetByName(WikiListCurName);
			_listColumnHeaders=WikiListHeaderWidths.GetForList(WikiListCurName);
			FillGrid();
			gridMain.SetSelected(pointNewSelectedCell);
		}

		private void butColumnEdit_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.WikiListSetup)) {//gives a message box if no permission
				return;
			}
			FormWikiListHeaders FormWLH=new FormWikiListHeaders(WikiListCurName);
			FormWLH.ShowDialog();
			if(FormWLH.DialogResult!=DialogResult.OK) {
				return;
			}
			SetIsEdited();
			_table=WikiLists.GetByName(WikiListCurName);
			_listColumnHeaders=WikiListHeaderWidths.GetForList(WikiListCurName);
			FillGrid();
		}

		private void butColumnAdd_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.WikiListSetup)) {//gives a message box if no permission
				return;
			}
			SetIsEdited();
			WikiLists.AddColumn(WikiListCurName);
			_table=WikiLists.GetByName(WikiListCurName);
			_listColumnHeaders=WikiListHeaderWidths.GetForList(WikiListCurName);
			FillGrid();
		}

		private void butColumnDelete_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.WikiListSetup)) {//gives a message box if no permission
				return;
			}
			if(gridMain.SelectedCell.X==-1) {
				MsgBox.Show(this,"Select cell in column to be deleted first.");
				return;
			}
			if(!WikiLists.CheckColumnEmpty(WikiListCurName,_table.Columns[gridMain.SelectedCell.X].ColumnName)) {
				MsgBox.Show(this,"Column cannot be deleted because it contains data.");
				return;
			}
			SetIsEdited();
			WikiLists.DeleteColumn(WikiListCurName,_table.Columns[gridMain.SelectedCell.X].ColumnName);
			_table=WikiLists.GetByName(WikiListCurName);
			_listColumnHeaders=WikiListHeaderWidths.GetForList(WikiListCurName);
			FillGrid();
		}

		private void butAddItem_Click(object sender,EventArgs e) {
			FormWikiListItemEdit FormWLIE = new FormWikiListItemEdit();
			FormWLIE.WikiListCurName=WikiListCurName;
			FormWLIE.ItemNum=WikiLists.AddItem(WikiListCurName);
			FormWLIE.ListColumnHeaders=_listColumnHeaders;
			FormWLIE.ShowDialog();
			if(FormWLIE.DialogResult!=DialogResult.OK) {
				WikiLists.DeleteItem(FormWLIE.WikiListCurName,FormWLIE.ItemNum);//delete new item because dialog was not OK'ed.
				return;
			}
			long itemNum=FormWLIE.ItemNum;//capture itemNum to prevent marshall-by-reference warning
			SetIsEdited();
			_table=WikiLists.GetByName(WikiListCurName);
			FillGrid();
			for(int i = 0;i<gridMain.Rows.Count;i++) {
				if(gridMain.Rows[i].Cells[0].Text==itemNum.ToString()) {
					gridMain.Rows[i].ColorBackG=Color.FromArgb(255,255,128);
					gridMain.ScrollToIndex(i);
				}
			}
		}

		private void textSearch_TextChanged(object sender,EventArgs e) {
			timerSearch.Stop();
			timerSearch.Start();
		}

		private void timerSearch_Tick(object sender,EventArgs e) {
			timerSearch.Stop();
			FillGrid();
		}

		private void butRenameList_Click(object sender,EventArgs e) {
			//Logic copied from FormWikiLists.butAdd_Click()---------------------
			InputBox inputListName = new InputBox("New List Name");
			inputListName.ShowDialog();
			if(inputListName.DialogResult!=DialogResult.OK) {
				return;
			}
			//Format input as it would be saved in the database--------------------------------------------
			inputListName.textResult.Text=inputListName.textResult.Text.ToLower().Replace(" ","");
			//Validate list name---------------------------------------------------------------------------
			if(DbHelper.isMySQLReservedWord(inputListName.textResult.Text)) {
				//Can become an issue when retrieving column header names.
				MsgBox.Show(this,"List name is a reserved word in MySQL.");
				return;
			}
			if(inputListName.textResult.Text=="") {
				MsgBox.Show(this,"List name cannot be blank.");
				return;
			}
			if(WikiLists.CheckExists(inputListName.textResult.Text)) {
				MsgBox.Show(this,"List name already exists.");
				return;
			}
			try {
				WikiLists.Rename(WikiListCurName,inputListName.textResult.Text);
				SetIsEdited();
				WikiListHists.Rename(WikiListCurName,inputListName.textResult.Text);
				WikiListCurName=inputListName.textResult.Text;
				_table=WikiLists.GetByName(WikiListCurName);
				FillGrid();
			}
			catch(Exception ex) {
				MessageBox.Show(this,ex.Message);
			}
		}

		///<summary>Set the _isEdited bool to true and saves a copy of the list. This only happens once. This prevents spamming of updates.</summary>
		private void SetIsEdited() {
			if(_isEdited || IsNew) {//Dont save a wikiListHist entry if this is a new list, or we have already saved an entry prior to a previous edit.
				return;
			}
			_wikiListOld.WikiListHistNum=WikiListHists.Insert(_wikiListOld);
			_isEdited=true;
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.WikiListSetup)) {//gives a message box if no permission
				return;
			}
			if(gridMain.Rows.Count>0) {
				MsgBox.Show(this,"Cannot delete a non-empty list.  Remove all items first and try again.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete this entire list and all references to it?")) {
				return;
			}
			SetIsEdited();
			WikiLists.DeleteList(WikiListCurName);
			//Someday, if we have links to lists, then this is where we would update all the wikipages containing those links.  Remove links to data that was contained in the table.
			DialogResult=DialogResult.OK;
		}

		private void butHistory_Click(object sender,EventArgs e) {
			FormWikiListHistory FormWLH=new FormWikiListHistory();
			FormWLH.ListNameCur=WikiListCurName;
			FormWLH.ShowDialog();
			if(!FormWLH.IsReverted) {
				return;
			}
			//Reversion has already saved a copy of the current revision.
			_wikiListOld=WikiListHists.GenerateFromName(WikiListCurName,Security.CurUser.UserNum);
			_table=WikiLists.GetByName(WikiListCurName);
			_listColumnHeaders=WikiListHeaderWidths.GetForList(WikiListCurName);
			FillGrid();
			_isEdited=false;
			IsNew=false;
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butAdvSearch_Click(object sender,EventArgs e) {
			List<WikiListHeaderWidth> colHeaderWidths=WikiListHeaderWidths.GetForList(WikiListCurName);
			FormWikiListAdvancedSearch FormWLAS=new FormWikiListAdvancedSearch(colHeaderWidths);
			//FormWLAS.ShowDialog();
			if(FormWLAS.ShowDialog()==DialogResult.OK) {
				_arraySearchColIdxs=FormWLAS.SelectedColumnIndices;
				FillGrid();
			}
			ActiveControl=textSearch;
		}

		private void butClearAdvSearch_Click(object sender,EventArgs e) {
			_arraySearchColIdxs=new int[0];
			radioButHighlight.Checked=true;
			textSearch.Clear();
			ActiveControl=textSearch;
			FillGrid();
		}

		private void radioButHighlight_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void radioButFilter_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
		}
	}
}