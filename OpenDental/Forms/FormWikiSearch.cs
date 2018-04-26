using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;

namespace OpenDental {
	///<summary></summary>
	public delegate void NavToPageDeligate(string pageTitle);

	public partial class FormWikiSearch:ODForm {
		private List<string> listWikiPageTitles;
		public string wikiPageTitleSelected;
		public NavToPageDeligate NavToPage;

		public FormWikiSearch() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormWikiSearch_Load(object sender,EventArgs e) {
			Rectangle rectWorkingArea=System.Windows.Forms.Screen.GetWorkingArea(this);
			Top=0;
			Left=Math.Max(0,((rectWorkingArea.Width-1200)/2)+rectWorkingArea.Left);
			Width=Math.Min(rectWorkingArea.Width,1200);
			Height=rectWorkingArea.Height;
			FillGrid();
			wikiPageTitleSelected="";
		}

		private void LoadWikiPage(string WikiPageTitleCur) {
			webBrowserWiki.AllowNavigation=true;
			butRestore.Enabled=false;
			try {
				if(checkArchivedOnly.Checked) {
					webBrowserWiki.DocumentText=WikiPages.TranslateToXhtml(WikiPages.GetByTitle(WikiPageTitleCur,isDeleted:true).PageContent,true);
					butRestore.Enabled=true;
				}
				else {
					webBrowserWiki.DocumentText=WikiPages.TranslateToXhtml(WikiPages.GetByTitle(WikiPageTitleCur).PageContent,true);
				}
			}
			catch(Exception ex) {
				webBrowserWiki.DocumentText="";
				MessageBox.Show(this,Lan.g(this,"This page is broken and cannot be viewed.  Error message:")+" "+ex.Message);
			}
		}

		/// <summary></summary>
		private void FillGrid() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g(this,"Title"),70);
			gridMain.Columns.Add(col);
			//col=new ODGridColumn(Lan.g(this,"Saved"),42);
			//gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			if(checkArchivedOnly.Checked) {
				//This used to search the wikipagehist table, now archived pages are stored in wikipage.  See JobNum 4429.
				listWikiPageTitles=WikiPages.GetForSearch(textSearch.Text,checkIgnoreContent.Checked,isDeleted:true);
			}
			else {
				listWikiPageTitles=WikiPages.GetForSearch(textSearch.Text,checkIgnoreContent.Checked);
			}
			for(int i=0;i<listWikiPageTitles.Count;i++) {
				ODGridRow row=new ODGridRow();
				row.Cells.Add(listWikiPageTitles[i]);
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			webBrowserWiki.DocumentText="";
		}

		private void gridMain_CellClick(object sender,ODGridClickEventArgs e) {
			LoadWikiPage(listWikiPageTitles[e.Row]);
			gridMain.Focus();
		}

		private void gridMain_CellDoubleClick(object sender,UI.ODGridClickEventArgs e) {			
			//SelectedWikiPage=listWikiPages[e.Row];
			if(checkArchivedOnly.Checked) {
				return;
			}
			wikiPageTitleSelected=listWikiPageTitles[e.Row];
			NavToPage(wikiPageTitleSelected);
			Close();
		}

		private void textSearch_TextChanged(object sender,EventArgs e) {
			//FillGrid();
			timer1.Stop();
			timer1.Start();
		}

		private void checkIgnoreContent_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void checkArchivedOnly_CheckedChanged(object sender,EventArgs e) {
			butOK.Enabled=!checkArchivedOnly.Checked;
			FillGrid();
		}

		private void webBrowserWiki_Navigated(object sender,WebBrowserNavigatedEventArgs e) {
			webBrowserWiki.AllowNavigation=false;//to disable links in pages.
		}

		private void butRestore_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				return;//should never happen.
			}
			wikiPageTitleSelected=listWikiPageTitles[gridMain.SelectedIndices[0]];
			if(WikiPages.GetByTitle(wikiPageTitleSelected)!=null) {
				MsgBox.Show(this,"Selected page has already been restored.");//should never happen.
				return;
			}
			WikiPage wikiPageRestored=WikiPages.GetByTitle(listWikiPageTitles[gridMain.SelectedIndices[0]],isDeleted:true);
			if(wikiPageRestored==null) {
				MsgBox.Show(this,"Selected page has already been restored.");//should never happen.
				return;
			}
			WikiPages.WikiPageRestore(wikiPageRestored,Security.CurUser.UserNum);
			Close();
		}

		private void timer1_Tick(object sender,EventArgs e) {
			timer1.Stop();
			FillGrid();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(gridMain.SelectedIndices.Length>0) {
				wikiPageTitleSelected=listWikiPageTitles[gridMain.SelectedIndices[0]];
			}
			NavToPage(wikiPageTitleSelected);
			Close();
		}

		private void butCancel_Click(object sender,EventArgs e) {
			Close();
		}
	}
}