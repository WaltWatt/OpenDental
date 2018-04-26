/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;

namespace OpenDental{
///<summary></summary>
	public class FormInsPlans:ODForm {
		private System.ComponentModel.Container components = null;// Required designer variable.
		//private InsTemplates InsTemplates;
		private OpenDental.UI.Button butBlank;
		private OpenDental.UI.Button butCancel;
		private OpenDental.UI.Button butOK;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton radioOrderCarrier;
		private System.Windows.Forms.RadioButton radioOrderEmp;
		//<summary>Set to true if we are only using the list to select a template to link to rather than creating a new plan. If this is true, then IsSelectMode will be ignored.</summary>
		//public bool IsLinkMode;
		///<summary>Set to true when selecting a plan for a patient and we want SelectedPlan to be filled upon closing.</summary>
		public bool IsSelectMode;
		///<summary>After closing this form, if IsSelectMode, then this will contain the selected Plan.</summary>
		public InsPlan SelectedPlan;
		private Label label1;
		private TextBox textEmployer;
		private TextBox textCarrier;
		private Label label2;
		private OpenDental.UI.ODGrid gridMain;
		//private InsPlan[] ListAll;
		///<summary>Supply a string here to start off the search with filtered employers.</summary>
		public string empText;
		private TextBox textGroupNum;
		private Label label3;
		private TextBox textGroupName;
		private Label label4;
		private OpenDental.UI.Button butMerge;
		///<summary>Supply a string here to start off the search with filtered carriers.</summary>
		public string carrierText;
		private TextBox textTrojanID;
		private Label labelTrojanID;
		private DataTable table;
		private CheckBox checkShowHidden;
		private OpenDental.UI.Button butHide;
		private bool trojan;
		///<summary>Supply a string here to start off the search with filtered group names.</summary>
		public string groupNameText;
		private UI.Button butGetAll;
		private TextBox textPlanNum;
		private Label labelPlanNum;
		private GroupBox groupBox1;

		///<summary>Supply a string here to start off the search with filtered group nums.</summary>
		public string groupNumText;

