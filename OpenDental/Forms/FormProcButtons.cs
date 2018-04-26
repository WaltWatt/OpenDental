using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.UI;
using System.Collections.Generic;
using CodeBase;

namespace OpenDental{
///<summary></summary>
	public class FormProcButtons : ODForm {
		private OpenDental.UI.Button butClose;
		private OpenDental.UI.Button butAdd;
		private OpenDental.UI.Button butDelete;
		private OpenDental.UI.Button butDown;
		private OpenDental.UI.Button butUp;
		private IContainer components;
		private Label label1;
		private Label label2;
		private ListBox listCategories;
		private OpenDental.UI.Button butEdit;
		private bool changed;
		///<summary>defnum</summary>
		private long selectedCat;
		private ListView listViewButtons;
		private ColumnHeader columnHeader1;
		private ImageList imageListProcButtons;
		///<summary>This list of displayed buttons for the selected cat.</summary>
		private ProcButton[] ButtonList;
		private ODButtonPanel panelQuickButtons;
		private Label labelEdit;
		private List<ProcButtonQuick> listProcButtonQuicks;
		private List<Def> _listProcButtonCatDefs;

		///<summary></summary>
		public FormProcButtons(){
			InitializeComponent();
			Lan.F(this);
		}

		///<summary></summary>
		protected override void Dispose( bool disposing ){
			if( disposing ){
				if(components != null){
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		private void InitializeComponent(){
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProcButtons));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.listCategories = new System.Windows.Forms.ListBox();
			this.listViewButtons = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.imageListProcButtons = new System.Windows.Forms.ImageList(this.components);
			this.labelEdit = new System.Windows.Forms.Label();
			this.panelQuickButtons = new OpenDental.UI.ODButtonPanel();
			this.butEdit = new OpenDental.UI.Button();
			this.butDown = new OpenDental.UI.Button();
			this.butUp = new OpenDental.UI.Button();
			this.butAdd = new OpenDental.UI.Button();
			this.butDelete = new OpenDental.UI.Button();
			this.butClose = new OpenDental.UI.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(324, 33);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(237, 22);
			this.label1.TabIndex = 36;
			this.label1.Text = "Buttons for the selected category";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(36, 33);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(237, 22);
			this.label2.TabIndex = 38;
			this.label2.Text = "Button Categories";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// listCategories
			// 
			this.listCategories.Location = new System.Drawing.Point(38, 59);
			this.listCategories.Name = "listCategories";
			this.listCategories.Size = new System.Drawing.Size(234, 316);
			this.listCategories.TabIndex = 37;
			this.listCategories.Click += new System.EventHandler(this.listCategories_Click);
			// 
			// listViewButtons
			// 
			this.listViewButtons.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.listViewButtons.AutoArrange = false;
			this.listViewButtons.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
			this.listViewButtons.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.listViewButtons.HideSelection = false;
			this.listViewButtons.Location = new System.Drawing.Point(326, 59);
			this.listViewButtons.MultiSelect = false;
			this.listViewButtons.Name = "listViewButtons";
			this.listViewButtons.Size = new System.Drawing.Size(234, 316);
			this.listViewButtons.SmallImageList = this.imageListProcButtons;
			this.listViewButtons.TabIndex = 189;
			this.listViewButtons.UseCompatibleStateImageBehavior = false;
			this.listViewButtons.View = System.Windows.Forms.View.Details;
			this.listViewButtons.DoubleClick += new System.EventHandler(this.listViewButtons_DoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Width = 155;
			// 
			// imageListProcButtons
			// 
			this.imageListProcButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListProcButtons.ImageStream")));
			this.imageListProcButtons.TransparentColor = System.Drawing.Color.Transparent;
			this.imageListProcButtons.Images.SetKeyName(0, "deposit.gif");
			// 
			// labelEdit
			// 
			this.labelEdit.Location = new System.Drawing.Point(326, 244);
			this.labelEdit.Name = "labelEdit";
			this.labelEdit.Size = new System.Drawing.Size(235, 72);
			this.labelEdit.TabIndex = 204;
			this.labelEdit.Text = "The Quick Buttons category allows custom placement of buttons and labels.  Double" +
    " click anywhere on panel above to add or edit an item.";
			// 
			// panelQuickButtons
			// 
			this.panelQuickButtons.Location = new System.Drawing.Point(326, 59);
			this.panelQuickButtons.Name = "panelQuickButtons";
			this.panelQuickButtons.Size = new System.Drawing.Size(195, 182);
			this.panelQuickButtons.TabIndex = 203;
			this.panelQuickButtons.UseBlueTheme = false;
			this.panelQuickButtons.RowDoubleClick += new OpenDental.UI.ODButtonPanelEventHandler(this.panelQuickButtons_RowDoubleClick);
			// 
			// butEdit
			// 
			this.butEdit.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butEdit.Autosize = true;
			this.butEdit.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEdit.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEdit.CornerRadius = 4F;
			this.butEdit.Image = global::OpenDental.Properties.Resources.Add;
			this.butEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEdit.Location = new System.Drawing.Point(38, 395);
			this.butEdit.Name = "butEdit";
			this.butEdit.Size = new System.Drawing.Size(109, 26);
			this.butEdit.TabIndex = 39;
			this.butEdit.Text = "Edit Categories";
			this.butEdit.Click += new System.EventHandler(this.butEdit_Click);
			// 
			// butDown
			// 
			this.butDown.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDown.Autosize = true;
			this.butDown.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDown.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDown.CornerRadius = 4F;
			this.butDown.Image = global::OpenDental.Properties.Resources.down;
			this.butDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDown.Location = new System.Drawing.Point(478, 433);
			this.butDown.Name = "butDown";
			this.butDown.Size = new System.Drawing.Size(82, 26);
			this.butDown.TabIndex = 34;
			this.butDown.Text = "&Down";
			this.butDown.Click += new System.EventHandler(this.butDown_Click);
			// 
			// butUp
			// 
			this.butUp.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butUp.Autosize = true;
			this.butUp.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUp.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUp.CornerRadius = 4F;
			this.butUp.Image = global::OpenDental.Properties.Resources.up;
			this.butUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butUp.Location = new System.Drawing.Point(478, 395);
			this.butUp.Name = "butUp";
			this.butUp.Size = new System.Drawing.Size(82, 26);
			this.butUp.TabIndex = 35;
			this.butUp.Text = "&Up";
			this.butUp.Click += new System.EventHandler(this.butUp_Click);
			// 
			// butAdd
			// 
			this.butAdd.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butAdd.Autosize = true;
			this.butAdd.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butAdd.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butAdd.CornerRadius = 4F;
			this.butAdd.Image = global::OpenDental.Properties.Resources.Add;
			this.butAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butAdd.Location = new System.Drawing.Point(326, 395);
			this.butAdd.Name = "butAdd";
			this.butAdd.Size = new System.Drawing.Size(82, 26);
			this.butAdd.TabIndex = 32;
			this.butAdd.Text = "&Add";
			this.butAdd.Click += new System.EventHandler(this.butAdd_Click);
			// 
			// butDelete
			// 
			this.butDelete.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butDelete.Autosize = true;
			this.butDelete.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butDelete.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butDelete.CornerRadius = 4F;
			this.butDelete.Image = global::OpenDental.Properties.Resources.deleteX;
			this.butDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butDelete.Location = new System.Drawing.Point(326, 433);
			this.butDelete.Name = "butDelete";
			this.butDelete.Size = new System.Drawing.Size(82, 26);
			this.butDelete.TabIndex = 33;
			this.butDelete.Text = "&Delete";
			this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
			// 
			// butClose
			// 
			this.butClose.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butClose.Autosize = true;
			this.butClose.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butClose.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butClose.CornerRadius = 4F;
			this.butClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butClose.Location = new System.Drawing.Point(648, 433);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 8;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// FormProcButtons
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(746, 483);
			this.Controls.Add(this.labelEdit);
			this.Controls.Add(this.panelQuickButtons);
			this.Controls.Add(this.listViewButtons);
			this.Controls.Add(this.butEdit);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.listCategories);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.butDown);
			this.Controls.Add(this.butUp);
			this.Controls.Add(this.butAdd);
			this.Controls.Add(this.butDelete);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormProcButtons";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Setup Procedure Buttons";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FormProcButtons_Closing);
			this.Load += new System.EventHandler(this.FormChartProcedureEntry_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormChartProcedureEntry_Load(object sender,System.EventArgs e) {
			fillPanelQuickButtons();
			ResizeControls();
			FillCategories();
			FillButtons();
			SetVisibility(); 
		}

		private void SetVisibility() {
			foreach(Control c in this.Controls) {//make all controls visible. Then hide below.
				c.Visible=true;
			}
			if(listCategories.SelectedIndex==0) {
				listViewButtons.Visible=false;
				butAdd.Visible=false;
				butDelete.Visible=false;
				butUp.Visible=false;
				butDown.Visible=false;
			}
			else {
				panelQuickButtons.Visible=false;
				labelEdit.Visible=false;
			}
		}

		///<summary>Make the QuickButtonGrid exactly the same size as it will display in the chart module.</summary>
		private void ResizeControls() {
			try {
				Control[] controlArray=this.Owner.Controls.Find("ContrChart",true);
				//force redraw and resize of control, Also deselects current patient.
				((ContrChart)controlArray[0]).ModuleSelected(0);
				controlArray=this.Owner.Controls.Find("panelQuickButtons",true);
				//set display size to actual size in from the control module. This is a dynamically sized control.
				panelQuickButtons.Size=controlArray[0].Size;
				labelEdit.Location=new Point(panelQuickButtons.Location.X,panelQuickButtons.Bounds.Bottom+20);
			}
			catch(Exception ex) {
				ex.DoNothing();
				//could not locate the gridquickbuttons control.
			}

		}

		private void fillPanelQuickButtons() {
			panelQuickButtons.BeginUpdate();
			panelQuickButtons.Items.Clear();
			listProcButtonQuicks=ProcButtonQuicks.GetAll();
			listProcButtonQuicks.Sort(ProcButtonQuicks.sortYX);
			ODPanelItem pItem;
			for(int i=0;i<listProcButtonQuicks.Count;i++) {
				pItem=new ODPanelItem();
				pItem.Text=listProcButtonQuicks[i].Description;
				pItem.YPos=listProcButtonQuicks[i].YPos;
				pItem.ItemOrder=listProcButtonQuicks[i].ItemOrder;
				pItem.ItemType=(listProcButtonQuicks[i].IsLabel?ODPanelItemType.Label:ODPanelItemType.Button);
				pItem.Tags.Add(listProcButtonQuicks[i]);
				panelQuickButtons.Items.Add(pItem);
			}
			panelQuickButtons.EndUpdate();
		}

		private void FillCategories(){
			ProcButtonQuicks.ValidateAll();
			listCategories.Items.Clear();
			listCategories.Items.Add("Quick Buttons");//hardcoded category.
			_listProcButtonCatDefs=Defs.GetDefsForCategory(DefCat.ProcButtonCats,true);
			if(_listProcButtonCatDefs.Count==0){
				selectedCat=0;
				listCategories.SelectedIndex=0;
				return;
			}
			for(int i=0;i<_listProcButtonCatDefs.Count;i++){
				listCategories.Items.Add(_listProcButtonCatDefs[i].ItemName);
				if(selectedCat==_listProcButtonCatDefs[i].DefNum){
					listCategories.SelectedIndex=i+1;
				}
			}
			if(listCategories.SelectedIndex==-1){//category was hidden, or just openning the form
				listCategories.SelectedIndex=0;
				selectedCat=0;
			}
			if(listCategories.SelectedIndex>0) {//hardcoded category doesn't have a DefNum.
				selectedCat=_listProcButtonCatDefs[listCategories.SelectedIndex-1].DefNum;
			}
		}

		private void FillButtons(){
			listViewButtons.Items.Clear();
			imageListProcButtons.Images.Clear();
			if(selectedCat==0) {
				//empty button list and return because we will be using and OD grid to display these buttons.
				ButtonList=new ProcButton[0];
				return;
			}
			ProcButtons.RefreshCache();
			ButtonList=ProcButtons.GetForCat(selectedCat);
			//first check and fix any order problems
			for(int i=0;i<ButtonList.Length;i++) {
				if(ButtonList[i].ItemOrder!=i) {
					ButtonList[i].ItemOrder=i;
					ProcButtons.Update(ButtonList[i]);
				}
			}
			ListViewItem item;
			for(int i=0;i<ButtonList.Length;i++) {
				if(ButtonList[i].ButtonImage!=""){
					//image keys are simply the ProcButtonNum
					try {
						imageListProcButtons.Images.Add(ButtonList[i].ProcButtonNum.ToString(),PIn.Bitmap(ButtonList[i].ButtonImage));
					}
					catch {
						imageListProcButtons.Images.Add(new Bitmap(20,20));//Add a blank image so the list stays in synch
					}
				}
				item=new ListViewItem(new string[] {ButtonList[i].Description},ButtonList[i].ProcButtonNum.ToString());
				listViewButtons.Items.Add(item);
			}
    }

		private void listViewButtons_DoubleClick(object sender,EventArgs e) {
			if(listViewButtons.SelectedIndices.Count==0) {//Nothing selected
				return;
			}
			ProcButton but=ButtonList[listViewButtons.SelectedIndices[0]].Copy();
			FormProcButtonEdit FormPBE=new FormProcButtonEdit(but);
			FormPBE.ShowDialog();
			changed=true;
			FillButtons();
		}

		private void listCategories_Click(object sender,EventArgs e) {
			if(listCategories.SelectedIndex==-1){
				return;
			}
			SetVisibility();
			if(listCategories.SelectedIndex==0) {
				selectedCat=0;
			}
			else {
				selectedCat=_listProcButtonCatDefs[listCategories.SelectedIndex-1].DefNum;
			}
			FillButtons();
		}

		private void butEdit_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormDefinitions FormD=new FormDefinitions(DefCat.ProcButtonCats);
			FormD.ShowDialog();
			FillCategories();
			FillButtons();
		}

