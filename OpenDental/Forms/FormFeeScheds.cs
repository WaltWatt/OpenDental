using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using System.Linq;

namespace OpenDental{
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormFeeScheds:ODForm {
		private OpenDental.UI.Button butAdd;
		private OpenDental.UI.Button butClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.ODGrid gridMain;
		private ListBox listType;
		private OpenDental.UI.Button butDown;
		private OpenDental.UI.Button butUp;
		private GroupBox groupBox7;
		private OpenDental.UI.Button butIns;
		private Label label6;
		private Label label1;
		private OpenDental.UI.Button butSort;
		private Label labelSort;
		private Label labelCleanUp;
		private UI.Button butCleanUp;
		private List<FeeSched> _listFeeSchedsForType;
		///<summary>Stale deep copy of _listFeeScheds to use with sync.</summary>
		private List<FeeSched> _listFeeSchedsOld;
		private bool _isSelectionMode;
		private UI.Button butOK;
		public long SelectedFeeSchedNum;
		///<summary>If IsSelectionMode then is a list of all non-hidden fee schedules.  Otherwise, uses the cache deep copy.</summary>
		private List<FeeSched> _listFeeScheds;

		///<summary></summary>
		public FormFeeScheds(bool isSelectionMode=true)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
			_isSelectionMode=isSelectionMode;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFeeScheds));
			this.butOK = new OpenDental.UI.Button();
			this.labelCleanUp = new System.Windows.Forms.Label();
			this.butCleanUp = new OpenDental.UI.Button();
			this.labelSort = new System.Windows.Forms.Label();
			this.butSort = new OpenDental.UI.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.butIns = new OpenDental.UI.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.listType = new System.Windows.Forms.ListBox();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butAdd = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.groupBox7.SuspendLayout();
			this.SuspendLayout();
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(412, 570);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 23;
			this.butOK.Text = "&OK";
			this.butOK.Visible = false;
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// labelCleanUp
			// 
			this.labelCleanUp.Location = new System.Drawing.Point(315, 511);
			this.labelCleanUp.Name = "labelCleanUp";
			this.labelCleanUp.Size = new System.Drawing.Size(161, 44);
			this.labelCleanUp.TabIndex = 22;
			this.labelCleanUp.Text = "Deletes any allowed fee schedules that are not in use.";
			// 
			// butCleanUp
			// 
			this.butCleanUp.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCleanUp.Autosize = true;
			this.butCleanUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCleanUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCleanUp.CornerRadius = 4F;
			this.butCleanUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butCleanUp.Location = new System.Drawing.Point(318, 484);
			this.butCleanUp.Name = "butCleanUp";
			this.butCleanUp.Size = new System.Drawing.Size(99, 24);
			this.butCleanUp.TabIndex = 21;
			this.butCleanUp.Text = "Clean Up Allowed";
			this.butCleanUp.Click += new System.EventHandler(this.butCleanUp_Click);
			// 
			// labelSort
			// 
			this.labelSort.Location = new System.Drawing.Point(315, 241);
			this.labelSort.Name = "labelSort";
			this.labelSort.Size = new System.Drawing.Size(123, 44);
			this.labelSort.TabIndex = 20;
			this.labelSort.Text = "Sorts by type and alphabetically";
			// 
			// butSort
			// 
			this.butSort.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSort.Autosize = true;
			this.butSort.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSort.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSort.CornerRadius = 4F;
			this.butSort.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butSort.Location = new System.Drawing.Point(318, 214);
			this.butSort.Name = "butSort";
			this.butSort.Size = new System.Drawing.Size(75, 24);
			this.butSort.TabIndex = 19;
			this.butSort.Text = "Sort";
			this.butSort.Click += new System.EventHandler(this.butSort_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(315, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 18);
			this.label1.TabIndex = 18;
			this.label1.Text = "Type";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.butIns);
			this.groupBox7.Controls.Add(this.label6);
			this.groupBox7.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox7.Location = new System.Drawing.Point(17, 570);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(340, 58);
			this.groupBox7.TabIndex = 17;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Check Ins Plan Fee Schedules";
			// 
			// butIns
			// 
			this.butIns.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butIns.Autosize = true;
			this.butIns.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butIns.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butIns.CornerRadius = 4F;
			this.butIns.Location = new System.Drawing.Point(248, 19);
			this.butIns.Name = "butIns";
			this.butIns.Size = new System.Drawing.Size(75, 24);
			this.butIns.TabIndex = 4;
			this.butIns.Text = "Go";
			this.butIns.Click += new System.EventHandler(this.butIns_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(6, 16);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(229, 39);
			this.label6.TabIndex = 5;
			this.label6.Text = "This tool will help make sure your insurance plans have the right fee schedules s" +
    "et.";
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(318, 157);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(75, 24);
			this.butDown.TabIndex = 16;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 1);
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(318, 125);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(75, 24);
			this.butUp.TabIndex = 15;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// listType
			// 
			this.listType.FormattingEnabled = true;
			this.listType.Location = new System.Drawing.Point(318, 26);
			this.listType.Name = "listType";
			this.listType.Size = new System.Drawing.Size(120, 69);
			this.listType.TabIndex = 12;
			this.listType.Click += new System.EventHandler(this.listType_Click);
			// 
			// gridMain
			// 
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(17, 12);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(278, 552);
			this.gridMain.TabIndex = 11;
			this.gridMain.Title = "FeeSchedules";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableFeeScheds";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(318, 437);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(75, 24);
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
			this.butClose.Location = new System.Drawing.Point(412, 604);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// FormFeeScheds
			// 
			this.ClientSize = new System.Drawing.Size(515, 644);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.labelCleanUp);
			this.Controls.Add(this.butCleanUp);
			this.Controls.Add(this.labelSort);
			this.Controls.Add(this.butSort);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox7);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.listType);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormFeeScheds";
			this.ShowInTaskbar = false;
			this.Text = "Fee Schedules";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormFeeSchedules_FormClosing);
			this.Load += new System.EventHandler(this.FormFeeSchedules_Load);
			this.groupBox7.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormFeeSchedules_Load(object sender, System.EventArgs e) {
			_listFeeScheds=FeeScheds.GetDeepCopy(_isSelectionMode);
			_listFeeSchedsOld=_listFeeScheds.Select(x => x.Copy()).ToList();
			listType.Items.Add(Lan.g(this,"All"));
			Array arrayValues=Enum.GetValues(typeof(FeeScheduleType));
			for(int i=0;i<arrayValues.Length;i++) {
				FeeScheduleType feeSchedType=((FeeScheduleType)arrayValues.GetValue(i));
				if(feeSchedType==FeeScheduleType.OutNetwork) {
					listType.Items.Add("Out of Network");
				}
				else {
					listType.Items.Add(arrayValues.GetValue(i).ToString());
				}
			}
			listType.SelectedIndex=0;
			if(!Security.IsAuthorized(Permissions.SecurityAdmin,true)){
				butCleanUp.Visible=false;
				labelCleanUp.Visible=false;
			}
			if(_isSelectionMode) {
				butOK.Visible=true;
				butUp.Visible=false;
				butDown.Visible=false;
				butSort.Visible=false;
				labelSort.Visible=false;
				butAdd.Visible=false;
				butCleanUp.Visible=false;
				labelCleanUp.Visible=false;
				groupBox7.Visible=false;
			}
			FillGrid();
		}

		private void FillGrid(){
			if(listType.SelectedIndex==0){ //All option
				_listFeeSchedsForType=_listFeeScheds;
			}
			else{
				_listFeeSchedsForType=FeeScheds.GetListForType((FeeScheduleType)(listType.SelectedIndex-1),true,_listFeeScheds);
			}
			_listFeeSchedsForType.Sort(CompareItemOrder);
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableFeeScheds","Description"),145);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableFeeScheds","Type"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableFeeScheds","Hidden"),60,HorizontalAlignment.Center);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			for(int i=0;i<_listFeeSchedsForType.Count;i++){
				if(_isSelectionMode && _listFeeSchedsForType[i].IsHidden) {
					continue;
				}
				row=new ODGridRow();
				row.Tag=_listFeeSchedsForType[i];
				row.Cells.Add(_listFeeSchedsForType[i].Description);
				row.Cells.Add(_listFeeSchedsForType[i].FeeSchedType.ToString());
				if(_listFeeSchedsForType[i].IsHidden){
					row.Cells.Add("X");
				}
				else{
					row.Cells.Add("");
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			FormFeeSchedEdit FormF=new FormFeeSchedEdit();
			FormF.FeeSchedCur=new FeeSched();
			FormF.FeeSchedCur.IsNew=true;
			FormF.FeeSchedCur.ItemOrder=_listFeeScheds.Count;
			FormF.ListFeeScheds=_listFeeScheds;
			if(listType.SelectedIndex>0){
				FormF.FeeSchedCur.FeeSchedType=(FeeScheduleType)(listType.SelectedIndex-1);
			}
			FormF.ShowDialog();
			FillGrid();
			for(int i=0;i<_listFeeSchedsForType.Count;i++){
				if(FormF.FeeSchedCur.FeeSchedNum==_listFeeSchedsForType[i].FeeSchedNum){
					gridMain.SetSelected(i,true);
				}
			}
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e) {
			if(_isSelectionMode) {
				SelectedFeeSchedNum=((FeeSched)gridMain.Rows[e.Row].Tag).FeeSchedNum;
				DialogResult=DialogResult.OK;
				Close();
				return;
			}
			FormFeeSchedEdit FormF=new FormFeeSchedEdit();
			FormF.FeeSchedCur=_listFeeSchedsForType[e.Row];
			FormF.ListFeeScheds=_listFeeScheds;
			FormF.ShowDialog();
			FillGrid();
			for(int i=0;i<_listFeeSchedsForType.Count;i++){
				if(FormF.FeeSchedCur.FeeSchedNum==_listFeeSchedsForType[i].FeeSchedNum){
					gridMain.SetSelected(i,true);
				}
			}
		}

		private void listType_Click(object sender,EventArgs e) {
			if(listType.SelectedIndex==0){
				butSort.Enabled=true;
			}
			else{
				butSort.Enabled=false;
			}
			FillGrid();
		}

		private void butUp_Click(object sender,EventArgs e) {
			int idx=gridMain.GetSelectedIndex();
			if(idx==-1){
				MsgBox.Show(this,"Please select a fee schedule first.");
				return;
			}
			if(idx==0){
				return;
			}
			//swap the orders.  This makes it work no matter which types are being viewed.
			int order1=_listFeeSchedsForType[idx-1].ItemOrder;
			int order2=_listFeeSchedsForType[idx].ItemOrder;
			_listFeeSchedsForType[idx-1].ItemOrder=order2;
			_listFeeSchedsForType[idx].ItemOrder=order1;
			FillGrid();
			gridMain.SetSelected(idx-1,true);
		}

		private void butDown_Click(object sender,EventArgs e) {
			int idx=gridMain.GetSelectedIndex();
			if(idx==-1){
				MsgBox.Show(this,"Please select a fee schedule first.");
				return;
			}
			if(idx==_listFeeSchedsForType.Count-1){
				return;
			}
			int order1=_listFeeSchedsForType[idx].ItemOrder;
			int order2=_listFeeSchedsForType[idx+1].ItemOrder;
			_listFeeSchedsForType[idx].ItemOrder=order2;
			_listFeeSchedsForType[idx+1].ItemOrder=order1;
			FillGrid();
			gridMain.SetSelected(idx+1,true);
		}

		private void butSort_Click(object sender,EventArgs e) {
			//only enabled if viewing all types
			//FeeSchedComparer comparer=new FeeSchedComparer();
			_listFeeSchedsForType.Sort(CompareFeeScheds);
			for(int i=0;i<_listFeeSchedsForType.Count;i++) {
				if(_listFeeSchedsForType[i].ItemOrder!=i) {
					_listFeeSchedsForType[i].ItemOrder=i;
				}
			}
			FillGrid();
		}

		///<summary>This sorts feescheds by their item order.</summary>
		private static int CompareItemOrder(FeeSched feeSched1,FeeSched feeSched2) {
			return feeSched1.ItemOrder.CompareTo(feeSched2.ItemOrder);
		}

		///<summary>This sorts feescheds by type and alphabetically.</summary>
		private static int CompareFeeScheds(FeeSched feeSched1,FeeSched feeSched2) {
			if(feeSched1==null){
				if(feeSched2==null){
					return 0;//both null, so equal
				}
				else{
					return -1;
				}
			}
			if(feeSched2==null){
				return 1;
			}
			if(feeSched1.FeeSchedType!=feeSched2.FeeSchedType){
				return feeSched1.FeeSchedType.CompareTo(feeSched2.FeeSchedType);
			}
			return feeSched1.Description.CompareTo(feeSched2.Description);
		}

		private void butIns_Click(object sender,EventArgs e) {
			FormFeesForIns FormF=new FormFeesForIns();
			FormF.ShowDialog();
			//DialogResult=DialogResult.OK;
		}

		private void butCleanUp_Click(object sender,EventArgs e) {
			if(!MsgBox.Show(this,MsgBoxButtons.OKCancel,"Delete allowed fee schedules that are not in use or that are attached to hidden insurance plans?")) {
				return;
			}
			long changed=FeeScheds.CleanupAllowedScheds();
			MessageBox.Show(changed.ToString()+" "+Lan.g(this,"unused fee schedules deleted."));
			FeeScheds.RefreshCache();
			_listFeeScheds=FeeScheds.GetDeepCopy(_isSelectionMode);  //After deletion, refresh in-memory copy to continue editing.
			_listFeeSchedsOld=_listFeeScheds.Select(x => x.Copy()).ToList();
			FillGrid();
		}

		private void butOK_Click(object sender,EventArgs e) {
			if(_isSelectionMode && gridMain.SelectedIndices.Length>0) {
				SelectedFeeSchedNum=((FeeSched)gridMain.Rows[gridMain.GetSelectedIndex()].Tag).FeeSchedNum;
			}
			DialogResult=DialogResult.OK;
			Close();
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			SelectedFeeSchedNum=0;
			DialogResult=DialogResult.Cancel;
			Close();
		}

		private void FormFeeSchedules_FormClosing(object sender,FormClosingEventArgs e) {
			//Renumber itemorder for the entire list before closing, this is defensive to prevent duplicate itemorder values
			//Changes in the form using Up and Down affect ListFeeScheds, so the order of the list is the order that the user made.
			for(int i=0;i<_listFeeScheds.Count;i++) {
				if(_listFeeScheds[i].ItemOrder!=i) {
					_listFeeScheds[i].ItemOrder=i;
				}
			}
			//Only send a signal if changes were made during the sync.  Changes can't be made if in selection mode.
			if(!_isSelectionMode && FeeScheds.Sync(_listFeeScheds,_listFeeSchedsOld)) {
				DataValid.SetInvalid(InvalidType.FeeScheds);
			}
		}
		

		
		
	}

	
		

	
}





















