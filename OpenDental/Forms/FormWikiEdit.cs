using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;
using System.Xml;
using System.Text.RegularExpressions;
using System.Linq;

namespace OpenDental {
	public partial class FormWikiEdit:ODForm {
		public WikiPage WikiPageCur;
		///<summary>Need a reference to the form where this was launched from so that we can tell it to refresh later.</summary>
		public FormWiki OwnerForm;
		public bool HasSaved;//used to differentiate what action caused the form to close.
		private int ScrollTop;
    private bool _isInvalidPreview;
		private Action<string> _onWikiSaved;

		public FormWikiEdit(Action<string> onWikiSaved=null) {
			InitializeComponent();
			Lan.F(this);
			this.textContent.TextChanged += new System.EventHandler(this.textContent_TextChanged);
			WikiSaveEvent.Fired+=WikiSaveEvent_Fired;
			_onWikiSaved=onWikiSaved;
		}

		private void FormWikiEdit_Load(object sender,EventArgs e) {			
			ResizeControls();
			//LayoutToolBar();
			Text = Lan.g(this,"Wiki Edit")+" - "+WikiPageCur.PageTitle;
			if(WikiPageCur.IsNew) {
				textContent.Text=WikiPageCur.PageContent;
			}
			else {
				textContent.Text=WikiPages.GetWikiPageContentWithWikiPageTitles(WikiPageCur.PageContent);
			}
			string[] strArray=new string[1];
			strArray[0]="\n";
			textContent.Focus();
			textContent.SelectionStart=0;
			textContent.SelectionLength=0;
			textContent.ScrollToCaret();
		}

		/// <summary>Because FormWikiAllPages is no longer modal, this is necessary to be able to tell FormWikiEdit to refresh with inserted data.</summary>
		public void RefreshPage(WikiPage selectedWikiPage) {
			int tempStart=textContent.SelectionStart;
			if(selectedWikiPage==null) {
				textContent.SelectionLength=0;
				textContent.SelectedText="[[]]";
				textContent.SelectionStart=tempStart+2;
			}
			else {
				textContent.SelectionLength=0;
				textContent.SelectedText="[["+selectedWikiPage.PageTitle+"]]";
				textContent.SelectionStart=tempStart+selectedWikiPage.PageTitle.Length+4;
			}
			textContent.Focus();
			textContent.SelectionLength=0;
		}

		private void RefreshHtml() {
			webBrowserWiki.AllowNavigation=true;
			try {
				//remember scroll
				if(webBrowserWiki.Document!=null) {
					ScrollTop=webBrowserWiki.Document.GetElementsByTagName("HTML")[0].ScrollTop;
				}
				webBrowserWiki.DocumentText=WikiPages.TranslateToXhtml(textContent.Text,true,true);
        _isInvalidPreview=false;
			}
			catch(Exception ex) {
				ex.DoNothing();
        _isInvalidPreview=true;
				//don't refresh
			}
			//textContent.Focus();//this was causing a bug where it would re-highlight text after a backspace.
		}

		private void webBrowserWiki_DocumentCompleted(object sender,WebBrowserDocumentCompletedEventArgs e) {
			webBrowserWiki.Document.GetElementsByTagName("HTML")[0].ScrollTop=ScrollTop;
			textContent.Focus();
		}

		private void ResizeControls() {
			int topborder=53;
			//text resize
			textContent.Top=topborder;
			textContent.Height=ClientSize.Height-topborder;
			textContent.Width=ClientSize.Width/2-2-textContent.Left;
			//Browser resize
			webBrowserWiki.Top=topborder;
			webBrowserWiki.Height=ClientSize.Height-topborder;
			webBrowserWiki.Left=ClientSize.Width/2+2;
			webBrowserWiki.Width=ClientSize.Width/2-2;
			//Toolbar resize
			ToolBarMain.Width=ClientSize.Width;
			toolBar2.Width=ClientSize.Width;
			ToolBarMain.Invalidate();
			toolBar2.Invalidate();
			LayoutToolBars();
			//Button move
			//butRefresh.Left=ClientSize.Width/2+2;
		}

		private void FormWikiEdit_SizeChanged(object sender,EventArgs e) {
			ResizeControls();
		}

		private void textContent_TextChanged(object sender,EventArgs e) {
			//Prevent browser from updating too frequently.
			timerWikiBrowserRefresh.Stop();
			timerWikiBrowserRefresh.Start();
		}

		private void textContent_KeyPress(object sender,KeyPressEventArgs e) {
			//this doesn't always fire, which is good because the user can still use the arrow keys to move around.
			//look through all tables:
			MatchCollection matches = Regex.Matches(textContent.Text,@"\{\|\n.+?\n\|\}",RegexOptions.Singleline);
			//MatchCollection matches = Regex.Matches(textContent.Text,
			//	@"(?<=(?:\n|^))" //Checks for preceding newline or beggining of file
			//	+@"\{\|.+?\n\|\}" //Matches the table markup.
			//	+@"(?=(?:\n|$))" //Checks for following newline or end of file
			//	,RegexOptions.Singleline);
			foreach(Match match in matches) {
				if(textContent.SelectionStart >	match.Index
					&& textContent.SelectionStart <	match.Index+match.Length) 
				{
					e.Handled=true;
					MsgBox.Show(this,"Direct editing of tables is not allowed here.  Use the table button or double click to edit.");
					return;
				}
			}
			//Tab character isn't recognized by our custom markup.  Replace all tab chars with three spaces.
			if(e.KeyChar=='\t') {
				textContent.SelectedText="   ";
				e.Handled=true;
			}
		}

