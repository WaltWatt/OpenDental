using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using OpenDentBusiness;
using NHunspell;
using System.Text.RegularExpressions;
using OpenDental.UI;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using CodeBase;

namespace OpenDental {
	/// <summary>This is used instead of a regular textbox when quickpaste functionality is needed.</summary>
	public class ODtextBox:RichTextBox {//System.ComponentModel.Component

		#region IMM (Input Method Manager) dll import for IME mid composition bug (Korea and east Asian languages)

		//See post by Jon Burchel - Microsoft: https://goo.gl/d1ehJb  (an MSDN forum post shortened using google url shortener)

		[DllImport("imm32.dll",CharSet = CharSet.Unicode)]
		public static extern IntPtr ImmReleaseContext(IntPtr hWnd,IntPtr context);

		[DllImport("imm32.dll",CharSet = CharSet.Unicode)]
		private static extern int ImmGetCompositionString(IntPtr hIMC,uint dwIndex,byte[] lpBuf,int dwBufLen);

		[DllImport("imm32.dll",CharSet = CharSet.Unicode)]
		private static extern IntPtr ImmGetContext(IntPtr hWnd);

		#endregion

		private System.Windows.Forms.ContextMenu contextMenu;
		private IContainer components;// Required designer variable.
		private static Hunspell HunspellGlobal;//We create this object one time for every instance of this textbox control within the entire program.
		private QuickPasteType quickPasteType;
		public List<string> ListCorrect;
		public List<string> ListIncorrect;
		private Graphics BufferGraphics;
		public Timer timerSpellCheck;
		private Point PositionOfClick;
		public MatchOD ReplWord;
		private bool _spellCheckIsEnabled;//set to true in constructor
		private bool _editMode;//set to false in constructor
		private Point textEndPoint;
		///<summary>Only used when ImeCompositionCompatibility is enabled.  Set to true when the user presses the space bar.
		///This will cause the cursor to move to the next position and no longer have composition affect the current character.
		///E.g. the Korean symbol '역' (dur) will display.  However, typing '여' (du) and then space will cause that char to no longer be affected.
		///This will allow the char 'ㄱ' (r) to appear after '여' instead of '역'.</summary>
		private bool _skipImeComposition=false;
		///<summary>Only used when ImeCompositionCompatibility is enabled.  Set to true when the user is in the middle of composing a symbol.
		///This will cause the cursor to stay over the current character and not move on (or separate) the current symbol being constructed.
		///E.g. the Korean symbol '역' (dur) will not display correctly without this set to true.  
		///When false, it will be broken apart into each character that comprizes it: 'ㅇ ㅕ ㄱ ' (d u r)</summary>
		private bool _imeComposing=false;
		///<summary>Always contains the text that should be displayed in the rich text box.  
		///Also used to store the UNICODE representation of the RichTextBox.Text property (which we override) due to a Korean bug.</summary>
		private string _msgText="";
		///<summary>Pointer to the Rich Edit version 4.1 dll.  Null unless the property () is set to true and the new version is loaded.
		///The new dll is named Msftedit.dll and the ClassName is changed from RichEdit20W to RichEdit50W.
		///The new dll has been available since Windows XP SP1, released in 2002.  .NET is just set to use the old dll by default.</summary>
		private static IntPtr _libPtr;
		private bool isImeComposition;
		private bool _detectLinksEnabled;
		///<summary>Must track menuitems that we have added, so that we know which ones to take away when reconstructing context menu.</summary>
		private List<MenuItem> _listMenuItemLinks;

		[Category("Behavior"), Description("Set true to enable context menu to detect links.")]
		[DefaultValue(true)]
		public bool DetectLinksEnabled {
			get {
				return _detectLinksEnabled;
			}
			set {
				_detectLinksEnabled=value;
			}
		}

		///<summary>Set true to enable spell checking in this control.</summary>
		[Category("Behavior"),Description("Set true to enable spell checking.")]
		[DefaultValue(true)]
		public bool SpellCheckIsEnabled {
			get {
				return _spellCheckIsEnabled;
			}
			set {
				_spellCheckIsEnabled=value;
			}
		}

		///<summary>Set true to enable formatted text to paste in this control.</summary>
		[Category("Behavior"),Description("Set true to allow edit mode formatting")]
		[DefaultValue(false)]
		public bool EditMode {
			get {
				return _editMode;
			}
			set {
				_editMode=value;
			}
		}

		///<summary>Set true to enable the newer version 4.1 RichEdit library.</summary>
		[Category("Behavior"), Description("Set true to enable RichEdit version 4.1 enhanced features.")]
		[DefaultValue(false)]
		public bool RichEdit4IsEnabled { get; set; }

		[DllImport("kernel32.dll",EntryPoint="LoadLibrary",CharSet=CharSet.Auto,SetLastError=true)]
		private static extern IntPtr LoadLibrary(string fileName);

