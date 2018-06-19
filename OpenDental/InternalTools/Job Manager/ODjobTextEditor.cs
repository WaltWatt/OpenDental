using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using CodeBase;
using System.Drawing.Text;
using System.ComponentModel;
using System.Linq;

namespace OpenDental {
	///<summary></summary>
	public delegate void ODjobTextEditorSaveEventHandler(object sender,EventArgs e);

	[DefaultEvent("SaveClick")]
	public partial class ODjobTextEditor:UserControl {
		private bool _hasSaveButton;
		private bool _hasEditorOptions;
		private bool _isRequirementsReadOnly;
		private bool _isImplementationReadOnly;
		///<summary>Used to set the button color back after hovering</summary>
		private Color _backColor;
		///<summary>Used when highlighting.</summary>
		private Color _highlightColor;
		private RichTextBox _textFocused;

		///<summary></summary>
		[Category("Appearance"),Description("Toggles whether the control contains a save button or not.")]
		public bool HasSaveButton {
			get {
				return _hasSaveButton;
			}
			set {
				_hasSaveButton=value;
				ODjobTextEditor_Layout(this,null);
				Invalidate();
			}
		}		
		
		///<summary></summary>
		[Category("Appearance"),Description("Toggles whether the control contains the editor toolbar.")]
		public bool HasEditorOptions {
			get {
				return _hasEditorOptions;
			}
			set {
				_hasEditorOptions=value;
				ODjobTextEditor_Layout(this,null);
				Invalidate();
			}
		}

		///<summary></summary>
		[Category("Appearance"),Description("Toggles whether the requirements can be edited.")]
		public bool ReadOnlyRequirements {
			get {
				return _isRequirementsReadOnly;
			}
			set {
				_isRequirementsReadOnly=value;
				ODjobTextEditor_Layout(this,null);
				Invalidate();
			}
		}

		///<summary></summary>
		[Category("Appearance"),Description("Toggles whether the implementation can be edited.")]
		public bool ReadOnlyImplementation {
			get {
				return _isImplementationReadOnly;
			}
			set {
				_isImplementationReadOnly=value;
				ODjobTextEditor_Layout(this,null);
				Invalidate();
			}
		}

		///<summary>Gets or sets the main textbox text.</summary>
		public string RequirementsText {
			get {
				return textRequirements.Text;
			}
			set {
				textRequirements.Text=value;
				ODjobTextEditor_Layout(this,null);
				Invalidate();
			}
		}

		///<summary>Gets or sets the main textbox RTF format text.</summary>
		public string RequirementsRtf {
			get {
				return textRequirements.Rtf;
			}
			set {
				textRequirements.Rtf=value;
				ODjobTextEditor_Layout(this,null);
				Invalidate();
			}
		}

		///<summary>Gets or sets the main textbox text.</summary>
		public string ImplementationText {
			get {
				return textImplementation.Text;
			}
			set {
				textImplementation.Text=value;
				ODjobTextEditor_Layout(this,null);
				Invalidate();
			}
		}

		///<summary>Gets or sets the main textbox RTF format text.</summary>
		public string ImplementationRtf {
			get {
				return textImplementation.Rtf;
			}
			set {
				textImplementation.Rtf=value;
				ODjobTextEditor_Layout(this,null);
				Invalidate();
			}
		}

		[Category("Action"),Description("Occurs when the save button is clicked.")]
		public event ODtextEditorSaveEventHandler SaveClick=null;

		public delegate void textChangedEventHandler();
		[Category("Action"),Description("Occurs as text is changed.")]
		public event textChangedEventHandler OnTextEdited=null;

		public ODjobTextEditor() {
			InitializeComponent();
			_hasSaveButton=true;
		}

		#region Public Methods
		public void Append(string appendText) {
			_textFocused.AppendText(appendText);
		}
		#endregion

		///<summary></summary>
		protected override void OnLoad(EventArgs e) {
			//Fill and set font
			InstalledFontCollection installedFonts=new InstalledFontCollection();
			installedFonts.Families.ToList().ForEach(x => comboFontType.Items.Add(x.Name));
			comboFontType.SelectedIndex=installedFonts.Families.ToList().FindIndex(x => x.Name.Contains("Microsoft Sans Serif"));			
			//Fill and set font size
			Enumerable.Range(7,14).ToList().ForEach(x => comboFontSize.Items.Add(x));//Sizes 7-20
			comboFontSize.SelectedIndex=1;//Size 8;
			butHighlight.BackColor=Color.Yellow;
			butSpellCheck.BackColor=textImplementation.SpellCheckIsEnabled?Color.LightGreen:Color.LightCoral;
			_highlightColor=butHighlight.BackColor;
		}

