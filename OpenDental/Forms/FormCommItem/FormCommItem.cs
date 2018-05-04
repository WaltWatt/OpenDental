using OpenDentBusiness;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OpenDental {
	public partial class FormCommItem : BaseFormCommItem {
		private const string _autoNotePromptRegex=@"\[Prompt:""[a-zA-Z_0-9 ]+""\]";
		private bool _sigChanged;
		private bool _isStartingUp;

		public FormCommItem() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormCommItem_Load(object sender,EventArgs e) {
			_isStartingUp=true;
			//there will usually be a commtype set before this dialog is opened
			for(int i=0;i<_model.ListCommlogTypeDefs.Count;i++){
				listType.Items.Add(_model.ListCommlogTypeDefs[i].ItemName);
				if(_model.ListCommlogTypeDefs[i].DefNum==_model.CommlogCur.CommType) {
					listType.SelectedIndex=i;
				}
			}
			for(int i=0;i<Enum.GetNames(typeof(CommItemMode)).Length;i++){
				listMode.Items.Add(Lan.g("enumCommItemMode",Enum.GetNames(typeof(CommItemMode))[i]));
			}
			for(int i=0;i<Enum.GetNames(typeof(CommSentOrReceived)).Length;i++) {
				listSentOrReceived.Items.Add(Lan.g("enumCommSentOrReceived",Enum.GetNames(typeof(CommSentOrReceived))[i]));
			}
			if(!PrefC.IsODHQ) {
				labelDateTimeEnd.Visible=false;
				textDateTimeEnd.Visible=false;
				butNow.Visible=false;
				butNowEnd.Visible=false;
			}
			if(!Security.IsAuthorized(Permissions.CommlogEdit,_model.CommlogCur.CommDateTime)) {
				//The user does not have permissions to create or edit commlogs.
				if(_controller.GetIsNew() || _controller.GetIsPersistent()) {
					DialogResult=DialogResult.Cancel;
					Close();
					return;
				}
				butDelete.Enabled=false;
				butOK.Enabled=false;
				butEditAutoNote.Enabled=false;
			}
			textNote.Select();
			string keyData=GetSignatureKey();
			signatureBoxWrapper.FillSignature(_model.CommlogCur.SigIsTopaz,keyData,_model.CommlogCur.Signature);
			signatureBoxWrapper.BringToFront();
			butEditAutoNote.Visible=GetHasAutoNotePrompt();
			if(_controller.GetIsPersistent()) {
				_controller.RefreshUserOdPrefs();
				labelCommlogNum.Visible=false;
				textCommlogNum.Visible=false;
				butUserPrefs.Visible=true;
				butOK.Text=Lan.g(this,"Create");
				butCancel.Text=Lan.g(this,"Close");
				butDelete.Visible=false;
			}
			if(_controller.GetIsNew() && PrefC.GetBool(PrefName.CommLogAutoSave)) {
				timerAutoSave.Start();
			}
			textPatientName.Text=_controller.GetPatientNameFL(_model.CommlogCur.PatNum);
			textUser.Text=_controller.GetUserodName(_model.CommlogCur.UserNum);
			textDateTime.Text=_model.CommlogCur.CommDateTime.ToShortDateString()+"  "+_model.CommlogCur.CommDateTime.ToShortTimeString();
			if(_model.CommlogCur.DateTimeEnd.Year>1880) {
				textDateTimeEnd.Text=_model.CommlogCur.DateTimeEnd.ToShortDateString()+"  "+_model.CommlogCur.DateTimeEnd.ToShortTimeString();
			}
			listMode.SelectedIndex=(int)_model.CommlogCur.Mode_;
			listSentOrReceived.SelectedIndex=(int)_model.CommlogCur.SentOrReceived;
			textNote.Text=_model.CommlogCur.Note;
			textNote.SelectionStart=textNote.Text.Length;
#if !DEBUG
			labelCommlogNum.Visible=false;
			textCommlogNum.Visible=false;
#endif
			textCommlogNum.Text=_model.CommlogCur.CommlogNum.ToString();
			_isStartingUp=false;
		}

		public bool TryGetCommItem(bool showMsg,out Commlog commlog) {
			commlog=null;
			if(!ValidateCommlog(showMsg)) {
				return false;
			}
			CommItemModel model;
			if(!TryGetModelFromView(out model)) {
				//Currently the only way for this method to fail is when saving the signature.
				MsgBox.Show(this,"Error saving signature.");
				return false;
			}
			commlog=model.CommlogCur;
			return true;
		}

		public override bool TryGetModelFromView(out CommItemModel model) {
			model=null;
			_model.CommlogCur.DateTimeEnd=PIn.DateT(textDateTimeEnd.Text);
			_model.CommlogCur.CommDateTime=PIn.DateT(textDateTime.Text);
			//there may not be a commtype selected.
			if(listType.SelectedIndex==-1) {
				_model.CommlogCur.CommType=0;
			}
			else {
				_model.CommlogCur.CommType=_model.ListCommlogTypeDefs[listType.SelectedIndex].DefNum;
			}
			_model.CommlogCur.Mode_=(CommItemMode)listMode.SelectedIndex;
			_model.CommlogCur.SentOrReceived=(CommSentOrReceived)listSentOrReceived.SelectedIndex;
			_model.CommlogCur.Note=textNote.Text;
			try {
				if(_sigChanged) {
					string keyData=GetSignatureKey();
					_model.CommlogCur.Signature=signatureBoxWrapper.GetSignature(keyData);
					_model.CommlogCur.SigIsTopaz=signatureBoxWrapper.GetSigIsTopaz();
				}
			}
			catch(Exception) {
				return false;
			}
			model=_model.Copy();
			return true;
		}

		public void SetPatNum(long patNum) {
			_model.CommlogCur.PatNum=patNum;
			textPatientName.Text=_controller.GetPatientNameFL(_model.CommlogCur.PatNum);
		}

		public void SetUserNum(long userNum) {
			_model.CommlogCur.UserNum=Security.CurUser.UserNum;
			textUser.Text=_controller.GetUserodName(_model.CommlogCur.UserNum);
		}

		///<summary>This is a hack in order to set the PK of the current commlog object associated to the view's model.
		///This method needs to get call as soon as the commlog has been inserted into the db so that subsequent updates can actually work.</summary>
		public void SetCommlogNum(long commlogNum) {
			_model.CommlogCur.CommlogNum=commlogNum;
			textCommlogNum.Text=_model.CommlogCur.CommlogNum.ToString();
		}

		///<summary>Helper method to update textDateTime with DateTime.Now</summary>
		public void UpdateButNow() {
			textDateTime.Text=DateTime.Now.ToShortDateString()+"  "+DateTime.Now.ToShortTimeString();
		}

		public void ClearNote() {
			textNote.Clear();
		}

		public void ClearDateTimeEnd() {
			textDateTimeEnd.Clear();
		}

		private bool GetHasAutoNotePrompt() {
			return Regex.IsMatch(textNote.Text,_autoNotePromptRegex);
		}

		private bool ValidateCommlog(bool showMsg) {
			if(textDateTime.Text=="") {
				if(showMsg) {
					MsgBox.Show(this,"Please enter a date first.");
				}
				return false;
			}
			try {
				DateTime.Parse(textDateTime.Text);
			}
			catch {
				if(showMsg) {
					MsgBox.Show(this,"Date / Time invalid.");
				}
				return false;
			}
			if(textDateTimeEnd.Text!="") {
				try {
					DateTime.Parse(textDateTimeEnd.Text);
				}
				catch {
					if(showMsg) {
						MsgBox.Show(this,"End date and time invalid.");
					}
					return false;
				}
			}
			return true;
		}

		private void signatureBoxWrapper_SignatureChanged(object sender,EventArgs e) {
			_controller.signatureBoxWrapper_SignatureChanged(sender,e);
			_sigChanged=true;
		}
		
		private void ClearSignature() {
			if(!_isStartingUp//so this happens only if user changes the note
				&& !_sigChanged)//and the original signature is still showing.
			{
				//SigChanged=true;//happens automatically through the event.
				signatureBoxWrapper.ClearSignature();
			}
		}

		private void ClearSignature_Event(object sender,EventArgs e) {
			ClearSignature();
		}

		private string GetSignatureKey() {
			string keyData=_model.CommlogCur.UserNum.ToString();
			keyData+=_model.CommlogCur.CommDateTime.ToString();
			keyData+=_model.CommlogCur.Mode_.ToString();
			keyData+=_model.CommlogCur.SentOrReceived.ToString();
			if(_model.CommlogCur.Note!=null) {
				keyData+=_model.CommlogCur.Note.ToString();
			}
			return keyData;
		}

		private void timerAutoSave_Tick(object sender,EventArgs e) {
			if(_controller.GetIsPersistent()) {
				//Just in case the auto save timer got turned on in persistent mode.
				timerAutoSave.Stop();
				return;
			}
			Commlog commlog;
			if(!TryGetCommItem(false,out commlog)) {
				return;
			}
			if(_modelOld.CommlogCur.Compare(commlog)) {//They're equal, don't bother saving
				return;
			}
			_controller.AutoSaveCommItem(commlog);
			_modelOld.CommlogCur=commlog;
			//Getting this far means that the commlog was successfully updated so we need to update the UI to reflect that event.
			if(_controller.GetIsNew()) {
				textCommlogNum.Text=_model.CommlogCur.CommlogNum.ToString();
				butCancel.Enabled=false;
			}
			this.Text=Lan.g(this,"Communication Item - Saved:")+" "+DateTime.Now;
		}

		private void timerManualSave_Tick(object sender,EventArgs e) {
			labelSavedManually.Visible=false;
			timerManualSave.Stop();
		}

		private void butUserPrefs_Click(object sender,EventArgs e) {
			FormCommItemUserPrefs FormCIUP=new FormCommItemUserPrefs();
			FormCIUP.ShowDialog();
			if(FormCIUP.DialogResult==DialogResult.OK) {
				_controller.RefreshUserOdPrefs();
			}
		}

		private void butNow_Click(object sender,EventArgs e) {
			UpdateButNow();
		}

		private void butNowEnd_Click(object sender,EventArgs e) {
			textDateTimeEnd.Text=DateTime.Now.ToShortDateString()+"  "+DateTime.Now.ToShortTimeString();
		}

		private void butAutoNote_Click(object sender,EventArgs e) {
			FormAutoNoteCompose FormA=new FormAutoNoteCompose();
			FormA.ShowDialog();
			if(FormA.DialogResult==DialogResult.OK) {
				textNote.AppendText(FormA.CompletedNote);
				butEditAutoNote.Visible=GetHasAutoNotePrompt();
			}
		}

		private void butEditAutoNote_Click(object sender,EventArgs e) {
			if(GetHasAutoNotePrompt()) {
				FormAutoNoteCompose FormA=new FormAutoNoteCompose();
				FormA.MainTextNote=textNote.Text;
				FormA.ShowDialog();
				if(FormA.DialogResult==DialogResult.OK) {
					textNote.Text=FormA.CompletedNote;
					butEditAutoNote.Visible=GetHasAutoNotePrompt();
				}
			}
			else {
				MessageBox.Show(Lan.g(this,"No Auto Note available to edit."));
			}
		}

		private void butClearNote_Click(object sender,EventArgs e) {
			textNote.Clear();
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(_controller.GetIsNew()) {
				DialogResult=DialogResult.Cancel;
				return;
			}
			//button not enabled if no permission and is invisible for persistent mode.
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete?")) {
				return;
			}
			try {
				_controller.DeleteCommlog(_model.CommlogCur);
				DialogResult=DialogResult.OK;
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);//Tell the user what went wrong with deleting the commlog.
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			//button not enabled if no permission
			if(!_controller.SaveCommItem(true)) {
				return;
			}
			if(_controller.GetIsPersistent()) {
				//Show the user an indicator that the commlog has been saved but do not close the window.
				labelSavedManually.Visible=true;
				timerManualSave.Start();
				return;
			}
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
			Close();
		}

		private void FormCommItem_FormClosing(object sender,FormClosingEventArgs e) {
			_controller.CommItemView_FormClosing(sender,e);
			if(_controller.GetIsPersistent()) {
				return;
			}
			if(DialogResult==DialogResult.Cancel && timerAutoSave.Enabled && !_controller.GetIsNew()) {
				try {
					_controller.DeleteCommlog(_model.CommlogCur,"Autosaved Commlog Deleted");
				}
				catch(Exception ex) {
					MessageBox.Show(this,ex.Message);
				}
			}
			timerAutoSave.Stop();
			timerAutoSave.Enabled=false;
		}
	}

	///<summary>Required so that Visual Studio can design this form.  The designer does not allow directly extending classes with generics.</summary>
	public class BaseFormCommItem : ODFormMVC<CommItemModel,FormCommItem,CommItemController> { }
}