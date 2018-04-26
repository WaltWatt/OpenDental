using System;
using System.Diagnostics;
using System.Windows.Forms;
using OpenDentBusiness;
using OpenDental.Bridges;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using CodeBase;

namespace OpenDental {
	/// <summary>
	/// Summary description for FormBasicTemplate.
	/// </summary>
	public class FormReportsMore:ODForm {
		private OpenDental.UI.Button butClose;
		private Label labelPublicHealth;
		private Label labelLists;
		private Label labelMonthly;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private OpenDental.UI.ListBoxClickable listLists;
		private OpenDental.UI.ListBoxClickable listPublicHealth;
		private OpenDental.UI.Button butUserQuery;
		private OpenDental.UI.Button butPW;
		private OpenDental.UI.ListBoxClickable listProdInc;
		private Label labelProdInc;
		private OpenDental.UI.ListBoxClickable listDaily;
		private Label labelDaily;
		private Label label6;
		private OpenDental.UI.Button butLaserLabels;
		private OpenDental.UI.ListBoxClickable listArizonaPrimaryCare;
		private Label labelArizonaPrimaryCare;
		private OpenDental.UI.ListBoxClickable listMonthly;
		private MenuStrip menuMain;
		private UI.Button butPatList;
		private UI.Button butPatExport;
		private GroupBox groupPatientReviews;
		private ToolStripMenuItem setupToolStripMenuItem;
		private UI.ODPictureBox picturePodium;
		private UI.ODPictureBox pictureDentalIntel;
		private GroupBox groupBusiness;
		///<summary>After this form closes, this value is checked to see if any non-modal dialog boxes are needed.</summary>
		public ReportNonModalSelection RpNonModalSelection;
		private List<DisplayReport> _listProdInc;
		private List<DisplayReport> _listMonthly;
		private List<DisplayReport> _listDaily;
		private List<DisplayReport> _listList;
		private List<DisplayReport> _listPublicHealth;
		private List<DisplayReport> _listArizonaPrimary;
		private UI.ODPictureBox picturePracticeByNumbers;
		private List<GroupPermission> _listReportPermissions;

