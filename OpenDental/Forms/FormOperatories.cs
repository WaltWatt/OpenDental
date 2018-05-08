using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Linq;
using CodeBase;

namespace OpenDental{
	///<summary></summary>
	public class FormOperatories : ODForm {
		private OpenDental.UI.Button butAdd;
		private OpenDental.UI.Button butClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.ODGrid gridMain;
		private System.Windows.Forms.Label label1;
		private OpenDental.UI.Button butDown;
		private OpenDental.UI.Button butUp;
		private OpenDental.UI.ComboBoxClinic comboClinic;
		private Label labelClinic;
		///<summary>A list of all operatories used to sync at the end.</summary>
		private List<Operatory> _listOps;
		///<summary>Stale deep copy of _listOps to use with sync.</summary>
		private List<Operatory> _listOpsOld;
		private UI.Button butPickClinic;
		private UI.Button butCombine;
		///<summary>List of conflict appointments to show the user.
		///Only used for the combine operatories tool</summary>
		public List<Appointment> ListConflictingAppts=new List<Appointment>();

		///<summary></summary>
		public FormOperatories() {
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOperatories));
			this.label1 = new System.Windows.Forms.Label();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.comboClinic = new OpenDental.UI.ComboBoxClinic();
			this.labelClinic = new System.Windows.Forms.Label();
			this.butPickClinic = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.butCombine = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(20, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(251, 20);
			this.label1.TabIndex = 12;
			this.label1.Text = "(Also, see the appointment views section)";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(21, 31);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(759, 432);
			this.gridMain.TabIndex = 11;
			this.gridMain.Title = "Operatories";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableOperatories";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// comboClinic
			// 
			this.comboClinic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.comboClinic.DoIncludeAll = true;
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(530, 8);
			this.comboClinic.MaxDropDownItems = 30;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(226, 21);
			this.comboClinic.TabIndex = 119;
			this.comboClinic.SelectionChangeCommitted += new System.EventHandler(this.comboClinic_SelectionChangeCommitted);
			// 
			// labelClinic
			// 
			this.labelClinic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.labelClinic.Location = new System.Drawing.Point(460, 12);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(67, 16);
			this.labelClinic.TabIndex = 120;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butPickClinic
			// 
			this.butPickClinic.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPickClinic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butPickClinic.Autosize = false;
			this.butPickClinic.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPickClinic.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPickClinic.CornerRadius = 2F;
			this.butPickClinic.Location = new System.Drawing.Point(757, 8);
			this.butPickClinic.Name = "butPickClinic";
			this.butPickClinic.Size = new System.Drawing.Size(23, 21);
			this.butPickClinic.TabIndex = 121;
			this.butPickClinic.Text = "...";
			this.butPickClinic.Click += new System.EventHandler(this.butPickClinic_Click);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(800, 235);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(80, 26);
			this.butDown.TabIndex = 14;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(800, 203);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(80, 26);
			this.butUp.TabIndex = 13;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(800, 31);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(80, 26);
			this.butAdd.TabIndex = 10;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.Location = new System.Drawing.Point(800, 437);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(80, 26);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// butCombine
			// 
			this.butCombine.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCombine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butCombine.Autosize = true;
			this.butCombine.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCombine.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCombine.CornerRadius = 4F;
			this.butCombine.Location = new System.Drawing.Point(800, 123);
			this.butCombine.Name = "butCombine";
			this.butCombine.Size = new System.Drawing.Size(80, 26);
			this.butCombine.TabIndex = 122;
			this.butCombine.Text = "Co&mbine";
			this.butCombine.Click += new System.EventHandler(this.butCombine_Click);
			// 
			// FormOperatories
			// 
			this.ClientSize = new System.Drawing.Size(898, 486);
			this.Controls.Add(this.butCombine);
			this.Controls.Add(this.butPickClinic);
			this.Controls.Add(this.comboClinic);
			this.Controls.Add(this.labelClinic);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(850, 350);
			this.Name = "FormOperatories";
			this.ShowInTaskbar = false;
			this.Text = "Operatories";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormOperatories_Closing);
			this.Load += new System.EventHandler(this.FormOperatories_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormOperatories_Load(object sender,System.EventArgs e) {
			if(!PrefC.HasClinicsEnabled) {
				labelClinic.Visible=false;
				butPickClinic.Visible=false;
			}
			RefreshList();
		}

		private void RefreshList() {
			Cache.Refresh(InvalidType.Operatories);
			_listOps=Operatories.GetDeepCopy();//Already ordered by ItemOrder
			_listOpsOld=_listOps.Select(x => x.Copy()).ToList();
			FillGrid();
		}

		private void FillGrid(){
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			int opNameWidth=180;
			int clinicWidth=85;
			if(!PrefC.HasClinicsEnabled) {
				//Clinics are hidden so add the width of the clinic column to the Op Name column because the clinic column will not show.
				opNameWidth+=clinicWidth;
			}
			ODGridColumn col=new ODGridColumn(Lan.g("TableOperatories","Op Name"),opNameWidth);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableOperatories","Abbrev"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableOperatories","IsHidden"),64,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			if(PrefC.HasClinicsEnabled) {
				col=new ODGridColumn(Lan.g("TableOperatories","Clinic"),clinicWidth);
				gridMain.Columns.Add(col);
			}
			col=new ODGridColumn(Lan.g("TableOperatories","Provider"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableOperatories","Hygienist"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableOperatories","IsHygiene"),64,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableOperatories","IsWebSched"),74,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableOperatories","IsNewPat"),0,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			UI.ODGridRow row;
			for(int i=0;i<_listOps.Count;i++){
				if(PrefC.HasClinicsEnabled 
					&& !comboClinic.IsAllSelected
					&& _listOps[i].ClinicNum!=comboClinic.SelectedClinicNum) 
				{
					continue;
				}
				row=new OpenDental.UI.ODGridRow();
				row.Cells.Add(_listOps[i].OpName);
				row.Cells.Add(_listOps[i].Abbrev);
				if(_listOps[i].IsHidden){
					row.Cells.Add("X");
				}
				else{
					row.Cells.Add("");
				}
				if(PrefC.HasClinicsEnabled) {
					row.Cells.Add(Clinics.GetAbbr(_listOps[i].ClinicNum));
				}
				row.Cells.Add(Providers.GetAbbr(_listOps[i].ProvDentist));
				row.Cells.Add(Providers.GetAbbr(_listOps[i].ProvHygienist));
				if(_listOps[i].IsHygiene){
					row.Cells.Add("X");
				}
				else{
					row.Cells.Add("");
				}
				row.Cells.Add(_listOps[i].IsWebSched?"X":"");
				row.Cells.Add((_listOps[i].ListWSNPAOperatoryDefNums!=null && _listOps[i].ListWSNPAOperatoryDefNums.Count > 0) ? "X" : "");
				row.Tag=_listOps[i];
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void gridMain_CellDoubleClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			FormOperatoryEdit FormOE=new FormOperatoryEdit((Operatory)gridMain.Rows[e.Row].Tag);
			FormOE.ListOps=_listOps;
			FormOE.ShowDialog();
			FillGrid();
		}

		private void comboClinic_SelectionChangeCommitted(object sender,EventArgs e) {
			FillGrid();
		}

		private void butPickClinic_Click(object sender,EventArgs e) {
			FormClinics FormC=new FormClinics();
			FormC.IsSelectionMode=true;
			FormC.ShowDialog();
			if(FormC.DialogResult!=DialogResult.OK) {
				return;
			}
			comboClinic.SelectedClinicNum=FormC.SelectedClinicNum;
			FillGrid();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			Operatory opCur=new Operatory();
			opCur.IsNew=true;
			if(PrefC.HasClinicsEnabled && !comboClinic.IsAllSelected && !comboClinic.IsNothingSelected) {
				opCur.ClinicNum=comboClinic.SelectedClinicNum;
			}
			if(gridMain.SelectedIndices.Length>0){//a row is selected
				opCur.ItemOrder=gridMain.SelectedIndices[0];
			}
			else{
				opCur.ItemOrder=_listOps.Count;//goes at end of list
			}
			FormOperatoryEdit FormE=new FormOperatoryEdit(opCur);
			FormE.ListOps=_listOps;
			FormE.IsNew=true;
			FormE.ShowDialog();
			if(FormE.DialogResult==DialogResult.Cancel){
				return;
			}
			FillGrid();
		}
		
		private void butCombine_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			if(gridMain.SelectedIndices.Length<2) {
				MsgBox.Show(this,"Please select multiple items first while holding down the control key.");
				return;
			}
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,
				"Combine all selected operatories into a single operatory?\r\n\r\n"
				+"This will affect all appointments set in these operatories and could take a while to run.  "
				+"The next window will let you select which operatory to keep when combining."))
			{
				return;
			}
			#region Get selected OperatoryNums
			bool hasNewOp=gridMain.SelectedIndices.ToList().Exists(x => ((Operatory)gridMain.Rows[x].Tag).IsNew);
			if(hasNewOp) {
				//This is needed due to the user adding an operatory then clicking combine.
				//The newly added operatory does not hava and OperatoryNum , so sync.
				ReorderAndSync();
				DataValid.SetInvalid(InvalidType.Operatories);//With sync we don't know if anything changed.
				RefreshList();//Without this the user could cancel out of a prompt below and quickly close FormOperatories, causing a duplicate op from sync.
			}
			List<long> listSelectedOpNums=new List<long>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				listSelectedOpNums.Add(((Operatory)gridMain.Rows[gridMain.SelectedIndices[i]].Tag).OperatoryNum);
			}
			#endregion
			#region Determine what Op to keep as the 'master'
			FormOperatoryPick FormOP=new FormOperatoryPick(_listOps.FindAll(x => listSelectedOpNums.Contains(x.OperatoryNum)));
			FormOP.ShowDialog();
			if(FormOP.DialogResult!=DialogResult.OK) {
				return;
			}
			long masterOpNum=FormOP.SelectedOperatoryNum;
			#endregion
			#region Determine if any appts conflict exist and potentially show them
			//List of all appointments for all child ops. If conflict was detected the appointments OperatoryNum will bet set to -1
			List<ODTuple<Appointment,bool>> listTupleApptsToMerge=Operatories.MergeApptCheck(masterOpNum,listSelectedOpNums.FindAll(x => x!=masterOpNum));
			ListConflictingAppts=listTupleApptsToMerge.Where(x => x.Item2).Select(x => x.Item1).ToList();
			List<Appointment> listApptsToMerge=listTupleApptsToMerge.Select(x => x.Item1).ToList();
			if(ListConflictingAppts.Count > 0) {//Appointments conflicts exist, can not merge
				if(!MsgBox.Show(this,true,"Cannot merge operatories due to appointment conflicts.\r\n\r\n"
					+"These conflicts need to be resolved before combining can occur.\r\n"
					+"Click OK to view the conflicting appointments."))
				{
					ListConflictingAppts.Clear();
					return;
				}
				Close();//Having ListConflictingAppts filled with appointments will cause outside windows that care to show the corresponding window.
				return;
			}
			#endregion
			#region Final prompt, displays number of appts to move and the 'master' ops abbr.
			int apptCount=listApptsToMerge.FindAll(x => x.Op!=masterOpNum).Count;
			if(apptCount > 0) {
				string selectedOpName=_listOps.First(x => x.OperatoryNum==masterOpNum).Abbrev;//Safe
				if(MessageBox.Show(Lan.g(this,"Would you like to move")+" "+apptCount+" "
					+Lan.g(this,"appointments from their current operatories to")+" "+selectedOpName+"?\r\n\r\n"
					+Lan.g(this,"You cannot undo this!")
					,"WARNING"
					,MessageBoxButtons.OKCancel)!=DialogResult.OK) 
				{
					return;
				}
			}
			#endregion
			//Point of no return.
			if(!hasNewOp) {//Avoids running ReorderAndSync() twice.
				//The user could have made other changes within this window, we need to make sure to save those changes before merging.
				ReorderAndSync();
				DataValid.SetInvalid(InvalidType.Operatories);//With sync we don't know if anything changed.
			}
			try {
				Operatories.MergeOperatoriesIntoMaster(masterOpNum,listSelectedOpNums,listApptsToMerge);
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
				return;
			}
			MessageBox.Show(Lan.g("Operatories","The following operatories and all of their appointments were merged into the")
					+" "+_listOps.FirstOrDefault(x => x.OperatoryNum==masterOpNum).Abbrev+" "+Lan.g("Operatories","operatory;")+"\r\n"
					+string.Join(", ",_listOps.FindAll(x => x.OperatoryNum!=masterOpNum && listSelectedOpNums.Contains(x.OperatoryNum)).Select(x => x.Abbrev)));
			RefreshList();
			FillGrid();
		}

		private void butUp_Click(object sender, System.EventArgs e) {
			if(gridMain.SelectedIndices.Length==0){
				MsgBox.Show(this,"You must first select a row.");
				return;
			}
			int selected = gridMain.GetSelectedIndex();
			if(selected==0){
				return;//already at the top
			}
			Operatory selectedOp = (Operatory)gridMain.Rows[selected].Tag;
			Operatory aboveSelectedOp = (Operatory)gridMain.Rows[selected - 1].Tag;
			string strErr;
			if(!CanReorderOps(selectedOp,aboveSelectedOp,out strErr)) {
				MessageBox.Show(strErr); //already translated
				return;
			}
			int selectedItemOrder=selectedOp.ItemOrder;
			//move selected item up
			selectedOp.ItemOrder=aboveSelectedOp.ItemOrder;
			//move the one above it down
			aboveSelectedOp.ItemOrder=selectedItemOrder;
			//Swap positions
			_listOps = _listOps.OrderBy(x => x.ItemOrder).ToList();
			//FillGrid();  //We don't fill grid anymore because it takes too long and we dont need to pull any new data into the DB.
			SwapGridMainLocations(selected,selected-1);
			gridMain.SetSelected(selected-1,true);
		}

		private void butDown_Click(object sender, System.EventArgs e) {
			if(gridMain.SelectedIndices.Length==0){
				MsgBox.Show(this,"You must first select a row.");
				return;
			}
			int selected = gridMain.GetSelectedIndex();
			if(selected == gridMain.Rows.Count - 1) {
				return;//already at the bottom
			}
			Operatory selectedOp = (Operatory)gridMain.Rows[selected].Tag;
			Operatory belowSelectedOp = (Operatory)gridMain.Rows[selected + 1].Tag;
			string strErr;
			if(!CanReorderOps(selectedOp,belowSelectedOp,out strErr)) {
				MessageBox.Show(strErr); //already translated
				return;
			}
			int selectedItemOrder = selectedOp.ItemOrder;
			//move selected item down
			selectedOp.ItemOrder=belowSelectedOp.ItemOrder;
			//move the one below it up
			belowSelectedOp.ItemOrder=selectedItemOrder;
			//Swap positions
			_listOps = _listOps.OrderBy(x => x.ItemOrder).ToList();
			//FillGrid();  //We don't fill grid anymore because it takes too long and we dont need to pull any new data into the DB.
			SwapGridMainLocations(selected,selected+1);
			gridMain.SetSelected(selected+1,true);
		}

		///<summary>Returns true if the two operatories can be reordered. Returns false and fills in the error string if they cannot.</summary>
		private bool CanReorderOps(Operatory op1,Operatory op2,out string strErrorMsg) {
			strErrorMsg="";
			if(!PrefC.HasClinicsEnabled || comboClinic.IsAllSelected) {
				return true;
			}
			if(!op1.IsInHQView && !op2.IsInHQView) {
				return true;
			}
			strErrorMsg=Lan.g(this,"You cannot change the order of the Operatories")+" '"+op1.Abbrev+"' "+Lan.g(this,"and")+" '"+op2.Abbrev+"' "
				+Lan.g(this,"with Clinic")+" '"+comboClinic.SelectedAbbr+"' "
				+Lan.g(this,"selected because it is also a member of a Headquarters Appointment View.") +" "
				+Lan.g(this,"You must set your clinic selection to 'All' to reorder these operatories.");
			return false;
		}

		///<summary>Swaps two rows in the grid for use with the Up and Down buttons.  Does not edit lists or do any calls to cache or DB to refresh the grid.</summary>
		private void SwapGridMainLocations(int indxMoveFrom, int indxMoveTo) {
			gridMain.BeginUpdate();
			ODGridRow dataRow=gridMain.Rows[indxMoveFrom];
			gridMain.Rows.RemoveAt(indxMoveFrom);
			gridMain.Rows.Insert(indxMoveTo,dataRow);
			gridMain.EndUpdate();		
		}

		///<summary>Syncs _listOps and _listOpsOld after correcting the order of _listOps.</summary>
		private void ReorderAndSync() {
			//Renumber the itemorders to match the grid.  In most cases this will not do anything, but will fix any duplicate itemorders.
			for(int i=0;i<_listOps.Count;i++) {
				_listOps[i].ItemOrder=i;
			}
			Operatories.Sync(_listOps,_listOpsOld);
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		private void FormOperatories_Closing(object sender,System.ComponentModel.CancelEventArgs e) {
			ReorderAndSync();
			DataValid.SetInvalid(InvalidType.Operatories);//With sync we don't know if anything changed.
		}
	}
}





