		///<summary></summary>
		public FormInsPlans(){
			InitializeComponent();// Required for Windows Form Designer support
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInsPlans));
			this.textPlanNum = new System.Windows.Forms.TextBox();
			this.labelPlanNum = new System.Windows.Forms.Label();
			this.butGetAll = new OpenDental.UI.Button();
			this.butHide = new OpenDental.UI.Button();
			this.checkShowHidden = new System.Windows.Forms.CheckBox();
			this.textTrojanID = new System.Windows.Forms.TextBox();
			this.labelTrojanID = new System.Windows.Forms.Label();
			this.butMerge = new OpenDental.UI.Button();
			this.textGroupNum = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.textGroupName = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.textCarrier = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textEmployer = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.radioOrderCarrier = new System.Windows.Forms.RadioButton();
			this.radioOrderEmp = new System.Windows.Forms.RadioButton();
			this.butOK = new OpenDental.UI.Button();
			this.butBlank = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// textPlanNum
			// 
			this.textPlanNum.Location = new System.Drawing.Point(540, 36);
			this.textPlanNum.Name = "textPlanNum";
			this.textPlanNum.Size = new System.Drawing.Size(140, 20);
			this.textPlanNum.TabIndex = 31;
			this.textPlanNum.TextChanged += new System.EventHandler(this.textPlanNum_TextChanged);
			// 
			// labelPlanNum
			// 
			this.labelPlanNum.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelPlanNum.Location = new System.Drawing.Point(457, 38);
			this.labelPlanNum.Name = "labelPlanNum";
			this.labelPlanNum.Size = new System.Drawing.Size(81, 17);
			this.labelPlanNum.TabIndex = 32;
			this.labelPlanNum.Text = "Plan Num";
			this.labelPlanNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butGetAll
			// 
			this.butGetAll.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butGetAll.Autosize = true;
			this.butGetAll.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butGetAll.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butGetAll.CornerRadius = 4F;
			this.butGetAll.Location = new System.Drawing.Point(854, 12);
			this.butGetAll.Name = "butGetAll";
			this.butGetAll.Size = new System.Drawing.Size(75, 24);
			this.butGetAll.TabIndex = 30;
			this.butGetAll.Text = "Get All";
			this.butGetAll.Click += new System.EventHandler(this.butGetAll_Click);
			// 
			// butHide
			// 
			this.butHide.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butHide.Autosize = true;
			this.butHide.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butHide.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butHide.CornerRadius = 4F;
			this.butHide.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butHide.Location = new System.Drawing.Point(104, 664);
			this.butHide.Name = "butHide";
			this.butHide.Size = new System.Drawing.Size(84, 24);
			this.butHide.TabIndex = 28;
			this.butHide.Text = "Hide Unused";
			this.butHide.Click += new System.EventHandler(this.butHide_Click);
			// 
			// checkShowHidden
			// 
			this.checkShowHidden.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowHidden.Location = new System.Drawing.Point(837, 37);
			this.checkShowHidden.Name = "checkShowHidden";
			this.checkShowHidden.Size = new System.Drawing.Size(93, 20);
			this.checkShowHidden.TabIndex = 27;
			this.checkShowHidden.Text = "Show Hidden";
			this.checkShowHidden.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.checkShowHidden.UseVisualStyleBackColor = true;
			this.checkShowHidden.CheckedChanged += new System.EventHandler(this.checkShowHidden_CheckedChanged);
			// 
			// textTrojanID
			// 
			this.textTrojanID.Location = new System.Drawing.Point(540, 15);
			this.textTrojanID.Name = "textTrojanID";
			this.textTrojanID.Size = new System.Drawing.Size(140, 20);
			this.textTrojanID.TabIndex = 25;
			this.textTrojanID.TextChanged += new System.EventHandler(this.textTrojanID_TextChanged);
			// 
			// labelTrojanID
			// 
			this.labelTrojanID.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelTrojanID.Location = new System.Drawing.Point(457, 18);
			this.labelTrojanID.Name = "labelTrojanID";
			this.labelTrojanID.Size = new System.Drawing.Size(81, 17);
			this.labelTrojanID.TabIndex = 26;
			this.labelTrojanID.Text = "Trojan ID";
			this.labelTrojanID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butMerge
			// 
			this.butMerge.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butMerge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butMerge.Autosize = true;
			this.butMerge.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butMerge.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butMerge.CornerRadius = 4F;
			this.butMerge.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butMerge.Location = new System.Drawing.Point(11, 664);
			this.butMerge.Name = "butMerge";
			this.butMerge.Size = new System.Drawing.Size(74, 24);
			this.butMerge.TabIndex = 24;
			this.butMerge.Text = "Combine";
			this.butMerge.Click += new System.EventHandler(this.butMerge_Click);
			// 
			// textGroupNum
			// 
			this.textGroupNum.Location = new System.Drawing.Point(84, 35);
			this.textGroupNum.Name = "textGroupNum";
			this.textGroupNum.Size = new System.Drawing.Size(140, 20);
			this.textGroupNum.TabIndex = 20;
			this.textGroupNum.TextChanged += new System.EventHandler(this.textGroupNum_TextChanged);
			// 
			// label3
			// 
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label3.Location = new System.Drawing.Point(6, 37);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(76, 17);
			this.label3.TabIndex = 23;
			this.label3.Text = "Group Num";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textGroupName
			// 
			this.textGroupName.Location = new System.Drawing.Point(311, 36);
			this.textGroupName.Name = "textGroupName";
			this.textGroupName.Size = new System.Drawing.Size(140, 20);
			this.textGroupName.TabIndex = 21;
			this.textGroupName.TextChanged += new System.EventHandler(this.textGroupName_TextChanged);
			// 
			// label4
			// 
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label4.Location = new System.Drawing.Point(231, 36);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(78, 17);
			this.label4.TabIndex = 22;
			this.label4.Text = "Group Name";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HasAddButton = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = true;
			this.gridMain.Location = new System.Drawing.Point(11, 79);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(961, 578);
			this.gridMain.TabIndex = 19;
			this.gridMain.Title = "Insurance Plans";
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableInsurancePlans";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// textCarrier
			// 
			this.textCarrier.Location = new System.Drawing.Point(311, 15);
			this.textCarrier.Name = "textCarrier";
			this.textCarrier.Size = new System.Drawing.Size(140, 20);
			this.textCarrier.TabIndex = 0;
			this.textCarrier.TextChanged += new System.EventHandler(this.textCarrier_TextChanged);
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(230, 17);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(79, 17);
			this.label2.TabIndex = 17;
			this.label2.Text = "Carrier";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textEmployer
			// 
			this.textEmployer.Location = new System.Drawing.Point(84, 14);
			this.textEmployer.Name = "textEmployer";
			this.textEmployer.Size = new System.Drawing.Size(140, 20);
			this.textEmployer.TabIndex = 1;
			this.textEmployer.TextChanged += new System.EventHandler(this.textEmployer_TextChanged);
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 17);
			this.label1.TabIndex = 15;
			this.label1.Text = "Employer";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.radioOrderCarrier);
			this.groupBox2.Controls.Add(this.radioOrderEmp);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(688, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(132, 40);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Order By";
			// 
			// radioOrderCarrier
			// 
			this.radioOrderCarrier.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioOrderCarrier.Location = new System.Drawing.Point(75, 13);
			this.radioOrderCarrier.Name = "radioOrderCarrier";
			this.radioOrderCarrier.Size = new System.Drawing.Size(48, 16);
			this.radioOrderCarrier.TabIndex = 1;
			this.radioOrderCarrier.Text = "Carrier";
			this.radioOrderCarrier.Click += new System.EventHandler(this.radioOrderCarrier_Click);
			// 
			// radioOrderEmp
			// 
			this.radioOrderEmp.Checked = true;
			this.radioOrderEmp.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.radioOrderEmp.Location = new System.Drawing.Point(9, 13);
			this.radioOrderEmp.Name = "radioOrderEmp";
			this.radioOrderEmp.Size = new System.Drawing.Size(69, 16);
			this.radioOrderEmp.TabIndex = 0;
			this.radioOrderEmp.TabStop = true;
			this.radioOrderEmp.Text = "Employer";
			this.radioOrderEmp.Click += new System.EventHandler(this.radioOrderEmp_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(799, 664);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(78, 24);
			this.butOK.TabIndex = 4;
			this.butOK.Text = "OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butBlank
			// 
			this.butBlank.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butBlank.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butBlank.Autosize = true;
			this.butBlank.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butBlank.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butBlank.CornerRadius = 4F;
			this.butBlank.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butBlank.Location = new System.Drawing.Point(427, 664);
			this.butBlank.Name = "butBlank";
			this.butBlank.Size = new System.Drawing.Size(87, 24);
			this.butBlank.TabIndex = 3;
			this.butBlank.Text = "Blank Plan";
			this.butBlank.Visible = false;
			this.butBlank.Click += new System.EventHandler(this.butBlank_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.butCancel.Location = new System.Drawing.Point(894, 664);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(78, 24);
			this.butCancel.TabIndex = 5;
			this.butCancel.Text = "Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.textEmployer);
			this.groupBox1.Controls.Add(this.butGetAll);
			this.groupBox1.Controls.Add(this.textPlanNum);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.checkShowHidden);
			this.groupBox1.Controls.Add(this.labelPlanNum);
			this.groupBox1.Controls.Add(this.textCarrier);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.groupBox2);
			this.groupBox1.Controls.Add(this.textGroupName);
			this.groupBox1.Controls.Add(this.textGroupNum);
			this.groupBox1.Controls.Add(this.textTrojanID);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.labelTrojanID);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(960, 61);
			this.groupBox1.TabIndex = 33;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Filter";
			// 
			// FormInsPlans
			// 
			this.ClientSize = new System.Drawing.Size(987, 696);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butHide);
			this.Controls.Add(this.butMerge);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butBlank);
			this.Controls.Add(this.butCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormInsPlans";
			this.ShowInTaskbar = false;
			this.Text = "Insurance Plans";
			this.Load += new System.EventHandler(this.FormInsTemplates_Load);
			this.groupBox2.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormInsTemplates_Load(object sender, System.EventArgs e) {
			if(!IsSelectMode){
				butCancel.Text=Lan.g(this,"Close");
				butOK.Visible=false;
			}
			Program prog=Programs.GetCur(ProgramName.Trojan);
			if(prog!=null && prog.Enabled) {
				trojan=true;
			}
			else{
				labelTrojanID.Visible=false;
				textTrojanID.Visible=false;
			}
			textEmployer.Text=empText;
			textCarrier.Text=carrierText;
			textGroupName.Text=groupNameText;
			textGroupNum.Text=groupNumText;
			FillGrid();
		}

		private void FillGrid(bool isGetAll=false){
			Cursor=Cursors.WaitCursor;
			table=InsPlans.GetBigList(radioOrderEmp.Checked,textEmployer.Text,textCarrier.Text,
				textGroupName.Text,textGroupNum.Text,textPlanNum.Text,textTrojanID.Text,checkShowHidden.Checked,isGetAll);
			if(IsSelectMode){
				butBlank.Visible=true;
			}
			int selectedRow;//preserves the selected row.
			if(gridMain.SelectedIndices.Length==1){
				selectedRow=gridMain.SelectedIndices[0];
			}
			else{
				selectedRow=-1;
			}
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lans.g("TableInsurancePlans","Employer"),140);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g("TableInsurancePlans","Carrier"),140);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g("TableInsurancePlans","Phone"),82);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g("TableInsurancePlans","Address"),120);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g("TableInsurancePlans","City"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g("TableInsurancePlans","ST"),25);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g("TableInsurancePlans","Zip"),50);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g("TableInsurancePlans","Group#"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g("TableInsurancePlans","Group Name"),90);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g("TableInsurancePlans","noE"),35);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g("TableInsurancePlans","ElectID"),45);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lans.g("TableInsurancePlans","Subs"),40);
			gridMain.Columns.Add(col);
			if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
				col=new ODGridColumn(Lan.g("TableCarriers","CDAnet"),50);
				gridMain.Columns.Add(col);
			}
			if(trojan){
				col=new ODGridColumn(Lans.g("TableInsurancePlans","TrojanID"),60);
				gridMain.Columns.Add(col);
			}
			//PlanNote not shown
			gridMain.Rows.Clear();
			ODGridRow row;
			//Carrier carrier;
			for(int i=0;i<table.Rows.Count;i++) {
				row=new ODGridRow();
				row.Cells.Add(table.Rows[i]["EmpName"].ToString());
				row.Cells.Add(table.Rows[i]["CarrierName"].ToString());
				row.Cells.Add(table.Rows[i]["Phone"].ToString());
				row.Cells.Add(table.Rows[i]["Address"].ToString());
				row.Cells.Add(table.Rows[i]["City"].ToString());
				row.Cells.Add(table.Rows[i]["State"].ToString());
				row.Cells.Add(table.Rows[i]["Zip"].ToString());
				row.Cells.Add(table.Rows[i]["GroupNum"].ToString());
				row.Cells.Add(table.Rows[i]["GroupName"].ToString());
				row.Cells.Add(table.Rows[i]["noSendElect"].ToString());
				row.Cells.Add(table.Rows[i]["ElectID"].ToString());
				row.Cells.Add(table.Rows[i]["subscribers"].ToString());
				if(CultureInfo.CurrentCulture.Name.EndsWith("CA")) {//Canadian. en-CA or fr-CA
					row.Cells.Add((table.Rows[i]["IsCDA"].ToString()=="0")?"":"X");
				}
				if(trojan){
					row.Cells.Add(table.Rows[i]["TrojanID"].ToString());
				}
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			gridMain.SetSelected(selectedRow,true);
			Cursor=Cursors.Default;
		}

		private void gridMain_CellDoubleClick(object sender,ODGridClickEventArgs e){
			InsPlan plan=InsPlans.GetPlan(PIn.Long(table.Rows[e.Row]["PlanNum"].ToString()),null);
			if(plan==null || plan.PlanNum==0) {
				MsgBox.Show(this,"Insurance plan selected no longer exists.");
				FillGrid();
				return;
			}
			if(IsSelectMode) {
				SelectedPlan=plan.Copy();
				DialogResult=DialogResult.OK;
				return;
			}
			FormInsPlan FormIP=new FormInsPlan(plan,null,null);
			FormIP.ShowDialog();
			if(FormIP.DialogResult!=DialogResult.OK) {
				return;
			}
			FillGrid();
		}

		private void radioOrderEmp_Click(object sender, System.EventArgs e) {
			FillGrid();
		}

		private void radioOrderCarrier_Click(object sender, System.EventArgs e) {
			FillGrid();
		}

		private void textEmployer_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void textCarrier_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void textGroupName_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void textGroupNum_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void textPlanNum_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void textTrojanID_TextChanged(object sender,EventArgs e) {
			FillGrid();
		}

		private void checkShowHidden_CheckedChanged(object sender,EventArgs e) {
			FillGrid();
		}
		
		private void butGetAll_Click(object sender,EventArgs e) {
			FillGrid(true);
		}

		private void butMerge_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.InsPlanMerge)) {
				return;
			}
			if(gridMain.SelectedIndices.Length<2) {
				MessageBox.Show(Lan.g(this,"Please select at least two items first."));
				return;
			}
			InsPlan[] listSelected=new InsPlan[gridMain.SelectedIndices.Length];
			for(int i=0;i<listSelected.Length;i++){
				listSelected[i]=InsPlans.GetPlan(PIn.Long(table.Rows[gridMain.SelectedIndices[i]]["PlanNum"].ToString()),null);
				listSelected[i].NumberSubscribers=PIn.Int(table.Rows[gridMain.SelectedIndices[i]]["subscribers"].ToString());
			}
			FormInsPlansMerge FormI=new FormInsPlansMerge();
			FormI.ListAll=listSelected;
			FormI.ShowDialog();
			if(FormI.DialogResult!=DialogResult.OK){
				return;
			}
			//Do the merge.
			InsPlan planToMergeTo=FormI.PlanToMergeTo.Copy();
			//List<Benefit> benList=Benefits.RefreshForPlan(planToMergeTo,0);
			Cursor=Cursors.WaitCursor;
			bool didMerge=false;
			List<long> listMergedPlanNums=new List<long>();
			for(int i=0;i<listSelected.Length;i++){//loop through each selected plan
				//skip the planToMergeTo, because it's already correct
				if(planToMergeTo.PlanNum==listSelected[i].PlanNum){
					continue;
				}
				//==Michael - We are changing plans here, but not carriers, so this is not needed:
				//SecurityLogs.MakeLogEntry(Permissions.InsPlanChangeCarrierName
				InsPlans.ChangeReferences(listSelected[i].PlanNum,planToMergeTo.PlanNum);
				Benefits.DeleteForPlan(listSelected[i].PlanNum);
				try {
					InsPlans.Delete(listSelected[i],canDeleteInsSub:false);
				}
				catch (ApplicationException ex){
					MessageBox.Show(ex.Message);
					SecurityLogs.MakeLogEntry(Permissions.InsPlanEdit,0,
						Lan.g(this,"InsPlan Combine delete validation failed.  Plan was not deleted."),
						listSelected[i].PlanNum,listSelected[i].SecDateTEdit); //new plan, no date needed.
					//Since we already deleted/changed all of the other dependencies, 
					//we should continue in making the Securitylog entry and cleaning up. 
				}
				didMerge=true;
				listMergedPlanNums.Add(listSelected[i].PlanNum);
				//for(int j=0;j<planNums.Count;j++) {
					//InsPlans.ComputeEstimatesForPlan(planNums[j]);
					//Eliminated in 5.0 for speed.
				//}
			}
			if(didMerge) {
				string logText=Lan.g(this,"Merged the following PlanNum(s): ")+string.Join(", ",listMergedPlanNums)+" "+Lan.g(this,"into")+" "+planToMergeTo.PlanNum;
				SecurityLogs.MakeLogEntry(Permissions.InsPlanMerge,0,logText);
			}
			FillGrid();
			//highlight the merged plan
			for(int i=0;i<table.Rows.Count;i++){
				for(int j=0;j<listSelected.Length;j++){
					if(table.Rows[i]["PlanNum"].ToString()==listSelected[j].PlanNum.ToString()){
						gridMain.SetSelected(i,true);
					}
				}
			}
			Cursor=Cursors.Default;
		}

		private void butBlank_Click(object sender, System.EventArgs e) {
			//this button is normally not visible.  It's only set visible when IsSelectMode.
			SelectedPlan=new InsPlan();
			DialogResult=DialogResult.OK;
		}

		private void butHide_Click(object sender,EventArgs e) {
			int unusedCount=InsPlans.UnusedGetCount();
			if(unusedCount==0) {
				MsgBox.Show(this,"All plans are in use.");
				return;
			}
			string msgText=unusedCount.ToString()+" "+Lan.g(this,"plans found that are not in use by any subscribers.  Hide all of them?");
			if(MessageBox.Show(msgText,"",MessageBoxButtons.YesNo)!=DialogResult.Yes){
				return;
			}
			InsPlans.UnusedHideAll();
			FillGrid();
			MsgBox.Show(this,"Done.");
		}

		private void butOK_Click(object sender, System.EventArgs e) {
			//only visible if IsSelectMode
			if(gridMain.SelectedIndices.Length==0){
				MessageBox.Show(Lan.g(this,"Please select an item first."));
				return;
			}
			if(gridMain.SelectedIndices.Length>1) {
				MessageBox.Show(Lan.g(this,"Please select only one item first."));
				return;
			}
			SelectedPlan=InsPlans.GetPlan(PIn.Long(table.Rows[gridMain.SelectedIndices[0]]["PlanNum"].ToString()),null).Copy();
			DialogResult=DialogResult.OK;
		}

		private void butCancel_Click(object sender, System.EventArgs e) {
			DialogResult=DialogResult.Cancel;
		}
	}
}


















