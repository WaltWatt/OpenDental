using CodeBase;
using CodeBase.MVC;
using OpenDentBusiness;
using System;
using System.Linq;
using System.Windows.Forms;

namespace OpenDental {
	public class CommItemController : ODControllerAbs<FormCommItem> {
		public bool IsNew;
		///<summary>Set to true if this commlog window should always stay open.  Changes lots of functionality throughout the entire window.</summary>
		public bool IsPersistent;
		///<summary>The user pref that indicates if this user wants the Note text box to clear after a commlog is saved in persistent mode.
		///Can be null and will be treated as turned on (true) if null.</summary>
		private UserOdPref _userOdPrefClearNote;
		///<summary>The user pref that indicates if this user wants the End text box to clear after a commlog is saved in persistent mode.
		///Can be null and will be treated as turned on (true) if null</summary>
		private UserOdPref _userOdPrefEndDate;
		///<summary>The user pref that indicates if this user wants the Date / Time text box to clear after a commlog is saved in persistent mode.
		///Can be null and will be treated as turned on (true) if null</summary>
		private UserOdPref _userOdPrefUpdateDateTimeNewPat;

		public CommItemController(FormCommItem view) : base(view) {
		}

		public override void OnPostInit() {
			if(IsPersistent) {
				PatientChangedEvent.Fired+=PatientChangedEvent_Fired;
			}
			CommItemSaveEvent.Fired+=CommItemSaveEvent_Fired;
		}

		internal bool GetIsNew() {
			return IsNew;
		}

		internal bool GetIsPersistent() {
			return IsPersistent;
		}

		internal string GetPatientNameFL(long patNum) {
			return Patients.GetLim(patNum).GetNameFL();
		}

		internal string GetUserodName(long userNum) {
			return Userods.GetName(userNum);//might be blank. 
		}

		private void CommItemSaveEvent_Fired(CodeBase.ODEventArgs e) {
			if(e.Name!="CommItemSave") {
				return;
			}
			//save comm item
			SaveCommItem(false);
		}

		public void AutoSaveCommItem(Commlog commlog) {
			if(IsNew) {
				//Insert
				_view.SetCommlogNum(Commlogs.Insert(commlog));
				SecurityLogs.MakeLogEntry(Permissions.CommlogEdit,commlog.PatNum,"Autosave Insert");
				IsNew=false;
			}
			else {
				//Update
				Commlogs.Update(commlog);
			}
		}

		///<summary>Returns true if the commlog was able to save to the database.  Otherwise returns false.
		///Set showMsg to true to show a meaningful message when the commlog cannot be saved.</summary>
		public bool SaveCommItem(bool showMsg) {
			Commlog commlog;
			if(!_view.TryGetCommItem(showMsg,out commlog)) {
				return false;
			}
			if(IsPersistent && string.IsNullOrWhiteSpace(commlog.Note)) { //in persistent mode, we don't want to save empty commlogs
				return false;
			}
			if(IsNew || IsPersistent) {
				_view.SetCommlogNum(Commlogs.Insert(commlog));
				SecurityLogs.MakeLogEntry(Permissions.CommlogEdit,commlog.PatNum,"Insert");
				//Post insert persistent user preferences.
				if(IsPersistent) {
					if(_userOdPrefClearNote==null || PIn.Bool(_userOdPrefClearNote.ValueString)) {
						_view.ClearNote();
					}
					if(_userOdPrefEndDate==null || PIn.Bool(_userOdPrefEndDate.ValueString)) {
						_view.ClearDateTimeEnd();
					}
					ODException.SwallowAnyException(() => {
						FormOpenDental.S_RefreshCurrentModule();
					});
				}
			}
			else {
				Commlogs.Update(commlog);
				SecurityLogs.MakeLogEntry(Permissions.CommlogEdit,commlog.PatNum,"");
			}
			return true;
		}

		///<summary>Tries to delete the commlog passed in.  Throws exceptions if anything goes wrong.</summary>
		internal void DeleteCommlog(Commlog commlog,string logText="Delete") {
			Commlogs.Delete(commlog);
			SecurityLogs.MakeLogEntry(Permissions.CommlogEdit,commlog.PatNum,logText);
		}

		internal void signatureBoxWrapper_SignatureChanged(object sender,EventArgs e) {
			_view.SetUserNum(Security.CurUser.UserNum);
		}

		internal void RefreshUserOdPrefs() {
			if(Security.CurUser==null || Security.CurUser.UserNum < 1) {
				return;
			}
			_userOdPrefClearNote=UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum,UserOdFkeyType.CommlogPersistClearNote).FirstOrDefault();
			_userOdPrefEndDate=UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum,UserOdFkeyType.CommlogPersistClearEndDate).FirstOrDefault();
			_userOdPrefUpdateDateTimeNewPat=UserOdPrefs.GetByUserAndFkeyType(Security.CurUser.UserNum,UserOdFkeyType.CommlogPersistUpdateDateTimeWithNewPatient).FirstOrDefault();
		}

		private void PatientChangedEvent_Fired(ODEventArgs e) {
			if(e.Name!="FormOpenDental" || e.Tag.GetType()!=typeof(long)) {
				return;
			}
			//The tag for this event is the newly selected PatNum
			_view.SetPatNum((long)e.Tag);
			if(IsPersistent && (_userOdPrefUpdateDateTimeNewPat==null || PIn.Bool(_userOdPrefUpdateDateTimeNewPat.ValueString))) {
				_view.UpdateButNow();
			}
		}

		internal void CommItemView_FormClosing(object sender,FormClosingEventArgs e) {
			CommItemSaveEvent.Fired-=CommItemSaveEvent_Fired;
			if(IsPersistent) {
				PatientChangedEvent.Fired-=PatientChangedEvent_Fired;
			}
		}
	}
}
