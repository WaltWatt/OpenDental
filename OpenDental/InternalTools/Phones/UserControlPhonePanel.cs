﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental {
	public partial class UserControlPhonePanel:UserControl {
		private List<Phone> PhoneList;
		///<summary>When the GoToChanged event fires, this tells us which patnum.</summary>
		public long GotoPatNum;
		///<summary></summary>
		[Category("Property Changed"),Description("Event raised when user wants to go to a patient or related object.")]
		public event EventHandler GoToChanged=null;
		private int rowI;
		private int colI;
		///<summary>This is the difference between server time and local computer time.  Used to ensure that times displayed are accurate to the second.  This value is usally just a few seconds, but possibly a few minutes.</summary>
		private TimeSpan timeDelta;
#if !DEBUG
		private int msgCount;
#endif

		public UserControlPhonePanel() {
			InitializeComponent();
		}

		private void UserControlPhonePanel_Load(object sender,EventArgs e) {
			timer1.Enabled=true;
			timerMsgs.Enabled=true;
			SetLabelMsg();
			timeDelta=MiscData.GetNowDateTime()-DateTime.Now;
			FillEmps();
		}

		private void SetLabelMsg() {
#if DEBUG
				return;
#else
			if(!Directory.Exists(Phones.PathPhoneMsg)) {
				labelMsg.Text="msg path not found";
				labelMsg.Font=new Font(FontFamily.GenericSansSerif,8.5f,FontStyle.Regular);
				labelMsg.ForeColor=Color.Black;
				return;
			}
			msgCount=Directory.GetFiles(Phones.PathPhoneMsg,"*.txt").Length;
			if(msgCount==0) {
				labelMsg.Text="Phone Messages: 0";
				labelMsg.Font=new Font(FontFamily.GenericSansSerif,8.5f,FontStyle.Regular);
				labelMsg.ForeColor=Color.Black;
			}
			else {
				labelMsg.Text="Phone Messages: "+msgCount.ToString();
				labelMsg.Font=new Font(FontFamily.GenericSansSerif,10f,FontStyle.Bold);
				labelMsg.ForeColor=Color.Firebrick;
			}
#endif
		}

		protected void OnGoToChanged() {
			if(GoToChanged!=null) {
				GoToChanged(this,new EventArgs());
			}
		}

		private void FillEmps(){
			gridEmp.BeginUpdate();
			gridEmp.Columns.Clear();
			ODGridColumn col;
			col=new ODGridColumn(Lan.g("TableEmpClock","Ext"),25);
			gridEmp.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEmpClock","Employee"),60);
			gridEmp.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEmpClock","Status"),80);
			gridEmp.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEmpClock","Phone"),50);
			gridEmp.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEmpClock","InOut"),35);
			gridEmp.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEmpClock","Customer"),90);
			gridEmp.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableEmpClock","Time"),70);
			gridEmp.Columns.Add(col);
			gridEmp.Rows.Clear();
			UI.ODGridRow row;
			PhoneList=Phones.GetPhoneList();
			DateTime dateTimeStart;
			TimeSpan span;
			DateTime timeOfDay;//because TimeSpan does not have good formatting.
			for(int i=0;i<PhoneList.Count;i++){
				row=new OpenDental.UI.ODGridRow();
				row.Cells.Add(PhoneList[i].Extension.ToString());
				row.Cells.Add(PhoneList[i].EmployeeName);
				if(PhoneList[i].ClockStatus==ClockStatusEnum.None){
					row.Cells.Add("");
				}
				else{
					row.Cells.Add(PhoneList[i].ClockStatus.ToString());
				}
				row.Cells.Add(PhoneList[i].Description);
				row.Cells.Add(PhoneList[i].InOrOut);
				row.Cells.Add(PhoneList[i].CustomerNumber);
				dateTimeStart=PhoneList[i].DateTimeStart;
				if(dateTimeStart.Date==DateTime.Today){
					span=DateTime.Now-dateTimeStart+timeDelta;
					timeOfDay=DateTime.Today+span;
					row.Cells.Add(timeOfDay.ToString("H:mm:ss"));
				}
				else{
					row.Cells.Add("");
				}
				row.ColorBackG=PhoneList[i].ColorBar;
				row.ColorText=PhoneList[i].ColorText;
				gridEmp.Rows.Add(row);
			}
			gridEmp.EndUpdate();
			gridEmp.SetSelected(false);
		}

		private void timer1_Tick(object sender,EventArgs e) {
			//For now, happens once per 1.6 seconds regardless of phone activity.
			//This might need improvement.
			FillEmps();
		}

		private void timerMsgs_Tick(object sender,EventArgs e) {
			//every 3 sec.
			SetLabelMsg();
		}

		private void butOverride_Click(object sender,EventArgs e) {
			//FormPhoneOverrides FormO=new FormPhoneOverrides();
			//FormO.ShowDialog();
			MessageBox.Show("Not working right now.");
		}

		private void gridEmp_CellClick(object sender,ODGridClickEventArgs e) {
			if((e.Button & MouseButtons.Right)==MouseButtons.Right){
				return;
			}
			long patNum=PhoneList[e.Row].PatNum;
			GotoPatNum=patNum;
			OnGoToChanged();
		}

		private void menuItemManage_Click(object sender,EventArgs e) {
			long patNum=PhoneList[rowI].PatNum;
			if(patNum==0){
				MsgBox.Show(this,"Please attach this number to a patient first.");
				return;
			}
			FormPhoneNumbersManage FormM=new FormPhoneNumbersManage();
			FormM.PatNum=patNum;
			FormM.ShowDialog();
		}

		private void menuItemAdd_Click(object sender,EventArgs e) {
			if(FormOpenDental.CurPatNum==0) {
				MsgBox.Show(this,"Please select a patient in the main window first.");
				return;
			}
			if(PhoneList[rowI].PatNum!=0) {
				if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"The current number is already attached to a patient. Attach it to this patient instead?")) {
					return;
				}
				PhoneNumber ph=PhoneNumbers.GetByVal(PhoneList[rowI].CustomerNumber);
				ph.PatNum=FormOpenDental.CurPatNum;
				PhoneNumbers.Update(ph);
			}
			else {
				string patName=Patients.GetLim(FormOpenDental.CurPatNum).GetNameLF();
				if(MessageBox.Show("Attach this phone number to "+patName+"?","",MessageBoxButtons.OKCancel)!=DialogResult.OK) {
					return;
				}
				PhoneNumber ph=new PhoneNumber();
				ph.PatNum=FormOpenDental.CurPatNum;
				ph.PhoneNumberVal=PhoneList[rowI].CustomerNumber;
				PhoneNumbers.Insert(ph);
			}
			//tell the phone server to refresh this row with the patient name and patnum
			DataValid.SetInvalid(InvalidType.PhoneNumbers);
		}

		private void gridEmp_MouseUp(object sender,MouseEventArgs e) {
			if(e.Button!=MouseButtons.Right) {
				return;
			}
			rowI=gridEmp.PointToRow(e.Y);
			colI=gridEmp.PointToCol(e.X);
			if(rowI==-1){
				return;
			}
			if(colI==5){
				menuNumbers.Show(gridEmp,e.Location);
			}
			if(colI==2){
				menuStatus.Show(gridEmp,e.Location);
			}		
		}

		private void menuItemAvailable_Click(object sender,EventArgs e) {
			SetPhoneAvailable(ClockStatusEnum.Available);//green
		}

		private void menuItemTraining_Click(object sender,EventArgs e) {
			SetPhoneAvailable(ClockStatusEnum.Training);
		}

		private void menuItemTeamAssist_Click(object sender,EventArgs e) {
			SetPhoneAvailable(ClockStatusEnum.TeamAssist);
		}

		private void menuItemTCResponder_Click(object sender,EventArgs e) {
			SetPhoneAvailable(ClockStatusEnum.TCResponder);
		}

		private void menuItemWrapUp_Click(object sender,EventArgs e) {
			SetPhoneAvailable(ClockStatusEnum.WrapUp);//this is usually an automatic status
		}

		private void menuItemOfflineAssist_Click(object sender,EventArgs e) {
			SetPhoneAvailable(ClockStatusEnum.OfflineAssist);
		}

		///<summary>Sets the current phone's employee and extension as "available" and then sets the phone status to the clock status passed in.
		///Returns false if the user currently clocked in does not have permission or the credentials to do the desired action.
		///Also, calls FillEmps() if action was successfully taken.</summary>
		private bool SetPhoneAvailable(ClockStatusEnum clockStatus) {
			if(!ClockIn()) {
				return false;
			}
			int extension=PhoneList[rowI].Extension;
			long employeeNum=PhoneList[rowI].EmployeeNum;
			PhoneEmpDefaults.SetAvailable(extension,employeeNum);
			Phones.SetPhoneStatus(clockStatus,extension);
			FillEmps();
			return true;
		}

		private void menuItemUnavailable_Click(object sender,EventArgs e) {
			MessageBox.Show("Not working right now.");
			/*
			if(!ClockIn()) {
				return;
			}
			int extension=PhoneList[rowI].Extension;
			long employeeNum=PhoneList[rowI].EmployeeNum;
			//Employees.SetUnavailable(extension,employeeNum);
			//Get an override if it exists
			PhoneOverride phoneOR=PhoneOverrides.GetByExtAndEmp(extension,employeeNum);
			if(phoneOR==null) {//there is no override for that extension/emp combo.
				phoneOR=new PhoneOverride();
				phoneOR.EmpCurrent=employeeNum;
				phoneOR.Extension=extension;
				phoneOR.IsAvailable=false;
				FormPhoneOverrideEdit FormO=new FormPhoneOverrideEdit();
				FormO.phoneCur=phoneOR;
				FormO.IsNew=true;
				FormO.ForceUnAndExplanation=true;
				FormO.ShowDialog();
				if(FormO.DialogResult!=DialogResult.OK) {
					return;
				}
			}
			else {
				phoneOR.IsAvailable=false;
				FormPhoneOverrideEdit FormO=new FormPhoneOverrideEdit();
				FormO.phoneCur=phoneOR;
				FormO.ForceUnAndExplanation=true;
				FormO.ShowDialog();
				if(FormO.DialogResult!=DialogResult.OK) {
					return;
				}
			}
			FillEmps();*/
		}

		//RingGroups---------------------------------------------------

		private void menuItemQueueTech_Click(object sender,EventArgs e) {
			//This even works if the person is still clocked out.
			PhoneAsterisks.SetQueueForExtension(PhoneList[rowI].Extension,AsteriskQueues.Tech);
		}

		private void menuItemQueueNone_Click(object sender,EventArgs e) {
			//This even works if the person is still clocked out.
			PhoneAsterisks.SetQueueForExtension(PhoneList[rowI].Extension,AsteriskQueues.None);
		}

		private void menuItemQueueDefault_Click(object sender,EventArgs e) {
			PhoneAsterisks.SetToDefaultQueue(PhoneList[rowI].EmployeeNum);
		}

		private void menuItemQueueBackup_Click(object sender,EventArgs e) {
			if(SetPhoneAvailable(ClockStatusEnum.Backup)) {
				PhoneAsterisks.SetQueueForExtension(PhoneList[rowI].Extension,AsteriskQueues.Backup);
			}
		}

		//Timecard---------------------------------------------------

		private void menuItemLunch_Click(object sender,EventArgs e) {
			//verify that employee is logged in as user
			int extension=PhoneList[rowI].Extension;
			long employeeNum=PhoneList[rowI].EmployeeNum;
			if(PrefC.GetBool(PrefName.TimecardSecurityEnabled)){
				if(Security.CurUser.EmployeeNum!=employeeNum){
					if(!Security.IsAuthorized(Permissions.TimecardsEditAll)){
						MsgBox.Show(this,"Not authorized.");
						return;
					}
				}
			}
			try{
				ClockEvents.ClockOut(employeeNum,TimeClockStatus.Lunch);
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);//This message will tell user that they are already clocked out.
				return;
			}
			PhoneEmpDefaults.SetAvailable(extension,employeeNum);
			Employee EmpCur=Employees.GetEmp(employeeNum);
			EmpCur.ClockStatus=Lan.g("enumTimeClockStatus",TimeClockStatus.Lunch.ToString());
			Employees.Update(EmpCur);
			Phones.SetPhoneStatus(ClockStatusEnum.Lunch,extension);
			FillEmps();
		}

		private void menuItemHome_Click(object sender,EventArgs e) {
			//verify that employee is logged in as user
			int extension=PhoneList[rowI].Extension;
			long employeeNum=PhoneList[rowI].EmployeeNum;
			if(PrefC.GetBool(PrefName.TimecardSecurityEnabled)){
				if(Security.CurUser.EmployeeNum!=employeeNum){
					if(!Security.IsAuthorized(Permissions.TimecardsEditAll)){
						MsgBox.Show(this,"Not authorized.");
						return;
					}
				}
			}
			try{
				ClockEvents.ClockOut(employeeNum,TimeClockStatus.Home);
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);//This message will tell user that they are already clocked out.
				return;
			}
			PhoneEmpDefaults.SetAvailable(extension,employeeNum);
			Employee EmpCur=Employees.GetEmp(employeeNum);
			EmpCur.ClockStatus=Lan.g("enumTimeClockStatus",TimeClockStatus.Home.ToString());
			Employees.Update(EmpCur);
			Phones.SetPhoneStatus(ClockStatusEnum.Home,extension);
			FillEmps();
		}

		private void menuItemBreak_Click(object sender,EventArgs e) {
			//verify that employee is logged in as user
			int extension=PhoneList[rowI].Extension;
			long employeeNum=PhoneList[rowI].EmployeeNum;
			if(PrefC.GetBool(PrefName.TimecardSecurityEnabled)){
				if(Security.CurUser.EmployeeNum!=employeeNum){
					if(!Security.IsAuthorized(Permissions.TimecardsEditAll)){
						MsgBox.Show(this,"Not authorized.");
						return;
					}
				}
			}
			try{
				ClockEvents.ClockOut(employeeNum,TimeClockStatus.Break);
			}
			catch(Exception ex){
				MessageBox.Show(ex.Message);//This message will tell user that they are already clocked out.
				return;
			}
			PhoneEmpDefaults.SetAvailable(extension,employeeNum);
			Employee EmpCur=Employees.GetEmp(employeeNum);
			EmpCur.ClockStatus=Lan.g("enumTimeClockStatus",TimeClockStatus.Break.ToString());
			Employees.Update(EmpCur);
			Phones.SetPhoneStatus(ClockStatusEnum.Break,extension);
			FillEmps();
		}

		///<summary>If already clocked in, this does nothing.  Returns false if not able to clock in due to security, or true if successful.</summary>
		private bool ClockIn(){
			long employeeNum=PhoneList[rowI].EmployeeNum;
			if(employeeNum==0){
				MsgBox.Show(this,"No employee at that extension.");
				return false;
			}
			if(ClockEvents.IsClockedIn(employeeNum)) {
				return true;
			}
			if(PrefC.GetBool(PrefName.TimecardSecurityEnabled)){
				if(Security.CurUser.EmployeeNum!=employeeNum){
					if(!Security.IsAuthorized(Permissions.TimecardsEditAll)){
						MsgBox.Show(this,"Not authorized.");
						return false;
					}
				}
			}
			try{
				ClockEvents.ClockIn(employeeNum);
			}
			catch{
				//This should never happen.  Fail silently.
				return true;
			}
			Employee EmpCur=Employees.GetEmp(employeeNum);
			EmpCur.ClockStatus=Lan.g(this,"Working");;
			Employees.Update(EmpCur);
			return true;
		}

		

	

	

		

		

		

	}
}
