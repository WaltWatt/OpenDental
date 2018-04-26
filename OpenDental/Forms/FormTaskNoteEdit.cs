using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormTaskNoteEdit:ODForm {
		public TaskNote TaskNoteCur;
		///<summary>Only called when DialogResult is OK (for OK button and sometimes delete button).</summary>
		public delegate void DelegateEditComplete(object sender);
		///<summary>Called when this form is closed.</summary>
		public DelegateEditComplete EditComplete;

		public FormTaskNoteEdit() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormTaskNoteEdit_Load(object sender,EventArgs e) {
			textDateTime.Text=TaskNoteCur.DateTimeNote.ToString();
			textUser.Text=Userods.GetName(TaskNoteCur.UserNum);
			textNote.Text=TaskNoteCur.Note;
			textNote.Select(TaskNoteCur.Note.Length,0);
			this.Top+=150;
			if(TaskNoteCur.IsNew) {
				textDateTime.ReadOnly=true;
			}
			else if(!Security.IsAuthorized(Permissions.TaskNoteEdit)) {//Tasknotes are not editable unless user has TaskNoteEdit permission.
				butOK.Enabled=false;
				butDelete.Enabled=false;
			}
		}

		private void OnEditComplete() {
			if(EditComplete!=null) {
				EditComplete(this);
			}
		}

		private void butDelete_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete?")) {
				return;
			}
			if(TaskNoteCur.IsNew) {
				DialogResult=DialogResult.Cancel;
				Close();//Needed because the window is called as a non-modal window.
				return;
			}
			TaskNotes.Delete(TaskNoteCur.TaskNoteNum);
			DialogResult=DialogResult.OK;
			OnEditComplete();
			Tasks.TaskEditCreateLog(Permissions.TaskNoteEdit,Lan.g(this,"Deleted note from task"),Tasks.GetOne(TaskNoteCur.TaskNum));
			Close();//Needed because the window is called as a non-modal window.
		}

		private void butAutoNote_Click(object sender,EventArgs e) {
			FormAutoNoteCompose FormA=new FormAutoNoteCompose();
			FormA.ShowDialog();
			if(FormA.DialogResult==DialogResult.OK){
				textNote.Text+=FormA.CompletedNote;
			}
		}


		private void butOK_Click(object sender,EventArgs e) {
			if(textNote.Text=="") {
				MsgBox.Show(this,"Please enter a note, or delete this entry.");
				return;
			}
			try {
				TaskNoteCur.DateTimeNote=DateTime.Parse(textDateTime.Text);//Will not reach database if note is not actually edited.
			}
			catch{
				MsgBox.Show(this,"Please fix date.");
				return;
			}
			if(TaskNoteCur.IsNew) {
				TaskNoteCur.Note=textNote.Text;
				TaskNotes.Insert(TaskNoteCur);
				Tasks.TaskEditCreateLog(Permissions.TaskNoteEdit,Lan.g(this,"Added task note"),Tasks.GetOne(TaskNoteCur.TaskNum));
				//This refreshes "new" status for the task as appropriate.
				DataValid.SetInvalidTask(TaskNoteCur.TaskNum,true);//popup
				DialogResult=DialogResult.OK;
				OnEditComplete();
			}
			else if(TaskNoteCur.Note!=textNote.Text){
				//This refreshes "new" status for the task as appropriate.
				DataValid.SetInvalidTask(TaskNoteCur.TaskNum,true);//popup
				TaskNoteCur.Note=textNote.Text;
				TaskNotes.Update(TaskNoteCur);
				Tasks.TaskEditCreateLog(Permissions.TaskNoteEdit,Lan.g(this,"Task note changed"),Tasks.GetOne(TaskNoteCur.TaskNum));
				DialogResult=DialogResult.OK;
				OnEditComplete();
			}
			else {
				//Intentionally blank, user opened an existing task note and did not change the note but clicked OK.
				//This is effectively equivilent to a Cancel click
				DialogResult=DialogResult.Cancel;
			}
			Close();//Needed because the window is called as a non-modal window.
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
			Close();//Needed because the window is called as a non-modal window.
		}

	}
}