		private void butDown_Click(object sender, System.EventArgs e) {
		int selected=0;
		if(listViewButtons.SelectedIndices.Count==0){
        return;
      }
      else if(listViewButtons.SelectedIndices[0]==listViewButtons.Items.Count-1){
        return; 
      }
      else{
        ProcButton but=ButtonList[listViewButtons.SelectedIndices[0]].Copy();
        but.ItemOrder++;
        ProcButtons.Update(but);
        selected=but.ItemOrder;
        but=ButtonList[listViewButtons.SelectedIndices[0]+1].Copy();
        but.ItemOrder--;
        ProcButtons.Update(but);
      }		
      FillButtons();
			changed=true;
      listViewButtons.SelectedIndices.Clear();
			listViewButtons.SelectedIndices.Add(selected);	 
		}

		private void butUp_Click(object sender, System.EventArgs e) {
      int selected=0;
		  if(listViewButtons.SelectedIndices.Count==0){
        return;
      }
      else if(listViewButtons.SelectedIndices[0]==0){
        return; 
      }
      else{
        ProcButton but=ButtonList[listViewButtons.SelectedIndices[0]].Copy();
        but.ItemOrder--;
        ProcButtons.Update(but);
        selected=but.ItemOrder;
        but=ButtonList[listViewButtons.SelectedIndices[0]-1].Copy();
        but.ItemOrder++;
        ProcButtons.Update(but);
      }	
      FillButtons();	
			changed=true;
			listViewButtons.SelectedIndices.Clear();
			listViewButtons.SelectedIndices.Add(selected);
		}