    private void ODjobTextEditor_Layout(object sender,LayoutEventArgs e) {
			flowLayoutPanel1.Visible=_hasEditorOptions;
			butSave.Visible=_hasSaveButton;
			_textFocused=textRequirements;
      textRequirements.ReadOnly=_isRequirementsReadOnly;
			textImplementation.ReadOnly=_isImplementationReadOnly;
      bool isEnabled=!_isImplementationReadOnly || !_isRequirementsReadOnly;
      butCut.Enabled=isEnabled;
      butCopy.Enabled=isEnabled;
      butPaste.Enabled=isEnabled;
      butUndo.Enabled=isEnabled;
      butRedo.Enabled=isEnabled;
      butBold.Enabled=isEnabled;
      butItalics.Enabled=isEnabled;
      butUnderline.Enabled=isEnabled;
      butStrikeout.Enabled=isEnabled;
      butBullet.Enabled=isEnabled;
      comboFontSize.Enabled=isEnabled;
      comboFontType.Enabled=isEnabled;
			butApplyFont.Enabled=isEnabled;
      butColor.Enabled=isEnabled;
      butColorSelect.Enabled=isEnabled;
      butHighlight.Enabled=isEnabled;
      butHighlightSelect.Enabled=isEnabled;
      butSave.Enabled=isEnabled;
			butSpellCheck.Enabled=isEnabled;
			butClearFormatting.Enabled=isEnabled;
    }

		private void HoverColorEnter(object sender,EventArgs e) {
			System.Windows.Forms.Button btn=(System.Windows.Forms.Button)sender;
			_backColor=btn.BackColor;
			btn.BackColor=Color.PaleTurquoise;
		}

		private void HoverColorLeave(object sender,EventArgs e) {
			System.Windows.Forms.Button btn=(System.Windows.Forms.Button)sender;
			btn.BackColor=_backColor;
		}

		private void butCut_Click(object sender,EventArgs e) {
			if(_textFocused.ReadOnly) {
				return;
			}
			_textFocused.Cut();
			_textFocused.Focus();
		}

		private void butCopy_Click(object sender,EventArgs e) {
			if(_textFocused.ReadOnly) {
				return;
			}
			_textFocused.Copy();
			_textFocused.Focus();
		}

		private void butPaste_Click(object sender,EventArgs e) {
			if(_textFocused.ReadOnly) {
				return;
			}
			_textFocused.Paste();
			_textFocused.Focus();
		}

		private void butUndo_Click(object sender,EventArgs e) {
			if(_textFocused.ReadOnly) {
				return;
			}
			_textFocused.Undo();
			_textFocused.Focus();
		}

		private void butRedo_Click(object sender,EventArgs e) {
			if(_textFocused.ReadOnly) {
				return;
			}
			_textFocused.Redo();
			_textFocused.Focus();
		}

		private void butBold_Click(object sender,EventArgs e) {
			if(_textFocused.ReadOnly) {
				return;
			}
			try {
				_textFocused.SelectionFont=new Font(_textFocused.SelectionFont,_textFocused.SelectionFont.Style ^ FontStyle.Bold);
				_textFocused.Focus();
			}
			catch {
				//labelWarning.Text="Cannot format multiple Fonts";
			}
		}

		private void butItalics_Click(object sender,EventArgs e) {
			if(_textFocused.ReadOnly) {
				return;
			}
			try {
				_textFocused.SelectionFont=new Font(_textFocused.SelectionFont,_textFocused.SelectionFont.Style ^ FontStyle.Italic);
				_textFocused.Focus();
			}
			catch {
				//labelWarning.Text="Cannot format multiple Fonts";
			}
		}

		private void butUnderline_Click(object sender,EventArgs e) {
			if(_textFocused.ReadOnly) {
				return;
			}
			try {
				_textFocused.SelectionFont=new Font(_textFocused.SelectionFont,_textFocused.SelectionFont.Style ^ FontStyle.Underline);
				_textFocused.Focus();
			}
			catch {
				//labelWarning.Text="Cannot format multiple Fonts";
			}
		}

		private void butStrikeout_Click(object sender,EventArgs e) {
			if(_textFocused.ReadOnly) {
				return;
			}
			try {
				_textFocused.SelectionFont=new Font(_textFocused.SelectionFont,_textFocused.SelectionFont.Style ^ FontStyle.Strikeout);
				_textFocused.Focus();
			}
			catch {
				//labelWarning.Text="Cannot format multiple Fonts";
			}
		}

