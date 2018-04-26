using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;
using System.Globalization;


namespace OpenDental {
	public partial class FormApptTypes:ODForm {
		private List<AppointmentType> _listApptTypes;
		///<summary>Stale deep copy of _listApptTypes to use with sync.</summary>
		private List<AppointmentType> _listApptTypesOld;
		private bool _isChanged=false;
		public bool SelectionMode;
		public AppointmentType SelectedAptType;

		public FormApptTypes() {
			InitializeComponent();
			Lan.F(this);
			_listApptTypes=new List<AppointmentType>();
		}

		private void FormApptTypes_Load(object sender,EventArgs e) {
			if(SelectionMode) {
				butOK.Visible=true;
				butAdd.Visible=false;
				butDown.Visible=false;
				butUp.Visible=false;
				checkWarn.Visible=false;
				checkPrompt.Visible=false;
				this.Text=Lan.g(this,"Select Appointment Type");
				gridMain.Location=new Point(8,6);
				gridMain.Size=new Size(292,447);
			}
			checkPrompt.Checked=PrefC.GetBool(PrefName.AppointmentTypeShowPrompt);
			checkWarn.Checked=PrefC.GetBool(PrefName.AppointmentTypeShowWarning);
			//don't show hidden appointment types in selection mode
			_listApptTypes=AppointmentTypes.GetDeepCopy(SelectionMode);
			_listApptTypesOld=AppointmentTypes.GetDeepCopy();
			FillMain();
		}

