/*=============================================================================================================
Open Dental GPL license Copyright (C) 2003  Jordan Sparks, DMD.  http://www.open-dent.com,  www.docsparks.com
See header in FormOpenDental.cs for complete text.  Redistributions must retain this text.
===============================================================================================================*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using OpenDental.UI;
using OpenDentBusiness;
using CodeBase;

namespace OpenDental{
///<summary></summary>
	public class FormConfirmList : ODForm {
		private OpenDental.UI.Button butClose;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private IContainer components;
		///<summary>Will be set to true when form closes if user click Send to Pinboard.</summary>
		public bool PinClicked=false;
		private OpenDental.UI.Button butReport;
		private int pagesPrinted;
		private DataTable AddrTable;
		private int patientsPrinted;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.ComboBox comboStatus;
		private OpenDental.UI.Button butLabels;
		private OpenDental.UI.Button butPostcards;
		private OpenDental.ValidDate textDateFrom;
		private OpenDental.ValidDate textDateTo;
		private OpenDental.UI.ODGrid gridMain;
		///<summary>This list of appointments displayed</summary>
		private DataTable Table;
		private PrintDocument pd;
		private OpenDental.UI.Button butPrint;
		private OpenDental.UI.FormPrintPreview printPreview;
		private bool headingPrinted;
		private int headingPrintH;
		private ComboBox comboProv;
		private Label label4;
		private OpenDental.UI.Button butEmail;
		private ComboBox comboClinic;
		private Label labelClinic;
		private ComboBox comboShowRecall;
		private UI.Button butText;
		private List<Clinic> _listUserClinics;
		private ContextMenuStrip menuRightClick;
		private ComboBox comboEmailFrom;
		private GroupBox groupBox2;
		private ToolStripMenuItem toolStripMenuItemSelectPatient;
		private ToolStripMenuItem toolStripMenuItemSeeChart;
		private ToolStripMenuItem toolStripMenuItemSendToPinboard;
		private List<EmailAddress> _listEmailAddresses;
		private List<Provider> _listProviders;
		private Label labelConfirmList;
		private Label labelShowStatus;
		private ComboBox comboViewStatus;
		private Label label1;
		private UI.Button butRefresh;
		private List<Def> _listApptConfirmedDefs;

		///<summary></summary>
		public FormConfirmList(){
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

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfirmList));
			this.butClose = new OpenDental.UI.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.labelConfirmList = new System.Windows.Forms.Label();
			this.labelShowStatus = new System.Windows.Forms.Label();
			this.comboViewStatus = new System.Windows.Forms.ComboBox();
			this.comboShowRecall = new System.Windows.Forms.ComboBox();
			this.comboClinic = new System.Windows.Forms.ComboBox();
			this.labelClinic = new System.Windows.Forms.Label();
			this.comboProv = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textDateTo = new OpenDental.ValidDate();
			this.textDateFrom = new OpenDental.ValidDate();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.butReport = new OpenDental.UI.Button();
			this.butLabels = new OpenDental.UI.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.comboStatus = new System.Windows.Forms.ComboBox();
			this.butPostcards = new OpenDental.UI.Button();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.menuRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuItemSelectPatient = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemSeeChart = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItemSendToPinboard = new System.Windows.Forms.ToolStripMenuItem();
			this.butPrint = new OpenDental.UI.Button();
			this.butEmail = new OpenDental.UI.Button();
			this.butText = new OpenDental.UI.Button();
			this.comboEmailFrom = new System.Windows.Forms.ComboBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.butRefresh = new OpenDental.UI.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.menuRightClick.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
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
			this.butClose.Location = new System.Drawing.Point(972, 659);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 24);
			this.butClose.TabIndex = 2;
			this.butClose.Text = "&Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.labelConfirmList);
			this.groupBox1.Controls.Add(this.labelShowStatus);
			this.groupBox1.Controls.Add(this.comboViewStatus);
			this.groupBox1.Controls.Add(this.comboShowRecall);
			this.groupBox1.Controls.Add(this.comboClinic);
			this.groupBox1.Controls.Add(this.labelClinic);
			this.groupBox1.Controls.Add(this.comboProv);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.textDateTo);
			this.groupBox1.Controls.Add(this.textDateFrom);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(5, 4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(684, 63);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "View";
			// 
			// labelConfirmList
			// 
			this.labelConfirmList.Location = new System.Drawing.Point(6, 38);
			this.labelConfirmList.Name = "labelConfirmList";
			this.labelConfirmList.Size = new System.Drawing.Size(81, 14);
			this.labelConfirmList.TabIndex = 29;
			this.labelConfirmList.Text = "Confirm List";
			this.labelConfirmList.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelShowStatus
			// 
			this.labelShowStatus.Location = new System.Drawing.Point(6, 16);
			this.labelShowStatus.Name = "labelShowStatus";
			this.labelShowStatus.Size = new System.Drawing.Size(81, 14);
			this.labelShowStatus.TabIndex = 28;
			this.labelShowStatus.Text = "Status";
			this.labelShowStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboViewStatus
			// 
			this.comboViewStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboViewStatus.FormattingEnabled = true;
			this.comboViewStatus.Items.AddRange(new object[] {
            "All",
            "Recall Only",
            "Exclude Recall",
            "Hygiene Prescheduled"});
			this.comboViewStatus.Location = new System.Drawing.Point(93, 12);
			this.comboViewStatus.Name = "comboViewStatus";
			this.comboViewStatus.Size = new System.Drawing.Size(121, 21);
			this.comboViewStatus.TabIndex = 27;
			// 
			// comboShowRecall
			// 
			this.comboShowRecall.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboShowRecall.FormattingEnabled = true;
			this.comboShowRecall.Items.AddRange(new object[] {
            "All",
            "Recall Only",
            "Exclude Recall",
            "Hygiene Prescheduled"});
			this.comboShowRecall.Location = new System.Drawing.Point(93, 36);
			this.comboShowRecall.Name = "comboShowRecall";
			this.comboShowRecall.Size = new System.Drawing.Size(121, 21);
			this.comboShowRecall.TabIndex = 26;
			// 
			// comboClinic
			// 
			this.comboClinic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboClinic.Location = new System.Drawing.Point(497, 36);
			this.comboClinic.MaxDropDownItems = 40;
			this.comboClinic.Name = "comboClinic";
			this.comboClinic.Size = new System.Drawing.Size(181, 21);
			this.comboClinic.TabIndex = 25;
			// 
			// labelClinic
			// 
			this.labelClinic.Location = new System.Drawing.Point(404, 40);
			this.labelClinic.Name = "labelClinic";
			this.labelClinic.Size = new System.Drawing.Size(91, 14);
			this.labelClinic.TabIndex = 24;
			this.labelClinic.Text = "Clinic";
			this.labelClinic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// comboProv
			// 
			this.comboProv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboProv.Location = new System.Drawing.Point(497, 12);
			this.comboProv.MaxDropDownItems = 40;
			this.comboProv.Name = "comboProv";
			this.comboProv.Size = new System.Drawing.Size(181, 21);
			this.comboProv.TabIndex = 23;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(404, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(91, 14);
			this.label4.TabIndex = 22;
			this.label4.Text = "Provider";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// textDateTo
			// 
			this.textDateTo.Location = new System.Drawing.Point(304, 38);
			this.textDateTo.Name = "textDateTo";
			this.textDateTo.Size = new System.Drawing.Size(94, 20);
			this.textDateTo.TabIndex = 14;
			// 
			// textDateFrom
			// 
			this.textDateFrom.Location = new System.Drawing.Point(304, 16);
			this.textDateFrom.Name = "textDateFrom";
			this.textDateFrom.Size = new System.Drawing.Size(94, 20);
			this.textDateFrom.TabIndex = 13;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(220, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(81, 14);
			this.label2.TabIndex = 12;
			this.label2.Text = "To Date";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(220, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(81, 14);
			this.label1.TabIndex = 11;
			this.label1.Text = "From Date";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// butReport
			// 
			this.butReport.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butReport.Autosize = true;
			this.butReport.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butReport.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butReport.CornerRadius = 4F;
			this.butReport.Location = new System.Drawing.Point(698, 659);
			this.butReport.Name = "butReport";
			this.butReport.Size = new System.Drawing.Size(87, 24);
			this.butReport.TabIndex = 13;
			this.butReport.Text = "R&un Report";
			this.butReport.Click += new System.EventHandler(this.butReport_Click);
			// 
			// butLabels
			// 
			this.butLabels.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butLabels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butLabels.Autosize = true;
			this.butLabels.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLabels.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLabels.CornerRadius = 4F;
			this.butLabels.Image = global::OpenDental.Properties.Resources.butLabel;
			this.butLabels.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butLabels.Location = new System.Drawing.Point(129, 659);
			this.butLabels.Name = "butLabels";
			this.butLabels.Size = new System.Drawing.Size(102, 24);
			this.butLabels.TabIndex = 14;
			this.butLabels.Text = "Label Preview";
			this.butLabels.Click += new System.EventHandler(this.butLabels_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.comboStatus);
			this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox3.Location = new System.Drawing.Point(695, 4);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(143, 63);
			this.groupBox3.TabIndex = 15;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Set Status";
			// 
			// comboStatus
			// 
			this.comboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboStatus.Location = new System.Drawing.Point(7, 22);
			this.comboStatus.MaxDropDownItems = 40;
			this.comboStatus.Name = "comboStatus";
			this.comboStatus.Size = new System.Drawing.Size(128, 21);
			this.comboStatus.TabIndex = 15;
			this.comboStatus.SelectedIndexChanged += new System.EventHandler(this.comboStatus_SelectedIndexChanged);
			this.comboStatus.SelectionChangeCommitted += new System.EventHandler(this.comboStatus_SelectionChangeCommitted);
			// 
			// butPostcards
			// 
			this.butPostcards.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPostcards.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.butPostcards.Autosize = true;
			this.butPostcards.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPostcards.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPostcards.CornerRadius = 4F;
			this.butPostcards.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPostcards.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPostcards.Location = new System.Drawing.Point(4, 659);
			this.butPostcards.Name = "butPostcards";
			this.butPostcards.Size = new System.Drawing.Size(119, 24);
			this.butPostcards.TabIndex = 16;
			this.butPostcards.Text = "Postcard Preview";
			this.butPostcards.Click += new System.EventHandler(this.butPostcards_Click);
			// 
			// gridMain
			// 
			this.gridMain.CellFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
			this.gridMain.ContextMenuStrip = this.menuRightClick;
			this.gridMain.HasAddButton = false;
			this.gridMain.HasDropDowns = false;
			this.gridMain.HasMultilineHeaders = false;
			this.gridMain.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
			this.gridMain.HeaderHeight = 15;
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(4, 69);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.SelectionMode = OpenDental.UI.GridSelectionMode.MultiExtended;
			this.gridMain.Size = new System.Drawing.Size(1043, 585);
			this.gridMain.TabIndex = 0;
			this.gridMain.Title = "Confirmation List";
			this.gridMain.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
			this.gridMain.TitleHeight = 18;
			this.gridMain.TranslationName = "TableConfirmList";
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellDoubleClick);
			this.gridMain.CellClick += new OpenDental.UI.ODGridClickEventHandler(this.grid_CellClick);
			this.gridMain.Click += new System.EventHandler(this.grid_Click);
			this.gridMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridMain_MouseUp);
			// 
			// menuRightClick
			// 
			this.menuRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSelectPatient,
            this.toolStripMenuItemSeeChart,
            this.toolStripMenuItemSendToPinboard});
			this.menuRightClick.Name = "_menuRightClick";
			this.menuRightClick.Size = new System.Drawing.Size(166, 70);
			// 
			// toolStripMenuItemSelectPatient
			// 
			this.toolStripMenuItemSelectPatient.Name = "toolStripMenuItemSelectPatient";
			this.toolStripMenuItemSelectPatient.Size = new System.Drawing.Size(165, 22);
			this.toolStripMenuItemSelectPatient.Text = "Select Patient";
			this.toolStripMenuItemSelectPatient.Click += new System.EventHandler(this.menuRight_click);
			// 
			// toolStripMenuItemSeeChart
			// 
			this.toolStripMenuItemSeeChart.Name = "toolStripMenuItemSeeChart";
			this.toolStripMenuItemSeeChart.Size = new System.Drawing.Size(165, 22);
			this.toolStripMenuItemSeeChart.Text = "See Chart";
			this.toolStripMenuItemSeeChart.Click += new System.EventHandler(this.menuRight_click);
			// 
			// toolStripMenuItemSendToPinboard
			// 
			this.toolStripMenuItemSendToPinboard.Name = "toolStripMenuItemSendToPinboard";
			this.toolStripMenuItemSendToPinboard.Size = new System.Drawing.Size(165, 22);
			this.toolStripMenuItemSendToPinboard.Text = "Send to Pinboard";
			this.toolStripMenuItemSendToPinboard.Click += new System.EventHandler(this.menuRight_click);
			// 
			// butPrint
			// 
			this.butPrint.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butPrint.Autosize = true;
			this.butPrint.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPrint.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPrint.CornerRadius = 4F;
			this.butPrint.Image = global::OpenDental.Properties.Resources.butPrintSmall;
			this.butPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butPrint.Location = new System.Drawing.Point(791, 659);
			this.butPrint.Name = "butPrint";
			this.butPrint.Size = new System.Drawing.Size(87, 24);
			this.butPrint.TabIndex = 20;
			this.butPrint.Text = "Print List";
			this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
			// 
			// butEmail
			// 
			this.butEmail.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butEmail.Autosize = true;
			this.butEmail.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butEmail.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butEmail.CornerRadius = 4F;
			this.butEmail.Image = global::OpenDental.Properties.Resources.email1;
			this.butEmail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butEmail.Location = new System.Drawing.Point(237, 659);
			this.butEmail.Name = "butEmail";
			this.butEmail.Size = new System.Drawing.Size(91, 24);
			this.butEmail.TabIndex = 61;
			this.butEmail.Text = "E-Mail";
			this.butEmail.Click += new System.EventHandler(this.butEmail_Click);
			// 
			// butText
			// 
			this.butText.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butText.Autosize = false;
			this.butText.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butText.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butText.CornerRadius = 4F;
			this.butText.Image = global::OpenDental.Properties.Resources.Text;
			this.butText.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.butText.Location = new System.Drawing.Point(333, 659);
			this.butText.Name = "butText";
			this.butText.Size = new System.Drawing.Size(79, 24);
			this.butText.TabIndex = 61;
			this.butText.Text = "Text";
			this.butText.Click += new System.EventHandler(this.butText_Click);
			// 
			// comboEmailFrom
			// 
			this.comboEmailFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboEmailFrom.Location = new System.Drawing.Point(6, 22);
			this.comboEmailFrom.MaxDropDownItems = 40;
			this.comboEmailFrom.Name = "comboEmailFrom";
			this.comboEmailFrom.Size = new System.Drawing.Size(191, 21);
			this.comboEmailFrom.TabIndex = 63;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.comboEmailFrom);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(844, 4);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(203, 63);
			this.groupBox2.TabIndex = 16;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Email From";
			// 
			// butRefresh
			// 
			this.butRefresh.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butRefresh.Autosize = true;
			this.butRefresh.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butRefresh.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butRefresh.CornerRadius = 4F;
			this.butRefresh.Location = new System.Drawing.Point(617, 659);
			this.butRefresh.Name = "butRefresh";
			this.butRefresh.Size = new System.Drawing.Size(75, 24);
			this.butRefresh.TabIndex = 62;
			this.butRefresh.Text = "&Refresh";
			this.butRefresh.Click += new System.EventHandler(this.butRefresh_Click);
			// 
			// FormConfirmList
			// 
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(1052, 688);
			this.Controls.Add(this.butRefresh);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.butText);
			this.Controls.Add(this.butEmail);
			this.Controls.Add(this.butPrint);
			this.Controls.Add(this.butPostcards);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.butLabels);
			this.Controls.Add(this.butReport);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.butClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "FormConfirmList";
			this.Text = "Confirmation List";
			this.Load += new System.EventHandler(this.FormConfirmList_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.menuRightClick.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void FormConfirmList_Load(object sender, System.EventArgs e) {
			Cursor=Cursors.WaitCursor;
			comboShowRecall.SelectedIndex=0;//Default to show all.
			textDateFrom.Text=AddWorkDays(1,DateTime.Today).ToShortDateString();
			textDateTo.Text=AddWorkDays(2,DateTime.Today).ToShortDateString();
			comboProv.Items.Add(Lan.g(this,"All"));
			comboProv.SelectedIndex=0;
			_listProviders=Providers.GetDeepCopy(true);
			for(int i=0;i<_listProviders.Count;i++) {
				comboProv.Items.Add(_listProviders[i].GetLongDesc());
			}
			//textPostcardMessage.Text=PrefC.GetString(PrefName.ConfirmPostcardMessage");
			comboStatus.Items.Clear();
			comboViewStatus.Items.Clear();
			comboViewStatus.Items.Add(new ODBoxItem<Def>(Lan.g(this,"None"),new Def()));
			comboViewStatus.SelectedIndex=0;
			_listApptConfirmedDefs=Defs.GetDefsForCategory(DefCat.ApptConfirmed,true);
			for(int i=0;i<_listApptConfirmedDefs.Count;i++){
				comboStatus.Items.Add(_listApptConfirmedDefs[i].ItemName);
				comboViewStatus.Items.Add(new ODBoxItem<Def>(_listApptConfirmedDefs[i].ItemName,_listApptConfirmedDefs[i]));
			}
			if(!Security.IsAuthorized(Permissions.ApptConfirmStatusEdit,true)) {//Suppress message because it would be very annoying to users.
				comboStatus.Enabled=false;
			}
			if(PrefC.GetBool(PrefName.EasyNoClinics)) {
				comboClinic.Visible=false;
				labelClinic.Visible=false;
			}
			else {
				if(!Security.CurUser.ClinicIsRestricted) {
					comboClinic.Items.Add(Lan.g(this,"All"));
					comboClinic.SelectedIndex=0;
				}
				_listUserClinics=Clinics.GetForUserod(Security.CurUser);
				for(int i=0;i<_listUserClinics.Count;i++) {
					comboClinic.Items.Add(_listUserClinics[i].Abbr);
					if(_listUserClinics[i].ClinicNum==Clinics.ClinicNum) {
						comboClinic.SelectedIndex=i;
						if(!Security.CurUser.ClinicIsRestricted) {
							comboClinic.SelectedIndex++;//add 1 for "All"
						}
					}
				}
			}
			if(!Programs.IsEnabled(ProgramName.CallFire) && !SmsPhones.IsIntegratedTextingEnabled()) {
				butText.Enabled=false;
			}
			FillMain();
			FillComboEmail();
			Cursor=Cursors.Default;
			Plugins.HookAddCode(this,"FormConfirmList.Load_End",butText);
		}

		private void FillComboEmail() {
			_listEmailAddresses=EmailAddresses.GetDeepCopy();//Does not include user specific email addresses.
			List<Clinic> listClinicsAll=Clinics.GetDeepCopy();
			for(int i=0;i<listClinicsAll.Count;i++) {//Exclude any email addresses that are associated to a clinic.
				_listEmailAddresses.RemoveAll(x => x.EmailAddressNum==listClinicsAll[i].EmailAddressNum);
			}
			//Exclude default practice email address.
			_listEmailAddresses.RemoveAll(x => x.EmailAddressNum==PrefC.GetLong(PrefName.EmailDefaultAddressNum));
			//Exclude web mail notification email address.
			_listEmailAddresses.RemoveAll(x => x.EmailAddressNum==PrefC.GetLong(PrefName.EmailNotifyAddressNum));
			comboEmailFrom.Items.Add(Lan.g(this,"Practice/Clinic"));//default
			comboEmailFrom.SelectedIndex=0;
			//Add all email addresses which are not associated to a user, a clinic, or either of the default email addresses.
			for(int i=0;i<_listEmailAddresses.Count;i++) {
				comboEmailFrom.Items.Add(_listEmailAddresses[i].EmailUsername);
			}
			//Add user specific email address if present.
			EmailAddress emailAddressMe=EmailAddresses.GetForUser(Security.CurUser.UserNum);//can be null
			if(emailAddressMe!=null) {
				_listEmailAddresses.Insert(0,emailAddressMe);
				comboEmailFrom.Items.Insert(1,Lan.g(this,"Me")+" <"+emailAddressMe.EmailUsername+">");//Just below Practice/Clinic
			}
		}

		private void menuRight_click(object sender,System.EventArgs e) {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an appointment first.");
				return;
			}
			switch(menuRightClick.Items.IndexOf((ToolStripMenuItem)sender)) {
				case 0:
					SelectPatient_Click();
					break;
				case 1:
					SeeChart_Click();
					break;
				case 2:
					SendPinboard_Click();
					break;
			}
		}

		private void SelectPatient_Click() {
			//If multiple selected, just take the last one to remain consistent with SendPinboard_Click.
			long patNum=PIn.Long(Table.Rows[gridMain.SelectedIndices[gridMain.SelectedIndices.Length-1]]["PatNum"].ToString());
			Patient pat=Patients.GetPat(patNum);
			FormOpenDental.S_Contr_PatientSelected(pat,true);
		}

		private void gridMain_MouseUp(object sender,MouseEventArgs e) {
			if(e.Button==MouseButtons.Right && gridMain.SelectedIndices.Length>0) {
				//To maintain legacy behavior we will use the last selected index if multiple are selected.
				Patient pat=Patients.GetLim(PIn.Long(Table.Rows[gridMain.SelectedIndices[gridMain.SelectedIndices.Length-1]]["PatNum"].ToString()));
				toolStripMenuItemSelectPatient.Text=Lan.g(gridMain.TranslationName,"Select Patient")+" ("+pat.GetNameFL()+")";
			}
		}

		///<summary>If multiple patients are selected in UnchedList, will select the last patient to remain consistent with sending to pinboard behavior.</summary>
		private void SeeChart_Click() {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an appointment first.");
				return;
			}
			//If multiple selected, just take the last one to remain consistent with SendPinboard_Click.
			long patNum=PIn.Long(Table.Rows[gridMain.SelectedIndices[gridMain.SelectedIndices.Length-1]]["PatNum"].ToString());
			Patient pat=Patients.GetPat(patNum);
			FormOpenDental.S_Contr_PatientSelected(pat,false);
			GotoModule.GotoChart(pat.PatNum);
		}

		private void SendPinboard_Click() {
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"Please select an appointment first.");
				return;
			}
			List<long> listAptSelected=new List<long>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				listAptSelected.Add(PIn.Long(Table.Rows[gridMain.SelectedIndices[i]]["AptNum"].ToString()));
			}
			//This will send all appointments in listAptSelected to the pinboard, and will select the patient attached to the last appointment.
			GotoModule.PinToAppt(listAptSelected,0);
		}

		///<summary>Adds the specified number of work days, skipping saturday and sunday.</summary>
		private DateTime AddWorkDays(int days,DateTime date){
			DateTime retVal=date;
			for(int i=0;i<days;i++){
				retVal=retVal.AddDays(1);
				//then, this part jumps to monday if on a sat or sun
				while(retVal.DayOfWeek==DayOfWeek.Saturday || retVal.DayOfWeek==DayOfWeek.Sunday){
					retVal=retVal.AddDays(1);
				}
			}
			return retVal;
		}

		private void FillMain(){
			DateTime dateFrom=PIn.Date(textDateFrom.Text);
			DateTime dateTo=PIn.Date(textDateTo.Text);
			long provNum=0;
			if(comboProv.SelectedIndex!=0) {
				provNum=_listProviders[comboProv.SelectedIndex-1].ProvNum;
			}
			long clinicNum=0;
			//if clinics are not enabled, comboClinic.SelectedIndex will be -1, so clinicNum will be 0 and list will not be filtered by clinic
			if(Security.CurUser.ClinicIsRestricted && comboClinic.SelectedIndex>-1) {
				clinicNum=_listUserClinics[comboClinic.SelectedIndex].ClinicNum;
			}
			else if(comboClinic.SelectedIndex > 0) {//if user is not restricted, clinicNum will be 0 and the query will get all clinic data
				clinicNum=_listUserClinics[comboClinic.SelectedIndex-1].ClinicNum;//if user is not restricted, comboClinic will contain "All" so minus 1
			}
			bool showRecalls=false;
			bool showNonRecalls=false;
			bool showHygienePrescheduled=false;
			if(comboShowRecall.SelectedIndex==0 || comboShowRecall.SelectedIndex==1) {//All or Recall Only
				showRecalls=true;
			}
			if(comboShowRecall.SelectedIndex==0 || comboShowRecall.SelectedIndex==2) {//All or Exclude Recalls
				showNonRecalls=true;
			}
			if(comboShowRecall.SelectedIndex==0 || comboShowRecall.SelectedIndex==3) {//All or Hygiene Prescheduled
				showHygienePrescheduled=true;
			}
			Def apptConfirmedType=((ODBoxItem<Def>)comboViewStatus.SelectedItem).Tag;
			Table=Appointments.GetConfirmList(dateFrom,dateTo,provNum,clinicNum,showRecalls,showNonRecalls,showHygienePrescheduled,apptConfirmedType.DefNum);
			int scrollVal=gridMain.ScrollValue;
			gridMain.BeginUpdate();
			gridMain.Columns.Clear();
			ODGridColumn col=new ODGridColumn(Lan.g("TableConfirmList","Date Time"),70);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableConfirmList","DateSched"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableConfirmList","Patient"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableConfirmList","Age"),30);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableConfirmList","Contact"),150);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableConfirmList","Addr/Ph Note"),100);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableConfirmList","Status"),80);//confirmed
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableConfirmList","Procs"),110);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableConfirmList","Medical"),80);
			gridMain.Columns.Add(col);
			col=new ODGridColumn(Lan.g("TableConfirmList","Appt Note"),124);
			gridMain.Columns.Add(col);
			gridMain.Rows.Clear();
			ODGridRow row;
			ODGridCell cell;
			for(int i=0;i<Table.Rows.Count;i++){
				row=new ODGridRow();
				//aptDateTime=PIn.PDateT(table.Rows[i][4].ToString());
				row.Cells.Add(Table.Rows[i]["aptDateTime"].ToString());
				row.Cells.Add(Table.Rows[i]["dateSched"].ToString());
				//aptDateTime.ToShortDateString()+"\r\n"+aptDateTime.ToShortTimeString());
				row.Cells.Add(Table.Rows[i]["patientName"].ToString());
				row.Cells.Add(Table.Rows[i]["age"].ToString());
				row.Cells.Add(Table.Rows[i]["contactMethod"].ToString());
				row.Cells.Add(Table.Rows[i]["AddrNote"].ToString());
				row.Cells.Add(Table.Rows[i]["confirmed"].ToString());
				row.Cells.Add(Table.Rows[i]["ProcDescript"].ToString());
				cell=new ODGridCell(Table.Rows[i]["medNotes"].ToString());
				cell.ColorText=Color.Red;
				row.Cells.Add(cell);
				row.Cells.Add(Table.Rows[i]["Note"].ToString());
				gridMain.Rows.Add(row);
			}
			gridMain.EndUpdate();
			gridMain.ScrollValue=scrollVal;
		}

		private void grid_CellClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			//row selected before this event triggered
			SetFamilyColors();
			comboStatus.SelectedIndex=-1;
		}

		private void SetFamilyColors(){
			if(gridMain.SelectedIndices.Length!=1){
				for(int i=0;i<gridMain.Rows.Count;i++){
					gridMain.Rows[i].ColorText=Color.Black;
				}
				gridMain.Invalidate();
				return;
			}
			long guar=PIn.Long(Table.Rows[gridMain.SelectedIndices[0]]["Guarantor"].ToString());
			int famCount=0;
			for(int i=0;i<gridMain.Rows.Count;i++){
				if(PIn.Long(Table.Rows[i]["Guarantor"].ToString())==guar){
					famCount++;
					gridMain.Rows[i].ColorText=Color.Red;
				}
				else{
					gridMain.Rows[i].ColorText=Color.Black;
				}
			}
			if(famCount==1){//only the highlighted patient is red at this point
				gridMain.Rows[gridMain.SelectedIndices[0]].ColorText=Color.Black;
			}
			gridMain.Invalidate();
		}

		private void grid_Click(object sender,EventArgs e) {
			
		}

		private void grid_CellDoubleClick(object sender, OpenDental.UI.ODGridClickEventArgs e) {
			Cursor=Cursors.WaitCursor;
			long selectedApt=PIn.Long(Table.Rows[e.Row]["AptNum"].ToString());
			Patient pat=Patients.GetPat(PIn.Long(Table.Rows[e.Row]["PatNum"].ToString()));
			FormOpenDental.S_Contr_PatientSelected(pat,true);
			FormApptEdit FormA=new FormApptEdit(selectedApt);
			FormA.PinIsVisible=true;
			FormA.ShowDialog();
			if(FormA.PinClicked) {//set from inside form.
				SendPinboard_Click();//Whatever they double clicked on will still be selected, just fire the event.
				DialogResult=DialogResult.OK;
			}
			else {
				FillMain();
			}
			for(int i=0;i<Table.Rows.Count;i++){
				if(PIn.Long(Table.Rows[i]["AptNum"].ToString())==selectedApt){
					gridMain.SetSelected(i,true);
				}
			}
			SetFamilyColors();
			Cursor=Cursors.Default;
		}

		private void comboStatus_SelectionChangeCommitted(object sender, System.EventArgs e) {
			if(gridMain.SelectedIndices.Length==0){
				return;//because user could never initiate this action.
			}
			Appointment apt;
			Cursor=Cursors.WaitCursor;
			long[] selectedApts=new long[gridMain.SelectedIndices.Length];
			for(int i=0;i<gridMain.SelectedIndices.Length;i++){
				selectedApts[i]=PIn.Long(Table.Rows[gridMain.SelectedIndices[i]]["AptNum"].ToString());
			}
			for(int i=0;i<gridMain.SelectedIndices.Length;i++){
				apt=Appointments.GetOneApt(PIn.Long(Table.Rows[gridMain.SelectedIndices[i]]["AptNum"].ToString()));
				Appointment aptOld=apt.Copy();
				int selectedI=comboStatus.SelectedIndex;
				apt.Confirmed=_listApptConfirmedDefs[selectedI].DefNum;
				try{
					Appointments.Update(apt,aptOld);
				}
				catch(ApplicationException ex){
					Cursor=Cursors.Default;
					MessageBox.Show(ex.Message);
					return;
				}
				if(apt.Confirmed!=aptOld.Confirmed) {
					//Log confirmation status changes.
					SecurityLogs.MakeLogEntry(Permissions.ApptConfirmStatusEdit,apt.PatNum,Lans.g(this,"Appointment confirmation status changed from")+" "
						+Defs.GetName(DefCat.ApptConfirmed,aptOld.Confirmed)+" "+Lans.g(this,"to")+" "+Defs.GetName(DefCat.ApptConfirmed,apt.Confirmed)
						+" "+Lans.g(this,"from the confirmation list")+".",apt.AptNum,aptOld.DateTStamp);
				}
			}
			FillMain();
			//reselect all the apts
			for(int i=0;i<Table.Rows.Count;i++){
				for(int j=0;j<selectedApts.Length;j++){
					if(PIn.Long(Table.Rows[i]["AptNum"].ToString())==selectedApts[j]){
						gridMain.SetSelected(i,true);
					}
				}
			}
			SetFamilyColors();
			comboStatus.SelectedIndex=-1;
			Cursor=Cursors.Default;
		}

		private void comboStatus_SelectedIndexChanged(object sender, System.EventArgs e) {
			//?
		}

		private void butReport_Click(object sender, System.EventArgs e) {
			if(!Security.IsAuthorized(Permissions.UserQuery)) {
				return;
			}
		  if(Table.Rows.Count==0){
        MessageBox.Show(Lan.g(this,"There are no appointments in the list.  Must have at least one to run report."));    
        return;
      }
			long[] aptNums;
      if(gridMain.SelectedIndices.Length==0){
				aptNums=new long[Table.Rows.Count];
        for(int i=0;i<aptNums.Length;i++){
          aptNums[i]=PIn.Long(Table.Rows[i]["AptNum"].ToString());
        }
      }
      else{
				aptNums=new long[gridMain.SelectedIndices.Length];
        for(int i=0;i<aptNums.Length;i++){
          aptNums[i]=PIn.Long(Table.Rows[gridMain.SelectedIndices[i]]["AptNum"].ToString());
        }
      }
      FormRpConfirm FormC=new FormRpConfirm(aptNums);
      FormC.ShowDialog(); 
		}

		private void butLabels_Click(object sender, System.EventArgs e) {
			if(Table.Rows.Count==0){
        MessageBox.Show(Lan.g(this,"There are no appointments in the list.  Must have at least one to print."));    
        return;
      }
			if(gridMain.SelectedIndices.Length==0){
				for(int i=0;i<Table.Rows.Count;i++){
					gridMain.SetSelected(i,true);
				}
			}
			List<long> aptNums=new List<long>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
        aptNums.Add(PIn.Long(Table.Rows[gridMain.SelectedIndices[i]]["AptNum"].ToString()));
      }
			AddrTable=Appointments.GetAddrTable(aptNums);
			pagesPrinted=0;
			patientsPrinted=0;
			pd=new PrintDocument();
			pd.PrintPage+=new PrintPageEventHandler(this.pdLabels_PrintPage);
			pd.OriginAtMargins=true;
			pd.DefaultPageSettings.Margins=new Margins(0,0,0,0);
			printPreview=new FormPrintPreview(PrintSituation.LabelSheet
				,pd,(int)Math.Ceiling((double)AddrTable.Rows.Count/30),0,"Confirmation list labels printed");
			printPreview.ShowDialog();
		}

		///<summary>Changes made to printing confirmation postcards need to be made in FormRecallList.butPostcards_Click() as well.</summary>
		private void butPostcards_Click(object sender,System.EventArgs e) {
			if(Table.Rows.Count==0) {
				MessageBox.Show(Lan.g(this,"There are no appointments in the list.  Must have at least one to print."));
				return;
			}
			if(gridMain.SelectedIndices.Length==0) {
				ContactMethod cmeth;
				for(int i=0;i<Table.Rows.Count;i++) {
					cmeth=(ContactMethod)PIn.Long(Table.Rows[i]["PreferConfirmMethod"].ToString());
					if(cmeth!=ContactMethod.Mail && cmeth!=ContactMethod.None) {
						continue;
					}
					gridMain.SetSelected(i,true);
				}
			}
			List<long> aptNums=new List<long>();
			for(int i=0;i<gridMain.SelectedIndices.Length;i++) {
				aptNums.Add(PIn.Long(Table.Rows[gridMain.SelectedIndices[i]]["AptNum"].ToString()));
			}
			if(aptNums.Count==0) {
				MsgBox.Show(this,"No postcards necessary because contact method is not set to Mail for anyone in the list.");
				return;
			}
			AddrTable=Appointments.GetAddrTable(aptNums);
			pagesPrinted=0;
			patientsPrinted=0;
			pd=new PrintDocument();
			pd.PrintPage+=new PrintPageEventHandler(this.pdCards_PrintPage);
			pd.OriginAtMargins=true;
			pd.DefaultPageSettings.Margins=new Margins(0,0,0,0);
			if(PrefC.GetLong(PrefName.RecallPostcardsPerSheet)==1) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("Postcard",500,700);
				pd.DefaultPageSettings.Landscape=true;
			}
			else if(PrefC.GetLong(PrefName.RecallPostcardsPerSheet)==3) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("Postcard",850,1100);
			}
			else {//4
				pd.DefaultPageSettings.PaperSize=new PaperSize("Postcard",850,1100);
				pd.DefaultPageSettings.Landscape=true;
			}
			printPreview=new FormPrintPreview(PrintSituation.Postcard,pd,
				(int)Math.Ceiling((double)AddrTable.Rows.Count/(double)PrefC.GetLong(PrefName.RecallPostcardsPerSheet)),0,"Confirmation list postcards printed");
			printPreview.ShowDialog();
			if(printPreview.DialogResult==DialogResult.OK) { //dialog result was OK means that the postcards were sent to the printer.
				for(int i=0;i<AddrTable.Rows.Count;i++) { //loop through the address table and create commlog entries for all selected.
					Commlog postcardCommlog=new Commlog();
						postcardCommlog.CommDateTime=DateTimeOD.Today;
						postcardCommlog.Mode_=CommItemMode.Mail;
						postcardCommlog.Note="Confirmation postcard printed for "+AddrTable.Rows[i]["LName"].ToString()
								+", "+AddrTable.Rows[i]["FName"].ToString()+"\r\n"+AddrTable.Rows[i]["Address"].ToString()+"\r\n";
						if(AddrTable.Rows[i]["Address2"].ToString()!="") {
							postcardCommlog.Note+=AddrTable.Rows[i]["Address2"].ToString()+"\r\n";
						}
						postcardCommlog.Note+=AddrTable.Rows[i]["City"].ToString()+", "
						+AddrTable.Rows[i]["State"].ToString()+"   "
						+AddrTable.Rows[i]["Zip"].ToString()+"\r\n";
						postcardCommlog.PatNum=PIn.Long(AddrTable.Rows[i]["PatNum"].ToString());
						postcardCommlog.CommType=Commlogs.GetTypeAuto(CommItemTypeAuto.MISC);
						postcardCommlog.SentOrReceived=CommSentOrReceived.Sent;
						postcardCommlog.UserNum=Security.CurUser.UserNum;
					Commlogs.Insert(postcardCommlog);
				}
			}
		}

		///<summary>raised for each page to be printed.</summary>
		private void pdLabels_PrintPage(object sender, PrintPageEventArgs ev){
			int totalPages=(int)Math.Ceiling((double)AddrTable.Rows.Count/30);
			Graphics g=ev.Graphics;
			float yPos=75;
			float xPos=50;
			string text="";
			while(yPos<1000 && patientsPrinted<AddrTable.Rows.Count){
				text=AddrTable.Rows[patientsPrinted]["FName"].ToString()+" "
					+AddrTable.Rows[patientsPrinted]["MiddleI"].ToString()+" "
					+AddrTable.Rows[patientsPrinted]["LName"].ToString()+"\r\n"
					+AddrTable.Rows[patientsPrinted]["Address"].ToString()+"\r\n";
				if(AddrTable.Rows[patientsPrinted]["Address2"].ToString()!=""){
					text+=AddrTable.Rows[patientsPrinted]["Address2"].ToString()+"\r\n";
				}
				text+=AddrTable.Rows[patientsPrinted]["City"].ToString()+", "
					+AddrTable.Rows[patientsPrinted]["State"].ToString()+"   "
					+AddrTable.Rows[patientsPrinted]["Zip"].ToString()+"\r\n";
				Rectangle rect=new Rectangle((int)xPos,(int)yPos,275,100);
				MapAreaRoomControl.FitText(text,new Font(FontFamily.GenericSansSerif,11),Brushes.Black,rect,new StringFormat(),g);
				//reposition for next label
				xPos+=275;
				if(xPos>850){//drop a line
					xPos=50;
					yPos+=100;
				}
				patientsPrinted++;
			}
			pagesPrinted++;
			if(pagesPrinted>=totalPages){
				ev.HasMorePages=false;
				pagesPrinted=0;//because it has to print again from the print preview
				patientsPrinted=0;
			}
			else{
				ev.HasMorePages=true;
			}
			g.Dispose();
		}

		///<summary>raised for each page to be printed.</summary>
		private void pdCards_PrintPage(object sender, PrintPageEventArgs ev){
			int totalPages=(int)Math.Ceiling((double)AddrTable.Rows.Count/(double)PrefC.GetLong(PrefName.RecallPostcardsPerSheet));
			Graphics g=ev.Graphics;
			int yAdj=(int)(PrefC.GetDouble(PrefName.RecallAdjustDown)*100);
			int xAdj=(int)(PrefC.GetDouble(PrefName.RecallAdjustRight)*100);
			float yPos=0+yAdj;//these refer to the upper left origin of each postcard
			float xPos=0+xAdj;
			string str;
			while(yPos<ev.PageBounds.Height-100 && patientsPrinted<AddrTable.Rows.Count){
				//Return Address--------------------------------------------------------------------------
				if(PrefC.GetBool(PrefName.RecallCardsShowReturnAdd)){
					if(PrefC.GetBool(PrefName.EasyNoClinics) || PIn.Long(AddrTable.Rows[patientsPrinted]["ClinicNum"].ToString())==0) {//No clinics or no clinic selected for this appt
						str=PrefC.GetString(PrefName.PracticeTitle)+"\r\n";
						g.DrawString(str,new Font(FontFamily.GenericSansSerif,9,FontStyle.Bold),Brushes.Black,xPos+45,yPos+60);
						str=PrefC.GetString(PrefName.PracticeAddress)+"\r\n";
						if(PrefC.GetString(PrefName.PracticeAddress2)!="") {
							str+=PrefC.GetString(PrefName.PracticeAddress2)+"\r\n";
						}
						str+=PrefC.GetString(PrefName.PracticeCity)+",  "+PrefC.GetString(PrefName.PracticeST)+"  "+PrefC.GetString(PrefName.PracticeZip)+"\r\n";
						string phone=PrefC.GetString(PrefName.PracticePhone);
						if(CultureInfo.CurrentCulture.Name=="en-US"&& phone.Length==10) {
							str+="("+phone.Substring(0,3)+")"+phone.Substring(3,3)+"-"+phone.Substring(6);
						}
						else {//any other phone format
							str+=phone;
						}
					}
					else {//Clinics enabled and clinic selected
						Clinic clinic=Clinics.GetClinic(PIn.Long(AddrTable.Rows[patientsPrinted]["ClinicNum"].ToString()));
						str=clinic.Description+"\r\n";
						g.DrawString(str,new Font(FontFamily.GenericSansSerif,9,FontStyle.Bold),Brushes.Black,xPos+45,yPos+60);
						str=clinic.Address+"\r\n";
						if(clinic.Address2!="") {
							str+=clinic.Address2+"\r\n";
						}
						str+=clinic.City+",  "+clinic.State+"  "+clinic.Zip+"\r\n";
						string phone=clinic.Phone;
						if(CultureInfo.CurrentCulture.Name=="en-US"&& phone.Length==10) {
							str+="("+phone.Substring(0,3)+")"+phone.Substring(3,3)+"-"+phone.Substring(6);
						}
						else {//any other phone format
							str+=phone;
						}
					}
					g.DrawString(str,new Font(FontFamily.GenericSansSerif,8),Brushes.Black,xPos+45,yPos+75);
				}
				//Body text-------------------------------------------------------------------------------
				str=PrefC.GetString(PrefName.ConfirmPostcardMessage);
				//textPostcardMessage.Text;
				DateTime dateTimeCur=PIn.Date(AddrTable.Rows[patientsPrinted]["DateTimeAskedToArrive"].ToString());
				if(dateTimeCur==DateTime.MinValue) {//If there is no DateTimeAskedToArrive set for this appointment.
					//Use the AptDateTime
					dateTimeCur=PIn.Date(AddrTable.Rows[patientsPrinted]["AptDateTime"].ToString());
				}
				str=str.Replace("[date]",dateTimeCur.ToString(PrefC.PatientCommunicationDateFormat));
				str=str.Replace("[time]",dateTimeCur.ToShortTimeString());
				g.DrawString(str,new Font(FontFamily.GenericSansSerif,10),Brushes.Black,new RectangleF(xPos+45,yPos+180,250,190));
				//Patient's Address-----------------------------------------------------------------------
				str=AddrTable.Rows[patientsPrinted]["FName"].ToString()+" "
						+AddrTable.Rows[patientsPrinted]["MiddleI"].ToString()+" "
						+AddrTable.Rows[patientsPrinted]["LName"].ToString()+"\r\n"
						+AddrTable.Rows[patientsPrinted]["Address"].ToString()+"\r\n";
					if(AddrTable.Rows[patientsPrinted]["Address2"].ToString()!=""){
						str+=AddrTable.Rows[patientsPrinted]["Address2"].ToString()+"\r\n";
					}
					str+=AddrTable.Rows[patientsPrinted]["City"].ToString()+", "
						+AddrTable.Rows[patientsPrinted]["State"].ToString()+"   "
						+AddrTable.Rows[patientsPrinted]["Zip"].ToString()+"\r\n";
				g.DrawString(str,new Font(FontFamily.GenericSansSerif,11),Brushes.Black,xPos+320,yPos+240);
				if(PrefC.GetLong(PrefName.RecallPostcardsPerSheet)==1){
					yPos+=400;
				}
				else if(PrefC.GetLong(PrefName.RecallPostcardsPerSheet)==3){
					yPos+=366;
				}
				else{//4
					xPos+=550;
					if(xPos>1000){
						xPos=0+xAdj;
						yPos+=425;
					}
				}
				patientsPrinted++;
			}//while
			pagesPrinted++;
			if(pagesPrinted==totalPages){
				ev.HasMorePages=false;
				pagesPrinted=0;
				patientsPrinted=0;
			}
			else{
				ev.HasMorePages=true;
			}
		}

		private void butRefresh_Click(object sender, System.EventArgs e) {
			FillMain();
		}

		private void butSetStatus_Click(object sender, System.EventArgs e) {
			/*if(comboStatus.SelectedIndex==-1){
				return;
			}
			int[] originalRecalls=new int[tbMain.SelectedIndices.Length];
			for(int i=0;i<tbMain.SelectedIndices.Length;i++){
				originalRecalls[i]=((RecallItem)MainAL[tbMain.SelectedIndices[i]]).RecallNum;
				Recalls.UpdateStatus(
					((RecallItem)MainAL[tbMain.SelectedIndices[i]]).RecallNum,
					Defs.Short[(int)DefCat.RecallUnschedStatus][comboStatus.SelectedIndex].DefNum);
				//((RecallItem)MainAL[tbMain.SelectedIndices[i]]).up
			}
			FillMain();
			for(int i=0;i<tbMain.MaxRows;i++){
				for(int j=0;j<originalRecalls.Length;j++){
					if(originalRecalls[j]==((RecallItem)MainAL[i]).RecallNum){
						tbMain.SetSelected(i,true);
					}
				}
			}*/
		}

		private void butEmail_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.EmailSend)){
				return;
			}
			if(gridMain.Rows.Count==0) {
				MsgBox.Show(this,"There are no Patients in the table.  Must have at least one.");
				return;
			}
			if(!EmailAddresses.ExistsValidEmail()) {
				MsgBox.Show(this,"You need to enter an SMTP server name in e-mail setup before you can send e-mail.");
				return;
			}
			if(PrefC.GetLong(PrefName.ConfirmStatusEmailed)==0) {
				MsgBox.Show(this,"No 'Status for e-mailed confirmation' set in the Setup Confirmation window.");
				return;
			}
			if(gridMain.SelectedIndices.Length==0) {
				ContactMethod cmeth;
				for(int i=0;i<Table.Rows.Count;i++) {
					cmeth=(ContactMethod)PIn.Int(Table.Rows[i]["PreferConfirmMethod"].ToString());
					if(cmeth!=ContactMethod.Email) {
						continue;
					}
					if(Table.Rows[i]["confirmed"].ToString()==Defs.GetName(DefCat.ApptConfirmed,PrefC.GetLong(PrefName.ConfirmStatusEmailed))) {//Already confirmed by email
						continue;
					}
					if(Table.Rows[i]["email"].ToString()=="") {
						continue;
					}
					gridMain.SetSelected(i,true);
				}
				if(gridMain.SelectedIndices.Length==0) {
					MsgBox.Show(this,"Confirmations have been sent to all patients of email type who also have an email address entered.");
					return;
				}
			}
			else {//deselect the ones that do not have email addresses specified
				int skipped=0;
				for(int i=gridMain.SelectedIndices.Length-1;i>=0;i--) {
					if(Table.Rows[gridMain.SelectedIndices[i]]["email"].ToString()=="") {
						skipped++;
						gridMain.SetSelected(gridMain.SelectedIndices[i],false);
					}
				}
				if(gridMain.SelectedIndices.Length==0) {
					MsgBox.Show(this,"None of the selected patients had email addresses entered.");
					return;
				}
				if(skipped>0) {
					MessageBox.Show(Lan.g(this,"Selected patients skipped due to missing email addresses: ")+skipped.ToString());
				}
			}
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Send email to all of the selected patients?")) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			EmailMessage message;
			string str="";
			List<long> listPatNumsSelected=new List<long>();
			List<long> listPatNumsFailed=new List<long>();
			EmailAddress emailAddress;
			string errors="";
			for(int i=0;i<gridMain.SelectedIndices.Length;i++){
				message=new EmailMessage();
				message.PatNum=PIn.Long(Table.Rows[gridMain.SelectedIndices[i]]["PatNum"].ToString());
				message.ToAddress=Table.Rows[gridMain.SelectedIndices[i]]["email"].ToString();//Could be guarantor email.
				if(comboEmailFrom.SelectedIndex==0) { //clinic/practice default
					emailAddress=EmailAddresses.GetByClinic(PIn.Long(Table.Rows[gridMain.SelectedIndices[i]]["ClinicNum"].ToString()));
				}
				else { //me or static email address, email address for 'me' is the first one in _listEmailAddresses
					emailAddress=_listEmailAddresses[comboEmailFrom.SelectedIndex-1];//-1 to account for predefined "Clinic/Practice" item in combobox
				}
				message.FromAddress=emailAddress.GetFrom();				
				message.Subject=PrefC.GetString(PrefName.ConfirmEmailSubject);
				listPatNumsSelected.Add(message.PatNum);
				str=PrefC.GetString(PrefName.ConfirmEmailMessage);
				str=str.Replace("[NameF]",Table.Rows[gridMain.SelectedIndices[i]]["nameF"].ToString());
				str=str.Replace("[NameFL]",Table.Rows[gridMain.SelectedIndices[i]]["nameFL"].ToString());
				str=str.Replace("[date]",((DateTime)Table.Rows[gridMain.SelectedIndices[i]]["AptDateTime"]).ToString(PrefC.PatientCommunicationDateFormat));
				str=str.Replace("[time]",((DateTime)Table.Rows[gridMain.SelectedIndices[i]]["AptDateTime"]).ToShortTimeString());
				message.BodyText=str;
				try {
					EmailMessages.SendEmailUnsecure(message,emailAddress);
				}
				catch (Exception ex){
					listPatNumsFailed.Add(message.PatNum);
					if(!errors.Contains("Message send fail for Patnum:"+message.PatNum+":  "+ex.Message)) {//unique messages only.
						errors+=("Message send fail for Patnum:"+message.PatNum+":  "+ex.Message+"\r\n");
					}
					continue;
				}
				message.MsgDateTime=DateTime.Now;
				message.SentOrReceived=EmailSentOrReceived.Sent;
				EmailMessages.Insert(message);
				Appointment appt=Appointments.GetOneApt(PIn.Long(Table.Rows[gridMain.SelectedIndices[i]]["AptNum"].ToString()));
				Appointments.SetConfirmed(appt,PrefC.GetLong(PrefName.ConfirmStatusEmailed));
			}
			Cursor=Cursors.Default;
			if(listPatNumsFailed.Count==gridMain.SelectedIndices.Length){ //all failed
				//no need to refresh
				if(DialogResult.Yes != MessageBox.Show(Lan.g(this,"All emails failed. Possibly due to invalid email addresses, a loss of connectivity, or a firewall blocking communication.  Would you like to see additional details?"),"",MessageBoxButtons.YesNo)){
					return;
				}
				CodeBase.MsgBoxCopyPaste msgbox=new CodeBase.MsgBoxCopyPaste(errors);
				msgbox.ShowDialog();
				return;
			}
			else if(listPatNumsFailed.Count>0){//if some failed
				FillMain();
				//reselect only the failed ones
				for(int i=0;i<Table.Rows.Count;i++) { //table.Rows.Count=grid.Rows.Count
					long patNum=PIn.Long(Table.Rows[i]["PatNum"].ToString());
					if(listPatNumsFailed.Contains(patNum)) {
						gridMain.SetSelected(i,true);
					}
				}
				if(DialogResult.Yes != MessageBox.Show(Lan.g(this,"Some emails failed to send.  All failed email confirmations have been selected in the confirmation list.  Would you like to see additional details?"),"",MessageBoxButtons.YesNo)) {
					return;
				}
				CodeBase.MsgBoxCopyPaste msgbox=new CodeBase.MsgBoxCopyPaste(errors);
				msgbox.ShowDialog();
				SecurityLogs.MakeLogEntry(Permissions.EmailSend,0,"Confirmation Emails Sent: "+(listPatNumsSelected.Count-listPatNumsFailed.Count));
				return;
			}
			//none failed
			SecurityLogs.MakeLogEntry(Permissions.EmailSend,0,"Confirmation Emails Sent: "+listPatNumsSelected.Count);
			FillMain();
			//reselect the original list 
			for(int i=0;i<Table.Rows.Count;i++) {
				long patNum=PIn.Long(Table.Rows[i]["PatNum"].ToString());
				if(listPatNumsSelected.Contains(patNum)) {
					gridMain.SetSelected(i,true);
				}
			}
		}

		private void butText_Click(object sender,EventArgs e) {
			long patNum;
			string wirelessPhone;
			YN txtMsgOk;
			if(gridMain.Rows.Count==0) {
				MsgBox.Show(this,"There are no Patients in the table.  Must have at least one.");
				return;
			}
			if(PrefC.GetLong(PrefName.ConfirmStatusTextMessaged)==0) {
				MsgBox.Show(this,"You need to set a status for text message confirmations in the Confirmation Setup window.");
				return;
			}
			if(gridMain.SelectedIndices.Length==0) {//None selected. Select all of type text that are not yet confirmed by text message.
				ContactMethod cmeth;
				for(int i=0;i<Table.Rows.Count;i++) {
					cmeth=(ContactMethod)PIn.Int(Table.Rows[i]["PreferConfirmMethod"].ToString());
					if(cmeth!=ContactMethod.TextMessage) {
						continue;
					}
					if(Table.Rows[i]["confirmed"].ToString()==Defs.GetName(DefCat.ApptConfirmed,PrefC.GetLong(PrefName.ConfirmStatusTextMessaged))) {//Already confirmed by text
						continue;
					}
					if(!Table.Rows[i]["contactMethod"].ToString().StartsWith("Text:")) {//Check contact method
						continue;
					}
					gridMain.SetSelected(i,true);
				}
				if(gridMain.SelectedIndices.Length==0) {
					MsgBox.Show(this,"All patients of text message type have been sent confirmations.");
					return;
				}
			}
			//deselect the ones that do not have text messages specified or are not OK to send texts to or have already been texted
			int skipped=0;
			for(int i=gridMain.SelectedIndices.Length-1;i>=0;i--) {
				wirelessPhone=Table.Rows[gridMain.SelectedIndices[i]]["WirelessPhone"].ToString();
				if(wirelessPhone=="") {//Check for wireless number
					skipped++;
					gridMain.SetSelected(gridMain.SelectedIndices[i],false);
					continue;
				}
				txtMsgOk=(YN)PIn.Int(Table.Rows[gridMain.SelectedIndices[i]]["TxtMsgOk"].ToString());
				if(txtMsgOk==YN.Unknown	&& PrefC.GetBool(PrefName.TextMsgOkStatusTreatAsNo)) {//Check if OK to text
					skipped++;
					gridMain.SetSelected(gridMain.SelectedIndices[i],false);
					continue;
				}
				if(txtMsgOk==YN.No){//Check if OK to text
					skipped++;
					gridMain.SetSelected(gridMain.SelectedIndices[i],false);
					continue;
				}
				if(!PrefC.GetBool(PrefName.EasyNoClinics) && SmsPhones.IsIntegratedTextingEnabled()){//using clinics with Integrated texting must have a non-zero clinic num.
					patNum=PIn.Long(Table.Rows[gridMain.SelectedIndices[i]]["PatNum"].ToString());
					long clinicNum=SmsPhones.GetClinicNumForTexting(patNum);
					if(clinicNum==0 || Clinics.GetClinic(clinicNum).SmsContractDate.Year<1880) {//no clinic or assigned clinic is not enabled.
						skipped++;
						gridMain.SetSelected(gridMain.SelectedIndices[i],false);
						continue;
					}
				}
			}
			if(gridMain.SelectedIndices.Length==0) {
				MsgBox.Show(this,"None of the selected patients have wireless phone numbers and are OK to text.");
				return;
			}
			if(skipped>0) {
				MessageBox.Show(Lan.g(this,"Selected patients skipped: ")+skipped.ToString());
			}
			if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"Send text message to all of the selected patients?")) {
				return;
			}
			Cursor=Cursors.WaitCursor;
			FormTxtMsgEdit FormTME=new FormTxtMsgEdit();
			string message="";
			//Appointment apt;
			for(int i=0;i<gridMain.SelectedIndices.Length;i++){
				patNum=PIn.Long(Table.Rows[gridMain.SelectedIndices[i]]["PatNum"].ToString());
				long clinicNum=SmsPhones.GetClinicNumForTexting(patNum);
				wirelessPhone=PIn.String(Table.Rows[gridMain.SelectedIndices[i]]["WirelessPhone"].ToString());
				txtMsgOk=((YN)PIn.Int(Table.Rows[gridMain.SelectedIndices[i]]["TxtMsgOk"].ToString()));
				message=PrefC.GetString(PrefName.ConfirmTextMessage);
				message=message.Replace("[NameF]",Table.Rows[gridMain.SelectedIndices[i]]["nameF"].ToString());
				message=message.Replace("[NameFL]",Table.Rows[gridMain.SelectedIndices[i]]["nameFL"].ToString());
				message=message.Replace("[date]",((DateTime)Table.Rows[gridMain.SelectedIndices[i]]["AptDateTime"])
					.ToString(PrefC.PatientCommunicationDateFormat));
				message=message.Replace("[time]",((DateTime)Table.Rows[gridMain.SelectedIndices[i]]["AptDateTime"]).ToShortTimeString());
				if(FormTME.SendText(patNum,wirelessPhone,message,txtMsgOk,clinicNum,SmsMessageSource.Confirmation)) {
					long aptNum=PIn.Long(Table.Rows[gridMain.SelectedIndices[i]]["AptNum"].ToString());
					long newStatus=PrefC.GetLong(PrefName.ConfirmStatusTextMessaged);
					Appointment aptOld = Appointments.GetOneApt(aptNum);
					long oldStatus=aptOld.Confirmed;
					Appointments.SetConfirmed(aptOld,newStatus);
					if(newStatus!=oldStatus) {
						//Log confirmation status changes.
						SecurityLogs.MakeLogEntry(Permissions.ApptConfirmStatusEdit,patNum,
							Lans.g(this,"Appointment confirmation status automatically changed from")+" "
							+Defs.GetName(DefCat.ApptConfirmed,oldStatus)+" "+Lans.g(this,"to")+" "+Defs.GetName(DefCat.ApptConfirmed,newStatus)
							+" "+Lans.g(this,"from the confirmation list")+".",ContrApptSingle.SelectedAptNum,aptOld.DateTStamp);
					}
				}
				else {//There was an exception thrown in FormTME.SendText() meaning something went wrong.  Give the user an option to stop sending messages.
					if(!MsgBox.Show(this,MsgBoxButtons.YesNo,"There was an error sending, do you want to continue sending messages?")) {
						break;
					}
				}
			}
			FillMain();
			Cursor=Cursors.Default;
		}

		private void butSave_Click(object sender, System.EventArgs e) {
			/*if(  textDaysPast.errorProvider1.GetError(textDaysPast)!=""
				|| textDaysFuture.errorProvider1.GetError(textDaysFuture)!="")
			{
				MessageBox.Show(Lan.g(this,"Please fix data entry errors first."));
				return;
			}
			Prefs.Cur.PrefName="RecallDaysPast";
			Prefs.Cur.ValueString=textDaysPast.Text;
			Prefs.UpdateCur();
			Prefs.Cur.PrefName="RecallDaysFuture";
			Prefs.Cur.ValueString=textDaysFuture.Text;
			Prefs.UpdateCur();
			DataValid.SetInvalid(InvalidTypes.Prefs);*/
		}

		private void butPrint_Click(object sender,EventArgs e) {
			pagesPrinted=0;
			pd=new PrintDocument();
			pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
			pd.DefaultPageSettings.Margins=new Margins(25,25,40,40);
			pd.DefaultPageSettings.Landscape=true;
			//pd.OriginAtMargins=true;
			if(pd.DefaultPageSettings.PrintableArea.Height==0) {
				pd.DefaultPageSettings.PaperSize=new PaperSize("default",850,1100);
			}
			headingPrinted=false;
			try {
				#if DEBUG
				FormRpPrintPreview pView = new FormRpPrintPreview();
				pView.printPreviewControl2.Document=pd;
				pView.ShowDialog();
				#else
					if(PrinterL.SetPrinter(pd,PrintSituation.Default,0,"Confirmation list printed")) {
						pd.Print();
					}
				#endif
			}
			catch {
				MessageBox.Show(Lan.g(this,"Printer not available"));
			}
		}

		private void pd_PrintPage(object sender,System.Drawing.Printing.PrintPageEventArgs e) {
			Rectangle bounds=e.MarginBounds;
			//new Rectangle(50,40,800,1035);//Some printers can handle up to 1042
			Graphics g=e.Graphics;
			string text;
			Font headingFont=new Font("Arial",13,FontStyle.Bold);
			Font subHeadingFont=new Font("Arial",10,FontStyle.Bold);
			int yPos=bounds.Top;
			int center=bounds.X+bounds.Width/2;
			#region printHeading
			if(!headingPrinted) {
				text=Lan.g(this,"Confirmation List");
				g.DrawString(text,headingFont,Brushes.Black,center-g.MeasureString(text,headingFont).Width/2,yPos);
				yPos+=(int)g.MeasureString(text,headingFont).Height;
				text=textDateFrom.Text+" "+Lan.g(this,"to")+" "+textDateTo.Text;
				g.DrawString(text,subHeadingFont,Brushes.Black,center-g.MeasureString(text,subHeadingFont).Width/2,yPos);
				yPos+=20;
				headingPrinted=true;
				headingPrintH=yPos;
			}
			#endregion
			yPos=gridMain.PrintPage(g,pagesPrinted,bounds,headingPrintH);
			pagesPrinted++;
			if(yPos==-1) {
				e.HasMorePages=true;
			}
			else {
				e.HasMorePages=false;
			}
			g.Dispose();
		}

		private void butClose_Click(object sender, System.EventArgs e) {
			Close();
		}

		

	

		

	

		

		

		

		

		
	}

}