		///<summary>By default .NET uses the old library, riched20.dll, which corresponds to Rich Edit versions 2.0 and 3.0.
		///As of Windows XP SP1 (2002 release date), the newer library, msftedit.dll, is included which corresponds to Rich Edit version 4.1.
		///This method attempts to load the newer library, with the enhanced features, and sets the ClassName accordingly.
		///If msftedit.dll is not found, the original default library is used.
		///The msftedit.dll library is only loaded if the libPtr==IntPtr.Zero to prevent memory leaks.</summary>
		protected override CreateParams CreateParams {
			get {
				CreateParams cParams=base.CreateParams;
				if(!RichEdit4IsEnabled) {
					return cParams;
				}
				try {
					if(_libPtr==IntPtr.Zero) {//only try to load the library if not loaded already.
						_libPtr=LoadLibrary("msftedit.dll");
					}
					if(_libPtr==IntPtr.Zero) {//still zero, library was not loaded successfully
						return cParams;
					}
					cParams.ClassName="RichEdit50W";//old ClassName: "RichEdit20W" new ClassName: "RichEdit50W"
				}
				catch(Exception) {
					//msftedit.dll must not exist, or LoadLibrary wasn't loaded, so simply return the base.CreateParams unaltered
				}
				return cParams;
			}
		}

		/*public ODtextBox(System.ComponentModel.IContainer container)
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			container.Add(this);
			InitializeComponent();

		}*/

		///<summary></summary>
		public ODtextBox() {
			//We have to try catch this just in case an ODTextBox is shown before upgrading to a version that already has this preference.
			try {
				if(System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime
					|| this.DesignMode || !Db.HasDatabaseConnection) 
				{
					isImeComposition=false;
				}
				else {
					isImeComposition=PrefC.GetBool(PrefName.ImeCompositionCompatibility);
				}
			}
			catch(Exception ex) {
				ex.DoNothing();//Do nothing.  Just treat the ODTextBox like it always has (no composition support).
			}
			InitializeComponent();// Required for Windows.Forms Class Composition Designer support
			_spellCheckIsEnabled=true;
			this.AcceptsTab=true;//Causes CR to not also trigger OK button on a form when that button is set as AcceptButton on the form.
			this.DetectUrls=false;
			if(System.ComponentModel.LicenseManager.UsageMode!=System.ComponentModel.LicenseUsageMode.Designtime
				&& HunspellGlobal==null) {
#if DEBUG
				try {
					HunspellGlobal=new Hunspell(Properties.Resources.en_US_aff,Properties.Resources.en_US_dic);
				}
				catch(Exception ex) {
					ex.DoNothing();
					System.IO.File.Copy(@"..\..\..\Required dlls\Hunspellx64.dll","Hunspellx64.dll");
					System.IO.File.Copy(@"..\..\..\Required dlls\Hunspellx86.dll","Hunspellx86.dll");
					HunspellGlobal=new Hunspell(Properties.Resources.en_US_aff,Properties.Resources.en_US_dic);
				}
#else
				HunspellGlobal=new Hunspell(Properties.Resources.en_US_aff,Properties.Resources.en_US_dic);
#endif
			}
			ListCorrect=new List<string>();
			ListCorrect.Add("\n");
			ListCorrect.Add("\t");
			ListIncorrect=new List<string>();
			EventHandler onClick=new EventHandler(menuItem_Click);
			contextMenu.MenuItems.Add("",onClick);//These five menu items will hold the suggested spelling for misspelled words.  If no misspelled words, they will not be visible.
			contextMenu.MenuItems.Add("",onClick);
			contextMenu.MenuItems.Add("",onClick);
			contextMenu.MenuItems.Add("",onClick);
			contextMenu.MenuItems.Add("",onClick);
			contextMenu.MenuItems.Add("-");
			contextMenu.MenuItems.Add(Lan.g(this,"Add to Dictionary"),onClick);
			contextMenu.MenuItems.Add(Lan.g(this,"Disable Spell Check"),onClick);
			contextMenu.MenuItems.Add("-");
			contextMenu.MenuItems.Add(new MenuItem(Lan.g(this,"Insert Date"),onClick,Shortcut.CtrlD));
			contextMenu.MenuItems.Add(new MenuItem(Lan.g(this,"Insert Quick Note"),onClick,Shortcut.CtrlQ));
			contextMenu.MenuItems.Add("-");
			contextMenu.MenuItems.Add(new MenuItem(Lan.g(this,"Cut"),onClick,Shortcut.CtrlX));
			contextMenu.MenuItems.Add(new MenuItem(Lan.g(this,"Copy"),onClick,Shortcut.CtrlC));
			contextMenu.MenuItems.Add(new MenuItem(Lan.g(this,"Paste"),onClick,Shortcut.CtrlV));
			contextMenu.MenuItems.Add(new MenuItem(Lan.g(this,"Paste Plain Text"),onClick));
			contextMenu.MenuItems.Add(new MenuItem(Lan.g(this,"Edit AutoNote"),onClick));
			base.BackColor=SystemColors.Window;//Needed for OnReadOnlyChanged() to change backcolor when ReadOnly because of an issue with RichTextBox.
		}