		private void FillMain() {
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableApptTypes","Name"),200);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableApptTypes","Color"),50,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableApptTypes","Hidden"),0,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			_listApptTypes.Sort(AppointmentTypes.SortItemOrder);
			for(int i=0;i<_listApptTypes.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_listApptTypes[i].AppointmentTypeName);
				row.Cells.Add("");//color row, no text.
				row.Cells[1].CellColor=_listApptTypes[i].AppointmentTypeColor;
				row.Cells.Add(_listApptTypes[i].IsHidden?"X":"");
				gridMain.Rows.Add(row);
			}
			if(SelectionMode) {
				row=new ODGridRow();
				row.Cells.Add("None");
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butUp_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select an item in the grid first.");
				return;
			}
			if(gridMain.GetSelectedIndex()==0) {
				//Do nothing, the item is at the top of the list.
				return;
			}
			int index=gridMain.GetSelectedIndex();
			_isChanged=true;
			_listApptTypes[index-1].ItemOrder+=1;
			_listApptTypes[index].ItemOrder-=1;
			FillMain();
			index-=1;
			gridMain.SetSelected(index,true);
		}

		private void butDown_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()==-1) {
				MsgBox.Show(this,"Please select an item in the grid first.");
				return;
			}
			if(gridMain.GetSelectedIndex()==_listApptTypes.Count-1) {
				//Do nothing, the item is at the bottom of the list.
				return;
			}
			int index=gridMain.GetSelectedIndex();
			_isChanged=true;
			_listApptTypes[index+1].ItemOrder-=1;
			_listApptTypes[index].ItemOrder+=1;
			FillMain();
			index+=1;
			gridMain.SetSelected(index,true);
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(SelectionMode) {
				if(e.Row>=_listApptTypes.Count) {
					SelectedAptType=null;
				}
				else {
					SelectedAptType=_listApptTypes[e.Row];
				}
				this.DialogResult=DialogResult.OK;
			}
			else {
				FormApptTypeEdit FormATE=new FormApptTypeEdit();
				FormATE.AppointmentTypeCur=_listApptTypes[e.Row];
				FormATE.ShowDialog();
				if(FormATE.DialogResult!=DialogResult.OK) {
					return;
				}
				if(FormATE.AppointmentTypeCur==null) {
					_listApptTypes.RemoveAt(e.Row);
				}
				else {
					_listApptTypes[e.Row]=FormATE.AppointmentTypeCur;
				}
				_isChanged=true;
				FillMain();
			}
		}

		private void checkPrompt_CheckedChanged(object sender,EventArgs e) {
			_isChanged=true;
		}

		private void checkWarn_CheckedChanged(object sender,EventArgs e) {
			_isChanged=true;
		}

		private void butAdd_Click(object sender,EventArgs e) {
			FormApptTypeEdit FormATE=new FormApptTypeEdit();
			FormATE.AppointmentTypeCur=new AppointmentType();
			FormATE.AppointmentTypeCur.ItemOrder=_listApptTypes.Count;
			FormATE.AppointmentTypeCur.IsNew=true;
			FormATE.AppointmentTypeCur.AppointmentTypeColor=Color.FromArgb(0);//Default color to white, otherwise it picks a light gray.
			FormATE.ShowDialog();
			if(FormATE.DialogResult!=DialogResult.OK) {
				return;
			}
			_listApptTypes.Add(FormATE.AppointmentTypeCur);
			_isChanged=true;
			FillMain();
		}

		///<summary>DEPRECATED - Originally writtento diagnose a bug in the middle teir that was solved by fixing Color serialization.
		///It appeared as though someonewasrepeatedly changing the appointment type colors.</summary>
		private void LogEntry(List<AppointmentType> listNew,List<AppointmentType> listDB) {
			listNew.Sort((AppointmentType x,AppointmentType y) => { return x.AppointmentTypeNum.CompareTo(y.AppointmentTypeNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			listDB.Sort((AppointmentType x,AppointmentType y) => { return x.AppointmentTypeNum.CompareTo(y.AppointmentTypeNum); });//Anonymous function, sorts by compairing PK.  Lambda expressions are not allowed, this is the one and only exception.  JS approved.
			int idxNew=0;
			int idxDB=0;
			AppointmentType fieldNew;
			AppointmentType fieldDB;
			//Because both lists have been sorted using the same criteria, we can now walk each list to determine which list contians the next element.  The next element is determined by Primary Key.
			//If the New list contains the next item it will be inserted.  If the DB contains the next item, it will be deleted.  If both lists contain the next item, the item will be updated.
			while(idxNew<listNew.Count || idxDB<listDB.Count) {
				fieldNew=null;
				if(idxNew<listNew.Count) {
					fieldNew=listNew[idxNew];//used to compare the new list to db list
				}
				fieldDB=null;				
				if(idxDB<listDB.Count) {
					fieldDB=listDB[idxDB];//used to compare the db list with new list
				}
				//begin compare
				if(fieldNew!=null && fieldDB==null) {//listNew has more items, listDB does not.
					SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Added new appointment type "+"'"+fieldNew.AppointmentTypeName+"'"+
						" Color '"+fieldNew.AppointmentTypeColor.ToKnownColor()+"',ItemOrder '"+fieldNew.ItemOrder+"', IsHidden '"+
						fieldNew.IsHidden+"'.");
					idxNew++;
					continue;
				}
				else if(fieldNew==null && fieldDB!=null) {//listDB has more items, listNew does not.
					SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Deleted appointment type "+"'"+fieldDB.AppointmentTypeName+"'"+" Color '"+
					fieldDB.AppointmentTypeColor.Name+"',ItemOrder '"+fieldDB.ItemOrder+"', IsHidden '"+fieldDB.IsHidden+"'.");
					idxDB++;
					continue;
				}
				else if(fieldNew.AppointmentTypeNum<fieldDB.AppointmentTypeNum) {//newPK less than dbPK, newItem is 'next'
					SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Added new appointment type "+"'"+fieldNew.AppointmentTypeName+"'"+
						" Color '"+fieldNew.AppointmentTypeColor.ToKnownColor()+"',ItemOrder '"+fieldNew.ItemOrder+"', IsHidden '"+
						fieldNew.IsHidden+"'.");
					idxNew++;
					continue;
				}
				else if(fieldNew.AppointmentTypeNum>fieldDB.AppointmentTypeNum) {//dbPK less than newPK, dbItem is 'next'
					SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Deleted appointment type "+"'"+fieldDB.AppointmentTypeName+"'"+" Color '"+
					fieldDB.AppointmentTypeColor.Name+"',ItemOrder '"+fieldDB.ItemOrder+"', IsHidden '"+fieldDB.IsHidden+"'.");
					idxDB++;
					continue;
				}
				//Both lists contain the 'next' item, update required
				string update="";
				if(fieldNew.AppointmentTypeName != fieldDB.AppointmentTypeName) {
					if(update!="") { update+=","; }
					update+="Name '"+fieldDB.AppointmentTypeName+"' to '"+fieldNew.AppointmentTypeName+"'";					
				}
				if(fieldNew.AppointmentTypeColor != fieldDB.AppointmentTypeColor) {
					if(update!="") { update+=","; }
					update+="Color '"+fieldDB.AppointmentTypeColor+"' to '"+fieldNew.AppointmentTypeColor+"'";
				}
				if(fieldNew.ItemOrder != fieldDB.ItemOrder) {
					if(update!="") { update+=","; }
					update+="Order '"+fieldDB.ItemOrder+"' to '"+fieldNew.ItemOrder+"'";
				}
				if(fieldNew.IsHidden != fieldDB.IsHidden) {
					if(update!="") { update+=","; }
					update+="Hidden '"+fieldDB.IsHidden+"' to '"+fieldNew.IsHidden+"'";
				}
				if(update=="") {
					//no changes
				}
				else {
					SecurityLogs.MakeLogEntry(Permissions.Setup,0,"Updated appointment type '"+fieldDB.AppointmentTypeName+"' ApptTypeNum("
						+fieldDB.AppointmentTypeNum+"). Changes- "+update+".");
				}			
				idxNew++;
				idxDB++;
			}
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(gridMain.GetSelectedIndex()>=_listApptTypes.Count || gridMain.GetSelectedIndex()==-1) {
				SelectedAptType=null;
			}
			else {
				SelectedAptType=_listApptTypes[gridMain.GetSelectedIndex()];
			}
			this.DialogResult=DialogResult.OK;
		}

		private void butClose_Click(object sender,EventArgs e) {
			if(SelectionMode) {
				DialogResult=DialogResult.Cancel;
			}
			Close();
		}

		private void FormApptTypes_FormClosing(object sender,FormClosingEventArgs e) {
			if(!SelectionMode) {
				if(_isChanged) {
					Prefs.UpdateBool(PrefName.AppointmentTypeShowPrompt,checkPrompt.Checked);
					Prefs.UpdateBool(PrefName.AppointmentTypeShowWarning,checkWarn.Checked);
					for(int i=0;i<_listApptTypes.Count;i++) {
						_listApptTypes[i].ItemOrder=i;
					}
					AppointmentTypes.Sync(_listApptTypes,_listApptTypesOld);
					//LogEntry(_listApptTypes,_listApptTypesOld);//no longer needed, originally used to diagnose a bugin the middle teir.
					DataValid.SetInvalid(InvalidType.AppointmentTypes);
					DataValid.SetInvalid(InvalidType.Prefs);
				}
				DialogResult=DialogResult.OK;
			}
		}
	}
}