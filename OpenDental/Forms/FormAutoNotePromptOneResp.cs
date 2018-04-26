using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental {
	public partial class FormAutoNotePromptOneResp:ODForm {
		///<summary>Set this value externally.</summary>
		public string PromptText;
		///<summary>What the user picked.</summary>
		public string ResultText;
		///<summary>The string value representing the list to pick from.  One item per line.</summary>
		public string PromptOptions;
		///<summary>If user has the option to go back</summary>
		public bool IsGoBack;
		///<summary>The string value of previous user response</summary>
		public string CurPromptResponse;
		/// <summary>The string of the autonote description which is used to bring up the actual prompt.</summary>
		private string _noteDescript;

		public FormAutoNotePromptOneResp(string noteDescription) {
			InitializeComponent();
			_noteDescript=noteDescription;
			Lan.F(this);
		}

		private void FormAutoNotePromptOneResp_Load(object sender,EventArgs e) {
			CurPromptResponse=!string.IsNullOrEmpty(CurPromptResponse) ? CurPromptResponse : "";
			Location=new Point(Left,Top+150);
			labelPrompt.Text=PromptText;
			butBack.Visible=IsGoBack;
			string[] lines=PromptOptions.Split(new string[] {"\r\n"},StringSplitOptions.RemoveEmptyEntries);
			string[] curReponseLines=CurPromptResponse.Split(new string[] {"\r\n"},StringSplitOptions.RemoveEmptyEntries);
			int stringWidthMax=120;//Minimum column width
			for(int i=0;i<lines.Length;i++) {
				int lineWidth=TextRenderer.MeasureText(lines[i],listMain.Font).Width;
				if(lineWidth>stringWidthMax) {
					stringWidthMax=lineWidth;
				}
				listMain.Items.Add(lines[i]);
				if(curReponseLines.Contains(lines[i])) {//check previous user response
					listMain.SetSelected(listMain.Items.Count-1,true);
				}
			}
			if(stringWidthMax>listMain.Width-20) {//Give them some room
				listMain.MultiColumn=false;
			}
			else {
				listMain.ColumnWidth=stringWidthMax;
			}
		}

		private void listMain_MouseDoubleClick(object sender,MouseEventArgs e) {
			if(listMain.SelectedIndex==-1) {
				return;
			}
			ResultText=listMain.SelectedItem.ToString();
			DialogResult=DialogResult.OK;
		}

		private void FormAutoNotePromptOneResp_KeyDown(object sender,KeyEventArgs e) {
			if(e.KeyCode==Keys.Enter) {
				if(listMain.SelectedIndex==-1) {
					MsgBox.Show(this,"One response must be selected");
					return;
				}
				ResultText=listMain.SelectedItem.ToString();
				DialogResult=DialogResult.OK;
			}
		}

		private void butRemovePrompt_Click(object sender,EventArgs e) {
			ResultText="";
			DialogResult=DialogResult.OK;
		}

		private void butNext_Click(object sender,EventArgs e) {
			if(listMain.SelectedIndex==-1) {
				ResultText="[Prompt:\""+_noteDescript+"\"]";
			}
			else {
				ResultText=listMain.SelectedItem.ToString();
			}
			DialogResult=DialogResult.OK;
		}

		private void butBack_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Retry;
			Close();
		}

		private void butPreview_Click(object sender,EventArgs e) {
			if(listMain.SelectedIndex==-1) {
				MsgBox.Show(this,"One response must be selected");
				return;
			}
			ResultText=listMain.SelectedItem.ToString();
			FormAutoNotePromptPreview FormP=new FormAutoNotePromptPreview();
			FormP.ResultText=ResultText;
			FormP.ShowDialog();
			if(FormP.DialogResult==DialogResult.OK) {
				ResultText=FormP.ResultText;
				DialogResult=DialogResult.OK;
			}
		}

		private void butExit_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Abort autonote entry?")) {
				return;
			}
			DialogResult=DialogResult.Cancel;
		}
	}
}