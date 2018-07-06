using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.ComponentModel;
using System.Globalization;

namespace OpenDental {
	public partial class FormTaskSearch:ODForm {

		private DataTable _tableTasks;
		private List<Def> _listTaskPriorities;
		private List<Userod> _listUsers;
		public TaskObjectType GotoType;
		public long UserNum;
		public long GotoKeyNum;
		public bool IsSelectionMode;
		public long SelectedTaskNum;

		public FormTaskSearch() {
			InitializeComponent();
			Lan.F(this);
		}

		private void FormTaskSearch_Load(object sender,EventArgs e) {
			if(IsSelectionMode) {
				butClose.Text="Cancel";
			}
			//Note: DateTime strings that are empty actually are " " due to how the empty datetime control behaves.
			_listTaskPriorities=Defs.GetDefsForCategory(DefCat.TaskPriorities);
			long userNum=0;
			comboUsers.Items.Add(Lan.g(this,"All"));
			comboUsers.Items.Add(Lan.g(this,"Me"));
			comboUsers.SelectedIndex=0;//Always default to All.
			_listUsers=Userods.GetDeepCopy();//List of all users for searching.  I figure we don't want to exclude hidden ones for searching.
			_listUsers.ForEach(x => comboUsers.Items.Add(x.UserName));
			comboPriority.Items.Add(Lan.g(this,"All"));
			for(int i=0;i<_listTaskPriorities.Count;i++) {
				comboPriority.Items.Add(_listTaskPriorities[i].ItemName);
			}
			comboPriority.SelectedIndex=0;
			checkLimit.Checked=true;
			if(PrefC.HasReportServer) {
				checkReportServer.Checked=true;
			}
			else {
				checkReportServer.Visible=false;
			}
			_tableTasks=Tasks.GetDataSet(userNum,new List<long>(),0," "," "," "," ",textDescription.Text,0,0,checkBoxIncludesTaskNotes.Checked,
				checkBoxIncludeCompleted.Checked,true,checkReportServer.Checked);
			FillGrid();
		}

		private void FillGrid() {
			gridTasks.BeginUpdate();
			gridTasks.Columns.Clear();
			gridTasks.Rows.Clear();
			ODGridColumn col=new ODGridColumn("Created",70,HorizontalAlignment.Left,false);
			gridTasks.Columns.Add(col);
			col=new ODGridColumn("Completed",70,HorizontalAlignment.Left,false);
			gridTasks.Columns.Add(col);
			col=new ODGridColumn("Description",0);
			gridTasks.Columns.Add(col);
			ODGridRow row;
			for(int i=0;i<_tableTasks.Rows.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(_tableTasks.Rows[i]["dateCreate"].ToString());
				row.Cells.Add(_tableTasks.Rows[i]["dateComplete"].ToString());
				row.Cells.Add(_tableTasks.Rows[i]["description"].ToString());
				row.Note=_tableTasks.Rows[i]["note"].ToString();
				row.ColorLborder=Color.Black;
				row.ColorText=Color.FromArgb(PIn.Int(_tableTasks.Rows[i]["color"].ToString()));
				gridTasks.Rows.Add(row);
				row.Tag=_tableTasks.Rows[i]["TaskNum"].ToString();
			}
			gridTasks.EndUpdate();
		}

		private void butRefresh_Click(object sender,EventArgs e) {
			RefreshTable();
			FillGrid();
		}

		private void RefreshTable() {
			long priority=0;
			if(comboPriority.SelectedIndex!=0) {
				priority=_listTaskPriorities[comboPriority.SelectedIndex-1].DefNum;
			}
			long userNum=0;
			if(comboUsers.SelectedIndex==1){//Me
				userNum=Security.CurUser.UserNum;
			}
			else if(comboUsers.SelectedIndex!=0) {
				userNum=_listUsers[comboUsers.SelectedIndex-2].UserNum;//1(All) + 1(Me)= 2
			}
			List<long> listTaskListNums=new List<long>();
			if(textTaskList.Text!="") {
				listTaskListNums=TaskLists.GetNumsByDescription(textTaskList.Text,checkReportServer.Checked);
				if(listTaskListNums.Count==0) {
					MsgBox.Show(this,"Task List not found.");
					return;
				}
			}
			long taskNum=0;
			if(textTaskNum.Text!="") {
				try {
					taskNum=PIn.Long(textTaskNum.Text);
				}
				catch {
					MsgBox.Show(this,"Invalid Task Num format.");
					return;
				}
			}
			long patNum=0;
			if(textPatNum.Text!="") {
				try {
					patNum=PIn.Long(textPatNum.Text);
				}
				catch {
					MsgBox.Show(this,"Invalid PatNum format.");
					return;
				}
			}
			_tableTasks=Tasks.GetDataSet(userNum,listTaskListNums,taskNum,dateCreatedFrom.Text,dateCreatedTo.Text,dateCompletedFrom.Text,
				dateCompletedTo.Text,textDescription.Text,priority,patNum,checkBoxIncludesTaskNotes.Checked,checkBoxIncludeCompleted.Checked,
				checkLimit.Checked,checkReportServer.Checked);
		}