		private void textContent_MouseDoubleClick(object sender,MouseEventArgs e) {
			int idx=textContent.GetCharIndexFromPosition(e.Location);
			TableOrDoubleClick(idx);//we don't care what this returns because we don't want to do anything else.
		}

		///<summary>This is called both when a user double clicks anywhere in the edit box, or when the click the Table button in the toolbar.  This ONLY handles popping up an edit window for an existing table.  If the cursor was not in an existing table, then this returns false.  After that, the behavior in the two areas differs.  Returns true if it popped up.</summary>
		private bool TableOrDoubleClick(int charIdx){
			//there is some code clutter in this method from when we used TableViews.  It seems harmless, but can be removed whenever.
			MatchCollection matches=Regex.Matches(textContent.Text,@"\{\|\n.+?\n\|\}",RegexOptions.Singleline);
			//Tables-------------------------------------------------------------------------------
			Match matchCur = matches.OfType<Match>().ToList().FirstOrDefault(x=>x.Index<=charIdx && x.Index+x.Length>=charIdx);
			//handle the clicks----------------------------------------------------------------------------
			if(matchCur==null) {
				return false;//did not click inside a table
			}
			bool isLastCharacter = matchCur.Index+matchCur.Length==textContent.Text.Length;
			textContent.SelectionLength=0;//otherwise we get an annoying highlight
			//==Travis 11/20/15:  If we want to fix wiki tables in the future so duplicate tables dont both get changed from a double click, we'll need to
			//   use a regular expression to find which match of strTableLoad the user clicked on, and only replace that match below.
			FormWikiTableEdit formT=new FormWikiTableEdit();
			formT.Markup=matchCur.Value;
			formT.CountTablesInPage=matches.Count;
			formT.IsNew=false;
			formT.ShowDialog();
			if(formT.DialogResult!=DialogResult.OK) {
				return true;
			}
			if(formT.Markup==null) {//indicates delete
				textContent.Text=textContent.Text.Remove(matchCur.Index,matchCur.Length);
				textContent.SelectionLength=0;
				return true;
			}
			textContent.Text=textContent.Text.Substring(0,matchCur.Index)//beginning of markup
				+formT.Markup//replace the table
				+(isLastCharacter ? "" : textContent.Text.Substring(matchCur.Index+matchCur.Length));//continue to end, if any text after table markup.
			textContent.SelectionLength=0;
			return true;
		}

		private void webBrowserWiki_Navigated(object sender,WebBrowserNavigatedEventArgs e) {
			webBrowserWiki.AllowNavigation=false;
		}