		private void butAdd_Click(object sender, System.EventArgs e) {
			if(listCategories.SelectedIndex==-1){
				return;
			}
			ProcButton but=new ProcButton();
			but.Category=selectedCat;
			but.ItemOrder=listViewButtons.Items.Count;
      FormProcButtonEdit FormPBE=new FormProcButtonEdit(but);
      FormPBE.IsNew=true;
      FormPBE.ShowDialog();
			changed=true;
      FillButtons();	
		}

		private void butDelete_Click(object sender, System.EventArgs e) {
			if(listViewButtons.SelectedIndices.Count==0){
				MessageBox.Show(Lan.g(this,"Please select an item first."));
				return;
			}
			ProcButtons.Delete(ButtonList[listViewButtons.SelectedIndices[0]]);
			changed=true;
			FillButtons();
		}

		private void butClose_Click(object sender,System.EventArgs e) {
			Close();
		}

		private void FormProcButtons_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if(changed){
				DataValid.SetInvalid(InvalidType.ProcButtons);
			}
		}

		private void panelQuickButtons_RowDoubleClick(object sender,ODButtonPanelEventArgs e) {
			FormProcButtonQuickEdit FormPBQ=new FormProcButtonQuickEdit();
			//Search through tags of the ODPanelItem for the PBQ.
			for(int i=0;e.Item!=null && i<e.Item.Tags.Count;i++) {
				if(e.Item.Tags[i].GetType()==typeof(ProcButtonQuick)){
					FormPBQ.pbqCur=(ProcButtonQuick)e.Item.Tags[i];
					break;
				}
			}
			if(FormPBQ.pbqCur==null) {//clicked on either a blank row or to the right of existing buttons on a row.
				FormPBQ.IsNew=true;
				FormPBQ.pbqCur=new ProcButtonQuick();
				FormPBQ.pbqCur.YPos=e.Row;//Set Row
				for(int i=0;i<listProcButtonQuicks.Count;i++){ //Set ItemOrder
					if(listProcButtonQuicks[i].YPos!=FormPBQ.pbqCur.YPos //Wrong row
						|| FormPBQ.pbqCur.ItemOrder>listProcButtonQuicks[i].ItemOrder) { //Already have a larger item order
							continue;
					}
					FormPBQ.pbqCur.ItemOrder=listProcButtonQuicks[i].ItemOrder+1;//new PBQ should have the highest item order in the row.
				}
			}
			FormPBQ.ShowDialog();
			if(FormPBQ.DialogResult!=DialogResult.OK) {
				return;
			}
			fillPanelQuickButtons();
		}

		

		

		
		

	}
}
