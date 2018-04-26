using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenDentBusiness;
using System.Linq;

namespace OpenDental {
	public partial class FormUserPick:ODForm {
		///<summary>The filtered list of Users to pick from.</summary>
		public List<Userod> ListUserodsFiltered;
		public List<Userod> ListUserodsShowing;
		///<summary>If this form closes with OK, then this value will be filled.</summary>
		public long SelectedUserNum;
		///<summary>When IsMultiSelect, this is the list of selected users after the OK click.</summary>
		public List<long> ListSelectedUserNums=new List<long>();
		///<summary>If provided, this usernum will be preselected if it is also in the list of available usernums.</summary>
		public long SuggestedUserNum=0;
		///<summary>When IsMultiSelect, these usernums will be preselected if it is also in the list of available usernums.</summary>
		public List<long> ListSuggestedUserNums=new List<long>();
		public bool IsSelectionmode;
		public bool IsShowAllAllowed;
		///<summary>Will return 0 for SelectedUserNum if the None 
		public bool IsPickNoneAllowed;
		///<summary>Will return -1 for SelectedUserNum if the All 
		public bool IsPickAllAllowed;
		///<summary>Set true when we want to allow multiple user selections.  When true uses ListSelectedUsers</summary>
		private bool _isMultiSelect;

		public FormUserPick(bool isMultiSelect=false) {
			InitializeComponent();
			Lan.F(this);
			_isMultiSelect=isMultiSelect;
		}

		private void FormUserPick_Load(object sender,EventArgs e) {
			if(IsShowAllAllowed && ListUserodsFiltered!=null && ListUserodsFiltered.Count>0) {
				butShow.Visible=true;
			}
			if(IsPickAllAllowed) {
				butAll.Visible=true;
			}
			if(IsPickNoneAllowed) {
				butNone.Visible=true;
			}
			if(!butNone.Visible && !butAll.Visible) {
				groupSelect.Visible=false;
			}
			if(_isMultiSelect) {
				listUser.SelectionMode=SelectionMode.MultiExtended;
				Text=Lans.g(this,"Pick Users");
			}
			FillList(ListUserodsFiltered);
		}

		private void FillList(List<Userod> listUserods) {
			if(listUserods==null) {
				listUserods=Userods.GetDeepCopy(true);
			}
			ListUserodsShowing=listUserods.Select(x => x.Copy()).ToList();
			listUserods.ForEach(x => listUser.Items.Add(x));
			if(_isMultiSelect) {
				foreach(long userNum in ListSuggestedUserNums) {
					int index=listUserods.FindIndex(x => x.UserNum==userNum);
					listUser.SetSelected(index,true);
				}
			}
			else { 
				listUser.SelectedIndex=listUserods.FindIndex(x => x.UserNum==SuggestedUserNum);
			}
		}

		private void listUser_DoubleClick(object sender,EventArgs e) {
			if(listUser.SelectedIndex==-1) {
				return;
			}
			if(!Security.IsAuthorized(Permissions.TaskEdit,true) && Userods.GetInbox(ListUserodsShowing[listUser.SelectedIndex].UserNum)!=0 && !IsSelectionmode) {
				MsgBox.Show(this,"Please select a user that does not have an inbox.");
				return;
			}
			SelectedUserNum=ListUserodsShowing[listUser.SelectedIndex].UserNum;
			foreach(int index in listUser.SelectedIndices) {
				ListSelectedUserNums.Add(ListUserodsShowing[index].UserNum);
			}
			DialogResult=DialogResult.OK;
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(listUser.SelectedIndex==-1) {
				MsgBox.Show(this,"Please pick a user first.");
				return;
			}
			if(!IsSelectionmode && !Security.IsAuthorized(Permissions.TaskEdit,true) && Userods.GetInbox(ListUserodsShowing[listUser.SelectedIndex].UserNum)!=0) {
				MsgBox.Show(this,"Please select a user that does not have an inbox.");
				return;
			}
			SelectedUserNum=ListUserodsShowing[listUser.SelectedIndex].UserNum;
			foreach(int index in listUser.SelectedIndices) {
				ListSelectedUserNums.Add(ListUserodsShowing[index].UserNum);
			}
			DialogResult=DialogResult.OK;
		}

		private void butAll_Click(object sender,EventArgs e) {
			SelectedUserNum=-1;
			ListSelectedUserNums=ListUserodsShowing.Select(x => x.UserNum).ToList();
			DialogResult=DialogResult.OK;
		}

		private void butNone_Click(object sender,EventArgs e) {
			SelectedUserNum=0;
			ListSelectedUserNums=new List<long>() { };
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender,EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}

		private void butShow_Click(object sender,EventArgs e) {
			SelectedUserNum=0;
			if(Text=="Show All") {
				Text="Show Filtered";
				FillList(null);
			}
			else {
				Text="Show All";
				FillList(ListUserodsFiltered);
			}
		}
	}
}