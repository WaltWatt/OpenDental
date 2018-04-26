using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;
using System.IO;
using System.Xml;

namespace OpenDental {
	public partial class FormWikiListHistory:ODForm {
		///<summary>Set from outside this form to load all appropriate data into the form during Form_Load().</summary>
		public string ListNameCur;
		public bool IsReverted;
		private List<WikiListHist> _listWikiListHists;
		private DataTable _tableCur;
		private DataTable _tableOld;

		public FormWikiListHistory() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormWikiListHistory_Load(object sender,EventArgs e) {
			FillGridMain();
			if(gridMain.Rows.Count>0) {
				gridMain.SetSelected(gridMain.Rows.Count-1,true);
			}
			Text="Wiki List History - "+ListNameCur;
			gridOld.Title="Old Revision";
			gridCur.Title="Current Revision";
			FillGridOld();
			FillGridCur();
		}

		private void FormWikiListHistory_Resize(object sender,EventArgs e) {
			//assuming gridMain, textNumbers do not change width or location.
			Rectangle rectWorkingArea=new Rectangle(
				gridMain.Right+4, //to the right of the history grid
				gridMain.Top, //at the same distance from the top of the form as the hostory grid.
				butRevert.Left-gridMain.Right-12, //Total width - the width of buttons, history grid, and padding.
				gridMain.Height);//same height as the history grid, which is resized via anchors.
			//old revision resize
			gridOld.Top=rectWorkingArea.Top;
			gridOld.Height=rectWorkingArea.Height;
			gridOld.Left=rectWorkingArea.Left;
			gridOld.Width=rectWorkingArea.Width/2-2;
			//current revision resize
			gridCur.Top=rectWorkingArea.Top;
			gridCur.Height=rectWorkingArea.Height;
			gridCur.Left=rectWorkingArea.Left+rectWorkingArea.Width/2+2;
			gridCur.Width=rectWorkingArea.Width/2-2;
		}

		/// <summary></summary>
		private void FillGridMain() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"User"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g(this,"Saved"),80);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			_listWikiListHists=WikiListHists.GetByName(ListNameCur);
			for(int i=0;i<_listWikiListHists.Count;i++) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(Userods.GetName(_listWikiListHists[i].UserNum));
				row.Cells.Add(_listWikiListHists[i].DateTimeSaved.ToString());
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		/// <summary></summary>
		private void FillGridOld() {
			List<WikiListHeaderWidth> colHeaderWidths=new List<WikiListHeaderWidth>();
			_tableOld=new DataTable();
			if(gridMain.GetSelectedIndex() > -1) {
				colHeaderWidths=WikiListHeaderWidths.GetFromListHist(_listWikiListHists[gridMain.GetSelectedIndex()]);
				using(XmlReader xmlReader=XmlReader.Create(new StringReader(_listWikiListHists[gridMain.GetSelectedIndex()].ListContent))) {
					try {
						_tableOld.ReadXml(xmlReader);
					}
					catch(Exception) {
						MsgBox.Show(this,"Corruption detected in the Old Revision table.  Partial data will be displayed.  Please call us for support.");
					}
				}
			}
			gridOld.BeginUpdate();
			gridOld.Columns.Clear();
			ODGridColumn col;
			for(int c=0;c<_tableOld.Columns.Count;c++) {
				int colWidth=100;//100 = default value in case something is malformed in the database.
				foreach(WikiListHeaderWidth colHead in colHeaderWidths) {
					if(colHead.ColName==_tableOld.Columns[c].ColumnName) {
						colWidth=colHead.ColWidth;
						break;
					}
				}
				col=new ODGridColumn(_tableOld.Columns[c].ColumnName,colWidth,false);
				gridOld.Columns.Add(col);
			}
			gridOld.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_tableOld.Rows.Count;i++) {
				row=new ODGridRow();
				for(int c=0;c<_tableOld.Columns.Count;c++) {
					row.Cells.Add(_tableOld.Rows[i][c].ToString());
				}
				gridOld.Rows.Add(row);
				gridOld.Rows[i].Tag=i;
			}
			gridOld.EndUpdate();
		}

		/// <summary></summary>
		private void FillGridCur() {
			List<WikiListHeaderWidth> listColHeaderWidths=WikiListHeaderWidths.GetForList(ListNameCur);
			_tableCur=WikiLists.GetByName(ListNameCur);
			gridCur.BeginUpdate();
			gridCur.Columns.Clear();
			ODGridColumn col;
			for(int c=0;c<_tableCur.Columns.Count;c++) {
				int colWidth=100;//100 = default value in case something is malformed in the database.
				foreach(WikiListHeaderWidth colHead in listColHeaderWidths) {
					if(colHead.ColName==_tableCur.Columns[c].ColumnName) {
						colWidth=colHead.ColWidth;
						break;
					}
				}
				col=new ODGridColumn(_tableCur.Columns[c].ColumnName,colWidth,false);
				gridCur.Columns.Add(col);
			}
			gridCur.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_tableCur.Rows.Count;i++) {
				row=new ODGridRow();
				for(int c=0;c<_tableCur.Columns.Count;c++) {
					row.Cells.Add(_tableCur.Rows[i][c].ToString());
				}
				gridCur.Rows.Add(row);
				gridCur.Rows[i].Tag=i;
			}
			gridCur.EndUpdate();
		}

		private void gridMain_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length<1) {
				return;
			}
			FillGridOld();
			gridMain.Focus();
		}

		private void butRevert_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1){
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Revert list to currently selected revision?")) {
				return;
			}
			try {
				WikiListHists.RevertFrom(_listWikiListHists[gridMain.GetSelectedIndex()],Security.CurUser.UserNum);
			}
			catch(Exception) {
				MsgBox.Show(this,"There was an error when trying to revert changes.  Please call us for support.");
				return;
			}
			FillGridMain();
			gridMain.SetSelected(false);
			gridMain.SetSelected(gridMain.Rows.Count-1,true);//select the new revision.
			gridMain.ScrollToEnd();//in case there are LOTS of revisions. Should this go in the fill grid code? 
			FillGridOld();
			FillGridCur();
			IsReverted=true;
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

	}
}