using OpenDental.UI;
using OpenDentBusiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental {
	public partial class FormWikiListItemEdit:ODForm {
		///<summary>Name of the wiki list.</summary>
		public string WikiListCurName;
		public long ItemNum;
		public bool IsNew;
		///<summary>A list of all possible column headers for the current wiki list.  Each header contains additional information (e.g. PickList) that can be useful.</summary>
		public List<WikiListHeaderWidth> ListColumnHeaders;
		private Point _selectedCell;
		///<summary>Creating a data table containing only one item allows us to use column names.</summary>
		private DataTable _tableWikiList;

		public FormWikiListItemEdit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormWikiListEdit_Load(object sender,EventArgs e) {
			_tableWikiList = WikiLists.GetItem(WikiListCurName,ItemNum);
			//Show the PK in the title bar for informational purposes.  We don't put it in the grid because user can't change it.
			this.Text=this.Text+" - "+_tableWikiList.Columns[0]+" "+_tableWikiList.Rows[0][0].ToString();//OK to use 0 indices here. If this fails something else is wrong.
			FillGrid();
		}

		/// <summary></summary>
		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Column"),200);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Value"),400);
			col.IsEditable=true;
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=1;i<_tableWikiList.Columns.Count;i++){//Start at 1 since row 0 (PK) goes in the title bar.
				row=new ODGridRow();
				row.Cells.Add(_tableWikiList.Columns[i].ColumnName);
				row.Cells.Add(_tableWikiList.Rows[0][i].ToString());
				if(i==0) {
					row.ColorBackG=Color.Gray;//darken the PK to imply that it cannot be edited.
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			gridMain.Title=Lan.g(this,"Edit List Item");
		}

		private void gridMain_CellEnter(object sender,ODGridClickEventArgs e) {
			if(ListColumnHeaders==null || ListColumnHeaders.Count<=e.Row+1 || string.IsNullOrEmpty(ListColumnHeaders[e.Row+1].PickList)) {
				comboEntry.Visible=false;
				return;
			}
			comboEntry.Items.Clear();
			//add 1 to e.Row because the primary key value isn't drawn.
			_selectedCell.Y=e.Row+1;
			_selectedCell.X=e.Col;
			List<string> listComboOptions=ListColumnHeaders[e.Row+1].PickList.Split(new string[] { "\r\n" },StringSplitOptions.RemoveEmptyEntries).ToList();
			foreach(string s in listComboOptions) {
				comboEntry.Items.Add(s);
			}
			comboEntry.SelectedIndex=listComboOptions.FindIndex(x => x==_tableWikiList.Rows[0][e.Row+1].ToString());
			//Hacktogether the location to puth the combo box
			Point drawLoc=new Point();
			drawLoc.Y=gridMain.Rows[e.Row].RowLoc+gridMain.Location.Y+gridMain.HeaderHeight+gridMain.TitleHeight+1;
			drawLoc.X=gridMain.Columns[0].ColWidth+gridMain.Location.X+1;
			comboEntry.Location=drawLoc;
			//Get the size to set the combo box to cover the item
			comboEntry.Width=gridMain.Columns[1].ColWidth+1;
			comboEntry.Height=(gridMain.Rows[e.Row].RowHeight-1);
			comboEntry.Visible=true;
			comboEntry.Focus();
			comboEntry.DroppedDown=true;
			comboEntry.Tag=(int)e.Row+1;
		}

		private void gridMain_CellLeave(object sender,ODGridClickEventArgs e) {
			//Save data from grid into table. No call to DB, so this should be safe.
			for(int i=0;i<gridMain.Rows.Count;i++) {
				_tableWikiList.Rows[0][i+1]=gridMain.Rows[i].Cells[1].Text.Replace("\r\n","\n").Replace("\n","\r\n");//Column 0 of TableItems.Rows[0] is in the title bar, so it is off from the grid by 1.
			}
		}

		private void comboEntry_Leave(object sender,EventArgs e) {
			comboEntry.Visible=false;
			//Save any previous combo box values if present.
			if(comboEntry.Tag!=null && comboEntry.SelectedItem!=null) {
				//We display this information to the user as if the columns of _tableWikiList are actually the rows.
				_tableWikiList.Rows[0][(int)comboEntry.Tag]=comboEntry.SelectedItem;
				comboEntry.Tag=null;
				comboEntry.SelectedItem=null;
				FillGrid();
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.WikiListSetup)) {//might want to implement a new security permission.
				return;
			}
			//maybe require all empty or admin priv
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete this list item and all references to it?")) {
				return;
			}
			WikiLists.DeleteItem(WikiListCurName,ItemNum);
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			WikiLists.UpdateItem(WikiListCurName,_tableWikiList);
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}