		///<summary>Clean up any resources being used.</summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(components != null) {
					components.Dispose();
				}
				if(BufferGraphics!=null) {//Dispose before bitmap.
					BufferGraphics.Dispose();
					BufferGraphics=null;
				}
				//We do not dispose the hunspell object because it will be automatially disposed of when the program closes.
			}
			base.Dispose(disposing);
		}


		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.timerSpellCheck = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// contextMenu
			// 
			this.contextMenu.Popup += new System.EventHandler(this.contextMenu_Popup);
			// 
			// timer1
			// 
			this.timerSpellCheck.Interval = 500;
			this.timerSpellCheck.Tick += new System.EventHandler(this.timerSpellCheck_Tick);
			// 
			// ODtextBox
			// 
			this.ContextMenu = this.contextMenu;
			this.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.ContentsResized += new System.Windows.Forms.ContentsResizedEventHandler(this.ODtextBox_ContentsResized);
			this.VScroll += new System.EventHandler(this.ODtextBox_VScroll);
			this.ResumeLayout(false);

		}
		#endregion

		///<summary>Statuses sent to an application when IME is doing 'composition' for a symbol.
		///See post by Jon Burchel - Microsoft: https://goo.gl/d1ehJb  (an MSDN forum post shortened using google url shortener)</summary>
		private enum WM_IME {
			GCS_RESULTSTR=0x800,
			EM_STREAMOUT=0x044A,
			WM_IME_COMPOSITION=0x10F,
			WM_IME_ENDCOMPOSITION=0x10E,
			WM_IME_STARTCOMPOSITION=0x10D
		}

		protected override void WndProc(ref Message m) {
			//The following code fixes a bug deep down in RichTextBox for foreign users that have a language that needs to use composition symbols.
			//See post by Jon Burchel - Microsoft: https://goo.gl/d1ehJb  (an MSDN forum post shortened using google url shortener)
			if(isImeComposition) {
				switch(m.Msg) {
					case (int)WM_IME.EM_STREAMOUT:
						if(_imeComposing) {
							_skipImeComposition=true;
						}
						base.WndProc(ref m);
						break;
					case (int)WM_IME.WM_IME_COMPOSITION:
						if(m.LParam.ToInt32()==(int)WM_IME.GCS_RESULTSTR) {
							IntPtr hImm=ImmGetContext(this.Handle);
							int dwSize=ImmGetCompositionString(hImm,(int)WM_IME.GCS_RESULTSTR,null,0);
							byte[] outstr=new byte[dwSize];
							ImmGetCompositionString(hImm,(int)WM_IME.GCS_RESULTSTR,outstr,dwSize);
							_msgText+=Encoding.Unicode.GetString(outstr).ToString();
							ImmReleaseContext(this.Handle,hImm);
						}
						if(_skipImeComposition) {
							_skipImeComposition=false;
							break;
						}
						base.WndProc(ref m);
						break;
					case (int)WM_IME.WM_IME_STARTCOMPOSITION:
						_imeComposing=true;
						base.WndProc(ref m);
						break;
					case (int)WM_IME.WM_IME_ENDCOMPOSITION:
						_imeComposing=false;
						base.WndProc(ref m);
						break;
					default:
						base.WndProc(ref m);
						break;
				}
			}
			else {//End IME check.
				base.WndProc(ref m);
			}
		}


		protected override void OnReadOnlyChanged(EventArgs e) {
			base.OnReadOnlyChanged(e);
			//Richtextbox does not redraw the textbox with grey after turning it ReadOnly, so we do this to immitate how textbox works.
			if(ReadOnly){
				base.BackColor=SystemColors.Control;
			}
			else{
				base.BackColor=SystemColors.Window;
			}
		}

		public override string Text {
			get {
				if(!_imeComposing) {
					_msgText=base.Text;
					return base.Text;
				}
				else {
					return _msgText;
				}
			}
			set {
				_msgText=value;
				base.Text=value;
			}
		}

		///<summary></summary>
		[Category("Behavior"),Description("This will determine which category of Quick Paste notes opens first.")]
		public QuickPasteType QuickPasteType {
			get {
				return quickPasteType;
			}
			set {
				quickPasteType=value;
				if(value==QuickPasteType.None) {
					throw new InvalidEnumArgumentException("A value for the QuickPasteType property must be set.");
				}
			}
		}

		private void contextMenu_Popup(object sender,System.EventArgs e) {
			if(SelectionLength==0) {
				contextMenu.MenuItems[12].Enabled=false;//cut
				contextMenu.MenuItems[13].Enabled=false;//copy
			}
			else {
				contextMenu.MenuItems[12].Enabled=true;
				contextMenu.MenuItems[13].Enabled=true;
			}
			if(!this._spellCheckIsEnabled
			  || !PrefC.GetBool(PrefName.SpellCheckIsEnabled)
			  || !IsOnMisspelled(PositionOfClick)) {//did not click on a misspelled word OR spell check is disabled
				contextMenu.MenuItems[0].Visible=false;//suggestion 1
				contextMenu.MenuItems[1].Visible=false;//suggestion 2
				contextMenu.MenuItems[2].Visible=false;//suggestion 3
				contextMenu.MenuItems[3].Visible=false;//suggestion 4
				contextMenu.MenuItems[4].Visible=false;//suggestion 5
				contextMenu.MenuItems[5].Visible=false;//contextMenu separator
				contextMenu.MenuItems[6].Visible=false;//Add to Dictionary
				contextMenu.MenuItems[7].Visible=false;//Disable Spell Check
				contextMenu.MenuItems[8].Visible=false;//separator
			}
			else if(this._spellCheckIsEnabled
			  && PrefC.GetBool(PrefName.SpellCheckIsEnabled)
			  && IsOnMisspelled(PositionOfClick)) {//clicked on or near a misspelled word AND spell check is enabled
				List<string> suggestions=SpellSuggest();
				if(suggestions.Count==0) {//no suggestions
					contextMenu.MenuItems[0].Text=Lan.g(this,"No Spelling Suggestions");
					contextMenu.MenuItems[0].Visible=true;
					contextMenu.MenuItems[0].Enabled=false;//suggestion 1 set to "No Spelling Suggestions"
					contextMenu.MenuItems[1].Visible=false;//suggestion 2
					contextMenu.MenuItems[2].Visible=false;//suggestion 3
					contextMenu.MenuItems[3].Visible=false;//suggestion 4
					contextMenu.MenuItems[4].Visible=false;//suggestion 5
				}
				else {//must be on misspelled word and spell check is enabled globally and locally
					for(int i=0;i<5;i++) {//Only display first 5 suggestions if available
						if(i>=suggestions.Count) {
							contextMenu.MenuItems[i].Visible=false;
							continue;
						}
						contextMenu.MenuItems[i].Text=suggestions[i];
						contextMenu.MenuItems[i].Visible=true;
						contextMenu.MenuItems[i].Enabled=true;
					}
				}
				contextMenu.MenuItems[5].Visible=true;//contextMenu separator, will display whether or not there is a suggestion for the misspelled word
				contextMenu.MenuItems[6].Visible=true;//Add to Dictionary
				contextMenu.MenuItems[7].Visible=true;//Disable Spell Check
				contextMenu.MenuItems[8].Visible=true;//contextMenu separator
			}
			if(EditMode) {
				contextMenu.MenuItems[15].Visible=true;//Paste Plain Text
				contextMenu.MenuItems[15].Enabled=true;
			}
			else {
				contextMenu.MenuItems[15].Visible=false;//Paste Plain Text
				contextMenu.MenuItems[15].Enabled=false;
			}
			string textCur=(SelectedText!=""?SelectedText:Text);
			if(Regex.IsMatch(textCur,@"\[Prompt:""[a-zA-Z_0-9 ]+""\]")) {
				contextMenu.MenuItems[16].Visible=true;
				contextMenu.MenuItems[16].Enabled=true;
			}
			else {
				contextMenu.MenuItems[16].Visible=false;
				contextMenu.MenuItems[16].Enabled=false;
			}
			if(!PrefC.GetBool(PrefName.WikiDetectLinks)) {//NOTE: if this preference is changed while the program is open there MAY be some lingering wiki links in the context menu. 
				//It is not worth it to force users to log off and back on again, or to run the link removal code below EVERY time, even if the pref is disabled.
				return;
			}
			if(_listMenuItemLinks==null) {
				_listMenuItemLinks=new List<MenuItem>();
			}
			_listMenuItemLinks.ForEach(x => contextMenu.MenuItems.Remove(x));//remove items from previous pass.
			_listMenuItemLinks.Clear();
			_listMenuItemLinks.Add(new MenuItem("-"));
			MatchCollection matches=Regex.Matches(Text,@"\[\[.+?]]");//wiki links.
			foreach(Match match in matches) {
				_listMenuItemLinks.Add(new MenuItem("Wiki - "+match.Value.Trim('[').Trim(']'),(s,eArg) => { OpenWikiPage(match.Value.Trim('[').Trim(']')); }));
			}
			_listMenuItemLinks=_listMenuItemLinks.OrderByDescending(x=>x.Text=="-").ThenBy(x => x.Text).ToList();//alphabetize the link items.
			if(_listMenuItemLinks.Any(x => x.Text!="-")) {//at least one REAL menu item that is not the divider.
				_listMenuItemLinks.ForEach(x => contextMenu.MenuItems.Add(x));
			}
		}

		private static void OpenWikiPage(string pageTitle) {
			FormOpenDental.S_WikiLoadPage(pageTitle);
			//WikiPages.NavPageDelegate(pageTitle); //this also works, but makes an extra hop to the business layer
		}

		///<summary>Determines whether the right click was on a misspelled word.  Also sets the start and end index of chars to be replaced in text.</summary>
		private bool IsOnMisspelled(Point PositionOfClick) {
			int charIndex=this.GetCharIndexFromPosition(PositionOfClick);
			Point charLocation=this.GetPositionFromCharIndex(charIndex);
			if(PositionOfClick.Y<charLocation.Y-2 || PositionOfClick.Y>charLocation.Y+this.FontHeight+2) {//this is the closest char but they were not very close when they right clicked
				return false;
			}
			char c=this.GetCharFromPosition(PositionOfClick);
			if(c=='\n') {//if closest char is a new line char, then assume not on a misspelled word
				return false;
			}
			List<MatchOD> words=GetWords();
			if(words.Count==0) {
				return false;
			}
			int ind=0;
			#region Binary search to find first word in visible area
			int minIndex=0;
			int maxIndex=words.Count-1;
			ind=maxIndex;
			while(maxIndex > minIndex) {
				if(this.GetPositionFromCharIndex(words[ind].StartIndex).Y<0) {//words[ind] is above the visible area, so make ind our new minimum index
					minIndex=ind;
				}
				else if(this.GetPositionFromCharIndex(words[ind].StartIndex).Y>this.Height) {//words[ind] is beyond the visible area, so make ind our new maximum index
					maxIndex=ind;
				}
				else {
					break;
				}
				ind=maxIndex-((maxIndex-minIndex)/2);//set ind to be the halfway point between max and min
				if(ind==maxIndex || ind==minIndex) {//this will occur if there is no word in the visible area, break out of loop
					break;
				}
			}
			#endregion
			if(this.GetPositionFromCharIndex(words[ind].StartIndex).Y>0 && this.GetPositionFromCharIndex(words[ind].StartIndex).Y<=this.Height) {//if words[ind] is in visible area
				while(ind>0 && this.GetPositionFromCharIndex(words[ind-1].StartIndex).Y>0) {
					ind--;//backup to first visible word
				}
			}
			for(int i=ind;i<words.Count;i++) {
				if(this.GetPositionFromCharIndex(words[i].StartIndex).Y>this.Height) {
					ReplWord=null;
					break;
				}
				if(charIndex>=words[i].StartIndex && charIndex<=(words[i].StartIndex+words[i].Value.Length-1)) {
					ReplWord=words[i];
					break;
				}
			}
			if(ReplWord==null) {
				return false;
			}
			if(ListIncorrect.Contains(ReplWord.Value)) {
				return true;
			}
			return false;
		}

		private List<string> SpellSuggest() {
			List<string> suggestions=HunspellGlobal.Suggest(ReplWord.Value);
			return suggestions;
		}

		protected override void OnMouseDown(MouseEventArgs e) {
			if(!this.Focused) {
				this.Focus();
			}
			base.OnMouseDown(e);
			PositionOfClick=new Point(e.X,e.Y);
		}

		private void menuItem_Click(object sender,System.EventArgs e) {
			if(ReadOnly && contextMenu.MenuItems.IndexOf((MenuItem)sender)!=13) {
				MsgBox.Show(this,"This feature is currently disabled due to this text box being read only.");
				return;
			}
			switch(contextMenu.MenuItems.IndexOf((MenuItem)sender)) {
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
					if(!this._spellCheckIsEnabled || !PrefC.GetBool(PrefName.SpellCheckIsEnabled)) {//if spell check disabled, break.  Should never happen since the suggested words won't show if spell check disabled
						break;
					}
					int originalCaret=this.SelectionStart;
					this.SelectionStart=ReplWord.StartIndex;
					this.SelectionLength=ReplWord.Value.Length;
					this.SelectedText=((MenuItem)sender).Text;
					if(this.Text.Length<=originalCaret) {
						this.SelectionStart=this.Text.Length;
					}
					else {
						this.SelectionStart=originalCaret;
					}
					timerSpellCheck.Start();
					break;
				//case 5 is separator
				case 6://Add to dict
					if(!this._spellCheckIsEnabled || !PrefC.GetBool(PrefName.SpellCheckIsEnabled)) {//if spell check disabled, break.  Should never happen since Add to Dict won't show if spell check disabled
						break;
					}
					string newWord=ReplWord.Value;
					//guaranteed to not already exist in custom dictionary, or it wouldn't be underlined.
					DictCustom word=new DictCustom();
					word.WordText=newWord;
					DictCustoms.Insert(word);
					DataValid.SetInvalid(InvalidType.DictCustoms);
					//removes all case versions of the word from incorrect list to avoid duplication
					for(int i = ListIncorrect.Count-1;i>=0;i--) {
						if(ListIncorrect[i].ToLower()==ReplWord.Value.ToLower()) {
							ListCorrect.Add(ListIncorrect[i]);
							ListIncorrect.Remove(ListIncorrect[i]);
						}
					}
					break;
				case 7://Disable spell check
					if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"This will disable spell checking.  To re-enable, go to Setup | Spell Check and check the \"Spell Check Enabled\" box.")) {
						break;
					}
					Prefs.UpdateBool(PrefName.SpellCheckIsEnabled,false);
					DataValid.SetInvalid(InvalidType.Prefs);
					ClearWavyLines();
					break;
				//case 8 is separator
				case 9:
					InsertDate();
					break;
				case 10://Insert Quick Note
					ShowFullDialog();
					break;
				//case 11 is separator
				case 12://cut
					base.Cut();
					break;
				case 13://copy
					base.Copy();
					break;
				case 14://paste
					if(EditMode) {
						PasteText();
					}
					else {
						PastePlainText();
					}
					break;
				case 15://paste plain text
					PastePlainText();
					break;
				case 16://Edit AutoNote
					EditAutoNote();
					break;
			}
		}

		///<summary>Pastes the content of the clipboard including the formatting.</summary>
		private void PasteText() {
			base.Paste();
		}

		///<summary>Pastes the content of the clipboard excluding the formatting.</summary>
		private void PastePlainText() {
			SelectionFont=Font;
			SelectionColor=ForeColor;
			SelectionBackColor=Color.Transparent;
			SelectionBullet=false;
			base.Paste(DataFormats.GetFormat("Text"));
		}

		///<summary></summary>
		private void EditAutoNote() {
			FormAutoNoteCompose FormA=new FormAutoNoteCompose();
			FormA.MainTextNote=SelectedText!=""?SelectedText:Text;
			FormA.ShowDialog();
			if(FormA.DialogResult==DialogResult.OK) {
				if(SelectedText!=""){
					SelectedText=FormA.CompletedNote;
				}
				else {
					Text=FormA.CompletedNote;
					SelectionStart=Text.Length;
				}
			}
		}

		private void timerSpellCheck_Tick(object sender,EventArgs e) {
			if(!this._spellCheckIsEnabled || !PrefC.GetBool(PrefName.SpellCheckIsEnabled)) {//if spell check disabled, return
				return;
			}
			timerSpellCheck.Stop();
			SpellCheck();
		}

		private void ODtextBox_VScroll(object sender,EventArgs e) {
			if(!this._spellCheckIsEnabled || !PrefC.GetBool(PrefName.SpellCheckIsEnabled)) {//if spell check disabled, return
				return;
			}
			timerSpellCheck.Stop();
			timerSpellCheck.Start();
		}

		protected override void OnKeyDown(KeyEventArgs e) {
			base.OnKeyDown(e);
			if(!this._spellCheckIsEnabled || !PrefC.GetBool(PrefName.SpellCheckIsEnabled)) {//if spell check disabled, return
				return;
			}
			//The lines were shifted due to new input. This causes the location of the red wavy underline to shift down as well, so clear them.
			if(e.KeyCode==Keys.Enter) {
				ClearWavyLines();
			}
		}

		///<summary>When the contents of the text box is resized, e.g. when word wrap creates a new line, clear red wavy lines so they don't shift down.</summary>
		private void ODtextBox_ContentsResized(object sender,ContentsResizedEventArgs e) {
			try {
				if(DesignMode || !this._spellCheckIsEnabled || !PrefC.GetBool(PrefName.SpellCheckIsEnabled)) {//if spell check disabled, return
					return;
				}
			}
			catch {
				//This can only happen if designing and DesignMode is false for some reason.  Has happened when this control is two levels deep.
				//The exception happens in PrefC.GetBool() because there is no database connection in design time.
				return;
			}
			Point textEndPointCur=this.GetPositionFromCharIndex(Text.Length-1);
			if(textEndPoint==new Point(0,0)) {
				textEndPoint=textEndPointCur;
				return;
			}
			if(textEndPointCur.Y!=textEndPoint.Y) {//textEndPoint cannot be null, if not set it defaults to 0,0
				ClearWavyLines();
			}
			textEndPoint=textEndPointCur;
		}

		///<summary></summary>
		protected override void OnKeyUp(KeyEventArgs e) {
			base.OnKeyUp(e);
			if(this._spellCheckIsEnabled && PrefC.GetBool(PrefName.SpellCheckIsEnabled)) {//Only spell check if enabled
				timerSpellCheck.Stop();
			}
			int originalLength=Text.Length;
			int originalCaret=base.SelectionStart;
			string newText=QuickPasteNotes.Substitute(Text,quickPasteType);
			if(Text!=newText) {
				Text=newText;
				SelectionStart=originalCaret+Text.Length-originalLength;
			}
			//then CtrlQ
			if(e.KeyCode==Keys.Q && e.Modifiers==Keys.Control) {
				ShowFullDialog();
			}
			if(this._spellCheckIsEnabled && PrefC.GetBool(PrefName.SpellCheckIsEnabled)) {//Only spell check if enabled
				timerSpellCheck.Start();
			}
		}

		public void ClearWavyLines() {
			if(this.Width <= 0 || this.Height <= 0) {//Width or Height can be 0 if the window or textbox is resized.  Causes a UE when creating a Bitmap. 
				return;
			}
			Bitmap bitmapOverlay=new Bitmap(this.Width,this.Height);
			BufferGraphics=Graphics.FromImage(bitmapOverlay);
			BufferGraphics.Clear(Color.Transparent);//We don't want to overwrite the text in the rich text box.
			Graphics graphicsTextBox=Graphics.FromHwnd(this.Handle);
			//split by spaces
			MatchCollection mc=Regex.Matches(Text,@"(\S+)");//use Regex.Matches because our matches include the index within our text for underlining
			if(mc.Count==0) {//all text was deleted, clear the entire text box
				Rectangle wavyLineArea=new Rectangle(1,1,this.Width,this.Height);
				BufferGraphics.FillRectangle(new SolidBrush(this.BackColor),wavyLineArea);
				graphicsTextBox.DrawImageUnscaled(bitmapOverlay,0,0);
				graphicsTextBox.Dispose();
				bitmapOverlay.Dispose();
				return;
			}
			int ind=0;
			if(mc.Count==1) {
				//ind=0;//just clear the line our only word is on
			}
			else if(this.GetPositionFromCharIndex(0).Y>0 && this.GetPositionFromCharIndex(0).Y<=this.Height) {
				//ind=0;//the first 'word' is visible, just clear starting here
			}
			else {
				if(this.GetPositionFromCharIndex(0).Y<0 && this.GetPositionFromCharIndex(mc[mc.Count-1].Index).Y<0) {//all text above visible area, just return
					bitmapOverlay.Dispose();
					return;
				}
				if(this.GetPositionFromCharIndex(0).Y>this.Height && this.GetPositionFromCharIndex(mc[mc.Count-1].Index).Y>this.Height) {//all text beyond visible area
					bitmapOverlay.Dispose();
					return;
				}
				#region Binary search to find first word in visible area
				int minIndex=0;
				int maxIndex=mc.Count-1;
				ind=maxIndex;
				while(maxIndex > minIndex) {
					if(this.GetPositionFromCharIndex(mc[ind].Index).Y<0) {//mc[ind] is above the visible area, so make ind our new minimum index
						minIndex=ind;
					}
					else if(this.GetPositionFromCharIndex(mc[ind].Index).Y>this.Height) {//mc[ind] is beyond the visible area, so make ind our new maximum index
						maxIndex=ind;
					}
					else {
						break;
					}
					ind=maxIndex-((maxIndex-minIndex)/2);//set ind to be the halfway point between max and min
					if(ind==maxIndex || ind==minIndex) {//this will occur if there is no word in the visible area, break out of loop
						break;
					}
				}
				#endregion
				if(this.GetPositionFromCharIndex(mc[ind].Index).Y>0 && this.GetPositionFromCharIndex(mc[ind].Index).Y<=this.Height) {//if mc[ind] is in visible area
					while(ind>0 && this.GetPositionFromCharIndex(mc[ind-1].Index).Y>0) {
						ind--;//backup to first visible word
					}
				}
			}
			for(int i=ind;i<mc.Count;i++) {
				Point start=this.GetPositionFromCharIndex(mc[i].Index);//get pos of character at index of match
				Point end=this.GetPositionFromCharIndex(mc[i].Index+mc[i].Value.Length-1);//get pos of the char at the end of this word, to see if the 'word' spans more than one line
				start.Y=start.Y+this.FontHeight;
				end.Y=end.Y+this.FontHeight;
				if(start.Y>=this.Height) {//y pos is below the visible area, with scroll bar active
					break;
				}
				//word may span more than one line, so white out all lines between the starting char line and the ending char line
				for(int j=start.Y;j<=end.Y;j+=this.FontHeight) {
					Rectangle wavyLineArea=new Rectangle(1,j,this.Width,2);
					BufferGraphics.FillRectangle(new SolidBrush(this.BackColor),wavyLineArea);
				}
			}
			graphicsTextBox.DrawImageUnscaled(bitmapOverlay,0,0);
			graphicsTextBox.Dispose();
			bitmapOverlay.Dispose();
		}

		///<summary>Performs spell checking against indiviudal words against the English USA dictionary.</summary>
		private void SpellCheck() {
			//Only spell check if enabled
			if(!this._spellCheckIsEnabled 
				|| !PrefC.GetBool(PrefName.SpellCheckIsEnabled)
				|| PrefC.GetBool(PrefName.ImeCompositionCompatibility))
			{
				//Do not spell check languages that use composition.  If needed in the future, fix the bug where the first char disapears in the box.
				//E.g. go into an ODTextBox, set language input to Korean, and simply type the letter 'ㅇ' (d) and wait.  It will disapear.
				return;
			}
			if(this.Width <= 0 || this.Height <= 0) {//Width or Height can be 0 if the window or textbox is resized.  Causes a UE when creating a Bitmap. 
				return;
			}
			ClearWavyLines();
			Bitmap bitmapOverlay=new Bitmap(this.Width,this.Height);
			BufferGraphics=Graphics.FromImage(bitmapOverlay);
			BufferGraphics.Clear(Color.Transparent);//We don't want to overwrite the text in the rich text box.
			List<MatchOD> words=GetWords();
			if(words.Count==0) {
				bitmapOverlay.Dispose();
				return;
			}
			int ind=0;
			if(words.Count==1) {
				//ind=0;//just clear the line our only word is on
			}
			else if(this.GetPositionFromCharIndex(0).Y>0 && this.GetPositionFromCharIndex(0).Y<=this.Height) {
				//ind=0;//the first 'word' is visible, just clear starting here
			}
			else {
				if(this.GetPositionFromCharIndex(0).Y<0 && this.GetPositionFromCharIndex(words[words.Count-1].StartIndex).Y<0) {//all text above visible area, just return
					bitmapOverlay.Dispose();
					return;
				}
				if(this.GetPositionFromCharIndex(0).Y>this.Height && this.GetPositionFromCharIndex(words[words.Count-1].StartIndex).Y>this.Height) {//all text beyond visible area
					bitmapOverlay.Dispose();
					return;
				}
				#region binary search to find first word in visible area
				int minIndex=0;
				int maxIndex=words.Count-1;
				ind=maxIndex;
				while(maxIndex > minIndex) {
					if(this.GetPositionFromCharIndex(words[ind].StartIndex).Y<0) {//words[ind] is above the visible area, so make ind our new minimum index
						minIndex=ind;
					}
					else if(this.GetPositionFromCharIndex(words[ind].StartIndex).Y>this.Height) {//words[ind] is beyond the visible area, so make ind our new maximum index
						maxIndex=ind;
					}
					else {
						break;
					}
					ind=maxIndex-((maxIndex-minIndex)/2);//set ind to be the halfway point between max and min
					if(ind==maxIndex || ind==minIndex) {//this will occur if there is no word in the visible area, break out of loop
						break;
					}
				}
				#endregion
				if(this.GetPositionFromCharIndex(words[ind].StartIndex).Y>0 && this.GetPositionFromCharIndex(words[ind].StartIndex).Y<=this.Height) {//if words[ind] is in visible area
					while(ind>0 && this.GetPositionFromCharIndex(words[ind-1].StartIndex).Y>0) {
						ind--;//backup to first visible word
					}
				}
			}
			for(int i=ind;i<words.Count;i++) {
				if(this.GetPositionFromCharIndex(words[i].StartIndex).Y>=this.Height) {
					break;//stop spell checking once we go beyond visible area
				}
				if(ListCorrect.Contains(words[i].Value)) {
					continue;
				}
				if(ListCorrect.Contains(words[i].Value.ToLower())) {//word as they typed it was not in the correct list, but lower case version is, see if the casing as they typed it is correct by Hunspell (google is incorrect but Google is not)
					if(HunspellGlobal.Spell(words[i].Value)) {
						ListCorrect.Add(words[i].Value);//add to appropriate list with the casing as typed this time
						continue;
					}
				}
				bool correct=false;
				int startIndex=words[i].StartIndex;//words[i].StartIndex is relative to Text.
				int endIndex=startIndex+words[i].Value.Length;//One spot past the end of the word, because DrawWave() draws to the beginning of the character of the endIndex.
				if(ListIncorrect.Contains(words[i].Value)) {
					DrawWave(startIndex,endIndex);
					continue;
				}
				if(ListIncorrect.Contains(words[i].Value.ToLower())) {//word as typed is not in incorrect list, but lower-case version is, see if this casing is correct
					if(HunspellGlobal.Spell(words[i].Value)) {
						ListCorrect.Add(words[i].Value);//add to approriate list with casing this time
						continue;
					}
					else {
						ListIncorrect.Add(words[i].Value);
						DrawWave(startIndex,endIndex);
						continue;
					}
				}
				DictCustom dictCustom=DictCustoms.GetFirstOrDefault(x => x.WordText.ToLower()==words[i].Value.ToLower());
				if(dictCustom!=null) {
					correct=true;
				}
				if(!correct) {//Not in custom dictionary, so spell check
					correct=HunspellGlobal.Spell(words[i].Value);
				}
				if(!correct) {
					DrawWave(startIndex,endIndex);
					ListIncorrect.Add(words[i].Value);
				}
				else {//if it gets here, the word was spelled correctly, determined by comparing to the custom word list and/or the hunspell dict
					ListCorrect.Add(words[i].Value);
				}
			}
			Graphics graphicsTextBox=Graphics.FromHwnd(this.Handle);
			graphicsTextBox.DrawImageUnscaled(bitmapOverlay,0,0);
			graphicsTextBox.Dispose();
			bitmapOverlay.Dispose();
		}

		///<summary></summary>
		private List<MatchOD> GetWords() {
			List<MatchOD> wordList=new List<MatchOD>();
			MatchCollection mc=Regex.Matches(Text,@"(\S+)");//use Regex.Matches because our matches include the index within our text for underlining
			foreach(Match m in mc) {
				Group g=m.Groups[0];//Group 0 is the entire match
				if(g.Value.Length<2) {//only allow 'words' that are at least two chars long, 1 char 'words' are assumed spelled correctly
					continue;
				}
				MatchOD word=new MatchOD();
				word.StartIndex=g.Index;//this index is the index within Text of the first char of this word (match)
				word.Value=g.Value;
				//loop through starting at the beginning of word looking for first letter or digit
				while(word.Value.Length>1 && !Char.IsLetterOrDigit(word.Value[0])) {
					word.Value=word.Value.Substring(1);
					word.StartIndex++;
				}
				//loop through starting at the last char looking for the last letter or digit
				while(word.Value.Length>1 && !Char.IsLetterOrDigit(word.Value[word.Value.Length-1])) {
					word.Value=word.Value.Substring(0,word.Value.Length-1);
				}
				if(word.Value.Length>1) {
					if(Regex.IsMatch(word.Value,@"[^a-zA-Z\'\-]")) {
						continue;
					}
					wordList.Add(word);
				}
			}
			return wordList;
		}

		private void DrawWave(int startIndex,int endIndex) {
			Point start=this.GetPositionFromCharIndex(startIndex);//accounts for scroll position
			Point end=this.GetPositionFromCharIndex(endIndex);//accounts for scroll position
			start.Y=start.Y+this.FontHeight;//move from top of line to bottom of line
			end.Y=end.Y+this.FontHeight;//move from top of line to bottom of line
			if(start.Y<=4 || start.Y>=this.Height) {//Don't draw lines for text which is currently not visible.
				return;
			}
			Pen pen=Pens.Red;
			if(end.Y>start.Y) {//Mispelled word spans multiple lines
				Point tempEnd=start;
				tempEnd.X=this.Width;
				while(tempEnd.Y<=end.Y) {
					if((tempEnd.X-start.X)>4) {//Only draw wavy line if mispelled word is at least 4 pixels wide, otherwise draw straight line
						ArrayList pl=new ArrayList();
						for(int i=start.X;i<=(tempEnd.X-2);i=i+4) {
							pl.Add(new Point(i,start.Y));
							pl.Add(new Point(i+2,start.Y+1));
						}
						Point[] p=(Point[])pl.ToArray(typeof(Point));
						BufferGraphics.DrawLines(pen,p);
					}
					else {
						BufferGraphics.DrawLine(pen,start,end);
					}
					start.X=1;
					start.Y=start.Y+this.FontHeight;
					tempEnd.Y=start.Y;
					if(tempEnd.Y==end.Y) {//We incremented to the next line and this is the last line of the mispelled word
						tempEnd.X=end.X;
					}
					else {//not the last line of mispelled word, so draw wavy line to end of this line
						tempEnd.X=this.Width;
					}
				}
			}
			else {
				if((end.X-start.X)>4) {
					ArrayList pl=new ArrayList();
					for(int i=start.X;i<=(end.X-2);i=i+4) {
						pl.Add(new Point(i,start.Y));
						pl.Add(new Point(i+2,start.Y+1));
					}
					Point[] p=(Point[])pl.ToArray(typeof(Point));
					BufferGraphics.DrawLines(pen,p);
				}
				else {
					BufferGraphics.DrawLine(pen,start,end);
				}
			}
		}

		private void ShowFullDialog() {
			FormQuickPaste FormQ=new FormQuickPaste();
			FormQ.TextToFill=this;
			FormQ.QuickType=quickPasteType;
			FormQ.ShowDialog();
		}

		private void InsertDate() {
			SelectedText=DateTime.Today.ToShortDateString();
		}

		///<summary>Analogous to a Match.  We use it to keep track of words that we find and their location within the larger string.</summary>
		public class MatchOD {
			//This is the 'word' for this match
			public string Value="";
			//This is the starting index of the first char of the 'word' within the full textbox text
			public int StartIndex=0;
		}

	}

}

