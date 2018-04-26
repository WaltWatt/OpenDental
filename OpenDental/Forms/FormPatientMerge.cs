using System;
using System.IO;
using System.Windows.Forms;
using CodeBase;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormPatientMerge:ODForm {
		///<summary>For display purposes only.  Reloaded from the db when merge actually occurs.</summary>
		private Patient _patTo;
		///<summary>For display purposes only.  Reloaded from the db when merge actually occurs.</summary>
		private Patient _patFrom;

		public FormPatientMerge() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormPatientMerge_Load(object sender,EventArgs e) {
		}

		private void butChangePatientInto_Click(object sender,EventArgs e) {
			FormPatientSelect fps=new FormPatientSelect();
			if(fps.ShowDialog()==DialogResult.OK){
				long selectedPatNum=fps.SelectedPatNum;//to prevent warning about marshal-by-reference
				textPatientIDInto.Text=selectedPatNum.ToString();
				_patTo=Patients.GetPat(selectedPatNum);
				textPatientNameInto.Text=_patTo.GetNameFLFormal();
				textPatToBirthdate.Text=_patTo.Birthdate.ToShortDateString();
			}
			CheckUIState();
		}

		private void butChangePatientFrom_Click(object sender,EventArgs e) {
			FormPatientSelect fps=new FormPatientSelect();
			if(fps.ShowDialog()==DialogResult.OK) {
				long selectedPatNum=fps.SelectedPatNum;//to prevent warning about marshal-by-reference
				textPatientIDFrom.Text=selectedPatNum.ToString();
				_patFrom=Patients.GetPat(selectedPatNum);
				textPatientNameFrom.Text=_patFrom.GetNameFLFormal();
				textPatFromBirthdate.Text=_patFrom.Birthdate.ToShortDateString();
			}
			CheckUIState();
		}

		private void CheckUIState(){
			butMerge.Enabled=(_patTo!=null && _patFrom!=null);
		}

		private void butMerge_Click(object sender,EventArgs e) {
			if(_patTo.PatNum==_patFrom.PatNum) {
				MsgBox.Show(this,"Cannot merge a patient account into itself. Please select a different patient to merge from.");
				return;
			}
			string msgText="";
			if(_patFrom.FName.Trim().ToLower()!=_patTo.FName.Trim().ToLower()
				|| _patFrom.LName.Trim().ToLower()!=_patTo.LName.Trim().ToLower()
				|| _patFrom.Birthdate!=_patTo.Birthdate) 
			{//mismatch
				msgText=Lan.g(this,"The two patients do not have the same first name, last name, and birthdate.");
				if(Programs.UsingEcwTightOrFullMode()) {
					msgText+="\r\n"+Lan.g(this,"The patients must first be merged from within eCW, then immediately merged in the same order in Open Dental.  "
						+"If the patients are not merged in this manner, some information may not properly bridge between eCW and Open Dental.");
				}
				msgText+="\r\n\r\n"
					+Lan.g(this,"Into patient name")+": "+Patients.GetNameFLnoPref(_patTo.LName,_patTo.FName,"")+", "//using Patients.GetNameFLnoPref to omit MiddleI
					+Lan.g(this,"Into patient birthdate")+": "+_patTo.Birthdate.ToShortDateString()+"\r\n"
					+Lan.g(this,"From patient name")+": "+Patients.GetNameFLnoPref(_patFrom.LName,_patFrom.FName,"")+", "//using Patients.GetNameFLnoPref to omit MiddleI
					+Lan.g(this,"From patient birthdate")+": "+_patFrom.Birthdate.ToShortDateString()+"\r\n\r\n"
					+Lan.g(this,"Merge the patient on the bottom into the patient shown on the top?");
				if(MessageBox.Show(msgText,"",MessageBoxButtons.YesNo)!=DialogResult.Yes) {
					return;//The user chose not to merge
				}
			}
			else {//name and bd match
				if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Merge the patient at the bottom into the patient shown at the top?")) {
					return;//The user chose not to merge.
				}
			}
			if(_patFrom.PatNum==_patFrom.Guarantor) {
				Family fam=Patients.GetFamily(_patFrom.PatNum);
				if(fam.ListPats.Length>1) {
					msgText=Lan.g(this,"The patient you have chosen to merge from is a guarantor.  Merging this patient into another account will cause all "
						+"family members of the patient being merged from to be moved into the same family as the patient account being merged into.")+"\r\n"
						+Lan.g(this,"Do you wish to continue with the merge?");
					if(MessageBox.Show(msgText,"",MessageBoxButtons.YesNo)!=DialogResult.Yes) {
						return;//The user chose not to merge.
					}
				}
			}
			Cursor=Cursors.WaitCursor;
			if(Patients.MergeTwoPatients(_patTo.PatNum,_patFrom.PatNum)) {
				//The patient has been successfully merged.
				//Now copy the physical images from the old patient to the new if they are using an AtoZ image share.
				//This has to happen in the UI because the middle tier server might not have access to the image share.
				//If the users are storing images within the database, those images have already been taken care of in the merge method above.
				int fileCopyFailures=0;
				if(PrefC.AtoZfolderUsed==DataStorageType.LocalAtoZ) {
					#region Copy AtoZ Documents
					//Move the patient documents within the 'patFrom' A to Z folder to the 'patTo' A to Z folder.
					//We have to be careful here of documents with the same name. We have to rename such documents
					//so that no documents are overwritten/lost.
					string atoZpath=ImageStore.GetPreferredAtoZpath();
					string atozFrom=ImageStore.GetPatientFolder(_patFrom,atoZpath);
					string atozTo=ImageStore.GetPatientFolder(_patTo,atoZpath);
					string[] fromFiles=Directory.GetFiles(atozFrom);
					foreach(string fileCur in fromFiles) {
						string fileName=Path.GetFileName(fileCur);
						string destFileName=fileName;
						string destFilePath=ODFileUtils.CombinePaths(atozTo,fileName);
						if(File.Exists(destFilePath)) {
							//The file being copied has the same name as a possibly different file within the destination a to z folder.
							//We need to copy the file under a unique file name and then make sure to update the document table to reflect
							//the change.
							destFileName=_patFrom.PatNum.ToString()+"_"+fileName;
							destFilePath=ODFileUtils.CombinePaths(atozTo,destFileName);
							while(File.Exists(destFilePath)) {
								destFileName=_patFrom.PatNum.ToString()+"_"+fileName+"_"+DateTime.Now.ToString("yyyyMMddhhmmss");
								destFilePath=ODFileUtils.CombinePaths(atozTo,destFileName);
							}
						}
						try {
							File.Copy(fileCur,destFilePath); //Will throw exception if file already exists.
						}
						catch(Exception ex) {
							ex.DoNothing();
							fileCopyFailures++;
							continue;//copy failed, increment counter and move onto the next file
						}
						//If the copy did not fail, try to delete the old file.
						//We can now safely update the document FileName and PatNum to the "to" patient.
						Documents.MergePatientDocument(_patFrom.PatNum,_patTo.PatNum,fileName,destFileName);
						try {
							File.Delete(fileCur);
						}
						catch(Exception ex) {
							ex.DoNothing();
							//If we were unable to delete the file then it is probably because someone has the document open currently.
							//Just skip deleting the file. This means that occasionally there will be an extra file in their backup
							//which is just clutter but at least the merge is guaranteed this way.
						}
					}
					#endregion Copy AtoZ Documents
				}//end if AtoZFolderUsed
				else if(CloudStorage.IsCloudStorage) {
					string atozFrom=ImageStore.GetPatientFolder(_patFrom,"");
					string atozTo=ImageStore.GetPatientFolder(_patTo,"");
					FormProgress FormP=new FormProgress();
					FormP.DisplayText="Moving Documents...";
					FormP.NumberFormat="F";
					FormP.NumberMultiplication=1;
					FormP.MaxVal=100;//Doesn't matter what this value is as long as it is greater than 0
					FormP.TickMS=1000;
					OpenDentalCloud.Core.TaskStateMove state=CloudStorage.MoveAsync(atozFrom
						,atozTo
						,new OpenDentalCloud.ProgressHandler(FormP.OnProgress));
					if(FormP.ShowDialog()==DialogResult.Cancel) {
						state.DoCancel=true;
						fileCopyFailures=state.CountTotal-state.CountSuccess;
					}
					else {
						fileCopyFailures=state.CountFailed;
					}
				}
				Cursor=Cursors.Default;
				if(fileCopyFailures>0) {
					MessageBox.Show(Lan.g(this,"Some files belonging to the from patient were not copied.")+"\r\n"
						+Lan.g(this,"Number of files not copied")+": "+fileCopyFailures);
				}
				textPatientIDFrom.Text="";
				textPatientNameFrom.Text="";
				textPatFromBirthdate.Text="";
				CheckUIState();
				//Make log entry here not in parent form because we can merge multiple patients at a time.
				SecurityLogs.MakeLogEntry(Permissions.PatientMerge,_patTo.PatNum,
					"Patient: "+_patFrom.GetNameFL()+"\r\nPatNum From: "+_patFrom.PatNum+"\r\nPatNum To: "+_patTo.PatNum);
				MsgBox.Show(this,"Patients merged successfully.");
			}//end MergeTwoPatients
			Cursor=Cursors.Default;
		}

		private void butClose_Click(object sender,EventArgs e) {
			Close();
		}

	}
}