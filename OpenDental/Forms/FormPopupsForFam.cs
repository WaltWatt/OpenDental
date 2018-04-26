using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class FormPopupsForFam:ODForm {
		public Patient PatCur;
		private List<PopupEvent> _listPopEvents;
		private List<Popup> _listPopups;

		public FormPopupsForFam(List<PopupEvent> listPopEvents) {
			InitializeComponent();
			Lan.F(this);
			_listPopEvents=listPopEvents;
		}

		private void FormPopupsForFam_Load(object sender,EventArgs e) {
			gridMain.AllowSortingByColumn=true;
			FillGrid();
		}

		private void FillGrid() {
			if(checkDeleted.Checked) {
				_listPopups=Popups.GetDeletedForFamily(PatCur);
			}
			else {
				_listPopups=Popups.GetForFamily(PatCur);
			}
			#region Automation PopUpThenDisable10Min
			//Joe - We are mimicing popups through a new AutomationAction PopUpThenDisable10Min.
			//We want to show it in this list to avoid confusion and phone calls.
			List<long> listAutoNums=FormOpenDental.DicBlockedAutomations.Keys.ToList();
			Dictionary<long,DateTime> dictPatDate;
			//First level dictionary key is an AutomationNum.
			//Value is Dictionary<long,DateTime>.
			Popup popup;
			Automation autoCur;
			List<Automation> listAutomations=Automations.GetDeepCopy();
			foreach(long automationNum in listAutoNums) {
				dictPatDate=FormOpenDental.DicBlockedAutomations[automationNum];//Gets all patnums for current automation.
				//Second level dictionary key is a patNum for current AutomationNum key.
				//Value is DateTime representing block until time.
				foreach(KeyValuePair<long,DateTime> kvp in dictPatDate) {
					if(!Patients.GetAllFamilyPatNums(new List<long>() { kvp.Key }).Contains(PatCur.PatNum)) {
						continue;//Not in the same family.
					}
					autoCur=listAutomations.FirstOrDefault(x => x.AutomationNum==automationNum);
					if(autoCur==null) {
						continue;
					}
					#region Dummy PopUp
					//Create dummy Popup.
					//Since we never insert these into the DB we just put them in our list that we fill the grid with.
					popup=new Popup() {
						PopupNum=automationNum,
						PatNum=kvp.Key,
						PopupLevel=EnumPopupLevel.Automation,
						DateTimeEntry=kvp.Value.AddMinutes(-10),
						Description=autoCur.MessageContent
					};
					_listPopups.Add(popup);
					#endregion Dummy PopUp
				}
			}
			_listPopups=_listPopups.OrderBy(x => x.PatNum).ThenBy(x => x.PopupLevel).ToList();
			#endregion Automation PopUpThenDisable10Min
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TablePopupsForFamily","Patient"),120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePopupsForFamily","Level"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePopupsForFamily","Disabled"),60,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TablePopupsForFamily","Last Viewed"),80,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			if(checkDeleted.Checked) {
				col=new ODGridColumn(Lan.g("TablePopupsForFamily","Deleted"),60,HorizontalAlignment.Center);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g("TablePopupsForFamily","Popup Message"),120);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			_listPopups.RemoveAll(x => x.PopupLevel==EnumPopupLevel.Automation && !listAutomations.Any(y => y.AutomationNum==x.PopupNum));
			for(int i=0;i<_listPopups.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(Patients.GetPat(_listPopups[i].PatNum).GetNameLF());
				if(_listPopups[i].PopupLevel==EnumPopupLevel.Automation) {
					autoCur=Automations.GetFirstOrDefault(x => x.AutomationNum==_listPopups[i].PopupNum);//Get by PK.
					//this should never happen because we remove any where the AutomationNum is not in Automations.Listt
					//if(autoCur==null) {
					//	continue;
					//}
					row.Cells.Add(Lan.g("enumEnumPopupLevel","Auto")+": "+Lan.g("enumEnumPopupLevel",autoCur.Autotrigger.ToString()));
					row.Cells.Add("");//Disabled column. Will never happen for automations.
					row.Cells.Add(_listPopups[i].DateTimeEntry.ToShortTimeString());
				}
				else {
					row.Cells.Add(Lan.g("enumEnumPopupLevel",_listPopups[i].PopupLevel.ToString()));
					row.Cells.Add(_listPopups[i].IsDisabled?"X":"");
					PopupEvent popEvent=_listPopEvents.FirstOrDefault(x => x.PopupNum==_listPopups[i].PopupNum);
					if(popEvent!=null && popEvent.LastViewed.Year>1880) {
						row.Cells.Add(popEvent.LastViewed.ToShortTimeString());
					}
					else {
						row.Cells.Add("");
					}
				}
				if(checkDeleted.Checked) {
					row.Cells.Add(_listPopups[i].IsArchived?"X":"");
				}
				row.Cells.Add(_listPopups[i].Description);
				row.Tag=_listPopups[i];
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(((Popup)gridMain.Rows[e.Row].Tag).PopupLevel==EnumPopupLevel.Automation){
				MsgBox.Show(this,"To edit automations go to Setup | Automation");
			}
			else{
				FormPopupEdit FormPE=new FormPopupEdit();
				FormPE.PopupCur=(Popup)gridMain.Rows[e.Row].Tag;
				FormPE.ShowDialog();
				if(FormPE.DialogResult==DialogResult.OK) {
					FillGrid();
				}
			}
		}
		
		private void checkDeleted_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormPopupEdit FormPE=new FormPopupEdit();
			Popup popup=new Popup();
			popup.PatNum=PatCur.PatNum;
			popup.PopupLevel=EnumPopupLevel.Patient;
			popup.IsNew=true;
			FormPE.PopupCur=popup;
			FormPE.ShowDialog();
			if(FormPE.DialogResult==DialogResult.OK) {
				FillGrid();
			}
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.OK;
		}
	}
}