		private void gridTasks_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			long taskNum=PIn.Long(gridTasks.Rows[e.Row].Tag.ToString());
			if(IsSelectionMode) {
				SelectedTaskNum=taskNum;
				DialogResult=DialogResult.OK;
				return;
			}
			Task task=Tasks.GetOne(taskNum);
			if(task!=null) {
				FormTaskEdit FormTE=new FormTaskEdit(task,task.Copy());
				FormTE.Show();
			}
			else {
				MsgBox.Show(this,"The task no longer exists.");
			}
		}

		private void butPatPicker_Click(object sender,EventArgs e) {
			FormPatientSelect FormPS=new FormPatientSelect();
			FormPS.SelectionModeOnly=true;
			if(FormPS.ShowDialog()==DialogResult.OK) {
				long patNum=FormPS.SelectedPatNum;
				textPatNum.Text=patNum.ToString();
			}
		}

		private void butUserPicker_Click(object sender,EventArgs e) {
			FormUserPick FormUP=new FormUserPick();
			FormUP.ListUserodsFiltered=Userods.GetDeepCopy();
			if(FormUP.ShowDialog()==DialogResult.OK) {
				comboUsers.SelectedIndex=_listUsers.FindIndex(x => x.UserNum==FormUP.SelectedUserNum)+2;
			}
		}

		private void dateCreatedFrom_ValueChanged(object sender,EventArgs e) {
			dateCreatedFrom.CustomFormat=CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
		}

		private void dateCreatedTo_ValueChanged(object sender,EventArgs e) {
			dateCreatedTo.CustomFormat=CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
		}

		private void dateCompletedFrom_ValueChanged(object sender,EventArgs e) {
			dateCompletedFrom.CustomFormat=CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
		}

		private void dateCompletedTo_ValueChanged(object sender,EventArgs e) {
			dateCompletedTo.CustomFormat=CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
		}

		private void butClearCreated_Click(object sender,EventArgs e) {
			dateCreatedFrom.Value=DateTime.UtcNow;
			dateCreatedTo.Value=DateTime.UtcNow;
			dateCreatedFrom.CustomFormat=" ";
			dateCreatedTo.CustomFormat=" ";
		}

		private void butClearCompleted_Click(object sender,EventArgs e) {
			dateCompletedFrom.Value=DateTime.UtcNow;
			dateCompletedTo.Value=DateTime.UtcNow;
			dateCompletedFrom.CustomFormat=" ";
			dateCompletedTo.CustomFormat=" ";
		}

		private void butNewTask_Click(object sender,EventArgs e) {
			FormTaskListSelect FormTLS = new FormTaskListSelect(TaskObjectType.Patient);
			FormTLS.Text=Lan.g(FormTLS,"Add Task")+" - "+FormTLS.Text;
			FormTLS.ShowDialog();
			if(FormTLS.DialogResult!=DialogResult.OK || FormTLS.ListSelectedLists[0]==0) {
				return;
			}
			Task task = new Task() { TaskListNum=-1 };//don't show it in any list yet.
			Tasks.Insert(task);
			Task taskOld = task.Copy();
			task.UserNum=Security.CurUser.UserNum;
			task.TaskListNum=FormTLS.ListSelectedLists[0];
			FormTaskEdit FormTE = new FormTaskEdit(task,taskOld);
			FormTE.IsNew=true;
			FormTE.ShowDialog();//modal
			if(FormTE.DialogResult!=DialogResult.OK) {
				return;
			}
			SelectedTaskNum=task.TaskNum;
			DialogResult=DialogResult.OK;
			Close();
		}

		private void butClose_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
			Close();
		}

	}
}