		private void LayoutToolBars() {
			ToolBarMain.Buttons.Clear();
			//Refresh no longer needed.
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Save"),1,"","Save"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Save as Draft"),18,"","SaveDraft"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Cancel"),2,"","Cancel"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Int Link"),7,"","Int Link"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Bookmark"),7,"","Bookmark"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"File"),7,"","File Link"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Folder"),7,"","Folder Link"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Ext Link"),8,"","Ext Link"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Heading1"),9,"","H1"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Heading2"),10,"","H2"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Heading3"),11,"","H3"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Table"),15,"","Table"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Image"),16,"","Image"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			if(Security.IsAuthorized(Permissions.WikiAdmin,true)) {
				if(WikiPageCur.IsLocked) {
					ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Unlock"),19,"","Lock"));
				}
				else {
					ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Lock"),20,"","Lock"));
				}
			}
			toolBar2.Buttons.Clear();
			toolBar2.Buttons.Add(new ODToolBarButton(Lan.g(this,"Cut"),3,"","Cut"));
			toolBar2.Buttons.Add(new ODToolBarButton(Lan.g(this,"Copy"),4,"","Copy"));
			toolBar2.Buttons.Add(new ODToolBarButton(Lan.g(this,"Paste"),5,"","Paste"));
			toolBar2.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			toolBar2.Buttons.Add(new ODToolBarButton(Lan.g(this,"Undo"),6,"","Undo"));
			toolBar2.Buttons.Add(new ODToolBarButton(ODToolBarButtonStyle.Separator));
			toolBar2.Buttons.Add(new ODToolBarButton(Lan.g(this,"Bold"),12,"","Bold"));
			toolBar2.Buttons.Add(new ODToolBarButton(Lan.g(this,"Italic"),13,"","Italic"));
			toolBar2.Buttons.Add(new ODToolBarButton(Lan.g(this,"Color"),14,"","Color"));
			toolBar2.Buttons.Add(new ODToolBarButton(Lan.g(this,"Font"),17,"","Font"));
		}

		private void timerWikiBrowserRefresh_Tick(object sender,EventArgs e) {
			//This timer was set by textContent_TextChanged in order to prevent refreshing too frequently.
			timerWikiBrowserRefresh.Stop();
			RefreshHtml();
		}

		private void ToolBarMain_ButtonClick(object sender,OpenDental.UI.ODToolBarButtonClickEventArgs e) {
			switch(e.Button.Tag.ToString()) {
				case "Save":
					Save_Click();
					break;
				case "SaveDraft":
					SaveDraft_Click();
					break;
				case "Cancel":
					Cancel_Click();
					break;
				case "Int Link": 
					Int_Link_Click(); 
					break;
				case "Bookmark":
					Bookmark_Click();
					break;
				case "File Link":
					File_Link_Click();
					break;
				case "Folder Link":
					Folder_Link_Click();
					break;
				case "Ext Link": 
					Ext_Link_Click(); 
					break;
				case "H1": 
					H1_Click(); 
					break;
				case "H2": 
					H2_Click(); 
					break;
				case "H3": 
					H3_Click(); 
					break;
				case "Table": 
					Table_Click();
					break;
				case "Image":
					Image_Click();
					break;
				case "Lock":
					if(!Security.IsAuthorized(Permissions.WikiAdmin,true)) {
						return;
					}
					WikiPageCur.IsLocked=!WikiPageCur.IsLocked;
					if(WikiPageCur.IsLocked) {
						//The wiki page is was locked, switch the icon to the unlock symbol because they locked it.
						e.Button.Text=Lan.g(this,"Unlock");
						e.Button.ImageIndex=19;
					}
					else {
						//The wiki page is was unlocked, switch the icon to the lock symbol because they unlocked it.
						e.Button.Text=Lan.g(this,"Lock");
						e.Button.ImageIndex=20;
					}
					ToolBarMain.Invalidate();
					break;
			}
		}

		private void toolBar2_ButtonClick(object sender,ODToolBarButtonClickEventArgs e) {
			switch(e.Button.Tag.ToString()) {
				case "Cut":
					Cut_Click();
					break;
				case "Copy":
					Copy_Click();
					break;
				case "Paste":
					Paste_Click();
					break;
				case "Undo":
					Undo_Click();
					break;
				case "Bold":
					Bold_Click();
					break;
				case "Italic":
					Italic_Click();
					break;
				case "Color":
					Color_Click();
					break;
				case "Font":
					Font_Click();
					break;
			}
		}

		private void menuItemCut_Click(object sender,EventArgs e) {
			Cut_Click();
		}

		private void menuItemCopy_Click(object sender,EventArgs e) {
			Copy_Click();
		}

		private void menuItemPaste_Click(object sender,EventArgs e) {
			Paste_Click();
		}

		private void menuItemUndo_Click(object sender,EventArgs e) {
			Undo_Click();
		}

		private void WikiSaveEvent_Fired(ODEventArgs e) {
			if(e.Name=="ForceSaveWiki") {
				SaveDraft_Click(false);
			}
		}

		private void Save_Click() {
      if(!ValidateWikiPage(true)) {
        return;
      }
      if(_isInvalidPreview) {
        MsgBox.Show(this,"This page is in an invalid state and cannot be saved.");
        return;
      }
			WikiPage wikiPageDB=WikiPages.GetByTitle(WikiPageCur.PageTitle);
			if(wikiPageDB!=null && WikiPageCur.DateTimeSaved<wikiPageDB.DateTimeSaved) {
				if(WikiPageCur.IsDraft) {
					if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"The wiki page has been edited since this draft was last saved.  Overwrite and continue?")) {
						return;
					}
				}
				else if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This page has been modified and saved since it was opened on this computer.  Save anyway?")) {
					return;
				}
			}
			List<string> listInvalidWikiPages=WikiPages.GetMissingPageTitles(textContent.Text);
			string message=Lan.g(this,"This page has the following invalid wiki page link(s):")+"\r\n"+string.Join("\r\n",listInvalidWikiPages.Take(10))+"\r\n"
				+(listInvalidWikiPages.Count>10 ? "...\r\n\r\n" : "\r\n")
				+Lan.g(this,"Create the wikipage(s) automatically?");
			if(listInvalidWikiPages.Count!=0 && MsgBox.Show(this,MsgBoxButtons.YesNo,message)) {
				foreach(string title in listInvalidWikiPages) {
					WikiPage wp=new WikiPage();
					wp.PageTitle=title;
					wp.UserNum=Security.CurUser.UserNum;
					wp.PageContent="[["+WikiPageCur.WikiPageNum+"]]\r\n"//link back
						+"<h1>"+title+"</h1>\r\n";//page title
					WikiPages.InsertAndArchive(wp);
				}
			}
			WikiPageCur.PageContent=WikiPages.ConvertTitlesToPageNums(textContent.Text);
			WikiPageCur.UserNum=Security.CurUser.UserNum;
			Regex regex=new Regex(@"\[\[(keywords:).+?\]\]");//only grab first match
			Match m=regex.Match(textContent.Text);
			WikiPageCur.KeyWords=m.Value.Replace("[[keywords:","").TrimEnd(']');//will be empty string if no match
			if(WikiPageCur.IsDraft) {
				WikiPages.DeleteDraft(WikiPageCur); //remove the draft from the database.
				WikiPageCur.IsDraft=false; //no longer a draft
				if(wikiPageDB!=null) {//wikiPageDb should never be null, otherwise there was no way to get to this draft.
					//We need to set the draft copy of the wiki page to the WikiPageNum of the real page.  
					//This is because the draft is the most up to date copy of the wiki page, and is about to be saved, however, we want to maintain any
					//links there may be to the real wiki page, since we link by WikiPageNum instead of PageTitle (see JobNum 4429 for more info)
					WikiPageCur.WikiPageNum=wikiPageDB.WikiPageNum;
				}
			}
			WikiPages.InsertAndArchive(WikiPageCur);
			//If the form was given an action, fire it.
			_onWikiSaved?.Invoke(WikiPageCur.PageTitle);
			FormWiki formWiki=(FormWiki)this.OwnerForm;
			if(formWiki!=null && !formWiki.IsDisposed) {
				formWiki.RefreshPage(WikiPageCur.PageTitle);
			}
			HasSaved=true;
			Close();//should be dialog result??
		}

		///<summary>Saves the the currently edited Wikipage as a draft. This method is copied from Save_Click with a few modifications.</summary>
		private void SaveDraft_Click(bool showMsgBox=true) {
			if(showMsgBox && WikiPageCur.IsNew) {
				MsgBox.Show(this,"You may not save a new Wiki page as a draft.  Save the Wiki page, then create a draft.");
				return;
			}
			if(!ValidateWikiPage(true,showMsgBox)) {
				return;
			}
      if(showMsgBox && _isInvalidPreview) {
        MsgBox.Show(this,"This page is in an invalid state and cannot be saved as a draft.");
        return;
      }
			WikiPageCur.PageContent=WikiPages.ConvertTitlesToPageNums(textContent.Text);
			WikiPageCur.UserNum=Security.CurUser.UserNum;
			Regex regex=new Regex(@"\[\[(keywords:).+?\]\]");//only grab first match
			Match m=regex.Match(textContent.Text);
			WikiPageCur.KeyWords=m.Value.Replace("[[keywords:","").TrimEnd(']');//will be empty string if no match
			if(WikiPageCur.IsDraft) { //If it's already a draft, overwrite the current one.
				WikiPageCur.DateTimeSaved=DateTime.Now;
				try {
					WikiPages.UpdateDraft(WikiPageCur);
				}
				catch (Exception ex){
					//should never happen due to the if Draft check above.
					if(showMsgBox) {
						MessageBox.Show(ex.Message);
					}
					return;
				}
			}
			else { //otherwise, set it as a draft, then insert it.
				WikiPageCur.IsDraft=true;
				WikiPages.InsertAsDraft(WikiPageCur);
			}
			//HasSaved not set so that the user will stay in FormWikiDrafts when this window closes, causing the grid to update.
			Close();
		}

		private void Cancel_Click() {
			Close();
		}

		private void Cut_Click() {
			textContent.Cut();
			textContent.Focus();
			//RefreshHtml();
		}

		private void Copy_Click() {
			textContent.Copy();
			textContent.Focus();
			//RefreshHtml();
		}

		private void Paste_Click() {
			textContent.Paste();
			textContent.Focus();
			//RefreshHtml();
		}

		private void Undo_Click() {
			textContent.Undo();
			textContent.Focus();
			//RefreshHtml();
		}

		private void Int_Link_Click() {
			FormWikiAllPages FormWAPSelect = new FormWikiAllPages();
			FormWAPSelect.OwnerForm=this;
			FormWAPSelect.Show();
		}

		private void Bookmark_Click() {
			FormWikiExternalLink FormWEL=new FormWikiExternalLink(Lan.g(this,"Insert Internal Bookmark"),Lan.g(this,"ID"));
			FormWEL.ShowDialog();
			int tempStart=textContent.SelectionStart;
			if(FormWEL.DialogResult!=DialogResult.OK) {
				return;
			}
			if(FormWEL.URL=="" && FormWEL.DisplayText=="") {
				textContent.SelectionLength=0;
				textContent.SelectedText="<a href=\"#\"></a>\n<div id=\"\"></div>";
				textContent.SelectionStart=tempStart+12;
				textContent.SelectionLength=0;
			}
			else {
				textContent.SelectionLength=0;
				textContent.SelectedText="<a href=\"#"+FormWEL.URL+"\">"+FormWEL.DisplayText+"</a>\n<div id=\""+FormWEL.URL+"\"></div>";
			}
			textContent.Focus();
		}

		private void File_Link_Click() {
			string fileLink;
			if(CloudStorage.IsCloudStorage) {
				FormFilePicker FormFP=new FormFilePicker(WikiPages.GetWikiPath());
				FormFP.DoHideLocalButton=true;
				FormFP.ShowDialog();
				if(FormFP.DialogResult!=DialogResult.OK) {
					return;
				}
				fileLink=FormFP.SelectedFiles[0];
				textContent.SelectedText="[[filecloud:"+fileLink+"]]";
			}
			else {//Not cloud
				FormWikiFileFolder FormWFF=new FormWikiFileFolder();
				FormWFF.ShowDialog();
				if(FormWFF.DialogResult!=DialogResult.OK) {
					return;
				}
				fileLink=FormWFF.SelectedLink;
				textContent.SelectedText="[[file:"+fileLink+"]]";
			}
			textContent.SelectionLength=0;
			//RefreshHtml();
		}

		private void Folder_Link_Click() {
			if(CloudStorage.IsCloudStorage) {
				FormFilePicker FormFP=new FormFilePicker(CloudStorage.PathTidy(WikiPages.GetWikiPath()));
				FormFP.DoHideLocalButton=true;
				FormFP.ShowDialog();
				if(FormFP.DialogResult!=DialogResult.OK) {
					return;
				}
				textContent.SelectedText="[[foldercloud:"+FormFP.SelectedFiles[0]+"]]";
			}
			else {
				FormWikiFileFolder formWFF=new FormWikiFileFolder();
				formWFF.IsFolderMode=true;
				formWFF.ShowDialog();
				if(formWFF.DialogResult!=DialogResult.OK) {
					return;
				}
				textContent.SelectedText="[[folder:"+formWFF.SelectedLink+"]]";
			}
			textContent.SelectionLength=0;
			//RefreshHtml();
		}

		private void Ext_Link_Click() {
			FormWikiExternalLink FormWEL=new FormWikiExternalLink();
			FormWEL.ShowDialog();
			int tempStart=textContent.SelectionStart;
			if(FormWEL.DialogResult!=DialogResult.OK) {
				return;
			}
			if(FormWEL.URL=="" && FormWEL.DisplayText=="") {
				textContent.SelectionLength=0;
				textContent.SelectedText="<a href=\"\"></a>";
				textContent.SelectionStart=tempStart+11;
				textContent.SelectionLength=0;
			}
			else {
				textContent.SelectionLength=0;
				textContent.SelectedText="<a href=\""+FormWEL.URL+"\">"+FormWEL.DisplayText+"</a>";
			}
			textContent.Focus();
		}

		private void H1_Click() {
			int tempStart=textContent.SelectionStart;
			int tempLength=textContent.SelectionLength;
			string s="<h1>"+textContent.SelectedText+"</h1>";
			textContent.SelectedText=s;
			textContent.Focus();
			if(tempLength==0) {//nothing selected, place cursor in middle of new tags
				textContent.SelectionStart=tempStart+4+tempLength;
			}
			else {
				textContent.SelectionStart=tempStart+s.Length;
				textContent.SelectionLength=0;
			}
			//RefreshHtml();
		}

		private void H2_Click() {
			int tempStart=textContent.SelectionStart;
			int tempLength=textContent.SelectionLength;
			string s="<h2>"+textContent.SelectedText+"</h2>";
			textContent.SelectedText=s;
			textContent.Focus();
			if(tempLength==0) {//nothing selected, place cursor in middle of new tags
				textContent.SelectionStart=tempStart+4+tempLength;
			}
			else {
				textContent.SelectionStart=tempStart+s.Length;
				textContent.SelectionLength=0;
			}
			//RefreshHtml();
		}

		private void H3_Click() {
			int tempStart=textContent.SelectionStart;
			int tempLength=textContent.SelectionLength;
			string s="<h3>"+textContent.SelectedText+"</h3>";
			textContent.SelectedText=s;
			textContent.Focus();
			if(tempLength==0) {//nothing selected, place cursor in middle of new tags
				textContent.SelectionStart=tempStart+4+tempLength;
			}
			else {
				textContent.SelectionStart=tempStart+s.Length;
				textContent.SelectionLength=0;
			}
			//RefreshHtml();
		}

		private void Bold_Click() {
			int tempStart=textContent.SelectionStart;
			int tempLength=textContent.SelectionLength;
			string s="<b>"+textContent.SelectedText+"</b>";
			textContent.SelectedText=s;
			textContent.Focus();
			if(tempLength==0) {//nothing selected, place cursor in middle of new tags
				textContent.SelectionStart=tempStart+3+tempLength;
			}
			else {
				textContent.SelectionStart=tempStart+s.Length;
				textContent.SelectionLength=0;
			}
			//RefreshHtml();
		}

		private void Italic_Click() {
			int tempStart=textContent.SelectionStart;
			int tempLength=textContent.SelectionLength;
			string s="<i>"+textContent.SelectedText+"</i>";
			textContent.SelectedText=s;
			textContent.Focus();
			if(tempLength==0) {//nothing selected, place cursor in middle of new tags
				textContent.SelectionStart=tempStart+3+tempLength;
			}
			else {
				textContent.SelectionStart=tempStart+s.Length;
				textContent.SelectionLength=0;
			}
			//RefreshHtml();
		}

		private void Color_Click() {
			int tempStart=textContent.SelectionStart;
			int tempLength=textContent.SelectionLength;
			string s="[[color:red|"+textContent.SelectedText+"]]";
			textContent.SelectedText=s;
			textContent.Focus();
			if(tempLength==0) {//nothing selected, place cursor in middle of new tags
				textContent.SelectionStart=tempStart+12+tempLength;
			}
			else {
				textContent.SelectionStart=tempStart+s.Length;
				textContent.SelectionLength=0;
			}
			//RefreshHtml();
		}

		private void Font_Click() {
			int tempStart=textContent.SelectionStart;
			int tempLength=textContent.SelectionLength;
			string s="[[font:courier|"+textContent.SelectedText+"]]";
			textContent.SelectedText=s;
			textContent.Focus();
			if(tempLength==0) {//nothing selected, place cursor in middle of new tags
				textContent.SelectionStart=tempStart+15+tempLength;
			}
			else {
				textContent.SelectionStart=tempStart+s.Length;
				textContent.SelectionLength=0;
			}
			//RefreshHtml();
		}

		///<summary>Works for a new table and for an existing table.</summary>
		private void Table_Click() {
			int idx=textContent.SelectionStart;
			if(TableOrDoubleClick(idx)) {
				return;//so it was already handled with an edit table dialog
			}
			//User did not click inside a table, so they must want to add a new table.
			FormWikiTableEdit FormWTE=new FormWikiTableEdit();
			FormWTE.Markup=@"{|
!Width=""100""|Heading1!!Width=""100""|Heading2!!Width=""100""|Heading3
|-
|||||
|-
|||||
|}";
			FormWTE.IsNew=true;
			FormWTE.ShowDialog();
			if(FormWTE.DialogResult!=DialogResult.OK){
				return;
			}
			textContent.SelectionLength=0;
			textContent.SelectedText=FormWTE.Markup;
			textContent.SelectionLength=0;
			textContent.Focus();
		}

		private void Image_Click() {
			//if storing images in database, GetWikiPath will throw an exception, cannot be storing in database to use images in the wiki
			try {
				WikiPages.GetWikiPath();
			}
			catch(Exception ex) {
				MessageBox.Show(this,ex.Message);
				return;
			}
			FormWikiImages FormWI = new FormWikiImages();
			FormWI.ShowDialog();
			if(FormWI.DialogResult!=DialogResult.OK) {
				return;
			}
			textContent.SelectionLength=0;
			textContent.SelectedText="[[img:"+FormWI.SelectedImageName+"]]";
			//webBrowserWiki.AllowNavigation=true;
			//RefreshHtml();
		}

		///<summary>Validates content, and keywords.  isForSaving can be false if just validating for refresh.</summary>
		private bool ValidateWikiPage(bool isForSaving,bool showMsgBox=true) {
			//xml validation----------------------------------------------------------------------------------------------------
			string s=textContent.Text;
			//"<",">", and "&"-----------------------------------------------------------------------------------------------------------
			s=s.Replace("&","&amp;");
			s=s.Replace("&amp;<","&lt;");//because "&" was changed to "&amp;" in the line above.
			s=s.Replace("&amp;>","&gt;");//because "&" was changed to "&amp;" in the line above.
			s="<body>"+s+"</body>";
			XmlDocument doc=new XmlDocument();
			StringReader reader=new StringReader(s);
			try {
				doc.Load(reader);
			}
			catch(Exception ex){
				if(showMsgBox) {
					MessageBox.Show(ex.Message);
				}
				return false;
			}
			try{
				//we do it this way to skip checking the main node itself since it's a dummy node.
				ValidateNodes(doc.DocumentElement.ChildNodes);
			}
			catch(Exception ex){
				if(showMsgBox) {
					MessageBox.Show(ex.Message);
				}
				return false;
			}
			//Cannot have CR within tag definition---------------------------------------------------------------------------------
			//(?<!&) means only match strings that do not start with an '&'. This is so we can continue to use '&' as an escape character for '<'.
			//<.*?> means anything as short as possible that is contained inside a tag
			MatchCollection tagMatches=Regex.Matches(textContent.Text,"(?<!&)<.*?>",RegexOptions.Singleline);
			for(int i=0;i<tagMatches.Count;i++) {
				if(tagMatches[i].ToString().Contains("\n")) {
					if(showMsgBox) {
						MessageBox.Show(Lan.g(this,"Error at line:")+" "+textContent.GetLineFromCharIndex(tagMatches[i].Index)+" - "
							+Lan.g(this,"Tag definitions cannot contain a return line:")+" "+tagMatches[i].Value.Replace("\n",""));
					}
					return false;
				}
			}
			//image validation-----------------------------------------------------------------------------------------------------
			string wikiImagePath="";
			try {
				wikiImagePath=WikiPages.GetWikiPath();//this also creates folder if it's missing.
			}
			catch(Exception ex) {
				ex.DoNothing();
				//do nothing, the wikiImagePath is only important if the user adds an image to the wiki page and is checked below
			}
			MatchCollection matches=Regex.Matches(textContent.Text,@"\[\[(img:).*?\]\]");// [[img:myimage.jpg]]
			if(matches.Count>0 && PrefC.AtoZfolderUsed==DataStorageType.InDatabase) {
				if(showMsgBox) {
					MessageBox.Show(Lan.g(this,"Error at line:")+" "+textContent.GetLineFromCharIndex(matches[0].Index)+" - "
						+Lan.g(this,"Cannot use images in wiki if storing images in database."));
				}
				return false;
			}
			if(isForSaving) {
				for(int i=0;i<matches.Count;i++) {
					string imgPath=FileAtoZ.CombinePaths(wikiImagePath,matches[i].Value.Substring(6).Trim(']'));
					if(!FileAtoZ.Exists(imgPath)) {
						if(showMsgBox) {
							MessageBox.Show(Lan.g(this,"Error at line:")+" "+textContent.GetLineFromCharIndex(matches[i].Index)+" - "
								+Lan.g(this,"Not allowed to save because image does not exist:")+" "+imgPath);
						}
						return false;
					}
				}
			}
			//List validation-----------------------------------------------------------------------------------------------------
			matches=Regex.Matches(textContent.Text,@"\[\[(list:).*?\]\]");// [[list:CustomList]]
			foreach(Match match in matches) {
				if(!WikiLists.CheckExists(match.Value.Substring(7).Trim(']'))) {
					if(showMsgBox) {
						MessageBox.Show(Lan.g(this,"Error at line:")+" "+textContent.GetLineFromCharIndex(match.Index)+" - "
							+Lan.g(this,"Wiki list does not exist in database:")+" "+match.Value.Substring(7).Trim(']'));
					}
					return false;
				}
			}
			//spacing around bullets-----------------------------------------------------------------------------------------------
			string[] lines=textContent.Text.Split(new string[] { "\n" },StringSplitOptions.None);
			for(int i=0;i<lines.Length;i++) {
				if(lines[i].Trim().StartsWith("*")) {
					if(!lines[i].StartsWith("*")) {
						if(showMsgBox) {
							MessageBox.Show(Lan.g(this,"Error at line:")+" "+(i+1)+" - "
								+Lan.g(this,"Stars used for lists may not have a space before them."));
						}
						return false;
					}
					if(lines[i].Trim().StartsWith("* ")) {
						if(showMsgBox) {
							MessageBox.Show(Lan.g(this,"Error at line:")+" "+(i+1)+" - "
								+Lan.g(this,"Stars used for lists may not have a space after them."));
						}
						return false;
					}
				}
				if(lines[i].Trim().StartsWith("#")) {
					if(!lines[i].StartsWith("#")) {
						if(showMsgBox) {
							MessageBox.Show(Lan.g(this,"Error at line:")+" "+(i+1)+" - "
								+Lan.g(this,"Hashes used for lists may not have a space before them."));
						}
						return false;
					}
					if(lines[i].Trim().StartsWith("# ")) {
						if(showMsgBox) {
							MessageBox.Show(Lan.g(this,"Error at line:")+" "+(i+1)+" - "
								+Lan.g(this,"Hashes used for lists may not have a space after them."));
						}
						return false;
					}
				}
			}
			//Invalid characters inside of various tags--------------------------------------------
			matches=Regex.Matches(textContent.Text,@"\[\[.*?\]\]");
			foreach(Match match in matches) {
				if(match.Value.Contains("\"") && !match.Value.StartsWith("[[color:") && !match.Value.StartsWith("[[font:")) {//allow colored text to have quotes.
					if(showMsgBox) {
						MessageBox.Show(Lan.g(this,"Error at line:")+" "+textContent.GetLineFromCharIndex(match.Index)+" - "
							+Lan.g(this,"Link cannot contain double quotes:")+" "+match.Value);
					}
					return false;
				}
				//This is not needed because our regex doesn't even catch them if the span a line break.  It's just interpreted as plain text.
				//if(match.Value.Contains("\r") || match.Value.Contains("\n")) {
				//	MessageBox.Show(Lan.g(this,"Link cannot contain carriage returns: ")+match.Value);
				//	return false;
				//}
				if(match.Value.StartsWith("[[img:")
					|| match.Value.StartsWith("[[keywords:")
					|| match.Value.StartsWith("[[file:")
					|| match.Value.StartsWith("[[folder:")
					|| match.Value.StartsWith("[[list:")
					|| match.Value.StartsWith("[[color:")
 					|| match.Value.StartsWith("[[font:"))
				{
					//other tags
				}
				else {
					if(match.Value.Contains("|")) {
						if(showMsgBox) {
							MessageBox.Show(Lan.g(this,"Error at line:")+" "+textContent.GetLineFromCharIndex(match.Index)+" - "
								+Lan.g(this,"Internal link cannot contain a pipe character:")+" "+match.Value);
						}
						return false;
					}
				}
			}
			//Table markup rigorously formatted----------------------------------------------------------------------
			//{|
			//!Width="100"|Column Heading 1!!Width="150"|Column Heading 2!!Width="75"|Column Heading 3
			//|- 
			//|Cell 1||Cell 2||Cell 3 
			//|-
			//|Cell A||Cell B||Cell C 
			//|}
			//Although this is rarely needed, it might still come in handy in certain cases, like paste, or when user doesn't add the |} until later, and other hacks.
			matches = Regex.Matches(s,@"\{\|\n.+?\n\|\}",RegexOptions.Singleline);
			//matches = Regex.Matches(textContent.Text,
			//	@"(?<=(?:\n|<body>))" //Checks for preceding newline or beggining of file
			//	+@"\{\|.+?\n\|\}" //Matches the table markup.
			//	+@"(?=(?:\n|</body>))" //Checks for following newline or end of file
			//	,RegexOptions.Singleline);
			foreach(Match match in matches) {
				lines=match.Value.Split(new string[] { "{|\n","\n|-\n","\n|}" },StringSplitOptions.RemoveEmptyEntries);
				if(!lines[0].StartsWith("!")) {
					if(showMsgBox) {
						MessageBox.Show(Lan.g(this,"Error at line:")+" "+textContent.GetLineFromCharIndex(match.Index)+" - "
							+Lan.g(this,"The second line of a table markup section must start with ! to indicate column headers."));
					}
					return false;
				}
				if(lines[0].StartsWith("! ")) {
					if(showMsgBox) {
						MessageBox.Show(Lan.g(this,"Error at line:")+" "+textContent.GetLineFromCharIndex(match.Index)+" - "
							+Lan.g(this,"In the table, at line 2, there cannot be a space after the first !"));
					}
					return false;
				}
				string[] cells=lines[0].Substring(1).Split(new string[] { "!!" },StringSplitOptions.None);//this also strips off the leading !
				for(int c=0;c<cells.Length;c++) {
					if(!Regex.IsMatch(cells[c],@"^(Width="")\d+""\|")) {//e.g. Width="90"| 
						if(showMsgBox) {
							MessageBox.Show(Lan.g(this,"Error at line:")+" "+textContent.GetLineFromCharIndex(match.Index)+" - "
							+Lan.g(this,"In the table markup, each header must be formatted like this: Width=\"#\"|..."));
						}
						return false;
					}
				}
				for(int i=1;i<lines.Length;i++) {//loop through the lines after the header
					if(!lines[i].StartsWith("|")) {
						if(showMsgBox) {
							MessageBox.Show(Lan.g(this,"Table rows must start with |.  At line ")+(i+1).ToString()+Lan.g(this,", this was found instead:")+lines[i]);
						}
						return false;
					}
				}
			}
			return true;  
		}

		///<summary>Recursive.</summary>
		private void ValidateNodes(XmlNodeList nodes) {
			foreach(XmlNode node in nodes) {
				if(node.NodeType==XmlNodeType.Comment) {
					throw new ApplicationException("The comment tag <!-- --> "+node.Name+" is not allowed.");
				}
				if(node.NodeType==XmlNodeType.ProcessingInstruction) {
					throw new ApplicationException("The XML processing instruction <?xml ?> "+node.Name+" is not allowed.");
				}
				if(node.NodeType==XmlNodeType.XmlDeclaration) {
					throw new ApplicationException("XML declarations like <?xml ?> "+node.Name+"> are not allowed.");
				}
				if(node.NodeType!=XmlNodeType.Element){
					continue;
				}
				//check child nodes for nested duplicate
				switch(node.Name) {
					case "i":
					case "b":
					case "h1":
					case "h2":
					case "h3":
						//These are all valid nodes that are allowed.
						break;
					case "div":
						//The only thing div is used for right now is to designate bookmarks within the page.
						//Therefore we require that there be one and only one attribute and that attribute can only be "id".
						if(node.Attributes.Count!=1 || node.Attributes[0].Name!="id") {
							throw new ApplicationException(Lan.g(this,"All <div> tags MUST be identified by the 'id' attribute."));
						}
						break;
					case "a":
						//a is an allowed node but can only have one attribute; href
						for(int i=0;i<node.Attributes.Count;i++) {
							if(node.Attributes[i].Name!="href") {
								throw new ApplicationException(node.Attributes[i].Name+" attribute is not allowed on <a> tag.");
							}
							//We know the only attribute is "href", make sure the user didn't manually type out a "wiki" link using <a>.  They need to use [[ ]].
							if(node.Attributes[i].InnerText.StartsWith("wiki:")) {
								throw new ApplicationException("wiki: is not allowed in an <a> tag.  Use [[ ]] instead of <a>.");
							}
						}
						break;
					case "img":
						throw new ApplicationException("Image tags are not allowed. Instead use [[img: ... ]]");
					default:
						throw new ApplicationException("<"+node.Name+"> is not one of the allowed tags. To display as plain text, escape the brackets with ampersands. I.e. \"&<"+node.Name+"&>\"");
				}
				ValidateNodes(node.ChildNodes);
				ValidateDuplicateNesting(node.Name,node.ChildNodes);
			}
		}

		///<summary>Recursive.</summary>
		private void ValidateDuplicateNesting(string nodeName,XmlNodeList nodes) {
			foreach(XmlNode node in nodes) {
				if(node.NodeType!=XmlNodeType.Element) {
					continue;
				}
				if(nodeName==node.Name) {
					throw new ApplicationException("There are multiple <"+node.Name+"> tags nested within each other.  Remove the unneeded tags.");
				}
				ValidateDuplicateNesting(nodeName,node.ChildNodes);
			}
		}

		private void FormWikiEdit_FormClosing(object sender,FormClosingEventArgs e) {
			//handles both the Cancel button and the user clicking on the x, and also the save button.
			WikiSaveEvent.Fired-=WikiSaveEvent_Fired;
			if(HasSaved) {
				return;
			}
			if(!WikiPageCur.IsNew && textContent.Text!=WikiPages.GetWikiPageContentWithWikiPageTitles(WikiPageCur.PageContent)){
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Unsaved changes will be lost. Would you like to continue?")) {
					e.Cancel=true;
				}
			}
		}
	}
}