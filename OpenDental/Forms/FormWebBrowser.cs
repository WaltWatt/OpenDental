using CodeBase;
using OpenDental.UI;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OpenDental {
	///<summary>A wrapper form to display an Internet browser within an Open Dental window.
	///This is essentially a Microsoft Internet Explorer control embedded into our form.
	///The browser.ScriptErrorsSuppressed is true in order to prevent javascript error popups from annoying the user.</summary>
	public partial class FormWebBrowser:ODForm {
		///<summary>Set to the value passed into the constructor if one was passed in.  Navigates the browser control to this url on load.</summary>
		private string _urlBrowseTo="";

		///<summary>Used when opening a new browser window via a link.</summary>
		public FormWebBrowser(string url="") {
			InitializeComponent();
			Lan.F(this);
			_urlBrowseTo=url;
			SHDocVw.WebBrowser axBrowser=(SHDocVw.WebBrowser)browser.ActiveXInstance;
			if(axBrowser!=null) {//This was null once during testing.  Not sure when null can happen.  Not sure if we should allow the user to continue.
				axBrowser.NewWindow2+=axBrowser_NewWindow2;
				axBrowser.NewWindow3+=axBrowser_NewWindow3;
			}
			browser.DocumentTitleChanged+=browser_DocumentTitleChanged;
		}

		private void FormWebBrowser_Load(object sender,EventArgs e) {
			Cursor=Cursors.WaitCursor;//Is set back to default cursor after the document loads inside the browser.
			Application.DoEvents();//To show cursor change.
			Text=Lan.g(this,"Loading")+"...";
			LayoutToolBars();
			if(_urlBrowseTo!="") { //Use the window as a simple web browswer when a URL is passed in.
				browser.Navigate(_urlBrowseTo);
				Cursor=Cursors.Default;
				return;
			}
		}

		///<summary></summary>
		public void LayoutToolBars() {
			ToolBarMain.Buttons.Clear();
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Back"),0,"","Back"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Forward"),1,"","Forward"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Refresh"),-1,"","Refresh"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Print"),-1,"","Print"));
			ToolBarMain.Buttons.Add(new ODToolBarButton(Lan.g(this,"Close"),-1,"","Close"));
			ToolBarMain.Invalidate();
		}

		///<summary>Linked up to the browser in the designer.
		///This event fires when a link is clicked within the webbrowser control which opens in a new window.
		///The browser.IsWebBrowserContextMenuEnabled is set to false to disable the popup menu that shows up when right clicking on links or images,
		///because right clicking a link and choosing to open in a new window causes this function to fire but the destination URL is unknown and thus
		///we cannot handle that situation.  Best to hide the context menu since there is little or no need for it.</summary>
		private void browser_NewWindow(object sender,CancelEventArgs e) {
			CreateNewWindow(browser.StatusText);//This is the URL of the page that is supposed to open in a new window.
			e.Cancel=true;//Cancel Internet Explorer from launching.
		}

		///<summary>This event fires when a javascript snippet calls window.open() to open a URL in a new	browser window.
		///When window.open() is called, our browser_NewWindow() event function does not fire.</summary>
		private void axBrowser_NewWindow2(ref object ppDisp,ref bool Cancel) {
			//We could not get this event to fire in testing.  Here just in case we need it.
			CreateNewWindow(browser.StatusText);//This is the URL of the page that is supposed to open in a new window.
			Cancel=true;//Cancel Internet Explorer from launching.
		}

		///<summary>We are not sure when this event function fires, but we implemented it just in case.</summary>
		void axBrowser_NewWindow3(ref object ppDisp,ref bool Cancel,uint dwFlags,string bstrUrlContext,string bstrUrl) {
			//We could not get this event to fire in testing.  Here just in case we need it.
			CreateNewWindow(bstrUrl);
			Cancel=true;//Cancel Internet Explorer from launching.
		}

		///<summary>This helper function is called any time a new browser window needs to be opened.  By default, new windows launched by clicking a link
		///from within the webbrowser control will open in Internet Explorer, even if the system default is another web browser such as Mozilla.  We had a
		///problem with cookies not being carried over from our webbrowser control into Internet Explorer when a link is clicked.  To preserve cookies, we
		///intercept the new window creation, cancel it, then launch the destination URL in a new OD browser window.  Cancel the new window creation
		///inside the calling event.</summary>
		private void CreateNewWindow(string url) {
			//For example, the "ScureScripts Drug History" link within the "Compose Rx" tab.
			if(Regex.IsMatch(url,"^.*javascript\\:.*$",RegexOptions.IgnoreCase)) {//Ignore tab clicks because the user is not navigating to a new page.
				return;
			}
			FormWebBrowser formNew=new FormWebBrowser(url);//Open the page in a new window, but stay inside of OD.
			formNew.WindowState=FormWindowState.Normal;
			formNew.Show();//Non-modal, so that we get the effect of opening in an independent window.
		}

		///<summary>Called after a document has finished loading, including initial page load and when Back and Forward buttons are pressed.</summary>
		public void browser_DocumentCompleted(object sender,WebBrowserDocumentCompletedEventArgs e) {
			Cursor=Cursors.Default;
			SetTitle();
		}

		private void browser_DocumentTitleChanged(object sender,EventArgs e) {
			SetTitle();
		}

		///<summary>Sets the text of the form to the browser's DocumentTitle.</summary>
		protected virtual void SetTitle() {
			Text=browser.DocumentTitle;
		}

		private void ToolBarMain_ButtonClick(object sender,ODToolBarButtonClickEventArgs e) {
			switch(e.Button.Tag.ToString()) {
				case "Back":
					if(browser.CanGoBack) {
						Cursor=Cursors.WaitCursor;//Is set back to default cursor after the document loads inside the browser.
						Application.DoEvents();//To show cursor change.
						Text=Lan.g(this,"Loading")+"...";
						browser.GoBack();
					}
					break;
				case "Forward":
					if(browser.CanGoForward) {
						Cursor=Cursors.WaitCursor;//Is set back to default cursor after the document loads inside the browser.
						Application.DoEvents();//To show cursor change.
						Text=Lan.g(this,"Loading")+"...";
						browser.GoForward();
					}
					break;
				case "Refresh":
					browser.Refresh();
					break;
				case "Print":
					browser.ShowPrintDialog();
					break;
				case "Close":
					DialogResult=DialogResult.Cancel;
					Close();//For when we launch the window in a non-modal manner.
					break;
			}
		}

	}
}