		private void butBullet_Click(object sender,EventArgs e) {
			if(_textFocused.ReadOnly) {
				return;
			}
			if(_textFocused.SelectionBullet) {
				_textFocused.SelectionBullet=false;
				_textFocused.Focus();
			}
			else {
				_textFocused.SelectionBullet=true;
				_textFocused.Focus();
			}
		}

		private void comboFontType_SelectionChangeCommitted(object sender,EventArgs e) {
			if(_textFocused.ReadOnly) {
				return;
			}
			try {
				_textFocused.SelectionFont=new Font((string)comboFontType.SelectedItem,(int)comboFontSize.SelectedItem,_textFocused.SelectionFont.Style);
				_textFocused.Focus();
			}
			catch {

			}
		}

		private void comboFontSize_SelectionChangeCommitted(object sender,EventArgs e) {
			if(_textFocused.ReadOnly) {
				return;
			}
			try {
				_textFocused.SelectionFont=new Font((string)comboFontType.SelectedItem,(int)comboFontSize.SelectedItem,_textFocused.SelectionFont.Style);
				_textFocused.Focus();
			}
			catch {

			}
		}

		private void butApplyFont_Click(object sender,EventArgs e) {
			if(_textFocused.ReadOnly) {
				return;
			}
			try {
				_textFocused.SelectionFont=new Font((string)comboFontType.SelectedItem,(int)comboFontSize.SelectedItem,_textFocused.SelectionFont.Style);
				_textFocused.Focus();
			}
			catch {

			}
		}

		private void butColor_Click(object sender,EventArgs e) {
			if(_textFocused.ReadOnly) {
				return;
			}
			_textFocused.SelectionColor=butColor.ForeColor;
			_textFocused.Focus();
		}

		private void butColorSelect_Click(object sender,EventArgs e) {
			if(_textFocused.ReadOnly) {
				return;
			}
			colorDialog1.Color=butColor.ForeColor;
			colorDialog1.ShowDialog();
			butColor.ForeColor=colorDialog1.Color;
			_textFocused.Focus();
		}

		private void butHighlight_Click(object sender,EventArgs e) {
			if(_textFocused.ReadOnly) {
				return;
			}
			_textFocused.SelectionBackColor=_highlightColor;
			_textFocused.Focus();
		}

		private void butHighlightSelect_Click(object sender,EventArgs e) {
			if(_textFocused.ReadOnly) {
				return;
			}
			colorDialog1.Color=butHighlight.BackColor;
			colorDialog1.ShowDialog();
			butHighlight.BackColor=colorDialog1.Color;
			_highlightColor=colorDialog1.Color;
			_textFocused.Focus();
		}

		///<summary></summary>
		private void butSave_Click(object sender,EventArgs e) {
			SaveText();
		}

		private void SaveText() {
			EventArgs gArgs=new EventArgs();
			if(SaveClick!=null) {
				SaveClick(this,gArgs);
				_textFocused.Focus();
			}
		}

		private void textDescription_TextChanged(object sender,EventArgs e) {
			if(OnTextEdited!=null) {
				OnTextEdited();
			}
		}

		private void butSpellCheck_Click(object sender,EventArgs e) {
			textImplementation.SpellCheckIsEnabled=!textImplementation.SpellCheckIsEnabled;
			textImplementation.Refresh();
			textImplementation.SpellCheck();
			butSpellCheck.BackColor=textImplementation.SpellCheckIsEnabled?Color.LightGreen:Color.LightCoral;
		}

		private void butClearFormatting_Click(object sender,EventArgs e) {
			if(_textFocused.ReadOnly) {
				return;
			}
			try {
				_textFocused.SelectionFont=new Font((string)comboFontType.SelectedItem,(int)comboFontSize.SelectedItem);
				_textFocused.SelectionBullet=false;
				_textFocused.SelectionColor=DefaultForeColor;
				_textFocused.SelectionBackColor=DefaultBackColor;
				_textFocused.Focus();
			}
			catch {

			}
		}

		private void textDescription_Enter(object sender,EventArgs e) {
			_textFocused=textRequirements;
		}

		private void textImplementation_Enter(object sender,EventArgs e) {
			_textFocused=textImplementation;
		}

		private void textRequirements_KeyUp(object sender,KeyEventArgs e) {
			if(e.Control && e.KeyCode == Keys.S) {
				SaveText();
			}
		}

		private void textImplementation_KeyUp(object sender,KeyEventArgs e) {
			if(e.Control && e.KeyCode == Keys.S) {
				SaveText();
			}
		}
	}

}