		///<summary></summary>
		public FormReportsMore() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Lan.F(this);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if(disposing) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReportsMore));
			this.groupBusiness = new System.Windows.Forms.GroupBox();
			this.picturePracticeByNumbers = new OpenDental.UI.ODPictureBox();
			this.pictureDentalIntel = new OpenDental.UI.ODPictureBox();
			this.groupPatientReviews = new System.Windows.Forms.GroupBox();
			this.picturePodium = new OpenDental.UI.ODPictureBox();
			this.labelArizonaPrimaryCare = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.labelDaily = new System.Windows.Forms.Label();
			this.labelProdInc = new System.Windows.Forms.Label();
			this.labelMonthly = new System.Windows.Forms.Label();
			this.labelLists = new System.Windows.Forms.Label();
			this.labelPublicHealth = new System.Windows.Forms.Label();
			this.menuMain = new System.Windows.Forms.MenuStrip();
			this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.butPatExport = new OpenDental.UI.Button();
			this.butPatList = new OpenDental.UI.Button();
			this.listArizonaPrimaryCare = new OpenDental.UI.ListBoxClickable();
			this.butLaserLabels = new OpenDental.UI.Button();
			this.listDaily = new OpenDental.UI.ListBoxClickable();
			this.listProdInc = new OpenDental.UI.ListBoxClickable();
			this.butPW = new OpenDental.UI.Button();
			this.butUserQuery = new OpenDental.UI.Button();
			this.listPublicHealth = new OpenDental.UI.ListBoxClickable();
			this.listLists = new OpenDental.UI.ListBoxClickable();
			this.listMonthly = new OpenDental.UI.ListBoxClickable();
			this.butClose = new OpenDental.UI.Button();
			this.groupBusiness.SuspendLayout();
			this.groupPatientReviews.SuspendLayout();
			this.menuMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBusiness
			// 
			this.groupBusiness.Controls.Add(this.picturePracticeByNumbers);
			this.groupBusiness.Controls.Add(this.pictureDentalIntel);
			this.groupBusiness.Location = new System.Drawing.Point(532, 72);
			this.groupBusiness.Name = "groupBusiness";
			this.groupBusiness.Size = new System.Drawing.Size(113, 81);
			this.groupBusiness.TabIndex = 28;
			this.groupBusiness.TabStop = false;
			this.groupBusiness.Text = "Business Analytics";
			// 
			// picturePracticeByNumbers
			// 
			this.picturePracticeByNumbers.HasBorder = false;
			this.picturePracticeByNumbers.Image = global::OpenDental.Properties.Resources.PracticeByNumbers_100x24;
			this.picturePracticeByNumbers.Location = new System.Drawing.Point(8, 49);
			this.picturePracticeByNumbers.Name = "picturePracticeByNumbers";
			this.picturePracticeByNumbers.Size = new System.Drawing.Size(95, 24);
			this.picturePracticeByNumbers.TabIndex = 31;
			this.picturePracticeByNumbers.TextNullImage = "Practice By Numbers";
			this.picturePracticeByNumbers.Click += new System.EventHandler(this.picturePracticeByNumbers_Click);
			// 
			// pictureDentalIntel
			// 
			this.pictureDentalIntel.HasBorder = false;
			this.pictureDentalIntel.Image = global::OpenDental.Properties.Resources.DI_Button_100x24;
			this.pictureDentalIntel.Location = new System.Drawing.Point(8, 19);
			this.pictureDentalIntel.Name = "pictureDentalIntel";
			this.pictureDentalIntel.Size = new System.Drawing.Size(95, 24);
			this.pictureDentalIntel.TabIndex = 0;
			this.pictureDentalIntel.TextNullImage = null;
			this.pictureDentalIntel.Click += new System.EventHandler(this.pictureDentalIntel_Click);
			// 
			// groupPatientReviews
			// 
			this.groupPatientReviews.Controls.Add(this.picturePodium);
			this.groupPatientReviews.Location = new System.Drawing.Point(532, 159);
			this.groupPatientReviews.Name = "groupPatientReviews";
			this.groupPatientReviews.Size = new System.Drawing.Size(113, 54);
			this.groupPatientReviews.TabIndex = 26;
			this.groupPatientReviews.TabStop = false;
			this.groupPatientReviews.Text = "Patient Reviews";
			// 
			// picturePodium
			// 
			this.picturePodium.HasBorder = false;
			this.picturePodium.Image = global::OpenDental.Properties.Resources.Podium_Button_100x24;
			this.picturePodium.Location = new System.Drawing.Point(8, 19);
			this.picturePodium.Name = "picturePodium";
			this.picturePodium.Size = new System.Drawing.Size(95, 24);
			this.picturePodium.TabIndex = 28;
			this.picturePodium.TextNullImage = null;
			this.picturePodium.Click += new System.EventHandler(this.picturePodium_Click);
			// 
			// labelArizonaPrimaryCare
			// 
			this.labelArizonaPrimaryCare.Location = new System.Drawing.Point(281, 410);
			this.labelArizonaPrimaryCare.Name = "labelArizonaPrimaryCare";
			this.labelArizonaPrimaryCare.Size = new System.Drawing.Size(156, 13);
			this.labelArizonaPrimaryCare.TabIndex = 20;
			this.labelArizonaPrimaryCare.Text = "Arizona Primary Care";
			this.labelArizonaPrimaryCare.Visible = false;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label6.Location = new System.Drawing.Point(9, 574);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(568, 82);
			this.label6.TabIndex = 17;
			this.label6.Text = resources.GetString("label6.Text");
			// 
			// labelDaily
			// 
			this.labelDaily.Location = new System.Drawing.Point(9, 200);
			this.labelDaily.Name = "labelDaily";
			this.labelDaily.Size = new System.Drawing.Size(118, 18);
			this.labelDaily.TabIndex = 15;
			this.labelDaily.Text = "Daily";
			this.labelDaily.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelProdInc
			// 
			this.labelProdInc.Location = new System.Drawing.Point(9, 54);
			this.labelProdInc.Name = "labelProdInc";
			this.labelProdInc.Size = new System.Drawing.Size(207, 18);
			this.labelProdInc.TabIndex = 13;
			this.labelProdInc.Text = "Production and Income";
			this.labelProdInc.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelMonthly
			// 
			this.labelMonthly.Location = new System.Drawing.Point(9, 349);
			this.labelMonthly.Name = "labelMonthly";
			this.labelMonthly.Size = new System.Drawing.Size(118, 18);
			this.labelMonthly.TabIndex = 6;
			this.labelMonthly.Text = "Monthly";
			this.labelMonthly.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelLists
			// 
			this.labelLists.Location = new System.Drawing.Point(281, 54);
			this.labelLists.Name = "labelLists";
			this.labelLists.Size = new System.Drawing.Size(118, 18);
			this.labelLists.TabIndex = 4;
			this.labelLists.Text = "Lists";
			this.labelLists.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// labelPublicHealth
			// 
			this.labelPublicHealth.Location = new System.Drawing.Point(281, 305);
			this.labelPublicHealth.Name = "labelPublicHealth";
			this.labelPublicHealth.Size = new System.Drawing.Size(118, 18);
			this.labelPublicHealth.TabIndex = 2;
			this.labelPublicHealth.Text = "Public Health";
			this.labelPublicHealth.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// menuMain
			// 
			this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setupToolStripMenuItem});
			this.menuMain.Location = new System.Drawing.Point(0, 0);
			this.menuMain.Name = "menuMain";
			this.menuMain.Size = new System.Drawing.Size(680, 24);
			this.menuMain.TabIndex = 22;
			// 
			// setupToolStripMenuItem
			// 
			this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
			this.setupToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
			this.setupToolStripMenuItem.Text = "Setup";
			this.setupToolStripMenuItem.Click += new System.EventHandler(this.setupToolStripMenuItem_Click);
			// 
			// butPatExport
			// 
			this.butPatExport.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPatExport.Autosize = true;
			this.butPatExport.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPatExport.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPatExport.CornerRadius = 4F;
			this.butPatExport.Location = new System.Drawing.Point(538, 262);
			this.butPatExport.Name = "butPatExport";
			this.butPatExport.Size = new System.Drawing.Size(101, 24);
			this.butPatExport.TabIndex = 24;
			this.butPatExport.Text = "EHR Pat Export";
			this.butPatExport.Visible = false;
			this.butPatExport.Click += new System.EventHandler(this.butPatExport_Click);
			// 
			// butPatList
			// 
			this.butPatList.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPatList.Autosize = true;
			this.butPatList.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPatList.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPatList.CornerRadius = 4F;
			this.butPatList.Location = new System.Drawing.Point(538, 232);
			this.butPatList.Name = "butPatList";
			this.butPatList.Size = new System.Drawing.Size(101, 24);
			this.butPatList.TabIndex = 23;
			this.butPatList.Text = "EHR Patient List";
			this.butPatList.Visible = false;
			this.butPatList.Click += new System.EventHandler(this.butPatList_Click);
			// 
			// listArizonaPrimaryCare
			// 
			this.listArizonaPrimaryCare.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listArizonaPrimaryCare.FormattingEnabled = true;
			this.listArizonaPrimaryCare.ItemHeight = 15;
			this.listArizonaPrimaryCare.Location = new System.Drawing.Point(284, 424);
			this.listArizonaPrimaryCare.Name = "listArizonaPrimaryCare";
			this.listArizonaPrimaryCare.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listArizonaPrimaryCare.Size = new System.Drawing.Size(242, 49);
			this.listArizonaPrimaryCare.TabIndex = 19;
			this.listArizonaPrimaryCare.Visible = false;
			this.listArizonaPrimaryCare.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listArizonaPrimaryCare_MouseDown);
			// 
			// butLaserLabels
			// 
			this.butLaserLabels.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butLaserLabels.Autosize = true;
			this.butLaserLabels.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butLaserLabels.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butLaserLabels.CornerRadius = 4F;
			this.butLaserLabels.Location = new System.Drawing.Point(294, 26);
			this.butLaserLabels.Name = "butLaserLabels";
			this.butLaserLabels.Size = new System.Drawing.Size(75, 24);
			this.butLaserLabels.TabIndex = 18;
			this.butLaserLabels.Text = "Laser Labels";
			this.butLaserLabels.UseVisualStyleBackColor = true;
			this.butLaserLabels.Click += new System.EventHandler(this.butLaserLabels_Click);
			// 
			// listDaily
			// 
			this.listDaily.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listDaily.FormattingEnabled = true;
			this.listDaily.ItemHeight = 15;
			this.listDaily.Location = new System.Drawing.Point(12, 221);
			this.listDaily.Name = "listDaily";
			this.listDaily.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listDaily.Size = new System.Drawing.Size(242, 124);
			this.listDaily.TabIndex = 16;
			this.listDaily.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listDaily_MouseDown);
			// 
			// listProdInc
			// 
			this.listProdInc.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listProdInc.FormattingEnabled = true;
			this.listProdInc.ItemHeight = 15;
			this.listProdInc.Location = new System.Drawing.Point(12, 75);
			this.listProdInc.Name = "listProdInc";
			this.listProdInc.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listProdInc.Size = new System.Drawing.Size(242, 124);
			this.listProdInc.TabIndex = 14;
			this.listProdInc.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listProdInc_MouseDown);
			// 
			// butPW
			// 
			this.butPW.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butPW.Autosize = true;
			this.butPW.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butPW.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butPW.CornerRadius = 4F;
			this.butPW.Location = new System.Drawing.Point(135, 26);
			this.butPW.Name = "butPW";
			this.butPW.Size = new System.Drawing.Size(84, 24);
			this.butPW.TabIndex = 12;
			this.butPW.Text = "PW Reports";
			this.butPW.Click += new System.EventHandler(this.butPW_Click);
			// 
			// butUserQuery
			// 
			this.butUserQuery.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butUserQuery.Autosize = true;
			this.butUserQuery.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butUserQuery.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butUserQuery.CornerRadius = 4F;
			this.butUserQuery.Location = new System.Drawing.Point(12, 26);
			this.butUserQuery.Name = "butUserQuery";
			this.butUserQuery.Size = new System.Drawing.Size(84, 24);
			this.butUserQuery.TabIndex = 11;
			this.butUserQuery.Text = "User Query";
			this.butUserQuery.Click += new System.EventHandler(this.butUserQuery_Click);
			// 
			// listPublicHealth
			// 
			this.listPublicHealth.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listPublicHealth.FormattingEnabled = true;
			this.listPublicHealth.ItemHeight = 15;
			this.listPublicHealth.Location = new System.Drawing.Point(284, 325);
			this.listPublicHealth.Name = "listPublicHealth";
			this.listPublicHealth.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listPublicHealth.Size = new System.Drawing.Size(242, 79);
			this.listPublicHealth.TabIndex = 10;
			this.listPublicHealth.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listPublicHealth_MouseDown);
			// 
			// listLists
			// 
			this.listLists.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listLists.FormattingEnabled = true;
			this.listLists.ItemHeight = 15;
			this.listLists.Location = new System.Drawing.Point(284, 75);
			this.listLists.Name = "listLists";
			this.listLists.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listLists.Size = new System.Drawing.Size(242, 229);
			this.listLists.TabIndex = 9;
			this.listLists.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listLists_MouseDown);
			// 
			// listMonthly
			// 
			this.listMonthly.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.listMonthly.FormattingEnabled = true;
			this.listMonthly.ItemHeight = 15;
			this.listMonthly.Location = new System.Drawing.Point(12, 370);
			this.listMonthly.Name = "listMonthly";
			this.listMonthly.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.listMonthly.Size = new System.Drawing.Size(242, 199);
			this.listMonthly.TabIndex = 8;
			this.listMonthly.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listMonthly_MouseDown);
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
			this.butClose.Location = new System.Drawing.Point(583, 618);
			this.butClose.Name = "butClose";
			this.butClose.Size = new System.Drawing.Size(75, 26);
			this.butClose.TabIndex = 0;
			this.butClose.Text = "Close";
			this.butClose.Click += new System.EventHandler(this.butClose_Click);
			// 
			// FormReportsMore
			// 
			this.CancelButton = this.butClose;
			this.ClientSize = new System.Drawing.Size(680, 665);
			this.Controls.Add(this.groupBusiness);
			this.Controls.Add(this.groupPatientReviews);
			this.Controls.Add(this.butPatExport);
			this.Controls.Add(this.butPatList);
			this.Controls.Add(this.labelArizonaPrimaryCare);
			this.Controls.Add(this.listArizonaPrimaryCare);
			this.Controls.Add(this.butLaserLabels);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.listDaily);
			this.Controls.Add(this.labelDaily);
			this.Controls.Add(this.listProdInc);
			this.Controls.Add(this.labelProdInc);
			this.Controls.Add(this.butPW);
			this.Controls.Add(this.butUserQuery);
			this.Controls.Add(this.listPublicHealth);
			this.Controls.Add(this.listLists);
			this.Controls.Add(this.listMonthly);
			this.Controls.Add(this.labelMonthly);
			this.Controls.Add(this.labelLists);
			this.Controls.Add(this.labelPublicHealth);
			this.Controls.Add(this.butClose);
			this.Controls.Add(this.menuMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuMain;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormReportsMore";
			this.ShowInTaskbar = false;
			this.Text = "Reports";
			this.Load += new System.EventHandler(this.FormReportsMore_Load);
			this.groupBusiness.ResumeLayout(false);
			this.groupPatientReviews.ResumeLayout(false);
			this.menuMain.ResumeLayout(false);
			this.menuMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void FormReportsMore_Load(object sender,EventArgs e) {
			Plugins.HookAddCode(this,"FormReportsMore.FormReportsMore_Load_beginning");
			butPW.Visible=Programs.IsEnabled(ProgramName.PracticeWebReports);
			//hiding feature for 13.3
			//butPatList.Visible=PrefC.GetBool(PrefName.ShowFeatureEhr);
			butPatExport.Visible=PrefC.GetBool(PrefName.ShowFeatureEhr);
			FillLists();
			if(ProgramProperties.IsAdvertisingDisabled(ProgramName.Podium)) {
				groupPatientReviews.Visible=false;
			}
			if(ProgramProperties.IsAdvertisingDisabled(ProgramName.DentalIntel)
				&& ProgramProperties.IsAdvertisingDisabled(ProgramName.PracticeByNumbers)) 
			{
				groupBusiness.Visible=false;
			}
			else if(ProgramProperties.IsAdvertisingDisabled(ProgramName.DentalIntel)
				|| (!Programs.GetCur(ProgramName.DentalIntel).Enabled && Programs.GetCur(ProgramName.PracticeByNumbers).Enabled))
			{
				//Don't show the marketing button if:
				//1.  HQ wants to hide the button
				//2.  The office wants to hide the button
				//3.  The office is using PBN but not DentalIntel
				pictureDentalIntel.Visible=false;
			}
			else if(ProgramProperties.IsAdvertisingDisabled(ProgramName.PracticeByNumbers) 
				|| (!Programs.GetCur(ProgramName.PracticeByNumbers).Enabled && Programs.GetCur(ProgramName.DentalIntel).Enabled))
			{
				//Don't show the marketing button if:
				//1.  HQ wants to hide the button
				//2.  The office wants to hide the button
				//3.  The office is using DentalIntel but not PBN
				picturePracticeByNumbers.Visible=false;
			}
		}

		///<summary>Takes all non-hidden display reports and displays them in their various listboxes.  
		///Hides listboxes that have no display reports.</summary>
		private void FillLists() {
			_listProdInc=DisplayReports.GetForCategory(DisplayReportCategory.ProdInc,false);
			_listMonthly=DisplayReports.GetForCategory(DisplayReportCategory.Monthly,false);
			_listDaily=DisplayReports.GetForCategory(DisplayReportCategory.Daily,false);
			_listList=DisplayReports.GetForCategory(DisplayReportCategory.Lists,false);
			_listPublicHealth=DisplayReports.GetForCategory(DisplayReportCategory.PublicHealth,false);
			_listArizonaPrimary=DisplayReports.GetForCategory(DisplayReportCategory.ArizonaPrimaryCare,false);
			_listReportPermissions=GroupPermissions.GetPermsForReports().Where(x => Security.CurUser.IsInUserGroup(x.UserGroupNum)).ToList();
			//add the items to the list boxes and set the list box heights. (positions too?)
			listProdInc.Items.Clear();
			listDaily.Items.Clear();
			listMonthly.Items.Clear();
			listLists.Items.Clear();
			listPublicHealth.Items.Clear();
			listArizonaPrimaryCare.Items.Clear();
			//listUDSReports.Items.Clear();
			foreach(DisplayReport report in _listProdInc) {
				if(!_listReportPermissions.Exists(x => x.FKey==report.DisplayReportNum)) {
					listProdInc.Items.Add(report.Description+" [Locked]");
				}
				else {
					listProdInc.Items.Add(report.Description);
				}
			}
			if(_listProdInc.Count==0) {
				listProdInc.Visible=false;
				labelProdInc.Visible=false;
			}
			else {
				listProdInc.Visible=true;
				labelProdInc.Visible=true;
				listProdInc.Height=Math.Min((_listProdInc.Count+1) * listProdInc.ItemHeight,listProdInc.Height);
			}
			foreach(DisplayReport report in _listDaily) {
				if(!_listReportPermissions.Exists(x => x.FKey==report.DisplayReportNum)) {
					listDaily.Items.Add(report.Description+" [Locked]");
				}
				else {
					listDaily.Items.Add(report.Description);
				}
			}
			if(_listDaily.Count==0) {
				listDaily.Visible=false;
				labelDaily.Visible=false;
			}
			else {
				listDaily.Visible=true;
				labelDaily.Visible=true;
				listDaily.Height=Math.Min((_listDaily.Count+1) * listDaily.ItemHeight,listDaily.Height);
			}
			foreach(DisplayReport report in _listMonthly) {
				if(!_listReportPermissions.Exists(x => x.FKey==report.DisplayReportNum)) {
					listMonthly.Items.Add(report.Description+" [Locked]");
				}
				else {
					listMonthly.Items.Add(report.Description);
				}
			}
			if(_listMonthly.Count==0) {
				listMonthly.Visible=false;
				labelMonthly.Visible=false;
			}
			else {
				listMonthly.Visible=true;
				labelMonthly.Visible=true;
				listMonthly.Height=Math.Min((_listMonthly.Count+1) * listMonthly.ItemHeight,listMonthly.Height);
			}
			foreach(DisplayReport report in _listList) {
				if(!_listReportPermissions.Exists(x => x.FKey==report.DisplayReportNum)) {
					listLists.Items.Add(report.Description+" [Locked]");
				}
				else {
					listLists.Items.Add(report.Description);
				}
			}
			if(_listList.Count==0) {
				listLists.Visible=false;
				labelLists.Visible=false;
			}
			else {
				listLists.Visible=true;
				labelLists.Visible=true;
				listLists.Height=Math.Min((_listList.Count+1) * listLists.ItemHeight,listLists.Height);
			}
			foreach(DisplayReport report in _listPublicHealth) {
				if(!_listReportPermissions.Exists(x => x.FKey==report.DisplayReportNum)) {
					listPublicHealth.Items.Add(report.Description+" [Locked]");
				}
				else {
					listPublicHealth.Items.Add(report.Description);
				}
			}
			if(_listPublicHealth.Count==0) {
				listPublicHealth.Visible=false;
				labelPublicHealth.Visible=false;
			}
			else {
				listPublicHealth.Visible=true;
				labelPublicHealth.Visible=true;
				listPublicHealth.Height=Math.Min((_listPublicHealth.Count+1) * listPublicHealth.ItemHeight,listPublicHealth.Height);
			}
			//Arizona primary care list and label must only be visible when the Arizona primary
			//care option is checked in the miscellaneous options.
			foreach(DisplayReport report in _listArizonaPrimary) {
				if(!_listReportPermissions.Exists(x => x.FKey==report.DisplayReportNum)) {
					listArizonaPrimaryCare.Items.Add(report.Description+" [Locked]");
				}
				else {
					listArizonaPrimaryCare.Items.Add(report.Description);
				}
			}
			if(_listArizonaPrimary.Count==0 || !UsingArizonaPrimaryCare()) {
				listArizonaPrimaryCare.Visible=false;
				labelArizonaPrimaryCare.Visible=false;
			}
			else {
				listArizonaPrimaryCare.Visible=true;
				labelArizonaPrimaryCare.Visible=true;
				listArizonaPrimaryCare.Height=Math.Min((_listArizonaPrimary.Count+1) * listArizonaPrimaryCare.ItemHeight,49);
			}
		}

		///<summary>Returns true if all of the required patient fields exist which are necessary to run the Arizona Primary Care reports.
		///Otherwise, false is returned.</summary>
		public static bool UsingArizonaPrimaryCare() {
			PatFieldDefs.RefreshCache();
			string[] patientFieldNames=new string[] {
				"SPID#",
				"Eligibility Status",
				"Household Gross Income",
				"Household % of Poverty",
			};
			int[] fieldCounts=new int[patientFieldNames.Length];
			foreach(PatFieldDef pfd in PatFieldDefs.GetDeepCopy(true)) {
				for(int i=0;i<patientFieldNames.Length;i++) {
					if(pfd.FieldName.ToLower()==patientFieldNames[i].ToLower()) {
						fieldCounts[i]++;
						break;
					}
				}
			}
			for(int i=0;i<fieldCounts.Length;i++) {
				//Each field must be defined exactly once. This verifies that each requied field
				//both exists and is not ambiguous with another field of the same name.
				if(fieldCounts[i]!=1) {
					return false;
				}
			}
			return true;
		}

		private void butUserQuery_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.UserQuery)) {
				return;
			}
			if(DataConnection.DBtype==DatabaseType.Oracle) {
				MsgBox.Show(this,"Not allowed while using Oracle.");
				return;
			}
			FormQuery FormQ;
			if(Security.IsAuthorized(Permissions.UserQueryAdmin,true)) {
				FormQ = new FormQuery(null);
				FormQ.ShowDialog();
				SecurityLogs.MakeLogEntry(Permissions.UserQuery,0,"");
			}
			else {
				FormQueryFavorites FormQF = new FormQueryFavorites();
				FormQF.ShowDialog();
				if(FormQF.DialogResult == DialogResult.OK) {
					FormQ=new FormQuery(null,true);
					FormQ.textQuery.Text=FormQF.UserQueryCur.QueryText;
					FormQ.textTitle.Text=FormQF.UserQueryCur.FileName;
					SecurityLogs.MakeLogEntry(Permissions.UserQuery,0,Lan.g(this,"User query form accessed."));
					FormQ.ShowDialog();
				}
			}
		}

		private void butPW_Click(object sender,EventArgs e) {
			try {
				Process.Start("PWReports.exe");
			}
			catch {
				System.Windows.Forms.MessageBox.Show("PracticeWeb Reports module unavailable.");
			}
			SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Practice Web");
		}

		private void listProdInc_MouseDown(object sender,MouseEventArgs e) {
			int selected=listProdInc.IndexFromPoint(e.Location);
			if(selected==-1) {
				return;
			}
			if(!_listReportPermissions.Exists(x => x.FKey==_listProdInc[selected].DisplayReportNum)) {
				MsgBox.Show(this,"You do not have permission to run this report.");
				return;
			}
			if(Security.CurUser.ProvNum==0 && !Security.IsAuthorized(Permissions.ReportProdIncAllProviders,true)) {
				MsgBox.Show(this,"The current user needs to be a provider or have the 'All Providers' permission for this report");
				return;
			}
			OpenReportLocalHelper(_listProdInc[selected],_listReportPermissions,false);
		}

		private void listDaily_MouseDown(object sender,MouseEventArgs e) {
			int selected=listDaily.IndexFromPoint(e.Location);
			if(selected==-1) {
				return;
			}
			OpenReportLocalHelper(_listDaily[selected],_listReportPermissions);
		}

		private void listMonthly_MouseDown(object sender,MouseEventArgs e) {
			int selected=listMonthly.IndexFromPoint(e.Location);
			if(selected==-1) {
				return;
			}
			OpenReportLocalHelper(_listMonthly[selected],_listReportPermissions);
		}

		private void listLists_MouseDown(object sender,MouseEventArgs e) {
			int selected=listLists.IndexFromPoint(e.Location);
			if(selected==-1) {
				return;
			}
			OpenReportLocalHelper(_listList[selected],_listReportPermissions);
		}

		private void listPublicHealth_MouseDown(object sender,MouseEventArgs e) {
			int selected=listPublicHealth.IndexFromPoint(e.Location);
			if(selected==-1) {
				return;
			}
			OpenReportLocalHelper(_listPublicHealth[selected],_listReportPermissions);
		}

		private void listArizonaPrimaryCare_MouseDown(object sender,MouseEventArgs e) {
			int selected=this.listArizonaPrimaryCare.IndexFromPoint(e.Location);
			if(selected==-1) {
				return;
			}
			OpenReportLocalHelper(_listArizonaPrimary[selected],_listReportPermissions,false);
		}
		
		///<summary>Called from this form to do OpenReportHelper(...) logic and then close when needed.</summary>
		private void OpenReportLocalHelper(DisplayReport displayReport,List<GroupPermission> listReportPermissions,bool doValidatePerm=true)
		{
			RpNonModalSelection=OpenReportHelper(displayReport,listReportPermissions,doValidatePerm);
			switch(displayReport.InternalName) {
				//Non-modal report windows are handled after closing.
				case DisplayReports.ReportNames.UnfinalizedInsPay:
				case DisplayReports.ReportNames.OutstandingInsClaims:
				case DisplayReports.ReportNames.ClaimsNotSent:
				case DisplayReports.ReportNames.CustomAging:
				case DisplayReports.ReportNames.TreatmentFinder:
				case DisplayReports.ReportNames.WebSchedAppointments:
				case DisplayReports.ReportNames.ReferredProcTracking:
				case DisplayReports.ReportNames.IncompleteProcNotes:
					Close();
				break;
			}
		}

		///<summary>Handles form and logic for most given displayReports.
		///Returns ReportNonModalSelection.None if modal report is provided.
		///If non-modal report is provided returns valid(not none) RpNonModalSelection to be handled later, See FormOpenDental.SpecialReportSelectionHelper(...)</summary>
		public static ReportNonModalSelection OpenReportHelper(DisplayReport displayReport,List<GroupPermission> listReportPermissions=null,bool doValidatePerm=true)
		{
			if(doValidatePerm) {
				if(listReportPermissions==null) {
					listReportPermissions=GroupPermissions.GetPermsForReports().Where(x => Security.CurUser.IsInUserGroup(x.UserGroupNum)).ToList();
				}
				if(!listReportPermissions.Exists(x => x.FKey==displayReport.DisplayReportNum)) {
					MsgBox.Show("FormReportsMore","You do not have permission to run this report.");
					return ReportNonModalSelection.None;
				}
			}
			FormRpProdInc FormPI;//Used for multiple reports
			switch(displayReport.InternalName) {
				case "ODToday"://Today
					FormPI=new FormRpProdInc();
					FormPI.DailyMonthlyAnnual="Daily";
					FormPI.DateStart=DateTime.Today;
					FormPI.DateEnd=DateTime.Today;
					FormPI.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Production and Income report run for today.");
					break;
				case "ODYesterday"://Yesterday
					FormPI=new FormRpProdInc();
					FormPI.DailyMonthlyAnnual="Daily";
					if(DateTime.Today.DayOfWeek==DayOfWeek.Monday) {
						FormPI.DateStart=DateTime.Today.AddDays(-3);
						FormPI.DateEnd=DateTime.Today.AddDays(-3);
					}
					else {
						FormPI.DateStart=DateTime.Today.AddDays(-1);
						FormPI.DateEnd=DateTime.Today.AddDays(-1);
					}
					FormPI.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Production and Income report run for yesterday.");
					break;
				case "ODThisMonth"://This Month
					FormPI=new FormRpProdInc();
					FormPI.DailyMonthlyAnnual="Monthly";
					FormPI.DateStart=new DateTime(DateTime.Today.Year,DateTime.Today.Month,1);
					FormPI.DateEnd=new DateTime(DateTime.Today.AddMonths(1).Year,DateTime.Today.AddMonths(1).Month,1).AddDays(-1);
					FormPI.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Production and Income report run for this month.");
					break;
				case "ODLastMonth"://Last Month
					FormPI=new FormRpProdInc();
					FormPI.DailyMonthlyAnnual="Monthly";
					FormPI.DateStart=new DateTime(DateTime.Today.AddMonths(-1).Year,DateTime.Today.AddMonths(-1).Month,1);
					FormPI.DateEnd=new DateTime(DateTime.Today.Year,DateTime.Today.Month,1).AddDays(-1);
					FormPI.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Production and Income report run for last month.");
					break;
				case "ODThisYear"://This Year
					FormPI=new FormRpProdInc();
					FormPI.DailyMonthlyAnnual="Annual";
					FormPI.DateStart=new DateTime(DateTime.Today.Year,1,1);
					FormPI.DateEnd=new DateTime(DateTime.Today.Year,12,31);
					FormPI.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Production and Income report run for this year.");
					break;
				case "ODMoreOptions"://More Options
					FormPI=new FormRpProdInc();
					FormPI.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Production and Income report 'more options' accessed.");
					break;
				case "ODProviderPayrollSummary":
					FormRpProviderPayroll FormPP=new FormRpProviderPayroll();
					FormPP.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Provier Payroll Summary report run.");
					break;
				case "ODProviderPayrollDetailed":
					FormRpProviderPayroll FormPPD=new FormRpProviderPayroll(true);
					FormPPD.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Provider Payroll Detailed report run.");
					break;
				case "ODAdjustments"://Adjustments
					if(Security.CurUser.ProvNum==0 && !Security.IsAuthorized(Permissions.ReportDailyAllProviders,true)) {
						MsgBox.Show("FormReportsMore","The current user needs to be a provider or have the 'All Providers' permission for Daily reports");
						break;
					}
					FormRpAdjSheet FormAdjSheet=new FormRpAdjSheet();
					FormAdjSheet.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Adjustments report run.");
					break;
				case "ODPayments"://Payments
					if(Security.CurUser.ProvNum==0 && !Security.IsAuthorized(Permissions.ReportDailyAllProviders,true)) {
						MsgBox.Show("FormReportsMore","The current user needs to be a provider or have the 'All Providers' permission for Daily reports");
						break;
					}
					FormRpPaySheet FormPaySheet=new FormRpPaySheet();
					FormPaySheet.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Daily Payments report run.");
					break;
				case "ODProcedures"://Procedures
					if(Security.CurUser.ProvNum==0 && !Security.IsAuthorized(Permissions.ReportDailyAllProviders,true)) {
						MsgBox.Show("FormReportsMore","The current user needs to be a provider or have the 'All Providers' permission for Daily reports");
						break;
					}
					FormRpProcSheet FormProcSheet=new FormRpProcSheet();
					FormProcSheet.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Daily Procedures report run.");
					break;
				case "ODWriteoffs"://Writeoffs
					if(Security.CurUser.ProvNum==0 && !Security.IsAuthorized(Permissions.ReportDailyAllProviders,true)) {
						MsgBox.Show("FormReportsMore","The current user needs to be a provider or have the 'All Providers' permission for Daily reports");
						break;
					}
					FormRpWriteoffSheet FormW=new FormRpWriteoffSheet();
					FormW.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Daily Writeoffs report run.");
					break;
				case DisplayReports.ReportNames.IncompleteProcNotes://Incomplete Procedure Notes
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Daily Procedure Notes report run.");
					return ReportNonModalSelection.IncompleteProcNotes;
				case "ODRoutingSlips"://Routing Slips
					FormRpRouting FormR=new FormRpRouting();
					FormR.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Routing Slips report run.");
					break;
				case "ODNetProdDetailDaily":
					FormRpNetProdDetail FormNetProdDetail=new FormRpNetProdDetail(true);
					FormNetProdDetail.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Daily Net Prod report run.");
					break;
				case DisplayReports.ReportNames.UnfinalizedInsPay:
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Unfinalized Insurance Payment report run.");
					return ReportNonModalSelection.UnfinalizedInsPay;
				case DisplayReports.ReportNames.PatPortionUncollected:
					FormRpPatPortionUncollected FormPPU=new FormRpPatPortionUncollected();
					FormPPU.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Patient Portion Uncollected report run.");
					break;
				case "ODAgingAR"://Aging of Accounts Receivable Report
					FormRpAging FormA=new FormRpAging();
					FormA.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Aging of A/R report run.");
					break;
				case DisplayReports.ReportNames.ClaimsNotSent://Claims Not Sent
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Claims Not Sent report run.");
					return ReportNonModalSelection.UnsentClaim;
				case "ODCapitation"://Capitation Utilization
					FormRpCapitation FormC=new FormRpCapitation();
					FormC.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Capitation report run.");
					break;
				case "ODFinanceCharge"://Finance Charge Report
					FormRpFinanceCharge FormRpFinance=new FormRpFinanceCharge();
					FormRpFinance.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Finance Charges report run.");
					break;
				case DisplayReports.ReportNames.OutstandingInsClaims://Outstanding Insurance Claims
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Outstanding Insurance Claims report run.");
					return ReportNonModalSelection.OutstandingIns;
				case "ODProcsNotBilled"://Procedures Not Billed to Insurance
					FormRpProcNotBilledIns FormProc=new FormRpProcNotBilledIns();
					FormProc.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Procedures not billed to insurance report run.");
					break;
				case "ODPPOWriteoffs"://PPO Writeoffs
					FormRpPPOwriteoffs FormPPO=new FormRpPPOwriteoffs();
					FormPPO.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"PPO Writeoffs report run.");
					break;
				case "ODPaymentPlans"://Payment Plans
					FormRpPayPlans FormPayPlans=new FormRpPayPlans();
					FormPayPlans.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Payment Plans report run.");
					break;
				case "ODReceivablesBreakdown"://Receivable Breakdown
					FormRpReceivablesBreakdown FormRcv = new FormRpReceivablesBreakdown();
					FormRcv.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Receivable Breakdown report run.");
					break;
				case "ODUnearnedIncome"://Unearned Income
					FormRpUnearnedIncome FormU=new FormRpUnearnedIncome();
					FormU.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Unearned Income report run.");
					break;
				case "ODInsuranceOverpaid"://Insurance Overpaid
					FormRpInsOverpaid FormI=new FormRpInsOverpaid();
					FormI.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Insurance Overpaid report run.");
					break;
				case "ODPresentedTreatmentProd"://Treatment Planned Presenter
					FormRpPresentedTreatmentProduction FormPTP=new FormRpPresentedTreatmentProduction();
					FormPTP.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Treatment Plan Presenter report run.");
					break;
				case "ODTreatmentPresentationStats"://Treatment Planned Presenter
					FormRpTreatPlanPresentationStatistics FormTPS = new FormRpTreatPlanPresentationStatistics();
					FormTPS.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Treatment Plan Presented Procedures report run.");
					break;
				case "ODInsurancePayPlansPastDue"://Insurance Payment Plans
					FormRpInsPayPlansPastDue FormIPP = new FormRpInsPayPlansPastDue();
					FormIPP.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Insurance Payment Plan report run.");
					break;
				case DisplayReports.ReportNames.InsAging://Insurance Aging Report
					FormRpInsAging FormIA=new FormRpInsAging();
					FormIA.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Insurance Aging report run");
					break;
				case DisplayReports.ReportNames.CustomAging://Insurance Aging Report
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Custom Aging report run");
					return ReportNonModalSelection.CustomAging;
				case "ODActivePatients"://Active Patients
					FormRpActivePatients FormAP=new FormRpActivePatients();
					FormAP.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Active Patients report run.");
					break;
				case "ODAppointments"://Appointments
					FormRpAppointments FormAppointments=new FormRpAppointments();
					FormAppointments.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Appointments report run.");
					break;
				case "ODBirthdays"://Birthdays
					FormRpBirthday FormB=new FormRpBirthday();
					FormB.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Birthdays report run.");
					break;
				case "ODBrokenAppointments"://Broken Appointments
					FormRpBrokenAppointments FormBroken=new FormRpBrokenAppointments();
					FormBroken.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Broken Appointments report run.");
					break;
				case "ODInsurancePlans"://Insurance Plans
					FormRpInsCo FormInsCo=new FormRpInsCo();
					FormInsCo.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Insurance Plans report run.");
					break;
				case "ODNewPatients"://New Patients
					FormRpNewPatients FormNewPats=new FormRpNewPatients();
					FormNewPats.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"New Patients report run.");
					break;
				case "ODPatientsRaw"://Patients - Raw
					if(!Security.IsAuthorized(Permissions.UserQuery)) {
						break;
					}
					FormRpPatients FormPatients=new FormRpPatients();
					FormPatients.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Patients - Raw report run.");
					break;
				case "ODPatientNotes"://Patient Notes
					if(!Security.IsAuthorized(Permissions.UserQuery)) {
						break;
					}
					FormSearchPatNotes FormSearchPatNotes=new FormSearchPatNotes();
					FormSearchPatNotes.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Patient Notes report run.");
					break;
				case "ODPrescriptions"://Prescriptions
					FormRpPrescriptions FormPrescript=new FormRpPrescriptions();
					FormPrescript.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Rx report run.");
					break;
				case "ODProcedureCodes"://Procedure Codes
					FormRpProcCodes FormProcCodes=new FormRpProcCodes();
					FormProcCodes.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Procedure Codes report run.");
					break;
				case "ODReferralsRaw"://Referrals - Raw
					if(!Security.IsAuthorized(Permissions.UserQuery)) {
						break;
					}
					FormRpReferrals FormReferral=new FormRpReferrals();
					FormReferral.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Referrals - Raw report run.");
					break;
				case "ODReferralAnalysis"://Referral Analysis
					FormRpReferralAnalysis FormRA=new FormRpReferralAnalysis();
					FormRA.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Referral Analysis report run.");
					break;
				case DisplayReports.ReportNames.ReferredProcTracking://Referred Proc Tracking
					FormReferralProcTrack FormRP=new FormReferralProcTrack();
					FormRP.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"ReferredProcTracking report run.");
					break;
				case DisplayReports.ReportNames.TreatmentFinder://Treatment Finder
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Treatment Finder report run.");
					return ReportNonModalSelection.TreatmentFinder;
				case DisplayReports.ReportNames.WebSchedAppointments://Web Sched Appts
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Web Sched Appointments report run.");
					return ReportNonModalSelection.WebSchedAppointments;
				case "ODRawScreeningData"://Raw Screening Data
					if(!Security.IsAuthorized(Permissions.UserQuery)) {
						break;
					}
					FormRpPHRawScreen FormPH=new FormRpPHRawScreen();
					FormPH.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"PH Raw Screening");
					break;
				case "ODRawPopulationData"://Raw Population Data
					if(!Security.IsAuthorized(Permissions.UserQuery)) {
						break;
					}
					FormRpPHRawPop FormPHR=new FormRpPHRawPop();
					FormPHR.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"PH Raw population");
					break;
				case "ODDentalSealantMeasure"://FQHC Dental Sealant Measure
					FormRpDentalSealantMeasure FormDSM=new FormRpDentalSealantMeasure();
					FormDSM.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"FQHC Dental Sealant Measure report run.");
					break;
				case "ODEligibilityFile"://Eligibility File
					FormRpArizonaPrimaryCareEligibility frapce=new FormRpArizonaPrimaryCareEligibility();
					frapce.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Arizona Primary Care Eligibility");
					break;
				case "ODEncounterFile"://Encounter File
					FormRpArizonaPrimaryCareEncounter frapcn=new FormRpArizonaPrimaryCareEncounter();
					frapcn.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Arizona Primary Care Encounter");
					break;
				case "ODDiscountPlan"://Discount Plans
					FormRpDiscountPlan FormDiscountPlan=new FormRpDiscountPlan();
					FormDiscountPlan.ShowDialog();
					SecurityLogs.MakeLogEntry(Permissions.Reports,0,"Discount Plans report run.");
					break;
				default:
					MsgBox.Show("FormReportsMore","Error finding the report");
					break;
			}
			return ReportNonModalSelection.None;
		}

		private void butLaserLabels_Click(object sender,EventArgs e) {
			FormRpLaserLabels LaserLabels = new FormRpLaserLabels();
			LaserLabels.ShowDialog();
		}

		private void setupToolStripMenuItem_Click(object sender,EventArgs e) {
			if(!Security.IsAuthorized(Permissions.Setup)) {
				return;
			}
			FormReportSetup formRS = new FormReportSetup(0,false); //no need to pass in a usergroupnum.
			if(formRS.ShowDialog()==DialogResult.OK) {
				FillLists();
			}
		}

		private void butPatList_Click(object sender,EventArgs e) {
			FormPatListEHR2014 FormPL=new FormPatListEHR2014();
			FormPL.ShowDialog();
		}

		private void butPatExport_Click(object sender,EventArgs e) {
			FormEhrPatientExport FormEhrPE=new FormEhrPatientExport();
			FormEhrPE.ShowDialog();
		}

		private void pictureDentalIntel_Click(object sender,EventArgs e) {
			DentalIntel.ShowPage();
		}
		
		private void picturePracticeByNumbers_Click(object sender,EventArgs e) {
			PracticeByNumbers.ShowPage();
		}

		private void picturePodium_Click(object sender,EventArgs e) {
			try {
				Podium.ShowPage();
			}
			catch(Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void butClose_Click(object sender,System.EventArgs e) {
			Close();
		}

	}

	///<summary>Used in FormReportsMore to indicate that a non-modal window should be shown.</summary>
	public enum ReportNonModalSelection {
		///<summary></summary>
		None,
		///<summary></summary>
		TreatmentFinder,
		///<summary></summary>
		OutstandingIns,
		///<summary></summary>
		UnfinalizedInsPay,
		///<summary></summary>
		UnsentClaim,
		WebSchedAppointments,
		CustomAging,
		IncompleteProcNotes,
	}
}